Imports System.Windows.Forms

Public Class YCM_MovePosList
    Private Sub YCM_MovePosList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Move_Point.DGV_MoveList：移動点一括処理結果DataGrid    
        '    

        'DGV_MoveList
        'For i As Integer = 0 To nLookPoints - 1
        '    Move_Point.DGV_MoveList.Rows.Add()
        '    If gDrawPoints(i).blnDraw Then
        '        Move_Point.DGV_MoveList.Rows(i).Cells(0).Value = True
        '    Else
        '        Move_Point.DGV_MoveList.Rows(i).Cells(0).Value = False
        '    End If
        '    Move_Point.DGV_MoveList.Rows(i).Cells(1).Value = gDrawPoints(i).LabelName
        '    Move_Point.DGV_MoveList.Rows(i).Cells(2).Value = gDrawPoints(i).Real_x
        '    Move_Point.DGV_MoveList.Rows(i).Cells(3).Value = gDrawPoints(i).Real_y
        '    Move_Point.DGV_MoveList.Rows(i).Cells(4).Value = gDrawPoints(i).Real_z
        '    Move_Point.DGV_MoveList.Rows(i).Cells(5).Value = gDrawPoints(i).mid
        'Next

    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK



        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class
