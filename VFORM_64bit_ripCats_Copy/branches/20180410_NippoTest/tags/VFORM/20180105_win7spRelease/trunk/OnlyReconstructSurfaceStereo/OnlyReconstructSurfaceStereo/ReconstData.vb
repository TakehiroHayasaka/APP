Imports FBMlib
Imports HalconDotNet

Public Class ReconstData
    Public pair As List(Of ImageSet)
    Public SettingData As SettingsTable
    Public flgSuccess As Boolean
    Public flgSuccessGenStereoModel As Boolean
    Public flgProcessEnd As Boolean = False
    Public cntP3d As HTuple
    Public cntNormal As HTuple
    Public cntTriangle As HTuple
    Public nVec As Point3D
    Public p3D As Point3D
    Public vGrayval As HTuple
    Public GrayR As HTuple
    Public GrayG As HTuple
    Public GrayB As HTuple
    Public indTriangle As HTuple
    Public boundingbox As HTuple

    Public Sub New()
        cntP3d = New HTuple(-1)
        cntTriangle = New HTuple(-1)
        cntNormal = New HTuple(-1)
        nVec = New Point3D
        p3D = New Point3D
        vGrayval = New HTuple
        GrayR = New HTuple
        GrayG = New HTuple
        GrayB = New HTuple
        indTriangle = New HTuple
        flgSuccess = False
        flgSuccessGenStereoModel = False
        pair = Nothing
        SettingData = New SettingsTable
        boundingbox = New HTuple
    End Sub
    Public Sub CalcSmooth(Optional flgIsRun As Boolean = False)
        If flgIsRun = True Then
            CalcSmoothNaiyo()
        End If
    End Sub
    Private Sub SettingSmoothParam(ByRef hv_SmoothParamNames As HTuple, ByRef hv_SmoothParamValues As HTuple)

        hv_SmoothParamNames = (((New HTuple(New HTuple("mls_kNN"))).TupleConcat(
                                            New HTuple("mls_order"))).TupleConcat(
                                            New HTuple("mls_relative_sigma"))).TupleConcat(
                                            New HTuple("mls_force_inwards"))
        hv_SmoothParamValues = (((New HTuple(60)).TupleConcat(
                                             1)).TupleConcat(
                                             0.2)).TupleConcat(
                                             New HTuple("false"))
    End Sub
    Private Sub CalcSmoothNaiyo()
        Dim hv_SmoothObjectModel3D As HTuple = New HTuple
        Dim hv_SmoothParamNames As HTuple = New HTuple
        Dim hv_SmoothParamValues As HTuple = New HTuple
        SettingSmoothParam(hv_SmoothParamNames, hv_SmoothParamValues)
        HOperatorSet.SmoothObjectModel3d(SettingData.hv_ResultObject3D, New HTuple("mls"), hv_SmoothParamNames, hv_SmoothParamValues, hv_SmoothObjectModel3D)
        HOperatorSet.ClearObjectModel3d(SettingData.hv_ResultObject3D)
        HOperatorSet.CopyObjectModel3d(hv_SmoothObjectModel3D, "all", SettingData.hv_ResultObject3D)
        HOperatorSet.ClearObjectModel3d(hv_SmoothObjectModel3D)
    End Sub

    Public Sub CalcMesh(Optional flgIsRun As Boolean = False)
        If flgIsRun = True Then
            CalcMeshNaiyo2()
        End If
    End Sub
    Private Sub CalcMeshNaiyo()
        Dim isMesh As Integer = GetPrivateProfileInt("ReconstParam", "Mesh", 0, My.Application.Info.DirectoryPath & "\vform.ini")
        If isMesh > 0 Then
            Dim hv_NormalObjectModel3d As HTuple = Nothing
            Dim hv_TriangulatedObjectModel3D As HTuple = Nothing
            HOperatorSet.GetObjectModel3dParams(SettingData.hv_ResultObject3D, "num_points", hv_TriangulatedObjectModel3D)
            HOperatorSet.SurfaceNormalsObjectModel3d(SettingData.hv_ResultObject3D, "mls", New HTuple, New HTuple, hv_NormalObjectModel3d)
            HOperatorSet.ClearObjectModel3d(SettingData.hv_ResultObject3D)
            'Dim strParamName As String = "information,greedy_radius_type,greedy_fix_flips,greedy_hole_filling"
            'Dim hv_ParamName As New HTuple(strParamName.Split(","))
            'Dim hv_ParamVal As HTuple = (((New HTuple(New HTuple("verbose"))).TupleConcat(New HTuple("auto"))).TupleConcat("false")).TupleConcat(40)
            Dim hv_Info As New HTuple
            Dim hv_ParameterNames As HTuple = New HTuple
            Dim hv_ParameterValues As HTuple = New HTuple
            'hv_ParameterNames = (((((New HTuple(New HTuple("information"))).TupleConcat(New HTuple("greedy_radius_type"))).TupleConcat( _
            '                         New HTuple("greedy_radius_value"))).TupleConcat(New HTuple("greedy_hole_filling"))).TupleConcat( _
            '                         New HTuple("greedy_fix_flips"))).TupleConcat(New HTuple("greedy_remove_small_surfaces"))
            'hv_ParameterValues = (((((New HTuple(New HTuple("verbose"))).TupleConcat(New HTuple("fixed"))).TupleConcat( _
            '                                       isMesh)).TupleConcat(40)).TupleConcat(New HTuple("false"))).TupleConcat(5)

            hv_ParameterNames = (((((((New HTuple(New HTuple("information"))).TupleConcat(New HTuple("greedy_fix_flips"))).TupleConcat( _
                                       New HTuple("greedy_hole_filling"))).TupleConcat(New HTuple("greedy_remove_small_surfaces"))).TupleConcat( _
                                       New HTuple("greedy_neigh_orient_consistent"))).TupleConcat(New HTuple("greedy_neigh_orient_tol"))).TupleConcat( _
                                       New HTuple("greedy_neigh_latitude_tol"))).TupleConcat(New HTuple("greedy_neigh_vertical_tol"))
            hv_ParameterValues = (((((((New HTuple(New HTuple("verbose"))).TupleConcat(New HTuple("false"))).TupleConcat( _
                400)).TupleConcat(0.002)).TupleConcat(New HTuple("true"))).TupleConcat(40)).TupleConcat( _
                40)).TupleConcat(0.2)


            HOperatorSet.TriangulateObjectModel3d(hv_NormalObjectModel3d, "greedy", hv_ParameterNames, hv_ParameterValues, hv_TriangulatedObjectModel3D, hv_Info)
            HOperatorSet.ClearObjectModel3d(hv_NormalObjectModel3d)
            Dim hv_SmoothObjectModel3D As HTuple = New HTuple
            HOperatorSet.SmoothObjectModel3d(hv_TriangulatedObjectModel3D, New HTuple("mls"), New HTuple(), New HTuple(), hv_SmoothObjectModel3D)
#If DEBUG Then
            HOperatorSet.WriteObjectModel3d(hv_SmoothObjectModel3D, "obj", "TriangulatedObjectTest.obj", New HTuple, New HTuple)
#End If
            'HOperatorSet.TriangulateObjectModel3d(SettingData.hv_ResultObject3D, "implicit", New HTuple, New HTuple, hv_TriangulatedObjectModel3D, New HTuple)
            ' HOperatorSet.TriangulateObjectModel3d(SettingData.hv_ResultObject3D, "polygon_triangulation", New HTuple, New HTuple, hv_TriangulatedObjectModel3D, New HTuple)
            ' HOperatorSet.ClearObjectModel3d(SettingData.hv_ResultObject3D)
            HOperatorSet.CopyObjectModel3d(hv_SmoothObjectModel3D, "all", SettingData.hv_ResultObject3D)
            HOperatorSet.ClearObjectModel3d(hv_TriangulatedObjectModel3D)
            HOperatorSet.ClearObjectModel3d(hv_SmoothObjectModel3D)

        End If
    End Sub
    Private Sub CalcMeshNaiyo2()
        Dim isMesh As Integer = GetPrivateProfileInt("ReconstParam", "Mesh", 0, My.Application.Info.DirectoryPath & "\vform.ini")
        If isMesh > 0 Then
            Dim hv_TriangulatedObjectModel3D As HTuple = Nothing
            Dim hv_SmoothObjectModel3D As HTuple = New HTuple
            'Dim hv_SmoothParamNames As HTuple = New HTuple
            'Dim hv_SmoothParamValues As HTuple = New HTuple

            'SettingSmoothParam(hv_SmoothParamNames, hv_SmoothParamValues)

            'HOperatorSet.GetObjectModel3dParams(SettingData.hv_ResultObject3D, "num_points", hv_TriangulatedObjectModel3D)
            'HOperatorSet.SmoothObjectModel3d(SettingData.hv_ResultObject3D, New HTuple("mls"), hv_SmoothParamNames, hv_SmoothParamValues, hv_SmoothObjectModel3D)
            'HOperatorSet.ClearObjectModel3d(SettingData.hv_ResultObject3D)

            Dim hv_Info As New HTuple
            Dim hv_ParameterNames As HTuple = New HTuple
            Dim hv_ParameterValues As HTuple = New HTuple

            hv_ParameterNames = (((((((New HTuple(New HTuple("information"))).TupleConcat(
                                       New HTuple("greedy_fix_flips"))).TupleConcat(
                                       New HTuple("greedy_hole_filling"))).TupleConcat(
                                       New HTuple("greedy_remove_small_surfaces"))).TupleConcat(
                                       New HTuple("greedy_neigh_orient_consistent"))).TupleConcat(
                                       New HTuple("greedy_neigh_orient_tol"))).TupleConcat(
                                       New HTuple("greedy_neigh_latitude_tol"))).TupleConcat(
                                       New HTuple("greedy_neigh_vertical_tol"))
            hv_ParameterValues = (((((((New HTuple(New HTuple("verbose"))).TupleConcat(
                                       New HTuple("false"))).TupleConcat(
                                       400)).TupleConcat(
                                       0.002)).TupleConcat(
                                       New HTuple("true"))).TupleConcat(
                                       40)).TupleConcat(
                                       40)).TupleConcat(
                                       0.2)

            HOperatorSet.TriangulateObjectModel3d(SettingData.hv_ResultObject3D, "greedy", hv_ParameterNames, hv_ParameterValues, hv_TriangulatedObjectModel3D, hv_Info)
            HOperatorSet.ClearObjectModel3d(SettingData.hv_ResultObject3D)
#If DEBUG Then
            HOperatorSet.WriteObjectModel3d(hv_TriangulatedObjectModel3D, "obj", "TriangulatedObjectTest.obj", New HTuple, New HTuple)
#End If
            HOperatorSet.SmoothObjectModel3d(hv_TriangulatedObjectModel3D, New HTuple("mls"), New HTuple, New HTuple, hv_SmoothObjectModel3D)
            HOperatorSet.ClearObjectModel3d(hv_TriangulatedObjectModel3D)
            HOperatorSet.CopyObjectModel3d(hv_SmoothObjectModel3D, "all", SettingData.hv_ResultObject3D)
            HOperatorSet.ClearObjectModel3d(hv_SmoothObjectModel3D)

        End If
    End Sub

    Public Sub CalcMeshNoParam(Optional flgIsRun As Boolean = False)
        If flgIsRun = True Then
            CalcMeshNoParamNaiyo()
        End If
    End Sub
    Private Sub CalcMeshNoParamNaiyo()
        Dim isMesh As Integer = GetPrivateProfileInt("ReconstParam", "Mesh", 0, My.Application.Info.DirectoryPath & "\vform.ini")
        If isMesh > 0 Then
            Dim hv_NormalObjectModel3d As HTuple = Nothing
            Dim hv_TriangulatedObjectModel3D As HTuple = Nothing
            HOperatorSet.GetObjectModel3dParams(SettingData.hv_ResultObject3D, "num_points", hv_TriangulatedObjectModel3D)
            HOperatorSet.SurfaceNormalsObjectModel3d(SettingData.hv_ResultObject3D, "mls", New HTuple, New HTuple, hv_NormalObjectModel3d)
            HOperatorSet.ClearObjectModel3d(SettingData.hv_ResultObject3D)
            Dim hv_Info As New HTuple

            HOperatorSet.TriangulateObjectModel3d(hv_NormalObjectModel3d, "greedy", New HTuple, New HTuple, hv_TriangulatedObjectModel3D, hv_Info)
            HOperatorSet.ClearObjectModel3d(hv_NormalObjectModel3d)
            Dim hv_SmoothObjectModel3D As HTuple = New HTuple
            HOperatorSet.SmoothObjectModel3d(hv_TriangulatedObjectModel3D, New HTuple("mls"), New HTuple, New HTuple, hv_SmoothObjectModel3D)
#If DEBUG Then
            HOperatorSet.WriteObjectModel3d(hv_SmoothObjectModel3D, "obj", "TriangulatedObjectTest.obj", New HTuple, New HTuple)
#End If
            'HOperatorSet.TriangulateObjectModel3d(SettingData.hv_ResultObject3D, "implicit", New HTuple, New HTuple, hv_TriangulatedObjectModel3D, New HTuple)
            ' HOperatorSet.TriangulateObjectModel3d(SettingData.hv_ResultObject3D, "polygon_triangulation", New HTuple, New HTuple, hv_TriangulatedObjectModel3D, New HTuple)
            ' HOperatorSet.ClearObjectModel3d(SettingData.hv_ResultObject3D)
            HOperatorSet.CopyObjectModel3d(hv_SmoothObjectModel3D, "all", SettingData.hv_ResultObject3D)
            HOperatorSet.ClearObjectModel3d(hv_TriangulatedObjectModel3D)
            HOperatorSet.ClearObjectModel3d(hv_SmoothObjectModel3D)

        End If
    End Sub

    Public Sub CalcSampling(Optional flgIsRun As Boolean = False)
        If flgIsRun = True Then
            CalcSamplingNaiyo()
        End If
    End Sub
    Private Sub CalcSamplingNaiyo()
        Dim hv_SamplingObjectModel3D As HTuple = Nothing
        Dim dblOneMiliMeter As Double = 1 ' / CT_OFFSET_Kanren.ScaleToMM
        Dim tt As HTuple = Nothing
        Dim hv_hasPoint As New HTuple
        Dim Pitch As Integer = GetPrivateProfileInt("ReconstParam", "Sampling", 0, My.Application.Info.DirectoryPath & "\vform.ini")
        If Pitch > 0 Then
            HOperatorSet.GetObjectModel3dParams(SettingData.hv_ResultObject3D, "num_points", tt)

            HOperatorSet.SampleObjectModel3d(SettingData.hv_ResultObject3D, "accurate", New HTuple(dblOneMiliMeter * CDbl(SettingData.sub_sampling_step)), New HTuple("min_num_points"), New HTuple(Pitch), hv_SamplingObjectModel3D)
            HOperatorSet.GetObjectModel3dParams(hv_SamplingObjectModel3D, "num_points", tt)
            HOperatorSet.GetObjectModel3dParams(hv_SamplingObjectModel3D, "has_points", hv_hasPoint)
            If hv_hasPoint.ToString = "true" Then
                HOperatorSet.ClearObjectModel3d(SettingData.hv_ResultObject3D)
                HOperatorSet.CopyObjectModel3d(hv_SamplingObjectModel3D, "all", SettingData.hv_ResultObject3D)
            End If
            HOperatorSet.ClearObjectModel3d(hv_SamplingObjectModel3D)
        End If
    End Sub
    Public Sub CalcSamplingFast(Optional flgIsRun As Boolean = False)
        If flgIsRun = True Then
            CalcSamplingFastNaiyo()
        End If
    End Sub
    Private Sub CalcSamplingFastNaiyo()
        Dim hv_SamplingObjectModel3D As HTuple = Nothing
        Dim dblOneMiliMeter As Double = 1 ' / CT_OFFSET_Kanren.ScaleToMM
        Dim cntPoints As HTuple = Nothing
        Dim hv_hasPoint As New HTuple
        Dim hv_SampleKankaku As HTuple = New HTuple(CDbl(SettingData.sub_sampling_step))
        Dim hv_hasAtr As New HTuple

        HOperatorSet.GetObjectModel3dParams(SettingData.hv_ResultObject3D, "num_points", cntPoints)

        Do
            HOperatorSet.SampleObjectModel3d(SettingData.hv_ResultObject3D, "fast", hv_SampleKankaku, New HTuple, New HTuple, hv_SamplingObjectModel3D)
            HOperatorSet.GetObjectModel3dParams(hv_SamplingObjectModel3D, "num_points", cntPoints)
            HOperatorSet.GetObjectModel3dParams(hv_SamplingObjectModel3D, "has_points", hv_hasPoint)
            HOperatorSet.GetObjectModel3dParams(hv_SamplingObjectModel3D, "num_extended_attribute", hv_hasAtr)
            If hv_hasPoint.ToString = "true" Then
                HOperatorSet.ClearObjectModel3d(SettingData.hv_ResultObject3D)
                HOperatorSet.CopyObjectModel3d(hv_SamplingObjectModel3D, "all", SettingData.hv_ResultObject3D)
                hv_SampleKankaku = New HTuple(hv_SampleKankaku.D + 1)
            End If
            HOperatorSet.ClearObjectModel3d(hv_SamplingObjectModel3D)
            If cntPoints.I < 250000 Then
                Exit Do
            End If
        Loop


    End Sub
    Public Sub RunCleanUp(Optional flgIsRun As Boolean = False)
        If flgIsRun = True Then
            RunCleanUpNaiyo()
        End If
    End Sub
    Private Sub RunCleanUpNaiyo()
        Dim hv_ConnectDistance As New HTuple(2.0)
        Dim hv_Connected As New HTuple
        Dim hv_Selected As New HTuple
        Dim hv_UnionObject3D As New HTuple
        Dim hv_numPoint As New HTuple
        Dim hv_MaxNum As New HTuple
        Dim hv_hasPoint As New HTuple
        'HOperatorSet.SmoothObjectModel3d(hv_HalconObjectModel3d, "mls", New HTuple("mls_force_inwards", "mls_kNN"), _
        '                                 New HTuple("true", 400), hv_SmoothObject3D)
        Dim Pitch As Integer = GetPrivateProfileInt("ReconstParam", "Sampling", 0, My.Application.Info.DirectoryPath & "\vform.ini")
        If Double.Parse(SettingData.cleanup_min) > 0 Then
            HOperatorSet.ConnectionObjectModel3d(SettingData.hv_ResultObject3D, "distance_3d", New HTuple(Double.Parse(SettingData.cleanup_min) * Pitch), hv_Connected)
            HOperatorSet.GetObjectModel3dParams(hv_Connected, "num_points", hv_numPoint)
            HOperatorSet.TupleMax(hv_numPoint, hv_MaxNum)
            HOperatorSet.TupleAdd(hv_MaxNum, New HTuple(1), hv_MaxNum)
            HOperatorSet.SelectObjectModel3d(hv_Connected, "num_points", "and", New HTuple(Double.Parse(SettingData.cleanup_min) * Pitch * 3), hv_MaxNum, hv_Selected)
            HOperatorSet.ClearObjectModel3d(hv_Connected)
            HOperatorSet.UnionObjectModel3d(hv_Selected, "points_surface", hv_UnionObject3D)
            HOperatorSet.ClearObjectModel3d(hv_Selected)
            HOperatorSet.GetObjectModel3dParams(hv_UnionObject3D, "has_points", hv_hasPoint)
            If hv_hasPoint.ToString = "true" Then
                HOperatorSet.ClearObjectModel3d(SettingData.hv_ResultObject3D)
                HOperatorSet.CopyObjectModel3d(hv_UnionObject3D, "all", SettingData.hv_ResultObject3D)
            End If
            HOperatorSet.ClearObjectModel3d(hv_UnionObject3D)
        End If


    End Sub

    Private Sub RunCleanUpNaiyo2()
        Dim hv_ConnectDistance As New HTuple(2.0)
        Dim hv_Connected As New HTuple
        Dim hv_Selected As New HTuple
        Dim hv_UnionObject3D As New HTuple
        Dim hv_numPoint As New HTuple
        Dim hv_MaxNum As New HTuple
        Dim hv_hasPoint As New HTuple
        'HOperatorSet.SmoothObjectModel3d(hv_HalconObjectModel3d, "mls", New HTuple("mls_force_inwards", "mls_kNN"), _
        '                                 New HTuple("true", 400), hv_SmoothObject3D)
        Dim Pitch As Integer = GetPrivateProfileInt("ReconstParam", "Sampling", 0, My.Application.Info.DirectoryPath & "\vform.ini")
        If Double.Parse(SettingData.cleanup_min) > 0 Then
            'Dim hv_SmoothObjectModel3D As HTuple = New HTuple
            'Dim hv_SmoothParamNames As HTuple = New HTuple
            'Dim hv_SmoothParamValues As HTuple = New HTuple

            'SettingSmoothParam(hv_SmoothParamNames, hv_SmoothParamValues)

            'HOperatorSet.SmoothObjectModel3d(SettingData.hv_ResultObject3D, New HTuple("mls"), hv_SmoothParamNames, hv_SmoothParamValues, hv_SmoothObjectModel3D)

            HOperatorSet.ConnectionObjectModel3d(SettingData.hv_ResultObject3D, "distance_3d", New HTuple(Double.Parse(SettingData.cleanup_min) + Pitch), hv_Connected)
            HOperatorSet.GetObjectModel3dParams(hv_Connected, "num_points", hv_numPoint)
            HOperatorSet.TupleMax(hv_numPoint, hv_MaxNum)
            HOperatorSet.TupleAdd(hv_MaxNum, New HTuple(1), hv_MaxNum)
            HOperatorSet.SelectObjectModel3d(hv_Connected, "num_points", "and", New HTuple(Double.Parse(SettingData.cleanup_min)), hv_MaxNum, hv_Selected)
            HOperatorSet.ClearObjectModel3d(hv_Connected)
            HOperatorSet.UnionObjectModel3d(hv_Selected, "points_surface", hv_UnionObject3D)
            HOperatorSet.ClearObjectModel3d(hv_Selected)
            HOperatorSet.GetObjectModel3dParams(hv_UnionObject3D, "has_points", hv_hasPoint)
            If hv_hasPoint.ToString = "true" Then
                HOperatorSet.ClearObjectModel3d(SettingData.hv_ResultObject3D)
                HOperatorSet.CopyObjectModel3d(hv_UnionObject3D, "all", SettingData.hv_ResultObject3D)
            End If
            HOperatorSet.ClearObjectModel3d(hv_UnionObject3D)
        End If


    End Sub




    Public Function calcColorRGB() As Boolean
        calcColorRGB = False
        Dim OP As New HOperatorSet
        Dim Images As List(Of ImageSet) = pair
        Dim ObjectModel3d As HTuple = SettingData.hv_ResultObject3D
        Dim image As New HObject
        'GC.Collect()
        'GC.WaitForPendingFinalizers()
        ' MsgBox("CalcColorRGB")
        Try
            For Each OneImage As ImageSet In Images
                HOperatorSet.GenEmptyObj(image)
                HOperatorSet.ReadImage(image, OneImage.ImageFullPath)
                Dim HomMat3d As HTuple = Nothing, HomMat3dInvert As HTuple = Nothing, ObjectModelTrans As HTuple = Nothing
                HOperatorSet.PoseToHomMat3d(OneImage.ImagePose.ReConstWorldPose, HomMat3d)
                HOperatorSet.HomMat3dInvert(HomMat3d, HomMat3dInvert)
                HOperatorSet.AffineTransObjectModel3d(ObjectModel3d, HomMat3dInvert, ObjectModelTrans)
                Dim Pnum As New HTuple
                HOperatorSet.GetObjectModel3dParams(ObjectModelTrans, "num_points", Pnum)
                Dim X As New HTuple
                Dim Y As New HTuple
                Dim Z As New HTuple
                Dim Row As New HTuple
                Dim Col As New HTuple
                'Dim Grayval As New HTuple
                Dim GrayR As New HTuple
                Dim GrayG As New HTuple
                Dim GrayB As New HTuple
                HOperatorSet.TupleGenConst(Pnum, New HTuple(0), X)
                HOperatorSet.TupleGenConst(Pnum, New HTuple(0), Y)
                HOperatorSet.TupleGenConst(Pnum, New HTuple(0), Z)
                HOperatorSet.TupleGenConst(Pnum, New HTuple(0), Row)
                HOperatorSet.TupleGenConst(Pnum, New HTuple(0), Col)
                'HOperatorSet.TupleGenConst(Pnum * 3, New HTuple(0), Grayval)
                HOperatorSet.TupleGenConst(Pnum, New HTuple(0), GrayR)
                HOperatorSet.TupleGenConst(Pnum, New HTuple(0), GrayG)
                HOperatorSet.TupleGenConst(Pnum, New HTuple(0), GrayB)

                HOperatorSet.GetObjectModel3dParams(ObjectModelTrans, "point_coord_x", X)
                HOperatorSet.GetObjectModel3dParams(ObjectModelTrans, "point_coord_y", Y)
                HOperatorSet.GetObjectModel3dParams(ObjectModelTrans, "point_coord_z", Z)
                HOperatorSet.ClearObjectModel3d(ObjectModelTrans)


                HOperatorSet.Project3dPoint(X, Y, Z, OneImage.objCamparam.Camparam, Row, Col)


                Dim ImageR As New HObject
                Dim ImageG As New HObject
                Dim ImageB As New HObject
                HOperatorSet.GenEmptyObj(ImageR)
                HOperatorSet.GenEmptyObj(ImageB)
                HOperatorSet.GenEmptyObj(ImageG)
                Try
                    HOperatorSet.Decompose3(image, ImageR, ImageG, ImageB)
                    'HOperatorSet.GetGrayval(image, Row, Col, Grayval)
                    HOperatorSet.GetGrayval(ImageR, Row, Col, GrayR)
                    HOperatorSet.GetGrayval(ImageG, Row, Col, GrayG)
                    HOperatorSet.GetGrayval(ImageB, Row, Col, GrayB)

                Catch ex As Exception
                    '              HOperatorSet.TupleGenConst(Row.TupleLength * 3, New HTuple(0.0), Grayval)
                    Continue For
                End Try

                ClearHObject(image)
                ClearHObject(ImageR)
                ClearHObject(ImageG)
                ClearHObject(ImageB)

                'HOperatorSet.GetObjectModel3dParams(ObjectModel3d, "point_coord_x", X)
                'HOperatorSet.GetObjectModel3dParams(ObjectModel3d, "point_coord_y", Y)
                'HOperatorSet.GetObjectModel3dParams(ObjectModel3d, "point_coord_z", Z)
                'HOperatorSet.TupleLength(X, Me.cntP3d)
                'p3D = New Point3D(X, Y, Z)
                'HOperatorSet.TupleDiv(Grayval, New HTuple(255.0), vGrayval)
                HOperatorSet.TupleDiv(GrayR, New HTuple(255.0), GrayR)
                HOperatorSet.TupleDiv(GrayG, New HTuple(255.0), GrayG)
                HOperatorSet.TupleDiv(GrayB, New HTuple(255.0), GrayB)
                HOperatorSet.SetObjectModel3dAttribMod(ObjectModel3d, "&grayR", "points", GrayR)
                HOperatorSet.SetObjectModel3dAttribMod(ObjectModel3d, "&grayG", "points", GrayG)
                HOperatorSet.SetObjectModel3dAttribMod(ObjectModel3d, "&grayB", "points", GrayB)

                'Dim hstrHasPnormal As New HTuple
                'HOperatorSet.GetObjectModel3dParams(ObjectModel3d, "has_point_normals", hstrHasPnormal)
                'If hstrHasPnormal.ToString = "true" Then
                '    Dim nX As New HTuple, nY As New HTuple, nZ As New HTuple
                '    HOperatorSet.GetObjectModel3dParams(ObjectModel3d, "point_normal_x", nX)
                '    HOperatorSet.GetObjectModel3dParams(ObjectModel3d, "point_normal_y", nY)
                '    HOperatorSet.GetObjectModel3dParams(ObjectModel3d, "point_normal_z", nZ)
                '    Dim normal_cnt As New HTuple
                '    HOperatorSet.TupleLength(nX, normal_cnt)
                '    If normal_cnt.I > 0 Then
                '        nVec.X = nX
                '        nVec.Y = nY
                '        nVec.Z = nZ
                '        cntNormal = normal_cnt
                '    End If
                'End If

                'Dim hstrHasTriangle As New HTuple
                'HOperatorSet.GetObjectModel3dParams(ObjectModel3d, "has_triangles", hstrHasTriangle)
                'If hstrHasTriangle.ToString = "true" Then
                '    Dim hTriangle As New HTuple
                '    HOperatorSet.GetObjectModel3dParams(ObjectModel3d, "triangles", hTriangle)
                '    Dim triangle_cnt As New HTuple
                '    HOperatorSet.TupleLength(hTriangle, triangle_cnt)
                '    If triangle_cnt.I > 0 Then
                '        HOperatorSet.TupleAdd(hTriangle, New HTuple(1), indTriangle)
                '        cntTriangle = triangle_cnt
                '    End If
                'End If
                calcColorRGB = True
                Exit For
            Next
        Catch ex As Exception
            SettingData.strErrorMessage = ex.Message
        End Try


    End Function
    Public Sub WriteObjFile(ByVal strFilePath As String)
        HOperatorSet.WriteObjectModel3d(SettingData.hv_ResultObject3D, "obj", strFilePath & "\" & SettingData.SelectedImageIDs & "_object3d.obj", New HTuple, New HTuple)

    End Sub
    Public Sub SetRGB_to_ObjectModel3d()
        Dim hCopyObject3D As New HTuple
        'メッシュ情報以外をコピーする必要がある。こうしないと色情報が欠落する。
        Dim copyParam As New HTuple("all", "~face_triangle")

        HOperatorSet.CopyObjectModel3d(SettingData.hv_ResultObject3D, copyParam, hCopyObject3D)

        HOperatorSet.ClearObjectModel3d(SettingData.hv_ResultObject3D)
        HOperatorSet.CopyObjectModel3d(hCopyObject3D, "all", SettingData.hv_ResultObject3D)
        HOperatorSet.ClearObjectModel3d(hCopyObject3D)

    End Sub

    Public Sub SetRGB_to_ObjectModel3d_mishiyo()
        Dim hAllObject3D As HTuple
        hAllObject3D = SettingData.hv_ResultObject3D
        Dim Pnum As New HTuple
        Dim ttt As New HTuple
        HOperatorSet.GetObjectModel3dParams(hAllObject3D, "num_points", Pnum)
        Dim GrayR As New HTuple
        Dim GrayG As New HTuple
        Dim GrayB As New HTuple
        Dim tmpP3d As New Point3D
        HOperatorSet.TupleGenConst(Pnum, New HTuple(0), tmpP3d.X)
        HOperatorSet.TupleGenConst(Pnum, New HTuple(0), tmpP3d.Y)
        HOperatorSet.TupleGenConst(Pnum, New HTuple(0), tmpP3d.Z)
        HOperatorSet.TupleGenConst(Pnum, New HTuple(0), GrayR)
        HOperatorSet.TupleGenConst(Pnum, New HTuple(0), GrayG)
        HOperatorSet.TupleGenConst(Pnum, New HTuple(0), GrayB)
        HOperatorSet.GetObjectModel3dParams(hAllObject3D, "point_coord_x", tmpP3d.X)
        HOperatorSet.GetObjectModel3dParams(hAllObject3D, "point_coord_y", tmpP3d.Y)
        HOperatorSet.GetObjectModel3dParams(hAllObject3D, "point_coord_z", tmpP3d.Z)
        'For i As Integer = 0 To Pnum.I - 1
        '    Dim ss As New Point3D(tmpP3d.X.TupleSelect(i), tmpP3d.Y.TupleSelect(i), tmpP3d.Z.TupleSelect(i))
        '    Dim dist As New HTuple
        '    ss.GetDisttoOtherPose(Me.p3D, dist)
        '    Dim minInd As HTuple = dist.TupleSortIndex()
        '    GrayR(i) = Me.GrayR(minInd.TupleSelect(0))
        '    GrayG(i) = Me.GrayG(minInd.TupleSelect(0))
        '    GrayB(i) = Me.GrayB(minInd.TupleSelect(0))

        '    'For j As Integer = 0 To Me.cntP3d.I - 1
        '    '    If tmpP3d.X(i).D = Me.p3D.X(j).D And tmpP3d.Y(i).D = Me.p3D.Y(j).D And tmpP3d.Z(i).D = Me.p3D.Z(j).D Then
        '    '        GrayR(i) = Me.GrayR(j)
        '    '        GrayG(i) = Me.GrayG(j)
        '    '        GrayB(i) = Me.GrayB(j)
        '    '    End If
        '    '    If tmpP3d.X(i).D = Me.p3D.X(j).D And tmpP3d.Y(i).D = Me.p3D.Y(j).D And tmpP3d.Z(i).D = Me.p3D.Z(j).D Then
        '    '        GrayR(i) = Me.GrayR(j)
        '    '        GrayG(i) = Me.GrayG(j)
        '    '        GrayB(i) = Me.GrayB(j)
        '    '    End If
        '    'Next
        'Next

        System.Threading.Tasks.Parallel.For(0, Pnum.I, Sub(i)
                                                           Dim ss As New Point3D(tmpP3d.X.TupleSelect(i), tmpP3d.Y.TupleSelect(i), tmpP3d.Z.TupleSelect(i))
                                                           Dim dist As New HTuple
                                                           ss.GetDisttoOtherPose(Me.p3D, dist)
                                                           Dim minInd As HTuple = dist.TupleSortIndex()
                                                           GrayR(i) = Me.GrayR(minInd.TupleSelect(0))
                                                           GrayG(i) = Me.GrayG(minInd.TupleSelect(0))
                                                           GrayB(i) = Me.GrayB(minInd.TupleSelect(0))
                                                       End Sub)

        HOperatorSet.SetObjectModel3dAttribMod(hAllObject3D, "&grayR", "points", GrayR)
        HOperatorSet.SetObjectModel3dAttribMod(hAllObject3D, "&grayG", "points", GrayG)
        HOperatorSet.SetObjectModel3dAttribMod(hAllObject3D, "&grayB", "points", GrayB)

    End Sub
    Public Sub GetDataFromObjectModel3d()
        Dim hAllObject3D As HTuple
        hAllObject3D = SettingData.hv_ResultObject3D
        With Me
            Dim Pnum As New HTuple
            Dim ttt As New HTuple
            HOperatorSet.GetObjectModel3dParams(hAllObject3D, "num_points", Pnum)
            'Dim GrayR As New HTuple
            'Dim GrayG As New HTuple
            'Dim GrayB As New HTuple
            HOperatorSet.TupleGenConst(Pnum, New HTuple(0), .p3D.X)
            HOperatorSet.TupleGenConst(Pnum, New HTuple(0), .p3D.Y)
            HOperatorSet.TupleGenConst(Pnum, New HTuple(0), .p3D.Z)
            HOperatorSet.TupleGenConst(Pnum, New HTuple(0), .GrayR)
            HOperatorSet.TupleGenConst(Pnum, New HTuple(0), .GrayG)
            HOperatorSet.TupleGenConst(Pnum, New HTuple(0), .GrayB)

            HOperatorSet.GetObjectModel3dParams(hAllObject3D, "point_coord_x", .p3D.X)
            HOperatorSet.GetObjectModel3dParams(hAllObject3D, "point_coord_y", .p3D.Y)
            HOperatorSet.GetObjectModel3dParams(hAllObject3D, "point_coord_z", .p3D.Z)
            .cntP3d = Pnum
            Try
                HOperatorSet.GetObjectModel3dParams(hAllObject3D, "&grayR", .GrayR)
                HOperatorSet.GetObjectModel3dParams(hAllObject3D, "&grayG", .GrayG)
                HOperatorSet.GetObjectModel3dParams(hAllObject3D, "&grayB", .GrayB)
                HOperatorSet.GetObjectModel3dParams(hAllObject3D, "extended_attribute_names", ttt)

            Catch ex As Exception
                HOperatorSet.TupleGenConst(Pnum, New HTuple(0.5), .GrayR)
                HOperatorSet.TupleGenConst(Pnum, New HTuple(0.5), .GrayG)
                HOperatorSet.TupleGenConst(Pnum, New HTuple(0.5), .GrayB)
                HOperatorSet.GetObjectModel3dParams(hAllObject3D, "extended_attribute_names", ttt)
                Debug.Print(ex.Message)
            End Try

            Dim hstrHasPnormal As New HTuple
            HOperatorSet.GetObjectModel3dParams(hAllObject3D, "has_point_normals", hstrHasPnormal)
            If hstrHasPnormal.ToString = "true" Then
                .cntNormal = Pnum
                HOperatorSet.TupleGenConst(.cntNormal, New HTuple(0), .nVec.X)
                HOperatorSet.TupleGenConst(.cntNormal, New HTuple(0), .nVec.Y)
                HOperatorSet.TupleGenConst(.cntNormal, New HTuple(0), .nVec.Z)

                HOperatorSet.GetObjectModel3dParams(hAllObject3D, "point_normal_x", .nVec.X)
                HOperatorSet.GetObjectModel3dParams(hAllObject3D, "point_normal_y", .nVec.Y)
                HOperatorSet.GetObjectModel3dParams(hAllObject3D, "point_normal_z", .nVec.Z)
            End If

            Dim hstrHasTriangle As New HTuple
            HOperatorSet.GetObjectModel3dParams(hAllObject3D, "has_triangles", hstrHasTriangle)
            If hstrHasTriangle.ToString = "true" Then
                Dim hTriangle As New HTuple
                HOperatorSet.GetObjectModel3dParams(hAllObject3D, "num_triangles", .cntTriangle)
                HOperatorSet.TupleGenConst(.cntTriangle, New HTuple(0), hTriangle)
                HOperatorSet.GetObjectModel3dParams(hAllObject3D, "triangles", hTriangle)
                HOperatorSet.TupleAdd(hTriangle, New HTuple(1), .indTriangle)

            End If
        End With
    End Sub

    Public Sub OutPutObjFile()
        Dim savePath As String = projectpath & "\Pdata\" & FileName3D
        Using file As System.IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(savePath, False)
            With Me

                Dim fRow As String = ""
                If .cntNormal.I > 0 Then
                    For vi As Integer = 0 To .p3D.X.Length - 1 Step 1
                        '法線ベクトルの書き込み
                        fRow = "vn " & .nVec.X(vi).D & " " & .nVec.Y(vi).D & " " & .nVec.Z(vi).D
                        file.WriteLine(fRow)

                        '3次元点座標と色情報の書き込み
                        fRow = "v " & .p3D.X(vi).D & " " & .p3D.Y(vi).D & " " & .p3D.Z(vi).D
                        fRow &= " " & .GrayR(vi).D & " " & .GrayG(vi).D & " " & .GrayB(vi).D
                        file.WriteLine(fRow)
                    Next
                Else
                    For vi As Integer = 0 To .p3D.X.Length - 1 Step 1
                        '3次元点座標と色情報の書き込み
                        fRow = "v " & .p3D.X(vi).D & " " & .p3D.Y(vi).D & " " & .p3D.Z(vi).D
                        fRow &= " " & .GrayR(vi).D & " " & .GrayG(vi).D & " " & .GrayB(vi).D
                        file.WriteLine(fRow)
                    Next
                End If

                Dim f_Pnum As New HTuple(0)

                'メッシの書き込み
                If .cntTriangle.I > 0 Then
                    Dim tmpTriangle As HTuple = Nothing
                    HOperatorSet.TupleAdd(.indTriangle, f_Pnum, tmpTriangle)
                    For ti As Integer = 0 To tmpTriangle.Length - 1 Step 3
                        fRow = "f " & tmpTriangle(ti).I & " " & tmpTriangle(ti + 1).I & " " & tmpTriangle(ti + 2).I
                        file.WriteLine(fRow)
                    Next
                End If
                HOperatorSet.TupleAdd(f_Pnum, .cntP3d, f_Pnum)

                file.Close()

            End With
        End Using
    End Sub

    Public Sub GenStereoModel()
        Dim hv_StereoModelID As New HTuple
        Dim hv_CameraSetupModelID As New HTuple
        HOperatorSet.ReadCameraSetupModel(projectpath & "\" & SettingData.SelectedImageIDs & ".csm", hv_CameraSetupModelID)

        HOperatorSet.CreateStereoModel(hv_CameraSetupModelID, "surface_pairwise", New HTuple, New HTuple, hv_StereoModelID)
        HOperatorSet.ClearCameraSetupModel(hv_CameraSetupModelID)
        HOperatorSet.SetStereoModelParam(hv_StereoModelID, "persistence", New HTuple(IIf(SettingData.persistence, 1, 0)))
        HOperatorSet.SetStereoModelParam(hv_StereoModelID, "rectif_interpolation", SettingData.rectif_interpolation)
        HOperatorSet.SetStereoModelParam(hv_StereoModelID, "rectif_sub_sampling", Convert.ToDouble(SettingData.rectif_sub_sampling))
        HOperatorSet.SetStereoModelParam(hv_StereoModelID, "sub_sampling_step", New HTuple(1)) ' CInt(SettingsData.sub_sampling_step)) '1に固定
        HOperatorSet.SetStereoModelParam(hv_StereoModelID, "disparity_method", SettingData.disparity_method)
        HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_method", SettingData.binocular_method)
        HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_num_levels", CInt(SettingData.binocular_num_levels))
        HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_mask_width", CInt(SettingData.binocular_mask_width))
        HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_mask_height", CInt(SettingData.binocular_mask_height))
        HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_texture_thresh", Convert.ToDouble(SettingData.binocular_texture_thresh))
        HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_score_thresh", Convert.ToDouble(SettingData.binocular_score_thresh))
        HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_filter", SettingData.binocular_filter)
        HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_sub_disparity", SettingData.binocular_sub_disparity)
        Try
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "bounding_box", Me.boundingbox)
            SettingData.hv_StereoModelID = hv_StereoModelID
        Catch ex As Exception
            SettingData.hv_StereoModelID = Nothing
        End Try

    End Sub
    '    Public Function GenCameraSetupModel(Optional ByVal showMsg As Boolean = True, Optional ByRef flgRepeat As Boolean = False)
    '        GenCameraSetupModel = False
    '        Dim images As List(Of ImageSet) = Me.pair

    '        Dim hv_T0 As HTuple = Nothing, hv_T1 As HTuple = Nothing, numPoints As HTuple = Nothing
    '        Dim hv_CameraSetupModelID As HTuple = Nothing, hv_CameraParam As HTuple = Nothing ', hv_StereoModelID As HTuple = Nothing
    '        Dim ho_Images As HObject = Nothing
    '        Dim hv_ObjectModel3D As HTuple = Nothing, HM3D As HTuple = Nothing, HM3DI As HTuple = Nothing
    '        Dim hv_PoseIn As HTuple = Nothing, hv_PoseOut As HTuple = Nothing

    '        HOperatorSet.GenEmptyObj(ho_Images)
    '        If False Then
    '            Try
    '                HOperatorSet.ReadCamPar(SettingData.camera_param, hv_CameraParam)
    '                ' HOperatorSet.ReadCamPar(SettingsData.camera_param, hv_CameraParam)
    '            Catch ex As Exception
    '                HOperatorSet.ReadTuple(SettingData.camera_param, hv_CameraParam)
    '            End Try
    '        Else
    '            hv_CameraParam = MainFrm.objFBM.hv_CamparamOut
    '        End If
    '        If BTuple.TupleLength(hv_CameraParam).I <= 0 Then
    '            If showMsg Then
    '                MsgBox("Invalid Camera Parameters")
    '            End If
    '            Throw New Exception("Invalid Camera Parameters")
    '            Exit Function
    '        End If
    '        Dim hv_HomMatModosu As HTuple = Nothing
    '        Dim hv_BoundingBoxByOnlyCT As New HTuple
    '        Try
    '            HOperatorSet.CreateCameraSetupModel(pair.Count, hv_CameraSetupModelID)
    '            'Dim hv_OnePoseHomMat As HTuple = Nothing
    '            'Dim hv_OnePose As HTuple = images(0).ImagePose.Pose
    '            'Dim hv_OnePoseInvert As HTuple = Nothing

    '            'HOperatorSet.PoseToHomMat3d(hv_OnePose, hv_OnePoseHomMat)
    '            'HOperatorSet.HomMat3dInvert(hv_OnePoseHomMat, hv_HomMatModosu)
    '            'HOperatorSet.PoseInvert(hv_OnePose, hv_OnePoseInvert)
    '            If True Then
    '                Dim tmpImagePair As New ImagePairSet(images(0), images(1))
    '                tmpImagePair.GetRelPoseFrom2Pose()
    '                If tmpImagePair.cntComCT <> -1 Then
    '                    Dim hv_XYZ As New HTuple
    '                    Dim hv_Con As New HTuple
    '                    HOperatorSet.GenObjectModel3dFromPoints(tmpImagePair.PairPose.X, tmpImagePair.PairPose.Y, tmpImagePair.PairPose.Z, hv_XYZ)
    '                    HOperatorSet.ConvexHullObjectModel3d(hv_XYZ, hv_Con)
    '                    HOperatorSet.ClearObjectModel3d(hv_XYZ)
    '                    HOperatorSet.GetObjectModel3dParams(hv_Con, "bounding_box1", hv_BoundingBoxByOnlyCT)
    '                    HOperatorSet.ClearObjectModel3d(hv_Con)
    '                Else
    '                    Exit Function
    '                End If
    '                Dim tmpRelPose As New HTuple '= tmpImagePair.GetRelPoseFrom2Pose
    '                '  Dim tmpRelPose As HTuple = tmpImagePair.RelPose(False)
    '                'If tmpRelPose Is Nothing Then
    '                '    Exit Function
    '                'End If

    '                'HOperatorSet.PoseCompose(images(0).ImagePose.Pose, hv_OnePoseInvert, images(0).ImagePose.ReConstPose)
    '                'images(1).ImagePose.ReConstPose = tmpRelPose
    '                'HOperatorSet.TupleMult(tmpRelPose, New HTuple({MainFrm.objFBM.pScaleMM, MainFrm.objFBM.pScaleMM, MainFrm.objFBM.pScaleMM, 1.0, 1.0, 1.0, 1.0}),
    '                '                                                              images(1).ImagePose.ReConstPose)
    '                For index As Integer = 0 To images.Count - 1
    '                    HOperatorSet.TupleMult(images(index).ImagePose.Pose, New HTuple({MainFrm.objFBM.pScaleMM, MainFrm.objFBM.pScaleMM, MainFrm.objFBM.pScaleMM, 1.0, 1.0, 1.0, 1.0}),
    '                                                                                    images(index).ImagePose.ReConstWorldPose)
    '                Next
    '#If DEBUG Then
    '                HOperatorSet.WritePose(images(0).ImagePose.ReConstWorldPose, MainFrm.objFBM.ProjectPath & "\" & images(0).ImageId & "_pose.pos")
    '                HOperatorSet.WritePose(images(1).ImagePose.ReConstWorldPose, MainFrm.objFBM.ProjectPath & "\" & images(1).ImageId & "_pose.pos")
    '#End If
    '                CalcRelPoseBetweenTwoPose(images(0).ImagePose.ReConstWorldPose, images(1).ImagePose.ReConstWorldPose, tmpRelPose)
    '                HOperatorSet.CreatePose(0, 0, 0, 0, 0, 0, "Rp+T", "gba", "point", images(0).ImagePose.ReConstPose)
    '                images(1).ImagePose.ReConstPose = tmpRelPose

    '            Else
    '                For index As Integer = 0 To images.Count - 1
    '                    HOperatorSet.TupleMult(images(index).ImagePose.Pose, New HTuple({MainFrm.objFBM.pScaleMM, MainFrm.objFBM.pScaleMM, MainFrm.objFBM.pScaleMM, 1.0, 1.0, 1.0, 1.0}),
    '                                                                                    images(index).ImagePose.ReConstWorldPose)
    '                    images(index).ImagePose.ReConstPose = images(index).ImagePose.ReConstWorldPose
    '                Next
    '            End If


    '            'HOperatorSet.WriteTuple(tmpImagePair.PairPose.RelPose, MainFrm.objFBM.ProjectPath & "\" & tmpImagePair.IS1.ImageId & "," & tmpImagePair.IS2.ImageId & "_relpose" & ".pss")
    '            For index As Integer = 0 To images.Count - 1

    '                '   HOperatorSet.WriteTuple(images(index).ImagePose.Pose, MainFrm.objFBM.ProjectPath & "\" & images(index).ImageId & ".pss")

    '                HOperatorSet.SetCameraSetupCamParam(hv_CameraSetupModelID, index, "area_scan_polynomial", hv_CameraParam, images(index).ImagePose.ReConstPose)
    '            Next

    '            '  HOperatorSet.CreateStereoModel(hv_CameraSetupModelID, "surface_pairwise", New HTuple, New HTuple, hv_StereoModelID)
    '#If DEBUG Then
    '            HOperatorSet.WriteCameraSetupModel(hv_CameraSetupModelID, MainFrm.objFBM.ProjectPath & "\" & SettingData.SelectedImageIDs & ".csm")
    '#End If
    '            HOperatorSet.ClearCameraSetupModel(hv_CameraSetupModelID)
    '            '======================================SetStereoModel=========================================

    '            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "persistence", New HTuple(IIf(SettingData.persistence, 1, 0)))
    '            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "rectif_interpolation", SettingData.rectif_interpolation)
    '            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "rectif_sub_sampling", Convert.ToDouble(SettingData.rectif_sub_sampling))
    '            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "sub_sampling_step", New HTuple(1)) ' CInt(SettingsData.sub_sampling_step)) '1に固定
    '            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "disparity_method", SettingData.disparity_method)
    '            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_method", SettingData.binocular_method)
    '            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_num_levels", CInt(SettingData.binocular_num_levels))
    '            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_mask_width", CInt(SettingData.binocular_mask_width))
    '            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_mask_height", CInt(SettingData.binocular_mask_height))
    '            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_texture_thresh", Convert.ToDouble(SettingData.binocular_texture_thresh))
    '            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_score_thresh", Convert.ToDouble(SettingData.binocular_score_thresh))
    '            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_filter", SettingData.binocular_filter)
    '            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_sub_disparity", SettingData.binocular_sub_disparity)

    '            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "point_meshing", SettingsData.point_meshing)
    '            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "poisson_depth", Convert.ToInt32(SettingsData.poisson_depth))
    '            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "poisson_solver_divide", Convert.ToInt32(SettingsData.poisson_solver_divide))
    '            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "poisson_samples_per_node", Convert.ToInt32(SettingsData.poisson_samples_per_node))
    '            '======================================SetStereoModel=========================================

    '            '======================================CalculateBoundingBox====================================
    '            Dim ImageRegion As HObject = Nothing, DirectBoundingBox As HTuple = Nothing
    '            Dim ReducedBox As HTuple = Nothing, TempBox As HTuple = Nothing, ConvexBox As HTuple = Nothing, SampleBox As HTuple = Nothing, TriangulatedBox As HTuple = Nothing
    '            Dim margin As Integer = 10
    '            ' HOperatorSet.GenRectangle1(ImageRegion, 0, 0, BTuple.TupleSelect(hv_CameraParam, 11), BTuple.TupleSelect(hv_CameraParam, 10))
    '            HOperatorSet.GenEmptyRegion(ImageRegion)
    '            HOperatorSet.GenRectangle1(ImageRegion, margin, margin, BTuple.TupleSelect(hv_CameraParam, 11) - margin, BTuple.TupleSelect(hv_CameraParam, 10) - margin)
    '            '======================================GenHomMatPyramid====================================
    '            Dim PX As HTuple = Nothing, PY As HTuple = Nothing, PZ As HTuple = Nothing
    '            Dim QX As HTuple = Nothing, QY As HTuple = Nothing, QZ As HTuple = Nothing
    '            Dim XX As HTuple = Nothing, XY As HTuple = Nothing, XZ As HTuple = Nothing

    '            Dim LineOfSightRow As HTuple = Nothing

    '            LineOfSightRow = BTuple.TupleConcat(LineOfSightRow, margin)
    '            LineOfSightRow = BTuple.TupleConcat(LineOfSightRow, margin)
    '            LineOfSightRow = BTuple.TupleConcat(LineOfSightRow, BTuple.TupleSelect(hv_CameraParam, 11) - margin)
    '            LineOfSightRow = BTuple.TupleConcat(LineOfSightRow, BTuple.TupleSelect(hv_CameraParam, 11) - margin)
    '            Dim LineOfSightCol As HTuple = Nothing
    '            LineOfSightCol = BTuple.TupleConcat(LineOfSightCol, margin)
    '            LineOfSightCol = BTuple.TupleConcat(LineOfSightCol, BTuple.TupleSelect(hv_CameraParam, 10) - margin)
    '            LineOfSightCol = BTuple.TupleConcat(LineOfSightCol, BTuple.TupleSelect(hv_CameraParam, 10) - margin)
    '            LineOfSightCol = BTuple.TupleConcat(LineOfSightCol, margin)

    '            Dim HomMat3D As HTuple = Nothing, HomMat3DIdentity As HTuple = Nothing, HomMat3DScale As HTuple = Nothing, Diameter As HTuple = Nothing
    '            Dim LineOfSightPoints As HTuple = Nothing, LineOfSightObject As HTuple = Nothing
    '            Dim LineOfSightObjectSample As HTuple = Nothing, LineOfSightObjectTriangulated As HTuple = Nothing, LineOfSightObjectScaled As HTuple = Nothing

    '            'If Not images(0).Region Is Nothing Then 'TODO
    '            '    Dim r_Rows As HTuple = Nothing, r_Cols As HTuple = Nothing
    '            '    HOperatorSet.GetRegionConvex(CalculateRegion(hv_CameraParam, images(0).Region), r_Rows, r_Cols)
    '            '    LineOfSightRow = r_Rows
    '            '    LineOfSightCol = r_Cols
    '            'End If

    '            HOperatorSet.GetLineOfSight(LineOfSightRow, LineOfSightCol, hv_CameraParam, PX, PY, PZ, QX, QY, QZ)
    '            HOperatorSet.TupleConcat(QX, 0.0, QX)
    '            HOperatorSet.TupleConcat(QY, 0.0, QY)
    '            HOperatorSet.TupleConcat(QZ, 0.0, QZ)
    '            HOperatorSet.PoseToHomMat3d(images(0).ImagePose.ReConstPose, HomMat3D)
    '            HOperatorSet.AffineTransPoint3d(HomMat3D, QX, QY, QZ, XX, XY, XZ)
    '            HOperatorSet.GenObjectModel3dFromPoints(XX, XY, XZ, LineOfSightPoints)
    '            HOperatorSet.ConvexHullObjectModel3d(LineOfSightPoints, LineOfSightObject)
    '            HOperatorSet.ClearObjectModel3d(LineOfSightPoints)
    '            ' Dim hScaleAtPiramid As New HTuple((SettingsData.bounding_box * 1000 / MainFrm.objFBM.pScaleMM) / hv_CameraParam(0))
    '            Dim hScaleAtPiramid As New HTuple((SettingData.bounding_box * 1000) / hv_CameraParam(0))
    '            Try
    '                HOperatorSet.HomMat3dIdentity(HomMat3DIdentity)
    '                HOperatorSet.HomMat3dScale(HomMat3DIdentity, hScaleAtPiramid, hScaleAtPiramid, hScaleAtPiramid, _
    '                                 BTuple.TupleSelect(images(0).ImagePose.ReConstPose, 0), _
    '                                 BTuple.TupleSelect(images(0).ImagePose.ReConstPose, 1), _
    '                                 BTuple.TupleSelect(images(0).ImagePose.ReConstPose, 2), HomMat3DScale)
    '                HOperatorSet.AffineTransObjectModel3d(LineOfSightObject, HomMat3DScale, LineOfSightObjectScaled)
    '                HOperatorSet.ClearObjectModel3d(LineOfSightObject)

    '                HOperatorSet.GetObjectModel3dParams(LineOfSightObjectScaled, "diameter_axis_aligned_bounding_box", Diameter)
    '                HOperatorSet.SampleObjectModel3d(LineOfSightObjectScaled, "fast", BTuple.TupleMult(ReconstructionWindow.SamplingFactor, Diameter), New HTuple, New HTuple, LineOfSightObjectSample)
    '                HOperatorSet.ClearObjectModel3d(LineOfSightObjectScaled)
    '                HOperatorSet.TriangulateObjectModel3d(LineOfSightObjectSample, "greedy", New HTuple, New HTuple, LineOfSightObjectTriangulated, New HTuple)
    '                HOperatorSet.ClearObjectModel3d(LineOfSightObjectSample)

    '                '======================================GenHomMatPyramid====================================
    '                HOperatorSet.CopyObjectModel3d(LineOfSightObjectTriangulated, "all", ConvexBox)
    '                HOperatorSet.ClearObjectModel3d(LineOfSightObjectTriangulated)
    '                Dim hv_ZeroCameraParam As HTuple = Nothing
    '                HOperatorSet.TupleReplace(hv_CameraParam, New HTuple(1, 2, 3, 4, 5), 0, hv_ZeroCameraParam)
    '                For i As Integer = 1 To images.Count - 1
    '                    HOperatorSet.CopyObjectModel3d(ConvexBox, "all", TempBox)
    '                    HOperatorSet.ClearObjectModel3d(ConvexBox)
    '                    HOperatorSet.GetObjectModel3dParams(TempBox, "num_points", numPoints)
    '                    If numPoints.I <= 0 Then
    '                        Debug.Print("")
    '                    End If
    '                    HOperatorSet.PoseToHomMat3d(images(i).ImagePose.ReConstPose, HM3D)
    '                    HOperatorSet.HomMat3dInvert(HM3D, HM3DI)
    '                    HOperatorSet.HomMat3dToPose(HM3DI, hv_PoseIn)
    '                    If images(i).Region Is Nothing Then
    '                        HOperatorSet.ReduceObjectModel3dByView(ImageRegion, TempBox, hv_ZeroCameraParam, hv_PoseIn, ReducedBox)

    '                        'Else
    '                        '    HOperatorSet.ReduceObjectModel3dByView(CalculateRegion(hv_CameraParam, images(i).Region), TempBox, hv_ZeroCameraParam, hv_PoseIn, ReducedBox)
    '                    End If
    '                    HOperatorSet.ClearObjectModel3d(TempBox)
    '                    HOperatorSet.GetObjectModel3dParams(ReducedBox, "num_points", numPoints)
    '                    If numPoints.I <= 0 Then
    '                        If showMsg Then
    '                            MsgBox("バウンディングボックス算出でエラー発生しました。「" & images(i).ImageId & ":" & images(i).ImageName & "」画像のレジョンの設定に誤りがあります。")
    '                        End If

    '                        Exit Function
    '                    End If
    '                    'HOperatorSet.GetObjectModel3dParams(ReducedBox, "bounding_box1", DirectBoundingBox)
    '                    HOperatorSet.ConvexHullObjectModel3d(ReducedBox, ConvexBox)
    '                    HOperatorSet.ClearObjectModel3d(ReducedBox)
    '                    HOperatorSet.GetObjectModel3dParams(ConvexBox, "diameter_axis_aligned_bounding_box", Diameter)
    '                    HOperatorSet.SampleObjectModel3d(ConvexBox, "fast", BTuple.TupleMult(ReconstructionWindow.SamplingFactor, Diameter), New HTuple, New HTuple, SampleBox)
    '                    HOperatorSet.ClearObjectModel3d(ConvexBox)
    '                    HOperatorSet.TriangulateObjectModel3d(SampleBox, "greedy", New HTuple, New HTuple, TriangulatedBox, New HTuple)
    '                    HOperatorSet.ClearObjectModel3d(SampleBox)
    '                    HOperatorSet.CopyObjectModel3d(TriangulatedBox, "all", ConvexBox)
    '                    HOperatorSet.ClearObjectModel3d(TriangulatedBox)
    '                    HOperatorSet.GetObjectModel3dParams(ConvexBox, "bounding_box1", DirectBoundingBox)

    '                Next

    '            Catch ex As Exception
    '                flgRepeat = True
    '                'If Not hv_StereoModelID Is Nothing Then
    '                '    HOperatorSet.ClearStereoModel(hv_StereoModelID)
    '                'End If

    '                SettingData.hv_StereoModelID = Nothing
    '            End Try



    '            '####バウンディングボックスがカメラを中に含んいないかを調べる。
    '            Dim hvCameraInBoundinbox As HTuple = Nothing
    '            Dim isCameraInBoundingbox As Boolean = False
    '            Dim boxpose As HTuple = Nothing
    '            Dim boxL1 As HTuple = Nothing
    '            Dim boxL2 As HTuple = Nothing
    '            Dim boxL3 As HTuple = Nothing
    '            Dim hv_BoundBox As HTuple = Nothing
    '            Dim hv_BBzoomfactor As New HTuple(1.0)
    '            HOperatorSet.SmallestBoundingBoxObjectModel3d(ConvexBox, "axis_aligned", boxpose, boxL1, boxL2, boxL3)
    '            Do
    '                HOperatorSet.GenEmptyObjectModel3d(hv_BoundBox)
    '                HOperatorSet.GenBoxObjectModel3d(boxpose, boxL1 * hv_BBzoomfactor, boxL2 * hv_BBzoomfactor, boxL3 * hv_BBzoomfactor, hv_BoundBox)
    '                For index As Integer = 0 To images.Count - 1
    '                    HOperatorSet.IntersectPlaneObjectModel3d(hv_BoundBox, images(index).ImagePose.ReConstPose, hvCameraInBoundinbox)
    '                    HOperatorSet.GetObjectModel3dParams(hvCameraInBoundinbox, "num_points", numPoints)
    '                    If BTuple.TupleInt(numPoints) > 0 Then
    '                        isCameraInBoundingbox = True
    '                    End If
    '                    HOperatorSet.ClearObjectModel3d(hvCameraInBoundinbox)
    '                Next

    '                If isCameraInBoundingbox = False Or hv_BBzoomfactor < 0.1 Then
    '                    Exit Do
    '                Else
    '                    hv_BBzoomfactor = hv_BBzoomfactor - 0.1
    '                    isCameraInBoundingbox = False
    '                End If
    '                HOperatorSet.ClearObjectModel3d(hv_BoundBox)
    '            Loop
    '            HOperatorSet.GetObjectModel3dParams(hv_BoundBox, "bounding_box1", DirectBoundingBox)
    '            HOperatorSet.ClearObjectModel3d(hv_BoundBox)
    '            '####バウンディングボックスがカメラを中に含んいないかを調べる。

    '            If True Then
    '                HOperatorSet.TupleMult(hv_BoundingBoxByOnlyCT, MainFrm.objFBM.pScaleMM, hv_BoundingBoxByOnlyCT)
    '                'SettingsData.bounding_boxの値をメートル単位とし、CTのみで計算したバウンディングボックスを奥行方向に伸ばす量とする。
    '                '手前
    '                DirectBoundingBox(2) = hv_BoundingBoxByOnlyCT(2) - 500
    '                '奥
    '                DirectBoundingBox(5) = hv_BoundingBoxByOnlyCT(5) + SettingData.bounding_box * 1000
    '                ' DirectBoundingBox = hv_BoundingBoxByOnlyCT
    '                '   DirectBoundingBox(2) = hv_BoundingBoxByOnlyCT(2)

    '            End If



    '            '  HOperatorSet.ClearObj(ImageRegion)
    '            ClearHObject(ImageRegion)
    '            ' HOperatorSet.GetObjectModel3dParams(ConvexBox, "bounding_box1", DirectBoundingBox)
    '            'Dim TemaewoTouku As HTuple = New HTuple(1.0, 1.0, 2.0, 1.0, 1.0, 1.0)
    '            ''  TemaewoTouku = New HTuple(0.5, 0.5, 2.0, 0.5, 0.5, 0.5)
    '            'HOperatorSet.TupleMult(DirectBoundingBox, TemaewoTouku, DirectBoundingBox)
    '            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "bounding_box", ttttbb)
    '            '  HOperatorSet.SetStereoModelParam(hv_StereoModelID, "bounding_box", DirectBoundingBox)
    '            Me.boundingbox = DirectBoundingBox
    '            HOperatorSet.ClearObjectModel3d(ConvexBox)
    '#If DEBUG Then
    '            HOperatorSet.WriteTuple(DirectBoundingBox, MainFrm.objFBM.ProjectPath & "\" & SettingData.SelectedImageIDs & "boundingbox.tpl")

    '#End If
    '            '======================================CalculateBoundingBox====================================

    '            ' ''======================================SetImagePair============================================
    '            ''Dim imagesCount As Int32 = 0
    '            ''imagesCount = images.Count
    '            ''Try
    '            ''    For i As Integer = 0 To imagesCount - 2 Step 1
    '            ''        HOperatorSet.SetStereoModelImagePairs(hv_StereoModelID, i, i + 1)
    '            ''    Next
    '            ''Catch ex As Exception
    '            ''    HOperatorSet.ClearStereoModel(hv_StereoModelID)
    '            ''    SettingsData.hv_StereoModelID = Nothing
    '            ''    Exit Function
    '            ''End Try

    '            ' ''======================================SetImagePair============================================
    '            '    SettingData.hv_StereoModelID = hv_StereoModelID

    '            GenCameraSetupModel = True
    '        Catch ex As Exception
    '            flgRepeat = True
    '            'If Not hv_StereoModelID Is Nothing Then
    '            '    HOperatorSet.ClearStereoModel(hv_StereoModelID)
    '            'End If

    '            '  SettingData.hv_StereoModelID = Nothing

    '        End Try




    '    End Function

    Public Sub New(objPair As List(Of ImageSet), objSettingData As SettingsTable)

        cntP3d = New HTuple(-1)
        cntTriangle = New HTuple(-1)
        cntNormal = New HTuple(-1)
        nVec = New Point3D
        p3D = New Point3D
        vGrayval = New HTuple
        GrayR = New HTuple
        GrayG = New HTuple
        GrayB = New HTuple
        indTriangle = New HTuple
        flgSuccess = False
        flgSuccessGenStereoModel = False
        pair = objPair
        SettingData = New SettingsTable(objSettingData)
        boundingbox = New HTuple
    End Sub
    Public Sub New(ByVal objSettingData As SettingsTable)
        cntP3d = New HTuple(-1)
        cntTriangle = New HTuple(-1)
        cntNormal = New HTuple(-1)
        nVec = New Point3D
        p3D = New Point3D
        vGrayval = New HTuple
        GrayR = New HTuple
        GrayG = New HTuple
        GrayB = New HTuple
        indTriangle = New HTuple
        flgSuccess = False
        flgSuccessGenStereoModel = False
        pair = Nothing
        SettingData = New SettingsTable(objSettingData)
        boundingbox = New HTuple
    End Sub
End Class
