Public Class SunpoKaisekiTable

    Private _ID As String = ""
    Public Property ID() As String
        Get
            Return _ID
        End Get
        Set(ByVal value As String)
            _ID = value
        End Set
    End Property

    Private _SunpoID As String = ""
    Public Property SunpoID() As String
        Get
            Return _SunpoID
        End Get
        Set(ByVal value As String)
            _SunpoID = value
        End Set
    End Property

    Private _SunpoMark As String = ""
    Public Property SunpoMark() As String
        Get
            Return _SunpoMark
        End Get
        Set(ByVal value As String)
            _SunpoMark = value
        End Set
    End Property

    Private _SunpoName As String = ""
    Public Property SunpoName() As String
        Get
            Return _SunpoName
        End Get
        Set(ByVal value As String)
            _SunpoName = value
        End Set
    End Property

    Private _SunpoCellName As String = ""
    Public Property SunpoCellName() As String
        Get
            Return _SunpoCellName
        End Get
        Set(ByVal value As String)
            _SunpoCellName = value
        End Set
    End Property

    Private _SunpoCalcHouhou As String = ""
    Public Property SunpoCalcHouhou() As String
        Get
            Return _SunpoCalcHouhou
        End Get
        Set(ByVal value As String)
            _SunpoCalcHouhou = value
        End Set
    End Property

    Private _CT_ID1 As String = ""
    Public Property CT_ID1() As String
        Get
            Return _CT_ID1
        End Get
        Set(ByVal value As String)
            _CT_ID1 = value
        End Set
    End Property

    Private _CT_ID2 As String = ""
    Public Property CT_ID2() As String
        Get
            Return _CT_ID2
        End Get
        Set(ByVal value As String)
            _CT_ID2 = value
        End Set
    End Property

    Private _CT_ID3 As String = ""
    Public Property CT_ID3() As String
        Get
            Return _CT_ID3
        End Get
        Set(ByVal value As String)
            _CT_ID3 = value
        End Set
    End Property

    Private _seibunXYZ As String = ""
    Public Property seibunXYZ() As String
        Get
            Return _seibunXYZ
        End Get
        Set(ByVal value As String)
            _seibunXYZ = value
        End Set
    End Property

    Private _CT_Active As String = ""
    Public Property CT_Active() As String
        Get
            Return _CT_Active
        End Get
        Set(ByVal value As String)
            _CT_Active = value
        End Set
    End Property

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

    Private _GunID1 As String = ""
    Public Property GunID1() As String
        Get
            Return _GunID1
        End Get
        Set(ByVal value As String)
            _GunID1 = value
        End Set
    End Property

    Private _GunID2 As String = ""
    Public Property GunID2() As String
        Get
            Return _GunID2
        End Get
        Set(ByVal value As String)
            _GunID2 = value
        End Set
    End Property

    Private _GunID3 As String = ""
    Public Property GunID3() As String
        Get
            Return _GunID3
        End Get
        Set(ByVal value As String)
            _GunID3 = value
        End Set
    End Property

    Private _KiteiVal As String = ""
    Public Property KiteiVal() As String
        Get
            Return _KiteiVal
        End Get
        Set(ByVal value As String)
            _KiteiVal = value
        End Set
    End Property

    Private _KiteiMin As String = ""
    Public Property KiteiMin() As String
        Get
            Return _KiteiMin
        End Get
        Set(ByVal value As String)
            _KiteiMin = value
        End Set
    End Property

    Private _KiteiMax As String = ""
    Public Property KiteiMax() As String
        Get
            Return _KiteiMax
        End Get
        Set(ByVal value As String)
            _KiteiMax = value
        End Set
    End Property

    Private _TypeID As String = ""
    Public Property TypeID() As String
        Get
            Return _TypeID
        End Get
        Set(ByVal value As String)
            _TypeID = value
        End Set
    End Property

    Private _flg_gouhi As String = ""
    Public Property flg_gouhi() As String
        Get
            Return _flg_gouhi
        End Get
        Set(ByVal value As String)
            _flg_gouhi = value
        End Set
    End Property

    Private _sai As String = ""
    Public Property sai() As String
        Get
            Return _sai
        End Get
        Set(ByVal value As String)
            _sai = _SunpoVal - _KiteiVal
        End Set
    End Property

    Public flgSyusei As Boolean = False
    Public flgNewInput As Boolean = False
    Private _backup As SunpoKaisekiTable
    Public Const MaxFieldCnt = 19
    Public strFieldNames() As String
    Public strFieldTexts() As String

    Public SaiL As List(Of SaiTable)

    Public HistgramL As List(Of HistogramTable)

    Public m_dbClass As FBMlib.CDBOperateOLE

    Public Sub New()

    End Sub

    Public Sub BackUp()
        _backup = New SunpoKaisekiTable
        With _backup
            ._ID = Me._ID                               '0  ID
            ._SunpoID = Me._SunpoID                     '1  SunpoID
            ._SunpoMark = Me._SunpoMark                 '2  SunpoMark
            ._SunpoName = Me._SunpoName                 '3  SunpoName
            ._SunpoCellName = Me._SunpoCellName         '4  SunpoCellName
            ._SunpoCalcHouhou = Me._SunpoCalcHouhou     '5  寸法算出方法(2点間距離：１、線と点の距離：２）

            ._CT_ID1 = Me._CT_ID1                       '6  点群ＩＤ１

            ._CT_ID2 = Me._CT_ID2                       '7  点群ＩＤ２

            ._CT_ID3 = Me._CT_ID3                       '8  点群ＩＤ３

            ._seibunXYZ = Me._seibunXYZ                 '9  P1
            ._CT_Active = Me._CT_Active                 '10 P2
            ._SunpoVal = Me._SunpoVal                   '11 P3
            ._GunID1 = Me._GunID1                       '12 
            ._GunID2 = Me._GunID2                       '13 
            ._GunID3 = Me._GunID3                       '14 
            ._KiteiVal = Me._KiteiVal                   '15  KiteiVal
            ._KiteiMin = Me._KiteiMin                   '16  KiteiMin
            ._KiteiMax = Me._KiteiMax                   '17  KiteiMax
            ._TypeID = Me._TypeID                       '18  TypeID
            ._flg_gouhi = Me._flg_gouhi                 '19  flg_gouhi
        End With
    End Sub

    Public Sub copy(ByVal SST As SunpoSetTable)

        With SST
            _ID = .ID                                   '0  ID
            _SunpoID = .SunpoID                         '1  SunpoID
            _SunpoMark = .SunpoMark                     '2  SunpoPart
            _SunpoName = .SunpoName                     '3  SunpoName
            _SunpoCellName = .SunpoCellName             '4  SunpoCellName
            _SunpoCalcHouhou = .SunpoCalcHouhou         '5  寸法算出方法(2点間距離：１、線と点の距離：２）

            _CT_ID1 = .CT_ID1                           '6  点群ＩＤ１

            _CT_ID2 = .CT_ID2                           '7  点群ＩＤ２

            _CT_ID3 = .CT_ID3                           '8  点群ＩＤ３

            _seibunXYZ = .seibunXYZ                     '9  P1
            _CT_Active = .CT_Active                     '10 P2
            _SunpoVal = .SunpoVal                       '11 P3
            _GunID1 = .GunID1                           '12 
            _GunID2 = .GunID2                           '13 
            _GunID3 = .GunID3                           '14 
            _KiteiVal = .KiteiVal                       '15  KiteiVal
            _KiteiMin = .KiteiMin                       '16  KiteiMin
            _KiteiMax = .KiteiMax                       '17  KiteiMax
            _TypeID = .TypeID                           '18  TypeID
            _flg_gouhi = .flg_gouhi                     '19  flg_gouhi
        End With

        BackUp()

    End Sub

End Class

Public Class SaiTable

    Public WorksD As WorksTable
    Public WorksKihonL As List(Of WorksKihonTable)

    Private _sai As String = ""
    Public Property sai() As String
        Get
            Return _sai
        End Get
        Set(ByVal value As String)
            _sai = value
        End Set
    End Property

    Private _SunpoVal As String = ""
    Public Property SunpoVal() As String
        Get
            Return _SunpoVal
        End Get
        Set(ByVal value As String)
            _SunpoVal = value
        End Set
    End Property

End Class

Public Class HistogramTable

    Private _Deviation As Double = CDbl(0)
    Public Property Deviation() As Double
        Get
            Return _Deviation
        End Get
        Set(ByVal value As Double)
            _Deviation = value
        End Set
    End Property

    Private _DataKukanMin As Double = CDbl(0)
    Public Property DataKukanMin() As Double
        Get
            Return _DataKukanMin
        End Get
        Set(ByVal value As Double)
            _DataKukanMin = value
        End Set
    End Property

    Private _DataKukanMax As Double = CDbl(0)
    Public Property DataKukanMax() As Double
        Get
            Return _DataKukanMax
        End Get
        Set(ByVal value As Double)
            _DataKukanMax = value
        End Set
    End Property

    Private _Hindo As Integer = CInt(0)
    Public Property Hindo() As Integer
        Get
            Return _Hindo
        End Get
        Set(ByVal value As Integer)
            _Hindo = value
        End Set
    End Property

End Class
