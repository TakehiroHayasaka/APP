Option Strict Off
Option Explicit On
Public Class GeoPlane

    '==============================================================================
    Private m_origin As GeoPoint
    Private m_normal As GeoVector

    '==============================================================================
    Public Sub Class_Initialize_Renamed()
        m_origin = New GeoPoint
        m_normal = GeoVector_Zaxis()
    End Sub
    Public Sub New()
        MyBase.New()
        Class_Initialize_Renamed()
    End Sub

    '==============================================================================
    Public Sub Class_Terminate_Renamed()
        On Error Resume Next
        m_origin = Nothing
        m_normal = Nothing
    End Sub
    Protected Overrides Sub Finalize()
        Class_Terminate_Renamed()
        MyBase.Finalize()
    End Sub

    '==============================================================================
    '==============================================================================
    Public Sub Assign(ByRef plane As GeoPlane)
        Call m_origin.Assign((plane.origin))
        Call m_normal.Assign((plane.normal))
    End Sub
    Public Function Copy() As GeoPlane
        Copy = New GeoPlane
        Call Copy.Assign(Me)
    End Function

    '==============================================================================
    '==============================================================================
    Public Sub SetByOriginNormal(ByRef point As GeoPoint, ByRef normal As GeoVector)
        Call m_origin.Assign(point)
        Call m_normal.Assign(normal)
    End Sub

    '==============================================================================
    '==============================================================================
    Public Property origin() As GeoPoint
        Get
            origin = m_origin
        End Get
        Set(ByVal Value As GeoPoint)
            Call m_origin.Assign(Value)
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
    '==============================================================================
    ' 有効かどうかを調べる
    Public Function IsValid() As Boolean
        If Me.normal.GetLength <> 0 Then
            IsValid = True
        Else
            IsValid = False
        End If
    End Function

    '==============================================================================
    '==============================================================================
    ' 変換マトリクスで座標変換する
    Public Sub Transform(ByRef mat As GeoMatrix)
        Call m_origin.Transform(mat)
        Call m_normal.Transform(mat)
    End Sub
    ' 変換マトリクスで座標変換された新たなオブジェクトを得る
    Public Function GetTransformed(ByRef mat As GeoMatrix) As GeoPlane
        GetTransformed = Me.Copy
        Call GetTransformed.Transform(mat)
    End Function

    '==============================================================================
    ' ベクトル方向に移動する
    Public Sub Move(ByRef vec As GeoVector)
        Call m_origin.Move(vec)
    End Sub
    ' ベクトル方向に移動された新たなオブジェクトを得る
    Public Function GetMoved(ByRef vec As GeoVector) As GeoPlane
        GetMoved = Me.Copy
        Call GetMoved.Move(vec)
    End Function
    ' 曲線との交点を得る
    Public Function GetIntersectionWithCurve(ByVal Curve As GeoCurve, ByVal blnExtendCurve As Boolean) As GeoPoint
        GetIntersectionWithCurve = Nothing
        ' 今のところ線分以外は未対応
        If Curve.CurveType <> GeoCurveTypeEnum.geoLine Then Exit Function

        ' 1. 線分 curve の始点と垂直方向からなる平面 plane を得る
        Dim plane As GeoPlane
        plane = GeoPlane_ByOriginNormal(Curve.StartPoint, Curve.GetStartTangent.GetPerpendicular)

        ' 2. 2つの平面 Me, plane の交線 interLine を得る
        Dim interLine As GeoCurve = Nothing
        If Me.GetIntersectionWithPlane(interLine, plane) = False Then Exit Function 'H25.4.23以前　interLineはNothingのまま出力

        ' 3. 交線 interLine と curve の交点を得る
        Dim interPnts As New ArrayList

        interPnts = interLine.GetIntersection(Curve, True, blnExtendCurve, 0.0001)

        'curve.PrintClass
        'interLine.PrintClass
        'Debug.Print blnExtendCurve, interPnts.Size

        If interPnts.Count = 0 Then Exit Function

        GetIntersectionWithCurve = interPnts.Item(0)

    End Function

    '    '==============================================================================
    '    Public Function GetIntersectionWithCurves( _
    '        ByVal argCurves As GeoCurveArray, _
    '        ByVal blnExtendThis As Boolean, _
    '        ByVal blnExtendArg As Boolean _
    '    ) As GeoPointArray
    '#If 1 Then ' 2008.1.15 Yoshimoto
    '        GetIntersectionWithCurves = New GeoPointArray
    '        Dim i As Integer
    '        For i = 0 To argCurves.Size - 1
    '            Call GetIntersectionWithCurves.AppendArray( _
    '                Me.GetIntersectionWithCurve(argCurves.at(i), blnExtendThis, blnExtendArg, 0.0001))
    '        Next
    '#Else
    '    Set GetIntersectionWithCurves = GeoPointArray_ByGeoPoints( _
    '        GeoCurves_GetIntersectionWithCurves( _
    '            GeoCurves_ByGeoCurveArray(Me), GeoCurves_ByGeoCurveArray(argCurves), blnExtendThis, blnExtendArg))
    '#End If
    '    End Function
    '==============================================================================
    '==============================================================================
    ' 面分との交線を得る
    'Public Function getIntersection(menbun As GeoMenbun) As GeoCurveVector
    'End Function

    '' 曲線との交点を得る
    'Public Function GetIntersectionWithCurve(Curve As GeoCurve, blnExtendCurve As Boolean) As GeoPoint
    '
    '    ' 今のところ線分以外は未対応
    '    If Curve.CurveType <> geoLine Then Exit Function
    '
    '    ' 1. 線分 curve の始点と垂直方向からなる平面 plane を得る
    '    Dim plane As GeoPlane
    '    Set plane = GeoPlane_ByOriginNormal(Curve.StartPoint, Curve.GetStartTangent.GetPerpendicular)
    '
    '    ' 2. 2つの平面 Me, plane の交線 interLine を得る
    '    Dim interLine As GeoCurve
    '    If Me.GetIntersectionWithPlane(interLine, plane) = False Then Exit Function
    '
    '    ' 3. 交線 interLine と curve の交点を得る
    '    Dim interPnts As ObjectArray
    '    Set interPnts = interLine.GetIntersection(Curve, True, blnExtendCurve, 0.0001)
    '
    '    'curve.PrintClass
    '    'interLine.PrintClass
    '    'Debug.Print blnExtendCurve, interPnts.Size
    '
    '    If interPnts.Size = 0 Then Exit Function
    '
    '    Set GetIntersectionWithCurve = interPnts.at(0)
    '
    'End Function
    '
    ' 平面との交線を得る
    Public Function GetIntersectionWithPlane(ByRef Curve As GeoCurve, ByVal plane As GeoPlane) As Boolean 'H25.4.25　修正（Yamada）
        '修正内容｛ByVal Curve As GeoCurve → ByRef Curve As GeoCurve｝
        'Public Function GetIntersectionWithPlane(ByVal Curve As GeoCurve, ByVal plane As GeoPlane) As Boolean'H25.4.25　修正前（Yamada）


        Dim xpoint As GeoPoint
        Dim xdir As GeoVector
        xdir = Me.normal.GetOuterProduct(plane.normal) '二つの平面の法線ベクトルの外積

        Dim dir2 As GeoVector    ' holds the squares of the coordinates of xdir
        dir2 = GeoVector_ByXYZ(xdir.x * xdir.x, xdir.y * xdir.y, xdir.z * xdir.z)

        Dim dblEpsilon As Double
        dblEpsilon = 0.0000001
        Dim dblW1 As Double, dblW2 As Double
        dblW1 = -Me.normal.GetInnerProduct(Me.origin.ToGeoVector)
        dblW2 = -plane.normal.GetInnerProduct(plane.origin.ToGeoVector)

        Dim dblInvdet As Double  ' inverse of 2x2 matrix determinant
        If dir2.z > dir2.y And dir2.z > dir2.x And dir2.z > dblEpsilon Then     ' then get a point on the XY plane
            dblInvdet = 1.0# / xdir.z
            ' solve 1) Me.Normal.x * xpoint.x + Me.Normal.y * xpoint.y = - dblW1
            '       2) plane.Normal.x * xpoint.x + plane.Normal.y * xpoint.y = - dblW2
            xpoint = GeoPoint_ByXYZ(Me.normal.y * dblW2 - plane.normal.y * dblW1, _
                                    plane.normal.x * dblW1 - Me.normal.x * dblW2, 0.0#)
        ElseIf dir2.y > dir2.x And dir2.y > dblEpsilon Then                     ' then get a point on the XZ plane
            dblInvdet = -1.0# / xdir.y
            ' solve 1) Me.Normal.x * xpoint.x + Me.Normal.z * xpoint.z = - dblW1
            '       2) plane.Normal.x * xpoint.x + plane.Normal.z * xpoint.z = - dblW2
            xpoint = GeoPoint_ByXYZ(Me.normal.z * dblW2 - plane.normal.z * dblW1, 0.0#, _
                                        plane.normal.x * dblW1 - Me.normal.x * dblW2)
        ElseIf dir2.x > dblEpsilon Then                                         ' then get a point on the YZ plane
            dblInvdet = 1.0# / xdir.x
            ' solve 1) Me.Normal.y * xpoint.y + Me.Normal.z * xpoint.z = - dblW1
            '       2) plane.Normal.y * xpoint.y + plane.Normal.z * xpoint.z = - dblW2
            xpoint = GeoPoint_ByXYZ(0.0#, Me.normal.z * dblW2 - plane.normal.z * dblW1, _
                                        plane.normal.y * dblW1 - Me.normal.y * dblW2)
        Else
            ' xdir is zero, no point of intersection exists
            Exit Function
        End If

        Call xpoint.Multiple(dblInvdet)

        dblInvdet = 1.0# / Math.Sqrt(dir2.x + dir2.y + dir2.z)
        Call xdir.Multiple(dblInvdet)

        Curve = GeoCurve_LineByPointVector(xpoint, xdir)

        GetIntersectionWithPlane = True

    End Function

    '==============================================================================
    '==============================================================================
    Public Function GetIsCoplanar(ByRef plane As GeoPlane, ByRef dblTol As Double) As Boolean

        ' 1. 法線が平行でなければ共面ではない
        If Me.normal.GetIsParallelTo((plane.normal), dblTol) = False Then Exit Function

        ' 2. 原点が異なる場合、原点間のベクトルと法線の垂直方向ベクトルの外積が、法線と平行でなければ共面ではない
        Dim v1, v2 As GeoVector
        If Me.origin.IsEqualTo((plane.origin), dblTol) = False Then
            v1 = Me.origin.GetSubtracted((plane.origin)).GetNormal
            v2 = Me.normal.GetPerpendicular
            If v1.GetIsParallelTo(v2, dblTol) = False Then
                If v1.GetOuterProduct(v2).GetIsParallelTo((Me.normal), dblTol) = False Then Exit Function
            End If
        End If

        GetIsCoplanar = True

    End Function

    ''==============================================================================
    ''==============================================================================
    'Public Function GetClosestPointTo(point As GeoPoint) As GeoPoint
    '    Set GetClosestPointTo = Me.GetIntersectionWithCurve(GeoCurve_LineByPointVector(point, Me.normal), True)
    'End Function

    '==============================================================================
    '==============================================================================
    ' クラス内容を表示する
    Public Sub PrintClass()
        Debug.Print("GeoPlane: O:(" & origin.x & "," & origin.y & "," & origin.z & ") N:(" & normal.x & "," & normal.y & "," & normal.z & ")")
    End Sub
End Class
