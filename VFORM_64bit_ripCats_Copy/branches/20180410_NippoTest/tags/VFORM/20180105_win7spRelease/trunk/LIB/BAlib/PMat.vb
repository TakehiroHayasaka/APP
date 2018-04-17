Imports HalconDotNet

Public Class PMat
    Public P11 As Double = 0.0
    Public P12 As Double = 0.0
    Public P13 As Double = 0.0
    Public P14 As Double = 0.0
    Public P21 As Double = 0.0
    Public P22 As Double = 0.0
    Public P23 As Double = 0.0
    Public P24 As Double = 0.0
    Public P31 As Double = 0.0
    Public P32 As Double = 0.0
    Public P33 As Double = 0.0
    Public P34 As Double = 0.0

    Public Sub New()

    End Sub

    Public Sub SetDataByHomMat2(ByVal HomMat As Object, ByVal CMat() As Double)
        Dim PMatID As Object = Nothing
        Dim PMatValue As Object = Nothing
        Dim tmpMatID As Object = Nothing
        Dim tmpCMatID As Object = Nothing
        Dim RTmat(8) As Double
        Dim Tmat(2) As Double
        Dim RTmatid As Object = Nothing
        Dim Tmatid As Object = Nothing
        Dim RT_x_Tmatid As Object = Nothing
        RTmat(0) = BTuple.TupleSelect(HomMat, 0).D
        RTmat(1) = BTuple.TupleSelect(HomMat, 4).D
        RTmat(2) = BTuple.TupleSelect(HomMat, 8).D
        RTmat(3) = BTuple.TupleSelect(HomMat, 1).D
        RTmat(4) = BTuple.TupleSelect(HomMat, 5).D
        RTmat(5) = BTuple.TupleSelect(HomMat, 9).D
        RTmat(6) = BTuple.TupleSelect(HomMat, 2).D
        RTmat(7) = BTuple.TupleSelect(HomMat, 6).D
        RTmat(8) = BTuple.TupleSelect(HomMat, 10).D
        Tmat(0) = BTuple.TupleSelect(HomMat, 3).D * (-1)
        Tmat(1) = BTuple.TupleSelect(HomMat, 7).D * (-1)
        Tmat(2) = BTuple.TupleSelect(HomMat, 11).D * (-1)
        HOperatorSet.CreateMatrix(3, 3, RTmat, RTmatid)
        HOperatorSet.CreateMatrix(3, 1, Tmat, Tmatid)
        HOperatorSet.MultMatrix(RTmatid, Tmatid, "AB", RT_x_Tmatid)


        HOperatorSet.CreateMatrix(3, 4, 0, tmpMatID)
        HOperatorSet.SetSubMatrix(tmpMatID, RTmatid, 0, 0)
        HOperatorSet.SetSubMatrix(tmpMatID, RT_x_Tmatid, 0, 3)

        HOperatorSet.CreateMatrix(3, 3, CMat, tmpCMatID)

        HOperatorSet.MultMatrix(tmpCMatID, tmpMatID, "AB", PMatID)
        HOperatorSet.GetFullMatrix(PMatID, PMatValue)
        HOperatorSet.ClearMatrix(tmpMatID)
        HOperatorSet.ClearMatrix(PMatID)
        HOperatorSet.ClearMatrix(tmpCMatID)

        HOperatorSet.ClearMatrix(RTmatid)
        HOperatorSet.ClearMatrix(RT_x_Tmatid)
        HOperatorSet.ClearMatrix(Tmatid)

        P11 = PMatValue(0).D
        P12 = PMatValue(1).D
        P13 = PMatValue(2).D
        P14 = PMatValue(3).D
        P21 = PMatValue(4).D
        P22 = PMatValue(5).D
        P23 = PMatValue(6).D
        P24 = PMatValue(7).D
        P31 = PMatValue(8).D
        P32 = PMatValue(9).D
        P33 = PMatValue(10).D
        P34 = PMatValue(11).D

    End Sub

    Public Sub SetDataByHomMat(ByVal HomMat As Object, ByVal CMat() As Double)

        Dim Concat As Object = Nothing
        Dim PMatID As Object = Nothing
        Dim PMatValue As Object = Nothing
        Dim Val1 As Object = Nothing
        Dim Selected As Object = Nothing
        Dim tmpMatID As Object = Nothing
        Dim tmpCMatID As Object = Nothing

        HOperatorSet.TupleConcat(HomMat, BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(0, 0), 0), 1), Concat)
        HOperatorSet.CreateMatrix(4, 4, Concat, tmpMatID)
        HOperatorSet.InvertMatrixMod(tmpMatID, "general", 0)
        HOperatorSet.GetFullMatrix(tmpMatID, Val1)
        HOperatorSet.TupleFirstN(Val1, 11, Selected)
        HOperatorSet.ClearMatrix(tmpMatID)
        HOperatorSet.CreateMatrix(3, 4, Selected, tmpMatID)
        HOperatorSet.CreateMatrix(3, 3, CMat, tmpCMatID)

        HOperatorSet.MultMatrix(tmpCMatID, tmpMatID, "AB", PMatID)
        HOperatorSet.GetFullMatrix(PMatID, PMatValue)
        HOperatorSet.ClearMatrix(tmpMatID)
        HOperatorSet.ClearMatrix(PMatID)
        HOperatorSet.ClearMatrix(tmpCMatID)

        P11 = PMatValue(0).D
        P12 = PMatValue(1).D
        P13 = PMatValue(2).D
        P14 = PMatValue(3).D
        P21 = PMatValue(4).D
        P22 = PMatValue(5).D
        P23 = PMatValue(6).D
        P24 = PMatValue(7).D
        P31 = PMatValue(8).D
        P32 = PMatValue(9).D
        P33 = PMatValue(10).D
        P34 = PMatValue(11).D
    End Sub

    Public Sub CopyToMe(ByVal objP As PMat)

        P11 = objP.P11
        P12 = objP.P12
        P13 = objP.P13
        P14 = objP.P14
        P21 = objP.P21
        P22 = objP.P22
        P23 = objP.P23
        P24 = objP.P24
        P31 = objP.P31
        P32 = objP.P32
        P33 = objP.P33
        P34 = objP.P34

    End Sub
End Class
