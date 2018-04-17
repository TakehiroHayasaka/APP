Imports HalconDotNet

Public Class Point3D

    ''' <summary>
    ''' X座標
    ''' </summary>
    Public X As HTuple
    ''' <summary>
    ''' Y座標
    ''' </summary>
    Public Y As HTuple
    ''' <summary>
    ''' Z座標
    ''' </summary>
    Public Z As HTuple
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
        X = Nothing
        Y = Nothing
        Z = Nothing
        MyDist = Nothing
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
    ''' 
    Public Sub GetDisttoOtherPose(ByVal objPoints As Point3D, ByRef Dist As Object)
#If Halcon = "True" Then
        Dim tmpAdd As Object
        Dim tmpSqrt As Object = Nothing
        Dim tmpSubX As Object
        Dim tmpSubY As Object
        Dim tmpSubZ As Object
        Dim tmpPowX As Object
        Dim tmpPowY As Object
        Dim tmpPowZ As Object
        tmpSubX = BTuple.TupleSub(X, objPoints.X)
        tmpSubY = BTuple.TupleSub(Y, objPoints.Y)
        tmpSubZ = BTuple.TupleSub(Z, objPoints.Z)
        tmpPowX = BTuple.TuplePow(tmpSubX, 2)
        tmpPowY = BTuple.TuplePow(tmpSubY, 2)
        tmpPowZ = BTuple.TuplePow(tmpSubZ, 2)
        tmpAdd = BTuple.TupleAdd(tmpPowX, tmpPowY)
        tmpAdd = BTuple.TupleAdd(tmpAdd, tmpPowZ)
        If TypeOf (Dist) Is Double Then
            Dist = BTuple.TupleSqrt(tmpAdd).D
        Else
            Dist = BTuple.TupleSqrt(tmpAdd)
        End If
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
        MultPoints.X = BTuple.TupleMult(objPoints.X, X)
        MultPoints.Y = BTuple.TupleMult(objPoints.Y, Y)
        MultPoints.Z = BTuple.TupleMult(objPoints.Z, Z)
    End Sub
    ''' <summary>
    ''' X+Y+Zの合計
    ''' </summary>
    Public Function GetSumXYZ() As Object
        GetSumXYZ = BTuple.TupleSum(BTuple.TupleAdd(BTuple.TupleAdd(X, Y), Z))
    End Function
    ''' <summary>
    ''' X,Y,Z座標値にスケール値を掛ける（FBMlibにのみ使用/未使用? ）
    ''' </summary>
    Public Sub SetScale(ByVal dblScale As Double)
        X = BTuple.TupleMult(X, dblScale)
        Y = BTuple.TupleMult(Y, dblScale)
        Z = BTuple.TupleMult(Z, dblScale)
        'RelPose = BTuple.TupleMult(RelPose, dblScale)
    End Sub
    'SUSANO ADD START 20150828
    Public Function SetScaleX(ByVal dblScale As Double) As Point3D
        SetScaleX = New Point3D

        SetScaleX.X = BTuple.TupleMult(X, dblScale)
        SetScaleX.Y = Y
        SetScaleX.Z = Z
    End Function
    Public Function SetScaleY(ByVal dblScale As Double) As Point3D
        SetScaleY = New Point3D

        SetScaleY.X = X
        SetScaleY.Y = BTuple.TupleMult(Y, dblScale)
        SetScaleY.Z = Z
    End Function

    Public Sub SetScaleXY(ByVal dblScaleX As Double, ByVal dblScaleY As Double)

        X = BTuple.TupleMult(X, dblScaleX)
        Y = BTuple.TupleMult(Y, dblScaleY)

    End Sub

    'SUSANO ADD END 20150828

    'SUSANO ADD START 20150929
    Public Sub SetScaleXY(ByVal Dx1 As Double,
                          ByVal Lx1 As Double,
                          ByVal Zx1 As Double,
                          ByVal Dx2 As Double,
                          ByVal Lx2 As Double,
                          ByVal Zx2 As Double,
                          ByVal Dy1 As Double,
                          ByVal Ly1 As Double,
                          ByVal Zy1 As Double,
                          ByVal Dy2 As Double,
                          ByVal Ly2 As Double,
                          ByVal Zy2 As Double)
        Dim x1 As Double
        Dim x2 As Double
        Dim dz1x As Double
        Dim dz2x As Double
        x1 = X * (1.0 + Dx1 / Lx1)
        x2 = X * (1.0 + Dx2 / Lx2)
        dz1x = Zx1 - Z
        dz2x = Z - Zx2
        X = (dz1x * x2 + dz2x * x1) / (dz1x + dz2x)

        Dim y1 As Double
        Dim y2 As Double
        Dim dz1y As Double
        Dim dz2y As Double

        y1 = Y * (1.0 + Dy1 / Ly1)
        y2 = Y * (1.0 + Dy2 / Ly2)
        dz1y = Zy1 - Z
        dz2y = Z - Zy2
        Y = (dz1y * y2 + dz2y * y1) / (dz1y + dz2y)

    End Sub
    'SUSANO ADD END 20150929
    ''' <summary>
    ''' X,Y,Z座標値にスケール値を掛ける
    ''' </summary>
    Public Function SetScale2(ByVal dblScale As Double) As Point3D
        SetScale2 = New Point3D
#If Halcon = "True" Then
        SetScale2.X = BTuple.TupleMult(X, dblScale)
        SetScale2.Y = BTuple.TupleMult(Y, dblScale)
        SetScale2.Z = BTuple.TupleMult(Z, dblScale)
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
        'RelPose = BTuple.TupleMult(RelPose, dblScale)
    End Function
    ''' <summary>
    ''' X,Y,Z座標値にある値を引く
    ''' </summary>
    Public Function SubPoint3d(ByVal P As Point3D) As Point3D
        SubPoint3d = New Point3D

        SubPoint3d.X = BTuple.TupleSub(X, P.X)
        SubPoint3d.Y = BTuple.TupleSub(Y, P.Y)
        SubPoint3d.Z = BTuple.TupleSub(Z, P.Z)

    End Function
    ''' <summary>
    ''' X,Y,Z座標値にある値を足す
    ''' </summary>
    Public Function AddPoint3d(ByVal P As Point3D) As Point3D
        AddPoint3d = New Point3D

        AddPoint3d.X = BTuple.TupleAdd(X, P.X)
        AddPoint3d.Y = BTuple.TupleAdd(Y, P.Y)
        AddPoint3d.Z = BTuple.TupleAdd(Z, P.Z)
    End Function
    ''' <summary>
    ''' FBMlibにのみ使用
    ''' </summary>
    Public Function ScalarPoint3d(ByVal P As Point3D) As Object
        ScalarPoint3d = BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleMult(X, P.X), BTuple.TupleMult(Y, P.Y)), BTuple.TupleMult(Z, P.Z))
    End Function

    ''' <summary>
    ''' 未使用
    ''' </summary>
    Public Function VectorMult(ByVal objMult As Point3D) As Point3D
        VectorMult = New Point3D
        'y * vec.z - z * vec.y, z * vec.x - x * vec.z, x * vec.y - y * vec.x
        VectorMult.X = BTuple.TupleSub(BTuple.TupleMult(Y, objMult.Z), BTuple.TupleMult(Z, objMult.Y))
        VectorMult.Y = BTuple.TupleSub(BTuple.TupleMult(Z, objMult.X), BTuple.TupleMult(X, objMult.Z))
        VectorMult.Z = BTuple.TupleSub(BTuple.TupleMult(X, objMult.Y), BTuple.TupleMult(Y, objMult.X))
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

        hv_N = BTuple.TupleLength(X)
        tuple_gen_sequence(0, hv_N - 1, 1, one_to_N)
        HOperatorSet.CreateMatrix(3, hv_N, BTuple.TupleConcat(BTuple.TupleConcat(X, Y), Z), hv_MatrixID)
        HOperatorSet.CreateMatrix(3, 3, ScaleMatrix, hv_ScaleMatrixID)
        HOperatorSet.TransposeMatrix(hv_MatrixID, hv_MatrixTransposedID)
        HOperatorSet.MultMatrix(hv_MatrixTransposedID, hv_ScaleMatrixID, "AB", hv_MatrixMultID)
        HOperatorSet.TransposeMatrix(hv_MatrixMultID, hv_MatrixTransposedID1)

        HOperatorSet.GetFullMatrix(hv_MatrixTransposedID1, hv_XYZ)
        X = BTuple.TupleFirstN(hv_XYZ, hv_N - 1)
        hv_XYZ = BTuple.TupleRemove(hv_XYZ, one_to_N)
        Y = BTuple.TupleFirstN(hv_XYZ, hv_N - 1)
        hv_XYZ = BTuple.TupleRemove(hv_XYZ, one_to_N)
        Z = BTuple.TupleFirstN(hv_XYZ, hv_N - 1)

        HOperatorSet.ClearMatrix(hv_MatrixID)
        HOperatorSet.ClearMatrix(hv_MatrixTransposedID)
        HOperatorSet.ClearMatrix(hv_MatrixTransposedID1)
        HOperatorSet.ClearMatrix(hv_MatrixMultID)
        HOperatorSet.ClearMatrix(hv_ScaleMatrixID)

    End Sub

    ''' <summary>
    ''' 3次元座標値を任意のパスに保存（FBMlibにのみ使用/未使用?）
    ''' </summary>
    Public Sub Save3dPoints(ByVal strPath As String)
        On Error Resume Next
        If BTuple.TupleLength(X) > 0 Then
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
        X = BTuple.TupleConcat(X, in3dPoints.X)
        Y = BTuple.TupleConcat(Y, in3dPoints.Y)
        Z = BTuple.TupleConcat(Z, in3dPoints.Z)
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
        Dim n As Integer = BTuple.TupleLength(X).I
        Dim tn As Integer = BTuple.TupleLength(objP3.X).I
        Dim i As Integer

        If n = tn Then
            For i = 0 To n - 1
                P1.X = BTuple.TupleSelect(X, i)
                P1.Y = BTuple.TupleSelect(Y, i)
                P1.Z = BTuple.TupleSelect(Z, i)
                P2.X = BTuple.TupleSelect(objP3.X, i)
                P2.Y = BTuple.TupleSelect(objP3.Y, i)
                P2.Z = BTuple.TupleSelect(objP3.Z, i)
                P1.GetDisttoOtherPose(Me, tmpDist1)
                P2.GetDisttoOtherPose(objP3, tmpDist2)
                Dist1 = BTuple.TupleConcat(Dist1, tmpDist1)
                Dist2 = BTuple.TupleConcat(Dist2, tmpDist2)

            Next
            'P1.X = X(0)
            'P1.Y = Y(0)
            'P1.Z = Z(0)
            'P2.X = objP3.X(0)
            'P2.Y = objP3.Y(0)
            'P2.Z = objP3.Z(0)
            'P1.GetDisttoOtherPose(Me, Dist1)
            'P2.GetDisttoOtherPose(objP3, Dist2)
            'BTuple.TupleSum(BTuple.TuplePow(Dist1, 2))
            'BTuple.TupleSum(BTuple.TupleMult(Dist1, Dist2))
            Scale = BTuple.TupleDiv(BTuple.TupleSum(BTuple.TupleMult(Dist1, Dist2)), BTuple.TupleSum(BTuple.TuplePow(Dist1, 2)))
        End If
    End Sub

    ''' <summary>
    ''' 平均値を算出
    ''' </summary>
    Public Function GetMean() As Point3D
        GetMean = New Point3D
        GetMean.X = BTuple.TupleMean(X)
        GetMean.Y = BTuple.TupleMean(Y)
        GetMean.Z = BTuple.TupleMean(Z)

    End Function
    ''' <summary>
    ''' 全要素の合計を算出
    ''' </summary>
    Public Function GetSummary() As Point3D
        GetSummary = New Point3D
        GetSummary.X = BTuple.TupleSum(X)
        GetSummary.Y = BTuple.TupleSum(Y)
        GetSummary.Z = BTuple.TupleSum(Z)
    End Function

    ''' <summary>
    ''' X,Y,Z座標値にある値を掛ける②
    ''' </summary>
    Public Function GetMultedByTuple(ByVal Val As Object) As Point3D
        GetMultedByTuple = New Point3D
        GetMultedByTuple.X = BTuple.TupleMult(X, Val)
        GetMultedByTuple.Y = BTuple.TupleMult(Y, Val)
        GetMultedByTuple.Z = BTuple.TupleMult(Z, Val)
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
        If X Is Nothing Or Y Is Nothing Or Z Is Nothing Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
