Imports Microsoft.VisualBasic.FileIO

Public Class YCM_Change
    Dim iAxis1, iAxis2, iFixPos, dblMat As Integer
    Dim iAxis1_db, iAxis2_db As Integer


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_O.Click
        If point_name.Count > 0 Then
            Dim posOrg As New CLookPoint
            Me.Hide() '20160926 ToDo Kiryu ハイド中に座標系設定ボタンを押すと.netエラーが発生するため修正が必要
            If IOUtil.GetPoint(posOrg, "点目を指示：") <> -1 Then
                Label_O.Text = posOrg.LabelName
                sys_CoordInfo.pOrg = posOrg
                COMB_0.Text = posOrg.LabelName '13.1.29 選択した原点をCOMB_0のTextに
                '<COMB_0.SelectedIndexChanged>イベントに


            End If

            Me.Show()
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_P1.Click
        Dim pos1 As New CLookPoint
        If point_name.Count > 0 Then
            Me.Hide()
            If IOUtil.GetPoint(pos1, "点目を指示：") <> -1 Then
                Label_P1.Text = pos1.LabelName
                sys_CoordInfo.p1 = pos1
                COMB_1.Text = pos1.LabelName '13.1.29 選択した点1をCOMB_1のTextに
                '<COMB_1.SelectedIndexChanged>イベントに

            End If
            Me.Show()
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_P2.Click
        Dim pos2 As New CLookPoint
        If point_name.Count > 0 Then
            Me.Hide()
            If IOUtil.GetPoint(pos2, "点目を指示：") <> -1 Then
                Label_P2.Text = pos2.LabelName
                sys_CoordInfo.p2 = pos2
                COMB_2.Text = pos2.LabelName '13.1.29 選択したCOMB_2のTextに
                '<COMB_2.SelectedIndexChanged>イベントに

            End If
            Me.Show()
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Me.Close()
    End Sub

    Private Sub YCM_Change_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim CT_offset As Integer = GetPrivateProfileInt("Command", "CT_offset", 0, My.Application.Info.DirectoryPath & "\vform.ini")
        'オフセット点・オフセット量算出ボタン表示切り替え　0:非表示　1：表示
        If CT_offset <> 1 Then
            '#If CT_offset <> 1 Then
            Label4.Visible = False
            Button_OffsetP.Visible = False
            COMB_3.Visible = False
            Button1.Visible = False
            '#End If
        End If
        '13.1.29============================================
        If nLookPoints <= 0 Then
            MsgBox("計測点がありません。", MsgBoxStyle.Critical)
            Me.Close()
            Exit Sub
        End If
        'ComboBoxに計測点を追加
        COMB_0.Items.Add("") '1行目は空白
        COMB_1.Items.Add("") '1行目は空白
        COMB_2.Items.Add("") '1行目は空白
        For n As Integer = 0 To nLookPoints - 1
            COMB_0.Items.Add(gDrawPoints(n).LabelName)
            COMB_1.Items.Add(gDrawPoints(n).LabelName)
            COMB_2.Items.Add(gDrawPoints(n).LabelName)
        Next

        COMB_0.Text = sys_CoordInfo.pOrg.LabelName
        COMB_1.Text = sys_CoordInfo.p1.LabelName
        COMB_2.Text = sys_CoordInfo.p2.LabelName
        '13.1.29============================================

        '13.1.28以前==================================
        'Label_O.Text = sys_CoordInfo.pOrg.LabelName
        'Label_P1.Text = sys_CoordInfo.p1.LabelName
        'Label_P2.Text = sys_CoordInfo.p2.LabelName
        '13.1.28以前==================================

        '軸方向の設定
        ComboBox_1.Text = sys_CoordInfo.strP1XYZ
        ComboBox_2.Text = sys_CoordInfo.strP2XYZ

        If sys_CoordInfo.intOxyais = 1 Then
            RadioButton_1.Checked = True
        Else
            RadioButton_2.Checked = True
        End If
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click

        sys_CoordInfo.strP1XYZ = ComboBox_1.Text
        sys_CoordInfo.strP2XYZ = ComboBox_2.Text
        If RadioButton_1.Checked = True Then
            sys_CoordInfo.intOxyais = 1
        Else
            sys_CoordInfo.intOxyais = 2
        End If

        If CoordInfo_Check() = -1 Then Exit Sub '入力エラーチェック

        Dim ii As Integer, jj As Integer
        Select Case ComboBox_1.Text
            Case "+X"
                iAxis1 = 1
                iAxis1_db = 11
            Case "+Y"
                iAxis1 = 2
                iAxis1_db = 21
            Case "+Z"
                iAxis1 = 3
                iAxis1_db = 31
            Case "-X"
                iAxis1 = -1
                iAxis1_db = 12
            Case "-Y"
                iAxis1 = -2
                iAxis1_db = 22
            Case "-Z"
                iAxis1 = -3
                iAxis1_db = 32
        End Select

        Select Case ComboBox_2.Text
            Case "+X"
                iAxis2 = 1
                iAxis2_db = 11
            Case "+Y"
                iAxis2 = 2
                iAxis2_db = 21
            Case "+Z"
                iAxis2 = 3
                iAxis2_db = 31
            Case "-X"
                iAxis2 = -1
                iAxis2_db = 12
            Case "-Y"
                iAxis2 = -2
                iAxis2_db = 22
            Case "-Z"
                iAxis2 = -3
                iAxis2_db = 32
        End Select

        If RadioButton_1.Checked = True Then
            Select Case iAxis1
                Case 1
                    iFixPos = 1
                Case 2
                    iFixPos = 2
                Case 3
                    iFixPos = 3
                Case -1
                    iFixPos = 1
                Case -2
                    iFixPos = 2
                Case -3
                    iFixPos = 3
            End Select
        Else
            Select Case iAxis2
                Case 1
                    iFixPos = 1
                Case 2
                    iFixPos = 2
                Case 3
                    iFixPos = 3
                Case -1
                    iFixPos = 1
                Case -2
                    iFixPos = 2
                Case -3
                    iFixPos = 3
            End Select
        End If

        Dim iRet As Integer
        iRet = YCM_Get3PosMatrix(sys_CoordInfo.pOrg, sys_CoordInfo.p1, sys_CoordInfo.p2, iAxis1, iAxis2, sys_CoordInfo.intOxyais, sys_CoordInfo.mat)
        Dim dblupdatadb(16) As Double
        dblupdatadb = sys_CoordInfo.mat
        Dim arrupdatadb As New ArrayList
        arrupdatadb.Add(CLng(sys_CoordInfo.pOrg.mid))
        arrupdatadb.Add(CLng(sys_CoordInfo.p1.mid))
        arrupdatadb.Add(CLng(sys_CoordInfo.p2.mid))
        arrupdatadb.Add(0)
        arrupdatadb.Add(iAxis1_db)
        arrupdatadb.Add(iAxis2_db)
        arrupdatadb.Add(iFixPos)
        YCM_UpdataCoordsetting(m_strDataBasePath, dblupdatadb)
        YCM_UpdataCoordsettingvalue(m_strDataBasePath, arrupdatadb)
        YCM_UpDateLookPointReal()

        For ii = 0 To nLookPoints - 1
            For jj = 0 To Data_Point.DGV_DV.Rows.Count - 1
                If Data_Point.DGV_DV.Rows(jj).Cells(5).Value = gDrawPoints(ii).mid Then
                    Data_Point.DGV_DV.Rows(jj).Cells(2).Value = gDrawPoints(ii).Real_x
                    Data_Point.DGV_DV.Rows(jj).Cells(3).Value = gDrawPoints(ii).Real_y
                    Data_Point.DGV_DV.Rows(jj).Cells(4).Value = gDrawPoints(ii).Real_z
                    Exit For
                End If
            Next
        Next

        'For ii = 0 To nRays - 1
        '    YCM_RayPointMatRay(gDrawRays(ii), sys_CoordInfo.mat)
        'Next

        'For ii = 0 To nCamers - 1
        '    YCM_CameraPointMatPoint(gDrawCamers(ii), sys_CoordInfo.mat)
        'Next

        'For ii = 0 To nLabelText - 1
        '    YCM_LabelPointMatPoint(gDrawLabelText(ii), sys_CoordInfo.mat)
        'Next

        Bln_setCoord = True
        Me.Close()
    End Sub
    Private Function CoordInfo_Check() As Integer
        CoordInfo_Check = -1

        If sys_CoordInfo.pOrg.LabelName = "" Then
            MsgBox("原点を選択してください。", MsgBoxStyle.Critical)
            Exit Function
        End If

        If sys_CoordInfo.p1.LabelName = "" Then
            MsgBox("点1を選択してください。", MsgBoxStyle.Critical)
            Exit Function
        End If

        If sys_CoordInfo.p2.LabelName = "" Then
            MsgBox("点2を選択してください。", MsgBoxStyle.Critical)
            Exit Function
        End If

        If nLookPoints = 0 Then
            MsgBox("計測データを開いてください。", MsgBoxStyle.Critical)
            Exit Function
        End If

        Dim blnFindP As Boolean = False
        For ii As Integer = 0 To nLookPoints - 1
            If gDrawPoints(ii).LabelName = sys_CoordInfo.pOrg.LabelName Then
                blnFindP = True
                Exit For
            End If
        Next
        If blnFindP = False Then
            MsgBox("原点のラベルが見つかりませんので、原点を改めて選択してください。", MsgBoxStyle.Critical)
            Exit Function
        End If

        blnFindP = False
        For ii As Integer = 0 To nLookPoints - 1
            If gDrawPoints(ii).LabelName = sys_CoordInfo.p1.LabelName Then
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
            If gDrawPoints(ii).LabelName = sys_CoordInfo.p2.LabelName Then
                blnFindP = True
                Exit For
            End If
        Next
        If blnFindP = False Then
            MsgBox("点2のラベルが見つかりませんので、点2を改めて選択してください。", MsgBoxStyle.Critical)
            Exit Function
        End If

        If (sys_CoordInfo.p1.LabelName = sys_CoordInfo.p2.LabelName) Or _
            (sys_CoordInfo.p1.LabelName = sys_CoordInfo.pOrg.LabelName) Or _
            (sys_CoordInfo.pOrg.LabelName = sys_CoordInfo.p2.LabelName) Then
            MsgBox("原点、点1と点2は同名のラベルを持っていますので、再選択してください。", MsgBoxStyle.Critical)
            Exit Function
        End If

        If sys_CoordInfo.strP1XYZ = sys_CoordInfo.strP2XYZ Then
            MsgBox("点1と点2は同じ軸を持っていますので、再選択してください。", MsgBoxStyle.Critical)
            Exit Function
        End If


        CoordInfo_Check = 0
    End Function
    '13.1.29 原点のコンボボックスの選択が変わったら

    Private Sub COMB_0_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles COMB_0.SelectedIndexChanged
        Dim strmark0 As String = COMB_0.Text
        Dim CP0 As New CLookPoint
        Dim ind0 As Integer
        ind0 = GetPosFromLabelName(CLookPoint.posTypeMode.All, strmark0, CP0)
        If ind0 < 0 Then
            MsgBox("指定されたラベルに対応する計測点が見つかりません。" & vbCrLf & "ラベルを確認して下さい。", MsgBoxStyle.Critical)
            Exit Sub
        End If
        sys_CoordInfo.pOrg = CP0
    End Sub

    '13.1.29 点1のコンボボックスの選択が変わったら
    Private Sub COMB_1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles COMB_1.SelectedIndexChanged
        Dim strmark1 As String = COMB_1.Text
        Dim CP1 As New CLookPoint
        Dim ind1 As Integer
        ind1 = GetPosFromLabelName(CLookPoint.posTypeMode.All, strmark1, CP1)
        If ind1 < 0 Then
            MsgBox("指定されたラベルに対応する計測点が見つかりません。" & vbCrLf & "ラベルを確認して下さい。", MsgBoxStyle.Critical)
            Exit Sub
        End If
        sys_CoordInfo.p1 = CP1
    End Sub

    '13.1.29 点2のコンボボックスの選択が変わったら
    Private Sub COMB_2_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles COMB_2.SelectedIndexChanged
        Dim strmark2 As String = COMB_2.Text
        Dim CP2 As New CLookPoint
        Dim ind2 As Integer
        ind2 = GetPosFromLabelName(CLookPoint.posTypeMode.All, strmark2, CP2)
        If ind2 < 0 Then
            MsgBox("指定されたラベルに対応する計測点が見つかりません。" & vbCrLf & "ラベルを確認して下さい。", MsgBoxStyle.Critical)
            Exit Sub
        End If
        sys_CoordInfo.p2 = CP2
    End Sub



    Dim offsetP As New CLookPoint
    Private Sub Button_OffsetP_Click(sender As System.Object, e As System.EventArgs) Handles Button_OffsetP.Click


        If point_name.Count > 0 Then
            Dim posOrg As New CLookPoint
            Me.Hide()
            If IOUtil.GetPoint(posOrg, "点目を指示：") <> -1 Then

                offsetP = posOrg
                COMB_3.Text = posOrg.LabelName '13.1.29 選択した原点をCOMB_0のTextに
                '<COMB_0.SelectedIndexChanged>イベントに


            End If

            Me.Show()
        End If

    End Sub

    Private Sub Button1_Click_1(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim strOneCToffsetVal As String = ""
        strOneCToffsetVal = strOneCToffsetVal & Now & "," & sys_CoordInfo.pOrg.LabelName & "," & offsetP.Real_x & "," & offsetP.Real_y & "," & offsetP.Real_z & "," & offsetP.LabelName & vbNewLine
        Dim filename As String = "CT_offset_val.csv"
        Dim fields As String()
        Dim delimiter As String = ","
        Dim flgKousin As Boolean = False
        Dim sss As MsgBoxResult
        Using parser As New TextFieldParser(filename)
            parser.SetDelimiters(delimiter)
            While Not parser.EndOfData
                ' Read in the fields for the current line
                fields = parser.ReadFields()
                ' Add code here to use data in fields variable.
                If fields(1) = sys_CoordInfo.pOrg.LabelName Then
                    sss = MsgBox("すでに存在します。更新しますか？", vbYesNo, "確認")
                    flgKousin = True
                    Exit While
                End If
            End While
        End Using

        If flgKousin = True Then
            If sss = MsgBoxResult.Yes Then
                My.Computer.FileSystem.WriteAllText(filename, strOneCToffsetVal, True)
            End If
        Else
            My.Computer.FileSystem.WriteAllText(filename, strOneCToffsetVal, True)
        End If
     
    End Sub
End Class