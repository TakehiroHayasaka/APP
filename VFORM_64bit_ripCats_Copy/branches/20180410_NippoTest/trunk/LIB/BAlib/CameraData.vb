Imports HalconDotNet

Public Class CameraData
    Public P As PMat         '投影行列
    Public T As Point3D      '移動ベクトル
    Public R As RMat         '回転行列
    Public W As Point3D      '微少回転要素
    Private Cmat(8) As Double 'カメラ行列
    Public CNP As BAdata.CamNaibuParam '内部パラメータ　SUURI ADD
    Public CID As Integer  ' カメラIDSUURI ADD
    Public Sub New()
        P = New PMat
        T = New Point3D
        R = New RMat
        W = New Point3D
        CNP = New BAdata.CamNaibuParam 'SUURI ADD

    End Sub


    Public Sub New(ByVal Pose As Object, ByVal objNaiPar As BAdata.CamNaibuParam)
        Dim HomMat As New HTuple

        P = New PMat
        T = New Point3D
        R = New RMat
        W = New Point3D
        CNP = New BAdata.CamNaibuParam 'SUURI ADD

        HOperatorSet.PoseToHomMat3d(Pose, HomMat)

        KousinCamMatByFlen_U0_V0(objNaiPar)
        CNP.CopyToMe(objNaiPar) 'SUURI ADD
        P.SetDataByHomMat2(HomMat, Cmat)

        R.Set_R_ByHomMat(HomMat)

        T.X = BTuple.TupleSelect(HomMat, 3).D
        T.Y = BTuple.TupleSelect(HomMat, 7).D
        T.Z = BTuple.TupleSelect(HomMat, 11).D

    End Sub
    'SUURI ADD 20150119
    Public Sub New(ByVal Pose As Object, ByVal objNaiPar As Object)
        Dim HomMat As New HTuple

        P = New PMat
        T = New Point3D
        R = New RMat
        W = New Point3D
        CNP = New BAdata.CamNaibuParam 'SUURI ADD

        HOperatorSet.PoseToHomMat3d(Pose, HomMat)

        CNP.SetDataByCamParam(objNaiPar) 'SUURI ADD

        KousinCamMatByFlen_U0_V0(CNP)

        P.SetDataByHomMat2(HomMat, Cmat)

        R.Set_R_ByHomMat(HomMat)

        T.X = BTuple.TupleSelect(HomMat, 3).D
        T.Y = BTuple.TupleSelect(HomMat, 7).D
        T.Z = BTuple.TupleSelect(HomMat, 11).D

    End Sub

    Public Sub P_wo_Koushin(ByVal objNaiPar As BAdata.CamNaibuParam)
        Dim HomMat As Object = Nothing
        GenHomMatBy_R_to_T(HomMat)
        KousinCamMatByFlen_U0_V0(objNaiPar)
        P.SetDataByHomMat2(HomMat, Cmat)
    End Sub
    Public Sub KousinCamMatByFlen_U0_V0(ByVal objNaiPar As BAdata.CamNaibuParam)
        Cmat(0) = objNaiPar.A * objNaiPar.F / F0
        Cmat(1) = 0
        Cmat(2) = objNaiPar.U0 / F0
        Cmat(3) = 0
        Cmat(4) = objNaiPar.F / F0
        Cmat(5) = objNaiPar.V0 / F0
        Cmat(6) = 0
        Cmat(7) = 0
        Cmat(8) = 1
    End Sub

    Public Sub GenHomMatBy_R_to_T(ByRef HomMat As Object)
        HomMat = New HTuple
        HomMat = BTuple.TupleConcat(HomMat, R.R11)
        HomMat = BTuple.TupleConcat(HomMat, R.R12)
        HomMat = BTuple.TupleConcat(HomMat, R.R13)
        HomMat = BTuple.TupleConcat(HomMat, T.X)
        HomMat = BTuple.TupleConcat(HomMat, R.R21)
        HomMat = BTuple.TupleConcat(HomMat, R.R22)
        HomMat = BTuple.TupleConcat(HomMat, R.R23)
        HomMat = BTuple.TupleConcat(HomMat, T.Y)
        HomMat = BTuple.TupleConcat(HomMat, R.R31)
        HomMat = BTuple.TupleConcat(HomMat, R.R32)
        HomMat = BTuple.TupleConcat(HomMat, R.R33)
        HomMat = BTuple.TupleConcat(HomMat, T.Z)
    End Sub

    Public Function GenPoseBy_R_to_T() As Object
        GenPoseBy_R_to_T = Nothing
        Dim HomMat As New HTuple
        HomMat = BTuple.TupleConcat(HomMat, R.R11)
        HomMat = BTuple.TupleConcat(HomMat, R.R12)
        HomMat = BTuple.TupleConcat(HomMat, R.R13)
        HomMat = BTuple.TupleConcat(HomMat, T.X)
        HomMat = BTuple.TupleConcat(HomMat, R.R21)
        HomMat = BTuple.TupleConcat(HomMat, R.R22)
        HomMat = BTuple.TupleConcat(HomMat, R.R23)
        HomMat = BTuple.TupleConcat(HomMat, T.Y)
        HomMat = BTuple.TupleConcat(HomMat, R.R31)
        HomMat = BTuple.TupleConcat(HomMat, R.R32)
        HomMat = BTuple.TupleConcat(HomMat, R.R33)
        HomMat = BTuple.TupleConcat(HomMat, T.Z)
        HOperatorSet.HomMat3dToPose(HomMat, GenPoseBy_R_to_T)

    End Function

End Class
