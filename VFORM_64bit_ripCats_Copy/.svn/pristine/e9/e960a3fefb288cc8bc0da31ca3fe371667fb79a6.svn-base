Imports HalconDotNet

Public Class YCM_CADCoordSetting
    Private Sub Button_P1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_P1.Click
        If point_name.Count > 0 Then
            Dim pos1 As New CLookPoint
            Me.Hide()
            If IOUtil.GetPoint(pos1, "１点目を指示：") <> -1 Then
                Label_P1.Text = pos1.LabelName
                CAD_CoordInfo.p1 = pos1
            End If
            Me.Show()
        End If
    End Sub

    Private Sub Button_P2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_P2.Click
        If point_name.Count > 0 Then
            Dim pos2 As New CLookPoint
            Me.Hide()
            If IOUtil.GetPoint(pos2, "２点目を指示：") <> -1 Then
                Label_P2.Text = pos2.LabelName
                CAD_CoordInfo.p2 = pos2
            End If
            Me.Show()
        End If
    End Sub

    Private Sub Button_P3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_P3.Click
        If point_name.Count > 0 Then
            Dim pos3 As New CLookPoint
            Me.Hide()
            If IOUtil.GetPoint(pos3, "３点目を指示：") <> -1 Then
                Label_P3.Text = pos3.LabelName
                CAD_CoordInfo.p3 = pos3
            End If
            Me.Show()
        End If
    End Sub

    Private Sub Button_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_OK.Click
        Call matching3CADPoint()
        Me.Close()
    End Sub

    Private Sub Button_CANCEL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_CANCEL.Click
        Me.Close()
    End Sub

    Private Sub YCM_CADCoordSetting_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '計測点ラベルをクリア
        Label_P1.Text = "" '-CAD_CoordInfo.p1.LabelName
        Label_P2.Text = "" '-CAD_CoordInfo.p2.LabelName
        Label_P3.Text = "" '-CAD_CoordInfo.p3.LabelName
    End Sub
    Private Function CoordInfo_Check() As Integer
        CoordInfo_Check = -1
        If (CAD_CoordInfo.p1.LabelName = "") Then
            MsgBox("点1を選択してください。", MsgBoxStyle.Critical)
            Exit Function
        End If
        If (CAD_CoordInfo.p2.LabelName = "") Then
            MsgBox("点2を選択してください。", MsgBoxStyle.Critical)
            Exit Function
        End If
        If (CAD_CoordInfo.p3.LabelName = "") Then
            MsgBox("点3を選択してください。", MsgBoxStyle.Critical)
            Exit Function
        End If
        If (nLookPoints <= 0) Then
            MsgBox("計測データを開いてください。", MsgBoxStyle.Critical)
            Exit Function
        End If
        If (findDrawPointIndexByLabelName(CAD_CoordInfo.p1.LabelName) < 0) Then
            MsgBox("点1のラベルが見つかりません、点1を改めて選択してください。", MsgBoxStyle.Critical)
            Exit Function
        End If
        If (findDrawPointIndexByLabelName(CAD_CoordInfo.p2.LabelName) < 0) Then
            MsgBox("点2のラベルが見つかりません、点2を改めて選択してください。", MsgBoxStyle.Critical)
            Exit Function
        End If
        If (findDrawPointIndexByLabelName(CAD_CoordInfo.p3.LabelName) < 0) Then
            MsgBox("点3のラベルが見つかりません、点3を改めて選択してください。", MsgBoxStyle.Critical)
            Exit Function
        End If
        If (CAD_CoordInfo.p1.LabelName = CAD_CoordInfo.p2.LabelName) Or _
            (CAD_CoordInfo.p2.LabelName = CAD_CoordInfo.p3.LabelName) Or _
            (CAD_CoordInfo.p3.LabelName = CAD_CoordInfo.p1.LabelName) Then
            MsgBox("点1,点2,点3は同名のラベルを持っています、再選択してください。", MsgBoxStyle.Critical)
            Exit Function
        End If
        CoordInfo_Check = 0
    End Function

    Public Function findDrawPointIndexByLabelName(ByVal strLabelName As String) As Long
        findDrawPointIndexByLabelName = -1
        If (nLookPoints <= 0) Then Exit Function
        For ii As Integer = 0 To nLookPoints - 1
            If gDrawPoints(ii).LabelName = CAD_CoordInfo.p1.LabelName Then
                findDrawPointIndexByLabelName = ii
                Exit Function
            End If
        Next
    End Function

    '===========================================================================
    ' 機能：CAD図形の3点と計測点の3点で読込んだCAD図形の座標系を3点マッチングする
    ' 備考：

    '   CAD図面の3点：gpCADPos(3)
    '   計測点の 3点：CAD_CoordInfo.p1～p3
    ' (nUserLineStart  - 1) から (nUserLines - 1)までを座標変換
    ' (nCircleNewStart - 1) から (nCircleNew - 1)までを座標変換
    '===========================================================================
    Private Sub matching3CADPoint()
        '入力チェック
        If CoordInfo_Check() = -1 Then Exit Sub

        ' 3点マッチングした座標変換マトリックスを得る
        Dim iRet As Integer
        iRet = Get3CADPosMatchingMatrix(CAD_CoordInfo.p1, CAD_CoordInfo.p2, CAD_CoordInfo.p3, CAD_CoordInfo.mat)
        If (iRet = 0) Then
            '読込んだCAD図形の座標変換を行い座標系を合わせる

            Call transformUserCADDataFor3PosMatching(CAD_CoordInfo.mat)
        End If

        '2.読込んだCAD図形を任意図形データに保持
        ''If (cadFilterFrm.CheckBox_ADDMode.CheckState = False) Then
        If (bCADElmAddMode = False) Then
            '2.1 読込まれているCADデータを削除
            Call deleteUserCADData(False, True)
        End If
        ' 2.2 goCADElmArrsからgDrawUserLines、gDrawCircleNewへ読込んだCAD図形を追加
        Call CADElmArrToUserDrawings()

        '#If 0 Then
        '        Dim ii As Integer, jj As Integer
        '        iRet = YCM_Get3PosMatrix(sys_CoordInfo.pOrg, sys_CoordInfo.p1, sys_CoordInfo.p2, iAxis1, iAxis2, sys_CoordInfo.intOxyais, sys_CoordInfo.mat)
        '        Dim dblupdatadb(16) As Double
        '        dblupdatadb = sys_CoordInfo.mat
        '        Dim arrupdatadb As New ArrayList
        '        arrupdatadb.Add(CLng(sys_CoordInfo.pOrg.mid))
        '        arrupdatadb.Add(CLng(sys_CoordInfo.p1.mid))
        '        arrupdatadb.Add(CLng(sys_CoordInfo.p2.mid))
        '        arrupdatadb.Add(0)
        '        arrupdatadb.Add(iAxis1_db)
        '        arrupdatadb.Add(iAxis2_db)
        '        arrupdatadb.Add(iFixPos)
        '        YCM_UpdataCoordsetting(m_strDataBasePath, dblupdatadb)
        '        YCM_UpdataCoordsettingvalue(m_strDataBasePath, arrupdatadb)
        '        YCM_UpDateLookPointReal()
        '        For ii = 0 To nLookPoints - 1
        '            For jj = 0 To Data_Point.DGV_DV.Rows.Count - 1
        '                If Data_Point.DGV_DV.Rows(jj).Cells(5).Value = gDrawPoints(ii).mid Then
        '                    Data_Point.DGV_DV.Rows(jj).Cells(2).Value = gDrawPoints(ii).Real_x
        '                    Data_Point.DGV_DV.Rows(jj).Cells(3).Value = gDrawPoints(ii).Real_y
        '                    Data_Point.DGV_DV.Rows(jj).Cells(4).Value = gDrawPoints(ii).Real_z
        '                    Exit For
        '                End If
        '            Next
        '        Next
        '        Bln_setCoord = True
        '#End If
        Me.Close()
    End Sub

    Private Sub transformUserCADDataFor3PosMatching(ByVal mat() As Double)
        '読込んだCAD図形データの座標変換を行う
        Dim gpS As New GeoPoint, gpE As New GeoPoint, gpOrg As New GeoPoint
        Dim dRad As Double, dXAng As Double, dYAng As Double
        Dim sclX As Double, sclY As Double, sclZ As Double, scl As Double
        sclX = mat(0)   '(1,1)
        sclY = mat(5)   '(2,2)
        sclZ = mat(10)  '(3,3)
        scl = (sclX + sclY + sclZ) / 3.0

        Dim numElm As Long
        Dim cElm As CElment
        Dim i As Long, elmType As Integer
        numElm = goCADElmArrs.Size
        If (numElm <= 0) Then Exit Sub
        For i = 0 To (numElm - 1)
            cElm = New CElment(ElementType.ElmLine)
            cElm = goCADElmArrs.at(i)
            elmType = cElm.ElmType
            Select Case (elmType)
                Case ElementType.ElmLine
                    gpS = cElm.StartPoint.Copy
                    gpE = cElm.EndPoint.Copy
                    transformPoint(gpS, mat)
                    transformPoint(gpE, mat)
                    cElm.StartPoint = gpS
                    cElm.EndPoint = gpE
                    goCADElmArrs.SetAt(i, cElm)

                Case ElementType.ElmCircle
                    gpOrg = cElm.Origin.Copy
                    transformPoint(gpOrg, mat)
                    cElm.Origin = gpOrg
                    dRad = cElm.Rad
                    cElm.Rad = dRad * scl
                    dXAng = cElm.XAng
                    dYAng = cElm.YAng
                    goCADElmArrs.SetAt(i, cElm)
                Case Else
            End Select
            cElm = Nothing
        Next


        ' ''--rep.-------------------------------------
        ' ''変換する範囲
        ''numElm = nUserLines - nUserLineStart
        ' ''線分図形の座標変換
        ''If numElm > 0 Then
        ''    For ii = nUserLineStart To (nUserLines - 1)
        ''        If (gDrawUserLines(ii).elmType = 1) Then
        ''            transformPoint(gDrawUserLines(ii).startPnt, mat)
        ''            transformPoint(gDrawUserLines(ii).endPnt, mat)
        ''        End If
        ''    Next
        ''End If

        ' ''変換する範囲
        ' '' nCircleNewStart As Long          'CAD図形読込前の円数
        ''numElm = nCircleNew - nCircleNewStart
        ' ''円図形の座標変換
        ''If (numElm > 0) Then
        ''    For ii = nCircleNewStart To (nCircleNew - 1)
        ''        If (gDrawCircleNew(ii).elmType = 1) Then
        ''            transformPoint(gDrawCircleNew(ii).org, mat)

        ''            '半径、xAng、yAngの変換


        ''            ''--出力次元による座標値の調整
        ''            'dblRad = gDrawCircleNew(ii).r * sys_ScaleInfo.scale
        ''            'If (iOutCoord <> 1) Then
        ''            '    dblAng = DegToRadian(gDrawCircleNew(ii).x_angle)
        ''            '    If (Math.Abs(dblAng) > 0.0#) Then
        ''            '        icdEnt.Rotate3D(iPS, iPE, dblAng)
        ''            '    End If
        ''            '    iPE = g_IJcadApp.Library.CreatePoint(pRS.x, (pRS.y + 1000.0#), pRS.z)
        ''            '    dblAng = DegToRadian(gDrawCircleNew(ii).y_angle)
        ''            '    If (Math.Abs(dblAng) > 0.0#) Then
        ''            '        icdEnt.Rotate3D(iPS, iPE, dblAng)
        ''            '    End If
        ''            'End If
        ''        End If
        ''    Next
        ''End If


        ''+++++++++++++++++++++++++++++++
        ''線分図形
        'If nUserLines > 0 Then
        '    For ii = (nUserLines - 1) To 0 Step -1
        '        bDelete = (gDrawUserLines(ii).elmType = 0) And bDelUser
        '        bDelete = bDelete Or (gDrawUserLines(ii).elmType = 1) And bDelCAD
        '        If (bDelete = True) Then
        '            nUserLines = nUserLines - 1
        '            ReDim Preserve gDrawUserLines(nUserLines)
        '        End If
        '    Next
        'End If
        ''円図形
        'If (nCircleNew > 0) Then
        '    For ii = 0 To nCircleNew - 1
        '        bDelete = (gDrawCircleNew(ii).elmType = 0) And bDelUser
        '        bDelete = bDelete Or (gDrawCircleNew(ii).elmType = 1) And bDelCAD
        '        If (bDelete = True) Then
        '            nCircleNew = nCircleNew - 1
        '            ReDim Preserve gDrawCircleNew(nCircleNew)
        '        End If
        '    Next
        'End If
    End Sub
    '============================================================
    ' 機能：指定された点を座標変換する
    '============================================================
    Public Function transformPoint(ByRef gpPos As GeoPoint, ByVal mat() As Double) As Integer
        Dim lookmat(0 To 3) As Double
        Dim XM(4, 4) As Double
        lookmat(0) = gpPos.x : lookmat(1) = gpPos.y : lookmat(2) = gpPos.z : lookmat(3) = 0
        lookmat(0) = gpPos.x * mat(0) + gpPos.y * mat(4) + gpPos.z * mat(8) + 1.0 * mat(12)
        lookmat(1) = gpPos.x * mat(1) + gpPos.y * mat(5) + gpPos.z * mat(9) + 1.0 * mat(13)
        lookmat(2) = gpPos.x * mat(2) + gpPos.y * mat(6) + gpPos.z * mat(10) + 1.0 * mat(14)
        gpPos.x = lookmat(0) : gpPos.y = lookmat(1) : gpPos.z = lookmat(2)
    End Function


    '===============================================================
    ' 機　能：3点と軸方向を指定して座標変換マトリックスを取得する

    ' 戻り値：=0：正常終了

    '         =1：入力パラメタエラー（  ）

    ' 引　数：

    '    pos1   [I/ ] 点１

    '    pos2   [I/ ] 点２

    '    pos3   [I/ ] 点３

    '    dblMat() [ /O] 座標変換マトリックス（4×4）

    ' 備　考：

    '   重み：Xi(),Yi(),Zi()
    '   計測値：Xs(i),Ys(i),Zs(i)   ：CAD座標点
    '   設計値：Xj(i),Yj(i),Zj(i)   ：計測点
    '   計算された座標値：Xa(i),Ya(i),Za(i)  
    '===============================================================
    Private Function Get3CADPosMatchingMatrix( _
        ByRef pos1 As CLookPoint, _
        ByRef pos2 As CLookPoint, _
        ByRef pos3 As CLookPoint, _
        ByRef dblMat() As Double _
    ) As Integer
        'On Error GoTo Err_lbl
        Dim Xj(6) As Double, Yj(6) As Double, Zj(6) As Double '--設計点：Xj(),Yj(),Zj()
        Dim Xs(6) As Double, Ys(6) As Double, Zs(6) As Double '--計測点：Xs(),Ys(),Zs()
        Dim Xi(6) As Double, Yi(6) As Double, Zi(6) As Double '--重み　：Xi(),Yi(),Zi()
        Dim Xa(6) As Double, Ya(6) As Double, Za(6) As Double '--重みを考慮したジャストフィット計算結果：Xa(),Ya(),Za()

        Get3CADPosMatchingMatrix = -1

        Dim dScale As Double
        dScale = 1.0
        If (sys_ScaleInfo.scale > 0.0) Then dScale = 1.0 / sys_ScaleInfo.scale

        '重みを設定する

        Xi(0) = 1.0# : Yi(0) = 1.0# : Zi(0) = 1.0#
        Xi(1) = 1.0# : Yi(1) = 1.0# : Zi(1) = 1.0#
        Xi(2) = 1.0# : Yi(2) = 1.0# : Zi(2) = 1.0#
        '--3.設計点座標値の設定（計測点を設定）

        Xj(0) = pos1.x : Yj(0) = pos1.y : Zj(0) = pos1.z
        Xj(1) = pos2.x : Yj(1) = pos2.y : Zj(1) = pos2.z
        Xj(2) = pos3.x : Yj(2) = pos3.y : Zj(2) = pos3.z
        '--3.計測点座標値の設定（CAD座標値を設定）

        '-3.1 計測点３点
        Xs(0) = gpCADPos(0).x * dScale : Ys(0) = gpCADPos(0).y * dScale : Zs(0) = gpCADPos(0).z * dScale
        Xs(1) = gpCADPos(1).x * dScale : Ys(1) = gpCADPos(1).y * dScale : Zs(1) = gpCADPos(1).z * dScale
        Xs(2) = gpCADPos(2).x * dScale : Ys(2) = gpCADPos(2).y * dScale : Zs(2) = gpCADPos(2).z * dScale
        '-3.2 計測点に単位座標系のデータを追加
        Xs(3) = 0.0# : Ys(3) = 0.0# : Zs(3) = 0.0#
        Xs(4) = 1.0# : Ys(4) = 0.0# : Zs(4) = 0.0#
        Xs(5) = 0.0# : Ys(5) = 1.0# : Zs(5) = 0.0#
        Xs(6) = 0.0# : Ys(6) = 0.0# : Zs(6) = 1.0#

        '--4.重みを考慮したジャストフィット計算：結果：Xa(),Ya(),Za()
        Dim num As Integer
        'Dim iRet As Integer
        num = 7

        Dim gmat As New GeoMatrix
        Dim hhmm As HTuple = Nothing
        correspond_3d_3d_weight(Xs, Ys, Zs, Xj, Yj, Zj, Xi, Yi, Zi, hhmm)
        Dim i As Integer, j As Integer
        For i = 1 To 4
            For j = 1 To 4
                gmat.SetAt(i, j, 0.0)
            Next j
        Next i
        For i = 1 To 4
            For j = 1 To 3
                gmat.SetAt(i, j, hhmm((j - 1) * 4 + (i - 1)))
            Next j
        Next i
        gmat.SetAt(4, 4, 1.0)
        '20170330 baluu del start
        'iRet = OmomiExcute(num, Xi(0), Yi(0), Zi(0), Xj(0), Yj(0), Zj(0), Xa(0), Ya(0), Za(0), Xs(0), Ys(0), Zs(0))
        'If iRet <> 0 Then
        'End If
        '20170330 baluu del end
        '--5.座標変換マトリックスを作成する
        'Dim gmat As New GeoMatrix '20170330 baluu del start
        Dim gpOrg As New GeoPoint
        'Dim gvX As New GeoVector, gvY As New GeoVector, gvZ As New GeoVector
        'Call gpOrg.setXYZ(Xa(3), Ya(3), Za(3))
        'Call gvX.setXYZ(Xa(4), Ya(4), Za(4))
        'Call gvY.setXYZ(Xa(5), Ya(5), Za(5))
        'Call gvZ.setXYZ(Xa(6), Ya(6), Za(6))
        'Call gvX.SubtractPoint(gpOrg)
        'Call gvY.SubtractPoint(gpOrg)
        'Call gvZ.SubtractPoint(gpOrg)
        'Call gmat.SetCoordSystem(gpOrg, gvX, gvY, gvZ)
        ''gpOrg.Transform()
        ''gmat.PrintClass
        'Call gmat.Invert()
        'sys_CoordInfo.mat_geo = gmat.Copy
        'gmat.PrintClass
        '20170330 baluu del end
        '--6.結果を変換
        Dim ind As Integer
        ReDim dblMat(0 To 4 * 4)
        ind = -1
        For i = 1 To 4
            For j = 1 To 4
                ind = ind + 1
                dblMat(ind) = gmat.GetAt(i, j)
            Next j
        Next i
        Get3CADPosMatchingMatrix = 0
        Exit Function
Err_lbl:
    End Function

End Class