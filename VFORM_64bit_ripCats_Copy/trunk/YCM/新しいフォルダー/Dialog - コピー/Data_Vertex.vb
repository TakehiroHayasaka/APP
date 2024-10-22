﻿Public Class Data_Vertex
    Friend Data_Vertex_Dock As Integer = 1
    Dim Flg_Child As Integer
    Dim Flg_Single As Integer
    Dim Flg_Scale As Integer
    Dim Sort_Flg As Integer = 0



    Private Sub Data_Vertex_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        binfrmdataview = True
        If binfrmdataviewclosed = True Then
            For i As Long = 0 To nLookPoints - 1

                '(20151207 Tezuka ADD) 小番号の表示・非表示
                If Combo_Child.SelectedIndex = 1 Then
                    If gDrawPoints(i).LabelName.IndexOf("_"c) > 0 Then
                        Continue For
                    End If
                End If

                '(20151207 Tezuka ADD) STの表示・非表示
                If Combo_Single.SelectedIndex = 1 Then
                    If gDrawPoints(i).LabelName.Substring(0, 2) = "ST" Then
                        Continue For
                    End If
                End If

                Data_Point.DGV_DV.Rows.Add()
                If gDrawPoints(i).blnDraw Then
                    Data_Point.DGV_DV.Rows(i).Cells(0).Value = True
                Else
                    Data_Point.DGV_DV.Rows(i).Cells(0).Value = False
                End If
                Data_Point.DGV_DV.Rows(i).Cells(1).Value = gDrawPoints(i).LabelName
                Data_Point.DGV_DV.Rows(i).Cells(2).Value = gDrawPoints(i).Real_x
                Data_Point.DGV_DV.Rows(i).Cells(3).Value = gDrawPoints(i).Real_y
                Data_Point.DGV_DV.Rows(i).Cells(4).Value = gDrawPoints(i).Real_z
                ' Data_Point.DGV_DV.Rows(i).Cells(5).Value = gDrawPoints(i).mid
                Dim strTid As String = Trim(gDrawPoints(i).tid)
                If strTid.Count = 1 Then
                    Data_Point.DGV_DV.Rows(i).Cells(5).Value = "0" & gDrawPoints(i).tid
                Else
                    Data_Point.DGV_DV.Rows(i).Cells(5).Value = gDrawPoints(i).tid
                End If
            Next
            Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
            '--rep.start-----------------------12.10.17
#If 1 Then
            '--            MainFrm.SplitContainer3.SplitterDistance = MainFrm.SplitContainer3.Width - 405
            MainFrm.SplitContainer3.SplitterDistance = MainFrm.SplitContainer2.Panel2.Width
            Me.TopLevel = False
            Me.Location = New System.Drawing.Point(0, 0)
            View_Dilaog.Size = New System.Drawing.Size(MainFrm.SplitContainer4.Panel1.Width, MainFrm.SplitContainer4.Panel1.Height)
            '--            Me.Size = New System.Drawing.Size(400, MainFrm.SplitContainer1.Panel2.Height)
            Me.Size = New System.Drawing.Size(400, 585)
            MainFrm.SplitContainer3.Panel2.Controls.Add(Me)
#Else
            MainFrm.SplitContainer3.SplitterDistance = MainFrm.SplitContainer3.Width - 405
            Me.TopLevel = False
            Me.Location = New System.Drawing.Point(0, 0)
            YCM_3DView.Size = New System.Drawing.Size(MainFrm.SplitContainer4.Panel1.Width, MainFrm.SplitContainer4.Panel1.Height)
            Me.Size = New System.Drawing.Size(400, MainFrm.SplitContainer1.Panel2.Height)
            MainFrm.SplitContainer3.Panel2.Controls.Add(Me)
#End If
            '--rep.end-------------------------12.10.17
        End If
        binfrmdataviewclosed = False
        With DGV_DV
            .AllowUserToAddRows = False
            .RowHeadersWidth = 15
            .DefaultCellStyle.Format = "0.000"
            .Columns(1).ReadOnly = False    'ラベル欄だけ編集可能にする
            .Columns(2).ReadOnly = True
            .Columns(3).ReadOnly = True
            .Columns(4).ReadOnly = True
        End With
        Flg_Child = 0
        Flg_Single = 0
        Flg_Single = 0
        '20160621 Add Kiryu CT小番号表示しない　ST表示しない　定規表示しない　をデフォルトに変更
        '20160621 Add Kiryu CT小番号表示する　ST表示する　定規表示する　をデフォルトに変更 
        Combo_Child.SelectedIndex = 0
        Combo_Single.SelectedIndex = 0
        Combo_Scale.SelectedIndex = 0

        For Each c As DataGridViewColumn In DGV_DV.Columns
            c.SortMode = DataGridViewColumnSortMode.NotSortable
        Next c
        'Sort_Flg = 0
    End Sub

    Private Sub Btn_Hochi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Hochi.Click
        If Data_Vertex_Dock = 1 Then
            Data_Vertex_Dock = 0
            Dim temp_data As New ArrayList
            For i As Integer = 0 To DGV_DV.RowCount - 1
                temp_data.Add(DGV_DV.Rows(i).Cells(0).Value)
                DGV_DV.Rows(i).Cells(0).Value = False
            Next
            Me.Hide()
            '--rep.start-----------------------12.10.17
#If 1 Then
            MainFrm.SplitContainer3.Panel2.Controls.Clear()
            MainFrm.SplitContainer3.SplitterDistance = MainFrm.SplitContainer3.Width
            View_Dilaog.Size = New System.Drawing.Size(MainFrm.SplitContainer4.Panel1.Width, MainFrm.SplitContainer4.Panel1.Height)
#Else
            MainFrm.SplitContainer1.Panel2.Controls.Clear()
            MainFrm.SplitContainer1.SplitterDistance = MainFrm.SplitContainer1.Width
            YCM_3DView.Size = New System.Drawing.Size(MainFrm.SplitContainer2.Panel1.Width, MainFrm.SplitContainer2.Panel1.Height)
#End If
            '--rep.end-------------------------12.10.17
            Me.TopLevel = True
            Me.Location = New System.Drawing.Point(104, 7)
            Me.Size = New System.Drawing.Size(400, 585)
            Me.Btn_Hochi.Text = "結合"
            Me.Show()
            For i As Integer = 0 To DGV_DV.RowCount - 1
                DGV_DV.Rows(i).Cells(0).Value = temp_data(i)
            Next
            Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedDialog
        Else
            Data_Vertex_Dock = 1
            Me.Hide()
            Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
            'Me.Size = New System.Drawing.Size(400, 585)
            '--rep.start-----------------------12.10.17
#If 1 Then
            MainFrm.SplitContainer3.SplitterDistance = MainFrm.SplitContainer3.Width - 405
            Me.TopLevel = False
            Me.Location = New System.Drawing.Point(0, 0)
            View_Dilaog.Size = New System.Drawing.Size(MainFrm.SplitContainer4.Panel1.Width, MainFrm.SplitContainer4.Panel1.Height)
            Me.Size = New System.Drawing.Size(400, MainFrm.SplitContainer3.Panel2.Height)
            MainFrm.SplitContainer3.Panel2.Controls.Add(Me)
#Else
            MainFrm.SplitContainer1.SplitterDistance = MainFrm.SplitContainer1.Width - 405
            Me.TopLevel = False
            Me.Location = New System.Drawing.Point(0, 0)
            YCM_3DView.Size = New System.Drawing.Size(MainFrm.SplitContainer2.Panel1.Width, MainFrm.SplitContainer2.Panel1.Height)
            Me.Size = New System.Drawing.Size(400, MainFrm.SplitContainer1.Panel2.Height)
            MainFrm.SplitContainer1.Panel2.Controls.Add(Me)
#End If
            '--rep.end-------------------------12.10.17
            Me.Btn_Hochi.Text = "分離"
            Me.Show()
        End If
    End Sub

    Private Sub Btn_AllSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_AllSelect.Click
        Dim ii As Long
        For ii = 0 To DGV_DV.Rows.Count - 1
            DGV_DV.Rows(ii).Cells(0).Value = True
        Next
        For ii = 0 To nRays - 1
            gDrawRays(ii).blnDraw = True
        Next
        For ii = 0 To nLookPoints - 1
            gDrawPoints(ii).blnDraw = True
        Next
        For ii = 0 To nLabelText - 1
            gDrawLabelText(ii).blnDraw = True
        Next
    End Sub

    Private Sub Btn_AllKaijyo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_AllKaijyo.Click
        Dim ii As Long
        For ii = 0 To DGV_DV.Rows.Count - 1
            DGV_DV.Rows(ii).Cells(0).Value = False
        Next
        For ii = 0 To nRays - 1
            gDrawRays(ii).blnDraw = False
        Next
        For ii = 0 To nLookPoints - 1
            gDrawPoints(ii).blnDraw = False
        Next
        For ii = 0 To nLabelText - 1
            gDrawLabelText(ii).blnDraw = False
        Next
    End Sub
    '13.1.17 CSV出力（座標値リスト）

    Private Sub Btn_CSOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_CSOut.Click
        Call gYCM_MainFrame.FileCSVOut()
    End Sub

    Private Sub Data_Vertex_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        Me.Hide()
        binfrmdataview = False
        '--del.rep.start-------------------------12.10.17
        ' リボンメニューに表示／非表示の状態を反映
#If 0 Then
        MainFrm.MM_V_12.Checked = False
#End If
        '--del.rep.end---------------------------12.10.17
        binfrmdataviewclosed = True
        'binarrdata = New ArrayList
        'For ii As Integer = 0 To DGV_DV.RowCount - 1
        '    binarrdata.Add(DGV_DV.Rows(ii).Cells(0).Value)
        '    binarrdata.Add(DGV_DV.Rows(ii).Cells(1).Value)
        '    binarrdata.Add(DGV_DV.Rows(ii).Cells(2).Value)
        '    binarrdata.Add(DGV_DV.Rows(ii).Cells(3).Value)
        '    binarrdata.Add(DGV_DV.Rows(ii).Cells(4).Value)
        '    binarrdata.Add(DGV_DV.Rows(ii).Cells(5).Value)
        'Next
    End Sub

    Private Sub DGV_DV_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DGV_DV.CellContentClick
        bindataviewclickmode = True

        If e.ColumnIndex = 1 And e.RowIndex = -1 Then
            If Sort_Flg = 0 Then
                DGV_DV.Sort(DGV_DV.Columns(6), System.ComponentModel.ListSortDirection.Ascending)
                Sort_Flg = 1
            Else
                DGV_DV.Sort(DGV_DV.Columns(6), System.ComponentModel.ListSortDirection.Descending)
                Sort_Flg = 0
            End If
            'DGV_DV.Refresh()
        End If

        If e.ColumnIndex = 0 And e.RowIndex > -1 Then
            intdataviewdraw = e.RowIndex
            Dim pid As Integer, ii As Long, bDraw As Boolean
            bDraw = DGV_DV(0, e.RowIndex).EditedFormattedValue
            pid = CInt(DGV_DV(5, e.RowIndex).Value)
            For ii = 0 To nRays - 1
                If gDrawRays(ii).PointID = pid Then
                    gDrawRays(ii).blnDraw = bDraw  'Not gDrawRays(ii).blnDraw
                    Exit For
                End If
            Next
            For ii = 0 To nLookPoints - 1
                If gDrawPoints(ii).mid = pid Then
                    gDrawPoints(ii).blnDraw = bDraw  'Not gDrawPoints(ii).blnDraw
                    Exit For
                End If
            Next
            For ii = 0 To nLabelText - 1
                If gDrawLabelText(ii).mid = pid Then
                    gDrawLabelText(ii).blnDraw = bDraw  'Not gDrawLabelText(ii).blnDraw
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Sub DGV_DV_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DGV_DV.CellEndEdit
        '--    MsgBox("cellEndEdit Col[" & e.ColumnIndex & "][" & e.RowIndex & "]")
        '座標値一覧でラベルを変更された場合、データベースと表示ラベルの変更が必要

        Dim strIDArr As New StringArray
        Dim strLabelArr As New StringArray
        'gDrawLabelText(cp1.sortID).LabelName = strLabelNew
        'gDrawPoints(cp1.sortID).LabelName = strLabelNew
        'Data_Point.DGV_DV.Rows(cp1.sortID).Cells(1).Value = strLabelNew
        'strIDArr.Append(CStr(cp1.mid))
        'strLabelArr.Append(strLabelNew)

        Dim bDraw As Boolean
        bDraw = DGV_DV.Rows(e.RowIndex).Cells(0).Value
        bDraw = DGV_DV(0, e.RowIndex).EditedFormattedValue

        Dim pid As Integer, strLabelNew As String, ii As Long
        pid = CInt(DGV_DV(5, e.RowIndex).Value)
        strLabelNew = (DGV_DV(1, e.RowIndex).Value)
        '--ins.start----------------------------13.1.9
        Dim bIsHan As Boolean
        bIsHan = isHankakuText(strLabelNew)
        If (bIsHan = False) Then
            IOUtil.WritePrompt("ラベルは、半角文字で入力してください")
            '変更を元に戻す

            For ii = 0 To nLookPoints - 1
                If gDrawPoints(ii).mid = pid Then
                    Data_Point.DGV_DV.Rows(ii).Cells(1).Value = gDrawPoints(ii).LabelName
                End If
            Next
            Exit Sub
        End If
        '--ins.end------------------------------13.1.9

        For ii = 0 To nLookPoints - 1
            If gDrawPoints(ii).mid = pid Then
                gDrawPoints(ii).LabelName = strLabelNew
                Exit For
            End If
        Next
        For ii = 0 To nLabelText - 1
            If gDrawLabelText(ii).mid = pid Then
                gDrawLabelText(ii).LabelName = strLabelNew
                Exit For
            End If
        Next
        ''変更したラベル情報で「measurepoint3d」テーブルを更新
        strIDArr.Append(CStr(pid))
        strLabelArr.Append(strLabelNew)
        Call YCM_UpdataUserLabel(m_strDataBasePath, strIDArr, strLabelArr)

        Debug.Print("CellEndEdit[" & CStr(pid) & "][" & strLabelNew & "]")
    End Sub

    Public Sub New()

        ' この呼び出しはデザイナーで必要です。

        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。


    End Sub

    '(20151207 Tezuka ADD)
    Private Sub Combo_Child_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles Combo_Child.SelectedIndexChanged
        If Flg_Child > 0 Then
            YMC_3DViewReDraw(m_strDataBasePath)
            'View_Dilaog.MainProc()
        Else
            Flg_Child = 1
        End If
    End Sub

    '(20151207 Tezuka ADD)
    Private Sub Combo_Single_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles Combo_Single.SelectedIndexChanged
        If Flg_Single > 0 Then
            YMC_3DViewReDraw(m_strDataBasePath)
            'View_Dilaog.MainProc()
        Else
            Flg_Single = 1
        End If
    End Sub

    '(20160714 Tezuka ADD)
    Private Sub Combo_Scale_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles Combo_Scale.SelectedIndexChanged
        If Flg_Scale > 0 Then
            YMC_3DViewReDraw(m_strDataBasePath)
            'View_Dilaog.MainProc()
        Else
            Flg_Scale = 1
        End If
    End Sub

    Public Function Get_Data_Vertex_View_Withe()
        Return Me.DGV_DV.Size.Width
    End Function

    Private Sub DGV_DV_CellClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DGV_DV.CellClick
    End Sub

    Private Sub DGV_DV_CellMouseDown(sender As System.Object, e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DGV_DV.CellMouseDown

        For ii As Long = 0 To nLabelText - 1
            gDrawLabelText(ii).HighlightFlag = False
        Next

    End Sub

    Private Sub DGV_DV_CellMouseUp(sender As System.Object, e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DGV_DV.CellMouseUp

        For Each c As DataGridViewCell In DGV_DV.SelectedCells

            For ii As Long = 0 To nLabelText - 1
                If TypeOf c.Value Is String Then
                    If gDrawLabelText(ii).LabelName = c.Value Then
                        gDrawLabelText(ii).HighlightFlag = True
                    End If
                End If
            Next

        Next c

		
    End Sub

    '20170302 baluu add start
    Private Sub DGV_DV_MouseUp(sender As Object, e As MouseEventArgs) Handles DGV_DV.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Dim menu As ContextMenuStrip = New ContextMenuStrip
            Dim hideItem As ToolStripItem = menu.Items.Add("Hide")
            hideItem.Tag = 1
            AddHandler hideItem.Click, AddressOf HidePoints
            menu.Show(sender, e.Location)
        End If

    End Sub

    Private Sub HidePoints()
        DGV_DV.BeginEdit(False)
        If DGV_DV.SelectedRows.Count > 0 Then
            For i As Integer = 0 To DGV_DV.SelectedRows.Count - 1 Step 1
                DGV_DV.SelectedRows(i).Cells(0).Value = False
                DGV_DV.UpdateCellValue(0, DGV_DV.SelectedRows(i).Index)
                gDrawPoints(DGV_DV.SelectedRows(i).Index).blnDraw = False
            Next
        ElseIf DGV_DV.SelectedCells.Count > 0 Then
            For i As Integer = 0 To DGV_DV.SelectedCells.Count - 1 Step 1
                DGV_DV.SelectedCells(i).OwningRow.Cells(0).Value = False
                DGV_DV.UpdateCellValue(0, DGV_DV.SelectedCells(i).OwningRow.Index)
                gDrawPoints(DGV_DV.SelectedCells(i).OwningRow.Index).blnDraw = False
            Next
        End If
        
        DGV_DV.EndEdit()
        DGV_DV.ClearSelection()
    End Sub
    '20170302 baluu add end

End Class

