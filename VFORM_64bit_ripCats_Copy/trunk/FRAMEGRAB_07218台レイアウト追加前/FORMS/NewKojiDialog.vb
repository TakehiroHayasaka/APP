Imports System.Windows.Forms

Public Class NewKojiDialog
    Public strwKojiMei As String = ""
    Public strwKojiFolder As String = ""

    Public intFlgMode As Integer = 0
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        'OK_Button.Enabled = False

        If TextBox1.Text = "" Then
            MsgBox("工事名を入力して下さい。", MsgBoxStyle.Critical)
            Exit Sub
        End If

        If TextBox2.Text = "" Then
            MsgBox("工事フォルダを指定して下さい。", MsgBoxStyle.Critical)
            Exit Sub
        End If

        'OK_Button.Enabled = True

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        strwKojiMei = TextBox1.Text
        strwKojiFolder = TextBox2.Text
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim m_fbd As FolderBrowserDialog        '新規フォルダ作成可
        m_fbd = New FolderBrowserDialog
        With m_fbd
            Dim iDialogResult As System.Windows.Forms.DialogResult
            iDialogResult = .ShowDialog()
            If iDialogResult = System.Windows.Forms.DialogResult.OK Then
                ''選択されたフォルダを表示する
                TextBox2.Text = m_fbd.SelectedPath
            Else
                Exit Sub
            End If
        End With

    End Sub

    Private Sub NewKojiDialog_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If intFlgMode = 0 Then
            Me.Text = "新規作成"
            TextBox1.Enabled = True
        Else
            Me.Text = "開く"
            TextBox1.Enabled = False

        End If
    End Sub

    Private Sub TextBox2_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox2.TextChanged
        If intFlgMode = 1 Then
            If My.Computer.FileSystem.FileExists(TextBox2.Text & "\工事名.txt") Then
                Dim path As String = TextBox2.Text & "\工事名.txt"
                Dim fs As New System.IO.StreamReader(path, System.Text.Encoding.Default)
                Dim StrKojiMei As String
                StrKojiMei = Trim(fs.ReadLine())
                fs.Close()
                TextBox1.Text = StrKojiMei
            Else
                TextBox1.Text = "　工事フォルダーではありません。"
            End If
        End If
    End Sub
End Class
