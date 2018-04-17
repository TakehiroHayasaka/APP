Imports System.Windows.Forms

Public Class YCM_UserPointBatchListResult
    Public m_Return As Boolean

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        m_Return = False
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        m_Return = True
        Me.Close()
    End Sub

    Private Sub Button_AllSel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_AllSel.Click
        Dim ii As Long, strErr As String
        For ii = 0 To DataGridView1.Rows.Count - 1
            strErr = DataGridView1.Rows(ii).Cells(5).Value
            If (Len(strErr) <= 0) Then
                DataGridView1.Rows(ii).Cells(0).Value = True
            End If
        Next
    End Sub

    Private Sub Button_UnSel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_UnSel.Click
        Dim ii As Long
        For ii = 0 To DataGridView1.Rows.Count - 1
            DataGridView1.Rows(ii).Cells(0).Value = False
        Next
    End Sub
End Class
