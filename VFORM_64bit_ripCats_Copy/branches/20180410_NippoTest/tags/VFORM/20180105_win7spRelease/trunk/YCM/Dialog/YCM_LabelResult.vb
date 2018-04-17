Public Class YCM_LabelResult
    Public m_Return As Boolean
    Private Sub YCM_LabelResult_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label2.Text = csv_path
        data_point_result.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        data_point_result.AllowUserToAddRows = False
        data_point_result.AllowUserToResizeColumns = False
        data_point_result.AllowUserToResizeRows = False
        data_point_result.AllowUserToDeleteRows = False
        data_point_result.ReadOnly = True
        m_Return = False
        data_point_result.DefaultCellStyle.Format = "0.000"
    End Sub
    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        OKsyori()
    End Sub
    Public Sub OKsyori()
        Dim arrupdaatlabel As New ArrayList
        Dim arrupdatalabelnew As New ArrayList
        For ii As Integer = 0 To data_point_result.Rows.Count - 1
            If data_point_result.Rows(ii).Cells(1).Value <> "" And data_point_result.Rows(ii).Cells(2).Value <> "" Then
                For jj As Integer = 0 To nLookPoints - 1
                    If gDrawPoints(jj).LabelName = data_point_result.Rows(ii).Cells(2).Value Then

                        gDrawPoints(jj).LabelName = data_point_result.Rows(ii).Cells(1).Value
                        'gDrawLabelText(jj).LabelName = data_point_result.Rows(ii).Cells(1).Value


                        'Data_Point.DGV_DV.Rows(jj).Cells(1).Value = data_point_result.Rows(ii).Cells(1).Value
                    End If
                    If Data_Point.DGV_DV.Rows(jj).Cells(1).Value = data_point_result.Rows(ii).Cells(2).Value Then
                        arrupdaatlabel.Add(data_point_result.Rows(ii).Cells(2))
                        arrupdatalabelnew.Add(data_point_result.Rows(ii).Cells(1))
                        Data_Point.DGV_DV.Rows(jj).Cells(1).Value = data_point_result.Rows(ii).Cells(1).Value
                    End If

                Next
            End If
        Next

        For ii As Integer = 0 To nLookPoints - 1
            For jj As Integer = 0 To nLabelText - 1
                If gDrawPoints(ii).mid = gDrawLabelText(jj).mid Then
                    gDrawLabelText(jj).LabelName = gDrawPoints(ii).LabelName
                End If
            Next
        Next
        YCM_Updatalabelresult(m_strDataBasePath, arrupdaatlabel, arrupdatalabelnew)
        MainFrm.SetCurrentLabelTo_objFBM()

        Me.Close()
    End Sub

    Private Sub CSVOUT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CSVOUT.Click
        str_OutCSVPath = ""
        str_OutCSVPath = Filesave_P(MainFrm.SaveFileDialog1)
        If str_OutCSVPath <> "" Then
            Dim fi_csvout As New IO.FileInfo(str_OutCSVPath)
            If str_OutCSVPath <> "" Then
                If fi_csvout.IsReadOnly = True And fi_csvout.Exists = True Then
                    Dim intoutcsvpathresult As Integer = MsgBox("このファイルは読み込みのみですので、別の名前を指定してください")
                    'Select Case intoutcsvpathresult
                    '    Case 1
                    '        fi_csvout.IsReadOnly = False
                    '    Case 2
                    '        Exit Sub
                    'End Select
                    Exit Sub
                End If
                Creattxt(str_OutCSVPath)
                readlabeltxt(str_OutCSVPath, data_point_result)
            End If
        End If
    End Sub

    Private Sub Rreturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rreturn.Click
        m_Return = True
        Me.Close()
    End Sub
End Class