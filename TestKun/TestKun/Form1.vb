Public Class Form1
    Private _cnt As Integer = 0
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If _cnt = 100 Then
            Me.ShowClearMessage()
            _cnt += 1
            Return
        End If

        _cnt += 1
        Me.Label1.Text = _cnt.ToString
        Me.Refresh()

    End Sub

    Private Sub ShowClearMessage()

        Me.Label2.Text = "おめでとうございます！" + vbCrLf + "高橋名人に近づきましたね．．．。"
        Dim a As Integer

        a = 2

        a = 3

    End Sub

End Class
