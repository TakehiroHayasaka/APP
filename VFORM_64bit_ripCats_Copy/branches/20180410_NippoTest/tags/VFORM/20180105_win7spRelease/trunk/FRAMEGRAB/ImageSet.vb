﻿Imports System.Runtime.InteropServices

'20150203 ADD By Yamada
Public Class ImageSet

    ''' <summary>
    ''' 画像名
    ''' </summary>
    Public ImageName As String
    ''' <summary>
    ''' カメラレンズのひずみ補正をした画像の名前（現在未使用）
    ''' </summary>
    Public ImageTransName As String
    ''' <summary>
    ''' 画像のフルパス
    ''' </summary>
    Public ImageFullPath As String
    ''' <summary>
    ''' カメラレンズのひずみ補正をした画像のパス（現在未使用）
    ''' </summary>
    Public ImageTransPath As String
    ''' <summary>
    ''' カメラレンズのひずみ補正をした画像のフルパス（現在未使用）
    ''' </summary>
    Public ImageTransFullPath As String
    ''' <summary>
    ''' サムネイル画像のフルパス
    ''' </summary>
    Public ImageSmallFullPath As String
    ''' <summary>
    ''' 画像ID
    ''' </summary>
    ''' <remarks>重複しない</remarks>
    Public ImageId As Integer
    ''' <summary>
    ''' 読み込む画像
    ''' </summary>
    ''' Private _hx_Image As HALCONXLib.HUntypedObjectX
    Public ReadOnly Property hx_Image() As HALCONXLib.HUntypedObjectX

        Get
            Dim _hx_image As HALCONXLib.HUntypedObjectX = Nothing
            'GC.Collect()
            'GC.WaitForPendingFinalizers()
            Op.GenEmptyObj(_hx_image)
            Marshal.ReleaseComObject(_hx_image)
            Op.ReadImage(_hx_image, ImageFullPath)
            'Dim Mean As Object = Nothing
            'Op.Intensity(_hx_image, _hx_image, Mean, Nothing)
            'Op.ScaleImage(_hx_image, _hx_image, 100 / Mean, 0)
            Return _hx_image
        End Get

    End Property

    '    ''' <summary>
    '    ''' 画像を取込む
    '    ''' </summary>
    '    Public ReadOnly Property hx_ImageT() As HALCONXLib.HUntypedObjectX

    '        Get
    '            Dim _hx_image As HALCONXLib.HUntypedObjectX = Nothing
    '            GC.Collect()
    '            'GC.WaitForPendingFinalizers() SUURI DEL 20150115
    '            Op.GenEmptyObj(_hx_image)
    '            Marshal.ReleaseComObject(_hx_image)
    '            Op.ReadImage(_hx_image, ImageFullPath)
    '            'Op.GrabImage(_hx_image, IGFlag)

    '            Return _hx_image
    '        End Get

    '    End Property

    '    ''' <summary>
    '    ''' 計測点を画像上に表示するためのオブジェクト（クロス）
    '    ''' </summary>
    '    Public hx_FeatureCross As HALCONXLib.HUntypedObjectX
    '    Public hx_EdgePS As HALCONXLib.HUntypedObjectX
    '    ''' <summary>
    '    ''' FeatureImage.vbのみ使用、ターゲットレス計測（FBMlibライブラリ）
    '    ''' </summary>
    '    Public FeaturePoints As ImagePoints
    '    Public RansacFirst As RansacPoints
    '    Public RansacSecond As RansacPoints
    '    Public RansacFirstIndexBack As Object
    '    Public RansacSecondIndexBack As Object
    '    Public RansacMid As RansacPoints
    '    Public RansacMidFirst As RansacPoints
    '    Public RansacMidSecond As RansacPoints
    '    Public Ransac3ImagePoints(2) As RansacPoints
    '    ''' <summary>
    '    ''' カメラ姿勢
    '    ''' </summary>
    '    ''' <remarks>位置と向き</remarks>
    '    Public ImagePose As CameraPose
    '    ''' <summary>
    '    ''' 3次元点を画像に投影するためのマトリックス
    '    ''' </summary>
    '    Public HomMatInvert As New Object
    '    '  Public WorldPose As WorldPoints
    '    Public MidPose As CameraPose
    '    Public MRS As MatchRansac
    '    Public VectorPose As CameraPose
    '    ''' <summary>
    '    ''' ターゲット（ST、CT）の構造体
    '    ''' </summary>
    '    ''' Public ScaleRegion1 As HALCONXLib.HUntypedObjectX
    '    ''' Public ScaleRegion2 As HALCONXLib.HUntypedObjectX
    '    ''' Public ScalePoint1 As ImagePoints
    '    ''' Public ScalePoint2 As ImagePoints
    '    Public Targets As TargetDetect
    '    ''' <summary>
    '    ''' カメラの姿勢などのリスト
    '    ''' </summary>
    '    Public lstImagePose As List(Of Object)

    '    ''' <summary>
    '    ''' FeatureImage.vb、MeasPoint.vbに使用（どちらもFBMlibライブラリ）
    '    ''' </summary>
    '    Public CommonScale As Double = 1
    '    ''' <summary>
    '    ''' ターゲットレス計測
    '    ''' </summary>
    '    Public CommonHomMat3d As New Object
    '    ''' <summary>
    '    ''' FeatureImage.vbのみ使用（FBMlibライブラリ）
    '    ''' </summary>
    '    Public MeanError As New Object
    '    ''' <summary>
    '    ''' FeatureImage.vbのみ使用（FBMlibライブラリ）
    '    ''' </summary>
    '    Public DevError As New Object
    '    Public MidCount As New Object
    '    Public Scale As Double = 1
    '    Public ScaleBA As Double = 1
    '    ''' <summary>
    '    ''' 解析時の第一カメラのフラグ
    '    ''' </summary>
    '    Public flgFirst As Boolean
    '    ''' <summary>
    '    ''' 解析時の第二カメラのフラグ
    '    ''' </summary>
    '    Public flgSecond As Boolean

    '    ''' <summary>
    '    ''' 画像読み込み時の最後の画像フラグ
    '    ''' </summary>
    '    Public flgLast As Boolean
    '    ''' <summary>
    '    ''' ImageSet.vbのみ使用
    '    ''' </summary>
    '    Public flgN As Boolean
    '    ''' <summary>
    '    ''' 画像が繋がったかどうか、のフラグ
    '    ''' </summary>
    '    Public flgConnected As Boolean
    '    ''' <summary>
    '    ''' 画像が使えるかどうかのフラグ
    '    ''' </summary>
    '    Public flgUsable As Boolean
    '    Public Pose As Object
    '    ''' <summary>
    '    ''' 画像が繋がった時の質を表す（繋げた際のコードターゲットの3次元点同士の距離の配列）
    '    ''' </summary>
    '    Public Quality As Object

    '    Public objCamparam As CameraParam '20150115 SUURI ADD 複数カメラ内部パラメータ取扱機能

    '    ''' <summary>
    '    ''' 初期化
    '    ''' </summary>
    '    Public Sub New()
    '        ImageName = ""
    '        ImageId = -1
    '        ' hx_Image = New HALCONXLib.HUntypedObjectX

    '        FeaturePoints = New ImagePoints
    '        hx_FeatureCross = Nothing
    '        hx_EdgePS = Nothing

    '#If Halcon = "True" Then
    '        Op.GenEmptyObj(hx_FeatureCross)
    '        RansacSecond = New RansacPoints
    '        RansacFirst = New RansacPoints
    '        RansacMid = New RansacPoints
    '        RansacMidFirst = New RansacPoints
    '        RansacMidSecond = New RansacPoints
    '#End If

    '        'ScaleRegion1 = New HALCONXLib.HUntypedObjectX
    '        'ScaleRegion2 = New HALCONXLib.HUntypedObjectX
    '        'ScalePoint1 = New ImagePoints
    '        'ScalePoint2 = New ImagePoints
    '        flgN = False
    '        MRS = New MatchRansac(1)
    '        RansacFirstIndexBack = Nothing
    '        RansacSecondIndexBack = Nothing
    '        Pose = Nothing
    '        Quality = Nothing
    '        ReDim Ransac3ImagePoints(2)
    '        ImagePose = New CameraPose
    '        VectorPose = New CameraPose
    '        MidPose = New CameraPose
    '        ' WorldPose = New WorldPoints
    '        flgFirst = False
    '        flgLast = False
    '        flgConnected = False
    '        Targets = New TargetDetect
    '        objCamparam = New CameraParam'20150115 SUURI ADD 複数カメラ内部パラメータ取扱機能

    '    End Sub

    '    Public Sub New(ByVal ISI As ImageSet)
    '        ImageName = ""
    '        ImageId = -1
    '        ' hx_Image = New HALCONXLib.HUntypedObjectX
    '        hx_FeatureCross = Nothing
    '        Op.GenEmptyObj(hx_FeatureCross)
    '        FeaturePoints = New ImagePoints
    '        RansacSecond = New RansacPoints
    '        RansacFirst = New RansacPoints
    '        RansacMid = New RansacPoints
    '        RansacMidFirst = New RansacPoints
    '        RansacMidSecond = New RansacPoints
    '        'ScaleRegion1 = New HALCONXLib.HUntypedObjectX
    '        'ScaleRegion2 = New HALCONXLib.HUntypedObjectX
    '        'ScalePoint1 = New ImagePoints
    '        'ScalePoint2 = New ImagePoints
    '        flgN = False
    '        MRS = New MatchRansac(1)
    '        RansacFirstIndexBack = Nothing
    '        RansacSecondIndexBack = Nothing
    '        Pose = Nothing
    '        Quality = Nothing
    '        ReDim Ransac3ImagePoints(2)
    '        ImagePose = New CameraPose
    '        VectorPose = New CameraPose
    '        MidPose = New CameraPose
    '        ' WorldPose = New WorldPoints
    '        flgFirst = False
    '        flgLast = False
    '        flgConnected = False
    '        Targets = New TargetDetect
    '        objCamparam = New CameraParam '20150115 SUURI ADD 複数カメラ内部パラメータ取扱機能

    '        ImageName = ISI.ImageName

    '        ''' <summary>
    '        ''' カメラレンズのひずみ補正をした画像の名前（現在未使用）
    '        ''' </summary>
    '        ImageTransName = ISI.ImageTransName
    '        ''' <summary>
    '        ''' 画像のフルパス
    '        ''' </summary>
    '        ImageFullPath = ISI.ImageFullPath
    '        ''' <summary>
    '        ''' カメラレンズのひずみ補正をした画像のパス（現在未使用）
    '        ''' </summary>
    '        ImageTransPath = ISI.ImageTransPath
    '        ''' <summary>
    '        ''' カメラレンズのひずみ補正をした画像のフルパス（現在未使用）
    '        ''' </summary>
    '        ImageTransFullPath = ISI.ImageTransFullPath
    '        ''' <summary>
    '        ''' サムネイル画像のフルパス
    '        ''' </summary>
    '        ImageSmallFullPath = ISI.ImageSmallFullPath

    '    End Sub

    '    ''' <summary>
    '    ''' ターゲットレス計測
    '    ''' </summary>
    '    Public Sub SubSetFirst()
    '        RansacFirst.SubSet(FeaturePoints)
    '    End Sub

    '    ''' <summary>
    '    ''' ターゲットレス計測
    '    ''' </summary>
    '    Public Sub SubSetSecond()
    '        RansacSecond.SubSet(FeaturePoints)
    '    End Sub

    '    ''' <summary>
    '    ''' ターゲットレス計測
    '    ''' </summary>
    '    Public Sub SubSetMid()
    '        RansacMid_SubSet()
    '        MidPose.X = Tuple.TupleSelect(ImagePose.X, RansacMid.RansacPointsIndex)
    '        MidPose.Y = Tuple.TupleSelect(ImagePose.Y, RansacMid.RansacPointsIndex)
    '        MidPose.Z = Tuple.TupleSelect(ImagePose.Z, RansacMid.RansacPointsIndex)

    '    End Sub
    '    ''' <summary>
    '    ''' ターゲットレス計測
    '    ''' </summary>
    '    Public Sub RansacMid_SubSet()
    '        RansacMid.SubSet(FeaturePoints)
    '    End Sub

    '    Public Function GetScalePoint() As Double
    '        ' If flgN = False Then
    '        'GetPointScale(ScaleRegion1, ScalePoint1)
    '        'GetPointScale(ScaleRegion2, ScalePoint2)
    '        flgN = True
    '        '  End If

    '    End Function

    '    ''' <summary>
    '    ''' 未使用
    '    ''' </summary>
    '    Private Sub GetPointScale(ByRef hx_Region As HALCONXLib.HUntypedObjectX, ByRef ScalePoint As ImagePoints)
    '        'Dim hx_Reduce As New HALCONXLib.HUntypedObjectX
    '        'Dim hTarget As New HALCONXLib.HUntypedObjectX
    '        'Op.DilationCircle(hx_Region, hx_Region, 20)
    '        'Op.ReduceDomain(hx_Image, hx_Region, hx_Reduce)
    '        ''Op.MeanImage(hx_Reduce, hx_Reduce, 10, 10)
    '        'Op.Threshold(hx_Reduce, hTarget, 180, 255)
    '        'Op.Connection(hTarget, hTarget)
    '        'Op.SelectShapeStd(hTarget, hx_Region, "max_area", 100)
    '        'Op.AreaCenter(hx_Region, Nothing, ScalePoint.Row, ScalePoint.Col)
    '        ' Op.DilationCircle(hTarget, hx_Region, 20)
    '    End Sub

    '    Public Sub SaveImageSet(ByVal strProjectPath As String)
    '#If False Then


    '        Dim strSaveFullPath As String
    '        strSaveFullPath = strProjectPath & ImageId & "\"
    '        Dim folderExists As Boolean
    '        folderExists = My.Computer.FileSystem.DirectoryExists(strSaveFullPath)
    '        If folderExists = False Then
    '            My.Computer.FileSystem.CreateDirectory(strSaveFullPath)
    '        End If

    '        'Op.WriteImage(hx_Image, "jpeg", 0, strSaveFullPath & ImageName)
    '        FeaturePoints.SaveData(strSaveFullPath & "FeaturePoints")
    '        'RansacFirst.SaveData(strSaveFullPath & "RansacFirst")
    '        'RansacSecond.SaveData(strSaveFullPath & "RansacSecond")
    '        'SaveTupleObj(RansacFirstIndexBack, strSaveFullPath & "RansacFirstIndexBack.tpl")
    '        'SaveTupleObj(RansacSecondIndexBack, strSaveFullPath & "RansacSecondIndexBack.tpl")
    '        'RansacMid.SaveData(strSaveFullPath & "RansacMid")
    '        'RansacMidFirst.SaveData(strSaveFullPath & "RansacMidFirst")
    '        'RansacMidSecond.SaveData(strSaveFullPath & "RansacMidSecond")
    '        ImagePose.SaveData(strSaveFullPath & "ImagePose")
    '        'MidPose.SaveData(strSaveFullPath & "MidPose")
    '        'SaveTupleObj(CommonHomMat3d, strSaveFullPath & "CommonHomMat3d.tpl")
    '        'VectorPose.SaveData(strSaveFullPath & "VectorPose")

    '        Dim objSaveTuple As Object = Nothing
    '        'objSaveTuple = Tuple.TupleGenConst(0, 0)
    '        ExtendVar(objSaveTuple, 7)
    '        objSaveTuple.setvalue(CommonScale, 0)
    '        objSaveTuple.setvalue(Scale, 1)
    '        objSaveTuple.setvalue(ScaleBA, 2)
    '        objSaveTuple.setvalue(CStr(flgFirst), 3)
    '        objSaveTuple.setvalue(CStr(flgLast), 4)
    '        objSaveTuple.setvalue(CStr(flgN), 5)
    '        objSaveTuple.setvalue(CStr(flgConnected), 6)
    '        objSaveTuple.setvalue(CStr(flgSecond), 7)
    '        WriteMRS_Xml(strSaveFullPath & "MRS.xml")
    '        'objSaveTuple = Tuple.TupleConcat(CommonScale, Scale)
    '        'objSaveTuple = Tuple.TupleConcat(objSaveTuple, ScaleBA)
    '        'objSaveTuple = Tuple.TupleConcat(objSaveTuple, "false")
    '        'objSaveTuple = Tuple.TupleConcat(objSaveTuple, "true")
    '        'objSaveTuple = Tuple.TupleConcat(objSaveTuple, "false")
    '        'objSaveTuple = Tuple.TupleConcat(Tuple.TupleConcat(Tuple.TupleConcat(Tuple.TupleConcat(Tuple.TupleConcat(CommonScale, Scale), ScaleBA), flgFirst), flgLast), flgN)
    '        SaveTupleObj(objSaveTuple, strSaveFullPath & "OtherParams.tpl")
    '        SaveTupleObj(MeanError, strSaveFullPath & "MeanError.tpl")
    '        SaveTupleObj(DevError, strSaveFullPath & "DevError.tpl")
    '        SaveTupleObj(MidCount, strSaveFullPath & "MidCount.tpl")
    '#End If
    '        SaveCameraPose()
    '        Targets.SaveData()

    '    End Sub

    '    ''' <summary>
    '    ''' カメラの姿勢を保存
    '    ''' </summary>
    '    Private Function SaveCameraPose() As Long

    '        Dim strFieldData(11) As String
    '        strFieldData(0) = ImageId
    '        strFieldData(1) = "'" & ImageName & "'"
    '        If flgConnected = True Then
    '            strFieldData(2) = ImagePose.Pose(0)
    '            strFieldData(3) = ImagePose.Pose(1)
    '            strFieldData(4) = ImagePose.Pose(2)
    '            strFieldData(5) = ImagePose.Pose(3)
    '            strFieldData(6) = ImagePose.Pose(4)
    '            strFieldData(7) = ImagePose.Pose(5)
    '        Else
    '            strFieldData(2) = ""
    '            strFieldData(3) = ""
    '            strFieldData(4) = ""
    '            strFieldData(5) = ""
    '            strFieldData(6) = ""
    '            strFieldData(7) = ""
    '        End If
    '        strFieldData(8) = "0"
    '        strFieldData(9) = IIf(flgConnected = True, "1", "0")
    '        strFieldData(10) = IIf(flgFirst = True, "1", "0")
    '        strFieldData(11) = IIf(flgSecond = True, "1", "0")
    '        SaveCameraPose = dbClass.DoInsert(CameraPoseFields, "CameraPose", strFieldData)

    '    End Function

    '    Public Sub ReadImageSet(ByVal strProjectPath As String)



    '        'Dim strSaveFullPath As String
    '        'strSaveFullPath = strProjectPath & ImageId & "\"
    '        'Dim folderExists As Boolean
    '        'folderExists = My.Computer.FileSystem.DirectoryExists(strSaveFullPath)
    '        'If folderExists = False Then
    '        '    Exit Sub
    '        'End If
    '#If False Then
    '        'Op.ReadImage(hx_Image, "")
    '        FeaturePoints.ReadData(strSaveFullPath & "FeaturePoints")
    '        Marshal.ReleaseComObject(hx_FeatureCross)
    '        Op.GenCrossContourXld(hx_FeatureCross, FeaturePoints.Row, FeaturePoints.Col, CrossSize, CrossAngle)
    '        RansacFirst.ReadData(strSaveFullPath & "RansacFirst")
    '        RansacSecond.ReadData(strSaveFullPath & "RansacSecond")

    '        ReadTupleObj(RansacFirstIndexBack, strSaveFullPath & "RansacFirstIndexBack.tpl")
    '        ReadTupleObj(RansacSecondIndexBack, strSaveFullPath & "RansacSecondIndexBack.tpl")

    '        RansacMid.ReadData(strSaveFullPath & "RansacMid")
    '        RansacMidFirst.ReadData(strSaveFullPath & "RansacMidFirst")
    '        RansacMidSecond.ReadData(strSaveFullPath & "RansacMidSecond")
    '        ImagePose.ReadData(strSaveFullPath & "ImagePose")
    '        MidPose.ReadData(strSaveFullPath & "MidPose")
    '        ReadTupleObj(CommonHomMat3d, strSaveFullPath & "CommonHomMat3d.tpl")
    '        VectorPose.ReadData(strSaveFullPath & "VectorPose")
    '        Dim objOtherParams As New Object

    '        If ReadTupleObj(objOtherParams, strSaveFullPath & "OtherParams.tpl") Then
    '            CommonScale = CDbl(Tuple.TupleSelect(objOtherParams, 0))
    '            Scale = CDbl(Tuple.TupleSelect(objOtherParams, 1))
    '            ScaleBA = CDbl(Tuple.TupleSelect(objOtherParams, 2))
    '            flgFirst = CBool(Tuple.TupleSelect(objOtherParams, 3))
    '            flgLast = CBool(Tuple.TupleSelect(objOtherParams, 4))
    '            flgN = CBool(Tuple.TupleSelect(objOtherParams, 5))
    '            Try
    '                flgConnected = CBool(Tuple.TupleSelect(objOtherParams, 6))
    '            Catch ex As Exception
    '                flgConnected = False
    '            End Try
    '            Try
    '                flgSecond = CBool(Tuple.TupleSelect(objOtherParams, 7))
    '            Catch ex As Exception
    '                flgSecond = False
    '            End Try

    '        End If

    '        ReadMRS_Xml(strSaveFullPath & "MRS.xml")

    '        ReadTupleObj(MeanError, strSaveFullPath & "MeanError.tpl")
    '        ReadTupleObj(DevError, strSaveFullPath & "DevError.tpl")
    '        ReadTupleObj(MidCount, strSaveFullPath & "MidCount.tpl")
    '#End If
    '        Dim IDR As IDataReader
    '        Dim strSqlText As String = ""
    '        strSqlText = "SELECT "
    '        Dim n As Integer = CameraPoseFields.Length
    '        Dim i As Integer
    '        For i = 0 To n - 2
    '            strSqlText = strSqlText & CameraPoseFields(i) & ","
    '        Next
    '        strSqlText = strSqlText & CameraPoseFields(n - 1) & " "
    '        strSqlText = strSqlText & "FROM CameraPose WHERE ID=" & ImageId
    '        IDR = dbClass.DoSelect(strSqlText)
    '        If Not IDR Is Nothing Then
    '            IDR.Read()
    '            flgConnected = IIf(IDR.GetValue(9) = 1, True, False)
    '            If flgConnected = True Then
    '                ' Op.CreatePose(0, 0, 0, 0, 0, 0, "Rp+T", "gba", "point", ImagePose.Pose)
    '                ImagePose.Pose = New Object()
    '                ReDim ImagePose.Pose(6)
    '                ImagePose.Pose(0) = IDR.GetValue(2)
    '                ImagePose.Pose(1) = IDR.GetValue(3)
    '                ImagePose.Pose(2) = IDR.GetValue(4)
    '                ImagePose.Pose(3) = IDR.GetValue(5)
    '                ImagePose.Pose(4) = IDR.GetValue(6)
    '                ImagePose.Pose(5) = IDR.GetValue(7)
    '                ImagePose.Pose(6) = 0
    '            End If
    '            flgFirst = IIf(IDR.GetValue(10) = 1, True, False)
    '            flgSecond = IIf(IDR.GetValue(11) = 1, True, False)
    '            IDR.Close()
    '        End If

    '        Targets.ReadData(strProjectPath, ImageId)
    '        lstImagePose = New List(Of Object)
    '#If Halcon = "True" Then
    '        Marshal.ReleaseComObject(hx_FeatureCross)
    '        Op.GenCrossContourXld(hx_FeatureCross, Targets.All2D.Row, Targets.All2D.Col, CrossSize, CrossAngle)
    '#End If

    '    End Sub

    '    ''' <summary>
    '    ''' ターゲットレス計測
    '    ''' </summary>
    '    Public Sub WriteMRS_Xml(ByVal strPath As String)
    '        Dim xmlSerializer As System.Xml.Serialization.XmlSerializer

    '        xmlSerializer = New System.Xml.Serialization.XmlSerializer(GetType(MatchRansac))
    '        Using fileStream As New System.IO.FileStream(strPath, IO.FileMode.Create)
    '            xmlSerializer.Serialize(fileStream, MRS)
    '        End Using
    '    End Sub

    '    ''' <summary>
    '    ''' 未使用
    '    ''' </summary>
    '    Public Sub ReadMRS_Xml(ByVal strPath As String)
    '        Dim xmlSerializer As System.Xml.Serialization.XmlSerializer
    '        xmlSerializer = New System.Xml.Serialization.XmlSerializer(GetType(MatchRansac))
    '        Using fileStream As New System.IO.FileStream(strPath, IO.FileMode.Open)
    '            'デシリアル 
    '            MRS = xmlSerializer.Deserialize(fileStream)
    '        End Using
    '    End Sub

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    Public Sub CalcRay(ByVal hv_CamparamOut As Object)
    '        Dim hvHomMat3d As Object = Nothing
    '        If Not ImagePose.Pose Is Nothing Then
    '            Op.PoseToHomMat3d(ImagePose.Pose, hvHomMat3d)
    '            For Each ST As SingleTarget In Targets.lstST
    '                CalcRayOfOneSingleTarget(ST, hv_CamparamOut, hvHomMat3d)
    '            Next
    '            For Each CT As CodedTarget In Targets.lstCT
    '                CT.flgUsable = True '!!!!!!!!SUURI!!!!!!!!!
    '                For Each CT_no_ST As SingleTarget In CT.lstCTtoST
    '                    CT_no_ST.flgUsed = 1 '!!!!!!!!SUURI!!!!!!!!!

    '                    CalcRayOfOneSingleTarget(CT_no_ST, hv_CamparamOut, hvHomMat3d)
    '                Next
    '            Next
    '        End If
    '    End Sub

    '	'20150115 SUURI ADD Sta 複数カメラ内部パラメータ取扱機能--------
    '    Public Sub CalcRay()
    '        CalcRay(objCamparam.Camparam)
    '    End Sub
    '	'20150115 SUURI ADD End 複数カメラ内部パラメータ取扱機能--------

    '    ''' <summary>
    '    ''' 固有アフィン 3 次元変換をSTに適用する。
    '    ''' </summary>
    '    Private Sub CalcRayOfOneSingleTarget(ByRef ST As SingleTarget, ByVal hv_CamparamOut As Object, ByVal hvHomMat3d As Object)

    '        Op.GetLineOfSight(ST.P2D.Row, ST.P2D.Col, hv_CamparamOut, _
    '                        ST.RayP1.X, ST.RayP1.Y, ST.RayP1.Z, ST.RayP2.X, ST.RayP2.Y, ST.RayP2.Z) '画像中の点に対応する視線を計算する。

    '        Op.AffineTransPoint3D(hvHomMat3d, ST.RayP1.X, ST.RayP1.Y, ST.RayP1.Z, ST.RayP1.X, ST.RayP1.Y, ST.RayP1.Z) '固有アフィン 3 次元変換を点に適用する。

    '        Op.AffineTransPoint3D(hvHomMat3d, ST.RayP2.X, ST.RayP2.Y, ST.RayP2.Z, ST.RayP2.X, ST.RayP2.Y, ST.RayP2.Z)
    '    End Sub
    '    'Public Sub CalcRay()
    '    '    Dim hvHomMat3d As Object = Nothing
    '    '    Op.PoseToHomMat3d(ImagePose.Pose, hvHomMat3d)

    '    '    For Each ST As SingleTarget In Targets.lstST
    '    '        Op.AffineTransPoint3D(hvHomMat3d, ST.RayP1.X, ST.RayP1.Y, ST.RayP1.Z, ST.RayP1.X, ST.RayP1.Y, ST.RayP1.Z)
    '    '        Op.AffineTransPoint3D(hvHomMat3d, ST.RayP2.X, ST.RayP2.Y, ST.RayP2.Z, ST.RayP2.X, ST.RayP2.Y, ST.RayP2.Z)
    '    '    Next

    '    'End Sub

    '    Public Function CalcPoseByCommonCT3dPoint(ByVal Camparam As Object, ByRef lstCommon3dCT As List(Of Common3DCodedTarget), _
    '                                             ByRef Pose As Object, ByRef Quality As Object) As Boolean
    '        Dim MyCT_3dPoint As New Point3D
    '        Dim MyIP1 As New ImagePoints
    '        Dim i As Integer = 0
    '        Dim j As Integer = 0
    '        Dim t As Integer = 0
    '        Dim cmnCTnum As Integer = 0
    '        CalcPoseByCommonCT3dPoint = False
    '        For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
    '            If C3DCT.flgUsable = True Then
    '                For Each CT As CodedTarget In Targets.lstCT
    '                    If C3DCT.PID = CT.CT_ID Then
    '                        For i = 0 To CodedTarget.CTnoSTnum - 1
    '                            MyCT_3dPoint.ConcatToMe(C3DCT.lstP3d.Item(i))
    '                        Next
    '                        MyIP1.ConcatToMe(CT.CT_Points)
    '                        cmnCTnum += 1
    '                    End If
    '                Next
    '            End If
    '        Next

    '        If cmnCTnum < intCommonCTnum Then
    '            Exit Function
    '        End If


    '        Try
    '            Dim hv_PoseOut As Object = Nothing
    '            Op.VectorToPose(MyCT_3dPoint.X, MyCT_3dPoint.Y, MyCT_3dPoint.Z, MyIP1.Row, MyIP1.Col, _
    '                 Camparam, "iterative", "error", Pose, Quality)

    '            If Quality > 1 Then
    '                'Exit Function

    '            End If
    '            ' Quality = RunBA_PoseOnly(FBMlib.T_treshold, Camparam, Pose, MyIP1, MyCT_3dPoint, Tuple.TupleLength(MyCT_3dPoint.Z))
    '            Op.PoseToHomMat3d(Pose, hv_PoseOut)
    '            Op.HomMat3dInvert(hv_PoseOut, hv_PoseOut)
    '            Op.HomMat3dToPose(hv_PoseOut, Pose)


    '            ' Quality = RunBA_PoseOnly(FBMlib.T_treshold, Camparam, Pose, MyIP1, MyCT_3dPoint, Tuple.TupleLength(MyCT_3dPoint.Z))
    '        Catch ex As Exception
    '            Exit Function
    '        End Try



    '        'If Quality > 100 Then
    '        '    Exit Function
    '        'End If
    '        CalcPoseByCommonCT3dPoint = True

    '    End Function

    '    ''' <summary>
    '    ''' 3次元姿勢情報パラメーターを同次変換行列に変換し、さらにその行列の逆行列を算出する
    '    ''' </summary>
    '    Public Sub CalcInvertHomMat()
    '        Dim hvHommat3d As Object = Nothing
    '        If flgConnected = True Then
    '            Op.PoseToHomMat3d(ImagePose.Pose, hvHommat3d) '3次元姿勢情報パラメーターを同次変換行列に変換する（HALCON）
    '            Op.HomMat3dInvert(hvHommat3d, HomMatInvert) '同次3次元変換行列の逆行列を算出する（HALCON）
    '        End If
    '    End Sub

    '    ''' <summary>
    '    ''' ピクセル座標の放射歪みを変更
    '    ''' </summary>
    '    Public Sub ChangeRadialDistortionP2D(ByVal CamparamIn As Object, ByVal CamparamOut As Object)
    '        '変更する・P2Dを・放射状のひずみ?

    '        If Tuple.TupleLength(Targets.All2D_ST.Row) > 0 Then
    '            Op.ChangeRadialDistortionPoints(Targets.All2D_ST.Row, Targets.All2D_ST.Col, CamparamIn, CamparamOut, Targets.All2D_ST_Trans.Row, Targets.All2D_ST_Trans.Col)
    '            'ChangeRadialDistortionPoints:ピクセル座標の放射歪みを変更。 
    '            For Each ST As SingleTarget In Targets.lstST
    '                Op.ChangeRadialDistortionPoints(ST.P2D.Row, ST.P2D.Col, CamparamIn, CamparamOut, ST.TP2D.Row, ST.TP2D.Col)
    '            Next
    '        End If
    '    End Sub



    '    ''' <summary>
    '    ''' 画面上でクリックされた点
    '    ''' </summary>
    '    Public Function GetClickedTarget(ByVal R As Object, ByVal C As Object, ByRef minVal As Double) As SingleTarget
    '        GetClickedTarget = Nothing
    '        Dim objDist As Object = DBNull.Value
    '        minVal = Double.MaxValue
    '        For Each ST As SingleTarget In Targets.lstST
    '            ST.P2D.CalcDistToInputPoint(R, C, objDist)
    '            If objDist < minVal Then
    '                minVal = objDist
    '                GetClickedTarget = ST
    '            End If
    '        Next
    '        For Each CT As CodedTarget In Targets.lstCT
    '            For Each ST As SingleTarget In CT.lstCTtoST
    '                ST.P2D.CalcDistToInputPoint(R, C, objDist)
    '                If objDist < minVal Then
    '                    minVal = objDist
    '                    GetClickedTarget = ST
    '                End If
    '            Next
    '        Next
    '    End Function

    '    Public Function GetST_MaxID() As Integer
    '        GetST_MaxID = Targets.GetST_MaxID

    '    End Function

    '    '20150115 SUURI ADD Sta 複数カメラ内部パラメータ取扱機能--------
    '    Public Function GetCamParamID(ByVal strReadPath As String) As Integer


    '        Dim IDR As IDataReader
    '        Dim strSqlText As String = ""

    '        ConnectDbFBM(strReadPath)

    '        strSqlText = "SELECT CamParamID FROM CameraPose WHERE ID = " & ImageId

    '        IDR = dbClass.DoSelect(strSqlText)
    '        If Not IDR Is Nothing Then
    '            Do While IDR.Read
    '                GetCamParamID = CInt(IDR.GetValue(0))
    '            Loop
    '            IDR.Close()
    '        End If

    '        AccessDisConnect()
    '    End Function
    '	'20150115 SUURI ADD End 複数カメラ内部パラメータ取扱機能--------

    'End Class

    'Public Class MatchRansac

    '    Public GrayMatchMethod As String

    '    Public MaskSize As Integer

    '    Public RowMove As Integer

    '    Public ColMove As Integer

    '    Public RowTolerance As Integer

    '    Public ColTolerance As Integer

    '    Public Rotation As Object

    '    Public MatchThreshold As Double

    '    Public EstimationMethod As String '= "gold_standard"

    '    Public DistanceThreshold As Double

    '    Public RandSeed As Integer
    '    Public Sub New()

    '    End Sub
    '    Public Sub New(ByVal D As Integer)
    '        My.Settings.Upgrade()
    '        With My.Settings

    '            GrayMatchMethod = .GrayMatchMethod

    '            MaskSize = .MaskSize

    '            RowMove = .RowMove

    '            ColMove = .ColMove

    '            RowTolerance = .RowTolerance

    '            ColTolerance = .ColTolerance

    '            Rotation = .Rotation

    '            MatchThreshold = .MatchThreshold

    '            EstimationMethod = .EstimationMethod

    '            DistanceThreshold = .DistanceThreshold

    '            RandSeed = .RandSeed

    '        End With
    '    End Sub

End Class

