Public Class YCM_ScaleSetting
    Dim cp1 As New CLookPoint
    Dim cp2 As New CLookPoint
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        If point_name.Count > 0 Then
            Me.Hide()
            If IOUtil.GetPoint(cp1, "点目を指示：") <> -1 Then
                Label_P1.Text = cp1.LabelName
                sys_ScaleInfo.p1 = cp1
                ComboBox1.Text = cp1.LabelName  '13.1.29　選択した計測点をComboBox1のTextに
                '選択した計測点をCommboBoxへ、イベント発生<ComboBox1.SelectedIndexChanged>
            End If
            Me.Show()
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If point_name.Count > 0 Then
            Me.Hide()
            If IOUtil.GetPoint(cp2, "点目を指示：") <> -1 Then
                Label_P2.Text = cp2.LabelName
                sys_ScaleInfo.p2 = cp2
                ComboBox2.Text = cp2.LabelName '13.1.29　選択した計測点をComboBox2に
                '選択した計測点をCommboBoxへ、イベント発生<ComboBox2.SelectedIndexChanged>
            End If
            Me.Show()
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Me.Close()
    End Sub

    Private Sub YCM_ScaleSetting_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        Dim intmax As Integer
        Label_P1.Text = sys_ScaleInfo.p1.LabelName
        Label_P2.Text = sys_ScaleInfo.p2.LabelName
        If point_name.Count > 0 Then '元からあった

            'ComboBoxに計測点を追加
            '13.1.29===========================================
            If nLookPoints <= 0 Then
                MsgBox("計測点がありません。", MsgBoxStyle.Critical)
                Me.Close()
                Exit Sub
            End If
            ComboBox1.Items.Add("") '初期値は空白
            ComboBox2.Items.Add("") '初期値は空白
            For n As Integer = 0 To point_name.Count - 1
                ComboBox1.Items.Add(gDrawPoints(n).LabelName)
                ComboBox2.Items.Add(gDrawPoints(n).LabelName)
            Next
            '13.1.29===========================================

            For i As Integer = 0 To nscalesetting - 1
                If System_scalesetting(0, i) <> 0 Then '長さ

                    CbBscalelong.Items.Add(System_scalesetting(0, i)) 'CbBscalelong：スケール値を入力するコンボボックス)
                End If

                intmax = i
            Next
            If sys_ScaleInfo.len <> 0 Then
                CbBscalelong.Text = sys_ScaleInfo.len.ToString
            Else
                If CbBscalelong.Items.Count > 0 Then
                    CbBscalelong.SelectedIndex = 0
                End If
            End If
            If nscalesetting <> 0 Then
                CbBscalelong.Text = System_scalesetting(0, intmax)
            Else
                CbBscalelong.Text = 1.0
            End If
        End If


    End Sub
    Public tmpFlag As Boolean = False  ' SUSANO ADD 20160606
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        'YCM_LabelResult.Show()
        If scaleInfo_Check() = -1 Then Exit Sub
        sys_ScaleInfo.len = CDbl(CbBscalelong.Text)
        Dim dis As Double
        Dim cp_temp As New GeoPoint

        '13.1.29修正================================================================================================================================
        Dim SP1, SP2 As New CLookPoint
        SP1 = sys_ScaleInfo.p1
        SP2 = sys_ScaleInfo.p2

        dis = SP1.distTo(SP2)
        sys_ScaleInfo.scale = CDbl(CbBscalelong.Text) / CDbl(dis)
        YCM_UpdataSystemscalesettingAcs(m_strDataBasePath, SP1, SP2, CDbl(CbBscalelong.Text), sys_ScaleInfo.scale)
        YCM_UpdataSystemscalevalueAcs(m_strDataBasePath, sys_ScaleInfo.scale)
        sys_Labeling.blnsyslabel = True

        YCM_UpDateLookPointReal()
        Dim arrlist As New ArrayList

        For qq As Integer = 0 To Data_Point.DGV_DV.Rows.Count - 1
            arrlist.Add(Data_Point.DGV_DV.Rows(qq).Cells(5).Value)
        Next

        'On Error Resume Next
        For ii = 0 To nLookPoints - 1
            For jj = 0 To Data_Point.DGV_DV.Rows.Count - 1
                If gDrawPoints(ii).mid = arrlist.Item(jj) Then
                    Data_Point.DGV_DV.Rows(jj).Cells(2).Value = gDrawPoints(ii).Real_x
                    Data_Point.DGV_DV.Rows(jj).Cells(3).Value = gDrawPoints(ii).Real_y
                    Data_Point.DGV_DV.Rows(jj).Cells(4).Value = gDrawPoints(ii).Real_z
                    Exit For
                End If
            Next
        Next

        binfirstscalesetting = True

        YCM_GenScaleLine(SP1, SP2)
        '13.1.29修正================================================================================================================================


        ''13.1.29以前=============================================================================================================================
        'dis = sys_ScaleInfo.p1.distTo(sys_ScaleInfo.p2)
        'sys_ScaleInfo.scale = CDbl(CbBscalelong.Text) / CDbl(dis)
        ''YCM_UpdataSystemscalesettingAcs(m_strDataBasePath, cp1, cp2, CDbl(CbBscalelong.Text), sys_ScaleInfo.scale)'元（13.1.29以前）

        'YCM_UpdataSystemscalesettingAcs(m_strDataBasePath, sys_ScaleInfo.p1, sys_ScaleInfo.p2, CDbl(CbBscalelong.Text), sys_ScaleInfo.scale)
        'YCM_UpdataSystemscalevalueAcs(m_strDataBasePath, sys_ScaleInfo.scale)
        'sys_Labeling.blnsyslabel = True

        'YCM_UpDateLookPointReal()
        'Dim arrlist As New ArrayList

        'For qq As Integer = 0 To Data_Point.DGV_DV.Rows.Count - 1
        '    arrlist.Add(Data_Point.DGV_DV.Rows(qq).Cells(5).Value)
        'Next

        ''On Error Resume Next
        'For ii = 0 To nLookPoints - 1
        '    For jj = 0 To Data_Point.DGV_DV.Rows.Count - 1
        '        If gDrawPoints(ii).mid = arrlist.Item(jj) Then
        '            Data_Point.DGV_DV.Rows(jj).Cells(2).Value = gDrawPoints(ii).Real_x
        '            Data_Point.DGV_DV.Rows(jj).Cells(3).Value = gDrawPoints(ii).Real_y
        '            Data_Point.DGV_DV.Rows(jj).Cells(4).Value = gDrawPoints(ii).Real_z
        '            Exit For
        '        End If
        '    Next
        'Next

        'binfirstscalesetting = True

        ''13.1.23 指定した2点（cp1,cp2）間に線分を作成
        ''YCM_GenScaleLine(cp1, cp2) '元（13.1.29以前）

        'YCM_GenScaleLine(sys_ScaleInfo.p1, sys_ScaleInfo.p2)
        ''13.1.29以前=============================================================================================================================
        Me.Close()

        'SUSANO ADD START 20160523
        If tmpFlag = True Then  ' SUSANO ADD 20160606
            If MsgBox("解析（３D）を実行しますか？" & vbNewLine & "(オフセット量の再計算と図形の再描画を行います。）", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                flgManualScaleAndOffset = True
            End If
        End If
       
        'SUSANO ADD END 20160524
    End Sub
    Private Function scaleInfo_Check() As Integer

        scaleInfo_Check = -1

        If sys_ScaleInfo.p1.LabelName = "" Then
            MsgBox("点1を選択してください。", MsgBoxStyle.Critical)
            Exit Function
        End If

        If sys_ScaleInfo.p2.LabelName = "" Then
            MsgBox("点2を選択してください。", MsgBoxStyle.Critical)
            Exit Function
        End If

        If nLookPoints = 0 Then
            MsgBox("計測データを開いてください。", MsgBoxStyle.Critical)
            Exit Function
        End If

        Dim blnFindP As Boolean = False
        For ii As Integer = 0 To nLookPoints - 1
            If gDrawPoints(ii).LabelName = sys_ScaleInfo.p1.LabelName Then
                blnFindP = True
                Exit For
            End If
        Next
        If blnFindP = False Then
            MsgBox("点1のラベルが見つかりませんので、点1を改めて選択してください。", MsgBoxStyle.Critical)
            Exit Function
        End If
        blnFindP = False
        For ii As Integer = 0 To nLookPoints - 1
            If gDrawPoints(ii).LabelName = sys_ScaleInfo.p2.LabelName Then
                blnFindP = True
                Exit For
            End If
        Next
        If blnFindP = False Then
            MsgBox("点2のラベルが見つかりませんので、点2を改めて選択してください。", MsgBoxStyle.Critical)
            Exit Function
        End If

        If sys_ScaleInfo.p1.LabelName = sys_ScaleInfo.p2.LabelName Then
            MsgBox("点1のラベル名と点2のラベル名は同じです。再選択してください。", MsgBoxStyle.Critical)
            Exit Function
        End If

        If CbBscalelong.Text = "" Or (IsNumeric(CbBscalelong.Text) = False) Then
            MsgBox("スケール長さに数値を入力してください。", MsgBoxStyle.Critical)
            Exit Function
        Else
            If CDbl(CbBscalelong.Text) <= 0 Then
                MsgBox("スケール長さに正整数を入力してください。", MsgBoxStyle.Critical)
                Exit Function
            End If
        End If
        scaleInfo_Check = 0
    End Function
    '13.1.29　ComboBox1の選択が変わったら
    Private Sub ComboBox1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged

        Dim strmark1 As String = ComboBox1.Text
        Dim CP1 As New CLookPoint
        Dim ind1 As Integer
        ind1 = GetPosFromLabelName(CLookPoint.posTypeMode.All, strmark1, CP1)
        If ind1 < 0 Then
            MsgBox("指定されたラベルに対応する計測点が見つかりません。" & vbCrLf & "ラベルを確認して下さい。", MsgBoxStyle.Critical)
            Exit Sub
        End If
        sys_ScaleInfo.p1 = CP1

    End Sub
    '13.1.29　ComboBox2の選択が変わったら
    Private Sub ComboBox2_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged

        Dim strmark2 As String = ComboBox2.Text
        Dim CP2 As New CLookPoint
        Dim ind2 As Integer
        ind2 = GetPosFromLabelName(CLookPoint.posTypeMode.All, strmark2, CP2)
        If ind2 < 0 Then
            MsgBox("指定されたラベルに対応する計測点が見つかりません。" & vbCrLf & "ラベルを確認して下さい。", MsgBoxStyle.Critical)
            Exit Sub
        End If
        sys_ScaleInfo.p2 = CP2
    End Sub

End Class