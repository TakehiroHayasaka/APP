﻿Imports Microsoft.VisualBasic.FileIO

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

    Private ReadOnly S_flgScale As String() = {"flgScale"}                  '26 スケールフラグ(20150105 Tezuka ADD)

    Private ReadOnly S_MeasurementSet As String() = {"MeasurementSet"}     '27 (20160822 byambaa(SUSANO) ADD)

    Private ReadOnly S_Sek_flgOutZu As String() = {"sek_flgOutZu"}                  '28 (20160904 byambaa(SUSANO) ADD)
    Private ReadOnly S_Sek_ZU_colorID As String() = {"sek_ZU_colorID"}              '29 (20160904 byambaa(SUSANO) ADD)
    Private ReadOnly S_Sek_ZU_layer As String() = {"sek_ZU_layer"}                  '30 (20160904 byambaa(SUSANO) ADD)
    Private ReadOnly S_Sek_ZU_LineTypeID As String() = {"sek_ZU_LineTypeID"}        '31 (20160904 byambaa(SUSANO) ADD)
    Private ReadOnly S_Sek_Val As String() = {"sek_val"}                            '32 (20161108 baluu(SUSANO) ADD)


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
    '(20160822 byambaa ADD) Start
    Private _measurementSet As String = ""
    Public Property MeasurementSet() As String
        Get
            Return _measurementSet
        End Get
        Set(ByVal value As String)
            If value = "0" Then
                _measurementSet = value
            End If
            _measurementSet = value
        End Set
    End Property

    Private Sub p_set_MeasurementSet(ByRef IDR As IDataReader)
        _measurementSet = GetDataByIndexOfDataReader(FIELD_INDEX(27), IDR)
        If _measurementSet = "0" Then
            _measurementSet = _measurementSet
        End If
    End Sub
    '(20160822 byambaa ADD) End

    '20160904 byambaa(SUSANO) add start
    Private _sek_flgOutZu As String = ""
    Public Property sek_flgOutZu() As String
        Get
            Return _sek_flgOutZu
        End Get
        Set(ByVal value As String)
            _sek_flgOutZu = value
        End Set
    End Property

    Private Sub p_set_sek_flgOutZu(ByRef IDR As IDataReader)
        _sek_flgOutZu = GetDataByIndexOfDataReader(FIELD_INDEX(28), IDR)
    End Sub
    Private _sek_ZU_colorID As String = ""
    Public Property sek_ZU_colorID() As String
        Get
            Return _sek_ZU_colorID
        End Get
        Set(ByVal value As String)
            _sek_ZU_colorID = value
        End Set
    End Property

    Private Sub p_set_sek_ZU_colorID(ByRef IDR As IDataReader)
        _sek_ZU_colorID = GetDataByIndexOfDataReader(FIELD_INDEX(29), IDR)
    End Sub

    Private _sek_ZU_layer As String = ""
    Public Property sek_ZU_layer() As String
        Get
            Return _sek_ZU_layer
        End Get
        Set(ByVal value As String)
            _sek_ZU_layer = value
        End Set
    End Property

    Private Sub p_set_sek_ZU_layer(ByRef IDR As IDataReader)
        _sek_ZU_layer = GetDataByIndexOfDataReader(FIELD_INDEX(30), IDR)
    End Sub

    Private _sek_ZU_LineTypeID As String = ""
    Public Property sek_ZU_LineTypeID() As String
        Get
            Return _sek_ZU_LineTypeID
        End Get
        Set(ByVal value As String)
            _sek_ZU_LineTypeID = value
        End Set
    End Property
    Private Sub p_set_sek_ZU_LineTypeID(ByRef IDR As IDataReader)
        _sek_ZU_LineTypeID = GetDataByIndexOfDataReader(FIELD_INDEX(31), IDR)
    End Sub
    '20160904 byambaa(SUSANO) add end

    '20161108 baluu(SUSANO) add start
    Private _sek_Val As String = ""
    Public Property sek_Val() As String
        Get
            Return _sek_Val
        End Get
        Set(value As String)
            If Not value Is Nothing And Not value = 0 Then '20161129 baluu add start
                _sek_Val = value
                If (_KiteiMin < (_SunpoVal - _sek_Val)) And ((_SunpoVal - _sek_Val) < _KiteiMax) Then
                    _flg_gouhi = "1"
                Else
                    _flg_gouhi = "0"
                End If
            End If '20161129 baluu add end
        End Set
    End Property
    Private Sub p_set_sek_Val(ByRef IDR As IDataReader)
        _sek_Val = GetDataByIndexOfDataReader(FIELD_INDEX(32), IDR)
    End Sub

    '20161108 baluu(SUSANO) add end

    Public flgSyusei As Boolean = False
    Public flgNewInput As Boolean = False
    Private _backup As SunpoSetTable
    Public Const MaxFieldCnt = 32
    Public strFieldNames() As String
    Public strFieldTexts() As String
    Public lstCT_ID As List(Of Integer)
    Public objZu As Object()
    Public objZuSek As Object()

    Public m_dbClass As FBMlib.CDBOperateOLE

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
            ._measurementSet = Me.MeasurementSet    '27  MeasurementSet(20160822 Byambaa ADD)
            ._sek_flgOutZu = Me.sek_flgOutZu        '28  (20160904 Byambaa ADD)
            ._sek_ZU_colorID = Me.sek_ZU_colorID    '29  (20160904 Byambaa ADD)
            ._sek_ZU_layer = Me._sek_ZU_layer       '30  (20160904 Byambaa ADD)
            ._sek_ZU_LineTypeID = Me._sek_ZU_LineTypeID     '31  (20160904 Byambaa ADD)
            ._sek_Val = Me._sek_Val                 '32 (20161108 baluu ADD)

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
            _measurementSet = ._measurementSet      '27  MeasurementSet(20160822 Byambaa ADD)
            _sek_flgOutZu = .sek_flgOutZu           '28  sek_flgOutZu (20160904 Byambaa ADD)
            _sek_ZU_colorID = .sek_ZU_colorID       '29  sek_ZU_colorID (20160904 Byambaa ADD)
            _sek_ZU_layer = ._sek_ZU_layer          '30  _sek_ZU_layer (20160904 Byambaa ADD)
            _sek_ZU_LineTypeID = .sek_ZU_LineTypeID '31  sek_ZU_LineTypeID (20160904 Byambaa ADD)
            _sek_Val = .sek_Val                     '32  sek_val (20161108 baluu ADD)
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
        iList.Add(GetIndexOfDataReader(S_MeasurementSet, IDR))      '27  MeasurementSet(20160822 byambaa ADD)
        iList.Add(GetIndexOfDataReader(S_Sek_flgOutZu, IDR))        '28  sek_flgOutZu
        iList.Add(GetIndexOfDataReader(S_Sek_ZU_colorID, IDR))      '29  sek_ZU_colorID
        iList.Add(GetIndexOfDataReader(S_Sek_ZU_layer, IDR))        '30  sek_ZU_layer
        iList.Add(GetIndexOfDataReader(S_Sek_ZU_LineTypeID, IDR))   '31  sek_ZU_LineTypeID
        iList.Add(GetIndexOfDataReader(S_Sek_Val, IDR))             '32  sek_val (20161108 baluu ADD)
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
        'iList.Add(GetIndexOfDataReader(S_Sek_flgOutZu, IDR))            '27  sek_flgOutZu
        'iList.Add(GetIndexOfDataReader(S_Sek_ZU_colorID, IDR))          '28  sek_ZU_colorID
        'iList.Add(GetIndexOfDataReader(S_Sek_ZU_layer, IDR))            '29  sek_ZU_layer
        'iList.Add(GetIndexOfDataReader(S_ZU_LineTypeID, IDR))           '30  sek_ZU_LineTypeID
        'iList.Add(GetIndexOfDataReader(S_Sek_Val, IDR))                  '31  sek_val (20161108 baluu ADD)
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
        p_set_MeasurementSet(iDataReader)                       '27 MeasurementSet (20160822 byambaa ADD)
        p_set_sek_flgOutZu(iDataReader)                         '28  (20160904 byambaa ADD)
        p_set_sek_ZU_colorID(iDataReader)                       '29  (20160904 byambaa ADD)
        p_set_sek_ZU_layer(iDataReader)                         '30  (20160904 byambaa ADD)
        p_set_sek_ZU_LineTypeID(iDataReader)                    '31  (20160904 byambaa ADD)
        p_set_sek_Val(iDataReader)                              '32  (20161108 baluu ADD)

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

        'p_set_sek_flgOutZu(iDataReader)                         '28  (20160904 byambaa ADD)
        'p_set_sek_ZU_colorID(iDataReader)                       '29  (20160904 byambaa ADD)
        'p_set_sek_ZU_layer(iDataReader)                         '30  (20160904 byambaa ADD)
        'p_set_sek_ZU_LineTypeID(iDataReader)                    '31  (20160904 byambaa ADD)
        'p_set_sek_Val(iDataReader)                              '32  (20161108 baluu ADD)

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
        strFieldNames(26) = "MeasurementSet"                          '20160822 byambaa ADD
        strFieldNames(27) = "sek_flgOutZu"                            '20160904 byambaa ADD
        strFieldNames(28) = "sek_ZU_colorID"                          '20160904 byambaa ADD
        strFieldNames(29) = "sek_ZU_layer"                            '20160904 byambaa ADD
        strFieldNames(30) = "sek_ZU_LineTypeID"                       '20160904 byambaa ADD
        strFieldNames(31) = "sek_val"                                 '20161108 baluu ADD
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
        strFieldTexts(26) = "'" & _measurementSet & "'"                     '20160822 byambaa ADD
        strFieldTexts(27) = _sek_flgOutZu                                   '20160904 byambaa ADD
        strFieldTexts(28) = _sek_ZU_colorID                                 '20160904 byambaa ADD
        strFieldTexts(29) = _sek_ZU_layer                                   '20160904 byambaa ADD
        strFieldTexts(30) = _sek_ZU_LineTypeID                              '20160904 byambaa ADD
        strFieldTexts(31) = _sek_Val                                        '20161108 baluu ADD
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

        '20160822 byambaa ADD start
        ReDim Preserve strFieldNames(IDX)
        ReDim Preserve strFieldTexts(IDX)
        strFieldNames(IDX) = "MeasurementSet"
        strFieldTexts(IDX) = "'" & _measurementSet & "'"
        IDX += 1
        '20160822 byambaa ADD end

        '20160904 byambaa ADD start
        If _sek_flgOutZu <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "sek_flgOutZu"
            strFieldTexts(IDX) = _sek_flgOutZu
            IDX += 1
        End If

        If _sek_ZU_colorID <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "sek_ZU_colorID"
            strFieldTexts(IDX) = _sek_ZU_colorID
            IDX += 1
        End If

        If _sek_ZU_layer <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "sek_ZU_layer"
            strFieldTexts(IDX) = "'" & _sek_ZU_layer & "'"
            IDX += 1
        End If

        If _sek_ZU_LineTypeID <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "sek_ZU_LineTypeID"
            strFieldTexts(IDX) = _sek_ZU_LineTypeID
            IDX += 1
        End If
        '20160904 byambaa ADD end

        '20161108 baluu ADD start
        If _sek_Val <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "sek_val"
            strFieldTexts(IDX) = _sek_Val
            IDX += 1
        End If
        '20161108 baluu ADD end
    End Sub

    Public Sub SetUpdateField()

        ReDim strFieldNames(20)
        ReDim strFieldTexts(20)

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
        strFieldNames(12) = "MeasurementSet"      '20160822 byambaa ADD
        strFieldNames(13) = "sek_flgOutZu"
        strFieldNames(14) = "sek_ZU_colorID"
        strFieldNames(15) = "sek_ZU_layer"
        strFieldNames(16) = "sek_ZU_LineTypeID"
        strFieldNames(17) = "sek_val"


        strFieldNames(18) = "GunID1"
        strFieldNames(19) = "GunID2"
        strFieldNames(20) = "GunID3"


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
        strFieldTexts(12) = "'" & _measurementSet & "'"      '20160822 byambaa ADD
        strFieldTexts(13) = dblField(_sek_flgOutZu)
        strFieldTexts(14) = dblField(_sek_ZU_colorID)
        strFieldTexts(15) = "'" & _sek_ZU_layer & "'"
        strFieldTexts(16) = dblField(_sek_ZU_LineTypeID)
        strFieldTexts(17) = dblField(_sek_Val)

        strFieldTexts(18) = dblField(_GunID1)
        strFieldTexts(19) = dblField(_GunID2)
        strFieldTexts(20) = dblField(_GunID3)

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

#Region "CALC"
    Public Sub SetSunpoCTID()
        If lstCT_ID Is Nothing Then
            lstCT_ID = New List(Of Integer)
        Else
            lstCT_ID.Clear()
        End If
        Try


            Select Case SunpoCalcHouhou

                Case SunpoKeisanHouhou.LineToPoint
                    lstCT_ID.Add(CT_ID1)
                    lstCT_ID.Add(CT_ID2)
                    lstCT_ID.Add(CT_ID3)
                Case SunpoKeisanHouhou.PointToPoint
                    lstCT_ID.Add(CT_ID1)
                    lstCT_ID.Add(CT_ID2)
                Case SunpoKeisanHouhou.MidPointToMidPoint
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    Dim G2() As CT_Data = GetTengunByGunID(GunID2)
                    For Each CTD As CT_Data In G1
                        lstCT_ID.Add(CTD.CT_dat.PID)
                    Next
                    For Each CTD As CT_Data In G2
                        lstCT_ID.Add(CTD.CT_dat.PID)
                    Next
                Case SunpoKeisanHouhou.CCenterToCCenter
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    Dim G2() As CT_Data = GetTengunByGunID(GunID2)
                    For Each CTD As CT_Data In G1
                        lstCT_ID.Add(CTD.CT_dat.PID)
                    Next
                    For Each CTD As CT_Data In G2
                        lstCT_ID.Add(CTD.CT_dat.PID)
                    Next
                Case SunpoKeisanHouhou.CircleToCircle_Max
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    Dim G2() As CT_Data = GetTengunByGunID(GunID2)
                    For Each CTD As CT_Data In G1
                        lstCT_ID.Add(CTD.CT_dat.PID)
                    Next
                    For Each CTD As CT_Data In G2
                        lstCT_ID.Add(CTD.CT_dat.PID)
                    Next
                Case SunpoKeisanHouhou.CircleToCircle_Min
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    Dim G2() As CT_Data = GetTengunByGunID(GunID2)
                    For Each CTD As CT_Data In G1
                        lstCT_ID.Add(CTD.CT_dat.PID)
                    Next
                    For Each CTD As CT_Data In G2
                        lstCT_ID.Add(CTD.CT_dat.PID)
                    Next
                Case SunpoKeisanHouhou.CircleRadius
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    For Each CTD As CT_Data In G1
                        lstCT_ID.Add(CTD.CT_dat.PID)
                    Next
                Case SunpoKeisanHouhou.PgunToPgun
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    Dim G2() As CT_Data = GetTengunByGunID(GunID2)
                    If G1 Is Nothing Then
                    Else
                        For Each CTD As CT_Data In G1
                            lstCT_ID.Add(CTD.CT_dat.PID)
                        Next
                    End If

                    For Each CTD As CT_Data In G2
                        lstCT_ID.Add(CTD.CT_dat.PID)
                    Next
                Case SunpoKeisanHouhou.PlaneToPoint
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    Dim G2() As CT_Data = GetTengunByGunID(GunID2)
                    For Each CTD As CT_Data In G1
                        lstCT_ID.Add(CTD.CT_dat.PID)
                    Next
                    For Each CTD As CT_Data In G2
                        lstCT_ID.Add(CTD.CT_dat.PID)
                    Next
                Case SunpoKeisanHouhou.Toori
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    For Each CTD As CT_Data In G1
                        lstCT_ID.Add(CTD.CT_dat.PID)
                    Next
                Case SunpoKeisanHouhou.MainRaleCamber
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    Dim G2() As CT_Data = GetTengunByGunID(GunID2)
                    Dim G3() As CT_Data = GetTengunByGunID(GunID3)
                    For Each CTD As CT_Data In G1
                        lstCT_ID.Add(CTD.CT_dat.PID)
                    Next
                    For Each CTD As CT_Data In G2
                        lstCT_ID.Add(CTD.CT_dat.PID)
                    Next
                    For Each CTD As CT_Data In G3
                        lstCT_ID.Add(CTD.CT_dat.PID)
                    Next
                Case SunpoKeisanHouhou.CreateCircle
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    For Each CTD As CT_Data In G1
                        lstCT_ID.Add(CTD.CT_dat.PID)
                    Next

                Case SunpoKeisanHouhou.CreateCircle_On_OXY  'SUURI ADD 20140901-20140903
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    For Each CTD As CT_Data In G1
                        Dim dblR As Double = CDbl(KiteiVal)
                        '①CTの法線ベクトルとCTの点で線を作成
                        Dim GL As New GeoCurve
                        Dim CT_Point As New GeoPoint
                        CT_Point.setXYZ(CTD.CT_dat.lstP3d.Item(0).X, CTD.CT_dat.lstP3d.Item(0).Y, CTD.CT_dat.lstP3d.Item(0).Z)
                        GL.SetLineByPointVec(CT_Point, New GeoVector(CTD.CT_dat.NormalV.X, CTD.CT_dat.NormalV.Y, CTD.CT_dat.NormalV.Z))
                        '②OXY面を作成
                        Dim OXYmen As New GeoPlane
                        Dim Opoint As New GeoPoint
                        OXYmen.SetByOriginNormal(Opoint, New GeoVector(0, 0, 1))
                        '①と②の交点を算出
                        Dim NemotoPoint As New GeoPoint
                        NemotoPoint = OXYmen.GetIntersectionWithCurve(GL, True)
                        CT_Point.z = 0
                        '先端点と根元点間の距離がKiteiMaxより大きい場合に円を作図する。
                        If NemotoPoint.GetDistanceTo(CT_Point) >= CDbl(KiteiMax) / ScaleToMM Then
                            lstCT_ID.Add(CTD.CT_dat.PID)
                        End If
                    Next
                Case SunpoKeisanHouhou.CircleDiameter 'SUURI ADD START 20141023
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    For Each CTD As CT_Data In G1
                        lstCT_ID.Add(CTD.CT_dat.PID)
                    Next
                Case SunpoKeisanHouhou.KyoriPlusRaduis
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    Dim G2() As CT_Data = GetTengunByGunID(GunID2)
                    If G1 Is Nothing Then
                    Else
                        For Each CTD As CT_Data In G1
                            lstCT_ID.Add(CTD.CT_dat.PID)
                        Next
                    End If

                    For Each CTD As CT_Data In G2
                        lstCT_ID.Add(CTD.CT_dat.PID)
                    Next
                    Dim G3() As CT_Data = GetTengunByGunID(GunID3)
                    For Each CTD As CT_Data In G3
                        Dim blnAru As Boolean = False
                        For Each IDD As Integer In lstCT_ID
                            If CTD.CT_dat.PID = IDD Then
                                blnAru = True
                            End If
                        Next
                        If blnAru = False Then
                            lstCT_ID.Add(CTD.CT_dat.PID)
                        End If
                    Next
                    'SUURI ADD END 20141024
            End Select

        Catch ex As Exception


        End Try

    End Sub

    Public Function CalcSunpoVal() As Integer
        Try
            'entset_line.linetype.code = CInt(ZU_LineTypeID)
            'entset_line.color.code = CInt(ZU_colorID)
            'entset_circle.linetype.code = CInt(ZU_LineTypeID)
            'entset_circle.color.code = CInt(ZU_colorID)
            Select Case SunpoCalcHouhou
                Case SunpoKeisanHouhou.PointToPoint
                    SunpoVal = GetDist(CInt(CT_ID1), CInt(CT_ID2), seibunXYZ).ToString
                    Dim dd As Object
                    dd = GetDist(CInt(CT_ID1), CInt(CT_ID2), seibunXYZ)
                    'entset_line.linetype.code = CInt(ZU_LineTypeID)
                    'entset_line.color.code = CInt(ZU_colorID)
                    AddUserLine(CInt(CT_ID1), CInt(CT_ID2))
                    ReDim objZu(1)                             ' (20140129 Tezuka ADD)
                    objZu(0) = gDrawUserLines(nUserLines - 1)   ' (20140129 Tezuka ADD)
                    '20170208 baluu add start
                    If objZu(0).GetType Is GetType(CUserLine) Then
                        Dim tObjZu As CUserLine = objZu(0)
                        tObjZu.createType = 1
                    End If
                    '20170208 baluu add end
                    'gDrawUserLines(nUserLines - 1).layerName = ZU_layer  'SUURI ADD 20140818 
                    'AddUserLineWithColorLine(P1, P2, CInt(ZU_colorID), CInt(ZU_LineTypeID))
                Case SunpoKeisanHouhou.LineToPoint
                    flgAddUserLine = False
                    SunpoVal = GetDist(CInt(CT_ID1), CInt(CT_ID2), CInt(CT_ID3), seibunXYZ)
                    flgAddUserLine = True
                    Dim P1 As FBMlib.Point3D = GetGunTeigiTableDataByGunID(GunID1).GetFromTengunNoUnit
                    Dim P2 As FBMlib.Point3D = GetGunTeigiTableDataByGunID(GunID2).GetFromTengunNoUnit
                    Dim P3 As FBMlib.Point3D = GetGunTeigiTableDataByGunID(GunID3).GetFromTengunNoUnit
                    YCM_Sunpo.GetDist(P1, P2, P3, seibunXYZ)
                    ReDim objZu(1)                                 'SUURI ADD 20140817 
                    objZu(0) = gDrawUserLines(nUserLines - 1)      'SUURI ADD 20140817 
                    '20170208 baluu add start
                    If objZu(0).GetType Is GetType(CUserLine) Then
                        Dim tObjZu As CUserLine = objZu(0)
                        tObjZu.createType = 1
                    End If
                    '20170208 baluu add end
                    'gDrawUserLines(nUserLines - 1).layerName = ZU_layer 'SUURI ADD 20140818 
                    flgAddUserLine = False
                Case SunpoKeisanHouhou.MidPointToMidPoint
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    Dim G2() As CT_Data = GetTengunByGunID(GunID2)
                    If G1.Count = 2 And G2.Count = 2 Then
                        Dim P1 As New FBMlib.Point3D
                        Dim P2 As New FBMlib.Point3D
                        P1.GetMidTen(G1(0).CT_dat.lstRealP3d(0), G1(1).CT_dat.lstRealP3d(0))
                        P2.GetMidTen(G2(0).CT_dat.lstRealP3d(0), G2(1).CT_dat.lstRealP3d(0))
                        SunpoVal = GetDist(P1, P2, seibunXYZ)
                        P1.GetMidTen(G1(0).CT_dat.lstP3d(0), G1(1).CT_dat.lstP3d(0))
                        P2.GetMidTen(G2(0).CT_dat.lstP3d(0), G2(1).CT_dat.lstP3d(0))
                        AddUserLine(P1, P2)
                        ReDim objZu(1)                             ' (20140129 Tezuka ADD)
                        objZu(0) = gDrawUserLines(nUserLines - 1)   ' (20140129 Tezuka ADD)
                        '20170208 baluu add start
                        If objZu(0).GetType Is GetType(CUserLine) Then
                            Dim tObjZu As CUserLine = objZu(0)
                            tObjZu.createType = 1
                        End If
                        '20170208 baluu add end
                        'gDrawUserLines(nUserLines - 1).layerName = ZU_layer 'SUURI ADD 20140818 
                    Else
                        'H25.6.28 Yamadaコメントアウト

                        'MsgBox("中点算出用の２点が不正です。！")
                    End If
                Case SunpoKeisanHouhou.CCenterToCCenter
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    Dim G2() As CT_Data = GetTengunByGunID(GunID2)
                    Dim C1 As New GeoCurve
                    Dim C2 As New GeoCurve

                    If G1.Count = 3 And G2.Count = 3 Then
                        YCM_Sunpo.GetCircle3P(G1(0).CT_dat.lstRealP3d(0),
                                               G1(1).CT_dat.lstRealP3d(0),
                                               G1(2).CT_dat.lstRealP3d(0), C1)
                        YCM_Sunpo.GetCircle3P(G2(0).CT_dat.lstRealP3d(0),
                                             G2(1).CT_dat.lstRealP3d(0),
                                             G2(2).CT_dat.lstRealP3d(0), C2)
                        SunpoVal = YCM_Sunpo.GetDist(C1.center.GetP3D, C2.center.GetP3D, seibunXYZ)
                        YCM_Sunpo.GetCircle3P(G1(0).CT_dat.lstP3d(0),
                                             G1(1).CT_dat.lstP3d(0),
                                             G1(2).CT_dat.lstP3d(0), C1)
                        YCM_Sunpo.GetCircle3P(G2(0).CT_dat.lstP3d(0),
                                             G2(1).CT_dat.lstP3d(0),
                                             G2(2).CT_dat.lstP3d(0), C2)
                        AddUserLine(C1.center.GetP3D, C2.center.GetP3D)
                        ReDim objZu(1)                             ' (20140129 Tezuka ADD)
                        objZu(0) = gDrawUserLines(nUserLines - 1)   ' (20140129 Tezuka ADD)
                        '20170208 baluu add start
                        If objZu(0).GetType Is GetType(CUserLine) Then
                            Dim tObjZu As CUserLine = objZu(0)
                            tObjZu.createType = 1
                        End If
                        '20170208 baluu add end
                        ' gDrawUserLines(nUserLines - 1).layerName = ZU_layer 'SUURI ADD 20140818 
                    End If

                Case SunpoKeisanHouhou.CircleToCircle_Max
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    Dim G2() As CT_Data = GetTengunByGunID(GunID2)
                    Dim C1 As New GeoCurve
                    Dim C2 As New GeoCurve

                    If G1.Count = 3 And G2.Count = 3 Then
                        YCM_Sunpo.GetCircle3P(G1(0).CT_dat.lstRealP3d(0),
                                              G1(1).CT_dat.lstRealP3d(0),
                                              G1(2).CT_dat.lstRealP3d(0), C1)
                        YCM_Sunpo.GetCircle3P(G2(0).CT_dat.lstRealP3d(0),
                                             G2(1).CT_dat.lstRealP3d(0),
                                             G2(2).CT_dat.lstRealP3d(0), C2)
                        Dim minPC1, maxPC1, minPC2, maxPC2 As New FBMlib.Point3D
                        YCM_Sunpo.GetMinMaxPointCToC(C1, C2, minPC1, maxPC1, minPC2, maxPC2)
                        SunpoVal = YCM_Sunpo.GetDist(maxPC1, maxPC2, seibunXYZ)
                        YCM_Sunpo.GetCircle3P(G1(0).CT_dat.lstP3d(0),
                                           G1(1).CT_dat.lstP3d(0),
                                           G1(2).CT_dat.lstP3d(0), C1)
                        YCM_Sunpo.GetCircle3P(G2(0).CT_dat.lstP3d(0),
                                             G2(1).CT_dat.lstP3d(0),
                                             G2(2).CT_dat.lstP3d(0), C2)
                        YCM_Sunpo.GetMinMaxPointCToC(C1, C2, minPC1, maxPC1, minPC2, maxPC2)
                        AddUserLine(maxPC1, maxPC2)
                        ReDim objZu(1)                             ' (20140129 Tezuka ADD)
                        objZu(0) = gDrawUserLines(nUserLines - 1)   ' (20140129 Tezuka ADD)
                        '20170208 baluu add start
                        If objZu(0).GetType Is GetType(CUserLine) Then
                            Dim tObjZu As CUserLine = objZu(0)
                            tObjZu.createType = 1
                        End If
                        '20170208 baluu add end
                        '  gDrawUserLines(nUserLines - 1).layerName = ZU_layer 'SUURI ADD 20140818 
                    End If
                Case SunpoKeisanHouhou.CircleToCircle_Min
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    Dim G2() As CT_Data = GetTengunByGunID(GunID2)
                    Dim C1 As New GeoCurve
                    Dim C2 As New GeoCurve

                    If G1.Count = 3 And G2.Count = 3 Then
                        YCM_Sunpo.GetCircle3P(G1(0).CT_dat.lstRealP3d(0),
                                              G1(1).CT_dat.lstRealP3d(0),
                                              G1(2).CT_dat.lstRealP3d(0), C1)
                        YCM_Sunpo.GetCircle3P(G2(0).CT_dat.lstRealP3d(0),
                                             G2(1).CT_dat.lstRealP3d(0),
                                             G2(2).CT_dat.lstRealP3d(0), C2)
                        Dim minPC1, maxPC1, minPC2, maxPC2 As New FBMlib.Point3D
                        YCM_Sunpo.GetMinMaxPointCToC(C1, C2, minPC1, maxPC1, minPC2, maxPC2)
                        SunpoVal = YCM_Sunpo.GetDist(minPC1, minPC2, seibunXYZ)
                        YCM_Sunpo.GetCircle3P(G1(0).CT_dat.lstP3d(0),
                                            G1(1).CT_dat.lstP3d(0),
                                            G1(2).CT_dat.lstP3d(0), C1)
                        YCM_Sunpo.GetCircle3P(G2(0).CT_dat.lstP3d(0),
                                             G2(1).CT_dat.lstP3d(0),
                                             G2(2).CT_dat.lstP3d(0), C2)

                        YCM_Sunpo.GetMinMaxPointCToC(C1, C2, minPC1, maxPC1, minPC2, maxPC2)
                        AddUserLine(minPC1, minPC2)
                        ReDim objZu(1)                             ' (20140129 Tezuka ADD)
                        objZu(0) = gDrawUserLines(nUserLines - 1)   ' (20140129 Tezuka ADD)
                        '20170208 baluu add start
                        If objZu(0).GetType Is GetType(CUserLine) Then
                            Dim tObjZu As CUserLine = objZu(0)
                            tObjZu.createType = 1
                        End If
                        '20170208 baluu add end
                        ' gDrawUserLines(nUserLines - 1).layerName = ZU_layer 'SUURI ADD 20140818 

                    End If
                Case SunpoKeisanHouhou.CircleRadius
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    Dim C1 As New GeoCurve
                    If G1.Count = 3 Then
                        YCM_Sunpo.GetCircle3P(G1(0).CT_dat.lstRealP3d(0),
                                            G1(1).CT_dat.lstRealP3d(0),
                                            G1(2).CT_dat.lstRealP3d(0), C1)
                        SunpoVal = C1.radius
                        YCM_Sunpo.GetCircle3P(G1(0).CT_dat.lstP3d(0),
                                          G1(1).CT_dat.lstP3d(0),
                                          G1(2).CT_dat.lstP3d(0), C1)

                        AddUserCircle3P(G1(0).CT_dat.lstP3d(0),
                                          G1(2).CT_dat.lstP3d(0),
                                          G1(1).CT_dat.lstP3d(0))
                        ReDim objZu(1)                             ' (20140129 Tezuka ADD)
                        objZu(0) = gDrawCircleNew(nCircleNew - 1)   ' (20140129 Tezuka ADD)
                        '20170208 baluu add start
                        If objZu(0).GetType Is GetType(Ccircle) Then
                            Dim tObjZu As Ccircle = objZu(0)
                            tObjZu.createType = 1
                        End If
                        '20170208 baluu add end
                    ElseIf G1.Count > 3 Then
                        GetCirclePointGroup(G1, C1)
                        SunpoVal = C1.radius
                        AddUserCircle1P(New FBMlib.Point3D(C1.center.x, C1.center.y, C1.center.z), C1.radius, C1.normal)
                        ReDim objZu(1)
                        objZu(0) = gDrawCircleNew(nCircleNew - 1)
                        '20170208 baluu add start
                        If objZu(0).GetType Is GetType(Ccircle) Then
                            Dim tObjZu As Ccircle = objZu(0)
                            tObjZu.createType = 1
                        End If
                        '20170208 baluu add end
                    End If
                Case SunpoKeisanHouhou.PgunToPgun
                    Dim P1 As FBMlib.Point3D
                    Dim P2 As FBMlib.Point3D
                    If GunID1 = 0 Then
                        P1 = New FBMlib.Point3D
                        P1.X = 0.0
                        P1.Y = 0.0
                        P1.Z = 0.0
                    Else
                        P1 = GetGunTeigiTableDataByGunID(GunID1).GetFromTengun
                    End If
                    P2 = GetGunTeigiTableDataByGunID(GunID2).GetFromTengun
                    If P1 Is Nothing Or P2 Is Nothing Then
                        flgKeisan = "0"
                    End If
                    SunpoVal = YCM_Sunpo.GetDist(P1, P2, seibunXYZ)
                    If GunID1 = 0 Then
                    Else
                        P1 = New FBMlib.Point3D
                        P1.X = 0.0
                        P1.Y = 0.0
                        P1.Z = 0.0
                        P1 = GetGunTeigiTableDataByGunID(GunID1).GetFromTengunNoUnit
                    End If

                    P2 = GetGunTeigiTableDataByGunID(GunID2).GetFromTengunNoUnit
                    Select Case seibunXYZ
                        Case XYZseibun.X
                            P1.Y = P2.Y
                            P1.Z = P2.Z
                        Case XYZseibun.Y
                            P1.X = P2.X
                            P1.Z = P2.Z
                        Case XYZseibun.Z
                            P1.Y = P2.Y
                            P1.X = P2.X
                    End Select

                    AddUserLine(P1, P2)
                    ReDim objZu(1)                             ' (20140129 Tezuka ADD)
                    objZu(0) = gDrawUserLines(nUserLines - 1)   ' (20140129 Tezuka ADD)
                    '20170208 baluu add start
                    If objZu(0).GetType Is GetType(CUserLine) Then
                        Dim tObjZu As CUserLine = objZu(0)
                        tObjZu.createType = 1
                    End If
                    '20170208 baluu add end
                    If P1 IsNot Nothing And P2 IsNot Nothing Then
                        If flgOutZu = "1" Then
                            gDrawUserLines(nUserLines - 1).blnDraw = True
                        Else
                            gDrawUserLines(nUserLines - 1).blnDraw = False
                        End If
                        ' 20161109 baluu ADD start
                        ' Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                        ' Dim G2() As CT_Data = GetTengunByGunID(GunID2)
                        ' If Not G1(0).SekDat Is Nothing Then
                        '     sek_Val = YCM_Sunpo.GetDist(G1(0).SekDat.PID, G2(0).SekDat.PID, seibunXYZ)
                        ' End If
                        ' 20161109 baluu ADD end
                    End If
                    ' gDrawUserLines(nUserLines - 1).layerName = ZU_layer
                    ' AddUserLineWithColorLine(P1, P2, CInt(ZU_colorID), CInt(ZU_LineTypeID))
                Case SunpoKeisanHouhou.PlaneToPoint
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    Dim P0 As FBMlib.Point3D = GetGunTeigiTableDataByGunID(GunID2).GetFromTengun
                    flgAddUserLine = False
                    SunpoVal = YCM_Sunpo.GetDist(G1(0).CT_dat.lstRealP3d(0),
                                            G1(1).CT_dat.lstRealP3d(0),
                                            G1(2).CT_dat.lstRealP3d(0),
                                            P0, seibunXYZ)

                    P0 = GetGunTeigiTableDataByGunID(GunID2).GetFromTengunNoUnit
                    flgAddUserLine = True
                    YCM_Sunpo.GetDist(G1(0).CT_dat.lstP3d(0),
                                            G1(1).CT_dat.lstP3d(0),
                                            G1(2).CT_dat.lstP3d(0),
                                            P0, seibunXYZ)
                    flgAddUserLine = False
                Case SunpoKeisanHouhou.PointToPoint_XYZsabun
                    Try
                        Dim P1 As FBMlib.Point3D = GetGunTeigiTableDataByGunID(GunID1).GetFromTengun
                        Dim P2 As FBMlib.Point3D = GetGunTeigiTableDataByGunID(GunID2).GetFromTengun
                        SunpoVal = YCM_Sunpo.GetSabunXYZ(P1, P2, seibunXYZ)
                        P1 = GetGunTeigiTableDataByGunID(GunID1).GetFromTengunNoUnit
                        P2 = GetGunTeigiTableDataByGunID(GunID2).GetFromTengunNoUnit
                        AddUserLine(P1, P2)
                        ReDim objZu(1)                             ' (20140129 Tezuka ADD)
                        objZu(0) = gDrawUserLines(nUserLines - 1)   ' (20140129 Tezuka ADD)
                        '20170208 baluu add start
                        If objZu(0).GetType Is GetType(CUserLine) Then
                            Dim tObjZu As CUserLine = objZu(0)
                            tObjZu.createType = 1
                        End If
                        '20170208 baluu add end
                        'gDrawUserLines(nUserLines - 1).layerName = ZU_layer 'SUURI ADD 20140818 
                    Catch ex As Exception

                    End Try

                Case SunpoKeisanHouhou.Toori

                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    Dim D() As Double = {}
                    Dim i As Integer = 0
                    Dim n As Integer = G1.Length - 1
                    Dim C1 As New GeoCurve
                    YCM_Sunpo.GetDists_ByPointsLine(G1, seibunXYZ, D)
                    SunpoVal = D.Max
                    ReDim objZu(n + 2)
                    For i = 0 To n - 1
                        AddUserLine(G1(i).CT_dat.lstP3d.Item(0), G1(i + 1).CT_dat.lstP3d.Item(0))
                        objZu(i) = gDrawUserLines(nUserLines - 1)   ' (20140129 Tezuka ADD)
                        '20170208 baluu add start
                        If objZu(i).GetType Is GetType(CUserLine) Then
                            Dim tObjZu As CUserLine = objZu(i)
                            tObjZu.createType = 1
                        End If
                        '20170208 baluu add end
                        ' gDrawUserLines(nUserLines - 1).layerName = ZU_layer 'SUURI ADD 20140818 
                    Next



                    '20140901　ポリラインの始終点を結ぶか否か-----------------------------------------
                    '20160625 AddKiryu vform.iniで設定変更可能に　1:結ぶ　0:結ばない
                    Dim POLYLINE As Boolean = GetPrivateProfileInt("Command", "POLYLINE", 0, My.Application.Info.DirectoryPath & "\vform.ini")
                    If POLYLINE = True Then
                        AddUserLine(G1(0).CT_dat.lstP3d.Item(0), G1(n).CT_dat.lstP3d.Item(0))
                        objZu(n) = gDrawUserLines(nUserLines - 1)   ' (20140129 Tezuka ADD)
                    End If
                    '20170208 baluu add start
                    If objZu(n).GetType Is GetType(CUserLine) Then
                        Dim tObjZu As CUserLine = objZu(n)
                        tObjZu.createType = 1
                    End If
                    '20170208 baluu add end
                    '20140901　ポリラインの始終点を結ぶか否か-----------------------------------------

                    'GetCirclePointGroup(G1, C1)
                    'AddUserCircle1P(New FBMlib.Point3D(C1.center.x, C1.center.y, C1.center.z), C1.radius, C1.normal)

                    'objZu(n + 1) = gDrawCircleNew(nCircleNew - 1)

                Case SunpoKeisanHouhou.MainRaleCamber
                    '京浜産業トレーラフレームのメインレールキャンバー計算用
                    Dim Ma1 As FBMlib.Point3D = GetGunTeigiTableDataByGunID(GunID1).GetFromTengun
                    Dim Q1 As FBMlib.Point3D = GetGunTeigiTableDataByGunID(GunID2).GetFromTengun
                    Dim Q2 As FBMlib.Point3D = GetGunTeigiTableDataByGunID(GunID3).GetFromTengun

                    Dim dblLevelAdj As Double
                    For Each objSunpo As SunpoSetTable In WorksD.SunpoSetL
                        If objSunpo.SunpoMark = "Leveladj" Then
                            dblLevelAdj = objSunpo.KiteiVal
                        End If
                    Next

                    SunpoVal = Ma1.Z - (Q1.Z + Q2.Z - dblLevelAdj) / 2
                Case SunpoKeisanHouhou.PgunToPgun_no_XYZsabun
                    '京浜産業トレーラフレームの網掛フック計算用
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    Dim G2() As CT_Data = GetTengunByGunID(GunID2)
                    Dim n1 As Integer = G1.Length - 1
                    Dim n2 As Integer = G2.Length - 1
                    Dim n As Integer = 0
                    Dim i As Integer
                    If n1 > n2 Then
                        n = n2
                    Else
                        n = n1
                    End If
                    Dim sabun(n) As Double
                    Dim maxSabun As Double = -1
                    For i = 0 To n
                        sabun(i) = GetDist(G1(i).CT_dat.PID, G2(i).CT_dat.PID, seibunXYZ)
                        If sabun(i) > maxSabun Then
                            maxSabun = sabun(i)
                        End If
                    Next
                    SunpoVal = maxSabun
                    ' 20161109 baluu ADD start
                    ' Dim sekTest(n) As Double
                    ' Dim maxSekTest As Double = -1
                    'For i = 0 To n
                    'If Not G1(i).SekDat Is Nothing Then
                    'sekTest(i) = GetDist(G1(i).SekDat.PID, G2(i).SekDat.PID, seibunXYZ)
                    'If sekTest(i) > maxSekTest Then
                    'maxSekTest = sekTest(i)
                    'End If
                    'End If
                    'Next
                    ' sek_Val = maxSekTest
                    ' 20161109 baluu ADD end
                Case SunpoKeisanHouhou.CreateCircle
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    ReDim objZu(G1.Count + 1)
                    Dim iCnt As Integer = 0
                    For Each CTD As CT_Data In G1
                        Dim dblR As Double = CDbl(KiteiVal)
                        AddUserCircle1P(CTD.CT_dat.lstP3d.Item(0), (dblR / 2), New GeoVector(0, 0, 1))
                        'objZu(iCnt) = New Ccircle                          ' (20140129 Tezuka ADD)
                        objZu(iCnt) = gDrawCircleNew(nCircleNew - 1)    ' (20140129 Tezuka ADD)
                        '20170208 baluu add start
                        If objZu(iCnt).GetType Is GetType(Ccircle) Then
                            Dim tObjZu As Ccircle = objZu(iCnt)
                            tObjZu.createType = 1
                        End If
                        '20170208 baluu add end
                        If flgOutZu = "1" Then
                            gDrawCircleNew(nCircleNew - 1).blnDraw = True
                        Else
                            gDrawCircleNew(nCircleNew - 1).blnDraw = False
                        End If
                        ' gDrawCircleNew(nCircleNew - 1).layerName = ZU_layer
                        iCnt += 1
                        ' #If True Then
                        'If Not CTD.SekDat Is Nothing Then
                        '   If Not CTD.SekDat.lstP3d Is Nothing Then
                        '       If CTD.SekDat.lstP3d.Count > 0 Then
                        '           AddUserCircle1P(CTD.SekDat.lstP3d.Item(0), (dblR / 2), New GeoVector(0, 0, 1))
                        '           objZu(iCnt) = New Ccircle                          ' (20140129 Tezuka ADD)
                        '           objZu(iCnt) = gDrawCircleNew(nCircleNew - 1)    ' (20140129 Tezuka ADD)
                        '       If flgOutZu = "1" Then
                        '           gDrawCircleNew(nCircleNew - 1).blnDraw = True
                        '       Else
                        '           gDrawCircleNew(nCircleNew - 1).blnDraw = False
                        '       End If
                        '           gDrawCircleNew(nCircleNew - 1).layerName = ZU_layer
                        '           iCnt += 1
                        '       End If
                        '   End If
                        'End If
                        ' #End If
                    Next
                    SunpoVal = KiteiVal
                    'SUURI　ADD 20140804 START
                    '円作成機能において、検出した円の数によって合否判定を行う
                    If (_KiteiMin <= (G1.Count)) And ((G1.Count) <= _KiteiMax) Then
                        _flg_gouhi = "1"
                    Else
                        _flg_gouhi = "0"
                    End If
                    'SUURI　ADD 20140804 END
                    'SUURI ADD 20140805 START
                Case SunpoKeisanHouhou.CreateCircle_On_OXY
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    ReDim objZu(G1.Count + 1)
                    Dim iCnt As Integer = 0
                    For Each CTD As CT_Data In G1
                        Dim dblR As Double = CDbl(KiteiVal)
                        '①CTの法線ベクトルとCTの点で線を作成
                        Dim GL As New GeoCurve
                        Dim CT_Point As New GeoPoint
                        CT_Point.setXYZ(CTD.CT_dat.lstP3d.Item(0).X, CTD.CT_dat.lstP3d.Item(0).Y, CTD.CT_dat.lstP3d.Item(0).Z)
                        GL.SetLineByPointVec(CT_Point, New GeoVector(CTD.CT_dat.NormalV.X, CTD.CT_dat.NormalV.Y, CTD.CT_dat.NormalV.Z))
                        '②OXY面を作成
                        Dim OXYmen As New GeoPlane
                        Dim Opoint As New GeoPoint
                        OXYmen.SetByOriginNormal(Opoint, New GeoVector(0, 0, 1))
                        '①と②の交点を算出
                        Dim NemotoPoint As New GeoPoint
                        NemotoPoint = OXYmen.GetIntersectionWithCurve(GL, True)
                        CT_Point.z = 0
                        '先端点と根元点間の距離がKiteiMaxより大きい場合に円を作図する。
                        If NemotoPoint.GetDistanceTo(CT_Point) >= CDbl(KiteiMax) / ScaleToMM Then
                            AddUserCircle1P(New FBMlib.Point3D(NemotoPoint.x, NemotoPoint.y, NemotoPoint.z), (dblR / 2), New GeoVector(0, 0, 1))
                            'objZu(iCnt) = New Ccircle                          ' (20140129 Tezuka ADD)
                            objZu(iCnt) = gDrawCircleNew(nCircleNew - 1)    ' (20140129 Tezuka ADD)
                            '20170208 baluu add start
                            If objZu(iCnt).GetType Is GetType(Ccircle) Then
                                Dim tObjZu As Ccircle = objZu(iCnt)
                                tObjZu.createType = 1
                            End If
                            '20170208 baluu add end
                            If flgOutZu = "1" Then
                                gDrawCircleNew(nCircleNew - 1).blnDraw = True
                            Else
                                gDrawCircleNew(nCircleNew - 1).blnDraw = False
                            End If
                            ' gDrawCircleNew(nCircleNew - 1).layerName = ZU_layer
                            iCnt += 1
                        End If

                    Next

                    SunpoVal = KiteiVal

                    'SUURI　ADD 20140916 START
                    '円作成機能において、検出した円の数によって合否判定を行う
                    If iCnt > 0 Then
                        _flg_gouhi = "0"
                    Else
                        _flg_gouhi = "1"
                    End If
                    'SUURI　ADD 20140916 END

                    'SUURI ADD 20140806 END

                Case SunpoKeisanHouhou.CircleDiameter 'SUURI ADD START 20141023
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    Dim C1 As New GeoCurve
                    If G1.Count = 3 Then
                        YCM_Sunpo.GetCircle3P(G1(0).CT_dat.lstRealP3d(0),
                                            G1(1).CT_dat.lstRealP3d(0),
                                            G1(2).CT_dat.lstRealP3d(0), C1)
                        SunpoVal = C1.radius * 2
                        YCM_Sunpo.GetCircle3P(G1(0).CT_dat.lstP3d(0),
                                          G1(1).CT_dat.lstP3d(0),
                                          G1(2).CT_dat.lstP3d(0), C1)

                        AddUserCircle3P(G1(0).CT_dat.lstP3d(0),
                                          G1(2).CT_dat.lstP3d(0),
                                          G1(1).CT_dat.lstP3d(0))
                        ReDim objZu(1)                             ' (20140129 Tezuka ADD)
                        objZu(0) = gDrawCircleNew(nCircleNew - 1)   ' (20140129 Tezuka ADD)
                        '20170208 baluu add start
                        If objZu(0).GetType Is GetType(Ccircle) Then
                            Dim tObjZu As Ccircle = objZu(0)
                            tObjZu.createType = 1
                        End If
                        '20170208 baluu add end
                    ElseIf G1.Count > 3 Then
                        GetCirclePointGroup(G1, C1)
                        SunpoVal = C1.radius * 2
                        AddUserCircle1P(New FBMlib.Point3D(C1.center.x, C1.center.y, C1.center.z), C1.radius, C1.normal)
                        ReDim objZu(1)
                        objZu(0) = gDrawCircleNew(nCircleNew - 1)
                        '20170208 baluu add start
                        If objZu(0).GetType Is GetType(Ccircle) Then
                            Dim tObjZu As Ccircle = objZu(0)
                            tObjZu.createType = 1
                        End If
                        '20170208 baluu add end
                    End If
                    'SUURI ADD END 20141023
                Case SunpoKeisanHouhou.KyoriPlusRaduis 'SUURI ADD START 20141023
                    Dim P1 As FBMlib.Point3D
                    Dim P2 As FBMlib.Point3D

                    P1 = GetGunTeigiTableDataByGunID(GunID1).GetFromTengun

                    P2 = GetGunTeigiTableDataByGunID(GunID2).GetFromTengun
                    If P1 Is Nothing Or P2 Is Nothing Then
                        flgKeisan = "0"
                    End If
                    SunpoVal = YCM_Sunpo.GetDist(P1, P2, seibunXYZ)

                    Dim G3() As CT_Data = GetTengunByGunID(GunID3)
                    Dim C1 As New GeoCurve
                    If G3.Count = 3 Then
                        YCM_Sunpo.GetCircle3P(G3(0).CT_dat.lstRealP3d(0),
                                            G3(1).CT_dat.lstRealP3d(0),
                                            G3(2).CT_dat.lstRealP3d(0), C1)
                        SunpoVal = SunpoVal + C1.radius
                        YCM_Sunpo.GetCircle3P(G3(0).CT_dat.lstP3d(0),
                                          G3(1).CT_dat.lstP3d(0),
                                          G3(2).CT_dat.lstP3d(0), C1)

                        AddUserCircle3P(G3(0).CT_dat.lstP3d(0),
                                          G3(2).CT_dat.lstP3d(0),
                                          G3(1).CT_dat.lstP3d(0))
                        ReDim objZu(1)
                        objZu(0) = gDrawCircleNew(nCircleNew - 1)
                        '20170208 baluu add start
                        If objZu(0).GetType Is GetType(Ccircle) Then
                            Dim tObjZu As Ccircle = objZu(0)
                            tObjZu.createType = 1
                        End If
                        '20170208 baluu add end
                    ElseIf G3.Count > 3 Then
                        GetCirclePointGroup(G3, C1)
                        SunpoVal = SunpoVal + C1.radius
                        AddUserCircle1P(New FBMlib.Point3D(C1.center.x, C1.center.y, C1.center.z), C1.radius, C1.normal)
                        ReDim objZu(1)
                        objZu(0) = gDrawCircleNew(nCircleNew - 1)
                        '20170208 baluu add start
                        If objZu(0).GetType Is GetType(Ccircle) Then
                            Dim tObjZu As Ccircle = objZu(0)
                            tObjZu.createType = 1
                        End If
                        '20170208 baluu add end
                    End If
                    'SUURI ADD END 20141023
            End Select
            If SunpoVal = 0 Then
                flgKeisan = "0"
            End If
        Catch ex As Exception
            'H25.6.28 Yamadaコメントアウト
            'MsgBox(SunpoName & "の算出が失敗しました。")
        End Try
    End Function

    ' 20161109 baluu ADD start
    Public Function CalcSekVal() As Integer
        Try
            'entset_line.linetype.code = CInt(ZU_LineTypeID)
            'entset_line.color.code = CInt(ZU_colorID)
            'entset_circle.linetype.code = CInt(ZU_LineTypeID)
            'entset_circle.color.code = CInt(ZU_colorID)
            Select Case SunpoCalcHouhou
                Case SunpoKeisanHouhou.PointToPoint
                    sek_Val = GetDist(CInt(CT_ID1), CInt(CT_ID2), seibunXYZ).ToString
                    Dim dd As Object
                    dd = GetDist(CInt(CT_ID1), CInt(CT_ID2), seibunXYZ)
                    'entset_line.linetype.code = CInt(ZU_LineTypeID)
                    'entset_line.color.code = CInt(ZU_colorID)
                    Dim nUserLinesTemp As Integer = nUserLines '20161124 baluu add start
                    AddUserLine(CInt(CT_ID1), CInt(CT_ID2))
                    If nUserLines > nUserLinesTemp Then
                        ReDim objZuSek(1)                             ' (20140129 Tezuka ADD)
                        objZuSek(0) = gDrawUserLines(nUserLines - 1)   ' (20140129 Tezuka ADD)
                        '20170208 baluu add start
                        If objZuSek(0).GetType Is GetType(CUserLine) Then
                            Dim tObjZuSek As CUserLine = objZuSek(0)
                            tObjZuSek.createType = 2
                        End If
                        '20170208 baluu add end
                    End If '20161124 baluu add end

                    'gDrawUserLines(nUserLines - 1).layerName = ZU_layer  'SUURI ADD 20140818 
                    'AddUserLineWithColorLine(P1, P2, CInt(ZU_colorID), CInt(ZU_LineTypeID))

                Case SunpoKeisanHouhou.LineToPoint
                    flgAddUserLine = False
                    sek_Val = GetDist(CInt(CT_ID1), CInt(CT_ID2), CInt(CT_ID3), seibunXYZ)
                    flgAddUserLine = True
                    Dim P1 As FBMlib.Point3D = GetGunTeigiTableDataByGunID(GunID1).GetFromTengunNoUnitBySek
                    Dim P2 As FBMlib.Point3D = GetGunTeigiTableDataByGunID(GunID2).GetFromTengunNoUnitBySek
                    Dim P3 As FBMlib.Point3D = GetGunTeigiTableDataByGunID(GunID3).GetFromTengunNoUnitBySek
                    Dim nUserLinesTemp As Integer = nUserLines '20161124 baluu add start
                    YCM_Sunpo.GetDist(P1, P2, P3, seibunXYZ)
                    If nUserLines > nUserLinesTemp Then
                        ReDim objZuSek(1)                                 'SUURI ADD 20140817 
                        objZuSek(0) = gDrawUserLines(nUserLines - 1)      'SUURI ADD 20140817
                        '20170208 baluu add start
                        If objZuSek(0).GetType Is GetType(CUserLine) Then
                            Dim tObjZuSek As CUserLine = objZuSek(0)
                            tObjZuSek.createType = 2
                        End If
                        '20170208 baluu add end
                    End If '20161124 baluu add end
                    'gDrawUserLines(nUserLines - 1).layerName = ZU_layer 'SUURI ADD 20140818 
                    flgAddUserLine = False
                Case SunpoKeisanHouhou.MidPointToMidPoint
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    Dim G2() As CT_Data = GetTengunByGunID(GunID2)
                    If G1.Count = 2 And G2.Count = 2 Then
                        If Not G1(0).SekDat Is Nothing And G1(1).SekDat Is Nothing And G2(0).SekDat Is Nothing And G2(1).SekDat Is Nothing Then
                            If Not G1(0).SekDat.lstRealP3d Is Nothing And G1(1).SekDat.lstRealP3d Is Nothing And G2(0).SekDat.lstRealP3d Is Nothing And G2(1).SekDat.lstRealP3d Is Nothing Then
                                If G1(0).SekDat.lstRealP3d.Count > 0 And G1(1).SekDat.lstRealP3d.Count > 0 And G2(0).SekDat.lstRealP3d.Count > 0 And G2(1).SekDat.lstRealP3d.Count > 0 Then
                                    Dim P1 As New FBMlib.Point3D
                                    Dim P2 As New FBMlib.Point3D
                                    P1.GetMidTen(G1(0).SekDat.lstRealP3d(0), G1(1).SekDat.lstRealP3d(0))
                                    P2.GetMidTen(G2(0).SekDat.lstRealP3d(0), G2(1).SekDat.lstRealP3d(0))
                                    sek_Val = GetDist(P1, P2, seibunXYZ)
                                    P1.GetMidTen(G1(0).SekDat.lstP3d(0), G1(1).SekDat.lstP3d(0))
                                    P2.GetMidTen(G2(0).SekDat.lstP3d(0), G2(1).SekDat.lstP3d(0))
                                    Dim nUserLinesTemp As Integer = nUserLines '20161124 baluu add start
                                    AddUserLine(P1, P2)
                                    If nUserLines > nUserLinesTemp Then
                                        ReDim objZuSek(1)                             ' (20140129 Tezuka ADD)
                                        objZuSek(0) = gDrawUserLines(nUserLines - 1)   ' (20140129 Tezuka ADD)
                                        '20170208 baluu add start
                                        If objZuSek(0).GetType Is GetType(CUserLine) Then
                                            Dim tObjZuSek As CUserLine = objZuSek(0)
                                            tObjZuSek.createType = 2
                                        End If
                                        '20170208 baluu add end
                                    End If '20161124 baluu add end
                                    'gDrawUserLines(nUserLines - 1).layerName = ZU_layer 'SUURI ADD 20140818 
                                End If
                            End If
                        End If
                    Else
                        'H25.6.28 Yamadaコメントアウト

                        'MsgBox("中点算出用の２点が不正です。！")
                    End If
                Case SunpoKeisanHouhou.CCenterToCCenter
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    Dim G2() As CT_Data = GetTengunByGunID(GunID2)
                    Dim C1 As New GeoCurve
                    Dim C2 As New GeoCurve

                    If G1.Count = 3 And G2.Count = 3 Then
                        If Not G1(0).SekDat Is Nothing And G1(1).SekDat Is Nothing And G1(2).SekDat Is Nothing And G2(0).SekDat Is Nothing And G2(1).SekDat Is Nothing And G2(2).SekDat Is Nothing Then
                            If Not G1(0).SekDat.lstRealP3d Is Nothing And G1(1).SekDat.lstRealP3d Is Nothing And G1(2).SekDat.lstRealP3d Is Nothing And G2(0).SekDat.lstRealP3d Is Nothing And G2(1).SekDat.lstRealP3d Is Nothing And G2(2).SekDat.lstRealP3d Is Nothing Then
                                If G1(0).SekDat.lstRealP3d.Count > 0 And G1(1).SekDat.lstRealP3d.Count > 0 And G1(2).SekDat.lstRealP3d.Count > 0 And G2(0).SekDat.lstRealP3d.Count > 0 And G2(1).SekDat.lstRealP3d.Count > 0 And G2(2).SekDat.lstRealP3d.Count > 0 Then
                                    YCM_Sunpo.GetCircle3P(G1(0).SekDat.lstRealP3d(0),
                                               G1(1).SekDat.lstRealP3d(0),
                                               G1(2).SekDat.lstRealP3d(0), C1)
                                    YCM_Sunpo.GetCircle3P(G2(0).SekDat.lstRealP3d(0),
                                                         G2(1).SekDat.lstRealP3d(0),
                                                         G2(2).SekDat.lstRealP3d(0), C2)
                                    sek_Val = YCM_Sunpo.GetDist(C1.center.GetP3D, C2.center.GetP3D, seibunXYZ)
                                    YCM_Sunpo.GetCircle3P(G1(0).SekDat.lstP3d(0),
                                                         G1(1).SekDat.lstP3d(0),
                                                         G1(2).SekDat.lstP3d(0), C1)
                                    YCM_Sunpo.GetCircle3P(G2(0).SekDat.lstP3d(0),
                                                         G2(1).SekDat.lstP3d(0),
                                                         G2(2).SekDat.lstP3d(0), C2)
                                    Dim nUserLinesTemp As Integer = nUserLines '20161124 baluu add start
                                    AddUserLine(C1.center.GetP3D, C2.center.GetP3D)
                                    If nUserLines > nUserLinesTemp Then
                                        ReDim objZuSek(1)                             ' (20140129 Tezuka ADD)
                                        objZuSek(0) = gDrawUserLines(nUserLines - 1)   ' (20140129 Tezuka ADD)
                                        '20170208 baluu add start
                                        If objZuSek(0).GetType Is GetType(CUserLine) Then
                                            Dim tObjZuSek As CUserLine = objZuSek(0)
                                            tObjZuSek.createType = 2
                                        End If
                                        '20170208 baluu add end
                                    End If '20161124 baluu add end
                                    ' gDrawUserLines(nUserLines - 1).layerName = ZU_layer 'SUURI ADD 20140818 
                                End If
                            End If
                        End If
                    End If

                Case SunpoKeisanHouhou.CircleToCircle_Max
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    Dim G2() As CT_Data = GetTengunByGunID(GunID2)
                    Dim C1 As New GeoCurve
                    Dim C2 As New GeoCurve

                    If G1.Count = 3 And G2.Count = 3 Then
                        If Not G1(0).SekDat Is Nothing And G1(1).SekDat Is Nothing And G1(2).SekDat Is Nothing And G2(0).SekDat Is Nothing And G2(1).SekDat Is Nothing And G2(2).SekDat Is Nothing Then
                            If Not G1(0).SekDat.lstRealP3d Is Nothing And G1(1).SekDat.lstRealP3d Is Nothing And G1(2).SekDat.lstRealP3d Is Nothing And G2(0).SekDat.lstRealP3d Is Nothing And G2(1).SekDat.lstRealP3d Is Nothing And G2(2).SekDat.lstRealP3d Is Nothing Then
                                If G1(0).SekDat.lstRealP3d.Count > 0 And G1(1).SekDat.lstRealP3d.Count > 0 And G1(2).SekDat.lstRealP3d.Count > 0 And G2(0).SekDat.lstRealP3d.Count > 0 And G2(1).SekDat.lstRealP3d.Count > 0 And G2(2).SekDat.lstRealP3d.Count > 0 Then
                                    YCM_Sunpo.GetCircle3P(G1(0).SekDat.lstRealP3d(0),
                                              G1(1).SekDat.lstRealP3d(0),
                                              G1(2).SekDat.lstRealP3d(0), C1)
                                    YCM_Sunpo.GetCircle3P(G2(0).SekDat.lstRealP3d(0),
                                                         G2(1).SekDat.lstRealP3d(0),
                                                         G2(2).SekDat.lstRealP3d(0), C2)
                                    Dim minPC1, maxPC1, minPC2, maxPC2 As New FBMlib.Point3D
                                    YCM_Sunpo.GetMinMaxPointCToC(C1, C2, minPC1, maxPC1, minPC2, maxPC2)
                                    sek_Val = YCM_Sunpo.GetDist(maxPC1, maxPC2, seibunXYZ)
                                    YCM_Sunpo.GetCircle3P(G1(0).SekDat.lstP3d(0),
                                                       G1(1).SekDat.lstP3d(0),
                                                       G1(2).SekDat.lstP3d(0), C1)
                                    YCM_Sunpo.GetCircle3P(G2(0).SekDat.lstP3d(0),
                                                         G2(1).SekDat.lstP3d(0),
                                                         G2(2).SekDat.lstP3d(0), C2)
                                    YCM_Sunpo.GetMinMaxPointCToC(C1, C2, minPC1, maxPC1, minPC2, maxPC2)
                                    Dim nUserLinesTemp As Integer = nUserLines '20161124 baluu add start
                                    AddUserLine(maxPC1, maxPC2)
                                    If nUserLines > nUserLinesTemp Then
                                        ReDim objZuSek(1)                             ' (20140129 Tezuka ADD)
                                        objZuSek(0) = gDrawUserLines(nUserLines - 1)   ' (20140129 Tezuka ADD)
                                        '20170208 baluu add start
                                        If objZuSek(0).GetType Is GetType(CUserLine) Then
                                            Dim tObjZuSek As CUserLine = objZuSek(0)
                                            tObjZuSek.createType = 2
                                        End If
                                        '20170208 baluu add end
                                    End If '20161124 baluu add end
                                    '  gDrawUserLines(nUserLines - 1).layerName = ZU_layer 'SUURI ADD 20140818 
                                End If
                            End If
                        End If
                    End If
                Case SunpoKeisanHouhou.CircleToCircle_Min
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    Dim G2() As CT_Data = GetTengunByGunID(GunID2)
                    Dim C1 As New GeoCurve
                    Dim C2 As New GeoCurve

                    If G1.Count = 3 And G2.Count = 3 Then
                        If Not G1(0).SekDat Is Nothing And G1(1).SekDat Is Nothing And G1(2).SekDat Is Nothing And G2(0).SekDat Is Nothing And G2(1).SekDat Is Nothing And G2(2).SekDat Is Nothing Then
                            If Not G1(0).SekDat.lstRealP3d Is Nothing And G1(1).SekDat.lstRealP3d Is Nothing And G1(2).SekDat.lstRealP3d Is Nothing And G2(0).SekDat.lstRealP3d Is Nothing And G2(1).SekDat.lstRealP3d Is Nothing And G2(2).SekDat.lstRealP3d Is Nothing Then
                                If G1(0).SekDat.lstRealP3d.Count > 0 And G1(1).SekDat.lstRealP3d.Count > 0 And G1(2).SekDat.lstRealP3d.Count > 0 And G2(0).SekDat.lstRealP3d.Count > 0 And G2(1).SekDat.lstRealP3d.Count > 0 And G2(2).SekDat.lstRealP3d.Count > 0 Then
                                    YCM_Sunpo.GetCircle3P(G1(0).SekDat.lstRealP3d(0),
                                              G1(1).SekDat.lstRealP3d(0),
                                              G1(2).SekDat.lstRealP3d(0), C1)
                                    YCM_Sunpo.GetCircle3P(G2(0).SekDat.lstRealP3d(0),
                                                         G2(1).SekDat.lstRealP3d(0),
                                                         G2(2).SekDat.lstRealP3d(0), C2)
                                    Dim minPC1, maxPC1, minPC2, maxPC2 As New FBMlib.Point3D
                                    YCM_Sunpo.GetMinMaxPointCToC(C1, C2, minPC1, maxPC1, minPC2, maxPC2)
                                    sek_Val = YCM_Sunpo.GetDist(minPC1, minPC2, seibunXYZ)
                                    YCM_Sunpo.GetCircle3P(G1(0).SekDat.lstP3d(0),
                                                        G1(1).SekDat.lstP3d(0),
                                                        G1(2).SekDat.lstP3d(0), C1)
                                    YCM_Sunpo.GetCircle3P(G2(0).SekDat.lstP3d(0),
                                                         G2(1).SekDat.lstP3d(0),
                                                         G2(2).SekDat.lstP3d(0), C2)

                                    YCM_Sunpo.GetMinMaxPointCToC(C1, C2, minPC1, maxPC1, minPC2, maxPC2)
                                    Dim nUserLinesTemp As Integer = nUserLines '20161124 baluu add start
                                    AddUserLine(minPC1, minPC2)
                                    If nUserLines > nUserLinesTemp Then
                                        ReDim objZuSek(1)                             ' (20140129 Tezuka ADD)
                                        objZuSek(0) = gDrawUserLines(nUserLines - 1)   ' (20140129 Tezuka ADD)
                                        '20170208 baluu add start
                                        If objZuSek(0).GetType Is GetType(CUserLine) Then
                                            Dim tObjZuSek As CUserLine = objZuSek(0)
                                            tObjZuSek.createType = 2
                                        End If
                                        '20170208 baluu add end
                                    End If '20161124 baluu add end
                                    ' gDrawUserLines(nUserLines - 1).layerName = ZU_layer 'SUURI ADD 20140818 
                                End If
                            End If
                        End If
                    End If
                Case SunpoKeisanHouhou.CircleRadius
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    Dim C1 As New GeoCurve
                    If G1.Count = 3 Then
                        If Not (G1(0).SekDat Is Nothing And G1(1).SekDat Is Nothing And G1(2).SekDat Is Nothing) Then '20161130 baluu edit
                            If Not (G1(0).SekDat.lstRealP3d Is Nothing And G1(1).SekDat.lstRealP3d Is Nothing And G1(2).SekDat.lstRealP3d Is Nothing) Then '20161130 baluu edit
                                If G1(0).SekDat.lstRealP3d.Count > 0 And G1(1).SekDat.lstRealP3d.Count > 0 And G1(2).SekDat.lstRealP3d.Count > 0 Then
                                    YCM_Sunpo.GetCircle3P(G1(0).SekDat.lstRealP3d(0),
                                            G1(1).SekDat.lstRealP3d(0),
                                            G1(2).SekDat.lstRealP3d(0), C1)
                                    sek_Val = C1.radius
                                    YCM_Sunpo.GetCircle3P(G1(0).SekDat.lstP3d(0),
                                                      G1(1).SekDat.lstP3d(0),
                                                      G1(2).SekDat.lstP3d(0), C1)
                                    Dim nCircleTemp As Integer = nCircleNew '20161124 baluu add start '20161129 baluu edit
                                    AddUserCircle3P(G1(0).SekDat.lstP3d(0),
                                                      G1(2).SekDat.lstP3d(0),
                                                      G1(1).SekDat.lstP3d(0))
                                    If nCircleNew > nCircleTemp Then '20161129 baluu edit
                                        ReDim objZuSek(1)                             ' (20140129 Tezuka ADD)
                                        objZuSek(0) = gDrawCircleNew(nCircleNew - 1)   ' (20140129 Tezuka ADD)
                                        '20170208 baluu add start
                                        If objZuSek(0).GetType Is GetType(Ccircle) Then
                                            Dim tObjZuSek As Ccircle = objZuSek(0)
                                            tObjZuSek.createType = 2
                                        End If
                                        '20170208 baluu add end
                                    End If '20161124 baluu add end
                                End If
                            End If
                        End If
                    ElseIf G1.Count > 3 Then
                        GetCirclePointGroup(G1, C1)
                        sek_Val = C1.radius
                        Dim nCircleTemp As Integer = nCircleNew '20161124 baluu add start '20161129 baluu edit
                        AddUserCircle1P(New FBMlib.Point3D(C1.center.x, C1.center.y, C1.center.z), C1.radius, C1.normal)
                        If nCircleNew > nCircleTemp Then '20161129 baluu edit
                            ReDim objZuSek(1)
                            objZuSek(0) = gDrawCircleNew(nCircleNew - 1)
                            '20170208 baluu add start
                            If objZuSek(0).GetType Is GetType(Ccircle) Then
                                Dim tObjZuSek As Ccircle = objZuSek(0)
                                tObjZuSek.createType = 2
                            End If
                            '20170208 baluu add end
                        End If '20161124 baluu add end
                    End If
                Case SunpoKeisanHouhou.PgunToPgun
                    Dim P1 As FBMlib.Point3D
                    Dim P2 As FBMlib.Point3D
                    If GunID1 = 0 Then
                        P1 = New FBMlib.Point3D
                        P1.X = 0.0
                        P1.Y = 0.0
                        P1.Z = 0.0
                    Else
                        P1 = GetGunTeigiTableDataByGunID(GunID1).GetFromTengunBySek
                    End If
                    P2 = GetGunTeigiTableDataByGunID(GunID2).GetFromTengunBySek
                    If P1 Is Nothing Or P2 Is Nothing Then
                        'flgKeisan = "0" '20161124 baluu edit
                    End If
                    sek_Val = YCM_Sunpo.GetDist(P1, P2, seibunXYZ)
                    If GunID1 = 0 Then
                    Else
                        P1 = New FBMlib.Point3D
                        P1.X = 0.0
                        P1.Y = 0.0
                        P1.Z = 0.0
                        P1 = GetGunTeigiTableDataByGunID(GunID1).GetFromTengunNoUnitBySek
                    End If

                    P2 = GetGunTeigiTableDataByGunID(GunID2).GetFromTengunNoUnitBySek
                    Select Case seibunXYZ
                        Case XYZseibun.X
                            P1.Y = P2.Y
                            P1.Z = P2.Z
                        Case XYZseibun.Y
                            P1.X = P2.X
                            P1.Z = P2.Z
                        Case XYZseibun.Z
                            P1.Y = P2.Y
                            P1.X = P2.X
                    End Select
                    Dim nUserLinesTemp As Integer = nUserLines '20161124 baluu add start
                    AddUserLine(P1, P2)
                    If nUserLines > nUserLinesTemp Then
                        ReDim objZuSek(1)                             ' (20140129 Tezuka ADD)
                        objZuSek(0) = gDrawUserLines(nUserLines - 1)   ' (20140129 Tezuka ADD)
                        '20170208 baluu add start
                        If objZuSek(0).GetType Is GetType(CUserLine) Then
                            Dim tObjZuSek As CUserLine = objZuSek(0)
                            tObjZuSek.createType = 2
                        End If
                        '20170208 baluu add end
                    End If '20161124 baluu add end
                    If P1 IsNot Nothing And P2 IsNot Nothing Then
                        If sek_flgOutZu = "1" Then
                            gDrawUserLines(nUserLines - 1).blnDraw = True
                        Else
                            gDrawUserLines(nUserLines - 1).blnDraw = False
                        End If
                    End If
                    ' gDrawUserLines(nUserLines - 1).layerName = ZU_layer
                    ' AddUserLineWithColorLine(P1, P2, CInt(ZU_colorID), CInt(ZU_LineTypeID))
                Case SunpoKeisanHouhou.PlaneToPoint
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    If Not G1(0).SekDat Is Nothing And G1(1).SekDat Is Nothing And G1(2).SekDat Is Nothing Then
                        If Not G1(0).SekDat.lstRealP3d Is Nothing And G1(1).SekDat.lstRealP3d Is Nothing And G1(2).SekDat.lstRealP3d Is Nothing Then
                            If G1(0).SekDat.lstRealP3d.Count > 0 And G1(1).SekDat.lstRealP3d.Count > 0 And G1(2).SekDat.lstRealP3d.Count > 0 Then
                                Dim P0 As FBMlib.Point3D = GetGunTeigiTableDataByGunID(GunID2).GetFromTengunBySek
                                flgAddUserLine = False
                                sek_Val = YCM_Sunpo.GetDist(G1(0).SekDat.lstRealP3d(0),
                                                        G1(1).SekDat.lstRealP3d(0),
                                                        G1(2).SekDat.lstRealP3d(0),
                                                        P0, seibunXYZ)

                                P0 = GetGunTeigiTableDataByGunID(GunID2).GetFromTengunNoUnitBySek
                                flgAddUserLine = True
                                YCM_Sunpo.GetDist(G1(0).SekDat.lstP3d(0),
                                                        G1(1).SekDat.lstP3d(0),
                                                        G1(2).SekDat.lstP3d(0),
                                                        P0, seibunXYZ)
                                flgAddUserLine = False
                            End If
                        End If
                    End If
                Case SunpoKeisanHouhou.PointToPoint_XYZsabun
                    Try
                        Dim P1 As FBMlib.Point3D = GetGunTeigiTableDataByGunID(GunID1).GetFromTengunBySek
                        Dim P2 As FBMlib.Point3D = GetGunTeigiTableDataByGunID(GunID2).GetFromTengunBySek
                        sek_Val = YCM_Sunpo.GetSabunXYZ(P1, P2, seibunXYZ)
                        P1 = GetGunTeigiTableDataByGunID(GunID1).GetFromTengunNoUnitBySek
                        P2 = GetGunTeigiTableDataByGunID(GunID2).GetFromTengunNoUnitBySek
                        Dim nUserLinesTemp As Integer = nUserLines '20161124 baluu add start
                        AddUserLine(P1, P2)
                        If nUserLines > nUserLinesTemp Then
                            ReDim objZuSek(1)                             ' (20140129 Tezuka ADD)
                            objZuSek(0) = gDrawUserLines(nUserLines - 1)   ' (20140129 Tezuka ADD)
                            '20170208 baluu add start
                            If objZuSek(0).GetType Is GetType(CUserLine) Then
                                Dim tObjZuSek As CUserLine = objZuSek(0)
                                tObjZuSek.createType = 2
                            End If
                            '20170208 baluu add end
                        End If '20161124 baluu add end
                        'gDrawUserLines(nUserLines - 1).layerName = ZU_layer 'SUURI ADD 20140818 
                    Catch ex As Exception

                    End Try

                Case SunpoKeisanHouhou.Toori

                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    Dim D() As Double = {}
                    Dim i As Integer = 0
                    Dim n As Integer = G1.Length - 1
                    Dim C1 As New GeoCurve
                    YCM_Sunpo.GetDists_ByPointsLine(G1, seibunXYZ, D)
                    '20161202 baluu add start
                    If D.Length > 0 Then
                        sek_Val = D.Max
                        ReDim objZuSek(n + 2)
                        For i = 0 To n - 1
                            If Not (G1(i).SekDat Is Nothing And G1(i + 1).SekDat Is Nothing) Then
                                If Not (G1(i).SekDat.lstP3d Is Nothing And G1(i + 1).SekDat.lstP3d Is Nothing) Then
                                    If G1(i).SekDat.lstP3d.Count > 0 And G1(i + 1).SekDat.lstP3d.Count > 0 Then
                                        Dim nUserLinesTemp As Integer = nUserLines '20161124 baluu add start
                                        AddUserLine(G1(i).SekDat.lstP3d.Item(0), G1(i + 1).SekDat.lstP3d.Item(0))
                                        If nUserLines > nUserLinesTemp Then
                                            objZuSek(i) = gDrawUserLines(nUserLines - 1)   ' (20140129 Tezuka ADD)
                                            '20170208 baluu add start
                                            If objZuSek(i).GetType Is GetType(CUserLine) Then
                                                Dim tObjZuSek As CUserLine = objZuSek(i)
                                                tObjZuSek.createType = 2
                                            End If
                                            '20170208 baluu add end
                                        End If '20161124 baluu add end
                                        ' gDrawUserLines(nUserLines - 1).layerName = ZU_layer 'SUURI ADD 20140818 
                                    End If
                                End If
                            End If
                        Next

                        '20140901　ポリラインの始終点を結ぶか否か-----------------------------------------
                        If Not (G1(0).SekDat Is Nothing And G1(n).SekDat Is Nothing) Then
                            If Not (G1(0).SekDat.lstP3d Is Nothing And G1(n).SekDat.lstP3d Is Nothing) Then
                                If G1(0).SekDat.lstP3d.Count > 0 And G1(n).SekDat.lstP3d.Count > 0 Then
                                    Dim nUserLinesTemp As Integer = nUserLines '20161124 baluu add start
                                    AddUserLine(G1(0).SekDat.lstP3d.Item(0), G1(n).SekDat.lstP3d.Item(0))
                                    If nUserLines > nUserLinesTemp Then
                                        objZuSek(n) = gDrawUserLines(nUserLines - 1)   ' (20140129 Tezuka ADD)
                                        '20170208 baluu add start
                                        If objZuSek(n).GetType Is GetType(CUserLine) Then
                                            Dim tObjZuSek As CUserLine = objZuSek(n)
                                            tObjZuSek.createType = 2
                                        End If
                                        '20170208 baluu add end
                                    End If '20161124 baluu add end
                                End If
                            End If
                        End If

                        '20140901　ポリラインの始終点を結ぶか否か-----------------------------------------

                        'GetCirclePointGroup(G1, C1)
                        'AddUserCircle1P(New FBMlib.Point3D(C1.center.x, C1.center.y, C1.center.z), C1.radius, C1.normal)

                        'objZu(n + 1) = gDrawCircleNew(nCircleNew - 1)
                    Else
                        sek_Val = 0
                    End If

                Case SunpoKeisanHouhou.MainRaleCamber
                    '京浜産業トレーラフレームのメインレールキャンバー計算用
                    Dim Ma1 As FBMlib.Point3D = GetGunTeigiTableDataByGunID(GunID1).GetFromTengunBySek
                    Dim Q1 As FBMlib.Point3D = GetGunTeigiTableDataByGunID(GunID2).GetFromTengunBySek
                    Dim Q2 As FBMlib.Point3D = GetGunTeigiTableDataByGunID(GunID3).GetFromTengunBySek

                    Dim dblLevelAdj As Double
                    For Each objSunpo As SunpoSetTable In WorksD.SunpoSetL
                        If objSunpo.SunpoMark = "Leveladj" Then
                            dblLevelAdj = objSunpo.KiteiVal
                        End If
                    Next

                    sek_Val = Ma1.Z - (Q1.Z + Q2.Z - dblLevelAdj) / 2
                Case SunpoKeisanHouhou.PgunToPgun_no_XYZsabun
                    '京浜産業トレーラフレームの網掛フック計算用
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    Dim G2() As CT_Data = GetTengunByGunID(GunID2)
                    Dim n1 As Integer = G1.Length - 1
                    Dim n2 As Integer = G2.Length - 1
                    Dim n As Integer = 0
                    Dim i As Integer
                    If n1 > n2 Then
                        n = n2
                    Else
                        n = n1
                    End If
                    Dim sabun(n) As Double
                    Dim maxSabun As Double = -1
                    For i = 0 To n
                        If Not G1(i).SekDat Is Nothing And G2(i).SekDat Is Nothing Then
                            If Not G1(i).SekDat.lstRealP3d Is Nothing And G2(i).SekDat.lstRealP3d Is Nothing Then
                                If G1(i).SekDat.lstRealP3d.Count > 0 And G2(i).SekDat.lstRealP3d.Count > 0 Then
                                    sabun(i) = GetDist(G1(i).SekDat.lstRealP3d.Item(0), G2(i).SekDat.lstRealP3d.Item(0), seibunXYZ)
                                    If sabun(i) > maxSabun Then
                                        maxSabun = sabun(i)
                                    End If
                                End If
                            End If
                        End If
                    Next
                    sek_Val = maxSabun
                Case SunpoKeisanHouhou.CreateCircle
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    ReDim objZuSek(G1.Count + 1)
                    Dim iCnt As Integer = 0
                    For Each CTD As CT_Data In G1
                        Dim dblR As Double = CDbl(KiteiVal)
#If True Then
                        If Not CTD.SekDat Is Nothing Then
                            If Not CTD.SekDat.lstP3d Is Nothing Then
                                If CTD.SekDat.lstP3d.Count > 0 Then
                                    Dim nCircleNewTemp As Integer = nCircleNew '20161124 baluu add start
                                    AddUserCircle1P(CTD.SekDat.lstP3d.Item(0), (dblR / 2), New GeoVector(0, 0, 1))
                                    If nCircleNew > nCircleNewTemp Then
                                        'objZu(iCnt) = New Ccircle                          
                                        objZuSek(iCnt) = gDrawCircleNew(nCircleNew - 1)
                                        '20170208 baluu add start
                                        If objZuSek(iCnt).GetType Is GetType(Ccircle) Then
                                            Dim tObjZuSek As Ccircle = objZuSek(iCnt)
                                            tObjZuSek.createType = 2
                                        End If
                                        '20170208 baluu add end
                                    End If '20161124 baluu add end
                                    If sek_flgOutZu = "1" Then
                                        gDrawCircleNew(nCircleNew - 1).blnDraw = True
                                    Else
                                        gDrawCircleNew(nCircleNew - 1).blnDraw = False
                                    End If
                                    ' gDrawCircleNew(nCircleNew - 1).layerName = ZU_layer
                                End If
                            End If
                        End If
#End If
                        iCnt += 1
                    Next
                    sek_Val = KiteiVal
                Case SunpoKeisanHouhou.CreateCircle_On_OXY
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    ReDim objZuSek(G1.Count + 1)
                    Dim iCnt As Integer = 0
                    For Each CTD As CT_Data In G1
                        Dim dblR As Double = CDbl(KiteiVal)
                        '①CTの法線ベクトルとCTの点で線を作成
                        Dim GL As New GeoCurve
                        Dim CT_Point As New GeoPoint
                        If Not CTD.SekDat Is Nothing Then
                            If Not CTD.SekDat.lstP3d Is Nothing Then
                                If CTD.SekDat.lstP3d.Count > 0 Then
                                    CT_Point.setXYZ(CTD.SekDat.lstP3d.Item(0).X, CTD.SekDat.lstP3d.Item(0).Y, CTD.SekDat.lstP3d.Item(0).Z)
                                    GL.SetLineByPointVec(CT_Point, New GeoVector(CTD.SekDat.NormalV.X, CTD.SekDat.NormalV.Y, CTD.SekDat.NormalV.Z))
                                    '②OXY面を作成
                                    Dim OXYmen As New GeoPlane
                                    Dim Opoint As New GeoPoint
                                    OXYmen.SetByOriginNormal(Opoint, New GeoVector(0, 0, 1))
                                    '①と②の交点を算出
                                    Dim NemotoPoint As New GeoPoint
                                    NemotoPoint = OXYmen.GetIntersectionWithCurve(GL, True)
                                    CT_Point.z = 0
                                    '先端点と根元点間の距離がKiteiMaxより大きい場合に円を作図する。
                                    If NemotoPoint.GetDistanceTo(CT_Point) >= CDbl(KiteiMax) / ScaleToMM Then
                                        Dim nCircleTemp As Integer = nCircleNew '20161124 baluu add start '20161129 baluu edit
                                        AddUserCircle1P(New FBMlib.Point3D(NemotoPoint.x, NemotoPoint.y, NemotoPoint.z), (dblR / 2), New GeoVector(0, 0, 1))
                                        If nCircleNew > nCircleTemp Then '20161129 baluu edit
                                            'objZu(iCnt) = New Ccircle                          ' (20140129 Tezuka ADD)
                                            objZuSek(iCnt) = gDrawCircleNew(nCircleNew - 1)    ' (20140129 Tezuka ADD)
                                            '20170208 baluu add start
                                            If objZuSek(iCnt).GetType Is GetType(Ccircle) Then
                                                Dim tObjZuSek As Ccircle = objZuSek(iCnt)
                                                tObjZuSek.createType = 2
                                            End If
                                            '20170208 baluu add end
                                        End If '20161124 baluu add end
                                        If sek_flgOutZu = "1" Then
                                            gDrawCircleNew(nCircleNew - 1).blnDraw = True
                                        Else
                                            gDrawCircleNew(nCircleNew - 1).blnDraw = False
                                        End If
                                        ' gDrawCircleNew(nCircleNew - 1).layerName = ZU_layer
                                        iCnt += 1
                                    End If
                                End If
                            End If
                        End If
                    Next

                    sek_Val = KiteiVal

                    'SUURI　ADD 20140916 START
                    '円作成機能において、検出した円の数によって合否判定を行う
                    If iCnt > 0 Then
                        _flg_gouhi = "0" '20161129 baluu edit
                    Else
                        _flg_gouhi = "1" '20161129 baluu edit
                    End If
                    'SUURI　ADD 20140916 END

                    'SUURI ADD 20140806 END

                Case SunpoKeisanHouhou.CircleDiameter 'SUURI ADD START 20141023
                    Dim G1() As CT_Data = GetTengunByGunID(GunID1)
                    Dim C1 As New GeoCurve
                    If G1.Count = 3 Then
                        If Not (G1(0).SekDat Is Nothing And G1(1).SekDat Is Nothing And G1(2).SekDat Is Nothing) Then
                            If Not (G1(0).SekDat.lstP3d Is Nothing And G1(1).SekDat.lstP3d Is Nothing And G1(2).SekDat.lstP3d Is Nothing) Then
                                If G1(0).SekDat.lstP3d.Count > 0 And G1(1).SekDat.lstP3d.Count > 0 And G1(2).SekDat.lstP3d.Count > 0 Then
                                    YCM_Sunpo.GetCircle3P(G1(0).SekDat.lstRealP3d(0),
                                            G1(1).SekDat.lstRealP3d(0),
                                            G1(2).SekDat.lstRealP3d(0), C1)
                                    sek_Val = C1.radius * 2
                                    YCM_Sunpo.GetCircle3P(G1(0).SekDat.lstP3d(0),
                                                      G1(1).SekDat.lstP3d(0),
                                                      G1(2).SekDat.lstP3d(0), C1)
                                    Dim nCircleTemp As Integer = nCircleNew '20161124 baluu add start '20161129 baluu edit
                                    AddUserCircle3P(G1(0).SekDat.lstP3d(0),
                                                      G1(2).SekDat.lstP3d(0),
                                                      G1(1).SekDat.lstP3d(0))
                                    If nCircleNew > nCircleTemp Then
                                        ReDim objZuSek(1)                             ' (20140129 Tezuka ADD)
                                        objZuSek(0) = gDrawCircleNew(nCircleNew - 1)   ' (20140129 Tezuka ADD)
                                        '20170208 baluu add start
                                        If objZuSek(0).GetType Is GetType(Ccircle) Then
                                            Dim tObjZuSek As Ccircle = objZuSek(0)
                                            tObjZuSek.createType = 2
                                        End If
                                        '20170208 baluu add end
                                    End If '20161124 baluu add end
                                End If
                            End If
                        End If
                    ElseIf G1.Count > 3 Then
                        GetCirclePointGroup(G1, C1)
                        sek_Val = C1.radius * 2
                        Dim nCircleTemp As Integer = nCircleNew '20161124 baluu add start '20161129 baluu edit
                        AddUserCircle1P(New FBMlib.Point3D(C1.center.x, C1.center.y, C1.center.z), C1.radius, C1.normal)
                        If nCircleNew > nCircleTemp Then
                            ReDim objZuSek(1)
                            objZuSek(0) = gDrawCircleNew(nCircleNew - 1)
                            '20170208 baluu add start
                            If objZuSek(0).GetType Is GetType(Ccircle) Then
                                Dim tObjZuSek As Ccircle = objZuSek(0)
                                tObjZuSek.createType = 2
                            End If
                            '20170208 baluu add end
                        End If '20161124 baluu add end
                    End If
                    'SUURI ADD END 20141023
                Case SunpoKeisanHouhou.KyoriPlusRaduis 'SUURI ADD START 20141023
                    Dim P1 As FBMlib.Point3D
                    Dim P2 As FBMlib.Point3D

                    P1 = GetGunTeigiTableDataByGunID(GunID1).GetFromTengunBySek

                    P2 = GetGunTeigiTableDataByGunID(GunID2).GetFromTengunBySek
                    If P1 Is Nothing Or P2 Is Nothing Then
                        'flgKeisan = "0" '20161124 baluu edit
                    End If
                    sek_Val = YCM_Sunpo.GetDist(P1, P2, seibunXYZ)

                    Dim G3() As CT_Data = GetTengunByGunID(GunID3)
                    Dim C1 As New GeoCurve
                    If G3.Count = 3 Then
                        If Not G3(0).SekDat Is Nothing And G3(1).SekDat Is Nothing And G3(2).SekDat Is Nothing Then
                            If Not G3(0).SekDat.lstP3d Is Nothing And G3(1).SekDat.lstP3d Is Nothing And G3(2).SekDat.lstP3d Is Nothing Then
                                If G3(0).SekDat.lstP3d.Count > 0 And G3(1).SekDat.lstP3d.Count > 0 And G3(2).SekDat.lstP3d.Count > 0 Then
                                    YCM_Sunpo.GetCircle3P(G3(0).SekDat.lstRealP3d(0),
                                            G3(1).SekDat.lstRealP3d(0),
                                            G3(2).SekDat.lstRealP3d(0), C1)
                                    sek_Val = sek_Val + C1.radius
                                    YCM_Sunpo.GetCircle3P(G3(0).SekDat.lstP3d(0),
                                                      G3(1).SekDat.lstP3d(0),
                                                      G3(2).SekDat.lstP3d(0), C1)
                                    Dim nCircleTemp As Integer = nCircleNew '20161124 baluu add start '20161129 baluu edit
                                    AddUserCircle3P(G3(0).SekDat.lstP3d(0),
                                                      G3(2).SekDat.lstP3d(0),
                                                      G3(1).SekDat.lstP3d(0))
                                    If nCircleNew > nCircleTemp Then
                                        ReDim objZuSek(1)
                                        objZuSek(0) = gDrawCircleNew(nCircleNew - 1)
                                        '20170208 baluu add start
                                        If objZuSek(0).GetType Is GetType(Ccircle) Then
                                            Dim tObjZuSek As Ccircle = objZuSek(0)
                                            tObjZuSek.createType = 2
                                        End If
                                        '20170208 baluu add end
                                    End If '20161124 baluu add end
                                End If
                            End If
                        End If
                    ElseIf G3.Count > 3 Then
                        GetCirclePointGroup(G3, C1)
                        sek_Val = sek_Val + C1.radius
                        Dim nCircleTemp As Integer = nCircleNew '20161124 baluu add start '20161129 baluu edit
                        AddUserCircle1P(New FBMlib.Point3D(C1.center.x, C1.center.y, C1.center.z), C1.radius, C1.normal)
                        If nCircleNew > nCircleTemp Then '20161129 baluu edit
                            ReDim objZuSek(1)
                            objZuSek(0) = gDrawCircleNew(nCircleNew - 1)
                            '20170208 baluu add start
                            If objZuSek(0).GetType Is GetType(Ccircle) Then
                                Dim tObjZuSek As Ccircle = objZuSek(0)
                                tObjZuSek.createType = 2
                            End If
                            '20170208 baluu add end
                        End If '20161124 baluu add end
                    End If
                    'SUURI ADD END 20141023
            End Select
        Catch ex As Exception
            'H25.6.28 Yamadaコメントアウト
            MsgBox(SunpoName & "の算出が失敗しました。")
        End Try
    End Function
    ' 20161109 baluu ADD end


#End Region

    Public Sub CsvOut(ByVal strFileName As String, ByRef system_dbclass As FBMlib.CDBOperateOLE)


        Dim strKekka As String = ""
        Dim strSql As String = "SELECT * FROM " & "SunpoSet" & " WHERE TypeID = " & CommonTypeID & " ORDER BY SunpoID"
        Dim SunpoL As List(Of SunpoSetTable) = Nothing
        Dim strSelect As String = "SunpoID" _
       & "," & "SunpoMark" _
       & "," & "SunpoName" _
       & "," & "SunpoCellName" _
       & "," & "KiteiVal" _
       & "," & "KiteiMin" _
       & "," & "KiteiMax" _
       & "," & "SunpoCalcHouhou" _
       & "," & "CT_ID1" _
       & "," & "CT_ID2" _
       & "," & "CT_ID3" _
       & "," & "seibunXYZ" _
       & "," & "CT_Active" _
       & "," & "SunpoVal" _
       & "," & "GunID1" _
       & "," & "GunID2" _
       & "," & "GunID3" _
       & "," & "flg_gouhi" _
       & "," & "Targettype" _
       & "," & "flgOutZu" _
       & "," & "ZU_layer" _
       & "," & "ZU_colorID" _
       & "," & "ZU_LineTypeID" _
       & "," & "flgKeisan" _
       & "," & "flgScale" _
       & "," & "sek_flgOutZu" _
       & "," & "sek_ZU_layer" _
       & "," & "sek_ZU_colorID" _
       & "," & "sek_ZU_LineTypeID"
        My.Computer.FileSystem.WriteAllText(strFileName, strSelect + vbNewLine, True)

        Dim IDR As IDataReader = system_dbclass.DoSelect(strSql)
        If Not IDR Is Nothing Then
            'SunpoL = New List(Of SunpoSetTable)
            Dim IDX As New SunpoSetTable
            IDX.SetFieldIndex(IDR)
            Do While IDR.Read
                'Dim SSD As New SunpoSetTable
                '20130907 SUURI UPDATE START
                'SSD.GetDataByReader(IDR)
                Me.GetDataByReader(IDR)
                '20130907 SUURI UPDATE END
                'SunpoL.Add(SSD)
                'SSD.CsvOut(ExportDirPath + "\" + "db_out_SunpoSetTabel.csv")
                strKekka = _SunpoID _
                & "," & _SunpoMark _
                & "," & _SunpoName _
                & "," & _SunpoCellName _
                & "," & _KiteiVal _
                & "," & _KiteiMin _
                & "," & _KiteiMax _
                & "," & _SunpoCalcHouhou _
                & "," & _CT_ID1 _
                & "," & _CT_ID2 _
                & "," & _CT_ID3 _
                & "," & _seibunXYZ _
                & "," & _CT_Active _
                & "," & _SunpoVal _
                & "," & _GunID1 _
                & "," & _GunID2 _
                & "," & _GunID3 _
                & "," & _flg_gouhi _
                & "," & _Targettype _
                & "," & _flgOutZu _
                & "," & _ZU_layer _
                & "," & _ZU_colorID _
                & "," & _ZU_LineTypeID _
                & "," & _flgKeisan _
                & "," & _flgScale _
                & "," & _sek_flgOutZu _
                & "," & _sek_ZU_layer _
                & "," & _sek_ZU_colorID _
                & "," & _sek_ZU_LineTypeID _
                & "," & _sek_Val _
                & vbNewLine

                My.Computer.FileSystem.WriteAllText(strFileName, strKekka, True)

            Loop
            IDR.Close()
        End If


        'My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & strFileName, strKekka, True)
        'My.Computer.FileSystem.WriteAllText(strFileName, strKekka, True)


    End Sub

    Public Sub CsvInport(ByVal strFileName As String, ByRef dbclass As FBMlib.CDBOperateOLE, ByVal TypeID As Integer)

        'Dim filename As String = My.Application.Info.DirectoryPath & "\" & strFileName
        Dim filename As String = strFileName
        Dim fields As String()
        Dim delimiter As String = ","
        Dim i As Integer = 0

        Dim strSelect As String

        strSelect = "SunpoID" _
        & "," & "SunpoMark" _
        & "," & "SunpoName" _
        & "," & "SunpoCellName" _
        & "," & "KiteiVal" _
        & "," & "KiteiMin" _
        & "," & "KiteiMax" _
        & "," & "SunpoCalcHouhou" _
        & "," & "CT_ID1" _
        & "," & "CT_ID2" _
        & "," & "CT_ID3" _
        & "," & "seibunXYZ" _
        & "," & "CT_Active" _
        & "," & "SunpoVal" _
        & "," & "GunID1" _
        & "," & "GunID2" _
        & "," & "GunID3" _
        & "," & "flg_gouhi" _
        & "," & "Targettype" _
        & "," & "flgOutZu" _
        & "," & "ZU_layer" _
        & "," & "ZU_colorID" _
        & "," & "ZU_LineTypeID" _
        & "," & "flgKeisan" _
        & "," & "flgScale" _
        & "," & "sek_flgOutZu" _
        & "," & "sek_ZU_layer" _
        & "," & "sek_ZU_colorID" _
        & "," & "sek_ZU_LineTypeID" _
        & "," & "TypeID"
        '& "," & "sek_Val" _


        Using parser As New TextFieldParser(filename, System.Text.Encoding.Default)
            parser.SetDelimiters(delimiter)
            fields = parser.ReadFields() 'ヘッダの読み飛ばし

            While Not parser.EndOfData
                ' Read in the fields for the current line
                fields = parser.ReadFields()
                ' Add code here to use data in fields variable.
                If IsNumeric(fields(0)) Then
                    'Dim tmp As New Sunposet(fields)

                    Dim strValue As String = fields(0)
                    strValue = strValue & ",'" & fields(1) & "'"
                    strValue = strValue & ",'" & fields(2) & "'"
                    strValue = strValue & ",'" & fields(3) & "'"
                    strValue = strValue & "," & fields(4)
                    strValue = strValue & "," & fields(5)
                    strValue = strValue & "," & fields(6)
                    strValue = strValue & "," & fields(7)
                    strValue = strValue & "," & fields(8)
                    strValue = strValue & "," & fields(9)
                    strValue = strValue & "," & fields(10)
                    strValue = strValue & "," & fields(11)
                    strValue = strValue & "," & fields(12)
                    strValue = strValue & "," & fields(13)
                    strValue = strValue & "," & fields(14)
                    strValue = strValue & "," & fields(15)
                    strValue = strValue & "," & fields(16)
                    strValue = strValue & "," & fields(17)
                    strValue = strValue & "," & fields(18)
                    strValue = strValue & "," & fields(19)
                    strValue = strValue & ",'" & fields(20) & "'"
                    strValue = strValue & "," & fields(21)
                    strValue = strValue & "," & fields(22)
                    strValue = strValue & "," & fields(23)
                    strValue = strValue & "," & fields(24)
                    strValue = strValue & "," & fields(25)
                    strValue = strValue & ",'" & fields(26) & "'"
                    strValue = strValue & "," & fields(27)
                    strValue = strValue & "," & fields(28)
                    'strValue = strValue & "," & fields(29)
                    strValue = strValue & "," & TypeID

                    Dim strSql As String = "INSERT INTO SunpoSet(" & strSelect & ") " & " VALUES( " & strValue & " ) "

                    dbclass.ExecuteSQL(strSql)
                    'End With

                End If
            End While
        End Using
    End Sub


End Class
