'YCMコマンドのインターフェース用
Module YCM_CmdIF
#If 0 Then '---昔のメニュー

#Region "メニュー関連(画像)"
    Private Sub ToolStripButton13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgToolStripButton1.Click, ToolStripButton13.Click
        If flgPreview = True Then
            flgPreview = False
            Panel1.Visible = False
            FourImageTableLayoutPanel.Visible = True
            '   Me.Text = "３画像表示"
        Else
            flgPreview = True
            Panel1.Visible = True
            FourImageTableLayoutPanel.Visible = False
            ' Me.Text = "画像プレビュー"
        End If
    End Sub

    Private Sub ToolStripButton14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgToolStripButton2.Click
        FBMlib.T_treshold = CInt(imgToolStripTextBox1.Text)
        objFBM.DetectTargetsAllImages()
        flgDispVal = flgDisp.flgCT
        MsgBox("ターゲット抽出処理完了しました。", MsgBoxStyle.OkOnly, "確認")
    End Sub
    Private Sub imgToolStripButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgToolStripButton3.Click
        If objFBM.ProjectPath = "" Then
            MsgBox("画像データがありません。！", MsgBoxStyle.OkOnly, "確認")
            Exit Sub
        End If
        If MsgBox("一括解析を実行しますか？", MsgBoxStyle.OkCancel, "確認") = MsgBoxResult.Ok Then

            frmProgressBar.Show()
            frmProgressBar.Top = Me.Top
            frmProgressBar.Left = Me.Left
            frmProgressBar.ProgressBar1.Maximum = 100
            frmProgressBar.ProgressBar1.Value = 0
            frmProgressBar.Label1.Text = "ターゲット抽出中"
            Me.Refresh()
            frmProgressBar.Refresh()
            Dim sw As New System.Diagnostics.Stopwatch()
            sw.Start()
            FBMlib.T_treshold = CDbl(imgToolStripTextBox1.Text)
            objFBM.IkkatuSyori2()
            objFBM.SaveToMeasureDataDB(objFBM.ProjectPath & "\")
            sw.Stop()
            YMC_3DViewReDraw(m_strDataBasePath)

            frmProgressBar.Close()
            Main_Tab.SelectTab(1)

            ' MsgBox("一括処理完了しました。(" & sw.Elapsed.TotalSeconds.ToString & "秒)", MsgBoxStyle.OkOnly, "確認")
        End If
    End Sub

    Private Sub imgToolStripComboBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgToolStripComboBox1.SelectedIndexChanged
        Select Case imgToolStripComboBox1.SelectedIndex
            Case 0
                flgDispVal = flgDisp.flgFeature
            Case 1
                flgDispVal = flgDisp.flgCT
            Case 2
                flgDispVal = flgDisp.flgObject
        End Select
    End Sub

    Private Sub imgToolStripButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgToolStripButton4.Click
        flgDraw = True
        Dim i As Integer = ImageListView.SelectedIndices.Item(0)

        Dim DelRegion As HObject = Nothing
        HOperatorSet.GenEmptyObj(DelRegion)
        HOperatorSet.ClearObj(DelRegion)

        DrawDeleteRegion(winpreview, DelRegion)
        objFBM.DeletePoint(i, DelRegion)

        DispOneObjByIndex(winpreview, i)
        HOperatorSet.ClearObj(DelRegion)
        objFBM.SaveToMeasureDataDB(objFBM.ProjectPath & "\")
        YMC_3DViewReDraw(m_strDataBasePath)
Exit_Sub:
        flgDraw = False
    End Sub

    Private Sub ToolStripButton13_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton13.Click
        Dim sw As New System.Diagnostics.Stopwatch()
        sw.Start()
        FBMlib.T_treshold = CDbl(imgToolStripTextBox1.Text)
        objFBM.RunBundleAdjOnly()
        objFBM.SaveToMeasureDataDB(objFBM.ProjectPath & "\")
        sw.Stop()
        YMC_3DViewReDraw(m_strDataBasePath)
        MsgBox("バンドル調整処理完了しました。(" & sw.Elapsed.TotalSeconds.ToString & "秒)", MsgBoxStyle.OkOnly, "確認")
    End Sub
    Private Sub imgToolStripButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgToolStripButton6.Click
        Dim sw As New System.Diagnostics.Stopwatch()
        sw.Start()
        FBMlib.T_treshold = CDbl(imgToolStripTextBox1.Text)
        objFBM.SingleTargetNumberingOnly()
        objFBM.SaveToMeasureDataDB(objFBM.ProjectPath & "\")
        sw.Stop()
        YMC_3DViewReDraw(m_strDataBasePath)
        MsgBox("シングルターゲット番号付け処理完了しました。(" & sw.Elapsed.TotalSeconds.ToString & "秒)", MsgBoxStyle.OkOnly, "確認")
    End Sub

    Private Sub MM_F_7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MM_F_7.Click
        'VBMファイルの読込み
    End Sub
#End Region
#End If
#Region "メニュー関連"

#End Region

End Module