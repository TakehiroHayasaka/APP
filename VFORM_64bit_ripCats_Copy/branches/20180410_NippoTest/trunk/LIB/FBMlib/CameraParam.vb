Imports HalconDotNet

Public Class CameraParam
    Public Cid As Integer '使用カメラID
    Public CName As String '使用カメラ名
    Public Cpath As String '使用カメラの内部パラメータのパス
    Public Camparam As Object '使用カメラの内部パラメータ
    Public CamParamFirst As Object
    Public CamParamZero As Object '使用カメラのひずみ係数ゼロの内部パラメータ
    Public CamSerialNo As String    '20150216 ADD BY Suuri 新キャリブレーション対応機能

    Public Sub New()
        Cid = -1
        CName = ""
        Cpath = ""
        Camparam = Nothing
        CamSerialNo = ""    '20150216 ADD BY Suuri 新キャリブレーション対応機能
    End Sub

    Public Sub New(strCamparamPath As String)
        Cpath = strCamparamPath
        ' HOperatorSet.ReadCamPar(Cpath, Camparam)
        ReadCamParamFromFile()
    End Sub
    Public Sub ReadCamParamFromFile()
        '20150216 Rep BY Suuri 新キャリブレーション対応機能 Sta--
        'HOperatorSet.ReadCamPar(Cpath, Camparam)
        'HOperatorSet.ReadCamPar(Cpath, CamParamFirst)

        Try
            HOperatorSet.ReadCamPar(Cpath, Camparam)
            HOperatorSet.ReadCamPar(Cpath, CamParamFirst)
        Catch ex As Exception
            HOperatorSet.ReadTuple(Cpath, Camparam)
            HOperatorSet.ReadTuple(Cpath, CamParamFirst)
        End Try
        '20150216 Rep BY Suuri 新キャリブレーション対応機能 End--

        Dim Zeros As Object
        Zeros = BTuple.TupleGenConst(5, 0)
        HOperatorSet.ChangeRadialDistortionCamPar("fixed", Camparam, Zeros, CamParamZero)
    End Sub
End Class
