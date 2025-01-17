﻿Imports System.Runtime.InteropServices
Imports System
Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic.FileIO


Public Class FrmMain
    Dim hwinid1 As Object
    Dim hwinid2 As Object
    Dim hvWinPrev As Object

    Dim lst_AcqHandle As List(Of Object)
    Dim lstCamParam As List(Of CameraParam)

    Dim blnCamLive As Boolean
    ' Dim lst_resultImage As List(Of HALCONXLib.HUntypedObjectX)

    Dim PrevImageCid As Integer

    Dim resultImageFolder As String = "" '= My.Application.Info.DirectoryPath & "\ResultImage\"

    Dim resultImageFileName As String = "ShootImage"

    '20150202 ADD BY Yamada Sta --------
    '  Dim F2 As New Form2         '画像選択機能（未使用）
    'Dim F3 As New Form3         '画像選択機能
    Dim blnNewKouji As Boolean  '新規工事フラグ（=True：新規工事／=False：既存工事を継続）
    Private m_fbd As FolderBrowserDialog        '新規フォルダ作成可
    Private m_fbd_s As FolderBrowserDialog      '参照のみ
    Private m_fod As OpenFileDialog             'フォルダ指定用
    ''20150202 ADD BY Yamada Sta --------

    Private strKojiMei As String = ""
    Dim flgCTari As Boolean = True
    Private Const blnParaShoot As Boolean = True
    ' Private Const blnDotNet As Boolean = True
    Dim intW As Integer
    Dim intH As Integer
    Dim intZW As Integer
    Dim intZH As Integer

    Public Ctrl_MouseWheel As Integer = 0
    Dim ViewTarget1 As TargetDetect = Nothing
    Dim viewtarget2 As TargetDetect = Nothing
    Dim ViewImage1 As HALCONXLib.HUntypedObjectX = Nothing
    Dim ViewImage2 As HALCONXLib.HUntypedObjectX = Nothing

    Dim AllTargetRegion1 As HALCONXLib.HUntypedObjectX = Nothing 'SUURI ADD 20150412
    Dim AllTargetRegion2 As HALCONXLib.HUntypedObjectX = Nothing 'SUURI ADD 20150412
    Dim AllTargetList1 As List(Of TargetDetect) 'SUURI ADD 20150412
    Dim Alltargetlist2 As List(Of TargetDetect) 'SUURI ADD 20150412

    Dim ZoomFactor As Integer = 1.5  '--10
    Dim minZoomSize As Integer = 5 '最小表示範囲ＰＩＸＥＬ
    Dim flgDraw As Boolean = False

    '( 20150805 Kiryu Add ) 解析画面ボタン操作
    Dim Center_x As Integer = 3840 / 2  '(解析画像のセンター座標初期値)
    Dim Center_y As Integer = 2748 / 2  '(解析画像のセンター座標初期値)
    Const MoveVal As Integer = 100

    Dim iReShootMaxCnt As Integer = 3
    Dim iTimerInterval As Integer = 100

    Dim strNewKoujiFolder As String = My.Settings.KoujiRootFolder & "\" & "Data" & Now.ToFileTimeUtc
    Dim strBeforeKoujiFolder As String = ""

    Public SenserSet As Sensser_setting     '(20150731 Tezuka ADD)
    Dim SensorPeak As Double                '(20150804 Tezuka ADD) ピーク計測値保存用
    Dim ActiveDataFlg As Integer
    Dim Incident_All As Integer = 480       '(20150916 Tezuka ADD) 光軸の最大値
    Public DoveModeFlg As Integer              '(20150916 Tezuka ADD) センサー常時監視モード（ON:1  OFF:0)

    'GIFアニメーションプログレスバー関連（テスト）
    Dim iRingProgress As Integer

    Dim hrCarSearchRegion1 As HalconDotNet.HRegion
    Dim hrCarSearchRegion2 As HalconDotNet.HRegion

    Dim threadNoCar As System.Threading.Thread
    Dim timerNoCar As Windows.Forms.Timer

    Dim flgKaisekiKahi As Boolean = False
    Dim lstPeakValue As New List(Of Double)
    Dim flgJyojiStart As Boolean = False

    Private Function ResizePart(ByRef win As Object, ByVal Delta As Integer) As Boolean
        Dim R As Object = Nothing, C As Object = Nothing
        Dim Rw1 As Object = Nothing, Cw1 As Object = Nothing
        Dim Rw2 As Object = Nothing, Cw2 As Object = Nothing
        Dim Rd1 As Object = Nothing, Cd1 As Object = Nothing
        Dim Rd2 As Object = Nothing, Cd2 As Object = Nothing

        Dim ZF As Double
        Try

            Dim B As Object = Nothing
            Op.GetMposition(win, R, C, B)
            '( 20150820 Kiryu Add )ズーム位置を保持
            Center_x = C
            Center_y = R
            'Kiryu Add End
            Op.GetPart(win, Rw1, Cw1, Rw2, Cw2)
            If (Delta > 0) Then
                ZF = 1 / ZoomFactor
            Else
                ZF = ZoomFactor
            End If

            Cd1 = C - (C - Cw1) * ZF
            Cd2 = C + (Cw2 - C) * ZF
            Rd1 = R - (R - Rw1) * ZF
            Rd2 = Rd1 + CInt(Math.Abs(Cd2 - Cd1) * (intH / intW))

            If (Rd1 < 0) Then Rd1 = 0
            If (Cd1 < 0) Then Cd1 = 0
            If (Rd2 > intH) Then Rd2 = intH '-ZH
            If (Cd2 > intW) Then Cd2 = intW '-ZW
            If (Math.Abs(Rd1 - Rd2) > minZoomSize And Math.Abs(Cd1 - Cd2) > minZoomSize) Then
                '--表示領域の再設定

                Op.SetPart(win, Rd1, Cd1, Rd2, Cd2)
            End If

            ' Op.DispObj(hImage, win)
            ResizePart = True
        Catch ex As Exception
            ResizePart = False
        End Try
    End Function
    Private Sub TableLayoutPanel7_Paint(sender As Object, e As PaintEventArgs)

    End Sub

    Private Sub TableLayoutPanel9_Paint(sender As Object, e As PaintEventArgs)

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        'Timer1.Stop()
        '20151120 Kiryu コメントアウト
        'TimersStop() '測定値登録画面ライブ映像停止
        Dim objFrmReg As New FrmReg
        objFrmReg.Tag = strNewKoujiFolder
#If False Then
        objFrmReg.Tag = "C:\01_VFORM_Projects\SUEOKI\Data130826165926560955"
        objFrmReg.Tag = "C:\01_VFORM_Projects\SUEOKI\Data130828018318923997"
#End If
        objFrmReg.ShowDialog()
        ' Timer1.Start()
        TimersStart()

    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs)
        Dim objFrmDetail As New FrmDetail
        objFrmDetail.Show()

    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs)
        Dim objFrmOption As New FrmOption
        objFrmOption.Show()
    End Sub

    Private Sub FrmMain_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        ' Timer1.Start()

    End Sub

    Private Sub FrmMain_Load(sender As Object, e As EventArgs) Handles Me.Load

#If blnShinMeiwa = True Then
        'Me.Text = "車両寸法測定システム" & " Ver" & Application.ProductVersion '& " (Build No:" & My.Application.Info.Version.Build & ")"
        '(20151109 Kiryu Add) ビルド番号をビルド時の日付に変更( MyProject 設定より)
        Me.Text = "車両寸法測定装置 " & "Build" & My.Settings.BuildNo
        'Add End

#Else
        Me.Text = "据置型ＶＦＯＲＭ" & " Ver" & Application.ProductVersion
#End If

        objLiveParam = New Common.LiveParam(My.Application.Info.DirectoryPath & "\live_param.csv")

        blnNewKouji = True
        hwinid1 = AxHWindowXCtrl1.HalconWindow.HalconID
        hwinid2 = AxHWindowXCtrl2.HalconWindow.HalconID
        intH = 2748
        intW = 3840

        '(20150731 Tezuka ADD) シリアルポートオープン、スキャンタイプ設定
        SerialPort_Open()

        'センサーモードをデフォルトストレートにする
        Dim Scan As Integer = 1
        SerialPort_ScanTypeChange(Scan)

        DoveModeSet_Read()
        If DoveModeFlg >= 2 Then
            GroupBox3.Visible = True
            IncidentGetAndDisp(0)
        Else
            GroupBox3.Visible = False
        End If

        '(20150915 Tezuka ADD) アクティブ計測値用タイマー起動
        '!!!!! テスト用コード（本番は消すこと!!!)
        'DoveModeSet_Read()
        If DoveModeFlg = 1 Then
            Timer_ActiveData.Interval = SenserSet.ActiveInterval * 1000
            ActiveDataFlg = 0
            Timer_ActiveData.Start()
        End If
        '!!!!! テスト用コード（本番は消すこと!!!)

        If objLiveParam.ImageHeight >= 1 And objLiveParam.ImageWidth >= 1 Then

#If True Then          '20170713 SUURI TEST 用
            Op.SetPart(AxHWindowXCtrl1.HalconWindow.HalconID, 0, 0, CInt(intH / objLiveParam.ImageHeight), CInt(intW / objLiveParam.ImageWidth))
            Op.SetPart(AxHWindowXCtrl2.HalconWindow.HalconID, 0, 0, CInt(intH / objLiveParam.ImageHeight), CInt(intW / objLiveParam.ImageWidth))

            Op.SetPart(AxHWindowXCtrl3.HalconWindow.HalconID, 0, 0, CInt(intH / objLiveParam.ImageHeight), CInt(intW / objLiveParam.ImageWidth))
            Op.SetPart(AxHWindowXCtrl4.HalconWindow.HalconID, 0, 0, CInt(intH / objLiveParam.ImageHeight), CInt(intW / objLiveParam.ImageWidth))
            Op.SetPart(AxHWindowXCtrl5.HalconWindow.HalconID, 0, 0, CInt(intH / objLiveParam.ImageHeight), CInt(intW / objLiveParam.ImageWidth))
            Op.SetPart(AxHWindowXCtrl6.HalconWindow.HalconID, 0, 0, CInt(intH / objLiveParam.ImageHeight), CInt(intW / objLiveParam.ImageWidth))

#Else
            Op.SetPart(AxHWindowXCtrl1.HalconWindow.HalconID, CInt(intH / objLiveParam.ImageHeight) * (objLiveParam.ImageHeight - 1), CInt(intW / objLiveParam.ImageWidth) * (objLiveParam.ImageWidth - 1), intH, intW)
            Op.SetPart(AxHWindowXCtrl2.HalconWindow.HalconID, CInt(intH / objLiveParam.ImageHeight) * (objLiveParam.ImageHeight - 1), CInt(intW / objLiveParam.ImageWidth) * (objLiveParam.ImageWidth - 1), intH, intW)

            Op.SetPart(AxHWindowXCtrl3.HalconWindow.HalconID, CInt(intH / objLiveParam.ImageHeight) * (objLiveParam.ImageHeight - 1), CInt(intW / objLiveParam.ImageWidth) * (objLiveParam.ImageWidth - 1), intH, intW)
            Op.SetPart(AxHWindowXCtrl4.HalconWindow.HalconID, CInt(intH / objLiveParam.ImageHeight) * (objLiveParam.ImageHeight - 1), CInt(intW / objLiveParam.ImageWidth) * (objLiveParam.ImageWidth - 1), intH, intW)
            Op.SetPart(AxHWindowXCtrl5.HalconWindow.HalconID, CInt(intH / objLiveParam.ImageHeight) * (objLiveParam.ImageHeight - 1), CInt(intW / objLiveParam.ImageWidth) * (objLiveParam.ImageWidth - 1), intH, intW)
            Op.SetPart(AxHWindowXCtrl6.HalconWindow.HalconID, CInt(intH / objLiveParam.ImageHeight) * (objLiveParam.ImageHeight - 1), CInt(intW / objLiveParam.ImageWidth) * (objLiveParam.ImageWidth - 1), intH, intW)

#End If
         
            Op.SetPart(AxHWindowXCtrl7.HalconWindow.HalconID, 0, 0, CInt(intH / objLiveParam.ImageHeight), CInt(intW / objLiveParam.ImageWidth))
            Op.SetPart(AxHWindowXCtrl8.HalconWindow.HalconID, 0, 0, CInt(intH / objLiveParam.ImageHeight), CInt(intW / objLiveParam.ImageWidth))
            Op.SetPart(AxHWindowXCtrl9.HalconWindow.HalconID, 0, 0, CInt(intH / objLiveParam.ImageHeight), CInt(intW / objLiveParam.ImageWidth))
            Op.SetPart(AxHWindowXCtrl10.HalconWindow.HalconID, 0, 0, CInt(intH / objLiveParam.ImageHeight), CInt(intW / objLiveParam.ImageWidth))
            Op.SetPart(AxHWindowXCtrl11.HalconWindow.HalconID, 0, 0, CInt(intH / objLiveParam.ImageHeight), CInt(intW / objLiveParam.ImageWidth))
            Op.SetPart(AxHWindowXCtrl12.HalconWindow.HalconID, 0, 0, CInt(intH / objLiveParam.ImageHeight), CInt(intW / objLiveParam.ImageWidth))

            'Op.SetPart(AxHWindowXCtrl1.HalconWindow.HalconID, 0, 0, objLiveParam.ImageHeight, objLiveParam.ImageWidth)
            'Op.SetPart(AxHWindowXCtrl2.HalconWindow.HalconID, 0, 0, objLiveParam.ImageHeight, objLiveParam.ImageWidth)

            'Op.SetPart(AxHWindowXCtrl3.HalconWindow.HalconID, 0, 0, objLiveParam.ImageHeight, objLiveParam.ImageWidth)
            'Op.SetPart(AxHWindowXCtrl4.HalconWindow.HalconID, 0, 0, objLiveParam.ImageHeight, objLiveParam.ImageWidth)
            'Op.SetPart(AxHWindowXCtrl5.HalconWindow.HalconID, 0, 0, objLiveParam.ImageHeight, objLiveParam.ImageWidth)
            'Op.SetPart(AxHWindowXCtrl6.HalconWindow.HalconID, 0, 0, objLiveParam.ImageHeight, objLiveParam.ImageWidth)
            'Op.SetPart(AxHWindowXCtrl7.HalconWindow.HalconID, 0, 0, objLiveParam.ImageHeight, objLiveParam.ImageWidth)
            'Op.SetPart(AxHWindowXCtrl8.HalconWindow.HalconID, 0, 0, objLiveParam.ImageHeight, objLiveParam.ImageWidth)
            'Op.SetPart(AxHWindowXCtrl9.HalconWindow.HalconID, 0, 0, objLiveParam.ImageHeight, objLiveParam.ImageWidth)
            'Op.SetPart(AxHWindowXCtrl10.HalconWindow.HalconID, 0, 0, objLiveParam.ImageHeight, objLiveParam.ImageWidth)
            'Op.SetPart(AxHWindowXCtrl11.HalconWindow.HalconID, 0, 0, objLiveParam.ImageHeight, objLiveParam.ImageWidth)
            'Op.SetPart(AxHWindowXCtrl12.HalconWindow.HalconID, 0, 0, objLiveParam.ImageHeight, objLiveParam.ImageWidth)

            CheckBox1.Checked = True
            CheckBox2.Checked = True
            CheckBox3.Checked = False
            CheckBox4.Checked = False
            CheckBox5.Checked = False
            CheckBox6.Checked = False
            CheckBox7.Checked = True
            CheckBox8.Checked = True
            CheckBox9.Checked = True
            CheckBox10.Checked = True
            CheckBox11.Checked = True
            CheckBox12.Checked = True

            Op.SetPart(AxHWinPrev.HalconWindow.HalconID, 0, 0, intH, intW)
            hvWinPrev = AxHWinPrev.HalconWindow.HalconID

            tmptarget = New TargetDetect
            hv_ALLCTID = Tuple.ReadTuple(My.Application.Info.DirectoryPath & "\RectangleCT_ID.tup")

            ConnectDbFBM(My.Application.Info.DirectoryPath & "\")
            ReadCamParamList()
#If blnDotNet Then
            Try
                Dim hv_ParameterInfo As HALCONXLib.HTupleX = New HALCONXLib.HTupleX, hv_ParameterInfoValues As HALCONXLib.HTupleX = New HALCONXLib.HTupleX
                Dim cam As Object = "uEye"
                Dim par As Object = "parameters"
                Op.InfoFramegrabber(cam, par, hv_ParameterInfo, hv_ParameterInfoValues)
            Catch ex As Exception
                '    MsgBox(ex.Message)
            End Try

            GetFrameGrabberHandleWithDotNet()
#Else
            GetFrameGrabberHandle()
#End If

            GetALLShootParam()
            cbShootParam.Items.Clear()
            Dim i As Integer = 0
            Dim selectedIndex As Integer = -1
            For Each objShootParam As ShootParam In lstShootParam
                '20160303 ADD KIRYU 起動時の撮影パラメータを標準がデフォルとになるよう変更
                'If objShootParam.settei = True Then
                If objShootParam.setteiname = "標準" Then
                    selectedIndex = i
                End If
                cbShootParam.Items.Add(objShootParam.setteiname)
                i += 1
            Next
            cbShootParam.SelectedIndex = selectedIndex
#If DebugTest = True And False Then
            'Dim testLiveImagePath As String = "C:\tmp\20150702-新明和センサ用画像＋4TGテスト\カメラ位置からのセンサ用画\cam1-3\LiveImages"
            Dim testLiveImagePath As String = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20150925\15時34分頃のリフター無認識-設定230-30\15時34分頃のリフター無認識-設定230-30"
            ' testLiveImagePath = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20150919\20150919-LIVE18時\20150919-LIVE18時\Cam1_del"
            'testLiveImagePath = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20151002120209\s2\s2\cam1"
            testLiveImagePath = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20151002120209\黒頭フレーム\Cam1_del"
            'testLiveImagePath = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20151002120209\Live200白頭タンク黒小型１\Live200白頭タンク黒小型１\Cam1"
            testLiveImagePath = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20151002120209\白ダンプ通り抜け\白ダンプ通り抜け\Cam1"
            ' testLiveImagePath = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20151002120209\緑大型産廃ダンプ１\緑大型産廃ダンプ１\Cam1"
            'testLiveImagePath = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20151006\車6-8-9-10-11\Cam1"
            'testLiveImagePath = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20151006\case13_白ダンプ　①\case13_白ダンプ　①\Cam1"
            'testLiveImagePath = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20151006\case14＿白フレーム認識○\case14＿白フレーム認識○\Cam1"
            'testLiveImagePath = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20151006\case28_白フレーム_入り曇り出晴れ１\case28_白フレーム_入り曇り出晴れ１\Cam1"
            ''testLiveImagePath = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20151009\CASE4と5-緑頭白台ダンプと黒頭フレームいないのあと不安定_1\CASE4&5-緑頭白台ダンプと黒頭フレームいないのあと不安定_1"
            testLiveImagePath = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20151016\1742_デカタンク（白黒テープ幅2倍）\1742_デカタンク（白黒テープ幅2倍）\Cam1_del"
            'testLiveImagePath = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20151016\1723_白黒テープ幅2倍\1723_白黒テープ幅2倍\Cam1"
            testLiveImagePath = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20151020\1404_緑ダンプ(サビ荷台)_車いないにならず\1404_緑ダンプ(サビ荷台)_車いないにならず\Cam1"
            ' testLiveImagePath = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20151020\1411_その他車両(フレームのみ)_車いないにならず\1411_その他車両(フレームのみ)_車いないにならず\Cam1"
            lstCamParam.Item(0).Fr = New HalconDotNet.HFramegrabber("File", 1, 1, 0, 0, 0, 0, "default", -1, "default", -1, "default", testLiveImagePath, "default", -1, -1)
            'Op.OpenFramegrabber("File", -1, -1, -1, -1, -1, -1, "default", -1, "default", -1, "default", _projeh, "default", -1, -1, IGFlag)
            Dim testLiveImagePath2 As String = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20150925\15時34分頃のリフター無認識-設定230-30\15時34分頃のリフター無認識-設定230-30\Cam6"
            '  testLiveImagePath2 = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20150919\20150919-LIVE18時\20150919-LIVE18時\Cam6_del"
            'testLiveImagePath2 = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20151002120209\s2\s2\cam6"
            testLiveImagePath2 = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20151002120209\黒頭フレーム\Cam6_del"
            ' testLiveImagePath2 = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20151002120209\Live200白頭タンク黒小型１\Live200白頭タンク黒小型１\Cam6"
            testLiveImagePath2 = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20151002120209\白ダンプ通り抜け\白ダンプ通り抜け\Cam6"
            'testLiveImagePath2 = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20151002120209\緑大型産廃ダンプ１\緑大型産廃ダンプ１\Cam6"
            ' testLiveImagePath2 = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20151006\車6-8-9-10-11\Cam6"
            ' testLiveImagePath2 = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20151006\case13_白ダンプ　①\case13_白ダンプ　①\Cam6"
            'testLiveImagePath2 = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20151006\case14＿白フレーム認識○\case14＿白フレーム認識○\Cam6"
            'testLiveImagePath2 = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20151006\case28_白フレーム_入り曇り出晴れ１\case28_白フレーム_入り曇り出晴れ１\Cam6"
            testLiveImagePath2 = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20151016\1742_デカタンク（白黒テープ幅2倍）\1742_デカタンク（白黒テープ幅2倍）\Cam6_del"
            'testLiveImagePath2 = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20151016\1723_白黒テープ幅2倍\1723_白黒テープ幅2倍\Cam6"
            testLiveImagePath2 = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20151020\1404_緑ダンプ(サビ荷台)_車いないにならず\1404_緑ダンプ(サビ荷台)_車いないにならず\Cam6"
            ' testLiveImagePath2 = "C:\MyWorks\VFORM\SUEOKI\Jyuryo\20151020\1411_その他車両(フレームのみ)_車いないにならず\1411_その他車両(フレームのみ)_車いないにならず\Cam6"
            lstCamParam.Item(1).Fr = New HalconDotNet.HFramegrabber("File", 1, 1, 0, 0, 0, 0, "default", -1, "default", -1, "default", testLiveImagePath2, "default", -1, -1)

#End If
            Try
                hrCarSearchRegion1 = New HalconDotNet.HRegion
                hrCarSearchRegion2 = New HalconDotNet.HRegion
                Dim tmpImage As New HalconDotNet.HImage
                tmpImage.ReadImage(My.Application.Info.DirectoryPath & "\TempImage\CarSearchRegion10.jpg")
                hrCarSearchRegion1 = tmpImage.Threshold(125.0, 255.0)
                tmpImage.ReadImage(My.Application.Info.DirectoryPath & "\TempImage\CarSearchRegion20.jpg")
                hrCarSearchRegion2 = tmpImage.Threshold(125.0, 255.0)
                'hrCarSearchRegion2.ReadRegion(My.Application.Info.DirectoryPath & "\TempImage\CarSearchRegion2.reg")
                'hrCarSearchRegion2.GenRegionPolygonFilled(New HalconDotNet.HTuple({327, 161, 260, 393}), New HalconDotNet.HTuple({38, 850, 866, 18}))

                'HalconDotNet.HOperatorSet.GenRegionPolygon(hrCarSearchRegion2, New HalconDotNet.HTuple({327, 161, 260, 393}), New HalconDotNet.HTuple({38, 850, 866, 18}))
                'hrCarSearchRegion2 = hrCarSearchRegion2.FillUp
                'HalconDotNet.HOperatorSet.ReadRegion(hrCarSearchRegion, New HalconDotNet.HTuple(My.Application.Info.DirectoryPath & "\TempImage\CarSearchRegion.reg"))
                'Dim htArea As New HalconDotNet.HTuple
                'HalconDotNet.HOperatorSet.AreaCenter(hrCarSearchRegion, htArea, New HalconDotNet.HTuple, New HalconDotNet.HTuple)
                Debug.Print(hrCarSearchRegion1.Area.ToString)
                Debug.Print(hrCarSearchRegion2.Area.ToString)
            Catch ex As Exception
                Debug.Print(ex.Message)
            End Try

            blnCamLive = False
            TimersStart()

            TimerForNoCar.Interval = CInt(1000 * CDbl(SenserSet.ActiveInterval))
            TimerForNoCar.Enabled = True

            '(20150915 Tezuka ADD) アクティブ計測値用タイマー起動
            DoveModeSet_Read()
            If DoveModeFlg = 1 Then
                Timer_ActiveData.Interval = SenserSet.ActiveInterval * 1000
                ActiveDataFlg = 0
                Timer_ActiveData.Start()
            End If
        Else
            MsgBox("画像サイズ倍率を正しく設定してください。", MsgBoxStyle.OkOnly, "確認")

        End If

        '( 20150831 Kiryu Add )
        KeepAspectReSize()
        Me.dgMeasureResult.DefaultCellStyle.Font = New Font("Microsoft Sans Serif", 12, FontStyle.Bold) '測定結果グリッドのフォント設定
        Me.dgAccuracyConf.DefaultCellStyle.Font = New Font("Microsoft Sans Serif", 12, FontStyle.Bold) '精度確認グリッドのフォント設定

        '起動時から測定箇所名を表示
        'Me.dgMeasureResult.Rows.Add("車長")
        'Me.dgMeasureResult.Rows.Add("車幅")
        'Me.dgMeasureResult.Rows.Add("リアオーバーハング")
        'Me.dgMeasureResult.Rows.Add("バンパー長")
        'Me.dgMeasureResult.Rows.Add("バンパー外側長(左)")
        'Me.dgMeasureResult.Rows.Add("バンパー外側長(右)")
        'Me.dgMeasureResult.Rows.Add("リアバンパー後端オフセット(左)")
        'Me.dgMeasureResult.Rows.Add("リアバンパー後端オフセット(右)")
        'Me.dgMeasureResult.Rows.Add("リアバンパー高(左)")
        'Me.dgMeasureResult.Rows.Add("リアバンパー高(右)")
        'Me.dgMeasureResult.Rows.Add("車高")


    End Sub
    Private Sub TimersStop()

        Timer1.Enabled = False

        Timer2.Enabled = False

        Timer3.Enabled = False

        Timer4.Enabled = False

        Timer5.Enabled = False

        Timer6.Enabled = False

        Timer7.Enabled = False

        Timer8.Enabled = False

        Timer9.Enabled = False

        Timer10.Enabled = False

        Timer11.Enabled = False

        Timer12.Enabled = False


    End Sub
    Private Sub TimersStart()

        Timer1.Interval = iTimerInterval
        Timer1.Enabled = True
        Timer2.Interval = iTimerInterval
        Timer2.Enabled = True
        Timer3.Interval = iTimerInterval
        Timer3.Enabled = True
        Timer4.Interval = iTimerInterval
        Timer4.Enabled = True
        Timer5.Interval = iTimerInterval
        Timer5.Enabled = True
        Timer6.Interval = iTimerInterval
        Timer6.Enabled = True
        Timer7.Interval = iTimerInterval
        Timer7.Enabled = True
        Timer8.Interval = iTimerInterval
        Timer8.Enabled = True
        Timer9.Interval = iTimerInterval
        Timer9.Enabled = True
        Timer10.Interval = iTimerInterval
        Timer10.Enabled = True
        Timer11.Interval = iTimerInterval
        Timer11.Enabled = True
        Timer12.Interval = iTimerInterval
        Timer12.Enabled = True
    End Sub
    Private Sub LiveEizou(ByRef hvWinHand As Object, ByRef objLiveCam As CameraParam)
        Dim ht_Image As HALCONXLib.HUntypedObjectX = Nothing
        Do While blnCamLive

            Op.GenEmptyObj(ht_Image)
            Op.GrabImage(ht_Image, objLiveCam.objAcqHandle)
            Op.DispObj(ht_Image, hvWinHand)
            Marshal.ReleaseComObject(ht_Image)
            System.Windows.Forms.Application.DoEvents()

        Loop

    End Sub
    Private Sub ShootImageOnlyWithDotNet(ByVal objCam As CameraParam)
        Dim i As Integer

        objCam.lstImagesDotNet = New List(Of HalconDotNet.HImage)
        With objShootParam
            For i = .exposure_min To .exposure_max Step .exposure_kankaku
                Try
                    Dim hoImage As HalconDotNet.HImage
                    objCam.Fr.SetFramegrabberParam("gain_master", .gain_master)
                    objCam.Fr.SetFramegrabberParam("exposure", i)
                    hoImage = objCam.Fr.GrabImage
                    objCam.lstImagesDotNet.Add(hoImage)
                    objCam.blnCamShootOK = True
                Catch ex As Exception
                    objCam.blnCamShootOK = False
                    'OutMessage("カメラ番号：" & objCam.Cid & "　での撮影が失敗しました。")
                End Try
                'hoImage.WriteImage("jpeg", 0, My.Application.Info.DirectoryPath & "\TempImage\Image_" & objCam.Cid & "_" & i & ".jpg")

                'Dim hxImage As New HALCONXLib.HImageX
                'For r As Integer = 0 To intH - 1
                '    For c As Integer = 0 To intW - 1
                '        hxImage.SetGrayval(r, c, hoImage.GetGrayval(r, c))
                '    Next
                'Next
                'objCam.lstImages.Add(hxImage)

            Next
        End With
    End Sub
    Private Sub ShootImageOnly2(ByVal objCam As CameraParam)

        Dim i As Integer
        Dim Op As New HALCONXLib.HOperatorSetX
        objCam.lstImages = New List(Of HALCONXLib.HUntypedObjectX)
        With objShootParam
            For i = .exposure_min To .exposure_max Step .exposure_kankaku
                Dim ho_Image As HALCONXLib.HUntypedObjectX = Nothing
                '   Op.GenEmptyObj(ho_Image)
                Op.SetFramegrabberParam(objCam.objAcqHandle, "gain_master", .gain_master)
                Op.SetFramegrabberParam(objCam.objAcqHandle, "exposure", i)
                Op.GrabImage(ho_Image, objCam.objAcqHandle)
                objCam.lstImages.Add(ho_Image)

            Next

        End With

    End Sub
    Private Sub ShootImageOnly(ByVal objCam As CameraParam)
        Dim Opp As New HALCONXLib.HOperatorSetX
        Dim i As Integer
        Dim j As Integer
        ' Dim k As Integer
        Dim hvImageResult As HALCONXLib.HUntypedObjectX = Nothing
        Dim ResultTarget As TargetDetect = Nothing
        Dim lstAllTarget As New List(Of TargetDetect) 'SUURI ADD
        GC.Collect()
        GC.WaitForPendingFinalizers()


        Dim hv_AcqHandle As Object = Nothing
        Dim maxCTcount As Integer = 0
        Dim blnOK As Boolean = False
        Dim hvWinHand As Object
        Dim ImageIndex As Integer
        Dim ho_Image As HALCONXLib.HUntypedObjectX = Nothing
        hvWinHand = objCam.hwindhand
        ImageIndex = objCam.Cid
        hv_AcqHandle = objCam.objAcqHandle
        '   Dim Opp As New HALCONXLib.HOperatorSetX
        Dim indexResultImage As Integer
        '撮影モードに戻す
        'Opp.SetFramegrabberParam(hv_AcqHandle, "vertical_resolution", 1)
        'Opp.SetFramegrabberParam(hv_AcqHandle, "horizontal_resolution", 1)
        objCam.lstImages = New List(Of HALCONXLib.HUntypedObjectX)
        With objShootParam
            maxCTcount = 0

            'Rep BY Yamada 20150316 Sta ---------------------
            'indexResultImage = ImageIndex + objCam.Cid * 100
            indexResultImage = ImageIndex
            'Rep BY Yamada 20150316 Sta ---------------------

            Opp.GenEmptyObj(hvImageResult)
            'SUURI ADD START 20150323
            Dim bestExposure As Double = -999
            Dim bestTargetThreshold As Double = -999
            'SUURI ADD END 20150323
            '露光時間をパラメータ
            For i = .exposure_min To .exposure_max Step .exposure_kankaku

                '  tmptarget = New TargetDetect

                Opp.GenEmptyObj(ho_Image)

                Opp.SetFramegrabberParam(hv_AcqHandle, "gain_master", .gain_master)
                Opp.SetFramegrabberParam(hv_AcqHandle, "exposure", i)
                Opp.GrabImageAsync(ho_Image, hv_AcqHandle, -1)
                '  Opp.GrabImage(ho_Image, hv_AcqHandle)
                ' Op.DispObj(ho_Image, hvWinHand)
                objCam.lstImages.Add(ho_Image)
                'tmptarget.DetectTargetsOther(indexResultImage, ho_Image, hv_ALLCTID, -1, 0)
                'lstAllTarget.Add(tmptarget) 'SUURI ADD 20150402
                '' TextBox1.Text = tmptarget.lstCT.Count
                'If maxCTcount < tmptarget.lstCT.Count Then
                '    maxCTcount = tmptarget.lstCT.Count
                '    ResultTarget = tmptarget
                '    ' Marshal.ReleaseComObject(hvImageResult)
                '    Op.CopyImage(ho_Image, hvImageResult)
                '    'SUURI ADD START 20150323
                '    bestExposure = i
                '    bestTargetThreshold = -1
                '    'SUURI ADD END 20150323
                '    blnOK = True
                'End If

                ''しきい値をパラメータ
                'For j = .targetthreshold_min To .targetthreshold_max Step .targetthreshold_kankaku
                '    tmptarget = New TargetDetect
                '    tmptarget.DetectTargetsOther(indexResultImage, ho_Image, hv_ALLCTID, j, 0)
                '    lstAllTarget.Add(tmptarget) 'SUURI ADD 20150402
                '    If tmptarget.lstCT.Count > 0 Then
                '        ' TextBox1.Text = tmptarget.lstCT.Count
                '        If maxCTcount < tmptarget.lstCT.Count Then
                '            maxCTcount = tmptarget.lstCT.Count
                '            ResultTarget = tmptarget
                '            '  Marshal.ReleaseComObject(hvImageResult)
                '            Op.CopyImage(ho_Image, hvImageResult)
                '            DispCT(hvWinHand, tmptarget)
                '            'SUURI ADD START 20150323
                '            bestExposure = i
                '            bestTargetThreshold = j
                '            'SUURI ADD END 20150323
                '            blnOK = True
                '        End If
                '    End If
                'Next
                ' Marshal.ReleaseComObject(ho_Image)
            Next

            '' SaveAllParameterTarget(lstAllTarget, ImageIndex) 'SUURI ADD 20150404
            'objCam.lstAllTarget = lstAllTarget

            'Dim objTmpRegion As New HALCONXLib.HUntypedObjectX
            'objTmpRegion.GenEmptyObj()

            'For Each objTRegion As TargetDetect In lstAllTarget
            '    Op.ConcatObj(objTmpRegion, objTRegion.objTargetRegion, objTmpRegion)
            'Next

            'If hvWinHand = hwinid1 Then
            '    AllTargetRegion1 = objTmpRegion
            '    AllTargetList1 = lstAllTarget
            'End If
            'If hvWinHand = hwinid2 Then
            '    AllTargetRegion2 = objTmpRegion
            '    Alltargetlist2 = lstAllTarget
            'End If
            'If blnOK = True Then
            '    Op.DispObj(hvImageResult, hvWinHand)
            '    DispCT(hvWinHand, ResultTarget)
            '    Dim StrJpegFileName As String
            '    'StrJpegFileName = resultImageFileName & "_C" & objCam.Cid.ToString & "_" & Format(ImageIndex, "000") & ".jpg"
            '    'Op.WriteImage(hvImageResult, "jpeg", 0, resultImageFolder & resultImageFileName & Format(indexResultImage, "000") & ".jpg")

            '    'REP BY Yamada 20150316 Sta -----------------
            '    StrJpegFileName = resultImageFileName & "_C" & objCam.Cid.ToString & "_" & Format(indexResultImage, "000") & ".jpg"
            '    StrJpegFileName = resultImageFileName & "_" & Format(indexResultImage, "000") & "_C" & objCam.Cid.ToString & ".jpg"
            '    'REP BY Yamada 20150316 Sta -----------------
            '    ' Op.WriteImage(hvImageResult, "jpeg", 0, resultImageFolder & StrJpegFileName)
            '    Dim channelcount As New Object

            '    Op.CountChannels(hvImageResult, channelcount)
            '    If channelcount = 1 Then
            '        Dim objtmpImage As New HALCONXLib.HUntypedObjectX
            '        Op.Compose3(hvImageResult, hvImageResult, hvImageResult, objtmpImage)
            '        Op.WriteImage(objtmpImage, "jpeg", 0, resultImageFolder & StrJpegFileName)
            '    ElseIf channelcount = 3 Then
            '        Op.WriteImage(hvImageResult, "jpeg", 0, resultImageFolder & StrJpegFileName)
            '    End If

            '    objCam.objResultTarget = ResultTarget

            '    'If Not dbClass Is Nothing Then

            '    '    '20150130 ADD By Yamada Sta ---------------------------- 
            '    '    '重複レコード削除（上書き対応）
            '    '    ResultTarget.DeleteData(indexResultImage, False) 'Targetsテーブル
            '    '    DeleteCameraPoseTable(indexResultImage) 'CameraPoseテーブル
            '    '    '20150130 ADD By Yamada End ----------------------------

            '    '    'レコード登録
            '    '    ResultTarget.SaveData() 'Targetsテーブル
            '    '    SaveCameraPoseTable(objCam, indexResultImage, StrJpegFileName) 'CameraPoseテーブル
            '    '    If hvWinHand = hwinid1 Then
            '    '        ViewTarget1 = ResultTarget
            '    '        ' ViewImage1 = hvImageResult
            '    '        Op.CopyImage(hvImageResult, ViewImage1)
            '    '    End If
            '    '    If hvWinHand = hwinid2 Then
            '    '        viewtarget2 = ResultTarget
            '    '        Op.CopyImage(hvImageResult, ViewImage2)
            '    '    End If
            '    '    Dim strLogText As String = ""
            '    '    'SUURI ADD START 20150323
            '    '    strLogText = CStr(Now) & " ImageIndex=" & ImageIndex & " FILENAME: " & StrJpegFileName & "->ゲイン値：" & objShootParam.gain_master & "  露光時間：" & bestExposure & "  TargetThreshold: " & bestTargetThreshold & vbNewLine
            '    '    My.Computer.FileSystem.WriteAllText(resultImageFolder & "LogOut.txt", strLogText, True)
            '    '    'SUURI ADD END 20150323
            '    'End If

            '    '  MsgBox("完了しました。")
            'Else
            '    indexResultImage -= 1

            '    ' MsgBox("コードターゲット認識しませんでした。")
            'End If
        End With

    End Sub
    Private Sub ShootImageGetTarget(ByVal objCam As CameraParam)

        Dim i As Integer
        Dim j As Integer
        ' Dim k As Integer
        Dim hvImageResult As HALCONXLib.HUntypedObjectX = Nothing
        Dim ResultTarget As TargetDetect = Nothing
        Dim lstAllTarget As New List(Of TargetDetect) 'SUURI ADD

        Dim ho_Image As New HALCONXLib.HUntypedObjectX

        Dim hv_AcqHandle As Object = Nothing
        Dim maxCTcount As Integer = 0
        Dim blnOK As Boolean = False
        Dim hvWinHand As Object
        Dim ImageIndex As Integer

        hvWinHand = objCam.hwindhand
        ImageIndex = objCam.Cid
        hv_AcqHandle = objCam.objAcqHandle
        '   Dim Opp As New HALCONXLib.HOperatorSetX
        Dim indexResultImage As Integer

        With objShootParam
            maxCTcount = 0

            'Rep BY Yamada 20150316 Sta ---------------------
            'indexResultImage = ImageIndex + objCam.Cid * 100
            indexResultImage = ImageIndex
            'Rep BY Yamada 20150316 Sta ---------------------

            Op.GenEmptyObj(hvImageResult)
            'SUURI ADD START 20150323
            Dim bestExposure As Double = -999
            Dim bestTargetThreshold As Double = -999
            'SUURI ADD END 20150323
            '露光時間をパラメータ
            For i = .exposure_min To .exposure_max Step .exposure_kankaku

                tmptarget = New TargetDetect
                Op.GenEmptyObj(ho_Image)

                Op.SetFramegrabberParam(hv_AcqHandle, "gain_master", .gain_master)
                Op.SetFramegrabberParam(hv_AcqHandle, "exposure", i)
                ' Op.GrabImageAsync(ho_Image, hv_AcqHandle, -1)
                Op.GrabImage(ho_Image, hv_AcqHandle)
                Op.DispObj(ho_Image, hvWinHand)
                tmptarget.DetectTargetsOther(indexResultImage, ho_Image, hv_ALLCTID, -1, 0)
                lstAllTarget.Add(tmptarget) 'SUURI ADD 20150402
                ' TextBox1.Text = tmptarget.lstCT.Count
                If maxCTcount < tmptarget.lstCT.Count Then
                    maxCTcount = tmptarget.lstCT.Count
                    ResultTarget = tmptarget
                    ' Marshal.ReleaseComObject(hvImageResult)
                    Op.CopyImage(ho_Image, hvImageResult)
                    'SUURI ADD START 20150323
                    bestExposure = i
                    bestTargetThreshold = -1
                    'SUURI ADD END 20150323
                    blnOK = True
                End If

                'しきい値をパラメータ
                For j = .targetthreshold_min To .targetthreshold_max Step .targetthreshold_kankaku
                    tmptarget = New TargetDetect
                    tmptarget.DetectTargetsOther(indexResultImage, ho_Image, hv_ALLCTID, j, 0)
                    lstAllTarget.Add(tmptarget) 'SUURI ADD 20150402
                    If tmptarget.lstCT.Count > 0 Then
                        ' TextBox1.Text = tmptarget.lstCT.Count
                        If maxCTcount < tmptarget.lstCT.Count Then
                            maxCTcount = tmptarget.lstCT.Count
                            ResultTarget = tmptarget
                            '  Marshal.ReleaseComObject(hvImageResult)
                            Op.CopyImage(ho_Image, hvImageResult)
                            DispCT(hvWinHand, tmptarget)
                            'SUURI ADD START 20150323
                            bestExposure = i
                            bestTargetThreshold = j
                            'SUURI ADD END 20150323
                            blnOK = True
                        End If
                    End If
                Next
                Marshal.ReleaseComObject(ho_Image)
            Next

            ' SaveAllParameterTarget(lstAllTarget, ImageIndex) 'SUURI ADD 20150404
            objCam.lstAllTarget = lstAllTarget

            Dim objTmpRegion As New HALCONXLib.HUntypedObjectX
            objTmpRegion.GenEmptyObj()

            For Each objTRegion As TargetDetect In lstAllTarget
                Op.ConcatObj(objTmpRegion, objTRegion.objTargetRegion, objTmpRegion)
            Next

            If hvWinHand = hwinid1 Then
                AllTargetRegion1 = objTmpRegion
                AllTargetList1 = lstAllTarget
            End If
            If hvWinHand = hwinid2 Then
                AllTargetRegion2 = objTmpRegion
                Alltargetlist2 = lstAllTarget
            End If
            If blnOK = True Then
                Op.DispObj(hvImageResult, hvWinHand)
                DispCT(hvWinHand, ResultTarget)
                Dim StrJpegFileName As String
                'StrJpegFileName = resultImageFileName & "_C" & objCam.Cid.ToString & "_" & Format(ImageIndex, "000") & ".jpg"
                'Op.WriteImage(hvImageResult, "jpeg", 0, resultImageFolder & resultImageFileName & Format(indexResultImage, "000") & ".jpg")

                'REP BY Yamada 20150316 Sta -----------------
                StrJpegFileName = resultImageFileName & "_C" & objCam.Cid.ToString & "_" & Format(indexResultImage, "000") & ".jpg"
                StrJpegFileName = resultImageFileName & "_" & Format(indexResultImage, "000") & "_C" & objCam.Cid.ToString & ".jpg"
                'REP BY Yamada 20150316 Sta -----------------
                ' Op.WriteImage(hvImageResult, "jpeg", 0, resultImageFolder & StrJpegFileName)
                Dim channelcount As New Object

                Op.CountChannels(hvImageResult, channelcount)
                If channelcount = 1 Then
                    Dim objtmpImage As New HALCONXLib.HUntypedObjectX
                    Op.Compose3(hvImageResult, hvImageResult, hvImageResult, objtmpImage)
                    Op.WriteImage(objtmpImage, "jpeg", 0, resultImageFolder & StrJpegFileName)
                ElseIf channelcount = 3 Then
                    Op.WriteImage(hvImageResult, "jpeg", 0, resultImageFolder & StrJpegFileName)
                End If

                objCam.objResultTarget = ResultTarget
                Op.CopyImage(hvImageResult, objCam.hvImageResult)

                'If Not dbClass Is Nothing Then

                '    '20150130 ADD By Yamada Sta ---------------------------- 
                '    '重複レコード削除（上書き対応）
                '    ResultTarget.DeleteData(indexResultImage, False) 'Targetsテーブル
                '    DeleteCameraPoseTable(indexResultImage) 'CameraPoseテーブル
                '    '20150130 ADD By Yamada End ----------------------------

                '    'レコード登録
                '    ResultTarget.SaveData() 'Targetsテーブル
                '    SaveCameraPoseTable(objCam, indexResultImage, StrJpegFileName) 'CameraPoseテーブル
                '    If hvWinHand = hwinid1 Then
                '        ViewTarget1 = ResultTarget
                '        ' ViewImage1 = hvImageResult
                '        Op.CopyImage(hvImageResult, ViewImage1)
                '    End If
                '    If hvWinHand = hwinid2 Then
                '        viewtarget2 = ResultTarget
                '        Op.CopyImage(hvImageResult, ViewImage2)
                '    End If
                '    Dim strLogText As String = ""
                '    'SUURI ADD START 20150323
                '    strLogText = CStr(Now) & " ImageIndex=" & ImageIndex & " FILENAME: " & StrJpegFileName & "->ゲイン値：" & objShootParam.gain_master & "  露光時間：" & bestExposure & "  TargetThreshold: " & bestTargetThreshold & vbNewLine
                '    My.Computer.FileSystem.WriteAllText(resultImageFolder & "LogOut.txt", strLogText, True)
                '    'SUURI ADD END 20150323
                'End If

                '  MsgBox("完了しました。")
            Else
                indexResultImage -= 1

                ' MsgBox("コードターゲット認識しませんでした。")
            End If
        End With

    End Sub
    Private Sub ShootImage(ByRef objCam As CameraParam)
        Dim Op As New HALCONXLib.HOperatorSetX
        Dim Tuple As New HALCONXLib.HTupleX
        Try



            Dim i As Integer
            Dim j As Integer
            ' Dim k As Integer
            Dim hvImageResult As HALCONXLib.HUntypedObjectX = Nothing
            Dim ResultTarget As TargetDetect = Nothing
            Dim lstAllTarget As New List(Of TargetDetect) 'SUURI ADD
            Dim ho_Image As New HALCONXLib.HUntypedObjectX


            Dim hv_AcqHandle As Object = Nothing
            Dim maxCTcount As Integer = 0
            Dim blnOK As Boolean = False
            Dim hvWinHand As Object
            Dim ImageIndex As Integer

            hvWinHand = objCam.hwindhand
            ImageIndex = objCam.Cid
            hv_AcqHandle = objCam.objAcqHandle
            '   Dim Opp As New HALCONXLib.HOperatorSetX
            Dim indexResultImage As Integer
            objCam.lstImages = New List(Of HALCONXLib.HUntypedObjectX)
          

            With objShootParam
                maxCTcount = 0

                'Rep BY Yamada 20150316 Sta ---------------------
                'indexResultImage = ImageIndex + objCam.Cid * 100
                indexResultImage = ImageIndex
                'Rep BY Yamada 20150316 Sta ---------------------
                Dim StrJpegFileName As String
                StrJpegFileName = resultImageFileName & "_" & Format(indexResultImage, "000") & "_C" & objCam.Cid.ToString & ".jpg"

                Op.GenEmptyObj(hvImageResult)
                'SUURI ADD START 20150323
                Dim bestExposure As Double = -999
                Dim bestTargetThreshold As Double = -999
                'SUURI ADD END 20150323
                '露光時間をパラメータ
                Dim iRWimageCount As Integer = 0
                For i = .exposure_min To .exposure_max Step .exposure_kankaku

                    tmptarget = New TargetDetect
                    Op.GenEmptyObj(ho_Image)

                    'Op.SetFramegrabberParam(hv_AcqHandle, "gain_master", .gain_master)
                    'Op.SetFramegrabberParam(hv_AcqHandle, "exposure", i)
                    '' Op.GrabImageAsync(ho_Image, hv_AcqHandle, -1)
                    'Op.GrabImage(ho_Image, hv_AcqHandle)
#If DebugTest = True Then
                
                    Op.ReadImage(ho_Image, strNewKoujiFolder & "\" & StrJpegFileName)

#Else
                    Debug.Print(objCam.lstImagesDotNet.Item(iRWimageCount).CountChannels.I)

                    objCam.lstImagesDotNet.Item(iRWimageCount).WriteImage("jpeg", 0, My.Application.Info.DirectoryPath & "\TempImage\Image_" & objCam.Cid & "_" & i & ".jpg")
                    iRWimageCount += 1
                    Op.ReadImage(ho_Image, My.Application.Info.DirectoryPath & "\TempImage\Image_" & objCam.Cid & "_" & i & ".jpg") '\TempImage\Image_" & objCam.Cid & "_" & i & ".jpg"

#End If
                     '  Op.DispObj(ho_Image, hvWinHand)
                    tmptarget.DetectTargetsOther(indexResultImage, ho_Image, hv_ALLCTID, -1, 0)
                    lstAllTarget.Add(tmptarget) 'SUURI ADD 20150402
                    ' TextBox1.Text = tmptarget.lstCT.Count
                    If maxCTcount < tmptarget.lstCT.Count Then
                        maxCTcount = tmptarget.lstCT.Count
                        ResultTarget = tmptarget
                        Marshal.ReleaseComObject(hvImageResult)
                        Op.CopyImage(ho_Image, hvImageResult)
                        'SUURI ADD START 20150323
                        bestExposure = i
                        bestTargetThreshold = -1
                        'SUURI ADD END 20150323
                        blnOK = True
                    End If
                    'SUSANO ADD START 20150919
                    If i = .exposure_min Then
                        ResultTarget = tmptarget
                        Marshal.ReleaseComObject(hvImageResult)
                        Op.CopyImage(ho_Image, hvImageResult)
                    End If
                    'SUSANO ADD END 20150919
                    'しきい値をパラメータ
                    For j = .targetthreshold_min To .targetthreshold_max Step .targetthreshold_kankaku
                        tmptarget = New TargetDetect
                        tmptarget.DetectTargetsOther(indexResultImage, ho_Image, hv_ALLCTID, j, 0)
                        lstAllTarget.Add(tmptarget) 'SUURI ADD 20150402
                        If tmptarget.lstCT.Count > 0 Then
                            ' TextBox1.Text = tmptarget.lstCT.Count
                            If maxCTcount < tmptarget.lstCT.Count Then
                                maxCTcount = tmptarget.lstCT.Count
                                ResultTarget = tmptarget
                                Marshal.ReleaseComObject(hvImageResult)
                                Op.CopyImage(ho_Image, hvImageResult)
                                '  DispCT(hvWinHand, tmptarget)
                                'SUURI ADD START 20150323
                                bestExposure = i
                                bestTargetThreshold = j
                                'SUURI ADD END 20150323
                                blnOK = True
                            End If
                        End If
                    Next
                    Marshal.ReleaseComObject(ho_Image)
                Next

                ' SaveAllParameterTarget(lstAllTarget, ImageIndex) 'SUURI ADD 20150404
                ' objCam.lstAllTarget = lstAllTarget

                'Dim objTmpRegion As New HALCONXLib.HUntypedObjectX
                'objTmpRegion.GenEmptyObj()

                'For Each objTRegion As TargetDetect In lstAllTarget
                '    Op.ConcatObj(objTmpRegion, objTRegion.objTargetRegion, objTmpRegion)
                'Next

                'If hvWinHand = hwinid1 Then
                '    AllTargetRegion1 = objTmpRegion
                '    AllTargetList1 = lstAllTarget
                'End If
                'If hvWinHand = hwinid2 Then
                '    AllTargetRegion2 = objTmpRegion
                '    Alltargetlist2 = lstAllTarget
                'End If
             
                If blnOK = True Then
                    'CT認識補完機能
                    Dim tmpListCT As New List(Of CodedTarget)
                    '20160225 SUSANO ADD START
                    Dim tmptmpListCT As New List(Of CodedTarget)
                    For Each objTRegion As TargetDetect In lstAllTarget
                        For Each objCT As CodedTarget In objTRegion.lstCT
                            If tmptmpListCT.Count = 0 Then
                                tmptmpListCT.Add(objCT)
                                Continue For
                            End If
                            Dim aa As Boolean = False
                            Dim ii As Integer = 0
                            For Each CT As CodedTarget In tmptmpListCT
                                If CT.CT_ID = objCT.CT_ID Then
                                    If CT.AllST_Area < objCT.AllST_Area Then
                                        tmptmpListCT.RemoveAt(ii)
                                        tmptmpListCT.Add(objCT)
                                        'CT = objCT
                                    End If
                                    aa = True
                                    Exit For
                                End If
                                ii += 1
                            Next
                            If aa = False Then
                                tmptmpListCT.Add(objCT)
                            End If
                        Next
                    Next
                    '20160225 SUSANO ADD END
                    '  For Each objTRegion As TargetDetect In lstAllTarget
                    For Each objCT As CodedTarget In tmptmpListCT
                        Dim blnAri As Boolean = False
                        For Each objResCT As CodedTarget In ResultTarget.lstCT
                            If objCT.CT_ID = objResCT.CT_ID Then
                                blnAri = True
                                Exit For
                            End If
                        Next
                        If blnAri = False Then
                            'SUSANO ADD 20160220 START
                            Dim minD As Double = Double.MaxValue
                            Dim minind As Integer = -1
                            Dim inde As Integer = -1
                            For Each RCT As CodedTarget In ResultTarget.lstCT
                                inde += 1
                                For Each ST As SingleTarget In RCT.lstCTtoST
                                    Dim tD As Double
                                    ST.P2D.CalcDistToInputPoint(objCT.lstCTtoST.Item(0).P2D.Row, objCT.lstCTtoST.Item(0).P2D.Col, tD)
                                    If tD < minD Then
                                        minD = tD
                                        minind = inde
                                    End If
                                Next
                            Next

                            If minD > 10 Then
                                Dim isaru As Boolean = False
                                For Each CT As CodedTarget In tmpListCT
                                    If objCT.CT_ID = CT.CT_ID Then
                                        isaru = True
                                        Exit For
                                    End If
                                Next
                                If isaru = False Then
                                    ResultTarget.lstCT.Add(objCT)
                                End If
                            ElseIf minind > -1 Then
                                tmpListCT.Add(ResultTarget.lstCT.Item(minind))
                                If ResultTarget.lstCT.Item(minind).AllST_Area < objCT.AllST_Area Then
                                    ResultTarget.lstCT.RemoveAt(minind)
                                    ResultTarget.lstCT.Add(objCT)
                                End If

                            End If
                            'SUSANO ADD 20160220 END
                        End If
                    Next
                    ' Next

                    Dim channelcount As New Object
                    Op.CountChannels(hvImageResult, channelcount)
                    If channelcount = 1 Then
                        Dim objtmpImage As New HALCONXLib.HUntypedObjectX
                        Op.GenEmptyObj(objtmpImage)
                        Op.Compose3(hvImageResult, hvImageResult, hvImageResult, objtmpImage)
                        Op.WriteImage(objtmpImage, "jpeg", 0, resultImageFolder & StrJpegFileName)
                        Marshal.ReleaseComObject(objtmpImage)
                    ElseIf channelcount = 3 Then
                        Op.WriteImage(hvImageResult, "jpeg", 0, resultImageFolder & StrJpegFileName)
                    End If

                    objCam.objResultTarget = ResultTarget
                    Op.CopyImage(hvImageResult, objCam.hvImageResult)
                    Marshal.ReleaseComObject(hvImageResult)
                    If Not dbClass Is Nothing Then

                        '20150130 ADD By Yamada Sta ---------------------------- 
                        '重複レコード削除（上書き対応）
                        ResultTarget.DeleteData(indexResultImage, False) 'Targetsテーブル
                        DeleteCameraPoseTable(indexResultImage) 'CameraPoseテーブル
                        '20150130 ADD By Yamada End ----------------------------

                        'レコード登録
                        ResultTarget.SaveData() 'Targetsテーブル
                        SaveCameraPoseTable(objCam, indexResultImage, StrJpegFileName) 'CameraPoseテーブル

                        Dim strLogText As String = ""
                        'SUURI ADD START 20150323
                        strLogText = CStr(Now) & " ImageIndex=" & ImageIndex & " FILENAME: " & StrJpegFileName & "->ゲイン値：" & objShootParam.gain_master & "  露光時間：" & bestExposure & "  TargetThreshold: " & bestTargetThreshold & vbNewLine
                        My.Computer.FileSystem.WriteAllText(resultImageFolder & "LogOut.txt", strLogText, True)
                        'SUURI ADD END 20150323
                    End If
                    '  MsgBox("完了しました。")
                Else
                    indexResultImage -= 1
                    OutMessage("カメラ番号：" & objCam.Cid & " からコードターゲット認識しませんでした。")
                    'SUSANO ADD START 20150919
                    objCam.objResultTarget = ResultTarget
                    Op.CopyImage(hvImageResult, objCam.hvImageResult)
                    Dim channelcount As New Object
                    Op.CountChannels(hvImageResult, channelcount)
                    If channelcount = 1 Then
                        Dim objtmpImage As New HALCONXLib.HUntypedObjectX
                        Op.GenEmptyObj(objtmpImage)
                        Op.Compose3(hvImageResult, hvImageResult, hvImageResult, objtmpImage)
                        Op.WriteImage(objtmpImage, "jpeg", 0, resultImageFolder & StrJpegFileName)
                        Marshal.ReleaseComObject(objtmpImage)
                    ElseIf channelcount = 3 Then
                        Op.WriteImage(hvImageResult, "jpeg", 0, resultImageFolder & StrJpegFileName)
                    End If
                    'SUSANO ADD END 20150919
                    flgKaisekiKahi = False
                    ' MsgBox("コードターゲット認識しませんでした。")
                End If
            End With
        Catch ex As Exception
            OutMessage("エラーメッセージ 「" & ex.Message & "」")
            flgKaisekiKahi = False
        End Try
    End Sub

    '20150130 Rep By Yamada Sta --------------------
    'Private Sub SaveCameraPoseTable(ByVal objCam As CameraParam, ByVal indexresultimage As Integer)
    Public Sub SaveCameraPoseTable(ByVal objCam As CameraParam, ByVal indexresultimage As Integer, ByVal StrPJpegFileName As String)
        Dim strFieldName As String() = {"ID", "imagefilename", "CamParamID"}
        Dim strFieldData(2) As String
        strFieldData(0) = indexresultimage
        'strFieldData(1) = "'" & resultImageFileName & Format(indexresultimage, "000") & ".jpg" & "'"
        strFieldData(1) = "'" & StrPJpegFileName & "'"
        strFieldData(2) = objCam.Cid
        If dbClass.DoInsert(strFieldName, "CameraPose", strFieldData) < 0 Then
            MsgBox("DB登録に失敗しました。", MsgBoxStyle.OkOnly, "エラー")
            Exit Sub
        End If
    End Sub
    'Private Sub DeleteCameraPoseTable(ByVal indexResultImage As Integer)
    Public Sub DeleteCameraPoseTable(ByVal indexResultImage As Integer)
        Dim strWhere As String
        strWhere = "ID=" & indexResultImage
        If dbClass.DoDelete("CameraPose", strWhere) < 0 Then
            'MsgBox("DB更新に失敗しました。", MsgBoxStyle.OkOnly, "エラー")
            Exit Sub
        End If
    End Sub
    '20150130 Rep By Yamada End --------------------

    Private Sub GetFrameGrabberHandle()
        Dim i As Integer = 0

        For Each objCam As CameraParam In lstCamParam
            objCam.objAcqHandle = Nothing
            If objCam.objCheckBox.Checked = True Then

                Try
                    Dim flgIsOkOpenFram As Boolean = True
                    For j As Integer = 0 To i - 1
                        If objCam.CamSerialNo = lstCamParam.Item(j).CamSerialNo Then
                            objCam.objAcqHandle = lstCamParam.Item(j).objAcqHandle
                            flgIsOkOpenFram = False
                            Exit For
                        End If
                    Next
                    i += 1
                    If flgIsOkOpenFram = True Then
                        Dim hv_AcqHandle As Object = Nothing
                        '  Op.OpenFramegrabber("uEye", objLiveParam.ImageWidth, objLiveParam.ImageHeight, 0, 0, 0, 0, "default", -1, "default", -1, "default", "default", objCam.CamSerialNo, -1, -1, hv_AcqHandle)
                        Op.OpenFramegrabber("uEye", 1, 1, 0, 0, 0, 0, "default", -1, "default", -1, "default", "default", objCam.CamSerialNo, -1, -1, hv_AcqHandle)
                        Op.SetFramegrabberParam(hv_AcqHandle, "vertical_resolution", objLiveParam.ImageHeight)
                        Op.SetFramegrabberParam(hv_AcqHandle, "horizontal_resolution", objLiveParam.ImageWidth)
                        If objLiveParam.isAuto = 1 Then
                            Op.SetFramegrabberParam(hv_AcqHandle, "exposure", "auto")
                            Op.SetFramegrabberParam(hv_AcqHandle, "gain_master", "auto")
                            'Op.SetFramegrabberParam(hv_AcqHandle, "gain_master", objLiveParam.gain_master)
                            If objLiveParam.frameRate > 0 Then
                                Op.SetFramegrabberParam(hv_AcqHandle, "frame_rate", objLiveParam.frameRate)
                            End If
                        Else
                            Op.SetFramegrabberParam(hv_AcqHandle, "gain_master", objLiveParam.gain_master)
                            Op.SetFramegrabberParam(hv_AcqHandle, "exposure", objLiveParam.exposure)
                        End If

                        'Op.SetFramegrabberParam(hv_AcqHandle, "gain_master", objLiveParam.gain_master)
                        'Op.SetFramegrabberParam(hv_AcqHandle, "exposure", objLiveParam.exposure)

                        'Op.SetFramegrabberParam(hv_AcqHandle, "image_height", objLiveParam.ImageHeight)
                        'Op.SetFramegrabberParam(hv_AcqHandle, "image_width", objLiveParam.ImageWidth)

                        objCam.objAcqHandle = hv_AcqHandle

                    End If


                Catch ex As Exception
                    Debug.Print(ex.Message)
                    'MsgBox("カメラが接続されていません。カメラNo=" & objCam.CamSerialNo, MsgBoxStyle.Critical)
                    ' MsgBox("カメラが接続されていません。カメラNo=" & objCam.Cid & "(" & objCam.CamSerialNo & ")", MsgBoxStyle.Critical)
                    OutMessage("カメラが接続されていません。カメラNo=" & objCam.Cid & "(" & objCam.CamSerialNo & ")")

                End Try
            End If
        Next

    End Sub
    Private Sub GetFrameGrabberHandleWithDotNet()
        Dim i As Integer = 0

        For Each objCam As CameraParam In lstCamParam
            objCam.Fr = Nothing
            If objCam.objCheckBox.Checked = True Then


                Try
                    Dim Frr As New HalconDotNet.HFramegrabber()
                    'Debug.WriteLine(hv_ParameterInfoValues.ToString())
                    Frr.OpenFramegrabber("uEye", 1, 1, 0, 0, 0, 0, "default", -1, "default", -1, "default", "default", objCam.CamSerialNo, -1, -1)
                    'Frr.SetFramegrabberParam("gain_master", objLiveParam.gain_master)
                    'Frr.SetFramegrabberParam("exposure", objLiveParam.exposure)
                    Frr.SetFramegrabberParam("vertical_resolution", objLiveParam.ImageHeight)
                    Frr.SetFramegrabberParam("horizontal_resolution", objLiveParam.ImageWidth)
                    If objLiveParam.isAuto = 1 Then
                        Frr.SetFramegrabberParam("exposure", "auto")
                        'Frr.SetFramegrabberParam("gain_master", objLiveParam.gain_master) '20160727 Kiryu 変更 
                        Frr.SetFramegrabberParam("gain_master", "auto") '20160727 Kiryu 変更 
                        If objLiveParam.frameRate > 0 Then
                            Frr.SetFramegrabberParam("frame_rate", objLiveParam.frameRate)
                        End If
                    Else
                        Frr.SetFramegrabberParam("gain_master", objLiveParam.gain_master)
                        Frr.SetFramegrabberParam("exposure", objLiveParam.exposure)
                    End If

                    objCam.Fr = Frr


                Catch ex As Exception
                    Debug.Print(ex.Message)
                    'MsgBox("カメラが接続されていません。カメラNo=" & objCam.CamSerialNo, MsgBoxStyle.Critical)
                    ' MsgBox("カメラが接続されていません。カメラNo=" & objCam.Cid & "(" & objCam.CamSerialNo & ")", MsgBoxStyle.Critical)
                    OutMessage("カメラが接続されていません。カメラNo=" & objCam.Cid & "(" & objCam.CamSerialNo & ")")

                End Try
            End If
        Next

    End Sub


    '20150202 Rep By Yamada 
    Public Sub DispCT(ByVal win As Object, ByVal DispTarget As TargetDetect)
        Dim obj As New HALCONXLib.HUntypedObjectX
        'GenEmptyObj：空のオブジェクトtupleを作成
        Op.GenEmptyObj(obj)
        'SetFont：テキスト出力に対して用いるフォントを設定する。

        Op.SetFont(win, "-ＭＳ 明朝-12-")
        'SetColor：出力カラーを設定する。
        Op.SetDraw(win, "margin") 'SUURI ADD 20150412
        Op.SetColored(win, 12)
        Dim TD As TargetDetect = DispTarget
        Try 'SUURI UPDATE 20150410
            ' Op.DispObj(DispTarget.objTargetRegion, win) 'SUURI ADD 20150405
            'SUURI ADD START 20150412
            If win = hwinid1 Then
                Op.DispObj(AllTargetRegion1, win)
            End If
            If win = hwinid2 Then
                Op.DispObj(AllTargetRegion2, win)
            End If
            'SUURI ADD END 20150412
        Catch ex As Exception

        End Try

        For Each CT As CodedTarget In TD.lstCT
            Marshal.ReleaseComObject(obj)
            'GenCrossContourXld:各入力点に対して、十字の XLD オブジェクトを生成する。

            Op.GenCrossContourXld(obj, CT.CenterPoint.Row, CT.CenterPoint.Col, CrossSize, CrossAngle)
            Op.DispObj(obj, win)
            'SetTposition：テキストカーソルの位置を設定する。

            Op.SetTposition(win, CT.CenterPoint.Row, CT.CenterPoint.Col)
            'CT.CurrentLabel = ""
            'For i = 0 To nLookPoints - 1
            '    If gDrawPoints(i).tid = CT.CT_ID And gDrawPoints(i).flgLabel = 1 Then
            '        CT.CurrentLabel = Trim(gDrawPoints(i).LabelName)
            '        Exit For
            '    End If
            'Next
            If CT.CurrentLabel = "" Then
                'WriteString:ウインドウ内にテキストを出力する。

                Op.WriteString(win, CT.systemlabel)
            Else
                Op.WriteString(win, CT.CurrentLabel)
            End If

            'Dim t As Integer
            'For t = 0 To FBMlib.CodedTarget.CTnoSTnum - 1
            '    Op.GenCrossContourXld(obj, CT.CT_Points.Row(t), CT.CT_Points.Col(t), FBMlib.CrossSize, FBMlib.CrossAngle)
            '    Op.DispObj(obj, win)
            '    Op.SetTposition(win, CT.CT_Points.Row(t), CT.CT_Points.Col(t))
            '    Op.WriteString(win, "CT_" & CStr(CT.CT_ID) & "_" & t + 1)
            'Next

        Next
        ''SetColor：出力カラーを設定する。

        Op.SetColor(win, "blue") '画面操作ビューに青色の計測点を表示
        'Dim strName As Ls
        'Op.SetColor(win, entset_line.color.strName) '画面操作ビューに3Dビューの作図属性で定義した属性の計測点を表示

        For Each ST As SingleTarget In TD.lstST

            Marshal.ReleaseComObject(obj) '?
            Op.GenCrossContourXld(obj, ST.P2D.Row, ST.P2D.Col, CrossSize, CrossAngle)
            Op.DispObj(obj, win)
            If ST.P3ID <> -1 Then 'P3ID：SingleTarget.vb

                Op.SetTposition(win, ST.P2D.Row, ST.P2D.Col)
                'ST.currentLabel = ""
                'For i = 0 To nLookPoints - 1
                '    If gDrawPoints(i).tid = 10000 + ST.P3ID Then
                '        ST.currentLabel = gDrawPoints(i).LabelName
                '        Exit For
                '    End If
                'Next
                'If ST.P3ID = 357 Then
                '    If True Then

                '    End If
                'End If
                If ST.currentLabel = "" Then
                    Op.WriteString(win, ST.systemlabel) '：SingleTarget.vb
                Else
                    Op.WriteString(win, ST.currentLabel) '：SingleTarget.vb
                End If

            End If
        Next
        Marshal.ReleaseComObject(obj)

    End Sub

    Public Sub ReadCamParamList()
        Dim IDR As IDataReader
        Dim strSqlText As String = ""

        '   ConnectDbFBM(strReadPath)

        strSqlText = "SELECT CamParamID,CamParamFile,SerialNo FROM CamParamList WHERE FlgType = 1 ORDER BY CamParamID"
        If lstCamParam Is Nothing Then
            lstCamParam = New List(Of CameraParam)
        Else
            lstCamParam.Clear()
        End If

        IDR = dbClass.DoSelect(strSqlText)
        If Not IDR Is Nothing Then
            Do While IDR.Read
                Dim objCamPar As New CameraParam
                objCamPar.Cid = CInt(IDR.GetValue(0))
                objCamPar.CName = IDR.GetValue(1)
                objCamPar.CamSerialNo = IDR.GetValue(2)
                ' objCamPar.Cpath = My.Application.Info.DirectoryPath & "\計測システムフォルダ\CamParam\" & objCamPar.CName
                ' objCamPar.ReadCamParamFromFile()
                Select Case objCamPar.Cid
                    Case 1
                        objCamPar.objCheckBox = CheckBox7
                        objCamPar.hwindhand = AxHWindowXCtrl7.HalconWindow.HalconID
                    Case 2
                        objCamPar.objCheckBox = CheckBox8
                        objCamPar.hwindhand = AxHWindowXCtrl8.HalconWindow.HalconID
                    Case 3
                        objCamPar.objCheckBox = CheckBox9
                        objCamPar.hwindhand = AxHWindowXCtrl9.HalconWindow.HalconID
                    Case 4
                        objCamPar.objCheckBox = CheckBox10
                        objCamPar.hwindhand = AxHWindowXCtrl10.HalconWindow.HalconID
                    Case 5
                        objCamPar.objCheckBox = CheckBox11
                        objCamPar.hwindhand = AxHWindowXCtrl11.HalconWindow.HalconID
                    Case 6
                        objCamPar.objCheckBox = CheckBox12
                        objCamPar.hwindhand = AxHWindowXCtrl12.HalconWindow.HalconID
                    Case 7
                        objCamPar.objCheckBox = CheckBox1
                        objCamPar.hwindhand = AxHWindowXCtrl1.HalconWindow.HalconID
                    Case 8
                        objCamPar.objCheckBox = CheckBox2
                        objCamPar.hwindhand = AxHWindowXCtrl2.HalconWindow.HalconID
                    Case 9
                        objCamPar.objCheckBox = CheckBox3
                        objCamPar.hwindhand = AxHWindowXCtrl3.HalconWindow.HalconID
                    Case 10
                        objCamPar.objCheckBox = CheckBox4
                        objCamPar.hwindhand = AxHWindowXCtrl4.HalconWindow.HalconID
                    Case 11
                        objCamPar.objCheckBox = CheckBox5
                        objCamPar.hwindhand = AxHWindowXCtrl5.HalconWindow.HalconID
                    Case 12
                        objCamPar.objCheckBox = CheckBox6
                        objCamPar.hwindhand = AxHWindowXCtrl6.HalconWindow.HalconID

                End Select
                lstCamParam.Add(objCamPar)
            Loop

            IDR.Close()
        End If
    End Sub

    'SUURI ADD 20150404
    Private Sub SaveAllParameterTarget(ByRef lstAllTarget As List(Of TargetDetect), ByVal ImageInd As Integer)

        If dbClass.DoDelete("AllTargets", "ImageID = " & ImageInd) < 0 Then
            'MsgBox("DB更新に失敗しました。", MsgBoxStyle.OkOnly, "エラー")
            Exit Sub
        End If
        For Each tmptarget As TargetDetect In lstAllTarget
            tmptarget.SaveDataCTonly()
        Next

    End Sub


    Private Sub btnAllShootPara_Click(sender As Object, e As EventArgs)
        Dim lstParallelCamParam As New List(Of CameraParam)

        For Each objCamParam As CameraParam In lstCamParam
            If objCamParam.objCheckBox.Checked = True Then
                lstParallelCamParam.Add(objCamParam)
            End If
        Next
        objShootParam = New Common.ShootParam(My.Application.Info.DirectoryPath & "\shoot_param.csv")

        'Parallel.For(1, lstParallelCamParam.Count + 1, Sub(ii)
        '                                                   ShootImage(Nothing, lstParallelCamParam(ii - 1), ii)
        '                                               End Sub)
        'Parallel.ForEach(Of CameraParam)(lstParallelCamParam, Sub(objcamparam)
        '                                                          Try
        '                                                              ShootImageOnly(objcamparam)
        '                                                          Catch ex As Exception

        '                                                          End Try

        '                                                      End Sub)


        'For ii As Integer = 1 To lstParallelCamParam.Count
        '    '  ShootImage(lstParallelCamParam(ii - 1))
        '    ShootImageOnly(lstParallelCamParam(ii - 1))
        'Next

        For ii As Integer = 1 To lstParallelCamParam.Count
            '  ShootImage(lstParallelCamParam(ii - 1))
            ShootImage(lstParallelCamParam(ii - 1))
        Next

        If Not dbClass Is Nothing Then

            For Each objCamParam As CameraParam In lstParallelCamParam
                If objCamParam.objCheckBox.Checked = True Then
                    If Not objCamParam.objResultTarget Is Nothing Then
                        SaveAllParameterTarget(objCamParam.lstAllTarget, objCamParam.Cid)
                        Dim StrJpegFileName As String
                        StrJpegFileName = resultImageFileName & "_C" & objCamParam.Cid.ToString & "_" & Format(objCamParam.Cid, "000") & ".jpg"
                        StrJpegFileName = resultImageFileName & "_" & Format(objCamParam.Cid, "000") & "_C" & objCamParam.Cid.ToString & ".jpg"
                        objCamParam.objResultTarget.DeleteData(objCamParam.Cid, False) 'Targetsテーブル
                        DeleteCameraPoseTable(objCamParam.Cid) 'CameraPoseテーブル
                        'レコード登録
                        objCamParam.objResultTarget.SaveData()
                        SaveCameraPoseTable(objCamParam, objCamParam.Cid, StrJpegFileName) 'CameraPoseテーブル
                    End If
                End If
            Next
            MsgBox("画像取得完了しました", MsgBoxStyle.OkOnly, "確認")
        Else
            MsgBox("画像取得失敗しました", MsgBoxStyle.OkOnly, "確認")
        End If

    End Sub

    'Private Sub 新規作成ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 新規作成ToolStripMenuItem.Click
    '    strKojiMei = ""
    '    strKojiFolder = ""
    '    '’  LblWorkName.Text = ""
    '    NewKojiDialog.intFlgMode = 0 '新規作成
    '    NewKojiDialog.ShowDialog()
    '    If NewKojiDialog.DialogResult = Windows.Forms.DialogResult.OK Then
    '        strKojiMei = NewKojiDialog.strwKojiMei
    '        strKojiFolder = NewKojiDialog.strwKojiFolder
    '        ' LblWorkName.Text = "工事名 : " & strKojiMei & "      フォルダー名 : " & strKojiFolder
    '        ' LblWorkName.Visible = True
    '        ' GroupBox1.Visible = True
    '        My.Computer.FileSystem.CopyFile(FBM_MDB, strKojiFolder & "\" & FBM_MDB, True)
    '        Dim path As String = strKojiFolder & "\工事名.txt"
    '        If My.Computer.FileSystem.FileExists(path) Then
    '            My.Computer.FileSystem.DeleteFile(path)
    '        End If
    '        Dim fs As New StreamWriter(path, True, System.Text.Encoding.Default)
    '        fs.Write(strKojiMei)
    '        fs.Close()
    '        resultImageFolder = strKojiFolder & "\"
    '        ConnectDbFBM(resultImageFolder)
    '        blnCamLive = False
    '    End If
    'End Sub

    'Private Sub 開くToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 開くToolStripMenuItem.Click
    '    strKojiMei = ""
    '    strKojiFolder = ""
    '    'LblWorkName.Text = ""
    '    NewKojiDialog.intFlgMode = 1 '開く
    '    NewKojiDialog.ShowDialog()
    '    If NewKojiDialog.DialogResult = Windows.Forms.DialogResult.OK Then
    '        strKojiMei = NewKojiDialog.strwKojiMei
    '        strKojiFolder = NewKojiDialog.strwKojiFolder
    '        ' LblWorkName.Text = "工事名 : " & strKojiMei & "      フォルダー名 : " & strKojiFolder
    '        ' LblWorkName.Visible = True
    '        'GroupBox1.Visible = True
    '        If My.Computer.FileSystem.FileExists(strKojiFolder & "\" & FBM_MDB) = False Then
    '            My.Computer.FileSystem.CopyFile(FBM_MDB, strKojiFolder & "\" & FBM_MDB, True)
    '        End If
    '        resultImageFolder = strKojiFolder & "\"
    '        ConnectDbFBM(resultImageFolder)
    '        blnCamLive = False
    '    End If
    'End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick


        '   Timer1.Stop()

        'If blnParaShoot = True Then


        '    ' Dim objfirstCam As CameraParam = Nothing
        '    Parallel.ForEach(Of CameraParam)(lstCamParam, Sub(objcamparam)
        '                                                      Try

        '                                                          If objcamparam.objCheckBox.Checked = True And (Not objcamparam.objAcqHandle Is Nothing) Then
        '                                                              'Op.SetFramegrabberParam(objCamparam.objAcqHandle, "gain_master", objShootParam.gain_master)
        '                                                              'Op.SetFramegrabberParam(objCamparam.objAcqHandle, "exposure", IIf(objShootParam.exposure_max < objShootParam.exposure_min, objShootParam.exposure_min, objShootParam.exposure_max))

        '                                                              'Op.SetFramegrabberParam(objCamparam.objAcqHandle, "gain_master", objLiveParam.gain_master)
        '                                                              'Op.SetFramegrabberParam(objCamparam.objAcqHandle, "exposure", objLiveParam.exposure)

        '                                                              Dim Op As New HALCONXLib.HOperatorSetX
        '                                                              Dim ht_Image As HALCONXLib.HUntypedObjectX = Nothing
        '                                                              '  objfirstCam = objCamparam
        '                                                              Op.GenEmptyObj(ht_Image)

        '                                                              Op.GrabImage(ht_Image, objcamparam.objAcqHandle)
        '                                                              'Dim ttImageWidth As Object = Nothing
        '                                                              'Dim ttImageHeight As Object = Nothing
        '                                                              'Op.GetImagePointer1(ht_Image, Nothing, Nothing, ttImageWidth, ttImageHeight)
        '                                                              'Trace.WriteLine(ttImageHeight & "   " & ttImageWidth)
        '                                                              Op.DispObj(ht_Image, objcamparam.hwindhand)

        '                                                              Marshal.ReleaseComObject(ht_Image)

        '                                                              ' System.Windows.Forms.Application.DoEvents()

        '                                                          End If

        '                                                      Catch ex As Exception
        '                                                          Trace.WriteLine(ex.Message)
        '                                                      End Try
        '                                                  End Sub)
        'Else
#If blnDotNet Then
        'Parallel.ForEach(Of CameraParam)(lstCamParam, Sub(objcamparam)
        '                                                  If objcamparam.objCheckBox.Checked = True And (Not objcamparam.Fr Is Nothing) Then
        '                                                      Dim hhh As New HalconDotNet.HTuple(objcamparam.hwindhand)
        '                                                      HalconDotNet.HOperatorSet.DispObj(objcamparam.Fr.GrabImage, hhh)

        '                                                  End If
        '                                              End Sub)


        'ライブ映像中にLAN接続が切れた場合の処理が不足している。！！！！　SUURI　20150806
#If True Then
        LiveImageOneCamera(0)
#Else
        For Each objCamparam As CameraParam In lstCamParam
            If objCamparam.objCheckBox.Checked = True And (Not objCamparam.Fr Is Nothing) Then
                Try
                    HalconDotNet.HOperatorSet.DispObj(objCamparam.Fr.GrabImage, New HalconDotNet.HTuple(objCamparam.hwindhand))
                    If objCamparam.blnCamConnectOK = False Then
                        OutMessage("カメラ番号:" & objCamparam.Cid & "の接続が戻りました。")
                    End If
                    objCamparam.blnCamConnectOK = True
                Catch ex As Exception
                    If objCamparam.blnCamConnectOK = True Then
                        OutMessage("カメラ番号:" & objCamparam.Cid & "の接続が切れました！！！。確認してください！！！")
                        objCamparam.blnCamConnectOK = False
                    End If
                End Try
            End If
        Next

#End If


#Else
        Try
            Dim ht_Image As HALCONXLib.HUntypedObjectX = Nothing
            '  objfirstCam = objCamparam
            For Each objCamparam As CameraParam In lstCamParam
                If objCamparam.objCheckBox.Checked = True And (Not objCamparam.objAcqHandle Is Nothing) Then
                    'Op.SetFramegrabberParam(objCamparam.objAcqHandle, "gain_master", objShootParam.gain_master)
                    'Op.SetFramegrabberParam(objCamparam.objAcqHandle, "exposure", IIf(objShootParam.exposure_max < objShootParam.exposure_min, objShootParam.exposure_min, objShootParam.exposure_max))

                    'Op.SetFramegrabberParam(objCamparam.objAcqHandle, "gain_master", objLiveParam.gain_master)
                    'Op.SetFramegrabberParam(objCamparam.objAcqHandle, "exposure", objLiveParam.exposure)


                    '  objfirstCam = objCamparam
                    Op.GenEmptyObj(ht_Image)

                    Op.GrabImage(ht_Image, objCamparam.objAcqHandle)
                    Dim ttImageWidth As Object = Nothing
                    Dim ttImageHeight As Object = Nothing
                    Op.GetImagePointer1(ht_Image, Nothing, Nothing, ttImageWidth, ttImageHeight)
                    Trace.WriteLine(ttImageHeight & "   " & ttImageWidth)
                    Op.DispObj(ht_Image, objCamparam.hwindhand)

                    Marshal.ReleaseComObject(ht_Image)

                    ' System.Windows.Forms.Application.DoEvents()

                End If
            Next

            'End If

        Catch ex As Exception

        End Try
#End If

    End Sub
    Private Sub TrackCheckAndSensorReset(ByVal objCamParam As CameraParam, ByVal objDispImage As HalconDotNet.HImage)
        'Dim fileContents As String
        'fileContents = My.Computer.FileSystem.ReadAllText(My.Application.Info.DirectoryPath & "\CarDetectCamera.txt")
        Dim Cid1 As Integer = CInt(ReadFrameGrabSetting(1))
        Dim Cid2 As Integer = CInt(ReadFrameGrabSetting(2))
        Static Dim flgNoCar As Boolean = False
        If objCamParam.Cid = Cid1 Or objCamParam.Cid = Cid2 Then

            Dim flg1 As Boolean
            Dim flg2 As Boolean
            Dim flg As Boolean = False

            If objCamParam.Cid = Cid2 Then
                'flg2 = TrackCheck22(objCamParam, objDispImage)
                flg2 = TrackCheck222(objCamParam, objDispImage, hrCarSearchRegion2)
            End If
            If objCamParam.Cid = Cid1 Then
                ' flg1 = TrackCheck11(objCamParam, objDispImage)
                flg1 = TrackCheck111(objCamParam, objDispImage, hrCarSearchRegion1)
            End If
            For Each objCCC As CameraParam In lstCamParam
                If objCCC.blnCar = True Then
                    flg = True
                    Exit For
                End If
            Next
            If flg Then
                If flgNoCar = False Then
                    OutMessage("車がいる")
                End If
                flgNoCar = True

            Else
                If flgNoCar = True Then
                    'If TimerForNoCar Is Nothing Then
                    '    TimerForNoCar.Interval = CInt(1000 * CDbl(ReadFrameGrabSetting(4)))
                    '    TimerForNoCar.Enabled = True
                    'End If

                    If flgBtnNoCar = True Then
                        ''センサーをリセット
                        'Dim Peak As Double = 0.0
                        'SerialPort_PeakGet(Peak)
                        'OutMessage("エリアセンサーをリセットしました。リセット時のピーク値＝" & Peak)
                        flgBtnNoCar = False

                    Else
                        OutMessage("車がいない")
                        'If Cid2 = 0 Then
                        '    If TimerForNoCar.Enabled = False Then

                        '        TimerForNoCar.Interval = CInt(1000 * CDbl(ReadFrameGrabSetting(5)))
                        '        TimerForNoCar.Enabled = True

                        '    End If
                        'Else
#If False Then
                        'センサーをリセット
                        Dim Peak As Double = 0.0
                        SerialPort_PeakGet(Peak)
                        OutMessage("エリアセンサーをリセットしました。リセット時のピーク値＝" & Peak)
                        '522もリセットする　20151029
                        Dim MVal As Double = 0.0
                        SerialPort_MonitorValueGet(MVal)
#End If
                        If TimerForSensorReset.Enabled = False Then
                            TimerForSensorReset.Interval = CInt(1000 * CDbl(ReadFrameGrabSetting(6)))
                            TimerForSensorReset.Enabled = True
                            TimerForSensorReset.Start()
                            TimerForSensorReset.Tag = Now

                        End If
                        'End If
                    End If
                'If threadNoCar Is Nothing Then
                '    OutMessage("車がいない")
                '    ' Threading.Thread.Sleep(1000 * CDbl(ReadFrameGrabSetting(4)))
                '    threadNoCar = New System.Threading.Thread(New System.Threading.ThreadStart(AddressOf TimerSerialPortReset))
                '    threadNoCar.Start()
                'End If
                'If threadNoCar.ThreadState = Threading.ThreadState.Running Then
                'Else
                '    If threadNoCar.ThreadState = Threading.ThreadState.WaitSleepJoin Then
                '    Else
                '        ' Threading.Thread.Sleep(1000 * CDbl(ReadFrameGrabSetting(4)))
                '        threadNoCar = New System.Threading.Thread(New System.Threading.ThreadStart(AddressOf TimerSerialPortReset))
                '        threadNoCar.Start()
                '        ''センサーをリセット
                '        'Dim Peak As Double = 0.0
                '        'SerialPort_PeakGet(Peak)
                '        'OutMessage("エリアセンサーをリセットしました。リセット時のピーク値＝" & Peak)
                '    End If


                'End If

                End If

                flgNoCar = False
            End If

        End If
    End Sub
    Private Sub TimerSerialPortReset()

        'Threading.Thread.Sleep(1000 * CDbl(ReadFrameGrabSetting(4)))
        'センサーをリセット
        Dim Peak As Double = 0.0
        SerialPort_PeakGet(Peak)
        OutMessage("エリアセンサーをリセットしました。リセット時のピーク値＝" & Peak)
    End Sub


    Private Sub TimerForNoCar_Tick(sender As Object, e As EventArgs) Handles TimerForNoCar.Tick
        'Static Dim flgTimerNoCar As Integer = 0
        'If flgTimerNoCar Mod 2 = 1 Then
        '    TimerSerialPortReset()
        '    TimerForNoCar.Enabled = False
        '    flgTimerNoCar = 0
        'Else
        '    flgTimerNoCar += 1
        'End If

        'ピーク値を常時取得する。
        Dim flgJyojiSensor As Integer = CInt(ReadFrameGrabSetting(8))
        If flgJyojiSensor = 1 Then


            'Dim Peak As Double = 0.0
            'SerialPort_PeakGet(Peak)
            'If Peak > 0 Then
            '    Dim Cid1 As Integer = CInt(ReadFrameGrabSetting(1))
            '    For Each objCam As CameraParam In lstCamParam
            '        If objCam.Cid = Cid1 Then
            '            If objCam.blnCar = True Then
            '                lstPeakValue.Add(Peak)
            '            Else
            '                lstPeakValue.Clear()
            '            End If
            '        End If
            '    Next
            'End If
            'SUSANO ADD START 20151028
            Dim SettingFile As String = My.Application.Info.DirectoryPath & "\Setting\Sensor_Setting.txt"
            Dim S_Sensor As New Sensser_setting(SettingFile)

            Dim MVal As Double = 0.0
            SerialPort_MonitorValueGet(MVal)
            Static Dim JyojiStart As Date
            ' MVal = 1
            If MVal > 0 And flgJyojiStart = False Then
                JyojiStart = Now
                flgJyojiStart = True
            End If
            If flgJyojiStart = True Then
                Dim intNowCompare As Integer = JyojiStart.AddSeconds(S_Sensor.CarExistInterval).CompareTo(Now)
                If intNowCompare = -1 Then
                    Dim Cid1 As Integer = CInt(ReadFrameGrabSetting(1))
                    Dim Cid2 As Integer = CInt(ReadFrameGrabSetting(2))
                    Dim blnFlg As Boolean = False
                    For Each objCam As CameraParam In lstCamParam
                        If objCam.Cid = Cid1 Then
                            For Each blnCarExist As Boolean In objCam.lstCarExistFlg
                                If blnCarExist = True Then
                                    blnFlg = True
                                    Exit For
                                End If
                            Next
                            objCam.lstCarExistFlg.Clear()
                        End If
                        If objCam.Cid = Cid2 Then
                            For Each blnCarExist As Boolean In objCam.lstCarExistFlg
                                If blnCarExist = True Then
                                    blnFlg = True
                                    Exit For
                                End If
                            Next
                            objCam.lstCarExistFlg.Clear()
                        End If
                    Next
                    If blnFlg = False Then
                        'センサーをリセット
                        Dim Peak As Double = 0.0
                        SerialPort_PeakGet(Peak)
                        OutMessage("エリアセンサーを常時監視機能により、リセットしました。リセット時のピーク値＝" & Peak)
                    End If
                    flgJyojiStart = False
                End If
            End If
            '   Application.DoEvents()
            ' SUSANO ADD END 20151028
        End If
    End Sub


    Private Function TrackCheck1(ByVal objCamParam As CameraParam, ByVal objDispImage As HalconDotNet.HImage) As Boolean
        Dim objThreshold1 As Double
        Dim objThreshold2 As Double
        Dim objMinArea As Double
        GetCarDetectThreshold(objThreshold1, objThreshold2, objMinArea)

        Static Dim hxNoCarImage As HalconDotNet.HImage
        Static Dim flgNoCar As Boolean = False
        Dim blnLiveSave As Integer = CInt(ReadFrameGrabSetting(7))

        If blnLiveSave = 1 Then
            Dim folderExists As Boolean
            Dim strLivePath As String = My.Application.Info.DirectoryPath & "\LiveImage"
            folderExists = My.Computer.FileSystem.DirectoryExists(strLivePath)
            If folderExists = False Then
                My.Computer.FileSystem.CreateDirectory(strLivePath)
            End If
            objDispImage.WriteImage("jpeg", 0, strLivePath & "\LiveImage_" & objCamParam.Cid & "_" & Now.ToFileTimeUtc & ".jpg")
            'objDispImage.WriteImage("jpeg", 0, My.Application.Info.DirectoryPath & "\TempImage\TempImage_" & objCamParam.Cid & ".jpg")
        End If

        If hxNoCarImage Is Nothing Or flgBaseImageReset1 = True Then
            '  hxNoCarImage = New HalconDotNet.HImage(My.Application.Info.DirectoryPath & "\TempImage\NOCARIMAGE.jpg")
            hxNoCarImage = objDispImage.CopyImage()
            flgBaseImageReset1 = False
            HalconDotNet.HOperatorSet.SetDraw(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple("margin"))
            HalconDotNet.HOperatorSet.SetLineWidth(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple(3.0))
            HalconDotNet.HOperatorSet.SetColor(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple("green"))
        End If
        Dim hxReduceDomain As HalconDotNet.HImage
        hxReduceDomain = objDispImage.ReduceDomain(hrCarSearchRegion1)
#If False Then

        'HalconDotNet.HOperatorSet.ReduceDomain(objDispImage, hrCarSearchRegion, hxReduceDomain)
        Dim hxSubImage As New HalconDotNet.HImage
        hxSubImage = hxReduceDomain.SubImage(hxNoCarImage, 1.0, 128.0)
        Dim hrThreshold1 As New HalconDotNet.HRegion
        hrThreshold1 = hxSubImage.Threshold(objThreshold1, 255.0) '反応を変更
        Dim hrThreshold2 As New HalconDotNet.HRegion
        hrThreshold2 = hxSubImage.Threshold(0.0, objThreshold2) '反応を変更
        Dim hrUnion2 As New HalconDotNet.HRegion
        hrUnion2 = hrThreshold1.Union2(hrThreshold2)
        Dim hrDillationCircle As New HalconDotNet.HRegion
        hrDillationCircle = hrUnion2.DilationCircle(5.5)
        Dim hrConnection As New HalconDotNet.HRegion
        hrConnection = hrDillationCircle.Connection()
        Dim hrSelRegion As New HalconDotNet.HRegion
        hrSelRegion = hrConnection.SelectShapeStd("max_area", 70.0)
#Else
        'SUSANO ADD START 20150918
        Dim hxSubImage As New HalconDotNet.HImage
        hxSubImage = hxReduceDomain.AbsDiffImage(hxNoCarImage, 1.0)
        Dim hxScaleImage As New HalconDotNet.HImage
        hxScaleImage = hxNoCarImage.DivImage(hxReduceDomain, 255.0, 0.0)

        Dim hrThreshold As New HalconDotNet.HRegion
        ' hrThreshold = hxSubImage.Threshold(IIf(objThreshold1 < objThreshold2, objThreshold1, objThreshold2), 255) '反応を変更
        hrThreshold = hxScaleImage.Threshold(IIf(objThreshold1 < objThreshold2, objThreshold1, objThreshold2), 255.0) '反応を変更
        hrThreshold = hxSubImage.Regiongrowing(2, 2, 3.0, 1000)
        'hrThreshold = hxScaleImage.AutoThreshold(2.0)
        'hrThreshold = hxScaleImage.BinThreshold()
        'hrThreshold = hxScaleImage.DynThreshold(hxScaleImage.MeanImage(10, 10), 10.0, "light")
        Dim hrDillationCircle As New HalconDotNet.HRegion
        hrDillationCircle = hrThreshold.DilationCircle(5.5)
        Dim hrConnection As New HalconDotNet.HRegion
        hrConnection = hrDillationCircle.Connection()
        Dim hrSelRegion As New HalconDotNet.HRegion
        hrSelRegion = hrConnection.SelectShapeStd("max_area", 70.0)
        'SUSANO ADD END 20150919
#End If


        If hrSelRegion.Area.Length > 1 Then
            hrSelRegion = hrSelRegion.SelectObj(1)
        End If

        Dim hrFillUpRegion As New HalconDotNet.HRegion
        hrFillUpRegion = hrSelRegion.FillUp()


        HalconDotNet.HOperatorSet.DispObj(hrCarSearchRegion1, New HalconDotNet.HTuple(objCamParam.hwindhand))
        If hrSelRegion.Area > objMinArea Then
            HalconDotNet.HOperatorSet.DispObj(hrFillUpRegion, New HalconDotNet.HTuple(objCamParam.hwindhand))
            If flgNoCar = True Then
                objCamParam.blnCar = True
                TrackCheck1 = True
                'OutMessage("車がいる")
            End If
            flgNoCar = False
        Else
            hxNoCarImage = objDispImage.CopyImage
            If flgNoCar = False Then
                objCamParam.blnCar = False
                TrackCheck1 = False
                ' OutMessage("車がいない")
                '  OutMessage(hrCarSearchRegion.Area.ToString)
            End If
            flgNoCar = True
        End If



    End Function

    'SUSANO ADD START 20151016
    Private Function TrackCheck111(ByVal objCamParam As CameraParam, ByVal objDispImage As HalconDotNet.HImage, ByVal hrCarSearchRegion As HalconDotNet.HRegion) As Boolean
        Dim objThreshold1 As Double
        Dim objThreshold2 As Double
        Dim objMinArea As Double
        GetCarDetectThreshold(objThreshold1, objThreshold2, objMinArea)
        Static Dim flgNoCar As Boolean = False
        Dim blnLiveSave As Integer = CInt(ReadFrameGrabSetting(7))

        If blnLiveSave = 1 Then
            Dim folderExists As Boolean
            Dim strLivePath As String = My.Application.Info.DirectoryPath & "\LiveImage"
            folderExists = My.Computer.FileSystem.DirectoryExists(strLivePath)
            If folderExists = False Then
                My.Computer.FileSystem.CreateDirectory(strLivePath)
            End If
            objDispImage.WriteImage("jpeg", 0, strLivePath & "\LiveImage_" & objCamParam.Cid & "_" & Now.ToFileTimeUtc & ".jpg")
        End If
        Dim hxReduceDomain As HalconDotNet.HImage
        hxReduceDomain = objDispImage.ReduceDomain(hrCarSearchRegion)
        If flgBaseImageReset1 = True Then
             flgBaseImageReset1 = False
        End If
        HalconDotNet.HOperatorSet.SetDraw(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple("margin"))
        HalconDotNet.HOperatorSet.SetLineWidth(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple(3.0))

        Dim hxMeanImage As New HalconDotNet.HImage
        Dim hrDynThreshold As New HalconDotNet.HRegion
        Dim hrConnection As New HalconDotNet.HRegion
        Dim hrSelRegion As New HalconDotNet.HRegion
        Dim hrSkeleton As New HalconDotNet.HRegion
        Dim ImageG As New HalconDotNet.HImage
        hxReduceDomain.Decompose3(ImageG, Nothing)
        hxMeanImage = ImageG.MeanImage(25, 25)
        hrDynThreshold = ImageG.DynThreshold(hxMeanImage, 10.0, "light")
        hrConnection = hrDynThreshold.Connection
        hrSelRegion = hrConnection.SelectShapeStd("max_area", 70)
        hrSkeleton = hrSelRegion.Skeleton()
        hrSelRegion = hrSkeleton

        If hrSelRegion.Area.Length > 1 Then
            hrSelRegion = hrSelRegion.SelectObj(1)
        End If

        Dim hrFillUpRegion As New HalconDotNet.HRegion
        hrFillUpRegion = hrSelRegion.FillUp()

        HalconDotNet.HOperatorSet.SetColor(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple("green"))
        HalconDotNet.HOperatorSet.DispObj(hrCarSearchRegion, New HalconDotNet.HTuple(objCamParam.hwindhand))
        HalconDotNet.HOperatorSet.SetColor(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple("blue"))
        HalconDotNet.HOperatorSet.DispObj(hrFillUpRegion, New HalconDotNet.HTuple(objCamParam.hwindhand))
        Dim D1 As Double
        Dim Dmax As Double
        hrSelRegion.DiameterRegion(Nothing, Nothing, Nothing, Nothing, D1)
        hrCarSearchRegion.DiameterRegion(Nothing, Nothing, Nothing, Nothing, Dmax)

        If D1 < Dmax * objThreshold1 Then
            'SUSANO ADD START 20151028
            If flgJyojiStart = True Then
                objCamParam.lstCarExistFlg.Add(True)
            End If
            'SUSANO ADD END 20151028
            objCamParam.blnCar = True
            If flgNoCar = True Then
                TrackCheck111 = True
                objCamParam.lstNoCarFlg.Clear()
                'OutMessage("車がいる")
                OutMessage("カメラ" & objCamParam.Cid & "では車がいる")
            End If
            flgNoCar = False
        Else
            'SUSANO ADD START 20151028
            If flgJyojiStart = True Then
                objCamParam.lstCarExistFlg.Add(False)
            End If
            'SUSANO ADD END 20151028
            objCamParam.lstNoCarFlg.Add(True)
            If flgNoCar = False Then
                objCamParam.blnCar = False
                TrackCheck111 = False
                OutMessage("カメラ" & objCamParam.Cid & "では車がいない")
            End If
            flgNoCar = True
        End If

    End Function
    Private Function TrackCheck222(ByVal objCamParam As CameraParam, ByVal objDispImage As HalconDotNet.HImage, ByVal hrCarSearchRegion As HalconDotNet.HRegion) As Boolean
        Dim objThreshold1 As Double
        Dim objThreshold2 As Double
        Dim objMinArea As Double
        GetCarDetectThreshold(objThreshold1, objThreshold2, objMinArea)
        Static Dim flgNoCar As Boolean = False
        Dim blnLiveSave As Integer = CInt(ReadFrameGrabSetting(7))

        If blnLiveSave = 1 Then
            Dim folderExists As Boolean
            Dim strLivePath As String = My.Application.Info.DirectoryPath & "\LiveImage"
            folderExists = My.Computer.FileSystem.DirectoryExists(strLivePath)
            If folderExists = False Then
                My.Computer.FileSystem.CreateDirectory(strLivePath)
            End If
            objDispImage.WriteImage("jpeg", 0, strLivePath & "\LiveImage_" & objCamParam.Cid & "_" & Now.ToFileTimeUtc & ".jpg")
        End If
        Dim hxReduceDomain As HalconDotNet.HImage
        hxReduceDomain = objDispImage.ReduceDomain(hrCarSearchRegion)
        If flgBaseImageReset2 = True Then
            flgBaseImageReset2 = False
        End If
        HalconDotNet.HOperatorSet.SetDraw(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple("margin"))
        HalconDotNet.HOperatorSet.SetLineWidth(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple(3.0))
     
        Dim hxMeanImage As New HalconDotNet.HImage
        Dim hrDynThreshold As New HalconDotNet.HRegion
        Dim hrConnection As New HalconDotNet.HRegion
        Dim hrSelRegion As New HalconDotNet.HRegion
        Dim hrSkeleton As New HalconDotNet.HRegion
        Dim ImageG As New HalconDotNet.HImage
        hxReduceDomain.Decompose3(ImageG, Nothing)
        hxMeanImage = ImageG.MeanImage(25, 25)
        hrDynThreshold = ImageG.DynThreshold(hxMeanImage, 10.0, "light")
        hrConnection = hrDynThreshold.Connection
        hrSelRegion = hrConnection.SelectShapeStd("max_area", 70)
        hrSkeleton = hrSelRegion.Skeleton()
        hrSelRegion = hrSkeleton

        If hrSelRegion.Area.Length > 1 Then
            hrSelRegion = hrSelRegion.SelectObj(1)
        End If

        Dim hrFillUpRegion As New HalconDotNet.HRegion
        hrFillUpRegion = hrSelRegion.FillUp()

        HalconDotNet.HOperatorSet.SetColor(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple("green"))
        HalconDotNet.HOperatorSet.DispObj(hrCarSearchRegion, New HalconDotNet.HTuple(objCamParam.hwindhand))
        HalconDotNet.HOperatorSet.SetColor(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple("blue"))
        HalconDotNet.HOperatorSet.DispObj(hrFillUpRegion, New HalconDotNet.HTuple(objCamParam.hwindhand))
        Dim D1 As Double
        Dim Dmax As Double
        hrSelRegion.DiameterRegion(Nothing, Nothing, Nothing, Nothing, D1)
        hrCarSearchRegion.DiameterRegion(Nothing, Nothing, Nothing, Nothing, Dmax)

        If D1 < Dmax * objThreshold2 Then
            'SUSANO ADD START 20151028
            If flgJyojiStart = True Then
                objCamParam.lstCarExistFlg.Add(True)
            End If
            'SUSANO ADD END 20151028
            objCamParam.blnCar = True
            If flgNoCar = True Then

                TrackCheck222 = True
                objCamParam.lstNoCarFlg.Clear()
                OutMessage("カメラ" & objCamParam.Cid & "では車がいる")
            End If
            flgNoCar = False
        Else
            'SUSANO ADD START 20151028
            If flgJyojiStart = True Then
                objCamParam.lstCarExistFlg.Add(False)
            End If
            'SUSANO ADD END 20151028
            objCamParam.lstNoCarFlg.Add(True)
            If objCamParam.lstNoCarFlg.Count >= CInt(ReadFrameGrabSetting(5)) Then
                objCamParam.blnCar = False
            End If
            If flgNoCar = False Then
                TrackCheck222 = False
                OutMessage("カメラ" & objCamParam.Cid & "では車がいない")
                ' Threading.Thread.Sleep(1000 * CDbl(ReadFrameGrabSetting(5)))
            End If
            flgNoCar = True

        End If


    End Function
   
    'SUSANO ADD END 20151018

    Private Function TrackCheck11(ByVal objCamParam As CameraParam, ByVal objDispImage As HalconDotNet.HImage, ByVal hrCarSearchRegion As HalconDotNet.HRegion) As Boolean
        Dim objThreshold1 As Double
        Dim objThreshold2 As Double
        Dim objMinArea As Double
        GetCarDetectThreshold(objThreshold1, objThreshold2, objMinArea)
        Static Dim dd As New HalconDotNet.HBgEsti
        Static Dim hxNoCarImage As HalconDotNet.HImage
        Static Dim flgNoCar As Boolean = False
        Static Dim AveGray1 As Double = 0
        Static Dim AveGray2 As Double = 0
        Dim blnLiveSave As Integer = CInt(ReadFrameGrabSetting(7))

        If blnLiveSave = 1 Then
            Dim folderExists As Boolean
            Dim strLivePath As String = My.Application.Info.DirectoryPath & "\LiveImage"
            folderExists = My.Computer.FileSystem.DirectoryExists(strLivePath)
            If folderExists = False Then
                My.Computer.FileSystem.CreateDirectory(strLivePath)
            End If
            objDispImage.WriteImage("jpeg", 0, strLivePath & "\LiveImage_" & objCamParam.Cid & "_" & Now.ToFileTimeUtc & ".jpg")
            'objDispImage.WriteImage("jpeg", 0, My.Application.Info.DirectoryPath & "\TempImage\TempImage_" & objCamParam.Cid & ".jpg")
        End If
        Dim hxReduceDomain As HalconDotNet.HImage
        hxReduceDomain = objDispImage.ReduceDomain(hrCarSearchRegion)
        objDispImage = hxReduceDomain.CopyImage

        If hxNoCarImage Is Nothing Or flgBaseImageReset1 = True Then
            '  hxNoCarImage = New HalconDotNet.HImage(My.Application.Info.DirectoryPath & "\TempImage\NOCARIMAGE.jpg")
            hxNoCarImage = objDispImage.CopyImage()
            hxNoCarImage = hxNoCarImage.ReduceDomain(hrCarSearchRegion)
            hxNoCarImage.MinMaxGray(hrCarSearchRegion, 50.0, AveGray1, AveGray1, Nothing)

            flgBaseImageReset1 = False
            HalconDotNet.HOperatorSet.SetDraw(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple("margin"))
            HalconDotNet.HOperatorSet.SetLineWidth(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple(3.0))
            HalconDotNet.HOperatorSet.SetColor(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple("green"))

            dd.CreateBgEsti(hxNoCarImage, 0.7, 0.7, "fixed", 0.002, 0.05, "on", 28.0, 3, 4, 25)
            ' dd.CreateBgEsti(hxNoCarImage, 0.7, 0.7, "fixed", 0.009, 0.09, "on", 8.0, 5, 4.0, 5)
            'Else
            '    objDispImage.MinMaxGray(hrCarSearchRegion1, 50.0, AveGray2, AveGray2, Nothing)

            ''dd.CreateBgEsti(hxNoCarImage, 0.7, 0.7, "fixed", 0.02, 0.2, "on", 18.0, 2, 3.25, 5)
            '' dd.CreateBgEsti(hxNoCarImage, 0.7, 0.7, "fixed", 0.02, 0.2, "on", 8.0, 10, 3.25, 15)
            'dd.CreateBgEsti(hxNoCarImage, 0.7, 0.7, "fixed", 0.02, 0.5, "on",
            '                IIf(Math.Abs(AveGray2 - AveGray1) * 0.75 < 10, 10, Math.Abs(AveGray2 - AveGray1) * 0.75), 10, 3.25, 15)
            ' dd.UpdateBgEsti(hxNoCarImage, hrCarSearchRegion1)
        End If


#If False Then

        'HalconDotNet.HOperatorSet.ReduceDomain(objDispImage, hrCarSearchRegion, hxReduceDomain)
        Dim hxSubImage As New HalconDotNet.HImage
        hxSubImage = hxReduceDomain.SubImage(hxNoCarImage, 1.0, 128.0)
        Dim hrThreshold1 As New HalconDotNet.HRegion
        hrThreshold1 = hxSubImage.Threshold(objThreshold1, 255.0) '反応を変更
        Dim hrThreshold2 As New HalconDotNet.HRegion
        hrThreshold2 = hxSubImage.Threshold(0.0, objThreshold2) '反応を変更
        Dim hrUnion2 As New HalconDotNet.HRegion
        hrUnion2 = hrThreshold1.Union2(hrThreshold2)
        Dim hrDillationCircle As New HalconDotNet.HRegion
        hrDillationCircle = hrUnion2.DilationCircle(5.5)
        Dim hrConnection As New HalconDotNet.HRegion
        hrConnection = hrDillationCircle.Connection()
        Dim hrSelRegion As New HalconDotNet.HRegion
        hrSelRegion = hrConnection.SelectShapeStd("max_area", 70.0)
#Else
        'SUSANO ADD START 20150918
        Dim hxSubImage As New HalconDotNet.HImage
        ' hxSubImage = hxReduceDomain.AbsDiffImage(hxNoCarImage, 1.0)
        Dim hrThreshold As New HalconDotNet.HRegion
        '  hrThreshold = hxSubImage.Threshold(IIf(objThreshold1 < objThreshold2, objThreshold1, objThreshold2), 255) '反応を変更
        Dim hrDillationCircle As New HalconDotNet.HRegion
        '  hrDillationCircle = hrThreshold.DilationCircle(5.5)
        Dim hrConnection As New HalconDotNet.HRegion
        ' hrConnection = hrDillationCircle.Connection()
        Dim hrSelRegion As New HalconDotNet.HRegion
        ' hrSelRegion = hrConnection.SelectShapeStd("max_area", 70.0)
        'SUSANO ADD END 20150919
        'objDispImage.MinMaxGray(hrCarSearchRegion1, 50.0, AveGray2, AveGray2, Nothing)
        Try
            'hrThreshold = hxNoCarImage.CheckDifference(objDispImage, "diff_outside", 0 - IIf(AveGray1 < AveGray2, AveGray2, AveGray1),
            '                       IIf(AveGray1 < AveGray2, AveGray2, AveGray1),
            '                       IIf(AveGray1 < AveGray2, AveGray1 - AveGray2, AveGray2 - AveGray1), 0, 0)
            hrThreshold = dd.RunBgEsti(objDispImage)
        Catch ex As Exception
            hrThreshold = dd.RunBgEsti(objDispImage)
            'Return False
        End Try

        'hrThreshold = dd.RunBgEsti(objDispImage)

        hrDillationCircle = hrThreshold.Intersection(hrCarSearchRegion)
        hrConnection = hrDillationCircle.Connection
        ' hrSelRegion = hrConnection.SelectShapeStd("max_area", 70.0)
        hrSelRegion = hrConnection.SelectShape("area", "and", objMinArea, 9999999)
        hrSelRegion = hrSelRegion.Union1

#End If


        If hrSelRegion.Area.Length > 1 Then
            hrSelRegion = hrSelRegion.SelectObj(1)
        End If

        Dim hrFillUpRegion As New HalconDotNet.HRegion
        hrFillUpRegion = hrSelRegion.FillUp()

        HalconDotNet.HOperatorSet.SetColor(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple("green"))
        HalconDotNet.HOperatorSet.DispObj(hrCarSearchRegion, New HalconDotNet.HTuple(objCamParam.hwindhand))
#If False Then

        HalconDotNet.HOperatorSet.DispObj(dd.GiveBgEsti, New HalconDotNet.HTuple(lstCamParam.Item(2).hwindhand))
#End If
        If hrSelRegion.Area > objMinArea Then
            HalconDotNet.HOperatorSet.SetColor(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple("blue"))
            HalconDotNet.HOperatorSet.DispObj(hrFillUpRegion, New HalconDotNet.HTuple(objCamParam.hwindhand))
            If flgNoCar = True Then
                objCamParam.blnCar = True
                TrackCheck11 = True
                'OutMessage("車がいる")
            End If
            flgNoCar = False
            ' dd.CreateBgEsti(hxNoCarImage, 0.7, 0.7, "fixed", 0.02, 0.2, "on", 8.0, 10, 3.25, 15)
            objDispImage.MinMaxGray(hrCarSearchRegion, 50.0, AveGray2, AveGray2, Nothing)

            'dd.CreateBgEsti(hxNoCarImage, 0.7, 0.7, "fixed", 0.02, 0.2, "on", 18.0, 2, 3.25, 5)
            ' dd.CreateBgEsti(hxNoCarImage, 0.7, 0.7, "fixed", 0.02, 0.2, "on", 8.0, 10, 3.25, 15)
            'dd.CreateBgEsti(hxNoCarImage, 0.7, 0.7, "fixed", 0.02, 0.5, "on",
            '                IIf(Math.Abs(AveGray2 - AveGray1) * objThreshold1 < 10, 10, Math.Abs(AveGray2 - AveGray1) * objThreshold1), 10, 3.25, 15)
            'dd.SetBgEstiParams(0.7, 0.7, "fixed", 0.002, 0.05, "on",
            '                IIf(Math.Abs(AveGray2 - AveGray1) * objThreshold1 < 10, 10, Math.Abs(AveGray2 - AveGray1) * objThreshold1), 3, 3.25, 15)
        Else
            hxNoCarImage = objDispImage.CopyImage
            hxNoCarImage = hxNoCarImage.ReduceDomain(hrCarSearchRegion)
            hxNoCarImage.MinMaxGray(hrCarSearchRegion, 50.0, AveGray1, AveGray1, Nothing)
            'dd.CreateBgEsti(hxNoCarImage, 0.7, 0.7, "fixed", 0.02, 0.2, "on", 8.0, 5, 3.25, 15)
            'dd.UpdateBgEsti(hxNoCarImage, hrCarSearchRegion1)
            If flgNoCar = False Then
                objCamParam.blnCar = False
                TrackCheck11 = False
                'OutMessage("車がいない")
                'OutMessage(hrCarSearchRegion.Area.ToString)
            End If
            flgNoCar = True
        End If
        dd.SetBgEstiParams(0.7, 0.7, "fixed", 0.002, 0.05, "on",
                         IIf(Math.Abs(AveGray2 - AveGray1) * objThreshold1 < 10, 10, Math.Abs(AveGray2 - AveGray1) * objThreshold1), 3, 4, 25)
        dd.SetBgEstiParams(0.7, 0.7, "fixed", 0.002, 0.05, "on",
                        objThreshold1 * (0.5 + AveGray1 / AveGray2), 3, 4, 25)

        dd.UpdateBgEsti(hxNoCarImage, hrCarSearchRegion)

    End Function
    Private Sub GetCarDetectThreshold(ByRef objThreshold1 As Double, ByRef objThreshold2 As Double, ByRef objMinArea As Double)

        'Dim filename As String = My.Application.Info.DirectoryPath & "\CarDetectThreshold.txt"
        'Dim fields As String() = Nothing
        'Dim delimiter As String = ","
        'Using parser As New TextFieldParser(filename)
        '    parser.SetDelimiters(delimiter)
        '    While Not parser.EndOfData
        '        ' Read in the fields for the current line
        '        fields = parser.ReadFields()
        '        ' Add code here to use data in fields variable.
        '        Exit While
        '    End While
        'End Using

        Try
            objThreshold1 = CDbl(ReadFrameGrabSetting(3))
            objThreshold2 = CDbl(ReadFrameGrabSetting(4))
            objMinArea = CDbl(ReadFrameGrabSetting(5))
        Catch ex As Exception
            objThreshold1 = 230.0
            objThreshold2 = 30.0
            objMinArea = 10000
        End Try
    End Sub
    Private Function TrackCheck2(ByVal objCamParam As CameraParam, ByVal objDispImage As HalconDotNet.HImage) As Boolean
        Dim objThreshold1 As Double
        Dim objThreshold2 As Double
        Dim objMinArea As Double
        GetCarDetectThreshold(objThreshold1, objThreshold2, objMinArea)
        Static Dim hxNoCarImage As HalconDotNet.HImage

        Static Dim flgNoCar As Boolean = False
        Dim blnLiveSave As Integer = CInt(ReadFrameGrabSetting(7))

        If blnLiveSave = 1 Then
            Dim folderExists As Boolean
            Dim strLivePath As String = My.Application.Info.DirectoryPath & "\LiveImage"
            folderExists = My.Computer.FileSystem.DirectoryExists(strLivePath)
            If folderExists = False Then
                My.Computer.FileSystem.CreateDirectory(strLivePath)
            End If
            objDispImage.WriteImage("jpeg", 0, strLivePath & "\LiveImage_" & objCamParam.Cid & "_" & Now.ToFileTimeUtc & ".jpg")
            'objDispImage.WriteImage("jpeg", 0, My.Application.Info.DirectoryPath & "\TempImage\TempImage_" & objCamParam.Cid & ".jpg")
        End If

        If hxNoCarImage Is Nothing Or flgBaseImageReset2 = True Then
            '  hxNoCarImage = New HalconDotNet.HImage(My.Application.Info.DirectoryPath & "\TempImage\NOCARIMAGE.jpg")
            hxNoCarImage = objDispImage.CopyImage()
            flgBaseImageReset2 = False
            HalconDotNet.HOperatorSet.SetDraw(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple("margin"))
            HalconDotNet.HOperatorSet.SetLineWidth(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple(3.0))
            HalconDotNet.HOperatorSet.SetColor(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple("green"))

        End If
        Dim hxReduceDomain As HalconDotNet.HImage

        hxReduceDomain = objDispImage.ReduceDomain(hrCarSearchRegion2)

        'HalconDotNet.HOperatorSet.ReduceDomain(objDispImage, hrCarSearchRegion, hxReduceDomain)
#If False Then


        Dim hxSubImage As New HalconDotNet.HImage
        hxSubImage = hxReduceDomain.SubImage(hxNoCarImage, 1.0, 128.0)
        Dim hrThreshold1 As New HalconDotNet.HRegion
        hrThreshold1 = hxSubImage.Threshold(objThreshold1, 255.0) '反応を変更
        Dim hrThreshold2 As New HalconDotNet.HRegion
        hrThreshold2 = hxSubImage.Threshold(0.0, objThreshold2) '反応を変更
        Dim hrUnion2 As New HalconDotNet.HRegion
        hrUnion2 = hrThreshold1.Union2(hrThreshold2)
        Dim hrDillationCircle As New HalconDotNet.HRegion
        hrDillationCircle = hrUnion2.DilationCircle(5.5)
        Dim hrConnection As New HalconDotNet.HRegion
        hrConnection = hrDillationCircle.Connection()
        Dim hrSelRegion As New HalconDotNet.HRegion
        hrSelRegion = hrConnection.SelectShapeStd("max_area", 70.0)
#Else
        'SUSANO ADD START 20150918
        Dim hxSubImage As New HalconDotNet.HImage
        hxSubImage = hxReduceDomain.AbsDiffImage(hxNoCarImage, 1.0)
        Dim hrThreshold As New HalconDotNet.HRegion
        hrThreshold = hxSubImage.Threshold(IIf(objThreshold1 < objThreshold2, objThreshold1, objThreshold2), 255) '反応を変更
        Dim hrDillationCircle As New HalconDotNet.HRegion
        hrDillationCircle = hrThreshold.DilationCircle(5.5)
        Dim hrConnection As New HalconDotNet.HRegion
        hrConnection = hrDillationCircle.Connection()
        Dim hrSelRegion As New HalconDotNet.HRegion
        hrSelRegion = hrConnection.SelectShapeStd("max_area", 70.0)
        'SUSANO ADD END 20150919
#End If

        If hrSelRegion.Area.Length > 1 Then
            hrSelRegion = hrSelRegion.SelectObj(1)
        End If

        Dim hrFillUpRegion As New HalconDotNet.HRegion
        hrFillUpRegion = hrSelRegion.FillUp()

        HalconDotNet.HOperatorSet.DispObj(hrCarSearchRegion2, New HalconDotNet.HTuple(objCamParam.hwindhand))
        If hrSelRegion.Area > objMinArea Then
            HalconDotNet.HOperatorSet.DispObj(hrFillUpRegion, New HalconDotNet.HTuple(objCamParam.hwindhand))

            If flgNoCar = True Then
                objCamParam.blnCar = True
                TrackCheck2 = True
                'OutMessage("車がいる")
            End If
            flgNoCar = False
        Else
            hxNoCarImage = objDispImage.CopyImage
            If flgNoCar = False Then
                objCamParam.blnCar = False
                TrackCheck2 = False
                ' OutMessage("車がいない")
                '  OutMessage(hrCarSearchRegion.Area.ToString)
            End If
            flgNoCar = True
        End If



    End Function


    Private Function TrackCheck22(ByVal objCamParam As CameraParam, ByVal objDispImage As HalconDotNet.HImage, ByVal hrCarSearchRegion As HalconDotNet.HRegion) As Boolean
        Dim objThreshold1 As Double
        Dim objThreshold2 As Double
        Dim objMinArea As Double
        GetCarDetectThreshold(objThreshold1, objThreshold2, objMinArea)
        Static Dim dd As New HalconDotNet.HBgEsti
        Static Dim hxNoCarImage As HalconDotNet.HImage
        Static Dim flgNoCar As Boolean = False
        Static Dim AveGray1 As Double = 0
        Static Dim AveGray2 As Double = 0
        Dim blnLiveSave As Integer = CInt(ReadFrameGrabSetting(7))

        If blnLiveSave = 1 Then
            Dim folderExists As Boolean
            Dim strLivePath As String = My.Application.Info.DirectoryPath & "\LiveImage"
            folderExists = My.Computer.FileSystem.DirectoryExists(strLivePath)
            If folderExists = False Then
                My.Computer.FileSystem.CreateDirectory(strLivePath)
            End If
            objDispImage.WriteImage("jpeg", 0, strLivePath & "\LiveImage_" & objCamParam.Cid & "_" & Now.ToFileTimeUtc & ".jpg")
            'objDispImage.WriteImage("jpeg", 0, My.Application.Info.DirectoryPath & "\TempImage\TempImage_" & objCamParam.Cid & ".jpg")
        End If
        Dim hxReduceDomain As HalconDotNet.HImage
        hxReduceDomain = objDispImage.ReduceDomain(hrCarSearchRegion)
        objDispImage = hxReduceDomain.CopyImage

        If hxNoCarImage Is Nothing Or flgBaseImageReset2 = True Then
            '  hxNoCarImage = New HalconDotNet.HImage(My.Application.Info.DirectoryPath & "\TempImage\NOCARIMAGE.jpg")
            hxNoCarImage = objDispImage.CopyImage()
            hxNoCarImage = hxNoCarImage.ReduceDomain(hrCarSearchRegion)
            hxNoCarImage.MinMaxGray(hrCarSearchRegion, 50.0, AveGray1, AveGray1, Nothing)

            flgBaseImageReset2 = False
            HalconDotNet.HOperatorSet.SetDraw(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple("margin"))
            HalconDotNet.HOperatorSet.SetLineWidth(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple(3.0))
            HalconDotNet.HOperatorSet.SetColor(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple("green"))

            dd.CreateBgEsti(hxNoCarImage, 0.7, 0.7, "fixed", 0.002, 0.05, "on", 28.0, 3, 4, 25)
            ' dd.CreateBgEsti(hxNoCarImage, 0.7, 0.7, "fixed", 0.009, 0.09, "on", 8.0, 5, 4.0, 5)
            'Else
            '    objDispImage.MinMaxGray(hrCarSearchRegion1, 50.0, AveGray2, AveGray2, Nothing)

            ''dd.CreateBgEsti(hxNoCarImage, 0.7, 0.7, "fixed", 0.02, 0.2, "on", 18.0, 2, 3.25, 5)
            '' dd.CreateBgEsti(hxNoCarImage, 0.7, 0.7, "fixed", 0.02, 0.2, "on", 8.0, 10, 3.25, 15)
            'dd.CreateBgEsti(hxNoCarImage, 0.7, 0.7, "fixed", 0.02, 0.5, "on",
            '                IIf(Math.Abs(AveGray2 - AveGray1) * 0.75 < 10, 10, Math.Abs(AveGray2 - AveGray1) * 0.75), 10, 3.25, 15)
            ' dd.UpdateBgEsti(hxNoCarImage, hrCarSearchRegion1)
        End If


#If False Then

        'HalconDotNet.HOperatorSet.ReduceDomain(objDispImage, hrCarSearchRegion, hxReduceDomain)
        Dim hxSubImage As New HalconDotNet.HImage
        hxSubImage = hxReduceDomain.SubImage(hxNoCarImage, 1.0, 128.0)
        Dim hrThreshold1 As New HalconDotNet.HRegion
        hrThreshold1 = hxSubImage.Threshold(objThreshold1, 255.0) '反応を変更
        Dim hrThreshold2 As New HalconDotNet.HRegion
        hrThreshold2 = hxSubImage.Threshold(0.0, objThreshold2) '反応を変更
        Dim hrUnion2 As New HalconDotNet.HRegion
        hrUnion2 = hrThreshold1.Union2(hrThreshold2)
        Dim hrDillationCircle As New HalconDotNet.HRegion
        hrDillationCircle = hrUnion2.DilationCircle(5.5)
        Dim hrConnection As New HalconDotNet.HRegion
        hrConnection = hrDillationCircle.Connection()
        Dim hrSelRegion As New HalconDotNet.HRegion
        hrSelRegion = hrConnection.SelectShapeStd("max_area", 70.0)
#Else
        'SUSANO ADD START 20150918
        Dim hxSubImage As New HalconDotNet.HImage
        ' hxSubImage = hxReduceDomain.AbsDiffImage(hxNoCarImage, 1.0)
        Dim hrThreshold As New HalconDotNet.HRegion
        '  hrThreshold = hxSubImage.Threshold(IIf(objThreshold1 < objThreshold2, objThreshold1, objThreshold2), 255) '反応を変更
        Dim hrDillationCircle As New HalconDotNet.HRegion
        '  hrDillationCircle = hrThreshold.DilationCircle(5.5)
        Dim hrConnection As New HalconDotNet.HRegion
        ' hrConnection = hrDillationCircle.Connection()
        Dim hrSelRegion As New HalconDotNet.HRegion
        ' hrSelRegion = hrConnection.SelectShapeStd("max_area", 70.0)
        'SUSANO ADD END 20150919
        'objDispImage.MinMaxGray(hrCarSearchRegion1, 50.0, AveGray2, AveGray2, Nothing)
        Try
            'hrThreshold = hxNoCarImage.CheckDifference(objDispImage, "diff_outside", 0 - IIf(AveGray1 < AveGray2, AveGray2, AveGray1),
            '                       IIf(AveGray1 < AveGray2, AveGray2, AveGray1),
            '                       IIf(AveGray1 < AveGray2, AveGray1 - AveGray2, AveGray2 - AveGray1), 0, 0)
            hrThreshold = dd.RunBgEsti(objDispImage)
        Catch ex As Exception
            hrThreshold = dd.RunBgEsti(objDispImage)
            'Return False
        End Try

        'hrThreshold = dd.RunBgEsti(objDispImage)

        hrDillationCircle = hrThreshold.Intersection(hrCarSearchRegion)
        hrConnection = hrDillationCircle.Connection
        ' hrSelRegion = hrConnection.SelectShapeStd("max_area", 70.0)
        hrSelRegion = hrConnection.SelectShape("area", "and", objMinArea, 9999999)
        hrSelRegion = hrSelRegion.Union1

#End If


        If hrSelRegion.Area.Length > 1 Then
            hrSelRegion = hrSelRegion.SelectObj(1)
        End If

        Dim hrFillUpRegion As New HalconDotNet.HRegion
        hrFillUpRegion = hrSelRegion.FillUp()

        HalconDotNet.HOperatorSet.SetColor(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple("green"))
        HalconDotNet.HOperatorSet.DispObj(hrCarSearchRegion, New HalconDotNet.HTuple(objCamParam.hwindhand))
#If False Then

        HalconDotNet.HOperatorSet.DispObj(dd.GiveBgEsti, New HalconDotNet.HTuple(lstCamParam.Item(2).hwindhand))
#End If
        If hrSelRegion.Area > objMinArea Then
            HalconDotNet.HOperatorSet.SetColor(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple("blue"))
            HalconDotNet.HOperatorSet.DispObj(hrFillUpRegion, New HalconDotNet.HTuple(objCamParam.hwindhand))
            If flgNoCar = True Then
                objCamParam.blnCar = True
                TrackCheck22 = True
                'OutMessage("車がいる")
            End If
            flgNoCar = False
            ' dd.CreateBgEsti(hxNoCarImage, 0.7, 0.7, "fixed", 0.02, 0.2, "on", 8.0, 10, 3.25, 15)
            objDispImage.MinMaxGray(hrCarSearchRegion, 50.0, AveGray2, AveGray2, Nothing)

            'dd.CreateBgEsti(hxNoCarImage, 0.7, 0.7, "fixed", 0.02, 0.2, "on", 18.0, 2, 3.25, 5)
            ' dd.CreateBgEsti(hxNoCarImage, 0.7, 0.7, "fixed", 0.02, 0.2, "on", 8.0, 10, 3.25, 15)
            'dd.CreateBgEsti(hxNoCarImage, 0.7, 0.7, "fixed", 0.02, 0.5, "on",
            '                IIf(Math.Abs(AveGray2 - AveGray1) * objThreshold1 < 10, 10, Math.Abs(AveGray2 - AveGray1) * objThreshold1), 10, 3.25, 15)
            'dd.SetBgEstiParams(0.7, 0.7, "fixed", 0.002, 0.05, "on",
            '                IIf(Math.Abs(AveGray2 - AveGray1) * objThreshold1 < 10, 10, Math.Abs(AveGray2 - AveGray1) * objThreshold1), 3, 3.25, 15)
        Else
            hxNoCarImage = objDispImage.CopyImage
            hxNoCarImage = hxNoCarImage.ReduceDomain(hrCarSearchRegion)
            hxNoCarImage.MinMaxGray(hrCarSearchRegion, 50.0, AveGray1, AveGray1, Nothing)
            'dd.CreateBgEsti(hxNoCarImage, 0.7, 0.7, "fixed", 0.02, 0.2, "on", 8.0, 5, 3.25, 15)
            'dd.UpdateBgEsti(hxNoCarImage, hrCarSearchRegion1)
            If flgNoCar = False Then
                objCamParam.blnCar = False
                TrackCheck22 = False
                'OutMessage("車がいない")
                'OutMessage(hrCarSearchRegion.Area.ToString)
            End If
            flgNoCar = True
        End If
        dd.SetBgEstiParams(0.7, 0.7, "fixed", 0.002, 0.05, "on",
                         IIf(Math.Abs(AveGray2 - AveGray1) * objThreshold1 < 10, 10, Math.Abs(AveGray2 - AveGray1) * objThreshold1), 3, 4, 25)
        dd.SetBgEstiParams(0.7, 0.7, "fixed", 0.002, 0.05, "on",
                        objThreshold2 * (0.5 + AveGray1 / AveGray2), 3, 4, 25)

        dd.UpdateBgEsti(hxNoCarImage, hrCarSearchRegion)

    End Function

    '    Private Function TrackCheck22(ByVal objCamParam As CameraParam, ByVal objDispImage As HalconDotNet.HImage) As Boolean
    '        Dim objThreshold1 As Double
    '        Dim objThreshold2 As Double
    '        Dim objMinArea As Double
    '        GetCarDetectThreshold(objThreshold1, objThreshold2, objMinArea)
    '        Static Dim hxNoCarImage As HalconDotNet.HImage
    '        Static Dim dd As New HalconDotNet.HBgEsti
    '        Static Dim flgNoCar As Boolean = False
    '        Dim blnLiveSave As Integer = CInt(ReadFrameGrabSetting(10))
    '        Static Dim AveGray1 As Double
    '        Static Dim AveGray2 As Double

    '        If blnLiveSave = 1 Then
    '            Dim folderExists As Boolean
    '            Dim strLivePath As String = My.Application.Info.DirectoryPath & "\LiveImage"
    '            folderExists = My.Computer.FileSystem.DirectoryExists(strLivePath)
    '            If folderExists = False Then
    '                My.Computer.FileSystem.CreateDirectory(strLivePath)
    '            End If
    '            objDispImage.WriteImage("jpeg", 0, strLivePath & "\LiveImage_" & objCamParam.Cid & "_" & Now.ToFileTimeUtc & ".jpg")
    '            'objDispImage.WriteImage("jpeg", 0, My.Application.Info.DirectoryPath & "\TempImage\TempImage_" & objCamParam.Cid & ".jpg")
    '        End If

    '        If hxNoCarImage Is Nothing Or flgBaseImageReset2 = True Then
    '            '  hxNoCarImage = New HalconDotNet.HImage(My.Application.Info.DirectoryPath & "\TempImage\NOCARIMAGE.jpg")
    '            hxNoCarImage = objDispImage.CopyImage()
    '            hxNoCarImage = hxNoCarImage.ReduceDomain(hrCarSearchRegion2)
    '            flgBaseImageReset2 = False
    '            HalconDotNet.HOperatorSet.SetDraw(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple("margin"))
    '            HalconDotNet.HOperatorSet.SetLineWidth(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple(3.0))
    '            HalconDotNet.HOperatorSet.SetColor(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple("green"))
    '            hxNoCarImage.MinMaxGray(hrCarSearchRegion2, 50.0, AveGray1, AveGray1, Nothing)

    '            dd.CreateBgEsti(hxNoCarImage, 0.7, 0.7, "fixed", 0.02, 0.5, "on", 28.0, 10, 3.25, 15)
    '        Else
    '            objDispImage.MinMaxGray(hrCarSearchRegion2, 50.0, AveGray2, AveGray2, Nothing)

    '            'dd.CreateBgEsti(hxNoCarImage, 0.7, 0.7, "fixed", 0.02, 0.2, "on", 18.0, 2, 3.25, 5)
    '            ' dd.CreateBgEsti(hxNoCarImage, 0.7, 0.7, "fixed", 0.02, 0.2, "on", 8.0, 10, 3.25, 15)
    '            'dd.CreateBgEsti(hxNoCarImage, 0.7, 0.7, "fixed", 0.02, 0.5, "on", Math.Abs(AveGray2 - AveGray1) * 0.75, 10, 3.25, 15)
    '        End If
    '        Dim hxReduceDomain As HalconDotNet.HImage

    '        hxReduceDomain = objDispImage.ReduceDomain(hrCarSearchRegion2)

    '        'HalconDotNet.HOperatorSet.ReduceDomain(objDispImage, hrCarSearchRegion, hxReduceDomain)
    '#If False Then


    '        Dim hxSubImage As New HalconDotNet.HImage
    '        hxSubImage = hxReduceDomain.SubImage(hxNoCarImage, 1.0, 128.0)
    '        Dim hrThreshold1 As New HalconDotNet.HRegion
    '        hrThreshold1 = hxSubImage.Threshold(objThreshold1, 255.0) '反応を変更
    '        Dim hrThreshold2 As New HalconDotNet.HRegion
    '        hrThreshold2 = hxSubImage.Threshold(0.0, objThreshold2) '反応を変更
    '        Dim hrUnion2 As New HalconDotNet.HRegion
    '        hrUnion2 = hrThreshold1.Union2(hrThreshold2)
    '        Dim hrDillationCircle As New HalconDotNet.HRegion
    '        hrDillationCircle = hrUnion2.DilationCircle(5.5)
    '        Dim hrConnection As New HalconDotNet.HRegion
    '        hrConnection = hrDillationCircle.Connection()
    '        Dim hrSelRegion As New HalconDotNet.HRegion
    '        hrSelRegion = hrConnection.SelectShapeStd("max_area", 70.0)
    '#Else
    '        'SUSANO ADD START 20150918
    '        Dim hxSubImage As New HalconDotNet.HImage
    '        ' hxSubImage = hxReduceDomain.AbsDiffImage(hxNoCarImage, 1.0)
    '        Dim hrThreshold As New HalconDotNet.HRegion
    '        'hrThreshold = hxSubImage.Threshold(IIf(objThreshold1 < objThreshold2, objThreshold1, objThreshold2), 255) '反応を変更
    '        Dim hrDillationCircle As New HalconDotNet.HRegion
    '        ' hrDillationCircle = hrThreshold.DilationCircle(5.5)
    '        Dim hrConnection As New HalconDotNet.HRegion
    '        ' hrConnection = hrDillationCircle.Connection()
    '        Dim hrSelRegion As New HalconDotNet.HRegion
    '        '  hrSelRegion = hrConnection.SelectShapeStd("max_area", 70.0)
    '        'SUSANO ADD END 20150919
    '        '
    '        objDispImage.MinMaxGray(hrCarSearchRegion2, 50.0, AveGray2, AveGray2, Nothing)
    '        Try
    '            hrThreshold = hxNoCarImage.CheckDifference(objDispImage, "diff_outside", 0 - IIf(AveGray1 < AveGray2, AveGray2, AveGray1),
    '                                   IIf(AveGray1 < AveGray2, AveGray2, AveGray1),
    '                                   IIf(AveGray1 < AveGray2, AveGray1 - AveGray2, AveGray2 - AveGray1), 0, 0)
    '            hrThreshold = dd.RunBgEsti(objDispImage)
    '        Catch ex As Exception
    '            hrThreshold = dd.RunBgEsti(objDispImage)
    '            'Return False
    '        End Try
    '        hrConnection = hrThreshold.Connection
    '        hrDillationCircle = hrConnection.Intersection(hrCarSearchRegion2)
    '        hrSelRegion = hrDillationCircle.SelectShapeStd("max_area", 70.0)

    '#End If

    '        If hrSelRegion.Area.Length > 1 Then
    '            hrSelRegion = hrSelRegion.SelectObj(1)
    '        End If

    '        Dim hrFillUpRegion As New HalconDotNet.HRegion
    '        hrFillUpRegion = hrSelRegion.FillUp()
    '        HalconDotNet.HOperatorSet.SetColor(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple("green"))
    '        HalconDotNet.HOperatorSet.DispObj(hrCarSearchRegion2, New HalconDotNet.HTuple(objCamParam.hwindhand))
    '#If False Then

    '        HalconDotNet.HOperatorSet.DispObj(dd.GiveBgEsti, New HalconDotNet.HTuple(lstCamParam.Item(3).hwindhand))
    '#End If
    '        If hrSelRegion.Area > objMinArea Then
    '            HalconDotNet.HOperatorSet.SetColor(New HalconDotNet.HTuple(objCamParam.hwindhand), New HalconDotNet.HTuple("blue"))
    '            HalconDotNet.HOperatorSet.DispObj(hrFillUpRegion, New HalconDotNet.HTuple(objCamParam.hwindhand))
    '            If flgNoCar = True Then
    '                objCamParam.blnCar = True
    '                TrackCheck22 = True
    '                'OutMessage("車がいる")
    '            End If
    '            flgNoCar = False

    '            dd.CreateBgEsti(hxNoCarImage, 0.7, 0.7, "fixed", 0.02, 0.5, "on",
    '                           IIf(Math.Abs(AveGray2 - AveGray1) * objThreshold1 < 10, 10, Math.Abs(AveGray2 - AveGray1) * objThreshold1), 10, 3.25, 15)
    '        Else
    '            hxNoCarImage = objDispImage.CopyImage
    '            hxNoCarImage = hxNoCarImage.ReduceDomain(hrCarSearchRegion2)
    '            hxNoCarImage.MinMaxGray(hrCarSearchRegion2, 50.0, AveGray1, AveGray1, Nothing)
    '            dd.CreateBgEsti(hxNoCarImage, 0.7, 0.7, "fixed", 0.02, 0.5, "on",
    '                           IIf(Math.Abs(AveGray2 - AveGray1) * objThreshold1 < 10, 10, Math.Abs(AveGray2 - AveGray1) * objThreshold1), 10, 3.25, 15)
    '            If flgNoCar = False Then
    '                objCamParam.blnCar = False
    '                TrackCheck22 = False
    '                ' OutMessage("車がいない")
    '                '  OutMessage(hrCarSearchRegion.Area.ToString)
    '            End If
    '            flgNoCar = True
    '        End If



    '    End Function

    Private Sub LiveImageOneCamera(ByVal CID As Integer)
        Dim objCamparam As CameraParam = lstCamParam.Item(CID)

        If objCamparam.objCheckBox.Checked = True And (Not objCamparam.Fr Is Nothing) Then
            Try
                Dim objDispImage As HalconDotNet.HImage = objCamparam.Fr.GrabImage
                'Dim objDispImage As HalconDotNet.HImage = objCamparam.Fr.GrabImageAsync(10) '20160727 Kiryuu VFORM　OSごと終了

                Dim gain As Integer = 0
                Dim exp As Integer = 0
                MirrorImage(objCamparam, objDispImage)
                HalconDotNet.HOperatorSet.DispObj(objDispImage, New HalconDotNet.HTuple(objCamparam.hwindhand))
                Debug.WriteLine(objCamparam.Fr.GetFramegrabberParam("gain_master").ToString())
                Debug.WriteLine(objCamparam.Fr.GetFramegrabberParam("exposure").ToString())
                Debug.WriteLine(objCamparam.Fr.GetFramegrabberParam("frame_rate").ToString())
                TrackCheckAndSensorReset(objCamparam, objDispImage)

                If objCamparam.blnCamConnectOK = False Then
                    OutMessage("カメラ番号:" & objCamparam.Cid & "の接続が戻りました。")
                End If
                objCamparam.blnCamConnectOK = True
            Catch ex As Exception
                If objCamparam.blnCamConnectOK = True Then
                    OutMessage("カメラ番号:" & objCamparam.Cid & "の接続が切れました！！！。確認してください！！！")
                    objCamparam.blnCamConnectOK = False
                End If
            End Try
        End If
        'ＳＵＳＡＮＯ　ＡＤＤ　ＳＴＡＲＴ　２０１５１０２１
        ''ピーク値を常時取得する。
        'Dim flgJyojiSensor As Integer = CInt(ReadFrameGrabSetting(11))
        'If flgJyojiSensor = 1 Then


        '    'Dim Peak As Double = 0.0
        '    'SerialPort_PeakGet(Peak)
        '    'If Peak > 0 Then
        '    '    Dim Cid1 As Integer = CInt(ReadFrameGrabSetting(1))
        '    '    For Each objCam As CameraParam In lstCamParam
        '    '        If objCam.Cid = Cid1 Then
        '    '            If objCam.blnCar = True Then
        '    '                lstPeakValue.Add(Peak)
        '    '            Else
        '    '                lstPeakValue.Clear()
        '    '            End If
        '    '        End If
        '    '    Next
        '    'End If
        '    'SUSANO ADD START 20151028
        '    Dim SettingFile As String = My.Application.Info.DirectoryPath & "\Sensor_Setting.txt"
        '    Dim S_Sensor As New Sensser_setting(SettingFile)

        '    Dim MVal As Double = 0.0
        '    SerialPort_MonitorValueGet(MVal)
        '    Static Dim JyojiStart As Date

        '    If MVal > 0 And flgJyojiStart = False Then
        '        JyojiStart = Now
        '        flgJyojiStart = True
        '    End If
        '    If flgJyojiStart = True Then
        '        Dim intNowCompare As Integer = JyojiStart.AddSeconds(S_Sensor.CarExistInterval).CompareTo(Now)
        '        If intNowCompare = -1 Then
        '            Dim Cid1 As Integer = CInt(ReadFrameGrabSetting(1))
        '            Dim Cid2 As Integer = CInt(ReadFrameGrabSetting(2))
        '            Dim blnFlg As Boolean = False
        '            For Each objCam As CameraParam In lstCamParam
        '                If objCam.Cid = Cid1 Then
        '                    For Each blnCarExist As Boolean In objCam.lstCarExistFlg
        '                        If blnCarExist = True Then
        '                            blnFlg = True
        '                            Exit For
        '                        End If
        '                    Next
        '                    objCam.lstCarExistFlg.Clear()
        '                End If
        '                If objCam.Cid = Cid2 Then
        '                    For Each blnCarExist As Boolean In objCam.lstCarExistFlg
        '                        If blnCarExist = True Then
        '                            blnFlg = True
        '                            Exit For
        '                        End If
        '                    Next
        '                    objCam.lstCarExistFlg.Clear()
        '                End If
        '            Next
        '            If blnFlg = False Then
        '                'センサーをリセット
        '                Dim Peak As Double = 0.0
        '                SerialPort_PeakGet(Peak)
        '                OutMessage("エリアセンサーを常時監視機能により、リセットしました。リセット時のピーク値＝" & Peak)
        '            End If
        '            flgJyojiStart = False
        '        End If
        '    End If
        '    Application.DoEvents()
        '    ' SUSANO ADD END 20151028
        'End If

        'ＳＵＳＡＮＯ　ＡＤＤ　END　２０１５１０２２
    End Sub
    Private Sub FrmMain_MouseWheel(sender As Object, e As MouseEventArgs) Handles Me.MouseWheel
        If ResizePart(hvWinPrev, e.Delta) = True Then
            Dim objfirstCam As CameraParam = Nothing

            For Each objCamparam As CameraParam In lstCamParam
                If objCamparam.objCheckBox.Checked = True And (Not objCamparam.Fr Is Nothing) Then
                    If PrevImageCid = objCamparam.Cid Then
                        objfirstCam = objCamparam
                        Exit For
                    End If
                End If
            Next
            If objfirstCam Is Nothing Then
                Exit Sub
            End If
            If Not objfirstCam.hvImageResult Is Nothing Then
                Op.DispImage(objfirstCam.hvImageResult, hvWinPrev)
                DispCT(hvWinPrev, objfirstCam.objResultTarget)
            End If

        End If
    End Sub


    Private Sub btnSatsueiSoku_Click(sender As Object, e As EventArgs) Handles btnSatsueiSoku.Click
        ' Timer1.Stop()
        '連続して押した場合の対処
        If Me.Enabled = False Then
            Exit Sub
        End If
        Me.Enabled = False

        flgKaisekiKahi = True

        TimersStop()
        System.Threading.Thread.Sleep(100)
        'OutMessage("撮影・測定　開始")

        '監視用測定値（アドレス５２２）読み取りテストコード(20151027 Tezuka ADD Start)
        'SenserSet.MonitorAddress = 522
        'Dim MVal, MVal2 As Double
        'SerialPort_MonitorValueGet(MVal)
        'SerialPort_PeakGet(MVal2)
        'IncidentGetAndDisp(1)
        'Exit Sub
        '監視用測定値（アドレス５２２）読み取りテストコード(20151027 Tezuka ADD End)

        Dim fileContents As String
        fileContents = My.Computer.FileSystem.ReadAllText(My.Application.Info.DirectoryPath & "\flgCTari.txt")
        flgCTari = IIf(fileContents.StartsWith("1"), True, False)
        If flgCTari = True Then
            OutMessage("撮影・測定　をコードターゲットありのモードで開始しました。")
        Else
            OutMessage("撮影・測定　をコードターゲットなしのモードで開始しました。")
            ' strBeforeKoujiFolder = My.Computer.FileSystem.ReadAllText(My.Application.Info.DirectoryPath & "\BasePointSearchKoujiDataPath.txt")
            strBeforeKoujiFolder = My.Application.Info.DirectoryPath & "\Setting\BasePointSearch"
        End If


        ' Op.CloseAllFramegrabbers()

        DoveModeSet_Read()
        If DoveModeFlg >= 3 Then
            Dim threadA As New System.Threading.Thread(New System.Threading.ThreadStart(AddressOf Thead_RingProgress))
            iRingProgress = 0
            threadA.Start()
        End If

        If blnParaShoot Then
#If blnDotNet Then
            Parallel.ForEach(Of CameraParam)(lstCamParam, Sub(objcamparam)
                                                              Try
                                                                  If objcamparam.objCheckBox.Checked = True And (Not objcamparam.Fr Is Nothing) Then
                                                                      objcamparam.Fr.SetFramegrabberParam("vertical_resolution", 1)
                                                                      objcamparam.Fr.SetFramegrabberParam("horizontal_resolution", 1)
                                                                  End If
                                                              Catch ex As Exception
                                                                  objcamparam.blnCamConnectOK = False
                                                                  OutMessage("エラーメッセージ 「" & ex.Message & "」")
                                                              End Try

                                                          End Sub)
            Dim blnSatueiSuru As Boolean = True
            For Each objCamparam As CameraParam In lstCamParam
                If objCamparam.objCheckBox.Checked = True And (Not objCamparam.Fr Is Nothing) Then
                    If objCamparam.blnCamConnectOK = False Then
                        OutMessage("カメラ番号：" & objCamparam.Cid & "が撮影不可の状態です！！！　確認してください。")
                        blnSatueiSuru = False
                    End If
                End If
            Next
            If blnSatueiSuru = False Then
                Exit Sub
            End If
            'For Each objCamparam As CameraParam In lstCamParam
            '    If objCamparam.objCheckBox.Checked = True And (Not objCamparam.Fr Is Nothing) Then
            '        objCamparam.Fr.SetFramegrabberParam("vertical_resolution", 1)
            '        objCamparam.Fr.SetFramegrabberParam("horizontal_resolution", 1)
            '    End If
            'Next

            'Op.SetPart(AxHWindowXCtrl1.HalconWindow.HalconID, 0, 0, CInt(intH), CInt(intW))
            'Op.SetPart(AxHWindowXCtrl2.HalconWindow.HalconID, 0, 0, CInt(intH), CInt(intW))
            'Op.SetPart(AxHWindowXCtrl3.HalconWindow.HalconID, 0, 0, CInt(intH), CInt(intW))
            'Op.SetPart(AxHWindowXCtrl4.HalconWindow.HalconID, 0, 0, CInt(intH), CInt(intW))
            'Op.SetPart(AxHWindowXCtrl5.HalconWindow.HalconID, 0, 0, CInt(intH), CInt(intW))
            'Op.SetPart(AxHWindowXCtrl6.HalconWindow.HalconID, 0, 0, CInt(intH), CInt(intW))
            'Op.SetPart(AxHWindowXCtrl7.HalconWindow.HalconID, 0, 0, CInt(intH), CInt(intW))
            'Op.SetPart(AxHWindowXCtrl8.HalconWindow.HalconID, 0, 0, CInt(intH), CInt(intW))
            'Op.SetPart(AxHWindowXCtrl9.HalconWindow.HalconID, 0, 0, CInt(intH), CInt(intW))
            'Op.SetPart(AxHWindowXCtrl10.HalconWindow.HalconID, 0, 0, CInt(intH), CInt(intW))
            'Op.SetPart(AxHWindowXCtrl11.HalconWindow.HalconID, 0, 0, CInt(intH), CInt(intW))
            'Op.SetPart(AxHWindowXCtrl12.HalconWindow.HalconID, 0, 0, CInt(intH), CInt(intW))

#Else
            For Each objCamparam As CameraParam In lstCamParam
                If objCamparam.objCheckBox.Checked = True And (Not objCamparam.objAcqHandle Is Nothing) Then
                    Op.SetFramegrabberParam(objCamparam.objAcqHandle, "vertical_resolution", 1)
                    Op.SetFramegrabberParam(objCamparam.objAcqHandle, "horizontal_resolution", 1)
                End If
            Next
#End If


        Else
            For Each objCamparam As CameraParam In lstCamParam
                If objCamparam.objCheckBox.Checked = True And (Not objCamparam.objAcqHandle Is Nothing) Then
                    Op.CloseFramegrabber(objCamparam.objAcqHandle)
                End If
            Next
        End If


        'strNewKoujiFolder = My.Settings.KoujiRootFolder & "\" & "Data" & Now.ToFileTimeUtc
        strNewKoujiFolder = My.Settings.KoujiRootFolder & "\" & "Data" & Now.ToString.Replace(":", "").Replace("/", "").Replace(" ", "_")
        My.Computer.FileSystem.CreateDirectory(strNewKoujiFolder)
        OutMessage("工事フォルダに新規フォルダ「" & strNewKoujiFolder & "」を作成しました。")



        strKojiMei = "新規工事"
        strKojiFolder = strNewKoujiFolder
        ' LblWorkName.Text = "工事名 : " & strKojiMei & "      フォルダー名 : " & strKojiFolder
        ' LblWorkName.Visible = True
        ' GroupBox1.Visible = True
        My.Computer.FileSystem.CopyFile(FBM_MDB, strKojiFolder & "\" & FBM_MDB, True)
        Dim path As String = strKojiFolder & "\工事名.txt"
        If My.Computer.FileSystem.FileExists(path) Then
            My.Computer.FileSystem.DeleteFile(path)
        End If
        Dim fs As New StreamWriter(path, True, System.Text.Encoding.Default)
        fs.Write(strKojiMei)
        fs.Close()
        resultImageFolder = strKojiFolder & "\"
        ConnectDbFBM(resultImageFolder)

        blnCamLive = False
        OutMessage("撮影処理開始。")
        Dim strSatueeiSetteiMes As String = ""
        strSatueeiSetteiMes = "撮影設定名称：" & lstShootParam.Item(cbShootParam.SelectedIndex).setteiname
        strSatueeiSetteiMes = strSatueeiSetteiMes & " (" & lstShootParam.Item(cbShootParam.SelectedIndex).gain_master & ","
        strSatueeiSetteiMes = strSatueeiSetteiMes & lstShootParam.Item(cbShootParam.SelectedIndex).exposure_min & ","
        strSatueeiSetteiMes = strSatueeiSetteiMes & lstShootParam.Item(cbShootParam.SelectedIndex).exposure_max & ","
        strSatueeiSetteiMes = strSatueeiSetteiMes & lstShootParam.Item(cbShootParam.SelectedIndex).exposure_kankaku & ","
        strSatueeiSetteiMes = strSatueeiSetteiMes & lstShootParam.Item(cbShootParam.SelectedIndex).targetthreshold_min & ","
        strSatueeiSetteiMes = strSatueeiSetteiMes & lstShootParam.Item(cbShootParam.SelectedIndex).targetthreshold_max & ","
        strSatueeiSetteiMes = strSatueeiSetteiMes & lstShootParam.Item(cbShootParam.SelectedIndex).targetthreshold_kankaku & ")"

        OutMessage(strSatueeiSetteiMes)
        If blnParaShoot Then
            objShootParam = lstShootParam.Item(cbShootParam.SelectedIndex)
            Parallel.ForEach(Of CameraParam)(lstCamParam, Sub(objcamparam)
                                                              Try
#If blnDotNet Then
                                                                  If objcamparam.objCheckBox.Checked = True And (Not objcamparam.Fr Is Nothing) Then
                                                                      ShootImageOnlyWithDotNet(objcamparam)
                                                                  End If
#Else
                                                                              If objcamparam.objCheckBox.Checked = True And (Not objcamparam.objAcqHandle Is Nothing) Then
                                                                                  ShootImageOnly2(objcamparam)
                                                                              End If
#End If

                                                              Catch ex As Exception
                                                                  OutMessage("エラーメッセージ 「" & ex.Message & "」")
                                                              End Try

                                                          End Sub)


            'For Each objCamParam As CameraParam In lstCamParam
            '    If objCamParam.objCheckBox.Checked = True And (Not objCamParam.Fr Is Nothing) Then
            '        ShootImageOnlyWithDotNet(objCamParam)
            '    End If
            'Next

            '画像取得ができなかった場合再度撮影する。最大３回まで
            Parallel.ForEach(Of CameraParam)(lstCamParam, Sub(objcamparam)
                                                              If objcamparam.objCheckBox.Checked = True And (Not objcamparam.Fr Is Nothing) Then
                                                                  If objcamparam.blnCamShootOK = False Then
                                                                      For iReShootCnt As Integer = 1 To iReShootMaxCnt
                                                                          ShootImageOnlyWithDotNet(objcamparam)
                                                                          If objcamparam.blnCamShootOK = True Then
                                                                              Exit For
                                                                          End If
                                                                      Next
                                                                  End If
                                                              End If
                                                          End Sub)

            OutMessage("撮影処理終了。")
            OutMessage("解析処理開始。")
#If DebugTest = True Then
            ''strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data130860080602916710-CT無&4TG-1回目"
            ' strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data130860147980404250-CT有-2回目"
            ''strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data130863487761942091_ct_goninsiki"
            'strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data130864152410012581_CASE26"
            'strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data130864103953901050_CASE23"
            'strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data20150917_93604_CASE56"
            'strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data20150917_145917_case65"
            'strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data20150925_00552_Case4"
            'strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data20150917_145917_case65"
            'strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data20150928_223631_z1450\Data20150928_223631_z1450"
            'strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data20151009_110006_c3-#406"
            'strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data20151012_104830_#ct411_403"
            'strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data20151013_140044解析エラー"
            'strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data20151013_143537-解析エラー"
            'strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data20151013_144429-解析エラー"
            'strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\測点Data20151014_100119-C7で406が2回-C1ではじかれた"
            'strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\C1で406ロスト\Data20151014_110319"
            'strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\1141_C1で406ロスト（デカダンプ）\Data20151014_114633"
            'strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data20151014_155055"
            'strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\1408_C8で車両部品を406に誤認識、C9でダミー406を認識\Data20151013_140741"
            'strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data20151007_133527_case11_C7406ミス\Data20151007_133527_case11_C7406ミス"
            'strNewKoujiFolder = "C:\01_VFORM_Projects\Test工事データ\Data20151026_170941_通常配置"
            ''(strNewKoujiFolder = "C:\01_VFORM_Projects\Test工事データ\Data20151120_165509_正常に撮影"
            ''strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\誤認工事データ\412認識せず\Data20151120_162034"
            'strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data20151120_114526_車幅2503\Data20151120_114526_車幅2503"

            'strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\20151201\Data20151130_135856_ダンプ406認識せず\Data20151130_135856_ダンプ406認識せず"
            'strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\20151201\Data20151130_131052_C1_406認識せず_ごみ点なし"
            'strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data130864152410012581_CASE26KIJYUN"

            strNewKoujiFolder = "C:\01_VFORM_Projects\Test工事データ\Data20151201_133329_サンプル用"

            strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\20151201\Data20151130_135856_ダンプ406認識せず\Data20151130_135856_ダンプ406認識せず"
            strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\20151201\Data20151130_131052_C1_406認識せず_ごみ点なし"
            strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data130864152410012581_CASE26KIJYUN"
            'strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data20160218_133602\Data20160218_133602"
            'strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data20160218_145241\Data20160218_145241"
            'strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data20160225_105452_車長１\Data20160225_105452"
            strNewKoujiFolder = "C:\01_VFORM_Projects\Test工事データ\Data20160603_153805_正常_コピー"
            strNewKoujiFolder = "C:\01_VFORM_Projects\Test工事データ\Data20160603_153805_正常"
            strNewKoujiFolder = "C:\01_VFORM_Projects\20170510_外部評定テスト\sueoki\Data20170510_185923"
            strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data20170510_180936"       'CTari
            strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data20170510_183149"       'CTnashi
            strNewKoujiFolder = "C:\01_VFORM_Projects\20170510_外部評定テスト\sueoki\Data20170510_191950_スケール・座標系以外隠し"    '基準点なし
            'strNewKoujiFolder = "C:\01_VFORM_Projects\20170510_外部評定テスト\sueoki\Data20170510_185923"

            '佐野回帰データ
            strNewKoujiFolder = "Y:\開発\#研究開発\H29年度\CATS-1706H（デジカメ計測システム（VFORM）開発）\05テスト記録\3)軽微な改善\実行速度向上\回帰テストデータ\Case1_Data20160202_174700"
            'strNewKoujiFolder = "Y:\開発\#研究開発\H29年度\CATS-1706H（デジカメ計測システム（VFORM）開発）\05テスト記録\3)軽微な改善\実行速度向上\回帰テストデータ\Case2_Data20160203_131806"
            'strNewKoujiFolder = "Y:\開発\#研究開発\H29年度\CATS-1706H（デジカメ計測システム（VFORM）開発）\05テスト記録\3)軽微な改善\実行速度向上\回帰テストデータ\Case3_Data20151026_170941_通常配置"
            'strNewKoujiFolder = "Y:\開発\#研究開発\H29年度\CATS-1706H（デジカメ計測システム（VFORM）開発）\05テスト記録\3)軽微な改善\実行速度向上\回帰テストデータ\Case4_Data20151120_165509_正常に撮影"
            'strNewKoujiFolder = "Y:\開発\#研究開発\H29年度\CATS-1706H（デジカメ計測システム（VFORM）開発）\05テスト記録\3)軽微な改善\実行速度向上\回帰テストデータ\Case5_Data20151201_133329_サンプル用"
            'strNewKoujiFolder = "Y:\開発\#研究開発\H29年度\CATS-1706H（デジカメ計測システム（VFORM）開発）\05テスト記録\3)軽微な改善\実行速度向上\回帰テストデータ\Case6_Data20151201_133329_正常"
            'strNewKoujiFolder = "Y:\開発\#研究開発\H29年度\CATS-1706H（デジカメ計測システム（VFORM）開発）\05テスト記録\3)軽微な改善\実行速度向上\回帰テストデータ\Case7_Data20160603_153805_正常_全基準点あり"
            'strNewKoujiFolder = "Y:\開発\#研究開発\H29年度\CATS-1706H（デジカメ計測システム（VFORM）開発）\05テスト記録\3)軽微な改善\実行速度向上\回帰テストデータ\Case8_Data20160603_153805_全TG設置"
            'strNewKoujiFolder = "Y:\開発\#研究開発\H29年度\CATS-1706H（デジカメ計測システム（VFORM）開発）\05テスト記録\3)軽微な改善\実行速度向上\回帰テストデータ\Case9_Data20160727_153110_TG配置確認"
            'strNewKoujiFolder = "Y:\開発\#研究開発\H29年度\CATS-1706H（デジカメ計測システム（VFORM）開発）\05テスト記録\3)軽微な改善\実行速度向上\回帰テストデータ\Case10_Data20160206_112000"

            Dim FolderBrowserDialog1 As New FolderBrowserDialog
            FolderBrowserDialog1.SelectedPath = "Y:\開発\#研究開発\H29年度\CATS-1706H（デジカメ計測システム（VFORM）開発）\05テスト記録\3)軽微な改善\実行速度向上\回帰テストデータ"
            If Not FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                strNewKoujiFolder = My.Settings.TestKoujiFolder
                Return
            End If
            strNewKoujiFolder = FolderBrowserDialog1.SelectedPath




            ReadAllShootedData(strNewKoujiFolder)
            ' strBeforeKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\BasePointSearch"
#End If
            Dim strMonFile As String = "開始" & Now.ToLongDateString & vbNewLine

            For Each objCamParam As CameraParam In lstCamParam
                If objCamParam.objCheckBox.Checked = True And (Not objCamParam.Fr Is Nothing) Then
                    objBeforeTarget = New TargetDetect
                    If objBeforeTarget.lstCT Is Nothing Then
                        objBeforeTarget.lstCT = New List(Of CodedTarget)
                    Else
                        objBeforeTarget.lstCT.Clear()
                    End If
                    If flgCTari = False Then

                        Try
                            If strBeforeKoujiFolder <> "" Then
                                ConnectDbFBM(strBeforeKoujiFolder & "\")
                                objBeforeTarget = New TargetDetect
                                objBeforeTarget.ReadData("", CInt(objCamParam.Cid))
                                AccessDisConnect()
                                Dim lsttmpBasePoint As New List(Of measurepoint3dTable)
                                GetBasePoint(lsttmpBasePoint)
                                Dim icnt As Integer
                                For icnt = objBeforeTarget.lstCT.Count - 1 To 0 Step -1
                                    Dim blnSonzai As Boolean = False
                                    For Each objBaseP As measurepoint3dTable In lsttmpBasePoint
                                        If objBeforeTarget.lstCT.Item(icnt).systemlabel = objBaseP.systemlabel Then
                                            blnSonzai = True
                                            Exit For
                                        End If
                                    Next
                                    If blnSonzai = False Then
                                        objBeforeTarget.lstCT.RemoveAt(icnt)
                                    End If
                                Next
                            Else
                            End If
                        Catch ex As Exception
                            OutMessage("エラーメッセージ 「" & ex.Message & "」")
                        End Try
                        ConnectDbFBM(strNewKoujiFolder & "\")
                    End If

                    ShootImage(objCamParam)

                    For Each objtmpTT As CodedTarget In objBeforeTarget.lstCT
                        Dim blnAriri As Boolean = False
                        For Each objct As CodedTarget In objCamParam.objResultTarget.lstCT
                            If objtmpTT.CT_ID = objct.CT_ID Then
                                blnAriri = True
                                Exit For
                            End If
                        Next
                        If blnAriri = False Then
                            strMonFile = strMonFile & "CameraNo=" & objtmpTT.ImageID1 & "からCTNo=" & objtmpTT.CT_ID & "を取得しませんでした." & vbNewLine
                        End If
                    Next

                End If
            Next
            strMonFile = strMonFile & "終了" & Now.ToLongDateString & vbNewLine
            My.Computer.FileSystem.WriteAllText(strNewKoujiFolder & "\BasePointDetectMonitor.txt", strMonFile, True)

            'strBeforeKoujiFolder = strNewKoujiFolder ' SUURI DELETE 20150806



            'Dim lstThread As New List(Of System.Threading.Thread)
            'For Each objCamParam As CameraParam In lstCamParam
            '    If objCamParam.objCheckBox.Checked = True And (Not objCamParam.objAcqHandle Is Nothing) Then
            '        Try

            '            Dim t As System.Threading.Thread
            '            t = New System.Threading.Thread(New System.Threading.ParameterizedThreadStart(AddressOf ShootImageOnly2))
            '            t.IsBackground = True
            '            'スレッドを開始する
            '            t.Start(objCamParam)
            '            lstThread.Add(t)
            '        Catch ex As Exception
            '            MsgBox(ex.Message)
            '        End Try
            '    End If

            'Next
            'Do
            '    Dim blnOk As Boolean = True
            '    For Each objThread As System.Threading.Thread In lstThread
            '        If objThread.IsAlive = True Then
            '            blnOk = False
            '            Exit For
            '        End If
            '    Next
            '    If blnOk = True Then
            '        Exit Do
            '    End If
            'Loop

        Else
            For Each objCamparam As CameraParam In lstCamParam
                If objCamparam.objCheckBox.Checked = True And (Not objCamparam.objAcqHandle Is Nothing) Then
                    If flgCTari = True Then
                        Shell(My.Application.Info.DirectoryPath & "\FrameGrab.exe " & objCamparam.Cid & " " & strNewKoujiFolder, AppWinStyle.Hide, False)

                    Else
                        Shell(My.Application.Info.DirectoryPath & "\FrameGrab.exe " & objCamparam.Cid & " " & strNewKoujiFolder & " " & strBeforeKoujiFolder, AppWinStyle.Hide, False)

                    End If
                End If
            Next
            strBeforeKoujiFolder = strNewKoujiFolder

            Do
                Dim ps As System.Diagnostics.Process() = System.Diagnostics.Process.GetProcesses()

                Dim blnFrameGrabeIsGoing As Boolean = False
                For Each p As System.Diagnostics.Process In ps
                    If p.ProcessName = "FrameGrab" Then
                        blnFrameGrabeIsGoing = True
                    End If
                Next
                If blnFrameGrabeIsGoing = False Then
                    Exit Do
                End If
            Loop
            OutMessage("撮影処理終了。")
            ReadAllShootedData(strNewKoujiFolder)
        End If

#If DebugTest = True Then
        'strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data130860147980404250-CT有-2回目"
        ''strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data130863487761942091_ct_goninsiki"
        'strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data130864152410012581_CASE26KIJYUN"

        'dbClass.Connect(strNewKoujiFolder & "\dbFBM.mdb")
        'ReadAllShootedData(strNewKoujiFolder)
        'dbClass.DisConnect()
#End If

        If flgKaisekiKahi = True Then

            ' SUSANO ADD START 20160805
            Dim CalcGaibuParamFile As String = My.Application.Info.DirectoryPath & "\Setting\CalcGaibuParam.txt"
            Dim fileContents1 As String
            fileContents1 = My.Computer.FileSystem.ReadAllText(CalcGaibuParamFile)
            If IsNumeric(fileContents1) Then
                If Int(fileContents1) = 0 Then

                    'ＦＲＡＭＥＧＲＡＢのdbFBM.mdbから外部パラメータを詠み込む
                    If ConnectDbFBM(My.Application.Info.DirectoryPath & "\Setting\BasePointSearch" & "\") = True Then
                        ReadCameraPose()
                    End If

                    '工事フォルダ内の計測データ.mdbから外部パラメータを読込む
                    If ConnectDbFBM(strNewKoujiFolder & "\") = True Then
                        'ReadCameraPose()
                        SaveCameraPose()
                        MonSavePose(strNewKoujiFolder & "\MonKaisekiMae.txt")
                    End If
                End If
            End If
            'SUSANO ADD END

            KaisekiAndView()

            ' SUSANO ADD START 20160805
            CalcGaibuParamFile = My.Application.Info.DirectoryPath & "\Setting\CalcGaibuParam.txt"
            fileContents1 = My.Computer.FileSystem.ReadAllText(CalcGaibuParamFile)
            If IsNumeric(fileContents1) Then
                If Int(fileContents1) = 1 Then
                    '工事フォルダ内の計測データ.mdbからＦＲＡＭＥＧＲＡＢのdbFBM.mdbに外部パラメータを保存
                    '工事フォルダ内の計測データ.mdbから外部パラメータを読込む
                    If ConnectDbYCMNoTemp(strNewKoujiFolder & "\") = True Then
                        ReadCameraPoseByKeisokuDataMDB()
                    End If
                    'ＦＲＡＭＥＧＲＡＢのdbFBM.mdbに外部パラメータを保存
                    ' If ConnectDbFBM(My.Application.Info.DirectoryPath & "\") = True Then
                    If ConnectDbFBM(My.Application.Info.DirectoryPath & "\Setting\BasePointSearch" & "\") = True Then
                        ' ReadCameraPose()
                        SaveCameraPose()
                        MonSavePose(strNewKoujiFolder & "\MonKaisekiGo.txt")

                    End If
                End If
            End If
            'SUSANO ADD END
        Else
            MsgBox("照明ＯＦＦ状態で撮影したか、予期しないエラーが発生しました。")
        End If
        OutMessage("解析処理終了。")


        If blnParaShoot = True Then
#If blnDotNet Then

            Parallel.ForEach(Of CameraParam)(lstCamParam, Sub(objcamparam)
                                                              Try
                                                                  If objcamparam.objCheckBox.Checked = True And (Not objcamparam.Fr Is Nothing) Then
                                                                      'objcamparam.Fr.SetFramegrabberParam("gain_master", objLiveParam.gain_master)
                                                                      'objcamparam.Fr.SetFramegrabberParam("exposure", objLiveParam.exposure)
                                                                      objcamparam.Fr.SetFramegrabberParam("vertical_resolution", objLiveParam.ImageHeight)
                                                                      objcamparam.Fr.SetFramegrabberParam("horizontal_resolution", objLiveParam.ImageWidth)
                                                                      If objLiveParam.isAuto = 1 Then
                                                                          '20160607 SUSANO ADD START
                                                                          objcamparam.Fr.SetFramegrabberParam("gain_master", objLiveParam.gain_master)
                                                                          objcamparam.Fr.SetFramegrabberParam("exposure", objLiveParam.exposure)
                                                                          objcamparam.Fr.GrabImage()
                                                                          '20160607 SUSANO ADD END
                                                                          objcamparam.Fr.SetFramegrabberParam("exposure", "auto")
                                                                          objcamparam.Fr.SetFramegrabberParam("gain_master", "auto")
                                                                          'objcamparam.Fr.SetFramegrabberParam("gain_master", objLiveParam.gain_master) '20160727 Kiryu 変更

                                                                          If objLiveParam.frameRate > 0 Then
                                                                              objcamparam.Fr.SetFramegrabberParam("frame_rate", objLiveParam.frameRate)
                                                                          End If
                                                                      Else
                                                                          objcamparam.Fr.SetFramegrabberParam("gain_master", objLiveParam.gain_master)
                                                                          objcamparam.Fr.SetFramegrabberParam("exposure", objLiveParam.exposure)
                                                                      End If

                                                                  End If
                                                              Catch ex As Exception
                                                                  '  MsgBox(ex.Message)
                                                                  ' OutMessage("エラーメッセージ 「" & ex.Message & "」")
                                                              End Try

                                                          End Sub)

            'For Each objCamparam As CameraParam In lstCamParam
            '    If objCamparam.objCheckBox.Checked = True And (Not objCamparam.Fr Is Nothing) Then
            '        objCamparam.Fr.SetFramegrabberParam("gain_master", objLiveParam.gain_master)
            '        objCamparam.Fr.SetFramegrabberParam("exposure", objLiveParam.exposure)
            '        objCamparam.Fr.SetFramegrabberParam("vertical_resolution", objLiveParam.ImageHeight)
            '        objCamparam.Fr.SetFramegrabberParam("horizontal_resolution", objLiveParam.ImageWidth)
            '    End If
            'Next

            'Op.SetPart(AxHWindowXCtrl1.HalconWindow.HalconID, 0, 0, CInt(intH / objLiveParam.ImageHeight), CInt(intW / objLiveParam.ImageWidth))
            'Op.SetPart(AxHWindowXCtrl2.HalconWindow.HalconID, 0, 0, CInt(intH / objLiveParam.ImageHeight), CInt(intW / objLiveParam.ImageWidth))

            'Op.SetPart(AxHWindowXCtrl3.HalconWindow.HalconID, 0, 0, CInt(intH / objLiveParam.ImageHeight), CInt(intW / objLiveParam.ImageWidth))
            'Op.SetPart(AxHWindowXCtrl4.HalconWindow.HalconID, 0, 0, CInt(intH / objLiveParam.ImageHeight), CInt(intW / objLiveParam.ImageWidth))
            'Op.SetPart(AxHWindowXCtrl5.HalconWindow.HalconID, 0, 0, CInt(intH / objLiveParam.ImageHeight), CInt(intW / objLiveParam.ImageWidth))
            'Op.SetPart(AxHWindowXCtrl6.HalconWindow.HalconID, 0, 0, CInt(intH / objLiveParam.ImageHeight), CInt(intW / objLiveParam.ImageWidth))
            'Op.SetPart(AxHWindowXCtrl7.HalconWindow.HalconID, 0, 0, CInt(intH / objLiveParam.ImageHeight), CInt(intW / objLiveParam.ImageWidth))
            'Op.SetPart(AxHWindowXCtrl8.HalconWindow.HalconID, 0, 0, CInt(intH / objLiveParam.ImageHeight), CInt(intW / objLiveParam.ImageWidth))
            'Op.SetPart(AxHWindowXCtrl9.HalconWindow.HalconID, 0, 0, CInt(intH / objLiveParam.ImageHeight), CInt(intW / objLiveParam.ImageWidth))
            'Op.SetPart(AxHWindowXCtrl10.HalconWindow.HalconID, 0, 0, CInt(intH / objLiveParam.ImageHeight), CInt(intW / objLiveParam.ImageWidth))
            'Op.SetPart(AxHWindowXCtrl11.HalconWindow.HalconID, 0, 0, CInt(intH / objLiveParam.ImageHeight), CInt(intW / objLiveParam.ImageWidth))
            'Op.SetPart(AxHWindowXCtrl12.HalconWindow.HalconID, 0, 0, CInt(intH / objLiveParam.ImageHeight), CInt(intW / objLiveParam.ImageWidth))
#Else
            For Each objCamparam As CameraParam In lstCamParam
                If objCamparam.objCheckBox.Checked = True And (Not objCamparam.objAcqHandle Is Nothing) Then
                    Op.SetFramegrabberParam(objCamparam.objAcqHandle, "gain_master", objLiveParam.gain_master)
                    Op.SetFramegrabberParam(objCamparam.objAcqHandle, "exposure", objLiveParam.exposure)
                    Op.SetFramegrabberParam(objCamparam.objAcqHandle, "vertical_resolution", objLiveParam.ImageHeight)
                    Op.SetFramegrabberParam(objCamparam.objAcqHandle, "horizontal_resolution", objLiveParam.ImageWidth)
                End If
            Next
#End If

        Else
            GetFrameGrabberHandle()
        End If


        'Timer1.Start()
        TimersStart()

        DoveModeSet_Read()
        If DoveModeFlg >= 3 Then
            iRingProgress = 1
        End If

        '(20150915 Tezuka ADD) アクティブデータ取得タイマースタート
        If DoveModeFlg = 1 Then
            Timer_ActiveData.Interval = SenserSet.ActiveInterval * 1000
            Timer_ActiveData.Start()
        End If
        '(テスト用コード）　センサーの光軸数を取得する場合は下の一行を有効にしてください
        If DoveModeFlg >= 2 Then
            IncidentGetAndDisp(1)
        End If

        'センサーモードをストレートに戻す
        Dim Scan As Integer = 1
        Me.SerialPort_ScanTypeChange(Scan)

        OutMessage("撮影・測定　終了。")
        Me.Enabled = True
    End Sub

    '////////////////////////////////

    Public Sub KaisekiAndView()
        Dim fileExists As Boolean
        fileExists = My.Computer.FileSystem.FileExists(strNewKoujiFolder & "\計測データ.mdb")
        If fileExists = True Then
            My.Computer.FileSystem.DeleteFile(strNewKoujiFolder & "\計測データ.mdb", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
        End If
        ' OutMessage("解析処理開始。")
        System.IO.Directory.SetCurrentDirectory(My.Settings.YCMFolder)
        Shell(My.Settings.YCMFolder & "\YCM.exe " & strNewKoujiFolder & " 1", AppWinStyle.Hide, True)
        System.IO.Directory.SetCurrentDirectory(My.Application.Info.DirectoryPath)
        OutMessage("解析処理終了。")
        Dim clsSunpoData As New SunpoSetTable
        Dim lstSokuteiData As New List(Of SunpoSetTable)
        Dim flg As Boolean
        If clsSunpoData.m_dbClass Is Nothing Then
            clsSunpoData.m_dbClass = New CDBOperateOLE
        End If

        flg = clsSunpoData.m_dbClass.Connect(strNewKoujiFolder & "\計測データ.mdb")
        lstSokuteiData = clsSunpoData.GetDataToList()

        putDataTpDataGrids(lstSokuteiData)
        clsSunpoData.m_dbClass.DisConnect()

        '今回の計測結果の基準点の認識が100パーセントの場合に次回の基準点捜索のお手本データとする。２０１５０８０６
        Dim objMeasureInfo As New frmMeasureResult
        objMeasureInfo.KeikokuSaryou = My.Settings.AlartGosa
        objMeasureInfo.LoadInfo(strNewKoujiFolder)
        RTB_Alert.Text = objMeasureInfo.SokteiLogs
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\\Alert_Message.txt", vbNewLine & objMeasureInfo.SokteiLogs, True)

        Dim dblBasePointNinsikiRitu As Double = objMeasureInfo.BaseNinsikiHiritu
#If True Then
        If dblBasePointNinsikiRitu = 1 Then
            'strBeforeKoujiFolder = strNewKoujiFolder
            'My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\BasePointSearchKoujiDataPath.txt", strBeforeKoujiFolder, False)
            '   strBeforeKoujiFolder = My.Computer.FileSystem.ReadAllText(My.Application.Info.DirectoryPath & "\BasePointSearchKoujiDataPath.txt")
            strBeforeKoujiFolder = My.Application.Info.DirectoryPath & "\Setting\BasePointSearch"
            ConnectDbFBM(strBeforeKoujiFolder & "\")
            For Each objCam As CameraParam In lstCamParam
                For Each objCT As CodedTarget In objCam.objResultTarget.lstCT
                    objCT.UpdateData()
                Next
            Next
            AccessDisConnect()
        End If
#End If
        '今回の計測結果の基準点の認識が100パーセントの場合に次回の基準点捜索のお手本データとする。２０１５０８０６
        ' Me.txtNinshikiRitu.Text = Math.Round(objMeasureInfo.MeasureNinsikiHiritu * 100, 1)
        Me.txtNinshikiRitu.Text = objMeasureInfo.MeasureNinsikiHiritu 'SUSANO ADD 20151029
        Me.txtBaseNinsiki.Text = Math.Round(objMeasureInfo.BaseNinsikiHiritu * 100, 1)

        'SUSANO ADD START 20151012
        dbClass.Connect(strNewKoujiFolder & "\Pdata\dbFBM.mdb")
        For Each objCamparam As CameraParam In lstCamParam
            objCamparam.objResultTarget.lstCT.Clear()
            objCamparam.objResultTarget.ReadData("", objCamparam.Cid)
        Next
        dbClass.DisConnect()
        'SUSANO ADD END 20151012

    End Sub
    Private Sub GetBasePoint(ByRef lstBasePoint As List(Of measurepoint3dTable))

        Dim sysPath As String = My.Settings.YCMFolder & YCM_SYS_FLDR
        ' ConnectDbYCM(sysPath)
        ConnectDbSystemSetting(sysPath)
        Dim dReaderKijyun As System.Data.IDataReader
        If lstBasePoint Is Nothing Then
            lstBasePoint = New List(Of measurepoint3dTable)
        Else
            lstBasePoint.Clear()
        End If
        dReaderKijyun = dbClass.DoSelect("SELECT * FROM " & YCM_SYS_MDB_KIJYUNTBL & " order by CTID")
        While dReaderKijyun.Read()
            Dim strLabel As String
            Dim xx, yy, zz As Integer
            Dim intKijyunFlag As Integer
            intKijyunFlag = CInt(dReaderKijyun.GetValue(5))
            If intKijyunFlag = 1 Then
                strLabel = dReaderKijyun.GetValue(1).ToString
                If strLabel.IndexOf("_") = -1 Then          '(20150806 Tezuka ADD)
                    Dim strSys2 As String
                    strSys2 = Join(Split(strLabel, "_"), "")
                    xx = CDbl(dReaderKijyun.GetValue(2))
                    yy = CDbl(dReaderKijyun.GetValue(3))
                    zz = CDbl(dReaderKijyun.GetValue(4))
                    Dim objBasePoint As New measurepoint3dTable
                    objBasePoint.systemlabel = strLabel
                    objBasePoint.X = xx
                    objBasePoint.Y = yy
                    objBasePoint.Z = zz
                    lstBasePoint.Add(objBasePoint)
                    'If strSys1 = strSys2 Then
                    '    Dim objVal2(7) As Object
                    '    objVal2(0) = strLabel
                    '    objVal2(1) = "○"
                    '    objVal2(2) = Math.Round(xx, 1)
                    '    objVal2(3) = Math.Round(yy, 1)
                    '    objVal2(4) = Math.Round(zz, 1)
                    '    objVal2(5) = Math.Round(xx - X, 1)
                    '    objVal2(6) = Math.Round(yy - Y, 1)
                    '    objVal2(7) = Math.Round(zz - Z, 1)
                    '    Me.DataGridView1.Rows.Add(objVal2)
                    '    isExistFlag = True
                    'End If
                End If                                   '(20150806 Tezuka ADD)
            End If
        End While

        dReaderKijyun.Close()
        AccessDisConnect()

    End Sub
    Public Sub putDataTpDataGrids(ByVal lstSokuteiData As List(Of SunpoSetTable))
        Dim cntKeisan As Long
        Dim cntAlldata As Long

        '(20150731 Tezuka ADD) 車高の取得＆表に反映
        Dim Peak As Double = 0.0
        SerialPort_PeakGet(Peak)
        If Peak = 0.0 Then
            Peak = SensorPeak
        Else
            SensorPeak = Peak
        End If


        cntAlldata = lstSokuteiData.Count
        Me.dgMeasureResult.Rows.Clear()
        Me.dgAccuracyConf.Rows.Clear()
        For Each Sokutei As SunpoSetTable In lstSokuteiData
            If Sokutei.ZU_layer = "LINE" Then
                If Sokutei.SunpoName = "車高" Then
                    Dim objVal1(1) As Object
                    objVal1(0) = Sokutei.SunpoName
                    'objVal1(1) = Math.Round(CDbl(Peak) + 0.5, 0)
                    objVal1(1) = CDbl(Peak)
                    Me.dgMeasureResult.Rows.Add(objVal1)
                    If Sokutei.KiteiMin < Peak And Sokutei.KiteiMax > Peak Then
                    Else
                        Me.dgMeasureResult.Rows(Me.dgMeasureResult.Rows.Count - 2).DefaultCellStyle.BackColor = Color.Red
                    End If
                Else
                    Dim objVal1(1) As Object
                    objVal1(0) = Sokutei.SunpoName
                    'objVal1(1) = Math.Round(CDbl(Sokutei.SunpoVal) + 0.5, 0)
                    objVal1(1) = CDbl(Sokutei.SunpoVal)
                    Me.dgMeasureResult.Rows.Add(objVal1)
                    If Sokutei.flg_gouhi = "0" Then
                        Me.dgMeasureResult.Rows(Me.dgMeasureResult.Rows.Count - 2).DefaultCellStyle.BackColor = Color.Red
                    End If
                End If
            Else
                Dim objVal2(2) As Object
                objVal2(0) = Sokutei.SunpoName
                objVal2(1) = Math.Round(CDbl(Sokutei.KiteiVal) + 0.5, 0)
                objVal2(2) = Math.Round(CDbl(Sokutei.SunpoVal) - CDbl(Sokutei.KiteiVal), 0) 'SUURI 計測値から規定値を引く
                Me.dgAccuracyConf.Rows.Add(objVal2)
                If Sokutei.flg_gouhi = "0" Then
                    Me.dgAccuracyConf.Rows(Me.dgAccuracyConf.Rows.Count - 2).DefaultCellStyle.BackColor = Color.Red
                End If
            End If
            If Sokutei.flgKeisan = "1" Then
                cntKeisan += 1
            End If
        Next

      
        ''SUSANO ADD START 20151021
        'Dim flgJyojiSensor As Integer = CInt(ReadFrameGrabSetting(11))
        'If flgJyojiSensor = 1 Then
        '    Dim maxVal As Double = 0
        '    For Each dblPeak As Double In lstPeakValue
        '        If dblPeak > maxVal Then
        '            maxVal = dblPeak
        '        End If
        '    Next
        '    If Peak < maxVal Then
        '        Peak = maxVal
        '    End If
        'End If
        ''SUSANO ADD END 20151022
        'Dim objVal(1) As Object
        'objVal(0) = "車高"
        'objVal(1) = Math.Round(Peak, 1)
        'Me.dgMeasureResult.Rows.Add(objVal)
     
        Me.txtNinshikiRitu.Text = Math.Round(cntKeisan / cntAlldata * 100, 0)
    End Sub

    Private Sub btnDetailMeasureData_Click(sender As Object, e As EventArgs) Handles btnDetailMeasureData.Click
        'Timer1.Stop()
        '20151120 Kiryu コメントアウト
        'TimersStop()
        Dim frmObjMeasureResult As New frmMeasureResult

#If False Then
        strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data130826165926560955"
        strNewKoujiFolder = "C:\01_VFORM_Projects\SUEOKI\Data130828018318923997"

#End If
        frmObjMeasureResult.Tag = strNewKoujiFolder
        'frmObjMeasureResult.asdfasdfasdf(strNewKoujiFolder)
        'frmObjMeasureResult.BaseNinsikiHiritu
        frmObjMeasureResult.ShowDialog()
        ' Timer1.Start()
        TimersStart()
    End Sub

    Private Sub 設定ToolStripMenuItem_Click(sender As Object, e As EventArgs)
        'Timer1.Stop()
        'frmBasePointInfo.ShowDialog()
        frmBasePointInfo.Show()
        'Timer1.Start()


    End Sub

    Public Sub OutMessage(ByVal strMessageNoTime As String)
        '   Me.rtxtMessage.Text = Me.rtxtMessage.Text & vbNewLine
        Dim strTmpMes As String = Now.ToString & " " & strMessageNoTime & vbNewLine
        Me.rtxtMessage.Text = Me.rtxtMessage.Text & strTmpMes
        '   Me.rtxtMessage.Text = Me.rtxtMessage.Text & vbNewLine
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\MessageOutPut.txt", vbNewLine & strTmpMes, True)
        Me.rtxtMessage.SelectionStart = Me.rtxtMessage.Text.Length
        Me.rtxtMessage.Focus()
        Me.rtxtMessage.ScrollToCaret()
    End Sub
    Private Sub ReadAllShootedData(ByVal strKoujiFolder As String)
        Dim tmpFR As New HalconDotNet.HFramegrabber
#If DebugTest = True Then
        For Each objCamparam As CameraParam In lstCamParam
            If objCamparam.objCheckBox.Checked = True And (Not objCamparam.Fr Is Nothing) Then
                tmpFR = objCamparam.Fr
                Exit For
            End If
        Next
#End If
        For Each objCamparam As CameraParam In lstCamParam
#If DebugTest = True Then
            If objCamparam.objAcqHandle Is Nothing Then
                objCamparam.objAcqHandle = New Object
            End If
            If objCamparam.Fr Is Nothing Then
                objCamparam.Fr = tmpFR
            End If
#End If
            If objCamparam.objCheckBox.Checked = True And (Not objCamparam.objAcqHandle Is Nothing) Then

                objCamparam.objResultTarget.ReadData("", objCamparam.Cid)

                Dim StrJpegFileName As String
                StrJpegFileName = resultImageFileName & "_" & Format(objCamparam.Cid, "000") & "_C" & objCamparam.Cid.ToString & ".jpg"
                Try
                    Op.ReadImage(objCamparam.hvImageResult, strKoujiFolder & "\" & StrJpegFileName)
                    '#If DebugTest = True Then
                    '                    With objShootParam
                    '                        '露光時間をパラメータ
                    '                        Dim iRWimageCount As Integer = 0
                    '                        Dim objttt As New HalconDotNet.HImage
                    '                        ' objttt.ReadImage(strKoujiFolder & " \ " & StrJpegFileName)
                    '                        HalconDotNet.HOperatorSet.ReadImage(objttt, New HalconDotNet.HTuple(strKoujiFolder & " \ " & StrJpegFileName))
                    '                        For i = .exposure_min To .exposure_max Step .exposure_kankaku
                    '                            objCamparam.lstImagesDotNet.Add(objttt)
                    '                        Next
                    '                    End With
                    '#End If
                Catch ex As Exception
                    Debug.Print(ex.Message)
                End Try

            End If
        Next
    End Sub

    Private Sub DisConnectAllCamera()
        If lstCamParam Is Nothing Then
            Exit Sub
        End If
        For Each objCamparam As CameraParam In lstCamParam
            If objCamparam.objCheckBox.Checked = True And (Not objCamparam.Fr Is Nothing) Then
                Try
                    HalconDotNet.HOperatorSet.CloseFramegrabber(objCamparam.Fr)
                Catch ex As Exception
                    Debug.Print(ex.Message)
                End Try

            End If
        Next
    End Sub
    Private Sub cbShootParam_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbShootParam.SelectedIndexChanged
        Dim i As Integer = 0
        Dim selectedIndex As Integer = cbShootParam.SelectedIndex
        For Each objShootPar As ShootParam In lstShootParam

            If i = selectedIndex Then
                objShootPar.settei = True

            Else
                objShootPar.settei = False
            End If
            objShootPar.UpdateToDb()
            i += 1
        Next

    End Sub

    Private Sub AxHWindowXCtrl1_ClickEvent(sender As Object, e As EventArgs) Handles AxHWindowXCtrl1.ClickEvent
        PrevImageCid = 7
        DispKaisekiView() '20150820 Add Kiryu Live映像クリック時に解析画像を表示する
    End Sub

    Private Sub AxHWindowXCtrl2_ClickEvent(sender As Object, e As EventArgs) Handles AxHWindowXCtrl2.ClickEvent
        PrevImageCid = 8
        DispKaisekiView()
    End Sub

    Private Sub AxHWindowXCtrl3_ClickEvent(sender As Object, e As EventArgs) Handles AxHWindowXCtrl3.ClickEvent
        PrevImageCid = 9
        DispKaisekiView()
    End Sub

    Private Sub AxHWindowXCtrl4_ClickEvent(sender As Object, e As EventArgs) Handles AxHWindowXCtrl4.ClickEvent
        PrevImageCid = 10
        DispKaisekiView()
    End Sub

    Private Sub AxHWindowXCtrl4_Enter(sender As Object, e As EventArgs) Handles AxHWindowXCtrl4.Enter

    End Sub

    Private Sub AxHWindowXCtrl5_ClickEvent(sender As Object, e As EventArgs) Handles AxHWindowXCtrl5.ClickEvent
        PrevImageCid = 11
        DispKaisekiView()
    End Sub

    Private Sub AxHWindowXCtrl5_Enter(sender As Object, e As EventArgs) Handles AxHWindowXCtrl5.Enter

    End Sub

    Private Sub AxHWindowXCtrl6_ClickEvent(sender As Object, e As EventArgs) Handles AxHWindowXCtrl6.ClickEvent
        PrevImageCid = 12
        DispKaisekiView()
    End Sub

    Private Sub AxHWindowXCtrl6_Enter(sender As Object, e As EventArgs) Handles AxHWindowXCtrl6.Enter

    End Sub

    Private Sub AxHWindowXCtrl7_ClickEvent(sender As Object, e As EventArgs) Handles AxHWindowXCtrl7.ClickEvent
        PrevImageCid = 1
        DispKaisekiView()
    End Sub

    Private Sub AxHWindowXCtrl8_ClickEvent(sender As Object, e As EventArgs) Handles AxHWindowXCtrl8.ClickEvent
        PrevImageCid = 2
        DispKaisekiView()
    End Sub

    Private Sub AxHWindowXCtrl8_Enter(sender As Object, e As EventArgs) Handles AxHWindowXCtrl8.Enter

    End Sub

    Private Sub AxHWindowXCtrl9_ClickEvent(sender As Object, e As EventArgs) Handles AxHWindowXCtrl9.ClickEvent
        PrevImageCid = 3
        DispKaisekiView()
    End Sub

    Private Sub AxHWindowXCtrl9_Enter(sender As Object, e As EventArgs) Handles AxHWindowXCtrl9.Enter

    End Sub

    Private Sub AxHWindowXCtrl10_ClickEvent(sender As Object, e As EventArgs) Handles AxHWindowXCtrl10.ClickEvent
        PrevImageCid = 4
        DispKaisekiView()
    End Sub


    Private Sub AxHWindowXCtrl11_ClickEvent(sender As Object, e As EventArgs) Handles AxHWindowXCtrl11.ClickEvent
        PrevImageCid = 5
        DispKaisekiView()
    End Sub

    Private Sub AxHWindowXCtrl11_DockChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub AxHWindowXCtrl11_Enter(sender As Object, e As EventArgs) Handles AxHWindowXCtrl11.Enter

    End Sub

    Private Sub AxHWindowXCtrl12_ClickEvent(sender As Object, e As EventArgs) Handles AxHWindowXCtrl12.ClickEvent
        PrevImageCid = 6
        DispKaisekiView()
    End Sub

    Private Sub SplitContainer2_Panel2_Paint(sender As System.Object, e As System.Windows.Forms.PaintEventArgs) Handles SplitContainer2.Panel2.Paint

    End Sub

    Private Sub FrmMain_SizeChanged(sender As Object, e As System.EventArgs) Handles Me.SizeChanged

    End Sub

    Private Sub SC_Main_Resize(sender As System.Object, e As System.EventArgs) Handles SC_Main.Resize
        KeepAspectReSize()
    End Sub

    '(20150730 kiryu Add) -ハルコンウィンドウのアスペクト比を維持してリサイズ-
    Private Sub KeepAspectReSize()
        Dim Bairitu As Double = 0.715

        SC_Main.SplitterDistance = (Bairitu * AxHWindowXCtrl1.Size.Width + 15) * 2
        SplitContainer5.SplitterDistance = (Bairitu * AxHWinPrev.Size.Width)

    End Sub

    Private Sub btnSettei_Click(sender As System.Object, e As System.EventArgs) Handles btnSettei.Click
        'Timer1.Stop()
        ' frmBasePointInfo.ShowDialog()
        frmBasePointInfo.ShowDialog()
        ConnectDbFBM(My.Application.Info.DirectoryPath & "\")
        GetALLShootParam()
        cbShootParam.Items.Clear()
        Dim i As Integer = 0
        Dim selectedIndex As Integer = -1
        For Each objShootParam As ShootParam In lstShootParam
            If objShootParam.settei = True Then
                selectedIndex = i
            End If
            cbShootParam.Items.Add(objShootParam.setteiname)
            i += 1
        Next
        cbShootParam.SelectedIndex = selectedIndex
        AccessDisConnect()
        'Timer1.Start()
    End Sub


#Region "車高センサー関連" '(20150731 Tezuka ADD)

    '車高センサー設定情報クラス
    Public Class Sensser_setting
        Public strHead As String            '見出し文字列
        Public PortNumber As String         'ポート番号
        Public BaudRate As Integer          'ボーレート
        Public Parity As String             'パリティ
        Public DataBits As Integer          'データビット
        Public StopBits As Integer          'ストップビット
        Public SlaveAddress As Integer      'スレーブアドレス
        Public kijunHeight As Integer       '基準高さ
        Public SenserPitch As Double        'センサーピッチ
        Public PeakAddress As Integer       'ピーク測定値アドレス
        Public MonitorAddress As Integer    '監視用測定値アドレス(20151027 ADD)
        Public ErrcodeAddress As Integer    'エラーコードアドレス
        Public ServiceAddress As Integer    '稼働時間アドレス
        Public ScanAddress As Integer       'スキャンタイプアドレス
        Public ActiveAddress As Integer     'アクティブ計測値アドレス
        Public ActiveInterval As Double     'アクティブ計測値取得間隔（秒）
        Public CarExistInterval As Double   '車有り／無し取得間隔（秒）
        Public IncidentAddress As Integer   '入光状態光軸総数取得アドレス
        Public IncidentAll As Integer       '正常時の入光光軸数

        Public Sub New()

        End Sub

        Public Sub New(ByVal SettingFile As String)
            Dim strRead(2) As String
            Dim stPrm(50) As String
            Dim ii As Integer

            If System.IO.File.Exists(SettingFile) = True Then
                Dim fp As Integer = FreeFile()
                Try
                    'ファイルを読む
                    FileOpen(fp, SettingFile, OpenMode.Input)
                    strRead(0) = LineInput(fp)
                    strRead(1) = LineInput(fp)
                    FileClose(fp)

                    '行を","で区切る
                    Dim stArrayData As String() = strRead(1).Split(","c)
                    ii = 0
                    For Each stData As String In stArrayData
                        stPrm(ii) = stData
                        ii += 1
                    Next stData

                    '変数の格納する
                    strHead = strRead(0)
                    PortNumber = stPrm(0)
                    BaudRate = CInt(stPrm(1))
                    Parity = stPrm(2)
                    DataBits = CInt(stPrm(3))
                    StopBits = CInt(stPrm(4))
                    SlaveAddress = CInt(stPrm(5))
                    kijunHeight = CInt(stPrm(6))
                    SenserPitch = CDbl(stPrm(7))
                    PeakAddress = CInt(stPrm(8))
                    ErrcodeAddress = CInt(stPrm(9))
                    ServiceAddress = CInt(stPrm(10))
                    ScanAddress = CInt(stPrm(11))
                    If ii > 12 Then
                        MonitorAddress = CInt(stPrm(12))
                        ActiveInterval = CDbl(stPrm(13))
                        CarExistInterval = CDbl(stPrm(14))
                    Else
                        MonitorAddress = 522
                        ActiveInterval = 1.0
                        CarExistInterval = 10.0
                    End If
                    If ii > 15 Then
                        IncidentAddress = CInt(stPrm(15))
                    Else
                        IncidentAddress = 507
                    End If
                    If ii > 16 Then
                        IncidentAll = CInt(stPrm(16))
                    Else
                        IncidentAll = 480
                    End If
                Catch ex As Exception
                    FileClose(fp)
                End Try
            End If
        End Sub
    End Class

    'シリアルポートのオープン
    Public Sub SerialPort_Open()
        Dim SettingFile As String = My.Application.Info.DirectoryPath & "\Setting\Sensor_Setting.txt"
        Dim S_Sensor As New Sensser_setting(SettingFile)
        Dim Parity As Integer

        If S_Sensor.Parity = "None" Then
            Parity = 0
        ElseIf S_Sensor.Parity = "Odd" Then
            Parity = 1
        Else
            Parity = 2
        End If

        Dim PortName As String = "COM" & S_Sensor.PortNumber
        Dim sts As Integer = Ez_PortOpen(PortName, S_Sensor.BaudRate, S_Sensor.DataBits, S_Sensor.StopBits, Parity)
        If sts <> 0 Then
            SenserSet = Nothing
        Else
            SenserSet = New Sensser_setting(SettingFile)
        End If
    End Sub

    'ピーク値の取得
    Public Sub SerialPort_PeakGet(ByRef Peak As Double)
        Dim sMsg As String = ""

        If SenserSet Is Nothing Then
            sMsg = "シリアルポートがオープンされていません。"
            OutMessage(sMsg)
            Exit Sub
        End If

        Peak = 0.0
        Dim sts = Ez_PeakGet(SenserSet.SlaveAddress, SenserSet.kijunHeight, SenserSet.SenserPitch, SenserSet.PeakAddress, SenserSet.IncidentAll, Peak)

    End Sub

    '監視用測定値の取得
    Public Sub SerialPort_MonitorValueGet(ByRef MValue As Double)
        Dim sMsg As String = ""

        If SenserSet Is Nothing Then
            sMsg = "シリアルポートがオープンされていません。"
            OutMessage(sMsg)
            Exit Sub
        End If

        MValue = 0.0
        Dim sts = Ez_MonitorValueGet(SenserSet.SlaveAddress, SenserSet.kijunHeight, SenserSet.SenserPitch, SenserSet.MonitorAddress, SenserSet.IncidentAll, MValue)

    End Sub

    'アクティブ計測値の取得(20150915 Tezuka ADD)
    Public Sub SerialPort_ActiveDataGet(ByRef ActiveData As Double)
        Dim sMsg As String = ""

        If SenserSet Is Nothing Then
            sMsg = "シリアルポートがオープンされていません。"
            OutMessage(sMsg)
            Exit Sub
        End If

        ActiveData = 0.0
        Dim sts = Ez_ActiveDataGet(SenserSet.SlaveAddress, SenserSet.kijunHeight, SenserSet.SenserPitch, SenserSet.ActiveAddress, ActiveData)

    End Sub

    'アクティブ計測値の取得(20150915 Tezuka ADD)
    Public Sub SerialPort_IncidentGet(ByRef Incident As Integer)
        Dim sMsg As String = ""

        If SenserSet Is Nothing Then
            sMsg = "シリアルポートがオープンされていません。"
            OutMessage(sMsg)
            Exit Sub
        End If

        Incident = 0
        Dim sts = Ez_IncidentGet(SenserSet.SlaveAddress, SenserSet.IncidentAddress, Incident)

    End Sub

    'スキャンタイプ取得
    Public Sub SerialPort_ScanTypeGet(ByRef ScanType As Integer)
        Dim sMsg As String = ""

        If SenserSet Is Nothing Then
            sMsg = "シリアルポートがオープンされていません。"
            OutMessage(sMsg)
            Exit Sub
        End If

        ScanType = 0
        Dim sts = Ez_ScanTypeGet(SenserSet.SlaveAddress, ScanType)

    End Sub

    'スキャンタイプの変更
    Public Sub SerialPort_ScanTypeChange(ByVal ScanType As Integer)
        Dim sMsg As String = ""

        If SenserSet Is Nothing Then
            sMsg = "シリアルポートがオープンされていません。"
            OutMessage(sMsg)
            Exit Sub
        End If

        If (ScanType < 1) Or (ScanType > 2) Then
            sMsg = "スキャンタイプは、1（ストレート）か2（シングル）を指定してください。"
            OutMessage(sMsg)
            Exit Sub
        End If

        Dim sts = Ez_ScanTypeChange(SenserSet.SlaveAddress, ScanType)

    End Sub

    'Ez_Arrayエラーコード取得
    Public Sub SerialPort_ErrCodeGet(ByRef ErrCode As Integer)
        Dim sMsg As String = ""

        If SenserSet Is Nothing Then
            sMsg = "シリアルポートがオープンされていません。"
            OutMessage(sMsg)
            Exit Sub
        End If

        ErrCode = 0
        Dim sts = Ez_StatusGet(SenserSet.SlaveAddress, SenserSet.ErrcodeAddress, ErrCode)

    End Sub

    '稼働時間取得
    Public Sub SerialPort_ServiceTimeGet(ByRef ServiceTime As String)
        Dim sMsg As String = ""

        If SenserSet Is Nothing Then
            sMsg = "シリアルポートがオープンされていません。"
            OutMessage(sMsg)
            Exit Sub
        End If

        ServiceTime = ""
        Dim sts = Ez_ServiceTimeGet(SenserSet.SlaveAddress, SenserSet.ServiceAddress, ServiceTime)

    End Sub

    'シリアルポートクローズ
    Public Sub SerialPort_Close()
        Dim sMsg As String = ""

        If SenserSet Is Nothing Then
            Exit Sub
        End If

        Dim sts = Ez_PortClose()
        If sts = 0 Then
            SenserSet = Nothing
        End If
    End Sub

    'セッティングファイル書き込み
    Public Sub SerialPort_SetFileWrite()
        Dim FileNm As String = My.Application.Info.DirectoryPath & "\Setting\Sensor_Setting.txt"
        Dim fp As Integer = FreeFile()

        Dim strWrt As String = ""
        strWrt = SenserSet.PortNumber & "," & SenserSet.BaudRate.ToString & "," & SenserSet.Parity.ToString & "," & _
                 SenserSet.DataBits.ToString & "," & SenserSet.StopBits.ToString & "," & SenserSet.SlaveAddress & "," & _
                 SenserSet.kijunHeight.ToString & "," & SenserSet.SenserPitch & "," & SenserSet.PeakAddress & "," & _
                 SenserSet.ErrcodeAddress & "," & SenserSet.ServiceAddress & "," & SenserSet.ScanAddress & "," & _
                 SenserSet.MonitorAddress & "," & SenserSet.ActiveInterval & "," & SenserSet.CarExistInterval & "," & _
                 SenserSet.IncidentAddress & "," & SenserSet.IncidentAll
        Try
            FileOpen(fp, FileNm, OpenMode.Output)
            PrintLine(fp, SenserSet.strHead)
            PrintLine(fp, strWrt)
            FileClose(fp)
        Catch ex As Exception
            FileClose(fp)
        End Try
    End Sub
#End Region

    '(20150731 Tezuka ADD) フォームが閉じられる時に、シリアルポートをクローズする
    Private Sub FrmMain_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        DisConnectAllCamera()
        'SerialPort_SetFileWrite()
        SerialPort_Close()
    End Sub

    '(20150731 Tezuka ADD) スキャンタイプをシングルの変更する。
    'Private Sub rbSensorSingle_CheckedChanged(sender As System.Object, e As System.EventArgs)
    '    Dim Scan As Integer
    '    If rbSensorSingle.Checked = True Then
    '        Scan = 2
    '        SerialPort_ScanTypeChange(Scan)
    '    End If
    'End Sub

    '(20150731 Tezuka ADD) スキャンタイプをストレートに変更する
    'Private Sub rbSensorStraight_CheckedChanged(sender As System.Object, e As System.EventArgs)
    '    Dim Scan As Integer
    '    If rbSensorStraight.Checked = True Then
    '        Scan = 1
    '        SerialPort_ScanTypeChange(Scan)
    '    End If
    'End Sub

    '( 20150805 Kiryu Add ) 解析画面のボタン操作( 拡大縮小 )
    'ZoomBtn : 0より大きい=拡大,0以下=縮小
    '返り値:   正常=1,異常=0
    Private Function BtnReSizePart(ByVal ZoomBtn)
        Dim R As Object = Nothing, C As Object = Nothing
        Dim Rw1 As Object = Nothing, Cw1 As Object = Nothing
        Dim Rw2 As Object = Nothing, Cw2 As Object = Nothing
        Dim Rd1 As Object = Nothing, Cd1 As Object = Nothing
        Dim Rd2 As Object = Nothing, Cd2 As Object = Nothing

        Dim ZF As Double
        Try

            Op.GetPart(hvWinPrev, Rw1, Cw1, Rw2, Cw2)
            C = Center_x
            R = Center_y

            If (ZoomBtn > 0) Then
                ZF = 1 / ZoomFactor
            Else
                ZF = ZoomFactor
            End If

            Cd1 = C - (C - Cw1) * ZF
            Cd2 = C + (Cw2 - C) * ZF
            Rd1 = R - (R - Rw1) * ZF
            Rd2 = Rd1 + CInt(Math.Abs(Cd2 - Cd1) * (intH / intW))

            If (Rd1 < 0) Then Rd1 = 0
            If (Cd1 < 0) Then Cd1 = 0
            If (Rd2 > intH) Then Rd2 = intH '-ZH
            If (Cd2 > intW) Then Cd2 = intW '-ZW
            If (Math.Abs(Rd1 - Rd2) > minZoomSize And Math.Abs(Cd1 - Cd2) > minZoomSize) Then
                '--表示領域の再設定

                Op.SetPart(hvWinPrev, Rd1, Cd1, Rd2, Cd2)
            End If

            ' Op.DispObj(hImage, win)
            BtnReSizePart = True

        Catch ex As Exception
            BtnReSizePart = False
        End Try

    End Function

    '( 20150820 Kiryu Add ) 解析画面のボタン操作( 移動 )
    'Integer HorizotalCnt   : 水平移動量[pixel]
    'Integer VerticalCnt    : 垂直移動量[pixel]
    '返り値:   正常=1,異常=0
    Private Function MovePart(ByVal HorizontalCnt, ByVal VerticalCnt)
        Dim R As Object = Nothing, C As Object = Nothing
        Dim Rw1 As Object = Nothing, Cw1 As Object = Nothing
        Dim Rw2 As Object = Nothing, Cw2 As Object = Nothing
        Dim Rd1 As Object = Nothing, Cd1 As Object = Nothing
        Dim Rd2 As Object = Nothing, Cd2 As Object = Nothing

        Try
            Op.GetPart(hvWinPrev, Rw1, Cw1, Rw2, Cw2)
            C = Center_x
            R = Center_y
            Center_x = C + HorizontalCnt
            Center_y = R + VerticalCnt

            Cd1 = Cw1 + HorizontalCnt
            Cd2 = Cw2 + HorizontalCnt
            Rd1 = Rw1 + VerticalCnt
            Rd2 = Rw2 + VerticalCnt

            If (Rd1 < 0 Or Rd2 > intH) Then
                Rd1 = Rw1
                Rd2 = Rw2
                Center_y = R
            End If

            If (Cd1 < 0 Or Cd2 > intW) Then
                Cd1 = Cw1
                Cd2 = Cw2
                Center_x = C
            End If

            If (Math.Abs(Rd1 - Rd2) > minZoomSize And Math.Abs(Cd1 - Cd2) > minZoomSize) Then
                '--表示領域の再設定

                Op.SetPart(hvWinPrev, Rd1, Cd1, Rd2, Cd2)
            End If

            ' Op.DispObj(hImage, win)
            MovePart = True

        Catch ex As Exception
            MovePart = False
        End Try

    End Function

    '(20150820 Kiryu Add) 解析画像の表示
    Private Sub DispKaisekiView()
        Dim objfirstCam As CameraParam = Nothing

        For Each objCamparam As CameraParam In lstCamParam
            '' If objCamparam.objCheckBox.Checked = True And (Not objCamparam.Fr Is Nothing) Then
            If PrevImageCid = objCamparam.Cid Then
                objfirstCam = objCamparam
                Exit For
            End If
            ''  End If
        Next
        If objfirstCam Is Nothing Then
            Exit Sub
        End If
        If Not objfirstCam.hvImageResult Is Nothing Then
            Op.DispImage(objfirstCam.hvImageResult, hvWinPrev)
            DispCT(hvWinPrev, objfirstCam.objResultTarget)
        End If

    End Sub

    Private Sub btnZoomIn_Click(sender As System.Object, e As System.EventArgs) Handles btnZoomIn.Click
        BtnReSizePart(1)
        DispKaisekiView()
    End Sub

    Private Sub btnZoomOut_Click(sender As System.Object, e As System.EventArgs) Handles btnZoomOut.Click
        BtnReSizePart(-1)
        DispKaisekiView()
    End Sub

    Private Sub btnLeft_Click(sender As System.Object, e As System.EventArgs) Handles btnLeft.Click
        MovePart(-MoveVal, 0)
        DispKaisekiView()
    End Sub

    Private Sub btnRight_Click(sender As System.Object, e As System.EventArgs) Handles btnRight.Click
        MovePart(MoveVal, 0)
        DispKaisekiView()
    End Sub

    Private Sub btnUp_Click(sender As System.Object, e As System.EventArgs) Handles btnUp.Click
        MovePart(0, -MoveVal)
        DispKaisekiView()
    End Sub

    Private Sub btnDown_Click(sender As System.Object, e As System.EventArgs) Handles btnDown.Click
        MovePart(0, MoveVal)
        DispKaisekiView()
    End Sub


    Private Sub Timer2_Tick(sender As System.Object, e As System.EventArgs) Handles Timer2.Tick
        LiveImageOneCamera(1)
    End Sub

    Private Sub Timer3_Tick(sender As System.Object, e As System.EventArgs) Handles Timer3.Tick
        LiveImageOneCamera(2)
    End Sub

    Private Sub Timer4_Tick(sender As System.Object, e As System.EventArgs) Handles Timer4.Tick
        LiveImageOneCamera(3)
    End Sub

    Private Sub Timer5_Tick(sender As System.Object, e As System.EventArgs) Handles Timer5.Tick
        LiveImageOneCamera(4)
    End Sub

    Private Sub Timer6_Tick(sender As System.Object, e As System.EventArgs) Handles Timer6.Tick
        LiveImageOneCamera(5)
    End Sub

    Private Sub Timer7_Tick(sender As System.Object, e As System.EventArgs) Handles Timer7.Tick
        LiveImageOneCamera(6)
    End Sub

    Private Sub Timer8_Tick(sender As System.Object, e As System.EventArgs) Handles Timer8.Tick
        LiveImageOneCamera(7)
    End Sub

    Private Sub Timer9_Tick(sender As System.Object, e As System.EventArgs) Handles Timer9.Tick
        LiveImageOneCamera(8)
    End Sub

    Private Sub Timer10_Tick(sender As System.Object, e As System.EventArgs) Handles Timer10.Tick
        LiveImageOneCamera(9)
    End Sub

    Private Sub Timer11_Tick(sender As System.Object, e As System.EventArgs) Handles Timer11.Tick
        LiveImageOneCamera(10)
    End Sub

    Private Sub Timer12_Tick(sender As System.Object, e As System.EventArgs) Handles Timer12.Tick
        LiveImageOneCamera(11)
    End Sub

    Private Sub AxHWinPrev_MouseDownEvent(sender As Object, e As AxHALCONXLib._HWindowXCtrlEvents_MouseDownEvent) Handles AxHWinPrev.MouseDownEvent
        Dim objfirstCam As CameraParam = Nothing
        If e.nShiftState = 12 Then


            For Each objCamparam As CameraParam In lstCamParam
                If objCamparam.objCheckBox.Checked = True And (Not objCamparam.Fr Is Nothing) Then
                    If PrevImageCid = objCamparam.Cid Then
                        objfirstCam = objCamparam
                        Exit For
                    End If
                End If
            Next
        End If
        If objfirstCam Is Nothing Then
            Exit Sub
        End If
        Try
            Dim R As New Object, C As New Object
            Dim mBtn As New Object
            ' HalconDotNet.HOperatorSet.GetMposition(New HalconDotNet.HTuple(hvWinPrev), R, C, mBtn)
            Op.GetMposition(hvWinPrev, R, C, mBtn)

            Dim objDist As Object = Nothing
            Dim minVal As Double = Double.MaxValue
            Dim tmpSt As SingleTarget = Nothing
            If mBtn = 28 Then

                For Each ST As SingleTarget In objfirstCam.objResultTarget.lstST
                    ST.P2D.CalcDistToInputPoint(R, C, objDist)
                    If objDist < minVal Then
                        minVal = objDist
                        tmpSt = ST
                    End If
                Next
                Dim frmSTtoCT As New STtoCTpoint
                frmSTtoCT.SelectedST = tmpSt
                frmSTtoCT.ShowDialog()

                If frmSTtoCT.SelectedCT <> "" And IsNumeric(frmSTtoCT.SelectedCT) Then
                    ConnectDbFBM(strNewKoujiFolder & "\")
                    objfirstCam.objResultTarget.OneSTtoCTpoint(tmpSt, frmSTtoCT.SelectedCT)
                    Dim NewCT As New CodedTarget
                    NewCT.CT_ID = CInt(frmSTtoCT.SelectedCT)
                    NewCT.ImageID1 = objfirstCam.Cid
                    'Dim oneSTtoCT As New ImagePoints
                    'oneSTtoCT.Row = Tuple.TupleConcat(Tuple.TupleConcat(objMinKK_ST.P2D.Row, objMinKK_ST.P2D.Row), Tuple.TupleConcat(objMinKK_ST.P2D.Row, objMinKK_ST.P2D.Row))
                    'oneSTtoCT.Col = Tuple.TupleConcat(Tuple.TupleConcat(objMinKK_ST.P2D.Col, objMinKK_ST.P2D.Col), Tuple.TupleConcat(objMinKK_ST.P2D.Col, objMinKK_ST.P2D.Col))

                    NewCT.CT_Points.Row = Tuple.TupleConcat(Tuple.TupleConcat(tmpSt.P2D.Row, tmpSt.P2D.Row), Tuple.TupleConcat(tmpSt.P2D.Row, tmpSt.P2D.Row))
                    NewCT.CT_Points.Col = Tuple.TupleConcat(Tuple.TupleConcat(tmpSt.P2D.Col, tmpSt.P2D.Col), Tuple.TupleConcat(tmpSt.P2D.Col, tmpSt.P2D.Col))
                    NewCT.AllST_Area = tmpSt.AreaST
                    NewCT.SetlstCTtoST()
                    objfirstCam.objResultTarget.lstCT.Add(NewCT)

                    objfirstCam.objResultTarget.DeleteData(objfirstCam.Cid, False) 'Targetsテーブル

                    'レコード登録
                    objfirstCam.objResultTarget.SaveData() 'Targetsテーブル
                    AccessDisConnect()

                End If
                If frmSTtoCT.KaisekiRun = True Then
                    KaisekiAndView()
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub


    Public Sub LoadShootParam()

        GetALLShootParam()
        cbShootParam.Items.Clear()
        Dim i As Integer = 0
        Dim selectedIndex As Integer = -1
        For Each objShootParam As ShootParam In lstShootParam
            If objShootParam.settei = True Then
                selectedIndex = i
            End If
            cbShootParam.Items.Add(objShootParam.setteiname)
            i += 1
        Next
        cbShootParam.SelectedIndex = selectedIndex

    End Sub
    Dim flgBaseImageReset1 As Boolean = False
    Dim flgBaseImageReset2 As Boolean = False
    Dim flgBtnNoCar As Boolean = False

    Private Sub btnNoCar_Click(sender As Object, e As EventArgs) Handles btnNoCar.Click

        flgBaseImageReset1 = True
        flgBaseImageReset2 = True
        '   flgBtnNoCar = True

        OutMessage("車がいない")
        'センサーをリセット
        Dim Peak As Double = 0.0
        SerialPort_PeakGet(Peak)
        OutMessage("エリアセンサーをリセットしました。リセット時のピーク値＝" & Peak)
        'flgBaseImageReset1 = False
        'flgBaseImageReset2 = False
    End Sub

    Private Sub MirrorImage(ByVal objCamParam As CameraParam, ByRef objDispImage As HalconDotNet.HImage)
        'If objCamParam.Cid >= 7 And objCamParam.Cid <= 12 Then
        If False Then    '20170713 SUURI TEST 用
            objDispImage = objDispImage.MirrorImage("row")
            objDispImage = objDispImage.MirrorImage("column")
        End If
    End Sub

    Private Sub Timer_ActiveData_Tick(sender As System.Object, e As System.EventArgs) Handles Timer_ActiveData.Tick
        Dim ActiveHeight As Double = 0.0

        SerialPort_ActiveDataGet(ActiveHeight)
        If Math.Abs(ActiveHeight) > 0 Then
            rtxtMessage.SelectionColor = Color.Red
            rtxtMessage.SelectedText = "!!! 何かが進入した模様：" & SenserSet.CarExistInterval.ToString & "秒間待機します。!!!" & vbCrLf
            'rtxtMessage.SelectionColor = Color.Black
            Timer_ActiveData.Stop()
            System.Threading.Thread.Sleep(SenserSet.CarExistInterval * 1000)
            Dim flg As Boolean = ActiveTrackExists()
            If flg = False Then
                'センサーをリセット
                Dim Peak As Double = 0.0
                SerialPort_PeakGet(Peak)
                rtxtMessage.SelectionColor = Color.Green
                rtxtMessage.SelectedText = "<<< 車以外が進入しました。ピーク値をリセットしました。" & vbCrLf
                'rtxtMessage.SelectionColor = Color.Black
                Timer_ActiveData.Interval = SenserSet.ActiveInterval * 1000
                Timer_ActiveData.Start()
            Else
                rtxtMessage.SelectionColor = Color.Blue
                rtxtMessage.SelectedText = ">>> 車が進入しました。計測まで待機します。" & vbCrLf
                'rtxtMessage.SelectionColor = Color.Black
            End If
        End If
    End Sub

    '(20150915 Tezuka ADD) 車有り／無しチェック
    Private Function ActiveTrackExists() As Boolean
        ActiveTrackExists = False

        Dim fileContents As String
        fileContents = My.Computer.FileSystem.ReadAllText(My.Application.Info.DirectoryPath & "\CarDetectCamera.txt")

        If lstCamParam IsNot Nothing Then
            For Each objCamparam As CameraParam In lstCamParam
                If objCamparam.Cid = fileContents.Split(",")(0) Or objCamparam.Cid = fileContents.Split(",")(1) Then
                    If objCamparam.objCheckBox.Checked = True And (Not objCamparam.Fr Is Nothing) Then
                        Try
                            Dim objDispImage As HalconDotNet.HImage = objCamparam.Fr.GrabImage
                            MirrorImage(objCamparam, objDispImage)
                            HalconDotNet.HOperatorSet.DispObj(objDispImage, New HalconDotNet.HTuple(objCamparam.hwindhand))
                            Dim flg As Boolean = False
                            If objCamparam.Cid = fileContents.Split(",")(0) Then
                                flg = TrackCheck1(objCamparam, objDispImage)
                            ElseIf objCamparam.Cid = fileContents.Split(",")(1) Then
                                flg = TrackCheck2(objCamparam, objDispImage)
                            End If
                            If flg = True Then
                                ActiveTrackExists = True
                                Exit Function
                            End If
                        Catch ex As Exception
                            OutMessage("アクティブデータ取得エラー：ActiveTrackExists:" & ex.Message)
                            Exit Function
                        End Try
                    End If
                End If
            Next
        End If
    End Function

    '(20150916 Tezuka ADD) 入光状態光軸総数取得＆画面表示
    Private Sub IncidentGetAndDisp(ByVal mode As Integer)
        Dim Incident As Integer

        If mode = 0 Then
            Label1.ForeColor = Color.Blue
            Label1.Text = "測定前"
            TextBox1.Text = "( --- / " & SenserSet.IncidentAll.ToString & " )"
        Else
            SerialPort_IncidentGet(Incident)
            If Incident = SenserSet.IncidentAll Then
                Label1.ForeColor = Color.Black
                Label1.Text = "正常"
            Else
                Label1.ForeColor = Color.Red
                Label1.Text = "!!!異常!!!"
            End If

            TextBox1.Text = "( " & Incident.ToString & " / " & SenserSet.IncidentAll.ToString & " )"
        End If
    End Sub

    '(20150916 Tezuka ADD) DoveModeSet.txtファイルを読み、センサー常時監視モードをセットする
    Private Sub DoveModeSet_Read()
        Dim fp As Integer = FreeFile()
        Dim Fname As String = My.Application.Info.DirectoryPath & "\DoveModeSet.txt"
        Dim sw As String
        Dim iFlg As Integer

        iFlg = 0
        Try
            FileOpen(fp, Fname, OpenMode.Input)
            sw = LineInput(fp)
            Input(fp, iFlg)
            FileClose(fp)
        Catch ex As Exception
            FileClose(fp)
        End Try

        DoveModeFlg = iFlg
    End Sub

    Private Sub Thead_RingProgress()
        Dim frm As New RingProgress

        frm.Show()
        frm.BringToFront()
        While (1)
            If iRingProgress = 1 Then
                Exit While
            Else
                Application.DoEvents()
            End If
        End While
        frm.Close()

    End Sub
  
    Private Sub TimerForSensorReset_Tick(sender As Object, e As EventArgs) Handles TimerForSensorReset.Tick

        Dim intNowCompare As Integer = CDate(TimerForSensorReset.Tag).AddSeconds(CDbl(ReadFrameGrabSetting(6) / 2)).CompareTo(Now)
        If intNowCompare = -1 Then
            'センサーをリセット
            Dim Peak As Double = 0.0
            SerialPort_PeakGet(Peak)
            OutMessage("エリアセンサーをリセットしました。リセット時のピーク値＝" & Peak)
            '522もリセットする　20151029
            Dim MVal As Double = 0.0
            SerialPort_MonitorValueGet(MVal)
            TimerForSensorReset.Enabled = False
        End If

    End Sub

End Class
