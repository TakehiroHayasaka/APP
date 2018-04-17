Imports System.IO
Imports System.Windows.Threading
Imports HalconDotNet

Public Class MMainGamen

    Dim cmds As String() = System.Environment.GetCommandLineArgs()
    Public m_system_dbclass As FBMlib.CDBOperateOLE             'システム設定.mdb
    Public m_keisoku_dbclass As FBMlib.CDBOperateOLE            '計測データ.mdb

    Public TypeInfoDL As List(Of TypeInfoTable)                 'TypeIDテーブルリスト
    'add by tsene 2016/05/12 start
    Dim dpTimer As DispatcherTimer
    'add by tsene 2016/05/12 end
    Dim frmkihon As KentouKihonInfoTabDialog
    Dim m_NiniListSelectFlg As Integer = 0
    Dim m_SenyouListSelectFlg As Integer = 0
    Dim strpath As String = ""
    Dim type_id As Integer
    Dim flgInstantRun As Boolean = False

    ' 任意計測
    Private m_ListNiniItems As New System.Collections.ObjectModel.ObservableCollection(Of MyListViewItem)
    ' 専用計測
    Private m_ListSenyouItems As New System.Collections.ObjectModel.ObservableCollection(Of MyListViewItem)
    Class MyListViewItem
        Public Property Img As System.Windows.Media.Imaging.BitmapImage
        Public Property ImgSeleted As System.Windows.Media.Imaging.BitmapImage
        Public Property Text As String
        Public Property ID As Integer

        Public Sub New()
        End Sub
        Public Sub New(ByVal contentText As String, ByVal pathOfImg As String, ByVal pathOfImgSeleted As String)
            Text = contentText
            Img = New System.Windows.Media.Imaging.BitmapImage(
                New System.Uri(pathOfImg,
                    System.UriKind.RelativeOrAbsolute))
            ImgSeleted = New System.Windows.Media.Imaging.BitmapImage(
                New System.Uri(pathOfImgSeleted,
                    System.UriKind.RelativeOrAbsolute))
        End Sub
    End Class

    Private m_KojiDataConfUserControl As KojiDataConfUserControl

    '新規ボタン押下

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ButtonNew.Click

        Dim iSts As Integer = 0

        '選択行の取得

        iSts = SelectGet()
        If iSts < 0 Then
            Exit Sub
        End If

        ' ①初期画面から「新規」あるいは「開く」を押すと初期画面が閉じる（非表示にする）

        'Me.Visibility = System.Windows.Visibility.Hidden
        Try

            frmkihon = New KentouKihonInfoTabDialog
            frmkihon.WindowState = Windows.WindowState.Maximized
            frmkihon.IsNew = True
            Me.WindowState = Windows.WindowState.Minimized
            'frmkihon.ShowDialog()
            frmkihon.Show()
            AddHandler frmkihon.Closed, AddressOf frmkihon_closed
            Me.IsEnabled = False

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        ' ①初期画面から「新規」あるいは「開く」を押すと初期画面が閉じる（非表示にする）

        'Me.Close()
    End Sub

    Private Sub frmkihon_closed(sender As Object, e As System.EventArgs)
        Me.IsEnabled = True
        'Me.Show()
        Me.WindowState = Windows.WindowState.Maximized

    End Sub

    '開くボタン押下

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOpen.Click
        m_KojiDataConfUserControl.openKentouKihonInfoTabDialog()

    End Sub

    '計測
    Private Sub ChangeKeisoku()
        openKojiDataConf()

        'select
        If m_KojiDataConfUserControl.DataTable1.Rows.Count > 0 Then
            m_KojiDataConfUserControl.DataGridView1.SelectedIndex = 0
            Me.ButtonOpen.IsEnabled = True
        Else
            Me.ButtonOpen.IsEnabled = False
        End If
    End Sub

    Private Sub openKojiDataConf()
        Dim iSts As Integer = 0

        '選択行の取得

        iSts = SelectGet()
        If iSts < 0 Then
            Exit Sub
        End If

        'frmkihon = New KentouKihonInfoTabDialog
        'frmkihon.Tag = 2
        'frmkihon.ShowDialog()
        'Return

        ' add m_KojiDataConfUserControl to the grid Grid_KojiDataConf
        If m_KojiDataConfUserControl Is Nothing Then
            m_KojiDataConfUserControl = New KojiDataConfUserControl
            m_KojiDataConfUserControl.parentWindow = Me

            m_KojiDataConfUserControl.Margin = New System.Windows.Thickness(0, 0, 0, 0)
            m_KojiDataConfUserControl.HorizontalAlignment = System.Windows.HorizontalAlignment.Left
            m_KojiDataConfUserControl.VerticalAlignment = System.Windows.VerticalAlignment.Top
            'm_KojiDataConfUserControl.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center
            'm_KojiDataConfUserControl.VerticalContentAlignment = System.Windows.VerticalAlignment.Center
            m_KojiDataConfUserControl.Width = Grid_KojiDataConf.ActualWidth
            m_KojiDataConfUserControl.Height = Grid_KojiDataConf.ActualHeight

            Grid_KojiDataConf.Children.Add(m_KojiDataConfUserControl)
        End If
        ' set global variable CommonTypeID
        If True Then
            Dim system_dbclass As FBMlib.CDBOperateOLE
            If ConnectSystemDB(system_dbclass) = False Then
                MsgBox(err_msg_systemdb_connect, MsgBoxStyle.OkOnly)
                Exit Sub
            End If

            Dim TypeInfoD As New TypeInfoTable
            TypeInfoD.m_dbClass = system_dbclass
            TypeInfoDL = TypeInfoD.GetDataToList
            If TypeInfoDL Is Nothing Then
                Exit Sub
            End If

            Try
                If m_flg_Senyou = True Then
                    CommonTypeID = TypeInfoDL(m_SenyouList_No).ID
                Else
                    CommonTypeID = m_Senyou_Id
                End If
            Catch e As Exception
                Dim sss As String
                sss = e.InnerException.Message.ToString
            End Try

            'システム設定mdbを切断する
            DisConnectDB(system_dbclass)
        End If
        ' set global variable WorksD and load data of control KojiDataConfUserControl

        loadKojiDataConfUserControl()
    End Sub
    ' set global variable WorksD and load data of control KojiDataConfUserControl
    Private Sub loadKojiDataConfUserControl()

        Dim sw As New System.Diagnostics.Stopwatch
        sw.Start()

        Dim system_dbclass As FBMlib.CDBOperateOLE
        Dim keisoku_dbclass As FBMlib.CDBOperateOLE
        If ConnectSystemDB(system_dbclass) = False Then
            MsgBox(err_msg_systemdb_connect, MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        'WorksTableデータを取得する

        'システム設定mdbから
        WorksD = New WorksTable
        WorksD.m_dbClass = system_dbclass
        If WorksD.GetDataToList() = False Then
            Exit Sub
        End If

        ''他工事データからの参照ボタンの制御
        'SetKoujiSansyou()

        '基本情報画面の設定()
        'SetTLP4()

        'システム設定mdbに切断する
        DisConnectDB(system_dbclass)

        With m_KojiDataConfUserControl
            .m_form = "基本情報"
            .m_SystemMdbPath = m_SystemMdbPath
            .m_SystemMdbFullPath = m_SystemMdbFullPath
            .m_keisoku_dbclass = keisoku_dbclass          '計測データ.mdb
            .m_system_dbclass = system_dbclass            'システム設定.mdb
            If .WorksD Is Nothing Then
                .WorksD = New WorksTable
            End If
            .WorksD.copy(WorksD)
            sw.Stop()
            Trace.WriteLine(sw.Elapsed.TotalSeconds & "秒かかりました")

            '.ShowDialog()
            m_KojiDataConfUserControl.Load()

            'If .Tag Is Nothing Then
            '    Me.Close()
            '    Exit Sub
            'Else
            '    Me.Tag = .Tag
            '    Dim WorksD1 As New WorksTable
            '    'SetYomikomiData(WorksD1)
            '    WorksD = New WorksTable
            '    WorksD.copy(WorksD1)
            '    Dim m_Keisoku_mdb_path = WorksD1.SavePath & "\" & m_Keisoku_mdb_name
            '    m_koji_kanri_path = WorksD1.SavePath
            'End If
        End With
    End Sub

    Private Sub MMainGamen_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        'Dim ss = m_KojiDataConfUserControl.WorksL.Item(0).SavePath

        'Dim indx As New Integer
        'For Each w_tb As WorksTable In m_KojiDataConfUserControl.WorksL
        '    If w_tb.SavePath = strpath Then
        '        m_KojiDataConfUserControl.DataGridView1.SelectedIndex = indx

        '    End If
        '    indx += 1
        'Next


        'If cmds.Count > 1 Then
        '    ListSenyou.SelectedIndex = 1
        '    Dim iSts As Integer = 0

        '    '選択行の取得
        '    m_flg_Senyou = True
        '    iSts = SelectGet()
        '    If iSts < 0 Then
        '        Exit Sub
        '    End If

        '    ' ①初期画面から「新規」あるいは「開く」を押すと初期画面が閉じる（非表示にする）

        '    'Me.Visibility = System.Windows.Visibility.Hidden
        '    m_koji_kanri_path = cmds(1)
        '    frmkihon = New KentouKihonInfoTabDialog
        '    frmkihon.WindowState = Windows.WindowState.Maximized
        '    frmkihon.IsNew = True

        '    frmkihon.Show()



        '    frmkihon.KaisekiRun()


        '    SueokiSyori(cmds)
        '    Exit Sub
        'End If

        'If flgInstantRun = True Then
        '    flgInstantRun = False
        '    'MsgBox("activate")
        '    m_KojiDataConfUserControl.openKentouKihonInfoTabDialog()
        'End If

    End Sub

    'フォームロード
    Private Sub MMainGamen_Load(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        'add by tsene 2016/4/25 start

        Me.Title = Me.Title & " BuildNo" & My.Settings.BuildNo 'Add Kiryu 20160613

        flgInstantRun = False
        If cmds.Count > 2 Then
            Me.Hide()
            ListSenyou.SelectedIndex = 1
            Dim iSts As Integer = 0
            flgSUEOKI = True


            ' ①初期画面から「新規」あるいは「開く」を押すと初期画面が閉じる（非表示にする）

            'Me.Visibility = System.Windows.Visibility.Hidden
            m_koji_kanri_path = cmds(1)
            CommonTypeID = 28
            frmkihon = New KentouKihonInfoTabDialog
            frmkihon.WindowState = Windows.WindowState.Maximized
            frmkihon.IsNew = True

            ' frmkihon.Show()
            '   CommonTypeID = 28
            frmkihon.NewSyoriSUEOKI()
            '  CommonTypeID = 28

            frmkihon.KaisekiRunSUE()


            SueokiSyori(cmds)
            Me.Close()
            Exit Sub

        ElseIf cmds.Count = 2 Then
            'end shineer nemeh
            'システム設定mdbに接続する

            If ConnectSystemDB(m_system_dbclass) = False Then
                MsgBox(err_msg_systemdb_connect, MsgBoxStyle.OkOnly)
                Exit Sub
            End If

            '任意計測リストの設定

            SetListNini()

            '専用計測リストの設定
            SetListSenyou()

            If IsVFormFile() = True Then
                SelectListSenyou()
            Else
                Exit Sub
            End If

            'システム設定mdbを切断する
            DisConnectDB(m_system_dbclass)

            ' SetHajimeni()
            ListNini.SelectedIndex = 0

            'Rep By Yamada Sta 20140818-----------
#If Anchor = 1 Then
        Me.Title = Me.Title & " アンカーボルト計測専用 Ver 1.1"    '20140818 日本車輌リリース
#Else
            Me.Title = Me.Title & " BuildNo" & My.Settings.BuildNo
#End If

        ElseIf cmds.Count = 1 Then

            'システム設定mdbに接続する
            If ConnectSystemDB(m_system_dbclass) = False Then
                MsgBox(err_msg_systemdb_connect, MsgBoxStyle.OkOnly)
                Exit Sub
            End If

            '任意計測リストの設定

            SetListNini()

            '専用計測リストの設定

            SetListSenyou()

            'システム設定mdbを切断する
            DisConnectDB(m_system_dbclass)

            ' SetHajimeni()
            ListNini.SelectedIndex = 0
        End If

        'add by tsene 2016/4/25 end


        '        'Rep By Yamada End 20140818-----------
        '        If cmds.Count > 1 Then

        '            Me.Hide()
        '            ListSenyou.SelectedIndex = 1
        '            Dim iSts As Integer = 0
        '            flgSUEOKI = True
        '            ''選択行の取得
        '            'm_flg_Senyou = True
        '            'iSts = SelectGet()
        '            'If iSts < 0 Then
        '            '    Exit Sub
        '            'End If

        '            ' ①初期画面から「新規」あるいは「開く」を押すと初期画面が閉じる（非表示にする）

        '            'Me.Visibility = System.Windows.Visibility.Hidden
        '            m_koji_kanri_path = cmds(1)
        '            CommonTypeID = 28
        '            frmkihon = New KentouKihonInfoTabDialog
        '            frmkihon.WindowState = Windows.WindowState.Maximized
        '            frmkihon.IsNew = True

        '            ' frmkihon.Show()
        '            '   CommonTypeID = 28
        '            frmkihon.NewSyoriSUEOKI()
        '            '  CommonTypeID = 28

        '            frmkihon.KaisekiRunSUE()


        '            SueokiSyori(cmds)
        '            Me.Close()
        '            Exit Sub
        '        Else

        '            'システム設定mdbに接続する

        '            If ConnectSystemDB(m_system_dbclass) = False Then
        '                MsgBox(err_msg_systemdb_connect, MsgBoxStyle.OkOnly)
        '                Exit Sub
        '            End If

        '            '任意計測リストの設定

        '            SetListNini()

        '            '専用計測リストの設定

        '            SetListSenyou()

        '            'システム設定mdbを切断する
        '            DisConnectDB(m_system_dbclass)

        '            ' SetHajimeni()
        '            ListNini.SelectedIndex = 0

        '            'Rep By Yamada Sta 20140818-----------
        '#If Anchor = 1 Then
        '                Me.Title = Me.Title & " アンカーボルト計測専用 Ver 1.1"    '20140818 日本車輌リリース
        '#Else
        '            Me.Title = Me.Title & " 2014"
        '#End If
        '        End If

        'add by tsene 2016/05/12 start
        dpTimer = New DispatcherTimer
        'dpTimer.Interval = TimeSpan.FromMilliseconds(20)
        AddHandler dpTimer.Tick, AddressOf dispatcherTimer_Tick
        dpTimer.Start()
        'dpTimer.Stop()
        'add by tsene 2016/05/12 end
    End Sub

    '任意計測リストの設定

    Private Sub SetListNini()
        ListNini.ItemsSource = Nothing

        Dim item As MyListViewItem

        '(20140527 Tezuka ADD) 任意計測メニューの表示制御
        'add by SUSANO 
        Dim iiM_ZAHYOU As Integer = GetPrivateProfileInt("MENU", "M_ZAHYOU", 0, My.Application.Info.DirectoryPath & "\vform.ini")
        Dim iiM_SUNPO As Integer = GetPrivateProfileInt("MENU", "M_SUNPO", 0, My.Application.Info.DirectoryPath & "\vform.ini")
        Dim iiM_ZUKEI As Integer = GetPrivateProfileInt("MENU", "M_ZUKEI", 0, My.Application.Info.DirectoryPath & "\vform.ini")

        If iiM_ZAHYOU = 1 Then
            '#If M_ZAHYOU = "TRUE" Then
            item = New MyListViewItem("任意座標計測",
                                      "/YCM;component/Image2/MMainGamen/1_1.png",
                                      "/YCM;component/Image2/MMainGamen/1_2.png")
            item.ID = -1
            m_ListNiniItems.Add(item)
            '#End If
        End If

        If iiM_SUNPO = 1 Then
            '#If M_SUNPO = "TRUE" Then
            item = New MyListViewItem("任意寸法計測",
                                      "/YCM;component/Image2/MMainGamen/2_1.png",
                                      "/YCM;component/Image2/MMainGamen/2_2.png")
            item.ID = -2
            m_ListNiniItems.Add(item)
            '#End If
        End If

        If iiM_ZUKEI = 1 Then
            '#If M_ZUKEI = "TRUE" Then
            item = New MyListViewItem("任意図形作成",
                                      "/YCM;component/Image2/MMainGamen/3_1.png",
                                      "/YCM;component/Image2/MMainGamen/3_2.png")
            item.ID = -3
            m_ListNiniItems.Add(item)
            '#End If
        End If
        ListNini.ItemsSource = m_ListNiniItems
    End Sub

    '専用選択リスト作成
    Private Sub SetListSenyou()

        Dim TypeInfoD As New TypeInfoTable
        TypeInfoD.m_dbClass = m_system_dbclass
        TypeInfoDL = TypeInfoD.GetDataToList
        If TypeInfoDL Is Nothing Then
            Exit Sub
        End If

        ListSenyou.ItemsSource = Nothing
        With m_ListSenyouItems
            .Clear()

            Dim item As MyListViewItem
            For iRows = 0 To TypeInfoDL.Count - 1
                item = New MyListViewItem("任意座標計測",
                                          "/YCM;component/Image2/MMainGamen/n_1.png",
                                          "/YCM;component/Image2/MMainGamen/n_2.png")
                item.Text = TypeInfoDL(iRows).type_name.Trim
                item.ID = TypeInfoDL(iRows).ID

                .Add(item)
            Next
        End With
        ListSenyou.ItemsSource = m_ListSenyouItems

        'Me.Refresh()

    End Sub

    ' 各フラグのクリア
    Private Sub FlgClear()
        m_flg_Zahyo = False
        m_flg_Sunpo = False
        m_flg_Zukei = False
        m_flg_Senyou = False
        m_Senyou_Id = -1
        m_SenyouList_No = -1
        m_NiniListSelectFlg = 0
        m_SenyouListSelectFlg = 0
    End Sub

    '選択行を取得し、各フラグを設定する

    Private Function SelectGet() As Integer

        '選択行がない場合、メッセージを出力する

        If m_NiniListSelectFlg = 0 And m_SenyouListSelectFlg = 0 Then
            MsgBox("対象物を選択してください。", MsgBoxStyle.OkOnly)
            Return -1
            Exit Function
        End If

        '選択行の取得

        If m_flg_Senyou = True Then
            m_Senyou_Id = CInt(TypeInfoDL(ListSenyou.SelectedIndex).ID)
            m_SenyouList_No = ListSenyou.SelectedIndex
        End If

        If m_flg_Senyou = True And (m_Senyou_Id = -1 Or m_SenyouList_No = -1) Then
            MsgBox("専用計測の取得でエラー発生", MsgBoxStyle.OkOnly)
            Return -1
            Exit Function
        End If

        Return 0

    End Function

    '閉じるボタン押下（フォームクローズ）

    'Private Sub ButtonClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonClose.Click
    '    Try
    '        DisConnectDB(m_system_dbclass)
    '    Catch ex As Exception

    '    End Try
    '    FlgClear()
    '    Me.Close()
    'End Sub

    Private Sub ListNini_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles ListNini.SelectionChanged
        If ListNini.SelectedItems.Count > 0 Then
            FlgClear()

            'Select Case ListNini.SelectedIndex
            '    Case 0
            '        m_flg_Zahyo = True
            '        m_Senyou_Id = -1
            '    Case 1
            '        m_flg_Sunpo = True
            '        m_Senyou_Id = -2
            '    Case 2
            '        m_flg_Zukei = True
            '        m_Senyou_Id = -3
            'End Select
            Select Case ListNini.Items(ListNini.SelectedIndex).Text
                Case "任意座標計測"
                    m_flg_Zahyo = True
                    m_Senyou_Id = -1
                Case "任意寸法計測"
                    m_flg_Sunpo = True
                    m_Senyou_Id = -2
                Case "任意図形作成"
                    m_flg_Zukei = True
                    m_Senyou_Id = -3
            End Select

            m_SenyouList_No = 0
            m_NiniListSelectFlg = 1

            ListSenyou.SelectedIndex = -1

            ChangeKeisoku()
        Else
            Exit Sub
        End If
        'ListNini.HideSelection = False
        'ListSenyou.HideSelection = True

    End Sub

    Private Sub ListSenyou_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles ListSenyou.SelectionChanged
        If ListSenyou.SelectedItems.Count > 0 Then
            FlgClear()

            m_flg_Senyou = True
            m_SenyouListSelectFlg = 1

            ListNini.SelectedIndex = -1

            ChangeKeisoku()
        Else
            Exit Sub
        End If
        'ListNini.HideSelection = True
        'ListSenyou.HideSelection = False

    End Sub

    Private Sub Grid_KojiDataConf_SizeChanged(ByVal sender As Object, ByVal e As System.Windows.SizeChangedEventArgs) Handles Grid_KojiDataConf.SizeChanged
        If m_KojiDataConfUserControl IsNot Nothing Then
            m_KojiDataConfUserControl.Width = Grid_KojiDataConf.ActualWidth
            m_KojiDataConfUserControl.Height = Grid_KojiDataConf.ActualHeight
        End If
    End Sub

    Private Sub SelectListSenyou()
        'Dim ret As Boolean = False
        strpath = BeforePointWords(cmds.GetValue(1).ToString)

        Dim bln As Boolean = False

        Try
            '****************************************************************************
            While bln = False
                Dim cdbDB As New CDBOperate
                Dim strpathmdb As String = strpath & "計測データ.mdb"
                'MsgBox(strpathmdb)
                If cdbDB.ConnectDB(strpathmdb) = False Then
                    MsgBox("計測データ.mdbへの接続に失敗しました。")
                    Exit Sub
                End If
                Dim strSql As String = "SELECT * FROM KihonInfo"
                Dim adoRst As ADODB.Recordset = cdbDB.CreateRecordset(strSql)
                type_id = adoRst.Fields("TypeID").Value
                Dim indx As New Integer
                If type_id > 0 Then
                    For Each id As TypeInfoTable In TypeInfoDL
                        If id.ID = type_id Then
                            ListSenyou.SelectedIndex = indx
                        End If
                        indx += 1
                    Next
                End If
                'cdbDB.DisConnectDB()
                '****************************************************************************

                '****************************************************************************
                Dim cdbDB1 As New CDBOperate
                Dim idd As New Integer
                indx = 0

                For Each WK As WorksTable In m_KojiDataConfUserControl.WorksT.WorksL
                    If WK.SavePath = strpath Then
                        idd = indx
                        bln = True
                        Exit For
                    End If
                    indx += 1
                Next

                If idd > 0 Then
                    For i As Integer = 0 To m_KojiDataConfUserControl.DataGridView1.Items.Count - 1
                        If idd = m_KojiDataConfUserControl.DataGridView1.Items(i).Item("LIDX") Then
                            m_KojiDataConfUserControl.DataGridView1.SelectedIndex = i
                            'm_KojiDataConfUserControl.openKentouKihonInfoTabDialog()
                            flgInstantRun = True
                            Exit For
                        End If
                    Next
                Else
                    strpathmdb = My.Application.Info.DirectoryPath & "\計測システムフォルダ\システム設定.mdb"
                    Dim dt As Date = Date.Now
                    Dim maxid As Integer = -1
                    If cdbDB1.ConnectDB(strpathmdb) = False Then
                        MsgBox("システム設定.mdbへの接続に失敗しました。")
                        Exit Sub
                    End If

                    strSql = "INSERT INTO Works(SavePath, CreateDate, TypeID, flg_Kihon, flg_Kiteiti," & _
                                           "flg_Gazou, flg_Kaiseki, flg_Kakunin, flg_Out) " & _
                                           "VALUES('" & strpath & "', '" & dt & "', " & type_id & ", 1, 1, 1, 1, 1, 0)"
                    If cdbDB1.ExcuteSQL(strSql) = True Then
                        strSql = "SELECT max(ID) FROM Works"
                        Dim adoRstmaxID As ADODB.Recordset = cdbDB1.CreateRecordset(strSql)
                        If adoRstmaxID.Fields.Count > 0 Then
                            maxid = adoRstmaxID.Fields(0).Value
                        End If
                    End If

                    If maxid > 0 Then
                        adoRst.MoveFirst()
                        While adoRst.EOF = False
                            strSql = "INSERT INTO WorksKihon(WorkID, KI_ID, ItemValue) " & _
                             "VALUES(" & maxid & ", " & adoRst.Fields("ID").Value & ", '" & adoRst.Fields("ItemValue").Value & "')"
                            cdbDB1.ExcuteSQL(strSql)
                            adoRst.MoveNext()
                        End While
                        SetListSenyou()
                    End If
                    cdbDB1.DisConnectDB()
                End If
                cdbDB.DisConnectDB()
            End While
            '****************************************************************************
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try


    End Sub

    Private Function IsVFormFile() As Boolean
        Dim ret As Boolean = True
        strpath = BeforePointWords(cmds.GetValue(1).ToString)
        'strpath = cmds.GetValue(1).ToString
        'MsgBox(strpath)
        Dim strpathmdb = strpath & "計測データ.mdb"
        'MsgBox(strpathmdb)
        If IO.File.Exists(strpathmdb) = False Then
            MsgBox("計測データ.mdb ファイルがありません")
            ret = False
            Exit Function
        End If

        strpathmdb = strpath & "Pdata\dbFBM.mdb"
        If IO.File.Exists(strpathmdb) = False Then
            MsgBox("Pdataフォルダがありません。")
            ret = False
            Exit Function
        End If

        Dim i As New Integer
        For Each fl In Directory.GetFiles(strpath & "\", "*.jpg")
            i += 1
        Next
        If i < 1 Then
            MsgBox("既存工事フォルダには写真がありません。")
            ret = False
            Exit Function
        End If

        i = 0
        For Each fl In Directory.GetFiles(strpath & "\Pdata\", "*.jpg")
            i += 1
        Next
        If i < 1 Then
            MsgBox("PdataフォルダにはICON写真がありません")
            ret = False
            Exit Function
        End If

        Return ret
    End Function
    Private Function BeforePointWords(input As String) As String
        Dim words = 0
        Dim ret As String = ""
        Dim i As Integer
        For i = input.Length - 1 To 0 Step -1
            If input(i) = "\" Then
                words -= 1
            End If
            If words = 0 Then
                ret = input.Substring(0, i)
            End If
        Next
        Return ret
    End Function

    'add by tsene 2016/05/12 start
    Private Sub dispatcherTimer_Tick()
        If flgInstantRun = True Then
            flgInstantRun = False
            m_KojiDataConfUserControl.openKentouKihonInfoTabDialog()
        End If
    End Sub
    'add by tsene 2016/05/12 end


    'Add Kiryu 20170331 テーブルのcsv出力
    Private Sub SystemSettingExport(sender As System.Object, e As System.Windows.RoutedEventArgs)

        Dim FolderBrowserDialog1 As New FolderBrowserDialog
        If Not FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Return
        End If
        Dim ExportDirPath As String = FolderBrowserDialog1.SelectedPath


        Dim system_dbclass As FBMlib.CDBOperateOLE
        If ConnectSystemDB(system_dbclass) = False Then
            MsgBox(err_msg_systemdb_connect, MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        'Dim TypeInfoD As New TypeInfoTable
        'TypeInfoD.m_dbClass = system_dbclass
        'TypeInfoDL = TypeInfoD.GetDataToList
        'If TypeInfoDL Is Nothing Then
        '    Exit Sub
        'End If

        'Dim sel As Integer = ListSenyou.SelectedIndex
        'Dim SelectItemTypeID As Integer = TypeInfoDL(sel).ID

        Dim DBPath As String = My.Application.Info.DirectoryPath & "\計測システムフォルダ\" & strSystemMdbFile
        If system_dbclass Is Nothing Then
            If ConnectDB(DBPath, system_dbclass) = False Then
                MsgBox(strSystemMdbFile & "への接続に失敗しました。")
                Exit Sub
            End If
        End If

        Try

            Dim CToffsetD As New CToffset
            CToffsetD.CsvOut(ExportDirPath + "\" + "db_out_CT_Bunrui.csv", system_dbclass)

            Dim CTCoordD As New CT_CoordSettingData
            CTCoordD.CsvOut(ExportDirPath + "\" + "db_out_CT_CoordSetting.csv", system_dbclass)

            Dim KIDX As New KihonInfoTable
            KIDX.CsvOut(ExportDirPath + "\" + "db_out_KihonInfo.csv", system_dbclass)

            Dim SSD As New SunpoSetTable
            SSD.CsvOut(ExportDirPath + "\" + "db_out_SunpoSetTabel.csv", system_dbclass)

            Dim TGD As New TenGunTeigiTable
            TGD.CsvOut(ExportDirPath + "\" + "db_out_TengunTeigiTable.csv", system_dbclass)


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        DisConnectDB(system_dbclass)  'システム設定DBへ切断

        MsgBox("出力が完了しました。")

    End Sub

    Private Sub SystemSettingNewInport(sender As System.Object, e As System.Windows.RoutedEventArgs)

        Dim system_dbclass As FBMlib.CDBOperateOLE
        If ConnectSystemDB(system_dbclass) = False Then
            MsgBox(err_msg_systemdb_connect, MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        Dim strSelMax As String = "SELECT MAX( ID ) FROM TypeInfo  " 'TypeIDの最大値を取得
        Dim IDR As IDataReader = system_dbclass.DoSelect(strSelMax)
        IDR.Read()
        Dim MaxTypeID As Integer = IDR.GetValue(0)
        Dim NewTypeID As Integer = MaxTypeID + 1

        Dim SysInfoName As String = "Notitle"
        Dim flgActive As Integer = 1
        Dim flgSekkei As Integer = 0

        SysInfoName = InputBox("設定名を入力してください。", "システム設定名入力", "設定1", 300, 540)


        Dim strSql As String = "INSERT INTO TypeInfo(ID,Name,flg_Active,flg_sekkei) " & " VALUES(" & NewTypeID & ",'" & SysInfoName & "'," & flgActive & "," & flgSekkei & ")"
        system_dbclass.ExecuteSQL(strSql)


        DisConnectDB(system_dbclass)

        SystemSettingInport(NewTypeID)

        MMainGamen_Load(Nothing, Nothing)

        MsgBox("新規インポートが完了しました。")

    End Sub

    Private Sub SystemSettingOverWriteInport(sender As System.Object, e As System.Windows.RoutedEventArgs)

        SystemSettingInport(CommonTypeID)

        MsgBox("上書きインポートが完了しました。")


    End Sub


    Private Sub SystemSettingInport(ByVal TypeID As Integer)

        Dim FolderBrowserDialog1 As New FolderBrowserDialog
        If Not FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Return
        End If

        Dim INportDirPath As String = FolderBrowserDialog1.SelectedPath

        Dim system_dbclass As FBMlib.CDBOperateOLE
        If ConnectSystemDB(system_dbclass) = False Then
            MsgBox(err_msg_systemdb_connect, MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        SystemSettingDelete(system_dbclass, TypeID, False)

        Try
            Dim ReadCT_Bunrui As New CToffset
            ReadCT_Bunrui.CsvInput(INportDirPath + "\" + "db_out_CT_Bunrui.csv", system_dbclass, TypeID)

            Dim ReadCTCoordD As New CT_CoordSettingData
            ReadCTCoordD.CsvInput(INportDirPath + "\" + "db_out_CT_CoordSetting.csv", system_dbclass, TypeID)

            Dim ReadKihonInfo As New KihonInfoTable
            ReadKihonInfo.CsvInport(INportDirPath + "\" + "db_out_KihonInfo.csv", system_dbclass, TypeID)

            Dim ReadSunpoSet As New SunpoSetTable
            ReadSunpoSet.CsvInport(INportDirPath + "\" + "db_out_SunpoSetTabel.csv", system_dbclass, TypeID)

            Dim ReadTenGunTeigi As New TenGunTeigiTable
            ReadTenGunTeigi.CsvInport(INportDirPath + "\" + "db_out_TengunTeigiTable.csv", system_dbclass, TypeID)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        DisConnectDB(system_dbclass) 'システム設定DBへ切断


    End Sub

    Private Sub SystemSettingSelectItemDelete(sender As System.Object, e As System.Windows.RoutedEventArgs)

        Dim system_dbclass As FBMlib.CDBOperateOLE
        If ConnectSystemDB(system_dbclass) = False Then
            MsgBox(err_msg_systemdb_connect, MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        SystemSettingDelete(system_dbclass, CommonTypeID, True)

        'Dim strSelMax As String = "SELECT ID FROM TypeInfo WHER  " 'レコードの少ないテーブルからTypeIDの最大値を取得
        'Dim IDR As IDataReader = system_dbclass.DoSelect(strSelMax)
        'IDR.Read()
        'Dim MaxTypeID As Integer = IDR.GetValue(0)

        DisConnectDB(system_dbclass)

        MsgBox("削除しました。")


        'メニューアイコンリロード
        MMainGamen_Load(Nothing, Nothing)

    End Sub


    Private Sub SystemSettingDelete(ByRef dbClass As FBMlib.CDBOperateOLE, ByVal TypeID As Integer, ByVal DelTypeIDFlg As Boolean)

        'TypeIDが一致するフィールドを削除
        Dim strWhere As String = "TypeID =" & TypeID.ToString

        dbClass.DoDelete("CT_Bunrui", strWhere)
        dbClass.DoDelete("CT_CoordSetting", strWhere)
        dbClass.DoDelete("KihonInfo", strWhere)
        dbClass.DoDelete("SunpoSet", strWhere)
        dbClass.DoDelete("TenGunTeigi", "タイプID =" & TypeID.ToString)
        If DelTypeIDFlg Then
            dbClass.DoDelete("TypeInfo", "ID =" & CommonTypeID.ToString)
        End If

    End Sub

    Private Sub ButtonNew_Loaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles ButtonNew.Loaded

    End Sub
End Class

