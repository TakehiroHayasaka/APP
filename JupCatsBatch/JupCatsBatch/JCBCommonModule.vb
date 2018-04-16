Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices

Module JCBCommonModule

    'ハンドルセット
    Private Declare Function SetParent Lib "user32.dll" (
    ByVal hWndChild As System.IntPtr,
    ByVal hWndNewParent As System.IntPtr) As System.IntPtr

    '指定された文字列と一致するクラス名とウィンドウ名文字列を持つウィンドウのハンドルを返します。この関数は、子ウィンドウを検索します。
    'この関数による検索は、指定された子ウィンドウの直後の子ウィンドウから開始されます。大文字小文字は区別されません。
    'HWND FindWindowEx(
    '   HWND hwndParent,     // 親ウィンドウのハンドル
    '   HWND hwndChildAfter, // 子ウィンドウのハンドル
    '   LPCTSTR lpszClass,   // クラス名
    '   LPCTSTR lpszWindow   // ウィンドウ名
    ')
    Declare Function FindWindowEx Lib "user32.dll" Alias "FindWindowExA" _
    (ByVal hwndParent As Integer, ByVal hwndChildAfter As Integer,
    ByVal lpszClass As String, ByVal lpszWindow As String) As Integer

    '---------------------------------------------------------------------------------------------------
    '機能
    '   EXEを起動する。
    '引数
    '   exeName：EXEファイル名
    '   arg：プログラム引数
    '   ob：JCBMainWindowのオブジェクト
    '   wait：0=プロセス終了まで待機、1=待機しない
    '戻り値
    '   無し
    '---------------------------------------------------------------------------------------------------
    Function ExeStart(kojiFolder As String, exeName As String, arg As String, ByVal ob As Object, Optional wait As Integer = 0) As Integer

        ' ProcessStartInfo の新しいインスタンスを生成する
        Dim process As New System.Diagnostics.ProcessStartInfo()
        ' 起動するアプリケーションを設定する
        process.FileName = exeName
        ' コマンドライン引数を設定する
        If arg <> "" Then
            process.Arguments = arg
        End If
        '新しいウィンドウを作成するかどうかを設定する(初期値 False)
        process.CreateNoWindow = True
        'シェルを使用するかどうか設定する(初期値 True)
        process.UseShellExecute = False
        ' 起動できなかった時にエラーダイアログを表示するかどうかを設定する (初期値 False)
        process.ErrorDialog = True
        ' アプリケーションを起動する時の動詞を設定する
        process.Verb = "Open"
        ' 起動ディレクトリを設定する
        process.WorkingDirectory = kojiFolder
        ' 起動時のウィンドウの状態を設定する
        process.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal '通常

        ' ProcessStartInfo を指定して起動する
        Dim pr As System.Diagnostics.Process = System.Diagnostics.Process.Start(process)

        'サブウィンドウが開いている際のみMDIを実装
        Dim main As JCBMainWindow = ob
        If main._subWinFlag = 1 Then
            'プロセスが終了していない場合MDIを実装
            If pr.HasExited = False Then
                Try
                    pr.WaitForInputIdle()
                    SetParent(pr.MainWindowHandle, main.PanelSubWindowMain.Handle)
                Catch ex As Exception
                    Return 0
                End Try

            End If
        End If

        'プロセスが終了するまで待機する。
        If wait = 1 Then
            pr.WaitForExit()
        End If

        Return 0

    End Function

    '---------------------------------------------------------------------------------------------------
    '機能
    '   プログラム(EXE)を起動する。(プログラムの検索順序＝工事フォルダ→(GENSUN_DIR)\Bin)
    '引数
    '   exeName：EXEファイル名
    '   arg：プログラム引数
    '   mainOb：JCBMainWindowのオブジェクト
    '戻り値
    '   0：正常終了
    '   1：exeが存在しない
    '---------------------------------------------------------------------------------------------------
    Function ExeStartJupiter(kojiFolder As String, exeName As String, arg As String, ByVal mainOb As JCBMainWindow) As Integer
        'Jupiterのプログラムを起動する。

        'プログラムのファイル名を作成する。
        If System.IO.File.Exists(exeName) = False Then
            '工事フォルダ内にEXEファイルが存在しない。
            exeName = FunGetBin(mainOb._flagJC) + exeName
            If System.IO.File.Exists(exeName) = False Then
                Return 1
            End If
        End If

        ExeStart(kojiFolder, exeName, arg, mainOb, 1)

        Return 0

    End Function

    '---------------------------------------------------------------------------------------------------
    '機能
    '   Profile.iniを読み込み、橋梁形式,チャージコード,チャージコード登録位置を取得
    '引数
    '   profileFileName：ファイル名
    '   kozo：橋梁形式
    '   chargeCode：チャージコード
    '   recno：チャージコード登録位置
    '戻り値
    '   橋梁形式番号(0:RC鈑桁、1:RC箱桁、2:鋼床版箱桁、3:合成床版、4:鋼製橋脚、5:波形ウェブ、6:鋼製セグメント
    '   エラーの場合は-1
    '---------------------------------------------------------------------------------------------------
    Function ReadProfileIni(profileFileName As String, ByRef kozo As String, ByRef chargeCode As String, ByRef recno As Integer) As Integer

        ReadProfileIni = -1

        Dim findIndex As Integer = 0
        ' Profile.iniを読み込み、橋梁形式とチャージコードを取得
        If System.IO.File.Exists(profileFileName) = True Then
            Dim profileIni As New StreamReader(profileFileName, Encoding.GetEncoding("Shift_JIS"))
            While (profileIni.Peek() >= 0)
                Dim lineText As String = profileIni.ReadLine().Trim()
                If lineText Like "主桁断面=*" Then
                    Dim kozoArray As String() = lineText.Split("=")
                    kozo = kozoArray(kozoArray.Count() - 1)
                    findIndex = findIndex + 1
                ElseIf lineText Like "チャージコード=*" Then
                    Dim chargeCodeArray As String() = lineText.Split("=")
                    chargeCode = chargeCodeArray(chargeCodeArray.Count() - 1)
                    If chargeCode = "FREE-JUPITER" Then
                        chargeCode = ""
                    End If
                    findIndex = findIndex + 1
                ElseIf lineText Like "登録ファイルレコード位置=*" Then
                    Dim recnoArray As String() = lineText.Split("=")
                    recno = Integer.Parse(recnoArray(recnoArray.Count() - 1))
                    findIndex = findIndex + 1
                End If
            End While
            profileIni.Close()
        Else
            Exit Function
        End If

        ReadProfileIni = GetKozoNo(kozo)

    End Function

    Function GetKozo(kozoNo As Integer) As String

        GetKozo = ""

        If kozoNo = 0 Then
            GetKozo = "RC鈑桁"
        ElseIf kozoNo = 1 Then
            GetKozo = "RC箱桁"
        ElseIf kozoNo = 2 Then
            GetKozo = "鋼床版箱桁"
        ElseIf kozoNo = 3 Then
            GetKozo = "合成床版"
        ElseIf kozoNo = 4 Then
            GetKozo = "鋼製橋脚"
        ElseIf kozoNo = 5 Then
            GetKozo = "波形ウェブ"
        ElseIf kozoNo = 6 Then
            GetKozo = "鋼製セグメント"
        End If

    End Function

    Function GetKozoNo(kozo As String) As Integer

        GetKozoNo = -1

        If kozo = "RC鈑桁" Then
            GetKozoNo = 0
        ElseIf kozo = "RC箱桁" Then
            GetKozoNo = 1
        ElseIf kozo = "鋼床版箱桁" Then
            GetKozoNo = 2
        ElseIf kozo = "合成床版" Then
            GetKozoNo = 3
        ElseIf kozo = "鋼製橋脚" Then
            GetKozoNo = 4
        ElseIf kozo = "波形ウェブ" Then
            GetKozoNo = 5
        ElseIf kozo = "鋼製セグメント" Then
            GetKozoNo = 6
        End If

    End Function

    Public Sub RemoveComment(ByRef text As String)

        Dim TextTmp As String = text

        Dim tIdx1 As Integer = text.IndexOf(";")
        Dim tIdx2 As Integer = text.IndexOf("//")
        Dim tIdx3 As Integer = text.IndexOf("/*")

        If tIdx1 = -1 And tIdx2 = -1 And tIdx3 = -1 Then
            Return
        End If

        Dim tIdxList As New ArrayList
        tIdxList.Add(tIdx1)
        tIdxList.Add(tIdx2)
        tIdxList.Add(tIdx3)
        tIdxList.Sort()

        Dim tIdx As Integer = 0
        Dim i As Integer = 0
        For i = 0 To tIdxList.Count - 1
            If tIdxList(i) <> -1 Then
                tIdx = tIdxList(i)
                Exit For
            End If
        Next

        text = TextTmp.Remove(tIdx)

    End Sub
    '---------------------------------------------------------------------------------------------------
    '機能
    '   テキストから指定した文字以降の文字を抜き出す。
    '引数
    '   text：(I)テキスト
    '   delim：(I)指定文字
    '戻り値
    '   0：正常終了
    '   1：異常終了
    '---------------------------------------------------------------------------------------------------
    Public Function SubStringEnd(text As String, delim As String) As String

        SubStringEnd = text

        Dim index As Integer = text.LastIndexOf(delim)
        If index >= 0 And index <> text.Count - 1 Then
            SubStringEnd = text.Substring(index + 1)
        End If

    End Function

    '---------------------------------------------------------------------------------------------------
    '機能
    '   System.txtに工事フォルダ情報を書込む
    '引数
    '   SysFile(I):System.txtのパス
    '   kojiFolder(I):工号
    '戻り値
    '   無し
    '---------------------------------------------------------------------------------------------------
    Sub WriteSystemTxt(sysFile As String, kojiFolder As String)

        Dim bytes1() As Byte = System.BitConverter.GetBytes(kojiFolder.Count())
        Dim bytes2() As Byte = System.Text.Encoding.GetEncoding(932).GetBytes(kojiFolder)
        Dim bytes3() As Byte = System.BitConverter.GetBytes(kojiFolder.Count() / 2)

        Dim fs As New System.IO.FileStream(sysFile, System.IO.FileMode.Create, System.IO.FileAccess.Write)
        fs.Write(bytes1, 0, bytes1.Length)
        fs.Write(bytes2, 0, bytes2.Length)
        fs.Write(bytes3, 0, bytes3.Length)
        fs.Close()

        'Open SysFile For Binary Access Write As #fnum
        '    Put #fnum, , Len(cdir)
        '    Put #fnum, , cdir
        '    Put #fnum, , Len(cdir) / 2
        'Close #fnum

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   Environフォルダ内の設定ファイルを工事フォルダにコピーし、フルパスのファイル名を返す
    '引数
    '   kojiFolder：(I)工事フォルダ
    '   env：(I)Environフォルダ
    '   fileName：(I)ファイル名(パスなし)
    '戻り値
    '   フルパスのファイル名
    '---------------------------------------------------------------------------------------------------
    Public Function CopySettingFileFromEnviron(kojiFolder As String, env As String, fileName As String, Optional copyFileName As String = "") As String

        CopySettingFileFromEnviron = ""

        Dim kojiFileName As String = kojiFolder.TrimEnd("\") + "\" + fileName
        Dim envFileName As String = ""
        If copyFileName = "" Then
            envFileName = env.TrimEnd("\") + "\" + fileName
        Else
            envFileName = env.TrimEnd("\") + "\" + copyFileName
        End If

        If Not System.IO.File.Exists(kojiFileName) Then
            If System.IO.File.Exists(envFileName) Then
                System.IO.File.Copy(envFileName, kojiFileName)
            End If
        End If

        CopySettingFileFromEnviron = kojiFileName

    End Function

    '---------------------------------------------------------------------------------------------------
    '機能
    '   System.txtに空白を書込む(JupiterManagerを閉じたときの処理)
    '引数
    '   SysFile(I):System.txtのパス
    '戻り値
    '   無し
    '---------------------------------------------------------------------------------------------------
    Sub WriteSystemTxtClose(sysFile As String)

        Dim str As String = " "

        Dim bytes() As Byte = System.Text.Encoding.GetEncoding(932).GetBytes(str)

        Dim fs As New System.IO.FileStream(sysFile, System.IO.FileMode.Create, System.IO.FileAccess.Write)
        fs.Write(bytes, 0, bytes.Length)
        fs.Close()

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   引数を組み合わせてファイルパスを作る
    '引数
    '   kojiFolder：工事フォルダ
    '   dataFolder：データフォルダー
    '   fileName：ファイル名
    '戻り値
    '   無し
    '---------------------------------------------------------------------------------------------------
    Public Function CreateFilePath(kojiFolder As String, dataFolder As String, fileName As String) As String
        Dim filePath As String = ""

        If dataFolder = "" Then
            filePath = kojiFolder + fileName
        Else
            filePath = kojiFolder + dataFolder + "\" + fileName
        End If

        Return filePath
    End Function

    '---------------------------------------------------------------------------------------------------
    '機能
    '   引数が数値かどうか判定する
    '引数
    '   arg：数値
    '戻り値
    '   True or False
    '---------------------------------------------------------------------------------------------------
    Public Function TryParseToInteger(ByVal arg As String) As Boolean
        Try
            Integer.Parse(arg)
        Catch ex As StackOverflowException
            Return False
        Catch ex As OutOfMemoryException
            Return False
        Catch ex As System.Threading.ThreadAbortException
            Return False
        Catch
            Return False
        End Try

        Return True
    End Function

    '---------------------------------------------------------------------------------------------------
    ' 指定したウィンドウの画面にあるプログラムを取得
    ' 引数：取得したいウィンドウのハンドル
    ' 戻り値：プログラムのリスト(タイトル(str)、ハンドル(int))
    '---------------------------------------------------------------------------------------------------
    Public Function GetDisplayProgram(handle As Integer) As ArrayList
        Dim j As Long
        Dim list As New ArrayList
        Dim bytClass As Byte()
        Dim bytTitle As Byte()

        ' ウィンドウとコントロールの全ての情報を取得
        Dim colWindows As Collection
        colWindows = GetAllWindows(handle)

        ' 子コントロール毎のコレクション取得
        Dim colChilds As Collection
        colChilds = colWindows.Item(1)

        If colChilds.Count > 1 Then         ' 子コントロールを持つ物のみ対象
            ' 子コントロール毎のコレクションループ
            For j = 1 To colChilds.Count
                ' コレクションからクラス名取得
                bytClass = System.Text.Encoding.GetEncoding(
                        "SHIFT-JIS").GetBytes(colChilds.Item(j)(1))
                Dim strClass As String = System.Text.Encoding.GetEncoding(
                        "SHIFT-JIS").GetString(bytClass)

                ' コレクションから文字列取得
                bytTitle = System.Text.Encoding.GetEncoding(
                        "SHIFT-JIS").GetBytes(colChilds.Item(j)(2))

                Dim strTitle As String = System.Text.Encoding.GetEncoding(
                                        "SHIFT-JIS").GetString(bytTitle)

                Dim hwnd As Integer
                hwnd = FindWindowEx(handle, 0, strClass, strTitle)

                'ハンドルが存在する際に配列に格納
                If hwnd <> 0 Then
                    Dim ob(1) As Object
                    ob(0) = bytTitle
                    ob(1) = hwnd
                    list.Add(ob)
                End If

            Next j
        End If

        Return list
    End Function

    '---------------------------------------------------------------------------------------------------
    ' 指定したウィンドウの画面に指定したプログラムがあるか
    ' 引数：取得したいウィンドウのハンドル、プログラム
    ' 戻り値： exit:1, no:0
    '---------------------------------------------------------------------------------------------------
    Public Function SearchProgram(handle As Integer, programName As String) As Integer
        Dim list As ArrayList = GetDisplayProgram(handle)
        Dim count As Integer = list.Count
        For i As Integer = 0 To count - 1
            Dim name As String = System.Text.Encoding.GetEncoding("SHIFT-JIS").GetString(list(i)(0))
            If name = programName Then
                Return 1
            End If
        Next
        Return 0
    End Function

End Module
