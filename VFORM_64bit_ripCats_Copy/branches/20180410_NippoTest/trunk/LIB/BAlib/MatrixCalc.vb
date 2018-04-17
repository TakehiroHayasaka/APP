Module MatrixCalc
    Public Const MNORM As Integer = 1
    Public Const MERR As Integer = 2
    Public Const MARGERR As Integer = 3

    ''' <summary>
    ''' 行列を定数で掛け算する。
    ''' </summary>
    ''' <param name="A">入力: 行列</param>
    ''' <param name="T">入力: 定数</param>
    ''' <param name="B">出力: txA 行列</param>
    ''' <returns>MNORM:正常　　MERR: 異常　</returns>
    ''' <remarks></remarks>
    Public Function MScale(ByVal A(,) As Double, ByVal T As Double, ByRef B(,) As Double) As Integer
        Dim N As Integer = A.GetUpperBound(0)
        Dim M As Integer = A.GetUpperBound(1)
        Dim i As Integer
        Dim j As Integer
        ReDim B(N, M)

        Try
            For i = 0 To N
                For j = 0 To M
                    B(i, j) = A(i, j) * T
                Next
            Next
            Return MNORM
        Catch ex As Exception
            Return MERR
        End Try

    End Function

    ''' <summary>
    ''' 行列を定数で掛け算する。
    ''' </summary>
    ''' <param name="A">入力: 行列</param>
    ''' <param name="T">入力: 定数</param>
    ''' <returns>MNORM:正常　　MERR: 異常　</returns>
    ''' <remarks></remarks>
    Public Function MScaleMod(ByRef A(,) As Double, ByVal T As Double) As Integer
        Dim N As Integer = A.GetUpperBound(0)
        Dim M As Integer = A.GetUpperBound(1)
        Dim i As Integer
        Dim j As Integer
       
        Try
            For i = 0 To N
                For j = 0 To M
                    A(i, j) = A(i, j) * T
                Next
            Next
            Return MNORM
        Catch ex As Exception
            Return MERR
        End Try

    End Function

    ''' <summary>
    ''' 行列と行列の掛け算する。
    ''' </summary>
    ''' <param name="A">入力: 掛け算の左側の行列</param>
    ''' <param name="B">入力: 掛け算の右側の行列</param>
    ''' <param name="AxB">出力: AxB 行列</param>
    ''' <returns>MNORM:正常　　MERR: 異常　MARGERR: 引数エラー</returns>
    ''' <remarks></remarks>
    Public Function MMult(ByVal A(,) As Double, ByVal B(,) As Double, ByRef AxB(,) As Double) As Integer
        Dim NA As Integer = A.GetUpperBound(0)
        Dim MA As Integer = A.GetUpperBound(1)
        Dim NB As Integer = B.GetUpperBound(0)
        Dim MB As Integer = B.GetUpperBound(1)
        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim S As Double = 0
        If MA = NB Then
            ReDim AxB(NA, MB)

            Try
                For i = 0 To NA
                    For j = 0 To MB
                        S = 0
                        For k = 0 To MA
                            S = S + A(i, k) * B(k, j)
                        Next
                        AxB(i, j) = S
                    Next
                Next
                Return MNORM
            Catch ex As Exception
                Return MERR
            End Try
        Else
            Return MARGERR
        End If
    End Function

    ''' <summary>
    ''' 行列と行列の掛け算する。
    ''' </summary>
    ''' <param name="A">入力: 掛け算の左側の行列</param>
    ''' <param name="B">入力: 掛け算の右側の行列</param>
    ''' <param name="AxB">出力: AxB 行列</param>
    ''' <returns>MNORM:正常　　MERR: 異常　MARGERR: 引数エラー</returns>
    ''' <remarks></remarks>
    Public Function MMultAdd2(ByVal A(,) As Double, ByVal B(,) As Double, ByRef AxB(,) As Double) As Integer
        Dim NA As Integer = A.GetUpperBound(0)
        Dim MA As Integer = A.GetUpperBound(1)
        Dim NB As Integer = B.GetUpperBound(0)
        Dim MB As Integer = B.GetUpperBound(1)
        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim S As Double = 0
        If MA = NB Then
            'ReDim AxB(NA, MB)

            Try
                For i = 0 To NA
                    For j = 0 To MB
                        S = 0
                        For k = 0 To MA
                            S = S + A(i, k) * B(k, j)
                        Next
                        AxB(i, j) = AxB(i, j) + S
                    Next
                Next
                Return MNORM
            Catch ex As Exception
                Return MERR
            End Try
        Else
            Return MARGERR
        End If
    End Function

    ''' <summary>
    ''' 行列ＡとＢの掛け算をＡxＢに足し算、行列ＡとＣの掛け算をＡxＣに足し算
    ''' </summary>
    ''' <param name="A">入力: 掛け算の左側の行列</param>
    ''' <param name="B">入力: 掛け算の右側の行列</param>
    ''' <param name="AxB">出力: AxB 行列</param>
    ''' <returns>MNORM:正常　　MERR: 異常　MARGERR: 引数エラー</returns>
    ''' <remarks></remarks>
    Public Function MMultAdd2_Both(ByRef A(,) As Double, ByRef B(,) As Double, ByRef C(,) As Double, ByRef AxB(,) As Double, ByRef AxC(,) As Double) As Integer
        Dim NA As Integer = A.GetUpperBound(0)
        Dim MA As Integer = A.GetUpperBound(1)
        Dim NB As Integer = B.GetUpperBound(0)
        Dim MB As Integer = B.GetUpperBound(1)
        Dim NC As Integer = C.GetUpperBound(0)
        Dim MC As Integer = C.GetUpperBound(1)
        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim S_AB As Double = 0
        Dim S_AC As Double = 0
        If MA = NB And MA = NC Then
            'ReDim AxB(NA, MB)
            Try
                For i = 0 To NA
                    For j = 0 To MB
                        S_AB = 0
                        For k = 0 To MA
                            S_AB = S_AB + A(i, k) * B(k, j)
                        Next
                        AxB(i, j) = AxB(i, j) + S_AB
                    Next
                    For j = 0 To MC
                        S_AC = 0
                        For k = 0 To MA
                            S_AC = S_AC + A(i, k) * C(k, j)
                        Next
                        AxC(i, j) = AxC(i, j) + S_AC
                    Next
                Next
                Return MNORM
            Catch ex As Exception
                Return MERR
            End Try
        Else
            Return MARGERR
        End If
    End Function


    ''' <summary>
    ''' 行列ＡとＢの掛け算をＡxＢに足し算、行列ＡとＣの掛け算をＡxＣに足し算
    ''' </summary>
    ''' <param name="A">入力: 掛け算の左側の行列</param>
    ''' <param name="B">入力: 掛け算の右側の行列</param>
    ''' <param name="AxB">出力: AxB 行列</param>
    ''' <returns>MNORM:正常　　MERR: 異常　MARGERR: 引数エラー</returns>
    ''' <remarks></remarks>
    Public Function MCalcLeftRightMat(ByRef Fmat(,) As Double, ByRef D(,) As Double, ByRef C(,) As Double, ByRef AxB(,) As Double, ByRef AxC(,) As Double) As Integer
        'Dim NMult As Integer = Mult.GetUpperBound(0)
        'Dim MMult As Integer = Mult.GetUpperBound(1)
        Dim NFmat As Integer = Fmat.GetUpperBound(0)
        Dim MFmat As Integer = Fmat.GetUpperBound(1)
        Dim NC As Integer = C.GetUpperBound(0)
        Dim MC As Integer = C.GetUpperBound(1)
        Dim ND As Integer = D.GetUpperBound(0)
        Dim MD As Integer = D.GetUpperBound(1)
        Dim Mult1(,) As Double = {}
        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim S_AB As Double = 0
        Dim S_AC As Double = 0
        Dim S_Mult As Double = 0
        ' If MMult = NFmat And MMult = NC Then
        ReDim Mult1(MFmat, MD)
        Try
            For i = 0 To MFmat
                For j = 0 To MD
                    S_Mult = 0
                    For k = 0 To NFmat
                        S_Mult = S_Mult + Fmat(k, i) * D(k, j)
                    Next
                    Mult1(i, j) = S_Mult
                Next
                For j = 0 To MFmat
                    S_AB = 0
                    For k = 0 To MD
                        S_AB = S_AB + Mult1(i, k) * Fmat(k, j)
                    Next
                    AxB(i, j) = AxB(i, j) + S_AB
                Next
                For j = 0 To MC
                    S_AC = 0
                    For k = 0 To MD
                        S_AC = S_AC + Mult1(i, k) * C(k, j)
                    Next
                    AxC(i, j) = AxC(i, j) + S_AC
                Next
            Next
            Return MNORM
        Catch ex As Exception
            Return MERR
        End Try
        'Else
        'Return MARGERR
        'End If
    End Function
 

    Public Function MMult_ATBT(ByRef Fmat(,) As Double, ByRef D(,) As Double, ByRef Mult1(,) As Double) As Integer
        Dim NFmat As Integer = Fmat.GetUpperBound(0)
        Dim MFmat As Integer = Fmat.GetUpperBound(1)
        Dim ND As Integer = D.GetUpperBound(0)
        Dim MD As Integer = D.GetUpperBound(1)
        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim S_Mult As Double = 0
        If NFmat = ND Then
            ReDim Mult1(MFmat, MD)
            Try
                For i = 0 To MFmat
                    For j = 0 To MD
                        S_Mult = 0
                        For k = 0 To NFmat
                            S_Mult = S_Mult + Fmat(k, i) * D(k, j)
                        Next
                        Mult1(i, j) = S_Mult
                    Next
                Next
                Return MNORM
            Catch ex As Exception
                Return MERR
            End Try
        Else
            Return MARGERR
        End If
    End Function

    ''' <summary>
    ''' 行列ＡのＴｒａｎｓと行列Ｂの掛け算をする。
    ''' </summary>
    ''' <param name="A">入力: 掛け算の左側の行列</param>
    ''' <param name="B">入力: 掛け算の右側の行列</param>
    ''' <param name="AxB">出力: A_TxB 行列</param>
    ''' <returns>MNORM:正常　　MERR: 異常　MARGERR: 引数エラー</returns>
    ''' <remarks></remarks>
    Public Function MMult_ATB(ByRef A(,) As Double, ByRef B(,) As Double, ByRef AxB(,) As Double) As Integer
        Dim NA As Integer = A.GetUpperBound(0)
        Dim MA As Integer = A.GetUpperBound(1)
        Dim NB As Integer = B.GetUpperBound(0)
        Dim MB As Integer = B.GetUpperBound(1)
        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim S As Double = 0
        If NA = NB Then
            ReDim AxB(MA, MB)

            Try
                For i = 0 To MA
                    For j = 0 To MB
                        S = 0
                        For k = 0 To NA
                            S = S + A(k, i) * B(k, j)
                        Next
                        AxB(i, j) = S
                    Next
                Next
                Return MNORM
            Catch ex As Exception
                Return MERR
            End Try
        Else
            Return MARGERR
        End If
    End Function
    ''' <summary>
    ''' 行列の回転
    ''' </summary>
    ''' <param name="A">入力: 行列</param>
    ''' <param name="B">出力: A_T 行列</param>
    ''' <returns>MNORM:正常　　MERR: 異常　MARGERR: 引数エラー</returns>
    ''' <remarks></remarks>
    Public Function MTranspose(ByVal A(,) As Double, ByRef B(,) As Double) As Integer
        Dim N As Integer = A.GetUpperBound(0)
        Dim M As Integer = A.GetUpperBound(1)
        Dim i As Integer
        Dim j As Integer
        ReDim B(M, N)

        Try
            For i = 0 To M
                For j = 0 To N
                    B(i, j) = A(j, i)
                Next
            Next
            Return MNORM
        Catch ex As Exception
            Return MERR
        End Try

    End Function

    ''' <summary>
    ''' 行列の足し算
    ''' </summary>
    ''' <param name="A">入力: 行列</param>
    ''' <param name="B">入力: 行列</param>
    ''' <param name="A_add_B">出力: A+B 行列</param>
    ''' <returns>MNORM:正常　　MERR: 異常　MARGERR: 引数エラー</returns>
    ''' <remarks></remarks>
    Public Function MAdd(ByVal A(,) As Double, ByVal B(,) As Double, ByRef A_add_B(,) As Double) As Integer
        Dim NA As Integer = A.GetUpperBound(0)
        Dim MA As Integer = A.GetUpperBound(1)
        Dim NB As Integer = B.GetUpperBound(0)
        Dim MB As Integer = B.GetUpperBound(1)
        Dim i As Integer
        Dim j As Integer
        If NA = NB And MA = MB Then
            ReDim A_add_B(NA, MA)
            Try
                For i = 0 To NA
                    For j = 0 To MA
                        A_add_B(i, j) = A(i, j) + B(i, j)
                    Next
                Next
                Return MNORM
            Catch ex As Exception
                Return MERR
            End Try
        Else
            Return MARGERR
        End If
    End Function
    ''' <summary>
    ''' 行列の足し算(B=B+A)
    ''' </summary>
    ''' <param name="A">入力: 行列</param>
    ''' <param name="B">入出力: 行列</param>
    ''' <returns>MNORM:正常　　MERR: 異常　MARGERR: 引数エラー</returns>
    ''' <remarks></remarks>
    Public Function MAdd2(ByVal A(,) As Double, ByRef B(,) As Double) As Integer
        Dim NA As Integer = A.GetUpperBound(0)
        Dim MA As Integer = A.GetUpperBound(1)
        Dim NB As Integer = B.GetUpperBound(0)
        Dim MB As Integer = B.GetUpperBound(1)
        Dim i As Integer
        Dim j As Integer
        If NA = NB And MA = MB Then
            Try
                For i = 0 To NA
                    For j = 0 To MA
                        B(i, j) = B(i, j) + A(i, j)
                    Next
                Next
                Return MNORM
            Catch ex As Exception
                Return MERR
            End Try
        Else
            Return MARGERR
        End If
    End Function
    ''' <summary>
    ''' 行列の引き算
    ''' </summary>
    ''' <param name="A">入力: 行列</param>
    ''' <param name="B">入力: 行列</param>
    ''' <param name="A_sub_B">出力: 行列</param>
    ''' <returns>MNORM:正常　　MERR: 異常　MARGERR: 引数エラー</returns>
    ''' <remarks></remarks>
    Public Function MSub(ByVal A(,) As Double, ByVal B(,) As Double, ByRef A_sub_B(,) As Double) As Integer
        Dim NA As Integer = A.GetUpperBound(0)
        Dim MA As Integer = A.GetUpperBound(1)
        Dim NB As Integer = B.GetUpperBound(0)
        Dim MB As Integer = B.GetUpperBound(1)
        Dim i As Integer
        Dim j As Integer

        If NA = NB And MA = MB Then
            ReDim A_sub_B(NA, MA)
            Try
                For i = 0 To NA
                    For j = 0 To MA
                        A_sub_B(i, j) = A(i, j) - B(i, j)
                    Next
                Next
                Return MNORM
            Catch ex As Exception
                Return MERR
            End Try
        Else
            Return MARGERR
        End If
    End Function

    ''' <summary>
    ''' Minor行列の計算
    ''' </summary>
    ''' <param name="A">入力: 行列（NｘN）</param>
    ''' <param name="ii">入力: 行番号</param>
    ''' <param name="jj">入力: 列番号</param>
    ''' <param name="B">出力: Minor行列（ (N-1)x(N-1) )（入力行列の大きさより1つ小さい行列）</param>
    ''' <returns>MNORM:正常　　MERR: 異常　MARGERR: 引数エラー</returns>
    ''' <remarks></remarks>
    Private Function MMinor(ByVal A(,) As Double, ByVal ii As Integer, ByVal jj As Integer, ByRef B(,) As Double) As Integer
        Dim N As Integer = A.GetUpperBound(0)

        Dim i As Integer
        Dim j As Integer
        Dim bi As Integer = 0
        Dim bj As Integer = 0

        ReDim B(N - 1, N - 1)

        Try
            For i = 0 To N
                If i <> ii Then
                    bj = 0
                    For j = 0 To N
                        If j <> jj Then
                            B(bi, bj) = A(i, j)
                            bj = bj + 1
                        End If
                    Next
                    bi = bi + 1
                End If
            Next
            Return MNORM
        Catch ex As Exception
            Return MERR
        End Try

    End Function

    ''' <summary>
    ''' 行列式を計算する。
    ''' </summary>
    ''' <param name="A">入力: 行列（NxN)</param>
    ''' <param name="Det">出力: 行列式</param>
    ''' <returns>MNORM:正常　　MERR: 異常　MARGERR: 引数エラー</returns>
    ''' <remarks></remarks>
    Public Function MDeterm(ByVal A(,) As Double, ByRef Det As Double) As Integer
        Dim N As Integer = A.GetUpperBound(0)
        Dim M As Integer = A.GetUpperBound(1)
        Dim i As Integer = 0
        Dim j As Integer
        Dim S As Double = 0
        Dim MinorDet As Double = 0
        If N = M Then
            If N = 1 Then
                Det = A(0, 0) * A(1, 1) - A(0, 1) * A(1, 0)
                Return MNORM
            Else
                Dim Minor(,) As Double = {}

                Try
                    For j = 0 To N
                        MMinor(A, i, j, Minor)
                        MDeterm(Minor, MinorDet)
                        S = S + A(i, j) * Math.Pow(-1, i + j) * MinorDet
                    Next
                    Det = S
                    Return MNORM
                Catch ex As Exception
                    Return MERR
                End Try
            End If
        Else
            Return MARGERR
        End If
    End Function

    ''' <summary>
    ''' 逆行列の計算
    ''' </summary>
    ''' <param name="A">入力: 行列(NxN)</param>
    ''' <param name="B">出力: 逆行列(NxN)</param>
    ''' <returns>MNORM:正常　　MERR: 異常　MARGERR: 引数エラー</returns>
    ''' <remarks>行列式が０の場合は逆行列の計算不可</remarks>
    Public Function MInvert(ByVal A(,) As Double, ByRef B(,) As Double) As Integer
        Dim N As Integer = A.GetUpperBound(0)
        Dim M As Integer = A.GetUpperBound(1)
        Dim i As Integer
        Dim j As Integer
        Dim Adet As Double
        Dim MinorDet As Double = 0
        If N = M Then

            Try
                MDeterm(A, Adet)
                If Math.Abs(Adet) > Double.Epsilon Then
                    ReDim B(N, N)
                    Dim Minor(,) As Double = {}
                  
                    For i = 0 To M
                        For j = 0 To N
                            MMinor(A, i, j, Minor)
                            MDeterm(Minor, MinorDet)
                            B(j, i) = (Math.Pow(-1, i + j) * MinorDet) / Adet
                        Next
                    Next

                    Return MNORM
                Else
                    Return MARGERR
                End If
            Catch ex As Exception
                Return MERR
            End Try
        Else
            Return MARGERR
        End If
    End Function

    ''' <summary>
    ''' 逆行列を用いた連立一次方程式を解く
    ''' </summary>
    ''' <param name="A">入力: 行列(NxN)</param>
    ''' <param name="B">入力: 行列(Nx1)</param>
    ''' <param name="X">解:  行列(Nx1)</param>
    ''' <returns>MNORM:正常　　MERR: 異常　MARGERR: 引数エラー</returns>
    ''' <remarks>行列式が０の場合は逆行列の計算不可->解を解くことも不可</remarks>
    Public Function MSolve(ByVal A(,) As Double, ByRef B(,) As Double, ByRef X(,) As Double) As Integer
        Dim N As Integer = A.GetUpperBound(0)
        Dim M As Integer = A.GetUpperBound(1)
        Dim A_invert(,) As Double = {}
        Dim res As Integer = MNORM
        If N = M Then
            ReDim A_invert(N, N)
        Else
            Return MARGERR
        End If
        res = MInvert(A, A_invert)
        If res = MNORM Then
            Return MMult(A_invert, B, X)
        Else
            Return res
        End If
    End Function


    Public Function MSolve_GJ(ByVal A(,) As Double, ByRef B(,) As Double, ByRef X(,) As Double) As Integer
        Dim N As Integer = A.GetUpperBound(0)
        Dim M As Integer = A.GetUpperBound(1)
        Dim AT(,) As Double = {}
        ReDim AT(N + 1, M)
        Dim i As Integer
        Dim j As Integer

        For i = 0 To M
            For j = 0 To N
                AT(i, j) = A(j, i)
            Next
            AT(N + 1, 0) = B(i, 0)
        Next
      
        GaussJordan(AT, N + 1)
        ReDim X(N, 0)
        For i = 0 To M
            X(i, 0) = AT(N + 1, i)
        Next
        Return MNORM
    End Function

    Private Function GaussJordan(ByRef value(,) As Double, _
                            count As Long) As Boolean
        Dim a As Double          '注目式の未知数の係数
        Dim i As Long, j As Long '縦の係数操作のためのカウンタ
        Dim k As Long            '横の係数操作のためのカウンタ
        Dim temp As Double       '注目式以外の未知数の係数

        '1つ目の未知数から、順に処理していく(上から下へ)
        For i = 0& To count - 1&

            '注目式の未知数の係数を1にするためにその係数を格納
            a = value(i, i)

            '0除算、オーバーフロー除け
            If Math.Abs(a) < 0.0001 Then GaussJordan = False : Exit Function

            '注目式の未知数の係数を1にするため注目式全体をaで割る
            For k = i To count
                value(k, i) = value(k, i) / a
            Next k

            '注目式以外の未知数の係数を0にする
            For j = 0& To count - 1&
                '注目式以外なら実行する
                If j <> i Then
                    '注目式以外の未知数の係数を格納
                    temp = value(i, j)
                    'その値を0にするため、注目式との演算を行う
                    For k = i To count
                        value(k, j) = value(k, j) - _
                                      temp * value(k, i)
                    Next k
                End If
            Next j
        Next i
        GaussJordan = True
    End Function

    ''' <summary>
    ''' Minor行列の計算
    ''' </summary>
    ''' <param name="A">入力: 行列（NｘN）</param>
    ''' <param name="ii">入力: 除外する行番号の配列</param>
    ''' <param name="jj">入力: 除外する列番号の配列</param>
    ''' <param name="B">出力: Minor行列（ (N-1)x(N-1) )（入力行列の大きさより1つ小さい行列）</param>
    ''' <returns>MNORM:正常　　MERR: 異常　MARGERR: 引数エラー</returns>
    ''' <remarks></remarks>
    Public Function MMinors(ByVal A(,) As Double, ByVal ii() As Integer, ByVal jj() As Integer, ByRef B(,) As Double) As Integer
        Dim N As Integer = A.GetUpperBound(0)
        Dim iin As Integer = ii.GetUpperBound(0)
        Dim jjm As Integer = jj.GetUpperBound(0)
        Dim i As Integer
        Dim j As Integer
        Dim bi As Integer = 0
        Dim bj As Integer = 0
        Try
            ReDim B(N - iin - 1, N - jjm - 1)
        Catch ex As Exception
            Return MARGERR
        End Try

        Try
            For i = 0 To N
                If Isok(i, ii) Then
                    bj = 0
                    For j = 0 To N
                        If Isok(j, jj) Then
                            B(bi, bj) = A(i, j)
                            bj = bj + 1
                        End If
                    Next
                    bi = bi + 1
                End If
            Next
            Return MNORM
        Catch ex As Exception
            Return MERR
        End Try

    End Function

    Private Function Isok(ByVal i As Integer, ByVal ii() As Integer) As Boolean
        Isok = True
        For Each idi As Integer In ii
            If i = idi Then
                Isok = False
                Exit For
            End If
        Next
    End Function
End Module
