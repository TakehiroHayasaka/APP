Option Strict Off
Option Explicit On
Public Class GeoVector


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
    'H25.4.22　2つのGeoPointからVectorを作成
    Public Sub New(ByVal P1 As GeoPoint, ByVal P2 As GeoPoint)
        m_x = P2.x - P1.x
        m_y = P2.y - P1.y
        m_z = P2.z - P1.z
    End Sub
    Public Sub New(ByVal X As Double, ByVal Y As Double, ByVal Z As Double)
        m_x = X
        m_y = Y
        m_z = Z
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
    Public Sub Assign(ByRef vec As GeoVector)
        m_x = vec.x()
        m_y = vec.y()
        m_z = vec.z()
    End Sub
    Public Function Copy() As GeoVector
        Copy = New GeoVector
        Call Copy.Assign(Me)
    End Function

    '==============================================================================
    ' クラス内容を表示する
    Public Sub PrintClass()
        Debug.Print("GeoVector:(" & x & "," & y & "," & z & ")")
    End Sub

    '==============================================================================
    ' 点を加算する
    Public Sub AddPoint(ByRef point As GeoVector)
        m_x = m_x + point.x()
        m_y = m_y + point.y()
        m_z = m_z + point.z()
    End Sub
    ' 点を加算したオブジェクトを返す
    Public Function GetAddedPoint(ByRef point As GeoVector) As GeoVector
        Dim value As GeoVector
        value = Me.Copy
        Call value.AddPoint(point)
        GetAddedPoint = value
    End Function
    '==============================================================================
    ' ベクトルを加算する
    Public Sub Add(ByRef vec As GeoVector)
        m_x = m_x + vec.x()
        m_y = m_y + vec.y()
        m_z = m_z + vec.z()
    End Sub
    ' ベクトルを加算したオブジェクトを返す
    Public Function GetAdded(ByRef vec As GeoVector) As GeoVector
        Dim value As GeoVector
        value = Me.Copy
        Call value.Add(vec)
        GetAdded = value
    End Function

    '==============================================================================
    ' 点を減算する
    Public Sub SubtractPoint(ByRef point As GeoPoint)
        m_x = m_x - point.x()
        m_y = m_y - point.y()
        m_z = m_z - point.z()
    End Sub
    ' 点を減算したオブジェクトを返す
    Public Function GetSubtractedPoint(ByRef point As GeoPoint) As GeoVector
        Dim value As GeoVector
        value = Me.Copy
        Call value.SubtractPoint(point)
        GetSubtractedPoint = value
    End Function
    '==============================================================================
    ' ベクトルを減算する
    Public Sub Subtract(ByRef vec As GeoVector)
        m_x = m_x - vec.x()
        m_y = m_y - vec.y()
        m_z = m_z - vec.z()
    End Sub
    ' ベクトルを減算したオブジェクトを返す
    Public Function GetSubtracted(ByRef vec As GeoVector) As GeoVector
        Dim value As GeoVector
        value = Me.Copy
        Call value.Subtract(vec)
        GetSubtracted = value
    End Function

    '==============================================================================
    ' 符号を反対にする
    Public Sub Negate()
        m_x = -m_x
        m_y = -m_y
        m_z = -m_z
    End Sub
    ' 符号を反対にしたオブジェクトを返す
    Public Function GetNegative() As GeoVector
        Dim value As GeoVector
        value = Me.Copy
        Call value.Negate()
        GetNegative = value
    End Function

    '==============================================================================
    ' 指定した値で乗算する
    Public Sub Multiple(ByVal dblValue As Double)
        m_x = m_x * dblValue
        m_y = m_y * dblValue
        m_z = m_z * dblValue
    End Sub
    ' 指定した値で乗算したオブジェクトを返す
    Public Function GetMultipled(ByVal dblValue As Double) As GeoVector
        Dim v As GeoVector
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
    Public Function GetDivided(ByRef dblValue As Double) As GeoVector
        Dim v As GeoVector
        v = Me.Copy
        Call v.Divide(dblValue)
        GetDivided = v
    End Function

    '==============================================================================
    ' 長さを得る
    Public Function GetLength() As Double
        GetLength = System.Math.Sqrt(x() * x() + y() * y() + z() * z())
    End Function

    '==============================================================================
    ' 許容誤差内で等価かどうかを返す
    Public Function IsEqualTo(ByRef vec As GeoVector, ByRef dblTol As Double) As Boolean
        IsEqualTo = System.Math.Abs(x - vec.x) < dblTol And System.Math.Abs(y - vec.y) < dblTol And System.Math.Abs(z - vec.z) < dblTol
    End Function

    '==============================================================================
    ' 許容誤差内で平行かどうかを返す
    Public Function IsParallelTo(ByRef vec As GeoVector, ByRef dblTol As Double) As Boolean
        IsParallelTo = (Me.IsEqualTo(vec, dblTol) Or Me.GetNegative.IsEqualTo(vec, dblTol))
    End Function

    '==============================================================================
    ' 指定ベクトルと、許容誤差範囲内で平行かどうかを調べる
    Public Function GetIsParallelTo(ByRef vec As GeoVector, ByRef dblTol As Double) As Boolean
        GetIsParallelTo = True
        If Me.GetNormal.IsEqualTo(vec.GetNormal, dblTol) = True Then Exit Function
        If Me.GetNormal.IsEqualTo(vec.GetNegative.GetNormal, dblTol) = True Then Exit Function
        GetIsParallelTo = False
    End Function

    '==============================================================================
    ' 変換マトリクスで座標変換する
    Public Sub Transform(ByRef matrix As GeoMatrix)
        Dim tpoint1, tpoint2 As GeoPoint
        tpoint1 = New GeoPoint
        tpoint2 = Me.ToGeoPoint
        Call tpoint1.Transform(matrix)
        Call tpoint2.Transform(matrix)
        Call Me.Assign(tpoint2.GetSubtracted(tpoint1))
    End Sub
    ' 変換マトリクスで座標変換された新たなオブジェクトを得る
    Public Function GetTransformed(ByRef matrix As GeoMatrix) As GeoVector
        GetTransformed = Me.Copy
        Call GetTransformed.Transform(matrix)
    End Function
    '
    '==============================================================================
    ' 指定軸を中心に回転する
    Public Sub RotateBy(ByRef dblAngle As Double, ByRef axis As GeoVector)
        If Me.GetIsParallelTo(axis, 0.0001) Then Exit Sub
        Dim dblAxisAngle As Double
        dblAxisAngle = axis.GetSmallAngleTo(Me)
        If Math_IsEqual(dblAxisAngle, 0.0#, 0.000000001) Or Math_IsEqual(dblAxisAngle, Math_Pai, 0.000000001) Then Exit Sub
        Dim dirY, dirX, dirZ As GeoVector
        dirY = axis
        dirZ = dirY.GetOuterProduct(Me).GetNormal
        dirX = dirZ.GetOuterProduct(dirY).GetNormal
        dblAxisAngle = dirY.GetAngleTo(Me, dirZ)
        Dim dirYProjected As GeoVector
        dirYProjected = dirY.GetMultipled(System.Math.Cos(dblAxisAngle) * Me.GetLength)
        Call dirX.Multiple(System.Math.Cos(dblAngle) * Me.GetLength)
        Call dirZ.Multiple(System.Math.Sin(dblAngle) * Me.GetLength)
        Call Me.Assign(dirX.GetAdded(dirZ).GetAdded(dirYProjected))
    End Sub
    ' 指定軸を中心に回転された新たなオブジェクトを得る
    Public Function GetRotatedBy(ByRef dblAngle As Double, ByRef axis As GeoVector) As GeoVector
        GetRotatedBy = Me.Copy
        Call GetRotatedBy.RotateBy(dblAngle, axis)
    End Function
    '
    ''==============================================================================
    'Public Sub RotateBy90DegZ()
    '    Call Me.RotateBy(Math_Pai / 2, GeoVector_Zaxis)
    'End Sub
    'Public Function GetRotatedBy90DegZ() As GeoVector
    '    Set GetRotatedBy90DegZ = Me.Copy
    '    Call GetRotatedBy90DegZ.RotateBy(Math_Pai / 2, GeoVector_Zaxis)
    'End Function

    '==============================================================================
    ' 内積を得る
    Public Function GetInnerProduct(ByRef vec As GeoVector) As Double
        GetInnerProduct = x() * vec.x() + y() * vec.y() + z() * vec.z()
    End Function
    ' 外積を得る
    Public Function GetOuterProduct(ByRef vec As GeoVector) As GeoVector
        GetOuterProduct = New GeoVector
        Call GetOuterProduct.setXYZ(y * vec.z - z * vec.y, z * vec.x - x * vec.z, x * vec.y - y * vec.x)
    End Function

    '==============================================================================
    ' ベクトルとなす角度(0-π)を返す
    Public Function GetSmallAngleTo(ByRef vec As GeoVector) As Double
        Dim dblDenom As Double
        dblDenom = GetLength() * vec.GetLength
        If dblDenom = 0.0# Then
            GetSmallAngleTo = 0.0#
        Else
            GetSmallAngleTo = Math_Acos(Me.GetInnerProduct(vec) / dblDenom)
        End If
    End Function
    ' refVec 側の、ベクトルとなす角度(-π-π)を返す
    ' ( me が refVec 平面の 12 時方向とすると、vec が 9 時方向のとき + 、3 時方向のとき - となる)
    Public Function GetAngleTo(ByRef vec As GeoVector, ByRef refVec As GeoVector) As Double
        GetAngleTo = GetSmallAngleTo(vec)
        If Me.GetOuterProduct(vec).GetInnerProduct(refVec) < 0.0# Then
            GetAngleTo = -GetAngleTo
        End If
    End Function

    '==============================================================================
    ' 垂直方向を得る
    Public Function GetPerpendicular() As GeoVector
        GetPerpendicular = New GeoVector
        Dim dblEpsilon As Double
        dblEpsilon = 1.0# / 64.0#
        If System.Math.Abs(x()) < dblEpsilon And System.Math.Abs(y()) < dblEpsilon Then
            Call GetPerpendicular.setXYZ(z(), 0, -x())
            Call GetPerpendicular.Normalize()
            Call GetPerpendicular.Multiple(Me.GetLength)
            Exit Function
        End If
        Call GetPerpendicular.setXYZ(-y(), x(), 0)
        Call GetPerpendicular.Normalize()
        Call GetPerpendicular.Multiple(Me.GetLength)
    End Function

    '==============================================================================
    ' 単位ベクトルにする
    Public Sub Normalize()
        Dim dblLength As Double
        dblLength = GetLength()
        If dblLength = 0.0# Then Exit Sub
        m_x = m_x / dblLength
        m_y = m_y / dblLength
        m_z = m_z / dblLength
    End Sub
    ' 単位ベクトルを得る
    Public Function GetNormal() As GeoVector
        Dim v As GeoVector
        v = Me.Copy
        Call v.Normalize()
        GetNormal = v
    End Function

    '==============================================================================
    ' 平面に投影された新たなオブジェクトを得る
    Public Function GetProjected(ByRef plane As GeoPlane) As GeoVector
        GetProjected = New GeoVector
        Dim dblAngle As Double
        dblAngle = plane.normal.GetSmallAngleTo(Me)
        If Math_IsEqual(dblAngle, 0.0#, 0.000000001) Or Math_IsEqual(dblAngle, Math_Pai, 0.000000001) Then
            Exit Function
        End If
        Dim dirY, dirX, dirZ As GeoVector
        dirY = plane.normal
        dirZ = dirY.GetOuterProduct(Me).GetNormal
        dirX = dirZ.GetOuterProduct(dirY).GetNormal
        dblAngle = dirY.GetAngleTo(Me, dirZ)
        Call GetProjected.Assign(dirX.GetMultipled(System.Math.Sin(dblAngle) * Me.GetLength))
        'Dim mat As GeoMatrix
        'Set mat = GeoMatrix_ByPlane(plane)
        'Call Me.Transform(mat)
        'Me.z = 0
        'Call Me.Transform(mat.GetInverse)
    End Function

    '==============================================================================
    ' XY平面に投影する
    Public Sub ProjectToXYPlane()
        Me.z = 0
    End Sub
    '==============================================================================
    ' XY平面に投影された新たなオブジェクトを得る
    Public Function GetProjectedToXYPlane() As GeoVector
        GetProjectedToXYPlane = Me.Copy
        GetProjectedToXYPlane.ProjectToXYPlane()
    End Function

    '==============================================================================
    ''==============================================================================
    '' X,Y,Z のうち、最小値のインデクス(0:X,1:Y,2:Z)を返す
    'Public Function GetMinimumIndex() As Integer
    '    GetMinimumIndex = DoubleArray_ByParamArray(Me.X, Me.Y, Me.Z).GetMinimum
    'End Function
    ''==============================================================================
    '' X,Y,Z のうち、絶対値での最小値のインデクス(0:X,1:Y,2:Z)を返す
    'Public Function GetAbsoluteMinimumIndex() As Integer
    '    GetAbsoluteMinimumIndex = DoubleArray_ByParamArray(Abs(Me.X), Abs(Me.Y), Abs(Me.Z)).GetMinimum
    'End Function
    '==============================================================================
    '' X,Y,Z のうち、最大値のインデクス(0:X,1:Y,2:Z)を返す
    'Public Function GetMaximumIndex() As Integer
    '    GetMaximumIndex = DoubleArray_ByParamArray(Me.X, Me.Y, Me.Z).GetMaximum
    'End Function
    '==============================================================================
    '' X,Y,Z のうち、絶対値での最大値のインデクス(0:X,1:Y,2:Z)を返す
    'Public Function GetAbsoluteMaximumIndex() As Integer
    '    GetAbsoluteMaximumIndex = DoubleArray_ByParamArray(Abs(Me.X), Abs(Me.Y), Abs(Me.Z)).GetMaximum
    'End Function

    '==============================================================================
    '==============================================================================
    ' GeoPoint に変換する
    Public Function ToGeoPoint() As GeoPoint
        ToGeoPoint = New GeoPoint
        Call ToGeoPoint.setXYZ(m_x, m_y, m_z)
    End Function

    '==============================================================================
    '   省略形メソッド群
    '==============================================================================
    Public Sub Mul(ByVal dblValue As Double)
        Me.Multiple(dblValue)
    End Sub
    Public Function GetMul(ByVal dblValue As Double) As GeoVector
        GetMul = Me.GetMultipled(dblValue)
    End Function
End Class