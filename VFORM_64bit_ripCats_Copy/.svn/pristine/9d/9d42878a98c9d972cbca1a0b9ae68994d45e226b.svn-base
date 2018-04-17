Public Class YCM_DrawSetting
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub YCM_DrawSetting_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '★20121115計測点
        For i As Integer = 0 To ncolor - 1
            CobBPoint.Items.Add(sys_color(i).strColorName)
        Next
        If entset_point.color.strName <> "" Then
            CobBPoint.Text = entset_point.color.strName
        Else
            CobBPoint.SelectedIndex = 0
        End If

        '================================================================================================20121115
        ''★20121114追加計測点(ターゲット面)
        'For i As Integer = 0 To ncolor - 1
        '    CobBPoint1.Items.Add(sys_color(i).strColorName)
        'Next
        'If entset_point1.color.strName <> "" Then
        '    CobBPoint1.Text = entset_point1.color.strName
        'Else
        '    CobBPoint1.SelectedIndex = 0
        'End If
        '================================================================================================20121115

        '★20121114計測点(中心円)
        For i As Integer = 0 To ncolor - 1
            CobBPoint2.Items.Add(sys_color(i).strColorName)
        Next
        If entset_point.color2.strName <> "" Then
            CobBPoint2.Text = entset_point.color2.strName
        Else
            CobBPoint2.SelectedIndex = 0
        End If
        '★20121114計測点(十字線)
        For i As Integer = 0 To ncolor - 1
            CobBPoint3.Items.Add(sys_color(i).strColorName)
        Next
        If entset_point.color3.strName <> "" Then
            CobBPoint3.Text = entset_point.color3.strName
        Else
            CobBPoint3.SelectedIndex = 0
        End If

        '計測点(任意追加点)
        For i As Integer = 0 To ncolor - 1
            CobBPointUser.Items.Add(sys_color(i).strColorName)
        Next
        If entset_pointUser.color.strName <> "" Then
            CobBPointUser.Text = entset_pointUser.color.strName
        Else
            CobBPointUser.SelectedIndex = 0
        End If
        'カメラ
        For i As Integer = 0 To ncolor - 1
            CobBCamera.Items.Add(sys_color(i).strColorName)
        Next
        If entset_camera.color.strName <> "" Then
            CobBCamera.Text = entset_camera.color.strName
        Else
            CobBCamera.SelectedIndex = 0
        End If
        'カメラレンズ
        For i As Integer = 0 To ncolor - 1
            CobBCamera2.Items.Add(sys_color(i).strColorName)
        Next
        If entset_camera.color2.strName <> "" Then
            CobBCamera2.Text = entset_camera.color2.strName
        Else
            CobBCamera2.SelectedIndex = 0
        End If
        'カメラシャッター
        For i As Integer = 0 To ncolor - 1
            CobBCamera3.Items.Add(sys_color(i).strColorName)
        Next
        If entset_camera.color3.strName <> "" Then
            CobBCamera3.Text = entset_camera.color3.strName
        Else
            CobBCamera3.SelectedIndex = 0
        End If
        'レイ
        For i As Integer = 0 To ncolor - 1
            CobBRay.Items.Add(sys_color(i).strColorName)
        Next
        If entset_ray.color.strName <> "" Then
            CobBRay.Text = entset_ray.color.strName
        Else
            CobBRay.SelectedIndex = 0
        End If

        'ラベル
        For i As Integer = 0 To ncolor - 1
            CobBLaber.Items.Add(sys_color(i).strColorName)
        Next
        If entset_label.color.strName <> "" Then
            CobBLaber.Text = entset_label.color.strName
        Else
            CobBLaber.SelectedIndex = 0
        End If
        '任意線分の色
        For i As Integer = 0 To ncolor - 1
            cmb_LineColor.Items.Add(sys_color(i).strColorName)
        Next
        If entset_line.color.strName <> "" Then
            cmb_LineColor.Text = entset_line.color.strName
        Else
            cmb_LineColor.SelectedIndex = 0
        End If
        '任意円の色
        For i As Integer = 0 To ncolor - 1
            cmb_circleColor.Items.Add(sys_color(i).strColorName)
        Next
        If entset_circle.color.strName <> "" Then
            cmb_circleColor.Text = entset_circle.color.strName
        Else
            cmb_circleColor.SelectedIndex = 0
        End If

        For i As Integer = 0 To nLineType - 1
            cmb_rayLinetype.Items.Add(sys_LineType(i).strLineTypeName)
        Next
        If entset_ray.linetype.strName <> "" Then
            cmb_rayLinetype.Text = entset_ray.linetype.strName
        Else
            cmb_rayLinetype.SelectedIndex = 0
        End If
        '任意線分の線種
        For i As Integer = 0 To nLineType - 1
            cmb_LineLineType.Items.Add(sys_LineType(i).strLineTypeName)
        Next
        If entset_line.linetype.strName <> "" Then
            cmb_LineLineType.Text = entset_line.linetype.strName
        Else
            cmb_LineLineType.SelectedIndex = 0
        End If
        '任意円の線種
        For i As Integer = 0 To nLineType - 1
            cmb_circleLineType.Items.Add(sys_LineType(i).strLineTypeName) '①
        Next
        If entset_circle.linetype.strName <> "" Then
            cmb_circleLineType.Text = entset_circle.linetype.strName
        Else
            cmb_circleLineType.SelectedIndex = 0
        End If

        '★20121115計測点
        Me.txt_pointsize.Text = CStr(entset_point.screensize)
        Me.txt_pointmin.Text = CStr(entset_point.minscale)
        Me.txt_pointmax.Text = CStr(entset_point.maxscale)
        Me.txt_pointkonomi.Text = CStr(entset_point.konomi)
        '20130107計測点(タグ)
        Me.txt_pointsize.Tag = CStr(entset_point.screensize)
        Me.txt_pointmin.Tag = CStr(entset_point.minscale)
        Me.txt_pointmax.Tag = CStr(entset_point.maxscale)
        Me.txt_pointkonomi.Tag = CStr(entset_point.konomi)
        '================================================================================================20121115
        ''★20121114計測点(ターゲット面)/カメラを参考にすべきか?(レンズ、シャッターがない)
        'Me.txt_pointsize.Text = CStr(entset_point1.screensize)
        'Me.txt_pointmin.Text = CStr(entset_point1.minscale)
        'Me.txt_pointmax.Text = CStr(entset_point1.maxscale)
        'Me.txt_pointkonomi.Text = CStr(entset_point1.konomi)
        ''★20121114計測点(中心円)
        'Me.txt_pointsize.Text = CStr(entset_point2.screensize)
        'Me.txt_pointmin.Text = CStr(entset_point2.minscale)
        'Me.txt_pointmax.Text = CStr(entset_point2.maxscale)
        'Me.txt_pointkonomi.Text = CStr(entset_point2.konomi)
        ''★20121114計測点(十字線)
        'Me.txt_pointsize.Text = CStr(entset_point3.screensize)
        'Me.txt_pointmin.Text = CStr(entset_point3.minscale)
        'Me.txt_pointmax.Text = CStr(entset_point3.maxscale)
        'Me.txt_pointkonomi.Text = CStr(entset_point3.konomi)
        '================================================================================================20121115

        '計測点（任意追加点）

        Me.txt_pointsizeUser.Text = CStr(entset_pointUser.screensize)
        Me.txt_pointminUser.Text = CStr(entset_pointUser.minscale)
        Me.txt_pointmaxUser.Text = CStr(entset_pointUser.maxscale)
        Me.txt_pointkonomiUser.Text = CStr(entset_pointUser.konomi)
        '20130107任意追加点(タグ)
        Me.txt_pointsizeUser.Tag = CStr(entset_pointUser.screensize)
        Me.txt_pointminUser.Tag = CStr(entset_pointUser.minscale)
        Me.txt_pointmaxUser.Tag = CStr(entset_pointUser.maxscale)
        Me.txt_pointkonomiUser.Tag = CStr(entset_pointUser.konomi)

        'カメラ
        Me.txt_camerasize.Text = CStr(entset_camera.screensize)
        Me.txt_cameramin.Text = CStr(entset_camera.minscale)
        Me.txt_cameramax.Text = CStr(entset_camera.maxscale)
        Me.txt_camerakonami.Text = CStr(entset_camera.konomi)
        '20130107カメラ(タグ)
        Me.txt_camerasize.Tag = CStr(entset_camera.screensize)
        Me.txt_cameramin.Tag = CStr(entset_camera.minscale)
        Me.txt_cameramax.Tag = CStr(entset_camera.maxscale)
        Me.txt_camerakonami.Tag = CStr(entset_camera.konomi)

        'ラベル
        Me.txt_labelsize.Text = CStr(entset_label.screensize)
        Me.txt_labelmin.Text = CStr(entset_label.minscale)
        Me.txt_labelmax.Text = CStr(entset_label.maxscale)
        Me.txt_labelkonami.Text = CStr(entset_label.konomi)
        '20130107ラベル(タグ)
        Me.txt_labelsize.Tag = CStr(entset_label.screensize)
        Me.txt_labelmin.Tag = CStr(entset_label.minscale)
        Me.txt_labelmax.Tag = CStr(entset_label.maxscale)
        Me.txt_labelkonami.Tag = CStr(entset_label.konomi)

        '各種線幅

        Me.cmb_LineLineWidth.Text = CStr(LineWidth_Line)
        Me.cmb_circleLineWidth.Text = CStr(circleWidth_Line) '!!!!!!!!!!!!!!!!20121122
        Me.cmb_rayLineWidth.Text = CStr(LineWidth_Ray)
        '20130107各種線幅(タグ)
        Me.cmb_LineLineWidth.Tag = CStr(LineWidth_Line)
        Me.cmb_circleLineWidth.Tag = CStr(circleWidth_Line)
        Me.cmb_rayLineWidth.Tag = CStr(LineWidth_Ray)

        '★20121115計測点
        chkLookPoint.Checked = entset_point.blnVisiable
        chkLookPointUser.Checked = entset_pointUser.blnVisiable

        '================================================================================================20121115
        ''★20121114計測点(ターゲット面)/カメラを参考にすべきか(レンズ、シャッターがない)
        'chkLookPoint.Checked = entset_point1.blnVisiable
        ''★20121114entset_Point2も必要か？

        'chkLookPoint.Checked = entset_point2.blnVisiable
        ''★20121114entset_Point3も必要か？

        'chkLookPoint.Checked = entset_point3.blnVisiable
        '================================================================================================20121115

        chkCamera.Checked = entset_camera.blnVisiable
        chkLabel.Checked = entset_label.blnVisiable
        chkRay.Checked = entset_ray.blnVisiable
        chkLine.Checked = entset_line.blnVisiable
        chkCircle.Checked = entset_circle.blnVisiable

        'CAD図形分の追加
        chkLine_CAD.Checked = entset_line_CAD.blnVisiable
        chkCircle_CAD.Checked = entset_circle_CAD.blnVisiable
        Me.cmb_LineLineWidth_CAD.Text = CStr(LineWidth_Line)
        Me.cmb_circleLineWidth_CAD.Text = CStr(LineWidth_Line)
        '20130107CAD図形(タグ)
        Me.cmb_LineLineWidth_CAD.Tag = CStr(LineWidth_Line)
        Me.cmb_circleLineWidth_CAD.Tag = CStr(LineWidth_Line)

        Txt_LineLayerName.Text = entset_line.layerName
        Txt_CircleLayerName.Text = entset_circle.layerName
        Txt_LineLayerName_CAD.Text = entset_line_CAD.layerName
        Txt_CircleLayerName_CAD.Text = entset_circle_CAD.layerName
        'CAD図形：色
        For i As Integer = 0 To ncolor - 1
            cmb_LineColor_CAD.Items.Add(sys_color(i).strColorName)
        Next
        If entset_line_CAD.color.strName <> "" Then
            cmb_LineColor_CAD.Text = entset_line_CAD.color.strName
        Else
            cmb_LineColor_CAD.SelectedIndex = 0
        End If
        For i As Integer = 0 To ncolor - 1
            cmb_circleColor_CAD.Items.Add(sys_color(i).strColorName)
        Next
        If entset_circle_CAD.color.strName <> "" Then
            cmb_circleColor_CAD.Text = entset_circle_CAD.color.strName
        Else
            cmb_circleColor_CAD.SelectedIndex = 0
        End If
        'CAD図形：線種
        For i As Integer = 0 To nLineType - 1
            cmb_LineLineType_CAD.Items.Add(sys_LineType(i).strLineTypeName)
        Next
        If entset_line_CAD.linetype.strName <> "" Then
            cmb_LineLineType_CAD.Text = entset_line_CAD.linetype.strName
        Else
            cmb_LineLineType_CAD.SelectedIndex = 0
        End If

        For i As Integer = 0 To nLineType - 1
            cmb_circleLineType_CAD.Items.Add(sys_LineType(i).strLineTypeName)
        Next
        If entset_circle_CAD.linetype.strName <> "" Then
            cmb_circleLineType_CAD.Text = entset_circle_CAD.linetype.strName
        Else
            cmb_circleLineType_CAD.SelectedIndex = 0
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        '★20121114計測点(ターゲット面)の作図情報を更新する
        entset_point.screensize = CDbl(txt_pointsize.Text)
        entset_point.color = YCM_GetColorInfoByName(CobBPoint.Text) '計測点(ターゲット面)
        entset_point.color2 = YCM_GetColorInfoByName(CobBPoint2.Text) '計測点(中心円)
        entset_point.color3 = YCM_GetColorInfoByName(CobBPoint3.Text) '計測点(十字線)
        entset_point.minscale = CDbl(txt_pointmin.Text)
        entset_point.maxscale = CDbl(txt_pointmax.Text)
        entset_point.konomi = CDbl(txt_pointkonomi.Text)
        entset_point.blnVisiable = chkLookPoint.Checked

        '================================================================================================20121115
        ''★カメラの作図情報を更新する
        'entset_camera.screensize = CDbl(txt_camerasize.Text)
        'entset_camera.color = YCM_GetColorInfoByName(CobBCamera.Text)
        'entset_camera.color2 = YCM_GetColorInfoByName(CobBCamera2.Text)
        'entset_camera.color3 = YCM_GetColorInfoByName(CobBCamera3.Text)
        'entset_camera.minscale = CDbl(txt_cameramin.Text)
        'entset_camera.maxscale = CDbl(txt_cameramax.Text)
        'entset_camera.konomi = CDbl(txt_camerakonami.Text)
        'entset_camera.blnVisiable = chkCamera.Checked

        ''★20121114計測点(ターゲット面)の作図情報を更新する/下部のカメラを参考にすべきか?(レンズ、シャッター統一?)
        'entset_point1.screensize = CDbl(txt_pointsize.Text)
        'entset_point1.color = YCM_GetColorInfoByName(CobBPoint1.Text)
        'entset_point1.minscale = CDbl(txt_pointmin.Text)
        'entset_point1.maxscale = CDbl(txt_pointmax.Text)
        'entset_point1.konomi = CDbl(txt_pointkonomi.Text)
        'entset_point1.blnVisiable = chkLookPoint.Checked
        ''★20121114計測点(中心円)の作図情報を更新する
        'entset_point2.screensize = CDbl(txt_pointsize.Text)
        'entset_point2.color = YCM_GetColorInfoByName(CobBPoint1.Text)
        'entset_point2.minscale = CDbl(txt_pointmin.Text)
        'entset_point2.maxscale = CDbl(txt_pointmax.Text)
        'entset_point2.konomi = CDbl(txt_pointkonomi.Text)
        'entset_point2.blnVisiable = chkLookPoint.Checked
        ''★20121114計測点(十字線)の作図情報を更新する
        'entset_point3.screensize = CDbl(txt_pointsize.Text)
        'entset_point3.color = YCM_GetColorInfoByName(CobBPoint1.Text)
        'entset_point3.minscale = CDbl(txt_pointmin.Text)
        'entset_point3.maxscale = CDbl(txt_pointmax.Text)
        'entset_point3.konomi = CDbl(txt_pointkonomi.Text)
        'entset_point3.blnVisiable = chkLookPoint.Checked
        '================================================================================================20121115

        '計測点（任意追加点）の作図情報を更新する
        entset_pointUser.screensize = CDbl(txt_pointsizeUser.Text)
        entset_pointUser.color = YCM_GetColorInfoByName(CobBPointUser.Text)
        entset_pointUser.minscale = CDbl(txt_pointminUser.Text)
        entset_pointUser.maxscale = CDbl(txt_pointmaxUser.Text)
        entset_pointUser.konomi = CDbl(txt_pointkonomiUser.Text)
        entset_pointUser.blnVisiable = chkLookPointUser.Checked

        'レイの作図情報を更新する
        entset_ray.color = YCM_GetColorInfoByName(CobBRay.Text)
        entset_ray.linetype = YCM_GetLineTypeInfoByName(cmb_rayLinetype.Text)
        entset_ray.blnVisiable = chkRay.Checked
        entset_ray.linewidth = cmb_rayLineWidth.Text
        LineWidth_Ray = entset_ray.linewidth
        '任意線分の作図情報を更新する
        entset_line.color = YCM_GetColorInfoByName(cmb_LineColor.Text)
        entset_line.linetype = YCM_GetLineTypeInfoByName(cmb_LineLineType.Text)
        entset_line.blnVisiable = chkLine.Checked
        entset_line.linewidth = cmb_LineLineWidth.Text
        LineWidth_Line = entset_line.linewidth
        entset_line.layerName = Txt_LineLayerName.Text
        '任意円の作図情報を更新する
        entset_circle.color = YCM_GetColorInfoByName(cmb_circleColor.Text)
        entset_circle.linetype = YCM_GetLineTypeInfoByName(cmb_circleLineType.Text)
        entset_circle.blnVisiable = chkCircle.Checked
        entset_circle.linewidth = cmb_circleLineWidth.Text
        circleWidth_Line = entset_circle.linewidth '20121122!!!!!!!!!!!!!!!!!
        entset_circle.layerName = Txt_CircleLayerName.Text

        'CAD図形分

        'ラインの作図情報を更新する
        entset_line_CAD.color = YCM_GetColorInfoByName(cmb_LineColor_CAD.Text)
        entset_line_CAD.linetype = YCM_GetLineTypeInfoByName(cmb_LineLineType_CAD.Text)
        entset_line_CAD.blnVisiable = chkLine_CAD.Checked
        entset_line_CAD.linewidth = cmb_LineLineWidth_CAD.Text
        entset_line_CAD.layerName = Txt_LineLayerName_CAD.Text
        '円の作図情報を更新する
        entset_circle_CAD.color = YCM_GetColorInfoByName(cmb_circleColor_CAD.Text)
        entset_circle_CAD.linetype = YCM_GetLineTypeInfoByName(cmb_circleLineType_CAD.Text)
        entset_circle_CAD.blnVisiable = chkCircle_CAD.Checked
        entset_circle_CAD.linewidth = cmb_circleLineWidth_CAD.Text
        entset_circle_CAD.layerName = Txt_CircleLayerName_CAD.Text

        'ラベルの作図情報を更新する
        entset_label.screensize = CDbl(txt_labelsize.Text)
        entset_label.color = YCM_GetColorInfoByName(CobBLaber.Text)
        entset_label.minscale = CDbl(txt_labelmin.Text)
        entset_label.maxscale = CDbl(txt_labelmax.Text)
        entset_label.konomi = CDbl(txt_labelkonami.Text)
        entset_label.blnVisiable = chkLabel.Checked

        '★カメラの作図情報を更新する
        entset_camera.screensize = CDbl(txt_camerasize.Text)
        entset_camera.color = YCM_GetColorInfoByName(CobBCamera.Text)
        entset_camera.color2 = YCM_GetColorInfoByName(CobBCamera2.Text)
        entset_camera.color3 = YCM_GetColorInfoByName(CobBCamera3.Text)
        entset_camera.minscale = CDbl(txt_cameramin.Text)
        entset_camera.maxscale = CDbl(txt_cameramax.Text)
        entset_camera.konomi = CDbl(txt_camerakonami.Text)
        entset_camera.blnVisiable = chkCamera.Checked


        '--del.rep.start-------------------------12.10.17
        ' リボンメニューに表示／非表示の状態を反映
        Call setRibbonMenuChkOnOff()

#If 0 Then
        '★2012115計測点
        If entset_point.blnVisiable Then
            MainFrm.MM_V_9_1.Checked = True
        Else
            MainFrm.MM_V_9_1.Checked = False
        End If
        '================================================================================================20121115
        ''★2012114計測点(ターゲット面)
        'If entset_point1.blnVisiable Then
        '    MainFrm.MM_V_9_1.Checked = True
        'Else
        '    MainFrm.MM_V_9_1.Checked = False
        'End If
        ' '★2012114計測点(中心円)
        'If entset_point2.blnVisiable Then
        '    MainFrm.MM_V_9_1.Checked = True
        'Else
        '    MainFrm.MM_V_9_1.Checked = False
        'End If
        ' '★2012114計測点(十字線)
        'If entset_point3.blnVisiable Then
        '    MainFrm.MM_V_9_1.Checked = True
        'Else
        '    MainFrm.MM_V_9_1.Checked = False
        'End If
        '================================================================================================20121115
        If entset_pointUser.blnVisiable Then
            MainFrm.MM_V_9_1User.Checked = True     '---任意計測点の表示／非表示を追加する必要あり！！！

        Else
            MainFrm.MM_V_9_1User.Checked = False
        End If

        If entset_camera.blnVisiable Then
            MainFrm.MM_V_9_2.Checked = True
        Else
            MainFrm.MM_V_9_2.Checked = False
        End If

        If entset_ray.blnVisiable Then
            MainFrm.MM_V_9_3.Checked = True
        Else
            MainFrm.MM_V_9_3.Checked = False
        End If

        If entset_label.blnVisiable Then
            MainFrm.MM_V_9_4.Checked = True
        Else
            MainFrm.MM_V_9_4.Checked = False
        End If

        If entset_line.blnVisiable And entset_circle.blnVisiable Then
            MainFrm.MM_V_9_5.Checked = True
        Else
            MainFrm.MM_V_9_5.Checked = False
        End If
#End If
        '--del.rep.end---------------------------12.10.17

        YCM_UpdateEntSetting(m_strDataBasePath)
        Me.Close()


        '13.1.7　計測点の「表示サイズ」「最小」「最大」について　By 山田
        Dim PS As Double = txt_pointsize.Text
        Dim PMin As Double = txt_pointmin.Text
        Dim Pmax As Double = txt_pointmax.Text
        If PMin > Pmax Then
            MessageBox.Show("計測点の「最小」に入力する値は「最大」より小さい値を入力して下さい。", "エラー", _
       MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        ElseIf PS > Pmax Then
            MessageBox.Show("計測点の「表示サイズ」に入力する値は「最大」より小さい値を入力して下さい。", "エラー", _
       MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        ElseIf PS < PMin Then
            MessageBox.Show("計測点の「表示サイズ」に入力する値は「最小」より大きい値を入力して下さい。", "エラー", _
      MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        '13.1.7　追加計測点の「表示サイズ」「最小」「最大」について　
        Dim US As Double = txt_pointsizeUser.Text
        Dim UMin As Double = txt_pointminUser.Text
        Dim UMax As Double = txt_pointmaxUser.Text
        If UMin > UMax Then
            MessageBox.Show("追加計測点の「最小」に入力する値は「最大」より小さい値を入力して下さい。", "エラー", _
       MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        ElseIf US > UMax Then
            MessageBox.Show("追加計測点の「表示サイズ」に入力する値は「最大」より小さい値を入力して下さい。", "エラー", _
       MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        ElseIf US < UMin Then
            MessageBox.Show("追加計測点の「表示サイズ」に入力する値は「最小」より大きい値を入力して下さい。", "エラー", _
      MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        '13.1.7　カメラの「表示サイズ」「最小」「最大」について
        Dim CS As Double = txt_camerasize.Text
        Dim CMin As Double = txt_cameramin.Text
        Dim CMax As Double = txt_cameramax.Text
        If CMin > CMax Then
            MessageBox.Show("カメラの「最小」に入力する値は「最大」より小さい値を入力して下さい。", "エラー", _
       MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        ElseIf CS > CMax Then
            MessageBox.Show("カメラの「表示サイズ」に入力する値は「最大」より小さい値を入力して下さい。", "エラー", _
       MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        ElseIf CS < CMin Then
            MessageBox.Show("カメラの「表示サイズ」に入力する値は「最小」より大きい値を入力して下さい。", "エラー", _
      MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        '13.1.7　ラベルの「表示サイズ」「最小」「最大」について　By 山田
        Dim LS As Double = txt_labelsize.Text
        Dim LMin As Double = txt_labelmin.Text
        Dim LMax As Double = txt_labelmax.Text
        If LMin > LMax Then
            MessageBox.Show("ラベルの「最小」に入力する値は「最大」より小さい値を入力して下さい。", "エラー", _
       MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        ElseIf LS > LMax Then
            MessageBox.Show("ラベルの「表示サイズ」に入力する値は「最大」より小さい値を入力して下さい。", "エラー", _
       MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        ElseIf LS < LMin Then
            MessageBox.Show("ラベルの「表示サイズ」に入力する値は「最小」より大きい値を入力して下さい。", "エラー", _
      MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
    End Sub

    Private Sub GroupBox1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub Label32_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label32.Click

    End Sub

    Private Sub TextBox8_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox8.TextChanged

    End Sub

    Private Sub CobBPoint1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CobBPoint.SelectedIndexChanged

    End Sub

    Private Sub GroupBox2_Enter(sender As System.Object, e As System.EventArgs) Handles GroupBox2.Enter

    End Sub

    Private Sub Label11_Click(sender As System.Object, e As System.EventArgs) Handles Label11.Click

    End Sub

    Private Sub Label10_Click(sender As System.Object, e As System.EventArgs) Handles Label10.Click

    End Sub

    Private Sub Label9_Click(sender As System.Object, e As System.EventArgs) Handles Label9.Click

    End Sub

    Private Sub Label8_Click(sender As System.Object, e As System.EventArgs) Handles Label8.Click

    End Sub

    Private Sub Label7_Click(sender As System.Object, e As System.EventArgs) Handles Label7.Click

    End Sub
    '★2012.12.26　テキストボックスに値を入力⇒全て '計測点”表示サイズ”と同様の処理


    '==========================================================================================================
    '!!!!!2012.12.27　リボンの拡大/縮小ボタンとの連携がおかしい，チェックが必要：未対応（山田
    '⇒開発プログラムと運用テストを行ったVer.1のもので動作の比較を行った

    '==========================================================================================================
    '計測点”表示サイズ”

    Private Sub txt_pointsize_TextChanged(sender As System.Object, e As System.EventArgs) Handles txt_pointsize.TextChanged,
        txt_pointmin.TextChanged,
        txt_pointmax.TextChanged,
        txt_pointkonomi.TextChanged,
        txt_pointsizeUser.TextChanged,
        txt_pointminUser.TextChanged,
        txt_pointmaxUser.TextChanged,
        txt_pointkonomiUser.TextChanged,
        txt_camerasize.TextChanged,
        txt_cameramin.TextChanged,
        txt_cameramax.TextChanged,
        txt_camerakonami.TextChanged,
        txt_labelsize.TextChanged,
        txt_labelmin.TextChanged,
        txt_labelmax.TextChanged,
        txt_labelkonami.TextChanged,
        cmb_rayLineWidth.TextChanged,
        cmb_LineLineWidth.TextChanged,
        cmb_circleLineWidth.TextChanged,
        cmb_LineLineWidth_CAD.TextChanged,
        cmb_circleLineWidth_CAD.TextChanged

        TextSChanged(sender)

    End Sub
    Public Sub TextSChanged(ByRef objTextBox As TextBox) '
        'Try
        '    If CDbl(objTextBox.Text) > 0 Then
        '    Else
        '        objTextBox.Text = 0.1
        '    End If
        'Catch ex As Exception
        '    objTextBox.Text = 0.1
        'End Try
        If IsNumeric(objTextBox.Text) = False Then
            objTextBox.Text = CStr(objTextBox.Tag) 'Falseの場合：テキストボックスには既存のタグの値
            'Trueでも＜ 0（マイナス）の場合も同様

        ElseIf (objTextBox.Text < 0) Then
            objTextBox.Text = CStr(objTextBox.Tag)
            'objTextBox.Text =""
            'objTextBox.Text = 0.1

        End If
    End Sub
    '13.1.7山田
    Private Sub txt_pointsize_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txt_pointsize.Validating,
        txt_pointmin.Validating,
        txt_pointmax.Validating,
        txt_pointkonomi.Validating,
        txt_pointsizeUser.Validating,
        txt_pointminUser.Validating,
        txt_pointmaxUser.Validating,
        txt_pointkonomiUser.Validating,
        txt_camerasize.Validating,
        txt_cameramin.Validating,
        txt_cameramax.Validating,
        txt_camerakonami.Validating,
        txt_labelsize.Validating,
        txt_labelmin.Validating,
        txt_labelmax.Validating,
        txt_labelkonami.Validating,
        cmb_rayLineWidth.Validating,
        cmb_LineLineWidth.Validating,
        cmb_circleLineWidth.Validating,
        cmb_LineLineWidth_CAD.Validating,
        cmb_circleLineWidth_CAD.Validating


        Dim objTEXTBOX As New TextBox
        objTEXTBOX = sender

        If objTEXTBOX.Text = 0 Then
            e.Cancel = True
            MsgBox("0以外の値を入力してください", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
            objTEXTBOX.Text = CStr(objTEXTBOX.Tag)
        End If
        '  If txt_pointsize.Text = 0 Then
        '      e.Cancel = True
        '      MessageBox.Show("0以外の値を入力してください。", "エラー", _
        'MessageBoxButtons.OK, MessageBoxIcon.Error)
        '      txt_pointsize.Text = txt_pointsize.Tag
        '  End If
    End Sub


    'Private Sub txt_pointmin_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txt_pointmin.Validating
    '    If txt_pointmin.Text = 0 Then
    '        e.Cancel = True
    '        MessageBox.Show("0以外の値を入力してください。", "エラー", _
    '  MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        txt_pointmin.Text = txt_pointmin.Tag
    '    End If
    'End Sub
    'Private Sub txt_pointmax_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txt_pointmax.Validating
    '    If txt_pointmax.Text = 0 Then
    '        e.Cancel = True
    '        MessageBox.Show("0以外の値を入力してください。", "エラー", _
    '  MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        txt_pointmax.Text = txt_pointmax.Tag
    '    End If
    'End Sub
    'Private Sub txt_pointkonomi_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txt_pointkonomi.Validating
    '    If txt_pointkonomi.Text = 0 Then
    '        e.Cancel = True
    '        MessageBox.Show("0以外の値を入力してください。", "エラー", _
    '  MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        txt_pointkonomi.Text = txt_pointkonomi.Tag
    '    End If
    'End Sub
    'Private Sub txt_pointsizeUser_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txt_pointsizeUser.Validating
    '    If txt_pointsizeUser.Text = 0 Then
    '        e.Cancel = True
    '        MessageBox.Show("0以外の値を入力してください。", "エラー", _
    '  MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        txt_pointsizeUser.Text = txt_pointsizeUser.Tag
    '    End If
    'End Sub
    'Private Sub txt_pointminUser_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txt_pointminUser.Validating
    '    If txt_pointminUser.Text = 0 Then
    '        e.Cancel = True
    '        MessageBox.Show("0以外の値を入力してください。", "エラー", _
    '  MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        txt_pointminUser.Text = txt_pointminUser.Tag
    '    End If
    'End Sub
    'Private Sub txt_pointmaxUser_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txt_pointmaxUser.Validating
    '    If txt_pointmaxUser.Text = 0 Then
    '        e.Cancel = True
    '        MessageBox.Show("0以外の値を入力してください。", "エラー", _
    '  MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        txt_pointmaxUser.Text = txt_pointmaxUser.Tag
    '    End If
    'End Sub
    'Private Sub txt_pointkonomiUser_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txt_pointkonomiUser.Validating
    '    If txt_pointkonomiUser.Text = 0 Then
    '        e.Cancel = True
    '        MessageBox.Show("0以外の値を入力してください。", "エラー", _
    '  MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        txt_pointkonomiUser.Text = txt_pointkonomiUser.Tag
    '    End If
    'End Sub
    'Private Sub txt_camerasize_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txt_camerasize.Validating
    '    If txt_camerasize.Text = 0 Then
    '        e.Cancel = True
    '        MessageBox.Show("0以外の値を入力してください。", "エラー", _
    '  MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        txt_camerasize.Text = txt_camerasize.Tag
    '    End If
    'End Sub
    'Private Sub txt_cameramin_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txt_cameramin.Validating
    '    If txt_cameramin.Text = 0 Then
    '        e.Cancel = True
    '        MessageBox.Show("0以外の値を入力してください。", "エラー", _
    '  MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        txt_cameramin.Text = txt_cameramin.Tag
    '    End If
    'End Sub
    'Private Sub txt_cameramax_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txt_cameramax.Validating
    '    If txt_cameramax.Text = 0 Then
    '        e.Cancel = True
    '        MessageBox.Show("0以外の値を入力してください。", "エラー", _
    '  MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        txt_cameramax.Text = txt_cameramax.Tag
    '    End If
    'End Sub
    'Private Sub txt_camerakonami_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txt_camerakonami.Validating
    '    If txt_camerakonami.Text = 0 Then
    '        e.Cancel = True
    '        MessageBox.Show("0以外の値を入力してください。", "エラー", _
    '  MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        txt_camerakonami.Text = txt_camerakonami.Tag
    '    End If
    'End Sub
    'Private Sub txt_labelsize_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txt_labelsize.Validating
    '    If txt_labelsize.Text = 0 Then
    '        e.Cancel = True
    '        MessageBox.Show("0以外の値を入力してください。", "エラー", _
    '  MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        txt_labelsize.Text = txt_labelsize.Tag
    '    End If
    'End Sub
    'Private Sub txt_labelmin_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txt_labelmin.Validating
    '    If txt_labelmin.Text = 0 Then
    '        e.Cancel = True
    '        MessageBox.Show("0以外の値を入力してください。", "エラー", _
    '  MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        txt_labelmin.Text = txt_labelmin.Tag
    '    End If
    'End Sub
    'Private Sub txt_labelmax_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txt_labelmax.Validating
    '    If txt_labelmax.Text = 0 Then
    '        e.Cancel = True
    '        MessageBox.Show("0以外の値を入力してください。", "エラー", _
    '  MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        txt_labelmax.Text = txt_labelmax.Tag
    '    End If
    'End Sub
    'Private Sub txt_labelkonami_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txt_labelkonami.Validating
    '    If txt_labelkonami.Text = 0 Then
    '        e.Cancel = True
    '        MessageBox.Show("0以外の値を入力してください。", "エラー", _
    '  MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        txt_labelkonami.Text = txt_labelkonami.Tag
    '    End If
    'End Sub
    'Private Sub cmb_rayLineWidth_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cmb_rayLineWidth.Validating
    '    If cmb_rayLineWidth.Text = 0 Then
    '        e.Cancel = True
    '        MessageBox.Show("0以外の値を入力してください。", "エラー", _
    '  MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        cmb_rayLineWidth.Text = cmb_rayLineWidth.Tag
    '    End If
    'End Sub
    'Private Sub cmb_LineLineWidth_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cmb_LineLineWidth.Validating
    '    If cmb_LineLineWidth.Text = 0 Then
    '        e.Cancel = True
    '        MessageBox.Show("0以外の値を入力してください。", "エラー", _
    '  MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        cmb_LineLineWidth.Text = cmb_LineLineWidth.Tag
    '    End If
    'End Sub
    'Private Sub cmb_circleLineWidth_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cmb_circleLineWidth.Validating
    '    If cmb_circleLineWidth.Text = 0 Then
    '        e.Cancel = True
    '        MessageBox.Show("0以外の値を入力してください。", "エラー", _
    '  MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        cmb_circleLineWidth.Text = cmb_circleLineWidth.Tag
    '    End If
    'End Sub
    'Private Sub cmb_LineLineWidth_CAD_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cmb_LineLineWidth_CAD.Validating
    '    If cmb_LineLineWidth_CAD.Text = 0 Then
    '        e.Cancel = True
    '        MessageBox.Show("0以外の値を入力してください。", "エラー", _
    '  MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        cmb_LineLineWidth_CAD.Text = cmb_LineLineWidth_CAD.Tag
    '    End If
    'End Sub
    'Private Sub cmb_circleLineWidth_CAD_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cmb_circleLineWidth_CAD.Validating
    '    If cmb_circleLineWidth_CAD.Text = 0 Then
    '        e.Cancel = True
    '        MessageBox.Show("0以外の値を入力してください。", "エラー", _
    '  MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        cmb_circleLineWidth_CAD.Text = cmb_circleLineWidth_CAD.Tag
    '    End If
    'End Sub
    '2012.12.27　カット & ペーストでの数値以外の文字の入力破棄

    'Public Sub TextSChanged(ByRef objTextBox As TextBox) '
    '    Dim newText As System.Text.StringBuilder = New System.Text.StringBuilder(Text.Length
    '    For Each ch As Char In objTextBox.Text'テキストボックスの Text プロパティの値を一文字ずつ数値か否かを判断（数値：テキストボックス出力/数値ではない：破棄）

    '        If Char.IsDigit(ch) = True Then'指定した文字 (ch) が 10 進数の数値かどうかを判別(数値:「newText.Append(ch)」でメモリ内に新しい文字列オブジェクトを作成)
    '            newText.Append(ch)'メモリ内に作成した新しい文字列オブジェクトをテキストボックスに入力

    '        End If
    '    Next
    '    If (objTextBox.TextLength <> newText.Length) Then
    '        Dim start As Integer = objTextBox.SelectionStart
    '        Dim len As Integer = objTextBox.SelectionLength
    '        objTextBox.Text = newText.ToString()
    '        If start >= 0 And len >= 0 Then
    '            objTextBox.SelectionStart = start
    '            objTextBox.SelectionLength = len
    '        End If
    '    End If
    'End Sub


    '＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝

    ''計測点”最小”

    'Private Sub txt_pointmin_TextChanged(sender As System.Object, e As System.EventArgs) Handles txt_pointmin.TextChanged

    'End Sub
    ''計測点”最大”

    'Private Sub txt_pointmax_TextChanged(sender As System.Object, e As System.EventArgs) Handles txt_pointmax.TextChanged

    'End Sub
    ''計測点”刻み”

    'Private Sub txt_pointkonomi_TextChanged(sender As System.Object, e As System.EventArgs) Handles txt_pointkonomi.TextChanged

    'End Sub

    ''追加計測点”表示サイズ”

    'Private Sub txt_pointsizeUser_TextChanged(sender As System.Object, e As System.EventArgs) Handles txt_pointsizeUser.TextChanged

    'End Sub
    ''追加計測点”最小”

    'Private Sub txt_pointminUser_TextChanged(sender As System.Object, e As System.EventArgs) Handles txt_pointminUser.TextChanged

    'End Sub
    ''追加計測点”最小”

    'Private Sub txt_pointmaxUser_TextChanged(sender As System.Object, e As System.EventArgs) Handles txt_pointmaxUser.TextChanged

    'End Sub
    ''追加計測点”刻み”

    'Private Sub txt_pointkonomiUser_TextChanged(sender As System.Object, e As System.EventArgs) Handles txt_pointkonomiUser.TextChanged

    'End Sub
    ''カメラ”表示サイズ”

    'Private Sub txt_camerasize_TextChanged(sender As System.Object, e As System.EventArgs) Handles txt_camerasize.TextChanged

    'End Sub
    ''カメラ”最小”

    'Private Sub txt_cameramin_TextChanged(sender As System.Object, e As System.EventArgs) Handles txt_cameramin.TextChanged

    'End Sub
    ''カメラ”最大”

    'Private Sub txt_cameramax_TextChanged(sender As System.Object, e As System.EventArgs) Handles txt_cameramax.TextChanged

    'End Sub
    ''カメラ”刻み”

    'Private Sub txt_camerakonami_TextChanged(sender As System.Object, e As System.EventArgs) Handles txt_camerakonami.TextChanged

    'End Sub
    ''ラベル”表示サイズ”

    'Private Sub txt_labelsize_TextChanged(sender As System.Object, e As System.EventArgs) Handles txt_labelsize.TextChanged

    'End Sub
    ''ラベル”最小”

    'Private Sub txt_labelmin_TextChanged(sender As System.Object, e As System.EventArgs) Handles txt_labelmin.TextChanged

    'End Sub
    'ラベル”最大”

    'Private Sub txt_labelmax_TextChanged(sender As System.Object, e As System.EventArgs) Handles txt_labelmax.TextChanged

    'End Sub
    ' ''ラベル”刻み”

    ''Private Sub txt_labelkonami_TextChanged(sender As System.Object, e As System.EventArgs) Handles txt_labelkonami.TextChanged

    ''End Sub
    ''レイ”線の幅”

    'Private Sub cmb_rayLineWidth_TextChanged(sender As System.Object, e As System.EventArgs) Handles cmb_rayLineWidth.TextChanged

    'End Sub
    ''線分（任意図形）”線の幅”

    'Private Sub cmb_LineLineWidth_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmb_LineLineWidth.TextChanged

    'End Sub
    ''円（任意図形）”線の幅”

    'Private Sub cmb_circleLineWidth_TextChanged(sender As System.Object, e As System.EventArgs) Handles cmb_circleLineWidth.TextChanged

    'End Sub
    ''線分（CAD図形）”線の幅”

    'Private Sub cmb_LineLineWidth_CAD_TextChanged(sender As System.Object, e As System.EventArgs) Handles cmb_LineLineWidth_CAD.TextChanged

    'End Sub
    ' ''円（CAD図形）”線の幅”

    ''Private Sub cmb_circleLineWidth_CAD_TextChanged(sender As System.Object, e As System.EventArgs) Handles cmb_circleLineWidth_CAD.TextChanged

    ''End Sub

    Private Sub TextBox9_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox9.TextChanged

    End Sub
    Private Sub CheckBox3_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox3.CheckedChanged

    End Sub

End Class