'Option Strict Off
Option Explicit On
Public Class GeoPoint

    '==============================================================================
    Private m_x As Double
    Private m_y As Double
    Private m_z As Double

    '==============================================================================
    '==============================================================================
    'UPGRADE_NOTE: Class_Initialize was upgraded to Class_Initialize_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Public Sub Class_Initialize_Renamed()
        On Error Resume Next
        m_x = 0.0#
        m_y = 0.0#
        m_z = 0.0#
    End Sub
    Public Sub New()
        MyBase.New()
        Class_Initialize_Renamed()
    End Sub

    '==============================================================================
    'UPGRADE_NOTE: Class_Terminate was upgraded to Class_Terminate_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Public Sub Class_Terminate_Renamed()
        On Error Resume Next
    End Sub
    Protected Overrides Sub Finalize()
        Class_Terminate_Renamed()
        MyBase.Finalize()
    End Sub

    '==============================================================================
    '==============================================================================

    '==============================================================================
    Public Property x() As Double
        Get
            x = m_x
        End Get
        Set(ByVal Value As Double)
            m_x = Value
        End Set
    End Property
    Public Property y() As Double
        Get
            y = m_y
        End Get
        Set(ByVal Value As Double)
            m_y = Value
        End Set
    End Property
    Public Property z() As Double
        Get
            z = m_z
        End Get
        Set(ByVal Value As Double)
            m_z = Value
        End Set
    End Property

    '==============================================================================
    Public Sub SetXY(ByRef x As Double, ByRef y As Double)
        m_x = x
        m_y = y
    End Sub
    Public Sub setXYZ(ByRef x As Double, ByRef y As Double, ByRef z As Double)
        m_x = x
        m_y = y
        m_z = z
    End Sub
    '==============================================================================
    Public Sub SetIcadPoint(ByVal icadPoint As IntelliCAD.Point)
        m_x = icadPoint.x
        m_y = icadPoint.Y
        m_z = icadPoint.Z
    End Sub

    Public Sub SetFBM_3DPoint(ByVal fbm3Dpoint As FBMlib.Point3D)
        m_x = fbm3Dpoint.X.D
        m_y = fbm3Dpoint.Y.D
        m_z = fbm3Dpoint.Z.D
    End Sub

    'H25.4.22 GeoPoint → Point3Dに　（Yamada）

    Public Function GetP3D() As FBMlib.Point3D
        GetP3D = New FBMlib.Point3D
        GetP3D.X = m_x
        GetP3D.Y = m_y
        GetP3D.Z = m_z
    End Function


    '==============================================================================
    Public Sub Assign(ByRef point As GeoPoint)
        m_x = point.x()
        m_y = point.y()
        m_z = point.z()
    End Sub
    Public Function Copy() As GeoPoint
        Copy = New GeoPoint
        Call Copy.Assign(Me)
    End Function

    '==============================================================================
    ' クラス内容を表示する
    Public Sub PrintClass()
        Debug.Print("GeoPoint:(" & x & "," & y & "," & z & ")")
    End Sub

    '==============================================================================
    ' 点を加算する

    Public Sub Add(ByRef point As GeoPoint)
        m_x = m_x + point.x()
        m_y = m_y + point.y()
        m_z = m_z + point.z()
    End Sub
    ' 点を加算したオブジェクトを返す
    Public Function GetAdded(ByRef point As GeoPoint) As GeoPoint
        Dim value As GeoPoint
        value = Me.Copy
        Call value.Add(point)
        GetAdded = value
    End Function
    '==============================================================================
    ' ベクトルを加算する

    Public Sub AddVec(ByRef vec As GeoVector)
        m_x = m_x + vec.x()
        m_y = m_y + vec.y()
        m_z = m_z + vec.z()
    End Sub
    ' ベクトルを加算したオブジェクトを返す
    Public Function GetAddedVec(ByRef vec As GeoVector) As GeoPoint
        Dim value As GeoPoint
        value = Me.Copy
        Call value.AddVec(vec)
        GetAddedVec = value
    End Function

    '==============================================================================
    ' 点を減算する

    Public Sub Subtract(ByRef point As GeoPoint)
        m_x = m_x - point.x()
        m_y = m_y - point.y()
        m_z = m_z - point.z()
    End Sub
    ' 点を減算したオブジェクトを返す
    Public Function GetSubtracted(ByRef point As GeoPoint) As GeoVector
        Dim value As GeoVector
        value = Me.ToGeoVector
        Call value.SubtractPoint(point)
        GetSubtracted = value
    End Function
    '==============================================================================
    ' ベクトルを減算する

    Public Sub SubtractVec(ByRef vec As GeoVector)
        m_x = m_x - vec.x()
        m_y = m_y - vec.y()
        m_z = m_z - vec.z()
    End Sub
    ' ベクトルを減算したオブジェクトを返す
    Public Function GetSubtractedVec(ByRef vec As GeoVector) As GeoPoint
        Dim value As GeoPoint
        value = Me.Copy
        Call value.SubtractVec(vec)
        GetSubtractedVec = value
    End Function

    '==============================================================================
    ' 符号を反対にする
    Public Sub Negate()
        m_x = -m_x
        m_y = -m_y
        m_z = -m_z
    End Sub
    ' 符号を反対にしたオブジェクトを返す
    Public Function GetNegative() As GeoPoint
        Dim value As GeoPoint
        value = Me.Copy
        Call value.Negate()
        GetNegative = value
    End Function

    '==============================================================================
    ' 指定した値で乗算する

    Public Sub Multiple(ByRef dblValue As Double)
        m_x = m_x * dblValue
        m_y = m_y * dblValue
        m_z = m_z * dblValue
    End Sub
    ' 指定した値で乗算したオブジェクトを返す
    Public Function GetMultipled(ByRef dblValue As Double) As GeoPoint
        Dim v As GeoPoint
        v = Me.Copy
        Call v.Multiple(dblValue)
        GetMultipled = v
    End Function

    '==============================================================================
    ' 指定した値で除算する

    Public Sub Divide(ByRef dblValue As Double)
        If dblValue = 0.0# Then
            Exit Sub
        End If
        m_x = m_x / dblValue
        m_y = m_y / dblValue
        m_z = m_z / dblValue
    End Sub
    ' 指定した値で除算したオブジェクトを返す
    Public Function GetDivided(ByRef dblValue As Double) As GeoPoint
        Dim v As GeoPoint
        v = Me.Copy
        Call v.Divide(dblValue)
        GetDivided = v
    End Function

    '==============================================================================
    ' 許容誤差内で等価かどうかを返す
    Public Function IsEqualTo(ByRef point As GeoPoint, ByRef dblTol As Double) As Boolean
        IsEqualTo = System.Math.Abs(x - point.x) < dblTol And System.Math.Abs(y - point.y) < dblTol And System.Math.Abs(z - point.z) < dblTol
    End Function

    '==============================================================================
    ' 変換マトリクスで座標変換する
    Public Sub Transform(ByRef matrix As GeoMatrix)
        Dim tpoint As GeoPoint
        tpoint = New GeoPoint
        tpoint.x = matrix.GetAt(1, 1) * Me.x + matrix.GetAt(2, 1) * Me.y + matrix.GetAt(3, 1) * Me.z + matrix.GetAt(4, 1)
        tpoint.y = matrix.GetAt(1, 2) * Me.x + matrix.GetAt(2, 2) * Me.y + matrix.GetAt(3, 2) * Me.z + matrix.GetAt(4, 2)
        tpoint.z = matrix.GetAt(1, 3) * Me.x + matrix.GetAt(2, 3) * Me.y + matrix.GetAt(3, 3) * Me.z + matrix.GetAt(4, 3)
        Call Me.Assign(tpoint)
    End Sub
    ' 変換マトリクスで座標変換された新たなオブジェクトを得る
    Public Function GetTransformed(ByRef matrix As GeoMatrix) As GeoPoint
        GetTransformed = Me.Copy
        Call GetTransformed.Transform(matrix)
    End Function

    '==============================================================================
    ' 指定点・軸を中心に回転する
    Public Sub RotateBy(ByRef center As GeoPoint, ByRef dblAngle As Double, ByRef axis As GeoVector)
        Dim mat1, mat2 As GeoMatrix
        mat1 = GeoMatrix_ByOriginNormal(center, axis)
        mat2 = GeoMatrix_ByZaxisRotation(dblAngle)
        mat1 = mat1.GetMultipled(mat2).GetMultipled(mat1.GetInverse)
        Call Me.Transform(mat1)
    End Sub
    ' 指定点・軸を中心に回転された新たなオブジェクトを得る
    Public Function GetRotatedBy(ByRef center As GeoPoint, ByRef dblAngle As Double, ByRef axis As GeoVector) As GeoPoint
        GetRotatedBy = Me.Copy
        Call GetRotatedBy.RotateBy(center, dblAngle, axis)
    End Function

    '==============================================================================
    ' ベクトル方向に移動する

    Public Sub Move(ByRef vec As GeoVector)
        Me.x = Me.x + vec.x
        Me.y = Me.y + vec.y
        Me.z = Me.z + vec.z
    End Sub
    ' ベクトル方向に移動された新たなオブジェクトを得る
    Public Function GetMoved(ByRef vec As GeoVector) As GeoPoint
        GetMoved = Me.Copy
        Call GetMoved.Move(vec)
    End Function

    '==============================================================================
    ' 点を基準に拡大/縮小する

    Public Sub ScaleBy(ByRef dblScaleX As Double, ByRef dblScaleY As Double, ByRef dblScaleZ As Double, ByRef point As GeoPoint)

        Dim v As GeoVector
        v = Me.GetSubtracted(point)
        v.x = v.x * dblScaleX
        v.y = v.y * dblScaleY
        v.z = v.z * dblScaleZ
        Call Me.Assign(point.GetAddedVec(v))

    End Sub
    ' 点を基準に拡大/縮小された新たなオブジェクトを得る
    Public Function GetScaledBy(ByRef dblScaleX As Double, ByRef dblScaleY As Double, ByRef dblScaleZ As Double, ByRef point As GeoPoint) As GeoPoint
        GetScaledBy = Me.Copy
        Call GetScaledBy.ScaleBy(dblScaleX, dblScaleY, dblScaleZ, point)
    End Function

    '==============================================================================
    ' 点までの距離を得る
    Public Function GetDistanceTo(ByRef point As GeoPoint) As Double
        GetDistanceTo = GetSubtracted(point).GetLength
    End Function
    '==============================================================================
    ' 曲線までの距離を得る
    Public Function GetDistanceToCurve(ByVal Curve As GeoCurve) As Double
        GetDistanceToCurve = 0.0#
        If Curve Is Nothing Then Exit Function

        Dim closest As GeoPoint = Nothing
        If Curve.GetClosestPointTo(closest, Me) <> 0 Then Exit Function
        'Curveとの最近点（closest）が見つかった場合

        GetDistanceToCurve = GetDistanceTo(closest)
    End Function

    '==============================================================================
    ' 曲線までの距離を得る
    'Public Function GetDistanceToCurve(Curve As GeoCurve) As Double
    '
    '    If Curve Is Nothing Then Exit Function
    '
    '    Dim closest As GeoPoint
    '    If Curve.GetClosestPointTo(closest, Me) <> 0 Then Exit Function
    '
    '    GetDistanceToCurve = GetDistanceTo(closest)
    '
    'End Function
    '
    ''==============================================================================
    '' 曲線までの垂線の足への距離を得る
    'Public Function GetPerpendicularDistanceToCurve(Curve As GeoCurve) As Double
    '    If Curve Is Nothing Then Exit Function
    '    GetPerpendicularDistanceToCurve = Me.GetDistanceTo(Curve.GetPerpendicularFoot(Me))
    'End Function
    '
    ''==============================================================================
    '' 平面までの距離を得る
    'Public Function GetDistanceToPlane(plane As GeoPlane) As Double
    '    GetDistanceToPlane = GetDistanceTo(GetProjected(plane, plane.normal()))
    'End Function
    '
    ''==============================================================================
    '' 平面に対してベクトル方向に投影する
    'Public Sub Project(plane As GeoPlane, dir As GeoVector)
    '#If 1 Then
    '    ' 平面に投影できない場合のため、平面との交点の有効/無効を判定する 2007.12.19 Yoshimoto
    '    Dim p As GeoPoint
    '    Set p = plane.GetIntersectionWithCurve(GeoCurve_LineByPointVector(Me, dir), True)
    '    If p Is Nothing Then Exit Sub
    '    Call Me.Assign(p)
    '#Else
    '    Call Me.Assign(plane.GetIntersectionWithCurve(GeoCurve_LineByPointVector(Me, dir), True))
    '#End If
    'End Sub
    '
    ''==============================================================================
    '' 平面に対してベクトル方向に投影された新たなオブジェクトを得る
    'Public Function GetProjected(plane As GeoPlane, dir As GeoVector) As GeoPoint
    '    Set GetProjected = plane.GetIntersectionWithCurve(GeoCurve_LineByPointVector(Me, dir), True)
    'End Function
    '
    '==============================================================================
    ' XY平面に投影する
    Public Sub ProjectToXYPlane()
        Me.z = 0
    End Sub
    '==============================================================================
    ' XY平面に投影された新たなオブジェクトを得る
    Public Function GetProjectedToXYPlane() As GeoPoint
        GetProjectedToXYPlane = Me.Copy
        GetProjectedToXYPlane.ProjectToXYPlane()
    End Function

    ''==============================================================================
    '' 平面との最近点を得る
    'Public Function GetClosestPointToPlane(plane As GeoPlane) As GeoPoint
    '    Set GetClosestPointToPlane = GetProjected(plane, plane.normal)
    'End Function

    '==============================================================================
    '==============================================================================
    ' 指定した座標値で、点を比較する

    ' (例: strAt1="X",strAt2="Y",strAt3="Z" の場合、( 1,2,3 ) < ( 1,3,2 )、

    '      strAt1="Z",strAt2="",strAt3="" の場合、( 1,2,3 ) > ( 1,3,2 )、となる)
    ' (戻り値は、-1: Me > point、0: Me = point、1: Me < point)
    Public Function Compare(ByRef point As GeoPoint, ByRef strAt1 As String, ByRef strAt2 As String, ByRef strAt3 As String, ByRef dblEps As Double) As Short
        Compare = False
        Select Case strAt1
            Case "X"
                If Math_IsEqual((Me.x), (point.x), dblEps) = False Then Compare = IIf(Me.x > point.x, -1, 1)
            Case "Y"
                If Math_IsEqual((Me.y), (point.y), dblEps) = False Then Compare = IIf(Me.y > point.y, -1, 1)
            Case "Z"
                If Math_IsEqual((Me.z), (point.z), dblEps) = False Then Compare = IIf(Me.z > point.z, -1, 1)
        End Select
        If Compare <> 0 Then Exit Function

        Select Case strAt2
            Case "X"
                If Math_IsEqual((Me.x), (point.x), dblEps) = False Then Compare = IIf(Me.x > point.x, -1, 1)
            Case "Y"
                If Math_IsEqual((Me.y), (point.y), dblEps) = False Then Compare = IIf(Me.y > point.y, -1, 1)
            Case "Z"
                If Math_IsEqual((Me.z), (point.z), dblEps) = False Then Compare = IIf(Me.z > point.z, -1, 1)
        End Select
        If Compare <> 0 Then Exit Function

        Select Case strAt3
            Case "X"
                If Math_IsEqual((Me.x), (point.x), dblEps) = False Then Compare = IIf(Me.x > point.x, -1, 1)
            Case "Y"
                If Math_IsEqual((Me.y), (point.y), dblEps) = False Then Compare = IIf(Me.y > point.y, -1, 1)
            Case "Z"
                If Math_IsEqual((Me.z), (point.z), dblEps) = False Then Compare = IIf(Me.z > point.z, -1, 1)
        End Select
        If Compare <> 0 Then Exit Function

    End Function

    '==============================================================================
    '==============================================================================
    ' GeoVector に変換する
    Public Function ToGeoVector() As GeoVector
        ToGeoVector = New GeoVector
        Call ToGeoVector.setXYZ(m_x, m_y, m_z)
    End Function

    '==============================================================================
    '   省略形メソッド群
    '==============================================================================
    'UPGRADE_WARNING: ParamArray params was changed from ByRef to ByVal. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="93C6A0DC-8C99-429A-8696-35FC4DCEFCCC"'
    Public Sub pls(ByVal ParamArray params() As Object)
        On Error GoTo ErrorLabel
        If UBound(params) = -1 Then Exit Sub
        Dim i As Short
        Dim p As GeoPoint
        Dim v As GeoVector
        For i = 0 To UBound(params)
            'UPGRADE_WARNING: TypeName has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            Select Case TypeName(params(i))
                Case "GeoPoint"
                    p = params(i)
                    Me.Add(p)
                Case "GeoVector"
                    v = params(i)
                    Me.AddVec(v)
            End Select
        Next
        Exit Sub
ErrorLabel:
        'Debug.Print(vb6.0.TabLayout("GeoPoint.Pls", Err.Description))
    End Sub
    'UPGRADE_WARNING: ParamArray params was changed from ByRef to ByVal. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="93C6A0DC-8C99-429A-8696-35FC4DCEFCCC"'
    Public Function GetPls(ByVal ParamArray params() As Object) As GeoPoint
        On Error GoTo ErrorLabel
        GetPls = Me.Copy
        If UBound(params) = -1 Then Exit Function
        Dim i As Short
        Dim p As GeoPoint
        Dim v As GeoVector
        For i = 0 To UBound(params)
            'UPGRADE_WARNING: TypeName has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            Select Case TypeName(params(i))
                Case "GeoPoint"
                    p = params(i)
                    GetPls.Add(p)
                Case "GeoVector"
                    v = params(i)
                    GetPls.AddVec(v)
            End Select
        Next
        Exit Function
ErrorLabel:
        'Debug.Print(VB6.TabLayout("GeoPoint.GetPls", Err.Description))
    End Function
    '==============================================================================
    'UPGRADE_WARNING: ParamArray params was changed from ByRef to ByVal. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="93C6A0DC-8C99-429A-8696-35FC4DCEFCCC"'
    Public Sub Mns(ByVal ParamArray params() As Object)
        On Error GoTo ErrorLabel
        If UBound(params) = -1 Then Exit Sub
        Dim i As Short
        Dim p As GeoPoint
        Dim v As GeoVector
        For i = 0 To UBound(params)
            'UPGRADE_WARNING: TypeName has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            Select Case TypeName(params(i))
                Case "GeoPoint"
                    p = params(i)
                    Me.Subtract(p)
                Case "GeoVector"
                    v = params(i)
                    Me.SubtractVec(v)
            End Select
        Next
        Exit Sub
ErrorLabel:
        'Debug.Print(VB6.TabLayout("GeoPoint.Mns", Err.Description))
    End Sub
    'UPGRADE_WARNING: ParamArray params was changed from ByRef to ByVal. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="93C6A0DC-8C99-429A-8696-35FC4DCEFCCC"'
    Public Function GetMns(ByVal ParamArray params() As Object) As Object
        On Error GoTo ErrorLabel
        Dim p As GeoPoint
        If UBound(params) = 0 Then
            'UPGRADE_WARNING: TypeName has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            If TypeName(params(0)) = "GeoPoint" Then
                p = params(0)
                GetMns = Me.GetSubtracted(p)
                Exit Function
            End If
        End If
        GetMns = Me.Copy
        If UBound(params) = -1 Then Exit Function
        Dim i As Short
        Dim v As GeoVector
        For i = 0 To UBound(params)
            'UPGRADE_WARNING: TypeName has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            Select Case TypeName(params(i))
                Case "GeoPoint"
                    p = params(i)
                    'UPGRADE_WARNING: Couldn't resolve default property of object GetMns.Subtract. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    GetMns.Subtract(p)
                Case "GeoVector"
                    v = params(i)
                    'UPGRADE_WARNING: Couldn't resolve default property of object GetMns.SubtractVec. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    GetMns.SubtractVec(v)
            End Select
        Next
        Exit Function
ErrorLabel:
        'Debug.Print(VB6.TabLayout("GeoPoint.GetMns", Err.Description))
    End Function

    Public Sub GetVectorPlaneTo(ByRef vec1 As GeoVector, ByRef vec2 As GeoVector, ByRef Vec3 As GeoVector)
        Dim icdPointVector As New GeoVector
        Call icdPointVector.setXYZ((Me.x), (Me.y), (Me.z))
        Me.x = icdPointVector.GetInnerProduct(vec1) / vec1.GetLength
        Me.y = icdPointVector.GetInnerProduct(vec2) / vec2.GetLength
        Me.z = icdPointVector.GetInnerProduct(Vec3) / Vec3.GetLength
    End Sub
End Class