Imports Microsoft.Office.Interop
Imports System.Runtime.InteropServices
Imports System
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Diagnostics

Public Class KentouKihonInfoTabDialog

#Region "各ボタンの設定値"

    '始めに
    '位置
    Public Const lct_btn_tab1_x As Integer = 5
    Public Const lct_btn_tab1_y As Integer = 0
    'サイズ
    Public Const size_btn_tab1_w As Integer = 155
    Public Const size_btn_tab1_h As Integer = 50

    '基本情報
    '位置
    Public Const lct_btn_tab2_x As Integer = 110
    Public Const lct_btn_tab2_y As Integer = 0
    'サイズ
    Public Const size_btn_tab2_w As Integer = 155
    Public Const size_btn_tab2_h As Integer = 50

    '規定値
    '位置
    Public Const lct_btn_tab3_x As Integer = 215
    Public Const lct_btn_tab3_y As Integer = 0
    'サイズ
    Public Const size_btn_tab3_w As Integer = 155
    Public Const size_btn_tab3_h As Integer = 50

    '画像

    '位置
    Public Const lct_btn_tab4_x As Integer = 320
    Public Const lct_btn_tab4_y As Integer = 0
    'サイズ
    Public Const size_btn_tab4_w As Integer = 155
    Public Const size_btn_tab4_h As Integer = 50

    '解析

    '位置
    Public Const lct_btn_tab5_x As Integer = 425
    Public Const lct_btn_tab5_y As Integer = 0
    'サイズ
    Public Const size_btn_tab5_w As Integer = 155
    Public Const size_btn_tab5_h As Integer = 50

    '寸法解析

    '位置
    Public Const lct_btn_tab6_x As Integer = 530
    Public Const lct_btn_tab6_y As Integer = 0
    'サイズ
    Public Const size_btn_tab6_w As Integer = 155
    Public Const size_btn_tab6_h As Integer = 50

    '検査表出力

    '位置
    Public Const lct_btn_tab7_x As Integer = 635
    Public Const lct_btn_tab7_y As Integer = 0
    'サイズ
    Public Const size_btn_tab7_w As Integer = 155
    Public Const size_btn_tab7_h As Integer = 50


#End Region

    Public m_Keisoku_mdb_path As String                         '計測データ.mdbパス
    Public m_keisoku_dbclass As FBMlib.CDBOperateOLE            '計測データ.mdb
    Public m_system_dbclass As FBMlib.CDBOperateOLE             'システム設定.mdb

    Public TypeInfoDL As List(Of TypeInfoTable)                 'TypeIDテーブルリスト

    Public SunpoSetL As List(Of SunpoSetTable)                  'SunpoSetテーブルリスト


    Dim m_TemplatePath As String = System.Environment.CurrentDirectory & "\計測システムフォルダ\Template"
    'Dim m_TemplatePath As String = My.Application.Info.DirectoryPath & "\計測システムフォルダ\Template"
    'Dim m_TemplateExcelFile As String

    'Dim m_db_path As String = System.Environment.CurrentDirectory & "\" & m_Keisoku_mdb_name
    'Dim m_db_path As String = My.Application.Info.DirectoryPath & "\" & m_Keisoku_mdb_name

    Dim m_KeisokuTempMdbPath As String = System.Environment.CurrentDirectory & "\計測システムフォルダ\Template\" & m_Keisoku_mdb_name
    'Dim m_KeisokuTempMdbPath As String = My.Application.Info.DirectoryPath & "\計測システムフォルダ\Template\" & m_Keisoku_mdb_name

    Public m_gazou_path As String           '画像フォルダ

    Dim m_gazou_event As Boolean

    '  Public WorksD As WorksTable

    Public flg_camera_grid As Boolean

    'FolderBrowserDialogクラスのインスタンスを作成
    Private m_fbd As FolderBrowserDialog        '新規フォルダ作成可

    Private m_fbd_s As FolderBrowserDialog      '参照のみ

    Private m_fod As OpenFileDialog             'フォルダ指定用

    Public kojiData As KojiDataConfUserControl

    Dim KensaHyoOutput As New KensaHyoOutput

    Public IsNew As Boolean = True
    Public bln_KaisekiOpened As Boolean = False
    Private m_KiteitiItems As New System.Collections.ObjectModel.ObservableCollection(Of KiteitiItem)
    ' 規定値
    Class KiteitiItem
        Public Property No() As Integer             'No
        Public Property MeasurementSet() As String  '計測セット 'byambaa 20160822
        Public Property Bui() As String             '部位

        Public Property KensaItem() As String       '検査項目
        Public Property KiteiVal() As Double        '規定値
        Public Property KiteiMin() As Double        '最小許容値
        Public Property KiteiMax() As Double        '最大許容値
        Public Property Targettype() As String
        Public Property flgOutZu() As Boolean
        Public Property ZU_layer() As String
        Public Property ZU_color1() As String
        Public Property ZU_LineType1() As String
        'BYAMBAA ADD 20160904
        Public Property sek_flgOutZu() As Boolean
        Public Property sek_ZU_layer() As String
        Public Property sek_ZU_color1() As String
        Public Property sek_ZU_LineType1() As String

        Public Shared Property ZU_colors1 As System.Collections.ObjectModel.ObservableCollection(Of String)
        Public Shared Property ZU_LineTypes1 As System.Collections.ObjectModel.ObservableCollection(Of String)
        Public Shared Property sek_ZU_colors1 As System.Collections.ObjectModel.ObservableCollection(Of String)
        Public Shared Property sek_ZU_LineTypes1 As System.Collections.ObjectModel.ObservableCollection(Of String)
        Public Property RowIndex() As Integer
           
        Shared Sub New()
            ZU_colors1 = New System.Collections.ObjectModel.ObservableCollection(Of String)
            ZU_colors1.Add("BLACK")
            ZU_colors1.Add("RED")
            ZU_colors1.Add("GREEN")
            ZU_colors1.Add("BLUE")
            ZU_colors1.Add("YELLOW")
            ZU_colors1.Add("MAGENTA")
            ZU_colors1.Add("CYAN")
            ZU_colors1.Add("WHITE")
            ZU_colors1.Add("DEEPPINK")
            ZU_colors1.Add("BROWN")
            ZU_colors1.Add("ORANGE")
            ZU_colors1.Add("LIGHTGREEN")
            ZU_colors1.Add("LIGHTBLUE")
            ZU_colors1.Add("LAVENDER")
            ZU_colors1.Add("LIGHTGRAY")
            ZU_colors1.Add("DARKGRAY")
            ZU_colors1.Add("SEAGREEN")
            ZU_colors1.Add("STEELBLUE")

            ZU_LineTypes1 = New System.Collections.ObjectModel.ObservableCollection(Of String)
            ZU_LineTypes1.Add("CONTINUOUS")
            ZU_LineTypes1.Add("DASHED")

            sek_ZU_colors1 = New System.Collections.ObjectModel.ObservableCollection(Of String)
            sek_ZU_colors1.Add("BLACK")
            sek_ZU_colors1.Add("RED")
            sek_ZU_colors1.Add("GREEN")
            sek_ZU_colors1.Add("BLUE")
            sek_ZU_colors1.Add("YELLOW")
            sek_ZU_colors1.Add("MAGENTA")
            sek_ZU_colors1.Add("CYAN")
            sek_ZU_colors1.Add("WHITE")
            sek_ZU_colors1.Add("DEEPPINK")
            sek_ZU_colors1.Add("BROWN")
            sek_ZU_colors1.Add("ORANGE")
            sek_ZU_colors1.Add("LIGHTGREEN")
            sek_ZU_colors1.Add("LIGHTBLUE")
            sek_ZU_colors1.Add("LAVENDER")
            sek_ZU_colors1.Add("LIGHTGRAY")
            sek_ZU_colors1.Add("DARKGRAY")
            sek_ZU_colors1.Add("SEAGREEN")
            sek_ZU_colors1.Add("STEELBLUE")

            sek_ZU_LineTypes1 = New System.Collections.ObjectModel.ObservableCollection(Of String)
            sek_ZU_LineTypes1.Add("CONTINUOUS")
            sek_ZU_LineTypes1.Add("DASHED")

        End Sub
        'BYAMBAA ADD 20160904

    End Class

    Private m_KakuninItems As New System.Collections.ObjectModel.ObservableCollection(Of KakuninViewItem)
    ' 寸法確認

    Class KakuninItem   ' DataGridView9

        Public Property No() As Integer             'No
        Public Property Bui() As String             '部位
        Public Property MeasurementSet() As String  '計測セット 'byambaa 20160822
        Public Property KensaItem() As String       '検査項目
        Public Property KiteiVal() As Double        '規定値
        Public Property KiteiMin() As Double        '最小許容値
        Public Property KiteiMax() As Double        '最大許容値
        Public Property SunpoVal() As Double        '計測値
        Public Property flg_gouhi() As String
        Public Property CTIDs() As String
        Public Property Targettype() As String
        Public Property flgOutZu() As Boolean
        Public Property ZU_layer() As String
        Public Property ZU_color2() As String
        Public Property ZU_LineType2() As String
        'BYAMBAA ADD 20160904
        Public Property sek_flgOutZu() As Boolean
        Public Property sek_ZU_layer() As String
        Public Property sek_ZU_color2() As String
        Public Property sek_ZU_LineType2() As String

        '20161110 baluu ADD start
        Public Property sek_Val() As String
        Public Property sek_sunpo_Val() As String


        '20161110 baluu ADD end

        Public Shared Property ZU_colors2 As System.Collections.ObjectModel.ObservableCollection(Of String)
        Public Shared Property ZU_LineTypes2 As System.Collections.ObjectModel.ObservableCollection(Of String)
        Public Shared Property sek_ZU_colors2 As System.Collections.ObjectModel.ObservableCollection(Of String)
        Public Shared Property sek_ZU_LineTypes2 As System.Collections.ObjectModel.ObservableCollection(Of String)

        Shared Sub New()
            ZU_colors2 = New System.Collections.ObjectModel.ObservableCollection(Of String)
            ZU_colors2.Add("BLACK")
            ZU_colors2.Add("RED")
            ZU_colors2.Add("GREEN")
            ZU_colors2.Add("BLUE")
            ZU_colors2.Add("YELLOW")
            ZU_colors2.Add("MAGENTA")
            ZU_colors2.Add("CYAN")
            ZU_colors2.Add("WHITE")
            ZU_colors2.Add("DEEPPINK")
            ZU_colors2.Add("BROWN")
            ZU_colors2.Add("ORANGE")
            ZU_colors2.Add("LIGHTGREEN")
            ZU_colors2.Add("LIGHTBLUE")
            ZU_colors2.Add("LAVENDER")
            ZU_colors2.Add("LIGHTGRAY")
            ZU_colors2.Add("DARKGRAY")
            ZU_colors2.Add("SEAGREEN")
            ZU_colors2.Add("STEELBLUE")

            ZU_LineTypes2 = New System.Collections.ObjectModel.ObservableCollection(Of String)
            ZU_LineTypes2.Add("CONTINUOUS")
            ZU_LineTypes2.Add("DASHED")

            sek_ZU_colors2 = New System.Collections.ObjectModel.ObservableCollection(Of String)
            sek_ZU_colors2.Add("BLACK")
            sek_ZU_colors2.Add("RED")
            sek_ZU_colors2.Add("GREEN")
            sek_ZU_colors2.Add("BLUE")
            sek_ZU_colors2.Add("YELLOW")
            sek_ZU_colors2.Add("MAGENTA")
            sek_ZU_colors2.Add("CYAN")
            sek_ZU_colors2.Add("WHITE")
            sek_ZU_colors2.Add("DEEPPINK")
            sek_ZU_colors2.Add("BROWN")
            sek_ZU_colors2.Add("ORANGE")
            sek_ZU_colors2.Add("LIGHTGREEN")
            sek_ZU_colors2.Add("LIGHTBLUE")
            sek_ZU_colors2.Add("LAVENDER")
            sek_ZU_colors2.Add("LIGHTGRAY")
            sek_ZU_colors2.Add("DARKGRAY")
            sek_ZU_colors2.Add("SEAGREEN")
            sek_ZU_colors2.Add("STEELBLUE")

            sek_ZU_LineTypes2 = New System.Collections.ObjectModel.ObservableCollection(Of String)
            sek_ZU_LineTypes2.Add("CONTINUOUS")
            sek_ZU_LineTypes2.Add("DASHED")
        End Sub
        'BYAMBAA ADD 20160904

    End Class

    Class KakuninViewItem
        Inherits KakuninItem

        Public Property flg_gouhi2() As String
            Set(ByVal value As String)
                If value = "合" Then
                    flg_gouhi = "1"
                Else
                    Throw New Exception("Unexpected call of KakuninViewItem.flg_gouhi2 = not 1")
                End If
            End Set
            Get
                If flg_gouhi = "1" Then
                    Return "合"
                Else
                    Return "否"
                End If
            End Get
        End Property
        Public Property flg_gouhiBrush() As System.Windows.Media.Brush
            Set(ByVal value As System.Windows.Media.Brush)
                Throw New Exception("Unexpected call of KakuninViewItem.flg_gouhiBrush=?")
            End Set
            Get
                If flg_gouhi = "1" Then
                    Return System.Windows.Media.Brushes.White
                ElseIf flg_gouhi = "9" Then
                    Return System.Windows.Media.Brushes.Gray
                Else
                    Return System.Windows.Media.Brushes.Salmon
                End If
            End Get
        End Property

        Public Property RowIndex() As Integer

        'Public Property Visibility() As System.Windows.Visibility

        'Public Sub New()
        '    Visibility = System.Windows.Visibility.Visible
        'End Sub

    End Class

    Private m_KekkaKakuninItems As New System.Collections.ObjectModel.ObservableCollection(Of KekkaKakuninViewItem)
    ' 寸法確認

    Class KekkaKakuninItem   ' DGW_KekkaKakunin
        Implements System.ComponentModel.INotifyPropertyChanged

        Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

        Public Property SunpoID() As Integer        'No
        Public Property MeasurementSet() As String  '計測セット 'byambaa 20160822
        Public Property SunpoName() As String       '検査項目
        Public Property Calcname() As String        '種類の決定

        Public Property CTIDs() As String
        Public Property seibunXYZ() As String       'XYZ成分の決定


        Private m_SunpoVal As Double
        Public Property SunpoVal() As Double        '計測値
            Set(ByVal value As Double)
                If m_SunpoVal <> value Then
                    m_SunpoVal = value
                    proChanged("SunpoVal")
                End If
            End Set
            Get
                Return m_SunpoVal
            End Get
        End Property
        Public Property flgOutZu() As Boolean       '図化

        Public Property ZU_color() As String        '線色
        Public Property ZU_LineType() As String     '線種

        Public Shared Property seibunXYZs As System.Collections.ObjectModel.ObservableCollection(Of String)
        Public Shared Property ZU_colors As System.Collections.ObjectModel.ObservableCollection(Of String)
        Public Shared Property ZU_LineTypes As System.Collections.ObjectModel.ObservableCollection(Of String)
        'BYAMBAA ADD 20160904
        Public Property sek_flgOutZu() As Boolean       '図化

        Public Property sek_ZU_color() As String        '線色
        Public Property sek_ZU_LineType() As String     '線種

        Public Shared Property sek_ZU_colors As System.Collections.ObjectModel.ObservableCollection(Of String)
        Public Shared Property sek_ZU_LineTypes As System.Collections.ObjectModel.ObservableCollection(Of String)
        'BYAMBAA ADD 20160904

        Private Sub proChanged(ByVal info As String)
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(info))
        End Sub

        Shared Sub New()
            seibunXYZs = New System.Collections.ObjectModel.ObservableCollection(Of String)
            seibunXYZs.Add("XYZ")
            seibunXYZs.Add("XY")
            seibunXYZs.Add("XZ")
            seibunXYZs.Add("YZ")
            seibunXYZs.Add("X")
            seibunXYZs.Add("Y")
            seibunXYZs.Add("Z")

            ZU_colors = New System.Collections.ObjectModel.ObservableCollection(Of String)
            ZU_colors.Add("BLACK")
            ZU_colors.Add("RED")
            ZU_colors.Add("GREEN")
            ZU_colors.Add("BLUE")
            ZU_colors.Add("YELLOW")
            ZU_colors.Add("MAGENTA")
            ZU_colors.Add("CYAN")
            ZU_colors.Add("WHITE")
            ZU_colors.Add("DEEPPINK")
            ZU_colors.Add("BROWN")
            ZU_colors.Add("ORANGE")
            ZU_colors.Add("LIGHTGREEN")
            ZU_colors.Add("LIGHTBLUE")
            ZU_colors.Add("LAVENDER")
            ZU_colors.Add("LIGHTGRAY")
            ZU_colors.Add("DARKGRAY")
            ZU_colors.Add("SEAGREEN")
            ZU_colors.Add("STEELBLUE")

            ZU_LineTypes = New System.Collections.ObjectModel.ObservableCollection(Of String)
            ZU_LineTypes.Add("CONTINUOUS")
            ZU_LineTypes.Add("DASHED")

            'BYAMBAA ADD 20160904
            sek_ZU_colors = New System.Collections.ObjectModel.ObservableCollection(Of String)
            sek_ZU_colors.Add("BLACK")
            sek_ZU_colors.Add("RED")
            sek_ZU_colors.Add("GREEN")
            sek_ZU_colors.Add("BLUE")
            sek_ZU_colors.Add("YELLOW")
            sek_ZU_colors.Add("MAGENTA")
            sek_ZU_colors.Add("CYAN")
            sek_ZU_colors.Add("WHITE")
            sek_ZU_colors.Add("DEEPPINK")
            sek_ZU_colors.Add("BROWN")
            sek_ZU_colors.Add("ORANGE")
            sek_ZU_colors.Add("LIGHTGREEN")
            sek_ZU_colors.Add("LIGHTBLUE")
            sek_ZU_colors.Add("LAVENDER")
            sek_ZU_colors.Add("LIGHTGRAY")
            sek_ZU_colors.Add("DARKGRAY")
            sek_ZU_colors.Add("SEAGREEN")
            sek_ZU_colors.Add("STEELBLUE")

            sek_ZU_LineTypes = New System.Collections.ObjectModel.ObservableCollection(Of String)
            sek_ZU_LineTypes.Add("CONTINUOUS")
            sek_ZU_LineTypes.Add("DASHED")
            'BYAMBAA ADD 20160904
        End Sub
    End Class
    Class KekkaKakuninViewItem
        Inherits KekkaKakuninItem

        Public Property RowIndex() As Integer

        Public Property Visibility() As System.Windows.Visibility

        Public Sub New()
            Visibility = System.Windows.Visibility.Visible
        End Sub

    End Class

    Private m_KaisekiItems As New System.Collections.ObjectModel.ObservableCollection(Of KaisekiItem)
    ' 解析

    Class KaisekiItem   ' DataGridView8
        Public Property SokutenName() As String ' 測点名

        Public Property X() As String
        Public Property Y() As String
        Public Property Z() As String
    End Class

    Friend WithEvents SFD1 As New System.Windows.Forms.SaveFileDialog
    Friend WithEvents ImageList1 As New System.Windows.Forms.ImageList

    Private m_ListViewTabItemsItems As New System.Collections.ObjectModel.ObservableCollection(Of MyListViewItem)
    Class MyListViewItem
        Public Property Img As System.Windows.Media.Imaging.BitmapImage
        Public Property ImgSeleted As System.Windows.Media.Imaging.BitmapImage
        Public Property Text As String

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

    Private Sub KentouKihonInfoTabDialog_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        If flgManualScaleAndOffset = True Then
            flgManualScaleAndOffset = False ' SUSANO ADD
            SetScaleAfterRun()
            'flgManualScaleAndOffset = False SUSANO DELETE 
        End If
    End Sub

    Private Sub KihonInfoTabDialog_FormClosing(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed

        m_CellChange = 0
        Try
            DisConnectDB(m_keisoku_dbclass)
            DisConnectDB(m_system_dbclass)
            If MainFrm IsNot Nothing Then
                MainFrm.Close()
            End If

        Catch ex As Exception

        End Try

    End Sub
    Private Sub KentouKihonInfoTabDialog_PreviewKeyDown(sender As Object, e As Windows.Input.KeyEventArgs) Handles Me.PreviewKeyDown
        'If e.Key = Windows.Input.Key.Enter Then
        '    MainFrm.flgCreateSTEnter = True
        'End If
        'If e.Key = Windows.Input.Key.Escape Then
        '    MainFrm.flgCreateSTCancel = True
        'End If
        If Not MainFrm Is Nothing Then
            If e.Key = Windows.Input.Key.Enter Then
                MainFrm.flgCreateSTEnter = True
            End If
            If e.Key = Windows.Input.Key.Escape Then
                MainFrm.flgCreateSTCancel = True
            End If
        End If
    End Sub
    Private Sub KentouKihonInfoTabDialog_KeyDown(sender As Object, e As Windows.Input.KeyEventArgs) Handles Me.KeyDown
        'If e.Key = Windows.Input.Key.Enter Then
        '    MainFrm.flgCreateSTEnter = True
        'End If
        'If e.Key = Windows.Input.Key.Escape Then
        '    MainFrm.flgCreateSTCancel = True
        'End If
        If Not MainFrm Is Nothing Then
            If e.Key = Windows.Input.Key.Enter Then
                MainFrm.flgCreateSTEnter = True
            End If
            If e.Key = Windows.Input.Key.Escape Then
                MainFrm.flgCreateSTCancel = True
            End If
        End If
    End Sub

    Private Sub KihonInfoTabDialog_Load(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If LicenseCheck() = False Then
            Me.Close()
            Exit Sub
        End If

        '初期設定
        'Rep By Yamada 20140922 Sta -----------------------------------------------------------------------
#If Anchor = 1 Then
        Me.Title = Me.Title & " アンカーボルト計測専用 Ver 1.1"    '20140818 日本車輌リリース
#Else
        Me.Title = Me.Title & " BuildNo" & My.Settings.BuildNo  'Add By Kiryu 20160518
#End If
        'Me.Title = Me.Title & " 2014"   'Add By Yamada 20140620
        'Rep By Yamada 20140922 End -----------------------------------------------------------------------
        SetInitForm()

        ''始めに画面の設定

        'SetTLP1()

        ''基本情報画面の設定

        'SetTLP4()

        ''確認データを表示する
        'SetKakuninData()

        ''カメラ関連を表示する
        'SetCameraGrid()

        ''帳票出力のテンプレートを表示する
        'SetTemplateGrid()

        'SetKihon()

        'CommonTypeIDセット
        'システム設定mdbに接続する

        If ConnectSystemDB(m_system_dbclass) = False Then
            MsgBox(err_msg_systemdb_connect, MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        Dim TypeInfoD As New TypeInfoTable
        TypeInfoD.m_dbClass = m_system_dbclass
        TypeInfoDL = TypeInfoD.GetDataToList
        If TypeInfoDL Is Nothing Then
            Exit Sub
        End If

        If m_flg_Senyou = True Then
            CommonTypeID = TypeInfoDL(m_SenyouList_No).ID
        Else
            CommonTypeID = m_Senyou_Id
        End If

        'システム設定mdbを切断する
        DisConnectDB(m_system_dbclass)

        m_CellChange = 0

        '始めにを表示する
        If Me.IsNew Then
            'SetHajimeniPanel()
            BtnT1New()
        Else
            'SetHajimeniPanel()
            BtnT1Kaishi()
        End If

        'BtnT1Next.PerformClick()

        'SetKihon()
        For j As Integer = m_ListViewTabItemsItems.Count - 1 To 0 Step -1
            Dim doesExist As Boolean = False
            For i As Integer = 0 To TabControl1.Items.Count - 1
                Dim item As System.Windows.Controls.TabItem = TabControl1.Items(i)
                If m_ListViewTabItemsItems(j).Text = item.Header Then
                    doesExist = True
                    Exit For
                End If
            Next
            If Not doesExist Then
                m_ListViewTabItemsItems.RemoveAt(j)
            End If
        Next
        Me.ListViewTabItems.SelectedIndex = 0
       

        'Me.Activate()
        'Me.Hide()
        'Me.Show()
        'Me.FontSize = Me.FontSize
        'Me.RenderSize = Me.RenderSize

    End Sub

    ''' 

    ''' 初期設定

    ''' 2013/05/13 作成
    ''' 

    ''' 
    Private Function SetInitForm() As Boolean

        'Temolateフォルダのシステム設定.mdbを開く


        ''システム設定.mdbに接続する

        'If ConnectDB(m_SystemMdbFullPath, m_system_dbclass) = False Then
        '    MsgBox("データベースを開くことができません。")
        '    SetInitForm = False
        '    Exit Function
        'End If

        'With TabControl1
        '    'タブのサイズを変更できるようにする
        '    .SizeMode = TabSizeMode.Fixed
        '    'タブのサイズを 80x30 にする
        '    .ItemSize = New Size(100, 46)
        'End With

        '各ボタンを制御する
        SetButtonControl()

        '画像フォルダ
        m_gazou_path = My.Settings.strGazouPath

        m_gazou_event = True

        m_fbd = New FolderBrowserDialog
        With m_fbd
            'ルートフォルダを指定する

            'デフォルトでDesktop
            .RootFolder = Environment.SpecialFolder.Desktop
            'ユーザーが新しいフォルダを作成できるようにする
            'デフォルトでTrue
            .ShowNewFolderButton = True
        End With

        m_fbd_s = New FolderBrowserDialog
        With m_fbd_s
            'ルートフォルダを指定する

            'デフォルトでDesktop
            .RootFolder = Environment.SpecialFolder.Desktop
            'ユーザーが新しいフォルダを作成できるようにする
            'デフォルトでTrue
            .ShowNewFolderButton = False
        End With

        'SUURI ADD START 20140912
        m_fod = New OpenFileDialog
        With m_fod
            .InitialDirectory = Environment.SpecialFolder.Desktop
            .CheckFileExists = False
        End With
        'SUURI ADD END 20140912
        ''他工事データからの参照
        'SetKoujiSansyou()

        '表示位置
        With Me
            .Left = 0
            .Top = 0
        End With
        'TabControl1.TabPages(2).Size = New Size(0, 0)
        'TabControl1.TabPages(2).Hide()

    End Function

    ''' 

    ''' 初期設定

    ''' 2013/05/15 作成
    ''' 

    ''' 
    Private Function SetInitFormKitei() As Boolean

        SetInitFormKitei = True

        '計測データ.mdbに接続する

        If ConnectDB(m_Keisoku_mdb_path, m_keisoku_dbclass) = False Then
            MsgBox("データベースを開くことができません。")
            SetInitFormKitei = False
            Exit Function
        End If

    End Function

    ''' 

    ''' ボタン設定

    ''' 2013/05/20 作成
    ''' 

    ''' 
    Private Sub SetButtonControl()

        If m_flg_Zahyo = True Then      '任意座標計測
            '解析画面"戻る"ボタンを隠す

            BtnT5Next.Visibility = System.Windows.Visibility.Hidden

            'その他のボタンを表示OFFする
            TabControl1.Items.Remove(TabKiteiti)
            TabControl1.Items.Remove(TabKakunin)
            'TabControl1.Items.Remove(TabOut)

        ElseIf m_flg_Sunpo = True Or m_flg_Zukei = True Then
            '解析ボタン
            BtnT5Next.Visibility = System.Windows.Visibility.Visible
            '寸法確認ボタン
            BtnT6_Next.Visibility = System.Windows.Visibility.Hidden

            'その他のボタンを表示OFFする
            'TabControl1.Items.Remove(TabKiteiti)
            'TabControl1.Items.Remove(TabOut)

        ElseIf m_flg_Senyou = True Then
            BtnT5Next.Visibility = System.Windows.Visibility.Visible
        End If

        Dim B_KENSAOUT As Integer = GetPrivateProfileInt("Command", "B_KENSAOUT", 0, My.Application.Info.DirectoryPath & "\vform.ini")
        'ADD BY Yamada 201500304 Sta ------検査表出力のON/OFF　追加
        If B_KENSAOUT = 0 Then  'Add Kiryu 20160609 0:OFF　1：ONに変更
            '#If B_KENSAOUT = "FALSE" Then
            TabControl1.Items.Remove(TabOut)
            '#End If
        End If
        'ADD BY Yamada 201500304 Sta ------検査表出力のON/OFF　追加


    End Sub

    ''' 

    ''' ボタン設定

    ''' 2013/05/20 作成
    ''' 

    ''' 
    Private Sub SetCustumButtonControl(ByRef iButton As System.Windows.Controls.Button,
                                       ByVal iText As String,
                                       ByVal iLocationX As Integer,
                                       ByVal iLocationY As Integer,
                                       ByVal iSizeWidth As Integer,
                                       ByVal iSizeHeight As Integer,
                                       Optional ByRef iType As Integer = 0)
        With iButton
            'コントロールのサイズを適当に変更
            .Margin = New System.Windows.Thickness(iLocationX, iLocationY, iSizeWidth, iSizeHeight)
            .Content = iText
            '.TextAlign = ContentAlignment.MiddleCenter
            Dim points() As Point
            Dim types() As Byte
            'ボタンの形状
            If iType = 0 Then
                '------
                '-     -
                '-      -
                '-     -
                '------
                points = _
                    {New Point(0, 0), _
                    New Point(0, 50), _
                    New Point(100 + 2, 50), _
                    New Point(125 + 2, 25), _
                    New Point(100 + 2, 0), _
                    New Point(0, 0)}
                types = _
                    {Drawing.Drawing2D.PathPointType.Line, _
                    Drawing.Drawing2D.PathPointType.Line, _
                    Drawing.Drawing2D.PathPointType.Line, _
                    Drawing.Drawing2D.PathPointType.Line, _
                    Drawing.Drawing2D.PathPointType.Line, _
                    Drawing.Drawing2D.PathPointType.Line}
            ElseIf iType = 1 Then
                '------
                ' -    -
                '  -    -
                ' -    -
                '------
                points = _
                    {New Point(0, 50), _
                    New Point(100 + 2, 50), _
                    New Point(125 + 2, 25), _
                    New Point(100 + 2, 0), _
                    New Point(0, 0), _
                    New Point(25 + 1, 25), _
                    New Point(0, 50)}
                types = _
                    {Drawing.Drawing2D.PathPointType.Line, _
                    Drawing.Drawing2D.PathPointType.Line, _
                    Drawing.Drawing2D.PathPointType.Line, _
                    Drawing.Drawing2D.PathPointType.Line, _
                    Drawing.Drawing2D.PathPointType.Line, _
                    Drawing.Drawing2D.PathPointType.Line, _
                    Drawing.Drawing2D.PathPointType.Line}
            Else
                '--------
                ' -     -
                '  -    -
                ' -     -
                '--------
                points = _
                     {New Point(0, 50), _
                     New Point(125 + 2, 50), _
                     New Point(125 + 2, 0), _
                     New Point(0, 0), _
                     New Point(25 + 1, 25), _
                     New Point(0, 50)}
                types = _
                    {Drawing.Drawing2D.PathPointType.Line, _
                    Drawing.Drawing2D.PathPointType.Line, _
                    Drawing.Drawing2D.PathPointType.Line, _
                    Drawing.Drawing2D.PathPointType.Line, _
                    Drawing.Drawing2D.PathPointType.Line, _
                    Drawing.Drawing2D.PathPointType.Line}
            End If
            'GraphicsPathの作成
            Dim path1 As New Drawing2D.GraphicsPath(points, types)
            'コントロールの形を変更
            '.Region = New Region(path1)
        End With

    End Sub

    '''' 

    '''' 他工事データからの参照ボタン設定

    '''' 2013/05/20 作成
    '''' 

    '''' 
    'Private Sub SetKoujiSansyou()

    '    WorksD = New WorksTable
    '    WorksD.m_dbClass = m_system_dbclass
    '    WorksD.GetDataToList()
    '    If WorksD.KihonL Is Nothing Then
    '        Exit Sub
    '    End If

    '    Dim WorksT As Works
    '    WorksT = New Works(m_system_dbclass)
    '    WorksT.WKihonL = New List(Of KihonInfoTable)
    '    WorksT.KihonInfoCopy(WorksD.KihonL)
    '    WorksT.GetDataToList()
    '    If WorksT.WorksL.Count = 0 Then
    '        BtnT2KojiSansyou.Enabled = False
    '    Else
    '        BtnT2KojiSansyou.Enabled = True
    '    End If

    'End Sub

    ''' 

    ''' 他工事データからの参照ボタン設定

    ''' 2013/05/23 作成
    ''' 

    ''' 
    Private Sub SetKoujiSansyou()

        'WorksD = New WorksTable
        'WorksD.m_dbClass = m_system_dbclass
        'WorksD.GetDataToList()
        'If WorksD.KihonL Is Nothing Then
        '    Exit Sub
        'End If

        Dim WorksT As Works
        WorksT = New Works(m_system_dbclass)
        WorksT.WKihonL = New List(Of KihonInfoTable)
        WorksT.KihonInfoCopy(WorksD.KihonL)
        WorksT.GetDataToList()
        If WorksT.WorksL.Count = 0 Then
            BtnT2KojiSansyou.IsEnabled = False
        Else
            BtnT2KojiSansyou.IsEnabled = True
        End If

    End Sub

    ''' 

    ''' 他工事データからの参照ボタン設定

    ''' 2013/05/24 作成
    ''' 

    ''' 
    Private Sub SetKoujiSansyou(ByVal m_dbclass As FBMlib.CDBOperateOLE)

        Dim WorksT As Works
        WorksT = New Works(m_dbclass)
        WorksT.WKihonL = New List(Of KihonInfoTable)
        WorksT.KihonInfoCopy(WorksD.KihonL)
        WorksT.GetDataToList()
        If WorksT.WorksL.Count = 0 Then
            BtnT2KojiSansyou.IsEnabled = False
        Else
            BtnT2KojiSansyou.IsEnabled = True
        End If

    End Sub



    ''' 

    ''' グリッドヘッダ設定

    ''' 2013/05/15 作成
    ''' 

    ''' 
    Private Sub SetGridHeaddderKitei()

        With DataGridView6 '規定値

            .CanUserDeleteRows = False

            'ヘッダを中央
            '.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeaderStyle = Application.Current.Resources("DataGrid_Header_Center")

            '並び替え禁止
            For Each T As System.Windows.Controls.DataGridColumn In DataGridView6.Columns
                T.CanUserSort = False
            Next

            '.DefaultCellStyle.Background = System.Windows.Media.Brushes.White 'H25.5.25 Yamada

            '.Columns(3).DefaultCellStyle.Format = "###0.0" '規定値
            '.Columns(4).DefaultCellStyle.Format = "###0.0" '最小許容値
            '.Columns(5).DefaultCellStyle.Format = "###0.0" '最大許容値

            .ItemsSource = m_KiteitiItems
        End With

    End Sub

    ''' 

    ''' 規定値データをグリッドに表示
    ''' 2013/05/24 作成
    ''' 

    ''' 
    Private Sub SetKiteiDataToGrid(ByVal cbVal As String)

        Dim iRowNo As Integer = 0
        'データ表示
        Dim items As New System.Collections.ObjectModel.ObservableCollection(Of KiteitiItem)

        With items
            .Clear()
            If WorksD.SunpoSetL.Count < 20 Then
                For i As Integer = 1 To 20
                    .Add(New KiteitiItem)
                Next
            Else
                For i As Integer = 0 To WorksD.SunpoSetL.Count - 1
                    .Add(New KiteitiItem)
                Next
            End If
            For i As Integer = 0 To WorksD.SunpoSetL.Count - 1

                iRowNo = i + 1
                If cbVal = WorksD.SunpoSetL(i).MeasurementSet Or WorksD.SunpoSetL(i).MeasurementSet = "" Or cbVal = "" Then
                    Try
                        .Item(i).No = iRowNo                                'No
                        .Item(i).MeasurementSet = WorksD.SunpoSetL(i).MeasurementSet '計測セット　(20160822 byambaa ADD)
                        .Item(i).Bui = WorksD.SunpoSetL(i).SunpoMark        '部位
                        .Item(i).KensaItem = WorksD.SunpoSetL(i).SunpoName  '検査項目

                        'Rep By Yamada 20140919 Sta ------------------------------------------------------
                        '.Item(i).KiteiVal = CDbl(WorksD.SunpoSetL(i).KiteiVal).ToString("N1")  '規定値
                        .Item(i).KiteiVal = CDbl(WorksD.SunpoSetL(i).KiteiVal).ToString("N2")   '規定値
                        'Rep By Yamada 20140919 End ------------------------------------------------------

                        .Item(i).KiteiMin = CDbl(WorksD.SunpoSetL(i).KiteiMin)  '最小許容値
                        .Item(i).KiteiMax = CDbl(WorksD.SunpoSetL(i).KiteiMax)  '最大許容値

                        .Item(i).Targettype = WorksD.SunpoSetL(i).Targettype
                        '.Item(i).flgOutZu = WorksD.SunpoSetL(i).flgOutZu
                        '.Item(i).ZU_layer = WorksD.SunpoSetL(i).ZU_layer
                        '.Item(i).ZU_colorID = WorksD.SunpoSetL(i).ZU_colorID
                        '.Item(i).ZU_LineTypeID = WorksD.SunpoSetL(i).ZU_LineTypeID

                        '20160904 byambaa(SUSANO) add start
                        '出図フラグ
                        If WorksD.SunpoSetL(i).flgOutZu = "1" Then
                            .Item(i).flgOutZu = True
                        Else
                            .Item(i).flgOutZu = False
                        End If

                        ncolor = 0
                        YCM_ReadSystemColorAcs(m_strDataSystemPath)
                        Dim mcolor1 As ModelColor
                        If ncolor > 0 Then
                            If WorksD.SunpoSetL(i).ZU_colorID = "" Or WorksD.SunpoSetL(i).ZU_colorID = "0" Then
                                .Item(i).ZU_color1 = "MAGENTA"
                            Else
                                mcolor1 = YCM_GetColorInfoByCode(WorksD.SunpoSetL(i).ZU_colorID)
                                .Item(i).ZU_color1 = mcolor1.strName
                            End If
                        End If

                        nLineType = 0
                        YCM_ReadSystemLineTypes(m_strDataSystemPath)
                        Dim mLinetype1 As LineType
                        If nLineType > 0 Then
                            If WorksD.SunpoSetL(i).ZU_LineTypeID = "" Or WorksD.SunpoSetL(i).ZU_LineTypeID = "0" Then
                                .Item(i).ZU_LineType1 = "CONTINUOUS"
                            Else
                                mLinetype1 = YCM_GetLineTypeInfoByCode(WorksD.SunpoSetL(i).ZU_LineTypeID)
                                .Item(i).ZU_LineType1 = mLinetype1.strName
                            End If
                        End If

                        'レイヤ表示
                        .Item(i).ZU_layer = WorksD.SunpoSetL(i).ZU_layer

                        '出図フラグ
                        If WorksD.SunpoSetL(i).sek_flgOutZu = "1" Then
                            .Item(i).sek_flgOutZu = True
                        Else
                            .Item(i).sek_flgOutZu = False
                        End If

                        ncolor = 0
                        YCM_ReadSystemColorAcs(m_strDataSystemPath)
                        Dim mcolor2 As ModelColor
                        If ncolor > 0 Then
                            If WorksD.SunpoSetL(i).sek_ZU_colorID = "" Or WorksD.SunpoSetL(i).sek_ZU_colorID = "0" Then
                                .Item(i).sek_ZU_color1 = "MAGENTA"
                            Else
                                mcolor2 = YCM_GetColorInfoByCode(WorksD.SunpoSetL(i).sek_ZU_colorID)
                                .Item(i).sek_ZU_color1 = mcolor2.strName
                            End If
                        End If

                        nLineType = 0
                        YCM_ReadSystemLineTypes(m_strDataSystemPath)
                        Dim mLinetype2 As LineType
                        If nLineType > 0 Then
                            If WorksD.SunpoSetL(i).sek_ZU_LineTypeID = "" Or WorksD.SunpoSetL(i).sek_ZU_LineTypeID = "0" Then
                                .Item(i).sek_ZU_LineType1 = "CONTINUOUS"
                            Else
                                mLinetype2 = YCM_GetLineTypeInfoByCode(WorksD.SunpoSetL(i).sek_ZU_LineTypeID)
                                .Item(i).sek_ZU_LineType1 = mLinetype2.strName
                            End If
                        End If

                        'レイヤ表示
                        .Item(i).sek_ZU_layer = WorksD.SunpoSetL(i).sek_ZU_layer
                        '20160904 byambaa(SUSANO) add end

                    Catch ex As Exception
                        MsgBox("値設定エラー", MsgBoxStyle.OkOnly)
                        Exit Sub
                    End Try
                End If

            Next

        End With
        With m_KiteitiItems
            .Clear()
            For i As Integer = 0 To items.Count - 1
                If items(i).No <> 0 Then
                    .Add(items(i))
                End If
            Next
        End With

        'BYAMBAA ADD 20160904
        If ConnectSystemDB(m_system_dbclass) = False Then
            MsgBox(err_msg_systemdb_connect, MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        Dim TypeInfoD As New TypeInfoTable
        TypeInfoD.m_dbClass = m_system_dbclass
        TypeInfoDL = TypeInfoD.GetFlgSekkeiFromData
        If TypeInfoDL Is Nothing Then
            DisConnectDB(m_system_dbclass)
            Exit Sub
        End If

        If TypeInfoDL.Item(0).flg_sekkei = "0" Then
            DataGridView6.Columns(15).Visibility = System.Windows.Visibility.Hidden
            DataGridView6.Columns(14).Visibility = System.Windows.Visibility.Hidden
            DataGridView6.Columns(13).Visibility = System.Windows.Visibility.Hidden
            DataGridView6.Columns(12).Visibility = System.Windows.Visibility.Hidden
        End If
        DisConnectDB(m_system_dbclass)
        'BYAMBAA ADD 20160904
    End Sub
    '20160822 Byambaa ADD start
    Private Sub SetValueToComboBox() 'TabKiteitiComboBox
        Dim items As New System.Collections.ObjectModel.ObservableCollection(Of KiteitiItem)
        Dim strSunpoSet As New List(Of String)

        With items
            .Clear()
            For i As Integer = 0 To WorksD.SunpoSetL.Count - 1
                strSunpoSet.Add(WorksD.SunpoSetL(i).MeasurementSet)
            Next
            ComboBox.Items.Clear()

            For Each SS As String In strSunpoSet.Distinct.ToList()
                ComboBox.Items.Add(SS)
            Next
        End With
    End Sub

    Private Sub SetValueToComboBox1() 'TabKakuninComboBox
        Dim strSunpoSet As New List(Of String)

        With m_KakuninItems
            .Clear()
            For i As Integer = 0 To WorksD.SunpoSetL.Count - 1
                strSunpoSet.Add(WorksD.SunpoSetL(i).MeasurementSet)
            Next
            ComboBox1.Items.Clear()
            For Each SS As String In strSunpoSet.Distinct.ToList()
                ComboBox1.Items.Add(SS)
            Next
        End With

    End Sub
    '20160822 Byambaa ADD end

    ''' 

    ''' データを取得してグリッドに表示
    ''' 2013/05/15 作成
    ''' 

    ''' 
    Private Sub SetDataKitei()

        'データ取得

        Dim SSD As New SunpoSetTable
        SSD.m_dbClass = m_keisoku_dbclass
        WorksD.SunpoSetL = SSD.GetDataToList()
        If WorksD.SunpoSetL Is Nothing Then
            '計測データ.mdbを切断する
            DisConnectDB(m_keisoku_dbclass)
            Exit Sub
        End If

        'BYAMBAA ADD 20160904
        If ConnectSystemDB(m_system_dbclass) = False Then
            MsgBox(err_msg_systemdb_connect, MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        Dim TypeInfoD As New TypeInfoTable
        TypeInfoD.m_dbClass = m_system_dbclass
        TypeInfoDL = TypeInfoD.GetFlgSekkeiFromData
        If TypeInfoDL Is Nothing Then
            DisConnectDB(m_system_dbclass)
            Exit Sub
        End If

        If TypeInfoDL.Item(0).flg_sekkei = "0" Then
            DataGridView6.Columns(15).Visibility = System.Windows.Visibility.Hidden '20161202 baluu edit
            DataGridView6.Columns(14).Visibility = System.Windows.Visibility.Hidden '20161202 baluu edit
            DataGridView6.Columns(13).Visibility = System.Windows.Visibility.Hidden '20161202 baluu edit
            DataGridView6.Columns(12).Visibility = System.Windows.Visibility.Hidden '20161202 baluu edit
        End If
        DisConnectDB(m_system_dbclass)
        'BYAMBAA ADD 20160904

        Dim iRowNo As Integer = 0

        'データ表示
        Dim items As New System.Collections.ObjectModel.ObservableCollection(Of KiteitiItem)
        With items
            .Clear()
            'If WorksD.SunpoSetL.Count < 20 Then
            '    For i As Integer = 1 To 20
            '        .Add(New KiteitiItem)
            '    Next
            'Else
            '    For i As Integer = 1 To WorksD.SunpoSetL.Count - 1
            '        .Add(New KiteitiItem)
            '    Next
            'End If
            For i As Integer = 0 To WorksD.SunpoSetL.Count - 1
                .Add(New KiteitiItem)
                iRowNo = i + 1
                Try
                    .Item(i).No = iRowNo                                'No
                    .Item(i).MeasurementSet = WorksD.SunpoSetL(i).MeasurementSet '計測セット　(20160822 byambaa ADD)
                    .Item(i).Bui = WorksD.SunpoSetL(i).SunpoMark         '部位

                    .Item(i).KensaItem = WorksD.SunpoSetL(i).SunpoName         '検査項目
                    'Rep By Yamada 20150303 Sta --------
                    '.Item(i).KiteiVal = CDbl(WorksD.SunpoSetL(i).KiteiVal)    '規定値
                    .Item(i).KiteiVal = CDbl(WorksD.SunpoSetL(i).KiteiVal).ToString("N2")    '規定値
                    'Rep By Yamada 20150303 End --------
                    .Item(i).KiteiMin = CDbl(WorksD.SunpoSetL(i).KiteiMin)   '最小許容値
                    .Item(i).KiteiMax = CDbl(WorksD.SunpoSetL(i).KiteiMax)   '最大許容値
                    .Item(i).Targettype = WorksD.SunpoSetL(i).Targettype

                    '20160904 byambaa(SUSANO) add start
                    '出図フラグ
                    If WorksD.SunpoSetL(i).flgOutZu = "1" Then
                        .Item(i).flgOutZu = True
                    Else
                        .Item(i).flgOutZu = False
                    End If

                    ncolor = 0
                    YCM_ReadSystemColorAcs(m_strDataSystemPath)
                    Dim mcolor1 As ModelColor
                    If ncolor > 0 Then
                        If WorksD.SunpoSetL(i).ZU_colorID = "" Or WorksD.SunpoSetL(i).ZU_colorID = "0" Then
                            .Item(i).ZU_color1 = "MAGENTA"
                        Else
                            mcolor1 = YCM_GetColorInfoByCode(WorksD.SunpoSetL(i).ZU_colorID)
                            .Item(i).ZU_color1 = mcolor1.strName
                        End If
                    End If

                    nLineType = 0
                    YCM_ReadSystemLineTypes(m_strDataSystemPath)
                    Dim mLinetype1 As LineType
                    If nLineType > 0 Then
                        If WorksD.SunpoSetL(i).ZU_LineTypeID = "" Or WorksD.SunpoSetL(i).ZU_LineTypeID = "0" Then
                            .Item(i).ZU_LineType1 = "CONTINUOUS"
                        Else
                            mLinetype1 = YCM_GetLineTypeInfoByCode(WorksD.SunpoSetL(i).ZU_LineTypeID)
                            .Item(i).ZU_LineType1 = mLinetype1.strName
                        End If
                    End If

                    'レイヤ表示
                    .Item(i).ZU_layer = WorksD.SunpoSetL(i).ZU_layer

                    '出図フラグ
                    If WorksD.SunpoSetL(i).sek_flgOutZu = "1" Then
                        .Item(i).sek_flgOutZu = True
                    Else
                        .Item(i).sek_flgOutZu = False
                    End If

                    ncolor = 0
                    YCM_ReadSystemColorAcs(m_strDataSystemPath)
                    Dim mcolor2 As ModelColor
                    If ncolor > 0 Then
                        If WorksD.SunpoSetL(i).sek_ZU_colorID = "" Or WorksD.SunpoSetL(i).sek_ZU_colorID = "0" Then
                            .Item(i).sek_ZU_color1 = "MAGENTA"
                        Else
                            mcolor2 = YCM_GetColorInfoByCode(WorksD.SunpoSetL(i).sek_ZU_colorID)
                            .Item(i).sek_ZU_color1 = mcolor2.strName
                        End If
                    End If

                    nLineType = 0
                    YCM_ReadSystemLineTypes(m_strDataSystemPath)
                    Dim mLinetype2 As LineType
                    If nLineType > 0 Then
                        If WorksD.SunpoSetL(i).sek_ZU_LineTypeID = "" Or WorksD.SunpoSetL(i).sek_ZU_LineTypeID = "0" Then
                            .Item(i).sek_ZU_LineType1 = "CONTINUOUS"
                        Else
                            mLinetype2 = YCM_GetLineTypeInfoByCode(WorksD.SunpoSetL(i).sek_ZU_LineTypeID)
                            .Item(i).sek_ZU_LineType1 = mLinetype2.strName
                        End If
                    End If

                    'レイヤ表示
                    .Item(i).sek_ZU_layer = WorksD.SunpoSetL(i).sek_ZU_layer
                    '20160904 byambaa(SUSANO) add end 

                Catch ex As Exception
                    MsgBox("値設定エラー", MsgBoxStyle.OkOnly)
                    Exit Sub
                End Try

            Next

        End With

        With m_KiteitiItems
            .Clear()
            For i As Integer = 0 To items.Count - 1
                .Add(items(i))
            Next
        End With

        '計測データ.mdbを切断する
        DisConnectDB(m_keisoku_dbclass)

    End Sub

    ''' 

    ''' 2013/05/21 作成
    ''' 

    ''' 
    'Private Sub SetTLP1()

    '    '挿入位置
    '    Dim insertRow As Integer = 0
    '    Dim insertColumn As Integer = 0

    '    Dim iCols As Integer = 0
    '    Dim iRows As Integer = 0

    '    Dim iTabIndex As Long = 0

    '    Dim TypeInfoD As New TypeInfoTable
    '    TypeInfoD.m_dbClass = m_system_dbclass
    '    TypeInfoDL = TypeInfoD.GetDataToList
    '    If TypeInfoDL Is Nothing Then
    '        Exit Sub
    '    End If

    '    With TLP1

    '        .Controls.Clear()

    '        .RowCount = 0

    '        '.ColumnCount = 2
    '        .ColumnCount = 1

    '        .CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetPartial

    '        .Visible = True
    '        .Location = New System.Drawing.Point(23, 50)

    '        '.Size = New System.Drawing.Size(400, 145)

    '        '.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 15))
    '        '.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 85))

    '        .GrowStyle = TableLayoutPanelGrowStyle.AddRows

    '        For iRows = 0 To TypeInfoDL.Count - 1

    '            'ラジオボタン
    '            Dim newRadio As New RadioButton
    '            newRadio.Size = New Size(GroupBox1.Size.Width - 15, newRadio.Size.Height)
    '            newRadio.Text = TypeInfoDL(iRows).type_name.Trim
    '            newRadio.Name = "R" & iRows
    '            newRadio.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
    '            If iRows = 0 Then
    '                newRadio.Checked = True
    '            End If

    '            'newRadio.Anchor = AnchorStyles.Left
    '            .Controls.Add(newRadio, 0, iRows)
    '            .Controls(.Controls.Count - 1).Anchor = AnchorStyles.Left
    '            '動的にラジオボタンイベントを関連付ける

    '            AddHandler newRadio.Click, AddressOf rbtn_Check
    '            'AddHandler newRadio.MouseMove, AddressOf rbtn_MouseMove

    '            '''Type名

    '            ''Dim newLabel As New Label
    '            ''newLabel.Text = TypeInfoDL(iRows).type_name.Trim
    '            ''newLabel.Size = New Size(350, 28)
    '            ''newLabel.Name = "T" & iRows
    '            ''newLabel.TabIndex = iTabIndex
    '            ''newLabel.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
    '            '''newLabel.Anchor = AnchorStyles.Left
    '            ''Type名

    '            'Dim newLabel As New TextBox
    '            'newLabel.Text = TypeInfoDL(iRows).type_name.Trim
    '            'newLabel.Size = New Size(350, 28)
    '            'newLabel.Name = "T" & iRows
    '            'newLabel.TabIndex = iTabIndex
    '            'newLabel.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
    '            'newLabel.TabStop = False
    '            'newLabel.ReadOnly = True
    '            'newLabel.BorderStyle = BorderStyle.None
    '            'newLabel.Anchor = AnchorStyles.Left

    '            'iTabIndex += 1
    '            '.Controls.Add(newLabel, 1, iRows)
    '            '.Controls(.Controls.Count - 1).Dock = DockStyle.Fill
    '            '.Controls(.Controls.Count - 1).Anchor = AnchorStyles.Left

    '        Next

    '        .RowStyles.Item(0).SizeType = SizeType.AutoSize

    '    End With

    '    'Me.GroupBox1.Size = New Size(500, (55 + ((TypeInfoDL.Count - 1) * 32)))

    '    Dim iHeight As Integer = (55 + ((TypeInfoDL.Count - 1) * 32))
    '    With GroupBox1
    '        If iHeight < 375 Then
    '            .Size = New Size(392, iHeight)
    '            TLP1.AutoScroll = False
    '            Panel1.AutoScroll = False
    '        Else
    '            .Size = New Size(392, 384)
    '            TLP1.AutoScroll = True
    '            Panel1.AutoScroll = True
    '        End If
    '    End With
    '    TLP1.Refresh()
    '    Me.Refresh()

    'End Sub

    ''' 

    ''' TableLayoutPanelのRadioButtonチェックイベント

    ''' 2013/05/27 作成
    ''' 

    ''' 
    Private Sub rbtn_Check(ByVal sender As Object, ByVal e As System.EventArgs)

        'Dim irbtn As RadioButton = sender
        ''PictureBox1.ImageLocation = m_TemplatePath & "\" & TypeInfoDL(CInt(irbtn.Name.Replace("R", ""))).picture_name
        'If TypeInfoDL(CInt(irbtn.Name.Replace("R", ""))).picture_name <> "" Then
        '    PictureBox1.ImageLocation = m_TemplatePath & "\" & TypeInfoDL(CInt(irbtn.Name.Replace("R", ""))).picture_name
        'Else
        '    PictureBox1.ImageLocation = ""
        'End If

    End Sub

    ''' 

    ''' TableLayoutPanelのRadioButtonマウスムーブイベント

    ''' 2013/05/28 作成
    ''' 

    ''' 
    Private Sub rbtn_MouseMove(ByVal sender As Object, ByVal e As System.EventArgs)

        'Dim irbtn As RadioButton = sender
        ''PictureBox1.ImageLocation = m_TemplatePath & "\" & TypeInfoDL(CInt(irbtn.Name.Replace("R", ""))).picture_name
        'If TypeInfoDL(CInt(irbtn.Name.Replace("R", ""))).picture_name <> "" Then
        '    PictureBox1.ImageLocation = m_TemplatePath & "\" & TypeInfoDL(CInt(irbtn.Name.Replace("R", ""))).picture_name
        'Else
        '    PictureBox1.ImageLocation = ""
        'End If

    End Sub

    '''' 

    '''' 2013/05/13 作成
    '''' 

    '''' 
    'Private Sub SetTLP4()

    '    '挿入位置
    '    Dim insertRow As Integer = 0
    '    Dim insertColumn As Integer = 0

    '    Dim iCols As Integer = 0
    '    Dim iRows As Integer = 0

    '    'If Me.Tag Is Nothing Then
    '    '    '
    '    WorksD = New WorksTable
    '    WorksD.m_dbClass = m_system_dbclass
    '    WorksD.GetDataToList()
    '    If WorksD.KihonL Is Nothing Then
    '        Exit Sub
    '    End If
    '    'Else
    '    '    '読込から
    '    '    WorksD = New WorksTable
    '    '    WorksD.m_dbClass = m_system_dbclass
    '    '    WorksD.copy(CType(Me.Tag, WorksTable))
    '    'End If

    '    With TLP4

    '        .Controls.Clear()

    '        .RowCount = 0

    '        .ColumnCount = 2

    '        .CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetPartial

    '        .Visible = True
    '        .Location = New System.Drawing.Point(23, 50)
    '        '.Size = New System.Drawing.Size(200, 200)

    '        '.Size = New System.Drawing.Size(500, 120)

    '        .ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25))
    '        .ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 75))

    '        '.ColumnCount = 2

    '        .GrowStyle = TableLayoutPanelGrowStyle.AddRows

    '        Dim iTabIndex As Long = 0

    '        For iRows = 0 To WorksD.KihonL.Count - 1

    '            '項目名

    '            Dim newLabel As New Label
    '            newLabel.Text = WorksD.KihonL(iRows).item_name
    '            .Controls.Add(newLabel, insertColumn, iRows)
    '            .Controls(.Controls.Count - 1).Dock = DockStyle.Fill
    '            .Controls(.Controls.Count - 1).Anchor = AnchorStyles.Left

    '            '項目タイプ

    '            Select Case WorksD.KihonL(iRows).value_type
    '                Case "TEXT"
    '                    Dim newTextBox As New TextBox()
    '                    newTextBox.Text = WorksD.KihonL(iRows).item_value
    '                    newTextBox.Size = New Size(200, 28)
    '                    newTextBox.Name = WorksD.KihonL(iRows).item_cell_name
    '                    newTextBox.TabIndex = iTabIndex
    '                    iTabIndex += 1
    '                    .Controls.Add(newTextBox, insertColumn + 1, iRows)
    '                    .Controls(.Controls.Count - 1).Anchor = AnchorStyles.Left
    '                    Dim objS1 As New Object
    '                    objS1 = newTextBox
    '                    WorksD.KihonL(iRows).objControl = objS1
    '                    WorksD.KihonL(iRows).SetDefaultValueToControl()
    '                    Exit Select
    '                Case "COMBOBOX"
    '                    Dim newCombobox As New ComboBox
    '                    newCombobox.Size = New Size(150, 28)
    '                    newCombobox.Name = WorksD.KihonL(iRows).item_cell_name
    '                    newCombobox.TabIndex = iTabIndex
    '                    iTabIndex += 1
    '                    For Each CTEXT As String In WorksD.KihonL(iRows).item_value.Split("|")
    '                        newCombobox.Items.Add(CTEXT)
    '                    Next
    '                    .Controls.Add(newCombobox, insertColumn + 1, iRows)
    '                    .Controls(.Controls.Count - 1).Anchor = AnchorStyles.Left
    '                    Dim objS1 As New Object
    '                    objS1 = newCombobox
    '                    WorksD.KihonL(iRows).objControl = objS1
    '                    WorksD.KihonL(iRows).SetDefaultValueToControl()
    '                    Exit Select
    '                Case "DATETIME"
    '                    Dim newDateTimePicker As New DateTimePicker
    '                    newDateTimePicker.Size = New Size(210, 28)
    '                    newDateTimePicker.Name = WorksD.KihonL(iRows).item_cell_name
    '                    newDateTimePicker.TabIndex = iTabIndex
    '                    iTabIndex += 1
    '                    If IsDate(WorksD.KihonL(iRows).item_value) = True Then
    '                        newDateTimePicker.Value = CDate(WorksD.KihonL(iRows).item_value)
    '                    End If
    '                    .Controls.Add(newDateTimePicker, insertColumn + 1, iRows)
    '                    .Controls(.Controls.Count - 1).Anchor = AnchorStyles.Left
    '                    Dim objS1 As New Object
    '                    objS1 = newDateTimePicker
    '                    WorksD.KihonL(iRows).objControl = objS1
    '                    WorksD.KihonL(iRows).SetDefaultValueToControl()
    '                    Exit Select
    '            End Select

    '        Next

    '        .RowStyles.Item(0).SizeType = SizeType.AutoSize

    '    End With

    '    ''Me.GroupBox6.Size = New Size(500, TLP4.Height + 10)
    '    ''Me.GroupBox6.Size = New Size(500, (WorksD.KihonL.Count * 30) + 13)
    '    ''Me.GroupBox6.Size = New Size(500, (WorksD.KihonL.Count * 28) + (WorksD.KihonL.Count + 1) + 8)
    '    'Me.GroupBox6.Size = New Size(500, (WorksD.KihonL.Count * 28) + (WorksD.KihonL.Count + 1) + 8)
    '    ''Me.GroupBox6.Size = New Size(500, TLP4.Controls(0).Size.Height * WorksD.KihonL.Count)
    '    ''Me.Panel9.Size = New Size(500, (TLP4.Size.Height + 8))

    '    Me.GroupBox6.Size = New Size(500, (WorksD.KihonL.Count * 30))
    '    Me.Refresh()

    '    '他工事データからの参照
    '    SetKoujiSansyou()

    'End Sub

    ''' 

    ''' 2013/05/13 作成
    ''' 

    ''' 
    Private Sub SetTLP4()

        '挿入位置
        Dim insertRow As Integer = 0
        Dim insertColumn As Integer = 0

        Dim iCols As Integer = 0
        Dim iRows As Integer = 0

        Dim iTabIndex As Long = 2

        With TLP4

            .Children.Clear()

            .RowDefinitions.Clear()

            '.ColumnCount = 2

            '.CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetPartial

            .Visibility = System.Windows.Visibility.Visible
            '.Location = New System.Drawing.Point(23, 50)
            '.Size = New System.Drawing.Size(200, 200)

            '.Size = New System.Drawing.Size(500, 120)

            '.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25))
            '.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 75))

            '.GrowStyle = TableLayoutPanelGrowStyle.AddRows

            For iRows = 0 To WorksD.KihonL.Count - 1
                Dim newRow As New System.Windows.Controls.RowDefinition
                newRow.Height = New System.Windows.GridLength(38)
                .RowDefinitions.Add(newRow)

                '項目名

                Dim newLabel As New System.Windows.Controls.Label
                newLabel.Content = WorksD.KihonL(iRows).item_name
                newLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left
                .SetRow(newLabel, iRows)
                .SetColumn(newLabel, insertColumn)
                .Children.Add(newLabel)
                '.Controls(.Controls.Count - 1).Dock = DockStyle.Fill
                '.Controls(.Controls.Count - 1).Anchor = AnchorStyles.Left

                '項目タイプ

                Select Case WorksD.KihonL(iRows).value_type
                    Case "TEXT"
                        Dim newTextBox As New System.Windows.Controls.TextBox()
                        newTextBox.Text = WorksD.KihonL(iRows).item_value
                        newTextBox.Width = 200
                        newTextBox.Height = 29
                        newTextBox.FontSize = 16
                        newTextBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left
                        newTextBox.Name = WorksD.KihonL(iRows).item_cell_name
                        newTextBox.TabIndex = iTabIndex
                        iTabIndex += 1
                        .SetRow(newTextBox, iRows)
                        .SetColumn(newTextBox, insertColumn + 1)
                        .Children.Add(newTextBox)
                        Dim objS1 As New Object
                        objS1 = newTextBox
                        WorksD.KihonL(iRows).objControl = objS1
                        WorksD.KihonL(iRows).SetDefaultValueToControl()
                        Exit Select
                    Case "COMBOBOX"
                        Dim newCombobox As New System.Windows.Controls.ComboBox
                        newCombobox.Width = 150
                        newCombobox.Height = 29
                        newCombobox.FontSize = 16
                        newCombobox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left
                        newCombobox.Name = WorksD.KihonL(iRows).item_cell_name
                        newCombobox.TabIndex = iTabIndex
                        iTabIndex += 1
                        For Each CSUBD As SubInfoTable In WorksD.KihonL(iRows).SubInfoL
                            newCombobox.Items.Add(CSUBD.ItemValue)
                        Next
                        .SetRow(newCombobox, iRows)
                        .SetColumn(newCombobox, insertColumn + 1)
                        .Children.Add(newCombobox)
                        Dim objS1 As New Object
                        objS1 = newCombobox
                        WorksD.KihonL(iRows).objControl = objS1
                        WorksD.KihonL(iRows).SetDefaultValueToControl()
                        Exit Select
                    Case "DATETIME"
                        Dim newDateTimePicker As New System.Windows.Controls.DatePicker
                        newDateTimePicker.Width = 210
                        newDateTimePicker.Height = 29
                        newDateTimePicker.FontSize = 16
                        newDateTimePicker.HorizontalAlignment = System.Windows.HorizontalAlignment.Left
                        newDateTimePicker.Name = WorksD.KihonL(iRows).item_cell_name
                        newDateTimePicker.TabIndex = iTabIndex
                        iTabIndex += 1
                        If IsDate(WorksD.KihonL(iRows).item_value) = True Then
                            newDateTimePicker.SelectedDate = CDate(WorksD.KihonL(iRows).item_value)
                        End If
                        .SetRow(newDateTimePicker, iRows)
                        .SetColumn(newDateTimePicker, insertColumn + 1)
                        .Children.Add(newDateTimePicker)
                        Dim objS1 As New Object
                        objS1 = newDateTimePicker
                        WorksD.KihonL(iRows).objControl = objS1
                        WorksD.KihonL(iRows).SetDefaultValueToControl()
                        Exit Select
                End Select

            Next

            '.RowStyles.Item(0).SizeType = SizeType.AutoSize

        End With

        'タブインデックスの設定

        TxtKojiData.TabIndex = 0
        BtnKojiData.TabIndex = 1
        BtnT2KojiSansyou.TabIndex = iTabIndex
        iTabIndex += 1
        BtnT2Back.TabIndex = iTabIndex
        iTabIndex += 1
        BtnT2Next.TabIndex = iTabIndex

        Me.GroupBox6.FontSize = 16
        Me.GroupBox6.Width = 500
        Me.GroupBox6.Height = 65 + ((WorksD.KihonL.Count - 1) * 38)

        'Me.Refresh()

    End Sub

    ''' 

    ''' 読み込んできたデータを表示する
    ''' 2013/05/15 作成
    ''' 

    ''' 
    Private Sub SetYomikomiData()

        Dim WorksD1 As New WorksTable
        With WorksD1
            .m_dbClass = m_system_dbclass
            .copy(CType(Me.Tag, WorksTable))

            For i As Integer = 0 To .KihonL.Count - 1

                For ii As Integer = 0 To TLP4.Children.Count - 1
                    Dim control As System.Windows.Controls.Control = TLP4.Children(ii)
                    If .KihonL(i).item_cell_name = control.Name Then

                        .KihonL(i).SetItemValueToControl()
                        Exit For
                    End If

                Next

            Next

        End With

    End Sub

    ''' 

    ''' 読み込んできたデータを表示する
    ''' 2013/05/15 作成
    ''' 

    ''' 
    Private Sub SetYomikomiData(ByRef WorksD1 As WorksTable)

        With WorksD1
            .m_dbClass = m_system_dbclass
            .copy(CType(Me.Tag, WorksTable))

            For i As Integer = 0 To .KihonL.Count - 1

                For ii As Integer = 0 To TLP4.Children.Count - 1

                    If .KihonL(i).item_cell_name = CType(TLP4.Children(ii), System.Windows.Controls.Control).Name Then

                        .KihonL(i).SetItemValueToControl()
                        Exit For
                    End If

                Next

            Next

        End With

    End Sub

    '''' 

    '''' 次へボタン
    '''' 2013/05/13 作成
    '''' 

    '''' 
    'Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    '    Dim strdatabasefile As String = ""
    '    m_strDataBasePath = ""
    '    m_strDataBasePath = ComSel_SelectFolderByShell("工事データフォルダを指定して下さい。", True)
    '    If m_strDataBasePath <> Nothing Then
    '        If System.IO.File.Exists(m_strDataBasePath & "\" & m_Keisoku_mdb_name) = False Then
    '            '計測データ.mdbが無い場合、コピーする
    '            System.IO.File.Copy(m_KeisokuTempMdbPath, m_strDataBasePath & "\" & m_Keisoku_mdb_name)
    '        Else
    '            If MsgBox("既に計測データが存在します。" & vbCrLf & "上書きしますか？", MsgBoxStyle.YesNo, "確認") = MsgBoxResult.Yes Then
    '                System.IO.File.Delete(m_strDataBasePath & "\" & m_Keisoku_mdb_name)
    '                System.IO.File.Copy(m_KeisokuTempMdbPath, m_strDataBasePath & "\" & m_Keisoku_mdb_name)
    '            End If
    '        End If

    '        '規定値入力

    '        'コピーした計測データ.mdbに寸法算出テーブルの規定値・最小許容値・最大許容値を設定する

    '        m_Keisoku_mdb_path = m_strDataBasePath & "\" & m_Keisoku_mdb_name
    '        ' KiteitiInput.ShowDialog()

    '        '画面の入力値を取得する

    '        GetDataFromToControl()

    '        '計測データ.mdbに入力値を書き込む
    '        'KihonInfoの更新
    '        If SetKeisokuData(m_strDataBasePath & "\" & m_Keisoku_mdb_name) = False Then
    '            DisConnectDB(m_keisoku_dbclass)
    '            MsgBox("計測データ更新に失敗しました。", MsgBoxStyle.OkOnly, "確認")
    '            Exit Sub
    '        End If

    '        'システム設定.mdbを更新する
    '        'Works, WorksInfoの更新
    '        If SetSystemData(m_SystemMdbFullPath, m_strDataBasePath) = False Then
    '            MsgBox("システム設定データ更新に失敗しました。", MsgBoxStyle.OkOnly, "確認")
    '            Exit Sub
    '        End If
    '    Else
    '        Exit Sub
    '    End If

    '    '計測データ.mdbを切断する
    '    DisConnectDB(m_keisoku_dbclass)
    '    'システム設定.mdbを切断する
    '    DisConnectDB(m_system_dbclass)
    '    'Me.Dispose()
    '    'Me.Close()

    '    '初期設定

    '    If SetInitFormKitei() = False Then
    '        Exit Sub
    '    End If

    '    'グリッドヘッダ設定

    '    SetGridHeaddderKitei()

    '    'データを取得してグリッドに表示
    '    SetDataKitei()

    'End Sub

    '''' 

    '''' 次へボタン
    '''' 2013/05/16 作成
    '''' 

    '''' 
    'Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    '    If CType(sender, Button).Name = "Button5" Then
    '        RadioButton1.Checked = True
    '    End If

    '    'Me.TabControl1.SelectedIndex += 1

    'End Sub

    '''' 

    '''' 戻るボタン
    '''' 2013/05/16 作成
    '''' 

    '''' 
    'Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    '    'Me.TabControl1.SelectedIndex -= 1

    'End Sub

    ''' 

    ''' 計測データ.mdbに入力値を書き込む
    ''' 2013/05/14 作成
    ''' 

    ''' 
    Private Function SetKeisokuData(ByVal strPath As String) As Boolean

        ''計測データDBに接続する

        'If ConnectDB(strPath, m_keisoku_dbclass) = False Then
        '    MsgBox("計測データ.mdbを開くことができません。")
        '    SetKeisokuData = False
        '    Exit Function
        'End If

        Dim bRet As Boolean = True

        m_keisoku_dbclass.BeginTrans()
        With WorksD

            For i As Long = 0 To .KihonL.Count - 1

                .KihonL(i).m_dbClass = m_keisoku_dbclass
                .KihonL(i).update_date = Now
                If .KihonL(i).SaveData(False) = False Then
                    bRet = False
                    Exit For
                End If

            Next

        End With

        If bRet = True Then
            m_keisoku_dbclass.CommitTrans()
        Else
            m_keisoku_dbclass.RollbackTrans()
        End If

        SetKeisokuData = bRet

    End Function

    ''' 

    ''' 計測データ.mdbに基本情報を書き込む
    ''' システム設定mdbのKihonInfoテーブルを計測データmdbに書き込む
    ''' 2013/05/23 作成
    ''' 

    ''' 
    Private Function SetKeisokuData(ByVal strPath As String, ByVal strKeisokuPath As String) As Boolean

        SetKeisokuData = True

        With WorksD

            'KihonInfoテーブルに書き込む
            m_keisoku_dbclass.BeginTrans()

            Dim KHD As New KihonInfoTable
            KHD.m_dbClass = m_keisoku_dbclass
            For i As Integer = 0 To .KihonL.Count - 1
                KHD.copy(.KihonL(i))
                If KHD.SaveData(False) = False Then
                    SetKeisokuData = False
                    m_keisoku_dbclass.RollbackTrans()
                    Exit Function
                End If
            Next


            m_keisoku_dbclass.CommitTrans()

        End With

    End Function

    ''' 

    ''' システム設定.mdbに入力値を書き込む
    ''' Works, WorksInfoテーブルを書き込む
    ''' 2013/05/14 作成
    ''' 

    ''' 
    Private Function SetSystemData(ByVal strPath As String, ByVal strKeisokuPath As String) As Boolean

        SetSystemData = True

        m_system_dbclass.BeginTrans()

        Dim bRet As Boolean = True

        With WorksD

            'Worksテーブルに書き込む
            .SavePath = strKeisokuPath
            .CreateDate = Now
            .MeasureDate = ""
            .TypeID = CommonTypeID
            .m_dbClass = m_system_dbclass
            'Worksテーブルの更新
            If .SaveData(False) = False Then
                SetSystemData = False
                m_system_dbclass.RollbackTrans()
                Exit Function
            End If

            'WorksKihonテーブルに書き込む
            For i As Long = 0 To .KihonL.Count - 1

                Dim WKID As New WorksKihonTable
                WKID.m_dbClass = m_system_dbclass
                WKID.Work_ID = .ID                          'Works.ID
                WKID.KI_ID = .KihonL(i).ID                  'KihonInfo.ID
                WKID.ItemValue = .KihonL(i).item_value      'KihonInfo.ItemVal
                'WorksKihonテーブルの更新
                If WKID.SaveData(False) = False Then
                    SetSystemData = False
                    m_system_dbclass.RollbackTrans()
                    Exit Function
                End If

            Next

        End With

        m_system_dbclass.CommitTrans()

        SetSystemData = bRet

    End Function

    '''' 

    '''' 入力値を書き込む
    '''' システム設定.mdbに書き込む
    '''' 2013/05/13 作成
    '''' 

    '''' 
    'Private Function SetInputData() As Boolean

    '    Dim bRet As Boolean = True

    '    '画面の入力値を取得する

    '    GetDataFromToControl()



    '    SaveKeisokuData()

    '    '項目値を更新する
    '    bRet = SaveSystemData()

    'End Function

    ''' 

    ''' 画面の項目を取得する

    ''' 2013/05/13 作成
    ''' 

    ''' 
    Private Sub GetDataFromToControl()

        For Each III As KihonInfoTable In WorksD.KihonL
            III.GetDataFromControl()
        Next

        'With KIO

        '    For i As Long = 0 To .iControls.Count - 1

        '        Try
        '            If KInfoD.KihonL(i).item_value <> .iControls(i).text.trim Then
        '                KInfoD.KihonL(i).flgSyusei = True       '修正フラグ true
        '                KInfoD.KihonL(i).update_date = Now      '更新日
        '            Else
        '                KInfoD.KihonL(i).flgSyusei = False      '修正フラグ false
        '            End If
        '            KInfoD.KihonL(i).item_value = .iControls(i).text.trim
        '        Catch ex As Exception
        '            If KInfoD.KihonL(i).item_value <> .iControls(i).value.trim Then
        '                KInfoD.KihonL(i).flgSyusei = True       '修正フラグ true
        '                KInfoD.KihonL(i).update_date = Now      '更新日
        '            Else
        '                KInfoD.KihonL(i).flgSyusei = False      '修正フラグ false
        '            End If
        '            KInfoD.KihonL(i).item_value = .iControls(i).value
        '        End Try
        '    Next

        'End With

    End Sub

    ''' 

    ''' 計測データ.mdbを更新する
    ''' 2013/05/14 作成
    ''' 

    ''' 
    Private Function SaveKeisokuData() As Boolean

        Dim bRet As Boolean = True

        With WorksD

            For i As Long = 0 To .KihonL.Count - 1

                If .KihonL(i).flgSyusei = True Then
                    If .KihonL(i).SaveData() = False Then
                        bRet = False
                        Exit For
                    End If
                End If

            Next

        End With

        SaveKeisokuData = bRet

    End Function

    ''' 

    ''' システム設定データ.mdbを更新する
    ''' 2013/05/14 作成
    ''' 

    ''' 
    Private Function SaveSystemData() As Boolean

        Dim bRet As Boolean = True

        With WorksD

            For i As Long = 0 To .KihonL.Count - 1

                If .KihonL(i).flgSyusei = True Then
                    If .KihonL(i).SaveData() = False Then
                        bRet = False
                        Exit For
                    End If
                End If

            Next

        End With

        SaveSystemData = bRet

    End Function

    Private Sub KentouKihonInfoTabDialog_LocationChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LocationChanged

    End Sub

    'Private Sub KihonInfoTabDialog_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

    '    ''Formが最少化
    '    'If Me.WindowState = FormWindowState.Minimized Then
    '    '    For Each f As Form In My.Application.OpenForms
    '    '        If Me.Name.ToString <> f.Name.ToString Then
    '    '            f.Visible = False
    '    '        End If
    '    '    Next

    '    'End If

    '    'TLP4.SuspendLayout()

    '    'TLP4.ResumeLayout()

    'End Sub

#Region "工事データ一覧画面"

    ''' 

    ''' 工事データ一覧ボタン
    ''' 2013/05/14 作成
    ''' 

    ''' 
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)


        With gKojiDataConf
            .m_SystemMdbPath = m_SystemMdbPath
            .m_SystemMdbFullPath = m_SystemMdbFullPath
            .m_keisoku_dbclass = m_keisoku_dbclass          '計測データ.mdb
            .m_system_dbclass = m_system_dbclass            'システム設定.mdb
            If .WorksD Is Nothing Then
                .WorksD = New WorksTable
            End If
            .WorksD.copy(WorksD)
            .ShowDialog()

            If .Tag Is Nothing Then
            Else
                Me.Tag = .Tag
                SetYomikomiData()
            End If
        End With

    End Sub

#End Region

    Private Sub SaveKitei_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        SaveSunpo()

    End Sub

    ''' 

    ''' 保存処理

    ''' 2013/05/15 作成
    ''' 

    ''' 
    Private Function SaveSunpo() As Boolean

        Dim bRet As Boolean = True

        SaveSunpo = True

        'グリッドから規定値・最小許容値・最大許容値を取得する

        GetDataFromGrid()

        '規定値データ(SunpoSet)更新
        If SaveSunpoData() = False Then
            SaveSunpo = False
        End If

    End Function

    ''' 

    ''' グリッドから規定値・最小許容値・最大許容値を取得

    ''' 2013/05/15 作成
    ''' 

    ''' 
    Private Sub GetDataFromGrid()

        With m_KiteitiItems
            '.Rows.Clear() 'H25.5.25 Yamada
            'H25.5.25 Yamada
            'If WorksD.SunpoSetL.Count < 20 Then
            '    .Rows.Add(20)
            'Else
            '    .Rows.Add(WorksD.SunpoSetL.Count)
            'End If

            'For i As Integer = 0 To WorksD.SunpoSetL.Count - 1
            For i As Integer = 0 To m_KiteitiItems.Count - 1
                '規定値
                'm_KiteitiItems.Item(i).KiteiVal = dblField(.Item(i).KiteiVal)
                ''最小許容値
                'm_KiteitiItems.Item(i).KiteiMin = dblField(.Item(i).KiteiMin)
                ''最大許容値
                'm_KiteitiItems.Item(i).KiteiMax = dblField(.Item(i).KiteiMax)


                If WorksD.SunpoSetL.Count > i Then '20161114 baluu add start
                    '規定値
                    WorksD.SunpoSetL(i).KiteiVal = dblField(.Item(i).KiteiVal)
                    '最小許容値
                    WorksD.SunpoSetL(i).KiteiMin = dblField(.Item(i).KiteiMin)
                    '最大許容値
                    WorksD.SunpoSetL(i).KiteiMax = dblField(.Item(i).KiteiMax)

                    'SUURI DELETE 20140903
                    WorksD.SunpoSetL(i).Targettype = dblField(.Item(i).Targettype)
                    'baluu EDIT 20161110 start
                    If .Item(i).flgOutZu Then
                        WorksD.SunpoSetL(i).flgOutZu = "1"
                    Else
                        WorksD.SunpoSetL(i).flgOutZu = "0"
                    End If
                    'baluu EDIT 20161110 end


                    WorksD.SunpoSetL(i).ZU_layer = .Item(i).ZU_layer

                    Dim mcolor As ModelColor
                    mcolor = YCM_GetColorInfoByName(.Item(i).ZU_color1)
                    WorksD.SunpoSetL(i).ZU_colorID = mcolor.code

                    Dim mLinetype As LineType
                    mLinetype = YCM_GetLineTypeInfoByName(.Item(i).ZU_LineType1)
                    WorksD.SunpoSetL(i).ZU_LineTypeID = mLinetype.code
                    'SUURI DELETE 20140903

                    'BYAMBAA ADD 20160904

                    'baluu EDIT 20161110 start
                    If .Item(i).sek_flgOutZu Then
                        WorksD.SunpoSetL(i).sek_flgOutZu = "1"
                    Else
                        WorksD.SunpoSetL(i).sek_flgOutZu = "0"
                    End If
                    'baluu EDIT 20161110 end

                    WorksD.SunpoSetL(i).sek_ZU_layer = .Item(i).sek_ZU_layer

                    Dim mcolor1 As ModelColor
                    mcolor1 = YCM_GetColorInfoByName(.Item(i).sek_ZU_color1)
                    WorksD.SunpoSetL(i).sek_ZU_colorID = mcolor1.code

                    Dim mLinetype1 As LineType
                    mLinetype1 = YCM_GetLineTypeInfoByName(.Item(i).sek_ZU_LineType1)
                    WorksD.SunpoSetL(i).sek_ZU_LineTypeID = mLinetype1.code
                    'BYAMBAA ADD 20160904
                End If '20161114 baluu add end
            Next

        End With

    End Sub

    '''' 

    '''' 規定値データ(SunpoSet)更新
    '''' 2013/05/15 作成
    '''' 

    '''' 
    'Private Function SaveSunpoData() As Boolean

    '    m_keisoku_dbclass.BeginTrans()

    '    For i As Integer = 0 To WorksD.SunpoSetL.Count - 1

    '        WorksD.SunpoSetL(i).m_dbClass = m_keisoku_dbclass
    '        If WorksD.SunpoSetL(i).SaveData(False) = False Then
    '            m_keisoku_dbclass.RollbackTrans()
    '            SaveSunpoData = False
    '            Exit For
    '        End If

    '    Next

    '    m_keisoku_dbclass.CommitTrans()

    '    SaveSunpoData = True

    'End Function
    Private Function SaveTenGunTeigiData(Optional ByRef flg_connect As Boolean = True) As Boolean

        SaveTenGunTeigiData = True

        If flg_connect = False Then
            '計測データmdbに接続する

            If ConnectDB(m_koji_kanri_path & "\" & m_Keisoku_mdb_name, m_keisoku_dbclass) = False Then

                MsgBox("計測データmdbへの接続に失敗しました。", MsgBoxStyle.OkOnly)
                SaveTenGunTeigiData = False
                Exit Function

            End If
        End If

        m_keisoku_dbclass.BeginTrans()

        For i As Integer = 0 To lstTengun.Count - 1

            lstTengun(i).m_dbClass = m_keisoku_dbclass
            If lstTengun(i).SaveData(False) = False Then
                m_keisoku_dbclass.RollbackTrans()
                SaveTenGunTeigiData = False
                Exit For
            End If

        Next

        m_keisoku_dbclass.CommitTrans()

        If flg_connect = False Then
            '計測データmdbに接続切断する
            DisConnectDB(m_keisoku_dbclass)
        End If

        SaveTenGunTeigiData = True

    End Function
    ''' 

    ''' 規定値データ(SunpoSet)更新
    ''' 2013/05/24 作成
    ''' 

    ''' 
    Public Function SaveSunpoData(Optional ByRef flg_connect As Boolean = True) As Boolean
        TimeMonStart()

        SaveSunpoData = True

        If flg_connect = False Then
            '計測データmdbに接続する

            If ConnectDB(m_koji_kanri_path & "\" & m_Keisoku_mdb_name, m_keisoku_dbclass) = False Then

                MsgBox("計測データmdbへの接続に失敗しました。", MsgBoxStyle.OkOnly)
                SaveSunpoData = False
                Exit Function
            End If

        ElseIf m_keisoku_dbclass.DbConnection Is Nothing Then '20161110 baluu add start
            '計測データmdbに接続する
            flg_connect = False
            If ConnectDB(m_koji_kanri_path & "\" & m_Keisoku_mdb_name, m_keisoku_dbclass) = False Then

                MsgBox("計測データmdbへの接続に失敗しました。", MsgBoxStyle.OkOnly)
                SaveSunpoData = False
                Exit Function
            End If
        End If
        '20161110 baluu add end
        m_keisoku_dbclass.BeginTrans()

        For i As Integer = 0 To WorksD.SunpoSetL.Count - 1

            WorksD.SunpoSetL(i).m_dbClass = m_keisoku_dbclass
            If WorksD.SunpoSetL(i).SaveData(False) = False Then
                m_keisoku_dbclass.RollbackTrans()
                SaveSunpoData = False
                Exit For
            End If

        Next

        m_keisoku_dbclass.CommitTrans()

        If flg_connect = False Then
            '計測データmdbに接続切断する
            DisConnectDB(m_keisoku_dbclass)
        End If

        SaveTenGunTeigiData(flg_connect)

        SaveSunpoData = True

        TimeMonEnd()

    End Function

    ''' 

    ''' カメラ一覧にデータを表示する
    ''' 2013/05/22 作成
    ''' 

    ''' 
    Private Sub SetCameraTab()

        Dim iRowNo As Integer = 0

        'Dim CameraD As New CameraInfoTable
        'CameraD.m_dbClass = m_system_dbclass
        'WorksD.CameraL = CameraD.GetDataToList

        'With DataGridView7

        '    .AllowUserToAddRows = False

        '    'ヘッダを中央
        '    .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        '    '並び替え禁止
        '    For Each T As DataGridViewColumn In .Columns
        '        T.SortMode = DataGridViewColumnSortMode.NotSortable
        '    Next

        '    .Rows.Clear()

        '    .Rows.Add(WorksD.CameraL.Count)

        '    flg_camera_grid = False

        '    For iRowNo = 0 To WorksD.CameraL.Count - 1

        '        If WorksD.CameraL(iRowNo).flg_use = "1" Then
        '            .Rows(iRowNo).Cells(0).Value = True
        '            TxtCamera.Text = WorksD.CameraL(iRowNo).camera_name             'カメラ名

        '        Else
        '            .Rows(iRowNo).Cells(0).Value = False
        '        End If

        '        .Rows(iRowNo).Cells(1).Value = WorksD.CameraL(iRowNo).camera_name   'カメラ名

        '    Next

        '    flg_camera_grid = True

        'End With

        With CmbCamera

            .Items.Clear()

            For iRowNo = 0 To WorksD.CameraL.Count - 1

                .Items.Add(WorksD.CameraL(iRowNo).camera_name)
                If WorksD.CameraL(iRowNo).flg_use = "1" Then
                    .Text = WorksD.CameraL(iRowNo).camera_name             'カメラ名

                    .SelectedItem = WorksD.CameraL(iRowNo).camera_name
                End If
            Next

            If .Text.Trim = "" Then
                .Text = WorksD.CameraL(0).camera_name
                .SelectedItem = WorksD.CameraL(0).camera_name
            End If

        End With

    End Sub

    'Private Sub SetTemplateGrid()

    '    Dim iRowNo As Integer = 0

    '    With DataGridView10

    '        .AllowUserToAddRows = False

    '        'ヘッダを中央
    '        .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

    '        '並び替え禁止
    '        For Each T As DataGridViewColumn In .Columns
    '            T.SortMode = DataGridViewColumnSortMode.NotSortable
    '        Next

    '        .Rows.Clear()

    '        .Rows.Add(2)
    '        iRowNo = 0
    '        .Rows(iRowNo).Cells(0).Value = True                     'No
    '        .Rows(iRowNo).Cells(1).Value = "製缶チェックシート１"     'テンプレート名
    '        iRowNo = 1
    '        .Rows(iRowNo).Cells(0).Value = False                     'No
    '        .Rows(iRowNo).Cells(1).Value = "製缶チェックシート２"     'テンプレート名

    '    End With

    'End Sub


#Region "実行ボタン"

    ''' 

    ''' 実行ボタン
    ''' 2013/05/16 作成
    ''' 

    ''' 
    Private Sub Button18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        With DataGridView8

            'ヘッダを中央
            .ColumnHeaderStyle = Application.Current.Resources("DataGrid_Header_Center")

            '並び替え禁止
            For Each T As System.Windows.Controls.DataGridColumn In .Columns
                T.CanUserSort = False
            Next

            .ItemsSource = Nothing
        End With
        With m_KaisekiItems
            .Clear()

            For i As Integer = 1 To 100
                .Add(New KaisekiItem)
            Next

            Dim ttt As New Random

            For iRowNo As Integer = 0 To 99

                Dim ss1 As Double = (ttt.NextDouble - 0.5) * 2000
                Dim ss2 As Double = (ttt.NextDouble - 0.5) * 2000
                Dim ss3 As Double = (ttt.NextDouble - 0.5) * 2000

                .Item(iRowNo).SokutenName = "CT" & iRowNo + 1        'No
                .Item(iRowNo).X = ss1.ToString("N2")                   'No
                .Item(iRowNo).Y = ss2.ToString("N2")                 '部位

                .Item(iRowNo).Z = ss3.ToString("N2")  '検査項目

            Next

        End With
        DataGridView8.ItemsSource = m_KaisekiItems

    End Sub

#End Region

#Region "詳細確認"

    ''' 

    ''' 詳細確認ボタン
    ''' 2013/05/16 作成
    ''' 

    ''' 
    Private Sub Button21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

#End Region




#Region "メニューボタン制御"

    Private Shared Function SystemColorBrush() As System.Windows.Media.Brush
        Return System.Windows.Media.Brushes.White
    End Function


    ''' 

    ''' 各画面の更新チェック
    ''' 2013/05/31 作成
    ''' 

    ''' 
    Private Function GamenUpdateCheck(ByVal iNo As Integer) As Boolean

        Dim flg As String = ""

        Try
            Select Case iNo
                Case 1
                    '基本情報
                    flg = WorksD.flg_Kihon
                    Exit Select
                Case 2
                    '規定値
                    flg = WorksD.flg_Kiteiti
                    Exit Select
                Case 3
                    '画像

                    flg = WorksD.flg_Gazou
                    Exit Select
                Case 4
                    '解析

                    flg = WorksD.flg_Kaiseki
                    Exit Select
                Case 5
                    '寸法確認

                    flg = WorksD.flg_Kakunin
                    Exit Select
                Case 6
                    '検査表出力

                    flg = WorksD.flg_Out
                    Exit Select
                Case Else
            End Select
        Catch ex As Exception
            GamenUpdateCheck = False
            Exit Function
        End Try

        If flg = "1" Then
            GamenUpdateCheck = True
        Else
            GamenUpdateCheck = False
        End If

    End Function


    ''' 

    ''' 基本情報画面設定

    ''' 2013/05/21 作成
    ''' 

    ''' 
    Private Sub SetKihon()
        Me.TabControl1.Tag = True
        TabControl1.SelectedIndex = 0
        Me.TabControl1.Tag = False

        ' change listview selected index
        Dim header As String = TabKihonInfo.Header
        Dim idx As Integer = -1
        For i As Integer = 0 To m_ListViewTabItemsItems.Count - 1
            If m_ListViewTabItemsItems(i).Text = header Then
                idx = i
                Exit For
            End If
        Next
        Me.ListViewTabItems.Tag = True
        Me.ListViewTabItems.SelectedIndex = idx
        Me.ListViewTabItems.Tag = False
        Me.ListViewTabItems.Focus()

        ' MainFrm
        Grid_MainFrame.Visibility = System.Windows.Visibility.Hidden
        'Rep By Yamada 20150319 Sta -------
        'ADD By Yamada 20150303 Sta ------- 
        '#If B_TARGET <> "TRUE" Then
#If USER = "TRUE" Then
        'Rep By Yamada 20150319 End -------
        Me.Button_TargetSetting.Visibility = Windows.Visibility.Collapsed
#End If
        'ADD By Yamada 20150303 End -------

    End Sub


    ''' 

    ''' 規定値画面設定

    ''' 2013/05/21 作成
    ''' 

    ''' 
    Private Sub SetKiteiti()
        Me.TabControl1.Tag = True
        TabControl1.SelectedIndex = 1
        Me.TabControl1.Tag = False

        ' change listview selected index
        Dim header As String = TabKiteiti.Header
        Dim idx As Integer = -1
        For i As Integer = 0 To m_ListViewTabItemsItems.Count - 1
            If m_ListViewTabItemsItems(i).Text = header Then
                idx = i
                Exit For
            End If
        Next
        Me.ListViewTabItems.Tag = True
        Me.ListViewTabItems.SelectedIndex = idx
        Me.ListViewTabItems.Tag = False
        Me.ListViewTabItems.Focus()

        ' MainFrm
        Grid_MainFrame.Visibility = System.Windows.Visibility.Hidden
    End Sub


    ''' 

    ''' 画像画面設定

    ''' 2013/05/21 作成
    ''' 

    ''' 
    Private Sub SetGazou()
        '  If m_flg_Senyou = True Then
        Me.TabControl1.Tag = True
        If m_flg_Zahyo = True Then
            TabControl1.SelectedIndex = 1
        Else
            TabControl1.SelectedIndex = 2
        End If
        Me.TabControl1.Tag = False
        BtnT4_FrmOut.Visibility = System.Windows.Visibility.Visible
        'Else
        '    Me.TabControl1.Tag = True
        '    TabControl1.SelectedIndex = 1
        '    Me.TabControl1.Tag = False
        '    BtnT4_FrmOut.Visibility = System.Windows.Visibility.Hidden
        'End If

        ' change listview selected index
        Dim header As String = TabGazou.Header
        Dim idx As Integer = -1
        For i As Integer = 0 To m_ListViewTabItemsItems.Count - 1
            If m_ListViewTabItemsItems(i).Text = header Then
                idx = i
                Exit For
            End If
        Next
        Me.ListViewTabItems.Tag = True
        Me.ListViewTabItems.SelectedIndex = idx
        Me.ListViewTabItems.Tag = False
        Me.ListViewTabItems.Focus()

        'Add By Yamada 20140922 Sta ----------------日本車輌
#If Anchor = 1 Then
        RbtnGazou.IsChecked = True
        'RbtnMemoryCard.IsChecked = True
#End If
#If True Then
        BtnCameraInput.Visibility = Windows.Visibility.Hidden
#End If

        'Add By Yamada 20140922 Sta ----------------日本車輌
        ' MainFrm
        Grid_MainFrame.Visibility = System.Windows.Visibility.Hidden

        'カメラ関連を表示する
        'SetCameraGrid()

        '(20141031 Tezuka ADD) テキストボックス、参照ボタンの制御
        If RbtnGazou.IsChecked = True Then
            BtnGazouFolder.IsEnabled = True
            TxtGazouFolder.IsEnabled = True
            BtnMemoryCard.IsEnabled = False
            TxtMemoryCard.IsEnabled = False
        ElseIf RbtnMemoryCard.IsChecked = True Then
            BtnGazouFolder.IsEnabled = False
            TxtGazouFolder.IsEnabled = False
            BtnMemoryCard.IsEnabled = True
            TxtMemoryCard.IsEnabled = True
        End If

        SetCameraTab()
        'ADD BY Kiryu 20160609　Sta 画像画面の検査表出力ボタンを常に非表示
        'ADD BY Yamada 20150304 Sta ----検査表出力ボタンを非表示にする
        'Dim B_KENSAOUT As Integer = GetPrivateProfileInt("Command", "B_KENSAOUT", 0, My.Application.Info.DirectoryPath & "\vform.ini")
        'If B_KENSAOUT = 1 Then
        '#If B_KENSAOUT = "FALSE" Then
        Me.BtnT4_FrmOut.Visibility = Windows.Visibility.Collapsed
        '#End If
        'End If
        'ADD BY Yamada 20150304 End ----検査表出力ボタンを非表示にする
        'ADD BY Kiryu 20160609 End画像画面の検査表出力ボタンを常に非表示

    End Sub


    ''' 

    ''' 解析画面設定

    ''' 2013/05/21 作成
    ''' 

    ''' 
    Private Sub SetKaiseki(ByVal flgSyori As Boolean)
        If m_flg_Senyou = True Then
            Me.TabControl1.Tag = True
            TabControl1.SelectedIndex = 3
            Me.TabControl1.Tag = False
            BtnT5Next.Visibility = System.Windows.Visibility.Visible
            If BtnT5Back.Margin.Left = BtnT5Next.Margin.Left Then
                BtnT5Back.Margin =
                    New System.Windows.Thickness(BtnT5Back.Margin.Left - 101, BtnT5Back.Margin.Top,
                                                 BtnT5Back.Margin.Right, BtnT5Back.Margin.Bottom)
            End If
        Else
            Me.TabControl1.Tag = True
            If m_flg_Zahyo = True Then
                TabControl1.SelectedIndex = 2
            Else
                TabControl1.SelectedIndex = 3
            End If
            Me.TabControl1.Tag = False
            If m_flg_Zahyo = True Then
                BtnT5Next.Visibility = System.Windows.Visibility.Hidden
                BtnT5Back.Margin =
                    New System.Windows.Thickness(BtnT5Next.Margin.Left, BtnT5Back.Margin.Top,
                                                 BtnT5Back.Margin.Right, BtnT5Back.Margin.Bottom)
            Else
                BtnT5Next.Visibility = System.Windows.Visibility.Visible
                If BtnT5Back.Margin.Left = BtnT5Next.Margin.Left Then
                    BtnT5Back.Margin =
                        New System.Windows.Thickness(BtnT5Back.Margin.Left - 101, BtnT5Back.Margin.Top,
                                                     BtnT5Back.Margin.Right, BtnT5Back.Margin.Bottom)
                End If
            End If
        End If

        '(20140527 Tezuka ADD) 「3次元座標出力」ボタンの制御
        Dim iiB_3DOUT As Integer = GetPrivateProfileInt("Command", "B_3DOUT", 1, My.Application.Info.DirectoryPath & "\vform.ini")
        Dim iiB_DXFOUT As Integer = GetPrivateProfileInt("Command", "B_DXFOUT", 1, My.Application.Info.DirectoryPath & "\vform.ini")

        If iiB_3DOUT = 1 Then
            '#If B_3DOUT = "TRUE" Then
            Btn3DOut.Visibility = System.Windows.Visibility.Visible
        Else
            '#Else
            Btn3DOut.Visibility = System.Windows.Visibility.Hidden

            '#End If
        End If
        '(20140527 Tezuka ADD) 「DXF出力」ボタンの制御
        If iiB_DXFOUT = 1 Then
            '#If B_DXFOUT = "TRUE" Then
            Button2.Visibility = System.Windows.Visibility.Visible
        Else
            '#Else
            Button2.Visibility = System.Windows.Visibility.Hidden
            '#End If
        End If
        'Add By Yamada 20140919 Sta -------------------「実行」ボタンの制御　日本車輌
#If Anchor = 1 Then
        BtnT5Run.Visibility = Windows.Visibility.Hidden
        Button2.Visibility = System.Windows.Visibility.Hidden
#End If
        'Add By Yamada 20140919 End -------------------日本車輌

        ' change listview selected index
        Dim header As String = TabKaiseki.Header
        Dim idx As Integer = -1
        For i As Integer = 0 To m_ListViewTabItemsItems.Count - 1
            If m_ListViewTabItemsItems(i).Text = header Then
                idx = i
                Exit For
            End If
        Next
        Me.ListViewTabItems.Tag = True
        Me.ListViewTabItems.SelectedIndex = idx
        Me.ListViewTabItems.Tag = False
        Me.ListViewTabItems.Focus()

        ' update MainFrm

        If MainFrm IsNot Nothing Then
            If bln_KaisekiOpened = True Then
                Grid_MainFrame.Visibility = System.Windows.Visibility.Visible
                'Else
                '    MainFrm.Close()
            End If
        End If
        If flgSyori = True Then
            'open3DMainFrame()
            'Host_MainFrame.Child = MainFrm
            'draw3DMainFrame()
        Else
            If bln_KaisekiOpened = True Then
                Grid_MainFrame.Visibility = System.Windows.Visibility.Visible
            End If
            'ここで実行の処理をする。
            'RunKaisekiProg()
            'draw3DMainFrame()
            ''画像処理フラグを更新する
            'WorksD.SetKaisekiFlg(False)
            ''解析データを一覧に表示する
            ''SetKaisekiDataToGrid()
        End If

    End Sub


    ''' 

    ''' 寸法確認画面設定

    ''' 2013/05/21 作成
    ''' 

    ''' 
    Public Sub SetSunpouKakunin()
        If m_flg_Senyou = True Then
            Me.TabControl1.Tag = True
            TabControl1.SelectedIndex = 4
            Me.TabControl1.Tag = False
            BtnT6_Next.Visibility = System.Windows.Visibility.Visible
            If BtnT6_Back.Margin.Left = BtnT6_Next.Margin.Left Then
                BtnT6_Back.Margin = New System.Windows.Thickness(BtnT6_Back.Margin.Left - 100,
                                                                 BtnT6_Back.Margin.Top,
                                                                 BtnT6_Back.Margin.Right,
                                                                 BtnT6_Back.Margin.Bottom)
            End If
            System.Windows.Controls.Grid.SetZIndex(DGW_KekkaKakunin, -99)
            System.Windows.Controls.Grid.SetZIndex(PictureBox3_Border, -99)
            System.Windows.Controls.Grid.SetZIndex(DataGridView9, 99)
        Else
            Me.TabControl1.Tag = True
            TabControl1.SelectedIndex = 4
            Me.TabControl1.Tag = False
            BtnT6_Next.Visibility = System.Windows.Visibility.Hidden
            BtnT6_Back.Margin = New System.Windows.Thickness(BtnT6_Next.Margin.Left,
                                                             BtnT6_Back.Margin.Top,
                                                             BtnT6_Back.Margin.Right,
                                                             BtnT6_Back.Margin.Bottom)
            System.Windows.Controls.Grid.SetZIndex(DGW_KekkaKakunin, 99)
            System.Windows.Controls.Grid.SetZIndex(PictureBox3_Border, 99)
            System.Windows.Controls.Grid.SetZIndex(DataGridView9, -99)
            If m_flg_Sunpo = True Then
                DGW_KekkaKakunin.Columns(6).Visibility = System.Windows.Visibility.Hidden
                DGW_KekkaKakunin.Columns(7).Visibility = System.Windows.Visibility.Hidden
                DGW_KekkaKakunin.Columns(8).Visibility = System.Windows.Visibility.Hidden
            ElseIf m_flg_Zukei Then
                DGW_KekkaKakunin.Columns(6).Visibility = System.Windows.Visibility.Visible
                DGW_KekkaKakunin.Columns(7).Visibility = System.Windows.Visibility.Visible
                DGW_KekkaKakunin.Columns(8).Visibility = System.Windows.Visibility.Visible
            End If
        End If

        ' change listview selected index
        Dim header As String = TabKakunin.Header
        Dim idx As Integer = -1
        For i As Integer = 0 To m_ListViewTabItemsItems.Count - 1
            If m_ListViewTabItemsItems(i).Text = header Then
                idx = i
                Exit For
            End If
        Next
        Me.ListViewTabItems.Tag = True
        Me.ListViewTabItems.SelectedIndex = idx
        Me.ListViewTabItems.Tag = False
        Me.ListViewTabItems.Focus()

        ' MainFrm
        Grid_MainFrame.Visibility = System.Windows.Visibility.Hidden

        SetValueToComboBox1()
        SetSunpoDataToGrid("")
        'add by SUSANO
        Dim iiB_KENSAOUT As Integer = GetPrivateProfileInt("Command", "B_KENSAOUT", 0, My.Application.Info.DirectoryPath & "\vform.ini")
        'ADD By Yamada 20150303 Sta ---------検査表出力へは行かせない
        If iiB_KENSAOUT = 0 Then
            '#If B_KENSAOUT = "FALSE" Then
            Me.BtnT6_Next.Visibility = Windows.Visibility.Collapsed
            '#End If
        End If
        'ADD By Yamada 20150303 End ---------検査表出力へは行かせない
    End Sub



    ''' 

    ''' 寸法確認一覧表にデータを表示する
    ''' 2013/05/22 作成
    ''' 

    '''    
    Private Sub SetSunpoDataToGrid(ByVal cb1Val As String)
        Dim iRowNo As Integer = 0

        'データ表示
        With DataGridView9

            'ヘッダを中央
            '.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeaderStyle = Application.Current.Resources("DataGrid_Header_Center")
            '.DefaultCellStyle.Background = System.Windows.Media.Brushes.White 'H25.5.25 Yamada

            '並び替え禁止
            For Each T As System.Windows.Controls.DataGridColumn In .Columns
                T.CanUserSort = False
            Next

            .CanUserAddRows = False
            .CanUserDeleteRows = False
            '.IsReadOnly = True


            .ItemsSource = Nothing
        End With
        With m_KakuninItems

            .Clear()
            'H25.5.25 Yamada
            'If WorksD.SunpoSetL.Count < 20 Then
            '    For i As Integer = 1 To 20
            '        .Add(New KakuninViewItem)
            '    Next
            'Else
            'For i As Integer = 1 To WorksD.SunpoSetL.Count
            '    .Add(New KakuninViewItem)
            'Next
            'End If
            For i As Integer = 0 To WorksD.SunpoSetL.Count - 1
                .Add(New KakuninViewItem)
                iRowNo = i + 1
                If cb1Val = WorksD.SunpoSetL(i).MeasurementSet Or WorksD.SunpoSetL(i).MeasurementSet = "" Or cb1Val = "" Then
                    Try
                        .Item(i).RowIndex = i
                        .Item(i).No = iRowNo                                'No
                        .Item(i).MeasurementSet = WorksD.SunpoSetL(i).MeasurementSet '計測セット (20160822 byambaa ADD)
                        .Item(i).Bui = WorksD.SunpoSetL(i).SunpoMark        '部位

                        .Item(i).KensaItem = WorksD.SunpoSetL(i).SunpoName        '検査項目

                        'Rep By Yamada 20140919 Sta ---------------------------------------------------------- 
                        '.Item(i).KiteiVal = CDbl(WorksD.SunpoSetL(i).KiteiVal).ToString("N1")       '規定値
                        .Item(i).KiteiVal = CDbl(WorksD.SunpoSetL(i).KiteiVal).ToString("N2")       '規定値
                        'Rep By Yamada 20140919 End ---------------------------------------------------------- 

                        .Item(i).KiteiMin = CDbl(WorksD.SunpoSetL(i).KiteiMin)         '最小許容値
                        .Item(i).KiteiMax = CDbl(WorksD.SunpoSetL(i).KiteiMax)         '最大許容値

                        'Rep By Yamada 20140919 Sta ---------------------------------------------------------- 
                        '.Item(i).SunpoVal = CDbl(WorksD.SunpoSetL(i).SunpoVal).ToString("N1")         '計測値
                        .Item(i).SunpoVal = CDbl(WorksD.SunpoSetL(i).SunpoVal).ToString("N2")         '計測値
                        'Rep By Yamada 20140919 End ---------------------------------------------------------- 


                        '20161110 baluu ADD start '20161202 baluu edit start
                        If WorksD.SunpoSetL(i).sek_Val Is Nothing Or WorksD.SunpoSetL(i).sek_Val = "" Then
                            .Item(i).sek_Val = CDbl(0).ToString("N2")
                            .Item(i).sek_sunpo_Val = (CDbl(WorksD.SunpoSetL(i).SunpoVal) - CDbl(0)).ToString("N2")
                        Else
                            .Item(i).sek_Val = CDbl(WorksD.SunpoSetL(i).sek_Val).ToString("N2")
                            .Item(i).sek_sunpo_Val = (CDbl(WorksD.SunpoSetL(i).SunpoVal) - CDbl(WorksD.SunpoSetL(i).sek_Val)).ToString("N2")
                        End If
                        '20161110 baluu ADD end '20161202 baluu edit end

                        .Item(i).flg_gouhi = WorksD.SunpoSetL(i).flg_gouhi
                        '合否判定
                        If WorksD.SunpoSetL(i).flg_gouhi = "1" Then
                            '.Item(i).flg_gouhi = "合"
                        Else

                            'Rep By Yamada 20150303 Sta --------------------------------
                            If WorksD.SunpoSetL(i).SunpoMark = "S-A" Or WorksD.SunpoSetL(i).SunpoMark = "S-B" Or WorksD.SunpoSetL(i).SunpoMark = "C-A" Or WorksD.SunpoSetL(i).SunpoMark = "C-B" Then
                                If WorksD.SunpoSetL(i).lstCT_ID.Count = 0 Then
                                    .Item(i).flg_gouhi = "9"
                                End If
                            End If

                            '#If Anchor = 1 Then
                            '                                                If WorksD.SunpoSetL(i).SunpoMark = "S-A" Or WorksD.SunpoSetL(i).SunpoMark = "S-B" Or WorksD.SunpoSetL(i).SunpoMark = "C-A" Or WorksD.SunpoSetL(i).SunpoMark = "C-B" Then
                            '                                                    If WorksD.SunpoSetL(i).lstCT_ID.Count = 0 Then
                            '                                                        .Item(i).flg_gouhi = "9"
                            '                                                        '  .Rows(i).DefaultCellStyle.BackColor = Color.Gray  'SUURI 20140318
                            '                                                    Else
                            '                                                        '.Item(i).flg_gouhi = "否"
                            '                                                        ' .Rows(i).DefaultCellStyle.BackColor = Color.Red 'SUURI 20140318
                            '                                                    End If
                            '                                                Else
                            '                                                    '.Item(i).flg_gouhi = "否"
                            '                                                    '.Rows(i).DefaultCellStyle.BackColor = Color.Red 'SUURI 20140318
                            '                                                End If
                            '#Else
                            '                        '.Rows(i).Cells(7).Value = "否"
                            '                        '.Rows(i).DefaultCellStyle.BackColor = Color.Red 'H25.5.25 Yamada　
                            '#End If
                            'Rep By Yamada 20150303 End --------------------------------
                        End If

                        '参照ターゲット
                        WorksD.SunpoSetL(i).SetSunpoCTID()
                        Dim strCTIDs As String = WorksD.SunpoSetL(i).lstCT_ID.Count & "個 ("
                        For Each CTID As Integer In WorksD.SunpoSetL(i).lstCT_ID
                            If CTID > 10000 Then
                                strCTIDs = strCTIDs & "ST" & CTID - 10000 & ","
                            Else
                                strCTIDs = strCTIDs & "CT" & CTID & ","
                            End If

                        Next
                        Try
                            strCTIDs = strCTIDs.Substring(0, strCTIDs.Length - 1)
                            If WorksD.SunpoSetL(i).lstCT_ID.Count > 0 Then
                                strCTIDs = strCTIDs & ")"
                            End If

                        Catch ex As Exception

                        End Try

                        .Item(i).CTIDs = strCTIDs

                        '出図フラグ
                        If WorksD.SunpoSetL(i).flgOutZu = "1" Then
                            .Item(i).flgOutZu = True
                        Else
                            .Item(i).flgOutZu = False
                        End If

                        '線色
                        'If .Columns(10).Visible = True Then
                        '    Dim mcolor As ModelColor
                        '    If ncolor > 0 Then
                        '        If WorksD.SunpoSetL(i).ZU_colorID = "" Or WorksD.SunpoSetL(i).ZU_colorID = "0" Then
                        '            .Rows(i).Cells(10).Value = "MAGENTA"
                        '        Else
                        '            mcolor = YCM_GetColorInfoByCode(WorksD.SunpoSetL(i).ZU_colorID)
                        '            .Rows(i).Cells(10).Value = mcolor.strName
                        '        End If
                        '    End If
                        'End If
                        ncolor = 0
                        YCM_ReadSystemColorAcs(m_strDataSystemPath)
                        Dim mcolor As ModelColor
                        If ncolor > 0 Then
                            If WorksD.SunpoSetL(i).ZU_colorID = "" Or WorksD.SunpoSetL(i).ZU_colorID = "0" Then
                                .Item(i).ZU_color2 = "MAGENTA"
                            Else
                                mcolor = YCM_GetColorInfoByCode(WorksD.SunpoSetL(i).ZU_colorID)
                                .Item(i).ZU_color2 = mcolor.strName
                            End If
                        End If

                        'SUURI20140817

                        nLineType = 0
                        YCM_ReadSystemLineTypes(m_strDataSystemPath)
                        Dim mLinetype As LineType
                        If nLineType > 0 Then
                            If WorksD.SunpoSetL(i).ZU_LineTypeID = "" Or WorksD.SunpoSetL(i).ZU_LineTypeID = "0" Then
                                .Item(i).ZU_LineType2 = "CONTINUOUS"
                            Else
                                mLinetype = YCM_GetLineTypeInfoByCode(WorksD.SunpoSetL(i).ZU_LineTypeID)
                                .Item(i).ZU_LineType2 = mLinetype.strName
                            End If
                        End If
                        'Select Case WorksD.SunpoSetL(i).ZU_LineTypeID
                        '    Case 1
                        '        .Rows(i).Cells(8).Value = "COUTINUOUS"
                        '    Case 2
                        '        .Rows(i).Cells(8).Value = "DASHED"
                        '    Case Else
                        '        .Rows(i).Cells(8).Value = ""
                        'End Select


                        ''H25.5.28
                        'WorksD.SunpoSetL
                        'レイヤ表示
                        .Item(i).ZU_layer = WorksD.SunpoSetL(i).ZU_layer

                        '20160904 byambaa(SUSANO) add start
                        '出図フラグ
                        If WorksD.SunpoSetL(i).sek_flgOutZu = "1" Then
                            .Item(i).sek_flgOutZu = True
                        Else
                            .Item(i).sek_flgOutZu = False
                        End If

                        ncolor = 0
                        YCM_ReadSystemColorAcs(m_strDataSystemPath)
                        Dim mcolor1 As ModelColor
                        If ncolor > 0 Then
                            If WorksD.SunpoSetL(i).sek_ZU_colorID = "" Or WorksD.SunpoSetL(i).sek_ZU_colorID = "0" Then
                                .Item(i).sek_ZU_color2 = "MAGENTA"
                            Else
                                mcolor1 = YCM_GetColorInfoByCode(WorksD.SunpoSetL(i).sek_ZU_colorID)
                                .Item(i).sek_ZU_color2 = mcolor1.strName
                            End If
                        End If

                        nLineType = 0
                        YCM_ReadSystemLineTypes(m_strDataSystemPath)
                        Dim mLinetype1 As LineType
                        If nLineType > 0 Then
                            If WorksD.SunpoSetL(i).sek_ZU_LineTypeID = "" Or WorksD.SunpoSetL(i).sek_ZU_LineTypeID = "0" Then
                                .Item(i).sek_ZU_LineType2 = "CONTINUOUS"
                            Else
                                mLinetype1 = YCM_GetLineTypeInfoByCode(WorksD.SunpoSetL(i).sek_ZU_LineTypeID)
                                .Item(i).sek_ZU_LineType2 = mLinetype1.strName
                            End If
                        End If

                        'WorksD.SunpoSetL
                        'レイヤ表示
                        .Item(i).sek_ZU_layer = WorksD.SunpoSetL(i).sek_ZU_layer
                        '20160904 byambaa (SUSANO) add end

                    Catch ex As Exception
                        MsgBox("値設定エラー", MsgBoxStyle.OkOnly)
                        Exit Sub
                    End Try
                End If
            Next
            ' .Columns.Item("Sunpo_KiteiVal").DefaultCellStyle.Format = "N1"

        End With

        For j As Integer = m_KakuninItems.Count - 1 To 0 Step -1
            If m_KakuninItems.Item(j).No = 0 Then
                m_KakuninItems.RemoveAt(j)
            End If
        Next

        '20161116 baluu add start
        Dim clsOPe As New CDBOperate
        DataGridView9.Columns.Item(4).Visibility = Windows.Visibility.Visible
        DataGridView9.Columns.Item(5).Visibility = Windows.Visibility.Visible
        DataGridView9.Columns.Item(9).Visibility = Windows.Visibility.Visible

        If Not clsOPe.ConnectDB(MainFrm.objFBM.ProjectPath & "\" & "計測データ.mdb") = False Then
            Dim adoRet As ADODB.Recordset
            Dim strSQL As String

            If ExistsTable(clsOPe, "[SekkeiKeisokuData]") Then
                strSQL = "SELECT COUNT(*) From [SekkeiKeisokuData]"
                adoRet = clsOPe.CreateRecordset(strSQL)

                If adoRet Is Nothing Then
                    DataGridView9.Columns.Item(5).Visibility = Windows.Visibility.Hidden
                    DataGridView9.Columns.Item(9).Visibility = Windows.Visibility.Hidden
                ElseIf adoRet.EOF Then
                    DataGridView9.Columns.Item(5).Visibility = Windows.Visibility.Hidden
                    DataGridView9.Columns.Item(9).Visibility = Windows.Visibility.Hidden
                Else
                    adoRet.MoveFirst()
                    If Integer.Parse(adoRet(0).Value) > 0 Then
                        DataGridView9.Columns.Item(4).Visibility = Windows.Visibility.Hidden
                    End If
                End If
            Else
                DataGridView9.Columns.Item(5).Visibility = Windows.Visibility.Hidden
                DataGridView9.Columns.Item(9).Visibility = Windows.Visibility.Hidden
            End If
            clsOPe.DisConnectDB()
        Else
            DataGridView9.Columns.Item(5).Visibility = Windows.Visibility.Hidden
            DataGridView9.Columns.Item(9).Visibility = Windows.Visibility.Hidden
        End If
        '20161116 baluu add end

        DataGridView9.ItemsSource = m_KakuninItems
        m_CellChange = 1

        'BYAMBAA ADD 20160904 
        If ConnectSystemDB(m_system_dbclass) = False Then
            MsgBox(err_msg_systemdb_connect, MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        Dim TypeInfoD As New TypeInfoTable
        TypeInfoD.m_dbClass = m_system_dbclass
        TypeInfoDL = TypeInfoD.GetFlgSekkeiFromData
        If TypeInfoDL Is Nothing Then
            DisConnectDB(m_system_dbclass)
            Exit Sub
        End If

        If TypeInfoDL.Item(0).flg_sekkei = "0" Then
            DataGridView9.Columns(17).Visibility = System.Windows.Visibility.Hidden
            DataGridView9.Columns(16).Visibility = System.Windows.Visibility.Hidden
            DataGridView9.Columns(15).Visibility = System.Windows.Visibility.Hidden
            DataGridView9.Columns(14).Visibility = System.Windows.Visibility.Hidden
        End If
        DisConnectDB(m_system_dbclass)
        'BYAMBAA ADD 20160904
    End Sub

    ''' 

    ''' 検査表出力画面設定

    ''' 2013/05/21 作成
    ''' 

    ''' 
    Private Sub SetOut()
        Me.TabControl1.Tag = True
        TabControl1.SelectedIndex = 5
        Me.TabControl1.Tag = False

        ' change listview selected index
        Dim header As String = "帳票出力"
        Dim idx As Integer = -1
        For i As Integer = 0 To m_ListViewTabItemsItems.Count - 1
            If m_ListViewTabItemsItems(i).Text = header Then
                idx = i
                Exit For
            End If
        Next
        Me.ListViewTabItems.Tag = True
        Me.ListViewTabItems.SelectedIndex = idx
        Me.ListViewTabItems.Tag = False
        Me.ListViewTabItems.Focus()

        If CommonTypeID = 24 Then
            KensaHyoOutput.GroupBox2.IsEnabled = False
        Else
            KensaHyoOutput.GroupBox2.IsEnabled = True
        End If

        ' MainFrm
        Grid_MainFrame.Visibility = System.Windows.Visibility.Hidden
    End Sub


    ''' 

    ''' 検査表出力画面ExcelTemplate設定

    ''' 2013/05/27 作成
    ''' 

    ''' 
    'Public Sub SetOutTemplate()

    '    With CmbExcelTemplate

    '        .Items.Clear()

    '        For i As Integer = 0 To WorksD.ExcelTemplateL.Count - 1

    '            .Items.Add(WorksD.ExcelTemplateL(i).TemplateFileName)
    '            If i = 0 Then
    '                .SelectedItem = WorksD.ExcelTemplateL(i).TemplateFileName
    '            End If

    '        Next

    '    End With


    'End Sub

#End Region






#Region "始めに画面"

    ''' 

    ''' 新規ボタン
    ''' 2013/05/21 作成
    ''' 

    ''' 
    Private Sub BtnT1New()

        ''新規

        'm_syori_flg = 0

        'チェックされたTypeIDを取得する

        'If GetTypeID() = False Then
        '    MsgBox("対象物を選択してください。", MsgBoxStyle.OkOnly)
        '    Exit Sub
        'End If

        'システム設定mdbに接続

        If ConnectSystemDB(m_system_dbclass) = False Then
            MsgBox(err_msg_systemdb_connect, MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        If AddDBflgScale(m_system_dbclass) = 0 Then
            ChgDBflgScale(m_system_dbclass)
        End If
        '    '
        WorksD = New WorksTable
        WorksD.m_dbClass = m_system_dbclass
        If WorksD.GetDataToList() = False Then
            Exit Sub
        End If

        'END SHUU
        Dim TGD As New TenGunTeigiTable
        TGD.m_dbClass = m_system_dbclass
        lstTengun = TGD.GetDataToList()


        ''計測データ.mdbにSunpouInfoをシステム設定mdbから書き込む
        'SetSunpouData()

        '(20141024 Tezuka ADD) strLastProjPathが存在しない時はデフォルトを設定する
        If System.IO.Directory.Exists(My.Settings.strLastProjPath) = False Then
            My.Settings.strLastProjPath = My.Settings.DefaultSavePath
        End If

        m_koji_kanri_path = My.Settings.strLastProjPath
        '20160608 Kiryu Add Sta 工事データフォルダの一つ上のディレクトリを初期表示
        If m_koji_kanri_path <> "C:\01_VFORM_Projects" Then
            Dim iFind1 As Integer = m_koji_kanri_path.LastIndexOf("\")
            m_koji_kanri_path = m_koji_kanri_path.Remove(iFind1)
        End If
        '20160608 Kiryu Add End

        '工事データの保存先
        TxtKojiData.Text = m_koji_kanri_path

        'メモリカード保存先
        If My.Settings.GazouSaveType = 1 Then
            If System.IO.Directory.Exists(My.Settings.strGazouPath) = True Then
                TxtMemoryCard.Text = My.Settings.strGazouPath
            Else
                TxtMemoryCard.Text = GetMemoryCardFolder()
                My.Settings.strGazouPath = GetMemoryCardFolder()
            End If
        Else
            TxtMemoryCard.Text = GetMemoryCardFolder()
        End If

        '画像フォルダ
        If My.Settings.GazouSaveType = 0 Then
            If System.IO.Directory.Exists(My.Settings.strGazouPath) = True Then
                TxtGazouFolder.Text = My.Settings.strGazouPath
            Else
                TxtGazouFolder.Text = My.Settings.DefaultSavePath
                My.Settings.strGazouPath = My.Settings.DefaultSavePath
            End If
        Else
            TxtGazouFolder.Text = My.Settings.DefaultSavePath
        End If


        My.Settings.Save()


        '他工事データからの参照ボタンの制御
        'SetKoujiSansyou()

        'システム設定mdb切断
        DisConnectDB(m_system_dbclass)

        SetTLP4()

        '2013/05/27 ADD
        '画像一覧に表示する
        '   GazouView(m_koji_kanri_path)

        'テンプレートを表示する
        KensaHyoOutput.SetOutTemplate()

        '基本情報を表示する
        SetKihon()

    End Sub

    ''' 

    ''' 開くボタン
    ''' 2013/05/24 作成
    ''' 

    ''' 
    Private Sub BtnT1Kaishi()
        Dim sw As New System.Diagnostics.Stopwatch
        sw.Start()
        ''開く
        'm_syori_flg = 1

        'チェックされたTypeIDを取得する

        'If GetTypeID() = False Then
        '    MsgBox("対象物を選択してください。", MsgBoxStyle.OkOnly)
        '    Exit Sub
        'End If

        'システム設定mdbに接続する

        If ConnectSystemDB(m_system_dbclass) = False Then
            MsgBox(err_msg_systemdb_connect, MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        If AddDBflgScale(m_system_dbclass) = 0 Then
            ChgDBflgScale(m_system_dbclass)
        End If

        'WorksTableデータを取得する

        'システム設定mdbから
        WorksD = New WorksTable
        WorksD.m_dbClass = m_system_dbclass
        If WorksD.GetDataToList() = False Then
            Exit Sub
        End If


        ''他工事データからの参照ボタンの制御
        'SetKoujiSansyou()

        '基本情報画面の設定()
        SetTLP4()

        'システム設定mdbに切断する
        DisConnectDB(m_system_dbclass)

        With kojiData
            '.m_form = "基本情報"
            '.m_SystemMdbPath = m_SystemMdbPath
            '.m_SystemMdbFullPath = m_SystemMdbFullPath
            '.m_keisoku_dbclass = m_keisoku_dbclass          '計測データ.mdb
            '.m_system_dbclass = m_system_dbclass            'システム設定.mdb
            'If .WorksD Is Nothing Then
            '    .WorksD = New WorksTable
            'End If
            '.WorksD.copy(WorksD)
            'sw.Stop()
            'Trace.WriteLine(sw.Elapsed.TotalSeconds & "秒かかりました")
            '.ShowDialog()

            If .Tag Is Nothing Then
                Me.Close()
                Exit Sub
            Else
                Me.Tag = .Tag
                ' set objControl of the Me.Tag
                If True Then
                    Dim wt As WorksTable = CType(Me.Tag, WorksTable)
                    If wt.KihonL.Count = WorksD.KihonL.Count Then
                        For i As Integer = 0 To wt.KihonL.Count - 1
                            wt.KihonL(i).objControl = WorksD.KihonL(i).objControl
                        Next
                    End If
                End If
                Dim WorksD1 As New WorksTable
                SetYomikomiData(WorksD1)
                WorksD = New WorksTable
                WorksD.copy(WorksD1)
                m_Keisoku_mdb_path = WorksD1.SavePath & "\" & m_Keisoku_mdb_name
                m_koji_kanri_path = WorksD1.SavePath
            End If
        End With

        '工事データフォルダ
        TxtKojiData.Text = m_koji_kanri_path
        'Excelファイル出力フォルダ
        KensaHyoOutput.TxtExcelFolder.Text = m_koji_kanri_path

        '計測データmdbに接続する

        If ConnectDB(m_Keisoku_mdb_path, m_keisoku_dbclass) = False Then
            MsgBox(err_msg_keisokumdb_connect, MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        If AddDBflgScale(m_keisoku_dbclass) = 0 Then
            ChgDBflgScale(m_keisoku_dbclass)
        End If

        'WorksTableデータを取得する

        '計測データmdbから
        'WorksD = New WorksTable
        'WorksD.copy(WorksD1)
        WorksD.m_dbClass = m_keisoku_dbclass
        sw.Start()
        If WorksD.GetDataToList() = False Then
            Exit Sub
        End If
        sw.Stop()
        Trace.WriteLine("WorksD.GetDataToList() が" & sw.Elapsed.TotalSeconds & "秒かかりました")

        '点群定義テーブルを計測データＭＤＢから読込み

        Dim TGD As New TenGunTeigiTable
        TGD.m_dbClass = m_keisoku_dbclass
        lstTengun = TGD.GetDataToList()

        '解析結果読み込み
        If WorksD.flg_Kaiseki = "1" Then
            ReadKaisekiData()
        End If

        SetYomikomiData(WorksD)

        '計測データmdbを切断する
        DisConnectDB(m_keisoku_dbclass)

        ''基本情報画面の設定

        'SetTLP4()

        '各画面を表示する
        ViewForm()

        '基本画面を開く

        SetKihon()
        sw.Stop()
        Trace.WriteLine(sw.Elapsed.TotalSeconds)

    End Sub

    ''' 

    ''' 全ての画面を設定する

    ''' 2013/05/24 作成
    ''' 

    ''' 
    Private Sub ViewForm()

        '*****************************************
        '規定値画面を表示する
        '*****************************************
        'If WorksD.flg_Kiteiti = "1" Then
        '    BtnKiteiti.BackColor = Color.Yellow
        'Else
        '    BtnKiteiti.BackColor = System.Drawing.SystemColors.Control
        'End If

        If m_flg_Senyou = True Then
            'グリッドヘッダ設定

            SetGridHeaddderKitei()
            SetValueToComboBox() '20160822 Byambaa ADD
            'データを取得してグリッドに表示
            SetKiteiDataToGrid("")
        End If

        '*****************************************
        '画像画面を表示する
        '*****************************************
        'If WorksD.flg_Gazou = "1" Then
        '    BtnGazou.BackColor = Color.Yellow
        'Else
        '    BtnGazou.BackColor = System.Drawing.SystemColors.Control
        'End If
        'メモリカードフォルダ
        TxtMemoryCard.Text = GetMemoryCardFolder()
        '画像フォルダ
        TxtGazouFolder.Text = m_koji_kanri_path
        '画像をチェックすると画像一覧に表示される

        '(20141024 Tezuka ADD) settingのプロジェクトパス、画像フォルダ、画像フォルダタイプを変更する   
        My.Settings.GazouSaveType = 0
        My.Settings.strLastProjPath = m_koji_kanri_path
        My.Settings.strGazouPath = m_koji_kanri_path

        RbtnGazou.IsChecked = True
        m_gazou_path = m_koji_kanri_path & "\Pdata"
        GazouView(m_gazou_path)
        'GazouListView1(m_koji_kanri_path)
        'カメラ関連を表示する
        SetCameraTab()

        '*****************************************
        '確認画面を表示する
        '*****************************************
        '解析データを一覧に表示する
        If WorksD.flg_Kaiseki = "1" Then
            SetKaisekiDataToGrid()
        End If


        '*****************************************
        '寸法確認画面を表示する
        '*****************************************
        'If WorksD.flg_Kakunin = "1" Then
        '    BtnSunpouKakunin.BackColor = Color.Yellow
        'Else
        '    BtnSunpouKakunin.BackColor = System.Drawing.SystemColors.Control
        'End If
        If m_flg_Sunpo = True Or m_flg_Zukei = True Or m_flg_Senyou = True Then
            SetValueToComboBox1()  '20160822 Byambaa ADD
            SetSunpoDataToGrid("")
        End If

        'テンプレートを表示する
        'If m_flg_Senyou = True Then
        '    SetOutTemplate()
        'End If

    End Sub

    ''' 

    ''' 次へボタン
    ''' 2013/05/21 作成
    ''' 

    ''' 
    Private Sub BtnT1Next_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'チェックされたTypeIDを取得する

        'GetTypeID()

        '基本情報を表示する
        SetKihon()

    End Sub

    ''' 

    ''' チェックされたTypeIDを取得する

    ''' 2013/05/24 作成
    ''' 

    ''' 
    'Private Function GetTypeID() As Boolean

    '    GetTypeID = True

    '    CommonTypeID = 0

    '    With TLP1

    '        Dim iRow As Integer = 0
    '        For i As Integer = 0 To .Controls.Count - 1

    '            Try
    '                If .Controls.Item(i).GetType.Name = "RadioButton" Then

    '                    Dim RD2 As Object = CType(.Controls.Item(i), RadioButton)
    '                    If RD2.checked = True Then
    '                        CommonTypeID = TypeInfoDL(iRow).ID
    '                        'm_TemplateExcelFile = TypeInfoDL(iRow).ExcelTemplateName
    '                        Exit For
    '                    End If
    '                    iRow += 1
    '                End If
    '            Catch ex As Exception

    '            End Try

    '        Next

    '    End With

    '    If CommonTypeID = 0 Then
    '        GetTypeID = False
    '    End If

    'End Function

#End Region

#Region "基本情報画面"

    ''' 

    ''' 工事データフォルダを選択する

    ''' 2013/05/17 作成
    ''' 

    ''' 
    Private Sub BtnKojiData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnKojiData.Click

        ''FolderBrowserDialogクラスのインスタンスを作成
        'Dim fbd As New FolderBrowserDialog
#If True Then
        With m_fod
            .FileName = "工事フォルダを指定してください。"
            If System.IO.Directory.Exists(My.Settings.strLastProjPath) = True Then
                .InitialDirectory = My.Settings.strLastProjPath
            Else
                .InitialDirectory = My.Settings.DefaultSavePath
            End If

            Dim iDialogResult As System.Windows.Forms.DialogResult
            iDialogResult = .ShowDialog()
            If iDialogResult = System.Windows.Forms.DialogResult.OK Then
                '選択されたフォルダを表示する
                TxtKojiData.Text = YCM_GetFolderPath_fromFullPath(.FileName)
                m_koji_kanri_path = YCM_GetFolderPath_fromFullPath(.FileName)
                My.Settings.strLastProjPath = m_koji_kanri_path
            Else
                Exit Sub
            End If
        End With
#Else
   With m_fbd

            '上部に表示する説明テキストを指定する

            .Description = "工事データフォルダを指定して下さい。"
            ''ルートフォルダを指定する

            ''デフォルトでDesktop
            '.RootFolder = Environment.SpecialFolder.Desktop
            '最初に選択するフォルダを指定する

            'RootFolder以下にあるフォルダである必要がある
            .SelectedPath = TxtKojiData.Text.Trim
            ''ユーザーが新しいフォルダを作成できるようにする
            ''デフォルトでTrue
            '.ShowNewFolderButton = True

            ''ダイアログを表示する
            'If .ShowDialog(Me) = DialogResult.OK Then
            '    '選択されたフォルダを表示する
            '    TxtKojiData.Text = .SelectedPath
            '    m_koji_kanri_path = .SelectedPath
            'End If

            'ダイアログを表示する
            Dim iDialogResult As System.Windows.Forms.DialogResult
            iDialogResult = .ShowDialog()
            If iDialogResult = System.Windows.Forms.DialogResult.OK Then
                '選択されたフォルダを表示する
                TxtKojiData.Text = .SelectedPath
                m_koji_kanri_path = .SelectedPath
            Else
                Exit Sub
            End If

        End With
#End If
     

    End Sub

    ''' 

    ''' 他工事データからの参照ボタン
    ''' 2013/05/21 作成
    ''' 

    ''' 
    Private Sub BtnT2KojiSansyou_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnT2KojiSansyou.Click

        '基本情報
        gKojiDataConf = New KojiDataConf
        With gKojiDataConf
            .m_form = "基本情報"
            .m_SystemMdbPath = m_SystemMdbPath
            .m_SystemMdbFullPath = m_SystemMdbFullPath
            .m_keisoku_dbclass = m_keisoku_dbclass          '計測データ.mdb
            .m_system_dbclass = m_system_dbclass            'システム設定.mdb
            If .WorksD Is Nothing Then
                .WorksD = New WorksTable
            End If
            .WorksD.copy(WorksD)
            .ShowDialog()

            If .Tag Is Nothing Then
            Else
                Me.Tag = .Tag
                Dim WorksD1 As New WorksTable
                SetYomikomiData(WorksD1)
                m_Keisoku_mdb_path = WorksD1.SavePath & "\" & m_Keisoku_mdb_name
            End If
            '.Dispose()
        End With

    End Sub

    ''' 

    ''' 前へボタン
    ''' 2013/05/21 作成
    ''' 

    ''' 
    Private Sub BtnT2Back_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnT2Back.Click
        Me.Close()
        'gMMainGamen.Refresh()
        '始めに
        'SetHajimeni()

    End Sub

    '''' 

    '''' 次へボタン
    '''' 2013/05/21 作成
    '''' 「新規」と「開く」に対応するため一時削除
    '''' 2013/05/24 削除
    '''' 

    '''' 
    'Private Sub BtnT2Next_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnT2Next.Click

    '    If System.IO.Directory.Exists(TxtKojiData.Text.Trim) = True Then
    '        If System.IO.File.Exists(TxtKojiData.Text.Trim & "\" & m_Keisoku_mdb_name) = True Then
    '            MsgBox("既に計測データが存在します。" & vbCrLf & "別のフォルダを指定して下さい。", MsgBoxStyle.OkOnly)
    '            Exit Sub
    '        Else
    '            '計測データ.mdbが無い場合、コピーする
    '            m_koji_kanri_path = TxtKojiData.Text.Trim
    '            System.IO.File.Copy(m_KeisokuTempMdbPath, m_koji_kanri_path & "\" & m_Keisoku_mdb_name)
    '        End If
    '    Else
    '        MsgBox("工事データを指定して下さい。", MsgBoxStyle.YesNo)
    '        Exit Sub
    '    End If

    '    '規定値入力

    '    'コピーした計測データ.mdbに寸法算出テーブルの規定値・最小許容値・最大許容値を設定する

    '    My.Settings.strLastProjPath = m_koji_kanri_path
    '    My.Settings.Save()

    '    m_Keisoku_mdb_path = m_koji_kanri_path & "\" & m_Keisoku_mdb_name

    '    m_koji_kanri_path = m_koji_kanri_path

    '    '検査表出力フォルダに設定する

    '    TxtExcelFolder.Text = m_koji_kanri_path

    '    '画面の入力値を取得する

    '    GetDataFromToControl()

    '    '計測データDBに接続する

    '    If ConnectDB(m_koji_kanri_path & "\" & m_Keisoku_mdb_name, m_keisoku_dbclass) = False Then
    '        MsgBox("計測データ.mdbを開くことができません。")
    '        Exit Sub
    '    End If

    '    '計測データ.mdbに入力値を書き込む
    '    'KihonInfoの更新
    '    If SetKeisokuData(m_koji_kanri_path & "\" & m_Keisoku_mdb_name) = False Then
    '        DisConnectDB(m_keisoku_dbclass)
    '        MsgBox("計測データ更新に失敗しました。", MsgBoxStyle.OkOnly)
    '        Exit Sub
    '    End If

    '    '計測データ.mdbのSunpouInfoに書き込む
    '    If SetSunpouData() = False Then
    '        DisConnectDB(m_keisoku_dbclass)
    '        MsgBox("計測データ更新に失敗しました。", MsgBoxStyle.OkOnly)
    '        Exit Sub
    '    End If

    '    '計測データ.mdbを切断する
    '    DisConnectDB(m_keisoku_dbclass)

    '    'システム設定mdbに接続する

    '    If ConnectSystemDB(m_system_dbclass) = False Then
    '        MsgBox(err_msg_systemdb_connect, MsgBoxStyle.OkOnly)
    '        Exit Sub
    '    End If

    '    'システム設定.mdbを更新する
    '    'Works, WorksInfoの更新
    '    If SetSystemData(m_SystemMdbFullPath, m_koji_kanri_path) = False Then
    '        MsgBox("システム設定データ更新に失敗しました。", MsgBoxStyle.OkOnly)
    '        Exit Sub
    '    End If

    '    'システム設定.mdbを切断する
    '    DisConnectDB(m_system_dbclass)

    '    '初期設定

    '    If SetInitFormKitei() = False Then
    '        Exit Sub
    '    End If

    '    'グリッドヘッダ設定

    '    SetGridHeaddderKitei()

    '    'データを取得してグリッドに表示
    '    SetDataKitei()

    '    '規定値画面へ
    '    SetKiteiti()




    '    'Dim strdatabasefile As String = ""
    '    'm_strDataBasePath = ""
    '    'm_strDataBasePath = ComSel_SelectFolderByShell("工事データフォルダを指定して下さい。", True)
    '    'If m_strDataBasePath <> Nothing Then
    '    '    If System.IO.File.Exists(m_strDataBasePath & "\" & m_Keisoku_mdb_name) = False Then
    '    '        '計測データ.mdbが無い場合、コピーする
    '    '        System.IO.File.Copy(m_KeisokuTempMdbPath, m_strDataBasePath & "\" & m_Keisoku_mdb_name)
    '    '    Else
    '    '        'If MsgBox("既に計測データが存在します。" & vbCrLf & "上書きしますか？", MsgBoxStyle.YesNo, "確認") = MsgBoxResult.Yes Then
    '    '        '    System.IO.File.Delete(m_strDataBasePath & "\" & m_Keisoku_mdb_name)
    '    '        '    System.IO.File.Copy(m_KeisokuTempMdbPath, m_strDataBasePath & "\" & m_Keisoku_mdb_name)
    '    '        'End If
    '    '        '既に計測データが存在する場合は、別のフォルダを指定してもらう

    '    '        MsgBox("既に計測データが存在します。" & vbCrLf & "別のフォルダを指定して下さい。", MsgBoxStyle.OkOnly)
    '    '        Exit Sub
    '    '    End If

    '    '    '規定値入力

    '    '    'コピーした計測データ.mdbに寸法算出テーブルの規定値・最小許容値・最大許容値を設定する

    '    '    m_Keisoku_mdb_path = m_strDataBasePath & "\" & m_Keisoku_mdb_name

    '    '    m_koji_kanri_path = m_strDataBasePath

    '    '    '画面の入力値を取得する

    '    '    GetDataFromToControl()

    '    '    '計測データ.mdbに入力値を書き込む
    '    '    'KihonInfoの更新
    '    '    If SetKeisokuData(m_strDataBasePath & "\" & m_Keisoku_mdb_name) = False Then
    '    '        DisConnectDB(m_keisoku_dbclass)
    '    '        MsgBox("計測データ更新に失敗しました。", MsgBoxStyle.OkOnly, "確認")
    '    '        Exit Sub
    '    '    End If

    '    '    '計測データ.mdbにSunpouInfoをシステム設定mdbから書き込む
    '    '    SetSunpouData()

    '    '    'システム設定.mdbを更新する
    '    '    'Works, WorksInfoの更新
    '    '    If SetSystemData(m_SystemMdbFullPath, m_strDataBasePath) = False Then
    '    '        MsgBox("システム設定データ更新に失敗しました。", MsgBoxStyle.OkOnly, "確認")
    '    '        Exit Sub
    '    '    End If
    '    'Else
    '    '    Exit Sub
    '    'End If

    '    ''BtnKojiSansyo.Enabled = True

    '    ''計測データ.mdbを切断する
    '    'DisConnectDB(m_keisoku_dbclass)
    '    '''システム設定.mdbを切断する
    '    ''DisConnectDB(m_system_dbclass)

    '    ''初期設定

    '    'If SetInitFormKitei() = False Then
    '    '    Exit Sub
    '    'End If

    '    ''グリッドヘッダ設定

    '    'SetGridHeaddderKitei()

    '    ''データを取得してグリッドに表示
    '    'SetDataKitei()

    '    ''規定値画面へ
    '    'SetKiteiti()

    'End Sub

    ''' 

    ''' 次へボタン
    ''' 2013/05/21 作成
    ''' 

    ''' 
    Private Sub BtnT2Next_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnT2Next.Click

        If WorksD.flg_Kihon = "1" Then
            ' 2014-5-27 str by Ljj
            'If Me.CheckBox1.IsChecked = True Then
            '    Dim strPathCp As String = ""

            '    Try
            '        Dim StrU As String = My.Settings.DefaultSavePath
            '        Dim StrGetPath As String = TxtKojiData.Text
            '        Dim strArr As String() = Split(StrGetPath, "\")
            '        Dim strPath As String
            '        Dim strP As String = ""
            '        Dim ii As Integer

            '        For ii = 0 To strArr.Length - 2
            '            strP = strP + strArr(ii).ToString + "\"
            '        Next
            '        strPath = strP + StrU

            '        strPathCp = strPath

            '        MkDir(strPath)


            '        'System.IO.File.Copy(m_KeisokuTempMdbPath, strPathCp & "\" & m_Keisoku_mdb_name)

            '    Catch ex As Exception
            '        'MsgBox("自動保存失敗" + ex.Message)
            '    End Try

            '    System.IO.File.Copy(m_KeisokuTempMdbPath, strPathCp & "\" & m_Keisoku_mdb_name)
            'End If
            ' 2014-5-27 end by Ljj

            '開く
            BtnT2Next_Open()
        Else
            ' 2014-5-27 str by Ljj
            'If Me.CheckBox1.IsChecked = True Then
            '    Dim strPathCp As String = ""
            '    Try
            '        Dim StrU As String = My.Settings.DefaultSavePath
            '        Dim strPath As String = "KeisokuFolder_" & DateTime.Now.ToString("yyyyMMddHHmmss")
            '        strPathCp = StrU & "\" & strPath
            '        If System.IO.Directory.Exists(strPathCp.Trim) = True Then
            '            strPathCp = strPathCp & "00"
            '        End If
            '        MkDir(strPathCp)
            '        TxtKojiData.Text = strPathCp
            '        'System.IO.File.Copy(m_KeisokuTempMdbPath, strPathCp & "\" & m_Keisoku_mdb_name)
            '    Catch ex As Exception
            '        'MsgBox("自動保存失敗" + ex.Message)
            '    End Try
            '    'System.IO.File.Copy(m_KeisokuTempMdbPath, strPathCp & "\" & m_Keisoku_mdb_name)
            'End If

            ' 2014-5-27 end by Ljj
            If TxtKojiDataFolder.Text = "" Then
                Dim strPathCp As String = ""

                Try
                    Dim StrU As String = My.Settings.DefaultSavePath
                    Dim strPath As String = "KeisokuFolder_" & DateTime.Now.ToString("yyyyMMddHHmmss")

                    strPathCp = StrU & "\" & strPath
                    If System.IO.Directory.Exists(strPathCp.Trim) = True Then
                        strPathCp = strPathCp & "00"
                        Me.TxtKojiDataFolder.Text = strPath & "00"
                    End If
                    MkDir(strPathCp)
                    TxtKojiData.Text = strPathCp
                    Me.TxtKojiDataFolder.Text = strPath
                Catch ex As Exception
                    'MsgBox("自動保存失敗" + ex.Message)
                End Try
            Else
                Dim strPath As String = TxtKojiData.Text & "\" & TxtKojiDataFolder.Text
                Dim strPathNew As String = strPath
                Dim i As Integer = 0
                While True
                    If System.IO.Directory.Exists(strPathNew) = False Then
                        'System.IO.Directory.CreateDirectory(strPathNew)
                        MkDir(strPathNew)
                        TxtKojiData.Text = strPathNew
                        Exit While
                    Else
                        If Directory.EnumerateFileSystemEntries(strPathNew).Any() = False Then
                            TxtKojiData.Text = strPathNew
                            Exit While
                        End If
                    End If
                    i += 1
                    strPathNew = strPath & i
                End While
                If i > 0 Then
                    TxtKojiDataFolder.Text = TxtKojiDataFolder.Text & i
                End If
            End If

            '新規
            BtnT2Next_New()
        End If

    End Sub

    ''' 

    ''' 次へボタン
    ''' 新規の場合

    ''' 2013/05/24 作成
    ''' 

    ''' 
    Private Sub BtnT2Next_New()
        
        If System.IO.Directory.Exists(TxtKojiData.Text.Trim) = True Then
            If System.IO.File.Exists(TxtKojiData.Text.Trim & "\" & m_Keisoku_mdb_name) = True Then
                MsgBox("既に計測データが存在します。" & vbCrLf & "別のフォルダを指定して下さい。", MsgBoxStyle.OkOnly)
                Exit Sub
            Else
                '計測データ.mdbが無い場合、コピーする
                m_koji_kanri_path = TxtKojiData.Text.Trim
                System.IO.File.Copy(m_KeisokuTempMdbPath, m_koji_kanri_path & "\" & m_Keisoku_mdb_name)
            End If
        Else
            MsgBox("工事データを指定して下さい。", MsgBoxStyle.YesNo)
            Exit Sub
        End If

        'If System.IO.File.Exists(TxtKojiData.Text.Trim & "\" & Me.TxtKojiDataFolder.Text & ".vform") = False Then
        '    System.IO.File.Create(TxtKojiData.Text.Trim & "\" & Me.TxtKojiDataFolder.Text & ".vform").Dispose()
        'End If
        If System.IO.File.Exists(TxtKojiData.Text.Trim & "\" & Me.TxtKojiDataFolder.Text & ".vform") = False Then
            System.IO.File.Create(TxtKojiData.Text.Trim & "\" & Me.TxtKojiDataFolder.Text & ".vform")
        End If

        '規定値入力

        'コピーした計測データ.mdbに寸法算出テーブルの規定値・最小許容値・最大許容値を設定する

        My.Settings.strLastProjPath = m_koji_kanri_path
        My.Settings.Save()

        m_Keisoku_mdb_path = m_koji_kanri_path & "\" & m_Keisoku_mdb_name

        m_koji_kanri_path = m_koji_kanri_path

        '検査表出力フォルダに設定する

        KensaHyoOutput.TxtExcelFolder.Text = m_koji_kanri_path

        '画面の入力値を取得する

        GetDataFromToControl()

        '計測データDBに接続する

        If ConnectDB(m_koji_kanri_path & "\" & m_Keisoku_mdb_name, m_keisoku_dbclass) = False Then
            MsgBox("計測データ.mdbを開くことができません。")
            Exit Sub
        End If

        If AddDBflgScale(m_keisoku_dbclass) = 0 Then
            ChgDBflgScale(m_keisoku_dbclass)
        End If

        '計測データ.mdbに入力値を書き込む
        'KihonInfoの更新
        If SetKeisokuData(m_koji_kanri_path & "\" & m_Keisoku_mdb_name) = False Then
            DisConnectDB(m_keisoku_dbclass)
            MsgBox("計測データ更新に失敗しました。(KihonInfo)", MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        '計測データ.mdbのTenGunTeigiに書き込む
        'SUURI ADD START 点群定義テーブルも工事固有のデータにする。
        If SetTengunTeigData() = False Then
            DisConnectDB(m_keisoku_dbclass)
            MsgBox("計測データ更新に失敗しました。(TenGunTeigi)", MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        Dim TGD As New TenGunTeigiTable
        TGD.m_dbClass = m_keisoku_dbclass
        lstTengun = TGD.GetDataToList()
        'SUURI ADD END 

        '計測データ.mdbのSunpouInfoに書き込む
        If SetSunpouData() = False Then
            DisConnectDB(m_keisoku_dbclass)
            MsgBox("計測データ更新に失敗しました。(SunpouInfo)", MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        '計測データ.mdbのCameraInfoに書き込む
        If SetCameraInfoData() = False Then
            DisConnectDB(m_keisoku_dbclass)
            MsgBox("計測データ更新に失敗しました。(CameraInfo)", MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        '計測データ.mdbのExcelTemplateに書き込む
        If SetExcelTemplateData() = False Then
            DisConnectDB(m_keisoku_dbclass)
            MsgBox("計測データ更新に失敗しました。(ExcelTemplate)", MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        '計測データ.mdbのSunpoCalcHouhouに書き込む
        If SetSunpoCalcHouhouData() = False Then
            DisConnectDB(m_keisoku_dbclass)
            MsgBox("寸法計算方法データ更新に失敗しました。", MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        '計測データ.mdbを切断する
        DisConnectDB(m_keisoku_dbclass)

        'システム設定mdbに接続する

        If ConnectSystemDB(m_system_dbclass) = False Then
            MsgBox(err_msg_systemdb_connect, MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        'システム設定.mdbを更新する
        'Works, WorksInfoの更新
       
        If SetSystemData(m_SystemMdbFullPath, m_koji_kanri_path) = False Then
            MsgBox("システム設定データ更新に失敗しました。", MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        '基本情報処理フラグを更新する
        WorksD.SetKihonFlg()

        'システム設定.mdbを切断する
        DisConnectDB(m_system_dbclass)

        '(20140527 Tezuka Change)
        If m_flg_Zahyo = True Then
            '画像画面へ
            SetGazou()
        Else
            '規定値画面へ
            If SetInitFormKitei() = False Then
                Exit Sub
            End If

            'グリッドヘッダ設定
            SetGridHeaddderKitei()
            SetValueToComboBox() '20160822 Byambaa ADD
            'データを取得してグリッドに表示
            SetDataKitei()

            SetKiteiti()
        End If
        ' Else
        '  SetGazou()
        ' End If

    End Sub

    ''' 

    ''' 次へボタン
    ''' 開くの場合

    ''' 2013/05/24 作成
    ''' 

    ''' 
    Private Sub BtnT2Next_Open()

        m_koji_kanri_path = TxtKojiData.Text.Trim

        m_Keisoku_mdb_path = m_koji_kanri_path & "\" & m_Keisoku_mdb_name

        '画面の入力値を取得する

        GetDataFromToControl()

        '計測データDBに接続する

        If ConnectDB(m_koji_kanri_path & "\" & m_Keisoku_mdb_name, m_keisoku_dbclass) = False Then
            MsgBox("計測データ.mdbを開くことができません。")
            Exit Sub
        End If

        '計測データ.mdbに入力値を書き込む
        'KihonInfoの更新
        If SetKeisokuData(m_koji_kanri_path & "\" & m_Keisoku_mdb_name) = False Then
            DisConnectDB(m_keisoku_dbclass)
            MsgBox("計測データ更新に失敗しました。", MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        '計測データ.mdbを切断する
        DisConnectDB(m_keisoku_dbclass)

        'システム設定mdbに接続する

        If ConnectSystemDB(m_system_dbclass) = False Then
            MsgBox(err_msg_systemdb_connect, MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        'システム設定.mdbを更新する
        'Works, WorksInfoの更新
        If SetSystemData(m_SystemMdbFullPath, m_koji_kanri_path) = False Then
            MsgBox("システム設定データ更新に失敗しました。", MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        '基本情報処理フラグを更新する
        WorksD.SetKihonFlg()

        'システム設定.mdbを切断する
        DisConnectDB(m_system_dbclass)

        '(20140527 Tezuka Change)
        If m_flg_Zahyo = True Then

            '画像画面へ
            m_gazou_event = False
            TabControl1.SelectedIndex = 2
            m_gazou_event = True
            TxtGazouFolder.Text = m_koji_kanri_path
            RbtnGazou.IsChecked = True

            SetGazou()

        Else

            '規定値画面へ
            SetGridHeaddderKitei()
            SetValueToComboBox()  '20160822 Byambaa ADD
            SetKiteiDataToGrid("")
            SetKiteiti()
        End If
        'Else
        'SetGazou()
        'End If

    End Sub
    'SUURI ADD 点群定義
    Private Function SetTengunTeigData() As Boolean

        ''データ取得

        'Dim SSD As New SunpoSetTable
        'SSD.m_dbClass = m_system_dbclass
        'WorksD.SunpoSetL = SSD.GetDataToList()
        'If WorksD.SunpoSetL Is Nothing Then
        '    Exit Sub
        'End If

        SetTengunTeigData = True

        m_keisoku_dbclass.BeginTrans()
        'データ書き込み

        For Each T As TenGunTeigiTable In lstTengun
            T.m_dbClass = m_keisoku_dbclass
            If T.SaveData(False) = False Then
                m_keisoku_dbclass.RollbackTrans()
                SetTengunTeigData = False
                Exit Function
            End If
        Next

        m_keisoku_dbclass.CommitTrans()

    End Function
    Private Function SetSunpouData() As Boolean

        ''データ取得

        'Dim SSD As New SunpoSetTable
        'SSD.m_dbClass = m_system_dbclass
        'WorksD.SunpoSetL = SSD.GetDataToList()
        'If WorksD.SunpoSetL Is Nothing Then
        '    Exit Sub
        'End If

        SetSunpouData = True

        m_keisoku_dbclass.BeginTrans()

        'データ書き込み
        For Each T As SunpoSetTable In WorksD.SunpoSetL
            T.m_dbClass = m_keisoku_dbclass
            If T.SaveData(False) = False Then
                m_keisoku_dbclass.RollbackTrans()
                SetSunpouData = False
                Exit Function
            End If
        Next

        m_keisoku_dbclass.CommitTrans()

    End Function

    Private Function SetCameraInfoData() As Boolean

        SetCameraInfoData = True

        m_keisoku_dbclass.BeginTrans()

        'データ書き込み
        For Each T As CameraInfoTable In WorksD.CameraL
            T.m_dbClass = m_keisoku_dbclass
            If T.SaveData(False) = False Then
                m_keisoku_dbclass.RollbackTrans()
                SetCameraInfoData = False
                Exit Function
            End If
        Next

        m_keisoku_dbclass.CommitTrans()

    End Function

    Private Function SetExcelTemplateData() As Boolean

        SetExcelTemplateData = True

        m_keisoku_dbclass.BeginTrans()

        'データ書き込み
        For Each T As ExcelTemplateTable In WorksD.ExcelTemplateL
            T.m_dbClass = m_keisoku_dbclass
            If T.SaveData(False) = False Then
                m_keisoku_dbclass.RollbackTrans()
                SetExcelTemplateData = False
                Exit Function
            End If
        Next

        m_keisoku_dbclass.CommitTrans()

    End Function

    Private Function SetSunpoCalcHouhouData() As Boolean

        SetSunpoCalcHouhouData = True

        m_keisoku_dbclass.BeginTrans()

        'データ書き込み
        For Each T As SunpoCalcHouhouTable In WorksD.SunpoCalcHouhouL
            T.m_dbClass = m_keisoku_dbclass
            If T.SaveData(False) = False Then
                m_keisoku_dbclass.RollbackTrans()
                SetSunpoCalcHouhouData = False
                Exit Function
            End If
        Next

        m_keisoku_dbclass.CommitTrans()

    End Function
#End Region

#Region "規定値画面"

    ''' 

    ''' 他工事データからの参照ボタン
    ''' 2013/05/21 作成
    ''' 

    ''' 
    Private Sub BtnT3KojiSansyou_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnT3KojiSansyou.Click

        With gKojiDataConf
            .m_form = "規定値"
            .m_SystemMdbPath = m_SystemMdbPath
            .m_SystemMdbFullPath = m_SystemMdbFullPath
            .m_keisoku_dbclass = m_keisoku_dbclass          '計測データ.mdb
            .m_system_dbclass = m_system_dbclass            'システム設定.mdb
            If .WorksD Is Nothing Then
                .WorksD = New WorksTable
            End If
            .WorksD.copy(WorksD)
            .ShowDialog()

            If .Tag Is Nothing Then
            Else
                Me.Tag = .Tag
                Dim WorksD1 As New WorksTable
                WorksD1.copy(CType(Me.Tag, WorksTable))
                If ConnectDB(WorksD1.SavePath & "\" & m_Keisoku_mdb_name, m_keisoku_dbclass) = True Then
                    SetDataKitei()
                End If
                m_Keisoku_mdb_path = WorksD1.SavePath & "\" & m_Keisoku_mdb_name
            End If
        End With

    End Sub

    ''' 

    ''' 前へボタン
    ''' 2013/05/21 作成
    ''' 

    ''' 
    Private Sub BtnT3Back_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnT3Back.Click

        '基本情報
        SetKihon()

    End Sub

    ''' 

    ''' 次へボタン
    ''' 2013/05/24 作成
    ''' 

    ''' 
    Private Sub BtnT3Next_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnT3Next.Click

        If WorksD.flg_Kiteiti = "1" Then
            '開く
            BtnT3Next_Open()
        Else
            '新規

            BtnT3Next_New()
        End If

    End Sub

    ''' 

    ''' 次へボタン
    ''' 新規の場合

    ''' 2013/05/24 作成
    ''' 

    ''' 
    Private Sub BtnT3Next_New()

        '画面の入力値を取得する

        GetDataFromToControl()

        '計測データ.mdbに接続する

        If ConnectDB(m_Keisoku_mdb_path, m_keisoku_dbclass) = False Then
            MsgBox("計測データ.mdbを開くことができません。")
            Exit Sub
        End If

        '計測データ.mdbに入力値を書き込む
        'KihonInfoの更新
        If SetKeisokuData(m_Keisoku_mdb_path) = False Then
            DisConnectDB(m_keisoku_dbclass)
            MsgBox("計測データ更新に失敗しました。", MsgBoxStyle.OkOnly, "確認")
            Exit Sub
        End If

        '寸法算出定義データ(SunpoSet)更新
        If SaveSunpo() = False Then
            DisConnectDB(m_keisoku_dbclass)
            MsgBox("計測データ更新に失敗しました。", MsgBoxStyle.OkOnly, "確認")
            Exit Sub
        End If

        '計測データ.mdbを切断する
        DisConnectDB(m_keisoku_dbclass)

        '規定値処理フラグを更新する
        WorksD.SetKiteitiFlg(False)

        '画像画面へ
        SetGazou()

    End Sub

    ''' 

    ''' 次へボタン
    ''' 開くの場合

    ''' 2013/05/24 作成
    ''' 

    ''' 
    Private Sub BtnT3Next_Open()

        '画面の入力値を取得する

        GetDataFromToControl()

        '計測データ.mdbに接続する

        If ConnectDB(m_Keisoku_mdb_path, m_keisoku_dbclass) = False Then
            MsgBox("計測データ.mdbを開くことができません。")
            Exit Sub
        End If

        '計測データ.mdbに入力値を書き込む
        'KihonInfoの更新
        If SetKeisokuData(m_Keisoku_mdb_path) = False Then
            DisConnectDB(m_keisoku_dbclass)
            MsgBox("計測データ更新に失敗しました。", MsgBoxStyle.OkOnly, "確認")
            Exit Sub
        End If

        '寸法算出定義データ(SunpoSet)更新
        If SaveSunpo() = False Then
            DisConnectDB(m_keisoku_dbclass)
            MsgBox("計測データ更新に失敗しました。", MsgBoxStyle.OkOnly, "確認")
            Exit Sub
        End If

        '計測データ.mdbを切断する
        DisConnectDB(m_keisoku_dbclass)

        '画像画面へ
        m_gazou_event = False
        TabControl1.SelectedIndex = 2
        m_gazou_event = True
        TxtGazouFolder.Text = m_koji_kanri_path
        RbtnGazou.IsChecked = True

        ''画像画面へ
        SetGazou()

    End Sub

#End Region

#Region "画像画面"

    ''' 

    ''' メモリカードフォルダを選択する

    ''' 2013/05/17 作成
    ''' 

    ''' 
    Private Sub BtnMemoryCard_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnMemoryCard.Click

        ''FolderBrowserDialogクラスのインスタンスを作成
        'Dim fbd As New FolderBrowserDialog

        'With fbd

        '    '上部に表示する説明テキストを指定する

        '    .Description = "フォルダを指定してください。"
        '    'ルートフォルダを指定する(デフォルトでDesktop)
        '    .RootFolder = Environment.SpecialFolder.Desktop
        '    '最初に選択するフォルダを指定する

        '    'RootFolder以下にあるフォルダである必要がある
        '    .SelectedPath = TxtMemoryCard.Text.Trim
        '    'ユーザーが新しいフォルダを作成できるようにする(デフォルトでTrue)
        '    .ShowNewFolderButton = False

        '    'ダイアログを表示する
        '    Dim iDialogResult As DialogResult
        '    iDialogResult = .ShowDialog(Me)
        '    If iDialogResult = DialogResult.OK Then
        '        'OKボタン
        '        '選択されたフォルダを設定する

        '        TxtMemoryCard.Text = .SelectedPath
        '    Else
        '        Exit Sub
        '    End If

        'End With
#If True Then
        With m_fod
            '.InitialDirectory = YCM_GetFolderPath_fromFullPath(TxtMemoryCard.Text.Trim)
            If My.Settings.GazouSaveType = 1 Then
                .InitialDirectory = My.Settings.strGazouPath
            Else
                .InitialDirectory = GetMemoryCardFolder()
            End If

            Dim iDialogResult As System.Windows.Forms.DialogResult
            iDialogResult = .ShowDialog()
            If iDialogResult = System.Windows.Forms.DialogResult.OK Then
                'OKボタン
                '選択されたフォルダを設定する
                TxtMemoryCard.Text = YCM_GetFolderPath_fromFullPath(.FileName)
                My.Settings.strGazouPath = YCM_GetFolderPath_fromFullPath(.FileName)
                My.Settings.GazouSaveType = 1
            Else
                Exit Sub
            End If
        End With
#Else
           With m_fbd_s

            '上部に表示する説明テキストを指定する

            .Description = "フォルダを指定してください。"
            '最初に選択するフォルダを指定する

            'RootFolder以下にあるフォルダである必要がある
            .SelectedPath = TxtMemoryCard.Text.Trim

            'ダイアログを表示する
            Dim iDialogResult As System.Windows.Forms.DialogResult
            iDialogResult = .ShowDialog()
            If iDialogResult = System.Windows.Forms.DialogResult.OK Then
                'OKボタン
                '選択されたフォルダを設定する

                TxtMemoryCard.Text = .SelectedPath
            Else
                Exit Sub
            End If

        End With
#End If
     

        '画像一覧に表示する
        GazouView(TxtMemoryCard.Text)

    End Sub

    ''' 

    ''' 画像フォルダを選択する

    ''' 2013/05/17 作成
    ''' 

    ''' 
    Private Sub BtnGazouFolder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGazouFolder.Click

        ''FolderBrowserDialogクラスのインスタンスを作成
        'Dim fbd As New FolderBrowserDialog

        'With fbd

        '    '上部に表示する説明テキストを指定する

        '    .Description = "フォルダを指定してください。"
        '    'ルートフォルダを指定する(デフォルトでDesktop)
        '    .RootFolder = Environment.SpecialFolder.Desktop
        '    '最初に選択するフォルダを指定する

        '    'RootFolder以下にあるフォルダである必要がある
        '    .SelectedPath = TxtGazouFolder.Text.Trim
        '    'ユーザーが新しいフォルダを作成できるようにする(デフォルトでTrue)
        '    .ShowNewFolderButton = False

        '    'ダイアログを表示する
        '    Dim iDialogResult As DialogResult
        '    iDialogResult = .ShowDialog(Me)
        '    If iDialogResult = DialogResult.OK Then
        '        'OKボタン
        '        '選択されたフォルダを設定する

        '        TxtGazouFolder.Text = .SelectedPath
        '    Else
        '        Exit Sub
        '    End If

        'End With
#If True Then
        With m_fod
            .InitialDirectory = YCM_GetFolderPath_fromFullPath(TxtGazouFolder.Text.Trim)
            If My.Settings.GazouSaveType = 0 Then
                .InitialDirectory = My.Settings.strGazouPath
            Else
                .InitialDirectory = My.Settings.DefaultSavePath
            End If

            'ダイアログを表示する
            Dim iDialogResult As System.Windows.Forms.DialogResult
            iDialogResult = .ShowDialog()
            If iDialogResult = System.Windows.Forms.DialogResult.OK Then
                'OKボタン
                '選択されたフォルダを設定する
                TxtGazouFolder.Text = YCM_GetFolderPath_fromFullPath(.FileName)
                My.Settings.strGazouPath = TxtGazouFolder.Text
                My.Settings.GazouSaveType = 0
                My.Settings.Save()

            Else
                Exit Sub
            End If
        End With
#Else
          With m_fbd_s

            '上部に表示する説明テキストを指定する

            .Description = "フォルダを指定してください。"
            '最初に選択するフォルダを指定する

            'RootFolder以下にあるフォルダである必要がある
            .SelectedPath = TxtGazouFolder.Text.Trim

            'ダイアログを表示する
            Dim iDialogResult As System.Windows.Forms.DialogResult
            iDialogResult = .ShowDialog()
            If iDialogResult = System.Windows.Forms.DialogResult.OK Then
                'OKボタン
                '選択されたフォルダを設定する

                TxtGazouFolder.Text = .SelectedPath
            Else
                Exit Sub
            End If

        End With
#End If
      

        '画像一覧に表示する
        GazouView(TxtGazouFolder.Text)

    End Sub

    ''' 

    ''' 前へボタン
    ''' 2013/05/21 作成
    ''' 

    ''' 
    Private Sub BtnT4Back_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnT4Back.Click

        '(20140527 Tezuka Change)
        If m_flg_Zahyo = True Then
            '基本情報
            SetKihon()
        Else
            '規定値画面へ
            SetKiteiti()
        End If
        'Else
        ''基本画面へ
        'SetKihon()
        'End If

    End Sub

    ''' 

    ''' 次へボタン
    ''' 2013/05/21 作成
    ''' 

    ''' 
    Private Sub BtnT4Next_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles BtnT4Next.Click

        'End Sub

        'Private Sub BtnT4Next_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnT4Next.Click

        ''H25.7.3修正前 Yamada================
        'If WorksD.flg_Gazou = "1" Then
        '    '開く
        '    BtnT4Next_Open()
        'Else
        '    '新規

        '    BtnT4Next_New()
        'End If
        ''H25.7.3修正前 Yamada================

        Dim TrgtThreshold As Integer = 0

        If RadioAout.IsChecked = True Then
            TrgtThreshold = -1
        Else
            If Not Integer.TryParse(TxtTrgtThreshold.Text, TrgtThreshold) Then
                MsgBox("閾値に整数以外の文字が入力されています。整数を入力し直してください。")
                Exit Sub
            End If

            If TrgtThreshold > 255 Or TrgtThreshold < 0 Then
                MsgBox("閾値が範囲外です。0≦閾値≦255の範囲で入力してください。")
                Exit Sub
            End If
        End If

        WritePrivateProfileString("Kaiseki", "TargetThreshold", TrgtThreshold.ToString, My.Application.Info.DirectoryPath & "\vform.ini")

        'H25.7.3修正 Yamada================
        If WorksD.flg_Gazou = "1" Then
            '開く
            BtnT4Next_Open(False)
        Else
            '新規

            BtnT4Next_New(False)
        End If
        'H25.7.3修正 Yamada================
    End Sub

    ''' 

    ''' 次へボタン
    ''' 新規の場合

    ''' 2013/05/24 作成
    ''' 

    ''' 
    '(入力)blnFrmOut：「次へ」はFalse/「検査表出力」はTrue

    Private Sub BtnT4Next_New(ByVal blnFrmOut As Boolean) 'H26.6.28Yamada修正
        'Private Sub BtnT4Next_New()
        '画像

        'MsgBox(System.Reflection.MethodBase.GetCurrentMethod.Name + " sta")


        If ListView2.Items.Count > 0 Then

            If RbtnMemoryCard.IsChecked = True Then
                m_gazou_path = TxtMemoryCard.Text.Trim
            Else
                m_gazou_path = TxtGazouFolder.Text.Trim
            End If

            '(20141023 Tezuka ADD) ボタンを操作不可にする
            BtnT4_FrmOut.IsEnabled = False
            BtnT4Next.IsEnabled = False
            BtnT4Back.IsEnabled = False

            If blnFrmOut = False Then 'H25.6.28　Yamada修正
                Dim msgR As MsgBoxResult = MsgBox("指定された画像フォルダの画像を移動させますか？", MsgBoxStyle.YesNoCancel, "確認")
                If msgR = MsgBoxResult.Yes Then
                    '画像ファイルを工事データフォルダに移動する

                    MoveFolder(m_gazou_path, m_koji_kanri_path)
                ElseIf msgR = MsgBoxResult.No Then
                    '画像ファイルを工事データフォルダにコピーする
                    CopyFolder(m_gazou_path, m_koji_kanri_path)

                    'ADD BY Yamada 20150303 Sta --------------キャンセルの際にエラー 
                ElseIf msgR = MsgBoxResult.Cancel Then
                    'キャンセル
                    BtnT4_FrmOut.IsEnabled = True
                    BtnT4Next.IsEnabled = True
                    BtnT4Back.IsEnabled = True
                    m_gazou_path = Nothing
                    Exit Sub
                    'ADD BY Yamada 20150303 Sta --------------キャンセルの際にエラー 

                End If
            Else
                '画像ファイルを工事データフォルダにコピーする
                CopyFolder(m_gazou_path, m_koji_kanri_path)
            End If

            '(20141023 Tezuka ADD) ボタンを操作可能にする
            BtnT4_FrmOut.IsEnabled = True
            BtnT4Next.IsEnabled = True
            BtnT4Back.IsEnabled = True

            ''計測データmdbを接続する

            'If ConnectDB(m_Keisoku_mdb_path, m_keisoku_dbclass) = False Then
            '    MsgBox("計測データ.mdbを開くことができません。")
            '    Exit Sub
            'End If

            'カメラ情報を更新する
            If UpdateCameraInfo() = False Then
                MsgBox("計測データ.mdbを開くことができません。")
                Exit Sub
            End If

            ''計測データmdbを接続解除する
            'DisConnectDB(m_keisoku_dbclass)

            '画像処理フラグを更新する
            WorksD.SetGazouFlg(False)

            '解析画面へ
            'SetKaiseki()'H25.7.1修正前Yamada
            If blnFrmOut = False Then 'H25.7.1　Yamada修正
                SetKaiseki(False)
                KaisekiRun()
            End If
        Else
            MsgBox("画像一覧が選択されていません。", MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        'MsgBox(System.Reflection.MethodBase.GetCurrentMethod.Name)


    End Sub

    ''' 

    ''' 次へボタン
    ''' 開くの場合

    ''' 2013/05/24 作成
    ''' 

    ''' 

    ''H25.7.3修正　Yamada
    'Private Sub BtnT4Next_Open()
    Private Sub BtnT4Next_Open(ByVal blnFrmOut As Boolean)

        '計測データmdbのカメラ情報を更新する
        UpdateCameraInfo()
        ''H25.7.3修正前　Yamada
        ''解析画面へ
        'SetKaiseki(False)

        'H25.7.3修正　Yamada
        If blnFrmOut = False Then
            SetKaiseki(False)
        End If

    End Sub

    Private Sub CopyFolder(ByVal iFolder1 As String, ByVal iFolder2 As String)

        If System.IO.Directory.Exists(iFolder1) = True Then
            Dim jpgFiles As String() = _
                        System.IO.Directory.GetFiles(iFolder1, "*.jpg")

            If jpgFiles.Length > 0 Then

                Dim iCnt As Integer = 100 / jpgFiles.Length

                '(20141023 Tezuka ADD) プログレスバーの表示
                gfrmProgressBar = New frmProgressBar
                gfrmProgressBar.Show()
                gfrmProgressBar.ProgressBar1.Maximum = 100
                gfrmProgressBar.ProgressBar1.Value = 0
                gfrmProgressBar.Label1.Text = "画像コピー中･･･"
                gfrmProgressBar.Refresh()
                Dim ii As Integer = 0

                For Each t As String In jpgFiles
                    System.IO.File.Copy(t, iFolder2 & "\" & System.IO.Path.GetFileName(t), True)

                    '(20141023 Tezuka ADD) プログレスバーカウントアップ
                    ii += iCnt
                    If ii >= 100 Then ii = 100
                    gfrmProgressBar.ProgressBar1.Value = ii
                    gfrmProgressBar.Refresh()
                Next

                '(20141023 Tezuka ADD) プログレスバーの消去
                System.Threading.Thread.Sleep(1000)
                gfrmProgressBar.Close()


                'SUURI ADD 20141223 
                jpgFiles = System.IO.Directory.GetFiles(iFolder1, "*.mdb")
                For Each t As String In jpgFiles
                    If "dbFBM.mdb" = System.IO.Path.GetFileName(t) Then 'ここに問題があった。計測データ.mdbがある場合はコピーしないようにする。
                        System.IO.File.Copy(t, iFolder2 & "\" & System.IO.Path.GetFileName(t), True)
                    End If
                Next

            End If
        End If
    End Sub

    Private Sub MoveFolder(ByVal iFolder1 As String, ByVal iFolder2 As String)

        If System.IO.Directory.Exists(iFolder1) = True Then
            Dim jpgFiles As String() = _
              System.IO.Directory.GetFiles(iFolder1, "*.jpg")

            If jpgFiles.Length > 0 Then
                Dim iCnt As Integer = 100 / jpgFiles.Length

                '(20141023 Tezuka ADD) プログレスバーの表示
                gfrmProgressBar = New frmProgressBar
                gfrmProgressBar.Show()
                gfrmProgressBar.ProgressBar1.Maximum = 100
                gfrmProgressBar.ProgressBar1.Value = 0
                gfrmProgressBar.Label1.Text = "画像移動中･･･"
                gfrmProgressBar.Refresh()
                Dim ii As Integer = 0

                For Each t As String In jpgFiles
                    System.IO.File.Move(t, iFolder2 & "\" & System.IO.Path.GetFileName(t))

                    '(20141023 Tezuka ADD) プログレスバーカウントアップ
                    ii += iCnt
                    If ii >= 100 Then ii = 100
                    gfrmProgressBar.ProgressBar1.Value = ii
                    gfrmProgressBar.Refresh()
                Next

                '(20141023 Tezuka ADD) プログレスバーの消去
                System.Threading.Thread.Sleep(1000)
                gfrmProgressBar.Close()

                'SUURI ADD 20141223
                jpgFiles = System.IO.Directory.GetFiles(iFolder1, "*.mdb")
                For Each t As String In jpgFiles
                    System.IO.File.Move(t, iFolder2 & "\" & System.IO.Path.GetFileName(t))
                Next


            End If
        End If

    End Sub

    '''' 

    '''' カメラ情報を更新する
    '''' 2013/05/22 作成
    '''' 

    '''' 
    'Private Function UpdateCameraInfo() As Boolean

    '    'コントロールを設定する


    '    'With DataGridView7

    '    '    For iRow As Integer = 0 To WorksD.CameraL.Count - 1

    '    '        WorksD.CameraL(iRow).objControl = DataGridView7.Rows(iRow).Cells(0)
    '    '        WorksD.CameraL(iRow).GetDataFromControl()

    '    '    Next

    '    'End With

    '    For iRow As Integer = 0 To WorksD.CameraL.Count - 1

    '        'WorksD.CameraL(iRow).objControl = CmbCamera.Text
    '        'WorksD.CameraL(iRow).GetDataFromControl()

    '        If WorksD.CameraL(iRow).camera_name.Trim = CmbCamera.Text.Trim Then
    '            WorksD.CameraL(iRow).flg_use = "1"
    '        Else
    '            WorksD.CameraL(iRow).flg_use = "0"
    '        End If

    '    Next

    '    UpdateCameraInfo = True

    '    Dim bRet As Boolean = True

    '    '計測データmdbを接続する

    '    ConnectDB(m_Keisoku_mdb_path, m_keisoku_dbclass)

    '    '選択したカメラ情報を取得する

    '    'カメラ情報を計測データmdbに更新する
    '    For Each T As CameraInfoTable In WorksD.CameraL

    '        'If T.objControl.selected = True Then
    '        '    'チェックされたカメラ情報をWorksDにコピーする
    '        '    WorksD.CameraD = New CameraInfoTable
    '        '    WorksD.CameraD.copy(T)
    '        'End If

    '        If T.camera_name.Trim = CmbCamera.Text.Trim Then
    '            '選択されたカメラ情報をWorksDにコピーする
    '            WorksD.CameraD = New CameraInfoTable
    '            WorksD.CameraD.copy(T)
    '            T.flg_use = "1"
    '        Else
    '            T.flg_use = "0"
    '        End If

    '        '選択したカメラ情報のflg_useを更新する
    '        T.m_dbClass = m_keisoku_dbclass
    '        T.InsertData()

    '    Next

    '    '計測データmdbを接続解除する
    '    DisConnectDB(m_keisoku_dbclass)

    'End Function

    ''' 

    ''' カメラ情報を更新する
    ''' 2013/05/22 作成
    ''' 

    ''' 
    Private Function UpdateCameraInfo() As Boolean

        For iRow As Integer = 0 To WorksD.CameraL.Count - 1

            If WorksD.CameraL(iRow).camera_name.Trim = CmbCamera.Text.Trim Then
                WorksD.CameraL(iRow).flg_use = "1"
                WorksD.CameraD = WorksD.CameraL(iRow)
            Else
                WorksD.CameraL(iRow).flg_use = "0"
            End If

        Next

        UpdateCameraInfo = True

        Dim bRet As Boolean = True

        '計測データmdbを接続する

        If ConnectDB(m_Keisoku_mdb_path, m_keisoku_dbclass) = False Then
            UpdateCameraInfo = False
            Exit Function
        End If

        '選択したカメラ情報を取得する

        'カメラ情報を計測データmdbに更新する
        For Each T As CameraInfoTable In WorksD.CameraL

            '選択したカメラ情報のflg_useを更新する
            T.m_dbClass = m_keisoku_dbclass
            T.SaveData()

        Next

        '計測データmdbを接続解除する
        DisConnectDB(m_keisoku_dbclass)

    End Function

    ''' 

    ''' メモリーカード画像フォルダを指定する

    ''' ラジオボタンチェック時の処理

    ''' 2013/05/20 作成
    ''' 

    ''' 
    Private Sub RbtnMemoryCard_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RbtnMemoryCard.Click

        'If m_gazou_event = False Then Exit Sub

        If RbtnMemoryCard.IsChecked = True Then

            '(20141031 Tezuka ADD) テキストエリア、参照ボタンの制限
            BtnGazouFolder.IsEnabled = False
            TxtGazouFolder.IsEnabled = False
            BtnMemoryCard.IsEnabled = True
            TxtMemoryCard.IsEnabled = True

            m_gazou_path = TxtMemoryCard.Text

            GazouView(m_gazou_path)

        End If

    End Sub

    ''' 

    ''' 画像フォルダを指定する

    ''' ラジオボタンチェック時の処理

    ''' 2013/05/20 作成
    ''' 

    ''' 
    Private Sub RbtnGazou_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RbtnGazou.Click

        'If m_gazou_path = TxtGazouFolder.Text Then Exit Sub

        'If m_gazou_event = False Then Exit Sub

        If RbtnGazou.IsChecked = True Then

            '(20141031 Tezuka ADD) テキストエリア、参照ボタンの制限
            BtnGazouFolder.IsEnabled = True
            TxtGazouFolder.IsEnabled = True
            BtnMemoryCard.IsEnabled = False
            TxtMemoryCard.IsEnabled = False

            If m_gazou_path = TxtGazouFolder.Text Then Exit Sub

            m_gazou_path = TxtGazouFolder.Text

            GazouView(m_gazou_path)

        End If

    End Sub

    ''' 

    ''' 画像一覧表示処理

    ''' 2013/05/20 作成
    ''' 

    ''' 
    Private Sub GazouView(ByVal iFolderName As String)

        With ListView2

            '.BackgroundImage = Nothing

            .Items.Clear()

            Dim imageDir As String = iFolderName.Trim        ' 画像ディレクトリ
            If System.IO.Directory.Exists(imageDir) = True Then
                Dim jpgFiles As String() = _
                  System.IO.Directory.GetFiles(imageDir, "*.jpg")


                ''H25.修正前==================================================================
                'Dim width As Integer = 100
                'Dim height As Integer = 80

                'ImageList1.ImageSize = New Size(width, height)
                'ImageList1.Images.Clear()

                '.LargeImageList = ImageList1

                'For i As Integer = 0 To jpgFiles.Length - 1
                '    Dim original As Image = Bitmap.FromFile(jpgFiles(i))
                '    Dim thumbnail As Image = createThumbnail(original, width, height)
                '    ''_trans.jpgを表示する
                '    Dim strFileName As String = System.IO.Path.GetFileName(jpgFiles(i))
                '    If strFileName.Substring(12, strFileName.Length - 12) = "_trans.jpg" Then
                '        strFileName = strFileName.Replace("_trans.jpg", "")
                '    End If
                '    Dim iImgFile As String = i + 1 & ": " & strFileName

                '    ImageList1.Images.Add(thumbnail)
                '    .Items.Add(iImgFile, i)

                '    original.Dispose()
                '    'thumbnail.Dispose()
                'Next
                ''H25.修正前==================================================================


                For i As Integer = 0 To jpgFiles.Length - 1
                    Dim strFileName As String = System.IO.Path.GetFileName(jpgFiles(i))
                    Try
                        If strFileName.Substring(12, strFileName.Length - 12) = "_trans.jpg" Then
                            strFileName = strFileName.Replace("_trans.jpg", "")
                        End If
                    Catch ex As Exception

                    End Try
                
                    Dim iImgFile As String = i + 1 & ": " & strFileName
                    .Items.Add(iImgFile)
                Next
            End If
        End With

    End Sub

    ''' 

    ''' 画像一覧表示処理

    ''' JPEGファイル指定

    ''' 2013/05/20 作成
    ''' 

    ''' 
    Private Sub GazouListView(ByVal iJpgFiles As String())

        With ListView2

            '.BackgroundImage = Nothing

            .Items.Clear()

            Dim width As Integer = 100
            Dim height As Integer = 80

            ImageList1.ImageSize = New Size(width, height)
            ImageList1.Images.Clear()
            '.LargeImageList = ImageList1

            For i As Integer = 0 To iJpgFiles.Length - 1
                Dim original As Image = Bitmap.FromFile(iJpgFiles(i))
                Dim thumbnail As Image = createThumbnail(original, width, height)
                'Dim iImgFile As String = i + 1 & ": " & System.IO.Path.GetFileName(iJpgFiles(i))
                '_trans.jpgを表示する
                Dim strFileName As String = System.IO.Path.GetFileName(iJpgFiles(i))
                If strFileName.Substring(12, strFileName.Length - 12) = "_trans.jpg" Then
                    strFileName = strFileName.Replace("_trans.jpg", "")
                End If
                Dim iImgFile As String = i + 1 & ": " & strFileName

                ImageList1.Images.Add(thumbnail)
                .Items.Add(iImgFile)

                original.Dispose()
                'thumbnail.Dispose()
            Next

        End With

    End Sub

    ' 幅w、高さhのImageオブジェクトを作成
    Function createThumbnail(ByRef image As Image, ByVal w As Integer, ByVal h As Integer) As Image
        Dim canvas As New Bitmap(w, h)

        Dim g As Graphics = Graphics.FromImage(canvas)
        g.FillRectangle(New SolidBrush(Color.White), 0, 0, w, h)

        Dim fw As Double = CDbl(w) / CDbl(image.Width)
        Dim fh As Double = CDbl(h) / CDbl(image.Height)
        Dim scale As Double = Math.Min(fw, fh)

        Dim w2 As Integer = CInt(image.Width * scale)
        Dim h2 As Integer = CInt(image.Height * scale)

        g.DrawImage(image, (w - w2) \ 2, (h - h2) \ 2, w2, h2)
        g.Dispose()

        Return canvas
    End Function

    '''テストの為作成
    ''' あとで消す
    'Private Sub GazouListView1(ByVal iFolderName As String)

    '    With ListView2

    '        .BackgroundImage = Nothing

    '        .Items.Clear()


    '        Dim dir As String = iFolderName  ' 画像のあるディレクトリ
    '        Dim jpgFiles As String() = Directory.GetFiles(dir, "*.jpg")

    '        Dim imgconv As New ImageConverter()

    '        'Dim sw As Stopwatch = Stopwatch.StartNew()

    '        Dim iNo As Integer = 0

    '        For Each jpg As String In jpgFiles
    '            Console.WriteLine(jpg)

    '            Using fs As FileStream = File.OpenRead(jpg)

    '                ' 画像オブジェクトの作成
    '                Dim orig As Image = Image.FromStream(fs, False, False)

    '                Dim pils As Integer() = orig.PropertyIdList
    '                Dim index As Integer = Array.IndexOf(pils, &H501B) ' サムネイル・データ

    '                If index = -1 Then
    '                    Console.WriteLine("画像にサムネイルが含まれていません。")
    '                Else
    '                    ' サムネイル・データの取得

    '                    Dim pi As PropertyItem = orig.PropertyItems(index)
    '                    Dim jpgBytes As Byte() = pi.Value

    '                    ' サムネイルの作成
    '                    Dim thumbnail As Image _
    '                        = CType(imgconv.ConvertFrom(jpgBytes), Image)

    '                    Dim iImgFile As String = iNo + 1 & ": " & Path.GetFileName(jpg)
    '                    ' サムネイルの保存

    '                    thumbnail.Save(m_koji_kanri_path & "\tn_" + Path.GetFileName(jpg), _
    '                      System.Drawing.Imaging.ImageFormat.Jpeg)

    '                    ImageList1.Images.Add(thumbnail)
    '                    .Items.Add("tn_" + Path.GetFileName(jpg), iNo)
    '                    iNo += 1
    '                    'thumbnail.Dispose()
    '                End If
    '                'orig.Dispose()
    '            End Using
    '        Next
    '        'sw.Stop()
    '    End With

    'End Sub

    ''' 

    ''' 画像をダブルクリックしたら画像を開く
    ''' 2013/05/30 作成
    ''' 

    ''' 
    Private Sub ListView2_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles ListView2.MouseDoubleClick


        '項目が１つも選択されていない場合

        If ListView2.SelectedItems.Count = 0 Then
            '処理を抜ける

            Exit Sub
        End If

        Dim iSelectItem As String

        '1番目に選択されれいるアイテムをitemxに格納

        iSelectItem = ListView2.SelectedItems(0)
        Try
            Dim strPath As String = m_koji_kanri_path & "\" & iSelectItem.Split(" ")(1)
            Process.Start(strPath)
        Catch ex As Exception

        End Try

    End Sub


    'Private Sub DataGridView7_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView7.CurrentCellDirtyStateChanged

    '    With DataGridView7

    '        If .IsCurrentCellDirty = True Then
    '            'CommitEdit メソッドを呼ぶ事によってCellValueChanged イベントを発生させます。

    '            .CommitEdit(DataGridViewDataErrorContexts.Commit)
    '        End If

    '    End With

    'End Sub

    'Private Sub DataGridView7_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView7.CellValueChanged

    '    If flg_camera_grid = False Then

    '    Else
    '        If e.ColumnIndex = 0 Then
    '            flg_camera_grid = False
    '            If CBool(DataGridView7(e.ColumnIndex, e.RowIndex).Value) = True Then
    '                'チェック
    '                '全てのチェックを外す
    '                For i As Integer = 0 To DataGridView7.RowCount - 1
    '                    DataGridView7(0, i).Value = False
    '                Next
    '                'チェックをする

    '                DataGridView7(e.ColumnIndex, e.RowIndex).Value = True
    '                TxtCamera.Text = WorksD.CameraL(e.RowIndex).camera_name   'カメラ名

    '            Else
    '                'Noチェック
    '            End If
    '            flg_camera_grid = True
    '        End If
    '    End If

    'End Sub

    ''' 

    ''' 新規作成時、メモリカード（ﾘﾑｰﾊﾞﾙﾃﾞｨｽｸ）フォルダを取得する

    ''' 見つからない場合は、デフォルトのフォルダ
    ''' 2013/05/31 作成
    ''' 

    ''' 
    Private Function GetMemoryCardFolder()

        Dim strFolder As String = "E:\Image"

        '全ドライブ

        Dim drives As System.IO.DriveInfo() = System.IO.DriveInfo.GetDrives()

        For Each T As DriveInfo In drives

            If T.DriveType = DriveType.Removable Then

                strFolder = T.Name
                Exit For
            End If

        Next

        GetMemoryCardFolder = strFolder

    End Function

#End Region

#Region "解析画面"

    ''' 

    ''' 実行ボタン
    ''' 2013/05/21 作成
    ''' 

    ''' 
    Private Sub BtnT5Run_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnT5Run.Click
        KaisekiRun()
    End Sub

    Public Sub KaisekiRunSUE()
    
        '  Grid_MainFrame.Visibility = System.Windows.Visibility.Visible

        MainFrm = New YCM_MainFrame
        MainFrm.Tag = "1"
        '  Host_MainFrame.Child = MainFrm
        MainFrm.Show()
        YmdSleep()
        MainFrm.ChgViewImage1()
        YmdSleep()
        RunKaisekiProg()
        YmdSleep()


        ' Host_MainFrame.Child = MainFrm

        '画像処理フラグを更新する
        WorksD.SetKaisekiFlg(False)
        '解析データを一覧に表示する
        SetKaisekiDataToGrid()

        '寸法確認処理フラグを更新する
        WorksD.SetKakuninFlg(False)
        If ConnectDB(m_koji_kanri_path & "\" & m_Keisoku_mdb_name, m_keisoku_dbclass) = False Then

            MsgBox("計測データmdbへの接続に失敗しました。", MsgBoxStyle.OkOnly)
            Exit Sub

        End If

        m_keisoku_dbclass.DoDelete("SunpoSet")
        m_keisoku_dbclass.DisConnect()

        '寸法確認データを保存する
        SaveSunpoData(False)
        'open3DMainFrame()
        bln_KaisekiOpened = True
        '    draw3DMainFrame()

        '    BtnT5Run_MouseUp(Nothing, Nothing)
    End Sub
    'SUSANO ADD START 20160525
    Public Sub SetScaleAfterRun()
        MainFrm.objFBM.GenCommon3Dpoint()
        YCM_Offset_GenData2(MainFrm.objFBM) ' SUURI 20130523 ADD
        'YCM_Offset_GenDataNoCoordTrans(MainFrm.objFBM)
        MainFrm.objFBM.SaveToMeasureDataDB(MainFrm.objFBM.ProjectPath & "\")
        YCM_SaveUserFigureToDB()
        YMC_3DViewReDraw(m_strDataBasePath)
        MainFrm.FileSave()
        CalcSunpo()

        YmdSleep()

        ' Host_MainFrame.Child = MainFrm

        '画像処理フラグを更新する
        WorksD.SetKaisekiFlg(False)
        '解析データを一覧に表示する
        SetKaisekiDataToGrid()

        '寸法確認処理フラグを更新する
        WorksD.SetKakuninFlg(False)
        '寸法確認データを保存する
        SaveSunpoData(False)
        'open3DMainFrame()
        bln_KaisekiOpened = True
        ' draw3DMainFrame()
    End Sub
    'SUSANO ADD END 20160526
    Private Sub KaisekiRun()

        ''MsgBox(System.Reflection.MethodBase.GetCurrentMethod.Name + " sta")

        'YmdSleep() 'SetKaiseki()での処理が残っている可能性があり、vaio(win8)でNew YCM_MainFrameに失敗していそうなので、DoEvent実行。
        DispatcherHelper.DoEvents()

        TimeMonStart()
        'Yamada修正
        'RunKaisekiProg()
        Grid_MainFrame.Visibility = System.Windows.Visibility.Visible

        MainFrm = New YCM_MainFrame
        MainFrm.Tag = "1"
        Host_MainFrame.Child = MainFrm
        'MsgBox("MainFram show")

        'MsgBox(System.Reflection.MethodBase.GetCurrentMethod.Name + " befoer show")
        YmdSleep()
        MainFrm.Show()

        'MsgBox(System.Reflection.MethodBase.GetCurrentMethod.Name + " after")

        YmdSleep()
        'MsgBox("ChgViewImage1() sta")
        MainFrm.ChgViewImage1()
        YmdSleep()
        'MsgBox("RunKaisekiProg sta")

        'MsgBox(System.Reflection.MethodBase.GetCurrentMethod.Name + " befoer RunKaisekiProg")
        RunKaisekiProg()
        YmdSleep()

        ' Host_MainFrame.Child = MainFrm

        '画像処理フラグを更新する
        WorksD.SetKaisekiFlg(False)
        '解析データを一覧に表示する
        SetKaisekiDataToGrid()

        '寸法確認処理フラグを更新する
        WorksD.SetKakuninFlg(False)
        '寸法確認データを保存する
        SaveSunpoData(False)
        'open3DMainFrame()
        bln_KaisekiOpened = True
        TimeMonEnd()
        TimeMonOut(m_koji_kanri_path & "\TimeMonitorKekka.txt")

        draw3DMainFrame()


        'MsgBox(System.Reflection.MethodBase.GetCurrentMethod.Name + " done")

        '    BtnT5Run_MouseUp(Nothing, Nothing)

    End Sub
    ''' 

    ''' 実行ボタンのマウスボタンが離れた
    ''' 2013/06/07 作成
    ''' 

    ''' 
    Private Sub BtnT5Run_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles BtnT5Run.MouseUp

        ' View_Dilaog.MainProc()

    End Sub

    ''' 

    ''' 詳細確認ボタン
    ''' 2013/05/22 作成
    ''' 

    ''' 
    'ToDo 詳細確認がすでに表示されている状態で、この関数を呼ぶと不安定な動作をするため、修正が必要
    Private Sub BtnT5Kakunin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnT5Kakunin.Click
        Grid_MainFrame.Visibility = System.Windows.Visibility.Visible
        open3DMainFrame()
        bln_KaisekiOpened = True
        draw3DMainFrame()

    End Sub
    Private Sub open3DMainFrame()
        MainFrm = New YCM_MainFrame
        Host_MainFrame.Child = MainFrm
        RunReadKaisekiProg()

        SetKaisekiDataToGrid()
    End Sub
    Private Sub draw3DMainFrame() '3DView初期化
        View_Dilaog.MainProc()
    End Sub

    ''' 

    ''' 詳細確認ボタンのマウスボタンが離れた
    ''' 2013/06/07 作成
    ''' 

    ''' 
    Private Sub BtnT5Kakunin_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles BtnT5Kakunin.MouseUp

        draw3DMainFrame()

    End Sub

    ''' 

    ''' 前へボタン
    ''' 2013/05/21 作成
    ''' 

    ''' 
    Private Sub BtnT5Back_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnT5Back.Click

        '画像画面へ
        SetGazou()

        m_gazou_event = False

        '画像画面へ
        '(20140527 Tezuka コメント化)
        'If m_flg_Senyou = True Then
        '    TabControl1.SelectedIndex = 2
        'Else
        '    TabControl1.SelectedIndex = 1
        'End If
    End Sub

    ''' 

    ''' 次へボタン
    ''' 2013/05/21 作成
    ''' 

    ''' 
    Private Sub BtnT5Next_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnT5Next.Click

        '寸法確認処理フラグを更新する
        WorksD.SetKakuninFlg(False)

        '寸法確認データを保存する

        SaveSunpoData(False)

        '寸法確認画面へ
        SetSunpouKakunin()
        'KekkaKakuninKentou()

        '任意計測用寸法確認画面構築

        If m_flg_Sunpo = True Or m_flg_Zahyo = True Or m_flg_Zukei = True Then
            SupoKakuninGridDraw()
        End If

    End Sub
    Private Sub BtnKensaHyo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnKensaHyo.Click
        KensaHyoOutput.MeasurementSet = groupValue
        KensaHyoOutput.ShowDialog()
    End Sub

    ''' 

    ''' 解析データをグリッドに表示する
    ''' 2013/05/24 作成
    ''' 

    ''' 
    Private Sub SetKaisekiDataToGrid()
        TimeMonStart()

        With DataGridView8

            'ヘッダを中央
            .ColumnHeaderStyle = Application.Current.Resources("DataGrid_Header_Right")
            '.CellStyle = Application.Current.Resources("DataGrid_Cell_Right")

            '並び替え禁止
            For Each T As System.Windows.Controls.DataGridColumn In .Columns
                T.CanUserSort = False
            Next

            'ユーザーは行の追加・削除不可
            .CanUserAddRows = False
            .CanUserDeleteRows = False

            .ItemsSource = Nothing
        End With

        Dim iiB_3DOUT As Integer = GetPrivateProfileInt("Command", "B_3DOUT", 1, My.Application.Info.DirectoryPath & "\vform.ini")
        With m_KaisekiItems
            If iiB_3DOUT = 1 Then
                '#If B_3DOUT = "TRUE" Then   'ADD By Yamada 20140617 For 鉄道総研 
                .Clear()
                ''H25.5.25 Yamada
                'If WorksD.SunpoSetL.Count < 20 Then
                '    .Rows.Add(20)
                'Else
                '    .Rows.Add(WorksD.SunpoSetL.Count)
                'End If
                Dim iRowNo As Integer = 0

                For Each C3DCT As FBMlib.Common3DCodedTarget In MainFrm.objFBM.lstCommon3dCT
                    Dim flgFirst As Boolean = True
                    For Each P3D As FBMlib.Point3D In C3DCT.lstRealP3d
                        If flgFirst = True Then
                            .Add(New KaisekiItem)
                            .Item(iRowNo).SokutenName = C3DCT.currentLabel                   '測点名

                            .Item(iRowNo).X = CDbl(P3D.X.D).ToString("N2") '.ToString("###0.0")     'X
                            .Item(iRowNo).Y = CDbl(P3D.Y.D).ToString("N2") '.ToString("###0.0")     'Y
                            .Item(iRowNo).Z = CDbl(P3D.Z.D).ToString("N2") '.ToString("###0.0")     'Z
                            iRowNo += 1
                            flgFirst = False
                        End If
                    Next
                Next
                Try
                    For Each C3DST As FBMlib.Common3DSingleTarget In MainFrm.objFBM.lstCommon3dST
                        .Add(New KaisekiItem)
                        .Item(iRowNo).SokutenName = C3DST.currentLabel                   '測点名

                        .Item(iRowNo).X = CDbl(C3DST.realP3d.X.D).ToString("N2") '.ToString("###0.0")     'X
                        .Item(iRowNo).Y = CDbl(C3DST.realP3d.Y.D).ToString("N2") '.ToString("###0.0")     'Y
                        .Item(iRowNo).Z = CDbl(C3DST.realP3d.Z.D).ToString("N2") '.ToString("###0.0")     'Z
                        iRowNo += 1
                    Next
                Catch ex As Exception

                End Try
                '#End If
            End If
        End With

        DataGridView8.ItemsSource = m_KaisekiItems
        TimeMonEnd()
    End Sub

#End Region

#Region "寸法確認画面"

    ''' 

    ''' 前へボタン
    ''' 2013/05/21 作成
    ''' 

    ''' 
    Private Sub BtnT6Back_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BtnT6Back.Click

        '解析画面へ
        SetKaiseki(True)

    End Sub

    ''' 

    ''' 次へボタン
    ''' 2013/05/21 作成
    ''' 

    ''' 
    Private Sub BtnT6Next_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BtnT6Next.Click

        '寸法確認データを保存する

        SaveSunpoData(False)

        '検査表出力画面へ
        SetOut()

    End Sub

#End Region

#Region "検査表出力画面"

    ''' 

    ''' 前へボタン
    ''' 2013/05/21 作成
    ''' 

    ''' 
    'Private Sub BtnT7Back_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnT7Back.Click

    '    '寸法確認画面へ
    '    SetSunpouKakunin()

    'End Sub

    ''' 

    ''' Excel出力先
    ''' 参照ボタン
    ''' 2013/05/21 作成
    ''' 

    ''' 
    '    Private Sub BtnExcelFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnExcelFolder.Click

    '        ''FolderBrowserDialogクラスのインスタンスを作成
    '        'Dim fbd As New FolderBrowserDialog

    '        'With fbd

    '        '    '上部に表示する説明テキストを指定する

    '        '    .Description = "フォルダを指定してください。"
    '        '    'ルートフォルダを指定する(デフォルトでDesktop)
    '        '    .RootFolder = Environment.SpecialFolder.Desktop
    '        '    '最初に選択するフォルダを指定する

    '        '    'RootFolder以下にあるフォルダである必要がある
    '        '    .SelectedPath = TxtExcelFolder.Text.Trim
    '        '    'ユーザーが新しいフォルダを作成できるようにする(デフォルトでTrue)
    '        '    .ShowNewFolderButton = True

    '        '    'ダイアログを表示する
    '        '    Dim iDialogResult As DialogResult
    '        '    iDialogResult = .ShowDialog(Me)
    '        '    If iDialogResult = DialogResult.OK Then
    '        '        '選択されたフォルダを表示する
    '        '        TxtExcelFolder.Text = .SelectedPath
    '        '    Else
    '        '        Exit Sub
    '        '    End If

    '        'End With

    '#If True Then 'SUURI ADD 20140915
    '        With m_fod
    '            .InitialDirectory = TxtExcelFolder.Text.Trim
    '            .FileName = TxtOutFileName.Text

    '            'ダイアログを表示する
    '            Dim iDialogResult As System.Windows.Forms.DialogResult
    '            iDialogResult = .ShowDialog()
    '            If iDialogResult = System.Windows.Forms.DialogResult.OK Then
    '                '選択されたフォルダを表示する
    '                TxtExcelFolder.Text = YCM_GetFolderPath_fromFullPath(.FileName)
    '            Else
    '                Exit Sub
    '            End If

    '        End With
    '#Else
    '         With m_fbd
    '            '上部に表示する説明テキストを指定する

    '            .Description = "フォルダを指定してください。"
    '            '最初に選択するフォルダを指定する

    '            'RootFolder以下にあるフォルダである必要がある
    '            .SelectedPath = TxtExcelFolder.Text.Trim

    '            'ダイアログを表示する
    '            Dim iDialogResult As System.Windows.Forms.DialogResult
    '            iDialogResult = .ShowDialog()
    '            If iDialogResult = System.Windows.Forms.DialogResult.OK Then
    '                '選択されたフォルダを表示する
    '                TxtExcelFolder.Text = .SelectedPath
    '            Else
    '                Exit Sub
    '            End If

    '        End With
    '#End If


    '    End Sub

    ''' 

    ''' Excel出力先
    ''' 出力ボタン
    ''' 2013/05/21 作成
    ''' 

    ''' 
    'Private Sub BtnExcelOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnExcelOut.Click

    '    '検査表の出力

    '    If SetKensaOut() = False Then
    '        Exit Sub
    '    End If

    '    '検査表出力処理フラグを更新する
    '    WorksD.SetOutFlg(False)

    '    'MsgBox("検査表を出力しました。", MsgBoxStyle.OkOnly)

    'End Sub

    ''' 

    ''' 検査表の出力(Excel出力)
    ''' 2013/05/27 作成
    ''' 

    ''' 
    'Public Function SetKensaOut() As Boolean

    '    SetKensaOut = True

    '    '出力先のフォルダ存在チェック
    '    If System.IO.Directory.Exists(TxtExcelFolder.Text) = False Then
    '        SetKensaOut = False
    '        MsgBox("出力先フォルダが存在しません。")
    '        Exit Function
    '    End If

    '    If CommonTypeID = 24 Then
    '        Sunpo_CsvOut(TxtExcelFolder.Text.Trim, TxtOutFileName.Text.Trim)
    '        Exit Function
    '    End If

    '    'テンプレートの入力チェック
    '    If CmbExcelTemplate.Text.Trim = "" Then
    '        SetKensaOut = False
    '        MsgBox("テンプレートを選択して下さい。")
    '        Exit Function
    '    End If

    '    m_TemplateExcelFile = WorksD.ExcelTemplateL(0).TemplateFileName & ".xls"

    '    Try
    '        'テンプレートを出力先にコピーする
    '        System.IO.File.Copy(m_TemplatePath & "\" & m_TemplateExcelFile,
    '                            TxtExcelFolder.Text & "\" & TxtOutFileName.Text & ".xls", True)

    '    Catch ex As System.IO.IOException
    '        '既に開かれている可能性あり
    '        SetKensaOut = False
    '        Exit Function

    '    Catch ex As Exception
    '        SetKensaOut = False
    '        Exit Function
    '    End Try

    '    ''出力Excelファイル名の存在チェックを行う
    '    'If System.IO.File.Exists(TxtExcelFolder.Text & "\" & TxtOutFileName.Text & ".xls") = True Then
    '    '    SetKensaOut = True
    '    '    Exit Function
    '    'End If

    '    ''テンプレートを出力先にコピーする
    '    'System.IO.File.Copy(m_TemplatePath & "\" & m_TemplateExcelFile,
    '    '                    TxtExcelFolder.Text & "\" & TxtOutFileName.Text & ".xls", True)

    '    '各項目をExcelに設定する

    '    SetMessageToExcel(TxtExcelFolder.Text & "\" & TxtOutFileName.Text & ".xls")

    'End Function

#End Region

#If True Then




#Region "デバイス認識"

    Private Const WM_DEVICECHANGE As Integer = &H219

    Private Const DBT_DEVICEARRIVAL As Integer = &H8000

    Private Const DBT_DEVICEREMOVECOMPLETE As Integer = &H8004

    Private Const DBT_DEVTYP_VOLUME As Integer = &H2  '  

    '  

    'Get the information about the detected volume.  

    Private Structure DEV_BROADCAST_VOLUME

        Dim Dbcv_Size As Integer

        Dim Dbcv_Devicetype As Integer

        Dim Dbcv_Reserved As Integer

        Dim Dbcv_Unitmask As Integer

        Dim Dbcv_Flags As Short

    End Structure




    Private Sub Window1_SourceInitialized(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SourceInitialized
        Dim handle As System.IntPtr = (New System.Windows.Interop.WindowInteropHelper(Me)).Handle
        System.Windows.Interop.HwndSource.FromHwnd(handle).AddHook(New System.Windows.Interop.HwndSourceHook(AddressOf WndProc))
    End Sub

    'Protected Overrides Sub WndProc(ByRef M As System.Windows.Forms.Message)
    Private Function WndProc(ByVal hwnd As System.IntPtr,
                          ByVal msg As Integer,
                          ByVal wParam As System.IntPtr,
                          ByVal lParam As System.IntPtr,
                          ByRef handled As Boolean) As System.IntPtr
        'デバイスの装着を認識する


        If msg = WM_DEVICECHANGE Then

            Select Case wParam

                'デバイスが装着されたかチェックする

                Case DBT_DEVICEARRIVAL
                    If TabControl1.SelectedIndex = 3 Then
                        Dim DevType As Integer = Runtime.InteropServices.Marshal.ReadInt32(lParam, 4)

                        If DevType = DBT_DEVTYP_VOLUME Then

                            Dim Vol As New DEV_BROADCAST_VOLUME

                            Vol = Runtime.InteropServices.Marshal.PtrToStructure(lParam, GetType(DEV_BROADCAST_VOLUME))

                            If Vol.Dbcv_Flags = 0 Then

                                For i As Integer = 0 To 20

                                    If Math.Pow(2, i) = Vol.Dbcv_Unitmask Then

                                        Dim Usb As String = Chr(65 + i) + ":\"

                                        'USBメモリが差し込まれた

                                        'USBメモリからJPGファイルの存在をチェックする

                                        Dim iJpgFiles As String() = System.IO.Directory.GetFiles(Usb.ToString, "*.jpg", System.IO.SearchOption.AllDirectories)

                                        'JPGファイルがあったら、画像を取り込む
                                        If iJpgFiles.Length > 0 Then
                                            If MsgBox("USBメモリが認識されました。" & vbCrLf & "画像を取り込みますか？", MsgBoxStyle.YesNo, "確認") = MsgBoxResult.Yes Then
                                                GazouListView(iJpgFiles)
                                                SetGazou()
                                                RbtnMemoryCard.IsChecked = True
                                                RbtnGazou.IsChecked = False
                                                TxtMemoryCard.Text = Usb
                                            End If
                                            'Else
                                            '    MsgBox("USBメモリが認識されました。", MsgBoxStyle.OkOnly, "確認")
                                        End If
                                        'My.Computer.FileSystem.Drives
                                        Exit For

                                    End If

                                Next

                            End If

                        End If
                    End If
                    'デバイスが取り外されたかチェックする

                    handled = True
                Case DBT_DEVICEREMOVECOMPLETE

                    Dim DevType As Integer = Runtime.InteropServices.Marshal.ReadInt32(lParam, 4)

                    If DevType = DBT_DEVTYP_VOLUME Then

                        Dim Vol As New DEV_BROADCAST_VOLUME

                        Vol = Runtime.InteropServices.Marshal.PtrToStructure(lParam, GetType(DEV_BROADCAST_VOLUME))

                        If Vol.Dbcv_Flags = 0 Then

                            For i As Integer = 0 To 20

                                If Math.Pow(2, i) = Vol.Dbcv_Unitmask Then

                                    Dim Usb As String = Chr(65 + i) + ":\"

                                    'USBメモリが取り外された

                                    Exit For

                                End If

                            Next

                        End If

                    End If

                    handled = True
            End Select

        End If

        'MyBase.WndProc(M)

    End Function

#End Region

#End If

#Region "Excel出力設定"

    'Public BeforePID As Integer()
    'Public MyPID As Integer

    ''' 

    ''' Excel出力設定

    ''' 2013/05/27 作成
    ''' 

    ''' パス
    ''' 
    'Public Sub SetMessageToExcel(ByVal strFilePath As String)

    '    Dim xlApp As Excel.Application
    '    Dim xlBook As Excel.Workbook
    '    Dim xlSheet As Excel.Worksheet

    '    Dim strFilename As String                             'ファイル名(フルパス)
    '    strFilename = strFilePath                             'ファイル名をセット

    '    Dim strSheetName As String                            'シート名
    '    strSheetName = "Sheet1"

    '    Dim xlBooks As Excel.Workbooks
    '    Dim xlSheets As Excel.Sheets

    '    Try

    '        GetExcelPID()
    '        xlApp = New Excel.Application
    '        GetExcelNewPID()
    '        xlBooks = xlApp.Workbooks
    '        xlBook = xlBooks.Open(strFilename)
    '        xlSheets = xlBook.Worksheets
    '        xlSheet = xlSheets.Item(strSheetName)

    '        '基本情報を設定する

    '        For i As Integer = 0 To WorksD.KihonL.Count - 1
    '            Try
    '                Dim xlRangeItem As Excel.Range
    '                xlRangeItem = xlSheet.Range(WorksD.KihonL(i).item_cell_name)
    '                xlRangeItem.Value = WorksD.KihonL(i).item_value
    '                MRComObject(xlRangeItem)
    '            Catch ex As Exception
    '            End Try
    '        Next

    '        For i As Integer = 0 To WorksD.SunpoSetCellL.Count - 1
    '            Dim indexSunposet As Integer = -1

    '            Dim t As Integer = 0
    '            Dim intSunpoId As Integer = WorksD.SunpoSetCellL(i).SunpoID

    '            For Each SSL As SunpoSetTable In WorksD.SunpoSetL

    '                If SSL.SunpoID = intSunpoId Then
    '                    indexSunposet = t
    '                End If
    '                t += 1
    '            Next
    '            If WorksD.SunpoSetCellL(i).SunpoMark = "g" Then
    '                Dim s As Integer = -1
    '            End If
    '            If WorksD.SunpoSetCellL(i).CT_Active = "1" Then

    '                '規定値
    '                Try
    '                    Dim xlRangeVal As Excel.Range
    '                    xlRangeVal = xlSheet.Range(WorksD.SunpoSetCellL(i).KiteiVal)
    '                    xlRangeVal.Value = WorksD.SunpoSetL(indexSunposet).KiteiVal
    '                    MRComObject(xlRangeVal)
    '                Catch ex As Exception
    '                End Try
    '                '最小許容値
    '                Try
    '                    Dim xlRangeMin As Excel.Range
    '                    xlRangeMin = xlSheet.Range(WorksD.SunpoSetCellL(i).KiteiMin)
    '                    xlRangeMin.Value = WorksD.SunpoSetL(indexSunposet).KiteiMin
    '                    MRComObject(xlRangeMin)
    '                Catch ex As Exception
    '                End Try
    '                '最大許容値
    '                Try
    '                    Dim xlRangeMax As Excel.Range
    '                    xlRangeMax = xlSheet.Range(WorksD.SunpoSetCellL(i).KiteiMax)
    '                    xlRangeMax.Value = WorksD.SunpoSetL(indexSunposet).KiteiMax
    '                    MRComObject(xlRangeMax)
    '                Catch ex As Exception
    '                End Try
    '                '計測値
    '                Try
    '                    Dim xlRangeSVal As Excel.Range
    '                    xlRangeSVal = xlSheet.Range(WorksD.SunpoSetCellL(i).SunpoVal)
    '                    xlRangeSVal.Value = WorksD.SunpoSetL(indexSunposet).SunpoVal
    '                    MRComObject(xlRangeSVal)
    '                Catch ex As Exception
    '                End Try
    '                '合否
    '                Try
    '                    Dim xlRangeGouhi As Excel.Range
    '                    xlRangeGouhi = xlSheet.Range(WorksD.SunpoSetCellL(i).flg_gouhi)
    '                    If WorksD.SunpoSetL(indexSunposet).flg_gouhi = "1" Then
    '                        xlRangeGouhi.Value = "合"
    '                    Else
    '                        xlRangeGouhi.Value = "否"
    '                    End If
    '                    MRComObject(xlRangeGouhi)
    '                Catch ex As Exception
    '                End Try
    '            End If
    '        Next

    '        Dim rc As Integer = 0

    '        xlApp.DisplayAlerts = False
    '        xlBook.Save()
    '        MRComObject(xlSheet)
    '        MRComObject(xlSheets)
    '        xlBook.Close(False)
    '        MRComObject(xlBook)
    '        MRComObject(xlBooks)
    '        xlApp.Quit()
    '        MRComObject(xlApp)

    '        GC.Collect()

    '    Catch ex As Exception

    '    End Try

    '    ExKillProcess()

    '    System.Diagnostics.Process.Start(strFilePath)

    'End Sub

    'Public Sub MRComObject(Of T As Class)(ByRef objCom As T, Optional ByVal force As Boolean = False)
    '    If objCom Is Nothing Then
    '        Return
    '    End If
    '    Try
    '        If System.Runtime.InteropServices.Marshal.IsComObject(objCom) Then
    '            If force Then
    '                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(objCom)
    '            Else
    '                Dim count As Integer = System.Runtime.InteropServices.HOperatorSet.ClearObj(objCom)
    '            End If
    '        End If
    '    Finally
    '        objCom = Nothing
    '    End Try
    'End Sub

    'Public Sub GetExcelPID()

    '    Dim bRet As Boolean = True
    '    Dim localByName As Process() = Process.GetProcessesByName("Excel")
    '    ReDim BeforePID(localByName.Length)

    '    If localByName.Length = 0 Then
    '    Else
    '        For i As Integer = 0 To localByName.Length - 1
    '            BeforePID(i) = localByName(i).Id
    '        Next

    '    End If

    'End Sub

    'Public Sub GetExcelNewPID()

    '    Dim bRet As Boolean = True
    '    Dim localByName As Process() = Process.GetProcessesByName("Excel")
    '    Dim AfterPID As Integer()
    '    Dim flg_Excel_PID As Boolean = False

    '    localByName = Process.GetProcessesByName("Excel")
    '    ReDim AfterPID(localByName.Length)
    '    For i As Integer = 0 To localByName.Length - 1
    '        flg_Excel_PID = False
    '        AfterPID(i) = localByName(i).Id
    '        For j As Integer = 0 To UBound(BeforePID)
    '            If BeforePID(j) = AfterPID(i) Then
    '                flg_Excel_PID = True
    '                Exit For
    '            End If
    '        Next
    '        If flg_Excel_PID = False Then
    '            MyPID = AfterPID(i)
    '            Exit For
    '        End If
    '    Next

    'End Sub

    'Public Sub ExKillProcess()

    '    Dim localByPID As Process = Process.GetProcessById(MyPID)
    '    If localByPID.Id = MyPID Then
    '        Process.GetProcessById(MyPID).Kill()
    '    End If
    '    localByPID.Close()
    '    localByPID.Dispose()

    'End Sub

#End Region

#Region "規定値一覧のDataGridView"

    Private iColSelNo As Integer
    Private iRowSelNo As Integer

    'DataGridViewのTextBoxセルを宣言
    Private TextEditCtrl As New System.Windows.Controls.TextBox

    '///////////////////////////////
    '////   列毎にIMEを制御   ///////
    'Private Sub DataGridView6_CellEnter(ByVal sender As Object,
    '                                    ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
    '                                    Handles DataGridView6.CellEnter
    Private Sub DataGridView6_PreparingCellForEdit(ByVal sender As Object,
                                                   ByVal e As System.Windows.Controls.DataGridPreparingCellForEditEventArgs
                                                   ) Handles DataGridView6.PreparingCellForEdit


        iColSelNo = e.Column.DisplayIndex   '現在の列番号
        'iRowSelNo = DataGridView6.SelectedIndex      '現在の行番号 　20170531-Kiryu Del 行選択せずに直接カラムをクリックすると取得に失敗する
        iRowSelNo = e.Row.GetIndex()   '20170531 Edit Kiryu 行番号の取得方法変更

        Select Case iColSelNo
            Case 0, 1, 2
                'No・部位・検査項目はIMEモードは設定しない（入力不可のため）
                TextEditCtrl = CType(e.EditingElement, System.Windows.Controls.TextBox)
                AddHandler TextEditCtrl.PreviewKeyDown, AddressOf TextEditCtrl_KeyPress
            Case 3, 4, 5            'IME無効(半角英数のみ)
                '規定値・最小許容値・最大許容値
                System.Windows.Input.InputMethod.SetIsInputMethodEnabled(e.EditingElement, False)
                TextEditCtrl = CType(e.EditingElement, System.Windows.Controls.TextBox)
                AddHandler TextEditCtrl.PreviewKeyDown, AddressOf TextEditCtrl_KeyPress
        End Select

        '///// DataGridViewの編集コントロールが表示された時に //////
        '///// KeyPressイベントを関連付け                    //////
    End Sub

    ''///// DataGridViewの編集コントロールが表示された時に //////
    ''///// KeyPressイベントを関連付け                    //////
    'Private Sub DataGridView6_EditingControlShowing(ByVal sender As Object, _
    '                                                ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) _
    '                                                Handles DataGridView6.EditingControlShowing

    '    TextEditCtrl = CType(e.Control, DataGridViewTextBoxEditingControl)
    '    AddHandler TextEditCtrl.KeyPress, AddressOf TextEditCtrl_KeyPress

    'End Sub

    '///// TextBoxセル内でのKeyPressイベント //////
    Private Sub TextEditCtrl_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs)

        Select Case iColSelNo       '現在の列番号
            Case 3, 4, 5            '数値(0から9)のみ許可
                Select Case e.Key
                    Case System.Windows.Input.Key.Back                                          'BackSpace
                    Case System.Windows.Input.Key.D0 To System.Windows.Input.Key.D9,
                        System.Windows.Input.Key.NumPad0 To System.Windows.Input.Key.NumPad9    '数値(0から9)
                    Case System.Windows.Input.Key.Decimal, System.Windows.Input.Key.OemPeriod   '小数点
                    Case System.Windows.Input.Key.Subtract, System.Windows.Input.Key.OemMinus   'マイナス
                    Case System.Windows.Input.Key.Add, System.Windows.Input.Key.OemPlus         'プラス
                    Case Else
                        '上記キー以外は処理しないようにする
                        '(Delete,Enter,Tabなどは有効)
                        e.Handled = True
                End Select
        End Select

    End Sub

    'Private Sub DataGridView6_CellEndEdit(ByVal sender As Object, _
    '                                      ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
    '                                      Handles DataGridView6.CellEndEdit

    Private Sub DataGridView6_CellEditEnding(ByVal sender As Object,
                                             ByVal e As System.Windows.Controls.DataGridCellEditEndingEventArgs) _
                                         Handles DataGridView6.CellEditEnding

        With DataGridView6
            If TextEditCtrl.Text <> "" Then
                If IsNumeric(m_KiteitiItems(iRowSelNo).No) = True Then
                    '入力行の場合

                    '入力された数値を小数点1桁で表示する
                    If IsNumeric(TextEditCtrl.Text) = True Then
                        Dim iCell As String = Format(dblField(TextEditCtrl.Text), "###0.0")
                        TextEditCtrl.Text = iCell
                    Else
                        '入力された値が数値として成り立たない場合は、空白とする
                        'Rep By Yamada 20150320 Sta ---
                        e.Cancel = True
                        'TextEditCtrl.Text = "0.0"
                        'Rep By Yamada 20150320 End ---
                    End If
                Else
                    '入力行でない場合、空白とする
                    TextEditCtrl.Text = ""
                End If
            End If

            Select Case iColSelNo
                Case 0, 1, 2
                    'No・部位・検査項目はIMEモードは設定しない（入力不可のため）

                Case 3, 4, 5            'IME無効(半角英数のみ)
                    '規定値・最小許容値・最大許容値
                    System.Windows.Input.InputMethod.SetIsInputMethodEnabled(e.EditingElement, False)
            End Select

        End With

        '編集が終わったのでKeyPressイベントの関連付けを解除
        RemoveHandler TextEditCtrl.PreviewKeyDown, AddressOf TextEditCtrl_KeyPress

    End Sub

#End Region

    Private Sub DataGridView6_CurrentCellChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridView6.CurrentCellChanged

        Static OldDataGridView6Item As KiteitiItem = Nothing
        Static OldColumnIndex2 As Integer = -1

        If DataGridView6.CurrentColumn IsNot Nothing Then
            DataGridView6.Focus()
            DataGridView6.BeginEdit()
        End If

        If OldColumnIndex2 >= 0 And OldDataGridView6Item IsNot Nothing Then
            DataGridView6_CellValueChanged(OldColumnIndex2, OldDataGridView6Item)
        End If

        OldDataGridView6Item = DataGridView6.CurrentItem
        If DataGridView6.CurrentColumn Is Nothing Then
            OldColumnIndex2 = -1
        Else
            OldColumnIndex2 = DataGridView6.CurrentColumn.DisplayIndex
        End If
    End Sub
    Private Sub DataGridView6_CellValueChanged(ByVal ColumnIndex As Integer, ByVal item As KiteitiItem)

        Dim iFlg As Integer = 0

        If m_CellChange = 1 Then

            '列番号毎に格納するデータを決定する

            Select Case ColumnIndex
                Case 7
                    If item.flgOutZu = True Then
                        WorksD.SunpoSetL(item.RowIndex).flgOutZu = 1
                    Else
                        WorksD.SunpoSetL(item.RowIndex).flgOutZu = 0
                    End If
                    If WorksD.SunpoSetL(item.RowIndex).flgKeisan <> "0" Then
                        ZukeiColLTypeChg(WorksD.SunpoSetL(item.RowIndex))
                    End If
                    iFlg = 1
                Case 11
                    If item.sek_flgOutZu = True Then
                        WorksD.SunpoSetL(item.RowIndex).sek_flgOutZu = 1
                    Else
                        WorksD.SunpoSetL(item.RowIndex).sek_flgOutZu = 0
                    End If
                    If WorksD.SunpoSetL(item.RowIndex).flgKeisan <> "0" Then
                        ZukeiColLTypeChg(WorksD.SunpoSetL(item.RowIndex))
                    End If
                    iFlg = 1
                Case 8
                    ncolor = 0
                    YCM_ReadSystemColorAcs(m_strDataSystemPath)
                    Dim mcolor As ModelColor
                    If ncolor > 0 Then
                        mcolor = YCM_GetColorInfoByName(item.ZU_color1)
                        WorksD.SunpoSetL(item.RowIndex).ZU_colorID = mcolor.code
                    End If
                    If WorksD.SunpoSetL(item.RowIndex).flgKeisan <> "0" Then
                        ZukeiColLTypeChg(WorksD.SunpoSetL(item.RowIndex))
                    End If
                    iFlg = 1
                Case 12
                    ncolor = 0
                    YCM_ReadSystemColorAcs(m_strDataSystemPath)
                    Dim mcolor1 As ModelColor
                    If ncolor > 0 Then
                        mcolor1 = YCM_GetColorInfoByName(item.sek_ZU_color1)
                        WorksD.SunpoSetL(item.RowIndex).sek_ZU_colorID = mcolor1.code
                    End If
                    If WorksD.SunpoSetL(item.RowIndex).flgKeisan <> "0" Then
                        ZukeiColLTypeChg(WorksD.SunpoSetL(item.RowIndex))
                    End If
                    iFlg = 1
                Case 9
                    nLineType = 0
                    YCM_ReadSystemLineTypes(m_strDataSystemPath)
                    Dim mLinetype As LineType
                    If nLineType > 0 Then
                        mLinetype = YCM_GetLineTypeInfoByName(item.ZU_LineType1)
                        WorksD.SunpoSetL(item.RowIndex).ZU_LineTypeID = mLinetype.code
                    End If
                    If WorksD.SunpoSetL(item.RowIndex).flgKeisan <> "0" Then
                        ZukeiColLTypeChg(WorksD.SunpoSetL(item.RowIndex))
                    End If
                    iFlg = 1
                Case 13
                    nLineType = 0
                    YCM_ReadSystemLineTypes(m_strDataSystemPath)
                    Dim mLinetype1 As LineType
                    If nLineType > 0 Then
                        mLinetype1 = YCM_GetLineTypeInfoByName(item.sek_ZU_LineType1)
                        WorksD.SunpoSetL(item.RowIndex).sek_ZU_LineTypeID = mLinetype1.code
                    End If
                    If WorksD.SunpoSetL(item.RowIndex).flgKeisan <> "0" Then
                        ZukeiColLTypeChg(WorksD.SunpoSetL(item.RowIndex))
                    End If
                    iFlg = 1
                Case 10
                    WorksD.SunpoSetL(item.RowIndex).ZU_layer = item.ZU_layer       'レイヤ変更
                    If WorksD.SunpoSetL(item.RowIndex).flgKeisan <> "0" Then
                        ZukeiColLTypeChg(WorksD.SunpoSetL(item.RowIndex))
                    End If
                    iFlg = 1
                Case 14
                    WorksD.SunpoSetL(item.RowIndex).sek_ZU_layer = item.sek_ZU_layer       'レイヤ変更
                    If WorksD.SunpoSetL(item.RowIndex).flgKeisan <> "0" Then
                        ZukeiColLTypeChg(WorksD.SunpoSetL(item.RowIndex))
                    End If
                    iFlg = 1
            End Select

            If iFlg = 1 Then
                SaveSunpo()
            End If
        End If
    End Sub

    Private Sub TLP1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs)

    End Sub


    'H25.6.28 画像→検査表出力へ(Yamada)

    Private Sub BtnT4_FrmOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnT4_FrmOut.Click
        '=================================
        '画像

        If WorksD.flg_Gazou = "1" Then
            '開く
            BtnT4Next_Open(True)
        Else
            '新規

            BtnT4Next_New(True)
        End If

        '=================================
        '解析

        RunKaisekiProg()
        '画像処理フラグを更新する
        WorksD.SetKaisekiFlg(False)
        '解析データを一覧に表示する
        SetKaisekiDataToGrid()
        '
        View_Dilaog.MainProc()
        YCM.MainFrm.Close()
        '=================================
        '寸法確認

        '寸法確認処理フラグを更新する
        WorksD.SetKakuninFlg(False)

        '寸法確認データを保存する

        SaveSunpoData(False)

        SetValueToComboBox1() '20160822 Byambaa ADD
        '寸法確認画面へ
        'SetSunpouKakunin()'H25.7.1修正前Yamada
        SetSunpoDataToGrid("") 'H25.7.1修正Yamada

        '=================================

        '寸法確認データを保存する

        SaveSunpoData(False)

        '検査表出力画面へ
        'SetOut()H24.7.2コメントアウト　Yamada

        '=================================

        '検査表の出力

        If KensaHyoOutput.SetKensaOut() = False Then
            Exit Sub
        End If

        '検査表出力処理フラグを更新する
        WorksD.SetOutFlg(False)

        'MsgBox("検査表を出力しました。", MsgBoxStyle.OkOnly)

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn3DOut.Click
        'SFD1.InitialDirectory = m_koji_kanri_path

        '' セーブCSVファイル名取得

        'Filesave_P(SFD1)

        '' CSV出力

        'KaisekiP_CsvOut(SFD1)
        Dim frmResult As New frmResultOutput
        frmResult.ShowDialog()
        'BBBYYYAAAMMMBBBAAA
    End Sub

    Private Sub KaisekiP_CsvOut(ByVal SFD As SaveFileDialog)

        Dim i As Integer

        ' データグリッドに行が存在する場合、CSVファイルを出力する

        If m_KaisekiItems.Count > 0 Then

            ' CSVファイルオープン
            Dim FileNo As Integer
            FileNo = FreeFile()
            FileOpen(FileNo, SFD.FileName, OpenMode.Output)

            ' 見出し出力

            Dim strMidasi As String = "測定点,X,Y,Z"
            WriteLine(FileNo, strMidasi)

            With m_KaisekiItems

                ' 解析点出力

                Dim iRowNo As Integer = 0
                For i = 0 To .Count - 1
                    Write(FileNo, .Item(iRowNo).SokutenName)
                    Write(FileNo, .Item(iRowNo).X)
                    Write(FileNo, .Item(iRowNo).Y)
                    WriteLine(FileNo, .Item(iRowNo).Z)

                    iRowNo += 1
                Next
            End With

            ' CSVファイルクローズ
            FileClose(FileNo)

        Else
            MsgBox("表にデータが存在しません", MsgBoxStyle.OkOnly)
        End If

    End Sub

    Private Sub KekkaKakuninKentou()
        With DGW_KekkaKakunin
            'ヘッダを中央
            '.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeaderStyle = Application.Current.Resources("DataGrid_Header_Center")

            '並び替え禁止
            For Each T As System.Windows.Controls.DataGridColumn In .Columns
                T.CanUserSort = False
            Next

            'ユーザーは行の追加・削除不可
            .CanUserAddRows = False
            .CanUserDeleteRows = False

            .ItemsSource = Nothing
        End With
        With m_KekkaKakuninItems

            .Clear()
            ''H25.5.25 Yamada
            'If WorksD.SunpoSetL.Count < 20 Then
            '    .Rows.Add(20)
            'Else
            '    .Rows.Add(WorksD.SunpoSetL.Count)
            'End If
            Dim iRowNo As Integer = 0
            Dim itmpRowcount As Integer = 10

            .Add(New KekkaKakuninViewItem)
            .Item(iRowNo).RowIndex = iRowNo
            .Item(iRowNo).SunpoID = 1
            .Item(iRowNo).SunpoName = "スケール(基準)"
            .Item(iRowNo).Calcname = "点と点"
            .Item(iRowNo).CTIDs = "CT1,CT2"
            .Item(iRowNo).seibunXYZ = "XYZ"
            .Item(iRowNo).SunpoVal = 1000
            iRowNo += 1

            .Add(New KekkaKakuninViewItem)
            .Item(iRowNo).RowIndex = iRowNo
            .Item(iRowNo).SunpoID = 2
            .Item(iRowNo).SunpoName = "スケール(検証)"
            .Item(iRowNo).Calcname = "点と点"
            .Item(iRowNo).CTIDs = "CT3,CT4"
            .Item(iRowNo).seibunXYZ = "XYZ"
            .Item(iRowNo).SunpoVal = 1000.5
            iRowNo += 1

            .Add(New KekkaKakuninViewItem)
            .Item(iRowNo).RowIndex = iRowNo
            .Item(iRowNo).SunpoID = 3
            .Item(iRowNo).SunpoName = "距離１"
            .Item(iRowNo).Calcname = "点と点"
            .Item(iRowNo).CTIDs = "CT10,CT11"
            .Item(iRowNo).seibunXYZ = "XY"
            .Item(iRowNo).SunpoVal = 1234
            iRowNo += 1

            .Add(New KekkaKakuninViewItem)
            .Item(iRowNo).RowIndex = iRowNo
            .Item(iRowNo).SunpoID = 4
            .Item(iRowNo).SunpoName = "距離２"
            .Item(iRowNo).Calcname = "点と点"
            .Item(iRowNo).CTIDs = "CT12,CT13"
            .Item(iRowNo).seibunXYZ = "XZ"
            .Item(iRowNo).SunpoVal = 1002.3
            iRowNo += 1

            .Add(New KekkaKakuninViewItem)
            .Item(iRowNo).RowIndex = iRowNo
            .Item(iRowNo).SunpoID = 5
            .Item(iRowNo).SunpoName = "距離３"
            .Item(iRowNo).Calcname = "面と点"
            .Item(iRowNo).CTIDs = "CT22,CT23,CT24,CT35"
            .Item(iRowNo).seibunXYZ = "XZ"
            .Item(iRowNo).SunpoVal = 102.3
            iRowNo += 1

            .Add(New KekkaKakuninViewItem)
            .Item(iRowNo).RowIndex = iRowNo
            .Item(iRowNo).SunpoID = 6
            .Item(iRowNo).SunpoName = "距離４"
            .Item(iRowNo).Calcname = "原点と点"
            .Item(iRowNo).CTIDs = "CT50"
            .Item(iRowNo).seibunXYZ = "XZ"
            .Item(iRowNo).SunpoVal = 1202.3
            iRowNo += 1

            .Add(New KekkaKakuninViewItem)
            .Item(iRowNo).RowIndex = iRowNo
            .Item(iRowNo).SunpoID = 7
            .Item(iRowNo).SunpoName = "円１"
            .Item(iRowNo).Calcname = "３点"
            .Item(iRowNo).CTIDs = "CT100,CT101,CT102"
            .Item(iRowNo).seibunXYZ = "XYZ"
            .Item(iRowNo).SunpoVal = 52.3
            iRowNo += 1

            .Add(New KekkaKakuninViewItem)
            .Item(iRowNo).RowIndex = iRowNo
            .Item(iRowNo).SunpoID = 8
            .Item(iRowNo).SunpoName = "円２"
            .Item(iRowNo).Calcname = "３点"
            .Item(iRowNo).CTIDs = "CT110,CT111,CT112"
            .Item(iRowNo).seibunXYZ = "XYZ"
            .Item(iRowNo).SunpoVal = 55.3
            iRowNo += 1
        End With
        DGW_KekkaKakunin.ItemsSource = m_KekkaKakuninItems
    End Sub

    'Private Sub DGW_KekkaKakunin_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DGW_KekkaKakunin.CellClick
    Private Sub DGW_KekkaKakunin_CurrentCellChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DGW_KekkaKakunin.CurrentCellChanged

        'Select Case e.RowIndex
        '    Case 0
        '        PictureBox3.ImageLocation = m_TemplatePath & "\2tenkan.JPG"
        '    Case 1
        '        PictureBox3.ImageLocation = m_TemplatePath & "\2tenkan.JPG"
        '    Case 2
        '        PictureBox3.ImageLocation = m_TemplatePath & "\2tenkan.JPG"
        '    Case 3
        '        PictureBox3.ImageLocation = m_TemplatePath & "\2tenkan.JPG"
        '    Case 4
        '        PictureBox3.ImageLocation = m_TemplatePath & "\mentoten_dist.JPG"
        '    Case 5
        '        PictureBox3.ImageLocation = m_TemplatePath & "\GentenKyori.JPG"
        '    Case 6
        '        PictureBox3.ImageLocation = m_TemplatePath & "\3tenEn.JPG"
        '    Case 7
        '        PictureBox3.ImageLocation = m_TemplatePath & "\3tenEn.JPG"

        'End Select

        ' init OldKekkaKakuninViewItem and OldColumnIndex
        Static OldKekkaKakuninViewItem As KekkaKakuninViewItem = Nothing
        Static OldColumnIndex As Integer = -1

        '(20140530 Tezuka ADD) ComboBoxクリック対応
        If DGW_KekkaKakunin.CurrentColumn IsNot Nothing Then
            DGW_KekkaKakunin.Focus()
            DGW_KekkaKakunin.BeginEdit()
        End If

        ' update PictureBox3
        If DGW_KekkaKakunin.CurrentItem Is Nothing Then
        Else
            Dim strSelect As String
            strSelect = CType(DGW_KekkaKakunin.CurrentItem, KekkaKakuninViewItem).Calcname

            Dim iCnt As Integer = 0
            Dim ii As Integer
            iCnt = WorksD.SunpoCalcHouhouL.Count
            If iCnt > 0 Then
                For ii = 0 To iCnt - 1
                    If strSelect = WorksD.SunpoCalcHouhouL(ii).Calcname Then
                        Try
                            Dim u As New Uri(m_TemplatePath & "\" & WorksD.SunpoCalcHouhouL(ii).PreviewImagePath, UriKind.RelativeOrAbsolute)
                            Dim img As New BitmapImage(u)
                            PictureBox3.Source = img
                            Exit For
                        Catch ex As Exception
                            Exit For
                        End Try

                    End If
                Next
            End If
        End If

        ' copy previous selected cell value to 
        If OldColumnIndex >= 0 And OldKekkaKakuninViewItem IsNot Nothing Then
            DGW_KekkaKakunin_CellValueChanged(OldColumnIndex, OldKekkaKakuninViewItem)
        End If

        ' update OldKekkaKakuninViewItem and OldColumnIndex
        OldKekkaKakuninViewItem = DGW_KekkaKakunin.CurrentItem
        If DGW_KekkaKakunin.CurrentColumn Is Nothing Then
            OldColumnIndex = -1
        Else
            OldColumnIndex = DGW_KekkaKakunin.CurrentColumn.DisplayIndex
        End If
    End Sub

    'Private Sub DGW_KekkaKakunin_RowValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DGW_KekkaKakunin.RowValidated
    '    Static Dim ss As Integer = 0
    '    'ss += 1
    '    'If ss Mod 3 = 0 Then
    '    '    'PictureBox3.Image.FromFile(".\計測システムフォルダ\template\mentoten_dist.JPG")
    '    '    PictureBox3.ImageLocation = m_TemplatePath & "\mentoten_dist.JPG"
    '    'End If

    '    'If ss Mod 3 = 1 Then
    '    '    'PictureBox3.Image.FromFile(".\計測システムフォルダ\template\mentoten_dist.JPG")
    '    '    PictureBox3.ImageLocation = m_TemplatePath & "\angle.JPG"
    '    'End If
    '    'If ss Mod 3 = 2 Then
    '    '    'PictureBox3.Image.FromFile(".\計測システムフォルダ\template\mentoten_dist.JPG")
    '    '    PictureBox3.ImageLocation = m_TemplatePath & "\2tenkan.JPG"
    '    'End If



    '    'Select Case e.RowIndex
    '    '    Case 1
    '    '        PictureBox3.ImageLocation = m_TemplatePath & "\2tenkan.JPG"
    '    '    Case 2
    '    '        PictureBox3.ImageLocation = m_TemplatePath & "\2tenkan.JPG"
    '    '    Case 3
    '    '        PictureBox3.ImageLocation = m_TemplatePath & "\2tenkan.JPG"
    '    '    Case 4
    '    '        PictureBox3.ImageLocation = m_TemplatePath & "\2tenkan.JPG"
    '    '    Case 5
    '    '        PictureBox3.ImageLocation = m_TemplatePath & "\mentoten_dist.JPG"
    '    '    Case 6
    '    '        PictureBox3.ImageLocation = m_TemplatePath & "\GentenKyori.JPG"
    '    '    Case 7
    '    '        PictureBox3.ImageLocation = m_TemplatePath & "\3tenEn.JPG"
    '    '    Case 8
    '    '        PictureBox3.ImageLocation = m_TemplatePath & "\3tenEn.JPG"

    '    'End Select


    'End Sub

    Private Sub BtnT6_Back_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnT6_Back.Click
        m_CellChange = 0
        '解析画面へ
        SetKaiseki(True)

    End Sub

    Private Sub BtnT6_Next_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnT6_Next.Click

        '寸法確認データを保存する

        SaveSunpoData(False)

        '検査表出力画面へ
        SetOut()

    End Sub

    Public Sub SupoKakuninGridDraw()
        With DGW_KekkaKakunin
            'ヘッダを中央
            .ColumnHeaderStyle = Application.Current.Resources("DataGrid_Header_Center")

            '並び替え禁止
            For Each T As System.Windows.Controls.DataGridColumn In .Columns
                T.CanUserSort = False
            Next

            'ユーザーは行の追加・削除不可
            .CanUserAddRows = False
            .CanUserDeleteRows = False

            .ItemsSource = Nothing
        End With
        With m_KekkaKakuninItems
            .Clear()

            'If WorksD.SunpoSetL.Count < 20 Then
            '    For i As Integer = 1 To 20
            '        .Add(New KekkaKakuninViewItem)
            '    Next
            'Else
            For i As Integer = 1 To WorksD.SunpoSetL.Count
                .Add(New KekkaKakuninViewItem)
            Next
            'End If
            For i As Integer = 0 To WorksD.SunpoSetL.Count - 1

                Try
                    .Item(i).RowIndex = i
                    .Item(i).SunpoID = CInt(WorksD.SunpoSetL(i).SunpoID)           'No
                    .Item(i).MeasurementSet = WorksD.SunpoSetL(i).MeasurementSet   '計測セット　(20160822 byambaa ADD)
                    .Item(i).SunpoName = WorksD.SunpoSetL(i).SunpoName             '検査項目
                    .Item(i).SunpoVal = CDbl(WorksD.SunpoSetL(i).SunpoVal)         '計測値
                    '種類の決定

                    .Item(i).Calcname = ""
                    If WorksD.SunpoCalcHouhouL.Count > 0 Then
                        Dim ii As Integer
                        For ii = 0 To WorksD.SunpoCalcHouhouL.Count - 1
                            If WorksD.SunpoCalcHouhouL(ii).SchID = WorksD.SunpoSetL(i).SunpoCalcHouhou Then
                                .Item(i).Calcname = WorksD.SunpoCalcHouhouL(ii).Calcname
                                Exit For
                            End If
                        Next
                    End If
                    'Select Case WorksD.SunpoSetL(i).Targettype
                    '    Case 1
                    '        .Rows(i).Cells(2).Value = "点と点"
                    '    Case 2
                    '        .Rows(i).Cells(2).Value = "面と点"
                    '    Case 3
                    '        .Rows(i).Cells(2).Value = "原点と点"
                    '    Case 4
                    '        .Rows(i).Cells(2).Value = "点群"
                    '    Case 5
                    '        .Rows(i).Cells(2).Value = "３点"
                    '    Case 6
                    '        .Rows(i).Cells(2).Value = "１点，半径"
                    'End Select
                    .Item(i).SunpoVal = CDbl(WorksD.SunpoSetL(i).SunpoVal)         '計測値
                    '参照ターゲット文字列作成
                    WorksD.SunpoSetL(i).SetSunpoCTID()
                    Dim strCTIDs As String = ""
                    For Each CTID As Integer In WorksD.SunpoSetL(i).lstCT_ID
                        strCTIDs = strCTIDs & "CT" & CTID & ","
                    Next
                    Try
                        strCTIDs = strCTIDs.Substring(0, strCTIDs.Length - 1)
                    Catch ex As Exception

                    End Try
                    .Item(i).CTIDs = strCTIDs                                  '参照ターゲット
                    'XYZ成分の決定

                    Select Case WorksD.SunpoSetL(i).seibunXYZ
                        Case XYZseibun.X
                            .Item(i).seibunXYZ = "X"
                        Case XYZseibun.Y
                            .Item(i).seibunXYZ = "Y"
                        Case XYZseibun.Z
                            .Item(i).seibunXYZ = "Z"
                        Case XYZseibun.XY
                            .Item(i).seibunXYZ = "XY"
                        Case XYZseibun.XZ
                            .Item(i).seibunXYZ = "XZ"
                        Case XYZseibun.YZ
                            .Item(i).seibunXYZ = "YZ"
                        Case XYZseibun.XYZ
                            .Item(i).seibunXYZ = "XYZ"
                    End Select
                    '図化

                    If DGW_KekkaKakunin.Columns(6).Visibility = System.Windows.Visibility.Visible Then
                        If WorksD.SunpoSetL(i).flgOutZu = "1" Then
                            .Item(i).flgOutZu = True
                        Else
                            .Item(i).flgOutZu = False
                        End If
                    End If
                    '線色
                    If DGW_KekkaKakunin.Columns(7).Visibility = System.Windows.Visibility.Visible Then
                        ncolor = 0
                        YCM_ReadSystemColorAcs(m_strDataSystemPath)
                        Dim mcolor As ModelColor
                        If ncolor > 0 Then
                            'Dim dtTbl As New DataTable
                            'dtTbl.Columns.Add("Display", GetType(String))
                            'dtTbl.Columns.Add("Value", GetType(Integer))

                            'Dim ii As Integer
                            'Dim strSelect(ncolor) As String
                            'Dim mcolor2 As ModelColor
                            'For ii = 0 To ncolor - 1
                            '    mcolor2 = YCM_GetColorInfoByCode(ii + 1)
                            '    dtTbl.Rows.Add(mcolor2.strName, mcolor2.code)
                            '    'strSelect(ii) = mcolor2.strName
                            'Next
                            'Dim cbc As New DataGridViewComboBoxColumn
                            'cbc = CType(.Columns(7), DataGridViewComboBoxColumn)
                            'cbc.DataSource = dtTbl
                            'cbc.ValueMember = "Value"
                            'cbc.DisplayMember = "Display"
                            If WorksD.SunpoSetL(i).ZU_colorID = "" Or WorksD.SunpoSetL(i).ZU_colorID = "0" Then
                                .Item(i).ZU_color = "RED"
                            Else
                                mcolor = YCM_GetColorInfoByCode(WorksD.SunpoSetL(i).ZU_colorID)
                                .Item(i).ZU_color = mcolor.strName
                            End If
                        End If
                        'Select Case WorksD.SunpoSetL(i).ZU_colorID
                        '    Case 1
                        '        .Rows(i).Cells(7).Value = "RED"
                        '    Case 2
                        '        .Rows(i).Cells(7).Value = "BLUE"
                        '    Case 3
                        '        .Rows(i).Cells(7).Value = "GREEN"
                        'End Select
                    End If
                    '線種
                    If DGW_KekkaKakunin.Columns(8).Visibility = System.Windows.Visibility.Visible Then
                        nLineType = 0
                        YCM_ReadSystemLineTypes(m_strDataSystemPath)
                        Dim mLinetype As LineType
                        If nLineType > 0 Then
                            If WorksD.SunpoSetL(i).ZU_LineTypeID = "" Or WorksD.SunpoSetL(i).ZU_LineTypeID = "0" Then
                                .Item(i).ZU_LineType = "CONTINUOUS"
                            Else
                                mLinetype = YCM_GetLineTypeInfoByCode(WorksD.SunpoSetL(i).ZU_LineTypeID)
                                .Item(i).ZU_LineType = mLinetype.strName
                            End If
                        End If
                        'Select Case WorksD.SunpoSetL(i).ZU_LineTypeID
                        '    Case 1
                        '        .Rows(i).Cells(8).Value = "COUTINUOUS"
                        '    Case 2
                        '        .Rows(i).Cells(8).Value = "DASHED"
                        '    Case Else
                        '        .Rows(i).Cells(8).Value = ""
                        'End Select
                    End If

                Catch ex As Exception
                    MsgBox("値設定エラー", MsgBoxStyle.OkOnly)
                    Exit Sub
                End Try

                If WorksD.SunpoSetL(i).flgKeisan <> "1" Then
                    .Item(i).Visibility = System.Windows.Visibility.Hidden
                End If

            Next
            ' .Columns.Item("Sunpo_KiteiVal").DefaultCellStyle.Format = "N1"
        End With
        DGW_KekkaKakunin.ItemsSource = m_KekkaKakuninItems
        m_CellChange = 1
    End Sub


    'Private Sub DGW_KekkaKakunin_CellValueChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DGW_KekkaKakunin.CellValueChanged
    Private Sub DGW_KekkaKakunin_CellValueChanged(ByVal ColumnIndex As Integer, ByVal item As KekkaKakuninViewItem)

        Dim iFlg As Integer = 0

        If m_CellChange = 1 Then

            '列番号毎に格納するデータを決定する


            Select Case ColumnIndex
                Case 0      '寸法ID
                    'WorksD.SunpoSetL(e.RowIndex).SunpoID = DGW_KekkaKakunin.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
                    'iFlg = 1
                Case 1      '寸法名
                    WorksD.SunpoSetL(item.RowIndex).SunpoName = item.SunpoName
                    iFlg = 1
                Case 2      '種類

                    'If WorksD.SunpoCalcHouhouL.Count > 0 Then
                    '    Dim ii As Integer
                    '    For ii = 0 To WorksD.SunpoCalcHouhouL.Count - 1
                    '        If DGW_KekkaKakunin.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = WorksD.SunpoCalcHouhouL(ii).Calcname Then
                    '            WorksD.SunpoSetL(e.RowIndex).SunpoCalcHouhou = WorksD.SunpoCalcHouhouL(ii).SchID
                    '            Exit For
                    '        End If
                    '    Next
                    'End If

                    'Select Case DGW_KekkaKakunin.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
                    '    Case "点と点"
                    '        WorksD.SunpoSetL(e.RowIndex).Targettype = 1
                    '    Case "点と点"
                    '        WorksD.SunpoSetL(e.RowIndex).Targettype = 2
                    '    Case "点と点"
                    '        WorksD.SunpoSetL(e.RowIndex).Targettype = 3
                    '    Case "点群"
                    '        WorksD.SunpoSetL(e.RowIndex).Targettype = 4
                    '    Case "点群"
                    '        WorksD.SunpoSetL(e.RowIndex).Targettype = 5
                    '    Case "点群"
                    '        WorksD.SunpoSetL(e.RowIndex).Targettype = 6
                    'End Select
                    iFlg = 1
                Case 4      'XYZ成分
                    Select Case item.seibunXYZ
                        Case "X"
                            WorksD.SunpoSetL(item.RowIndex).seibunXYZ = XYZseibun.X
                        Case "Y"
                            WorksD.SunpoSetL(item.RowIndex).seibunXYZ = XYZseibun.Y
                        Case "Z"
                            WorksD.SunpoSetL(item.RowIndex).seibunXYZ = XYZseibun.Z
                        Case "XY"
                            WorksD.SunpoSetL(item.RowIndex).seibunXYZ = XYZseibun.XY
                        Case "XZ"
                            WorksD.SunpoSetL(item.RowIndex).seibunXYZ = XYZseibun.XZ
                        Case "YZ"
                            WorksD.SunpoSetL(item.RowIndex).seibunXYZ = XYZseibun.YZ
                        Case "XYZ"
                            WorksD.SunpoSetL(item.RowIndex).seibunXYZ = XYZseibun.XYZ
                    End Select
                    'WorksD.SunpoSetL(e.RowIndex).CalcSunpoVal()
                    CalcSunpo()
                    item.SunpoVal = WorksD.SunpoSetL(item.RowIndex).SunpoVal
                    iFlg = 1
                Case 5
                    'WorksD.SunpoSetL(e.RowIndex).SunpoVal = DGW_KekkaKakunin.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
                    'iFlg = 1
                Case 6
                    If item.flgOutZu = True Then
                        WorksD.SunpoSetL(item.RowIndex).flgOutZu = 1
                    Else
                        WorksD.SunpoSetL(item.RowIndex).flgOutZu = 0
                    End If
                    If WorksD.SunpoSetL(item.RowIndex).flgKeisan <> "0" Then
                        ZukeiColLTypeChg(WorksD.SunpoSetL(item.RowIndex))
                    End If
                    iFlg = 1
                Case 7
                    ncolor = 0
                    YCM_ReadSystemColorAcs(m_strDataSystemPath)
                    Dim mcolor As ModelColor
                    If ncolor > 0 Then
                        mcolor = YCM_GetColorInfoByName(item.ZU_color)
                        WorksD.SunpoSetL(item.RowIndex).ZU_colorID = mcolor.code
                    End If
                    If WorksD.SunpoSetL(item.RowIndex).flgKeisan <> "0" Then
                        ZukeiColLTypeChg(WorksD.SunpoSetL(item.RowIndex))
                    End If
                    'Select Case DGW_KekkaKakunin.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
                    '    Case "RED"
                    '        WorksD.SunpoSetL(e.RowIndex).ZU_colorID = 1
                    '    Case "BLUE"
                    '        WorksD.SunpoSetL(e.RowIndex).ZU_colorID = 2
                    '    Case "GREEN"
                    '        WorksD.SunpoSetL(e.RowIndex).ZU_colorID = 3
                    'End Select
                    iFlg = 1
                Case 8
                    nLineType = 0
                    YCM_ReadSystemLineTypes(m_strDataSystemPath)
                    Dim mLinetype As LineType
                    If nLineType > 0 Then
                        mLinetype = YCM_GetLineTypeInfoByName(item.ZU_LineType)
                        WorksD.SunpoSetL(item.RowIndex).ZU_LineTypeID = mLinetype.code
                    End If
                    If WorksD.SunpoSetL(item.RowIndex).flgKeisan <> "0" Then
                        ZukeiColLTypeChg(WorksD.SunpoSetL(item.RowIndex))
                    End If
                    'Select Case DGW_KekkaKakunin.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
                    '    Case "COUTINUOUS"
                    '        WorksD.SunpoSetL(e.RowIndex).ZU_LineTypeID = 1
                    '    Case "DASHED"
                    '        WorksD.SunpoSetL(e.RowIndex).ZU_LineTypeID = 2
                    'End Select
                    iFlg = 1
                Case 9
                    If item.sek_flgOutZu = True Then
                        WorksD.SunpoSetL(item.RowIndex).sek_flgOutZu = 1
                    Else
                        WorksD.SunpoSetL(item.RowIndex).sek_flgOutZu = 0
                    End If
                    If WorksD.SunpoSetL(item.RowIndex).flgKeisan <> "0" Then
                        ZukeiColLTypeChg(WorksD.SunpoSetL(item.RowIndex))
                    End If
                    iFlg = 1
                Case 10
                    ncolor = 0
                    YCM_ReadSystemColorAcs(m_strDataSystemPath)
                    Dim mcolor1 As ModelColor
                    If ncolor > 0 Then
                        mcolor1 = YCM_GetColorInfoByName(item.sek_ZU_color)
                        WorksD.SunpoSetL(item.RowIndex).sek_ZU_colorID = mcolor1.code
                    End If
                    If WorksD.SunpoSetL(item.RowIndex).flgKeisan <> "0" Then
                        ZukeiColLTypeChg(WorksD.SunpoSetL(item.RowIndex))
                    End If
                    'Select Case DGW_KekkaKakunin.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
                    '    Case "RED"
                    '        WorksD.SunpoSetL(e.RowIndex).ZU_colorID = 1
                    '    Case "BLUE"
                    '        WorksD.SunpoSetL(e.RowIndex).ZU_colorID = 2
                    '    Case "GREEN"
                    '        WorksD.SunpoSetL(e.RowIndex).ZU_colorID = 3
                    'End Select
                    iFlg = 1
                Case 11
                    nLineType = 0
                    YCM_ReadSystemLineTypes(m_strDataSystemPath)
                    Dim mLinetype1 As LineType
                    If nLineType > 0 Then
                        mLinetype1 = YCM_GetLineTypeInfoByName(item.sek_ZU_LineType)
                        WorksD.SunpoSetL(item.RowIndex).sek_ZU_LineTypeID = mLinetype1.code
                    End If
                    If WorksD.SunpoSetL(item.RowIndex).flgKeisan <> "0" Then
                        ZukeiColLTypeChg(WorksD.SunpoSetL(item.RowIndex))
                    End If
                    'Select Case DGW_KekkaKakunin.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
                    '    Case "COUTINUOUS"
                    '        WorksD.SunpoSetL(e.RowIndex).ZU_LineTypeID = 1
                    '    Case "DASHED"
                    '        WorksD.SunpoSetL(e.RowIndex).ZU_LineTypeID = 2
                    'End Select
                    iFlg = 1
            End Select

            If iFlg = 1 Then

                '寸法確認データを保存する

                SaveSunpo()
            End If
        End If
    End Sub

    'Private Sub DGW_KekkaKakunin_CurrentCellDirtyStateChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DGW_KekkaKakunin.CurrentCellDirtyStateChanged
    '    If DGW_KekkaKakunin.IsCurrentCellDirty Then
    '        DGW_KekkaKakunin.CommitEdit(DataGridViewDataErrorContexts.Commit)
    '    End If
    'End Sub


    'ljj add sta 2014/01/15

    'Rep By Yamada 20140919 Sat ------------------------------------------------------------------------------------日本車輌
    'Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

    '    Dim bln_cadOpen As Boolean = False

    '    '(20140605 Tezuka ADD) ３D画像が表示されていない時はメッセージ出力して終わる
    '    If bln_KaisekiOpened = False Then
    '        MsgBox("３Ｄ画像が表示されていません。" + vbCrLf + "解析を実行するか詳細表示ボタンを押して、" + vbCrLf + _
    '               "３Ｄ画像を表示してから再実行してください", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "ＤＸＦ出力エラー")
    '        Exit Sub
    '    End If

    '    Dim cadOutFrm As New YCM_CADOutOption
    '    cadOutFrm.CheckBox_LookPoint.Checked = False
    '    cadOutFrm.CheckBox_Ray.Checked = False
    '    cadOutFrm.CheckBox_LabelText.Checked = False
    '    cadOutFrm.CheckBox_UserElm.Checked = True
    '    cadOutFrm.ComboBox_OutDim.SelectedIndex = 0
    '    cadOutFrm.CheckBox_Information.Checked = True
    '    'SUURI UPDATE 20140914
    '    Dim strDXFdefaultName As String = WorksD.GetDefaultDXGFileName
    '    cadOutFrm.TextBox_CADFileName.Text = MainFrm.objFBM.ProjectPath & "\" & strDXFdefaultName
    '    'SUURI UPDATE 20140914
    '    cadOutFrm.TextBox_TextH.Text = "10"
    '    cadOutFrm.isDwgFilter(False)
    '    cadOutFrm.ShowDialog()

    '    Dim iOutCoord As Integer  '--出力次元


    '    If (cadOutFrm.DialogResult = System.Windows.Forms.DialogResult.OK) Then

    '        '=======================================================================
    '        ' read and check the information in Dialog
    '        '=======================================================================
    '        Dim bLookPos As Boolean, bRay As Boolean, bLabelText As Boolean, bUserElm As Boolean
    '        Dim dSize As Double, dTextH As Double
    '        Dim strDxfName As String
    '        Try
    '            bLookPos = cadOutFrm.CheckBox_LookPoint.Checked
    '            bRay = cadOutFrm.CheckBox_Ray.Checked
    '            bLabelText = cadOutFrm.CheckBox_LabelText.Checked()
    '            bUserElm = cadOutFrm.CheckBox_UserElm.Checked

    '            dSize = entset_point.screensize '★20121115計測点
    '            '========================================================================================================20121115
    '            'dSize = entset_point1.screensize '★20121114計測点(ターゲット面)
    '            'dSize = entset_point2.screensize '★20121114計測点(中心円)
    '            'dSize = entset_point3.screensize '★20121114計測点(十字線)
    '            '========================================================================================================20121115
    '            dTextH = CDbl(cadOutFrm.TextBox_TextH.Text)
    '            iOutCoord = (cadOutFrm.ComboBox_OutDim.SelectedIndex)
    '            strDxfName = cadOutFrm.TextBox_CADFileName.Text
    '        Catch ex As Exception
    '            MsgBox("The input date are incorrect." + vbCrLf + ex.Message)
    '            Return
    '        End Try


    '        '=======================================================================
    '        ' Initialize Teigha.
    '        '=======================================================================
    '        Dim Serv As Teigha.Core.ExSystemServices
    '        Dim _hostApp As MyHostAppServ
    '        Try
    '            Serv = New Teigha.Core.ExSystemServices()
    '            _hostApp = New MyHostAppServ()
    '            Teigha.TD.TD_Db.odInitialize(Serv)
    '        Catch ex As Exception
    '            MsgBox("Failed to initialize Teigha.")
    '            Return
    '        End Try

    '        Try
    '            'Dim pDb As Teigha.TD.OdDbDatabase = _hostApp.createDatabase(True, Teigha.TD.MeasurementValue.kEnglish)
    '            Using pDb As Teigha.TD.OdDbDatabase = _hostApp.createDatabase(True, Teigha.TD.MeasurementValue.kEnglish)

    '            'add by liwei.z 20140527 start
    '            SetStandardTextStyleFont(pDb)
    '            'add by liwei.z 20140527 end

    '            Dim cad As New ODOperator(pDb)

    '            '' 2014-5-26 str by Ljj
    '                ' loadData()
    '            ' 2014-5-26 end by Ljj

    '            ' loadData()

    '            '--計測点の書き出し

    '            If (bLookPos = True) Then exportLookPoints(cad, iOutCoord)

    '            '--レイの書き出し

    '            If (bRay = True) Then Call exportRay(cad, iOutCoord)

    '            '--計測ラベルの書き出し

    '            If (bLabelText = True) Then Call exportLabelText(cad, dTextH, iOutCoord)

    '            '--ユーザ作成図形の書き出し

    '            If (bUserElm = True) Then Call exportUserElm(cad, iOutCoord)

    '            '--作成したCAD図の保存



    '            ' 2014-5-26 str by Ljj
    '            '出力情報
    '                Dim ij As Integer
    '                Dim pt As New GeoPoint
    '                Dim ihd As Integer = 10
    '                ' 2014-5-30 str by Ljj
    '                Dim pRS As New GeoPoint
    '                Dim pRE As New GeoPoint
    '                Dim pRMaxX As Integer
    '                Dim pRMaxY As Integer
    '                Dim pRMinX As Integer  'SUURI ADD 20140913
    '                Dim pPminY As Integer

    '                'SUURI ADD 20140914
    '                GetBoundingBox_OutZU(iOutCoord, pRMaxX, pRMaxY, pRMinX, pPMinY)


    '                'SUURI UPDATE START 20140913
    '                'Dim iX As Integer = pRMaxX + 200
    '                'Dim iY As Integer = (pRMaxY + pPminY) / 2
    '                Dim iX As Integer = pRMinX
    '                Dim iY As Integer = pRMaxY + (WorksD.KihonL.Count * 20)
    '                'SUURI UPDATE END 20140913
    '                Dim iZ As Integer = 0
    '                ' 2014-5-30 end by Ljj

    '                If cadOutFrm.CheckBox_Information.Checked = True Then
    '                    Try
    '                        cad.CreateLayer(entset_label.layerName) ' "LABELTEXT")
    '                        cad.CreateLayerEntSet(entset_label)
    '                        For ij = 0 To WorksD.KihonL.Count - 1
    '                            Call pt.setXYZ(iX, iY - ij * 20, iZ)
    '                            'cad.AddText(WorksD.KihonL(ij).item_name + " : " + WorksD.KihonL(ij).item_value, pt, ihd, entset_label.layerName)
    '                            cad.AddText_Information(WorksD.KihonL(ij).item_name + " : " + WorksD.KihonL(ij).item_value, pt, ihd, entset_label.layerName)
    '                        Next
    '                    Catch ex As Exception
    '                        MsgBox("CAD図形の情報出しを失敗しました" + ex.Message)
    '                    End Try
    '                End If
    '            ' 2014-5-26 end by Ljj



    '            pDb.writeFile(strDxfName, _
    '                          Teigha.TD.SaveType.kDxf, Teigha.Core.DwgVersion.vAC15)

    '            If MsgBox("CAD図形の書き出しを終了しました。開きますか？", MsgBoxStyle.YesNo, "確認") = MsgBoxResult.Yes Then
    '                bln_cadOpen = True
    '                End If
    '            End Using
    '        Catch ex As Exception
    '            MsgBox("CAD図形の書き出しを失敗しました。" + vbCrLf + ex.Message)
    '        Finally
    '            '=======================================================================
    '            ' Uninitialize Teigha.
    '            '=======================================================================
    '            Try
    '                ' start : crash in writeFile without this codes
    '                _hostApp.Dispose()
    '                ' end   : crash in writeFile without this codes
    '                Teigha.TD.TD_Db.odUninitialize()
    '            Catch ex As Exception
    '                MsgBox("Failed to uninitialize Teigha.")
    '            End Try
    '        End Try
    '    End If
    '    If bln_cadOpen = True Then
    '        System.Diagnostics.Process.Start(cadOutFrm.TextBox_CADFileName.Text)
    '    End If
    'End Sub
    Private Sub BtnT6_DXF_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnT6_DXF.Click
        DXFOUT()
    End Sub


    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        ' DXFOUT()

        DXFOUT_WithObj3d()

    End Sub

    Private Sub DXFOUT_WithObj3d()
        Dim bln_cadOpen As Boolean = False

        '(20140605 Tezuka ADD) ３D画像が表示されていない時はメッセージ出力して終わる
        If bln_KaisekiOpened = False Then
            MsgBox("３Ｄ画像が表示されていません。" + vbCrLf + "解析を実行するか詳細表示ボタンを押して、" + vbCrLf + _
                   "３Ｄ画像を表示してから再実行してください", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "ＤＸＦ出力エラー")
            Exit Sub
        End If

        Dim cadOutFrm As New YCM_CADOutOption
        cadOutFrm.CheckBox_LookPoint.Checked = False
        cadOutFrm.CheckBox_Ray.Checked = False
        cadOutFrm.CheckBox_LabelText.Checked = True
        cadOutFrm.CheckBox_UserElm.Checked = True
        cadOutFrm.CheckBox_Reconstruction.Checked = False
        cadOutFrm.CheckBox_Information.Checked = False
        cadOutFrm.ComboBox_OutDim.Visible = False
        Dim strDXFdefaultName As String = WorksD.GetDefaultDXGFileName
        cadOutFrm.TextBox_CADFileName.Text = MainFrm.objFBM.ProjectPath & "\" & strDXFdefaultName
        'SUURI UPDATE 20140914
        cadOutFrm.TextBox_TextH.Text = "10"
        cadOutFrm.isDwgFilter(False)
        cadOutFrm.ShowDialog()

        Dim iOutCoord As Integer  '--出力次元


        If (cadOutFrm.DialogResult = System.Windows.Forms.DialogResult.OK) Then
            Dim bLookPos As Boolean, bRay As Boolean, bLabelText As Boolean, bUserElm As Boolean, bReconstructedObject As Boolean '20170214 baluu edit( added , bReconstructedObject As Boolean )
            Dim dSize As Double, dTextH As Double
            Dim strDxfName As String
            Try
                bLookPos = cadOutFrm.CheckBox_LookPoint.Checked
                bRay = cadOutFrm.CheckBox_Ray.Checked
                bLabelText = cadOutFrm.CheckBox_LabelText.Checked()
                bUserElm = cadOutFrm.CheckBox_UserElm.Checked
                bReconstructedObject = cadOutFrm.CheckBox_Reconstruction.Checked
                dSize = entset_point.screensize
              
                dTextH = CDbl(cadOutFrm.TextBox_TextH.Text)
                ' iOutCoord = (cadOutFrm.ComboBox_OutDim.SelectedIndex)
                strDxfName = cadOutFrm.TextBox_CADFileName.Text
            Catch ex As Exception
                MsgBox("The input date are incorrect." + vbCrLf + ex.Message)
                Return
            End Try
            If File.Exists(strDxfName) = True Then
                Try
                    File.Delete(strDxfName)
                Catch ex As Exception
                    MsgBox("出力ファイルは既に別のプロセスで使用中です。別名にするか、現在のファイルを閉じてください。")
                    Exit Sub
                End Try

            End If
            Using file As System.IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(strDxfName, False, System.Text.Encoding.Default)
                file.WriteLine("  0")
                file.WriteLine("SECTION")
                file.WriteLine("  2")
                file.WriteLine("ENTITIES")

                '3Dオブジェクトを出力
                If (bReconstructedObject = True) Then Call exportReconstructedObject(file)
                '計測点を出力
                If (bLookPos = True) Then exportLookPoints(file)


                '      If (bRay = True) Then Call exportRay(file)

                '--計測ラベルの書き出し
                If (bLabelText = True) Then Call exportLabelText(file, dTextH)

                '--ユーザ作成図形の書き出し
                If (bUserElm = True) Then Call exportUserElm(file)


                file.WriteLine("  0")
                file.WriteLine("ENDSEC")
                file.WriteLine("  0")
                file.WriteLine("EOF")

            End Using

            If MsgBox("DXFファイルの書き出しを終了しました。開きますか？", MsgBoxStyle.YesNo, "確認") = MsgBoxResult.Yes Then
                System.Diagnostics.Process.Start(strDxfName)
            End If

        End If
    End Sub
    Private Sub DXFOUT()
        Dim bln_cadOpen As Boolean = False

        '(20140605 Tezuka ADD) ３D画像が表示されていない時はメッセージ出力して終わる
        If bln_KaisekiOpened = False Then
            MsgBox("３Ｄ画像が表示されていません。" + vbCrLf + "解析を実行するか詳細表示ボタンを押して、" + vbCrLf + _
                   "３Ｄ画像を表示してから再実行してください", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "ＤＸＦ出力エラー")
            Exit Sub
        End If

        Dim cadOutFrm As New YCM_CADOutOption
        cadOutFrm.CheckBox_LookPoint.Checked = False
        cadOutFrm.CheckBox_Ray.Checked = False
        cadOutFrm.CheckBox_LabelText.Checked = False
        cadOutFrm.CheckBox_UserElm.Checked = True
        cadOutFrm.CheckBox_Reconstruction.Checked = False '20170214 baluu add

        'Rep By Yamada 20140922 Sta ----------------------日本車輌
        cadOutFrm.ComboBox_OutDim.SelectedIndex = 1
        'cadOutFrm.ComboBox_OutDim.SelectedIndex = 0
        'Rep By Yamada 20140922 End ----------------------日本車輌

        cadOutFrm.CheckBox_Information.Checked = True
        'SUURI UPDATE 20140914

        Dim strDXFdefaultName As String = WorksD.GetDefaultDXGFileName
        cadOutFrm.TextBox_CADFileName.Text = MainFrm.objFBM.ProjectPath & "\" & strDXFdefaultName
        'SUURI UPDATE 20140914
        cadOutFrm.TextBox_TextH.Text = "10"
        cadOutFrm.isDwgFilter(False)
        cadOutFrm.ShowDialog()

        Dim iOutCoord As Integer  '--出力次元


        If (cadOutFrm.DialogResult = System.Windows.Forms.DialogResult.OK) Then

            '=======================================================================
            ' read and check the information in Dialog
            '=======================================================================
            Dim bLookPos As Boolean, bRay As Boolean, bLabelText As Boolean, bUserElm As Boolean, bReconstructedObject As Boolean '20170214 baluu edit( added , bReconstructedObject As Boolean )
            Dim dSize As Double, dTextH As Double
            Dim strDxfName As String
            Try
                bLookPos = cadOutFrm.CheckBox_LookPoint.Checked
                bRay = cadOutFrm.CheckBox_Ray.Checked
                bLabelText = cadOutFrm.CheckBox_LabelText.Checked()
                bUserElm = cadOutFrm.CheckBox_UserElm.Checked
                bReconstructedObject = cadOutFrm.CheckBox_Reconstruction.Checked '20170214 baluu add

                dSize = entset_point.screensize '★20121115計測点
                '========================================================================================================20121115
                'dSize = entset_point1.screensize '★20121114計測点(ターゲット面)
                'dSize = entset_point2.screensize '★20121114計測点(中心円)
                'dSize = entset_point3.screensize '★20121114計測点(十字線)
                '========================================================================================================20121115
                dTextH = CDbl(cadOutFrm.TextBox_TextH.Text)
                iOutCoord = (cadOutFrm.ComboBox_OutDim.SelectedIndex)
                strDxfName = cadOutFrm.TextBox_CADFileName.Text
            Catch ex As Exception
                MsgBox("The input date are incorrect." + vbCrLf + ex.Message)
                Return
            End Try


            '=======================================================================
            ' Initialize Teigha.
            '=======================================================================
            Dim Serv As Teigha.Core.ExSystemServices
            Dim _hostApp As MyHostAppServ
            Try
                Serv = New Teigha.Core.ExSystemServices()
                _hostApp = New MyHostAppServ()
                Teigha.TD.TD_Db.odInitialize(Serv)
            Catch ex As Exception
                MsgBox("Failed to initialize Teigha.")
                Return
            End Try

            Try
                'Dim pDb As Teigha.TD.OdDbDatabase = _hostApp.createDatabase(True, Teigha.TD.MeasurementValue.kEnglish)
                Using pDb As Teigha.TD.OdDbDatabase = _hostApp.createDatabase(True, Teigha.TD.MeasurementValue.kEnglish)

                    'add by liwei.z 20140527 start
                    SetStandardTextStyleFont(pDb)
                    'add by liwei.z 20140527 end

                    Dim cad As New ODOperator(pDb)

                    '' 2014-5-26 str by Ljj
                    ' loadData()
                    ' 2014-5-26 end by Ljj

                    ' loadData()

                    '--計測点の書き出し

                    If (bLookPos = True) Then exportLookPoints(cad, iOutCoord)

                    '--レイの書き出し

                    If (bRay = True) Then Call exportRay(cad, iOutCoord)

                    '--計測ラベルの書き出し

                    If (bLabelText = True) Then Call exportLabelText(cad, dTextH, iOutCoord)

                    '--ユーザ作成図形の書き出し

                    If (bUserElm = True) Then Call exportUserElm(cad, iOutCoord)

                    If (bReconstructedObject = True) Then Call exportReconstructedObject(cad, iOutCoord) '20170214 baluu add


                    '--作成したCAD図の保存



                    ' 2014-5-26 str by Ljj
                    '出力情報
                    Dim ij As Integer
                    Dim pt As New GeoPoint
                    Dim ihd As Integer = 10
                    ' 2014-5-30 str by Ljj
                    Dim pRS As New GeoPoint
                    Dim pRE As New GeoPoint
                    Dim pRMaxX As Integer
                    Dim pRMaxY As Integer
                    Dim pRMinX As Integer  'SUURI ADD 20140913
                    Dim pPminY As Integer

                    'SUURI ADD 20140914
                    GetBoundingBox_OutZU(iOutCoord, pRMaxX, pRMaxY, pRMinX, pPminY)


                    'SUURI UPDATE START 20140913
                    'Dim iX As Integer = pRMaxX + 200
                    'Dim iY As Integer = (pRMaxY + pPminY) / 2
                    Dim iX As Integer = pRMinX
                    Dim iY As Integer = pRMaxY + (WorksD.KihonL.Count * 20)
                    'SUURI UPDATE END 20140913
                    Dim iZ As Integer = 0
                    ' 2014-5-30 end by Ljj

                    If cadOutFrm.CheckBox_Information.Checked = True Then
                        Try
                            cad.CreateLayer(entset_label.layerName) ' "LABELTEXT")
                            cad.CreateLayerEntSet(entset_label)
                            For ij = 0 To WorksD.KihonL.Count - 1
                                Call pt.setXYZ(iX, iY - ij * 20, iZ)
                                'cad.AddText(WorksD.KihonL(ij).item_name + " : " + WorksD.KihonL(ij).item_value, pt, ihd, entset_label.layerName)
                                cad.AddText_Information(WorksD.KihonL(ij).item_name + " : " + WorksD.KihonL(ij).item_value, pt, ihd, entset_label.layerName)
                            Next
                        Catch ex As Exception
                            MsgBox("CAD図形の情報出しを失敗しました" + ex.Message)
                        End Try
                    End If
                    ' 2014-5-26 end by Ljj



                    pDb.writeFile(strDxfName, _
                                  Teigha.TD.SaveType.kDxf, Teigha.Core.DwgVersion.vAC15)

                    'Rep By Yamada 20150304 Sta -------
                    If MsgBox("CAD図形の書き出しを終了しました。開きますか？", MsgBoxStyle.YesNo, "確認") = MsgBoxResult.Yes Then
                        bln_cadOpen = True
                    End If
                    'Rep By Yamada 20150304 End -------

                End Using
            Catch ex As Exception
                MsgBox("CAD図形の書き出しを失敗しました。" + vbCrLf + ex.Message)
            Finally
                '=======================================================================
                ' Uninitialize Teigha.
                '=======================================================================
                Try
                    ' start : crash in writeFile without this codes
                    _hostApp.Dispose()
                    ' end   : crash in writeFile without this codes
                    Teigha.TD.TD_Db.odUninitialize()
                Catch ex As Exception
                    MsgBox("Failed to uninitialize Teigha.")
                End Try
            End Try
        End If
        If bln_cadOpen = True Then
            System.Diagnostics.Process.Start(cadOutFrm.TextBox_CADFileName.Text)
        End If
    End Sub
    ''Rep By Yamada 20140919 End ------------------------------------------------------------------------------------日本車輌

    'SUURI ADD START 20140914
    Private Sub GetBoundingBox_OutZU(ByVal iOutCoord As Integer,
                                     ByRef pRMaxX As Integer,
                                     ByRef pRMaxY As Integer,
                                     ByRef pRMinX As Integer,
                                     ByRef pPMinY As Integer
                                    )
        Dim pRS As New GeoPoint
        Dim pRE As New GeoPoint
        Dim sumX As Integer
        Dim sumY As Integer

        Dim arrPtX As ArrayList = New ArrayList
        Dim arrPtY As ArrayList = New ArrayList

        For ik = 0 To nUserLines - 1
            ' start point
            pRS = gDrawUserLines(ik).startPnt.Copy
            localToGlobalPoint(pRS, sys_CoordInfo.mat)
            '--出力次元による座標値の調整
            Call transOutCoord(iOutCoord, pRS.x, pRS.y, pRS.z)
            arrPtX.Add(pRS.x)
            arrPtY.Add(pRS.y)
            pRE = gDrawUserLines(ik).endPnt.Copy
            localToGlobalPoint(pRE, sys_CoordInfo.mat)
            Call transOutCoord(iOutCoord, pRE.x, pRE.y, pRE.z)
            arrPtX.Add(pRE.x)
            arrPtY.Add(pRS.y)
        Next
        'SUURI ADD START 20140914
        '出力図形範囲は線図形のみならず円形も対象にする。
        For ik = 0 To nCircleNew - 1
            '  pRS=gDrawCircleNew (ik).
            pRS = gDrawCircleNew(ik).org.Copy
            localToGlobalPoint(pRS, sys_CoordInfo.mat)
            '--出力次元による座標値の調整
            Call transOutCoord(iOutCoord, pRS.x, pRS.y, pRS.z)
            arrPtX.Add(pRS.x)
            arrPtY.Add(pRS.y)
        Next
        'SUURI ADD END 20140914

        pRMaxX = arrPtX(0)
        pRMinX = arrPtX(0)  'SUURI ADD 20140913
        pPMinY = arrPtY(0)
        pRMaxY = arrPtY(0)

        For ii = 0 To arrPtX.Count - 1
            If ii < arrPtX.Count - 1 Then
                sumX = arrPtX(ii + 1)
            Else
                sumX = arrPtX(arrPtX.Count - 1)
            End If
            'Get max  X
            If pRMaxX < sumX Then
                pRMaxX = sumX
            End If
            'Get min X 'SUURI ADD 20140913
            If pRMinX > sumX Then
                pRMinX = sumX
            End If


            'Get max and min Y
            If ii < arrPtY.Count - 1 Then
                sumY = arrPtY(ii + 1)
            Else
                sumY = arrPtY(arrPtX.Count - 1)
            End If

            If pRMaxY < sumY Then
                pRMaxY = sumY
            End If

            If pPMinY > sumY Then
                pPMinY = sumY
            End If
        Next
    End Sub
    'SUURI ADD END 20140914

    'add by liwei.z 20140527 start
    Private Sub SetStandardTextStyleFont(ByRef pDb As Teigha.TD.OdDbDatabase)
        Dim objId As Teigha.TD.OdDbObjectId = pDb.getTextStyleStandardId()
        If objId.isNull() Then
            Exit Sub
        End If
        Dim pObj As Teigha.TD.OdDbObject
        If objId.openObject(pObj, Teigha.TD.OpenMode.kForWrite) <> Teigha.Core.OdResult.eOk Then
            Exit Sub
        End If
        Dim objTextStyleRec As Teigha.TD.OdDbTextStyleTableRecord = CType(pObj, Teigha.TD.OdDbTextStyleTableRecord)
        objTextStyleRec.setFont("MS Mincho", False, False, 128, 17)
        objTextStyleRec.Dispose()
    End Sub
    'add by liwei.z 20140527 end

    Private Shared Sub loadData()
        ' load data to output dwg/dxf file for function Button2_Click_1()
        ' copy the codes from YCM_Command.vb Command_Open()

        Dim strdatabasefile As String = ""
        m_strDataBasePath = m_koji_kanri_path
        ' m_strDataBasePath = ComSel_SelectFolderByShell("計測データ.mdbが入っているフォルダーを指定して下さい。", False)
        If m_strDataBasePath <> Nothing Then
            strdatabasefile = m_strDataBasePath & "\計測データ.mdb"
            Dim iodatabasefile As New IO.FileInfo(strdatabasefile)
            If iodatabasefile.Exists = True Then

                g_InputIsavePath = m_strDataBasePath
                Dim strtempend As String = m_strDataBasePath & "\計測データ_Temp.mdb"
                FileCopy(strdatabasefile, strtempend)
                g_InputIsavePath = m_strDataBasePath
                m_strDataBasePath = strtempend
                Dim clsOPe As New CDBOperate

                If clsOPe.ConnectDB(m_strDataBasePath) = False Then
                    'MsgBox("Error!")
                    Exit Sub
                End If
                Call YCM_UpdateDBbyLayer(clsOPe)   '--「dispsetting」「userfigure」テーブルに[レイヤ名]フィールドを追加
                '--ins.start----------------------12.10.12
                ' 座標補間点情報テーブルが存在しない場合には作成する
                YCM_AddNewInterPosInfoTable(clsOPe)
                '--ins.end------------------------12.10.12
                clsOPe.DisConnectDB()

                Dim intmat As Integer
                Dim dblmat(1, 15) As Double
                YCM_ReadSystemscalesettingAcs(m_strDataBasePath)
                YCM_ReadSystemscalesettingAcsvalue(m_strDataBasePath)
                YCM_ReadSystemscalesettingAcsP1_2(m_strDataBasePath) '13.1.25　スケール設定を行った2点をDB（userscalesetting）から呼び出す

                intmat = YCM_ReadSystemscalesettingAcsmat(m_strDataBasePath)

                If intmat = 1 Then
                    sys_CoordInfo.mat = YCM_GetUnitMat()
                End If
                If intmat = 2 Then
                    For ii As Integer = 0 To 15
                        sys_CoordInfo.mat(ii) = CDbl(system_scalesettingmat(0, ii))
                    Next
                End If
                If nscalesettingvalue = 0 Then
                    sys_ScaleInfo.scale = 1.0
                Else
                    sys_ScaleInfo.scale = System_scalesettingvalue(0, nscalesetting - 1)
                End If

#If False Then
                '要素をなくす
                ReDim gDrawRays(0)
                ReDim gDrawCamers(0)
                ReDim gDrawPoints(0)
                ReDim gDrawLabelText(0)
                ReDim gDrawUserLines(0)
                ReDim gDrawCircleNew(0)
                ReDim gDrawCirclepoint(0)
                nRays = 0
                nCamers = 0
                nLookPoints = 0
                nLabelText = 0
                nUserLines = 0
                ncirclepoint = 0
                nCircleNew = 0
                gScaleLine = Nothing '13.1.28スケール設定のライン

                Try
                    Command_LoadToDb()
                Catch ex As Exception
                    MessageBox.Show(ex.ToString())
                End Try
#End If

                YCM_ReadZumenInfoFrmAcs(m_strDataBasePath)
                YCM_ReadSystemColorAcs(m_strDataSystemPath)
                YCM_ReadSystemLineTypes(m_strDataSystemPath)
                YCM_ReadEntSetting(m_strDataBasePath)
            Else
                MsgBox("計測データ.mdbが存在しません。")
            End If

        End If
    End Sub

    Private Sub ListViewTabItems_SelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles ListViewTabItems.SelectionChanged
        Static SelectedIndexChangedBySelectedIndexChanging As Boolean = False

        If Me.ListViewTabItems.Tag IsNot Nothing Then
            If Me.ListViewTabItems.Tag = True Then
                Return
            End If
        End If

        ' check whether a TabItem is added and removed, if true, go on
        If e.AddedItems.Count = 1 And e.RemovedItems.Count = 1 Then
            If Not SelectedIndexChangedBySelectedIndexChanging Then
                TabControl1.SelectedIndex = ListViewTabItems.SelectedIndex
                If ListViewTabItems.SelectedIndex <> TabControl1.SelectedIndex Then
                    SelectedIndexChangedBySelectedIndexChanging = True
                    ListViewTabItems.SelectedIndex = TabControl1.SelectedIndex
                    SelectedIndexChangedBySelectedIndexChanging = False
                End If
            End If
        End If
    End Sub
    Private Sub TabControl1_SelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles TabControl1.SelectionChanged

        '(20140529 Tezuka ADD) タブコントロール以外からのイベントは何もしない
        If Not e.Source.Equals(Me.TabControl1) Then
            Exit Sub
        End If

        ' the previous Selected TabItem when this function is called
        Static previousSelectedTabItem As System.Windows.Controls.TabItem = Nothing

        Static SelectedIndexChangedBySelectedIndexChanging As Boolean = False

        If Me.TabControl1.Tag Is Nothing Then
            Me.TabControl1.Tag = False
        End If

        If Me.TabControl1.Tag = False Then
            ' check whether a TabItem is added and removed, if true, go on
            If e.AddedItems.Count = 1 And e.RemovedItems.Count = 1 Then
                If Not SelectedIndexChangedBySelectedIndexChanging Then
                    Dim cancelSelectedIndexChanged As Boolean = False
                    TabControl1_SelectedIndexChanging(cancelSelectedIndexChanged)
                    If Not cancelSelectedIndexChanged Then
                        TabControl1_SelectedIndexChanged()
                    Else
                        SelectedIndexChangedBySelectedIndexChanging = True
                        TabControl1.SelectedItem = previousSelectedTabItem
                        SelectedIndexChangedBySelectedIndexChanging = False
                    End If
                End If
            End If
        End If

        ' update previousSelectedTabItem
        previousSelectedTabItem = TabControl1.SelectedItem
    End Sub
    Private Sub TabControl1_SelectedIndexChanging(ByRef cancel As Boolean)
        If TabControl1.SelectedItem IsNot Nothing Then
            Dim item As System.Windows.Controls.TabItem = TabControl1.SelectedItem
            Select Case item.Name
                Case TabKihonInfo.Name  ' 基本情報
                    '基本情報が更新されている
                    cancel = Not GamenUpdateCheck(1)
                Case TabKiteiti.Name    ' 規定値
                    cancel = Not GamenUpdateCheck(2)
                Case TabGazou.Name      ' 画像

                    cancel = Not GamenUpdateCheck(3)
                Case TabKaiseki.Name    ' 解析

                Case TabKakunin.Name    ' 寸法確認

                    cancel = Not GamenUpdateCheck(5)
                    'Case TabOut.Name        ' 検査表出力

            End Select
        End If
    End Sub
    Private Sub TabControl1_SelectedIndexChanged()
        If TabControl1.SelectedItem IsNot Nothing Then
            Dim item As System.Windows.Controls.TabItem = TabControl1.SelectedItem

            Select Case item.Name
                Case TabKihonInfo.Name  ' 基本情報
                    SetKihon()
                Case TabKiteiti.Name    ' 規定値
                    SetKiteiti()
                Case TabGazou.Name      ' 画像

                    SetGazou()
                Case TabKaiseki.Name    ' 解析

                    SetKaiseki(True)
                Case TabKakunin.Name    ' 寸法確認

                    SetSunpouKakunin()
                    'Case TabOut.Name        ' 検査表出力

                    'SetOut()
            End Select
        End If
    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        initListViewTabItems()
    End Sub
    Private Sub initListViewTabItems()
        ListViewTabItems.ItemsSource = Nothing

        Const ImgPath As String = "/YCM;component/Image2/KentouKihonInfoTabDialog/"

        Dim item As MyListViewItem

        item = New MyListViewItem("基本情報",
                                  ImgPath + "1_1.png",
                                  ImgPath + "1_2.png")
        m_ListViewTabItemsItems.Add(item)

        item = New MyListViewItem("規定値",
                                  ImgPath + "2_1.png",
                                  ImgPath + "2_2.png")
        m_ListViewTabItemsItems.Add(item)

        item = New MyListViewItem("画像",
                                  ImgPath + "3_1.png",
                                  ImgPath + "3_2.png")
        m_ListViewTabItemsItems.Add(item)

        item = New MyListViewItem("解析",
                                  ImgPath + "4_1.png",
                                  ImgPath + "4_2.png")
        m_ListViewTabItemsItems.Add(item)

        item = New MyListViewItem("寸法確認",
                                  ImgPath + "5_1.png",
                                  ImgPath + "5_2.png")
        m_ListViewTabItemsItems.Add(item)

        item = New MyListViewItem("検査表出力",
                                  ImgPath + "6_1.png",
                                  ImgPath + "6_2.png")
        m_ListViewTabItemsItems.Add(item)

        ListViewTabItems.ItemsSource = m_ListViewTabItemsItems
    End Sub

    Private Sub BtnKojiData_Click_1(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BtnKojiData.Click

    End Sub

    '(20140528 Teztka ADD) カメラ選択変更対応
    Private Sub CmbCamera_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles CmbCamera.SelectionChanged
        UpdateCameraInfo()
    End Sub

    '(20140529 Tezuka ADD) DataGridView9のCellChangeイベント発行
    Private Sub DataGridView9_CurrentCellChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridView9.CurrentCellChanged

        ' init OldDataGridView9Item and OldColumnIndex
        Static OldDataGridView9Item As KakuninViewItem = Nothing
        Static OldColumnIndex2 As Integer = -1

        '(20140530 Tezuka ADD) ComboBoxクリック対応
        If DataGridView9.CurrentColumn IsNot Nothing Then
            DataGridView9.Focus()
            DataGridView9.BeginEdit()
        End If

        ' copy previous selected cell value to 
        If OldColumnIndex2 >= 0 And OldDataGridView9Item IsNot Nothing Then
            DataGridView9_CellValueChanged(OldColumnIndex2, OldDataGridView9Item)
        End If

        ' update OldKekkaKakuninViewItem and OldColumnIndex
        OldDataGridView9Item = DataGridView9.CurrentItem
        If DataGridView9.CurrentColumn Is Nothing Then
            OldColumnIndex2 = -1
        Else
            OldColumnIndex2 = DataGridView9.CurrentColumn.DisplayIndex
        End If
    End Sub

    Private Sub DataGridView9_CellValueChanged(ByVal ColumnIndex As Integer, ByVal item As KakuninViewItem)

        Dim iFlg As Integer = 0

        If m_CellChange = 1 Then

            '列番号毎に格納するデータを決定する


            Select Case ColumnIndex
                Case 2
                    WorksD.SunpoSetL(item.RowIndex).SunpoMark = item.Bui            '部位
                    iFlg = 1
                Case 3
                    WorksD.SunpoSetL(item.RowIndex).SunpoName = item.KensaItem        '検査項目
                    iFlg = 1
                Case 4
                    WorksD.SunpoSetL(item.RowIndex).KiteiVal = item.KiteiVal      '規定値
                    iFlg = 1
                Case 5 '20161110 baluu ADD
                    Exit Sub
                Case 6
                    WorksD.SunpoSetL(item.RowIndex).KiteiMin = item.KiteiMin    '最小許容値
                    iFlg = 1
                Case 7
                    WorksD.SunpoSetL(item.RowIndex).KiteiMax = item.KiteiMax     '最大許容値
                    iFlg = 1
                Case 8
                    WorksD.SunpoSetL(item.RowIndex).SunpoVal = item.SunpoVal       '計測値
                    iFlg = 1
                Case 9 '20161110 baluu ADD
                    Exit Sub
                Case 10
                    Exit Sub
                Case 11
                    Exit Sub
                Case 12
                    If item.flgOutZu = True Then
                        WorksD.SunpoSetL(item.RowIndex).flgOutZu = 1
                    Else
                        WorksD.SunpoSetL(item.RowIndex).flgOutZu = 0
                    End If
                    If WorksD.SunpoSetL(item.RowIndex).flgKeisan <> "0" Then
                        ZukeiColLTypeChg(WorksD.SunpoSetL(item.RowIndex))
                    End If
                    iFlg = 1
                Case 13
                    ncolor = 0
                    YCM_ReadSystemColorAcs(m_strDataSystemPath)
                    Dim mcolor As ModelColor
                    If ncolor > 0 Then
                        mcolor = YCM_GetColorInfoByName(item.ZU_color2)
                        WorksD.SunpoSetL(item.RowIndex).ZU_colorID = mcolor.code
                    End If
                    If WorksD.SunpoSetL(item.RowIndex).flgKeisan <> "0" Then
                        ZukeiColLTypeChg(WorksD.SunpoSetL(item.RowIndex))
                    End If
                    iFlg = 1
                Case 14

                    nLineType = 0
                    YCM_ReadSystemLineTypes(m_strDataSystemPath)
                    Dim mLinetype As LineType
                    If nLineType > 0 Then
                        mLinetype = YCM_GetLineTypeInfoByName(item.ZU_LineType2)
                        WorksD.SunpoSetL(item.RowIndex).ZU_LineTypeID = mLinetype.code
                    End If
                    If WorksD.SunpoSetL(item.RowIndex).flgKeisan <> "0" Then
                        ZukeiColLTypeChg(WorksD.SunpoSetL(item.RowIndex))
                    End If
                    iFlg = 1
                Case 15
                    WorksD.SunpoSetL(item.RowIndex).ZU_layer = item.ZU_layer       'レイヤ変更
                    'SUURI ADD　20140907
                    If WorksD.SunpoSetL(item.RowIndex).flgKeisan <> "0" Then
                        ZukeiColLTypeChg(WorksD.SunpoSetL(item.RowIndex))
                    End If
                    'SUURI ADD　20140907
                    iFlg = 1
                Case 16
                    If item.sek_flgOutZu = True Then
                        WorksD.SunpoSetL(item.RowIndex).sek_flgOutZu = 1
                    Else
                        WorksD.SunpoSetL(item.RowIndex).sek_flgOutZu = 0
                    End If
                    '  If WorksD.SunpoSetL(item.RowIndex).flgKeisan <> "0" Then
                    ZukeiColLTypeChgBySek(WorksD.SunpoSetL(item.RowIndex))
                    '  End If
                    iFlg = 1
                Case 17
                    ncolor = 0
                    YCM_ReadSystemColorAcs(m_strDataSystemPath)
                    Dim mcolor1 As ModelColor
                    If ncolor > 0 Then
                        mcolor1 = YCM_GetColorInfoByName(item.sek_ZU_color2)
                        WorksD.SunpoSetL(item.RowIndex).sek_ZU_colorID = mcolor1.code
                    End If
                    '   If WorksD.SunpoSetL(item.RowIndex).flgKeisan <> "0" Then
                    ZukeiColLTypeChgBySek(WorksD.SunpoSetL(item.RowIndex))
                    ' End If
                    iFlg = 1
                Case 18

                    nLineType = 0
                    YCM_ReadSystemLineTypes(m_strDataSystemPath)
                    Dim mLinetype1 As LineType
                    If nLineType > 0 Then
                        mLinetype1 = YCM_GetLineTypeInfoByName(item.sek_ZU_LineType2)
                        WorksD.SunpoSetL(item.RowIndex).sek_ZU_LineTypeID = mLinetype1.code
                    End If
                    '   If WorksD.SunpoSetL(item.RowIndex).flgKeisan <> "0" Then
                    ZukeiColLTypeChgBySek(WorksD.SunpoSetL(item.RowIndex))
                    ' End If
                    iFlg = 1
                Case 19
                    WorksD.SunpoSetL(item.RowIndex).sek_ZU_layer = item.sek_ZU_layer       'レイヤ変更
                    ' If WorksD.SunpoSetL(item.RowIndex).flgKeisan <> "0" Then
                    ZukeiColLTypeChgBySek(WorksD.SunpoSetL(item.RowIndex))
                    '  End If
                    iFlg = 1
            End Select

            If iFlg = 1 Then

                '寸法確認データを保存する
                SaveSunpoData(False)
            End If
        End If
    End Sub

    '(20141218 Tezuka ADD) ターゲット設定ボタンの処理
    Private Sub Button_TargetSetting_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_TargetSetting.Click

        Dim Form As New TargetSetting
        Form.IsNew = Me.IsNew
        Form.ShowDialog()

        '読込データの更新
        If ConnectSystemDB(m_system_dbclass) = False Then
            MsgBox(err_msg_systemdb_connect, MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        WorksD.m_dbClass = m_system_dbclass
        If WorksD.GetSunpoDataToList() = False Then
            Exit Sub
        End If
        DisConnectDB(m_system_dbclass)

    End Sub

    '寸法値のCSVファイルを出力する(TypeID = 24の時のみ)
    Public Sub Sunpo_CsvOut(ByVal path As String, ByVal fname As String)
        Dim i As Integer
        Dim syasyu As String = ""
        Dim maker As String = ""
        Dim WrtStr As String = ""
        Dim WrtHdr As String = ""
        Dim strA As String = ""
        Dim strB As String = ""
        Dim strC As String = ""
        Dim strD As String = ""
        Dim strE As String = ""
        Dim strH As String = ""
        Dim Fname_Out As String = ""

        'KihonInfoから出力値主得
        For i = 0 To WorksD.KihonL.Count - 1
            If WorksD.KihonL(i).item_name.Trim = "計測箇所名" Then
                syasyu = WorksD.KihonL(i).item_value
            End If
            If WorksD.KihonL(i).item_name.Trim = "計測者" Then
                maker = WorksD.KihonL(i).item_value
            End If
        Next i

        'SunpoSetから出力値取得
        For i = 0 To WorksD.SunpoSetL.Count - 1
            If WorksD.SunpoSetL(i).SunpoMark.Trim = "L2" Then
                strH = WorksD.SunpoSetL(i).SunpoVal.Trim
            ElseIf WorksD.SunpoSetL(i).SunpoMark.Trim = "L3" Then
                strD = WorksD.SunpoSetL(i).SunpoVal.Trim
            ElseIf WorksD.SunpoSetL(i).SunpoMark.Trim = "L4" Then
                strE = WorksD.SunpoSetL(i).SunpoVal.Trim
            ElseIf WorksD.SunpoSetL(i).SunpoMark.Trim = "L5" Then
                strB = WorksD.SunpoSetL(i).SunpoVal.Trim
            ElseIf WorksD.SunpoSetL(i).SunpoMark.Trim = "L6" Then
                strC = WorksD.SunpoSetL(i).SunpoVal.Trim
            ElseIf WorksD.SunpoSetL(i).SunpoMark.Trim = "L7" Then
                strA = WorksD.SunpoSetL(i).SunpoVal.Trim
            End If
        Next i

        '出力イメージ作成
        WrtHdr = "車種名,メーカーID,,全幅,幅(⑤),高さ(⑥),前面高さ(ハーフ)(③),,,,,,,後部高さ(ハーフ)(④),,,タイヤから後部(②),"
        WrtStr = syasyu.Trim & "," & maker & ",NULL," & _
                 strA & "," & strB & "," & strC & "," & _
                 strD & ",null,null,null,0,null,null," & _
                 strE & ",null,null," & strH & ",null"

        'CSVファイルに書き込む
        Fname_Out = path & "\" & fname & ".csv"
        Try
            Dim sw As New System.IO.StreamWriter(Fname_Out, False, System.Text.Encoding.UTF8)
            sw.WriteLine(WrtHdr)
            sw.WriteLine(WrtStr)
            sw.Close()
        Catch e As Exception
            MsgBox("CSVファイルの出力に失敗しました。", MsgBoxStyle.Exclamation, "CSVファイル出力")
            Exit Sub
        End Try

        MsgBox("CSVファイルの出力に成功しました。", MsgBoxStyle.Information, "CSVファイル出力")

    End Sub

    Private Sub DataGridView9_MouseDoubleClick(sender As Object, e As Windows.Input.MouseButtonEventArgs) Handles DataGridView9.MouseDoubleClick
        Dim OldDataGridView9Item As KakuninViewItem = Nothing

        OldDataGridView9Item = DataGridView9.CurrentItem
        Dim intSunpoSetID = OldDataGridView9Item.No
        If IOUtil Is Nothing Then
            MsgBox("解析画面の詳細確認をしてください。", MsgBoxStyle.OkOnly, "確認")
            Exit Sub
        End If
        For Each SST As SunpoSetTable In WorksD.SunpoSetL
            If SST.SunpoID = intSunpoSetID Then
                IOUtil.activeSunpoSet = SST
            End If
        Next

        IOUtil.EndThread()
        IOUtil.LibCommand("NinniTenShitei")

    End Sub

    Public Sub NewSyoriSUEOKI()

        If ConnectSystemDB(m_system_dbclass) = False Then
            MsgBox(err_msg_systemdb_connect, MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        If AddDBflgScale(m_system_dbclass) = 0 Then
            ChgDBflgScale(m_system_dbclass)
        End If
        '    '
        WorksD = New WorksTable
        WorksD.m_dbClass = m_system_dbclass
        If WorksD.GetDataToList() = False Then
            Exit Sub
        End If

        'END SHUU
        Dim TGD As New TenGunTeigiTable
        TGD.m_dbClass = m_system_dbclass
        lstTengun = TGD.GetDataToList()


        '計測データ.mdbを切断する
        DisConnectDB(m_system_dbclass)



        If System.IO.Directory.Exists(m_koji_kanri_path) = True Then
            If System.IO.File.Exists(m_koji_kanri_path & "\" & m_Keisoku_mdb_name) = True Then
                MsgBox("既に計測データが存在します。" & vbCrLf & "別のフォルダを指定して下さい。", MsgBoxStyle.OkOnly)
                Exit Sub
            Else
                '計測データ.mdbが無い場合、コピーする
                ' m_koji_kanri_path = TxtKojiData.Text.Trim
                System.IO.File.Copy(m_KeisokuTempMdbPath, m_koji_kanri_path & "\" & m_Keisoku_mdb_name)
            End If
        Else
            MsgBox("工事データを指定して下さい。", MsgBoxStyle.YesNo)
            Exit Sub
        End If

        '規定値入力

        'コピーした計測データ.mdbに寸法算出テーブルの規定値・最小許容値・最大許容値を設定する

        My.Settings.strLastProjPath = m_koji_kanri_path
        My.Settings.Save()

        m_Keisoku_mdb_path = m_koji_kanri_path & "\" & m_Keisoku_mdb_name

        m_koji_kanri_path = m_koji_kanri_path

        '検査表出力フォルダに設定する

        KensaHyoOutput.TxtExcelFolder.Text = m_koji_kanri_path

        '画面の入力値を取得する

        '    GetDataFromToControl()

        '計測データDBに接続する

        If ConnectDB(m_koji_kanri_path & "\" & m_Keisoku_mdb_name, m_keisoku_dbclass) = False Then
            MsgBox("計測データ.mdbを開くことができません。")
            Exit Sub
        End If

        If AddDBflgScale(m_keisoku_dbclass) = 0 Then
            ChgDBflgScale(m_keisoku_dbclass)
        End If

        '計測データ.mdbに入力値を書き込む
        'KihonInfoの更新
        If SetKeisokuData(m_koji_kanri_path & "\" & m_Keisoku_mdb_name) = False Then
            DisConnectDB(m_keisoku_dbclass)
            MsgBox("計測データ更新に失敗しました。(基本infoエラー)", MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        '計測データ.mdbのTenGunTeigiに書き込む
        'SUURI ADD START 点群定義テーブルも工事固有のデータにする。
        If SetTengunTeigData() = False Then
            DisConnectDB(m_keisoku_dbclass)
            MsgBox("計測データ更新に失敗しました。(点群定義エラー)", MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        '   Dim TGD As New TenGunTeigiTable
        TGD.m_dbClass = m_keisoku_dbclass
        lstTengun = TGD.GetDataToList()
        'SUURI ADD END 

        '計測データ.mdbのSunpouInfoに書き込む
        If SetSunpouData() = False Then
            DisConnectDB(m_keisoku_dbclass)
            MsgBox("計測データ更新に失敗しました。(SunpoInfoエラー)", MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        '計測データ.mdbのCameraInfoに書き込む
        If SetCameraInfoData() = False Then
            DisConnectDB(m_keisoku_dbclass)
            MsgBox("計測データ更新に失敗しました。(カメラインフォエラー)", MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        '計測データ.mdbのExcelTemplateに書き込む
        If SetExcelTemplateData() = False Then
            DisConnectDB(m_keisoku_dbclass)
            MsgBox("計測データ更新に失敗しました。(ExcelTemplateエラー)", MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        '計測データ.mdbのSunpoCalcHouhouに書き込む
        If SetSunpoCalcHouhouData() = False Then
            DisConnectDB(m_keisoku_dbclass)
            MsgBox("寸法計算方法データ更新に失敗しました。SunpoCalcエラー", MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        '計測データ.mdbを切断する
        DisConnectDB(m_keisoku_dbclass)

        'システム設定mdbに接続する

        If ConnectSystemDB(m_system_dbclass) = False Then
            MsgBox(err_msg_systemdb_connect, MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        'システム設定.mdbを更新する
        'Works, WorksInfoの更新
        If SetSystemData(m_SystemMdbFullPath, m_koji_kanri_path) = False Then
            MsgBox("システム設定データ更新に失敗しました。", MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        '基本情報処理フラグを更新する
        WorksD.SetKihonFlg()

        'システム設定.mdbを切断する
        DisConnectDB(m_system_dbclass)

        '(20140527 Tezuka Change)
        If m_flg_Zahyo = True Then
            '画像画面へ
            SetGazou()
        Else
            '規定値画面へ
            If SetInitFormKitei() = False Then
                Exit Sub
            End If

            'グリッドヘッダ設定
            SetGridHeaddderKitei()

            'データを取得してグリッドに表示
            SetDataKitei()

            SetKiteiti()
        End If
        ' Else
        '  SetGazou()
        ' End If

    End Sub
    '20160823 byambaa ADD Start
    Dim groupValue As String
    Private Sub ComboBox_SelectedIndexChanged(ByVal sender As Object, _
       ByVal e As System.EventArgs) Handles ComboBox.SelectionChanged
        groupValue = ComboBox.SelectedValue
        If groupValue = "" Then
            SetKiteiDataToGrid("")
        Else
            SetKiteiDataToGrid(groupValue)
        End If
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As Object, _
       ByVal e As System.EventArgs) Handles ComboBox1.SelectionChanged
        groupValue = ComboBox1.SelectedValue
        If groupValue = "" Then
            SetSunpoDataToGrid("")
        Else
            SetSunpoDataToGrid(groupValue)
        End If
    End Sub
    'Private Sub BtnSekkei_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSekkei.Click
    '    Dim sekkeiKeisoku As New Sekkei_Keisoku
    '    sekkeiKeisoku.ShowDialog()
    'End Sub
    '20160823 byambaa ADD End

    'Private Sub BtnReadCamParam_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles BtnReadCamParam.Click

    '    Dim ofd As New OpenFileDialog()

    '    ofd.FileName = ""
    '    ofd.InitialDirectory = IO.Path.GetDirectoryName(My.Settings.strCamparam)
    '    ofd.Filter = "calファイル(*.cal;)|*.cal;|すべてのファイル(*.*)|*.*"
    '    ofd.FilterIndex = 3
    '    ofd.Title = "開くファイルを選択してください"
    '    ofd.RestoreDirectory = True
    '    ofd.CheckFileExists = True
    '    ofd.CheckPathExists = True

    '    'ダイアログを表示する
    '    If ofd.ShowDialog() = Windows.Forms.DialogResult.OK Then
    '        'OKボタンがクリックされたとき、選択されたファイル名を表示する
    '        My.Settings.strCamparam = ofd.FileName
    '    End If

    'End Sub

End Class