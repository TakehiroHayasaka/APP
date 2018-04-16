Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

'----------------ボタン詳細クラス----------------
Public Class JManDispButtonDetail

    '----------------プロパティ----------------
    Public _type As Integer = 0                    'ボタンタイプ(1～6)
    Public _textHeigthRatio As Double = 1.0        'キャプション文字高比率
    Public _caption As String = ""                 'ボタンキャプション
    Public _fileName As String = ""                '実行ファイル・設定ファイル名
    Public _argument As String = ""                '実行ファイルの引数

    Public Function initialize(text As String) As Integer
        initialize = 0

        Dim status As Integer = 0
        '連続している空白を一つにする。
        Dim re As Regex
        re = New Regex("\s+") '空白文字の連続で区切られる

        Dim texts As String() = re.Split(text)
        If texts.Count < 4 Then
            initialize = 1
            Exit Function
        End If

        ' _type
        If Integer.TryParse(texts(0), _type) <> True Then
            initialize = 1
            Exit Function
        End If
        If _type <= 0 Or _type > 6 Then
            initialize = 1
            Exit Function
        End If

        ' _textHeigthRatio
        If Double.TryParse(texts(1), _textHeigthRatio) <> True Then
            initialize = 1
            Exit Function
        End If
        If _textHeigthRatio <= 0.0 Then
            initialize = 1
            Exit Function
        End If

        ' _caption
        status = createCaption(texts(2), _caption)
        If status <> 0 Then
            initialize = 1
            Exit Function
        End If

        ' _fileName
        _fileName = texts(3)

        ' _argument
        _argument = ""
        If texts.Count >= 5 Then
            Dim i As Integer = 0
            For i = 4 To texts.Count - 1
                _argument += texts(i)
                If i <> texts.Count - 1 Then
                    _argument += " "
                End If
            Next
        End If

    End Function

End Class

'----------------ボタンクラス----------------
Public Class JManDispButton

    '----------------プロパティ----------------
    Public _rowNo As Integer = 0                   '行番号
    Public _columnNo As Integer = 0                '列番号
    Public _buttonNum As Integer = 0               'ボタン数
    Public _caption As String = ""                 'ボタンキャプション
    Public _detailList As New ArrayList                'ボタン詳細リスト


    Public Function initialize(textList As ArrayList, ByRef idx As Integer) As Integer
        initialize = 0

        Dim status As Integer = 0
        '連続している空白を一つにする。
        Dim re As Regex
        re = New Regex("\s+") '空白文字の連続で区切られる

        '------ 1行目 ------
        Dim texts1 As String() = re.Split(textList(idx))
        If texts1.Count <> 4 Then
            initialize = 1
            Exit Function
        End If

        ' _rowNo,_columnNo
        status = getRowColumnNo(texts1(1), _rowNo, _columnNo)
        If status <> 0 Then
            initialize = 1
            Exit Function
        End If
        If _rowNo <= 0 Or _columnNo <= 0 Then
            initialize = 1
            Exit Function
        End If

        ' _buttonNum
        If Integer.TryParse(texts1(2), _buttonNum) <> True Then
            initialize = 1
            Exit Function
        End If
        If _buttonNum <= 0 Then
            initialize = 1
            Exit Function
        End If

        ' _caption
        status = createCaption(texts1(3), _caption)
        If status <> 0 Then
            initialize = 1
            Exit Function
        End If

        idx = idx + 1

        '------ 2行目以降 ------
        Dim i As Integer = 0
        For i = 0 To _buttonNum - 1
            Dim buttonDetail As New JManDispButtonDetail
            status = buttonDetail.initialize(textList(idx))
            If status <> 0 Then
                initialize = 1
                Exit Function
            End If
            _detailList.Add(buttonDetail)
            If i <> _buttonNum - 1 Then
                idx = idx + 1
            End If
        Next

    End Function

End Class

'----------------グループボックスクラス----------------
Public Class JManDispGroupBox

    '----------------プロパティ----------------
    Public _rowNo As Integer = 0                   '行番号
    Public _columnNo As Integer = 0                '列番号
    Public _height As Integer = 0                  'グループボックス高さ(行数を指定)
    Public _width As Integer = 0                   'グループボックス幅(列数を指定)
    Public _caption As String = ""                 'グループボックスのキャプション

    Public Function initialize(text As String) As Integer
        initialize = 0

        Dim status As Integer = 0
        '連続している空白を一つにする。
        Dim re As Regex
        re = New Regex("\s+") '空白文字の連続で区切られる

        Dim texts As String() = re.Split(text)
        If texts.Count <> 5 Then
            initialize = 1
            Exit Function
        End If

        ' _rowNo,_columnNo
        status = getRowColumnNo(texts(1), _rowNo, _columnNo)
        If status <> 0 Then
            initialize = 1
            Exit Function
        End If
        If _rowNo <= 0 Or _columnNo <= 0 Then
            initialize = 1
            Exit Function
        End If

        ' _height
        If Integer.TryParse(texts(2), _height) <> True Then
            initialize = 1
            Exit Function
        End If
        If _height <= 0 Then
            initialize = 1
            Exit Function
        End If

        ' _width
        If Integer.TryParse(texts(3), _width) <> True Then
            initialize = 1
            Exit Function
        End If
        If _width <= 0 Then
            initialize = 1
            Exit Function
        End If

        ' _caption
        status = createCaption(texts(4), _caption)
        If status <> 0 Then
            initialize = 1
            Exit Function
        End If

    End Function

End Class

'----------------テキストクラス----------------
Public Class JManDispText

    '----------------プロパティ----------------
    Public _rowNo As Integer = 0                   '行番号
    Public _columnNo As Integer = 0                '列番号
    Public _width As Integer = 0                   'グループボックス幅(列数を指定)
    Public _caption As String = ""                 'グループボックスのキャプション

    Public Function initialize(text As String) As Integer
        initialize = 0

        Dim status As Integer = 0
        '連続している空白を一つにする。
        Dim re As Regex
        re = New Regex("\s+") '空白文字の連続で区切られる

        Dim texts As String() = re.Split(text)
        If texts.Count <> 4 Then
            initialize = 1
            Exit Function
        End If

        ' _rowNo,_columnNo
        status = getRowColumnNo(texts(1), _rowNo, _columnNo)
        If status <> 0 Then
            initialize = 1
            Exit Function
        End If
        If _rowNo <= 0 Or _columnNo <= 0 Then
            initialize = 1
            Exit Function
        End If

        ' _height
        If Integer.TryParse(texts(2), _width) <> True Then
            initialize = 1
            Exit Function
        End If
        If _width <= 0 Then
            initialize = 1
            Exit Function
        End If

        ' _caption
        status = createCaption(texts(3), _caption)
        If status <> 0 Then
            initialize = 1
            Exit Function
        End If

    End Function

End Class

'----------------タブクラス----------------
Public Class JManDispTab

    '----------------プロパティ----------------
    Public _no As Integer = 0                      'タブ番号
    Public _type As String = ""                    'タブのタイプ
    Public _caption As String = ""                 'グループボックスのキャプション
    Public _buttonList As New ArrayList                'ボタンリスト
    Public _groupBoxList As New ArrayList              'グループボックスリスト
    Public _textList As New ArrayList                  'テキストリスト

    Public Function initialize(tabText As String) As Integer
        initialize = 0

        Dim status As Integer = 0

        '連続している空白を一つにする。
        Dim re As Regex
        re = New Regex("\s+") '空白文字の連続で区切られる

        Dim tabTexts As String() = re.Split(tabText)
        If tabTexts.Count <> 4 Then
            initialize = 1
            Exit Function
        End If

        ' _no
        If Integer.TryParse(tabTexts(1), _no) <> True Then
            initialize = 1
            Exit Function
        End If

        ' _type
        _type = tabTexts(2)
        'If Not (_type Like "SKL,BZI_I,BZI_B,BZI_D,BZI_G,BZI_P,BZI_N,BZI_S,SEISAN,REP1,REP2") Then
        '    initialize = 1
        '    Exit Function
        'End If

        ' _caption
        status = createCaption(tabTexts(3), _caption)
        If status <> 0 Then
            initialize = 1
            Exit Function
        End If

    End Function

End Class

'----------------タブクラス----------------
Public Class JManDispWindow

    '----------------プロパティ----------------
    Public _height As Integer = 0                      'ウインドウの高さ
    Public _width As Integer = 0                       'ウインドウの幅
    Public _caption As String = ""                     'ウィンドウのキャプション
    Public _buttonList As New ArrayList                'ボタンリスト
    Public _groupBoxList As New ArrayList              'グループボックスリスト
    Public _textList As New ArrayList                  'テキストリスト

    Public Function initialize(windowText As String) As Integer
        initialize = 0

        Dim status As Integer = 0

        '連続している空白を一つにする。
        Dim re As Regex
        re = New Regex("\s+") '空白文字の連続で区切られる

        Dim tabTexts As String() = re.Split(windowText)
        If tabTexts.Count <> 4 Then
            initialize = 1
            Exit Function
        End If

        ' _height
        If Integer.TryParse(tabTexts(1), _height) <> True Then
            initialize = 1
            Exit Function
        End If

        ' _width
        If Integer.TryParse(tabTexts(2), _width) <> True Then
            initialize = 1
            Exit Function
        End If

        ' _caption
        status = createCaption(tabTexts(3), _caption)
        If status <> 0 Then
            initialize = 1
            Exit Function
        End If

    End Function

    Public Sub clearList()
        _height = 0
        _width = 0
        _caption = ""
        _buttonList.Clear()
        _groupBoxList.Clear()
        _textList.Clear()
    End Sub

End Class

'----------------ファイルクラス----------------
Public Class JManDispFile

    '----------------プロパティ----------------
    Public _dspFileName As String = ""             'ファイル名
    Public _tabList As New ArrayList               'タブリスト
    Public _window As New JManDispWindow()         'ウインドウ

    Public Function loadFile(kozoIdx As Integer) As Integer

        loadFile = 0

        Me.setDspFileName(kozoIdx)

        Dim textList As New ArrayList
        Dim isTabDefine As Boolean = False

        If System.IO.File.Exists(_dspFileName) = True Then
            Dim dspFile As New StreamReader(_dspFileName, Encoding.GetEncoding("Shift_JIS"))
            While (dspFile.Peek() >= 0)
                Dim lineText As String = dspFile.ReadLine().Trim()
                removeComment(lineText)
                If lineText = "" Then
                    Continue While
                End If
                If lineText Like "\TAB*" Then
                    textList.Clear()
                    isTabDefine = True
                ElseIf lineText Like "}*" Then
                    isTabDefine = False
                End If

                If isTabDefine = True And Not (lineText Like "{*") Then
                    textList.Add(lineText)
                End If

                If isTabDefine = False And textList.Count <> 0 Then
                    Dim status As Integer = Me.loadTab(textList)
                    If status <> 0 Then
                        loadFile = 1
                        Exit Function
                    End If
                End If

            End While
            dspFile.Close()
        Else
            loadFile = 1
        End If

    End Function

    Private Function setDspFileName(kozoIdx As Integer) As Integer

        setDspFileName = 0

        Dim folder As String = FunGetGensun() + "Environ\"

        Select Case kozoIdx
            Case 0
                _dspFileName = folder + "JmanDisp_I.tbs"
            Case 1
                _dspFileName = folder + "JmanDisp_BOX.tbs"
            Case 2
                _dspFileName = folder + "JmanDisp_DECK.tbs"
            Case 3
                _dspFileName = folder + "JmanDisp_GDK.tbs"
            Case 4
                _dspFileName = folder + "JmanDisp_PIER.tbs"
            Case 5
                _dspFileName = folder + "JmanDisp_NAMI.tbs"
            Case 6
                _dspFileName = folder + "JmanDisp_SEG.tbs"
            Case 100
                _dspFileName = folder + "JmanDispSub_Seisan.wbs"
            Case Else
                _dspFileName = ""
                setDspFileName = 1
        End Select

    End Function

    Private Function loadTab(textList As ArrayList) As Integer
        loadTab = 0

        Dim status As Integer = 0

        ' \TAB行読込
        Dim tab As New JManDispTab
        status = tab.initialize(textList(0))
        If status <> 0 Then
            loadTab = 1
            Exit Function
        End If

        Dim i As Integer = 0
        For i = 1 To textList.Count - 1
            If textList(i) Like "\GROUP*" Then
                Dim group As New JManDispGroupBox
                status = group.initialize(textList(i))
                If status <> 0 Then
                    loadTab = 1
                    Exit Function
                End If
                tab._groupBoxList.Add(group)
            ElseIf textList(i) Like "\BUTTON*" Then
                Dim button As New JManDispButton
                status = button.initialize(textList, i)
                If status <> 0 Then
                    loadTab = 1
                    Exit Function
                End If
                tab._buttonList.Add(button)
            ElseIf textList(i) Like "\TEXT*" Then
                Dim text As New JManDispText
                status = text.initialize(textList(i))
                If status <> 0 Then
                    loadTab = 1
                    Exit Function
                End If
                tab._textList.Add(text)
            End If
        Next

        _tabList.Add(tab)

    End Function

    Private Function loadWindow(textList As ArrayList) As Integer
        loadWindow = 0

        Dim status As Integer = 0

        ' \WINDOW行読込
        status = Me._window.initialize(textList(0))
        If status <> 0 Then
            loadWindow = 1
            Exit Function
        End If

        Dim i As Integer = 0
        For i = 1 To textList.Count - 1
            If textList(i) Like "\GROUP*" Then
                Dim group As New JManDispGroupBox
                status = group.initialize(textList(i))
                If status <> 0 Then
                    loadWindow = 1
                    Exit Function
                End If
                Me._window._groupBoxList.Add(group)
            ElseIf textList(i) Like "\BUTTON*" Then
                Dim button As New JManDispButton
                status = button.initialize(textList, i)
                If status <> 0 Then
                    loadWindow = 1
                    Exit Function
                End If
                Me._window._buttonList.Add(button)
            ElseIf textList(i) Like "\TEXT*" Then
                Dim text As New JManDispText
                status = text.initialize(textList(i))
                If status <> 0 Then
                    loadWindow = 1
                    Exit Function
                End If
                Me._window._textList.Add(text)
            End If
        Next

    End Function

    Public Function loadSeisanSubFile() As Integer

        loadSeisanSubFile = 0

        Me.setDspFileName(100)
        Me._window.clearList()

        Dim textList As New ArrayList
        Dim isWindowDefine As Boolean = False
        Dim windowNum As Integer = 0

        If System.IO.File.Exists(_dspFileName) = True Then
            Dim dspFile As New StreamReader(_dspFileName, Encoding.GetEncoding("Shift_JIS"))
            While (dspFile.Peek() >= 0)
                Dim lineText As String = dspFile.ReadLine().Trim()
                removeComment(lineText)
                If lineText = "" Then
                    Continue While
                End If
                If lineText Like "\WINDOW*" Then
                    textList.Clear()
                    isWindowDefine = True
                    windowNum = windowNum + 1
                ElseIf lineText Like "}*" Then
                    isWindowDefine = False
                End If

                If isWindowDefine = True And Not (lineText Like "{*") Then
                    textList.Add(lineText)
                End If

                If isWindowDefine = False And textList.Count <> 0 Then
                    If windowNum <> 1 Then
                        loadSeisanSubFile = 1
                        Exit Function
                    End If
                    Dim status As Integer = Me.loadWindow(textList)
                    If status <> 0 Then
                        loadSeisanSubFile = 1
                        Exit Function
                    End If
                End If

            End While
            dspFile.Close()
        Else
            loadSeisanSubFile = 1
        End If

    End Function

    Public Function findTab(tabType As String, ByRef tab As JManDispTab) As Integer

        findTab = 0

        Dim i As Integer = 0
        For i = 0 To _tabList.Count - 1
            Dim type As String = _tabList(i)._type
            If type = tabType Then
                tab = _tabList(i)
                Exit Function
            End If
        Next

        findTab = 1

    End Function

    Public Function findMatchTab(tabType As String, ByRef tab As JManDispTab) As Integer

        findMatchTab = 0

        Dim i As Integer = 0
        For i = 0 To _tabList.Count - 1
            Dim type As String = _tabList(i)._type
            If type Like tabType Then
                tab = _tabList(i)
                Exit Function
            End If
        Next

        findMatchTab = 1

    End Function

    Public Sub findTabList(tabType As String, ByRef tabList As ArrayList)

        Dim i As Integer = 0
        For i = 0 To _tabList.Count - 1
            Dim type As String = _tabList(i)._type
            If type = tabType Then
                tabList.Add(_tabList(i))
            End If
        Next

    End Sub

End Class

'ボタンのイベントと実行ファイル名を関連付けるクラス
Public Class JManDispButtonInfoList

    Private _infoList As New ArrayList              'ボタン情報リスト

    Public Sub append(info As JManDispButtonInfo)

        _infoList.Add(info)

    End Sub

    Public Sub clearList()

        _infoList.Clear()

    End Sub

    Public Function size() As Integer

        Return _infoList.Count

    End Function

    Public Function getNameAt(index As Integer) As String

        Return _infoList(index)._name

    End Function

    Public Function find(buttonName As String, ByRef info As JManDispButtonInfo)

        find = 1

        Dim i As Integer = 0
        For i = 0 To _infoList.Count - 1
            If _infoList(i)._name = buttonName Then
                find = 0
                info = _infoList(i)
                Exit Function
            End If
        Next

    End Function

End Class

Public Class JManDispButtonInfo
    '----------------プロパティ----------------
    Public _type As Integer = 0                    'ボタンタイプ(1～6)
    Public _name As String = ""                    'ボタン名(動的にコントロールを作成したときの名前)
    Public _fileName As String = ""                '実行ファイル・設定ファイル名
    Public _argument As String = ""                '実行ファイルの引数

End Class
