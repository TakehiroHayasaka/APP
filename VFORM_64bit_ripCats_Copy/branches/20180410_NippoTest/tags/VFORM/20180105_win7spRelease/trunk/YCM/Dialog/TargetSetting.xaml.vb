Imports FBMlib

Public Class TargetSetting

    Private Shared m_TagetSetItems As New System.Collections.ObjectModel.ObservableCollection(Of TargetSetItem)
    Private m_TagetSetItems_S As New System.Collections.ObjectModel.ObservableCollection(Of TargetSetItem)

    ''ADD By Yamada 20150323 Sta -----
    'Private intLastRow As Integer       '最終編集列番号
    'Private intLastCol As Integer       '最終編集行番号
    ''DataGridViewのTextBoxセルを宣言
    'Private TextEditCtrl As System.Windows.Controls.TextBox
    ''ADD By Yamada 20150323 End -----

    Class TargetSetItem             'ターゲットテーブル要素
        Public Property TargetNumber() As String              'TG番号()
        'Public Property TargetNumber() As String             'TG番号()
        Public Property TargetOffsetX() As String             'オフセットX
        Public Property TargetOffsetY() As String             'オフセットY
        Public Property TargetOffsetZ() As String             'オフセットZ
        Public Property TargetInfo() As String                'Info
        Public Property OpeFlg() As Integer                   '操作フラグ（0:未操作　1:更新　2:削除　3:追加）
        Public Property UseFlg() As Integer                   '使用フラグ（0:未使用　1:使用中）
        Public Property ID() As Integer                       'データID
    End Class

    Private m_ScaleItems As New System.Collections.ObjectModel.ObservableCollection(Of ScaleItem)
    Class ScaleItem            'スケールテーブル要素
        Public Property StartTarget() As String         '開始TG番号
        'Public Property EndTarget() As Double          '終了TG番号
        Public Property EndTarget() As String           '終了TG番号
        Public Property SunpoMark() As String           '寸法マーク
        Public Property SunpoName() As String           '寸法名
        Public Property KiteiVal() As String            '規定値
        Public Property flgScale() As Integer           'スケールフラグ(1:基準スケール　0:検証用スケール　-1:それ以外)
        Public Property OpeFlg() As Integer             '操作フラグ（0:未操作　1:更新　2:削除　3:追加）
        Public Property SunpoID() As Integer            '寸法ID
        Public Shared Property ScaleMarks As System.Collections.ObjectModel.ObservableCollection(Of String)
        Public Shared ReadOnly Property CoordTargets As System.Collections.ObjectModel.ObservableCollection(Of String)
            Get
                Dim Cts As New System.Collections.ObjectModel.ObservableCollection(Of String)
                Dim IDX As Integer = m_TagetSetItems.Count
                Dim ii As Integer
                If IDX > 0 Then
                    For ii = 0 To IDX - 1
                        'If m_TagetSetItems.Item(ii).UseFlg = 0 Then
                        Cts.Add(m_TagetSetItems.Item(ii).TargetNumber.ToString)
                        'End If
                    Next ii
                End If
                Return (Cts)
            End Get
        End Property

        Shared Sub New()
            ScaleMarks = New System.Collections.ObjectModel.ObservableCollection(Of String)
            ScaleMarks.Add("S12")
            ScaleMarks.Add("S34")
            ScaleMarks.Add("S56")
            ScaleMarks.Add("S-A")
            ScaleMarks.Add("S-B")
        End Sub
    End Class

    Private m_ZahyouItems As New System.Collections.ObjectModel.ObservableCollection(Of ZahyouItem)
    Class ZahyouItem                '座標系テーブル要素
        'Public Property CT_Type() As Integer             '原点TG番号
        'Public Property CT_GenID() As Integer            'X軸TG番号
        'Public Property CT_XID() As Integer              'Y軸TG番号
        'Public Property CT_YID() As Integer              'Y軸TG番号
        'Public Property CT_ZID() As Integer              'Y軸TG番号
        'Public Property CT_ID1() As Integer              'Y軸TG番号
        'Public Property CT_ID2() As Integer              'Y軸TG番号
        'Public Property CT_ID3() As Integer              'Y軸TG番号
        Public Property CT_Type() As String               'ターゲット種類
        Public Property CT_No() As String                 'ターゲット番号
        Public Property ID() As Integer                   '
        Public Property OpeFlg() As Integer               '操作フラグ（0:未操作　1:更新　2:削除　3:追加）

        Public Shared ReadOnly Property CoordTargets As System.Collections.ObjectModel.ObservableCollection(Of String)
            Get
                Dim Cts As New System.Collections.ObjectModel.ObservableCollection(Of String)
                Dim IDX As Integer = m_TagetSetItems.Count
                Dim ii As Integer
                If IDX > 0 Then
                    For ii = 0 To IDX - 1
                        'If m_TagetSetItems.Item(ii).UseFlg = 0 Then
                        Cts.Add(m_TagetSetItems.Item(ii).TargetNumber.ToString)
                        'End If
                    Next ii
                End If
                Return (Cts)
            End Get
        End Property
    End Class

    Private m_LineLengthItems As New System.Collections.ObjectModel.ObservableCollection(Of LineLengthItem)
    Class LineLengthItem            '２点による線分・長さテーブル要素
        Public Property StartTarget() As String               '開始TG番号
        Public Property EndTarget() As String                 '終了TG番号
        Public Property Dimension() As String                 '次元
        Public Property Layer() As String                     '画層
        Public Property LineColor() As String                 '線色
        Public Property LineType() As String                  '線種
        Public Property SunpoMark() As String                 '寸法マーク
        Public Property SunpoName() As String                 '寸法名
        Public Property OpeFlg() As Integer                   '操作フラグ（0:未操作　1:更新　2:削除　3:追加）
        Public Property flgScale() As Integer                 'スケールフラグ(1:基準スケール　0:検証用スケール　-1:それ以外)
        Public Property SunpoID() As Integer                  '寸法ID
        Public Property TengunID1() As Integer                '点群ID
        Public Property TengunID2() As Integer                '点群ID
        Public Shared Property Dimensions As System.Collections.ObjectModel.ObservableCollection(Of String)
        Public Shared Property LineColors As System.Collections.ObjectModel.ObservableCollection(Of String)
        Public Shared Property LineTypes As System.Collections.ObjectModel.ObservableCollection(Of String)
        Public Shared ReadOnly Property CoordTargets As System.Collections.ObjectModel.ObservableCollection(Of String)
            Get
                Dim Cts As New System.Collections.ObjectModel.ObservableCollection(Of String)
                Dim IDX As Integer = m_TagetSetItems.Count
                Dim ii As Integer
                If IDX > 0 Then
                    For ii = 0 To IDX - 1
                        'If m_TagetSetItems.Item(ii).UseFlg = 0 Then
                        Cts.Add(m_TagetSetItems.Item(ii).TargetNumber.ToString)
                        'End If
                    Next ii
                End If
                Return (Cts)
            End Get
        End Property

        Shared Sub New()
            Dimensions = New System.Collections.ObjectModel.ObservableCollection(Of String)
            Dimensions.Add("XYZ")
            Dimensions.Add("XY")
            Dimensions.Add("XZ")
            Dimensions.Add("YZ")
            Dimensions.Add("X")
            Dimensions.Add("Y")
            Dimensions.Add("Z")

            LineColors = New System.Collections.ObjectModel.ObservableCollection(Of String)
            LineColors.Add("BLACK")
            LineColors.Add("RED")
            LineColors.Add("GREEN")
            LineColors.Add("BLUE")
            LineColors.Add("YELLOW")
            LineColors.Add("MAGENTA")
            LineColors.Add("CYAN")
            LineColors.Add("WHITE")
            LineColors.Add("DEEPPINK")
            LineColors.Add("BROWN")
            LineColors.Add("ORANGE")
            LineColors.Add("LIGHTGREEN")
            LineColors.Add("LIGHTBLUE")
            LineColors.Add("LAVENDER")
            LineColors.Add("LIGHTGRAY")
            LineColors.Add("DARKGRAY")
            LineColors.Add("SEAGREEN")
            LineColors.Add("STEELBLUE")

            LineTypes = New System.Collections.ObjectModel.ObservableCollection(Of String)
            LineTypes.Add("CONTINUOUS")
            LineTypes.Add("DASHED")
        End Sub
    End Class

    Private m_PolyLineItems As New System.Collections.ObjectModel.ObservableCollection(Of PolyLineItem)
    Class PolyLineItem              'ポリラインテーブル要素
        Public Property VertexTarget() As String              '頂点TG番号リスト
        Public Property Dimension() As String                 '次元
        Public Property Layer() As String                     '画層
        Public Property LineColor() As String                 '線色
        Public Property LineType() As String                  '線種
        Public Property SunpoMark() As String                 '寸法マーク
        Public Property SunpoName() As String                 '寸法名
        Public Property OpeFlg() As Integer                   '操作フラグ（0:未操作　1:更新　2:削除　3:追加）
        Public Property SunpoID() As Integer                  '寸法ID
        Public Property TengunID() As Integer                 '点群ID
        Public Shared Property LineColors As System.Collections.ObjectModel.ObservableCollection(Of String)
        Public Shared Property LineTypes As System.Collections.ObjectModel.ObservableCollection(Of String)
        Public Shared Property Dimensions As System.Collections.ObjectModel.ObservableCollection(Of String)

        Shared Sub New()
            Dimensions = New System.Collections.ObjectModel.ObservableCollection(Of String)
            Dimensions.Add("XYZ")
            Dimensions.Add("XY")
            Dimensions.Add("XZ")
            Dimensions.Add("YZ")
            Dimensions.Add("X")
            Dimensions.Add("Y")
            Dimensions.Add("Z")

            LineColors = New System.Collections.ObjectModel.ObservableCollection(Of String)
            LineColors.Add("BLACK")
            LineColors.Add("RED")
            LineColors.Add("GREEN")
            LineColors.Add("BLUE")
            LineColors.Add("YELLOW")
            LineColors.Add("MAGENTA")
            LineColors.Add("CYAN")
            LineColors.Add("WHITE")
            LineColors.Add("DEEPPINK")
            LineColors.Add("BROWN")
            LineColors.Add("ORANGE")
            LineColors.Add("LIGHTGREEN")
            LineColors.Add("LIGHTBLUE")
            LineColors.Add("LAVENDER")
            LineColors.Add("LIGHTGRAY")
            LineColors.Add("DARKGRAY")
            LineColors.Add("SEAGREEN")
            LineColors.Add("STEELBLUE")

            LineTypes = New System.Collections.ObjectModel.ObservableCollection(Of String)
            LineTypes.Add("CONTINUOUS")
            LineTypes.Add("DASHED")
        End Sub
    End Class

    Private m_CircleItems As New System.Collections.ObjectModel.ObservableCollection(Of CircleItem)
    Class CircleItem              '１点による円テーブル要素
        Public Property CenterTarget() As String              '中心点TG番号リスト
        'Public Property Diameter() As Double                  '直径  'Del By Yamada 20150327 
        Public Property Diameter As String                    '直径
        Public Property Dimension() As String                 '次元
        Public Property Layer() As String                     '画層
        Public Property LineColor() As String                 '線色
        Public Property LineType() As String                  '線種
        Public Property SunpoMark() As String                 '寸法マーク
        Public Property SunpoName() As String                 '寸法名
        Public Property OpeFlg() As Integer                   '操作フラグ（0:未操作　1:更新　2:削除　3:追加）
        Public Property SunpoID() As Integer                  '寸法ID
        Public Property TengunID() As Integer                 '点群ID
        Public Shared Property LineColors As System.Collections.ObjectModel.ObservableCollection(Of String)
        Public Shared Property LineTypes As System.Collections.ObjectModel.ObservableCollection(Of String)
        Public Shared Property Dimensions As System.Collections.ObjectModel.ObservableCollection(Of String)

        Shared Sub New()
            Dimensions = New System.Collections.ObjectModel.ObservableCollection(Of String)
            Dimensions.Add("XYZ")
            Dimensions.Add("XY")
            Dimensions.Add("XZ")
            Dimensions.Add("YZ")
            Dimensions.Add("X")
            Dimensions.Add("Y")
            Dimensions.Add("Z")

            LineColors = New System.Collections.ObjectModel.ObservableCollection(Of String)
            LineColors.Add("BLACK")
            LineColors.Add("RED")
            LineColors.Add("GREEN")
            LineColors.Add("BLUE")
            LineColors.Add("YELLOW")
            LineColors.Add("MAGENTA")
            LineColors.Add("CYAN")
            LineColors.Add("WHITE")
            LineColors.Add("DEEPPINK")
            LineColors.Add("BROWN")
            LineColors.Add("ORANGE")
            LineColors.Add("LIGHTGREEN")
            LineColors.Add("LIGHTBLUE")
            LineColors.Add("LAVENDER")
            LineColors.Add("LIGHTGRAY")
            LineColors.Add("DARKGRAY")
            LineColors.Add("SEAGREEN")
            LineColors.Add("STEELBLUE")

            LineTypes = New System.Collections.ObjectModel.ObservableCollection(Of String)
            LineTypes.Add("CONTINUOUS")
            LineTypes.Add("DASHED")
        End Sub
    End Class

    'CT_Bunruiデータ格納
    Public Class CTBunruiDB
        Public CT_NO As Integer
        Public flg_Use As Integer
        Public m_x As Double
        Public m_y As Double
        Public m_z As Double
        Public kubun1 As Integer
        Public kubun2 As Integer
        Public info As String
        Public OpeFlg As Integer
        Public ID As Integer
        Public TengunID As Integer
        Public UseFlg As Integer

        Public Sub New()
            CT_NO = -1
            flg_Use = -1
            m_x = 0.0
            m_y = 0.0
            m_z = 0.0
            kubun1 = -1
            kubun2 = -1
            info = ""
            OpeFlg = 0
            ID = -1
            TengunID = -1
            UseFlg = 0
        End Sub

        Public Sub GetDataByReader(ByVal IDR As IDataReader)
            CT_NO = CInt(IDR.GetValue(0).ToString.Trim)
            flg_Use = CInt(IDR.GetValue(1).ToString.Trim)
            m_x = CDbl(IDR.GetValue(2).ToString.Trim)
            m_y = CDbl(IDR.GetValue(3).ToString.Trim)
            m_z = CDbl(IDR.GetValue(4).ToString.Trim)
            kubun1 = CInt(IDR.GetValue(5).ToString.Trim)
            kubun2 = CInt(IDR.GetValue(6).ToString.Trim)
            info = IDR.GetValue(7).ToString.Trim
        End Sub

    End Class

    'SunpoSetデータ格納
    Public Class SunpoSetGunID
        Public ID As Integer
        Public SunpoID As Integer
        Public SunpoMark As String
        Public SunpoName As String
        Public SunpoCellName As String
        Public SunpoCalcHouhou As Integer
        Public CT_ID1 As Integer
        Public CT_ID2 As Integer
        Public CT_ID3 As Integer
        Public CT_Active As String
        Public GunID1 As Integer
        Public GunID2 As Integer
        Public GunID3 As Integer
        Public Dimension As Integer
        Public Layer As String
        Public LineColor As Integer
        Public LineType As Integer
        Public KiteiVal As Double
        Public SunpoVal As Double
        Public KiteiMin As Double
        Public KiteiMax As Double
        Public flg_gouhi As Integer
        Public Targettype As Integer
        Public flgOutZu As Integer
        Public flgKeisan As Integer
        Public flgScale As Integer
        Public OpeFlg As Integer

        Public Sub New()
            ID = -1
            SunpoID = -1
            SunpoMark = ""
            SunpoName = ""
            SunpoCellName = ""
            SunpoCalcHouhou = -1
            CT_ID1 = -1
            CT_ID2 = -1
            CT_ID3 = -1
            CT_Active = ""
            GunID1 = -1
            GunID2 = -1
            GunID3 = -1
            Dimension = -1
            Layer = ""
            LineColor = -1
            LineType = -1
            KiteiVal = 0.0
            SunpoVal = 0.0
            KiteiMin = 0.0
            KiteiMax = 0.0
            flg_gouhi = 0
            Targettype = 0
            flgOutZu = 1
            flgKeisan = 0
            flgScale = -1
            OpeFlg = 0
        End Sub

        Public Sub GetDataByReader(ByVal IDR As IDataReader)
            ID = CInt(IDR.GetValue(0).ToString.Trim)
            SunpoID = CInt(IDR.GetValue(1).ToString.Trim)
            SunpoMark = IDR.GetValue(2).ToString.Trim
            SunpoName = IDR.GetValue(3).ToString.Trim
            SunpoCellName = IDR.GetValue(4).ToString.Trim
            SunpoCalcHouhou = CInt(IDR.GetValue(5).ToString.Trim)
            CT_ID1 = CInt(IDR.GetValue(6).ToString.Trim)
            CT_ID2 = CInt(IDR.GetValue(7).ToString.Trim)
            CT_ID3 = CInt(IDR.GetValue(8).ToString.Trim)
            CT_Active = CInt(IDR.GetValue(9).ToString.Trim)
            GunID1 = CInt(IDR.GetValue(10).ToString.Trim)
            GunID2 = CInt(IDR.GetValue(11).ToString.Trim)
            GunID3 = CInt(IDR.GetValue(12).ToString.Trim)
            Dimension = CInt(IDR.GetValue(13).ToString.Trim)
            Layer = IDR.GetValue(14).ToString.Trim
            If IDR.GetValue(15).ToString.Trim <> "" Then
                LineColor = CInt(IDR.GetValue(15).ToString.Trim)
            End If
            If IDR.GetValue(16).ToString.Trim <> "" Then
                LineType = CInt(IDR.GetValue(16).ToString.Trim)
            End If
            KiteiVal = CDbl(IDR.GetValue(17).ToString.Trim)
            SunpoVal = CDbl(IDR.GetValue(18).ToString.Trim)
            KiteiMin = CDbl(IDR.GetValue(19).ToString.Trim)
            KiteiMax = CDbl(IDR.GetValue(20).ToString.Trim)
            flg_gouhi = CInt(IDR.GetValue(21).ToString.Trim)
            Targettype = CInt(IDR.GetValue(22).ToString.Trim)
            flgOutZu = CInt(IDR.GetValue(23).ToString.Trim)
            flgKeisan = CInt(IDR.GetValue(24).ToString.Trim)
            flgScale = CInt(IDR.GetValue(25).ToString.Trim)
        End Sub
    End Class

    'TengunTeigiデータ格納
    Public Class TengunTeigiBunrui
        Public TengunID As Integer
        Public Syori As Integer
        Public Bunrui1 As Integer
        Public Bunrui2 As Integer
        Public flgOnlyOne As Boolean
        Public OpeFlg As Integer
        Public SunpoID As Integer

        Public Sub New()
            TengunID = -1
            Syori = -1
            Bunrui1 = -1
            Bunrui2 = -1
            flgOnlyOne = False
            OpeFlg = 0
            SunpoID = -1
        End Sub

        Public Sub GetDataByReader(ByVal IDR As IDataReader)
            TengunID = CInt(IDR.GetValue(0).ToString.Trim)
            Syori = CInt(IDR.GetValue(1).ToString.Trim)
            If IDR.GetValue(2).ToString.Trim = "" Then
                Bunrui1 = -1
            Else
                Bunrui1 = CInt(IDR.GetValue(2).ToString.Trim)
            End If
            If IDR.GetValue(3).ToString.Trim = "" Then
                Bunrui2 = -1
            Else
                Bunrui2 = CInt(IDR.GetValue(3).ToString.Trim)
            End If
            If IDR.GetValue(4).ToString.Trim = "TRUE" Then
                flgOnlyOne = True
            Else
                flgOnlyOne = False
            End If
        End Sub
    End Class

    'CT_CoordSettingデータ格納
    Public Class ZahyouDBGet
        Public XYorXZorYZ As Integer
        Public CT_GenID As Integer
        Public CT_XID As Integer
        Public CT_YID As Integer
        Public CT_ZID As Integer
        Public CT_ID1 As Integer
        Public CT_ID2 As Integer
        Public CT_ID3 As Integer
        Public Active As Integer
        Public OpeFlg As Integer

        Public Sub New()
            XYorXZorYZ = -1
            CT_GenID = -1
            CT_XID = -1
            CT_YID = -1
            CT_ZID = -1
            CT_ID1 = -1
            CT_ID2 = -1
            CT_ID3 = -1
            Active = -1
            OpeFlg = 0
        End Sub

        Public Sub GetDataByReader(ByVal IDR As IDataReader)
            XYorXZorYZ = CInt(IDR.GetValue(0).ToString.Trim)
            CT_GenID = CInt(IDR.GetValue(1).ToString.Trim)
            CT_XID = CInt(IDR.GetValue(2).ToString.Trim)
            CT_YID = CInt(IDR.GetValue(3).ToString.Trim)
            CT_ZID = CInt(IDR.GetValue(4).ToString.Trim)
            CT_ID1 = CInt(IDR.GetValue(5).ToString.Trim)
            CT_ID2 = CInt(IDR.GetValue(6).ToString.Trim)
            CT_ID3 = CInt(IDR.GetValue(7).ToString.Trim)
            Active = CInt(IDR.GetValue(8).ToString.Trim)
        End Sub
    End Class

    '座標系高さ補正値データ格納
    Public Class Zahyouheight
        Public Zahyou_Height As Double
        Public SunpoID As Integer
        Public OpeFlg As Integer

        Public Sub New()
            Zahyou_Height = 0.0
            SunpoID = -1
            OpeFlg = 0
        End Sub
    End Class

    'データベース要素数
    Public Shared m_Targetcount As Integer                              'CT_Bunrui要素数
    Public Shared m_TengunCount As Integer                              'TengunTeigi要素数
    Public Shared m_SunpoCount As Integer                               'SunpoSet要素数
    'グリッドデータ要素数
    Public m_Scalecount As Integer                                      'スケールグリッド要素数
    Public m_LIneLengthcount As Integer                                 '２点線分グリッド要素数
    Public m_PolyLinecount As Integer                                   'ポリライングリッド要素数
    Public m_Circlecount As Integer                                     '１点円グリッド要素数
    'データベース要素配列
    Public Shared arrCToffsetData(500) As CTBunruiDB                    'CT_Bunrui
    Public Shared arrZahyouData As ZahyouDBGet                          'CT_CoordSetting
    Public Shared arrTengunTeigiData(500) As TengunTeigiBunrui          'TengunTeigi
    Public Shared arrSunpoSetData(500) As SunpoSetGunID                 'SunpoSet
    Public Shared arrSunpoSetL As List(Of SunpoSetTable)                'SunpoSet(ALL DATA)
    'グリッドデータ要素配列
    Public Shared arrScaleData(50) As ScaleItem                         'スケールグリッド
    Public Shared arrLineLegthData(100) As LineLengthItem               '２点線分グリッド
    Public Shared arrPolyLineData(100) As PolyLineItem                  'ポリライングリッド
    Public Shared arrCircleData(100) As CircleItem                      '１点円グリッド
    Public arrZahyouGet(10) As ZahyouItem                               '座標系グリッド
    'ターゲット番号格納用
    Public Shared StrCTNO(100) As Integer                               '開始ターゲット
    Public Shared EndCTNO(100) As Integer                               '終了ターゲット
    Public Shared PolyLineCTNO(100) As String                           'ポリラインターゲット文字列
    Public Shared CircleCTNO(100) As String                             '１点円ターゲット文字列
    '直前のカレントアイテム、カレントカラム、削除用アイテム保存用
    Public beforeTargetItem As New TargetSetItem                        'ターゲットグリッド用
    Public beforeTargetCulumn As Integer
    Public delTargetItem As New TargetSetItem
    Public delTargetCulumn As Integer
    Public beforeScaleItem As New ScaleItem                             'スケールグリッド用
    Public beforeScaleCulumn As Integer
    Public delScaleItem As New ScaleItem
    Public delScaleCulumn As Integer
    Public beforeZahyouItem As New ZahyouItem                           '座標系グリッド用
    Public beforeZahyouCulumn As Integer
    Public delZahyouItem As New ZahyouItem
    Public delZahyouCulumn As Integer
    Public beforeLineItem As New LineLengthItem                         '２点線分グリッド用
    Public beforeLineCulumn As Integer
    Public delLineItem As New LineLengthItem
    Public delLineCulumn As Integer
    Public beforePolyLineItem As New PolyLineItem                       'ポリライングリッド用
    Public beforePolyLineCulumn As Integer
    Public delPolyLineItem As New PolyLineItem
    Public delPolyLineCulumn As Integer
    Public beforeCircleItem As New CircleItem                           '１点円グリッド用
    Public beforeCircleCulumn As Integer
    Public delCircleItem As New CircleItem
    Public delCircleCulumn As Integer
    Public edtZahyouCTNo As Integer                                     ' 座標系エディット行の変更前ターゲット番号（エラー時に戻すため）
    Public edtZahyouCulumn As Integer                                   ' 座標系エディット行のカラム番号
    Public edtScaleSTNo As Integer
    Public edtScaleEDNo As Integer
    Public edtScaleLng As Double
    '最大値の設定用
    Public maxCTno As Integer                                           'ターゲット番号最大数
    Public Shared m_maxtargetID As Integer                              'ターゲットID最大値
    Public Shared MaxSunpoID As Integer                                 '寸法ID最大値
    'データベース書込み用フィールド名、フィールド値設定用
    Public strFieldNames() As String                                    'フィールド名
    Public strFieldTexts() As String                                    'フィールド値
    '新規・開くの種別
    Public IsNew As Boolean = True                                      '新規=True  開く=False
    Public Shared IsNew_Work As Boolean
    Public Shared Zahyou_HData As Zahyouheight                          '座標系の高さ補正値

    '初期ロード
    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

        IsNew_Work = Me.IsNew

        'データベース読み込み
        DB_Read()

        '画面構築
        Tab_Set(m_Targetcount)

        Grid_Refresh()

        Me.Show()
    End Sub

    'データベースより必要なテーブルを読み込む
    Private Sub DB_Read()
        '既に登録されているターゲットを取得する
        m_Targetcount = 0
        Dim IDX As Integer = Read_CTBunruiDB()

        'TengunTeigiデータをDBより読み込む
        m_TengunCount = 0
        Dim IDX2 As Integer = Read_TengunTeigiDB()

        'SunpoSetデータをDBより読み込む
        m_SunpoCount = 0
        Dim IDX3 As Integer = Read_SunpoSetDB()

        'CT_CoordSettingデータをDBより読み込む
        Dim IDX4 As Integer = Read_CTCoordSetDB()

    End Sub

    '各タブの設定
    Private Sub Tab_Set(ByVal IDX As Integer)

        '「ターゲット」タブの設定
        TabTarget_Set(IDX)

        '「スケール・座標系」タブの設定
        TabScaleZhyou_Set(IDX)

        '「２点による線分・長さ」タブの設定
        TabLineLength_Set(IDX)

        '「ポリライン」タブの設定
        TabPolyLine_Set(IDX)

        '「１点による円」タブの設定
        TabCircle_Set(IDX)

    End Sub

    '各DataGridのアイテムの再表示
    Private Sub Grid_Refresh()
        Dim strMsg As String

        Grid_Target.CommitEdit()
        Grid_Target.Items.Refresh()
        Try
            Grid_Kijun.CommitEdit()
            Grid_Kijun.Items.Refresh()

            Grid_zahyou.CommitEdit()
            Grid_zahyou.Items.Refresh()

            Grid_Line.CommitEdit()
            Grid_Line.Items.Refresh()
        Catch ex As Exception
            strMsg = "ターゲット番号が登録されていません。正しいターゲット番号を入力してください。"
            MsgBox(strMsg, MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "ターゲット入力エラー")
        End Try

        Try
            Grid_PolyLine.CommitEdit()
            Grid_PolyLine.Items.Refresh()

            Grid_Circle.CommitEdit()
            Grid_Circle.Items.Refresh()
        Catch ex As Exception
            strMsg = "ターゲット番号が不正です。正しいターゲット番号を入力してください。"
            MsgBox(strMsg, MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "ターゲット入力エラー")
        End Try

    End Sub

    '計測システムDBよりコードターゲットの一覧を読み込む（CT_Bunruiテーブル）
    Private Shared Function Read_CTBunruiDB(Optional ByRef m_dbclass As CDBOperateOLE = Nothing) As Integer
        Dim IDX As Integer = 0

        If m_Targetcount > 0 Then
            Return m_Targetcount
        End If

        For Each CTOS2 As CTBunruiDB In arrCToffsetData
            CTOS2 = New CTBunruiDB
        Next

        'システム設定DBへ接続
        Dim DBPath As String = My.Application.Info.DirectoryPath & "\計測システムフォルダ\" & strSystemMdbFile
        If m_dbclass Is Nothing Then
            If ConnectDB(DBPath, m_dbclass) = False Then
                MsgBox(strSystemMdbFile & "への接続に失敗しました。")
                Return (IDX)
                Exit Function
            End If
        End If

        Dim strSql As String = "SELECT CT_NO,flg_Use,m_x,m_y,m_z,Kubun1,Kubun2,Info FROM CT_Bunrui WHERE TypeID = " & _
                                CommonTypeID & " ORDER BY CT_NO"
        Dim IDR As IDataReader
        IDR = m_dbclass.DoSelect(strSql)
        If Not IDR Is Nothing Then
            Do While IDR.Read
                Dim CToffsetD As New CToffset
                CToffsetD.GetDataByReader(IDR)
                arrCToffsetData(IDX) = New CTBunruiDB
                arrCToffsetData(IDX).CT_NO = CToffsetD.CT_ID
                arrCToffsetData(IDX).flg_Use = CToffsetD.flg_Use
                arrCToffsetData(IDX).m_x = CToffsetD.offset_val.x
                arrCToffsetData(IDX).m_y = CToffsetD.offset_val.y
                arrCToffsetData(IDX).m_z = CToffsetD.offset_val.z
                arrCToffsetData(IDX).kubun1 = CToffsetD.kubun1
                arrCToffsetData(IDX).kubun2 = CToffsetD.kubun2
                arrCToffsetData(IDX).info = CToffsetD.info
                arrCToffsetData(IDX).OpeFlg = 0
                arrCToffsetData(IDX).ID = IDX
                arrCToffsetData(IDX).UseFlg = 0
                IDX += 1
            Loop
            IDR.Close()
        End If
        m_maxtargetID = IDX - 1
        m_Targetcount = IDX

        'システム設定DBへ切断
        DisConnectDB(m_dbclass)
        Return (IDX)

    End Function

    '計測システムDBより点群定義の一覧を読み込む（TengunTeigiテーブル）
    Private Shared Function Read_TengunTeigiDB(Optional ByRef m_dbclass As CDBOperateOLE = Nothing) As Integer
        Dim IDX As Integer = 0

        If m_TengunCount > 0 Then
            Return m_TengunCount
        End If

        For Each CTOS2 As TengunTeigiBunrui In arrTengunTeigiData
            CTOS2 = New TengunTeigiBunrui
        Next

        'システム設定DBへ接続
        Dim DBPath As String = My.Application.Info.DirectoryPath & "\計測システムフォルダ\" & strSystemMdbFile
        If m_dbclass Is Nothing Then
            If ConnectDB(DBPath, m_dbclass) = False Then
                MsgBox(strSystemMdbFile & "への接続に失敗しました。")
                Return (IDX)
                Exit Function
            End If
        End If

        Dim strSql As String = "SELECT 点群ID,処理方法ID,分類1,分類2,flgOnlyOne FROM TenGunTeigi WHERE タイプID = " & CommonTypeID
        Dim IDR As IDataReader
        IDR = m_dbclass.DoSelect(strSql)
        If Not IDR Is Nothing Then
            If Not IDR Is Nothing Then
                IDX = 0
                Do While IDR.Read
                    Dim TengunD As New TengunTeigiBunrui
                    TengunD.GetDataByReader(IDR)
                    arrTengunTeigiData(IDX) = New TengunTeigiBunrui
                    arrTengunTeigiData(IDX).TengunID = TengunD.TengunID
                    arrTengunTeigiData(IDX).Syori = TengunD.Syori
                    arrTengunTeigiData(IDX).Bunrui1 = TengunD.Bunrui1
                    arrTengunTeigiData(IDX).Bunrui2 = TengunD.Bunrui2
                    arrTengunTeigiData(IDX).flgOnlyOne = TengunD.flgOnlyOne
                    arrTengunTeigiData(IDX).OpeFlg = 0
                    IDX += 1
                Loop
                IDR.Close()
            End If
        End If
        m_TengunCount = IDX

        'システム設定DBへ切断
        DisConnectDB(m_dbclass)
        Return (IDX)

    End Function

    '計測システムDBより寸法定義の一覧を読み込む（SunpoSetテーブル）
    Private Shared Function Read_SunpoSetDB(Optional ByRef m_dbclass As CDBOperateOLE = Nothing) As Integer
        Dim IDX As Integer = 0
        Dim ii As Integer
        MaxSunpoID = 0

        If m_SunpoCount > 0 Then
            Return m_SunpoCount
        End If

        'システム設定DBへ接続
        Dim DBPath As String
        'If IsNew_Work = True Then
        DBPath = My.Application.Info.DirectoryPath & "\計測システムフォルダ\" & strSystemMdbFile
        'Else
        'DBPath = m_strDataBasePath & "\計測データ.mdb"
        'End If
        If m_dbclass Is Nothing Then
            If ConnectDB(DBPath, m_dbclass) = False Then
                MsgBox(strSystemMdbFile & "への接続に失敗しました。")
                Return (IDX)
                Exit Function
            End If
        End If

        Dim SSD As New SunpoSetTable
        SSD.m_dbClass = m_dbclass
        arrSunpoSetL = SSD.GetDataToList()
        If arrSunpoSetL Is Nothing Then
            Exit Function
        End If

        Zahyou_HData = New Zahyouheight
        For ii = 0 To arrSunpoSetL.Count - 1
            arrSunpoSetData(IDX) = New SunpoSetGunID
            arrSunpoSetData(IDX).ID = CInt(arrSunpoSetL(ii).ID)
            arrSunpoSetData(IDX).SunpoID = CInt(arrSunpoSetL(ii).SunpoID)
            arrSunpoSetData(IDX).SunpoMark = arrSunpoSetL(ii).SunpoMark
            If Trim(arrSunpoSetL(ii).SunpoMark) = "Leveladj" Then
                Zahyou_HData.Zahyou_Height = arrSunpoSetL(ii).KiteiVal
                Zahyou_HData.SunpoID = arrSunpoSetL(ii).SunpoID
                Zahyou_HData.OpeFlg = 0
            End If
            arrSunpoSetData(IDX).SunpoName = arrSunpoSetL(ii).SunpoName
            arrSunpoSetData(IDX).SunpoCalcHouhou = CInt(arrSunpoSetL(ii).SunpoCalcHouhou)
            arrSunpoSetData(IDX).CT_ID1 = CInt(arrSunpoSetL(ii).CT_ID1)
            arrSunpoSetData(IDX).CT_ID2 = CInt(arrSunpoSetL(ii).CT_ID2)
            arrSunpoSetData(IDX).CT_ID3 = CInt(arrSunpoSetL(ii).CT_ID3)
            arrSunpoSetData(IDX).GunID1 = CInt(arrSunpoSetL(ii).GunID1)
            arrSunpoSetData(IDX).GunID2 = CInt(arrSunpoSetL(ii).GunID2)
            arrSunpoSetData(IDX).GunID3 = CInt(arrSunpoSetL(ii).GunID3)
            arrSunpoSetData(IDX).Dimension = CInt(arrSunpoSetL(ii).seibunXYZ)
            arrSunpoSetData(IDX).Layer = arrSunpoSetL(ii).ZU_layer
            If arrSunpoSetL(ii).ZU_colorID.ToString.Trim = "" Then
                arrSunpoSetData(IDX).LineColor = 6
            Else
                arrSunpoSetData(IDX).LineColor = CInt(arrSunpoSetL(ii).ZU_colorID)
            End If
            If arrSunpoSetL(ii).ZU_LineTypeID.ToString.Trim = "" Then
                arrSunpoSetData(IDX).LineType = 1
            Else
                arrSunpoSetData(IDX).LineType = CInt(arrSunpoSetL(ii).ZU_LineTypeID)
            End If
            arrSunpoSetData(IDX).KiteiVal = CDbl(arrSunpoSetL(ii).KiteiVal)
            arrSunpoSetData(IDX).SunpoVal = CDbl(arrSunpoSetL(ii).SunpoVal)
            arrSunpoSetData(IDX).KiteiMin = CDbl(arrSunpoSetL(ii).KiteiMin)
            arrSunpoSetData(IDX).KiteiMax = CDbl(arrSunpoSetL(ii).KiteiMax)
            arrSunpoSetData(IDX).flg_gouhi = CInt(arrSunpoSetL(ii).flg_gouhi)
            If arrSunpoSetL(ii).Targettype.ToString.Trim = "" Then
                arrSunpoSetData(IDX).Targettype = 0
            Else
                arrSunpoSetData(IDX).Targettype = CInt(arrSunpoSetL(ii).Targettype)
            End If
            If arrSunpoSetL(ii).flgOutZu.ToString.Trim = "" Then
                arrSunpoSetData(IDX).flgOutZu = 0
            Else
                arrSunpoSetData(IDX).flgOutZu = CInt(arrSunpoSetL(ii).flgOutZu)
            End If
            arrSunpoSetData(IDX).flgKeisan = CInt(arrSunpoSetL(ii).flgKeisan)
            arrSunpoSetData(IDX).flgScale = CInt(arrSunpoSetL(ii).flgScale)
            arrSunpoSetData(IDX).OpeFlg = 0
            IDX += 1
            If MaxSunpoID < CInt(arrSunpoSetL(ii).SunpoID) Then
                MaxSunpoID = CInt(arrSunpoSetL(ii).SunpoID)
            End If
        Next ii

        m_SunpoCount = arrSunpoSetL.Count

        'システム設定DBへ切断
        DisConnectDB(m_dbclass)
        Return (IDX)

    End Function

    '計測システムDBより座標定義の一覧を読み込む（CT_CoordSettingテーブル）
    Private Shared Function Read_CTCoordSetDB(Optional ByRef m_dbclass As CDBOperateOLE = Nothing) As Integer
        Dim IDX As Integer = 0

        'システム設定DBへ接続
        Dim DBPath As String = My.Application.Info.DirectoryPath & "\計測システムフォルダ\" & strSystemMdbFile
        If m_dbclass Is Nothing Then
            If ConnectDB(DBPath, m_dbclass) = False Then
                MsgBox(strSystemMdbFile & "への接続に失敗しました。")
                Return (IDX)
                Exit Function
            End If
        End If

        '寸法セットテーブルより、タイプIDをキーにして、寸法マーク、CT_ID1、CT_ID2を取得する
        Dim strSql As String = "SELECT XYorXZorYZ,CT_GenID,CT_XID,CT_YID,CT_ZID,CT_ID1,CT_ID2,CT_ID3,CT_Active FROM CT_CoordSetting WHERE TypeID = " & CommonTypeID
        Dim IDR As IDataReader
        IDR = m_dbclass.DoSelect(strSql)
        If Not IDR Is Nothing Then
            Do While IDR.Read
                Dim ZahyouGetD As New ZahyouDBGet
                ZahyouGetD.GetDataByReader(IDR)
                arrZahyouData = New ZahyouDBGet
                arrZahyouData.XYorXZorYZ = ZahyouGetD.XYorXZorYZ
                arrZahyouData.CT_GenID = ZahyouGetD.CT_GenID
                arrZahyouData.CT_XID = ZahyouGetD.CT_XID
                arrZahyouData.CT_YID = ZahyouGetD.CT_YID
                arrZahyouData.CT_ZID = ZahyouGetD.CT_ZID
                arrZahyouData.CT_ID1 = ZahyouGetD.CT_ID1
                arrZahyouData.CT_ID2 = ZahyouGetD.CT_ID2
                arrZahyouData.CT_ID3 = ZahyouGetD.CT_ID3
                arrZahyouData.Active = ZahyouGetD.Active
                arrZahyouData.OpeFlg = 0
                IDX += 1
            Loop
            IDR.Close()
        End If

        'システム設定DBへ切断
        DisConnectDB(m_dbclass)

        Return (IDX)
    End Function

    '２点線分・長さの要素一覧を設定する
    Private Shared Function Set_LineLengthData(Optional ByRef m_dbclass As CDBOperateOLE = Nothing) As Integer

        Dim IDX As Integer = 0
        Dim IDX2 As Integer = 0
        Dim IDX3 As Integer = 0
        Dim IDX4 As Integer = 0
        Dim ii, jj, kk As Integer
        Dim arrTengunTeigi(100) As TengunTeigiBunrui
        Dim iStrFlg As Integer = 0
        Dim iEndFlg As Integer = 0

        For Each CTOS3 As LineLengthItem In arrLineLegthData
            CTOS3 = New LineLengthItem
        Next

        'SunpoSet、TengunTeigi、CT_Bunruiのそれぞれのデータが読み込まれていない時は終了する
        If (m_SunpoCount <= 0) And (m_TengunCount <= 0) And (m_Targetcount <= 0) Then
            Exit Function
        End If

        '読み込んだSunpoSetデータより、SunpoCalsHouhou=1、8のデータを取得する
        For ii = 0 To m_SunpoCount - 1
            If (arrSunpoSetData(ii).SunpoCalcHouhou = 1) Or (arrSunpoSetData(ii).SunpoCalcHouhou = 8) Then
                'If (arrSunpoSetData(ii).SunpoMark = "S-A") Or (arrSunpoSetData(ii).SunpoMark = "S-B") Or _
                '   (arrSunpoSetData(ii).SunpoMark = "S12") Or (arrSunpoSetData(ii).SunpoMark = "S34") Or (arrSunpoSetData(ii).SunpoMark = "S56") Then
                If (arrSunpoSetData(ii).flgScale = 1) Or (arrSunpoSetData(ii).flgScale = 1) Then
                    Continue For
                Else
                    If arrSunpoSetData(ii).OpeFlg <> 2 Then
                        arrLineLegthData(IDX) = New LineLengthItem
                        arrLineLegthData(IDX).Layer = arrSunpoSetData(ii).Layer
                        arrLineLegthData(IDX).Dimension = arrSunpoSetData(ii).Dimension
                        arrLineLegthData(IDX).LineColor = arrSunpoSetData(ii).LineColor
                        arrLineLegthData(IDX).LineType = arrSunpoSetData(ii).LineType
                        arrLineLegthData(IDX).SunpoMark = arrSunpoSetData(ii).SunpoMark
                        arrLineLegthData(IDX).SunpoName = arrSunpoSetData(ii).SunpoName
                        arrLineLegthData(IDX).flgScale = arrSunpoSetData(ii).flgScale
                        arrLineLegthData(IDX).StartTarget = "0"
                        arrLineLegthData(IDX).EndTarget = "0"
                        arrLineLegthData(IDX).OpeFlg = 0
                        arrLineLegthData(IDX).SunpoID = arrSunpoSetData(ii).SunpoID
                        arrLineLegthData(IDX).TengunID1 = arrSunpoSetData(ii).GunID1
                        arrLineLegthData(IDX).TengunID2 = arrSunpoSetData(ii).GunID2
                        IDX += 1
                    End If
                End If
            End If
        Next ii

        '読み込んだTengunTeigiデータより、対応するコードターゲット番号を取得する
        If IDX > 0 Then
            For ii = 0 To IDX - 1
                '始点のターゲット取得
                Dim kbn1 As Integer = -1
                Dim kbn2 As Integer = -1
                Dim sCnt As Integer = -1
                For jj = 0 To m_TengunCount - 1
                    If arrTengunTeigiData(jj).TengunID = arrLineLegthData(ii).TengunID1 Then
                        kbn1 = arrTengunTeigiData(jj).Bunrui1
                        kbn2 = arrTengunTeigiData(jj).Bunrui2
                        arrTengunTeigiData(jj).SunpoID = arrLineLegthData(ii).SunpoID
                        sCnt = jj
                        Exit For
                    End If
                Next jj
                If (kbn1 >= 0) And (kbn2 >= 0) Then
                    For kk = 0 To m_Targetcount - 1
                        If (arrCToffsetData(kk).kubun1 = kbn1) And (arrCToffsetData(kk).kubun2 = kbn2) Then
                            arrLineLegthData(ii).StartTarget = arrCToffsetData(kk).CT_NO.ToString
                            arrCToffsetData(kk).UseFlg = 1
                            arrCToffsetData(kk).TengunID = sCnt
                            Exit For
                        End If
                    Next kk
                End If

                '終点のターゲット取得
                kbn1 = -1
                kbn2 = -1
                sCnt = -1
                For jj = 0 To m_TengunCount - 1
                    If arrTengunTeigiData(jj).TengunID = arrLineLegthData(ii).TengunID2 Then
                        kbn1 = arrTengunTeigiData(jj).Bunrui1
                        kbn2 = arrTengunTeigiData(jj).Bunrui2
                        arrTengunTeigiData(jj).SunpoID = arrLineLegthData(ii).SunpoID
                        sCnt = jj
                        Exit For
                    End If
                Next jj
                If (kbn1 >= 0) And (kbn2 >= 0) Then
                    For kk = 0 To m_Targetcount - 1
                        If (arrCToffsetData(kk).kubun1 = kbn1) And (arrCToffsetData(kk).kubun2 = kbn2) Then
                            arrLineLegthData(ii).EndTarget = arrCToffsetData(kk).CT_NO.ToString
                            arrCToffsetData(kk).UseFlg = 1
                            arrCToffsetData(kk).TengunID = sCnt
                            Exit For
                        End If
                    Next kk
                End If
            Next ii
        End If

        Return (IDX)

    End Function

    'ポリラインの要素一覧を設定する
    Private Shared Function Set_PolyLineData(Optional ByRef m_dbclass As CDBOperateOLE = Nothing) As Integer

        Dim IDX As Integer = 0
        Dim IDX4 As Integer = 0
        Dim ii, iii As Integer
        Dim arrintPlineCTNO(500) As Integer
        Dim saveCTNO As Integer
        Dim haiflg As Integer

        'SunpoSet、TengunTeigi、CT_Bunruiのそれぞれのデータが読み込まれていない時は終了する
        If (m_SunpoCount <= 0) And (m_TengunCount <= 0) And (m_Targetcount <= 0) Then
            Exit Function
        End If

        '読み込んだSunpoSetデータより、SunpoCalsHouhou=11のデータを取得する
        For ii = 0 To m_SunpoCount - 1
            If arrSunpoSetData(ii).SunpoCalcHouhou = 11 Then
                If arrSunpoSetData(ii).OpeFlg <> 2 Then
                    arrPolyLineData(IDX) = New PolyLineItem
                    arrPolyLineData(IDX).Layer = arrSunpoSetData(ii).Layer
                    arrPolyLineData(IDX).Dimension = arrSunpoSetData(ii).Dimension
                    arrPolyLineData(IDX).LineColor = arrSunpoSetData(ii).LineColor
                    arrPolyLineData(IDX).LineType = arrSunpoSetData(ii).LineType
                    arrPolyLineData(IDX).SunpoMark = arrSunpoSetData(ii).SunpoMark
                    arrPolyLineData(IDX).SunpoName = arrSunpoSetData(ii).SunpoName
                    arrPolyLineData(IDX).VertexTarget = ""
                    arrPolyLineData(IDX).OpeFlg = 0
                    arrPolyLineData(IDX).SunpoID = arrSunpoSetData(ii).SunpoID
                    arrPolyLineData(IDX).TengunID = arrSunpoSetData(ii).GunID1
                    IDX += 1
                End If
            End If
        Next ii

        '点群IDの対応するコードターゲット番号の取得
        If IDX > 0 Then
            For ii = 0 To IDX - 1
                '読み込んだ点群定義テーブルより、点群IDをキーとして、分類IDを取得する
                IDX4 = Get_TargetArray(arrPolyLineData(ii).TengunID, arrintPlineCTNO)

                'コードターゲット文字列の作成
                Dim CTNO_Str As String = ""
                For iii = 0 To IDX4 - 1
                    If iii = 0 Then
                        CTNO_Str = arrintPlineCTNO(iii).ToString
                        saveCTNO = arrintPlineCTNO(iii)
                        haiflg = 0
                    Else
                        If arrintPlineCTNO(iii) = saveCTNO + 1 Then
                            If haiflg = 0 Then
                                If iii = IDX4 - 1 Then
                                    CTNO_Str = CTNO_Str & "," & arrintPlineCTNO(iii).ToString
                                    saveCTNO = arrintPlineCTNO(iii)
                                Else
                                    CTNO_Str = CTNO_Str & "-"
                                    saveCTNO = arrintPlineCTNO(iii)
                                    haiflg = 1
                                End If
                            Else
                                If iii = IDX4 - 1 Then
                                    CTNO_Str = CTNO_Str & arrintPlineCTNO(iii).ToString
                                End If
                                saveCTNO = arrintPlineCTNO(iii)
                            End If
                        Else
                            If haiflg = 0 Then
                                CTNO_Str = CTNO_Str & "," & arrintPlineCTNO(iii).ToString
                                saveCTNO = arrintPlineCTNO(iii)
                            Else
                                CTNO_Str = CTNO_Str & saveCTNO.ToString & "," & arrintPlineCTNO(iii).ToString
                                saveCTNO = arrintPlineCTNO(iii)
                                haiflg = 0
                            End If
                        End If
                    End If
                Next iii
                arrPolyLineData(ii).VertexTarget = CTNO_Str
            Next ii
        End If

        Return (IDX)

    End Function

    '一点による円の要素一覧を設定する
    Private Shared Function Set_CircleData(Optional ByRef m_dbclass As CDBOperateOLE = Nothing) As Integer

        Dim IDX As Integer = 0
        Dim IDX4 As Integer = 0
        Dim ii, iii As Integer
        Dim arrintCircleCTNO(500) As Integer
        Dim saveCTNO As Integer
        Dim haiflg As Integer

        'SunpoSet、TengunTeigi、CT_Bunruiのそれぞれのデータが読み込まれていない時は終了する
        If (m_SunpoCount <= 0) And (m_TengunCount <= 0) And (m_Targetcount <= 0) Then
            Exit Function
        End If

        '読み込んだSunpoSetデータより、SunpoCalsHouhou=11のデータを取得する
        For ii = 0 To m_SunpoCount - 1
            If arrSunpoSetData(ii).SunpoCalcHouhou = 14 Then
                If arrSunpoSetData(ii).OpeFlg <> 2 Then
                    arrCircleData(IDX) = New CircleItem
                    arrCircleData(IDX).Layer = arrSunpoSetData(ii).Layer
                    arrCircleData(IDX).Dimension = arrSunpoSetData(ii).Dimension
                    arrCircleData(IDX).LineColor = arrSunpoSetData(ii).LineColor
                    arrCircleData(IDX).LineType = arrSunpoSetData(ii).LineType
                    arrCircleData(IDX).SunpoMark = arrSunpoSetData(ii).SunpoMark
                    arrCircleData(IDX).SunpoName = arrSunpoSetData(ii).SunpoName
                    arrCircleData(IDX).CenterTarget = ""
                    arrCircleData(IDX).Diameter = arrSunpoSetData(ii).KiteiVal.ToString
                    arrCircleData(IDX).OpeFlg = 0
                    arrCircleData(IDX).SunpoID = arrSunpoSetData(ii).SunpoID
                    arrCircleData(IDX).TengunID = arrSunpoSetData(ii).GunID1
                    IDX += 1
                End If
            End If
        Next ii

        '点群IDの対応するコードターゲット番号の取得
        If IDX > 0 Then
            For ii = 0 To IDX - 1
                '読み込んだ点群定義テーブルより、点群IDをキーとして、分類IDを取得する
                IDX4 = Get_TargetArray(arrCircleData(ii).TengunID, arrintCircleCTNO)

                'コードターゲット文字列の作成
                Dim CTNO_Str As String = ""
                For iii = 0 To IDX4 - 1
                    If iii = 0 Then
                        CTNO_Str = arrintCircleCTNO(iii).ToString
                        saveCTNO = arrintCircleCTNO(iii)
                        haiflg = 0
                    Else
                        If arrintCircleCTNO(iii) = saveCTNO + 1 Then
                            If haiflg = 0 Then
                                If iii = IDX4 - 1 Then
                                    CTNO_Str = CTNO_Str & "," & arrintCircleCTNO(iii).ToString
                                    saveCTNO = arrintCircleCTNO(iii)
                                Else
                                    CTNO_Str = CTNO_Str & "-"
                                    saveCTNO = arrintCircleCTNO(iii)
                                    haiflg = 1
                                End If
                            Else
                                If iii = IDX4 - 1 Then
                                    CTNO_Str = CTNO_Str & arrintCircleCTNO(iii).ToString
                                End If
                                saveCTNO = arrintCircleCTNO(iii)
                            End If
                        Else
                            If haiflg = 0 Then
                                CTNO_Str = CTNO_Str & "," & arrintCircleCTNO(iii).ToString
                                saveCTNO = arrintCircleCTNO(iii)
                            Else
                                CTNO_Str = CTNO_Str & saveCTNO.ToString & "," & arrintCircleCTNO(iii).ToString
                                saveCTNO = arrintCircleCTNO(iii)
                                haiflg = 0
                            End If
                        End If
                    End If
                Next iii
                arrCircleData(ii).CenterTarget = CTNO_Str
            Next ii
        End If

        Return (IDX)

    End Function

    '点群IDよりコードターゲット番号列を取得する
    Private Shared Function Get_TargetArray(ByVal TengunN As Integer, ByRef TargetN() As Integer) As Integer
        Dim IDX4 As Integer = 0
        Dim jj, kk As Integer

        For jj = 0 To m_TengunCount - 1
            Dim kbn1 As Integer = -1
            Dim kbn2 As Integer = -1
            Dim sCnt As Integer = -1
            If TengunN = arrTengunTeigiData(jj).TengunID Then
                kbn1 = arrTengunTeigiData(jj).Bunrui1
                kbn2 = arrTengunTeigiData(jj).Bunrui2
                sCnt = jj
                For kk = 0 To m_Targetcount - 1
                    If ((kbn2 = 0) And (arrCToffsetData(kk).kubun1 = kbn1)) Or _
                       ((kbn2 > 0) And (arrCToffsetData(kk).kubun1 = kbn1) And (arrCToffsetData(kk).kubun2 = kbn2)) Then
                        TargetN(IDX4) = arrCToffsetData(kk).CT_NO
                        arrCToffsetData(kk).UseFlg = 1
                        arrCToffsetData(kk).TengunID = sCnt
                        IDX4 += 1
                    End If
                Next kk
            End If
        Next jj

        Return (IDX4)
    End Function

    '計測システムDBより座標系のCT_IDを取得する（SunpoSetテーブル）
    Private Function Set_ZahyouData(Optional ByRef m_dbclass As CDBOperateOLE = Nothing) As Integer
        Dim IDX As Integer = 0
        Dim CTNO As Integer
        Dim CTID As Integer
        Dim ii As Integer

        For ii = 0 To 6
            arrZahyouGet(ii) = New ZahyouItem
            arrZahyouGet(ii).CT_Type = " "
            arrZahyouGet(ii).CT_No = "-1"
            arrZahyouGet(ii).ID = ii
            arrZahyouGet(ii).OpeFlg = 0
        Next ii

        If arrZahyouData IsNot Nothing And arrZahyouData.Active = 1 Then
            'CT_GenIDの取得
            If arrZahyouData.CT_GenID > 0 Then
                CTID = arrZahyouData.CT_GenID
                CTNO = ZahyouCTNO(CTID)
                If CTNO >= 0 Then
                    arrZahyouGet(0).CT_No = CTNO.ToString
                Else
                    arrZahyouGet(0).CT_No = "0"
                End If
            Else
                arrZahyouGet(0).CT_No = "0"
            End If
            'ターゲット使用フラグのセット
            If CInt(arrZahyouGet(0).CT_No) <> -1 Then
                For ii = 0 To m_Targetcount - 1
                    If CInt(arrZahyouGet(0).CT_No) = arrCToffsetData(ii).CT_NO Then
                        arrCToffsetData(ii).UseFlg = 1
                        Exit For
                    End If
                Next ii
            End If

            'CT_XIDの取得
            If arrZahyouData.CT_XID > 0 Then
                CTID = arrZahyouData.CT_XID
                CTNO = ZahyouCTNO(CTID)
                If CTNO >= 0 Then
                    arrZahyouGet(1).CT_No = CTNO.ToString
                Else
                    arrZahyouGet(1).CT_No = "0"
                End If
            Else
                arrZahyouGet(1).CT_No = "0"
            End If
            'ターゲット使用フラグのセット
            If CInt(arrZahyouGet(1).CT_No) <> -1 Then
                For ii = 0 To m_Targetcount - 1
                    If CInt(arrZahyouGet(1).CT_No) = arrCToffsetData(ii).CT_NO Then
                        arrCToffsetData(ii).UseFlg = 1
                        Exit For
                    End If
                Next ii
            End If

            'CT_YIDの取得
            If arrZahyouData.CT_YID > 0 Then
                CTID = arrZahyouData.CT_YID
                CTNO = ZahyouCTNO(CTID)
                If CTNO >= 0 Then
                    arrZahyouGet(2).CT_No = CTNO.ToString
                Else
                    arrZahyouGet(2).CT_No = "0"
                End If
            Else
                arrZahyouGet(2).CT_No = "0"
            End If
            'ターゲット使用フラグのセット
            If CInt(arrZahyouGet(2).CT_No) <> -1 Then
                For ii = 0 To m_Targetcount - 1
                    If CInt(arrZahyouGet(2).CT_No) = arrCToffsetData(ii).CT_NO Then
                        arrCToffsetData(ii).UseFlg = 1
                        Exit For
                    End If
                Next ii
            End If

            'CT_ZIDの取得
            If arrZahyouData.CT_ZID > 0 Then
                CTID = arrZahyouData.CT_ZID
                CTNO = ZahyouCTNO(CTID)
                If CTNO >= 0 Then
                    arrZahyouGet(3).CT_No = CTNO.ToString
                Else
                    arrZahyouGet(3).CT_No = "0"
                End If
            Else
                arrZahyouGet(3).CT_No = "0"
            End If
            'ターゲット使用フラグのセット
            If CInt(arrZahyouGet(3).CT_No) <> -1 Then
                For ii = 0 To m_Targetcount - 1
                    If CInt(arrZahyouGet(3).CT_No) = arrCToffsetData(ii).CT_NO Then
                        arrCToffsetData(ii).UseFlg = 1
                        Exit For
                    End If
                Next ii
            End If

            'CT_ID1の取得
            If arrZahyouData.CT_ID1 > 0 Then
                CTID = arrZahyouData.CT_ID1
                CTNO = ZahyouCTNO(CTID)
                If CTNO >= 0 Then
                    arrZahyouGet(4).CT_No = CTNO.ToString
                Else
                    arrZahyouGet(4).CT_No = "0"
                End If
            Else
                arrZahyouGet(4).CT_No = "0"
            End If
            'ターゲット使用フラグのセット
            If CInt(arrZahyouGet(4).CT_No) <> -1 Then
                For ii = 0 To m_Targetcount - 1
                    If CInt(arrZahyouGet(4).CT_No) = arrCToffsetData(ii).CT_NO Then
                        arrCToffsetData(ii).UseFlg = 1
                        Exit For
                    End If
                Next ii
            End If

            'CT_ID2の取得
            If arrZahyouData.CT_ID2 > 0 Then
                CTID = arrZahyouData.CT_ID2
                CTNO = ZahyouCTNO(CTID)
                If CTNO >= 0 Then
                    arrZahyouGet(5).CT_No = CTNO.ToString
                Else
                    arrZahyouGet(5).CT_No = "0"
                End If
            Else
                arrZahyouGet(5).CT_No = "0"
            End If
            'ターゲット使用フラグのセット
            If CInt(arrZahyouGet(5).CT_No) <> -1 Then
                For ii = 0 To m_Targetcount - 1
                    If CInt(arrZahyouGet(5).CT_No) = arrCToffsetData(ii).CT_NO Then
                        arrCToffsetData(ii).UseFlg = 1
                        Exit For
                    End If
                Next ii
            End If

            'CT_ID3の取得
            If arrZahyouData.CT_ID3 > 0 Then
                CTID = arrZahyouData.CT_ID3
                CTNO = ZahyouCTNO(CTID)
                If CTNO >= 0 Then
                    arrZahyouGet(6).CT_No = CTNO.ToString
                Else
                    arrZahyouGet(6).CT_No = "0"
                End If
            Else
                arrZahyouGet(6).CT_No = "0"
            End If

            'ターゲット使用フラグのセット
            If CInt(arrZahyouGet(6).CT_No) <> -1 Then
                For ii = 0 To m_Targetcount - 1
                    If CInt(arrZahyouGet(6).CT_No) = arrCToffsetData(ii).CT_NO Then
                        arrCToffsetData(ii).UseFlg = 1
                        Exit For
                    End If
                Next ii
            End If
        End If

        Return (1)
    End Function

    '点群IDより、TengunTeigiテーブル、CT_Bunruiテーブルを経て、ターゲット番号を取得する
    Public Function ZahyouCTNO(ByVal CTID As Integer, Optional ByRef m_dbclass As CDBOperateOLE = Nothing) As Integer
        Dim iReturn = -1
        Dim kbn1 As Integer = -1
        Dim kbn2 As Integer = -1

        For ii = 0 To m_TengunCount - 1
            If arrTengunTeigiData(ii).TengunID = CTID Then
                kbn1 = arrTengunTeigiData(ii).Bunrui1
                kbn2 = arrTengunTeigiData(ii).Bunrui2
                Exit For
            End If
        Next ii

        If (kbn1 >= 0) And (kbn2 >= 0) Then
            For ii = 0 To m_Targetcount - 1
                If (arrCToffsetData(ii).kubun1 = kbn1) And (arrCToffsetData(ii).kubun2 = kbn2) Then
                    iReturn = arrCToffsetData(ii).CT_NO
                End If
            Next ii
        End If

        Return (iReturn)
    End Function

    '「ターゲット」タブの値を設定する
    Private Sub TabTarget_Set(ByVal IDX As Integer)
        'ターゲットタブの要素登録
        Dim iRowNo As Integer = 0

        'データ表示
        With Grid_Target
            '並び替え禁止
            For Each T As System.Windows.Controls.DataGridColumn In .Columns
                T.CanUserSort = False
            Next
            .CanUserAddRows = False
            .CanUserDeleteRows = False
            .ItemsSource = Nothing
        End With

        With m_TagetSetItems
            .Clear()

            maxCTno = 0
            For i As Integer = 0 To IDX - 1
                If arrCToffsetData(i).OpeFlg <> 2 Then
                    .Add(New TargetSetItem)
                    Try
                        .Item(iRowNo).TargetNumber = arrCToffsetData(i).CT_NO.ToString
                        .Item(iRowNo).TargetOffsetX = arrCToffsetData(i).m_x.ToString
                        .Item(iRowNo).TargetOffsetY = arrCToffsetData(i).m_y.ToString
                        .Item(iRowNo).TargetOffsetZ = arrCToffsetData(i).m_z.ToString
                        .Item(iRowNo).TargetInfo = arrCToffsetData(i).info
                        .Item(iRowNo).OpeFlg = 0
                        .Item(iRowNo).UseFlg = 0
                        .Item(iRowNo).ID = arrCToffsetData(i).ID
                        If arrCToffsetData(i).CT_NO > maxCTno Then
                            maxCTno = arrCToffsetData(i).CT_NO
                        End If
                        iRowNo += 1
                    Catch ex As Exception
                        MsgBox("値設定エラー", MsgBoxStyle.OkOnly)
                        Exit Sub
                    End Try
                End If
            Next
        End With
        Grid_Target.ItemsSource = m_TagetSetItems
        Grid_Target.CurrentCell = Nothing
        Grid_Target.Items.Refresh()
    End Sub

    '「スケール・座標系」タブの値を設定する
    Private Sub TabScaleZhyou_Set(ByVal IDX As Integer)
        Dim IDX2 As Integer = 0
        Dim IDX3 As Integer = 0
        Dim CtNo(2) As Integer
        Dim CTID As Integer
        Dim ii, jj As Integer
        Dim iFlg As Integer

        'スケールテーブルの設定
        With Grid_Kijun
            .CanUserAddRows = False
            .CanUserDeleteRows = False
            .ItemsSource = Nothing
        End With
        Label_note.Content = "基準スケールにチェックが入っていないスケールは" & vbCrLf & "精度確認用のスケールとして扱います。"

        With m_ScaleItems
            .Clear()

            For ii = 0 To m_SunpoCount - 1
                ' If (arrSunpoSetData(ii).SunpoMark = "S-A") Or (arrSunpoSetData(ii).SunpoMark = "S-B") Or (arrSunpoSetData(ii).SunpoMark = "S-C") Or (arrSunpoSetData(ii).SunpoMark = "S-D") Or _
                '   (arrSunpoSetData(ii).SunpoMark = "S12") Or (arrSunpoSetData(ii).SunpoMark = "S34") Or (arrSunpoSetData(ii).SunpoMark = "S56") Then
                'If (arrSunpoSetData(ii).flgScale = 0) Or (arrSunpoSetData(ii).flgScale = 1) Then
                If ((arrSunpoSetData(ii).SunpoMark)(0) = "S") Or ((arrSunpoSetData(ii).SunpoMark)(0) = "C") Then
                    If arrSunpoSetData(ii).OpeFlg <> 2 Then
                        .Add(New ScaleItem)
                        arrScaleData(IDX3) = New ScaleItem
                        If arrSunpoSetData(ii).CT_ID1 = 0 Then
                            CTID = arrSunpoSetData(ii).GunID1
                            .Item(IDX3).StartTarget = ZahyouCTNO(CTID).ToString
                            arrScaleData(IDX3).StartTarget = .Item(IDX3).StartTarget
                        Else
                            .Item(IDX3).StartTarget = arrSunpoSetData(ii).CT_ID1.ToString
                            arrScaleData(IDX3).StartTarget = .Item(IDX3).StartTarget
                        End If
                        If arrSunpoSetData(ii).CT_ID2 = 0 Then
                            CTID = arrSunpoSetData(ii).GunID2
                            .Item(IDX3).EndTarget = ZahyouCTNO(CTID).ToString
                            arrScaleData(IDX3).EndTarget = .Item(IDX3).EndTarget
                        Else
                            .Item(IDX3).EndTarget = arrSunpoSetData(ii).CT_ID2.ToString
                            arrScaleData(IDX3).EndTarget = .Item(IDX3).EndTarget
                        End If
                        .Item(IDX3).SunpoMark = arrSunpoSetData(ii).SunpoMark
                        .Item(IDX3).SunpoName = arrSunpoSetData(ii).SunpoName
                        .Item(IDX3).KiteiVal = arrSunpoSetData(ii).KiteiVal.ToString
                        If arrSunpoSetData(ii).flgScale = 1 Then
                            .Item(IDX3).flgScale = 1
                        Else
                            .Item(IDX3).flgScale = 0
                        End If
                        .Item(IDX3).OpeFlg = arrSunpoSetData(ii).OpeFlg
                        .Item(IDX3).SunpoID = arrSunpoSetData(ii).SunpoID
                        arrScaleData(IDX3).SunpoMark = arrSunpoSetData(ii).SunpoMark
                        arrScaleData(IDX3).SunpoName = arrSunpoSetData(ii).SunpoName
                        arrScaleData(IDX3).OpeFlg = arrSunpoSetData(ii).OpeFlg
                        arrScaleData(IDX3).SunpoID = arrSunpoSetData(ii).SunpoID
                        arrScaleData(IDX3).KiteiVal = CDbl(arrSunpoSetData(ii).KiteiVal)
                        arrScaleData(IDX3).flgScale = arrSunpoSetData(ii).flgScale
                        IDX3 += 1
                    End If
                End If
            Next ii
        End With

        'ターゲット使用状況のセット
        For ii = 0 To IDX3 - 1
            For jj = 0 To m_TagetSetItems.Count - 1
                If CInt(arrScaleData(ii).StartTarget) = CInt(m_TagetSetItems.Item(jj).TargetNumber) Then
                    m_TagetSetItems.Item(jj).UseFlg = 1
                ElseIf CInt(arrScaleData(ii).EndTarget) = CInt(m_TagetSetItems.Item(jj).TargetNumber) Then
                    m_TagetSetItems.Item(jj).UseFlg = 1
                End If
            Next jj
        Next ii

        '基準ターゲット設定有無のチェック
        iFlg = 0
        For ii = 0 To IDX3 - 1
            If arrScaleData(ii).flgScale = 1 Then
                iFlg = 1
                Exit For
            End If
        Next ii
        If iFlg = 0 Then
            Dim ScaleD As New ScaleItem
            arrScaleData(0).flgScale = 1
            m_ScaleItems.Item(0).flgScale = 1
            ScaleD = arrScaleData(0)
            Update_Scale(ScaleD)
        End If

        Grid_Kijun.ItemsSource = m_ScaleItems
        m_Scalecount = IDX3
        Grid_Kijun.CurrentCell = Nothing
        Grid_Kijun.Items.Refresh()

        '座標系テーブルの設定
        Dim IR As Integer = Set_ZahyouData()

        With Grid_zahyou
            .CanUserAddRows = False
            .CanUserDeleteRows = False
            .ItemsSource = Nothing
        End With

        'XYorXZorYZの設定
        Combo_Zahyou.SelectedIndex = arrZahyouData.XYorXZorYZ

        With m_ZahyouItems
            .Clear()
            Select Case arrZahyouData.XYorXZorYZ
                Case 0
                    .Add(New ZahyouItem)
                    .Item(0) = arrZahyouGet(0)
                    .Item(0).CT_Type = "原点TG"
                    .Add(New ZahyouItem)
                    .Item(1) = arrZahyouGet(1)
                    .Item(1).CT_Type = "X軸TG(正)"
                    .Add(New ZahyouItem)
                    .Item(2) = arrZahyouGet(2)
                    .Item(2).CT_Type = "Y方向TG(正)"
                    Label_Height.Visibility = Windows.Visibility.Hidden
                    Text_Height.Visibility = Windows.Visibility.Hidden
                    Label_Unit.Visibility = Windows.Visibility.Hidden
                Case 1
                    .Add(New ZahyouItem)
                    .Item(0) = arrZahyouGet(0)
                    .Item(0).CT_Type = "原点TG(キングピン)"
                    .Add(New ZahyouItem)
                    .Item(1) = arrZahyouGet(1)
                    .Item(1).CT_Type = "X軸TG①"
                    .Add(New ZahyouItem)
                    .Item(2) = arrZahyouGet(2)
                    .Item(2).CT_Type = "X軸TG②"
                    .Add(New ZahyouItem)
                    .Item(3) = arrZahyouGet(4)
                    .Item(3).CT_Type = "平面TG①"
                    .Add(New ZahyouItem)
                    .Item(4) = arrZahyouGet(5)
                    .Item(4).CT_Type = "平面TG②"
                    .Add(New ZahyouItem)
                    .Item(5) = arrZahyouGet(6)
                    .Item(5).CT_Type = "平面TG③"
                    Text_Height.Text = Zahyou_HData.Zahyou_Height.ToString
                    Label_Height.Visibility = Windows.Visibility.Visible
                    Text_Height.Visibility = Windows.Visibility.Visible
                    Label_Unit.Visibility = Windows.Visibility.Visible
                Case 2
                    .Add(New ZahyouItem)
                    .Item(0) = arrZahyouGet(0)
                    .Item(0).CT_Type = "原点TG"
                    .Add(New ZahyouItem)
                    .Item(1) = arrZahyouGet(1)
                    .Item(1).CT_Type = "X軸TG①"
                    .Add(New ZahyouItem)
                    .Item(2) = arrZahyouGet(2)
                    .Item(2).CT_Type = "X軸TG②"
                    .Add(New ZahyouItem)
                    .Item(3) = arrZahyouGet(4)
                    .Item(3).CT_Type = "平面TG①"
                    .Add(New ZahyouItem)
                    .Item(4) = arrZahyouGet(5)
                    .Item(4).CT_Type = "平面TG②"
                    .Add(New ZahyouItem)
                    .Item(5) = arrZahyouGet(6)
                    .Item(5).CT_Type = "平面TG③"
                    Label_Height.Visibility = Windows.Visibility.Hidden
                    Text_Height.Visibility = Windows.Visibility.Hidden
                    Label_Unit.Visibility = Windows.Visibility.Hidden
                Case 3
                    .Add(New ZahyouItem)
                    .Item(0) = arrZahyouGet(0)
                    .Item(0).CT_Type = "原点TG"
                    .Add(New ZahyouItem)
                    .Item(1) = arrZahyouGet(1)
                    .Item(1).CT_Type = "X軸TG(正)"
                    .Add(New ZahyouItem)
                    .Item(2) = arrZahyouGet(2)
                    .Item(2).CT_Type = "Z軸TG(始点)"
                    .Add(New ZahyouItem)
                    .Item(3) = arrZahyouGet(3)
                    .Item(3).CT_Type = "Z軸TG(終点)"
                    Label_Height.Visibility = Windows.Visibility.Hidden
                    Text_Height.Visibility = Windows.Visibility.Hidden
                    Label_Unit.Visibility = Windows.Visibility.Hidden
                Case 4
                    .Add(New ZahyouItem)
                    .Item(0) = arrZahyouGet(0)
                    .Item(0).CT_Type = "原点TG"
                    .Add(New ZahyouItem)
                    .Item(1) = arrZahyouGet(1)
                    .Item(1).CT_Type = "X軸TG(正)"
                    .Add(New ZahyouItem)
                    .Item(2) = arrZahyouGet(2)
                    .Item(2).CT_Type = "Y方向TG(負)"
                    Label_Height.Visibility = Windows.Visibility.Hidden
                    Text_Height.Visibility = Windows.Visibility.Hidden
                    Label_Unit.Visibility = Windows.Visibility.Hidden
                Case 5
                    .Add(New ZahyouItem)
                    .Item(0) = arrZahyouGet(4)
                    .Item(0).CT_Type = "平面TG①"
                    .Add(New ZahyouItem)
                    .Item(1) = arrZahyouGet(5)
                    .Item(1).CT_Type = "平面TG②"
                    .Add(New ZahyouItem)
                    .Item(2) = arrZahyouGet(6)
                    .Item(2).CT_Type = "平面TG③"
                    .Add(New ZahyouItem)
                    .Item(3) = arrZahyouGet(0)
                    .Item(3).CT_Type = "X軸TG(始点)"
                    .Add(New ZahyouItem)
                    .Item(4) = arrZahyouGet(1)
                    .Item(4).CT_Type = "X軸TG(終点)"
                    Label_Height.Visibility = Windows.Visibility.Hidden
                    Text_Height.Visibility = Windows.Visibility.Hidden
                    Label_Unit.Visibility = Windows.Visibility.Hidden
                Case 6
                    .Add(New ZahyouItem)
                    .Item(0) = arrZahyouGet(4)
                    .Item(0).CT_Type = "平面TG①"
                    .Add(New ZahyouItem)
                    .Item(1) = arrZahyouGet(5)
                    .Item(1).CT_Type = "平面TG②"
                    .Add(New ZahyouItem)
                    .Item(2) = arrZahyouGet(6)
                    .Item(2).CT_Type = "平面TG③"
                    .Add(New ZahyouItem)
                    .Item(3) = arrZahyouGet(0)
                    .Item(3).CT_Type = "原点TG"
                    .Add(New ZahyouItem)
                    .Item(4) = arrZahyouGet(1)
                    .Item(4).CT_Type = "X方向TG"
                    Label_Height.Visibility = Windows.Visibility.Hidden
                    Text_Height.Visibility = Windows.Visibility.Hidden
                    Label_Unit.Visibility = Windows.Visibility.Hidden
            End Select
        End With

        'ターゲット使用状況のセット
        For ii = 0 To m_TagetSetItems.Count - 1
            If (CInt(arrZahyouGet(0).CT_No) = CInt(m_TagetSetItems.Item(ii).TargetNumber)) Or _
               (CInt(arrZahyouGet(1).CT_No) = CInt(m_TagetSetItems.Item(ii).TargetNumber)) Or _
               (CInt(arrZahyouGet(2).CT_No) = CInt(m_TagetSetItems.Item(ii).TargetNumber)) Or _
               (CInt(arrZahyouGet(3).CT_No) = CInt(m_TagetSetItems.Item(ii).TargetNumber)) Or _
               (CInt(arrZahyouGet(4).CT_No) = CInt(m_TagetSetItems.Item(ii).TargetNumber)) Or _
               (CInt(arrZahyouGet(5).CT_No) = CInt(m_TagetSetItems.Item(ii).TargetNumber)) Or _
               (CInt(arrZahyouGet(6).CT_No) = CInt(m_TagetSetItems.Item(ii).TargetNumber)) Then
                m_TagetSetItems.Item(ii).UseFlg = 1
            End If
        Next ii

        Grid_zahyou.ItemsSource = m_ZahyouItems
        Grid_zahyou.Items.Refresh()
    End Sub

    '「２点による線分・長さ」タブの値を設定する
    Private Sub TabLineLength_Set(ByVal IDX As Integer)
        Dim IDX3 As Integer = 0

        Dim IDX2 = Set_LineLengthData()

        'データ表示
        With Grid_Line
            '並び替え禁止
            For Each T As System.Windows.Controls.DataGridColumn In .Columns
                T.CanUserSort = False
            Next
            .CanUserAddRows = False
            .CanUserDeleteRows = False
            .ItemsSource = Nothing
        End With

        With m_LineLengthItems
            .Clear()
            IDX3 = 0
            For ii = 0 To IDX2 - 1
                'If (arrLineLegthData(ii).SunpoMark = "S-A") Or (arrLineLegthData(ii).SunpoMark = "S-B") Or _
                '   (arrLineLegthData(ii).SunpoMark = "S12") Or (arrLineLegthData(ii).SunpoMark = "S34") Or (arrLineLegthData(ii).SunpoMark = "S56") Then
                If (arrLineLegthData(ii).flgScale = 0) Or (arrLineLegthData(ii).flgScale = 1) Then
                    Continue For
                Else
                    .Add(New LineLengthItem)
                    .Item(IDX3).StartTarget = arrLineLegthData(ii).StartTarget
                    .Item(IDX3).EndTarget = arrLineLegthData(ii).EndTarget
                    Select Case arrLineLegthData(ii).Dimension
                        Case XYZseibun.X
                            .Item(IDX3).Dimension = "X"
                        Case XYZseibun.Y
                            .Item(IDX3).Dimension = "Y"
                        Case XYZseibun.Z
                            .Item(IDX3).Dimension = "Z"
                        Case XYZseibun.XY
                            .Item(IDX3).Dimension = "XY"
                        Case XYZseibun.XZ
                            .Item(IDX3).Dimension = "XZ"
                        Case XYZseibun.YZ
                            .Item(IDX3).Dimension = "YZ"
                        Case XYZseibun.XYZ
                            .Item(IDX3).Dimension = "XYZ"
                    End Select
                    .Item(IDX3).Layer = arrLineLegthData(ii).Layer
                    ncolor = 0
                    YCM_ReadSystemColorAcs(m_strDataSystemPath)
                    Dim mcolor As ModelColor
                    If ncolor > 0 Then
                        mcolor = YCM_GetColorInfoByCode(arrLineLegthData(ii).LineColor)
                        .Item(IDX3).LineColor = mcolor.strName
                    End If
                    nLineType = 0
                    YCM_ReadSystemLineTypes(m_strDataSystemPath)
                    Dim mLinetype As LineType
                    If nLineType > 0 Then
                        If arrLineLegthData(ii).LineType <= 0 Then
                            .Item(IDX3).LineType = "CONTINUOUS"
                        Else
                            mLinetype = YCM_GetLineTypeInfoByCode(arrLineLegthData(ii).LineType)
                            .Item(IDX3).LineType = mLinetype.strName
                        End If
                    End If
                    .Item(IDX3).SunpoMark = arrLineLegthData(ii).SunpoMark
                    .Item(IDX3).SunpoName = arrLineLegthData(ii).SunpoName
                    .Item(IDX3).OpeFlg = 0
                    .Item(IDX3).SunpoID = arrLineLegthData(ii).SunpoID
                    IDX3 += 1
                End If
            Next ii
        End With

        'ターゲット使用状況のセット
        For ii = 0 To IDX3 - 1
            For jj = 0 To m_TagetSetItems.Count - 1
                If CInt(arrLineLegthData(ii).StartTarget) = CInt(m_TagetSetItems.Item(jj).TargetNumber) Then
                    m_TagetSetItems.Item(jj).UseFlg = 1
                ElseIf CInt(arrLineLegthData(ii).EndTarget) = CInt(m_TagetSetItems.Item(jj).TargetNumber) Then
                    m_TagetSetItems.Item(jj).UseFlg = 1
                End If
            Next jj
        Next ii

        Grid_Line.ItemsSource = m_LineLengthItems
        m_LIneLengthcount = IDX3
        Grid_Line.CurrentCell = Nothing
        Grid_Line.Items.Refresh()
    End Sub

    '「ポリラインタブ」の値を設定する
    Private Sub TabPolyLine_Set(ByVal IDX As Integer)

        Dim IDX2 As Integer
        Dim IDX3 = Set_PolyLineData()
        Dim vertNum As Integer = 0
        Dim arrTarget(100) As Integer
        Dim ii, jj, kk As Integer

        'データ表示
        With Grid_PolyLine
            '並び替え禁止
            For Each T As System.Windows.Controls.DataGridColumn In .Columns
                T.CanUserSort = False
            Next
            .CanUserAddRows = False
            .CanUserDeleteRows = False
            .ItemsSource = Nothing
        End With

        With m_PolyLineItems
            .Clear()
            IDX2 = 0
            For ii = 0 To IDX3 - 1
                .Add(New PolyLineItem)
                .Item(IDX2).VertexTarget = arrPolyLineData(ii).VertexTarget
                Select Case arrPolyLineData(ii).Dimension
                    Case XYZseibun.X
                        .Item(IDX2).Dimension = "X"
                    Case XYZseibun.Y
                        .Item(IDX2).Dimension = "Y"
                    Case XYZseibun.Z
                        .Item(IDX2).Dimension = "Z"
                    Case XYZseibun.XY
                        .Item(IDX2).Dimension = "XY"
                    Case XYZseibun.XZ
                        .Item(IDX2).Dimension = "XZ"
                    Case XYZseibun.YZ
                        .Item(IDX2).Dimension = "YZ"
                    Case XYZseibun.XYZ
                        .Item(IDX2).Dimension = "XYZ"
                End Select
                .Item(IDX2).Layer = arrPolyLineData(ii).Layer
                ncolor = 0
                YCM_ReadSystemColorAcs(m_strDataSystemPath)
                Dim mcolor As ModelColor
                If ncolor > 0 Then
                    mcolor = YCM_GetColorInfoByCode(arrPolyLineData(ii).LineColor)
                    .Item(IDX2).LineColor = mcolor.strName
                End If
                nLineType = 0
                YCM_ReadSystemLineTypes(m_strDataSystemPath)
                Dim mLinetype As LineType
                If nLineType > 0 Then
                    If arrPolyLineData(ii).LineType <= 0 Then
                        .Item(IDX2).LineType = "CONTINUOUS"
                    Else
                        mLinetype = YCM_GetLineTypeInfoByCode(arrPolyLineData(ii).LineType)
                        .Item(IDX2).LineType = mLinetype.strName
                    End If
                End If
                .Item(IDX2).SunpoMark = arrPolyLineData(ii).SunpoMark
                .Item(IDX2).SunpoName = arrPolyLineData(ii).SunpoName
                .Item(IDX2).OpeFlg = 0
                .Item(IDX2).SunpoID = arrPolyLineData(ii).SunpoID
                IDX2 += 1
            Next ii
        End With

        'ターゲット使用状況のセット
        For ii = 0 To IDX2 - 1
            vertNum = GetNumber_TargetString(arrPolyLineData(ii).VertexTarget, arrTarget)
            If vertNum > 0 Then
                For jj = 0 To vertNum - 1
                    For kk = 0 To m_TagetSetItems.Count - 1
                        If arrTarget(jj) = CInt(m_TagetSetItems.Item(kk).TargetNumber) Then
                            m_TagetSetItems.Item(kk).UseFlg = 1
                            Exit For
                        End If
                    Next kk
                Next jj
            End If
        Next ii

        Grid_PolyLine.ItemsSource = m_PolyLineItems
        m_PolyLinecount = IDX2
        Grid_PolyLine.CurrentCell = Nothing
        Grid_PolyLine.Items.Refresh()
    End Sub

    '「１点による円」タブの値を設定する
    Private Sub TabCircle_Set(ByVal IDX As Integer)

        Dim IDX3 = Set_CircleData()
        Dim vertNum As Integer = 0
        Dim arrTarget(100) As Integer
        Dim ii, jj, kk As Integer

        'データ表示
        With Grid_Circle
            '並び替え禁止
            For Each T As System.Windows.Controls.DataGridColumn In .Columns
                T.CanUserSort = False
            Next
            .CanUserAddRows = False
            .CanUserDeleteRows = False
            .ItemsSource = Nothing
        End With

        With m_CircleItems
            .Clear()
            For ii = 0 To IDX3 - 1
                .Add(New CircleItem)
            Next ii
            For ii = 0 To IDX3 - 1
                .Item(ii).CenterTarget = arrCircleData(ii).CenterTarget
                .Item(ii).Diameter = arrCircleData(ii).Diameter.ToString
                Select Case arrCircleData(ii).Dimension
                    Case XYZseibun.X
                        .Item(ii).Dimension = "X"
                    Case XYZseibun.Y
                        .Item(ii).Dimension = "Y"
                    Case XYZseibun.Z
                        .Item(ii).Dimension = "Z"
                    Case XYZseibun.XY
                        .Item(ii).Dimension = "XY"
                    Case XYZseibun.XZ
                        .Item(ii).Dimension = "XZ"
                    Case XYZseibun.YZ
                        .Item(ii).Dimension = "YZ"
                    Case XYZseibun.XYZ
                        .Item(ii).Dimension = "XYZ"
                End Select
                .Item(ii).Layer = arrCircleData(ii).Layer
                ncolor = 0
                YCM_ReadSystemColorAcs(m_strDataSystemPath)
                Dim mcolor As ModelColor
                If ncolor > 0 Then
                    mcolor = YCM_GetColorInfoByCode(arrCircleData(ii).LineColor)
                    .Item(ii).LineColor = mcolor.strName
                End If
                nLineType = 0
                YCM_ReadSystemLineTypes(m_strDataSystemPath)
                Dim mLinetype As LineType
                If nLineType > 0 Then
                    If arrCircleData(ii).LineType <= 0 Then
                        .Item(ii).LineType = "CONTINUOUS"
                    Else
                        mLinetype = YCM_GetLineTypeInfoByCode(arrCircleData(ii).LineType)
                        .Item(ii).LineType = mLinetype.strName
                    End If
                End If
                .Item(ii).SunpoMark = arrCircleData(ii).SunpoMark
                .Item(ii).SunpoName = arrCircleData(ii).SunpoName
                .Item(ii).OpeFlg = 0
                .Item(ii).SunpoID = arrCircleData(ii).SunpoID
            Next ii
        End With

        'ターゲット使用状況のセット
        For ii = 0 To IDX3 - 1
            vertNum = GetNumber_TargetString(arrCircleData(ii).CenterTarget, arrTarget)
            If vertNum > 0 Then
                For jj = 0 To vertNum - 1
                    For kk = 0 To m_TagetSetItems.Count - 1
                        If arrTarget(jj) = CInt(m_TagetSetItems.Item(kk).TargetNumber) Then
                            m_TagetSetItems.Item(kk).UseFlg = 1
                            Exit For
                        End If
                    Next kk
                Next jj
            End If
        Next ii

        Grid_Circle.ItemsSource = m_CircleItems
        m_Circlecount = IDX3
        Grid_Circle.CurrentCell = Nothing
        Grid_Circle.Items.Refresh()
    End Sub

    'ターゲットテーブルのセルチェンジイベント   
    Private Sub Grid_Target_CurrentCellChanged(sender As System.Object, e As System.EventArgs) Handles Grid_Target.CurrentCellChanged
        If Grid_Target.CurrentColumn IsNot Nothing Then
            If beforeTargetItem IsNot Nothing Then
                TargetItemChanged(beforeTargetItem, beforeTargetCulumn)
            End If
            beforeTargetCulumn = Grid_Target.CurrentColumn.DisplayIndex
            beforeTargetItem = Grid_Target.CurrentItem
            Grid_Target.Focus()
            Grid_Target.BeginEdit()
        Else
            If beforeTargetItem IsNot Nothing Then
                TargetItemChanged(beforeTargetItem, beforeTargetCulumn)
            End If
            If delTargetItem Is Nothing Then
                delTargetItem = beforeTargetItem
                delTargetCulumn = beforeTargetCulumn
            End If
            beforeTargetCulumn = -1
            beforeTargetItem = Nothing
        End If
    End Sub

    'スケールテーブルのセルチェンジイベント   
    Private Sub Grid_Kijun_CurrentCellChanged(sender As System.Object, e As System.EventArgs) Handles Grid_Kijun.CurrentCellChanged
        If Grid_Kijun.CurrentColumn IsNot Nothing Then
            'If Grid_Kijun.CurrentColumn.DisplayIndex > 0 Then
            If beforeScaleItem IsNot Nothing Then
                ScaleItemChanged(beforeScaleItem, beforeScaleCulumn)
            End If
            beforeScaleCulumn = Grid_Kijun.CurrentColumn.DisplayIndex
            beforeScaleItem = Grid_Kijun.CurrentItem
            Grid_Kijun.Focus()
            Grid_Kijun.BeginEdit()
            'End If
        Else
            'If beforeScaleCulumn > 0 Then
            If beforeScaleItem IsNot Nothing Then
                ScaleItemChanged(beforeScaleItem, beforeScaleCulumn)
            End If
            If delScaleItem Is Nothing Then
                delScaleItem = beforeScaleItem
                delScaleCulumn = beforeScaleCulumn
            End If
            beforeScaleCulumn = -1
            beforeScaleItem = Nothing
            'End If
        End If
    End Sub

    '座標系テーブルのセルチェンジイベント   
    Private Sub Grid_zahyou_CurrentCellChanged(sender As System.Object, e As System.EventArgs) Handles Grid_zahyou.CurrentCellChanged
        If Grid_zahyou.CurrentColumn IsNot Nothing Then
            If beforeZahyouItem IsNot Nothing Then
                zahyouItemChanged(beforeZahyouItem, beforeZahyouCulumn)
            End If
            beforeZahyouCulumn = Grid_zahyou.CurrentColumn.DisplayIndex
            beforeZahyouItem = Grid_zahyou.CurrentItem
            Grid_zahyou.Focus()
            Grid_zahyou.BeginEdit()
        Else
            If beforeZahyouItem IsNot Nothing Then
                zahyouItemChanged(beforeZahyouItem, beforeZahyouCulumn)
            End If
            If delZahyouItem Is Nothing Then
                delZahyouItem = beforeZahyouItem
                delZahyouCulumn = beforeZahyouCulumn
            End If
            beforeZahyouCulumn = -1
            beforeZahyouItem = Nothing
        End If
    End Sub

    '２点線分テーブルのセルチェンジイベント   
    Private Sub Grid_Line_CurrentCellChanged(sender As System.Object, e As System.EventArgs) Handles Grid_Line.CurrentCellChanged
        If Grid_Line.CurrentColumn IsNot Nothing Then
            If beforeLineItem IsNot Nothing Then
                LineLengthItemChanged(beforeLineItem, beforeLineCulumn)
            End If
            beforeLineCulumn = Grid_Line.CurrentColumn.DisplayIndex
            beforeLineItem = Grid_Line.CurrentItem
            Grid_Line.Focus()
            Grid_Line.BeginEdit()
        Else
            If beforeLineItem IsNot Nothing Then
                LineLengthItemChanged(beforeLineItem, beforeLineCulumn)
            End If
            If delLineItem Is Nothing Then
                delLineItem = beforeLineItem
                delLineCulumn = beforeLineCulumn
            End If
            beforeLineCulumn = -1
            beforeLineItem = Nothing
        End If
    End Sub

    'ポリラインテーブルのセルチェンジイベント   
    Private Sub Grid_PolyLine_CurrentCellChanged(sender As System.Object, e As System.EventArgs) Handles Grid_PolyLine.CurrentCellChanged
        If Grid_PolyLine.CurrentColumn IsNot Nothing Then
            If beforePolyLineItem IsNot Nothing Then
                PolyLineItemChanged(beforePolyLineItem, beforeLineCulumn)
            End If
            beforePolyLineCulumn = Grid_PolyLine.CurrentColumn.DisplayIndex
            beforePolyLineItem = Grid_PolyLine.CurrentItem
            Grid_PolyLine.Focus()
            Grid_PolyLine.BeginEdit()
        Else
            If beforePolyLineItem IsNot Nothing Then
                PolyLineItemChanged(beforePolyLineItem, beforeLineCulumn)
            End If
            If delPolyLineItem Is Nothing Then
                delPolyLineItem = beforePolyLineItem
                delPolyLineCulumn = beforePolyLineCulumn
            End If
            beforePolyLineCulumn = -1
            beforePolyLineItem = Nothing
        End If
    End Sub

    '１点円テーブルのセルチェンジイベント   
    Private Sub Grid_Circle_CurrentCellChanged(sender As System.Object, e As System.EventArgs) Handles Grid_Circle.CurrentCellChanged
        If Grid_Circle.CurrentColumn IsNot Nothing Then
            If beforeCircleItem IsNot Nothing Then
                CircleItemChanged(beforeCircleItem, beforeLineCulumn)
            End If
            beforeCircleCulumn = Grid_Circle.CurrentColumn.DisplayIndex
            beforeCircleItem = Grid_Circle.CurrentItem
            Grid_Circle.Focus()
            Grid_Circle.BeginEdit()
        Else
            If beforeCircleItem IsNot Nothing Then
                CircleItemChanged(beforeCircleItem, beforeLineCulumn)
            End If
            If delCircleItem Is Nothing Then
                delCircleItem = beforeCircleItem
                delCircleCulumn = beforeCircleCulumn
            End If
            beforeCircleCulumn = -1
            beforeCircleItem = Nothing
        End If
    End Sub

    'ターゲットテーブル変更時の処理
    Private Sub TargetItemChanged(ByVal Items As TargetSetItem, ByVal iColumn As Integer)
        Dim ii As Integer
        Dim chgFlg As Integer = 0
        Dim strMsg As String

        If (Items.TargetInfo Is Nothing) Then
        ElseIf (Items.OpeFlg <> 3) Then
            For ii = 0 To m_Targetcount - 1
                If arrCToffsetData(ii).ID = Items.ID Then
                    If arrCToffsetData(ii).CT_NO <> CInt(Items.TargetNumber) Then
                        '(20150305 Tezuka) バグ修正
                        'arrCToffsetData(ii).CT_NO = Items.TargetNumber
                        'arrCToffsetData(ii).OpeFlg = 1
                        strMsg = "追加したターゲット以外は、ターゲット番号の変更はできません。"
                        MsgBox(strMsg, MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "ターゲット入力エラー")
                        Items.TargetNumber = arrCToffsetData(ii).CT_NO.ToString
                        chgFlg = 1
                    End If
                    If System.Math.Abs(arrCToffsetData(ii).m_x - CDbl(Items.TargetOffsetX)) > 0.0001 Then
                        arrCToffsetData(ii).m_x = CDbl(Items.TargetOffsetX)
                        arrCToffsetData(ii).OpeFlg = 1
                    End If
                    If System.Math.Abs(arrCToffsetData(ii).m_y - CDbl(Items.TargetOffsetY)) > 0.0001 Then
                        arrCToffsetData(ii).m_y = CDbl(Items.TargetOffsetY)
                        arrCToffsetData(ii).OpeFlg = 1
                    End If
                    If System.Math.Abs(arrCToffsetData(ii).m_z - CDbl(Items.TargetOffsetZ)) > 0.0001 Then
                        arrCToffsetData(ii).m_z = CDbl(Items.TargetOffsetZ)
                        arrCToffsetData(ii).OpeFlg = 1
                    End If
                    If arrCToffsetData(ii).info <> Items.TargetInfo Then
                        arrCToffsetData(ii).info = Items.TargetInfo
                        arrCToffsetData(ii).OpeFlg = 1
                    End If
                    Exit For
                End If
            Next ii
        Else
            'For ii = 0 To m_Targetcount - 1
            '    If arrCToffsetData(ii).CT_NO = Items.TargetNumber Then
            '        '(20150305 Tezuka) バグ修正
            '        'arrCToffsetData(ii).CT_NO = Items.TargetNumber
            '        'arrCToffsetData(ii).OpeFlg = 1
            '        strMsg = "<ターゲット番号：" & Items.TargetNumber.ToString & "> は、既に登録されています。"
            '        MsgBox(strMsg, MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "ターゲット入力エラー")
            '        Items.TargetNumber = 0
            '        Exit For
            '    End If
            'Next ii
            chgFlg = 1
        End If

        If chgFlg = 1 Then
            'Grid_Target.CommitEdit()
            'Grid_Target.Items.Refresh()
            'Grid_Refresh()
        End If
    End Sub

    'スケールテーブル変更時の処理
    Private Sub ScaleItemChanged(ByVal Items As ScaleItem, ByVal iColumn As Integer)
        Dim ii As Integer
        Dim chgFlg As Integer = 0
        Dim chgStartTarget As Integer = -1
        Dim chgEndTarget As Integer = -1

        If Items.OpeFlg <> 3 Then
            For ii = 0 To m_Scalecount - 1
                If arrScaleData(ii).SunpoID = Items.SunpoID Then
                    If arrScaleData(ii).StartTarget <> Items.StartTarget Then
                        chgStartTarget = CInt(arrScaleData(ii).StartTarget)
                        arrScaleData(ii).StartTarget = Items.StartTarget
                        arrScaleData(ii).OpeFlg = 1
                        chgFlg = 1
                    End If
                    If arrScaleData(ii).EndTarget <> Items.EndTarget Then
                        chgEndTarget = CInt(arrScaleData(ii).EndTarget)
                        arrScaleData(ii).EndTarget = Items.EndTarget
                        arrScaleData(ii).OpeFlg = 1
                        chgFlg = 1
                    End If
                    If arrScaleData(ii).SunpoMark <> Items.SunpoMark Then
                        arrScaleData(ii).SunpoMark = Items.SunpoMark
                        arrScaleData(ii).OpeFlg = 1
                    End If
                    If arrScaleData(ii).SunpoName <> Items.SunpoName Then
                        arrScaleData(ii).SunpoName = Items.SunpoName
                        arrScaleData(ii).OpeFlg = 1
                    End If
                    If System.Math.Abs(arrScaleData(ii).KiteiVal - CDbl(Items.KiteiVal)) > 0.0001 Then
                        arrScaleData(ii).KiteiVal = CDbl(Items.KiteiVal)
                        arrScaleData(ii).OpeFlg = 1
                    End If
                    If arrScaleData(ii).flgScale <> Items.flgScale Then
                        arrScaleData(ii).flgScale = Items.flgScale
                        arrScaleData(ii).OpeFlg = 1
                    End If
                    Exit For
                End If
            Next ii
        Else
            chgFlg = 1
        End If

        If chgFlg = 1 Then
            Dim chgFlg2 As Integer = 0
            For ii = 0 To m_TagetSetItems.Count - 1
                If (CInt(m_TagetSetItems.Item(ii).TargetNumber) = CInt(Items.StartTarget)) Or (CInt(m_TagetSetItems.Item(ii).TargetNumber) = CInt(Items.EndTarget)) Then
                    If m_TagetSetItems.Item(ii).UseFlg = 0 Then
                        m_TagetSetItems.Item(ii).UseFlg = 1
                        chgFlg2 = 1
                    End If
                End If
                If (chgStartTarget >= 0) And (CInt(m_TagetSetItems.Item(ii).TargetNumber) = chgStartTarget) Then
                    m_TagetSetItems.Item(ii).UseFlg = 0
                    chgFlg2 = 1
                End If
                If (chgEndTarget >= 0) And (CInt(m_TagetSetItems.Item(ii).TargetNumber) = chgEndTarget) Then
                    m_TagetSetItems.Item(ii).UseFlg = 0
                    chgFlg2 = 1
                End If
            Next ii
            If chgFlg2 = 1 Then
                Grid_Refresh()
            End If
        End If
    End Sub

    '座標系テーブル変更時の処理
    Private Sub zahyouItemChanged(ByVal Items As ZahyouItem, ByVal iColumn As Integer)

        If (Items.OpeFlg <> 3) And (iColumn <> -1) Then
            If (CInt(Items.CT_No) > 0) And (CInt(Items.CT_No) <> edtZahyouCTNo) Then
                arrZahyouGet(Items.ID).CT_No = Items.CT_No
                arrZahyouGet(Items.ID).OpeFlg = 1
                arrZahyouData.OpeFlg = 1
            End If
        End If

    End Sub

    '２点線分テーブル変更時の処理
    Private Sub LineLengthItemChanged(ByVal Items As LineLengthItem, ByVal iColumn As Integer)
        Dim ii As Integer
        Dim chgStartTarget As Integer = -1
        Dim chgEndTarget As Integer = -1
        Dim chgFlg As Integer = 0

        If Items.OpeFlg <> 3 Then
            For ii = 0 To m_LIneLengthcount - 1
                If arrLineLegthData(ii).SunpoID = Items.SunpoID Then
                    If arrLineLegthData(ii).StartTarget <> Items.StartTarget Then
                        chgStartTarget = CInt(arrLineLegthData(ii).StartTarget)
                        arrLineLegthData(ii).StartTarget = Items.StartTarget
                        arrLineLegthData(ii).OpeFlg = 1
                        chgFlg = 1
                    End If
                    If arrLineLegthData(ii).EndTarget <> Items.EndTarget Then
                        chgEndTarget = CInt(arrLineLegthData(ii).EndTarget)
                        arrLineLegthData(ii).EndTarget = Items.EndTarget
                        arrLineLegthData(ii).OpeFlg = 1
                        chgFlg = 1
                    End If
                    If arrLineLegthData(ii).SunpoMark <> Items.SunpoMark Then
                        arrLineLegthData(ii).SunpoMark = Items.SunpoMark
                        arrLineLegthData(ii).OpeFlg = 1
                    End If
                    If arrLineLegthData(ii).SunpoName <> Items.SunpoName Then
                        arrLineLegthData(ii).SunpoName = Items.SunpoName
                        arrLineLegthData(ii).OpeFlg = 1
                    End If
                    Dim Dimension As Integer = 0
                    Select Case Items.Dimension
                        Case "X"
                            Dimension = XYZseibun.X
                        Case "Y"
                            Dimension = XYZseibun.Y
                        Case "Z"
                            Dimension = XYZseibun.Z
                        Case "XY"
                            Dimension = XYZseibun.XY
                        Case "XZ"
                            Dimension = XYZseibun.XZ
                        Case "YZ"
                            Dimension = XYZseibun.YZ
                        Case "XYZ"
                            Dimension = XYZseibun.XYZ
                    End Select
                    If arrLineLegthData(ii).Dimension <> Dimension Then
                        arrLineLegthData(ii).Dimension = Dimension
                        arrLineLegthData(ii).OpeFlg = 1
                    End If
                    If arrLineLegthData(ii).Layer <> Items.Layer Then
                        arrLineLegthData(ii).Layer = Items.Layer
                        arrLineLegthData(ii).OpeFlg = 1
                    End If
                    Dim colorID As Integer
                    ncolor = 0
                    YCM_ReadSystemColorAcs(m_strDataSystemPath)
                    Dim mcolor As ModelColor
                    If ncolor > 0 Then
                        mcolor = YCM_GetColorInfoByName(Items.LineColor)
                        colorID = mcolor.code
                    End If
                    If arrLineLegthData(ii).LineColor <> colorID Then
                        arrLineLegthData(ii).LineColor = colorID
                        arrLineLegthData(ii).OpeFlg = 1
                    End If
                    Dim ltype As Integer
                    nLineType = 0
                    YCM_ReadSystemLineTypes(m_strDataSystemPath)
                    Dim mLinetype As LineType
                    If nLineType > 0 Then
                        mLinetype = YCM_GetLineTypeInfoByName(Items.LineType)
                        ltype = mLinetype.code
                    End If
                    If arrLineLegthData(ii).LineType <> ltype Then
                        arrLineLegthData(ii).LineType = ltype
                        arrLineLegthData(ii).OpeFlg = 1
                    End If
                    Exit For
                End If
            Next ii
        End If

        If chgFlg = 1 Then
            Dim chgFlg2 As Integer = 0
            For ii = 0 To m_TagetSetItems.Count - 1
                If (CInt(m_TagetSetItems.Item(ii).TargetNumber) = CInt(Items.StartTarget)) Or (CInt(m_TagetSetItems.Item(ii).TargetNumber) = CInt(Items.EndTarget)) Then
                    If m_TagetSetItems.Item(ii).UseFlg = 0 Then
                        m_TagetSetItems.Item(ii).UseFlg = 1
                        chgFlg2 = 1
                    End If
                End If
                If (chgStartTarget >= 0) And (CInt(m_TagetSetItems.Item(ii).TargetNumber) = chgStartTarget) Then
                    m_TagetSetItems.Item(ii).UseFlg = 0
                    chgFlg2 = 1
                End If
                If (chgEndTarget >= 0) And (CInt(m_TagetSetItems.Item(ii).TargetNumber) = chgEndTarget) Then
                    m_TagetSetItems.Item(ii).UseFlg = 0
                    chgFlg2 = 1
                End If
            Next ii
            If chgFlg2 = 1 Then
                Grid_Refresh()
            End If
        End If

    End Sub

    'ポリラインテーブル変更時の処理
    Private Sub PolyLineItemChanged(ByVal Items As PolyLineItem, ByVal iColumn As Integer)
        Dim ii, jj As Integer
        Dim vertTarget As String = ""
        Dim arrTarget(100) As Integer
        Dim vertNum As Integer = 0
        Dim chgFlg As Integer = 0

        If Items.OpeFlg <> 3 Then
            For ii = 0 To m_PolyLinecount - 1
                If arrPolyLineData(ii).SunpoID = Items.SunpoID Then
                    If arrPolyLineData(ii).VertexTarget <> Items.VertexTarget Then
                        vertTarget = arrPolyLineData(ii).VertexTarget
                        arrPolyLineData(ii).VertexTarget = Items.VertexTarget
                        arrPolyLineData(ii).OpeFlg = 1
                        chgFlg = 1
                    End If
                    If arrPolyLineData(ii).SunpoMark <> Items.SunpoMark Then
                        arrPolyLineData(ii).SunpoMark = Items.SunpoMark
                        arrPolyLineData(ii).OpeFlg = 1
                    End If
                    If arrPolyLineData(ii).SunpoName <> Items.SunpoName Then
                        arrPolyLineData(ii).SunpoName = Items.SunpoName
                        arrPolyLineData(ii).OpeFlg = 1
                    End If
                    Dim Dimension As Integer = 0
                    Select Case Items.Dimension
                        Case "X"
                            Dimension = XYZseibun.X
                        Case "Y"
                            Dimension = XYZseibun.Y
                        Case "Z"
                            Dimension = XYZseibun.Z
                        Case "XY"
                            Dimension = XYZseibun.XY
                        Case "XZ"
                            Dimension = XYZseibun.XZ
                        Case "YZ"
                            Dimension = XYZseibun.YZ
                        Case "XYZ"
                            Dimension = XYZseibun.XYZ
                    End Select
                    If arrPolyLineData(ii).Dimension <> Dimension Then
                        arrPolyLineData(ii).Dimension = Dimension
                        arrPolyLineData(ii).OpeFlg = 1
                    End If
                    If arrPolyLineData(ii).Layer <> Items.Layer Then
                        arrPolyLineData(ii).Layer = Items.Layer
                        arrPolyLineData(ii).OpeFlg = 1
                    End If
                    Dim colorID As Integer
                    ncolor = 0
                    YCM_ReadSystemColorAcs(m_strDataSystemPath)
                    Dim mcolor As ModelColor
                    If ncolor > 0 Then
                        mcolor = YCM_GetColorInfoByName(Items.LineColor)
                        colorID = mcolor.code
                    End If
                    If arrPolyLineData(ii).LineColor <> colorID Then
                        arrPolyLineData(ii).LineColor = colorID
                        arrPolyLineData(ii).OpeFlg = 1
                    End If
                    Dim ltype As Integer
                    nLineType = 0
                    YCM_ReadSystemLineTypes(m_strDataSystemPath)
                    Dim mLinetype As LineType
                    If nLineType > 0 Then
                        mLinetype = YCM_GetLineTypeInfoByName(Items.LineType)
                        ltype = mLinetype.code
                    End If
                    If arrPolyLineData(ii).LineType <> ltype Then
                        arrPolyLineData(ii).LineType = ltype
                        arrPolyLineData(ii).OpeFlg = 1
                    End If
                    Exit For
                End If
            Next ii
        Else
            chgFlg = 1
        End If

        If chgFlg = 1 Then
            Dim chgFlg2 As Integer = 0
            If vertTarget <> "" Then
                vertNum = GetNumber_TargetString(vertTarget, arrTarget)
                If vertNum > 0 Then
                    For ii = 0 To vertNum - 1
                        For jj = 0 To m_TagetSetItems.Count - 1
                            If arrTarget(ii) = CInt(m_TagetSetItems.Item(jj).TargetNumber) Then
                                m_TagetSetItems.Item(jj).UseFlg = 0
                                chgFlg2 = 1
                                Exit For
                            End If
                        Next jj
                    Next ii
                End If
            End If
            vertNum = 0
            vertNum = GetNumber_TargetString(Items.VertexTarget, arrTarget)
            If vertNum > 0 Then
                For ii = 0 To vertNum - 1
                    For jj = 0 To m_TagetSetItems.Count - 1
                        If arrTarget(ii) = CInt(m_TagetSetItems.Item(jj).TargetNumber) Then
                            If m_TagetSetItems.Item(jj).UseFlg = 0 Then
                                m_TagetSetItems.Item(jj).UseFlg = 1
                                chgFlg2 = 1
                            End If
                            Exit For
                        End If
                    Next jj
                Next ii
            End If
            If chgFlg2 = 1 Then
                Grid_Refresh()
            End If
        End If
    End Sub

    '１点円テーブル変更時の処理
    Private Sub CircleItemChanged(ByVal Items As CircleItem, ByVal iColumn As Integer)
        Dim ii, jj As Integer
        Dim vertTarget As String = ""
        Dim arrTarget(100) As Integer
        Dim vertNum As Integer = 0
        Dim chgFlg As Integer = 0

        If Items.OpeFlg <> 3 Then
            For ii = 0 To m_Circlecount - 1
                If arrCircleData(ii).SunpoID = Items.SunpoID Then
                    If arrCircleData(ii).CenterTarget <> Items.CenterTarget Then
                        vertTarget = arrCircleData(ii).CenterTarget
                        arrCircleData(ii).CenterTarget = Items.CenterTarget
                        arrCircleData(ii).OpeFlg = 1
                        chgFlg = 1
                    End If
                    If arrCircleData(ii).SunpoMark <> Items.SunpoMark Then
                        arrCircleData(ii).SunpoMark = Items.SunpoMark
                        arrCircleData(ii).OpeFlg = 1
                    End If
                    If arrCircleData(ii).SunpoName <> Items.SunpoName Then
                        arrCircleData(ii).SunpoName = Items.SunpoName
                        arrCircleData(ii).OpeFlg = 1
                    End If
                    If System.Math.Abs(CDbl(arrCircleData(ii).Diameter) - CDbl(Items.Diameter)) > 0.0001 Then
                        arrCircleData(ii).Diameter = Items.Diameter.ToString
                        arrCircleData(ii).OpeFlg = 1
                    End If
                    Dim Dimension As Integer = 0
                    Select Case Items.Dimension
                        Case "X"
                            Dimension = XYZseibun.X
                        Case "Y"
                            Dimension = XYZseibun.Y
                        Case "Z"
                            Dimension = XYZseibun.Z
                        Case "XY"
                            Dimension = XYZseibun.XY
                        Case "XZ"
                            Dimension = XYZseibun.XZ
                        Case "YZ"
                            Dimension = XYZseibun.YZ
                        Case "XYZ"
                            Dimension = XYZseibun.XYZ
                    End Select
                    If arrCircleData(ii).Dimension <> Dimension Then
                        arrCircleData(ii).Dimension = Dimension
                        arrCircleData(ii).OpeFlg = 1
                    End If
                    If arrCircleData(ii).Layer <> Items.Layer Then
                        arrCircleData(ii).Layer = Items.Layer
                        arrCircleData(ii).OpeFlg = 1
                    End If
                    Dim colorID As Integer
                    ncolor = 0
                    YCM_ReadSystemColorAcs(m_strDataSystemPath)
                    Dim mcolor As ModelColor
                    If ncolor > 0 Then
                        mcolor = YCM_GetColorInfoByName(Items.LineColor)
                        colorID = mcolor.code
                    End If
                    If arrCircleData(ii).LineColor <> colorID Then
                        arrCircleData(ii).LineColor = colorID
                        arrCircleData(ii).OpeFlg = 1
                    End If
                    Dim ltype As Integer
                    nLineType = 0
                    YCM_ReadSystemLineTypes(m_strDataSystemPath)
                    Dim mLinetype As LineType
                    If nLineType > 0 Then
                        mLinetype = YCM_GetLineTypeInfoByName(Items.LineType)
                        ltype = mLinetype.code
                    End If
                    If arrCircleData(ii).LineType <> ltype Then
                        arrCircleData(ii).LineType = ltype
                        arrCircleData(ii).OpeFlg = 1
                    End If
                    Exit For
                End If
            Next ii
        Else
            chgFlg = 1
        End If

        If chgFlg = 1 Then
            Dim chgFlg2 As Integer = 0
            If vertTarget <> "" Then
                vertNum = GetNumber_TargetString(vertTarget, arrTarget)
                If vertNum > 0 Then
                    For ii = 0 To vertNum - 1
                        For jj = 0 To m_TagetSetItems.Count - 1
                            If arrTarget(ii) = CInt(m_TagetSetItems.Item(jj).TargetNumber) Then
                                m_TagetSetItems.Item(jj).UseFlg = 0
                                chgFlg2 = 1
                                Exit For
                            End If
                        Next jj
                    Next ii
                End If
            End If
            vertNum = 0
            vertNum = GetNumber_TargetString(Items.CenterTarget, arrTarget)
            If vertNum > 0 Then
                For ii = 0 To vertNum - 1
                    For jj = 0 To m_TagetSetItems.Count - 1
                        If arrTarget(ii) = CInt(m_TagetSetItems.Item(jj).TargetNumber) Then
                            If m_TagetSetItems.Item(jj).UseFlg = 0 Then
                                m_TagetSetItems.Item(jj).UseFlg = 1
                                chgFlg2 = 1
                            End If
                            Exit For
                        End If
                    Next jj
                Next ii
            End If
            If chgFlg2 = 1 Then
                Grid_Refresh()
            End If
        End If

    End Sub

    'タブ変更イベント
    Private Sub Tab_TargetSetting_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles Tab_TargetSetting.SelectionChanged
        'If Tab_TargetSetting.SelectedItem IsNot Nothing Then
        '    Dim item As System.Windows.Controls.TabItem = Tab_TargetSetting.SelectedItem
        '    Dim ss As String = item.Name

        '    Select Case ss
        '        Case "tabTarget"
        '            delTargetItem = Nothing
        '            delTargetCulumn = -1
        '        Case "tabScale"
        '            delScaleItem = Nothing
        '            delScaleCulumn = -1
        '        Case "tabLine"
        '            delLineItem = Nothing
        '            delLineCulumn = -1
        '        Case "tabPolyLine"
        '            delPolyLineItem = Nothing
        '            delPolyLineCulumn = -1
        '        Case "tabCircle"
        '            delCircleItem = Nothing
        '            delCircleCulumn = -1
        '        Case Else

        '    End Select
        'End If
    End Sub

    'CT_Bunruiテーブルの更新
    Private Sub CT_Bunrui_Update(ByRef CToffsetD As CToffset, Optional ByRef m_dbclass As CDBOperateOLE = Nothing)

        Dim lRet As Integer
        Dim strFieldName(8) As String
        Dim strFieldText(8) As String
        Dim strWhere As String = "TypeID = " & CommonTypeID & " AND CT_NO = " & CToffsetD.CT_ID.ToString
        Dim strTableName As String = "CT_Bunrui"

        'システム設定DBへ接続
        Dim DBPath As String = My.Application.Info.DirectoryPath & "\計測システムフォルダ\" & strSystemMdbFile
        If m_dbclass Is Nothing Then
            If ConnectDB(DBPath, m_dbclass) = False Then
                MsgBox(strSystemMdbFile & "への接続に失敗しました。")
                Exit Sub
            End If
        End If

        strFieldName(0) = "CT_NO"
        strFieldName(1) = "flg_Use"
        strFieldName(2) = "m_x"
        strFieldName(3) = "m_y"
        strFieldName(4) = "m_z"
        strFieldName(5) = "Kubun1"
        strFieldName(6) = "Kubun2"
        strFieldName(7) = "Info"
        strFieldName(8) = "TypeID"

        strFieldText(0) = CToffsetD.CT_ID.ToString
        If CToffsetD.flg_Use = True Then
            strFieldText(1) = "1"
        Else
            strFieldText(1) = "0"
        End If
        strFieldText(2) = CToffsetD.offset_val.x.ToString
        strFieldText(3) = CToffsetD.offset_val.y.ToString
        strFieldText(4) = CToffsetD.offset_val.z.ToString
        strFieldText(5) = CToffsetD.kubun1.ToString
        strFieldText(6) = CToffsetD.kubun2.ToString
        strFieldText(7) = Chr(34) & CToffsetD.info & Chr(34)
        strFieldText(8) = CommonTypeID.ToString

        With m_dbclass
            .BeginTrans()
            lRet = .DoUpdate(strFieldName, strTableName, strFieldText, strWhere)
            If lRet = 1 Then
                .CommitTrans()
            Else
                .RollbackTrans()
            End If
        End With

        'システム設定DBへ切断
        DisConnectDB(m_dbclass)

    End Sub

    'CT_Bunruiテーブルの追加
    Private Sub CT_Bunrui_Insert(ByRef CToffsetD As CToffset, Optional ByRef m_dbclass As CDBOperateOLE = Nothing)

        Dim lRet As Integer
        Dim strFieldName(8) As String
        Dim strFieldText(8) As String
        Dim strTableName As String = "CT_Bunrui"

        'システム設定DBへ接続
        Dim DBPath As String = My.Application.Info.DirectoryPath & "\計測システムフォルダ\" & strSystemMdbFile
        If m_dbclass Is Nothing Then
            If ConnectDB(DBPath, m_dbclass) = False Then
                MsgBox(strSystemMdbFile & "への接続に失敗しました。")
                Exit Sub
            End If
        End If

        strFieldName(0) = "CT_NO"
        strFieldName(1) = "flg_Use"
        strFieldName(2) = "m_x"
        strFieldName(3) = "m_y"
        strFieldName(4) = "m_z"
        strFieldName(5) = "Kubun1"
        strFieldName(6) = "Kubun2"
        strFieldName(7) = "Info"
        strFieldName(8) = "TypeID"

        strFieldText(0) = CToffsetD.CT_ID.ToString
        strFieldText(1) = CToffsetD.flg_Use.ToString
        strFieldText(2) = CToffsetD.offset_val.x.ToString
        strFieldText(3) = CToffsetD.offset_val.y.ToString
        strFieldText(4) = CToffsetD.offset_val.z.ToString
        strFieldText(5) = CToffsetD.kubun1.ToString
        strFieldText(6) = CToffsetD.kubun2.ToString
        strFieldText(7) = CToffsetD.info
        strFieldText(8) = CommonTypeID.ToString

        With m_dbclass
            .BeginTrans()
            lRet = .DoInsert(strFieldName, strTableName, strFieldText)
            If lRet = 1 Then
                .CommitTrans()
            Else
                .RollbackTrans()
            End If
        End With

        'システム設定DBへ切断
        DisConnectDB(m_dbclass)

    End Sub

    'CT_Bunruiテーブルの削除
    Private Sub CT_Bunrui_Delete(ByRef CToffsetD As CToffset, Optional ByRef m_dbclass As CDBOperateOLE = Nothing)

        Dim lRet As Integer
        Dim strWhere As String = "TypeID = " & CommonTypeID & " AND CT_NO = " & CToffsetD.CT_ID.ToString
        Dim strTableName As String = "CT_Bunrui"

        'システム設定DBへ接続
        Dim DBPath As String = My.Application.Info.DirectoryPath & "\計測システムフォルダ\" & strSystemMdbFile
        If m_dbclass Is Nothing Then
            If ConnectDB(DBPath, m_dbclass) = False Then
                MsgBox(strSystemMdbFile & "への接続に失敗しました。")
                Exit Sub
            End If
        End If

        With m_dbclass
            .BeginTrans()
            lRet = .DoDelete(strTableName, strWhere)
            If lRet = 1 Then
                .CommitTrans()
            Else
                .RollbackTrans()
            End If
        End With

        'システム設定DBへ切断
        DisConnectDB(m_dbclass)

    End Sub

    'TenGunteigiテーブルの追加
    Private Sub TenGunTeigi_Insert(ByRef TengunD As TengunTeigiBunrui, Optional ByRef m_dbclass As CDBOperateOLE = Nothing)

        Dim lRet As Integer
        Dim strTableName As String = "TenGunTeigi"

        'システム設定DBへ接続
        Dim DBPath As String = My.Application.Info.DirectoryPath & "\計測システムフォルダ\" & strSystemMdbFile
        If m_dbclass Is Nothing Then
            If ConnectDB(DBPath, m_dbclass) = False Then
                MsgBox(strSystemMdbFile & "への接続に失敗しました。")
                Exit Sub
            End If
        End If

        Dim SaveTengunD As New TenGunTeigiTable

        SaveTengunD.TypeID = CommonTypeID.ToString
        SaveTengunD.TenGunID = TengunD.TengunID.ToString
        SaveTengunD.Bunrui1 = TengunD.Bunrui1.ToString
        SaveTengunD.Bunrui2 = TengunD.Bunrui2.ToString
        SaveTengunD.SyoriHouhouID = "4"
        SaveTengunD.MotoGunID = "0"
        SaveTengunD.SortingHouhou = 0
        SaveTengunD.objXYZ = 0
        SaveTengunD.BasePointID = 0
        SaveTengunD.objDaisyo = 0
        SaveTengunD.Index1 = 0
        SaveTengunD.Index2 = 0
        SaveTengunD.flgBunrui = "1"
        SaveTengunD.flgOnlyOne = "FALSE"
        SaveTengunD.strName = ""
        SaveTengunD.strBikou = ""

        CreateTengunFieldName()
        CreateTengunFieldText(SaveTengunD)

        With m_dbclass
            .BeginTrans()
            lRet = .DoInsert(strFieldNames, strTableName, strFieldTexts)
            If lRet = 1 Then
                .CommitTrans()
            Else
                .RollbackTrans()
            End If
        End With

        'システム設定DBへ切断
        DisConnectDB(m_dbclass)

    End Sub

    'TenGunTeigiテーブルの削除
    Private Sub TenGunTeigi_Delete(ByRef TengunD As TengunTeigiBunrui, Optional ByRef m_dbclass As CDBOperateOLE = Nothing)

        Dim lRet As Integer
        Dim strWhere As String = "タイプID = " & CommonTypeID & " AND 点群ID = " & TengunD.TengunID.ToString
        Dim strTableName As String = "TenGunTeigi"

        'システム設定DBへ接続
        Dim DBPath As String = My.Application.Info.DirectoryPath & "\計測システムフォルダ\" & strSystemMdbFile
        If m_dbclass Is Nothing Then
            If ConnectDB(DBPath, m_dbclass) = False Then
                MsgBox(strSystemMdbFile & "への接続に失敗しました。")
                Exit Sub
            End If
        End If

        With m_dbclass
            .BeginTrans()
            lRet = .DoDelete(strTableName, strWhere)
            If lRet = 1 Then
                .CommitTrans()
            Else
                .RollbackTrans()
            End If
        End With

        'システム設定DBへ切断
        DisConnectDB(m_dbclass)

    End Sub

    'SunpoSetテーブルの更新
    Private Sub SunpoSet_Update(ByRef SunpoD As SunpoSetTable, Optional ByRef m_dbclass As CDBOperateOLE = Nothing)

        Dim lRet As Integer
        Dim strWhere As String = "TypeID = " & CommonTypeID & " AND SunpoID = " & SunpoD.SunpoID.ToString
        Dim strTableName As String = "SunpoSet"

        'システム設定DBへ接続
        Dim DBPath As String
        'If IsNew_Work = True Then
        DBPath = My.Application.Info.DirectoryPath & "\計測システムフォルダ\" & strSystemMdbFile
        'Else
        'DBPath = m_strDataBasePath & "\計測データ.mdb"
        'End If
        If m_dbclass Is Nothing Then
            If ConnectDB(DBPath, m_dbclass) = False Then
                MsgBox(strSystemMdbFile & "への接続に失敗しました。")
                Exit Sub
            End If
        End If

        'フィールド名、フィールド値設定
        CreateSunpoField(SunpoD)

        'システム設定DB更新
        With m_dbclass
            .BeginTrans()
            lRet = .DoUpdate(strFieldNames, strTableName, strFieldTexts, strWhere)
            If lRet = 1 Then
                .CommitTrans()
            Else
                .RollbackTrans()
            End If
        End With

        'システム設定DBへ切断
        DisConnectDB(m_dbclass)

    End Sub

    'SunpoSetテーブルの追加
    Private Sub SunpoSet_Insert(ByRef SunpoD As SunpoSetTable, Optional ByRef m_dbclass As CDBOperateOLE = Nothing)

        Dim lRet As Integer
        Dim strTableName As String = "SunpoSet"

        'システム設定DBへ接続
        Dim DBPath As String
        'If IsNew_Work = True Then
        DBPath = My.Application.Info.DirectoryPath & "\計測システムフォルダ\" & strSystemMdbFile
        'Else
        'DBPath = m_strDataBasePath & "\計測データ.mdb"
        'End If
        If m_dbclass Is Nothing Then
            If ConnectDB(DBPath, m_dbclass) = False Then
                MsgBox(strSystemMdbFile & "への接続に失敗しました。")
                Exit Sub
            End If
        End If

        '追加のフィールド名、フィールド値の配列を作成する
        CreateSunpoField(SunpoD)

        'システム設定DB追加
        With m_dbclass
            .BeginTrans()
            lRet = .DoInsert(strFieldNames, strTableName, strFieldTexts)
            If lRet = 1 Then
                .CommitTrans()
            Else
                .RollbackTrans()
            End If
        End With

        'システム設定DBへ切断
        DisConnectDB(m_dbclass)

    End Sub

    'SunpoSetテーブルの削除
    Private Sub SunpoSet_Delete(ByRef SunpoD As SunpoSetTable, Optional ByRef m_dbclass As CDBOperateOLE = Nothing)

        Dim lRet As Integer
        Dim strWhere As String = "TypeID = " & CommonTypeID & " AND SunpoID = " & SunpoD.SunpoID.ToString
        Dim strTableName As String = "SunpoSet"

        'システム設定DBへ接続
        Dim DBPath As String
        'If IsNew_Work = True Then
        DBPath = My.Application.Info.DirectoryPath & "\計測システムフォルダ\" & strSystemMdbFile
        'Else
        'DBPath = m_strDataBasePath & "\計測データ.mdb"
        'End If
        If m_dbclass Is Nothing Then
            If ConnectDB(DBPath, m_dbclass) = False Then
                MsgBox(strSystemMdbFile & "への接続に失敗しました。")
                Exit Sub
            End If
        End If

        With m_dbclass
            .BeginTrans()
            lRet = .DoDelete(strTableName, strWhere)
            If lRet = 1 Then
                .CommitTrans()
            Else
                .RollbackTrans()
            End If
        End With

        'システム設定DBへ切断
        DisConnectDB(m_dbclass)

    End Sub

    'CT_CoordSettingテーブルの更新
    Private Sub CoordSet_Update(ByRef ZahyouD As ZahyouDBGet, Optional ByRef m_dbclass As CDBOperateOLE = Nothing)

        Dim lRet As Integer
        Dim strFieldName(9) As String
        Dim strFieldText(9) As String
        Dim strWhere As String = "TypeID = " & CommonTypeID
        Dim strTableName As String = "CT_CoordSetting"

        'システム設定DBへ接続
        Dim DBPath As String = My.Application.Info.DirectoryPath & "\計測システムフォルダ\" & strSystemMdbFile
        If m_dbclass Is Nothing Then
            If ConnectDB(DBPath, m_dbclass) = False Then
                MsgBox(strSystemMdbFile & "への接続に失敗しました。")
                Exit Sub
            End If
        End If

        strFieldName(0) = "CT_GenID"
        strFieldName(1) = "CT_XID"
        strFieldName(2) = "CT_YID"
        strFieldName(3) = "CT_ZID"
        strFieldName(4) = "XYorXZorYZ"
        strFieldName(5) = "CT_ID1"
        strFieldName(6) = "CT_ID2"
        strFieldName(7) = "CT_ID3"
        strFieldName(8) = "CT_Active"
        strFieldName(9) = "TypeID"

        strFieldText(0) = ZahyouD.CT_GenID.ToString
        strFieldText(1) = ZahyouD.CT_XID.ToString
        strFieldText(2) = ZahyouD.CT_YID.ToString
        strFieldText(3) = ZahyouD.CT_ZID.ToString
        strFieldText(4) = ZahyouD.XYorXZorYZ.ToString
        strFieldText(5) = ZahyouD.CT_ID1.ToString
        strFieldText(6) = ZahyouD.CT_ID2.ToString
        strFieldText(7) = ZahyouD.CT_ID3.ToString
        strFieldText(8) = ZahyouD.Active.ToString
        strFieldText(9) = CommonTypeID.ToString

        With m_dbclass
            .BeginTrans()
            lRet = .DoUpdate(strFieldName, strTableName, strFieldText, strWhere)
            If lRet = 1 Then
                .CommitTrans()
            Else
                .RollbackTrans()
            End If
        End With

        'システム設定DBへ切断
        DisConnectDB(m_dbclass)

    End Sub

    '「ターゲット」タブの追加ボタンの処理
    Private Sub Button_ADD_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_ADD.Click
        delTargetItem = Nothing
        'グリッドに行の追加
        m_TagetSetItems.Add(New TargetSetItem)
        Dim iRowno As Integer = m_TagetSetItems.Count - 1
        m_TagetSetItems.Item(iRowno).TargetNumber = CStr(maxCTno + 1)
        m_TagetSetItems.Item(iRowno).TargetOffsetX = "0.0"
        m_TagetSetItems.Item(iRowno).TargetOffsetY = "0.0"
        m_TagetSetItems.Item(iRowno).TargetOffsetZ = "0.0"
        m_TagetSetItems.Item(iRowno).OpeFlg = 3
        m_TagetSetItems.Item(iRowno).UseFlg = 0
        m_TagetSetItems.Item(iRowno).TargetInfo = ""
        m_TagetSetItems.Item(iRowno).ID = m_maxtargetID + 1

        'データセーブエリアにデータ追加
        arrCToffsetData(m_Targetcount) = New CTBunruiDB
        arrCToffsetData(m_Targetcount).CT_NO = maxCTno + 1
        arrCToffsetData(m_Targetcount).m_x = 0.0
        arrCToffsetData(m_Targetcount).m_y = 0.0
        arrCToffsetData(m_Targetcount).m_z = 0.0
        arrCToffsetData(m_Targetcount).OpeFlg = 3
        arrCToffsetData(m_Targetcount).info = "(追加)"
        arrCToffsetData(m_Targetcount).ID = m_maxtargetID + 1

        'グリッドの再表示
        Grid_Target.ItemsSource = m_TagetSetItems
        Grid_Refresh()

        Dim iii As Integer = arrCToffsetData(m_Targetcount - 1).CT_NO

        Grid_Target.Focus()
        Grid_Target.BeginEdit()
        Grid_Target.SelectedIndex = iRowno
        Grid_Target.ScrollIntoView(Grid_Target.SelectedItem)

        '各種カウンタのカウントアップ
        m_Targetcount += 1
        m_maxtargetID += 1
        maxCTno += 1
    End Sub

    '「スケール・座標」タブの追加ボタンの処理
    Private Sub Button_Kijun_ADD_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_Kijun_ADD.Click
        delScaleItem = Nothing
        'グリッドに行の追加
        m_ScaleItems.Add(New ScaleItem)
        Dim iRowno As Integer = m_ScaleItems.Count - 1
        m_ScaleItems.Item(iRowno).StartTarget = "0"
        m_ScaleItems.Item(iRowno).EndTarget = "0"
        m_ScaleItems.Item(iRowno).SunpoMark = "S00"
        m_ScaleItems.Item(iRowno).SunpoName = "(追加)"
        m_ScaleItems.Item(iRowno).KiteiVal = "1000.0"
        m_ScaleItems.Item(iRowno).flgScale = 0
        m_ScaleItems.Item(iRowno).OpeFlg = 3
        m_ScaleItems.Item(iRowno).SunpoID = MaxSunpoID + 1

        'グリッドの再表示
        Grid_Kijun.ItemsSource = m_ScaleItems
        Grid_Kijun.Items.Refresh()

        Grid_Kijun.Focus()
        Grid_Kijun.BeginEdit()
        Grid_Kijun.SelectedIndex = iRowno
        Grid_Kijun.ScrollIntoView(Grid_Kijun.SelectedItem)

        '各種カウンタのカウントアップ
        'm_Scalecount += 1
        MaxSunpoID += 1

        MessageBox.Show("追加した「スケール」要素は、始点TG番号と終点TG番号を必ず設定してください。", "スケールの追加", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    '「２点線分・長さ」タブの追加ボタンの処理
    Private Sub Button_Line_ADD_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_Line_ADD.Click
        delLineItem = Nothing
        'グリッドに行の追加
        m_LineLengthItems.Add(New LineLengthItem)
        Dim iRowno As Integer = m_LineLengthItems.Count - 1
        m_LineLengthItems.Item(iRowno).StartTarget = "0"
        m_LineLengthItems.Item(iRowno).EndTarget = "0"
        m_LineLengthItems.Item(iRowno).Dimension = "XYZ"
        m_LineLengthItems.Item(iRowno).LineColor = "MAGENTA"
        m_LineLengthItems.Item(iRowno).LineType = "CONTINUOUS"
        m_LineLengthItems.Item(iRowno).Layer = "0"
        m_LineLengthItems.Item(iRowno).SunpoMark = "L00"
        m_LineLengthItems.Item(iRowno).SunpoName = "(追加)"
        m_LineLengthItems.Item(iRowno).OpeFlg = 3
        m_LineLengthItems.Item(iRowno).SunpoID = MaxSunpoID + 1

        'グリッドの再表示
        Grid_Line.ItemsSource = m_LineLengthItems
        Grid_Line.Items.Refresh()

        Grid_Line.Focus()
        Grid_Line.BeginEdit()
        Grid_Line.SelectedIndex = iRowno
        Grid_Line.ScrollIntoView(Grid_Line.SelectedItem)

        '各種カウンタのカウントアップ
        'm_LIneLengthcount += 1
        MaxSunpoID += 1

        MessageBox.Show("追加した「２点による線分・長さ」要素は、始点TG番号と終点TG番号を必ず設定してください。", "２点線分の追加", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    '「ポリライン」タブの追加ボタンの処理
    Private Sub Button_PolyLine_ADD_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_PolyLine_ADD.Click
        delPolyLineItem = Nothing
        'グリッドに行の追加
        m_PolyLineItems.Add(New PolyLineItem)
        Dim iRowno As Integer = m_PolyLineItems.Count - 1
        m_PolyLineItems.Item(iRowno).VertexTarget = ""
        m_PolyLineItems.Item(iRowno).Dimension = "XYZ"
        m_PolyLineItems.Item(iRowno).LineColor = "MAGENTA"
        m_PolyLineItems.Item(iRowno).LineType = "CONTINUOUS"
        m_PolyLineItems.Item(iRowno).Layer = "0"
        m_PolyLineItems.Item(iRowno).SunpoMark = "PL00"
        m_PolyLineItems.Item(iRowno).SunpoName = "(追加)"
        m_PolyLineItems.Item(iRowno).OpeFlg = 3
        m_PolyLineItems.Item(iRowno).SunpoID = MaxSunpoID + 1

        'グリッドの再表示
        Grid_PolyLine.ItemsSource = m_PolyLineItems
        Grid_PolyLine.Items.Refresh()

        Grid_PolyLine.Focus()
        Grid_PolyLine.BeginEdit()
        Grid_PolyLine.SelectedIndex = iRowno
        Grid_PolyLine.ScrollIntoView(Grid_PolyLine.SelectedItem)

        '各種カウンタのカウントアップ
        'm_PolyLinecount += 1
        MaxSunpoID += 1

        MessageBox.Show("追加した「ポリライン」要素は、頂点TG番号リストを必ず設定してください。", "ポリラインの追加", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    '「１点円」タブの追加ボタンの処理
    Private Sub Button_Circle_ADD_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_Circle_ADD.Click
        delCircleItem = Nothing
        'グリッドに行の追加
        m_CircleItems.Add(New CircleItem)
        Dim iRowno As Integer = m_CircleItems.Count - 1
        m_CircleItems.Item(iRowno).CenterTarget = ""
        m_CircleItems.Item(iRowno).Diameter = "10.0"
        m_CircleItems.Item(iRowno).Dimension = "XYZ"
        m_CircleItems.Item(iRowno).LineColor = "MAGENTA"
        m_CircleItems.Item(iRowno).LineType = "CONTINUOUS"
        m_CircleItems.Item(iRowno).Layer = "0"
        m_CircleItems.Item(iRowno).SunpoMark = "C00"
        m_CircleItems.Item(iRowno).SunpoName = "(追加)"
        m_CircleItems.Item(iRowno).OpeFlg = 3
        m_CircleItems.Item(iRowno).SunpoID = MaxSunpoID + 1

        'グリッドの再表示
        Grid_Circle.ItemsSource = m_CircleItems
        Grid_Circle.Items.Refresh()

        Grid_Circle.Focus()
        Grid_Circle.BeginEdit()
        Grid_Circle.SelectedIndex = iRowno
        Grid_Circle.ScrollIntoView(Grid_Circle.SelectedItem)

        '各種カウンタのカウントアップ
        'm_Circlecount += 1
        MaxSunpoID += 1

        MessageBox.Show("追加した「１点による円」要素は、中心点TG番号リストを必ず設定してください。", "１点円の追加", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    '「ターゲット」タブの削除ボタンの処理
    Private Sub Button_DEL_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_DEL.Click

        Dim ii As Integer
        Dim delItem As TargetSetItem
        Dim strMsg As String = ""

        If delTargetItem IsNot Nothing Then
            If delTargetCulumn > 0 Then
                delItem = delTargetItem
            Else
                MessageBox.Show("行を選択して削除ボタンを押してください", "ターゲットの削除", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                delTargetCulumn = -1
                delTargetItem = Nothing
                Exit Sub
            End If
        Else
            If Grid_Target.CurrentItem IsNot Nothing Then
                delItem = Grid_Target.CurrentItem
            Else
                MessageBox.Show("行を選択して削除ボタンを押してください", "ターゲットの削除", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                delTargetCulumn = -1
                delTargetItem = Nothing
                Exit Sub
            End If
        End If

        If delItem.UseFlg = 1 Then
            strMsg = "コードターゲット（CT番号＝" & delItem.TargetNumber & "）は寸法要素で使用されています。" & vbCrLf & "削除してよろしいですか？"
        Else
            strMsg = "コードターゲット（CT番号＝" & delItem.TargetNumber & "）を削除します。よろしいですか？"
        End If
        If MessageBox.Show(strMsg, "ターゲットの削除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
            delTargetCulumn = -1
            delTargetItem = Nothing
            Exit Sub
        End If

        If delItem.OpeFlg = 3 Then
            For ii = 0 To m_TagetSetItems.Count - 1
                If m_TagetSetItems.Item(ii).TargetNumber = delItem.TargetNumber Then
                    m_TagetSetItems.RemoveAt(ii)
                    Exit For
                End If
            Next ii
        Else
            Dim delID As Integer = delItem.ID
            Dim delFlg As Integer = 0
            For ii = 0 To m_Targetcount
                If arrCToffsetData(ii).ID = delID Then
                    arrCToffsetData(ii).OpeFlg = 2
                    delFlg = 1
                    Exit For
                End If
            Next ii
            If delFlg = 1 Then
                For ii = 0 To m_TagetSetItems.Count - 1
                    If m_TagetSetItems.Item(ii).ID = delItem.ID Then
                        m_TagetSetItems.RemoveAt(ii)
                        Exit For
                    End If
                Next ii
            End If
        End If
        Grid_Refresh()

        delTargetItem = Nothing
        delTargetCulumn = -1
    End Sub

    '「スケール・座標」タブの削除ボタンの処理
    Private Sub Button_Kijun_DEL_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_Kijun_DEL.Click
        Dim ii As Integer
        Dim delItem As ScaleItem
        Dim startTarget As Integer = -1
        Dim endTarget As Integer = -1

        If delScaleItem IsNot Nothing Then
            If delScaleCulumn > 0 Then
                delItem = delScaleItem
                If MessageBox.Show("スケール（" & delItem.SunpoMark & "）を削除します。よろしいですか？", "スケールの削除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                    delScaleItem = Nothing
                    delScaleCulumn = -1
                    Exit Sub
                End If
            Else
                MessageBox.Show("行を選択して削除ボタンを押してください", "スケールの削除", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                delScaleItem = Nothing
                delScaleCulumn = -1
                Exit Sub
            End If
        Else
            If Grid_Kijun.CurrentItem IsNot Nothing Then
                delItem = Grid_Kijun.CurrentItem
                If MessageBox.Show("スケール（" & delItem.SunpoMark & "）を削除します。よろしいですか？", "スケールの削除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                    delScaleItem = Nothing
                    delScaleCulumn = -1
                    Exit Sub
                End If
            Else
                MessageBox.Show("行を選択して削除ボタンを押してください", "スケールの削除", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
        End If

        If delItem.OpeFlg = 3 Then
            For ii = 0 To m_ScaleItems.Count - 1
                If m_ScaleItems.Item(ii).SunpoID = delItem.SunpoID Then
                    startTarget = CInt(m_ScaleItems.Item(ii).StartTarget)
                    endTarget = CInt(m_ScaleItems.Item(ii).EndTarget)
                    m_ScaleItems.RemoveAt(ii)
                    Grid_Kijun.Items.Refresh()
                    Exit For
                End If
            Next ii
        Else
            For ii = 0 To m_SunpoCount - 1
                If arrSunpoSetData(ii).SunpoID = delItem.SunpoID Then
                    arrSunpoSetData(ii).OpeFlg = 2
                    Exit For
                End If
            Next ii
            For ii = 0 To m_Scalecount - 1
                If arrScaleData(ii).SunpoID = delItem.SunpoID Then
                    arrScaleData(ii).OpeFlg = 2
                    Exit For
                End If
            Next ii
            For ii = 0 To m_ScaleItems.Count - 1
                If m_ScaleItems.Item(ii).SunpoID = delItem.SunpoID Then
                    startTarget = CInt(m_ScaleItems.Item(ii).StartTarget)
                    endTarget = CInt(m_ScaleItems.Item(ii).EndTarget)
                    m_ScaleItems.RemoveAt(ii)
                    Grid_Kijun.Items.Refresh()
                    Exit For
                End If
            Next ii
        End If

        'ターゲット使用フラグの更新
        If (startTarget >= 0) And (endTarget >= 0) Then
            For ii = 0 To m_TagetSetItems.Count - 1
                If (CInt(m_TagetSetItems.Item(ii).TargetNumber) = startTarget) Or (CInt(m_TagetSetItems.Item(ii).TargetNumber) = endTarget) Then
                    m_TagetSetItems.Item(ii).UseFlg = 0
                End If
            Next ii
        End If
        Grid_Refresh()

        delScaleItem = Nothing
        delScaleCulumn = -1

    End Sub

    '「２点線分」タブの削除ボタンの処理
    Private Sub Button_Line_DEL_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_Line_DEL.Click
        Dim ii As Integer
        Dim delItem As LineLengthItem
        Dim startTarget As Integer = -1
        Dim endTarget As Integer = -1

        If delLineItem IsNot Nothing Then
            If delLineCulumn > 0 Then
                delItem = delLineItem
                If MessageBox.Show("２点線分（" & delItem.SunpoMark & "）を削除します。よろしいですか？", "２点線分の削除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                    delLineItem = Nothing
                    delLineCulumn = -1
                    Exit Sub
                End If
            Else
                MessageBox.Show("行を選択して削除ボタンを押してください", "２点線分・長さの削除", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                delLineItem = Nothing
                delLineCulumn = -1
                Exit Sub
            End If
        Else
            If Grid_Line.CurrentItem IsNot Nothing Then
                delItem = Grid_Line.CurrentItem
                If MessageBox.Show("２点線分（" & delItem.SunpoMark & "）を削除します。よろしいですか？", "２点線分の削除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                    delLineItem = Nothing
                    delLineCulumn = -1
                    Exit Sub
                End If
            Else
                MessageBox.Show("行を選択して削除ボタンを押してください", "２点線分・長さの削除", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
        End If

        If delItem.OpeFlg = 3 Then
            For ii = 0 To m_LineLengthItems.Count - 1
                If m_LineLengthItems.Item(ii).SunpoID = delItem.SunpoID Then
                    startTarget = CInt(m_LineLengthItems.Item(ii).StartTarget)
                    endTarget = CInt(m_LineLengthItems.Item(ii).EndTarget)
                    m_LineLengthItems.RemoveAt(ii)
                    Grid_Line.Items.Refresh()
                    Exit For
                End If
            Next ii
        Else
            For ii = 0 To m_SunpoCount - 1
                If arrSunpoSetData(ii).SunpoID = delItem.SunpoID Then
                    arrSunpoSetData(ii).OpeFlg = 2
                    Exit For
                End If
            Next ii
            For ii = 0 To m_LIneLengthcount - 1
                If arrLineLegthData(ii).SunpoID = delItem.SunpoID Then
                    arrLineLegthData(ii).OpeFlg = 2
                    Exit For
                End If
            Next ii
            For ii = 0 To m_LineLengthItems.Count - 1
                If m_LineLengthItems.Item(ii).SunpoID = delItem.SunpoID Then
                    startTarget = CInt(m_LineLengthItems.Item(ii).StartTarget)
                    endTarget = CInt(m_LineLengthItems.Item(ii).EndTarget)
                    m_LineLengthItems.RemoveAt(ii)
                    Grid_Line.Items.Refresh()
                    Exit For
                End If
            Next ii
        End If

        'ターゲット使用フラグの更新
        If (startTarget >= 0) And (endTarget >= 0) Then
            For ii = 0 To m_TagetSetItems.Count - 1
                If (CInt(m_TagetSetItems.Item(ii).TargetNumber) = startTarget) Or (CInt(m_TagetSetItems.Item(ii).TargetNumber) = endTarget) Then
                    m_TagetSetItems.Item(ii).UseFlg = 0
                End If
            Next ii
        End If
        Grid_Refresh()

        delLineItem = Nothing
        delLineCulumn = -1

    End Sub

    '「ポリライン」タブの削除ボタンの処理
    Private Sub Button_PolyLine_DEL_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_PolyLine_DEL.Click
        Dim ii, jj As Integer
        Dim delItem As PolyLineItem
        Dim vertTarget As String = ""
        Dim vertNum As Integer = 0
        Dim arrTarget(100) As Integer

        If delPolyLineItem IsNot Nothing Then
            If delPolyLineCulumn > 0 Then
                delItem = delPolyLineItem
                If MessageBox.Show("ポリライン（" & delItem.SunpoMark & "）を削除します。よろしいですか？", "ポリラインの削除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                    delPolyLineItem = Nothing
                    delPolyLineCulumn = -1
                    Exit Sub
                End If
            Else
                MessageBox.Show("行を選択して削除ボタンを押してください", "ポリラインの削除", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                delPolyLineItem = Nothing
                delPolyLineCulumn = -1
                Exit Sub
            End If
        Else
            If Grid_PolyLine.CurrentItem IsNot Nothing Then
                delItem = Grid_PolyLine.CurrentItem
                If MessageBox.Show("ポリライン（" & delItem.SunpoMark & "）を削除します。よろしいですか？", "ポリラインの削除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                    delPolyLineItem = Nothing
                    delPolyLineCulumn = -1
                    Exit Sub
                End If
            Else
                MessageBox.Show("行を選択して削除ボタンを押してください", "ポリラインの削除", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
        End If

        If delItem.OpeFlg = 3 Then
            For ii = 0 To m_PolyLineItems.Count - 1
                If m_PolyLineItems.Item(ii).SunpoID = delItem.SunpoID Then
                    vertTarget = m_PolyLineItems.Item(ii).VertexTarget
                    m_PolyLineItems.RemoveAt(ii)
                    Grid_PolyLine.Items.Refresh()
                    Exit For
                End If
            Next ii
        Else
            For ii = 0 To m_SunpoCount - 1
                If arrSunpoSetData(ii).SunpoID = delItem.SunpoID Then
                    arrSunpoSetData(ii).OpeFlg = 2
                    Exit For
                End If
            Next ii
            For ii = 0 To m_PolyLinecount - 1
                If arrPolyLineData(ii).SunpoID = delItem.SunpoID Then
                    arrPolyLineData(ii).OpeFlg = 2
                    Exit For
                End If
            Next ii
            For ii = 0 To m_PolyLineItems.Count - 1
                If m_PolyLineItems.Item(ii).SunpoID = delItem.SunpoID Then
                    vertTarget = m_PolyLineItems.Item(ii).VertexTarget
                    m_PolyLineItems.RemoveAt(ii)
                    Grid_PolyLine.Items.Refresh()
                    Exit For
                End If
            Next ii
        End If

        'ターゲット使用フラグの更新
        If vertTarget <> "" Then
            vertNum = GetNumber_TargetString(vertTarget, arrTarget)
            If vertNum > 0 Then
                For ii = 0 To vertNum - 1
                    For jj = 0 To m_TagetSetItems.Count - 1
                        If CInt(m_TagetSetItems.Item(jj).TargetNumber) = arrTarget(ii) Then
                            m_TagetSetItems.Item(jj).UseFlg = 0
                        End If
                    Next jj
                Next ii
            End If
        End If
        Grid_Refresh()

        delPolyLineItem = Nothing
        delPolyLineCulumn = -1

    End Sub

    '「１点円」タブの削除ボタンの処理
    Private Sub Button_Circle_DEL_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_Circle_DEL.Click
        Dim ii, jj As Integer
        Dim delItem As CircleItem
        Dim centerTarget As String = ""
        Dim centerNum As Integer = 0
        Dim arrTarget(100) As Integer

        If delCircleItem IsNot Nothing Then
            If delCircleCulumn > 0 Then
                delItem = delCircleItem
                If MessageBox.Show("１点円（" & delItem.SunpoMark & "）を削除します。よろしいですか？", "１点円の削除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                    delCircleItem = Nothing
                    delCircleCulumn = -1
                    Exit Sub
                End If
            Else
                MessageBox.Show("行を選択して削除ボタンを押してください", "１点円の削除", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                delCircleItem = Nothing
                delCircleCulumn = -1
                Exit Sub
            End If
        Else
            If Grid_Circle.CurrentItem IsNot Nothing Then
                delItem = Grid_Circle.CurrentItem
                If MessageBox.Show("１点円（" & delItem.SunpoMark & "）を削除します。よろしいですか？", "１点円の削除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                    delCircleItem = Nothing
                    delCircleCulumn = -1
                    Exit Sub
                End If
            Else
                MessageBox.Show("行を選択して削除ボタンを押してください", "１点円の削除", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
        End If

        If delItem.OpeFlg = 3 Then
            For ii = 0 To m_CircleItems.Count - 1
                If m_CircleItems.Item(ii).SunpoID = delItem.SunpoID Then
                    centerTarget = m_CircleItems.Item(ii).CenterTarget
                    m_CircleItems.RemoveAt(ii)
                    Grid_Circle.Items.Refresh()
                    Exit For
                End If
            Next ii
        Else
            For ii = 0 To m_SunpoCount - 1
                If arrSunpoSetData(ii).SunpoID = delItem.SunpoID Then
                    arrSunpoSetData(ii).OpeFlg = 2
                    Exit For
                End If
            Next ii
            For ii = 0 To m_Circlecount - 1
                If arrCircleData(ii).SunpoID = delItem.SunpoID Then
                    arrCircleData(ii).OpeFlg = 2
                    Exit For
                End If
            Next ii
            For ii = 0 To m_CircleItems.Count - 1
                If m_CircleItems.Item(ii).SunpoID = delItem.SunpoID Then
                    centerTarget = m_CircleItems.Item(ii).CenterTarget
                    m_CircleItems.RemoveAt(ii)
                    Grid_Circle.Items.Refresh()
                    Exit For
                End If
            Next ii
        End If

        'ターゲット使用フラグの更新
        If centerTarget <> "" Then
            centerNum = GetNumber_TargetString(centerTarget, arrTarget)
            If centerNum > 0 Then
                For ii = 0 To centerNum - 1
                    For jj = 0 To m_TagetSetItems.Count - 1
                        If CInt(m_TagetSetItems.Item(jj).TargetNumber) = arrTarget(ii) Then
                            m_TagetSetItems.Item(jj).UseFlg = 0
                        End If
                    Next jj
                Next ii
            End If
        End If
        Grid_Refresh()

        delCircleItem = Nothing
        delCircleCulumn = -1

    End Sub

    '「更新」ボタンクリック
    Private Sub Button_OK_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_OK.Click
        Dim i, j As Integer
        Dim iSflg As Integer
        Dim ist As Integer
        Dim ErrMsg As String = ""
        Dim iCnt As Integer

        gfrmProgressBar = New frmProgressBar
        gfrmProgressBar.Show()
        gfrmProgressBar.ProgressBar1.Maximum = 100
        gfrmProgressBar.ProgressBar1.Value = 0
        gfrmProgressBar.Label1.Text = "データベース更新中"
        gfrmProgressBar.Refresh()

        'D/B更新前のエラーチェック
        'ターゲット登録チェック
        If m_TagetSetItems.Count <= 0 Then
            MsgBox("ターゲットが一つも登録されていません。ターゲットを追加して下さい。", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "D/B更新エラー")
            gfrmProgressBar.Close()
            Exit Sub
        End If

        '基準スケール設定チェック
        iCnt = 0
        For i = 0 To m_ScaleItems.Count - 1
            If m_ScaleItems(i).flgScale = 1 Then
                iCnt = 1
                Exit For
            End If
        Next i
        If iCnt = 0 Then
            MsgBox("基準スケールが登録されていません。基準スケールを追加して下さい。", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "D/B更新エラー")
            gfrmProgressBar.Close()
            Exit Sub
        End If

        'スケールの始点TG番号、終点TG番号のチェック
        For i = 0 To m_ScaleItems.Count - 1
            If (m_ScaleItems(i).OpeFlg = 3) And ((CInt(m_ScaleItems(i).StartTarget) <= 0) Or (CInt(m_ScaleItems(i).EndTarget) <= 0)) Then
                MsgBox("追加されたスケールのターゲット番号が不正です。", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "D/B更新エラー")
                gfrmProgressBar.Close()
                Exit Sub
            End If
        Next i

        '２点線分の始点TG番号、終点TG番号のチェック
        For i = 0 To m_LineLengthItems.Count - 1
            If (m_LineLengthItems(i).OpeFlg = 3) And ((CInt(m_LineLengthItems(i).StartTarget) <= 0) Or (CInt(m_LineLengthItems(i).EndTarget) <= 0)) Then
                MsgBox("追加された「２点による線分・長さ」要素のターゲット番号が不正です。", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "D/B更新エラー")
                gfrmProgressBar.Close()
                Exit Sub
            End If
        Next i

        'ポリラインの頂点TG番号リストのチェック
        For i = 0 To m_PolyLineItems.Count - 1
            If (m_PolyLineItems(i).OpeFlg = 3) And (Trim(m_PolyLineItems(i).VertexTarget) = "") Then
                MsgBox("追加されたポリライン要素の頂点TG番号リストが不正です。", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "D/B更新エラー")
                gfrmProgressBar.Close()
                Exit Sub
            End If
        Next i

        '１点円の中心点TG番号リストのチェック
        For i = 0 To m_CircleItems.Count - 1
            If (m_CircleItems(i).OpeFlg = 3) And (Trim(m_CircleItems(i).CenterTarget) = "") Then
                MsgBox("追加された「１点による円」要素の中心点TG番号リストが不正です。", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "D/B更新エラー")
                gfrmProgressBar.Close()
                Exit Sub
            End If
        Next i

        'スケールターゲットの重複チェック
        For i = 0 To m_ScaleItems.Count - 1
            Dim strMsg As String = ""
            '座標系ターゲットとの重複チェック
            ist = ZhyouItem_Exist(CInt(m_ScaleItems(i).StartTarget))
            If ist = 1 Then
                If strMsg.Length = 0 Then
                    strMsg = "ターゲット " & m_ScaleItems(i).StartTarget & " 番は[スケール]、[座標系]"
                Else
                    strMsg = strMsg & "、[座標系]"
                End If
            Else
                ist = ZhyouItem_Exist(CInt(m_ScaleItems(i).EndTarget))
                If ist = 1 Then
                    If strMsg.Length = 0 Then
                        strMsg = "ターゲット " & m_ScaleItems(i).EndTarget & " 番は[スケール]、[座標系]"
                    Else
                        strMsg = strMsg & "、[座標系]"
                    End If
                End If
            End If
            'ポリラインターゲットとの重複チェック
            ist = PolyLineItem_Exist(CInt(m_ScaleItems(i).StartTarget))
            If ist = 1 Then
                If strMsg.Length = 0 Then
                    strMsg = "ターゲット " & m_ScaleItems(i).StartTarget & " 番は[スケール]、[ポリライン]"
                Else
                    strMsg = strMsg & "、[ポリライン]"
                End If
            Else
                ist = PolyLineItem_Exist(CInt(m_ScaleItems(i).EndTarget))
                If ist = 1 Then
                    If strMsg.Length = 0 Then
                        strMsg = "ターゲット " & m_ScaleItems(i).EndTarget & " 番は[スケール]、[ポリライン]"
                    Else
                        strMsg = strMsg & "、[ポリライン]"
                    End If
                End If
            End If
            '１点円ターゲットとの重複チェック
            ist = CircleItem_Exist(CInt(m_ScaleItems(i).StartTarget))
            If ist = 1 Then
                If strMsg.Length = 0 Then
                    strMsg = "ターゲット " & m_ScaleItems(i).StartTarget & " 番は[スケール]、[１点円]"
                Else
                    strMsg = strMsg & "、[１点円]"
                End If
            Else
                ist = CircleItem_Exist(CInt(m_ScaleItems(i).EndTarget))
                If ist = 1 Then
                    If strMsg.Length = 0 Then
                        strMsg = "ターゲット " & m_ScaleItems(i).EndTarget & " 番は[スケール]、[１点円]"
                    Else
                        strMsg = strMsg & "、[１点円]"
                    End If
                End If
            End If
            If strMsg.Length > 0 Then
                strMsg = strMsg & "で重複して使用されています。"
                If ErrMsg.Length > 0 Then
                    ErrMsg = ErrMsg & vbCrLf & strMsg
                Else
                    ErrMsg = strMsg
                End If
            End If
        Next i

        '座標系ターゲットの重複チェック（スケールとのチェックは上で実施しているため、ここでは行わない）
        For i = 0 To m_ZahyouItems.Count - 1
            Dim strMsg As String = ""

            'スケールのチェックで重複ありとなった場合は、メッセージが重複するので処理を飛ばす。
            ist = ScaleItem_Exist(CInt(m_ZahyouItems(i).CT_No))
            If ist = 1 Then
                Continue For
            End If

            'ポリラインターゲットとの重複チェック
            ist = PolyLineItem_Exist(CInt(m_ZahyouItems(i).CT_No))
            If ist = 1 Then
                If strMsg.Length = 0 Then
                    strMsg = "ターゲット " & m_ZahyouItems(i).CT_No & " 番は[座標系]、[ポリライン]"
                Else
                    strMsg = strMsg & "、[ポリライン]"
                End If
            End If
            '１点円ターゲットとの重複チェック
            ist = CircleItem_Exist(CInt(m_ZahyouItems(i).CT_No))
            If ist = 1 Then
                If strMsg.Length = 0 Then
                    strMsg = "ターゲット " & m_ZahyouItems(i).CT_No & " 番は[座標系]、[１点円]"
                Else
                    strMsg = strMsg & "、[１点円]"
                End If
            End If
            If strMsg.Length > 0 Then
                strMsg = strMsg & "で重複して使用されています。"
                If ErrMsg.Length > 0 Then
                    ErrMsg = ErrMsg & vbCrLf & strMsg
                Else
                    ErrMsg = strMsg
                End If
            End If
        Next i

        '２点線分ターゲットの重複チェック
        For i = 0 To m_LineLengthItems.Count - 1
            Dim strMsg As String = ""
            'ポリラインターゲットとの重複チェック
            ist = PolyLineItem_Exist(CInt(m_LineLengthItems(i).StartTarget))
            If ist = 1 Then
                If strMsg.Length = 0 Then
                    strMsg = "ターゲット " & m_LineLengthItems(i).StartTarget & " 番は[２点による線分]、[ポリライン]"
                Else
                    strMsg = strMsg & "、[ポリライン]"
                End If
            Else
                ist = PolyLineItem_Exist(CInt(m_LineLengthItems(i).EndTarget))
                If ist = 1 Then
                    If strMsg.Length = 0 Then
                        strMsg = "ターゲット " & m_LineLengthItems(i).EndTarget & " 番は[２点による線分]、[ポリライン]"
                    Else
                        strMsg = strMsg & "、[ポリライン]"
                    End If
                End If
            End If
            '１点円ターゲットとの重複チェック
            ist = CircleItem_Exist(CInt(m_LineLengthItems(i).StartTarget))
            If ist = 1 Then
                If strMsg.Length = 0 Then
                    strMsg = "ターゲット " & m_LineLengthItems(i).StartTarget & " 番は[２点による線分]、[１点円]"
                Else
                    strMsg = strMsg & "、[１点円]"
                End If
            Else
                ist = CircleItem_Exist(CInt(m_LineLengthItems(i).EndTarget))
                If ist = 1 Then
                    If strMsg.Length = 0 Then
                        strMsg = "ターゲット " & m_LineLengthItems(i).EndTarget & " 番は[２点による線分]、[１点円]"
                    Else
                        strMsg = strMsg & "、[１点円]"
                    End If
                End If
            End If
            If strMsg.Length > 0 Then
                strMsg = strMsg & "で重複して使用されています。"
                If ErrMsg.Length > 0 Then
                    ErrMsg = ErrMsg & vbCrLf & strMsg
                Else
                    ErrMsg = strMsg
                End If
            End If
        Next i

        'ポリラインターゲットの重複チェック（１点円とのチェックのみ行う)
        For i = 0 To m_PolyLineItems.Count - 1
            Dim arrTarget(100) As Integer
            Dim iCnt2 As Integer = GetNumber_TargetString(m_PolyLineItems(i).VertexTarget, arrTarget)
            For j = 0 To iCnt2 - 1
                Dim strMsg As String = ""

                'スケールのチェックで重複ありとなった場合は、メッセージが重複するので処理を飛ばす。
                ist = ScaleItem_Exist(arrTarget(j))
                If ist = 1 Then
                    Continue For
                End If
                '座標系のチェックで重複ありとなった場合は、メッセージが重複するので処理を飛ばす。
                ist = ZhyouItem_Exist(arrTarget(j))
                If ist = 1 Then
                    Continue For
                End If
                '２点線分のチェックで重複ありとなった場合は、メッセージが重複するので処理を飛ばす。
                ist = LineLengthItem_Exist(arrTarget(j))
                If ist = 1 Then
                    Continue For
                End If

                '１点円ターゲットとの重複チェック
                ist = CircleItem_Exist(arrTarget(j))
                If ist = 1 Then
                    If strMsg.Length = 0 Then
                        strMsg = "ターゲット " & arrTarget(j) & " 番は[ポリライン]、[１点円]"
                    Else
                        strMsg = strMsg & "、[１点円]"
                    End If
                End If
                If strMsg.Length > 0 Then
                    strMsg = strMsg & "で重複して使用されています。"
                    If ErrMsg.Length > 0 Then
                        ErrMsg = ErrMsg & vbCrLf & strMsg
                    Else
                        ErrMsg = strMsg
                    End If
                End If
            Next j
        Next i

        If ErrMsg.Length > 0 Then
            Dim ErrorMessage As String = ""
            ErrorMessage = "[以下のエラーがあるため、更新処理をキャンセルします。]" & vbCrLf & ErrMsg
            MsgBox(ErrorMessage, MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "D/B更新エラー")
            gfrmProgressBar.Close()
            Exit Sub
        End If

        'CT_Bunruiテーブルの更新、削除
        Dim CToffsetD As New CToffset
        For i = 0 To m_Targetcount - 1
            If arrCToffsetData(i).OpeFlg > 0 Then
                CToffsetD.CT_ID = arrCToffsetData(i).CT_NO
                CToffsetD.flg_Use = True
                CToffsetD.offset_val.x = arrCToffsetData(i).m_x
                CToffsetD.offset_val.y = arrCToffsetData(i).m_y
                CToffsetD.offset_val.z = arrCToffsetData(i).m_z
                CToffsetD.kubun1 = arrCToffsetData(i).kubun1
                CToffsetD.kubun2 = arrCToffsetData(i).kubun2
                CToffsetD.info = arrCToffsetData(i).info

                Select Case arrCToffsetData(i).OpeFlg
                    Case 1          '更新
                        CT_Bunrui_Update(CToffsetD)
                    Case 2          '削除
                        CT_Bunrui_Delete(CToffsetD)
                End Select
            End If
        Next i
        gfrmProgressBar.ProgressBar1.Value = 5
        gfrmProgressBar.Refresh()

        'CT_Bunruiテーブルの追加
        Dim max_kubun As Integer = Get_MaxKubun1()
        Dim max_TengunID As Integer = Get_MaxTengunID()
        Dim TengunD As New TengunTeigiBunrui
        For i = 0 To m_TagetSetItems.Count - 1
            If m_TagetSetItems.Item(i).OpeFlg = 3 Then
                'CT_Bunruiテーブルの値の設定
                CToffsetD.CT_ID = CInt(m_TagetSetItems.Item(i).TargetNumber)
                CToffsetD.flg_Use = True
                CToffsetD.offset_val.x = CDbl(m_TagetSetItems.Item(i).TargetOffsetX)
                CToffsetD.offset_val.y = CDbl(m_TagetSetItems.Item(i).TargetOffsetY)
                CToffsetD.offset_val.z = CDbl(m_TagetSetItems.Item(i).TargetOffsetZ)
                CToffsetD.kubun1 = max_kubun + 1
                CToffsetD.kubun2 = 0
                CToffsetD.info = m_TagetSetItems.Item(i).TargetInfo
                'CT_Bunruiテーブル追加
                CT_Bunrui_Insert(CToffsetD)

                '最大区分コードカウントアップ
                max_kubun += 1
            End If
        Next i
        gfrmProgressBar.ProgressBar1.Value = 10
        gfrmProgressBar.Refresh()

        'CT_Bunruiテーブルの再読み込み
        m_Targetcount = 0
        Dim TargetNum As Integer = Read_CTBunruiDB()
        gfrmProgressBar.ProgressBar1.Value = 15
        gfrmProgressBar.Refresh()

        '基準スケール重複チェック(重複している場合、最初に見つかったスケールを基準スケールとする）
        iSflg = 0
        For i = 0 To m_Scalecount - 1
            If (arrScaleData(i).OpeFlg <> 2) And (arrScaleData(i).flgScale = 1) Then
                If iSflg = 0 Then
                    iSflg = 1
                Else
                    arrScaleData(i).flgScale = 0
                    If arrScaleData(i).OpeFlg = 0 Then
                        arrScaleData(i).OpeFlg = 1
                    End If
                End If
            End If
        Next i
        iSflg = 0
        For i = 0 To m_ScaleItems.Count - 1
            If (m_ScaleItems.Item(i).OpeFlg <> 2) And (m_ScaleItems.Item(i).flgScale = 1) Then
                If iSflg = 0 Then
                    iSflg = 1
                Else
                    m_ScaleItems.Item(i).flgScale = 0
                End If
            End If
        Next i
        'スケールの更新
        For i = 0 To m_Scalecount - 1
            If arrScaleData(i).OpeFlg = 1 Then
                Dim ScaleD As New ScaleItem
                ScaleD = arrScaleData(i)
                Update_Scale(ScaleD)
            End If
        Next i
        gfrmProgressBar.ProgressBar1.Value = 20
        gfrmProgressBar.Refresh()

        '座標系の更新
        If arrZahyouData.OpeFlg = 1 Then
            Update_Zahyou(arrZahyouData)
        End If
        gfrmProgressBar.ProgressBar1.Value = 30
        gfrmProgressBar.Refresh()

        '２点線分・長さの更新
        For i = 0 To m_LIneLengthcount - 1
            If arrLineLegthData(i).OpeFlg = 1 Then
                Dim LineD As New LineLengthItem
                LineD = arrLineLegthData(i)
                Update_LineLength(LineD)
            End If
        Next i
        gfrmProgressBar.ProgressBar1.Value = 40
        gfrmProgressBar.Refresh()

        'ポリラインの更新
        For i = 0 To m_PolyLinecount - 1
            If arrPolyLineData(i).OpeFlg = 1 Then
                Dim PolyLineD As New PolyLineItem
                PolyLineD = arrPolyLineData(i)
                Update_PolyLine(PolyLineD)
            End If
        Next i
        gfrmProgressBar.ProgressBar1.Value = 50
        gfrmProgressBar.Refresh()

        '１点円の更新
        For i = 0 To m_Circlecount - 1
            If arrCircleData(i).OpeFlg = 1 Then
                Dim CircleD As New CircleItem
                CircleD = arrCircleData(i)
                Update_Circle(CircleD)
            End If
        Next i
        gfrmProgressBar.ProgressBar1.Value = 60
        gfrmProgressBar.Refresh()

        'スケールの追加
        For i = 0 To m_ScaleItems.Count - 1
            If m_ScaleItems.Item(i).OpeFlg = 3 Then
                Dim ScaleD As New ScaleItem
                ScaleD = m_ScaleItems.Item(i)
                Insert_Scale(ScaleD)
            End If
        Next i
        gfrmProgressBar.ProgressBar1.Value = 70
        gfrmProgressBar.Refresh()

        '２点線分・長さの追加
        For i = 0 To m_LineLengthItems.Count - 1
            If m_LineLengthItems.Item(i).OpeFlg = 3 Then
                Dim LineD As New LineLengthItem
                LineD = m_LineLengthItems.Item(i)
                Insert_LineLength(LineD)
            End If
        Next i
        gfrmProgressBar.ProgressBar1.Value = 75
        gfrmProgressBar.Refresh()

        'ポリラインの追加
        For i = 0 To m_PolyLineItems.Count - 1
            If m_PolyLineItems.Item(i).OpeFlg = 3 Then
                Dim PolyLineD As New PolyLineItem
                PolyLineD = m_PolyLineItems.Item(i)
                Insert_PolyLine(PolyLineD)
            End If
        Next i
        gfrmProgressBar.ProgressBar1.Value = 80
        gfrmProgressBar.Refresh()

        '１点円の追加
        For i = 0 To m_CircleItems.Count - 1
            If m_CircleItems.Item(i).OpeFlg = 3 Then
                Dim CircleD As New CircleItem
                CircleD = m_CircleItems.Item(i)
                Insert_Circle(CircleD)
            End If
        Next i
        gfrmProgressBar.ProgressBar1.Value = 85
        gfrmProgressBar.Refresh()

        'SunpoSetテーブルの削除
        For i = 0 To m_SunpoCount - 1
            If arrSunpoSetData(i).OpeFlg = 2 Then
                Dim SunpoSetD As New SunpoSetTable
                SunpoSetD.SunpoID = arrSunpoSetData(i).SunpoID
                SunpoSet_Delete(SunpoSetD)
            End If
        Next i
        gfrmProgressBar.ProgressBar1.Value = 90
        gfrmProgressBar.Refresh()


        'データベース、及び、画面の再ロード
        DB_Read()
        gfrmProgressBar.ProgressBar1.Value = 99
        gfrmProgressBar.Refresh()
        Tab_Set(m_Targetcount)
        gfrmProgressBar.ProgressBar1.Value = 100
        gfrmProgressBar.Refresh()
        gfrmProgressBar.Close()

        Me.Show()
    End Sub

    '「キャンセル」ボタンクリック
    Private Sub Button_Cancel_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_Cancel.Click
        Me.Close()
    End Sub

    'CT_Bunruiテーブルに登録されている最大区分１コードを取得する
    Private Function Get_MaxKubun1() As Integer
        Dim max_kubun As Integer = 0
        Dim ii As Integer

        For ii = 0 To m_Targetcount - 1
            If arrCToffsetData(ii).kubun1 < 99 Then
                If arrCToffsetData(ii).kubun1 > max_kubun Then
                    max_kubun = arrCToffsetData(ii).kubun1
                End If
            End If
        Next ii
        Return (max_kubun)
    End Function

    'CT_Bunruiテーブルに登録されている、指定区分1コードにおける最大区分２コードを取得する
    Private Function Get_MaxKubun2(ByVal kubun1 As Integer) As Integer
        Dim max_kubun2 As Integer = 0
        Dim ii As Integer

        For ii = 0 To m_Targetcount - 1
            If arrCToffsetData(ii).kubun1 = kubun1 Then
                If arrCToffsetData(ii).kubun2 > max_kubun2 Then
                    max_kubun2 = arrCToffsetData(ii).kubun2
                End If
            End If
        Next ii
        Return (max_kubun2)
    End Function

    'TengunTeigiテーブルに登録されている最大点群IDを取得する
    Private Function Get_MaxTengunID() As Integer
        Dim max_teugunID As Integer = 0
        Dim ii As Integer
        Dim kubun As Integer = 0

        For ii = 0 To m_TengunCount - 1
            If arrTengunTeigiData(ii).TengunID > max_teugunID Then
                max_teugunID = arrTengunTeigiData(ii).TengunID
            End If
        Next ii
        Return (max_teugunID)
    End Function

    '点群IDよりターゲット番号を取得する
    Private Function Get_TargetNumber(ByVal TengunNumber As Integer) As Integer
        Dim ii As Integer
        Dim Rtn As Integer = -1
        Dim kubun1 As Integer = -1
        Dim kubun2 As Integer = -1

        For ii = 0 To m_TengunCount - 1
            If arrTengunTeigiData(ii).TengunID = TengunNumber Then
                kubun1 = arrTengunTeigiData(ii).Bunrui1
                kubun2 = arrTengunTeigiData(ii).Bunrui2
                Exit For
            End If
        Next ii

        If (kubun1 >= 0) And (kubun2 >= 0) Then
            For ii = 0 To m_Targetcount - 1
                If (arrCToffsetData(ii).kubun1 = kubun1) And (arrCToffsetData(ii).kubun2 = kubun2) Then
                    Rtn = arrCToffsetData(ii).CT_NO
                    Exit For
                End If
            Next ii
        End If

        Return (Rtn)
    End Function

    'ターゲット文字列を分解して、全てのターゲット番号を取得する
    Private Function GetNumber_TargetString(ByVal TargetString As String, ByRef TargetNumber() As Integer) As Integer
        Dim iCnt As Integer = 0
        Dim ii, jj As Integer
        Dim iCnt2 As Integer
        Dim TargetNumber2(100) As Integer
        Dim iFlg As Integer
        Dim iCnt3 As Integer
        Dim DbTGNumber(100) As Integer

        If TargetString.Length = 0 Then
            Return (0)
            Exit Function
        End If

        For ii = 0 To TargetString.Length - 1
            If (TargetString(ii) <> ","c) And (TargetString(ii) <> "-"c) Then
                If (TargetString(ii) < "0"c) Or (TargetString(ii) > "9"c) Then
                    Return (-999)
                    Exit Function
                End If
            End If
        Next ii

        Dim arrString As String() = TargetString.Split(","c)
        For Each stData As String In arrString
            If stData.IndexOf("-"c) > 0 Then
                Dim arrString2 As String() = stData.Split("-"c)
                If arrString2.Length = 2 Then
                    For ii = CInt(arrString2(0)) To CInt(arrString2(1))
                        TargetNumber2(iCnt) = ii
                        iCnt += 1
                    Next ii
                End If
            Else
                If Trim(stData) <> "" Then
                    TargetNumber2(iCnt) = CInt(stData)
                    iCnt += 1
                End If
            End If
        Next stData

        'ターゲット番号が一つも無い場合
        If iCnt = 0 Then
            Return (-998)
            Exit Function
        End If

        '重複しているターゲット番号を削除する
        iCnt2 = iCnt
        iCnt = 0
        iCnt3 = 0
        If iCnt2 > 0 Then
            For ii = 0 To iCnt2 - 1
                If ii = 0 Then
                    TargetNumber(ii) = TargetNumber2(ii)
                    iCnt += 1
                Else
                    iFlg = 0
                    For jj = 0 To iCnt - 1
                        If TargetNumber2(ii) = TargetNumber(jj) Then
                            iFlg = 1
                            Exit For
                        End If
                    Next jj
                    If iFlg = 0 Then
                        TargetNumber(iCnt) = TargetNumber2(ii)
                        iCnt += 1
                    Else
                        DbTGNumber(iCnt3) = TargetNumber2(ii)
                        iCnt3 += 1
                    End If
                End If
            Next ii
        End If

        If iCnt3 > 0 Then
            For ii = 0 To iCnt3 - 1
                TargetNumber(ii) = DbTGNumber(ii)
            Next ii
            iCnt = -1 * iCnt3
        End If

        Return (iCnt)
    End Function

    'ターゲット番号より点群IDを求める
    Private Function Get_TengunNumber(ByVal TargetNumber As Integer) As Integer
        Dim ii As Integer
        Dim Rtn As Integer = -1
        Dim kubun1 As Integer = -1
        Dim kubun2 As Integer = -1
        Dim GetCToffsetData As New CTBunruiDB

        'ターゲット番号より区分コードを取得する
        For ii = 0 To m_Targetcount - 1
            If arrCToffsetData(ii).CT_NO = TargetNumber Then
                kubun1 = arrCToffsetData(ii).kubun1
                kubun2 = arrCToffsetData(ii).kubun2
                GetCToffsetData = arrCToffsetData(ii)
                Exit For
            End If
        Next ii

        '区分コードより点群IDを取得する
        If (kubun1 > 0) And (kubun2 > 0) Then
            For ii = 0 To m_TengunCount - 1
                If (arrTengunTeigiData(ii).Bunrui1 = kubun1) And (arrTengunTeigiData(ii).Bunrui2 = kubun2) Then
                    Rtn = arrTengunTeigiData(ii).TengunID
                    Exit For
                End If
            Next ii
            '点群IDが取得できなかった時は区分2を０にして再度検索する
            If Rtn = -1 Then
                For ii = 0 To m_TengunCount - 1
                    If (arrTengunTeigiData(ii).Bunrui1 = kubun1) And (arrTengunTeigiData(ii).Bunrui2 = 0) Then
                        Rtn = arrTengunTeigiData(ii).TengunID
                        Exit For
                    End If
                Next ii
            End If
        End If

        '点群IDが取得できなかった時はTengunTeigiデータを新規追加する
        If Rtn = -1 Then
            Rtn = Get_MaxTengunID() + 1
            Dim TengunD As New TengunTeigiBunrui
            TengunD.TengunID = Rtn
            If kubun1 = 0 Then
                kubun1 = Get_MaxKubun1() + 1
                kubun2 = 0
                Dim CToffsetD As New CToffset
                CToffsetD.CT_ID = GetCToffsetData.CT_NO
                CToffsetD.flg_Use = True
                CToffsetD.offset_val.x = GetCToffsetData.m_x
                CToffsetD.offset_val.y = GetCToffsetData.m_y
                CToffsetD.offset_val.z = GetCToffsetData.m_z
                CToffsetD.kubun1 = kubun1
                CToffsetD.kubun2 = kubun2
                CToffsetD.info = GetCToffsetData.info
                CT_Bunrui_Update(CToffsetD)

                'CT_Bunruiテーブルを再読み込みする
                m_Targetcount = 0
                Read_CTBunruiDB()
            End If
            TengunD.Bunrui1 = kubun1
            TengunD.Bunrui2 = kubun2
            TenGunTeigi_Insert(TengunD)

            'TengunTeigiデータを再読み込みする
            m_TengunCount = 0
            Read_TengunTeigiDB()
        End If

        Return (Rtn)
    End Function

    'スケールの更新を行う
    Private Sub Update_Scale(ByVal ScaleD As ScaleItem)
        Dim ii As Integer
        Dim SunpoD As New SunpoSetTable
        Dim GetSunpoD As New SunpoSetTable
        Dim UpdateFlg As Integer = 0
        Dim TengunN As Integer

        '更新するスケールの寸法IDより、対応する寸法データを特定する
        For ii = 0 To m_SunpoCount - 1
            If ScaleD.SunpoID = arrSunpoSetL(ii).SunpoID Then
                SunpoD = arrSunpoSetL(ii)
                Exit For
            End If
        Next ii
        If SunpoD Is Nothing Then
            Exit Sub
        End If

        'ターゲット番号の更新
        If (CInt(SunpoD.CT_ID1) > 0) And (CInt(SunpoD.CT_ID2) > 0) Then
            If CInt(SunpoD.CT_ID1) <> CInt(ScaleD.StartTarget) Then
                SunpoD.CT_ID1 = ScaleD.StartTarget.ToString
                UpdateFlg = 1
            End If
            If CInt(SunpoD.CT_ID2) <> CInt(ScaleD.EndTarget) Then
                SunpoD.CT_ID2 = ScaleD.EndTarget.ToString
                UpdateFlg = 1
            End If
        ElseIf (CInt(SunpoD.GunID1) > 0) And (CInt(SunpoD.GunID2) > 0) Then
            TengunN = Get_TengunNumber(CInt(ScaleD.StartTarget))
            If CInt(SunpoD.GunID1) <> TengunN Then
                SunpoD.GunID1 = TengunN.ToString
                UpdateFlg = 1
            End If
            TengunN = Get_TengunNumber(CInt(ScaleD.EndTarget))
            If CInt(SunpoD.GunID2) <> TengunN Then
                SunpoD.GunID2 = TengunN.ToString
                UpdateFlg = 1
            End If
        End If

        '寸法マークの更新
        If SunpoD.SunpoMark <> ScaleD.SunpoMark Then
            SunpoD.SunpoMark = ScaleD.SunpoMark.ToString
            UpdateFlg = 1
        End If

        '寸法名の更新
        If SunpoD.SunpoName <> ScaleD.SunpoName Then
            SunpoD.SunpoName = ScaleD.SunpoName.ToString
            UpdateFlg = 1
        End If

        '規定値（長さ）の更新
        If (System.Math.Abs(CDbl(SunpoD.KiteiVal) - CDbl(ScaleD.KiteiVal)) > 0.001) Then
            SunpoD.KiteiVal = ScaleD.KiteiVal.ToString
            UpdateFlg = 1
        End If

        'スケールフラグの更新
        If SunpoD.flgScale <> ScaleD.flgScale Then
            SunpoD.flgScale = ScaleD.flgScale.ToString
            UpdateFlg = 1
        End If

        'SunpoSetテーブル更新
        If UpdateFlg = 1 Then
            SunpoSet_Update(SunpoD)
        End If
    End Sub

    'スケールの追加を行う
    Private Sub Insert_Scale(ByVal ScaleD As ScaleItem)
        Dim SunpoD As New SunpoSetTable
        Dim GetSunpoD As New SunpoSetTable
        Dim UpdateFlg As Integer = 0

        '寸法データに初期値をセットする
        SunpoData_Default(SunpoD)

        'ターゲット番号の更新
        SunpoD.SunpoID = ScaleD.SunpoID.ToString
        SunpoD.CT_ID1 = ScaleD.StartTarget.ToString
        SunpoD.CT_ID2 = ScaleD.EndTarget.ToString
        SunpoD.CT_ID3 = "0"
        SunpoD.GunID1 = "0"
        SunpoD.GunID2 = "0"
        SunpoD.GunID3 = "0"
        SunpoD.SunpoMark = ScaleD.SunpoMark
        SunpoD.SunpoName = ScaleD.SunpoName
        SunpoD.SunpoCalcHouhou = "1"
        SunpoD.KiteiVal = ScaleD.KiteiVal.ToString
        SunpoD.KiteiMin = "-1.0"
        SunpoD.KiteiMax = "1.0"
        SunpoD.CT_Active = "1"
        SunpoD.seibunXYZ = "7"
        SunpoD.flgScale = ScaleD.flgScale.ToString
        SunpoD.flgOutZu = "1"

        'SunpoSetテーブル更新
        SunpoSet_Insert(SunpoD)

    End Sub

    '座標系の更新を行う
    Private Sub Update_Zahyou(ByVal ZahyouD As ZahyouDBGet)
        Dim CoordD As New ZahyouDBGet
        Dim SunpoD As New SunpoSetTable
        Dim ii As Integer

        If ZahyouD.CT_GenID <> CInt(arrZahyouGet(0).CT_No) Then
            CoordD.CT_GenID = Get_TengunNumber(CInt(arrZahyouGet(0).CT_No))
        Else
            If ZahyouD.CT_GenID = -1 Then
                CoordD.CT_GenID = 0
            Else
                CoordD.CT_GenID = ZahyouD.CT_GenID
            End If
        End If
        If ZahyouD.CT_XID <> CInt(arrZahyouGet(1).CT_No) Then
            CoordD.CT_XID = Get_TengunNumber(CInt(arrZahyouGet(1).CT_No))
        Else
            If ZahyouD.CT_XID = -1 Then
                CoordD.CT_XID = 0
            Else
                CoordD.CT_XID = ZahyouD.CT_XID
            End If
        End If
        If ZahyouD.CT_YID <> CInt(arrZahyouGet(2).CT_No) Then
            CoordD.CT_YID = Get_TengunNumber(CInt(arrZahyouGet(2).CT_No))
        Else
            If ZahyouD.CT_YID = -1 Then
                CoordD.CT_YID = 0
            Else
                CoordD.CT_YID = ZahyouD.CT_YID
            End If
        End If
        If ZahyouD.CT_ZID <> CInt(arrZahyouGet(3).CT_No) Then
            CoordD.CT_ZID = Get_TengunNumber(CInt(arrZahyouGet(3).CT_No))
        Else
            If ZahyouD.CT_ZID = -1 Then
                CoordD.CT_ZID = 0
            Else
                CoordD.CT_ZID = ZahyouD.CT_ZID
            End If
        End If
        If ZahyouD.CT_ID1 <> CInt(arrZahyouGet(4).CT_No) Then
            CoordD.CT_ID1 = Get_TengunNumber(CInt(arrZahyouGet(4).CT_No))
        Else
            If ZahyouD.CT_ID1 = -1 Then
                CoordD.CT_ID1 = 0
            Else
                CoordD.CT_ID1 = ZahyouD.CT_ID1
            End If
        End If
        If ZahyouD.CT_ID2 <> CInt(arrZahyouGet(5).CT_No) Then
            CoordD.CT_ID2 = Get_TengunNumber(CInt(arrZahyouGet(5).CT_No))
        Else
            If ZahyouD.CT_ID2 = -1 Then
                CoordD.CT_ID2 = 0
            Else
                CoordD.CT_ID2 = ZahyouD.CT_ID2
            End If
        End If
        If ZahyouD.CT_ID3 <> CInt(arrZahyouGet(6).CT_No) Then
            CoordD.CT_ID3 = Get_TengunNumber(CInt(arrZahyouGet(6).CT_No))
        Else
            If ZahyouD.CT_ID3 = -1 Then
                CoordD.CT_ID3 = 0
            Else
                CoordD.CT_ID3 = ZahyouD.CT_ID3
            End If
        End If
        If CoordD.XYorXZorYZ <> Combo_Zahyou.SelectedIndex Then
            CoordD.XYorXZorYZ = Combo_Zahyou.SelectedIndex
        End If
        CoordD.Active = ZahyouD.Active

        CoordSet_Update(CoordD)

        '座標系モードが高さ補正有りの場合
        If Combo_Zahyou.SelectedIndex = 1 Then
            If Zahyou_HData.OpeFlg = 3 Then     '高さ補正値をSunpoSetテーブルに追加
                SunpoData_Default(SunpoD)
                SunpoD.SunpoMark = "Leveladj"
                SunpoD.KiteiVal = Zahyou_HData.Zahyou_Height
                SunpoD.SunpoID = Zahyou_HData.SunpoID
                SunpoD.CT_Active = "1"
                SunpoSet_Insert(SunpoD)
            ElseIf Zahyou_HData.OpeFlg = 1 Then '高さ補正値を更新
                For ii = 0 To m_SunpoCount - 1
                    If arrSunpoSetL(ii).SunpoID = Zahyou_HData.SunpoID Then
                        SunpoD = arrSunpoSetL(ii)
                        Exit For
                    End If
                Next ii
                If SunpoD Is Nothing Then
                    Exit Sub
                End If
                SunpoD.KiteiVal = Zahyou_HData.Zahyou_Height
                SunpoSet_Update(SunpoD)
            End If
        End If

    End Sub

    '２点線分・長さの更新
    Private Sub Update_LineLength(ByVal LineD As LineLengthItem)
        Dim ii As Integer
        Dim SunpoD As New SunpoSetTable
        Dim GetSunpoD As New SunpoSetGunID
        Dim UpdateFlg As Integer = 0
        Dim TengunNum As Integer = -1

        '更新元の寸法データを取得する
        For ii = 0 To m_SunpoCount - 1
            If arrSunpoSetL(ii).SunpoID = LineD.SunpoID Then
                SunpoD = arrSunpoSetL(ii)
                Exit For
            End If
        Next ii
        If SunpoD Is Nothing Then
            Exit Sub
        End If

        'ターゲット番号の更新
        For ii = 0 To m_SunpoCount - 1
            If LineD.SunpoID = arrSunpoSetData(ii).SunpoID Then
                GetSunpoD = arrSunpoSetData(ii)
                Exit For
            End If
        Next ii

        'ターゲット番号の更新
        TengunNum = Get_TengunNumber(CInt(LineD.StartTarget))
        If CInt(SunpoD.GunID1) <> TengunNum Then
            SunpoD.GunID1 = TengunNum.ToString
            UpdateFlg = 1
        End If
        TengunNum = Get_TengunNumber(CInt(LineD.EndTarget))
        If CInt(SunpoD.GunID2) <> TengunNum Then
            SunpoD.GunID2 = TengunNum.ToString
            UpdateFlg = 1
        End If

        '寸歩マーク、寸法名更新
        If SunpoD.SunpoMark <> LineD.SunpoMark Then
            SunpoD.SunpoMark = LineD.SunpoMark
            UpdateFlg = 1
        End If
        If SunpoD.SunpoName <> LineD.SunpoName Then
            SunpoD.SunpoName = LineD.SunpoName
            UpdateFlg = 1
        End If

        'XYZ成分の更新
        If LineD.Dimension <> CInt(SunpoD.seibunXYZ) Then
            SunpoD.seibunXYZ = LineD.Dimension.ToString
            UpdateFlg = 1
        End If

        '画層の更新
        If LineD.Layer <> SunpoD.ZU_layer Then
            If LineD.Layer = "" Then
                SunpoD.ZU_layer = "0"
            Else
                SunpoD.ZU_layer = LineD.Layer
            End If
            UpdateFlg = 1
        End If

        '線色の更新
        If LineD.LineColor <> CInt(SunpoD.ZU_colorID) Then
            SunpoD.ZU_colorID = LineD.LineColor.ToString
            UpdateFlg = 1
        End If

        '線種の更新
        If LineD.LineType <> CInt(SunpoD.ZU_LineTypeID) Then
            SunpoD.ZU_LineTypeID = LineD.LineType.ToString
            UpdateFlg = 1
        End If

        'SunpoSetテーブル更新
        If UpdateFlg = 1 Then
            SunpoSet_Update(SunpoD)
        End If

    End Sub

    '２点線分・長さの追加
    Private Sub Insert_LineLength(ByVal LineD As LineLengthItem)
        Dim SunpoD As New SunpoSetTable
        Dim GetSunpoD As New SunpoSetGunID

        'ターゲット番号の更新
        SunpoD.CT_ID1 = 0
        SunpoD.CT_ID2 = 0
        SunpoD.CT_ID3 = 0
        SunpoD.GunID1 = Get_TengunNumber(CInt(LineD.StartTarget)).ToString
        SunpoD.GunID2 = Get_TengunNumber(CInt(LineD.EndTarget)).ToString
        SunpoD.GunID3 = 0

        'その他のパラメータの更新
        SunpoD.TypeID = CommonTypeID.ToString
        SunpoD.SunpoID = LineD.SunpoID.ToString
        SunpoD.SunpoMark = LineD.SunpoMark
        SunpoD.SunpoName = LineD.SunpoName
        Dim Dimension As Integer = 0
        Select Case LineD.Dimension
            Case "X"
                Dimension = XYZseibun.X
            Case "Y"
                Dimension = XYZseibun.Y
            Case "Z"
                Dimension = XYZseibun.Z
            Case "XY"
                Dimension = XYZseibun.XY
            Case "XZ"
                Dimension = XYZseibun.XZ
            Case "YZ"
                Dimension = XYZseibun.YZ
            Case "XYZ"
                Dimension = XYZseibun.XYZ
        End Select
        SunpoD.seibunXYZ = Dimension.ToString
        If LineD.Layer = "" Then
            LineD.Layer = "0"
        End If
        SunpoD.ZU_layer = LineD.Layer
        Dim colorID As Integer
        ncolor = 0
        YCM_ReadSystemColorAcs(m_strDataSystemPath)
        Dim mcolor As ModelColor
        If ncolor > 0 Then
            mcolor = YCM_GetColorInfoByName(LineD.LineColor)
            colorID = mcolor.code
        End If
        SunpoD.ZU_colorID = colorID.ToString
        Dim ltype As Integer
        nLineType = 0
        YCM_ReadSystemLineTypes(m_strDataSystemPath)
        Dim mLinetype As LineType
        If nLineType > 0 Then
            mLinetype = YCM_GetLineTypeInfoByName(LineD.LineType)
            ltype = mLinetype.code
        End If
        SunpoD.ZU_LineTypeID = ltype.ToString
        SunpoD.SunpoCalcHouhou = "8"
        SunpoD.KiteiVal = "0.0"
        SunpoD.KiteiMin = "-1.0"
        SunpoD.KiteiMax = "1.0"
        SunpoD.CT_Active = "1"
        SunpoD.flgScale = "-1"
        SunpoD.flgOutZu = "1"

        'SunpoSetテーブル更新
        SunpoSet_Insert(SunpoD)

    End Sub

    'ポリラインの更新
    Private Sub Update_PolyLine(ByVal PolyLineD As PolyLineItem)
        Dim ii, jj As Integer
        Dim SunpoD As New SunpoSetTable
        Dim GetSunpoD As New SunpoSetGunID
        Dim TGNumber(500) As Integer
        Dim kubun1 As Integer = -1
        Dim kubun2 As Integer = -1
        Dim IDX4 As Integer = 0
        Dim motoTargetN(500) As Integer
        Dim UpdateTargetFlg As Integer = 0
        Dim UpdateFlg As Integer = 0

        '更新元の寸法データ取得
        For ii = 0 To m_SunpoCount - 1
            If PolyLineD.SunpoID = arrSunpoSetL(ii).SunpoID Then
                SunpoD = arrSunpoSetL(ii)
                Exit For
            End If
        Next ii
        If SunpoD Is Nothing Then
            Exit Sub
        End If

        '更新する寸法データを取得する
        For ii = 0 To m_SunpoCount - 1
            If PolyLineD.SunpoID = arrSunpoSetData(ii).SunpoID Then
                GetSunpoD = arrSunpoSetData(ii)
                Exit For
            End If
        Next ii
        If GetSunpoD Is Nothing Then
            Exit Sub
        End If

        '更新元のターゲット番号列を取得する
        IDX4 = Get_TargetArray(GetSunpoD.GunID1, motoTargetN)
        If IDX4 = 0 Then
            Exit Sub
        End If

        'ターゲット文字列を分解し、ターゲット番号列を取得する
        Dim TGCnt As Integer = GetNumber_TargetString(PolyLineD.VertexTarget, TGNumber)
        If TGCnt <= 0 Then
            Exit Sub
        End If

        'ターゲット番号の更新有無のチェック
        If IDX4 <> TGCnt Then
            UpdateTargetFlg = 1
        Else
            Dim UpdateCnt As Integer = 0
            For ii = 0 To IDX4 - 1
                For jj = 0 To TGCnt - 1
                    If motoTargetN(ii) = TGNumber(jj) Then
                        UpdateCnt += 1
                        Exit For
                    End If
                Next jj
            Next ii
            If UpdateCnt <> IDX4 Then
                UpdateTargetFlg = 1
            End If
        End If

        'ターゲットの更新
        If UpdateTargetFlg = 1 Then
            'SunpoSetの点群IDより、区分コードを取得する
            For ii = 0 To m_TengunCount - 1
                If arrTengunTeigiData(ii).TengunID = GetSunpoD.GunID1 Then
                    kubun1 = arrTengunTeigiData(ii).Bunrui1
                    kubun2 = arrTengunTeigiData(ii).Bunrui2
                    Exit For
                End If
            Next ii
            If kubun1 < 0 Then
                Exit Sub
            ElseIf kubun1 = 0 Then
                kubun1 = Get_MaxKubun1() + 1
                kubun2 = 0
            End If

            'CT_Bunruiテーブルを更新する
            For ii = 0 To m_Targetcount - 1
                If arrCToffsetData(ii).kubun1 = kubun1 Then
                    arrCToffsetData(ii).kubun1 = 0
                    arrCToffsetData(ii).kubun2 = 0
                    arrCToffsetData(ii).OpeFlg = 1
                End If
            Next ii

            For jj = 0 To TGCnt - 1
                For ii = 0 To m_Targetcount - 1
                    If arrCToffsetData(ii).CT_NO = TGNumber(jj) Then
                        arrCToffsetData(ii).kubun1 = kubun1
                        arrCToffsetData(ii).kubun2 = kubun2
                        arrCToffsetData(ii).OpeFlg = 1
                        kubun2 += 1
                        Exit For
                    End If
                Next ii
            Next jj

            Dim CToffsetD As New CToffset
            For ii = 0 To m_Targetcount - 1
                If arrCToffsetData(ii).OpeFlg = 1 Then
                    CToffsetD.CT_ID = arrCToffsetData(ii).CT_NO
                    CToffsetD.flg_Use = True
                    CToffsetD.offset_val.x = arrCToffsetData(ii).m_x
                    CToffsetD.offset_val.y = arrCToffsetData(ii).m_y
                    CToffsetD.offset_val.z = arrCToffsetData(ii).m_z
                    CToffsetD.kubun1 = arrCToffsetData(ii).kubun1
                    CToffsetD.kubun2 = arrCToffsetData(ii).kubun2
                    CToffsetD.info = arrCToffsetData(ii).info
                    'CT_Bunruiテーブル更新
                    CT_Bunrui_Update(CToffsetD)
                End If
            Next ii

            'CT_Bunruiテーブルを再読み込みする
            m_Targetcount = 0
            Dim TargetNum As Integer = Read_CTBunruiDB()
        End If

        '寸法マーク、寸法名の更新
        If SunpoD.SunpoMark <> PolyLineD.SunpoMark Then
            SunpoD.SunpoMark = PolyLineD.SunpoMark
            UpdateFlg = 1
        End If
        If SunpoD.SunpoName <> PolyLineD.SunpoName Then
            SunpoD.SunpoName = PolyLineD.SunpoName
            UpdateFlg = 1
        End If

        'XYZ成分の更新
        If PolyLineD.Dimension <> CInt(SunpoD.seibunXYZ) Then
            SunpoD.seibunXYZ = PolyLineD.Dimension.ToString
            UpdateFlg = 1
        End If

        '画層の更新
        If PolyLineD.Layer <> SunpoD.ZU_layer Then
            If PolyLineD.Layer = "" Then
                SunpoD.ZU_layer = "0"
            Else
                SunpoD.ZU_layer = PolyLineD.Layer
            End If
            UpdateFlg = 1
        End If

        '線色の更新
        If PolyLineD.LineColor <> CInt(SunpoD.ZU_colorID) Then
            SunpoD.ZU_colorID = PolyLineD.LineColor.ToString
            UpdateFlg = 1
        End If

        '線種の更新
        Dim ltype As Integer
        nLineType = 0
        YCM_ReadSystemLineTypes(m_strDataSystemPath)
        Dim mLinetype As LineType
        If nLineType > 0 Then
            mLinetype = YCM_GetLineTypeInfoByName(PolyLineD.LineType)
            ltype = mLinetype.code
        End If
        If PolyLineD.LineType <> CInt(SunpoD.ZU_LineTypeID) Then
            SunpoD.ZU_LineTypeID = PolyLineD.LineType.ToString
            UpdateFlg = 1
        End If

        'SunpoSetテーブル更新
        If UpdateFlg = 1 Then
            SunpoSet_Update(SunpoD)
        End If

    End Sub

    'ポリラインの追加
    Private Sub Insert_PolyLine(ByVal PolyLineD As PolyLineItem)
        Dim ii, jj As Integer
        Dim SunpoD As New SunpoSetTable
        Dim GetSunpoD As New SunpoSetGunID
        Dim TGNumber(500) As Integer
        Dim kubun1 As Integer = -1
        Dim kubun2 As Integer = -1
        Dim maxKubun As Integer = 0
        Dim maxTengun As Integer = 0
        Dim TengunD As New TengunTeigiBunrui

        '最大区分番号、最大点群IDを取得する
        maxKubun = Get_MaxKubun1()
        maxTengun = Get_MaxTengunID()
        kubun1 = maxKubun + 1
        kubun2 = 0

        'ターゲット文字列を分解し、ターゲット番号列を取得する
        Dim TGCnt As Integer = GetNumber_TargetString(PolyLineD.VertexTarget, TGNumber)
        If TGCnt <= 0 Then
            Exit Sub
        End If

        For ii = 0 To TGCnt - 1
            For jj = 0 To m_Targetcount - 1
                If arrCToffsetData(jj).CT_NO = TGNumber(ii) Then
                    arrCToffsetData(jj).kubun1 = kubun1
                    arrCToffsetData(jj).kubun2 = kubun2 + 1
                    arrCToffsetData(jj).OpeFlg = 1
                    Exit For
                End If
            Next jj
        Next ii

        Dim CToffsetD As New CToffset
        For ii = 0 To m_Targetcount - 1
            If arrCToffsetData(ii).OpeFlg = 1 Then
                CToffsetD.CT_ID = arrCToffsetData(ii).CT_NO
                CToffsetD.flg_Use = True
                CToffsetD.offset_val.x = arrCToffsetData(ii).m_x
                CToffsetD.offset_val.y = arrCToffsetData(ii).m_y
                CToffsetD.offset_val.z = arrCToffsetData(ii).m_z
                CToffsetD.kubun1 = arrCToffsetData(ii).kubun1
                CToffsetD.kubun2 = arrCToffsetData(ii).kubun2
                CToffsetD.info = arrCToffsetData(ii).info
                'CT_Bunruiテーブル更新
                CT_Bunrui_Update(CToffsetD)
            End If
        Next ii

        'TengunTeigiテーブルの値の設定
        TengunD.TengunID = maxTengun + 1
        TengunD.Bunrui1 = kubun1
        TengunD.Bunrui2 = kubun2
        'TengunTeigiテーブル追加
        TenGunTeigi_Insert(TengunD)

        'CT_Bunruiテーブル、TengunTeigiテーブルを再読み込みする
        m_Targetcount = 0
        Dim TargetNum As Integer = Read_CTBunruiDB()
        m_TengunCount = 0
        Dim TengunNum As Integer = Read_TengunTeigiDB()

        'SunpoSetテーブルの更新を行う
        'ターゲット番号の更新
        SunpoD.CT_ID1 = "0"
        SunpoD.CT_ID2 = "0"
        SunpoD.CT_ID3 = "0"
        SunpoD.GunID1 = TengunD.TengunID.ToString
        SunpoD.GunID2 = "0"
        SunpoD.GunID3 = "0"
        'その他のパラメータの更新
        SunpoD.TypeID = CommonTypeID.ToString
        SunpoD.SunpoID = PolyLineD.SunpoID.ToString
        SunpoD.SunpoMark = PolyLineD.SunpoMark
        SunpoD.SunpoName = PolyLineD.SunpoName
        SunpoD.SunpoCalcHouhou = "11"
        Dim Dimension As Integer = 0
        Select Case PolyLineD.Dimension
            Case "X"
                Dimension = XYZseibun.X
            Case "Y"
                Dimension = XYZseibun.Y
            Case "Z"
                Dimension = XYZseibun.Z
            Case "XY"
                Dimension = XYZseibun.XY
            Case "XZ"
                Dimension = XYZseibun.XZ
            Case "YZ"
                Dimension = XYZseibun.YZ
            Case "XYZ"
                Dimension = XYZseibun.XYZ
        End Select
        SunpoD.seibunXYZ = Dimension.ToString
        SunpoD.ZU_layer = PolyLineD.Layer
        Dim colorID As Integer
        ncolor = 0
        YCM_ReadSystemColorAcs(m_strDataSystemPath)
        Dim mcolor As ModelColor
        If ncolor > 0 Then
            mcolor = YCM_GetColorInfoByName(PolyLineD.LineColor)
            colorID = mcolor.code
        End If
        SunpoD.ZU_colorID = colorID.ToString
        Dim ltype As Integer
        nLineType = 0
        YCM_ReadSystemLineTypes(m_strDataSystemPath)
        Dim mLinetype As LineType
        If nLineType > 0 Then
            mLinetype = YCM_GetLineTypeInfoByName(PolyLineD.LineType)
            ltype = mLinetype.code
        End If
        SunpoD.ZU_LineTypeID = ltype.ToString
        SunpoD.KiteiVal = "0.0"
        SunpoD.KiteiMin = "-1.0"
        SunpoD.KiteiMax = "1.0"
        SunpoD.CT_Active = "1"
        SunpoD.flgScale = "-1"
        SunpoD.flgOutZu = "1"

        'SunpoSetテーブル更新
        SunpoSet_Insert(SunpoD)

    End Sub

    '１点円の更新
    Private Sub Update_Circle(ByVal CircleD As CircleItem)
        Dim ii, jj As Integer
        Dim SunpoD As New SunpoSetTable
        Dim GetSunpoD As New SunpoSetGunID
        Dim TGNumber(500) As Integer
        Dim kubun1 As Integer = -1
        Dim kubun2 As Integer = -1
        Dim IDX4 As Integer = 0
        Dim motoTargetN(100) As Integer
        Dim UpdateFlg As Integer = 0

        '変更元寸法データを取得する
        For ii = 0 To m_SunpoCount - 1
            If arrSunpoSetL(ii).SunpoID = CircleD.SunpoID Then
                SunpoD = arrSunpoSetL(ii)
                Exit For
            End If
        Next ii
        If SunpoD Is Nothing Then
            Exit Sub
        End If

        '更新する寸法データを取得する
        For ii = 0 To m_SunpoCount - 1
            If CircleD.SunpoID = arrSunpoSetData(ii).SunpoID Then
                GetSunpoD = arrSunpoSetData(ii)
                Exit For
            End If
        Next ii
        If GetSunpoD Is Nothing Then
            Exit Sub
        End If

        '更新元のターゲット番号列を取得する
        IDX4 = Get_TargetArray(GetSunpoD.GunID1, motoTargetN)
        'If IDX4 = 0 Then
        '    Exit Sub
        'End If

        'ターゲット文字列を分解し、ターゲット番号列を取得する
        Dim TGCnt As Integer = GetNumber_TargetString(CircleD.CenterTarget, TGNumber)
        If TGCnt <= 0 Then
            Exit Sub
        End If

        'ターゲット番号の更新有無のチェック
        Dim UpdateTargetFlg As Integer = 0
        If IDX4 <> TGCnt Then
            UpdateTargetFlg = 1
        Else
            Dim UpdateCnt As Integer = 0
            For ii = 0 To IDX4 - 1
                For jj = 0 To TGCnt - 1
                    If motoTargetN(ii) = TGNumber(jj) Then
                        UpdateCnt += 1
                        Exit For
                    End If
                Next jj
            Next ii
            If UpdateCnt <> IDX4 Then
                UpdateTargetFlg = 1
            End If
        End If

        'CT_Bunruiテーブルの更新
        If UpdateTargetFlg = 1 Then
            'SunpoSetの点群IDより、区分コードを取得する
            For ii = 0 To m_TengunCount - 1
                If arrTengunTeigiData(ii).TengunID = GetSunpoD.GunID1 Then
                    kubun1 = arrTengunTeigiData(ii).Bunrui1
                    kubun2 = arrTengunTeigiData(ii).Bunrui2
                    Exit For
                End If
            Next ii
            If (kubun1 < 0) Or (kubun2 < 0) Then
                Exit Sub
            ElseIf kubun1 = 0 Then
                kubun1 = Get_MaxKubun1() + 1
                kubun2 = 0
            End If

            'CT_Bunruiテーブルを更新する
            For ii = 0 To m_Targetcount - 1
                If (arrCToffsetData(ii).kubun1 = kubun1) And (arrCToffsetData(ii).kubun2 = kubun2) Then
                    arrCToffsetData(ii).kubun1 = 0
                    arrCToffsetData(ii).kubun2 = 0
                    arrCToffsetData(ii).OpeFlg = 1
                End If
            Next ii

            For ii = 0 To TGCnt - 1
                For jj = 0 To m_Targetcount - 1
                    If arrCToffsetData(jj).CT_NO = TGNumber(ii) Then
                        arrCToffsetData(jj).kubun1 = kubun1
                        arrCToffsetData(jj).kubun2 = kubun2
                        arrCToffsetData(jj).OpeFlg = 1
                        Exit For
                    End If
                Next jj
            Next ii

            Dim CToffsetD As New CToffset
            For ii = 0 To m_Targetcount - 1
                If arrCToffsetData(ii).OpeFlg = 1 Then
                    CToffsetD.CT_ID = arrCToffsetData(ii).CT_NO
                    CToffsetD.flg_Use = True
                    CToffsetD.offset_val.x = arrCToffsetData(ii).m_x
                    CToffsetD.offset_val.y = arrCToffsetData(ii).m_y
                    CToffsetD.offset_val.z = arrCToffsetData(ii).m_z
                    CToffsetD.kubun1 = arrCToffsetData(ii).kubun1
                    CToffsetD.kubun2 = arrCToffsetData(ii).kubun2
                    CToffsetD.info = arrCToffsetData(ii).info
                    'CT_Bunruiテーブル更新
                    CT_Bunrui_Update(CToffsetD)
                End If
            Next ii

            'CT_Bunruiテーブルを再読み込みする
            m_Targetcount = 0
            Dim TargetNum As Integer = Read_CTBunruiDB()
        End If

        'SunpoSetテーブルの更新を行う
        '寸法マーク、寸法名の更新
        If SunpoD.SunpoMark <> CircleD.SunpoMark Then
            SunpoD.SunpoMark = CircleD.SunpoMark
            UpdateFlg = 1
        End If
        If SunpoD.SunpoName <> CircleD.SunpoName Then
            SunpoD.SunpoName = CircleD.SunpoName
            UpdateFlg = 1
        End If

        '次元の更新
        If CInt(SunpoD.seibunXYZ) <> CircleD.Dimension Then
            SunpoD.seibunXYZ = CircleD.Dimension.ToString
            UpdateFlg = 1
        End If

        '画層の更新
        If SunpoD.ZU_layer <> CircleD.Layer Then
            SunpoD.ZU_layer = CircleD.Layer
            UpdateFlg = 1
        End If

        '線色の更新
        If CInt(SunpoD.ZU_colorID) <> CircleD.LineColor Then
            SunpoD.ZU_colorID = CircleD.LineColor.ToString
            UpdateFlg = 1
        End If

        '線種の更新
        If CInt(SunpoD.ZU_LineTypeID) <> CircleD.LineType Then
            SunpoD.ZU_LineTypeID = CircleD.LineType.ToString
            UpdateFlg = 1
        End If

        '規定値（直径）の更新
        If (System.Math.Abs(CDbl(SunpoD.KiteiVal) - CDbl(CircleD.Diameter)) > 0.001) Then
            SunpoD.KiteiVal = CircleD.Diameter.ToString
            UpdateFlg = 1
        End If

        'SunpoSetテーブル更新
        If UpdateFlg = 1 Then
            SunpoSet_Update(SunpoD)
        End If

    End Sub

    '１点円の追加
    Private Sub Insert_Circle(ByVal CircleD As CircleItem)
        Dim ii, jj As Integer
        Dim SunpoD As New SunpoSetTable
        Dim GetSunpoD As New SunpoSetGunID
        Dim TGNumber(500) As Integer
        Dim kubun1 As Integer = -1
        Dim kubun2 As Integer = -1
        Dim maxKubun1 As Integer = 0
        Dim maxkubun2 As Integer = 0
        Dim maxTengun As Integer = 0
        Dim TengunD As New TengunTeigiBunrui

        '最大区分番号、最大点群IDを取得する
        maxKubun1 = Get_MaxKubun1()
        maxTengun = Get_MaxTengunID()
        kubun1 = maxKubun1 + 1
        kubun2 = 1

        'ターゲット文字列を分解し、ターゲット番号列を取得する
        Dim TGCnt As Integer = GetNumber_TargetString(CircleD.CenterTarget, TGNumber)
        If TGCnt <= 0 Then
            Exit Sub
        End If

        'CT_Bunruiテーブルを更新する
        For ii = 0 To TGCnt - 1
            For jj = 0 To m_Targetcount - 1
                If arrCToffsetData(jj).CT_NO = TGNumber(ii) Then
                    arrCToffsetData(jj).kubun1 = kubun1
                    arrCToffsetData(jj).kubun2 = kubun2
                    arrCToffsetData(jj).OpeFlg = 1
                    Exit For
                End If
            Next jj
        Next ii

        Dim CToffsetD As New CToffset
        For ii = 0 To m_Targetcount - 1
            If arrCToffsetData(ii).OpeFlg = 1 Then
                CToffsetD.CT_ID = arrCToffsetData(ii).CT_NO
                CToffsetD.flg_Use = True
                CToffsetD.offset_val.x = arrCToffsetData(ii).m_x
                CToffsetD.offset_val.y = arrCToffsetData(ii).m_y
                CToffsetD.offset_val.z = arrCToffsetData(ii).m_z
                CToffsetD.kubun1 = arrCToffsetData(ii).kubun1
                CToffsetD.kubun2 = arrCToffsetData(ii).kubun2
                CToffsetD.info = arrCToffsetData(ii).info
                'CT_Bunruiテーブル更新
                CT_Bunrui_Update(CToffsetD)
            End If
        Next ii

        'TengunTeigiテーブルの値の設定
        TengunD.TengunID = maxTengun + 1
        TengunD.Bunrui1 = kubun1
        TengunD.Bunrui2 = kubun2
        'TengunTeigiテーブル追加
        TenGunTeigi_Insert(TengunD)

        'CT_Bunruiテーブル、TengunTeigiテーブルを再読み込みする
        m_Targetcount = 0
        Dim TargetNum As Integer = Read_CTBunruiDB()
        m_TengunCount = 0
        Dim TengunNum As Integer = Read_TengunTeigiDB()

        'SunpoSetテーブルの追加を行う
        '点群IDの設定
        SunpoD.CT_ID1 = "0"
        SunpoD.CT_ID2 = "0"
        SunpoD.CT_ID3 = "0"
        SunpoD.GunID1 = TengunD.TengunID.ToString
        SunpoD.GunID2 = "0"
        SunpoD.GunID3 = "0"

        '寸法ID、寸法マーク、寸法名の設定
        SunpoD.TypeID = CommonTypeID.ToString
        SunpoD.SunpoID = CircleD.SunpoID.ToString
        SunpoD.SunpoMark = CircleD.SunpoMark
        SunpoD.SunpoName = CircleD.SunpoName

        '次元の設定
        Dim Dimension As Integer = 0
        Select Case CircleD.Dimension
            Case "X"
                Dimension = XYZseibun.X
            Case "Y"
                Dimension = XYZseibun.Y
            Case "Z"
                Dimension = XYZseibun.Z
            Case "XY"
                Dimension = XYZseibun.XY
            Case "XZ"
                Dimension = XYZseibun.XZ
            Case "YZ"
                Dimension = XYZseibun.YZ
            Case "XYZ"
                Dimension = XYZseibun.XYZ
        End Select
        SunpoD.seibunXYZ = Dimension.ToString

        '画層の設定
        SunpoD.ZU_layer = CircleD.Layer

        '線色の設定
        Dim colorID As Integer
        ncolor = 0
        YCM_ReadSystemColorAcs(m_strDataSystemPath)
        Dim mcolor As ModelColor
        If ncolor > 0 Then
            mcolor = YCM_GetColorInfoByName(CircleD.LineColor)
            colorID = mcolor.code
        End If
        SunpoD.ZU_colorID = colorID.ToString

        '線種の設定
        Dim ltype As Integer
        nLineType = 0
        YCM_ReadSystemLineTypes(m_strDataSystemPath)
        Dim mLinetype As LineType
        If nLineType > 0 Then
            mLinetype = YCM_GetLineTypeInfoByName(CircleD.LineType)
            ltype = mLinetype.code
        End If
        SunpoD.ZU_LineTypeID = ltype.ToString

        '寸法計算方法、規定値（直径）の設定
        SunpoD.SunpoCalcHouhou = "14"
        SunpoD.KiteiVal = CircleD.Diameter.ToString
        SunpoD.KiteiMin = "-1.0"
        SunpoD.KiteiMax = "1.0"
        SunpoD.CT_Active = "1"
        SunpoD.flgScale = "-1"
        SunpoD.flgOutZu = "1"

        'SunpoSetテーブル追加
        SunpoSet_Insert(SunpoD)

    End Sub

    '寸法データのイメージ作成（フィールド名、フィールド値）
    Private Sub CreateSunpoField(ByVal SunpoD As SunpoSetTable)

        Dim IDX As Integer = 0

        ReDim strFieldNames(IDX)
        ReDim strFieldTexts(IDX)
        strFieldNames(IDX) = "SunpoID"
        strFieldTexts(IDX) = SunpoD.SunpoID
        IDX += 1

        ReDim Preserve strFieldNames(IDX)
        ReDim Preserve strFieldTexts(IDX)
        strFieldNames(IDX) = "SunpoMark"
        strFieldTexts(IDX) = "'" & SunpoD.SunpoMark & "'"
        IDX += 1

        ReDim Preserve strFieldNames(IDX)
        ReDim Preserve strFieldTexts(IDX)
        strFieldNames(IDX) = "SunpoName"
        strFieldTexts(IDX) = "'" & SunpoD.SunpoName & "'"
        IDX += 1

        ReDim Preserve strFieldNames(IDX)
        ReDim Preserve strFieldTexts(IDX)
        strFieldNames(IDX) = "SunpoCellName"
        strFieldTexts(IDX) = "'" & SunpoD.SunpoCellName & "'"
        IDX += 1

        If SunpoD.SunpoCalcHouhou <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "SunpoCalcHouhou"
            strFieldTexts(IDX) = SunpoD.SunpoCalcHouhou
            IDX += 1
        End If

        If SunpoD.CT_ID1 <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "CT_ID1"
            strFieldTexts(IDX) = SunpoD.CT_ID1
            IDX += 1
        End If

        If SunpoD.CT_ID2 <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "CT_ID2"
            strFieldTexts(IDX) = SunpoD.CT_ID2
            IDX += 1
        End If

        If SunpoD.CT_ID3 <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "CT_ID3"
            strFieldTexts(IDX) = SunpoD.CT_ID3
            IDX += 1
        End If

        If SunpoD.seibunXYZ <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "seibunXYZ"
            strFieldTexts(IDX) = SunpoD.seibunXYZ
            IDX += 1
        End If

        If SunpoD.CT_Active <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "CT_Active"
            strFieldTexts(IDX) = SunpoD.CT_Active
            IDX += 1
        End If

        If SunpoD.SunpoVal <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "SunpoVal"
            strFieldTexts(IDX) = dblField(SunpoD.SunpoVal)
            IDX += 1
        End If

        If SunpoD.GunID1 <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "GunID1"
            strFieldTexts(IDX) = SunpoD.GunID1
            IDX += 1
        End If

        If SunpoD.GunID2 <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "GunID2"
            strFieldTexts(IDX) = SunpoD.GunID2
            IDX += 1
        End If

        If SunpoD.GunID3 <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "GunID3"
            strFieldTexts(IDX) = SunpoD.GunID3
            IDX += 1
        End If

        If SunpoD.KiteiVal <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "KiteiVal"
            strFieldTexts(IDX) = dblField(SunpoD.KiteiVal)
            IDX += 1
        End If

        If SunpoD.KiteiMin <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "KiteiMin"
            strFieldTexts(IDX) = dblField(SunpoD.KiteiMin)
            IDX += 1
        End If

        If SunpoD.KiteiMax <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "KiteiMax"
            strFieldTexts(IDX) = dblField(SunpoD.KiteiMax)
            IDX += 1
        End If

        If SunpoD.TypeID <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "TypeID"
            strFieldTexts(IDX) = CommonTypeID.ToString
            IDX += 1
        End If

        If SunpoD.flg_gouhi <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "flg_gouhi"
            strFieldTexts(IDX) = SunpoD.flg_gouhi
            IDX += 1
        End If

        If SunpoD.Targettype <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "Targettype"
            strFieldTexts(IDX) = SunpoD.Targettype
            IDX += 1
        End If

        If SunpoD.flgOutZu <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "flgOutZu"
            strFieldTexts(IDX) = SunpoD.flgOutZu
            IDX += 1
        End If

        If SunpoD.ZU_layer <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "ZU_layer"
            strFieldTexts(IDX) = "'" & SunpoD.ZU_layer & "'"
            IDX += 1
        End If

        If SunpoD.ZU_colorID <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "ZU_colorID"
            strFieldTexts(IDX) = SunpoD.ZU_colorID
            IDX += 1
        End If

        If SunpoD.ZU_LineTypeID <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "ZU_LineTypeID"
            strFieldTexts(IDX) = SunpoD.ZU_LineTypeID
            IDX += 1
        End If

        If SunpoD.flgKeisan <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "flgKeisan"
            strFieldTexts(IDX) = SunpoD.flgKeisan
            IDX += 1
        End If

        If SunpoD.flgScale <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "flgScale"
            strFieldTexts(IDX) = SunpoD.flgScale
            IDX += 1
        End If

    End Sub

    '寸法データに初期値をセットする
    Private Sub SunpoData_Default(ByVal SunpoD As SunpoSetTable)
        SunpoD.ID = "0"                               '0  ID
        SunpoD.SunpoID = "-1"                         '1  SunpoID
        SunpoD.SunpoMark = " "                        '2  SunpoPart
        SunpoD.SunpoName = " "                        '3  SunpoName
        SunpoD.SunpoCellName = " "                    '4  SunpoCellName
        SunpoD.SunpoCalcHouhou = "0"                  '5  寸法算出方法(2点間距離：１、線と点の距離：２）
        SunpoD.CT_ID1 = "-1"                          '6  点群ＩＤ１
        SunpoD.CT_ID2 = "-1"                          '7  点群ＩＤ２
        SunpoD.CT_ID3 = "-1"                          '8  点群ＩＤ３
        SunpoD.seibunXYZ = "0"                        '9  P1
        SunpoD.CT_Active = "1"                        '10 P2
        SunpoD.KiteiVal = "0.0"                       '15  KiteiVal
        SunpoD.KiteiMin = "0.0"                       '16  KiteiMin
        SunpoD.KiteiMax = "0.0"                       '17  KiteiMax
        SunpoD.SunpoVal = "0.0"                       '11 P3
        SunpoD.GunID1 = "-1"                          '12 
        SunpoD.GunID2 = "-1"                          '13 
        SunpoD.GunID3 = "-1"                          '14 
        SunpoD.TypeID = "0"                           '18  TypeID
        SunpoD.flg_gouhi = "0"                        '19  flg_gouhi
        SunpoD.Targettype = "0"                       '20  Targettype
        SunpoD.flgOutZu = "1"                         '21  flgOutZu
        SunpoD.ZU_layer = "0"                         '22  ZU_layer
        SunpoD.ZU_colorID = "0"                       '23  ZU_colorID
        SunpoD.ZU_LineTypeID = "0"                    '24  ZU_LineTypeID
        SunpoD.flgKeisan = "0"                        '25  flgKeisan
        SunpoD.flgScale = "-1"                        '26  flgScale
    End Sub

    '点群定義データのイメージ作成（フィールド名）
    Private Sub CreateTengunFieldName()
        ReDim strFieldNames(15)

        strFieldNames(0) = "タイプID"                 '1  TypeID
        strFieldNames(1) = "点群ＩＤ"                 '2  点群ＩＤ
        strFieldNames(2) = "元点群ＩＤ"               '3  元点群ＩＤ
        strFieldNames(3) = "処理方法ＩＤ"             '4  処理方法ＩＤ
        strFieldNames(4) = "ソーティング順"           '5  ソーティング順
        strFieldNames(5) = "ＸＹＺ成分"               '6  ＸＹＺ成分
        strFieldNames(6) = "基準点ＩＤ"               '7  基準点ＩＤ
        strFieldNames(7) = "大小指定"                 '8  大小指定
        strFieldNames(8) = "範囲１"                   '9  範囲１
        strFieldNames(9) = "範囲２"                   '10 範囲２
        strFieldNames(10) = "分類１"                  '11 分類１
        strFieldNames(11) = "分類２"                  '12 分類２
        strFieldNames(12) = "分類使用方法"            '13 分類使用方法
        strFieldNames(13) = "flgOnlyOne"              '14 flgOnlyOne
        strFieldNames(14) = "TenGunName"              '15 TenGunName
        strFieldNames(15) = "備考"                    '16 備考
    End Sub

    '点群定義データのイメージ作成（フィールド値）
    Private Sub CreateTengunFieldText(ByVal TengunD As TenGunTeigiTable)
        ReDim strFieldTexts(15)

        strFieldTexts(0) = CommonTypeID                        '1  TypeID
        strFieldTexts(1) = TengunD.TenGunID                    '2  点群ＩＤ
        strFieldTexts(2) = TengunD.MotoGunID                   '3  元点群ＩＤ
        strFieldTexts(3) = TengunD.SyoriHouhouID               '4  処理方法ＩＤ
        strFieldTexts(4) = TengunD.SortingHouhou               '5  ソーティング順
        strFieldTexts(5) = TengunD.objXYZ                      '6  ＸＹＺ成分
        strFieldTexts(6) = TengunD.BasePointID                 '7  基準点ＩＤ
        strFieldTexts(7) = TengunD.objDaisyo                   '8  大小指定
        strFieldTexts(8) = TengunD.Index1                      '9  範囲１
        strFieldTexts(9) = TengunD.Index2                      '10 範囲２
        strFieldTexts(10) = TengunD.Bunrui1                    '11 分類１
        strFieldTexts(11) = TengunD.Bunrui2                    '12 分類２
        strFieldTexts(12) = TengunD.flgBunrui                  '13 分類使用方法
        strFieldTexts(13) = "'" & TengunD.flgOnlyOne & "'"     '14 flgOnlyOne
        strFieldTexts(14) = "'" & TengunD.strName & "'"        '15 TenGunName
        strFieldTexts(15) = "'" & TengunD.strBikou & "'"       '16 備考
    End Sub

    Private Sub Grid_Kijun_CellEditEnding(sender As System.Object, e As System.Windows.Controls.DataGridCellEditEndingEventArgs) Handles Grid_Kijun.CellEditEnding
        Dim ScaleD As New ScaleItem
        Dim iColumn As Integer
        Dim GetScaleD As New ScaleItem
        Dim ii, jj As Integer
        Dim iFlg As Integer
        Dim strMsg As String
        Dim strDbSupo As String = ""
        Dim iDbTarget As Integer

        If e.Row.Item IsNot Nothing Then
            ScaleD = e.Row.Item
            iColumn = e.Column.DisplayIndex
        End If

        For ii = 0 To m_Scalecount - 1
            If ScaleD.SunpoID = arrScaleData(ii).SunpoID Then
                GetScaleD = arrScaleData(ii)
                Exit For
            End If
        Next ii

        If iColumn = 3 Then
            Dim dKitei As Double
            iFlg = 0
            Try
                dKitei = CDbl(ScaleD.KiteiVal)
                If dKitei < 0.0 Then
                    iFlg = 1
                    If ScaleD.OpeFlg = 3 Then
                        ScaleD.KiteiVal = edtScaleLng.ToString
                    Else
                        For ii = 0 To m_ScaleItems.Count - 1
                            If m_ScaleItems(ii).SunpoID = ScaleD.SunpoID Then
                                m_ScaleItems(ii).KiteiVal = GetScaleD.KiteiVal.ToString
                                Exit For
                            End If
                        Next ii
                    End If
                End If
            Catch ex As Exception
                iFlg = 1
                If ScaleD.OpeFlg = 3 Then
                    ScaleD.KiteiVal = edtScaleLng.ToString
                Else
                    For ii = 0 To m_ScaleItems.Count - 1
                        If m_ScaleItems(ii).SunpoID = ScaleD.SunpoID Then
                            m_ScaleItems(ii).KiteiVal = GetScaleD.KiteiVal.ToString
                            Exit For
                        End If
                    Next ii
                End If
            End Try
            If iFlg = 1 Then
                MsgBox("長さには0より大きい数値を入力してください。", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "スケール入力エラー")
                Exit Sub
            End If
        End If

        If (iColumn = 4) Or (iColumn = 5) Then
            iFlg = 0
            If (ScaleD.OpeFlg = 3) Or ((iColumn = 4) And (GetScaleD.StartTarget <> ScaleD.StartTarget)) Or _
               ((iColumn = 5) And (GetScaleD.EndTarget <> ScaleD.EndTarget)) Then
                If iColumn = 4 Then
                    If ScaleD.StartTarget Is Nothing Then
                        iFlg = 3
                    Else
                        iDbTarget = CInt(ScaleD.StartTarget)
                        If CInt(ScaleD.StartTarget) = CInt(ScaleD.EndTarget) Then
                            iFlg = 2
                        Else
                            For jj = 0 To m_Scalecount - 1
                                If ScaleD.SunpoID <> arrScaleData(jj).SunpoID Then
                                    If (ScaleD.StartTarget = arrScaleData(jj).StartTarget) Or (CInt(ScaleD.StartTarget) = CInt(arrScaleData(jj).EndTarget)) Then
                                        iFlg = 1
                                        strDbSupo = arrScaleData(jj).SunpoMark
                                        Exit For
                                    End If
                                End If
                            Next jj
                        End If
                    End If
                ElseIf iColumn = 5 Then
                    If ScaleD.EndTarget Is Nothing Then
                        iFlg = 3
                    Else
                        iDbTarget = CInt(ScaleD.EndTarget)
                        If CInt(ScaleD.EndTarget) = CInt(ScaleD.StartTarget) Then
                            iFlg = 2
                        Else
                            For jj = 0 To m_Scalecount - 1
                                If ScaleD.SunpoID <> arrScaleData(jj).SunpoID Then
                                    If (CInt(ScaleD.EndTarget) = CInt(arrScaleData(jj).StartTarget)) Or (CInt(ScaleD.EndTarget) = CInt(arrScaleData(jj).EndTarget)) Then
                                        iFlg = 1
                                        strDbSupo = arrScaleData(jj).SunpoMark
                                        Exit For
                                    End If
                                End If
                            Next jj
                        End If
                    End If
                End If
            End If

            If iFlg > 0 Then
                If iFlg = 1 Then
                    strMsg = "ターゲット番号" & iDbTarget & "は、" & ScaleD.SunpoMark & "と" & strDbSupo & "で重複しています。"
                ElseIf iFlg = 2 Then
                    strMsg = "寸法マーク" & ScaleD.SunpoMark & "は、始点と終点のターゲット番号が同じです。"
                Else
                    strMsg = "ターゲット番号が登録されていないか、ターゲット番号の入力が不正です。"
                End If
                MsgBox(strMsg, MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "スケール入力エラー")
                For ii = 0 To m_ScaleItems.Count - 1
                    If m_ScaleItems(ii).SunpoID = ScaleD.SunpoID Then
                        If iColumn = 4 Then
                            If m_ScaleItems(ii).OpeFlg = 3 Then
                                m_ScaleItems(ii).StartTarget = edtScaleSTNo.ToString
                            Else
                                m_ScaleItems(ii).StartTarget = GetScaleD.StartTarget
                            End If
                        Else
                            If m_ScaleItems(ii).OpeFlg = 3 Then
                                m_ScaleItems(ii).EndTarget = edtScaleEDNo.ToString
                            Else
                                m_ScaleItems(ii).EndTarget = GetScaleD.EndTarget
                            End If
                        End If
                        Exit For
                    End If
                Next ii

            End If

                '    'ADD By Yamada 20150327 Sta -------------
                'ElseIf iColumn = 3 Then



                '    'ADD By Yamada 20150327 End -------------
        End If

    End Sub

    'Private Sub Grid_Kijun_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles Grid_Kijun.SelectionChanged
    '    Dim ScaleD As New ScaleItem
    '    ScaleD = Grid_Kijun.CurrentItem
    'End Sub

    'Private Sub Grid_Line_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles Grid_Line.SelectionChanged
    '    Dim LineD As LineLengthItem
    '    LineD = Grid_Line.CurrentItem
    'End Sub

    Private Sub Grid_Line_CellEditEnding(sender As System.Object, e As System.Windows.Controls.DataGridCellEditEndingEventArgs) Handles Grid_Line.CellEditEnding
        Dim LineD As New LineLengthItem
        Dim iColumn As Integer
        Dim GetLineD As New LineLengthItem
        Dim ii, jj As Integer
        Dim iFlg As Integer
        Dim strMsg As String
        Dim strDbSupo As String = ""
        Dim iDbTarget As Integer

        If e.Row.Item IsNot Nothing Then
            LineD = e.Row.Item
            iColumn = e.Column.DisplayIndex
        End If

        If (iColumn = 3) Or (iColumn = 4) Then
            iFlg = 0
            For ii = 0 To m_LIneLengthcount - 1
                If LineD.SunpoID = arrLineLegthData(ii).SunpoID Then
                    GetLineD = arrLineLegthData(ii)
                    Exit For
                End If
            Next ii

            If ((iColumn = 3) And (GetLineD.StartTarget <> LineD.StartTarget)) Or _
               ((iColumn = 4) And (GetLineD.EndTarget <> LineD.EndTarget)) Then
                If iColumn = 3 Then
                    If LineD.StartTarget Is Nothing Then
                        iFlg = 3
                    Else
                        iDbTarget = CInt(LineD.StartTarget)
                        If LineD.StartTarget = LineD.EndTarget Then
                            iFlg = 2
                        Else
                            For jj = 0 To m_LIneLengthcount - 1
                                If LineD.SunpoID <> arrLineLegthData(jj).SunpoID Then
                                    If (LineD.StartTarget = arrLineLegthData(jj).StartTarget) Or (LineD.StartTarget = arrLineLegthData(jj).EndTarget) Then
                                        iFlg = 1
                                        strDbSupo = arrLineLegthData(jj).SunpoMark
                                        Exit For
                                    End If
                                End If
                            Next jj
                        End If
                    End If
                ElseIf iColumn = 4 Then
                    If LineD.EndTarget Is Nothing Then
                        iFlg = 3
                    Else
                        iDbTarget = CInt(LineD.EndTarget)
                        If LineD.EndTarget = LineD.StartTarget Then
                            iFlg = 2
                        Else
                            For jj = 0 To m_Scalecount - 1
                                If LineD.SunpoID <> arrLineLegthData(jj).SunpoID Then
                                    If (LineD.EndTarget = arrLineLegthData(jj).StartTarget) Or (LineD.EndTarget = arrLineLegthData(jj).EndTarget) Then
                                        iFlg = 1
                                        strDbSupo = arrLineLegthData(jj).SunpoMark
                                        Exit For
                                    End If
                                End If
                            Next jj
                        End If
                    End If
                End If
            End If

            If iFlg > 0 Then
                If iFlg = 1 Then
                    strMsg = "ターゲット番号" & iDbTarget & "は、" & LineD.SunpoMark & "と" & strDbSupo & "で重複しています。"
                ElseIf iFlg = 2 Then
                    strMsg = "寸法マーク" & LineD.SunpoMark & "は、始点と終点のターゲット番号が同じです。"
                Else
                    strMsg = "ターゲット番号が登録されていないか、ターゲット番号の入力が不正です。"
                End If
                MsgBox(strMsg, MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "２点線分ターゲット番入力エラー")
                For ii = 0 To m_LineLengthItems.Count - 1
                    If m_LineLengthItems(ii).SunpoID = LineD.SunpoID Then
                        If iColumn = 3 Then
                            m_LineLengthItems(ii).StartTarget = GetLineD.StartTarget
                        Else
                            m_LineLengthItems(ii).EndTarget = GetLineD.EndTarget
                        End If
                        Exit For
                    End If
                Next ii

            End If
        End If

    End Sub

    'ポリラインのターゲット入力チェック
    Private Sub Grid_PolyLine_CellEditEnding(sender As System.Object, e As System.Windows.Controls.DataGridCellEditEndingEventArgs) Handles Grid_PolyLine.CellEditEnding
        Dim ii, jj, kk, ll As Integer
        Dim GetPolyLineD As New PolyLineItem
        Dim PolyLineD As New PolyLineItem
        Dim iCurumn As Integer = 0
        Dim iCnt As Integer = 0
        Dim arrTarget(100) As Integer
        Dim iCnt2 As Integer = 0
        Dim arrTarget2(100) As Integer
        Dim iCnt3 As Integer = 0
        Dim arrTarget3(100) As Integer
        Dim arrFlg(100) As Integer
        Dim iFFlg As Integer
        Dim strNotFound As String
        Dim strUse As String
        Dim strMsg As String = ""
        Dim strMsg2 As String = ""
        Dim arrTarget4(100) As Integer

        'カレントカラム番号、カレントアイテムの取得
        If e.Row.Item IsNot Nothing Then
            iCurumn = e.Column.DisplayIndex
            PolyLineD = e.Row.Item
        End If

        'カレントカラム番号が３の時のみチェックを行う
        If iCurumn = 3 Then
            '変更前ポリラインデータ取得
            For ii = 0 To m_PolyLinecount - 1
                If PolyLineD.SunpoID = arrPolyLineData(ii).SunpoID Then
                    GetPolyLineD = arrPolyLineData(ii)
                    Exit For
                End If
            Next ii

            If GetPolyLineD IsNot Nothing Then
                '変更前、変更後でターゲット文字列が異なっている時のみチェックを行う
                If GetPolyLineD.VertexTarget <> PolyLineD.VertexTarget Then
                    '変更前、変更後のターゲット番号取得
                    iCnt = GetNumber_TargetString(PolyLineD.VertexTarget, arrTarget)


                    'ターゲット文字列入力エラー処理


                    'Rep By Yamada 20150327 Sta -------
                    'If iCnt < 0 Then
                    'If iCnt = -999 Then
                    '    strMsg = "頂点TG番号リストは "",""、""-""、及び 0 より大きい整数のみ入力してください"
                    'ElseIf iCnt = -998 Then
                    '    strMsg = "頂点TG番号リストの入力が不正です"
                    'Else
                    If iCnt <= 0 Then
                        If iCnt = 0 Or iCnt = -999 Then
                            strMsg = "頂点TG番号リストは "",""、""-""、及び 0 より大きい整数のみ入力してください"
                        ElseIf iCnt = -998 Then
                            strMsg = "頂点TG番号リストの入力が不正です"
                        Else
                            'Rep By Yamada 20150327 End -------


                            For ii = 0 To Math.Abs(iCnt) - 1
                                If ii = 0 Then
                                    strMsg2 = arrTarget(ii)
                                Else
                                    strMsg2 = strMsg2 & "," & arrTarget(ii)
                                End If
                            Next ii

                            'Rep By Yamada 20150327 Sta------------------------------------
                            'strMsg = "ターゲット番号 (" & strMsg2 & ") が重複しています。"
                            strMsg = "寸法マーク (" & PolyLineD.SunpoMark & ") でターゲット番号 (" & strMsg2 & ") が重複しています。"
                            'Rep By Yamada 20150327 Sta------------------------------------

                        End If
                        MsgBox(strMsg, MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "頂点TG番号リスト入力エラー")
                        For ii = 0 To m_PolyLineItems.Count - 1
                            If m_PolyLineItems.Item(ii).SunpoID = PolyLineD.SunpoID Then
                                If PolyLineD.OpeFlg = 3 Then
                                    m_PolyLineItems.Item(ii).VertexTarget = ""
                                Else
                                    m_PolyLineItems.Item(ii).VertexTarget = GetPolyLineD.VertexTarget
                                End If
                                Exit For
                            End If
                        Next ii
                        Exit Sub
                    End If

                    If PolyLineD.OpeFlg = 3 Then
                        iCnt2 = 0
                    Else
                        iCnt2 = GetNumber_TargetString(GetPolyLineD.VertexTarget, arrTarget2)
                    End If
                    If (iCnt > 0) And (iCnt2 > 0) Then
                        '変更後のターゲット番号が、変更前のターゲット番号内に含まれるかチェック
                        iCnt3 = 0
                        For ii = 0 To iCnt - 1
                            iFFlg = 0
                            For jj = 0 To iCnt2 - 1
                                If arrTarget(ii) = arrTarget2(jj) Then
                                    iFFlg = 1
                                    Exit For
                                End If
                            Next jj
                            If iFFlg = 0 Then
                                arrTarget3(iCnt3) = arrTarget(ii)
                                iCnt3 += 1
                            End If
                        Next ii
                        If iCnt3 = 0 Then
                            Exit Sub
                        End If
                    ElseIf iCnt > 0 Then
                        For ii = 0 To iCnt - 1
                            arrTarget3(ii) = arrTarget(ii)
                        Next ii
                        iCnt3 = iCnt
                    End If

                    If iCnt3 > 0 Then
                        '追加されたターゲット番号が存在するか、存在する場合は使用中かチェック
                        For ii = 0 To iCnt3 - 1
                            arrFlg(ii) = 0
                            iFFlg = 0
                            For jj = 0 To m_TagetSetItems.Count - 1
                                If arrTarget3(ii) = CInt(m_TagetSetItems.Item(jj).TargetNumber) Then
                                    'If m_TagetSetItems.Item(jj).UseFlg = 1 Then
                                    '    arrFlg(ii) = 2
                                    'End If
                                    For kk = 0 To m_PolyLinecount - 1
                                        If PolyLineD.SunpoID <> arrPolyLineData(kk).SunpoID Then
                                            iCnt = GetNumber_TargetString(arrPolyLineData(kk).VertexTarget, arrTarget4)
                                            For ll = 0 To iCnt - 1
                                                If arrTarget3(ii) = arrTarget4(ll) Then
                                                    arrFlg(ii) = 2
                                                    Exit For
                                                End If
                                            Next ll
                                        End If
                                    Next kk
                                    iFFlg = 2
                                    Exit For
                                End If
                            Next jj
                            If iFFlg = 0 Then
                                arrFlg(ii) = 1
                            End If
                        Next ii
                        '登録されていないターゲット、使用中のターゲットの文字列作成
                        strNotFound = ""
                        strUse = ""
                        For ii = 0 To iCnt3 - 1
                            If arrFlg(ii) = 1 Then
                                strNotFound = strNotFound & arrTarget3(ii).ToString & " "
                            ElseIf arrFlg(ii) = 2 Then
                                strUse = strUse & arrTarget3(ii).ToString & " "
                            End If
                        Next ii
                        '登録されていない、使用中のターゲットが存在する時、メッセージを表示
                        If (strNotFound <> "") Or (strUse <> "") Then
                            strMsg = "ターゲット指定に次のエラーがあるため、入力キャンセルします。"
                            If strNotFound <> "" Then
                                strMsg = strMsg & vbCrLf & "    <ターゲット番号：" & strNotFound & "> は、登録されていません。"
                            End If
                            If strUse <> "" Then
                                strMsg = strMsg & vbCrLf & "    <ターゲット番号：" & strUse & "> は、他のポリライン要素で使用されています。"
                            End If
                            MsgBox(strMsg, MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "ターゲット入力エラー")
                            For ii = 0 To m_PolyLineItems.Count - 1
                                If m_PolyLineItems.Item(ii).SunpoID = PolyLineD.SunpoID Then
                                    If PolyLineD.OpeFlg = 3 Then
                                        m_PolyLineItems.Item(ii).VertexTarget = ""
                                    Else
                                        m_PolyLineItems.Item(ii).VertexTarget = GetPolyLineD.VertexTarget
                                    End If
                                    Exit For
                                End If
                            Next ii
                        End If
                    End If
                End If
            End If
        End If

    End Sub

    '１点円のターゲット入力チェック
    Private Sub Grid_Circle_CellEditEnding(sender As System.Object, e As System.Windows.Controls.DataGridCellEditEndingEventArgs) Handles Grid_Circle.CellEditEnding
        Dim ii, jj As Integer
        Dim GetCircleD As New CircleItem
        Dim CircleD As New CircleItem
        Dim iCurumn As Integer = 0
        Dim iCnt As Integer = 0
        Dim arrTarget(100) As Integer
        Dim iCnt2 As Integer = 0
        Dim arrTarget2(100) As Integer
        Dim iCnt3 As Integer = 0
        Dim arrTarget3(100) As Integer
        Dim arrFlg(100) As Integer
        Dim iFFlg As Integer
        Dim strNotFound As String
        Dim strUse As String
        Dim strMsg As String = ""
        Dim strMsg2 As String = ""
        Dim arrTarget4(100) As Integer

        'カレントカラム番号、カレントアイテムの取得
        If e.Row.Item IsNot Nothing Then
            iCurumn = e.Column.DisplayIndex
            CircleD = e.Row.Item
        End If

        'カレントカラム番号が３の時のみチェックを行う
        If iCurumn = 3 Then
            '変更前ポリラインデータ取得
            For ii = 0 To m_Circlecount - 1
                If CircleD.SunpoID = arrCircleData(ii).SunpoID Then
                    GetCircleD = arrCircleData(ii)
                    Exit For
                End If
            Next ii

            If GetCircleD IsNot Nothing Then
                '変更前、変更後でターゲット文字列が異なっている時のみチェックを行う
                If GetCircleD.CenterTarget <> CircleD.CenterTarget Then
                    '変更前、変更後のターゲット番号取得
                    iCnt = GetNumber_TargetString(CircleD.CenterTarget, arrTarget)

                    'ターゲット文字列入力エラー処理

                    'Rep By Yamada 20150327 Sta ---------------

                    'If iCnt < 0 Then
                    '    If iCnt = -999 Then
                    '        strMsg = "中心点TG番号リストは "",""、""-""、及び 0 より大きい整数のみ入力してください"
                    '    ElseIf iCnt = -998 Then
                    '        strMsg = "中心点TG番号リストの入力が不正です"
                    '    Else

                    If iCnt <= 0 Then
                        If iCnt = 0 Or iCnt = -999 Then
                            strMsg = "中心点TG番号リストは "",""、""-""、及び 0 より大きい整数のみ入力してください"
                        ElseIf iCnt = -998 Then
                            strMsg = "中心点TG番号リストの入力が不正です"
                        Else
                            'Rep By Yamada 20150327 End ---------------

                            For ii = 0 To Math.Abs(iCnt) - 1
                                If ii = 0 Then
                                    strMsg2 = arrTarget(ii)
                                Else
                                    strMsg2 = strMsg2 & "," & arrTarget(ii)
                                End If
                            Next ii

                            'Rep By Yamada 20150327 Sta -------------------------------------
                            'strMsg = "ターゲット番号 (" & strMsg2 & ") が重複しています。"
                            strMsg = "寸法マーク (" & CircleD.SunpoMark & ") でターゲット番号 (" & strMsg2 & ") が重複しています。"
                            'Rep By Yamada 20150327 Sta -------------------------------------

                        End If
                        MsgBox(strMsg, MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "中心点TG番号リスト入力エラー")
                        For ii = 0 To m_CircleItems.Count - 1
                            If m_CircleItems.Item(ii).SunpoID = CircleD.SunpoID Then
                                If CircleD.OpeFlg = 3 Then
                                    m_CircleItems.Item(ii).CenterTarget = ""
                                Else
                                    m_CircleItems.Item(ii).CenterTarget = GetCircleD.CenterTarget
                                End If
                                Exit For
                            End If
                        Next ii
                        Exit Sub
                    End If

                    If CircleD.OpeFlg = 3 Then
                        iCnt2 = 0
                    Else
                        iCnt2 = GetNumber_TargetString(GetCircleD.CenterTarget, arrTarget2)
                    End If
                    If (iCnt > 0) And (iCnt2 > 0) Then
                        '変更後のターゲット番号が、変更前のターゲット番号内に含まれるかチェック
                        iCnt3 = 0
                        For ii = 0 To iCnt - 1
                            iFFlg = 0
                            For jj = 0 To iCnt2 - 1
                                If arrTarget(ii) = arrTarget2(jj) Then
                                    iFFlg = 1
                                    Exit For
                                End If
                            Next jj
                            If iFFlg = 0 Then
                                arrTarget3(iCnt3) = arrTarget(ii)
                                iCnt3 += 1
                            End If
                        Next ii
                        If iCnt3 = 0 Then
                            Exit Sub
                        End If
                    ElseIf iCnt > 0 Then
                        For ii = 0 To iCnt - 1
                            arrTarget3(ii) = arrTarget(ii)
                        Next ii
                        iCnt3 = iCnt
                    End If

                    If iCnt3 > 0 Then
                        '追加されたターゲット番号が存在するか、存在する場合は使用中かチェック
                        For ii = 0 To iCnt3 - 1
                            arrFlg(ii) = 0
                            iFFlg = 0
                            For jj = 0 To m_TagetSetItems.Count - 1
                                If arrTarget3(ii) = CInt(m_TagetSetItems.Item(jj).TargetNumber) Then
                                    'If m_TagetSetItems.Item(jj).UseFlg = 1 Then
                                    '    arrFlg(ii) = 2
                                    'End If
                                    For kk = 0 To m_Circlecount - 1
                                        If CircleD.SunpoID <> arrCircleData(kk).SunpoID Then
                                            iCnt = GetNumber_TargetString(arrCircleData(kk).CenterTarget, arrTarget4)
                                            For ll = 0 To iCnt - 1
                                                If arrTarget3(ii) = arrTarget4(ll) Then
                                                    arrFlg(ii) = 2
                                                    Exit For
                                                End If
                                            Next ll
                                            'Exit For
                                        End If
                                    Next kk
                                    iFFlg = 2
                                    Exit For
                                End If
                            Next jj
                            If iFFlg = 0 Then
                                arrFlg(ii) = 1
                            End If
                        Next ii
                        '登録されていないターゲット、使用中のターゲットの文字列作成
                        strNotFound = ""
                        strUse = ""
                        For ii = 0 To iCnt3 - 1
                            If arrFlg(ii) = 1 Then
                                strNotFound = strNotFound & arrTarget3(ii).ToString & " "
                            ElseIf arrFlg(ii) = 2 Then
                                strUse = strUse & arrTarget3(ii).ToString & " "
                            End If
                        Next ii
                        '登録されていない、使用中のターゲットが存在する時、メッセージを表示
                        If (strNotFound <> "") Or (strUse <> "") Then
                            strMsg = "ターゲット指定に次のエラーがあるため、入力キャンセルします。"
                            If strNotFound <> "" Then
                                strMsg = strMsg & vbCrLf & "    <ターゲット番号：" & strNotFound & "> は、登録されていません。"
                            End If
                            If strUse <> "" Then
                                strMsg = strMsg & vbCrLf & "    <ターゲット番号：" & strUse & "> は、他の1点円要素で使用されています。"
                            End If
                            MsgBox(strMsg, MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "ターゲット入力エラー")
                            For ii = 0 To m_CircleItems.Count - 1
                                If m_CircleItems.Item(ii).SunpoID = CircleD.SunpoID Then
                                    If CircleD.OpeFlg = 3 Then
                                        m_CircleItems.Item(ii).CenterTarget = ""
                                    Else
                                        m_CircleItems.Item(ii).CenterTarget = GetCircleD.CenterTarget
                                    End If
                                    Exit For
                                End If
                            Next ii
                        End If
                    End If
                End If
            End If
        End If

        '4列（直径）の時のチェック
        If iCurumn = 4 Then
            '変更前ポリラインデータ取得
            For ii = 0 To m_Circlecount - 1
                If CircleD.SunpoID = arrCircleData(ii).SunpoID Then
                    GetCircleD = arrCircleData(ii)
                    Exit For
                End If
            Next ii

            If GetCircleD IsNot Nothing Then
                Dim dKitei As Double
                Dim iFlg As Integer = 0
                Try
                    dKitei = CDbl(CircleD.Diameter)
                    If dKitei < 0.0 Then
                        iFlg = 1
                        If CircleD.OpeFlg = 3 Then
                            CircleD.Diameter = "10.0"
                        Else
                            For ii = 0 To m_CircleItems.Count - 1
                                If m_CircleItems(ii).SunpoID = CircleD.SunpoID Then
                                    m_CircleItems(ii).Diameter = GetCircleD.Diameter.ToString
                                    Exit For
                                End If
                            Next ii
                        End If
                    End If
                Catch ex As Exception
                    iFlg = 1
                    If CircleD.OpeFlg = 3 Then
                        CircleD.Diameter = "10.0"
                    Else
                        For ii = 0 To m_CircleItems.Count - 1
                            If m_CircleItems(ii).SunpoID = CircleD.SunpoID Then
                                m_CircleItems(ii).Diameter = GetCircleD.Diameter.ToString
                                Exit For
                            End If
                        Next ii
                    End If
                End Try
                If iFlg = 1 Then
                    MsgBox("直径には0より大きい数値を入力してください。", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "１点円入力エラー")
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Private Sub Grid_Target_CellEditEnding(sender As System.Object, e As System.Windows.Controls.DataGridCellEditEndingEventArgs) Handles Grid_Target.CellEditEnding
        Dim ii As Integer
        Dim GetTargetD As New CTBunruiDB
        Dim TargetD As New TargetSetItem
        Dim iCurumn As Integer = 0
        Dim iFFlg As Integer
        Dim strMsg As String
        Dim ist As Integer

        'イベント発生アイテムが存在する時のみ処理を行う
        If e.Row.Item IsNot Nothing Then
            'イベントの発生したカラム番号、アイテムの取得
            iCurumn = e.Column.DisplayIndex
            TargetD = e.Row.Item

            For ii = 0 To m_Targetcount - 1
                If arrCToffsetData(ii).ID = TargetD.ID Then
                    GetTargetD = arrCToffsetData(ii)
                    ist = ii
                    Exit For
                End If
            Next ii

            'オフセット値チェック
            Dim dTarget As Double
            Try
                dTarget = CDbl(TargetD.TargetOffsetX)
            Catch ex As Exception
                For ii = 0 To m_TagetSetItems.Count - 1
                    If m_TagetSetItems.Item(ii).ID = TargetD.ID Then
                        m_TagetSetItems.Item(ii).TargetOffsetX = GetTargetD.m_x.ToString
                        Exit For
                    End If
                Next ii
                MsgBox("オフセットXには数値のみ入力してください。", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "ターゲット入力エラー")
                'beforeTargetItem = Nothing
                Exit Sub
            End Try
            Try
                dTarget = CDbl(TargetD.TargetOffsetY)
            Catch ex As Exception
                For ii = 0 To m_TagetSetItems.Count - 1
                    If m_TagetSetItems.Item(ii).ID = TargetD.ID Then
                        m_TagetSetItems.Item(ii).TargetOffsetY = GetTargetD.m_y.ToString
                        Exit For
                    End If
                Next ii
                MsgBox("オフセットYには数値のみ入力してください。", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "ターゲット入力エラー")
                'beforeTargetItem = Nothing
                Exit Sub
            End Try
            Try
                dTarget = CDbl(TargetD.TargetOffsetZ)
            Catch ex As Exception
                For ii = 0 To m_TagetSetItems.Count - 1
                    If m_TagetSetItems.Item(ii).ID = TargetD.ID Then
                        m_TagetSetItems.Item(ii).TargetOffsetZ = GetTargetD.m_z.ToString
                        beforeTargetItem = Nothing
                        Exit For
                    End If
                Next ii
                MsgBox("オフセットZには数値のみ入力してください。", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "ターゲット入力エラー")
                'beforeTargetItem = Nothing
                Exit Sub
            End Try

            'ターゲット番号チェック
            iFFlg = 0

            'Rep By Yamada 20150320 Sta ----------------
            Try
                If CInt(TargetD.TargetNumber) < 1 Then
                    'TG番号が0以下の時
                    iFFlg = 3

                ElseIf CInt(TargetD.TargetNumber) <> GetTargetD.CT_NO Then
                    If TargetD.OpeFlg = 3 Then
                        For ii = 0 To m_Targetcount - 1
                            If ii = ist Then Continue For
                            If arrCToffsetData(ii).CT_NO = CInt(TargetD.TargetNumber) Then
                                iFFlg = 1
                                Exit For
                            End If
                        Next ii
                    ElseIf iCurumn = 1 Then
                        iFFlg = 2
                    End If
                End If
            Catch ex As Exception
                iFFlg = 3
            End Try

            'If TargetD.TargetNumber <> GetTargetD.CT_NO Then
            '    If TargetD.OpeFlg = 3 Then
            '        For ii = 0 To m_Targetcount - 1
            '            If ii = ist Then Continue For
            '            If arrCToffsetData(ii).CT_NO = TargetD.TargetNumber Then
            '                iFFlg = 1
            '                Exit For
            '            End If
            '        Next ii
            '    ElseIf iCurumn = 1 Then
            '        iFFlg = 2
            '    End If
            'End If
            'Rep By Yamada 20150320 End ----------------


            '登録されていない、使用中のターゲットが存在する時、メッセージを表示
            If iFFlg > 0 Then
                strMsg = ""
                If iFFlg = 1 Then
                    strMsg = "<ターゲット番号：" & TargetD.TargetNumber.ToString & "> は、既に登録されています。"
                ElseIf iFFlg = 2 Then
                    strMsg = "追加したターゲット以外は、ターゲット番号の変更はできません。"

                    'ADD By Yamada 20150320 Sta ---------
                ElseIf iFFlg = 3 Then
                    strMsg = "ターゲット番号は0より大きな整数を入力してください。"
                    'ADD By Yamada 20150320 End ---------
                End If

                MsgBox(strMsg, MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "ターゲット入力エラー")
                For ii = 0 To m_TagetSetItems.Count - 1
                    If m_TagetSetItems.Item(ii).ID = TargetD.ID Then
                        If TargetD.OpeFlg = 3 Then
                            m_TagetSetItems.Item(ii).TargetNumber = GetTargetD.CT_NO.ToString
                        Else
                            m_TagetSetItems.Item(ii).TargetNumber = GetTargetD.CT_NO.ToString
                        End If
                        Exit For
                    End If
                Next ii
                'ADD By Yamada 20150320 Sta -
                '入力をキャンセル
                'e.Cancel = True
                'ADD By Yamada 20150320 End -
            End If

        End If

        'Dell 
        '編集が終わったのでKeyPressイベントの関連付けを解除
        'RemoveHandler TextEditCtrl.PreviewKeyDown, AddressOf TextEditCtrl_KeyPress_Grid_Target

    End Sub

    Private Sub Grid_zahyou_CellEditEnding(sender As System.Object, e As System.Windows.Controls.DataGridCellEditEndingEventArgs) Handles Grid_zahyou.CellEditEnding
        Dim ZahyouD As New ZahyouItem
        Dim GetZahyouD As New ZahyouItem
        Dim iColumn As Integer
        Dim ii As Integer
        Dim iFlg As Integer = 0
        Dim strMsg As String = ""

        'カレントカラム番号、カレントアイテムの取得
        If e.Row.Item IsNot Nothing Then
            iColumn = e.Column.DisplayIndex
            ZahyouD = e.Row.Item
        End If

        If ZahyouD.CT_No Is Nothing Then
            iFlg = 2
        Else
            For ii = 0 To 6
                If ZahyouD.ID = arrZahyouGet(ii).ID Then
                    GetZahyouD = arrZahyouGet(ii)
                    Exit For
                End If
            Next ii

            For ii = 0 To m_ZahyouItems.Count - 1
                If ZahyouD.ID <> m_ZahyouItems(ii).ID Then
                    If ZahyouD.CT_No = m_ZahyouItems(ii).CT_No Then
                        iFlg = 1
                        Exit For
                    End If
                End If
            Next ii
        End If

        If iFlg > 0 Then
            If iFlg = 1 Then
                strMsg = "ターゲット" & ZahyouD.CT_No & "番は重複しています。"
            Else
                strMsg = "ターゲット番号が登録されていないか、ターゲット番号の入力が不正です。"
            End If
            MsgBox(strMsg, MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "座標系ターゲット入力エラー")
            'e.Cancel = True
            For ii = 0 To m_ZahyouItems.Count - 1
                If ZahyouD.ID = m_ZahyouItems(ii).ID Then
                    m_ZahyouItems(ii).CT_No = edtZahyouCTNo.ToString
                    Exit For
                End If
            Next ii
        End If
    End Sub

    Private Sub Combo_Zahyou_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles Combo_Zahyou.SelectionChanged

        If arrZahyouData.XYorXZorYZ <> Combo_Zahyou.SelectedIndex Then
            arrZahyouData.XYorXZorYZ = Combo_Zahyou.SelectedIndex
            arrZahyouData.OpeFlg = 1

            With m_ZahyouItems
                .Clear()
                Select Case Combo_Zahyou.SelectedIndex
                    Case 0
                        .Add(New ZahyouItem)
                        .Item(0) = arrZahyouGet(0)
                        .Item(0).CT_Type = "原点TG"
                        .Add(New ZahyouItem)
                        .Item(1) = arrZahyouGet(1)
                        .Item(1).CT_Type = "X軸TG(正)"
                        .Add(New ZahyouItem)
                        .Item(2) = arrZahyouGet(2)
                        .Item(2).CT_Type = "Y方向TG(正)"
                        Label_Height.Visibility = Windows.Visibility.Hidden
                        Text_Height.Visibility = Windows.Visibility.Hidden
                        Label_Unit.Visibility = Windows.Visibility.Hidden
                    Case 1
                        .Add(New ZahyouItem)
                        .Item(0) = arrZahyouGet(0)
                        .Item(0).CT_Type = "原点TG(キングピン)"
                        .Add(New ZahyouItem)
                        .Item(1) = arrZahyouGet(1)
                        .Item(1).CT_Type = "X軸TG①"
                        .Add(New ZahyouItem)
                        .Item(2) = arrZahyouGet(2)
                        .Item(2).CT_Type = "X軸TG②"
                        .Add(New ZahyouItem)
                        .Item(3) = arrZahyouGet(4)
                        .Item(3).CT_Type = "平面TG①"
                        .Add(New ZahyouItem)
                        .Item(4) = arrZahyouGet(5)
                        .Item(4).CT_Type = "平面TG②"
                        .Add(New ZahyouItem)
                        .Item(5) = arrZahyouGet(6)
                        .Item(5).CT_Type = "平面TG③"
                        Text_Height.Text = Zahyou_HData.Zahyou_Height.ToString
                        Label_Height.Visibility = Windows.Visibility.Visible
                        Text_Height.Visibility = Windows.Visibility.Visible
                        Label_Unit.Visibility = Windows.Visibility.Visible
                        If Zahyou_HData.SunpoID = -1 Then
                            Zahyou_HData.SunpoID = MaxSunpoID + 1
                            Zahyou_HData.OpeFlg = 3
                            MaxSunpoID += 1
                        End If
                    Case 2
                        .Add(New ZahyouItem)
                        .Item(0) = arrZahyouGet(0)
                        .Item(0).CT_Type = "原点TG"
                        .Add(New ZahyouItem)
                        .Item(1) = arrZahyouGet(1)
                        .Item(1).CT_Type = "X軸TG①"
                        .Add(New ZahyouItem)
                        .Item(2) = arrZahyouGet(2)
                        .Item(2).CT_Type = "X軸TG②"
                        .Add(New ZahyouItem)
                        .Item(3) = arrZahyouGet(4)
                        .Item(3).CT_Type = "平面TG①"
                        .Add(New ZahyouItem)
                        .Item(4) = arrZahyouGet(5)
                        .Item(4).CT_Type = "平面TG②"
                        .Add(New ZahyouItem)
                        .Item(5) = arrZahyouGet(6)
                        .Item(5).CT_Type = "平面TG③"
                        Label_Height.Visibility = Windows.Visibility.Hidden
                        Text_Height.Visibility = Windows.Visibility.Hidden
                        Label_Unit.Visibility = Windows.Visibility.Hidden
                    Case 3
                        .Add(New ZahyouItem)
                        .Item(0) = arrZahyouGet(0)
                        .Item(0).CT_Type = "原点TG"
                        .Add(New ZahyouItem)
                        .Item(1) = arrZahyouGet(1)
                        .Item(1).CT_Type = "X軸TG(正)"
                        .Add(New ZahyouItem)
                        .Item(2) = arrZahyouGet(2)
                        .Item(2).CT_Type = "Z軸TG(始点)"
                        .Add(New ZahyouItem)
                        .Item(3) = arrZahyouGet(3)
                        .Item(3).CT_Type = "Z軸TG(終点)"
                        Label_Height.Visibility = Windows.Visibility.Hidden
                        Text_Height.Visibility = Windows.Visibility.Hidden
                        Label_Unit.Visibility = Windows.Visibility.Hidden
                    Case 4
                        .Add(New ZahyouItem)
                        .Item(0) = arrZahyouGet(0)
                        .Item(0).CT_Type = "原点TG"
                        .Add(New ZahyouItem)
                        .Item(1) = arrZahyouGet(1)
                        .Item(1).CT_Type = "X軸TG(正)"
                        .Add(New ZahyouItem)
                        .Item(2) = arrZahyouGet(2)
                        .Item(2).CT_Type = "Y方向TG(負)"
                        Label_Height.Visibility = Windows.Visibility.Hidden
                        Text_Height.Visibility = Windows.Visibility.Hidden
                        Label_Unit.Visibility = Windows.Visibility.Hidden
                    Case 5
                        .Add(New ZahyouItem)
                        .Item(0) = arrZahyouGet(4)
                        .Item(0).CT_Type = "平面TG①"
                        .Add(New ZahyouItem)
                        .Item(1) = arrZahyouGet(5)
                        .Item(1).CT_Type = "平面TG②"
                        .Add(New ZahyouItem)
                        .Item(2) = arrZahyouGet(6)
                        .Item(2).CT_Type = "平面TG③"
                        .Add(New ZahyouItem)
                        .Item(3) = arrZahyouGet(1)
                        .Item(3).CT_Type = "X軸TG(始点)"
                        .Add(New ZahyouItem)
                        .Item(4) = arrZahyouGet(2)
                        .Item(4).CT_Type = "X軸TG(終点)"
                        Label_Height.Visibility = Windows.Visibility.Hidden
                        Text_Height.Visibility = Windows.Visibility.Hidden
                        Label_Unit.Visibility = Windows.Visibility.Hidden
                    Case 6
                        .Add(New ZahyouItem)
                        .Item(0) = arrZahyouGet(4)
                        .Item(0).CT_Type = "平面TG①"
                        .Add(New ZahyouItem)
                        .Item(1) = arrZahyouGet(5)
                        .Item(1).CT_Type = "平面TG②"
                        .Add(New ZahyouItem)
                        .Item(2) = arrZahyouGet(6)
                        .Item(2).CT_Type = "平面TG③"
                        .Add(New ZahyouItem)
                        .Item(3) = arrZahyouGet(1)
                        .Item(3).CT_Type = "原点TG"
                        .Add(New ZahyouItem)
                        .Item(4) = arrZahyouGet(2)
                        .Item(4).CT_Type = "X方向TG"
                        Label_Height.Visibility = Windows.Visibility.Hidden
                        Text_Height.Visibility = Windows.Visibility.Hidden
                        Label_Unit.Visibility = Windows.Visibility.Hidden
                End Select
            End With

            Grid_zahyou.ItemsSource = m_ZahyouItems
            Grid_zahyou.Items.Refresh()
        End If
    End Sub

    Private Sub Text_Height_TextChanged(sender As System.Object, e As System.Windows.Controls.TextChangedEventArgs) Handles Text_Height.TextChanged
        If Zahyou_HData IsNot Nothing Then
            If System.Math.Abs(Zahyou_HData.Zahyou_Height - CDbl(Text_Height.Text)) > 0.0001 Then
                Zahyou_HData.Zahyou_Height = CDbl(Text_Height.Text)
                If Zahyou_HData.OpeFlg = 0 Then
                    Zahyou_HData.OpeFlg = 1
                End If
            End If
        End If
    End Sub

    'Private Sub Text_Height_PreviewKeyDown(sender As System.Object, e As System.Windows.Input.KeyEventArgs) Handles Text_Height.PreviewKeyDown
    '    'If (e.Key >= Windows.Input.Key.D0 And e.Key <= Windows.Input.Key.D9) Or (e.Key >= Windows.Input.Key.NumPad0 And e.Key <= Windows.Input.Key.NumPad9) Or _
    '    '   (e.Key = Windows.Input.Key.DbeEnterDialogConversionMode;
    'End Sub

    Private Sub Grid_Kijun_BeginningEdit(sender As System.Object, e As System.Windows.Controls.DataGridBeginningEditEventArgs) Handles Grid_Kijun.BeginningEdit
        delScaleCulumn = Grid_Kijun.CurrentColumn.DisplayIndex
        delScaleItem = Grid_Kijun.CurrentItem
        edtScaleSTNo = CInt(delScaleItem.StartTarget)
        edtScaleEDNo = CInt(delScaleItem.EndTarget)
        edtScaleLng = CDbl(delScaleItem.KiteiVal)
    End Sub

    Private Sub Grid_Target_BeginningEdit(sender As System.Object, e As System.Windows.Controls.DataGridBeginningEditEventArgs) Handles Grid_Target.BeginningEdit
        delTargetCulumn = Grid_Target.CurrentColumn.DisplayIndex
        delTargetItem = Grid_Target.CurrentItem
    End Sub

    Private Sub Grid_Line_BeginningEdit(sender As System.Object, e As System.Windows.Controls.DataGridBeginningEditEventArgs) Handles Grid_Line.BeginningEdit
        delLineCulumn = Grid_Line.CurrentColumn.DisplayIndex
        delLineItem = Grid_Line.CurrentItem
    End Sub

    Private Sub Grid_PolyLine_BeginningEdit(sender As System.Object, e As System.Windows.Controls.DataGridBeginningEditEventArgs) Handles Grid_PolyLine.BeginningEdit
        delPolyLineCulumn = Grid_PolyLine.CurrentColumn.DisplayIndex
        delPolyLineItem = Grid_PolyLine.CurrentItem
    End Sub

    Private Sub Grid_Circle_BeginningEdit(sender As System.Object, e As System.Windows.Controls.DataGridBeginningEditEventArgs) Handles Grid_Circle.BeginningEdit
        delCircleCulumn = Grid_Circle.CurrentColumn.DisplayIndex
        delCircleItem = Grid_Circle.CurrentItem
    End Sub
    '    'ADD By Yamada 20150320 Sta ---  
    '#If True Then
    '    Private Sub Grid_Target_PreparingCellForEdit(sender As Object, e As System.Windows.Controls.DataGridPreparingCellForEditEventArgs) Handles Grid_Target.PreparingCellForEdit
    '        With Grid_Target
    '            intLastCol = e.Column.DisplayIndex   '現在の列番号
    '            intLastRow = .SelectedIndex      '現在の行番号



    '            Select Case intLastCol

    '                'TG番号、オフセットX～ZはIME無効(半角英数のみ)
    '                Case 1
    '                    'TG番号
    '                    System.Windows.Input.InputMethod.SetIsInputMethodEnabled(e.EditingElement, False)
    '                Case 2, 3, 4
    '                    'オフセットX～Z
    '                    System.Windows.Input.InputMethod.SetIsInputMethodEnabled(e.EditingElement, False)
    '            End Select

    '            '///// DataGridViewの編集コントロールが表示された時に //////
    '            '///// KeyPressイベントを関連付け                    //////
    '            TextEditCtrl = CType(e.EditingElement, System.Windows.Controls.TextBox)
    '            AddHandler TextEditCtrl.PreviewKeyDown, AddressOf TextEditCtrl_KeyPress_Grid_Target
    '        End With
    '    End Sub
    '    ''///// DataGridViewの編集コントロールが表示された時に /////
    '    '///// TextBoxセル内でのKeyPressイベント //////
    '    Private Sub TextEditCtrl_KeyPress_Grid_Target(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs)

    '        Select Case intLastCol
    '            Case 1
    '                'TG番号
    '                Select Case e.Key
    '                    Case System.Windows.Input.Key.Back                                          'BackSpace
    '                    Case System.Windows.Input.Key.D0 To System.Windows.Input.Key.D9,
    '                        System.Windows.Input.Key.NumPad0 To System.Windows.Input.Key.NumPad9
    '                    Case Else
    '                        '上記キー以外は処理しないようにする
    '                        e.Handled = True
    '                End Select
    '            Case 2, 3, 4
    '                'オフセットX～Z
    '                Select Case e.Key
    '                    Case System.Windows.Input.Key.Back                                          'BackSpace
    '                    Case System.Windows.Input.Key.D0 To System.Windows.Input.Key.D9,
    '                        System.Windows.Input.Key.NumPad0 To System.Windows.Input.Key.NumPad9    '数値(0から9)
    '                    Case System.Windows.Input.Key.Decimal, System.Windows.Input.Key.OemPeriod   '小数点
    '                    Case System.Windows.Input.Key.Subtract, System.Windows.Input.Key.OemMinus   'マイナス
    '                    Case System.Windows.Input.Key.Add, System.Windows.Input.Key.OemPlus         'プラス

    '                    Case Else
    '                        '上記キー以外は処理しないようにする
    '                        e.Handled = True
    '                End Select
    '        End Select

    '    End Sub
    '#End If
    '    'ADD By Yamada 20150320 End ---

    Private Sub Grid_zahyou_BeginningEdit(sender As System.Object, e As System.Windows.Controls.DataGridBeginningEditEventArgs) Handles Grid_zahyou.BeginningEdit
        Dim ZahyouD As New ZahyouItem

        ZahyouD = Grid_zahyou.CurrentItem

        edtZahyouCTNo = CInt(ZahyouD.CT_No)
        edtZahyouCulumn = Grid_zahyou.CurrentColumn.DisplayIndex
    End Sub

    '指定TG番号が座標値アイテムで使用されているかチェックする
    Private Function ZhyouItem_Exist(ByVal CT_No As Integer) As Integer
        Dim Ist As Integer
        Dim ii As Integer

        Ist = 0
        For ii = 0 To m_ZahyouItems.Count - 1
            If CInt(m_ZahyouItems(ii).CT_No) = CT_No Then
                Ist = 1
                Exit For
            End If
        Next ii

        Return (Ist)
    End Function

    '指定TG番号がスケールアイテムで使用されているかチェックする
    Private Function ScaleItem_Exist(ByVal CT_No As Integer) As Integer
        Dim Ist As Integer
        Dim ii As Integer

        Ist = 0
        For ii = 0 To m_ScaleItems.Count - 1
            If (CInt(m_ScaleItems(ii).StartTarget) = CT_No) Or (CInt(m_ScaleItems(ii).EndTarget = CT_No)) Then
                Ist = 1
                Exit For
            End If
        Next ii

        Return (Ist)
    End Function

    '指定TG番号が2点線分アイテムで使用されているかチェックする
    Private Function LineLengthItem_Exist(ByVal CT_No As Integer) As Integer
        Dim Ist As Integer
        Dim ii As Integer

        Ist = 0
        For ii = 0 To m_LineLengthItems.Count - 1
            If (CInt(m_LineLengthItems(ii).StartTarget) = CT_No) Or (CInt(m_LineLengthItems(ii).EndTarget) = CT_No) Then
                Ist = 1
                Exit For
            End If
        Next ii

        Return (Ist)
    End Function

    '指定TG番号がポリラインアイテムで使用されているかチェックする
    Private Function PolyLineItem_Exist(ByVal CT_No As Integer) As Integer
        Dim Ist As Integer
        Dim ii, jj As Integer
        Dim iCnt As Integer
        Dim arrTarget(100) As Integer

        Ist = 0
        For ii = 0 To m_PolyLineItems.Count - 1
            iCnt = GetNumber_TargetString(m_PolyLineItems(ii).VertexTarget, arrTarget)
            If iCnt > 0 Then
                For jj = 0 To iCnt - 1
                    If arrTarget(jj) = CT_No Then
                        Ist = 1
                        Exit For
                    End If
                Next jj
            End If
            If Ist = 1 Then
                Exit For
            End If
        Next ii

        Return (Ist)
    End Function

    '指定TG番号が１点円アイテムで使用されているかチェックする
    Private Function CircleItem_Exist(ByVal CT_No As Integer) As Integer
        Dim Ist As Integer
        Dim ii, jj As Integer
        Dim iCnt As Integer
        Dim arrTarget(100) As Integer

        Ist = 0
        For ii = 0 To m_CircleItems.Count - 1
            iCnt = GetNumber_TargetString(m_CircleItems(ii).CenterTarget, arrTarget)
            If iCnt > 0 Then
                For jj = 0 To iCnt - 1
                    If arrTarget(jj) = CT_No Then
                        Ist = 1
                        Exit For
                    End If
                Next jj
            End If
            If Ist = 1 Then
                Exit For
            End If
        Next ii

        Return (Ist)
    End Function

    Private Sub Scale_Checked(sender As Object, e As System.Windows.RoutedEventArgs)
        Dim iColumn As Integer
        Dim KijunD As ScaleItem
        Dim ii, jj As Integer
        Dim iFlg As Integer

        If Grid_Kijun.CurrentColumn Is Nothing Then
            Exit Sub
        End If

        iColumn = Grid_Kijun.CurrentColumn.DisplayIndex
        KijunD = Grid_Kijun.CurrentItem
        iFlg = 0
        '基準スケールチェックボックスのみ処理を行う
        If iColumn = 6 Then
            For ii = 0 To m_ScaleItems.Count - 1
                If KijunD.SunpoID <> m_ScaleItems(ii).SunpoID Then
                    If m_ScaleItems(ii).flgScale = 1 Then
                        m_ScaleItems(ii).flgScale = 0
                        If m_ScaleItems(ii).OpeFlg = 0 Then
                            m_ScaleItems(ii).OpeFlg = 1
                        End If
                        iFlg = 1
                        If m_ScaleItems(ii).OpeFlg <> 3 Then
                            For jj = 0 To m_Scalecount - 1
                                If arrScaleData(jj).SunpoID = m_ScaleItems(ii).SunpoID Then
                                    If arrScaleData(jj).flgScale <> m_ScaleItems(ii).flgScale Then
                                        arrScaleData(jj).flgScale = m_ScaleItems(ii).flgScale
                                        arrScaleData(jj).OpeFlg = 1
                                    End If
                                    Exit For
                                End If
                            Next jj
                        End If
                    End If
                Else
                    If m_ScaleItems(ii).OpeFlg <> 3 Then
                        For jj = 0 To m_Scalecount - 1
                            If arrScaleData(jj).SunpoID = m_ScaleItems(ii).SunpoID Then
                                If arrScaleData(jj).flgScale <> m_ScaleItems(ii).flgScale Then
                                    arrScaleData(jj).flgScale = m_ScaleItems(ii).flgScale
                                    arrScaleData(jj).OpeFlg = 1
                                End If
                                Exit For
                            End If
                        Next jj
                    End If
                End If
            Next ii
        End If
        If iFlg = 1 Then
            Grid_Kijun.CommitEdit()
            Grid_Kijun.Items.Refresh()
        End If
    End Sub

    Private Sub Scale_UnChecked(sender As Object, e As System.Windows.RoutedEventArgs)
        Dim iColumn As Integer
        Dim KijunD As ScaleItem

        If Grid_Kijun.CurrentColumn IsNot Nothing Then
            iColumn = Grid_Kijun.CurrentColumn.DisplayIndex
            KijunD = Grid_Kijun.CurrentItem
        End If
    End Sub

    Private Sub Scale_TextChanged(sender As Object, e As System.Windows.RoutedEventArgs)
        Dim iColumn As Integer
        Dim KijunD As ScaleItem

        If Grid_Kijun.CurrentColumn IsNot Nothing Then
            iColumn = Grid_Kijun.CurrentColumn.DisplayIndex
            KijunD = Grid_Kijun.CurrentItem
        End If
    End Sub

End Class
