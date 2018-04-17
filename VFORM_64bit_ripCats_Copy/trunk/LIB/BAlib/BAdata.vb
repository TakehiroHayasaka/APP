Imports System.Threading.Tasks
Imports HalconDotNet

Public Class BAdata
    Public Points() As Point3D
    Public ImagePoints(,) As Point2D
    Public Images() As CameraData
    Public Imat(,) As Integer

    Public dEmat() As Double
    'Public ddEmat(,) As Double
    ' Public Emat(,) As Double
    Public lstEmat As List(Of Double(,))
    
    'Public Fmat(,) As Double
    Public Fmat()(,) As Double
    '  Public lstFmat As List(Of Double(,))

    Public Gmat(,) As Double

    Public Pak(,) As Double
    Public Qak(,) As Double
    Public Rak(,) As Double
    Public Uak(,) As Double
    Public Vak(,) As Double
    Public Dak(,) As Double
    Public HXak(,) As Double
    Public HYak(,) As Double

    Public dblE As Double '再投影誤差
    Public dbl_SqrtE As Double

    Public N As Integer = 0
    Public Shikisuu As Integer

    Public CNP() As CamNaibuParam

    Public FirstImageIndex As Integer
    Public SecondImageIndex As Integer
    Public SecondImageXorY As Integer
    Public RemoveRowCol(7) As Integer

    Public Class CamNaibuParam
        Public F As Double
        Public K1 As Double
        Public K2 As Double
        Public K3 As Double
        Public P1 As Double
        Public P2 As Double
        Public Sx As Double
        Public Sy As Double
        Public U0 As Double
        Public V0 As Double
        Public A As Double

        Public Sub New()
            F = 0
            K1 = 0
            K2 = 0
            K3 = 0
            P1 = 0
            P2 = 0
            Sx = 0
            Sy = 0
            U0 = 0
            V0 = 0
        End Sub

        Public Sub SetDataByCamParam(ByVal CamParam As Object)
#If IsDotNet = True Then
            F = CamParam(0).D / CamParam(7).D
            K1 = CamParam(1).D
            K2 = CamParam(2).D
            K3 = CamParam(3).D
            P1 = CamParam(4).D
            P2 = CamParam(5).D
            Sx = CamParam(6).D
            Sy = CamParam(7).D
            U0 = CamParam(8).D
            V0 = CamParam(9).D
            A = Sy / Sx
#Else
            F = CamParam(0) / CamParam(7)
            K1 = CamParam(1)
            K2 = CamParam(2)
            K3 = CamParam(3)
            P1 = CamParam(4)
            P2 = CamParam(5)
            Sx = CamParam(6)
            Sy = CamParam(7)
            U0 = CamParam(8)
            V0 = CamParam(9)
            A = Sy / Sx
#End If
        End Sub

        Public Sub CopyToMe(ByVal objCNP As CamNaibuParam)
            F = objCNP.F
            K1 = objCNP.K1
            K2 = objCNP.K2
            K3 = objCNP.K3
            P1 = objCNP.P1
            P2 = objCNP.P2
            Sx = objCNP.Sx
            Sy = objCNP.Sy
            U0 = objCNP.U0
            V0 = objCNP.V0
            A = objCNP.A
        End Sub
    End Class

    Public Sub New()

    End Sub

    Public Sub New(ByVal t As Integer)

        ReDim Points(PointNum)
        ReDim ImagePoints(PointNum, ImageNum)
        ReDim Images(ImageNum)
        ReDim Imat(PointNum, ImageNum)
        ReDim CNP(CameraNum)
        ResetEmat()

        '  CNP.SetDataByCamParam(CamParam)
    End Sub

    Public Sub New(ByVal objBAMain As BAdata)

        ReDim Points(PointNum)
        ReDim ImagePoints(PointNum, ImageNum)
        ReDim Images(ImageNum)
        ReDim Imat(PointNum, ImageNum)
        Dim i As Integer
        Dim j As Integer
        ResetEmat()

        For i = 1 To PointNum
            For j = 1 To ImageNum
                If objBAMain.Imat(i, j) = 1 Then
                    Me.ImagePoints(i, j) = New Point2D(objBAMain.ImagePoints(i, j).x, objBAMain.ImagePoints(i, j).y)
                    Me.Imat(i, j) = objBAMain.Imat(i, j)
                    'Uak(i, j) = (ImagePoints(i, j).x - Images(j).U0) * Images(j).Sx
                    'Vak(i, j) = (ImagePoints(i, j).y - Images(j).V0) * Images(j).Sy
                End If
                '  Me.Images(j).A = objBAMain.Images(j).A
                '  Me.Images(j).CNP.CopyToMe(objBAMain.Images(j).CNP) 'SUURI ADD 
            Next
        Next

        ' CNP.SetDataByCamParam(CamParam)'SUURI DELL
    End Sub

    Public Sub ResetEmat()
        GC.Collect()
        GC.WaitForPendingFinalizers()
        GC.Collect()
        Shikisuu = PointHensu * PointNum + CamGaibuHensu * ImageNum + CamNaibuHensu * CameraNum
        ReDim Pak(PointNum, ImageNum)
        ReDim Qak(PointNum, ImageNum)
        ReDim Rak(PointNum, ImageNum)
        ReDim Uak(PointNum, ImageNum)
        ReDim Vak(PointNum, ImageNum)
        ReDim Dak(PointNum, ImageNum)
        ReDim HXak(PointNum, ImageNum)
        ReDim HYak(PointNum, ImageNum)

    End Sub
    Public Sub ResetdE_E_F_Gmat()

        Shikisuu = PointHensu * PointNum + CamGaibuHensu * ImageNum + CamNaibuHensu * CameraNum
        ReDim dEmat(Shikisuu)
        ' ReDim ddEmat(Shikisuu, Shikisuu)
        '  ReDim Emat(PointHensu * PointNum, PointHensu)
        If lstEmat Is Nothing Then
            lstEmat = New List(Of Double(,))
        Else
            lstEmat.Clear()
        End If
        For i As Integer = 0 To PointNum - 1
            Dim arrEmat(PointHensu - 1, PointHensu - 1) As Double
            lstEmat.Add(arrEmat)
        Next
        'If lstFmat Is Nothing Then
        '    lstFmat = New List(Of Double(,))
        'Else
        '    lstFmat.Clear()
        'End If
        Fmat = New Double(PointNum - 1)(,) {}
        For i As Integer = 0 To PointNum - 1
            Fmat(i) = New Double(PointHensu - 1, CamGaibuHensu * ImageNum + CamNaibuHensu * CameraNum - KoteiHensu - 1) {}

            'Dim arrFmat(PointHensu - 1, CamGaibuHensu * ImageNum + CamNaibuHensu - KoteiHensu - 1) As Double
            'lstFmat.Add(arrFmat)
        Next
        ' ReDim Fmat(PointHensu * PointNum, CamGaibuHensu * ImageNum + CamNaibuHensu)
        ReDim Gmat(CamGaibuHensu * ImageNum + CamNaibuHensu * CameraNum - KoteiHensu - 1, CamGaibuHensu * ImageNum + CamNaibuHensu * CameraNum - KoteiHensu - 1)
        'Fmat = New Double(PointHensu * PointNum)() {}
        'For i As Integer = 0 To PointHensu * PointNum
        '    Fmat(i) = New Double(CamGaibuHensu * ImageNum + CamNaibuHensu) {}
        'Next
    End Sub
    Public Sub Reconst3D()
        Dim i As Integer
        Dim j As Integer

        For i = 1 To PointNum
            Dim nn As Integer = 0
            For j = 1 To ImageNum
                If Imat(i, j) = 1 Then
                    nn = nn + 1
                End If
            Next
            Dim kakko1(3 * 2 * nn - 1) As Double
            Dim kakko3(2 * nn - 1)

            Dim k1 As Integer = 0
            Dim k3 As Integer = 0
            For j = 1 To ImageNum
                If Imat(i, j) = 1 Then
                    Dim tmp2D As Point2D = ImagePoints(i, j)
                    Dim P As PMat = Images(j).P
                    kakko1(k1) = tmp2D.x * P.P31 - F0 * P.P11
                    k1 = k1 + 1
                    kakko1(k1) = tmp2D.x * P.P32 - F0 * P.P12
                    k1 = k1 + 1
                    kakko1(k1) = tmp2D.x * P.P33 - F0 * P.P13
                    k1 = k1 + 1
                    kakko1(k1) = tmp2D.y * P.P31 - F0 * P.P21
                    k1 = k1 + 1
                    kakko1(k1) = tmp2D.y * P.P32 - F0 * P.P22
                    k1 = k1 + 1
                    kakko1(k1) = tmp2D.y * P.P33 - F0 * P.P23
                    k1 = k1 + 1
                    kakko3(k3) = (-1) * (tmp2D.x * P.P34 - F0 * P.P14)
                    k3 = k3 + 1
                    kakko3(k3) = (-1) * (tmp2D.y * P.P34 - F0 * P.P24)
                    k3 = k3 + 1
                End If
            Next
            Dim Kakko1ID As HTuple = Nothing
            Dim Kakko2ID As HTuple = Nothing
            Dim Kakko3ID As HTuple = Nothing

            HOperatorSet.CreateMatrix(2 * nn, 3, kakko1, Kakko1ID)
            HOperatorSet.CreateMatrix(2 * nn, 1, kakko3, Kakko3ID)

            HOperatorSet.SolveMatrix(Kakko1ID, "general", 0.00000000000000022204, Kakko3ID, Kakko2ID)
            Dim XYZa As HTuple = Nothing
            HOperatorSet.GetFullMatrix(Kakko2ID, XYZa)
            Points(i).X = BTuple.TupleSelect(XYZa, 0).D
            Points(i).Y = BTuple.TupleSelect(XYZa, 1).D
            Points(i).Z = BTuple.TupleSelect(XYZa, 2).D
        Next
    End Sub

    ' Pak,Qak,Rakの要素と再投影誤差を計算
    Public Sub CalcReProjectionError()
        Dim i As Integer
        Dim j As Integer
        Dim ta As Double
        Dim tb As Double
        dblE = 0
        dbl_SqrtE = 0
        N = 0
        For i = 1 To PointNum
            Dim tmpPoint As Point3D = Points(i)
            For j = 1 To ImageNum
                If Imat(i, j) = 1 Then
                    CalcPQRUVDHXHYak(i, j, tmpPoint)

                    ta = Pak(i, j) / Rak(i, j) - HXak(i, j) / F0
                    tb = Qak(i, j) / Rak(i, j) - HYak(i, j) / F0

                    dblE = dblE + ta * ta + tb * tb
                    dbl_SqrtE = dbl_SqrtE + Math.Sqrt(ta * ta + tb * tb) * F0
                    N += 1
                End If
            Next
        Next
    End Sub

    Public Sub CalcReProjectionError_Para()
        ' Dim i As Integer
        
        dblE = 0
        dbl_SqrtE = 0
        N = 0
        Parallel.For(1, PointNum + 1, Sub(i)

                                          Dim tmpPoint As Point3D = Points(i)
                                          For j As Integer = 1 To ImageNum
                                              If Imat(i, j) = 1 Then
                                                  Dim ta As Double
                                                  Dim tb As Double
                                                  CalcPQRUVDHXHYak(i, j, tmpPoint)

                                                  ta = Pak(i, j) / Rak(i, j) - HXak(i, j) / F0
                                                  tb = Qak(i, j) / Rak(i, j) - HYak(i, j) / F0

                                                  dblE = dblE + ta * ta + tb * tb
                                                  dbl_SqrtE = dbl_SqrtE + Math.Sqrt(ta * ta + tb * tb) * F0
                                                  N += 1
                                              End If
                                          Next
                                      End Sub)
    End Sub

    Private Sub CalcPQRUVDHXHYak(ByVal i As Integer, ByVal j As Integer, ByVal tmpPoint As Point3D)
        Dim tmpPmat As PMat = Images(j).P
        Pak(i, j) = tmpPmat.P11 * tmpPoint.X + tmpPmat.P12 * tmpPoint.Y + tmpPmat.P13 * tmpPoint.Z + tmpPmat.P14
        Qak(i, j) = tmpPmat.P21 * tmpPoint.X + tmpPmat.P22 * tmpPoint.Y + tmpPmat.P23 * tmpPoint.Z + tmpPmat.P24
        Rak(i, j) = tmpPmat.P31 * tmpPoint.X + tmpPmat.P32 * tmpPoint.Y + tmpPmat.P33 * tmpPoint.Z + tmpPmat.P34

        Uak(i, j) = (ImagePoints(i, j).x - Images(j).CNP.U0) * Images(j).CNP.Sx
        Vak(i, j) = (ImagePoints(i, j).y - Images(j).CNP.V0) * Images(j).CNP.Sy
        Dak(i, j) = Uak(i, j) * Uak(i, j) + Vak(i, j) * Vak(i, j)
        HXak(i, j) = Uak(i, j) + _
                     Uak(i, j) * (Images(j).CNP.K1 * Dak(i, j) + Images(j).CNP.K2 * Dak(i, j) * Dak(i, j) + Images(j).CNP.K3 * Dak(i, j) * Dak(i, j) * Dak(i, j)) + _
                    +2 * Images(j).CNP.P2 * Uak(i, j) * Vak(i, j) + _
                    Images(j).CNP.P1 * (Dak(i, j) + 2 * Uak(i, j) * Uak(i, j))
        HYak(i, j) = Vak(i, j) + _
                     Vak(i, j) * (Images(j).CNP.K1 * Dak(i, j) + Images(j).CNP.K2 * Dak(i, j) * Dak(i, j) + Images(j).CNP.K3 * Dak(i, j) * Dak(i, j) * Dak(i, j)) + _
                    +Images(j).CNP.P2 * (Dak(i, j) + 2 * Vak(i, j) * Vak(i, j)) + _
                    2 * Images(j).CNP.P1 * Vak(i, j) * Uak(i, j)
        HXak(i, j) = HXak(i, j) / Images(j).CNP.Sx + Images(j).CNP.U0
        HYak(i, j) = HYak(i, j) / Images(j).CNP.Sy + Images(j).CNP.V0
    End Sub


    Public Sub Calc_dEmat()
        Dim i As Integer
        Dim j As Integer

        Dim t As Integer = 0
        For i = 1 To PointNum       '三次元点に対する一次微分要素
            For j = 1 To PointHensu
                t = t + 1
                dEmat(t) = Calc_dE_dEpsK_Points(i, j)
                If dEmat(t) = 0 Then
                    Dim s As String = ""
                End If
            Next
        Next

        For i = 1 To ImageNum       '画像の外部パラメータに対する一次微分要素

            For j = 1 To CamGaibuHensu

                t = t + 1
                If FirstImageIndex = i Then
                    RemoveRowCol(j) = t
                End If
                If SecondImageIndex = i Then
                    If SecondImageXorY = j Then
                        RemoveRowCol(7) = t
                    End If
                End If
                Select Case j
                    Case 1
                        dEmat(t) = Calc_dE_dEpsK_T1(i)
                    Case 2
                        dEmat(t) = Calc_dE_dEpsK_T2(i)
                    Case 3
                        dEmat(t) = Calc_dE_dEpsK_T3(i)
                    Case 4
                        dEmat(t) = Calc_dE_dEpsK_W1(i)
                    Case 5
                        dEmat(t) = Calc_dE_dEpsK_W2(i)
                    Case 6
                        dEmat(t) = Calc_dE_dEpsK_W3(i)
                End Select
            Next
        Next
        Dim ic As Integer
        For ic = 1 To CameraNum

            Select Case CamNaibuHensu   '画像の内部パラメータに対する一次微分要素
                Case 3
                    t = t + 1
                    dEmat(t) = Calc_dE_dEpsK_Focal_AllImage(ic)
                    t = t + 1
                    dEmat(t) = Calc_dE_dEpsK_U0_AllImage(ic)
                    t = t + 1
                    dEmat(t) = Calc_dE_dEpsK_V0_AllImage(ic)
                Case 6
                    t = t + 1
                    dEmat(t) = Calc_dE_dEpsK_Focal_AllImage(ic)
                    t = t + 1
                    dEmat(t) = Calc_dE_dEpsK_U0_AllImage(ic)
                    t = t + 1
                    dEmat(t) = Calc_dE_dEpsK_V0_AllImage(ic)
                    t = t + 1
                    dEmat(t) = Calc_dE_dEpsK_K1_AllImage(ic)
                    t = t + 1
                    dEmat(t) = Calc_dE_dEpsK_K2_AllImage(ic)
                    t = t + 1
                    dEmat(t) = Calc_dE_dEpsK_K3_AllImage(ic)
                Case 8
                    t = t + 1
                    dEmat(t) = Calc_dE_dEpsK_Focal_AllImage(ic)
                    t = t + 1
                    dEmat(t) = Calc_dE_dEpsK_U0_AllImage(ic)
                    t = t + 1
                    dEmat(t) = Calc_dE_dEpsK_V0_AllImage(ic)
                    t = t + 1
                    dEmat(t) = Calc_dE_dEpsK_K1_AllImage(ic)
                    t = t + 1
                    dEmat(t) = Calc_dE_dEpsK_K2_AllImage(ic)
                    t = t + 1
                    dEmat(t) = Calc_dE_dEpsK_K3_AllImage(ic)
                    t = t + 1
                    dEmat(t) = Calc_dE_dEpsK_P1_AllImage(ic)
                    t = t + 1
                    dEmat(t) = Calc_dE_dEpsK_P2_AllImage(ic)
            End Select

        Next
        System.Array.Sort(RemoveRowCol)
    End Sub


    Public Sub Calc_dEmat_para()
    
        Parallel.Invoke(
            Sub()
                Dim t As Integer = 0
                Dim i As Integer
                Dim j As Integer
                For i = 1 To PointNum       '三次元点に対する一次微分要素
                    For j = 1 To PointHensu
                        t = t + 1
                        dEmat(t) = Calc_dE_dEpsK_Points(i, j)
                    Next
                Next
            End Sub,
            Sub()
                Dim t As Integer = PointNum * PointHensu
                Dim i As Integer
                Dim j As Integer
                For i = 1 To ImageNum       '画像の外部パラメータに対する一次微分要素
                    For j = 1 To CamGaibuHensu

                        t = t + 1
                        If FirstImageIndex = i Then
                            RemoveRowCol(j) = t
                        End If
                        If SecondImageIndex = i Then
                            If SecondImageXorY = j Then
                                RemoveRowCol(7) = t
                            End If
                        End If
                        Select Case j
                            Case 1
                                dEmat(t) = Calc_dE_dEpsK_T1(i)
                            Case 2
                                dEmat(t) = Calc_dE_dEpsK_T2(i)
                            Case 3
                                dEmat(t) = Calc_dE_dEpsK_T3(i)
                            Case 4
                                dEmat(t) = Calc_dE_dEpsK_W1(i)
                            Case 5
                                dEmat(t) = Calc_dE_dEpsK_W2(i)
                            Case 6
                                dEmat(t) = Calc_dE_dEpsK_W3(i)
                        End Select
                    Next
                Next
            End Sub,
            Sub()
                Dim t As Integer = PointNum * PointHensu + ImageNum * CamGaibuHensu
                Dim ic As Integer
                For ic = 1 To CameraNum
                    Select Case CamNaibuHensu   '画像の内部パラメータに対する一次微分要素
                        Case 3
                            t = t + 1
                            dEmat(t) = Calc_dE_dEpsK_Focal_AllImage(ic)
                            t = t + 1
                            dEmat(t) = Calc_dE_dEpsK_U0_AllImage(ic)
                            t = t + 1
                            dEmat(t) = Calc_dE_dEpsK_V0_AllImage(ic)
                        Case 6
                            t = t + 1
                            dEmat(t) = Calc_dE_dEpsK_Focal_AllImage(ic)
                            t = t + 1
                            dEmat(t) = Calc_dE_dEpsK_U0_AllImage(ic)
                            t = t + 1
                            dEmat(t) = Calc_dE_dEpsK_V0_AllImage(ic)
                            t = t + 1
                            dEmat(t) = Calc_dE_dEpsK_K1_AllImage(ic)
                            t = t + 1
                            dEmat(t) = Calc_dE_dEpsK_K2_AllImage(ic)
                            t = t + 1
                            dEmat(t) = Calc_dE_dEpsK_K3_AllImage(ic)
                        Case 8
                            t = t + 1
                            dEmat(t) = Calc_dE_dEpsK_Focal_AllImage(ic)
                            t = t + 1
                            dEmat(t) = Calc_dE_dEpsK_U0_AllImage(ic)
                            t = t + 1
                            dEmat(t) = Calc_dE_dEpsK_V0_AllImage(ic)
                            t = t + 1
                            dEmat(t) = Calc_dE_dEpsK_K1_AllImage(ic)
                            t = t + 1
                            dEmat(t) = Calc_dE_dEpsK_K2_AllImage(ic)
                            t = t + 1
                            dEmat(t) = Calc_dE_dEpsK_K3_AllImage(ic)
                            t = t + 1
                            dEmat(t) = Calc_dE_dEpsK_P1_AllImage(ic)
                            t = t + 1
                            dEmat(t) = Calc_dE_dEpsK_P2_AllImage(ic)
                    End Select
                Next
            End Sub
        )
   
        System.Array.Sort(RemoveRowCol)
    End Sub


    Public Sub Calc_EFGmat()
        '  Dim i As Integer
        '  Dim j As Integer
        'Dim i1 As Integer
        ' Dim i2 As Integer
        '   Dim j1 As Integer
        '  Dim j2 As Integer
        '  Dim t As Integer
        ''Dim Ei As Integer
        ''Dim Ej As Integer
        ''Dim tmpVal As Double = 0
        ''Dim tmpflg As Boolean = False
        '三次元点同士に対する二次微分要素
        'For i1 = 1 To PointHensu
        '    For i2 = i1 To PointHensu
        '        For i = 1 To PointNum
        '            'Ei = (i - 1) * PointHensu + i1
        '            'Ej = (i - 1) * PointHensu + i2
        '            'ddEmat(Ei, Ej) = Calc_ddE_dEpsKL_PointsOnly(i, i1, i2)
        '            'ddEmat(Ej, Ei) = ddEmat(Ei, Ej)
        '            Ei = (i - 1) * PointHensu + i1
        '            Ej = i2
        '            tmpVal = Calc_ddE_dEpsKL_PointsOnly(i, i1, i2)
        '            Emat(Ei, Ej) = tmpVal
        '            Ei = (i - 1) * PointHensu + i2
        '            Ej = i1
        '            Emat(Ei, Ej) = tmpVal
        '        Next
        '    Next
        'Next
        Calc_Emat()
        ' t = PointNum * PointHensu
        '画像同士に対する二次微分要素
        'For j1 = 1 To CamGaibuHensu
        '    For j2 = j1 To CamGaibuHensu
        '        For j = 1 To ImageNum
        '            'Ei = t + (j - 1) * CamGaibuHensu + j1
        '            'Ej = t + (j - 1) * CamGaibuHensu + j2
        '            'ddEmat(Ei, Ej) = Calc_ddE_dEpsKL_ImageOnly(j, j1, j2)
        '            'ddEmat(Ej, Ei) = ddEmat(Ei, Ej)
        '            Ei = (j - 1) * CamGaibuHensu + j1
        '            Ej = (j - 1) * CamGaibuHensu + j2
        '            Gmat(Ei, Ej) = Calc_ddE_dEpsKL_ImageOnly(j, j1, j2)
        '            Gmat(Ej, Ei) = Gmat(Ei, Ej)
        '        Next
        '    Next
        'Next
        Calc_Gmat_GaibuToGaibu()
        '三次元点と画像同士に対する二次微分要素()

        'For i1 = 1 To PointHensu
        '    For j1 = 1 To CamGaibuHensu
        '        For i = 1 To PointNum
        '            For j = 1 To ImageNum
        '                If Imat(i, j) = 1 Then
        '                    'Ei = (i - 1) * PointHensu + i1
        '                    'Ej = t + (j - 1) * CamGaibuHensu + j1
        '                    'ddEmat(Ei, Ej) = Calc_ddE_dEpsKL_PointsToImage(i, j, i1, j1)
        '                    'ddEmat(Ej, Ei) = ddEmat(Ei, Ej)
        '                    Ei = (i - 1) * PointHensu + i1
        '                    Ej = (j - 1) * CamGaibuHensu + j1
        '                    'Fmat(Ei, Ej) = Calc_ddE_dEpsKL_PointsToImage(i, j, i1, j1)
        '                    Fmat(Ei)(Ej) = Calc_ddE_dEpsKL_PointsToImage(i, j, i1, j1)
        '                End If
        '            Next
        '        Next
        '    Next
        'Next
        Calc_Fmat_PointToGaibu()

        If CamGaibuHensu = 6 And CamNaibuHensu > 0 Then
            'Dim n1 As Integer
            ''   Dim n2 As Integer
            ''三次元点と画像同士(naibu)に対する二次微分要素()
            'For i1 = 1 To PointHensu
            '    For n1 = 1 To CamNaibuHensu
            '        For i = 1 To PointNum
            '            'Ei = (i - 1) * PointHensu + i1
            '            'Ej = t + ImageNum * CamGaibuHensu + n1
            '            'ddEmat(Ei, Ej) = Calc_ddE_dEpsKL_PointsToImageNaibu(i, i1, n1)
            '            'ddEmat(Ej, Ei) = ddEmat(Ei, Ej)
            '            Ei = (i - 1) * PointHensu + i1
            '            Ej = ImageNum * CamGaibuHensu + n1
            '            ' Fmat(Ei, Ej) = Calc_ddE_dEpsKL_PointsToImageNaibu(i, i1, n1)
            '            Fmat(Ei)(Ej) = Calc_ddE_dEpsKL_PointsToImageNaibu(i, i1, n1)
            '        Next
            '    Next
            'Next
            Calc_Fmat_PointToNaibu()

            '画像同士(gaibutonaibu)に対する二次微分要素
            'For j1 = 1 To CamGaibuHensu
            '    For n1 = 1 To CamNaibuHensu
            '        For j = 1 To ImageNum
            '            'Ei = t + (j - 1) * CamGaibuHensu + j1
            '            'Ej = t + ImageNum * CamGaibuHensu + n1
            '            'ddEmat(Ei, Ej) = Calc_ddE_dEpsKL_GaibuToNaibu(j, j1, n1)
            '            'ddEmat(Ej, Ei) = ddEmat(Ei, Ej)
            '            Ei = (j - 1) * CamGaibuHensu + j1
            '            Ej = ImageNum * CamGaibuHensu + n1
            '            Gmat(Ei, Ej) = Calc_ddE_dEpsKL_GaibuToNaibu(j, j1, n1)
            '            Gmat(Ej, Ei) = Gmat(Ei, Ej)
            '        Next
            '    Next
            'Next
            Calc_Gmat_GaibuToNaibu()

            '画像同士(naibutonaibu)に対する二次微分要素
            'For n1 = 1 To CamNaibuHensu
            '    For n2 = n1 To CamNaibuHensu
            '        'Ei = t + ImageNum * CamGaibuHensu + n1
            '        'Ej = t + ImageNum * CamGaibuHensu + n2
            '        'ddEmat(Ei, Ej) = Calc_ddE_dEpsKL_NaibuToNaibu(n1, n2)
            '        'ddEmat(Ej, Ei) = ddEmat(Ei, Ej)
            '        Ei = ImageNum * CamGaibuHensu + n1 - KoteiHensu - 1
            '        Ej = ImageNum * CamGaibuHensu + n2 - KoteiHensu - 1
            '        Gmat(Ei, Ej) = Calc_ddE_dEpsKL_NaibuToNaibu(n1, n2)
            '        Gmat(Ej, Ei) = Gmat(Ei, Ej)
            '    Next
            'Next
            Calc_Gmat_NaibuToNaibu()
        End If

    End Sub


    Public Sub Calc_EFGmat_para()

        Parallel.Invoke(
            Sub()
                Calc_Emat()
            End Sub,
            Sub()
                Calc_Gmat_GaibuToGaibu()
            End Sub,
            Sub()
                Calc_Fmat_PointToGaibu()
            End Sub,
            Sub()
                Calc_Fmat_PointToNaibu()
            End Sub,
            Sub()
                Calc_Gmat_GaibuToNaibu()
            End Sub,
            Sub()
                Calc_Gmat_NaibuToNaibu()
            End Sub
        )

    End Sub
    Private Sub Calc_Emat()
        Dim i As Integer
        Dim i1 As Integer
        Dim i2 As Integer
        '  Dim t As Integer
        ''Dim Ei As Integer
        ''Dim Ej As Integer
        Dim tmpVal As Double = 0
        '三次元点同士に対する二次微分要素
        For i = 1 To PointNum
            If i = 61 Then
                i = i
            End If
            For i1 = 1 To PointHensu
                For i2 = i1 To PointHensu

                    'Ei = (i - 1) * PointHensu + i1
                    'Ej = (i - 1) * PointHensu + i2
                    'ddEmat(Ei, Ej) = Calc_ddE_dEpsKL_PointsOnly(i, i1, i2)
                    'ddEmat(Ej, Ei) = ddEmat(Ei, Ej)
                    'Ei = (i - 1) * PointHensu + i1
                    'Ej = i2
                    tmpVal = Calc_ddE_dEpsKL_PointsOnly(i, i1, i2)
                    'Emat(Ei, Ej) = tmpVal
                    'Ei = (i - 1) * PointHensu + i2
                    'Ej = i1
                    'Emat(Ei, Ej) = tmpVal
                    lstEmat(i - 1)(i1 - 1, i2 - 1) = tmpVal
                    lstEmat(i - 1)(i2 - 1, i1 - 1) = tmpVal
                Next
            Next
        Next
    End Sub

    Private Sub Calc_Fmat_PointToGaibu()
        Dim i As Integer
        Dim j As Integer
        Dim i1 As Integer
        Dim j1 As Integer
        'Dim Ei As Integer
        Dim Ej As Integer

        '三次元点と画像同士に対する二次微分要素
        Dim tj As Integer = 0
        Dim tj1 As Integer = 0
        For j = 1 To ImageNum
            For i = 1 To PointNum
                If Imat(i, j) = 1 Then
                    If FirstImageIndex = j Then
                        tj = 1
                        Continue For
                    End If
                    For i1 = 1 To PointHensu
                        For j1 = 1 To CamGaibuHensu
                            If SecondImageIndex = j And SecondImageXorY = j1 Then
                                tj1 = 1
                                Continue For
                            End If

                            'Ei = (i - 1) * PointHensu + i1
                            'Ej = t + (j - 1) * CamGaibuHensu + j1
                            'ddEmat(Ei, Ej) = Calc_ddE_dEpsKL_PointsToImage(i, j, i1, j1)
                            'ddEmat(Ej, Ei) = ddEmat(Ei, Ej)
                            ' Ei = (i - 1) * PointHensu + i1
                            'Ej = (j - 1) * CamGaibuHensu + j1
                            Ej = (j - 1 - tj) * CamGaibuHensu + j1 - 1 - tj1
                            'Fmat(Ei, Ej) = Calc_ddE_dEpsKL_PointsToImage(i, j, i1, j1)
                            ' Fmat(Ei)(Ej) = Calc_ddE_dEpsKL_PointsToImage(i, j, i1, j1)
                            'lstFmat.Item(i - 1)(i1 - 1, Ej) = Calc_ddE_dEpsKL_PointsToImage(i, j, i1, j1)
                            Fmat(i - 1)(i1 - 1, Ej) = Calc_ddE_dEpsKL_PointsToImage(i, j, i1, j1)
                        Next
                    Next
                End If
            Next
        Next
    End Sub

    Private Sub Calc_Fmat_PointToNaibu()
        Dim i As Integer
        Dim i1 As Integer
        Dim ic As Integer
        '    Dim Ei As Integer
        Dim Ej As Integer
        Dim n1 As Integer
        '三次元点と画像同士(naibu)に対する二次微分要素()
        For i = 1 To PointNum
            For i1 = 1 To PointHensu
                For ic = 1 To CameraNum
                    For n1 = 1 To CamNaibuHensu

                        'Ei = (i - 1) * PointHensu + i1
                        'Ej = t + ImageNum * CamGaibuHensu + n1
                        'ddEmat(Ei, Ej) = Calc_ddE_dEpsKL_PointsToImageNaibu(i, i1, n1)
                        'ddEmat(Ej, Ei) = ddEmat(Ei, Ej)
                        '  Ei = (i - 1) * PointHensu + i1
                        Ej = ImageNum * CamGaibuHensu + n1 * ic - KoteiHensu - 1
                        ' Fmat(Ei, Ej) = Calc_ddE_dEpsKL_PointsToImageNaibu(i, i1, n1)
                        'Fmat(Ei)(Ej) = Calc_ddE_dEpsKL_PointsToImageNaibu(i, i1, n1)
                        'lstFmat(i - 1)(i1 - 1, Ej) = Calc_ddE_dEpsKL_PointsToImageNaibu(i, i1, n1)
                        Fmat(i - 1)(i1 - 1, Ej) = Calc_ddE_dEpsKL_PointsToImageNaibu(i, i1, ic, n1)
                    Next
                Next
            Next
        Next
    End Sub
    Private Sub Calc_Gmat_GaibuToGaibu()
        Dim j As Integer
        Dim j1 As Integer
        Dim j2 As Integer
        Dim Ei As Integer
        Dim Ej As Integer
        ' t = PointNum * PointHensu
        '画像同士に対する二次微分要素
        Dim tj As Integer = 0
        Dim tj12 As Integer = 0
        ' Dim tj2 As Integer = 0
        For j = 1 To ImageNum
            If FirstImageIndex = j Then
                tj = 1
                Continue For
            End If
            For j1 = 1 To CamGaibuHensu
                If SecondImageIndex = j And SecondImageXorY = j1 Then
                    tj12 = 1
                    Continue For
                End If
                For j2 = j1 To CamGaibuHensu
                    If SecondImageIndex = j And SecondImageXorY = j2 Then
                        tj12 = 1
                        Continue For
                    End If
                    'Ei = t + (j - 1) * CamGaibuHensu + j1
                    'Ej = t + (j - 1) * CamGaibuHensu + j2
                    'ddEmat(Ei, Ej) = Calc_ddE_dEpsKL_ImageOnly(j, j1, j2)
                    'ddEmat(Ej, Ei) = ddEmat(Ei, Ej)
                    Ei = (j - 1 - tj) * CamGaibuHensu + j1 - tj12 - 1
                    Ej = (j - 1 - tj) * CamGaibuHensu + j2 - tj12 - 1
                    Gmat(Ei, Ej) = Calc_ddE_dEpsKL_ImageOnly(j, j1, j2)
                    Gmat(Ej, Ei) = Gmat(Ei, Ej)
                Next
            Next
        Next
    End Sub
    Private Sub Calc_Gmat_GaibuToNaibu()
        Dim j As Integer
        Dim j1 As Integer
        Dim Ei As Integer
        Dim Ej As Integer
        Dim n1 As Integer
        Dim ic As Integer
        '画像同士(gaibutonaibu)に対する二次微分要素
        Dim tj As Integer = 0
        Dim tj1 As Integer = 0
        For j = 1 To ImageNum
            If FirstImageIndex = j Then
                tj = 1
                Continue For
            End If
            For j1 = 1 To CamGaibuHensu
                If SecondImageIndex = j And SecondImageXorY = j1 Then
                    tj1 = 1
                    Continue For
                End If
                For ic = 1 To CameraNum
                    For n1 = 1 To CamNaibuHensu
                        'Ei = t + (j - 1) * CamGaibuHensu + j1
                        'Ej = t + ImageNum * CamGaibuHensu + n1
                        'ddEmat(Ei, Ej) = Calc_ddE_dEpsKL_GaibuToNaibu(j, j1, n1)
                        'ddEmat(Ej, Ei) = ddEmat(Ei, Ej)
                        Ei = (j - 1 - tj) * CamGaibuHensu + j1 - tj1 - 1
                        Ej = ImageNum * CamGaibuHensu + n1 * ic - KoteiHensu - 1
                        Gmat(Ei, Ej) = Calc_ddE_dEpsKL_GaibuToNaibu(j, j1, ic, n1)
                        Gmat(Ej, Ei) = Gmat(Ei, Ej)
                    Next
                Next
            Next
        Next
    End Sub

    Private Sub Calc_Gmat_NaibuToNaibu()
        Dim Ei As Integer
        Dim Ej As Integer
        Dim n1 As Integer
        Dim n2 As Integer
        Dim ic As Integer
        '画像同士(naibutonaibu)に対する二次微分要素
        For ic = 1 To CameraNum
            For n1 = 1 To CamNaibuHensu
                For n2 = n1 To CamNaibuHensu
                    'Ei = t + ImageNum * CamGaibuHensu + n1
                    'Ej = t + ImageNum * CamGaibuHensu + n2
                    'ddEmat(Ei, Ej) = Calc_ddE_dEpsKL_NaibuToNaibu(n1, n2)
                    'ddEmat(Ej, Ei) = ddEmat(Ei, Ej)
                    Ei = ImageNum * CamGaibuHensu + n1 * ic - KoteiHensu - 1
                    Ej = ImageNum * CamGaibuHensu + n2 * ic - KoteiHensu - 1
                    Gmat(Ei, Ej) = Calc_ddE_dEpsKL_NaibuToNaibu(ic, n1, n2)
                    Gmat(Ej, Ei) = Gmat(Ei, Ej)
                Next
            Next
        Next
    End Sub
#If False Then
    Public Sub Calc_EFGmat_faster()
        Dim i As Integer 'PointNumのインディクス
        Dim j As Integer 'ImageNumのインディクス
        Dim i1 As Integer 'PointHensuのインディクス1
        Dim i2 As Integer 'PointHensuのインディクス2
        Dim j1 As Integer 'CamGaibuHensuのインディクス1
        Dim j2 As Integer 'CamGaibuHensuのインディクス2
        Dim n1 As Integer 'CamNaibuHensuのインディクス1
        Dim n2 As Integer 'CamNaibuHensuのインディクス2
        '  Dim t As Integer
        Dim Ei As Integer '配列のインディクス１
        Dim Ej As Integer '配列のインディクス2
        Dim tmpVal As Double = 0
        For i = 1 To PointNum
            For j = 1 To ImageNum
                If Imat(i, j) = 1 Then
                    For i1 = 1 To PointHensu
                        '三次元点同士に対する二次微分要素
                        For i2 = i1 To PointHensu
                            Ei = (i - 1) * PointHensu + i1
                            Ej = i2
                            tmpVal = Calc_ddE_dEpsKL_PointsOnly_Faster(i, j, i1, i2)
                            Emat(Ei, Ej) += tmpVal
                            Ei = (i - 1) * PointHensu + i2
                            Ej = i1
                            Emat(Ei, Ej) += tmpVal
                        Next
                        '三次元点と画像同士に対する二次微分要素()
                        For j1 = 1 To CamGaibuHensu
                            Ei = (i - 1) * PointHensu + i1
                            Ej = (j - 1) * CamGaibuHensu + j1
                            ' Fmat(Ei, Ej) += Calc_ddE_dEpsKL_PointsToImage(i, j, i1, j1)
                            Fmat(Ei)(Ej) += Calc_ddE_dEpsKL_PointsToImage(i, j, i1, j1)
                        Next
                        '三次元点と画像同士(naibu)に対する二次微分要素()
                        For n1 = 1 To CamNaibuHensu
                            Ei = (i - 1) * PointHensu + i1
                            Ej = ImageNum * CamGaibuHensu + n1
                            ' Fmat(Ei, Ej) += Calc_ddE_dEpsKL_PointsToImageNaibu_Faster(i, j, i1, n1)
                            Fmat(Ei)(Ej) += Calc_ddE_dEpsKL_PointsToImageNaibu_Faster(i, j, i1, n1)
                        Next
                    Next

                    '画像同士に対する二次微分要素
                    For j1 = 1 To CamGaibuHensu
                        '画像同士に対する二次微分要素
                        For j2 = j1 To CamGaibuHensu
                            Ei = (j - 1) * CamGaibuHensu + j1
                            Ej = (j - 1) * CamGaibuHensu + j2
                            Gmat(Ei, Ej) += Calc_ddE_dEpsKL_ImageOnly_Faster(i, j, j1, j2)
                            Gmat(Ej, Ei) = Gmat(Ei, Ej)
                        Next
                        '画像同士(gaibutonaibu)に対する二次微分要素
                        For n1 = 1 To CamNaibuHensu
                            Ei = (j - 1) * CamGaibuHensu + j1
                            Ej = ImageNum * CamGaibuHensu + n1
                            Gmat(Ei, Ej) += Calc_ddE_dEpsKL_GaibuToNaibu_Faster(i, j, j1, n1)
                            Gmat(Ej, Ei) = Gmat(Ei, Ej)
                        Next
                    Next

                    '画像同士の内部評定に対する二次微分要素
                    For n1 = 1 To CamNaibuHensu
                        For n2 = n1 To CamNaibuHensu
                            Ei = ImageNum * CamGaibuHensu + n1
                            Ej = ImageNum * CamGaibuHensu + n2
                            Gmat(Ei, Ej) += Calc_ddE_dEpsKL_NaibuToNaibu_Faster(i, j, n1, n2)
                            Gmat(Ej, Ei) = Gmat(Ei, Ej)
                        Next
                    Next
                End If
            Next
        Next
    End Sub


    Public Sub Calc_EFGmat_faster_para()
        '  Dim i As Integer 'PointNumのインディクス

        Parallel.
            For(1,
                PointNum + 1,
                Sub(i)
                    Dim j As Integer 'ImageNumのインディクス
                    Dim i1 As Integer 'PointHensuのインディクス1
                    Dim i2 As Integer 'PointHensuのインディクス2
                    Dim j1 As Integer 'CamGaibuHensuのインディクス1
                    Dim j2 As Integer 'CamGaibuHensuのインディクス2
                    Dim n1 As Integer 'CamNaibuHensuのインディクス1
                    Dim n2 As Integer 'CamNaibuHensuのインディクス2
                    '  Dim t As Integer
                    Dim Ei As Integer '配列のインディクス１
                    Dim Ej As Integer '配列のインディクス2
                    Dim tmpVal As Double = 0
                    For j = 1 To ImageNum
                        If Imat(i, j) = 1 Then
                            For i1 = 1 To PointHensu
                                '三次元点同士に対する二次微分要素
                                For i2 = i1 To PointHensu
                                    Ei = (i - 1) * PointHensu + i1
                                    Ej = i2
                                    tmpVal = Calc_ddE_dEpsKL_PointsOnly_Faster(i, j, i1, i2)
                                    Emat(Ei, Ej) += tmpVal
                                    Ei = (i - 1) * PointHensu + i2
                                    Ej = i1
                                    Emat(Ei, Ej) += tmpVal
                                Next
                                '三次元点と画像同士に対する二次微分要素()
                                For j1 = 1 To CamGaibuHensu
                                    Ei = (i - 1) * PointHensu + i1
                                    Ej = (j - 1) * CamGaibuHensu + j1
                                    ' Fmat(Ei, Ej) += Calc_ddE_dEpsKL_PointsToImage(i, j, i1, j1)
                                    Fmat(Ei)(Ej) += Calc_ddE_dEpsKL_PointsToImage(i, j, i1, j1)
                                Next
                                '三次元点と画像同士(naibu)に対する二次微分要素()
                                For n1 = 1 To CamNaibuHensu
                                    Ei = (i - 1) * PointHensu + i1
                                    Ej = ImageNum * CamGaibuHensu + n1
                                    ' Fmat(Ei, Ej) += Calc_ddE_dEpsKL_PointsToImageNaibu_Faster(i, j, i1, n1)
                                    Fmat(Ei)(Ej) += Calc_ddE_dEpsKL_PointsToImageNaibu_Faster(i, j, i1, n1)
                                Next
                            Next

                            '画像同士に対する二次微分要素
                            For j1 = 1 To CamGaibuHensu
                                '画像同士に対する二次微分要素
                                For j2 = j1 To CamGaibuHensu
                                    Ei = (j - 1) * CamGaibuHensu + j1
                                    Ej = (j - 1) * CamGaibuHensu + j2
                                    Gmat(Ei, Ej) += Calc_ddE_dEpsKL_ImageOnly_Faster(i, j, j1, j2)
                                    Gmat(Ej, Ei) = Gmat(Ei, Ej)
                                Next
                                '画像同士(gaibutonaibu)に対する二次微分要素
                                For n1 = 1 To CamNaibuHensu
                                    Ei = (j - 1) * CamGaibuHensu + j1
                                    Ej = ImageNum * CamGaibuHensu + n1
                                    Gmat(Ei, Ej) += Calc_ddE_dEpsKL_GaibuToNaibu_Faster(i, j, j1, n1)
                                    Gmat(Ej, Ei) = Gmat(Ei, Ej)
                                Next
                            Next

                            '画像同士の内部評定に対する二次微分要素
                            For n1 = 1 To CamNaibuHensu
                                For n2 = n1 To CamNaibuHensu
                                    Ei = ImageNum * CamGaibuHensu + n1
                                    Ej = ImageNum * CamGaibuHensu + n2
                                    Gmat(Ei, Ej) += Calc_ddE_dEpsKL_NaibuToNaibu_Faster(i, j, n1, n2)
                                    Gmat(Ej, Ei) = Gmat(Ei, Ej)
                                Next
                            Next
                        End If
                    Next
                End Sub)

    End Sub
#End If

    Public Sub Calc_Hessmat(ByRef ddEmat(,) As Double)
        Dim i As Integer
        Dim j As Integer
        Dim i1 As Integer
        Dim i2 As Integer
        Dim j1 As Integer
        Dim j2 As Integer
        Dim t As Integer
        Dim Ei As Integer
        Dim Ej As Integer
        Dim tmpVal As Double = 0
        Dim tmpflg As Boolean = False
        '三次元点同士に対する二次微分要素
        For i1 = 1 To PointHensu
            For i2 = i1 To PointHensu
                For i = 1 To PointNum
                    Ei = (i - 1) * PointHensu + i1 - 1
                    Ej = (i - 1) * PointHensu + i2 - 1
                    ddEmat(Ei, Ej) = Calc_ddE_dEpsKL_PointsOnly(i, i1, i2)
                    ddEmat(Ej, Ei) = ddEmat(Ei, Ej)

                Next
            Next
        Next

        t = PointNum * PointHensu
        '画像同士に対する二次微分要素
        For j1 = 1 To CamGaibuHensu
            For j2 = j1 To CamGaibuHensu
                For j = 1 To ImageNum
                    Ei = t + (j - 1) * CamGaibuHensu + j1 - 1
                    Ej = t + (j - 1) * CamGaibuHensu + j2 - 1
                    ddEmat(Ei, Ej) = Calc_ddE_dEpsKL_ImageOnly(j, j1, j2)
                    ddEmat(Ej, Ei) = ddEmat(Ei, Ej)

                Next
            Next
        Next
        '三次元点と画像同士に対する二次微分要素()

        For i1 = 1 To PointHensu
            For j1 = 1 To CamGaibuHensu
                For i = 1 To PointNum
                    For j = 1 To ImageNum
                        If Imat(i, j) = 1 Then
                            Ei = (i - 1) * PointHensu + i1 - 1
                            Ej = t + (j - 1) * CamGaibuHensu + j1 - 1
                            ddEmat(Ei, Ej) = Calc_ddE_dEpsKL_PointsToImage(i, j, i1, j1)
                            ddEmat(Ej, Ei) = ddEmat(Ei, Ej)

                        End If
                    Next
                Next
            Next
        Next

        If CamGaibuHensu = 6 And CamNaibuHensu > 0 Then
            Dim n1 As Integer
            Dim n2 As Integer
            '三次元点と画像同士(naibu)に対する二次微分要素()
            For i1 = 1 To PointHensu
                For n1 = 1 To CamNaibuHensu
                    For i = 1 To PointNum
                        Ei = (i - 1) * PointHensu + i1 - 1
                        Ej = t + ImageNum * CamGaibuHensu + n1 - 1
                        ddEmat(Ei, Ej) = Calc_ddE_dEpsKL_PointsToImageNaibu(i, i1, 1, n1)
                        ddEmat(Ej, Ei) = ddEmat(Ei, Ej)

                    Next
                Next
            Next
            '画像同士(gaibutonaibu)に対する二次微分要素
            For j1 = 1 To CamGaibuHensu
                For n1 = 1 To CamNaibuHensu
                    For j = 1 To ImageNum
                        Ei = t + (j - 1) * CamGaibuHensu + j1 - 1
                        Ej = t + ImageNum * CamGaibuHensu + n1 - 1
                        ddEmat(Ei, Ej) = Calc_ddE_dEpsKL_GaibuToNaibu(j, j1, 1, n1)
                        ddEmat(Ej, Ei) = ddEmat(Ei, Ej)

                    Next
                Next
            Next

            '画像同士(naibutonaibu)に対する二次微分要素
            For n1 = 1 To CamNaibuHensu
                For n2 = n1 To CamNaibuHensu
                    Ei = t + ImageNum * CamGaibuHensu + n1 - 1
                    Ej = t + ImageNum * CamGaibuHensu + n2 - 1
                    ddEmat(Ei, Ej) = Calc_ddE_dEpsKL_NaibuToNaibu(1, n1, n2)
                    ddEmat(Ej, Ei) = ddEmat(Ei, Ej)
                Next
            Next

        End If

    End Sub

    Public Sub Calc_DeltaMat_yti(ByVal c As Double, ByRef arrDeltaMat() As Double)
        Dim GMatID(,) As Double = {}
        Dim DFMatID(,) As Double = {}
        'Dim FallMatID As HTuple = Nothing
        '  Dim EallMatID As HTuple = Nothing
        'Dim FaMatID As HTuple = Nothing
        'Dim EaInvMatID As HTuple = Nothing
        'Dim DeltaAEMatID As HTuple = Nothing
        'Dim arrFaMatID(PointNum) As HTuple
        Dim arrEaInvMatID(PointNum)(,) As Double
        Dim arrDeltaAEMatID(PointNum)(,) As Double

        '  Dim DeltaEpsPMatID As HTuple = Nothing
        Dim LeftMatID(,) As Double = {}
        Dim RightMatID(,) As Double = {}
        'Dim SumLeftMatID As HTuple = Nothing
        'Dim SumRightMatID As HTuple = Nothing
        'Dim MultMatId1 As HTuple = Nothing
        'Dim MultMatId2 As HTuple = Nothing
        'Dim AddMatID As HTuple = Nothing
        'Dim tmpMatID As HTuple = Nothing
        Dim i As Integer
        Dim j As Integer

        Static t As Integer = 0
        t = t + 1

        ReDim arrDeltaMat(Shikisuu - KoteiHensu - 1)

        StopWatchResetStart()

        Calc_G_mat_yti(c, GMatID)
        ' Calc_DF_Mat(DFMatID)
        Calc_DF_Mat_yti(DFMatID)
        ' Calc_Fall_mat(FallMatID)
        '   Calc_EaInvAll_mat(c, EallMatID)
        For i = 1 To PointNum
            ' Dim FaTMatID(,) As Double = {}
            Dim EaInvMatID(,) As Double = {}
            Dim DeltaAEMatID(,) As Double = {}
            'Dim MultMat1(,) As Double = {}
            'Dim MultMat2(,) As Double = {}
            Calc_Delta_a_E_mat_yti(i, DeltaAEMatID)
           
            Calc_EaInv_mat_yti(c, i, EaInvMatID)
            arrEaInvMatID(i) = EaInvMatID
            arrDeltaAEMatID(i) = DeltaAEMatID
        Next
        JikanMonitor("バンドル更新値計算１： ")


        StopWatchResetStart()

        Dim nG As Integer = Shikisuu - KoteiHensu - PointNum * PointHensu
        'GC.Collect()
        'GC.WaitForPendingFinalizers()

        Dim arrSumLeftMatID(nG - 1, nG - 1) As Double
        Dim arrSumRightMatID(nG - 1, 0) As Double

        'Stopwatchオブジェクトを作成する 
        Dim sw As New System.Diagnostics.Stopwatch()
        'ストップウォッチを開始する 
        sw.Start()

        'Del Kiryu 20161207 バンドル調整用行列算出の並列処理を削除(計算結果の格納順序が実行ごとに変わるため)
        'Parallel.For(1, PointNum + 1, Sub(ii)
        '                                  'Dim MultMat1(,) As Double = {}
        '                                  'MMult_ATB(Fmat(ii - 1), arrEaInvMatID(ii), MultMat1)
        '                                  'MMultAdd2_Both(MultMat1, Fmat(ii - 1), arrDeltaAEMatID(ii), arrSumLeftMatID, arrSumRightMatID)
        '                                  MCalcLeftRightMat(Fmat(ii - 1), arrEaInvMatID(ii), arrDeltaAEMatID(ii), arrSumLeftMatID, arrSumRightMatID)
        '                              End Sub)


        'Edit Kiryu 20161207　バンドル調整用行列算出の並列処理を直列処理に変更(計算結果の格納順序を決めるよう変更すれば再度並列化可能)
        For ii As Integer = 1 To PointNum
            MCalcLeftRightMat(Fmat(ii - 1), arrEaInvMatID(ii), arrDeltaAEMatID(ii), arrSumLeftMatID, arrSumRightMatID)
        Next

        'ストップウォッチを止める 
        sw.Stop()

        'MCalcLeftRightMat実行時間計測
        'Dim SwOut As New System.IO.StreamWriter("C:\01_VFORM_Projects\MCalcLeftRightMat実行時間\time" + CStr(t) + ".txt", False, System.Text.Encoding.GetEncoding("shift_jis"))
        'SwOut.Write(CStr(sw.Elapsed.TotalSeconds))
        'SwOut.Close()
       
        '結果を表示する 
        Console.WriteLine(sw.Elapsed)

        'For i = 1 To PointNum
        '    MCalcLeftRightMat(Fmat(i - 1), arrEaInvMatID(i), arrDeltaAEMatID(i), arrSumLeftMatID, arrSumRightMatID)
        'Next

        'For ii As Integer = 1 To PointNum
        '    'Dim MultMat1(,) As Double = {}
        '    'MMult_ATB(Fmat(ii - 1), arrEaInvMatID(ii), MultMat1)
        '    'MMultAdd2_Both(MultMat1, Fmat(ii - 1), arrDeltaAEMatID(ii), arrSumLeftMatID, arrSumRightMatID)
        '    MCalcLeftRightMat(Fmat(ii - 1), arrEaInvMatID(ii), arrDeltaAEMatID(ii), arrSumLeftMatID, arrSumRightMatID)
        'Next
        JikanMonitor("バンドル更新値計算2： ")

        StopWatchResetStart()
        '  HOperatorSet.SubMatrix(GMatID, SumLeftMatID, LeftMatID)
        MSub(GMatID, arrSumLeftMatID, LeftMatID)

        'HOperatorSet.SubMatrix(SumRightMatID, DFMatID, RightMatID)
        MSub(arrSumRightMatID, DFMatID, RightMatID)

        Dim arrDeltaEpsFMatID(nG - 1, 0) As Double

        'If MSolve_GJ(LeftMatID, RightMatID, arrDeltaEpsFMatID) <> MNORM Then
        '    MsgBox("ddd")

        Halcon_Solve(LeftMatID, RightMatID, arrDeltaEpsFMatID)
        '  End If

        ' HOperatorSet.CreateMatrix(PointHensu, 1, -1.0, tmpMatID)
        JikanMonitor("バンドル更新値計算3(solve)：")

        StopWatchResetStart()

        For i = 1 To PointNum

            Dim EaInvMatID(,) As Double = {}
            Dim DeltaAEMatID(,) As Double = {}
            Dim MultMat1(,) As Double = {}
            Dim MultMat2(,) As Double = {}
            Dim AddMat(,) As Double = {}

            EaInvMatID = arrEaInvMatID(i)
            DeltaAEMatID = arrDeltaAEMatID(i)

            'HOperatorSet.MultMatrix(FaMatID, DeltaEpsFMatID, "AB", MultMatId1)
            MMult(Fmat(i - 1), arrDeltaEpsFMatID, MultMat1)

            'HOperatorSet.AddMatrix(MultMatId1, DeltaAEMatID, AddMatID)
            MAdd(MultMat1, DeltaAEMatID, AddMat)

            ' HOperatorSet.MultMatrix(EaInvMatID, AddMatID, "AB", MultMatId2)
            MMult(EaInvMatID, AddMat, MultMat2)

            'HOperatorSet.MultElementMatrixMod(MultMatId2, tmpMatID)
            MScaleMod(MultMat2, -1.0)

            'HOperatorSet.SetSubMatrix(DeltaMatID, MultMatId2, (i - 1) * PointHensu, 0)
            For j = 0 To PointHensu - 1
                arrDeltaMat((i - 1) * PointHensu + j) = MultMat2(j, 0)
            Next

            'HOperatorSet.ClearMatrix(MultMatId2)
            'HOperatorSet.ClearMatrix(MultMatId1)
            'HOperatorSet.ClearMatrix(DeltaAEMatID)
            'HOperatorSet.ClearMatrix(EaInvMatID)
            ''HOperatorSet.ClearMatrix(FaMatID)
            'HOperatorSet.ClearMatrix(AddMatID)
        Next
        For j = PointHensu * PointNum To Shikisuu - KoteiHensu - 1
            arrDeltaMat(j) = arrDeltaEpsFMatID(j - PointHensu * PointNum, 0)
        Next

        ' HOperatorSet.SetSubMatrix(DeltaMatID, DeltaEpsFMatID, PointNum * PointHensu, 0)

        'HOperatorSet.CreateMatrix(Shikisuu - KoteiHensu, 1, arrDeltaMatID, DeltaMatID)

        JikanMonitor("バンドル更新値計算4:")

        'HOperatorSet.ClearMatrix(GMatID)
        'HOperatorSet.ClearMatrix(DFMatID)
        '  HOperatorSet.ClearMatrix(FallMatID)
        '  HOperatorSet.ClearMatrix(EallMatID)

        'HOperatorSet.ClearMatrix(DeltaEpsPMatID)
        'HOperatorSet.ClearMatrix(LeftMatID)
        'HOperatorSet.ClearMatrix(RightMatID)
        'HOperatorSet.ClearMatrix(SumLeftMatID)
        'HOperatorSet.ClearMatrix(SumRightMatID)
        'HOperatorSet.ClearMatrix(MultMatId1)
        'HOperatorSet.ClearMatrix(MultMatId2)
        'HOperatorSet.ClearMatrix(tmpMatID)
        'HOperatorSet.ClearMatrix(AddMatID)
    End Sub
    Private Sub Halcon_Solve(ByRef LeftMatId(,) As Double, ByRef RightMatID(,) As Double, ByRef arrDeltaEpsFMatID(,) As Double)
        Dim nG As Integer = Shikisuu - KoteiHensu - PointNum * PointHensu
        Dim i As Integer
        Dim j As Integer


        Dim LeftMatID_hal As HTuple = Nothing
        Dim RigthMatID_hal As HTuple = Nothing
        Dim DeltaEpsFMatID As HTuple = Nothing
        Dim DeltaEpsFMatID2 As HTuple = Nothing

        Dim t As Integer = 0
        Dim tmpObj(nG * nG - 1) As Double
        Dim tmpObj2(nG * nG - 1) As Double

        'add kiryu 20161209 SolveMatrix動作確認ファイル出力ナンバー用
        Static N As Integer = 0
        N = N + 1

        'SyncLock Op ' 排他制御

        For i = 0 To nG - 1
            For j = 0 To nG - 1
                tmpObj(t) = LeftMatId(i, j)
                t += 1
            Next
        Next
        HOperatorSet.CreateMatrix(nG, nG, tmpObj, LeftMatID_hal)
        ' HOperatorSet.WriteTuple(tmpObj, "C:\01_VFORM_Projects\SolveMatrixOut\Leftmatrixresult" + CStr(N) + ".tpl")

        ReDim tmpObj(nG - 1)
        For i = 0 To nG - 1
            tmpObj(i) = RightMatID(i, 0)
        Next
        HOperatorSet.CreateMatrix(nG, 1, tmpObj, RigthMatID_hal)
        'HOperatorSet.WriteTuple(tmpObj, "C:\01_VFORM_Projects\SolveMatrixOut\Rightmatrixresult" + CStr(N) + ".tpl")

        'HOperatorSet.SetFullMatrix()
        'HOperatorSet.GetFullMatrix(DeltaEpsFMatID, tmpObj)
        'HOperatorSet.WriteTuple(tmpObj, "C:\01_VFORM_Projects\DeltaEpsFmat_nothing.tpl")
        Try
            HOperatorSet.SolveMatrix(LeftMatID_hal, "general", 0, RigthMatID_hal, DeltaEpsFMatID)
            'HOperatorSet.SolveMatrix(LeftMatID_hal, "general", 0, RigthMatID_hal, DeltaEpsFMatID2)

        Catch ex As Exception
            HOperatorSet.SolveMatrix(LeftMatID_hal, "general", 0.00000000000000022204, RigthMatID_hal, DeltaEpsFMatID)
        End Try

        HOperatorSet.GetFullMatrix(DeltaEpsFMatID, tmpObj)
        'HOperatorSet.GetFullMatrix(DeltaEpsFMatID2, tmpObj2)

        'HOperatorSet.WriteTuple(tmpObj, "C:\01_VFORM_Projects\solvematrixresult.tpl")
        '20161260 Add kiryu solve出力値テスト用 
        'HOperatorSet.WriteTuple(tmpObj, "C:\01_VFORM_Projects\SolveMatrixOut1\solvematrixresult" + CStr(N) + ".tpl")
        'HOperatorSet.WriteTuple(tmpObj2, "C:\01_VFORM_Projects\SolveMatrixOut2\solvematrixresult" + CStr(N) + ".tpl")


        For i = 0 To nG - 1
            arrDeltaEpsFMatID(i, 0) = tmpObj(i)
        Next

        'End SyncLock

        HOperatorSet.ClearMatrix(DeltaEpsFMatID)
        HOperatorSet.ClearMatrix(LeftMatID_hal)
        HOperatorSet.ClearMatrix(RigthMatID_hal)
    End Sub
    Private Sub Calc_DF_Mat_yti(ByRef DF_MatID(,) As Double)
        Dim Ei As Integer
        Dim i As Integer
        Dim l As Integer = 1
        Dim t As Integer = 0
        ReDim DF_MatID((Shikisuu - KoteiHensu - PointNum * PointHensu) - 1, 0)
        For i = PointNum * PointHensu + 1 To Shikisuu
            If RemoveRowCol(l) = i Then
                If l < KoteiHensu Then
                    l += 1
                End If
            Else
                Ei = i
                DF_MatID(t, 0) = dEmat(Ei)
                t = t + 1
            End If
        Next
        ''GC.Collect()
        ''GC.WaitForPendingFinalizers()
        'HOperatorSet.CreateMatrix(Shikisuu - KoteiHensu - PointNum * PointHensu, 1, tmpobj, DF_MatID)
    End Sub

    Private Sub Calc_Delta_a_E_mat_yti(ByVal a As Integer, ByRef Delta_a_E_MatID(,) As Double)
        Dim Ei As Integer
        Dim i As Integer
        Dim t As Integer = 0

        ReDim Delta_a_E_MatID(PointHensu - 1, 0)

        For i = 1 To PointHensu
            Ei = (a - 1) * PointHensu + i
            Delta_a_E_MatID(t, 0) = dEmat(Ei)
            t = t + 1
        Next

    End Sub

    Private Sub Calc_EaInv_mat_yti(ByVal c As Double, ByVal a As Integer, ByRef EaMatID(,) As Double)
        Dim i1 As Integer
        Dim i2 As Integer
        Dim t As Integer = 0
        'GC.Collect()
        'GC.WaitForPendingFinalizers()
        Dim tmpobj(PointHensu - 1, PointHensu - 1) As Double
        'ReDim EaMatID(PointHensu - 1, PointHensu - 1)

        For i1 = 0 To PointHensu - 1
            For i2 = 0 To PointHensu - 1
                If i1 = i2 Then
                    tmpobj(i1, i2) = (1 + c) * lstEmat.Item(a - 1)(i1, i2)
                Else
                    tmpobj(i1, i2) = lstEmat.Item(a - 1)(i1, i2)
                End If
            Next
        Next

        MInvert(tmpobj, EaMatID)

    End Sub
    Private Sub Calc_G_mat_yti(ByVal c As Double, ByRef GMatID(,) As Double)
        Dim i As Integer
        Dim j As Integer
        ReDim GMatID(Shikisuu - KoteiHensu - PointNum * PointHensu - 1, Shikisuu - KoteiHensu - PointNum * PointHensu - 1)
        For i = 0 To Shikisuu - KoteiHensu - PointNum * PointHensu - 1
            For j = 0 To Shikisuu - KoteiHensu - PointNum * PointHensu - 1
                If i = j Then
                    GMatID(i, j) = (1 + c) * Gmat(i, j)
                Else
                    GMatID(i, j) = Gmat(i, j)
                End If
            Next
        Next

    End Sub

    Public Sub Calc_DeltaMat(ByVal c As Double, ByRef DeltaMatID As HTuple)
        Dim GMatID As HTuple = Nothing
        Dim DFMatID As HTuple = Nothing
        'Dim FallMatID As HTuple = Nothing
        '  Dim EallMatID As HTuple = Nothing
        'Dim FaMatID As HTuple = Nothing
        'Dim EaInvMatID As HTuple = Nothing
        'Dim DeltaAEMatID As HTuple = Nothing
        Dim arrFaMatID(PointNum) As HTuple
        Dim arrEaInvMatID(PointNum) As HTuple
        Dim arrDeltaAEMatID(PointNum) As HTuple
        Dim DeltaEpsFMatID As HTuple = Nothing
        ' Dim DeltaEpsPMatID As HTuple = Nothing
        Dim LeftMatID As HTuple = Nothing
        Dim RightMatID As HTuple = Nothing
        Dim SumLeftMatID As HTuple = Nothing
        Dim SumRightMatID As HTuple = Nothing
        Dim MultMatId1 As HTuple = Nothing
        Dim MultMatId2 As HTuple = Nothing
        Dim AddMatID As HTuple = Nothing
        Dim tmpMatID As HTuple = Nothing
        Dim i As Integer

        HOperatorSet.CreateMatrix(Shikisuu - KoteiHensu, 1, 0.0, DeltaMatID)
        Dim sw As New System.Diagnostics.Stopwatch()
        'カメラ結合（CTによる結合）
        sw.Reset()
        sw.Start()

        Calc_G_mat(c, GMatID)
        Calc_DF_Mat(DFMatID)
        ' Calc_Fall_mat(FallMatID)
        '   Calc_EaInvAll_mat(c, EallMatID)
        sw.Stop()
        Trace.WriteLine("バンドル更新値計算１：")
        Trace.WriteLine(sw.Elapsed.TotalSeconds & "秒")
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "JikanMonitor.mon",
                                         "バンドル更新値計算１： " & sw.Elapsed.TotalSeconds & "秒" & vbNewLine, True)
        sw.Reset()
        sw.Start()
        Dim nG As Integer = Shikisuu - KoteiHensu - PointNum * PointHensu
        GC.Collect()
        GC.WaitForPendingFinalizers()
        HOperatorSet.CreateMatrix(nG, nG, 0.0, SumLeftMatID)
        HOperatorSet.CreateMatrix(nG, 1, 0.0, SumRightMatID)
        For i = 1 To PointNum
            Dim FaMatID As HTuple = Nothing
            Dim EaInvMatID As HTuple = Nothing
            Dim DeltaAEMatID As HTuple = Nothing

            Calc_Fa_mat(i, FaMatID)
            'Calc_Fa_mat(FallMatID, i, FaMatID)
            'Calc_EaInv_mat(c, i, EaInvMatID)
            Calc_EaInv_mat(c, i, EaInvMatID)
            Calc_Delta_a_E_mat(i, DeltaAEMatID)
            arrFaMatID(i) = FaMatID
            arrEaInvMatID(i) = EaInvMatID
            arrDeltaAEMatID(i) = DeltaAEMatID

            HOperatorSet.MultMatrix(FaMatID, EaInvMatID, "ATB", MultMatId1)
            HOperatorSet.MultMatrix(MultMatId1, FaMatID, "AB", MultMatId2)
            HOperatorSet.AddMatrixMod(SumLeftMatID, MultMatId2)
            HOperatorSet.ClearMatrix(MultMatId2)
            HOperatorSet.MultMatrix(MultMatId1, DeltaAEMatID, "AB", MultMatId2)
            HOperatorSet.AddMatrixMod(SumRightMatID, MultMatId2)
            HOperatorSet.ClearMatrix(MultMatId2)

            HOperatorSet.ClearMatrix(MultMatId1)

        Next
        sw.Stop()
        Trace.WriteLine("バンドル更新値計算2：")
        Trace.WriteLine(sw.Elapsed.TotalSeconds & "秒")
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "JikanMonitor.mon",
                                        "バンドル更新値計算2: " & sw.Elapsed.TotalSeconds & "秒" & vbNewLine, True)

        sw.Reset()
        sw.Start()
        HOperatorSet.SubMatrix(GMatID, SumLeftMatID, LeftMatID)

        HOperatorSet.SubMatrix(SumRightMatID, DFMatID, RightMatID)

        HOperatorSet.SolveMatrix(LeftMatID, "general", 0.00000000000000022204, RightMatID, DeltaEpsFMatID)

        HOperatorSet.ClearMatrix(LeftMatID)
        HOperatorSet.ClearMatrix(RightMatID)

        HOperatorSet.CreateMatrix(PointHensu, 1, -1.0, tmpMatID)
        sw.Stop()
        Trace.WriteLine("バンドル更新値計算3(solve)：")
        Trace.WriteLine(sw.Elapsed.TotalSeconds & "秒")
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "JikanMonitor.mon",
                                   "バンドル更新値計算3(solve)： " & sw.Elapsed.TotalSeconds & "秒" & vbNewLine, True)

        sw.Reset()
        sw.Start()
        For i = 1 To PointNum
            Dim FaMatID As HTuple = Nothing
            Dim EaInvMatID As HTuple = Nothing
            Dim DeltaAEMatID As HTuple = Nothing

            FaMatID = arrFaMatID(i)
            EaInvMatID = arrEaInvMatID(i)
            DeltaAEMatID = arrDeltaAEMatID(i)
            HOperatorSet.MultMatrix(FaMatID, DeltaEpsFMatID, "AB", MultMatId1)
            HOperatorSet.AddMatrix(MultMatId1, DeltaAEMatID, AddMatID)
            HOperatorSet.MultMatrix(EaInvMatID, AddMatID, "AB", MultMatId2)
            HOperatorSet.MultElementMatrixMod(MultMatId2, tmpMatID)

            HOperatorSet.SetSubMatrix(DeltaMatID, MultMatId2, (i - 1) * PointHensu, 0)

            HOperatorSet.ClearMatrix(MultMatId2)

            HOperatorSet.ClearMatrix(MultMatId1)
            HOperatorSet.ClearMatrix(DeltaAEMatID)
            HOperatorSet.ClearMatrix(EaInvMatID)
            HOperatorSet.ClearMatrix(FaMatID)
            HOperatorSet.ClearMatrix(AddMatID)
        Next

        HOperatorSet.SetSubMatrix(DeltaMatID, DeltaEpsFMatID, PointNum * PointHensu, 0)
        sw.Stop()
        Trace.WriteLine("バンドル更新値計算4：")
        Trace.WriteLine(sw.Elapsed.TotalSeconds & "秒")
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "JikanMonitor.mon",
                              "バンドル更新値計算4：： " & sw.Elapsed.TotalSeconds & "秒" & vbNewLine, True)
        HOperatorSet.ClearMatrix(GMatID)
        HOperatorSet.ClearMatrix(DFMatID)
        '  HOperatorSet.ClearMatrix(FallMatID)
        '  HOperatorSet.ClearMatrix(EallMatID)
        HOperatorSet.ClearMatrix(DeltaEpsFMatID)
        '  HOperatorSet.ClearMatrix(DeltaEpsPMatID)
        HOperatorSet.ClearMatrix(LeftMatID)
        HOperatorSet.ClearMatrix(RightMatID)
        HOperatorSet.ClearMatrix(SumLeftMatID)
        HOperatorSet.ClearMatrix(SumRightMatID)
        HOperatorSet.ClearMatrix(MultMatId1)
        HOperatorSet.ClearMatrix(MultMatId2)
        HOperatorSet.ClearMatrix(tmpMatID)
        HOperatorSet.ClearMatrix(AddMatID)
    End Sub


    Public Sub Calc_DeltaMat_para(ByVal c As Double, ByRef DeltaMatID As HTuple)
        Dim GMatID As HTuple = Nothing
        Dim DFMatID As HTuple = Nothing
        Dim FallMatID As HTuple = Nothing
        Dim EallMatID As HTuple = Nothing
        'Dim FaMatID As HTuple = Nothing
        'Dim EaInvMatID As HTuple = Nothing
        'Dim DeltaAEMatID As HTuple = Nothing
        Dim arrFaMatID(PointNum) As HTuple
        Dim arrEaInvMatID(PointNum) As HTuple
        Dim arrDeltaAEMatID(PointNum) As HTuple
        Dim arrDeltaMatItemID(PointNum) As HTuple
        Dim DeltaEpsFMatID As HTuple = Nothing
        Dim DeltaEpsPMatID As HTuple = Nothing
        Dim LeftMatID As HTuple = Nothing
        Dim RightMatID As HTuple = Nothing
        Dim SumLeftMatID As HTuple = Nothing
        Dim SumRightMatID As HTuple = Nothing
        Dim arrSumLeftMatID(PointNum) As HTuple
        Dim arrSumRightMatID(PointNum) As HTuple


        '  Dim i As Integer

        HOperatorSet.CreateMatrix(Shikisuu - KoteiHensu, 1, 0.0, DeltaMatID)
        Dim sw As New System.Diagnostics.Stopwatch()
        'カメラ結合（CTによる結合）
        sw.Reset()
        sw.Start()

        'Parallel.Invoke(
        '    Sub()
        '        Calc_G_mat(c, GMatID)
        '    End Sub,
        '    Sub()
        '        Calc_DF_Mat(DFMatID)
        '    End Sub,
        '    Sub()
        '        Calc_Fall_mat(FallMatID)
        '    End Sub,
        '    Sub()
        '        Calc_EaInvAll_mat(c, EallMatID)
        '    End Sub)

        Calc_G_mat(c, GMatID)
        Calc_DF_Mat(DFMatID)
        '   Calc_Fall_mat(FallMatID)
        ' Calc_EaInvAll_mat(c, EallMatID)
        sw.Stop()
        Trace.WriteLine("バンドル更新値計算１：")
        Trace.WriteLine(sw.Elapsed.TotalSeconds & "秒")
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "JikanMonitor.mon",
                                    "kokomade： " & sw.Elapsed.TotalSeconds & "秒" & vbNewLine, True)
        sw.Reset()
        sw.Start()

        Dim nG As Integer = Shikisuu - KoteiHensu - PointNum * PointHensu
        'GC.Collect()
        'GC.WaitForPendingFinalizers()
        HOperatorSet.CreateMatrix(nG, nG, 0.0, SumLeftMatID)
        HOperatorSet.CreateMatrix(nG, 1, 0.0, SumRightMatID)
        Parallel.For(1, PointNum + 1, Sub(i)
                                          Dim MultMatId1 As HTuple = Nothing
                                          Dim MultMatId2 As HTuple = Nothing
                                          Dim MultMatId3 As HTuple = Nothing
                                          Dim FaMatID As HTuple = Nothing
                                          Dim EaInvMatID As HTuple = Nothing
                                          Dim DeltaAEMatID As HTuple = Nothing

                                          Calc_Fa_mat(i, FaMatID)
                                          'Calc_Fa_mat(FallMatID, i, FaMatID)
                                          Calc_EaInv_mat(c, i, EaInvMatID)
                                          ' Calc_EaInv_mat(EallMatID, i, EaInvMatID)
                                          Calc_Delta_a_E_mat(i, DeltaAEMatID)
                                          arrFaMatID(i) = FaMatID
                                          arrEaInvMatID(i) = EaInvMatID
                                          arrDeltaAEMatID(i) = DeltaAEMatID
                                          HOperatorSet.MultMatrix(FaMatID, EaInvMatID, "ATB", MultMatId1)

                                          HOperatorSet.MultMatrix(MultMatId1, FaMatID, "AB", MultMatId2)
                                          arrSumLeftMatID(i) = MultMatId2

                                          HOperatorSet.MultMatrix(MultMatId1, DeltaAEMatID, "AB", MultMatId3)
                                          arrSumRightMatID(i) = MultMatId3
                                          HOperatorSet.ClearMatrix(MultMatId1)
                                      End Sub)

        For i As Integer = 1 To PointNum
            HOperatorSet.AddMatrixMod(SumLeftMatID, arrSumLeftMatID(i))
            HOperatorSet.AddMatrixMod(SumRightMatID, arrSumRightMatID(i))
            HOperatorSet.ClearMatrix(arrSumLeftMatID(i))
            HOperatorSet.ClearMatrix(arrSumRightMatID(i))
        Next
        sw.Stop()
        Trace.WriteLine("バンドル更新値計算2：")
        Trace.WriteLine(sw.Elapsed.TotalSeconds & "秒")
        sw.Reset()
        sw.Start()
        HOperatorSet.SubMatrix(GMatID, SumLeftMatID, LeftMatID)

        HOperatorSet.SubMatrix(SumRightMatID, DFMatID, RightMatID)

        HOperatorSet.SolveMatrix(LeftMatID, "general", 0.00000000000000022204, RightMatID, DeltaEpsFMatID)

        HOperatorSet.ClearMatrix(LeftMatID)
        HOperatorSet.ClearMatrix(RightMatID)
        Dim tmpMatID As HTuple = Nothing
        HOperatorSet.CreateMatrix(PointHensu, 1, -1.0, tmpMatID)
        sw.Stop()
        Trace.WriteLine("バンドル更新値計算3(solve)：")
        Trace.WriteLine(sw.Elapsed.TotalSeconds & "秒")

        sw.Reset()
        sw.Start()
        Parallel.For(1, PointNum + 1, Sub(i)
                                          Dim FaMatID As HTuple = Nothing
                                          Dim EaInvMatID As HTuple = Nothing
                                          Dim DeltaAEMatID As HTuple = Nothing
                                          Dim MultMatId1 As HTuple = Nothing
                                          Dim MultMatId2 As HTuple = Nothing
                                          Dim AddMatID As HTuple = Nothing

                                          FaMatID = arrFaMatID(i)
                                          EaInvMatID = arrEaInvMatID(i)
                                          DeltaAEMatID = arrDeltaAEMatID(i)
                                          HOperatorSet.MultMatrix(FaMatID, DeltaEpsFMatID, "AB", MultMatId1)
                                          HOperatorSet.AddMatrix(MultMatId1, DeltaAEMatID, AddMatID)
                                          HOperatorSet.MultMatrix(EaInvMatID, AddMatID, "AB", MultMatId2)
                                          HOperatorSet.MultElementMatrixMod(MultMatId2, tmpMatID)

                                          ' HOperatorSet.SetSubMatrix(DeltaMatID, MultMatId2, (i - 1) * PointHensu, 0)
                                          arrDeltaMatItemID(i) = MultMatId2
                                          HOperatorSet.ClearMatrix(MultMatId1)
                                          HOperatorSet.ClearMatrix(DeltaAEMatID)
                                          HOperatorSet.ClearMatrix(EaInvMatID)
                                          HOperatorSet.ClearMatrix(FaMatID)
                                          HOperatorSet.ClearMatrix(AddMatID)
                                      End Sub)

        For i As Integer = 1 To PointNum
            HOperatorSet.SetSubMatrix(DeltaMatID, arrDeltaMatItemID(i), (i - 1) * PointHensu, 0)
            HOperatorSet.ClearMatrix(arrDeltaMatItemID(i))
        Next

        HOperatorSet.SetSubMatrix(DeltaMatID, DeltaEpsFMatID, PointNum * PointHensu, 0)
        sw.Stop()
        Trace.WriteLine("バンドル更新値計算4：")
        Trace.WriteLine(sw.Elapsed.TotalSeconds & "秒")

        HOperatorSet.ClearMatrix(GMatID)
        HOperatorSet.ClearMatrix(DFMatID)
        HOperatorSet.ClearMatrix(FallMatID)
        HOperatorSet.ClearMatrix(EallMatID)
        HOperatorSet.ClearMatrix(DeltaEpsFMatID)
        HOperatorSet.ClearMatrix(DeltaEpsPMatID)
        HOperatorSet.ClearMatrix(LeftMatID)
        HOperatorSet.ClearMatrix(RightMatID)
        HOperatorSet.ClearMatrix(SumLeftMatID)
        HOperatorSet.ClearMatrix(SumRightMatID)
        HOperatorSet.ClearMatrix(tmpMatID)

    End Sub


    Public Sub Calc_DeltaMat2(ByVal c As Double, ByRef DeltaMatID As HTuple)
        Dim GMatID As HTuple = Nothing
        Dim DFMatID As HTuple = Nothing
        Dim arrFaMatID(PointNum) As HTuple
        Dim arrEaInvMatID(PointNum) As HTuple
        Dim arrDeltaAEMatID(PointNum) As HTuple
        Dim DeltaEpsFMatID As HTuple = Nothing
        Dim DeltaEpsPMatID As HTuple = Nothing
        Dim LeftMatID As HTuple = Nothing
        Dim RightMatID As HTuple = Nothing
        Dim SumLeftMatID As HTuple = Nothing
        Dim SumRightMatID As HTuple = Nothing
        Dim MultMatId1 As HTuple = Nothing
        Dim MultMatId2 As HTuple = Nothing
        Dim AddMatID As HTuple = Nothing
        Dim tmpMatID As HTuple = Nothing
        Dim i As Integer

        HOperatorSet.CreateMatrix(Shikisuu - KoteiHensu, 1, 0.0, DeltaMatID)
#If DEBUG Then
        Dim sw As New System.Diagnostics.Stopwatch()
        sw.Reset()
        sw.Start()
#End If
        Calc_G_mat(c, GMatID)
        Calc_DF_Mat(DFMatID)
        '   Calc_Fall_mat(arrFaMatID)
        '  Calc_EaInvAll_mat(c, arrEaInvMatID)
        Calc_AllDelta_a_E_mat(arrDeltaAEMatID)
#If DEBUG Then
        sw.Stop()
        Trace.WriteLine("バンドル更新値計算1：")
        Trace.WriteLine(sw.Elapsed.TotalMilliseconds & "ミリ秒")
        sw.Reset()
        sw.Start()
#End If
        Dim nG As Integer = Shikisuu - KoteiHensu - PointNum * PointHensu
        GC.Collect()
        GC.WaitForPendingFinalizers()
        HOperatorSet.CreateMatrix(nG, nG, 0.0, SumLeftMatID)
        HOperatorSet.CreateMatrix(nG, 1, 0.0, SumRightMatID)

        For i = 1 To PointNum
            HOperatorSet.MultMatrix(arrFaMatID(i), arrEaInvMatID(i), "ATB", MultMatId1)
            HOperatorSet.MultMatrix(MultMatId1, arrFaMatID(i), "AB", MultMatId2)
            HOperatorSet.AddMatrixMod(SumLeftMatID, MultMatId2)
            HOperatorSet.ClearMatrix(MultMatId2)
            HOperatorSet.MultMatrix(MultMatId1, arrDeltaAEMatID(i), "AB", MultMatId2)
            HOperatorSet.AddMatrixMod(SumRightMatID, MultMatId2)
            HOperatorSet.ClearMatrix(MultMatId2)
            HOperatorSet.ClearMatrix(MultMatId1)
        Next
#If DEBUG Then
        sw.Stop()
        Trace.WriteLine("バンドル更新値計算2：")
        Trace.WriteLine(sw.Elapsed.TotalMilliseconds & "ミリ秒")
        sw.Reset()
        sw.Start()
#End If
        HOperatorSet.SubMatrix(GMatID, SumLeftMatID, LeftMatID)

        HOperatorSet.SubMatrix(SumRightMatID, DFMatID, RightMatID)

        HOperatorSet.SolveMatrix(LeftMatID, "general", 0, RightMatID, DeltaEpsFMatID)

        HOperatorSet.ClearMatrix(LeftMatID)
        HOperatorSet.ClearMatrix(RightMatID)

        HOperatorSet.CreateMatrix(PointHensu, 1, -1.0, tmpMatID)
#If DEBUG Then
        sw.Stop()
        Trace.WriteLine("バンドル更新値計算3(solve)：")
        Trace.WriteLine(sw.Elapsed.TotalMilliseconds & "ミリ秒")
        sw.Reset()
        sw.Start()
#End If
        For i = 1 To PointNum
            HOperatorSet.MultMatrix(arrFaMatID(i), DeltaEpsFMatID, "AB", MultMatId1)
            HOperatorSet.AddMatrix(MultMatId1, arrDeltaAEMatID(i), AddMatID)
            HOperatorSet.MultMatrix(arrEaInvMatID(i), AddMatID, "AB", MultMatId2)
            HOperatorSet.MultElementMatrixMod(MultMatId2, tmpMatID)

            HOperatorSet.SetSubMatrix(DeltaMatID, MultMatId2, (i - 1) * PointHensu, 0)

            HOperatorSet.ClearMatrix(MultMatId2)
            HOperatorSet.ClearMatrix(MultMatId1)
            HOperatorSet.ClearMatrix(arrDeltaAEMatID(i))
            HOperatorSet.ClearMatrix(arrEaInvMatID(i))
            HOperatorSet.ClearMatrix(arrFaMatID(i))
            HOperatorSet.ClearMatrix(AddMatID)
        Next

        HOperatorSet.SetSubMatrix(DeltaMatID, DeltaEpsFMatID, PointNum * PointHensu, 0)

#If DEBUG Then
        sw.Stop()
        Trace.WriteLine("バンドル更新値計算4：")
        Trace.WriteLine(sw.Elapsed.TotalMilliseconds & "ミリ秒")
#End If
        HOperatorSet.ClearMatrix(GMatID)
        HOperatorSet.ClearMatrix(DFMatID)
        HOperatorSet.ClearMatrix(DeltaEpsFMatID)
        HOperatorSet.ClearMatrix(DeltaEpsPMatID)
        HOperatorSet.ClearMatrix(LeftMatID)
        HOperatorSet.ClearMatrix(RightMatID)
        HOperatorSet.ClearMatrix(SumLeftMatID)
        HOperatorSet.ClearMatrix(SumRightMatID)
        HOperatorSet.ClearMatrix(MultMatId1)
        HOperatorSet.ClearMatrix(MultMatId2)
        HOperatorSet.ClearMatrix(tmpMatID)
        HOperatorSet.ClearMatrix(AddMatID)
    End Sub

    Private Sub Calc_G_mat(ByVal c As Double, ByRef GMatID As HTuple)
        Dim i As Integer
        Dim j As Integer
        Dim tmpobj((Shikisuu - KoteiHensu - PointNum * PointHensu) * (Shikisuu - KoteiHensu - PointNum * PointHensu) - 1) As Double
        Dim t As Long = 0
        For i = 0 To Shikisuu - KoteiHensu - PointNum * PointHensu - 1
            For j = 0 To Shikisuu - KoteiHensu - PointNum * PointHensu - 1
                If i = j Then
                    tmpobj(t) = (1 + c) * Gmat(i, j)
                Else
                    tmpobj(t) = Gmat(i, j)
                End If
                t = t + 1
            Next
        Next
        'GC.Collect()
        'GC.WaitForPendingFinalizers()
        HOperatorSet.CreateMatrix(Shikisuu - KoteiHensu - PointNum * PointHensu, Shikisuu - KoteiHensu - PointNum * PointHensu, tmpobj, GMatID)
    End Sub
    'Private Sub Calc_Fall_mat(ByRef arrFaMatID() As HTuple)
    '    Dim i As Integer
    '    Dim k As Integer
    '    Dim j As Integer
    '    Dim l As Integer = 1
    '    Dim t As Integer = 0
    '    For i = 1 To PointNum
    '        Dim tmpobj((PointHensu) * (Shikisuu - KoteiHensu - PointNum * PointHensu) - 1) As Double
    '        t = 0
    '        For k = 1 To PointHensu
    '            l = 1
    '            For j = PointNum * PointHensu + 1 To Shikisuu
    '                If RemoveRowCol(l) = j Then
    '                    If l < KoteiHensu Then
    '                        l += 1
    '                    End If
    '                Else
    '                    ' tmpobj(t) = Fmat((i - 1) * PointHensu + k, j - PointNum * PointHensu)
    '                    tmpobj(t) = Fmat((i - 1) * PointHensu + k)(j - PointNum * PointHensu)
    '                    t = t + 1
    '                End If
    '                'tmpobj(t) = ddEmat(Ei, Ej)
    '            Next
    '        Next
    '        Dim FaMatID As HTuple = Nothing
    '        'GC.Collect()
    '        'GC.WaitForPendingFinalizers()
    '        HOperatorSet.CreateMatrix(PointHensu, Shikisuu - KoteiHensu - PointNum * PointHensu, tmpobj, FaMatID)
    '        arrFaMatID(i) = FaMatID
    '    Next

    'End Sub

    'Private Sub Calc_Fall_mat(ByRef FallMatID As HTuple)
    '    Dim i As Integer
    '    Dim j As Integer
    '    Dim l As Integer = 1
    '    Dim t As Integer = 0
    '    Dim s As Integer
    '    Dim a As Integer
    '    'GC.Collect()
    '    'GC.WaitForPendingFinalizers()
    '    'GC.Collect()
    '    Dim tmpobj((PointHensu * PointNum) * (Shikisuu - KoteiHensu - PointNum * PointHensu) - 1) As Double
    '    For i = 1 To PointHensu * PointNum
    '        l = 1
    '        For j = PointNum * PointHensu + 1 To Shikisuu
    '            If RemoveRowCol(l) = j Then
    '                If l < KoteiHensu Then
    '                    l += 1
    '                End If
    '            Else
    '                s = CInt(System.Math.Floor((i - 1) / PointHensu) + 1)
    '                a = (i - 1) Mod PointHensu + 1
    '                ' tmpobj(t) = Fmat(i, j - PointNum * PointHensu)
    '                tmpobj(t) = Fmat(i)(j - PointNum * PointHensu)
    '                t = t + 1
    '            End If
    '            'tmpobj(t) = ddEmat(Ei, Ej)
    '        Next
    '    Next

    '    HOperatorSet.CreateMatrix(PointHensu * PointNum, Shikisuu - KoteiHensu - PointNum * PointHensu, tmpobj, FallMatID)
    'End Sub
    Private Sub Calc_Fa_mat(ByRef FallMatId As HTuple, ByVal a As Integer, ByRef FaMatID As HTuple)

        'GC.Collect()
        'GC.WaitForPendingFinalizers()
        HOperatorSet.GetSubMatrix(FallMatId, (a - 1) * PointHensu, 0, _
                        PointHensu, Shikisuu - KoteiHensu - PointNum * PointHensu, FaMatID)

    End Sub


    Private Sub Calc_Fa_mat(ByVal a As Integer, ByRef FaMatID As HTuple)
        Dim i As Integer
        Dim j As Integer
        Dim t As Integer = 0
        'GC.Collect()
        'GC.WaitForPendingFinalizers()
        Dim tmpobj((PointHensu) * (Shikisuu - KoteiHensu - PointNum * PointHensu) - 1) As Double
        'Dim objFmat(PointHensu - 1, (Shikisuu - KoteiHensu - PointNum * PointHensu) - 1) As Double
        'objFmat = lstFmat(a - 1)
        For i = 0 To PointHensu - 1
            For j = 0 To Shikisuu - KoteiHensu - PointNum * PointHensu - 1
                'Ei = (a - 1) * PointHensu + i
                'Ej = j
                ''tmpobj(t) = ddEmat(Ei, Ej)
                ''  tmpobj(t) = Fmat(Ei, Ej - PointNum * PointHensu)
                'tmpobj(t) = Fmat(Ei)(Ej - PointNum * PointHensu)
                ' tmpobj(t) = objFmat(i, j)
                tmpobj(t) = Fmat(a - 1)(i, j)
                t = t + 1
            Next
        Next

        HOperatorSet.CreateMatrix(PointHensu, Shikisuu - KoteiHensu - PointNum * PointHensu, tmpobj, FaMatID)
    End Sub
    'Private Sub Calc_EaInvAll_mat(ByVal c As Double, ByRef arrEaInvMatID() As HTuple)
    '    Dim i As Integer
    '    Dim i1 As Integer
    '    Dim i2 As Integer
    '    Dim t As Integer = 0

    '    For i = 1 To PointNum
    '        Dim tmpobj(PointHensu * PointHensu - 1) As Double
    '        t = 0
    '        For i1 = 1 To PointHensu
    '            For i2 = 1 To PointHensu
    '                If i1 = i2 Then
    '                    tmpobj(t) = (1 + c) * Emat((i - 1) * PointHensu + i1, i2)
    '                Else
    '                    tmpobj(t) = Emat((i - 1) * PointHensu + i1, i2)
    '                End If
    '                t += 1
    '            Next
    '        Next
    '        Dim EaMatID As HTuple = Nothing
    '        HOperatorSet.CreateMatrix(PointHensu, PointHensu, tmpobj, EaMatID)
    '        HOperatorSet.InvertMatrixMod(EaMatID, "general", 0)
    '        arrEaInvMatID(i) = EaMatID
    '    Next

    'End Sub
    'Private Sub Calc_EaInvAll_mat(ByVal c As Double, ByRef EallMatID As HTuple)

    '    Dim i1 As Integer
    '    Dim i2 As Integer
    '    Dim t As Integer = 0
    '    'GC.Collect()
    '    'GC.WaitForPendingFinalizers()
    '    Dim tmpobj(PointHensu * PointNum * PointHensu - 1) As Double

    '    For i1 = 0 To PointHensu * PointNum - 1
    '        For i2 = 0 To PointHensu - 1
    '            If (i1 Mod PointHensu) = i2 Then
    '                tmpobj(t) = (1 + c) * Emat(i1 + 1, i2 + 1)
    '            Else
    '                tmpobj(t) = Emat(i1 + 1, i2 + 1)
    '            End If
    '            t += 1
    '        Next
    '    Next

    '    HOperatorSet.CreateMatrix(PointHensu * PointNum, PointHensu, tmpobj, EallMatID)

    'End Sub

    'Private Sub Calc_EaInv_mat(ByRef EallMatID As HTuple, ByVal a As Integer, ByRef EaMatID As HTuple)
    '    HOperatorSet.GetSubMatrix(EallMatID, (a - 1) * PointHensu, 0, PointHensu, PointHensu, EaMatID)
    '    'HOperatorSet.CreateMatrix(PointHensu, PointHensu, tmpobj, EaMatID)
    '    HOperatorSet.InvertMatrixMod(EaMatID, "general", 0.00000000000000022204)

    'End Sub

    Private Sub Calc_EaInv_mat(ByVal c As Integer, ByVal a As Integer, ByRef EaMatID As HTuple)
        Dim i1 As Integer
        Dim i2 As Integer
        Dim t As Integer = 0
        'GC.Collect()
        'GC.WaitForPendingFinalizers()
        Dim tmpobj(PointHensu * PointHensu - 1) As Double
        For i1 = 0 To PointHensu - 1
            For i2 = 0 To PointHensu - 1
                If i1 = i2 Then
                    tmpobj(t) = (1 + c) * lstEmat.Item(a - 1)(i1, i2)
                Else
                    tmpobj(t) = lstEmat.Item(a - 1)(i1, i2)
                End If
                t += 1
            Next
        Next
        HOperatorSet.CreateMatrix(PointHensu, PointHensu, tmpobj, EaMatID)

        HOperatorSet.InvertMatrixMod(EaMatID, "general", 0.00000000000000022204)

    End Sub

    'Private Sub Calc_EaInv_mat(ByVal c As Double, ByVal a As Integer, ByRef EaMatID As HTuple)
    '    Dim Ei As Integer
    '    Dim Ej As Integer
    '    Dim i1 As Integer
    '    Dim i2 As Integer
    '    Dim t As Integer = 0
    '    Dim tmpobj(PointHensu * PointHensu - 1) As Double
    '    For i1 = 1 To PointHensu
    '        For i2 = 1 To PointHensu
    '            Ei = (a - 1) * PointHensu + i1
    '            'Ej = (a - 1) * PointHensu + i2
    '            Ej = i2
    '            If i1 = i2 Then
    '                'tmpobj(t) = (1 + c) * ddEmat(Ei, Ej)
    '                tmpobj(t) = (1 + c) * Emat(Ei, Ej)
    '            Else
    '                'tmpobj(t) = ddEmat(Ei, Ej)
    '                tmpobj(t) = Emat(Ei, Ej)
    '            End If

    '            t = t + 1
    '        Next
    '    Next
    '    HOperatorSet.CreateMatrix(PointHensu, PointHensu, tmpobj, EaMatID)
    '    HOperatorSet.InvertMatrixMod(EaMatID, "general", 0.00000000000000022204)

    'End Sub

    Private Sub Calc_AllDelta_a_E_mat(ByRef arrDeltaAEMatID() As HTuple)
        Dim Ei As Integer
        Dim i As Integer
        Dim j As Integer
        Dim t As Integer = 0

        For i = 1 To PointNum
            Dim tmpobj(PointHensu - 1) As Double
            t = 0
            For j = 1 To PointHensu
                Ei = (i - 1) * PointHensu + j
                tmpobj(t) = dEmat(Ei)
                t = t + 1
            Next
            Dim Delta_a_E_MatID As HTuple = Nothing
            HOperatorSet.CreateMatrix(PointHensu, 1, tmpobj, Delta_a_E_MatID)
            arrDeltaAEMatID(i) = Delta_a_E_MatID
        Next


    End Sub

    Private Sub Calc_Delta_a_E_mat(ByVal a As Integer, ByRef Delta_a_E_MatID As HTuple)
        Dim Ei As Integer
        Dim i As Integer
        Dim t As Integer = 0
        Dim tmpobj(PointHensu - 1) As Double
        For i = 1 To PointHensu
            Ei = (a - 1) * PointHensu + i
            tmpobj(t) = dEmat(Ei)
            t = t + 1
        Next
        HOperatorSet.CreateMatrix(PointHensu, 1, tmpobj, Delta_a_E_MatID)
    End Sub

    Private Sub Calc_DF_Mat(ByRef DF_MatID As HTuple)
        Dim Ei As Integer
        Dim i As Integer
        Dim l As Integer = 1
        Dim t As Integer = 0
        Dim tmpobj((Shikisuu - KoteiHensu - PointNum * PointHensu) - 1) As Double
        For i = PointNum * PointHensu + 1 To Shikisuu
            If RemoveRowCol(l) = i Then
                If l < KoteiHensu Then
                    l += 1
                End If
            Else
                Ei = i
                tmpobj(t) = dEmat(Ei)
                t = t + 1
            End If
        Next
        'GC.Collect()
        'GC.WaitForPendingFinalizers()
        HOperatorSet.CreateMatrix(Shikisuu - KoteiHensu - PointNum * PointHensu, 1, tmpobj, DF_MatID)
    End Sub

    '    Public Sub Create_ddEmatID(ByVal c As Double, ByRef ddEMatID As HTuple)
    '        Dim i As Integer
    '        Dim j As Integer
    '        '  Dim tmpObj As HTuple = DBNull.Value
    '        GC.Collect()
    '        GC.WaitForPendingFinalizers()
    '        Dim tmpobj((Shikisuu - KoteiHensu) * (Shikisuu - KoteiHensu) - 1) As Double
    '        Dim t As Long = 0
    '        Dim l As Integer
    '        For i = 1 To Shikisuu
    '            For l = 1 To KoteiHensu
    '                If RemoveRowCol(l) = i Then
    '                    GoTo removeline
    '                End If
    '            Next
    '            For j = 1 To Shikisuu
    '                For l = 1 To KoteiHensu
    '                    If RemoveRowCol(l) = j Then
    '                        GoTo removeline1
    '                    End If
    '                Next
    '                If i = j Then
    '                    tmpobj(t) = (1 + c) * ddEmat(i, j)
    '                Else
    '                    tmpobj(t) = ddEmat(i, j)
    '                End If
    '                '   tmpobj = BTuple.TupleConcat(tmpobj, tmpData)
    '                t = t + 1
    'removeline1:
    '            Next
    'removeline:

    '        Next
    '        HOperatorSet.CreateMatrix(Shikisuu - KoteiHensu, Shikisuu - KoteiHensu, tmpobj, ddEMatID)
    '    End Sub

    Public Sub Create_dEmatID(ByRef dEMatID As HTuple)
        Dim i As Integer
        Dim t As Integer
        Dim k As Integer = 0
        GC.Collect()
        GC.WaitForPendingFinalizers()
        Dim tmpObj(Shikisuu - 1 - KoteiHensu) As Double
        For i = 1 To Shikisuu
            For t = 1 To KoteiHensu
                If RemoveRowCol(t) = i Then
                    k += 1
                    GoTo removeline
                End If
            Next
            tmpObj(i - 1 - k) = (-1) * dEmat(i)
removeline:
        Next
        HOperatorSet.CreateMatrix(Shikisuu - KoteiHensu, 1, tmpObj, dEMatID)

    End Sub


    '更新用のデータを作成する。そのためBAMainDataに対して使ってはいけない。
    Public Sub BaDataKoushin(ByRef BaMainData As BAdata, ByVal DeltaVal() As Double)
        ' Dim DeltaVal As HTuple = Nothing
        Dim i As Integer
        Dim j As Integer
        Dim ic As Integer

        Dim t As Integer
        Dim tmpBaMainData_Images_T As New Point3D
        Dim HomMat3dIdentity As HTuple = Nothing
        HOperatorSet.HomMat3dIdentity(HomMat3dIdentity)

        ' HOperatorSet.GetFullMatrix(DeltaMatID, DeltaVal)

        t = 0
        '三次元座標の更新
        For i = 1 To PointNum
            Me.Points(i) = New Point3D

            If PointHensu = 0 Then
                Me.Points(i).X = BaMainData.Points(i).X

                Me.Points(i).Y = BaMainData.Points(i).Y

                Me.Points(i).Z = BaMainData.Points(i).Z
            End If

            For j = 1 To PointHensu
                Select Case j
                    Case 1
                        Me.Points(i).X = BaMainData.Points(i).X + DeltaVal(t)
                    Case 2
                        Me.Points(i).Y = BaMainData.Points(i).Y + DeltaVal(t)
                    Case 3
                        Me.Points(i).Z = BaMainData.Points(i).Z + DeltaVal(t)
                End Select
                t = t + 1
            Next

        Next

        '内部パラメータの更新 SUURI MODOSU
        ReDim Me.CNP(CameraNum)
        For ic = 1 To CameraNum
            Me.CNP(ic) = New CamNaibuParam
            Select Case CamNaibuHensu
                Case 0
                    Me.CNP(ic).F = BaMainData.CNP(ic).F
                    Me.CNP(ic).U0 = BaMainData.CNP(ic).U0
                    Me.CNP(ic).V0 = BaMainData.CNP(ic).V0
                    Me.CNP(ic).K1 = BaMainData.CNP(ic).K1
                    Me.CNP(ic).K2 = BaMainData.CNP(ic).K2
                    Me.CNP(ic).K3 = BaMainData.CNP(ic).K3
                    Me.CNP(ic).P1 = BaMainData.CNP(ic).P1
                    Me.CNP(ic).P2 = BaMainData.CNP(ic).P2
                Case 3
                    Me.CNP(ic).F = BaMainData.CNP(ic).F + DeltaVal(Shikisuu - (CameraNum - ic) * CamNaibuHensu - 3 - KoteiHensu)
                    Me.CNP(ic).U0 = BaMainData.CNP(ic).U0 + DeltaVal(Shikisuu - (CameraNum - ic) * CamNaibuHensu - 2 - KoteiHensu)
                    Me.CNP(ic).V0 = BaMainData.CNP(ic).V0 + DeltaVal(Shikisuu - (CameraNum - ic) * CamNaibuHensu - 1 - KoteiHensu)
                    Me.CNP(ic).K1 = BaMainData.CNP(ic).K1
                    Me.CNP(ic).K2 = BaMainData.CNP(ic).K2
                    Me.CNP(ic).K3 = BaMainData.CNP(ic).K3
                    Me.CNP(ic).P1 = BaMainData.CNP(ic).P1
                    Me.CNP(ic).P2 = BaMainData.CNP(ic).P2
                Case 6
                    Me.CNP(ic).F = BaMainData.CNP(ic).F + DeltaVal(Shikisuu - (CameraNum - ic) * CamNaibuHensu - 6 - KoteiHensu)
                    Me.CNP(ic).U0 = BaMainData.CNP(ic).U0 + DeltaVal(Shikisuu - (CameraNum - ic) * CamNaibuHensu - 5 - KoteiHensu)
                    Me.CNP(ic).V0 = BaMainData.CNP(ic).V0 + DeltaVal(Shikisuu - (CameraNum - ic) * CamNaibuHensu - 4 - KoteiHensu)
                    Me.CNP(ic).K1 = BaMainData.CNP(ic).K1 + DeltaVal(Shikisuu - (CameraNum - ic) * CamNaibuHensu - 3 - KoteiHensu)
                    Me.CNP(ic).K2 = BaMainData.CNP(ic).K2 + DeltaVal(Shikisuu - (CameraNum - ic) * CamNaibuHensu - 2 - KoteiHensu)
                    Me.CNP(ic).K3 = BaMainData.CNP(ic).K3 + DeltaVal(Shikisuu - (CameraNum - ic) * CamNaibuHensu - 1 - KoteiHensu)
                    Me.CNP(ic).P1 = BaMainData.CNP(ic).P1
                    Me.CNP(ic).P2 = BaMainData.CNP(ic).P2
                Case 8
                    Me.CNP(ic).F = BaMainData.CNP(ic).F + DeltaVal(Shikisuu - (CameraNum - ic) * CamNaibuHensu - 8 - KoteiHensu)
                    Me.CNP(ic).U0 = BaMainData.CNP(ic).U0 + DeltaVal(Shikisuu - (CameraNum - ic) * CamNaibuHensu - 7 - KoteiHensu)
                    Me.CNP(ic).V0 = BaMainData.CNP(ic).V0 + DeltaVal(Shikisuu - (CameraNum - ic) * CamNaibuHensu - 6 - KoteiHensu)
                    Me.CNP(ic).K1 = BaMainData.CNP(ic).K1 + DeltaVal(Shikisuu - (CameraNum - ic) * CamNaibuHensu - 5 - KoteiHensu)
                    Me.CNP(ic).K2 = BaMainData.CNP(ic).K2 + DeltaVal(Shikisuu - (CameraNum - ic) * CamNaibuHensu - 4 - KoteiHensu)
                    Me.CNP(ic).K3 = BaMainData.CNP(ic).K3 + DeltaVal(Shikisuu - (CameraNum - ic) * CamNaibuHensu - 3 - KoteiHensu)
                    Me.CNP(ic).P1 = BaMainData.CNP(ic).P1 + DeltaVal(Shikisuu - (CameraNum - ic) * CamNaibuHensu - 2 - KoteiHensu)
                    Me.CNP(ic).P2 = BaMainData.CNP(ic).P2 + DeltaVal(Shikisuu - (CameraNum - ic) * CamNaibuHensu - 1 - KoteiHensu)
            End Select

            Me.CNP(ic).Sx = BaMainData.CNP(ic).Sx
            Me.CNP(ic).Sy = BaMainData.CNP(ic).Sy
            Me.CNP(ic).A = BaMainData.CNP(ic).A

        Next
        '外部パラメータの更新
        For j = 1 To KoteiHensu
            Me.RemoveRowCol(j) = BaMainData.RemoveRowCol(j)
        Next

        For j = 1 To ImageNum
            Me.Images(j) = New CameraData
            'SUURI ADD
            Me.Images(j).CNP.CopyToMe(Me.CNP(BaMainData.Images(j).CID))
            '平行移動ベクトル更新
            tmpBaMainData_Images_T = BaMainData.Images(j).T

            If j = BaMainData.FirstImageIndex And KoteiHensu <> 0 Then

                Me.Images(j).T.X = tmpBaMainData_Images_T.X
                Me.Images(j).T.Y = tmpBaMainData_Images_T.Y
                Me.Images(j).T.Z = tmpBaMainData_Images_T.Z
                Me.Images(j).R.CopyToMe(BaMainData.Images(j).R)
                '投影行列の更新
                Me.Images(j).P_wo_Koushin(Me.Images(j).CNP)
                Continue For
            End If
            If j = BaMainData.SecondImageIndex And KoteiHensu <> 0 Then
                If BaMainData.SecondImageXorY = 1 Then
                    Me.Images(j).T.X = tmpBaMainData_Images_T.X
                    Me.Images(j).T.Y = tmpBaMainData_Images_T.Y + DeltaVal(t)
                    t = t + 1
                Else
                    Me.Images(j).T.X = tmpBaMainData_Images_T.X + DeltaVal(t)
                    t = t + 1
                    Me.Images(j).T.Y = tmpBaMainData_Images_T.Y
                End If
            Else
                Me.Images(j).T.X = tmpBaMainData_Images_T.X + DeltaVal(t)
                t = t + 1
                Me.Images(j).T.Y = tmpBaMainData_Images_T.Y + DeltaVal(t)
                t = t + 1
            End If

            Me.Images(j).T.Z = tmpBaMainData_Images_T.Z + DeltaVal(t)
            t = t + 1
            Me.Images(j).W.X = DeltaVal(t)
            t = t + 1
            Me.Images(j).W.Y = DeltaVal(t)
            t = t + 1
            Me.Images(j).W.Z = DeltaVal(t)
            t = t + 1

            '回転行列の更新
            Dim kakudo As Double = Me.Images(j).W.Nagasa
            Dim Axis As HTuple = Me.Images(j).W.Axis
            Dim RWk As HTuple = Nothing
            Dim tmpR As New RMat
            HOperatorSet.HomMat3dRotateLocal(HomMat3dIdentity, kakudo, Axis, RWk)
            tmpR.Set_R_ByHomMat(RWk)
            Me.Images(j).R.CopyToMe(tmpR.Multi_By_Rmat(BaMainData.Images(j).R))

            '投影行列の更新
            Me.Images(j).P_wo_Koushin(Me.Images(j).CNP)
        Next

        '再投影誤差を計算
        CalcReProjectionError()

    End Sub

    Public Sub BaDataCopyToMe(ByVal BaKousinData As BAdata)

        Parallel.Invoke(
            Sub()

                '   Me.CNP.CopyToMe(BaKousinData.CNP)
                Dim i As Integer
                For i = 1 To CameraNum
                    Me.CNP(i).CopyToMe(BaKousinData.CNP(i))
                Next
                For i = 1 To PointNum
                    Me.Points(i).CopyToMe(BaKousinData.Points(i))
                Next
            End Sub,
            Sub()
                Dim j As Integer
                For j = 1 To ImageNum
                    Me.Images(j).T.CopyToMe(BaKousinData.Images(j).T)
                    Me.Images(j).R.CopyToMe(BaKousinData.Images(j).R)
                    Me.Images(j).P.CopyToMe(BaKousinData.Images(j).P)
                Next
            End Sub,
            Sub()
                Dim i As Integer
                Dim j As Integer
                For i = 1 To PointNum
                    For j = 1 To ImageNum
                        If Imat(i, j) = 1 Then
                            Pak(i, j) = BaKousinData.Pak(i, j)
                            Qak(i, j) = BaKousinData.Qak(i, j)
                            Rak(i, j) = BaKousinData.Rak(i, j)
                            Uak(i, j) = BaKousinData.Uak(i, j)
                            Vak(i, j) = BaKousinData.Vak(i, j)
                            Dak(i, j) = BaKousinData.Dak(i, j)
                            HXak(i, j) = BaKousinData.HXak(i, j)
                            HYak(i, j) = BaKousinData.HYak(i, j)
                        End If
                    Next
                Next
            End Sub
        )


        Me.dblE = BaKousinData.dblE
        Me.dbl_SqrtE = BaKousinData.dbl_SqrtE
    End Sub

    Public Sub WriteObj(ByRef sw As System.IO.StreamWriter)

        Dim i As Integer
        Dim j As Integer
        For i = 1 To PointNum
            For j = 1 To ImageNum
                If Imat(i, j) = 1 Then
                    sw.WriteLine(Pak(i, j))
                    sw.WriteLine(Qak(i, j))
                    sw.WriteLine(Rak(i, j))
                    sw.WriteLine(Uak(i, j))
                    sw.WriteLine(Vak(i, j))
                    sw.WriteLine(Dak(i, j))
                    sw.WriteLine(HXak(i, j))
                    sw.WriteLine(HYak(i, j))
                End If
            Next
        Next


    End Sub

#Region "一次微分要素"

    Private Function Calc_dE_dEpsK_Points(ByVal pointindex As Integer, _
                                          ByVal XYZ As Integer _
                                          ) As Double
        Dim i As Integer
        Dim j As Integer
        Dim dPak_dEpsk As Double
        Dim dQak_dEpsk As Double
        Dim dRak_dEpsk As Double
        Dim Sum As Double = 0
        i = pointindex
        For j = 1 To ImageNum
            If Imat(i, j) = 1 Then
                Select Case XYZ
                    Case 1
                        dPak_dEpsk = dPak_dXb(j)
                        dQak_dEpsk = dQak_dXb(j)
                        dRak_dEpsk = dRak_dXb(j)
                    Case 2
                        dPak_dEpsk = dPak_dYb(j)
                        dQak_dEpsk = dQak_dYb(j)
                        dRak_dEpsk = dRak_dYb(j)
                    Case 3
                        dPak_dEpsk = dPak_dZb(j)
                        dQak_dEpsk = dQak_dZb(j)
                        dRak_dEpsk = dRak_dZb(j)
                End Select
                Sum = Sum + Calc_Ichiji_Bibun_KakkoNai(i, j, dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, 0, 0)
            End If
        Next
        Calc_dE_dEpsK_Points = 2 * Sum

    End Function

    Private Function Calc_dE_dEpsK_Focal_AllImage(ByVal ic As Integer) As Double
        Dim i As Integer
        Dim j As Integer
        Dim dPak_dEpsk As Double
        Dim dQak_dEpsk As Double
        Dim dRak_dEpsk As Double
        Dim Sum As Double = 0
        For i = 1 To PointNum
            For j = 1 To ImageNum
                If Imat(i, j) = 1 And Images(j).CID = ic Then
                    dPak_dEpsk = dPak_dF(i, j)
                    dQak_dEpsk = dQak_dF(i, j)
                    dRak_dEpsk = dRak_dF(i, j)
                    Sum = Sum + Calc_Ichiji_Bibun_KakkoNai(i, j, dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, 0, 0)
                End If
            Next
        Next
        Calc_dE_dEpsK_Focal_AllImage = 2 * Sum
    End Function

    Private Function Calc_dE_dEpsK_U0_AllImage(ByVal ic As Integer) As Double
        Dim i As Integer
        Dim j As Integer
        Dim dPak_dEpsk As Double
        Dim dQak_dEpsk As Double
        Dim dRak_dEpsk As Double
        Dim dHXak_dEpsk As Double
        Dim dHYak_dEpsk As Double
        Dim Sum As Double = 0
        For i = 1 To PointNum
            For j = 1 To ImageNum
                If Imat(i, j) = 1 And Images(j).CID = ic Then
                    dPak_dEpsk = dPak_dU0(i, j)
                    dQak_dEpsk = 0 'dQak_dU0()
                    dRak_dEpsk = 0 'dRak_dU0()
                    dHXak_dEpsk = dHXak_dU0(i, j)
                    dHYak_dEpsk = dHYak_dU0(i, j)
                    Sum = Sum + Calc_Ichiji_Bibun_KakkoNai(i, j, dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, dHXak_dEpsk, dHYak_dEpsk)
                End If
            Next
        Next
        Calc_dE_dEpsK_U0_AllImage = 2 * Sum
    End Function

    Private Function Calc_dE_dEpsK_V0_AllImage(ByVal ic As Integer) As Double
        Dim i As Integer
        Dim j As Integer
        Dim dPak_dEpsk As Double
        Dim dQak_dEpsk As Double
        Dim dRak_dEpsk As Double
        Dim dHXak_dEpsk As Double
        Dim dHYak_dEpsk As Double
        Dim Sum As Double = 0
        For i = 1 To PointNum
            For j = 1 To ImageNum
                If Imat(i, j) = 1 And Images(j).CID = ic Then
                    dPak_dEpsk = dPak_dV0()
                    dQak_dEpsk = dQak_dV0(i, j)
                    dRak_dEpsk = dRak_dV0()
                    dHXak_dEpsk = dHXak_dV0(i, j)
                    dHYak_dEpsk = dHYak_dV0(i, j)
                    Sum = Sum + Calc_Ichiji_Bibun_KakkoNai(i, j, dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, dHXak_dEpsk, dHYak_dEpsk)
                End If
            Next
        Next
        Calc_dE_dEpsK_V0_AllImage = 2 * Sum
    End Function

    Private Function Calc_dE_dEpsK_K1_AllImage(ByVal ic As Integer) As Double
        Dim i As Integer
        Dim j As Integer
        Dim dPak_dEpsk As Double
        Dim dQak_dEpsk As Double
        Dim dRak_dEpsk As Double
        Dim dHXak_dEpsk As Double
        Dim dHYak_dEpsk As Double
        Dim Sum As Double = 0
        For i = 1 To PointNum
            For j = 1 To ImageNum
                If Imat(i, j) = 1 And Images(j).CID = ic Then
                    dPak_dEpsk = 0
                    dQak_dEpsk = 0
                    dRak_dEpsk = 0
                    dHXak_dEpsk = dHXak_dK1(i, j)
                    dHYak_dEpsk = dHYak_dK1(i, j)
                    Sum = Sum + Calc_Ichiji_Bibun_KakkoNai(i, j, dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, dHXak_dEpsk, dHYak_dEpsk)
                End If
            Next
        Next
        Calc_dE_dEpsK_K1_AllImage = 2 * Sum
    End Function

    Private Function Calc_dE_dEpsK_K2_AllImage(ByVal ic As Integer) As Double
        Dim i As Integer
        Dim j As Integer
        Dim dPak_dEpsk As Double
        Dim dQak_dEpsk As Double
        Dim dRak_dEpsk As Double
        Dim dHXak_dEpsk As Double
        Dim dHYak_dEpsk As Double
        Dim Sum As Double = 0
        For i = 1 To PointNum
            For j = 1 To ImageNum
                If Imat(i, j) = 1 And Images(j).CID = ic Then
                    dPak_dEpsk = 0
                    dQak_dEpsk = 0
                    dRak_dEpsk = 0
                    dHXak_dEpsk = dHXak_dK2(i, j)
                    dHYak_dEpsk = dHYak_dK2(i, j)
                    Sum = Sum + Calc_Ichiji_Bibun_KakkoNai(i, j, dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, dHXak_dEpsk, dHYak_dEpsk)
                End If
            Next
        Next
        Calc_dE_dEpsK_K2_AllImage = 2 * Sum
    End Function

    Private Function Calc_dE_dEpsK_K3_AllImage(ByVal ic As Integer) As Double
        Dim i As Integer
        Dim j As Integer
        Dim dPak_dEpsk As Double
        Dim dQak_dEpsk As Double
        Dim dRak_dEpsk As Double
        Dim dHXak_dEpsk As Double
        Dim dHYak_dEpsk As Double
        Dim Sum As Double = 0
        For i = 1 To PointNum
            For j = 1 To ImageNum
                If Imat(i, j) = 1 And Images(j).CID = ic Then
                    dPak_dEpsk = 0
                    dQak_dEpsk = 0
                    dRak_dEpsk = 0
                    dHXak_dEpsk = dHXak_dK3(i, j)
                    dHYak_dEpsk = dHYak_dK3(i, j)
                    Sum = Sum + Calc_Ichiji_Bibun_KakkoNai(i, j, dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, dHXak_dEpsk, dHYak_dEpsk)
                End If
            Next
        Next
        Calc_dE_dEpsK_K3_AllImage = 2 * Sum
    End Function

    Private Function Calc_dE_dEpsK_P1_AllImage(ByVal ic As Integer) As Double
        Dim i As Integer
        Dim j As Integer
        Dim dPak_dEpsk As Double
        Dim dQak_dEpsk As Double
        Dim dRak_dEpsk As Double
        Dim dHXak_dEpsk As Double
        Dim dHYak_dEpsk As Double
        Dim Sum As Double = 0
        For i = 1 To PointNum
            For j = 1 To ImageNum
                If Imat(i, j) = 1 And Images(j).CID = ic Then
                    dPak_dEpsk = 0
                    dQak_dEpsk = 0
                    dRak_dEpsk = 0
                    dHXak_dEpsk = dHXak_dP1(i, j)
                    dHYak_dEpsk = dHYak_dP1(i, j)
                    Sum = Sum + Calc_Ichiji_Bibun_KakkoNai(i, j, dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, dHXak_dEpsk, dHYak_dEpsk)
                End If
            Next
        Next
        Calc_dE_dEpsK_P1_AllImage = 2 * Sum
    End Function

    Private Function Calc_dE_dEpsK_P2_AllImage(ByVal ic As Integer) As Double
        Dim i As Integer
        Dim j As Integer
        Dim dPak_dEpsk As Double
        Dim dQak_dEpsk As Double
        Dim dRak_dEpsk As Double
        Dim dHXak_dEpsk As Double
        Dim dHYak_dEpsk As Double
        Dim Sum As Double = 0
        For i = 1 To PointNum
            For j = 1 To ImageNum
                If Imat(i, j) = 1 And Images(j).CID = ic Then
                    dPak_dEpsk = 0
                    dQak_dEpsk = 0
                    dRak_dEpsk = 0
                    dHXak_dEpsk = dHXak_dP2(i, j)
                    dHYak_dEpsk = dHYak_dP2(i, j)
                    Sum = Sum + Calc_Ichiji_Bibun_KakkoNai(i, j, dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, dHXak_dEpsk, dHYak_dEpsk)
                End If
            Next
        Next
        Calc_dE_dEpsK_P2_AllImage = 2 * Sum
    End Function

    Private Function Calc_dE_dEpsK_T1(ByVal ImageIndex As Integer) As Double
        Dim i As Integer
        Dim j As Integer
        Dim dPak_dEpsk As Double
        Dim dQak_dEpsk As Double
        Dim dRak_dEpsk As Double
        Dim Sum As Double = 0
        For i = 1 To PointNum
            j = ImageIndex
            If Imat(i, j) = 1 Then
                dPak_dEpsk = dPak_dT1(j)
                dQak_dEpsk = dQak_dT1(j)
                dRak_dEpsk = dRak_dT1(j)
                Sum = Sum + Calc_Ichiji_Bibun_KakkoNai(i, j, dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, 0, 0)
            End If
        Next
        Calc_dE_dEpsK_T1 = 2 * Sum
    End Function

    Private Function Calc_dE_dEpsK_T2(ByVal ImageIndex As Integer) As Double
        Dim i As Integer
        Dim j As Integer
        Dim dPak_dEpsk As Double
        Dim dQak_dEpsk As Double
        Dim dRak_dEpsk As Double
        Dim Sum As Double = 0
        For i = 1 To PointNum
            j = ImageIndex
            If Imat(i, j) = 1 Then
                dPak_dEpsk = dPak_dT2(j)
                dQak_dEpsk = dQak_dT2(j)
                dRak_dEpsk = dRak_dT2(j)
                Sum = Sum + Calc_Ichiji_Bibun_KakkoNai(i, j, dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, 0, 0)
            End If
        Next
        Calc_dE_dEpsK_T2 = 2 * Sum
    End Function

    Private Function Calc_dE_dEpsK_T3(ByVal ImageIndex As Integer) As Double
        Dim i As Integer
        Dim j As Integer
        Dim dPak_dEpsk As Double
        Dim dQak_dEpsk As Double
        Dim dRak_dEpsk As Double
        Dim Sum As Double = 0
        For i = 1 To PointNum
            j = ImageIndex
            If Imat(i, j) = 1 Then
                dPak_dEpsk = dPak_dT3(j)
                dQak_dEpsk = dQak_dT3(j)
                dRak_dEpsk = dRak_dT3(j)
                Sum = Sum + Calc_Ichiji_Bibun_KakkoNai(i, j, dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, 0, 0)
            End If
        Next
        Calc_dE_dEpsK_T3 = 2 * Sum
    End Function

    Private Function Calc_dE_dEpsK_W1(ByVal ImageIndex As Integer) As Double
        Dim i As Integer
        Dim j As Integer
        Dim dPak_dEpsk As Double
        Dim dQak_dEpsk As Double
        Dim dRak_dEpsk As Double
        Dim Sum As Double = 0
        For i = 1 To PointNum
            j = ImageIndex
            If Imat(i, j) = 1 Then
                dPak_dEpsk = dPak_dW1(i, j)
                dQak_dEpsk = dQak_dW1(i, j)
                dRak_dEpsk = dRak_dW1(i, j)
                Sum = Sum + Calc_Ichiji_Bibun_KakkoNai(i, j, dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, 0, 0)
            End If
        Next
        Calc_dE_dEpsK_W1 = 2 * Sum
    End Function

    Private Function Calc_dE_dEpsK_W2(ByVal ImageIndex As Integer) As Double
        Dim i As Integer
        Dim j As Integer
        Dim dPak_dEpsk As Double
        Dim dQak_dEpsk As Double
        Dim dRak_dEpsk As Double
        Dim Sum As Double = 0
        For i = 1 To PointNum
            j = ImageIndex
            If Imat(i, j) = 1 Then
                dPak_dEpsk = dPak_dW2(i, j)
                dQak_dEpsk = dQak_dW2(i, j)
                dRak_dEpsk = dRak_dW2(i, j)
                Sum = Sum + Calc_Ichiji_Bibun_KakkoNai(i, j, dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, 0, 0)
            End If
        Next
        Calc_dE_dEpsK_W2 = 2 * Sum
    End Function

    Private Function Calc_dE_dEpsK_W3(ByVal ImageIndex As Integer) As Double
        Dim i As Integer
        Dim j As Integer
        Dim dPak_dEpsk As Double
        Dim dQak_dEpsk As Double
        Dim dRak_dEpsk As Double
        Dim Sum As Double = 0
        For i = 1 To PointNum
            j = ImageIndex
            If Imat(i, j) = 1 Then
                dPak_dEpsk = dPak_dW3(i, j)
                dQak_dEpsk = dQak_dW3(i, j)
                dRak_dEpsk = dRak_dW3(i, j)
                Sum = Sum + Calc_Ichiji_Bibun_KakkoNai(i, j, dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, 0, 0)
            End If
        Next
        Calc_dE_dEpsK_W3 = 2 * Sum
    End Function

    Private Function Calc_Ichiji_Bibun_KakkoNai_delete(ByVal i As Integer, ByVal j As Integer, _
                                                ByVal dPak_dEpsk As Double, _
                                                ByVal dQak_dEpsk As Double, _
                                                ByVal dRak_dEpsk As Double) As Double
        Dim kakko1 As Double
        Dim kakko2 As Double
        Dim kakko3 As Double
        Dim kakko4 As Double
        Dim dblPak As Double = Pak(i, j)
        Dim dblQak As Double = Qak(i, j)
        Dim dblRak As Double = Rak(i, j)
        Dim P2d As Point2D = ImagePoints(i, j)
        kakko1 = dblPak / dblRak - P2d.x / F0
        kakko2 = dblRak * dPak_dEpsk - dblPak * dRak_dEpsk
        kakko3 = dblQak / dblRak - P2d.y / F0
        kakko4 = dblRak * dQak_dEpsk - dblQak * dRak_dEpsk

        Return (kakko1 * kakko2 + kakko3 * kakko4) / (dblRak * dblRak)

    End Function

    Private Function Calc_Ichiji_Bibun_KakkoNai(ByVal i As Integer, ByVal j As Integer, _
                                               ByVal dPak_dEpsk As Double, _
                                               ByVal dQak_dEpsk As Double, _
                                               ByVal dRak_dEpsk As Double, _
                                               ByVal dHXak_dEpsk As Double, _
                                               ByVal dHYak_dEpsk As Double) As Double
        Dim kakko1 As Double
        Dim kakko2 As Double
        Dim kakko3 As Double
        Dim kakko4 As Double
        Dim dblPak As Double = Pak(i, j)
        Dim dblQak As Double = Qak(i, j)
        Dim dblRak As Double = Rak(i, j)

        kakko1 = dblPak / dblRak - HXak(i, j) / F0
        kakko2 = (dblRak * dPak_dEpsk - dblPak * dRak_dEpsk) / (dblRak * dblRak) - dHXak_dEpsk / F0
        kakko3 = dblQak / dblRak - HYak(i, j) / F0
        kakko4 = (dblRak * dQak_dEpsk - dblQak * dRak_dEpsk) / (dblRak * dblRak) - dHYak_dEpsk / F0

        Calc_Ichiji_Bibun_KakkoNai = (kakko1 * kakko2 + kakko3 * kakko4)

    End Function


#End Region

#Region "二次微分要素"

    Private Function Calc_ddE_dEpsKL_PointsOnly_Faster(ByVal pointindex As Integer, _
                                                       ByVal imageindex As Integer, _
                                          ByVal I1 As Integer, _
                                          ByVal I2 As Integer) As Double
        Calc_ddE_dEpsKL_PointsOnly_Faster = 0
        Dim i As Integer
        Dim j As Integer
        Dim dPak_dEpsk As Double
        Dim dQak_dEpsk As Double
        Dim dRak_dEpsk As Double
        Dim dPak_dEpsl As Double
        Dim dQak_dEpsl As Double
        Dim dRak_dEpsl As Double
        i = pointindex
        j = imageindex

        Select Case I1
            Case 1
                dPak_dEpsk = dPak_dXb(j)
                dQak_dEpsk = dQak_dXb(j)
                dRak_dEpsk = dRak_dXb(j)
            Case 2
                dPak_dEpsk = dPak_dYb(j)
                dQak_dEpsk = dQak_dYb(j)
                dRak_dEpsk = dRak_dYb(j)
            Case 3
                dPak_dEpsk = dPak_dZb(j)
                dQak_dEpsk = dQak_dZb(j)
                dRak_dEpsk = dRak_dZb(j)
        End Select
        Select Case I2
            Case 1
                dPak_dEpsl = dPak_dXb(j)
                dQak_dEpsl = dQak_dXb(j)
                dRak_dEpsl = dRak_dXb(j)
            Case 2
                dPak_dEpsl = dPak_dYb(j)
                dQak_dEpsl = dQak_dYb(j)
                dRak_dEpsl = dRak_dYb(j)
            Case 3
                dPak_dEpsl = dPak_dZb(j)
                dQak_dEpsl = dQak_dZb(j)
                dRak_dEpsl = dRak_dZb(j)
        End Select

        Calc_ddE_dEpsKL_PointsOnly_Faster = Calc_ddE_dEpsKL_PointsOnly_Faster + _
                                        Calc_Niji_Bibun_KakkoNai(i, j, _
                                        dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, 0, 0, _
                                        dPak_dEpsl, dQak_dEpsl, dRak_dEpsl, 0, 0)

        Calc_ddE_dEpsKL_PointsOnly_Faster = 2 * Calc_ddE_dEpsKL_PointsOnly_Faster

    End Function

    Private Function Calc_ddE_dEpsKL_PointsOnly(ByVal pointindex As Integer, _
                                         ByVal I1 As Integer, _
                                         ByVal I2 As Integer) As Double
        Calc_ddE_dEpsKL_PointsOnly = 0
        Dim i As Integer
        Dim j As Integer
        Dim dPak_dEpsk As Double
        Dim dQak_dEpsk As Double
        Dim dRak_dEpsk As Double
        Dim dPak_dEpsl As Double
        Dim dQak_dEpsl As Double
        Dim dRak_dEpsl As Double
        i = pointindex
        For j = 1 To ImageNum
            If Imat(i, j) = 1 Then
                Select Case I1
                    Case 1
                        dPak_dEpsk = dPak_dXb(j)
                        dQak_dEpsk = dQak_dXb(j)
                        dRak_dEpsk = dRak_dXb(j)
                    Case 2
                        dPak_dEpsk = dPak_dYb(j)
                        dQak_dEpsk = dQak_dYb(j)
                        dRak_dEpsk = dRak_dYb(j)
                    Case 3
                        dPak_dEpsk = dPak_dZb(j)
                        dQak_dEpsk = dQak_dZb(j)
                        dRak_dEpsk = dRak_dZb(j)
                End Select

                Select Case I2
                    Case 1
                        dPak_dEpsl = dPak_dXb(j)
                        dQak_dEpsl = dQak_dXb(j)
                        dRak_dEpsl = dRak_dXb(j)
                    Case 2
                        dPak_dEpsl = dPak_dYb(j)
                        dQak_dEpsl = dQak_dYb(j)
                        dRak_dEpsl = dRak_dYb(j)
                    Case 3
                        dPak_dEpsl = dPak_dZb(j)
                        dQak_dEpsl = dQak_dZb(j)
                        dRak_dEpsl = dRak_dZb(j)
                End Select

                Calc_ddE_dEpsKL_PointsOnly = Calc_ddE_dEpsKL_PointsOnly + _
                                                Calc_Niji_Bibun_KakkoNai(i, j, _
                                                dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, 0, 0, _
                                                dPak_dEpsl, dQak_dEpsl, dRak_dEpsl, 0, 0)
            End If
        Next
        Calc_ddE_dEpsKL_PointsOnly = 2 * Calc_ddE_dEpsKL_PointsOnly

    End Function

    Private Function Calc_ddE_dEpsKL_ImageOnly(ByVal ImageIndex As Integer, ByVal J1 As Integer, ByVal J2 As Integer) As Double
        Dim i As Integer
        Dim j As Integer
        Dim dPak_dEpsk As Double
        Dim dQak_dEpsk As Double
        Dim dRak_dEpsk As Double
        Dim dPak_dEpsl As Double
        Dim dQak_dEpsl As Double
        Dim dRak_dEpsl As Double
        Calc_ddE_dEpsKL_ImageOnly = 0

        For i = 1 To PointNum
            j = ImageIndex
            If Imat(i, j) = 1 Then
                Calc_dPQRak_dKL_ImageToImage(i, j, J1, J2, dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, dPak_dEpsl, dQak_dEpsl, dRak_dEpsl)

                Calc_ddE_dEpsKL_ImageOnly = Calc_ddE_dEpsKL_ImageOnly + _
                                        Calc_Niji_Bibun_KakkoNai(i, j, _
                                        dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, 0, 0, _
                                        dPak_dEpsl, dQak_dEpsl, dRak_dEpsl, 0, 0)
            End If
        Next
        Calc_ddE_dEpsKL_ImageOnly = 2 * Calc_ddE_dEpsKL_ImageOnly


    End Function


    Private Function Calc_ddE_dEpsKL_ImageOnly_Faster(ByVal PointIndex As Integer, ByVal ImageIndex As Integer, _
                                                      ByVal J1 As Integer, ByVal J2 As Integer) As Double
        Dim i As Integer
        Dim j As Integer
        Dim dPak_dEpsk As Double
        Dim dQak_dEpsk As Double
        Dim dRak_dEpsk As Double
        Dim dPak_dEpsl As Double
        Dim dQak_dEpsl As Double
        Dim dRak_dEpsl As Double
        Calc_ddE_dEpsKL_ImageOnly_Faster = 0


        i = PointIndex
        j = ImageIndex

        Calc_dPQRak_dKL_ImageToImage(i, j, J1, J2, dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, dPak_dEpsl, dQak_dEpsl, dRak_dEpsl)

        Calc_ddE_dEpsKL_ImageOnly_Faster = Calc_ddE_dEpsKL_ImageOnly_Faster + _
                                Calc_Niji_Bibun_KakkoNai(i, j, _
                                dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, 0, 0, _
                                dPak_dEpsl, dQak_dEpsl, dRak_dEpsl, 0, 0)

        Calc_ddE_dEpsKL_ImageOnly_Faster = 2 * Calc_ddE_dEpsKL_ImageOnly_Faster


    End Function

    Private Function Calc_ddE_dEpsKL_GaibuToNaibu(ByVal ImageIndex As Integer, ByVal J1 As Integer, ByVal IC As Integer, ByVal N2 As Integer) As Double
        Dim i As Integer
        Dim j As Integer
        Dim dPak_dEpsk As Double
        Dim dQak_dEpsk As Double
        Dim dRak_dEpsk As Double
        Dim dPak_dEpsl As Double
        Dim dQak_dEpsl As Double
        Dim dRak_dEpsl As Double
        Dim dHXak_dEpsk As Double
        Dim dHYak_dEpsk As Double
        Dim dHXak_dEpsl As Double
        Dim dHYak_dEpsl As Double
        Dim sum As Double = 0
        Calc_ddE_dEpsKL_GaibuToNaibu = 0

        For i = 1 To PointNum
            j = ImageIndex
            If Imat(i, j) = 1 Then
                If Images(j).CID = IC Then
                    Calc_dPQRak_dKL_GaibuToNaibu(i, j, J1, N2, dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, _
                                                dHXak_dEpsk, dHYak_dEpsk, _
                                                dPak_dEpsl, dQak_dEpsl, dRak_dEpsl, _
                                                dHXak_dEpsl, dHYak_dEpsl)

                    Calc_ddE_dEpsKL_GaibuToNaibu = Calc_ddE_dEpsKL_GaibuToNaibu + Calc_Niji_Bibun_KakkoNai(i, j, _
                                                                                               dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, _
                                                                                               dHXak_dEpsk, dHYak_dEpsk, _
                                                                                               dPak_dEpsl, dQak_dEpsl, dRak_dEpsl, _
                                                                                               dHXak_dEpsl, dHYak_dEpsl)
                End If
            End If
        Next

        Calc_ddE_dEpsKL_GaibuToNaibu = 2 * Calc_ddE_dEpsKL_GaibuToNaibu


    End Function


    Private Function Calc_ddE_dEpsKL_GaibuToNaibu_Faster(ByVal PointIndex As Integer, ByVal ImageIndex As Integer, _
                                                         ByVal J1 As Integer, ByVal N2 As Integer) As Double
        Dim i As Integer
        Dim j As Integer
        Dim dPak_dEpsk As Double
        Dim dQak_dEpsk As Double
        Dim dRak_dEpsk As Double
        Dim dPak_dEpsl As Double
        Dim dQak_dEpsl As Double
        Dim dRak_dEpsl As Double
        Dim dHXak_dEpsk As Double
        Dim dHYak_dEpsk As Double
        Dim dHXak_dEpsl As Double
        Dim dHYak_dEpsl As Double
        Dim sum As Double = 0
        Calc_ddE_dEpsKL_GaibuToNaibu_Faster = 0

        i = PointIndex
        j = ImageIndex

        Calc_dPQRak_dKL_GaibuToNaibu(i, j, J1, N2, dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, _
                                    dHXak_dEpsk, dHYak_dEpsk, _
                                    dPak_dEpsl, dQak_dEpsl, dRak_dEpsl, _
                                    dHXak_dEpsl, dHYak_dEpsl)

        Calc_ddE_dEpsKL_GaibuToNaibu_Faster = Calc_ddE_dEpsKL_GaibuToNaibu_Faster + Calc_Niji_Bibun_KakkoNai(i, j, _
                                                                                   dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, _
                                                                                   dHXak_dEpsk, dHYak_dEpsk, _
                                                                                   dPak_dEpsl, dQak_dEpsl, dRak_dEpsl, _
                                                                                   dHXak_dEpsl, dHYak_dEpsl)

        Calc_ddE_dEpsKL_GaibuToNaibu_Faster = 2 * Calc_ddE_dEpsKL_GaibuToNaibu_Faster

    End Function


    Private Function Calc_dPQRak_dKL_GaibuToNaibu(ByVal a As Integer, ByVal k As Integer, ByVal J1 As Integer, ByVal N2 As Integer, _
                                        ByRef dPak_dEpsk As Double, _
                                        ByRef dQak_dEpsk As Double, _
                                        ByRef dRak_dEpsk As Double, _
                                        ByRef dHXak_dEpsk As Double, _
                                        ByRef dHYak_dEpsk As Double, _
                                        ByRef dPak_dEpsl As Double, _
                                        ByRef dQak_dEpsl As Double, _
                                        ByRef dRak_dEpsl As Double, _
                                        ByRef dHXak_dEpsl As Double, _
                                        ByRef dHYak_dEpsl As Double) As Double

        Select Case J1
            Case 1
                dPak_dEpsk = dPak_dT1(k)
                dQak_dEpsk = dQak_dT1(k)
                dRak_dEpsk = dRak_dT1(k)
            Case 2
                dPak_dEpsk = dPak_dT2(k)
                dQak_dEpsk = dQak_dT2(k)
                dRak_dEpsk = dRak_dT2(k)
            Case 3
                dPak_dEpsk = dPak_dT3(k)
                dQak_dEpsk = dQak_dT3(k)
                dRak_dEpsk = dRak_dT3(k)
            Case 4
                dPak_dEpsk = dPak_dW1(a, k)
                dQak_dEpsk = dQak_dW1(a, k)
                dRak_dEpsk = dRak_dW1(a, k)
            Case 5
                dPak_dEpsk = dPak_dW2(a, k)
                dQak_dEpsk = dQak_dW2(a, k)
                dRak_dEpsk = dRak_dW2(a, k)
            Case 6
                dPak_dEpsk = dPak_dW3(a, k)
                dQak_dEpsk = dQak_dW3(a, k)
                dRak_dEpsk = dRak_dW3(a, k)
        End Select


        dHXak_dEpsk = 0
        dHYak_dEpsk = 0

        Select Case N2
            Case 1
                dPak_dEpsl = dPak_dF(a, k)
                dQak_dEpsl = dQak_dF(a, k)
                dRak_dEpsl = dRak_dF(a, k)
                dHXak_dEpsl = 0
                dHYak_dEpsl = 0
            Case 2
                dPak_dEpsl = dPak_dU0(a, k)
                dQak_dEpsl = dQak_dU0()
                dRak_dEpsl = dRak_dU0()
                dHXak_dEpsl = dHXak_dU0(a, k)
                dHYak_dEpsl = dHYak_dU0(a, k)
            Case 3
                dPak_dEpsl = dPak_dV0()
                dQak_dEpsl = dQak_dV0(a, k)
                dRak_dEpsl = dRak_dV0()
                dHXak_dEpsl = dHXak_dV0(a, k)
                dHYak_dEpsl = dHYak_dV0(a, k)
            Case 4
                dPak_dEpsl = 0
                dQak_dEpsl = 0
                dRak_dEpsl = 0
                dHXak_dEpsl = dHXak_dK1(a, k)
                dHYak_dEpsl = dHYak_dK1(a, k)
            Case 5
                dPak_dEpsl = 0
                dQak_dEpsl = 0
                dRak_dEpsl = 0
                dHXak_dEpsl = dHXak_dK2(a, k)
                dHYak_dEpsl = dHYak_dK2(a, k)
            Case 6
                dPak_dEpsl = 0
                dQak_dEpsl = 0
                dRak_dEpsl = 0
                dHXak_dEpsl = dHXak_dK3(a, k)
                dHYak_dEpsl = dHYak_dK3(a, k)
            Case 7
                dPak_dEpsl = 0
                dQak_dEpsl = 0
                dRak_dEpsl = 0
                dHXak_dEpsl = dHXak_dP1(a, k)
                dHYak_dEpsl = dHYak_dP1(a, k)
            Case 8
                dPak_dEpsl = 0
                dQak_dEpsl = 0
                dRak_dEpsl = 0
                dHXak_dEpsl = dHXak_dP2(a, k)
                dHYak_dEpsl = dHYak_dP2(a, k)
        End Select


    End Function

    Private Function Calc_ddE_dEpsKL_NaibuToNaibu(ByVal IC As Integer, ByVal N1 As Integer, ByVal N2 As Integer) As Double
        Dim i As Integer
        Dim j As Integer
        Dim dPak_dEpsk As Double
        Dim dQak_dEpsk As Double
        Dim dRak_dEpsk As Double
        Dim dPak_dEpsl As Double
        Dim dQak_dEpsl As Double
        Dim dRak_dEpsl As Double
        Dim dHXak_dEpsk As Double
        Dim dHYak_dEpsk As Double
        Dim dHXak_dEpsl As Double
        Dim dHYak_dEpsl As Double
        Calc_ddE_dEpsKL_NaibuToNaibu = 0

        For i = 1 To PointNum
            For j = 1 To ImageNum
                If Imat(i, j) = 1 Then
                    If Images(j).CID = IC Then
                        Calc_dPQRak_dKL_NaibuToNaibu(i, j, N1, N2, dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, _
                                                dHXak_dEpsk, dHYak_dEpsk, _
                                                dPak_dEpsl, dQak_dEpsl, dRak_dEpsl, _
                                                dHXak_dEpsl, dHYak_dEpsl)

                        Calc_ddE_dEpsKL_NaibuToNaibu = Calc_ddE_dEpsKL_NaibuToNaibu + _
                                                        Calc_Niji_Bibun_KakkoNai(i, j, _
                                                dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, _
                                                dHXak_dEpsk, dHYak_dEpsk, _
                                                dPak_dEpsl, dQak_dEpsl, dRak_dEpsl, _
                                                dHXak_dEpsl, dHYak_dEpsl)
                    End If
                End If
            Next
        Next
        Calc_ddE_dEpsKL_NaibuToNaibu = 2 * Calc_ddE_dEpsKL_NaibuToNaibu


    End Function


    Private Function Calc_ddE_dEpsKL_NaibuToNaibu_Faster(ByVal PointIndex As Integer, ByVal ImageIndex As Integer, _
                                                         ByVal N1 As Integer, ByVal N2 As Integer) As Double
        Dim i As Integer
        Dim j As Integer
        Dim dPak_dEpsk As Double
        Dim dQak_dEpsk As Double
        Dim dRak_dEpsk As Double
        Dim dPak_dEpsl As Double
        Dim dQak_dEpsl As Double
        Dim dRak_dEpsl As Double
        Dim dHXak_dEpsk As Double
        Dim dHYak_dEpsk As Double
        Dim dHXak_dEpsl As Double
        Dim dHYak_dEpsl As Double
        Calc_ddE_dEpsKL_NaibuToNaibu_Faster = 0
        i = PointIndex
        j = ImageIndex
        Calc_dPQRak_dKL_NaibuToNaibu(i, j, N1, N2, dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, _
                                            dHXak_dEpsk, dHYak_dEpsk, _
                                            dPak_dEpsl, dQak_dEpsl, dRak_dEpsl, _
                                            dHXak_dEpsl, dHYak_dEpsl)
        Calc_ddE_dEpsKL_NaibuToNaibu_Faster = Calc_ddE_dEpsKL_NaibuToNaibu_Faster + _
                                                    Calc_Niji_Bibun_KakkoNai(i, j, _
                                            dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, _
                                            dHXak_dEpsk, dHYak_dEpsk, _
                                            dPak_dEpsl, dQak_dEpsl, dRak_dEpsl, _
                                            dHXak_dEpsl, dHYak_dEpsl)

        Calc_ddE_dEpsKL_NaibuToNaibu_Faster = 2 * Calc_ddE_dEpsKL_NaibuToNaibu_Faster


    End Function

    Private Function Calc_dPQRak_dKL_NaibuToNaibu(ByVal a As Integer, ByVal k As Integer, ByVal N1 As Integer, ByVal N2 As Integer, _
                                          ByRef dPak_dEpsk As Double, _
                                          ByRef dQak_dEpsk As Double, _
                                          ByRef dRak_dEpsk As Double, _
                                          ByRef dHXak_dEpsk As Double, _
                                          ByRef dHYak_dEpsk As Double, _
                                          ByRef dPak_dEpsl As Double, _
                                          ByRef dQak_dEpsl As Double, _
                                          ByRef dRak_dEpsl As Double, _
                                          ByRef dHXak_dEpsl As Double, _
                                          ByRef dHYak_dEpsl As Double) As Double
        Select Case N1
            Case 1
                dPak_dEpsk = dPak_dF(a, k)
                dQak_dEpsk = dQak_dF(a, k)
                dRak_dEpsk = dRak_dF(a, k)
                dHXak_dEpsk = 0
                dHYak_dEpsk = 0
            Case 2
                dPak_dEpsk = dPak_dU0(a, k)
                dQak_dEpsk = dQak_dU0()
                dRak_dEpsk = dRak_dU0()
                dHXak_dEpsk = dHXak_dU0(a, k)
                dHYak_dEpsk = dHYak_dU0(a, k)
            Case 3
                dPak_dEpsk = dPak_dV0()
                dQak_dEpsk = dQak_dV0(a, k)
                dRak_dEpsk = dRak_dV0()
                dHXak_dEpsk = dHXak_dV0(a, k)
                dHYak_dEpsk = dHYak_dV0(a, k)
            Case 4
                dPak_dEpsk = 0
                dQak_dEpsk = 0
                dRak_dEpsk = 0
                dHXak_dEpsk = dHXak_dK1(a, k)
                dHYak_dEpsk = dHYak_dK1(a, k)
            Case 5
                dPak_dEpsk = 0
                dQak_dEpsk = 0
                dRak_dEpsk = 0
                dHXak_dEpsk = dHXak_dK2(a, k)
                dHYak_dEpsk = dHYak_dK2(a, k)
            Case 6
                dPak_dEpsk = 0
                dQak_dEpsk = 0
                dRak_dEpsk = 0
                dHXak_dEpsk = dHXak_dK3(a, k)
                dHYak_dEpsk = dHYak_dK3(a, k)
            Case 7
                dPak_dEpsk = 0
                dQak_dEpsk = 0
                dRak_dEpsk = 0
                dHXak_dEpsk = dHXak_dP1(a, k)
                dHYak_dEpsk = dHYak_dP1(a, k)
            Case 8
                dPak_dEpsk = 0
                dQak_dEpsk = 0
                dRak_dEpsk = 0
                dHXak_dEpsk = dHXak_dP2(a, k)
                dHYak_dEpsk = dHYak_dP2(a, k)
        End Select

        Select Case N2
            Case 1
                dPak_dEpsl = dPak_dF(a, k)
                dQak_dEpsl = dQak_dF(a, k)
                dRak_dEpsl = dRak_dF(a, k)
                dHXak_dEpsl = 0
                dHYak_dEpsl = 0
            Case 2
                dPak_dEpsl = dPak_dU0(a, k)
                dQak_dEpsl = dQak_dU0()
                dRak_dEpsl = dRak_dU0()
                dHXak_dEpsl = dHXak_dU0(a, k)
                dHYak_dEpsl = dHYak_dU0(a, k)
            Case 3
                dPak_dEpsl = dPak_dV0()
                dQak_dEpsl = dQak_dV0(a, k)
                dRak_dEpsl = dRak_dV0()
                dHXak_dEpsl = dHXak_dV0(a, k)
                dHYak_dEpsl = dHYak_dV0(a, k)
            Case 4
                dPak_dEpsl = 0
                dQak_dEpsl = 0
                dRak_dEpsl = 0
                dHXak_dEpsl = dHXak_dK1(a, k)
                dHYak_dEpsl = dHYak_dK1(a, k)
            Case 5
                dPak_dEpsl = 0
                dQak_dEpsl = 0
                dRak_dEpsl = 0
                dHXak_dEpsl = dHXak_dK2(a, k)
                dHYak_dEpsl = dHYak_dK2(a, k)
            Case 6
                dPak_dEpsl = 0
                dQak_dEpsl = 0
                dRak_dEpsl = 0
                dHXak_dEpsl = dHXak_dK3(a, k)
                dHYak_dEpsl = dHYak_dK3(a, k)
            Case 7
                dPak_dEpsl = 0
                dQak_dEpsl = 0
                dRak_dEpsl = 0
                dHXak_dEpsl = dHXak_dP1(a, k)
                dHYak_dEpsl = dHYak_dP1(a, k)
            Case 8
                dPak_dEpsl = 0
                dQak_dEpsl = 0
                dRak_dEpsl = 0
                dHXak_dEpsl = dHXak_dP2(a, k)
                dHYak_dEpsl = dHYak_dP2(a, k)
        End Select


    End Function

    Private Function Calc_ddE_dEpsKL_PointsToImage(ByVal PointIndex As Integer, ByVal ImageIndex As Integer, _
                                                   ByVal I1 As Integer, ByVal J2 As Integer) As Double
        Dim i As Integer
        Dim j As Integer
        Dim dPak_dEpsk As Double
        Dim dQak_dEpsk As Double
        Dim dRak_dEpsk As Double
        Dim dPak_dEpsl As Double
        Dim dQak_dEpsl As Double
        Dim dRak_dEpsl As Double
        Calc_ddE_dEpsKL_PointsToImage = 0
        i = PointIndex
        j = ImageIndex
        If Imat(i, j) = 1 Then
            Calc_dPQRak_dKL_PointToImage(i, j, I1, J2, dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, dPak_dEpsl, dQak_dEpsl, dRak_dEpsl)
            Calc_ddE_dEpsKL_PointsToImage = Calc_ddE_dEpsKL_PointsToImage + _
                                    Calc_Niji_Bibun_KakkoNai(i, j, _
                                    dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, 0, 0, _
                                    dPak_dEpsl, dQak_dEpsl, dRak_dEpsl, 0, 0)
        End If
        Calc_ddE_dEpsKL_PointsToImage = 2 * Calc_ddE_dEpsKL_PointsToImage

    End Function

    Private Function Calc_ddE_dEpsKL_PointsToImageNaibu(ByVal PointIndex As Integer, ByVal I1 As Integer, ByVal IC As Integer, ByVal N2 As Integer) As Double
        Dim i As Integer
        Dim j As Integer
        Dim dPak_dEpsk As Double
        Dim dQak_dEpsk As Double
        Dim dRak_dEpsk As Double
        Dim dPak_dEpsl As Double
        Dim dQak_dEpsl As Double
        Dim dRak_dEpsl As Double
        Dim dHXak_dEpsk As Double
        Dim dHYak_dEpsk As Double
        Dim dHXak_dEpsl As Double
        Dim dHYak_dEpsl As Double

        Calc_ddE_dEpsKL_PointsToImageNaibu = 0
        i = PointIndex
        For j = 1 To ImageNum
            If Imat(i, j) = 1 Then
                If Images(j).CID = IC Then

                    Calc_dPQRak_dKL_PointsToImageNaibu(i, j, I1, N2, dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, _
                                                       dHXak_dEpsk, dHYak_dEpsk, _
                                                       dPak_dEpsl, dQak_dEpsl, dRak_dEpsl, _
                                                       dHXak_dEpsl, dHYak_dEpsl)
                    Calc_ddE_dEpsKL_PointsToImageNaibu = Calc_ddE_dEpsKL_PointsToImageNaibu + Calc_Niji_Bibun_KakkoNai(i, j, _
                                                                                         dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, _
                                                                                         dHXak_dEpsk, dHYak_dEpsk, _
                                                                                         dPak_dEpsl, dQak_dEpsl, dRak_dEpsl, _
                                                                                         dHXak_dEpsl, dHYak_dEpsl)
                End If
            End If
        Next

        Calc_ddE_dEpsKL_PointsToImageNaibu = 2 * Calc_ddE_dEpsKL_PointsToImageNaibu
    End Function


    Private Function Calc_ddE_dEpsKL_PointsToImageNaibu_Faster(ByVal PointIndex As Integer, ByVal ImageIndex As Integer, _
                                                               ByVal I1 As Integer, ByVal N2 As Integer) As Double
        Dim i As Integer
        Dim j As Integer
        Dim dPak_dEpsk As Double
        Dim dQak_dEpsk As Double
        Dim dRak_dEpsk As Double
        Dim dPak_dEpsl As Double
        Dim dQak_dEpsl As Double
        Dim dRak_dEpsl As Double
        Dim dHXak_dEpsk As Double
        Dim dHYak_dEpsk As Double
        Dim dHXak_dEpsl As Double
        Dim dHYak_dEpsl As Double

        Calc_ddE_dEpsKL_PointsToImageNaibu_Faster = 0
        i = PointIndex
        j = ImageIndex


        Calc_dPQRak_dKL_PointsToImageNaibu(i, j, I1, N2, dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, _
                                           dHXak_dEpsk, dHYak_dEpsk, _
                                           dPak_dEpsl, dQak_dEpsl, dRak_dEpsl, _
                                           dHXak_dEpsl, dHYak_dEpsl)
        Calc_ddE_dEpsKL_PointsToImageNaibu_Faster = Calc_ddE_dEpsKL_PointsToImageNaibu_Faster + Calc_Niji_Bibun_KakkoNai(i, j, _
                                                                             dPak_dEpsk, dQak_dEpsk, dRak_dEpsk, _
                                                                             dHXak_dEpsk, dHYak_dEpsk, _
                                                                             dPak_dEpsl, dQak_dEpsl, dRak_dEpsl, _
                                                                             dHXak_dEpsl, dHYak_dEpsl)

        Calc_ddE_dEpsKL_PointsToImageNaibu_Faster = 2 * Calc_ddE_dEpsKL_PointsToImageNaibu_Faster
    End Function


    Private Function Calc_dPQRak_dKL_PointsToImageNaibu(ByVal a As Integer, ByVal k As Integer, ByVal I1 As Integer, ByVal N2 As Integer, _
                                          ByRef dPak_dEpsk As Double, _
                                          ByRef dQak_dEpsk As Double, _
                                          ByRef dRak_dEpsk As Double, _
                                          ByRef dHXak_dEpsk As Double, _
                                          ByRef dHYak_dEpsk As Double, _
                                          ByRef dPak_dEpsl As Double, _
                                          ByRef dQak_dEpsl As Double, _
                                          ByRef dRak_dEpsl As Double, _
                                          ByRef dHXak_dEpsl As Double, _
                                          ByRef dHYak_dEpsl As Double) As Double
        Select Case I1
            Case 1
                dPak_dEpsk = dPak_dXb(k)
                dQak_dEpsk = dQak_dXb(k)
                dRak_dEpsk = dRak_dXb(k)
            Case 2
                dPak_dEpsk = dPak_dYb(k)
                dQak_dEpsk = dQak_dYb(k)
                dRak_dEpsk = dRak_dYb(k)
            Case 3
                dPak_dEpsk = dPak_dZb(k)
                dQak_dEpsk = dQak_dZb(k)
                dRak_dEpsk = dRak_dZb(k)
        End Select
        dHXak_dEpsk = 0
        dHYak_dEpsk = 0


        Select Case N2
            Case 1
                dPak_dEpsl = dPak_dF(a, k)
                dQak_dEpsl = dQak_dF(a, k)
                dRak_dEpsl = dRak_dF(a, k)
                dHXak_dEpsl = 0
                dHYak_dEpsl = 0
            Case 2
                dPak_dEpsl = dPak_dU0(a, k)
                dQak_dEpsl = dQak_dU0()
                dRak_dEpsl = dRak_dU0()
                dHXak_dEpsl = dHXak_dU0(a, k)
                dHYak_dEpsl = dHYak_dU0(a, k)
            Case 3
                dPak_dEpsl = dPak_dV0()
                dQak_dEpsl = dQak_dV0(a, k)
                dRak_dEpsl = dRak_dV0()
                dHXak_dEpsl = dHXak_dV0(a, k)
                dHYak_dEpsl = dHYak_dV0(a, k)
            Case 4
                dPak_dEpsl = 0
                dQak_dEpsl = 0
                dRak_dEpsl = 0
                dHXak_dEpsl = dHXak_dK1(a, k)
                dHYak_dEpsl = dHYak_dK1(a, k)
            Case 5
                dPak_dEpsl = 0
                dQak_dEpsl = 0
                dRak_dEpsl = 0
                dHXak_dEpsl = dHXak_dK2(a, k)
                dHYak_dEpsl = dHYak_dK2(a, k)
            Case 6
                dPak_dEpsl = 0
                dQak_dEpsl = 0
                dRak_dEpsl = 0
                dHXak_dEpsl = dHXak_dK3(a, k)
                dHYak_dEpsl = dHYak_dK3(a, k)
            Case 7
                dPak_dEpsl = 0
                dQak_dEpsl = 0
                dRak_dEpsl = 0
                dHXak_dEpsl = dHXak_dP1(a, k)
                dHYak_dEpsl = dHYak_dP1(a, k)
            Case 8
                dPak_dEpsl = 0
                dQak_dEpsl = 0
                dRak_dEpsl = 0
                dHXak_dEpsl = dHXak_dP2(a, k)
                dHYak_dEpsl = dHYak_dP2(a, k)
        End Select


    End Function



    Private Sub Calc_dPQRak_dKL_PointToImage(ByVal a As Integer, ByVal k As Integer, ByVal I1 As Integer, ByVal J1 As Integer, _
                                          ByRef dPak_dEpsk As Double, _
                                          ByRef dQak_dEpsk As Double, _
                                          ByRef dRak_dEpsk As Double, _
                                          ByRef dPak_dEpsl As Double, _
                                          ByRef dQak_dEpsl As Double, _
                                          ByRef dRak_dEpsl As Double)


        Select Case I1
            Case 1
                dPak_dEpsk = dPak_dXb(k)
                dQak_dEpsk = dQak_dXb(k)
                dRak_dEpsk = dRak_dXb(k)
            Case 2
                dPak_dEpsk = dPak_dYb(k)
                dQak_dEpsk = dQak_dYb(k)
                dRak_dEpsk = dRak_dYb(k)
            Case 3
                dPak_dEpsk = dPak_dZb(k)
                dQak_dEpsk = dQak_dZb(k)
                dRak_dEpsk = dRak_dZb(k)
        End Select

        If CamGaibuHensu = 6 Then
            Select Case J1
                Case 1
                    dPak_dEpsl = dPak_dT1(k)
                    dQak_dEpsl = dQak_dT1(k)
                    dRak_dEpsl = dRak_dT1(k)
                Case 2
                    dPak_dEpsl = dPak_dT2(k)
                    dQak_dEpsl = dQak_dT2(k)
                    dRak_dEpsl = dRak_dT2(k)
                Case 3
                    dPak_dEpsl = dPak_dT3(k)
                    dQak_dEpsl = dQak_dT3(k)
                    dRak_dEpsl = dRak_dT3(k)
                Case 4
                    dPak_dEpsl = dPak_dW1(a, k)
                    dQak_dEpsl = dQak_dW1(a, k)
                    dRak_dEpsl = dRak_dW1(a, k)
                Case 5
                    dPak_dEpsl = dPak_dW2(a, k)
                    dQak_dEpsl = dQak_dW2(a, k)
                    dRak_dEpsl = dRak_dW2(a, k)
                Case 6
                    dPak_dEpsl = dPak_dW3(a, k)
                    dQak_dEpsl = dQak_dW3(a, k)
                    dRak_dEpsl = dRak_dW3(a, k)
            End Select
        End If
    End Sub

    Private Sub Calc_dPQRak_dKL_ImageToImage(ByVal a As Integer, ByVal k As Integer, ByVal J1 As Integer, ByVal J2 As Integer, _
                                ByRef dPak_dEpsk As Double, _
                                ByRef dQak_dEpsk As Double, _
                                ByRef dRak_dEpsk As Double, _
                                ByRef dPak_dEpsl As Double, _
                                ByRef dQak_dEpsl As Double, _
                                ByRef dRak_dEpsl As Double)

        If CamGaibuHensu = 6 Then
            Select Case J1
                Case 1
                    dPak_dEpsk = dPak_dT1(k)
                    dQak_dEpsk = dQak_dT1(k)
                    dRak_dEpsk = dRak_dT1(k)
                Case 2
                    dPak_dEpsk = dPak_dT2(k)
                    dQak_dEpsk = dQak_dT2(k)
                    dRak_dEpsk = dRak_dT2(k)
                Case 3
                    dPak_dEpsk = dPak_dT3(k)
                    dQak_dEpsk = dQak_dT3(k)
                    dRak_dEpsk = dRak_dT3(k)
                Case 4
                    dPak_dEpsk = dPak_dW1(a, k)
                    dQak_dEpsk = dQak_dW1(a, k)
                    dRak_dEpsk = dRak_dW1(a, k)
                Case 5
                    dPak_dEpsk = dPak_dW2(a, k)
                    dQak_dEpsk = dQak_dW2(a, k)
                    dRak_dEpsk = dRak_dW2(a, k)
                Case 6
                    dPak_dEpsk = dPak_dW3(a, k)
                    dQak_dEpsk = dQak_dW3(a, k)
                    dRak_dEpsk = dRak_dW3(a, k)
            End Select
            Select Case J2
                Case 1
                    dPak_dEpsl = dPak_dT1(k)
                    dQak_dEpsl = dQak_dT1(k)
                    dRak_dEpsl = dRak_dT1(k)
                Case 2
                    dPak_dEpsl = dPak_dT2(k)
                    dQak_dEpsl = dQak_dT2(k)
                    dRak_dEpsl = dRak_dT2(k)
                Case 3
                    dPak_dEpsl = dPak_dT3(k)
                    dQak_dEpsl = dQak_dT3(k)
                    dRak_dEpsl = dRak_dT3(k)
                Case 4
                    dPak_dEpsl = dPak_dW1(a, k)
                    dQak_dEpsl = dQak_dW1(a, k)
                    dRak_dEpsl = dRak_dW1(a, k)
                Case 5
                    dPak_dEpsl = dPak_dW2(a, k)
                    dQak_dEpsl = dQak_dW2(a, k)
                    dRak_dEpsl = dRak_dW2(a, k)
                Case 6
                    dPak_dEpsl = dPak_dW3(a, k)
                    dQak_dEpsl = dQak_dW3(a, k)
                    dRak_dEpsl = dRak_dW3(a, k)
            End Select
        End If
    End Sub

    Private Function Calc_Niji_Bibun_KakkoNai(ByVal i As Integer, ByVal j As Integer, _
                                            ByVal dPak_dEpsk As Double, _
                                            ByVal dQak_dEpsk As Double, _
                                            ByVal dRak_dEpsk As Double, _
                                            ByVal dHXak_dEpsk As Double, _
                                            ByVal dHYak_dEpsk As Double, _
                                            ByVal dPak_dEpsl As Double, _
                                            ByVal dQak_dEpsl As Double, _
                                            ByVal dRak_dEpsl As Double, _
                                            ByVal dHXak_dEpsl As Double, _
                                            ByVal dHYak_dEpsl As Double) As Double
        Dim kakko1 As Double
        Dim kakko2 As Double
        Dim kakko3 As Double
        Dim kakko4 As Double
        Dim dblPak As Double = Pak(i, j)
        Dim dblQak As Double = Qak(i, j)
        Dim dblRak As Double = Rak(i, j)
        If dblRak = 0 Then
            Calc_Niji_Bibun_KakkoNai = 0
            Exit Function
        End If
        kakko1 = ((dblRak * dPak_dEpsk - dblPak * dRak_dEpsk) / (dblRak * dblRak)) - (dHXak_dEpsk / F0)
        kakko2 = ((dblRak * dPak_dEpsl - dblPak * dRak_dEpsl) / (dblRak * dblRak)) - (dHXak_dEpsl / F0)
        kakko3 = ((dblRak * dQak_dEpsk - dblQak * dRak_dEpsk) / (dblRak * dblRak)) - (dHYak_dEpsk / F0)
        kakko4 = ((dblRak * dQak_dEpsl - dblQak * dRak_dEpsl) / (dblRak * dblRak)) - (dHYak_dEpsl / F0)

        Calc_Niji_Bibun_KakkoNai = (kakko1 * kakko2 + kakko3 * kakko4)

    End Function

#End Region

#Region "Pak,Qak,Rakの一階微分"

#Region "Pak,Qak,Rakの３Dポイントに対する一階微分"
    Private Function dPak_dXb(ByVal k As Integer) As Double
        Return Images(k).P.P11
    End Function
    Private Function dPak_dYb(ByVal k As Integer) As Double
        Return Images(k).P.P12
    End Function
    Private Function dPak_dZb(ByVal k As Integer) As Double
        Return Images(k).P.P13
    End Function
    Private Function dQak_dXb(ByVal k As Integer) As Double
        Return Images(k).P.P21
    End Function
    Private Function dQak_dYb(ByVal k As Integer) As Double
        Return Images(k).P.P22
    End Function
    Private Function dQak_dZb(ByVal k As Integer) As Double
        Return Images(k).P.P23
    End Function
    Private Function dRak_dXb(ByVal k As Integer) As Double
        Return Images(k).P.P31
    End Function
    Private Function dRak_dYb(ByVal k As Integer) As Double
        Return Images(k).P.P32
    End Function
    Private Function dRak_dZb(ByVal k As Integer) As Double
        Return Images(k).P.P33
    End Function

#End Region

#Region "Pak,Qak,Rakの焦点距離に対する一階微分"

    Private Function dPak_dF(ByVal a As Integer, ByVal k As Integer) As Double
        Return (Pak(a, k) - (Images(k).CNP.U0 / F0) * Rak(a, k)) / Images(k).CNP.F
    End Function

    Private Function dQak_dF(ByVal a As Integer, ByVal k As Integer) As Double
        Return (Qak(a, k) - (Images(k).CNP.V0 / F0) * Rak(a, k)) / Images(k).CNP.F  'SUURI 20111114 U0->V0に修正
    End Function

    Private Function dRak_dF(ByVal a As Integer, ByVal k As Integer) As Double
        Return 0
    End Function

#End Region

#Region "Pak,Qak,Rakの光軸点に対する一階微分"
    'mijikai
    Private Function dPak_dU0(ByVal a As Integer, ByVal k As Integer) As Double
        Return Rak(a, k) / (F0)
    End Function
    Private Function dQak_dU0() As Double
        Return 0
    End Function
    Private Function dRak_dU0() As Double
        Return 0
    End Function

    Private Function dPak_dV0() As Double
        Return 0
    End Function
    Private Function dQak_dV0(ByVal a As Integer, ByVal k As Integer) As Double
        Return Rak(a, k) / (F0)
    End Function
    Private Function dRak_dV0() As Double
        Return 0
    End Function

#End Region

#Region "Pak,Qak,Rakの並進に対する一階微分"
    'T1に対する一階微分
    Private Function dPak_dT1(ByVal k As Integer) As Double

        Return (-1) * (Images(k).CNP.A * Images(k).CNP.F * Images(k).R.R11 + Images(k).CNP.U0 * Images(k).R.R13) / F0

    End Function

    Private Function dQak_dT1(ByVal k As Integer) As Double

        Return (-1) * (Images(k).CNP.F * Images(k).R.R12 + Images(k).CNP.V0 * Images(k).R.R13) / F0

    End Function

    Private Function dRak_dT1(ByVal k As Integer) As Double

        Return (-1) * (F0 * Images(k).R.R13) / F0

    End Function

    'T2に対する一階微分
    Private Function dPak_dT2(ByVal k As Integer) As Double

        Return (-1) * (Images(k).CNP.A * Images(k).CNP.F * Images(k).R.R21 + Images(k).CNP.U0 * Images(k).R.R23) / F0

    End Function

    Private Function dQak_dT2(ByVal k As Integer) As Double

        Return (-1) * (Images(k).CNP.F * Images(k).R.R22 + Images(k).CNP.V0 * Images(k).R.R23) / F0

    End Function

    Private Function dRak_dT2(ByVal k As Integer) As Double

        Return (-1) * (F0 * Images(k).R.R23) / F0

    End Function


    'T3に対する一階微分
    Private Function dPak_dT3(ByVal k As Integer) As Double

        Return (-1) * (Images(k).CNP.A * Images(k).CNP.F * Images(k).R.R31 + Images(k).CNP.U0 * Images(k).R.R33) / F0

    End Function

    Private Function dQak_dT3(ByVal k As Integer) As Double

        Return (-1) * (Images(k).CNP.F * Images(k).R.R32 + Images(k).CNP.V0 * Images(k).R.R33) / F0

    End Function

    Private Function dRak_dT3(ByVal k As Integer) As Double

        Return (-1) * (F0 * Images(k).R.R33) / F0

    End Function

#End Region

#Region "Pak,Qak,Rakの回転に対する一階微分"
    'W1に対する一階微分
    Private Function dPak_dW1(ByVal a As Integer, ByVal k As Integer) As Double

        Return dPak_dW123(a, k).X

    End Function

    Private Function dQak_dW1(ByVal a As Integer, ByVal k As Integer) As Double

        Return dQak_dW123(a, k).X

    End Function

    Private Function dRak_dW1(ByVal a As Integer, ByVal k As Integer) As Double

        Return dRak_dW123(a, k).X

    End Function

    'W2に対する一階微分
    Private Function dPak_dW2(ByVal a As Integer, ByVal k As Integer) As Double

        Return dPak_dW123(a, k).Y

    End Function

    Private Function dQak_dW2(ByVal a As Integer, ByVal k As Integer) As Double

        Return dQak_dW123(a, k).Y
    End Function

    Private Function dRak_dW2(ByVal a As Integer, ByVal k As Integer) As Double

        Return dRak_dW123(a, k).Y

    End Function

    'W3に対する一階微分
    Private Function dPak_dW3(ByVal a As Integer, ByVal k As Integer) As Double
        dPak_dW3 = dPak_dW123(a, k).Z
    End Function

    Private Function dQak_dW3(ByVal a As Integer, ByVal k As Integer) As Double
        dQak_dW3 = dQak_dW123(a, k).Z
    End Function

    Private Function dRak_dW3(ByVal a As Integer, ByVal k As Integer) As Double
        dRak_dW3 = dRak_dW123(a, k).Z
    End Function

    Private Function dPak_dW123(ByVal a As Integer, ByVal k As Integer) As Point3D
        Dim Rk1 As New Point3D
        Dim Rk3 As New Point3D
        Dim Tk As New Point3D
        Dim RK As RMat = Images(k).R

        Rk1.X = RK.R11
        Rk1.Y = RK.R21
        Rk1.Z = RK.R31

        Rk3.X = RK.R13
        Rk3.Y = RK.R23
        Rk3.Z = RK.R33

        Tk.CopyToMe(Images(k).T)
        Dim Tasu As New Point3D
        Dim Hiku As New Point3D

        Rk1.Scale(Images(k).CNP.A * Images(k).CNP.F / F0)
        Rk3.Scale(Images(k).CNP.U0 / F0)
        Tasu = Rk1.AddTo(Rk3)
        Tk.Scale(-1)
        Hiku = Points(a).AddTo(Tk)
        dPak_dW123 = Tasu.VectorMult(Hiku)

    End Function

    Private Function dQak_dW123(ByVal a As Integer, ByVal k As Integer) As Point3D
        Dim Rk2 As New Point3D
        Dim Rk3 As New Point3D
        Dim Tk As New Point3D
        Dim RK As RMat = Images(k).R

        Rk2.X = RK.R12
        Rk2.Y = RK.R22
        Rk2.Z = RK.R32

        Rk3.X = RK.R13
        Rk3.Y = RK.R23
        Rk3.Z = RK.R33

        Tk.CopyToMe(Images(k).T)
        Dim Tasu As New Point3D
        Dim Hiku As New Point3D

        Rk2.Scale(Images(k).CNP.F / F0)
        Rk3.Scale(Images(k).CNP.V0 / F0)
        Tasu = Rk2.AddTo(Rk3)
        Tk.Scale(-1)
        Hiku = Points(a).AddTo(Tk)
        dQak_dW123 = Tasu.VectorMult(Hiku)
    End Function

    Private Function dRak_dW123(ByVal a As Integer, ByVal k As Integer) As Point3D
        Dim Rk3 As New Point3D
        Dim Tk As New Point3D
        Dim RK As RMat = Images(k).R

        Rk3.X = RK.R13
        Rk3.Y = RK.R23
        Rk3.Z = RK.R33

        Tk.CopyToMe(Images(k).T)
        Dim Hiku As New Point3D
        Rk3.Scale(F0 / F0)

        Tk.Scale(-1)
        Hiku = Points(a).AddTo(Tk)
        dRak_dW123 = Rk3.VectorMult(Hiku)
    End Function

#End Region

#End Region

#Region "HXak,HYakをU0,V0,K1,K2,K3,P1,P2での一階微分"
    Private Function K123(ByVal a As Integer, ByVal k As Integer)
        Return Images(k).CNP.K1 * Dak(a, k) + Images(k).CNP.K2 * Dak(a, k) * Dak(a, k) + Images(k).CNP.K3 * Dak(a, k) * Dak(a, k) * Dak(a, k)
    End Function
    Private Function dK123_dU0(ByVal a As Integer, ByVal k As Integer)
        Return Images(k).CNP.K1 * dDak_dU0(a, k) + Images(k).CNP.K2 * (2 * Dak(a, k) * dDak_dU0(a, k)) + Images(k).CNP.K3 * (3 * Dak(a, k) * Dak(a, k) * dDak_dU0(a, k))
    End Function
    Private Function dK123_dV0(ByVal a As Integer, ByVal k As Integer)
        Return Images(k).CNP.K1 * dDak_dV0(a, k) + Images(k).CNP.K2 * (2 * Dak(a, k) * dDak_dV0(a, k)) + Images(k).CNP.K3 * (3 * Dak(a, k) * Dak(a, k) * dDak_dV0(a, k))
    End Function
    Private Function dDak_dU0(ByVal a As Integer, ByVal k As Integer)
        Return 2 * Uak(a, k) * (-1) * Images(k).CNP.Sx
    End Function
    Private Function dDak_dV0(ByVal a As Integer, ByVal k As Integer)
        Return 2 * Vak(a, k) * (-1) * Images(k).CNP.Sy
    End Function
    Private Function dUak_2_dU0(ByVal a As Integer, ByVal k As Integer)
        Return 2 * Uak(a, k) * (-1) * Images(k).CNP.Sx
    End Function
    Private Function dVak_2_dV0(ByVal a As Integer, ByVal k As Integer)
        Return 2 * Vak(a, k) * (-1) * Images(k).CNP.Sy
    End Function
    Private Function dUak_dU0(ByVal a As Integer, ByVal k As Integer)
        Return (-1) * Images(k).CNP.Sx
    End Function
    Private Function dVak_dV0(ByVal a As Integer, ByVal k As Integer)
        Return (-1) * Images(k).CNP.Sy
    End Function
    Private Function dHXak_dU0(ByVal a As Integer, ByVal k As Integer)
        Return (K123(a, k) * dUak_dU0(a, k) + _
                Uak(a, k) * dK123_dU0(a, k) + _
                2 * Images(k).CNP.P2 * Vak(a, k) * dUak_dU0(a, k) + _
                Images(k).CNP.P1 * dDak_dU0(a, k) + _
                2 * Images(k).CNP.P1 * dUak_2_dU0(a, k)) / Images(k).CNP.Sx
    End Function
    Private Function dHYak_dU0(ByVal a As Integer, ByVal k As Integer)
        Return (Vak(a, k) * dK123_dU0(a, k) + _
               Images(k).CNP.P2 * dDak_dU0(a, k) + _
                2 * Images(k).CNP.P1 * Vak(a, k) * dUak_dU0(a, k)) / Images(k).CNP.Sy
    End Function
    Private Function dHXak_dV0(ByVal a As Integer, ByVal k As Integer)
        Return (Uak(a, k) * dK123_dV0(a, k) + _
                2 * Images(k).CNP.P2 * Uak(a, k) * dVak_dV0(a, k) + _
                Images(k).CNP.P1 * dDak_dV0(a, k)) / Images(k).CNP.Sx
    End Function
    Private Function dHYak_dV0(ByVal a As Integer, ByVal k As Integer)
        Return (K123(a, k) * dVak_dV0(a, k) + _
                Vak(a, k) * dK123_dV0(a, k) + _
                Images(k).CNP.P2 * dDak_dV0(a, k) + _
                2 * Images(k).CNP.P2 * dVak_2_dV0(a, k) + _
                2 * Images(k).CNP.P1 * Uak(a, k) * dVak_dV0(a, k)) / Images(k).CNP.Sy
    End Function
    Private Function dHXak_dK1(ByVal a As Integer, ByVal k As Integer)
        Return (Uak(a, k) * Dak(a, k)) / Images(k).CNP.Sx
    End Function
    Private Function dHYak_dK1(ByVal a As Integer, ByVal k As Integer)
        Return (Vak(a, k) * Dak(a, k)) / Images(k).CNP.Sy
    End Function
    Private Function dHXak_dK2(ByVal a As Integer, ByVal k As Integer)
        Return Uak(a, k) * Dak(a, k) * Dak(a, k) / Images(k).CNP.Sx
    End Function
    Private Function dHYak_dK2(ByVal a As Integer, ByVal k As Integer)
        Return Vak(a, k) * Dak(a, k) * Dak(a, k) / Images(k).CNP.Sy
    End Function
    Private Function dHXak_dK3(ByVal a As Integer, ByVal k As Integer)
        Return Uak(a, k) * Dak(a, k) * Dak(a, k) * Dak(a, k) / Images(k).CNP.Sx
    End Function
    Private Function dHYak_dK3(ByVal a As Integer, ByVal k As Integer)
        Return Vak(a, k) * Dak(a, k) * Dak(a, k) * Dak(a, k) / Images(k).CNP.Sy
    End Function
    Private Function dHXak_dP1(ByVal a As Integer, ByVal k As Integer)
        Return (Dak(a, k) + 2 * Uak(a, k) * Uak(a, k)) / Images(k).CNP.Sx
    End Function
    Private Function dHYak_dP1(ByVal a As Integer, ByVal k As Integer)
        Return 2 * Uak(a, k) * Vak(a, k) / Images(k).CNP.Sy
    End Function
    Private Function dHXak_dP2(ByVal a As Integer, ByVal k As Integer)
        Return 2 * Uak(a, k) * Vak(a, k) / Images(k).CNP.Sx
    End Function
    Private Function dHYak_dP2(ByVal a As Integer, ByVal k As Integer)
        Return (Dak(a, k) + 2 * Vak(a, k) * Vak(a, k)) / Images(k).CNP.Sy
    End Function
#End Region

    'Monitor用
    'Public Sub OutdEmat(ByVal strFileName As String)
    '    Dim i As Integer
    '    Dim strText As String = ""
    '    For i = 1 To Shikisuu
    '        strText = strText & dEmat(i) & vbNewLine
    '    Next
    '    My.Computer.FileSystem.WriteAllText(strFileName, strText, False)

    'End Sub

    'Public Sub OutddEmat(ByVal strFileName As String)

    '    Dim i As Integer
    '    Dim j As Integer
    '    Dim strText As String = ""
    '    Dim strRowText As String
    '    For i = 1 To Shikisuu
    '        strRowText = ""
    '        For j = 1 To Shikisuu
    '            strRowText = strRowText & ddEmat(i, j) & ","
    '        Next
    '        strText = strText & strRowText & vbNewLine
    '    Next
    '    My.Computer.FileSystem.WriteAllText(strFileName, strText, False)

    'End Sub


End Class
