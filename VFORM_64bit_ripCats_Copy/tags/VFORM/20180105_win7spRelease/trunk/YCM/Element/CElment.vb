Public Enum ElementType     '図形タイプ（1:線分、2：円）
    ElmLine = 1
    ElmCircle = 2
End Enum 'ElementType

Public Class CElment
    Private iType As Integer    '図形タイプ（ElementType）
    Dim gpStart As GeoPoint, gpEnd As GeoPoint
    Dim gpOrigin As GeoPoint, dRad As Double
    Dim dXang As Double, dYang As Double

    Public Sub New(ByVal elmtype As ElementType)
        Me.iType = elmtype
    End Sub
    ' 図形タイプ
    Public ReadOnly Property ElmType() As ElementType
        Get
            Return iType
        End Get
    End Property
    ' 線分の開始点
    Public Property StartPoint() As GeoPoint
        Get
            Return gpStart
        End Get
        Set(ByVal value As GeoPoint)
            gpStart = value
        End Set
    End Property
    ' 線分の終了点
    Public Property EndPoint() As GeoPoint
        Get
            Return gpEnd
        End Get
        Set(ByVal value As GeoPoint)
            gpEnd = value
        End Set
    End Property
    ' 円の中心
    Public Property Origin() As GeoPoint
        Get
            Return gpOrigin
        End Get
        Set(ByVal value As GeoPoint)
            gpOrigin = value
        End Set
    End Property
    ' 円の半径
    Public Property Rad() As Double
        Get
            Return dRad
        End Get
        Set(ByVal value As Double)
            dRad = value
        End Set
    End Property
    Public Property XAng() As Double
        Get
            Return dXang
        End Get
        Set(ByVal value As Double)
            dXang = value
        End Set
    End Property
    Public Property YAng() As Double
        Get
            Return dYang
        End Get
        Set(ByVal value As Double)
            dYang = value
        End Set
    End Property
    ' 線分の設定
    Public Function SetLine(ByVal gpS As GeoPoint, ByVal gpE As GeoPoint) As Integer
        iType = 1
        gpStart = gpS
        gpEnd = gpE
        SetLine = 0
    End Function
    ' 円の設定
    Public Function SetCircle(ByVal gpOrg As GeoPoint, ByVal rad As Double, Optional ByVal xAng As Double = 0, Optional ByVal yAng As Double = 0) As Integer
        iType = 2
        gpOrigin = gpOrg
        dRad = rad
        dXang = xAng
        dYang = yAng
        SetCircle = 0
    End Function
    ' 座標変換
    Public Function Transform(ByVal mat As GeoMatrix) As Integer

    End Function
End Class
