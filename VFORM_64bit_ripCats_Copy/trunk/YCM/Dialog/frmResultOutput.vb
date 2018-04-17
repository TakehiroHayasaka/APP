Imports HalconDotNet

Public Class frmResultOutput
    Dim ofd As New OpenFileDialog

    Dim lstBunkatsu As New List(Of BunkatsuLabeling)
    Friend WithEvents SFD1 As New System.Windows.Forms.SaveFileDialog
    'add by Susano 2016-03-15
    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles bunkatsuResultView.CellContentClick
        If e.ColumnIndex = 2 Then
            If ofd.ShowDialog() = DialogResult.OK Then
                If lstBunkatsu.Item(e.RowIndex).ReadSekkeiData(ofd.FileName) = True Then
                    lstBunkatsu.Item(e.RowIndex).SekkeiFileName = ofd.FileName
                    bunkatsuResultView.Item(e.ColumnIndex - 1, e.RowIndex).Value = ofd.FileName
                Else
                    MsgBox("設計データ読み込めませんでした。")
                End If
            End If
        End If

    End Sub

    Private Sub newform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lstBunkatsu = New List(Of BunkatsuLabeling)
        For Each objC3DCT As FBMlib.Common3DCodedTarget In MainFrm.objFBM.lstCommon3dCT
            Dim newBunkatsu As New BunkatsuLabeling
            Dim blnExist As Boolean = False
            For Each objBunkatsu As BunkatsuLabeling In lstBunkatsu
                If objBunkatsu.HeadStr = GetHeadStr(objC3DCT.currentLabel) Then
                    blnExist = True
                    objBunkatsu.lstCommonCT.Add(objC3DCT)
                    Exit For
                End If
            Next
            If blnExist = False Then
                newBunkatsu.HeadStr = GetHeadStr(objC3DCT.currentLabel)
                newBunkatsu.Kubun1 = objC3DCT.K1
                newBunkatsu.lstCommonCT.Add(objC3DCT)
                lstBunkatsu.Add(newBunkatsu)
            End If
        Next

        Dim ii As Integer = 0
        For Each objBunkatsu As BunkatsuLabeling In lstBunkatsu

            bunkatsuResultView.Rows.Add()
            bunkatsuResultView.Rows(ii).Cells(0).Value = objBunkatsu.HeadStr
            ii = ii + 1
        Next

    End Sub
    'add by Susano 2016-03-17
    Private Function GetHeadStr(ByVal PointName As String) As String
        GetHeadStr = ""
        For i = 0 To PointName.Length - 1
            If Char.IsLetter(PointName.Chars(i)) Then
                GetHeadStr = String.Concat(GetHeadStr, PointName.Chars(i))
            Else
                Exit For
            End If
        Next
        Return GetHeadStr
    End Function

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked = True Then
            GroupBox2.Enabled = True
            GroupBox3.Enabled = False
            '20160407 Kiryu Add Disable時グレーアウト
            bunkatsuResultView.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.LightGray
            bunkatsuResultView.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray

        Else
            If RadioButton2.Checked = True Then
                GroupBox2.Enabled = False
                GroupBox3.Enabled = True
                '20160407 Kiryu Add Enable時標準フォント色
                bunkatsuResultView.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.Black
                bunkatsuResultView.DefaultCellStyle.ForeColor = System.Drawing.Color.Black

                'bunkatsuResultView.Enabled = True
            End If
        End If
    End Sub
    ' susano add 2016-03-15 sta
    Private Sub BunkatsuOutPut()
        For i As Integer = 0 To bunkatsuResultView.RowCount - 1
            Dim outputFileName As String = Me.bunkatsuResultView.Rows(i).Cells(3).Value
            If outputFileName <> "" Then 'String.IsNullOrEmpty(outputFileName) Then
                Dim sekkeiFilename As String = Me.bunkatsuResultView.Rows(i).Cells(1).Value
                Dim textValue As String = Me.bunkatsuResultView.Rows(i).Cells(0).Value
                Dim objBunkatsu As BunkatsuLabeling = lstBunkatsu.Item(i)
                If sekkeiFilename <> "" Then
                    If objBunkatsu.ReadSekkeiData(sekkeiFilename) = True Then
                        If objBunkatsu.lstSekkeiData.Count > 0 Then
                            Dim sP As New FBMlib.Point3D
                            Dim mP As New FBMlib.Point3D
                            Dim cntOk As Integer = 0
                            Dim homMat3d As Object = Nothing
                            For Each objC3DCT As FBMlib.Common3DCodedTarget In objBunkatsu.lstCommonCT
                                If objC3DCT.lstRealP3d.Count > 0 Then
                                    For ii As Integer = 0 To objBunkatsu.lstSekkeiData.Count - 1
                                        Dim objSekkeiData As SekkeiData = objBunkatsu.lstSekkeiData.Item(ii)
                                        If objC3DCT.K2 = 901 And objSekkeiData.flgKijyun = 1 Then
                                            sP.ConcatToMe(objSekkeiData.Point3d)
                                            mP.ConcatToMe(objC3DCT.lstRealP3d.Item(0))
                                            cntOk += 1
                                        End If
                                        If objC3DCT.K2 = 902 And objSekkeiData.flgKijyun = 2 Then
                                            sP.ConcatToMe(objSekkeiData.Point3d)
                                            mP.ConcatToMe(objC3DCT.lstRealP3d.Item(0))
                                            cntOk += 1
                                        End If
                                        If objC3DCT.K2 = 903 And objSekkeiData.flgKijyun = 3 Then
                                            sP.ConcatToMe(objSekkeiData.Point3d)
                                            mP.ConcatToMe(objC3DCT.lstRealP3d.Item(0))
                                            cntOk += 1
                                        End If
                                    Next
                                End If
                            Next

                            If cntOk = 3 Then
                                Dim PX As Object = sP.X
                                Dim PY As Object = sP.Y
                                Dim PZ As Object = sP.Z
                                Dim QX As Object = mP.X
                                Dim QY As Object = mP.Y
                                Dim QZ As Object = mP.Z

                                FBMlib.hom_mat_3d_from_3d_3d_point_correspondence(QX, QY, QZ, PX, PY, PZ, homMat3d)
                                Dim strFileDotorh As String = ""
                                For Each objC3DCT As FBMlib.Common3DCodedTarget In objBunkatsu.lstCommonCT
                                    If objC3DCT.lstRealP3d.Count > 0 Then
                                        Dim outP As New FBMlib.Point3D
                                        Dim inP As FBMlib.Point3D = objC3DCT.lstRealP3d.Item(0)
                                        HOperatorSet.AffineTransPoint3d(homMat3d, inP.X, inP.Y, inP.Z, outP.X, outP.Y, outP.Z)
                                        If objBunkatsu.HeadStr.StartsWith(textValue) Then
                                            strFileDotorh = strFileDotorh & objC3DCT.currentLabel & "," & outP.X.D & "," & outP.Y.D & "," & outP.Z.D & vbNewLine
                                        End If
                                    End If
                                Next
                                My.Computer.FileSystem.WriteAllText(m_koji_kanri_path & "\" & outputFileName & ".csv", strFileDotorh, False)
                            End If
                        End If
                    End If
                Else

                    Dim strFileDotorh As String = ""
                    For ii As Integer = 0 To bunkatsuResultView.RowCount - 1
                        Dim objBunkatsu1 As BunkatsuLabeling = lstBunkatsu.Item(ii)
                        For Each objC3DCT As FBMlib.Common3DCodedTarget In objBunkatsu1.lstCommonCT
                            If objC3DCT.lstRealP3d.Count > 0 Then
                                If objBunkatsu1.HeadStr.StartsWith(textValue) Then
                                    strFileDotorh = strFileDotorh & objC3DCT.currentLabel & "," & objC3DCT.lstRealP3d.Item(0).X.D & "," & objC3DCT.lstRealP3d.Item(0).Y.D & "," & objC3DCT.lstRealP3d.Item(0).Z.D & vbNewLine
                                End If
                            End If
                        Next
                    Next
                    My.Computer.FileSystem.WriteAllText(m_koji_kanri_path & "\" & outputFileName & ".csv", strFileDotorh, False)
                End If


            End If
        Next
        Me.Close()
    End Sub
    ' susano add 2016-03-15 end
    Private Sub AlloutPut()
        Call gYCM_MainFrame.FileCSVOut()
        'Dim strFileDotorh As String = ""
        'For Each objC3DCT As FBMlib.Common3DCodedTarget In MainFrm.objFBM.lstCommon3dCT
        '    If objC3DCT.lstRealP3d.Count > 0 Then
        '        strFileDotorh = strFileDotorh & objC3DCT.currentLabel & "," & objC3DCT.lstRealP3d.Item(0).X & "," & objC3DCT.lstRealP3d.Item(0).Y & "," & objC3DCT.lstRealP3d.Item(0).Z & vbNewLine
        '    End If

        'Next

        'For Each objC3DST As FBMlib.Common3DSingleTarget In MainFrm.objFBM.lstCommon3dST
        '    strFileDotorh = strFileDotorh & objC3DST.currentLabel & "," & objC3DST.realP3d.X  & "," & objC3DST.realP3d.Y & "," & objC3DST.realP3d.Z & vbNewLine
        'Next
        'My.Computer.FileSystem.WriteAllText(TextBox1.Text, strFileDotorh, False)
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If RadioButton1.Checked = True Then
            AlloutPut()
        End If
        If RadioButton2.Checked = True Then
            BunkatsuOutPut()
        End If

        Dim nameNotExists As Integer = 0
        Dim nameExists As Integer = 0
        For i As Integer = 0 To bunkatsuResultView.RowCount - 1
            Dim outputFileName As String = Me.bunkatsuResultView.Rows(i).Cells(3).Value
            If outputFileName = "" Then
                nameNotExists = nameNotExists + 1
            Else
                nameExists = nameExists + 1
            End If
        Next

        If nameExists >= 1 Then
            MsgBox("CSV出力完了しました。")
        Else
            MsgBox("出力ファイル名を入力してください")
        End If
        Me.Close()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        SFD1.InitialDirectory = m_koji_kanri_path
        If SFD1.ShowDialog = Windows.Forms.DialogResult.OK Then
            TextBox1.Text = SFD1.FileName
        End If
    End Sub
    Private Sub DataGridView2_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles bunkatsuResultView.CellEndEdit
        If (e.ColumnIndex = 0) Then
            Dim value As String = bunkatsuResultView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
            lstBunkatsu.Item(e.RowIndex).HeadStr = value
            bunkatsuResultView.Item(e.ColumnIndex, e.RowIndex).Value = value
        End If
    End Sub

End Class