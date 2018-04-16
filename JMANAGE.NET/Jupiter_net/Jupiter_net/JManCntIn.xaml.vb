Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Windows.Forms

Public Class JManCntIn

    Private Class JManCntInRecord

        ' 管理番号
        Public _dataNo As Integer = 0
        ' 入力データの管理名
        Public _dataName As String = ""
        ' 入力データのファイル名
        Public _dataFileName As String = ""

        Public Function initialize(texts As String()) As Integer

            initialize = 0

            If Integer.TryParse(texts(0), _dataNo) <> True Then
                initialize = 1
                Exit Function
            End If

            _dataName = texts(1)

            _dataFileName = texts(2)

        End Function

    End Class

    Public _cntFileName As String = ""                  ' CNTファイル名(フルパス)
    Public _cntName As String = ""                      ' CNTファイル名(パスなし)
    Public _cntFileHeader As String = ""                ' CNTファイル名(拡張子なし)
    Public _dataFileExt As String = ""                  ' 編集するデータファイルの拡張子
    Public _cntRecordList As New ArrayList
    Private _gridCollection As New JManGridCollection2()
    Public _dataFileName As String = ""                 ' 編集するデータファイル名
    Public _isOkCansel As Integer = 0

    Public Function OnInitialize(kojiFolder As String, cntFileName As String, dataFileExt As String, buttonColor As Integer, backColor As Integer) As Integer

        OnInitialize = 0

        _cntFileName = kojiFolder.TrimEnd("\") + "\" + cntFileName
        _cntName = cntFileName
        Dim cntFileHeaderTmp As String = cntFileName
        _cntFileHeader = cntFileHeaderTmp.ToUpper().Replace(".CNT", "")
        _dataFileExt = dataFileExt

        _isOkCansel = 0

        '背景色とボタン色を変更する。
        If backColor = 0 Then
            Me.Background = New SolidColorBrush(System.Windows.Media.Colors.DarkGray)
        Else
            Me.Background = New SolidColorBrush(System.Windows.Media.Colors.White)
        End If
        If buttonColor = 0 Then
            Me.ButtonCntInput.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 50, 50, 50))
            Me.ButtonCntCansel.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 50, 50, 50))
        Else
            Me.ButtonCntInput.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 164, 150))
            Me.ButtonCntCansel.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 164, 150))
        End If

        ' CNTファイルを読み込む
        Dim status As Integer = Me.load()
        If status = 0 Then
            'DataGridにCNTファイルの内容を表示する。
            Dim i As Integer = 0
            For i = 0 To _cntRecordList.Count - 1
                Dim cntData As New JManGridData2()
                cntData.Value1 = _cntRecordList(i)._dataNo.ToString()
                cntData.Value2 = _cntRecordList(i)._dataName
                _gridCollection.Add(cntData)
            Next
        Else
            'DataGridに空の1行を表示する。
            Dim cntData As New JManGridData2()
            cntData.Value1 = ""
            cntData.Value2 = ""
            _gridCollection.Add(cntData)
        End If

        Me.DatGridCntIn.DataContext = _gridCollection

    End Function

    '' DataGrid1で選択行が移動したら、下端のテキストボックスにその値を反映させる
    'Private Sub SelectionChanged_DatGridCntIn(sender As Object, e As SelectionChangedEventArgs)
    '    Dim selectedData = TryCast(Me.DatGridCntIn.SelectedItem, JManCntData)
    '    If (selectedData Is Nothing) Then
    '        Me.TextBoxIndex.Text = String.Empty
    '        Me.TextBoxValue.Text = String.Empty
    '    Else
    '        Me.TextBoxIndex.Text = selectedData.Index
    '        Me.TextBoxValue.Text = selectedData.Value
    '    End If
    'End Sub
    Private Function load() As Integer

        load = 0

        '連続している空白を一つにする。
        Dim re As Regex
        re = New Regex("\s+") '空白文字の連続で区切られる

        If System.IO.File.Exists(_cntFileName) = True Then
            Dim cntFile As New StreamReader(_cntFileName, Encoding.GetEncoding("Shift_JIS"))
            While (cntFile.Peek() >= 0)
                Dim lineText As String = cntFile.ReadLine().Trim()
                removeComment(lineText)
                If lineText = "" Then
                    Continue While
                End If

                Dim texts As String() = re.Split(lineText)
                If texts.Count = 1 Then
                ElseIf texts.Count = 3 Then
                    Dim record As New JManCntInRecord()
                    record.initialize(texts)
                    _cntRecordList.Add(record)
                End If

            End While
            cntFile.Close()
        Else
            load = 1
        End If

    End Function

    Private Sub OnClick_ButtonCntInput(sender As Object, e As RoutedEventArgs) Handles ButtonCntInput.Click

        'Dim index As Integer = Me.DatGridCntIn.Items.IndexOf(Me.DatGridCntIn.CurrentItem)
        Dim index As Integer = Me.DatGridCntIn.SelectedIndex

        If index = -1 Then
            MessageBox.Show("編集するデータが選択されていません。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' _cntRecordListを作成しなおす。
        Dim recList As New ArrayList()
        Dim isChanged As Boolean = True
        Dim i As Integer = 0
        For i = 0 To Me.DatGridCntIn.Items.Count - 1
            Dim dataName As String = ""
            Try
                dataName = Me.DatGridCntIn.Items.Item(i).Value2
            Catch ex As Exception
                Continue For
            End Try
            Dim idx As Integer = Me.findCntRecord(dataName)
            Dim record As New JManCntInRecord
            If idx = -1 Then
                isChanged = False
                Dim no As Integer = Me.getFileNumber()
                If no = -1 Then
                    no = 1
                End If
                Dim dataFileName As String = _cntFileHeader + no.ToString() + "." + _dataFileExt
                record._dataNo = i + 1
                record._dataName = dataName
                record._dataFileName = dataFileName
                recList.Add(record)
            Else
                record._dataNo = i + 1
                record._dataName = _cntRecordList(idx)._dataName
                record._dataFileName = _cntRecordList(idx)._dataFileName
                recList.Add(record)
            End If
        Next

        If isChanged = False Or _cntRecordList.Count <> recList.Count Then
            'グリッドが変更されてたらCNTファイルを保存する。
            _cntRecordList.Clear()
            _cntRecordList = recList
            Me.saveCntFile()
        End If

        If index >= _cntRecordList.Count Then
            MessageBox.Show("選択行の[入力データの管理名]が入力されていません。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        _dataFileName = _cntRecordList(index)._dataFileName

        _isOkCansel = 0

        Me.Close()

    End Sub

    Private Sub saveCntFile()

        Dim i As Integer = 0
        Dim cntWriter As New StreamWriter(_cntFileName, False, Encoding.GetEncoding("Shift_JIS"))
        cntWriter.WriteLine(";入力ﾃﾞｰﾀ管理ﾃﾞｰﾀ")
        cntWriter.WriteLine(";管理ﾃﾞｰﾀ数")
        cntWriter.WriteLine(_cntRecordList.Count.ToString())
        cntWriter.WriteLine(";管理番号    管理ﾃﾞｰﾀ名    入力ﾃﾞｰﾀﾌｧｲﾙ名")
        For i = 0 To _cntRecordList.Count - 1
            Dim text As String = _cntRecordList(i)._dataNo.ToString() + "   " + _cntRecordList(i)._dataName + "   " + _
                                 _cntRecordList(i)._dataFileName()
            cntWriter.WriteLine(text)
        Next
        cntWriter.Close()

    End Sub

    Private Function findCntRecord(dataName As String) As Integer

        findCntRecord = -1

        Dim i As Integer = 0
        For i = 0 To _cntRecordList.Count - 1
            If dataName = _cntRecordList(i)._dataName Then
                findCntRecord = i
                Exit Function
            End If
        Next

    End Function

    Private Function getFileNumber() As Integer

        getFileNumber = -1

        Dim noList As New ArrayList()
        Dim i As Integer = 0
        For i = 0 To _cntRecordList.Count - 1
            Dim fileName As String = _cntRecordList(i)._dataFileName
            Dim fileHeader As String = fileName.ToUpper().Replace("." + _dataFileExt, "")
            Dim fileNumber As String = fileHeader.ToUpper().Replace(_cntFileHeader.ToUpper(), "")
            Dim no As Integer = 0
            If Integer.TryParse(fileNumber, no) = True Then
                noList.Add(no)
            End If
        Next

        If noList.Count <> 0 Then
            noList.Sort()
            getFileNumber = noList(noList.Count - 1) + 1
        End If

    End Function

    Private Sub OnClick_ButtonCntCansel(sender As Object, e As RoutedEventArgs) Handles ButtonCntCansel.Click

        _cntFileName = ""
        _cntName = ""
        _cntFileHeader = ""
        _dataFileExt = ""
        _dataFileName = ""
        _cntRecordList.Clear()
        _gridCollection.Clear()

        _isOkCansel = 1
        Me.Close()

    End Sub

End Class
