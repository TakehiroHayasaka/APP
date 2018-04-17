Public Class STtoCTpoint
    Public Property SelectedST As SingleTarget
    Public Property SelectedCT As String
    Public Property KaisekiRun As Boolean = False
    Private Sub STtoCTpoint_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        SelectedCT = ""
    End Sub

    Private Sub STtoCTpoint_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox1.Text = SelectedST.P2ID
        TextBox2.Text = ""
        KaisekiRun = False
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        SelectedCT = TextBox2.Text
        Me.Hide()
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        KaisekiRun = True
        Me.Hide()
    End Sub
End Class