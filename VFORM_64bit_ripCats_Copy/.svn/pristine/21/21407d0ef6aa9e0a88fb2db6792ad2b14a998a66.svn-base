Public Class Point3D

    ''' <summary>
    ''' X座標
    ''' </summary>
    Public X As Object
    ''' <summary>
    ''' Y座標
    ''' </summary>
    Public Y As Object
    ''' <summary>
    ''' Z座標
    ''' </summary>
    Public Z As Object
    ''' <summary>
    ''' 各レイから自分の3次元座標までの距離の和
    ''' </summary>
    Public MyDist As Object
    ''' <summary>
    ''' FBMlib.Point3Dの初期化①（X,Y,Z,MyDist）
    ''' </summary>
    Public Sub New()
        'X = New Object
        'Y = New Object
        'Z = New Object
        X = System.DBNull.Value
        Y = System.DBNull.Value
        Z = System.DBNull.Value
        MyDist = System.DBNull.Value
    End Sub
    ''' <summary>
    ''' FBMlib.Point3Dの初期化②（X,Y,Z,MyDist）
    ''' </summary>
    Public Sub New(ByVal XYZ As Point3D)
        X = XYZ.X
        Y = XYZ.Y
        Z = XYZ.Z
        MyDist = XYZ.MyDist
    End Sub
    ''' <summary>
    ''' BestPlaneNormalVectorの初期化（X,Y,Z）
    ''' </summary>
    Public Sub New(ByVal tX As Object, ByVal tY As Object, ByVal tZ As Object)
        X = tX
        Y = tY
        Z = tZ
    End Sub
    ''' <summary>
    ''' 2点間の距離を算出
    ''' </summary>
    Public Sub GetDisttoOtherPose(ByVal objPoints As Point3D, ByRef Dist As Object)
#If Halcon = 1 Then
        Dim tmpAdd As Object
        Dim tmpSqrt As Object = Nothing
        Dim tmpSubX As Object
        Dim tmpSubY As Object
        Dim tmpSubZ As Object
        Dim tmpPowX As Object
        Dim tmpPowY As Object
        Dim tmpPowZ As Object
        tmpSubX = Tuple.TupleSub(X, objPoints.X)
        tmpSubY = Tuple.TupleSub(Y, objPoints.Y)
        tmpSubZ = Tuple.TupleSub(Z, objPoints.Z)
        tmpPowX = Tuple.TuplePow(tmpSubX, 2)
        tmpPowY = Tuple.TuplePow(tmpSubY, 2)
        tmpPowZ = Tuple.TuplePow(tmpSubZ, 2)
        tmpAdd = Tuple.TupleAdd(tmpPowX, tmpPowY)
        tmpAdd = Tuple.TupleAdd(tmpAdd, tmpPowZ)
        Dist = Tuple.TupleSqrt(tmpAdd)
#Else
        Try
            If IsNumeric(objPoints.X) And IsNumeric(objPoints.Y) And IsNumeric(objPoints.Z) Then
                If IsNumeric(X) And IsNumeric(Y) And IsNumeric(Z) Then
                    Dim tmpAdd As Object
                    Dim tmpSqrt As Object = Nothing
                    Dim tmpSubX As Object
                    Dim tmpSubY As Object
                    Dim tmpSubZ As Object
                    Dim tmpPowX As Object
                    Dim tmpPowY As Object
                    Dim tmpPowZ As Object
                    tmpSubX = X - objPoints.X
                    tmpSubY = Y - objPoints.Y
                    tmpSubZ = Z - objPoints.Z
                    tmpPowX = tmpSubX * tmpSubX
                    tmpPowY = tmpSubY * tmpSubY
                    tmpPowZ = tmpSubZ * tmpSubZ
                    tmpAdd = tmpPowX + tmpPowY + tmpPowZ
                    Dist = Math.Sqrt(tmpAdd)
                End If
            End If
        Catch ex As Exception

        End Try

#End If
    End Sub
    ''' <summary>
    ''' X,Y,Z座標値にある値を掛ける①
    ''' </summary>
    Public Sub GetMult3D(ByVal objPoints As Point3D, ByRef MultPoints As Point3D)
        MultPoints.X = Tuple.TupleMult(objPoints.X, X)
        MultPoints.Y = Tuple.TupleMult(objPoints.Y, Y)
        MultPoints.Z = Tuple.TupleMult(objPoints.Z, Z)
    End Sub
    ''' <summary>
    ''' X+Y+Zの合計
    ''' </summary>
    Public Function GetSumXYZ() As Object
        GetSumXYZ = Tuple.TupleSum(Tuple.TupleAdd(Tuple.TupleAdd(X, Y), Z))
    End Function
    ''' <summary>
    ''' X,Y,Z座標値にスケール値を掛ける（FBMlibにのみ使用/未使用? ）
    ''' </summary>
    Public Sub SetScale(ByVal dblScale As Double)
        X = Tuple.TupleMult(X, dblScale)
        Y = Tuple.TupleMult(Y, dblScale)
        Z = Tuple.TupleMult(Z, dblScale)
        'RelPose = Tuple.TupleMult(RelPose, dblScale)
    End Sub
    ''' <summary>
    ''' X,Y,Z座標値にスケール値を掛ける
    ''' </summary>
    Public Function SetScale2(ByVal dblScale As Double) As Point3D
        SetScale2 = New Point3D
#If Halcon = 11 Then
        SetScale2.X = Tuple.TupleMult(X, dblScale)
        SetScale2.Y = Tuple.TupleMult(Y, dblScale)
        SetScale2.Z = Tuple.TupleMult(Z, dblScale)
#Else
        Try
            If IsNumeric(X) And IsNumeric(Y) And IsNumeric(Z) Then
                SetScale2.X = X * dblScale
                SetScale2.Y = Y * dblScale
                SetScale2.Z = Z * dblScale
            End If
        Catch ex As Exception

        End Try
#End If
        'RelPose = Tuple.TupleMult(RelPose, dblScale)
    End Function
    ''' <summary>
    ''' X,Y,Z座標値にある値を引く
    ''' </summary>
    Public Function SubPoint3d(ByVal P As Point3D) As Point3D
        SubPoint3d = New Point3D

        SubPoint3d.X = Tuple.TupleSub(X, P.X)
        SubPoint3d.Y = Tuple.TupleSub(Y, P.Y)
        SubPoint3d.Z = Tuple.TupleSub(Z, P.Z)

    End Function
    ''' <summary>
    ''' X,Y,Z座標値にある値を足す
    ''' </summary>
    Public Function AddPoint3d(ByVal P As Point3D) As Point3D
        AddPoint3d = New Point3D

        AddPoint3d.X = Tuple.TupleAdd(X, P.X)
        AddPoint3d.Y = Tuple.TupleAdd(Y, P.Y)
        AddPoint3d.Z = Tuple.TupleAdd(Z, P.Z)
    End Function
    ''' <summary>
    ''' FBMlibにのみ使用
    ''' </summary>
    Public Function ScalarPoint3d(ByVal P As Point3D) As Object
        ScalarPoint3d = Tuple.TupleAdd(Tuple.TupleAdd(Tuple.TupleMult(X, P.X), Tuple.TupleMult(Y, P.Y)), Tuple.TupleMult(Z, P.Z))
    End Function

    ''' <summary>
    ''' 未使用
    ''' </summary>
    Public Function VectorMult(ByVal objMult As Point3D) As Point3D
        VectorMult = New Point3D
        'y * vec.z - z * vec.y, z * vec.x - x * vec.z, x * vec.y - y * vec.x
        VectorMult.X = Tuple.TupleSub(Tuple.TupleMult(Y, objMult.Z), Tuple.TupleMult(Z, objMult.Y))
        VectorMult.Y = Tuple.TupleSub(Tuple.TupleMult(Z, objMult.X), Tuple.TupleMult(X, objMult.Z))
        VectorMult.Z = Tuple.TupleSub(Tuple.TupleMult(X, objMult.Y), Tuple.TupleMult(Y, objMult.X))
    End Function

    ''' <summary>
    ''' FeatureImage.vbに使用（FBMlibにのみ ）
    ''' </summary>
    Public Sub SetScaleByMatrix(ByVal ScaleMatrix As Object)
        ' Local control variables 
        Dim hv_N As Object = Nothing, hv_MatrixID As Object = Nothing
        Dim hv_MatrixTransposedID As Object = Nothing
        Dim hv_MatrixTransposedID1 As Object = Nothing
        Dim hv_ScaleMatrixID As Object = Nothing
        Dim hv_MatrixMultID As Object = Nothing
        Dim hv_XYZ As Object = Nothing
        Dim one_to_N As Object = Nothing


        ' Initialize local and output iconic variables 

        hv_N = Tuple.TupleLength(X)
        tuple_gen_sequence(0, hv_N - 1, 1, one_to_N)
        Op.CreateMatrix(3, hv_N, Tuple.TupleConcat(Tuple.TupleConcat(X, Y), Z), hv_MatrixID)
        Op.CreateMatrix(3, 3, ScaleMatrix, hv_ScaleMatrixID)
        Op.TransposeMatrix(hv_MatrixID, hv_MatrixTransposedID)
        Op.MultMatrix(hv_MatrixTransposedID, hv_ScaleMatrixID, "AB", hv_MatrixMultID)
        Op.TransposeMatrix(hv_MatrixMultID, hv_MatrixTransposedID1)

        Op.GetFullMatrix(hv_MatrixTransposedID1, hv_XYZ)
        X = Tuple.TupleFirstN(hv_XYZ, hv_N - 1)
        hv_XYZ = Tuple.TupleRemove(hv_XYZ, one_to_N)
        Y = Tuple.TupleFirstN(hv_XYZ, hv_N - 1)
        hv_XYZ = Tuple.TupleRemove(hv_XYZ, one_to_N)
        Z = Tuple.TupleFirstN(hv_XYZ, hv_N - 1)

        Op.ClearMatrix(hv_MatrixID)
        Op.ClearMatrix(hv_MatrixTransposedID)
        Op.ClearMatrix(hv_MatrixTransposedID1)
        Op.ClearMatrix(hv_MatrixMultID)
        Op.ClearMatrix(hv_ScaleMatrixID)

    End Sub

    ''' <summary>
    ''' 3次元座標値を任意のパスに保存（FBMlibにのみ使用/未使用?）
    ''' </summary>
    Public Sub Save3dPoints(ByVal strPath As String)
        On Error Resume Next
        If Tuple.TupleLength(X) > 0 Then
            SaveTupleObj(X, strPath & "_X.tpl")
            SaveTupleObj(Y, strPath & "_Y.tpl")
            SaveTupleObj(Z, strPath & "_Z.tpl")
        End If

    End Sub
    ''' <summary>
    ''' 任意のパスを返す(FBMlibにのみ使用/未使用?)
    ''' </summary>
    Public Sub Read3dPoints(ByVal strPath As String)
        Try
            ReadTupleObj(X, strPath & "_X.tpl")
            ReadTupleObj(Y, strPath & "_Y.tpl")
            ReadTupleObj(Z, strPath & "_Z.tpl")
        Catch ex As Exception

        End Try
    End Sub

    ''' <summary>
    ''' 2つの座標値を連結
    ''' </summary>
    Public Sub ConcatToMe(ByVal in3dPoints As Point3D)
        X = Tuple.TupleConcat(X, in3dPoints.X)
        Y = Tuple.TupleConcat(Y, in3dPoints.Y)
        Z = Tuple.TupleConcat(Z, in3dPoints.Z)
    End Sub

    ''' <summary>
    ''' スケールを計算
    ''' </summary>
    Public Sub CalcScale(ByVal objP3 As Point3D, ByRef Scale As Object)
        Dim P1 As New Point3D
        Dim P2 As New Point3D
        Dim Dist1 As Object = Nothing
        Dim Dist2 As Object = Nothing
        Dim tmpDist1 As Object = Nothing
        Dim tmpDist2 As Object = Nothing
        Dim n As Integer = Tuple.TupleLength(X)
        Dim tn As Integer = Tuple.TupleLength(objP3.X)
        Dim i As Integer

        If n = tn Then
            For i = 0 To n - 1
                P1.X = X(i)
                P1.Y = Y(i)
                P1.Z = Z(i)
                P2.X = objP3.X(i)
                P2.Y = objP3.Y(i)
                P2.Z = objP3.Z(i)
                P1.GetDisttoOtherPose(Me, tmpDist1)
                P2.GetDisttoOtherPose(objP3, tmpDist2)
                Dist1 = Tuple.TupleConcat(Dist1, tmpDist1)
                Dist2 = Tuple.TupleConcat(Dist2, tmpDist2)

            Next
            'P1.X = X(0)
            'P1.Y = Y(0)
            'P1.Z = Z(0)
            'P2.X = objP3.X(0)
            'P2.Y = objP3.Y(0)
            'P2.Z = objP3.Z(0)
            'P1.GetDisttoOtherPose(Me, Dist1)
            'P2.GetDisttoOtherPose(objP3, Dist2)
            'Tuple.TupleSum(Tuple.TuplePow(Dist1, 2))
            'Tuple.TupleSum(Tuple.TupleMult(Dist1, Dist2))
            Scale = Tuple.TupleDiv(Tuple.TupleSum(Tuple.TupleMult(Dist1, Dist2)), Tuple.TupleSum(Tuple.TuplePow(Dist1, 2)))
        End If
    End Sub

    ''' <summary>
    ''' 平均値を算出
    ''' </summary>
    Public Function GetMean() As Point3D
        GetMean = New Point3D
        GetMean.X = Tuple.TupleMean(X)
        GetMean.Y = Tuple.TupleMean(Y)
        GetMean.Z = Tuple.TupleMean(Z)

    End Function
    ''' <summary>
    ''' 全要素の合計を算出
    ''' </summary>
    Public Function GetSummary() As Point3D
        GetSummary = New Point3D
        GetSummary.X = Tuple.TupleSum(X)
        GetSummary.Y = Tuple.TupleSum(Y)
        GetSummary.Z = Tuple.TupleSum(Z)
    End Function

    ''' <summary>
    ''' X,Y,Z座標値にある値を掛ける②
    ''' </summary>
    Public Function GetMultedByTuple(ByVal Val As Object) As Point3D
        GetMultedByTuple = New Point3D
        GetMultedByTuple.X = Tuple.TupleMult(X, Val)
        GetMultedByTuple.Y = Tuple.TupleMult(Y, Val)
        GetMultedByTuple.Z = Tuple.TupleMult(Z, Val)
    End Function

    Public Function GetMidTen(ByVal P1 As Point3D, ByVal P2 As Point3D) As Point3D
        GetMidTen = New Point3D

        GetMidTen.X = (P1.X + P2.X) / 2
        GetMidTen.Y = (P1.Y + P2.Y) / 2
        GetMidTen.Z = (P1.Z + P2.Z) / 2
        X = (P1.X + P2.X) / 2
        Y = (P1.Y + P2.Y) / 2
        Z = (P1.Z + P2.Z) / 2
    End Function

    Public Sub CopyToMe(ByVal objCopy As Point3D)
        Try
            X = objCopy.X
            Y = objCopy.Y
            Z = objCopy.Z
        Catch ex As Exception

        End Try
       
    End Sub

    Public Function IsDBNULL() As Boolean
        If X Is DBNull.Value Or Y Is DBNull.Value Or Z Is DBNull.Value Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
