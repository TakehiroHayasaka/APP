Public Class JManWaitSaveDlg

    Public Sub OnInitialize(backColor As Integer)

        If backColor = 0 Then
            Me.Background = New SolidColorBrush(System.Windows.Media.Colors.Black)
        Else
            Me.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 164, 150))
        End If

    End Sub

End Class
