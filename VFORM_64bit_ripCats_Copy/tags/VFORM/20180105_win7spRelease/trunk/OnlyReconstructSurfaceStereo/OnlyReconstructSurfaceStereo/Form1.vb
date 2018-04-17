Imports HalconDotNet
Imports FBMlib
Imports System.Collections.ObjectModel

Public Class Form1
   
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim cmds As String() = System.Environment.GetCommandLineArgs()
        If cmds.Count > 5 Then
            Dim strImageFile1 As String = cmds(1)
            Dim strImageFile2 As String = cmds(2)

            Dim objFileNames As String = cmds(3)
            Dim IDs() As String = objFileNames.Split(",")
            Dim I1_id As Integer = IDs(0)
            Dim I2_id As Integer = IDs(1)
            projectpath = cmds(4)
            mmScale = CDbl(cmds(5))

            Dim I1 As New ImageSet
            Dim I2 As New ImageSet
            I1.ImageFullPath = strImageFile1
            I2.ImageFullPath = strImageFile2
            I1.ImageId = I1_id
            I2.ImageId = I2_id

            ' I1.ImagePose .Pose = 
            Dim Pair As New List(Of ImageSet)
            'ConnectDbFBM(projectpath & "\Pdata\")
            'I1.ReadImageSet(projectpath)
            'I2.ReadImageSet(projectpath)
            'AccessDisConnect()
            HOperatorSet.ReadPose(projectpath & "\" & I1.ImageId & "_pose.pos", I1.ImagePose.ReConstWorldPose)
            HOperatorSet.ReadPose(projectpath & "\" & I2.ImageId & "_pose.pos", I2.ImagePose.ReConstWorldPose)

            HOperatorSet.TupleMult(I1.ImagePose.ReConstWorldPose, New HTuple({1 / mmScale, 1 / mmScale, 1 / mmScale, 1.0, 1.0, 1.0, 1.0}),
                                                                                   I1.ImagePose.Pose)
            HOperatorSet.TupleMult(I2.ImagePose.ReConstWorldPose, New HTuple({1 / mmScale, 1 / mmScale, 1 / mmScale, 1.0, 1.0, 1.0, 1.0}),
                                                                               I2.ImagePose.Pose)
            Dim objCamparam As New HTuple
            HOperatorSet.ReadTuple(projectpath & "\Pdata\" & "hv_CamparamOut.tpl", objCamparam)
            I1.objCamparam.Camparam = objCamparam
            I2.objCamparam.Camparam = objCamparam
            Pair.Add(I1)
            Pair.Add(I2)
            Dim SettingData As New SettingsTable
            SettingData.m_dbClass = New CDBOperateOLE
            SettingData.m_dbClass.Connect("TGless.mdb")
            SettingData.GetLatest()
            SettingData.m_dbClass.DisConnect()

            SettingData.SelectedImageIDs = objFileNames

            Dim outputfilename As String = projectpath & "\" & SettingData.SelectedImageIDs & "_object.om3"

            Dim objReconstData As New ReconstData(Pair, SettingData)
            HOperatorSet.ReadTuple(projectpath & "\" & SettingData.SelectedImageIDs & "boundingbox.tpl", objReconstData.boundingbox)
            objReconstData.GenStereoModel()
            If System.IO.File.Exists(outputfilename) = True Then
                System.IO.File.Delete(outputfilename)
            End If
            If ReconstructScene2(objReconstData.SettingData, objReconstData.pair, False) = True Then
                objReconstData.CalcSmooth(False)
                objReconstData.CalcSampling(False)
                objReconstData.RunCleanUp(False)
                objReconstData.CalcMesh(False)
                ' objReconstData.WriteObjFile(MainFrm.objFBM.ProjectPath)
                If objReconstData.calcColorRGB() = True Then
                    objReconstData.flgSuccess = True
                    HOperatorSet.WriteObjectModel3d(objReconstData.SettingData.hv_ResultObject3D, "om3", outputfilename, New HTuple, New HTuple)
                End If
            End If

        Else
            projectpath = cmds(1)
            Dim SettingData As New SettingsTable
            SettingData.m_dbClass = New CDBOperateOLE
            SettingData.m_dbClass.Connect("TGless.mdb")
            SettingData.GetLatest()
            SettingData.m_dbClass.DisConnect()
            UnionSceneAllResult(SettingData)

        End If
        Me.Close()
    End Sub

    Private Sub UnionSceneAllResult(ByVal SettingsData As SettingsTable)
        Dim hAllObject3DHandles As New HTuple
        Dim hAllObject3D As New HTuple
        '  HOperatorSet.GenEmptyObjectModel3d(hAllObject3DHandles)
        Dim files As ReadOnlyCollection(Of String)
        files = My.Computer.FileSystem.GetFiles(projectpath, FileIO.SearchOption.SearchTopLevelOnly, "*.om3")

        For Each objItemReconstData As String In files
            With objItemReconstData
                Dim hvObjModel3D As New HTuple
                HOperatorSet.ReadObjectModel3d(objItemReconstData, 1, New HTuple, New HTuple, hvObjModel3D, New HTuple)
                HOperatorSet.TupleConcat(hAllObject3DHandles, hvObjModel3D, hAllObject3DHandles)


            End With
        Next
        HOperatorSet.UnionObjectModel3d(hAllObject3DHandles, "points_surface", hAllObject3D)
        Try
            HOperatorSet.ClearObjectModel3d(hAllObject3DHandles)
        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try
        GC.Collect()
        GC.WaitForPendingFinalizers()

        Dim numPoints As HTuple = Nothing
        HOperatorSet.GetObjectModel3dParams(hAllObject3D, "has_points", numPoints)
        If numPoints.ToString = "false" Then
            Exit Sub
        End If
        HOperatorSet.GetObjectModel3dParams(hAllObject3D, "num_points", numPoints)
        Dim objReconstData As New ReconstData(Nothing, SettingsData)
        With objReconstData
            .SettingData.hv_ResultObject3D = hAllObject3D
#If True Then
            HOperatorSet.WriteObjectModel3d(hAllObject3D, "om3", projectpath & "\Pdata\" & "ReconstOm3.om3", New HTuple, New HTuple)
#End If
            .CalcSmooth(True)
            .RunCleanUp(True)
            .CalcSampling(True)
            .CalcSamplingFast(True)
            '.CalcMesh(True)
            .CalcMeshNoParam(True)
            hAllObject3D = .SettingData.hv_ResultObject3D
#If True Then
            HOperatorSet.WriteObjectModel3d(hAllObject3D, "om3", projectpath & "\Pdata\" & "ReconstResultOm3.om3", New HTuple, New HTuple)
#End If
            .GetDataFromObjectModel3d()

            'Dim Pnum As New HTuple
            'HOperatorSet.GetObjectModel3dParams(hAllObject3D, "num_points", Pnum)
            'Dim GrayR As New HTuple
            'Dim GrayG As New HTuple
            'Dim GrayB As New HTuple
            'HOperatorSet.TupleGenConst(Pnum, New HTuple(0), .p3D.X)
            'HOperatorSet.TupleGenConst(Pnum, New HTuple(0), .p3D.Y)
            'HOperatorSet.TupleGenConst(Pnum, New HTuple(0), .p3D.Z)
            'HOperatorSet.TupleGenConst(Pnum, New HTuple(0), GrayR)
            'HOperatorSet.TupleGenConst(Pnum, New HTuple(0), GrayG)
            'HOperatorSet.TupleGenConst(Pnum, New HTuple(0), GrayB)

            'HOperatorSet.GetObjectModel3dParams(hAllObject3D, "point_coord_x", .p3D.X)
            'HOperatorSet.GetObjectModel3dParams(hAllObject3D, "point_coord_y", .p3D.Y)
            'HOperatorSet.GetObjectModel3dParams(hAllObject3D, "point_coord_z", .p3D.Z)
            'Try
            '    HOperatorSet.GetObjectModel3dParams(hAllObject3D, "&grayR", GrayR)
            '    HOperatorSet.GetObjectModel3dParams(hAllObject3D, "&grayG", GrayG)
            '    HOperatorSet.GetObjectModel3dParams(hAllObject3D, "&grayB", GrayB)

            'Catch ex As Exception

            'End Try

            'Dim hstrHasPnormal As New HTuple
            'HOperatorSet.GetObjectModel3dParams(hAllObject3D, "has_point_normals", hstrHasPnormal)
            'If hstrHasPnormal.ToString = "true" Then
            '    .cntNormal = Pnum
            '    HOperatorSet.TupleGenConst(.cntNormal, New HTuple(0), .nVec.X)
            '    HOperatorSet.TupleGenConst(.cntNormal, New HTuple(0), .nVec.Y)
            '    HOperatorSet.TupleGenConst(.cntNormal, New HTuple(0), .nVec.Z)

            '    HOperatorSet.GetObjectModel3dParams(hAllObject3D, "point_normal_x", .nVec.X)
            '    HOperatorSet.GetObjectModel3dParams(hAllObject3D, "point_normal_y", .nVec.Y)
            '    HOperatorSet.GetObjectModel3dParams(hAllObject3D, "point_normal_z", .nVec.Z)
            'End If

            'Dim hstrHasTriangle As New HTuple
            'HOperatorSet.GetObjectModel3dParams(hAllObject3D, "has_triangles", hstrHasTriangle)
            'If hstrHasTriangle.ToString = "true" Then
            '    Dim hTriangle As New HTuple
            '    HOperatorSet.GetObjectModel3dParams(hAllObject3D, "num_triangles", .cntTriangle)
            '    HOperatorSet.TupleGenConst(.cntTriangle, New HTuple(0), hTriangle)
            '    HOperatorSet.GetObjectModel3dParams(hAllObject3D, "triangles", hTriangle)
            '    HOperatorSet.TupleAdd(hTriangle, New HTuple(1), .indTriangle)

            'End If

            'OBJ形式ファイルを出力
            .OutPutObjFile()

            'Dim savePath As String = MainFrm.objFBM.ProjectPath & "\Pdata\" & ReconstructionWindow.FileName3D
            'Using file As System.IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(savePath, True)

            '    Dim fRow As String = ""
            '    If .cntNormal.I > 0 Then
            '        For vi As Integer = 0 To .p3D.X.Length - 1 Step 1
            '            '法線ベクトルの書き込み
            '            fRow = "vn " & .nVec.X(vi).D & " " & .nVec.Y(vi).D & " " & .nVec.Z(vi).D
            '            file.WriteLine(fRow)

            '            '3次元点座標と色情報の書き込み
            '            fRow = "v " & .p3D.X(vi).D & " " & .p3D.Y(vi).D & " " & .p3D.Z(vi).D
            '            fRow &= " " & .GrayR(vi).D & " " & .GrayG(vi).D & " " & .GrayB(vi).D
            '            file.WriteLine(fRow)
            '        Next
            '    Else
            '        For vi As Integer = 0 To .p3D.X.Length - 1 Step 1
            '            '3次元点座標と色情報の書き込み
            '            fRow = "v " & .p3D.X(vi).D & " " & .p3D.Y(vi).D & " " & .p3D.Z(vi).D
            '            fRow &= " " & .GrayR(vi).D & " " & .GrayG(vi).D & " " & .GrayB(vi).D
            '            file.WriteLine(fRow)
            '        Next
            '    End If

            '    Dim f_Pnum As New HTuple(0)

            '    'メッシの書き込み
            '    If .cntTriangle.I > 0 Then
            '        Dim tmpTriangle As HTuple = Nothing
            '        HOperatorSet.TupleAdd(.indTriangle, f_Pnum, tmpTriangle)
            '        For ti As Integer = 0 To tmpTriangle.Length - 1 Step 3
            '            fRow = "f " & tmpTriangle(ti).I & " " & tmpTriangle(ti + 1).I & " " & tmpTriangle(ti + 2).I
            '            file.WriteLine(fRow)
            '        Next
            '    End If
            '    HOperatorSet.TupleAdd(f_Pnum, .cntP3d, f_Pnum)

            '    file.Close()

            'End Using

            'DXF形式で３DFACEを出力する
#If False Then
            Dim savePathDXF As String = MainFrm.objFBM.ProjectPath & "\Pdata\" & ReconstructionWindow.FileName3D & ".dxf"
            Using file As System.IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(savePathDXF, False, System.Text.Encoding.Default)
                Dim fRow As String = ""
                Dim f_Pnum As New HTuple(-1)
                If .cntTriangle.I > 0 Then
                    file.WriteLine("  0")
                    file.WriteLine("SECTION")
                    file.WriteLine("  2")
                    file.WriteLine("ENTITIES")

                    Dim tmpTriangle As HTuple = Nothing
                    HOperatorSet.TupleAdd(.indTriangle, f_Pnum, tmpTriangle)
                    For ti As Integer = 0 To tmpTriangle.Length - 1 Step 3

                        file.WriteLine("  0")
                        file.WriteLine("3DFACE")
                        file.WriteLine("  8")
                        file.WriteLine("0")
                        '1点目
                        file.WriteLine(" 10")
                        file.WriteLine(.p3D.X(tmpTriangle(ti).I).D)
                        file.WriteLine(" 20")
                        file.WriteLine(.p3D.Y(tmpTriangle(ti).I).D)
                        file.WriteLine(" 30")
                        file.WriteLine(.p3D.Z(tmpTriangle(ti).I).D)
                        '2点目
                        file.WriteLine(" 11")
                        file.WriteLine(.p3D.X(tmpTriangle(ti + 1).I).D)
                        file.WriteLine(" 21")
                        file.WriteLine(.p3D.Y(tmpTriangle(ti + 1).I).D)
                        file.WriteLine(" 31")
                        file.WriteLine(.p3D.Z(tmpTriangle(ti + 1).I).D)
                        '3点目
                        file.WriteLine(" 12")
                        file.WriteLine(.p3D.X(tmpTriangle(ti + 2).I).D)
                        file.WriteLine(" 22")
                        file.WriteLine(.p3D.Y(tmpTriangle(ti + 2).I).D)
                        file.WriteLine(" 32")
                        file.WriteLine(.p3D.Z(tmpTriangle(ti + 2).I).D)
                        '4点目兼1点目
                        file.WriteLine(" 13")
                        file.WriteLine(.p3D.X(tmpTriangle(ti).I).D)
                        file.WriteLine(" 23")
                        file.WriteLine(.p3D.Y(tmpTriangle(ti).I).D)
                        file.WriteLine(" 33")
                        file.WriteLine(.p3D.Z(tmpTriangle(ti).I).D)

                    Next
                    file.WriteLine("  0")
                    file.WriteLine("ENDSEC")
                    file.WriteLine("  0")
                    file.WriteLine("EOF")
                End If

            End Using
#End If
        End With

#If False Then
        HOperatorSet.ReadObjectModel3d(MainFrm.objFBM.ProjectPath & "\Pdata\" & "ReconstResultOm3.om3", 1, New HTuple, New HTuple, objReconstData.SettingData.hv_ResultObject3D, New HTuple)

        '   objReconstData.GetDataFromObjectModel3d()


        objReconstData.CalcSamplingFast(True)
        ' objReconstData.SetRGB_to_ObjectModel3d()
        objReconstData.GetDataFromObjectModel3d()
#End If
    End Sub

    Public Sub ReconstMaeSyori6(ByVal ho_Images As HObject, ByRef ImageResult As HObject)
        HOperatorSet.Illuminate(ho_Images, ImageResult, 100, 100, 0.7)
    End Sub

    Public Sub ClearHObject(ByRef obj As HObject)
        If obj Is Nothing Then
            Exit Sub
        End If
        Try
            'HOperatorSet.ClearObj(obj)
            obj.Dispose()
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Public Function ReconstructScene2(ByRef SettingsData As SettingsTable, ByVal images As List(Of ImageSet), Optional ByVal showMsg As Boolean = True) As Boolean
        ReconstructScene2 = False
        'GC.Collect()
        'GC.WaitForPendingFinalizers()
        '  Dim OP As New HOperatorSet
        Dim hv_T0 As HTuple = Nothing, hv_T1 As HTuple = Nothing, numPoints As HTuple = Nothing
        Dim hv_StereoModelID As HTuple = SettingsData.hv_StereoModelID

        '   Dim hv_CameraParam As HTuple = Nothing
        Dim ho_Images As HObject = Nothing
        Dim hv_ObjectModel3D As HTuple = Nothing, HM3D As HTuple = Nothing, HM3DI As HTuple = Nothing
        Dim hv_PoseIn As HTuple = Nothing, hv_PoseOut As HTuple = Nothing
        Dim hv_OnePose As HTuple = images(0).ImagePose.Pose
        Dim hv_OnePoseHomMat As HTuple = Nothing
        HOperatorSet.TupleMult(images(0).ImagePose.Pose, New HTuple({mmScale, mmScale, mmScale, 1.0, 1.0, 1.0, 1.0}),
                                                                               hv_OnePose)
        HOperatorSet.PoseToHomMat3d(hv_OnePose, hv_OnePoseHomMat)

        Dim imagesCount As Int32 = 0
        imagesCount = images.Count
        Try
            For i As Integer = 0 To imagesCount - 2 Step 1
                HOperatorSet.SetStereoModelImagePairs(hv_StereoModelID, i, i + 1)
            Next
        Catch ex As Exception
            HOperatorSet.ClearStereoModel(hv_StereoModelID)
            SettingsData.hv_StereoModelID = Nothing
            Exit Function
        End Try

        HOperatorSet.GenEmptyObj(ho_Images)
        'If False Then
        '    Try
        '        HOperatorSet.ReadCamPar(SettingsData.camera_param, hv_CameraParam)
        '    Catch ex As Exception
        '        HOperatorSet.ReadTuple(SettingsData.camera_param, hv_CameraParam)
        '    End Try
        'Else
        '    hv_CameraParam = "" 'MainFrm.objFBM.hv_CamparamOut
        'End If
        'If BTuple.TupleLength(hv_CameraParam).I <= 0 Then
        '    If showMsg Then
        '        MsgBox("Invalid Camera Parameters")
        '    End If
        '    Throw New Exception("Invalid Camera Parameters")
        '    Exit Function
        'End If

        Try

            HOperatorSet.CountSeconds(hv_T0)
            '======================================ReadMultiViewStereoImages==============================
            Dim ho_ReconstImage As HObject = Nothing
            HOperatorSet.GenEmptyObj(ho_Images)
            HOperatorSet.GenEmptyObj(ho_ReconstImage)
            Dim files As New HTuple()
            For i As Integer = 0 To images.Count - 1
                files(i) = images(i).ImageFullPath
            Next
            HOperatorSet.ReadImage(ho_Images, files)
            '前処理
            '  ReconstMaeSyori1(ho_Images, ho_ReconstImage)
            ' ReconstMaeSyori2(ho_Images, ho_ReconstImage)
            ' ReconstMaeSyori3(ho_Images, CInt(SettingsData.binocular_mask_width), CInt(SettingsData.binocular_mask_height), ho_ReconstImage)
            ' ReconstMaeSyori4(ho_Images, ho_ReconstImage)
            'ReconstMaeSyori5(ho_Images, ho_ReconstImage)
            ReconstMaeSyori6(ho_Images, ho_ReconstImage)
            'ReconstMaeSyori7(ho_Images, ho_ReconstImage)
            ClearHObject(ho_Images)


            '======================================ReadMultiViewStereoImages===============================
            HOperatorSet.CountSeconds(hv_T1)
            SettingsData.ImageHenkanTime = BTuple.TupleSub(hv_T1, hv_T0).ToString()

            HOperatorSet.CountSeconds(hv_T0)
            ' MsgBox("ReconstructSurfaceStereo start")
#If True Then
            HOperatorSet.ReconstructSurfaceStereo(ho_ReconstImage, hv_StereoModelID, hv_ObjectModel3D)
#Else
            'Dim p As System.Diagnostics.Process = System.Diagnostics.Process.Start("OnlyReconstructSurfaceStereo.exe", images(0).ImageFullPath & " " & images(1).ImageFullPath & " " & SettingsData.SelectedImageIDs & " " & hv_StereoModelID.L)
            'p.WaitForExit()
            HOperatorSet.ReadObjectModel3d(MainFrm.objFBM.ProjectPath & "\" & SettingsData.SelectedImageIDs & "_object3d.obj", 1, New HTuple, New HTuple, hv_ObjectModel3D, New HTuple)
#End If
            HOperatorSet.CountSeconds(hv_T1)
            SettingsData.ReconstructTime = BTuple.TupleSub(hv_T1, hv_T0).ToString()
            HOperatorSet.ClearStereoModel(hv_StereoModelID)

            '  MsgBox("ReconstructSurfaceStereo end")
            ClearHObject(ho_ReconstImage)
            HOperatorSet.CountSeconds(hv_T0)
            HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3D, "num_points", numPoints)
            '======================================Reduce3D============================================
            Dim hv_ReducedObject As HTuple
            Dim hv_Temp3d As HTuple = hv_ObjectModel3D
            Dim hv_Temp3d2 As HTuple = Nothing
            'For Each iset As ImageSet In images
            '    If Not iset.Region Is Nothing Then
            '        HOperatorSet.CopyObjectModel3d(hv_Temp3d, "all", hv_Temp3d2)
            '        HOperatorSet.ClearObjectModel3d(hv_Temp3d)
            '        HOperatorSet.PoseToHomMat3d(iset.ImagePose.ReConstPose, HM3D)
            '        HOperatorSet.HomMat3dInvert(HM3D, HM3DI)
            '        HOperatorSet.HomMat3dToPose(HM3DI, hv_PoseIn)
            '        HOperatorSet.ReduceObjectModel3dByView(CalculateRegion(hv_CameraParam, iset.Region), hv_Temp3d2, hv_CameraParam, hv_PoseIn, hv_Temp3d)
            '        HOperatorSet.ClearObjectModel3d(hv_Temp3d2)
            '    End If
            'Next
            hv_ReducedObject = hv_Temp3d
            '======================================Reduce3D============================================

            'Dim hv_SmoothModel3D As HTuple = hv_ReducedObject
            'Dim hv_SmoothModel3D As HTuple = Nothing
            'HOperatorSet.SmoothObjectModel3d(hv_ReducedObject, "mls", New HTuple("mls_force_inwards", "mls_kNN"), New HTuple("true", 20), hv_SmoothModel3D)
            'HOperatorSet.ClearObjectModel3d(hv_ReducedObject)
            HOperatorSet.GetObjectModel3dParams(hv_ReducedObject, "num_points", numPoints)
            If BTuple.TupleInt(numPoints) > 0 Then
                '======================================Cleanup============================================
                Dim hv_TempObject3D As HTuple = Nothing
                'Dim hv_Connected As HTuple = Nothing, hv_Selected As HTuple = Nothing
                ''HOperatorSet.SmoothObjectModel3d(hv_HalconObjectModel3d, "mls", New HTuple("mls_force_inwards", "mls_kNN"), _
                ''                                 New HTuple("true", 400), hv_SmoothObject3D)
                'HOperatorSet.ConnectionObjectModel3d(hv_ReducedObject, "distance_3d", hv_ConnectDistance, hv_Connected)
                'HOperatorSet.ClearObjectModel3d(hv_ReducedObject)
                'HOperatorSet.SelectObjectModel3d(hv_Connected, "diameter_bounding_box", "and", Double.Parse(SettingsData.cleanup_min), hv_CMaxBoundingBox, hv_Selected)
                'HOperatorSet.ClearObjectModel3d(hv_Connected)
                'HOperatorSet.UnionObjectModel3d(hv_Selected, "points_surface", hv_SmoothObject3D)
                'HOperatorSet.ClearObjectModel3d(hv_Selected)
                'HOperatorSet.GetObjectModel3dParams(hv_SmoothObject3D, "num_points", numPoints)
                'If BTuple.TupleInt(numPoints) = 0 Then
                '    Exit Function
                'End If

                HOperatorSet.CopyObjectModel3d(hv_ReducedObject, "all", hv_TempObject3D)
                HOperatorSet.ClearObjectModel3d(hv_ReducedObject)
                'hv_ReducedObject = hv_SmoothObject3D
                '======================================Cleanups============================================

                '======================================Moto ni Modosu============================================
                If True Then
                    HOperatorSet.AffineTransObjectModel3d(hv_TempObject3D, hv_OnePoseHomMat, hv_ReducedObject)
                Else
                    HOperatorSet.CopyObjectModel3d(hv_TempObject3D, "all", hv_ReducedObject)
                End If
                HOperatorSet.ClearObjectModel3d(hv_TempObject3D)
                '======================================Moto ni Modosu============================================


                '======================================Save3DModel==========================================

                'Dim currentDate As String = SettingsData.SelectedImageIDs & "_" & System.DateTime.Now.ToString("yyyyMMddHHmmss")
                'If Not System.IO.Directory.Exists(MainFrm.objFBM.ProjectPath & "\Pdata\" & currentDate) Then
                '    System.IO.Directory.CreateDirectory(MainFrm.objFBM.ProjectPath & "\Pdata\" & currentDate)
                'End If
                'Dim savePath = MainFrm.objFBM.ProjectPath & "\Pdata\" & currentDate & "\"
                'Dim fileName = "ReconstructionResult.obj"
                'SettingsData.OutputPath = savePath & fileName

                '' WriteObjFileWithColor(images(0).ImageFullPath, images(0).Region, images(0).ImagePose.Pose, images(0).objCamparam.Camparam, hv_ReducedObject, New HTuple(savePath & fileName))
                'WriteObjFileWithColor(images, hv_ReducedObject, New HTuple(savePath & fileName))
                ''HOperatorSet.WriteObjectModel3d(hv_ReducedObject, "obj", New HTuple(savePath & fileName), New HTuple, New HTuple)
                'If Not System.IO.File.Exists(savePath & fileName) Then
                '    If showMsg Then
                '        MsgBox("Failed to write 3d object model")
                '    End If
                'End If
                'HOperatorSet.ClearObjectModel3d(hv_ReducedObject)
                'Dim hv_TriangulatedObjectModel3D As HTuple = Nothing
                'HOperatorSet.TriangulateObjectModel3d(hv_ReducedObject, "greedy", New HTuple, New HTuple, hv_TriangulatedObjectModel3D, New HTuple)
                'HOperatorSet.ClearObjectModel3d(hv_ReducedObject)

                SettingsData.hv_ResultObject3D = hv_ReducedObject

                '======================================Save3DModel==========================================
                HOperatorSet.CountSeconds(hv_T1)
                SettingsData.OutputTime = BTuple.TupleSub(hv_T1, hv_T0).ToString()

                HOperatorSet.GetObjectModel3dParams(SettingsData.hv_ResultObject3D, "num_points", numPoints)
                If BTuple.TupleInt(numPoints) > 0 Then
                    ReconstructScene2 = True
                End If
            Else
                SettingsData.strErrorMessage = "3次元点群を算出しませんでした。"
                Throw New Exception("Not Enough Points To Visualize")
                Exit Function
            End If

        Catch ex As Exception

            SettingsData.strErrorMessage = ex.Message
            If showMsg Then
                MsgBox("Reconstruction Failed: " & ex.Message)
            End If
            Exit Function
        End Try
    End Function

End Class
