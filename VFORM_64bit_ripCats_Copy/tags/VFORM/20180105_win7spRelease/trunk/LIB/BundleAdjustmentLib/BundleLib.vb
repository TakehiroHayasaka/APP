Public Class BundleLib

    Private _ImageNum As Integer
    Public Property ImageNum() As Integer
        Get
            Return _ImageNum
        End Get
        Set(ByVal value As Integer)
            _ImageNum = value
        End Set
    End Property

    Private _PointNum As Integer
    Public Property PointNum() As Integer
        Get
            Return _PointNum
        End Get
        Set(ByVal value As Integer)
            _PointNum = value
        End Set
    End Property
    Private _lstBundData As List(Of BundleData)
    Public Property lstBundData() As List(Of BundleData)
        Get
            Return _lstBundData
        End Get
        Set(ByVal value As List(Of BundleData))
            _lstBundData = value
        End Set
    End Property

    Public WeightValue As Double = 0.000000000000546020500624

    Public Sub SetData()

    End Sub
    Public Sub RunBundleAdj()
        GenBMatrixNew1()
        ' SaveToFileResult()
    End Sub


    Private Sub GenBMatrixNew1()
        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim t As Integer
        Dim nR As Integer

        Dim BMatId As New Object
        Dim B1MatId As New Object
        Dim B2MatId As New Object
        Dim B3MatId As New Object
        Dim E1MatId As New Object
        Dim EMatId As New Object
        Dim LMatId As New Object
        Dim WMatId As New Object
        Dim IMatId As New Object
        Dim ZeroMatID As New Object
        Dim intShikiNum As Integer = lstBundData.Count
        Dim intColNum As Integer = 6 * ImageNum + 8 + 3 * PointNum
        Dim intRowNum As Integer = 2 * intShikiNum + intColNum + 1

        '   Dim TMatId As New Object
        For t = 0 To 200
            Op.CreateMatrix(intRowNum, intColNum, 0, BMatId)
            Op.CreateMatrix(intRowNum, 1, 0, EMatId)
            '    Op.CreateMatrix(intRowNum, intRowNum, "identity", WMatId)
            Op.CreateMatrix(intColNum, intColNum, "identity", IMatId)
            Op.CreateMatrix(intColNum, intColNum, 0, ZeroMatID)

            Op.ScaleMatrixMod(IMatId, -1)
            ' Op.CreateMatrix(2 * PointNum * ImageNum + 6 + 8 + 1, 2 * PointNum * ImageNum + 6 + 8 + 1, 0.000000000000546020500624, TMatId)
            ' Op.ScaleMatrixMod(WMatId, WeightValue)

            'Op.MultElementMatrixMod(WMatId, TMatId)
            'Op.ClearMatrix(TMatId)

            k = 0
            nR = 0
            For Each BD As BundleData In _lstBundData
                nR = k * 2
                BD.Refresh()

                B1MatId = BD.GenB1Matrix
                B2MatId = BD.GenB2Matrix
                B3MatId = BD.GenB3Matrix
                E1MatId = BD.GenEMatrix
                Op.SetSubMatrix(BMatId, B1MatId, nR, BD.ImageIndex * 6)
                Op.SetSubMatrix(BMatId, B2MatId, nR, 6 * ImageNum)
                Op.SetSubMatrix(BMatId, B3MatId, nR, 6 * ImageNum + 8 + BD.PointIndex * 3)
                Op.SetSubMatrix(EMatId, E1MatId, nR, 0)
                k += 1
                Op.ClearMatrix(B1MatId)
                Op.ClearMatrix(B2MatId)
                Op.ClearMatrix(B3MatId)
                Op.ClearMatrix(E1MatId)
            Next
            Dim EMatVal As New Object
            Dim EMataveVal As New Object
            Dim E_UmatId As New Object
            Dim E_TMatId As New Object
            Dim ExE_TMatId As New Object
            Dim ExE_TnoDiagMatId As New Object
            Dim ExE_TnoDiagMatVal As New Object
            Op.GetFullMatrix(EMatId, EMatVal)
            Op.TupleMean(EMatVal, EMataveVal)
            Op.TupleSub(EMatVal, EMataveVal, EMatVal)
            Op.CreateMatrix(Tuple.TupleLength(EMatVal), 1, EMatVal, E_UmatId)
            Op.TransposeMatrix(E_UmatId, E_TMatId)
            Op.MultMatrix(E_UmatId, E_TMatId, "AB", ExE_TMatId)
            Op.GetDiagonalMatrix(ExE_TMatId, 0, ExE_TnoDiagMatId)
            Op.GetFullMatrix(ExE_TnoDiagMatId, ExE_TnoDiagMatVal)
            Op.TupleDiv(1, ExE_TnoDiagMatVal, ExE_TnoDiagMatVal)
            Op.TupleMult(WeightValue * WeightValue, ExE_TnoDiagMatVal, ExE_TnoDiagMatVal)

            Op.CreateMatrix(intRowNum, intRowNum, ExE_TnoDiagMatVal, WMatId)

            Op.SetSubMatrix(BMatId, IMatId, 2 * intShikiNum, 0)
            Op.SetSubMatrix(WMatId, ZeroMatID, 2 * intShikiNum, 2 * intShikiNum)

            For i = 0 To 5
                Op.SetValueMatrix(WMatId, 2 * intShikiNum + i, 2 * intShikiNum + i, 100)
            Next

            For i = 0 To 7
                Op.SetValueMatrix(WMatId, 2 * intShikiNum + 6 * ImageNum + i, 2 * intShikiNum + 6 * ImageNum + i, 100)
            Next
            Op.SetValueMatrix(WMatId, intRowNum - 1, intRowNum - 1, 100)

            Op.SetValueMatrix(BMatId, intRowNum - 1, 0, (2) * (_lstBundData.Item(0).CamObj.X0 - _lstBundData.Item(1).CamObj.X0))
            Op.SetValueMatrix(BMatId, intRowNum - 1, 1, (2) * (_lstBundData.Item(0).CamObj.Y0 - _lstBundData.Item(1).CamObj.Y0))
            Op.SetValueMatrix(BMatId, intRowNum - 1, 2, (2) * (_lstBundData.Item(0).CamObj.Z0 - _lstBundData.Item(1).CamObj.Z0))

            Op.SetValueMatrix(BMatId, intRowNum - 1, 6, (-2) * (_lstBundData.Item(0).CamObj.X0 - _lstBundData.Item(1).CamObj.X0))
            Op.SetValueMatrix(BMatId, intRowNum - 1, 7, (-2) * (_lstBundData.Item(0).CamObj.Y0 - _lstBundData.Item(1).CamObj.Y0))
            Op.SetValueMatrix(BMatId, intRowNum - 1, 8, (-2) * (_lstBundData.Item(0).CamObj.Z0 - _lstBundData.Item(1).CamObj.Z0))

            E1MatId = _lstBundData.Item(0).GenE1Matrix
            Op.SetSubMatrix(EMatId, E1MatId, 2 * intShikiNum, 0)
            Op.ClearMatrix(E1MatId)
            E1MatId = _lstBundData.Item(0).GenE2Matrix
            Op.SetSubMatrix(EMatId, E1MatId, 2 * intShikiNum + 6 * ImageNum, 0)
            Op.ClearMatrix(E1MatId)
            Op.ClearMatrix(IMatId)
            Op.ClearMatrix(ZeroMatID)

            Op.SetValueMatrix(EMatId, intRowNum - 1, 0, Cam12Kyori)
            'If t = 0 Then
            '    Dim tempE As New Object
            '    Dim TempB As New Object
            '    Dim tempW As New Object

            '    Op.GetFullMatrix(BMatId, TempB)
            '    Op.GetFullMatrix(EMatId, tempE)
            '    Op.GetFullMatrix(WMatId, tempW)

            '    k = 0
            '    Dim strLine As String
            '    Dim strBmat As String = ""
            '    For i = 0 To intRowNum - 1
            '        strLine = ""
            '        For j = 0 To intColNum - 1

            '            strLine = strLine & CDbl(Tuple.TupleSelect(TempB, k)) & ","

            '            k += 1
            '        Next
            '        strLine = strLine & CDbl(Tuple.TupleSelect(tempE, i)) & vbNewLine
            '        strBmat = strBmat & strLine
            '    Next
            '    My.Computer.FileSystem.WriteAllText("C:\My Work\MandS\HALCON9関連\NonTarget\BundleAdjustmentLib\TestData4\BmatText.csv", strBmat, True)

            '    strBmat = ""
            '    k = 0
            '    For i = 0 To intRowNum - 1
            '        strLine = ""
            '        For j = 0 To intRowNum - 1
            '            strLine = strLine & CDbl(Tuple.TupleSelect(tempW, k)) & ","

            '            k += 1
            '        Next
            '        strLine = strLine & vbNewLine
            '        strBmat = strBmat & strLine
            '    Next

            '    My.Computer.FileSystem.WriteAllText("C:\My Work\MandS\HALCON9関連\NonTarget\BundleAdjustmentLib\TestData4\WmatText.csv", strBmat, True)

            'End If

            Dim BTMatid As New Object
            Dim BTWMatId As New Object
            Dim BTWBMatId As New Object
            Dim BTWB_1MatId As New Object
            Dim BTWB_1_BT_MatId As New Object
            Dim BTWB_1_BT_WMatId As New Object
            Dim BTWB_1_BT_W_EMatId As New Object
            Dim DeltaL As New Object

            Dim EMatSum As New Object
            Dim BTWB_1MatVal As New Object
            Op.TransposeMatrix(BMatId, BTMatid)
            Op.MultMatrix(BTMatid, WMatId, "AB", BTWMatId)
            Op.MultMatrix(BTWMatId, BMatId, "AB", BTWBMatId)
            'Dim strTmp As String = My.Application.Info.DirectoryPath
            'Op.WriteMatrix(BTWBMatId, "binary", strTmp & "\InvertMatrix\tmpMat.mtx")
            'Dim strArg As String = strTmp & "\InvertMatrix\InvertMatrix.exe " & """" & strTmp & "\InvertMatrix\tmpMat.mtx" & """"
            'Shell(strArg, AppWinStyle.Hide, True)
            'Op.ReadMatrix(strTmp & "\InvertMatrix\tmpMat.mtx", BTWB_1MatId)
            Op.InvertMatrix(BTWBMatId, "general", 0.00000000000000022204, BTWB_1MatId) '0.00000000000000022204
            Op.MultMatrix(BTWB_1MatId, BTMatid, "AB", BTWB_1_BT_MatId)
            Op.MultMatrix(BTWB_1_BT_MatId, WMatId, "AB", BTWB_1_BT_WMatId)
            Op.MultMatrix(BTWB_1_BT_WMatId, EMatId, "AB", BTWB_1_BT_W_EMatId)
            Op.GetFullMatrix(BTWB_1_BT_W_EMatId, LMatId)
            Op.GetFullMatrix(EMatId, EMatVal)
            Op.GetFullMatrix(BTWB_1MatId, BTWB_1MatVal)
            Op.ClearMatrix(BMatId)
            Op.ClearMatrix(EMatId)
            Op.ClearMatrix(WMatId)
            Op.ClearMatrix(BTMatid)
            Op.ClearMatrix(BTWMatId)
            Op.ClearMatrix(BTWBMatId)
            Op.ClearMatrix(BTWB_1MatId)
            Op.ClearMatrix(BTWB_1_BT_MatId)
            Op.ClearMatrix(BTWB_1_BT_WMatId)
            Op.ClearMatrix(BTWB_1_BT_W_EMatId)

            DeltaL = Tuple.TupleSum(Tuple.TuplePow(LMatId, 2))
            EMatSum = Tuple.TupleSqrt(Tuple.TupleSum(Tuple.TuplePow(EMatVal, 2)))
            If CDbl(Tuple.TupleDeviation(BTWB_1MatVal)) < 0.002 Then

                Exit For
            End If
            Dim DeltaPose As New Object
            Dim DeltaCamPar As New Object
            Dim DeltaXYZ As New Object
            Dim DeltaIndex As New Object

            For Each BD As BundleData In _lstBundData
                tuple_gen_sequence(BD.ImageIndex * 6, BD.ImageIndex * 6 + 6 - 1, 1, DeltaIndex)
                BD.CamObj.SetDeltaPose(Tuple.TupleSelect(LMatId, DeltaIndex))
                tuple_gen_sequence(6 * ImageNum, 6 * ImageNum + 8 - 1, 1, DeltaIndex)
                BD.CamObj.SetDeltaCamPar(Tuple.TupleSelect(LMatId, DeltaIndex))
                tuple_gen_sequence(6 * ImageNum + 8 + BD.PointIndex * 3, 6 * ImageNum + 8 + BD.PointIndex * 3 + 3 - 1, 1, DeltaIndex)
                BD.WorldPointObj.SetDeltaXYZ(Tuple.TupleSelect(LMatId, DeltaIndex))

            Next
        Next
    End Sub


    Private Sub GenBMatrix()
        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim t As Integer
        Dim nR As Integer
        Dim nC As Integer

        Dim BMatId As New Object
        Dim B1MatId As New Object
        Dim B2MatId As New Object
        Dim B3MatId As New Object
        Dim E1MatId As New Object
        Dim EMatId As New Object
        Dim LMatId As New Object
        Dim WMatId As New Object
        '   Dim TMatId As New Object
        For t = 0 To 200
            Op.CreateMatrix(2 * PointNum * ImageNum + 6 + 8 + 1, 6 * ImageNum + 8 + 3 * PointNum, 0, BMatId)
            Op.CreateMatrix(2 * PointNum * ImageNum + 6 + 8 + 1, 1, 0, EMatId)
            Op.CreateMatrix(2 * PointNum * ImageNum + 6 + 8 + 1, 2 * PointNum * ImageNum + 6 + 8 + 1, "identity", WMatId)
            ' Op.CreateMatrix(2 * PointNum * ImageNum + 6 + 8 + 1, 2 * PointNum * ImageNum + 6 + 8 + 1, 0.000000000000546020500624, TMatId)
            Op.ScaleMatrixMod(WMatId, WeightValue)

            'Op.MultElementMatrixMod(WMatId, TMatId)
            'Op.ClearMatrix(TMatId)
          
            k = 0
            nR = 0
            nC = 0
            For i = 0 To PointNum - 1
                nC = i * 3
                For j = 0 To ImageNum - 1
                    nR = k * 2
                    _lstBundData.Item(k).Refresh()

                    B1MatId = _lstBundData.Item(k).GenB1Matrix
                    B2MatId = _lstBundData.Item(k).GenB2Matrix
                    B3MatId = _lstBundData.Item(k).GenB3Matrix
                    E1MatId = _lstBundData.Item(k).GenEMatrix
                    Op.SetSubMatrix(BMatId, B1MatId, nR, j * 6)
                    Op.SetSubMatrix(BMatId, B2MatId, nR, 6 * ImageNum)
                    Op.SetSubMatrix(BMatId, B3MatId, nR, 6 * ImageNum + 8 + nC)
                    Op.SetSubMatrix(EMatId, E1MatId, nR, 0)

                    k += 1

                    Op.ClearMatrix(B1MatId)
                    Op.ClearMatrix(B2MatId)
                    Op.ClearMatrix(B3MatId)
                    Op.ClearMatrix(E1MatId)

                Next
            Next
            For i = 0 To 5
                Op.SetValueMatrix(BMatId, 2 * PointNum * ImageNum + i, i, -1)
                Op.SetValueMatrix(WMatId, 2 * PointNum * ImageNum + i, 2 * PointNum * ImageNum + i, 1)
            Next
            For i = 0 To 7
                Op.SetValueMatrix(BMatId, 2 * PointNum * ImageNum + 6 + i, 6 * ImageNum + i, -1)
                Op.SetValueMatrix(WMatId, 2 * PointNum * ImageNum + 6 + i, 2 * PointNum * ImageNum + 6 + i, 1)
            Next
            Op.SetValueMatrix(WMatId, 2 * PointNum * ImageNum + 6 + 8, 2 * PointNum * ImageNum + 6 + 8, 1)

            Op.SetValueMatrix(BMatId, 2 * PointNum * ImageNum + 6 + 8, 0, (2) * (_lstBundData.Item(0).CamObj.X0 - _lstBundData.Item(1).CamObj.X0))
            Op.SetValueMatrix(BMatId, 2 * PointNum * ImageNum + 6 + 8, 1, (2) * (_lstBundData.Item(0).CamObj.Y0 - _lstBundData.Item(1).CamObj.Y0))
            Op.SetValueMatrix(BMatId, 2 * PointNum * ImageNum + 6 + 8, 2, (2) * (_lstBundData.Item(0).CamObj.Z0 - _lstBundData.Item(1).CamObj.Z0))

            Op.SetValueMatrix(BMatId, 2 * PointNum * ImageNum + 6 + 8, 6, (-2) * (_lstBundData.Item(0).CamObj.X0 - _lstBundData.Item(1).CamObj.X0))
            Op.SetValueMatrix(BMatId, 2 * PointNum * ImageNum + 6 + 8, 7, (-2) * (_lstBundData.Item(0).CamObj.Y0 - _lstBundData.Item(1).CamObj.Y0))
            Op.SetValueMatrix(BMatId, 2 * PointNum * ImageNum + 6 + 8, 8, (-2) * (_lstBundData.Item(0).CamObj.Z0 - _lstBundData.Item(1).CamObj.Z0))

            E1MatId = _lstBundData.Item(0).GenE1Matrix
            Op.SetSubMatrix(EMatId, E1MatId, 2 * PointNum * ImageNum, 0)
            Op.ClearMatrix(E1MatId)
            E1MatId = _lstBundData.Item(0).GenE2Matrix
            Op.SetSubMatrix(EMatId, E1MatId, 2 * PointNum * ImageNum + 6, 0)
            Op.ClearMatrix(E1MatId)

            Op.SetValueMatrix(EMatId, 2 * PointNum * ImageNum + 6 + 8, 0, Cam12Kyori)

            'Dim tempE As New Object
            'Dim TempB As New Object

            'Op.GetFullMatrix(BMatId, TempB)
            'Op.GetFullMatrix(EMatId, tempE)
            'k = 0
            'Dim strLine As String
            'Dim strBmat As String = ""
            'For i = 0 To 2 * PointNum * ImageNum + 6 + 8
            '    strLine = ""
            '    For j = 0 To 6 * ImageNum + 8 + 3 * PointNum - 1

            '        strLine = strLine & CDbl(Tuple.TupleSelect(TempB, k)) & ","

            '        k += 1
            '    Next
            '    strLine = strLine & vbNewLine
            '    strBmat = strBmat & strLine
            'Next
            'My.Computer.FileSystem.WriteAllText("C:\My Work\MandS\HALCON9関連\NonTarget\BundleAdjustmentLib\TestData2\BmatText.csv", strBmat, True)
            Dim BTMatid As New Object
            Dim BTWMatId As New Object
            Dim BTWBMatId As New Object
            Dim BTWB_1MatId As New Object
            Dim BTWB_1_BT_MatId As New Object
            Dim BTWB_1_BT_WMatId As New Object
            Dim BTWB_1_BT_W_EMatId As New Object
            Dim DeltaL As New Object
            Dim EMatVal As New Object
            Dim EMatSum As New Object
            Dim BTWB_1MatVal As New Object
            Op.TransposeMatrix(BMatId, BTMatid)
            Op.MultMatrix(BTMatid, WMatId, "AB", BTWMatId)
            Op.MultMatrix(BTWMatId, BMatId, "AB", BTWBMatId)
            'Dim strTmp As String = My.Application.Info.DirectoryPath
            'Op.WriteMatrix(BTWBMatId, "binary", strTmp & "\InvertMatrix\tmpMat.mtx")
            'Dim strArg As String = strTmp & "\InvertMatrix\InvertMatrix.exe " & """" & strTmp & "\InvertMatrix\tmpMat.mtx" & """"
            'Shell(strArg, AppWinStyle.Hide, True)
            'Op.ReadMatrix(strTmp & "\InvertMatrix\tmpMat.mtx", BTWB_1MatId)
            Op.InvertMatrix(BTWBMatId, "general", 0.00000000000000022204, BTWB_1MatId)
            Op.MultMatrix(BTWB_1MatId, BTMatid, "AB", BTWB_1_BT_MatId)
            Op.MultMatrix(BTWB_1_BT_MatId, WMatId, "AB", BTWB_1_BT_WMatId)
            Op.MultMatrix(BTWB_1_BT_WMatId, EMatId, "AB", BTWB_1_BT_W_EMatId)
            Op.GetFullMatrix(BTWB_1_BT_W_EMatId, LMatId)
            Op.GetFullMatrix(EMatId, EMatVal)
            Op.GetFullMatrix(BTWB_1MatId, BTWB_1MatVal)
            Op.ClearMatrix(BMatId)
            Op.ClearMatrix(EMatId)
            Op.ClearMatrix(WMatId)
            Op.ClearMatrix(BTMatid)
            Op.ClearMatrix(BTWMatId)
            Op.ClearMatrix(BTWBMatId)
            Op.ClearMatrix(BTWB_1MatId)
            Op.ClearMatrix(BTWB_1_BT_MatId)
            Op.ClearMatrix(BTWB_1_BT_WMatId)
            Op.ClearMatrix(BTWB_1_BT_W_EMatId)

            DeltaL = Tuple.TupleSum(Tuple.TuplePow(LMatId, 2))
            EMatSum = Tuple.TupleSqrt(Tuple.TupleSum(Tuple.TuplePow(EMatVal, 2)))
            If CDbl(EMatSum) < 0.002 Then

                Exit For
            End If
            Dim DeltaPose As New Object
            Dim DeltaCamPar As New Object
            Dim DeltaXYZ As New Object
            Dim DeltaIndex As New Object
            nR = 0
            k = 0
            For i = 0 To PointNum - 1
                For j = 0 To ImageNum - 1
                    tuple_gen_sequence(j * 6, j * 6 + 6 - 1, 1, DeltaIndex)
                    _lstBundData.Item(k).CamObj.SetDeltaPose(Tuple.TupleSelect(LMatId, DeltaIndex))
                    tuple_gen_sequence(6 * ImageNum, 6 * ImageNum + 8 - 1, 1, DeltaIndex)
                    _lstBundData.Item(k).CamObj.SetDeltaCamPar(Tuple.TupleSelect(LMatId, DeltaIndex))
                    tuple_gen_sequence(6 * ImageNum + 8 + i * 3, 6 * ImageNum + 8 + i * 3 + 3 - 1, 1, DeltaIndex)
                    _lstBundData.Item(k).WorldPointObj.SetDeltaXYZ(Tuple.TupleSelect(LMatId, DeltaIndex))
                    k += 1
                Next
            Next
        Next
    End Sub


    Private Sub GenBMatrixNew()
        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim t As Integer
        Dim nR As Integer

        Dim BMatId As New Object
        Dim B1MatId As New Object
        Dim B2MatId As New Object
        Dim B3MatId As New Object
        Dim E1MatId As New Object
        Dim EMatId As New Object
        Dim LMatId As New Object
        Dim WMatId As New Object
        Dim IMatId As New Object
        Dim intShikiNum As Integer = lstBundData.Count
        '   Dim TMatId As New Object
        For t = 0 To 200
            Op.CreateMatrix(2 * intShikiNum + 6 + 8 + 1, 6 * ImageNum + 8 + 3 * PointNum, 0, BMatId)
            Op.CreateMatrix(2 * intShikiNum + 6 + 8 + 1, 1, 0, EMatId)
            Op.CreateMatrix(2 * intShikiNum + 6 + 8 + 1, 2 * intShikiNum + 6 + 8 + 1, "identity", WMatId)
            Op.CreateMatrix(6 * ImageNum + 8 + 3 * PointNum, 6 * ImageNum + 8 + 3 * PointNum, -1, IMatId)

            ' Op.CreateMatrix(2 * PointNum * ImageNum + 6 + 8 + 1, 2 * PointNum * ImageNum + 6 + 8 + 1, 0.000000000000546020500624, TMatId)
            Op.ScaleMatrixMod(WMatId, WeightValue)

            'Op.MultElementMatrixMod(WMatId, TMatId)
            'Op.ClearMatrix(TMatId)

            k = 0
            nR = 0
            For Each BD As BundleData In _lstBundData
                nR = k * 2
                BD.Refresh()

                B1MatId = BD.GenB1Matrix
                B2MatId = BD.GenB2Matrix
                B3MatId = BD.GenB3Matrix
                E1MatId = BD.GenEMatrix
                Op.SetSubMatrix(BMatId, B1MatId, nR, BD.ImageIndex * 6)
                Op.SetSubMatrix(BMatId, B2MatId, nR, 6 * ImageNum)
                Op.SetSubMatrix(BMatId, B3MatId, nR, 6 * ImageNum + 8 + BD.PointIndex * 3)
                Op.SetSubMatrix(EMatId, E1MatId, nR, 0)
                k += 1
                Op.ClearMatrix(B1MatId)
                Op.ClearMatrix(B2MatId)
                Op.ClearMatrix(B3MatId)
                Op.ClearMatrix(E1MatId)
            Next
            Op.SetSubMatrix(BMatId, IMatId, 2 * intShikiNum, 0)

            For i = 0 To 5
                Op.SetValueMatrix(BMatId, 2 * intShikiNum + i, i, -1)
                Op.SetValueMatrix(WMatId, 2 * intShikiNum + i, 2 * intShikiNum + i, 1000)
            Next
            For i = 0 To 7
                Op.SetValueMatrix(BMatId, 2 * intShikiNum + 6 + i, 6 * ImageNum + i, -1)
                Op.SetValueMatrix(WMatId, 2 * intShikiNum + 6 + i, 2 * intShikiNum + 6 + i, 1000)
            Next
            Op.SetValueMatrix(WMatId, 2 * intShikiNum + 6 + 8, 2 * intShikiNum + 6 + 8, 1000)

            Op.SetValueMatrix(BMatId, 2 * intShikiNum + 6 + 8, 0, (2) * (_lstBundData.Item(0).CamObj.X0 - _lstBundData.Item(1).CamObj.X0))
            Op.SetValueMatrix(BMatId, 2 * intShikiNum + 6 + 8, 1, (2) * (_lstBundData.Item(0).CamObj.Y0 - _lstBundData.Item(1).CamObj.Y0))
            Op.SetValueMatrix(BMatId, 2 * intShikiNum + 6 + 8, 2, (2) * (_lstBundData.Item(0).CamObj.Z0 - _lstBundData.Item(1).CamObj.Z0))

            Op.SetValueMatrix(BMatId, 2 * intShikiNum + 6 + 8, 6, (-2) * (_lstBundData.Item(0).CamObj.X0 - _lstBundData.Item(1).CamObj.X0))
            Op.SetValueMatrix(BMatId, 2 * intShikiNum + 6 + 8, 7, (-2) * (_lstBundData.Item(0).CamObj.Y0 - _lstBundData.Item(1).CamObj.Y0))
            Op.SetValueMatrix(BMatId, 2 * intShikiNum + 6 + 8, 8, (-2) * (_lstBundData.Item(0).CamObj.Z0 - _lstBundData.Item(1).CamObj.Z0))

            E1MatId = _lstBundData.Item(0).GenE1Matrix
            Op.SetSubMatrix(EMatId, E1MatId, 2 * intShikiNum, 0)
            Op.ClearMatrix(E1MatId)
            E1MatId = _lstBundData.Item(0).GenE2Matrix
            Op.SetSubMatrix(EMatId, E1MatId, 2 * intShikiNum + 6, 0)
            Op.ClearMatrix(E1MatId)

            Op.SetValueMatrix(EMatId, 2 * intShikiNum + 6 + 8, 0, Cam12Kyori)
            'If t = 0 Then
            '    Dim tempE As New Object
            '    Dim TempB As New Object

            '    Op.GetFullMatrix(BMatId, TempB)
            '    Op.GetFullMatrix(EMatId, tempE)
            '    k = 0
            '    Dim strLine As String
            '    Dim strBmat As String = ""
            '    For i = 0 To 2 * intShikiNum + 6 + 8
            '        strLine = ""
            '        For j = 0 To 6 * ImageNum + 8 + 3 * PointNum - 1

            '            strLine = strLine & CDbl(Tuple.TupleSelect(TempB, k)) & ","

            '            k += 1
            '        Next
            '        strLine = strLine & vbNewLine
            '        strBmat = strBmat & strLine
            '    Next

            '    My.Computer.FileSystem.WriteAllText("C:\My Work\MandS\HALCON9関連\NonTarget\BundleAdjustmentLib\TestData4\BmatText.csv", strBmat, True)

            'End If
          
            Dim BTMatid As New Object
            Dim BTWMatId As New Object
            Dim BTWBMatId As New Object
            Dim BTWB_1MatId As New Object
            Dim BTWB_1_BT_MatId As New Object
            Dim BTWB_1_BT_WMatId As New Object
            Dim BTWB_1_BT_W_EMatId As New Object
            Dim DeltaL As New Object
            Dim EMatVal As New Object
            Dim EMatSum As New Object
            Dim BTWB_1MatVal As New Object
            Op.TransposeMatrix(BMatId, BTMatid)
            Op.MultMatrix(BTMatid, WMatId, "AB", BTWMatId)
            Op.MultMatrix(BTWMatId, BMatId, "AB", BTWBMatId)
            'Dim strTmp As String = My.Application.Info.DirectoryPath
            'Op.WriteMatrix(BTWBMatId, "binary", strTmp & "\InvertMatrix\tmpMat.mtx")
            'Dim strArg As String = strTmp & "\InvertMatrix\InvertMatrix.exe " & """" & strTmp & "\InvertMatrix\tmpMat.mtx" & """"
            'Shell(strArg, AppWinStyle.Hide, True)
            'Op.ReadMatrix(strTmp & "\InvertMatrix\tmpMat.mtx", BTWB_1MatId)
            Op.InvertMatrix(BTWBMatId, "general", 0.00000000000000022204, BTWB_1MatId) '0.00000000000000022204
            Op.MultMatrix(BTWB_1MatId, BTMatid, "AB", BTWB_1_BT_MatId)
            Op.MultMatrix(BTWB_1_BT_MatId, WMatId, "AB", BTWB_1_BT_WMatId)
            Op.MultMatrix(BTWB_1_BT_WMatId, EMatId, "AB", BTWB_1_BT_W_EMatId)
            Op.GetFullMatrix(BTWB_1_BT_W_EMatId, LMatId)
            Op.GetFullMatrix(EMatId, EMatVal)
            Op.GetFullMatrix(BTWB_1MatId, BTWB_1MatVal)
            Op.ClearMatrix(BMatId)
            Op.ClearMatrix(EMatId)
            Op.ClearMatrix(WMatId)
            Op.ClearMatrix(BTMatid)
            Op.ClearMatrix(BTWMatId)
            Op.ClearMatrix(BTWBMatId)
            Op.ClearMatrix(BTWB_1MatId)
            Op.ClearMatrix(BTWB_1_BT_MatId)
            Op.ClearMatrix(BTWB_1_BT_WMatId)
            Op.ClearMatrix(BTWB_1_BT_W_EMatId)

            DeltaL = Tuple.TupleSum(Tuple.TuplePow(LMatId, 2))
            EMatSum = Tuple.TupleSqrt(Tuple.TupleSum(Tuple.TuplePow(EMatVal, 2)))
            If CDbl(DeltaL) < 0.002 Then

                Exit For
            End If
            Dim DeltaPose As New Object
            Dim DeltaCamPar As New Object
            Dim DeltaXYZ As New Object
            Dim DeltaIndex As New Object

            For Each BD As BundleData In _lstBundData
                tuple_gen_sequence(BD.ImageIndex * 6, BD.ImageIndex * 6 + 6 - 1, 1, DeltaIndex)
                BD.CamObj.SetDeltaPose(Tuple.TupleSelect(LMatId, DeltaIndex))
                tuple_gen_sequence(6 * ImageNum, 6 * ImageNum + 8 - 1, 1, DeltaIndex)
                BD.CamObj.SetDeltaCamPar(Tuple.TupleSelect(LMatId, DeltaIndex))
                tuple_gen_sequence(6 * ImageNum + 8 + BD.PointIndex * 3, 6 * ImageNum + 8 + BD.PointIndex * 3 + 3 - 1, 1, DeltaIndex)
                BD.WorldPointObj.SetDeltaXYZ(Tuple.TupleSelect(LMatId, DeltaIndex))

            Next
        Next
    End Sub
    Private Function Cam12Kyori() As Double
        Dim DeltaX12 As Double = CDbl(_lstBundData.Item(0).CamObj.X0 - _lstBundData.Item(1).CamObj.X0)
        Dim DeltaY12 As Double = CDbl(_lstBundData.Item(0).CamObj.Y0 - _lstBundData.Item(1).CamObj.Y0)
        Dim DeltaZ12 As Double = CDbl(_lstBundData.Item(0).CamObj.Z0 - _lstBundData.Item(1).CamObj.Z0)

        Cam12Kyori = (-1) * (DeltaX12 * DeltaX12 + DeltaY12 * DeltaY12 + DeltaZ12 * DeltaZ12 - 1)

    End Function
    Private Sub SaveToFileResult()
        Dim DeltaPose As New Object
        Dim DeltaCamPar As New Object
        Dim DeltaXYZ As New Object
        Dim DeltaIndex As New Object
        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim nR As Integer

        nR = 0
        k = 0
        Dim strLine As String = ""
        For j = 0 To ImageNum - 1
            strLine = strLine & _lstBundData.Item(j).CamObj.X0 & "," & _lstBundData.Item(j).CamObj.Y0 & "," & _lstBundData.Item(j).CamObj.Z0 & ","
            strLine = strLine & _lstBundData.Item(j).CamObj.A0 & "," & _lstBundData.Item(j).CamObj.B0 & "," & _lstBundData.Item(j).CamObj.G0 & vbNewLine
        Next

        My.Computer.FileSystem.WriteAllText("C:\My Work\MandS\HALCON9関連\NonTarget\BundleAdjustmentLib\TestData\CamPose.csv", strLine, True)
        strLine = ""
        For i = 0 To PointNum - 1
            strLine = strLine & _lstBundData.Item(i * 3).WorldPointObj.X & "," & _lstBundData.Item(i * 3).WorldPointObj.Y & "," & _lstBundData.Item(i * 3).WorldPointObj.Z & vbNewLine
        Next
        My.Computer.FileSystem.WriteAllText("C:\My Work\MandS\HALCON9関連\NonTarget\BundleAdjustmentLib\TestData\PointXYZ.csv", strLine, True)

    End Sub
    Private Function GenEMatrix() As Object
        GenEMatrix = Nothing


        Return GenEMatrix

    End Function

    Private Function GenWMatrix() As Object
        GenWMatrix = Nothing


        Return GenWMatrix

    End Function

    Private Function CalcLMatrix() As Object
        CalcLMatrix = Nothing


        Return CalcLMatrix

    End Function

End Class
