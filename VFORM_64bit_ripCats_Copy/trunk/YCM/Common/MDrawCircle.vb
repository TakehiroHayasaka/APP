'Option Strict Off
Option Explicit On
Module MDrawCircle
    '===============================================================
    ' 機　能：3点确定一个圆　－＞＞3点から1つの円を作成

    ' 引　数：point1,point2,point3(3点)
    ' 返  回：圆心：Opoint　－＞＞円の中心
    '         半径：dblR　－＞＞半径
    '        方向向量：VVector　－＞＞法線ベクトル
    Public Function CcDrawCircle(ByVal point1 As GeoPoint, ByVal point2 As GeoPoint, ByVal point3 As GeoPoint, ByRef Opoint As GeoPoint, ByRef dblR As Double, ByRef VVector As GeoVector) As Integer
        'Opoint = New GeoPoint
        Try
            Opoint = CcDraworg(point1, point2, point3)
            CcDrawCircle = 1
            dblR = point1.GetDistanceTo(Opoint)
            Dim vec1 As New GeoVector
            Dim vec2 As New GeoVector
            'Dim uvechousen As New GeoVector
            'Dim vecdir As New GeoVector
            'Dim pntkijyun As New GeoVector
            vec1.setXYZ(point2.x - point1.x, point2.y - point1.y, point2.z - point1.z)
            vec2.setXYZ(point2.x - point3.x, point2.y - point3.y, point2.z - point3.z)
            vec1.Normalize()
            vec2.Normalize()
            VVector = vec1.GetOuterProduct(vec2)
            'uvechousen.Normalize()
            'vecdir = vec1.GetOuterProduct(uvechousen)
            ''VVector.y = VVector.y
            'pntkijyun.x = (point2.x + point1.x) / 2
            'pntkijyun.y = (point2.y + point1.y) / 2
            'pntkijyun.z = (point2.z + point1.z) / 2
        Catch
            MessageBox.Show("選択した点が重複になっているので、再選択してください。")
            CcDrawCircle = -1
            Exit Function
        End Try
    End Function
    '===============================================================
    ' 機　能：3点确定3条线 
    ' 引　数：point1,point2,point3(3点)
    ' 返  回：线分：line1，line2，line3
    Public Function CcDrawGeoCurve(ByVal point1 As GeoPoint, ByVal point2 As GeoPoint, ByVal point3 As GeoPoint, ByVal line1 As GeoCurve, ByVal line2 As GeoCurve, ByVal line3 As GeoCurve) As Integer
        line1.StartPoint.setXYZ(point2.x, point2.y, point2.z)
        line1.EndPoint.setXYZ(point3.x, point3.y, point3.z)
        line2.StartPoint.setXYZ(point3.x, point3.y, point3.z)
        line2.EndPoint.setXYZ(point1.x, point1.y, point1.z)
        line3.StartPoint.setXYZ(point1.x, point1.y, point1.z)
        line3.EndPoint.setXYZ(point2.x, point2.y, point2.z)
        CcDrawGeoCurve = 0
    End Function
    ' 機　能：3点确定3条线 
    ' 引　数：point1,point2,point3(3点)
    ' 返  回：圆心：org
    Public Function CcDraworg(ByVal point1 As GeoPoint, ByVal point2 As GeoPoint, ByVal point3 As GeoPoint) As GeoPoint
        Dim line1 As New GeoCurve
        Dim line2 As New GeoCurve
        Dim line3 As New GeoCurve
        Dim pointfoottoline1 As New GeoPoint
        Dim pointfoottoline2 As New GeoPoint
        Dim pointfoottoline3 As New GeoPoint
        Dim vectorfoot1topoint1 As New GeoVector
        Dim vectorfoot2topoint2 As New GeoVector
        Dim vectorfoot3topoint3 As New GeoVector
        Dim perpendcularstart1 As New GeoPoint
        Dim perpendcularstart2 As New GeoPoint
        Dim perpendcularstart3 As New GeoPoint
        Dim arrintersection As New ArrayList
        'Dim perpendcularend1 As New GeoPoint
        'Dim perpendcularend2 As New GeoPoint
        'Dim perpendcularend3 As New GeoPoint
        Dim linetp1 As New GeoCurve
        Dim linetp2 As New GeoCurve
        perpendcularstart1.setXYZ((point2.x + point3.x) / 2, (point2.y + point3.y) / 2, (point2.z + point3.z) / 2)
        perpendcularstart2.setXYZ((point1.x + point3.x) / 2, (point1.y + point3.y) / 2, (point1.z + point3.z) / 2)
        perpendcularstart3.setXYZ((point2.x + point1.x) / 2, (point2.y + point1.y) / 2, (point2.z + point1.z) / 2)
        CcDrawGeoCurve(point1, point2, point3, line1, line2, line3)
        pointfoottoline1 = line1.GetPerpendicularFoot(point1)
        vectorfoot1topoint1.setXYZ(point1.x - pointfoottoline1.x, point1.y - pointfoottoline1.y, point1.z - pointfoottoline1.z)
        pointfoottoline2 = line2.GetPerpendicularFoot(point2)
        vectorfoot2topoint2.setXYZ(point2.x - pointfoottoline2.x, point2.y - pointfoottoline2.y, point2.z - pointfoottoline2.z)
        pointfoottoline3 = line3.GetPerpendicularFoot(point3)
        vectorfoot3topoint3.setXYZ(point3.x - pointfoottoline3.x, point3.y - pointfoottoline3.y, point3.z - pointfoottoline3.z)
        'perpendcularend1.setXYZ(perpendcularstart1.x + vectorfoot1topoint1.x, perpendcularstart1.y + vectorfoot1topoint1.y, perpendcularstart1.z + vectorfoot1topoint1.z)
        'perpendcularend2.setXYZ(perpendcularstart2.x + vectorfoot2topoint2.x, perpendcularstart2.y + vectorfoot2topoint2.y, perpendcularstart2.z + vectorfoot2topoint2.z)
        'perpendcularend3.setXYZ(perpendcularstart3.x + vectorfoot3topoint3.x, perpendcularstart3.y + vectorfoot3topoint3.y, perpendcularstart3.z + vectorfoot3topoint3.z)

        linetp1.SetLineByPointVec(perpendcularstart1, vectorfoot1topoint1)
        linetp2.SetLineByPointVec(perpendcularstart2, vectorfoot2topoint2)
        'linetp1.StartPoint.setXYZ(perpendcularstart1.x, perpendcularstart1.y, perpendcularstart1.z)
        'linetp1.EndPoint.setXYZ(perpendcularend1.x, perpendcularend1.y, perpendcularend1.z)
        'linetp2.StartPoint.setXYZ(perpendcularstart2.x, perpendcularstart2.y, perpendcularstart2.z)
        'linetp2.EndPoint.setXYZ(perpendcularend2.x, perpendcularend2.y, perpendcularend2.z)
        arrintersection = linetp1.GetIntersection(linetp2, True, True, 0.001)
        CcDraworg = arrintersection(0)
    End Function
End Module
