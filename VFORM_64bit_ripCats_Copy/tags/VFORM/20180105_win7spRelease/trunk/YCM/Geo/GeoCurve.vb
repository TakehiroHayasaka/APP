Option Strict Off
Option Explicit On
Public Class GeoCurve
    '==============================================================================
    '   GeoCurve
    '==============================================================================
    '==============================================================================
    ' 仕様:
    '   CurveType = geoLine のとき、半径、中心、法線ベクトルは、0、(0,0,0)、(0,0,0) となります
    '   CurveType = geoCircle のとき、始点・終点は、中心から法線ベクトルの垂直方向(GetPerpendicular)に
    '   半径だけ離れた位置となります
    '   CurveType = geoArc のとき、円弧の向きは、法線ベクトルのなす平面に対して反時計回り方向となります
    '==============================================================================
    Private m_enmType As GeoCurveTypeEnum ' 曲線種別
    Private m_startPoint As GeoPoint ' 始点
    Private m_endPoint As GeoPoint ' 終点
    Private m_dblRadius As Double ' 半径/主軸半径(>=0)
    Private m_center As GeoPoint ' 中心
    Private m_normal As GeoVector ' 法線ベクトル
    Private m_majorAxis As GeoVector ' 主軸方向ベクトル
    Private m_minorAxis As GeoVector ' 副軸方向ベクトル
    Private m_dblRadius2 As Double ' 副軸半径(>=0)
    Private m_minPoint As GeoPoint ' 最小座標
    Private m_maxPoint As GeoPoint ' 最大座標

    '==============================================================================
    Public Sub Class_Initialize_Renamed()
        m_enmType = GeoCurveTypeEnum.geoLine
        m_dblRadius = 0.0#
        m_center = New GeoPoint
        m_normal = New GeoVector
        m_startPoint = New GeoPoint
        m_endPoint = New GeoPoint
        m_majorAxis = New GeoVector
        m_minorAxis = New GeoVector
        m_dblRadius2 = 0.0#
        'H25.5.27追加　Yamada
        m_minPoint = New GeoPoint
        m_maxPoint = New GeoPoint

    End Sub
    Public Sub New()
        MyBase.New()
        Class_Initialize_Renamed()
    End Sub

    '==============================================================================
    Public Sub Class_Terminate_Renamed()
        On Error Resume Next
        m_center = Nothing
        m_normal = Nothing
        m_startPoint = Nothing
        m_endPoint = Nothing
        m_majorAxis = Nothing
        m_minorAxis = Nothing
    End Sub
    Protected Overrides Sub Finalize()
        Class_Terminate_Renamed()
        MyBase.Finalize()
    End Sub

    '==============================================================================
    Private Sub InitializeGeoCurve()
        m_enmType = GeoCurveTypeEnum.geoLine
        m_dblRadius = 0.0#
        Call m_center.setXYZ(0, 0, 0)
        Call m_normal.setXYZ(0, 0, 0)
        Call m_startPoint.setXYZ(0, 0, 0)
        Call m_endPoint.setXYZ(0, 0, 0)
        Call m_majorAxis.setXYZ(0, 0, 0)
        Call m_minorAxis.setXYZ(0, 0, 0)
        m_dblRadius2 = 0.0#
    End Sub

    '==============================================================================
    Public Sub Assign(ByRef Curve As GeoCurve)
        CurveType = Curve.CurveType
        Call StartPoint.Assign(Curve.StartPoint)
        Call EndPoint.Assign(Curve.EndPoint)
        radius = Curve.radius
        Call center.Assign(Curve.center)
        Call normal.Assign(Curve.normal)
        Call majorAxis.Assign(Curve.majorAxis)
        Call MinorAxis.Assign(Curve.MinorAxis)
        MinorRadius = Curve.MinorRadius
    End Sub
    Public Function Copy() As GeoCurve
        Copy = New GeoCurve
        Call Copy.Assign(Me)
    End Function

    '==============================================================================
    Public Sub SetLine(ByRef point1 As GeoPoint, ByRef point2 As GeoPoint)
        InitializeGeoCurve()
        CurveType = GeoCurveTypeEnum.geoLine
        Call StartPoint.Assign(point1)
        Call EndPoint.Assign(point2)
    End Sub

    '==============================================================================
    Public Sub SetLineByPointVec(ByRef point As GeoPoint, ByRef dir_Renamed As GeoVector)
        InitializeGeoCurve()
        CurveType = GeoCurveTypeEnum.geoLine
        Call StartPoint.Assign(point)
        Call EndPoint.Assign(point.GetAddedVec(dir_Renamed))
    End Sub

    '==============================================================================
    Public Sub SetLineByVec(ByRef dir_Renamed As GeoVector)
        InitializeGeoCurve()
        CurveType = GeoCurveTypeEnum.geoLine
        Call EndPoint.AddVec(dir_Renamed)
    End Sub

    '==============================================================================
    Public Sub SetArcByCenterStartEndAngle(ByRef centerPoint As GeoPoint, ByRef dblRadius As Double, ByRef dblStartAngle As Double, ByRef dblEndAngle As Double, ByRef normalVec As GeoVector)
        InitializeGeoCurve()
        CurveType = GeoCurveTypeEnum.geoArc
        m_center = centerPoint.Copy
        m_dblRadius = dblRadius
        m_normal = normalVec.GetNormal
        m_startPoint = m_center.GetAddedVec(m_normal.GetPerpendicular.GetRotatedBy(dblStartAngle, m_normal).GetMultipled(dblRadius))
        m_endPoint = m_center.GetAddedVec(m_normal.GetPerpendicular.GetRotatedBy(dblEndAngle, m_normal).GetMultipled(dblRadius))
    End Sub

    '==============================================================================
    Public Sub SetCircleByCenter(ByRef centerPoint As GeoPoint, ByRef dblRadius As Double, ByRef normalVec As GeoVector)
        InitializeGeoCurve()
        CurveType = GeoCurveTypeEnum.geoCircle
        m_center = centerPoint.Copy
        m_dblRadius = dblRadius
        m_normal = normalVec.GetNormal
        m_startPoint = m_center.GetAddedVec(m_normal.GetPerpendicular.GetRotatedBy(0, m_normal).GetMultipled(dblRadius))
        m_endPoint = m_startPoint.Copy
    End Sub

    ''==============================================================================
    'Public Sub SetEllipseByCenter(ByRef centerPoint As GeoPoint, ByRef majorAxisVec As GeoVector, ByRef dblMajorRadius As Double, ByRef dblMinorRadius As Double, ByRef normalVec As GeoVector)
    '    Dim geoEllipse As Object
    '    InitializeGeoCurve()
    '    CurveType = geoEllipse
    '    m_center = centerPoint.Copy
    '    m_dblRadius = dblMajorRadius
    '    m_dblRadius2 = dblMinorRadius
    '    m_normal = normalVec.GetNormal
    '    Call m_majorAxis.Assign(majorAxisVec.GetNormal)
    '    Call m_minorAxis.Assign(normalVec.GetOuterProduct(majorAxisVec).GetNormal)
    'End Sub

    '==============================================================================
    '==============================================================================
    ' 許容誤差内で等価かどうかを返す
    Public Function IsEqualTo(ByRef Curve As GeoCurve, ByRef dblTol As Double) As Boolean
        IsEqualTo = False
        If CurveType = GeoCurveTypeEnum.geoLine Or CurveType = GeoCurveTypeEnum.geoArc Or CurveType = GeoCurveTypeEnum.geoCircle Or CurveType = GeoCurveTypeEnum.geoEllipArc Then
            If StartPoint.IsEqualTo(Curve.StartPoint, dblTol) = False Then Exit Function
            If EndPoint.IsEqualTo(Curve.EndPoint, dblTol) = False Then Exit Function
        End If
        If CurveType = GeoCurveTypeEnum.geoArc Or CurveType = GeoCurveTypeEnum.geoCircle Or CurveType = GeoCurveTypeEnum.geoEllipArc Or CurveType = GeoCurveTypeEnum.geoEllipse Then
            If center.IsEqualTo(Curve.center, dblTol) = False Then Exit Function
            If normal.IsEqualTo(Curve.normal, dblTol) = False Then Exit Function
            If Math_IsEqual(radius, Curve.radius, dblTol) = False Then Exit Function
        End If
        If CurveType = GeoCurveTypeEnum.geoEllipArc Or CurveType = GeoCurveTypeEnum.geoEllipse Then
            If majorAxis.IsEqualTo(Curve.majorAxis, dblTol) = False Then Exit Function
            If MinorAxis.IsEqualTo(Curve.MinorAxis, dblTol) = False Then Exit Function
            If Math_IsEqual(MinorRadius, Curve.MinorRadius, dblTol) = False Then Exit Function
        End If
        IsEqualTo = True
    End Function

    '==============================================================================
    '==============================================================================
    ' 許容誤差内で同じ形状かどうかを返す
    '   線分: 端点が等しければ、始終端が逆でも可
    '   円弧: 中心・端点・法線方向・角度範囲が等しければ、掃引方向が逆でも可
    Public Function IsEqualShapeTo(ByRef Curve As GeoCurve, ByRef dblTol As Double) As Boolean
        If CurveType = GeoCurveTypeEnum.geoLine Or CurveType = GeoCurveTypeEnum.geoArc Or CurveType = GeoCurveTypeEnum.geoCircle Then
            If Not (StartPoint.IsEqualTo(Curve.StartPoint, dblTol) And EndPoint.IsEqualTo(Curve.EndPoint, dblTol)) And Not (StartPoint.IsEqualTo(Curve.EndPoint, dblTol) And EndPoint.IsEqualTo(Curve.StartPoint, dblTol)) Then Exit Function
        End If
        If CurveType = GeoCurveTypeEnum.geoArc Or CurveType = GeoCurveTypeEnum.geoCircle Then
            If Not center.IsEqualTo(Curve.center, dblTol) Then Exit Function
            '///// 方向が逆でも可能にする必要あり - 未実装
            If Not normal.IsEqualTo(Curve.normal, dblTol) Then Exit Function
            If Not Math_IsEqual(radius, Curve.radius, dblTol) Then Exit Function
        End If
        IsEqualShapeTo = True
    End Function

    '==============================================================================
    '==============================================================================
    Public Property CurveType() As GeoCurveTypeEnum
        Get
            CurveType = m_enmType
        End Get
        Set(ByVal Value As GeoCurveTypeEnum)
            m_enmType = Value
        End Set
    End Property
    '==============================================================================
    Public Property StartPoint() As GeoPoint
        Get
            StartPoint = m_startPoint
        End Get
        Set(ByVal Value As GeoPoint)
            Call m_startPoint.Assign(Value)
        End Set
    End Property
    '==============================================================================
    Public Property EndPoint() As GeoPoint
        Get
            EndPoint = m_endPoint
        End Get
        Set(ByVal Value As GeoPoint)
            Call m_endPoint.Assign(Value)
        End Set
    End Property
    '==============================================================================
    Public Property radius() As Double
        Get
            radius = m_dblRadius
        End Get
        Set(ByVal Value As Double)
            m_dblRadius = Value
        End Set
    End Property
    '==============================================================================
    Public Property center() As GeoPoint
        Get
            center = m_center
        End Get
        Set(ByVal Value As GeoPoint)
            Call m_center.Assign(Value)
        End Set
    End Property
    '==============================================================================
    Public Property normal() As GeoVector
        Get
            normal = m_normal
        End Get
        Set(ByVal Value As GeoVector)
            Call m_normal.Assign(Value)
        End Set
    End Property
    '==============================================================================
    Public Property majorAxis() As GeoVector
        Get
            majorAxis = m_majorAxis
        End Get
        Set(ByVal Value As GeoVector)
            Call m_majorAxis.Assign(Value)
        End Set
    End Property
    '==============================================================================
    Public Property MinorAxis() As GeoVector
        Get
            MinorAxis = m_minorAxis
        End Get
        Set(ByVal Value As GeoVector)
            Call m_minorAxis.Assign(Value)
        End Set
    End Property
    '==============================================================================
    Public Property MajorRadius() As Double
        Get
            MajorRadius = m_dblRadius
        End Get
        Set(ByVal Value As Double)
            m_dblRadius = Value
        End Set
    End Property
    '==============================================================================
    Public Property MinorRadius() As Double
        Get
            MinorRadius = m_dblRadius2
        End Get
        Set(ByVal Value As Double)
            m_dblRadius2 = Value
        End Set
    End Property
    Public Property SP() As GeoPoint
        Get
            SP = m_startPoint
        End Get
        Set(ByVal Value As GeoPoint)
            Call m_startPoint.Assign(Value)
        End Set
    End Property
    Public Property EP() As GeoPoint
        Get
            EP = m_endPoint
        End Get
        Set(ByVal Value As GeoPoint)
            Call m_endPoint.Assign(Value)
        End Set
    End Property

    '==============================================================================
    '==============================================================================
    ' 開始角度を得る
    Public Function GetStartAngle() As Double
        If CurveType = GeoCurveTypeEnum.geoArc Or CurveType = GeoCurveTypeEnum.geoEllipArc Then
            GetStartAngle = normal.GetPerpendicular.GetAngleTo(StartPoint.GetSubtracted(center), normal)
            If GetStartAngle < 0 Then
                GetStartAngle = Math_Pai() * 2 + GetStartAngle
            End If
        End If
    End Function

    '==============================================================================
    ' 終了角度を得る
    Public Function GetEndAngle() As Double
        If CurveType = GeoCurveTypeEnum.geoArc Or CurveType = GeoCurveTypeEnum.geoEllipArc Then
            GetEndAngle = normal.GetPerpendicular.GetAngleTo(EndPoint.GetSubtracted(center), normal)
            If GetEndAngle < 0 Then
                GetEndAngle = Math_Pai() * 2 + GetEndAngle
            End If
        End If
    End Function

    '==============================================================================
    ' 掃引角度を得る
    Public Function GetSweepAngle() As Double
        Dim dblAngle1, dblAngle2 As Double
        If CurveType = GeoCurveTypeEnum.geoArc Or CurveType = GeoCurveTypeEnum.geoEllipArc Then
            dblAngle1 = GetStartAngle()
            dblAngle2 = GetEndAngle()
            If dblAngle1 > dblAngle2 Then
                dblAngle2 = dblAngle2 + Math_Pai() * 2
            End If
            GetSweepAngle = dblAngle2 - dblAngle1
        ElseIf CurveType = GeoCurveTypeEnum.geoCircle Or CurveType = GeoCurveTypeEnum.geoEllipse Then
            GetSweepAngle = Math_Pai() * 2
        End If
    End Function

    '==============================================================================
    ' 曲線長を得る
    Public Function GetLength() As Double
        If CurveType = GeoCurveTypeEnum.geoLine Then
            GetLength = StartPoint.GetDistanceTo(EndPoint)
        ElseIf CurveType = GeoCurveTypeEnum.geoArc Or CurveType = GeoCurveTypeEnum.geoCircle Then
            GetLength = radius * GetSweepAngle()
        End If
    End Function

    '==============================================================================
    '==============================================================================
    ' クラス内容を表示する
    Public Sub PrintClass()
        If CurveType = GeoCurveTypeEnum.geoLine Or CurveType = GeoCurveTypeEnum.geoArc Then
            Debug.Print("GeoCurve: Type:" & CurveType & "  S:(" & StartPoint.x & "," & StartPoint.y & "," & StartPoint.z & ")" & " E:(" & EndPoint.x & "," & EndPoint.y & "," & EndPoint.z & ")")
        End If
        If CurveType = GeoCurveTypeEnum.geoArc Or CurveType = GeoCurveTypeEnum.geoCircle Then
            Debug.Print("GeoCurve: Type:" & CurveType & "  C:(" & center.x & "," & center.y & "," & center.z & ")" & "  Radius: " & radius & "  N:(" & normal.x & "," & normal.y & "," & normal.z & ")")
        End If
    End Sub

    '==============================================================================
    ' 平面との交点を得る
    'Public Function GetIntersection(plane As GeoPlane, extendThis As Boolean) As GeoPointVector
    'End Function

    '==============================================================================
    ' 曲線との交点を得る
    Public Function GetIntersection(ByRef Curve As GeoCurve, ByRef extendThis As Boolean, ByRef extendArg As Boolean, ByRef dblTol As Double) As ArrayList
        GetIntersection = New ArrayList
        Dim dblT, dblS As Double
        Dim dblValue2, dblValue1, dblValue As Double
        Dim extsSizes(2) As Double
        Dim intIndex As Short
        Dim interPnt As GeoPoint
        Dim v5, v3, v1, v2, v4, v6 As GeoVector
        If CurveType = GeoCurveTypeEnum.geoLine Then

            v1 = GetStartTangent()
            v2 = Curve.GetStartTangent
            If v1.GetIsParallelTo(v2, 0.0001) = True Then Exit Function
            v3 = Curve.StartPoint.GetSubtracted(StartPoint)
            ' V4 = V1 x V2
            v4 = v1.GetOuterProduct(v2)
            If v4.GetInnerProduct(v4) = 0 Then Exit Function
            ' V5 = P1 - P2
            v5 = StartPoint.GetSubtracted(Curve.StartPoint)
            ' V6 = V2 x V1
            v6 = v2.GetOuterProduct(v1)
            If v6.GetInnerProduct(v6) = 0 Then Exit Function

            dblT = v3.GetOuterProduct(v2).GetInnerProduct(v4) / v4.GetInnerProduct(v4)
            dblS = v5.GetOuterProduct(v1).GetInnerProduct(v6) / v6.GetInnerProduct(v6)
            interPnt = StartPoint.GetAddedVec(v1.GetMultipled(dblT))
            If interPnt.IsEqualTo(Curve.StartPoint.GetAddedVec(v2.GetMultipled(dblS)), dblTol) = False Then Exit Function

            If extendThis = False Then
                AddPoint((StartPoint))
                AddPoint((EndPoint))
                extsSizes(0) = GetXSize()
                extsSizes(1) = GetXSize()
                extsSizes(2) = GetXSize()
                intIndex = GetMaximum(extsSizes)
                If intIndex = 0 Then
                    dblValue1 = StartPoint.x
                    dblValue2 = EndPoint.x
                    dblValue = interPnt.x
                ElseIf intIndex = 1 Then
                    dblValue1 = StartPoint.y
                    dblValue2 = EndPoint.y
                    dblValue = interPnt.y
                Else
                    dblValue1 = StartPoint.z
                    dblValue2 = EndPoint.z
                    dblValue = interPnt.z
                End If
                If dblValue1 > dblValue2 Then Call Util_SwapDouble(dblValue1, dblValue2)
                If (dblValue < dblValue1 Or dblValue2 < dblValue) And Math_IsEqual(dblValue, dblValue1, dblTol) = False And Math_IsEqual(dblValue, dblValue2, dblTol) = False Then Exit Function
            End If
            Call GetIntersection.Add(interPnt)
        End If
    End Function

    ' 曲線群との交点を得る
    'Public Function GetIntersection(curves As GeoCurveVector, extendThis As Boolean, _
    'extendArg As Boolean) As GeoPointVector
    'End Function
    '==============================================================================
    '==============================================================================
    Public Sub AddPoint(ByRef point As GeoPoint)
        'Dim GetIsValid As Object
        'If GetIsValid = False Then
        '    Call m_minPoint.Assign(point)
        '    Call m_maxPoint.Assign(point)
        'Else
        If m_minPoint.x > point.x Then m_minPoint.x = point.x
        If m_minPoint.y > point.y Then m_minPoint.y = point.y
        If m_minPoint.z > point.z Then m_minPoint.z = point.z
        If m_maxPoint.x < point.x Then m_maxPoint.x = point.x
        If m_maxPoint.y < point.y Then m_maxPoint.y = point.y
        If m_maxPoint.z < point.z Then m_maxPoint.z = point.z
        'End If
    End Sub
    Public Function GetXSize() As Double
        GetXSize = 0.0#
        If m_minPoint.x > m_maxPoint.x Then Exit Function
        GetXSize = m_maxPoint.x - m_minPoint.x
    End Function

    '==============================================================================
    Public Function GetYSize() As Double
        GetYSize = 0.0#
        If m_minPoint.y > m_maxPoint.y Then Exit Function
        GetYSize = m_maxPoint.y - m_minPoint.y
    End Function

    '==============================================================================
    Public Function GetZSize() As Double
        GetZSize = 0.0#
        If m_minPoint.z > m_maxPoint.z Then Exit Function
        GetZSize = m_maxPoint.z - m_minPoint.z
    End Function

    '==============================================================================
    ' 交差しているかどうかを調べる
    'Public Function HasIntersection(curves As GeoCurveVector) As Boolean
    'End Function

    '==============================================================================
    ' 変換マトリクスで座標変換する
    Public Sub Transform(ByRef matrix As GeoMatrix)
        If CurveType = GeoCurveTypeEnum.geoLine Or CurveType = GeoCurveTypeEnum.geoArc Or CurveType = GeoCurveTypeEnum.geoCircle Or CurveType = GeoCurveTypeEnum.geoEllipArc Then
            Call StartPoint.Transform(matrix)
            Call EndPoint.Transform(matrix)
        End If
        If CurveType = GeoCurveTypeEnum.geoEllipArc Or CurveType = GeoCurveTypeEnum.geoEllipse Then
            Call m_majorAxis.Transform(matrix)
            Call m_minorAxis.Transform(matrix)
        End If
        Dim v As GeoVector
        If CurveType = GeoCurveTypeEnum.geoArc Or CurveType = GeoCurveTypeEnum.geoCircle Or CurveType = GeoCurveTypeEnum.geoEllipArc Or CurveType = GeoCurveTypeEnum.geoEllipse Then
            Call center.Transform(matrix)
            ' 投影変換によって法線ベクトル長さが 0 になる場合は、変換を行わない 2009.4.7 Yoshimoto
            v = normal.GetTransformed(matrix)
            If v.GetLength > 0.0001 Then Call m_normal.Assign(v)
        End If
    End Sub
    ' 変換マトリクスで座標変換された新たなオブジェクトを得る
    Public Function GetTransformed(ByRef matrix As GeoMatrix) As GeoCurve
        GetTransformed = Me.Copy
    End Function

    '==============================================================================
    ' 指定点・軸を中心に回転する
    Public Sub RotateBy(ByRef center As GeoPoint, ByRef dblAngle As Double, ByRef axis As GeoVector)
        Dim mat1, mat2 As GeoMatrix
        mat1 = GeoMatrix_ByOriginNormal(center, axis)
        mat2 = GeoMatrix_ByZaxisRotation(dblAngle)
        mat1 = mat1.GetMultipled(mat2).GetMultipled(mat1.GetInverse)
    End Sub
    ' 指定点・軸を中心に回転された新たなオブジェクトを得る
    Public Function GetRotatedBy(ByRef center As GeoPoint, ByRef dblAngle As Double, ByRef axis As GeoVector) As GeoCurve
        GetRotatedBy = Me.Copy
    End Function

    '==============================================================================
    ' ベクトル方向に移動する
    Public Sub Move(ByRef vec As GeoVector)
        If CurveType = GeoCurveTypeEnum.geoLine Or CurveType = GeoCurveTypeEnum.geoArc Or CurveType = GeoCurveTypeEnum.geoCircle Then
            Call m_startPoint.Move(vec)
            Call m_endPoint.Move(vec)
        End If
        If CurveType = GeoCurveTypeEnum.geoArc Or CurveType = GeoCurveTypeEnum.geoCircle Or CurveType = GeoCurveTypeEnum.geoEllipArc Or CurveType = GeoCurveTypeEnum.geoEllipse Then
            Call m_center.Move(vec)
        End If
    End Sub
    ' ベクトル方向に移動された新たなオブジェクトを得る
    Public Function GetMoved(ByRef vec As GeoVector) As GeoCurve
        GetMoved = Me.Copy
    End Function

    '==============================================================================
    ' 点を基準に拡大/縮小する
    Public Sub ScaleBy(ByRef dblScaleX As Double, ByRef dblScaleY As Double, ByRef dblScaleZ As Double, ByRef point As GeoPoint)
        If CurveType = GeoCurveTypeEnum.geoLine Or CurveType = GeoCurveTypeEnum.geoArc Or CurveType = GeoCurveTypeEnum.geoCircle Or CurveType = GeoCurveTypeEnum.geoEllipArc Then
            Call m_startPoint.ScaleBy(dblScaleX, dblScaleY, dblScaleZ, point)
            Call m_endPoint.ScaleBy(dblScaleX, dblScaleY, dblScaleZ, point)
        End If
        If CurveType = GeoCurveTypeEnum.geoArc Or CurveType = GeoCurveTypeEnum.geoCircle Or CurveType = GeoCurveTypeEnum.geoEllipArc Or CurveType = GeoCurveTypeEnum.geoEllipse Then
            Call m_center.ScaleBy(dblScaleX, dblScaleY, dblScaleZ, point)
        End If
    End Sub
    ' 点を基準に拡大/縮小された新たなオブジェクトを得る
    Public Function GetScaledBy(ByRef dblScaleX As Double, ByRef dblScaleY As Double, ByRef dblScaleZ As Double, ByRef point As GeoPoint) As GeoCurve
        GetScaledBy = Me.Copy
    End Function

    '==============================================================================
    ' XY平面に投影する
    Public Sub ProjectToXYPlane()
        If CurveType = GeoCurveTypeEnum.geoLine Or CurveType = GeoCurveTypeEnum.geoArc Or CurveType = GeoCurveTypeEnum.geoCircle Then
            m_startPoint.z = 0
            m_endPoint.z = 0
        End If
        If CurveType = GeoCurveTypeEnum.geoArc Or CurveType = GeoCurveTypeEnum.geoCircle Then
            m_center.z = 0
        End If
    End Sub
    ' XY平面に投影された新たなオブジェクトを得る
    Public Function GetProjectedToXYPlane() As GeoCurve
        GetProjectedToXYPlane = Me.Copy
        GetProjectedToXYPlane.ProjectToXYPlane()
    End Function

    '==============================================================================
    ' 開始点での接線方向を得る
    Public Function GetStartTangent() As GeoVector
        If CurveType = GeoCurveTypeEnum.geoCircle Or CurveType = GeoCurveTypeEnum.geoArc Then
            GetStartTangent = center.GetSubtracted(StartPoint).GetNormal.GetRotatedBy(Math_Pai() / 2.0#, normal)
        End If
        GetStartTangent = EndPoint.GetSubtracted(StartPoint).GetNormal
    End Function
    '==============================================================================
    ' 点が曲線上にあるかどうか調べる
    Public Function GetIsPointOnCurve(ByVal point As GeoPoint, ByVal dblTol As Double) As Boolean
        GetIsPointOnCurve = False
        If CurveType = GeoCurveTypeEnum.geoLine Then
            '/////
            'Debug.Print point.GetDistanceToCurve(Me)
            GetIsPointOnCurve = (point.GetDistanceToCurve(Me) < dblTol)
            '点と線の距離がdblTolより小さい：GetIsPointOnCurve = True　；点は線分上に存在する
        Else
            '///// 線分以外は未実装
        End If
    End Function
    '==============================================================================
    ' 点との最近点を得る
    Public Function GetClosestPointTo(ByRef closest As GeoPoint, ByVal point As GeoPoint) As Integer 'H25.修正　Yamada
        'Public Function GetClosestPointTo(ByVal closest As GeoPoint, ByVal point As GeoPoint) As Integer'H25.修正前　Yamada
        GetClosestPointTo = 1
        If CurveType = GeoCurveTypeEnum.geoLine Then
            closest = GetPerpendicularFoot(point)
            If closest.GetSubtracted(Me.StartPoint).GetNormal.IsEqualTo(Me.GetStartTangent, 0.0001) Then
                If closest.GetDistanceTo(Me.StartPoint) > Me.GetLength Then closest = Me.EndPoint
            Else
                closest = Me.StartPoint
            End If
            GetClosestPointTo = 0
        ElseIf CurveType = GeoCurveTypeEnum.geoArc Then
            closest = Me.center.GetAddedVec(point.GetSubtracted(Me.center).GetNormal.GetMultipled(Me.radius))
            Dim dblAngle As Double
            dblAngle = Me.normal.GetPerpendicular.GetAngleTo(closest.GetSubtracted(Me.center), Me.normal)
            If dblAngle < 0 Then dblAngle = dblAngle + Math_Pai() * 2
            If Me.GetStartAngle < Me.GetEndAngle Then
                If dblAngle < Me.GetStartAngle Or Me.GetEndAngle < dblAngle Then
                    If point.GetDistanceTo(Me.StartPoint) < point.GetDistanceTo(Me.EndPoint) Then
                        closest = Me.StartPoint
                    Else
                        closest = Me.EndPoint
                    End If
                End If
            Else
                If Me.GetEndAngle < dblAngle And dblAngle < Me.GetStartAngle Then
                    If point.GetDistanceTo(Me.StartPoint) < point.GetDistanceTo(Me.EndPoint) Then
                        closest = Me.StartPoint
                    Else
                        closest = Me.EndPoint
                    End If
                End If
            End If
            GetClosestPointTo = 0
        ElseIf CurveType = GeoCurveTypeEnum.geoCircle Then
            closest = Me.center.GetAddedVec(point.GetSubtracted(Me.center).GetNormal.GetMultipled(Me.radius))
            GetClosestPointTo = 0
        End If

    End Function
    '==============================================================================
    Public Function GetPerpendicularFoot(ByVal basePoint As GeoPoint) As GeoPoint
        GetPerpendicularFoot = New GeoPoint
        If CurveType <> GeoCurveTypeEnum.geoLine Then Exit Function

        Dim interPnts As New ArrayList

        Dim dir1 As GeoVector, dir2 As GeoVector
        dir1 = Me.GetStartTangent
        dir2 = basePoint.GetSubtracted(Me.StartPoint).GetNormal
        If basePoint.IsEqualTo(Me.StartPoint, 0.0001) Then
            GetPerpendicularFoot = basePoint.Copy
            Exit Function
        End If
        If dir1.GetIsParallelTo(dir2, 0.0001) Then
            interPnts = New ArrayList
            Call interPnts.Add(GeoPlane_ByOriginNormal(basePoint, dir1).GetIntersectionWithCurve(Me, True))
        Else
            dir2 = dir1.GetOuterProduct(dir2)
            interPnts = GetIntersection(GeoCurve_LineByPointVector(basePoint, _
                                            dir1.GetRotatedBy(Math_Pai() / 2, dir2)), True, True, 0.0001)
        End If
        If interPnts.Count = 0 Then Exit Function

        GetPerpendicularFoot = interPnts.Item(0)

    End Function
    ' 終了点での接線方向を得る
    Public Function GetEndTangent() As GeoVector
        If CurveType = GeoCurveTypeEnum.geoCircle Or CurveType = GeoCurveTypeEnum.geoArc Then
            GetEndTangent = center.GetSubtracted(EndPoint).GetNormal.GetRotatedBy(-Math_Pai() / 2.0#, normal)
        End If
        GetEndTangent = GetStartTangent()
    End Function

    '==============================================================================
    ' 開始点から指定座標までの曲線上の距離を得る
    Public Function GetDistanceAtPoint(ByRef point As GeoPoint, ByRef dblTol As Double) As Double
        If CurveType = GeoCurveTypeEnum.geoLine Then
            If Me.GetIsPointOnCurve(point, dblTol) Then
                GetDistanceAtPoint = Me.StartPoint.GetDistanceTo(point)
            Else
                Dim foot As GeoPoint
                foot = Me.GetPerpendicularFoot(point)
                If Me.GetIsPointOnCurve(foot, dblTol) Then
                    GetDistanceAtPoint = Me.StartPoint.GetDistanceTo(foot)
                End If
            End If
        End If
    End Function

    ''==============================================================================
    '' 開始点から指定座標までの曲線上の距離を得る
    '' (開始点より前(終了点に対して反対側)の場合は、負の距離を返す)
    'Public Function GetSignedDistanceAtPoint(ByRef point As GeoPoint, ByRef dblTol As Double) As Double
    '    Dim geoLine As Object

    '    'UPGRADE_ISSUE: GeoPoint object was not upgraded. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"'
    '    Dim foot As GeoPoint
    '    If CurveType = geoLine Then
    '        'UPGRADE_WARNING: Untranslated statement in GetSignedDistanceAtPoint. Please check source code.
    '    End If

    'End Function

    '==============================================================================
    ' 開始点から指定した曲線上の距離だけ離れた点を得る
    Public Function GetPointAtDistance(ByRef dist As Double) As GeoPoint
        If CurveType = GeoCurveTypeEnum.geoArc Or CurveType = GeoCurveTypeEnum.geoCircle Then
            If radius = 0.0# Then
                GetPointAtDistance = StartPoint.Copy
                Exit Function
            End If
            GetPointAtDistance = center.GetAddedVec(StartPoint.GetSubtracted(center).GetRotatedBy(dist / radius, normal))
            Exit Function
        End If
        GetPointAtDistance = StartPoint.GetAddedVec(GetStartTangent.GetMultipled(dist))
    End Function

    '==============================================================================
    ' 重なっているかどうか調べる
    Public Function GetHasOverlap(ByRef Curve As GeoCurve, ByRef dblTol As Double) As Boolean
        GetHasOverlap = False
        '///// 線分以外は未実装
        If Curve.CurveType = GeoCurveTypeEnum.geoCircle Or Curve.CurveType = GeoCurveTypeEnum.geoArc Then Exit Function
        If GetStartTangent.GetIsParallelTo(Curve.GetStartTangent, dblTol) = False Then Exit Function

        ' 端点が線上になければダメ
        If Curve.GetIsPointOnCurve(StartPoint, dblTol) = False And _
           Curve.GetIsPointOnCurve(EndPoint, dblTol) = False And _
           GetIsPointOnCurve(Curve.StartPoint, dblTol) = False And _
           GetIsPointOnCurve(Curve.EndPoint, dblTol) = False Then Exit Function

        ' 端点が等しく、向きが反対だったらダメ
        Dim dblInnerProduct As Double
        dblInnerProduct = GetStartTangent.GetInnerProduct(Curve.GetStartTangent)
        If StartPoint.IsEqualTo(Curve.StartPoint, dblTol) And dblInnerProduct < 0 Then Exit Function
        If StartPoint.IsEqualTo(Curve.EndPoint, dblTol) And dblInnerProduct > 0 Then Exit Function
        If EndPoint.IsEqualTo(Curve.StartPoint, dblTol) And dblInnerProduct > 0 Then Exit Function
        If EndPoint.IsEqualTo(Curve.EndPoint, dblTol) And dblInnerProduct < 0 Then Exit Function
        GetHasOverlap = True
    End Function

    '==============================================================================
    ' 指定した曲線群で分割された曲線群を得る(交差・重なりが分割の対象)
    'Public Function getDivided(curves As GeoCurveVector) As GeoCurveVector
    'End Function

    '==============================================================================
    '==============================================================================
    ' 方向を反転する
    Public Sub Reverse()
        If CurveType = GeoCurveTypeEnum.geoCircle Then
            m_normal.Negate()
        ElseIf CurveType = GeoCurveTypeEnum.geoArc Then
            Call Util_SwapObject(m_startPoint, m_endPoint)
            m_normal.Negate()
        ElseIf CurveType = GeoCurveTypeEnum.geoLine Then
            Call Util_SwapObject(m_startPoint, m_endPoint)
        End If
    End Sub

    '==============================================================================
    ' 方向を反転された新たなオブジェクトを得る
    Public Function GetReversed() As GeoCurve
        GetReversed = Me.Copy
        GetReversed.Reverse()
    End Function

    '==============================================================================
    Public Function GetIsCollinear(ByRef Curve As GeoCurve, ByRef dblTol As Double) As Boolean
        GetIsCollinear = False
        If CurveType <> GeoCurveTypeEnum.geoLine Or Curve.CurveType <> GeoCurveTypeEnum.geoLine Then Exit Function
        If GetStartTangent.GetIsParallelTo(Curve.GetStartTangent, dblTol) = False Then Exit Function
        If StartPoint.IsEqualTo(Curve.StartPoint, dblTol) = False Then
            If GetStartTangent.GetIsParallelTo(StartPoint.GetSubtracted(Curve.StartPoint), dblTol) = False Then Exit Function
        ElseIf StartPoint.IsEqualTo(Curve.EndPoint, dblTol) = False Then
            If GetStartTangent.GetIsParallelTo(StartPoint.GetSubtracted(Curve.EndPoint), dblTol) = False Then Exit Function
        ElseIf EndPoint.IsEqualTo(Curve.StartPoint, dblTol) = False Then
            If GetStartTangent.GetIsParallelTo(EndPoint.GetSubtracted(Curve.StartPoint), dblTol) = False Then Exit Function
        ElseIf EndPoint.IsEqualTo(Curve.EndPoint, dblTol) = False Then
            If GetStartTangent.GetIsParallelTo(EndPoint.GetSubtracted(Curve.EndPoint), dblTol) = False Then Exit Function
        End If
        GetIsCollinear = True
    End Function

    '==============================================================================
    Public Function GetMiddlePoint() As GeoPoint
        GetMiddlePoint = Me.StartPoint.GetAdded(Me.EndPoint).GetDivided(2)
    End Function

    '==============================================================================
    '==============================================================================
    ' 指定長さだけ延長/短縮する
    Public Sub Extend(ByRef dblLength As Double, ByRef blnStartEnd As Boolean)
        If blnStartEnd Then
            Me.StartPoint = Me.GetPointAtDistance(-dblLength)
        Else
            Me.EndPoint = Me.GetPointAtDistance(Me.GetLength + dblLength)
        End If
    End Sub
    '==============================================================================
    ' 指定長さだけ延長/短縮したオブジェクトを得る
    Public Function GetExtended(ByRef dblLength As Double, ByRef blnStartEnd As Boolean) As GeoCurve
        GetExtended = Me.Copy
        Call GetExtended.Extend(dblLength, blnStartEnd)
    End Function

    '==============================================================================
    '==============================================================================
    ' 両端を指定長さだけ延長/短縮する
    Public Sub BothExtend(ByRef dblLength As Double)
        Me.SP = Me.GetPointAtDistance(-dblLength)
        Me.EP = Me.GetPointAtDistance(Me.GetLength + dblLength)
    End Sub
    '==============================================================================
    ' 両端を指定長さだけ延長/短縮したオブジェクトを得る
    Public Function GetBothExtended(ByRef dblLength As Double) As GeoCurve
        GetBothExtended = Me.Copy
        Call GetBothExtended.BothExtend(dblLength)
    End Function

    '==============================================================================
    '==============================================================================
    Public Function GetDistanceToClosestPointTo(ByRef point As GeoPoint) As Double
        Dim closest As GeoPoint = Nothing
        If Me.GetClosestPointTo(closest, point) <> 0 Then Exit Function
        GetDistanceToClosestPointTo = closest.GetDistanceTo(point)
    End Function

    '==============================================================================
    '   省略形メソッド群
    '==============================================================================
    ' 開始点での接線方向を得る
    Public Function GetDir() As GeoVector
        GetDir = Me.GetStartTangent
    End Function
    ' 開始点での接線方向の反対方向を得る
    Public Function GetRevDir() As GeoVector
        GetRevDir = Me.GetDir.GetNegative
    End Function
    ' 開始点での接線方向を指定倍したベクトルを得る
    Public Function GetMulDir(ByRef dblValue As Double) As GeoVector
        GetMulDir = Me.GetDir.GetMultipled(dblValue)
    End Function
    Public Function MP() As GeoPoint
        MP = Me.GetMiddlePoint
    End Function
    Public Function dir_Renamed() As GeoVector
        dir_Renamed = Me.GetStartTangent
    End Function
    '==============================================================================
    ' XZ平面に投影する
    Public Sub ProjectToXZPlane()
        If CurveType = GeoCurveTypeEnum.geoLine Or CurveType = GeoCurveTypeEnum.geoArc Or CurveType = GeoCurveTypeEnum.geoCircle Then
            m_startPoint.y = 0
            m_endPoint.y = 0
        End If
        If CurveType = GeoCurveTypeEnum.geoArc Or CurveType = GeoCurveTypeEnum.geoCircle Then
            m_center.y = 0
        End If
    End Sub
    ' XZ平面に投影された新たなオブジェクトを得る
    Public Function GetProjectedToXZPlane() As GeoCurve
        GetProjectedToXZPlane = Me.Copy
        GetProjectedToXZPlane.ProjectToXZPlane()
    End Function
    Public Sub ProjectToVectorPlane(ByRef vec1 As GeoVector, ByRef vec2 As GeoVector, ByRef Vec3 As GeoVector)
        If CurveType = GeoCurveTypeEnum.geoLine Or CurveType = GeoCurveTypeEnum.geoArc Or CurveType = GeoCurveTypeEnum.geoCircle Then
            Call Me.StartPoint.GetVectorPlaneTo(vec1, vec2, Vec3)
            Call Me.EndPoint.GetVectorPlaneTo(vec1, vec2, Vec3)
        End If
        If CurveType = GeoCurveTypeEnum.geoArc Or CurveType = GeoCurveTypeEnum.geoCircle Then
            Call Me.center.GetVectorPlaneTo(vec1, vec2, Vec3)
        End If
    End Sub

    Public Function GetProjectToVectorPlane(ByRef vec1 As GeoVector, ByRef vec2 As GeoVector, ByRef Vec3 As GeoVector) As GeoCurve
        GetProjectToVectorPlane = Me.Copy
        Call GetProjectToVectorPlane.ProjectToVectorPlane(vec1, vec2, Vec3)
    End Function

    '==============================================================================
    ' YZ平面に投影する
    Public Sub ProjectToYZPlane()
        If CurveType = GeoCurveTypeEnum.geoLine Or CurveType = GeoCurveTypeEnum.geoArc Or CurveType = GeoCurveTypeEnum.geoCircle Then
            m_startPoint.x = 0
            m_endPoint.x = 0
        End If
        If CurveType = GeoCurveTypeEnum.geoArc Or CurveType = GeoCurveTypeEnum.geoCircle Then
            m_center.x = 0
        End If
    End Sub
    ' YZ平面に投影された新たなオブジェクトを得る
    Public Function GetProjectedToYZPlane() As GeoCurve
        GetProjectedToYZPlane = Me.Copy
        GetProjectedToYZPlane.ProjectToYZPlane()
    End Function
End Class