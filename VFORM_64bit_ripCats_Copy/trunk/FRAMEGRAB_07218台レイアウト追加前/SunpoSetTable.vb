Public Class SunpoSetTable

    Private Const S_sunpo_set As String = "SunpoSet"
    Private Const S_sunpo_cell_set As String = "SunpoSetCell"

    Private ReadOnly S_ID As String() = {"ID"}                              '0  ID
    Private ReadOnly S_SunpoID As String() = {"SunpoID"}                    '1  SunpoID
    Private ReadOnly S_SunpoMark As String() = {"SunpoMark"}                '2  部位

    Private ReadOnly S_SunpoName As String() = {"SunpoName"}                '3  算出項目名

    Private ReadOnly S_SunpoCellName As String() = {"SunpoCellName"}        '4  セル名

    Private ReadOnly S_SunpoCalcHouhou As String() = {"SunpoCalcHouhou"}    '5  寸法算出方法(2点間距離：１、線と点の距離：２）

    Private ReadOnly S_CT_ID1 As String() = {"CT_ID1"}                      '6  点群ＩＤ１

    Private ReadOnly S_CT_ID2 As String() = {"CT_ID2"}                      '7  点群ＩＤ２

    Private ReadOnly S_CT_ID3 As String() = {"CT_ID3"}                      '8  点群ＩＤ３

    Private ReadOnly S_seibunXYZ As String() = {"seibunXYZ"}                '9  P1
    Private ReadOnly S_CT_Active As String() = {"CT_Active"}                '10 P2
    Private ReadOnly S_SunpoVal As String() = {"SunpoVal"}                  '11 P3
    Private ReadOnly S_GunID1 As String() = {"GunID1"}                      '12 
    Private ReadOnly S_GunID2 As String() = {"GunID2"}                      '13 
    Private ReadOnly S_GunID3 As String() = {"GunID3"}                      '14 
    Private ReadOnly S_KiteiVal As String() = {"KiteiVal"}                  '15 規定値
    Private ReadOnly S_KiteiMin As String() = {"KiteiMin"}                  '16 最小許容値
    Private ReadOnly S_KiteiMax As String() = {"KiteiMax"}                  '17 最大許容値
    Private ReadOnly S_TypeID As String() = {"TypeID"}                      '18 TYPE_ID
    Private ReadOnly S_flg_gouhi As String() = {"flg_gouhi"}                '19 合否
    Private ReadOnly S_Targettype As String() = {"Targettype"}              '20 ターゲットタイプ

    Private ReadOnly S_flgOutZu As String() = {"flgOutZu"}                  '21 
    Private ReadOnly S_ZU_layer As String() = {"ZU_layer"}                  '22
    Private ReadOnly S_ZU_colorID As String() = {"ZU_colorID"}              '23 色コード

    Private ReadOnly S_ZU_LineTypeID As String() = {"ZU_LineTypeID"}        '24 線種コード

    Private ReadOnly S_flgKeisan As String() = {"flgKeisan"}                '25 計算済みフラグ

    Private ReadOnly S_flgScale As String() = {"flgScale"}                  '25 スケールフラグ(20150105 Tezuka ADD)

    Private _ID As String = ""
    Public Property ID() As String
        Get
            Return _ID
        End Get
        Set(ByVal value As String)
            _ID = value
        End Set
    End Property

    Private Sub p_set_ID(ByRef IDR As IDataReader)
        _ID = GetDataByIndexOfDataReader(FIELD_INDEX(0), IDR)
    End Sub

    Private _SunpoID As String = ""
    Public Property SunpoID() As String
        Get
            Return _SunpoID
        End Get
        Set(ByVal value As String)
            _SunpoID = value
        End Set
    End Property

    Private Sub p_set_SunpoID(ByRef IDR As IDataReader)
        _SunpoID = GetDataByIndexOfDataReader(FIELD_INDEX(1), IDR)
    End Sub

    Private _SunpoMark As String = ""
    Public Property SunpoMark() As String
        Get
            Return _SunpoMark
        End Get
        Set(ByVal value As String)
            _SunpoMark = value
        End Set
    End Property

    Private Sub p_set_SunpoMark(ByRef IDR As IDataReader)
        _SunpoMark = GetDataByIndexOfDataReader(FIELD_INDEX(2), IDR)
    End Sub

    Private _SunpoName As String = ""
    Public Property SunpoName() As String
        Get
            Return _SunpoName
        End Get
        Set(ByVal value As String)
            _SunpoName = value
        End Set
    End Property

    Private Sub p_set_SunpoName(ByRef IDR As IDataReader)
        _SunpoName = GetDataByIndexOfDataReader(FIELD_INDEX(3), IDR)
    End Sub

    Private _SunpoCellName As String = ""
    Public Property SunpoCellName() As String
        Get
            Return _SunpoCellName
        End Get
        Set(ByVal value As String)
            _SunpoCellName = value
        End Set
    End Property

    Private Sub p_set_SunpoCellName(ByRef IDR As IDataReader)
        _SunpoCellName = GetDataByIndexOfDataReader(FIELD_INDEX(4), IDR)
    End Sub

    Private _SunpoCalcHouhou As String = ""
    Public Property SunpoCalcHouhou() As String
        Get
            Return _SunpoCalcHouhou
        End Get
        Set(ByVal value As String)
            _SunpoCalcHouhou = value
        End Set
    End Property

    Private Sub p_set_SunpoCalcHouhou(ByRef IDR As IDataReader)
        _SunpoCalcHouhou = GetDataByIndexOfDataReader(FIELD_INDEX(5), IDR)
    End Sub

    Private _CT_ID1 As String = ""
    Public Property CT_ID1() As String
        Get
            Return _CT_ID1
        End Get
        Set(ByVal value As String)
            _CT_ID1 = value
        End Set
    End Property

    Private Sub p_set_CT_ID1(ByRef IDR As IDataReader)
        _CT_ID1 = GetDataByIndexOfDataReader(FIELD_INDEX(6), IDR)
    End Sub

    Private _CT_ID2 As String = ""
    Public Property CT_ID2() As String
        Get
            Return _CT_ID2
        End Get
        Set(ByVal value As String)
            _CT_ID2 = value
        End Set
    End Property

    Private Sub p_set_CT_ID2(ByRef IDR As IDataReader)
        _CT_ID2 = GetDataByIndexOfDataReader(FIELD_INDEX(7), IDR)
    End Sub

    Private _CT_ID3 As String = ""
    Public Property CT_ID3() As String
        Get
            Return _CT_ID3
        End Get
        Set(ByVal value As String)
            _CT_ID3 = value
        End Set
    End Property

    Private Sub p_set_CT_ID3(ByRef IDR As IDataReader)
        _CT_ID3 = GetDataByIndexOfDataReader(FIELD_INDEX(8), IDR)
    End Sub

    Private _seibunXYZ As String = ""
    Public Property seibunXYZ() As String
        Get
            Return _seibunXYZ
        End Get
        Set(ByVal value As String)
            _seibunXYZ = value
        End Set
    End Property

    Private Sub p_set_seibunXYZ(ByRef IDR As IDataReader)
        _seibunXYZ = GetDataByIndexOfDataReader(FIELD_INDEX(9), IDR)
    End Sub

    Private _CT_Active As String = ""
    Public Property CT_Active() As String
        Get
            Return _CT_Active
        End Get
        Set(ByVal value As String)
            _CT_Active = value
        End Set
    End Property

    Private Sub p_set_CT_Active(ByRef IDR As IDataReader)
        _CT_Active = GetDataByIndexOfDataReader(FIELD_INDEX(10), IDR)
    End Sub

    Private _SunpoVal As String = ""
    Public Property SunpoVal() As String
        Get
            Return _SunpoVal
        End Get
        Set(ByVal value As String)
            _SunpoVal = value
            If (_KiteiMin < (_SunpoVal - _KiteiVal)) And ((_SunpoVal - _KiteiVal) < _KiteiMax) Then
                _flg_gouhi = "1"
            Else
                _flg_gouhi = "0"
            End If
        End Set
    End Property

    Private Sub p_set_SunpoVal(ByRef IDR As IDataReader)
        _SunpoVal = GetDataByIndexOfDataReader(FIELD_INDEX(11), IDR)
    End Sub

    Private _GunID1 As String = ""
    Public Property GunID1() As String
        Get
            Return _GunID1
        End Get
        Set(ByVal value As String)
            _GunID1 = value
        End Set
    End Property

    Private Sub p_set_GunID1(ByRef IDR As IDataReader)
        _GunID1 = GetDataByIndexOfDataReader(FIELD_INDEX(12), IDR)
    End Sub

    Private _GunID2 As String = ""
    Public Property GunID2() As String
        Get
            Return _GunID2
        End Get
        Set(ByVal value As String)
            _GunID2 = value
        End Set
    End Property

    Private Sub p_set_GunID2(ByRef IDR As IDataReader)
        _GunID2 = GetDataByIndexOfDataReader(FIELD_INDEX(13), IDR)
    End Sub

    Private _GunID3 As String = ""
    Public Property GunID3() As String
        Get
            Return _GunID3
        End Get
        Set(ByVal value As String)
            _GunID3 = value
        End Set
    End Property

    Private Sub p_set_GunID3(ByRef IDR As IDataReader)
        _GunID3 = GetDataByIndexOfDataReader(FIELD_INDEX(14), IDR)
    End Sub

    Private _KiteiVal As String = ""
    Public Property KiteiVal() As String
        Get
            Return _KiteiVal
        End Get
        Set(ByVal value As String)
            _KiteiVal = value
        End Set
    End Property

    Private Sub p_set_KiteiVal(ByRef IDR As IDataReader)
        _KiteiVal = GetDataByIndexOfDataReader(FIELD_INDEX(15), IDR)
    End Sub

    Private _KiteiMin As String = ""
    Public Property KiteiMin() As String
        Get
            Return _KiteiMin
        End Get
        Set(ByVal value As String)
            _KiteiMin = value
        End Set
    End Property

    Private Sub p_set_KiteiMin(ByRef IDR As IDataReader)
        _KiteiMin = GetDataByIndexOfDataReader(FIELD_INDEX(16), IDR)
    End Sub

    Private _KiteiMax As String = ""
    Public Property KiteiMax() As String
        Get
            Return _KiteiMax
        End Get
        Set(ByVal value As String)
            _KiteiMax = value
        End Set
    End Property

    Private Sub p_set_KiteiMax(ByRef IDR As IDataReader)
        _KiteiMax = GetDataByIndexOfDataReader(FIELD_INDEX(17), IDR)
    End Sub

    Private _TypeID As String = ""
    Public Property TypeID() As String
        Get
            Return _TypeID
        End Get
        Set(ByVal value As String)
            _TypeID = value
        End Set
    End Property

    Private Sub p_set_TypeID(ByRef IDR As IDataReader)
        _TypeID = GetDataByIndexOfDataReader(FIELD_INDEX(18), IDR)
    End Sub

    Private _flg_gouhi As String = ""
    Public Property flg_gouhi() As String
        Get
            Return _flg_gouhi
        End Get
        Set(ByVal value As String)
            _flg_gouhi = value
        End Set
    End Property

    Private Sub p_set_flg_gouhi(ByRef IDR As IDataReader)
        _flg_gouhi = GetDataByIndexOfDataReader(FIELD_INDEX(19), IDR)
    End Sub

    '(20131128 Tezuka Add)Start
    Private _Targettype As String = ""
    Public Property Targettype() As String
        Get
            Return _Targettype
        End Get
        Set(ByVal value As String)
            _Targettype = value
        End Set
    End Property

    Private Sub p_set_Targettype(ByRef IDR As IDataReader)
        _Targettype = GetDataByIndexOfDataReader(FIELD_INDEX(20), IDR)
    End Sub

    Private _flgOutZu As String = ""
    Public Property flgOutZu() As String
        Get
            Return _flgOutZu
        End Get
        Set(ByVal value As String)
            _flgOutZu = value
        End Set
    End Property

    Private Sub p_set_flgOutZu(ByRef IDR As IDataReader)
        _flgOutZu = GetDataByIndexOfDataReader(FIELD_INDEX(21), IDR)
    End Sub

    Private _ZU_layer As String = ""
    Public Property ZU_layer() As String
        Get
            Return _ZU_layer
        End Get
        Set(ByVal value As String)
            _ZU_layer = value
        End Set
    End Property

    Private Sub p_set_ZU_layer(ByRef IDR As IDataReader)
        _ZU_layer = GetDataByIndexOfDataReader(FIELD_INDEX(22), IDR)
    End Sub

    Private _ZU_colorID As String = ""
    Public Property ZU_colorID() As String
        Get
            Return _ZU_colorID
        End Get
        Set(ByVal value As String)
            _ZU_colorID = value
        End Set
    End Property

    Private Sub p_set_ZU_colorID(ByRef IDR As IDataReader)
        _ZU_colorID = GetDataByIndexOfDataReader(FIELD_INDEX(23), IDR)
    End Sub

    Private _ZU_LineTypeID As String = ""
    Public Property ZU_LineTypeID() As String
        Get
            Return _ZU_LineTypeID
        End Get
        Set(ByVal value As String)
            _ZU_LineTypeID = value
        End Set
    End Property

    Private Sub p_set_ZU_LineTypeID(ByRef IDR As IDataReader)
        _ZU_LineTypeID = GetDataByIndexOfDataReader(FIELD_INDEX(24), IDR)
    End Sub

    Private _flgKeisan As String = ""
    Public Property flgKeisan() As String
        Get
            Return _flgKeisan
        End Get
        Set(ByVal value As String)
            _flgKeisan = value
        End Set
    End Property

    Private Sub p_set_flgKeisan(ByRef IDR As IDataReader)
        _flgKeisan = GetDataByIndexOfDataReader(FIELD_INDEX(25), IDR)
    End Sub
    '(20131128 Tezuka Add)End

    '(20150105 Tezuka Add)Start(スケールフラグ追加)
    Private _flgScale As String = ""
    Public Property flgScale() As String
        Get
            Return _flgScale
        End Get
        Set(ByVal value As String)
            _flgScale = value
        End Set
    End Property

    Private Sub p_set_flgScale(ByRef IDR As IDataReader)
        _flgScale = GetDataByIndexOfDataReader(FIELD_INDEX(26), IDR)
    End Sub
    '(20150105 Tezuka Add)End

    Public flgSyusei As Boolean = False
    Public flgNewInput As Boolean = False
    Private _backup As SunpoSetTable
    Public Const MaxFieldCnt = 26
    Public strFieldNames() As String
    Public strFieldTexts() As String
    Public lstCT_ID As List(Of Integer)
    Public objZu As Object()

    Public m_dbClass As CDBOperateOLE

    Public Sub New()
        lstCT_ID = New List(Of Integer)

    End Sub

    Public Sub BackUp()
        _backup = New SunpoSetTable
        With _backup
            ._ID = Me._ID                           '0  ID
            ._SunpoID = Me._SunpoID                 '1  SunpoID
            ._SunpoMark = Me._SunpoMark             '2  SunpoMark
            ._SunpoName = Me._SunpoName             '3  SunpoName
            ._SunpoCellName = Me._SunpoCellName     '4  SunpoCellName
            ._SunpoCalcHouhou = Me._SunpoCalcHouhou '5  寸法算出方法(2点間距離：１、線と点の距離：２）

            ._CT_ID1 = Me._CT_ID1                    '6  点群ＩＤ１

            ._CT_ID2 = Me._CT_ID2                    '7  点群ＩＤ２

            ._CT_ID3 = Me._CT_ID3                    '8  点群ＩＤ３

            ._seibunXYZ = Me._seibunXYZ              '9  P1
            ._CT_Active = Me._CT_Active              '10 P2
            ._SunpoVal = Me._SunpoVal                '11 P3
            ._GunID1 = Me._GunID1                    '12 
            ._GunID2 = Me._GunID2                    '13 
            ._GunID3 = Me._GunID3                    '14 
            ._KiteiVal = Me._KiteiVal               '15  KiteiVal
            ._KiteiMin = Me._KiteiMin               '16  KiteiMin
            ._KiteiMax = Me._KiteiMax               '17  KiteiMax
            ._TypeID = Me._TypeID                   '18  TypeID
            ._flg_gouhi = Me._flg_gouhi             '19  flg_gouhi
            ._Targettype = Me._Targettype           '20  Targettype
            ._flgOutZu = Me.flgOutZu                '21  flgOutZu
            ._ZU_layer = Me.ZU_layer                '22  ZU_layer
            ._ZU_colorID = Me._ZU_colorID           '23  ZU_colorID
            ._ZU_LineTypeID = Me.ZU_LineTypeID      '24  ZU_LineID
            ._flgKeisan = Me.flgKeisan              '25  flgKeisan
            ._flgScale = Me.flgScale                '26  flgScale(20150105 Tezuka ADD)
        End With
    End Sub

    Public Sub copy(ByVal SST As SunpoSetTable)

        With SST
            _ID = ._ID                              '0  ID
            _SunpoID = ._SunpoID                    '1  SunpoID
            _SunpoMark = ._SunpoMark                '2  SunpoPart
            _SunpoName = ._SunpoName                '3  SunpoName
            _SunpoCellName = ._SunpoCellName        '4  SunpoCellName
            _SunpoCalcHouhou = ._SunpoCalcHouhou    '5  寸法算出方法(2点間距離：１、線と点の距離：２）

            _CT_ID1 = ._CT_ID1                      '6  点群ＩＤ１

            _CT_ID2 = ._CT_ID2                      '7  点群ＩＤ２

            _CT_ID3 = ._CT_ID3                      '8  点群ＩＤ３

            _seibunXYZ = ._seibunXYZ                '9  P1
            _CT_Active = ._CT_Active                '10 P2
            _SunpoVal = ._SunpoVal                  '11 P3
            _GunID1 = ._GunID1                      '12 
            _GunID2 = ._GunID2                      '13 
            _GunID3 = ._GunID3                      '14 
            _KiteiVal = ._KiteiVal                  '15  KiteiVal
            _KiteiMin = ._KiteiMin                  '16  KiteiMin
            _KiteiMax = ._KiteiMax                  '17  KiteiMax
            _TypeID = ._TypeID                      '18  TypeID
            _flg_gouhi = ._flg_gouhi                '19  flg_gouhi
            _Targettype = ._Targettype              '20  Targettype
            _flgOutZu = ._flgOutZu                  '21  flgOutZu
            _ZU_layer = ._ZU_layer                  '22  ZU_layer
            _ZU_colorID = ._ZU_colorID              '23  ZU_colorID
            _ZU_LineTypeID = ._ZU_LineTypeID        '24  ZU_LineTypeID
            _flgKeisan = ._flgKeisan                '25  flgKeisan
            _flgScale = ._flgScale                  '26  flgScale(20150105 Tezuka ADD)
        End With

        BackUp()

    End Sub

    Public Sub SetFieldIndex(ByRef IDR As IDataReader)
        Dim iList As New List(Of Integer)
        '順番を間違いないように！！！！

        iList.Add(GetIndexOfDataReader(S_ID, IDR))                  '0  ID
        iList.Add(GetIndexOfDataReader(S_SunpoID, IDR))             '1  SunpoID
        iList.Add(GetIndexOfDataReader(S_SunpoMark, IDR))           '2  SunpoPart
        iList.Add(GetIndexOfDataReader(S_SunpoName, IDR))           '3  SunpoName
        iList.Add(GetIndexOfDataReader(S_SunpoCellName, IDR))       '4  SunpoCellName
        iList.Add(GetIndexOfDataReader(S_SunpoCalcHouhou, IDR))     '5  寸法算出方法(2点間距離：１、線と点の距離：２）

        iList.Add(GetIndexOfDataReader(S_CT_ID1, IDR))              '6  点群ＩＤ１

        iList.Add(GetIndexOfDataReader(S_CT_ID2, IDR))              '7  点群ＩＤ２

        iList.Add(GetIndexOfDataReader(S_CT_ID3, IDR))              '8  点群ＩＤ３

        iList.Add(GetIndexOfDataReader(S_seibunXYZ, IDR))           '9  P1
        iList.Add(GetIndexOfDataReader(S_CT_Active, IDR))           '10 P2
        iList.Add(GetIndexOfDataReader(S_SunpoVal, IDR))            '11 P3
        iList.Add(GetIndexOfDataReader(S_GunID1, IDR))              '12 
        iList.Add(GetIndexOfDataReader(S_GunID2, IDR))              '13 
        iList.Add(GetIndexOfDataReader(S_GunID3, IDR))              '14 
        iList.Add(GetIndexOfDataReader(S_KiteiVal, IDR))            '15  KiteiVal
        iList.Add(GetIndexOfDataReader(S_KiteiMin, IDR))            '16  KiteiMin
        iList.Add(GetIndexOfDataReader(S_KiteiMax, IDR))            '17  KiteiMax
        iList.Add(GetIndexOfDataReader(S_TypeID, IDR))              '18  TypeID
        iList.Add(GetIndexOfDataReader(S_flg_gouhi, IDR))           '19  flg_gouhi
        iList.Add(GetIndexOfDataReader(S_Targettype, IDR))          '20  Targettype
        iList.Add(GetIndexOfDataReader(S_flgOutZu, IDR))            '21  flgOutZu
        iList.Add(GetIndexOfDataReader(S_ZU_layer, IDR))            '22  ZU_layer
        iList.Add(GetIndexOfDataReader(S_ZU_colorID, IDR))          '23  ZU_colorID
        iList.Add(GetIndexOfDataReader(S_ZU_LineTypeID, IDR))       '24  ZU_LineTypeID
        iList.Add(GetIndexOfDataReader(S_flgKeisan, IDR))           '25  flgKeisan
        iList.Add(GetIndexOfDataReader(S_flgScale, IDR))            '26  flgScale(20150105 Tezuka ADD)

        FIELD_INDEX = iList

    End Sub

    Public Sub SetFieldIndexForCellTable(ByRef IDR As IDataReader)
        Dim iList As New List(Of Integer)
        '順番を間違いないように！！！！

        iList.Add(GetIndexOfDataReader(S_ID, IDR))                  '0  ID
        iList.Add(GetIndexOfDataReader(S_SunpoID, IDR))             '1  SunpoID
        iList.Add(GetIndexOfDataReader(S_SunpoMark, IDR))           '2  SunpoPart
        iList.Add(GetIndexOfDataReader(S_SunpoName, IDR))           '3  SunpoName
        'iList.Add(GetIndexOfDataReader(S_SunpoCellName, IDR))       '4  SunpoCellName
        'iList.Add(GetIndexOfDataReader(S_SunpoCalcHouhou, IDR))     '5  寸法算出方法(2点間距離：１、線と点の距離：２）

        'iList.Add(GetIndexOfDataReader(S_CT_ID1, IDR))              '6  点群ＩＤ１

        'iList.Add(GetIndexOfDataReader(S_CT_ID2, IDR))              '7  点群ＩＤ２

        'iList.Add(GetIndexOfDataReader(S_CT_ID3, IDR))              '8  点群ＩＤ３

        'iList.Add(GetIndexOfDataReader(S_seibunXYZ, IDR))           '9  P1
        iList.Add(GetIndexOfDataReader(S_CT_Active, IDR))           '10 P2
        iList.Add(GetIndexOfDataReader(S_SunpoVal, IDR))            '11 P3
        'iList.Add(GetIndexOfDataReader(S_GunID1, IDR))              '12 
        'iList.Add(GetIndexOfDataReader(S_GunID2, IDR))              '13 
        'iList.Add(GetIndexOfDataReader(S_GunID3, IDR))              '14 
        iList.Add(GetIndexOfDataReader(S_KiteiVal, IDR))            '15  KiteiVal
        iList.Add(GetIndexOfDataReader(S_KiteiMin, IDR))            '16  KiteiMin
        iList.Add(GetIndexOfDataReader(S_KiteiMax, IDR))            '17  KiteiMax
        iList.Add(GetIndexOfDataReader(S_TypeID, IDR))              '18  TypeID
        iList.Add(GetIndexOfDataReader(S_flg_gouhi, IDR))           '19  flg_gouhi
        'iList.Add(GetIndexOfDataReader(S_Targettype, IDR))          '20  Targettype
        'iList.Add(GetIndexOfDataReader(S_flgOutZu, IDR))            '21  flgOutZu
        'iList.Add(GetIndexOfDataReader(S_ZU_layer, IDR))            '22  ZU_layer
        'iList.Add(GetIndexOfDataReader(S_ZU_colorID, IDR))          '23  ZU_colorID
        'iList.Add(GetIndexOfDataReader(S_ZU_LineTypeID, IDR))       '24  ZU_LineTypeID
        'iList.Add(GetIndexOfDataReader(S_flgKeisan, IDR))           '25  flgKeisan

        FIELD_INDEX = iList

    End Sub

    Public Shared FIELD_INDEX As New List(Of Integer)

    Public Function GetDataByReader(ByRef iDataReader As IDataReader)
        GetDataByReader = False

        p_set_ID(iDataReader)                                   '0  ID
        p_set_SunpoID(iDataReader)                              '1	SunpoID
        p_set_SunpoMark(iDataReader)                            '2	SunpoPart
        p_set_SunpoName(iDataReader)                            '3	SunpoName
        p_set_SunpoCellName(iDataReader)                        '4	SunpoCellName
        p_set_SunpoCalcHouhou(iDataReader)                      '5  寸法算出方法(2点間距離：１、線と点の距離：２）

        p_set_CT_ID1(iDataReader)                               '6  点群ＩＤ１

        p_set_CT_ID2(iDataReader)                               '7  点群ＩＤ２

        p_set_CT_ID3(iDataReader)                               '8  点群ＩＤ３

        p_set_seibunXYZ(iDataReader)                            '9  P1
        p_set_CT_Active(iDataReader)                            '10 P2
        p_set_SunpoVal(iDataReader)                             '11 P3
        p_set_GunID1(iDataReader)                               '12 
        p_set_GunID2(iDataReader)                               '13 
        p_set_GunID3(iDataReader)                               '14 
        p_set_KiteiVal(iDataReader)                             '15	KiteiVal
        p_set_KiteiMin(iDataReader)                             '16	KiteiMin
        p_set_KiteiMax(iDataReader)                             '17	KiteiMax
        p_set_TypeID(iDataReader)                               '18	TypeID
        p_set_flg_gouhi(iDataReader)                            '19	flg_gouhi
        p_set_Targettype(iDataReader)                           '20 Targettype
        p_set_flgOutZu(iDataReader)                             '21 flgOutZu
        p_set_ZU_layer(iDataReader)                             '22 ZU_layer
        p_set_ZU_colorID(iDataReader)                           '23 ZU_colorID
        p_set_ZU_LineTypeID(iDataReader)                        '24 ZU_LineTypeID
        p_set_flgKeisan(iDataReader)                            '25 flgKeisan
        p_set_flgScale(iDataReader)                             '26 flgScale

        BackUp() 'バックアップ用

        GetDataByReader = True

    End Function
    '20130907 SUURI ADD START
    '速度低下の原因になっているため、不要なフィールド読み込みは外す
    Public Function GetDataByReaderForCellTable(ByRef iDataReader As IDataReader)
        GetDataByReaderForCellTable = False

        p_set_ID(iDataReader)                                   '0  ID
        p_set_SunpoID(iDataReader)                              '1	SunpoID
        p_set_SunpoMark(iDataReader)                            '2	SunpoPart
        p_set_SunpoName(iDataReader)                            '3	SunpoName
        'p_set_SunpoCellName(iDataReader)                        '4	SunpoCellName
        'p_set_SunpoCalcHouhou(iDataReader)                      '5  寸法算出方法(2点間距離：１、線と点の距離：２）

        'p_set_CT_ID1(iDataReader)                               '6  点群ＩＤ１

        'p_set_CT_ID2(iDataReader)                               '7  点群ＩＤ２

        'p_set_CT_ID3(iDataReader)                               '8  点群ＩＤ３

        'p_set_seibunXYZ(iDataReader)                            '9  P1
        p_set_CT_Active(iDataReader)                            '10 P2
        p_set_SunpoVal(iDataReader)                             '11 P3
        'p_set_GunID1(iDataReader)                               '12 
        'p_set_GunID2(iDataReader)                               '13 
        'p_set_GunID3(iDataReader)                               '14 
        p_set_KiteiVal(iDataReader)                             '15	KiteiVal
        p_set_KiteiMin(iDataReader)                             '16	KiteiMin
        p_set_KiteiMax(iDataReader)                             '17	KiteiMax
        p_set_TypeID(iDataReader)                               '18	TypeID
        p_set_flg_gouhi(iDataReader)                            '19	flg_gouhi
        'p_set_Targettype(iDataReader)                           '20 Targettype
        'p_set_flgOutZu(iDataReader)                             '21 flgOutZu
        'p_set_ZU_layer(iDataReader)                             '22 ZU_layer
        'p_set_ZU_colorID(iDataReader)                           '23 ZU_colorID
        'p_set_ZU_LineTypeID(iDataReader)                        '24 ZU_LineTypeID
        'p_set_flgKeisan(iDataReader)                            '25 flgkeisan

        BackUp() 'バックアップ用

        GetDataByReaderForCellTable = True

    End Function
    '20130907 SUURI ADD END
    Private Sub CreateFieldName()
        ReDim strFieldNames(MaxFieldCnt - 1)

        strFieldNames(0) = "SunpoID"
        strFieldNames(1) = "SunpoPart"
        strFieldNames(2) = "SunpoName"
        strFieldNames(3) = "SunpoCellName"
        strFieldNames(4) = "SunpoCalcHouhou"                      '4  寸法算出方法(2点間距離：１、線と点の距離：２）"
        strFieldNames(5) = "CT_ID1"                               '5  点群ＩＤ１"
        strFieldNames(6) = "CT_ID2"                               '6  点群ＩＤ２"
        strFieldNames(7) = "CT_ID3"                               '7  点群ＩＤ３"
        strFieldNames(8) = "seibunXYZ"                            '8  P1"
        strFieldNames(9) = "CT_Active"                            '9  P2"
        strFieldNames(10) = "SunpoVal"                             '10 P3"
        strFieldNames(11) = "GunID1"                               '11 "
        strFieldNames(12) = "GunID2"                               '12 "
        strFieldNames(13) = "GunID3"                               '13 "
        strFieldNames(14) = "KiteiVal"
        strFieldNames(15) = "KiteiMin"
        strFieldNames(16) = "KiteiMax"
        strFieldNames(17) = "TypeID"
        strFieldNames(18) = "flg_gouhi"
        strFieldNames(19) = "Targettype"
        strFieldNames(20) = "flgOutZu"
        strFieldNames(21) = "ZU_layer"
        strFieldNames(22) = "ZU_colorID"
        strFieldNames(23) = "ZU_LineTypeID"
        strFieldNames(24) = "flgkeisan"
        strFieldNames(25) = "flgScale"

    End Sub

    Public Sub CreateFieldText()
        ReDim strFieldTexts(MaxFieldCnt - 1)

        CreateFieldName()

        strFieldTexts(0) = _SunpoID
        strFieldTexts(1) = "'" & _SunpoMark & "'"
        strFieldTexts(2) = "'" & _SunpoName & "'"
        strFieldTexts(3) = "'" & _SunpoCellName & "'"
        strFieldTexts(4) = _SunpoCalcHouhou                      '4  寸法算出方法(2点間距離：１、線と点の距離：２）"
        strFieldTexts(5) = _CT_ID1                               '5  点群ＩＤ１"
        strFieldTexts(6) = _CT_ID2                               '6  点群ＩＤ２"
        strFieldTexts(7) = _CT_ID3                               '7  点群ＩＤ３"
        strFieldTexts(8) = _seibunXYZ                            '8  P1"
        strFieldTexts(9) = _CT_Active                            '9  P2"
        strFieldTexts(10) = _SunpoVal                             '10 P3"
        strFieldTexts(11) = _GunID1                               '11 "
        strFieldTexts(12) = _GunID2                               '12 "
        strFieldTexts(13) = _GunID3                               '13 "
        strFieldTexts(14) = _KiteiVal
        strFieldTexts(15) = _KiteiMin
        strFieldTexts(16) = _KiteiMax
        strFieldTexts(17) = _TypeID
        strFieldTexts(18) = _flg_gouhi
        strFieldTexts(19) = _Targettype
        strFieldTexts(20) = _flgOutZu
        strFieldTexts(21) = _ZU_layer
        strFieldTexts(22) = _ZU_colorID
        strFieldTexts(23) = _ZU_LineTypeID
        strFieldTexts(24) = _flgKeisan
        strFieldTexts(25) = _flgScale

    End Sub

    Public Sub CreateField()

        Dim IDX As Integer = 0

        ReDim strFieldNames(IDX)
        ReDim strFieldTexts(IDX)
        strFieldNames(IDX) = "SunpoID"
        strFieldTexts(IDX) = _SunpoID
        IDX += 1

        ReDim Preserve strFieldNames(IDX)
        ReDim Preserve strFieldTexts(IDX)
        strFieldNames(IDX) = "SunpoMark"
        strFieldTexts(IDX) = "'" & _SunpoMark & "'"
        IDX += 1

        ReDim Preserve strFieldNames(IDX)
        ReDim Preserve strFieldTexts(IDX)
        strFieldNames(IDX) = "SunpoName"
        strFieldTexts(IDX) = "'" & _SunpoName & "'"
        IDX += 1

        ReDim Preserve strFieldNames(IDX)
        ReDim Preserve strFieldTexts(IDX)
        strFieldNames(IDX) = "SunpoCellName"
        strFieldTexts(IDX) = "'" & _SunpoCellName & "'"
        IDX += 1

        If _SunpoCalcHouhou <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "SunpoCalcHouhou"
            strFieldTexts(IDX) = _SunpoCalcHouhou
            IDX += 1
        End If

        If _CT_ID1 <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "CT_ID1"
            strFieldTexts(IDX) = _CT_ID1
            IDX += 1
        End If

        If _CT_ID2 <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "CT_ID2"
            strFieldTexts(IDX) = _CT_ID2
            IDX += 1
        End If

        If _CT_ID3 <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "CT_ID3"
            strFieldTexts(IDX) = _CT_ID3
            IDX += 1
        End If

        If _seibunXYZ <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "seibunXYZ"
            strFieldTexts(IDX) = _seibunXYZ
            IDX += 1
        End If

        If _CT_Active <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "CT_Active"
            strFieldTexts(IDX) = _CT_Active
            IDX += 1
        End If

        If _SunpoVal <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "SunpoVal"
            strFieldTexts(IDX) = dblField(_SunpoVal)
            IDX += 1
        End If

        If _GunID1 <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "GunID1"
            strFieldTexts(IDX) = _GunID1
            IDX += 1
        End If

        If _GunID2 <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "GunID2"
            strFieldTexts(IDX) = _GunID2
            IDX += 1
        End If

        If _GunID3 <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "GunID3"
            strFieldTexts(IDX) = _GunID3
            IDX += 1
        End If

        If _KiteiVal <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "KiteiVal"
            strFieldTexts(IDX) = dblField(_KiteiVal)
            IDX += 1
        End If

        If _KiteiMin <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "KiteiMin"
            strFieldTexts(IDX) = dblField(_KiteiMin)
            IDX += 1
        End If

        If _KiteiMax <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "KiteiMax"
            strFieldTexts(IDX) = dblField(_KiteiMax)
            IDX += 1
        End If

        If _TypeID <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "TypeID"
            strFieldTexts(IDX) = _TypeID
            IDX += 1
        End If

        If _flg_gouhi <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "flg_gouhi"
            strFieldTexts(IDX) = _flg_gouhi
            IDX += 1
        End If

        If _Targettype <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "Targettype"
            strFieldTexts(IDX) = _Targettype
            IDX += 1
        End If

        If _flgOutZu <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "flgOutZu"
            strFieldTexts(IDX) = _flgOutZu
            IDX += 1
        End If

        If _ZU_layer <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "ZU_layer"
            strFieldTexts(IDX) = "'" & _ZU_layer & "'"
            IDX += 1
        End If

        If _ZU_colorID <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "ZU_colorID"
            strFieldTexts(IDX) = _ZU_colorID
            IDX += 1
        End If

        If _ZU_LineTypeID <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "ZU_LineTypeID"
            strFieldTexts(IDX) = _ZU_LineTypeID
            IDX += 1
        End If

        If _flgKeisan <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "flgKeisan"
            strFieldTexts(IDX) = _flgKeisan
            IDX += 1
        End If

        If _flgScale <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "flgScale"
            strFieldTexts(IDX) = _flgScale
            IDX += 1
        End If

    End Sub

    Public Sub SetUpdateField()

        ReDim strFieldNames(14)
        ReDim strFieldTexts(14)

        strFieldNames(0) = "KiteiVal"
        strFieldNames(1) = "KiteiMin"
        strFieldNames(2) = "KiteiMax"
        strFieldNames(3) = "SunpoVal"
        strFieldNames(4) = "Targettype"
        strFieldNames(5) = "flgOutZu"
        strFieldNames(6) = "ZU_layer"
        strFieldNames(7) = "ZU_colorID"
        strFieldNames(8) = "ZU_LineTypeID"
        strFieldNames(9) = "seibunXYZ"
        strFieldNames(10) = "flgKeisan"
        strFieldNames(11) = "flgScale"

        strFieldNames(12) = "GunID1"
        strFieldNames(13) = "GunID2"
        strFieldNames(14) = "GunID3"

        strFieldTexts(0) = dblField(_KiteiVal)
        strFieldTexts(1) = dblField(_KiteiMin)
        strFieldTexts(2) = dblField(_KiteiMax)
        strFieldTexts(3) = dblField(_SunpoVal)
        strFieldTexts(4) = dblField(_Targettype)
        strFieldTexts(5) = dblField(_flgOutZu)
        strFieldTexts(6) = "'" & _ZU_layer & "'"
        strFieldTexts(7) = dblField(_ZU_colorID)
        strFieldTexts(8) = dblField(_ZU_LineTypeID)
        strFieldTexts(9) = dblField(_seibunXYZ)
        strFieldTexts(10) = dblField(_flgKeisan)
        strFieldTexts(11) = dblField(_flgScale)

        strFieldTexts(12) = dblField(_GunID1)
        strFieldTexts(13) = dblField(_GunID2)
        strFieldTexts(14) = dblField(_GunID3)

    End Sub

#Region "データ取得"

    Public Function GetDataToList() As List(Of SunpoSetTable)

        Dim strSql As String = "SELECT * FROM " & S_sunpo_set & " WHERE TypeID = " & CommonTypeID & " AND CT_Active = 1 ORDER BY SunpoID"
        Dim IDR As IDataReader
        Dim bRet As Boolean = False
        Dim SunpoL As List(Of SunpoSetTable) = Nothing

        IDR = m_dbClass.DoSelect(strSql)
        If Not IDR Is Nothing Then
            SunpoL = New List(Of SunpoSetTable)
            Dim IDX As New SunpoSetTable
            IDX.SetFieldIndex(IDR)
            Do While IDR.Read
                Dim SSD As New SunpoSetTable
                SSD.GetDataByReader(IDR)
                SunpoL.Add(SSD)
            Loop
            IDR.Close()
        End If

        GetDataToList = SunpoL

    End Function

    Public Function GetCellDataToList() As List(Of SunpoSetTable)

        ' Dim strSql As String = "SELECT * FROM " & S_sunpo_cell_set & " WHERE TypeID = " & CommonTypeID & " AND CT_Active = 1 ORDER BY SunpoID"
        Dim strSql As String = "SELECT * FROM " & S_sunpo_cell_set & " WHERE TypeID = " & CommonTypeID & " ORDER BY SunpoID"
        Dim IDR As IDataReader
        Dim bRet As Boolean = False
        Dim SunpoL As List(Of SunpoSetTable) = Nothing

        IDR = m_dbClass.DoSelect(strSql)
        If Not IDR Is Nothing Then
            SunpoL = New List(Of SunpoSetTable)
            Dim IDX As New SunpoSetTable
            IDX.SetFieldIndex(IDR)
            Do While IDR.Read
                Dim SSD As New SunpoSetTable
                '20130907 SUURI UPDATE START
                'SSD.GetDataByReader(IDR)
                SSD.GetDataByReaderForCellTable(IDR)
                '20130907 SUURI UPDATE END
                SunpoL.Add(SSD)
            Loop
            IDR.Close()
        End If

        GetCellDataToList = SunpoL

    End Function

#End Region

#Region "Save"

    Public Function ExistsData() As Boolean

        Dim bRet As Boolean = False
        Dim lRet As Long = 0
        Dim strWhere As String = ""

        If _ID.Trim = "" Then
            ExistsData = False
            Exit Function
        End If

        strWhere = "SunpoID = " & _SunpoID.Trim

        lRet = m_dbClass.DoDCount("*", S_sunpo_set, strWhere)
        If lRet > 0 Then
            bRet = True
        End If

        ExistsData = bRet

    End Function

    Public Function SaveData(Optional ByRef flg_trans As Boolean = True) As Boolean

        Dim bRet As Boolean = True

        If ExistsData() = True Then
            'SunpoSetが登録されている場合、Update
            bRet = UpdateData(flg_trans)
        Else
            'SunpoSetが登録されている場合、Insert
            bRet = InsertData(flg_trans)
        End If

        SaveData = bRet

    End Function

#End Region

#Region "Insert"

    Public Function InsertData(Optional ByRef flg_trans As Boolean = True) As Boolean

        InsertData = True

        Dim strWhere As String = "ID = " & _ID

        'CreateFieldText()
        CreateField()

        Dim lRet As Long = 0

        lRet = m_dbClass.DoInsert(strFieldNames, S_sunpo_set, strFieldTexts)
        If lRet = 1 Then
        Else
            m_dbClass.RollbackTrans()
            InsertData = False
        End If

    End Function

#End Region

#Region "Update"

    Public Function UpdateData(Optional ByRef flg_trans As Boolean = True) As Boolean

        UpdateData = True

        Dim strWhere As String = "ID = " & _ID

        SetUpdateField()

        Dim lRet As Long = 0
        If flg_trans = True Then
            m_dbClass.BeginTrans()
            lRet = m_dbClass.DoUpdate(strFieldNames, S_sunpo_set, strFieldTexts, strWhere)
            If lRet = 1 Then
                m_dbClass.CommitTrans()
            Else
                m_dbClass.RollbackTrans()
                UpdateData = False
            End If
        Else
            lRet = m_dbClass.DoUpdate(strFieldNames, S_sunpo_set, strFieldTexts, strWhere)
            If lRet = 1 Then
            Else
                UpdateData = False
            End If
        End If

    End Function

#End Region




End Class
