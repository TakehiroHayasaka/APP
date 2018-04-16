Module JCBGetControls
    ' APIの定義
    Private Const WM_GETTEXT = &HD
    Private Delegate Function D_EnumWindowsProc(
        ByVal hWnd As Integer, ByVal lParam As Integer) As Integer
    Private Delegate Function D_EnumChildWindowsProc(
        ByVal hWnd As Integer, ByVal lParam As Integer) As Integer
    Private Declare Function EnumWindows Lib "user32.dll" _
        (ByVal lpEnumFunc As D_EnumWindowsProc, ByVal lParam As Integer) As Integer
    Private Declare Function EnumChildWindows Lib "user32.dll" _
        (ByVal hwndParent As Integer, ByVal lpEnumFunc As D_EnumChildWindowsProc,
         ByVal lParam As Integer) As Integer
    Private Declare Function GetClassName Lib "user32.dll" Alias "GetClassNameA" _
        (ByVal hWnd As Integer, ByVal lpClassName As Byte(),
         ByVal nMaxCount As Integer) As Integer
    Private Declare Function SendMessage Lib "user32.dll" Alias "SendMessageA" _
        (ByVal hWnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer,
         ByVal lParam As Byte()) As Integer

    ' コントロール毎の情報を設定するコレクション
    Private colWindows As New Collection

    Public _handle As Integer

    ' ウィンドウとコントロールを全て取得
    Public Function GetAllWindows(argHnadle As Integer) As Collection
        _handle = argHnadle
        On Error Resume Next
        Dim i As Long
        Dim lngRet As Long

        ' コントロール毎の情報を設定するコレクション生成
        colWindows = New Collection
        GetAllWindows = colWindows

        ' トップレベルウィンドウを全て取得
        lngRet = EnumWindows(AddressOf EnumWindowsProc, 0)

        GetAllWindows = colWindows

        ' 親ウィンドウに属するコントロールを全て取得
        lngRet = EnumChildWindows(colWindows.Item(1).Item(1)(0), AddressOf EnumChildWindowsProc, 1)

    End Function

    ' トップレベルウィンドウを全て取得
    Public Function EnumWindowsProc(
        ByVal hWnd As Integer, ByVal lParam As Integer) As Integer
        On Error Resume Next
        Dim lngRet As Long
        Dim bytClass As Byte() = New Byte(255) {}
        Dim bytTitle As Byte() = New Byte(255) {}
        Dim strClass As String
        Dim strTitle As String

        ' クラス名取得
        lngRet = GetClassName(_handle, bytClass, 255)
        strClass = StripNulls(bytClass)

        ' ウィンドウのタイトル取得
        lngRet = SendMessage(_handle, WM_GETTEXT, 255, bytTitle)
        strTitle = StripNulls(bytTitle)

        ' 取得した情報を配列に設定
        Dim strDa(2) As Object
        strDa(0) = _handle
        strDa(1) = strClass
        strDa(2) = strTitle

        ' 取得した情報をコレクションに設定
        Dim colDa As New Collection From {
            strDa
        }

        ' トップレベルウィンドウ毎のコレクションに追加
        colWindows.Add(colDa)


        ' リターン
        EnumWindowsProc = 0
    End Function

    ' 指定された親ウィンドウに属するコントロールを全て取得
    Private Function EnumChildWindowsProc(
        ByVal hWnd As Integer, ByVal lParam As Integer) As Integer
        On Error Resume Next
        Dim lngRet As Long
        Dim bytClass As Byte() = New Byte(255) {}
        Dim bytTitle As Byte() = New Byte(255) {}
        Dim strClass As String
        Dim strTitle As String

        ' クラス名取得
        lngRet = GetClassName(hWnd, bytClass, 255)
        strClass = StripNulls(bytClass)

        ' ウィンドウのタイトル取得
        lngRet = SendMessage(hWnd, WM_GETTEXT, 255, bytTitle)
        strTitle = StripNulls(bytTitle)

        ' 取得した情報を配列に設定
        Dim strDa(2) As Object
        strDa(0) = hWnd
        strDa(1) = strClass
        strDa(2) = strTitle

        ' コントロール毎のコレクションに追加
        colWindows.Item(lParam).Add(strDa)

        ' リターン
        EnumChildWindowsProc = 1
    End Function

    ' 文字列からＮＵＬＬ文字以降をカット
    Private Function StripNulls(ByVal bytOrg As Byte()) As String
        On Error Resume Next
        Dim strOrg As String =
            System.Text.Encoding.GetEncoding("SHIFT-JIS").GetString(bytOrg)
        If (InStr(strOrg, Chr(0)) > 0) Then
            strOrg = Left(strOrg, InStr(strOrg, Chr(0)) - 1)
        End If
        StripNulls = strOrg
    End Function

End Module