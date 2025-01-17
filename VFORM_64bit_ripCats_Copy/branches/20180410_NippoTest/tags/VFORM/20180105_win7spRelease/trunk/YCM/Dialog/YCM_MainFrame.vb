﻿Imports System.Runtime.InteropServices
Imports System
Imports System.Windows.Forms
Imports System.Drawing
Imports FBMlib
Imports HalconDotNet

#Const m_IconMenu = 0
Public Class YCM_MainFrame
#Region "Main"
    Private m_bDataPointShow As Boolean = False
    Delegate Sub SetText(ByVal text As String)
    Delegate Sub SetHistori(ByVal text As String)
    Delegate Sub SetImageWindow() '20170216 baluu add
    Delegate Function SetReconstructScene(ByRef SettingsData As SettingsTable, ByVal images As List(Of ImageSet)) As Boolean '20170224 baluu add '20170302 baluu edit
    Delegate Function SetUnionScene(ByVal scenePath As String) As Boolean '20170302 baluu add
    Delegate Sub SetUpdateDetectCT(ByVal sender As Object, ByVal E As MessageEventArgs)
    Public rtri As Single
    Public rquad As Single
    Dim arrlist As New ArrayList
    Dim FormTitle As String = "VFORM" 'H25.4.25 Yamada ->20130525 SUURI 変更
    Dim Ver As String = "Ver.1.0" 'H25.4.25 Yamada
    Dim iWin As Integer = 0
    Dim iBtn As Integer = 0
    '--12.11.23    Dim bIsFirstLoad As Boolean  '=True：FormLoad
    Dim LabelFont As String = "-メイリオ-"  'Add Kiryu 20161003 ２次元画像 ラベルフォント変更用
    Dim FontSize As Integer = 12 '          'Add Kiryu 20161003 ２次元画像 フォントサイズ変更用
    Const FontPich As Integer = 2
    Dim ILVCheckedFinishFlg As Boolean = False


    Public Sub Btn_text(ByVal str As String)
        Dim dh As SetText = New SetText(AddressOf Btn_SetText)
        Me.Invoke(dh, New Object() {str})
    End Sub
    Private Sub Btn_SetText(ByVal str As String)
        Me.TBox_Data.Text = str
        Me.TBox_Data.SelectionStart = Len(TBox_Data.Text)
    End Sub
    Public Sub Histori_text(ByVal str As String)
        Dim dh As SetHistori = New SetHistori(AddressOf Btn_SetHistoriText)
        Me.Invoke(dh, New Object() {str})
    End Sub
    Private Sub Btn_SetHistoriText(ByVal str As String)
        Me.LBox_Data.Items.Add(str)
        LBox_Data.SelectedIndex = LBox_Data.Items.Count - 1
        'Me.TBox_Data.Text = str
    End Sub

    '20170216 baluu add start
    Public Sub ResetImageWindow()
        Dim dh As SetImageWindow = New SetImageWindow(AddressOf AllDraw)
        Me.Invoke(dh, New Object() {})
    End Sub
    '20170216 baluu add end

    '20170224 baluu add start '20170302 baluu edit start
    Public Function StartReconstructScene(ByRef SettingsData As SettingsTable, ByVal images As List(Of ImageSet)) As Boolean
        'Dim dh As SetReconstructScene = New SetReconstructScene(AddressOf ReconstructScene)
        'StartReconstructScene = Me.Invoke(dh, New Object() {SettingsData, images})
        StartReconstructScene = ReconstructScene(SettingsData, images)
    End Function
    '20170224 baluu add end '20170302 baluu edit end
    '20170302 baluu add start
    Public Function StartUnionScene(ByVal scenePath As String) As Boolean
        Dim dh As SetUnionScene = New SetUnionScene(AddressOf UnionScene)
        StartUnionScene = Me.Invoke(dh, New Object() {scenePath})
    End Function
    '20170302 baluu add end
    Public Sub StartUpdateDetectCT(ByVal sender As Object, ByVal E As MessageEventArgs)
        Dim dh As SetUpdateDetectCT = New SetUpdateDetectCT(AddressOf FBMLIB_DetectCT)
        Me.Invoke(dh, New Object() {sender, E})
    End Sub

    '--rep.12.10.17    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Private Sub YCM_MainFrame_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Show()

    End Sub
    Public Sub Show()
#If False Then
             If LicenseCheck() = False Then
            Me.Close()
            Exit Sub
        End If
        'g_InputIsave = True
        Data_Point.TopLevel = False
        Data_Point.Dock = DockStyle.Fill 'クラスではなくオブジェクトに命令を

        Me.SplitContainer3.Panel2.Controls.Add(Data_Point)
        Data_Point.Show()
        m_bDataPointShow = True

        View_Dilaog.TopLevel = False
        View_Dilaog.Dock = DockStyle.Fill
        Me.SplitContainer4.Panel1.Controls.Add(View_Dilaog)
        View_Dilaog.Show()

        IOUtil = New IOLib(Me.TBox_Data, Me.LBox_Data)
        MainFrm = Me
        Me.TBox_Data.SelectionStart = Len(TBox_Data.Text)



        '--画面メニュー状態

        '--del.rep.start-------------------------12.10.17
        ' リボンメニューに表示／非表示の状態を反映
#If 0 Then
        ''MM_V_12.Checked = True
#End If
        '--del.rep.end---------------------------12.10.17
        '--ins.start-----------------------------12.10.17
        SplitContainer1.SplitterDistance = 135  '--ElementHost1.Height + 2
        SplitContainer1.VerticalScroll.Visible = False
        SplitContainer1.IsSplitterFixed = True
        SplitContainer3.IsSplitterFixed = True
        '--ins.end-------------------------------12.10.17

        sys_CoordInfo.mat = YCM_GetUnitMat()

        '画像タブ

        objFBM = New FBMlib.FeatureImage
        winpreview = AxHWindowXCtrl6.HalconID
        win1 = AxHWindowXCtrl2.HalconID
        win2 = AxHWindowXCtrl3.HalconID
        win3 = AxHWindowXCtrl4.HalconID
        win4 = AxHWindowXCtrl5.HalconID
        flgDispVal = flgDisp.flgCT
        HOperatorSet.SetDraw(winpreview, "margin")
        HOperatorSet.SetDraw(win1, "margin")
        HOperatorSet.SetDraw(win2, "margin")
        HOperatorSet.SetDraw(win3, "margin")
        HOperatorSet.SetDraw(win4, "margin")
        strLastProjectPath = My.Settings.strLastProjPath

        '座標値一覧を非表示にして表示
        MainFrm.SplitContainer3.SplitterDistance = MainFrm.SplitContainer2.Panel2.Width - 10
        'YCM_3DView.Size = New System.Drawing.Size(MainFrm.SplitContainer4.Panel1.Width, MainFrm.SplitContainer4.Panel1.Height)
        '   YCM_3DView.Dock = DockStyle.Fill
        Me.KeyPreview = True
        'RibbonMenuControl1.BackgroundProperty = Windows.SystemColors.ControlColor（13.1.22山本さん質問：リボンにwindowsシステム?の設定のカラーを設定したい⇒PPT）

        'Me.ElementHost1.BackColor = Color.Red

        'RibbonMenuControl14. = Common.FormBackColor
#End If

        If LicenseCheck() = False Then
            Me.Close()
            Exit Sub
        End If
        'g_InputIsave = True
        Data_Point = New Data_Vertex
        View_Dilaog = New YCM_3DView

        Data_Point.TopLevel = False
        Data_Point.Dock = DockStyle.Fill 'クラスではなくオブジェクトに命令を

        Me.SplitContainer3.Panel2.Controls.Add(Data_Point)
        Data_Point.Show()
        m_bDataPointShow = True

        View_Dilaog.TopLevel = False
        View_Dilaog.Dock = DockStyle.Fill
        Me.SplitContainer4.Panel1.Controls.Add(View_Dilaog)
        View_Dilaog.Show()

        IOUtil = New IOLib(Me.TBox_Data, Me.LBox_Data)
        'MainFrm = Me
        Me.TBox_Data.SelectionStart = Len(TBox_Data.Text)

        '#If B_3DOUT = "FALSE" Then
        '        RibbonMenuControl14.ZahyoList.Visibility = System.Windows.Visibility.Hidden    'DELL By Yamada 20150318
        '#End If



        '--画面メニュー状態

        '--del.rep.start-------------------------12.10.17
        ' リボンメニューに表示／非表示の状態を反映
#If 0 Then
        ''MM_V_12.Checked = True
#End If
        '--del.rep.end---------------------------12.10.17
        '--ins.start-----------------------------12.10.17
        SplitContainer1.SplitterDistance = 135  '--ElementHost1.Height + 2
        SplitContainer1.VerticalScroll.Visible = False
        SplitContainer1.IsSplitterFixed = True
        SplitContainer3.IsSplitterFixed = True
        '--ins.end-------------------------------12.10.17

        sys_CoordInfo.mat = YCM_GetUnitMat()

        '画像タブ

        objFBM = New FBMlib.FeatureImage
#If HALCON = 11 Then


        winpreview = AxHWindowXCtrl6.HalconWindow
        win1 = AxHWindowXCtrl2.HalconWindow
        win2 = AxHWindowXCtrl3.HalconWindow
        win3 = AxHWindowXCtrl4.HalconWindow
        win4 = AxHWindowXCtrl5.HalconWindow
        Try
            HOperatorSet.SetDraw(winpreview, "margin")
            HOperatorSet.SetDraw(win1, "margin")
            HOperatorSet.SetDraw(win2, "margin")
            HOperatorSet.SetDraw(win3, "margin")
            HOperatorSet.SetDraw(win4, "margin")

        Catch ex As Exception
            My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\log\log.txt",
                            System.Reflection.MethodBase.GetCurrentMethod.Name & " " & ex.Message & vbNewLine, True)
        End Try


        HOperatorSet.SetDraw(winpreview, "margin")
        HOperatorSet.SetDraw(win1, "margin")
        HOperatorSet.SetDraw(win2, "margin")
        HOperatorSet.SetDraw(win3, "margin")
        HOperatorSet.SetDraw(win4, "margin")
#End If
        flgDispVal = flgDisp.flgCT
        strLastProjectPath = My.Settings.strLastProjPath

        '座標値一覧を非表示にして表示
        Me.SplitContainer3.SplitterDistance = Me.SplitContainer2.Panel2.Width - 10
        'YCM_3DView.Size = New System.Drawing.Size(MainFrm.SplitContainer4.Panel1.Width, MainFrm.SplitContainer4.Panel1.Height)
        '   YCM_3DView.Dock = DockStyle.Fill
        'Me.KeyPreview = True
        'RibbonMenuControl1.BackgroundProperty = Windows.SystemColors.ControlColor（13.1.22山本さん質問：リボンにwindowsシステム?の設定のカラーを設定したい⇒PPT）

        'Me.ElementHost1.BackColor = Color.Red

        'RibbonMenuControl14. = Common.FormBackColor
        'ChgView3DView()
        'ChgViewImage1_3DView()
        If Me.Tag = "1" Then

        End If

        'ADD By Yamada 20150303 Sta -------ユーザー納品時には非表示に
#If USER = "TRUE" Then
        RibbonMenuControl14.RbnGrpGazou3DPoint.Visibility = Windows.Visibility.Collapsed        '画像操作タブの3D点グループを非表示
        RibbonMenuControl14.RbnGrpCreatPointBatch.Visibility = Windows.Visibility.Collapsed     'ツールタブの点作成グループを非表示
        RibbonMenuControl14.RbnGrpCAD.Visibility = Windows.Visibility.Collapsed                'ツールタブのCADグループを非表示
        RibbonMenuControl14.AutoLabeling.Visibility = Windows.Visibility.Collapsed              'ツールタブの自動ラべリングを非表示
        'RibbonMenuControl14.RbnGrpOffset.Visibility = Windows.Visibility.Collapsed              'ツールタブのOffsetグループを非表示

#End If
        'ADD By Yamada 20150303 End -------

        'ADD By Kiryu 20160204 Sta ---  カスタム定数で任意点計測の表示・非表示変更
        Dim B_NINITEN As Boolean = GetPrivateProfileInt("Command", "NINITEN", 0, My.Application.Info.DirectoryPath & "\vform.ini")

        If B_NINITEN = False Then
            RibbonMenuControl14.RbnGrpArbitraryPoint.Visibility = Windows.Visibility.Collapsed              '画像操作タブのフリー点・ターゲット点を非表示
        End If

        'ADD By Kiryu 20160620 Sta ---  カスタム定数でオフセットの表示・非表示変更
        Dim CT_offset As Integer = GetPrivateProfileInt("Command", "CT_offset", 0, My.Application.Info.DirectoryPath & "\vform.ini")
        If CT_offset = False Then
            RibbonMenuControl14.RbnGrpOffset.Visibility = Windows.Visibility.Collapsed              'ツールタブのOffSetを非表示
        End If
        'ADD By Kiryu 20160620 End ---

        'ADD By Yamada 20150303 End -------
        'ADD By Kiryu 20160204 End ---

    End Sub
    Protected Overrides Sub OnSizeChanged(ByVal e As System.EventArgs)
        MyBase.OnSizeChanged(e)
    End Sub
    Protected Sub myView_OnKeyDown(ByVal Sender As Object, ByVal kea As System.Windows.Forms.KeyEventArgs)
        If (kea.KeyCode = Keys.Escape) Then
            'Application.Exit()
        End If
    End Sub


    '--rep.12.10.17    Private Sub YCM_MainFrame_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs)
    'Private Sub YCM_MainFrame_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
    Public Sub Close()
        On Error Resume Next
        View_Dilaog.Visible = False
        View_Dilaog.Close()
        Data_Point.Close()
        If FileSystem.Dir(g_InputIsavePath & "\計測データ_Temp.mdb") <> "" Then
            Kill(g_InputIsavePath & "\計測データ_Temp.mdb")
        End If
        IOUtil.EndThread()
        ' Application.Exit()
    End Sub

    Public Sub setTextMessage(ByVal strMessage As String)
        Me.TBox_Data.Text = strMessage
    End Sub

    '--rep.12.11.23    Public Sub TBox_Data_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    'Private Sub TBox_Data_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TBox_Data.KeyUp
    Private Sub TBox_Data_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles TBox_Data.PreviewKeyDown
        If e.KeyCode = Keys.Enter Then
            If IOUtil.blnSelectMode = False Then
                IOUtil.EndThread()
                IOUtil.RunCommand()
            Else
                Dim intLen As Integer
                intLen = Len(Me.TBox_Data.Text) - Len(IOUtil.strStrValue)
                If (InStr(Me.TBox_Data.Text, IOUtil.strStrValue) > 0) Or (intLen > 0) Then
                    IOUtil.strStrValue = Strings.Right(Me.TBox_Data.Text, intLen)
                Else
                    IOUtil.strStrValue = Me.TBox_Data.Text
                End If
                ''IOUtil.strStrValue = Strings.Right(Me.TBox_Data.Text, intLen)
                Me.LBox_Data.Items.Add(Me.TBox_Data.Text)
                Me.TBox_Data.Text = ":"
                LBox_Data.SelectedIndex = LBox_Data.Items.Count - 1
                ChangeFinis = 1
            End If
        End If
        If e.KeyCode = Keys.Escape Then
            IOUtil.blnSelectMode = False
            'IOUtil.EndThread()
        End If
    End Sub

    '--rep.12.10.17    Private Sub YCM_MainFrame_Resize(ByVal sender As Object, ByVal e As System.EventArgs)
    Private Sub YCM_MainFrame_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        ''--ins.start-----------------------------12.10.17
        'SplitContainer1.SplitterDistance = ElementHost1.Height
        ''--ins.end-------------------------------12.10.17
        View_Dilaog.Height = SplitContainer2.Panel2.Height
        Data_Point.Height = SplitContainer2.Panel2.Height
        Me.LBox_Data.Height = Me.SplitContainer4.Panel2.Height - Me.TBox_Data.Height
        Debug.Print("Resized!")
    End Sub



    Private Sub YCM_MainFrame_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs)
        If NewOrOld = 1 Then

            If MsgBox("計測データが保存されていません、保存しますか？", MsgBoxStyle.OkCancel) Then
                'updata
                Dim str_finis As String
                str_finis = ComSel_SelectFolderByShell("", False) '13.1.17山田?
                ''2012.12.26=======================================================================================================================================================

                ''無限ループ（現在開いているフォルダに新規フォルダ作成）⇒「別のフォルダを指定して下さい」などのメッセージを

                ''※（新）が（現）+"\"のパスで始まっていればNG
                '2012.12.27 （現在開いているフォルダに新規フォルダ作成）⇒YCM_MainFrame_FormClosingは通っていない（YCM_Command.vbのCommand_saveas()を通過します）山田
                'Dim GInputIsavePath As String = g_InputIsavePath

                'If (GInputIsavePath.EndsWith("\")) Then
                '    If (str_finis.StartsWith(GInputIsavePath)) Then
                '        'EndsWith：文字列インスタンスの末尾が、指定した文字列（\）と一致するかどうかを判断します。

                '        'StartsWith：この文字列インスタンスの先頭が、指定した文字列（GInputIsavePath）と一致するかどうかを判断します。

                '        MsgBox("指定されたフォルダは現在開いているフォルダとなっています。別のフォルダを指定して下さい。", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
                '        Exit Sub
                '    Else
                '    End If
                'Else
                '    Dim PresentPath As String = GInputIsavePath & "\"
                '    If (str_finis.StartsWith(PresentPath)) Then
                '        MsgBox("指定されたフォルダは現在開いているフォルダとなっています。別のフォルダを指定して下さい。", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
                '        Exit Sub
                '    Else
                '    End If
                'End If
                ''2012.12.26=======================================================================================================================================================
                If str_finis <> "" Then
                    YCM_CopyDir(g_InputTempPath, str_finis)
                    YCM_DeleteDir(g_InputTempPath)
                End If
            Else
                YCM_DeleteDir(g_InputTempPath)
            End If
        End If
        YCM_DeleteDir(g_InputTempPath)
    End Sub

    Private Sub TBox_Data_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        View_Dilaog.Select()
    End Sub
#End Region

#Region "メニュー関連"
#Region "メニュー関連[ファイル]"
    'ファイル－「新規」★
    Public Sub FileNew()
        'TimeMonStart()
        Try
            IOUtil.EndThread()
            IOUtil.LibCommand("new")
            Dim ii As Integer
            For ii = 0 To 4
                dispWin(ii) = -1
            Next ii
        Catch ex As Exception
            '   MsgBox(ex.ToString)
        End Try

        'TimeMonEnd()
    End Sub
    'ファイル－「開く」★
    Public Sub FileOpen()
        IOUtil.EndThread()
        IOUtil.LibCommand("open")
        Dim ii As Integer
        For ii = 0 To 4
            dispWin(ii) = -1
        Next ii
    End Sub
    'ファイル－「閉じる」★
    Public Sub FileClose()
        Call YCMFileClose()
    End Sub
    'ファイル－「閉じる」

    Private Sub YCMFileClose()
        IOUtil.EndThread()
        Dim strColNameArr As New ArrayList
        Dim strColWidArr As New ArrayList
        '要素をなくす
        ReDim gDrawRays(0)
        ReDim gDrawCamers(0)
        ReDim gDrawPoints(0)
        ReDim gDrawLabelText(0)
        ReDim gDrawUserLines(0)
        ReDim gDrawCircleNew(0)


        nRays = 0
        nCamers = 0
        nLookPoints = 0
        nLabelText = 0
        nUserLines = 0
        nCircleNew = 0

        '13.1.25スケールライン削除==========？？？？（ひとまず、2点間の距離を0に）

        gScaleLine = Nothing
        'gScaleLine.SetStartPnt(0, 0, 0)
        'gScaleLine.SetEndPnt(0, 0, 0)
        '13.1.25スケールライン削除==========

        Data_Point.DGV_DV.Rows.Clear()
        If FileSystem.Dir(g_InputIsavePath & "\計測データ_Temp.mdb") <> "" Then
            Kill(g_InputIsavePath & "\計測データ_Temp.mdb")
        End If
        objFBM = Nothing
        GC.Collect()
        ImageListView.Clear()
#If HALCON = 11 Then
        HOperatorSet.ClearWindow(winpreview)
        HOperatorSet.ClearWindow(win1)
        HOperatorSet.ClearWindow(win2)
        HOperatorSet.ClearWindow(win3)
        HOperatorSet.ClearWindow(win4)
#End If
        binfirstopen = False

        Me.Text = FormTitle
    End Sub
    'ファイル－「上書き保存」★
    Public Sub FileSave()
        TimeMonStart()

        IOUtil.EndThread()
        IOUtil.LibCommand("save")

        TimeMonEnd()
    End Sub
    'ファイル－「名前を付けて保存」★
    Public Sub FileSaveAs()
        IOUtil.EndThread()
        IOUtil.LibCommand("saveas")
    End Sub
    'ファイル－「CSV書き出し」★
    Public Sub FileCSVOut()
        IOUtil.EndThread()
        IOUtil.LibCommand("outcsv")
    End Sub
    'ファイル－「終了」

    Public Sub FileEnd()
        On Error Resume Next
        If FileSystem.Dir(g_InputIsavePath & "\計測データ_Temp.mdb") <> "" Then
            Kill(g_InputIsavePath & "\計測データ_Temp.mdb")
        End If
        Me.Close()
    End Sub

    '========================================未実装関連 
#If 0 Then
    ''ファイル－「VBMファイル読込み」

    'Private Sub MM_F_7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MM_F_7.Click
    '    'VBMファイルの読込み
    'End Sub
    Private Sub ToolStripButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'DXF書き出し

    End Sub
#End If
#End Region
#Region "メニュー関連[表示]"
    '表示－「全体表示」★
    Public Sub BtnDispZoomAll()
        Call ZoomAll()
    End Sub
    '表示－「範囲」★
    Public Sub BtnDispZoomWin()
        IOUtil.EndThread()
        m_blnCommandArea = True
        'IOUtil.EndThread()
        'IOUtil.LibCommand("area")
    End Sub
    '表示－「拡大」★
    Public Sub BtnDispZoomIn()
        Dim sx As Double = Me.ClientSize.Width
        Dim sy As Double = Me.ClientSize.Height
        Dim pidx As Double, pidy As Double
        Dim lenx As Double, leny As Double
        Dim dix1 As Double, diy1 As Double
        Dim dix2 As Double, diy2 As Double
        pidx = (View_Dilaog.dblwindowLeft + View_Dilaog.dblwindowRight) / 2.0
        pidy = (View_Dilaog.dblwindowTop + View_Dilaog.dblwindowBottom) / 2.0
        lenx = View_Dilaog.dblwindowRight - View_Dilaog.dblwindowLeft
        leny = View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom
        dix1 = lenx / sx : diy1 = leny / sy
        lenx = lenx * 0.9 : leny = leny * 0.9

        dix2 = lenx / sx : diy2 = leny / sy
        View_Dilaog.dblwindowLeft = pidx - lenx / 2.0
        View_Dilaog.dblwindowRight = pidx + lenx / 2.0
        View_Dilaog.dblwindowBottom = pidy - leny / 2.0
        View_Dilaog.dblwindowTop = pidy + leny / 2.0

        View_Dilaog.Wheel_Speedx = lenx
        View_Dilaog.Wheel_Speedy = leny
    End Sub
    '表示－「縮小」★
    Public Sub BtnDispZoomOut()
        Dim sx As Double = Me.ClientSize.Width
        Dim sy As Double = Me.ClientSize.Height
        Dim pidx As Double, pidy As Double
        Dim lenx As Double, leny As Double
        Dim dix1 As Double, diy1 As Double
        Dim dix2 As Double, diy2 As Double
        pidx = (View_Dilaog.dblwindowLeft + View_Dilaog.dblwindowRight) / 2.0
        pidy = (View_Dilaog.dblwindowTop + View_Dilaog.dblwindowBottom) / 2.0
        lenx = View_Dilaog.dblwindowRight - View_Dilaog.dblwindowLeft
        leny = View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom
        dix1 = lenx / sx : diy1 = leny / sy
        lenx = lenx * 1.1 : leny = leny * 1.1

        dix2 = lenx / sx : diy2 = leny / sy
        View_Dilaog.dblwindowLeft = pidx - lenx / 2.0
        View_Dilaog.dblwindowRight = pidx + lenx / 2.0
        View_Dilaog.dblwindowBottom = pidy - leny / 2.0
        View_Dilaog.dblwindowTop = pidy + leny / 2.0

        View_Dilaog.Wheel_Speedx = lenx
        View_Dilaog.Wheel_Speedy = leny
    End Sub
    '表示－「画面移動」★
    Public Sub BtnDispScroll()
        IOUtil.EndThread()
        IOUtil.LibCommand("move")
    End Sub
    '表示－「3D回転」★
    Public Sub BtnDispRot3D()
        IOUtil.EndThread()
        IOUtil.LibCommand("rotate")
    End Sub
    ''表示－「カメラ視点」★
    'Public Sub BtnDispCameraView()
    '    IOUtil.EndThread()
    '    IOUtil.LibCommand("camer")
    'End Sub
    '表示－「XY平面」★
    Public Sub BtnDispXYPlane()
        IOUtil.EndThread()
        IOUtil.LibCommand("XY")
    End Sub
    '表示－「XZ平面」★
    Public Sub BtnDispXZPlane()
        IOUtil.EndThread()
        IOUtil.LibCommand("XZ")
    End Sub
    '表示－「YZ平面」★
    Public Sub BtnDispYZPlane()
        IOUtil.EndThread()
        IOUtil.LibCommand("YZ")
    End Sub

    '表示－「計測点表示／非表示」★
    Public Sub ChkBoxDispMeasPointOnOff(ByVal bOn As Boolean)
        entset_point.blnVisiable = bOn
    End Sub
    '表示－「追加計測点表示／非表示」★
    Public Sub ChkBoxDispAddMeasPointOnOff(ByVal bOn As Boolean)
        entset_pointUser.blnVisiable = bOn
    End Sub
    '表示－「カメラ表示／非表示」★
    Public Sub ChkBoxDispCameraOnOff(ByVal bOn As Boolean)
        entset_camera.blnVisiable = bOn
    End Sub
    '表示－「レイ表示／非表示」★
    Public Sub ChkBoxDispRayOnOff(ByVal bOn As Boolean)
        entset_ray.blnVisiable = bOn
    End Sub
    '表示－「ラベル表示／非表示」★
    Public Sub ChkBoxDispLabelOnOff(ByVal bOn As Boolean)
        entset_label.blnVisiable = bOn
    End Sub
    ''表示－「任意図形表示／非表示」★後々削除
    'Public Sub ChkBoxDispFigOnOff(ByVal bOn As Boolean)
    '    entset_line.blnVisiable = bOn
    '    entset_circle.blnVisiable = bOn
    'End Sub
    '表示－「線分(任意図形)表示／非表示」★20121126
    Public Sub ChkBoxDispFigLineOnOff(ByVal bOn As Boolean)
        entset_line.blnVisiable = bOn
    End Sub
    '表示－「円(任意図形)表示／非表示」★20121126
    Public Sub ChkBoxDispFigCircleOnOff(ByVal bOn As Boolean)
        entset_circle.blnVisiable = bOn
    End Sub
    '表示－「線分(CAD図形)表示／非表示」★20121126
    Public Sub ChkBoxDispCadLineOnOff(ByVal bOn As Boolean)
        entset_line_CAD.blnVisiable = bOn
    End Sub
    '表示－「円(CAD図形)表示／非表示」★20121126
    Public Sub ChkBoxDispCadCircleOnOff(ByVal bOn As Boolean)
        entset_circle_CAD.blnVisiable = bOn
    End Sub
    '表示－「コードターゲット表示／非表示」★20121126
    Public Sub ChkBoxDispCordPointOnOff(ByVal bOn As Boolean)
        CordTargetIsvisible = bOn
    End Sub

    '表示－「座標値リスト」★
    Public Sub BtnDispCoordList()
        Debug.Print("座標値リストON/OFF")
        Debug.Print("SplitContainer3.Width= " & SplitContainer3.Width)
        Debug.Print("SplitContainer3.Width= " & SplitContainer3.Width)
        Debug.Print("SplitContainer3.SplitterDistance= " & SplitContainer3.SplitterDistance)


#If 1 Then
        If binfrmdataviewclosed = False Then '（座標値リストが表示されているときがFalse）

            If (m_bDataPointShow = False) Then '（座標値リストが表示されているときがFalse⇒今から座標値リストを非表示に）

                arrlist.Clear()
                For ii As Integer = 0 To Data_Point.DGV_DV.RowCount - 1
                    arrlist.Add(Data_Point.DGV_DV.Rows(ii).Cells(0).Value)
                Next

                binfrmdataview = False
                Data_Point.Hide() '座標値リストを非表示にします


                '13.1.8　山田修正＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝

                If MainFrm.SplitContainer3.SplitterDistance <= 25 Then '画面操作ビューの幅≦25（画面操作ビューのみ表示など）

                    MainFrm.SplitContainer3.SplitterDistance = MainFrm.SplitContainer3.Width
                    'MainFrm.SplitContainer2.SplitterDistance = MainFrm.SplitContainer1.Panel2.Width '画像操作ビュー
                Else '画面操作ビューの幅＞25
                    MainFrm.SplitContainer3.SplitterDistance = MainFrm.SplitContainer3.Width
                    'MainFrm.SplitContainer3.SplitterDistance = MainFrm.SplitContainer2.Panel2.Width
                End If
                View_Dilaog.Size = New System.Drawing.Size(MainFrm.SplitContainer4.Panel1.Width, MainFrm.SplitContainer4.Panel1.Height)
                '13.1.8　山田修正＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝


                ''201211128以前-----------------------------------------------------------------------------------------------------
                'Data_Point.Hide()
                'MainFrm.SplitContainer3.Panel2.Controls.Clear() '座標値リストクリア
                'MainFrm.SplitContainer3.SplitterDistance = MainFrm.SplitContainer2.Panel2.Width
                'YCM_3DView.Size = New System.Drawing.Size(MainFrm.SplitContainer4.Panel1.Width, MainFrm.SplitContainer4.Panel1.Height)
                ''201211128以前-----------------------------------------------------------------------------------------------------

            Else
                binfrmdataview = True
                Data_Point.TopLevel = False
                Data_Point.FormBorderStyle = Windows.Forms.FormBorderStyle.None
                Data_Point.Location = New System.Drawing.Point(0, 0)
                Data_Point.Btn_Hochi.Text = "分離"
                Data_Point.Data_Vertex_Dock = 1
                Data_Point.Show()

                ''20121128以前===================================================================================================================================
                ''MainFrm.SplitContainer3.SplitterDistance = MainFrm.SplitContainer3.Width - 405
                'MainFrm.SplitContainer3.SplitterDistance = Math.Abs(MainFrm.SplitContainer3.Width - 405) '画像表示①，④では表示しない 
                'MainFrm.SplitContainer3.Panel2.Controls.Add(Data_Point)
                'If (arrlist.Count > 0) Then
                '    For ii As Integer = 0 To Data_Point.DGV_DV.RowCount - 1
                '        Data_Point.DGV_DV.Rows(ii).Cells(0).Value = arrlist(ii)
                '    Next
                'End If
                ''20121128以前===================================================================================================================================

                '13.1.8　山田修正＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝

                'If MainFrm.SplitContainer2.Width > 459 Then '全体の幅が、座標値リストを表示するに十分にある場合

                '    '（454=画像操作ビュー25+スプリッターの幅4+3D操作ビューの幅25+スプリッターの幅4+座標値リストの幅401）

                '    If MainFrm.SplitContainer3.Width > 405 Then '13.1.8　山田　（座標値リスト表示）座標値リストをの表示できる幅があるかどうか
                '        MainFrm.SplitContainer3.SplitterDistance = MainFrm.SplitContainer3.Width - 405 '表示できる幅がある場合

                '        MainFrm.SplitContainer3.Panel2.Controls.Add(Data_Point)
                '        If (arrlist.Count > 0) Then
                '            For ii As Integer = 0 To Data_Point.DGV_DV.RowCount - 1
                '                Data_Point.DGV_DV.Rows(ii).Cells(0).Value = arrlist(ii)
                '            Next
                '        End If
                '    Else '13.1.8　山田　表示できる幅がない場合⇒（座標値リスト表示）画面操作ビューに切り替え、もしくはSplitContainer2のSplitterを右にずらした
                '        MainFrm.SplitContainer2.SplitterDistance = MainFrm.SplitContainer2.Width - 434
                '        MainFrm.SplitContainer3.SplitterDistance = MainFrm.SplitContainer3.Panel1MinSize
                '        Data_Point.Show()
                '    End If
                'Else

                '    Exit Sub
                'End If
                '13.1.8　山田修正＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝

                '16.7.23　桐生修正＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝

                If MainFrm.SplitContainer2.Width > 459 Then '全体の幅が、座標値リストを表示するに十分にある場合


                    '（454=画像操作ビュー25+スプリッターの幅4+3D操作ビューの幅25+スプリッターの幅4+座標値リストの幅401）

                    If MainFrm.SplitContainer3.Width > 402 Then '13.1.8　山田　（座標値リスト表示）座標値リストをの表示できる幅があるかどうか
                        MainFrm.SplitContainer3.SplitterDistance = MainFrm.SplitContainer3.Width - 405 '表示できる幅がある場合

                        MainFrm.SplitContainer3.Panel2.Controls.Add(Data_Point)
                        If (arrlist.Count > 0) Then
                            For ii As Integer = 0 To Data_Point.DGV_DV.RowCount - 1
                                Data_Point.DGV_DV.Rows(ii).Cells(0).Value = arrlist(ii)
                            Next
                        End If
                    Else '13.1.8　山田　表示できる幅がない場合⇒（座標値リスト表示）画面操作ビューに切り替え、もしくはSplitContainer2のSplitterを右にずらした
                        MainFrm.SplitContainer2.SplitterDistance = MainFrm.SplitContainer2.Width - 434
                        MainFrm.SplitContainer3.SplitterDistance = MainFrm.SplitContainer3.Panel1MinSize
                        Data_Point.Show()
                    End If
                Else

                    Exit Sub
                End If
                '16.7.23　桐生修正＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝


            End If
        End If
        If binfrmdataviewclosed = True Then
            Data_Point = New Data_Vertex
            Data_Point.Show()
        End If

        m_bDataPointShow = Not m_bDataPointShow
        '---------------------------------------
#Else
        If binfrmdataviewclosed = False Then
            If MM_V_12.Checked = False Then
                arrlist.Clear()
                For ii As Integer = 0 To Data_Point.DGV_DV.RowCount - 1
                    arrlist.Add(Data_Point.DGV_DV.Rows(ii).Cells(0).Value)
                Next
                binfrmdataview = False
                Data_Point.Hide()
                MainFrm.SplitContainer1.Panel2.Controls.Clear()
                MainFrm.SplitContainer1.SplitterDistance = MainFrm.SplitContainer1.Width
            End If
            If MM_V_12.Checked = True Then

                binfrmdataview = True
                Data_Point.TopLevel = False
                Data_Point.FormBorderStyle = Windows.Forms.FormBorderStyle.None
                Data_Point.Location = New System.Drawing.Point(0, 0)
                Data_Point.Btn_Hochi.Text = "分離"
                Data_Point.Data_Vertex_Dock = 1
                Data_Point.Show()
                MainFrm.SplitContainer1.SplitterDistance = MainFrm.SplitContainer1.Width - 405
                MainFrm.SplitContainer1.Panel2.Controls.Add(Data_Point)
                For ii As Integer = 0 To Data_Point.DGV_DV.RowCount - 1
                    Data_Point.DGV_DV.Rows(ii).Cells(0).Value = arrlist(ii)
                Next
            End If
        End If
        If binfrmdataviewclosed = True Then
            Data_Point = New Data_Vertex
            Data_Point.Show()
        End If
#End If

    End Sub
    ''表示－「座標値リスト」

    'Private Sub MM_V_12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MM_V_12.Click
    '    MM_V_12.Checked = Not MM_V_12.Checked
    '    If binfrmdataviewclosed = False Then
    '        If MM_V_12.Checked = False Then
    '            arrlist.Clear()
    '            For ii As Integer = 0 To Data_Point.DGV_DV.RowCount - 1
    '                arrlist.Add(Data_Point.DGV_DV.Rows(ii).Cells(0).Value)
    '            Next
    '            binfrmdataview = False
    '            Data_Point.Hide()
    '            MainFrm.SplitContainer1.Panel2.Controls.Clear()
    '            MainFrm.SplitContainer1.SplitterDistance = MainFrm.SplitContainer1.Width
    '        End If
    '        If MM_V_12.Checked = True Then

    '            binfrmdataview = True
    '            Data_Point.TopLevel = False
    '            Data_Point.FormBorderStyle = Windows.Forms.FormBorderStyle.None
    '            Data_Point.Location = New System.Drawing.Point(0, 0)
    '            Data_Point.Btn_Hochi.Text = "分離"
    '            Data_Point.Data_Vertex_Dock = 1
    '            Data_Point.Show()
    '            MainFrm.SplitContainer1.SplitterDistance = MainFrm.SplitContainer1.Width - 405
    '            MainFrm.SplitContainer1.Panel2.Controls.Add(Data_Point)
    '            For ii As Integer = 0 To Data_Point.DGV_DV.RowCount - 1
    '                Data_Point.DGV_DV.Rows(ii).Cells(0).Value = arrlist(ii)
    '            Next
    '        End If
    '    End If
    '    If binfrmdataviewclosed = True Then
    '        Data_Point = New Data_Vertex
    '        Data_Point.Show()
    '    End If
    'End Sub
#End Region
#Region "メニュー関連[作成]"
    '作成－「任意線分」★
    Public Sub BtnToolCreatLine()
        IOUtil.EndThread()
        IOUtil.LibCommand("userline") '--drawline")
    End Sub
    '作成－「3点円」★
    Public Sub BtnToolCreatCircle()
        'bincirclestart = True
        'intcircle = 0
        IOUtil.EndThread()
        IOUtil.LibCommand("circle3p")
    End Sub
    '作成－「中心円」★
    Public Sub BtnToolCreatCentralCircle()
        IOUtil.EndThread()
        IOUtil.LibCommand("circle1p")
    End Sub
#End Region
#Region "メニュー関連[設定]"
    '設定－「作図属性」★
    Public Sub BtnToolDrawAttrSet()
        IOUtil.EndThread()
        IOUtil.LibCommand("drawset")
    End Sub
#End Region
#Region "メニュー関連[ツール]"
    'ツール－「座標変換設定」★
    Public Sub BtnToolCoordConvSet()
        IOUtil.EndThread()
        IOUtil.LibCommand("setcoord")
    End Sub
    'ツール－「スケール設定」★
    Public Sub BtnToolScaleSet()
        IOUtil.EndThread()
        IOUtil.LibCommand("setscale")
    End Sub
    'ツール－「2点間距離」★
    Public Sub BtnToolDistance2Point()
        IOUtil.EndThread()
        IOUtil.LibCommand("dist")
    End Sub
    'ツール－「自動ラベリング」★
    Public Sub BtnToolAutoLabeling()
        IOUtil.EndThread()
        IOUtil.LibCommand("label")
    End Sub
    'ツール－「手動ラベリング」★
    Public Sub BtnToolManuLabeling()
        IOUtil.EndThread()
        IOUtil.LibCommand("changelabel")
    End Sub
#End Region
#Region "メニュー関連[表示拡大／縮小]"
    '===================================================================================================20121115
    '計測点拡大★(タ－ゲット面が拡大⇒中心円、十字線も自動で拡大できるようにしたい)
    'Public Sub BtnDispMarkerBig()
    '    If entset_point1.screensize * (1 + entset_point1.konomi / 100.0) > entset_point1.maxscale Then
    '        entset_point1.screensize = entset_point1.maxscale
    '    Else
    '        entset_point1.screensize = entset_point1.screensize * (1 + entset_point1.konomi / 100.0)
    '    End If
    'End Sub
    '===================================================================================================20121115
    Public Sub BtnDispMarkerBig()
        'flgBtnDispMarkerBig = False
        If entset_point.screensize * (1 + entset_point.konomi / 100.0) > entset_point.maxscale Then
            entset_point.screensize = entset_point.maxscale
        Else
            entset_point.screensize = entset_point.screensize * (1 + entset_point.konomi / 100.0)
        End If
    End Sub


    '計測点縮小★(タ－ゲット面が縮小⇒中心円、十字線も自動で縮小できるようにしたい)
    '===================================================================================================20121115
    'Public Sub BtnDispMarkerSmall()
    '    If entset_point1.screensize * (1 - entset_point1.konomi / 100.0) < entset_point1.minscale Then
    '        entset_point1.screensize = entset_point1.minscale
    '    Else
    '        entset_point1.screensize = entset_point1.screensize * (1 - entset_point1.konomi / 100.0)
    '    End If
    '    'entset_point.screensize = entset_point.screensize * (1 - entset_point.konomi / 100.0)
    'End Sub
    '===================================================================================================20121115
    Public Sub BtnDispMarkerSmall()
        If entset_point.screensize * (1 - entset_point.konomi / 100.0) < entset_point.minscale Then
            entset_point.screensize = entset_point.minscale
        Else
            entset_point.screensize = entset_point.screensize * (1 - entset_point.konomi / 100.0)
        End If
        'entset_point.screensize = entset_point.screensize * (1 - entset_point.konomi / 100.0)
    End Sub
    'カメラ拡大★

    Public Sub BtnDispCameraBig()
        If entset_camera.screensize * (1 + entset_camera.konomi / 100.0) > entset_camera.maxscale Then
            entset_camera.screensize = entset_camera.maxscale
        Else
            entset_camera.screensize = entset_camera.screensize * (1 + entset_camera.konomi / 100.0)
        End If
        'entset_camera.screensize = entset_camera.screensize * (1 + entset_camera.konomi / 100.0)
    End Sub
    'カメラ縮小★
    Public Sub BtnDispCameraSmall()
        If entset_camera.screensize * (1 - entset_camera.konomi / 100.0) < entset_camera.minscale Then
            entset_camera.screensize = entset_camera.minscale
        Else
            entset_camera.screensize = entset_camera.screensize * (1 - entset_camera.konomi / 100.0)
        End If
        'entset_camera.screensize = entset_camera.screensize * (1 - entset_camera.konomi / 100.0)
    End Sub
    'ラベル拡大★

    Public Sub BtnDispLabelBig()
        If entset_label.screensize * (1 + entset_label.konomi / 100.0) > entset_label.maxscale Then
            entset_label.screensize = entset_label.maxscale
        Else
            entset_label.screensize = entset_label.screensize * (1 + entset_label.konomi / 100.0)
        End If
    End Sub
    'ラベル縮小★
    Public Sub BtnDispLabelSmall()
        If entset_label.screensize * (1 - entset_label.konomi / 100.0) < entset_label.minscale Then
            entset_label.screensize = entset_label.minscale
        Else
            entset_label.screensize = entset_label.screensize * (1 - entset_label.konomi / 100.0)
        End If
    End Sub
#End Region
#Region "メニュー関連[CAD関連]"
    'ツール-CADの起動★
    Public Sub BtnToolCadStart()
        IOUtil.EndThread()
        IOUtil.LibCommand("RunCAD")
    End Sub
    'ツール-CAD図形読み込み★

    Public Sub BtnToolCadRead()
        IOUtil.EndThread()
        IOUtil.LibCommand("importCAD")
    End Sub
    'ツール-CAD図形書き出し★
    Public Sub BtnToolCadWrite()
        IOUtil.EndThread()
        IOUtil.LibCommand("exportCAD")
    End Sub
#End Region
#Region "メニュー関連[補間点作成関連]"
    '点作成(2点)★==未実装

    '========================================
    '========================================移動点の作成関連 
    'Private Sub ToolStripButton38_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    '・移動方向を2点で指定して移動点を作成
    '    '		command_NewPointByVec()
    '    IOUtil.EndThread()
    '    IOUtil.LibCommand("newpointbyvec")
    'End Sub
    'Private Sub ToolStripButton39_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    '・移動方向を3点で構成する面で指定して移動点を作成
    '    '		command_NewPointByFace()
    '    IOUtil.EndThread()
    '    IOUtil.LibCommand("newpointbyface")
    'End Sub
    'Private Sub ToolStripButton40_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    '・2点を指定して相互に移動した点を作成
    '    '		command_NewEachPoint()
    '    IOUtil.EndThread()
    '    IOUtil.LibCommand("neweachpoint")
    'End Sub
    '点作成(一括)★

    Public Sub BtnToolCreatPointBatch()
        '・リストで指定して移動点を一括作成
        '		command_NewPointByList()
        IOUtil.EndThread()
        IOUtil.LibCommand("newpointbylist")
    End Sub
#End Region
#End Region

#Region "メニュー関連(画像)"
    'ここは削除に
    ''解析－「画面表示」

    'Private Sub ToolStripButton13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    If flgPreview = True Then
    '        flgPreview = False
    '        Panel1.Visible = False
    '        FourImageTableLayoutPanel.Visible = True
    '        '   Me.Text = "３画像表示"
    '    Else
    '        flgPreview = True
    '        Panel1.Visible = True
    '        FourImageTableLayoutPanel.Visible = False
    '        ' Me.Text = "画像プレビュー"
    '    End If
    'End Sub
    '解析-ターゲット抽出★

    Public Sub BtnAnaTargetExt()
        '--rep.        FBMlib.T_treshold = CInt(imgToolStripTextBox1.Text)
        FBMlib.T_treshold = CInt(100)
        objFBM.DetectTargetsAllImages()
        flgDispVal = flgDisp.flgCT
        MsgBox("ターゲット抽出処理完了しました。", MsgBoxStyle.OkOnly, "確認")
    End Sub

    ''解析－「ターゲット抽出」

    'Private Sub ToolStripButton14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    '--rep.        FBMlib.T_treshold = CInt(imgToolStripTextBox1.Text)
    '    FBMlib.T_treshold = CInt(100)
    '    objFBM.DetectTargetsAllImages()
    '    flgDispVal = flgDisp.flgCT
    '    MsgBox("ターゲット抽出処理完了しました。", MsgBoxStyle.OkOnly, "確認")
    'End Sub
    '解析-一括処理

    Public Sub BtnAnaBatch()
        TimeMonStart()

        If objFBM.ProjectPath = "" Then
            MsgBox("画像データがありません。！", MsgBoxStyle.OkOnly, "確認")
            Exit Sub
        End If
        '  If MsgBox("一括解析を実行しますか？", MsgBoxStyle.OkCancel, "確認") = MsgBoxResult.Ok Then

        gfrmProgressBar = New frmProgressBar

        gfrmProgressBar.Show()
        gfrmProgressBar.Top = Me.Top
        gfrmProgressBar.Left = Me.Left
        gfrmProgressBar.ProgressBar1.Maximum = 100
        gfrmProgressBar.ProgressBar1.Value = 0
        gfrmProgressBar.Label1.Text = "ターゲット抽出中"
        Me.Refresh()
        gfrmProgressBar.Refresh()
        Dim sw As New System.Diagnostics.Stopwatch()
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\TimeMonitor.txt",
                                          vbNewLine & "処理開始時刻： " & Now & vbNewLine, True)

        sw.Start()
        '--rep.            FBMlib.T_treshold = CDbl(imgToolStripTextBox1.Text)
        FBMlib.T_treshold = CDbl(100)
        Debug.Print("suuri debug")
        Dim strPair As Integer = GetPrivateProfileInt("Kaiseki", "PairByBasepoint", 0, My.Application.Info.DirectoryPath & "\vform.ini")
        If strPair = 1 Then
            objFBM.IkkatuSyoriPair_Kijyun(My.Settings.blnCTonly)
            '20150209 SUURI ADD Sta 各画像外部評定を基準点座標により算出する機能------------------------------------------
        ElseIf strPair = 2 Then
            objFBM.IkkatuSyoriOneImagePoseByBasePoint(My.Settings.blnCTonly)
            '20150209 SUURI ADD End 各画像外部評定を基準点座標により算出する機能------------------------------------------
        Else
            objFBM.IkkatuSyori2(My.Settings.blnCTonly)

        End If

        Dim iiD_URAOMOTE As Integer = GetPrivateProfileInt("Command", "D_URAOMOTE", 0, My.Application.Info.DirectoryPath & "\vform.ini")
        '#If True Then  'Rep By YAMADA 20140620
        If iiD_URAOMOTE = 1 Then
            '#If D_URAOMOTE = "TRUE" Then
            '20130710 SUURI 裏表のCTによる計測結果の結合
            If MsgBox("裏表コードターゲットによる結合をしますか？", MsgBoxStyle.YesNo, "確認") = MsgBoxResult.Yes Then
                IkkatuSyoriNotConnectedImages()
            End If
            '#End If
        End If

        '#End If
        'H25.6.27 Yamada
        ImageListItemCheck()


        ' YCM_Offset_GenData2() ' SUURI 20130523 ADD

        'objFBM.SaveToMeasureDataDB(objFBM.ProjectPath & "\")' SUURI 20130620 DEL

        sw.Stop()
        'YMC_3DViewReDraw(m_strDataBasePath)' SUURI 20130620 DEL

        gfrmProgressBar.Close()
        ''Main_Tab.SelectTab(1)
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\TimeMonitor.txt",
                                           vbNewLine & "処理終了時刻： " & Now & vbNewLine & "一括処理完了しました。(" & sw.Elapsed.TotalSeconds.ToString & "秒)", True)
        ' MsgBox("一括処理完了しました。(" & sw.Elapsed.TotalSeconds.ToString & "秒)", MsgBoxStyle.OkOnly, "確認")
        ' End If

        TimeMonEnd()
    End Sub

    Private Sub IkkatuSyoriNotConnectedImages()
#If HALCON = 11 Then

        Dim tmpObjFBM As FBMlib.FeatureImage
        Dim strCamParam As String = My.Settings.strCamparam
        '(20140528 Tezuka ADD)
        If WorksD.CameraD.camera_path.Length() > 0 Then
            strCamParam = WorksD.CameraD.camera_path
        End If
        tmpObjFBM = New FBMlib.FeatureImage(strCamParam)
        tmpObjFBM.lstImages = New List(Of FBMlib.ImageSet)
        Dim i As Integer = 1

        For Each ISI As FBMlib.ImageSet In objFBM.lstImages
            If ISI.flgConnected = False Then
                Dim tmpISI As New FBMlib.ImageSet(ISI)
                tmpISI.ImageId = i
                tmpObjFBM.lstImages.Add(tmpISI)
                i += 1
            End If
        Next
        If i < 3 Then
            Exit Sub
        End If
        tmpObjFBM.IkkatuSyori2(True)
        objFBM.GenCommon3Dpoint()
        tmpObjFBM.GenCommon3Dpoint()

        YCM_ScaleAndOffset(objFBM)
        YCM_ScaleAndOffset(tmpObjFBM, True)

        Dim arrURAOMOTECTData(0) As CT_Data
        GetPoint_ByKubun(arrCTData, 99, 0, arrURAOMOTECTData)
        Dim lstOMOTECT As New List(Of FBMlib.Common3DCodedTarget)
        Dim lsttmpOMOTECT As New List(Of FBMlib.Common3DCodedTarget)
        Dim lstURACT As New List(Of FBMlib.Common3DCodedTarget)
        Dim lsttmpURACT As New List(Of FBMlib.Common3DCodedTarget)
        For Each CTD As CT_Data In arrURAOMOTECTData
            For Each C3DCT As FBMlib.Common3DCodedTarget In objFBM.lstCommon3dCT
                If CTD.CT_dat.PID = C3DCT.PID Then
                    lsttmpOMOTECT.Add(C3DCT)
                    Exit For
                End If
            Next
            For Each C3DCT As FBMlib.Common3DCodedTarget In tmpObjFBM.lstCommon3dCT
                If CTD.CT_dat.PID = C3DCT.PID Then
                    lsttmpURACT.Add(C3DCT)
                    Exit For
                End If
            Next
        Next

        For Each omoteC3DCT As FBMlib.Common3DCodedTarget In lsttmpOMOTECT
            For Each uraC3DCT As FBMlib.Common3DCodedTarget In lsttmpURACT
                If omoteC3DCT.K1 = uraC3DCT.K1 And omoteC3DCT.K2 = uraC3DCT.K2 Then
                    lstURACT.Add(uraC3DCT)
                    Exit For
                End If
            Next
        Next

        For Each uraC3DCT As FBMlib.Common3DCodedTarget In lstURACT
            For Each omoteC3DCT As FBMlib.Common3DCodedTarget In lsttmpOMOTECT
                If omoteC3DCT.K1 = uraC3DCT.K1 And omoteC3DCT.K2 = uraC3DCT.K2 Then
                    lstOMOTECT.Add(omoteC3DCT)
                    Exit For
                End If
            Next
        Next

        If lstURACT.Count = lstOMOTECT.Count And lstOMOTECT.Count > 2 Then
            Dim OmotePnts As New FBMlib.Point3D
            Dim UraPnts As New FBMlib.Point3D
            Dim HomMat As New Object
            Dim Quality As New Object
            For Each omoteC3DCT As FBMlib.Common3DCodedTarget In lstOMOTECT
                For Each uraC3DCT As FBMlib.Common3DCodedTarget In lstURACT
                    If omoteC3DCT.K1 = uraC3DCT.K1 And omoteC3DCT.K2 = uraC3DCT.K2 Then
                        OmotePnts.ConcatToMe(omoteC3DCT.lstRealP3d.Item(0))
                        UraPnts.ConcatToMe(uraC3DCT.lstRealP3d.Item(0))
                        Exit For
                    End If
                Next
            Next
            'OmotePnts.SetScale(1 / objFBM.pScaleMM)
            'UraPnts.SetScale(1 / objFBM.pScaleMM)
            Dim tmpHommat3d As New Object


            HOperatorSet.VectorToHomMat3d("rigid", UraPnts.X, UraPnts.Y, UraPnts.Z, OmotePnts.X, OmotePnts.Y, OmotePnts.Z, HomMat)

            ' FBMlib.hom_mat_3d_from_3d_3d_point_correspondence(UraPnts.X, UraPnts.Y, UraPnts.Z, OmotePnts.X, OmotePnts.Y, OmotePnts.Z, HomMat)
            HOperatorSet.AffineTransPoint3d(HomMat, UraPnts.X, UraPnts.Y, UraPnts.Z, UraPnts.X, UraPnts.Y, UraPnts.Z)
            UraPnts.GetDisttoOtherPose(OmotePnts, Quality)
            ''HOperatorSet.HomMat3dInvert(HomMat, HomMat)
            For Each ISI As FBMlib.ImageSet In tmpObjFBM.lstImages
                If ISI.flgConnected = True Then
                    Dim HM3D As New Object

                    HOperatorSet.PoseToHomMat3d(ISI.ImagePose.Pose, HM3D)
                    HOperatorSet.HomMat3dScale(HM3D, tmpObjFBM.pScaleMM, tmpObjFBM.pScaleMM, tmpObjFBM.pScaleMM, 0.0, 0.0, 0.0, HM3D)
                    HOperatorSet.HomMat3dCompose(HomMat, HM3D, HM3D)
                    HOperatorSet.HomMat3dScale(HM3D, (1 / objFBM.pScaleMM), (1 / objFBM.pScaleMM), (1 / objFBM.pScaleMM), 0.0, 0.0, 0.0, HM3D)
                    'HOperatorSet.HomMat3dScale(HM3D, (1 / ScaleToMM), (1 / ScaleToMM), (1 / ScaleToMM), 0.0, 0.0, 0.0, HM3D)
                    HOperatorSet.HomMat3dToPose(HM3D, ISI.ImagePose.Pose)
                    For Each ISI_omote As FBMlib.ImageSet In objFBM.lstImages
                        If ISI.ImageName = ISI_omote.ImageName Then
                            ISI_omote.ImagePose.Pose = ISI.ImagePose.Pose
                            ISI_omote.flgConnected = ISI.flgConnected
                        End If
                    Next
                End If
            Next

            'For Each ISI As FBMlib.ImageSet In tmpObjFBM.lstImages
            '    If ISI.flgConnected = True Then
            '        Dim HM3D As New Object
            '        Dim tmpUraPnts As New FBMlib.Point3D
            '        tmpUraPnts.CopyToMe(UraPnts)

            '        HOperatorSet.PoseToHomMat3d(ISI.ImagePose.Pose, HM3D)
            '        HOperatorSet.HomMat3dScale(HM3D, tmpObjFBM.pScaleMM, tmpObjFBM.pScaleMM, tmpObjFBM.pScaleMM, 0.0, 0.0, 0.0, HM3D)
            '        HOperatorSet.HomMat3dScale(HM3D, (1 / objFBM.pScaleMM), (1 / objFBM.pScaleMM), (1 / objFBM.pScaleMM), 0.0, 0.0, 0.0, HM3D)
            '        HOperatorSet.HomMat3dInvert(HM3D, HM3D)

            '        HOperatorSet.AffineTransPoint3D(HM3D, tmpUraPnts.X, tmpUraPnts.Y, tmpUraPnts.Z, tmpUraPnts.X, tmpUraPnts.Y, tmpUraPnts.Z)
            '        FBMlib.hom_mat_3d_from_3d_3d_point_correspondence(tmpUraPnts.X, tmpUraPnts.Y, tmpUraPnts.Z, OmotePnts.X, OmotePnts.Y, OmotePnts.Z, HomMat)
            '        HOperatorSet.AffineTransPoint3D(HomMat, tmpUraPnts.X, tmpUraPnts.Y, tmpUraPnts.Z, tmpUraPnts.X, tmpUraPnts.Y, tmpUraPnts.Z)
            '        tmpUraPnts.GetDisttoOtherPose(OmotePnts, Quality)
            '        'HOperatorSet.HomMat3dCompose(HomMat, HM3D, HM3D)
            '        'HOperatorSet.HomMat3dInvert(HM3D, HM3D)
            '        'HOperatorSet.HomMat3dScale(HomMat, (1 / objFBM.pScaleMM), (1 / objFBM.pScaleMM), (1 / objFBM.pScaleMM), 0.0, 0.0, 0.0, HM3D)
            '        HOperatorSet.HomMat3dToPose(HomMat, ISI.ImagePose.Pose)
            '        'HOperatorSet.HomMat3dScaleLocal(HM3D, ScaleToMM, ScaleToMM, ScaleToMM, HM3D)
            '        'HOperatorSet.HomMat3dCompose(HomMat, HM3D, HM3D)
            '        'HOperatorSet.HomMat3dScaleLocal(HM3D, (1 / ScaleToMM), (1 / ScaleToMM), (1 / ScaleToMM), HM3D)
            '        'HOperatorSet.HomMat3dToPose(HM3D, ISI.ImagePose.Pose)
            '        For Each ISI_omote As FBMlib.ImageSet In objFBM.lstImages
            '            If ISI.ImageName = ISI_omote.ImageName Then
            '                ISI_omote.ImagePose.Pose = ISI.ImagePose.Pose
            '                ISI_omote.flgConnected = ISI.flgConnected
            '            End If
            '        Next
            '    End If
            'Next

            objFBM.GenCommon3Dpoint()
            'objFBM.RunBA_New(FBMlib.T_treshold, False, objFBM.hv_CamparamOut)
            'objFBM.GenCommon3Dpoint()

        End If

#End If

    End Sub
    ''解析－「一括処理」

    'Private Sub imgToolStripButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    If objFBM.ProjectPath = "" Then
    '        MsgBox("画像データがありません。！", MsgBoxStyle.OkOnly, "確認")
    '        Exit Sub
    '    End If
    '    If MsgBox("一括解析を実行しますか？", MsgBoxStyle.OkCancel, "確認") = MsgBoxResult.Ok Then
    '解析-3D点削除★

    'Public Sub BtnAnaDel3DPoint()
    '    flgDraw = True
    '    Dim i As Integer = ImageListView.SelectedIndices.Item(0)
    '        frmProgressBar.Show()
    '        frmProgressBar.Top = Me.Top
    '        frmProgressBar.Left = Me.Left
    '        frmProgressBar.ProgressBar1.Maximum = 100
    '        frmProgressBar.ProgressBar1.Value = 0
    '        frmProgressBar.Label1.Text = "ターゲット抽出中"
    '        Me.Refresh()
    '        frmProgressBar.Refresh()
    '        Dim sw As New System.Diagnostics.Stopwatch()
    '        sw.Start()
    '        '--rep.            FBMlib.T_treshold = CDbl(imgToolStripTextBox1.Text)
    '        FBMlib.T_treshold = CDbl(100)
    '        objFBM.IkkatuSyori2()
    '        objFBM.SaveToMeasureDataDB(objFBM.ProjectPath & "\")
    '        sw.Stop()
    '        YMC_3DViewReDraw(m_strDataBasePath)

    '        frmProgressBar.Close()
    '        Main_Tab.SelectTab(1)

    '        ' MsgBox("一括処理完了しました。(" & sw.Elapsed.TotalSeconds.ToString & "秒)", MsgBoxStyle.OkOnly, "確認")
    '    End If
    'End Sub
    ''解析－「表示」（将来的には削除）

    'Private Sub imgToolStripComboBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    '--rep.        Select imgToolStripComboBox1.SelectedIndex
    '    Select Case 1
    '        Case 0
    '            flgDispVal = flgDisp.flgFeature
    '        Case 1
    '            flgDispVal = flgDisp.flgCT
    '        Case 2
    '            flgDispVal = flgDisp.flgObject
    '    End Select
    'End Sub

    '解析-輝度調整「明るく」

    Public Function BtnAnaBright() As Boolean
        BtnAnaBright = False
        'If scaleImage < 1000 Then
        '    scaleImage = scaleImage + ddImage
        If Scaling < 100.0 Then
            Scaling += ScalingAdder
        Else
            MsgBox("これ以上明るくなりません!")
            Exit Function
        End If
        Debug.Print("明" & scaleImage)
        AllDraw()
        'End If
        BtnAnaBright = True
        'マウスホィールの場合

        'Dim HH As Double = SplitContainer1.Panel1.Height 'リボンメニューコントロールの幅

        'Dim WW As Double = SplitContainer2.Panel1.Width '画像操作ビューの幅

        'If e.Y > HH And e.X < WW Then '(1)もしマウスの位置が画面操作ビューにある時・・・・
        '    If Ctrl_MouseWheel = 1 Then '①輝度調整+
        '        Delta：マウスホイールのノッチ1分(1移動量)=120
        '        scaleImage += 0.1 * e.Delta
        '        Debug.Print("scaleImage" & scaleImage)
        '        If scaleImage > 1000 Then '値の最高値を1000に限定する。

        '            scaleImage = 1000 '最大値にする
        '            MsgBox("これ以上明るくなりません!")
        '        ElseIf scaleImage < 5 Then '値の最低値を0に限定する。

        '            scaleImage = 5 '最小値にする
        '            MsgBox("これ以上暗くなりません!")
        '        End If
    End Function
    '解析-輝度調整「暗く」

    Public Sub BtnAnaDark()
        'If scaleImage >= ddImage Then
        'scaleImage = scaleImage - ddImage
        If Scaling > 0.1 Then
            Scaling -= ScalingAdder
            ScalingFacter = Scaling
        Else
            MsgBox("これ以上暗くなりません!")
        End If
        Debug.Print("暗" & scaleImage)
        AllDraw()
    End Sub
    '    '解析－「3D点削除」

    Public Sub BtnAnaDel3DPoint()
        flgDraw = True
        Dim i As Integer = ImageListView.SelectedIndices.Item(0)
#If HALCON = 11 Then
        Dim DelRegion As HObject = Nothing
        HOperatorSet.GenEmptyObj(DelRegion)
        ClearHObject(DelRegion)

        DrawDeleteRegion(winpreview, DelRegion)
        objFBM.DeletePoint(i, DelRegion)

        DispOneObjByIndex(winpreview, i)
        ClearHObject(DelRegion)
        objFBM.SaveToMeasureDataDB(objFBM.ProjectPath & "\")
        YMC_3DViewReDraw_NoKousin(m_strDataBasePath)
#End If
Exit_Sub:
        flgDraw = False
    End Sub

    '解析－「バンドル」

    Public Sub RunBundleAdj()
        Dim sw As New System.Diagnostics.Stopwatch()
        sw.Start()
        '--rep.        FBMlib.T_treshold = CDbl(imgToolStripTextBox1.Text)
        FBMlib.T_treshold = CDbl(100)
        objFBM.RunBundleAdjOnly()
        objFBM.SaveToMeasureDataDB(objFBM.ProjectPath & "\")
        sw.Stop()
        YMC_3DViewReDraw_NoKousin(m_strDataBasePath)
        MsgBox("バンドル調整処理完了しました。(" & sw.Elapsed.TotalSeconds.ToString & "秒)", MsgBoxStyle.OkOnly, "確認")
        flgManualScaleAndOffset = True 'SUSANO ADD START 20160526

        '''  MenshinSy用の計算()
        ' YCM_Offset_GenData()

    End Sub
#If HALCON = 11 Then


    Public Sub MenshinSy用の計算()
        Dim X As New FBMlib.Point3D
        Dim vecX(nLookPoints - 1) As GeoPoint

        Dim i As Integer
        Dim nP As Integer = 0

        For i = 0 To nLookPoints - 1
            If gDrawPoints(i).LabelName.StartsWith("P") = True Then
                Dim T As New FBMlib.Point3D(New HTuple(gDrawPoints(i).Real_x), New HTuple(gDrawPoints(i).Real_y), New HTuple(gDrawPoints(i).Real_z))
                X.ConcatToMe(T)
                vecX(nP) = New GeoPoint
                vecX(nP).setXYZ(T.X, T.Y, T.Z)

                nP += 1
            End If
        Next
        ReDim Preserve vecX(nP - 1)
        Dim meanX As New FBMlib.Point3D
        meanX = X.GetMean()
        Dim X_minus_meanX As New FBMlib.Point3D
        X_minus_meanX = X.SubPoint3d(meanX)
        Dim A1 As New FBMlib.Point3D(X_minus_meanX.GetMultedByTuple(X_minus_meanX.X))
        Dim A2 As New FBMlib.Point3D(X_minus_meanX.GetMultedByTuple(X_minus_meanX.Y))
        Dim A3 As New FBMlib.Point3D(X_minus_meanX.GetMultedByTuple(X_minus_meanX.Z))
        A1 = A1.GetSummary()
        A2 = A2.GetSummary()
        A3 = A3.GetSummary()
        Dim CovMatrixVal(8) As Double
        Dim CovMatrixID As New Object
        Dim EigenValID As New Object
        Dim EigenValMat As New Object
        Dim EigenVectorID As New Object
        Dim EigenVectorMat As New Object
        CovMatrixVal(0) = A1.X
        CovMatrixVal(1) = A1.Y
        CovMatrixVal(2) = A1.Z
        CovMatrixVal(3) = A2.X
        CovMatrixVal(4) = A2.Y
        CovMatrixVal(5) = A2.Z
        CovMatrixVal(6) = A3.X
        CovMatrixVal(7) = A3.Y
        CovMatrixVal(8) = A3.Z
        HOperatorSet.CreateMatrix(3, 3, CovMatrixVal, CovMatrixID)
        HOperatorSet.EigenvaluesSymmetricMatrix(CovMatrixID, "true", EigenValID, EigenVectorID)
        HOperatorSet.GetFullMatrix(EigenValID, EigenValMat)
        HOperatorSet.GetFullMatrix(EigenVectorID, EigenVectorMat)
        Dim BestPlaneNormalVector As New FBMlib.Point3D(EigenVectorMat(0), EigenVectorMat(3), EigenVectorMat(6))
        Dim Origin As New GeoPoint
        Origin.setXYZ(meanX.X, meanX.Y, meanX.Z)
        Dim BPnormalvector As New GeoVector
        BPnormalvector.setXYZ(BestPlaneNormalVector.X, BestPlaneNormalVector.Y, BestPlaneNormalVector.Z)
        Dim CSmat As New GeoMatrix
        CSmat = GeoMatrix_ByOriginNormal(Origin, BPnormalvector)
        Dim strText As String = ""

        For i = 0 To nP - 1
            Dim Tvec As New GeoPoint
            Tvec = vecX(i).GetTransformed(CSmat)
            strText = strText & "P" & i + 1 & "," & vecX(i).x & "," & vecX(i).y & "," & vecX(i).z & ",-->," &
            Tvec.x & "," & Tvec.y & "," & Tvec.z & vbNewLine
        Next
        My.Computer.FileSystem.WriteAllText(objFBM.ProjectPath & "\Menshin.csv", strText, False)

        HOperatorSet.ClearMatrix(CovMatrixID)
        HOperatorSet.ClearMatrix(EigenValID)
        HOperatorSet.ClearMatrix(EigenVectorID)
    End Sub
#End If
    ''解析－「シングルターゲット番号付け」

    'Private Sub imgToolStripButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim sw As New System.Diagnostics.Stopwatch()
    '    sw.Start()
    '    '--rep.       FBMlib.T_treshold = CDbl(imgToolStripTextBox1.Text)
    '    FBMlib.T_treshold = CDbl(100)
    '    objFBM.SingleTargetNumberingOnly()
    '    objFBM.SaveToMeasureDataDB(objFBM.ProjectPath & "\")
    '    sw.Stop()
    '    YMC_3DViewReDraw(m_strDataBasePath)
    '    MsgBox("シングルターゲット番号付け処理完了しました。(" & sw.Elapsed.TotalSeconds.ToString & "秒)", MsgBoxStyle.OkOnly, "確認")
    'End Sub
#End Region

#Region "メニュー関連（画面切替）"
    '-------------------------------
    '     If flgPreview = True Then
    '        flgPreview = False
    '        Panel1.Visible = False
    '        FourImageTableLayoutPanel.Visible = True
    ''       Me.Text = "３画像表示"
    '    Else
    '        flgPreview = True
    '        Panel1.Visible = True
    '        FourImageTableLayoutPanel.Visible = False
    ''       Me.Text = "画像プレビュー"
    '    End If
    '-------------------------------
    '--画面構成の変更
    ''Private Sub Main_Tab_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Main_Tab.SelectedIndexChanged
    ''    If Main_Tab.SelectedIndex = 1 Then
    ''        'View_Dilaog = New YCM_3DView

    ''        'Me.SplitContainer2.Panel1.Controls.Add(View_Dilaog)
    ''        View_Dilaog.Show()
    ''        TBox_Data.Focus()
    ''        View_Dilaog.MainProc()
    ''    End If
    ''End Sub
    'Private Sub Button1_Click(sender As System.Object, e As System.EventArgs)
    '    Call FileOpen()
    '    Call ChgViewImg1_3DView()
    '    '        Call ChgViewImg1()
    'End Sub

    '------------------------------------------------
    'SplitContainer1.Panel1：リボンメニュー領域
    'SplitContainer1.Panel2：表示領域

    'SplitContainer2.Panel1：画像

    'halwinSplitContnr.Panel1：画像

    'halwinSplitContnr.Panel2：イメージリスト


    'SplitContainer2.Panel2：3Dビュー全体

    'SplitContainer3.Panel1：3Dビュー
    'SplitContainer3.Panel2：座標値リスト

    '------------------------------------------------
    '画像１

    Public Sub ChgViewImage1()
        'TimeMonStart()
        Try
            '画像パネルを最大化、3Dビューを最小化
            Call ImagePanelSetMaxSize()
            '画像１表示    
            Panel1.Visible = True
            flgPreview = True
            FourImageTableLayoutPanel.Visible = False
            '(20140603 Tezuka ADD) ４画面時表示状態の初期化
            If flgView14 = True Or flgView4 = True Then
                AxHWinAllLeave()
            End If
            DispObjByIndex(-1)
            flgView4 = False
            flgView14 = False
        Catch ex As Exception
            ' MsgBox(ex.ToString)
        End Try
        'TimeMonEnd()
    End Sub
    '画像４

    Public Sub ChgViewImage4()
        '画像パネルを最大化、3Dビューを最小化
        Call ImagePanelSetMaxSize()
        '画像４表示    
        Panel1.Visible = False
        flgPreview = False
        FourImageTableLayoutPanel.Visible = True
        '(20140603 Tezuka ADD) ４画面時表示状態の初期化
        If flgView14 = True Then
            AxHWinAllLeave()
        End If
        If flgView4 = False Then
            DispObjByIndex(-1)
        End If
        flgView4 = True
        flgView14 = False
    End Sub
    '画像１＋3DView
    Public Sub ChgViewImage1_3DView()
        '画像パネルと3Dビューパネルを表示
        Call InitialImageViewPanel()
        '画像１表示    
        Panel1.Visible = True
        flgPreview = True
        FourImageTableLayoutPanel.Visible = False
        '(20140603 Tezuka ADD) ４画面時表示状態の初期化
        If flgView14 = True Or flgView4 = True Then
            AxHWinAllLeave()
        End If
        DispObjByIndex(-1)
        flgView4 = False
        flgView14 = False
    End Sub
    '画像４＋3DView
    Public Sub ChgViewImage4_3DView()
        '画像パネルと3Dビューパネルを表示
        Call InitialImageViewPanel()
        '画像４表示    
        Panel1.Visible = False
        flgPreview = False
        FourImageTableLayoutPanel.Visible = True
        '(20140603 Tezuka ADD) ４画面時表示状態の初期化
        If flgView4 = True Then
            AxHWinAllLeave()
        End If
        If flgView14 = False Then
            DispObjByIndex(-1)
        End If
        flgView4 = False
        flgView14 = True
    End Sub
    '3DView
    Public Sub ChgView3DView()
        TimeMonStart()

        '画像パネルを最小化、3Dビューパネルを最大化
        Call ViewPanelSetMaxSize()
        '(20140603 Tezuka ADD) ４画面時表示状態の初期化
        If flgView14 = True Or flgView4 = True Then
            AxHWinAllLeave()
        End If
        DispObjByIndex(-1)
        flgView4 = False
        flgView14 = False

        TimeMonEnd()
    End Sub

    '画像表示を最大化

    Private Sub ImagePanelSetMaxSize()
        '3D操作ビューを最小化
        SplitContainer3.SplitterDistance = 0
        '画像パネルを最大化
        halwinSplitContnr.SplitterDistance = halwinSplitContnr.Height * 0.8
        If m_bDataPointShow = False Then
            SplitContainer2.SplitterDistance = SplitContainer1.Panel2.Width - 409
        Else
            SplitContainer2.SplitterDistance = SplitContainer1.Panel2.Width
        End If

    End Sub
    '3Dビュー表示を最大化

    Private Sub ViewPanelSetMaxSize()
        '画像パネルを最小化
        SplitContainer2.SplitterDistance = 0
        '3Dビューパネルを最大化

        '13.1.16修正==================================================================================================================
        If (m_bDataPointShow) Then    '=True：座標値一覧（非表示）

            'If (binfrmdataview And m_bDataPointShow) Then    '=True：座標値一覧（表示）

            SplitContainer3.SplitterDistance = SplitContainer1.Panel2.Width   '座標値リスト幅なし

            SplitContainer4.SplitterDistance = SplitContainer1.Panel2.Height - 80   '
            View_Dilaog.Size = New System.Drawing.Size(MainFrm.SplitContainer3.Panel1.Width, MainFrm.SplitContainer4.Panel1.Height)
        Else '=False：座標値一覧（表示）

            If (SplitContainer2.Panel2.Width > 430) Then '600
                SplitContainer3.SplitterDistance = SplitContainer1.Panel2.Width - 409   '座標値リスト幅（元は434）

                SplitContainer4.SplitterDistance = SplitContainer1.Panel2.Height - 80
            Else
                SplitContainer3.SplitterDistance = SplitContainer2.Panel2.Width * (3.0# / 4.0#)   '座標値リスト幅

                SplitContainer4.SplitterDistance = SplitContainer1.Panel2.Height - 80
            End If
            View_Dilaog.Size = New System.Drawing.Size(MainFrm.SplitContainer3.Panel1.Width, MainFrm.SplitContainer4.Panel1.Height)
        End If

        '13.1.16修正==================================================================================================================

        ''13.1.16以前（修正前）=======================================================================================================
        'Dim bIsVertexDisp As Boolean
        'bIsVertexDisp = Data_Point.DGV_DV.RowCount > 0
        'bIsVertexDisp = bIsVertexDisp And binfrmdataview And m_bDataPointShow
        'If (bIsVertexDisp) Then    '=True：座標値一覧（表示）

        '    'If (binfrmdataview And m_bDataPointShow) Then    '=True：座標値一覧（表示）

        '    SplitContainer3.SplitterDistance = SplitContainer1.Panel2.Width - 400   '座標値リスト幅（1/16以前）

        '    SplitContainer4.SplitterDistance = SplitContainer1.Panel2.Height - 80   '
        '    YCM_3DView.Size = New System.Drawing.Size(MainFrm.SplitContainer3.Panel1.Width, MainFrm.SplitContainer4.Panel1.Height)
        'Else
        '    SplitContainer3.SplitterDistance = SplitContainer1.Panel2.Width - 10  '座標値リストなし

        '    SplitContainer4.SplitterDistance = SplitContainer1.Panel2.Height - 80   '
        '    YCM_3DView.Size = New System.Drawing.Size(MainFrm.SplitContainer3.Panel1.Width, MainFrm.SplitContainer4.Panel1.Height)
        'End If
        ''13.1.16以前（修正前）=======================================================================================================

        'TBox_Data.Focus()
        'View_Dilaog.MainProc()
    End Sub
    '画像表示＋3DView表示
    Private Sub InitialImageViewPanel()
        '画像、3Dビューパネルを1/2
        SplitContainer2.SplitterDistance = SplitContainer1.Panel2.Width / 2

        'イメージリスト分、座標値リスト分
        '13.1.16修正==================================================================================================================
        If (m_bDataPointShow) Then    '=True：座標値一覧（非表示）

            'If (binfrmdataview And m_bDataPointShow) Then    '=True：座標値一覧（表示）

            halwinSplitContnr.SplitterDistance = SplitContainer2.Panel1.Width * (4.0# / 5.0#)   'イメージリスト

            SplitContainer3.SplitterDistance = SplitContainer2.Panel2.Width '座標値リスト幅なし
            SplitContainer4.SplitterDistance = SplitContainer1.Panel2.Height - 80
            View_Dilaog.Size = New System.Drawing.Size(MainFrm.SplitContainer4.Panel1.Width, MainFrm.SplitContainer4.Panel1.Height)
        Else  '=False：座標値一覧（表示）

            halwinSplitContnr.SplitterDistance = SplitContainer2.Panel1.Width * (4.0# / 5.0#)   'イメージリスト

            If (SplitContainer2.Panel2.Width > 600) Then '600
                SplitContainer3.SplitterDistance = SplitContainer2.Panel2.Width - 400   '座標値リスト幅

            Else
                SplitContainer3.SplitterDistance = SplitContainer2.Panel2.Width * (3.0# / 4.0#)   '座標値リスト幅

            End If
            View_Dilaog.Size = New System.Drawing.Size(MainFrm.SplitContainer4.Panel1.Width, MainFrm.SplitContainer4.Panel1.Height)
        End If
        '13.1.16修正==================================================================================================================

        ''13.1.16以前（修正前）=======================================================================================================
        'Dim bIsVertexDisp As Boolean
        'bIsVertexDisp = Data_Point.DGV_DV.RowCount > 0
        'bIsVertexDisp = bIsVertexDisp And binfrmdataview And m_bDataPointShow '（1/16以前）

        '論理式（Data_Point.DGV_DV.RowCount > 0かつbinfrmdataview=Trueかつm_bDataPointShow=True）⇒初期（『開く』の後）

        'If (bIsVertexDisp) Then    '=True：座標値一覧（表示）

        '    'If (binfrmdataview And m_bDataPointShow) Then    '=True：座標値一覧（表示）

        '    halwinSplitContnr.SplitterDistance = SplitContainer2.Panel1.Width * (4.0# / 5.0#)   'イメージリスト

        '    If (SplitContainer2.Panel2.Width > 600) Then
        '        SplitContainer3.SplitterDistance = SplitContainer2.Panel2.Width - 400   '座標値リスト幅（1/16以前）

        '    Else
        '        SplitContainer3.SplitterDistance = SplitContainer2.Panel2.Width * (3.0# / 4.0#)   '座標値リスト幅（1/16以前）

        '    End If
        '    YCM_3DView.Size = New System.Drawing.Size(MainFrm.SplitContainer4.Panel1.Width, MainFrm.SplitContainer4.Panel1.Height)
        'Else
        '    halwinSplitContnr.SplitterDistance = SplitContainer2.Panel1.Width * (4.0# / 5.0#)   'イメージリスト

        '    SplitContainer3.SplitterDistance = SplitContainer2.Panel2.Width - 10  '座標値リストなし

        '    YCM_3DView.Size = New System.Drawing.Size(MainFrm.SplitContainer4.Panel1.Width, MainFrm.SplitContainer4.Panel1.Height)
        'End If
        ''13.1.16以前（修正前）=======================================================================================================

        ''★20121115山本さん追加
        'Dim bounds As RectangleF
        'bounds = SplitContainer2.Panel1.Region.GetBounds(()
    End Sub

#End Region


#Region "画像解析部分"
    Dim winpreview As HWindow
    Dim win1 As HWindow
    Dim win2 As HWindow
    Dim win3 As HWindow
    Dim win4 As HWindow

    Dim intW As Integer
    Dim intH As Integer
    Dim intZW As Integer
    Dim intZH As Integer
    '20121113Ctrl+WouseWheel：輝度調整
    '★2012101026「scaleImage」「ddImage」追加
    Public scaleImage As Double = 100
    Dim ddImage As Double = 10
    '★20121113「Mean」「A」追加
    Public Mean As HTuple = Nothing
    Public Ctrl_MouseWheel As Integer = 0
    Public Scaling As Double = 10.0
    Public ScalingAdder As Double = 0.1
    Public ScalingFacter As Double = 50

    Dim ZoomFactor As Integer = 1.5  '--10
    Dim minZoomSize As Integer = 5 '最小表示範囲ＰＩＸＥＬ
    Dim flgDraw As Boolean = False
    Public Const MinTolerance As Integer = 50
    Dim flgDispVal As Integer
    Dim flgZoom As Boolean = False
    Dim flgPreview As Boolean = True
    Public WithEvents objFBM As FBMlib.FeatureImage

    '(20140603 Tezuka ADD) ４画面のランダム表示対応
    Dim flgView4 As Boolean = False     'イメージのみ４画面表示
    Dim flgView14 As Boolean = False    'イメージ４画面＋３ＤＶｉｅw
    Dim dispWin(4) As Integer           '４画面表示時に各画面に現在表示されている画像Ｎｏ
    Dim prevWin As Integer = -1

    '201702223 baluu add start '20170224 baluu edit start
    Dim regionR1 As Object, regionR2 As Object, regionC1 As Object, regionC2 As Object
    Public pSelectedImageIndex As Integer = -1 'TOBENOTED
    Dim flgDragStarted As Boolean = False
    '201702223 baluu add end '20170224 baluu edit start

    Public Enum flgDisp
        flgImage = 1
        flgFeature = 2
        flgRansac = 3
        flgObject = 4
        flgCT = 5
    End Enum

    Public Sub NewMeasureData(ByVal strPath As String)
        Dim strCamParam As String = My.Settings.strCamparam
        '(20140528 Tezuka ADD)
        If WorksD.CameraD.camera_path.Length() > 0 Then
            strCamParam = WorksD.CameraD.camera_path
        End If
        objFBM = New FBMlib.FeatureImage(strCamParam)

        'MsgBox("NewMeasureData")        '2013/06/07 ADD
        gfrmProgressBar = New frmProgressBar

        gfrmProgressBar.Show()
        gfrmProgressBar.Label1.Text = "画像読込中"
        Me.Refresh()
        gfrmProgressBar.Refresh()
        objFBM.ProjectPath(True) = strPath
        Dim iRet As Integer
        iRet = objFBM.ErrCode
        If (iRet <> 0) Then
            Select Case iRet
                Case 1
                    MsgBox("指定されたフォルダには画像が入っていません", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
                Case Else
                    MsgBox("objFBM.ProjectPath　Error", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
            End Select
            gfrmProgressBar.Close()
            Exit Sub
        End If
        gfrmProgressBar.Close()
        Dim i As Integer
        Dim n As Integer = CInt(objFBM.lstImages.Count)
        ImageListView.Clear()
        ImgLst.Images.Clear()
        For i = 0 To n - 1
            Dim smallimage As System.Drawing.Image
            smallimage = System.Drawing.Image.FromFile(objFBM.lstImages.Item(i).ImageSmallFullPath & ".jpg")
            ImgLst.Images.Add(smallimage)
            ' ListView1.Items.Add(objFBM.lstImages.Item(i).ImageName, i)
            ImageListView.Items.Add(i + 1 & ": " & objFBM.lstImages.Item(i).ImageName, i)
        Next
        intW = CInt(objFBM.hv_Width) - 1
        intH = CInt(objFBM.hv_Height) - 1
        AllWindowSetPart()

        'Me.Text = "YCM　Ver 1.0" & "  " & strPath
        Me.Text = FormTitle & " " & strPath

    End Sub

    Private Sub NewMeasureDataReading(ByVal sender As Object, ByVal e As FBMlib.MessageEventArgs) Handles objFBM.ImageReaded
        gfrmProgressBar.ProgressBar1.Maximum = e.ImageCount
        gfrmProgressBar.ProgressBar1.Value = e.ImageIndex

    End Sub

    Public Sub OpenMeasureNaibuData(ByVal strPath As String)

        'Dim strCamParam As String = My.Settings.strCamparam
        Dim strCamParam As String = My.Application.Info.DirectoryPath
        Dim str As String = AfterPointWords(My.Settings.strCamparam)
        strCamParam = strCamParam & str
        'MsgBox(strCamParam)
        '(20140528 Tezuka ADD)
        If Not WorksD.CameraD Is Nothing Then 'SUURI ADD 20141023 cameraDのNOTHINGチェック
            If WorksD.CameraD.camera_path.Length() > 0 Then
                strCamParam = WorksD.CameraD.camera_path
            End If
        End If
        objFBM = New FBMlib.FeatureImage(strCamParam)

        objFBM.ProjectPath(False) = strPath
        'Me.Text = Me.Text & "   " & strPath
        objFBM.ReadProjectData()
        objFBM.ReadFromMeasureDataDB(strPath)

    End Sub
    Public Sub OpenMeasureData(ByVal strPath As String)
        If strPath <> "" Then
            OpenMeasureNaibuData(strPath)

            Dim i As Integer
            If objFBM.lstImages Is Nothing Then
                Exit Sub
            End If

            Dim n As Integer = CInt(objFBM.lstImages.Count)
            ImageListView.Clear()
            ImgLst.Images.Clear()
            For i = 0 To n - 1

                ''H25.6.26修正前=================================================================================
                'Dim smallimage As System.Drawing.Image
                'smallimage = System.Drawing.Image.FromFile(objFBM.lstImages.Item(i).ImageSmallFullPath & ".jpg")
                'ImgLst.Images.Add(smallimage)
                '' ListView1.Items.Add(objFBM.lstImages.Item(i).ImageName, i)
                'ImageListView.Items.Add(i + 1 & ": " & objFBM.lstImages.Item(i).ImageName, i)
                'ImageListView.Items(i).Checked = True
                ''H25.6.26修正前=================================================================================

                ''H25.6.26修正後=================================================================================
                Dim smallimage As System.Drawing.Image
                smallimage = System.Drawing.Image.FromFile(objFBM.lstImages.Item(i).ImageSmallFullPath & ".jpg")
                ImgLst.Images.Add(smallimage)
                ' ListView1.Items.Add(objFBM.lstImages.Item(i).ImageName, i)
                ImageListView.Items.Add(i + 1 & ": " & objFBM.lstImages.Item(i).ImageName, i)

                'If objFBM.lstImages.Item(i).flgConnected = True Then
                '    ImageListView.Items(i).Checked = True
                'Else
                '    ImageListView.Items(i).Checked = False
                'End If
                ''H25.6.26修正後=================================================================================
            Next

            ImageListItemCheck() 'Yamada

            intW = CInt(objFBM.hv_CamparamFirst(10)) - 1
            intH = CInt(objFBM.hv_CamparamFirst(11)) - 1
            AllWindowSetPart()
            flgDispVal = flgDisp.flgCT
            'Me.Text = "YCM　Ver 1.0" & "  " & strPath
            Me.Text = FormTitle & " " & strPath
        End If

    End Sub


    Private Sub DispOneObjByIndex(ByVal win As HWindow, ByVal i As Integer)
        On Error Resume Next 'TEMP
        'Dim Op As New HALCONXLib.HOperatorSetX
        'Static previndex As Integer = 0
        'If previndex = i Then
        '    If flgZoom = False Then
        '        Exit Sub
        '    End If
        'Else
        '    previndex = i
        'End If
#If HALCON = 11 Then

        GC.Collect()
        GC.WaitForPendingFinalizers()
        With objFBM.lstImages
            HOperatorSet.SetColored(win, 12)
            DispImage(win, i)
            Select Case flgDispVal
                Case flgDisp.flgImage

                Case flgDisp.flgFeature
                    HOperatorSet.DispObj(.Item(i).hx_FeatureCross, win)
                Case flgDisp.flgRansac
                    'Ljj add 14/5/23
                    'HOperatorSet.DispObj(.Item(i).Ransac3ImagePoints(2).hx_RansacCross, win)
                Case flgDisp.flgObject
                    HOperatorSet.DispObj(.Item(i).hx_FeatureCross, win)
                    DispObjects(win, i)
                    ' DispTotalObject(win4, ImageListView.SelectedIndices.Item(0))
                Case flgDisp.flgCT
                    'HOperatorSet.DispObj(.Item(i).hx_FeatureCross, win)
                    DispCT(win, i)
            End Select

            HOperatorSet.SetColored(win, 12)
        End With
#End If
    End Sub

    '(20140603 Tezuka ADD) 画像表示（４画面）
    ' i<0の時はnextWinの初期化を行うのみ
    Private Sub DispObjByIndex(ByVal i As Integer)
        On Error Resume Next
        Static previndex As Integer = -1
        Static nextWin As Object = win1      '(20140602 Tezuka ADD) 次に更新するWindow
        Dim j As Integer
        'Static dispWin(4) As Integer

        'nextWin初期化(画面切り替わり時)
        If i < 0 Then
            nextWin = Nothing
            If dispWin(1) >= 0 Then
                DispOneObjByIndex(win1, dispWin(1))
            End If
            If dispWin(2) >= 0 Then
                DispOneObjByIndex(win2, dispWin(2))
            End If
            If dispWin(3) >= 0 Then
                DispOneObjByIndex(win3, dispWin(3))
            End If
            If dispWin(4) >= 0 Then
                DispOneObjByIndex(win4, dispWin(4))
            End If
            prevWin = -1
            Exit Sub
        Else
            'For j = 0 To 4
            '    If dispWin(j) = i Then
            '        Exit Sub
            '    End If
            'Next j
        End If

        '前回と同一画面の時は更新しない
        If previndex = i Then
            Exit Sub
        Else
            previndex = i
        End If

        '画面表示実行
        If nextWin Is Nothing Then
            nextWin = win1
        End If
        'DispOneObjByIndex(nextWin, i) '20160601 Kiryu コメントアウト

        '次に表示する画面の更新
        'Select Case nextWin
        '    Case win1
        '        dispWin(1) = i
        '        nextWin = win2
        '    Case win2
        '        dispWin(2) = i
        '        nextWin = win3
        '    Case win3
        '        dispWin(3) = i
        '        nextWin = win4
        '    Case win4
        '        dispWin(4) = i
        '        nextWin = win1
        '    Case Else
        '        nextWin = Nothing
        'End Select

    End Sub

    'Private Sub DispObjByIndex(ByVal i As Integer)
    '    On Error Resume Next
    '    Static previndex As Integer = 5
    '    If previndex = i Then
    '        'Exit Sub
    '    Else
    '        previndex = i
    '    End If

    '    If i < 3 Then
    '        i = 3
    '    End If

    '    DispOneObjByIndex(win1, i - 3)
    '    DispOneObjByIndex(win2, i - 2)
    '    DispOneObjByIndex(win3, i - 1)
    '    DispOneObjByIndex(win4, i)

    'End Sub

    Private Sub DispImage(ByVal win As HWindow, ByVal i As Integer)
        'On Error Resume Next
#If HALCON = 11 Then

        With objFBM.lstImages
            Static hImage As HObject = Nothing
            Dim hScaleImage As HObject = Nothing
            Static ii As Integer = -1

            If ii = i Then

            Else

                HOperatorSet.GenEmptyObj(hImage)
                ClearHObject(hImage)
                hImage = .Item(i).hx_Image
                ii = i
            End If
            HOperatorSet.Intensity(hImage, hImage, Mean, New HTuple)
            If Mean > Double.Epsilon Then
                'HOperatorSet.ScaleImage(hImage, hScaleImage, Scaling * Scaling * 0.1, 0) ???
                'HOperatorSet.DispObj(hScaleImage, win)
                Try
                    HOperatorSet.ScaleImage(hImage, hScaleImage, scaleImage / Mean, 0)
                    ' HOperatorSet.ScaleImage(hImage, hScaleImage, Scaling * Scaling * 0.1, 0)
                    HOperatorSet.DispObj(hScaleImage, win)
                Catch ex As Exception
                    HOperatorSet.ScaleImage(hImage, hScaleImage, Scaling * Scaling * 0.1, 0)
                    HOperatorSet.DispObj(hScaleImage, win)
                End Try
            Else
                HOperatorSet.DispObj(hImage, win)
            End If
            'HOperatorSet.SetTposition(win, 0, 0)
            Dim R1 As Object = Nothing
            Dim C1 As Object = Nothing
            HOperatorSet.GetPart(win, R1, C1, New HTuple, New HTuple)
            'SetPartで表示される画像部分を変更⇒GetPartで表示された部分の左上、右下の行列の値を返す
            HOperatorSet.SetTposition(win, R1, C1)
            'テキストカーソルの位置を指定

            HOperatorSet.WriteString(win, .Item(i).ImageName)
        End With
#End If
    End Sub
    '★20121026 MouseWheelで輝度調整　or 拡大縮小

    Public Sub Form1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case e.KeyCode = Keys.S
                ' このフォームで現在アクティブなコントロールを取得する
                Dim cControl As Control = Me.ActiveControl

                ' 取得できた場合のみ、そのコントロールの名前を表示する
                If Not cControl Is Nothing Then
                    MsgBox(cControl.Name)
                End If
        End Select


        '20121113 KeyDownイベント：Ctrl⇒輝度調整
        'If e.KeyCode = Keys.ControlKey Then
        '    Ctrl_MouseWheel = 1
        '    Debug.Print("Main-Down")
        'End If

        ''20121113以前--------------------------------------------------------
        ''(A)で明るく

        ''Me.KeyPreview = True
        'If e.KeyCode = Keys.A Then
        '    If scaleImage < 1500 Then
        '        scaleImage = scaleImage + ddImage
        '    Else
        '        MsgBox("これ以上明るくなりません!")
        '    End If
        '    Debug.Print("A" & scaleImage)
        '    AllDraw()
        'End If
        ''(B)で暗くする
        'If e.KeyCode = Keys.B Then
        '    Debug.Print("B" & scaleImage)
        '    If scaleImage > ddImage Then
        '        scaleImage = scaleImage - ddImage
        '    Else
        '        MsgBox("これ以上暗くなりません!")
        '    End If
        '    AllDraw()
        'End If
        '20121113以前--------------------------------------------------------
    End Sub
    '★20121026 MouseWheelで輝度調整　or 拡大縮小
    Public flgCreateSTCancel As Boolean = False
    Public flgCreateSTEnter As Boolean = False
    Public flgKoushin3DPoint As Boolean = False
    Private Sub YCM_MainFrame_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        'Ctrl_MouseWheel = 0
        If e.KeyCode = Keys.Enter Then

        End If
        If e.KeyCode = Keys.Escape Then
            If flgManualST = True Then
                flgCreateSTCancel = True
            End If
        End If
        Debug.Print("Main-UP")
    End Sub
    Public Sub AllDraw()
        Try
            Dim i As Integer = ImageListView.SelectedIndices.Item(0)


            If flgPreview = True Then
                DispOneObjByIndex(winpreview, i)
            Else
                '20170216 baluu del start
                'If i < 2 Then
                '    i = 2
                'End If
                '20170216 baluu del end
                DispOneObjByIndex(win1, dispWin(1)) '20170216 baluu edit
                DispOneObjByIndex(win2, dispWin(2)) '20170216 baluu edit
                DispOneObjByIndex(win3, dispWin(3)) '20170216 baluu edit
                DispOneObjByIndex(win4, dispWin(4)) '20170216 baluu edit
            End If
        Catch ex As Exception

        End Try
    End Sub
    '★20121226バトスーリさん(画像操作ビューと3Dビューで図形を同期)
    'ToDo Kiryu　一つの関数内で複数のことをやりすぎているのでサブ関数に分割したい
    Private Sub DispCT(ByVal win As HWindow, ByVal index As Integer)
#If HALCON = 11 Then
        Try
            Dim obj As New HObject
            'GenEmptyObj：空のオブジェクトtupleを作成
            HOperatorSet.GenEmptyObj(obj)
            'SetFont：テキスト出力に対して用いるフォントを設定する。

            'Add Kiryu Ctrl+マウスホイールで文字サイズ変更
            HOperatorSet.SetFont(win, LabelFont + FontSize.ToString() + "-")
            Dim DispMismatchCT As Integer = GetPrivateProfileInt("TargetDetectParam", "DispMismatchCT", -1, My.Application.Info.DirectoryPath & "\vform.ini")

            'SetColor：出力カラーを設定する。

            Dim R1 As Object = Nothing, R2 As Object = Nothing, C1 As Object = Nothing, C2 As Object = Nothing
            HOperatorSet.GetPart(win, R1, C1, R2, C2)
            Dim wW As Double, wH As Double
            HOperatorSet.GetWindowExtents(win, New HTuple, New HTuple, wW, wH)
            Dim wScale = (C2 - C1) / wW
            Dim hScale = (R2 - R1) / wH
            'HOperatorSet.SetColor(win, "red") '20170216 baluu del
            Dim TD As FBMlib.TargetDetect = objFBM.lstImages.Item(index).Targets

            For Each CT As FBMlib.CodedTarget In TD.lstCT
                If CT.CT_ID = -1 Then
                    Continue For
                End If
                ClearHObject(obj)
                'GenCrossContourXld:各入力点に対して、十字の XLD オブジェクトを生成する。
                HOperatorSet.SetColor(win, "red")
                HOperatorSet.GenCrossContourXld(obj, CT.CenterPoint.Row, CT.CenterPoint.Col, FBMlib.CrossSize, FBMlib.CrossAngle)
                HOperatorSet.DispObj(obj, win)
                '20170216 baluu add start
                Dim label As String
                If CT.CurrentLabel = "" Then
                    label = CT.systemlabel
                Else
                    label = CT.CurrentLabel
                End If
                Dim W As Object = Nothing, H As Object = Nothing
                HOperatorSet.GetStringExtents(win, label, New HTuple, New HTuple, W, H)

                Dim RS As HTuple = CT.CenterPoint.Row
                Dim CS As HTuple = CT.CenterPoint.Col
                Dim RE As HTuple = CT.CenterPoint.Row + H * hScale
                Dim CE As HTuple = CT.CenterPoint.Col + W * wScale

                HOperatorSet.SetColor(win, "green")
                HOperatorSet.SetDraw(win, "fill")
                HOperatorSet.DispRectangle1(win, RS, CS, RE, CE)
                HOperatorSet.SetDraw(win, "margin")

                'SetTposition：テキストカーソルの位置を設定する。
                HOperatorSet.SetColor(win, "black")
                'HOperatorSet.SetTposition(win, CT.CenterPoint.Row - TLabelPosU * hScale, CT.CenterPoint.Col + TLabelPosR * wScale)
                HOperatorSet.SetTposition(win, CT.CenterPoint.Row, CT.CenterPoint.Col)
                '20170216 baluu add end
                'WriteString:ウインドウ内にテキストを出力する。
                HOperatorSet.WriteString(win, label)
                'CT.CurrentLabel = ""
                'For i = 0 To nLookPoints - 1
                '    If gDrawPoints(i).tid = CT.CT_ID And gDrawPoints(i).flgLabel = 1 Then
                '        CT.CurrentLabel = Trim(gDrawPoints(i).LabelName)
                '        Exit For
                '    End If
                'Next
                '20170216 baluu del start
                'If CT.CurrentLabel = "" Then
                '    'WriteString:ウインドウ内にテキストを出力する。
                '    HOperatorSet.WriteString(win, CT.systemlabel)
                'Else
                '    HOperatorSet.WriteString(win, CT.CurrentLabel)
                'End If
                '20170216 baluu del end

                'Dim t As Integer
                'For t = 0 To FBMlib.CodedTarget.CTnoSTnum - 1
                '    HOperatorSet.GenCrossContourXld(obj, CT.CT_Points.Row(t), CT.CT_Points.Col(t), FBMlib.CrossSize, FBMlib.CrossAngle)
                '    HOperatorSet.DispObj(obj, win)
                '    HOperatorSet.SetTposition(win, CT.CT_Points.Row(t), CT.CT_Points.Col(t))
                '    HOperatorSet.WriteString(win, "CT_" & CStr(CT.CT_ID) & "_" & t + 1)
                'Next

            Next


            'Kiryu S 20170414 認識失敗CTのパターン番号を表示(デバッグ用)
            If DispMismatchCT Then
                Dim orange As String = "#ff8c00"
                HOperatorSet.SetColor(win, orange)
                For Each MismatchCT As FBMlib.CodedTarget In TD.lstMismatchCT
                    ClearHObject(obj)
                    'GenCrossContourXld:各入力点に対して、十字の XLD オブジェクトを生成する。
                    Dim W As Object = Nothing, H As Object = Nothing
                    HOperatorSet.GetStringExtents(win, MismatchCT.MismatchCT_Name, New HTuple, New HTuple, W, H)

                    Dim RS As HTuple = MismatchCT.CenterPoint.Row
                    Dim CS As HTuple = MismatchCT.CenterPoint.Col
                    Dim RE As HTuple = MismatchCT.CenterPoint.Row + H * hScale
                    Dim CE As HTuple = MismatchCT.CenterPoint.Col + W * wScale

                    HOperatorSet.SetColor(win, orange)
                    HOperatorSet.SetDraw(win, "fill")
                    HOperatorSet.DispRectangle1(win, RS, CS, RE, CE)
                    HOperatorSet.SetDraw(win, "margin")
                    HOperatorSet.GenCrossContourXld(obj, MismatchCT.CenterPoint.Row, MismatchCT.CenterPoint.Col, FBMlib.CrossSize, FBMlib.CrossAngle)
                    HOperatorSet.DispObj(obj, win)
                    HOperatorSet.SetColor(win, "black")
                    'SetTposition：テキストカーソルの位置を設定する。
                    HOperatorSet.SetTposition(win, MismatchCT.CenterPoint.Row, MismatchCT.CenterPoint.Col)
                    If MismatchCT.CurrentLabel = "" Then
                        'WriteString:ウインドウ内にテキストを出力する。
                        HOperatorSet.WriteString(win, MismatchCT.MismatchCT_Name)
                    Else
                        HOperatorSet.WriteString(win, MismatchCT.CurrentLabel)
                    End If
                Next
            End If


            ''SetColor：出力カラーを設定する。

            HOperatorSet.SetColor(win, "blue") '画面操作ビューに青色の計測点を表示
            'Dim strName As Ls
            'HOperatorSet.SetColor(win, entset_line.color.strName) '画面操作ビューに3Dビューの作図属性で定義した属性の計測点を表示

            For Each ST As FBMlib.SingleTarget In TD.lstST
                If ST.P3ID = -1 Then
                    Continue For
                End If
                ClearHObject(obj) '?
                HOperatorSet.GenCrossContourXld(obj, ST.P2D.Row, ST.P2D.Col, FBMlib.CrossSize, FBMlib.CrossAngle)
                HOperatorSet.DispObj(obj, win)
                If ST.P3ID <> -1 Then 'P3ID：SingleTarget.vb

                    'HOperatorSet.SetTposition(win, ST.P2D.Row, ST.P2D.Col) '20170216 baluu del
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

                    '20170216 baluu add start
                    Dim label As String
                    If ST.currentLabel = "" Then
                        label = ST.systemlabel
                    Else
                        label = ST.currentLabel
                    End If

                    Dim W As Object = Nothing, H As Object = Nothing
                    HOperatorSet.GetStringExtents(win, label, New HTuple, New HTuple, W, H)

                    Dim RS As HTuple = ST.P2D.Row
                    Dim CS As HTuple = ST.P2D.Col
                    Dim RE As HTuple = ST.P2D.Row + H * hScale
                    Dim CE As HTuple = ST.P2D.Col + W * wScale

                    HOperatorSet.SetColor(win, "green")
                    HOperatorSet.SetDraw(win, "fill")
                    HOperatorSet.DispRectangle1(win, RS, CS, RE, CE)
                    HOperatorSet.SetDraw(win, "margin")
                    HOperatorSet.SetColor(win, "black")
                    HOperatorSet.SetTposition(win, ST.P2D.Row, ST.P2D.Col)
                    HOperatorSet.WriteString(win, label) '：SingleTarget.vb
                    '20170216 baluu add end
                    '20170216 baluu del start
                    'If ST.currentLabel = "" Then
                    '    HOperatorSet.WriteString(win, ST.systemlabel) '：SingleTarget.vb
                    'Else
                    '    HOperatorSet.WriteString(win, ST.currentLabel) '：SingleTarget.vb
                    'End If
                    '20170216 baluu del end
                End If
            Next
            ClearHObject(obj)


            '20170215 baluu add start
            If Not model_pick = 0 Then
                Dim foundCT As FBMlib.CodedTarget = Nothing
                Dim foundST As FBMlib.SingleTarget = Nothing
                For Each CT As FBMlib.CodedTarget In TD.lstCT
                    For il As Integer = 0 To gDrawPoints.Count - 1 Step 1
                        If CT.CT_ID = gDrawPoints(il).tid And il = model_pick + 1 Then
                            foundCT = CT
                            Exit For
                        End If
                    Next
                    If Not foundCT Is Nothing Then
                        Exit For
                    End If
                Next
                For Each ST As FBMlib.SingleTarget In TD.lstST
                    For il As Integer = 0 To gDrawPoints.Count - 1 Step 1
                        If ST.P2ID = gDrawPoints(il).tid And il = model_pick + 1 Then
                            foundST = ST
                            Exit For
                        End If
                    Next
                    If Not foundST Is Nothing Then
                        Exit For
                    End If
                Next
                HOperatorSet.SetColor(win, "yellow")
                HOperatorSet.SetDraw(win, "fill")
                If Not foundCT Is Nothing Then
                    Dim label As String
                    If foundCT.CurrentLabel = "" Then
                        label = foundCT.systemlabel
                    Else
                        label = foundCT.CurrentLabel
                    End If
                    Dim W As Object = Nothing, H As Object = Nothing
                    HOperatorSet.GetStringExtents(win, label, New HTuple, New HTuple, W, H)
                    Dim RS As Double = foundCT.CenterPoint.Row
                    Dim CS As Double = foundCT.CenterPoint.Col
                    Dim RE As Double = (foundCT.CenterPoint.Row + H * hScale).D
                    Dim CE As Double = (foundCT.CenterPoint.Col + W * wScale).D
                    HOperatorSet.DispRectangle1(win, RS, CS, RE, CE)
                    HOperatorSet.SetColor(win, "black")
                    HOperatorSet.SetTposition(win, foundCT.CenterPoint.Row, foundCT.CenterPoint.Col)
                    HOperatorSet.WriteString(win, label)
                ElseIf Not foundST Is Nothing Then
                    Dim label As String
                    If foundCT.CurrentLabel = "" Then
                        label = foundCT.systemlabel
                    Else
                        label = foundCT.CurrentLabel
                    End If
                    Dim W As Object = Nothing, H As Object = Nothing
                    HOperatorSet.GetStringExtents(win, label, New HTuple, New HTuple, W, H)
                    Dim RS As Double = foundCT.CenterPoint.Row
                    Dim CS As Double = foundCT.CenterPoint.Col
                    Dim RE As Double = foundCT.CenterPoint.Row + H * hScale
                    Dim CE As Double = foundCT.CenterPoint.Col + W * wScale
                    HOperatorSet.DispRectangle1(win, RS, CS, RE, CE)
                    HOperatorSet.SetColor(win, "black")
                    HOperatorSet.SetTposition(win, foundST.P2D.Row, foundST.P2D.Col)
                    HOperatorSet.WriteString(win, label) '：SingleTarget.vb
                End If
                HOperatorSet.SetColor(win, "blue")
                HOperatorSet.SetDraw(win, "margin")
            End If
            '20170215 baluu add end
            'nUserLines：表示するユーザーラインの数
            If nUserLines > 0 And entset_line.blnVisiable = True Then
                Dim HomMat3dInvert As Object = Nothing 'HomMat3dInvert:?
                Dim TransPnt As New FBMlib.Point3D 'TransPnt:移動する点?
                Dim ProjectedImagePnt As New FBMlib.ImagePoints
                Dim tRow As New HTuple
                Dim tCol As New HTuple

                Dim Row As Object = Nothing
                Dim Col As Object = Nothing
                'PoseToHomMat3d：3次元姿勢情報パラメーターを同次変換行列に変換する。

                ':同次3次元変換行列の逆行列を算出する。

                HOperatorSet.PoseToHomMat3d(objFBM.lstImages.Item(index).ImagePose.Pose, HomMat3dInvert)
                'HomMat3dInvert = lstImages.Item(index + 1).CommonHomMat3d
                'Dim hUserLines As New HObject
                Dim oooo As Object = objFBM.lstImages.Item(index).ImagePose.Pose
                HOperatorSet.HomMat3dInvert(HomMat3dInvert, HomMat3dInvert)
                'HOperatorSet.GenEmptyObj(hUserLines)

                For i = 0 To nUserLines - 1
                    tRow = New HTuple
                    tCol = New HTuple
                    HOperatorSet.AffineTransPoint3d(HomMat3dInvert, gDrawUserLines(i).startPnt.x, gDrawUserLines(i).startPnt.y, gDrawUserLines(i).startPnt.z,
                                           TransPnt.X, TransPnt.Y, TransPnt.Z)
                    HOperatorSet.Project3dPoint(TransPnt.X, TransPnt.Y, TransPnt.Z, objFBM.hv_CamparamOut, Row, Col)
                    HOperatorSet.TupleInsert(tRow, 0, Row, tRow)
                    HOperatorSet.TupleInsert(tCol, 0, Col, tCol)

                    HOperatorSet.AffineTransPoint3d(HomMat3dInvert, gDrawUserLines(i).endPnt.x, gDrawUserLines(i).endPnt.y, gDrawUserLines(i).endPnt.z,
                                           TransPnt.X, TransPnt.Y, TransPnt.Z)
                    HOperatorSet.Project3dPoint(TransPnt.X, TransPnt.Y, TransPnt.Z, objFBM.hv_CamparamOut, Row, Col)
                    HOperatorSet.TupleInsert(tRow, 1, Row, tRow)
                    HOperatorSet.TupleInsert(tCol, 1, Col, tCol)
                    Dim hTmpUserLine As HObject = Nothing
                    HOperatorSet.GenEmptyObj(hTmpUserLine)
                    HOperatorSet.GenContourPolygonXld(hTmpUserLine, tRow, tCol)
                    HOperatorSet.SetColor(win, YCM_GetColorInfoByCode(gDrawUserLines(i).colorCode).strHalconColorName) ''!!!!!!!!!!!!!!!!!!!!!!!!!
                    If gDrawUserLines(i).blnDelOnGlwin = False Then
                        HOperatorSet.SetLineStyle(win, YCM_GetLineTypeInfoByCode(gDrawUserLines(i).lineTypeCode).strHalconLineType)  ''20150401 Tezuka ADD
                        HOperatorSet.DispObj(hTmpUserLine, win)
                    End If
                    'HOperatorSet.ConcatObj(hUserLines, hTmpUserLine, hUserLines)
                    ClearHObject(hTmpUserLine)
                Next
                'HOperatorSet.DispObj(hUserLines, win)
                'ClearHObject(hUserLines)
            End If

            If (flgManualST = True Or flgClickPoint = True) And (Not tmpC3dSt Is Nothing) Then
                Dim objlastST As FBMlib.SingleTarget = Nothing
                Dim blnIsNo As Boolean = False
                For Each objST As FBMlib.SingleTarget In tmpC3dSt.lstST
                    If objST.ImageID - 1 = index Then
                        objlastST = objST
                        blnIsNo = True
                        Exit For
                    End If
                Next
                If blnIsNo = True Then
                    'HOperatorSet.DrawRectangle1(win, objlastST.P2D.Row - 50,
                    '                    objlastST.P2D.Col - 50,
                    '                   objlastST.P2D.Row + 50,
                    '                    objlastST.P2D.Col + 50)
                    Dim objtmpRec As HObject = Nothing
                    'Add Kiryu 20160420 TG点選択箇所の□サイズを変更　元：50
                    Dim intRecWidth As Integer = 50
                    HOperatorSet.GenRectangle1(objtmpRec, objlastST.P2D.Row - intRecWidth,
                                        objlastST.P2D.Col - intRecWidth,
                                       objlastST.P2D.Row + intRecWidth,
                                        objlastST.P2D.Col + intRecWidth)
                    HOperatorSet.DispObj(objtmpRec, win)

                End If
                Try
                    HOperatorSet.DispObj(objFBM.lstImages.Item(index).hx_EpiLineAsOtherPoint, win)
                Catch ex As Exception

                End Try
                'Try
                '    HOperatorSet.DispObj(objFBM.lstImages.Item(index).hx_EdgePS, win)
                'Catch ex As Exception
                '    ' MsgBox("EDGE表示失敗　" & ex.Message)
                'End Try
            End If

            If flgEdgeST = True And (Not tmpC3dSt Is Nothing) Then
                'Dim objlastST As FBMlib.SingleTarget = Nothing
                'Dim blnIsNo As Boolean = False
                'For Each objST As FBMlib.SingleTarget In tmpC3dSt.lstST
                '    If objST.ImageID - 1 = index Then
                '        objlastST = objST
                '        blnIsNo = True
                '        Exit For
                '    End If
                'Next
                'If blnIsNo = True Then
                '    'HOperatorSet.DrawRectangle1(win, objlastST.P2D.Row - 50,
                '    '                    objlastST.P2D.Col - 50,
                '    '                   objlastST.P2D.Row + 50,
                '    '                    objlastST.P2D.Col + 50)
                '    Dim objtmpRec As HObject = Nothing
                '    Dim intRecWidth As Integer = 50
                '    HOperatorSet.GenRectangle1(objtmpRec, objlastST.P2D.Row - intRecWidth,
                '                        objlastST.P2D.Col - intRecWidth,
                '                       objlastST.P2D.Row + intRecWidth,
                '                        objlastST.P2D.Col + intRecWidth)
                '    HOperatorSet.DispObj(objtmpRec, win)

                'End If
                Try
                    HOperatorSet.DispObj(objFBM.lstImages.Item(index).hx_EdgePS, win)
                Catch ex As Exception
                    ' MsgBox("EDGE表示失敗　" & ex.Message)
                End Try
                Try
                    HOperatorSet.DispObj(objFBM.lstImages.Item(index).hx_EdgeRec, win)
                Catch ex As Exception
                    ' MsgBox("EDGE表示失敗　" & ex.Message)
                End Try
            End If
            If (flgDeletePoint = True) And (Not tmpC3dSt Is Nothing) Then
                Dim objlastST As FBMlib.SingleTarget = Nothing
                Dim blnIsNo As Boolean = False
                For Each objST As FBMlib.SingleTarget In tmpC3dSt.lstST
                    If objST.ImageID - 1 = index Then
                        objlastST = objST
                        Dim objtmpRec As HObject = Nothing
                        Dim intRecWidth As Integer = 50
                        HOperatorSet.GenRectangle1(objtmpRec, objlastST.P2D.Row - intRecWidth,
                                            objlastST.P2D.Col - intRecWidth,
                                           objlastST.P2D.Row + intRecWidth,
                                            objlastST.P2D.Col + intRecWidth)
                        HOperatorSet.DispObj(objtmpRec, win)

                    End If
                Next
            End If
            'If (flgDeletePoint = True) And (Not tmpC3dSt Is Nothing) Then
            '    Dim objtmpRec As HObject = Nothing
            '    Dim intRecWidth As Integer = 50
            '    HOperatorSet.GenRectangle1(objtmpRec, objlastST.P2D.Row - intRecWidth,
            '                        objlastST.P2D.Col - intRecWidth,
            '                       objlastST.P2D.Row + intRecWidth,
            '                        objlastST.P2D.Col + intRecWidth)
            '    HOperatorSet.DispObj(objtmpRec, win)
            'End If
            '20170224 baluu add start
            If Not objFBM.lstImages.Item(index).Region Is Nothing Then
                HOperatorSet.SetLineWidth(win, 2)
                HOperatorSet.SetColor(win, "red")
                HOperatorSet.DispObj(objFBM.lstImages.Item(index).Region, win)
                HOperatorSet.SetLineWidth(win, 1)
            End If
            '20170224 baluu add end
            '20170711 SUURI update 20170322 baluu add start
            If Not hv_HalconObjectModel3d Is Nothing Then
                'Dim hv_PoseIn As HTuple = Nothing, HM3D As HTuple = Nothing, HM3DI As HTuple = Nothing
                'HOperatorSet.TupleMult(objFBM.lstImages(index).ImagePose.Pose, New HTuple({MainFrm.objFBM.pScaleMM, MainFrm.objFBM.pScaleMM, MainFrm.objFBM.pScaleMM, 1.0, 1.0, 1.0, 1.0}),
                '                                                                   objFBM.lstImages(index).ImagePose.ReConstWorldPose)
                'HOperatorSet.PoseToHomMat3d(objFBM.lstImages.Item(index).ImagePose.ReConstWorldPose, HM3D)
                'HOperatorSet.HomMat3dInvert(HM3D, HM3DI)
                'HOperatorSet.HomMat3dToPose(HM3DI, hv_PoseIn)
                'HOperatorSet.DispObjectModel3d(win, hv_HalconObjectModel3d, objFBM.hv_CamparamOut, hv_PoseIn, New HTuple("disp_background", "alpha", "colored", "point_size"), New HTuple("true", 0.3, 12).TupleConcat(1))
            Else
                If FileIO.FileSystem.FileExists(MainFrm.objFBM.ProjectPath & "\Pdata\ReconstResultOm3.om3") Then
                    'Dim hv_ResultObject3d As New HTuple
                    HOperatorSet.ReadObjectModel3d(MainFrm.objFBM.ProjectPath & "\Pdata\ReconstResultOm3.om3", New HTuple(1), New HTuple, New HTuple, hv_HalconObjectModel3d, New HTuple)
                    'FitPlanesOptimal(hv_ResultObject3d, hv_HalconObjectModel3d)
                    'HOperatorSet.ClearObjectModel3d(hv_ResultObject3d)
                End If
                drawingDone = False
                Call Command_loadreconstruct()
                'While True
                '    If drawingDone = True Then
                '        Exit While
                '    End If
                'End While
            End If
            '20170711 SUURI update 20170322 baluu add end
        Catch ex As Exception
            ex.ToString()
        End Try

#End If
    End Sub

    Public Sub SetCurrentLabelTo_objFBM(Optional ByVal Index As Integer = -1)
        Dim i As Integer
        Dim sI As Integer
        Dim eI As Integer
        If Index <> -1 Then
            sI = Index
            eI = Index
        Else
            sI = 0
            eI = nLookPoints - 1
        End If
        For i = sI To eI
            'For Each ISI As FBMlib.ImageSet In objFBM.lstImages
            '    If gDrawPoints(i).type = 2 Then
            '        For Each CT As FBMlib.CodedTarget In ISI.Targets.lstCT
            '            If gDrawPoints(i).tid = CT.CT_ID And gDrawPoints(i).flgLabel = 1 Then
            '                CT.CurrentLabel = Trim(gDrawPoints(i).LabelName)
            '            End If
            '        Next
            '    ElseIf gDrawPoints(i).type = 1 Then
            '        For Each ST As FBMlib.SingleTarget In ISI.Targets.lstST
            '            If ST.P3ID <> -1 Then
            '                If gDrawPoints(i).tid = 10000 + ST.P3ID Then
            '                    ST.currentLabel = Trim(gDrawPoints(i).LabelName)
            '                End If
            '            End If
            '        Next
            '    End If
            'Next
            If gDrawPoints(i).type = 1 Then
                For Each C3DST As FBMlib.Common3DSingleTarget In objFBM.lstCommon3dST
                    If C3DST.PID = -1 Then
                        C3DST.PID = -1
                    End If
                    If C3DST.PID + 10000 = gDrawPoints(i).tid Then
                        For Each ST As FBMlib.SingleTarget In C3DST.lstST
                            ST.currentLabel = Trim(gDrawPoints(i).LabelName)
                        Next
                        C3DST.currentLabel = Trim(gDrawPoints(i).LabelName)
                    End If
                Next
            ElseIf gDrawPoints(i).type = 2 Then
                For Each C3DCT As FBMlib.Common3DCodedTarget In objFBM.lstCommon3dCT
                    If C3DCT.PID = gDrawPoints(i).tid And gDrawPoints(i).flgLabel = 1 Then
                        For Each CT As FBMlib.CodedTarget In C3DCT.lstCT
                            CT.CurrentLabel = Trim(gDrawPoints(i).LabelName)
                        Next
                        C3DCT.currentLabel = Trim(gDrawPoints(i).LabelName)
                    End If
                Next
            End If
        Next
    End Sub

    Private Sub DispTotalObject(ByVal win As Object, ByVal index As Integer)
#If HALCON = 11 Then

        Dim RapImage As New HObject
        Dim Frame As Object = Nothing
        HOperatorSet.GenEmptyObj(RapImage)
        ClearHObject(RapImage)
        RapImage = objFBM.GenObjectsProjectedToThisImage(index, Frame)
        HOperatorSet.ClearWindow(win)
        Dim spRow1 As Object = BTuple.TupleSelect(Frame, 0)
        Dim spCol1 As Object = BTuple.TupleSelect(Frame, 1)
        Dim spRow2 As Object = BTuple.TupleSelect(Frame, 2)
        Dim spCol2 As Object = BTuple.TupleSelect(Frame, 3)
        If spRow1 > 0 Then
            spRow1 = 0
        End If
        If spCol1 > 0 Then
            spCol1 = 0
        End If
        If spRow2 < objFBM.hv_Height Then
            spRow2 = objFBM.hv_Height
        End If
        If spCol2 < objFBM.hv_Width Then
            spCol2 = objFBM.hv_Width
        End If
        HOperatorSet.SetFont(win, "-ＭＳ 明朝-10-*-*-*-*-1-")
        HOperatorSet.SetColored(win, 12)
        HOperatorSet.SetPart(win, spRow1, spCol1, spRow2 + 200, spCol2 + 500)
        HOperatorSet.DispObj(objFBM.lstImages.Item(index).hx_Image, win)
        HOperatorSet.DispObj(RapImage, win)
        HOperatorSet.SetColor(win, "yellow")
        For Each MP As FBMlib.MeasurePoint In objFBM.lst3dPoints
            HOperatorSet.SetTposition(win, MP.tmpImagePoint.Row, MP.tmpImagePoint.Col)
            HOperatorSet.WriteString(win, MP.strPointLabel)
        Next

        ClearHObject(RapImage)

#End If
    End Sub

    'Private Sub ZoomSyori(ByVal win As Object, ByVal i As Integer, ByVal e As AxHALCONXLib._HWindowXCtrlEvents_HWindowXMouseDownEvent)
    '    On Error Resume Next
    '    Dim R As New Object
    '    Dim C As New Object
    '    If flgDraw = False Then
    '        ZoomFactor = CInt(imgToolStripTextBox1.Text)
    '        intZW = CInt(intW / ZoomFactor)
    '        intZH = CInt(intH / ZoomFactor)
    '        HOperatorSet.GetMposition(win, R, C, Nothing)

    '        If e.nButton = 4 Then
    '            flgZoom = True
    '            HOperatorSet.SetPart(win, CInt(R) - intZH, CInt(C) - intZW, CInt(R) + intZH, CInt(C) + intZW)
    '        End If
    '        If e.nButton = 1 Then
    '            flgZoom = False
    '            HOperatorSet.SetPart(win, 0, 0, intH, intW)
    '        End If
    '        DispOneObjByIndex(win, i)
    '    End If
    'End Sub

    Private Sub DispObjects(ByVal win As Object, ByVal index As Integer)
#If HALCON = 11 Then


        Dim obj As New HObject
        HOperatorSet.GenEmptyObj(obj)
        ClearHObject(obj)
        HOperatorSet.SetColor(win, "red")
        obj = objFBM.GenObjectsProjectedTargetsToThisImage(index)
        HOperatorSet.DispObj(obj, win)
        'For Each C3DST As FBMlib.Common3DSingleTarget In objFBM.lstCommon3dST
        '    HOperatorSet.SetTposition(win, C3DST.tmpImgPnt.Row, C3DST.tmpImgPnt.Col)
        '    HOperatorSet.WriteString(win, C3DST.systemlabel)
        'Next
        For Each C3DCT As FBMlib.Common3DCodedTarget In objFBM.lstCommon3dCT
            For Each CT As FBMlib.CodedTarget In C3DCT.lstCT
                If CT.ImageID = objFBM.lstImages.Item(index).ImageId Then

                    HOperatorSet.SetTposition(win, C3DCT.tmpImgPnt.Row, C3DCT.tmpImgPnt.Col)
                    HOperatorSet.WriteString(win, C3DCT.systemlabel)
                    Exit For
                End If
            Next
        Next
        HOperatorSet.SetColor(win, "blue")
        For Each C3DST As FBMlib.Common3DSingleTarget In objFBM.lstCommon3dST
            For Each ST As FBMlib.SingleTarget In C3DST.lstST
                If ST.ImageID = objFBM.lstImages.Item(index).ImageId Then
                    HOperatorSet.SetTposition(win, C3DST.tmpImgPnt.Row, C3DST.tmpImgPnt.Col)
                    HOperatorSet.WriteString(win, C3DST.systemlabel)
                    Exit For
                End If
            Next
        Next

#End If
    End Sub

    Private Sub ImageListView_Click(sender As Object, e As EventArgs) Handles ImageListView.Click

    End Sub


    Private Sub ListView2_ItemSelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs) Handles ImageListView.ItemSelectionChanged
        '20160601 Kiryu コメント解除 Sta
        '20170209 baluu edit start
        Dim i As Integer = e.ItemIndex

#If HALCON = 11 Then
        winpreview = AxHWindowXCtrl6.HalconWindow
#End If
        If flgPreview = True Then
            DispOneObjByIndex(winpreview, i)
            '20170209 baluu add start
            Dim tc As Integer = 0
            Dim sc As Integer = 0
            For Each item As ImageSet In objFBM.lstImages
                If i = tc Then
                    If item.flgConnected = False Then
                        camera_model_pick = 0
                        Exit Sub
                    End If
                End If
                If i >= tc Then
                    If item.flgConnected = False Then
                        sc += 1
                    End If
                Else
                    Exit For
                End If
                tc += 1
            Next

            camera_model_pick = View_Dilaog.Camera_Name + (i - sc)
            '20170209 baluu add end
        Else
            '(20140603 Tezuka Changed) 選択項目が0の時は処理しない
            If ImageListView.SelectedItems.Count > 0 Then
                DispObjByIndex(i)
            End If
        End If
        '20170209 baluu edit end
        '20160601 Kiryu コメント解除 End
    End Sub

    'Private Sub AxHWindowXCtrl2_HWindowXMouseDown(ByVal sender As Object, ByVal e As AxHALCONXLib._HWindowXCtrlEvents_HWindowXMouseDownEvent) Handles AxHWindowXCtrl2.HWindowXMouseDown
    '    Dim i As Integer = ImageListView.SelectedIndices.Item(0)
    '    If i < 2 Then
    '        i = 2
    '    End If
    '    ZoomSyori(win1, i - 2, e)
    'End Sub


    Private Sub AxHWindowXCtrl2_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If flgPreview = False Then

            If ImageListView.SelectedIndices.Count = 0 Then
                Exit Sub
            End If
            Dim i As Integer = ImageListView.SelectedIndices.Item(0)
            If i < 2 Then
                i = 2
            End If
            DispOneObjByIndex(win1, i - 2)

        End If
    End Sub

    'Private Sub AxHWindowXCtrl3_HWindowXMouseDown(ByVal sender As Object, ByVal e As AxHALCONXLib._HWindowXCtrlEvents_HWindowXMouseDownEvent) Handles AxHWindowXCtrl3.HWindowXMouseDown
    '    Dim i As Integer = ImageListView.SelectedIndices.Item(0)
    '    If i < 2 Then
    '        i = 2
    '    End If
    '    ZoomSyori(win2, i - 1, e)
    'End Sub


    Private Sub AxHWindowXCtrl3_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If flgPreview = False Then


            If ImageListView.SelectedIndices.Count = 0 Then
                Exit Sub
            End If
            Dim i As Integer = ImageListView.SelectedIndices.Item(0)
            If i < 2 Then
                i = 2
            End If
            DispOneObjByIndex(win2, i - 1)
        End If
    End Sub

    'Private Sub AxHWindowXCtrl4_HWindowXMouseDown(ByVal sender As Object, ByVal e As AxHALCONXLib._HWindowXCtrlEvents_HWindowXMouseDownEvent) Handles AxHWindowXCtrl4.HWindowXMouseDown

    '    Dim i As Integer = ImageListView.SelectedIndices.Item(0)
    '    If i < 2 Then
    '        i = 2
    '    End If
    '    ZoomSyori(win3, i, e)

    'End Sub

    Private Sub AxHWindowXCtrl4_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If flgPreview = False Then
            If ImageListView.SelectedIndices.Count = 0 Then
                Exit Sub
            End If
            Dim i As Integer = ImageListView.SelectedIndices.Item(0)
            If i < 2 Then
                i = 2
            End If
            DispOneObjByIndex(win3, i)
        End If
    End Sub

    Private Sub AllWindowSetPart()
#If HALCON = 11 Then
        HOperatorSet.SetPart(winpreview, 0, 0, intH, intW)
        HOperatorSet.SetPart(win1, 0, 0, intH, intW)
        HOperatorSet.SetPart(win2, 0, 0, intH, intW)
        HOperatorSet.SetPart(win3, 0, 0, intH, intW)
        HOperatorSet.SetPart(win4, 0, 0, intH, intW)
#End If
    End Sub

    '    Private Sub AxHWindowXCtrl6_MouseDownEvent(ByVal sender As Object, ByVal e As AxHALCONXLib._HWindowXCtrlEvents_MouseDownEvent) Handles AxHWindowXCtrl6.MouseMoveEvent
    '#If HALCON = 11 Then
    '        'Stop
    '        Try
    '            Dim R As Object = Nothing, C As Object = Nothing
    '            HOperatorSet.GetMposition(winpreview, R, C, Nothing)
    '            Dim i As Integer = ImageListView.SelectedIndices.Item(0)
    '            Dim ISI As FBMlib.ImageSet
    '            ISI = objFBM.lstImages(i)
    '            Dim objDist As Object = DBNull.Value
    '            Dim minVal As Double = Double.MaxValue
    '            Dim tmpSt As FBMlib.SingleTarget = Nothing
    '            If flgManualST = True Then

    '                For Each ST As FBMlib.SingleTarget In ISI.Targets.lstST
    '                    ST.P2D.CalcDistToInputPoint(R, C, objDist)
    '                    If objDist < minVal Then
    '                        minVal = objDist
    '                        tmpSt = ST
    '                    End If
    '                Next
    '                tmpManSt = tmpSt

    '            End If

    '            If flgSelectC3D = True Then
    '                tmpSt = ISI.GetClickedTarget(R, C, minVal)

    '                If minVal / (objFBM.hv_Width / AxHWindowXCtrl6.Width) < 5 Then
    '                    Dim ttt As New CLookPoint
    '                    If GetPosFromLabelName(CLookPoint.posTypeMode.All, tmpSt.currentLabel, ttt) <> -1 Then
    '                        IOUtil.blnPickPoint = True
    '                        IOUtil.pick_P = ttt
    '                        bln_ClickPoint = True
    '                    End If
    '                End If

    '            End If

    '        Catch ex As Exception
    '            Dim tt As Integer = 1

    '        End Try
    '#End If
    '    End Sub


    'Del kiryu 20161003 
    'Private Sub AxHWindowXCtrl6_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs)
    '    If e.KeyCode = Keys.ControlKey Then
    '        'Ctrl_MouseWheel = 1
    '        Debug.Print("Main-Down")
    '    End If
    'End Sub

    'Private Sub AxHWindowXCtrl6_HWindowXMouseDown(ByVal sender As Object, ByVal e As AxHALCONXLib._HWindowXCtrlEvents_HWindowXMouseDownEvent) Handles AxHWindowXCtrl6.HWindowXMouseDown
    '    Dim i As Integer = ImageListView.SelectedIndices.Item(0)
    '    ZoomSyori(winpreview, i, e)
    'End Sub

    Private Sub AxHWindowXCtrl6_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If ImageListView.SelectedIndices.Count = 0 Then
            Exit Sub
        End If
        Dim i As Integer = ImageListView.SelectedIndices.Item(0)
        DispOneObjByIndex(winpreview, i)
    End Sub


    Private Sub FBMLIB_ProgressBarEvent(ByVal sender As Object, ByVal e As FBMlib.MessageEventArgs) Handles objFBM.Progress

        gfrmProgressBar.ProgressBar1.Maximum = e.ImageCount
        gfrmProgressBar.ProgressBar1.Value = e.ImageIndex
        gfrmProgressBar.Label1.Text = e.MessageText
        gfrmProgressBar.Refresh()

    End Sub
    Private Sub FBMLIB_DetectCT(ByVal sender As Object, ByVal e As FBMlib.MessageEventArgs) Handles objFBM.DetectCT
        FBMLIB_ProgressBarEvent(sender, e)
        DispOneObjByIndex(winpreview, e.ImageIndex)
    End Sub


    Private Sub DrawDeleteRegion(ByVal win As Object, ByRef DelRegion As HObject)
        '  HOperatorSet.DrawRegion(DelRegion, win)
        Dim R1 As Object = Nothing
        Dim R2 As Object = Nothing
        Dim C1 As Object = Nothing
        Dim C2 As Object = Nothing
#If HALCON = 11 Then
        HOperatorSet.DrawRectangle1(win, R1, C1, R2, C2)
        HOperatorSet.GenRectangle1(DelRegion, R1, C1, R2, C2)
#End If
    End Sub

    Public Sub SelectedImageDisp(ByVal index As Integer)
        MainFrm.ImageListView.Focus()
        For Each SI As ListViewItem In MainFrm.ImageListView.Items
            SI.Selected = False
        Next
        Try
            MainFrm.ImageListView.Items(index).Selected = True
            'ImageListView.Items(1).Selected = True

        Catch ex As Exception
        End Try
    End Sub

    '(20140604 Tezuka ADD) 現在操作されているイメージの反転表示
    Public Sub SelectedImageDispCurrent(ByVal index As Integer)
        MainFrm.ImageListView.Focus()
        For Each SI As ListViewItem In MainFrm.ImageListView.Items
            SI.BackColor = System.Drawing.SystemColors.Window
            'SI.Selected = False
        Next
        Try
            MainFrm.ImageListView.Items(index).BackColor = System.Drawing.SystemColors.Highlight
            'MainFrm.ImageListView.Items(index).Selected = True
            ImageListView.EnsureVisible(index)
        Catch ex As Exception
        End Try
    End Sub

    Dim tmpC3dSt As FBMlib.Common3DSingleTarget
    Dim tmpManSt As FBMlib.SingleTarget
    Dim flgManualST As Boolean = False
    Dim flgClickPoint As Boolean = False
    Dim flgDeletePoint As Boolean = False

    Public flgSelectC3D As Boolean = False
    Public Sub AddUserPoint3Dextra()
        If tmpC3dSt Is Nothing Then
            tmpC3dSt = New FBMlib.Common3DSingleTarget
        End If
        flgManualST = True
        tmpManSt = Nothing

        Btn_SetHistoriText(":[Enter]=完了/[Esc]=取り消し")

        Do While True
            System.Windows.Forms.Application.DoEvents()
            If flgCreateSTCancel = True Then
                'キャンセル処理
                MsgBox("キャンセル処理")
                flgCreateSTCancel = False
                Exit Do
            End If
            If tmpManSt Is Nothing Then
            Else
                tmpC3dSt.lstST.Add(tmpManSt)
                MsgBox("計測点：" & tmpManSt.P2ID & "を選択しました。")
                tmpManSt = Nothing
                flgManualST = False


                If tmpC3dSt.lstST.Count >= 2 Then
                    Dim maxId As Integer

                    Dim Cdb As New CDBOperate
                    If Cdb.ConnectDB(m_strDataBasePath) = True Then
                        Dim ST1 As FBMlib.SingleTarget
                        Dim ST2 As FBMlib.SingleTarget
                        Dim C3DST1 As FBMlib.Common3DSingleTarget
                        Dim C3DST2 As FBMlib.Common3DSingleTarget
                        ST1 = tmpC3dSt.lstST(0)
                        ST2 = tmpC3dSt.lstST(1)
                        Dim flg1 As Boolean = True

                        Dim flg2 As Boolean = True
                        C3DST1 = objFBM.IsAruCommon3DSingleTarget(ST1)
                        C3DST2 = objFBM.IsAruCommon3DSingleTarget(ST2)
                        If C3DST1 Is Nothing Then
                            flg1 = False
                        End If
                        If C3DST2 Is Nothing Then
                            flg2 = False
                        End If

                        If flg1 = False And flg2 = False Then
                            Dim strSql As String = ""
                            Dim adoRet As ADODB.Recordset
                            strSql = "select max(TID) as maxID from measurepoint3d "
                            adoRet = Cdb.CreateRecordset(strSql)
                            If adoRet Is Nothing Then
                            Else
                                If adoRet.RecordCount > 0 Then
                                    Do Until adoRet.EOF

                                        maxId = adoRet("maxID").Value

                                        adoRet.MoveNext()
                                    Loop
                                End If
                            End If
                            tmpC3dSt.Calc3dPoints()
                            tmpC3dSt.PID = maxId - 10000 + 1
                            For Each ST As FBMlib.SingleTarget In tmpC3dSt.lstST
                                ST.P3ID = tmpC3dSt.PID
                            Next
                            objFBM.lstCommon3dST.Add(tmpC3dSt)
                        End If
                        If flg1 = True And flg2 = False Then
                            ST2.P3ID = C3DST1.PID
                            C3DST1.lstST.Add(ST2)
                            C3DST1.Calc3dPoints()

                        End If
                        If flg1 = False And flg2 = True Then
                            ST1.P3ID = C3DST2.PID
                            C3DST2.lstST.Add(ST1)
                            C3DST2.Calc3dPoints()
                        End If

                        Cdb.DisConnectDB()
                        objFBM.SaveToMeasureDataDBNewPoint(objFBM.ProjectPath & "\")
                        YMC_3DViewReDraw_NoKousin(m_strDataBasePath)
                        MsgBox("3D計測点：" & tmpC3dSt.systemlabel & "を選択しました。")
                        tmpC3dSt = Nothing

                    End If
                End If

                Exit Do

            End If
        Loop
    End Sub

    Public Sub AddUserPoint3D()
        If tmpC3dSt Is Nothing Then
            tmpC3dSt = New FBMlib.Common3DSingleTarget
        End If
        flgManualST = True
        tmpManSt = Nothing
        Do While True
            System.Windows.Forms.Application.DoEvents()
            If tmpManSt Is Nothing Then
            Else
                tmpC3dSt.lstST.Add(tmpManSt)
                MsgBox("計測点：" & tmpManSt.P2ID & "を選択しました。")
                tmpManSt = Nothing
                flgManualST = False


                If tmpC3dSt.lstST.Count >= 2 Then
                    Dim maxId As Integer

                    Dim Cdb As New CDBOperate
                    If Cdb.ConnectDB(m_strDataBasePath) = True Then
                        Dim ST1 As FBMlib.SingleTarget
                        Dim ST2 As FBMlib.SingleTarget
                        Dim C3DST1 As FBMlib.Common3DSingleTarget
                        Dim C3DST2 As FBMlib.Common3DSingleTarget
                        ST1 = tmpC3dSt.lstST(0)
                        ST2 = tmpC3dSt.lstST(1)
                        Dim flg1 As Boolean = True

                        Dim flg2 As Boolean = True
                        C3DST1 = objFBM.IsAruCommon3DSingleTarget(ST1)
                        C3DST2 = objFBM.IsAruCommon3DSingleTarget(ST2)
                        If C3DST1 Is Nothing Then
                            flg1 = False
                        End If
                        If C3DST2 Is Nothing Then
                            flg2 = False
                        End If

                        If flg1 = False And flg2 = False Then
                            Dim strSql As String = ""
                            Dim adoRet As ADODB.Recordset
                            strSql = "select max(TID) as maxID from measurepoint3d "
                            adoRet = Cdb.CreateRecordset(strSql)
                            If adoRet Is Nothing Then
                            Else
                                If adoRet.RecordCount > 0 Then
                                    Do Until adoRet.EOF

                                        maxId = adoRet("maxID").Value

                                        adoRet.MoveNext()
                                    Loop
                                End If
                            End If
                            tmpC3dSt.Calc3dPoints()
                            tmpC3dSt.PID = maxId - 10000 + 1
                            For Each ST As FBMlib.SingleTarget In tmpC3dSt.lstST
                                ST.P3ID = tmpC3dSt.PID
                            Next
                            objFBM.lstCommon3dST.Add(tmpC3dSt)
                        End If
                        If flg1 = True And flg2 = False Then
                            ST2.P3ID = C3DST1.PID
                            C3DST1.lstST.Add(ST2)
                            C3DST1.Calc3dPoints()

                        End If
                        If flg1 = False And flg2 = True Then
                            ST1.P3ID = C3DST2.PID
                            C3DST2.lstST.Add(ST1)
                            C3DST2.Calc3dPoints()
                        End If

                        Cdb.DisConnectDB()
                        objFBM.SaveToMeasureDataDBNewPoint(objFBM.ProjectPath & "\")
                        YMC_3DViewReDraw_NoKousin(m_strDataBasePath)
                        MsgBox("3D計測点：" & tmpC3dSt.systemlabel & "を選択しました。")
                        tmpC3dSt = Nothing

                    End If
                End If

                Exit Do

            End If
        Loop
    End Sub
    Dim flgEdgeST As Boolean = False
    Public Sub AddEdgePoint3D()

        'Dim FilePath As String = "C:\temp\test.txt"
        'Dim enc As System.Text.Encoding = System.Text.Encoding.GetEncoding("shift_jis")
        'Dim wrtLine As String = ""

        If tmpC3dSt Is Nothing Then
            tmpC3dSt = New FBMlib.Common3DSingleTarget
        End If
        flgEdgeST = True
        tmpManSt = Nothing
        Do While True
            System.Windows.Forms.Application.DoEvents()
            If flgEdgeST = False Then
                Exit Do
            End If
        Loop
        Dim icnt As Integer = 0
        Dim ISI1 As FBMlib.ImageSet = Nothing
        Dim ISI2 As FBMlib.ImageSet = Nothing
        For Each ISI As FBMlib.ImageSet In objFBM.lstImages
            If ISI.hx_EdgePS IsNot Nothing Then
                If icnt = 0 Then
                    ISI1 = ISI
                End If
                If icnt = 1 Then
                    ISI2 = ISI
                End If
                icnt = icnt + 1
            End If
        Next

        If icnt = 2 Then

            Dim RelPose As HTuple = Nothing
            Dim tmpImgPair As New FBMlib.ImagePairSet(ISI1, ISI2)
            'tmpImgPair.Camparam = objFBM.hv_CamparamOut '20150115 SUURI DEL
            'wrtLine = "icnt00=" & icnt
            'System.IO.File.WriteAllText(FilePath, wrtLine, enc)

            tmpImgPair.calcRelPose()
            'wrtLine = "icnt11=" & icnt
            'System.IO.File.WriteAllText(FilePath, wrtLine, enc)

            FBMlib.CalcRelPoseBetweenTwoPose(ISI1.ImagePose.Pose, ISI2.ImagePose.Pose, RelPose)
            'wrtLine = "icnt22=" & icnt
            'System.IO.File.WriteAllText(FilePath, wrtLine, enc)

            Dim ER As HTuple = Nothing
            Dim EC As HTuple = Nothing
            Dim ERtrans As HTuple = Nothing
            Dim ECtrans As HTuple = Nothing
            HOperatorSet.GetContourXld(ISI1.hx_EdgePS, ER, EC)
            'wrtLine = "icn33t=" & icnt
            'System.IO.File.WriteAllText(FilePath, wrtLine, enc)

            Dim i As Integer = 0
            Dim maxTID As Integer = objFBM.GetMaxTID - 10000
            Dim maxISI1_STid As Integer = ISI1.GetST_MaxID
            Dim maxISI2_STid As Integer = ISI2.GetST_MaxID
            Dim Zeros As Object
            Zeros = BTuple.TupleGenConst(5, 0)
            HOperatorSet.ChangeRadialDistortionCamPar("fixed", objFBM.hv_CamparamOut, Zeros, objFBM.hv_CamparamZero)
            HOperatorSet.ChangeRadialDistortionPoints(ER, EC, objFBM.hv_CamparamOut, objFBM.hv_CamparamZero, ERtrans, ECtrans)
            HOperatorSet.ChangeRadialDistortionContoursXld(ISI1.hx_EdgePS, ISI1.hx_EdgePS, objFBM.hv_CamparamOut, objFBM.hv_CamparamZero)
            HOperatorSet.ChangeRadialDistortionContoursXld(ISI2.hx_EdgePS, ISI2.hx_EdgePS, objFBM.hv_CamparamOut, objFBM.hv_CamparamZero)

            'wrtLine = "icnt=" & icnt & "::UBound(ER)=" & UBound(ER)
            'System.IO.File.WriteAllText(FilePath, wrtLine, enc)

            For i = 0 To ER.Length - 1
                Dim R As Double = ERtrans(i)
                Dim C As Double = ECtrans(i)
                Dim objEpiLine As HObject = Nothing

                objFBM.gen_epiline(objEpiLine, tmpImgPair.PairPose.RelPose, Nothing, objFBM.hv_CamparamOut, objFBM.hv_Width, R, C)
                Dim InterR As Object = Nothing
                Dim InterC As Object = Nothing
                Dim IsOverLap As Object = Nothing
                HOperatorSet.IntersectionContoursXld(ISI2.hx_EdgePS, objEpiLine, "all", InterR, InterC, IsOverLap)

                If IsOverLap <> 0 Or BTuple.TupleLength(InterR) <> 1 Then
                    Continue For
                End If
                HOperatorSet.ChangeRadialDistortionPoints(InterR, InterC, objFBM.hv_CamparamZero, objFBM.hv_CamparamOut, InterR, InterC)
                maxISI1_STid += 1
                maxISI2_STid += 1
                maxTID += 1
                Dim tmpcomST As New FBMlib.Common3DSingleTarget
                Dim tmpST1 As New FBMlib.SingleTarget(ISI1.ImageId, maxISI1_STid, ER(i), EC(i))
                Dim tmpST2 As New FBMlib.SingleTarget(ISI2.ImageId, maxISI2_STid, InterR, InterC)
                ISI1.Targets.lstST.Add(tmpST1)
                ISI2.Targets.lstST.Add(tmpST2)
                ISI1.CalcRay(objFBM.hv_CamparamOut)
                ISI2.CalcRay(objFBM.hv_CamparamOut)
                tmpcomST.lstST.Add(tmpST1)
                tmpcomST.lstST.Add(tmpST2)
                tmpcomST.PID = maxTID
                tmpcomST.Calc3dPoints()
                objFBM.lstCommon3dST.Add(tmpcomST)


            Next
            objFBM.SaveToMeasureDataDBNewPoint(objFBM.ProjectPath & "\")
            YMC_3DViewReDraw_NoKousin(m_strDataBasePath)

            For Each ISI As FBMlib.ImageSet In objFBM.lstImages
                If ISI.hx_EdgePS IsNot Nothing Then
                    ISI.hx_EdgePS = Nothing
                End If
            Next

        End If
    End Sub
    Dim flgAllEdgeST As Boolean = False
    Public Sub AddAllEdgePoint3D()

        If tmpC3dSt Is Nothing Then
            tmpC3dSt = New FBMlib.Common3DSingleTarget
        End If
        flgAllEdgeST = True
        tmpManSt = Nothing
        Do While True
            System.Windows.Forms.Application.DoEvents()
            If flgAllEdgeST = False Then
                Exit Do
            End If
        Loop
        Dim icnt As Integer = 0
        Dim ISI1 As FBMlib.ImageSet = Nothing

        For Each ISI As FBMlib.ImageSet In objFBM.lstImages
            If ISI.hx_EdgePS IsNot Nothing Then
                If icnt = 0 Then
                    ISI1 = ISI
                End If
                icnt = icnt + 1
            End If
        Next

        If icnt = 1 Then

            Dim KekkaXLD As HObject = Nothing
            '   Dim KekkaImage As HObject = Nothing
            Dim HenkanPose As Object = Nothing
            HOperatorSet.PoseToHomMat3d(ISI1.ImagePose.Pose, HenkanPose)
            HOperatorSet.HomMat3dInvert(HenkanPose, HenkanPose)
            HOperatorSet.HomMat3dToPose(HenkanPose, HenkanPose)
            HOperatorSet.ContourToWorldPlaneXld(ISI1.hx_EdgePS, KekkaXLD, objFBM.hv_CamparamOut, HenkanPose, 1 / objFBM.pScaleMM)
            'HOperatorSet.ImageToWorldPlane(ISI1.hx_Image, KekkaImage, objFBM.hv_CamparamOut, HenkanPose, objFBM.hv_Width * 3, objFBM.hv_Height * 3, 1 / objFBM.pScaleMM, "bilinear")
            HOperatorSet.WriteContourXldDxf(KekkaXLD, "XLDWORLD.dxf")
            'HOperatorSet.WriteImage(KekkaImage, "jpeg", 0, "ImageWorld.jpg")

            For Each ISI As FBMlib.ImageSet In objFBM.lstImages
                If ISI.hx_EdgePS IsNot Nothing Then
                    ClearHObject(ISI.hx_EdgePS)
                    ISI.hx_EdgePS = Nothing
                End If
            Next
            ClearHObject(KekkaXLD)

        End If
    End Sub

    '    Private Sub AxHWindowXCtrl6_MouseDownEvent1(ByVal sender As Object, ByVal e As AxHALCONXLib._HWindowXCtrlEvents_MouseDownEvent) Handles AxHWindowXCtrl6.MouseDownEvent
    '#If HALCON = 11 Then
    '        Stop
    '        Try
    '            Dim R As Object = Nothing, C As Object = Nothing
    '            HOperatorSet.GetMposition(winpreview, R, C, Nothing)
    '            Dim i As Integer = ImageListView.SelectedIndices.Item(0)
    '            Dim ISI As FBMlib.ImageSet
    '            ISI = objFBM.lstImages(i)
    '            Dim objDist As Object = DBNull.Value
    '            Dim minVal As Double = Double.MaxValue
    '            Dim tmpSt As FBMlib.SingleTarget = Nothing
    '            If flgManualST = True Then

    '                For Each ST As FBMlib.SingleTarget In ISI.Targets.lstST
    '                    ST.P2D.CalcDistToInputPoint(R, C, objDist)
    '                    If objDist < minVal Then
    '                        minVal = objDist
    '                        tmpSt = ST
    '                    End If
    '                Next
    '                tmpManSt = tmpSt

    '            End If
    '            If flgEdgeST = True Then
    '                HOperatorSet.EdgesColorSubPix(ISI.hx_Image, ISI.hx_EdgePS, "canny", 1, 30, 40)
    '                Dim IsInside As Object = DBNull.Value
    '                Dim hv_Selected As Object = DBNull.Value
    '                HOperatorSet.DispObj(ISI.hx_EdgePS, winpreview)
    '                HOperatorSet.TestXldPoint(ISI.hx_EdgePS, R, C, IsInside)

    '                Call HOperatorSet.TupleSortIndex(IsInside, IsInside)
    '                Call HOperatorSet.TupleInverse(IsInside, IsInside)
    '                Call HOperatorSet.TupleSelect(IsInside, 0, hv_Selected)
    '                If hv_Selected > 0 Then

    '                    Call HOperatorSet.SelectObj(ISI.hx_EdgePS, ISI.hx_EdgePS, BTuple.TupleAdd(hv_Selected, 1))
    '                    HOperatorSet.DispObj(ISI.hx_EdgePS, winpreview)
    '                    flgEdgeST = False

    '                End If
    '            End If

    '            If flgAllEdgeST = True Then
    '                Dim hxxImage As HObject = Nothing
    '                hxxImage = ISI.hx_Image

    '                HOperatorSet.EdgesColorSubPix(hxxImage, ISI.hx_EdgePS, "canny", 1, 10, 40)

    '                HOperatorSet.DispObj(ISI.hx_EdgePS, winpreview)
    '                flgAllEdgeST = False
    '            End If

    '            If flgSelectC3D = True Then
    '                tmpSt = ISI.GetClickedTarget(R, C, minVal)

    '                If minVal / (objFBM.hv_Width / AxHWindowXCtrl6.Width) < 5 Then
    '                    Dim ttt As New CLookPoint
    '                    If GetPosFromLabelName(CLookPoint.posTypeMode.All, tmpSt.currentLabel, ttt) <> -1 Then
    '                        IOUtil.blnPickPoint = True
    '                        IOUtil.pick_P = ttt
    '                        bln_ClickPoint = True
    '                    End If
    '                End If

    '            End If

    '        Catch ex As Exception
    '            Dim tt As Integer = 1

    '        End Try
    '#End If
    '    End Sub

#End Region


    '===========================================================================
#If 0 Then
        FourImageTableLayoutPanel.Visible = True
        Me.Text = "３画像表示"
#End If
    Private Sub Form1_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
        'Add Kiryu 20161003 Ctrl+マウスホイールで２次元画像中ラベルサイズ変更
        If (Control.ModifierKeys And Keys.Control) = Keys.Control Then
            If e.Delta > 0 Then
                If FontSize <= 72 Then
                    FontSize = FontSize + FontPich
                End If
            Else
                If FontSize >= 7 Then
                    FontSize = FontSize - FontPich
                End If
            End If
            If flgPreview = True Then
                DispOneObjByIndex(winpreview, ImageListView.SelectedIndices.Item(0))
            Else
                DispOneObjByIndex(win1, dispWin(1))
                DispOneObjByIndex(win2, dispWin(2))
                DispOneObjByIndex(win3, dispWin(3))
                DispOneObjByIndex(win4, dispWin(4))
            End If

            'マウスホイールで２次元画像の拡大縮小
        Else



            '★20121115条件追加
            'Dim HH As Double = SplitContainer1.Panel1.Height 'リボンメニューコントロールの幅
            'Dim WW As Double = SplitContainer2.Panel1.Width '画像操作ビューの幅
            'If e.Y > HH And e.X < WW Then '(1)もしマウスの位置が画面操作ビューにある時・・・・
            '    If Ctrl_MouseWheel = 1 Then '①輝度調整+
            '        'Delta：マウスホイールのノッチ1分(1移動量)=120
            '        scaleImage += 0.1 * e.Delta
            '        Debug.Print("scaleImage" & scaleImage)
            '        If scaleImage > 1000 Then '値の最高値を1000に限定する。

            '            scaleImage = 1000 '最大値にする
            '            MsgBox("これ以上明るくなりません!")
            '        ElseIf scaleImage < 5 Then '値の最低値を0に限定する。

            '            scaleImage = 5 '最小値にする
            '            MsgBox("これ以上暗くなりません!")
            '        End If
            '        AllDraw()
            '    Else '②拡大/縮小
            On Error Resume Next
            Dim n As Integer = CInt(objFBM.lstImages.Count)
            'Static prevIndex As Integer = -1
            If (n < 1) Then Exit Sub
            With objFBM.lstImages
                Static hImage1 As HObject = Nothing
                Dim i As Integer = ImageListView.SelectedIndices.Item(0)
                If flgPreview = True Then
                    ' hImage1 = .Item(0).hx_Image
                    If ResizePart(winpreview, e.Delta) = True Then
                        '      DispImage(winpreview, ImageListView.SelectedIndices.Item(0))
                        DispOneObjByIndex(winpreview, i)
                    End If
                Else
                    '(20140603 Tezuka Change) ４画面のランダム表示対応
                    'If i < 3 Then
                    '    i = 3
                    'End If
                    If ResizePart(win1, e.Delta) = True Then
                        'DispOneObjByIndex(win1, i - 3)
                        DispOneObjByIndex(win1, dispWin(1))
                        If prevWin <> dispWin(1) Then
                            SelectedImageDispCurrent(dispWin(1))
                            AxHWindowXCtrl2.Focus()
                            prevWin = dispWin(1)
                        End If
                    End If

                    If ResizePart(win2, e.Delta) = True Then
                        'DispOneObjByIndex(win2, i - 2)
                        DispOneObjByIndex(win2, dispWin(2))
                        If prevWin <> dispWin(2) Then
                            SelectedImageDispCurrent(dispWin(2))
                            AxHWindowXCtrl3.Focus()
                            prevWin = dispWin(2)
                        End If
                    End If



                    If ResizePart(win3, e.Delta) = True Then
                        'DispOneObjByIndex(win3, i - 1)
                        DispOneObjByIndex(win3, dispWin(3))
                        If prevWin <> dispWin(3) Then
                            SelectedImageDispCurrent(dispWin(3))
                            AxHWindowXCtrl4.Focus()
                            prevWin = dispWin(3)
                        End If
                    End If



                    If ResizePart(win4, e.Delta) = True Then
                        'DispOneObjByIndex(win4, i)
                        DispOneObjByIndex(win4, dispWin(4))
                        If prevWin <> dispWin(4) Then
                            SelectedImageDispCurrent(dispWin(4))
                            AxHWindowXCtrl5.Focus()
                            prevWin = dispWin(4)
                        End If
                    End If
                End If
            End With
        End If
        '    End If
        'Else '(2)もしマウスの位置が画面操作ビューにない時・・・・
        'End If

        '        ToolStripStatusLabel2.Text = e.Delta.ToString & e.Location.ToString() '--& IIf(bIsIn, "[IsIn]", "[Out]")
    End Sub

    Public flgDisableColorPart As Boolean = False '20170331 baluu add

    '20170224 baluu edit start
    '20170209 baluu add start
    Private Sub ImageWindowMouseMove(ByVal sender As Object, ByVal e As HalconDotNet.HMouseEventArgs) _
        Handles AxHWindowXCtrl2.HMouseMove, AxHWindowXCtrl3.HMouseMove, AxHWindowXCtrl4.HMouseMove, AxHWindowXCtrl5.HMouseMove, AxHWindowXCtrl6.HMouseMove
        On Error Resume Next
        Dim n As Integer = CInt(objFBM.lstImages.Count)
        If (n < 1) Then Exit Sub

        Dim win As HalconDotNet.HWindowControl = sender
        If Not win Is Nothing And flgDisableColorPart = False Then
            Select Case win.HalconID
                Case winpreview
                    If ImageListView.SelectedIndices.Count > 0 Then
                        ShowColorPart(winpreview, ImageListView.SelectedIndices.Item(0))
                    End If
                Case win1
                    ShowColorPart(win1, dispWin(1))
                Case win2
                    ShowColorPart(win2, dispWin(2))
                Case win3
                    ShowColorPart(win3, dispWin(3))
                Case win4
                    ShowColorPart(win4, dispWin(4))
            End Select
        End If
        '20170223 baluu add start
        If m_blnDrawRegion = True And flgDragStarted = True Then
            If Not win Is Nothing Then
                DispOneObjByIndex(win.HalconWindow, pSelectedImageIndex)
                HOperatorSet.GetMposition(win.HalconWindow, regionR2, regionC2, Nothing)
                HOperatorSet.SetLineWidth(win.HalconWindow, 2)
                Dim R1 As HTuple, R2 As HTuple, C1 As HTuple, C2 As HTuple
                If regionR1 < regionR2 Then
                    R1 = regionR1
                    R2 = regionR2
                Else
                    R1 = regionR2
                    R2 = regionR1
                End If
                If regionC1 < regionC2 Then
                    C1 = regionC1
                    C2 = regionC2
                Else
                    C1 = regionC2
                    C2 = regionC1
                End If
                HOperatorSet.DispRectangle1(win.HalconWindow, R1, C1, R2, C2)
                HOperatorSet.SetLineWidth(win.HalconWindow, 1)
            End If
        End If
        '20170223 baluu add end
    End Sub
    '20170209 baluu add end

    '20170223 baluu add start
    Private Sub ImageWindowDragMouseDown(ByVal sender As Object, ByVal e As HalconDotNet.HMouseEventArgs) _
        Handles AxHWindowXCtrl2.HMouseDown, AxHWindowXCtrl3.HMouseDown, AxHWindowXCtrl4.HMouseDown, AxHWindowXCtrl5.HMouseDown, AxHWindowXCtrl6.HMouseDown
        On Error Resume Next
        If m_blnDrawRegion = True Then
            Dim win As HalconDotNet.HWindowControl = sender
            If Not win Is Nothing Then
                HOperatorSet.GetMposition(win.HalconID, regionR1, regionC1, Nothing)
                flgDragStarted = True
                HOperatorSet.SetMshape(win.HalconID, "crosshair")
                Select Case win.HalconID
                    Case winpreview
                        If ImageListView.SelectedIndices.Count > 0 Then
                            pSelectedImageIndex = ImageListView.SelectedIndices.Item(0)
                        End If
                    Case win1
                        pSelectedImageIndex = dispWin(1)
                    Case win2
                        pSelectedImageIndex = dispWin(2)
                    Case win3
                        pSelectedImageIndex = dispWin(3)
                    Case win4
                        pSelectedImageIndex = dispWin(4)
                End Select
            End If
        End If
    End Sub
    Private Sub ImageWindowDragMouseUp(ByVal sender As Object, ByVal e As HalconDotNet.HMouseEventArgs) _
        Handles AxHWindowXCtrl2.HMouseUp, AxHWindowXCtrl3.HMouseUp, AxHWindowXCtrl4.HMouseUp, AxHWindowXCtrl5.HMouseUp, AxHWindowXCtrl6.HMouseUp
        If m_blnDrawRegion = True Then
            Dim win As HalconDotNet.HWindowControl = sender
            HOperatorSet.SetMshape(win.HalconID, "arrow")
            If Not pSelectedImageIndex = -1 Then
                m_blnDrawRegion = False
                m_blnCommandArea = False
                flgDragStarted = False

                Dim R1 As HTuple, R2 As HTuple, C1 As HTuple, C2 As HTuple

                R1 = regionR1
                R2 = regionR2
                C1 = regionC1
                C2 = regionC2

                If Not (regionR1 Is Nothing Or regionR2 Is Nothing Or regionC1 Is Nothing Or regionC2 Is Nothing) Then
                    If regionR1 < regionR2 Then
                        R1 = regionR1
                        R2 = regionR2
                    Else
                        R1 = regionR2
                        R2 = regionR1
                    End If
                    If regionC1 < regionC2 Then
                        C1 = regionC1
                        C2 = regionC2
                    Else
                        C1 = regionC2
                        C2 = regionC1
                    End If
                End If

                

                If m_blnSelectDelArea = True Then
                    DeleteTargetsInRegion(R1, C1, R2, C2)
                Else
                    If Not objFBM.lstImages(pSelectedImageIndex).Region Is Nothing Then '20170302 baluu add start
                        ClearHObject(objFBM.lstImages(pSelectedImageIndex).Region)
                        objFBM.lstImages(pSelectedImageIndex).Region = Nothing
                    End If '20170302 baluu add end

                    If Not (R1 Is Nothing Or C1 Is Nothing Or R2 Is Nothing Or C2 Is Nothing) Then
                        HOperatorSet.GenRectangle1(objFBM.lstImages(pSelectedImageIndex).Region, R1, C1, R2, C2)
                        regionR1 = Nothing
                        regionR2 = Nothing
                        regionC1 = Nothing
                        regionC2 = Nothing
                    End If
                End If

                DispOneObjByIndex(win.HalconWindow, pSelectedImageIndex)
                pSelectedImageIndex = -1
            End If
        End If
    End Sub

    Private Sub DeleteTargetsInRegion(ByVal R1 As HTuple, ByVal C1 As HTuple, ByVal R2 As HTuple, ByVal C2 As HTuple)
        If AskDelete() = True Then
            Dim TD As FBMlib.TargetDetect = objFBM.lstImages.Item(pSelectedImageIndex).Targets
            Dim CT_Indexs As New List(Of Integer)
            Dim ST_Indexs As New List(Of Integer)
            Dim i = 0
            For Each objST As SingleTarget In TD.lstST
                Dim row As Object = objST.P2D.Row
                Dim col As Object = objST.P2D.Col
                If BTuple.TupleGreater(row, R1).I And BTuple.TupleLess(row, R2).I And BTuple.TupleGreater(col, C1).I And BTuple.TupleLess(col, C2).I Then
                    ST_Indexs.Add(i)
                End If
                i += 1
            Next
            i = 0
            For Each objCT As CodedTarget In TD.lstCT
                Dim row As Object = objCT.CT_Points.Row
                Dim col As Object = objCT.CT_Points.Col

                If BTuple.TupleGreater(row, R1).I And BTuple.TupleLess(row, R2).I And _
                    BTuple.TupleGreater(col, C1).I And BTuple.TupleLess(col, C2).I Then
                    CT_Indexs.Add(i)
                End If
                i += 1
            Next
            If CT_Indexs.Count > 0 Or ST_Indexs.Count > 0 Then
                For Each stIndex As Integer In ST_Indexs
                    Dim ii As Integer = 0
                    Dim nn As Integer = objFBM.lstCommon3dST.Count
                    For ii = nn - 1 To 0 Step -1
                        If objFBM.lstCommon3dST.Item(ii).PID = TD.lstST(stIndex).P3ID Then
                            For Each objSST As FBMlib.SingleTarget In objFBM.lstCommon3dST.Item(ii).lstST
                                objSST.P3ID = -1
                            Next
                            objFBM.lstCommon3dST.RemoveAt(ii)
                            IOUtil.WritePrompt("3D計測点：" & TD.lstST(stIndex).currentLabel & "を削除しました。")
                            Exit For
                        End If
                    Next
                Next

                For Each ctIndex As Integer In CT_Indexs
                    Dim nn As Integer
                    Dim ii As Integer
                    nn = objFBM.lstCommon3dCT.Count
                    For ii = nn - 1 To 0 Step -1
                        If objFBM.lstCommon3dCT.Item(ii).PID = TD.lstCT(ctIndex).CT_ID Then
                            For Each objP3D As FBMlib.CodedTarget In objFBM.lstCommon3dCT.Item(ii).lstCT
                                objP3D.CT_ID = -1
                            Next
                            objFBM.lstCommon3dCT.RemoveAt(ii)
                            IOUtil.WritePrompt("3D計測点：" & TD.lstCT(ctIndex).CurrentLabel & "を削除しました。")
                            Exit For
                        End If
                    Next
                Next

                objFBM.SaveToMeasureDataDBNewPoint(objFBM.ProjectPath & "\")
                flgKoushin3DPoint = True
            End If
        End If
    End Sub
    '20170224 baluu edit end

    Private Function AskDelete() As Boolean
        Dim result As Integer = MessageBox.Show("Delete?", "Delete?", MessageBoxButtons.OKCancel)
        AskDelete = IIf(result = DialogResult.OK, True, False)
    End Function
    '20170223 baluu add end

    '20170209 baluu add end

    Private Function ResizePart(ByRef win As HWindow, ByVal Delta As Integer) As Boolean
#If HALCON = 11 Then
        Dim R As HTuple = Nothing, C As HTuple = Nothing
        Dim Rw1 As HTuple = Nothing, Cw1 As HTuple = Nothing
        Dim Rw2 As HTuple = Nothing, Cw2 As HTuple = Nothing
        Dim Rd1 As HTuple = Nothing, Cd1 As HTuple = Nothing
        Dim Rd2 As HTuple = Nothing, Cd2 As HTuple = Nothing

        Dim ZF As Double
        Try

            HOperatorSet.GetMposition(win, R, C, Nothing)
            HOperatorSet.GetPart(win, Rw1, Cw1, Rw2, Cw2)
            If (Delta > 0) Then
                ZF = 1 / ZoomFactor
            Else
                ZF = ZoomFactor
            End If

            Cd1 = C - (C - Cw1) * ZF
            Cd2 = C + (Cw2 - C) * ZF
            Rd1 = R - (R - Rw1) * ZF
            Rd2 = BTuple.TupleAdd(Rd1, New HTuple(CInt((BTuple.TupleAbs(Cd2 - Cd1) * (intH / intW)).D)))

            If (Rd1.D < 0) Then Rd1 = New HTuple(0)
            If (Cd1.D < 0) Then Cd1 = New HTuple(0)
            If (Rd2.D > intH) Then Rd2 = intH '-ZH
            If (Cd2.D > intW) Then Cd2 = intW '-ZW
            If (BTuple.TupleAbs(Rd1 - Rd2).D > minZoomSize And BTuple.TupleAbs(Cd1 - Cd2).D > minZoomSize) Then
                '--表示領域の再設定

                HOperatorSet.SetPart(win, Rd1, Cd1, Rd2, Cd2)
            End If

            ' HOperatorSet.DispObj(hImage, win)
            ResizePart = True
        Catch ex As Exception
            ResizePart = False
        End Try
#End If
    End Function

    '20170209 baluu add start
    Private Function ShowColorPart(ByRef win As HWindow, ByVal iIndex As Integer)
        Dim mR As Object = Nothing, mC As Object = Nothing
        Dim grayVal As Object = Nothing
        Try
            HOperatorSet.GetMposition(win, mR, mC, Nothing)
            If Not mR Is Nothing And Not mC Is Nothing Then
                Static hImage As HObject = Nothing
                Static i As Integer = -1
                If Not i = iIndex Then
                    ClearHObject(hImage)
                    HOperatorSet.GenEmptyObj(hImage)
                    hImage = objFBM.lstImages.Item(iIndex).hx_Image
                    i = iIndex
                End If

                HOperatorSet.GetGrayval(hImage, mR, mC, grayVal)
                If Not grayVal Is Nothing Then
                    Dim R As Object = Nothing, C As Object = Nothing, W As Object = Nothing, H As Object = Nothing, IR As Object = Nothing, IC As Object = Nothing, IR2 As Object = Nothing, IC2 As Object = Nothing
                    HOperatorSet.GetPart(win, IR, IC, IR2, IC2)
                    HOperatorSet.GetWindowExtents(win, R, C, W, H)
                    HOperatorSet.ClearRectangle(win, H - 25, 0, H, 125)
                    HOperatorSet.SetTposition(win, IR2 - (20 * ((IR2 - IR) / H)), IC)
                    HOperatorSet.WriteString(win, New HTuple("RGB=" & grayVal(0).I & "," & grayVal(1).I & "," & grayVal(2).I))
                End If
            End If
            ShowColorPart = True
        Catch ex As Exception
            ShowColorPart = False
        End Try
    End Function
    '20170209 baluu add end


    Private Sub ImageListView_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles ImageListView.MouseEnter
        ' Stop

        '--12.3.22 フォルダ選択など他のダイアログを表示する時は止める必要がある
        If (bIsOtherDlgOpen = True) Then Exit Sub

        ImageListView.Focus()

    End Sub

    Private Sub ImageListView_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles ImageListView.MouseLeave

        '--12.3.22 フォルダ選択など他のダイアログを表示する時は止める必要がある
        If (bIsOtherDlgOpen = True) Then Exit Sub

        If flgPreview = True Then
#If HALCON = 11 Then
            AxHWindowXCtrl6.Focus()
#End If

        End If
    End Sub

    Private Sub ImageSplCnt_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles ImageSplCnt.MouseEnter
        '--12.3.22 フォルダ選択など他のダイアログを表示する時は止める必要がある
        If (bIsOtherDlgOpen = True) Then Exit Sub

        If flgPreview = True Then
#If HALCON = 11 Then
            AxHWindowXCtrl6.Focus()
#End If
        End If
    End Sub
    ''20121115以下はCtrlキーのKeyDown&KeyUpのテスト

    'Private Sub SplitContainer1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles SplitContainer1.KeyDown
    '    '20121113 KeyDownイベント：Ctrl⇒輝度調整
    '    If e.KeyCode = Keys.ControlKey Then
    '        Ctrl_MouseWheel = 1
    '        Debug.Print("S1のDown")
    '    End If
    'End Sub
    'Private Sub SplitContainer1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles SplitContainer1.KeyUp
    '    Ctrl_MouseWheel = 0
    '    Debug.Print("S1のUP")
    'End Sub

    'Private Sub SplitContainer2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles SplitContainer2.KeyDown
    '    '20121113 KeyDownイベント：Ctrl⇒輝度調整
    '    If e.KeyCode = Keys.ControlKey Then
    '        Ctrl_MouseWheel = 1
    '        Debug.Print("S2のDown")
    '    End If
    'End Sub

    'Private Sub SplitContainer2_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles SplitContainer2.KeyUp
    '    Ctrl_MouseWheel = 0
    '    Debug.Print("S2のUP")
    'End Sub
    'Private Sub SplitContainer3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles SplitContainer3.KeyDown
    '    '20121113 KeyDownイベント：Ctrl⇒輝度調整
    '    If e.KeyCode = Keys.ControlKey Then
    '        Ctrl_MouseWheel = 1
    '        Debug.Print("S3のDown")
    '    End If
    'End Sub

    'Private Sub SplitContainer3_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles SplitContainer3.KeyUp
    '    Ctrl_MouseWheel = 0
    '    Debug.Print("S3のUP")
    'End Sub

    'Private Sub SplitContainer4_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles SplitContainer4.KeyDown
    '    If e.KeyCode = Keys.ControlKey Then
    '        Ctrl_MouseWheel = 1
    '        Debug.Print("S4のDown")
    '    End If
    'End Sub


    'Private Sub SplitContainer4_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles SplitContainer4.KeyUp
    '    Ctrl_MouseWheel = 0
    '    Debug.Print("S4のUP")
    'End Sub

    'Private Sub halwinSplitContnr_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles halwinSplitContnr.KeyDown
    '    If e.KeyCode = Keys.ControlKey Then
    '        Ctrl_MouseWheel = 1
    '        Debug.Print("HのDown")
    '    End If
    'End Sub

    'Private Sub halwinSplitContnr_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles halwinSplitContnr.KeyUp
    '    Ctrl_MouseWheel = 0
    '    Debug.Print("HのUP")
    'End Sub

    Private Sub YCM_MainFrame_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles Me.PreviewKeyDown

    End Sub

    '------------------------------------------------
    'SplitContainer1.Panel1：リボンメニュー領域
    'SplitContainer1.Panel2：表示領域

    'SplitContainer2.Panel1：画像 (1:1.5)
    'halwinSplitContnr.Panel1：画像

    'halwinSplitContnr.Panel2：イメージリスト


    'SplitContainer2.Panel2：3Dビュー全体（3Dビュー+座標値リスト）

    'SplitContainer3.Panel1：3Dビュー
    'SplitContainer3.Panel2：座標値リスト

    '------------------------------------------------
    Private Sub SplitContainer1_ClientSizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer1.ClientSizeChanged
        'Debug.Print("SplitContainer1_ClientSizeChanged")
        SplitContainer1.SplitterDistance = 135  '--ElementHost1.Height（13.1.8　Splitcontainer1.Panel1のMinSizeを25⇒135に設定/山田）

        SplitContainer1.VerticalScroll.Visible = False
        SplitContainer1.IsSplitterFixed = True
        Debug.Print("ElementH" & ElementHost1.Height)
        Debug.Print("RibbonH" & RibbonMenuControl14.ActualHeight)
        Debug.Print("SplitContainer1.SplitterDistance= " & SplitContainer1.SplitterDistance)
        Debug.Print("SplitContainer1.Panel1.Width= " & SplitContainer1.Panel1.Width)
        Debug.Print("SplitContainer1.Panel2.Width= " & SplitContainer1.Panel2.Width)
    End Sub
    Private Sub halwinSplitContnr_ClientSizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles halwinSplitContnr.ClientSizeChanged

    End Sub

    Private Sub SplitContainer2_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer2.Resize
        'Debug.Print("SplitContainer2.変わったよ!!!")
    End Sub

    Private Sub SplitContainer2_SplitterMoved(ByVal sender As Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles SplitContainer2.SplitterMoved

        '13.1.21以前================================================================================================================================
        'If m_bDataPointShow = False Then 'If binfrmdataview = True Then（座標値リストが表示されているとき）


        '    If SplitContainer2.Width > 459 Then
        ''459=25（画像操作ビュ－最小幅）+4（スプリッター幅）+25（3D操作ビュー最小幅）+4（スプリッター幅）+401（座標値リスト）

        '        If SplitContainer2.Panel2.Width < 430 Then '座標値リストが小さくなるとき

        '            SplitContainer2.SplitterDistance = SplitContainer2.Width - 434
        '        End If
        '    Else
        '        'SplitContainer3.FixedPanel = FixedPanel.None
        '        'Dim r As Integer = SplitContainer2.Width
        '    End If
        'End If
        '13.1.21以前================================================================================================================================

        If m_bDataPointShow = False Then 'If binfrmdataview = True Then（座標値リストが表示されているとき）


            If SplitContainer2.Width > 409 Then
                '409=4（スプリッター幅）+4（スプリッター幅）+401（座標値リスト）

                If SplitContainer2.Panel2.Width < 405 Then '座標値リストが小さくなるとき

                    '405=4（スプリッター幅）+401（座標値リスト）

                    SplitContainer2.SplitterDistance = SplitContainer2.Width - 409
                End If
            Else
                'SplitContainer3.FixedPanel = FixedPanel.None
                'Dim r As Integer = SplitContainer2.Width
            End If
        End If




        ''Dim W2 As Integer = SplitContainer2.Width '画面操作ビュー+3D操作ビュー＋座標値リスト（SplitContainer2）の幅（※幅：ピクセルサイズ）

        ''Dim D2 As Integer = SplitContainer2.SplitterDistance '画面操作ビュー（SplitContainer2.Panel1）の幅

        ''Dim S2P2W As Integer = SplitContainer2.Panel2.Width 'SplitContainer3.Widthの幅

        ''Dim S2P1Min As Integer = SplitContainer2.Panel1MinSize '画面操作ビュー（SplitContainer2.Panel1）の最小サイズ


        ''Dim W3 As Integer = SplitContainer3.Width '3D操作ビュー+座標値リスト（SplitContainer3⇒SplitContainer2.Panel2）の幅

        ''Dim D3 As Integer = SplitContainer3.SplitterDistance '3D操作ビュー（SplitContainer3.Panel1）の幅

        ''Dim S3P1Min As Integer = SplitContainer3.Panel1MinSize '3D操作ビュー（SplitContainer2.Panel1）の最小サイズ
        ''Dim S3P2W As Integer = SplitContainer3.Panel2.Width '座標値リスト（SplitContainer3.Panel2）の幅（401あれば座標値リストは正常に表示）


        ''Dim S2 As Integer = SplitContainer2.SplitterWidth 'SplitContainer2の分割境界線の幅（=4）

        ''Dim S3 As Integer = SplitContainer3.SplitterWidth 'SplitContainer2の分割境界線の幅（=4）


        ''If m_bDataPointShow = False Then 'If binfrmdataview = True Then（座標値リストが表示されているとき）


        ''    If W2 >= 459 Then
        ''        If S2P2W < 430 Then '座標値リストが小さくなるとき

        ''            D2 = W2 - 434
        ''            Dim t As Integer = SplitContainer3.Panel2.Width
        ''        End If
        ''    End If
        ''End If
    End Sub
    Private Sub SplitContainer2_SplitterMoving(ByVal sender As Object, ByVal e As System.Windows.Forms.SplitterCancelEventArgs) Handles SplitContainer2.SplitterMoving
        'Debug.Print("座標値リストの幅=" & SplitContainer2.Width - SplitContainer2.SplitterDistance - SplitContainer3.SplitterDistance)
        'Debug.Print("全幅=" & SplitContainer2.Width)
        'Debug.Print("画面操作ビュー=" & SplitContainer2.SplitterDistance)
        'Debug.Print("3D操作ビュー=" & SplitContainer3.SplitterDistance)
        'Debug.Print("座標値リスト=" & SplitContainer3.Panel2.Width)
        'Debug.Print("★m_bDataPointShow=" & m_bDataPointShow)

        'Debug.Print("SplitContainer2.Height=" & SplitContainer2.Height)
        'Debug.Print("SplitContainer3.Panel1.Height=" & SplitContainer3.Panel1.Height)
        'Debug.Print("SplitContainer4.Panel1.Height=" & SplitContainer4.Panel1.Height)

        'Debug.Print("View_Dilaog.dblwindowTop=" & View_Dilaog.dblwindowTop)
        'Debug.Print("View_Dilaog.dblwindowBottom=" & View_Dilaog.dblwindowBottom)
        Debug.Print("binfrmdataview=" & binfrmdataview)
        'Debug.Print("binfrmdataviewclosed=" & binfrmdataviewclosed)
        Debug.Print(" m_bDataPointShow=" & m_bDataPointShow)
    End Sub
    Private Sub SplitContainer3_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer3.Resize
        'Debug.Print("SplitContainer3.変わったよ!!!") '座標値リストが表示状態に際に!
        'Debug.Print("座標値リストの幅=" & SplitContainer2.Width - SplitContainer2.SplitterDistance - SplitContainer3.SplitterDistance)
        '13.1.8 山田=====================================================================================================================
        If m_bDataPointShow = False Then '座標値リストが表示の時(元：If binfrmdataview = True Then)
            If SplitContainer3.Panel2.Width < 401 Then
                SplitContainer3.SplitterDistance = SplitContainer3.Panel1MinSize
            Else
                If SplitContainer2.Width - SplitContainer2.SplitterDistance - 409 < 0 Then
                    Exit Sub
                Else
                    'Debug.Print("SplitContainer2.=" & SplitContainer2.Width)
                    'Debug.Print("SplitContainer3.SplitterDistance=" & SplitContainer3.SplitterDistance)
                    'Debug.Print("SplitContainer2.SplitterDistance=" & SplitContainer2.SplitterDistance)
                    SplitContainer3.SplitterDistance = SplitContainer2.Width - SplitContainer2.SplitterDistance - 409 '座標値リストの右側を非表示にする★★問題★★
                    'SplitContainer3.SplitterDistance = Data_Point.Get_Data_Vertex_View_Withe 'Change Kiryu

                End If
            End If
        Else '座標値リストが非表示のとき

            SplitContainer3.SplitterDistance = SplitContainer2.Panel2.Width 'SplitContainer3.Width
            Data_Point.Hide()
        End If


        ''13.1.8 山田=====================================================================================================================
        'Dim Data_Vertex_Width As Double = SplitContainer2.Width - SplitContainer2.SplitterDistance _
        '- SplitContainer2.SplitterWidth - SplitContainer3.SplitterDistance - SplitContainer3.SplitterWidth
        'If m_bDataPointShow = False Then 'If binfrmdataview = True Then
        '    If Data_Vertex_Width < 401 Then 'If SplitContainer3.Panel2.Width < 401 Then
        '        SplitContainer2.SplitterDistance = SplitContainer2.Width - 434
        '        SplitContainer3.SplitterDistance = SplitContainer3.Panel1MinSize
        '        Data_Point.Refresh() '13.1.9　座標値リストを再描画
        '    Else
        '        SplitContainer3.SplitterDistance = SplitContainer2.Width - SplitContainer2.SplitterDistance - 405 '座標値リストの右側を非表示にする
        '    End If
        'Else
        '    SplitContainer3.SplitterDistance = SplitContainer3.Width
        '    Data_Point.Hide()
        'End If
    End Sub

    Private Sub SplitContainer3_SplitterMoved(ByVal sender As Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles SplitContainer3.SplitterMoved
        'Dim Data_Vertex_Width As Double = SplitContainer2.Width - SplitContainer2.SplitterDistance _
        '                                  - SplitContainer2.SplitterWidth - SplitContainer3.SplitterDistance - SplitContainer3.SplitterWidth

        'If m_bDataPointShow = False Then 'If binfrmdataview = True Then
        '    If SplitContainer2.Panel2.Width < 401 Then
        '        SplitContainer2.SplitterDistance = SplitContainer2.Width - 434
        '        SplitContainer3.SplitterDistance = SplitContainer3.Panel1MinSize
        '        'Data_Point.Refresh() '13.1.9　座標値リストを再描画
        '    Else
        '        SplitContainer3.SplitterDistance = SplitContainer2.Width - SplitContainer2.SplitterDistance - 409 '座標値リストの右側を非表示にする
        '    End If
        'Else
        '    SplitContainer3.SplitterDistance = SplitContainer3.Width
        '    Data_Point.Hide()
        'End If
    End Sub
    '================================================================================================================13.1.9コメントに
    '--rep.12.10.17    Private Sub SplitContainer3_SplitterMoved(ByVal sender As System.Object, ByVal e As System.Windows.Forms.SplitterEventArgs)
    ''Private Sub SplitContainer3_SplitterMoved(ByVal sender As Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles SplitContainer3.SplitterMoved
    ''    View_Dilaog.Width = e.SplitX
    ''    If Data_Point.Data_Vertex_Dock = 1 Then
    ''        Data_Point.Width = Me.SplitContainer3.Width - e.SplitX
    ''    End If
    ''    'SplitContainer3.SplitterDistance = SplitContainer3.Width - SplitContainer3.Panel2.Width '20121128
    ''End Sub
    '================================================================================================================13.1.9コメントに
    Private Sub SplitContainer4_ClientSizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer4.ClientSizeChanged
        Debug.Print("SP4_ClientSizeChg")
    End Sub
    '--rep.12.11.2    Private Sub SplitContainer4_SplitterMoved(ByVal sender As System.Object, ByVal e As System.Windows.Forms.SplitterEventArgs)
    Private Sub SplitContainer4_SplitterMoved(ByVal sender As Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles SplitContainer4.SplitterMoved
        Me.LBox_Data.Height = Me.SplitContainer4.Panel2.Height - Me.TBox_Data.Height 'チェックしてみる

        'View_Dilaog.Height = SplitContainer2.Panel1.Height
    End Sub

    '--test.start----------------------------------
    Private Sub SplitContainer1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer1.Resize
        'Debug.Print("SplitContainer1_Resize")
    End Sub
    Private Sub SplitContainer1_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer1.SizeChanged
        'Debug.Print("SplitContainer1_SizeChanged")
    End Sub

    Private Sub ElementHost1_ChangeUICues(ByVal sender As Object, ByVal e As System.Windows.Forms.UICuesEventArgs) Handles ElementHost1.ChangeUICues
        Debug.Print("ElementHost1_ChangeUICues")
    End Sub

    Private Sub ElementHost1_ChildChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.Integration.ChildChangedEventArgs) Handles ElementHost1.ChildChanged
        Debug.Print("ElementHost1_ChildChanged")
    End Sub

    Private Sub ElementHost1_DockChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ElementHost1.DockChanged
        Debug.Print("ElementHost1_DockChanged")
    End Sub

    Private Sub ElementHost1_MarginChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ElementHost1.MarginChanged
        Debug.Print("ElementHost1_MarginChanged")
    End Sub

    Private Sub ElementHost1_RegionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ElementHost1.RegionChanged
        Debug.Print("ElementHost1_RegionChanged")
    End Sub

    Private Sub ElementHost1_StyleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ElementHost1.StyleChanged
        Debug.Print("ElementHost1_StyleChanged")
    End Sub
    'H25.6.27　　Yamada
    'IMageListView中のサムネイル画像のチェックのOn/Off
    'チェックOn：flgConnected = True
    'チェックOff：flgConnected = False

    Public Sub ImageListItemCheck()

        Dim i As Integer = 0
        Dim n As Integer = CInt(objFBM.lstImages.Count)

        For i = 0 To n - 1
            If objFBM.lstImages.Item(i).flgConnected = True Then
                ImageListView.Items(i).Checked = True
            Else
                ImageListView.Items(i).Checked = False
            End If
        Next i

        ILVCheckedFinishFlg = True

    End Sub
    Private Sub ImageListView_ItemCheck(sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles ImageListView.ItemCheck

        '20170607 Kiryu チェックボックスをクリックorダブルクリックしても、チェック状態を変更しない
        If ILVCheckedFinishFlg = True Then
            e.NewValue = e.CurrentValue
        End If
        'ImageListView.Items(e.Index).Checked = Not (ImageListView.Items(e.Index).Checked)



        'If (e.CurrentValue = CheckState.Unchecked) Then

        'Dim bCheck, bFlag As Boolean
        'bCheck = ImageListView.CheckedItems
        'bFlag = False


        'End If
    End Sub


    Private Sub YCM_MainFrame_Paint(sender As Object, e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint

    End Sub

    Private Sub SplitContainer4_Panel1_Paint(sender As System.Object, e As System.Windows.Forms.PaintEventArgs) Handles SplitContainer4.Panel1.Paint

    End Sub



    Private Sub YCM_MainFrame_QueryContinueDrag(sender As Object, e As System.Windows.Forms.QueryContinueDragEventArgs) Handles Me.QueryContinueDrag

    End Sub

    '(20140604 Tezuka ADD) 以下４ルーチン追加（４画面表示時に画面がアクティブじゃなくなった場合、色を戻す）
    Private Sub AxHWindowXCtrl2_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AxHWindowXCtrl2.Leave
        If dispWin(1) >= 0 Then
            If MainFrm.ImageListView.Items(dispWin(1)).BackColor = System.Drawing.SystemColors.Highlight Then
                MainFrm.ImageListView.Items(dispWin(1)).BackColor = System.Drawing.SystemColors.Window
                prevWin = -1
            End If
        End If
    End Sub

    Private Sub AxHWindowXCtrl3_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AxHWindowXCtrl3.Leave
        If dispWin(2) >= 0 Then
            If MainFrm.ImageListView.Items(dispWin(2)).BackColor = System.Drawing.SystemColors.Highlight Then
                MainFrm.ImageListView.Items(dispWin(2)).BackColor = System.Drawing.SystemColors.Window
                prevWin = -1
            End If
        End If
    End Sub

    Private Sub AxHWindowXCtrl4_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AxHWindowXCtrl4.Leave
        If dispWin(3) >= 0 Then
            If MainFrm.ImageListView.Items(dispWin(3)).BackColor = System.Drawing.SystemColors.Highlight Then
                MainFrm.ImageListView.Items(dispWin(3)).BackColor = System.Drawing.SystemColors.Window
                prevWin = -1
            End If
        End If
    End Sub

    Private Sub AxHWindowXCtrl5_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AxHWindowXCtrl5.Leave
        If dispWin(4) >= 0 Then
            If MainFrm.ImageListView.Items(dispWin(4)).BackColor = System.Drawing.SystemColors.Highlight Then
                MainFrm.ImageListView.Items(dispWin(4)).BackColor = System.Drawing.SystemColors.Window
                prevWin = -1
            End If
        End If
    End Sub

    Private Sub AxHWinAllLeave()
        If dispWin(1) >= 0 Then
            If MainFrm.ImageListView.Items(dispWin(1)).BackColor = System.Drawing.SystemColors.Highlight Then
                MainFrm.ImageListView.Items(dispWin(1)).BackColor = System.Drawing.SystemColors.Window
            End If
        End If
        If dispWin(2) >= 0 Then
            If MainFrm.ImageListView.Items(dispWin(2)).BackColor = System.Drawing.SystemColors.Highlight Then
                MainFrm.ImageListView.Items(dispWin(2)).BackColor = System.Drawing.SystemColors.Window
            End If
        End If
        If dispWin(3) >= 0 Then
            If MainFrm.ImageListView.Items(dispWin(3)).BackColor = System.Drawing.SystemColors.Highlight Then
                MainFrm.ImageListView.Items(dispWin(3)).BackColor = System.Drawing.SystemColors.Window
            End If
        End If
        If dispWin(4) >= 0 Then
            If MainFrm.ImageListView.Items(dispWin(4)).BackColor = System.Drawing.SystemColors.Highlight Then
                MainFrm.ImageListView.Items(dispWin(4)).BackColor = System.Drawing.SystemColors.Window
            End If
        End If
        prevWin = -1
    End Sub

    'Private Sub AxHWindowXCtrl2_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AxHWindowXCtrl2.Enter
    '    SelectedImageDispCurrent(dispWin(1))
    '    AxHWindowXCtrl2.Focus()
    'End Sub

    'Private Sub AxHWindowXCtrl3_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AxHWindowXCtrl3.Enter
    '    SelectedImageDispCurrent(dispWin(2))
    '    AxHWindowXCtrl3.Focus()
    'End Sub

    'Private Sub AxHWindowXCtrl4_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AxHWindowXCtrl4.Enter
    '    SelectedImageDispCurrent(dispWin(3))
    '    AxHWindowXCtrl4.Focus()
    'End Sub

    'Private Sub AxHWindowXCtrl5_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AxHWindowXCtrl5.Enter
    '    SelectedImageDispCurrent(dispWin(4))
    '    AxHWindowXCtrl5.Focus()
    'End Sub

    '(20140605 Tezuka ADD) ４画面表示パネルのリサイズイベント（座標値リスト表示対応）
    Private Sub FourImageTableLayoutPanel_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FourImageTableLayoutPanel.Resize
        DispObjByIndex(-1)
    End Sub

    '(20140609 Tezuka ADD) ４画面の３D点追加対応
    'Dim iWin As Integer = 0
    'Dim iBtn As Integer = 0

    '４画面の左上画面のマウスクリックイベント
    Private Sub AxHWindowXCtrl2_MouseDownEvent(ByVal sender As System.Object, ByVal e As HalconDotNet.HMouseEventArgs) Handles AxHWindowXCtrl2.HMouseDown
#If HALCON = 11 Then
        'Stop
        DrawRect(win1, 1)
        'Try

        '    Dim R As Object = Nothing, C As Object = Nothing
        '    Dim mBtn As Object = Nothing
        '    HOperatorSet.GetMposition(win1, R, C, mBtn)
        '    Dim ii As Integer = Integer.Parse(mBtn.ToString)
        '    iBtn = ii
        '    iWin = 1
        '    Dim i As Integer = dispWin(1)
        '    Dim ISI As FBMlib.ImageSet
        '    ISI = objFBM.lstImages(i)
        '    Dim objDist As Object = DBNull.Value
        '    Dim minVal As Double = Double.MaxValue
        '    Dim tmpSt As FBMlib.SingleTarget = Nothing
        '    If flgManualST = True Then
        '        If mBtn = 1 Then
        '            For Each ST As FBMlib.SingleTarget In ISI.Targets.lstST
        '                ST.P2D.CalcDistToInputPoint(R, C, objDist)
        '                If objDist < minVal Then
        '                    minVal = objDist
        '                    tmpSt = ST
        '                End If
        '            Next
        '            ISI.CalcRay(objFBM.hv_CamparamOut)
        '            tmpManSt = tmpSt
        '        End If
        '    End If
        '    If flgClickPoint = True Then
        '        If mBtn = 1 Then
        '            Dim tmpP2ID As Integer = ISI.Targets.GetST_MaxID

        '            tmpSt = New FBMlib.SingleTarget(ISI.ImageId, tmpP2ID + 1, R, C)
        '            ISI.Targets.lstST.Add(tmpSt)
        '            ISI.CalcRay(objFBM.hv_CamparamOut)
        '            tmpManSt = tmpSt
        '        End If
        '    End If
        '    If flgEdgeST = True Then
        '        If mBtn = 1 And tmpManSt Is Nothing Then
        '            Dim tmpP2ID As Integer = ISI.Targets.GetST_MaxID
        '            Dim tmpWH As Integer = 50
        '            tmpSt = New FBMlib.SingleTarget(ISI.ImageId, tmpP2ID + 1, R, C)
        '            tmpManSt = tmpSt
        '            System.Threading.Thread.Sleep(100)
        '            HOperatorSet.DrawRectangle1Mod(win1, R - tmpWH, R - tmpWH, R + tmpWH, R + tmpWH,
        '                                 tmpManSt.P2D.Row, tmpManSt.P2D.Col, tmpManSt.TP2D.Row, tmpManSt.TP2D.Col)

        '            'HOperatorSet.DrawRectangle1(win1, tmpManSt.P2D.Row, tmpManSt.P2D.Col, tmpManSt.TP2D.Row, tmpManSt.TP2D.Col)

        '        End If


        '        'HOperatorSet.EdgesColorSubPix(ISI.hx_Image, ISI.hx_EdgePS, "canny", 1, 30, 40)
        '        'Dim IsInside As Object = DBNull.Value
        '        'Dim hv_Selected As Object = DBNull.Value
        '        'HOperatorSet.DispObj(ISI.hx_EdgePS, win1)
        '        'HOperatorSet.TestXldPoint(ISI.hx_EdgePS, R, C, IsInside)

        '        'Call HOperatorSet.TupleSortIndex(IsInside, IsInside)
        '        'Call HOperatorSet.TupleInverse(IsInside, IsInside)
        '        'Call HOperatorSet.TupleSelect(IsInside, 0, hv_Selected)
        '        'If hv_Selected > 0 Then

        '        '    Call HOperatorSet.SelectObj(ISI.hx_EdgePS, ISI.hx_EdgePS, BTuple.TupleAdd(hv_Selected, 1))
        '        '    HOperatorSet.DispObj(ISI.hx_EdgePS, win1)
        '        '    flgEdgeST = False

        '        'End If
        '    End If

        '    If flgAllEdgeST = True Then
        '        'Dim hxxImage As HObject = Nothing
        '        'hxxImage = ISI.hx_Image
        '        'GC.Collect()
        '        'GC.WaitForPendingFinalizers()
        '        ''  HOperatorSet.EdgesColorSubPix(hxxImage, ISI.hx_EdgePS, "canny", 1, 10, 40)
        '        'HOperatorSet.EdgesColorSubPix(ISI.hx_Image, ISI.hx_EdgePS, "canny", 1, 10, 40)

        '        'HOperatorSet.DispObj(ISI.hx_EdgePS, win1)
        '        flgAllEdgeST = False
        '    End If

        '    If flgSelectC3D = True Then
        '        tmpSt = ISI.GetClickedTarget(R, C, minVal)

        '        If minVal / (objFBM.hv_Width / AxHWindowXCtrl2.Width) < 5 Then
        '            tmpManSt = tmpSt
        '            Dim ttt As New CLookPoint
        '            If GetPosFromLabelName(CLookPoint.posTypeMode.All, tmpSt.currentLabel, ttt) <> -1 Then
        '                IOUtil.blnPickPoint = True
        '                IOUtil.pick_P = ttt
        '                bln_ClickPoint = True
        '            End If
        '        End If

        '    End If

        'Catch ex As Exception
        '    Dim tt As Integer = 1
        '    MsgBox("画面1クリックイベントエラー")
        '    flgEdgeST = False
        '    flgAllEdgeST = False
        'End Try
#End If
    End Sub

    '４画面の右上画面のマウスクリックイベント
    Private Sub AxHWindowXCtrl3_MouseDownEvent(ByVal sender As System.Object, ByVal e As HalconDotNet.HMouseEventArgs) Handles AxHWindowXCtrl3.HMouseDown
#If HALCON = 11 Then
        'Stop
        DrawRect(win2, 2)
        'Try
        '    Dim R As Object = Nothing, C As Object = Nothing
        '    Dim mBtn As Object = Nothing
        '    HOperatorSet.GetMposition(win2, R, C, mBtn)
        '    Dim ii As Integer = Integer.Parse(mBtn.ToString)
        '    iBtn = ii
        '    iWin = 2
        '    Dim i As Integer = dispWin(2)
        '    Dim ISI As FBMlib.ImageSet
        '    ISI = objFBM.lstImages(i)
        '    Dim objDist As Object = DBNull.Value
        '    Dim minVal As Double = Double.MaxValue
        '    Dim tmpSt As FBMlib.SingleTarget = Nothing
        '    If flgManualST = True Then
        '        If mBtn = 1 Then
        '            For Each ST As FBMlib.SingleTarget In ISI.Targets.lstST
        '                ST.P2D.CalcDistToInputPoint(R, C, objDist)
        '                If objDist < minVal Then
        '                    minVal = objDist
        '                    tmpSt = ST
        '                End If
        '            Next
        '            ISI.CalcRay(objFBM.hv_CamparamOut)
        '            tmpManSt = tmpSt
        '        End If
        '    End If
        '    If flgClickPoint = True Then
        '        If mBtn = 1 Then
        '            Dim tmpP2ID As Integer = ISI.Targets.GetST_MaxID

        '            tmpSt = New FBMlib.SingleTarget(ISI.ImageId, tmpP2ID + 1, R, C)
        '            ISI.Targets.lstST.Add(tmpSt)
        '            ISI.CalcRay(objFBM.hv_CamparamOut)
        '            tmpManSt = tmpSt
        '        End If
        '    End If
        '    If flgEdgeST = True Then
        '        If mBtn = 1 Then
        '            Dim tmpP2ID As Integer = ISI.Targets.GetST_MaxID
        '            Dim tmpWH As Integer = 50
        '            tmpSt = New FBMlib.SingleTarget(ISI.ImageId, tmpP2ID + 1, R, C)
        '            tmpManSt = tmpSt
        '            'add by SUSANO
        '            HOperatorSet.DrawRectangle1Mod(win2, R - tmpWH, R - tmpWH, R + tmpWH, R + tmpWH,
        '                                 tmpManSt.P2D.Row, tmpManSt.P2D.Col, tmpManSt.TP2D.Row, tmpManSt.TP2D.Col)
        '            'HOperatorSet.DrawRectangle1(win2, tmpManSt.P2D.Row, tmpManSt.P2D.Col, tmpManSt.TP2D.Row, tmpManSt.TP2D.Col)
        '        End If
        '    End If

        '    If flgAllEdgeST = True Then
        '        'Dim hxxImage As HObject = Nothing
        '        'hxxImage = ISI.hx_Image

        '        'HOperatorSet.EdgesColorSubPix(hxxImage, ISI.hx_EdgePS, "canny", 1, 10, 40)

        '        'HOperatorSet.DispObj(ISI.hx_EdgePS, win2)
        '        flgAllEdgeST = False
        '    End If

        '    If flgSelectC3D = True Then
        '        tmpSt = ISI.GetClickedTarget(R, C, minVal)

        '        If minVal / (objFBM.hv_Width / AxHWindowXCtrl3.Width) < 5 Then
        '            tmpManSt = tmpSt
        '            Dim ttt As New CLookPoint
        '            If GetPosFromLabelName(CLookPoint.posTypeMode.All, tmpSt.currentLabel, ttt) <> -1 Then
        '                IOUtil.blnPickPoint = True
        '                IOUtil.pick_P = ttt
        '                bln_ClickPoint = True
        '            End If
        '        End If

        '    End If

        'Catch ex As Exception
        '    Dim tt As Integer = 1
        '    MsgBox("画面2クリックイベントエラー")
        '    flgEdgeST = False
        '    flgAllEdgeST = False
        'End Try
#End If
    End Sub

    '４画面の左下画面のマウスクリックイベント
    Private Sub AxHWindowXCtrl4_MouseDownEvent(ByVal sender As System.Object, ByVal e As HalconDotNet.HMouseEventArgs) Handles AxHWindowXCtrl4.HMouseDown
#If HALCON = 11 Then
        'Stop
        DrawRect(win3, 3)
        'Try
        '    Dim R As Object = Nothing, C As Object = Nothing
        '    Dim mBtn As Object = Nothing
        '    HOperatorSet.GetMposition(win3, R, C, mBtn)
        '    Dim ii As Integer = Integer.Parse(mBtn.ToString)
        '    iBtn = ii
        '    iWin = 3
        '    Dim i As Integer = dispWin(3)
        '    Dim ISI As FBMlib.ImageSet
        '    ISI = objFBM.lstImages(i)
        '    Dim objDist As Object = DBNull.Value
        '    Dim minVal As Double = Double.MaxValue
        '    Dim tmpSt As FBMlib.SingleTarget = Nothing
        '    If flgManualST = True Then
        '        If mBtn = 1 Then
        '            For Each ST As FBMlib.SingleTarget In ISI.Targets.lstST
        '                ST.P2D.CalcDistToInputPoint(R, C, objDist)
        '                If objDist < minVal Then
        '                    minVal = objDist
        '                    tmpSt = ST
        '                End If
        '            Next
        '            ISI.CalcRay(objFBM.hv_CamparamOut)
        '            tmpManSt = tmpSt
        '        End If
        '    End If
        '    If flgClickPoint = True Then
        '        If mBtn = 1 Then
        '            Dim tmpP2ID As Integer = ISI.Targets.GetST_MaxID

        '            tmpSt = New FBMlib.SingleTarget(ISI.ImageId, tmpP2ID + 1, R, C)
        '            ISI.Targets.lstST.Add(tmpSt)
        '            ISI.CalcRay(objFBM.hv_CamparamOut)
        '            tmpManSt = tmpSt
        '        End If
        '    End If
        '    If flgEdgeST = True Then
        '        If mBtn = 1 Then
        '            Dim tmpP2ID As Integer = ISI.Targets.GetST_MaxID
        '            Dim tmpWH As Integer = 50
        '            tmpSt = New FBMlib.SingleTarget(ISI.ImageId, tmpP2ID + 1, R, C)
        '            tmpManSt = tmpSt
        '            'add by SUSANO
        '            HOperatorSet.DrawRectangle1Mod(win3, R - tmpWH, R - tmpWH, R + tmpWH, R + tmpWH,
        '                                 tmpManSt.P2D.Row, tmpManSt.P2D.Col, tmpManSt.TP2D.Row, tmpManSt.TP2D.Col)
        '        End If
        '        'HOperatorSet.EdgesColorSubPix(ISI.hx_Image, ISI.hx_EdgePS, "canny", 1, 30, 40)
        '        'Dim IsInside As Object = DBNull.Value
        '        'Dim hv_Selected As Object = DBNull.Value
        '        'HOperatorSet.DispObj(ISI.hx_EdgePS, win3)
        '        'HOperatorSet.TestXldPoint(ISI.hx_EdgePS, R, C, IsInside)

        '        'Call HOperatorSet.TupleSortIndex(IsInside, IsInside)
        '        'Call HOperatorSet.TupleInverse(IsInside, IsInside)
        '        'Call HOperatorSet.TupleSelect(IsInside, 0, hv_Selected)
        '        'If hv_Selected > 0 Then

        '        '    Call HOperatorSet.SelectObj(ISI.hx_EdgePS, ISI.hx_EdgePS, BTuple.TupleAdd(hv_Selected, 1))
        '        '    HOperatorSet.DispObj(ISI.hx_EdgePS, win3)
        '        '    flgEdgeST = False

        '        'End If
        '    End If

        '    If flgAllEdgeST = True Then
        '        'Dim hxxImage As HObject = Nothing
        '        'hxxImage = ISI.hx_Image

        '        'HOperatorSet.EdgesColorSubPix(hxxImage, ISI.hx_EdgePS, "canny", 1, 10, 40)

        '        'HOperatorSet.DispObj(ISI.hx_EdgePS, win3)
        '        flgAllEdgeST = False
        '    End If

        '    If flgSelectC3D = True Then
        '        tmpSt = ISI.GetClickedTarget(R, C, minVal)

        '        If minVal / (objFBM.hv_Width / AxHWindowXCtrl4.Width) < 5 Then
        '            tmpManSt = tmpSt
        '            Dim ttt As New CLookPoint
        '            If GetPosFromLabelName(CLookPoint.posTypeMode.All, tmpSt.currentLabel, ttt) <> -1 Then
        '                IOUtil.blnPickPoint = True
        '                IOUtil.pick_P = ttt
        '                bln_ClickPoint = True
        '            End If
        '        End If

        '    End If

        'Catch ex As Exception
        '    Dim tt As Integer = 1
        '    MsgBox("画面3クリックイベントエラー")
        '    flgEdgeST = False
        '    flgAllEdgeST = False
        'End Try
#End If
    End Sub

    '４画面の右下画面のマウスクリックイベント
    Private Sub AxHWindowXCtrl5_MouseDownEvent(ByVal sender As System.Object, ByVal e As HalconDotNet.HMouseEventArgs) Handles AxHWindowXCtrl5.HMouseDown
#If HALCON = 11 Then
        'Stop
        DrawRect(win4, 4)
        'Try
        '    Dim R As Object = Nothing, C As Object = Nothing
        '    Dim mBtn As Object = Nothing
        '    HOperatorSet.GetMposition(win4, R, C, mBtn)
        '    Dim ii As Integer = Integer.Parse(mBtn.ToString)
        '    iBtn = ii
        '    iWin = 4
        '    Dim i As Integer = dispWin(4)
        '    Dim ISI As FBMlib.ImageSet
        '    ISI = objFBM.lstImages(i)
        '    Dim objDist As Object = DBNull.Value
        '    Dim minVal As Double = Double.MaxValue
        '    Dim tmpSt As FBMlib.SingleTarget = Nothing
        '    If flgManualST = True Then
        '        If mBtn = 1 Then
        '            For Each ST As FBMlib.SingleTarget In ISI.Targets.lstST
        '                ST.P2D.CalcDistToInputPoint(R, C, objDist)
        '                If objDist < minVal Then
        '                    minVal = objDist
        '                    tmpSt = ST
        '                End If
        '            Next
        '            ISI.CalcRay(objFBM.hv_CamparamOut)
        '            tmpManSt = tmpSt
        '        End If
        '    End If
        '    If flgClickPoint = True Then
        '        If mBtn = 1 Then
        '            Dim tmpP2ID As Integer = ISI.Targets.GetST_MaxID

        '            tmpSt = New FBMlib.SingleTarget(ISI.ImageId, tmpP2ID + 1, R, C)
        '            ISI.Targets.lstST.Add(tmpSt)
        '            ISI.CalcRay(objFBM.hv_CamparamOut)
        '            tmpManSt = tmpSt
        '        End If
        '    End If
        '    If flgEdgeST = True Then
        '        If mBtn = 1 Then
        '            Dim tmpP2ID As Integer = ISI.Targets.GetST_MaxID
        '            Dim tmpWH As Integer = 50
        '            tmpSt = New FBMlib.SingleTarget(ISI.ImageId, tmpP2ID + 1, R, C)
        '            tmpManSt = tmpSt
        '            'add by SUSANO
        '            HOperatorSet.DrawRectangle1Mod(win4, R - tmpWH, R - tmpWH, R + tmpWH, R + tmpWH,
        '                                 tmpManSt.P2D.Row, tmpManSt.P2D.Col, tmpManSt.TP2D.Row, tmpManSt.TP2D.Col)
        '        End If
        '        'HOperatorSet.EdgesColorSubPix(ISI.hx_Image, ISI.hx_EdgePS, "canny", 1, 30, 40)
        '        'Dim IsInside As Object = DBNull.Value
        '        'Dim hv_Selected As Object = DBNull.Value
        '        'HOperatorSet.DispObj(ISI.hx_EdgePS, win4)
        '        'HOperatorSet.TestXldPoint(ISI.hx_EdgePS, R, C, IsInside)

        '        'Call HOperatorSet.TupleSortIndex(IsInside, IsInside)
        '        'Call HOperatorSet.TupleInverse(IsInside, IsInside)
        '        'Call HOperatorSet.TupleSelect(IsInside, 0, hv_Selected)
        '        'If hv_Selected > 0 Then

        '        '    Call HOperatorSet.SelectObj(ISI.hx_EdgePS, ISI.hx_EdgePS, BTuple.TupleAdd(hv_Selected, 1))
        '        '    HOperatorSet.DispObj(ISI.hx_EdgePS, win4)
        '        '    flgEdgeST = False

        '        'End If
        '    End If

        '    If flgAllEdgeST = True Then
        '        'Dim hxxImage As HObject = Nothing
        '        'hxxImage = ISI.hx_Image

        '        'HOperatorSet.EdgesColorSubPix(hxxImage, ISI.hx_EdgePS, "canny", 1, 10, 40)

        '        'HOperatorSet.DispObj(ISI.hx_EdgePS, win4)
        '        flgAllEdgeST = False
        '    End If

        '    If flgSelectC3D = True Then
        '        tmpSt = ISI.GetClickedTarget(R, C, minVal)

        '        If minVal / (objFBM.hv_Width / AxHWindowXCtrl5.Width) < 5 Then
        '            tmpManSt = tmpSt
        '            Dim ttt As New CLookPoint
        '            If GetPosFromLabelName(CLookPoint.posTypeMode.All, tmpSt.currentLabel, ttt) <> -1 Then
        '                IOUtil.blnPickPoint = True
        '                IOUtil.pick_P = ttt
        '                bln_ClickPoint = True
        '            End If
        '        End If

        '    End If

        'Catch ex As Exception
        '    Dim tt As Integer = 1
        '    MsgBox("画面4クリックイベントエラー")
        '    flgEdgeST = False
        '    flgAllEdgeST = False
        'End Try
#End If
    End Sub

    '１画面表示時のマウスクリックイベント
    Private Sub AxHWindowXCtrl6_MouseDownEvent1(ByVal sender As Object, ByVal e As HalconDotNet.HMouseEventArgs) Handles AxHWindowXCtrl6.HMouseDown
#If HALCON = 11 Then
        'Stop
        Try
            Dim R As Object = Nothing, C As Object = Nothing
            Dim mBtn As Object = Nothing
            HOperatorSet.GetMposition(winpreview, R, C, mBtn)
            Dim ii As Integer = Integer.Parse(mBtn.ToString)
            iBtn = ii
            iWin = 1
            Dim i As Integer = ImageListView.SelectedIndices.Item(0)
            Dim ISI As FBMlib.ImageSet '= New FBMlib.ImageSet
            ISI = objFBM.lstImages(i)
            Dim objDist As Object = Nothing
            Dim minVal As Double = Double.MaxValue
            Dim tmpSt As FBMlib.SingleTarget = Nothing
            If flgManualST = True Then
                If mBtn.I = 1 Then
                    For Each ST As FBMlib.SingleTarget In ISI.Targets.lstST
                        ST.P2D.CalcDistToInputPoint(R, C, objDist)
                        If objDist < minVal Then
                            minVal = objDist
                            tmpSt = ST
                        End If
                    Next
                    ISI.CalcRay(objFBM.hv_CamparamOut)
                    tmpManSt = tmpSt
                End If
            End If
            If flgClickPoint = True Then
                If mBtn.I = 1 Then
                    Dim tmpP2ID As Integer = ISI.Targets.GetST_MaxID

                    tmpSt = New FBMlib.SingleTarget(ISI.ImageId, tmpP2ID + 1, R, C)
                    ISI.Targets.lstST.Add(tmpSt)
                    ISI.CalcRay(objFBM.hv_CamparamOut)
                    tmpManSt = tmpSt
                End If
            End If
            If flgEdgeST = True Then
                If mBtn.I = 1 Then
                    Dim tmpP2ID As Integer = ISI.Targets.GetST_MaxID
                    tmpSt = New FBMlib.SingleTarget(ISI.ImageId, tmpP2ID + 1, R, C)
                    tmpManSt = tmpSt
                End If
                'HOperatorSet.EdgesColorSubPix(ISI.hx_Image, ISI.hx_EdgePS, "canny", 1, 30, 40)
                'Dim IsInside As Object = DBNull.Value
                'Dim hv_Selected As Object = DBNull.Value
                'HOperatorSet.DispObj(ISI.hx_EdgePS, winpreview)
                'HOperatorSet.TestXldPoint(ISI.hx_EdgePS, R, C, IsInside)

                'Call HOperatorSet.TupleSortIndex(IsInside, IsInside)
                'Call HOperatorSet.TupleInverse(IsInside, IsInside)
                'Call HOperatorSet.TupleSelect(IsInside, 0, hv_Selected)
                'If hv_Selected > 0 Then

                '    Call HOperatorSet.SelectObj(ISI.hx_EdgePS, ISI.hx_EdgePS, BTuple.TupleAdd(hv_Selected, 1))
                '    HOperatorSet.DispObj(ISI.hx_EdgePS, winpreview)
                '    flgEdgeST = False

                'End If
            End If

            If flgAllEdgeST = True Then
                'Dim hxxImage As HObject = Nothing
                'hxxImage = ISI.hx_Image
                ''GC.Collect()
                ''GC.WaitForPendingFinalizers()
                'HOperatorSet.EdgesColorSubPix(hxxImage, ISI.hx_EdgePS, "canny", 1, 10, 40)

                'HOperatorSet.DispObj(ISI.hx_EdgePS, winpreview)
                flgAllEdgeST = False
            End If

            If flgSelectC3D = True Then
                tmpSt = ISI.GetClickedTarget(R, C, minVal)

                If minVal / (objFBM.hv_Width / AxHWindowXCtrl6.Width) < 5 Then
                    tmpManSt = tmpSt
                    Dim ttt As New CLookPoint
                    If GetPosFromLabelName(CLookPoint.posTypeMode.All, tmpSt.currentLabel, ttt) <> -1 Then
                        IOUtil.blnPickPoint = True
                        IOUtil.pick_P = ttt
                        bln_ClickPoint = True
                    End If
                End If

            End If

        Catch ex As Exception
            Dim tt As Integer = 1
            'MsgBox("画面クリックイベントエラー") 'Del Kiryu 20170606 画像されていないときに画像クリックするとマウスポジションを取得できずにエラーが発生
            'MsgBox(ex.Message)
            flgEdgeST = False
            flgAllEdgeST = False
        End Try
#End If
    End Sub
    Public Sub BtnDel3DPoint()
        If tmpC3dSt Is Nothing Then
            tmpC3dSt = New FBMlib.Common3DSingleTarget
        End If
        ' flgManualST = True
        tmpManSt = Nothing
        flgCreateSTCancel = False
        flgCreateSTEnter = False
        flgDeletePoint = True
        flgSelectC3D = True
        IOUtil.WritePrompt(":[Enter]=完了/[Esc]=取り消し")

        Dim vvkey As UShort = 0
        HOperatorSet.SetMshape(win1, "crosshair") ''arrow'
        HOperatorSet.SetMshape(win2, "crosshair")
        HOperatorSet.SetMshape(win3, "crosshair")
        HOperatorSet.SetMshape(win4, "crosshair")
        HOperatorSet.SetMshape(winpreview, "crosshair")

        Do
            System.Threading.Thread.Sleep(100)
            System.Windows.Forms.Application.DoEvents()
            'System.Threading.Thread.Sleep(100)
            vvkey = GetAsyncKeyState(Keys.Escape)
            If flgCreateSTCancel = True Or vvkey = 1 Then
                tmpC3dSt = Nothing
                iBtn = 0
                'キャンセル処理
                IOUtil.WritePrompt("キャンセルしました。")
                flgCreateSTCancel = False
                Exit Do
            End If
            'If flgCreateSTEnter = True Then
            '    'キャンセル処理
            '    MsgBox("確定処理")
            '    flgCreateSTEnter = False
            '    Exit Do
            'End If
            vvkey = GetAsyncKeyState(Keys.Enter)
            Dim ccp1 As New CLookPoint
            'If IOUtil.GetPointNoPromptDoevent(ccp1, "点を指示：") = -1 Then

            'End If
            If iBtn = 1 And tmpManSt IsNot Nothing Or IOUtil.GetPointNoPromptDoevent(ccp1, "点を指示：") = 0 Then
                If tmpManSt Is Nothing Then
                    tmpManSt = New FBMlib.SingleTarget

                    Dim tid As Integer = ccp1.tid
                    If ccp1.type = 1 Then
                        For Each C3DST As FBMlib.Common3DSingleTarget In objFBM.lstCommon3dST
                            If C3DST.PID = ccp1.tid - 10000 Then
                                tmpManSt = C3DST.lstST.Item(0)
                                tmpManSt.STorCT = 1
                            End If
                        Next
                        IOUtil.WritePrompt("3Dビューワより、削除するST" & tmpManSt.P3ID & "が選択されました")
                    ElseIf ccp1.type = 2 Then
                        For Each C3DCT As FBMlib.Common3DCodedTarget In objFBM.lstCommon3dCT
                            If C3DCT.PID = ccp1.tid Then
                                tmpManSt = C3DCT.lstCT.Item(0).lstCTtoST.Item(0)
                                tmpManSt.STorCT = 2
                                Exit For
                            End If
                        Next
                        IOUtil.WritePrompt("3Dビューワより、削除するCT" & tmpManSt.P3ID & "が選択されました")
                    End If
                    tmpC3dSt.lstST.Add(tmpManSt)
                Else
                    tmpC3dSt.lstST.Add(tmpManSt)
                    IOUtil.WritePrompt("ウィンドウ " & iWin & " より、画像名：" & objFBM.lstImages(tmpManSt.ImageID - 1).ImageName & " 計測点：" & tmpManSt.currentLabel & "を選択しました。")

                End If

                DispObjByIndex(-1)
                tmpManSt = Nothing
            ElseIf flgCreateSTEnter = True Then

                IOUtil.WritePrompt("Enterキーが押されました。点の取得を終了します。")
                flgManualST = False
                flgDeletePoint = False
                If tmpC3dSt.lstST.Count >= 1 Then
                    '  Dim maxId As Integer
                    Dim Cdb As New CDBOperate
                    If Cdb.ConnectDB(m_strDataBasePath) = True Then

                        For Each objST As FBMlib.SingleTarget In tmpC3dSt.lstST

                            If objST.P3ID > 0 And objST.STorCT = 1 Then
                                Dim ii As Integer = 0
                                Dim nn As Integer = objFBM.lstCommon3dST.Count
                                For ii = nn - 1 To 0 Step -1
                                    If objFBM.lstCommon3dST.Item(ii).PID = objST.P3ID Then
                                        For Each objSST As FBMlib.SingleTarget In objFBM.lstCommon3dST.Item(ii).lstST
                                            objSST.P3ID = -1
                                        Next
                                        objFBM.lstCommon3dST.RemoveAt(ii)
                                        IOUtil.WritePrompt("3D計測点：" & objST.currentLabel & "を削除しました。")
                                        Exit For
                                    End If
                                Next
                            End If
                            If objST.STorCT = 2 Then
                                Dim nn As Integer
                                Dim ii As Integer
                                nn = objFBM.lstCommon3dCT.Count
                                For ii = nn - 1 To 0 Step -1
                                    If objFBM.lstCommon3dCT.Item(ii).currentLabel = objST.currentLabel Then
                                        For Each objP3D As FBMlib.CodedTarget In objFBM.lstCommon3dCT.Item(ii).lstCT
                                            objP3D.CT_ID = -1
                                        Next
                                        objFBM.lstCommon3dCT.RemoveAt(ii)
                                        IOUtil.WritePrompt("3D計測点：" & objST.currentLabel & "を削除しました。")
                                        Exit For
                                    End If
                                Next
                            End If
                        Next

                        '計測DB更新＆３DVIEW再描画
                        Cdb.DisConnectDB()
                        objFBM.SaveToMeasureDataDBNewPoint(objFBM.ProjectPath & "\")
                        'YMC_3DViewReDraw_NoKousin(m_strDataBasePath)

                        '終了メッセージ出力

                    End If
                Else

                End If
                tmpC3dSt = Nothing
                iBtn = 0
                Exit Do
            End If

        Loop

        HOperatorSet.SetMshape(win1, "arrow")
        HOperatorSet.SetMshape(win2, "arrow")
        HOperatorSet.SetMshape(win3, "arrow")
        HOperatorSet.SetMshape(win4, "arrow")
        HOperatorSet.SetMshape(winpreview, "arrow")
        flgKoushin3DPoint = True
    End Sub
    '20170222 baluu add start
    Public Sub BtnDelPointsFrom3D()
        IOUtil.EndThread()
        m_blnSelectDelArea = True
        m_blnDrawRegion = True '20170224 baluu add
        m_blnCommandArea = True
    End Sub

    Public Sub DeletePointsFrom3D(ByVal indices As List(Of Integer))
        If AskDelete() = True Then
            For Each index As Integer In indices
                Dim lp As CLookPoint = gDrawPoints(index)
                If lp.type = 1 Then
                    Dim nn As Integer = objFBM.lstCommon3dST.Count
                    Dim ii As Integer = 0
                    For ii = nn - 1 To 0 Step -1
                        If objFBM.lstCommon3dST.Item(ii).TID = lp.tid Then
                            For Each objSST As FBMlib.SingleTarget In objFBM.lstCommon3dST.Item(ii).lstST
                                objSST.P3ID = -1
                            Next
                            objFBM.lstCommon3dST.RemoveAt(ii)
                            IOUtil.WritePrompt("3D計測点：" & lp.LabelName & "を削除しました。")
                            Exit For
                        End If
                    Next
                ElseIf lp.type = 2 Then
                    Dim nn As Integer
                    Dim ii As Integer
                    nn = objFBM.lstCommon3dCT.Count
                    For ii = nn - 1 To 0 Step -1
                        If objFBM.lstCommon3dCT.Item(ii).PID = lp.tid Then
                            For Each objP3D As FBMlib.CodedTarget In objFBM.lstCommon3dCT.Item(ii).lstCT
                                objP3D.CT_ID = -1
                            Next
                            objFBM.lstCommon3dCT.RemoveAt(ii)
                            IOUtil.WritePrompt("3D計測点：" & lp.LabelName & "を削除しました。")
                            Exit For
                        End If
                    Next
                End If
            Next
            objFBM.SaveToMeasureDataDBNewPoint(objFBM.ProjectPath & "\")
            flgKoushin3DPoint = True
            AllDraw()
        End If
    End Sub
    '20170222 baluu add end

    Public Sub AddUserPoint3D_FourDisp_Extra()
        If tmpC3dSt Is Nothing Then
            tmpC3dSt = New FBMlib.Common3DSingleTarget
        End If
        flgDisableColorPart = True '20170331 baluu add
        flgManualST = True
        tmpManSt = Nothing
        flgCreateSTCancel = False
        flgCreateSTEnter = False
        flgDeletePoint = False
        '20160420 不具合修正　SUSANO　START
        For Each ISI2 As FBMlib.ImageSet In objFBM.lstImages
            If ISI2.hx_EpiLineAsOtherPoint IsNot Nothing Then
                Try
                    ClearHObject(ISI2.hx_EpiLineAsOtherPoint)
                Catch ex As Exception
                End Try
            End If
            ISI2.hx_EpiLineAsOtherPoint = Nothing
        Next
        '20160420 不具合修正　SUSANO　END
        ' Btn_SetHistoriText(":[Enter]=完了/[Esc]=取り消し")
        IOUtil.WritePrompt(":[Enter]=完了/[Esc]=取り消し")
        Dim vvkey As UShort = 0
        Do
            System.Threading.Thread.Sleep(300)
            System.Windows.Forms.Application.DoEvents()
            'System.Threading.Thread.Sleep(100)
            vvkey = GetAsyncKeyState(Keys.Escape)
            If flgCreateSTCancel = True Or vvkey = 1 Then
                tmpC3dSt = Nothing
                iBtn = 0
                'キャンセル処理
                ' Btn_SetHistoriText("キャンセルしました。")
                IOUtil.WritePrompt("キャンセルしました。")
                flgCreateSTCancel = False
                Exit Do
            End If
            'If flgCreateSTEnter = True Then
            '    'キャンセル処理
            '    MsgBox("確定処理")
            '    flgCreateSTEnter = False
            '    Exit Do
            'End If
            vvkey = GetAsyncKeyState(Keys.Enter)
            If iBtn = 1 And tmpManSt IsNot Nothing Then
                tmpC3dSt.lstST.Add(tmpManSt)
                If tmpC3dSt.lstST.Count = 1 Then
                    'エピポーラ線の算出及び各画像への投影
                    CalcAllEpiLinebyOnePoint(tmpManSt)
                End If
                DispObjByIndex(-1)
                'DispObjects(iWin, tmpManSt.ImageID)
                '  DispOneObjByIndex(iWin, tmpManSt.ImageID - 1)

                ' Btn_SetHistoriText("ウィンドウ " & iWin & " より、画像名：" & objFBM.lstImages(tmpManSt.ImageID - 1).ImageName & " 計測点：" & tmpManSt.P2ID & "を選択しました。")
                IOUtil.WritePrompt("ウィンドウ " & iWin & " より、画像名：" & objFBM.lstImages(tmpManSt.ImageID - 1).ImageName & " 計測点：" & tmpManSt.P2ID & "を選択しました。")
                tmpManSt = Nothing
            ElseIf vvkey = 1 Or flgCreateSTEnter = True Then
                '  Btn_SetHistoriText("Enterキーが押されました。点の取得を終了します。")
                IOUtil.WritePrompt("Enterキーが押されました。点の取得を終了します。")
                flgManualST = False
                flgDeletePoint = False
                If tmpC3dSt.lstST.Count >= 2 Then
                    Dim maxId As Integer

                    Dim Cdb As New CDBOperate
                    If Cdb.ConnectDB(m_strDataBasePath) = True Then

                        '選択点がobjFMBに登録されているかチェック
                        Dim flg As Boolean = True
                        Dim C3DST As FBMlib.Common3DSingleTarget
                        Dim tmpC3dSt_E As List(Of FBMlib.SingleTarget) = New List(Of FBMlib.SingleTarget)
                        Dim tmpC3dSt_N As List(Of FBMlib.SingleTarget) = New List(Of FBMlib.SingleTarget)
                        Dim tmpC3dSt_C As List(Of FBMlib.Common3DSingleTarget) = New List(Of FBMlib.Common3DSingleTarget)
                        For Each ST As FBMlib.SingleTarget In tmpC3dSt.lstST
                            C3DST = objFBM.IsAruCommon3DSingleTarget(ST)
                            If C3DST IsNot Nothing Then
                                tmpC3dSt_E.Add(ST)
                                tmpC3dSt_C.Add(C3DST)
                            Else
                                tmpC3dSt_N.Add(ST)
                            End If
                        Next

                        '全て登録されていない場合
                        If tmpC3dSt_C.Count = 0 Then
                            Dim strSql As String = ""
                            Dim adoRet As ADODB.Recordset
                            strSql = "select max(TID) as maxID from measurepoint3d "
                            adoRet = Cdb.CreateRecordset(strSql)
                            If adoRet Is Nothing Then
                            Else
                                If adoRet.RecordCount > 0 Then
                                    Do Until adoRet.EOF
                                        maxId = adoRet("maxID").Value
                                        adoRet.MoveNext()
                                    Loop
                                End If
                            End If
                            tmpC3dSt.Calc3dPoints()
                            If maxId < 10000 Then
                                maxId = 10000
                            End If
                            tmpC3dSt.PID = maxId - 10000 + 1
                            For Each ST As FBMlib.SingleTarget In tmpC3dSt.lstST
                                ST.P3ID = tmpC3dSt.PID
                            Next
                            objFBM.lstCommon3dST.Add(tmpC3dSt)
                        End If

                        '登録されている場合
                        Dim C3DST1 As FBMlib.Common3DSingleTarget
                        If tmpC3dSt_C.Count > 0 Then
                            C3DST1 = objFBM.IsAruCommon3DSingleTarget(tmpC3dSt_E(0))
                            If tmpC3dSt_C.Count >= 2 Then   '２点以上登録されている場合
                                Dim ii As Integer
                                For ii = 1 To tmpC3dSt_C.Count - 1
                                    If C3DST1.PID <> tmpC3dSt_C(ii).PID Then
                                        flg = False
                                        Exit For
                                    End If
                                Next ii
                            End If

                            '２D点登録＆３D点再計算
                            If flg = True Then
                                If tmpC3dSt_N.Count > 0 Then
                                    Dim jj As Integer
                                    For jj = 0 To tmpC3dSt_N.Count - 1
                                        tmpC3dSt_N(jj).P3ID = C3DST1.PID
                                        C3DST1.lstST.Add(tmpC3dSt_N(jj))
                                    Next jj
                                End If
                                C3DST1.Calc3dPoints()
                                tmpC3dSt = C3DST1
                            End If

                        End If

                        '計測DB更新＆３DVIEW再描画
                        Cdb.DisConnectDB()
                        objFBM.SaveToMeasureDataDBNewPoint(objFBM.ProjectPath & "\")
                        ' YMC_3DViewReDraw_NoKousin(m_strDataBasePath)

                        '終了メッセージ出力
                        If flg = True Then
                            If tmpC3dSt_C.Count = 0 Then
                                'Btn_SetHistoriText("3D計測点：" & tmpC3dSt.systemlabel & "を追加しました。")
                                IOUtil.WritePrompt("3D計測点：" & tmpC3dSt.systemlabel & "を追加しました。")
                            Else
                                IOUtil.WritePrompt("3D計測点：" & tmpC3dSt.systemlabel & "を再計算しました。")
                            End If
                        Else
                            MsgBox("選択した点に異なる３D計測点が存在します。" & vbCrLf & _
                                   "点を選択し直してください。")
                        End If
                    End If
                Else
                    MsgBox("２点以上選択してください。")
                End If
                tmpC3dSt = Nothing
                iBtn = 0
                Exit Do
            End If

        Loop
        flgDeletePoint = False
        flgKoushin3DPoint = True
        flgDisableColorPart = False '20170331 baluu add
    End Sub


    '４画面の各画面のマウスクリックイベントを受け取り、実処理を行う
    Public Sub AddUserPoint3D_FourDisp()
        If tmpC3dSt Is Nothing Then
            tmpC3dSt = New FBMlib.Common3DSingleTarget
        End If
        flgManualST = True
        tmpManSt = Nothing
        Do
            System.Windows.Forms.Application.DoEvents()
            If iBtn = 1 And tmpManSt IsNot Nothing Then
                tmpC3dSt.lstST.Add(tmpManSt)
                MsgBox("ウィンドウ " & iWin & " より、計測点：" & tmpManSt.P2ID & "を選択しました。")
                tmpManSt = Nothing
            ElseIf iBtn = 4 Then
                MsgBox("マウス右ボタンが押されました。点の取得を終了します。")
                flgManualST = False
                If tmpC3dSt.lstST.Count >= 2 Then
                    Dim maxId As Integer

                    Dim Cdb As New CDBOperate
                    If Cdb.ConnectDB(m_strDataBasePath) = True Then

                        '選択点がobjFMBに登録されているかチェック
                        Dim flg As Boolean = True
                        Dim C3DST As FBMlib.Common3DSingleTarget
                        Dim tmpC3dSt_E As List(Of FBMlib.SingleTarget) = New List(Of FBMlib.SingleTarget)
                        Dim tmpC3dSt_N As List(Of FBMlib.SingleTarget) = New List(Of FBMlib.SingleTarget)
                        Dim tmpC3dSt_C As List(Of FBMlib.Common3DSingleTarget) = New List(Of FBMlib.Common3DSingleTarget)
                        For Each ST As FBMlib.SingleTarget In tmpC3dSt.lstST
                            C3DST = objFBM.IsAruCommon3DSingleTarget(ST)
                            If C3DST IsNot Nothing Then
                                tmpC3dSt_E.Add(ST)
                                tmpC3dSt_C.Add(C3DST)
                            Else
                                tmpC3dSt_N.Add(ST)
                            End If
                        Next

                        '全て登録されていない場合
                        If tmpC3dSt_C.Count = 0 Then
                            Dim strSql As String = ""
                            Dim adoRet As ADODB.Recordset
                            strSql = "select max(TID) as maxID from measurepoint3d "
                            adoRet = Cdb.CreateRecordset(strSql)
                            If adoRet Is Nothing Then
                            Else
                                If adoRet.RecordCount > 0 Then
                                    Do Until adoRet.EOF
                                        maxId = adoRet("maxID").Value
                                        adoRet.MoveNext()
                                    Loop
                                End If
                            End If
                            tmpC3dSt.Calc3dPoints()
                            If maxId < 10000 Then
                                maxId = 10000
                            End If
                            tmpC3dSt.PID = maxId - 10000 + 1
                            For Each ST As FBMlib.SingleTarget In tmpC3dSt.lstST
                                ST.P3ID = tmpC3dSt.PID
                            Next
                            objFBM.lstCommon3dST.Add(tmpC3dSt)
                        End If

                        '登録されている場合
                        Dim C3DST1 As FBMlib.Common3DSingleTarget
                        If tmpC3dSt_C.Count > 0 Then
                            C3DST1 = objFBM.IsAruCommon3DSingleTarget(tmpC3dSt_E(0))
                            If tmpC3dSt_C.Count >= 2 Then   '２点以上登録されている場合
                                Dim ii As Integer
                                For ii = 1 To tmpC3dSt_C.Count - 1
                                    If C3DST1.PID <> tmpC3dSt_C(ii).PID Then
                                        flg = False
                                        Exit For
                                    End If
                                Next ii
                            End If

                            '２D点登録＆３D点再計算
                            If flg = True Then
                                If tmpC3dSt_N.Count > 0 Then
                                    Dim jj As Integer
                                    For jj = 0 To tmpC3dSt_N.Count - 1
                                        tmpC3dSt_N(jj).P3ID = C3DST1.PID
                                        C3DST1.lstST.Add(tmpC3dSt_N(jj))
                                    Next jj
                                End If
                                C3DST1.Calc3dPoints()
                                tmpC3dSt = C3DST1
                            End If

                        End If

                        '計測DB更新＆３DVIEW再描画
                        Cdb.DisConnectDB()
                        objFBM.SaveToMeasureDataDBNewPoint(objFBM.ProjectPath & "\")
                        YMC_3DViewReDraw_NoKousin(m_strDataBasePath)

                        '終了メッセージ出力
                        If flg = True Then
                            If tmpC3dSt_C.Count = 0 Then
                                MsgBox("3D計測点：" & tmpC3dSt.systemlabel & "を追加しました。")
                            Else
                                MsgBox("3D計測点：" & tmpC3dSt.systemlabel & "を再計算しました。")
                            End If
                        Else
                            MsgBox("選択した点に異なる３D計測点が存在します。" & vbCrLf & _
                                   "点を選択し直してください。")
                        End If
                    End If
                Else
                    MsgBox("２点以上選択してください。")
                End If
                tmpC3dSt = Nothing
                iBtn = 0
                Exit Do
            End If

        Loop
    End Sub
    Public Sub GetEdgeToPoint3d()
        Dim intEdge2dPointMax = GetPrivateProfileInt("Kaiseki", "Edge2dPointMax", 1, My.Application.Info.DirectoryPath & "\vform.ini")
        'Add Kiryu 20160420 エッジ抽出パラメータをiniファイル読み込みに変更
        Dim mult = GetPrivateProfileInt("Kaiseki", "mult", 1, My.Application.Info.DirectoryPath & "\vform.ini")
        Dim alpha = GetPrivateProfileInt("Kaiseki", "alpha", 1, My.Application.Info.DirectoryPath & "\vform.ini")
        Dim low = GetPrivateProfileInt("Kaiseki", "low", 1, My.Application.Info.DirectoryPath & "\vform.ini")
        Dim hi = GetPrivateProfileInt("Kaiseki", "hi", 1, My.Application.Info.DirectoryPath & "\vform.ini")

        If tmpC3dSt Is Nothing Then
            tmpC3dSt = New FBMlib.Common3DSingleTarget
        End If
        flgClickPoint = False
        flgEdgeST = True
        tmpManSt = Nothing
        flgCreateSTCancel = False
        flgCreateSTEnter = False
        flgDeletePoint = False
        flgDisableColorPart = True '20170331 baluu add start
        IOUtil.WritePrompt("エッジ抽出する範囲(ドラッグ):")
        IOUtil.WritePrompt("右クリックで確定：")
        Dim vvkey As UShort = 0
        For Each ISI2 As FBMlib.ImageSet In objFBM.lstImages
            If ISI2.hx_EdgePS IsNot Nothing Then
                Try
                    ClearHObject(ISI2.hx_EdgePS)
                Catch ex As Exception
                End Try
            End If
            If ISI2.hx_EdgeRec IsNot Nothing Then
                Try
                    ClearHObject(ISI2.hx_EdgeRec)
                Catch ex As Exception
                End Try
            End If

            ISI2.hx_EdgePS = Nothing
            ISI2.hx_EdgeRec = Nothing
        Next
        Do
            System.Threading.Thread.Sleep(200)
            System.Windows.Forms.Application.DoEvents()

            vvkey = GetAsyncKeyState(Keys.Escape)
            If flgCreateSTCancel = True Or vvkey = 1 Then
                If tmpC3dSt.lstST.Count > 1 Then
                    Dim tmpImgID As Integer = tmpC3dSt.lstST.Item(tmpC3dSt.lstST.Count - 1).ImageID
                    If Not objFBM.lstImages.Item(tmpImgID - 1).hx_EdgePS Is Nothing Then
                        Try
                            ClearHObject(objFBM.lstImages.Item(tmpImgID - 1).hx_EdgePS)
                        Catch ex As Exception

                        End Try
                        objFBM.lstImages.Item(tmpImgID - 1).hx_EdgePS = Nothing
                        Try
                            ClearHObject(objFBM.lstImages.Item(tmpImgID - 1).hx_EdgeRec)
                        Catch ex As Exception

                        End Try
                        objFBM.lstImages.Item(tmpImgID - 1).hx_EdgeRec = Nothing
                    End If
                    iBtn = 0
                    flgCreateSTCancel = False
                    tmpC3dSt.lstST.RemoveAt(tmpC3dSt.lstST.Count - 1)
                    IOUtil.WritePrompt("エッジ抽出範囲選択処理をキャンセルしました。")

                Else
                    tmpC3dSt = Nothing
                    iBtn = 0
                    'キャンセル処理
                    IOUtil.WritePrompt("エッジ抽出処理をキャンセルしました。")
                    flgCreateSTCancel = False
                    Exit Do
                End If
                DispObjByIndex(-1)
            End If
            vvkey = GetAsyncKeyState(Keys.Enter)
            If iBtn = 4 And tmpManSt IsNot Nothing Then
                If tmpManSt.TP2D.Row IsNot Nothing Then
                    Dim tmpC3Dstcount As Integer = tmpC3dSt.lstST.Count
                    For i = tmpC3Dstcount - 1 To 0 Step -1
                        If tmpC3dSt.lstST.Item(i).ImageID = tmpManSt.ImageID Then
                            tmpC3dSt.lstST.RemoveAt(i)
                        End If
                    Next

                    Dim minPointR As Object = Nothing
                    Dim minPointC As Object = Nothing
                    Dim countEdge As Object = Nothing
                    Dim countEdge2dPoint As Object = Nothing

                    'For Each ISI2 As FBMlib.ImageSet In objFBM.lstImages
                    For Each ISI2 As FBMlib.ImageSet In objFBM.lstImages

                        If ISI2.ImageId = tmpManSt.ImageID Then
                            Dim tmpWH As Integer = 50
                            Dim objTmpImg As HObject = Nothing
                            Dim objTmpOrgImg As HObject = Nothing
                            HOperatorSet.GenEmptyObj(objTmpImg)
                            HOperatorSet.GenEmptyObj(objTmpOrgImg)
                            ClearHObject(objTmpImg)
                            ClearHObject(objTmpOrgImg)
                            If Not ISI2.hx_EdgeRec Is Nothing Then
                                Try
                                    ClearHObject(ISI2.hx_EdgeRec)
                                Catch ex As Exception

                                End Try

                            End If
                            If Not ISI2.hx_EdgePS Is Nothing Then
                                Try
                                    ClearHObject(ISI2.hx_EdgePS)
                                Catch ex As Exception

                                End Try

                            End If
                            'HOperatorSet.GenRectangle1(objTmpRec, tmpManSt.P2D.Row - tmpWH, tmpManSt.P2D.Col - tmpWH, tmpManSt.P2D.Row + tmpWH, tmpManSt.P2D.Col + tmpWH)
                            HOperatorSet.GenRectangle1(ISI2.hx_EdgeRec, tmpManSt.P2D.Row, tmpManSt.P2D.Col, tmpManSt.TP2D.Row, tmpManSt.TP2D.Col)
                            HOperatorSet.ReadImage(objTmpOrgImg, ISI2.ImageFullPath)
                            HOperatorSet.AddImage(objTmpOrgImg, objTmpOrgImg, objTmpImg, mult, 0.0)
                            ClearHObject(objTmpOrgImg)
                            HOperatorSet.ReduceDomain(objTmpImg, ISI2.hx_EdgeRec, objTmpOrgImg)
                            HOperatorSet.GenEmptyObj(ISI2.hx_EdgePS)
                            HOperatorSet.EdgesSubPix(objTmpOrgImg, ISI2.hx_EdgePS, "canny", alpha, low, hi)
                            HOperatorSet.CountObj(ISI2.hx_EdgePS, countEdge)
                            Dim NN As Object = Nothing
                            HOperatorSet.ContourPointNumXld(ISI2.hx_EdgePS, NN)
                            countEdge2dPoint = BTuple.TupleSum(NN)
                            ClearHObject(objTmpImg)
                            ClearHObject(objTmpOrgImg)

                            Exit For
                        End If
                    Next

                    tmpC3dSt.lstST.Add(tmpManSt)
                    If countEdge.I = 0 Then
                        IOUtil.WritePrompt("ウィンドウ " & iWin & " より、エッジ抽出されませんでした")

                    Else
                        IOUtil.WritePrompt("ウィンドウ " & iWin & " より、エッジ選択されました。(2次元点数：" & countEdge2dPoint.D & "個検出されました)")
                        If intEdge2dPointMax < countEdge2dPoint Then
                            IOUtil.WritePrompt("処理可能な点数（" & intEdge2dPointMax & ")を超えました")
                        End If
                    End If

                    DispObjByIndex(-1)

                    tmpManSt = Nothing

                End If

            ElseIf vvkey = 1 Or flgCreateSTEnter = True Then
                IOUtil.WritePrompt("Enterキーが押されました。エッジ取得を終了します。")
                flgClickPoint = False
                flgDeletePoint = False
                If tmpC3dSt.lstST.Count >= 3 Then
                    Dim mId As Integer = 0
                    Dim objTmpFbm As New FBMlib.FeatureImage(objFBM)

                    If objTmpFbm.CreateSTbyEdge() = True Then
                        Dim intAddPoints As Integer = objFBM.AddStByEdge(objTmpFbm)
                        If intAddPoints > 0 Then
                            objFBM.SaveToMeasureDataDBNewPoint(objFBM.ProjectPath & "\")
                            IOUtil.WritePrompt("エッジによる3D計測点：" & intAddPoints & "個を追加しました。")
                        End If

                        '  MsgBox("test createstbyedge end")
                        '計測DB更新＆３DVIEW再描画

                    End If


                    '  YMC_3DViewReDraw_NoKousin(m_strDataBasePath)

                    '終了メッセージ出力

                    'If flg = True Then
                    '    If tmpC3dSt_C.Count = 0 Then

                    '    Else
                    '        IOUtil.WritePrompt("3D計測点：" & tmpC3dSt.systemlabel & "を再計算しました。")
                    '    End If
                    'Else
                    '    MsgBox("選択した点に異なる３D計測点が存在します。" & vbCrLf & _
                    '           "点を選択し直してください。")
                    'End If
                    DispObjByIndex(-1)
                Else
                    MsgBox("２点以上選択してください。")
                End If
                tmpC3dSt = Nothing
                iBtn = 0
                Exit Do
            End If

        Loop
        flgDeletePoint = False
        flgKoushin3DPoint = True
        flgEdgeST = False
        flgDisableColorPart = False '20170331 baluu add
    End Sub
    Public Sub AddUserClickPoint3D_FourDisp_extra()
        If tmpC3dSt Is Nothing Then
            tmpC3dSt = New FBMlib.Common3DSingleTarget
        End If
        flgClickPoint = True
        tmpManSt = Nothing
        flgCreateSTCancel = False
        flgCreateSTEnter = False
        flgDeletePoint = True
        flgDisableColorPart = True ' 20170331 baluu add
        '20160420 不具合修正　SUSANO　START
        For Each ISI2 As FBMlib.ImageSet In objFBM.lstImages
            If ISI2.hx_EpiLineAsOtherPoint IsNot Nothing Then
                Try
                    ClearHObject(ISI2.hx_EpiLineAsOtherPoint)
                Catch ex As Exception
                End Try
            End If

            ISI2.hx_EpiLineAsOtherPoint = Nothing
        Next
        '20160420 不具合修正　SUSANO　END
        IOUtil.WritePrompt(":[Enter]=完了/[Esc]=取り消し")
        Dim vvkey As UShort = 0
        Do
            System.Threading.Thread.Sleep(200)
            System.Windows.Forms.Application.DoEvents()

            vvkey = GetAsyncKeyState(Keys.Escape)
            If flgCreateSTCancel = True Or vvkey = 1 Then
                tmpC3dSt = Nothing
                iBtn = 0
                'キャンセル処理
                IOUtil.WritePrompt("キャンセルしました。")
                flgCreateSTCancel = False
                Exit Do
            End If
            vvkey = GetAsyncKeyState(Keys.Enter)
            If iBtn = 1 And tmpManSt IsNot Nothing Then
                'Dim minPointR As Object = Nothing
                'Dim minPointC As Object = Nothing

                For Each ISI2 As FBMlib.ImageSet In objFBM.lstImages
                    If ISI2.ImageId = tmpManSt.ImageID Then
                        Dim RR As HTuple = Nothing
                        Dim CC As HTuple = Nothing
                        If ISI2.hx_EpiLineAsOtherPoint Is Nothing Then
                            Exit For
                        End If
                        Try
                            HOperatorSet.GetContourXld(ISI2.hx_EpiLineAsOtherPoint, RR, CC)
                        Catch ex As Exception
                            Exit For
                        End Try


                        Dim nn As HTuple = Nothing
                        nn = BTuple.TupleLength(RR)
                        If nn > 2 Then
                            Dim ii As Integer
                            Dim minD As Double = Double.MaxValue
                            For ii = 0 To nn - 1
                                Dim ddd As HTuple = Nothing
                                HOperatorSet.DistancePp(tmpManSt.P2D.Row, tmpManSt.P2D.Col, RR(ii), CC(ii), ddd)
                                If minD > ddd.D Then
                                    minD = ddd
                                    tmpManSt.P2D.Row = RR(ii)
                                    tmpManSt.P2D.Col = CC(ii)
                                End If
                            Next
                        ElseIf nn.I = 2 Then
                            Dim gline As New GeoCurve
                            Dim g1 As New GeoPoint
                            Dim g2 As New GeoPoint
                            Dim gg As New GeoPoint
                            g1.SetXY(RR(0), CC(0))
                            g2.SetXY(RR(1), CC(1))
                            gline.SetLine(g1, g2)
                            gg.SetXY(tmpManSt.P2D.Row, tmpManSt.P2D.Col)
                            Dim kouten As New GeoPoint

                            kouten = gline.GetPerpendicularFoot(gg)
                            tmpManSt.P2D.Row = kouten.x
                            tmpManSt.P2D.Col = kouten.y

                        End If

                        Exit For
                    End If
                Next

                tmpC3dSt.lstST.Add(tmpManSt)
                IOUtil.WritePrompt("ウィンドウ " & iWin & " より、計測点：" & tmpManSt.P2ID & "を選択しました。")
                If tmpC3dSt.lstST.Count = 1 Then
                    'エピポーラ線の算出及び各画像への投影
                    CalcAllEpiLinebyOnePoint(tmpManSt)
                End If
                DispObjByIndex(-1)
                tmpManSt = Nothing
            ElseIf vvkey = 1 Or flgCreateSTEnter = True Then
                IOUtil.WritePrompt("Enterキーが押されました。点の取得を終了します。")
                flgClickPoint = False
                flgDeletePoint = False
                If tmpC3dSt.lstST.Count >= 2 Then
                    Dim maxId As Integer

                    Dim Cdb As New CDBOperate
                    If Cdb.ConnectDB(m_strDataBasePath) = True Then

                        '選択点がobjFMBに登録されているかチェック
                        Dim flg As Boolean = True
                        Dim C3DST As FBMlib.Common3DSingleTarget
                        Dim tmpC3dSt_E As List(Of FBMlib.SingleTarget) = New List(Of FBMlib.SingleTarget)
                        Dim tmpC3dSt_N As List(Of FBMlib.SingleTarget) = New List(Of FBMlib.SingleTarget)
                        Dim tmpC3dSt_C As List(Of FBMlib.Common3DSingleTarget) = New List(Of FBMlib.Common3DSingleTarget)
                        For Each ST As FBMlib.SingleTarget In tmpC3dSt.lstST
                            C3DST = objFBM.IsAruCommon3DSingleTarget(ST)
                            If C3DST IsNot Nothing Then
                                tmpC3dSt_E.Add(ST)
                                tmpC3dSt_C.Add(C3DST)
                            Else
                                tmpC3dSt_N.Add(ST)
                            End If
                        Next

                        '全て登録されていない場合
                        If tmpC3dSt_C.Count = 0 Then
                            Dim strSql As String = ""
                            Dim adoRet As ADODB.Recordset
                            strSql = "select max(TID) as maxID from measurepoint3d "
                            adoRet = Cdb.CreateRecordset(strSql)
                            If adoRet Is Nothing Then
                            Else
                                If adoRet.RecordCount > 0 Then
                                    Do Until adoRet.EOF
                                        maxId = adoRet("maxID").Value
                                        adoRet.MoveNext()
                                    Loop
                                End If
                            End If
                            tmpC3dSt.Calc3dPoints()
                            If maxId < 10000 Then
                                maxId = 10000
                            End If
                            tmpC3dSt.PID = maxId - 10000 + 1
                            For Each ST As FBMlib.SingleTarget In tmpC3dSt.lstST
                                ST.P3ID = tmpC3dSt.PID
                            Next
                            objFBM.lstCommon3dST.Add(tmpC3dSt)
                        End If

                        '登録されている場合
                        Dim C3DST1 As FBMlib.Common3DSingleTarget
                        If tmpC3dSt_C.Count > 0 Then
                            C3DST1 = objFBM.IsAruCommon3DSingleTarget(tmpC3dSt_E(0))
                            If tmpC3dSt_C.Count >= 2 Then   '２点以上登録されている場合
                                Dim ii As Integer
                                For ii = 1 To tmpC3dSt_C.Count - 1
                                    If C3DST1.PID <> tmpC3dSt_C(ii).PID Then
                                        flg = False
                                        Exit For
                                    End If
                                Next ii
                            End If

                            '２D点登録＆３D点再計算
                            If flg = True Then
                                If tmpC3dSt_N.Count > 0 Then
                                    Dim jj As Integer
                                    For jj = 0 To tmpC3dSt_N.Count - 1
                                        tmpC3dSt_N(jj).P3ID = C3DST1.PID
                                        C3DST1.lstST.Add(tmpC3dSt_N(jj))
                                    Next jj
                                End If
                                C3DST1.Calc3dPoints()
                                tmpC3dSt = C3DST1
                                C3DST1.STtype = 2 'susano add 20160105
                            End If

                        End If
                        tmpC3dSt.STtype = 2 'susano add 20160105
                        For Each ST As FBMlib.SingleTarget In tmpC3dSt.lstST
                            ST.stType = 2
                        Next
                        '計測DB更新＆３DVIEW再描画
                        Cdb.DisConnectDB()
                        objFBM.SaveToMeasureDataDBNewPoint(objFBM.ProjectPath & "\")
                        ' YMC_3DViewReDraw_NoKousin(m_strDataBasePath)

                        '終了メッセージ出力
                        If flg = True Then
                            If tmpC3dSt_C.Count = 0 Then
                                IOUtil.WritePrompt("3D計測点：" & tmpC3dSt.systemlabel & "を追加しました。")
                            Else
                                IOUtil.WritePrompt("3D計測点：" & tmpC3dSt.systemlabel & "を再計算しました。")
                            End If
                        Else
                            MsgBox("選択した点に異なる３D計測点が存在します。" & vbCrLf & _
                                   "点を選択し直してください。")
                        End If
                        DispObjByIndex(-1)
                    End If
                Else
                    MsgBox("２点以上選択してください。")
                End If
                tmpC3dSt = Nothing
                iBtn = 0
                Exit Do
            End If

        Loop
        flgDeletePoint = False
        flgKoushin3DPoint = True
        flgDisableColorPart = False '20170331 baluu add
    End Sub

    Public Sub AddUserClickPoint3D_FourDisp()
        If tmpC3dSt Is Nothing Then
            tmpC3dSt = New FBMlib.Common3DSingleTarget
        End If
        flgClickPoint = True
        tmpManSt = Nothing
        Do
            System.Windows.Forms.Application.DoEvents()
            If iBtn = 1 And tmpManSt IsNot Nothing Then
                tmpC3dSt.lstST.Add(tmpManSt)
                MsgBox("ウィンドウ " & iWin & " より、計測点：" & tmpManSt.P2ID & "を選択しました。")
                tmpManSt = Nothing
            ElseIf iBtn = 4 Then
                MsgBox("マウス右ボタンが押されました。点の取得を終了します。")
                flgClickPoint = False
                If tmpC3dSt.lstST.Count >= 2 Then
                    Dim maxId As Integer

                    Dim Cdb As New CDBOperate
                    If Cdb.ConnectDB(m_strDataBasePath) = True Then

                        '選択点がobjFMBに登録されているかチェック
                        Dim flg As Boolean = True
                        Dim C3DST As FBMlib.Common3DSingleTarget
                        Dim tmpC3dSt_E As List(Of FBMlib.SingleTarget) = New List(Of FBMlib.SingleTarget)
                        Dim tmpC3dSt_N As List(Of FBMlib.SingleTarget) = New List(Of FBMlib.SingleTarget)
                        Dim tmpC3dSt_C As List(Of FBMlib.Common3DSingleTarget) = New List(Of FBMlib.Common3DSingleTarget)
                        For Each ST As FBMlib.SingleTarget In tmpC3dSt.lstST
                            C3DST = objFBM.IsAruCommon3DSingleTarget(ST)
                            If C3DST IsNot Nothing Then
                                tmpC3dSt_E.Add(ST)
                                tmpC3dSt_C.Add(C3DST)
                            Else
                                tmpC3dSt_N.Add(ST)
                            End If
                        Next

                        '全て登録されていない場合
                        If tmpC3dSt_C.Count = 0 Then
                            Dim strSql As String = ""
                            Dim adoRet As ADODB.Recordset
                            strSql = "select max(TID) as maxID from measurepoint3d "
                            adoRet = Cdb.CreateRecordset(strSql)
                            If adoRet Is Nothing Then
                            Else
                                If adoRet.RecordCount > 0 Then
                                    Do Until adoRet.EOF
                                        maxId = adoRet("maxID").Value
                                        adoRet.MoveNext()
                                    Loop
                                End If
                            End If
                            tmpC3dSt.Calc3dPoints()
                            If maxId < 10000 Then
                                maxId = 10000
                            End If
                            tmpC3dSt.PID = maxId - 10000 + 1
                            For Each ST As FBMlib.SingleTarget In tmpC3dSt.lstST
                                ST.P3ID = tmpC3dSt.PID
                            Next
                            objFBM.lstCommon3dST.Add(tmpC3dSt)
                        End If

                        '登録されている場合
                        Dim C3DST1 As FBMlib.Common3DSingleTarget
                        If tmpC3dSt_C.Count > 0 Then
                            C3DST1 = objFBM.IsAruCommon3DSingleTarget(tmpC3dSt_E(0))
                            If tmpC3dSt_C.Count >= 2 Then   '２点以上登録されている場合
                                Dim ii As Integer
                                For ii = 1 To tmpC3dSt_C.Count - 1
                                    If C3DST1.PID <> tmpC3dSt_C(ii).PID Then
                                        flg = False
                                        Exit For
                                    End If
                                Next ii
                            End If

                            '２D点登録＆３D点再計算
                            If flg = True Then
                                If tmpC3dSt_N.Count > 0 Then
                                    Dim jj As Integer
                                    For jj = 0 To tmpC3dSt_N.Count - 1
                                        tmpC3dSt_N(jj).P3ID = C3DST1.PID
                                        C3DST1.lstST.Add(tmpC3dSt_N(jj))
                                    Next jj
                                End If
                                C3DST1.Calc3dPoints()
                                tmpC3dSt = C3DST1
                            End If

                        End If

                        '計測DB更新＆３DVIEW再描画
                        Cdb.DisConnectDB()
                        objFBM.SaveToMeasureDataDBNewPoint(objFBM.ProjectPath & "\")
                        YMC_3DViewReDraw_NoKousin(m_strDataBasePath)

                        '終了メッセージ出力
                        If flg = True Then
                            If tmpC3dSt_C.Count = 0 Then
                                MsgBox("3D計測点：" & tmpC3dSt.systemlabel & "を追加しました。")
                            Else
                                MsgBox("3D計測点：" & tmpC3dSt.systemlabel & "を再計算しました。")
                            End If
                        Else
                            MsgBox("選択した点に異なる３D計測点が存在します。" & vbCrLf & _
                                   "点を選択し直してください。")
                        End If
                    End If
                Else
                    MsgBox("２点以上選択してください。")
                End If
                tmpC3dSt = Nothing
                iBtn = 0
                Exit Do
            End If

        Loop
    End Sub

    Public Sub ShowAutoOffsetCalc()

        IOUtil.EndThread()
        IOUtil.LibCommand("AutoOffsetCalc")

    End Sub

    '20170110 baluu add start
    '20170224 baluu add start
    Public Sub OpenTgReconstruct()
        If reconstructfrm Is Nothing Then
            IOUtil.EndThread()
            IOUtil.LibCommand("reconstruct")
        Else
            reconstructfrm.Activate()

            Debug.Print("isnothing")
        End If
    End Sub
    '20170224 baluu add end


    Public Sub OpenReconstruct()
        Dim reconsProcess = Process.Start("Reconstruction.exe", objFBM.ProjectPath)
        Dim timeout As Integer = 6000000 '60 hour

        If Not reconsProcess.WaitForExit(timeout) Then
            'Waited too long
        Else
            Dim dbClass = New CDBOperate()
            If dbClass.ConnectDB("TGLess.mdb") Then
                Try
                    Dim recordSet = dbClass.CreateRecordset("SELECT TOP 1 * FROM 設定値及び結果 WHERE KoujiPath='" & objFBM.ProjectPath & "' ORDER BY ID DESC")
                    If recordSet.RecordCount = 1 Then
                        Dim outputPath As String = recordSet(25).Value
                        If Not IsDBNull(outputPath) Then
                            View_Dilaog.Model3dPath = outputPath
                            View_Dilaog.ReloadModel3dPath = True
                        End If
                    End If
                Finally
                    dbClass.DisConnectDB()
                End Try
            End If
        End If
    End Sub
    '20170110 baluu add end


    Private Sub ImageListView_MouseClick(sender As Object, e As MouseEventArgs) Handles ImageListView.MouseDown
        flgImageSelectDragAndDrop = True

        '20160601 Kiryu コメントアウト 4画面右クリメニュー無効化 Sta
        '        If e.Button = Windows.Forms.MouseButtons.Right Then

        '            ContextMenuStrip1 = New ContextMenuStrip

        '            Dim menuItem1 As String = "ウインドウ１ヘ"
        '            Dim menuItem2 As String = "ウインドウ２ヘ"
        '            Dim menuItem3 As String = "ウインドウ３ヘ"
        '            Dim menuItem4 As String = "ウインドウ４ヘ"

        '            ContextMenuStrip1.Items.Add(menuItem1)
        '            ContextMenuStrip1.Items.Add(menuItem2)
        '            ContextMenuStrip1.Items.Add(menuItem3)
        '            ContextMenuStrip1.Items.Add(menuItem4)

        '            ContextMenuStrip1.Show(Cursor.Position)
        '        Else
        '            Dim i As Integer = ImageListView.SelectedIndices.Item(0)

        '#If HALCON = 11 Then
        '            winpreview = AxHWindowXCtrl6.HalconID
        '#End If
        '            If flgPreview = True Then
        '                DispOneObjByIndex(winpreview, i)
        '            Else
        '                '(20140603 Tezuka Changed) 選択項目が0の時は処理しない
        '                If ImageListView.SelectedItems.Count > 0 Then
        '                    DispObjByIndex(i)
        '                End If
        '            End If
        '        End If
        '20160601 Kiryu コメントアウト 4画面右クリメニュー無効化 End

    End Sub

    Private Sub ContextMenuStrip1_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening
        If Me.ImageListView.SelectedItems.Count < 1 Then
            e.Cancel = True
        End If
    End Sub
    Private Sub ContextMenuStrip1_ItemClicked(sender As Object, e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles ContextMenuStrip1.ItemClicked

        If flgPreview = False Then

            If ImageListView.SelectedItems.Count > 0 Then
                'DispObjByIndex(i)
                Select Case e.ClickedItem.ToString()
                    Case ContextMenuStrip1.Items(0).ToString
                        'MsgBox("select 1")
                        '   DispOneObjByIndex(win1, ImageListView.SelectedIndices.Item(0))
                        dispWin(1) = ImageListView.SelectedIndices.Item(0)
                        DispObjByIndex(-1)
                    Case ContextMenuStrip1.Items(1).ToString
                        'MsgBox("select 2")
                        '  DispOneObjByIndex(win2, ImageListView.SelectedIndices.Item(0))
                        dispWin(2) = ImageListView.SelectedIndices.Item(0)
                        DispObjByIndex(-1)
                    Case ContextMenuStrip1.Items(2).ToString
                        'MsgBox("select 3")
                        ' DispOneObjByIndex(win3, ImageListView.SelectedIndices.Item(0))
                        dispWin(3) = ImageListView.SelectedIndices.Item(0)
                        DispObjByIndex(-1)
                    Case ContextMenuStrip1.Items(3).ToString
                        'MsgBox("select 4")
                        '  DispOneObjByIndex(win4, ImageListView.SelectedIndices.Item(0))
                        dispWin(4) = ImageListView.SelectedIndices.Item(0)
                        DispObjByIndex(-1)
                End Select
            End If
        End If

    End Sub

    Private Sub CalcAllEpiLinebyOnePoint(ByVal tmpManST As FBMlib.SingleTarget)
        Dim ISI1 As FBMlib.ImageSet = objFBM.lstImages(tmpManST.ImageID - 1)
        '   Dim ISI2 As FBMlib.ImageSet = Nothing
        Dim Zeros As Object
        Zeros = BTuple.TupleGenConst(5, 0)
        HOperatorSet.ChangeRadialDistortionCamPar("fixed", objFBM.hv_CamparamOut, Zeros, objFBM.hv_CamparamZero)
        For Each ISI2 As FBMlib.ImageSet In objFBM.lstImages
            HOperatorSet.GenEmptyObj(ISI2.hx_EpiLineAsOtherPoint)
            If ISI2.ImageId = ISI1.ImageId Then
            Else
                Dim RelPose As HTuple = Nothing
                Dim tmpImgPair As New FBMlib.ImagePairSet(ISI1, ISI2)
                tmpImgPair.calcRelPose()
                If tmpImgPair.cntComCT = -1 Then
                    Continue For
                End If
                Dim ERtrans As HTuple = Nothing
                Dim ECtrans As HTuple = Nothing
                HOperatorSet.ChangeRadialDistortionPoints(tmpManST.P2D.Row, tmpManST.P2D.Col, objFBM.hv_CamparamOut, objFBM.hv_CamparamZero, ERtrans, ECtrans)

                Dim objEpiLine As HObject = Nothing

                objFBM.gen_epiline(objEpiLine, tmpImgPair.PairPose.RelPose, New HTuple, objFBM.hv_CamparamZero, objFBM.hv_Width, ERtrans, ECtrans)
                Try
                    ISI2.hx_EpiLineAsOtherPoint = objEpiLine
                    '  HOperatorSet.ChangeRadialDistortionContoursXld(objEpiLine, ISI2.hx_EpiLineAsOtherPoint, objFBM.hv_CamparamZero, objFBM.hv_CamparamOut)

                Catch ex As Exception
                    MsgBox(ex.Message)
                    Exit For
                End Try

            End If
        Next
        'If icnt = 2 Then

        '    Dim RelPose As Object = DBNull.Value
        '    Dim tmpImgPair As New FBMlib.ImagePairSet(ISI1, ISI2)
        '    'tmpImgPair.Camparam = objFBM.hv_CamparamOut '20150115 SUURI DEL
        '    'wrtLine = "icnt00=" & icnt
        '    'System.IO.File.WriteAllText(FilePath, wrtLine, enc)

        '    tmpImgPair.calcRelPose()
        '    'wrtLine = "icnt11=" & icnt
        '    'System.IO.File.WriteAllText(FilePath, wrtLine, enc)

        '    FBMlib.CalcRelPoseBetweenTwoPose(ISI1.ImagePose.Pose, ISI2.ImagePose.Pose, RelPose)
        '    'wrtLine = "icnt22=" & icnt
        '    'System.IO.File.WriteAllText(FilePath, wrtLine, enc)

        '    Dim ER As Object = DBNull.Value
        '    Dim EC As Object = DBNull.Value
        '    Dim ERtrans As Object = DBNull.Value
        '    Dim ECtrans As Object = DBNull.Value
        '    HOperatorSet.GetContourXld(ISI1.hx_EdgePS, ER, EC)
        '    'wrtLine = "icn33t=" & icnt
        '    'System.IO.File.WriteAllText(FilePath, wrtLine, enc)

        '    Dim i As Integer = 0
        '    Dim maxTID As Integer = objFBM.GetMaxTID - 10000
        '    Dim maxISI1_STid As Integer = ISI1.GetST_MaxID
        '    Dim maxISI2_STid As Integer = ISI2.GetST_MaxID
        '    Dim Zeros As Object
        '    Zeros = BTuple.TupleGenConst(5, 0)
        '    HOperatorSet.ChangeRadialDistortionCamPar("fixed", objFBM.hv_CamparamOut, Zeros, objFBM.hv_CamparamZero)
        '    HOperatorSet.ChangeRadialDistortionPoints(ER, EC, objFBM.hv_CamparamOut, objFBM.hv_CamparamZero, ERtrans, ECtrans)
        '    HOperatorSet.ChangeRadialDistortionContoursXld(ISI1.hx_EdgePS, ISI1.hx_EdgePS, objFBM.hv_CamparamOut, objFBM.hv_CamparamZero)
        '    HOperatorSet.ChangeRadialDistortionContoursXld(ISI2.hx_EdgePS, ISI2.hx_EdgePS, objFBM.hv_CamparamOut, objFBM.hv_CamparamZero)

        '    'wrtLine = "icnt=" & icnt & "::UBound(ER)=" & UBound(ER)
        '    'System.IO.File.WriteAllText(FilePath, wrtLine, enc)

        '    For i = 0 To UBound(ER) - 1
        '        Dim R As Double = ERtrans(i)
        '        Dim C As Double = ECtrans(i)
        '        Dim objEpiLine As HObject = Nothing

        '        objFBM.gen_epiline(objEpiLine, tmpImgPair.PairPose.RelPose, Nothing, objFBM.hv_CamparamOut, objFBM.hv_Width, R, C)
        '        Dim InterR As Object = Nothing
        '        Dim InterC As Object = Nothing
        '        Dim IsOverLap As Object = Nothing
        '        HOperatorSet.IntersectionContoursXld(ISI2.hx_EdgePS, objEpiLine, "all", InterR, InterC, IsOverLap)

        '        If IsOverLap <> 0 Or BTuple.TupleLength(InterR) <> 1 Then
        '            Continue For
        '        End If
        '        HOperatorSet.ChangeRadialDistortionPoints(InterR, InterC, objFBM.hv_CamparamZero, objFBM.hv_CamparamOut, InterR, InterC)
        '        maxISI1_STid += 1
        '        maxISI2_STid += 1
        '        maxTID += 1
        '        Dim tmpcomST As New FBMlib.Common3DSingleTarget
        '        Dim tmpST1 As New FBMlib.SingleTarget(ISI1.ImageId, maxISI1_STid, ER(i), EC(i))
        '        Dim tmpST2 As New FBMlib.SingleTarget(ISI2.ImageId, maxISI2_STid, InterR, InterC)
        '        ISI1.Targets.lstST.Add(tmpST1)
        '        ISI2.Targets.lstST.Add(tmpST2)
        '        ISI1.CalcRay(objFBM.hv_CamparamOut)
        '        ISI2.CalcRay(objFBM.hv_CamparamOut)
        '        tmpcomST.lstST.Add(tmpST1)
        '        tmpcomST.lstST.Add(tmpST2)
        '        tmpcomST.PID = maxTID
        '        tmpcomST.Calc3dPoints()
        '        objFBM.lstCommon3dST.Add(tmpcomST)


        '    Next
        '    objFBM.SaveToMeasureDataDBNewPoint(objFBM.ProjectPath & "\")
        '    YMC_3DViewReDraw_NoKousin(m_strDataBasePath)

        '    For Each ISI As FBMlib.ImageSet In objFBM.lstImages
        '        If ISI.hx_EdgePS IsNot Nothing Then
        '            ISI.hx_EdgePS = Nothing
        '        End If
        '    Next

        'End If
    End Sub

    '    Private Sub ImageListView_MouseUp(sender As Object, e As MouseEventArgs) Handles ImageListView.MouseUp
    '        If e.Button = Windows.Forms.MouseButtons.Right Then

    '            ContextMenuStrip1 = New ContextMenuStrip

    '            Dim menuItem1 As String = "ウインドウ１ヘ"
    '            Dim menuItem2 As String = "ウインドウ２ヘ"
    '            Dim menuItem3 As String = "ウインドウ３ヘ"
    '            Dim menuItem4 As String = "ウインドウ４ヘ"

    '            ContextMenuStrip1.Items.Add(menuItem1)
    '            ContextMenuStrip1.Items.Add(menuItem2)
    '            ContextMenuStrip1.Items.Add(menuItem3)
    '            ContextMenuStrip1.Items.Add(menuItem4)

    '            ContextMenuStrip1.Show(Cursor.Position)
    '        Else
    '            If ImageListView.SelectedIndices.Count = 0 Then
    '                Exit Sub
    '            End If
    '            Dim i As Integer = ImageListView.SelectedIndices.Item(0)

    '#If HALCON = 11 Then
    '            winpreview = AxHWindowXCtrl6.HalconID
    '#End If
    '            If flgPreview = True Then
    '                DispOneObjByIndex(winpreview, i)
    '            Else
    '                '(20140603 Tezuka Changed) 選択項目が0の時は処理しない
    '                'If ImageListView.SelectedItems.Count > 0 Then
    '                '    DispObjByIndex(i)
    '                'End If
    '            End If
    '            'End If
    '    End Sub



    Public Sub CallCreateTargetPoint3d()
        IOUtil.EndThread()
        IOUtil.LibCommand("CreateTargetPoint3d")
    End Sub

    '20170209 baluu del start
    'Private Sub AxHWindowXCtrl2_MouseMoveEvent(sender As Object, e As AxHALCONXLib._HWindowXCtrlEvents_MouseMoveEvent) Handles AxHWindowXCtrl2.MouseMoveEvent

    'End Sub
    '20170209 baluu del end

    Private Sub AxHWindowXCtrl2_MouseUpEvent(sender As Object, e As HalconDotNet.HMouseEventArgs) Handles AxHWindowXCtrl2.HMouseUp

        If flgImageSelectDragAndDrop = False Then
            Exit Sub
        Else
            flgImageSelectDragAndDrop = False
        End If
        If ImageListView.SelectedIndices.Count = 0 Then
            Exit Sub
        End If
        Dim i As Integer = ImageListView.SelectedIndices.Item(0)

#If HALCON = 11 Then
        winpreview = AxHWindowXCtrl6.HalconWindow
#End If
        If flgPreview = True Then
            DispOneObjByIndex(winpreview, i)
        Else
            '(20140603 Tezuka Changed) 選択項目が0の時は処理しない
            If ImageListView.SelectedItems.Count > 0 Then
                dispWin(1) = i
                DispObjByIndex(-1)
            End If
        End If
    End Sub

    Private Sub AxHWindowXCtrl3_MouseUpEvent(sender As Object, e As HalconDotNet.HMouseEventArgs) Handles AxHWindowXCtrl3.HMouseUp
        If flgImageSelectDragAndDrop = False Then
            Exit Sub
        Else
            flgImageSelectDragAndDrop = False
        End If
        If ImageListView.SelectedIndices.Count = 0 Then
            Exit Sub
        End If
        Dim i As Integer = ImageListView.SelectedIndices.Item(0)

#If HALCON = 11 Then
        winpreview = AxHWindowXCtrl6.HalconWindow
#End If
        If flgPreview = True Then
            DispOneObjByIndex(winpreview, i)
        Else
            '(20140603 Tezuka Changed) 選択項目が0の時は処理しない
            If ImageListView.SelectedItems.Count > 0 Then
                dispWin(2) = i
                DispObjByIndex(-1)
            End If
        End If
    End Sub

    Private Sub AxHWindowXCtrl5_MouseUpEvent(sender As Object, e As HalconDotNet.HMouseEventArgs) Handles AxHWindowXCtrl5.HMouseUp
        If flgImageSelectDragAndDrop = False Then
            Exit Sub
        Else
            flgImageSelectDragAndDrop = False
        End If
        If ImageListView.SelectedIndices.Count = 0 Then
            Exit Sub
        End If
        Dim i As Integer = ImageListView.SelectedIndices.Item(0)

#If HALCON = 11 Then
        winpreview = AxHWindowXCtrl6.HalconWindow
#End If
        If flgPreview = True Then
            DispOneObjByIndex(winpreview, i)
        Else
            '(20140603 Tezuka Changed) 選択項目が0の時は処理しない
            If ImageListView.SelectedItems.Count > 0 Then
                dispWin(4) = i
                DispObjByIndex(-1)
            End If
        End If
    End Sub

    Private Sub AxHWindowXCtrl4_MouseUpEvent(sender As Object, e As HalconDotNet.HMouseEventArgs) Handles AxHWindowXCtrl4.HMouseUp
        If flgImageSelectDragAndDrop = False Then
            Exit Sub
        Else
            flgImageSelectDragAndDrop = False
        End If
        If ImageListView.SelectedIndices.Count = 0 Then
            Exit Sub
        End If
        Dim i As Integer = ImageListView.SelectedIndices.Item(0)

#If HALCON = 11 Then
        winpreview = AxHWindowXCtrl6.HalconWindow
#End If
        If flgPreview = True Then
            DispOneObjByIndex(winpreview, i)
        Else
            '(20140603 Tezuka Changed) 選択項目が0の時は処理しない
            If ImageListView.SelectedItems.Count > 0 Then
                dispWin(3) = i
                DispObjByIndex(-1)
            End If
        End If
    End Sub

    Dim flgImageSelectDragAndDrop As Boolean = False
    'add by SUSANO 2016-03-24
    Private Sub DrawRect(ByVal objwin As HWindow, ByVal disp As Integer)
        Try

            Dim R As Object = Nothing, C As Object = Nothing
            Dim mBtn As Object = Nothing
            HOperatorSet.GetMposition(objwin, R, C, mBtn)
            Dim ii As Integer = Integer.Parse(mBtn.ToString)
            iBtn = ii
            iWin = disp
            Dim i As Integer = dispWin(disp)
            Dim ISI As FBMlib.ImageSet
            ISI = objFBM.lstImages(i)
            Dim objDist As Object = Nothing
            Dim minVal As Double = Double.MaxValue
            Dim tmpSt As FBMlib.SingleTarget = Nothing
            If flgManualST = True Then
                If mBtn.I = 1 Then
                    For Each ST As FBMlib.SingleTarget In ISI.Targets.lstST
                        ST.P2D.CalcDistToInputPoint(R, C, objDist)
                        If objDist < minVal Then
                            minVal = objDist
                            tmpSt = ST
                        End If
                    Next
                    ISI.CalcRay(objFBM.hv_CamparamOut)
                    tmpManSt = tmpSt
                End If
            End If
            If flgClickPoint = True Then
                If mBtn.I = 1 Then
                    Dim tmpP2ID As Integer = ISI.Targets.GetST_MaxID

                    tmpSt = New FBMlib.SingleTarget(ISI.ImageId, tmpP2ID + 1, R, C)
                    ISI.Targets.lstST.Add(tmpSt)
                    ISI.CalcRay(objFBM.hv_CamparamOut)
                    tmpManSt = tmpSt
                End If
            End If
            If flgEdgeST = True Then
                If mBtn.I = 1 And tmpManSt Is Nothing Then
                    Dim tmpP2ID As Integer = ISI.Targets.GetST_MaxID
                    Dim tmpWH As Integer = 50
                    tmpSt = New FBMlib.SingleTarget(ISI.ImageId, tmpP2ID + 1, R, C)
                    tmpManSt = tmpSt
                    System.Threading.Thread.Sleep(100)
                    'HOperatorSet.DrawRectangle1Mod(objwin, R - tmpWH, R - tmpWH, R + tmpWH, R + tmpWH,
                    '                     tmpManSt.P2D.Row, tmpManSt.P2D.Col, tmpManSt.TP2D.Row, tmpManSt.TP2D.Col)

                    HOperatorSet.DrawRectangle1(objwin, tmpManSt.P2D.Row, tmpManSt.P2D.Col, tmpManSt.TP2D.Row, tmpManSt.TP2D.Col)
                End If

            End If

            If flgAllEdgeST = True Then

                flgAllEdgeST = False
            End If

            If flgSelectC3D = True Then
                tmpSt = ISI.GetClickedTarget(R, C, minVal)

                If minVal / (objFBM.hv_Width / AxHWindowXCtrl2.Width) < 5 Then
                    tmpManSt = tmpSt
                    Dim ttt As New CLookPoint
                    If GetPosFromLabelName(CLookPoint.posTypeMode.All, tmpSt.currentLabel, ttt) <> -1 Then
                        IOUtil.blnPickPoint = True
                        IOUtil.pick_P = ttt
                        bln_ClickPoint = True
                    End If
                End If

            End If


        Catch ex As Exception
            Dim tt As Integer = 1
            MsgBox("画面1クリックイベントエラー")
            flgEdgeST = False
            flgAllEdgeST = False
        End Try
    End Sub

    Private Function AfterPointWords(mystr As String) As String
        Dim cut_at As String = "."
        Dim x As Integer = InStr(mystr, cut_at)
        Dim string_after As String = mystr.Substring(x + cut_at.Length - 1)
        Return string_after
    End Function




    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub


    'Add Kiryu 20161003 ２次元画像のショートカットキー定義
    Private Sub AxHWindowXCtrl6_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles AxHWindowXCtrl6.PreviewKeyDown
        Static hv_Pose As New HTuple
        Select Case e.KeyCode
            Case Keys.Up
                Call MainFrm.BtnAnaBright()
                System.Windows.Forms.Application.DoEvents()
            Case Keys.Down
                Call MainFrm.BtnAnaDark()
                System.Windows.Forms.Application.DoEvents()
            Case Keys.P
                Dim i As Integer = ImageListView.SelectedIndices.Item(0)
                GenPlaneBySelectedObject3d(winpreview, i, 0, hv_HalconObjectModel3d)
            Case Keys.C
                Dim i As Integer = ImageListView.SelectedIndices.Item(0)
                GenPlaneBySelectedObject3d(winpreview, i, 1, hv_HalconObjectModel3d)
            Case Keys.D
                Dim i As Integer = ImageListView.SelectedIndices.Item(0)

                DeleteSelectedObject3d(i, hv_HalconObjectModel3d, hv_Pose)
            Case Keys.Y
                Try
                    HOperatorSet.ClearObjectModel3d(hv_HalconObjectModel3d)
                Catch ex As Exception

                End Try
                HOperatorSet.ReadObjectModel3d(MainFrm.objFBM.ProjectPath & "\Pdata\ReconstResultOm3.om3", New HTuple(1), New HTuple, New HTuple, hv_HalconObjectModel3d, New HTuple)
            Case Keys.W
                HOperatorSet.UnionObjectModel3d(hv_HalconObjectModel3d, "points_surface", hv_PlaneObject3d)
                HOperatorSet.WriteObjectModel3d(hv_PlaneObject3d, "dxf", objFBM.ProjectPath & "\Pdata\Result.dxf", New HTuple, New HTuple)
                HOperatorSet.ClearObjectModel3d(hv_PlaneObject3d)
                MsgBox("DXF出力完了しました。")
        End Select
    End Sub

    ' Main procedure 
    Private Sub DeleteSelectedObject3d(ByVal ImageIndex As Integer, ByRef hv_Object3d As HTuple, ByRef hv_Pose As HTuple)
        Dim winHand As HTuple

        'Dim hv_PoseIn As HTuple = Nothing, HM3D As HTuple = Nothing, HM3DI As HTuple = Nothing
        'HOperatorSet.TupleMult(objFBM.lstImages(ImageIndex).ImagePose.Pose, New HTuple({MainFrm.objFBM.pScaleMM, MainFrm.objFBM.pScaleMM, MainFrm.objFBM.pScaleMM, 1.0, 1.0, 1.0, 1.0}),
        '                                                                   objFBM.lstImages(ImageIndex).ImagePose.ReConstWorldPose)
        'HOperatorSet.PoseToHomMat3d(objFBM.lstImages.Item(ImageIndex).ImagePose.ReConstWorldPose, HM3D)
        'HOperatorSet.HomMat3dInvert(HM3D, HM3DI)
        'HOperatorSet.HomMat3dToPose(HM3DI, hv_PoseIn)




        ' Local iconic variables 
        Dim ho_Ramp As HObject = Nothing, ho_Region As HObject = Nothing
        Dim ho_Rectangle As HObject = Nothing, ho_RegionComplement As HObject = Nothing


        ' Local control variables 

        Dim hv_Status As HTuple = New HTuple
        Dim hv_CamParam As HTuple = New HTuple ', hv_Pose As HTuple = New HTuple
        Dim hv_GenParamName As HTuple = New HTuple, hv_GenParamValue As HTuple = New HTuple
        Dim hv_Title As HTuple = New HTuple, hv_Instructions As HTuple = New HTuple
        Dim hv_Message As HTuple = New HTuple, hv_ObjectModel3DReduced As HTuple = New HTuple

        ' Initialize local and output iconic variables 
        HOperatorSet.GenEmptyObj(ho_Ramp)
        HOperatorSet.GenEmptyObj(ho_Region)
        HOperatorSet.GenEmptyObj(ho_Rectangle)
        HOperatorSet.GenEmptyObj(ho_RegionComplement)

        '********************************************************
        'This example program shows how to remove parts of a
        '3D object model using reduce_object_model_3d_by_view.
        'In particular, the user can rotate and translate the
        'object and then draw a region that cuts out all points
        'of the model that are projected into this region.
        'All faces that base on one of these points are removed.
        '********************************************************
        '
        'If (HDevWindowStack.IsOpen()) Then
        '    HOperatorSet.CloseWindow(HDevWindowStack.Pop())
        'End If
        'Prepare the Visualization
        dev_update_off()
        'If (HDevWindowStack.IsOpen()) Then
        '    HOperatorSet.CloseWindow(HDevWindowStack.Pop())
        'End If
        HOperatorSet.SetWindowAttr("background_color", New HTuple("black"))
        HOperatorSet.OpenWindow(New HTuple(0), New HTuple(0), New HTuple(768), New HTuple(512), 0, "", "", winHand)
        HDevWindowStack.Push(winHand)
        If (HDevWindowStack.IsOpen()) Then
            HOperatorSet.SetDraw(HDevWindowStack.GetActive(), New HTuple("margin"))
        End If
        set_display_font(winHand, New HTuple(16), New HTuple("mono"), New HTuple("true"), _
            New HTuple("false"))
        '
        'HOperatorSet.SetPart(winHand, New HTuple(0), New HTuple(0),
        '                        hv_CamParam.TupleSelect(New HTuple(11)),
        '                      hv_CamParam.TupleSelect(New HTuple(10)))
        '
        HOperatorSet.SetPart(winHand, 0, 0, 512, 768)
        hv_CamParam = (((((((New HTuple(0.01)).TupleConcat(0)).TupleConcat(0.000007)).TupleConcat( _
            0.000007)).TupleConcat(384)).TupleConcat(255)).TupleConcat(768)).TupleConcat( _
            512)

        '
        hv_GenParamName = New HTuple '("alpha", "colored")
        hv_GenParamValue = New HTuple '(0.3, 12)

        '
        'Show current 3D object model
        hv_Title = New HTuple("Move and rotate the object to an appropriate pose, before")
        If IsNothing(hv_Title) Then
            hv_Title = New HTuple
        End If
        hv_Title(New HTuple(1)) = New HTuple("selecting the region of the points to be trimmed off.")
        If IsNothing(hv_Instructions) Then
            hv_Instructions = New HTuple
        End If
        hv_Instructions(New HTuple(0)) = New HTuple("Rotate: Left button")
        If IsNothing(hv_Instructions) Then
            hv_Instructions = New HTuple
        End If
        hv_Instructions(New HTuple(1)) = New HTuple("Zoom:   Shift + left button")
        If IsNothing(hv_Instructions) Then
            hv_Instructions = New HTuple
        End If
        hv_Instructions(New HTuple(2)) = New HTuple("Move:   Ctrl  + left button")
        If (HDevWindowStack.IsOpen()) Then
            HOperatorSet.DispObj(ho_Ramp, HDevWindowStack.GetActive())
        End If
        '   hv_CamParam = objFBM.hv_CamparamOut

        visualize_object_model_3d(winHand, hv_Object3d, _
            hv_CamParam, hv_Pose, New HTuple, New HTuple, hv_Title, New HTuple(), _
            hv_Instructions, hv_Pose)
        '
        'Now, select the points to be trimmed off by entering a
        'suitable region
        hv_Message = New HTuple("Now draw region with the mouse to cut out part of the model")
        If IsNothing(hv_Message) Then
            hv_Message = New HTuple
        End If
        hv_Message(New HTuple(1)) = New HTuple("(Right click to finish)")
        disp_message(winHand, hv_Message, New HTuple("window"), New HTuple(12), _
            New HTuple(12), New HTuple("black"), New HTuple("true"))
        'Wait for a region
        If (HDevWindowStack.IsOpen()) Then
            HOperatorSet.SetLineWidth(HDevWindowStack.GetActive(), New HTuple(3))
        End If
        If (HDevWindowStack.IsOpen()) Then
            HOperatorSet.SetColor(HDevWindowStack.GetActive(), New HTuple("red"))
        End If
        ho_Region.Dispose()
        HOperatorSet.DrawRegion(ho_Region, winHand)
        ho_Rectangle.Dispose()
        HOperatorSet.GenRectangle1(ho_Rectangle, New HTuple(0), New HTuple(0), hv_CamParam.TupleSelect( _
          New HTuple(7)), hv_CamParam.TupleSelect(New HTuple(6)))
        ho_RegionComplement.Dispose()
        HOperatorSet.Difference(ho_Rectangle, ho_Region, ho_RegionComplement)
        'Apply the region drawn before to the 3d object model
        HOperatorSet.ReduceObjectModel3dByView(ho_RegionComplement, hv_Object3d, hv_CamParam, _
            hv_Pose, hv_ObjectModel3DReduced)
        '
        'Visualize the result
        hv_Title = New HTuple("Resulting reduced 3d object model")
        HOperatorSet.ClearWindow(winHand)
        visualize_object_model_3d(winHand, hv_ObjectModel3DReduced, hv_CamParam, hv_Pose, hv_GenParamName, hv_GenParamValue, _
            hv_Title, New HTuple(), hv_Instructions, hv_Pose)
        HOperatorSet.ClearObjectModel3d(hv_Object3d)

        HOperatorSet.CopyObjectModel3d(hv_ObjectModel3DReduced, "all", hv_Object3d)
        HOperatorSet.ClearObjectModel3d(hv_ObjectModel3DReduced)
        HOperatorSet.CloseWindow(winHand)
        ho_Ramp.Dispose()
        ho_Region.Dispose()
        ho_Rectangle.Dispose()
        ho_RegionComplement.Dispose()

    End Sub


    Private Sub AxHWindowXCtrl6_MouseDown(sender As Object, e As MouseEventArgs) Handles AxHWindowXCtrl6.MouseDown

    End Sub

    Private Sub AxHWindowXCtrl6_KeyDown(sender As Object, e As KeyEventArgs) Handles AxHWindowXCtrl6.KeyDown

    End Sub
End Class