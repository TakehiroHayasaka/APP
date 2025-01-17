﻿Imports System.Runtime.InteropServices
Imports System.Threading.Tasks
Imports Microsoft.VisualBasic.FileIO
Imports HalconDotNet
Imports System.IO

Public Class FeatureImage

    Private _projectpath As String
    Public Property ProjectPath() As String
        Get
            Return _projectpath
        End Get
        Set(ByVal value As String)
            _projectpath = value
            For Each ISI As ImageSet In lstImages
                ISI.ImageName = getFileNamefromFullPath(ISI.ImageFullPath)
                ISI.ImageFullPath = _projectpath & "\" & ISI.ImageName
                ISI.ImageTransPath = _projectpath & "\Pdata\TransImage\"
                ISI.ImageTransName = ISI.ImageName & "_trans"
                ISI.ImageTransFullPath = ISI.ImageTransPath & ISI.ImageTransName
                ISI.ImageSmallFullPath = _projectpath & "\Pdata\SmallImage\" & ISI.ImageTransName
            Next
        End Set
    End Property
    Public Property ProjectPath(ByVal flgNew As Boolean) As String
        Get
            Return _projectpath
        End Get
        Set(ByVal value As String)
            _projectpath = value
            ImportImages(flgNew) '20121220新規作成、フォルダの指定後
        End Set
    End Property
    Private _campar_path As String
    Public ReadOnly Property CamparamPath()
        Get
            Return _campar_path
        End Get
    End Property
    Private Const strSavePath As String = "\Pdata\"
    Public lstImages As List(Of ImageSet)
    Public lst3dPoints As List(Of MeasurePoint)
    Public lstObjects As List(Of Objects)
    ' Public lstImagePair As List(Of ImagePairSet)

    Public lstCommon3dST As List(Of Common3DSingleTarget)
    Public lstCommon3dCT As List(Of Common3DCodedTarget)

    Public hv_CamparamFirst As HTuple = Nothing
    Public hv_CamparamOut As HTuple = Nothing
    Public hv_CamparamZero As HTuple = Nothing
    Public Map As New HObject

    '150115 SUURI ADD 複数カメラ内部パラメータ取扱機能
    Public lstCamParam As List(Of CameraParam)
    Public lstBasePoint As List(Of Common3DCodedTarget)
    Public lstAllHikakuPoint As List(Of Common3DCodedTarget)
    Public WorldXYZ As New Point3D
    Public hv_Width As HTuple
    Public hv_Height As HTuple
    Private ScaleMM As Double = 1
    Private cntConnectedImage As Integer = -1
    Private MinCountOfRays As Integer = 3
    Dim XorY As Integer
    Public Event DetectCT(ByVal sender As Object, ByVal e As EventArgs)
    Public Event ImageReaded(ByVal sender As Object, ByVal e As EventArgs)
    Public Event Progress(ByVal sender As Object, ByVal e As EventArgs)
    Public ErrCode As Integer '12.12.20エラーコード（=1：画像が無い場合 / =2以降は追加していく）

    Public Property pScaleMM() As Double
        Get
            Return ScaleMM
        End Get
        Set(ByVal value As Double)
            ScaleMM = value
            'If lst3dPoints Is Nothing Then
            'Else
            '    If lst3dPoints.Count = 0 And ScaleMM = 0 Then
            '    Else
            '        For Each MP As MeasurePoint In lst3dPoints
            '            MP.Pnt.SetScale(ScaleMM)
            '        Next
            '    End If
            'End If
        End Set
    End Property

    Public zoomfactor As Integer = My.Settings.zoomfactor
    ' Public TCS_HomMat3d As New Object
    Public flgTargetMeasure As Boolean = False
    Public Sub New(ByVal strCamParam As String)
        _campar_path = strCamParam
        'Dim Zeros As Object
        'Zeros = BTuple.TupleGenConst(5, 0)
#If Halcon = "True" Then
        '20150216 Rep By Suuri Sta----新キャリブレーション対応機能
        'HOperatorSet.ReadCamPar(strCamParam, hv_CamparamFirst)
        'HOperatorSet.ReadCamPar(strCamParam, hv_CamparamOut)
        Try
            HOperatorSet.ReadCamPar(strCamParam, hv_CamparamFirst)
            HOperatorSet.ReadCamPar(strCamParam, hv_CamparamOut)
        Catch ex As Exception
            HOperatorSet.ReadTuple(strCamParam, hv_CamparamFirst)
            HOperatorSet.ReadTuple(strCamParam, hv_CamparamOut)
        End Try
        '20150216 Rep By Suuri End----新キャリブレーション対応機能

#End If

        '        HOperatorSet.ChangeRadialDistortionCamPar("fixed", hv_Camparam, Zeros, hv_CamparamOut)
        '        HOperatorSet.GenRadialDistortionMap(Map, hv_Camparam, hv_CamparamOut, "bilinear")
        '#If DEBUG Then
        '        HOperatorSet.TupleMult(hv_CamparamOut, BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat( _
        '      BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat( _
        '      BTuple.TupleConcat(BTuple.TupleConcat(1, 1), 1), 1), 1), 1), 1 / dblImageZoomFactor), 1 / dblImageZoomFactor), dblImageZoomFactor), dblImageZoomFactor), dblImageZoomFactor), _
        '      dblImageZoomFactor), hv_CamparamOut)
        '#End If

        'HOperatorSet.HomMat3dIdentity(TCS_HomMat3d)
        ErrCode = 0
    End Sub
    Public Sub New()

    End Sub
    Public Sub New(ByVal objFBM As FeatureImage)
        Me.hv_CamparamFirst = objFBM.hv_CamparamFirst
        Me.hv_CamparamOut = objFBM.hv_CamparamOut
        Me.hv_CamparamZero = objFBM.hv_CamparamZero
        Me.hv_Height = objFBM.hv_Height
        Me.hv_Width = objFBM.hv_Width
        ' Me.ProjectPath = objFBM.ProjectPath
        Me.pScaleMM = objFBM.pScaleMM
        Me._projectpath = objFBM.ProjectPath
        Me.lstImages = New List(Of ImageSet)
        Dim i As Integer = 0
        For Each ISI As ImageSet In objFBM.lstImages
            If ISI.hx_EdgePS Is Nothing Then
            Else
                Dim newISI As New ImageSet(ISI)
                newISI.hx_EdgePS = ISI.hx_EdgePS.Clone
                newISI.ImageId = i + 1
                For Each objCT As CodedTarget In ISI.Targets.lstCT
                    newISI.Targets.lstCT.Add(objCT)
                Next
                Me.lstImages.Add(newISI)
            End If
            i = i + 1
        Next
        Me.lstCommon3dST = New List(Of Common3DSingleTarget)
        Me.lstCommon3dCT = New List(Of Common3DCodedTarget)
        Me.lstCamParam = objFBM.lstCamParam

    End Sub
    Private Sub ClearOrNew_lstdatas()
        If lstImages Is Nothing Then
            lstImages = New List(Of ImageSet)
        Else
            lstImages.Clear()
        End If
        If lst3dPoints Is Nothing Then
            lst3dPoints = New List(Of MeasurePoint)
        Else
            lst3dPoints.Clear()
        End If
        If lstObjects Is Nothing Then
            lstObjects = New List(Of Objects)
        Else
            lstObjects.Clear()
        End If
        If lstCommon3dST Is Nothing Then
            lstCommon3dST = New List(Of Common3DSingleTarget)
        Else
            lstCommon3dST.Clear()
        End If
        If lstCommon3dCT Is Nothing Then
            lstCommon3dCT = New List(Of Common3DCodedTarget)
        Else
            lstCommon3dCT.Clear()
        End If

    End Sub

    Private Sub ImportImages(ByVal flgNew As Boolean)
        GC.Collect()
        GC.WaitForPendingFinalizers()

        Dim hv_ImageFiles() As String  '20121220画像枚数
        ' list_image_files(_projectpath, "default", "files", hv_ImageFiles) '20121220フォルダ名&画像枚数
        hv_ImageFiles = System.IO.Directory.GetFiles(_projectpath, "*.jpg")

        Dim i As Integer

        Dim n As Integer = hv_ImageFiles.Length - 1 '20121220（画像枚数-1）
        '============================================12.12.21山田
        If n < 0 Then
            ErrCode = 1
            Exit Sub
        End If
        '============================================12.12.21山田
        ClearOrNew_lstdatas()

        ' WritePP_XML()


        Dim strPdataPath As String = _projectpath & "\Pdata\"
        If IO.Directory.Exists(strPdataPath) = False Then
            My.Computer.FileSystem.CreateDirectory(strPdataPath)
        End If
        If IO.File.Exists(strPdataPath & FBM_MDB) = False Then
            IO.File.Copy(My.Application.Info.DirectoryPath & "\" & FBM_MDB, strPdataPath & FBM_MDB, True)
        End If

        'If IO.Directory.Exists(_projectpath & "\Pdata\SmallImage\") = False Then
        '    My.Computer.FileSystem.CreateDirectory(_projectpath & "\Pdata\SmallImage\")
        'End If
        'If IO.Directory.Exists(_projectpath & "\Pdata\TransImage\") = False Then
        '    My.Computer.FileSystem.CreateDirectory(_projectpath & "\Pdata\TransImage\")
        'End If
        'If IO.File.Exists(_projectpath & "\" & YCM_MDB) = False Then
        '    IO.File.Copy(My.Application.Info.DirectoryPath & "\" & "Template\計測データ.mdb", _projectpath & "\" & YCM_MDB, True)
        'End If

        Dim E As New MessageEventArgs
        E.ImageCount = n '20121220（画像枚数-1）

        ' HOperatorSet.OpenFramegrabber("File", -1, -1, -1, -1, -1, -1, "default", -1, "default", -1, "default", _projectpath, "default", -1, -1, IGFlag) '20121220画像枚数が0枚のとき、ここでSTOP

        For i = 0 To n

            Dim ISI As New ImageSet
            ISI.ImageId = i + 1
            ISI.ImageFullPath = hv_ImageFiles(i)
            ISI.ImageName = getFileNamefromFullPath(ISI.ImageFullPath)
            ISI.ImageTransPath = _projectpath & "\Pdata\TransImage\"
            ISI.ImageTransName = ISI.ImageName & "_trans"
            ISI.ImageTransFullPath = ISI.ImageTransPath & ISI.ImageTransName
            ISI.ImageSmallFullPath = _projectpath & "\Pdata\" & ISI.ImageTransName
            If i = 0 Then
                ISI.flgFirst = True
            ElseIf i = n Then
                ISI.flgLast = True
            End If
#If Halcon = "True" Then
            If flgNew = True Then
                GC.Collect()
                GC.WaitForPendingFinalizers()
                Dim hx_Image As HObject = Nothing
                HOperatorSet.GenEmptyObj(hx_Image)
                HOperatorSet.ReadImage(hx_Image, ISI.ImageFullPath)
                ' HOperatorSet.GrabImage(hx_Image, IGFlag)
                '  HOperatorSet.Decompose3(hx_Image, Nothing, hx_Image, Nothing)
                ' HOperatorSet.MapImage(hx_Image, Map, hx_Image)

                'HOperatorSet.ZoomImageFactor(hx_Image, hx_Image, dblImageZoomFactor, dblImageZoomFactor, "constant")
                ' HOperatorSet.WriteImage(hx_Image, "jpg", 0, ISI.ImageTransFullPath)
                Dim Mean As Object = Nothing
                HOperatorSet.Intensity(hx_Image, hx_Image, Mean, Nothing)
                HOperatorSet.ScaleImage(hx_Image, hx_Image, IIf(150 / Mean > 10, New HTuple(10), 150 / Mean), 0)
                HOperatorSet.ZoomImageSize(hx_Image, hx_Image, 96, 64, "constant")
                HOperatorSet.WriteImage(hx_Image, "jpg", 0, ISI.ImageSmallFullPath)
                HOperatorSet.ClearObj(hx_Image)

            End If
#End If
            lstImages.Add(ISI)

            E.ImageIndex = i
            RaiseEvent ImageReaded(Me, E)

        Next
        ' HOperatorSet.CloseFramegrabber(IGFlag)

#If Halcon = "True" Then
        If i >= 2 Then
            HOperatorSet.GetImagePointer1(lstImages.Item(0).hx_Image, Nothing, Nothing, hv_Width, hv_Height)
        End If
#End If

    End Sub

    Public Sub RemoveGreen(ByVal ho_Image1 As HObject, ByVal hv_Row1 As Object, _
     ByVal hv_Col1 As Object, ByRef hv_Row1Out As Object, ByRef hv_Col1Out As Object)



        ' Stack for temporary control variables 
        Dim CTemp(10) As Object
        Dim SP_C As Long
        SP_C = 0

        ' Local iconic variables 
        Dim ho_ImageR As HObject = Nothing
        Dim ho_ImageG As HObject = Nothing, ho_ImageB As HObject = Nothing
        Dim ho_Region1 As HObject = Nothing, ho_ImageSub As HObject = Nothing
        Dim ho_Region2 As HObject = Nothing, ho_RegionComplement As HObject = Nothing
        Dim ho_Contours As HObject = Nothing, ho_ObjectSelected As HObject = Nothing


        ' Local control variables 
        Dim hv_Number As Object = Nothing, hv_Index3 As Object = Nothing
        Dim hv_IsInside As Object = Nothing, hv_Indices4 As Object = Nothing

        ' Initialize local and output iconic variables 
        HOperatorSet.GenEmptyObj(ho_ImageR)
        HOperatorSet.GenEmptyObj(ho_ImageG)
        HOperatorSet.GenEmptyObj(ho_ImageB)
        HOperatorSet.GenEmptyObj(ho_Region1)
        HOperatorSet.GenEmptyObj(ho_ImageSub)
        HOperatorSet.GenEmptyObj(ho_Region2)
        HOperatorSet.GenEmptyObj(ho_RegionComplement)
        HOperatorSet.GenEmptyObj(ho_Contours)
        HOperatorSet.GenEmptyObj(ho_ObjectSelected)

        Try
            hv_Col1Out = hv_Col1
            hv_Row1Out = hv_Row1
            HOperatorSet.ClearObj(ho_ImageR)
            HOperatorSet.ClearObj(ho_ImageG)
            HOperatorSet.ClearObj(ho_ImageB)
            HOperatorSet.Decompose3(ho_Image1, ho_ImageR, ho_ImageG, ho_ImageB)
            HOperatorSet.ClearObj(ho_Region1)
            HOperatorSet.Threshold(ho_ImageR, ho_Region1, 100, 255)
            HOperatorSet.ClearObj(ho_ImageSub)
            HOperatorSet.SubImage(ho_ImageR, ho_ImageG, ho_ImageSub, 1, 5)
            HOperatorSet.ClearObj(ho_Region2)
            HOperatorSet.Threshold(ho_ImageSub, ho_Region2, 1, 255)
            HOperatorSet.ClearObj(ho_RegionComplement)
            HOperatorSet.Complement(ho_Region2, ho_RegionComplement)
            HOperatorSet.ClearObj(ho_Contours)
            HOperatorSet.GenContourRegionXld(ho_RegionComplement, ho_Contours, "border")
            HOperatorSet.CountObj(ho_Contours, hv_Number)
            For hv_Index3 = 1 To hv_Number Step 1
                HOperatorSet.ClearObj(ho_ObjectSelected)
                HOperatorSet.SelectObj(ho_Contours, ho_ObjectSelected, hv_Index3)
                HOperatorSet.TestXldPoint(ho_ObjectSelected, hv_Row1Out, hv_Col1Out, hv_IsInside)
                HOperatorSet.TupleFind(hv_IsInside, 1, hv_Indices4)
                'Row1Out := subset(Row1Out,Indices4)
                'Col1Out := subset(Col1Out,Indices4)
                CTemp(SP_C) = hv_Row1Out
                SP_C = SP_C + 1
                HOperatorSet.TupleRemove(CTemp(SP_C - 1), hv_Indices4, hv_Row1Out)
                SP_C = 0
                CTemp(SP_C) = hv_Col1Out
                SP_C = SP_C + 1
                HOperatorSet.TupleRemove(CTemp(SP_C - 1), hv_Indices4, hv_Col1Out)
                SP_C = 0
            Next
            HOperatorSet.ClearObj(ho_ImageR)
            HOperatorSet.ClearObj(ho_ImageG)
            HOperatorSet.ClearObj(ho_ImageB)
            HOperatorSet.ClearObj(ho_Region1)
            HOperatorSet.ClearObj(ho_ImageSub)
            HOperatorSet.ClearObj(ho_Region2)
            HOperatorSet.ClearObj(ho_RegionComplement)
            HOperatorSet.ClearObj(ho_Contours)
            HOperatorSet.ClearObj(ho_ObjectSelected)

            Exit Sub
        Catch HDevExpDefaultException As COMException
            HOperatorSet.ClearObj(ho_ImageR)
            HOperatorSet.ClearObj(ho_ImageG)
            HOperatorSet.ClearObj(ho_ImageB)
            HOperatorSet.ClearObj(ho_Region1)
            HOperatorSet.ClearObj(ho_ImageSub)
            HOperatorSet.ClearObj(ho_Region2)
            HOperatorSet.ClearObj(ho_RegionComplement)
            HOperatorSet.ClearObj(ho_Contours)
            HOperatorSet.ClearObj(ho_ObjectSelected)

            Throw HDevExpDefaultException
        End Try
    End Sub
    Public Sub SaveProjectData()
        Dim i As Integer
        If ConnectDbFBM(_projectpath & strSavePath) = True Then
            dbClass.BeginTrans()
            Try
                dbClass.DoDelete("Targets")
                dbClass.DoDelete("CameraPose")
                For i = 0 To lstImages.Count - 1
                    lstImages.Item(i).SaveImageSet(_projectpath & strSavePath)
                Next
                dbClass.CommitTrans()
            Catch ex As Exception
                dbClass.RollbackTrans()
            End Try

            SaveTupleObj(hv_CamparamFirst, _projectpath & strSavePath & "hv_Camparam.tpl")
            SaveTupleObj(hv_CamparamOut, _projectpath & strSavePath & "hv_CamparamOut.tpl")
            '  SaveTupleObj(ScaleMM, _projectpath & strSavePath & "ScaleMM.tpl")
            '  SaveTupleObj(TCS_HomMat3d, _projectpath & strSavePath & "TCS_HomMat3d.tpl")

            Dim objSaveTuple As New HTuple
            'ExtendVar(objSaveTuple, 2)
            objSaveTuple = objSaveTuple.TupleInsert(0, lst3dPoints.Count)
            objSaveTuple = objSaveTuple.TupleInsert(1, XorY)
            objSaveTuple = objSaveTuple.TupleInsert(2, CInt(IIf(flgTargetMeasure, 1, 0)))

            SaveTupleObj(objSaveTuple, _projectpath & strSavePath & "3dPointsCount.tpl")

            'For i = 0 To lst3dPoints.Count - 1
            '    lst3dPoints.Item(i).SaveMeasurePoint(_projectpath & strSavePath, i + 1)
            'Next
            'WorldXYZ.Save3dPoints(_projectpath & strSavePath & "WorldXYZ")

            'MP_SaveToDB()
            'Obj_SaveToDB()
            AccessDisConnect()
        End If
    End Sub
    Public Sub ReadProjectData()
        Dim i As Integer
        Dim n As Integer
        If ConnectDbFBM(_projectpath & "\Pdata\") = True Then

            For i = 0 To lstImages.Count - 1
                lstImages.Item(i).ReadImageSet(_projectpath & strSavePath)
            Next

            'For i = 1 To lstImages.Count - 2
            '    Dim IS1 As ImageSet = lstImages.Item(i - 1)
            '    Dim IS2 As ImageSet = lstImages.Item(i)
            '    Dim IS3 As ImageSet = lstImages.Item(i + 1)
            '    GetMidPointsOf3Image1(IS1, IS2, IS3)

            'Next
            ReadTupleObj(hv_CamparamFirst, _projectpath & strSavePath & "hv_Camparam.tpl")
            ReadTupleObj(hv_CamparamOut, _projectpath & strSavePath & "hv_CamparamOut.tpl")
            'ReadTupleObj(ScaleMM, _projectpath & strSavePath & "ScaleMM.tpl")
            'ReadTupleObj(TCS_HomMat3d, _projectpath & strSavePath & "TCS_HomMat3d.tpl")
            For i = 0 To lstImages.Count - 1
                lstImages.Item(i).objCamparam = New CameraParam
                lstImages.Item(i).objCamparam.Camparam = hv_CamparamOut
                lstImages.Item(i).objCamparam.CamParamFirst = hv_CamparamFirst
            Next
            'GenCommon3Dpoint()
            CreateCommon3Dpoint()

            Dim objReadTuple As Object = Nothing
            ReadTupleObj(objReadTuple, _projectpath & strSavePath & "3dPointsCount.tpl")
            If objReadTuple Is Nothing Then
                Exit Sub
            End If
            ' n = CInt(BTuple.TupleSelect(objReadTuple, 0))
            'If lst3dPoints Is Nothing Then
            '    lst3dPoints = New List(Of MeasurePoint)
            'Else
            '    lst3dPoints.Clear()
            'End If
            'For i = 1 To n
            '    Dim MP As New MeasurePoint
            '    MP.ReadMeasurePoint(_projectpath & strSavePath, i, lstImages)
            '    lst3dPoints.Add(MP)
            'Next
            XorY = objReadTuple(1).I
            'If BTuple.TupleLength(objReadTuple) < 3 Then
            If lstCommon3dCT.Count + lstCommon3dST.Count > 0 Then
                flgTargetMeasure = True
            Else
                flgTargetMeasure = False
            End If
            'Else
            '    flgTargetMeasure = CBool(BTuple.TupleSelect(objReadTuple, 2))
            'End If

            'Obj_ReadFromDB()
            AccessDisConnect()
        End If
        ' WorldXYZ.Read3dPoints(_projectpath & strSavePath & "WorldXYZ")

    End Sub

    Private Sub CreateCommon3Dpoint()
        If lstCommon3dST Is Nothing Then
            lstCommon3dST = New List(Of Common3DSingleTarget)
        Else
            lstCommon3dST.Clear()
        End If
        If lstCommon3dCT Is Nothing Then
            lstCommon3dCT = New List(Of Common3DCodedTarget)
        Else
            lstCommon3dCT.Clear()
        End If
        For Each ISI As ImageSet In lstImages
            For Each ST As SingleTarget In ISI.Targets.lstST
                If ST.P3ID <> -1 Then
                    Dim flgSTari As Boolean = False
                    For Each C3DST As Common3DSingleTarget In lstCommon3dST
                        If ST.P3ID = C3DST.PID Then
                            flgSTari = True
                            C3DST.lstST.Add(ST)
                            Exit For
                        End If
                    Next
                    If flgSTari = False Then
                        Dim C3DST As New Common3DSingleTarget
                        C3DST.PID = ST.P3ID
                        C3DST.lstST.Add(ST)
                        lstCommon3dST.Add(C3DST)

                    End If
                End If
            Next
            For Each CT As CodedTarget In ISI.Targets.lstCT
                'If CT.flgUsable = True Then
                Dim flgCTari As Boolean = False
                For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
                    If CT.CT_ID = C3DCT.PID Then
                        flgCTari = True
                        C3DCT.lstCT.Add(CT)
                        CT.myC3DCT = C3DCT '20160505 ADD SUSANO
                        Exit For
                    End If
                Next
                If flgCTari = False Then
                    Dim C3DCT As New Common3DCodedTarget
                    C3DCT.PID = CT.CT_ID
                    C3DCT.lstCT.Add(CT)
                    CT.myC3DCT = C3DCT '20160505 ADD SUSANO
                    lstCommon3dCT.Add(C3DCT)
                End If
                'End If
            Next
        Next
    End Sub

    Public Sub GenCommon3Dpoint()
        CreateCommon3Dpoint()

        CalcAllImages_Ray()
        For Each C3DST As Common3DSingleTarget In lstCommon3dST
            If C3DST.PID = 678 Then
                Dim t As Integer = 1

            End If
            C3DST.Calc3dPoints()
        Next
        For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
            C3DCT.Calc3dPoints()
            If C3DCT.PID = 258 Then
                C3DCT.PID = C3DCT.PID
            End If
        Next
    End Sub

    Public Sub GetFeaturePoints()
        For Each ISI As ImageSet In lstImages
            Dim hx_Cross As New HObject
            ' HOperatorSet.PointsHarris(ISI.hx_Image, 3, 3, 0.02, 10000, ISI.FeaturePoints.Row, ISI.FeaturePoints.Col)
            HOperatorSet.PointsFoerstner(ISI.hx_Image, 1, 2, 3, 200, 0.1, "gauss", "false", ISI.FeaturePoints.Row, ISI.FeaturePoints.Col, _
            Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            HOperatorSet.ClearObj(ISI.hx_FeatureCross)
            HOperatorSet.GenCrossContourXld(ISI.hx_FeatureCross, ISI.FeaturePoints.Row, ISI.FeaturePoints.Col, CrossSize, CrossAngle)
        Next
    End Sub

    Public Sub RansacMatching()
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim flgEnd As Boolean = True
        Dim n As Integer = lstImages.Count
        Dim homMat2d As Object = Nothing
        For Each IS1 As ImageSet In lstImages
            Dim IS2 As ImageSet
            i += 1
            If i = n Then
                Exit For
            End If
            IS2 = lstImages.Item(i)

            'flgEnd = False
            'While flgEnd = False
            '    For j = 0 To 10
            '        MRS.RowTolerance += 5
            '        MRS.ColTolerance += 5
            '        HOperatorSet.MatchRelPoseRansac(IS1.hx_Image, IS2.hx_Image, IS1.FeaturePoints.Row, IS1.FeaturePoints.Col, _
            '                           IS2.FeaturePoints.Row, IS2.FeaturePoints.Col, hv_CamparamOut, hv_CamparamOut, _
            '                            MRS.GrayMatchMethod, MRS.MaskSize, MRS.RowMove, _
            '                            MRS.ColMove, MRS.RowTolerance, MRS.ColTolerance, _
            '                            MRS.Rotation, MRS.MatchThreshold, MRS.EstimationMethod, _
            '                            MRS.DistanceThreshold, MRS.RandSeed, IS2.ImagePose.RelPose, _
            '                            IS2.ImagePose.CovRelPose, IS2.ImagePose.hError, IS1.RansacFirst.RansacPointsIndex, _
            '                            IS2.RansacSecond.RansacPointsIndex)

            '        If CalcRelPoseAndXYZ(IS1, IS2) = 2 Then
            '            flgEnd = False
            '        Else
            '            flgEnd = True
            '            Exit For
            '        End If
            '    Next
            'End While
            'HOperatorSet.MatchRelPoseRansac(IS1.hx_Image, IS2.hx_Image, IS1.FeaturePoints.Row, IS1.FeaturePoints.Col, _
            '                          IS2.FeaturePoints.Row, IS2.FeaturePoints.Col, hv_CamparamOut, hv_CamparamOut, _
            '                           MRS.GrayMatchMethod, MRS.MaskSize, MRS.RowMove, _
            '                           MRS.ColMove, MRS.RowTolerance, MRS.ColTolerance, _
            '                           MRS.Rotation, MRS.MatchThreshold, MRS.EstimationMethod, _
            '                           MRS.DistanceThreshold, MRS.RandSeed, IS2.ImagePose.RelPose, _
            '                           IS2.ImagePose.CovRelPose, IS2.ImagePose.hError, IS1.RansacFirst.RansacPointsIndex, _
            '                           IS2.RansacSecond.RansacPointsIndex)
            ''HOperatorSet.MatchFundamentalMatrixRansac(IS1.hx_Image, IS2.hx_Image, IS1.FeaturePoints.Row, IS1.FeaturePoints.Col, _
            ''                         IS2.FeaturePoints.Row, IS2.FeaturePoints.Col, _
            ''                          MRS.GrayMatchMethod, MRS.MaskSize, MRS.RowMove, _
            ''                          MRS.ColMove, MRS.RowTolerance, MRS.ColTolerance, _
            ''                          MRS.Rotation, MRS.MatchThreshold, MRS.EstimationMethod, _
            ''                          MRS.DistanceThreshold, MRS.RandSeed, Nothing, _
            ''                          Nothing, IS2.ImagePose.hError, IS1.RansacFirst.RansacPointsIndex, _
            ''                          IS2.RansacSecond.RansacPointsIndex)



            'CalcRelPoseAndXYZ(IS1, IS2)

            RunMatchRansac(IS1, IS2)

            ' ReMatchRansac(IS1, IS2, 3)

            'HOperatorSet.VectorToProjHomMat2d(IS1.RansacFirst.RansacPoints.Row, IS1.RansacFirst.RansacPoints.Col, _
            '                        IS2.RansacSecond.RansacPoints.Row, IS2.RansacSecond.RansacPoints.Col, _
            '                        "gold_standard", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, homMat2d, Nothing)
            'HOperatorSet.ProjMatchPointsRansacGuided(IS1.hx_Image, IS2.hx_Image, IS1.FeaturePoints.Row, IS1.FeaturePoints.Col, _
            '                          IS2.FeaturePoints.Row, IS2.FeaturePoints.Col, "ncc", 10, homMat2d, 20, 0.5, "gold_standard", _
            '                          2, 1, homMat2d, IS1.RansacFirst.RansacPointsIndex, _
            '                           IS2.RansacSecond.RansacPointsIndex)

            'CalcRelPoseAndXYZ(IS1, IS2)

            'ReCalcRansacMatch(IS1, IS2)



        Next
        OptimalMatching()
        Connect()
    End Sub

    Public Sub RansacMatching1()
        Dim i As Integer = 0
        Dim n As Integer = lstImages.Count

        For i = 1 To n - 2
            Dim IS1 As ImageSet = lstImages.Item(i - 1)
            Dim IS2 As ImageSet = lstImages.Item(i)
            Dim IS3 As ImageSet = lstImages.Item(i + 1)
            Dim Scale As Double
            Dim BadIndex As Object = Nothing
            Dim tmpIndex1 As Object = Nothing
            Dim tmpIndex2 As Object = Nothing
            Dim tmpIndex3 As Object = Nothing
            Dim tmpIndex4 As Object = Nothing
            Dim cnt As Integer = 0
            Dim limitCnt As Integer = 6
            If i = 3 Then
                Dim t As Integer = 0
            End If
            ' MRS = New MatchRansac(1)

            RunMatchRansac(IS1, IS2)
            RunMatchRansac(IS2, IS3)
            Dim flgEnd As Boolean = False
            Dim flgCon As Boolean = False
            Dim cntMid As Integer = 0
            While flgEnd = False
                cntMid = GetMidPointsOf3Image1(IS1, IS2, IS3)
                If cntMid >= limitCnt Then
                    cnt += 1

                    tmpIndex1 = IS1.RansacFirst.RansacPointsIndex
                    tmpIndex2 = IS2.RansacSecond.RansacPointsIndex
                    tmpIndex3 = IS2.RansacFirst.RansacPointsIndex
                    tmpIndex4 = IS3.RansacSecond.RansacPointsIndex

                    ReMatchRansac(IS1, IS2, 1)

                    ReMatchRansac(IS2, IS3, 1)

                    cntMid = GetMidPointsOf3Image1(IS1, IS2, IS3)
                    If cntMid >= limitCnt Then

                        CalcRelPoseByMidPose(IS1, IS2)
                        CalcRelPoseByMidPose(IS2, IS3)
                        GetScaleFromMidPoints1(IS2, IS3, Scale, BadIndex)
                        IS3.Scale = Scale
                        If cntMid - BTuple.TupleLength(BadIndex) >= limitCnt Then
                            RemoveBadPoints(IS1, IS2, IS3, BadIndex)
                        End If
                        tmpIndex1 = IS1.RansacFirst.RansacPointsIndex
                        tmpIndex2 = IS2.RansacSecond.RansacPointsIndex
                        tmpIndex3 = IS2.RansacFirst.RansacPointsIndex
                        tmpIndex4 = IS3.RansacSecond.RansacPointsIndex
                    End If
                    If cnt = 5 Then
                        flgEnd = True
                        flgCon = False
                    End If
                Else
                    flgEnd = True
                    IS1.RansacFirst.RansacPointsIndex = tmpIndex1
                    IS2.RansacSecond.RansacPointsIndex = tmpIndex2

                    IS2.RansacFirst.RansacPointsIndex = tmpIndex3
                    IS3.RansacSecond.RansacPointsIndex = tmpIndex4
                    flgCon = True
                End If

            End While

            If flgCon = True Then
                If GetMidPointsOf3Image1(IS1, IS2, IS3) >= limitCnt Then
                    CalcRelPoseByMidPose(IS1, IS2)
                    CalcRelPoseByMidPose(IS2, IS3)
                    GetScaleFromMidPoints1(IS2, IS3, Scale, BadIndex)
                    cntMid = BTuple.TupleLength(BadIndex)
                    IS3.Scale = Scale
                    If cntMid >= limitCnt Then
                        RemoveBadPoints(IS1, IS2, IS3, BadIndex)
                    End If
                End If
            End If

        Next
        Connect()
    End Sub


    Public Sub RansacMatching2()
        Dim i As Integer = 0
        Dim n As Integer = lstImages.Count

        For i = 1 To n - 2
            Dim IS1 As ImageSet = lstImages.Item(i - 1)
            Dim IS2 As ImageSet = lstImages.Item(i)
            Dim IS3 As ImageSet = lstImages.Item(i + 1)
            Dim Scale As Double
            Dim BadIndex As New Object
            Dim tmpIndex1 As New Object
            Dim tmpIndex2 As New Object
            Dim tmpIndex3 As New Object
            Dim tmpIndex4 As New Object
            Dim cnt As Integer = 0
            Dim limitCnt As Integer = 6
            Dim limitCnt1 As Integer = 0
            If i = 3 Then
                Dim t As Integer = 0
            End If

            Dim flgEnd As Boolean = False
            Dim flgCon As Boolean = False
            Dim cntMid As Integer = 0
            'While flgEnd = False
            '    cntMid = GetMidPointsOf3Image1(IS1, IS2, IS3)
            '    If cnt = 0 Then
            '        limitCnt1 = CInt(cntMid * 0.9)
            '    End If
            '    If cntMid >= limitCnt1 Then
            '        cnt += 1

            '        tmpIndex1 = IS1.RansacFirst.RansacPointsIndex
            '        tmpIndex2 = IS2.RansacSecond.RansacPointsIndex
            '        tmpIndex3 = IS2.RansacFirst.RansacPointsIndex
            '        tmpIndex4 = IS3.RansacSecond.RansacPointsIndex

            '        ReMatchRansac(IS1, IS2, 1)

            '        ReMatchRansac(IS2, IS3, 1)

            '        cntMid = GetMidPointsOf3Image1(IS1, IS2, IS3)
            '        If cntMid >= limitCnt1 Then

            '            '    'CalcRelPoseByMidPose(IS1, IS2)
            '            '    'CalcRelPoseByMidPose(IS2, IS3)
            '            '    'GetScaleFromMidPoints1(IS2, IS3, Scale, BadIndex)
            '            '    'IS3.Scale = Scale
            '            '    'If cntMid - BTuple.TupleLength(BadIndex) >= limitCnt Then
            '            '    '    RemoveBadPoints(IS1, IS2, IS3, BadIndex)
            '            '    'End If
            '            tmpIndex1 = IS1.RansacFirst.RansacPointsIndex
            '            tmpIndex2 = IS2.RansacSecond.RansacPointsIndex
            '            tmpIndex3 = IS2.RansacFirst.RansacPointsIndex
            '            tmpIndex4 = IS3.RansacSecond.RansacPointsIndex
            '        End If
            '        If cnt = 10 Then
            '            flgEnd = True
            '            flgCon = False
            '        End If
            '    Else
            '        flgEnd = True
            '        IS1.RansacFirst.RansacPointsIndex = tmpIndex1
            '        IS2.RansacSecond.RansacPointsIndex = tmpIndex2

            '        IS2.RansacFirst.RansacPointsIndex = tmpIndex3
            '        IS3.RansacSecond.RansacPointsIndex = tmpIndex4
            '        flgCon = True
            '    End If

            'End While

            IS1.RansacFirst = New RansacPoints
            IS2.RansacSecond = New RansacPoints
            IS2.RansacFirst = New RansacPoints
            IS3.RansacSecond = New RansacPoints

            flgEnd = False
            While flgEnd = False
                cntMid = GetMidPointsOf3Image1(IS1, IS2, IS3)
                If cntMid > 0 Then

                    CalcRelPoseByMidPose(IS1, IS2)

                    'CalcMidXYZ_byRelPose(IS1, IS2, IS2.ImagePose.RelPose, Nothing)
                    'CalcMidXYZ_byRelPose(IS2, IS3, IS3.ImagePose.RelPose, Nothing)

                    'CalcRelPoseByMidPose(IS2, IS3)
                    'GetScaleFromMidPoints1(IS2, IS3, Scale, BadIndex)
                    GetScaleFromMidPoints3(IS1, IS2, IS3, Scale, BadIndex)
                    IS3.Scale = Scale
                    If cntMid - BTuple.TupleLength(BadIndex) >= limitCnt Then
                        RemoveBadPoints(IS1, IS2, IS3, BadIndex)
                    Else
                        flgEnd = True
                        IS3.Scale = Scale
                    End If
                    If BTuple.TupleLength(BadIndex) = 0 Then
                        flgEnd = True
                        IS3.Scale = Scale

                    End If
                Else
                    ' MRS.RandSeed = 1
                    RunMatchRansac(IS1, IS2)
                    RunMatchRansac(IS2, IS3)


                End If

            End While

            'If flgCon = True Then
            '    If GetMidPointsOf3Image1(IS1, IS2, IS3) >= limitCnt Then
            '        CalcRelPoseByMidPose(IS1, IS2)
            '        CalcRelPoseByMidPose(IS2, IS3)
            '        GetScaleFromMidPoints1(IS2, IS3, Scale, BadIndex)
            '        cntMid = BTuple.TupleLength(BadIndex)
            '        IS3.Scale = Scale
            '        If cntMid >= limitCnt Then
            '            RemoveBadPoints(IS1, IS2, IS3, BadIndex)
            '        End If
            '    End If
            'End If

        Next
        ' Connect1()
    End Sub


    Public Sub RansacMatching3()
        Dim i As Integer = 0
        Dim n As Integer = lstImages.Count

        Dim FirstPose1 As New Object
        HOperatorSet.CreatePose(0, 0, 0, 0, 0, 0, "Rp+T", "gba", "point", FirstPose1)
        lstImages.Item(0).ImagePose.Pose = FirstPose1
        '  lstImages.Item(1).ImagePose.Pose = lstImages.Item(1).MidPose.RelPose

        WorldXYZ = New Point3D
        Dim ConnectHomMat3d As New Object
        Dim TempHomMat3d As New Object
        HOperatorSet.HomMat3dIdentity(ConnectHomMat3d)
        Dim commonscale As Double = 1
        Dim NextPose As New Object


        For i = 1 To n - 2
            Dim IS1 As ImageSet = lstImages.Item(i - 1)
            Dim IS2 As ImageSet = lstImages.Item(i)
            Dim IS3 As ImageSet = lstImages.Item(i + 1)
            Dim Scale As Double
            Dim BadIndex As New Object
            Dim cnt As Integer = 0
            Dim limitCnt As Integer = 6
            Dim limitCnt1 As Integer = 0
            If i = 3 Then
                Dim t As Integer = 0
            End If

            Dim flgEnd As Boolean = False
            Dim flgCon As Boolean = False
            Dim cntMid As Integer = 0

            IS1.RansacFirst = New RansacPoints
            IS2.RansacSecond = New RansacPoints
            IS2.RansacFirst = New RansacPoints
            IS3.RansacSecond = New RansacPoints

            flgEnd = False
            While flgEnd = False
                cntMid = GetMidPointsOf3Image1(IS1, IS2, IS3)
                If cntMid > 0 Then

                    CalcRelPoseByMidPose(IS1, IS2)
                    ' CalcRelPoseByMidPose(IS1, IS3)
                    CalcRelPoseByMidPose(IS2, IS3)
                    GetScaleFromMidPoints1(IS2, IS3, Scale, BadIndex)
                    IS3.Scale = Scale
                    If cntMid - BTuple.TupleLength(BadIndex) >= limitCnt Then
                        RemoveBadPoints(IS1, IS2, IS3, BadIndex)
                    Else
                        flgEnd = True
                        IS3.Scale = Scale
                    End If
                    If BTuple.TupleLength(BadIndex) = 0 Then
                        flgEnd = True
                        IS3.Scale = Scale

                    End If
                Else
                    ' MRS.RandSeed = 1
                    RunMatchRansac(IS1, IS2)
                    RunMatchRansac(IS2, IS3)


                End If
                If flgEnd = True Then
                    Dim homMat3d As New Object
                    Dim tmpWPoint As New Point3D
                    Dim testMidPose As New Point3D
                    Dim DiffPose As New CameraPose
                    Dim AbsPose As New CameraPose
                    Dim Result1 As New Object
                    Dim Result2 As New Object

                    ' GetMidPoints(IS1, IS2)
                    GetMidPointsOf3Image1(IS1, IS2, IS3)
                    CalcRelPoseByMidPose(IS1, IS2)
                    CalcRelPoseByMidPose(IS2, IS3)

                    If i = 1 Then
                        CalcRelPoseAndXYZ(IS1, IS2)
                        'IS2.ImagePose.Pose = IS2.MidPose.RelPose
                        IS2.ImagePose.Pose = IS2.ImagePose.RelPose
                        'IS2.ImagePose.RelPose = IS2.MidPose.RelPose
                        NextPose = IS2.ImagePose.RelPose
                        IS2.CommonHomMat3d = ConnectHomMat3d
                        IS2.CommonScale = 1
                        WorldXYZ.X = BTuple.TupleConcat(WorldXYZ.X, IS2.MidPose.X)
                        WorldXYZ.Y = BTuple.TupleConcat(WorldXYZ.Y, IS2.MidPose.Y)
                        WorldXYZ.Z = BTuple.TupleConcat(WorldXYZ.Z, IS2.MidPose.Z)
                    End If

                    CalcRelPoseAndXYZ(IS2, IS3)

                    CalcMidXYZ_byRelPose(IS1, IS2, IS2.MidPose.RelPose, Nothing)
                    CalcMidXYZ_byRelPose(IS2, IS3, IS3.MidPose.RelPose, Nothing)

                    '  GetScaleFromMidPoints1(IS2, IS3, Scale, BadIndex)
                    'IS2.Scale = Scale

                    IS2.MidPose.SetScale(commonscale)

                    commonscale = commonscale * IS3.Scale '1.03549
                    IS2.ImagePose.Pose = NextPose
                    CalcConnectedPose(IS2, IS3, commonscale)
                    NextPose = IS3.ImagePose.Pose

                    '  CalcMidXYZ_byRelPose(IS2, IS3, IS3.MidPose.RelPose)
                    IS3.MidPose.SetScale(commonscale)

                    hom_mat_3d_from_3d_3d_point_correspondence(IS3.MidPose.X, IS3.MidPose.Y, IS3.MidPose.Z, IS2.MidPose.X, IS2.MidPose.Y, IS2.MidPose.Z, homMat3d)
                    HOperatorSet.AffineTransPoint3d(homMat3d, IS3.MidPose.X, IS3.MidPose.Y, IS3.MidPose.Z, testMidPose.X, testMidPose.Y, testMidPose.Z)
                    tmpWPoint = IS2.MidPose.ConvertToPoint3d
                    tmpWPoint.GetDisttoOtherPose(testMidPose, Result1)

                    'HOperatorSet.PoseToHomMat3d(IS1.ImagePose.Pose, ConnectHomMat3d)
                    ' HOperatorSet.HomMat3dToPose(ConnectHomMat3d, IS1.ImagePose.Pose)
                    HOperatorSet.HomMat3dCompose(ConnectHomMat3d, homMat3d, TempHomMat3d)
                    ConnectHomMat3d = TempHomMat3d
                    ' HOperatorSet.HomMat3dToPose(ConnectHomMat3d, IS2.ImagePose.Pose)
                    HOperatorSet.AffineTransPoint3d(ConnectHomMat3d, IS3.MidPose.X, IS3.MidPose.Y, IS3.MidPose.Z, testMidPose.X, testMidPose.Y, testMidPose.Z)
                    Result2 = BTuple.TupleLength(WorldXYZ.X)
                    Result2 = BTuple.TupleLength(WorldXYZ.Y)
                    Result2 = BTuple.TupleLength(WorldXYZ.Z)

                    WorldXYZ.X = BTuple.TupleConcat(WorldXYZ.X, testMidPose.X)
                    WorldXYZ.Y = BTuple.TupleConcat(WorldXYZ.Y, testMidPose.Y)
                    WorldXYZ.Z = BTuple.TupleConcat(WorldXYZ.Z, testMidPose.Z)

                    IS3.ImagePose.SetScale(commonscale)
                    HOperatorSet.AffineTransPoint3d(ConnectHomMat3d, IS3.ImagePose.X, IS3.ImagePose.Y, IS3.ImagePose.Z, tmpWPoint.X, tmpWPoint.Y, tmpWPoint.Z)
                    IS3.ImagePose.X = tmpWPoint.X
                    IS3.ImagePose.Y = tmpWPoint.Y
                    IS3.ImagePose.Z = tmpWPoint.Z

                    IS3.CommonHomMat3d = ConnectHomMat3d
                    IS3.CommonScale = commonscale
                End If
            End While

        Next
        ' Connect1()
    End Sub

    Public Sub RansacMatchOnly()
        Dim i As Integer = 0
        Dim n As Integer = lstImages.Count
        For i = 1 To n - 1
            Dim IS1 As ImageSet = lstImages.Item(i - 1)
            Dim IS2 As ImageSet = lstImages.Item(i)
            RunMatchRansac(IS1, IS2, True, "\MatchRansac\")
        Next i

    End Sub

    'Public Sub RansacMatchOnlyBack(ByVal N As Integer)
    '    Dim i As Integer
    '    For i = 1 To N - 1
    '        Dim IS1 As ImageSet = lstImages.Item(i - 1)
    '        Dim IS2 As ImageSet = lstImages.Item(i)
    '        RunMatchRansacBack(IS1, IS2, True, "\MatchRansacBack\")
    '    Next i
    'End Sub

    Public Sub RansacMatching4()
        Dim i As Integer = 0
        Dim n As Integer = lstImages.Count

        Dim FirstPose1 As New Object
        HOperatorSet.CreatePose(0, 0, 0, 0, 0, 0, "Rp+T", "gba", "point", FirstPose1)
        lstImages.Item(0).ImagePose.Pose = FirstPose1
        '  lstImages.Item(1).ImagePose.Pose = lstImages.Item(1).MidPose.RelPose

        WorldXYZ = New Point3D
        Dim ConnectHomMat3d As New Object
        Dim TempHomMat3d As New Object
        HOperatorSet.HomMat3dIdentity(ConnectHomMat3d)
        Dim commonscale As Double = 1
        Dim NextPose As New Object
        '   Dim HomMatList As New List(Of Object)
        For i = 1 To n - 2
            Dim IS1 As ImageSet = lstImages.Item(i - 1)
            Dim IS2 As ImageSet = lstImages.Item(i)
            Dim IS3 As ImageSet = lstImages.Item(i + 1)
            Dim Scale As Double
            Dim BadIndex As New Object
            Dim cnt As Integer = 0
            Dim limitCnt As Integer = 6
            Dim limitCnt1 As Integer = 0
            If i = 4 Then
                Dim t As Integer = 0
            End If

            Dim flgEnd As Boolean = False
            Dim flgCon As Boolean = False
            Dim cntMid As Integer = 0

            IS1.RansacFirst = New RansacPoints
            IS2.RansacSecond = New RansacPoints
            IS2.RansacFirst = New RansacPoints
            IS3.RansacSecond = New RansacPoints
            IS1.RansacFirst.RansacPointsIndex = IS1.RansacFirstIndexBack
            IS2.RansacSecond.RansacPointsIndex = IS2.RansacSecondIndexBack
            IS2.RansacFirst.RansacPointsIndex = IS2.RansacFirstIndexBack
            IS3.RansacSecond.RansacPointsIndex = IS3.RansacSecondIndexBack
            Try
                If BTuple.TupleLength(IS3.RansacSecond.RansacPointsIndex) = 0 Then

                End If
            Catch ex As Exception
                IS3.RansacSecond.RansacPointsIndex = Nothing
            End Try
            flgEnd = False
            While flgEnd = False
                cntMid = GetMidPointsOf3Image1(IS1, IS2, IS3)
                If cntMid > 0 Then
                    If cntMid < 150 Then
                        cntMid = cntMid
                    End If
                    'CalcRelPoseByMidPose(IS1, IS2)
                    'CalcRelPoseByMidPose(IS2, IS3)

                    GetScaleFromMidPoints5(IS1, IS2, IS3, Scale, BadIndex)
                    IS3.Scale = Scale
                    If cntMid - BTuple.TupleLength(BadIndex) >= limitCnt Then
                        RemoveBadPoints(IS1, IS2, IS3, BadIndex)
                    Else
                        flgEnd = True
                        IS3.Scale = Scale
                    End If
                    If BTuple.TupleLength(BadIndex) = 0 Then
                        flgEnd = True
                        IS3.Scale = Scale

                    End If
                Else
                    ' MRS.RandSeed = 1
                    If BTuple.TupleLength(IS3.RansacSecond.RansacPointsIndex) = 0 Then
                        If i = 1 Then
                            RunMatchRansac(IS1, IS2, True, "\MatchRansac\")
                        Else
                            RunMatchRansac(IS1, IS2, False, "\MatchRansac\")
                        End If
                        RunMatchRansac(IS2, IS3, True, "\MatchRansac\")
                    End If
                End If
                If flgEnd = True Then
                    Dim homMat3d As New Object
                    Dim tmpWPoint As New Point3D
                    Dim testMidPose As New Point3D
                    Dim DiffPose As New CameraPose
                    Dim AbsPose As New CameraPose
                    Dim Result1 As New Object
                    Dim Result2 As New Object

                    ' GetMidPoints(IS1, IS2)
                    GetMidPointsOf3Image1(IS1, IS2, IS3)
                    'CalcRelPoseByMidPose(IS1, IS2)
                    'CalcRelPoseByMidPose(IS2, IS3)

                    If i = 1 Then
                        ' CalcRelPoseAndXYZ(IS1, IS2)
                        'IS2.ImagePose.Pose = IS2.MidPose.RelPose
                        IS2.ImagePose.Pose = IS2.VectorPose.Pose
                        NextPose = IS2.VectorPose.Pose
                        HOperatorSet.HomMat3dIdentity(ConnectHomMat3d)
                        IS2.CommonHomMat3d = ConnectHomMat3d
                        IS2.CommonScale = 1
                        CalcMidXYZ_byRelPose(IS1, IS2, IS2.VectorPose.Pose, Nothing)
                        WorldXYZ.X = BTuple.TupleConcat(WorldXYZ.X, IS2.MidPose.X)
                        WorldXYZ.Y = BTuple.TupleConcat(WorldXYZ.Y, IS2.MidPose.Y)
                        WorldXYZ.Z = BTuple.TupleConcat(WorldXYZ.Z, IS2.MidPose.Z)
                    End If

                    'CalcRelPoseAndXYZ(IS2, IS3)

                    CalcMidXYZ_byRelPose(IS1, IS2, IS2.VectorPose.Pose, Nothing)
                    CalcMidXYZ_byRelPose(IS2, IS3, IS2.VectorPose.RelPose, Nothing)

                    '  GetScaleFromMidPoints1(IS2, IS3, Scale, BadIndex)
                    'IS2.Scale = Scale

                    IS2.MidPose.SetScale(commonscale)
                    commonscale = commonscale * IS3.Scale '1.03549

                    IS3.MidPose.RelPose = IS2.VectorPose.RelPose

                    IS2.ImagePose.Pose = NextPose
                    CalcConnectedPose(IS2, IS3, commonscale)
                    NextPose = IS3.ImagePose.Pose

                    '  CalcMidXYZ_byRelPose(IS2, IS3, IS3.MidPose.RelPose)
                    IS3.MidPose.SetScale(commonscale)

                    'HOperatorSet.PoseToHomMat3d(IS2.ImagePose.Pose, homMat3d)
                    'HOperatorSet.PoseToHomMat3d(IS3.ImagePose.Pose, TempHomMat3d)
                    'HOperatorSet.AffineTransPoint3D(homMat3d, IS2.MidPose.X, IS2.MidPose.Y, IS2.MidPose.Z, tmpWPoint.X, tmpWPoint.Y, tmpWPoint.Z)
                    'HOperatorSet.AffineTransPoint3D(TempHomMat3d, IS3.MidPose.X, IS3.MidPose.Y, IS3.MidPose.Z, testMidPose.X, testMidPose.Y, testMidPose.Z)
                    'tmpWPoint.GetDisttoOtherPose(testMidPose, Result1)

                    hom_mat_3d_from_3d_3d_point_correspondence(IS3.MidPose.X, IS3.MidPose.Y, IS3.MidPose.Z, IS2.MidPose.X, IS2.MidPose.Y, IS2.MidPose.Z, homMat3d)
                    '  correspond_3d_3d_point_withweight(IS3.MidPose.X, IS3.MidPose.Y, IS3.MidPose.Z, IS2.MidPose.X, IS2.MidPose.Y, IS2.MidPose.Z, 10, 0.00001, homMat3d)
                    HOperatorSet.AffineTransPoint3d(homMat3d, IS3.MidPose.X, IS3.MidPose.Y, IS3.MidPose.Z, testMidPose.X, testMidPose.Y, testMidPose.Z)
                    tmpWPoint = IS2.MidPose.ConvertToPoint3d
                    tmpWPoint.GetDisttoOtherPose(testMidPose, Result1)
                    IS3.MeanError = BTuple.TupleMean(Result1)
                    IS3.DevError = BTuple.TupleDeviation(Result1)
                    IS3.MidCount = cntMid
                    HOperatorSet.HomMat3dCompose(ConnectHomMat3d, homMat3d, TempHomMat3d)
                    ConnectHomMat3d = TempHomMat3d
                    HOperatorSet.AffineTransPoint3d(ConnectHomMat3d, IS3.MidPose.X, IS3.MidPose.Y, IS3.MidPose.Z, testMidPose.X, testMidPose.Y, testMidPose.Z)
                    If BTuple.TupleMax(testMidPose.Z) > 48 Then
                        Dim t As Integer = 1

                    End If
                    WorldXYZ.X = BTuple.TupleConcat(WorldXYZ.X, testMidPose.X)
                    WorldXYZ.Y = BTuple.TupleConcat(WorldXYZ.Y, testMidPose.Y)
                    WorldXYZ.Z = BTuple.TupleConcat(WorldXYZ.Z, testMidPose.Z)

                    IS3.CommonHomMat3d = ConnectHomMat3d
                    IS3.CommonScale = commonscale

                    MidRow1 = IS2.RansacMid.RansacPoints.Row
                    MidCol1 = IS2.RansacMid.RansacPoints.Col
                    MidRow2 = IS3.RansacMid.RansacPoints.Row
                    MidCol2 = IS3.RansacMid.RansacPoints.Col
                End If
            End While

        Next
        ' Connect1()
    End Sub

    Public Sub ReConnect()

        Dim i As Integer = 0
        Dim n As Integer = lstImages.Count

        Dim FirstPose1 As New Object
        HOperatorSet.CreatePose(0, 0, 0, 0, 0, 0, "Rp+T", "gba", "point", FirstPose1)
        lstImages.Item(0).ImagePose.Pose = FirstPose1
        '  lstImages.Item(1).ImagePose.Pose = lstImages.Item(1).MidPose.RelPose

        WorldXYZ = New Point3D
        Dim ConnectHomMat3d As New Object
        Dim TempHomMat3d As New Object
        HOperatorSet.HomMat3dIdentity(ConnectHomMat3d)
        Dim commonscale As Double = 1
        Dim NextPose As New Object
        '   Dim HomMatList As New List(Of Object)
        For i = 1 To n - 2
            Dim IS1 As ImageSet = lstImages.Item(i - 1)
            Dim IS2 As ImageSet = lstImages.Item(i)
            Dim IS3 As ImageSet = lstImages.Item(i + 1)
            Dim Scale As Double
            Dim BadIndex As New Object
            Dim cnt As Integer = 0

            If i = 8 Then
                Dim t As Integer = 0
            End If

            Dim flgEnd As Boolean = False
            Dim flgCon As Boolean = False
            Dim cntMid As Integer = 0

            IS1.RansacFirst = New RansacPoints
            IS2.RansacSecond = New RansacPoints
            IS2.RansacFirst = New RansacPoints
            IS3.RansacSecond = New RansacPoints
            IS1.RansacFirst.RansacPointsIndex = IS1.RansacFirstIndexBack
            IS2.RansacSecond.RansacPointsIndex = IS2.RansacSecondIndexBack
            IS2.RansacFirst.RansacPointsIndex = IS2.RansacFirstIndexBack
            IS3.RansacSecond.RansacPointsIndex = IS3.RansacSecondIndexBack
            Try
                If BTuple.TupleLength(IS3.RansacSecond.RansacPointsIndex) = 0 Then

                End If
            Catch ex As Exception
                IS3.RansacSecond.RansacPointsIndex = Nothing
            End Try

            Dim homMat3d As New Object
            Dim tmpWPoint As New Point3D
            Dim testMidPose As New Point3D
            Dim DiffPose As New CameraPose
            Dim AbsPose As New CameraPose
            Dim Result1 As New Object
            Dim Result2 As New Object

            ' GetMidPoints(IS1, IS2)
            cntMid = GetMidPointsOf3Image1(IS1, IS2, IS3)
            'CalcRelPoseByMidPose(IS1, IS2)
            'CalcRelPoseByMidPose(IS2, IS3)

            GetScaleFromMidPoints5(IS1, IS2, IS3, Scale, BadIndex)
            IS3.Scale = Scale

            If i = 1 Then
                ' CalcRelPoseAndXYZ(IS1, IS2)
                'IS2.ImagePose.Pose = IS2.MidPose.RelPose
                IS2.ImagePose.Pose = IS2.VectorPose.Pose
                NextPose = IS2.VectorPose.Pose
                HOperatorSet.HomMat3dIdentity(ConnectHomMat3d)
                IS2.CommonHomMat3d = ConnectHomMat3d
                IS2.CommonScale = 1
                CalcMidXYZ_byRelPose(IS1, IS2, IS2.VectorPose.Pose, Nothing)
                WorldXYZ.X = BTuple.TupleConcat(WorldXYZ.X, IS2.MidPose.X)
                WorldXYZ.Y = BTuple.TupleConcat(WorldXYZ.Y, IS2.MidPose.Y)
                WorldXYZ.Z = BTuple.TupleConcat(WorldXYZ.Z, IS2.MidPose.Z)
            End If

            'CalcRelPoseAndXYZ(IS2, IS3)

            CalcMidXYZ_byRelPose(IS1, IS2, IS2.VectorPose.Pose, Nothing)
            CalcMidXYZ_byRelPose(IS2, IS3, IS2.VectorPose.RelPose, Nothing)

            '  GetScaleFromMidPoints1(IS2, IS3, Scale, BadIndex)
            'IS2.Scale = Scale

            IS2.MidPose.SetScale(commonscale)
            commonscale = commonscale * IS3.Scale '1.03549

            IS3.MidPose.RelPose = IS2.VectorPose.RelPose

            IS2.ImagePose.Pose = NextPose
            '    CalcConnectedPose(IS2, IS3, commonscale)
            ConnectPose(IS2.ImagePose.Pose, IS3.MidPose.RelPose, commonscale, NextPose)

            IS3.ImagePose.Pose = NextPose

            '  CalcMidXYZ_byRelPose(IS2, IS3, IS3.MidPose.RelPose)
            IS3.MidPose.SetScale(commonscale)

            'HOperatorSet.PoseToHomMat3d(IS2.ImagePose.Pose, homMat3d)
            'HOperatorSet.PoseToHomMat3d(IS3.ImagePose.Pose, TempHomMat3d)
            'HOperatorSet.AffineTransPoint3D(homMat3d, IS2.MidPose.X, IS2.MidPose.Y, IS2.MidPose.Z, tmpWPoint.X, tmpWPoint.Y, tmpWPoint.Z)
            'HOperatorSet.AffineTransPoint3D(TempHomMat3d, IS3.MidPose.X, IS3.MidPose.Y, IS3.MidPose.Z, testMidPose.X, testMidPose.Y, testMidPose.Z)
            'tmpWPoint.GetDisttoOtherPose(testMidPose, Result1)

            hom_mat_3d_from_3d_3d_point_correspondence(IS3.MidPose.X, IS3.MidPose.Y, IS3.MidPose.Z, IS2.MidPose.X, IS2.MidPose.Y, IS2.MidPose.Z, homMat3d)
            'correspond_3d_3d_point_withweight(IS3.MidPose.X, IS3.MidPose.Y, IS3.MidPose.Z, IS2.MidPose.X, IS2.MidPose.Y, IS2.MidPose.Z, 10, 0.00001, homMat3d)
            HOperatorSet.AffineTransPoint3d(homMat3d, IS3.MidPose.X, IS3.MidPose.Y, IS3.MidPose.Z, testMidPose.X, testMidPose.Y, testMidPose.Z)
            tmpWPoint = IS2.MidPose.ConvertToPoint3d
            tmpWPoint.GetDisttoOtherPose(testMidPose, Result1)
            IS3.MeanError = BTuple.TupleMean(Result1)
            IS3.DevError = BTuple.TupleMax(Result1)
            IS3.MidCount = cntMid
            'CalcReProjectError(IS3.ImagePose.Pose, IS2.MidPose.ConvertToPoint3d, IS3, Result2)

            HOperatorSet.HomMat3dCompose(ConnectHomMat3d, homMat3d, TempHomMat3d)
            ConnectHomMat3d = TempHomMat3d
            HOperatorSet.AffineTransPoint3d(ConnectHomMat3d, IS3.MidPose.X, IS3.MidPose.Y, IS3.MidPose.Z, testMidPose.X, testMidPose.Y, testMidPose.Z)
            If BTuple.TupleMax(testMidPose.Z) > 48 Then
                Dim t As Integer = 1

            End If
            WorldXYZ.X = BTuple.TupleConcat(WorldXYZ.X, testMidPose.X)
            WorldXYZ.Y = BTuple.TupleConcat(WorldXYZ.Y, testMidPose.Y)
            WorldXYZ.Z = BTuple.TupleConcat(WorldXYZ.Z, testMidPose.Z)

            IS3.CommonHomMat3d = ConnectHomMat3d
            IS3.CommonScale = commonscale

        Next
    End Sub

    Private Sub RunMatchRansac(ByRef IS1 As ImageSet, ByRef IS2 As ImageSet)
        'On Error Resume Next
        Dim strPath As String = My.Application.Info.DirectoryPath & "\MatchRansac\"
        IS2.ImagePose = New CameraPose
        IS1.RansacFirst = New RansacPoints
        IS2.RansacSecond = New RansacPoints
        IS2.ImagePose.hError = Nothing

        'HOperatorSet.WriteImage(IS1.hx_Image, "jpeg", 0, strPath & "Image1.jpg")
        'HOperatorSet.WriteImage(IS2.hx_Image, "jpeg", 0, strPath & "Image2.jpg")
        My.Computer.FileSystem.CopyFile(IS1.ImageFullPath, strPath & "Image1.jpg", True)
        My.Computer.FileSystem.CopyFile(IS2.ImageFullPath, strPath & "Image2.jpg", True)
        HOperatorSet.WriteTuple(IS1.FeaturePoints.Row, strPath & "Row1.tpl")
        HOperatorSet.WriteTuple(IS1.FeaturePoints.Col, strPath & "Col1.tpl")
        HOperatorSet.WriteTuple(IS2.FeaturePoints.Row, strPath & "Row2.tpl")
        HOperatorSet.WriteTuple(IS2.FeaturePoints.Col, strPath & "Col2.tpl")
        HOperatorSet.WriteTuple(hv_CamparamOut, strPath & "CamParam.tpl")
        IS2.WriteMRS_Xml(strPath & "MRS.xml")

        'HOperatorSet.MatchRelPoseRansac(IS1.hx_Image, IS2.hx_Image, IS1.FeaturePoints.Row, IS1.FeaturePoints.Col, _
        '                             IS2.FeaturePoints.Row, IS2.FeaturePoints.Col, hv_CamparamOut, hv_CamparamOut, _
        '                              IS2.MRS.GrayMatchMethod, IS2.MRS.MaskSize, IS2.MRS.RowMove, _
        '                              IS2.MRS.ColMove, IS2.MRS.RowTolerance, IS2.MRS.ColTolerance, _
        '                              IS2.MRS.Rotation, IS2.MRS.MatchThreshold, IS2.MRS.EstimationMethod, _
        '                              IS2.MRS.DistanceThreshold, IS2.MRS.RandSeed, IS2.ImagePose.RelPose, _
        '                              IS2.ImagePose.CovRelPose, IS2.ImagePose.hError, IS1.RansacFirst.RansacPointsIndex, _
        '                              IS2.RansacSecond.RansacPointsIndex)

        Dim strArg As String = strPath & "MatchRansac.exe"
        Shell(strArg, AppWinStyle.Hide, True)

        HOperatorSet.ReadTuple(strPath & "RelPose.tpl", IS2.ImagePose.RelPose)
        HOperatorSet.ReadTuple(strPath & "CovRelPose.tpl", IS2.ImagePose.CovRelPose)
        HOperatorSet.ReadTuple(strPath & "hError.tpl", IS2.ImagePose.hError)
        HOperatorSet.ReadTuple(strPath & "Index1.tpl", IS1.RansacFirst.RansacPointsIndex)
        HOperatorSet.ReadTuple(strPath & "Index2.tpl", IS2.RansacSecond.RansacPointsIndex)

        If BTuple.TupleLength(IS2.ImagePose.hError) = 1 Then

            CalcRelPoseAndXYZ(IS1, IS2)
        Else
            Dim t As Integer = 1
        End If


        'Dim i As Integerr
        'Dim Rtn1 As New Object
        'Dim Rtn2 As New Object
        'Dim Sel1 As New Object
        'Dim Sel2 As New Object
        'Dim ind1 As New Object
        'Dim ind2 As New Object
        'Dim Point1 As New Object
        'Dim Point2 As New Object
        'Dim tmp As New Object
        'Rtn1 = BTuple.TupleGenConst(0, 0)
        'Rtn2 = BTuple.TupleGenConst(0, 0)
        'For i = 0 To 4
        '    HOperatorSet.MatchRelPoseRansac(IS1.hx_Image, IS2.hx_Image, IS1.FeaturePoints.Row, IS1.FeaturePoints.Col, _
        '                           IS2.FeaturePoints.Row, IS2.FeaturePoints.Col, hv_CamparamOut, hv_CamparamOut, _
        '                            MRS.GrayMatchMethod, MRS.MaskSize, MRS.RowMove, _
        '                            MRS.ColMove, MRS.RowTolerance, MRS.ColTolerance, _
        '                            MRS.Rotation, MRS.MatchThreshold, MRS.EstimationMethod, _
        '                            MRS.DistanceThreshold, MRS.RandSeed, IS2.ImagePose.RelPose, _
        '                            IS2.ImagePose.CovRelPose, IS2.ImagePose.hError, Point1, _
        '                            Point2)
        '    Rtn1 = BTuple.TupleConcat(Rtn1, Point1)
        '    Rtn2 = BTuple.TupleConcat(Rtn2, Point2)
        '    ' HOperatorSet.WaitSeconds(2)
        'Next
        'sort_pairs(Rtn1, Rtn2, "1", Rtn1, Rtn2)
        'Point1 = BTuple.TupleGenConst(0, 0)
        'Point2 = BTuple.TupleGenConst(0, 0)
        'For i = 0 To BTuple.TupleLength(Rtn1) - 1
        '    Sel1 = BTuple.TupleSelect(Rtn1, i)
        '    Sel2 = BTuple.TupleSelect(Rtn2, i)
        '    ind1 = BTuple.TupleFind(Rtn1, Sel1)
        '    ind2 = BTuple.TupleFind(Rtn2, Sel2)
        '    If BTuple.TupleLength(ind1) = BTuple.TupleLength(ind2) And BTuple.TupleEqual(ind1, ind2) = 1 Then
        '        tmp = BTuple.TupleFind(Point1, Sel1)
        '        If BTuple.TupleLength(Point1) = 0 Then
        '            Point1 = BTuple.TupleConcat(Point1, Sel1)
        '            Point2 = BTuple.TupleConcat(Point2, Sel2)
        '        Else
        '            If BTuple.TupleLength(tmp) = 0 Or tmp = -1 Then
        '                Point1 = BTuple.TupleConcat(Point1, Sel1)
        '                Point2 = BTuple.TupleConcat(Point2, Sel2)

        '            End If
        '        End If

        '    End If
        'Next

        'IS1.RansacFirst.RansacPointsIndex = Point1
        'IS2.RansacSecond.RansacPointsIndex = Point2

        'CalcRelPoseAndXYZ(IS1, IS2)


    End Sub


    Private Sub RunMatchRansac(ByRef IS1 As ImageSet, ByRef IS2 As ImageSet, ByVal flgFirst As Boolean, ByVal strFolder As String)
        'On Error Resume Next
        Dim strPath As String = My.Application.Info.DirectoryPath & strFolder
        IS2.ImagePose = New CameraPose
        IS1.RansacFirst = New RansacPoints
        IS2.RansacSecond = New RansacPoints
        IS2.ImagePose.hError = Nothing
        If flgFirst = True Then


            'HOperatorSet.WriteImage(IS1.hx_Image, "jpeg", 0, strPath & "Image1.jpg")
            'HOperatorSet.WriteImage(IS2.hx_Image, "jpeg", 0, strPath & "Image2.jpg")
            My.Computer.FileSystem.CopyFile(IS1.ImageTransFullPath & ".png", strPath & "Image1.png", True)
            My.Computer.FileSystem.CopyFile(IS2.ImageTransFullPath & ".png", strPath & "Image2.png", True)
            HOperatorSet.WriteTuple(IS1.FeaturePoints.Row, strPath & "Row1.tpl")
            HOperatorSet.WriteTuple(IS1.FeaturePoints.Col, strPath & "Col1.tpl")
            HOperatorSet.WriteTuple(IS2.FeaturePoints.Row, strPath & "Row2.tpl")
            HOperatorSet.WriteTuple(IS2.FeaturePoints.Col, strPath & "Col2.tpl")
            HOperatorSet.WriteTuple(hv_CamparamOut, strPath & "CamParam.tpl")
            IS2.WriteMRS_Xml(strPath & "MRS.xml")

            Dim strArg As String = strPath & "MatchRansac.exe"
            Shell(strArg, AppWinStyle.Hide, True)
        End If

        HOperatorSet.ReadTuple(strPath & "RelPose.tpl", IS2.ImagePose.RelPose)
        HOperatorSet.ReadTuple(strPath & "CovRelPose.tpl", IS2.ImagePose.CovRelPose)
        HOperatorSet.ReadTuple(strPath & "hError.tpl", IS2.ImagePose.hError)
        HOperatorSet.ReadTuple(strPath & "Index1.tpl", IS1.RansacFirst.RansacPointsIndex)
        HOperatorSet.ReadTuple(strPath & "Index2.tpl", IS2.RansacSecond.RansacPointsIndex)
        HOperatorSet.ReadTuple(strPath & "Index1.tpl", IS1.RansacFirstIndexBack)
        HOperatorSet.ReadTuple(strPath & "Index2.tpl", IS2.RansacSecondIndexBack)
        If BTuple.TupleLength(IS2.ImagePose.hError) = 1 Then

            CalcRelPoseAndXYZ(IS1, IS2)
        Else
            Dim t As Integer = 1
        End If


    End Sub

    Private Sub ReMatchRansac(ByRef IS1 As ImageSet, ByRef IS2 As ImageSet, ByVal N As Integer)
        Dim i As Integer
        Dim nn As Integer
        Dim R As Object
        Dim C As Object
        Dim index As New Object
        Dim index1 As New Object
        Dim index2 As New Object
        For i = 1 To N

            HOperatorSet.MatchRelPoseRansac(IS1.hx_Image, IS2.hx_Image, IS1.RansacFirst.RansacPoints.Row, IS1.RansacFirst.RansacPoints.Col, _
                           IS2.RansacSecond.RansacPoints.Row, IS2.RansacSecond.RansacPoints.Col, hv_CamparamOut, hv_CamparamOut, _
                                     MRS.GrayMatchMethod, MRS.MaskSize, MRS.RowMove, _
                                     MRS.ColMove, MRS.RowTolerance, MRS.ColTolerance, _
                                     MRS.Rotation, MRS.MatchThreshold, MRS.EstimationMethod, _
                                     MRS.DistanceThreshold, MRS.RandSeed, IS2.ImagePose.RelPose, _
                                     IS2.ImagePose.CovRelPose, IS2.ImagePose.hError, IS1.RansacFirst.RansacPointsIndex, _
                                     IS2.RansacSecond.RansacPointsIndex)
            IS1.RansacFirst.SubSet(IS1.RansacFirst.RansacPoints)
            IS2.RansacSecond.SubSet(IS2.RansacSecond.RansacPoints)
            'ReCalcRansacMatch(IS1, IS2)

        Next
        index1 = BTuple.TupleGenConst(0, 0)
        index2 = BTuple.TupleGenConst(0, 0)
        nn = CInt(IS1.RansacFirst.RansacPoints.CountPoints)

        For i = 0 To nn - 1
            R = BTuple.TupleSelect(IS1.RansacFirst.RansacPoints.Row, i)
            C = BTuple.TupleSelect(IS1.RansacFirst.RansacPoints.Col, i)

            IS1.FeaturePoints.GetPointsByCoord(R, C, index)
            index1 = BTuple.TupleConcat(index1, index)

            R = BTuple.TupleSelect(IS2.RansacSecond.RansacPoints.Row, i)
            C = BTuple.TupleSelect(IS2.RansacSecond.RansacPoints.Col, i)
            IS2.FeaturePoints.GetPointsByCoord(R, C, index)
            index2 = BTuple.TupleConcat(index2, index)
        Next
        IS1.RansacFirst.RansacPointsIndex = index1
        IS2.RansacSecond.RansacPointsIndex = index2

        CalcRelPoseAndXYZ(IS1, IS2)

    End Sub

    Private Sub ReCalcRansacMatch(ByRef IS1 As ImageSet, ByRef IS2 As ImageSet)
        Dim RowTol As New Object
        Dim ColTol As New Object
        'IS1.SubSetFirst()
        'IS2.SubSetSecond()
        CalcTolerance(IS1.RansacFirst.RansacPoints.Row, IS2.RansacSecond.RansacPoints.Row, RowTol)
        CalcTolerance(IS1.RansacFirst.RansacPoints.Col, IS2.RansacSecond.RansacPoints.Col, ColTol)
        Dim rad As Object
        rad = BTuple.TupleSelect(IS2.ImagePose.RelPose, 5)
        If rad > 180 Then
            rad = BTuple.TupleAbs(BTuple.TupleRad(BTuple.TupleSub(rad, 360)))
        Else
            rad = BTuple.TupleAbs(BTuple.TupleRad(BTuple.TupleSub(rad, 0)))

        End If
        rad = BTuple.TupleConcat(-rad, rad)

        HOperatorSet.MatchRelPoseRansac(IS1.hx_Image, IS2.hx_Image, IS1.RansacFirst.RansacPoints.Row, IS1.RansacFirst.RansacPoints.Col, _
                         IS2.RansacSecond.RansacPoints.Row, IS2.RansacSecond.RansacPoints.Col, hv_CamparamOut, hv_CamparamOut, _
                                   MRS.GrayMatchMethod, MRS.MaskSize, MRS.RowMove, _
                                   MRS.ColMove, RowTol, ColTol, _
                                   MRS.Rotation, MRS.MatchThreshold, MRS.EstimationMethod, _
                                   MRS.DistanceThreshold, MRS.RandSeed, IS2.ImagePose.RelPose, _
                                   IS2.ImagePose.CovRelPose, IS2.ImagePose.hError, IS1.RansacFirst.RansacPointsIndex, _
                                   IS2.RansacSecond.RansacPointsIndex)
        IS1.RansacFirst.SubSet(IS1.RansacFirst.RansacPoints)
        IS2.RansacSecond.SubSet(IS2.RansacSecond.RansacPoints)

    End Sub

    Private Sub CalcTolerance(ByVal T1 As Object, ByVal T2 As Object, ByRef T As Object)
        Dim Diff As Object
        Diff = BTuple.TupleSub(T1, T2)
        Diff = BTuple.TupleAbs(Diff)
        Diff = BTuple.TupleMax(Diff)
        Diff = BTuple.TupleInt(Diff)
        T = BTuple.TupleAdd(Diff, 1)

    End Sub
    Private Function CalcRelPoseAndXYZ(ByRef IS1 As ImageSet, ByRef IS2 As ImageSet) As Integer
        On Error Resume Next
        IS1.SubSetFirst()
        IS2.SubSetSecond()
        IS2.ImagePose.hError = Nothing
        HOperatorSet.VectorToRelPose(IS1.RansacFirst.RansacPoints.Row, IS1.RansacFirst.RansacPoints.Col, _
                           IS2.RansacSecond.RansacPoints.Row, IS2.RansacSecond.RansacPoints.Col, _
                           New HTuple, New HTuple, New HTuple, New HTuple, New HTuple, New HTuple, hv_CamparamOut, hv_CamparamOut, "gold_standard", _
                            IS2.ImagePose.RelPose, IS2.ImagePose.CovRelPose, IS2.ImagePose.hError, _
                            IS2.ImagePose.X, IS2.ImagePose.Y, IS2.ImagePose.Z, IS2.ImagePose.CovXYZ)
        If BTuple.TupleLength(IS2.ImagePose.hError) = 2 Then
            CalcRelPoseAndXYZ = 2
        Else
            CalcRelPoseAndXYZ = 1
        End If

    End Function

    Private Sub CalcRelPoseByMidPose(ByRef IS1 As ImageSet, ByRef IS2 As ImageSet)
        'GetMidPoints(IS1, IS2)
        On Error Resume Next
        IS2.MidPose.hError = Nothing
        HOperatorSet.VectorToRelPose(IS1.RansacMid.RansacPoints.Row, IS1.RansacMid.RansacPoints.Col, _
                          IS2.RansacMid.RansacPoints.Row, IS2.RansacMid.RansacPoints.Col, _
                          Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, hv_CamparamOut, hv_CamparamOut, "gold_standard", _
                           IS2.MidPose.RelPose, IS2.MidPose.CovRelPose, IS2.MidPose.hError, _
                           IS2.MidPose.X, IS2.MidPose.Y, IS2.MidPose.Z, IS2.MidPose.CovXYZ)
        If BTuple.TupleLength(IS2.MidPose.hError) = 2 Or BTuple.TupleLength(IS2.MidPose.hError) = 0 Then
            If CalcRelPoseAndXYZ(IS1, IS2) = 1 Then

                CalcMidXYZ_byRelPose(IS1, IS2, IS2.ImagePose.RelPose, Nothing)
                IS2.MidPose.RelPose = IS2.ImagePose.RelPose
                IS2.MidPose.hError = IS2.ImagePose.hError

            Else
                If BTuple.TupleSelect(IS2.MidPose.hError, 0) < BTuple.TupleSelect(IS2.MidPose.hError, 1) Then
                    Dim n As Integer
                    n = BTuple.TupleLength(IS2.MidPose.X)
                    IS2.MidPose.RelPose = BTuple.TupleFirstN(IS2.MidPose.RelPose, 6)
                    IS2.MidPose.X = BTuple.TupleFirstN(IS2.MidPose.X, n / 2 - 1)
                    IS2.MidPose.Y = BTuple.TupleFirstN(IS2.MidPose.Y, n / 2 - 1)
                    IS2.MidPose.Z = BTuple.TupleFirstN(IS2.MidPose.Z, n / 2 - 1)
                Else
                    Dim n As Integer
                    n = BTuple.TupleLength(IS2.MidPose.X)
                    IS2.MidPose.RelPose = BTuple.TupleLastN(IS2.MidPose.RelPose, 7)
                    IS2.MidPose.X = BTuple.TupleLastN(IS2.MidPose.X, n / 2)
                    IS2.MidPose.Y = BTuple.TupleLastN(IS2.MidPose.Y, n / 2)
                    IS2.MidPose.Z = BTuple.TupleLastN(IS2.MidPose.Z, n / 2)


                End If
            End If
        End If


    End Sub

    Private Sub CalcMidXYZ_byRelPose(ByRef IS1 As ImageSet, ByRef IS2 As ImageSet, ByVal RelPose As Object, ByRef Dist As Object)
        On Error Resume Next
        Dim i As Integer
        Dim X As Object = New HTuple
        Dim Y As Object = New HTuple
        Dim Z As Object = New HTuple

        IS2.MidPose.X = Nothing
        IS2.MidPose.Y = Nothing
        IS2.MidPose.Z = Nothing


        HOperatorSet.IntersectLinesOfSight(hv_CamparamOut, hv_CamparamOut, BTuple.TupleFirstN(RelPose, 6), _
                                  IS1.RansacMid.RansacPoints.Row, IS1.RansacMid.RansacPoints.Col, _
                                   IS2.RansacMid.RansacPoints.Row, IS2.RansacMid.RansacPoints.Col, _
                                   IS2.MidPose.X, IS2.MidPose.Y, IS2.MidPose.Z, Dist)
        If BTuple.TupleLength(IS2.MidPose.X) = 0 Then
            IS2.MidPose.X = BTuple.TupleSelect(IS2.ImagePose.X, IS2.RansacMidSecond.RansacPointsIndex)
            IS2.MidPose.Y = BTuple.TupleSelect(IS2.ImagePose.Y, IS2.RansacMidSecond.RansacPointsIndex)
            IS2.MidPose.Z = BTuple.TupleSelect(IS2.ImagePose.Z, IS2.RansacMidSecond.RansacPointsIndex)
            For i = 0 To IS1.RansacMid.RansacPoints.CountPoints - 1
                X = Nothing
                HOperatorSet.IntersectLinesOfSight(hv_CamparamOut, hv_CamparamOut, BTuple.TupleFirstN(RelPose, 6), _
                                      BTuple.TupleSelect(IS1.RansacMid.RansacPoints.Row, i), BTuple.TupleSelect(IS1.RansacMid.RansacPoints.Col, i), _
                                       BTuple.TupleSelect(IS2.RansacMid.RansacPoints.Row, i), BTuple.TupleSelect(IS2.RansacMid.RansacPoints.Col, i), _
                                       X, Y, Z, Dist)
                If BTuple.TupleLength(X) = 0 Then
                    IS2.MidPose.X = BTuple.TupleConcat(IS2.MidPose.X, 9999)
                    IS2.MidPose.Y = BTuple.TupleConcat(IS2.MidPose.Y, 9999)
                    IS2.MidPose.Z = BTuple.TupleConcat(IS2.MidPose.Z, 9999)
                Else
                    IS2.MidPose.X = BTuple.TupleConcat(IS2.MidPose.X, X)
                    IS2.MidPose.Y = BTuple.TupleConcat(IS2.MidPose.Y, Y)
                    IS2.MidPose.Z = BTuple.TupleConcat(IS2.MidPose.Z, Z)
                End If
            Next
        End If

        'HOperatorSet.IntersectLinesOfSight(hv_CamparamOut, hv_CamparamOut, BTuple.TupleFirstN(RelPose, 6), _
        '                          IS1.RansacMid.RansacPoints.Row, IS1.RansacMid.RansacPoints.Col, _
        '                           IS2.RansacMid.RansacPoints.Row, IS2.RansacMid.RansacPoints.Col, _
        '                           IS2.MidPose.X, IS2.MidPose.Y, IS2.MidPose.Z, Dist)

    End Sub

    Private Sub CalcBadIndex_byRelPose(ByRef IS1 As ImageSet, ByRef IS2 As ImageSet, ByVal RelPose As Object, ByRef BadIndex As Object)
        On Error Resume Next
        Dim i As Integer
        Dim X As Object = New HTuple
        Dim Y As Object = New HTuple
        Dim Z As Object = New HTuple
        Dim Dist As New Object

        IS2.MidPose.X = Nothing
        IS2.MidPose.Y = Nothing
        IS2.MidPose.Z = Nothing

        BadIndex = Nothing
        HOperatorSet.IntersectLinesOfSight(hv_CamparamOut, hv_CamparamOut, BTuple.TupleFirstN(RelPose, 6), _
                                  IS1.RansacMid.RansacPoints.Row, IS1.RansacMid.RansacPoints.Col, _
                                   IS2.RansacMid.RansacPoints.Row, IS2.RansacMid.RansacPoints.Col, _
                                   IS2.MidPose.X, IS2.MidPose.Y, IS2.MidPose.Z, Dist)
        If BTuple.TupleLength(IS2.MidPose.X) = 0 Then

            For i = 0 To IS1.RansacMid.RansacPoints.CountPoints - 1
                X = Nothing
                HOperatorSet.IntersectLinesOfSight(hv_CamparamOut, hv_CamparamOut, BTuple.TupleFirstN(RelPose, 6), _
                                      BTuple.TupleSelect(IS1.RansacMid.RansacPoints.Row, i), BTuple.TupleSelect(IS1.RansacMid.RansacPoints.Col, i), _
                                       BTuple.TupleSelect(IS2.RansacMid.RansacPoints.Row, i), BTuple.TupleSelect(IS2.RansacMid.RansacPoints.Col, i), _
                                       X, Y, Z, Dist)
                If BTuple.TupleLength(X) = 0 Then
                    BadIndex = BTuple.TupleConcat(BadIndex, i)
                End If
            Next
            Exit Sub
        End If


        'HOperatorSet.IntersectLinesOfSight(hv_CamparamOut, hv_CamparamOut, BTuple.TupleFirstN(RelPose, 6), _
        '                          IS1.RansacMid.RansacPoints.Row, IS1.RansacMid.RansacPoints.Col, _
        '                           IS2.RansacMid.RansacPoints.Row, IS2.RansacMid.RansacPoints.Col, _
        '                           IS2.MidPose.X, IS2.MidPose.Y, IS2.MidPose.Z, Dist)

    End Sub


    Public Sub OptimalMatching()
        Dim i As Integer = 0
        Dim n As Integer = lstImages.Count

        For i = 1 To n - 2
            Dim IS1 As ImageSet = lstImages.Item(i - 1)
            Dim IS2 As ImageSet = lstImages.Item(i)
            Dim IS3 As ImageSet = lstImages.Item(i + 1)
            Dim Scale As Double
            Dim BadIndex As New Object

            If i = 14 Then
                Dim t As Integer = 0
            End If
            ReMatchRansac(IS1, IS2, 3)
            ReMatchRansac(IS2, IS3, 3)
            BadIndex = BTuple.TupleGenConst(1, 0)
            ' Do Until BTuple.TupleLength(BadIndex) = 0
            ' GetMidPoints(IS2, IS3)
            If GetMidPointsOf3Image1(IS1, IS2, IS3) >= 6 Then
            Else
                RunMatchRansac(IS1, IS2)
                RunMatchRansac(IS2, IS3)
                GetMidPointsOf3Image1(IS1, IS2, IS3)
            End If

            CalcRelPoseByMidPose(IS1, IS2)
            CalcRelPoseByMidPose(IS2, IS3)
            GetScaleFromMidPoints(IS2, IS3, Scale, BadIndex)
            IS3.Scale = Scale
            RemoveBadPoints(IS1, IS2, IS3, BadIndex)
            ' Loop

            'CalcRelPoseByMidPose(IS2, IS3)

        Next

    End Sub


    Public Sub Connect()
        Dim i As Integer = 0

        Dim n As Integer = lstImages.Count
        Dim FirstPose1 As New Object
        HOperatorSet.CreatePose(0, 0, 0, 0, 0, 0, "Rp+T", "gba", "point", FirstPose1)
        lstImages.Item(0).ImagePose.Pose = FirstPose1


        WorldXYZ = New Point3D
        Dim ConnectHomMat3d As New Object
        Dim TempHomMat3d As New Object
        HOperatorSet.HomMat3dIdentity(ConnectHomMat3d)

        Dim commonscale As Double = 1
        For i = 1 To n - 2
            Dim IS1 As ImageSet = lstImages.Item(i - 1)
            Dim IS2 As ImageSet = lstImages.Item(i)
            Dim IS3 As ImageSet = lstImages.Item(i + 1)
            Dim homMat3d As New Object
            Dim tmpWPoint As New Point3D
            Dim testMidPose As New Point3D
            Dim DiffPose As New CameraPose
            Dim AbsPose As New CameraPose
            Dim Result1 As New Object
            Dim Result2 As New Object
            Dim BadIndex As New Object
            If i = 10 Then
                Dim t As Integer = 0

            End If

            ' GetMidPoints(IS1, IS2)
            GetMidPointsOf3Image1(IS1, IS2, IS3)
            'CalcRelPoseByMidPose(IS1, IS2)
            'CalcRelPoseByMidPose(IS2, IS3)


            If i = 1 Then
                CalcRelPoseAndXYZ(IS1, IS2)
                'IS2.ImagePose.Pose = IS2.MidPose.RelPose
                IS2.ImagePose.Pose = IS2.ImagePose.RelPose
                WorldXYZ.X = BTuple.TupleConcat(WorldXYZ.X, IS2.MidPose.X)
                WorldXYZ.Y = BTuple.TupleConcat(WorldXYZ.Y, IS2.MidPose.Y)
                WorldXYZ.Z = BTuple.TupleConcat(WorldXYZ.Z, IS2.MidPose.Z)

            End If
            CalcRelPoseAndXYZ(IS2, IS3)

            CalcMidXYZ_byRelPose(IS1, IS2, IS2.ImagePose.RelPose, Nothing)
            CalcMidXYZ_byRelPose(IS2, IS3, IS3.ImagePose.RelPose, Nothing)

            '  GetScaleFromMidPoints1(IS2, IS3, Scale, BadIndex)
            'IS2.Scale = Scale
            commonscale = commonscale * IS3.Scale
            CalcConnectedPose1(IS2, IS3, commonscale)

            IS3.MidPose.SetScale(commonscale)
            HOperatorSet.PoseToHomMat3d(IS2.ImagePose.Pose, homMat3d)

            HOperatorSet.AffineTransPoint3d(homMat3d, IS3.MidPose.X, IS3.MidPose.Y, IS3.MidPose.Z, testMidPose.X, testMidPose.Y, testMidPose.Z)

            WorldXYZ.X = BTuple.TupleConcat(WorldXYZ.X, testMidPose.X)
            WorldXYZ.Y = BTuple.TupleConcat(WorldXYZ.Y, testMidPose.Y)
            WorldXYZ.Z = BTuple.TupleConcat(WorldXYZ.Z, testMidPose.Z)

            IS3.ImagePose.SetScale(commonscale)
            HOperatorSet.AffineTransPoint3d(homMat3d, IS3.ImagePose.X, IS3.ImagePose.Y, IS3.ImagePose.Z, tmpWPoint.X, tmpWPoint.Y, tmpWPoint.Z)
            IS3.ImagePose.X = tmpWPoint.X
            IS3.ImagePose.Y = tmpWPoint.Y
            IS3.ImagePose.Z = tmpWPoint.Z

        Next

    End Sub

    Public Sub Connect1()
        Dim i As Integer = 0

        Dim n As Integer = lstImages.Count
        Dim FirstPose1 As New Object
        HOperatorSet.CreatePose(0, 0, 0, 0, 0, 0, "Rp+T", "gba", "point", FirstPose1)
        lstImages.Item(0).ImagePose.Pose = FirstPose1
        '  lstImages.Item(1).ImagePose.Pose = lstImages.Item(1).MidPose.RelPose

        WorldXYZ = New Point3D
        Dim ConnectHomMat3d As New Object
        Dim TempHomMat3d As New Object
        HOperatorSet.HomMat3dIdentity(ConnectHomMat3d)
        Dim commonscale As Double = 1
        For i = 1 To n - 2
            Dim IS1 As ImageSet = lstImages.Item(i - 1)
            Dim IS2 As ImageSet = lstImages.Item(i)
            Dim IS3 As ImageSet = lstImages.Item(i + 1)
            Dim homMat3d As New Object
            Dim tmpWPoint As New Point3D
            Dim testMidPose As New Point3D
            Dim DiffPose As New CameraPose
            Dim AbsPose As New CameraPose
            Dim Result1 As New Object
            Dim Result2 As New Object
            Dim BadIndex As New Object
            If i = 25 Then
                Dim t As Integer = 0

            End If

            ' GetMidPoints(IS1, IS2)
            GetMidPointsOf3Image1(IS1, IS2, IS3)
            CalcRelPoseByMidPose(IS1, IS2)
            CalcRelPoseByMidPose(IS2, IS3)

            If i = 1 Then
                CalcRelPoseAndXYZ(IS1, IS2)
                'IS2.ImagePose.Pose = IS2.MidPose.RelPose
                IS2.ImagePose.Pose = IS2.ImagePose.RelPose
                IS2.CommonHomMat3d = ConnectHomMat3d
                IS2.CommonScale = 1
                WorldXYZ.X = BTuple.TupleConcat(WorldXYZ.X, IS2.MidPose.X)
                WorldXYZ.Y = BTuple.TupleConcat(WorldXYZ.Y, IS2.MidPose.Y)
                WorldXYZ.Z = BTuple.TupleConcat(WorldXYZ.Z, IS2.MidPose.Z)
            End If

            CalcRelPoseAndXYZ(IS2, IS3)

            CalcMidXYZ_byRelPose(IS1, IS2, IS2.MidPose.RelPose, Nothing)
            CalcMidXYZ_byRelPose(IS2, IS3, IS3.MidPose.RelPose, Nothing)

            '  GetScaleFromMidPoints1(IS2, IS3, Scale, BadIndex)
            'IS2.Scale = Scale

            IS2.MidPose.SetScale(commonscale)

            commonscale = commonscale * IS3.Scale '1.03549
            CalcConnectedPose(IS2, IS3, commonscale)
            '  CalcMidXYZ_byRelPose(IS2, IS3, IS3.MidPose.RelPose)
            IS3.MidPose.SetScale(commonscale)

            hom_mat_3d_from_3d_3d_point_correspondence(IS3.MidPose.X, IS3.MidPose.Y, IS3.MidPose.Z, IS2.MidPose.X, IS2.MidPose.Y, IS2.MidPose.Z, homMat3d)
            HOperatorSet.AffineTransPoint3d(homMat3d, IS3.MidPose.X, IS3.MidPose.Y, IS3.MidPose.Z, testMidPose.X, testMidPose.Y, testMidPose.Z)
            tmpWPoint = IS2.MidPose.ConvertToPoint3d
            tmpWPoint.GetDisttoOtherPose(testMidPose, Result1)

            'HOperatorSet.PoseToHomMat3d(IS1.ImagePose.Pose, ConnectHomMat3d)
            ' HOperatorSet.HomMat3dToPose(ConnectHomMat3d, IS1.ImagePose.Pose)
            HOperatorSet.HomMat3dCompose(ConnectHomMat3d, homMat3d, TempHomMat3d)
            ConnectHomMat3d = TempHomMat3d
            ' HOperatorSet.HomMat3dToPose(ConnectHomMat3d, IS2.ImagePose.Pose)
            HOperatorSet.AffineTransPoint3d(ConnectHomMat3d, IS3.MidPose.X, IS3.MidPose.Y, IS3.MidPose.Z, testMidPose.X, testMidPose.Y, testMidPose.Z)
            WorldXYZ.X = BTuple.TupleConcat(WorldXYZ.X, testMidPose.X)
            WorldXYZ.Y = BTuple.TupleConcat(WorldXYZ.Y, testMidPose.Y)
            WorldXYZ.Z = BTuple.TupleConcat(WorldXYZ.Z, testMidPose.Z)

            IS3.ImagePose.SetScale(commonscale)
            HOperatorSet.AffineTransPoint3d(ConnectHomMat3d, IS3.ImagePose.X, IS3.ImagePose.Y, IS3.ImagePose.Z, tmpWPoint.X, tmpWPoint.Y, tmpWPoint.Z)
            IS3.ImagePose.X = tmpWPoint.X
            IS3.ImagePose.Y = tmpWPoint.Y
            IS3.ImagePose.Z = tmpWPoint.Z

            IS3.CommonHomMat3d = ConnectHomMat3d
            IS3.CommonScale = commonscale
        Next

    End Sub

    Public Sub Connect2()
        Dim i As Integer = 0

        Dim n As Integer = lstImages.Count
        Dim FirstPose1 As New Object
        HOperatorSet.CreatePose(0, 0, 0, 0, 0, 0, "Rp+T", "gba", "point", FirstPose1)
        lstImages.Item(0).ImagePose.Pose = FirstPose1
        '  lstImages.Item(1).ImagePose.Pose = lstImages.Item(1).MidPose.RelPose

        WorldXYZ = New Point3D
        Dim ConnectHomMat3d As New Object
        Dim TempHomMat3d As New Object
        HOperatorSet.HomMat3dIdentity(ConnectHomMat3d)
        Dim commonscale As Double = 1
        For i = 1 To n - 2
            Dim IS1 As ImageSet = lstImages.Item(i - 1)
            Dim IS2 As ImageSet = lstImages.Item(i)
            Dim IS3 As ImageSet = lstImages.Item(i + 1)
            Dim homMat3d As New Object
            Dim tmpWPoint As New Point3D
            Dim testMidPose As New Point3D
            Dim DiffPose As New CameraPose
            Dim AbsPose As New CameraPose
            Dim Result1 As New Object
            Dim Result2 As New Object
            Dim BadIndex As New Object
            If i = 10 Then
                Dim t As Integer = 0

            End If

            ' GetMidPoints(IS1, IS2)
            GetMidPointsOf3Image1(IS1, IS2, IS3)
            CalcRelPoseByMidPose(IS1, IS2)
            CalcRelPoseByMidPose(IS2, IS3)
            ' RunBA(IS1, IS2, IS3)

            If i = 1 Then
                CalcRelPoseAndXYZ(IS1, IS2)
                'IS2.ImagePose.Pose = IS2.MidPose.RelPose
                IS2.ImagePose.Pose = IS2.MidPose.RelPose
                IS2.CommonHomMat3d = ConnectHomMat3d
                IS2.CommonScale = 1
                WorldXYZ.X = BTuple.TupleConcat(WorldXYZ.X, IS2.MidPose.X)
                WorldXYZ.Y = BTuple.TupleConcat(WorldXYZ.Y, IS2.MidPose.Y)
                WorldXYZ.Z = BTuple.TupleConcat(WorldXYZ.Z, IS2.MidPose.Z)
            End If

            CalcRelPoseAndXYZ(IS2, IS3)

            CalcMidXYZ_byRelPose(IS1, IS2, IS2.MidPose.RelPose, Nothing)
            CalcMidXYZ_byRelPose(IS2, IS3, IS3.MidPose.RelPose, Nothing)

            '  GetScaleFromMidPoints1(IS2, IS3, Scale, BadIndex)
            'IS2.Scale = Scale

            IS2.MidPose.SetScale(commonscale)

            commonscale = commonscale * IS3.ScaleBA  '1.03549
            CalcConnectedPose(IS2, IS3, commonscale)
            '  CalcMidXYZ_byRelPose(IS2, IS3, IS3.MidPose.RelPose)
            IS3.MidPose.SetScale(commonscale)

            hom_mat_3d_from_3d_3d_point_correspondence(IS3.MidPose.X, IS3.MidPose.Y, IS3.MidPose.Z, IS2.MidPose.X, IS2.MidPose.Y, IS2.MidPose.Z, homMat3d)
            HOperatorSet.AffineTransPoint3d(homMat3d, IS3.MidPose.X, IS3.MidPose.Y, IS3.MidPose.Z, testMidPose.X, testMidPose.Y, testMidPose.Z)
            tmpWPoint = IS2.MidPose.ConvertToPoint3d
            tmpWPoint.GetDisttoOtherPose(testMidPose, Result1)

            'HOperatorSet.PoseToHomMat3d(IS1.ImagePose.Pose, ConnectHomMat3d)
            ' HOperatorSet.HomMat3dToPose(ConnectHomMat3d, IS1.ImagePose.Pose)
            HOperatorSet.HomMat3dCompose(ConnectHomMat3d, homMat3d, TempHomMat3d)
            ConnectHomMat3d = TempHomMat3d
            ' HOperatorSet.HomMat3dToPose(ConnectHomMat3d, IS2.ImagePose.Pose)
            HOperatorSet.AffineTransPoint3d(ConnectHomMat3d, IS3.MidPose.X, IS3.MidPose.Y, IS3.MidPose.Z, testMidPose.X, testMidPose.Y, testMidPose.Z)
            WorldXYZ.X = BTuple.TupleConcat(WorldXYZ.X, testMidPose.X)
            WorldXYZ.Y = BTuple.TupleConcat(WorldXYZ.Y, testMidPose.Y)
            WorldXYZ.Z = BTuple.TupleConcat(WorldXYZ.Z, testMidPose.Z)

            IS3.ImagePose.SetScale(commonscale)
            HOperatorSet.AffineTransPoint3d(ConnectHomMat3d, IS3.ImagePose.X, IS3.ImagePose.Y, IS3.ImagePose.Z, tmpWPoint.X, tmpWPoint.Y, tmpWPoint.Z)
            IS3.ImagePose.X = tmpWPoint.X
            IS3.ImagePose.Y = tmpWPoint.Y
            IS3.ImagePose.Z = tmpWPoint.Z

            IS3.CommonHomMat3d = ConnectHomMat3d
            IS3.CommonScale = commonscale
        Next

    End Sub


    Public Sub Connect3()
        Dim i As Integer = 0

        Dim n As Integer = lstImages.Count
        Dim FirstPose1 As New Object
        HOperatorSet.CreatePose(0, 0, 0, 0, 0, 0, "Rp+T", "gba", "point", FirstPose1)
        lstImages.Item(0).ImagePose.Pose = FirstPose1
        '  lstImages.Item(1).ImagePose.Pose = lstImages.Item(1).MidPose.RelPose

        WorldXYZ = New Point3D
        Dim ConnectHomMat3d As New Object
        Dim TempHomMat3d As New Object
        HOperatorSet.HomMat3dIdentity(ConnectHomMat3d)
        Dim commonscale As Double = 1
        For i = 1 To n - 2
            Dim IS1 As ImageSet = lstImages.Item(i - 1)
            Dim IS2 As ImageSet = lstImages.Item(i)
            Dim IS3 As ImageSet = lstImages.Item(i + 1)
            Dim homMat3d As New Object
            Dim tmpWPoint As New Point3D
            Dim testMidPose As New Point3D
            Dim DiffPose As New CameraPose
            Dim AbsPose As New CameraPose
            Dim Result1 As New Object
            Dim Result2 As New Object

            If i = 25 Then
                Dim t As Integer = 0

            End If

            ' GetMidPoints(IS1, IS2)
            GetMidPointsOf3Image1(IS1, IS2, IS3)
            'CalcRelPoseByMidPose(IS1, IS2)
            'CalcRelPoseByMidPose(IS2, IS3)

            If i = 1 Then
                ' CalcRelPoseAndXYZ(IS1, IS2)
                'IS2.ImagePose.Pose = IS2.MidPose.RelPose
                IS2.ImagePose.Pose = IS2.VectorPose.Pose

                IS2.CommonHomMat3d = ConnectHomMat3d
                IS2.CommonScale = 1
                CalcMidXYZ_byRelPose(IS1, IS2, IS2.VectorPose.Pose, Nothing)
                WorldXYZ.X = BTuple.TupleConcat(WorldXYZ.X, IS2.MidPose.X)
                WorldXYZ.Y = BTuple.TupleConcat(WorldXYZ.Y, IS2.MidPose.Y)
                WorldXYZ.Z = BTuple.TupleConcat(WorldXYZ.Z, IS2.MidPose.Z)
            End If

            'CalcRelPoseAndXYZ(IS2, IS3)

            CalcMidXYZ_byRelPose(IS1, IS2, IS2.VectorPose.Pose, Nothing)
            CalcMidXYZ_byRelPose(IS2, IS3, IS2.VectorPose.RelPose, Nothing)

            '  GetScaleFromMidPoints1(IS2, IS3, Scale, BadIndex)
            'IS2.Scale = Scale

            IS2.MidPose.SetScale(commonscale)

            commonscale = commonscale * IS3.Scale '1.03549
            IS3.MidPose.RelPose = IS2.VectorPose.RelPose
            CalcConnectedPose(IS2, IS3, commonscale)
            '  CalcMidXYZ_byRelPose(IS2, IS3, IS3.MidPose.RelPose)
            IS3.MidPose.SetScale(commonscale)

            hom_mat_3d_from_3d_3d_point_correspondence(IS3.MidPose.X, IS3.MidPose.Y, IS3.MidPose.Z, IS2.MidPose.X, IS2.MidPose.Y, IS2.MidPose.Z, homMat3d)
            HOperatorSet.AffineTransPoint3d(homMat3d, IS3.MidPose.X, IS3.MidPose.Y, IS3.MidPose.Z, testMidPose.X, testMidPose.Y, testMidPose.Z)
            tmpWPoint = IS2.MidPose.ConvertToPoint3d
            tmpWPoint.GetDisttoOtherPose(testMidPose, Result1)

            'HOperatorSet.PoseToHomMat3d(IS1.ImagePose.Pose, ConnectHomMat3d)
            ' HOperatorSet.HomMat3dToPose(ConnectHomMat3d, IS1.ImagePose.Pose)
            HOperatorSet.HomMat3dCompose(ConnectHomMat3d, homMat3d, TempHomMat3d)
            ConnectHomMat3d = TempHomMat3d
            ' HOperatorSet.HomMat3dToPose(ConnectHomMat3d, IS2.ImagePose.Pose)
            HOperatorSet.AffineTransPoint3d(ConnectHomMat3d, IS3.MidPose.X, IS3.MidPose.Y, IS3.MidPose.Z, testMidPose.X, testMidPose.Y, testMidPose.Z)
            WorldXYZ.X = BTuple.TupleConcat(WorldXYZ.X, testMidPose.X)
            WorldXYZ.Y = BTuple.TupleConcat(WorldXYZ.Y, testMidPose.Y)
            WorldXYZ.Z = BTuple.TupleConcat(WorldXYZ.Z, testMidPose.Z)

            IS3.ImagePose.SetScale(commonscale)
            HOperatorSet.AffineTransPoint3d(ConnectHomMat3d, IS3.ImagePose.X, IS3.ImagePose.Y, IS3.ImagePose.Z, tmpWPoint.X, tmpWPoint.Y, tmpWPoint.Z)
            IS3.ImagePose.X = tmpWPoint.X
            IS3.ImagePose.Y = tmpWPoint.Y
            IS3.ImagePose.Z = tmpWPoint.Z

            IS3.CommonHomMat3d = ConnectHomMat3d
            IS3.CommonScale = commonscale
        Next

    End Sub
    'バンドル調整（B A）：再投影誤差を最小化する
    Public Sub RunBA_New(ByVal intF0 As Double, ByVal flgCTandST As Boolean, ByRef CamParamFix As Object)
        TimeMonStart()

        Dim objBA As New BAlib.BAmain
        Dim i As Integer
        Dim j As Integer
        Dim ConImageIndex(lstImages.Count) As Integer
        Dim cntT As Integer = 0
        Dim cntTP As Integer = CodedTarget.CTnoSTnum
        If flgCTandST = True Then
            For Each C3DST As Common3DSingleTarget In lstCommon3dST
                If C3DST.PID = 357 Then
                    cntT = cntT
                End If
                If C3DST.PID <> -1 Then
                    cntT += 1
                End If
            Next
        End If
        For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
            If C3DCT.lstP3d.Count = 0 Then
                Continue For
            End If
            If C3DCT.lstCT.Count < 2 Then
                Continue For
            End If
            If C3DCT.lstP3d.Count = CodedTarget.CTnoSTnum And Not C3DCT.lstP3d.Item(0).X Is Nothing Then
                cntT += CodedTarget.CTnoSTnum
            End If
            If C3DCT.lstP3d.Item(0).X Is Nothing Then
                cntT = cntT
            End If

        Next

        cntConnectedImage = 0
        For Each ISI As ImageSet In lstImages
            If ISI.flgConnected = True Then
                cntConnectedImage += 1
            End If
        Next
        If cntConnectedImage < 2 Then
            Exit Sub

        End If
        ' objBA.SetInitData(hv_CamparamOut, lstImages.Count, lstCommon3dST.Count + lstCommon3dCT.Count * CodedTarget.CTnoSTnum, intF0, 1)

        '  objBA.SetInitData(CamParamFix, cntConnectedImage, cntT * CodedTarget.CTnoSTnum, intF0, 4)
        '   Dim blnSangyoCam As Integer = CInt(My.Computer.FileSystem.ReadAllText(My.Application.Info.DirectoryPath & "\KaisekiSetting.txt"))
        'SUURI DELL 20150120
        'If blnSangyoCam = 1 Then
        '    objBA.SetInitData(CamParamFix, cntConnectedImage, cntT, intF0, 1)
        'Else
        '    objBA.SetInitData(CamParamFix, cntConnectedImage, cntT, intF0, 4)
        'End If
        objBA.SetInitData(CamParamFix, cntConnectedImage, cntT, intF0, 4, lstCamParam.Count)
        objBA.ProjectPath = ProjectPath
        '内部パラメータリストをBALIBに渡す

        i = 1
        For Each objCamparam As CameraParam In lstCamParam
            objBA.objBAmain.CNP(i) = New BAlib.BAdata.CamNaibuParam
            objBA.objBAmain.CNP(i).SetDataByCamParam(objCamparam.Camparam)
            i += 1
        Next
        i = 1
        For Each ISI As ImageSet In lstImages
            If ISI.flgConnected = True Then
                If ISI.flgFirst = True Then
                    If ISI.flgSecond = True Then
                        objBA.objBAmain.SecondImageIndex = i
                        objBA.objBAmain.SecondImageXorY = XorY
                    Else
                        objBA.objBAmain.FirstImageIndex = i
                    End If

                End If
                'If ISI.ImageId = 6 Then
                '    ISI.ImageId = ISI.ImageId
                'End If
                'SUURI UPDATE 20150119
                Dim BA_Camdata As BAlib.CameraData
                'If blnSangyoCam = 1 Then
                '    BA_Camdata = New BAlib.CameraData(ISI.ImagePose.Pose, ISI.objCamparam.Camparam)
                'Else
                '    BA_Camdata = New BAlib.CameraData(ISI.ImagePose.Pose, objBA.objBAmain.CNP(i))
                'End If
                BA_Camdata = New BAlib.CameraData(ISI.ImagePose.Pose, ISI.objCamparam.Camparam)
                BA_Camdata.CID = ISI.objCamparam.Cid

                objBA.objBAmain.Images(i) = BA_Camdata
                ConImageIndex(ISI.ImageId) = i

                i += 1
            Else
                ConImageIndex(ISI.ImageId) = 0
            End If
        Next
        i = 1
        If flgCTandST = True Then

            For Each C3DST As Common3DSingleTarget In lstCommon3dST
                If C3DST.PID <> -1 Then
                    Dim BA_Points As New BAlib.Point3D(C3DST.P3d.X, C3DST.P3d.Y, C3DST.P3d.Z)
                    objBA.objBAmain.Points(i) = BA_Points
                    For Each ST As SingleTarget In C3DST.lstST
                        Dim BA_ImgPoints As New BAlib.Point2D(ST.P2D.Col, ST.P2D.Row)
                        j = ConImageIndex(ST.ImageID)
                        If j = 0 Then
                            j = j
                        End If
                        objBA.objBAmain.ImagePoints(i, j) = BA_ImgPoints
                        objBA.objBAmain.Imat(i, j) = 1
                    Next
                    i += 1
                End If
            Next

        End If
        For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
            Dim t As Integer
            If C3DCT.lstP3d.Count = 0 Then
                Continue For
            End If
            If C3DCT.lstCT.Count < 2 Then
                Continue For
            End If
            If C3DCT.lstP3d.Count = CodedTarget.CTnoSTnum And Not C3DCT.lstP3d.Item(0).X Is Nothing Then
                For t = 1 To cntTP
                    Dim BA_Points As New BAlib.Point3D(C3DCT.lstP3d.Item(t - 1).X, C3DCT.lstP3d.Item(t - 1).Y, C3DCT.lstP3d.Item(t - 1).Z)
                    objBA.objBAmain.Points(i) = BA_Points
                    For Each CT As CodedTarget In C3DCT.lstCT
                        If lstImages(CT.ImageID - 1).flgConnected = True Then
                            Dim ST As SingleTarget = CT.lstCTtoST.Item(t - 1)
                            'My.Computer.FileSystem.WriteAllText("XYMonitor", C3DCT.PID & "," & CT.ImageID & "," & ST.P2D.Col.D & "," & ST.P2D.Row.D & vbNewLine, True)
                            Dim BA_ImgPoints As New BAlib.Point2D(ST.P2D.Col, ST.P2D.Row)
                            j = ConImageIndex(ST.ImageID)
                            objBA.objBAmain.ImagePoints(i, j) = BA_ImgPoints
                            objBA.objBAmain.Imat(i, j) = 1
                        End If
                    Next
                    'If i = 50 Then
                    '    i = 50
                    'End If
                    i += 1
                Next
            End If
        Next
        If flgCTandST = True Then
            objBA.RunBA(True)
        Else
            objBA.RunBA(True)
        End If

        Dim strFile1 As String = My.Application.Info.DirectoryPath & "\ddEmat.csv"
        Dim strfile2 As String = My.Application.Info.DirectoryPath & "\dEmat.csv"
        'objBA.objBAmain.OutddEmat(strFile1)
        'objBA.objBAmain.OutdEmat(strfile2)
        i = 1
        For Each ISI As ImageSet In lstImages
            If ISI.flgConnected = True Then
                Dim tmpPose As Object = objBA.objBAmain.Images(i).GenPoseBy_R_to_T
                Dim diffPose As Object = Nothing
                diffPose = BTuple.TupleSub(tmpPose, ISI.ImagePose.Pose)
                Trace.WriteLine(BTuple.TupleSelect(diffPose, 0).D & ", " & BTuple.TupleSelect(diffPose, 1).D & ", " & BTuple.TupleSelect(diffPose, 2).D & ", " & BTuple.TupleSelect(diffPose, 3).D & ", " & BTuple.TupleSelect(diffPose, 4).D & ", " & BTuple.TupleSelect(diffPose, 5).D & ", ")
                ISI.ImagePose.Pose = tmpPose

                i += 1
            End If
        Next
        i = 1
        If flgCTandST = True Then
            For Each C3DST As Common3DSingleTarget In lstCommon3dST
                If C3DST.PID <> -1 Then
                    C3DST.P3d.X = objBA.objBAmain.Points(i).X
                    C3DST.P3d.Y = objBA.objBAmain.Points(i).Y
                    C3DST.P3d.Z = objBA.objBAmain.Points(i).Z
                    i += 1
                End If
            Next
        End If
        For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
            If C3DCT.lstP3d.Count = CodedTarget.CTnoSTnum And C3DCT.lstCT.Count > 1 Then
                Dim t As Integer
                For t = 0 To cntTP - 1
                    C3DCT.lstP3d.Item(t).X = objBA.objBAmain.Points(i).X
                    C3DCT.lstP3d.Item(t).Y = objBA.objBAmain.Points(i).Y
                    C3DCT.lstP3d.Item(t).Z = objBA.objBAmain.Points(i).Z
                    i += 1
                Next
            End If
        Next

        Dim hv_result As Object
        i = 1
        For Each objCamParam As CameraParam In lstCamParam
            Dim kekkaCamMat As New HTuple
            kekkaCamMat(0) = objBA.objBAmain.CNP(i).A * objBA.objBAmain.CNP(i).F
            kekkaCamMat(1) = 0
            kekkaCamMat(2) = objBA.objBAmain.CNP(i).U0
            kekkaCamMat(3) = 0
            kekkaCamMat(4) = objBA.objBAmain.CNP(i).F
            kekkaCamMat(5) = objBA.objBAmain.CNP(i).V0
            kekkaCamMat(6) = 0
            kekkaCamMat(7) = 0
            kekkaCamMat(8) = 1
            hv_result = BTuple.TupleMult(BTuple.TupleSelect(kekkaCamMat, 4), BTuple.TupleSelect(objCamParam.Camparam, 7))

            objCamParam.Camparam(0) = hv_result
            objCamParam.Camparam(1) = objBA.objBAmain.CNP(i).K1
            objCamParam.Camparam(2) = objBA.objBAmain.CNP(i).K2
            objCamParam.Camparam(3) = objBA.objBAmain.CNP(i).K3
            objCamParam.Camparam(4) = objBA.objBAmain.CNP(i).P1
            objCamParam.Camparam(5) = objBA.objBAmain.CNP(i).P2

            objCamParam.Camparam(8) = BTuple.TupleSelect(kekkaCamMat, 2)
            objCamParam.Camparam(9) = BTuple.TupleSelect(kekkaCamMat, 5)


            i += 1
        Next

        '20150216 Rep By Suuri Sta----新キャリブレーション対応機能
        CamParamFix(0) = lstCamParam.Item(0).Camparam(0)
        CamParamFix(1) = lstCamParam.Item(0).Camparam(1)
        CamParamFix(2) = lstCamParam.Item(0).Camparam(2)
        CamParamFix(3) = lstCamParam.Item(0).Camparam(3)
        CamParamFix(4) = lstCamParam.Item(0).Camparam(4)
        CamParamFix(5) = lstCamParam.Item(0).Camparam(5)
        'CamParamFix(6) = BTuple.TupleSelect(hv_CamparamOut, 6)
        'CamParamFix(7) = BTuple.TupleSelect(hv_CamparamOut, 7)
        CamParamFix(8) = lstCamParam.Item(0).Camparam(8)
        CamParamFix(9) = lstCamParam.Item(0).Camparam(9)
        '20150216 Rep By Suuri End----新キャリブレーション対応機能

        ''hv_result = BTuple.TupleMult(BTuple.TupleSelect(objBA.kekkaCamMat, 4), BTuple.TupleSelect(hv_CamparamOut, 7))

        ' ''CamParamFix = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat( _
        ' ''    hv_result, BTuple.TupleSelectRange(hv_CamparamOut, 1, 7)), BTuple.TupleSelect(objBA.kekkaCamMat, _
        ' ''    2)), BTuple.TupleSelect(objBA.kekkaCamMat, 5)), BTuple.TupleSelectRange(hv_CamparamOut, _
        ' ''    10, 11))
        ''CamParamFix(0) = hv_result
        ''CamParamFix(1) = objBA.objBAmain.CNP.K1
        ''CamParamFix(2) = objBA.objBAmain.CNP.K2
        ''CamParamFix(3) = objBA.objBAmain.CNP.K3
        ''CamParamFix(4) = objBA.objBAmain.CNP.P1
        ''CamParamFix(5) = objBA.objBAmain.CNP.P2
        ' ''CamParamFix(6) = BTuple.TupleSelect(hv_CamparamOut, 6)
        ' ''CamParamFix(7) = BTuple.TupleSelect(hv_CamparamOut, 7)
        ''CamParamFix(8) = BTuple.TupleSelect(objBA.kekkaCamMat, 2)
        ''CamParamFix(9) = BTuple.TupleSelect(objBA.kekkaCamMat, 5)
        ' ''CamParamFix(10) = BTuple.TupleSelect(objBA.kekkaCamMat, 5)
        ' ''CamParamFix(11) = BTuple.TupleSelect(objBA.kekkaCamMat, 5)

        TimeMonEnd()
    End Sub


    Public Function RunBA_Totyu(ByVal intF0 As Double, ByRef CamParamFix As Object, ByRef dblE As Double) As Boolean
        Dim objBA As New BAlib.BAmain
        Dim i As Integer
        Dim j As Integer
        Dim ConImageIndex(lstImages.Count) As Integer
        Dim cntT As Integer = 0
        Dim cntTP As Integer = 1 'CodedTarget.CTnoSTnum
        For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
            If C3DCT.lstP3d.Count = CodedTarget.CTnoSTnum Then
                cntT += 1
            End If
        Next
        ' objBA.SetInitData(hv_CamparamOut, lstImages.Count, lstCommon3dST.Count + lstCommon3dCT.Count * CodedTarget.CTnoSTnum, intF0, 1)

        '  objBA.SetInitData(CamParamFix, cntConnectedImage, cntT * CodedTarget.CTnoSTnum, intF0, 4)
        objBA.SetInitData(CamParamFix, cntConnectedImage, cntT * cntTP, intF0, 4, 1)
        objBA.ProjectPath = ProjectPath
        i = 1
        For Each ISI As ImageSet In lstImages
            If ISI.flgConnected = True Then
                If ISI.flgFirst = True Then
                    If ISI.flgSecond = True Then
                        objBA.objBAmain.SecondImageIndex = i
                        objBA.objBAmain.SecondImageXorY = XorY
                    Else
                        objBA.objBAmain.FirstImageIndex = i
                    End If

                End If

                Dim BA_Camdata As New BAlib.CameraData(ISI.ImagePose.Pose, objBA.objBAmain.CNP)
                objBA.objBAmain.Images(i) = BA_Camdata
                ConImageIndex(ISI.ImageId) = i
                i += 1
            Else
                ConImageIndex(ISI.ImageId) = 0
            End If
        Next
        i = 1
        'For Each C3DST As Common3DSingleTarget In lstCommon3dST
        '    Dim BA_Points As New BAlib.Point3D(C3DST.P3d.X, C3DST.P3d.Y, C3DST.P3d.Z)
        '    objBA.objBAmain.Points(i) = BA_Points
        '    For Each ST As SingleTarget In C3DST.lstST
        '        Dim BA_ImgPoints As New BAlib.Point2D(ST.P2D.Col, ST.P2D.Row)
        '        j = ST.ImageID
        '        objBA.objBAmain.ImagePoints(i, j) = BA_ImgPoints
        '        objBA.objBAmain.Imat(i, j) = 1
        '    Next
        '    i += 1

        'Next

        For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
            Dim t As Integer
            If C3DCT.lstP3d.Count = CodedTarget.CTnoSTnum Then
                For t = 1 To cntTP
                    Dim BA_Points As New BAlib.Point3D(C3DCT.lstP3d.Item(t - 1).X, C3DCT.lstP3d.Item(t - 1).Y, C3DCT.lstP3d.Item(t - 1).Z)
                    objBA.objBAmain.Points(i) = BA_Points
                    For Each CT As CodedTarget In C3DCT.lstCT
                        If lstImages(CT.ImageID - 1).flgConnected = True Then
                            Dim ST As SingleTarget = CT.lstCTtoST.Item(t - 1)
                            Dim BA_ImgPoints As New BAlib.Point2D(ST.P2D.Col, ST.P2D.Row)
                            j = ConImageIndex(ST.ImageID)
                            objBA.objBAmain.ImagePoints(i, j) = BA_ImgPoints
                            objBA.objBAmain.Imat(i, j) = 1
                        End If
                    Next
                    i += 1
                Next
            End If
        Next

        objBA.RunBA_3DPoint_Pose()

        If objBA.objBAmain.dblE > dblE * 10 Then

            Return False
        Else
            dblE = objBA.objBAmain.dblE
        End If
        Dim strFile1 As String = My.Application.Info.DirectoryPath & "\ddEmat.csv"
        Dim strfile2 As String = My.Application.Info.DirectoryPath & "\dEmat.csv"
        'objBA.objBAmain.OutddEmat(strFile1)
        'objBA.objBAmain.OutdEmat(strfile2)
        i = 1
        For Each ISI As ImageSet In lstImages
            If ISI.flgConnected = True Then
                Dim tmpPose As Object = objBA.objBAmain.Images(i).GenPoseBy_R_to_T
                Dim diffPose As Object = Nothing
                diffPose = BTuple.TupleSub(tmpPose, ISI.ImagePose.Pose)
                Trace.WriteLine(diffPose(0) & ", " & diffPose(1) & ", " & diffPose(2) & ", " & diffPose(3) & ", " & diffPose(4) & ", " & diffPose(5) & ", ")
                ISI.ImagePose.Pose = tmpPose

                i += 1
            End If
        Next
        i = 1
        'For Each C3DST As Common3DSingleTarget In lstCommon3dST
        '    C3DST.P3d.X = objBA.objBAmain.Points(i).X
        '    C3DST.P3d.Y = objBA.objBAmain.Points(i).Y
        '    C3DST.P3d.Z = objBA.objBAmain.Points(i).Z
        '    i += 1
        'Next
        For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
            If C3DCT.lstP3d.Count = CodedTarget.CTnoSTnum Then
                Dim t As Integer
                For t = 0 To cntTP - 1
                    C3DCT.lstP3d.Item(t).X = objBA.objBAmain.Points(i).X
                    C3DCT.lstP3d.Item(t).Y = objBA.objBAmain.Points(i).Y
                    C3DCT.lstP3d.Item(t).Z = objBA.objBAmain.Points(i).Z
                    i += 1
                Next
            End If
        Next

        Return True
    End Function

    'Public Sub RunBA()
    '    Dim objBA As New BundleAdjustmentLib.BundleLib
    '    Dim X As New Object
    '    Dim Y As New Object
    '    Dim Z As New Object
    '    Dim i As Integer
    '    Dim Cam1 As New Object
    '    Dim Row As New Object
    '    Dim Col As New Object
    '    Dim QX As New Object
    '    Dim QY As New Object

    '    Dim CamParam As New Object
    '    CamParam = hv_CamparamOut
    '    Dim tmpCamParam As New Object
    '    tmpCamParam = System.Nothing
    '    tmpCamParam = BTuple.TupleConcat(tmpCamParam, BTuple.TupleSelect(CamParam, 0))
    '    tmpCamParam = BTuple.TupleConcat(tmpCamParam, 0.0)
    '    tmpCamParam = BTuple.TupleConcat(tmpCamParam, 0.0)
    '    tmpCamParam = BTuple.TupleConcat(tmpCamParam, 0.0)
    '    tmpCamParam = BTuple.TupleConcat(tmpCamParam, 0.0)
    '    tmpCamParam = BTuple.TupleConcat(tmpCamParam, 0.0)
    '    tmpCamParam = BTuple.TupleConcat(tmpCamParam, BTuple.TupleSelect(CamParam, 6))
    '    tmpCamParam = BTuple.TupleConcat(tmpCamParam, BTuple.TupleSelect(CamParam, 7))
    '    tmpCamParam = BTuple.TupleConcat(tmpCamParam, CDbl(BTuple.TupleSelect(CamParam, 10)) / 2)
    '    tmpCamParam = BTuple.TupleConcat(tmpCamParam, CDbl(BTuple.TupleSelect(CamParam, 11)) / 2)
    '    tmpCamParam = BTuple.TupleConcat(tmpCamParam, BTuple.TupleSelect(CamParam, 10))
    '    tmpCamParam = BTuple.TupleConcat(tmpCamParam, BTuple.TupleSelect(CamParam, 11))

    '    objBA.ImageNum = lstImages.Count
    '    objBA.PointNum = lstCommon3dST.Count + lstCommon3dCT.Count * CodedTarget.CTnoSTnum
    '    objBA.lstBundData = New List(Of BundleAdjustmentLib.BundleData)
    '    objBA.WeightValue = 0.000000000746020500624
    '    Dim int3DP_Index As Integer = 0
    '    Dim j As Integer = 1
    '    For Each C3DST As Common3DSingleTarget In lstCommon3dST
    '        If j Mod 1 = 0 Then
    '            For Each ST As SingleTarget In C3DST.lstST
    '                Cam1 = lstImages.Item(ST.ImageID - 1).ImagePose.Pose

    '                Dim itemBA1 As New BundleAdjustmentLib.BundleData
    '                Dim itemWP1 As New BundleAdjustmentLib.WorldPoint
    '                Dim itemIP1 As New BundleAdjustmentLib.ImagePoint
    '                Dim itemCam1 As New BundleAdjustmentLib.CameraParam
    '                Dim itemCam2 As New BundleAdjustmentLib.CameraParam
    '                SetData(itemCam1, CamParam, CamParam, Cam1)
    '                SetData(itemCam2, CamParam, CamParam, Cam1)
    '                Row = ST.P2D.Row
    '                Col = ST.P2D.Col
    '                HOperatorSet.GetLineOfSight(Row, Col, CamParam, Nothing, Nothing, Nothing, QX, QY, Nothing)
    '                itemIP1.U = QX
    '                itemIP1.V = QY
    '                itemWP1.X = C3DST.P3d.X
    '                itemWP1.Y = C3DST.P3d.Y
    '                itemWP1.Z = C3DST.P3d.Z
    '                itemBA1.CamObj = itemCam1
    '                itemBA1.FirstCamObj = itemCam2
    '                itemBA1.ImgObj = itemIP1
    '                itemBA1.WorldPointObj = itemWP1
    '                itemBA1.ImageIndex = ST.ImageID - 1
    '                itemBA1.PointIndex = int3DP_Index
    '                objBA.lstBundData.Add(itemBA1)
    '            Next
    '            int3DP_Index += 1
    '        End If
    '        j += 1
    '    Next


    '    For Each C3DCT As Common3DCodedTarget In lstCommon3dCT

    '        For Each CT As CodedTarget In C3DCT.lstCT
    '            i = 0
    '            For Each ST As SingleTarget In CT.lstCTtoST
    '                Cam1 = lstImages.Item(CT.ImageID - 1).ImagePose.Pose
    '                Dim itemBA1 As New BundleAdjustmentLib.BundleData
    '                Dim itemWP1 As New BundleAdjustmentLib.WorldPoint
    '                Dim itemIP1 As New BundleAdjustmentLib.ImagePoint
    '                Dim itemCam1 As New BundleAdjustmentLib.CameraParam

    '                SetData(itemCam1, CamParam, CamParam, Cam1)
    '                Row = ST.P2D.Row
    '                Col = ST.P2D.Col
    '                HOperatorSet.GetLineOfSight(Row, Col, CamParam, Nothing, Nothing, Nothing, QX, QY, Nothing)
    '                itemIP1.U = QX
    '                itemIP1.V = QY
    '                itemWP1.X = C3DCT.lstP3d.Item(i).X
    '                itemWP1.Y = C3DCT.lstP3d.Item(i).Y
    '                itemWP1.Z = C3DCT.lstP3d.Item(i).Z
    '                itemBA1.CamObj = itemCam1
    '                itemBA1.ImgObj = itemIP1
    '                itemBA1.WorldPointObj = itemWP1
    '                itemBA1.ImageIndex = CT.ImageID - 1
    '                itemBA1.PointIndex = int3DP_Index + i
    '                objBA.lstBundData.Add(itemBA1)
    '                i += 1
    '            Next
    '        Next
    '        int3DP_Index += CodedTarget.CTnoSTnum
    '    Next
    '    objBA.PointNum = int3DP_Index

    '    objBA.RunBundleAdj()
    '    i = 0
    '    j = 1
    '    For Each C3DST As Common3DSingleTarget In lstCommon3dST
    '        If j Mod 1 = 0 Then
    '            For Each ST As SingleTarget In C3DST.lstST
    '                Dim itemBA1 As BundleAdjustmentLib.BundleData
    '                itemBA1 = objBA.lstBundData.Item(i)
    '                C3DST.P3d.X = itemBA1.WorldPointObj.X
    '                C3DST.P3d.Y = itemBA1.WorldPointObj.Y
    '                C3DST.P3d.Z = itemBA1.WorldPointObj.Z
    '                Dim tmpPose As Object = Nothing
    '                HOperatorSet.CreatePose(itemBA1.CamObj.X0, itemBA1.CamObj.Y0, itemBA1.CamObj.Z0, _
    '                   BTuple.TupleDeg(itemBA1.CamObj.A0), BTuple.TupleDeg(itemBA1.CamObj.B0), _
    '                   BTuple.TupleDeg(itemBA1.CamObj.G0), "Rp+T", "gba", "point", tmpPose)
    '                lstImages.Item(ST.ImageID - 1).ImagePose.Pose = tmpPose
    '                i += 1

    '            Next
    '        End If
    '        j += 1
    '    Next

    'End Sub

    'Private Sub RunBA(ByRef IS1 As ImageSet, ByRef IS2 As ImageSet, ByRef IS3 As ImageSet)
    '    Dim objBA As New BundleAdjustmentLib.BundleLib
    '    Dim Row1 As New Object
    '    Dim Col1 As New Object
    '    Dim Row2 As New Object
    '    Dim Col2 As New Object
    '    Dim Row3 As New Object
    '    Dim Col3 As New Object
    '    Dim Cam1 As New Object
    '    Dim Cam2 As New Object
    '    Dim Cam3 As New Object

    '    Dim X As New Object
    '    Dim Y As New Object
    '    Dim Z As New Object
    '    Dim i As Integer

    '    Dim CamParam As New Object
    '    CamParam = hv_CamparamOut
    '    Dim tmpCamParam As New Object
    '    tmpCamParam = System.Nothing
    '    tmpCamParam = BTuple.TupleConcat(tmpCamParam, BTuple.TupleSelect(CamParam, 0))
    '    tmpCamParam = BTuple.TupleConcat(tmpCamParam, 0.0)
    '    tmpCamParam = BTuple.TupleConcat(tmpCamParam, 0.0)
    '    tmpCamParam = BTuple.TupleConcat(tmpCamParam, 0.0)
    '    tmpCamParam = BTuple.TupleConcat(tmpCamParam, 0.0)
    '    tmpCamParam = BTuple.TupleConcat(tmpCamParam, 0.0)
    '    tmpCamParam = BTuple.TupleConcat(tmpCamParam, BTuple.TupleSelect(CamParam, 6))
    '    tmpCamParam = BTuple.TupleConcat(tmpCamParam, BTuple.TupleSelect(CamParam, 7))
    '    tmpCamParam = BTuple.TupleConcat(tmpCamParam, CDbl(BTuple.TupleSelect(CamParam, 11)) / 2)
    '    tmpCamParam = BTuple.TupleConcat(tmpCamParam, CDbl(BTuple.TupleSelect(CamParam, 11)) / 2)
    '    tmpCamParam = BTuple.TupleConcat(tmpCamParam, BTuple.TupleSelect(CamParam, 10))
    '    tmpCamParam = BTuple.TupleConcat(tmpCamParam, BTuple.TupleSelect(CamParam, 11))

    '    objBA.ImageNum = 3
    '    objBA.PointNum = CInt(IS2.MidPose.CountXYZ)
    '    objBA.lstBundData = New List(Of BundleAdjustmentLib.BundleData)

    '    Dim Row As New Object
    '    Dim Col As New Object
    '    Dim QX As New Object
    '    Dim QY As New Object
    '    IS2.ImagePose.Pose = IS2.ImagePose.RelPose

    '    CalcConnectedPose(IS2, IS3, IS3.Scale)
    '    HOperatorSet.CreatePose(0, 0, 0, 0, 0, 0, "Rp+T", "gba", "point", Cam1)
    '    Cam2 = IS2.ImagePose.Pose

    '    Cam3 = IS3.ImagePose.Pose

    '    For i = 0 To CInt(IS2.MidPose.CountXYZ) - 1
    '        Dim itemBA1 As New BundleAdjustmentLib.BundleData
    '        Dim itemBA2 As New BundleAdjustmentLib.BundleData
    '        Dim itemBA3 As New BundleAdjustmentLib.BundleData

    '        Dim itemWP1 As New BundleAdjustmentLib.WorldPoint
    '        Dim itemIP1 As New BundleAdjustmentLib.ImagePoint
    '        Dim itemWP2 As New BundleAdjustmentLib.WorldPoint
    '        Dim itemIP2 As New BundleAdjustmentLib.ImagePoint
    '        Dim itemWP3 As New BundleAdjustmentLib.WorldPoint
    '        Dim itemIP3 As New BundleAdjustmentLib.ImagePoint
    '        Dim itemCam1 As New BundleAdjustmentLib.CameraParam
    '        Dim itemCam2 As New BundleAdjustmentLib.CameraParam
    '        Dim itemCam3 As New BundleAdjustmentLib.CameraParam

    '        SetData(itemCam1, CamParam, tmpCamParam, Cam1)
    '        SetData(itemCam2, CamParam, tmpCamParam, Cam2)
    '        SetData(itemCam3, CamParam, tmpCamParam, Cam3)

    '        Row = BTuple.TupleSelect(IS1.RansacMid.RansacPoints.Row, i)
    '        Col = BTuple.TupleSelect(IS1.RansacMid.RansacPoints.Col, i)
    '        HOperatorSet.GetLineOfSight(Row, Col, tmpCamParam, Nothing, Nothing, Nothing, QX, QY, Nothing)
    '        itemIP1.U = QX
    '        itemIP1.V = QY

    '        itemWP1.X = BTuple.TupleSelect(IS2.MidPose.X, i)
    '        itemWP1.Y = BTuple.TupleSelect(IS2.MidPose.Y, i)
    '        itemWP1.Z = BTuple.TupleSelect(IS2.MidPose.Z, i)

    '        itemBA1.CamObj = itemCam1
    '        itemBA1.ImgObj = itemIP1
    '        itemBA1.WorldPointObj = itemWP1

    '        objBA.lstBundData.Add(itemBA1)

    '        Row = BTuple.TupleSelect(IS2.RansacMid.RansacPoints.Row, i)
    '        Col = BTuple.TupleSelect(IS2.RansacMid.RansacPoints.Col, i)
    '        HOperatorSet.GetLineOfSight(Row, Col, tmpCamParam, Nothing, Nothing, Nothing, QX, QY, Nothing)
    '        itemIP2.U = QX
    '        itemIP2.V = QY

    '        itemWP2.X = BTuple.TupleSelect(IS2.MidPose.X, i)
    '        itemWP2.Y = BTuple.TupleSelect(IS2.MidPose.Y, i)
    '        itemWP2.Z = BTuple.TupleSelect(IS2.MidPose.Z, i)

    '        itemBA2.CamObj = itemCam2
    '        itemBA2.ImgObj = itemIP2
    '        itemBA2.WorldPointObj = itemWP2

    '        objBA.lstBundData.Add(itemBA2)

    '        Row = BTuple.TupleSelect(IS3.RansacMid.RansacPoints.Row, i)
    '        Col = BTuple.TupleSelect(IS3.RansacMid.RansacPoints.Col, i)
    '        HOperatorSet.GetLineOfSight(Row, Col, tmpCamParam, Nothing, Nothing, Nothing, QX, QY, Nothing)
    '        itemIP3.U = QX
    '        itemIP3.V = QY

    '        itemWP3.X = BTuple.TupleSelect(IS2.MidPose.X, i)
    '        itemWP3.Y = BTuple.TupleSelect(IS2.MidPose.Y, i)
    '        itemWP3.Z = BTuple.TupleSelect(IS2.MidPose.Z, i)

    '        itemBA3.CamObj = itemCam3
    '        itemBA3.ImgObj = itemIP3
    '        itemBA3.WorldPointObj = itemWP3

    '        objBA.lstBundData.Add(itemBA3)

    '    Next

    '    objBA.RunBundleAdj()
    '    Dim ind As Integer = 1
    '    HOperatorSet.CreatePose(objBA.lstBundData.Item(ind).CamObj.X0, objBA.lstBundData.Item(ind).CamObj.Y0, objBA.lstBundData.Item(ind).CamObj.Z0, _
    '                 BTuple.TupleDeg(objBA.lstBundData.Item(ind).CamObj.A0), BTuple.TupleDeg(objBA.lstBundData.Item(ind).CamObj.B0), _
    '                 BTuple.TupleDeg(objBA.lstBundData.Item(ind).CamObj.G0), "Rp+T", "gba", "point", IS2.MidPose.Pose)

    '    ind = 2
    '    HOperatorSet.CreatePose(objBA.lstBundData.Item(ind).CamObj.X0, objBA.lstBundData.Item(ind).CamObj.Y0, objBA.lstBundData.Item(ind).CamObj.Z0, _
    '                   BTuple.TupleDeg(objBA.lstBundData.Item(ind).CamObj.A0), BTuple.TupleDeg(objBA.lstBundData.Item(ind).CamObj.B0), _
    '                   BTuple.TupleDeg(objBA.lstBundData.Item(ind).CamObj.G0), "Rp+T", "gba", "point", IS3.MidPose.Pose)
    '    IS2.MidPose.RelPose = IS2.MidPose.Pose
    '    CalcRelPoseBetweenTwoPose(IS2.MidPose.Pose, IS3.MidPose.Pose, IS3.MidPose.RelPose)


    '    Dim tmpScale As New Object
    '    tmpScale = BTuple.TupleSqrt(BTuple.TupleSum(BTuple.TupleSelectRange(BTuple.TuplePow(BTuple.TupleSub(IS2.MidPose.Pose, IS3.MidPose.Pose), 2), 0, 2)))
    '    IS3.ScaleBA = tmpScale
    '    Dim tX As New Object
    '    Dim tY As New Object
    '    Dim tZ As New Object
    '    tX = System.Nothing
    '    tY = System.Nothing
    '    tZ = System.Nothing
    '    For i = 0 To objBA.PointNum - 1
    '        tX = BTuple.TupleConcat(tX, objBA.lstBundData.Item(i * 3).WorldPointObj.X)
    '        tY = BTuple.TupleConcat(tY, objBA.lstBundData.Item(i * 3).WorldPointObj.Y)
    '        tZ = BTuple.TupleConcat(tZ, objBA.lstBundData.Item(i * 3).WorldPointObj.Z)
    '    Next
    '    IS2.MidPose.X = tX
    '    IS2.MidPose.Y = tY
    '    IS2.MidPose.Z = tZ
    '    Dim Dist12 As New Object
    '    Dim Dist13 As New Object
    '    Dim Dist123 As New Object
    '    HOperatorSet.IntersectLinesOfSight(hv_CamparamOut, hv_CamparamOut, IS2.MidPose.Pose, _
    '                             IS1.RansacMid.RansacPoints.Row, IS1.RansacMid.RansacPoints.Col, _
    '                              IS2.RansacMid.RansacPoints.Row, IS2.RansacMid.RansacPoints.Col, _
    '                              IS2.MidPose.X, IS2.MidPose.Y, IS2.MidPose.Z, Dist12)
    '    HOperatorSet.IntersectLinesOfSight(hv_CamparamOut, hv_CamparamOut, IS3.MidPose.Pose, _
    '                            IS1.RansacMid.RansacPoints.Row, IS1.RansacMid.RansacPoints.Col, _
    '                             IS3.RansacMid.RansacPoints.Row, IS3.RansacMid.RansacPoints.Col, _
    '                             IS3.MidPose.X, IS3.MidPose.Y, IS3.MidPose.Z, Dist13)

    '    IS2.MidPose.ConvertToPoint3d.GetDisttoOtherPose(IS3.MidPose.ConvertToPoint3d, Dist123)


    'End Sub


    'Private Sub SetData(ByRef itemCam As BundleAdjustmentLib.CameraParam, ByVal Camparam As Object, ByVal tmpCamparam As Object, ByVal CamPose As Object)


    '    itemCam.X0 = BTuple.TupleSelect(CamPose, 0)
    '    itemCam.Y0 = BTuple.TupleSelect(CamPose, 1)
    '    itemCam.Z0 = BTuple.TupleSelect(CamPose, 2)
    '    itemCam.A0 = BTuple.TupleSelect(CamPose, 3)
    '    itemCam.B0 = BTuple.TupleSelect(CamPose, 4)
    '    itemCam.G0 = BTuple.TupleSelect(CamPose, 5)

    '    itemCam.c = BTuple.TupleSelect(Camparam, 0)
    '    itemCam.k1 = BTuple.TupleSelect(Camparam, 1)
    '    itemCam.k2 = BTuple.TupleSelect(Camparam, 2)
    '    itemCam.k3 = BTuple.TupleSelect(Camparam, 3)
    '    itemCam.p1 = BTuple.TupleSelect(Camparam, 4)
    '    itemCam.p2 = BTuple.TupleSelect(Camparam, 5)
    '    Dim Qx As New Object
    '    Dim Qy As New Object
    '    HOperatorSet.GetLineOfSight(BTuple.TupleSelect(Camparam, 9), BTuple.TupleSelect(Camparam, 8), tmpCamparam, Nothing, Nothing, Nothing, Qx, Qy, Nothing)

    '    itemCam.xp = Qx
    '    itemCam.yp = (-1) * Qy

    'End Sub


    'Private Sub GetPoseData(ByRef objBA As BundleAdjustmentLib.BundleLib, ByRef MidPose As FBMlib.CameraPose)

    '    HOperatorSet.CreatePose(objBA.lstBundData.Item(1).CamObj.X0, objBA.lstBundData.Item(1).CamObj.Y0, objBA.lstBundData.Item(1).CamObj.Z0, _
    '                  BTuple.TupleDeg(objBA.lstBundData.Item(1).CamObj.A0), BTuple.TupleDeg(objBA.lstBundData.Item(1).CamObj.B0), _
    '                  BTuple.TupleDeg(objBA.lstBundData.Item(1).CamObj.G0), "Rp+T", "gba", "point", MidPose.RelPose)


    'End Sub
    'Private Sub GetXYZData(ByVal aa As BundleAdjustmentLib.WorldPoint, ByRef MidPose As FBMlib.CameraPose)

    'End Sub

    Private Sub GetMidPoints(ByRef IS1 As ImageSet, ByRef IS2 As ImageSet)
        Dim i As Integer
        Dim index As Integer
        Dim n As Integer = CInt(BTuple.TupleLength(IS1.RansacSecond.RansacPointsIndex))
        IS1.RansacMid.IndexClear()
        IS2.RansacMid.IndexClear()
        For i = 0 To n - 1
            index = BTuple.TupleGenConst(1, -1)
            index = BTuple.TupleFind(IS1.RansacFirst.RansacPointsIndex, BTuple.TupleSelect(IS1.RansacSecond.RansacPointsIndex, i))
            If BTuple.TupleNotEqual(index, -1) > 0 Then
                IS1.RansacMid.RansacPointsIndex = BTuple.TupleConcat(IS1.RansacMid.RansacPointsIndex, i)
                IS2.RansacMid.RansacPointsIndex = BTuple.TupleConcat(IS2.RansacMid.RansacPointsIndex, index)
            End If
        Next
        IS1.SubSetMid()
        IS2.SubSetMid()

    End Sub

    Private Sub GetMidPointsOf3Image(ByRef IS1 As ImageSet, ByRef IS2 As ImageSet, ByRef IS3 As ImageSet)
        Dim i As Integer
        Dim index As Integer
        Dim n As Integer = CInt(BTuple.TupleLength(IS2.RansacSecond.RansacPointsIndex))
        IS1.RansacMid.IndexClear()
        IS2.RansacMid.IndexClear()
        IS3.RansacMid.IndexClear()
        For i = 0 To n - 1
            index = BTuple.TupleGenConst(1, -1)
            index = BTuple.TupleFind(IS2.RansacFirst.RansacPointsIndex, BTuple.TupleSelect(IS2.RansacSecond.RansacPointsIndex, i))
            If BTuple.TupleNotEqual(index, -1) > 0 Then
                IS1.RansacMid.RansacPointsIndex = BTuple.TupleConcat(IS1.RansacMid.RansacPointsIndex, BTuple.TupleSelect(IS1.RansacFirst.RansacPointsIndex, i))
                IS2.RansacMid.RansacPointsIndex = BTuple.TupleConcat(IS2.RansacMid.RansacPointsIndex, BTuple.TupleSelect(IS2.RansacSecond.RansacPointsIndex, i))
                IS3.RansacMid.RansacPointsIndex = BTuple.TupleConcat(IS3.RansacMid.RansacPointsIndex, BTuple.TupleSelect(IS3.RansacSecond.RansacPointsIndex, index))

            End If
        Next

        IS1.RansacMid_SubSet()
        IS2.RansacMid_SubSet()
        IS3.RansacMid_SubSet()


    End Sub

    Public Function GetMidPointsOf3Image1(ByRef IS1 As ImageSet, ByRef IS2 As ImageSet, ByRef IS3 As ImageSet) As Integer
        On Error Resume Next
        Dim i As Integer
        Dim index As Integer

        Dim hv_Concat As Object = Nothing, hv_Sorted As Object = Nothing
        Dim hv_Concat1 As Object = Nothing, hv_Selected As Object = Nothing
        Dim hv_Diff As Object = Nothing, hv_Indices5 As Object = Nothing
        Dim hv_Selected1 As Object = Nothing, hv_Index As Object = Nothing
        Dim hv_Indices6 As Object = Nothing, hv_Indices7 As Object = Nothing
        Dim hv_Ind1 As Object = Nothing, hv_Ind2 As Object = Nothing

        Dim n As Integer = CInt(BTuple.TupleLength(IS2.RansacSecond.RansacPointsIndex))
        If n = 0 Then
            GetMidPointsOf3Image1 = 0
            Exit Function
        Else
            If CInt(BTuple.TupleLength(IS2.RansacFirst.RansacPointsIndex)) = 0 Then
                GetMidPointsOf3Image1 = 0
                Exit Function
            End If
        End If
        IS1.RansacMidFirst.IndexClear()
        IS2.RansacMidSecond.IndexClear()
        IS2.RansacMidFirst.IndexClear()
        IS3.RansacMidSecond.IndexClear()

        HOperatorSet.TupleConcat(IS2.RansacSecond.RansacPointsIndex, IS2.RansacFirst.RansacPointsIndex, hv_Concat)
        HOperatorSet.TupleSort(hv_Concat, hv_Sorted)
        HOperatorSet.TupleConcat(hv_Sorted, BTuple.TupleSelect(hv_Sorted, 0), hv_Concat1)
        HOperatorSet.TupleLastN(hv_Concat1, 1, hv_Selected)
        HOperatorSet.TupleSub(hv_Sorted, hv_Selected, hv_Diff)
        HOperatorSet.TupleFind(hv_Diff, 0, hv_Indices5)
        HOperatorSet.TupleSelect(hv_Sorted, hv_Indices5, hv_Selected1)
        For hv_Index = 0 To BTuple.TupleSub(BTuple.TupleLength(hv_Selected1), 1) Step 1
            HOperatorSet.TupleFind(IS2.RansacSecond.RansacPointsIndex, BTuple.TupleSelect(hv_Selected1, hv_Index), hv_Indices6)
            HOperatorSet.TupleFind(IS2.RansacFirst.RansacPointsIndex, BTuple.TupleSelect(hv_Selected1, hv_Index), hv_Indices7)
            hv_Ind1 = BTuple.TupleConcat(hv_Ind1, hv_Indices6)
            hv_Ind2 = BTuple.TupleConcat(hv_Ind2, hv_Indices7)

            'IS1.RansacMidFirst.RansacPointsIndex = BTuple.TupleConcat(IS1.RansacMidFirst.RansacPointsIndex, hv_Indices6)
            'IS2.RansacMidSecond.RansacPointsIndex = BTuple.TupleConcat(IS2.RansacMidSecond.RansacPointsIndex, hv_Indices6)
            'IS2.RansacMidFirst.RansacPointsIndex = BTuple.TupleConcat(IS2.RansacMidFirst.RansacPointsIndex, hv_Indices7)
            'IS3.RansacMidSecond.RansacPointsIndex = BTuple.TupleConcat(IS3.RansacMidSecond.RansacPointsIndex, hv_Indices7)
        Next

        IS1.RansacMidFirst.RansacPointsIndex = hv_Ind1
        IS2.RansacMidSecond.RansacPointsIndex = hv_Ind1
        IS2.RansacMidFirst.RansacPointsIndex = hv_Ind2
        IS3.RansacMidSecond.RansacPointsIndex = hv_Ind2

        'For i = 0 To n - 1
        '    index = BTuple.TupleGenConst(1, -1)
        '    index = BTuple.TupleFind(IS2.RansacFirst.RansacPointsIndex, BTuple.TupleSelect(IS2.RansacSecond.RansacPointsIndex, i))
        '    If BTuple.TupleNotEqual(index, -1) > 0 Then
        '        IS1.RansacMidFirst.RansacPointsIndex = BTuple.TupleConcat(IS1.RansacMidFirst.RansacPointsIndex, i)
        '        IS2.RansacMidSecond.RansacPointsIndex = BTuple.TupleConcat(IS2.RansacMidSecond.RansacPointsIndex, i)
        '        IS2.RansacMidFirst.RansacPointsIndex = BTuple.TupleConcat(IS2.RansacMidFirst.RansacPointsIndex, index)
        '        IS3.RansacMidSecond.RansacPointsIndex = BTuple.TupleConcat(IS3.RansacMidSecond.RansacPointsIndex, index)
        '    End If
        'Next
        IS1.RansacMid.RansacPointsIndex = BTuple.TupleSelect(IS1.RansacFirst.RansacPointsIndex, IS1.RansacMidFirst.RansacPointsIndex)
        IS2.RansacMid.RansacPointsIndex = BTuple.TupleSelect(IS2.RansacFirst.RansacPointsIndex, IS2.RansacMidFirst.RansacPointsIndex)
        IS3.RansacMid.RansacPointsIndex = BTuple.TupleSelect(IS3.RansacSecond.RansacPointsIndex, IS3.RansacMidSecond.RansacPointsIndex)
        IS1.RansacMid_SubSet()
        IS2.RansacMid_SubSet()
        IS3.RansacMid_SubSet()
        IS3.Ransac3ImagePoints(0) = New RansacPoints
        IS3.Ransac3ImagePoints(1) = New RansacPoints
        IS3.Ransac3ImagePoints(2) = New RansacPoints
        HOperatorSet.ClearObj(IS3.Ransac3ImagePoints(0).hx_RansacCross)
        HOperatorSet.ClearObj(IS3.Ransac3ImagePoints(1).hx_RansacCross)
        HOperatorSet.ClearObj(IS3.Ransac3ImagePoints(2).hx_RansacCross)
        IS3.Ransac3ImagePoints(0).hx_RansacCross = IS1.RansacMid.GetCross
        IS3.Ransac3ImagePoints(1).hx_RansacCross = IS2.RansacMid.GetCross
        IS3.Ransac3ImagePoints(2).hx_RansacCross = IS3.RansacMid.GetCross
        GetMidPointsOf3Image1 = BTuple.TupleLength(IS1.RansacMid.RansacPointsIndex)

    End Function

    Private Function RemoveBadPoints(ByRef IS1 As ImageSet, ByRef IS2 As ImageSet, ByRef IS3 As ImageSet, ByVal BadIndex As Object) As Integer
        Dim Result1 As New Object
        Dim Result2 As New Object

        If BTuple.TupleLength(BadIndex) > 0 Then

            Result1 = BTuple.TupleSelect(IS2.RansacMidSecond.RansacPointsIndex, BadIndex)
            Result2 = BTuple.TupleSelect(IS2.RansacMidFirst.RansacPointsIndex, BadIndex)
            IS1.RansacFirst.RansacPointsIndex = BTuple.TupleRemove(IS1.RansacFirst.RansacPointsIndex, Result1)
            IS2.RansacSecond.RansacPointsIndex = BTuple.TupleRemove(IS2.RansacSecond.RansacPointsIndex, Result1)
            IS2.RansacFirst.RansacPointsIndex = BTuple.TupleRemove(IS2.RansacFirst.RansacPointsIndex, Result2)
            IS3.RansacSecond.RansacPointsIndex = BTuple.TupleRemove(IS3.RansacSecond.RansacPointsIndex, Result2)

            CalcRelPoseAndXYZ(IS1, IS2)

            CalcRelPoseAndXYZ(IS2, IS3)
            IS1.RansacMid.RansacPointsIndex = BTuple.TupleRemove(IS1.RansacMid.RansacPointsIndex, BadIndex)
            IS2.RansacMid.RansacPointsIndex = BTuple.TupleRemove(IS2.RansacMid.RansacPointsIndex, BadIndex)
            IS3.RansacMid.RansacPointsIndex = BTuple.TupleRemove(IS3.RansacMid.RansacPointsIndex, BadIndex)
            IS1.RansacMid_SubSet()
            IS2.RansacMid_SubSet()
            IS3.RansacMid_SubSet()

            IS3.Ransac3ImagePoints(0) = New RansacPoints
            IS3.Ransac3ImagePoints(1) = New RansacPoints
            IS3.Ransac3ImagePoints(2) = New RansacPoints
            HOperatorSet.ClearObj(IS3.Ransac3ImagePoints(0).hx_RansacCross)
            HOperatorSet.ClearObj(IS3.Ransac3ImagePoints(1).hx_RansacCross)
            HOperatorSet.ClearObj(IS3.Ransac3ImagePoints(2).hx_RansacCross)

            IS3.Ransac3ImagePoints(0).hx_RansacCross = IS1.RansacMid.GetCross
            IS3.Ransac3ImagePoints(1).hx_RansacCross = IS2.RansacMid.GetCross
            IS3.Ransac3ImagePoints(2).hx_RansacCross = IS3.RansacMid.GetCross
            'HOperatorSet.VectorToRelPose(IS2.RansacMid.RansacPoints.Row, IS2.RansacMid.RansacPoints.Col, _
            '                    IS3.RansacMid.RansacPoints.Row, IS3.RansacMid.RansacPoints.Col, _
            '                    Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, hv_CamparamOut, hv_CamparamOut, "gold_standard", _
            '                    IS3.ImagePose.RelPose, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        End If
        RemoveBadPoints = BTuple.TupleLength(IS1.RansacMid.RansacPointsIndex)
    End Function

    Private Sub GetScaleFromMidPoints(ByRef IS1 As ImageSet, ByRef IS2 As ImageSet, ByRef Scale As Double, ByRef BadIndex As Object)
        'Dim tmpIndex As Object
        'Dim tI1 As Integer
        'Dim tI2 As Integer
        Dim Dist1 As New Object
        Dim Dist2 As New Object
        Dim i As Integer = 0
        Dim n As Integer = BTuple.TupleLength(IS1.MidPose.X)
        Dim tn1 As Integer
        Dim tn2 As Integer

        'tmpIndex = BTuple.TupleSortIndex(IS1.MidPose.X)
        'tI1 = CInt(BTuple.TupleSelect(tmpIndex, 0))
        'tI2 = CInt(BTuple.TupleSelect(tmpIndex, BTuple.TupleLength(tmpIndex) - 1))
        'IS1.MidPose.Get2PointDist(tI1, tI2, Dist1)
        'IS2.MidPose.Get2PointDist(tI1, tI2, Dist2)
        'Scale = Dist1 / Dist2

        Dim tmpObj As New Object
        Dim tmpScale As New Object
        Dim epiDeviation As New Object
        Dim Deviation As New Object
        Dim ScaleMean As New Object
        Dim delIndex1 As New Object
        Dim delIndex2 As New Object
        Dim isInvert As Boolean = False
        epiDeviation = ScaleDevEpi
        tmpScale = BTuple.TupleGenConst(0, 0)
        BadIndex = BTuple.TupleGenConst(0, 0)
        For i = 0 To n - 1
            epiDeviation = ScaleDevEpi
            IS1.MidPose.Get1PointToAllPoints(i, Dist1)
            IS2.MidPose.Get1PointToAllPoints(i, Dist2)
            tmpObj = BTuple.TupleDiv(Dist1, Dist2)
            ScaleMean = BTuple.TupleMean(tmpObj)
            If i = 148 Then
                tn1 = 0
            End If
            If ScaleMean > 1 Then
                isInvert = False
            Else
                isInvert = True
                tmpObj = BTuple.TupleDiv(Dist2, Dist1)
                ScaleMean = BTuple.TupleMean(tmpObj)
            End If
            Deviation = BTuple.TupleDeviation(tmpObj)
            tn1 = 0
            tn2 = 0
            ' epiDeviation = 0.01 * BTuple.TupleMean(BTuple.TupleConcat(Dist1, Dist2))
            'epiDeviation = ScaleDevEpi * ScaleMean

            While Deviation > epiDeviation
                ScaleMean = BTuple.TupleMean(tmpObj)
                Deviation = BTuple.TupleDeviation(tmpObj)
                HOperatorSet.TupleFind(BTuple.TupleSgn(BTuple.TupleSub(BTuple.TupleSub(tmpObj, ScaleMean), _
                            Deviation)), 1, delIndex1)
                If BTuple.TupleSelect(delIndex1, 0) = -1 Then
                Else
                    tn1 += BTuple.TupleLength(delIndex1)
                End If
                tmpObj = BTuple.TupleRemove(tmpObj, delIndex1)
                HOperatorSet.TupleFind(BTuple.TupleSgn(BTuple.TupleAdd(BTuple.TupleSub(tmpObj, ScaleMean), _
                          Deviation)), -1, delIndex2)
                If BTuple.TupleSelect(delIndex2, 0) = -1 Then
                Else
                    tn2 += BTuple.TupleLength(delIndex2)
                End If


                tmpObj = BTuple.TupleRemove(tmpObj, delIndex2)
                If tn1 + tn2 > (n - 6) Or (BTuple.TupleSelect(delIndex1, 0) = -1 And BTuple.TupleSelect(delIndex2, 0) = -1) Then
                    Deviation = -1
                    'BadIndex = BTuple.TupleConcat(BadIndex, i)
                Else
                    Deviation = BTuple.TupleDeviation(tmpObj)
                End If
            End While
            If Deviation = -1 Then
                If isInvert = True Then
                    tmpObj = BTuple.TupleDiv(1, tmpObj)
                    ScaleMean = BTuple.TupleMean(tmpObj)
                Else
                    ScaleMean = BTuple.TupleMean(tmpObj)
                End If
                tmpScale = BTuple.TupleConcat(tmpScale, ScaleMean)
            Else
                If isInvert = True Then
                    tmpObj = BTuple.TupleDiv(1, tmpObj)
                    ScaleMean = BTuple.TupleMean(tmpObj)
                Else
                    ScaleMean = BTuple.TupleMean(tmpObj)
                End If
                tmpScale = BTuple.TupleConcat(tmpScale, ScaleMean)
            End If
        Next
        tmpObj = tmpScale
        ScaleMean = BTuple.TupleMean(tmpObj)
        Deviation = BTuple.TupleDeviation(tmpObj)
        '  While Deviation > epiDeviation
        ScaleMean = BTuple.TupleMean(tmpObj)
        Deviation = BTuple.TupleDeviation(tmpObj)
        If Deviation < epiDeviation Then
            Scale = ScaleMean
            Exit Sub
        End If
        HOperatorSet.TupleFind(BTuple.TupleSgn(BTuple.TupleSub(BTuple.TupleSub(tmpObj, ScaleMean), _
                    Deviation)), 1, delIndex1)
        If BTuple.TupleSelect(delIndex1, 0) = -1 Then
        Else

            BadIndex = BTuple.TupleConcat(BadIndex, delIndex1)
        End If
        ' tmpObj = BTuple.TupleRemove(tmpObj, delIndex1)
        HOperatorSet.TupleFind(BTuple.TupleSgn(BTuple.TupleAdd(BTuple.TupleSub(tmpObj, ScaleMean), _
                    Deviation)), -1, delIndex2)
        If BTuple.TupleSelect(delIndex2, 0) = -1 Then
        Else

            BadIndex = BTuple.TupleConcat(BadIndex, delIndex2)
        End If

        If BTuple.TupleLength(BadIndex) > n - 6 Then
            BadIndex = BTuple.TupleGenConst(0, 0)
        Else
            tmpObj = BTuple.TupleRemove(tmpObj, BadIndex)
        End If

        Deviation = BTuple.TupleDeviation(tmpObj)

        'If (BTuple.TupleSelect(delIndex1, 0) = -1 And BTuple.TupleSelect(delIndex2, 0) = -1) Then
        '    Deviation = -1
        '    ' BadIndex = BTuple.TupleConcat(BadIndex, i)
        'Else
        '    Deviation = BTuple.TupleDeviation(tmpObj)
        'End If
        ' BadIndex = BTuple.TupleConcat(BadIndex, delIndex2)
        ' End While
        ScaleMean = BTuple.TupleMean(tmpObj)
        Scale = ScaleMean

    End Sub
    Public Sub CalcBestScaleByXYZ(ByVal hv_X1 As Object, ByVal hv_Y1 As Object, ByVal hv_Z1 As Object, _
      ByVal hv_X2 As Object, ByVal hv_Y2 As Object, ByVal hv_Z2 As Object, ByRef hv_ScaleMatrix As Object, _
      ByRef hv_Errors As Object)


        ' Local control variables 
        Dim hv_N As Object = Nothing, hv_MatrixID As Object = Nothing
        Dim hv_MatrixID1 As Object = Nothing, hv_MatrixTransposedID As Object = Nothing
        Dim hv_MatrixTransposedID1 As Object = Nothing, hv_MatrixResultID As Object = Nothing
        Dim hv_MatrixMultID As Object = Nothing, hv_MatrixTransposedID2 As Object = Nothing
        Dim hv_MatrixSubID As Object = Nothing, hv_MatrixPowID1 As Object = Nothing
        Dim hv_MatrixSumID As Object = Nothing, hv_MatrixPowID2 As Object = Nothing

        ' Initialize local and output iconic variables 

        hv_N = BTuple.TupleLength(hv_X1)
        HOperatorSet.CreateMatrix(3, hv_N, BTuple.TupleConcat(BTuple.TupleConcat(hv_X1, hv_Y1), hv_Z1), _
            hv_MatrixID)
        HOperatorSet.CreateMatrix(3, hv_N, BTuple.TupleConcat(BTuple.TupleConcat(hv_X2, hv_Y2), hv_Z2), _
            hv_MatrixID1)
        HOperatorSet.TransposeMatrix(hv_MatrixID, hv_MatrixTransposedID)
        HOperatorSet.TransposeMatrix(hv_MatrixID1, hv_MatrixTransposedID1)
        HOperatorSet.SolveMatrix(hv_MatrixTransposedID1, "general", 0.00000000000000022204, hv_MatrixTransposedID, _
            hv_MatrixResultID)
        HOperatorSet.GetFullMatrix(hv_MatrixResultID, hv_ScaleMatrix)
        HOperatorSet.MultMatrix(hv_MatrixTransposedID1, hv_MatrixResultID, "AB", hv_MatrixMultID)
        HOperatorSet.TransposeMatrix(hv_MatrixMultID, hv_MatrixTransposedID2)
        HOperatorSet.SubMatrix(hv_MatrixID, hv_MatrixTransposedID2, hv_MatrixSubID)
        HOperatorSet.PowScalarElementMatrix(hv_MatrixSubID, 2, hv_MatrixPowID1)
        HOperatorSet.SumMatrix(hv_MatrixPowID1, "columns", hv_MatrixSumID)
        HOperatorSet.PowScalarElementMatrix(hv_MatrixSumID, 0.5, hv_MatrixPowID2)
        HOperatorSet.GetFullMatrix(hv_MatrixPowID2, hv_Errors)
        HOperatorSet.ClearMatrix(hv_MatrixID)
        HOperatorSet.ClearMatrix(hv_MatrixID1)
        HOperatorSet.ClearMatrix(hv_MatrixTransposedID)
        HOperatorSet.ClearMatrix(hv_MatrixTransposedID1)
        HOperatorSet.ClearMatrix(hv_MatrixResultID)
        HOperatorSet.ClearMatrix(hv_MatrixMultID)
        HOperatorSet.ClearMatrix(hv_MatrixTransposedID2)
        HOperatorSet.ClearMatrix(hv_MatrixSubID)
        HOperatorSet.ClearMatrix(hv_MatrixPowID1)
        HOperatorSet.ClearMatrix(hv_MatrixSumID)
        HOperatorSet.ClearMatrix(hv_MatrixPowID2)

    End Sub

    Private Sub GetScaleFromMidPoints1(ByRef IS1 As ImageSet, ByRef IS2 As ImageSet, ByRef Scale As Double, ByRef BadIndex As Object)
        'Dim tmpIndex As Object
        'Dim tI1 As Integer
        'Dim tI2 As Integer
        Dim Dist1 As New Object
        Dim Dist2 As New Object
        Dim Dist As New Object
        Dim i As Integer = 0
        Dim n As Integer = BTuple.TupleLength(IS1.MidPose.X)
        Dim tn1 As Integer

        'tmpIndex = BTuple.TupleSortIndex(IS1.MidPose.X)
        'tI1 = CInt(BTuple.TupleSelect(tmpIndex, 0))
        'tI2 = CInt(BTuple.TupleSelect(tmpIndex, BTuple.TupleLength(tmpIndex) - 1))
        'IS1.MidPose.Get2PointDist(tI1, tI2, Dist1)
        'IS2.MidPose.Get2PointDist(tI1, tI2, Dist2)
        'Scale = Dist1 / Dist2

        Dim tmpObj As New Object
        Dim tmpScale As New Object
        Dim epiDeviation As New Object
        Dim Deviation As New Object
        Dim ScaleMean As New Object
        Dim BestScale As New Object
        Dim delIndex1 As New Object
        Dim delIndex2 As New Object
        Dim delIndex As New Object
        Dim isInvert As Boolean = False
        Dim flgEnd As Integer = 0
        epiDeviation = ScaleDevEpi
        tmpScale = Nothing
        BadIndex = Nothing
        Dim MidPoints1 As New Point3D
        Dim MidPoints2 As New Point3D
        Dim ZeroPoints As New Point3D
        ZeroPoints.X = 0
        ZeroPoints.Y = 0
        ZeroPoints.Z = 0

        ZahyoHenkan(IS1, MidPoints1)
        ' MidPoints1 = IS1.MidPose.ConvertToPoint3d
        ' CalcBestScaleByXYZ(MidPoints1.X, MidPoints1.Y, MidPoints1.Z, MidPoints2.X, MidPoints2.Y, MidPoints2.Z, BestScale, tmpObj)

        ZeroPoints.GetDisttoOtherPose(MidPoints1, Dist1)
        MidPoints2 = IS2.MidPose.ConvertToPoint3d
        ZeroPoints.GetDisttoOtherPose(MidPoints2, Dist2)

        CalcBestScale(MidPoints1, MidPoints2, BestScale)

        tmpObj = BTuple.TupleDiv(Dist1, Dist2)
        ScaleMean = BTuple.TupleMean(tmpObj)
        Deviation = BTuple.TupleDeviation(tmpObj)
        If Deviation < epiDeviation Then
            Scale = ScaleMean
            'Exit Sub
        End If
        If ScaleMean / BestScale > 10 Then
            BestScale = ScaleMean
        End If
        MidPoints2.SetScale(BestScale)
        'Dim homMat3d As New Object
        'hom_mat_3d_from_3d_3d_point_correspondence(MidPoints2.X, MidPoints2.Y, MidPoints2.Z, _
        '                                 MidPoints1.X, MidPoints1.Y, MidPoints1.Z, homMat3d)
        'Dim testMidPose As New Point3D

        'HOperatorSet.AffineTransPoint3D(homMat3d, MidPoints2.X, MidPoints2.Y, MidPoints2.Z, testMidPose.X, testMidPose.Y, testMidPose.Z)
        'testMidPose.GetDisttoOtherPose(MidPoints1, Dist)

        MidPoints1.GetDisttoOtherPose(MidPoints2, Dist)
        Dim DistMean As New Object
        DistMean = BTuple.TupleMean(Dist)
        Dim DevDist As New Object
        DevDist = BTuple.TupleDeviation(Dist)
        'If DistMean < DevDist Then
        '    HOperatorSet.TupleFind(BTuple.TupleSgn(BTuple.TupleSub(Dist, DevDist)), 1, delIndex2)
        'Else
        '    HOperatorSet.TupleFind(BTuple.TupleSgn(BTuple.TupleSub(Dist, DistMean)), 1, delIndex2)
        'End If
        'HOperatorSet.TupleFind(BTuple.TupleSgn(BTuple.TupleSub(BTuple.TupleSub(Dist, DistMean), _
        '               DevDist)), 1, delIndex2)
        delIndex1 = BTuple.TupleSortIndex(Dist)
        delIndex2 = BTuple.TupleLastN(delIndex1, CInt(BTuple.TupleLength(delIndex1) * 0.98) - 1)

        If BTuple.TupleSelect(delIndex2, 0) <> -1 Then
            tn1 = BTuple.TupleLength(delIndex2)
            If n - tn1 <= 6 Then
                ScaleMean = BTuple.TupleMean(tmpObj)
                Scale = BestScale
                Exit Sub
            Else
                epiDeviation = 0.02
                If DevDist < epiDeviation And DistMean < epiDeviation Then
                    ScaleMean = BTuple.TupleMean(tmpObj)
                    Scale = BestScale
                    Exit Sub
                End If
            End If

        End If


        'If ScaleMean > 1 Then
        '    isInvert = False
        'Else
        '    isInvert = True
        '    tmpObj = BTuple.TupleDiv(Dist2, Dist1)
        '    ScaleMean = BTuple.TupleMean(tmpObj)
        'End If


        'While flgEnd = 0
        '    ScaleMean = BTuple.TupleMean(tmpObj)
        '    Deviation = BTuple.TupleDeviation(tmpObj)
        '    delIndex = BTuple.TupleGenConst(0, 0)

        '    HOperatorSet.TupleFind(BTuple.TupleSgn(BTuple.TupleSub(BTuple.TupleSub(tmpObj, ScaleMean), _
        '                Deviation)), 1, delIndex1)
        '    If BTuple.TupleSelect(delIndex1, 0) = -1 Then

        '    Else
        '        tn1 += BTuple.TupleLength(delIndex1)
        '    End If
        '    'tmpObj = BTuple.TupleRemove(tmpObj, delIndex1)
        '    HOperatorSet.TupleFind(BTuple.TupleSgn(BTuple.TupleAdd(BTuple.TupleSub(tmpObj, ScaleMean), _
        '              Deviation)), -1, delIndex2)
        '    If BTuple.TupleSelect(delIndex2, 0) = -1 Then
        '    Else
        '        tn2 += BTuple.TupleLength(delIndex2)
        '    End If

        '    delIndex = BTuple.TupleConcat(delIndex1, delIndex2)
        '    'tmpObj = BTuple.TupleRemove(tmpObj, delIndex2)
        '    If tn1 + tn2 > (n - 6) Or (BTuple.TupleSelect(delIndex1, 0) = -1 And BTuple.TupleSelect(delIndex2, 0) = -1) Then
        '        flgEnd = -1
        '    Else
        '        tmpObj = BTuple.TupleRemove(tmpObj, delIndex)
        '        ScaleMean = BTuple.TupleMean(tmpObj)
        '        Deviation = BTuple.TupleDeviation(tmpObj)
        '        If Deviation < epiDeviation Then
        '            flgEnd = -1
        '        End If
        '    End If
        'End While

        'If isInvert = True Then
        '    tmpObj = BTuple.TupleDiv(Dist2, Dist1)
        'Else
        '    tmpObj = BTuple.TupleDiv(Dist1, Dist2)
        'End If


        'HOperatorSet.TupleFind(BTuple.TupleSgn(BTuple.TupleSub(BTuple.TupleSub(tmpObj, ScaleMean), _
        '            Deviation)), 1, delIndex1)

        'HOperatorSet.TupleFind(BTuple.TupleSgn(BTuple.TupleAdd(BTuple.TupleSub(tmpObj, ScaleMean), _
        '            Deviation)), -1, delIndex2)
        'If BTuple.TupleSelect(delIndex1, 0) = -1 Then

        'Else
        '    BadIndex = delIndex1
        'End If
        If BTuple.TupleSelect(delIndex2, 0) = -1 Then

        Else
            BadIndex = BTuple.TupleConcat(BadIndex, delIndex2)
        End If

        'If isInvert = True Then
        '    tmpObj = BTuple.TupleDiv(Dist1, Dist2)
        '    tmpObj = BTuple.TupleRemove(tmpObj, BadIndex)
        'Else
        '    tmpObj = BTuple.TupleRemove(tmpObj, BadIndex)
        'End If
        tmpObj = BTuple.TupleRemove(tmpObj, BadIndex)
        ScaleMean = BTuple.TupleMean(tmpObj)
        Scale = BestScale

    End Sub


    Private Sub GetScaleFromMidPoints2(ByRef IS1 As ImageSet, ByRef IS2 As ImageSet, ByRef Scale As Double, ByRef BadIndex As Object)
        'Dim tmpIndex As Object
        'Dim tI1 As Integer
        'Dim tI2 As Integer
        Dim Dist1 As New Object
        Dim Dist2 As New Object
        Dim Dist As New Object
        Dim i As Integer = 0
        Dim n As Integer = BTuple.TupleLength(IS1.MidPose.X)
        Dim tn1 As Integer

        'tmpIndex = BTuple.TupleSortIndex(IS1.MidPose.X)
        'tI1 = CInt(BTuple.TupleSelect(tmpIndex, 0))
        'tI2 = CInt(BTuple.TupleSelect(tmpIndex, BTuple.TupleLength(tmpIndex) - 1))
        'IS1.MidPose.Get2PointDist(tI1, tI2, Dist1)
        'IS2.MidPose.Get2PointDist(tI1, tI2, Dist2)
        'Scale = Dist1 / Dist2

        Dim tmpObj As New Object
        Dim tmpScale As New Object
        Dim epiDeviation As New Object
        Dim Deviation As New Object
        Dim ScaleMean As New Object
        Dim BestScale As New Object
        Dim delIndex1 As New Object
        Dim delIndex2 As New Object
        Dim delIndex As New Object
        Dim isInvert As Boolean = False
        Dim flgEnd As Integer = 0
        epiDeviation = ScaleDevEpi
        tmpScale = Nothing
        BadIndex = Nothing
        Dim MidPoints1 As New Point3D
        Dim MidPoints2 As New Point3D
        Dim ZeroPoints As New Point3D
        ZeroPoints.X = 0
        ZeroPoints.Y = 0
        ZeroPoints.Z = 0

        ZahyoHenkan(IS1, MidPoints1)
        MidPoints2 = IS2.MidPose.ConvertToPoint3d
        '  MidPoints1 = IS1.MidPose.ConvertToPoint3d
        CalcBestScaleByXYZ(MidPoints1.X, MidPoints1.Y, MidPoints1.Z, MidPoints2.X, MidPoints2.Y, MidPoints2.Z, BestScale, tmpObj)


        ' tmpObj = BTuple.TupleDiv(Dist1, Dist2)
        ScaleMean = BTuple.TupleMean(tmpObj)
        Deviation = BTuple.TupleDeviation(tmpObj)
        If Deviation < epiDeviation Then
            Scale = ScaleMean
            'Exit Sub
        End If

        MidPoints2.SetScaleByMatrix(BestScale)
        'Dim homMat3d As New Object
        'hom_mat_3d_from_3d_3d_point_correspondence(MidPoints2.X, MidPoints2.Y, MidPoints2.Z, _
        '                                 MidPoints1.X, MidPoints1.Y, MidPoints1.Z, homMat3d)
        'Dim testMidPose As New Point3D

        'HOperatorSet.AffineTransPoint3D(homMat3d, MidPoints2.X, MidPoints2.Y, MidPoints2.Z, testMidPose.X, testMidPose.Y, testMidPose.Z)
        'testMidPose.GetDisttoOtherPose(MidPoints1, Dist)

        MidPoints1.GetDisttoOtherPose(MidPoints2, Dist)
        Dim DistMean As New Object
        DistMean = BTuple.TupleMean(Dist)
        Dim DevDist As New Object
        DevDist = BTuple.TupleDeviation(Dist)
        'If DistMean < DevDist Then
        '    HOperatorSet.TupleFind(BTuple.TupleSgn(BTuple.TupleSub(Dist, DevDist)), 1, delIndex2)
        'Else
        '    HOperatorSet.TupleFind(BTuple.TupleSgn(BTuple.TupleSub(Dist, DistMean)), 1, delIndex2)
        'End If
        HOperatorSet.TupleFind(BTuple.TupleSgn(BTuple.TupleSub(BTuple.TupleSub(Dist, DistMean), _
                       DevDist)), 1, delIndex2)

        If BTuple.TupleSelect(delIndex2, 0) <> -1 Then
            tn1 = BTuple.TupleLength(delIndex2)
            If n - tn1 <= 6 Then
                ScaleMean = BTuple.TupleMean(tmpObj)
                Scale = BestScale
                Exit Sub
            Else
                epiDeviation = 0.03
                If DevDist < epiDeviation And DistMean < epiDeviation Then
                    ScaleMean = BTuple.TupleMean(tmpObj)
                    Scale = BestScale
                    Exit Sub
                End If
            End If

        End If


        'If ScaleMean > 1 Then
        '    isInvert = False
        'Else
        '    isInvert = True
        '    tmpObj = BTuple.TupleDiv(Dist2, Dist1)
        '    ScaleMean = BTuple.TupleMean(tmpObj)
        'End If


        'While flgEnd = 0
        '    ScaleMean = BTuple.TupleMean(tmpObj)
        '    Deviation = BTuple.TupleDeviation(tmpObj)
        '    delIndex = BTuple.TupleGenConst(0, 0)

        '    HOperatorSet.TupleFind(BTuple.TupleSgn(BTuple.TupleSub(BTuple.TupleSub(tmpObj, ScaleMean), _
        '                Deviation)), 1, delIndex1)
        '    If BTuple.TupleSelect(delIndex1, 0) = -1 Then

        '    Else
        '        tn1 += BTuple.TupleLength(delIndex1)
        '    End If
        '    'tmpObj = BTuple.TupleRemove(tmpObj, delIndex1)
        '    HOperatorSet.TupleFind(BTuple.TupleSgn(BTuple.TupleAdd(BTuple.TupleSub(tmpObj, ScaleMean), _
        '              Deviation)), -1, delIndex2)
        '    If BTuple.TupleSelect(delIndex2, 0) = -1 Then
        '    Else
        '        tn2 += BTuple.TupleLength(delIndex2)
        '    End If

        '    delIndex = BTuple.TupleConcat(delIndex1, delIndex2)
        '    'tmpObj = BTuple.TupleRemove(tmpObj, delIndex2)
        '    If tn1 + tn2 > (n - 6) Or (BTuple.TupleSelect(delIndex1, 0) = -1 And BTuple.TupleSelect(delIndex2, 0) = -1) Then
        '        flgEnd = -1
        '    Else
        '        tmpObj = BTuple.TupleRemove(tmpObj, delIndex)
        '        ScaleMean = BTuple.TupleMean(tmpObj)
        '        Deviation = BTuple.TupleDeviation(tmpObj)
        '        If Deviation < epiDeviation Then
        '            flgEnd = -1
        '        End If
        '    End If
        'End While

        'If isInvert = True Then
        '    tmpObj = BTuple.TupleDiv(Dist2, Dist1)
        'Else
        '    tmpObj = BTuple.TupleDiv(Dist1, Dist2)
        'End If


        'HOperatorSet.TupleFind(BTuple.TupleSgn(BTuple.TupleSub(BTuple.TupleSub(tmpObj, ScaleMean), _
        '            Deviation)), 1, delIndex1)

        'HOperatorSet.TupleFind(BTuple.TupleSgn(BTuple.TupleAdd(BTuple.TupleSub(tmpObj, ScaleMean), _
        '            Deviation)), -1, delIndex2)
        'If BTuple.TupleSelect(delIndex1, 0) = -1 Then

        'Else
        '    BadIndex = delIndex1
        'End If
        If BTuple.TupleSelect(delIndex2, 0) = -1 Then

        Else
            BadIndex = BTuple.TupleConcat(BadIndex, delIndex2)
        End If

        'If isInvert = True Then
        '    tmpObj = BTuple.TupleDiv(Dist1, Dist2)
        '    tmpObj = BTuple.TupleRemove(tmpObj, BadIndex)
        'Else
        '    tmpObj = BTuple.TupleRemove(tmpObj, BadIndex)
        'End If
        tmpObj = BTuple.TupleRemove(tmpObj, BadIndex)
        ScaleMean = BTuple.TupleMean(tmpObj)
        Scale = BestScale

    End Sub


    Private Sub GetScaleFromMidPoints3(ByRef IS1 As ImageSet, ByRef IS2 As ImageSet, ByRef IS3 As ImageSet, ByRef Scale As Double, ByRef BadIndex As Object)
        Dim XYZ As Point3D
        Dim XYZ12 As New Point3D
        Dim XYZ13 As New Point3D
        Dim Dist2 As New Object
        Dim Dist3 As New Object
        Dim Dist23 As New Object
        Dim Pose2 As New Object
        Dim Pose3 As New Object
        Dim RelPose23 As New Object
        Dim Quality2 As Object = Nothing
        Dim Quality3 As Object = Nothing
        Dim Kyori As New Object
        Dim BadIndex1 As Object = Nothing
        Dim BadIndex2 As Object = Nothing
        Dim hv_Kyori13 As Object = Nothing
        Dim hv_Kyori12 As Object = Nothing
        Dim hv_Indices1 As Object = Nothing
        Dim hv_DistInd As Object = Nothing
        Dim hv_Mean As Object = Nothing
        Dim hv_MaxDist As Object = Nothing
        Dim hv_MeanVectorPose As Object = Nothing
        Dim hv_DeviationVectorPose As Object = Nothing
        Dim epiDeviation As Object = Nothing
        Dim N As Object = Nothing
        Dim BestScale As Object = Nothing
        Dim Error1 As Object = Nothing
        Dim Error2 As Object = Nothing
        Dim Error3 As Object = Nothing
        Dim tmpPose As Object = Nothing
        Dim tmpPose1 As Object = Nothing

        epiDeviation = 0.03
        'カメラ１と３の姿勢を計算

        If IS1.flgFirst = True Then
            CalcRelPoseByMidPose(IS1, IS2)
            XYZ = IS2.MidPose.ConvertToPoint3d
            CalcVectorToPose(XYZ, IS2, Pose2, Quality2)
        Else
            Pose2 = IS1.VectorPose.RelPose
            CalcMidXYZ_byRelPose(IS1, IS2, Pose2, Quality2)

            'CalcRelPoseByMidPose(IS1, IS2)
            'CalcMidXYZ_byRelPose(IS1, IS2, IS2.MidPose.RelPose, Quality3)
            'Pose2 = IS2.MidPose.RelPose
            If BTuple.TupleLength(Quality2) = 0 Then
                CalcRelPoseByMidPose(IS1, IS2)

            End If
            XYZ = IS2.MidPose.ConvertToPoint3d
        End If

        CalcVectorToPose(XYZ, IS3, Pose3, Quality3)
        CalcRelPoseByMidPose(IS1, IS2)
        Error1 = IS2.MidPose.hError
        CalcRelPoseByMidPose(IS2, IS3)
        Error2 = IS3.MidPose.hError
        CalcRelPoseByMidPose(IS1, IS3)
        Error3 = IS3.MidPose.hError
        tmpPose = Pose3
        tmpPose1 = Pose3
        CalcUnitPose(tmpPose, tmpPose1)

        hv_Kyori13 = BTuple.TupleSub(BTuple.TupleSelectRange(tmpPose, 0, 2), BTuple.TupleSelectRange(IS3.MidPose.RelPose, 0, 2))
        hv_Kyori13 = BTuple.TupleSqrt(BTuple.TupleSum(BTuple.TuplePow(hv_Kyori13, 2)))
        CalcUnitPose(Pose2, Pose3)
        Try
            HOperatorSet.IntersectLinesOfSight(hv_CamparamOut, hv_CamparamOut, Pose2, IS1.RansacMid.RansacPoints.Row, _
                 IS1.RansacMid.RansacPoints.Col, IS2.RansacMid.RansacPoints.Row, IS2.RansacMid.RansacPoints.Col, XYZ12.X, XYZ12.Y, XYZ12.Z, Dist2)
            HOperatorSet.IntersectLinesOfSight(hv_CamparamOut, hv_CamparamOut, Pose3, IS1.RansacMid.RansacPoints.Row, _
                 IS1.RansacMid.RansacPoints.Col, IS3.RansacMid.RansacPoints.Row, IS3.RansacMid.RansacPoints.Col, XYZ13.X, XYZ13.Y, XYZ13.Z, Dist3)
            XYZ12.GetDisttoOtherPose(XYZ13, Dist23)
            hv_DistInd = BTuple.TupleSortIndex(Dist23)
            HOperatorSet.TupleMean(Dist23, hv_MeanVectorPose)
            HOperatorSet.TupleDeviation(Dist23, hv_DeviationVectorPose)
        Catch ex As Exception
            hv_MeanVectorPose = epiDeviation * 2
            hv_DeviationVectorPose = epiDeviation * 2
            hv_DistInd = Nothing
        End Try
        If BTuple.TupleLength(XYZ12.X) = 0 Or BTuple.TupleLength(XYZ13.X) = 0 Then
            CalcRelPoseByMidPose(IS1, IS2)

            XYZ = IS2.MidPose.ConvertToPoint3d
            CalcReProjectError(Pose3, XYZ, IS3, Kyori)
        Else
            CalcReProjectError(Pose3, XYZ12, IS3, Kyori)
        End If

        HOperatorSet.TupleSortIndex(Kyori, hv_Indices1)
        HOperatorSet.TupleMean(Kyori, hv_Mean)
        HOperatorSet.TupleMax(Kyori, hv_MaxDist)
        CalcRelPoseBetweenTwoPose(Pose2, Pose3, RelPose23)
        Scale = BTuple.TupleSqrt(BTuple.TupleSum(BTuple.TupleFirstN(BTuple.TuplePow(RelPose23, 2), 2))) / BTuple.TupleSqrt(BTuple.TupleSum(BTuple.TupleFirstN(BTuple.TuplePow(Pose2, 2), 2)))
        CalcUnitPose(RelPose23, tmpPose)


        'Try
        '    'If Quality3 > 0.3 Then
        '    HOperatorSet.IntersectLinesOfSight(hv_CamparamOut, hv_CamparamOut, Pose2, IS1.RansacMid.RansacPoints.Row, _
        '          IS1.RansacMid.RansacPoints.Col, IS2.RansacMid.RansacPoints.Row, IS2.RansacMid.RansacPoints.Col, XYZ12.X, XYZ12.Y, XYZ12.Z, Dist2)
        '    HOperatorSet.IntersectLinesOfSight(hv_CamparamOut, hv_CamparamOut, RelPose23, IS2.RansacMid.RansacPoints.Row, _
        '         IS2.RansacMid.RansacPoints.Col, IS3.RansacMid.RansacPoints.Row, IS3.RansacMid.RansacPoints.Col, XYZ13.X, XYZ13.Y, XYZ13.Z, Dist3)
        '    ZahyoHenkan(Pose2, XYZ12, XYZ)

        '    CalcBestScale(XYZ, XYZ13, BestScale)
        '    Scale = BestScale
        '    Dim hv_HomMat3d As New Object
        '    XYZ13.SetScale(Scale)
        '    hom_mat_3d_from_3d_3d_point_correspondence(XYZ13.X, XYZ13.Y, XYZ13.Z, XYZ12.X, XYZ12.Y, XYZ12.Z, hv_HomMat3d)
        '    HOperatorSet.AffineTransPoint3D(hv_HomMat3d, XYZ13.X, XYZ13.Y, XYZ13.Z, XYZ13.X, XYZ13.Y, XYZ13.Z)
        '    XYZ12.GetDisttoOtherPose(XYZ13, Dist23)
        '    hv_DistInd = BTuple.TupleSortIndex(Dist23)
        '    HOperatorSet.TupleMean(Dist23, hv_MeanVectorPose)
        '    HOperatorSet.TupleDeviation(Dist23, hv_DeviationVectorPose)
        '    ' End If

        'Catch ex As Exception
        '    CalcRelPoseByMidPose(IS1, IS2)
        '    CalcRelPoseByMidPose(IS2, IS3)
        '    Pose2 = IS2.MidPose.RelPose
        '    RelPose23 = IS3.MidPose.RelPose
        '    XYZ12 = IS2.MidPose.ConvertToPoint3d
        '    XYZ13 = IS3.MidPose.ConvertToPoint3d
        '    'HOperatorSet.IntersectLinesOfSight(hv_CamparamOut, hv_CamparamOut, Pose2, IS1.RansacMid.RansacPoints.Row, _
        '    '    IS1.RansacMid.RansacPoints.Col, IS2.RansacMid.RansacPoints.Row, IS2.RansacMid.RansacPoints.Col, XYZ12.X, XYZ12.Y, XYZ12.Z, Dist2)
        '    'HOperatorSet.IntersectLinesOfSight(hv_CamparamOut, hv_CamparamOut, RelPose23, IS2.RansacMid.RansacPoints.Row, _
        '    '     IS2.RansacMid.RansacPoints.Col, IS3.RansacMid.RansacPoints.Row, IS3.RansacMid.RansacPoints.Col, XYZ13.X, XYZ13.Y, XYZ13.Z, Dist3)

        '    ZahyoHenkan(Pose2, XYZ12, XYZ)

        '    CalcBestScale(XYZ, XYZ13, BestScale)
        '    Scale = BestScale
        '    Dim hv_HomMat3d As New Object
        '    XYZ13.SetScale(Scale)
        '    hom_mat_3d_from_3d_3d_point_correspondence(XYZ13.X, XYZ13.Y, XYZ13.Z, XYZ12.X, XYZ12.Y, XYZ12.Z, hv_HomMat3d)
        '    HOperatorSet.AffineTransPoint3D(hv_HomMat3d, XYZ13.X, XYZ13.Y, XYZ13.Z, XYZ13.X, XYZ13.Y, XYZ13.Z)
        '    XYZ12.GetDisttoOtherPose(XYZ13, Dist23)
        '    hv_DistInd = BTuple.TupleSortIndex(Dist23)
        '    HOperatorSet.TupleMean(Dist23, hv_MeanVectorPose)
        '    HOperatorSet.TupleDeviation(Dist23, hv_DeviationVectorPose)

        'End Try

        IS2.VectorPose.RelPose = RelPose23
        IS2.VectorPose.Pose = Pose2
        IS3.VectorPose.Pose = RelPose23
        hv_Kyori13 = hv_Kyori13
        If (hv_MeanVectorPose < epiDeviation And hv_DeviationVectorPose < epiDeviation) And (hv_Mean < 0.5 And hv_MaxDist < 1) _
        And ((BTuple.TupleMin(Error1) + BTuple.TupleMin(Error2)) / 2 > BTuple.TupleMin(Error3)) Then
            BadIndex = Nothing
            Exit Sub
        Else
            'BadIndex = BTuple.TupleSelect(hv_Indices1, BTuple.TupleSub(BTuple.TupleLength( _
            '                          hv_Indices1), 1))
            N = CInt(BTuple.TupleLength(hv_Indices1) * 0.98) - 1
            'BadIndex1 = BTuple.TupleLastN(hv_DistInd, N)
            'BadIndex2 = BTuple.TupleLastN(hv_Indices1, N)
            'BadIndex = System.Nothing
            'For i = 0 To BTuple.TupleLength(BadIndex1) - 1
            '    Dim tmp As Object = Nothing
            '    tmp = BTuple.TupleFind(BadIndex1, BTuple.TupleSelect(BadIndex2, i))
            '    If BTuple.TupleSelect(tmp, 0) = -1 Then
            '    Else
            '        BadIndex = BTuple.TupleConcat(BadIndex, BTuple.TupleSelect(BadIndex2, i))
            '    End If
            'Next
            'If BTuple.TupleLength(BadIndex) = 0 Then
            BadIndex = BTuple.TupleLastN(hv_Indices1, N)
            '  End If
        End If

    End Sub


    Private Sub GetScaleFromMidPoints4(ByRef IS1 As ImageSet, ByRef IS2 As ImageSet, ByRef IS3 As ImageSet, ByRef Scale As Double, ByRef BadIndex As Object)
        Dim XYZ As New Point3D
        Dim XYZ12 As New Point3D
        Dim XYZ23 As New Point3D
        Dim Dist2 As Object = Nothing
        Dim Dist3 As Object = Nothing
        Dim Dist23 As Object = Nothing
        Dim Pose2 As Object = Nothing
        Dim Pose3 As Object = Nothing
        Dim RelPose23 As Object = Nothing
        Dim Quality2 As Object = Nothing
        Dim Quality3 As Object = Nothing
        Dim Kyori As New Object
        Dim BadIndex1 As Object = Nothing
        Dim BadIndex2 As Object = Nothing
        Dim hv_Kyori13 As Object = Nothing
        Dim hv_Kyori12 As Object = Nothing
        Dim hv_Indices1 As Object = Nothing
        Dim hv_DistInd As Object = Nothing
        Dim hv_Mean As Object = Nothing
        Dim hv_MaxDist As Object = Nothing
        Dim hv_MeanVectorPose As Object = Nothing
        Dim hv_DeviationVectorPose As Object = Nothing
        Dim epiDeviation As Object = Nothing
        Dim hv_HomMat3d As Object = Nothing

        Dim N As Object = Nothing
        Dim BestScale As Object = Nothing
        Dim tmpScale As Object = Nothing
        Dim i As Integer
        Dim Error1 As Object = Nothing
        Dim Error2 As Object = Nothing
        Dim Error3 As Object = Nothing
        Dim tmpPose As Object = Nothing
        Dim tmpPose1 As Object = Nothing
        Dim tmpError As Object = Nothing

        Dim MidRow11 As Object = Nothing
        Dim MidCol11 As Object = Nothing
        Dim MidRow21 As Object = Nothing
        Dim MidCol21 As Object = Nothing
        epiDeviation = 0.03
        'カメラ１と３の姿勢を計算
        CalcRelPoseByMidPose(IS1, IS2)
        Error1 = IS2.MidPose.hError
        ' CalcRelPoseByMidPose(IS1, IS3)
        ' Error3 = IS3.MidPose.hError
        tmpPose = IS3.MidPose.RelPose
        CalcRelPoseByMidPose(IS2, IS3)
        Error2 = IS3.MidPose.hError
        RelPose23 = IS3.MidPose.RelPose
        If IS1.flgFirst = True Then
            Pose2 = IS2.MidPose.RelPose
        Else
            'MidRow11 = BTuple.TupleConcat(MidRow1, IS1.RansacMid.RansacPoints.Row)
            'MidCol11 = BTuple.TupleConcat(MidCol1, IS1.RansacMid.RansacPoints.Col)
            'MidRow21 = BTuple.TupleConcat(MidRow2, IS2.RansacMid.RansacPoints.Row)
            'MidCol21 = BTuple.TupleConcat(MidCol2, IS2.RansacMid.RansacPoints.Col)
            MidRow11 = Nothing
            MidCol11 = Nothing
            MidRow21 = Nothing
            MidCol21 = Nothing

            For i = 0 To BTuple.TupleLength(MidRow1) - 1
                Dim R As Object = Nothing
                IS1.RansacMid.RansacPoints.GetPointsByCoord(BTuple.TupleSelect(MidRow1, i), _
                                                            BTuple.TupleSelect(MidCol1, i), R)
                If R = -1 Then
                    MidRow11 = BTuple.TupleConcat(MidRow11, BTuple.TupleSelect(MidRow1, i))
                    MidCol11 = BTuple.TupleConcat(MidCol11, BTuple.TupleSelect(MidCol1, i))
                    MidRow21 = BTuple.TupleConcat(MidRow21, BTuple.TupleSelect(MidRow2, i))
                    MidCol21 = BTuple.TupleConcat(MidCol21, BTuple.TupleSelect(MidCol2, i))
                End If
            Next
            MidRow11 = BTuple.TupleConcat(MidRow11, IS1.RansacMid.RansacPoints.Row)
            MidCol11 = BTuple.TupleConcat(MidCol11, IS1.RansacMid.RansacPoints.Col)
            MidRow21 = BTuple.TupleConcat(MidRow21, IS2.RansacMid.RansacPoints.Row)
            MidCol21 = BTuple.TupleConcat(MidCol21, IS2.RansacMid.RansacPoints.Col)

            Try
                HOperatorSet.VectorToRelPose(MidRow11, MidCol11, MidRow21, MidCol21, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, hv_CamparamOut, hv_CamparamOut, _
                            "gold_standard", Pose2, Nothing, tmpError, Nothing, Nothing, Nothing, Nothing)
            Catch ex As Exception
                Pose2 = IS1.VectorPose.RelPose
            End Try

            'Pose2 = IS1.VectorPose.RelPose
            hv_Kyori12 = BTuple.TupleSub(BTuple.TupleSelectRange(IS2.MidPose.RelPose, 0, 5), BTuple.TupleSelectRange(Pose2, 0, 5))
            hv_Kyori12 = BTuple.TupleSqrt(BTuple.TupleSum(BTuple.TuplePow(hv_Kyori12, 2)))
            CalcMidXYZ_byRelPose(IS1, IS2, Pose2, Quality2)

            'CalcRelPoseByMidPose(IS1, IS2)
            'CalcMidXYZ_byRelPose(IS1, IS2, IS2.MidPose.RelPose, Quality3)
            'Pose2 = IS2.MidPose.RelPose
            If BTuple.TupleLength(Quality2) = 0 Then
                CalcRelPoseByMidPose(IS1, IS2)
                Pose2 = IS2.MidPose.RelPose
            End If
        End If

        'XYZ = IS2.MidPose.ConvertToPoint3d
        'CalcVectorToPose(XYZ, IS3, Pose3, Quality3)
        'CalcRelPoseBetweenTwoPose(BTuple.TupleFirstN(Pose2, 6), Pose3, RelPose23)
        'tmpPose1 = RelPose23
        'CalcUnitPose(RelPose23, tmpPose1)

        Try
            HOperatorSet.IntersectLinesOfSight(hv_CamparamOut, hv_CamparamOut, Pose2, IS1.RansacMid.RansacPoints.Row, _
                  IS1.RansacMid.RansacPoints.Col, IS2.RansacMid.RansacPoints.Row, IS2.RansacMid.RansacPoints.Col, XYZ12.X, XYZ12.Y, XYZ12.Z, Dist2)
        Catch ex As Exception
            CalcRelPoseByMidPose(IS1, IS2)
            Pose2 = IS2.MidPose.RelPose
            XYZ12 = IS2.MidPose.ConvertToPoint3d
        End Try

        XYZ23 = IS3.MidPose.ConvertToPoint3d

        ZahyoHenkan(Pose2, XYZ12, XYZ)

        'CalcBestScale(XYZ, XYZ23, BestScale)
        CalcBestScaleWithWeight(XYZ, XYZ23, 10, BestScale)

        XYZ = XYZ23.SetScale2(BestScale)

        ConnectPose(Pose2, RelPose23, BestScale, Pose3)

        CalcMatchingResult(IS1, IS2, IS3, Pose3, XYZ12, XYZ, Dist23, Kyori)

        hv_DistInd = BTuple.TupleSortIndex(Dist23)
        HOperatorSet.TupleMean(Dist23, hv_MeanVectorPose)
        HOperatorSet.TupleDeviation(Dist23, hv_DeviationVectorPose)

        HOperatorSet.TupleSortIndex(Kyori, hv_Indices1)
        HOperatorSet.TupleMean(Kyori, hv_Mean)
        HOperatorSet.TupleMax(Kyori, hv_MaxDist)

        'CalcScaleForRelPose(Pose2, RelPose23, tmpPose, tmpScale, Nothing)
        'ConnectPose(Pose2, RelPose23, tmpScale, Pose3)
        'XYZ = XYZ23.SetScale2(tmpScale)
        'CalcMatchingResult(IS1, IS2, IS3, Pose3, XYZ12, XYZ, Dist23, Kyori)

        'If hv_Mean > BTuple.TupleMean(Kyori) Then
        '    Scale = tmpScale

        '    hv_DistInd = BTuple.TupleSortIndex(Dist23)
        '    HOperatorSet.TupleMean(Dist23, hv_MeanVectorPose)
        '    HOperatorSet.TupleDeviation(Dist23, hv_DeviationVectorPose)

        '    HOperatorSet.TupleSortIndex(Kyori, hv_Indices1)
        '    HOperatorSet.TupleMean(Kyori, hv_Mean)
        '    HOperatorSet.TupleMax(Kyori, hv_MaxDist)
        'Else
        '    ConnectPose(Pose2, RelPose23, BestScale, Pose3)
        '    Scale = BestScale
        'End If

        'ConnectPose(Pose2, RelPose23, BestScale, Pose3)
        Scale = BestScale

        IS2.VectorPose.RelPose = RelPose23
        IS2.VectorPose.Pose = Pose2
        IS3.VectorPose.Pose = RelPose23
        hv_Kyori13 = hv_Kyori13
        tmpPose1 = tmpPose
        CalcUnitPose(Pose3, tmpPose)
        hv_Kyori13 = BTuple.TupleSub(BTuple.TupleSelectRange(tmpPose1, 0, 5), BTuple.TupleSelectRange(Pose3, 0, 5))
        hv_Kyori13 = BTuple.TupleSqrt(BTuple.TupleSum(BTuple.TuplePow(hv_Kyori13, 2)))
        epiDeviation = epiDev
        If (hv_MeanVectorPose < epiDeviation And hv_DeviationVectorPose < epiDeviation) And (hv_Mean < MeanPixErr And hv_MaxDist < MaxPixErr) Then
            BadIndex = Nothing
            Exit Sub
        Else
            'BadIndex = BTuple.TupleSelect(hv_Indices1, BTuple.TupleSub(BTuple.TupleLength( _
            '                          hv_Indices1), 1))
            N = CInt(BTuple.TupleLength(hv_Indices1) * 0.98) - 1
            BadIndex1 = BTuple.TupleLastN(hv_DistInd, N)
            BadIndex2 = BTuple.TupleLastN(hv_Indices1, N)
            BadIndex = Nothing
            'For i = 0 To BTuple.TupleLength(BadIndex1) - 1
            '    Dim tmp As Object = Nothing
            '    tmp = BTuple.TupleFind(BadIndex1, BTuple.TupleSelect(BadIndex2, i))
            '    If BTuple.TupleSelect(tmp, 0) = -1 Then
            '    Else
            '        BadIndex = BTuple.TupleConcat(BadIndex, BTuple.TupleSelect(BadIndex2, i))
            '    End If
            'Next
            If BTuple.TupleLength(BadIndex) = 0 Then
                BadIndex = BTuple.TupleUniq(BTuple.TupleSort(BTuple.TupleConcat(BadIndex1, BadIndex2)))
            End If
        End If


    End Sub


    Private Sub GetScaleFromMidPoints5(ByRef IS1 As ImageSet, ByRef IS2 As ImageSet, ByRef IS3 As ImageSet, ByRef Scale As Double, ByRef BadIndex As Object)
        Dim XYZ As New Point3D
        Dim XYZ12 As New Point3D
        Dim XYZ23 As New Point3D
        Dim Dist2 As Object = Nothing
        Dim Dist3 As Object = Nothing
        Dim Dist23 As Object = Nothing
        Dim Pose2 As Object = Nothing
        Dim Pose3 As Object = Nothing
        Dim RelPose23 As Object = Nothing
        Dim Quality2 As Object = Nothing
        Dim Quality3 As Object = Nothing
        Dim Kyori As New Object
        Dim BadIndex1 As Object = Nothing
        Dim BadIndex2 As Object = Nothing
        Dim hv_Kyori13 As Object = Nothing
        Dim hv_Kyori12 As Object = Nothing
        Dim hv_Indices1 As Object = Nothing
        Dim hv_DistInd As Object = Nothing
        Dim hv_Mean As Object = Nothing
        Dim hv_MaxDist As Object = Nothing
        Dim hv_MeanVectorPose As Object = Nothing
        Dim hv_DeviationVectorPose As Object = Nothing
        Dim epiDeviation As Object = Nothing
        Dim hv_HomMat3d As Object = Nothing

        Dim N As Object = Nothing
        Dim BestScale As Object = Nothing
        Dim tmpScale As Object = Nothing
        Dim Error1 As Object = Nothing
        Dim Error2 As Object = Nothing
        Dim Error3 As Object = Nothing
        Dim tmpPose As Object = Nothing
        Dim tmpPose1 As Object = Nothing
        Dim tmpError As Object = Nothing

        Dim MidRow11 As Object = Nothing
        Dim MidCol11 As Object = Nothing
        Dim MidRow21 As Object = Nothing
        Dim MidCol21 As Object = Nothing
        epiDeviation = 0.03
        'カメラ１と３の姿勢を計算
        CalcRelPoseByMidPose(IS1, IS2)
        Error1 = IS2.MidPose.hError
        CalcRelPoseByMidPose(IS1, IS3)
        Error3 = IS3.MidPose.hError
        tmpPose1 = IS3.MidPose.RelPose
        tmpPose = IS3.MidPose.RelPose
        CalcRelPoseByMidPose(IS2, IS3)
        Error2 = IS3.MidPose.hError
        RelPose23 = IS3.MidPose.RelPose
        If IS1.flgFirst = True Then
            Pose2 = IS2.MidPose.RelPose
        Else
            Pose2 = IS1.VectorPose.RelPose
            hv_Kyori12 = BTuple.TupleSub(BTuple.TupleSelectRange(IS2.MidPose.RelPose, 0, 2), BTuple.TupleSelectRange(Pose2, 0, 2))
            hv_Kyori12 = BTuple.TupleSqrt(BTuple.TupleSum(BTuple.TuplePow(hv_Kyori12, 2)))
            CalcBadIndex_byRelPose(IS1, IS2, Pose2, BadIndex)
            If BTuple.TupleLength(BadIndex) = 0 Then
            Else
                Exit Sub
            End If
        End If

        'XYZ = IS2.MidPose.ConvertToPoint3d
        'CalcVectorToPose(XYZ, IS3, Pose3, Quality3)
        'CalcRelPoseBetweenTwoPose(BTuple.TupleFirstN(Pose2, 6), Pose3, RelPose23)
        'tmpPose1 = RelPose23
        'CalcUnitPose(RelPose23, tmpPose1)

        Try
            HOperatorSet.IntersectLinesOfSight(hv_CamparamOut, hv_CamparamOut, Pose2, IS1.RansacMid.RansacPoints.Row, _
                  IS1.RansacMid.RansacPoints.Col, IS2.RansacMid.RansacPoints.Row, IS2.RansacMid.RansacPoints.Col, XYZ12.X, XYZ12.Y, XYZ12.Z, Dist2)
        Catch ex As Exception
            CalcRelPoseByMidPose(IS1, IS2)
            Pose2 = IS2.MidPose.RelPose
            XYZ12 = IS2.MidPose.ConvertToPoint3d
        End Try

        XYZ23 = IS3.MidPose.ConvertToPoint3d

        ZahyoHenkan(Pose2, XYZ12, XYZ)

        ' CalcBestScale(XYZ, XYZ23, BestScale)
        CalcBestScaleWithWeight(XYZ, XYZ23, 10, BestScale)

        Dim T1 As Object = Nothing
        Dim T2 As Object = Nothing
        Dim XYZ12_T As New Point3D(XYZ)
        Dim XYZ23_T As New Point3D(XYZ23)


        'T1 = BTuple.TupleMean(XYZ12_T.Z)
        'XYZ12_T.Z = BTuple.TupleDiv(XYZ12_T.Z, T1)
        ''XYZ12_T.X = BTuple.TupleDiv(XYZ12_T.X, T1)
        ''XYZ12_T.Y = BTuple.TupleDiv(XYZ12_T.Y, T1)
        'T2 = BTuple.TupleMean(XYZ23_T.Z)
        'XYZ23_T.Z = BTuple.TupleDiv(XYZ23_T.Z, T1)
        ''XYZ23_T.X = BTuple.TupleDiv(XYZ23_T.X, T1)
        ''XYZ23_T.Y = BTuple.TupleDiv(XYZ23_T.Y, T1)

        'CalcBestScaleWithWeight(XYZ12_T, XYZ23_T, 10, tmpScale)
        '' CalcBestScale(XYZ12_T, XYZ23_T, tmpScale)
        'BestScale = tmpScale
        'XYZ = XYZ12_T
        'XYZ23 = XYZ23_T


        XYZ = XYZ23.SetScale2(BestScale)
        ' XYZ12.Z = BTuple.TupleDiv(XYZ12.Z, T1)


        ConnectPose(Pose2, RelPose23, BestScale, Pose3)

        CalcMatchingResult(IS1, IS2, IS3, Pose3, XYZ12, XYZ, Dist23, Kyori)

        T1 = Calc_any_scaling(XYZ.Z, 0.9, 1)
        If T1 Is Nothing Then
            hv_DistInd = BTuple.TupleSortIndex(Dist23)
        Else
            hv_DistInd = BTuple.TupleSortIndex(BTuple.TupleDiv(Dist23, T1))
        End If
        HOperatorSet.TupleMean(Dist23, hv_MeanVectorPose)
        HOperatorSet.TupleDeviation(Dist23, hv_DeviationVectorPose)

        If T1 Is Nothing Then
            HOperatorSet.TupleSortIndex(Kyori, hv_Indices1)
        Else
            HOperatorSet.TupleSortIndex(BTuple.TupleDiv(Kyori, T1), hv_Indices1)
        End If

        HOperatorSet.TupleMean(Kyori, hv_Mean)
        HOperatorSet.TupleMax(Kyori, hv_MaxDist)

        Scale = BestScale

        IS2.VectorPose.RelPose = RelPose23
        IS2.VectorPose.Pose = Pose2
        IS3.VectorPose.Pose = RelPose23

        'CalcUnitPose(Pose3, tmpPose)

        hv_Kyori13 = BTuple.TupleSub(BTuple.TupleSelectRange(tmpPose1, 0, 2), BTuple.TupleSelectRange(Pose3, 0, 2))
        hv_Kyori13 = BTuple.TupleSqrt(BTuple.TupleSum(BTuple.TuplePow(hv_Kyori13, 2)))
        hv_Kyori12 = BTuple.TupleSub(BTuple.TupleSelectRange(tmpPose1, 3, 5), BTuple.TupleSelectRange(Pose3, 3, 5))
        hv_Kyori12 = BTuple.TupleSqrt(BTuple.TupleSum(BTuple.TuplePow(hv_Kyori12, 2)))
        epiDeviation = (BTuple.TupleMean(XYZ12.Z) / 10) * epiDev

        If (hv_MeanVectorPose < epiDeviation * 2 And hv_DeviationVectorPose < epiDeviation) And (hv_Mean < MeanPixErr And hv_MaxDist < MaxPixErr) Then
            BadIndex = Nothing
            Exit Sub
        Else
            'BadIndex = BTuple.TupleSelect(hv_Indices1, BTuple.TupleSub(BTuple.TupleLength( _
            '                          hv_Indices1), 1))
            N = CInt(BTuple.TupleLength(hv_Indices1) * 0.98) - 1
            BadIndex1 = BTuple.TupleLastN(hv_DistInd, N)
            BadIndex2 = BTuple.TupleLastN(hv_Indices1, N)
            BadIndex = Nothing
            'For i = 0 To BTuple.TupleLength(BadIndex1) - 1
            '    Dim tmp As Object = Nothing
            '    tmp = BTuple.TupleFind(BadIndex1, BTuple.TupleSelect(BadIndex2, i))
            '    If BTuple.TupleSelect(tmp, 0) = -1 Then
            '    Else
            '        BadIndex = BTuple.TupleConcat(BadIndex, BTuple.TupleSelect(BadIndex2, i))
            '    End If
            'Next
            If BTuple.TupleLength(BadIndex) = 0 Then
                BadIndex = BTuple.TupleUniq(BTuple.TupleSort(BTuple.TupleConcat(BadIndex1, BadIndex2)))
            End If
        End If
    End Sub
    Private Function Calc_any_scaling(ByVal Data As Object, ByVal Smin As Double, ByVal Smax As Double) As Object
        Dim DMin As Object = Nothing
        Dim DMax As Object = Nothing
        Dim Dsub As Object = Nothing
        Dim Ssub As Double = Smax - Smin
        Dim hiritu As Double = 0
        Calc_any_scaling = Nothing
        If Smin > Smax Or Math.Abs(Smin - Smax) < Double.Epsilon Then
            Exit Function
        End If
        DMax = BTuple.TupleMax(Data)
        DMin = BTuple.TupleMin(Data)
        Dsub = BTuple.TupleSub(Data, DMin)
        hiritu = Ssub / (DMax - DMin)
        Calc_any_scaling = BTuple.TupleAdd(BTuple.TupleMult(Dsub, hiritu), Smin)

    End Function
    Private Sub CalcMatchingResult(ByRef IS1 As ImageSet, ByRef IS2 As ImageSet, ByRef IS3 As ImageSet, ByVal Pose3 As Object, ByVal XYZ12 As Point3D, ByVal XYZ23 As Point3D, _
                                  ByRef Dist As Object, ByRef Kyori As Object)
        Dim XYZ As New Point3D
        Dim hv_HomMat3d As Object = Nothing
        hom_mat_3d_from_3d_3d_point_correspondence(XYZ23.X, XYZ23.Y, XYZ23.Z, XYZ12.X, XYZ12.Y, XYZ12.Z, hv_HomMat3d)
        'correspond_3d_3d_point_withweight(XYZ23.X, XYZ23.Y, XYZ23.Z, XYZ12.X, XYZ12.Y, XYZ12.Z, 10, 0.00000000001, hv_HomMat3d)

        HOperatorSet.AffineTransPoint3d(hv_HomMat3d, XYZ23.X, XYZ23.Y, XYZ23.Z, XYZ23.X, XYZ23.Y, XYZ23.Z)
        XYZ12.GetDisttoOtherPose(XYZ23, Dist)

        If BTuple.TupleLength(XYZ12.X) = 0 Or BTuple.TupleLength(XYZ23.X) = 0 Then
            CalcRelPoseByMidPose(IS1, IS2)

            XYZ = IS2.MidPose.ConvertToPoint3d
            CalcReProjectError(Pose3, XYZ, IS3, Kyori)
        Else
            CalcReProjectError(Pose3, XYZ12, IS3, Kyori)
        End If

    End Sub

    Private Sub ConnectPose(ByVal Pose1 As Object, ByVal Pose2 As Object, ByVal Scale As Object, ByRef ConPose As Object)
        Dim HomMat3d1 As Object = Nothing
        Dim HomMat3d2 As Object = Nothing
        Dim HomMat3dScale As Object = Nothing
        Dim HomMat3dCompose As Object = Nothing
        HOperatorSet.PoseToHomMat3d(Pose1, HomMat3d1)
        HOperatorSet.PoseToHomMat3d(Pose2, HomMat3d2)
        HOperatorSet.HomMat3dCompose(HomMat3d1, HomMat3d2, HomMat3dCompose)
        HOperatorSet.HomMat3dScale(HomMat3dCompose, Scale, Scale, Scale, BTuple.TupleSelect(Pose1, 0), BTuple.TupleSelect(Pose1, 1), BTuple.TupleSelect(Pose1, 2), HomMat3dScale)
        'HOperatorSet.HomMat3dCompose(HomMat3d1, HomMat3dScale, HomMat3dCompose)
        HOperatorSet.HomMat3dToPose(HomMat3dScale, ConPose)

        'Dim tplPoseScale As Object = Nothing
        'tplPoseScale = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat( _
        '                       BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(Scale, Scale), Scale), 1), 1), 1), 1)
        'Pose2 = BTuple.TupleMult(Pose2, tplPoseScale)
        'HOperatorSet.PoseToHomMat3d(Pose1, HomMat3d1)
        'HOperatorSet.PoseToHomMat3d(Pose2, HomMat3d2)
        'HOperatorSet.HomMat3dCompose(HomMat3d1, HomMat3d2, HomMat3dCompose)
        'HOperatorSet.HomMat3dToPose(HomMat3dCompose, ConPose)
    End Sub

    Private Sub CalcVectorToPose(ByVal XYZ As Point3D, ByRef IS2 As ImageSet, ByRef Pose As Object, ByRef Quality As Object)
        Dim hv_PoseOut As New Object

        HOperatorSet.VectorToPose(XYZ.X, XYZ.Y, XYZ.Z, IS2.RansacMid.RansacPoints.Row, IS2.RansacMid.RansacPoints.Col, _
          hv_CamparamOut, "iterative", "error", Pose, Quality)

        HOperatorSet.ConvertPoseType(Pose, "R(p-T)", "abg", "coordinate_system", hv_PoseOut)
        HOperatorSet.TupleMult(hv_PoseOut, BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat( _
                                 BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(1, 1), 1), 1), 1), 1), 0), Pose)

    End Sub

    Private Sub CalcReProjectError(ByVal Pose As Object, ByVal XYZ As Point3D, ByRef IS3 As ImageSet, ByRef Kyori As Object)
        Dim hv_HomMat3D As New Object
        Dim hv_HomMat3DInvert As New Object
        Dim TPoint As New Point3D
        Dim hv_Row As New Object
        Dim hv_Column As New Object
        Dim hv_DiffR As New Object
        Dim hv_DiffC As New Object
        Dim hv_PowR As New Object
        Dim hv_PowC As New Object
        Dim hv_SumRC As New Object

        HOperatorSet.PoseToHomMat3d(Pose, hv_HomMat3D)
        HOperatorSet.HomMat3dInvert(hv_HomMat3D, hv_HomMat3DInvert)
        HOperatorSet.AffineTransPoint3d(hv_HomMat3DInvert, XYZ.X, XYZ.Y, XYZ.Z, _
            TPoint.X, TPoint.Y, TPoint.Z)
        HOperatorSet.Project3dPoint(TPoint.X, TPoint.Y, TPoint.Z, hv_CamparamOut, hv_Row, hv_Column)
        HOperatorSet.TupleSub(hv_Row, IS3.RansacMid.RansacPoints.Row, hv_DiffR)
        HOperatorSet.TupleSub(hv_Column, IS3.RansacMid.RansacPoints.Col, hv_DiffC)
        HOperatorSet.TuplePow(hv_DiffR, 2, hv_PowR)
        HOperatorSet.TuplePow(hv_DiffC, 2, hv_PowC)
        HOperatorSet.TupleAdd(hv_PowR, hv_PowC, hv_SumRC)
        HOperatorSet.TupleSqrt(hv_SumRC, Kyori)
        'IS3.RansacMid.RansacPoints.Row = hv_Row
        'IS3.RansacMid.RansacPoints.Col = hv_Column

    End Sub
    Private Sub ZahyoHenkan(ByRef IS1 As ImageSet, ByRef MidPoints As Point3D)
        Dim HomMat3d As New Object
        Dim HomMat3dInvert As New Object
        HOperatorSet.PoseToHomMat3d(BTuple.TupleFirstN(IS1.ImagePose.RelPose, 6), HomMat3d)
        HOperatorSet.HomMat3dInvert(HomMat3d, HomMat3dInvert)
        HOperatorSet.AffineTransPoint3d(HomMat3dInvert, IS1.MidPose.X, IS1.MidPose.Y, IS1.MidPose.Z, MidPoints.X, MidPoints.Y, MidPoints.Z)

    End Sub

    Private Sub ZahyoHenkan(ByVal Pose As Object, ByVal inputPoints As Point3D, ByRef Points As Point3D)
        Dim HomMat3d As New Object
        Dim HomMat3dInvert As New Object
        HOperatorSet.PoseToHomMat3d(BTuple.TupleFirstN(Pose, 6), HomMat3d)
        HOperatorSet.HomMat3dInvert(HomMat3d, HomMat3dInvert)
        HOperatorSet.AffineTransPoint3d(HomMat3dInvert, inputPoints.X, inputPoints.Y, inputPoints.Z, Points.X, Points.Y, Points.Z)
    End Sub
    Private Sub CalcConnectedPose(ByRef IS1 As ImageSet, ByRef IS2 As ImageSet, ByVal Scale As Double)
        Dim tmpPose As New Object
        Dim conPose As New Object
        Dim homMat3d_23 As New Object
        Dim homMat3d_12 As New Object
        Dim homMat3d_Composed As New Object
        Dim t_x As New Object
        Dim t_y As New Object
        Dim t_z As New Object
        Dim t_a As New Object
        Dim t_b As New Object
        Dim t_c As New Object
        t_x = BTuple.TupleSelect(IS2.MidPose.RelPose, 0) * Scale
        t_y = BTuple.TupleSelect(IS2.MidPose.RelPose, 1) * Scale
        t_z = BTuple.TupleSelect(IS2.MidPose.RelPose, 2) * Scale
        t_a = BTuple.TupleSelect(IS2.MidPose.RelPose, 3)
        t_b = BTuple.TupleSelect(IS2.MidPose.RelPose, 4)
        t_c = BTuple.TupleSelect(IS2.MidPose.RelPose, 5)
        tmpPose = Nothing
        tmpPose = BTuple.TupleConcat(t_x, t_y)
        tmpPose = BTuple.TupleConcat(tmpPose, t_z)
        tmpPose = BTuple.TupleConcat(tmpPose, t_a)
        tmpPose = BTuple.TupleConcat(tmpPose, t_b)
        tmpPose = BTuple.TupleConcat(tmpPose, t_c)
        tmpPose = BTuple.TupleConcat(tmpPose, 0)
        HOperatorSet.PoseToHomMat3d(IS1.ImagePose.Pose, homMat3d_12)
        HOperatorSet.PoseToHomMat3d(tmpPose, homMat3d_23)
        HOperatorSet.HomMat3dCompose(homMat3d_12, homMat3d_23, homMat3d_Composed)
        HOperatorSet.HomMat3dToPose(homMat3d_Composed, conPose)
        IS2.ImagePose.Pose = conPose

        IS2.MidPose.RelPose = tmpPose
    End Sub

    Private Sub CalcConnectedPose1(ByRef IS1 As ImageSet, ByRef IS2 As ImageSet, ByVal Scale As Double)
        Dim tmpPose As New Object
        Dim conPose As New Object
        Dim homMat3d_23 As New Object
        Dim homMat3d_12 As New Object
        Dim homMat3d_Composed As New Object
        Dim t_x As New Object
        Dim t_y As New Object
        Dim t_z As New Object
        Dim t_a As New Object
        Dim t_b As New Object
        Dim t_c As New Object
        t_x = BTuple.TupleSelect(IS2.ImagePose.RelPose, 0) * Scale
        t_y = BTuple.TupleSelect(IS2.ImagePose.RelPose, 1) * Scale
        t_z = BTuple.TupleSelect(IS2.ImagePose.RelPose, 2) * Scale
        t_a = BTuple.TupleSelect(IS2.ImagePose.RelPose, 3)
        t_b = BTuple.TupleSelect(IS2.ImagePose.RelPose, 4)
        t_c = BTuple.TupleSelect(IS2.ImagePose.RelPose, 5)
        tmpPose = Nothing
        tmpPose = BTuple.TupleConcat(t_x, t_y)
        tmpPose = BTuple.TupleConcat(tmpPose, t_z)
        tmpPose = BTuple.TupleConcat(tmpPose, t_a)
        tmpPose = BTuple.TupleConcat(tmpPose, t_b)
        tmpPose = BTuple.TupleConcat(tmpPose, t_c)
        tmpPose = BTuple.TupleConcat(tmpPose, 0)
        HOperatorSet.PoseToHomMat3d(IS1.ImagePose.Pose, homMat3d_12)
        HOperatorSet.PoseToHomMat3d(tmpPose, homMat3d_23)
        HOperatorSet.HomMat3dCompose(homMat3d_12, homMat3d_23, homMat3d_Composed)
        HOperatorSet.HomMat3dToPose(homMat3d_Composed, conPose)
        IS2.ImagePose.Pose = conPose

        ' IS2.MidPose.RelPose = tmpPose
    End Sub

    'Public Sub CalcScale3d(ByRef IS1 As ImageSet, ByRef IS2 As ImageSet, ByRef Kyori As Double)

    '    Dim P1 As New Point3D
    '    Dim P2 As New Point3D
    '    Dim Dist As New Object

    '    HOperatorSet.IntersectLinesOfSight(hv_CamparamOut, hv_CamparamOut, IS2.ImagePose.RelPose, IS1.ScalePoint1.Row, IS1.ScalePoint1.Col, _
    '                                IS2.ScalePoint1.Row, IS2.ScalePoint1.Col, P1.X, P1.Y, P1.Z, Dist)
    '    HOperatorSet.IntersectLinesOfSight(hv_CamparamOut, hv_CamparamOut, IS2.ImagePose.RelPose, IS1.ScalePoint2.Row, IS1.ScalePoint2.Col, _
    '                               IS2.ScalePoint2.Row, IS2.ScalePoint2.Col, P2.X, P2.Y, P2.Z, Dist)
    '    P1.GetDisttoOtherPose(P2, Kyori)
    'End Sub
    'Public Sub Calc3dPoint(ByRef IS1 As ImageSet, ByRef IS2 As ImageSet, ByRef Pnt As Point3D)

    '    Dim Dist As New Object

    '    HOperatorSet.IntersectLinesOfSight(hv_CamparamOut, hv_CamparamOut, IS2.ImagePose.RelPose, IS1.ScalePoint1.Row, IS1.ScalePoint1.Col, _
    '                                IS2.ScalePoint1.Row, IS2.ScalePoint1.Col, Pnt.X, Pnt.Y, Pnt.Z, Dist)

    'End Sub

    Public Sub Get3d(ByVal strFilename As String)
        '  Dim i As Integer
        Dim j As Integer
        Dim hv_Number As New Object
        Dim hv_i As New Object

        If ScaleMM = 1 Then
            ScaleMM = 100
        End If

        Dim hv_Filehandle As New Object
        HOperatorSet.OpenFile(strFilename & "\CameraPose.scr", "output", hv_Filehandle)
        For Each ISI As ImageSet In lstImages
            If j = 0 Then
            Else
                hv_Number = BTuple.TupleLength(ISI.ImagePose.X)
                HOperatorSet.FwriteString(hv_Filehandle, BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleAdd( _
                    BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleAdd("CIRCLE ", BTuple.TupleMult( _
                    BTuple.TupleSelect(ISI.ImagePose.Pose, 0), ScaleMM)), ","), BTuple.TupleMult( _
                    BTuple.TupleSelect(ISI.ImagePose.Pose, 1), ScaleMM)), ","), BTuple.TupleMult( _
                    BTuple.TupleSelect(ISI.ImagePose.Pose, 2), ScaleMM)), " 5"))
                HOperatorSet.FnewLine(hv_Filehandle)
            End If

            j += 1
        Next
        HOperatorSet.CloseFile(hv_Filehandle)
        HOperatorSet.OpenFile(strFilename & "\FeaturePoints.scr", "output", hv_Filehandle)
        hv_Number = BTuple.TupleLength(WorldXYZ.X)
        For hv_i = 1 To hv_Number Step 1
            HOperatorSet.FwriteString(hv_Filehandle, BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleAdd( _
                BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleAdd("CIRCLE ", BTuple.TupleMult( _
                BTuple.TupleSelect(WorldXYZ.X, BTuple.TupleSub(hv_i, 1)), ScaleMM)), ","), BTuple.TupleMult( _
                BTuple.TupleSelect(WorldXYZ.Y, BTuple.TupleSub(hv_i, 1)), ScaleMM)), ","), BTuple.TupleMult( _
                BTuple.TupleSelect(WorldXYZ.Z, BTuple.TupleSub(hv_i, 1)), ScaleMM)), " 2"))
            HOperatorSet.FnewLine(hv_Filehandle)
        Next

        HOperatorSet.CloseFile(hv_Filehandle)

    End Sub

    Public Sub gen_epiline(ByRef ho_EpiLine As HObject, ByVal hv_RelPose As Object, _
   ByVal hv_CovRelPose As Object, ByVal hv_CamPar As Object, ByVal hv_Width As Object, _
   ByVal hv_Row As Object, ByVal hv_Column As Object)
        ' Local control variables 
        Dim hv_FMatrix As Object = Nothing, hv_CovFMat As Object = Nothing
        Dim hv_MatrixF As Object = Nothing, hv_Tcount As Object = Nothing
        Dim hv_Ones As Object = Nothing, hv_MatrixID As Object = Nothing
        Dim hv_MatrixMultID As Object = Nothing, hv_Values As Object = Nothing
        Dim hv_V1 As Object = Nothing, hv_V2 As Object = Nothing
        Dim hv_V3 As Object = Nothing, hv_C1 As Object = Nothing
        Dim hv_C2 As Object = Nothing, hv_R1 As Object = Nothing
        Dim hv_R2 As Object = Nothing

        ' Initialize local and output iconic variables 
        HOperatorSet.GenEmptyObj(ho_EpiLine)
        CalcUnitPose(hv_RelPose, hv_RelPose)
        HOperatorSet.RelPoseToFundamentalMatrix(hv_RelPose, hv_CovRelPose, hv_CamPar, hv_CamPar, _
            hv_FMatrix, hv_CovFMat)
        HOperatorSet.CreateMatrix(3, 3, hv_FMatrix, hv_MatrixF)
        hv_Tcount = BTuple.TupleLength(hv_Row)
        HOperatorSet.TupleGenConst(hv_Tcount, 1, hv_Ones)
        HOperatorSet.CreateMatrix(3, hv_Tcount, BTuple.TupleConcat(BTuple.TupleConcat(hv_Column, hv_Row), _
            hv_Ones), hv_MatrixID)
        HOperatorSet.MultMatrix(hv_MatrixF, hv_MatrixID, "AB", hv_MatrixMultID)
        HOperatorSet.GetFullMatrix(hv_MatrixMultID, hv_Values)
        hv_V1 = BTuple.TupleSelectRange(hv_Values, 0, BTuple.TupleSub(hv_Tcount, 1))
        hv_V2 = BTuple.TupleSelectRange(hv_Values, hv_Tcount, BTuple.TupleSub(BTuple.TupleMult( _
            hv_Tcount, 2), 1))
        hv_V3 = BTuple.TupleSelectRange(hv_Values, BTuple.TupleMult(hv_Tcount, 2), BTuple.TupleSub( _
            BTuple.TupleMult(hv_Tcount, 3), 1))
        HOperatorSet.TupleGenConst(hv_Tcount, hv_Width, hv_C1)
        HOperatorSet.TupleGenConst(hv_Tcount, 0, hv_C2)
        hv_R1 = BTuple.TupleDiv(BTuple.TupleSub(BTuple.TupleMult(-1, hv_V3), BTuple.TupleMult( _
            hv_V1, hv_C1)), hv_V2)
        hv_R2 = BTuple.TupleDiv(BTuple.TupleSub(BTuple.TupleMult(-1, hv_V3), BTuple.TupleMult( _
            hv_V1, hv_C2)), hv_V2)
        Try
            'HOperatorSet.GenRegionLine(ho_EpiLine, hv_R1, hv_C1, hv_R2, hv_C2)
            'HOperatorSet.GenContourRegionXld(ho_EpiLine, ho_EpiLine, "center")
            HOperatorSet.ClearObj(ho_EpiLine)
            HOperatorSet.GenContourPolygonXld(ho_EpiLine, BTuple.TupleConcat(hv_R1, hv_R2), BTuple.TupleConcat(hv_C1, hv_C2))

        Catch ex As Exception

        End Try

        HOperatorSet.ClearMatrix(hv_MatrixMultID)
        HOperatorSet.ClearMatrix(hv_MatrixF)
        HOperatorSet.ClearMatrix(hv_MatrixID)

        Exit Sub
    End Sub

    Public Sub gen_epiline(ByVal hv_Fmat As Object, ByVal hv_Width As Object, ByVal hv_Row As Object, ByVal hv_Column As Object, _
                             ByRef hv_R1 As Object, ByRef hv_C1 As Object, ByRef hv_R2 As Object, ByRef hv_C2 As Object)

        ' Local control variables 
        Dim hv_MatrixF As Object = Nothing, hv_Tcount As Object = Nothing
        Dim hv_Ones As Object = Nothing, hv_MatrixID As Object = Nothing
        Dim hv_MatrixMultID As Object = Nothing, hv_Values As Object = Nothing
        Dim hv_V1 As Object = Nothing, hv_V2 As Object = Nothing
        Dim hv_V3 As Object = Nothing

        ' Initialize local and output iconic variables 

        HOperatorSet.CreateMatrix(3, 3, hv_Fmat, hv_MatrixF)
        hv_Tcount = BTuple.TupleLength(hv_Row)
        HOperatorSet.TupleGenConst(hv_Tcount, 1, hv_Ones)
        HOperatorSet.CreateMatrix(3, hv_Tcount, BTuple.TupleConcat(BTuple.TupleConcat(hv_Column, hv_Row), _
            hv_Ones), hv_MatrixID)
        HOperatorSet.MultMatrix(hv_MatrixF, hv_MatrixID, "AB", hv_MatrixMultID)
        HOperatorSet.GetFullMatrix(hv_MatrixMultID, hv_Values)
        hv_V1 = BTuple.TupleSelectRange(hv_Values, 0, BTuple.TupleSub(hv_Tcount, 1))
        hv_V2 = BTuple.TupleSelectRange(hv_Values, hv_Tcount, BTuple.TupleSub(BTuple.TupleMult( _
            hv_Tcount, 2), 1))
        hv_V3 = BTuple.TupleSelectRange(hv_Values, BTuple.TupleMult(hv_Tcount, 2), BTuple.TupleSub( _
            BTuple.TupleMult(hv_Tcount, 3), 1))
        HOperatorSet.TupleGenConst(hv_Tcount, hv_Width, hv_C1)
        HOperatorSet.TupleGenConst(hv_Tcount, 0, hv_C2)
        hv_R1 = BTuple.TupleDiv(BTuple.TupleSub(BTuple.TupleMult(-1, hv_V3), BTuple.TupleMult( _
            hv_V1, hv_C1)), hv_V2)
        hv_R2 = BTuple.TupleDiv(BTuple.TupleSub(BTuple.TupleMult(-1, hv_V3), BTuple.TupleMult( _
            hv_V1, hv_C2)), hv_V2)

        HOperatorSet.ClearMatrix(hv_MatrixMultID)
        HOperatorSet.ClearMatrix(hv_MatrixF)
        HOperatorSet.ClearMatrix(hv_MatrixID)

        Exit Sub
    End Sub

    Public Sub gen_epiline(ByVal hv_Fmat As Object, ByVal hv_Width As Object, ByVal hv_Height As Object, ByVal hv_Row As Object, ByVal hv_Column As Object, _
                           ByRef hv_R1 As Object, ByRef hv_C1 As Object, ByRef hv_R2 As Object, ByRef hv_C2 As Object)

        ' Local control variables 
        Dim hv_MatrixF As Object = Nothing, hv_Tcount As Object = Nothing
        Dim hv_Ones As Object = Nothing, hv_MatrixID As Object = Nothing
        Dim hv_MatrixMultID As Object = Nothing, hv_Values As Object = Nothing
        Dim hv_V1 As Object = Nothing, hv_V2 As Object = Nothing
        Dim hv_V3 As Object = Nothing

        ' Initialize local and output iconic variables 

        HOperatorSet.CreateMatrix(3, 3, hv_Fmat, hv_MatrixF)
        hv_Tcount = BTuple.TupleLength(hv_Row)
        HOperatorSet.TupleGenConst(hv_Tcount, 1, hv_Ones)
        HOperatorSet.CreateMatrix(3, hv_Tcount, BTuple.TupleConcat(BTuple.TupleConcat(hv_Column, hv_Row), _
            hv_Ones), hv_MatrixID)
        HOperatorSet.MultMatrix(hv_MatrixF, hv_MatrixID, "AB", hv_MatrixMultID)
        HOperatorSet.GetFullMatrix(hv_MatrixMultID, hv_Values)
        hv_V1 = BTuple.TupleSelectRange(hv_Values, 0, BTuple.TupleSub(hv_Tcount, 1))
        hv_V2 = BTuple.TupleSelectRange(hv_Values, hv_Tcount, BTuple.TupleSub(BTuple.TupleMult( _
            hv_Tcount, 2), 1))
        hv_V3 = BTuple.TupleSelectRange(hv_Values, BTuple.TupleMult(hv_Tcount, 2), BTuple.TupleSub( _
            BTuple.TupleMult(hv_Tcount, 3), 1))
        'HOperatorSet.TupleGenConst(hv_Tcount, hv_Width, hv_C1)
        'HOperatorSet.TupleGenConst(hv_Tcount, 0, hv_C2)
        hv_C1 = hv_Width
        hv_C2 = 0

        hv_R1 = BTuple.TupleDiv(BTuple.TupleSub(BTuple.TupleMult(-1, hv_V3), BTuple.TupleMult( _
            hv_V1, hv_C1)), hv_V2)
        hv_R2 = BTuple.TupleDiv(BTuple.TupleSub(BTuple.TupleMult(-1, hv_V3), BTuple.TupleMult( _
            hv_V1, hv_C2)), hv_V2)
        If hv_R1 > 0 And hv_R1 < hv_Height And hv_R2 > 0 And hv_R2 < hv_Height Then
        Else
            '  HOperatorSet.TupleGenConst(hv_Tcount, hv_Height, hv_R1)
            ' HOperatorSet.TupleGenConst(hv_Tcount, 0, hv_R2)
            hv_R1 = hv_Height
            hv_R2 = 0

            hv_C1 = BTuple.TupleDiv(BTuple.TupleSub(BTuple.TupleMult(-1, hv_V3), BTuple.TupleMult( _
                                hv_V2, hv_R1)), hv_V1)
            hv_C2 = BTuple.TupleDiv(BTuple.TupleSub(BTuple.TupleMult(-1, hv_V3), BTuple.TupleMult( _
                                hv_V2, hv_R2)), hv_V1)
            'If hv_C1 > 5000 Or hv_C2 > 5000 Then
            '    hv_C1 = hv_C1
            'End If
        End If
        HOperatorSet.ClearMatrix(hv_MatrixMultID)
        HOperatorSet.ClearMatrix(hv_MatrixF)
        HOperatorSet.ClearMatrix(hv_MatrixID)

        Exit Sub
    End Sub

    'Private Sub MP_SaveToDB()
    '    Dim i As Integer
    '    i = 0
    '    dbClass.DoDelete("3DPoints")
    '    For Each MP As MeasurePoint In lst3dPoints
    '        i += 1
    '        MP.SaveMP_toDb(i, ScaleMM, TCS_HomMat3d)

    '    Next

    'End Sub

    Private Sub Obj_SaveToDB()

        dbClass.DoDelete("Objects")
        For Each Obj As Objects In lstObjects
            Obj.SaveObj_toDb()
        Next
    End Sub

    Private Sub Obj_ReadFromDB()

        Dim strSql As String
        Dim IDR As IDataReader

        If lstObjects Is Nothing Then
            lstObjects = New List(Of Objects)
        Else
            lstObjects.Clear()
        End If

        strSql = "SELECT Object_ID, Object_Name, Object_Type, Point1_ID, Point2_ID,Point3_ID FROM Objects ORDER BY Object_ID"

        IDR = dbClass.DoSelect(strSql)
        If Not IDR Is Nothing Then

            Do While IDR.Read
                Dim Obj As New Objects
                Obj.ReadObj_toDb(IDR)
                lstObjects.Add(Obj)
            Loop
            IDR.Close()
        End If


    End Sub

    Public Function GenObjectsProjectedTargetsToThisImage(ByVal index As Integer) As HObject
        Dim HomMat3dInvert As Object = Nothing
        Dim TransPnt As New Point3D
        Dim ProjectedImagePnt As New ImagePoints
        Dim tRow(lstCommon3dCT.Count * CodedTarget.CTnoSTnum + lstCommon3dST.Count) As Double
        Dim tCol(lstCommon3dCT.Count * CodedTarget.CTnoSTnum + lstCommon3dST.Count) As Double

        Dim Row As Object = Nothing
        Dim Col As Object = Nothing
        HOperatorSet.PoseToHomMat3d(lstImages.Item(index).ImagePose.Pose, HomMat3dInvert)
        'HomMat3dInvert = lstImages.Item(index + 1).CommonHomMat3d

        HOperatorSet.HomMat3dInvert(HomMat3dInvert, HomMat3dInvert)

        GenObjectsProjectedTargetsToThisImage = New HObject
        HOperatorSet.GenEmptyObj(GenObjectsProjectedTargetsToThisImage)

        'For Each C3DT As Common3DSingleTarget In lstCommon3dST
        '    HOperatorSet.AffineTransPoint3D(HomMat3dInvert, C3DT.P3d.X, C3DT.P3d.Y, C3DT.P3d.Z, TransPnt.X, TransPnt.Y, TransPnt.Z)
        '    HOperatorSet.Project3DPoint(TransPnt.X, TransPnt.Y, TransPnt.Z, hv_CamparamOut, Row, Col)
        '    C3DT.tmpImgPnt.Row = Row
        '    C3DT.tmpImgPnt.Col = Col
        '    tRow = BTuple.TupleConcat(tRow, Row)
        '    tCol = BTuple.TupleConcat(tCol, Col)
        'Next
        Dim i As Integer
        Dim j As Integer = 0
        Dim flgAri As Boolean = False
        For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
            flgAri = False
            For Each CT As CodedTarget In C3DCT.lstCT
                If CT.ImageID = lstImages.Item(index).ImageId Then
                    flgAri = True
                    Exit For
                End If
            Next

            If C3DCT.lstP3d.Count = CodedTarget.CTnoSTnum And flgAri = True Then


                For i = 0 To CodedTarget.CTnoSTnum - 1
                    HOperatorSet.AffineTransPoint3d(HomMat3dInvert, C3DCT.lstP3d.Item(i).X, C3DCT.lstP3d.Item(i).Y, C3DCT.lstP3d.Item(i).Z, TransPnt.X, TransPnt.Y, TransPnt.Z)
                    HOperatorSet.Project3dPoint(TransPnt.X, TransPnt.Y, TransPnt.Z, hv_CamparamOut, Row, Col)
                    If i = 0 Then
                        C3DCT.tmpImgPnt.Row = Row
                        C3DCT.tmpImgPnt.Col = Col
                    End If
                    tRow(j) = Row
                    tCol(j) = Col
                    j += 1
                Next
            End If
        Next
        For Each C3DST As Common3DSingleTarget In lstCommon3dST
            flgAri = False
            If C3DST.PID <> -1 Then
                For Each ST As SingleTarget In C3DST.lstST
                    If ST.ImageID = lstImages.Item(index).ImageId Then
                        flgAri = True
                        Exit For
                    End If
                Next
                If flgAri = True Then
                    HOperatorSet.AffineTransPoint3d(HomMat3dInvert, C3DST.P3d.X, C3DST.P3d.Y, C3DST.P3d.Z, TransPnt.X, TransPnt.Y, TransPnt.Z)
                    HOperatorSet.Project3dPoint(TransPnt.X, TransPnt.Y, TransPnt.Z, hv_CamparamOut, Row, Col)
                    C3DST.tmpImgPnt.Row = Row
                    C3DST.tmpImgPnt.Col = Col
                    tRow(j) = Row
                    tCol(j) = Col
                    j += 1
                End If

            End If
        Next
        ReDim Preserve tRow(j - 1)
        ReDim Preserve tCol(j - 1)

        HOperatorSet.GenCrossContourXld(GenObjectsProjectedTargetsToThisImage, tRow, tCol, CrossSize, CrossAngle)

    End Function

    Public Function GenObjectsProjectedToThisImage(ByVal index As Integer, ByRef Frame As Object) As HObject
        Dim HomMat3dInvert As Object = Nothing
        Dim TransPnt As New Point3D
        Dim ProjectedImagePnt As New ImagePoints
        Dim Cross As New HObject
        Dim tRow As Object = Nothing
        Dim tCol As Object = Nothing

        HOperatorSet.PoseToHomMat3d(lstImages.Item(index).ImagePose.Pose, HomMat3dInvert)
        'HomMat3dInvert = lstImages.Item(index + 1).CommonHomMat3d

        HOperatorSet.HomMat3dInvert(HomMat3dInvert, HomMat3dInvert)

        GenObjectsProjectedToThisImage = New HObject
        HOperatorSet.GenEmptyObj(GenObjectsProjectedToThisImage)

        For Each MP As MeasurePoint In lst3dPoints
            HOperatorSet.AffineTransPoint3d(HomMat3dInvert, MP.Pnt.X, MP.Pnt.Y, MP.Pnt.Z, TransPnt.X, TransPnt.Y, TransPnt.Z)
            HOperatorSet.Project3dPoint(TransPnt.X, TransPnt.Y, TransPnt.Z, hv_CamparamOut, MP.tmpImagePoint.Row, MP.tmpImagePoint.Col)
            tRow = BTuple.TupleConcat(tRow, MP.tmpImagePoint.Row)
            tCol = BTuple.TupleConcat(tCol, MP.tmpImagePoint.Col)
        Next

        Frame = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat( _
               BTuple.TupleMin(tRow), BTuple.TupleMin(tCol)), _
                             BTuple.TupleMax(tRow)), BTuple.TupleMax(tCol))

        HOperatorSet.GenCrossContourXld(GenObjectsProjectedToThisImage, tRow, tCol, CrossSize, CrossAngle)

        For Each Obj As Objects In lstObjects
            If Obj.ObjectType = 1 Then
                HOperatorSet.ClearObj(Cross)
                ProjectedImagePnt.Row = BTuple.TupleConcat(lst3dPoints.Item(Obj.P1_ID).tmpImagePoint.Row, lst3dPoints.Item(Obj.P2_ID).tmpImagePoint.Row)
                ProjectedImagePnt.Col = BTuple.TupleConcat(lst3dPoints.Item(Obj.P1_ID).tmpImagePoint.Col, lst3dPoints.Item(Obj.P2_ID).tmpImagePoint.Col)
                HOperatorSet.GenContourPolygonXld(Cross, ProjectedImagePnt.Row, ProjectedImagePnt.Col)
                HOperatorSet.ConcatObj(GenObjectsProjectedToThisImage, Cross, GenObjectsProjectedToThisImage)
            End If
        Next
        HOperatorSet.ClearObj(Cross)

    End Function

    Public Function GenRappedImage(ByVal index As Integer) As HObject
        GenRappedImage = New HObject
        HOperatorSet.GenEmptyObj(GenRappedImage)

    End Function

    Public Sub DeletePoint(ByVal i As Integer, ByVal HitPointR As Object, ByVal HitPointC As Object)
        Dim cntMid As Integer
        Dim Distance As Object = Nothing
        Dim NearPointIndex As Object = Nothing
        Dim IS1 As ImageSet = lstImages.Item(i - 2)
        Dim IS2 As ImageSet = lstImages.Item(i - 1)
        Dim IS3 As ImageSet = lstImages.Item(i)

        cntMid = GetMidPointsOf3Image1(IS1, IS2, IS3)
        IS3.RansacMid.RansacPoints.CalcDistToInputPoint(HitPointR, HitPointC, Distance)
        NearPointIndex = BTuple.TupleSelect(BTuple.TupleSortIndex(Distance), 0)
        RemoveBadPoints(IS1, IS2, IS3, NearPointIndex)
    End Sub

    Public Sub DeletePoint(ByVal i As Integer, ByVal DelRegion As HObject)
        'Dim DelPointIndex As Object = Nothing
        'Dim IS1 As ImageSet = lstImages.Item(i - 2)
        'Dim IS2 As ImageSet = lstImages.Item(i - 1)
        'Dim IS3 As ImageSet = lstImages.Item(i)
        'Dim DelCont As HObject = Nothing
        'HOperatorSet.GenEmptyObj(DelCont)
        'HOperatorSet.ClearObj(DelCont)
        'HOperatorSet.GenContourRegionXld(DelRegion, DelCont, "border")
        'GetMidPointsOf3Image1(IS1, IS2, IS3)

        'HOperatorSet.TestXldPoint(DelCont, IS3.RansacMid.RansacPoints.Row, IS3.RansacMid.RansacPoints.Col, DelPointIndex)
        'DelPointIndex = BTuple.TupleFind(DelPointIndex, 1)
        'If BTuple.TupleSelect(DelPointIndex, 0) <> -1 Then
        '    RemoveBadPoints(IS1, IS2, IS3, DelPointIndex)

        '    IS1.RansacFirstIndexBack = IS1.RansacFirst.RansacPointsIndex
        '    IS2.RansacSecondIndexBack = IS2.RansacSecond.RansacPointsIndex
        '    IS2.RansacFirstIndexBack = IS2.RansacFirst.RansacPointsIndex
        '    IS3.RansacSecondIndexBack = IS3.RansacSecond.RansacPointsIndex
        '    HOperatorSet.ClearObj(DelCont)
        'End If

        Dim DelPointIndex As Object = Nothing
        Dim IS1 As ImageSet = lstImages.Item(i)

        Dim DelCont As HObject = Nothing
        HOperatorSet.GenEmptyObj(DelCont)
        HOperatorSet.ClearObj(DelCont)
        HOperatorSet.GenContourRegionXld(DelRegion, DelCont, "border")
        HOperatorSet.TestXldPoint(DelCont, IS1.Targets.All2D_ST.Row, IS1.Targets.All2D_ST.Col, DelPointIndex)
        DelPointIndex = BTuple.TupleAdd(BTuple.TupleFind(DelPointIndex, 1), 1)
        Dim j As Integer
        Dim k As Integer
        Dim t As Integer
        Dim lstDeleteSTindex As Object = Nothing
        If BTuple.TupleLength(DelPointIndex) > 0 Then

            If BTuple.TupleSelect(DelPointIndex, 0) <> -1 Then
                For j = 0 To BTuple.TupleLength(DelPointIndex) - 1
                    For Each ST As SingleTarget In IS1.Targets.lstST
                        If ST.P2ID = CInt(BTuple.TupleSelect(DelPointIndex, j)) Then
                            For t = lstCommon3dST.Count - 1 To 0 Step -1
                                If lstCommon3dST.Item(t).PID = ST.P3ID Then
                                    ' lstDeleteSTindex.Add(t)
                                    lstDeleteSTindex = BTuple.TupleConcat(lstDeleteSTindex, t)
                                    Exit For
                                End If
                            Next
                            Exit For
                        End If
                    Next
                Next
                'lstDeleteSTindex を小さい順に並べ替える
                '      lstDeleteSTindex.Sort()
                lstDeleteSTindex = BTuple.TupleSort(lstDeleteSTindex)
                For t = BTuple.TupleLength(lstDeleteSTindex) - 1 To 0 Step -1
                    Dim DelSTind As Integer
                    DelSTind = CInt(BTuple.TupleSelect(lstDeleteSTindex, t))
                    Dim C3DST As Common3DSingleTarget = lstCommon3dST.Item(DelSTind)
                    For Each ST As SingleTarget In C3DST.lstST
                        k = 0
                        For Each ST1 As SingleTarget In lstImages.Item(ST.ImageID - 1).Targets.lstST
                            If C3DST.PID = ST1.P3ID Then
                                lstImages.Item(ST.ImageID - 1).Targets.lstST.RemoveAt(k)
                                ' lstImages.Item(ST.ImageID - 1).Targets.lstST.Item(k).P3ID = -1
                                Exit For
                            End If
                            k += 1
                        Next
                    Next
                    lstCommon3dST.RemoveAt(DelSTind)
                Next

            End If
        End If
        HOperatorSet.ClearObj(DelCont)

    End Sub

#Region "コードターゲットによる画像解析関連"

    Public Sub TestSyori()
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim n As Integer = lstImages.Count
        Dim hv_ALLCTID As New Object
        hv_ALLCTID = BTuple.ReadTuple("RectangleCT_ID.tup")
        For i = 0 To n - 1
            Dim IS1 As ImageSet = lstImages.Item(i)
            IS1.Targets.DetectTargets(IS1.ImageId, IS1.hx_Image, hv_ALLCTID, T_treshold)

            IS1.FeaturePoints.Row = IS1.Targets.All2D.Row
            IS1.FeaturePoints.Col = IS1.Targets.All2D.Col
            HOperatorSet.GenCrossContourXld(IS1.hx_FeatureCross, IS1.FeaturePoints.Row, IS1.FeaturePoints.Col, CrossSize, CrossAngle)
        Next

        For i = 1 To n - 1
            Dim IS1 As ImageSet = lstImages.Item(i - 1)
            Dim IS2 As ImageSet = lstImages.Item(i)
            Dim ind1 As New Object
            Dim ind2 As New Object

            IS1.RansacFirstIndexBack = Nothing
            IS2.RansacSecondIndexBack = Nothing

            For Each objCT1 As CodedTarget In IS1.Targets.lstCT
                For Each objCT2 As CodedTarget In IS2.Targets.lstCT
                    If objCT1.CT_ID = objCT2.CT_ID Then
                        For j = 0 To 3
                            IS1.FeaturePoints.GetPointsByCoord(BTuple.TupleSelect(objCT1.CT_Points.Row, j), _
                                                               BTuple.TupleSelect(objCT1.CT_Points.Col, j), ind1)
                            IS2.FeaturePoints.GetPointsByCoord(BTuple.TupleSelect(objCT2.CT_Points.Row, j), _
                                                               BTuple.TupleSelect(objCT2.CT_Points.Col, j), ind2)
                            IS1.RansacFirstIndexBack = BTuple.TupleConcat(IS1.RansacFirstIndexBack, ind1)
                            IS2.RansacSecondIndexBack = BTuple.TupleConcat(IS2.RansacSecondIndexBack, ind2)
                        Next
                    End If
                Next
            Next
        Next

        ReConnect()

    End Sub

    Public Sub DetectTargetsAllImages() '全画像からターゲットを抽出する
        TimeMonStart()

        Dim i As Integer = 0
        Dim n As Integer = lstImages.Count '画像枚数
        Dim hv_ALLCTID As New Object
        hv_ALLCTID = BTuple.ReadTuple(My.Application.Info.DirectoryPath & "\RectangleCT_ID.tup")
        '\RectangleCT_ID.tup：
        Dim IS1 As ImageSet
        flgTargetMeasure = False
        'Dim TargetThreshold As Integer = CInt(My.Computer.FileSystem.ReadAllText(My.Application.Info.DirectoryPath & "\TargetThreshold.txt"))
        'Dim CTargetType As Integer = CInt(My.Computer.FileSystem.ReadAllText(My.Application.Info.DirectoryPath & "\CTargetType.txt"))
        Dim TargetThreshold As Integer = GetPrivateProfileInt("Kaiseki", "TargetThreshold", -1, My.Application.Info.DirectoryPath & "\vform.ini")
        Dim CTargetType As Integer = GetPrivateProfileInt("Kaiseki", "CTargetType", 0, My.Application.Info.DirectoryPath & "\vform.ini")

        'ADD By Suurui Sta 20141225------------------------------------------------------------------- 
        Dim blnSangyoCam As Integer = GetPrivateProfileInt("Kaiseki", "KaisekiSetting", 1, My.Application.Info.DirectoryPath & "\vform.ini")
        'ADD By Suurui Sta 20141225------------------------------------------------------------------- 

        'ADD By Kiryu Sta 20170220
        Dim PatternDispModeFlag As Boolean = GetPrivateProfileInt("Kaiseki", "PatternDispMode", 1, My.Application.Info.DirectoryPath & "\vform.ini")
        Dim TGDParam As New TargetDetectParam
        'ADD By Kiruu end


        '画像取得デバイスを指定したパラメーターで初期化する。
        '  HOperatorSet.OpenFramegrabber("File", -1, -1, -1, -1, -1, -1, "default", -1, "default", -1, "default", _projectpath, "default", -1, -1, IGFlag)
        Dim blnDebug As Boolean = False
#If blnDebug Then
        MsgBox("target1 " & n)
#End If
        Dim strErrorMessagetxtfile As String = ProjectPath & "\ErrorMessage.txt"
        If IO.File.Exists(strErrorMessagetxtfile) = True Then
            Try
                IO.File.Delete(strErrorMessagetxtfile)
            Catch ex As Exception

            End Try
        End If

        For i = 0 To n - 1
            IS1 = lstImages.Item(i)
            If IS1.ImageId = 4 Then
                'Stop
                blnSangyoCam = blnSangyoCam
            End If
            '150115 SUURI ADD Sta 複数カメラ内部パラメータ取扱機能--------------
            If blnSangyoCam = 1 Then
#If blnDebug Then
                MsgBox("target2 " & n)
#End If

                Dim intCID As Integer = IS1.GetCamParamID(_projectpath & "\")
#If blnDebug Then
                MsgBox("target3" & n)
#End If

                For Each objCP As CameraParam In lstCamParam
                    If objCP.Cid = intCID Then
                        IS1.objCamparam = objCP
                        Exit For
                    End If
                Next
#If blnDebug Then
                MsgBox("target4 " & n)
#End If


                IS1.Targets.ReadDataExtra(_projectpath & "\", IS1.ImageId)
#If blnDebug Then
                MsgBox("target5 " & IS1.ImageId)
#End If

            Else
                IS1.objCamparam = lstCamParam.Item(0)  'SUUR ADD 20150118
                Dim tempImageObject As HObject = IS1.hx_ImageT
                'IS1.Targets.DetectTargetsOther(IS1.ImageId, tempImageObject, hv_ALLCTID, TargetThreshold, CTargetType) 'ST，CTの抽出
                IS1.Targets.DetectTargetsOther_ver2(IS1.ImageId, tempImageObject, hv_ALLCTID, TargetThreshold, CTargetType, TGDParam) 'ST，CTの抽出
                ClearHObject(tempImageObject)
            End If
            '150115 ADD By Suurui End 20141225------------------------------------------------------------------- 

            ' IS1.CalcRay(hv_CamparamOut)
            IS1.FeaturePoints.Row = IS1.Targets.All2D.Row
            IS1.FeaturePoints.Col = IS1.Targets.All2D.Col
            ClearHObject(IS1.hx_FeatureCross)
            Try
                HOperatorSet.GenCrossContourXld(IS1.hx_FeatureCross, IS1.FeaturePoints.Row, IS1.FeaturePoints.Col, CrossSize, CrossAngle) '各点に対して、十字の XLD オブジェクトを生成する。
            Catch ex As Exception
                ex.ToString()
            End Try

            IS1.flgConnected = False
            If IS1.lstImagePose Is Nothing Then
                IS1.lstImagePose = New List(Of Object)
            Else
                IS1.lstImagePose.Clear()
            End If
            Dim E As New MessageEventArgs
            E.ImageIndex = i
            E.ImageCount = n
            E.MessageText = "ターゲット抽出中"
            RaiseEvent DetectCT(Me, E)
#If blnDebug Then
            MsgBox("target6 " & E.MessageText)
#End If
            'Add Kiryu 据置モードの時1画像に重複CTを検出した場合のエラー処理
            Try
                If IS1.Targets.strErrorMessageTxt.Count > 0 Then
                    For Each strErrCT As String In IS1.Targets.strErrorMessageTxt
                        Dim strErrorNaiyo As String = ""
                        strErrorNaiyo = Now.ToShortDateString & " " & Now.ToShortTimeString
                        strErrorNaiyo = strErrorNaiyo & " " & IS1.ImageName & "でCT" & strErrCT & " が2個検出されました。" & vbNewLine
                        strErrorNaiyo = strErrorNaiyo & "CT" & strErrCT & "を複数設置しているか、CT番号を誤認識している可能性があります。" & vbNewLine
                        My.Computer.FileSystem.WriteAllText(strErrorMessagetxtfile, strErrorNaiyo, True)
                    Next

                End If
            Catch ex As Exception
                ex.ToString()
            End Try
        Next
        If IO.File.Exists(strErrorMessagetxtfile) = True Then
            Dim strErrorNaiyoLast As String = "CTが複数設置されていれば重複しているCTを取り外し再撮影してください。" & vbNewLine & "CT番号を誤認識している場合はこの画像を取り除き解析を再実行してください。"
            My.Computer.FileSystem.WriteAllText(strErrorMessagetxtfile, strErrorNaiyoLast, True)
        End If

        'SUSANO ADD END 20160627
        '  HOperatorSet.CloseFramegrabber(IGFlag)
        flgTargetMeasure = True

        TimeMonEnd()
    End Sub

    Public Sub IkkatuSyori()
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim k As Integer = 0
        Dim n As Integer = lstImages.Count
        Dim IS1 As ImageSet
        Dim IS2 As ImageSet

        GC.Collect()
        GC.WaitForPendingFinalizers()
        Dim Zeros As Object
        Zeros = BTuple.TupleGenConst(5, 0)
        HOperatorSet.ChangeRadialDistortionCamPar("fixed", hv_CamparamFirst, Zeros, hv_CamparamOut)

        '画像からCTとSTの抽出
        DetectTargetsAllImages() '計算時間ー＞

        '全てのカメラペアを計算し、配列に格納。
        Dim tt(n - 1, n - 1) As ImagePairSet
        Dim t As Integer = 0
        Dim ts As Integer
        Dim strt As String = ""
        For i = 0 To n - 2
            For ts = 0 To i - 1
                strt = strt & ","
            Next
            For j = i + 1 To n - 1
                Dim IPS As ImagePairSet

                IS1 = lstImages.Item(i)
                IS2 = lstImages.Item(j)
                IPS = New ImagePairSet(IS1, IS2)
                ' IPS.Camparam = hv_CamparamOut 'SUURI DEL 
                tt(i, j) = IPS
                tt(j, i) = IPS
                t += 1
                IPS.Pair_ID = t
                If IPS.cntComCT >= intCommonCTnum Then
                    IPS.calcRelPose()
                End If

                strt = strt & IPS.cntComCT & ","
            Next
            strt = strt & vbNewLine
        Next
        My.Computer.FileSystem.WriteAllText(ProjectPath & "\Test.csv", strt, True)
        '全ての計測点を格納する。
        If lstCommon3dST Is Nothing Then
            lstCommon3dST = New List(Of Common3DSingleTarget)
        Else
            lstCommon3dST.Clear()
        End If
        '全てのコードターゲットを格納する。
        If lstCommon3dCT Is Nothing Then
            lstCommon3dCT = New List(Of Common3DCodedTarget)
        Else
            lstCommon3dCT.Clear()
        End If
        ' Dim lstConPairs As New List(Of ConnectingPair)

        'コードターゲットをまとめる
        CollectCT()
        Dim sw As New System.Diagnostics.Stopwatch()
        'カメラ結合（CTによる結合）
        sw.Start()
        'ConnectAllCamera(tt)
        'CalcALLCodedTarget3dCoordByHalcon()
        'ConnectAllCameraByVector(tt)
        ConnectAllCameraByBestScale(tt)
        ' ConnectAllCamera(tt, lstConPairs)
        sw.Stop()
        Trace.WriteLine("カメラ結合時間")
        Trace.WriteLine(sw.Elapsed.TotalMilliseconds & "ミリ秒")

        'シングルターゲットの番号付け
        sw.Reset()
        sw.Start()
        SingleTargetNumbering(tt)
        '    SingleTargetNumbering(tt, lstConPairs)
        sw.Stop()
        Trace.WriteLine("シングルターゲットの番号付け時間")
        Trace.WriteLine(sw.Elapsed.TotalMilliseconds & "ミリ秒")


        CalcALLSingleTarget3dCoordByHalcon()
        CalcALLCodedTarget3dCoordByHalcon()

        ''全てのレイを計算する。
        'CalcAllImages_Ray()
        ''全ての計測点の三次元座標を算出する。
        'CalcALLSingleTarget3dCoord()

        'For i = 0 To n - 2
        '    For j = i + 1 To n - 1
        '        If tt(i, j).cntComCT >= intCommonCTnum Then
        '            tt(i, j).GetCommonST()
        '            tt(i, j).calcRelPose()
        '            tt(i, j).IS1.flgConnected = False
        '            tt(i, j).IS2.flgConnected = False
        '        End If
        '    Next
        'Next
        ''再結合
        'ConnectAllCamera(tt)
        ''全てのレイを再計算する。
        'CalcAllImages_Ray()
        ''全ての計測点の三次元座標を再算出する。
        'CalcALLSingleTarget3dCoord()
        CalcReProjectionError()

        'sw.Start()
        ' CalcALLSingleTarget3dCoordByHalcon()
        'sw.Stop()
        'Trace.WriteLine(sw.Elapsed.TotalMilliseconds & "ミリ秒->halcon")
        'CalcReProjectionError()

        RunBA_New(FBMlib.T_treshold, False, hv_CamparamOut)


        CalcReProjectionError()
        ' hv_CamparamOut = tmpCamParam
        CalcALLSingleTarget3dCoordByHalcon()
        CalcALLCodedTarget3dCoordByHalcon()
        ''全てのレイを計算する。
        'CalcAllImages_Ray()
        ''全ての計測点の三次元座標を算出する。
        'CalcALLSingleTarget3dCoord()

        ' RunBA()
        CalcReProjectionError()
    End Sub
    Private Sub RunProgressBarEvent(ByVal Index As Integer, ByVal Count As Integer, ByVal strMessage As String)
        Dim E As New MessageEventArgs
        E.ImageIndex = Index
        E.ImageCount = Count
        E.MessageText = strMessage
        RaiseEvent Progress(Me, E)
    End Sub
    Private Sub ReadKijyunPoints(ByRef lstBasePoints As List(Of Common3DCodedTarget))
        Dim filename As String = My.Application.Info.DirectoryPath & "\基準座標.csv"
        Dim fields As String()
        Dim delimiter As String = ","
        If lstBasePoints Is Nothing Then
            lstBasePoints = New List(Of Common3DCodedTarget)
        Else
            lstBasePoints.Clear()
        End If
        Using parser As New TextFieldParser(filename)
            parser.SetDelimiters(delimiter)
            While Not parser.EndOfData
                fields = parser.ReadFields()
                Dim strLabel As String
                Dim intCTID As Integer
                Dim intKijyunFlag As Integer
                If IsNumeric(fields(5)) Then
                    intKijyunFlag = CInt(fields(5))
                    If intKijyunFlag = 1 Then
                        strLabel = fields(1)
                        Dim objnewPoint As New Point3D
                        objnewPoint.X = CDbl(fields(2))
                        objnewPoint.Y = CDbl(fields(3))
                        objnewPoint.Z = CDbl(fields(4))
                        If IsNumeric(strLabel.Replace("CT", "")) Then
                            intCTID = CInt(strLabel.Replace("CT", ""))
                            Dim objNewC3DP As New Common3DCodedTarget
                            objNewC3DP.PID = intCTID
                            objNewC3DP.flgUsable = True
                            objNewC3DP.lstP3d.Add(objnewPoint)
                            lstBasePoints.Add(objNewC3DP)
                        Else
                            lstBasePoints.Last.lstP3d.Add(objnewPoint)
                        End If
                    End If
                End If

            End While
        End Using

    End Sub
    Private Sub ReadAllHikakuPoints(ByRef lstAllhikakuPoints As List(Of Common3DCodedTarget))
        Dim filename As String = My.Application.Info.DirectoryPath & "\基準座標.csv"
        Dim fields As String()
        Dim delimiter As String = ","

        If lstAllhikakuPoints Is Nothing Then
            lstAllhikakuPoints = New List(Of Common3DCodedTarget)
        Else
            lstAllhikakuPoints.Clear()
        End If
        Using parser As New TextFieldParser(filename)
            parser.SetDelimiters(delimiter)
            While Not parser.EndOfData
                fields = parser.ReadFields()
                Dim strLabel As String
                Dim intCTID As Integer
                Dim intKijyunFlag As Integer
                If IsNumeric(fields(5)) Then
                    intKijyunFlag = CInt(fields(5))
                    strLabel = fields(1)
                    Dim objnewPoint As New Point3D
                    objnewPoint.X = CDbl(fields(2))
                    objnewPoint.Y = CDbl(fields(3))
                    objnewPoint.Z = CDbl(fields(4))
                    If IsNumeric(strLabel.Replace("CT", "")) Then
                        intCTID = CInt(strLabel.Replace("CT", ""))
                        Dim objNewC3DP As New Common3DCodedTarget
                        objNewC3DP.PID = intCTID
                        objNewC3DP.flgUsable = True
                        objNewC3DP.lstP3d.Add(objnewPoint)
                        lstAllhikakuPoints.Add(objNewC3DP)
                    Else
                        lstAllhikakuPoints.Last.lstP3d.Add(objnewPoint)
                    End If
                End If
            End While
        End Using

    End Sub

    'SUURI ADD 20150129
    '画像毎に基準点座標により、外部評定を求める。
    Public Function IkkatuSyoriOneImagePoseByBasePoint(ByVal flgCTonly As Boolean) As Boolean '一括処理実行
        IkkatuSyoriOneImagePoseByBasePoint = False
        Debug.Print("suuri debug")
        If lstImages Is Nothing Then
            Exit Function
        End If
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim k As Integer = 0
        Dim n As Integer = lstImages.Count
        Dim IS1 As ImageSet
        Dim IS2 As ImageSet
        Dim sw As New System.Diagnostics.Stopwatch()
        'メモリオーバーを防ぐ
        GC.Collect()
        GC.WaitForPendingFinalizers()
        Dim Zeros As Object
        Zeros = BTuple.TupleGenConst(5, 0)
        ' HOperatorSet.ReadCamPar("C:\My Work\MandS\HALCON9関連\NonTarget\camparamD3X_White_SxConst.cal", hv_Camparam)
        ' HOperatorSet.ChangeRadialDistortionCamPar("fixed", hv_Camparam, Zeros, hv_CamparamOut)

        'カメラの内部パラメータ(初期）を設定
        'CamParamの値
        hv_CamparamOut(0) = hv_CamparamFirst(0) 'Focus：
        hv_CamparamOut(1) = hv_CamparamFirst(1) 'Poly1：
        hv_CamparamOut(2) = hv_CamparamFirst(2) 'Poly2：
        hv_CamparamOut(3) = hv_CamparamFirst(3) 'Poly3：
        hv_CamparamOut(4) = hv_CamparamFirst(4) 'Poly4：
        hv_CamparamOut(5) = hv_CamparamFirst(5) 'Poly5：
        hv_CamparamOut(6) = hv_CamparamFirst(6) 'Sx：
        hv_CamparamOut(7) = hv_CamparamFirst(7) 'Sy：
        hv_CamparamOut(8) = hv_CamparamFirst(8) 'Cx：
        hv_CamparamOut(9) = hv_CamparamFirst(9) 'Cy：
        hv_CamparamOut(10) = hv_CamparamFirst(10) 'ImageWidth：
        hv_CamparamOut(11) = hv_CamparamFirst(11) 'ImageHeight：
        'SUURI ADD
        Dim blnSangyoCam As Integer = GetPrivateProfileInt("Kaiseki", "KaisekiSetting", 0, My.Application.Info.DirectoryPath & "\vform.ini")
        If blnSangyoCam = 1 Then
            ReadCamParamList(_projectpath & "\")
        Else
            ReadCamParamList()  'SUUR ADD 20150118
        End If
        If lstBasePoint Is Nothing Then
            lstBasePoint = New List(Of Common3DCodedTarget)
        Else
            lstBasePoint.Clear()
        End If
        If lstAllHikakuPoint Is Nothing Then
            lstAllHikakuPoint = New List(Of Common3DCodedTarget)
        Else
            lstAllHikakuPoint.Clear()
        End If

        'ReadKijyunPoints(lstBasePoint)
        ReadKijyunPointsFromDB(lstBasePoint)
        'ReadAllHikakuPoints(lstAllHikakuPoint)
        ReadAllHikakuPointsFromDB(lstAllHikakuPoint)


        '2つのImageに共通して写っているCT（共通CT）の最小数を設定（CommonCTnum.txtに記載。H25.4.10現在は"4"）
        intCommonCTnum = GetPrivateProfileInt("Kaiseki", "CommonCTnum", 3, My.Application.Info.DirectoryPath & "\vform.ini")

        sw.Reset()
        sw.Start()
        'RunProgressBarEvent(1, 8, "ターゲット抽出中")
        '画像からCTとSTの抽出
        If flgTargetMeasure = True Then
            If MsgBox("ターゲット抽出をしますか？", MsgBoxStyle.YesNo, "確認") = MsgBoxResult.Yes Then
                DetectTargetsAllImages()
            Else
                For Each ISI As ImageSet In lstImages
                    For Each ST As SingleTarget In ISI.Targets.lstST
                        ST.P3ID = -1
                        If ST.tmpPPP Is Nothing Then
                            ST.tmpPPP = New List(Of SingleTarget)
                        Else
                            ST.tmpPPP.Clear()
                        End If
                        ST.flgUsed = 0
                    Next
                Next
            End If
        Else
            '全Imageに対してTargetのスキャンを開始
            DetectTargetsAllImages()
        End If
        sw.Stop()
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "JikanMonitor.mon",
                                            "ターゲット抽出時間： " & sw.Elapsed.TotalSeconds & "秒" & vbNewLine, True)
        ImageCNT = n '画像枚数
        RunProgressBarEvent(2, 8, "解析処理中")
        sw.Reset()
        sw.Start()
        '全てのカメラペアを計算し、配列に格納。
        Dim tt(n - 1, n - 1) As ImagePairSet
        Dim t As Integer = 0
        '   Dim strt As String = ""
        FirstI = 0
        FirstJ = 0
        Dim FirstHyoka As Double = -1
        Dim MinPairPoseError As Double = Double.MaxValue '質問①
        Dim MinPairPoseMeanZ As Double = Double.MaxValue '質問②
#If DEBUG Then
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\RelPoseMonitor.txt", "Monitor開始", False)
#End If
        ''tt(n-1,n-1)配列の作成開始
        For i = 0 To n - 1
            For j = 0 To n - 1
                If i <> j Then '2枚のImageが同じかどうか
                    Dim IPS As ImagePairSet

                    IS1 = lstImages.Item(i)
                    IS2 = lstImages.Item(j)
                    IPS = New ImagePairSet(IS1, IS2) 'ImagePairSet作成
                    'IPS.Camparam = hv_CamparamOut 'SUURI DEL
                    tt(i, j) = IPS
                    t += 1
                    IPS.Pair_ID = t
                    If IPS.cntComCT >= intCommonCTnum Then '共通のCTが最低枚数を越えているか
                        IPS.calcRelPose() '相対Poseを算出
                        'MonitorPairInfo
                        'IPS.MonitorPairInfo(ProjectPath)

                        If IPS.cntComCT >= intCommonCTnum Then
                            If BTuple.TupleLength(IPS.PairPose.hError).I = 1 Then
                                'If IPS.PairPose.hError < MinPairPoseError And BTuple.TupleMean(IPS.PairPose.Z) < MinPairPoseMeanZ Then
                                '    FirstI = i
                                '    FirstJ = j
                                '    MinPairPoseError = IPS.PairPose.hError
                                '    MinPairPoseMeanZ = BTuple.TupleMean(IPS.PairPose.Z)
                                'End If
                                If BTuple.TupleAbs(BTuple.TupleMean(IPS.PairPose.Z) / IPS.cntComCT) < MinPairPoseError Then
                                    If i < j Then
                                        FirstI = i
                                        FirstJ = j
                                        MinPairPoseError = BTuple.TupleAbs(BTuple.TupleMean(IPS.PairPose.Z) / IPS.cntComCT).D
                                    End If

                                End If
                            End If
                        End If
                    End If
                    ' strt = strt & IPS.cntComCT & "," '共通CTの数
                Else
                    ' strt = strt & "×" & ","
                End If
            Next
            'strt = strt & vbNewLine
            RunProgressBarEvent(i + 1, n, "解析処理中(1)")
        Next

        sw.Stop()
        ' My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "JikanMonitor.mon",
        '                                   "２画像組の計算時間： " & sw.Elapsed.TotalSeconds & "秒" & vbNewLine, True)
        ''tt(n-1,n-1)配列の作成完了


#If DEBUG Then
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\RelPoseMonitor.txt", "Monitor終了", True)
#End If
        RunProgressBarEvent(3, 8, "解析処理中")
        '  My.Computer.FileSystem.WriteAllText(ProjectPath & "\Test.csv", strt, True)
        '全ての計測点を格納する。
        If lstCommon3dST Is Nothing Then
            lstCommon3dST = New List(Of Common3DSingleTarget)
        Else
            lstCommon3dST.Clear()
        End If
        '全てのコードターゲットを格納する。
        If lstCommon3dCT Is Nothing Then
            lstCommon3dCT = New List(Of Common3DCodedTarget)
        Else
            lstCommon3dCT.Clear()
        End If

        sw.Reset()
        sw.Start()
        'コードターゲットをまとめる
        CollectCT()
        sw.Stop()
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "JikanMonitor.mon",
                                            "コードターゲットをまとめる時間： " & sw.Elapsed.TotalSeconds & "秒" & vbNewLine, True)
        ''tt(n-1,n-1)配列の作成完了

        RunProgressBarEvent(4, 8, "解析処理中")

        'カメラ結合（CTによる結合）
        sw.Reset()
        sw.Start()

        '全ImageのPose（姿勢）を計算

        If CalcAllCameraPoseByBasePoint() < 2 Then
            '結合できた画像が２枚以上ではないため、終了
            MsgBox("画像結合が不充分です。", MsgBoxStyle.OkOnly, "確認")
            Exit Function
        End If
        'ConnectAllCameraByBestScale3(tt)
        'ConnectAllCameraByVector(tt)

        sw.Stop()
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "JikanMonitor.mon",
                                            "カメラ結合時間： " & sw.Elapsed.TotalSeconds & "秒" & vbNewLine, True)
        Trace.WriteLine("カメラ結合時間")
        Trace.WriteLine(sw.Elapsed.TotalMilliseconds & "ミリ秒")
#If DEBUG Then
        '3次元計測点を2次元計測点に投影するMatrix（逆行列）/1回目
        'CalcAllImagesInvertHomMat()

        '再投影誤差を求める /1回目
        ' CalcReProjectionError()
#End If
        RunProgressBarEvent(5, 8, "解析処理中(バンドル調整）")
        sw.Reset()
        sw.Start()

        MonitorImagePose(ProjectPath & "\MonitorImagePoseBaMae.txt")
        BTuple.WriteTuple(hv_CamparamOut, ProjectPath & "\MonitorCamPar_BaMae.tpl")
        'バンドル調整（B A）：再投影誤差を最小化する /1回目
        'RunBA_New(FBMlib.T_treshold, False, hv_CamparamOut)
        MonitorImagePose(ProjectPath & "\MonitorImagePoseBaAto.txt")
        BTuple.WriteTuple(hv_CamparamOut, ProjectPath & "\MonitorCamPar_BaAto.tpl")
        sw.Stop()
        Trace.WriteLine("バンドル調整の時間")
        Trace.WriteLine(sw.Elapsed.TotalMilliseconds & "ミリ秒")
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "JikanMonitor.mon",
                                          "バンドル調整の時間： " & sw.Elapsed.TotalSeconds & "秒" & vbNewLine, True)
        'カメラの内部パラメーターの修正
        HOperatorSet.ChangeRadialDistortionCamPar("fixed", hv_CamparamOut, Zeros, hv_CamparamZero)
        RunProgressBarEvent(6, 8, "解析処理中")

        sw.Reset()
        sw.Start()
        '3次元計測点を2次元計測点に投影するMatrix（逆行列）/2回目
        CalcAllImagesInvertHomMat()
        '全てのレイを計算する。 /1回目
        CalcAllImages_Ray()
        'RMS（2乗平方根）を計算する。 /1回目
        '  CalcRMS()
        sw.Stop()
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "JikanMonitor.mon",
                                            "その他計算時間： " & sw.Elapsed.TotalSeconds & "秒" & vbNewLine, True)
#If False Then
        IkkatuSyori2 = True
        Exit Function
#End If
        '  If flgCTonly = False Then
        If False Then
#If DEBUG Then
            '再投影誤差を求める /2回目
            CalcReProjectionError()
#End If
            '仮のスケールを算出：（CT内のST間の計測距離の合計）/（CT内のST間の設計距離の合計）
            CalcKarinoScaleByCTPoints(ScaleMM)

            'シングルターゲットの番号付け
            sw.Reset()
            sw.Start()
            '  CalcAllImagesInvertHomMat()



            MinCountOfRays = 3
            ' SingleTargetNumbering(tt)
            ' SingleTargetNumbering2(tt)
            'SingleTargetNumbering3(tt)

            'SingleTargetNumbering4(tt)


            'STの候補を見つける。
            SingelTargetNumberingOther(tt)
            'STの候補からBadなSTを除く
            CheckAndRemoveBadSingleTarget()
            sw.Stop()
            Trace.WriteLine("シングルターゲットの番号付け時間")
            Trace.WriteLine(sw.Elapsed.TotalMilliseconds & "ミリ秒")
            RunProgressBarEvent(7, 8, "解析処理中")
            sw.Reset()
            sw.Start()

            'STを含めバンドル調整（B A）：再投影誤差を最小化する /2回目
            RunBA_New(FBMlib.T_treshold, True, hv_CamparamOut)
            sw.Stop()
            Trace.WriteLine("バンドル調整の時間")
            Trace.WriteLine(sw.Elapsed.TotalMilliseconds & "ミリ秒")

            ''3次元計測点を2次元計測点に投影するMatrix（逆行列）/3回目
            CalcAllImagesInvertHomMat()
        End If
        RunProgressBarEvent(8, 8, "解析処理中")
#If DEBUG Then
        ''再投影誤差を求める /3回目
        CalcReProjectionError()

        '全てのレイを計算する。 /2回目
        CalcAllImages_Ray()

        'RMS（2乗平方根）を計算する。 /2回目
        CalcRMS()

#End If
        IkkatuSyoriOneImagePoseByBasePoint = True
    End Function

    '全ImageのPoseを計算
    Private Function ConnectAllCameraByBasePoint(ByRef tt(,) As ImagePairSet) As Integer
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim n As Integer = lstImages.Count
        Dim t As Integer = 0
        Dim dblE_mae As Double = Double.MaxValue
        Dim currentIPS As ImagePairSet

        Dim PoseByBestScale As Object = Nothing
        Dim Quality As Object = Nothing
        Dim ComScale As Object = Nothing

        Dim flgFirst As Boolean = True
        '   Dim lstTest As New List(Of String)

        For Each ISI As ImageSet In lstImages
            ISI.flgConnected = False
            ISI.flgUsable = True
            ISI.Quality = Double.MaxValue
        Next
        'For i = 0 To n - 1
        '    For j = 4 To n - 1
        '        If i <> j Then
        currentIPS = tt(FirstI, FirstJ)
        If currentIPS Is Nothing Then
            ConnectAllCameraByBasePoint = 0
            Exit Function
        End If
        If currentIPS.cntComCT >= intCommonCTnum And BTuple.TupleLength(currentIPS.PairPose.hError) = 1 Then
            currentIPS.IS1.flgFirst = True
            currentIPS.IS2.flgFirst = True
            currentIPS.IS2.flgSecond = True
            currentIPS.IS1.flgConnected = True
            currentIPS.IS2.flgConnected = True
            Dim FirstPose As New Object
            HOperatorSet.CreatePose(0, 0, 0, 0, 0, 0, "Rp+T", "gba", "point", FirstPose)
            currentIPS.IS1.ImagePose.Pose = FirstPose
            If 1 Then
                Dim tmpPoseX As Double = Math.Abs(CDbl(currentIPS.PairPose.RelPose(0)))
                Dim tmpPoseY As Double = Math.Abs(CDbl(currentIPS.PairPose.RelPose(1)))
                If tmpPoseX > tmpPoseY Then
                    currentIPS.PairPose.RelPose(0) = currentIPS.PairPose.RelPose(0) / tmpPoseX
                    currentIPS.PairPose.RelPose(1) = currentIPS.PairPose.RelPose(1) / tmpPoseX
                    currentIPS.PairPose.RelPose(2) = currentIPS.PairPose.RelPose(2) / tmpPoseX
                    XorY = 1
                Else
                    currentIPS.PairPose.RelPose(0) = currentIPS.PairPose.RelPose(0) / tmpPoseY
                    currentIPS.PairPose.RelPose(1) = currentIPS.PairPose.RelPose(1) / tmpPoseY
                    currentIPS.PairPose.RelPose(2) = currentIPS.PairPose.RelPose(2) / tmpPoseY
                    XorY = 2
                End If
            End If
            currentIPS.IS2.ImagePose.Pose = currentIPS.PairPose.RelPose
            currentIPS.IS2.VectorPose.Pose = currentIPS.PairPose.RelPose
            currentIPS.ComScale = 1.0
            ' currentIPS.CreateCommon3DCT(lstCommon3dCT)
            'CalcAllImages_Ray()
            'CalcALLSingleTarget3dCoord()
            'For Each CT As CodedTarget In currentIPS.lstIS1_ComCT
            '    Dim C3DCT As New Common3DCodedTarget
            '    C3DCT.PID = CT.CT_ID
            '    C3DCT.lstCT.Add(CT)
            '    lstCommon3dCT.Add(C3DCT)
            'Next
            'For Each CT As CodedTarget In currentIPS.lstIS2_ComCT
            '    For Each Com3DCT As Common3DCodedTarget In lstCommon3dCT
            '        If Com3DCT.PID = CT.CT_ID Then
            '            Com3DCT.lstCT.Add(CT)
            '        End If
            '    Next
            'Next
            'CalcALLCodedTarget3dCoordByHalcon()

            Dim objHomMat As Object = Nothing


            CalcOneImages_Ray(currentIPS.IS1)
            CalcOneImages_Ray(currentIPS.IS2)
            ' CalcALLCTnoSingleTarget3dCoord()

            CalcThisImagesCT_3dCoord(currentIPS.IS2)
            If currentIPS.CalcPoseByBasePointandBestScale(lstBasePoint, lstCommon3dCT, objHomMat, Quality, ComScale, ScaleMM) = True Then
                Dim posHommat As Object = Nothing
                Dim commonPose As Object = Nothing
                HOperatorSet.HomMat3dToPose(objHomMat, commonPose)
                HOperatorSet.PoseToHomMat3d(currentIPS.IS1.ImagePose.Pose, posHommat)
                HOperatorSet.PoseCompose(currentIPS.IS1.ImagePose.Pose, commonPose, currentIPS.IS1.ImagePose.Pose)
                HOperatorSet.PoseCompose(currentIPS.IS2.ImagePose.Pose, commonPose, currentIPS.IS2.ImagePose.Pose)
                For Each objC3DCT As Common3DCodedTarget In lstCommon3dCT
                    For Each objP3d As Point3D In objC3DCT.lstP3d
                        HOperatorSet.AffineTransPoint3d(objHomMat, objP3d.X, objP3d.Y, objP3d.Z, objP3d.X, objP3d.Y, objP3d.Z)
                    Next
                Next
                '確認用
                KakuninnoTame("BasePointHikaku.csv", "KekkaByBasePointToPair.csv")

                'Dim cntjj As Integer = 0
                'Dim strFileText As String = ""
                'For Each objC3DCT As Common3DCodedTarget In lstCommon3dCT
                '    If objC3DCT.flgUsable = True Then
                '        For Each objHikaku As Common3DCodedTarget In lstAllHikakuPoint
                '            If objC3DCT.PID = objHikaku.PID Then
                '                If objC3DCT.lstP3d.Count = CodedTarget.CTnoSTnum And objHikaku.lstP3d.Count = CodedTarget.CTnoSTnum Then
                '                    cntjj = 0
                '                    For Each objP3d As Point3D In objC3DCT.lstP3d
                '                        strFileText = strFileText & objC3DCT.PID & "," & cntjj & "," & objP3d.X & "," & objP3d.Y & "," & objP3d.Z & ","
                '                        strFileText = strFileText & objHikaku.lstP3d(cntjj).X & "," & objHikaku.lstP3d(cntjj).Y & "," & objHikaku.lstP3d(cntjj).Z
                '                        strFileText = strFileText & vbNewLine
                '                        cntjj += 1
                '                    Next
                '                    Exit For
                '                End If
                '            End If
                '        Next
                '    End If
                'Next
                'My.Computer.FileSystem.WriteAllText(_projectpath & "\BasePointHikaku.csv", strFileText, False)
                'strFileText = ""
                'For Each objC3DCT As Common3DCodedTarget In lstCommon3dCT
                '    If objC3DCT.flgUsable = True Then
                '        cntjj = 0
                '        For Each objP3d As Point3D In objC3DCT.lstP3d
                '            strFileText = strFileText & objC3DCT.PID & "," & cntjj & "," & objP3d.X & "," & objP3d.Y & "," & objP3d.Z
                '            strFileText = strFileText & vbNewLine
                '            cntjj += 1
                '        Next
                '    End If
                'Next
                'My.Computer.FileSystem.WriteAllText(_projectpath & "\KekkaByBasePointToPair.csv", strFileText, False)
            End If
            'currentIPS.IS1.flgConnected = False
            'currentIPS.IS2.flgConnected = False
            '    GoTo jump
            'End If
            CalcKarinoScaleByCTPoints(ScaleMM)

        End If
        '    Next j
        'Next i
jump:


        Dim MaxQuality As Object = Nothing
        Dim cntNotConnectedImage As Integer
        Dim tmpcntNotConnectedImage As Integer = -1
        cntCurrentConnectedImage = 0
        dblE_mae = Double.MaxValue
        Do
            For i = 0 To n - 1
                If lstImages(i).flgConnected = True Then
                    For j = 0 To n - 1
                        If i <> j Then
                            currentIPS = tt(i, j)

                            If currentIPS.cntComCT >= intCommonCTnum Then
                                If currentIPS.IS1.flgConnected = True And currentIPS.IS2.flgConnected = False And currentIPS.IS2.flgUsable = True Then

                                    If currentIPS.CalcPoseByBasePointandBestScale(lstBasePoint, lstCommon3dCT, PoseByBestScale, Quality, ComScale, ScaleMM) = True Then

                                        '  If currentIPS.IS2.Quality > Quality Then
                                        currentIPS.IS2.ImagePose.Pose = PoseByBestScale
                                        currentIPS.IS2.Quality = Quality
                                        currentIPS.IS2.CommonScale = ComScale
                                        currentIPS.IS2.flgConnected = True

                                        CalcOneImages_Ray(currentIPS.IS2)
                                        'CalcALLCTnoSingleTarget3dCoord()
                                        CalcThisImagesCT_3dCoord(currentIPS.IS2)

                                        'CalcReProjectionErrorOneCT(currentIPS.IS2)
                                        CalcReProjectionErrorOneCT_faster(currentIPS.IS2)
                                        'CalcALLCTnoSingleTarget3dCoord()
                                        CalcThisImagesCT_3dCoord(currentIPS.IS2)
                                        cntCurrentConnectedImage += 1
                                        RunProgressBarEvent(cntCurrentConnectedImage, n, "解析処理中(3)")
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
            Next

            cntNotConnectedImage = 0
            cntConnectedImage = 0
            For Each ISI As ImageSet In lstImages
                If ISI.flgConnected = False Then
                    cntNotConnectedImage = cntNotConnectedImage + 1
                Else
                    cntConnectedImage += 1
                End If
                ISI.flgUsable = True
            Next
            If tmpcntNotConnectedImage <> cntNotConnectedImage Then
                tmpcntNotConnectedImage = cntNotConnectedImage
            Else
                cntNotConnectedImage = 0
            End If
        Loop Until cntNotConnectedImage = 0


        '  GenCommon3Dpoint()

        ConnectAllCameraByBasePoint = cntConnectedImage

    End Function
    'SUURI ADD 20150124
    'Pair毎に基準点座標にセットする。
    Public Function IkkatuSyoriPair_Kijyun(ByVal flgCTonly As Boolean) As Boolean '一括処理実行
        IkkatuSyoriPair_Kijyun = False
        If lstImages Is Nothing Then
            Exit Function
        End If
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim k As Integer = 0
        Dim n As Integer = lstImages.Count
        Dim IS1 As ImageSet
        Dim IS2 As ImageSet
        Dim sw As New System.Diagnostics.Stopwatch()
        'メモリオーバーを防ぐ
        GC.Collect()
        GC.WaitForPendingFinalizers()
        Dim Zeros As Object
        Zeros = BTuple.TupleGenConst(5, 0)
        ' HOperatorSet.ReadCamPar("C:\My Work\MandS\HALCON9関連\NonTarget\camparamD3X_White_SxConst.cal", hv_Camparam)
        ' HOperatorSet.ChangeRadialDistortionCamPar("fixed", hv_Camparam, Zeros, hv_CamparamOut)

        'カメラの内部パラメータ(初期）を設定
        'CamParamの値
        hv_CamparamOut(0) = hv_CamparamFirst(0) 'Focus：
        hv_CamparamOut(1) = hv_CamparamFirst(1) 'Poly1：
        hv_CamparamOut(2) = hv_CamparamFirst(2) 'Poly2：
        hv_CamparamOut(3) = hv_CamparamFirst(3) 'Poly3：
        hv_CamparamOut(4) = hv_CamparamFirst(4) 'Poly4：
        hv_CamparamOut(5) = hv_CamparamFirst(5) 'Poly5：
        hv_CamparamOut(6) = hv_CamparamFirst(6) 'Sx：
        hv_CamparamOut(7) = hv_CamparamFirst(7) 'Sy：
        hv_CamparamOut(8) = hv_CamparamFirst(8) 'Cx：
        hv_CamparamOut(9) = hv_CamparamFirst(9) 'Cy：
        hv_CamparamOut(10) = hv_CamparamFirst(10) 'ImageWidth：
        hv_CamparamOut(11) = hv_CamparamFirst(11) 'ImageHeight：

        '150115 SUURI ADD Sta 複数カメラ内部パラメータ取扱機能--------------
        Dim blnSangyoCam As Integer = GetPrivateProfileInt("Kaiseki", "KaisekiSetting", 0, My.Application.Info.DirectoryPath & "\vform.ini")
        If blnSangyoCam = 1 Then
            ReadCamParamList(_projectpath & "\")
        Else
            ReadCamParamList()  'SUUR ADD 20150118
        End If
        '150115 SUURI ADD End 複数カメラ内部パラメータ取扱機能--------------

        ReadKijyunPoints(lstBasePoint)
        ReadAllHikakuPoints(lstAllHikakuPoint)
        '2つのImageに共通して写っているCT（共通CT）の最小数を設定（CommonCTnum.txtに記載。H25.4.10現在は"4"）
        intCommonCTnum = GetPrivateProfileInt("Kaiseki", "CommonCTnum", 0, My.Application.Info.DirectoryPath & "\vform.ini")

        sw.Reset()
        sw.Start()
        'RunProgressBarEvent(1, 8, "ターゲット抽出中")
        '画像からCTとSTの抽出
        If flgTargetMeasure = True Then
            If MsgBox("ターゲット抽出をしますか？", MsgBoxStyle.YesNo, "確認") = MsgBoxResult.Yes Then
                DetectTargetsAllImages()
            Else
                For Each ISI As ImageSet In lstImages
                    For Each ST As SingleTarget In ISI.Targets.lstST
                        ST.P3ID = -1
                        If ST.tmpPPP Is Nothing Then
                            ST.tmpPPP = New List(Of SingleTarget)
                        Else
                            ST.tmpPPP.Clear()
                        End If
                        ST.flgUsed = 0
                    Next
                Next
            End If
        Else
            '全Imageに対してTargetのスキャンを開始
            DetectTargetsAllImages()
        End If
        sw.Stop()
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "JikanMonitor.mon",
                                            "ターゲット抽出時間： " & sw.Elapsed.TotalSeconds & "秒" & vbNewLine, True)
        ImageCNT = n '画像枚数
        RunProgressBarEvent(2, 8, "解析処理中")
        sw.Reset()
        sw.Start()
        '全てのカメラペアを計算し、配列に格納。
        Dim tt(n - 1, n - 1) As ImagePairSet
        Dim t As Integer = 0
        Dim strt As String = ""
        FirstI = 0
        FirstJ = 0
        Dim FirstHyoka As Double = -1
        Dim MinPairPoseError As Double = Double.MaxValue '質問①
        Dim MinPairPoseMeanZ As Double = Double.MaxValue '質問②
        Dim strErrMsg As String = "" 'ADD By Yamada 20150116

#If DEBUG Then
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\RelPoseMonitor.txt", "Monitor開始", False)
#End If
        ''tt(n-1,n-1)配列の作成開始
        For i = 0 To n - 1
            For j = 0 To n - 1
                If i <> j Then '2枚のImageが同じかどうか
                    Dim IPS As ImagePairSet

                    IS1 = lstImages.Item(i)
                    IS2 = lstImages.Item(j)
                    IPS = New ImagePairSet(IS1, IS2) 'ImagePairSet作成
                    'IPS.Camparam = hv_CamparamOut 'SUURI DEL
                    tt(i, j) = IPS
                    t += 1
                    IPS.Pair_ID = t
                    If IPS.cntComCT >= intCommonCTnum Then '共通のCTが最低枚数を越えているか
                        IPS.calcRelPose() '相対Poseを算出
                        'MonitorPairInfo
                        'IPS.MonitorPairInfo(ProjectPath)

                        If IPS.cntComCT >= intCommonCTnum Then
                            If BTuple.TupleLength(IPS.PairPose.hError) = 1 Then
                                'If IPS.PairPose.hError < MinPairPoseError And BTuple.TupleMean(IPS.PairPose.Z) < MinPairPoseMeanZ Then
                                '    FirstI = i
                                '    FirstJ = j
                                '    MinPairPoseError = IPS.PairPose.hError
                                '    MinPairPoseMeanZ = BTuple.TupleMean(IPS.PairPose.Z)
                                'End If
                                If BTuple.TupleAbs(BTuple.TupleMean(IPS.PairPose.Z) / IPS.cntComCT) < MinPairPoseError Then
                                    If i < j Then
                                        FirstI = i
                                        FirstJ = j
                                        MinPairPoseError = BTuple.TupleAbs(BTuple.TupleMean(IPS.PairPose.Z) / IPS.cntComCT)
                                    End If

                                End If
                            End If
                        End If
                    End If
                    strt = strt & IPS.cntComCT & "," '共通CTの数
                Else
                    strt = strt & "×" & ","
                End If
            Next
            strt = strt & vbNewLine
            RunProgressBarEvent(i + 1, n, "解析処理中(1)")
        Next

        sw.Stop()
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "JikanMonitor.mon",
                                            "２画像組の計算時間： " & sw.Elapsed.TotalSeconds & "秒" & vbNewLine, True)
        ''tt(n-1,n-1)配列の作成完了


#If DEBUG Then
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\RelPoseMonitor.txt", "Monitor終了", True)
#End If
        RunProgressBarEvent(3, 8, "解析処理中")
        My.Computer.FileSystem.WriteAllText(ProjectPath & "\Test.csv", strt, True)
        '全ての計測点を格納する。
        If lstCommon3dST Is Nothing Then
            lstCommon3dST = New List(Of Common3DSingleTarget)
        Else
            lstCommon3dST.Clear()
        End If
        '全てのコードターゲットを格納する。
        If lstCommon3dCT Is Nothing Then
            lstCommon3dCT = New List(Of Common3DCodedTarget)
        Else
            lstCommon3dCT.Clear()
        End If

        sw.Reset()
        sw.Start()
        'コードターゲットをまとめる
        CollectCT()
        sw.Stop()
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "JikanMonitor.mon",
                                            "コードターゲットをまとめる時間： " & sw.Elapsed.TotalSeconds & "秒" & vbNewLine, True)
        ''tt(n-1,n-1)配列の作成完了

        RunProgressBarEvent(4, 8, "解析処理中")

        'カメラ結合（CTによる結合）
        sw.Reset()
        sw.Start()

        '全ImageのPose（姿勢）を計算
        If ConnectAllCameraByBasePoint(tt) < 2 Then
            '結合できた画像が２枚以上ではないため、終了
            MsgBox("画像結合が不充分です。", MsgBoxStyle.OkOnly, "確認")
            Exit Function
        End If
        'ConnectAllCameraByBestScale3(tt)
        'ConnectAllCameraByVector(tt)

        sw.Stop()
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "JikanMonitor.mon",
                                            "カメラ結合時間： " & sw.Elapsed.TotalSeconds & "秒" & vbNewLine, True)
        Trace.WriteLine("カメラ結合時間")
        Trace.WriteLine(sw.Elapsed.TotalMilliseconds & "ミリ秒")
#If DEBUG Then
        '3次元計測点を2次元計測点に投影するMatrix（逆行列）/1回目
        'CalcAllImagesInvertHomMat()

        '再投影誤差を求める /1回目
        ' CalcReProjectionError()
        'IPS.MonitorPairInfo(ProjectPath)
#End If
        RunProgressBarEvent(5, 8, "解析処理中(バンドル調整）")
        sw.Reset()
        sw.Start()

        MonitorImagePose(ProjectPath & "\MonitorImagePoseBaMae.txt")
        BTuple.WriteTuple(hv_CamparamOut, ProjectPath & "\MonitorCamPar_BaMae.tpl")
        'バンドル調整（B A）：再投影誤差を最小化する /1回目
        '  RunBA_New(FBMlib.T_treshold, False, hv_CamparamOut)
        MonitorImagePose(ProjectPath & "\MonitorImagePoseBaAto.txt")
        BTuple.WriteTuple(hv_CamparamOut, ProjectPath & "\MonitorCamPar_BaAto.tpl")
        sw.Stop()
        Trace.WriteLine("バンドル調整の時間")
        Trace.WriteLine(sw.Elapsed.TotalMilliseconds & "ミリ秒")
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "JikanMonitor.mon",
                                          "バンドル調整の時間： " & sw.Elapsed.TotalSeconds & "秒" & vbNewLine, True)
        'カメラの内部パラメーターの修正
        HOperatorSet.ChangeRadialDistortionCamPar("fixed", hv_CamparamOut, Zeros, hv_CamparamZero)
        RunProgressBarEvent(6, 8, "解析処理中")

        sw.Reset()
        sw.Start()
        '3次元計測点を2次元計測点に投影するMatrix（逆行列）/2回目
        CalcAllImagesInvertHomMat()
        '全てのレイを計算する。 /1回目
        CalcAllImages_Ray()
        'RMS（2乗平方根）を計算する。 /1回目
        CalcRMS()
        sw.Stop()
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "JikanMonitor.mon",
                                            "その他計算時間： " & sw.Elapsed.TotalSeconds & "秒" & vbNewLine, True)
#If False Then
        IkkatuSyori2 = True
        Exit Function
#End If
        '  If flgCTonly = False Then
        If False Then
#If DEBUG Then
            '再投影誤差を求める /2回目
            CalcReProjectionError()
#End If
            '仮のスケールを算出：（CT内のST間の計測距離の合計）/（CT内のST間の設計距離の合計）
            CalcKarinoScaleByCTPoints(ScaleMM)

            'シングルターゲットの番号付け
            sw.Reset()
            sw.Start()
            '  CalcAllImagesInvertHomMat()



            MinCountOfRays = 3
            ' SingleTargetNumbering(tt)
            ' SingleTargetNumbering2(tt)
            'SingleTargetNumbering3(tt)

            'SingleTargetNumbering4(tt)


            'STの候補を見つける。
            SingelTargetNumberingOther(tt)
            'STの候補からBadなSTを除く
            CheckAndRemoveBadSingleTarget()
            sw.Stop()
            Trace.WriteLine("シングルターゲットの番号付け時間")
            Trace.WriteLine(sw.Elapsed.TotalMilliseconds & "ミリ秒")
            RunProgressBarEvent(7, 8, "解析処理中")
            sw.Reset()
            sw.Start()

            'STを含めバンドル調整（B A）：再投影誤差を最小化する /2回目
            RunBA_New(FBMlib.T_treshold, True, hv_CamparamOut)
            sw.Stop()
            Trace.WriteLine("バンドル調整の時間")
            Trace.WriteLine(sw.Elapsed.TotalMilliseconds & "ミリ秒")

            ''3次元計測点を2次元計測点に投影するMatrix（逆行列）/3回目
            CalcAllImagesInvertHomMat()
        End If
        RunProgressBarEvent(8, 8, "解析処理中")
#If DEBUG Then
        ''再投影誤差を求める /3回目
        CalcReProjectionError()

        '全てのレイを計算する。 /2回目
        CalcAllImages_Ray()

        'RMS（2乗平方根）を計算する。 /2回目
        CalcRMS()

#End If
        IkkatuSyoriPair_Kijyun = True
    End Function
    Public Function CreateSTbyEdge() As Boolean
        CreateSTbyEdge = False

        If lstImages Is Nothing Then
            Exit Function
        End If
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim k As Integer = 0
        Dim n As Integer = lstImages.Count
        Dim IS1 As ImageSet
        Dim IS2 As ImageSet
        '全てのカメラペアを計算し、配列に格納。
        Dim tt(n - 1, n - 1) As ImagePairSet
        Dim t As Integer = 0
        Dim strt As String = ""
        FirstI = 0
        FirstJ = 0
        Dim FirstHyoka As Double = -1
        Dim MinPairPoseError As Double = Double.MaxValue '質問①
        Dim MinPairPoseMeanZ As Double = Double.MaxValue '質問②
        Dim Zeros As Object
        Zeros = BTuple.TupleGenConst(5, 0)
        HOperatorSet.ChangeRadialDistortionCamPar("fixed", hv_CamparamOut, Zeros, hv_CamparamZero)

        For Each ISI As ImageSet In lstImages
            If Not ISI.hx_EdgePS Is Nothing Then
                Dim RR As HTuple = Nothing
                Dim CC As HTuple = Nothing
                Dim NN As HTuple = Nothing
                HOperatorSet.ContourPointNumXld(ISI.hx_EdgePS, NN)

                RR = Nothing
                CC = Nothing
                For i = 1 To BTuple.TupleLength(NN).I
                    Dim tmpOneEdge As New HObject
                    Dim tmpRR As HTuple = Nothing
                    Dim tmpCC As HTuple = Nothing
                    Try
                        HOperatorSet.GenEmptyObj(tmpOneEdge)
                        HOperatorSet.SelectObj(ISI.hx_EdgePS, tmpOneEdge, i)
                        HOperatorSet.GetContourXld(tmpOneEdge, tmpRR, tmpCC)
                        RR = BTuple.TupleConcat(RR, tmpRR)
                        CC = BTuple.TupleConcat(CC, tmpCC)
                        HOperatorSet.ClearObj(tmpOneEdge)
                    Catch ex As Exception
                        Continue For
                    End Try            
                Next
                ISI.Targets.All2D_ST.Row = RR
                ISI.Targets.All2D_ST.Col = CC
                If ISI.Targets.lstST Is Nothing Then
                    ISI.Targets.lstST = New List(Of SingleTarget)
                Else
                    ISI.Targets.lstST.Clear()
                End If
                Dim nst As Integer = BTuple.TupleLength(RR).I
                For i = 0 To nst - 1
                    ISI.Targets.lstST.Add(New SingleTarget(ISI.ImageId, i + 1, RR(i), CC(i)))
                Next

            End If
            ISI.objCamparam.CamParamZero = hv_CamparamZero
        Next

        ''tt(n-1,n-1)配列の作成開始
        For i = 0 To n - 1
            For j = 0 To n - 1
                If i <> j Then '2枚のImageが同じかどうか
                    Dim IPS As ImagePairSet

                    IS1 = lstImages.Item(i)
                    IS2 = lstImages.Item(j)
                    IPS = New ImagePairSet(IS1, IS2) 'ImagePairSet作成
                    'IPS.Camparam = hv_CamparamOut 'SUURI DEL
                    tt(i, j) = IPS
                    t += 1
                    IPS.Pair_ID = t
                    If IPS.cntComCT >= intCommonCTnum Then '共通のCTが最低枚数を越えているか
                        IPS.calcRelPose() '相対Poseを算出

                        If IPS.cntComCT >= intCommonCTnum Then
                            If BTuple.TupleLength(IPS.PairPose.hError).I = 1 Then
                                If BTuple.TupleAbs(BTuple.TupleMean(IPS.PairPose.Z) / IPS.cntComCT) < MinPairPoseError Then
                                    If i < j Then
                                        FirstI = i
                                        FirstJ = j
                                        MinPairPoseError = BTuple.TupleAbs(BTuple.TupleMean(IPS.PairPose.Z) / IPS.cntComCT).D
                                    End If
                                End If
                            End If
                        End If
                    End If
                    strt = strt & IPS.cntComCT & "," '共通CTの数
                Else
                    strt = strt & "×" & ","
                End If
            Next
            strt = strt & vbNewLine
            RunProgressBarEvent(i + 1, n, "解析処理中(1)")
        Next

        '全ての計測点を格納する。
        If lstCommon3dST Is Nothing Then
            lstCommon3dST = New List(Of Common3DSingleTarget)
        Else
            lstCommon3dST.Clear()
        End If


        MinCountOfRays = 3
        ScaleMM = 1 / pScaleMM
        'CalcKarinoScaleByCTPoints(ScaleMM)
        ' ScaleMM = 0.001
        ''3次元計測点を2次元計測点に投影するMatrix（逆行列）/3回目
        CalcAllImagesInvertHomMat()
        '全てのレイを計算する。 /2回目
        CalcAllImages_Ray()

        'STの候補を見つける。
        SingelTargetNumberingOther(tt)
        'STの候補からBadなSTを除く
        CheckAndRemoveBadSingleTargetForEdge()
        CreateSTbyEdge = True

    End Function
    Public Function IkkatuSyori2(ByVal flgCTonly As Boolean) As Boolean '一括処理実行
        TimeMonStart()

        IkkatuSyori2 = False
        If lstImages Is Nothing Then
            Exit Function

        End If
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim k As Integer = 0
        Dim n As Integer = lstImages.Count
        Dim IS1 As ImageSet
        Dim IS2 As ImageSet
        Dim sw As New System.Diagnostics.Stopwatch()
        'メモリオーバーを防ぐ
        GC.Collect()
        GC.WaitForPendingFinalizers()
        Dim Zeros As Object
        Zeros = BTuple.TupleGenConst(5, 0)
        ' HOperatorSet.ReadCamPar("C:\My Work\MandS\HALCON9関連\NonTarget\camparamD3X_White_SxConst.cal", hv_Camparam)
        ' HOperatorSet.ChangeRadialDistortionCamPar("fixed", hv_Camparam, Zeros, hv_CamparamOut)

        'カメラの内部パラメータ(初期）を設定
        'CamParamの値
        hv_CamparamOut(0) = hv_CamparamFirst(0) 'Focus：
        hv_CamparamOut(1) = hv_CamparamFirst(1) 'Poly1：
        hv_CamparamOut(2) = hv_CamparamFirst(2) 'Poly2：
        hv_CamparamOut(3) = hv_CamparamFirst(3) 'Poly3：
        hv_CamparamOut(4) = hv_CamparamFirst(4) 'Poly4：
        hv_CamparamOut(5) = hv_CamparamFirst(5) 'Poly5：
        hv_CamparamOut(6) = hv_CamparamFirst(6) 'Sx：
        hv_CamparamOut(7) = hv_CamparamFirst(7) 'Sy：
        hv_CamparamOut(8) = hv_CamparamFirst(8) 'Cx：
        hv_CamparamOut(9) = hv_CamparamFirst(9) 'Cy：
        hv_CamparamOut(10) = hv_CamparamFirst(10) 'ImageWidth：
        hv_CamparamOut(11) = hv_CamparamFirst(11) 'ImageHeight：
        'SUURI ADD
        Dim blnSangyoCam As Integer = GetPrivateProfileInt("Kaiseki", "KaisekiSetting", 0, My.Application.Info.DirectoryPath & "\vform.ini")
        If blnSangyoCam = 1 Then
            ReadCamParamList(_projectpath & "\")
        Else
            ReadCamParamList()  'SUUR ADD 20150118
        End If

        '2つのImageに共通して写っているCT（共通CT）の最小数を設定（CommonCTnum.txtに記載。H25.4.10現在は"4",H28.04.01現在は3）
        intCommonCTnum = GetPrivateProfileInt("Kaiseki", "CommonCTnum", 3, My.Application.Info.DirectoryPath & "\vform.ini")

        sw.Reset()
        sw.Start()
        'RunProgressBarEvent(1, 8, "ターゲット抽出中")
        '画像からCTとSTの抽出
        If flgTargetMeasure = True Then
            If MsgBox("ターゲット抽出をしますか？", MsgBoxStyle.YesNo, "確認") = MsgBoxResult.Yes Then
                DetectTargetsAllImages()
            Else
                For Each ISI As ImageSet In lstImages
                    For Each ST As SingleTarget In ISI.Targets.lstST
                        ST.P3ID = -1
                        If ST.tmpPPP Is Nothing Then
                            ST.tmpPPP = New List(Of SingleTarget)
                        Else
                            ST.tmpPPP.Clear()
                        End If
                        ST.flgUsed = 0
                    Next
                Next
            End If
        Else
            '全Imageに対してTargetのスキャンを開始
            DetectTargetsAllImages()
        End If
        sw.Stop()
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "JikanMonitor.mon",
                                            "ターゲット抽出時間： " & sw.Elapsed.TotalSeconds & "秒" & vbNewLine, True)
        ImageCNT = n '画像枚数
        RunProgressBarEvent(2, 8, "解析処理中")
        sw.Reset()
        sw.Start()
        '全てのカメラペアを計算し、配列に格納。
        Dim tt(n - 1, n - 1) As ImagePairSet
        Dim t As Integer = 0
        Dim strt As String = ""
        FirstI = 0
        FirstJ = 0
        Dim FirstHyoka As Double = -1
        Dim MinPairPoseError As Double = Double.MaxValue '質問①
        Dim MinPairPoseMeanZ As Double = Double.MaxValue '質問②
#If DEBUG Then
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\RelPoseMonitor.txt", "Monitor開始", False)
#End If
        ''tt(n-1,n-1)配列の作成開始
        For i = 0 To n - 1
            For j = 0 To n - 1
                If i <> j Then '2枚のImageが同じかどうか
                    Dim IPS As ImagePairSet

                    IS1 = lstImages.Item(i)
                    IS2 = lstImages.Item(j)
                    IPS = New ImagePairSet(IS1, IS2) 'ImagePairSet作成
                    'IPS.Camparam = hv_CamparamOut 'SUURI DEL
                    tt(i, j) = IPS
                    t += 1
                    IPS.Pair_ID = t
                    If IPS.cntComCT >= intCommonCTnum Then '共通のCTが最低枚数を越えているか
                        IPS.calcRelPose() '相対Poseを算出
                        'MonitorPairInfo
                        'IPS.MonitorPairInfo(ProjectPath)

                        If IPS.cntComCT >= intCommonCTnum Then
                            If BTuple.TupleLength(IPS.PairPose.hError).I = 1 Then
                                'If IPS.PairPose.hError < MinPairPoseError And BTuple.TupleMean(IPS.PairPose.Z) < MinPairPoseMeanZ Then
                                '    FirstI = i
                                '    FirstJ = j
                                '    MinPairPoseError = IPS.PairPose.hError
                                '    MinPairPoseMeanZ = BTuple.TupleMean(IPS.PairPose.Z)
                                'End If
                                If BTuple.TupleAbs(1 - BTuple.TupleMean(IPS.PairPose.Z)) / IPS.cntComCT * IPS.PairPose.hError < MinPairPoseError Then
                                    If i < j Then
                                        FirstI = i
                                        FirstJ = j
                                        MinPairPoseError = (BTuple.TupleAbs(1 - BTuple.TupleMean(IPS.PairPose.Z)) / IPS.cntComCT * IPS.PairPose.hError).D
                                    End If

                                End If
                            End If
                        End If
                    End If
                    strt = strt & IPS.cntComCT & "," '共通CTの数
                Else
                    strt = strt & "×" & ","
                End If
            Next
            strt = strt & vbNewLine
            RunProgressBarEvent(i + 1, n, "解析処理中(1)")
        Next

        sw.Stop()
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "JikanMonitor.mon",
                                            "２画像組の計算時間： " & sw.Elapsed.TotalSeconds & "秒" & vbNewLine, True)

        ''MsgBox(strErrMsg, MsgBoxStyle.OkOnly, "FBMlib") 'ADD By Yamada 20150116 


        ''tt(n-1,n-1)配列の作成完了


#If DEBUG Then
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\RelPoseMonitor.txt", "Monitor終了", True)
#End If
        RunProgressBarEvent(3, 8, "解析処理中")
        My.Computer.FileSystem.WriteAllText(ProjectPath & "\Test.csv", strt, True)
        '全ての計測点を格納する。
        If lstCommon3dST Is Nothing Then
            lstCommon3dST = New List(Of Common3DSingleTarget)
        Else
            lstCommon3dST.Clear()
        End If
        '全てのコードターゲットを格納する。
        If lstCommon3dCT Is Nothing Then
            lstCommon3dCT = New List(Of Common3DCodedTarget)
        Else
            lstCommon3dCT.Clear()
        End If

        sw.Reset()
        sw.Start()
        'コードターゲットをまとめる
        CollectCT()
        sw.Stop()
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "JikanMonitor.mon",
                                            "コードターゲットをまとめる時間： " & sw.Elapsed.TotalSeconds & "秒" & vbNewLine, True)
        ''tt(n-1,n-1)配列の作成完了

        RunProgressBarEvent(4, 8, "解析処理中")

        'カメラ結合（CTによる結合）
        sw.Reset()
        sw.Start()

        '全ImageのPose（姿勢）を計算
        If ConnectAllCameraByBestScale2(tt) < 2 Then
            '結合できた画像が２枚以上ではないため、終了
            MsgBox("画像結合が不充分です。", MsgBoxStyle.OkOnly, "確認")
            Exit Function
        End If
        'ConnectAllCameraByBestScale3(tt)
        'ConnectAllCameraByVector(tt)

        sw.Stop()
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "JikanMonitor.mon",
                                            "カメラ結合時間： " & sw.Elapsed.TotalSeconds & "秒" & vbNewLine, True)
        Trace.WriteLine("カメラ結合時間")
        Trace.WriteLine(sw.Elapsed.TotalMilliseconds & "ミリ秒")
#If DEBUG Then
        '3次元計測点を2次元計測点に投影するMatrix（逆行列）/1回目
        'CalcAllImagesInvertHomMat()

        '再投影誤差を求める /1回目
        ' CalcReProjectionError()
#End If
        RunProgressBarEvent(5, 8, "解析処理中(バンドル調整）")
        sw.Reset()
        sw.Start()

        ''TOBEREMOVED
        'HOperatorSet.ReadTuple("C:\ComparedTo0306\OLD\MonitorCamPar_BaMae.tpl", hv_CamparamOut)
        'Using sr As New StreamReader("C:\ComparedTo0306\OLD\MonitorImagePoseBaMae.txt")
        '    While Not sr.EndOfStream
        '        Dim line As String
        '        line = sr.ReadLine()

        '        Dim sets As String() = line.Split(",")
        '        For Each ISI As ImageSet In lstImages
        '            If ISI.ImageId = CInt(sets(0)) Then
        '                Dim tmpPose As New HTuple
        '                tmpPose(0) = CDbl(sets(2))
        '                tmpPose(1) = CDbl(sets(3))
        '                tmpPose(2) = CDbl(sets(4))
        '                tmpPose(3) = CDbl(sets(5))
        '                tmpPose(4) = CDbl(sets(6))
        '                tmpPose(5) = CDbl(sets(7))
        '                tmpPose(6) = 0
        '                ISI.ImagePose.Pose = tmpPose
        '            End If
        '        Next
        '    End While
        'End Using




        MonitorImagePose(ProjectPath & "\MonitorImagePoseBaMae.txt")
        BTuple.WriteTuple(hv_CamparamOut, ProjectPath & "\MonitorCamPar_BaMae.tpl")

        'バンドル調整（B A）：再投影誤差を最小化する /1回目
        RunBA_New(FBMlib.T_treshold, False, hv_CamparamOut)
        MonitorImagePose(ProjectPath & "\MonitorImagePoseBaAto.txt")
        BTuple.WriteTuple(hv_CamparamOut, ProjectPath & "\MonitorCamPar_BaAto.tpl")
        sw.Stop()
        Trace.WriteLine("バンドル調整の時間")
        Trace.WriteLine(sw.Elapsed.TotalMilliseconds & "ミリ秒")
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "JikanMonitor.mon",
                                          "バンドル調整の時間： " & sw.Elapsed.TotalSeconds & "秒" & vbNewLine, True)


        'カメラの内部パラメーターの修正
        HOperatorSet.ChangeRadialDistortionCamPar("fixed", hv_CamparamOut, Zeros, hv_CamparamZero)
        RunProgressBarEvent(6, 8, "解析処理中")

        sw.Reset()
        sw.Start()
        '3次元計測点を2次元計測点に投影するMatrix（逆行列）/2回目
        CalcAllImagesInvertHomMat()
        '全てのレイを計算する。 /1回目
        CalcAllImages_Ray()
        'RMS（2乗平方根）を計算する。 /1回目
        CalcRMS()
        sw.Stop()
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "JikanMonitor.mon",
                                            "その他計算時間： " & sw.Elapsed.TotalSeconds & "秒" & vbNewLine, True)
#If False Then
        IkkatuSyori2 = True
        Exit Function
#End If
        If flgCTonly = False Then

#If DEBUG Then
            '再投影誤差を求める /2回目
            CalcReProjectionError()
#End If
            '仮のスケールを算出：（CT内のST間の計測距離の合計）/（CT内のST間の設計距離の合計）
            CalcKarinoScaleByCTPoints(ScaleMM)

            'シングルターゲットの番号付け
            sw.Reset()
            sw.Start()
            '  CalcAllImagesInvertHomMat()



            MinCountOfRays = 3
            ' SingleTargetNumbering(tt)
            ' SingleTargetNumbering2(tt)
            'SingleTargetNumbering3(tt)

            'SingleTargetNumbering4(tt)


            'STの候補を見つける。
            SingelTargetNumberingOther(tt)
            'STの候補からBadなSTを除く
            CheckAndRemoveBadSingleTarget()
            sw.Stop()
            Trace.WriteLine("シングルターゲットの番号付け時間")
            Trace.WriteLine(sw.Elapsed.TotalMilliseconds & "ミリ秒")
            RunProgressBarEvent(7, 8, "解析処理中")
            sw.Reset()
            sw.Start()

            'STを含めバンドル調整（B A）：再投影誤差を最小化する /2回目
            RunBA_New(FBMlib.T_treshold, True, hv_CamparamOut)
            sw.Stop()
            Trace.WriteLine("バンドル調整の時間")
            Trace.WriteLine(sw.Elapsed.TotalMilliseconds & "ミリ秒")

            ''3次元計測点を2次元計測点に投影するMatrix（逆行列）/3回目
            CalcAllImagesInvertHomMat()
        End If
        RunProgressBarEvent(8, 8, "解析処理中")
#If DEBUG Then
        ''再投影誤差を求める /3回目
        CalcReProjectionError()

        '全てのレイを計算する。 /2回目
        CalcAllImages_Ray()

        'RMS（2乗平方根）を計算する。 /2回目
        CalcRMS()

#End If
        IkkatuSyori2 = True

        TimeMonEnd()
    End Function
    Public Function CalcRMS() As Double
        TimeMonStart()

        Dim Dist_forRMS As Object = Nothing
        Dim AllDist_forRMS As Object = Nothing
        Dim minX As Double = Double.MaxValue
        Dim minY As Double = Double.MaxValue
        Dim minZ As Double = Double.MaxValue
        Dim maxX As Double = Double.MinValue
        Dim maxY As Double = Double.MinValue
        Dim maxZ As Double = Double.MinValue
        Dim lenXYZ As New HTuple
        Dim LenZentai As Double
        For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
            If C3DCT.flgUsable = True Then
                For Each CT As CodedTarget In C3DCT.lstCT
                    Dim ii As Integer = 0
                    For Each ST As SingleTarget In CT.lstCTtoST
                        Dist_forRMS = Nothing
                        GetMinMaxXYZ(minX, minY, minZ, maxX, maxY, maxZ, C3DCT.lstP3d(ii))
                        CalcTenRayDist(ST.RayP1, ST.RayP2, C3DCT.lstP3d(ii), Dist_forRMS)
                        AllDist_forRMS = BTuple.TupleConcat(AllDist_forRMS, Dist_forRMS)
                        ii += 1
                    Next
                Next
            End If
        Next
        For Each C3DST As Common3DSingleTarget In lstCommon3dST
            For Each ST As SingleTarget In C3DST.lstST
                GetMinMaxXYZ(minX, minY, minZ, maxX, maxY, maxZ, C3DST.P3d)
                CalcTenRayDist(ST.RayP1, ST.RayP2, C3DST.P3d, Dist_forRMS)
                AllDist_forRMS = BTuple.TupleConcat(AllDist_forRMS, Dist_forRMS)
            Next
        Next
        'For Each ISI As ImageSet In lstImages
        '    If ISI.flgConnected = True Then
        '        GetMinMaxXYZ(minX, minY, minZ, maxX, maxY, maxZ, New Point3D(ISI.ImagePose.Pose(0), ISI.ImagePose.Pose(1), ISI.ImagePose.Pose(2)))
        '    End If
        'Next
        lenXYZ(0) = maxX - minX
        lenXYZ(1) = maxY - minY
        lenXYZ(2) = maxZ - minZ
        LenZentai = BTuple.TupleSqrt(BTuple.TupleSum(BTuple.TuplePow(lenXYZ, 2))).D
        If LenZentai < Double.Epsilon Then
            Exit Function
        End If
        Dim PowAllDist As Object
        Dim Mean_PowAllDist As Object
        Dim RMS As Object
        PowAllDist = BTuple.TuplePow(AllDist_forRMS, 2)
        Mean_PowAllDist = BTuple.TupleMean(PowAllDist)
        RMS = BTuple.TupleSqrt(Mean_PowAllDist) / LenZentai
        Dim mm As Object
        mm = BTuple.TuplePow(BTuple.TupleMean(AllDist_forRMS), 2)
        Dim dd As Object
        dd = BTuple.TuplePow(BTuple.TupleDeviation(AllDist_forRMS), 2)
        Dim objRMS As Object
        objRMS = BTuple.TupleSqrt(mm + dd) / LenZentai
        CalcRMS = objRMS.D

        My.Computer.FileSystem.WriteAllText(ProjectPath & "\BAMonitor.csv", "RMS(無単位):" & objRMS.D & vbNewLine, True)

        TimeMonEnd()
    End Function
    Private Sub GetMinMaxXYZ(ByRef minX As Double, ByRef minY As Double, ByRef minZ As Double,
                             ByRef maxX As Double, ByRef maxY As Double, ByRef maxZ As Double,
                             ByRef P3D As Point3D)
        If P3D.X < minX Then
            minX = P3D.X
        End If
        If P3D.Y < minY Then
            minY = P3D.Y
        End If
        If P3D.Z < minZ Then
            minZ = P3D.Z
        End If
        If P3D.X > maxX Then
            maxX = P3D.X
        End If
        If P3D.Y > maxY Then
            maxY = P3D.Y
        End If
        If P3D.Z > maxZ Then
            maxZ = P3D.Z
        End If
    End Sub

    Public Sub RunBundleAdjOnly()
        Dim tmpCamParam As Object = hv_CamparamOut

        Dim Zeros As Object
        Zeros = BTuple.TupleGenConst(5, 0)
        ' HOperatorSet.ChangeRadialDistortionCamPar("fixed", hv_Camparam, Zeros, hv_CamparamOut)

        Dim blnSangyoCam As Integer = GetPrivateProfileInt("Kaiseki", "KaisekiSetting", 0, My.Application.Info.DirectoryPath & "\vform.ini")
        If blnSangyoCam = 1 Then
            ReadCamParamList(_projectpath & "\")
        Else
            ReadCamParamList()  'SUUR ADD 20150118
        End If

        RunBA_New(FBMlib.T_treshold, True, hv_CamparamOut)


        ''全てのレイを計算する。
        CalcAllImages_Ray()
        CalcRMS()
        ''全ての計測点の三次元座標を算出する。
        ' CalcALLSingleTarget3dCoord()
        CalcAllImagesInvertHomMat()
        CalcReProjectionError()

        ' hv_CamparamOut = tmpCamParam
        'CalcALLSingleTarget3dCoordByHalcon()
        'CalcALLCodedTarget3dCoordByHalcon()
        'CalcReProjectionError()
    End Sub
    Public Sub SingleTargetNumberingOnly()
        Dim Zeros As Object
        Zeros = BTuple.TupleGenConst(5, 0)
        HOperatorSet.ChangeRadialDistortionCamPar("fixed", hv_CamparamFirst, Zeros, hv_CamparamOut)
        cntConnectedImage = 0
        For Each ISI As ImageSet In lstImages
            If ISI.flgConnected = True Then
                cntConnectedImage += 1
            End If
            For Each ST As SingleTarget In ISI.Targets.lstST
                ST.P3ID = -1
                ST.flgUsed = 0
                ST.tmpPPP.Clear()
            Next
        Next
        If lstCommon3dST Is Nothing Then
            lstCommon3dST = New List(Of Common3DSingleTarget)
        Else
            lstCommon3dST.Clear()
        End If
        If lstCommon3dCT Is Nothing Then
            lstCommon3dCT = New List(Of Common3DCodedTarget)
        Else
            lstCommon3dCT.Clear()

        End If
        'コードターゲットをまとめる
        CollectCT()

        Dim IS1 As ImageSet
        Dim IS2 As ImageSet
        Dim i As Integer
        Dim j As Integer
        Dim n As Integer = lstImages.Count
        '全てのカメラペアを計算し、配列に格納。
        Dim tt(n - 1, n - 1) As ImagePairSet
        Dim t As Integer = 0
        For i = 0 To n - 1
            For j = 0 To n - 1
                If i <> j Then
                    Dim IPS As ImagePairSet

                    IS1 = lstImages.Item(i)
                    IS2 = lstImages.Item(j)
                    IPS = New ImagePairSet(IS1, IS2)
                    ' IPS.Camparam = hv_CamparamOut 'SUURI DEL
                    tt(i, j) = IPS
                    t += 1
                    IPS.Pair_ID = t
                    If IPS.cntComCT >= intCommonCTnum Then
                        IPS.calcRelPose()
                    End If
                End If
            Next
        Next

        CalcALLCodedTarget3dCoordByHalcon()
        CalcKarinoScaleByCTPoints(ScaleMM)


        ' ここから下がSTのナンバリング処理

        'HOperatorSet.ChangeRadialDistortionCamPar("fixed", hv_CamparamOut, Zeros, hv_CamparamOut2)
        'For Each ISI As ImageSet In lstImages
        '    HOperatorSet.ChangeRadialDistortionPoints(ISI.Targets.All2D_ST.Row, ISI.Targets.All2D_ST.Col, hv_CamparamOut, hv_CamparamOut2, ISI.Targets.All2D_ST.Row, ISI.Targets.All2D_ST.Col)
        'Next
        Dim sw As New System.Diagnostics.Stopwatch()
        'シングルターゲットの番号付け
        sw.Reset()
        sw.Start()
        CalcAllImagesInvertHomMat()
        '全てのレイを計算する。
        CalcAllImages_Ray()
        MinCountOfRays = 3
        ' SingleTargetNumbering(tt)
        ' SingleTargetNumbering2(tt)
        'SingleTargetNumbering3(tt)

        'SingleTargetNumbering4(tt)

        SingelTargetNumberingOther(tt)
        CheckAndRemoveBadSingleTarget()
        sw.Stop()
        Trace.WriteLine("シングルターゲットの番号付け時間")
        Trace.WriteLine(sw.Elapsed.TotalMilliseconds & "ミリ秒")

        sw.Reset()
        sw.Start()
        RunBA_New(FBMlib.T_treshold, True, hv_CamparamOut)
        sw.Stop()
        Trace.WriteLine("バンドル調整の時間")
        Trace.WriteLine(sw.Elapsed.TotalMilliseconds & "ミリ秒")

        CalcAllImagesInvertHomMat()
        CalcReProjectionError()

    End Sub

    Public Sub CheckAndRemoveBadSingleTarget()
        TimeMonStart()

        Dim i As Integer
        Dim j As Integer
        Dim n As Integer = lstCommon3dST.Count

        'Common3DSingleTarget点に同じ画像の複数点が含まれる場合に一つにする。
        For Each C3DST As Common3DSingleTarget In lstCommon3dST
            Dim m As Integer = C3DST.lstST.Count
            Dim SameImageIndex(m - 1) As Integer
            Dim tmpC3DST As New Common3DSingleTarget

            For i = 0 To m - 2
                For j = i + 1 To m - 1
                    Dim STT1 As SingleTarget = C3DST.lstST(i)
                    Dim STT2 As SingleTarget = C3DST.lstST(j)
                    If STT1.ImageID = STT2.ImageID And STT1.P2ID <> STT2.P2ID Then
                        SameImageIndex(i) = 1
                        SameImageIndex(j) = 1
                    End If
                Next
            Next

            i = 0
            For Each ST As SingleTarget In C3DST.lstST
                If SameImageIndex(i) = 0 Then
                    tmpC3DST.lstST.Add(ST)
                End If
                i += 1
                If ST.ImageID = 1 And ST.P2ID = 82 Then
                    i = i
                End If
            Next
            If tmpC3DST.lstST.Count >= 2 Then
                If tmpC3DST.Calc3dPoints() = True Then
                    m = C3DST.lstST.Count
                    For i = m - 1 To 0 Step -1
                        If SameImageIndex(i) = 1 Then
                            Dim Dist As Object = Nothing
                            CalcTenRayDist(C3DST.lstST.Item(i).RayP1, C3DST.lstST.Item(i).RayP2, tmpC3DST.P3d, Dist)
                            If Dist > 5 * ScaleMM Then
                                C3DST.lstST.RemoveAt(i)
                            End If
                        End If
                    Next
                Else
                    C3DST.lstST.Clear()
                    C3DST.PID = -1
                End If
            Else
                C3DST.lstST.Clear()
                C3DST.PID = -1
            End If
        Next

        '各シングルターゲットの全てのレイを参照し、不要なレイを削除する
        For Each C3DST As Common3DSingleTarget In lstCommon3dST
            IsAruBadPoint(C3DST)
            ' RemoveBadPoint(C3DST)
        Next

        n = lstCommon3dST.Count
        'CTに近いSTを削除する。
        Dim tmpF As Boolean = False
        For i = n - 1 To 0 Step -1
            If lstCommon3dST.Item(i).lstST.Count < MinCountOfRays Then
                'lstCommon3dST.RemoveAt(i)
            Else
                tmpF = False
                For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
                    For Each P3d As Point3D In C3DCT.lstP3d
                        Dim dist As Object = Nothing
                        P3d.GetDisttoOtherPose(lstCommon3dST.Item(i).P3d, dist)
                        If dist < 0.0011 * ScaleMM Then
                            lstCommon3dST.RemoveAt(i)
                            tmpF = True
                            Exit For
                        End If
                    Next
                    If tmpF = True Then
                        Exit For
                    End If
                Next
            End If
        Next


        'お互いに近いST同士を合併する。
        For Each C3DST1 As Common3DSingleTarget In lstCommon3dST

            If C3DST1.lstST.Count >= 2 Then
                Do
                    Dim mindist As Double = Double.MaxValue
                    Dim nearestC3DST As New Common3DSingleTarget
                    Dim dist As Double
                    For Each C3DST2 As Common3DSingleTarget In lstCommon3dST
                        If C3DST1.PID <> C3DST2.PID And C3DST2.lstST.Count >= MinCountOfRays And C3DST1.PID <> -1 And C3DST2.PID <> -1 Then
                            C3DST1.P3d.GetDisttoOtherPose(C3DST2.P3d, dist)
                            If mindist > dist Then
                                mindist = dist
                                nearestC3DST = C3DST2
                            End If
                        Else
                            If C3DST1.PID <> C3DST2.PID Then
                                C3DST1.PID = C3DST1.PID
                            End If
                        End If
                    Next

                    If mindist < 15 * ScaleMM Then
                        For Each ST As SingleTarget In nearestC3DST.lstST
                            Dim flgSamePoint As Boolean = False
                            For Each STo As SingleTarget In C3DST1.lstST
                                If STo.IsSamePoint(ST) = True Then
                                    flgSamePoint = True
                                    Exit For
                                End If
                            Next
                            If flgSamePoint = False Then
                                C3DST1.lstST.Add(ST)
                            End If
                        Next
                        nearestC3DST.PID = -1
                        '20121205 SUURI ADD START 結合したレイ群内でも不要なレイを削除する必要がある。
                        IsAruBadPoint(C3DST1)
                        '20121205 SUURI ADD END
                    Else
                        Exit Do
                    End If
                Loop
            End If
        Next

        n = lstCommon3dST.Count
        For i = n - 1 To 0 Step -1
            Dim tmpPID As Integer = lstCommon3dST.Item(i).PID
            If lstCommon3dST.Item(i).lstST.Count < MinCountOfRays Or tmpPID = -1 Then
                lstCommon3dST.RemoveAt(i)
            Else
                If lstCommon3dST.Item(i).Calc3dPoints() = True Then
                    For Each ST As SingleTarget In lstCommon3dST.Item(i).lstST
                        ST.P3ID = tmpPID
                        ST.flgUsed = 1
                    Next
                    CalcReProjectionErrorOnePoint(lstCommon3dST.Item(i))
                End If
            End If
        Next
        For Each ISI As ImageSet In lstImages
            For Each ST As SingleTarget In ISI.Targets.lstST
                Dim tempF As Boolean = False
                For Each C3DST As Common3DSingleTarget In lstCommon3dST
                    For Each objST As SingleTarget In C3DST.lstST
                        If ST.IsSamePoint(objST) Then
                            If tempF = True Then
                                tempF = tempF
                            End If
                            tempF = True
                            Exit For
                        End If
                    Next
                    'If tempF = True Then
                    '    Exit For
                    'End If
                Next
                If tempF = False Then
                    ST.P3ID = -1
                    ST.flgUsed = 0
                End If
            Next
        Next

        TimeMonEnd()

    End Sub


    Public Sub CheckAndRemoveBadSingleTargetForEdge()
        Dim i As Integer
        Dim j As Integer
        Dim n As Integer = lstCommon3dST.Count

        'Common3DSingleTarget点に同じ画像の複数点が含まれる場合に一つにする。
        For Each C3DST As Common3DSingleTarget In lstCommon3dST
            Dim m As Integer = C3DST.lstST.Count
            Dim SameImageIndex(m - 1) As Integer
            Dim tmpC3DST As New Common3DSingleTarget

            For i = 0 To m - 2
                For j = i + 1 To m - 1
                    Dim STT1 As SingleTarget = C3DST.lstST(i)
                    Dim STT2 As SingleTarget = C3DST.lstST(j)
                    If STT1.ImageID = STT2.ImageID And STT1.P2ID <> STT2.P2ID Then
                        SameImageIndex(i) = 1
                        SameImageIndex(j) = 1
                    End If
                Next
            Next

            i = 0
            For Each ST As SingleTarget In C3DST.lstST
                If SameImageIndex(i) = 0 Then
                    tmpC3DST.lstST.Add(ST)
                End If
                i += 1
                If ST.ImageID = 1 And ST.P2ID = 82 Then
                    i = i
                End If
            Next
            If tmpC3DST.lstST.Count >= 2 Then
                If tmpC3DST.Calc3dPoints() = True Then
                    m = C3DST.lstST.Count
                    For i = m - 1 To 0 Step -1
                        If SameImageIndex(i) = 1 Then
                            Dim Dist As Object = Nothing
                            CalcTenRayDist(C3DST.lstST.Item(i).RayP1, C3DST.lstST.Item(i).RayP2, tmpC3DST.P3d, Dist)
                            If Dist > 5 * ScaleMM Then
                                C3DST.lstST.RemoveAt(i)
                            End If
                        End If
                    Next
                Else
                    C3DST.lstST.Clear()
                    C3DST.PID = -1
                End If
            Else
                C3DST.lstST.Clear()
                C3DST.PID = -1
            End If
        Next

        '各シングルターゲットの全てのレイを参照し、不要なレイを削除する
        For Each C3DST As Common3DSingleTarget In lstCommon3dST
            IsAruBadPoint2(C3DST)
            ' RemoveBadPoint(C3DST)
        Next

        'n = lstCommon3dST.Count


        n = lstCommon3dST.Count
        For i = n - 1 To 0 Step -1
            Dim tmpPID As Integer = lstCommon3dST.Item(i).PID
            If lstCommon3dST.Item(i).lstST.Count < 2 Or tmpPID = -1 Then
                lstCommon3dST.RemoveAt(i)
            Else
                If lstCommon3dST.Item(i).Calc3dPoints() = True Then
                    For Each ST As SingleTarget In lstCommon3dST.Item(i).lstST
                        ST.P3ID = tmpPID
                        ST.flgUsed = 1
                    Next
                    CalcReProjectionErrorOnePoint(lstCommon3dST.Item(i))
                End If
            End If
        Next
        For Each ISI As ImageSet In lstImages
            For Each ST As SingleTarget In ISI.Targets.lstST
                Dim tempF As Boolean = False
                For Each C3DST As Common3DSingleTarget In lstCommon3dST
                    For Each objST As SingleTarget In C3DST.lstST
                        If ST.IsSamePoint(objST) Then
                            If tempF = True Then
                                tempF = tempF
                            End If
                            tempF = True
                            Exit For
                        End If
                    Next
                    'If tempF = True Then
                    '    Exit For
                    'End If
                Next
                If tempF = False Then
                    ST.P3ID = -1
                    ST.flgUsed = 0
                End If
            Next
        Next

    End Sub
    '20121205 SUURI ADD START 
    Private Sub RemoveBadPoint(ByRef C3DST As Common3DSingleTarget)
        Do
            If C3DST.lstST.Count < MinCountOfRays Then
                Exit Do
            Else
                If C3DST.Calc3dPoints() = True Then
                    If CalcReProjectionErrorOnePoint(C3DST) = False Then
                        C3DST.lstST.Clear()
                        Exit Do
                    End If
                Else
                    C3DST.lstST.Clear()
                    Exit Do
                End If
            End If
            If C3DST.CheckAndRemoveBadSingleTarget(2 * ScaleMM) = False Then
                Exit Do
            End If
        Loop
    End Sub

    Public Function IsAruCommon3DSingleTarget(ByVal objST As SingleTarget) As Common3DSingleTarget
        IsAruCommon3DSingleTarget = Nothing
        For Each C3DST As Common3DSingleTarget In lstCommon3dST
            For Each ST As SingleTarget In C3DST.lstST
                If objST.IsSamePoint(ST) = True Then
                    IsAruCommon3DSingleTarget = C3DST
                    Exit Function
                End If
            Next
        Next
    End Function

    '20121205 SUURI ADD END
    '20121214 SUURI ADD START 
    Private Sub IsAruBadPoint(ByRef C3DST As Common3DSingleTarget)
        Do
            If C3DST.lstST.Count < MinCountOfRays Then
                If C3DST.lstST.Count >= 2 Then
                    If C3DST.Calc3dPoints() = False Then
                        C3DST.lstST.Clear()
                    End If
                Else
                    C3DST.lstST.Clear()
                End If
                Exit Do
            Else

                If C3DST.Calc3dPoints = True Then
                    If CalcReProjectionErrorOnePoint(C3DST) = True Then
                        Dim tmpMaxReProjErr As Double = C3DST.maxProjErrST.ReProjectionError.D
                        If tmpMaxReProjErr > 1 Then
                            Dim minDist As Double = Double.MaxValue
                            Dim minReProjErr As Double = Double.MaxValue
                            Dim minST As New SingleTarget
                            For Each ST As SingleTarget In C3DST.lstST
                                Dim tmpC3DST As New Common3DSingleTarget
                                For Each objST As SingleTarget In C3DST.lstST
                                    If objST.IsSamePoint(ST) = True Then
                                    Else
                                        tmpC3DST.lstST.Add(objST)
                                    End If
                                Next
                                If tmpC3DST.Calc3dPoints = True Then
                                    If CalcReProjectionErrorOnePoint(tmpC3DST) = True Then
                                        If tmpC3DST.maxProjErrST.ReProjectionError.D < minReProjErr Then
                                            minReProjErr = tmpC3DST.maxProjErrST.ReProjectionError.D
                                            minST = ST
                                        End If
                                    End If
                                End If
                            Next
                            If tmpMaxReProjErr / minReProjErr >= 2 Then
                                C3DST.RemoveSTByST(minST)
                            Else
                                If C3DST.CheckAndRemoveBadSingleTarget(2 * ScaleMM) = False Then
                                    Exit Do
                                End If
                            End If
                        Else
                            Exit Do
                        End If
                    Else
                        C3DST.lstST.Clear()
                        Exit Do
                    End If
                End If
            End If

        Loop


    End Sub
    '20121214 SUURI ADD END
    Private Sub IsAruBadPoint2(ByRef C3DST As Common3DSingleTarget)
        Do
            If C3DST.lstST.Count < MinCountOfRays Then
                If C3DST.lstST.Count >= 2 Then
                    If C3DST.Calc3dPoints() = False Then
                        C3DST.lstST.Clear()
                    End If
                Else
                    C3DST.lstST.Clear()
                End If
                Exit Do
            Else

                If C3DST.Calc3dPoints = True Then
                    If CalcReProjectionErrorOnePoint(C3DST) = True Then
                        Dim tmpMaxReProjErr As Double = C3DST.maxProjErrST.ReProjectionError
                        If tmpMaxReProjErr > 1 Then
                            Dim minDist As Double = Double.MaxValue
                            Dim minReProjErr As Double = Double.MaxValue
                            Dim minST As New SingleTarget
                            For Each ST As SingleTarget In C3DST.lstST
                                Dim tmpC3DST As New Common3DSingleTarget
                                For Each objST As SingleTarget In C3DST.lstST
                                    If objST.IsSamePoint(ST) = True Then
                                    Else
                                        tmpC3DST.lstST.Add(objST)
                                    End If
                                Next
                                If tmpC3DST.Calc3dPoints = True Then
                                    If CalcReProjectionErrorOnePoint(tmpC3DST) = True Then
                                        If tmpC3DST.maxProjErrST.ReProjectionError < minReProjErr Then
                                            minReProjErr = tmpC3DST.maxProjErrST.ReProjectionError
                                            minST = ST
                                        End If
                                    End If
                                End If
                            Next
                            If tmpMaxReProjErr / minReProjErr >= 5 Then
                                C3DST.RemoveSTByST(minST)
                            Else
                                If C3DST.CheckAndRemoveBadSingleTarget(2 * ScaleMM) = False Then
                                    Exit Do
                                End If
                            End If
                        Else
                            Exit Do
                        End If
                    Else
                        C3DST.lstST.Clear()
                        Exit Do
                    End If
                End If
            End If

        Loop


    End Sub

    Public Sub CalcALLSingleTarget3dCoordByHalcon()
        'initCameraSetup
        Dim CamSetupModelID As Object = Nothing
        HOperatorSet.CreateCameraSetupModel(lstImages.Count, CamSetupModelID)
        'Dim AllRow As Object = Nothing
        'Dim AllCol As Object = Nothing
        'Dim All3DPointsIndex As Object = Nothing
        'Dim AllCams As Object = Nothing

        Dim N As Integer = lstCommon3dST.Count * lstImages.Count
        Dim AllRow(N) As Double
        Dim AllCol(N) As Double
        Dim All3DPointsIndex(N) As Integer
        Dim AllCams(N) As Integer
        Dim i As Integer = 0


        For Each ISI As ImageSet In lstImages
            If ISI.flgConnected = True Then
                HOperatorSet.SetCameraSetupCamParam(CamSetupModelID, ISI.ImageId - 1, "area_scan_polynomial", hv_CamparamOut, ISI.ImagePose.Pose)
            End If
        Next
        Dim cnt As Integer = 0
        For Each C3DST As Common3DSingleTarget In lstCommon3dST
            For Each ST As SingleTarget In C3DST.lstST
                If lstImages(ST.ImageID - 1).flgConnected = True Then
                    'AllRow = BTuple.TupleConcat(AllRow, ST.P2D.Row)
                    'AllCol = BTuple.TupleConcat(AllCol, ST.P2D.Col)
                    'All3DPointsIndex = BTuple.TupleConcat(All3DPointsIndex, ST.P3ID)
                    'AllCams = BTuple.TupleConcat(AllCams, ST.ImageID - 1)

                    AllRow(cnt) = ST.P2D.Row
                    AllCol(cnt) = ST.P2D.Col
                    All3DPointsIndex(cnt) = ST.P3ID
                    AllCams(cnt) = ST.ImageID - 1
                    cnt += 1
                End If
            Next
        Next

        ReDim Preserve AllRow(cnt - 1)
        ReDim Preserve AllCol(cnt - 1)
        ReDim Preserve All3DPointsIndex(cnt - 1)
        ReDim Preserve AllCams(cnt - 1)

        Dim StereoModelID As Object = Nothing
        HOperatorSet.CreateStereoModel(CamSetupModelID, "points_3d", Nothing, Nothing, StereoModelID)
        Dim X As Object = Nothing
        Dim Y As Object = Nothing
        Dim Z As Object = Nothing
        Dim Pindex As Object = Nothing
        HOperatorSet.ReconstructPointsStereo(StereoModelID, AllRow, AllCol, Nothing, AllCams, All3DPointsIndex, X, Y, Z, Nothing, Pindex)

        For Each C3DST As Common3DSingleTarget In lstCommon3dST
            Dim T As Object = BTuple.TupleFind(Pindex, C3DST.PID)
            If BTuple.TupleSelect(T, 0) <> -1 Then
                C3DST.P3d.X = BTuple.TupleSelect(X, T)
                C3DST.P3d.Y = BTuple.TupleSelect(Y, T)
                C3DST.P3d.Z = BTuple.TupleSelect(Z, T)
            End If
        Next
        HOperatorSet.ClearCameraSetupModel(CamSetupModelID)
        HOperatorSet.ClearStereoModel(StereoModelID)

    End Sub

    Public Function CalcALLCodedTarget3dCoordByHalcon() As Boolean
        'initCameraSetup
        CalcALLCodedTarget3dCoordByHalcon = False
        Dim CamSetupModelID As Object = Nothing
        HOperatorSet.CreateCameraSetupModel(lstImages.Count, CamSetupModelID)
        'Dim AllRow As Object = Nothing
        'Dim AllCol As Object = Nothing
        'Dim All3DPointsIndex As Object = Nothing
        'Dim AllCams As Object = Nothing
        Dim N As Integer = lstCommon3dCT.Count * lstImages.Count * CodedTarget.CTnoSTnum
        Dim AllRow(N) As Double
        Dim AllCol(N) As Double
        Dim All3DPointsIndex(N) As Integer
        Dim AllCams(N) As Integer
        Dim i As Integer = 0

        For Each ISI As ImageSet In lstImages
            If ISI.flgConnected = True Then
                HOperatorSet.SetCameraSetupCamParam(CamSetupModelID, ISI.ImageId - 1, "area_scan_polynomial", hv_CamparamOut, ISI.ImagePose.Pose)
                'For Each CT As CodedTarget In ISI.Targets.lstCT
                '    AllRow = BTuple.TupleConcat(AllRow, CT.CT_Points.Row)
                '    AllCol = BTuple.TupleConcat(AllCol, CT.CT_Points.Col)
                '    For i = 0 To CodedTarget.CTnoSTnum - 1
                '        All3DPointsIndex = BTuple.TupleConcat(All3DPointsIndex, (i + 1) * 10000 + CT.CT_ID)
                '        AllCams = BTuple.TupleConcat(AllCams, ISI.ImageId - 1)
                '    Next
                'Next
            Else
                ISI.flgConnected = ISI.flgConnected
            End If
        Next
        Dim cnt As Integer = 0
        For Each C3DCT As Common3DCodedTarget In lstCommon3dCT

            For Each CT As CodedTarget In C3DCT.lstCT

                If lstImages.Item(CT.ImageID - 1).flgConnected = True Then

                    For i = 0 To CodedTarget.CTnoSTnum - 1
                        'AllRow = BTuple.TupleConcat(AllRow, CT.CT_Points.Row(i))
                        'AllCol = BTuple.TupleConcat(AllCol, CT.CT_Points.Col(i))
                        'All3DPointsIndex = BTuple.TupleConcat(All3DPointsIndex, (i + 1) * 10000 + CT.CT_ID)
                        'AllCams = BTuple.TupleConcat(AllCams, CT.ImageID - 1)
                        AllRow(cnt) = CT.CT_Points.Row(i)
                        AllCol(cnt) = CT.CT_Points.Col(i)
                        All3DPointsIndex(cnt) = (i + 1) * 10000 + CT.CT_ID
                        AllCams(cnt) = CT.ImageID - 1
                        cnt += 1
                    Next
                End If
            Next

        Next
        ReDim Preserve AllRow(cnt - 1)
        ReDim Preserve AllCol(cnt - 1)
        ReDim Preserve All3DPointsIndex(cnt - 1)
        ReDim Preserve AllCams(cnt - 1)

        Dim StereoModelID As Object = Nothing
        HOperatorSet.CreateStereoModel(CamSetupModelID, "points_3d", Nothing, Nothing, StereoModelID)
        Dim X As Object = Nothing
        Dim Y As Object = Nothing
        Dim Z As Object = Nothing
        Dim Pindex As Object = Nothing
        Try
            HOperatorSet.ReconstructPointsStereo(StereoModelID, AllRow, AllCol, Nothing, AllCams, All3DPointsIndex, X, Y, Z, Nothing, Pindex)
        Catch ex As Exception
            HOperatorSet.ClearCameraSetupModel(CamSetupModelID)
            HOperatorSet.ClearStereoModel(StereoModelID)
            Exit Function
        End Try

        If Pindex Is Nothing Then
            HOperatorSet.ClearCameraSetupModel(CamSetupModelID)
            HOperatorSet.ClearStereoModel(StereoModelID)
            Exit Function
        End If
        For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
            If C3DCT.lstP3d.Count = 0 Then
                C3DCT.PID = C3DCT.PID
            End If
            If C3DCT.PID = 272 Then
                C3DCT.PID = C3DCT.PID
            End If
            For i = 0 To CodedTarget.CTnoSTnum - 1
                Dim T As Object = BTuple.TupleFind(Pindex, (i + 1) * 10000 + C3DCT.PID)
                If BTuple.TupleSelect(T, 0) <> -1 Then
                    If C3DCT.PID = 223 Then
                        C3DCT.PID = C3DCT.PID
                    End If
                    If C3DCT.lstP3d.Count = CodedTarget.CTnoSTnum Then
                        C3DCT.lstP3d.Item(i).X = BTuple.TupleSelect(X, T)
                        C3DCT.lstP3d.Item(i).Y = BTuple.TupleSelect(Y, T)
                        C3DCT.lstP3d.Item(i).Z = BTuple.TupleSelect(Z, T)
                    Else
                        Dim tmp3dP As New Point3D
                        tmp3dP.X = BTuple.TupleSelect(X, T)
                        tmp3dP.Y = BTuple.TupleSelect(Y, T)
                        tmp3dP.Z = BTuple.TupleSelect(Z, T)
                        C3DCT.lstP3d.Add(tmp3dP)
                    End If
                    C3DCT.flgUsable = True
                End If
            Next
        Next

        HOperatorSet.ClearCameraSetupModel(CamSetupModelID)
        HOperatorSet.ClearStereoModel(StereoModelID)
        CalcALLCodedTarget3dCoordByHalcon = True
    End Function

    '3次元計測点を2次元計測点に投影するMatrix（逆行列）
    Private Sub CalcAllImagesInvertHomMat()
        TimeMonStart()

        Dim hvHommat3d As Object = Nothing
        For Each ISI As ImageSet In lstImages
            ISI.CalcInvertHomMat()
        Next

        TimeMonEnd()
    End Sub
    Private Function CalcReProjectionErrorOnePoint(ByRef C3DST As Common3DSingleTarget) As Boolean

        CalcReProjectionErrorOnePoint = True
        Dim P3d As Point3D = C3DST.P3d
        Dim P3dTrans As New Point3D
        Dim tmpP2d As New ImagePoints
        Dim maxReProjErr As Double = Double.MinValue
        For Each ST As SingleTarget In C3DST.lstST
            Try
                HOperatorSet.AffineTransPoint3D(lstImages(ST.ImageID - 1).HomMatInvert, P3d.X, P3d.Y, P3d.Z, P3dTrans.X, P3dTrans.Y, P3dTrans.Z)
            Catch ex As Exception
                Continue For
                'CalcReProjectionErrorOnePoint = False
                'Exit For
            End Try
            Dim tmpP3d As New Point3D
            Dim CameraKaranoKyori As Double
            tmpP3d.X = 0
            tmpP3d.Y = 0
            tmpP3d.Z = 0
            tmpP3d.GetDisttoOtherPose(P3dTrans, CameraKaranoKyori)
            If CameraKaranoKyori / ScaleMM > 20000 Then
                CalcReProjectionErrorOnePoint = False
                Exit For
            End If
            Try

                '150115 SUURI Rep Sta 複数カメラ内部パラメータ取扱機能--------------
                'HOperatorSet.Project3DPoint(P3dTrans.X, P3dTrans.Y, P3dTrans.Z, hv_CamparamOut, tmpP2d.Row, tmpP2d.Col)
                HOperatorSet.Project3DPoint(P3dTrans.X, P3dTrans.Y, P3dTrans.Z, lstImages(ST.ImageID - 1).objCamparam.Camparam, tmpP2d.Row, tmpP2d.Col)
                '150115 SUURI Rep Sta 複数カメラ内部パラメータ取扱機能--------------

            Catch ex As Exception
                CalcReProjectionErrorOnePoint = False
                Exit For
            End Try

            HOperatorSet.DistancePp(ST.P2D.Row, ST.P2D.Col, tmpP2d.Row, tmpP2d.Col, ST.ReProjectionError)
            If maxReProjErr < ST.ReProjectionError.D Then
                maxReProjErr = ST.ReProjectionError.D
                C3DST.maxProjErrST = ST
            End If
        Next

    End Function
    '再投影誤差を求める
    Private Sub CalcReProjectionError()
        TimeMonStart()

        Dim i As Integer = 0
        Dim P3d As Point3D
        Dim P3dTrans As New Point3D
        Dim tmpP2d As New ImagePoints
        Dim sumErrors As Object = Nothing
        Trace.WriteLine("Start")
        GC.Collect()
        GC.WaitForPendingFinalizers()
        Dim strMaxErrorPointsInfo As String = ""
        Dim tmpMax As Double = Double.Epsilon
        For Each C3DST As Common3DSingleTarget In lstCommon3dST
            If C3DST.PID <> -1 Then
                P3d = C3DST.P3d
                For Each ST As SingleTarget In C3DST.lstST
                    HOperatorSet.AffineTransPoint3D(lstImages(ST.ImageID - 1).HomMatInvert, P3d.X, P3d.Y, P3d.Z, P3dTrans.X, P3dTrans.Y, P3dTrans.Z)
                    
					'150115 SUURI Rep Sta 複数カメラ内部パラメータ取扱機能--------------
                    'HOperatorSet.Project3DPoint(P3dTrans.X, P3dTrans.Y, P3dTrans.Z, hv_CamparamOut, tmpP2d.Row, tmpP2d.Col)
                    HOperatorSet.Project3DPoint(P3dTrans.X, P3dTrans.Y, P3dTrans.Z, lstImages(ST.ImageID - 1).objCamparam.Camparam, tmpP2d.Row, tmpP2d.Col)
					'150115 SUURI Rep End 複数カメラ内部パラメータ取扱機能--------------

                    HOperatorSet.DistancePp(ST.P2D.Row, ST.P2D.Col, tmpP2d.Row, tmpP2d.Col, ST.ReProjectionError)
                    sumErrors = BTuple.TupleConcat(sumErrors, ST.ReProjectionError)
                    If ST.ReProjectionError > tmpMax Then
                        strMaxErrorPointsInfo = strMaxErrorPointsInfo & "singletarget P3ID : " & ST.P3ID & " P2ID : " & ST.P2ID & " ImageID : " & ST.ImageID & " ReProjError : " & ST.ReProjectionError.D & vbNewLine
                        tmpMax = ST.ReProjectionError.D
                    End If
                Next
            End If
        Next
        For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
            For Each CT As CodedTarget In C3DCT.lstCT
                i = 0
                For Each ST As SingleTarget In CT.lstCTtoST
                    If lstImages(ST.ImageID - 1).flgConnected = True Then
                        Try
                            P3d = C3DCT.lstP3d(i)
                            HOperatorSet.AffineTransPoint3d(lstImages(ST.ImageID - 1).HomMatInvert, New HTuple(P3d.X), New HTuple(P3d.Y), New HTuple(P3d.Z), P3dTrans.X, P3dTrans.Y, P3dTrans.Z)
                        Catch ex As Exception
                            Continue For
                        End Try


                        '3Dポイント（x , y , z）⇒画像平面 (ピクセル： row , column ) へ投影 
                        '150115 SUURI Rep Sta 複数カメラ内部パラメータ取扱機能--------------
                        'HOperatorSet.Project3DPoint(P3dTrans.X, P3dTrans.Y, P3dTrans.Z, hv_CamparamOut, tmpP2d.Row, tmpP2d.Col)
                        HOperatorSet.Project3DPoint(P3dTrans.X, P3dTrans.Y, P3dTrans.Z, lstImages(ST.ImageID - 1).objCamparam.Camparam, tmpP2d.Row, tmpP2d.Col)
						'150115 SUURI Rep End 複数カメラ内部パラメータ取扱機能--------------

                        '2点間の距離を計算する。
                        '2点の（row , column) ⇒ 距離（ST.ReProjectionError） ； 再投影誤差
                        HOperatorSet.DistancePp(ST.P2D.Row, ST.P2D.Col, tmpP2d.Row, tmpP2d.Col, ST.ReProjectionError)
                        sumErrors = BTuple.TupleConcat(sumErrors, ST.ReProjectionError)
                        If ST.ReProjectionError > tmpMax Then
                            tmpMax = ST.ReProjectionError.D
                            strMaxErrorPointsInfo = strMaxErrorPointsInfo & "CodedTarget P3ID : " & ST.P3ID & " P2ID : " & ST.P2ID & " ImageID : " & ST.ImageID & " C3DCT.ID : " & C3DCT.PID & " ReProjError : " & ST.ReProjectionError.D & vbNewLine
                        End If


                        i += 1
                    End If
                Next
            Next
        Next
#If DEBUG Then
        'ここでメモリ不足が生じている！！！20120625　SUURI -> 同じデータを二回実行した場合になる。
        Trace.WriteLine("誤差合計：" & BTuple.TupleSum(sumErrors).D)
        Trace.WriteLine("誤差平均：" & BTuple.TupleMean(sumErrors).D)
        Trace.WriteLine("誤差バラ：" & BTuple.TupleDeviation(sumErrors).D)
        Trace.WriteLine("誤差最大：" & BTuple.TupleMax(sumErrors).D)
        Trace.WriteLine(strMaxErrorPointsInfo)

        Trace.WriteLine("END")
#End If

        TimeMonEnd()

    End Sub
    Public Sub CalcReProjectionErrorOneImage(ByRef ISI As ImageSet)
        Dim i As Integer = 0

        Dim sumErrors As Object = Nothing
        Trace.WriteLine("Start")
        GC.Collect()
        GC.WaitForPendingFinalizers()
        Dim HomMat As Object = Nothing
        HOperatorSet.PoseToHomMat3d(ISI.ImagePose.Pose, HomMat)
        HOperatorSet.HomMat3dInvert(HomMat, HomMat)
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\CTMonitor.txt", "START" & vbNewLine, False)

        For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
            If C3DCT.lstP3d.Count = 0 Then
                Continue For
            End If
            For Each CT As CodedTarget In C3DCT.lstCT
                If CT.ImageID = ISI.ImageId Then
                    i = 0
                    Dim P3d As New Point3D
                    Dim P3dTrans As New Point3D
                    Dim tmpP2d As New ImagePoints
                    Dim tmpP2d2 As New ImagePoints

                    For Each ST As SingleTarget In CT.lstCTtoST
                        P3d.ConcatToMe(C3DCT.lstP3d(i))
                        tmpP2d.ConcatToMe(ST.P2D)
                        i += 1
                    Next

                    HOperatorSet.AffineTransPoint3D(HomMat, P3d.X, P3d.Y, P3d.Z, P3dTrans.X, P3dTrans.Y, P3dTrans.Z)
                    HOperatorSet.Project3DPoint(P3dTrans.X, P3dTrans.Y, P3dTrans.Z, hv_CamparamOut, tmpP2d2.Row, tmpP2d2.Col)
                    HOperatorSet.DistancePp(tmpP2d.Row, tmpP2d.Col, tmpP2d2.Row, tmpP2d2.Col, sumErrors)
                    If BTuple.TupleMean(sumErrors) > 10 Then
                        i = 0
                        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\CTMonitor.txt", "ImageID:" & ISI.ImageId & "  ImageName:" & ISI.ImageName & "  CT_ID:" & CT.CT_ID & "  MeanError:" & BTuple.TupleMean(sumErrors).D & vbNewLine, True)
                    End If
                    Exit For
                End If
            Next
        Next

    End Sub

    Private Sub SueokiBadCTremove()

        Dim c3dct_num As Integer = lstCommon3dCT.Count
        Dim minError As Double = 20
        For i As Integer = c3dct_num - 1 To 0 Step -1
            Dim C3DCT As Common3DCodedTarget = lstCommon3dCT.Item(i)

            'For Each C3DCT As Common3DCodedTarget In lstCommon3dCT

            If C3DCT.lstP3d.Count = 0 Or C3DCT.lstCT.Count = 0 Then
                Continue For
            End If
            If C3DCT.PID = 406 Then
                C3DCT.PID = C3DCT.PID
            End If

            Do
                If C3DCT.lstCT.Count < 2 Then
                    If C3DCT.lstCT.Count = 1 Then
                        lstCommon3dCT.RemoveAt(i)
                        Exit Do
                    End If
                End If

                C3DCT.Calc3dPoints()
                Dim tmpLstCommon3dCT As New List(Of Common3DCodedTarget)

                Dim hikakuDist As Double = BTuple.TupleMax(C3DCT.AllDist).D

                For Each CT As CodedTarget In C3DCT.lstCT
                    Dim tmpC3DCT As New Common3DCodedTarget
                    For Each CTother As CodedTarget In C3DCT.lstCT
                        If CT.ImageID = CTother.ImageID And CT.CT_ID = CTother.CT_ID And CT.lstCTtoST(0).P2ID = CTother.lstCTtoST(0).P2ID And _
                             BTuple.TupleEqual(CT.lstCTtoST(0).P2D.Row, CTother.lstCTtoST(0).P2D.Row).I = 1 Then
                        Else
                            tmpC3DCT.lstCT.Add(CTother)
                        End If
                    Next
                    tmpC3DCT.Calc3dPoints()
                    tmpLstCommon3dCT.Add(tmpC3DCT)

                    'For Each C3DCTother As Common3DCodedTarget In lstCommon3dCT
                    '    If C3DCTother.lstP3d.Count = 0 Then
                    '        Continue For
                    '    End If
                    '    For Each CTother As CodedTarget In C3DCTother.lstCT
                    '    Next
                    'Next
                Next
                Dim icnt As Integer = 0
                Dim n As Integer = tmpLstCommon3dCT.Count
                Dim minDist As Double = Double.MaxValue
                Dim minI As Integer = 0
                For Each tmpCT As Common3DCodedTarget In tmpLstCommon3dCT
                    If tmpCT.flgUsable = True Then
                        If BTuple.TupleMean(tmpCT.AllDist) > 0.0001 Then
                            If BTuple.TupleMean(tmpCT.AllDist) < minDist Then
                                minDist = BTuple.TupleMean(tmpCT.AllDist)
                                minI = icnt
                            End If
                        End If
                    End If
                    icnt += 1
                Next
              
                If hikakuDist > minError Then
                    icnt = 0
                    Dim delID As Integer = -1
                    For Each CT As CodedTarget In lstImages.Item(C3DCT.lstCT(minI).ImageID - 1).Targets.lstCT
                        If BTuple.TupleEqual(CT.CenterPoint.Row, C3DCT.lstCT(minI).CenterPoint.Row).I = 1 And
                            BTuple.TupleEqual(CT.CenterPoint.Col, C3DCT.lstCT(minI).CenterPoint.Col).I = 1 Then
                            delID = icnt
                            Exit For
                        End If
                        icnt += 1
                    Next
                    lstImages.Item(C3DCT.lstCT(minI).ImageID - 1).Targets.lstCT.RemoveAt(delID)

                    C3DCT.lstCT.RemoveAt(minI)
                    MsgBox(" suuri debug " & hikakuDist & " " & minError)
                End If
                C3DCT.Calc3dPoints()
                If BTuple.TupleMax(C3DCT.AllDist) > minError Then
                Else
                    Exit Do
                End If
            Loop
        Next




    End Sub
    Private Sub CalcReProjectionErrorOneCT(ByRef ISI As ImageSet)
        Dim i As Integer = 0


        Dim sumErrors As Object = Nothing
        Trace.WriteLine("Start")
        GC.Collect()
        GC.WaitForPendingFinalizers()
        Dim HomMat As New Object
        HOperatorSet.PoseToHomMat3d(ISI.ImagePose.Pose, HomMat)
        HOperatorSet.HomMat3dInvert(HomMat, HomMat)
        Dim lstBadCT_ID As New List(Of Integer)

        For Each MyCT As CodedTarget In ISI.Targets.lstCT
            Dim P3d As New Point3D
            Dim P3dTrans As New Point3D
            Dim tmpP2d As New ImagePoints
            Dim tmpP2d2 As New ImagePoints

            If MyCT.CT_ID = 9 Then
                MyCT.CT_ID = MyCT.CT_ID
            End If

            For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
                If C3DCT.lstP3d.Count = 0 Then
                    Continue For
                End If
                If C3DCT.PID = MyCT.CT_ID Then

                    i = 0
                    For Each ST As SingleTarget In MyCT.lstCTtoST
                        P3d.ConcatToMe(C3DCT.lstP3d(i))
                        tmpP2d.ConcatToMe(ST.P2D)
                        i += 1
                    Next

                    Exit For
                End If

            Next

            HOperatorSet.AffineTransPoint3D(HomMat, P3d.X, P3d.Y, P3d.Z, P3dTrans.X, P3dTrans.Y, P3dTrans.Z)
            HOperatorSet.Project3DPoint(P3dTrans.X, P3dTrans.Y, P3dTrans.Z, hv_CamparamOut, tmpP2d2.Row, tmpP2d2.Col)
            HOperatorSet.DistancePp(tmpP2d.Row, tmpP2d.Col, tmpP2d2.Row, tmpP2d2.Col, sumErrors)
            If MyCT.CT_ID = 9 Then
                MyCT.CT_ID = MyCT.CT_ID

            End If
            If sumErrors Is Nothing Then
                Continue For
            End If
            If BTuple.TupleMean(sumErrors) > 10 Then
                i = 0
                If MyCT.CT_ID = 9 Then
                    MyCT.CT_ID = MyCT.CT_ID
                End If
                '                My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & ISI.ImageId & "_CTMonitor.txt", "ImageID:" & ISI.ImageId & "  ImageName:" & ISI.ImageName & "  CT_ID:" & _
                'MyCT.CT_ID & "  MeanError:" & BTuple.TupleMean(sumErrors) & vbNewLine, True)
                lstBadCT_ID.Add(MyCT.CT_ID)

            End If

        Next

        For Each BadCT_ID As Integer In lstBadCT_ID
            If BadCT_ID = 9 Then
                BadCT_ID = BadCT_ID
            End If
            For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
                If C3DCT.PID = BadCT_ID Then
                    Dim n As Integer = C3DCT.lstCT.Count
                    If BTuple.TupleLength(C3DCT.AllDist) = 2 Then
                        For i = n - 1 To 0 Step -1
                            Dim CT As CodedTarget
                            CT = C3DCT.lstCT.Item(i)
                            If lstImages.Item(CT.ImageID - 1).flgConnected = True Then
                                C3DCT.lstCT.RemoveAt(i)
                            End If
                        Next
                    Else
                        For i = n - 1 To 0 Step -1
                            Dim CT As CodedTarget
                            CT = C3DCT.lstCT.Item(i)
                            If lstImages.Item(CT.ImageID - 1).flgConnected = True Then
                                If CT.lstCTtoST.Item(CodedTarget.CTnoSTnum - 1).Dist >= C3DCT.meanerror + C3DCT.deverror Then
                                    C3DCT.lstCT.RemoveAt(i)
                                End If
                            End If
                        Next
                    End If


                End If
            Next
        Next

    End Sub


    Private Sub CalcReProjectionErrorOneCT_faster(ByRef ISI As ImageSet)
        Dim i As Integer = 0
        Trace.WriteLine("Start " & ISI.ImageId & "  " & ISI.ImageName)
        Dim HomMat As Object = Nothing
        HOperatorSet.PoseToHomMat3d(ISI.ImagePose.Pose, HomMat)
        HOperatorSet.HomMat3dInvert(HomMat, HomMat)

        For Each MyCT As CodedTarget In ISI.Targets.lstCT
            
            Dim P3d As New Point3D
            Dim P3dTrans As New Point3D
            Dim tmpP2d As New ImagePoints
            Dim tmpP2d2 As New ImagePoints
            Dim sumErrors As Object = Nothing
            If MyCT.myC3DCT.flgUsable = False Then
                MyCT.myC3DCT.Calc3dPoints()
                '  Continue For
            End If
            If MyCT.myC3DCT.lstP3d.Count > 0 Then
                i = 0
                For Each ST As SingleTarget In MyCT.lstCTtoST
                    P3d.ConcatToMe(MyCT.myC3DCT.lstP3d(i))
                    tmpP2d.ConcatToMe(ST.P2D)
                    i += 1
                Next

                HOperatorSet.AffineTransPoint3D(HomMat, P3d.X, P3d.Y, P3d.Z, P3dTrans.X, P3dTrans.Y, P3dTrans.Z)

                '150115 SUURI Rep Sta 複数カメラ内部パラメータ取扱機能--------------
                'HOperatorSet.Project3DPoint(P3dTrans.X, P3dTrans.Y, P3dTrans.Z, hv_CamparamOut, tmpP2d2.Row, tmpP2d2.Col)
                HOperatorSet.Project3DPoint(P3dTrans.X, P3dTrans.Y, P3dTrans.Z, ISI.objCamparam.Camparam, tmpP2d2.Row, tmpP2d2.Col)
                '150115 SUURI Rep End 複数カメラ内部パラメータ取扱機能--------------

                HOperatorSet.DistancePp(tmpP2d.Row, tmpP2d.Col, tmpP2d2.Row, tmpP2d2.Col, sumErrors)
                If sumErrors Is Nothing Then
                    Continue For
                End If
                If MyCT.CT_ID = 481 And MyCT.ImageID = 42 Then
                    MyCT.CT_ID = MyCT.CT_ID
                End If
                'Rep By Suuri Sta 20140826
                ' If BTuple.TupleMean(sumErrors) > 40 Then
                If BTuple.TupleMean(sumErrors) > 20 Then
                    'Rep By Suuri End 20140826
                    Dim n As Integer = MyCT.myC3DCT.lstCT.Count
                    If BTuple.TupleLength(MyCT.myC3DCT.AllDist).I = 2 Then
                        For i = n - 1 To 0 Step -1
                            Dim CT As CodedTarget
                            CT = MyCT.myC3DCT.lstCT.Item(i)
                            If lstImages.Item(CT.ImageID - 1).flgConnected = True Then
                                MyCT.myC3DCT.lstCT.RemoveAt(i)
                            End If
                        Next
                    Else
                        For i = n - 1 To 0 Step -1
                            Dim CT As CodedTarget
                            CT = MyCT.myC3DCT.lstCT.Item(i)
                            If lstImages.Item(CT.ImageID - 1).flgConnected = True Then
                                'Rep By Suuri Sta 20140826
                                ' If CT.lstCTtoST.Item(CodedTarget.CTnoSTnum - 1).Dist >= MyCT.myC3DCT.meanerror + MyCT.myC3DCT.deverror * 3 Then
                                If CT.lstCTtoST.Item(CodedTarget.CTnoSTnum - 1).Dist >= MyCT.myC3DCT.meanerror.D + MyCT.myC3DCT.deverror.D * 2 Then
                                    'Rep By Suuri Sta 20140826
                                    MyCT.myC3DCT.lstCT.RemoveAt(i)
                                End If
                            End If
                        Next
                    End If
                End If
            End If
        Next

    End Sub

    Private Sub CalcReProjectionError_Memory()
        Dim i As Integer = 0
        Dim P3d As New Point3D
        Dim P3dTrans As New Point3D
        Dim tmpP2d As New ImagePoints
        Dim tmpP2d2 As New ImagePoints


        Dim sumErrors As Object = Nothing
        Trace.WriteLine("Start")
        GC.Collect()
        GC.WaitForPendingFinalizers()

        For Each ISI As ImageSet In lstImages
            If ISI.flgConnected = True Then
                Dim lstTempST As New List(Of SingleTarget)
                For Each C3DST As Common3DSingleTarget In lstCommon3dST
                    If C3DST.PID <> -1 Then
                        For Each ST As SingleTarget In C3DST.lstST
                            If ST.ImageID = ISI.ImageId Then
                                P3d.ConcatToMe(C3DST.P3d)
                                tmpP2d.ConcatToMe(ST.P2D)
                                lstTempST.Add(ST)
                                Exit For
                            End If
                        Next
                    End If
                Next

                For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
                    If C3DCT.lstP3d.Count = 0 Then
                        Continue For

                    End If
                    For Each CT As CodedTarget In C3DCT.lstCT
                        If CT.ImageID = ISI.ImageId Then
                            i = 0
                            For Each ST As SingleTarget In CT.lstCTtoST
                                P3d.ConcatToMe(C3DCT.lstP3d(i))
                                tmpP2d.ConcatToMe(ST.P2D)
                                lstTempST.Add(ST)
                                i += 1
                            Next
                            Exit For
                        End If
                    Next
                Next
                HOperatorSet.AffineTransPoint3D(ISI.HomMatInvert, P3d.X, P3d.Y, P3d.Z, P3dTrans.X, P3dTrans.Y, P3dTrans.Z)
                HOperatorSet.Project3DPoint(P3dTrans.X, P3dTrans.Y, P3dTrans.Z, hv_CamparamOut, tmpP2d2.Row, tmpP2d2.Col)
                HOperatorSet.DistancePp(tmpP2d.Row, tmpP2d.Col, tmpP2d2.Row, tmpP2d2.Col, sumErrors)

                i = 0
                For Each ST As SingleTarget In lstTempST
                    ST.ReProjectionError = sumErrors(i)
                    i += 1
                Next
            End If
        Next

        'ここでメモリ不足が生じている！！！20120625　SUURI
        Trace.WriteLine("誤差合計：" & BTuple.TupleSum(sumErrors))
        Trace.WriteLine("誤差平均：" & BTuple.TupleMean(sumErrors))
        Trace.WriteLine("誤差バラ：" & BTuple.TupleDeviation(sumErrors))
        Trace.WriteLine("誤差最大：" & BTuple.TupleMax(sumErrors))
        Trace.WriteLine("END")
    End Sub

    Private Sub SingleTargetNumbering(ByRef tt(,) As ImagePairSet)
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim t As Integer = 0
        Dim n As Integer = lstImages.Count
        Dim currentIPS As ImagePairSet
        Dim otherIPS As ImagePairSet
        Dim midIPS As ImagePairSet
        For i = 0 To n - 2
            For j = i + 1 To n - 1
                currentIPS = tt(i, j)
                If currentIPS.cntComCT >= intCommonCTnum And currentIPS.IS1.Targets.lstST.Count > 0 And currentIPS.IS2.Targets.lstST.Count > 0 Then
                    currentIPS.gen_epilines(hv_Width, hv_Height)
                    currentIPS.CalcTaiouTenIndex()
                End If
            Next
        Next
        For i = 0 To n - 3
            For j = i + 1 To n - 2
                currentIPS = tt(i, j)
                If currentIPS.cntComCT >= intCommonCTnum And currentIPS.IS1.Targets.lstST.Count > 0 And currentIPS.IS2.Targets.lstST.Count > 0 Then
                    For t = j + 1 To n - 1
                        otherIPS = tt(j, t)
                        If otherIPS.cntComCT >= intCommonCTnum And otherIPS.IS1.Targets.lstST.Count > 0 And otherIPS.IS2.Targets.lstST.Count > 0 Then
                            'If currentIPS.GetCommonCTcount(otherIPS) >= intCommonCTnum Then
                            midIPS = tt(i, t)
                            If midIPS.cntComCT < intCommonCTnum Or midIPS.IS2.Targets.lstST.Count = 0 Or midIPS.IS1.Targets.lstST.Count = 0 Then
                                Continue For
                            End If
                            CalcSingleTarget(currentIPS, otherIPS, midIPS)
                        End If
                    Next t
                End If
            Next j
        Next i


    End Sub
    ''' 

    ''' ST1の属している共通３次元ターゲットにST2を追加する。
    ''' 

    ''' 
    ''' 
    ''' 
    Private Sub AddtoCommon3DSingleTargets(ByRef ST1 As SingleTarget, ByRef ST2 As SingleTarget)
        If lstCommon3dST.Item(ST1.P3ID - 1).PID <> -1 Then
            ST2.flgUsed = 1
            ST2.P3ID = ST1.P3ID
            lstCommon3dST.Item(ST1.P3ID - 1).lstST.Add(ST2)

            'lstCommon3dST.Item(ST1.P3ID - 1).Calc3dPoints()
            '' If lstCommon3dST.Item(ST1.P3ID - 1).lstST.Count = 2 Then
            'Dim dist As Double
            'Dim minDist As Double = Double.MaxValue
            'Dim minDistIndex As Integer = -1
            'For Each C3DST As Common3DSingleTarget In lstCommon3dST
            '    If C3DST.PID <> lstCommon3dST.Item(ST1.P3ID - 1).PID And C3DST.PID <> -1 Then
            '        If C3DST.P3d.X Is Nothing Then
            '            Continue For
            '        End If
            '        C3DST.P3d.GetDisttoOtherPose(lstCommon3dST.Item(ST1.P3ID - 1).P3d, dist)
            '        If minDist > dist Then
            '            minDist = dist
            '            minDistIndex = C3DST.PID - 1
            '        End If
            '    End If
            'Next
            'If minDist < 0.01 Then
            '    For Each ST As SingleTarget In lstCommon3dST.Item(ST1.P3ID - 1).lstST
            '        ST.P3ID = lstCommon3dST.Item(minDistIndex).PID
            '        lstCommon3dST.Item(minDistIndex).lstST.Add(ST)
            '    Next
            '    lstCommon3dST.Item(ST1.P3ID - 1).PID = -1
            '    'lstCommon3dST.RemoveAt(ST1.P3ID - 1)
            'End If
            '' End If

        End If
    End Sub
    Private Sub SingelTargetNumberingOther(ByRef tt(,) As ImagePairSet)
        TimeMonStart()

        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim n As Integer = lstImages.Count
        Dim IPS1 As ImagePairSet
        '  Dim IPS2 As ImagePairSet
        Dim ST2 As SingleTarget
        Dim IndexN1 As Object = Nothing
        Dim Index1N As Object = Nothing
        '20120315
        For Each ISI As ImageSet In lstImages
            '150115 SUURI Rep Sta 複数カメラ内部パラメータ取扱機能--------------
            'ISI.ChangeRadialDistortionP2D(hv_CamparamOut, hv_CamparamZero)
            ISI.ChangeRadialDistortionP2D(ISI.objCamparam.Camparam, ISI.objCamparam.CamParamZero)
            '150115 SUURI Rep End 複数カメラ内部パラメータ取扱機能--------------
        Next
        For i = 0 To n - 1
            For j = i To n - 1
                If i <> j Then
                    IPS1 = tt(j, i)
                    If IPS1.cntComCT >= intCommonCTnum And
                       IPS1.IS1.Targets.lstST.Count > 0 And
                       IPS1.IS2.Targets.lstST.Count > 0 And
                       IPS1.IS1.flgConnected = True And
                       IPS1.IS2.flgConnected = True Then 'PairSetが有効な共通点を持ち、両画像が繋がっていてかつシングルターゲットを１つ以上もっている必要がある。
                        If IPS1.CalcFmatTransed(hv_CamparamZero) Then
                            IPS1.gen_epilines(hv_Width, hv_Height)
                            For Each ST1 As SingleTarget In lstImages(i).Targets.lstST
                                IPS1.GetEpilineIndexNearestThisPoint(ST1, IndexN1)
                                If BTuple.TupleSelect(IndexN1, 0).I <> -1 Then
                                    Dim k As Integer
                                    For k = 0 To BTuple.TupleLength(IndexN1).I - 1
                                        ST2 = IPS1.IS1.Targets.lstST.Item(BTuple.TupleSelect(IndexN1, k).I)
                                        ST1.tmpPPP.Add(ST2)
                                        ST2.tmpPPP.Add(ST1)
                                    Next
                                End If
                            Next
                        End If
                    End If
                End If
            Next
        Next

        'For Each ISI As ImageSet In lstImages
        '    If ISI.flgConnected = True Then
        '        For Each ST As SingleTarget In ISI.Targets.lstST
        '            Dim flgSamePointAri As Boolean = False
        '            For Each C3DST As Common3DSingleTarget In lstCommon3dST
        '                If C3DST.SamePointAri(ST) = True Then
        '                    flgSamePointAri = True
        '                    Exit For
        '                End If
        '            Next
        '            If flgSamePointAri = False And ST.tmpPPP.Count > 0 Then
        '                Dim obj3dTargets As New Common3DSingleTarget
        '                obj3dTargets.PID = lstCommon3dST.Count + 1
        '                ' ST.P3ID = obj3dTargets.PID
        '                For Each STo As SingleTarget In ST.tmpPPP
        '                    If obj3dTargets.SamePointAri(STo) = False Then
        '                        obj3dTargets.lstST.Add(STo)
        '                    End If
        '                Next


        '                lstCommon3dST.Add(obj3dTargets)
        '                ' ReCallSubCollectAllST(obj3dTargets, ST)
        '            End If
        '        Next
        '    End If
        'Next
        For Each ISI As ImageSet In lstImages
            If ISI.flgConnected = True Then
                For Each ST As SingleTarget In ISI.Targets.lstST
                    If ST.tmpPPP.Count > 0 Then
                        Dim flgIsInputOk As Boolean = True
                        For Each C3DST As Common3DSingleTarget In lstCommon3dST
                            'Dim flgDoubleCount As Integer = 0
                            'If C3DST.SamePointAri(ST) Then
                            '    flgDoubleCount += 1
                            '    For Each STo As SingleTarget In ST.tmpPPP
                            '        If C3DST.SamePointAri(STo) = True Then
                            '            flgDoubleCount += 1
                            '        End If
                            '    Next
                            'End If
                            'If flgDoubleCount = ST.tmpPPP.Count + 1 Then
                            '    flgIsInputOk = False
                            '    Exit For
                            'End If
                            If C3DST.SamePointAri(ST) Then
                                flgIsInputOk = False
                                Exit For
                            End If
                            If C3DST.PID = 82 Then
                                flgIsInputOk = flgIsInputOk
                            End If
                        Next
                        If flgIsInputOk = True Then
                            Dim obj3dTargets As New Common3DSingleTarget
                            obj3dTargets.PID = lstCommon3dST.Count + 1
                            obj3dTargets.lstST.Add(ST)
                            For Each STo As SingleTarget In ST.tmpPPP
                                If obj3dTargets.SamePointAri(STo) = False Then
                                    '  If obj3dTargets.SameImagePointAri(STo) = False Then
                                    obj3dTargets.lstST.Add(STo)
                                    'End If
                                Else
                                    ' Stop
                                End If
                            Next
                            lstCommon3dST.Add(obj3dTargets)
                        End If
                    End If
                Next
            End If
        Next
        'Dim TTt As Integer = 0
        'For Each C3DST As Common3DSingleTarget In lstCommon3dST
        '    TTt += C3DST.lstST.Count
        '    If C3DST.lstST.Count < MinCountOfRays Then
        '    Else
        '        C3DST.Calc3dPoints()

        '    End If
        'Next
        'TTt = TTt / lstCommon3dST.Count

        TimeMonEnd()
    End Sub

    Private Sub ReCallSubCollectAllST(ByRef obj3dTargets As Common3DSingleTarget, ByRef ST As SingleTarget)
        For Each STo As SingleTarget In ST.tmpPPP
            Dim flgSamePointAri As Boolean = False
            If obj3dTargets.SamePointAri(STo) = False Then
                obj3dTargets.lstST.Add(STo)
            End If
            ReCallSubCollectAllST(obj3dTargets, STo)
        Next
    End Sub

    Private Sub SingleTargetNumbering4(ByRef tt(,) As ImagePairSet)
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim n As Integer = lstImages.Count
        Dim IPS As ImagePairSet
        'Dim otherIPS As ImagePairSet
        Dim ST2 As SingleTarget
        Dim hv_ind_dist1N As Object = Nothing
        For i = 0 To n - 1
            For j = i To n - 1
                If i <> j Then
                    IPS = tt(i, j)
                    If IPS.cntComCT >= intCommonCTnum And IPS.IS1.Targets.lstST.Count > 0 And IPS.IS2.Targets.lstST.Count > 0 Then
                        IPS.gen_epilines(hv_Width, hv_Height)
                        For Each ST1 As SingleTarget In IPS.IS1.Targets.lstST
                            ' getST_forEpiline(currentIPS, ST1, hv_ind_dist1N)
                            IPS.getST_forEpiline(ST1.P2ID - 1, hv_ind_dist1N)
                            If BTuple.TupleSelect(hv_ind_dist1N, 0) <> -1 Then
                                ST2 = IPS.IS2.Targets.lstST.Item(hv_ind_dist1N)
                                ST1.tmpPPP.Add(ST2)
                                'If ST1.flgUsed = 0 And ST2.flgUsed = 0 Then
                                '    Dim obj3dTargets As New Common3DSingleTarget
                                '    obj3dTargets.PID = lstCommon3dST.Count + 1
                                '    ST1.flgUsed = 1
                                '    ST1.P3ID = obj3dTargets.PID
                                '    obj3dTargets.lstST.Add(ST1)
                                '    lstCommon3dST.Add(obj3dTargets)
                                '    AddtoCommon3DSingleTargets(ST1, ST2)
                                'ElseIf ST1.flgUsed = 1 And ST2.flgUsed = 0 And ST1.P3ID <> -1 Then
                                '    AddtoCommon3DSingleTargets(ST1, ST2)
                                'ElseIf ST1.flgUsed = 0 And ST2.flgUsed = 1 And ST2.P3ID <> -1 Then
                                '    AddtoCommon3DSingleTargets(ST2, ST1)
                                'ElseIf ST1.flgUsed = 1 And ST2.flgUsed = 1 Then
                                '    'AddtoCommon3DSingleTargets(ST2, ST1)
                                '    'AddtoCommon3DSingleTargets(ST1, ST2)
                                '    'UnionCommonSTtoST(ST1, ST2)
                                'End If
                            End If
                        Next
                    End If
                End If
            Next
        Next

        For Each ISI As ImageSet In lstImages
            If ISI.flgConnected = True Then
                For Each ST As SingleTarget In ISI.Targets.lstST
                    Dim flgSamePointAri As Boolean = False
                    For Each C3DST As Common3DSingleTarget In lstCommon3dST
                        If C3DST.SamePointAri(ST) = True Then
                            flgSamePointAri = True
                            Exit For
                        End If
                    Next
                    If flgSamePointAri = False Then
                        Dim obj3dTargets As New Common3DSingleTarget
                        obj3dTargets.PID = lstCommon3dST.Count + 1
                        ' ST.P3ID = obj3dTargets.PID
                        obj3dTargets.lstST.Add(ST)
                        lstCommon3dST.Add(obj3dTargets)
                        ReCallSubCollectAllST(obj3dTargets, ST)
                    End If
                Next
            End If
        Next
        'i = 0
        'n = lstCommon3dST.Count
        'For i = n - 1 To 0 Step -1
        '    If lstCommon3dST.Item(i).lstST.Count < 2 Then
        '        lstCommon3dST.RemoveAt(i)
        '    End If
        'Next

    End Sub
    Private Sub UnionCommonSTtoST(ByRef ST1 As SingleTarget, ByRef ST2 As SingleTarget)
        If ST1.P3ID <> ST2.P3ID And ST1.P3ID <> -1 And ST2.P3ID <> -1 Then
            Dim C3DST1 As New Common3DSingleTarget
            Dim C3DST2 As New Common3DSingleTarget
            For Each C3DST As Common3DSingleTarget In lstCommon3dST
                If C3DST.PID = ST1.P3ID Then
                    C3DST1 = C3DST
                End If
                If C3DST.PID = ST2.P3ID Then
                    C3DST2 = C3DST
                End If
            Next
            For Each ST As SingleTarget In C3DST2.lstST
                ST.P3ID = C3DST1.PID
                C3DST1.lstST.Add(ST)
            Next
            C3DST2.PID = -1
        End If
    End Sub
    Private Sub SingleTargetNumbering3(ByRef tt(,) As ImagePairSet)
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim n As Integer = lstImages.Count
        Dim currentIPS As ImagePairSet

        Dim ST2 As SingleTarget
        Dim hv_ind_dist1N As Object = Nothing
        For i = 0 To n - 1
            For j = i To n - 1
                If i <> j Then
                    currentIPS = tt(i, j)
                    If currentIPS.cntComCT >= intCommonCTnum And currentIPS.IS1.Targets.lstST.Count > 0 And currentIPS.IS2.Targets.lstST.Count > 0 Then
                        currentIPS.gen_epilines(hv_Width, hv_Height)
                        For Each ST1 As SingleTarget In currentIPS.IS1.Targets.lstST
                            ' getST_forEpiline(currentIPS, ST1, hv_ind_dist1N)
                            currentIPS.getST_forEpiline(ST1.P2ID - 1, hv_ind_dist1N)
                            If BTuple.TupleSelect(hv_ind_dist1N, 0) <> -1 Then
                                ST2 = currentIPS.IS2.Targets.lstST.Item(hv_ind_dist1N)
                                If ST1.flgUsed = 0 Then
                                    Dim obj3dTargets As New Common3DSingleTarget
                                    obj3dTargets.PID = lstCommon3dST.Count + 1
                                    ST1.flgUsed = 1
                                    ST1.P3ID = obj3dTargets.PID
                                    obj3dTargets.lstST.Add(ST1)
                                    lstCommon3dST.Add(obj3dTargets)
                                End If
                                If ST2.flgUsed = 0 Then
                                    ST2.flgUsed = 1
                                    ST2.P3ID = ST1.P3ID
                                    Dim flgOK As Integer = 1
                                    For Each ST As SingleTarget In lstCommon3dST.Item(ST1.P3ID - 1).lstST
                                        If ST2.ImageID = ST.ImageID Then
                                            flgOK = 0
                                            Exit For
                                        End If
                                    Next
                                    If flgOK = 1 Then
                                        lstCommon3dST.Item(ST1.P3ID - 1).lstST.Add(ST2)
                                    End If
                                End If
                            End If
                        Next
                    End If
                End If
            Next
        Next

        i = 0
        n = lstCommon3dST.Count
        For i = n - 1 To 0 Step -1
            If lstCommon3dST.Item(i).lstST.Count < 2 Then
                lstCommon3dST.RemoveAt(i)
            End If
        Next

    End Sub


    Private Sub SingleTargetNumbering2(ByRef tt(,) As ImagePairSet)
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim n As Integer = lstImages.Count
        Dim currentIPS As ImagePairSet
        Dim otherIPS As ImagePairSet
        Dim ST2 As SingleTarget
        Dim hv_ind_dist1N As Object = Nothing
        Dim hv_ind_distN1 As Object = Nothing
        For i = 0 To n - 1
            For j = i To n - 1
                If i <> j Then
                    currentIPS = tt(i, j)
                    If currentIPS.cntComCT >= intCommonCTnum And currentIPS.IS1.Targets.lstST.Count > 0 Then
                        otherIPS = tt(j, i)
                        If otherIPS.cntComCT >= intCommonCTnum And otherIPS.IS1.Targets.lstST.Count > 0 Then
                            currentIPS.gen_epilines(hv_Width, hv_Height)
                            otherIPS.gen_epilines(hv_Width, hv_Height)
                            For Each ST1 As SingleTarget In currentIPS.IS1.Targets.lstST
                                ' getST_forEpiline(currentIPS, ST1, hv_ind_dist1N)
                                currentIPS.getST_forEpiline(ST1.P2ID - 1, hv_ind_dist1N)
                                If BTuple.TupleSelect(hv_ind_dist1N, 0) <> -1 Then
                                    ST2 = otherIPS.IS1.Targets.lstST.Item(hv_ind_dist1N)
                                    ' getST_forEpiline(otherIPS, ST2, hv_ind_distN1)
                                    otherIPS.getST_forEpiline(ST2.P2ID - 1, hv_ind_distN1)
                                    If BTuple.TupleSelect(hv_ind_distN1, 0) <> -1 Then
                                        If ST1.P2ID = currentIPS.IS1.Targets.lstST.Item(hv_ind_distN1).P2ID Then
                                            If ST1.flgUsed = 0 And ST2.flgUsed = 0 Then
                                                Dim obj3dTargets As New Common3DSingleTarget
                                                obj3dTargets.PID = lstCommon3dST.Count + 1
                                                ST1.flgUsed = 1
                                                ST1.P3ID = obj3dTargets.PID
                                                obj3dTargets.lstST.Add(ST1)
                                                lstCommon3dST.Add(obj3dTargets)
                                                AddtoCommon3DSingleTargets(ST1, ST2)
                                            ElseIf ST1.flgUsed = 1 And ST2.flgUsed = 0 Then
                                                AddtoCommon3DSingleTargets(ST1, ST2)
                                            ElseIf ST1.flgUsed = 0 And ST2.flgUsed = 1 Then
                                                AddtoCommon3DSingleTargets(ST2, ST1)
                                            ElseIf ST1.flgUsed = 1 And ST2.flgUsed = 1 Then
                                                'UnionCommonSTtoST(ST1, ST2)
                                            End If
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    End If
                End If
            Next
        Next

        i = 0
        n = lstCommon3dST.Count
        For i = n - 1 To 0 Step -1
            If lstCommon3dST.Item(i).lstST.Count < 2 Or lstCommon3dST.Item(i).PID = -1 Then
                lstCommon3dST.RemoveAt(i)
            End If
        Next

    End Sub

    Private Sub SingleTargetNumbering(ByRef tt(,) As ImagePairSet, ByRef lstConPairs As List(Of ConnectingPair))
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim t As Integer = 0
        Dim n As Integer = lstImages.Count
        Dim currentIPS As ImagePairSet
        Dim otherIPS As ImagePairSet
        Dim midIPS As ImagePairSet
        Dim flgFirst As Boolean = True
        For i = 0 To n - 3
            For j = i + 1 To n - 2
                currentIPS = tt(i, j)
                If currentIPS.cntComCT >= intCommonCTnum Then
                    If flgFirst = True Then
                        currentIPS.IS1.flgConnected = True
                        currentIPS.IS2.flgConnected = True
                        flgFirst = False
                    End If

                    For t = j + 1 To n - 1
                        otherIPS = tt(j, t)
                        If otherIPS.cntComCT >= intCommonCTnum Then
                            If currentIPS.GetCommonCTcount(otherIPS) >= intCommonCTnum Then
                                midIPS = tt(i, t)
                                If midIPS.cntComCT = -1 Then
                                    Continue For
                                End If
                                If otherIPS.IS2.flgConnected = False Then
                                    Dim CP As New ConnectingPair(i, j, t)
                                    lstConPairs.Add(CP)
                                    otherIPS.IS2.flgConnected = True
                                End If
                                CalcSingleTarget(currentIPS, otherIPS, midIPS)
                            End If
                        End If
                    Next t
                End If
            Next j
        Next i
    End Sub

    Private Sub ConnectAllCamera(ByRef tt(,) As ImagePairSet)
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim n As Integer = lstImages.Count
        Dim t As Integer = 0

        Dim currentIPS As ImagePairSet
        Dim otherIPS As ImagePairSet
        Dim flgFirst As Boolean = True
        '   Dim lstTest As New List(Of String)


        For i = 0 To n - 3
            For j = i + 1 To n - 2
                currentIPS = tt(i, j)
                If currentIPS.cntComCT >= intCommonCTnum Then

                    currentIPS.IS1.flgConnected = True
                    currentIPS.IS2.flgConnected = True
                    Dim FirstPose As New Object
                    HOperatorSet.CreatePose(0, 0, 0, 0, 0, 0, "Rp+T", "gba", "point", FirstPose)
                    currentIPS.IS1.ImagePose.Pose = FirstPose
                    currentIPS.IS2.ImagePose.Pose = currentIPS.PairPose.RelPose
                    currentIPS.IS2.VectorPose.Pose = currentIPS.PairPose.RelPose
                    currentIPS.ComScale = 1.0
                    ' currentIPS.CreateCommon3DCT(lstCommon3dCT)
                    CalcALLCodedTarget3dCoordByHalcon()
                    GoTo jump
                End If
            Next j
        Next i
jump:

        For i = 0 To n - 3
            For j = i + 1 To n - 2
                currentIPS = tt(i, j)
                If currentIPS.cntComCT >= intCommonCTnum Then
                    'If flgFirst = True Then
                    '    currentIPS.IS1.flgConnected = True
                    '    currentIPS.IS2.flgConnected = True
                    '    Dim FirstPose As New Object
                    '    HOperatorSet.CreatePose(0, 0, 0, 0, 0, 0, "Rp+T", "gba", "point", FirstPose)
                    '    currentIPS.IS1.ImagePose.Pose = FirstPose
                    '    currentIPS.IS2.ImagePose.Pose = currentIPS.PairPose.RelPose
                    '    currentIPS.IS2.VectorPose.Pose = currentIPS.PairPose.RelPose
                    '    currentIPS.ComScale = 1.0
                    '    flgFirst = False
                    'End If

                    If currentIPS.ComScale Is Nothing Then
                        Continue For
                    End If

                    For t = j + 1 To n - 1
                        otherIPS = tt(j, t)
                        'If otherIPS.IS2.flgConnected = False Then
                        If otherIPS.cntComCT >= intCommonCTnum Then
                            ' If currentIPS.GetCommonCTcount(otherIPS) >= intCommonCTnum Then

                            If Connect2Pair(currentIPS, otherIPS) = True Then
                            End If
                            'If Connect2PairByVector(currentIPS, otherIPS) = True Then
                            'End If
                            'End If
                            ' End If
                        End If
                    Next t


                    For t = i + 2 To n - 1
                        otherIPS = tt(i, t)
                        'f otherIPS.IS2.flgConnected = False Then
                        If otherIPS.cntComCT >= intCommonCTnum Then
                            ' If currentIPS.GetCommonCTcount(otherIPS) >= intCommonCTnum Then

                            If Connect2Pair(currentIPS, otherIPS) = True Then
                            End If
                            'If Connect2PairByVector(currentIPS, otherIPS) = True Then
                            'End If

                            'End If
                            ' End If
                        End If
                    Next t
                End If
            Next j
        Next i

        For Each ISI As ImageSet In lstImages
            Dim tmpObj As Object = Nothing
            tmpObj = BTuple.TupleGenConst(12, 0.0)
            If ISI.lstImagePose.Count > 1 Then
                For Each campose As Object In ISI.lstImagePose
                    Dim tmpHomMat3d As Object = Nothing
                    HOperatorSet.PoseToHomMat3d(campose, tmpHomMat3d)
                    tmpObj = BTuple.TupleAdd(tmpObj, tmpHomMat3d)
                Next
                tmpObj = BTuple.TupleDiv(tmpObj, ISI.lstImagePose.Count)
                Dim tmpObj1 As Object = Nothing
                HOperatorSet.HomMat3dToPose(tmpObj, tmpObj1)
                tmpObj = BTuple.TupleSub(ISI.ImagePose.Pose, tmpObj1)
                ISI.ImagePose.Pose = tmpObj1
            End If
        Next
    End Sub


    Private Sub ConnectAllCameraByVector(ByRef tt(,) As ImagePairSet)
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim n As Integer = lstImages.Count
        Dim t As Integer = 0
        Dim dblE_mae As Double = Double.MaxValue
        Dim currentIPS As ImagePairSet

        Dim flgFirst As Boolean = True
        '   Dim lstTest As New List(Of String)

        For Each ISI As ImageSet In lstImages
            ISI.flgConnected = False
            ISI.flgUsable = True
            ISI.Quality = Double.MaxValue
        Next
        For i = 0 To n - 1
            For j = 0 To n - 1
                If i <> j Then
                    currentIPS = tt(i, j)
                    If currentIPS.cntComCT >= intCommonCTnum And BTuple.TupleLength(currentIPS.PairPose.hError) = 1 Then
                        currentIPS.IS1.flgFirst = True
                        currentIPS.IS2.flgFirst = True
                        currentIPS.IS2.flgSecond = True
                        currentIPS.IS1.flgConnected = True
                        currentIPS.IS2.flgConnected = True
                        Dim FirstPose As New Object
                        HOperatorSet.CreatePose(0, 0, 0, 0, 0, 0, "Rp+T", "gba", "point", FirstPose)
                        currentIPS.IS1.ImagePose.Pose = FirstPose
                        If 1 Then
                            Dim tmpPoseX As Double = Math.Abs(CDbl(currentIPS.PairPose.RelPose(0)))
                            Dim tmpPoseY As Double = Math.Abs(CDbl(currentIPS.PairPose.RelPose(1)))
                            If tmpPoseX > tmpPoseY Then
                                currentIPS.PairPose.RelPose(0) = currentIPS.PairPose.RelPose(0) / tmpPoseX
                                currentIPS.PairPose.RelPose(1) = currentIPS.PairPose.RelPose(1) / tmpPoseX
                                currentIPS.PairPose.RelPose(2) = currentIPS.PairPose.RelPose(2) / tmpPoseX
                                XorY = 1
                            Else
                                currentIPS.PairPose.RelPose(0) = currentIPS.PairPose.RelPose(0) / tmpPoseY
                                currentIPS.PairPose.RelPose(1) = currentIPS.PairPose.RelPose(1) / tmpPoseY
                                currentIPS.PairPose.RelPose(2) = currentIPS.PairPose.RelPose(2) / tmpPoseY
                                XorY = 2
                            End If
                        End If
                        currentIPS.IS2.ImagePose.Pose = currentIPS.PairPose.RelPose
                        currentIPS.IS2.VectorPose.Pose = currentIPS.PairPose.RelPose
                        currentIPS.ComScale = 1.0
                        ' currentIPS.CreateCommon3DCT(lstCommon3dCT)
                        CalcALLCodedTarget3dCoordByHalcon()
                        'currentIPS.IS1.flgConnected = False
                        'currentIPS.IS2.flgConnected = False
                        GoTo jump
                    End If
                End If
            Next j
        Next i
jump:


        Dim PoseByVector As Object = Nothing
        Dim Quality As Object = Nothing
        Dim cntNotConnectedImage As Integer
        Dim minQ As Double = Double.MaxValue
        Dim minQ_Pose As Object = Nothing
        Dim minQ_ISI As ImageSet = Nothing
        Dim maxQ As Double = Double.MinValue
        Dim tmpcntNotConnectedImage As Integer = -1

        Do
            For Each ISI As ImageSet In lstImages
                If ISI.flgConnected = False Then
                    If ISI.CalcPoseByCommonCT3dPoint(hv_CamparamOut, lstCommon3dCT, PoseByVector, Quality) = True Then
                        ISI.ImagePose.Pose = PoseByVector
                        ISI.Quality = Quality
                        ISI.CommonScale = 1
                        ISI.flgConnected = True
                        CalcALLCodedTarget3dCoordByHalcon()
                    End If
                End If

            Next
            cntNotConnectedImage = 0
            cntConnectedImage = 0
            For Each ISI As ImageSet In lstImages
                If ISI.flgConnected = False Then
                    cntNotConnectedImage = cntNotConnectedImage + 1
                Else
                    cntConnectedImage += 1
                End If
                ISI.flgUsable = True
            Next
            If tmpcntNotConnectedImage <> cntNotConnectedImage Then
                tmpcntNotConnectedImage = cntNotConnectedImage
            Else
                cntNotConnectedImage = 0
            End If
        Loop Until cntNotConnectedImage = 0



        'Do
        '    maxQ = Double.MinValue
        '    For Each ISI As ImageSet In lstImages
        '        '  If ISI.flgConnected = False Then
        '        If ISI.CalcPoseByCommonCT3dPoint(hv_CamparamOut, lstCommon3dCT, PoseByVector, Quality) = True Then
        '            ISI.ImagePose.Pose = PoseByVector
        '            ISI.Quality = Quality
        '            If maxQ < Quality Then
        '                maxQ = Quality
        '            End If
        '        End If
        '        ' End If
        '    Next
        '    CalcALLCodedTarget3dCoordByHalcon()
        '    cntNotConnectedImage += 1
        'Loop Until cntNotConnectedImage = 10


        'Do
        '    minQ = Double.MaxValue
        '    For Each ISI As ImageSet In lstImages
        '        If ISI.flgConnected = False Then
        '            If ISI.CalcPoseByCommonCT3dPoint(hv_CamparamOut, lstCommon3dCT, PoseByVector, Quality) = True Then
        '                If minQ > Quality Then
        '                    minQ = Quality
        '                    minQ_ISI = ISI
        '                    minQ_Pose = PoseByVector
        '                End If
        '                'ISI.ImagePose.Pose = PoseByVector
        '                'ISI.Quality = Quality
        '                ''otherIPS.IS2.CalcRay(). chi 
        '                ''ISI.VectorPose.Pose = otherIPS.RelPose(True)
        '                'ISI.CommonScale = 1
        '                ''otherIPS.IS2.MeanError = BTuple.TupleMean(tmpdiffRelPose)
        '                ''otherIPS.IS2.DevError = BTuple.TupleMax(tmpdiffRelPose)
        '                ''otherIPS.IS2.MidCount = BTuple.TupleLength(CurCT3dPoint.X)
        '                'ISI.flgConnected = True
        '                'CalcALLCodedTarget3dCoordByHalcon()
        '            End If
        '        End If
        '    Next
        '    minQ_ISI.ImagePose.Pose = minQ_Pose
        '    minQ_ISI.Quality = minQ
        '    minQ_ISI.flgConnected = True
        '    minQ_ISI.CommonScale = 1
        '    CalcALLCodedTarget3dCoordByHalcon()
        '    cntNotConnectedImage -= 1
        '    If minQ > maxQ Then
        '        maxQ = minQ
        '    End If
        '    If minQ > 2 Then
        '        minQ = minQ
        '    End If
        'Loop Until cntNotConnectedImage = 0


    End Sub



    Private Sub ConnectAllCameraByBestScale(ByRef tt(,) As ImagePairSet)
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim n As Integer = lstImages.Count
        Dim t As Integer = 0

        Dim currentIPS As ImagePairSet

        Dim flgFirst As Boolean = True
        '   Dim lstTest As New List(Of String)

        For Each ISI As ImageSet In lstImages
            ISI.flgConnected = False
        Next
        For i = 0 To n - 2
            For j = i + 1 To n - 1
                currentIPS = tt(i, j)
                If currentIPS.cntComCT >= intCommonCTnum Then
                    currentIPS.IS1.flgFirst = True
                    currentIPS.IS2.flgFirst = True
                    currentIPS.IS2.flgSecond = True
                    currentIPS.IS1.flgConnected = True
                    currentIPS.IS2.flgConnected = True
                    Dim FirstPose As New Object
                    HOperatorSet.CreatePose(0, 0, 0, 0, 0, 0, "Rp+T", "gba", "point", FirstPose)
                    currentIPS.IS1.ImagePose.Pose = FirstPose
                    If 1 Then
                        Dim tmpPoseX As Double = Math.Abs(CDbl(currentIPS.PairPose.RelPose(0)))
                        Dim tmpPoseY As Double = Math.Abs(CDbl(currentIPS.PairPose.RelPose(1)))
                        If tmpPoseX > tmpPoseY Then
                            currentIPS.PairPose.RelPose(0) = currentIPS.PairPose.RelPose(0) / tmpPoseX
                            currentIPS.PairPose.RelPose(1) = currentIPS.PairPose.RelPose(1) / tmpPoseX
                            currentIPS.PairPose.RelPose(2) = currentIPS.PairPose.RelPose(2) / tmpPoseX
                            XorY = 1
                        Else
                            currentIPS.PairPose.RelPose(0) = currentIPS.PairPose.RelPose(0) / tmpPoseY
                            currentIPS.PairPose.RelPose(1) = currentIPS.PairPose.RelPose(1) / tmpPoseY
                            currentIPS.PairPose.RelPose(2) = currentIPS.PairPose.RelPose(2) / tmpPoseY
                            XorY = 2
                        End If
                    End If
                    currentIPS.IS2.ImagePose.Pose = currentIPS.PairPose.RelPose
                    currentIPS.IS2.VectorPose.Pose = currentIPS.PairPose.RelPose
                    currentIPS.ComScale = 1.0
                    ' currentIPS.CreateCommon3DCT(lstCommon3dCT)
                    CalcALLCodedTarget3dCoordByHalcon()
                    'currentIPS.IS1.flgConnected = False
                    'currentIPS.IS2.flgConnected = False
                    GoTo jump
                End If
            Next j
        Next i
jump:

        Dim PoseByBestScale As Object = Nothing
        Dim Quality As Object = Nothing
        Dim ComScale As Object = Nothing
        Dim cntNotConnectedImage As Integer

        Do
            For i = 0 To n - 2
                For j = i + 1 To n - 1
                    currentIPS = tt(i, j)
                    If currentIPS.cntComCT >= intCommonCTnum Then
                        If currentIPS.IS2.flgConnected = False Then
                            If currentIPS.CalcPoseByCommonCTandBestScale(lstCommon3dCT, PoseByBestScale, Quality, ComScale, ScaleMM) = True Then
                                currentIPS.IS2.ImagePose.Pose = PoseByBestScale
                                currentIPS.IS2.Quality = Quality
                                currentIPS.IS2.CommonScale = ComScale
                                currentIPS.IS2.flgConnected = True
                                currentIPS.IS2.lstImagePose.Add(PoseByBestScale)
                                CalcALLCodedTarget3dCoordByHalcon()
                            End If
                        End If
                    End If
                Next
            Next
            cntNotConnectedImage = 0
            For Each ISI As ImageSet In lstImages
                If ISI.flgConnected = False Then
                    cntNotConnectedImage = 1
                    Exit For
                End If
            Next
        Loop Until cntNotConnectedImage = 0

        't = 0
        'Dim flgSyuryo As Boolean = True
        'Do
        '    flgSyuryo = True
        '    For i = 0 To n - 2
        '        For j = i + 1 To n - 1
        '            currentIPS = tt(i, j)
        '            If currentIPS.cntComCT >= intCommonCTnum Then
        '                If currentIPS.IS2.flgFirst = False Then
        '                    If currentIPS.CalcPoseByCommonCTandBestScale(lstCommon3dCT, PoseByBestScale, Quality, ComScale) = True Then
        '                        If currentIPS.IS2.Quality > Quality Then
        '                            currentIPS.IS2.ImagePose.Pose = PoseByBestScale
        '                            currentIPS.IS2.Quality = Quality
        '                            currentIPS.IS2.CommonScale = ComScale
        '                            currentIPS.IS2.flgConnected = True
        '                            currentIPS.IS2.lstImagePose.Add(PoseByBestScale)
        '                            flgSyuryo = False
        '                        End If
        '                    End If
        '                End If
        '            End If
        '        Next
        '    Next
        '    CalcALLCodedTarget3dCoordByHalcon()
        '    t += 1
        'Loop Until t = 10 Or flgSyuryo = True

    End Sub

    '全ImageのPoseを計算
    Private Function ConnectAllCameraByBestScale2(ByRef tt(,) As ImagePairSet) As Integer
        TimeMonStart()

        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim n As Integer = lstImages.Count
        Dim t As Integer = 0
        Dim dblE_mae As Double = Double.MaxValue
        Dim currentIPS As ImagePairSet

        Dim flgFirst As Boolean = True
        '   Dim lstTest As New List(Of String)

        For Each ISI As ImageSet In lstImages
            ISI.flgConnected = False
            ISI.flgUsable = True
            ISI.Quality = Double.MaxValue
        Next
        'For i = 0 To n - 1
        '    For j = 4 To n - 1
        '        If i <> j Then
        currentIPS = tt(FirstI, FirstJ)
        If currentIPS Is Nothing Then
            ConnectAllCameraByBestScale2 = 0
            Exit Function
        End If
        If currentIPS.cntComCT >= intCommonCTnum And BTuple.TupleLength(currentIPS.PairPose.hError).I = 1 Then
            currentIPS.IS1.flgFirst = True
            currentIPS.IS2.flgFirst = True
            currentIPS.IS2.flgSecond = True
            currentIPS.IS1.flgConnected = True
            currentIPS.IS2.flgConnected = True
            Dim FirstPose As New HTuple
            HOperatorSet.CreatePose(0, 0, 0, 0, 0, 0, "Rp+T", "gba", "point", FirstPose)
            currentIPS.IS1.ImagePose.Pose = FirstPose
            If 1 Then
                Dim tmpPoseX As Double = Math.Abs(CDbl(currentIPS.PairPose.RelPose(0).D))
                Dim tmpPoseY As Double = Math.Abs(CDbl(currentIPS.PairPose.RelPose(1).D))
                If tmpPoseX > tmpPoseY Then
                    currentIPS.PairPose.RelPose(0) = currentIPS.PairPose.RelPose(0) / tmpPoseX
                    currentIPS.PairPose.RelPose(1) = currentIPS.PairPose.RelPose(1) / tmpPoseX
                    currentIPS.PairPose.RelPose(2) = currentIPS.PairPose.RelPose(2) / tmpPoseX
                    XorY = 1
                Else
                    currentIPS.PairPose.RelPose(0) = currentIPS.PairPose.RelPose(0) / tmpPoseY
                    currentIPS.PairPose.RelPose(1) = currentIPS.PairPose.RelPose(1) / tmpPoseY
                    currentIPS.PairPose.RelPose(2) = currentIPS.PairPose.RelPose(2) / tmpPoseY
                    XorY = 2
                End If
            End If
            currentIPS.IS2.ImagePose.Pose = currentIPS.PairPose.RelPose
            currentIPS.IS2.VectorPose.Pose = currentIPS.PairPose.RelPose
            currentIPS.ComScale = 1.0
            ' currentIPS.CreateCommon3DCT(lstCommon3dCT)
            'CalcAllImages_Ray()
            'CalcALLSingleTarget3dCoord()
            'For Each CT As CodedTarget In currentIPS.lstIS1_ComCT
            '    Dim C3DCT As New Common3DCodedTarget
            '    C3DCT.PID = CT.CT_ID
            '    C3DCT.lstCT.Add(CT)
            '    lstCommon3dCT.Add(C3DCT)
            'Next
            'For Each CT As CodedTarget In currentIPS.lstIS2_ComCT
            '    For Each Com3DCT As Common3DCodedTarget In lstCommon3dCT
            '        If Com3DCT.PID = CT.CT_ID Then
            '            Com3DCT.lstCT.Add(CT)
            '        End If
            '    Next
            'Next
            'CalcALLCodedTarget3dCoordByHalcon()

            CalcOneImages_Ray(currentIPS.IS1)
            CalcOneImages_Ray(currentIPS.IS2)
            ' CalcALLCTnoSingleTarget3dCoord()
            CalcThisImagesCT_3dCoord(currentIPS.IS2)

            'currentIPS.IS1.flgConnected = False
            'currentIPS.IS2.flgConnected = False
            '    GoTo jump
            'End If
            CalcKarinoScaleByCTPoints(ScaleMM)

        End If
        '    Next j
        'Next i
jump:

        Dim PoseByBestScale As Object = Nothing
        Dim Quality As Object = Nothing
        Dim ComScale As Object = Nothing
        Dim MaxQuality As Object = Nothing
        Dim cntNotConnectedImage As Integer
        Dim tmpcntNotConnectedImage As Integer = -1
        cntCurrentConnectedImage = 0
        dblE_mae = Double.MaxValue
        '  Dim strT As String = ""
        Do
            For i = 0 To n - 1
                If lstImages(i).flgConnected = True Then
                    For j = 0 To n - 1
                        If i <> j Then
                            currentIPS = tt(i, j)
                            If currentIPS.IS2.ImageId = 3 Then
                                Quality = Quality
                            End If
                            If currentIPS.cntComCT >= intCommonCTnum Then
                                If currentIPS.IS1.flgConnected = True And currentIPS.IS2.flgConnected = False And currentIPS.IS2.flgUsable = True Then
                                    'If currentIPS.IS2.flgConnected = False And currentIPS.IS2.flgUsable = True Then
                                    'If currentIPS.IS2.ImageId = 10 Then
                                    '    Quality = Quality
                                    'End If
                                    If currentIPS.CalcPoseByCommonCTandBestScale(lstCommon3dCT, PoseByBestScale, Quality, ComScale, ScaleMM) = True Then
                                        If currentIPS.IS2.ImageId = 3 Then
                                            Quality = Quality
                                        End If
                                        '  If currentIPS.IS2.Quality > Quality Then



                                        currentIPS.IS2.ImagePose.Pose = PoseByBestScale
                                        currentIPS.IS2.Quality = Quality
                                        currentIPS.IS2.CommonScale = ComScale.D
                                        currentIPS.IS2.flgConnected = True

                                        CalcOneImages_Ray(currentIPS.IS2)
                                        'CalcALLCTnoSingleTarget3dCoord()
                                        CalcThisImagesCT_3dCoord(currentIPS.IS2)

                                        'CalcReProjectionErrorOneCT(currentIPS.IS2)
                                        CalcReProjectionErrorOneCT_faster(currentIPS.IS2)
                                        'CalcALLCTnoSingleTarget3dCoord()
                                        CalcThisImagesCT_3dCoord(currentIPS.IS2)
                                        cntCurrentConnectedImage += 1
                                        RunProgressBarEvent(cntCurrentConnectedImage, n, "解析処理中(3)")


                                        'If CalcALLCodedTarget3dCoordByHalcon() = True Then
                                        '    'CalcReProjectionErrorOneImage(currentIPS.IS2)

                                        'Else
                                        '    currentIPS.IS2.ImagePose.Pose = Nothing
                                        '    currentIPS.IS2.Quality = Nothing
                                        '    currentIPS.IS2.CommonScale = Nothing
                                        '    currentIPS.IS2.flgConnected = False
                                        'End If

                                        ' currentIPS.IS2.lstImagePose.Add(PoseByBestScale)
                                        'CalcAllImages_Ray()


                                        'cntNotConnectedImage = 0
                                        'cntConnectedImage = 0
                                        'For Each ISI As ImageSet In lstImages
                                        '    If ISI.flgConnected = False Then
                                        '        cntNotConnectedImage = cntNotConnectedImage + 1
                                        '    Else
                                        '        cntConnectedImage += 1
                                        '    End If
                                        'Next
                                        'If RunBA_Totyu(FBMlib.T_treshold, hv_CamparamOut, dblE_mae) = False Then
                                        '    currentIPS.IS2.flgConnected = False
                                        '    If tmpcntNotConnectedImage = -1 Then
                                        '        currentIPS.IS2.flgUsable = False
                                        '    End If

                                        '    currentIPS.IS2.ImagePose.Pose = Nothing
                                        '    ' CalcALLCodedTarget3dCoordByHalcon()
                                        'Else
                                        '    strT = strT & currentIPS.IS1.ImageName & "によって" & currentIPS.IS2.ImageName & "を繋いだ" & vbNewLine
                                        '    CalcALLCodedTarget3dCoordByHalcon()
                                        'End If

                                        ' End If
                                    End If
                                    'ElseIf currentIPS.IS1.flgConnected = True And currentIPS.IS2.flgConnected = True And currentIPS.IS2.flgUsable = True Then
                                    '    If currentIPS.CalcPoseByCommonCTandBestScale(lstCommon3dCT, PoseByBestScale, Quality, ComScale) = True Then
                                    '        If Quality < currentIPS.IS2.Quality Then
                                    '            Dim tmpPose As Object
                                    '            tmpPose = currentIPS.IS2.ImagePose.Pose
                                    '            currentIPS.IS2.ImagePose.Pose = PoseByBestScale
                                    '            If CalcALLCodedTarget3dCoordByHalcon() = True Then
                                    '                currentIPS.IS2.Quality = Quality
                                    '                currentIPS.IS2.CommonScale = ComScale
                                    '            Else
                                    '                currentIPS.IS2.ImagePose.Pose = tmpPose
                                    '            End If
                                    '        End If
                                    '    End If
                                End If
                            End If
                        End If
                    Next
                End If
            Next


            cntNotConnectedImage = 0
            cntConnectedImage = 0
            For Each ISI As ImageSet In lstImages
                If ISI.flgConnected = False Then
                    cntNotConnectedImage = cntNotConnectedImage + 1
                Else
                    cntConnectedImage += 1
                End If
                ISI.flgUsable = True
            Next
            If tmpcntNotConnectedImage <> cntNotConnectedImage Then
                tmpcntNotConnectedImage = cntNotConnectedImage
            Else
                cntNotConnectedImage = 0
            End If
        Loop Until cntNotConnectedImage = 0
        'My.Computer.FileSystem.WriteAllText(ProjectPath & "\connect_monitor.csv", strT, True)
        'For Each ISI As ImageSet In lstImages
        '    ISI.flgConnected = False
        '    For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
        '        If C3DCT.flgUsable = True Then
        '            For Each CT As CodedTarget In C3DCT.lstCT
        '                If ISI.ImageId = CT.ImageID Then
        '                    ISI.flgConnected = True
        '                    Exit For
        '                End If
        '            Next
        '        End If
        '    Next
        '    If ISI.flgConnected = False Then
        '        ISI.flgConnected = False
        '    End If
        'Next
        'cntConnectedImage = 0
        'For Each ISI As ImageSet In lstImages
        '    If ISI.flgConnected = True Then
        '        cntConnectedImage += 1
        '    End If
        'Next



        GenCommon3Dpoint()
        '20160501 ADD SUSANO START
        For Each ISI As ImageSet In lstImages
            If ISI.flgConnected = True Then
                CalcReProjectionErrorOneCT_faster(ISI)
                CalcThisImagesCT_3dCoord(ISI)
            End If
        Next
        '20160505 ADD SUSANO END

        Dim nP As Integer = lstCommon3dCT.Count - 1
        Dim nCT As Integer = -1
        For i = nP To 0 Step -1

            Dim C3DCT As Common3DCodedTarget = lstCommon3dCT.Item(i)
            nCT = C3DCT.lstCT.Count - 1
            For j = nCT To 0 Step -1
                Dim CT As CodedTarget = C3DCT.lstCT.Item(j)
                If lstImages(CT.ImageID - 1).flgConnected = False Then
                    C3DCT.lstCT.RemoveAt(j)
                End If
            Next
            If C3DCT.lstCT.Count < MinCountOfRays Then
                lstCommon3dCT.RemoveAt(i)
            End If
        Next
        ConnectAllCameraByBestScale2 = cntConnectedImage
        'CalcALLCTnoSingleTarget3dCoord()
        ' CalcALLCodedTarget3dCoordByHalcon()

        TimeMonEnd()
    End Function


    Private Sub ConnectAllCameraByBestScale2_para(ByVal tt(,) As ImagePairSet)
        ' Dim i As Integer = 0
        Dim j As Integer = 0
        Dim n As Integer = lstImages.Count
        Dim t As Integer = 0
        Dim dblE_mae As Double = Double.MaxValue
        Dim currentIPS As ImagePairSet

        Dim flgFirst As Boolean = True
        '   Dim lstTest As New List(Of String)

        For Each ISI As ImageSet In lstImages
            ISI.flgConnected = False
            ISI.flgUsable = True
            ISI.Quality = Double.MaxValue
        Next
        'For i = 0 To n - 1
        '    For j = 4 To n - 1
        '        If i <> j Then
        currentIPS = tt(FirstI, FirstJ)
        If currentIPS Is Nothing Then
            Exit Sub
        End If
        If currentIPS.cntComCT >= intCommonCTnum And BTuple.TupleLength(currentIPS.PairPose.hError).I = 1 Then
            currentIPS.IS1.flgFirst = True
            currentIPS.IS2.flgFirst = True
            currentIPS.IS2.flgSecond = True
            currentIPS.IS1.flgConnected = True
            currentIPS.IS2.flgConnected = True
            Dim FirstPose As New Object
            HOperatorSet.CreatePose(0, 0, 0, 0, 0, 0, "Rp+T", "gba", "point", FirstPose)
            currentIPS.IS1.ImagePose.Pose = FirstPose
            If 1 Then
                Dim tmpPoseX As Double = Math.Abs(CDbl(currentIPS.PairPose.RelPose(0)))
                Dim tmpPoseY As Double = Math.Abs(CDbl(currentIPS.PairPose.RelPose(1)))
                If tmpPoseX > tmpPoseY Then
                    currentIPS.PairPose.RelPose(0) = currentIPS.PairPose.RelPose(0) / tmpPoseX
                    currentIPS.PairPose.RelPose(1) = currentIPS.PairPose.RelPose(1) / tmpPoseX
                    currentIPS.PairPose.RelPose(2) = currentIPS.PairPose.RelPose(2) / tmpPoseX
                    XorY = 1
                Else
                    currentIPS.PairPose.RelPose(0) = currentIPS.PairPose.RelPose(0) / tmpPoseY
                    currentIPS.PairPose.RelPose(1) = currentIPS.PairPose.RelPose(1) / tmpPoseY
                    currentIPS.PairPose.RelPose(2) = currentIPS.PairPose.RelPose(2) / tmpPoseY
                    XorY = 2
                End If
            End If
            currentIPS.IS2.ImagePose.Pose = currentIPS.PairPose.RelPose
            currentIPS.IS2.VectorPose.Pose = currentIPS.PairPose.RelPose
            currentIPS.ComScale = 1.0
            ' currentIPS.CreateCommon3DCT(lstCommon3dCT)
            'CalcAllImages_Ray()
            'CalcALLSingleTarget3dCoord()
            'For Each CT As CodedTarget In currentIPS.lstIS1_ComCT
            '    Dim C3DCT As New Common3DCodedTarget
            '    C3DCT.PID = CT.CT_ID
            '    C3DCT.lstCT.Add(CT)
            '    lstCommon3dCT.Add(C3DCT)
            'Next
            'For Each CT As CodedTarget In currentIPS.lstIS2_ComCT
            '    For Each Com3DCT As Common3DCodedTarget In lstCommon3dCT
            '        If Com3DCT.PID = CT.CT_ID Then
            '            Com3DCT.lstCT.Add(CT)
            '        End If
            '    Next
            'Next
            'CalcALLCodedTarget3dCoordByHalcon()

            CalcOneImages_Ray(currentIPS.IS1)
            CalcOneImages_Ray(currentIPS.IS2)
            CalcALLCTnoSingleTarget3dCoord()

            'currentIPS.IS1.flgConnected = False
            'currentIPS.IS2.flgConnected = False
            '    GoTo jump
            'End If
            CalcKarinoScaleByCTPoints(ScaleMM)

        End If
        '    Next j
        'Next i
jump:

        Dim PoseByBestScale As Object = Nothing
        Dim Quality As Object = Nothing
        Dim ComScale As Object = Nothing
        Dim MaxQuality As Object = Nothing
        Dim cntNotConnectedImage As Integer
        Dim tmpcntNotConnectedImage As Integer = -1
        dblE_mae = Double.MaxValue
        '  Dim strT As String = ""
        Do
            Parallel.For(0, n, Sub(i)
                                   For j = 0 To n - 1
                                       If i <> j Then
                                           currentIPS = tt(i, j)
                                           If currentIPS.IS2.ImageId = 9 Then
                                               Quality = Quality
                                           End If
                                           If currentIPS.cntComCT >= intCommonCTnum Then
                                               If currentIPS.IS1.flgConnected = True And currentIPS.IS2.flgConnected = False And currentIPS.IS2.flgUsable = True Then
                                                   If currentIPS.IS2.ImageId = 10 Then
                                                       Quality = Quality
                                                   End If
                                                   If currentIPS.CalcPoseByCommonCTandBestScale(lstCommon3dCT, PoseByBestScale, Quality, ComScale, ScaleMM) = True Then

                                                       '  If currentIPS.IS2.Quality > Quality Then
                                                       currentIPS.IS2.ImagePose.Pose = PoseByBestScale
                                                       currentIPS.IS2.Quality = Quality
                                                       currentIPS.IS2.CommonScale = ComScale
                                                       currentIPS.IS2.flgConnected = True

                                                       CalcOneImages_Ray(currentIPS.IS2)
                                                       CalcALLCTnoSingleTarget3dCoord()
                                                       CalcReProjectionErrorOneCT(currentIPS.IS2)
                                                       CalcALLCTnoSingleTarget3dCoord()
                                                       'If CalcALLCodedTarget3dCoordByHalcon() = True Then
                                                       '    'CalcReProjectionErrorOneImage(currentIPS.IS2)

                                                       'Else
                                                       '    currentIPS.IS2.ImagePose.Pose = Nothing
                                                       '    currentIPS.IS2.Quality = Nothing
                                                       '    currentIPS.IS2.CommonScale = Nothing
                                                       '    currentIPS.IS2.flgConnected = False
                                                       'End If

                                                       ' currentIPS.IS2.lstImagePose.Add(PoseByBestScale)
                                                       'CalcAllImages_Ray()


                                                       'cntNotConnectedImage = 0
                                                       'cntConnectedImage = 0
                                                       'For Each ISI As ImageSet In lstImages
                                                       '    If ISI.flgConnected = False Then
                                                       '        cntNotConnectedImage = cntNotConnectedImage + 1
                                                       '    Else
                                                       '        cntConnectedImage += 1
                                                       '    End If
                                                       'Next
                                                       'If RunBA_Totyu(FBMlib.T_treshold, hv_CamparamOut, dblE_mae) = False Then
                                                       '    currentIPS.IS2.flgConnected = False
                                                       '    If tmpcntNotConnectedImage = -1 Then
                                                       '        currentIPS.IS2.flgUsable = False
                                                       '    End If

                                                       '    currentIPS.IS2.ImagePose.Pose = Nothing
                                                       '    ' CalcALLCodedTarget3dCoordByHalcon()
                                                       'Else
                                                       '    strT = strT & currentIPS.IS1.ImageName & "によって" & currentIPS.IS2.ImageName & "を繋いだ" & vbNewLine
                                                       '    CalcALLCodedTarget3dCoordByHalcon()
                                                       'End If

                                                       ' End If
                                                   End If
                                                   'ElseIf currentIPS.IS1.flgConnected = True And currentIPS.IS2.flgConnected = True And currentIPS.IS2.flgUsable = True Then
                                                   '    If currentIPS.CalcPoseByCommonCTandBestScale(lstCommon3dCT, PoseByBestScale, Quality, ComScale) = True Then
                                                   '        If Quality < currentIPS.IS2.Quality Then
                                                   '            Dim tmpPose As Object
                                                   '            tmpPose = currentIPS.IS2.ImagePose.Pose
                                                   '            currentIPS.IS2.ImagePose.Pose = PoseByBestScale
                                                   '            If CalcALLCodedTarget3dCoordByHalcon() = True Then
                                                   '                currentIPS.IS2.Quality = Quality
                                                   '                currentIPS.IS2.CommonScale = ComScale
                                                   '            Else
                                                   '                currentIPS.IS2.ImagePose.Pose = tmpPose
                                                   '            End If
                                                   '        End If
                                                   '    End If
                                               End If
                                           End If
                                       End If
                                   Next
                               End Sub)


            cntNotConnectedImage = 0
            cntConnectedImage = 0
            For Each ISI As ImageSet In lstImages
                If ISI.flgConnected = False Then
                    cntNotConnectedImage = cntNotConnectedImage + 1
                Else
                    cntConnectedImage += 1
                End If
                ISI.flgUsable = True
            Next
            If tmpcntNotConnectedImage <> cntNotConnectedImage Then
                tmpcntNotConnectedImage = cntNotConnectedImage
            Else
                cntNotConnectedImage = 0
            End If
        Loop Until cntNotConnectedImage = 0
        'My.Computer.FileSystem.WriteAllText(ProjectPath & "\connect_monitor.csv", strT, True)
        'For Each ISI As ImageSet In lstImages
        '    ISI.flgConnected = False
        '    For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
        '        If C3DCT.flgUsable = True Then
        '            For Each CT As CodedTarget In C3DCT.lstCT
        '                If ISI.ImageId = CT.ImageID Then
        '                    ISI.flgConnected = True
        '                    Exit For
        '                End If
        '            Next
        '        End If
        '    Next
        '    If ISI.flgConnected = False Then
        '        ISI.flgConnected = False
        '    End If
        'Next
        'cntConnectedImage = 0
        'For Each ISI As ImageSet In lstImages
        '    If ISI.flgConnected = True Then
        '        cntConnectedImage += 1
        '    End If
        'Next
        CalcALLCTnoSingleTarget3dCoord()
        ' CalcALLCodedTarget3dCoordByHalcon()
    End Sub


    Private Sub ConnectAllCameraByBestScale3(ByRef tt(,) As ImagePairSet)
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim n As Integer = lstImages.Count
        Dim t As Integer = 0
        Dim dblE_mae As Double = Double.MaxValue
        Dim currentIPS As ImagePairSet

        Dim flgFirst As Boolean = True
        '   Dim lstTest As New List(Of String)

        For Each ISI As ImageSet In lstImages
            ISI.flgConnected = False
            ISI.flgUsable = True
            ISI.Quality = Double.MaxValue
        Next
        'For i = 0 To n - 1
        '    For j = 4 To n - 1
        '        If i <> j Then
        currentIPS = tt(FirstI, FirstJ)
        If currentIPS Is Nothing Then
            Exit Sub
        End If
        If currentIPS.cntComCT >= intCommonCTnum And BTuple.TupleLength(currentIPS.PairPose.hError) = 1 Then
            currentIPS.IS1.flgFirst = True
            currentIPS.IS2.flgFirst = True
            currentIPS.IS2.flgSecond = True
            currentIPS.IS1.flgConnected = True
            currentIPS.IS2.flgConnected = True
            Dim FirstPose As New Object
            HOperatorSet.CreatePose(0, 0, 0, 0, 0, 0, "Rp+T", "gba", "point", FirstPose)
            currentIPS.IS1.ImagePose.Pose = FirstPose
            If 1 Then
                Dim tmpPoseX As Double = Math.Abs(CDbl(currentIPS.PairPose.RelPose(0)))
                Dim tmpPoseY As Double = Math.Abs(CDbl(currentIPS.PairPose.RelPose(1)))
                If tmpPoseX > tmpPoseY Then
                    currentIPS.PairPose.RelPose(0) = currentIPS.PairPose.RelPose(0) / tmpPoseX
                    currentIPS.PairPose.RelPose(1) = currentIPS.PairPose.RelPose(1) / tmpPoseX
                    currentIPS.PairPose.RelPose(2) = currentIPS.PairPose.RelPose(2) / tmpPoseX
                    XorY = 1
                Else
                    currentIPS.PairPose.RelPose(0) = currentIPS.PairPose.RelPose(0) / tmpPoseY
                    currentIPS.PairPose.RelPose(1) = currentIPS.PairPose.RelPose(1) / tmpPoseY
                    currentIPS.PairPose.RelPose(2) = currentIPS.PairPose.RelPose(2) / tmpPoseY
                    XorY = 2
                End If
            End If
            currentIPS.IS2.ImagePose.Pose = currentIPS.PairPose.RelPose
            currentIPS.IS2.VectorPose.Pose = currentIPS.PairPose.RelPose
            currentIPS.ComScale = 1.0
            ' currentIPS.CreateCommon3DCT(lstCommon3dCT)
            'CalcAllImages_Ray()
            'CalcALLSingleTarget3dCoord()
            'For Each CT As CodedTarget In currentIPS.lstIS1_ComCT
            '    Dim C3DCT As New Common3DCodedTarget
            '    C3DCT.PID = CT.CT_ID
            '    C3DCT.lstCT.Add(CT)
            '    lstCommon3dCT.Add(C3DCT)
            'Next
            'For Each CT As CodedTarget In currentIPS.lstIS2_ComCT
            '    For Each Com3DCT As Common3DCodedTarget In lstCommon3dCT
            '        If Com3DCT.PID = CT.CT_ID Then
            '            Com3DCT.lstCT.Add(CT)
            '        End If
            '    Next
            'Next
            'CalcALLCodedTarget3dCoordByHalcon()

            CalcOneImages_Ray(currentIPS.IS1)
            CalcOneImages_Ray(currentIPS.IS2)
            ' CalcALLCTnoSingleTarget3dCoord()
            CalcThisImagesCT_3dCoord(currentIPS.IS2)

            'currentIPS.IS1.flgConnected = False
            'currentIPS.IS2.flgConnected = False
            '    GoTo jump
            'End If
            CalcKarinoScaleByCTPoints(ScaleMM)

        End If
        '    Next j
        'Next i
jump:

        Dim PoseByBestScale As Object = Nothing
        Dim Quality As Object = Nothing
        Dim ComScale As Object = Nothing
        Dim MaxQuality As Object = Nothing
        Dim cntNotConnectedImage As Integer
        Dim tmpcntNotConnectedImage As Integer = -1
        Dim cntCurrentConnectedImage As Integer = 0
        dblE_mae = Double.MaxValue
        Dim tmpIPS As ImagePairSet = currentIPS
        '  Dim strT As String = ""
        cntCurrentConnectedImage = 0

        Do
            ' i = tmpIPS.IS1.ImageId - 1
            'For i = 0 To n - 1
            '    RecallConnectCamera(i, tt)
            'Next
            For Each ISI As ImageSet In lstImages
                RecallConnectCamera(ISI.ImageId - 1, tt)
            Next

            cntNotConnectedImage = 0
            cntConnectedImage = 0
            For Each ISI As ImageSet In lstImages
                If ISI.flgConnected = False Then
                    cntNotConnectedImage = cntNotConnectedImage + 1
                Else
                    cntConnectedImage += 1
                End If
                ISI.flgUsable = True
            Next
            If tmpcntNotConnectedImage <> cntNotConnectedImage Then
                tmpcntNotConnectedImage = cntNotConnectedImage
            Else
                cntNotConnectedImage = 0
            End If
        Loop Until cntNotConnectedImage = 0

        Dim nP As Integer = lstCommon3dCT.Count - 1
        Dim nCT As Integer = -1
        For i = nP To 0 Step -1
            Dim C3DCT As Common3DCodedTarget = lstCommon3dCT.Item(i)
            nCT = C3DCT.lstCT.Count - 1
            For j = nCT To 0 Step -1
                Dim CT As CodedTarget = C3DCT.lstCT.Item(j)
                If lstImages(CT.ImageID - 1).flgConnected = False Then
                    C3DCT.lstCT.RemoveAt(j)
                End If
            Next
            If C3DCT.lstCT.Count < 2 Then
                lstCommon3dCT.RemoveAt(i)
            End If
        Next
        CalcALLCTnoSingleTarget3dCoord()
        ' CalcALLCodedTarget3dCoordByHalcon()
    End Sub

    Private Function RecallConnectCamera(ByVal i As Integer, ByRef tt(,) As ImagePairSet) As ImagePairSet
        Dim j As Integer
        Dim n As Integer = lstImages.Count
        Dim currentIPS As ImagePairSet
        Dim PoseByBestScale As Object = Nothing
        Dim Quality As Object = Nothing
        Dim ComScale As Object = Nothing
        Dim flgEnd As Boolean = False
        Dim tmpIPS As ImagePairSet = Nothing

        For j = 0 To n - 1
            If i <> j Then
                currentIPS = tt(i, j)
                If currentIPS.cntComCT >= intCommonCTnum Then
                    If currentIPS.IS2.flgConnected = False And currentIPS.IS2.flgUsable = True Then
                        If currentIPS.CalcPoseByCommonCTandBestScale(lstCommon3dCT, PoseByBestScale, Quality, ComScale, ScaleMM) = True Then

                            '  If currentIPS.IS2.Quality > Quality Then
                            currentIPS.IS2.ImagePose.Pose = PoseByBestScale
                            currentIPS.IS2.Quality = Quality
                            currentIPS.IS2.CommonScale = ComScale
                            currentIPS.IS2.flgConnected = True

                            CalcOneImages_Ray(currentIPS.IS2)
                            'CalcALLCTnoSingleTarget3dCoord()
                            CalcThisImagesCT_3dCoord(currentIPS.IS2)

                            'CalcReProjectionErrorOneCT(currentIPS.IS2)
                            CalcReProjectionErrorOneCT_faster(currentIPS.IS2)
                            'CalcALLCTnoSingleTarget3dCoord()
                            CalcThisImagesCT_3dCoord(currentIPS.IS2)
                            If currentIPS.CalcPoseByCommonCTandBestScale(lstCommon3dCT, PoseByBestScale, Quality, ComScale, ScaleMM) = True Then
                                currentIPS.IS2.ImagePose.Pose = PoseByBestScale
                                currentIPS.IS2.Quality = Quality
                                currentIPS.IS2.CommonScale = ComScale
                                currentIPS.IS2.flgConnected = True
                                cntCurrentConnectedImage += 1
                                'tmpIPS = currentIPS
                                RunProgressBarEvent(cntCurrentConnectedImage, n, "解析処理中(3)")

                                tmpIPS = RecallConnectCamera(currentIPS.IS2.ImageId - 1, tt)
                                flgEnd = True
                            Else
                                currentIPS.IS2.ImagePose.Pose = Nothing
                                currentIPS.IS2.Quality = Nothing
                                currentIPS.IS2.CommonScale = Nothing
                                currentIPS.IS2.flgConnected = False
                            End If

                        End If
                    End If
                End If
            End If
        Next

        If flgEnd = True Then
            Return tmpIPS
        Else
            Return Nothing
        End If
    End Function
    Private Sub ConnectAllCamera(ByRef tt(,) As ImagePairSet, ByRef lstConPairs As List(Of ConnectingPair))
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim n As Integer = lstImages.Count
        Dim t As Integer = 0

        Dim currentIPS As ImagePairSet
        Dim otherIPS As ImagePairSet
        Dim flgFirst As Boolean = True
        '   Dim lstTest As New List(Of String)

        For Each CP As ConnectingPair In lstConPairs
            currentIPS = tt(CP.ind1, CP.ind2)
            If currentIPS.cntComCT >= intCommonCTnum Then

                If flgFirst = True Then
                    currentIPS.IS1.flgConnected = True
                    currentIPS.IS2.flgConnected = True
                    Dim FirstPose As New Object
                    HOperatorSet.CreatePose(0, 0, 0, 0, 0, 0, "Rp+T", "gba", "point", FirstPose)
                    currentIPS.IS1.ImagePose.Pose = FirstPose
                    currentIPS.IS2.ImagePose.Pose = currentIPS.PairPose.RelPose
                    currentIPS.IS2.VectorPose.Pose = currentIPS.PairPose.RelPose
                    currentIPS.ComScale = 1.0
                    flgFirst = False
                End If
                If currentIPS.ComScale Is Nothing Then
                    Continue For
                End If
                otherIPS = tt(CP.ind2, CP.ind3)
                If otherIPS.IS2.flgConnected = False Then
                    If otherIPS.cntComCT >= intCommonCTnum Then
                        ' If currentIPS.GetCommonCTcount(otherIPS) >= intCommonCTnum Then
                        If Connect2Pair(currentIPS, otherIPS) = True Then
                        End If
                        'End If
                    End If
                End If
            End If

        Next

    End Sub

    'コードターゲットをまとめる
    Private Sub CollectCT()
        Dim flgAri As Boolean = False
        If lstCommon3dCT Is Nothing Then
            lstCommon3dCT = New List(Of Common3DCodedTarget)
        Else
            lstCommon3dCT.Clear()
        End If

        For Each ISI As ImageSet In lstImages
            flgAri = False
            For Each CT As CodedTarget In ISI.Targets.lstCT
                flgAri = False
                For Each Com3DCT As Common3DCodedTarget In lstCommon3dCT
                    If Com3DCT.PID = CT.CT_ID Then
                        Com3DCT.lstCT.Add(CT)
                        CT.myC3DCT = Com3DCT
                        flgAri = True
                        Exit For
                    End If
                Next
                If flgAri = False Then
                    Dim C3DCT As New Common3DCodedTarget
                    C3DCT.PID = CT.CT_ID
                    C3DCT.lstCT.Add(CT)
                    CT.myC3DCT = C3DCT
                    lstCommon3dCT.Add(C3DCT)
                End If
            Next
            RunProgressBarEvent(ISI.ImageId, lstImages.Count, "解析処理中(2)")
        Next
        Dim n As Integer
        Dim i As Integer
        n = lstCommon3dCT.Count 'どんなCTの数が格納されているのか?
        For i = n - 1 To 0 Step -1
            If lstCommon3dCT.Item(i).lstCT.Count >= 2 Then '2以上じゃなかったたその要素を削除
            Else
                lstCommon3dCT.RemoveAt(i)
            End If
        Next
    End Sub

    Public Sub CalcAllImages_Ray()
        TimeMonStart()

        For Each ISI As ImageSet In lstImages
            If ISI.flgConnected = True Then
                '150115 SUURI Rep Sta 複数カメラ内部パラメータ取扱機能--------------
                'ISI.CalcRay(hv_CamparamOut)　’複数カメラ対応
                ISI.CalcRay()
                '150115 SUURI Rep End 複数カメラ内部パラメータ取扱機能--------------
            End If
        Next

        TimeMonEnd()
    End Sub

    Private Sub CalcOneImages_Ray(ByRef ISI As ImageSet)

        If ISI.flgConnected = True Then
			'150115 SUURI Rep Sta 複数カメラ内部パラメータ取扱機能--------------
            'ISI.CalcRay(hv_CamparamOut)　　’複数カメラ対応
            ISI.CalcRay()
			'150115 SUURI Rep End 複数カメラ内部パラメータ取扱機能--------------
        End If

    End Sub

    'シングルターゲットの３次元座標を算出
    Private Function CalcALLSingleTarget3dCoord() As Boolean
        CalcALLSingleTarget3dCoord = True
        Dim i As Integer
        Dim n As Integer = lstCommon3dST.Count
        For i = n - 1 To 0 Step -1
            If lstCommon3dST.Item(i).lstST.Count < 2 Or lstCommon3dST.Item(i).PID = -1 Then
                If lstCommon3dST.Item(i).lstST.Count = 1 Then
                    lstCommon3dST.Item(i).lstST.Item(0).P3ID = -1
                End If
                lstCommon3dST.RemoveAt(i)
            Else
                lstCommon3dST.Item(i).Calc3dPoints()
            End If
        Next
    End Function

    'コードターゲットの３次元座標を算出
    Private Function CalcALLCTnoSingleTarget3dCoord() As Boolean
        CalcALLCTnoSingleTarget3dCoord = True

        'For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
        '    If C3DCT.PID = 223 Then
        '        C3DCT.PID = 223
        '    End If
        '    C3DCT.Calc3dPoints()
        'Next
        Parallel.ForEach(lstCommon3dCT, Sub(C3DCT)
                                            C3DCT.Calc3dPoints()
                                        End Sub)

    End Function

    Private Function CalcThisImagesCT_3dCoord(ByVal ISI As ImageSet) As Boolean
        For Each CT As CodedTarget In ISI.Targets.lstCT
            If CT.CT_ID = 258 Then
                CT.CT_ID = 258
            End If
            CT.myC3DCT.Calc3dPoints()
        Next
    End Function
    Private Sub CalcSingleTarget(ByRef IPS12 As ImagePairSet, ByRef IPS23 As ImagePairSet, ByRef IPS13 As ImagePairSet)

        For Each ST1 As SingleTarget In IPS12.IS1.Targets.lstST
            If ST1.P3ID = 9 Then
                Dim t As Integer = 1
            End If
            Dim hv_ind_dist12 As Object = Nothing
            Dim hv_ind_dist13 As Object = Nothing
            Dim hv_ind_dist23 As Object = Nothing

            'getST_forEpiline(IPS12, ST1, hv_ind_dist12)
            'getST_forEpiline(IPS13, ST1, hv_ind_dist13)


            ' IPS12.getST_forEpiline(ST1.P2ID - 1, hv_ind_dist12)
            hv_ind_dist12 = IPS12.TaiouTenIndex.Item(ST1.P2ID - 1)

            '  IPS13.getST_forEpiline(ST1.P2ID - 1, hv_ind_dist13)
            hv_ind_dist13 = IPS13.TaiouTenIndex.Item(ST1.P2ID - 1)
            Dim i As Integer = 0
            Dim j As Integer = 0
            Dim k As Integer = 0
            If hv_ind_dist13 Is Nothing Or hv_ind_dist12 Is Nothing Then
                Exit Sub
            End If
            If BTuple.TupleSelect(hv_ind_dist12, 0) <> -1 And BTuple.TupleSelect(hv_ind_dist13, 0) <> -1 Then

                Dim flgOK As Integer = 0
                Dim ST2 As New SingleTarget
                Dim ST3 As New SingleTarget
                For i = 0 To BTuple.TupleLength(hv_ind_dist12) - 1
                    ST2 = IPS12.IS2.Targets.lstST.Item(BTuple.TupleSelect(hv_ind_dist12, i))
                    Dim tmpInd As Object = Nothing

                    ' getST_forEpiline(IPS23, ST2, hv_ind_dist23)
                    ' IPS23.getST_forEpiline(ST2.P2ID - 1, hv_ind_dist23)
                    hv_ind_dist23 = IPS23.TaiouTenIndex.Item(ST2.P2ID - 1)
                    If BTuple.TupleSelect(hv_ind_dist23, 0) <> -1 Then
                        For k = 0 To BTuple.TupleLength(hv_ind_dist13) - 1
                            For j = 0 To BTuple.TupleLength(hv_ind_dist23) - 1
                                If BTuple.TupleSelect(hv_ind_dist13, k) = BTuple.TupleSelect(hv_ind_dist23, j) Then
                                    ST3 = IPS23.IS2.Targets.lstST.Item(BTuple.TupleSelect(hv_ind_dist13, k))

                                    'If ST1.flgUsed = 0 Then
                                    '    If ST2.flgUsed = 1 Or ST3.flgUsed = 1 Then
                                    '        Continue For
                                    '    End If
                                    'End If
                                    flgOK = 1
                                    GoTo labelGo
                                End If
                            Next
                        Next
                    End If
                Next
labelGo:

                If flgOK = 0 Then
                    Continue For
                End If



                AddAndUnionToCommonST(ST1, ST2, ST3)
                '' Dim pnt As New Point3D
                'If ST1.flgUsed = 0 Then
                '    'Dim samepointid As Integer = -1
                '    'Dim lsttmpST As New List(Of SingleTarget)
                '    'lsttmpST.Add(ST1)
                '    'lsttmpST.Add(ST2)
                '    'lsttmpST.Add(ST3)
                '    'CalcNearest3dPointofRays(lsttmpST, pnt)
                '    'lsttmpST.Clear()
                '    ''Dim MP As New MeasurePoint(lstImages, IPS12.IS1_ID - 1, IPS12.IS2_ID - 1)
                '    ''MP.IS1_ImagePoint.Row = ST1.P2D.Row
                '    ''MP.IS1_ImagePoint.Col = ST1.P2D.Col
                '    ''MP.IS2_ImagePoint.Row = ST2.P2D.Row
                '    ''MP.IS2_ImagePoint.Col = ST2.P2D.Col
                '    ''MP.Calc3dCoord(hv_CamparamOut)

                '    'If check_same_point(pnt, samepointid) = True Then
                '    '    ST1.flgUsed = 1
                '    '    ST1.P3ID = samepointid
                '    '    lstCommon3dTargets.Item(ST1.P3ID - 1).lstST.Add(ST1)
                '    'Else
                '    '    Dim obj3dTargets As New Common3DTarget
                '    '    obj3dTargets.P3d = New Point3D(pnt)
                '    '    obj3dTargets.PID = lstCommon3dTargets.Count + 1
                '    '    ST1.flgUsed = 1
                '    '    ST1.P3ID = obj3dTargets.PID
                '    '    obj3dTargets.lstST.Add(ST1)
                '    '    lstCommon3dTargets.Add(obj3dTargets)

                '    'End If
                '    Dim obj3dTargets As New Common3DSingleTarget
                '    ' obj3dTargets.P3d = New Point3D(pnt)
                '    obj3dTargets.PID = lstCommon3dST.Count + 1
                '    ST1.flgUsed = 1
                '    ST1.P3ID = obj3dTargets.PID
                '    obj3dTargets.lstST.Add(ST1)
                '    lstCommon3dST.Add(obj3dTargets)
                '    'lstCommon3dST.Item(0).lstST.Item(0).P3ID = 1
                '    If ST1.P3ID = 9 Then
                '        Dim t As Integer = 1
                '    End If
                'End If
                'If ST2.flgUsed = 0 Then
                '    ST2.flgUsed = 1
                '    ST2.P3ID = ST1.P3ID
                '    For Each ST As SingleTarget In lstCommon3dST.Item(ST1.P3ID - 1).lstST
                '        If ST2.ImageID = ST.ImageID Then
                '            flgOK = 0
                '            Exit For
                '        End If
                '    Next
                '    If flgOK = 1 Then
                '        lstCommon3dST.Item(ST1.P3ID - 1).lstST.Add(ST2)
                '    End If

                '    If ST1.P3ID = 9 Then
                '        Dim t As Integer = 1
                '    End If
                'End If
                'If ST3.flgUsed = 0 Then
                '    ST3.flgUsed = 1
                '    ST3.P3ID = ST1.P3ID
                '    For Each ST As SingleTarget In lstCommon3dST.Item(ST1.P3ID - 1).lstST
                '        If ST3.ImageID = ST.ImageID Then
                '            flgOK = 0
                '            Exit For
                '        End If
                '    Next
                '    If flgOK = 1 Then
                '        lstCommon3dST.Item(ST1.P3ID - 1).lstST.Add(ST3)
                '    End If

                '    If ST1.P3ID = 9 Then
                '        Dim t As Integer = 1
                '    End If
                'End If
            End If

        Next
    End Sub
    Private Sub AddAndUnionToCommonST(ByRef ST1 As SingleTarget, ByRef ST2 As SingleTarget, ByRef ST3 As SingleTarget)

        If ST1.flgUsed = 0 And ST2.flgUsed = 0 And ST3.flgUsed = 1 Then
            AddtoCommon3DSingleTargets(ST3, ST1)
            AddtoCommon3DSingleTargets(ST3, ST2)
            Exit Sub
        End If
        If ST1.flgUsed = 0 And ST2.flgUsed = 1 And ST3.flgUsed = 0 Then
            AddtoCommon3DSingleTargets(ST2, ST1)
            AddtoCommon3DSingleTargets(ST2, ST3)
            Exit Sub
        End If
        If ST1.flgUsed = 0 And ST2.flgUsed = 1 And ST3.flgUsed = 1 Then
            '    UnionCommonSTtoST(ST2, ST3)
            AddtoCommon3DSingleTargets(ST2, ST1)
            AddtoCommon3DSingleTargets(ST3, ST1)
            Exit Sub
        End If
        If ST1.flgUsed = 1 And ST2.flgUsed = 0 And ST3.flgUsed = 0 Then
            AddtoCommon3DSingleTargets(ST1, ST2)
            AddtoCommon3DSingleTargets(ST1, ST3)
            Exit Sub
        End If
        If ST1.flgUsed = 1 And ST2.flgUsed = 0 And ST3.flgUsed = 1 Then
            '   UnionCommonSTtoST(ST1, ST3)
            AddtoCommon3DSingleTargets(ST1, ST2)
            AddtoCommon3DSingleTargets(ST3, ST2)
            Exit Sub
        End If
        If ST1.flgUsed = 1 And ST2.flgUsed = 1 And ST3.flgUsed = 0 Then
            '  UnionCommonSTtoST(ST1, ST2)
            AddtoCommon3DSingleTargets(ST1, ST3)
            AddtoCommon3DSingleTargets(ST2, ST3)
            Exit Sub
        End If
        If ST1.flgUsed = 1 And ST2.flgUsed = 1 And ST3.flgUsed = 1 Then
            '  UnionCommonSTtoST(ST1, ST2)
            '  UnionCommonSTtoST(ST1, ST3)
            Exit Sub
        End If
        If ST1.flgUsed = 0 And ST2.flgUsed = 0 And ST3.flgUsed = 0 Then
            Dim obj3dTargets As New Common3DSingleTarget
            obj3dTargets.PID = lstCommon3dST.Count + 1
            ST1.flgUsed = 1
            ST1.P3ID = obj3dTargets.PID
            obj3dTargets.lstST.Add(ST1)
            lstCommon3dST.Add(obj3dTargets)
            AddtoCommon3DSingleTargets(ST1, ST2)
            AddtoCommon3DSingleTargets(ST1, ST3)
        End If

    End Sub
    Private Sub getST_forEpiline(ByRef IPS As ImagePairSet, ByRef ST As SingleTarget, ByRef hv_ind_dist As Object)
        Dim line12 As New ImageLine
        Dim hv_dist12 As Object = Nothing
        gen_epiline(IPS.PairFMat, hv_Width, hv_Height, ST.P2D.Row, ST.P2D.Col, _
                     line12.R1, line12.C1, line12.R2, line12.C2)
        '  Dim strtemp As String = ""
        ' strtemp = strtemp & ST.P2D.Row & "," & ST.P2D.Col & "," & line12.R1 & "," & line12.C1 & "," & line12.R2 & "," & line12.C2 & vbNewLine
        'My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\EpilineMonitor.csv", strtemp, True)
        HOperatorSet.DistancePl(IPS.IS2.Targets.All2D_ST.Row, IPS.IS2.Targets.All2D_ST.Col, line12.R1, line12.C1, line12.R2, line12.C2, hv_dist12)
        ' HOperatorSet.TupleFind(BTuple.TupleSgn(BTuple.TupleSub(hv_dist12, 2)), -1, hv_ind_dist)
        hv_ind_dist = BTuple.TupleSelect(BTuple.TupleSortIndex(hv_dist12), 0)
        If BTuple.TupleSelect(hv_dist12, hv_ind_dist) > 1 Then
            hv_ind_dist = -1
        End If
    End Sub

    Private Function Connect2Pair(ByRef currentIPS As ImagePairSet, ByRef otherIPS As ImagePairSet) As Boolean
        Connect2Pair = False
        Dim CurCT3dPoint As New Point3D
        Dim OtherCT3dPoint As New Point3D
        Dim BestScale As New Object
        Dim RelPose As New Object
        Dim tmpRelPose As Object = Nothing
        Dim tmpdiffRelPose As Object = Nothing
        Dim tmpDist3d As Object = Nothing
        Dim dblBaseLine As New Object
        Dim ConPose As Object = Nothing
        Dim homMat3d As Object = Nothing
        If otherIPS.IS2.ImageId = 36 Then
            Dim tttt As Integer = 1
        End If
        'ペアセット間の共通点を抽出
        If currentIPS.GetCommonCT3dPoint_of_2Pair(otherIPS, CurCT3dPoint, OtherCT3dPoint) = True Then
            '座標変換
            If otherIPS.IS1_ID = currentIPS.IS2_ID Then
                ZahyoHenkan(currentIPS.RelPose(True), CurCT3dPoint, CurCT3dPoint)
            End If
            'ベストスケールを計算
            'CalcBestScaleWithWeight(CurCT3dPoint, OtherCT3dPoint, 10, BestScale)
            CalcBestScale(CurCT3dPoint, OtherCT3dPoint, BestScale)
            OtherCT3dPoint.SetScale(BestScale)
            OtherCT3dPoint.GetDisttoOtherPose(CurCT3dPoint, tmpdiffRelPose)
            'hom_mat_3d_from_3d_3d_point_correspondence(OtherCT3dPoint.X, OtherCT3dPoint.Y, OtherCT3dPoint.Z, CurCT3dPoint.X, CurCT3dPoint.Y, CurCT3dPoint.Z, homMat3d)
            ''correspond_3d_3d_point_withweight(IS3.MidPose.X, IS3.MidPose.Y, IS3.MidPose.Z, IS2.MidPose.X, IS2.MidPose.Y, IS2.MidPose.Z, 10, 0.00001, homMat3d)
            'HOperatorSet.AffineTransPoint3D(homMat3d, OtherCT3dPoint.X, OtherCT3dPoint.Y, OtherCT3dPoint.Z, OtherCT3dPoint.X, OtherCT3dPoint.Y, OtherCT3dPoint.Z)

            'OtherCT3dPoint.GetDisttoOtherPose(CurCT3dPoint, tmpdiffRelPose)
            '結合
            If otherIPS.IS1_ID = currentIPS.IS2_ID Then
                ConnectPose(currentIPS.IS2.ImagePose.Pose, otherIPS.RelPose(True), currentIPS.ComScale * BestScale, ConPose)
            End If
            If otherIPS.IS1_ID = currentIPS.IS1_ID Then
                ConnectPose(currentIPS.IS1.ImagePose.Pose, otherIPS.RelPose(True), currentIPS.ComScale * BestScale, ConPose)
            End If

            Dim strText As String = CStr(currentIPS.IS1_ID & ",と," & currentIPS.IS2_ID & ",に対して," & otherIPS.IS1_ID & ",と," & otherIPS.IS2_ID)

            '結合後の結果を代入
            If otherIPS.IS2.flgConnected = False Then
                otherIPS.IS2.ImagePose.Pose = ConPose
                'otherIPS.IS2.CalcRay(). chi 
                otherIPS.IS2.VectorPose.Pose = otherIPS.RelPose(True)
                otherIPS.IS2.CommonScale = currentIPS.IS2.CommonScale * BestScale
                otherIPS.IS2.MeanError = BTuple.TupleMean(tmpdiffRelPose)
                otherIPS.IS2.DevError = BTuple.TupleMax(tmpdiffRelPose)
                otherIPS.IS2.MidCount = BTuple.TupleLength(CurCT3dPoint.X)
                otherIPS.IS2.flgConnected = True
            End If
            If otherIPS.IS2.flgConnected = True Then
                If otherIPS.IS2.MeanError > BTuple.TupleMean(tmpdiffRelPose) And otherIPS.IS2.DevError > BTuple.TupleMax(tmpdiffRelPose) And _
                         otherIPS.IS2.MidCount < BTuple.TupleLength(CurCT3dPoint.X) Then
                    otherIPS.IS2.ImagePose.Pose = ConPose
                    'otherIPS.IS2.CalcRay(). chi 
                    otherIPS.IS2.VectorPose.Pose = otherIPS.RelPose(True)
                    otherIPS.IS2.CommonScale = currentIPS.IS2.CommonScale * BestScale
                    otherIPS.IS2.MeanError = BTuple.TupleMean(tmpdiffRelPose)
                    otherIPS.IS2.DevError = BTuple.TupleMax(tmpdiffRelPose)
                    otherIPS.IS2.MidCount = BTuple.TupleLength(CurCT3dPoint.X)
                End If
            End If

            'ペアセットのコモンスケールを計算
            If otherIPS.ComScale Is Nothing Then
                otherIPS.ComScale = currentIPS.ComScale * BestScale
            End If
            ' If otherIPS.IS1_ID = currentIPS.IS1_ID Then
            '  otherIPS.IS2.lstImagePose.Add(BTuple.TupleConcat(BTuple.TupleConcat(strText, ConPose), currentIPS.ComScale * BestScale))
            otherIPS.IS2.lstImagePose.Add(ConPose)
            'End If


        Else
            Exit Function
        End If
        Connect2Pair = True
    End Function


    Private Function Connect2PairByVector(ByRef currentIPS As ImagePairSet, ByRef otherIPS As ImagePairSet) As Boolean
        Connect2PairByVector = False
        Dim CurCT3dPoint As New Point3D
        Dim OtherCT3dPoint As New Point3D
        Dim BestScale As New Object
        Dim RelPose As New Object
        Dim tmpRelPose As Object = Nothing
        Dim tmpdiffRelPose As Object = Nothing
        Dim tmpDist3d As Object = Nothing
        Dim dblBaseLine As New Object
        Dim ConPose As Object = Nothing
        Dim PoseByVector As Object = Nothing
        Dim Quality As Object = Nothing
        Dim homMat3d As Object = Nothing
        If otherIPS.IS2.ImageId = 36 Then
            Dim tttt As Integer = 1
        End If
        Connect2PairByVector = False
        If currentIPS.CalcPoseByCommonCT3dPoint(otherIPS, lstCommon3dCT, PoseByVector, Quality) = True Then


            CalcALLCodedTarget3dCoordByHalcon()
            ConPose = PoseByVector
            '結合後の結果を代入
            If otherIPS.IS2.flgConnected = False Then
                otherIPS.IS2.ImagePose.Pose = ConPose
                otherIPS.IS2.Quality = Quality
                'otherIPS.IS2.CalcRay(). chi 
                otherIPS.IS2.VectorPose.Pose = otherIPS.RelPose(True)
                otherIPS.IS2.CommonScale = 1
                'otherIPS.IS2.MeanError = BTuple.TupleMean(tmpdiffRelPose)
                'otherIPS.IS2.DevError = BTuple.TupleMax(tmpdiffRelPose)
                'otherIPS.IS2.MidCount = BTuple.TupleLength(CurCT3dPoint.X)
                otherIPS.IS2.flgConnected = True
            End If
            If otherIPS.IS2.flgConnected = True Then
                If otherIPS.IS2.Quality > Quality Then
                    otherIPS.IS2.ImagePose.Pose = ConPose
                    otherIPS.IS2.Quality = Quality
                    'otherIPS.IS2.CalcRay(). chi 
                    otherIPS.IS2.VectorPose.Pose = otherIPS.RelPose(True)
                    otherIPS.IS2.CommonScale = 1
                    'otherIPS.IS2.MeanError = BTuple.TupleMean(tmpdiffRelPose)
                    'otherIPS.IS2.DevError = BTuple.TupleMax(tmpdiffRelPose)
                    'otherIPS.IS2.MidCount = BTuple.TupleLength(CurCT3dPoint.X)
                End If
            End If

            'ペアセットのコモンスケールを計算
            If otherIPS.ComScale Is Nothing Then
                otherIPS.ComScale = 1
            End If
            ' If otherIPS.IS1_ID = currentIPS.IS1_ID Then
            '  otherIPS.IS2.lstImagePose.Add(BTuple.TupleConcat(BTuple.TupleConcat(strText, ConPose), currentIPS.ComScale * BestScale))
            otherIPS.IS2.lstImagePose.Add(ConPose)
            'End If
        Else
            Exit Function
        End If
        Connect2PairByVector = True
    End Function


    Private Function check_same_point(ByVal XYZ As Point3D, ByRef MinID As Integer) As Boolean
        Dim minDist As Object = Nothing
        SearchNearestPoint(XYZ, minDist, MinID)
        If minDist < 0.01 Then
            check_same_point = True
        Else
            check_same_point = False
        End If

    End Function

    Private Sub SearchNearestPoint(ByVal XYZ As Point3D, ByRef minDist As Object, ByRef minID As Integer)
        Dim dist As Object = Nothing
        minDist = 9999999

        For Each C3DT As Common3DSingleTarget In lstCommon3dST
            C3DT.P3d.GetDisttoOtherPose(XYZ, dist)
            If minDist > dist Then
                minDist = dist
                minID = C3DT.PID
            End If
        Next

    End Sub


    Public Sub DebugCT3DPoint(ByRef IPS As ImagePairSet)

#If DEBUG Then
        Dim j As Integer = 0
        Dim k As Integer = 0
        For Each IS1_CT As CodedTarget In IPS.lstIS1_ComCT
            Dim XYZ As New Point3D
            Dim IS2_CT As CodedTarget

            IS2_CT = IPS.lstIS2_ComCT.Item(j)
            j += 1
            Try
                HOperatorSet.IntersectLinesOfSight(hv_CamparamOut, hv_CamparamOut, IPS.PairPose.RelPose, _
                                         IS1_CT.CT_Points.Row, IS1_CT.CT_Points.Col, IS2_CT.CT_Points.Row, IS2_CT.CT_Points.Col, _
                                         XYZ.X, XYZ.Y, XYZ.Z, XYZ.MyDist)
            Catch ex As Exception
                Continue For
            End Try
            XYZ.SetScale(IPS.ComScale)
            'ZahyoHenkan(IPS.IS1.ImagePose.Pose, XYZ, XYZ)
            Dim HomMat3d As New Object
            HOperatorSet.PoseToHomMat3d(BTuple.TupleFirstN(IPS.IS1.ImagePose.Pose, 6), HomMat3d)
            HOperatorSet.AffineTransPoint3D(HomMat3d, XYZ.X, XYZ.Y, XYZ.Z, XYZ.X, XYZ.Y, XYZ.Z)

            For k = 0 To 3
                Dim strTemp As String = ""
                strTemp = strTemp & ",CT_" & CStr(IS1_CT.CT_ID) & "_" & k + 1 & "," & _
                XYZ.X(k).D & "," & XYZ.Y(k).D & "," & XYZ.Z(k).D & "," & IPS.IS1.ImageId & "," & IPS.IS2.ImageId
                Trace.WriteLine(strTemp)
            Next

        Next
#End If

    End Sub

    Private Sub ReCalcVectorPose()
        Dim IS1 As ImageSet
        Dim IS2 As ImageSet
        Dim i As Integer
        Dim n As Integer = lstImages.Count
        For i = 0 To n - 2
            IS1 = lstImages.Item(i)
            IS2 = lstImages.Item(i + 1)
            CalcRelPoseBetweenTwoPose(IS1.ImagePose.Pose, IS2.ImagePose.Pose, IS2.VectorPose.Pose)
            If IS1.flgConnected = False Then
                MsgBox(IS1.ImageName & "が結合されていません", MsgBoxStyle.OkOnly, "確認")
            End If
            If IS2.flgConnected = False Then
                MsgBox(IS2.ImageName & "が結合されていません", MsgBoxStyle.OkOnly, "確認")
            End If
        Next
    End Sub

    Private Sub ReCalcVectorPose(ByVal T(,) As ImagePairSet)
        Dim IS1 As ImageSet
        Dim IS2 As ImageSet
        Dim i As Integer
        Dim n As Integer = lstImages.Count
        Dim dblBaseLine As Double
        Dim pose As New Object
        For i = 0 To n - 2
            IS1 = lstImages.Item(i)
            IS2 = lstImages.Item(i + 1)
            CalcRelPoseBetweenTwoPose(IS1.ImagePose.Pose, IS2.ImagePose.Pose, pose)
            dblBaseLine = BTuple.TupleSqrt(BTuple.TupleSum(BTuple.TupleFirstN(BTuple.TuplePow(pose, 2), 2)))
            Dim tmpdiffRelPose As New Object
            tmpdiffRelPose = pose
            CalcUnitPose(pose, tmpdiffRelPose)
            tmpdiffRelPose = BTuple.TupleSub(pose, T(i, i + 1).PairPose.RelPose)
            ' CalcRelPoseBetweenTwoPose(IS1.ImagePose.Pose, IS2.ImagePose.Pose, pose)
            IS2.VectorPose.Pose = pose
            IS2.CommonScale = dblBaseLine
            If IS1.flgConnected = False Then
                MsgBox(IS1.ImageName & "が結合されていません", MsgBoxStyle.OkOnly, "確認")
            End If
            If IS2.flgConnected = False Then
                MsgBox(IS2.ImageName & "が結合されていません", MsgBoxStyle.OkOnly, "確認")
            End If
        Next
    End Sub

    '仮のスケールを算出：（CT内のST間の計測距離の合計）/（CT内のST間の設計距離の合計）
    Public Sub CalcKarinoScaleByCTPoints(ByRef Scale As Double)
        TimeMonStart()

        Dim hv_ALLCTID As New Object
        Dim i As Integer
        Dim j As Integer
        Dim PkanKyori As Double = 10
        hv_ALLCTID = BTuple.ReadTuple(My.Application.Info.DirectoryPath & "\RectangleCT_ID.tup")
        PkanKyori = GetPrivateProfileInt("Kaiseki", "CTsize", 5, My.Application.Info.DirectoryPath & "\vform.ini")

        'Dim Dummy(4, 4) As ImagePoints
        Dim Dummy2ten5(4, 4) As ImagePoints
        Dim Dummy5(4, 4) As ImagePoints
        Dim Dummy10(4, 4) As ImagePoints
        For i = 0 To 5 - 1
            For j = 0 To 5 - 1
                Dim D As New ImagePoints
                D.Row = i * PkanKyori
                D.Col = j * PkanKyori

                Dummy10(i, j) = D

            Next
        Next
        'PkanKyori = 5
        'For i = 0 To 5 - 1
        '    For j = 0 To 5 - 1
        '        Dim D As New ImagePoints
        '        D.Row = i * PkanKyori
        '        D.Col = j * PkanKyori

        '        Dummy5(i, j) = D

        '    Next
        'Next
        'PkanKyori = 2.5
        'For i = 0 To 5 - 1
        '    For j = 0 To 5 - 1
        '        Dim D As New ImagePoints
        '        D.Row = i * PkanKyori
        '        D.Col = j * PkanKyori

        '        Dummy2ten5(i, j) = D

        '    Next
        'Next
        Dim Soukyori As Double = 0
        Dim SouKyori3d As Double = 0
        For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
            Dim CTPointhaiti As Object = BTuple.TupleSelect(hv_ALLCTID, C3DCT.PID - 1)
            Dim CTHaiti(CodedTarget.CTnoSTnum - 1) As String
            CTHaiti = CStr(CTPointhaiti.S).Split("_")
            Dim II As Integer = CInt(CTHaiti(0)) / 5
            Dim JJ As Integer = CInt(CTHaiti(0)) Mod 5
            Dim P1 As ImagePoints '= Dummy(II, JJ)

            P1 = Dummy10(II, JJ)

            Dim P2 As New ImagePoints
            Dim P12_kyori As Double
            If C3DCT.lstP3d.Count = 0 Then
                Continue For
            End If
            Dim P3d1 As Point3D = C3DCT.lstP3d.Item(0)
            Dim P3d2 As New Point3D
            Dim P3d12_kyori As HTuple = Nothing
            'Dim CheckCT(CodedTarget.CTnoSTnum - 1)

            For i = 1 To CodedTarget.CTnoSTnum - 1
                II = CInt(CInt(CTHaiti(i)) / 5) - 1

                JJ = IIf((CInt(CTHaiti(i)) Mod 5) = 0, 4, (CInt(CTHaiti(i)) Mod 5) - 1)

                P2 = Dummy10(II, JJ)

                P1.CalcDistToInputPoint(P2.Row, P2.Col, P12_kyori)
                Soukyori += P12_kyori
                P3d2 = C3DCT.lstP3d(i)
                P3d1.GetDisttoOtherPose(P3d2, P3d12_kyori)
                SouKyori3d += P3d12_kyori
                ' CheckCT(i) = P12_kyori / P3d12_kyori
            Next
        Next
        Scale = SouKyori3d / Soukyori

        TimeMonEnd()
    End Sub

    Public Function GetMaxTID() As Integer
        GetMaxTID = 10000
        For Each C3DST As Common3DSingleTarget In lstCommon3dST
            If GetMaxTID < C3DST.TID Then
                GetMaxTID = C3DST.TID
            End If
        Next
    End Function
#End Region

#Region "YCMDB関連"
    Public Sub ReadFromMeasureDataDB(ByVal strDBfilePath As String)

        ReadFromYCMDB_MeasurePoint3d(strDBfilePath)
        AccessDisConnect()

    End Sub
    Private Sub ReadFromYCMDB_MeasurePoint3d(ByVal strDBfilePath As String)

        Dim flgConnected As Integer
        flgConnected = AccessConnect(dbClass, strDBfilePath & "\", YCM_MDB)
        If flgConnected = -1 Then
            MsgBox("Access(" & YCM_MDB & ")に接続できませんでした。", MsgBoxStyle.OkOnly, "確認")
            Exit Sub
        End If

        For Each C3DST As Common3DSingleTarget In lstCommon3dST
            If C3DST.PID <> -1 Then
                Dim strSql As String
                Dim IDR As IDataReader
                strSql = "SELECT X,Y,Z FROM measurepoint3d WHERE TID = " & (10000 + C3DST.PID)
                IDR = dbClass.DoSelect(strSql)
                If Not IDR Is Nothing Then
                    IDR.Read()
                    Dim readP3d As New Point3D
                    Try
                        readP3d.X = CDbl(IDR.GetValue(0))
                        readP3d.Y = CDbl(IDR.GetValue(1))
                        readP3d.Z = CDbl(IDR.GetValue(2))
                        C3DST.P3d.CopyToMe(readP3d)
                    Catch ex As Exception

                    End Try
                   

                    IDR.Close()
                End If
            End If
        Next

        For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
            If C3DCT.PID = 10 Then
                C3DCT.PID = C3DCT.PID
            End If
            If C3DCT.PID <> -1 Then
                Dim strSql As String
                Dim IDR As IDataReader
                strSql = "SELECT X,Y,Z FROM measurepoint3d WHERE TID = " & C3DCT.PID & " ORDER BY systemlabel"
                IDR = dbClass.DoSelect(strSql)
                If Not IDR Is Nothing Then
                    Do While IDR.Read
                        Dim readP3d As New Point3D
                        readP3d.X = CDbl(IDR.GetValue(0))
                        readP3d.Y = CDbl(IDR.GetValue(1))
                        readP3d.Z = CDbl(IDR.GetValue(2))
                        C3DCT.lstP3d.Add(readP3d)
                        C3DCT.flgUsable = True
                    Loop
                    'C3DCT.flgUsable = True
                    IDR.Close()
                End If
            End If
        Next

    End Sub
    Public Sub SaveToMeasureDataDB(ByVal strDBfilePath As String)
        TimeMonStart()

        If ConnectDbYCM(strDBfilePath) = True Then
            SaveToYCMDB_MeasurePoint3d(True)
            SaveToYCMDB_CameraPose(True)
            SaveToYCMDB_Ray(True)
            AccessDisConnect()
        End If

        TimeMonEnd()
    End Sub

    Public Sub SaveToMeasureDataDBNewPoint(ByVal strDBfilePath As String)

        If ConnectDbYCM(strDBfilePath) = True Then
            SaveToYCMDB_MeasurePoint3d(True)
            ' SaveToYCMDB_CameraPose(True)
            SaveToYCMDB_Ray(True)
            AccessDisConnect()
        End If

    End Sub

    Private Sub SaveToYCMDB_MeasurePoint3d(ByVal flgSyori As Boolean)
        If flgSyori = True Then
            dbClass.DoDelete("measurepoint3d")
        End If
        Dim index = 1 '20170223 baluu add
        For Each C3DST As Common3DSingleTarget In lstCommon3dST
            If C3DST.PID <> -1 And Not C3DST.P3d.X Is Nothing Then
                Dim meanerror As Double = Nothing
                Dim deverror As Double = Nothing
                If Not C3DST.meanerror Is Nothing Then
                    meanerror = C3DST.meanerror.D
                End If
                If Not C3DST.deverror Is Nothing Then
                    deverror = C3DST.deverror.D
                End If
                If UpdateMeasurePoint3d(C3DST.TID, C3DST.systemlabel, TargetType.SingleTarget, C3DST.P3d, meanerror, deverror, 1) <= 0 Then
                    If InsertMeasurePoint3d(C3DST.TID, C3DST.systemlabel, C3DST.currentLabel, TargetType.SingleTarget, C3DST.P3d, meanerror, deverror, 1, index) <= 0 Then '20170223 baluu add
                        'If InsertMeasurePoint3d(C3DST.TID, C3DST.systemlabel, C3DST.currentLabel, TargetType.SingleTarget, C3DST.P3d, CDbl(C3DST.meanerror), CDbl(C3DST.deverror), 1) <= 0 Then '20170223 baluu del
                        Continue For
                    End If
                End If
            End If
        Next
        index = 1 '20170223 baluu add
        For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
            If C3DCT.PID <> -1 And C3DCT.flgUsable = True Then
                Dim strSystemLabel As String
                Dim strCurrentLabel As String
                Dim flgLabel As Integer = 1
                Dim i As Integer = 0
                For Each P3d As Point3D In C3DCT.lstP3d
                    strSystemLabel = C3DCT.systemlabel
                    strCurrentLabel = C3DCT.currentLabel
                    If i <> 0 Then
                        strSystemLabel = strSystemLabel & "_" & i + 1
                        strCurrentLabel = strSystemLabel '1CTに4STのラベル
                        flgLabel = 0
                    End If
                    Dim meanerror As Double = Nothing
                    Dim deverror As Double = Nothing
                    If Not C3DCT.meanerror Is Nothing Then
                        meanerror = C3DCT.meanerror.D
                    End If
                    If Not C3DCT.deverror Is Nothing Then
                        deverror = C3DCT.deverror.D
                    End If

                    If UpdateMeasurePoint3d(C3DCT.PID, strSystemLabel, TargetType.CodedTarget, P3d, meanerror, deverror, flgLabel) <= 0 Then
                        If InsertMeasurePoint3d(C3DCT.PID, strSystemLabel, strCurrentLabel, TargetType.CodedTarget, P3d, meanerror, deverror, flgLabel, index) <= 0 Then '20170223 baluu add
                            'If InsertMeasurePoint3d(C3DCT.PID, strSystemLabel, strCurrentLabel, TargetType.CodedTarget, P3d, C3DCT.meanerror, C3DCT.deverror, flgLabel, index) <= 0 Then  '20170223 baluu del
                            Exit For
                        End If
                    End If
                    i += 1
                Next
            End If
        Next
    End Sub

    Private Function UpdateMeasurePoint3d(ByVal TID As Integer, ByVal strSystemLabel As String, _
                                          ByVal intType As Integer, ByVal P3d As Point3D, _
                                          ByVal dblMeanError As Double, ByVal dblDevError As Double, _
                                          ByVal flgLabel As Integer) As Long
        Dim strFieldName(7) As String
        Dim strFieldData(7) As String
        strFieldName(0) = "TID"
        strFieldName(1) = "Type"
        strFieldName(2) = "X"
        strFieldName(3) = "Y"
        strFieldName(4) = "Z"
        strFieldName(5) = "meanerror"
        strFieldName(6) = "deverror"
        strFieldName(7) = "flglabel"

        strFieldData(0) = CStr(TID)
        strFieldData(1) = CStr(intType)
        If Not P3d.X.Type = HTupleType.EMPTY Then
            strFieldData(2) = CStr(P3d.X.D)
            strFieldData(3) = CStr(P3d.Y.D)
            strFieldData(4) = CStr(P3d.Z.D)
        End If

        strFieldData(5) = CStr(dblMeanError)
        strFieldData(6) = CStr(dblDevError)
        strFieldData(7) = CStr(flgLabel)
        UpdateMeasurePoint3d = dbClass.DoUpdate(strFieldName, "measurepoint3d", strFieldData, "systemlabel = " & "'" & strSystemLabel & "'")

    End Function

    Private Function InsertMeasurePoint3d(ByVal TID As Integer, ByVal strSystemLabel As String, ByVal strCurrentLabel As String, _
                                          ByVal intType As Integer, ByVal P3d As Point3D, _
                                          ByVal dblMeanError As Double, ByVal dblDevError As Double, _
                                          ByVal flgLabel As Integer, ByRef index As Integer) As Long
        Dim strFieldName(10) As String
        Dim strFieldData(10) As String
        'Dim maxid As Long = GETMAXID("measurepoint3d") '20170223 baluu del

        strFieldName(0) = "TID"
        strFieldName(1) = "Type"
        strFieldName(2) = "X"
        strFieldName(3) = "Y"
        strFieldName(4) = "Z"
        strFieldName(5) = "meanerror"
        strFieldName(6) = "deverror"
        strFieldName(7) = "flglabel"
        strFieldName(8) = "systemlabel"
        strFieldName(9) = "currentlabel"
        strFieldName(10) = "ID"

        strFieldData(0) = CStr(TID)
        strFieldData(1) = CStr(intType)
        If Not P3d.X.Type = HTupleType.EMPTY Then
            strFieldData(2) = CStr(P3d.X.D)
            strFieldData(3) = CStr(P3d.Y.D)
            strFieldData(4) = CStr(P3d.Z.D)
        End If

        strFieldData(5) = CStr(dblMeanError)
        strFieldData(6) = CStr(dblDevError)
        strFieldData(7) = CStr(flgLabel)
        strFieldData(8) = "'" & strSystemLabel & "'"
        strFieldData(9) = "'" & strCurrentLabel & "'"
        '20170223 baluu add start
        If intType = TargetType.CodedTarget Then
            strFieldData(10) = index
            index = index + 1
        Else
            strFieldData(10) = CStr(TID)
        End If
        '20170223 baluu add end
        'strFieldData(10) = maxid + 1 '20170223 baluu del

        InsertMeasurePoint3d = dbClass.DoInsert(strFieldName, "measurepoint3d", strFieldData)

    End Function

    Private Function GETMAXID(ByVal strTableName As String) As Long
        Dim strSQLTEXT As String
        strSQLTEXT = "SELECT MAX(ID) as MAX_ID FROM " & strTableName
        Dim IDR As IDataReader

        IDR = dbClass.DoSelect(strSQLTEXT)

        If Not IDR Is Nothing Then

            Do While IDR.Read
                If IDR.GetValue(0) Is Nothing Then
                    GETMAXID = 0
                Else
                    If IDR.GetValue(0) Is DBNull.Value Then
                        GETMAXID = 0
                    Else
                        GETMAXID = CLng(IDR.GetValue(0))
                    End If
                End If
            Loop
            IDR.Close()
        End If

    End Function

    Private Sub SaveToYCMDB_CameraPose(ByVal flgSyori As Boolean)
        If flgSyori = True Then
            dbClass.DoDelete("camerapos")
        End If
        For Each ISI As ImageSet In lstImages
            If ISI.flgConnected = True Then
                If UpdateCameraPose(ISI) <= 0 Then
                    If InsertCameraPose(ISI) <= 0 Then
                        Continue For
                    End If
                End If
            End If
        Next
    End Sub

    Private Function UpdateCameraPose(ByVal ISI As ImageSet) As Long
        Dim strFieldName(9) As String
        Dim strFieldData(9) As String
        strFieldName(0) = "ID"
        strFieldName(1) = "imagefilename"
        strFieldName(2) = "CX"
        strFieldName(3) = "CY"
        strFieldName(4) = "CZ"
        strFieldName(5) = "CA"
        strFieldName(6) = "CB"
        strFieldName(7) = "CG"
        strFieldName(8) = "flgNotUse"
        strFieldName(9) = "flgSystemNotUse"

        strFieldData(0) = ISI.ImageId
        strFieldData(1) = "'" & ISI.ImageName & "'"
        strFieldData(2) = ISI.ImagePose.Pose(0).D
        strFieldData(3) = ISI.ImagePose.Pose(1).D
        strFieldData(4) = ISI.ImagePose.Pose(2).D
        strFieldData(5) = ISI.ImagePose.Pose(3).D
        strFieldData(6) = ISI.ImagePose.Pose(4).D
        strFieldData(7) = ISI.ImagePose.Pose(5).D
        strFieldData(8) = "0"
        strFieldData(9) = "0"

        UpdateCameraPose = dbClass.DoUpdate(strFieldName, "camerapos", strFieldData, "ID = " & ISI.ImageId)
    End Function

    Private Function InsertCameraPose(ByVal ISI As ImageSet) As Long
        Dim strFieldName(9) As String
        Dim strFieldData(9) As String
        strFieldName(0) = "ID"
        strFieldName(1) = "imagefilename"
        strFieldName(2) = "CX"
        strFieldName(3) = "CY"
        strFieldName(4) = "CZ"
        strFieldName(5) = "CA"
        strFieldName(6) = "CB"
        strFieldName(7) = "CG"
        strFieldName(8) = "flgNotUse"
        strFieldName(9) = "flgSystemNotUse"

        strFieldData(0) = ISI.ImageId
        strFieldData(1) = "'" & ISI.ImageName & "'"
        strFieldData(2) = ISI.ImagePose.Pose(0).D
        strFieldData(3) = ISI.ImagePose.Pose(1).D
        strFieldData(4) = ISI.ImagePose.Pose(2).D
        strFieldData(5) = ISI.ImagePose.Pose(3).D
        strFieldData(6) = ISI.ImagePose.Pose(4).D
        strFieldData(7) = ISI.ImagePose.Pose(5).D
        strFieldData(8) = "0"
        strFieldData(9) = "0"

        InsertCameraPose = dbClass.DoInsert(strFieldName, "camerapos", strFieldData)

    End Function

    Private Sub SaveToYCMDB_Ray(ByVal flgSyori As Boolean)

        Dim i As Integer = 1
        If flgSyori = True Then
            dbClass.DoDelete("imagepoint3d")
        Else
            i = GETMAXID("imagepoint3d") + 1
        End If

        For Each C3DST As Common3DSingleTarget In lstCommon3dST
            For Each ST As SingleTarget In C3DST.lstST
                If InsertRays(ST.ImageID, C3DST.TID, i) > 0 Then
                    i += 1
                End If
            Next
        Next
        For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
            For Each CT As CodedTarget In C3DCT.lstCT
                If InsertRays(CT.ImageID, C3DCT.PID, i) > 0 Then
                    i += 1
                End If
            Next
        Next
    End Sub

    Private Function InsertRays(ByVal imageid As Integer, ByVal TID As Integer, ByVal index As Integer) As Long
        Dim strFieldName(2) As String
        Dim strFieldData(2) As String

        strFieldName(0) = "ID"
        strFieldName(1) = "CPID"
        strFieldName(2) = "MPTID"

        strFieldData(0) = index
        strFieldData(1) = imageid
        strFieldData(2) = TID

        InsertRays = dbClass.DoInsert(strFieldName, "imagepoint3d", strFieldData)

    End Function


    '150115 SUURI Rep Sta 複数カメラ内部パラメータ取扱機能--------------
    Public Sub ReadCamParamList(ByVal strReadPath As String)
        Dim IDR As IDataReader
        Dim strSqlText As String = ""

        ConnectDbFBM(strReadPath)

        strSqlText = "SELECT CamParamID,CamParamFile FROM CamParamList"
        If lstCamParam Is Nothing Then
            lstCamParam = New List(Of CameraParam)
        Else
            lstCamParam.Clear()
        End If

        IDR = dbClass.DoSelect(strSqlText)
        If Not IDR Is Nothing Then
            Do While IDR.Read
                Dim objCamPar As New CameraParam
                objCamPar.Cid = CInt(IDR.GetValue(0))
                objCamPar.CName = IDR.GetValue(1)
                'Commit hiih uyed doorhiig True bolgono. SUSANO
#If True Then
                objCamPar.Cpath = My.Application.Info.DirectoryPath & "\計測システムフォルダ\CamParam\" & objCamPar.CName
#Else
                objCamPar.Cpath = My.Application.Info.DirectoryPath & "\SystemFolder\CamParam\" & objCamPar.CName
#End If

                objCamPar.ReadCamParamFromFile()
                lstCamParam.Add(objCamPar)
            Loop

            IDR.Close()
        End If



        AccessDisConnect()

    End Sub
	'150115 SUURI Rep End 複数カメラ内部パラメータ取扱機能--------------
	'150118 SUURI ADD Sta 20150118--------------------------------------
    Public Sub ReadCamParamList()

        If lstCamParam Is Nothing Then
            lstCamParam = New List(Of CameraParam)
        Else
            lstCamParam.Clear()
        End If


        Dim objCamPar As New CameraParam

        objCamPar.Cid = 1
        objCamPar.CName = ""
        objCamPar.Cpath = _campar_path
        objCamPar.ReadCamParamFromFile()
        lstCamParam.Add(objCamPar)



    End Sub

   

    Private Function CalcAllCameraPoseByBasePoint() As Integer
        Dim icnt As Integer = 0
        'SUSANO UPDATE START 
        Dim isCalcPoseByBasePoint As Integer = GetPrivateProfileInt("Kaiseki", "CalcPoseByBasePoint ", -1, My.Application.Info.DirectoryPath & "\vform.ini")
        If isCalcPoseByBasePoint = 1 Then
            For Each ISI As ImageSet In lstImages
                Dim tmpPose As Object
                Dim tmpQuality As Object
                tmpPose = Nothing
                tmpQuality = Nothing

                If ISI.CalcPoseByCommonCT3dPoint(ISI.objCamparam.Camparam, lstBasePoint, tmpPose, tmpQuality) = True Then
                    ISI.ImagePose.Pose = tmpPose
                    ISI.flgConnected = True

                    icnt += 1

                End If
            Next
        Else
            ConnectDbFBM(ProjectPath & "\")

            For Each ISI As ImageSet In lstImages
                Dim IDR As IDataReader
                Dim strSqlText As String = ""
                'strSqlText = "SELECT ID,imagefilename,CX,CY,CZ,CA,CB,CG,flgNotUse,flgSystemNotUse,flgFirst,flgSecond,CamParamID FROM CameraPose"
                strSqlText = "SELECT ID,imagefilename,CX,CY,CZ,CA,CB,CG,flgNotUse,flgSystemNotUse FROM CameraPose WHERE ID = " & ISI.ImageId
                Dim tmpPose(6) As Double

                IDR = dbClass.DoSelect(strSqlText)
                If Not IDR Is Nothing Then
                    Do While IDR.Read
                        'Dim objCamPose As New CameraPose
                        'objCamPose.Cid = CInt(IDR.GetValue(0))
                        'objCamPose.CFileName = IDR.GetValue(1)
                        tmpPose(0) = CDbl(IDR.GetValue(2))
                        tmpPose(1) = CDbl(IDR.GetValue(3))
                        tmpPose(2) = CDbl(IDR.GetValue(4))
                        tmpPose(3) = CDbl(IDR.GetValue(5))
                        tmpPose(4) = CDbl(IDR.GetValue(6))
                        tmpPose(5) = CDbl(IDR.GetValue(7))
                        tmpPose(6) = 1
                        'objCamPose.CflgNotUse = CInt(IDR.GetValue(8))
                        'objCamPose.CflgSystemNotUse = CInt(IDR.GetValue(9))
                        ''objCamPose.CflgFirst = CInt(IDR.GetValue(10))
                        ''objCamPose.CflgSecond = CInt(IDR.GetValue(11))
                        ''objCamPose.CCamParamID = CInt(IDR.GetValue(12))
                        'lstCamPose.Add(objCamPose)

                    Loop
                    IDR.Close()
                End If
                ISI.ImagePose.Pose = tmpPose
                ISI.flgConnected = True

                icnt += 1


            Next
            AccessDisConnect()
        End If
        'SUSANO UPDATE END

        GenCommon3Dpoint()


        'SUSANO ADD START 20151012
        '悪いレイを削除

        ' SueokiBadCTremove()

        ' GenCommon3Dpoint()
        'For Each ISI As ImageSet In lstImages
        '    CalcReProjectionErrorOneCT_faster(ISI)
        '    'CalcALLCTnoSingleTarget3dCoord()
        '    CalcThisImagesCT_3dCoord(ISI)
        'Next
        'SUSANO ADD END 20151012

        Dim objHommat As Object = Nothing
        Dim objQuality As Object = Nothing

        Dim ComScalse As Object = Nothing
		'20150212 Rep By Suuri Sta ---------------------------------
        Dim strHoseiMode As String = CStr(My.Computer.FileSystem.ReadAllText(My.Application.Info.DirectoryPath & "\HoseiMode.txt"))
        If strHoseiMode = "" Then
            KakuninnoTame("BasePointHikakuHoseiNashi.csv", "KekkaByBasePointToPairHoseiNashi.csv")
        Else
            CalcAffineHoseiByBasePoint(objHommat, objQuality, ComScalse, 1, strHoseiMode)
            KakuninnoTame("BasePointHikakuHoseiMae.csv", "KekkaByBasePointToPairHoseiMae.csv")
            For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
                If C3DCT.flgUsable = True Then


                    For Each P3D As Point3D In C3DCT.lstP3d
                        HOperatorSet.AffineTransPoint3D(objHommat, P3D.X, P3D.Y, P3D.Z, P3D.X, P3D.Y, P3D.Z)
                    Next
                End If
            Next
            KakuninnoTame("BasePointHikakuHoseiAto.csv", "KekkaByBasePointToPairHoseiAto.csv")
        End If

        'CalcAffineHoseiByBasePoint(objHommat, objQuality, ComScalse, 1)

        'For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
        'If C3DCT.flgUsable = True Then


        'For Each P3D As Point3D In C3DCT.lstP3d
        'HOperatorSet.AffineTransPoint3D(objHommat, P3D.X, P3D.Y, P3D.Z, P3D.X, P3D.Y, P3D.Z)
        'Next
        'End If
        'Next
        'KakuninnoTame()

        '20150212 Rep By Suuri End --------------------------------- 

        Return icnt

    End Function


	'20150212 Rep By Suuri Sta -------------------------------------- 
    'Public Function CalcAffineHoseiByBasePoint(ByRef Hommat As Object, ByRef Quality As Object, _
    '                                            ByRef ComScale As Object, ByVal ScaleMM As Double) As Boolean

    Public Function CalcAffineHoseiByBasePoint(ByRef Hommat As Object, ByRef Quality As Object, _
                                                ByRef ComScale As Object, ByVal ScaleMM As Double, ByVal strHoseiMode As String) As Boolean
	'20150212 Rep By Suuri End --------------------------------------
        Dim MyCT1_IP As New ImagePoints
        Dim MyCT2_IP As New ImagePoints
        Dim CommonCT3d As New Point3D
        Dim BaseCT3d As New Point3D

        Dim cnttmpComCT As Integer = 0
        Dim t As Integer
        Dim N As Integer = IIf(lstBasePoint.Count > lstCommon3dCT.Count, lstBasePoint.Count, lstCommon3dCT.Count) * CodedTarget.CTnoSTnum - 1
        Dim X(N) As Object
        Dim Y(N) As Object
        Dim Z(N) As Object
        Dim M As Integer = lstBasePoint.Count * CodedTarget.CTnoSTnum - 1
        Dim XB(N) As Object
        Dim YB(N) As Object
        Dim ZB(N) As Object
        Dim cnt As Integer = 0
        Dim HomMatInvert As Object = Nothing

        CalcAffineHoseiByBasePoint = False

        For Each objC3D As Common3DCodedTarget In lstBasePoint
            For Each MyCT1 As Common3DCodedTarget In lstCommon3dCT
                If MyCT1.flgUsable = True Then
                    If MyCT1.PID = objC3D.PID Then
                        For t = 0 To CodedTarget.CTnoSTnum - 1
                            X(cnt) = MyCT1.lstP3d.Item(t).X
                            Y(cnt) = MyCT1.lstP3d.Item(t).Y
                            Z(cnt) = MyCT1.lstP3d.Item(t).Z
                            XB(cnt) = objC3D.lstP3d(t).X
                            YB(cnt) = objC3D.lstP3d(t).Y
                            ZB(cnt) = objC3D.lstP3d(t).Z
                            cnt += 1
                        Next
                        cnttmpComCT += 1
                        Exit For
                    End If
                End If
            Next
        Next
        cnt -= 1
        ReDim Preserve X(cnt)
        ReDim Preserve Y(cnt)
        ReDim Preserve Z(cnt)
        ReDim Preserve XB(cnt)
        ReDim Preserve YB(cnt)
        ReDim Preserve ZB(cnt)
        CommonCT3d.X = X
        CommonCT3d.Y = Y
        CommonCT3d.Z = Z
        BaseCT3d.X = XB
        BaseCT3d.Y = YB
        BaseCT3d.Z = ZB

        If cnttmpComCT < intCommonCTnum Then
            Exit Function
        End If
        Hommat = Nothing
        'PairPose.hError = Nothing
        'HOperatorSet.VectorToRelPose(MyCT2_IP.Row, MyCT2_IP.Col, MyCT1_IP.Row, MyCT1_IP.Col, _
        '                 Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, IS2.objCamparam.Camparam, IS1.objCamparam.Camparam, "gold_standard", _
        '                 PairPose.RelPose, PairPose.CovRelPose, PairPose.hError, _
        '                 PairPose.X, PairPose.Y, PairPose.Z, PairPose.CovXYZ)

        'If BTuple.TupleLength(PairPose.hError) = 1 Then
        '    If BTuple.TupleDeviation(PairPose.Z) > 50 Or BTuple.TupleMean(PairPose.Z) > 50 Then
        '        cnttmpComCT = -1
        '        Exit Function
        '    End If
        '    If PairPose.hError > 1 Then
        '        Dim tt As Integer = 1
        '        cnttmpComCT = -1
        '    End If
        'Else
        '    cnttmpComCT = -1
        'End If

        'If cnttmpComCT < intCommonCTnum Then
        '    Exit Function
        'End If

        '  Dim MyCT_3d As New Point3D(PairPose.X, PairPose.Y, PairPose.Z)
        'Dim MyCT_3dT As New Point3D
        Dim Scale As Object = Nothing
        CommonCT3d.CalcScale(BaseCT3d, ComScale)
        ' CommonCT3d.SetScale(ComScale)
        ' 'rigid', 'similarity', 'affine', 'projective' 

		'20150212 Rep By Suuri Sta -------------------------
        'HOperatorSet.VectorToHomMat3d("affine", CommonCT3d.X, CommonCT3d.Y, CommonCT3d.Z, BaseCT3d.X, BaseCT3d.Y, BaseCT3d.Z, Hommat)
        HOperatorSet.VectorToHomMat3d(strHoseiMode, CommonCT3d.X, CommonCT3d.Y, CommonCT3d.Z, BaseCT3d.X, BaseCT3d.Y, BaseCT3d.Z, Hommat)
		'20150212 Rep By Suuri End -------------------------
        HOperatorSet.AffineTransPoint3D(Hommat, CommonCT3d.X, CommonCT3d.Y, CommonCT3d.Z, CommonCT3d.X, CommonCT3d.Y, CommonCT3d.Z)

        Dim Q1 As Object = Nothing
        Dim Q2 As Object = Nothing
        Dim Q12 As Object = Nothing

        CommonCT3d.GetDisttoOtherPose(BaseCT3d, Quality)

        'HOperatorSet.VectorToHomMat3d("similarity", MyCT_3d.X, MyCT_3d.Y, MyCT_3d.Z, CommonCT3d.X, CommonCT3d.Y, CommonCT3d.Z, HomMat)
        'HOperatorSet.AffineTransPoint3D(HomMat, MyCT_3d.X, MyCT_3d.Y, MyCT_3d.Z, MyCT_3dT.X, MyCT_3dT.Y, MyCT_3dT.Z)
        'MyCT_3dT.GetDisttoOtherPose(CommonCT3d, Quality)
        'Q1 = BTuple.TupleMean(Quality)
        'HOperatorSet.HomMat3dToPose(HomMat, Q2)

        'MyCT_3d.CalcScale(CommonCT3d, ComScale)
        'MyCT_3d.SetScale(ComScale)
        'hom_mat_3d_from_3d_3d_point_correspondence(MyCT_3d.X, MyCT_3d.Y, MyCT_3d.Z, CommonCT3d.X, CommonCT3d.Y, CommonCT3d.Z, HomMat)
        'HOperatorSet.AffineTransPoint3D(HomMat, MyCT_3d.X, MyCT_3d.Y, MyCT_3d.Z, MyCT_3d.X, MyCT_3d.Y, MyCT_3d.Z)
        'MyCT_3d.GetDisttoOtherPose(CommonCT3d, Quality)
        Q1 = BTuple.TupleMean(Quality)
        ''Rep By Suuri Sta 20140826 
        'If Q1 > 20 * ScaleMM Then
        '    'If Q1 > 10 * ScaleMM Then
        '    'Rep By Suuri End 20140826 
        '    Exit Function
        'End If

        'CalcBestHomMat_Scale(HomMat, ComScale, New Point3D(PairPose.X, PairPose.Y, PairPose.Z), CommonCT3d)
        'MyCT_3d = New Point3D(PairPose.X, PairPose.Y, PairPose.Z)
        'MyCT_3d.SetScale(ComScale)
        'HOperatorSet.AffineTransPoint3D(HomMat, MyCT_3d.X, MyCT_3d.Y, MyCT_3d.Z, MyCT_3d.X, MyCT_3d.Y, MyCT_3d.Z)
        'MyCT_3d.GetDisttoOtherPose(CommonCT3d, Q1)
        'Quality = BTuple.TupleMean(Q1)
        'If Quality > 10 * ScaleMM Then
        '    Exit Function
        'End If

        'HOperatorSet.HomMat3dToPose(HomMat, Pose)


        'HOperatorSet.VectorToPose(CommonCT3d.X, CommonCT3d.Y, CommonCT3d.Z, MyCT2_IP.Row, MyCT2_IP.Col, Camparam, "iterative", "error", PairPose.RelPose, Q12)
        'HOperatorSet.PoseToHomMat3d(PairPose.RelPose, HomMatInvert)
        'HOperatorSet.HomMat3dInvert(HomMatInvert, HomMatInvert)
        'HOperatorSet.HomMat3dToPose(HomMatInvert, PairPose.RelPose)
        'Scale = BTuple.TupleSub(PairPose.RelPose, Pose)
        'If BTuple.TupleMax(BTuple.TupleAbs(Scale)) > 1 Then
        '    Scale = Scale
        'Else
        '    Pose = PairPose.RelPose
        'End If
        '3次元座標を固定して、カメラの姿勢をバンドル調整する。
        'Quality = RunBA_PoseOnly(FBMlib.T_treshold, Camparam, Pose, MyCT2_IP, CommonCT3d, BTuple.TupleLength(CommonCT3d.Z))
linejump:


        'HOperatorSet.PoseToHomMat3d(Pose, HomMat)
        'HOperatorSet.HomMat3dInvert(HomMat, HomMatInvert)
        'Dim RowTrans As Object = Nothing
        'Dim ColTrans As Object = Nothing
        'Dim ReProjError As Object = Nothing
        'Dim MyCT_3d1 As New Point3D(PairPose.X, PairPose.Y, PairPose.Z)
        'HOperatorSet.AffineTransPoint3D(HomMatInvert, CommonCT3d.X, CommonCT3d.Y, CommonCT3d.Z, MyCT_3d1.X, MyCT_3d1.Y, MyCT_3d1.Z)
        'HOperatorSet.Project3DPoint(MyCT_3d1.X, MyCT_3d1.Y, MyCT_3d1.Z, Camparam, RowTrans, ColTrans)
        'HOperatorSet.DistancePp(MyCT2_IP.Row, MyCT2_IP.Col, RowTrans, ColTrans, ReProjError)
        'Quality = BTuple.TupleMean(ReProjError)

        CalcAffineHoseiByBasePoint = True

    End Function

	'20150212 Rep By Suuri Sta--------- 
    'Private Sub KakuninnoTame()
    Private Sub KakuninnoTame(ByVal FileName1 As String, ByVal FileName2 As String)
	'20150212 Rep By Suuri Sta---------

        '確認用
        Dim cntjj As Integer = 0
        Dim strFileText As String = ""
        For Each objC3DCT As Common3DCodedTarget In lstCommon3dCT
            If objC3DCT.flgUsable = True Then
                For Each objHikaku As Common3DCodedTarget In lstAllHikakuPoint
                    If objC3DCT.PID = objHikaku.PID Then
                        If objC3DCT.lstP3d.Count = CodedTarget.CTnoSTnum And objHikaku.lstP3d.Count = CodedTarget.CTnoSTnum Then
                            cntjj = 0
                            For Each objP3d As Point3D In objC3DCT.lstP3d
                                strFileText = strFileText & objC3DCT.PID & "," & cntjj & "," & objP3d.X.D & "," & objP3d.Y.D & "," & objP3d.Z.D & ","
                                strFileText = strFileText & objHikaku.lstP3d(cntjj).X.D & "," & objHikaku.lstP3d(cntjj).Y.D & "," & objHikaku.lstP3d(cntjj).Z.D
                                strFileText = strFileText & vbNewLine
                                cntjj += 1
                            Next
                            Exit For
                        End If
                    End If
                Next
            End If
        Next

		'20150212 Rep By Suuri Sta -----------
        'My.Computer.FileSystem.WriteAllText(_projectpath & "\BasePointHikaku.csv", strFileText, False)
        My.Computer.FileSystem.WriteAllText(_projectpath & "\" & FileName1, strFileText, False)
		'20150212 Rep By Suuri Sta -----------
        strFileText = ""
        For Each objC3DCT As Common3DCodedTarget In lstCommon3dCT
            If objC3DCT.flgUsable = True Then
                cntjj = 0
                For Each objP3d As Point3D In objC3DCT.lstP3d
                    strFileText = strFileText & objC3DCT.PID & "," & cntjj & "," & objP3d.X.D & "," & objP3d.Y.D & "," & objP3d.Z.D
                    strFileText = strFileText & vbNewLine
                    cntjj += 1
                Next
            End If
        Next

		'20150212 Rep By Suuri Sta ------------
        'My.Computer.FileSystem.WriteAllText(_projectpath & "\KekkaByBasePointToPair.csv", strFileText, False)
        My.Computer.FileSystem.WriteAllText(_projectpath & "\" & FileName2, strFileText, False)
		'20150212 Rep By Suuri End ------------
		
    End Sub
#End Region


    'ADD BY TUGI 20150706 Sta 
    Private Sub ReadKijyunPointsFromDB(ByRef lstBasePoints As List(Of Common3DCodedTarget))
        Dim sysPath As String = My.Application.Info.DirectoryPath & YCM_SYS_FLDR
        Dim dReader As System.Data.IDataReader
        Try
            If lstBasePoints Is Nothing Then
                lstBasePoints = New List(Of Common3DCodedTarget)
            Else
                lstBasePoints.Clear()
            End If
            If ConnectDbSystemSetting(sysPath) Then
                dReader = dbClass.DoSelect("SELECT * FROM " & YCM_SYS_MDB_KIJYUNTBL & " order by CTID")
                While dReader.Read()
                    Dim strLabel As String
                    Dim intCTID As Integer
                    Dim intKijyunFlag As Integer
                    intKijyunFlag = CInt(dReader.GetValue(5))
                    If intKijyunFlag = 1 Then
                        strLabel = dReader.GetValue(1).ToString
                        Dim objnewPoint As New Point3D
                        objnewPoint.X = CDbl(dReader.GetValue(2))
                        objnewPoint.Y = CDbl(dReader.GetValue(3))
                        objnewPoint.Z = CDbl(dReader.GetValue(4))
                        If IsNumeric(strLabel.Replace("CT", "")) Then
                            intCTID = CInt(strLabel.Replace("CT", ""))
                            Dim objNewC3DP As New Common3DCodedTarget
                            objNewC3DP.PID = intCTID
                            objNewC3DP.flgUsable = True
                            objNewC3DP.lstP3d.Add(objnewPoint)
                            lstBasePoints.Add(objNewC3DP)
                        Else
                            lstBasePoints.Last.lstP3d.Add(objnewPoint)
                        End If
                    End If
                End While
                dbClass.DisConnect()
            End If
        Catch ex As Exception
        Finally
        End Try
    End Sub
    Private Sub ReadAllHikakuPointsFromDB(ByRef lstAllhikakuPoints As List(Of Common3DCodedTarget))
        Dim sysPath As String = My.Application.Info.DirectoryPath & YCM_SYS_FLDR
        Dim dReader As System.Data.IDataReader
        Try
            If lstAllhikakuPoints Is Nothing Then
                lstAllhikakuPoints = New List(Of Common3DCodedTarget)
            Else
                lstAllhikakuPoints.Clear()
            End If
            If ConnectDbSystemSetting(sysPath) Then
                dReader = dbClass.DoSelect("SELECT * FROM " & YCM_SYS_MDB_KIJYUNTBL & " order by CTID")
                While dReader.Read()
                    Dim strLabel As String
                    Dim intCTID As Integer
                    Dim intKijyunFlag As Integer
                    intKijyunFlag = CInt(dReader.GetValue(5))
                    strLabel = dReader.GetValue(1).ToString
                    Dim objnewPoint As New Point3D
                    objnewPoint.X = CDbl(dReader.GetValue(2))
                    objnewPoint.Y = CDbl(dReader.GetValue(3))
                    objnewPoint.Z = CDbl(dReader.GetValue(4))
                    If IsNumeric(strLabel.Replace("CT", "")) Then
                        intCTID = CInt(strLabel.Replace("CT", ""))
                        Dim objNewC3DP As New Common3DCodedTarget
                        objNewC3DP.PID = intCTID
                        objNewC3DP.lstP3d.Add(objnewPoint)
                        lstAllhikakuPoints.Add(objNewC3DP)
                    Else
                        lstAllhikakuPoints.Last.lstP3d.Add(objnewPoint)
                    End If
                End While
                dbClass.DisConnect()
            End If
        Catch ex As Exception
        Finally
        End Try


    End Sub
    'ADD BY TUGI 20150706 End
    Public Function AddStByEdge(ByVal objtmpFbm As FeatureImage) As Integer
        AddStByEdge = 0
        For Each ISI As FBMlib.ImageSet In Me.lstImages
            If Not ISI.hx_EdgePS Is Nothing Then
                Dim maxID As Integer = ISI.Targets.GetST_MaxID
                For Each ISItmp As ImageSet In objtmpFbm.lstImages
                    If ISI.ImageId = ISItmp.ImageId Then
                        For Each ST As FBMlib.SingleTarget In ISItmp.Targets.lstST
                            maxID += 1
                            ST.ImageID = ISI.ImageId
                            ST.P2ID = maxID
                            ISI.Targets.lstST.Add(ST)
                        Next
                        Exit For
                    End If
                Next
            End If
        Next

        Dim maxTid As Integer = Me.GetMaxTID
        For Each objC3dST As FBMlib.Common3DSingleTarget In objtmpFbm.lstCommon3dST
            If objC3dST.PID = -1 Then
                Continue For
            End If
            maxTid += 1
            objC3dST.PID = maxTid - 10000
            For Each objST As SingleTarget In objC3dST.lstST
                objST.P3ID = objC3dST.PID
            Next

            Me.lstCommon3dST.Add(objC3dST)
            AddStByEdge += 1
        Next
        Me.CalcAllImagesInvertHomMat()
        Me.CalcAllImages_Ray()
        For Each objC3DST As Common3DSingleTarget In Me.lstCommon3dST
            objC3DST.Calc3dPoints()
        Next

    End Function

#Region "Monitor関連"
    Private Sub MonitorImagePose(ByVal strOutFileName As String)
        Dim strtxt As String = ""
        For Each ISI As ImageSet In lstImages
            If ISI.flgConnected = True Then
                strtxt = strtxt & ISI.ImageId & "," & ISI.ImageName & "," &
                        BTuple.TupleSelect(ISI.ImagePose.Pose, 0).D & "," &
                        BTuple.TupleSelect(ISI.ImagePose.Pose, 1).D & "," &
                        BTuple.TupleSelect(ISI.ImagePose.Pose, 2).D & "," &
                        BTuple.TupleSelect(ISI.ImagePose.Pose, 3).D & "," &
                        BTuple.TupleSelect(ISI.ImagePose.Pose, 4).D & "," &
                        BTuple.TupleSelect(ISI.ImagePose.Pose, 5).D & vbNewLine
            End If
        Next
        My.Computer.FileSystem.WriteAllText(strOutFileName, strtxt, False)

    End Sub
#End Region
End Class