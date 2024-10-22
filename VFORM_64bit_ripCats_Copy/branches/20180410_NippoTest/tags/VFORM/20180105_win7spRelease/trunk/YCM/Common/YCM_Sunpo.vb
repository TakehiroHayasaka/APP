﻿Imports FBMlib
Imports HalconDotNet

'どの成分を用いるか選択

Public Enum XYZseibun
    'XYZ = 0
    X = 1
    Y = 2
    Z = 4
    XY = 3
    XZ = 5
    YZ = 6
    XYZ = 7
End Enum
'Public Enum XYZseibun2 'どの成分を用いるか選択

'    'XYZ = 0
'    X = 1
'    Y = 2
'    Z = 4
'End Enum

'角度を算出する際、どの成分を用いるか選択

Public Enum Plane
    XY = 1
    XZ = 2
    YZ = 3
End Enum
'a.整列の際、(降順 ， 昇順)を選択

'b.Offset抽出の際、(+方向 ， -方向)を選択

Public Enum SortMethod
    descending = 1 '降順 or +方向

    ascending = 2 '昇順 or -方向

End Enum
'ある値に対して、大きいか・小さいか
Public Enum DaiSyou
    Over = 1 '大きい( ＞ ある値)
    andOver = 2 '以上（ ≧ ある値）

    andUnder = 3 '以下（ ≦ ある値）

    Under = 4 '小さい（ ＜ ある値）

End Enum
''ターゲットのオフセットの方向を選択

'Public Enum Offsetseibun
'    X = 1 '+X方向

'    Y = 2 '+Y方向

'    Z = 4 '+Z方向

'End Enum
''ターゲットのオフセットの方向を選択

'Public Enum OffsetHoukou
'    Plus = 1 '+方向

'    Minus = 2 '-方向

'End Enum
Module YCM_Sunpo
    Public flgAddUserLine As Boolean = False

    '①-1CTのIDからPoint3Dを取得する（1番目/CT内のST4点中）（Yamada）

    '（入力）ID：CTのID
    '（出力）GetPoint_CTID：IDから取得したCTのPoint3D

    Public Function GetPoint_CTID(ByVal ID As Integer) As Point3D
        Try
            Return arrCTData(ID).CT_dat.lstRealP3d.Item(0)
        Catch ex As Exception 'セルがブランクだった場合

            Dim fName As String = System.Reflection.MethodBase.GetCurrentMethod.Name
            errorFunctionMonitor(fName)
            Return Nothing
        End Try
    End Function

    '①-2CTのIDからPoint3D（無単位）を取得する（1番目/CT内のST4点中）（Yamada）

    '（入力）ID：CTのID
    '（出力）GetPoint_CTID：IDから取得したCTのPoint3D（無単位）


    Public Function GetPoint_CTID_NoUnit(ByVal ID As Integer) As Point3D
        Try
            Return arrCTData(ID).CT_dat.lstP3d.Item(0)
        Catch ex As Exception
            Dim fName As String = System.Reflection.MethodBase.GetCurrentMethod.Name
            errorFunctionMonitor(fName)
            Return Nothing
        End Try
    End Function

    '②-1 CT2点間の距離を算出（Yamada）


    Public Function GetDist(ByVal ID1 As Integer, ByVal ID2 As Integer, ByVal XYZ As XYZseibun) As Double
        Try
            Return GetDist(GetPoint_CTID(ID1), GetPoint_CTID(ID2), XYZ) 'CTのIDから3DPointを取得

        Catch ex As Exception 'セルがブランクだった場合

            Dim fName As String = System.Reflection.MethodBase.GetCurrentMethod.Name
            errorFunctionMonitor(fName)
            Return Nothing
        End Try
    End Function

    '②-2 CT2点間の距離を算出（Yamada）

    '（入力）P1，P2：CTのPoint3D
    '        XYZ：求めたい距離のXYZ成分
    '（出力）GetDist：2点間の距離

    Public Function GetDist(ByVal P1 As Point3D, ByVal P2 As Point3D, XYZ As XYZseibun) As Double
        Try
            Select Case XYZ
                'Case XYZseibun.XYZ
                '    GetDist = Math.Sqrt((P2.X - P1.X) * (P2.X - P1.X) + (P2.Y - P1.Y) * (P2.Y - P1.Y) + (P2.Z - P1.Z) * (P2.Z - P1.Z))
                'Case XYZseibun.X
                '    GetDist = Math.Abs(P1.X - P2.X)
                'Case XYZseibun.Y
                '    GetDist = Math.Abs(P1.Y - P2.Y)
                'Case XYZseibun.XY
                '    GetDist = Math.Sqrt((P2.X - P1.X) * (P2.X - P1.X) + (P2.Y - P1.Y) * (P2.Y - P1.Y))
                'Case XYZseibun.Z
                '    GetDist = Math.Abs(P1.Z - P2.Z)
                'Case XYZseibun.XZ
                '    GetDist = Math.Sqrt((P2.X - P1.X) * (P2.X - P1.X) + (P2.Z - P1.Z) * (P2.Z - P1.Z))
                'Case XYZseibun.YZ
                '    GetDist = Math.Sqrt((P2.Y - P1.Y) * (P2.Y - P1.Y) + (P2.Z - P1.Z) * (P2.Z - P1.Z))
                'Case Else
                '    Exit Function
                Case XYZseibun.XYZ
                    GetDist = Math.Sqrt((P2.X.D - P1.X.D) * (P2.X.D - P1.X.D) + (P2.Y.D - P1.Y.D) * (P2.Y.D - P1.Y.D) + (P2.Z.D - P1.Z.D) * (P2.Z.D - P1.Z.D))
                Case XYZseibun.X
                    GetDist = Math.Abs(P1.X.D - P2.X.D)
                Case XYZseibun.Y
                    GetDist = Math.Abs(P1.Y.D - P2.Y.D)
                Case XYZseibun.XY
                    GetDist = Math.Sqrt((P2.X.D - P1.X.D) * (P2.X.D - P1.X.D) + (P2.Y.D - P1.Y.D) * (P2.Y.D - P1.Y.D))
                Case XYZseibun.Z
                    GetDist = Math.Abs(P1.Z.D - P2.Z.D)
                Case XYZseibun.XZ
                    GetDist = Math.Sqrt((P2.X.D - P1.X.D) * (P2.X.D - P1.X.D) + (P2.Z.D - P1.Z.D) * (P2.Z.D - P1.Z.D))
                Case XYZseibun.YZ
                    GetDist = Math.Sqrt((P2.Y.D - P1.Y.D) * (P2.Y.D - P1.Y.D) + (P2.Z.D - P1.Z.D) * (P2.Z.D - P1.Z.D))
                Case Else
                    Exit Function

            End Select
        Catch ex As Exception
            Dim fName As String = System.Reflection.MethodBase.GetCurrentMethod.Name
            errorFunctionMonitor(fName)
            Return Nothing
        End Try
    End Function
    '②-2 CT2点間の距離を算出（Yamada）

    '（入力）P1，P2：CTのPoint3D
    '        XYZ：求めたい距離のXYZ成分
    '（出力）GetDist：2点間の距離

    Public Function GetSabunXYZ(ByVal P1 As Point3D, ByVal P2 As Point3D, XYZ As XYZseibun) As Double
        Try
            Select Case XYZ

                Case XYZseibun.X
                    GetSabunXYZ = P1.X - P2.X
                Case XYZseibun.Y
                    GetSabunXYZ = P1.Y - P2.Y
                Case XYZseibun.Z
                    GetSabunXYZ = P1.Z - P2.Z
                Case Else
                    Exit Function
            End Select
        Catch ex As Exception
            Dim fName As String = System.Reflection.MethodBase.GetCurrentMethod.Name
            errorFunctionMonitor(fName)
            Return Nothing
        End Try
    End Function
    '③-1 原点からCTまでの距離を算出（Yamada）

    Public Function GetDist(ByVal ID1 As Integer, ByVal XYZ As XYZseibun) As Double
        'Dim OriginP As New FBMlib.Point3D '原点
        'OriginP.X = 0
        'OriginP.Y = 0
        'OriginP.Z = 0
        Try
            Return GetDist(GetPoint_CTID(ID1), XYZ) 'CTのIDから3DPointを取得

        Catch ex As Exception 'セルがブランクだった場合

            Dim fName As String = System.Reflection.MethodBase.GetCurrentMethod.Name
            errorFunctionMonitor(fName)
            Return Nothing
        End Try
    End Function

    '③-2 原点からCTまでの距離を算出（Yamada）

    Public Function GetDist(ByVal P1 As Point3D, ByVal XYZ As XYZseibun) As Double
        Dim OriginP As New FBMlib.Point3D '原点
        OriginP.X = 0
        OriginP.Y = 0
        OriginP.Z = 0
        Try
            Return GetDist(OriginP, P1, XYZ) 'CTのIDから3DPointを取得

        Catch ex As Exception 'セルがブランクだった場合

            Dim fName As String = System.Reflection.MethodBase.GetCurrentMethod.Name
            errorFunctionMonitor(fName)
            Return Nothing
        End Try
    End Function

    '④-1 P3から線分P1-P2までの距離を算出（Yamada）

    Public Function GetDist(ByVal ID1 As Integer, ByVal ID2 As Integer, ByVal ID3 As Integer, ByVal XYZ As XYZseibun) As Double
        Try
            Return GetDist(GetPoint_CTID(ID1), GetPoint_CTID(ID2), GetPoint_CTID(ID3), XYZ) 'CTのIDから3DPointを取得

        Catch ex As Exception
            Dim fName As String = System.Reflection.MethodBase.GetCurrentMethod.Name
            errorFunctionMonitor(fName)
            Return Nothing
        End Try
    End Function
    '④-2 P3から線分P1-P2までの距離を算出（Yamada）

    '（1）P3から線分P1-P2に下した垂線の交点（Pfoot）のPoint3Dを作成
    '（2）P3からPfootまでの距離を算出

    '（入力）P1，P2：CTのPoint3D(線分の端点)
    '        P3：CTのPoint3D
    '　　　　XYZ：求めたい距離のXYZ成分
    '（出力）GetDistPtoL：算出した距離

    Public Function GetDist(ByVal P1 As Point3D, ByVal P2 As Point3D, ByVal P3 As Point3D, ByVal XYZ As XYZseibun) As Double
        Try
            If (IsNothing(P1) Or IsNothing(P2) Or IsNothing(P3)) Then
                Exit Function
            ElseIf XYZ = 0 Then
                Exit Function
            End If

            Dim geoL As New GeoCurve '線分P1-P2
            Dim geo_PF As New GeoPoint '垂線の足（点geoP3から線分geoLへおろした垂線と線分geoLの交点）


            geoL.StartPoint.SetFBM_3DPoint(P1) '始点
            geoL.EndPoint.SetFBM_3DPoint(P2) '終点

            Return GetDist(P3, geoL, XYZ) '13.6.11修正　Yamada
        Catch ex As Exception
            Dim fName As String = System.Reflection.MethodBase.GetCurrentMethod.Name
            errorFunctionMonitor(fName)
            Return Nothing
        End Try




        ''13.6.11修正前　Yamada ========================================
        'If (IsNothing(P1) Or IsNothing(P2) Or IsNothing(P3)) Then
        '    Exit Function
        'ElseIf XYZ = 0 Then
        '    Exit Function
        'End If

        'Dim geoL As New GeoCurve '線分P1-P2
        'Dim geoP3 As New GeoPoint 'P3
        'Dim geo_PF As New GeoPoint '垂線の足（点geoP3から線分geoLへおろした垂線と線分geoLの交点）


        'geoL.StartPoint.SetFBM_3DPoint(P1) '始点
        'geoL.EndPoint.SetFBM_3DPoint(P2) '終点
        'geoP3.SetFBM_3DPoint(P3)
        'geo_PF = geoL.GetPerpendicularFoot(geoP3) '垂線の足（交点）

        ''GetDistPtoL = geo_PF.GetDistanceTo(geoP3)'
        'Dim Pfoot As New FBMlib.Point3D
        'Pfoot.X = geo_PF.x
        'Pfoot.Y = geo_PF.y
        'Pfoot.Z = geo_PF.z

        ''P3とPfoot（垂線の足）の2点間の距離を算出⇒②へ
        'Try
        '    Return GetDist(P3, Pfoot, XYZ)
        'Catch ex As Exception 'セルがブランクだった場合

        '    Return Nothing
        'End Try
        ''13.6.11修正前　Yamada ========================================
    End Function

    '④-3 P3から線分lineまでの距離を算出（13.6.11 Yamada）

    '（1）P3から線分lineに下した垂線の交点（Pfoot）のPoint3Dを作成
    '（2）P3からPfootまでの距離を算出

    '（入力）P1：CTのPoint3D
    '        Line1：GeoCurve
    '　　　　XYZ：求めたい距離のXYZ成分
    '（出力）GetDistPtoL：算出した距離

    Public Function GetDist(ByVal P1 As Point3D, ByVal Line1 As GeoCurve, ByVal XYZ As XYZseibun) As Double

        If (IsNothing(P1) Or IsNothing(Line1) Or IsNothing(XYZ)) Then
            Exit Function
        End If

        Dim geoP1 As New GeoPoint
        Dim geo_PF As New GeoPoint '垂線の足（点geoP3から線分geoLへおろした垂線と線分geoLの交点）

        geoP1.SetFBM_3DPoint(P1)
        geo_PF = Line1.GetPerpendicularFoot(geoP1) '垂線の足（交点）

        Dim Pfoot As New FBMlib.Point3D
        Pfoot.X = geo_PF.x
        Pfoot.Y = geo_PF.y
        Pfoot.Z = geo_PF.z
        Try
            If flgAddUserLine = True Then    'SUURI ADD 20130531
                AddUserLine(P1, Pfoot)
            End If
            Return GetDist(P1, Pfoot, XYZ)
        Catch ex As Exception
            Dim fName As String = System.Reflection.MethodBase.GetCurrentMethod.Name
            errorFunctionMonitor(fName)
            Return Nothing
        End Try
    End Function

    '⑤-1 点と面の距離を算出
    '3点P1,P2,P3を通る平面πの式はa*x+b*y+c*z+d = 0  //（a,b,c）は平面πの法線ベクトル
    '（入力値）P1、P2、P3：平面πを作成する3点のCTのID
    '          P0：平面からの距離を算出するCTのID
    '          XYZ：XYZ：求めたい距離のXYZ成分
    '（出力値）GetDist：平面πから点P0までの距離

    Public Function GetDist(ByVal ID1 As Integer, ByVal ID2 As Integer, ByVal ID3 As Integer, ByVal ID0 As Integer, ByVal XYZ As XYZseibun) As Double
        Try
            Return GetDist(GetPoint_CTID(ID1), GetPoint_CTID(ID2), GetPoint_CTID(ID3), GetPoint_CTID(ID0), XYZ) 'CTのIDから3DPointを取得

        Catch ex As Exception
            Dim fName As String = System.Reflection.MethodBase.GetCurrentMethod.Name
            errorFunctionMonitor(fName)
            Return Nothing
        End Try
    End Function

    '⑤-2 点と面の距離を算出
    '3点P1,P2,P3を通る平面πの式はa*x+b*y+c*z+d = 0  //（a,b,c）は平面πの法線ベクトル
    '（入力値）P1、P2、P3：平面πを作成する3点のCTのPoint3D
    '          P0：平面からの距離を算出するCTのPoint3D
    '          XYZ：XYZ：求めたい距離のXYZ成分
    '（出力値）GetDist：平面πから点P0までの距離

    Public Function GetDist(ByVal P1 As Point3D, ByVal P2 As Point3D, ByVal P3 As Point3D, ByVal P0 As Point3D, ByVal XYZ As XYZseibun) As Double

        If (IsNothing(P1) Or IsNothing(P2) Or IsNothing(P3) Or IsNothing(P0)) Then
            Exit Function
        ElseIf XYZ = 0 Then
            Exit Function
        End If

        Dim Pnt1, Pnt2, Pnt3, Pnt0 As New GeoPoint 'Point3D→GeoPoint
        Dim Normal As New GeoVector '平面πの法線ベクトルの作成（V12→V13）

        Dim VLine As New GeoCurve '法線ベクトルNormalと点P0から作成したGeoCurve
        Dim PIPlane As New GeoPlane '平面πのGeoPlane
        Dim interPnt As New GeoPoint '点P0から平面πに下した垂線の交点
        '（1）P1～P3,P0をGeoPointに変換
        Pnt1.SetFBM_3DPoint(P1)
        Pnt2.SetFBM_3DPoint(P2)
        Pnt3.SetFBM_3DPoint(P3)
        Pnt0.SetFBM_3DPoint(P0)
        Dim V12 As New GeoVector(Pnt1, Pnt2) '2Vectorを作成（P1→P2,P1→P3）

        Dim V13 As New GeoVector(Pnt1, Pnt3)

        '（2）平面πの法線ベクトルの作成（V12→V13）

        Normal = V12.GetOuterProduct(V13)

        '（3）Origin点(P1)と法線ベクトルNormal(V13→V12)からGeoPlaneを作成
        PIPlane = GeoPlane_ByOriginNormal(Pnt1, Normal)

        Try
            Return GetDist(Pnt0, PIPlane, XYZ) '→（5）-3へ
        Catch ex As Exception
            Dim fName As String = System.Reflection.MethodBase.GetCurrentMethod.Name
            errorFunctionMonitor(fName)
            Return Nothing
        End Try



        ''H25.5.9修正前　Yamada　：（5）-3を用いずに距離を得る＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝

        ''（1）P1～P3,P0をGeoPointに変換
        'Dim Pnt1, Pnt2, Pnt3, Pnt0 As New GeoPoint 'Point3D→GeoPoint
        'Pnt1.SetFBM_3DPoint(P1)
        'Pnt2.SetFBM_3DPoint(P2)
        'Pnt3.SetFBM_3DPoint(P3)
        'Pnt0.SetFBM_3DPoint(P0)

        ''（2）2Vectorを作成（P1→P2,P1→P3を設定）

        'Dim V12 As New GeoVector(Pnt1, Pnt2)
        'Dim V13 As New GeoVector(Pnt1, Pnt3)

        ''（3）平面πの法線ベクトルの作成（V12→V13）

        'Dim Normal As New GeoVector
        'Normal = V12.GetOuterProduct(V13)

        'Dim VLine As New GeoCurve '法線ベクトルNormalと点P0から作成したGeoCurve
        'Dim PIPlane As New GeoPlane '平面πのGeoPlane
        ''Dim VPlane As New GeoPlane '点P0とLineから作成したGeoPlane
        'Dim interPnt As New GeoPoint '点P0から平面πに下した垂線の交点

        ''（4）Origin点(P1)と法線ベクトルNormal(V13→V12)からGeoPlaneを作成
        'PIPlane = GeoPlane_ByOriginNormal(Pnt1, Normal)

        ''（5）始点と終点をつなぐ直線VLineを作成
        'VLine.StartPoint = Pnt0  '始点のセット
        'VLine.EndPoint = VLine.StartPoint.GetMoved(Normal) '終点のセット（始点から法線Normal分移動）


        ''（6）点P0と直線VLineから、平面πの交点を算出
        'interPnt = PIPlane.GetIntersectionWithCurve(VLine, True) '（True： / False： )
        ''interPnt = VPlane.GetIntersectionWithCurve(VLine, True) '（True： / False： )

        'Dim Pfoot As New FBMlib.Point3D
        'Pfoot = interPnt.GetP3D() '交点interPntをGeoPointからPoint3Dに変換

        ''（7）点P0とPfoot（垂線の足）の2点間の距離を算出
        'Try
        '    Return GetDist(P0, Pfoot, XYZ)
        'Catch ex As Exception 'セルがブランクだった場合

        '    Return Nothing
        'End Try
        ''H25.5.9修正前　Yamada　：（5）-3を用いずに距離を得る＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝


    End Function
    '⑤-3 点と面の距離を算出
    '
    '（入力値）Pnt0：点 GeoPoint
    '          PIPlane：平面π GeoPlane
    '          XYZ：XYZ：求めたい距離のXYZ成分
    '（出力値）GetDist：平面πから点P0までの距離

    Public Function GetDist(ByVal Pnt0 As GeoPoint, ByVal PIPlane As GeoPlane, ByVal XYZ As XYZseibun) As Double


        If IsNothing(Pnt0) Then
            Exit Function
        ElseIf IsNothing(PIPlane) Then
            Exit Function
        ElseIf XYZ = 0 Then
            Exit Function
        End If


        Dim VLine As New GeoCurve '点から平面の法線ベクトル分移動した点とのCurve
        Dim InterPoint As New GeoPoint '点から平面πへ下した垂線の足
        Dim Pfoot As New Point3D
        Dim P0 As New Point3D
        Dim Normal As GeoVector = PIPlane.normal '平面πの法線ベクトル

        '（1）Curve作成
        VLine.StartPoint = Pnt0
        VLine.EndPoint = VLine.StartPoint.GetMoved(Normal)

        '（2）Curveと平面との交点（垂線の足）を得る
        InterPoint = PIPlane.GetIntersectionWithCurve(VLine, True)

        '（3）点および垂線の足をGeoPoint→Point3Dに
        Pfoot = InterPoint.GetP3D()
        P0 = Pnt0.GetP3D()

        Try
            If flgAddUserLine = True Then    'SUURI ADD 20130531
                AddUserLine(P0, Pfoot)
            End If
            Return GetDist(P0, Pfoot, XYZ)

        Catch ex As Exception
            Dim fName As String = System.Reflection.MethodBase.GetCurrentMethod.Name
            errorFunctionMonitor(fName)
            Return Nothing
        End Try

    End Function

    '⑥-1 2つのベクトルのなす角度を算出（原点を回転軸に、P1からP2の角度）（Yamada）

    '（入力）ID1,ID2：既存の2点のCTのID
    '　　　  XYZ：求めたい角度のXYZ成分
    Public Function GetAngleRight(ByVal ID1 As Integer, ByVal ID2 As Integer, ByVal XYZ As Plane) As Double
        Try
            Return GetAngleRight(GetPoint_CTID(ID1), GetPoint_CTID(ID2), XYZ)
        Catch ex As Exception
            Dim fName As String = System.Reflection.MethodBase.GetCurrentMethod.Name
            errorFunctionMonitor(fName)
            Return Nothing
        End Try
    End Function

    '⑥-2 2つのベクトルのなす角度を算出（原点を回転軸に、P1からP2の角度）（Yamada）

    '（入力）P1,P2：2点のPoint3D（座標位置）

    '        XYZ  ：求めたい角度のXYZ成分
    '（出力）GetAngleRight：角度（P1⇒P2の右回りのなす角）


    Public Function GetAngleRight(ByVal P1 As Point3D, ByVal P2 As Point3D, ByVal XYZ As Plane) As Double
        Try
            If (IsNothing(P1) Or IsNothing(P2)) Then
                Exit Function
            ElseIf XYZ = 0 Then
                Exit Function
            End If

            Dim X1, Y1, X2, Y2 As New Double
            Dim angR As New Double
            Dim S As New Double

            Select Case XYZ
                Case Plane.XY
                    X1 = P1.X
                    Y1 = P1.Y
                    X2 = P2.X
                    Y2 = P2.Y
                    '鋭角を求める(0<=ang1<=180)：内積

                    angR = Math.Acos((X1 * X2 + Y1 * Y2) / System.Math.Sqrt(X1 * X1 + Y1 * Y1) * System.Math.Sqrt(X2 * X2 + Y2 * Y2)) '(Rad)

                    'P1⇒P2が右廻り（時計廻り）なのか左廻り（反時計廻り）なのかを調べる：外積

                    S = X1 * Y2 - X2 * Y1
                    If S < 0 Then 'P1⇒P2は右回り
                        GetAngleRight = angR
                    ElseIf S > 0 Then 'P1⇒P2は左回り
                        GetAngleRight = 2 * Math.PI - angR '求めたい角は鈍角の方（右廻り）

                    End If
                Case Plane.XZ

                Case Plane.YZ

            End Select
        Catch ex As Exception
            Dim fName As String = System.Reflection.MethodBase.GetCurrentMethod.Name
            errorFunctionMonitor(fName)
            Return Nothing
        End Try

    End Function

    '⑦-1 CT3点のXYZ成分を用いて、新規にCT1点のPoint3Dを作成（Yamada）

    '（入力）ID1，ID2，ID3：既存の3点のCTのID
    '        XYZ1,XYZ2,XYZ3：選択したP1、P2、P3のXYZ成分

    Public Function GetPoint(ByVal ID1 As Integer, ByVal XYZ1 As XYZseibun, ByVal ID2 As Integer, ByVal XYZ2 As XYZseibun, ByVal ID3 As Integer, ByVal XYZ3 As XYZseibun) As Point3D
        GetPoint = New Point3D
        Try
            Return GetPoint(GetPoint_CTID(ID1), XYZ1, GetPoint_CTID(ID2), XYZ2, GetPoint_CTID(ID3), XYZ3) 'CTのIDから3DPointを取得

        Catch ex As Exception
            Dim fName As String = System.Reflection.MethodBase.GetCurrentMethod.Name
            errorFunctionMonitor(fName)
            Return Nothing
        End Try
    End Function

    '⑦-2 CT3点のXYZ成分を用いて、新規にCT1点のPoint3Dを作成（Yamada）

    '（入力）P1，P2，P3：既存の3点のPoint3D（座標位置）

    '        XYZ1，XYZ2，XYZ3：選択したP1、P2、P3のXYZ成分
    '（出力）GetPoint：新規に作成したPoint3D

    '例)（入力）P1 = (X1,Y1,Z1)　，XYZ1 = X　，P2 = (X2,Y2,Z2)　，XYZ2 = XY　，P3 = (X3,Y3,Z3)　，XYZ3 = nothing
    '　 （出力）GetPoint =（X1,Y2,Z2）


    Public Function GetPoint(ByVal P1 As Point3D, ByVal XYZ1 As XYZseibun, ByVal P2 As Point3D, ByVal XYZ2 As XYZseibun, ByVal P3 As Point3D, ByVal XYZ3 As XYZseibun) As Point3D
        Try
            GetPoint = New Point3D
            Select Case XYZ1
                Case XYZseibun.XYZ
                    GetPoint.X = P1.X
                    GetPoint.Y = P1.Y
                    GetPoint.Z = P1.Z
                Case XYZseibun.X
                    GetPoint.X = P1.X
                Case XYZseibun.Y
                    GetPoint.Y = P1.Y
                Case XYZseibun.XY
                    GetPoint.X = P1.X
                    GetPoint.Y = P1.Y
                Case XYZseibun.Z
                    GetPoint.Z = P1.Z
                Case XYZseibun.XZ
                    GetPoint.X = P1.X
                    GetPoint.Z = P1.Z
                Case XYZseibun.YZ
                    GetPoint.Y = P1.Y
                    GetPoint.Z = P1.Z
                Case Else
                    Exit Function
            End Select

            Select Case XYZ2
                Case XYZseibun.XYZ
                    GetPoint.X = P2.X
                    GetPoint.Y = P2.Y
                    GetPoint.Z = P2.Z
                Case XYZseibun.X
                    GetPoint.X = P2.X
                Case XYZseibun.Y
                    GetPoint.Y = P2.Y
                Case XYZseibun.XY
                    GetPoint.X = P2.X
                    GetPoint.Y = P2.Y
                Case XYZseibun.Z
                    GetPoint.Z = P2.Z
                Case XYZseibun.XZ
                    GetPoint.X = P2.X
                    GetPoint.Z = P2.Z
                Case XYZseibun.YZ
                    GetPoint.Y = P2.Y
                    GetPoint.Z = P2.Z
                Case Else
                    Exit Function
            End Select

            Select Case XYZ3
                Case XYZseibun.XYZ
                    GetPoint.X = P3.X
                    GetPoint.Y = P3.Y
                    GetPoint.Z = P3.Z
                Case XYZseibun.X
                    GetPoint.X = P3.X
                Case XYZseibun.Y
                    GetPoint.Y = P3.Y
                Case XYZseibun.XY
                    GetPoint.X = P3.X
                    GetPoint.Y = P3.Y
                Case XYZseibun.Z
                    GetPoint.Z = P3.Z
                Case XYZseibun.XZ
                    GetPoint.X = P3.X
                    GetPoint.Z = P3.Z
                Case XYZseibun.YZ
                    GetPoint.Y = P3.Y
                    GetPoint.Z = P3.Z
                Case Else
                    Exit Function
            End Select

            'エラー処理：もしもGetPointの成分に「nothing」があれば
            If IsDBNull(GetPoint.X) Then
                MessageBox.Show("作成する点にX成分がありません。", "エラー", _
           MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Function

            ElseIf IsDBNull(GetPoint.Y) Then
                MessageBox.Show("作成する点にY成分がありません。", "エラー", _
          MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Function

            ElseIf IsDBNull(GetPoint.Z) Then
                MessageBox.Show("作成する点にZ成分がありません。", "エラー", _
          MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Function
            End If
        Catch ex As Exception
            Dim fName As String = System.Reflection.MethodBase.GetCurrentMethod.Name
            errorFunctionMonitor(fName)
        End Try
    End Function

    '⑧-1 arCTDataの整列を座標成分を指定して並び替え　（Yamada）

    '=============================================================================================
    'b) CT_Dataが2つ入力された場合


    '（入力）NonSortedList：入力arrCTData
    '　　　　XYZ：ソーティングを行う項目（成分）

    '        SortHouhou：ソーティングの方法（降順 descending / 昇順 ascending ）

    '（出力）SortedList：ソーティングされたarrCTData
    '（テスト）GetSortedTest_1()
    '=============================================================================================
    Public Sub GetSorted(ByVal NonSortedList() As CT_Data, ByVal XYZ As XYZseibun, ByVal SortHouhou As SortMethod, ByVal BasePoint As Point3D, ByRef SortedList() As CT_Data)
        Try
            GetSorted(NonSortedList, XYZ, SortHouhou, BasePoint)
            SortedList = NonSortedList
        Catch ex As Exception
            Dim fName As String = System.Reflection.MethodBase.GetCurrentMethod.Name
            errorFunctionMonitor(fName)
        End Try
    End Sub

    '⑧-2 arCTDataの整列を座標成分を指定して並び替え//LINQ使用　（Yamada）

    '=============================================================================================
    'a) CT_Dataが一つ入力された場合


    '（入力）  SortedList：ソーティングを行うリスト

    '          XYZ：ソーティングを行う項目（成分）

    '          SortHouhou：ソーティングの方法（降順 descending / 昇順 ascending ）

    '（入出力）SortedList：ソーティングされたarrCTData
    '（テスト）GetSortedTest_2
    '=============================================================================================

    Public Sub GetSorted(ByRef SortedList() As CT_Data, ByVal XYZ As XYZseibun, ByVal SortHouhou As SortMethod, ByVal BasePoint As Point3D)
        Try
            '定義（変数）の作成-------------------------------------------------------------
            '（1）X，降順

            Dim XDes = From CTD_XD As CT_Data In SortedList
                        Where CTD_XD IsNot Nothing
                        Order By CTD_XD.CT_dat.lstRealP3d(0).X.D Descending

            'Select CTD_XD

            '（2）X，昇順

            Dim XAsc = From CTD_XA As CT_Data In SortedList
                        Where CTD_XA IsNot Nothing
                        Order By CTD_XA.CT_dat.lstRealP3d(0).X.D Ascending
            'Select CTD_XA

            '（3）Y，降順

            Dim YDes = From CTD_YD As CT_Data In SortedList
                        Where CTD_YD IsNot Nothing
                        Order By CTD_YD.CT_dat.lstRealP3d(0).Y.D Descending
            'Select CTD_YD
            'Dim arrCTXdict() As CT_Data = XDes.ToArray

            '（4）Y，昇順

            Dim YAsc = From CTD_YA As CT_Data In SortedList
                        Where CTD_YA IsNot Nothing
                        Order By CTD_YA.CT_dat.lstRealP3d(0).Y.D Ascending
            'Select CTD_YA

            '（5）Z，降順

            Dim ZDes = From CTD_ZD As CT_Data In SortedList
                        Where CTD_ZD IsNot Nothing
                        Order By CTD_ZD.CT_dat.lstRealP3d(0).Z.D Descending
            'Select CTD_ZD


            '（6）Z，昇順

            Dim ZAsc = From CTD_ZA As CT_Data In SortedList
                        Where CTD_ZA IsNot Nothing
                        Order By CTD_ZA.CT_dat.lstRealP3d(0).Z.D Ascending
            'Select CTD_ZA

            '(20150803 Tezuka ADD) 2点間距離の降順、昇順の追加

            '（7）XY, 降順

            Dim XYDes = From CTD_XYD As CT_Data In SortedList
                        Where CTD_XYD IsNot Nothing
                        Order By (System.Math.Sqrt((BasePoint.X.D - CTD_XYD.CT_dat.lstRealP3d(0).X.D) ^ 2 + (BasePoint.Y.D - CTD_XYD.CT_dat.lstRealP3d(0).Y.D) ^ 2)) Descending

            '（8）XY, 昇順

            Dim XYAsc = From CTD_XYA As CT_Data In SortedList
                        Where CTD_XYA IsNot Nothing
                        Order By System.Math.Sqrt((BasePoint.X.D - CTD_XYA.CT_dat.lstRealP3d(0).X.D) ^ 2 + (BasePoint.Y.D - CTD_XYA.CT_dat.lstRealP3d(0).Y.D) ^ 2) Ascending

            '（9）XZ, 降順

            Dim XZDes = From CTD_XZD As CT_Data In SortedList
                        Where CTD_XZD IsNot Nothing
                        Order By System.Math.Sqrt((BasePoint.X.D - CTD_XZD.CT_dat.lstRealP3d(0).X.D) ^ 2 + (BasePoint.Z.D - CTD_XZD.CT_dat.lstRealP3d(0).Z.D) ^ 2) Descending

            '（10）XZ, 昇順

            Dim XZAsc = From CTD_XZA As CT_Data In SortedList
                        Where CTD_XZA IsNot Nothing
                        Order By System.Math.Sqrt((BasePoint.X.D - CTD_XZA.CT_dat.lstRealP3d(0).X.D) ^ 2 + (BasePoint.Z.D - CTD_XZA.CT_dat.lstRealP3d(0).Z.D) ^ 2) Ascending

            '（11）YZ, 降順

            Dim YZDes = From CTD_YZD As CT_Data In SortedList
                        Where CTD_YZD IsNot Nothing
                        Order By System.Math.Sqrt((BasePoint.Y.D - CTD_YZD.CT_dat.lstRealP3d(0).Y.D) ^ 2 + (BasePoint.Z.D - CTD_YZD.CT_dat.lstRealP3d(0).Z.D) ^ 2) Descending

            '（12）YZ, 昇順

            Dim YZAsc = From CTD_YZA As CT_Data In SortedList
                        Where CTD_YZA IsNot Nothing
                        Order By System.Math.Sqrt((BasePoint.Y.D - CTD_YZA.CT_dat.lstRealP3d(0).Y.D) ^ 2 + (BasePoint.Z.D - CTD_YZA.CT_dat.lstRealP3d(0).Z.D) ^ 2) Ascending

            '（13）XYZ, 降順

            Dim XYZDes = From CTD_XYZD As CT_Data In SortedList
                        Where CTD_XYZD IsNot Nothing
                        Order By System.Math.Sqrt((BasePoint.X.D - CTD_XYZD.CT_dat.lstRealP3d(0).X.D) ^ 2 + (BasePoint.Y.D - CTD_XYZD.CT_dat.lstRealP3d(0).Y.D) ^ 2 + (BasePoint.Z.D - CTD_XYZD.CT_dat.lstRealP3d(0).Z.D) ^ 2) Descending

            '（14）XYZ, 昇順

            Dim XYZAsc = From CTD_XYZA As CT_Data In SortedList
                        Where CTD_XYZA IsNot Nothing
                        Order By System.Math.Sqrt((BasePoint.X.D - CTD_XYZA.CT_dat.lstRealP3d(0).X.D) ^ 2 + (BasePoint.Y.D - CTD_XYZA.CT_dat.lstRealP3d(0).Y.D) ^ 2 + (BasePoint.Z.D - CTD_XYZA.CT_dat.lstRealP3d(0).Z.D) ^ 2) Ascending

            'arrayListの作成-------------------------------------------------------------
            Select Case XYZ

                Case XYZseibun.X '-->成分がX
                    If SortHouhou = SortMethod.descending Then
                        SortedList = XDes.ToArray '-->ソートは降順

                    Else
                        SortedList = XAsc.ToArray '-->ソートは昇順

                    End If

                Case XYZseibun.Y '-->成分がY
                    If SortHouhou = SortMethod.descending Then
                        SortedList = YDes.ToArray '-->ソートは降順

                    Else
                        SortedList = YAsc.ToArray '-->ソートは昇順

                    End If

                Case XYZseibun.Z '-->成分がZ
                    If SortHouhou = SortMethod.descending Then
                        SortedList = ZDes.ToArray '-->ソートは降順

                    Else
                        SortedList = ZAsc.ToArray '-->ソートは昇順

                    End If
                    '(20150803 Tezuka ADD) XY,XZ,YZの追加
                Case XYZseibun.XY '-->成分がXY
                    If SortHouhou = SortMethod.descending Then
                        SortedList = XYDes.ToArray '-->ソートは降順

                    Else
                        SortedList = XYAsc.ToArray '-->ソートは昇順

                    End If
                Case XYZseibun.XZ '-->成分がXZ
                    If SortHouhou = SortMethod.descending Then
                        SortedList = XZDes.ToArray '-->ソートは降順

                    Else
                        SortedList = XZAsc.ToArray '-->ソートは昇順

                    End If
                Case XYZseibun.YZ '-->成分がYZ
                    If SortHouhou = SortMethod.descending Then
                        SortedList = YZDes.ToArray '-->ソートは降順

                    Else
                        SortedList = YZAsc.ToArray '-->ソートは昇順

                    End If
                Case XYZseibun.XYZ '-->成分がXYZ
                    If SortHouhou = SortMethod.descending Then
                        SortedList = XYZDes.ToArray '-->ソートは降順

                    Else
                        SortedList = XYZAsc.ToArray '-->ソートは昇順

                    End If
            End Select
        Catch ex As Exception
            Dim fName As String = System.Reflection.MethodBase.GetCurrentMethod.Name
            errorFunctionMonitor(fName)
        End Try

    End Sub

    '（27）-1 arCTDataの整列をIDについて並び替え//LINQ使用　（H25.7.5 Yamada）

    '=============================================================================================
    'CT_Dataが一つ入力された場合


    '（入力）  SortHouhou：ソーティングの方法（降順 descending / 昇順 ascending ）

    '（入出力）SortedList()：ソーティングを行うCT_Data
    '（テスト）GetSortedByCTID_Test1()
    '=============================================================================================

    Public Sub GetSortedByCTID(ByRef SortedList() As CT_Data, ByVal SortHouhou As SortMethod)
        Try
            Dim Temp1 = From CTDataItem As CT_Data In SortedList
                            Where CTDataItem IsNot Nothing
            Dim Temp2 = From CTDataItem As CT_Data In SortedList
                           Where CTDataItem.CT_dat IsNot Nothing
            'Dim Temp3 = From CTDataItem As CT_Data In SortedList
            'Where CTDataItem.CT_dat.PID IsNot Nothing

            '（1）降順

            Dim Koujyun = From CTDataList As CT_Data In Temp2
                          Order By CTDataList.CT_dat.PID Descending
            'Order By CTDataList.CT_dat.lstRealP3d(0).X.Descending

            '（2）昇順

            Dim Syoujyun = From CTDataList As CT_Data In Temp2
                           Order By CTDataList.CT_dat.PID Ascending
            'Order By CTDataList.CT_dat.lstRealP3d(0).X Ascending

            If SortHouhou = SortMethod.descending Then
                SortedList = Koujyun.ToArray '-->ソートは降順

            Else
                SortedList = Syoujyun.ToArray '-->ソートは昇順

            End If
        Catch ex As Exception
            'errorFunctionMonitor(ex.Source.ToString)
            Dim fName As String = System.Reflection.MethodBase.GetCurrentMethod.Name
            errorFunctionMonitor(fName)
        End Try
    End Sub

    '（27）-2 arCTDataの整列をIDについて並び替え//LINQ使用　（H25.7.5 Yamada）

    '=============================================================================================
    'CT_Dataが二つ入力された場合


    '（入力）  NonSortedList()：入力CT_Data
    '          SortHouhou：ソーティングの方法（降順 descending / 昇順 ascending ）

    '（入出力）SortedList()：ソーティングされたCT_Data
    '（テスト）GetSortedByCTID_Test2(),GetSortedByCTID_Test0()
    '=============================================================================================

    Public Sub GetSortedByCTID(ByVal NonSortedList() As CT_Data, ByVal SortHouhou As SortMethod, ByRef SortedList() As CT_Data)
        Try
            GetSortedByCTID(NonSortedList, SortHouhou)
            SortedList = NonSortedList
        Catch ex As Exception
            Dim fName As String = System.Reflection.MethodBase.GetCurrentMethod.Name
            errorFunctionMonitor(ex.Source)
        End Try
    End Sub





    '⑨-1 点群の指定したXYZ成分に対して、ある値より大きい or 小さい範囲にある点群を返す（フィルタリング）　H25.4.26 Yamada
    '=============================================================================================
    'a) CT_Data 1つ，Point3Dが入力された場合


    '（入力）PointGroup：フィルタリング対象となる点群
    '        XYZ：成分の指定

    '        BasePoint：フィルタリングの基準となるPoint3D
    '        Daisyou：大小関係を指定

    '（入出力）PointGroup：フィルタリングした点群
    '（テスト）GetAreaPointGroupTest1()
    '=============================================================================================

    Public Sub GetAreaPointGroup(ByRef PointGroup() As CT_Data, ByVal XYZ As XYZseibun, ByVal BasePoint As Point3D, ByVal DaiSyou As DaiSyou)
        Try
            Dim TempQ = From T As CT_Data In PointGroup
                                Where T IsNot Nothing

            ''（1）BasePoint（X,Y,Z）のXより大きい範囲(DaiSyou=Over)の点群を返す
            'Dim X_Over = From X_O As CT_Data In TempQ
            '             Where X_O.CT_dat.lstRealP3d(0).X > BasePoint.X

            ''（2）BasePoint（X,Y,Z）のX以上(DaiSyou=andOver)の範囲の点群を返す
            'Dim X_andOver = From X_aO As CT_Data In TempQ
            '             Where X_aO.CT_dat.lstRealP3d(0).X >= BasePoint.X

            ''（3）BasePoint（X,Y,Z）のX以下(DaiSyou=andUnder)の範囲の点群を返す
            'Dim X_andUnder = From X_aU As CT_Data In TempQ
            '             Where X_aU.CT_dat.lstRealP3d(0).X <= BasePoint.X

            ''（4）BasePoint（X,Y,Z）のXより小さい(DaiSyou=Under)範囲の点群を返す
            'Dim X_Under = From X_U As CT_Data In TempQ
            '             Where X_U.CT_dat.lstRealP3d(0).X < BasePoint.X

            ''（5）BasePoint（X,Y,Z）のYより大きい(DaiSyou=Over)範囲の点群を返す
            'Dim Y_Over = From Y_O As CT_Data In TempQ
            '             Where Y_O.CT_dat.lstRealP3d(0).Y > BasePoint.Y

            ''（6）BasePoint（X,Y,Z）のY以上(DaiSyou=andOver)の範囲の点群を返す
            'Dim Y_andOver = From Y_aO As CT_Data In TempQ
            '             Where Y_aO.CT_dat.lstRealP3d(0).Y >= BasePoint.Y

            ''（7）BasePoint（X,Y,Z）のY以下(DaiSyou=andUnder)の範囲の点群を返す
            'Dim Y_andUnder = From Y_aU As CT_Data In TempQ
            '             Where Y_aU.CT_dat.lstRealP3d(0).Y <= BasePoint.Y

            ''（8）BasePoint（X,Y,Z）のYより小さい(DaiSyou=Under)の範囲の点群を返す
            'Dim Y_Under = From Y_U As CT_Data In TempQ
            '             Where Y_U.CT_dat.lstRealP3d(0).Y < BasePoint.Y

            ''（9）BasePoint（X,Y,Z）のZより大きい(DaiSyou=Over)範囲の点群を返す
            'Dim Z_Over = From Z_O As CT_Data In TempQ
            '             Where Z_O.CT_dat.lstRealP3d(0).Z > BasePoint.Z

            ''（10）BasePoint（X,Y,Z）のZ以上(DaiSyou=andOver)の範囲の点群を返す
            'Dim Z_andOver = From Z_aO As CT_Data In TempQ
            '             Where Z_aO.CT_dat.lstRealP3d(0).Z >= BasePoint.Z

            ''（11）BasePoint（X,Y,Z）のZ以下(DaiSyou=andUnder)の範囲の点群を返す
            'Dim Z_andUnder = From Z_aU As CT_Data In TempQ
            '              Where Z_aU.CT_dat.lstRealP3d(0).Z <= BasePoint.Z

            ''（12）BasePoint（X,Y,Z）のZより小さい(DaiSyou=Under)の範囲の点群を返す
            'Dim Z_Under = From Z_U As CT_Data In TempQ
            '              Where Z_U.CT_dat.lstRealP3d(0).Z < BasePoint.Z

            '（1）BasePoint（X,Y,Z）のXより大きい範囲(DaiSyou=Over)の点群を返す
            Dim X_Over = From X_O As CT_Data In TempQ
                         Where X_O.CT_dat.lstRealP3d(0).X.D > BasePoint.X.D

            '（2）BasePoint（X,Y,Z）のX以上(DaiSyou=andOver)の範囲の点群を返す
            Dim X_andOver = From X_aO As CT_Data In TempQ
                         Where X_aO.CT_dat.lstRealP3d(0).X.D >= BasePoint.X.D

            '（3）BasePoint（X,Y,Z）のX以下(DaiSyou=andUnder)の範囲の点群を返す
            Dim X_andUnder = From X_aU As CT_Data In TempQ
                         Where X_aU.CT_dat.lstRealP3d(0).X.D <= BasePoint.X.D

            '（4）BasePoint（X,Y,Z）のXより小さい(DaiSyou=Under)範囲の点群を返す
            Dim X_Under = From X_U As CT_Data In TempQ
                         Where X_U.CT_dat.lstRealP3d(0).X.D < BasePoint.X.D

            '（5）BasePoint（X,Y,Z）のYより大きい(DaiSyou=Over)範囲の点群を返す
            Dim Y_Over = From Y_O As CT_Data In TempQ
                         Where Y_O.CT_dat.lstRealP3d(0).Y.D > BasePoint.Y.D

            '（6）BasePoint（X,Y,Z）のY以上(DaiSyou=andOver)の範囲の点群を返す
            Dim Y_andOver = From Y_aO As CT_Data In TempQ
                         Where Y_aO.CT_dat.lstRealP3d(0).Y.D >= BasePoint.Y.D

            '（7）BasePoint（X,Y,Z）のY以下(DaiSyou=andUnder)の範囲の点群を返す
            Dim Y_andUnder = From Y_aU As CT_Data In TempQ
                         Where Y_aU.CT_dat.lstRealP3d(0).Y.D <= BasePoint.Y.D

            '（8）BasePoint（X,Y,Z）のYより小さい(DaiSyou=Under)の範囲の点群を返す
            Dim Y_Under = From Y_U As CT_Data In TempQ
                         Where Y_U.CT_dat.lstRealP3d(0).Y.D < BasePoint.Y.D

            '（9）BasePoint（X,Y,Z）のZより大きい(DaiSyou=Over)範囲の点群を返す
            Dim Z_Over = From Z_O As CT_Data In TempQ
                         Where Z_O.CT_dat.lstRealP3d(0).Z.D > BasePoint.Z.D

            '（10）BasePoint（X,Y,Z）のZ以上(DaiSyou=andOver)の範囲の点群を返す
            Dim Z_andOver = From Z_aO As CT_Data In TempQ
                         Where Z_aO.CT_dat.lstRealP3d(0).Z.D >= BasePoint.Z.D

            '（11）BasePoint（X,Y,Z）のZ以下(DaiSyou=andUnder)の範囲の点群を返す
            Dim Z_andUnder = From Z_aU As CT_Data In TempQ
                          Where Z_aU.CT_dat.lstRealP3d(0).Z.D <= BasePoint.Z.D

            '（12）BasePoint（X,Y,Z）のZより小さい(DaiSyou=Under)の範囲の点群を返す
            Dim Z_Under = From Z_U As CT_Data In TempQ
                          Where Z_U.CT_dat.lstRealP3d(0).Z.D < BasePoint.Z.D



            Select Case XYZ
                Case XYZseibun.X 'X成分
                    Select Case DaiSyou
                        Case DaiSyou.Over
                            PointGroup = X_Over.ToArray '(1)Xより大きい範囲
                        Case DaiSyou.andOver
                            PointGroup = X_andOver.ToArray '(2)X以上の範囲
                        Case DaiSyou.andUnder
                            PointGroup = X_andUnder.ToArray '(3)X以下の範囲
                        Case DaiSyou.Under
                            PointGroup = X_Under.ToArray '(4)Xより小さい範囲
                    End Select

                Case XYZseibun.Y 'Y成分
                    Select Case DaiSyou
                        Case DaiSyou.Over
                            PointGroup = Y_Over.ToArray '(5)Yより大きい範囲
                        Case DaiSyou.andOver
                            PointGroup = Y_andOver.ToArray '(6)Y以上の範囲
                        Case DaiSyou.andUnder
                            PointGroup = Y_andUnder.ToArray '(7)Y以下の範囲
                        Case DaiSyou.Under
                            PointGroup = Y_Under.ToArray '(8)Yより小さい範囲
                    End Select


                Case XYZseibun.Z 'Z成分に注目
                    Select Case DaiSyou
                        Case DaiSyou.Over
                            PointGroup = Z_Over.ToArray '(9)Zより大きい範囲
                        Case DaiSyou.andOver
                            PointGroup = Z_andOver.ToArray '(10)Z以上の範囲
                        Case DaiSyou.andUnder
                            PointGroup = Z_andUnder.ToArray '(11)Z以下の範囲
                        Case DaiSyou.Under
                            PointGroup = Z_Under.ToArray '(12)Zより小さい範囲
                    End Select
            End Select
        Catch ex As Exception

        End Try
    End Sub
    '⑨-2 点群の指定したXYZ成分に対して、ある値より大きい or 小さい範囲にある点群を返す（フィルタリング）　H25.5.7 Yamada
    '=================================================================
    'b) CT_Data 1つ，CTのID(Integer)が入力された場合　-> IDをPoint3Dに変換し，a)へ

    '（入力）PointGroup：フィルタリング対象となる点群
    '        XYZ：成分の指定

    '        BaseID：フィルタリングの基準となるCTのID
    '        Daisyou：大小関係を指定

    '（入出力）PointGroup：フィルタリングした点群
    '（テスト）GetAreaPointGroupTest2()
    '=================================================================
    Public Sub GetAreaPointGroup(ByRef PointGroup() As CT_Data, ByVal XYZ As XYZseibun, ByVal BaseID As Integer, ByVal DaiSyou As DaiSyou)
        Try
            GetAreaPointGroup(PointGroup, XYZ, GetPoint_CTID(BaseID), DaiSyou)
        Catch ex As Exception

        End Try

    End Sub

    '⑨-3 点群の指定したXYZ成分に対して、ある値より大きい or 小さい範囲にある点群を返す（フィルタリング）　H25.5.7 Yamada
    '=================================================================
    'c) CT_Data 2つ，Point3Dが入力された場合


    '（入力）PointGroup：フィルタリング対象となる点群
    '        XYZ：成分の指定

    '        BasePoint：フィルタリングの基準となるPoint3D
    '        Daisyou：大小関係を指定

    '（入出力）FiltPointGroup：フィルタリングした点群
    '（テスト）GetAreaPointGroupTest3()
    '=================================================================
    Public Sub GetAreaPointGroup(ByVal PointGroup() As CT_Data, ByVal XYZ As XYZseibun, ByVal BasePoint As Point3D, ByVal DaiSyou As DaiSyou, ByRef FiltPointGroup() As CT_Data)
        Try
            GetAreaPointGroup(PointGroup, XYZ, BasePoint, DaiSyou)
            FiltPointGroup = PointGroup
        Catch ex As Exception

        End Try

    End Sub

    '⑨-4 点群の指定したXYZ成分に対して、ある値より大きい or 小さい範囲にある点群を返す（フィルタリング）　H25.5.7 Yamada
    '=================================================================
    'd) CT_Data 2つ，CTのID(Integer)が入力された場合　-> IDをPoint3Dに変換し，a)へ

    '（入力）PointGroup：フィルタリング対象となる点群
    '        XYZ：成分の指定

    '        BaseID：フィルタリングの基準となるCTのID
    '        Daisyou：大小関係を指定

    '（入出力）FiltPointGroup：フィルタリングした点群
    '（テスト）GetAreaPointGroupTest4()
    '=================================================================
    Public Sub GetAreaPointGroup(ByVal PointGroup() As CT_Data, _
                                 ByVal XYZ As XYZseibun, _
                                 ByVal BaseID As Integer, _
                                 ByVal DaiSyou As DaiSyou, _
                                 ByRef FiltPointGroup() As CT_Data)
        Try
            GetAreaPointGroup(PointGroup, XYZ, GetPoint_CTID(BaseID), DaiSyou) '⑨-3 へ
            FiltPointGroup = PointGroup
        Catch ex As Exception

        End Try

    End Sub
    '⑨-5 点群の指定したXYZ成分に対して、ある値より大きい or 小さい範囲にある点群を返す（フィルタリング）　H25.6.10 Yamada
    '=============================================================================================
    'a) CT_Data 1つ，Point3Dが入力された場合


    '（入力）PointGroup：フィルタリング対象となる点群
    '        XYZ：成分の指定

    '        BasePointGroup()：ファイルタリングの基準となる点群
    '        Daisyou：大小関係を指定

    '（入出力）PointGroup：フィルタリングした点群
    '（テスト）GetAreaPointGroupTest1()
    '=============================================================================================

    Public Sub GetAreaPointGroup(ByRef PointGroup() As CT_Data, _
                                 ByVal XYZ As XYZseibun, _
                                 ByVal BasePointGroup() As CT_Data, _
                                 ByVal DaiSyou As DaiSyou, _
                                 ByRef FiltPointGroup() As CT_Data)

        Dim BasePoint As New Point3D
        Dim TempBase = From Points As CT_Data In BasePointGroup
                       Where Points IsNot Nothing
        BasePointGroup = TempBase.ToArray
        If BasePointGroup.Length = 1 Then
            BasePoint = BasePointGroup(0).CT_dat.lstRealP3d.Item(0)
        Else
            Exit Sub
        End If

        Try
            GetAreaPointGroup(PointGroup, XYZ, BasePoint, DaiSyou, FiltPointGroup) '⑨-3 へ
        Catch ex As Exception

        End Try
    End Sub
    '⑩-1 3点から円を定義 H25.5.8 Yamada
    '=============================================================================================
    '※YCM_Command.vbのCommand_Circle3p（）のCcDrawCircle（）参考 
    '⇒算出した円の中心、半径、法線ベクトルをGeoCurveのm_center、m_dblRadius、m_normalに
    '⇒また、GeoCurveTypeEnum.geoCircle = 4(円)

    '（入力）Point1，Point2，Point3：Point3D
    '（出力）Circle3P：GeoCurve
    '=============================================================================================

    Public Sub GetCircle3P(ByVal Point1 As Point3D, ByVal Point2 As Point3D, ByVal Point3 As Point3D, ByRef Circle3P As GeoCurve)

        Try
            Dim P1, P2, P3 As New GeoPoint 'Point3D→GeoPoint
            P1.SetFBM_3DPoint(Point1)
            P2.SetFBM_3DPoint(Point2)
            P3.SetFBM_3DPoint(Point3)

            Circle3P = New GeoCurve
            Circle3P.CurveType = GeoCurveTypeEnum.geoCircle

            Dim r As Double = 0.0 '半径

            Dim Org As New GeoPoint '円の中心

            Dim vec As New GeoVector '法線ベクトル

            CcDrawCircle(P1, P2, P3, Org, r, vec) '入力（P1,P2,P3）⇒出力（Org, r, vec）


            Circle3P.center = Org 'm_center
            Circle3P.radius = r 'm_dblRadius
            Circle3P.normal = vec 'm_normal
        Catch ex As Exception

        End Try

    End Sub

    '⑩-2 3点から円を定義 H25.5.8 Yamada
    '=============================================================================================
    '※YCM_Command.vbのCommand_Circle3p（）のCcDrawCircle（）参考

    '⇒算出した円の中心、半径、法線ベクトルをGeoCurveのm_center、m_dblRadius、m_normalに
    '⇒また、GeoCurveTypeEnum.geoCircle = 4(円)

    '（入力）ID1，ID2，ID3：CTのID
    '（出力）Circle3P：GeoCurve
    '=============================================================================================

    Public Sub GetCircle3P(ByVal ID1 As Integer, ByVal ID2 As Integer, ByVal ID3 As Integer, ByRef Circle3P As GeoCurve)
        Try
            GetCircle3P(GetPoint_CTID(ID1), GetPoint_CTID(ID2), GetPoint_CTID(ID3), Circle3P)
        Catch ex As Exception

        End Try
    End Sub

    '⑪円と平面の最大距離・最小距離を算出する  H25.5.9 Yamada
    '=============================================================================================
    '（入力）Circle：円，Plane：平面，XYZ：距離を算出する際にXYZ成分を指定する

    '（出力）DistMax：円と平面の最大距離，DistMin：円と平面の最小距離

    '＜＜円と平面の位置関係について＞＞

    '（A）平行関係にある場合

    '　　：交線Lが見つからないので円の中心から平面までの距離を返す
    '（B）ある交線で交わる場合：

    '　　：交線Lから円の中心までのVectorを用いて、最大距離・最小距離
    '　　　となる円周上の2点を見つける（円と平面が垂直関係でもよし）

    '（C）直接交わり、かつ平面上に円の中心がある場合

    '　　：交線Lのベクトルと円の法線ベクトルの外積Vectorを用いて
    '　　 最大距離・最小距離となる円周上の2点を見つける（円と平面が垂直関係でもよし）

    '=============================================================================================

    Public Sub GetDistCircletoPlane(ByVal Circle As GeoCurve, ByVal Plane As GeoPlane, XYZ As XYZseibun, _
                                          ByRef DistMax As Double, ByRef DistMin As Double)

        If IsNothing(Circle) Then
            Exit Sub
        ElseIf IsNothing(Plane) Then
            Exit Sub
        ElseIf XYZ = 0 Then
            Exit Sub
        End If


        Dim P1 As New GeoPoint  '平面との距離が最大もしくは最小となる円周上の点
        Dim P2 As New GeoPoint '平面との距離が最大もしくは最小となる円周上の点
        Dim PlaneCircle As New GeoPlane '円から新たに作成する平面
        Dim Line As New GeoCurve '平面（円から作成）と平面（入力）の交線（interLine）

        Dim LSFoot As New GeoPoint '円の中心からLineへ下した垂線の足

        Dim LSVec As New GeoVector '垂線の足から円の中心へのベクトル
        Dim LrVec As New GeoVector 'LsVecに円の半径を掛けたベクトル

        Dim Dist1, Dist2 As New Double

        Dim C_Origin As New GeoPoint '入力円の中心座標

        Dim C_Normal As New GeoVector '入力円の法線ベクトル
        Dim C_r As Double '入力円の半径

        Dim dblTol As Double '許容精度（距離）

        dblTol = 0.0001

        C_Origin = Circle.center
        C_Normal = Circle.normal
        C_r = Circle.radius

        '（1）円から新たに平面を作成
        PlaneCircle.SetByOriginNormal(C_Origin, C_Normal)
        '（2）新たに作成した平面と入力平面との交線を探す

        If Plane.GetIntersectionWithPlane(Line, PlaneCircle) = False Then
            '（A）円と平面が平行関係にある場合：交線Lが見つからない

            DistMax = GetDist(C_Origin, Plane, XYZ)
            DistMin = DistMax
            Exit Sub
        End If
        '（3）円の中心から交線Lへ下した垂線の足を得る
        LSFoot = Line.GetPerpendicularFoot(C_Origin)
        'LSFootとC_Originが等価かどうか
        If LSFoot.IsEqualTo(C_Origin, dblTol) = False Then
            '等価ではない⇒（B）ある交線で交わる場合

            '（4）垂線の足から円の中心までベクトルを作成し、

            LSVec = New GeoVector(LSFoot, C_Origin)
            LrVec = LSVec.GetNormal.GetMul(C_r)
            '（5）円の中心からLSベクトル分移動した位置にP1を

            P1 = C_Origin.GetMoved(LrVec)
            '（6）逆側にP2を

            LrVec.Negate()
            P2 = C_Origin.GetMoved(LrVec)
        Else

            '等価である ⇒（C）直接交わり、かつ平面上に円の中心がある場合

            '（4）交線のベクトルLineVecと円の法線ベクトルの外積，からLSを作成
            Dim LineVec As New GeoVector(Line.StartPoint, Line.EndPoint) '交線Lのベクトル
            LSVec = LineVec.GetOuterProduct(C_Normal)
            LrVec = LSVec.GetNormal.GetMul(C_r)
            '（5）円の中心からLrベクトル分移動した位置にP1を

            P1 = C_Origin.GetMoved(LrVec)
            '（6）逆側にP2を

            LrVec.Negate()
            P2 = C_Origin.GetMoved(LrVec)
        End If
        '（7）最小距離・最大距離を算出

        Dist1 = GetDist(P1, Plane, XYZ)
        Dist2 = GetDist(P2, Plane, XYZ)
        Try
            If Dist1 > Dist2 Then
                DistMax = Dist1
                DistMin = Dist2
            Else
                DistMax = Dist2
                DistMin = Dist1
            End If
        Catch ex As Exception

        End Try
    End Sub

    '⑫-1 点群に対する近似面を定義する  H25.5.10 Yamada
    '=============================================================================================
    '（入力）arrPoint()：点群（Point3D）

    '（出力）KinjiPlane：点群の近似面（GeoPlane）

    '（テスト）KinjiPlane_PointGroup_Test1()，ProjectPointGroup_Test2()，ProjectPointGroup_Test3()
    'MenshinSy用の計算()を参考

    '=============================================================================================

    Public Sub KinjiPlane_PointGroup(ByVal arrPoint() As Point3D, ByRef KinjiPlane As GeoPlane)
        Try
            '（1）入力Point3DをTuple型に（HALCON）＜Point3DとGeoPoint＞


            Dim X As New FBMlib.Point3D
            '  Dim vecX(arrPoint.Length - 1) As GeoPoint
            Dim i As Integer

            For i = 0 To arrPoint.Length - 1
                X.ConcatToMe(arrPoint(i))
                'vecX(i).SetFBM_3DPoint(arrPoint(i))
            Next

            Dim meanX As New FBMlib.Point3D 'Xの平均

            meanX = X.GetMean() '=(meanX,MeanY,MeanZ)
            Dim X_minus_meanX As New FBMlib.Point3D
            X_minus_meanX = X.SubPoint3d(meanX) '=X-meanX
            Dim A1 As New FBMlib.Point3D(X_minus_meanX.GetMultedByTuple(X_minus_meanX.X)) '=(X-meanX)^2
            Dim A2 As New FBMlib.Point3D(X_minus_meanX.GetMultedByTuple(X_minus_meanX.Y))
            Dim A3 As New FBMlib.Point3D(X_minus_meanX.GetMultedByTuple(X_minus_meanX.Z))
            A1 = A1.GetSummary()
            A2 = A2.GetSummary()
            A3 = A3.GetSummary()
            Dim CovMatrixVal(8) As Double
            Dim CovMatrixID As New Object
            Dim EigenValID As New Object
            Dim EigenValMat As New Object
            Dim EigenVectorID As New Object
            Dim EigenVectorMat As New Object
            CovMatrixVal(0) = A1.X
            CovMatrixVal(1) = A1.Y
            CovMatrixVal(2) = A1.Z
            CovMatrixVal(3) = A2.X
            CovMatrixVal(4) = A2.Y
            CovMatrixVal(5) = A2.Z
            CovMatrixVal(6) = A3.X
            CovMatrixVal(7) = A3.Y
            CovMatrixVal(8) = A3.Z
            HOperatorSet.CreateMatrix(3, 3, CovMatrixVal, CovMatrixID)
            HOperatorSet.EigenvaluesSymmetricMatrix(CovMatrixID, "true", EigenValID, EigenVectorID)
            HOperatorSet.GetFullMatrix(EigenValID, EigenValMat)
            HOperatorSet.GetFullMatrix(EigenVectorID, EigenVectorMat)
            Dim BestPlaneNormalVector As New FBMlib.Point3D(EigenVectorMat(0), EigenVectorMat(3), EigenVectorMat(6))
            Dim Origin As New GeoPoint
            Origin.setXYZ(meanX.X, meanX.Y, meanX.Z)
            Dim BPnormalvector As New GeoVector
            BPnormalvector.setXYZ(BestPlaneNormalVector.X, BestPlaneNormalVector.Y, BestPlaneNormalVector.Z)
            KinjiPlane.SetByOriginNormal(Origin, BPnormalvector) '近似面をOrigin，BPnormalVectorから作成

        Catch ex As Exception

        End Try

    End Sub

    '⑫-2 点群に対する近似面を定義する

    Public Sub KinjiPlane_PointGroup(ByVal arrCTPoint() As CT_Data, ByRef KinjiPlane As GeoPlane)

    End Sub

    '⑬点群をある指定した平面へ投影する     H25.5.10 Yamada
    '=============================================================================================
    '（入力）arrPoint()：点群（Point3D）

    '　　　  Plane：点群を投影する平面（GeoPlane）

    '（出力）ProjectPntG()：平面へ投影した点群（Point3D）

    '（テスト）ProjectPointGroup_Test1()
    '=============================================================================================

    Public Sub ProjectPointGroup(ByVal arrPoint() As Point3D, ByVal Plane As GeoPlane, ByRef ProjectPntG() As Point3D)
        Try
            Dim VLine As New GeoCurve '点arrPoint(i)から平面の法線ベクトル分移動した点とで結ぶCurve
            Dim InterPoint As New GeoPoint '点から平面へ下した垂線の足
            Dim Normal As GeoVector = Plane.normal '平面の法線ベクトル

            For i As Integer = 0 To arrPoint.Length - 1
                VLine = New GeoCurve
                InterPoint = New GeoPoint
                ProjectPntG(i) = New Point3D

                VLine.StartPoint.SetFBM_3DPoint(arrPoint(i))
                VLine.EndPoint = VLine.StartPoint.GetMoved(Normal)
                InterPoint = Plane.GetIntersectionWithCurve(VLine, True)
                ProjectPntG(i) = InterPoint.GetP3D()
            Next
        Catch ex As Exception

        End Try
    End Sub
    '⑭同一平面上の点群に対して近似円を定義する
    '=============================================================================================
    '（入力）arrPoint()：点群（Point3D）

    '（出力）circle：近似円（GeoCurve）

    '=============================================================================================

    Public Sub GetCirclePointGroup(ByVal arrPoint() As CT_Data, ByRef circle As GeoCurve)
        ' Dim
        circle = New GeoCurve
        circle.CurveType = GeoCurveTypeEnum.geoCircle
        Dim KinjiPl As New GeoPlane
        Dim tmpArrPoint(arrPoint.Length - 1) As Point3D
        Dim inputArrPoint(arrPoint.Length - 1) As Point3D
        Dim i As Integer
        For i = 0 To arrPoint.Length - 1
            inputArrPoint(i) = arrPoint(i).CT_dat.lstRealP3d(0)
        Next

        KinjiPlane_PointGroup(inputArrPoint, KinjiPl)
        ProjectPointGroup(inputArrPoint, KinjiPl, tmpArrPoint)

        Dim R As New FBMlib.Point3D
        '  Dim vecX(arrPoint.Length - 1) As GeoPoint

        For i = 0 To arrPoint.Length - 1
            R.ConcatToMe(tmpArrPoint(i))
        Next

        Dim KinjiMenPose As New Object
        Dim KinjiMenHomMat As New Object
        HOperatorSet.CreatePose(KinjiPl.origin.x, KinjiPl.origin.y, KinjiPl.origin.z,
                       KinjiPl.normal.x, KinjiPl.normal.y, KinjiPl.normal.z,
                       "Rp+T", "rodriguez", "coordinate_system", KinjiMenPose)
        HOperatorSet.PoseToHomMat3d(KinjiMenPose, KinjiMenHomMat)
        HOperatorSet.AffineTransPoint3d(KinjiMenHomMat, R.X, R.Y, R.Z, R.X, R.Y, R.Z)

        Dim tmpobjXLD As New HObject

        HOperatorSet.GenContourPolygonXld(tmpobjXLD, R.X, R.Y)
        HOperatorSet.ShapeTransXld(tmpobjXLD, tmpobjXLD, "convex")
        HOperatorSet.FitCircleContourXld(tmpobjXLD, "algebraic", -1, 0, 0, 3, 2, circle.center.x, circle.center.y, circle.radius, Nothing, Nothing, Nothing)
        HOperatorSet.HomMat3dInvert(KinjiMenHomMat, KinjiMenHomMat)
        HOperatorSet.AffineTransPoint3d(KinjiMenHomMat, circle.center.x, circle.center.y, 0.0, circle.center.x, circle.center.y, circle.center.z)
        circle.center.x = KinjiPl.origin.x / sys_ScaleInfo.scale
        circle.center.y = KinjiPl.origin.y / sys_ScaleInfo.scale
        circle.center.z = KinjiPl.origin.z / sys_ScaleInfo.scale
        circle.normal.x = KinjiPl.normal.x
        circle.normal.y = KinjiPl.normal.y
        circle.normal.z = KinjiPl.normal.z
        'AddUserCircle1P(New FBMlib.Point3D(circle.center.x, circle.center.y, circle.center.z), circle.radius, circle.normal)


        '    Dim object3dModelId As New Object
        '    Dim cylinder3dModelId As New Object
        '    HOperatorSet.GenObjectModel3DFromPoints(R.X, R.Y, R.Z, object3dModelId)
        '    Dim hv_ParFitting As New Object
        '    Dim hv_ValFitting As New Object

        '    hv_ParFitting = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat( _
        '"primitive_type", "fitting_algorithm"), "min_radius"), "max_radius"), "output_xyz_mapping")
        '    hv_ValFitting = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat( _
        '        "cylinder", "least_squares"), 0.01), 10000), "true")
        '    'Apply the fitting
        '    HOperatorSet.FitPrimitivesObjectModel3D(object3dModelId, hv_ParFitting, hv_ValFitting, _
        '        cylinder3dModelId)
        '    Dim kekkaPose As New Object
        '    HOperatorSet.GetObjectModel3DParams(cylinder3dModelId, "primitive_parameter", kekkaPose)
        '    circle.center.x = kekkaPose(0) / sys_ScaleInfo.scale
        '    circle.center.y = kekkaPose(1) / sys_ScaleInfo.scale
        '    circle.center.z = kekkaPose(2) / sys_ScaleInfo.scale
        '    circle.normal.x = kekkaPose(3)
        '    circle.normal.y = kekkaPose(4)
        '    circle.normal.z = kekkaPose(5)
        '    circle.radius = kekkaPose(6)
        '    AddUserCircle1P(New FBMlib.Point3D(circle.center.x, circle.center.y, circle.center.z), circle.radius, circle.normal)
        '    'Clear the object model that is no longer used
        '    Call HOperatorSet.ClearObjectModel3D(object3dModelId)
        '    Call HOperatorSet.ClearObjectModel3D(cylinder3dModelId)

    End Sub

    '⑰CT_DataからKubun1,Kubun2より、ある条件を満たすCTを抽出する      H25.5.13 Yamada
    '=============================================================================================
    '（入力）arrCT()：CTのarrayList(CT_Data)
    '　　　　K1：CT_Bunrui.CSVの”区分1”

    '　　　　K2：CT_Bunrui.CSVの”区分2”

    '（出力）arrCT_Result()：K1，K2を用いて抽出したCT_Data

    '※CT_Bunrui.CSV・・・C:\121126\YCM\bin\Debug
    '=============================================================================================

    Public Sub GetPoint_ByKubun(ByVal arrCT() As CT_Data, ByVal K1 As Integer, ByVal K2 As Integer, ByRef arrCT_Result() As CT_Data)
        If IsNothing(arrCT) Then
            Exit Sub
        ElseIf (K1 = Nothing Or K2 = Nothing) Then
            ' Exit Sub
        End If

        Try
            arrCT_Result = Nothing
            '空白を削除
            Dim TempCT = From CT As CT_Data In arrCT
                         Where CT IsNot Nothing
            Dim TempCTOffset = From CT As CT_Data In TempCT
                               Where CT.CToffsetval IsNot Nothing

            '（1）K1，K2両方を用いて抽出
            Dim FiltCT12 = From CT As CT_Data In TempCTOffset
                           Where CT.CToffsetval.kubun1 = K1
                           Where CT.CToffsetval.kubun2 = K2

            '（2）K1を用いて抽出
            Dim FiltCT1 = From CT As CT_Data In TempCTOffset
                          Where CT.CToffsetval.kubun1 = K1
            '（3）K2を用いて抽出
            Dim FiltCT2 = From CT As CT_Data In TempCTOffset
                          Where CT.CToffsetval.kubun2 = K2

            If K1 > 0 And K2 > 0 Then '（1）K1，K2ともに定義した数値である場合

                arrCT_Result = FiltCT12.ToArray
            ElseIf K1 > 0 And K2 < 1 Then '（2）K1は定義した数値、K2は定義した数値でない場合

                arrCT_Result = FiltCT1.ToArray
            ElseIf K1 < 1 And K2 > 0 Then '（3）K1は定義した数値でない、K2は定義した数値である場合

                arrCT_Result = FiltCT2.ToArray
            Else '（4）K1，K2ともに定義した数値でない場合

                arrCT_Result = TempCTOffset.ToArray
            End If

        Catch ex As Exception

        End Try

    End Sub

    '⑱CT_DataからIndex1～Index2までのCTを抽出する      H25.5.13 Yamada
    '=============================================================================================
    '（入力）arrCT()：CTのarrayList(CT_Data)
    '　　　　Index1：Integer
    '　　　　Index2：Integer
    '（出力）arrCT_Result()：CT_IDがIndex1～Index2のCTを抽出したCT_Data
    '=============================================================================================

    Public Sub GetPoint_ByIndex(ByVal arrCT() As CT_Data, ByVal Index1 As Integer, ByVal Index2 As Integer, ByRef arrCT_Result() As CT_Data)
        If IsNothing(arrCT) Then
            Exit Sub
        ElseIf (Index1 = Nothing Or Index2 = Nothing) Then
            Exit Sub
        End If

        Try
            arrCT_Result = Nothing
            Dim I1 As Integer = Index1 - 1 '開始Index
            Dim I2 As Integer = Index2 - 1 '終了Index
            '空白を削除
            Dim TempCT = From CT As CT_Data In arrCT
                         Where CT IsNot Nothing
            Dim TempCTOffset = From CT As CT_Data In TempCT
                               Where CT.CToffsetval IsNot Nothing

            arrCT = Nothing
            arrCT = TempCTOffset.ToArray

            If I1 < I2 Then

                If I2 > arrCT.Length Then
                    I2 = arrCT.Length - 1
                End If

                Dim N As Integer = I2 - I1
                ReDim arrCT_Result(N)
                Try
                    For i As Integer = I1 To I2
                        For K As Integer = 0 To N
                            Dim ObjCT As New CT_Data
                            arrCT_Result(K) = New CT_Data
                            ObjCT = arrCT(i)
                            arrCT_Result(K) = ObjCT
                            i += 1
                        Next
                    Next
                Catch ex As Exception
                    Exit Sub
                End Try


            ElseIf I1 > I2 Then
                arrCT_Result = Nothing
            ElseIf I1 = I2 Then
                '初期化がまで
                Dim ObjCT As New CT_Data
                ObjCT = arrCT(I1)
                ReDim arrCT_Result(0)
                Try
                    arrCT_Result(0) = New CT_Data
                    arrCT_Result(0) = ObjCT
                Catch ex As Exception
                    Exit Sub
                End Try

            End If
        Catch ex As Exception

        End Try

        ''（1）Index2 > Index1
        'Dim FiltCT1 = From CT As CT_Data In TempCTOffset
        '               Where CT.CToffsetval.CT_ID >= Index1
        '               Where CT.CToffsetval.CT_ID <= Index2
        ''（2）Index1 > Index2
        'Dim FiltCT2 = From CT As CT_Data In TempCTOffset
        '               Where CT.CToffsetval.CT_ID >= Index2
        '               Where CT.CToffsetval.CT_ID <= Index1
        ''（3）Index1 = Index2
        'Dim FiltCT12 = From CT As CT_Data In TempCTOffset
        '               Where CT.CToffsetval.CT_ID = Index1

        'If Index1 <> Index2 Then

        '    If Index2 > Index1 Then
        '        arrCT_Result = FiltCT1.ToArray
        '    Else
        '        arrCT_Result = FiltCT2.ToArray
        '    End If

        'ElseIf Index1 = Index2 Then
        '    arrCT_Result = FiltCT12.ToArray
        'Else
        '    arrCT_Result = TempCTOffset.ToArray
        'End If

    End Sub
    '⑲-1 線分と平面との交点を得る
    '=============================================================================================
    '；2点（P1，P2）からなる線分とある点（P3）を含む平面との交点
    '（入力）P1～P3：Point3D  P1とP2で線分を作成，P3は平面を定義する点
    '       XYZ：XYZseibun    P3とXYZで線分と交差する平面を定義
    '（入出力）P：線分（P1，P2で定義）と平面（P3とXYZで定義）の交差点
    '=============================================================================================

    Public Sub GetPointIntersection(ByVal P1 As Point3D, ByVal P2 As Point3D, ByVal P3 As Point3D, _
                                    ByVal XYZ As XYZseibun, ByRef P As Point3D)
        If (IsNothing(P1) Or IsNothing(P2) Or IsNothing(P3)) Then
            'MessageBox.Show("入力点がありません。", "エラー", _
            '                   MessageBoxButtons.OK, MessageBoxIcon.Error)

            Exit Sub
        End If

        Dim Normal As New GeoVector
        Dim Plane As New GeoPlane
        Dim Line As New GeoCurve
        Line.CurveType = GeoCurveTypeEnum.geoLine
        Dim Pnt1, Pnt2, Pnt3 As New GeoPoint
        Dim IntPnt As New GeoPoint

        Pnt1.SetFBM_3DPoint(P1)
        Pnt2.SetFBM_3DPoint(P2)
        Pnt3.SetFBM_3DPoint(P3)
        '（1）線分を定義
        Line.StartPoint = Pnt1
        Line.EndPoint = Pnt2
        '（2）平面の定義
        Select Case XYZ
            Case XYZseibun.XY
                Normal.x = 0.0
                Normal.y = 0.0
                Normal.z = 1.0
            Case XYZseibun.XZ
                Normal.x = 0.0
                Normal.y = 1.0
                Normal.z = 0.0
            Case XYZseibun.YZ
                Normal.x = 1.0
                Normal.y = 0.0
                Normal.z = 0.0
            Case Else
                MessageBox.Show("XY平面，XZ平面，YZ平面のいづれかを選択して下さい。", "エラー", _
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
        End Select
        Plane.SetByOriginNormal(Pnt3, Normal)
        '（3）平面と線分との交点を算出する
        Try
            IntPnt = Plane.GetIntersectionWithCurve(Line, True) 'H25.5.20 True/Falseの判定は現在未使用（Yamada）

            P = IntPnt.GetP3D()
        Catch ex As Exception
            MessageBox.Show("交差点が見つかりませんでした。", "エラー", _
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        '=======================================================================================
        'H25.5.20 平面と線分の交差判定が未完了（Yamada）

        '（1）1点にて交差する
        '（2）交差しない①（平行）

        '（3）交差しない②（線分が短い）

        '（3）交点は無数にある（線分は平面上にある）

        '=======================================================================================

    End Sub
    '⑲-2 線分と平面との交点を得る
    '=============================================================================================
    '；2点（P1，P2）からなる線分とある点（P3）を含む平面との交点
    '（入力）P1～P3：Point3D  P1とP2で線分を作成，P3は平面を定義する点
    '       XYZ：XYZseibun    P3とXYZで線分と交差する平面を定義
    '（入出力）P：線分（P1，P2で定義）と平面（P3とXYZで定義）の交差点
    '=============================================================================================

    Public Sub GetPointIntersection(ByVal ID1 As Integer, ByVal ID2 As Integer, ByVal ID3 As Integer, _
                                    ByVal XYZ As XYZseibun, ByRef P As Point3D)
        Try
            GetPointIntersection(GetPoint_CTID(ID1), GetPoint_CTID(ID2), GetPoint_CTID(ID3), XYZ, P)
        Catch ex As Exception

        End Try
    End Sub
    '⑳-1 2点を用いて新たな線のオブジェクトを追加する。

    '=============================================================================================
    '（入力）P1，P2：Point3D
    '（出力）gDrawUserLines()：CUserLineの配列。新たな要素を追加する。

    'Command_UserLine()参考

    '=============================================================================================
    Public Sub AddUserLineWithColorLine(ByVal P1 As Point3D, ByVal P2 As Point3D, ByVal colorcode As Integer, ByVal colortype As Integer)

        If (IsNothing(P1) Or IsNothing(P2)) Then
            Exit Sub
        End If


        '（1）2点をGeoPointに変換
        Dim GP1 As New GeoPoint
        Dim GP2 As New GeoPoint
        GP1.SetFBM_3DPoint(P1)
        GP2.SetFBM_3DPoint(P2)
        '（2）線分情報を新たに配列に追加
        ReDim Preserve gDrawUserLines(nUserLines)
        gDrawUserLines(nUserLines) = New CUserLine()
        gDrawUserLines(nUserLines).MID = nUserLines
        gDrawUserLines(nUserLines).blnDraw = True
        Call gDrawUserLines(nUserLines).SetStartPnt(GP1.x, GP1.y, GP1.z)
        Call gDrawUserLines(nUserLines).SetEndPnt(GP2.x, GP2.y, GP2.z)
        gDrawUserLines(nUserLines).lineTypeCode = colortype
        gDrawUserLines(nUserLines).colorCode = colorcode
        nUserLines = nUserLines + 1
    End Sub

    Public Sub AddUserLine(ByVal P1 As Point3D, ByVal P2 As Point3D)

        If (IsNothing(P1) Or IsNothing(P2)) Then
            Exit Sub
        End If


        '（1）2点をGeoPointに変換
        Dim GP1 As New GeoPoint
        Dim GP2 As New GeoPoint
        GP1.SetFBM_3DPoint(P1)
        GP2.SetFBM_3DPoint(P2)
        '（2）線分情報を新たに配列に追加
        ReDim Preserve gDrawUserLines(nUserLines)
        gDrawUserLines(nUserLines) = New CUserLine()
        gDrawUserLines(nUserLines).MID = nUserLines
        gDrawUserLines(nUserLines).blnDraw = True
        Call gDrawUserLines(nUserLines).SetStartPnt(GP1.x, GP1.y, GP1.z)
        Call gDrawUserLines(nUserLines).SetEndPnt(GP2.x, GP2.y, GP2.z)
        '20161124 baluu add
        gDrawUserLines(nUserLines).lineTypeCode = entset_line.linetype.code
        gDrawUserLines(nUserLines).colorCode = entset_line.color.code
        nUserLines = nUserLines + 1
    End Sub
    '⑳-1 2点を用いて新たな線のオブジェクトを追加する。

    '=============================================================================================
    '（入力）ID1，ID2：CTのID
    '（出力）gDrawUserLines()：CUserLineの配列。新たな要素を追加する。

    'Command_UserLine()参考

    '=============================================================================================

    Public Sub AddUserLine(ByVal ID1 As Integer, ByVal ID2 As Integer)
        Try
            AddUserLine(GetPoint_CTID_NoUnit(ID1), GetPoint_CTID_NoUnit(ID2))
        Catch ex As Exception

        End Try
    End Sub

    '（21）-1 3点を用いて新たな円のオブジェクトを追加する。

    '=============================================================================================
    '（入力）P1，P2，P3：CTのID
    '（出力）gDrawCircleNew()：Ccircleの配列。新たな要素を追加する。

    'Command_Circle3p()参考

    '=============================================================================================

    Public Sub AddUserCircle3P(ByVal P1 As Point3D, ByVal P2 As Point3D, P3 As Point3D)
        If (IsNothing(P1) Or IsNothing(P2) Or IsNothing(P3)) Then
            Exit Sub
        End If

        '（1）3点をGeoPointに変換
        Dim GP1 As New GeoPoint
        Dim GP2 As New GeoPoint
        Dim GP3 As New GeoPoint
        'GP1.SetFBM_3DPoint(GetPoint_CTID(ID1))
        'GP2.SetFBM_3DPoint(GetPoint_CTID(ID2))
        'GP3.SetFBM_3DPoint(GetPoint_CTID(ID3))

        GP1.SetFBM_3DPoint(P1)
        GP2.SetFBM_3DPoint(P2)
        GP3.SetFBM_3DPoint(P3)
        Dim r As Double = 0.0
        Dim Org As New GeoPoint
        Dim vec As New GeoVector
        Dim vecx As New GeoVector
        vecx.setXYZ(1.0, 0.0, 0.0)
        Dim Pi As Double = 3.1415926536
        Dim vecz As New GeoVector
        vecz.setXYZ(0, 0, -1.0)
        Dim vecy As New GeoVector
        vecy.setXYZ(0.0, 1.0, 0.0)
        Dim vecxtemp As New GeoVector
        Dim vecytemp As New GeoVector
        vecxtemp.x = vec.x
        vecxtemp.y = vec.y
        vecxtemp.z = vec.z

        '（2）3点から円の中心，半径，法線ベクトルを得る
        CcDrawCircle(GP1, GP2, GP3, Org, r, vec) '入力：GeoPoint3点/出力：円の中心Org，半径r，法線ベクトルvec

        vec.Normalize()
        vecytemp.x = 0.0
        vecytemp.y = vec.y
        vecytemp.z = vec.z
        vecytemp.Normalize()

        Dim vectemp As New GeoVector
        Dim double1 As Double
        Dim double2 As Double
        Dim double3 As Double

        '（3）円情報を新たに配列に追加
        ReDim Preserve gDrawCircleNew(nCircleNew)
        gDrawCircleNew(nCircleNew) = New Ccircle()
        gDrawCircleNew(nCircleNew).org = Org
        gDrawCircleNew(nCircleNew).r = r
        gDrawCircleNew(nCircleNew).mid = nCircleNew
        gDrawCircleNew(nCircleNew).blnDraw = True
        gDrawCircleNew(nCircleNew).Vec = vec

        vectemp = vec.GetOuterProduct(vecytemp)
        double1 = Math.Sqrt(vec.x * vec.x + vec.y * vec.y + vec.z * vec.z)
        double2 = Math.Sqrt(vec.y * vec.y + vec.z * vec.z)
        double3 = Math.Acos(double2 / double1)

        If vec.x > 0 And vec.y > 0 And vec.z > 0 Then
            gDrawCircleNew(nCircleNew).x_angle = vecytemp.GetSmallAngleTo(vecz) / Pi * 180
            gDrawCircleNew(nCircleNew).y_angle = -double3 / Pi * 180
        ElseIf vec.x > 0 And vec.y > 0 And vec.z < 0 Then
            gDrawCircleNew(nCircleNew).x_angle = vecytemp.GetSmallAngleTo(vecz) / Pi * 180
            gDrawCircleNew(nCircleNew).y_angle = -double3 / Pi * 180
        ElseIf vec.x > 0 And vec.y < 0 And vec.z > 0 Then
            gDrawCircleNew(nCircleNew).x_angle = vecytemp.GetSmallAngleTo(vecz) / Pi * 180
            gDrawCircleNew(nCircleNew).y_angle = double3 / Pi * 180
        ElseIf vec.x > 0 And vec.y < 0 And vec.z < 0 Then
            gDrawCircleNew(nCircleNew).x_angle = -vecytemp.GetSmallAngleTo(vecz) / Pi * 180
            gDrawCircleNew(nCircleNew).y_angle = -double3 / Pi * 180
        ElseIf vec.x < 0 And vec.y > 0 And vec.z > 0 Then
            gDrawCircleNew(nCircleNew).x_angle = vecytemp.GetSmallAngleTo(vecz) / Pi * 180
            gDrawCircleNew(nCircleNew).y_angle = double3 / Pi * 180
        ElseIf vec.x < 0 And vec.y > 0 And vec.z < 0 Then
            gDrawCircleNew(nCircleNew).x_angle = -vecytemp.GetSmallAngleTo(vecz) / Pi * 180
            gDrawCircleNew(nCircleNew).y_angle = -double3 / Pi * 180
        ElseIf vec.x < 0 And vec.y < 0 And vec.z > 0 Then
            gDrawCircleNew(nCircleNew).x_angle = -vecytemp.GetSmallAngleTo(vecz) / Pi * 180
            gDrawCircleNew(nCircleNew).y_angle = double3 / Pi * 180
        ElseIf vec.x < 0 And vec.y < 0 And vec.z < 0 Then
            gDrawCircleNew(nCircleNew).x_angle = -vecytemp.GetSmallAngleTo(vecz) / Pi * 180
            gDrawCircleNew(nCircleNew).y_angle = double3 / Pi * 180
        End If

        gDrawCircleNew(nCircleNew).lineTypeCode = entset_circle.linetype.code
        gDrawCircleNew(nCircleNew).colorCode = entset_circle.color.code
        gDrawCircleNew(nCircleNew).layerName = entset_circle.layerName
        nCircleNew = nCircleNew + 1
    End Sub

    '（21）-2 3点を用いて新たな円のオブジェクトを追加する。

    '=============================================================================================
    '（入力）ID1，ID2，ID3：CTのID
    '（出力）gDrawCircleNew()：Ccircleの配列。新たな要素を追加する。

    'Command_Circle3p()参考

    '=============================================================================================

    Public Sub AddUserCircle3P(ByVal ID1 As Integer, ByVal ID2 As Integer, ID3 As Integer)
        Try
            AddUserCircle3P(GetPoint_CTID_NoUnit(ID1), GetPoint_CTID_NoUnit(ID2), GetPoint_CTID_NoUnit(ID3))
        Catch ex As Exception

        End Try
    End Sub
    '（21）-1 1点を用いて新たな円のオブジェクトを追加する。

    '=============================================================================================
    '（入力）P1：Point3D，r：円の半径，Normal：円の法線ベクトル
    '（出力）gDrawCircleNew()：Ccircleの配列。新たな要素を追加する。

    '（Command_Circle1p()参考------異なる箇所；法線ベクトルの定義（angleではなくVecを用いる） 
    '=============================================================================================

    Public Sub AddUserCircle1P(ByVal P1 As Point3D, ByVal r As Double, ByVal Normal As GeoVector)
        If IsNothing(P1) Then
            Exit Sub
        ElseIf IsNothing(r) Then
            Exit Sub
        ElseIf IsNothing(Normal) Then
            Exit Sub
        End If

        '（1）GeoPointに変換
        Dim GP1 As New GeoPoint
        'GP1.SetFBM_3DPoint(GetPoint_CTID(ID1))
        GP1.SetFBM_3DPoint(P1)
        '（2）円情報を新たに配列に追加
        ReDim Preserve gDrawCircleNew(nCircleNew)
        gDrawCircleNew(nCircleNew) = New Ccircle()
        gDrawCircleNew(nCircleNew).org = GP1
        gDrawCircleNew(nCircleNew).Vec = Normal '-->x_angle,y_angleの代わり（H25.5.21 / Vecと●_angleはともに法線ベクトルを定義 Yamada）

        gDrawCircleNew(nCircleNew).r = r / sys_ScaleInfo.scale
        gDrawCircleNew(nCircleNew).mid = nCircleNew
        gDrawCircleNew(nCircleNew).blnDraw = True
        gDrawCircleNew(nCircleNew).colorCode = entset_circle.color.code
        gDrawCircleNew(nCircleNew).lineTypeCode = entset_circle.linetype.code
        gDrawCircleNew(nCircleNew).layerName = entset_circle.layerName
        nCircleNew = nCircleNew + 1
    End Sub
    '（21）-2 1点を用いて新たな円のオブジェクトを追加する。

    '=============================================================================================
    '（入力）ID1：CTのID，r：円の半径，Normal：円の法線ベクトル
    '（出力）gDrawCircleNew()：Ccircleの配列。新たな要素を追加する。

    '（Command_Circle1p()参考------異なる箇所；法線ベクトルの定義（angleではなくVecを用いる） 
    '=============================================================================================

    Public Sub AddUserCircle1P(ByVal ID1 As Integer, ByVal r As Double, ByVal Normal As GeoVector)
        Try
            AddUserCircle1P(GetPoint_CTID_NoUnit(ID1), r, Normal)
        Catch ex As Exception

        End Try
    End Sub
    '（22）二つの円の最小距離・最大距離となる円周上の4点を得る。

    '=============================================================================================
    '（入力）C1，C2：円1と円2・・・GeoCurve，

    '（出力）minPC1：最小距離となる円1の円周上の点
    '       maxPC1：最大距離となる円1の円周上の点
    '       minPC2：最小距離となる円2の円周上の点
    '       maxPC2：最大距離となる円2の円周上の点
    '=============================================================================================

    Public Sub GetMinMaxPointCToC(ByVal C1 As GeoCurve, ByVal C2 As GeoCurve, _
                                  ByRef minPC1 As Point3D, ByRef maxPC1 As Point3D, _
                                  ByRef minPC2 As Point3D, ByRef maxPC2 As Point3D)

        If (IsNothing(C1) Or IsNothing(C2)) Then
            Exit Sub
        End If

        '初期化

        minPC1 = New Point3D
        minPC2 = New Point3D
        maxPC1 = New Point3D
        maxPC2 = New Point3D
        C1.CurveType = GeoCurveTypeEnum.geoCircle
        C2.CurveType = GeoCurveTypeEnum.geoCircle
        Dim dblTol As Double '許容精度（距離）

        dblTol = 0.0001

        '（1）C1，C2を拡張した2平面（P1，P2）の交線lineを求める。

        Dim PlaneC1 As New GeoPlane '円C1から新たに作成する平面
        Dim PlaneC2 As New GeoPlane '円C2から新たに作成する平面
        Dim line As New GeoCurve '平面P1と平面P2の交線

        Dim OrgC1 As New GeoPoint '円C1の中心

        Dim OrgC2 As New GeoPoint '円C2の中心

        Dim NormalC1 As New GeoVector '円C1の法線ベクトル
        Dim NormalC2 As New GeoVector '円C2の法線ベクトル
        Dim rC1 As Double = 0.0 '円C1の半径

        Dim rC2 As Double = 0.0 '円C2の半径

        Dim P1_C1, P2_C1, P1_C2, P2_C2 As New GeoPoint '各円周上の任意の2点（P1，P2はMax点もしくはMin点）


        OrgC1 = C1.center
        OrgC2 = C2.center
        NormalC1 = C1.normal
        NormalC2 = C2.normal
        rC1 = C1.radius
        rC2 = C2.radius
        PlaneC1.SetByOriginNormal(OrgC1, NormalC1)
        PlaneC2.SetByOriginNormal(OrgC2, NormalC2)


        '（2）平面P1と平面P2の交線の有無により，各円周上の2点の求め方が変わる

        If PlaneC1.GetIntersectionWithPlane(line, PlaneC2) = False Then
            '==================================================================================
            '（A）円C1と円C2が平行関係にある場合：交線Lが見つからない

            '==================================================================================

            '円の法線ベクトルと任意のベクトルとの外積を用いて、

            '各円周上の任意な2点を決定し，その計4点を出力


            '円の法線ベクトルと任意ベクトルの外積

            Dim nVecC1 As New GeoVector 'NormalC1×任意ベクトル
            Dim nVecC2 As New GeoVector 'NormalC2×任意ベクトル
            '任意のベクトル（2種類）

            Dim tempVec1 As New GeoVector
            Dim tempVec2 As New GeoVector
            tempVec1.setXYZ(1.0, 0.0, 0.0)
            tempVec2.setXYZ(0.0, 0.0, 1.0)
            '円の法線ベクトルを単位ベクトルに
            NormalC1.GetNormal()
            NormalC2.GetNormal()

            '各円周上の任意な2点を決めるベクトルを求める

            '→円の法線ベクトルNormalC1と任意ベクトルtempVec1が平行かどうか判定

            If (NormalC1.GetIsParallelTo(tempVec1, dblTol)) Then
                '平行だった場合

                nVecC1 = NormalC1.GetOuterProduct(tempVec2)
            Else
                '平行ではない場合場合

                nVecC1 = NormalC1.GetOuterProduct(tempVec1)
            End If
            '→円の法線ベクトルNormalC2と任意ベクトルtempVec1が平行かどうか判定

            If (NormalC2.GetIsParallelTo(tempVec1, dblTol)) Then
                '平行だった場合

                nVecC2 = NormalC2.GetOuterProduct(tempVec2)
            Else
                '平行ではない場合場合

                nVecC2 = NormalC2.GetOuterProduct(tempVec1)
            End If
            'ベクトルの大きさを各円の半径に
            nVecC1 = nVecC1.GetNormal.GetMul(rC1)
            nVecC2 = nVecC2.GetNormal.GetMul(rC2)
            '円周上の2点を原点OrgC1，OrgC2よりベクトルnVecC1，nVecC2を用いて移動した点とする。

            'Dim P1_C1, P2_C1, P1_C2, P2_C2 As New GeoPoint
            P1_C1 = OrgC1.GetMoved(nVecC1) '円C1上の点1
            P1_C2 = OrgC2.GetMoved(nVecC2) '円C2上の点1
            nVecC1.Negate()
            nVecC2.Negate()
            P2_C1 = OrgC1.GetMoved(nVecC1) '円C2上の点2
            P2_C2 = OrgC2.GetMoved(nVecC2) '円C1上の点2
        Else
            '==================================================================================
            '（B）円C1と円C2が平行関係でない場合：交線Lは見つかる
            '==================================================================================
            '2各円の中心（O1，O2）から交線lineに下した垂線の足Q1，Q2を求める。

            Dim Q1 As New GeoPoint '円の中心からLineへ下した垂線の足
            Dim Q2 As New GeoPoint '円の中心からLineへ下した垂線の足
            Q1 = line.GetPerpendicularFoot(OrgC1)
            Q2 = line.GetPerpendicularFoot(OrgC2)
            'If Q1.IsEqualTo(Q2, dblTol) = True Then
            '    'Q1とQ2が等価である場合

            'End If
            'ベクトルV1(O1→Q1)，V2（O2→Q2），V3（Q1→Q2?）を定義する。

            Dim V1 As New GeoVector(OrgC1, Q1)
            Dim V2 As New GeoVector(OrgC2, Q2)
            Dim V3 As New GeoVector(Q1, Q2)
            'ベクトルVA（V1+V2+V3?），VB（O1→O2）を定義する。

            Dim VA As New GeoVector
            Dim VB As New GeoVector(OrgC1, OrgC2)
            VA = V1.GetAdded(V2.GetAdded(V3))
            'VA，VBの外積outVAVBを定義する。

            Dim outVAVB As New GeoVector
            outVAVB = VA.GetOuterProduct(VB)
            'outVAVBと各円の法線ベクトルNormalC1，NormalC2との外積を求める。

            Dim VO1, VO2 As New GeoVector
            VO1 = NormalC1.GetOuterProduct(outVAVB)
            VO2 = NormalC2.GetOuterProduct(outVAVB)
            VO1 = VO1.GetNormal.GetMul(rC1)
            VO2 = VO2.GetNormal.GetMul(rC2)
            '円周上の2点を原点OrgC1，OrgC2よりベクトルVO1，VO2を用いて移動した点とする。

            'Dim P1_C1, P2_C1, P1_C2, P2_C2 As New GeoPoint
            P1_C1 = OrgC1.GetMoved(VO1) '円C1上の点1
            P1_C2 = OrgC2.GetMoved(VO2) '円C2上の点1
            VO1.Negate()
            VO2.Negate()
            P2_C1 = OrgC1.GetMoved(VO1) '円C2上の点2
            P2_C2 = OrgC2.GetMoved(VO2) '円C1上の点2

            '====================================================================================
            ''（6）O1もしくはO2を原点，outVAVBを法線ベクトルとした平面（PlaneC1C2）を定義する。

            'Dim PlaneC1C2 As New GeoPlane
            'PlaneC1C2.SetByOriginNormal(OrgC1, outVAVB)

            ''（7）平面PlaneC1C2と交線lineの交点interPointを求める。

            'Dim interPoint As New GeoPoint
            'interPoint = PlaneC1C2.GetIntersectionWithCurve(line, True)

            ''（8）ベクトルVIO1（interPoint→O1），VIO2（interPoint→O2）

            'Dim ViO1 As New GeoVector(interPoint, OrgC1)
            'Dim ViO2 As New GeoVector(interPoint, OrgC2)
            'ViO1 = ViO1.GetNormal.GetMul(rC1)
            'ViO2 = ViO2.GetNormal.GetMul(rC2)

            ''（9）O1，O2をVIO1，VIO2を用いて半径r1，r2移動した点を定義する。

            ''Dim Pa_C1, Pb_C1, Pa_C2, Pb_C2 As New GeoPoint
            'Dim Pmax_C1, Pmin_C1, Pmax_C2, Pmin_C2 As New Point3D
            ''最大距離の点（仮定）

            'Pmax_C1 = OrgC1.GetMoved(ViO1).GetP3D()
            'Pmax_C2 = OrgC2.GetMoved(ViO2).GetP3D()
            'ViO1.GetNegative()
            'ViO2.GetNegative()
            ''最小距離の点（仮定）

            'Pmin_C1 = OrgC1.GetMoved(ViO1).GetP3D()
            'Pmin_C2 = OrgC2.GetMoved(ViO2).GetP3D()
            '====================================================================================
        End If

        '（3）円周上の各2点の決定

        '円C1上の点：P1_C1，P2_C1（StartPoint）

        '円C2上の点：P1_C2，P2_C2（EndPoint）

        '円C1の点と円C2の点を結ぶ線分を定義
        Dim Line1, Line2, Line3, Line4 As New GeoCurve
        Dim arrIntP1, arrIntP2 As New ArrayList
        Dim flgIntPOnLine As Boolean = False '（True：最大距離・最小距離の線分が交差する/False：交差しない）


        'Line1セット
        Line1.StartPoint = P1_C1
        Line1.EndPoint = P1_C2

        'Line2セット
        Line2.StartPoint = P1_C1
        Line2.EndPoint = P2_C2

        'Line3セット
        Line3.StartPoint = P2_C1
        Line3.EndPoint = P1_C2

        'Line4のセット()
        Line4.StartPoint = P2_C1
        Line4.EndPoint = P2_C2

        '線分同士が交差しているかの判定

        arrIntP1 = Line1.GetIntersection(Line4, False, False, dblTol) '①extendThis：True　直線として交点を探す，False　線分として交点を探す/②extendArg：未使用

        '交点が線分上にあるかの判定

        If (arrIntP1.Count = 1) Then
            flgIntPOnLine = True

            '交点が線分上にいるか調べる

            If (Line1.GetIsPointOnCurve(arrIntP1(0), dblTol)) = False Then 'True：交点は線分上にある/False：交点は線分上にない

                '交点は線分上にない場合

                flgIntPOnLine = False
            End If
            If (Line4.GetIsPointOnCurve(arrIntP1(0), dblTol)) = False Then 'True：交点は線分上にある/False：交点は線分上にない

                '交点は線分上にない場合

                flgIntPOnLine = False
            End If
        End If

        ''H25.5.27　コメントアウト　Yamada=======================================================
        'arrIntP2 = Line2.GetIntersection(Line3, False, False, dblTol)
        ''arrIntP2 = Line2.GetIntersection(Line3, True, True, dblTol)
        'If (arrIntP2.Count = 1) Then
        '    '交点が存在
        '    If (Line3.GetIsPointOnCurve(arrIntP2(0), dblTol)) = False Then 'True：線分上にある
        '        '交点は線分上にない場合

        '        arrIntP2 = Nothing
        '    End If
        'End If
        ''H25.5.27　コメントアウト　Yamada=======================================================
        Try
            If flgIntPOnLine = False Then '交点がない場合’If arrIntP1.Count = 0 Then
                Dim DistL1 As Double = GetDist(Line1.StartPoint.GetP3D, Line1.EndPoint.GetP3D, XYZseibun.XYZ)
                Dim DistL4 As Double = GetDist(Line4.StartPoint.GetP3D, Line4.EndPoint.GetP3D, XYZseibun.XYZ)
                If DistL1 > DistL4 Then
                    minPC1 = Line4.StartPoint.GetP3D
                    minPC2 = Line4.EndPoint.GetP3D
                    maxPC1 = Line1.StartPoint.GetP3D
                    maxPC2 = Line1.EndPoint.GetP3D
                Else
                    minPC1 = Line1.StartPoint.GetP3D
                    minPC2 = Line1.EndPoint.GetP3D
                    maxPC1 = Line4.StartPoint.GetP3D
                    maxPC2 = Line4.EndPoint.GetP3D
                End If

            Else '交点がある場合

                Dim DistL2 As Double = GetDist(Line2.StartPoint.GetP3D, Line2.EndPoint.GetP3D, XYZseibun.XYZ)
                Dim DistL3 As Double = GetDist(Line3.StartPoint.GetP3D, Line3.EndPoint.GetP3D, XYZseibun.XYZ)
                If DistL2 > DistL3 Then
                    minPC1 = Line3.StartPoint.GetP3D
                    minPC2 = Line3.EndPoint.GetP3D
                    maxPC1 = Line2.StartPoint.GetP3D
                    maxPC2 = Line2.EndPoint.GetP3D
                Else
                    minPC1 = Line2.StartPoint.GetP3D
                    minPC2 = Line2.EndPoint.GetP3D
                    maxPC1 = Line3.StartPoint.GetP3D
                    maxPC2 = Line3.EndPoint.GetP3D
                End If
            End If
        Catch ex As Exception

        End Try

        'flgIntPOnLine = False
    End Sub
    '（23）ターゲットのオフセット方向を指定し、その方向に該当するターゲットを得る
    '=============================================================================================
    '（入力）arrCT：CTのarrayList(CT_Data)
    '       XYZ：オフセットの成分
    '       OffsetVec：オフセットの方向


    '（出力）arrCT_Result：抽出したリスト

    '=============================================================================================

    Public Sub GetPoint_ByOffset(ByVal arrCT() As CT_Data, _
                                 ByVal XYZ As XYZseibun, _
                                 ByVal OffsetVec As SortMethod, _
                                 ByRef arrCT_Result() As CT_Data)

        Dim Vec_X As Double = 0.0
        Dim Vec_Y As Double = 0.0
        Dim Vec_Z As Double = 0.0

        If IsNothing(arrCT) Then
            Exit Sub
        ElseIf XYZ = XYZseibun.XY Or XYZ = XYZseibun.XZ Or XYZ = XYZseibun.YZ Or XYZ = XYZseibun.XYZ Then
            Exit Sub
        ElseIf IsNothing(OffsetVec) Then
            Exit Sub
        End If

        arrCT_Result = Nothing
        ReDim arrCT_Result(arrCT.Length - 1)



        Dim tempCT = From CT As CT_Data In arrCT
                     Where CT IsNot Nothing

        Dim tempCTOffset = From CT As CT_Data In tempCT
                           Where CT.CT_dat.OffsetV IsNot Nothing

        Dim tempPlusX = From CT As CT_Data In tempCTOffset
                        Where CT.CT_dat.OffsetV.X > 0 And
                        System.Math.Abs(CT.CT_dat.OffsetV.X) > System.Math.Abs(CT.CT_dat.OffsetV.Y) And
                        System.Math.Abs(CT.CT_dat.OffsetV.X) > System.Math.Abs(CT.CT_dat.OffsetV.Z)

        Dim tempMinusX = From CT As CT_Data In tempCTOffset
                         Where CT.CT_dat.OffsetV.X < 0 And
                         System.Math.Abs(CT.CT_dat.OffsetV.X) > System.Math.Abs(CT.CT_dat.OffsetV.Y) And
                         System.Math.Abs(CT.CT_dat.OffsetV.X) > System.Math.Abs(CT.CT_dat.OffsetV.Z)

        Dim tempPlusY = From CT As CT_Data In tempCTOffset
                        Where CT.CT_dat.OffsetV.Y > 0 And
                        System.Math.Abs(CT.CT_dat.OffsetV.Y) > System.Math.Abs(CT.CT_dat.OffsetV.X) And
                        System.Math.Abs(CT.CT_dat.OffsetV.Y) > System.Math.Abs(CT.CT_dat.OffsetV.Z)

        Dim tempMinusY = From CT As CT_Data In tempCTOffset
                         Where CT.CT_dat.OffsetV.Y < 0 And
                         System.Math.Abs(CT.CT_dat.OffsetV.Y) > System.Math.Abs(CT.CT_dat.OffsetV.X) And
                         System.Math.Abs(CT.CT_dat.OffsetV.Y) > System.Math.Abs(CT.CT_dat.OffsetV.Z)

        Dim tempPlusZ = From CT As CT_Data In tempCTOffset
                        Where CT.CT_dat.OffsetV.Z > 0 And
                        System.Math.Abs(CT.CT_dat.OffsetV.Z) > System.Math.Abs(CT.CT_dat.OffsetV.X) And
                        System.Math.Abs(CT.CT_dat.OffsetV.Z) > System.Math.Abs(CT.CT_dat.OffsetV.Y)

        Dim tempMinusZ = From CT As CT_Data In tempCTOffset
                         Where CT.CT_dat.OffsetV.Z < 0 And
                         System.Math.Abs(CT.CT_dat.OffsetV.Z) > System.Math.Abs(CT.CT_dat.OffsetV.X) And
                         System.Math.Abs(CT.CT_dat.OffsetV.Z) > System.Math.Abs(CT.CT_dat.OffsetV.Y)

        Select Case XYZ
            Case XYZseibun.X
                If OffsetVec = SortMethod.descending Then
                    arrCT_Result = tempPlusX.ToArray
                Else
                    arrCT_Result = tempMinusX.ToArray
                End If
            Case XYZseibun.Y
                If OffsetVec = SortMethod.descending Then
                    arrCT_Result = tempPlusY.ToArray
                Else
                    arrCT_Result = tempMinusY.ToArray
                End If

            Case XYZseibun.Z
                If OffsetVec = SortMethod.descending Then
                    arrCT_Result = tempPlusZ.ToArray
                Else
                    arrCT_Result = tempMinusZ.ToArray
                End If
        End Select

        '空白を削除
        Dim temp_RCT = From CT As CT_Data In arrCT_Result
                     Where CT IsNot Nothing
        arrCT_Result = temp_RCT.ToArray
    End Sub
    '（24）-1　軸を基準に点と直線から、距離および交点を求める　　　　H25.6.11 Yamada
    '=============================================================================================
    '（入力）line1：直線（GeoCurve）

    '       P1：点（Point3D）

    '       XYZ：XYZ成分
    '（出力）Dist：距離
    '       Point：line1上の点
    'H25.6.14要修正
    '=============================================================================================

    Public Sub GetDistPoint_ByJiku(ByVal line1 As GeoCurve, _
                                   ByVal P1 As Point3D, _
                                   ByVal XYZ As XYZseibun, _
                                   ByRef Dist As Double, _
                                   ByRef Point As Point3D)

        Dist = 0.0
        Point = New Point3D
        Dim dbtol As Double = 0.0001

        If IsNothing(line1) Then
            Exit Sub
        ElseIf IsNothing(P1) Then
            Exit Sub
        ElseIf IsNothing(XYZ) Then
            Exit Sub
        End If

        '（1）入力XYZ成分からベクトルを作成
        Dim gVec As New GeoVector
        Select Case XYZ
            Case XYZseibun.X
                gVec.setXYZ(1.0, 0.0, 0.0)
            Case XYZseibun.Y
                gVec.setXYZ(0.0, 1.0, 0.0)
            Case XYZseibun.Z
                gVec.setXYZ(0.0, 0.0, 1.0)
            Case Else
                Exit Sub
        End Select
        '（2）入力pをVec分移動した点を作成
        Dim gPoint As New GeoPoint
        Dim moveP As New GeoPoint
        gPoint.SetFBM_3DPoint(P1)
        moveP = gPoint.GetMoved(gVec)
        '（3）入力Pと移動点movePとでgLineを作成
        Dim gLine As New GeoCurve
        gLine.CurveType = GeoCurveTypeEnum.geoLine
        gLine.StartPoint = gPoint
        gLine.EndPoint = moveP
        '（4）入力lineと作成したgLineとの交点Pointを求める

        Dim arrIntP As New ArrayList
        arrIntP = line1.GetIntersection(gLine, True, True, dbtol)
        'extendThis：True　直線として交点を探す，False　線分として交点を探す

        'extendArg：未使用

        '交点が線分上にいるか調べる

        If (line1.GetIsPointOnCurve(arrIntP(0), dbtol)) = True Then 'True：交点は線分上にある/False：交点は線分上にない

            Point = arrIntP(0)
        Else
            Point = Nothing
            Exit Sub
        End If

        '（5）求めた交点Pointと入力点P1の距離を求める

        Dist = GetDist(P1, Point, XYZ)

    End Sub
    '（24）-2　軸を基準に点と直線から、距離および交点を求める  H25.6.11 Yamada
    '=============================================================================================
    '（入力）P1,P2：線分の端点（Point3D）

    '       P1：点（Point3D）

    '       XYZ：XYZ成分
    '（出力）Dist：距離
    '       Point：line1上の点
    'H25.6.14要修正
    '=============================================================================================

    Public Sub GetDistPoint_ByJiku(ByVal P1 As Point3D, _
                                   ByVal P2 As Point3D, _
                                   ByVal P3 As Point3D, _
                                   ByVal XYZ As XYZseibun, _
                                   ByRef Dist As Double, _
                                   ByRef Point As Point3D)

        Dist = 0.0
        Point = New Point3D

        If IsNothing(P1) Or IsNothing(P2) Or IsNothing(P3) Then
            Exit Sub
        ElseIf IsNothing(XYZ) Then
            Exit Sub
        End If

        Dim gLine1 As New GeoCurve
        gLine1.CurveType = GeoCurveTypeEnum.geoLine
        gLine1.StartPoint.SetFBM_3DPoint(P1)
        gLine1.EndPoint.SetFBM_3DPoint(P2)

        GetDistPoint_ByJiku(gLine1, P3, XYZ, Dist, Point)

    End Sub
    '（25）-1　面（XY,XZ,YZ）と線分の交点を求める　　H25.6.11 Yamada
    '=============================================================================================
    '（入力）Line1：直線（GeoCurve）

    '       P1：点（Point3D）

    '       XYZ：XYZ成分
    '（出力）Dist：距離
    '       Point：line1上の交点
    '=============================================================================================

    Public Sub GetPoint_ByPlane(ByVal Line1 As GeoCurve, _
                                ByVal P1 As Point3D, _
                                ByVal XYZ As XYZseibun, _
                                ByRef Point As Point3D)

        Point = New Point3D
        Dim dbtol As Double = 0.0001

        If IsNothing(Line1) Then
            Exit Sub
        ElseIf IsNothing(P1) Then
            Exit Sub
        ElseIf IsNothing(XYZ) Then
            Exit Sub
        ElseIf Line1.CurveType <> GeoCurveTypeEnum.geoLine Then
            Exit Sub
        End If

        '（1）入力XYZ成分からGeoPlaneを定義
        Dim gPlane As New GeoPlane
        gPlane.origin.SetFBM_3DPoint(P1)
        Dim gNormal As New GeoVector
        Select Case XYZ
            Case XYZseibun.XY
                gNormal.setXYZ(0.0, 0.0, 1.0)
            Case XYZseibun.XZ
                gNormal.setXYZ(0.0, 1.0, 0.0)
            Case XYZseibun.YZ
                gNormal.setXYZ(1.0, 0.0, 0.0)
            Case Else
                Exit Sub
        End Select
        gPlane.normal = gNormal

        '（2）入力Line1とGeoPlaneとの交点を求める

        Dim gIntP As New GeoPoint

        gIntP = gPlane.GetIntersectionWithCurve(Line1, False)
        'blnExtendCurveについて→True or False: 現在未使用

        'Curveは関数内に限り直線に拡張()

        ''H25.6.11 現在、線分Line1は直線に拡張する=============================================================
        ''交点が線分上にいるか調べる・・・・バトスーリさんと相談

        'If (Line1.GetIsPointOnCurve(gIntP, dbtol)) = True Then 'True：交点は線分上にある/False：交点は線分上にない

        '    Point = New Point3D(gIntP.x, gIntP.y, gIntP.z)
        'Else
        '    'Point = Nothing
        'End If
        ''H25.6.11 現在、線分Line1は直線に拡張する=============================================================

        Point = New Point3D(gIntP.x, gIntP.y, gIntP.z)

    End Sub
    '（25）-2　面（XY,XZ,YZ）と線分の交点を求める　　H25.6.11 Yamada
    '=============================================================================================
    '（入力）P1，P2：線分の端点(Point3D）

    '       P3：点（Point3D）

    '       XYZ：XYZ成分
    '（出力）Dist：距離
    '       Point：交点（Point3D）

    '=============================================================================================

    Public Sub GetPoint_ByPlane(ByVal P1 As Point3D, _
                                ByVal P2 As Point3D, _
                                ByVal P3 As Point3D, _
                                ByVal XYZ As XYZseibun, _
                                ByRef Point As Point3D)

        Point = New Point3D
        If IsNothing(P1) Or IsNothing(P2) Or IsNothing(P3) Then
            Exit Sub
        ElseIf IsNothing(XYZ) Then
            Exit Sub
        End If

        Dim gLine1 As New GeoCurve
        gLine1.CurveType = GeoCurveTypeEnum.geoLine
        gLine1.StartPoint.SetFBM_3DPoint(P1)
        gLine1.EndPoint.SetFBM_3DPoint(P2)

        Try
            GetPoint_ByPlane(gLine1, P3, XYZ, Point)
        Catch ex As Exception

        End Try
    End Sub
    '（26）-1　点群から作成した線分と点群の各点との距離を求める　　　H25.6.11 Yamada
    '=============================================================================================
    '（入力）Points()：点群の配列
    '       XYZ：XYZ成分
    '（出力）Dists()：各PointsとLineとの距離の配列
    ' ※入力値の点群はX、Y、Z軸に対し、降順もしくは昇順で並び替えらているものとする!      
    '=============================================================================================
    Public Sub GetDists_ByPointsLine(ByVal Points() As CT_Data, _
                                    ByVal XYZ As XYZseibun, _
                                    ByRef Dists() As Double)

        If IsNothing(Points) Or Points.Length <= 0 Then '20161102 baluu edit
            Exit Sub
        ElseIf IsNothing(XYZ) Then
            Exit Sub
        End If

        Dim n As Integer = Points.Length - 1
        Dim startP As New GeoPoint
        Dim endP As New GeoPoint
        startP.SetFBM_3DPoint(Points(0).CT_dat.lstRealP3d.Item(0))
        endP.SetFBM_3DPoint(Points(n).CT_dat.lstRealP3d.Item(0))

        '(1)点群より、第一要素と最終要素で線分を作成
        Dim gPointSLine As New GeoCurve
        gPointSLine.StartPoint = startP
        gPointSLine.EndPoint = endP

        ReDim Dists(n)
        For i As Integer = 0 To n
            Dists(i) = 0.0
        Next

        '(2)Dists()に点群の各点と線分gPointSLineとの距離を格納する

        For j As Integer = 0 To n
            Dists(j) = GetDist(Points(j).CT_dat.lstRealP3d.Item(0), gPointSLine, XYZ)
        Next j
    End Sub

    Public Sub TestSunpo() 'H25.4.15テスト確認のため（Yamada）

        ''①4.15GetPointのテスト===============================================================================

        'Dim ID1 As Integer = 55
        'Dim ID2 As Integer = 58
        'Dim ID3 As Integer = 56

        'Dim XYZ1 As XYZseibun = XYZseibun.X
        'Dim XYZ2 As XYZseibun = XYZseibun.Y
        'Dim XYZ3 As XYZseibun = XYZseibun.Z

        ''GetPoint(ID1, XYZ1, ID2, XYZ2, ID3, XYZ3)

        'GetPoint(ID1, XYZ1, ID2, XYZ2, Nothing, Nothing)

        ''①4.15GetPointのテスト===============================================================================



        '''②4.18GetSortedのテスト（1）;返すarryListは値がソーティングされたarrTempのみ===============================================================================
        'Dim arrTemp(500) As CT_Data

        'Dim XYZ As XYZseibun = XYZseibun.X 'ソーティングを行う項目（成分）

        'Dim SortHouhou As SortMethod = SortMethod.descending 'ソーティングの方法（降順 descending / 昇順 ascending ）


        ''arrCTData（既存のCT_Data）を新規のCT_Dataに
        ''arrTemp = arrCTData.Clone
        'Dim i As Integer = 0
        'For Each CTD As CT_Data In arrCTData
        '    arrTemp(i) = CTD
        '    i += 1
        'Next

        '''ソート前のリストをCSV出力確認

        ''Dim SortedListMonitor As String = ""
        ''For Each SortListItem As CT_Data In arrTemp
        ''    If SortListItem Is Nothing Then
        ''        SortedListMonitor = SortedListMonitor & vbNewLine
        ''        Continue For
        ''    End If
        ''    SortedListMonitor = SortedListMonitor & SortListItem.CT_dat.PID & "," & SortListItem.CT_dat.lstRealP3d(0).X & "," & SortListItem.CT_dat.lstRealP3d(0).Y & "," & SortListItem.CT_dat.lstRealP3d(0).Z & vbNewLine
        ''Next
        ''My.Computer.FileSystem.WriteAllText("C:\01 YCM Projects\KeihinTEST\SortMae.CSV", SortedListMonitor, False)


        ''arrTempをソーティング
        'GetSorted(arrTemp, XYZ, SortHouhou) 

        '''ソート後のリストをCSV出力確認（2）

        ''SortedListMonitor = ""
        ''For Each SortListItem As CT_Data In arrTemp
        ''    If SortListItem Is Nothing Then
        ''        SortedListMonitor = SortedListMonitor & vbNewLine
        ''        Continue For
        ''    End If
        ''    SortedListMonitor = SortedListMonitor & SortListItem.CT_dat.PID & "," & SortListItem.CT_dat.lstRealP3d(0).X & "," & SortListItem.CT_dat.lstRealP3d(0).Y & "," & SortListItem.CT_dat.lstRealP3d(0).Z & vbNewLine
        ''Next
        ''My.Computer.FileSystem.WriteAllText("C:\01 YCM Projects\KeihinTEST\Sorted.CSV", SortedListMonitor, False)

        '''②4.18GetSortedのテスト（1）;返すarryListは値がソーティングされたarrTempのみ===============================================================================


        '''②4.18GetSortedのテスト（1）;返すarryListは値がソーティングされたarrTempのみ===============================================================================
        'Dim arrTemp(500) As CT_Data

        'Dim XYZ As XYZseibun = XYZseibun.X 'ソーティングを行う項目（成分）

        'Dim SortHouhou As SortMethod = SortMethod.descending 'ソーティングの方法（降順 descending / 昇順 ascending ）


        ''arrCTData（既存のCT_Data）を新規のCT_Dataに
        ''arrTemp = arrCTData.Clone
        'Dim i As Integer = 0
        'For Each CTD As CT_Data In arrCTData
        '    arrTemp(i) = CTD
        '    i += 1
        'Next

        '''ソート前のリストをCSV出力確認

        ''Dim SortedListMonitor As String = ""
        ''For Each SortListItem As CT_Data In arrTemp
        ''    If SortListItem Is Nothing Then
        ''        SortedListMonitor = SortedListMonitor & vbNewLine
        ''        Continue For
        ''    End If
        ''    SortedListMonitor = SortedListMonitor & SortListItem.CT_dat.PID & "," & SortListItem.CT_dat.lstRealP3d(0).X & "," & SortListItem.CT_dat.lstRealP3d(0).Y & "," & SortListItem.CT_dat.lstRealP3d(0).Z & vbNewLine
        ''Next
        ''My.Computer.FileSystem.WriteAllText("C:\01 YCM Projects\KeihinTEST\SortMae.CSV", SortedListMonitor, False)


        ''arrTempをソーティング
        'GetSorted(arrTemp, XYZ, SortHouhou)

        '''ソート後のリストをCSV出力確認（2）

        ''SortedListMonitor = ""
        ''For Each SortListItem As CT_Data In arrTemp
        ''    If SortListItem Is Nothing Then
        ''        SortedListMonitor = SortedListMonitor & vbNewLine
        ''        Continue For
        ''    End If
        ''    SortedListMonitor = SortedListMonitor & SortListItem.CT_dat.PID & "," & SortListItem.CT_dat.lstRealP3d(0).X & "," & SortListItem.CT_dat.lstRealP3d(0).Y & "," & SortListItem.CT_dat.lstRealP3d(0).Z & vbNewLine
        ''Next
        ''My.Computer.FileSystem.WriteAllText("C:\01 YCM Projects\KeihinTEST\Sorted.CSV", SortedListMonitor, False)

        '''②4.18GetSortedのテスト（1）;返すarryListは値がソーティングされたarrTempのみ===============================================================================





        ''③4.18GetSortedのテスト（2）;返すarryListは値がソーティングされたarrTempのみ===============================================================================
        'Dim arrTemp1(500) As CT_Data
        'Dim arrTemp2(500) As CT_Data

        'Dim XYZ As XYZseibun = XYZseibun.Z 'ソーティングを行う項目（成分）

        'Dim SortHouhou As SortMethod = SortMethod.ascending     'ソーティングの方法（降順 descending / 昇順 ascending ）


        ''arrCTData（既存のCT_Data）を新規のCT_Dataに
        'arrTemp1 = arrCTData.Clone
        ''Dim i As Integer = 0
        ''For Each CTD As CT_Data In arrCTData
        ''    arrTemp1(i) = CTD
        ''    i += 1
        ''Next

        ''arrTemp1をCSV出力確認（1）：ソーティングされない方
        'Dim SortedListMonitor As String = ""
        'For Each SortListItem1 As CT_Data In arrTemp1
        '    If SortListItem1 Is Nothing Then
        '        SortedListMonitor = SortedListMonitor & vbNewLine
        '        Continue For
        '    End If
        '    SortedListMonitor = SortedListMonitor & SortListItem1.CT_dat.PID & "," & SortListItem1.CT_dat.lstRealP3d(0).X & "," & SortListItem1.CT_dat.lstRealP3d(0).Y & "," & SortListItem1.CT_dat.lstRealP3d(0).Z & vbNewLine
        'Next
        'My.Computer.FileSystem.WriteAllText("C:\01 YCM Projects\KeihinTEST\arrTemp1（実行前）.CSV", SortedListMonitor, False)


        ''arrTemp2をCSV出力確認（2）：ソーティングされた方
        'SortedListMonitor = ""
        'For Each SortListItem2 As CT_Data In arrTemp2
        '    If SortListItem2 Is Nothing Then
        '        SortedListMonitor = SortedListMonitor & vbNewLine
        '        Continue For
        '    End If
        '    SortedListMonitor = SortedListMonitor & SortListItem2.CT_dat.PID & "," & SortListItem2.CT_dat.lstRealP3d(0).X & "," & SortListItem2.CT_dat.lstRealP3d(0).Y & "," & SortListItem2.CT_dat.lstRealP3d(0).Z & vbNewLine
        'Next
        'My.Computer.FileSystem.WriteAllText("C:\01 YCM Projects\KeihinTEST\arrTemp2（実行前）.CSV", SortedListMonitor, False)



        ''（入力）arrTemp1：arrCTData，XYZ：ソーティングを行う成分，SortHouhou：降順か昇順，arrTemp2：Null
        ''（出力）arrTemp2　→　ソーティングされたarrCTData
        'GetSorted(arrTemp1, XYZ, SortHouhou, arrTemp2)



        ''arrTemp1をCSV出力確認（1）：ソーティングされない方
        'SortedListMonitor = ""
        'For Each SortListItem3 As CT_Data In arrTemp1
        '    If SortListItem3 Is Nothing Then
        '        SortedListMonitor = SortedListMonitor & vbNewLine
        '        Continue For
        '    End If
        '    SortedListMonitor = SortedListMonitor & SortListItem3.CT_dat.PID & "," & SortListItem3.CT_dat.lstRealP3d(0).X & "," & SortListItem3.CT_dat.lstRealP3d(0).Y & "," & SortListItem3.CT_dat.lstRealP3d(0).Z & vbNewLine
        'Next
        'My.Computer.FileSystem.WriteAllText("C:\01 YCM Projects\KeihinTEST\arrTemp1（実行後）.CSV", SortedListMonitor, False)


        ''arrTemp2をCSV出力確認（2）：ソーティングされた方
        'SortedListMonitor = ""
        'For Each SortListItem4 As CT_Data In arrTemp2
        '    If SortListItem4 Is Nothing Then
        '        SortedListMonitor = SortedListMonitor & vbNewLine
        '        Continue For
        '    End If
        '    SortedListMonitor = SortedListMonitor & SortListItem4.CT_dat.PID & "," & SortListItem4.CT_dat.lstRealP3d(0).X & "," & SortListItem4.CT_dat.lstRealP3d(0).Y & "," & SortListItem4.CT_dat.lstRealP3d(0).Z & vbNewLine
        'Next
        'My.Computer.FileSystem.WriteAllText("C:\01 YCM Projects\KeihinTEST\arrTemp2（実行後）.CSV", SortedListMonitor, False)


        ''③4.18GetSortedのテスト（1）;返すarryListは値がソーティングされたarrTempのみ===============================================================================


        '''④H25.4.22GetDistのテスト======================================================================================================================
        ''Dim ID1 As Integer = 1
        ''Dim ID2 As Integer = 3
        ''Dim ID3 As Integer = 4
        ''Dim ID0 As Integer = 30
        'Dim P1, P2, P3, P0 As New Point3D
        'Dim XYZ As XYZseibun = XYZseibun.XYZ

        'P1.X = 0
        'P1.Y = 0
        'P1.Z = 0

        'P2.X = 1
        'P2.Y = 0
        'P2.Z = 0

        'P3.X = 0
        'P3.Y = 1
        'P3.Z = 0

        'P0.X = 2
        'P0.Y = 2
        'P0.Z = 10


        'GetDist(P1, P2, P3, P0, XYZ)
        '''④H25.4.22GetDistのテスト======================================================================================================================
    End Sub

    '⑨H25.5.7GetAreaPointGroupのTest
    Public Sub TestSunpo1() 'H25.4.15テスト確認のため（Yamada）


        Dim PointGroup1(500) As CT_Data
        Dim PointGroup2(500) As CT_Data
        Dim PointGroup3(500) As CT_Data
        Dim PointGroup4(500) As CT_Data
        Dim XYZ As XYZseibun
        Dim DaiSyou1, Daisyou2, Daisyou3 As DaiSyou
        Dim Point As New Point3D

        PointGroup1 = arrCTData.Clone

        'XYZ = XYZseibun.X
        'XYZ = XYZseibun.Y
        'XYZ = XYZseibun.Z
        XYZ = XYZseibun.XYZ

        DaiSyou1 = DaiSyou.Over
        Daisyou2 = DaiSyou.Over
        Daisyou3 = DaiSyou.Over

        'DaiSyou = YCM_Sunpo.DaiSyou.andOver
        'DaiSyou = YCM_Sunpo.DaiSyou.andUnder
        'DaiSyou = YCM_Sunpo.DaiSyou.Under

        'Point = CTのID1(0)の座標値
        'Point.X = 881.59731651778839
        'Point.Y = 154.31403424869197
        'Point.Z = -161.00504862525315
        'Point.X = 50000000
        'Point.Y = 50000000
        'Point.Z = 50000000
        Point.X = 800
        Point.Y = 0
        Point.Z = 90



        'PointGroup1をCSV出力確認（1）：フィルタされないCT_Data
        Dim FilterMonitor As String = ""
        For Each GroupItem1 As CT_Data In PointGroup1
            If GroupItem1 Is Nothing Then
                FilterMonitor = FilterMonitor & vbNewLine
                Continue For
            End If
            FilterMonitor = FilterMonitor & GroupItem1.CT_dat.PID & "," & GroupItem1.CT_dat.lstRealP3d(0).X.D & "," & GroupItem1.CT_dat.lstRealP3d(0).Y.D & "," & GroupItem1.CT_dat.lstRealP3d(0).Z.D & vbNewLine
        Next
        My.Computer.FileSystem.WriteAllText("C:\Temp\2_ISO400_250\NonFilt（実行前）.CSV", FilterMonitor, False)


        'PointGroup2をCSV出力確認（2）：フィルタされるCT_Data
        FilterMonitor = ""
        For Each GroupItem2 As CT_Data In PointGroup2
            If GroupItem2 Is Nothing Then
                FilterMonitor = FilterMonitor & vbNewLine
                Continue For
            End If
            FilterMonitor = FilterMonitor & GroupItem2.CT_dat.PID & "," & GroupItem2.CT_dat.lstRealP3d(0).X.D & "," & GroupItem2.CT_dat.lstRealP3d(0).Y.D & "," & GroupItem2.CT_dat.lstRealP3d(0).Z.D & vbNewLine
        Next
        My.Computer.FileSystem.WriteAllText("C:\Temp\2_ISO400_250\Filt（実行前）.CSV", FilterMonitor, False)



        Select Case XYZ
            Case XYZseibun.X, XYZseibun.Y, XYZseibun.Z
                GetAreaPointGroup(PointGroup1, XYZ, Point, DaiSyou1, PointGroup2)

            Case XYZseibun.XY

                XYZ = XYZseibun.X
                GetAreaPointGroup(PointGroup1, XYZ, Point, DaiSyou1, PointGroup2)

                PointGroup3 = PointGroup2
                PointGroup2 = Nothing

                XYZ = XYZseibun.Y
                GetAreaPointGroup(PointGroup3, XYZ, Point, Daisyou2, PointGroup2)

            Case XYZseibun.XZ

                XYZ = XYZseibun.X
                GetAreaPointGroup(PointGroup1, XYZ, Point, DaiSyou1, PointGroup2)

                PointGroup3 = PointGroup2
                PointGroup2 = Nothing

                XYZ = XYZseibun.Z
                GetAreaPointGroup(PointGroup3, XYZ, Point, Daisyou3, PointGroup2)

            Case XYZseibun.YZ

                XYZ = XYZseibun.Y
                GetAreaPointGroup(PointGroup1, XYZ, Point, Daisyou2, PointGroup2)

                PointGroup3 = PointGroup2
                PointGroup2 = Nothing

                XYZ = XYZseibun.Z
                GetAreaPointGroup(PointGroup1, XYZ, Point, Daisyou3, PointGroup2)

            Case XYZseibun.XYZ

                XYZ = XYZseibun.X
                GetAreaPointGroup(PointGroup1, XYZ, Point, DaiSyou1, PointGroup2)

                PointGroup3 = PointGroup2
                PointGroup2 = Nothing

                XYZ = XYZseibun.Y
                GetAreaPointGroup(PointGroup3, XYZ, Point, Daisyou2, PointGroup2)

                PointGroup4 = PointGroup2
                PointGroup2 = Nothing

                XYZ = XYZseibun.Z
                GetAreaPointGroup(PointGroup4, XYZ, Point, Daisyou3, PointGroup2)

        End Select

        'GetAreaPointGroup(PointGroup1, XYZ, Point, DaiSyou, PointGroup2)

        'PointGroup1をCSV出力確認：フィルタされないCT_Data
        FilterMonitor = ""
        For Each GroupItem1 As CT_Data In PointGroup1
            If GroupItem1 Is Nothing Then
                FilterMonitor = FilterMonitor & vbNewLine
                Continue For
            End If
            FilterMonitor = FilterMonitor & GroupItem1.CT_dat.PID & "," & GroupItem1.CT_dat.lstRealP3d(0).X.D & "," & GroupItem1.CT_dat.lstRealP3d(0).Y.D & "," & GroupItem1.CT_dat.lstRealP3d(0).Z.D & vbNewLine
        Next
        My.Computer.FileSystem.WriteAllText("C:\Temp\2_ISO400_250\NonFilt（実行後）.CSV", FilterMonitor, False)


        'PointGroup2をCSV出力確認：フィルタされたCT_Data
        FilterMonitor = ""
        For Each GroupItem2 As CT_Data In PointGroup2
            If GroupItem2 Is Nothing Then
                FilterMonitor = FilterMonitor & vbNewLine
                Continue For
            End If
            FilterMonitor = FilterMonitor & GroupItem2.CT_dat.PID & "," & GroupItem2.CT_dat.lstRealP3d(0).X.D & "," & GroupItem2.CT_dat.lstRealP3d(0).Y.D & "," & GroupItem2.CT_dat.lstRealP3d(0).Z.D & vbNewLine
        Next
        My.Computer.FileSystem.WriteAllText("C:\Temp\2_ISO400_250\Filt（実行後）.CSV", FilterMonitor, False)

    End Sub

    '⑩H25.5.8のGetCircle3PのTest
    Public Sub TestSunpo2() 'H25.4.15テスト確認のため（Yamada）

        Dim P1, P2, P3 As New Point3D
        Dim Circle As New GeoCurve
        'Dim ID1, ID2, ID3 As Integer

        '3点が同一平面上

        'P1.X = 10
        'P1.Y = 0
        'P1.Z = 0
        'P2.X = 0
        'P2.Y = 10
        'P2.Z = 0
        'P3.X = -10
        'P3.Y = 0
        'P3.Z = 0

        '3点が同じ点
        P1.X = 1
        P1.Y = 0
        P1.Z = 0
        P2.X = 1
        P2.Y = 0
        P2.Z = 0
        P3.X = 1
        P3.Y = 0
        P3.Z = 0

        '共通モジュールの結果との比較

        'P1.X = -2.18816737291363
        'P1.Y = -0.182420032378784
        'P1.Z = 2.2142475908918
        'P2.X = -1.96226779900066
        'P2.Y = -0.618086283153542
        'P2.Z = 2.06302013739891
        'P3.X = -1.38071486414732
        'P3.Y = -0.376829557556083
        'P3.Z = 1.81984874429064

        'ID1 = 68
        'ID2 = 351
        'ID3 = 350

        GetCircle3P(P1, P2, P3, Circle)
        'GetCircle3P(ID1, ID2, ID3, Circle)

    End Sub

    '（1）P3から線分P1-P2に下した垂線の交点（Pfoot）のPoint3Dを作成
    '（2）P3からPfootまでの距離を算出

    '（入力）P1，P2：CTのPoint3D(線分の端点)
    '        P3：CTのPoint3D
    '　　　　XYZ：求めたい距離のXYZ成分
    '（出力）GetDistPtoL：算出した距離

    Public Function GetLineToPointNoFootP(ByVal P1 As Point3D, ByVal P2 As Point3D, ByVal P3 As Point3D) As Point3D
        GetLineToPointNoFootP = Nothing
        If (IsNothing(P1) Or IsNothing(P2) Or IsNothing(P3)) Then
            Exit Function
        End If
        Dim geoL As New GeoCurve '線分P1-P2
        Dim geoP3 As New GeoPoint 'P3
        Dim geo_PF As New GeoPoint '垂線の足（点geoP3から線分geoLへおろした垂線と線分geoLの交点）


        geoL.StartPoint.SetFBM_3DPoint(P1) '始点
        geoL.EndPoint.SetFBM_3DPoint(P2) '終点
        geoP3.SetFBM_3DPoint(P3)
        geo_PF = geoL.GetPerpendicularFoot(geoP3) '垂線の足（交点）

        'GetDistPtoL = geo_PF.GetDistanceTo(geoP3)'
        Dim Pfoot As New FBMlib.Point3D
        Pfoot.X = geo_PF.x
        Pfoot.Y = geo_PF.y
        Pfoot.Z = geo_PF.z

        'P3とPfoot（垂線の足）の2点間の距離を算出⇒②へ
        Try
            Return Pfoot
        Catch ex As Exception 'セルがブランクだった場合

            Return Nothing
        End Try
    End Function
    'H25.7.5 Yamada　エラーが発生した関数をテキストファイルに出力する

    '（入力）errorFunc：


    Public Sub errorFunctionMonitor(ByVal errorFunc As String)
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\YCM_Sunpo_ErrorFunctionMonitor.txt",
                                                vbNewLine & "●日時： " & Now & vbNewLine & "→エラー発生：" & errorFunc, True)
    End Sub

End Module
