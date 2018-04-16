Imports System.IO
Imports System.Text

Public Class JCBSetFileExplore
    'ファイルエクスプローラーに関するリスト
    Public _fileExploreInfList As New ArrayList
    '工事フォルダ
    Public _kojiFolder As String = ""

    Public Function LoadFile(fileName As String) As Integer

        LoadFile = 0

        If System.IO.File.Exists(fileName) = True Then

            Dim file As New StreamReader(fileName, Encoding.GetEncoding("Shift_JIS"))

            While (file.Peek() >= 0)
                Dim line As String = file.ReadLine().Trim()

                RemoveComment(line)
                If line = "" Then
                    Continue While
                End If

                Me.AssociateInformation(line, _kojiFolder)

            End While
            file.Close()
        Else
            LoadFile = 1
        End If

    End Function

    Public Function Size() As Integer

        Return _fileExploreInfList.Count

    End Function

    Public Sub clearList()

        _fileExploreInfList.Clear()

    End Sub

    Public Function FindJCBFileExploreInfo(fileType As String) As JCBFileExploreInfo
        Dim info As New JCBFileExploreInfo
        Dim count As Integer = size()

        For i As Integer = 0 To count - 1
            info = _fileExploreInfList(i)

            If info._fileType = fileType Then
                Return info
                Exit For
            End If

        Next

        Return info
    End Function

    Public Function AssociateInformation(str As String, kojiFolder As String) As Integer
        Dim info As New JCBFileExploreInfo
        Dim array As String() = str.Split(","c)
        Dim size As Integer = array.Length
        Dim filePath As String = ""
        Dim comparisonFilePath As String = ""

        '項目数は4つ以上必要
        If size >= 4 Then
            If TryParseToInteger(array(0)) <> False And array(0) <> "*" Then
                info._noElement1 = array(0)
            End If

            If TryParseToInteger(array(1)) <> False And array(1) <> "*" Then
                info._noElement2 = array(1)
            End If

            If TryParseToInteger(array(2)) <> False And array(2) <> "*" Then
                info._noElement3 = array(2)
            End If

            info._fileType = array(3)
            info._dataFolder = array(4)

            'ファイル名に"*"を含む場合
            If array(5).IndexOf("*") >= 0 Then
                AddWildCardFile(array, kojiFolder)
                Return 0
            End If

            info._fileName = array(5)

            If array(6) = "" Then
                filePath = CreateFilePath(kojiFolder, array(4), array(5))

                If System.IO.File.Exists(filePath) = False Then
                    info._status = 2

                Else

                    Dim updateDate As Date = System.IO.File.GetLastWriteTime(filePath)
                    Dim fileSize As New System.IO.FileInfo(filePath)
                    Dim extension() As String = array(5).Split("."c)

                    info._status = 0
                    info._updateDate = updateDate
                    'KBに変換
                    info._size = fileSize.Length * 0.001
                End If

            Else
                filePath = CreateFilePath(kojiFolder, array(4), array(5))
                '比較ファイル名に"*"を含む場合
                If array(6).IndexOf("*") >= 0 Then
                    Dim res As String = ""
                    res = GetComparisonFile(array(6), kojiFolder, info._dataFolder)
                    comparisonFilePath = CreateFilePath(kojiFolder, array(4), res)
                Else
                    comparisonFilePath = CreateFilePath(kojiFolder, array(4), array(6))
                End If

                If System.IO.File.Exists(filePath) = False Or System.IO.File.Exists(comparisonFilePath) = False Then
                    info._status = 2

                Else
                    Dim updateDate As Date = System.IO.File.GetLastWriteTime(filePath)
                    Dim comparisonUpdateDate As Date = System.IO.File.GetLastWriteTime(comparisonFilePath)
                    Dim result As Integer = DateTime.Compare(updateDate, comparisonUpdateDate)
                    Dim fileSize As New System.IO.FileInfo(filePath)
                    Dim extension() As String = array(5).Split("."c)

                    If result < 0 Then
                        '比較ファイルより古い
                        info._status = 1
                    ElseIf result = 0 Then
                        '同じ
                        'ありえるのか？？
                        info._status = 1
                    Else
                        '比較ファイルよりも新しい
                        info._status = 0
                    End If

                    info._updateDate = updateDate
                    'KBに変換
                    info._size = fileSize.Length * 0.001
                End If

            End If

            info._displayProgram1 = array(7)
            info._displayProgram2 = array(8)

            _fileExploreInfList.Add(info)

            Return 0

        Else
            Return 1
        End If

    End Function

    Sub AddWildCardFile(array As String(), kojiFolder As String)
        Dim folder As String = kojiFolder + array(4)
        ' フォルダ (ディレクトリ) が存在しているかどうか確認する
        If System.IO.Directory.Exists(folder) Then
            Dim files As String() = System.IO.Directory.GetFiles(folder, array(5), System.IO.SearchOption.AllDirectories)
            Dim filesLength As Integer = files.Length
            If filesLength > 0 Then
                For i As Integer = 0 To filesLength - 1
                    Dim info As New JCBFileExploreInfo
                    Dim filePath As String = ""
                    Dim comparisonFilePath As String = ""

                    If TryParseToInteger(array(0)) <> False And array(0) <> "*" Then
                        info._noElement1 = array(0)
                    End If

                    If TryParseToInteger(array(1)) <> False And array(1) <> "*" Then
                        info._noElement2 = array(1)
                    End If

                    info._noElement3 = i + 1

                    'XXX.dbaの形
                    Dim stringArray As String() = files(i).Split("\"c)
                    Dim fileName As String = stringArray(stringArray.Count - 1)
                    'XXXの形
                    stringArray = stringArray(stringArray.Count - 1).Split("."c)
                    Dim fileType As String = stringArray(0)
                    info._fileType = fileType
                    info._dataFolder = array(4)
                    info._fileName = fileName

                    If array(6) = "" Then
                        filePath = CreateFilePath(kojiFolder, array(4), array(5))

                        If System.IO.File.Exists(filePath) = False Then
                            info._status = 2

                        Else

                            Dim updateDate As Date = System.IO.File.GetLastWriteTime(filePath)
                            Dim fileSize As New System.IO.FileInfo(filePath)
                            Dim extension() As String = array(5).Split("."c)

                            info._status = 0
                            info._updateDate = updateDate
                            'KBに変換
                            info._size = fileSize.Length * 0.001
                        End If

                    Else
                        filePath = CreateFilePath(kojiFolder, array(4), info._fileName)
                        '比較ファイル名に"*"を含む場合
                        If array(6).IndexOf("*") >= 0 Then
                            Dim res As String = ""
                            res = GetComparisonFile(array(6), kojiFolder, info._dataFolder)
                            comparisonFilePath = CreateFilePath(kojiFolder, array(4), res)
                        Else
                            comparisonFilePath = CreateFilePath(kojiFolder, array(4), array(6))
                        End If

                        If System.IO.File.Exists(filePath) = False Or System.IO.File.Exists(comparisonFilePath) = False Then
                            info._status = 2

                        Else
                            Dim updateDate As Date = System.IO.File.GetLastWriteTime(filePath)
                            Dim comparisonUpdateDate As Date = System.IO.File.GetLastWriteTime(comparisonFilePath)
                            Dim result As Integer = DateTime.Compare(updateDate, comparisonUpdateDate)
                            Dim fileSize As New System.IO.FileInfo(filePath)
                            Dim extension() As String = array(5).Split("."c)

                            If result < 0 Then
                                '比較ファイルより古い
                                info._status = 1
                            ElseIf result = 0 Then
                                '同じ
                                'ありえるのか？？
                                info._status = 1
                            Else
                                '比較ファイルよりも新しい
                                info._status = 0
                            End If

                            info._updateDate = updateDate
                            'KBに変換
                            info._size = fileSize.Length * 0.001
                        End If

                    End If

                    info._displayProgram1 = array(7)
                    info._displayProgram2 = array(8)

                    _fileExploreInfList.Add(info)
                Next
            End If
        End If
    End Sub

    '最新の比較ファイルを取得する
    Private Function GetComparisonFile(arg As String, kojiFolder As String, dateFolder As String) As String
        Dim filename As String = ""
        Dim filePath As String = ""
        Dim folder As String = kojiFolder + dateFolder

        If System.IO.Directory.Exists(folder) Then
            Dim files As String() = System.IO.Directory.GetFiles(folder, arg, System.IO.SearchOption.AllDirectories)
            Dim filesLength As Integer = files.Length
            If filesLength > 0 Then
                For i As Integer = 0 To filesLength - 1
                    If i = 0 Then
                        filePath = files(i)
                        Continue For
                    End If
                    Dim latestfileDate As Date = System.IO.File.GetLastWriteTime(filePath)
                    Dim fileDate As Date = System.IO.File.GetLastWriteTime(files(i))
                    Dim result As Integer = DateTime.Compare(fileDate, latestfileDate)
                    If result > 0 Then
                        '比較ファイルより新しい
                        filePath = files(i)
                    End If

                Next
            End If
        End If

        Dim name As String() = filePath.Split("\"c)
        filename = name(name.Count - 1)

        Return filename
    End Function

End Class

'------------------------------------------------------------------------------------------
'ファイルエクスプローラに表示する情報
'------------------------------------------------------------------------------------------
Public Class JCBFileExploreInfo

    Public _noElement1 As Integer = 0              'No要素1
    Public _noElement2 As Integer = 0              'No要素2
    Public _noElement3 As Integer = 0              'No要素3
    Public _fileType As String = ""                'ファイル種類
    Public _dataFolder As String = ""              'データフォルダ
    Public _fileName As String = ""                'ファイル名
    Public _status As Integer = 0                  'ステータス(0：新しい、1：古い、2：存在しない)
    Public _updateDate As String = ""              '更新日
    Public _size As Integer = 0                    'サイズ
    Public _lastSavedPerson As String = ""         '最終保存者
    Public _displayProgram1 As String = ""         '表示プログラム1
    Public _displayProgram2 As String = ""         '表示プログラム2

End Class
