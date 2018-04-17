Public Class YCM_DrawCricle
    Private Sub YCM_DrawCricle_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ComboBox1.SelectedIndex = 0
        Me.ComboBox1.SelectedIndex = sys_DrawCircle.iCombox
        Me.TextBox1.Text = CStr(sys_DrawCircle.dblR)
        Dim dd = New System.Drawing.Color

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            DrawCircleR = CDbl(TextBox1.Text)
            Select Case ComboBox1.SelectedIndex
                Case 0
                    DrawCircleXangle = 0
                    DrawCircleYangle = 0
                Case 1
                    DrawCircleXangle = 90
                    DrawCircleYangle = 0
                Case 2
                    DrawCircleXangle = 0
                    DrawCircleYangle = 90
            End Select
            sys_DrawCircle.iCombox = Me.ComboBox1.SelectedIndex
            sys_DrawCircle.dblR = CDbl(Me.TextBox1.Text)
            If (DrawCircleR <= 0.0#) Then
                MsgBox("半径には正の数値を入力してください。")
                DialogResult = Windows.Forms.DialogResult.Abort
            Else
                Me.Close()
            End If
        Catch ex As Exception
            MsgBox("半径には正の数値を入力してください。")
            DialogResult = Windows.Forms.DialogResult.Abort
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Private Sub btnCancle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancle.Click
        Me.Close()
    End Sub
End Class