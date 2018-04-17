Option Strict Off
Option Explicit On
Module GeoModule
    '==============================================================================
    '
    '   GeoCurveTypeEnum
    '
    '==============================================================================
    'Public Enum GeoCurveTypeEnum
    '    geoLine = 3     ' 線分
    '    geoCircle = 4   ' 円
    '    geoArc = 5      ' 円弧
    '    geoEllipse = 6  ' 楕円
    '    geoEllipArc = 7 ' 楕円弧
    'End Enum

    Public Function GetMaximum(ByVal dblSu() As Double) As Short
        If dblSu(0) > dblSu(1) And dblSu(0) > dblSu(2) Then
            Return 0
        End If
        If dblSu(1) > dblSu(0) And dblSu(1) > dblSu(2) Then
            Return 1
        End If
        If dblSu(2) > dblSu(1) And dblSu(2) > dblSu(0) Then
            Return 2
        End If

    End Function
    '==============================================================================
    Public Sub Util_SwapObject(ByRef objValue1 As Object, ByRef objValue2 As Object)
        Dim objValue As Object
        objValue = objValue1
        objValue1 = objValue2
        objValue2 = objValue
    End Sub

    '==============================================================================
    Public Sub Util_SwapDouble(ByRef dblValue1 As Double, ByRef dblValue2 As Double)
        Dim dblValue As Double
        dblValue = dblValue1
        dblValue1 = dblValue2
        dblValue2 = dblValue
    End Sub
    ' X軸を返す
    Public Function GeoVector_Xaxis() As GeoVector
        GeoVector_Xaxis = New GeoVector
        Call GeoVector_Xaxis.setXYZ(1, 0, 0)
    End Function
    ' Y軸を返す
    Public Function GeoVector_Yaxis() As GeoVector
        GeoVector_Yaxis = New GeoVector
        Call GeoVector_Yaxis.setXYZ(0, 1, 0)
    End Function
    ' Z軸を返す
    Public Function GeoVector_Zaxis() As GeoVector
        GeoVector_Zaxis = New GeoVector
        Call GeoVector_Zaxis.setXYZ(0, 0, 1)
    End Function


    ' 平面原点+法線より変換マトリクスを得る
    Public Function GeoMatrix_ByOriginNormal(ByRef origin As GeoPoint, ByRef normal As GeoVector) As GeoMatrix
        GeoMatrix_ByOriginNormal = New GeoMatrix
        Dim xdir, ydir As GeoVector
        xdir = normal.GetPerpendicular
        'Set ydir = xdir.GetRotatedBy(Math_Pai / 2, normal)
        ydir = normal.GetOuterProduct(xdir)
        Call GeoMatrix_ByOriginNormal.SetCoordSystem(origin, xdir, ydir, normal)
    End Function
    ' 平面法線より変換マトリクスを得る
    Public Function GeoMatrix_ByNormal(ByRef normal As GeoVector) As GeoMatrix
        GeoMatrix_ByNormal = New GeoMatrix
        Dim xdir, ydir As GeoVector
        xdir = normal.GetPerpendicular
        'Set ydir = xdir.GetRotatedBy(Math_Pai / 2, normal)
        ydir = normal.GetOuterProduct(xdir)
        Dim origin As GeoPoint
        origin = New GeoPoint
        Call GeoMatrix_ByNormal.SetCoordSystem(origin, xdir, ydir, normal)
    End Function
    ' X 軸回りの回転マトリクスを得る
    Public Function GeoMatrix_ByXaxisRotation(ByRef dblAngle As Double) As GeoMatrix
        GeoMatrix_ByXaxisRotation = New GeoMatrix
        Call GeoMatrix_ByXaxisRotation.SetAt(2, 2, System.Math.Cos(dblAngle))
        Call GeoMatrix_ByXaxisRotation.SetAt(2, 3, System.Math.Sin(dblAngle))
        Call GeoMatrix_ByXaxisRotation.SetAt(3, 2, -System.Math.Sin(dblAngle))
        Call GeoMatrix_ByXaxisRotation.SetAt(3, 3, System.Math.Cos(dblAngle))
    End Function
    ' Y 軸回りの回転マトリクスを得る
    Public Function GeoMatrix_ByYaxisRotation(ByRef dblAngle As Double) As GeoMatrix
        GeoMatrix_ByYaxisRotation = New GeoMatrix
        Call GeoMatrix_ByYaxisRotation.SetAt(1, 1, System.Math.Cos(dblAngle))
        Call GeoMatrix_ByYaxisRotation.SetAt(1, 3, -System.Math.Sin(dblAngle))
        Call GeoMatrix_ByYaxisRotation.SetAt(3, 1, System.Math.Sin(dblAngle))
        Call GeoMatrix_ByYaxisRotation.SetAt(3, 3, System.Math.Cos(dblAngle))
    End Function
    ' Z 軸回りの回転マトリクスを得る
    Public Function GeoMatrix_ByZaxisRotation(ByRef dblAngle As Double) As GeoMatrix
        GeoMatrix_ByZaxisRotation = New GeoMatrix
        Call GeoMatrix_ByZaxisRotation.SetAt(1, 1, System.Math.Cos(dblAngle))
        Call GeoMatrix_ByZaxisRotation.SetAt(1, 2, System.Math.Sin(dblAngle))
        Call GeoMatrix_ByZaxisRotation.SetAt(2, 1, -System.Math.Sin(dblAngle))
        Call GeoMatrix_ByZaxisRotation.SetAt(2, 2, System.Math.Cos(dblAngle))
    End Function

    ' 法線より平面を得る
    Public Function GeoPlane_ByNormal(ByRef normal As GeoVector) As GeoPlane
        GeoPlane_ByNormal = New GeoPlane
        GeoPlane_ByNormal.normal = normal
    End Function
    '==============================================================================
    Public Function GeoCurve_LineByPointVector(ByVal basePoint As GeoPoint, ByVal dir As GeoVector) As GeoCurve
        GeoCurve_LineByPointVector = New GeoCurve
        Call GeoCurve_LineByPointVector.SetLine(basePoint, basePoint.GetAddedVec(dir))
    End Function
    ' XYZ 座標から点を得る
    Public Function GeoPoint_ByXYZ(ByVal X As Double, ByVal Y As Double, ByVal Z As Double) As GeoPoint
        GeoPoint_ByXYZ = New GeoPoint
        Call GeoPoint_ByXYZ.SetXYZ(X, Y, Z)
    End Function
    '==============================================================================
    ' 原点と法線より平面を得る
    Public Function GeoPlane_ByOriginNormal(ByRef origin As GeoPoint, ByRef normal As GeoVector) As GeoPlane
        GeoPlane_ByOriginNormal = New GeoPlane
        GeoPlane_ByOriginNormal.origin = origin
        GeoPlane_ByOriginNormal.normal = normal
    End Function
    ' XYZ 座標からベクトルを得る
    Public Function GeoVector_ByXYZ(ByVal X As Double, ByVal Y As Double, ByVal Z As Double) As GeoVector
        GeoVector_ByXYZ = New GeoVector
        Call GeoVector_ByXYZ.SetXYZ(X, Y, Z)
    End Function

    '==============================================================================
    ' XY平面を得る
    Public Function GeoPlane_XYPlane() As GeoPlane
        GeoPlane_XYPlane = New GeoPlane
        GeoPlane_XYPlane.normal = GeoVector_Zaxis
    End Function

    '==============================================================================
    ' YZ平面を得る
    Public Function GeoPlane_YZPlane() As GeoPlane
        GeoPlane_YZPlane = New GeoPlane
        GeoPlane_YZPlane.normal = GeoVector_Xaxis
    End Function

    '==============================================================================
    ' ZX平面を得る
    Public Function GeoPlane_ZXPlane() As GeoPlane
        GeoPlane_ZXPlane = New GeoPlane
        GeoPlane_ZXPlane.normal = GeoVector_Yaxis
    End Function

    ' 3点より平面を得る
    Public Function GeoPlane_By3Points(ByVal P1 As GeoPoint, ByVal P2 As GeoPoint, ByVal p3 As GeoPoint) As GeoPlane
        Dim normalDir As GeoVector
        normalDir = p3.GetSubtracted(P1).GetOuterProduct(P2.GetSubtracted(P1)).GetNormal
        GeoPlane_By3Points = GeoPlane_ByOriginNormal(P1, normalDir)
    End Function

End Module
