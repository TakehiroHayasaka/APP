Imports FBMlib

Public Class frmAutoCalcOffset

    Dim lstNearestST As New List(Of NearestST)

    Private Sub frmAutoCalcOffset_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        txtMAX.Text = My.Settings.OffsetMax
        txtMIN.Text = My.Settings.OffsetMin
        txtFileName.Text = My.Settings.OffsetOutput

        Dim col1 = New DataGridViewCheckBoxColumn()
        'Dim chk As New CheckBox
        'chk = DataGridView1.Rows.Item(0).Cells(0).Value
        col1.Width = 70
        col1.Name = "colCheck"
        col1.HeaderText = "出カ"
        'chk.Checked = True
        DataGridView1.Columns.Add(col1)

        Dim col2, col3, col5, col6, col7 As New DataGridViewTextBoxColumn
        'col2.DataPropertyName = "PropertyName"
        col2.HeaderText = "CT"
        col2.Name = "colCT"
        DataGridView1.Columns.Add(col2)

        col3.HeaderText = "ST"
        col3.Name = "colST"
        DataGridView1.Columns.Add(col3)

        Dim col4btn As DataGridViewButtonColumn = New DataGridViewButtonColumn()
        col4btn.HeaderText = "ST再選択"
        col4btn.Name = "colbtn"
        col4btn.Text = "選択"
        col4btn.UseColumnTextForButtonValue = True
        DataGridView1.Columns.Add(col4btn)

        col5.HeaderText = "OX"
        col5.Name = "colOX"
        DataGridView1.Columns.Add(col5)

        col6.HeaderText = "OY"
        col6.Name = "colOY"
        DataGridView1.Columns.Add(col6)

        col7.HeaderText = "OZ"
        col7.Name = "colOZ"
        DataGridView1.Columns.Add(col7)

        DataGridView1.AllowUserToAddRows = False
        For i = 0 To DataGridView1.Columns.Count - 1
            DataGridView1.Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
            'DataGridView1.Columns(i).ReadOnly = True
        Next

    End Sub

    Private Sub btnFileString_Click(sender As Object, e As EventArgs) Handles btnFileString.Click

        Dim saveFD1 As New SaveFileDialog()
        saveFD1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*"

        'saveFileDialog1.FilterIndex = 2
        'saveFileDialog1.RestoreDirectory = True
        Try
            If saveFD1.ShowDialog() = Windows.Forms.DialogResult.OK Then

            End If
        Catch ex As Exception

        End Try


        If saveFD1.FileName <> "" Then

            Me.txtFileName.Text = saveFD1.FileName


        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If My.Computer.FileSystem.FileExists(Me.txtFileName.Text) Then
            Dim result = MessageBox.Show("Already exists. Do you want to replace it?", "", MessageBoxButtons.YesNo)
            If result = Windows.Forms.DialogResult.No Then
                Exit Sub
            End If
        End If

        Try
            Dim csvFile As String = Me.txtFileName.Text
            Dim outFile As IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(csvFile, False)

            outFile.WriteLine("CT_NO, flg_Use, m_x, m_y, m_z")

            For Each row As DataGridViewRow In DataGridView1.Rows
                If row.Cells(0).Value = True Then
                    Dim CTString As String = row.Cells(1).Value
                    Dim CTstr As String = CTString.Substring(2)
                    outFile.WriteLine("" & CTstr & ", 1, " & row.Cells(4).Value.ToString & ", " & row.Cells(5).Value.ToString & ", " & (row.Cells(6).Value * (-1)).ToString & "")
                End If
            Next

            outFile.Close()
            MsgBox("出力しました。")

        Catch ex As Exception

            MsgBox("using another process...")

        End Try
        

        'Console.WriteLine(My.Computer.FileSystem.ReadAllText(csvFile))
        'Process.Start(Me.txtFileName.Text)

    End Sub

    Private Sub btnCose_Click(sender As Object, e As EventArgs) Handles btnClose.Click

        My.Settings.OffsetMax = txtMAX.Text
        My.Settings.OffsetMin = txtMIN.Text
        My.Settings.OffsetOutput = txtFileName.Text
        My.Settings.Save()
        Me.Close()

    End Sub

    Private Sub dataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick

        If e.ColumnIndex = DataGridView1.Columns(3).Index And e.RowIndex >= 0 Then
            Dim posOrg As New CLookPoint
            Me.Hide()
            If IOUtil.GetPoint(posOrg, "点目を指示：") <> -1 Then

                For Each ST As Common3DSingleTarget In MainFrm.objFBM.lstCommon3dST
                    If ST.PID = posOrg.tid - 10000 Then
                        lstNearestST.Item(e.RowIndex).ST = ST
                        lstNearestST.Item(e.RowIndex).CalcOffset()
                        Exit For
                    End If
                Next

                SetControlFromData()
            End If
            Me.Show()
        End If

    End Sub

    Private Sub btnRun_Click(sender As Object, e As EventArgs) Handles btnRun.Click
        DataGridView1.Rows.Clear()
        Dim dblMin As Double = CDbl(txtMIN.Text)
        Dim dblMax As Double = CDbl(txtMAX.Text)
        If lstNearestST Is Nothing Then
            lstNearestST = New List(Of NearestST)
        Else
            lstNearestST.Clear()
        End If
        FindNearestST(dblMin, dblMax, lstNearestST)

        If lstNearestST.Count > 0 Then
            For Each nrst As NearestST In lstNearestST
                nrst.CalcOffset()
            Next

            Me.txtMIN.Enabled = False
            Me.txtMAX.Enabled = False
            Me.txtFileName.Enabled = False
            Me.btnFileString.Enabled = False
            Me.btnSave.Enabled = False
            Me.btnClose.Enabled = False

            SetControlFromData()

            Me.txtMIN.Enabled = True
            Me.txtMAX.Enabled = True
            Me.txtFileName.Enabled = True
            Me.btnFileString.Enabled = True
            Me.btnSave.Enabled = True
            Me.btnClose.Enabled = True

        End If

    End Sub

    Private Sub SetControlFromData()
        DataGridView1.Rows.Clear()
        For Each nrst As NearestST In lstNearestST
            DataGridView1.Rows.Add({True, nrst.CT.currentLabel, nrst.ST.currentLabel, True, nrst.objOFFset.X, nrst.objOFFset.Y, nrst.objOFFset.Z})
        Next
    End Sub

    Private Sub FindNearestST(ByVal dblMin As Double, ByVal dblMax As Double, ByRef lstNearestST As List(Of NearestST))
        Dim lstCT As List(Of Common3DCodedTarget) = MainFrm.objFBM.lstCommon3dCT
        Dim lstST As List(Of Common3DSingleTarget) = MainFrm.objFBM.lstCommon3dST

        If lstNearestST Is Nothing Then
            lstNearestST = New List(Of NearestST)
        Else
            lstNearestST.Clear()
        End If

        For Each CT As Common3DCodedTarget In lstCT

            If CT.lstRealP3d.Count > 0 Then
                Dim mindist As Double = Double.MaxValue
                Dim NearST As New Common3DSingleTarget

                For Each ST As Common3DSingleTarget In lstST

                    Dim dblCTtoST As Double
                    CT.lstRealP3d.Item(0).GetDisttoOtherPose(ST.realP3d, dblCTtoST)
                    If dblMin < dblCTtoST And dblCTtoST < dblMax Then
                        If mindist > dblCTtoST Then
                            mindist = dblCTtoST
                            NearST = ST
                        End If
                    End If
                Next

                If dblMin < mindist And mindist < dblMax Then
                    Dim cc As New NearestST
                    cc.ST = NearST
                    cc.CT = CT
                    If cc.Check3Ten = True Then
                        lstNearestST.Add(cc)
                    End If
                    'Else
                End If
            End If
            
        Next

    End Sub

    Private Class NearestST
        Public CT As Common3DCodedTarget
        Public ST As Common3DSingleTarget
        Public objOFFset As New Point3D
        Public Sub CalcOffset()
            Dim CSO As Point3D = CT.lstRealP3d(0)
            Dim minusX As Point3D = CT.lstRealP3d(1)
            Dim plusY As Point3D = CT.lstRealP3d(2)
            Dim iRet As Integer
            Dim mat() As Double

            iRet = YCM_Get3PosMatrix(New YCM.CLookPoint(CSO.X, CSO.Y, CSO.Z), New YCM.CLookPoint(minusX.X, minusX.Y, minusX.Z), New YCM.CLookPoint(plusY.X, plusY.Y, plusY.Z), -1, 2, 1, mat)

            Dim tmpLookPoint As New YCM.CLookPoint(ST.realP3d.X, ST.realP3d.Y, ST.realP3d.Z)
            YCM_LookPointMatPoint(tmpLookPoint, mat)
            objOFFset.X = tmpLookPoint.Real_x
            objOFFset.Y = tmpLookPoint.Real_y
            objOFFset.Z = tmpLookPoint.Real_z
            ' YCM_Get3PosMatrix()

        End Sub
        Public Function Check3Ten() As Boolean
            Check3Ten = False
            Dim CSO As New GeoPoint
            CSO.setXYZ(CT.lstRealP3d(0).X, CT.lstRealP3d(0).Y, CT.lstRealP3d(0).Z)

            Dim minusX As New GeoPoint
            minusX.setXYZ(CT.lstRealP3d(1).X, CT.lstRealP3d(1).Y, CT.lstRealP3d(1).Z)

            Dim plusY As New GeoPoint
            plusY.setXYZ(CT.lstRealP3d(2).X, CT.lstRealP3d(2).Y, CT.lstRealP3d(2).Z)

            Dim cso_x As New GeoCurve
            cso_x.SetLine(CSO, minusX)
            Dim geofoot As New GeoPoint
            geofoot = cso_x.GetPerpendicularFoot(plusY)
            If geofoot.GetDistanceTo(plusY) > 2 Then
                Check3Ten = True
            End If

        End Function
    End Class

End Class