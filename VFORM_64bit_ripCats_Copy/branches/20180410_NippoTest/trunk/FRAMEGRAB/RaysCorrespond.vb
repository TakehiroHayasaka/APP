Module RaysCorrespond

    Private Class XYZseibun
        Public X1 As Double
        Public X2 As Double
        Public X3 As Double
        Public X4 As Double
        Public Y1 As Double
        Public Y2 As Double
        Public Y3 As Double
        Public Y4 As Double
        Public Z1 As Double
        Public Z2 As Double
        Public Z3 As Double
        Public Z4 As Double

        Public Sub New()
            X1 = 0
            X2 = 0
            X3 = 0
            X4 = 0
            Y1 = 0
            Y2 = 0
            Y3 = 0
            Y4 = 0
            Z1 = 0
            Z2 = 0
            Z3 = 0
            Z4 = 0
        End Sub

        Public Function CalcXYZ(ByRef XYZ As Point3D) As Boolean
            CalcXYZ = False
            Dim A(2, 2) As Double
            Dim B1(2, 2) As Double
            Dim B2(2, 2) As Double
            Dim B3(2, 2) As Double
            A(0, 0) = X1
            A(0, 1) = X2
            A(0, 2) = X3
            A(1, 0) = Y1
            A(1, 1) = Y2
            A(1, 2) = Y3
            A(2, 0) = Z1
            A(2, 1) = Z2
            A(2, 2) = Z3

            Dim D As Double
            D = DetM_3x3(A)
            If Math.Abs(D) < Double.Epsilon * 10000 Then
                Exit Function
            End If

            B1 = A.Clone
            B1(0, 0) = -X4
            B1(1, 0) = -Y4
            B1(2, 0) = -Z4
            B2 = A.Clone
            B2(0, 1) = -X4
            B2(1, 1) = -Y4
            B2(2, 1) = -Z4
            B3 = A.Clone
            B3(0, 2) = -X4
            B3(1, 2) = -Y4
            B3(2, 2) = -Z4

            XYZ.X = DetM_3x3(B1) / D
            XYZ.Y = DetM_3x3(B2) / D
            XYZ.Z = DetM_3x3(B3) / D
            CalcXYZ = True
        End Function

        Public Function DetM_3x3(ByVal A(,) As Double) As Double
            'a11a22a33+a12a23a31 + a13a32a21 - a31a22a13 - a11a23a32 - a21a12a33
            DetM_3x3 = A(0, 0) * A(1, 1) * A(2, 2) + A(0, 1) * A(1, 2) * A(2, 0) + A(0, 2) * A(2, 1) * A(1, 0) - _
                       A(2, 0) * A(1, 1) * A(0, 2) - A(0, 0) * A(1, 2) * A(2, 1) - A(1, 0) * A(0, 1) * A(2, 2)

        End Function


    End Class



    Private Sub CalcMatrixCoef(ByVal P1 As Point3D, ByVal P2 As Point3D, ByRef S As XYZseibun)
        If P1.X Is DBNull.Value Then
            Exit Sub
        End If
        Dim K As New XYZseibun
        Dim a1 As Double = P1.X
        Dim a2 As Double = P1.Y
        Dim a3 As Double = P1.Z
        Dim b1 As Double = P2.X
        Dim b2 As Double = P2.Y
        Dim b3 As Double = P2.Z
        Dim b1_a1 As Double = b1 - a1
        Dim b2_a2 As Double = b2 - a2
        Dim b3_a3 As Double = b3 - a3
        Dim b1_a1_2 As Double = b1_a1 * b1_a1
        Dim b2_a2_2 As Double = b2_a2 * b2_a2
        Dim b3_a3_2 As Double = b3_a3 * b3_a3
        Dim D As Double = b1_a1_2 + b2_a2_2 + b3_a3_2
        Dim T As Double = (-1) * (a1 * b1_a1 + a2 * b2_a2 + a3 * b3_a3)
        If D < Double.Epsilon * 10000 Then
            Exit Sub
        End If

        With K

            .X1 = b1_a1_2 / D - 1
            .X2 = (b1_a1 * b2_a2) / D
            .X3 = (b1_a1 * b3_a3) / D
            .X4 = (T * b1_a1) / D + a1
            .Y1 = b1_a1 * b2_a2 / D
            .Y2 = b2_a2_2 / D - 1
            .Y3 = b2_a2 * b3_a3 / D
            .Y4 = (T * b2_a2) / D + a2
            .Z1 = b1_a1 * b3_a3 / D
            .Z2 = b2_a2 * b3_a3 / D
            .Z3 = b3_a3_2 / D - 1
            .Z4 = (T * b3_a3) / D + a3

            S.X1 += .X1 * .X1 + .Y1 * .Y1 + .Z1 * .Z1
            S.X2 += .X1 * .X2 + .Y1 * .Y2 + .Z1 * .Z2
            S.X3 += .X1 * .X3 + .Y1 * .Y3 + .Z1 * .Z3
            S.X4 += .X1 * .X4 + .Y1 * .Y4 + .Z1 * .Z4
            S.Y1 += .X1 * .X2 + .Y1 * .Y2 + .Z1 * .Z2
            S.Y2 += .X2 * .X2 + .Y2 * .Y2 + .Z2 * .Z2
            S.Y3 += .X2 * .X3 + .Y2 * .Y3 + .Z2 * .Z3
            S.Y4 += .X2 * .X4 + .Y2 * .Y4 + .Z2 * .Z4
            S.Z1 += .X1 * .X3 + .Y1 * .Y3 + .Z1 * .Z3
            S.Z2 += .X2 * .X3 + .Y2 * .Y3 + .Z2 * .Z3
            S.Z3 += .X3 * .X3 + .Y3 * .Y3 + .Z3 * .Z3
            S.Z4 += .X3 * .X4 + .Y3 * .Y4 + .Z3 * .Z4
        End With

    End Sub

    Public Sub CalcTenRayDist(ByVal P1 As Point3D, ByVal P2 As Point3D, ByVal XYZ As Point3D, ByRef Dist As Object)
        If P1.X Is DBNull.Value Or XYZ.X Is DBNull.Value Then
            Exit Sub
        End If
        Dim B As Point3D = P2.SubPoint3d(P1)
        Dim P_sub_A As Point3D = XYZ.SubPoint3d(P1)
        Dim P_sub_A_scalar_B As Object = P_sub_A.ScalarPoint3d(B)
        Dim B_scalar_B As Object = B.ScalarPoint3d(B)
        Dim Sq As Object = Tuple.TupleDiv(P_sub_A_scalar_B, B_scalar_B)
        Dim Sq_x_B As Point3D = B.SetScale2(Sq)
        Dim Q As Point3D = P1.AddPoint3d(Sq_x_B)
        XYZ.GetDisttoOtherPose(Q, Dist)

    End Sub
    Public Sub CalcTenRayDistXYZ(ByVal P1 As Point3D, ByVal P2 As Point3D, ByVal XYZ As Point3D, ByRef dist As Object,
                                 ByRef deltaX As Object, ByRef deltaY As Object, ByRef deltaZ As Object)
        If P1.X Is DBNull.Value Or XYZ.X Is DBNull.Value Then
            Exit Sub
        End If
        Dim B As Point3D = P2.SubPoint3d(P1)
        Dim P_sub_A As Point3D = XYZ.SubPoint3d(P1)
        Dim P_sub_A_scalar_B As Object = P_sub_A.ScalarPoint3d(B)
        Dim B_scalar_B As Object = B.ScalarPoint3d(B)
        Dim Sq As Object = Tuple.TupleDiv(P_sub_A_scalar_B, B_scalar_B)
        Dim Sq_x_B As Point3D = B.SetScale2(Sq)
        Dim Q As Point3D = P1.AddPoint3d(Sq_x_B)
        deltaX = Tuple.TupleAbs(Tuple.TupleSub(XYZ.X, Q.X))
        deltaY = Tuple.TupleAbs(Tuple.TupleSub(XYZ.Y, Q.Y))
        deltaZ = Tuple.TupleAbs(Tuple.TupleSub(XYZ.Z, Q.Z))
        XYZ.GetDisttoOtherPose(Q, dist)

    End Sub
    Public Sub CalcNearest3dPointofRays(ByRef lstST As List(Of SingleTarget), ByRef XYZ As Point3D)
        Dim S As New XYZseibun
        For Each ST As SingleTarget In lstST
            CalcMatrixCoef(ST.RayP1, ST.RayP2, S)
        Next
        S.CalcXYZ(XYZ)
        Dim Dist As Object = Nothing
        For Each ST As SingleTarget In lstST
            CalcTenRayDist(ST.RayP1, ST.RayP2, XYZ, Dist)
            ST.Dist = Dist
        Next
    End Sub

    Public Function CalcNearest3dPointofRays(ByRef lstST As List(Of SingleTarget), ByRef XYZ As Point3D, ByRef allDist As Object) As Boolean
        CalcNearest3dPointofRays = False
        Dim S As New XYZseibun
        For Each ST As SingleTarget In lstST

            If Not ST.RayP1.X Is DBNull.Value Then
                CalcMatrixCoef(ST.RayP1, ST.RayP2, S)
            Else
                Dim t As Integer = 1
            End If
        Next


        Dim Dist As Object = Nothing
        allDist = System.DBNull.Value
        If S.CalcXYZ(XYZ) = False Then
            Exit Function
        End If
        For Each ST As SingleTarget In lstST
            If Not ST.RayP1.X Is DBNull.Value Then
                CalcTenRayDist(ST.RayP1, ST.RayP2, XYZ, Dist)
                ST.Dist = Dist
                allDist = Tuple.TupleConcat(allDist, Dist)
            Else
                Dim t As Integer = 1
            End If
            'If Dist > 0.1 Then
            '    allDist = Tuple.TupleConcat(allDist, Dist)
            'End If
        Next
        CalcNearest3dPointofRays = True
    End Function


End Module
