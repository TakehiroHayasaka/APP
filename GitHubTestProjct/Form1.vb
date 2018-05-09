Public Class Form1

    Private _count As Integer = 0

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim i As Integer = 0
        Dim total As Integer = 0

        total = i + 1

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        _count += 2
        Me.Text = _count.ToString

    End Sub
End Class
