
Public Class JManSelectTsc

    Private _kojiFolder As String = ""
    Private _type As Integer = 0        '0：会話処理のみ、1：部材一括＋会話処理
    Private _tscFileList As New ArrayList()
    Public _tscFileName As String = ""

    Public Function OnInitialize(kojiFolder As String, type As Integer, buttonColor As Integer, backColor As Integer) As Integer

        Me._kojiFolder = kojiFolder
        Me._type = type

        '背景色とボタン色を変更する。
        If backColor = 0 Then
            Me.Background = New SolidColorBrush(System.Windows.Media.Colors.DarkGray)
        Else
            Me.Background = New SolidColorBrush(System.Windows.Media.Colors.White)
        End If
        If buttonColor = 0 Then
            Me.ButtonSelectTscOk.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 50, 50, 50))
            Me.ButtonSelectTscCansel.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 50, 50, 50))
        Else
            Me.ButtonSelectTscOk.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 164, 150))
            Me.ButtonSelectTscCansel.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 164, 150))
        End If

        'コンボボックスの選択肢の作成とTSCファイルの読込を行う。
        Dim tscFiles As String() = System.IO.Directory.GetFiles(Me._kojiFolder, "*.TSC")
        Dim i As Integer = 0
        For i = 0 To tscFiles.Count - 1
            Dim tscFileNameTmp As String = subStringEnd(tscFiles(i), "\")
            Dim tscFileName As String = tscFileNameTmp.Substring(0, tscFileNameTmp.Count - 4)
            If tscFileName.ToUpper() <> "BZIBATCH" Then
                Dim tscFile As New JManTscFile()
                Dim status As Integer = tscFile.load(tscFiles(i))
                If status <> 0 Then
                    Continue For
                End If
                If Me._type = tscFile._isBatch Then
                    Me.ComboBoxTsc.Items.Add(tscFileName)
                    tscFile._fileName = tscFileName
                    Me._tscFileList.Add(tscFile)
                End If
            End If
        Next

        If Me._tscFileList.Count = 0 Then
            Return 1
        End If

        Me.ComboBoxTsc.SelectedIndex = 0
        Me.dispListBox(0)

        Return 0

    End Function

    Public Sub dispListBox(index As Integer)

        Me.ListBoxTsc.Items.Clear()

        Dim i As Integer = 0
        For i = 0 To Me._tscFileList(index).size() - 1
            Dim text As String = ""
            If i <> 0 Then
                text = i.ToString() + "  "
            End If
            text += Me._tscFileList(index)._recordList(i).toStringRecord()
            Me.ListBoxTsc.Items.Add(text)
        Next

    End Sub

    Private Sub OnClick_ButtonSelectTscOk(sender As Object, e As RoutedEventArgs) Handles ButtonSelectTscOk.Click

        Dim index As Integer = Me.ComboBoxTsc.SelectedIndex
        Me._tscFileName = Me._tscFileList(index)._fileName

        Me.Close()

    End Sub

    Private Sub OnClick_ButtonSelectTscCansel(sender As Object, e As RoutedEventArgs) Handles ButtonSelectTscCansel.Click

        Me._tscFileName = ""
        Me.Close()

    End Sub

    Private Sub SelectionChanged_ComboBoxTsc(sender As Object, e As SelectionChangedEventArgs) Handles ComboBoxTsc.SelectionChanged

        Me.dispListBox(Me.ComboBoxTsc.SelectedIndex)

    End Sub
End Class
