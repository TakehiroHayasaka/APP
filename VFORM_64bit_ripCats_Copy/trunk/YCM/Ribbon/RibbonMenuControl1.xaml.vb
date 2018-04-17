Public Class RibbonMenuControl1
#Region "メニュアプリケーション"
    'ファイル-新規作成
    Private Sub RbnFileNew(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.FileNew()
    End Sub
    'ファイル-開く
    Private Sub RbnFileOpen(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

        Call MainFrm.FileOpen()
    End Sub
    'ファイル-閉じる

    Private Sub RbnFileClose(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.FileClose()
    End Sub
    'ファイル-上書き保存

    Private Sub RbnFileSave(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.FileSave()
    End Sub
    'ファイル-名前を付けて保存

    Private Sub RbnFileSaveAs(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.FileSaveAs()
    End Sub
    'ファイル-CSV書き出し

    Private Sub RbnFileCSVOut(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.FileCSVOut()
    End Sub
    'ファイル-終了======未実装

    Private Sub RbnFileEnd(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.FileEnd()
    End Sub
#End Region

#Region "画面変更関連"
    '画像１

    Private Sub RbnChgViewImage1(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.ChgViewImage1()
    End Sub
    '画像４

    Private Sub RbnChgViewImage4(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.ChgViewImage4()
    End Sub
    '画像１＋3DView
    Private Sub RbnChgViewImage1_3DView(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.ChgViewImage1_3DView()
    End Sub
    '画像４＋3DView
    Private Sub RbnChgViewImage4_3DView(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.ChgViewImage4_3DView()
    End Sub
    '3DView
    Private Sub RbnChgView3DView(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.ChgView3DView()
    End Sub
#End Region

#Region "画像操作タブ(解析タブ)"
    ''解析-ターゲット抽出
    'Private Sub RbnBtnAnaTargetExt(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
    '    Call MainFrm.BtnAnaTargetExt()
    'End Sub

    '解析-一括処理

    Private Sub RbnBtnAnaBatch(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnAnaBatch()
        flgManualScaleAndOffset = True 'SUSANO ADD START 20160526
    End Sub
    '解析-バンドル==-未実装20121127!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    Private Sub RbnBtnAnaBundle(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.RunBundleAdj()
    End Sub
    '解析-3D点点作成==-未実装20121127!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    Private Sub RbnBtnAnaCreat3DPoint(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.AddUserPoint3D()
    End Sub
    '解析-3D点削除
    Private Sub RbnBtnAnaDel3DPoint(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        '  Call MainFrm.BtnAnaDel3DPoint()
        '    Call MainFrm.BtnDel3DPoint()
        '   Call IOUtil.LibCommand("CreateTargetPoint3d")
        Call IOUtil.LibCommand("DeletePoint3ds")
        'If MainFrm.flgCreateSTEnter = True Then
        '    YMC_3DViewReDraw_NoKousin(m_strDataBasePath)
        'End If

    End Sub
    '20170222 baluu add start
    Private Sub RbnBtnAnaDelFrom3D(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnDelPointsFrom3D()
    End Sub
    '20170222 baluu add end
    'Private Sub 未実装ボタン1(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) 'H25.5.8イベント追加　Yamada
    '    YCM_Offset_GenData(MainFrm.objFBM)
    'End Sub
    'Private Sub 未実装ボタン2(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) 'H25.5.8イベント追加　Yamada
    '    '鈴木 20130513 追加
    '    '基本情報入力画面
    '    'KihonInfoDialog.Show()
    '    'KihonInfoDialogNew.Show()
    '    gKihonInfoTabDialog.Show()

    '    'End Sub
    Private Sub 未実装ボタン3(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) 'H25.5.8イベント追加　Yamada
        '  Call MainFrm.AddUserPoint3D_FourDisp()
        '     Call MainFrm.AddUserPoint3D_FourDisp_Extra()
        ' MainFrm.CallCreateTargetPoint3d()
        Call IOUtil.LibCommand("CreateTargetPoint3d")

    End Sub
    'Private Sub 未実装ボタン4(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) 'H25.5.8イベント追加　Yamada
    '    gExtraMainForm.Show()

    'End Sub


    '解析-輝度調整(明るく)
    Private Sub RbnBtnAnaBright(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        ' Call MainFrm.BtnAnaBright()
    End Sub
    Dim flgBright As Boolean = False
    Private Sub RbnBtnAnaBrightPrevMouDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        flgBright = True
        '20150304 Rep By Yamada 20150304 Sta --------無限ループ修正
        If flgBright = True Then
            Call MainFrm.BtnAnaBright()
            System.Windows.Forms.Application.DoEvents()
        End If
        'Do While flgBright
        '    Call MainFrm.BtnAnaBright()
        '    System.Windows.Forms.Application.DoEvents()
        'Loop
        '20150304 Rep By Yamada 20150304 End --------無限ループ修正
    End Sub
    Private Sub RbnBtnAnaBrightPrevMouUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        flgBright = False
    End Sub
    Private Sub RbnBtnAnaBrightMouseLeave(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs)
        flgBright = False
    End Sub

    '解析-輝度調整(暗く)
    Private Sub RbnBtnAnaDark(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        ' Call MainFrm.BtnAnaDark()
    End Sub
    Dim flgDark As Boolean = False
    Private Sub RbnBtnAnaDarkPrevMouDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        flgDark = True
        '20150304 Rep By Yamada 20150304 Sta --------無限ループ修正
        If flgDark = True Then
            Call MainFrm.BtnAnaDark()
            System.Windows.Forms.Application.DoEvents()
        End If
        'Do While flgDark
        '    Call MainFrm.BtnAnaDark()
        '    System.Windows.Forms.Application.DoEvents()
        'Loop
        '20150304 Rep By Yamada 20150304 End --------無限ループ修正
    End Sub
    Private Sub RbnBtnAnaDarkPrevMouUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        flgDark = False
    End Sub
    Private Sub RbnBtnAnaDarkMouseLeave(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs)
        flgDark = False
    End Sub
#End Region
#Region "3D操作タブ(表示タブ)"
    '表示-全体

    Private Sub RbnBtnDispZoomAll(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnDispZoomAll()
    End Sub
    '表示-範囲
    Private Sub RbnBtnDispZoomWin(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnDispZoomWin()
    End Sub
    '表示-拡大
    Private Sub RbnBtnDispZoomIn(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnDispZoomIn()
    End Sub
    '表示-縮小

    Private Sub RbnBtnDispZoomOut(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnDispZoomOut()
    End Sub
    '表示-画面移動

    Private Sub RbnBtnDispScroll(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnDispScroll()
    End Sub
    '表示-3D回転
    Private Sub RbnBtnDispRot3D(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnDispRot3D()
    End Sub
    ''表示-カメラ視点
    'Private Sub RbnBtnDispCameraView(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
    '    Call MainFrm.BtnDispCameraView()
    'End Sub
    '表示-XY平面
    Private Sub RbnBtnDispXYPlane(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnDispXYPlane()
    End Sub
    '表示-XZ平面
    Private Sub RbnBtnDispXZPlane(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnDispXZPlane()
    End Sub
    '表示-YZ平面
    Private Sub RbnBtnDispYZPlane(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnDispYZPlane()
    End Sub
    ' ''--rep.del.start--------------------------未使用
    '表示-計測点表示／非表示====未実装

    Private Sub RbnChkBoxDispMeasPointOnOff(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        ''Call MainFrm.ChkBoxDispMeasPointOnOff()
    End Sub
    '表示-追加計測点表示／非表示====未実装

    Private Sub RbnChkBoxDispAddMeasPointOnOff(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        ''Call MainFrm.ChkBoxDispAddMeasPointOnOff()
    End Sub
    '表示-カメラ表示／非表示
    Private Sub RbnChkBoxDispCameraOnOff(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        ''Call MainFrm.ChkBoxDispCameraOnOff()
    End Sub
    '表示-レイ表示／非表示====未実装

    Private Sub RbnChkBoxDispRayOnOff(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        ''Call MainFrm.ChkBoxDispRayOnOff()
    End Sub
    '表示-ラベル表示／非表示
    Private Sub RbnChkBoxDispLabelOnOff(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        ''Call MainFrm.ChkBoxDispLabelOnOff()
    End Sub
    '表示-任意図形表示／非表示====未実装

    Private Sub RbnChkBoxDispFigOnOff(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        ''Call MainFrm.ChkBoxDispFigOnOff()
    End Sub
    ' ''--rep.del.end----------------------------
    '表示-計測点表示／非表示
    Private Sub RbnChkBoxDispMeasPoint_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RbnChkBoxDispMeasPoint.Checked
        Call MainFrm.ChkBoxDispMeasPointOnOff(True)
        '        entset_point.blnVisiable = True
    End Sub
    Private Sub RbnChkBoxDispMeasPoint_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RbnChkBoxDispMeasPoint.Unchecked
        Call MainFrm.ChkBoxDispMeasPointOnOff(False)
        '        entset_point.blnVisiable = False
    End Sub
    '表示-追加計測点表示／非表示
    Private Sub RbnChkBoxDispAddMeasPoint_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RbnChkBoxDispAddMeasPoint.Checked
        Call MainFrm.ChkBoxDispAddMeasPointOnOff(True)
    End Sub
    Private Sub RbnChkBoxDispAddMeasPoint_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RbnChkBoxDispAddMeasPoint.Unchecked
        Call MainFrm.ChkBoxDispAddMeasPointOnOff(False)
    End Sub
    '表示-カメラ表示／非表示
    Private Sub RbnChkBoxDispCamera_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RbnChkBoxDispCamera.Checked
        Call MainFrm.ChkBoxDispCameraOnOff(True)
    End Sub
    Private Sub RbnChkBoxDispCamera_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RbnChkBoxDispCamera.Unchecked
        Call MainFrm.ChkBoxDispCameraOnOff(False)
    End Sub
    '表示-レイ表示／非表示
    Private Sub RbnChkBoxDispRay_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RbnChkBoxDispRay.Checked
        Call MainFrm.ChkBoxDispRayOnOff(True)
    End Sub
    Private Sub RbnChkBoxDispRay_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RbnChkBoxDispRay.Unchecked
        Call MainFrm.ChkBoxDispRayOnOff(False)
    End Sub
    '表示-ラベル表示／非表示
    Private Sub RbnChkBoxDispLabel_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RbnChkBoxDispLabel.Checked
        Call MainFrm.ChkBoxDispLabelOnOff(True)
    End Sub
    Private Sub RbnChkBoxDispLabel_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RbnChkBoxDispLabel.Unchecked
        Call MainFrm.ChkBoxDispLabelOnOff(False)
    End Sub
    ''表示-任意図形表示／非表示====未実装★後々コメントに
    'Private Sub RbnChkBoxDispFig_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RbnChkBoxDispFig.Checked
    '    Call MainFrm.ChkBoxDispFigOnOff(True)
    'End Sub
    'Private Sub RbnChkBoxDispFig_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RbnChkBoxDispFig.Unchecked
    '    Call MainFrm.ChkBoxDispFigOnOff(False)
    'End Sub
    '表示-線分(任意図形)表示／非表示
    Private Sub RbnChkBoxDispFigLine_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RbnChkBoxDispFigLine.Checked
        Call MainFrm.ChkBoxDispFigLineOnOff(True)
    End Sub
    Private Sub RbnChkBoxDispFigLine_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RbnChkBoxDispFigLine.Unchecked
        Call MainFrm.ChkBoxDispFigLineOnOff(False)
    End Sub
    '表示-円(任意図形)表示／非表示
    Private Sub RbnChkBoxDispFigCircle_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RbnChkBoxDispFigCircle.Checked
        Call MainFrm.ChkBoxDispFigCircleOnOff(True)
    End Sub
    Private Sub RbnChkBoxDispFigCircle_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RbnChkBoxDispFigCircle.Unchecked
        Call MainFrm.ChkBoxDispFigCircleOnOff(False)
    End Sub
    '表示-線分(CAD図形)表示／非表示
    Private Sub RbnChkBoxDispCadLine_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RbnChkBoxDispCadLine.Checked
        Call MainFrm.ChkBoxDispCadLineOnOff(True)
    End Sub
    Private Sub RbnChkBoxDispCadLine_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RbnChkBoxDispCadLine.Unchecked
        Call MainFrm.ChkBoxDispCadLineOnOff(False)
    End Sub
    '表示-円(CAD図形)表示／非表示
    Private Sub RbnChkBoxDispCadCircle_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RbnChkBoxDispCadCircle.Checked
        Call MainFrm.ChkBoxDispCadCircleOnOff(True)
    End Sub
    Private Sub RbnChkBoxDispCadCircle_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RbnChkBoxDispCadCircle.Unchecked
        Call MainFrm.ChkBoxDispCadCircleOnOff(False)
    End Sub
    '表示-コードターゲット表示／非表示
    Private Sub RbnChkBoxDispCordPoint_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RbnChkBoxDispCordPoint.Checked
        Call MainFrm.ChkBoxDispCordPointOnOff(True)
    End Sub
    Private Sub RbnChkBoxDispCordPoint_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RbnChkBoxDispCordPoint.Unchecked
        Call MainFrm.ChkBoxDispCordPointOnOff(False)
    End Sub

    '表示／非表示チェックボックスの設定

    Public Sub setRbnChkBoxOnOff(ByVal iType As Integer, ByVal bOn As Boolean)
        'iType=1:計測点、=2:追加計測点、=3:カメラ、=4:レイ、=5:ラベル
        'iType=6：線分(任意図形)、=7：円(任意図形)、=8：線分(CAD図形)、=9：円(CAD図形)、=10：コードターゲット
        Select Case iType
            Case 1  '計測点
                RbnChkBoxDispMeasPoint.IsChecked = bOn
            Case 2  '追加計測点
                RbnChkBoxDispAddMeasPoint.IsChecked = bOn
            Case 3  'カメラ
                RbnChkBoxDispCamera.IsChecked = bOn
            Case 4  'レイ
                RbnChkBoxDispRay.IsChecked = bOn
            Case 5  'ラベル
                RbnChkBoxDispLabel.IsChecked = bOn
                'Case 6  '任意図形(後々コメント)
                '    RbnChkBoxDispFig.IsChecked = bOn
            Case 6 '線分(任意図形)
                RbnChkBoxDispFigLine.IsChecked = bOn
            Case 7 '円(任意図形)
                RbnChkBoxDispFigCircle.IsChecked = bOn
            Case 8 '線分(CAD図形)
                RbnChkBoxDispCadLine.IsChecked = bOn
            Case 9 '円(CAD図形)
                RbnChkBoxDispCadCircle.IsChecked = bOn
            Case 10 'コードターゲット
                RbnChkBoxDispCordPoint.IsChecked = bOn
            Case Else
        End Select
    End Sub

    '表示-座標値リスト====未実装

    Private Sub RbnBtnDispCoordList(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnDispCoordList()
    End Sub
    '表示-計測点拡大
    Private Sub RbnBtnDispMarkerBig(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnDispMarkerBig()
    End Sub
    'Dim flgBtnDispMarkerBig As Boolean = False
    'Private Sub RbnBtnDispMarkerBigPrevMouDown(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs)
    '    flgBtnDispMarkerBig = True
    '    Do While flgBtnDispMarkerBig
    '        Call MainFrm.BtnDispMarkerBig()
    '        System.Windows.Forms.Application.DoEvents()
    '    Loop
    'End Sub
    'Private Sub RbnBtnDispMarkerBigPrevMouUp(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs)
    '    flgBtnDispMarkerBig = False
    'End Sub
    'Private Sub RbnBtnDispMarkerBigMouseLeave(sender As System.Object, e As System.Windows.Input.MouseEventArgs)
    '    flgBtnDispMarkerBig = False
    'End Sub
    '表示-計測点縮小

    Private Sub RbnBtnDispMarkerSmall(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnDispMarkerSmall()
    End Sub
    '表示-カメラ拡大
    Private Sub RbnBtnDispCameraBig(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnDispCameraBig()
    End Sub
    '表示-カメラ縮小

    Private Sub RbnBtnDispCameraSmall(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnDispCameraSmall()
    End Sub
    '表示-ラベル拡大
    Private Sub RbnBtnDispLabelBig(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnDispLabelBig()
    End Sub
    '表示-ラベル縮小

    Private Sub RbnBtnDispLabelSmall(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnDispLabelSmall()
    End Sub
#End Region
#Region "ツールタブ"
    'ツール－任意線分
    Private Sub RbnBtnToolCreatLine(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnToolCreatLine()
    End Sub
    'ツール－3点円

    Private Sub RbnBtnToolCreatCircle(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnToolCreatCircle()
    End Sub
    'ツール－中心円
    Private Sub RbnBtnToolCreatCentralCircle(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnToolCreatCentralCircle()
    End Sub
    'ツール－作図属性
    Private Sub RbnBtnToolDrawAttrSet(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnToolDrawAttrSet()
    End Sub
    'ツール－座標変換設定

    Private Sub RbnBtnToolCoordConvSet(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnToolCoordConvSet()
    End Sub
    'ツール－スケール設定

    Private Sub RbnBtnToolScaleSet(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnToolScaleSet()
    End Sub
    'ツール－2点間距離
    Private Sub RbnBtnToolDistance2Point(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnToolDistance2Point()
    End Sub
    'ツール－ラベリング
    Private Sub RbnBtnToolAutoLabeling(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnToolAutoLabeling()
    End Sub
    'ツール－手動ラベリング
    Private Sub RbnBtnToolManuLabeling(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnToolManuLabeling()
    End Sub
    'ツール－点作成(一括)
    Private Sub RbnBtnToolCreatPointBatch(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnToolCreatPointBatch()
    End Sub
    'ツール－CAD起動

    Private Sub RbnBtnToolCadStart(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnToolCadStart()
    End Sub
    'ツール－CAD図形読み込み
    Private Sub RbnBtnToolCadRead(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnToolCadRead()
    End Sub
    'ツール－CAD図形書き出し

    Private Sub RbnBtnToolCadWrite(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.BtnToolCadWrite()
    End Sub
#End Region

    'Private Sub RibbonMenuControl1_IsVisibleChanged(ByVal sender As Object, ByVal e As System.Windows.DependencyPropertyChangedEventArgs) Handles Me.IsVisibleChanged
    '    Debug.Print("RibbonMenuControl1_IsVisibleChanged")
    'End Sub

    'Private Sub RibbonMenuControl1_RequestBringIntoView(ByVal sender As Object, ByVal e As System.Windows.RequestBringIntoViewEventArgs) Handles Me.RequestBringIntoView
    '    Debug.Print("RibbonMenuControl1_RequestBringIntoView")
    '    'このイベント

    'End Sub

    'Private Sub RibbonMenuControl1_LayoutUpdated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LayoutUpdated
    '    Debug.Print("RibbonMenuControl1_LayoutUpdated")
    '    'このイベント

    'End Sub

    'Private Sub Ribbon1_ToolTipClosing(ByVal sender As System.Object, ByVal e As System.Windows.Controls.ToolTipEventArgs)
    '    Debug.Print("Ribbon1_ToolTipClosing")
    'End Sub

    'Private Sub RibbonMenuControl1_ContextMenuClosing(ByVal sender As Object, ByVal e As System.Windows.Controls.ContextMenuEventArgs) Handles Me.ContextMenuClosing
    '    Debug.Print("RibbonMenuControl1_ContextMenuClosing")
    'End Sub

    'Private Sub RibbonMenuControl1_ContextMenuOpening(ByVal sender As Object, ByVal e As System.Windows.Controls.ContextMenuEventArgs) Handles Me.ContextMenuOpening
    '    Debug.Print("RibbonMenuControl1_ContextMenuOpening")
    'End Sub

    'Private Sub RibbonMenuControl1_DataContextChanged(ByVal sender As Object, ByVal e As System.Windows.DependencyPropertyChangedEventArgs) Handles Me.DataContextChanged
    '    Debug.Print("RibbonMenuControl1_DataContextChanged")
    'End Sub

    'Private Sub RibbonMenuControl1_SizeChanged(ByVal sender As Object, ByVal e As System.Windows.SizeChangedEventArgs) Handles Me.SizeChanged
    '    Debug.Print("RibbonMenuControl1_SizeChanged")
    'End Sub

    'Private Sub RibbonMenuControl1_Collapsed(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Ribbon1.Collapsed
    '    Debug.Print("Ribbon1_Collapsed")
    'End Sub

    'Private Sub RibbonMenuControl1_Expanded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Ribbon1.Expanded
    '    Debug.Print("Ribbon1_Expanded")
    'End Sub

    Private Sub 未実装ボタン1(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        '  Call MainFrm.AddAllEdgePoint3D()

        Call IOUtil.LibCommand("EdgeToPoint3d")
    End Sub

    Private Sub 未実装ボタン2(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Call MainFrm.AddEdgePoint3D()
    End Sub

    Private Sub 未実装ボタン4(sender As System.Object, e As System.Windows.RoutedEventArgs)
        ' Call MainFrm.AddUserClickPoint3D_FourDisp()
        ' Call MainFrm.AddUserClickPoint3D_FourDisp_extra()
        Call IOUtil.LibCommand("CreateClickPoint3d")
    End Sub

    Private Sub RbnBtnToolAutoOffsetCalc(sender As Object, e As Windows.RoutedEventArgs)
        MainFrm.ShowAutoOffsetCalc()

    End Sub

    Private Sub RbnBtnToolReconstruct(sender As Object, e As Windows.RoutedEventArgs)
        MainFrm.OpenReconstruct()
    End Sub

    '20170224 baluu add start
    Private Sub RbnBtnToolTgReconstruct(sender As Object, e As Windows.RoutedEventArgs)
        MainFrm.OpenTgReconstruct()
    End Sub
    '20170224 baluu add end
    '20170209 baluu add start
    Private Sub ToggleUserLineButton(sender As Object, e As Windows.RoutedEventArgs)
        View_Dilaog.DrawUserLine = Not View_Dilaog.DrawUserLine
        If View_Dilaog.DrawUserLine = True Then
            Button_UserLine.SmallImageSource = New BitmapImage(New Uri("pack://application:,,,/YCM;component/Images/1353986904_bulb yellow.png", UriKind.RelativeOrAbsolute))
            Button_UserLine.LargeImageSource = New BitmapImage(New Uri("pack://application:,,,/YCM;component/Images/1353986904_bulb yellow.png", UriKind.RelativeOrAbsolute))
        Else
            Button_UserLine.SmallImageSource = New BitmapImage(New Uri("pack://application:,,,/YCM;component/Images/1353984272_bulb blue.png", UriKind.RelativeOrAbsolute))
            Button_UserLine.LargeImageSource = New BitmapImage(New Uri("pack://application:,,,/YCM;component/Images/1353984272_bulb blue.png", UriKind.RelativeOrAbsolute))
        End If
    End Sub

    Private Sub ToggleSunpoLineButton(sender As Object, e As Windows.RoutedEventArgs)
        View_Dilaog.DrawSunpoLine = Not View_Dilaog.DrawSunpoLine
        If View_Dilaog.DrawSunpoLine = True Then
            Button_SunpoLine.SmallImageSource = New BitmapImage(New Uri("pack://application:,,,/YCM;component/Images/1353986904_bulb yellow.png"))
            Button_SunpoLine.LargeImageSource = New BitmapImage(New Uri("pack://application:,,,/YCM;component/Images/1353986904_bulb yellow.png"))
        Else
            Button_SunpoLine.SmallImageSource = New BitmapImage(New Uri("pack://application:,,,/YCM;component/Images/1353984272_bulb blue.png"))
            Button_SunpoLine.LargeImageSource = New BitmapImage(New Uri("pack://application:,,,/YCM;component/Images/1353984272_bulb blue.png"))
        End If
    End Sub
    Private Sub ToggleSekkeiLineButton(sender As Object, e As Windows.RoutedEventArgs)
        View_Dilaog.ToggleSekkeiLine()
        If View_Dilaog.DrawSekkeiLine = True Then
            Button_SekkeiLine.SmallImageSource = New BitmapImage(New Uri("pack://application:,,,/YCM;component/Images/1353986904_bulb yellow.png"))
            Button_SekkeiLine.LargeImageSource = New BitmapImage(New Uri("pack://application:,,,/YCM;component/Images/1353986904_bulb yellow.png"))
        Else
            Button_SekkeiLine.SmallImageSource = New BitmapImage(New Uri("pack://application:,,,/YCM;component/Images/1353984272_bulb blue.png"))
            Button_SekkeiLine.LargeImageSource = New BitmapImage(New Uri("pack://application:,,,/YCM;component/Images/1353984272_bulb blue.png"))
        End If
    End Sub
    '20170209 baluu add end

    Private Sub UserControl_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

    End Sub
End Class
