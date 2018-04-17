Public Class BAmain
    Public objBAmain As BAdata
    Public objBAKoushin As BAdata
    Public kekkaCamMat(8) As Double
    Public RMS As Double
    Public ProjectPath As String
    Private flgPoseOnly As Boolean
    Public Sub New()
        objBAmain = New BAdata
        RMS = -1
        flgPoseOnly = False
    End Sub
    Public Sub RunBA(ByVal flgSyori As Boolean)

        ''第一バンドル調整
        'CamNaibuHensu = 6
        'BA_Hontai(FirstE)

        '第二バンドル調整
        If flgSyori = True Then
            CamNaibuHensu = 8
        Else
            CamNaibuHensu = 0
        End If

        BA_Hontai()

    End Sub

    Public Sub RunBA_PoseOnly()

        CamNaibuHensu = 0
        CamGaibuHensu = 6
        PointHensu = 0
        KoteiHensu = 0
        flgPoseOnly = True
        BA_Hontai()

    End Sub
    Public Sub RunBA_3DPoint_Pose()
        CamNaibuHensu = 0
        BA_Hontai()
    End Sub

    Public Sub BA_Hontai()
        'Dim ddEMatID As Object = Nothing
        'Dim dEMatID As Object = Nothing
        Dim DeltaMat() As Double = {}
        Dim C As Double = 0.0001
        Dim Sigma As Double
        Dim pixKyoyouGosa As Double = 0.01

        Dim E As Double = 0
        Dim FirstE As Double = 0
        Dim i As Integer
        Dim strMonitor As String = ""
        Dim tmpE As Double = Double.MaxValue
        Dim pixE As Double
        ' Dim sw As New System.Diagnostics.Stopwatch()
        ' objBAmain.Reconst3D()
        '手順１
        objBAmain.ResetEmat()
        objBAmain.ResetdE_E_F_Gmat()
        objBAKoushin = New BAdata(objBAmain)

        JikanMonStart()
        StopWatchResetStart()
        objBAmain.CalcReProjectionError()
        JikanMonitor("CalcReProjectionError:")

        'Dim ttt As Double = objBAmain.dbl_SqrtE

        'sw.Reset()
        'sw.Start()
        'objBAmain.CalcReProjectionError_Para()
        'sw.Stop()
        'My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "JikanMonitor.mon",
        '                                   "CalcReProjectionError_Para： " & sw.Elapsed.TotalSeconds & "秒" & vbNewLine, True)
        'Trace.WriteLine("CalcReProjectionError_Para：")
        'Trace.WriteLine(sw.Elapsed.TotalSeconds & "秒")
        'Dim ddd As Double = objBAmain.dbl_SqrtE

        E = objBAmain.dblE
        FirstE = E
        Sigma = (objBAmain.N * pixKyoyouGosa * pixKyoyouGosa) / (F0 * F0)
        RMS = F0 * Math.Sqrt(E / (objBAmain.N * 2))
        strMonitor = strMonitor & "＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊" & vbNewLine
        strMonitor = strMonitor & "バンドル調整開始：" & Now & vbNewLine
        strMonitor = strMonitor & "初期状態" & vbNewLine
        Monitor(strMonitor)


        For i = 1 To 10

            StopWatchResetStart()
            'objBAmain.CalcPQRak()                               '手順２
            objBAmain.Calc_dEmat_para()                              '手順２
            JikanMonitor("Calc_dEmat_para:")

            StopWatchResetStart()
            objBAmain.Calc_EFGmat_para()
            JikanMonitor("Calc_EFGmat_para:")

            '  strMonitor = strMonitor & "更新量の計算開始：" & Now & vbNewLine
            Dim cntD As Integer = 0

            Do

                If cntD = 3 Then
                    Exit Do
                End If
                cntD += 1
                objBAmain.Calc_DeltaMat_yti(C, DeltaMat)

                StopWatchResetStart()
                objBAKoushin.BaDataKoushin(objBAmain, DeltaMat)
                JikanMonitor("BaDataKoushin:")
                System.Threading.Thread.Sleep(500)
                If objBAKoushin.dblE >= E Then
                    C = C * 10
                    strMonitor = strMonitor & C & "," & objBAKoushin.dblE & "," & E & vbNewLine
                Else
                    Exit Do
                End If
            Loop
            If cntD = 3 Then
                MsgBox("バンドル調整が収束しませんでした。！")
                Exit For
            End If
            StopWatchResetStart()
            objBAmain.BaDataCopyToMe(objBAKoushin)
            JikanMonitor("BaDataCopyToMe:")

            pixE = F0 * (Math.Sqrt(E / (2 * objBAmain.N - objBAmain.Shikisuu)))

            If Math.Abs(objBAKoushin.dblE - E) <= Sigma Then
                '終了
                Exit For
            Else
                '継続
                C = C / 10
                strMonitor = strMonitor & C & "," & objBAKoushin.dblE & "," & E & vbNewLine
                E = objBAKoushin.dblE
            End If

        Next

        RMS = F0 * Math.Sqrt(E / (objBAmain.N * 2))
        strMonitor = strMonitor & "最終結果" & vbNewLine
        Monitor(strMonitor)
        strMonitor = strMonitor & "バンドル調整終了：" & Now & vbNewLine
        strMonitor = strMonitor & "＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊" & vbNewLine
        My.Computer.FileSystem.WriteAllText(ProjectPath & "\BAMonitor.csv", strMonitor, True)

        kekkaCamMat(0) = objBAmain.CNP.A * objBAmain.CNP.F
        kekkaCamMat(1) = 0
        kekkaCamMat(2) = objBAmain.CNP.U0
        kekkaCamMat(3) = 0
        kekkaCamMat(4) = objBAmain.CNP.F
        kekkaCamMat(5) = objBAmain.CNP.V0
        kekkaCamMat(6) = 0
        kekkaCamMat(7) = 0
        kekkaCamMat(8) = 1

    End Sub

    Private Sub Monitor(ByRef strMonitor As String)
        strMonitor = strMonitor & "再投影誤差：" & objBAmain.dblE & vbNewLine
        strMonitor = strMonitor & "再投影誤差合計(pix)：" & objBAmain.dbl_SqrtE & vbNewLine
        strMonitor = strMonitor & "再投影誤差平均(pix)：" & objBAmain.dbl_SqrtE / (objBAmain.N) & vbNewLine
        strMonitor = strMonitor & "RMS：" & RMS & vbNewLine
    End Sub


    Public Sub SetInitData(ByVal CamPar As Object, ByVal INum As Integer, ByVal PNum As Integer, ByVal dblF0 As Double, ByVal flgSyori As Integer)
        CamParam = CamPar
        ImageNum = INum
        PointNum = PNum
        F0 = dblF0
        '  CamParamToCamMat()
        If flgSyori = 1 Then
            CamGaibuHensu = 6
            CamNaibuHensu = 3
            PointHensu = 3
        ElseIf flgSyori = 2 Then
            CamGaibuHensu = 6
            CamNaibuHensu = 0
            PointHensu = 3
        ElseIf flgSyori = 3 Then
            CamGaibuHensu = 6
            CamNaibuHensu = 0
            PointHensu = 0
        ElseIf flgSyori = 4 Then
            CamGaibuHensu = 6
            CamNaibuHensu = 8
            PointHensu = 3
        End If

        objBAmain = New BAdata(1)

    End Sub
End Class
