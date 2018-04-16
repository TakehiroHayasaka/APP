Imports System.IO
Imports System.Text

Public Class JCBExecutionLog
    '---------------------------------------------------------------------------------------------------
    ' メンバ変数
    '---------------------------------------------------------------------------------------------------
    'ファイルの中身
    Public _fileArray As New ArrayList
    '"◎ ｺﾏﾝﾄﾞ実行開始"の位置
    Public _commandStartTug As Integer = -1
    '◎ ｺﾏﾝﾄﾞ正常終了のフラグ
    Public _commandEndTug As Integer = -1

    '---------------------------------------------------------------------------------------------------
    ' リッチボックスにtextを追加
    '---------------------------------------------------------------------------------------------------
    Public Sub RichTextBoxLog_AddText(text As String)
        RichTextBoxLog.AppendText(text + Environment.NewLine + Environment.NewLine)
    End Sub

    '---------------------------------------------------------------------------------------------------
    ' リッチボックスにtextFileを追加
    ' 引数で実行ログが存在するか判定
    ' 0:なし、1:あり
    '---------------------------------------------------------------------------------------------------
    Public Function RichTextBoxLog_AddTextFile(folder As String, flag As String) As Integer

        _fileArray.Clear()

        If LoadFile(folder) <> 0 Then
            Return 1
        End If

        Dim count As Integer = _fileArray.Count

        If flag = 1 Then
            If 0 <= _commandStartTug Then
                For i As Integer = _commandStartTug To count - 1
                    '追加して改行
                    RichTextBoxLog.AppendText(_fileArray(i) + Environment.NewLine)
                Next
            End If
        End If

        '異常終了
        If _commandEndTug < _commandStartTug Or _commandStartTug = -1 Then
            _commandStartTug = -1
            Return 1
        End If

        Return 0
    End Function

    Public Function LoadFile(fileName As String) As Integer

        LoadFile = 0

        If System.IO.File.Exists(fileName) = True Then

            Dim file As New StreamReader(fileName, Encoding.GetEncoding("Shift_JIS"))

            While (file.Peek() >= 0)
                Dim line As String = file.ReadLine()

                If line.IndexOf("◎ ｺﾏﾝﾄﾞ実行開始") >= 0 Then
                    _commandStartTug = _fileArray.Count
                End If
                If line.IndexOf("◎ ｺﾏﾝﾄﾞ正常終了") >= 0 Then
                    _commandEndTug = _fileArray.Count
                End If

                _fileArray.Add(line)

            End While
            file.Close()
        Else
            LoadFile = 1
        End If

    End Function

    '---------------------------------------------------------------------------------------------------
    ' 各ボタンの設定
    '---------------------------------------------------------------------------------------------------
    Private Sub RadioButtonAbnormal_Click(sender As Object, e As EventArgs) Handles RadioButtonAbnormal.Click
        RadioButtonAbnormal.Checked = True
        RadioButtonAllShow.Checked = False
        RadioButtonFinishedMessage.Checked = False
        RadioButtonAbnormalWorning.Checked = False
    End Sub

    Private Sub RadioButtonAbnormalWorning_Click(sender As Object, e As EventArgs) Handles RadioButtonAbnormalWorning.Click
        RadioButtonAbnormal.Checked = False
        RadioButtonAllShow.Checked = False
        RadioButtonFinishedMessage.Checked = False
        RadioButtonAbnormalWorning.Checked = True
    End Sub

    Private Sub RadioButtonFinishedMessage_Click(sender As Object, e As EventArgs) Handles RadioButtonFinishedMessage.Click
        RadioButtonAbnormal.Checked = False
        RadioButtonAllShow.Checked = False
        RadioButtonFinishedMessage.Checked = True
        RadioButtonAbnormalWorning.Checked = False
    End Sub

    Private Sub RadioButtonAllShow_Click(sender As Object, e As EventArgs) Handles RadioButtonAllShow.Click
        RadioButtonAbnormal.Checked = False
        RadioButtonAllShow.Checked = True
        RadioButtonFinishedMessage.Checked = False
        RadioButtonAbnormalWorning.Checked = False
    End Sub

    Private Sub CheckBoxCombineMessages_Click(sender As Object, e As EventArgs) Handles CheckBoxCombineMessages.Click

    End Sub

    Private Sub OpenLogButton_Click(sender As Object, e As EventArgs) Handles openLogButton.Click

    End Sub

    '---------------------------------------------------------------------------------------------------
    ' MainWindow画面のサイズ変更が行われた際の処理
    '---------------------------------------------------------------------------------------------------
    'SizeChangedイベントハンドラ
    Private Sub MainWindow_SizeChanged(sender As Object, e As EventArgs) _
    Handles MyBase.SizeChanged

        'ログウィンドウのサイズを変更
        Dim c As Control = DirectCast(sender, Control)
        RichTextBoxLog.Width = c.Width - 40
        RichTextBoxLog.Height = c.Height - 180
        Panel1.Left = c.Width - 580
        Panel1.Top = RichTextBoxLog.Height + 25
        'Me.Width = c.Width - 420
        'Me.Height = c.Height - 105

    End Sub
End Class