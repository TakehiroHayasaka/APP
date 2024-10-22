﻿Imports HalconDotNet

Public Class CameraPose

    ''' <summary>
    ''' カメラX座標
    ''' </summary>
    Public X As Object
    ''' <summary>
    ''' カメラY座標
    ''' </summary>
    Public Y As Object
    ''' <summary>
    ''' カメラY座標
    ''' </summary>
    Public Z As Object
    ''' <summary>
    ''' FeatureImage.vb，ImagePairSet.vb，CameraPose.vbにのみ使用（FBMライブラリ）
    ''' </summary>
    Public CovXYZ As Object
    ''' <summary>
    ''' FeatureImage.vb，ImagePairSet.vb，CameraPose.vbにのみ使用（FBMライブラリ）
    ''' </summary>
    Public Pose As HTuple
    ''' <summary>
    ''' FeatureImage.vb，ImagePairSet.vb，CameraPose.vbにのみ使用（FBMライブラリ）
    ''' </summary>
    Public RelPose As HTuple
    ''' <summary>
    ''' FeatureImage.vb，ImagePairSet.vb，CameraPose.vbにのみ使用（FBMライブラリ）
    ''' </summary>
    Public CovRelPose As HTuple
    ''' <summary>
    ''' FeatureImage.vb，ImagePairSet.vb，CameraPose.vbにのみ使用（FBMライブラリ）
    ''' </summary>
    Public hError As HTuple
    ''' <summary>
    ''' CameraPose.vbにのみ使用（FBMlibライブラリ）
    ''' </summary>
    Public badIndex As Object
    ''' <summary>
    ''' Reconstruct用のカメラ姿勢
    ''' </summary>
    ''' <remarks></remarks>
    Public ReConstPose As HTuple
    ''' <summary>
    ''' Reconstuct用のカメラ姿勢（世界座標系ミリ単位）
    ''' </summary>
    ''' <remarks></remarks>
    Public ReConstWorldPose As HTuple

    ''' 
    ''' <summary>
    ''' X，Y，Z，CovXYZ，Pose，RelPose，hError，badIndexの初期化
    ''' </summary>
    Public Sub New()
        X = Nothing
        Y = Nothing
        Z = Nothing
        CovXYZ = Nothing
        Pose = Nothing
        RelPose = Nothing
        CovRelPose = Nothing
        hError = Nothing
        badIndex = Nothing
    End Sub
    ''' <summary>
    ''' 未使用
    ''' </summary>
    Public ReadOnly Property CountXYZ() As Object
        Get
            Return BTuple.TupleLength(X)
        End Get
    End Property

    ''' <summary>
    ''' 未使用
    ''' </summary>
    Public Sub Get2PointDist(ByVal hv_I1 As Integer, ByVal hv_I2 As Integer, ByRef Dist As Double)
        Dist = CDbl(BTuple.TupleSqrt(BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TuplePow(BTuple.TupleSub( _
         BTuple.TupleSelect(X, hv_I1), BTuple.TupleSelect(X, hv_I2)), 2), BTuple.TuplePow( _
         BTuple.TupleSub(BTuple.TupleSelect(Y, hv_I1), BTuple.TupleSelect(Y, _
         hv_I2)), 2)), BTuple.TuplePow(BTuple.TupleSub(BTuple.TupleSelect(Z, hv_I1), _
         BTuple.TupleSelect(Z, hv_I2)), 2))))

    End Sub
    ''' <summary>
    ''' FeatureImage.vbのみ使用（FBMライブラリ）
    ''' </summary>
    Public Sub Get1PointToAllPoints(ByVal I1 As Integer, ByRef Dist As Object)

        Dim tmpXYZ As New Point3D
        Dim tmpXYZ1 As New Point3D

        tmpXYZ.X = BTuple.TupleRemove(X, I1)
        tmpXYZ.Y = BTuple.TupleRemove(Y, I1)
        tmpXYZ.Z = BTuple.TupleRemove(Z, I1)
        tmpXYZ1.X = BTuple.TupleGenConst(BTuple.TupleLength(tmpXYZ.X), BTuple.TupleSelect(X, I1))
        tmpXYZ1.Y = BTuple.TupleGenConst(BTuple.TupleLength(tmpXYZ.X), BTuple.TupleSelect(Y, I1))
        tmpXYZ1.Z = BTuple.TupleGenConst(BTuple.TupleLength(tmpXYZ.X), BTuple.TupleSelect(Z, I1))

        tmpXYZ.GetDisttoOtherPose(tmpXYZ1, Dist)

    End Sub


    ''' <summary>
    ''' FeatureImage.vb，CameraPose.vbにのみ使用（FBMライブラリ）
    ''' </summary>
    Public Function ConvertToPoint3d() As Point3D
        ConvertToPoint3d = New Point3D
        ConvertToPoint3d.X = X
        ConvertToPoint3d.Y = Y
        ConvertToPoint3d.Z = Z

    End Function
    ''' <summary>
    ''' 未使用?
    ''' </summary>
    Public Sub SetScale(ByVal dblScale As Double)
        X = BTuple.TupleMult(X, dblScale)
        Y = BTuple.TupleMult(Y, dblScale)
        Z = BTuple.TupleMult(Z, dblScale)
        'RelPose = BTuple.TupleMult(RelPose, dblScale)
    End Sub

    ''' <summary>
    ''' X,Y,Z座標の差分を計算（未使用）
    ''' </summary>
    Public Sub CalcSub(ByVal objSub As CameraPose, ByRef objRes As CameraPose)
        objRes.X = BTuple.TupleSub(X, objSub.X)
        objRes.Y = BTuple.TupleSub(Y, objSub.Y)
        objRes.Z = BTuple.TupleSub(Z, objSub.Z)
    End Sub

    ''' <summary>
    ''' X,Y,Z座標の絶対値を計算（未使用）
    ''' </summary>
    Public Sub CalcAbs(ByRef objRes As CameraPose)
        objRes.X = BTuple.TupleAbs(X)
        objRes.Y = BTuple.TupleAbs(Y)
        objRes.Z = BTuple.TupleAbs(Z)
    End Sub

    ''' <summary>
    ''' 未使用
    ''' </summary>
    Public Sub SearchBadPoint(ByRef TestMidPose As CameraPose, ByVal epi As Double)
        Dim Dist As New Object
        Dim MyXYZ As New Point3D
        Dim testXYZ As New Point3D
        MyXYZ.X = X
        MyXYZ.Y = Y
        MyXYZ.Z = Z
        testXYZ.X = TestMidPose.X
        testXYZ.Y = TestMidPose.Y
        testXYZ.Z = TestMidPose.Z
        MyXYZ.GetDisttoOtherPose(testXYZ, Dist)
        badIndex = BTuple.TupleGenConst(0, 0)
        HOperatorSet.TupleFind(BTuple.TupleSgn(BTuple.TupleSub(Dist, epi)), 1, badIndex)
        'badIndex = BTuple.TupleSort(badIndex)
        'badIndex = BTuple.TupleUniq(badIndex)
        If BTuple.TupleSelect(badIndex, 0) = -1 Then
            If BTuple.TupleLength(badIndex) = 1 Then
                badIndex = BTuple.TupleGenConst(0, 0)
            Else
                badIndex = BTuple.TupleRemove(badIndex, 0)
            End If
        End If
    End Sub

    ''' <summary>
    ''' 未使用
    ''' </summary>
    Public Sub SaveData(ByVal strPath As String)

        SaveTupleObj(X, strPath & "_X.tpl")
        SaveTupleObj(Y, strPath & "_Y.tpl")
        SaveTupleObj(Z, strPath & "_Z.tpl")
        SaveTupleObj(CovXYZ, strPath & "_CovXYZ.tpl")

        SaveTupleObj(Pose, strPath & "_Pose.tpl")
        SaveTupleObj(RelPose, strPath & "_RelPose.tpl")
        SaveTupleObj(CovRelPose, strPath & "_CovRelPose.tpl")
        SaveTupleObj(hError, strPath & "_hError.tpl")
    End Sub

    ''' <summary>
    ''' 未使用
    ''' </summary>
    Public Sub ReadData(ByVal StrPath As String)

        ReadTupleObj(X, StrPath & "_X.tpl")
        ReadTupleObj(Y, StrPath & "_Y.tpl")
        ReadTupleObj(Z, StrPath & "_Z.tpl")
        ReadTupleObj(CovXYZ, StrPath & "_CovXYZ.tpl")

        ReadTupleObj(Pose, StrPath & "_Pose.tpl")
        ReadTupleObj(RelPose, StrPath & "_RelPose.tpl")
        ReadTupleObj(CovRelPose, StrPath & "_CovRelPose.tpl")
        ReadTupleObj(hError, StrPath & "_hError.tpl")
    End Sub
    ''' <summary>
    ''' 未使用
    ''' </summary>
    Public Sub CopyToMyPose(ByVal ObjPose As Object)

        Dim hv_Index As New Object
        Dim TmpIndex As New Object

        For hv_Index = 0 To 6 Step 1
            TmpIndex = hv_Index
            Call ExtendVar(Pose, TmpIndex)
            'Pose.SetValue(BTuple.TupleSelect(ObjPose, hv_Index), TmpIndex)

        Next

    End Sub
End Class
