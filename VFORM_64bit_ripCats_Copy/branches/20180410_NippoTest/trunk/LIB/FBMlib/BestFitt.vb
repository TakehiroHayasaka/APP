Imports HalconDotNet

Module BestFitt

    Dim m11 As Double = 0
    Dim m12 As Double = 0
    Dim m13 As Double = 0
    Dim m21 As Double = 0
    Dim m22 As Double = 0
    Dim m23 As Double = 0
    Dim m31 As Double = 0
    Dim m32 As Double = 0
    Dim m33 As Double = 0
    Dim PX As Double = 0
    Dim PY As Double = 0
    Dim PZ As Double = 0
    Dim S As Double = 0

    Dim Pi As Object = Nothing
    Dim Qi As Object = Nothing
    Dim Ri As Object = Nothing
    Dim xi As Object = Nothing
    Dim yi As Object = Nothing
    Dim zi As Object = Nothing
    Dim X0i As Object = Nothing
    Dim Y0i As Object = Nothing
    Dim Z0i As Object = Nothing

    Dim dPi(13) As Object
    Dim dQi(13) As Object
    Dim dRi(13) As Object
    Dim dEi(12) As Double
    Dim ddEij(168) As Double

    Public Sub CalcBestHomMat_Scale(ByRef HomMat3d As Object, ByRef Scale As Object, ByRef Obj3d As Point3D, ByVal Mokuhyo3d As Point3D)

        xi = Obj3d.X
        yi = Obj3d.Y
        zi = Obj3d.Z
        X0i = Mokuhyo3d.X
        Y0i = Mokuhyo3d.Y
        Z0i = Mokuhyo3d.Z


        Dim i As Integer
        Dim j As Integer
        Dim c As Double = 0.0001
        Dim l As Integer

        Dim FirstE As Double
        Dim E As Double
        Dim Quality As Object = Nothing
        Dim Sigma As Double = 0.000001
        Obj3d.CalcScale(Mokuhyo3d, Scale)
        Obj3d.SetScale(Scale)
        hom_mat_3d_from_3d_3d_point_correspondence(Obj3d.X, Obj3d.Y, Obj3d.Z, Mokuhyo3d.X, Mokuhyo3d.Y, Mokuhyo3d.Z, HomMat3d)
        HOperatorSet.AffineTransPoint3d(HomMat3d, Obj3d.X, Obj3d.Y, Obj3d.Z, Obj3d.X, Obj3d.Y, Obj3d.Z)
        Obj3d.GetDisttoOtherPose(Mokuhyo3d, Quality)
        Quality = BTuple.TuplePow(Quality, 2)
        FirstE = BTuple.TupleSum(Quality)
        E = FirstE
        S = Scale

        Obj3d.X = xi
        Obj3d.Y = yi
        Obj3d.Z = zi

        SetRT(HomMat3d)

        For l = 1 To 10
            CalcPQRi()
            CalcDPQR_dEi()

            Dim dEmatid As Object = Nothing
            Dim ddEmatid As Object = Nothing
            Dim DeltaMatID As Object = Nothing
            Dim DeltaVal As Object = Nothing

            For i = 1 To 13
                dEi(i - 1) = (-1) * 2 * CDbl(BTuple.TupleSum(BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleMult(Pi, dPi(i)), BTuple.TupleMult(Qi, dQi(i))), BTuple.TupleMult(Ri, dRi(i)))))
            Next
            Dim t As Integer = 0

            HOperatorSet.CreateMatrix(13, 1, dEi, dEmatid)
step3:
            t = 0
            For i = 1 To 13
                For j = 1 To 13
                    If i = j Then
                        ddEij(t) = (1 + c) * 2 * CDbl(BTuple.TupleSum(BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleMult(dPi(i), dPi(j)), BTuple.TupleMult(dQi(i), dQi(j))), BTuple.TupleMult(dRi(i), dRi(j)))))
                    Else
                        ddEij(t) = 2 * CDbl(BTuple.TupleSum(BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleMult(dPi(i), dPi(j)), BTuple.TupleMult(dQi(i), dQi(j))), BTuple.TupleMult(dRi(i), dRi(j)))))
                    End If
                    t += 1
                Next
            Next

            HOperatorSet.CreateMatrix(13, 13, ddEij, ddEmatid)

            '連立一次方程式を解く
            HOperatorSet.SolveMatrix(ddEmatid, "general", 0, dEmatid, DeltaMatID)

            HOperatorSet.GetFullMatrix(DeltaMatID, DeltaVal)

            Dim tmpE As Double = Kousin(DeltaVal)
            If tmpE > E Then
                c = c * 10
                HOperatorSet.ClearMatrix(ddEmatid)
                HOperatorSet.ClearMatrix(DeltaMatID)
                GoTo step3
            End If

            BackKousin(DeltaVal)

            If Math.Abs(tmpE - E) <= Sigma Then
                '終了

                HOperatorSet.ClearMatrix(dEmatid)
                HOperatorSet.ClearMatrix(ddEmatid)
                HOperatorSet.ClearMatrix(DeltaMatID)
                Exit For
            Else
                E = tmpE
                c = c / 10
                HOperatorSet.ClearMatrix(dEmatid)
                HOperatorSet.ClearMatrix(ddEmatid)
                HOperatorSet.ClearMatrix(DeltaMatID)

            End If
        Next

        Result(HomMat3d, Scale)

    End Sub

    Private Sub Result(ByRef HomMat3d As Object, ByRef Scale As Object)

        HomMat3d(0) = m11
        HomMat3d(1) = m12
        HomMat3d(2) = m13
        HomMat3d(4) = m21
        HomMat3d(5) = m22
        HomMat3d(6) = m23
        HomMat3d(8) = m31
        HomMat3d(9) = m32
        HomMat3d(10) = m33
        HomMat3d(3) = PX
        HomMat3d(7) = PY
        HomMat3d(11) = PZ
        Scale = S
    End Sub
    Private Sub BackKousin(ByVal DeltaVal As Object)
        m11 = m11 + BTuple.TupleSelect(DeltaVal, 0)
        m12 = m12 + BTuple.TupleSelect(DeltaVal, 1)
        m13 = m13 + BTuple.TupleSelect(DeltaVal, 2)
        m21 = m21 + BTuple.TupleSelect(DeltaVal, 3)
        m22 = m22 + BTuple.TupleSelect(DeltaVal, 4)
        m23 = m23 + BTuple.TupleSelect(DeltaVal, 5)
        m31 = m31 + BTuple.TupleSelect(DeltaVal, 6)
        m32 = m32 + BTuple.TupleSelect(DeltaVal, 7)
        m33 = m33 + BTuple.TupleSelect(DeltaVal, 8)
        PX = PX + BTuple.TupleSelect(DeltaVal, 9)
        PY = PY + BTuple.TupleSelect(DeltaVal, 10)
        PZ = PZ + BTuple.TupleSelect(DeltaVal, 11)
        S = S + BTuple.TupleSelect(DeltaVal, 12)

    End Sub

    Private Function Kousin(ByVal DeltaVal As Object) As Double
        Dim KousinParam(11) As Double
        Dim Ko_S As Double

        KousinParam(0) = m11 + BTuple.TupleSelect(DeltaVal, 0)
        KousinParam(1) = m12 + BTuple.TupleSelect(DeltaVal, 1)
        KousinParam(2) = m13 + BTuple.TupleSelect(DeltaVal, 2)
        KousinParam(4) = m21 + BTuple.TupleSelect(DeltaVal, 3)
        KousinParam(5) = m22 + BTuple.TupleSelect(DeltaVal, 4)
        KousinParam(6) = m23 + BTuple.TupleSelect(DeltaVal, 5)
        KousinParam(8) = m31 + BTuple.TupleSelect(DeltaVal, 6)
        KousinParam(9) = m32 + BTuple.TupleSelect(DeltaVal, 7)
        KousinParam(10) = m33 + BTuple.TupleSelect(DeltaVal, 8)
        KousinParam(3) = PX + BTuple.TupleSelect(DeltaVal, 9)
        KousinParam(7) = PY + BTuple.TupleSelect(DeltaVal, 10)
        KousinParam(11) = PZ + BTuple.TupleSelect(DeltaVal, 11)

        Ko_S = S + BTuple.TupleSelect(DeltaVal, 12)
        Dim Obj3d As New Point3D
        Dim Mokuhyo3d As New Point3D
        Dim Quality As Object = Nothing

        Mokuhyo3d.X = X0i
        Mokuhyo3d.Y = Y0i
        Mokuhyo3d.Z = Z0i
        HOperatorSet.AffineTransPoint3d(KousinParam, BTuple.TupleMult(xi, Ko_S), BTuple.TupleMult(yi, Ko_S), BTuple.TupleMult(zi, Ko_S), Obj3d.X, Obj3d.Y, Obj3d.Z)
        Obj3d.GetDisttoOtherPose(Mokuhyo3d, Quality)
        Quality = BTuple.TuplePow(Quality, 2)
        Kousin = BTuple.TupleSum(Quality)

    End Function



    Private Sub CalcPQRi()
        Pi = BTuple.TupleSub(BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleAdd(MIS(m11, xi, S), MIS(m12, yi, S)), MIS(m13, zi, S)), PX), X0i)
        Qi = BTuple.TupleSub(BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleAdd(MIS(m21, xi, S), MIS(m22, yi, S)), MIS(m23, zi, S)), PY), Y0i)
        Ri = BTuple.TupleSub(BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleAdd(MIS(m31, xi, S), MIS(m32, yi, S)), MIS(m33, zi, S)), PZ), Z0i)

    End Sub

    Private Function MIS(ByVal m As Object, ByVal x As Object, ByVal s As Object) As Object

        MIS = BTuple.TupleMult(BTuple.TupleMult(x, s), m)

    End Function

    Private Sub SetRT(ByVal HomMat3d As Object)

        m11 = BTuple.TupleSelect(HomMat3d, 0)
        m12 = BTuple.TupleSelect(HomMat3d, 1)
        m13 = BTuple.TupleSelect(HomMat3d, 2)
        m21 = BTuple.TupleSelect(HomMat3d, 4)
        m22 = BTuple.TupleSelect(HomMat3d, 5)
        m23 = BTuple.TupleSelect(HomMat3d, 6)
        m31 = BTuple.TupleSelect(HomMat3d, 8)
        m32 = BTuple.TupleSelect(HomMat3d, 9)
        m33 = BTuple.TupleSelect(HomMat3d, 10)
        PX = BTuple.TupleSelect(HomMat3d, 3)
        PY = BTuple.TupleSelect(HomMat3d, 7)
        PZ = BTuple.TupleSelect(HomMat3d, 11)
    End Sub

    Private Sub CalcDPQR_dEi()
        dPi(1) = BTuple.TupleMult(xi, S)
        dPi(2) = BTuple.TupleMult(yi, S)
        dPi(3) = BTuple.TupleMult(zi, S)
        dPi(4) = BTuple.TupleGenConst(1, 0)
        dPi(5) = BTuple.TupleGenConst(1, 0)
        dPi(6) = BTuple.TupleGenConst(1, 0)
        dPi(7) = BTuple.TupleGenConst(1, 0)
        dPi(8) = BTuple.TupleGenConst(1, 0)
        dPi(9) = BTuple.TupleGenConst(1, 0)
        dPi(10) = BTuple.TupleGenConst(1, 1)
        dPi(11) = BTuple.TupleGenConst(1, 0)
        dPi(12) = BTuple.TupleGenConst(1, 0)
        dPi(13) = BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleMult(m11, xi), BTuple.TupleMult(m12, yi)), BTuple.TupleMult(m13, zi))

        dQi(1) = BTuple.TupleGenConst(1, 0)
        dQi(2) = BTuple.TupleGenConst(1, 0)
        dQi(3) = BTuple.TupleGenConst(1, 0)
        dQi(4) = BTuple.TupleMult(xi, S)
        dQi(5) = BTuple.TupleMult(yi, S)
        dQi(6) = BTuple.TupleMult(zi, S)
        dQi(7) = BTuple.TupleGenConst(1, 0)
        dQi(8) = BTuple.TupleGenConst(1, 0)
        dQi(9) = BTuple.TupleGenConst(1, 0)
        dQi(10) = BTuple.TupleGenConst(1, 0)
        dQi(11) = BTuple.TupleGenConst(1, 1)
        dQi(12) = BTuple.TupleGenConst(1, 0)
        dQi(13) = BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleMult(m21, xi), BTuple.TupleMult(m22, yi)), BTuple.TupleMult(m23, zi))

        dRi(1) = BTuple.TupleGenConst(1, 0)
        dRi(2) = BTuple.TupleGenConst(1, 0)
        dRi(3) = BTuple.TupleGenConst(1, 0)
        dRi(4) = BTuple.TupleGenConst(1, 0)
        dRi(5) = BTuple.TupleGenConst(1, 0)
        dRi(6) = BTuple.TupleGenConst(1, 0)
        dRi(7) = BTuple.TupleMult(xi, S)
        dRi(8) = BTuple.TupleMult(yi, S)
        dRi(9) = BTuple.TupleMult(zi, S)
        dRi(10) = BTuple.TupleGenConst(1, 0)
        dRi(11) = BTuple.TupleGenConst(1, 0)
        dRi(12) = BTuple.TupleGenConst(1, 1)
        dRi(13) = BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleMult(m31, xi), BTuple.TupleMult(m32, yi)), BTuple.TupleMult(m33, zi))

    End Sub
End Module
