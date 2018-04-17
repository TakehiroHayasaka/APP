Public Class CameraParam
    Public Cid As Integer '使用カメラID
    Public CName As String '使用カメラ名
    Public Cpath As String '使用カメラの内部パラメータのパス
    Public Camparam As Object '使用カメラの内部パラメータ
    Public CamParamFirst As Object
    Public CamParamZero As Object '使用カメラのひずみ係数ゼロの内部パラメータ
    Public CamSerialNo As String
    Public objAcqHandle As Object 'カメラ制御用オブジェクト
    Public hwindhand As Object
    Public objCheckBox As CheckBox
    Public hvImageResult As HALCONXLib.HUntypedObjectX = Nothing
    Public objResultTarget As TargetDetect = Nothing
    Public lstAllTarget As List(Of TargetDetect) 'SUURI ADD
    Public lstImages As List(Of HALCONXLib.HUntypedObjectX)
    Public Fr As HalconDotNet.HFramegrabber
    Public lstImagesDotNet As List(Of HalconDotNet.HImage)
    Public blnCamConnectOK As Boolean = True
    Public blnCamShootOK As Boolean = True
    Public blnCar As Boolean = False
    Public lstNoCarFlg As List(Of Boolean)
    Public lstCarExistFlg As List(Of Boolean)
    Public Sub New()
        Cid = -1
        CName = ""
        Cpath = ""
        Camparam = Nothing
        CamSerialNo = ""
        objAcqHandle = Nothing
        hwindhand = Nothing
        objCheckBox = Nothing
        hvImageResult = Nothing
        lstImages = New List(Of HALCONXLib.HUntypedObjectX)
        objResultTarget = New TargetDetect
        lstImagesDotNet = New List(Of HalconDotNet.HImage)
        Fr = Nothing
        blnCamConnectOK = True
        blnCamShootOK = True
        blnCar = False
        lstNoCarFlg = New List(Of Boolean)
        lstCarExistFlg = New List(Of Boolean)
    End Sub

    Public Sub New(strCamparamPath As String)
        Cpath = strCamparamPath
        ' Op.ReadCamPar(Cpath, Camparam)
        ReadCamParamFromFile()
    End Sub
    Public Sub ReadCamParamFromFile()
        Op.ReadCamPar(Cpath, Camparam)
        Op.ReadCamPar(Cpath, CamParamFirst)
        Dim Zeros As Object
        Zeros = Tuple.TupleGenConst(5, 0)
        Op.ChangeRadialDistortionCamPar("fixed", Camparam, Zeros, CamParamZero)
    End Sub
End Class
