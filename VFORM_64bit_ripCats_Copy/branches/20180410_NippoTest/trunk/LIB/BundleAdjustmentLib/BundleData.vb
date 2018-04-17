Public Class BundleData

    Public CamObj As New CameraParam
    Public FirstCamObj As New CameraParam
    Public WorldPointObj As New WorldPoint
    Public ImgObj As New ImagePoint
    Public ImageIndex As Integer
    Public PointIndex As Integer
 
    Private Nx As Double
    Private Ny As Double
    Private D As Double
    Private D_pow_2 As Double
    Private X_sub_X0 As Double
    Private Y_sub_Y0 As Double
    Private Z_sub_Z0 As Double
    Private x_sub_xp As Double
    Private y_sub_yp As Double
    Private x_sub_xp2 As Double
    Private y_sub_yp2 As Double
    Private r As Double
    Private r2 As Double
    Private r4 As Double
    Private r6 As Double
    Private b1_11 As Double
    Private b1_12 As Double
    Private b1_13 As Double
    Private b1_14 As Double
    Private b1_15 As Double
    Private b1_16 As Double
    Private b1_21 As Double
    Private b1_22 As Double
    Private b1_23 As Double
    Private b1_24 As Double
    Private b1_25 As Double
    Private b1_26 As Double

    Private b2_11 As Double
    Private b2_12 As Double
    Private b2_13 As Double
    Private b2_14 As Double
    Private b2_15 As Double
    Private b2_16 As Double
    Private b2_17 As Double
    Private b2_18 As Double
    Private b2_21 As Double
    Private b2_22 As Double
    Private b2_23 As Double
    Private b2_24 As Double
    Private b2_25 As Double
    Private b2_26 As Double
    Private b2_27 As Double
    Private b2_28 As Double

    Private b3_11 As Double
    Private b3_12 As Double
    Private b3_13 As Double
    Private b3_21 As Double
    Private b3_22 As Double
    Private b3_23 As Double

    Private e1 As Double
    Private e2 As Double

    Public Sub Refresh()
        CamObj.CalcM()
        CalcCommonValue()

    End Sub
    Private Sub CalcCommonValue()
        X_sub_X0 = WorldPointObj.X - CamObj.X0
        Y_sub_Y0 = WorldPointObj.Y - CamObj.Y0
        Z_sub_Z0 = WorldPointObj.Z - CamObj.Z0
        Nx = CamObj.m11 * X_sub_X0 + CamObj.m12 * Y_sub_Y0 + CamObj.m13 * Z_sub_Z0
        Ny = CamObj.m21 * X_sub_X0 + CamObj.m22 * Y_sub_Y0 + CamObj.m23 * Z_sub_Z0
        D = CamObj.m31 * X_sub_X0 + CamObj.m32 * Y_sub_Y0 + CamObj.m33 * Z_sub_Z0
        D_pow_2 = D * D
        x_sub_xp = ImgObj.U - CamObj.xp
        y_sub_yp = ImgObj.V - CamObj.yp
        x_sub_xp2 = x_sub_xp * x_sub_xp
        y_sub_yp2 = y_sub_yp * y_sub_yp
        r2 = x_sub_xp * x_sub_xp + y_sub_yp * y_sub_yp
        r = Math.Sqrt(r2)
        r4 = r2 * r2
        r6 = r4 * r2

    End Sub
    Private Function GenAMatrix() As Object
        GenAMatrix = Nothing


        Return GenAMatrix

    End Function

    Public Function GenB1Matrix() As Object
        Dim B1MatId As New Object
        Dim MatVal As New Object

        With CamObj
            b1_11 = (.c * ((-1) * .m11 * D + .m31 * Nx)) / D_pow_2
            b1_12 = (.c * ((-1) * .m12 * D + .m32 * Nx)) / D_pow_2
            b1_13 = (.c * ((-1) * .m13 * D + .m33 * Nx)) / D_pow_2
            b1_14 = (.c / D) * (X_sub_X0 * .m11_d_a + Y_sub_Y0 * .m12_d_a + Z_sub_Z0 * .m13_d_a) - _
                    (.c * Nx / D_pow_2) * (X_sub_X0 * .m31_d_a + Y_sub_Y0 * .m32_d_a + Z_sub_Z0 * .m33_d_a)
            b1_15 = (.c / D) * (X_sub_X0 * .m11_d_b + Y_sub_Y0 * .m12_d_b + Z_sub_Z0 * .m13_d_b) - _
                    (.c * Nx / D_pow_2) * (X_sub_X0 * .m31_d_b + Y_sub_Y0 * .m32_d_b + Z_sub_Z0 * .m33_d_b)
            b1_16 = (.c / D) * (X_sub_X0 * .m11_d_g + Y_sub_Y0 * .m12_d_g + Z_sub_Z0 * .m13_d_g) - _
                    (.c * Nx / D_pow_2) * (X_sub_X0 * .m31_d_g + Y_sub_Y0 * .m32_d_g + Z_sub_Z0 * .m33_d_g)

            b1_21 = (.c * ((-1) * .m21 * D + .m31 * Ny)) / D_pow_2
            b1_22 = (.c * ((-1) * .m22 * D + .m32 * Ny)) / D_pow_2
            b1_23 = (.c * ((-1) * .m23 * D + .m33 * Ny)) / D_pow_2
            b1_24 = (.c / D) * (X_sub_X0 * .m21_d_a + Y_sub_Y0 * .m22_d_a + Z_sub_Z0 * .m23_d_a) - _
                    (.c * Ny / D_pow_2) * (X_sub_X0 * .m31_d_a + Y_sub_Y0 * .m32_d_a + Z_sub_Z0 * .m33_d_a)
            b1_25 = (.c / D) * (X_sub_X0 * .m21_d_b + Y_sub_Y0 * .m22_d_b + Z_sub_Z0 * .m23_d_b) - _
                    (.c * Ny / D_pow_2) * (X_sub_X0 * .m31_d_b + Y_sub_Y0 * .m32_d_b + Z_sub_Z0 * .m33_d_b)
            b1_26 = (.c / D) * (X_sub_X0 * .m21_d_g + Y_sub_Y0 * .m22_d_g + Z_sub_Z0 * .m23_d_g) - _
                    (.c * Ny / D_pow_2) * (X_sub_X0 * .m31_d_g + Y_sub_Y0 * .m32_d_g + Z_sub_Z0 * .m33_d_g)
            MatVal = Tuple.TupleGenConst(0, 0)
            MatVal = Tuple.TupleConcat(MatVal, b1_11)
            MatVal = Tuple.TupleConcat(MatVal, b1_12)
            MatVal = Tuple.TupleConcat(MatVal, b1_13)
            MatVal = Tuple.TupleConcat(MatVal, b1_14)
            MatVal = Tuple.TupleConcat(MatVal, b1_15)
            MatVal = Tuple.TupleConcat(MatVal, b1_16)
            MatVal = Tuple.TupleConcat(MatVal, b1_21)
            MatVal = Tuple.TupleConcat(MatVal, b1_22)
            MatVal = Tuple.TupleConcat(MatVal, b1_23)
            MatVal = Tuple.TupleConcat(MatVal, b1_24)
            MatVal = Tuple.TupleConcat(MatVal, b1_25)
            MatVal = Tuple.TupleConcat(MatVal, b1_26)
            Op.CreateMatrix(2, 6, MatVal, B1MatId)
        End With
        Return B1MatId
    End Function
    Public Function GenB2Matrix() As Object
        Dim B2MatId As New Object
        Dim MatVal As New Object

        With CamObj
            b2_11 = (-1) * ((.k1 * r2 + .k2 * r4 + .k3 * r6) + 2 * .k1 * x_sub_xp2 + _
                    4 * .k2 * r2 * x_sub_xp2 + 6 * .k3 * r4 * x_sub_xp2 + _
                     6 * .p1 * x_sub_xp + 2 * .p2 * y_sub_yp + 1)
            b2_12 = (-2) * (.k1 * y_sub_yp * x_sub_xp + 2 * .k2 * r2 * x_sub_xp * y_sub_yp + 3 * .k3 * r4 * x_sub_xp * y_sub_yp + .p1 * y_sub_yp + .p2 * x_sub_xp)
            b2_13 = Nx / D
            b2_14 = r2 * x_sub_xp
            b2_15 = r4 * x_sub_xp
            b2_16 = r6 * x_sub_xp
            b2_17 = r2 + 2 * x_sub_xp2
            b2_18 = 2 * x_sub_xp * y_sub_yp
            b2_21 = (-2) * (.k1 * y_sub_yp * x_sub_xp + 2 * .k2 * r2 * x_sub_xp * y_sub_yp + 3 * .k3 * r4 * x_sub_xp * y_sub_yp + .p1 * y_sub_yp + .p2 * x_sub_xp)
            b2_22 = (-1) * ((.k1 * r2 + .k2 * r4 + .k3 * r6) + 2 * .k1 * y_sub_yp2 + _
                      4 * .k2 * r2 * y_sub_yp2 + 6 * .k3 * r4 * y_sub_yp2 + _
                        6 * .p2 * y_sub_yp + 2 * .p1 * x_sub_xp + 1)
            b2_23 = Ny / D
            b2_24 = r2 * y_sub_yp
            b2_25 = r4 * y_sub_yp
            b2_26 = r6 * y_sub_yp
            b2_27 = b2_18
            b2_28 = r2 + 2 * y_sub_yp2

            'b2_11 = 0


            'b2_12 = 0
            'b2_13 = 0
            'b2_14 = 0
            'b2_15 = 0
            'b2_16 = 0
            'b2_17 = 0
            'b2_18 = 0
            'b2_21 = 0
            'b2_22 = 0


            'b2_23 = 0
            'b2_24 = 0
            'b2_25 = 0
            'b2_26 = 0
            'b2_27 = 0
            'b2_28 = 0

            MatVal = Tuple.TupleGenConst(0, 0)
            MatVal = Tuple.TupleConcat(MatVal, b2_11)
            MatVal = Tuple.TupleConcat(MatVal, b2_12)
            MatVal = Tuple.TupleConcat(MatVal, b2_13)
            MatVal = Tuple.TupleConcat(MatVal, b2_14)
            MatVal = Tuple.TupleConcat(MatVal, b2_15)
            MatVal = Tuple.TupleConcat(MatVal, b2_16)
            MatVal = Tuple.TupleConcat(MatVal, b2_17)
            MatVal = Tuple.TupleConcat(MatVal, b2_18)
            MatVal = Tuple.TupleConcat(MatVal, b2_21)
            MatVal = Tuple.TupleConcat(MatVal, b2_22)
            MatVal = Tuple.TupleConcat(MatVal, b2_23)
            MatVal = Tuple.TupleConcat(MatVal, b2_24)
            MatVal = Tuple.TupleConcat(MatVal, b2_25)
            MatVal = Tuple.TupleConcat(MatVal, b2_26)
            MatVal = Tuple.TupleConcat(MatVal, b2_27)
            MatVal = Tuple.TupleConcat(MatVal, b2_28)
            Op.CreateMatrix(2, 8, MatVal, B2MatId)
        End With

        Return B2MatId

    End Function
    Public Function GenB3Matrix() As Object
        Dim B3MatId As New Object
        Dim MatVal As New Object

        b3_11 = (-1) * b1_11
        b3_12 = (-1) * b1_12
        b3_13 = (-1) * b1_13
        b3_21 = (-1) * b1_21
        b3_22 = (-1) * b1_22
        b3_23 = (-1) * b1_23
        MatVal = Tuple.TupleGenConst(0, 0)
        MatVal = Tuple.TupleConcat(MatVal, b3_11)
        MatVal = Tuple.TupleConcat(MatVal, b3_12)
        MatVal = Tuple.TupleConcat(MatVal, b3_13)
        MatVal = Tuple.TupleConcat(MatVal, b3_21)
        MatVal = Tuple.TupleConcat(MatVal, b3_22)
        MatVal = Tuple.TupleConcat(MatVal, b3_23)
        Op.CreateMatrix(2, 3, MatVal, B3MatId)
        Return B3MatId

    End Function

    Public Function GenEMatrix() As Object
        Dim EMatId As New Object
        Dim MatVal As New Object
        With CamObj
            e1 = (-1) * (x_sub_xp + (.k1 * r2 + .k2 * r4 + .k3 * r6) * x_sub_xp + .p1 * (r2 + 2 * x_sub_xp2) + 2 * .p2 * x_sub_xp * y_sub_yp + .c * (Nx / D))
            e2 = (-1) * (y_sub_yp + (.k1 * r2 + .k2 * r4 + .k3 * r6) * y_sub_yp + .p2 * (r2 + 2 * y_sub_yp2) + 2 * .p1 * x_sub_xp * y_sub_yp + .c * (Ny / D))
        End With
        MatVal = Tuple.TupleGenConst(0, 0)
        MatVal = Tuple.TupleConcat(MatVal, e1)
        MatVal = Tuple.TupleConcat(MatVal, e2)

        Op.CreateMatrix(2, 1, MatVal, EMatId)

        Return EMatId
    End Function
    Public Function GenE1Matrix() As Object
        Dim E1MatId As New Object
        Dim MatVal As New Object
        Dim t As Integer = 1
        With CamObj
            MatVal = Tuple.TupleGenConst(0, 0)
            MatVal = Tuple.TupleConcat(MatVal, t * .X0)
            MatVal = Tuple.TupleConcat(MatVal, t * .Y0)
            MatVal = Tuple.TupleConcat(MatVal, t * .Z0)
            MatVal = Tuple.TupleConcat(MatVal, t * .A0)
            MatVal = Tuple.TupleConcat(MatVal, t * .B0)
            MatVal = Tuple.TupleConcat(MatVal, t * .G0)
        End With
       
        Op.CreateMatrix(6, 1, MatVal, E1MatId)
        Return E1MatId

    End Function

    Public Function GenE2Matrix() As Object
        Dim E2MatId As New Object
        Dim MatVal As New Object
        Dim t As Integer = 1
        With CamObj
            MatVal = Tuple.TupleGenConst(0, 0)
            MatVal = Tuple.TupleConcat(MatVal, t * (.xp - FirstCamObj.xp))
            MatVal = Tuple.TupleConcat(MatVal, t * (.yp - FirstCamObj.yp))
            MatVal = Tuple.TupleConcat(MatVal, t * (.c - FirstCamObj.c))
            MatVal = Tuple.TupleConcat(MatVal, t * (.k1 - FirstCamObj.k1))
            MatVal = Tuple.TupleConcat(MatVal, t * (.k2 - FirstCamObj.k2))
            MatVal = Tuple.TupleConcat(MatVal, t * (.k3 - FirstCamObj.k3))
            MatVal = Tuple.TupleConcat(MatVal, t * (.p1 - FirstCamObj.p1))
            MatVal = Tuple.TupleConcat(MatVal, t * (.p2 - FirstCamObj.p2))
        End With

        Op.CreateMatrix(8, 1, MatVal, E2MatId)
        Return E2MatId

    End Function
   
End Class
