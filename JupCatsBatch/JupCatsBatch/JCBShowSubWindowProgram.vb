Public Class JCBShowSubWindowProgram

    Public _buttonList As New ArrayList

    Private Sub JCBShowSubWindowProgram_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetButtons()
    End Sub

    Private Sub SetButtons()
        Dim count As Integer = _buttonList.Count

        For i As Integer = 0 To count - 1
            Dim name As String = System.Text.Encoding.GetEncoding("SHIFT-JIS").GetString(_buttonList(i)(0))
            Dim handle As Integer = _buttonList(i)(1)
            Dim button As New Button With {
                .Size = New Size(194, 20),
                .Location = New Point(3, i * 23 + 3),
                .BackColor = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer)),
                .Cursor = System.Windows.Forms.Cursors.Hand,
                .Name = "ShowProgramListButton",
                .TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                .ForeColor = System.Drawing.Color.White,
                .Text = name
            }
            'ボタンの識別名としてハンドルを設定
            button.Name = handle
            button.FlatAppearance.BorderSize = 0
            button.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
            button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray
            button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray
            button.FlatStyle = System.Windows.Forms.FlatStyle.Flat

            Me.Controls.Add(button)

            'イベントハンドラとして機能するようにする
            AddHandler button.Click, AddressOf Button_click

            If i > 0 Then
                'ボタンの数で高さを変更
                Me.Height = Me.Height + 23
            End If

        Next

    End Sub

    Public Sub Button_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim handle As Integer = sender.name
        'ハンドルのプログラムをアクティブにする
        SwitchToThisWindow(handle, 0)
    End Sub

    '指定したハンドルをアクティブにする
    <Runtime.InteropServices.DllImport("user32.dll", SetLastError:=True)>
    Public Shared Sub SwitchToThisWindow(hWnd As IntPtr, fAltTab As Boolean)
    End Sub
End Class