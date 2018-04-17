Module Math_Circle

    '求两个向量的差　－－＞＞２つのベクトルの差を求めます
    'vector为所求向量

    Public Function C_Vector_Sa(ByVal cp1 As GeoPoint, ByVal cp2 As GeoPoint)
        Dim Vectorsa As New GeoPoint
        Vectorsa.x = cp1.x - cp2.x
        Vectorsa.y = cp1.y - cp2.y
        Vectorsa.z = cp1.z - cp2.z
        Return Vectorsa
    End Function

    '两点间的向量　－－＞＞２点間のベクトル
    
    Public Function C_2tenVec(ByVal pntsta As GeoPoint, ByVal pntend As GeoPoint)
        Dim vec As New GeoPoint
        vec = C_Vector_Sa(pntsta, pntend)
        Return vec
    End Function

    '求向量的单位向量　－－＞＞単位ベクトルを求める

    Public Function C_VecSeikika(ByVal vec As GeoPoint)
        Dim rsize As Double
        Dim uvec As New GeoPoint
        rsize = C_VecOhkisa(vec)
        uvec = C_VecScalar(vec, 1.0 / rsize)
        Return uvec
    End Function

    '求向量的大小　－－＞＞ベクトルの大きさを求めます

    Public Function C_VecOhkisa(ByVal vec As GeoPoint) As Double
        C_VecOhkisa = Math.Sqrt((vec.x) * (vec.x) + (vec.y) * (vec.y) + (vec.z) * (vec.z))
    End Function

    '向量的角度二倍　－－＞＞ベクトルの角度の2倍

    Public Function C_VecScalar(ByVal vec As GeoPoint, ByVal rscalar As Double)
        Dim vec_new As New GeoPoint
        vec_new.x = vec.x * rscalar
        vec_new.y = vec.y * rscalar
        vec_new.z = vec.z * rscalar
        Return vec_new
    End Function

    '求两点间的方向余弦　－－＞＞2点間の方向余弦を求めまる

    Public Function C_2tenHoukouYogen(ByVal cp1 As GeoPoint, ByVal cp2 As GeoPoint)
        Dim vec_new As New GeoPoint
        Dim vec_temp As New GeoPoint
        vec_temp = C_2tenVec(cp1, cp2)
        vec_new = C_VecSeikika(vec_temp)
        Return vec_new
    End Function

    '求两个向量的和　－－＞＞２つのベクトルの和を求める

    Public Function C_VecWa(ByVal vec1 As GeoPoint, ByVal vec2 As GeoPoint)
        Dim vec_new As New GeoPoint
        vec_new.x = vec1.x + vec2.x
        vec_new.y = vec1.y + vec2.y
        vec_new.z = vec1.z + vec2.z
        Return vec_new
    End Function

    '向量的外积　－－＞＞外積

    Public Function C_VecGaiseki(ByVal vec1 As GeoPoint, ByVal vec2 As GeoPoint)
        Dim vecgaiseki As New GeoPoint
        vecgaiseki.x = vec1.y * vec2.z - vec1.z * vec2.y
        vecgaiseki.y = vec1.z * vec2.x - vec1.x * vec2.z
        vecgaiseki.z = vec1.x * vec2.y - vec1.y * vec2.x
        Return vecgaiseki
    End Function

    '求向量的单位外积　－－＞＞外積を正規化（単位ベクトルに）

    Public Function C_VecSeikikaGaiseki(ByVal vec1 As GeoPoint, ByVal vec2 As GeoPoint)
        Dim vecgaiseki As New GeoPoint
        Dim vec_new As New GeoPoint
        vecgaiseki = C_VecGaiseki(vec1, vec2)
        vec_new = C_VecSeikika(vecgaiseki)
        Return vec_new
    End Function

    ' 求两点间的中点　－－＞＞2点間の中点を求めます

    Public Function C_2tenChuuten(ByVal pnt1 As GeoPoint, ByVal pnt2 As GeoPoint)
        Dim pnt As New GeoPoint
        pnt.x = (pnt2.x - pnt1.x) * 0.5 + pnt1.x
        pnt.y = (pnt2.y - pnt1.y) * 0.5 + pnt1.y
        pnt.z = (pnt2.z - pnt1.z) * 0.5 + pnt1.z
        Return pnt
    End Function

    '点和方向向量所在直线　－－＞＞

    Public Function C_TenVecChokusen(ByVal pnt As GeoPoint, ByVal vecdir As GeoPoint)
        Dim uvec As New GeoPoint
        uvec = C_VecSeikika(vecdir)
        Dim line As New CUserLine
        line.startPnt.x = pnt.x
        line.startPnt.y = pnt.y
        line.startPnt.z = pnt.z
        line.endPnt.x = uvec.x
        line.endPnt.y = uvec.y
        line.endPnt.z = uvec.z
        Return line
    End Function

    '三点确定一个园　－－＞＞

    Public Sub C_3tenEnko(ByVal pnt1 As GeoPoint, ByVal pnt2 As GeoPoint, ByVal pnt3 As GeoPoint)

    End Sub
End Module
