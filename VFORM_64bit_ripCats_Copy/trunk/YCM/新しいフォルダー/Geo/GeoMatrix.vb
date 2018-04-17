Option Strict Off
Option Explicit On
Public Class GeoMatrix

    '==============================================================================
    Private m_dblMatrix(4 * 4) As Double ' 4x4行列を表す1次元配列

    '==============================================================================
    '==============================================================================
    'UPGRADE_NOTE: Class_Initialize was upgraded to Class_Initialize_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Public Sub Class_Initialize_Renamed()
        SetUnitMatrix()
    End Sub
    Public Sub New()
        MyBase.New()
        Class_Initialize_Renamed()
    End Sub

    '==============================================================================
    'UPGRADE_NOTE: Class_Terminate was upgraded to Class_Terminate_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Public Sub Class_Terminate_Renamed()
        On Error Resume Next
        'UPGRADE_NOTE: Erase was upgraded to System.Array.Clear. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
        System.Array.Clear(m_dblMatrix, 0, m_dblMatrix.Length)
    End Sub
    Protected Overrides Sub Finalize()
        Class_Terminate_Renamed()
        MyBase.Finalize()
    End Sub

    '==============================================================================
    Public Sub Assign(ByRef matrix As GeoMatrix)
        Dim intRow, intCol As Short
        For intRow = 1 To 4
            For intCol = 1 To 4
                Call Me.SetAt(intRow, intCol, matrix.GetAt(intRow, intCol))
            Next
        Next
    End Sub
    Public Function Copy() As GeoMatrix
        Copy = New GeoMatrix
        Call Copy.Assign(Me)
    End Function

    '==============================================================================
    '==============================================================================
    ' 座標系への変換を設定する
    Public Sub SetCoordSystem(ByRef origin As GeoPoint, ByRef xAxis As GeoVector, ByRef yAxis As GeoVector, ByRef zAxis As GeoVector)
        SetUnitMatrix()

        If xAxis Is Nothing Then xAxis = yAxis.GetOuterProduct(zAxis)
        If yAxis Is Nothing Then yAxis = zAxis.GetOuterProduct(xAxis)
        If zAxis Is Nothing Then zAxis = xAxis.GetOuterProduct(yAxis)

        Dim matX As New GeoMatrix
        Dim matY As New GeoMatrix
        Dim matZ As New GeoMatrix
        Dim axis As New GeoVector
        Dim dblAngle As Double
        Dim baseYaxis, baseXaxis, baseZaxis As GeoVector
        baseXaxis = GeoVector_Xaxis()
        baseYaxis = GeoVector_Yaxis()
        baseZaxis = GeoVector_Zaxis()

        ' 1. 基準座標系の Z 軸 baseZaxis を、ローカル座標系の zAxis に合わせる
        ' 1-1. baseZaxis から zAxis への、基準座標系 X 軸回りの回転行列を得る
        ' 1-1-1. 平面に zAxis を投影し axis を得る
        axis = zAxis.GetProjected(GeoPlane_ByNormal(baseXaxis))
        ' 1-1-2. axis の大きさが 0 でなかったら
        If Math_IsEqual(axis.GetLength(), 0.0#, 0.00001) = False Then
            ' 1-1-3. Z 軸となす角度 dblAngle を得る
            dblAngle = axis.GetNormal().GetAngleTo(baseZaxis, baseXaxis)
            'Debug.Print "Xangle:" & dblAngle * 180 / Math_Pai
            ' 1-1-4. X 軸周りの dblAngle 分の回転行列 matX を得る
            matX.SetXaxisRotation((dblAngle))
            Call baseYaxis.RotateBy(-dblAngle, baseXaxis)
            Call baseZaxis.RotateBy(-dblAngle, baseXaxis)
        End If
        'matX.PrintClass
        ' 1-2. baseZaxis から zAxis への、基準座標系 Y 軸回りの回転行列を得る
        ' 1-2-1. 平面に zAxis を投影し axis を得る
        axis = zAxis.GetProjected(GeoPlane_ByNormal(baseYaxis))
        ' 1-2-2. axis の大きさが 0 でなかったら
        If Math_IsEqual(axis.GetLength(), 0.0#, 0.00001) = False Then
            ' 1-2-3. Z 軸となす角度 dblAngle を得る
            dblAngle = axis.GetNormal().GetAngleTo(baseZaxis, baseYaxis)
            'Debug.Print "Yangle:" & dblAngle * 180 / Math_Pai
            ' 1-2-4. Y 軸周りの dblAngle 分の回転行列 matY を得る
            matY.SetYaxisRotation((dblAngle))
            Call baseXaxis.RotateBy(-dblAngle, baseYaxis)
            Call baseZaxis.RotateBy(-dblAngle, baseYaxis)
        End If
        'matY.PrintClass

        ' 2. X 軸から xAxis への Z 軸周りの回転行列を得る
        ' 2-1. 平面に xAxis を投影し axis を得る
        axis = xAxis.GetProjected(GeoPlane_ByNormal(baseZaxis))
        ' 2-2. axis の大きさが 0 でなかったら
        If Math_IsEqual(axis.GetLength(), 0.0#, 0.00001) = False Then
            ' 2-3. X 軸となす角度 dblAngle を得る
            dblAngle = axis.GetNormal().GetAngleTo(baseXaxis, baseZaxis)
            'Debug.Print "Zangle:" & dblAngle * 180 / Math_Pai
            ' 2-4. Z 軸周りの dblAngle 分の回転行列 matZ を得る
            matZ.SetZaxisRotation((dblAngle))
            Call baseXaxis.RotateBy(-dblAngle, baseZaxis)
            Call baseYaxis.RotateBy(-dblAngle, baseZaxis)
        End If
        'matZ.PrintClass

        ' 3. *this を -origin だけ移動する
        Call SetAt(4, 1, -origin.x())
        Call SetAt(4, 2, -origin.y())
        Call SetAt(4, 3, -origin.z())

        ' 4. 3 つの回転行列を掛け合わせる
        'Dim mat As GeoMatrix
        'Set mat = Me.GetMultipled(matX)
        'Set mat = mat.GetMultipled(matY)
        'Set mat = mat.GetMultipled(matZ)
        'Call Me.Assign(mat)
        Call Me.Assign(Me.GetMultipled(matX).GetMultipled(matY).GetMultipled(matZ))

        Dim i As Short
        ' 1E-10 以下は 0 とする
        For i = 0 To 15
            If System.Math.Abs(m_dblMatrix(i)) < 0.0000000001 Then m_dblMatrix(i) = 0
        Next
    End Sub

    '==============================================================================
    ' 移動変換を設定する
    Public Sub SetMove(ByRef Move As GeoVector)
        SetUnitMatrix()
        Call SetAt(4, 1, Move.x())
        Call SetAt(4, 2, Move.y())
        Call SetAt(4, 3, Move.z())
    End Sub

    '==============================================================================
    ' 拡大/縮小変換を設定する
    Public Sub SetScale(ByRef dblScaleValue As Double)
        SetUnitMatrix()
        Call SetAt(1, 1, dblScaleValue)
        Call SetAt(2, 2, dblScaleValue)
        Call SetAt(3, 3, dblScaleValue)
    End Sub

    '==============================================================================
    ' X 軸方向の拡大/縮小変換を設定する
    Public Sub SetXScale(ByRef dblScaleValue As Double)
        SetUnitMatrix()
        Call SetAt(1, 1, dblScaleValue)
    End Sub

    '==============================================================================
    ' Y 軸方向の拡大/縮小変換を設定する
    Public Sub SetYScale(ByRef dblScaleValue As Double)
        SetUnitMatrix()
        Call SetAt(2, 2, dblScaleValue)
    End Sub

    '==============================================================================
    ' Z 軸方向の拡大/縮小変換を設定する
    Public Sub SetZScale(ByRef dblScaleValue As Double)
        SetUnitMatrix()
        Call SetAt(3, 3, dblScaleValue)
    End Sub

    '==============================================================================
    '==============================================================================
    Public Sub SetAt(ByRef intRow As Short, ByRef intCol As Short, ByRef value As Double)
        If intRow < 1 Or 4 < intRow Or intCol < 1 Or 4 < intCol Then Exit Sub
        m_dblMatrix((intRow - 1) * 4 + (intCol - 1)) = value
    End Sub
    Public Function GetAt(ByRef intRow As Short, ByRef intCol As Short) As Double
        If intRow < 1 Or 4 < intRow Or intCol < 1 Or 4 < intCol Then Exit Function
        GetAt = m_dblMatrix((intRow - 1) * 4 + (intCol - 1))
    End Function

    '==============================================================================
    '==============================================================================
    ' 単位行列を設定する
    Public Sub SetUnitMatrix()
        On Error Resume Next
        Dim i As Short
        For i = 0 To 15
            m_dblMatrix(i) = 0.0#
        Next
        m_dblMatrix(0) = 1.0#
        m_dblMatrix(5) = 1.0#
        m_dblMatrix(10) = 1.0#
        m_dblMatrix(15) = 1.0#
    End Sub
    ' X 軸回りの回転行列を設定する
    Public Sub SetXaxisRotation(ByRef dblAngle As Double)
        Call SetUnitMatrix()
        Call SetAt(2, 2, System.Math.Cos(dblAngle))
        Call SetAt(2, 3, System.Math.Sin(dblAngle))
        Call SetAt(3, 2, -System.Math.Sin(dblAngle))
        Call SetAt(3, 3, System.Math.Cos(dblAngle))
    End Sub
    ' Y 軸回りの回転行列を設定する
    Public Sub SetYaxisRotation(ByRef dblAngle As Double)
        Call SetUnitMatrix()
        Call SetAt(1, 1, System.Math.Cos(dblAngle))
        Call SetAt(1, 3, -System.Math.Sin(dblAngle))
        Call SetAt(3, 1, System.Math.Sin(dblAngle))
        Call SetAt(3, 3, System.Math.Cos(dblAngle))
    End Sub
    ' Z 軸回りの回転行列を設定する
    Public Sub SetZaxisRotation(ByRef dblAngle As Double)
        Call SetUnitMatrix()
        Call SetAt(1, 1, System.Math.Cos(dblAngle))
        Call SetAt(1, 2, System.Math.Sin(dblAngle))
        Call SetAt(2, 1, -System.Math.Sin(dblAngle))
        Call SetAt(2, 2, System.Math.Cos(dblAngle))
    End Sub

    '==============================================================================
    ' 行列の積を得る
    Public Function GetMultipled(ByRef matrix As GeoMatrix) As GeoMatrix
        GetMultipled = New GeoMatrix
        Dim intRow, intCol As Short
        For intRow = 1 To 4
            For intCol = 1 To 4
                Call GetMultipled.SetAt(intRow, intCol, GetRowByColumn(Me, intRow, matrix, intCol))
            Next
        Next
    End Function

    '==============================================================================
    ' 逆行列にする
    Public Sub Invert()
        ' 2倍の列を持つ行列を用意し、前半列には自分自身を、後半列には単位行列を、それぞれコピーする
        Dim dblMat(4 * 8) As Double

        ' 前半列にコピー(行/列を入れ替える)
        dblMat(0) = m_dblMatrix(0)
        dblMat(1) = m_dblMatrix(4)
        dblMat(2) = m_dblMatrix(8)
        dblMat(3) = m_dblMatrix(12)
        dblMat(8) = m_dblMatrix(1)
        dblMat(9) = m_dblMatrix(5)
        dblMat(10) = m_dblMatrix(9)
        dblMat(11) = m_dblMatrix(13)
        dblMat(16) = m_dblMatrix(2)
        dblMat(17) = m_dblMatrix(6)
        dblMat(18) = m_dblMatrix(10)
        dblMat(19) = m_dblMatrix(14)
        dblMat(24) = m_dblMatrix(3)
        dblMat(25) = m_dblMatrix(7)
        dblMat(26) = m_dblMatrix(11)
        dblMat(27) = m_dblMatrix(15)
        ' 後半列は単位行列に
        dblMat(4) = 1
        dblMat(13) = 1
        dblMat(22) = 1
        dblMat(31) = 1

        ' dblMat(0) == 0 のとき, 別の行と取り替え, 後で元に戻す
        Dim intExchangedIndex1 As Short
        intExchangedIndex1 = 0
        Dim i As Short
        Dim intMinCount As Short
        Dim intFlags1(3) As Short
        Dim intFlagCounts1(3) As Short
        If Math_IsEqual(dblMat(0), 0.0#, 0.00001) Then
            For i = 1 To 3
                If Math_IsEqual(dblMat(i * 8), 0.0#, 0.00001) = False Then
                    intFlags1(i - 1) = intFlags1(i - 1) + 1
                    intFlagCounts1(i - 1) = intFlagCounts1(i - 1) + 1
                End If
                If Math_IsEqual(dblMat(i * 8 + 1), 0.0#, 0.00001) = False Then
                    intFlags1(i - 1) = intFlags1(i - 1) + 2
                    intFlagCounts1(i - 1) = intFlagCounts1(i - 1) + 1
                End If
                If Math_IsEqual(dblMat(i * 8 + 2), 0.0#, 0.00001) = False Then
                    intFlags1(i - 1) = intFlags1(i - 1) + 4
                    intFlagCounts1(i - 1) = intFlagCounts1(i - 1) + 1
                End If
                If Math_IsEqual(dblMat(i * 8 + 3), 0.0#, 0.00001) = False Then
                    intFlags1(i - 1) = intFlags1(i - 1) + 8
                    intFlagCounts1(i - 1) = intFlagCounts1(i - 1) + 1
                End If
            Next
            ' first & 0x01 が true で、かつ最も second が小さい行を得る
            intMinCount = 5
            For i = 1 To 3
                If (intFlags1(i - 1) And 1) And intFlagCounts1(i - 1) < intMinCount Then
                    intMinCount = intFlagCounts1(i - 1)
                    intExchangedIndex1 = i
                End If
            Next
            If intExchangedIndex1 <> 0 Then
                Call SwapDoubleArray(dblMat, 0, intExchangedIndex1 * 8, 8)
            End If
        End If

        If dblMat(0) = 0.0# Then Exit Sub

        Call DivideInvertingMatrix(dblMat, 0, 0) ' dblMat(0)  を 1 にする: 1行目を 1 / dblMat(0) で割る
        Call SubtractInvertingMatrix(dblMat, 1, 0, 8) ' dblMat(8)  を 0 にする: 2行目に1行目の -dblMat(8)  倍を加える
        Call SubtractInvertingMatrix(dblMat, 2, 0, 16) ' dblMat(16) を 0 にする: 3行目に1行目の -dblMat(16) 倍を加える
        Call SubtractInvertingMatrix(dblMat, 3, 0, 24) ' dblMat(24) を 0 にする: 4行目に1行目の -dblMat(24) 倍を加える

        ' dblMat(9) == 0 のとき, 別の行と取り替え, 後で元に戻す
        Dim intExchangedIndex2 As Short
        intExchangedIndex2 = 0
        Dim intFlags2(2) As Short
        Dim intFlagCounts2(2) As Short
        If Math_IsEqual(dblMat(9), 0.0#, 0.00001) Then
            For i = 2 To 3
                If Math_IsEqual(dblMat(i * 8), 0.0#, 0.00001) = False Then
                    intFlags2(i - 2) = intFlags2(i - 2) + 1
                    intFlagCounts2(i - 2) = intFlagCounts2(i - 2) + 1
                End If
                If Math_IsEqual(dblMat(i * 8 + 1), 0.0#, 0.00001) = False Then
                    intFlags2(i - 2) = intFlags2(i - 2) + 2
                    intFlagCounts2(i - 2) = intFlagCounts2(i - 2) + 1
                End If
                If Math_IsEqual(dblMat(i * 8 + 2), 0.0#, 0.00001) = False Then
                    intFlags2(i - 2) = intFlags2(i - 2) + 4
                    intFlagCounts2(i - 2) = intFlagCounts2(i - 2) + 1
                End If
                If Math_IsEqual(dblMat(i * 8 + 3), 0.0#, 0.00001) = False Then
                    intFlags2(i - 2) = intFlags2(i - 2) + 8
                    intFlagCounts2(i - 2) = intFlagCounts2(i - 2) + 1
                End If
            Next
            ' first | 0x02 が true で、かつ最も second が小さい行を得る
            intMinCount = 5
            For i = 2 To 3
                If (intFlags2(i - 2) And 2) And intFlagCounts2(i - 2) < intMinCount Then
                    intMinCount = intFlagCounts2(i - 2)
                    intExchangedIndex2 = i
                End If
            Next
            If intExchangedIndex2 <> 0 Then
                Call SwapDoubleArray(dblMat, 8, intExchangedIndex2 * 8, 8)
            End If
        End If

        If dblMat(9) = 0.0# Then Exit Sub

        Call DivideInvertingMatrix(dblMat, 1, 9) ' dblMat(9)  を 1 にする: 2行目を 1 / dblMat(9) で割る
        Call SubtractInvertingMatrix(dblMat, 0, 1, 1) ' dblMat(1)  を 0 にする: 1行目に2行目の -dblMat(1)  倍を加える
        Call SubtractInvertingMatrix(dblMat, 2, 1, 17) ' dblMat(17) を 0 にする: 3行目に2行目の -dblMat(17) 倍を加える
        Call SubtractInvertingMatrix(dblMat, 3, 1, 25) ' dblMat(25) を 0 にする: 4行目に2行目の -dblMat(25) 倍を加える

        ' dblMat(18) == 0 のとき, 別の行と取り替え, 後で元に戻す
        Dim intExchangedIndex3 As Short
        intExchangedIndex3 = 0
        Dim intFlags3(1) As Short
        Dim intFlagCounts3(1) As Short
        If Math_IsEqual(dblMat(18), 0.0#, 0.00001) Then
            For i = 3 To 3
                If Math_IsEqual(dblMat(i * 8), 0.0#, 0.00001) = False Then
                    intFlags3(i - 3) = intFlags3(i - 3) + 1
                    intFlagCounts3(i - 3) = intFlagCounts3(i - 3) + 1
                End If
                If Math_IsEqual(dblMat(i * 8 + 1), 0.0#, 0.00001) = False Then
                    intFlags3(i - 3) = intFlags3(i - 3) + 2
                    intFlagCounts3(i - 3) = intFlagCounts3(i - 3) + 1
                End If
                If Math_IsEqual(dblMat(i * 8 + 2), 0.0#, 0.00001) = False Then
                    intFlags3(i - 3) = intFlags3(i - 3) + 4
                    intFlagCounts3(i - 3) = intFlagCounts3(i - 3) + 1
                End If
                If Math_IsEqual(dblMat(i * 8 + 3), 0.0#, 0.00001) = False Then
                    intFlags3(i - 3) = intFlags3(i - 3) + 8
                    intFlagCounts3(i - 3) = intFlagCounts3(i - 3) + 1
                End If
            Next
            ' first | 0x04 が true で、かつ最も second が小さい行を得る
            intMinCount = 5
            For i = 3 To 4
                If (intFlags3(i - 3) And 4) And intFlagCounts3(i - 3) < intMinCount Then
                    intMinCount = intFlagCounts3(i - 3)
                    intExchangedIndex3 = i
                End If
            Next
            If intExchangedIndex3 <> 0 Then
                Call SwapDoubleArray(dblMat, 16, intExchangedIndex3 * 8, 8)
            End If
        End If

        If dblMat(18) = 0.0# Then Exit Sub

        Call DivideInvertingMatrix(dblMat, 2, 18) ' dblMat(18) を 1 にする: 3行目を 1 / dblMat(18) で割る
        Call SubtractInvertingMatrix(dblMat, 0, 2, 2) ' dblMat(2)  を 0 にする: 1行目に3行目の -dblMat(2) 倍を加える
        Call SubtractInvertingMatrix(dblMat, 1, 2, 10) ' dblMat(10) を 0 にする: 2行目に3行目の -dblMat(10) 倍を加える
        Call SubtractInvertingMatrix(dblMat, 3, 2, 26) ' dblMat(26) を 0 にする: 4行目に3行目の -dblMat(26) 倍を加える

        If dblMat(27) = 0.0# Then Exit Sub

        Call DivideInvertingMatrix(dblMat, 3, 27) ' dblMat(27) を 1 にする: 4行目を 1 / dblMat(27) で割る
        Call SubtractInvertingMatrix(dblMat, 0, 3, 3) ' dblMat(3)  を 0 にする: 1行目に4行目の -dblMat(3)  倍を加える
        Call SubtractInvertingMatrix(dblMat, 1, 3, 11) ' dblMat(11) を 0 にする: 2行目に4行目の -dblMat(11) 倍を加える
        Call SubtractInvertingMatrix(dblMat, 2, 3, 19) ' dblMat(19) を 0 にする: 3行目に4行目の -dblMat(19) 倍を加える

        ' 前半列をコピー(入れ替えた行/列を戻す)
        m_dblMatrix(0) = dblMat(4)
        m_dblMatrix(1) = dblMat(12)
        m_dblMatrix(2) = dblMat(20)
        m_dblMatrix(3) = dblMat(28)
        m_dblMatrix(4) = dblMat(5)
        m_dblMatrix(5) = dblMat(13)
        m_dblMatrix(6) = dblMat(21)
        m_dblMatrix(7) = dblMat(29)
        m_dblMatrix(8) = dblMat(6)
        m_dblMatrix(9) = dblMat(14)
        m_dblMatrix(10) = dblMat(22)
        m_dblMatrix(11) = dblMat(30)
        m_dblMatrix(12) = dblMat(7)
        m_dblMatrix(13) = dblMat(15)
        m_dblMatrix(14) = dblMat(23)
        m_dblMatrix(15) = dblMat(31)

        ' 1E-10 以下は 0 とする
        For i = 0 To 15
            If System.Math.Abs(m_dblMatrix(i)) < 0.0000000001 Then m_dblMatrix(i) = 0
        Next
    End Sub

    '==============================================================================
    ' 逆行列を得る
    Public Function GetInverse() As GeoMatrix
        GetInverse = Me.Copy
        Call GetInverse.Invert()
    End Function

    '==============================================================================
    '==============================================================================
    ' クラス内容を表示する
    Public Sub PrintClass()
        Debug.Print("GeoMatrix:")
        Debug.Print("  (" & m_dblMatrix(0) & "," & m_dblMatrix(1) & "," & m_dblMatrix(2) & "," & m_dblMatrix(3) & ")")
        Debug.Print("  (" & m_dblMatrix(4) & "," & m_dblMatrix(5) & "," & m_dblMatrix(6) & "," & m_dblMatrix(7) & ")")
        Debug.Print("  (" & m_dblMatrix(8) & "," & m_dblMatrix(9) & "," & m_dblMatrix(10) & "," & m_dblMatrix(11) & ")")
        Debug.Print("  (" & m_dblMatrix(12) & "," & m_dblMatrix(13) & "," & m_dblMatrix(14) & "," & m_dblMatrix(15) & ")")
    End Sub

    '==============================================================================
    '==============================================================================
    Private Function GetRowByColumn(ByRef matrix1 As GeoMatrix, ByRef intRow As Short, ByRef matrix2 As GeoMatrix, ByRef intCol As Short) As Double
        GetRowByColumn = 0
        GetRowByColumn = GetRowByColumn + matrix1.GetAt(intRow, 1) * matrix2.GetAt(1, intCol)
        GetRowByColumn = GetRowByColumn + matrix1.GetAt(intRow, 2) * matrix2.GetAt(2, intCol)
        GetRowByColumn = GetRowByColumn + matrix1.GetAt(intRow, 3) * matrix2.GetAt(3, intCol)
        GetRowByColumn = GetRowByColumn + matrix1.GetAt(intRow, 4) * matrix2.GetAt(4, intCol)
    End Function

    '==============================================================================
    '==============================================================================
    Private Sub SwapDoubleArray(ByRef dblAry() As Double, ByRef intRow1 As Short, ByRef intRow2 As Short, ByRef intSize As Short)
        Dim dblValue As Double
        Dim i As Short
        For i = 1 To intSize
            dblValue = dblAry(intRow1 + i - 1)
            dblAry(intRow1 + i - 1) = dblAry(intRow2 + i - 1)
            dblAry(intRow2 + i - 1) = dblValue
        Next
    End Sub

    '==============================================================================
    Private Sub DivideInvertingMatrix(ByRef invertMat() As Double, ByRef intRowIndex As Short, ByRef intMatrixIndex As Short)
        If invertMat(intMatrixIndex) = 0.0# Then Exit Sub

        Dim dblValue As Double
        dblValue = invertMat(intMatrixIndex)
        Dim i As Short
        For i = 0 To 7
            invertMat(intRowIndex * 8 + i) = invertMat(intRowIndex * 8 + i) / dblValue
        Next
    End Sub

    '==============================================================================
    Private Sub SubtractInvertingMatrix(ByRef invertMat() As Double, ByRef intRowIndex1 As Short, ByRef intRowIndex2 As Short, ByRef intMatrixIndex As Short)
        Dim dblValue As Double
        dblValue = invertMat(intMatrixIndex)
        Dim i As Short
        For i = 0 To 7
            invertMat(intRowIndex1 * 8 + i) = invertMat(intRowIndex1 * 8 + i) - invertMat(intRowIndex2 * 8 + i) * dblValue
        Next
    End Sub
End Class