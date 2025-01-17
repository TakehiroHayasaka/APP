﻿Imports System.Windows
Imports ADODB
Imports System.Data

Imports System.Threading
Imports Microsoft.Win32
Imports FBMlib
Imports HalconDotNet

Class ReconstructionWindow

    Private _settings_data As SettingsTable
    Public Property SettingsData() As SettingsTable
        Get
            Return _settings_data
        End Get
        Set(ByVal value As SettingsTable)
            _settings_data = value
        End Set
    End Property

    Public Const SamplingFactor As Double = 0.03

    Public Const PairScoreThreshold As Double = 0

    Public Const MaxPairCount As Integer = 2

    Private selectedImageSet As List(Of ImageSet) = New List(Of ImageSet)

    Public Const FileName3D As String = "Reconstructed.obj"

    Private progressThread As Thread

    Private progressModal As frmProgressBar

    Private progressImageCount As Integer

    Private isProgressUp As Boolean = False

    Private isProgressCanceled As Boolean = False

    Private isProgressFinished As Boolean = False

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        ReconstructionWindow.DataContext = Me
        SettingsData = New SettingsTable
        SettingsData.m_dbClass = New CDBOperateOLE
        SettingsData.m_dbClass.Connect("TGless.mdb")
        SettingsData.GetLatest()
        SettingsData.m_dbClass.DisConnect()
        SettingsData.KoujiPath = MainFrm.objFBM.ProjectPath
        SettingsData.image_size_width = MainFrm.objFBM.hv_Width.D
        SettingsData.image_size_height = MainFrm.objFBM.hv_Height.D

    End Sub


    Private Sub Button_Run_Click(sender As Object, e As RoutedEventArgs)
#If True Then
        Try
            If Validation() Then
                MsgBox("Invalid Reconstruction Parameters")
                Exit Sub
            End If
            If Not ReconstructScene(SettingsData, selectedImageSet) Then
                Exit Sub
            End If
            SettingsData.SelectedImageIDs = ""
            Dim comma As String = ""
            For i As Integer = 0 To selectedImageSet.Count - 1
                SettingsData.SelectedImageIDs = SettingsData.SelectedImageIDs & comma & selectedImageSet(i).ImageId
                comma = ","
            Next
            If SettingsData.m_dbClass.Connect("TGless.mdb") Then
                SettingsData.InsertData()
                SettingsData.GetLatest()
                SettingsData.m_dbClass.DisConnect()
                MsgBox("Save complete")
            Else
                MsgBox("Save failed")
            End If
            Call Command_previewreconstruct()
            'MainFrm.PreviewReconstructObject()
        Catch ex As Exception

        End Try
        'Dim thread As New Thread(AddressOf Command_previewreconstruct)
        'thread.Start()
#Else

        If System.IO.File.Exists(MainFrm.objFBM.ProjectPath & "\Pdata\" & "ReconstResultOm3.om3") = False Then

            Exit Sub
        End If
        progressImageCount = 4

        progressThread = New Thread(Sub() Me.ReSampleAndReMesh())
        progressThread.Start()

        StartProgress()


        MsgBox("再サンプリング処理が完了しました。")

        'hv_HalconObjectModel3d = Nothing
        'HOperatorSet.ReadObjectModel3d(MainFrm.objFBM.ProjectPath & "\Pdata\ReconstResultOm3.om3", New HTuple(1), New HTuple, New HTuple, hv_HalconObjectModel3d, New HTuple)
        Command_loadreconstruct()

        HOperatorSet.ClearAllObjectModel3d()

        Me.Close()
#End If
      
    End Sub

    Private Sub Button_Smooth_Click(sender As Object, e As RoutedEventArgs)
        Dim dbClass = New CDBOperate()
        If dbClass.ConnectDB("TGLess.mdb") Then
            Try
                Dim recordSet = dbClass.CreateRecordset("SELECT TOP 1 * FROM 設定値及び結果 WHERE KoujiPath='" & MainFrm.objFBM.ProjectPath & "' ORDER BY ID DESC")
                If recordSet.RecordCount = 1 Then
                    Dim outputPath As String = recordSet(24).Value
                    If Not IsDBNull(outputPath) Then
                        Dim hv_HalconObjectModel3d As HTuple = Nothing
                        HOperatorSet.ReadObjectModel3d(outputPath, 1, New HTuple, New HTuple, hv_HalconObjectModel3d, New HTuple)
                        If hv_HalconObjectModel3d IsNot Nothing Then

                            Dim hv_SmoothObject3D As HTuple = Nothing
                            Dim hv_Connected As HTuple = Nothing, hv_Selected As HTuple = Nothing
                            'HOperatorSet.SmoothObjectModel3d(hv_HalconObjectModel3d, "mls", New HTuple("mls_force_inwards", "mls_kNN"), _
                            '                                 New HTuple("true", 400), hv_SmoothObject3D)
                            HOperatorSet.ConnectionObjectModel3d(hv_HalconObjectModel3d, "distance_3d", 0.02, hv_Connected)
                            HOperatorSet.ClearObjectModel3d(hv_HalconObjectModel3d)
                            HOperatorSet.SelectObjectModel3d(hv_Connected, "diameter_bounding_box", "and", 0.05, 1000, hv_Selected)
                            HOperatorSet.ClearObjectModel3d(hv_Connected)
                            HOperatorSet.UnionObjectModel3d(hv_Selected, "points_surface", hv_SmoothObject3D)
                            HOperatorSet.ClearObjectModel3d(hv_Selected)
                            HOperatorSet.WriteObjectModel3d(hv_SmoothObject3D, "obj", outputPath, New HTuple, New HTuple)
                            HOperatorSet.ClearObjectModel3d(hv_SmoothObject3D)
                            MsgBox("Smoothing done.")
                            Call Command_previewreconstruct()
                        Else
                            MsgBox("Failed to load object model")
                        End If
                    End If
                Else
                    MsgBox("No scenes to smooth. Please run reconstruct scene again")
                End If
            Finally
                dbClass.DisConnectDB()
            End Try
        End If
    End Sub

    Private Sub StartProgress()
        progressModal = New frmProgressBar
        progressModal.Button1.Visible = True
        progressModal.Label1.Text = "Reconstructing..."
        progressModal.ProgressBar1.Minimum = 0
        progressModal.ProgressBar1.Maximum = progressImageCount
        progressModal.ProgressBar1.Step = 1
        'progressModal.TopLevel = True
        progressModal.TopMost = True
        progressModal.ShowDialog()
    End Sub

    Private Sub Button_All_Click(sender As Object, e As RoutedEventArgs)
        Dim sw As System.Diagnostics.Stopwatch
        sw = New System.Diagnostics.Stopwatch
        sw.Start()



        If System.IO.File.Exists(MainFrm.objFBM.ProjectPath & "\Pdata\" & ReconstructionWindow.FileName3D) Then
            System.IO.File.Delete(MainFrm.objFBM.ProjectPath & "\Pdata\" & ReconstructionWindow.FileName3D)
        End If
        Dim imageList As List(Of ImageSet) = New List(Of ImageSet)
        For Each Image As ImageSet In MainFrm.objFBM.lstImages
            If Image.flgConnected = True Then
                imageList.Add(Image)
            End If
        Next

        Dim imagePairs As List(Of List(Of ImageSet)) = FindNeighborCameras3(imageList)
        'tameshi イメージ2枚のペアではなくすべての画像をリストにして一気にによってみる　20170606 START
        If False Then
            imagePairs.Clear()
            imagePairs.Add(imageList)
        End If
        'tameshi イメージ2枚のペアではなくすべての画像をリストにして一気にによってみる　20170606 END


        '201700602 hishgee(Susano) add.start
        Dim lstIndx As New List(Of Integer)
        lstIndx = IsSamePair(imagePairs)
        Dim cnt As Integer = 0
        For Each ii As Integer In lstIndx
            imagePairs.RemoveAt(ii - cnt)
            cnt += 1
        Next
        '20170602 hishgee(Susano) add.end
        progressImageCount = imagePairs.Count * 3
        'Monitor Pair output
        Dim strMonFile As String = ""
        For Each Pair As List(Of ImageSet) In imagePairs
            Dim strLine As String = ""
            For Each Img As ImageSet In Pair
                strLine = strLine & Img.ImageId & ":" & Img.ImageName & " "
            Next
            strMonFile = strMonFile & strLine & vbNewLine
        Next
        My.Computer.FileSystem.WriteAllText(MainFrm.objFBM.ProjectPath & "\Mon_Reconstuct_Pair.txt", strMonFile, False)

        progressThread = New Thread(Sub() Me.AsyncRunAll_withMesh(imagePairs))
        progressThread.Start()

        StartProgress()
        sw.Stop()

        My.Computer.FileSystem.WriteAllText(MainFrm.objFBM.ProjectPath & "\Mon_Reconstuct_Pair.txt", "TGレス解析時間:" & sw.Elapsed.TotalSeconds & "秒", True)
        'System.Diagnostics.Process.Start("C:\Program Files\VCG\MeshLab\meshlab.exe", MainFrm.objFBM.ProjectPath & "\Pdata\Reconstructed.obj")

        MsgBox("3次元点群算出完了しました。")

        hv_HalconObjectModel3d = Nothing
        '  Dim hv_ResultObject3d As New HTuple
        HOperatorSet.ReadObjectModel3d(MainFrm.objFBM.ProjectPath & "\Pdata\ReconstResultOm3.om3", New HTuple(1), New HTuple, New HTuple, hv_HalconObjectModel3d, New HTuple)
        'FitPlanesOptimal(hv_ResultObject3d, hv_HalconObjectModel3d)
        'HOperatorSet.ClearObjectModel3d(hv_ResultObject3d)

        '  Command_loadreconstruct()
        HOperatorSet.ClearAllObjectModel3d()
         Me.Close()
    End Sub

   

    '20170602 hishgee(Susano) add.start
    Private Function IsSamePair(ByVal imageList As List(Of List(Of ImageSet))) As List(Of Integer)
        IsSamePair = New List(Of Integer)
        For i As Integer = i To imageList.Count - 2
            Dim elemOfImgPair As Integer = 0
            For y As Integer = 0 To imageList(i).Count - 1
                Dim dd As ImageSet = imageList(i).Item(y)
                If imageList(i + 1).Contains(dd) Then
                    elemOfImgPair += 1
                    If elemOfImgPair = imageList(i).Count Then
                        IsSamePair.Add(i + 1)
                    End If
                End If
            Next
        Next
    End Function
    '20170602 hishgee(Susano) add.end

    Private Sub AsyncRunAll(imagePairs As List(Of List(Of ImageSet)))
        While True
            If progressModal IsNot Nothing Then
                Exit While
            End If
        End While
        If SettingsData.m_dbClass.Connect("TGless.mdb") Then
            Try
                For Each pair As List(Of ImageSet) In imagePairs
                    If progressModal.isCancelClicked = True Then
                        Exit For
                    End If
                    SettingsData.SelectedImageIDs = ""
                    Dim comma As String = ""
                    For k As Integer = 0 To pair.Count - 1
                        SettingsData.SelectedImageIDs = SettingsData.SelectedImageIDs & comma & pair(k).ImageId
                        comma = ","
                    Next
                    If GenStereoModel(SettingsData, pair, False) = True Then
                        If ReconstructScene2(SettingsData, pair, False) = True Then
                            WriteObjFileWithColor(pair, SettingsData.hv_ResultObject3D, SettingsData.OutputPath)
                            HOperatorSet.ClearObjectModel3d(SettingsData.hv_ResultObject3D)
                            SettingsData.InsertData()
                            If Not UnionScene(SettingsData.OutputPath) Then
                                Throw New Exception("Union Scene Failed")
                            Else
                                drawingDone = False
                                Call Command_loadreconstruct()
                                While True
                                    If drawingDone = True Then
                                        Exit While
                                    End If
                                End While
                            End If
                            My.Computer.FileSystem.WriteAllText(MainFrm.objFBM.ProjectPath & "\Mon_Reconstuct_Pair.txt", "成功ペアのImageIDs:" & SettingsData.SelectedImageIDs & vbNewLine, True)
                        Else
                            My.Computer.FileSystem.WriteAllText(MainFrm.objFBM.ProjectPath & "\Mon_Reconstuct_Pair.txt", "失敗ペアのImageIDs:" & SettingsData.SelectedImageIDs & vbNewLine, True)

                        End If
                    End If
                    progressModal.ProgressUp()
                Next
                progressModal.FinishDialog()
            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                SettingsData.m_dbClass.DisConnect()
            End Try
        End If
    End Sub


    Private Sub AsyncRunAll2(imagePairs As List(Of List(Of ImageSet)))
        While True
            If progressModal IsNot Nothing Then
                Exit While
            End If
        End While

        Try
            Dim lstReconstData As New List(Of ReconstData)
            For Each pair As List(Of ImageSet) In imagePairs
                Dim objReconstData As New ReconstData(pair, SettingsData)
                lstReconstData.Add(objReconstData)
            Next
            Dim savePath As String = MainFrm.objFBM.ProjectPath & "\Pdata\" & ReconstructionWindow.FileName3D
            'System.Threading.Tasks.Parallel.For(1, lstReconstData.Count + 1, Sub(i)
            '                                                                     Dim objReconstdata As ReconstData = lstReconstData.Item(i)
            '                                                                     If ReconstructScene2(objReconstdata.SettingData, objReconstdata.pair, False) = True Then
            '                                                                         objReconstdata.flgSuccess = True
            '                                                                     Else
            '                                                                         objReconstdata.flgSuccess = False
            '                                                                     End If

            '                                                                 End Sub)
            For Each objReconstData As ReconstData In lstReconstData

                If objReconstData.SettingData.m_dbClass.Connect("TGless.mdb") Then
                    If progressModal.isCancelClicked = True Then
                        Exit For
                    End If
                    objReconstData.SettingData.SelectedImageIDs = ""
                    Dim comma As String = ""
                    For k As Integer = 0 To objReconstData.pair.Count - 1
                        objReconstData.SettingData.SelectedImageIDs = objReconstData.SettingData.SelectedImageIDs & comma & objReconstData.pair(k).ImageId
                        comma = ","
                    Next
                    If GenStereoModel(objReconstData.SettingData, objReconstData.pair, False) = True Then
                        If ReconstructScene2(objReconstData.SettingData, objReconstData.pair, False) = True Then
                            WriteObjFileWithColor(objReconstData.pair, objReconstData.SettingData.hv_ResultObject3D, savePath)
                            HOperatorSet.ClearObjectModel3d(objReconstData.SettingData.hv_ResultObject3D)
                            objReconstData.flgSuccess = True
                        End If
                    End If
                    If objReconstData.flgSuccess = True Then
                        objReconstData.SettingData.InsertData()
                        My.Computer.FileSystem.WriteAllText(MainFrm.objFBM.ProjectPath & "\Mon_Reconstuct_Pair.txt", "成功ペアのImageIDs:" & objReconstData.SettingData.SelectedImageIDs & vbNewLine, True)
                    Else
                        My.Computer.FileSystem.WriteAllText(MainFrm.objFBM.ProjectPath & "\Mon_Reconstuct_Pair.txt", "失敗ペアのImageIDs:" & objReconstData.SettingData.SelectedImageIDs & vbNewLine, True)

                    End If
                    progressModal.ProgressUp()
                    objReconstData.SettingData.m_dbClass.DisConnect()
                End If
            Next

            'System.Threading.Tasks.Parallel.For(0, lstReconstData.Count, Sub(i)
            '                                                                 Dim objReconstdata As ReconstData = lstReconstData.Item(i)
            '                                                                 If objReconstdata.flgSuccess = True Then
            '                                                                     WriteObjFileWithColor(objReconstdata.pair, objReconstdata.SettingData.hv_ResultObject3D, objReconstdata.SettingData.OutputPath)
            '                                                                     HOperatorSet.ClearObjectModel3d(objReconstdata.SettingData.hv_ResultObject3D)
            '                                                                 End If
            '                                                             End Sub)

            'Dim savePath As String = MainFrm.objFBM.ProjectPath & "\Pdata\" & ReconstructionWindow.FileName3D
            'Using writer = New System.IO.FileStream(savePath, System.IO.FileMode.Append)
            '    System.Threading.Tasks.Parallel.For(0, lstReconstData.Count, Sub(i)
            '                                                                     Dim objReconstdata As ReconstData = lstReconstData.Item(i)
            '                                                                     If objReconstdata.flgSuccess = True Then
            '                                                                         UnionScene2(objReconstdata.SettingData.OutputPath, writer)
            '                                                                     End If
            '                                                                 End Sub)
            'End Using

            'For Each objReconstData As ReconstData In lstReconstData
            '    If objReconstData.flgSuccess = True Then
            '        If Not UnionScene(objReconstData.SettingData.OutputPath) Then

            '        End If
            '    End If
            'Next

            drawingDone = False
            Call Command_loadreconstruct()
            While True
                If drawingDone = True Then
                    Exit While
                End If
            End While

            progressModal.FinishDialog()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            SettingsData.m_dbClass.DisConnect()
        End Try

    End Sub

    Private Function CreateListReconstData(ByVal imagePairs As List(Of List(Of ImageSet))) As List(Of ReconstData)
        Dim lstReconstData As New List(Of ReconstData)
        For Each pair As List(Of ImageSet) In imagePairs
            Dim objReconstData As New ReconstData(pair, SettingsData)
            objReconstData.SettingData.SelectedImageIDs = ""
            Dim comma As String = ""
            For k As Integer = 0 To objReconstData.pair.Count - 1
                objReconstData.SettingData.SelectedImageIDs = objReconstData.SettingData.SelectedImageIDs & comma & objReconstData.pair(k).ImageId
                comma = ","
            Next
            lstReconstData.Add(objReconstData)
        Next

        Dim cntSuccesGenStereoModel As Integer = 0
        My.Computer.FileSystem.WriteAllText(MainFrm.objFBM.ProjectPath & "\Mon_Reconstuct_Pair.txt", "ペア仮計算の開始" & vbNewLine, True)

        For Each objReconstData As ReconstData In lstReconstData
            Dim flgRepeat As Boolean = False
            If objReconstData.flgSuccessGenStereoModel = False Then

                If GenStereoModel(objReconstData.SettingData, objReconstData.pair, False, flgRepeat) = True Then
                    objReconstData.flgSuccessGenStereoModel = True
                    My.Computer.FileSystem.WriteAllText(MainFrm.objFBM.ProjectPath & "\Mon_Reconstuct_Pair.txt", "成功ペア仮計算のImageIDs:" & objReconstData.SettingData.SelectedImageIDs & vbNewLine, True)
                Else
                    If flgRepeat = False Then
                        cntSuccesGenStereoModel = cntSuccesGenStereoModel + 1
                    End If
                    My.Computer.FileSystem.WriteAllText(MainFrm.objFBM.ProjectPath & "\Mon_Reconstuct_Pair.txt", "失敗ペア仮計算のImageIDs:" & objReconstData.SettingData.SelectedImageIDs & vbNewLine, True)
                End If
            End If
            If objReconstData.flgSuccessGenStereoModel = True Then
                cntSuccesGenStereoModel = cntSuccesGenStereoModel + 1
            End If
            progressModal.ProgressUp()
        Next
        My.Computer.FileSystem.WriteAllText(MainFrm.objFBM.ProjectPath & "\Mon_Reconstuct_Pair.txt", "ペア仮計算の終了" & vbNewLine, True)
         
        Return lstReconstData
    End Function

    Private Sub AsyncRunAll3(imagePairs As List(Of List(Of ImageSet)))
        While True
            If progressModal IsNot Nothing Then
                Exit While
            End If
        End While

        Try
            Dim lstReconstData As New List(Of ReconstData)
            lstReconstData = CreateListReconstData(imagePairs)

            progressModal.ProgressUp()
            My.Computer.FileSystem.WriteAllText(MainFrm.objFBM.ProjectPath & "\Mon_Reconstuct_Pair.txt", "RECONSTRUCT本体処理開始（並列計算）" & vbNewLine, True)
#If True Then
            System.Threading.Tasks.Parallel.For(0, lstReconstData.Count, Sub(i)
                                                                             Dim objReconstdata As ReconstData = lstReconstData.Item(i)
                                                                             If objReconstdata.flgSuccessGenStereoModel = True Then
                                                                                 If ReconstructScene2(objReconstdata.SettingData, objReconstdata.pair, False) = True Then
                                                                                     objReconstdata.flgSuccess = True
                                                                                     WriteObjFileWithColor(objReconstdata.pair, objReconstdata.SettingData.hv_ResultObject3D, objReconstdata.SettingData.OutputPath)
                                                                                     ' WriteObjFileWithColorAndMesh(objReconstdata.pair, objReconstdata.SettingData.hv_ResultObject3D, objReconstdata.SettingData.OutputPath)
                                                                                 Else
                                                                                     objReconstdata.flgSuccess = False
                                                                                 End If
                                                                             End If
                                                                             progressModal.ProgressUp()
                                                                         End Sub)
#Else
            For Each objReconstData As ReconstData In lstReconstData
                If objReconstData.flgSuccessGenStereoModel = True Then
                    If ReconstructScene2(objReconstData.SettingData, objReconstData.pair, False) = True Then
                        objReconstData.flgSuccess = True
                        WriteObjFileWithColor(objReconstdata.pair, objReconstdata.SettingData.hv_ResultObject3D, objReconstdata.SettingData.OutputPath)
                        'WriteObjFileWithColorAndMesh(objReconstData.pair, objReconstData.SettingData.hv_ResultObject3D, objReconstData.SettingData.OutputPath)
                    Else
                        objReconstData.flgSuccess = False
                    End If
                End If
                progressModal.ProgressUp()
            Next
#End If
           
          
            '  progressModal.ProgressUp()
            My.Computer.FileSystem.WriteAllText(MainFrm.objFBM.ProjectPath & "\Mon_Reconstuct_Pair.txt", "RECONSTRUCT本体処理終了（並列計算）" & vbNewLine, True)
            Dim cntSuccess As Integer = 0
            For Each objReconstData As ReconstData In lstReconstData
                If progressModal.isCancelClicked = True Then
                    Exit For
                End If
                If objReconstData.SettingData.m_dbClass.Connect("TGless.mdb") Then
                    If objReconstData.flgSuccess = True Then
                        ' WriteObjFileWithColor(objReconstData.pair, objReconstData.SettingData.hv_ResultObject3D, savePath)
                        HOperatorSet.ClearObjectModel3d(objReconstData.SettingData.hv_ResultObject3D)
                        If My.Computer.FileSystem.FileExists(objReconstData.SettingData.OutputPath) Then
                            objReconstData.SettingData.InsertData()
                            If Not UnionScene(objReconstData.SettingData.OutputPath) Then

                            End If
                            cntSuccess += 1
                            My.Computer.FileSystem.WriteAllText(MainFrm.objFBM.ProjectPath & "\Mon_Reconstuct_Pair.txt", "成功ペアのImageIDs:" & objReconstData.SettingData.SelectedImageIDs & vbNewLine, True)
                        Else
                            My.Computer.FileSystem.WriteAllText(MainFrm.objFBM.ProjectPath & "\Mon_Reconstuct_Pair.txt", "失敗ペアのImageIDs:" & objReconstData.SettingData.SelectedImageIDs & vbNewLine, True)
                        End If
                    Else
                        My.Computer.FileSystem.WriteAllText(MainFrm.objFBM.ProjectPath & "\Mon_Reconstuct_Pair.txt", "失敗ペアのImageIDs:" & objReconstData.SettingData.SelectedImageIDs & vbNewLine, True)

                    End If
                    objReconstData.SettingData.m_dbClass.DisConnect()
                End If
                progressModal.ProgressUp()
            Next
            My.Computer.FileSystem.WriteAllText(MainFrm.objFBM.ProjectPath & "\Mon_Reconstuct_Pair.txt", "成功ペア数：" & cntSuccess & vbNewLine, True)


            progressModal.FinishDialog()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            SettingsData.m_dbClass.DisConnect()
        End Try

    End Sub

    Private Sub ActiveDevice()
        Dim hv_ComDevices As New HTuple
        HOperatorSet.QueryAvailableComputeDevices(hv_ComDevices)
        If hv_ComDevices.Length > 0 Then
            Dim hv_DeviceHand As New HTuple
            HOperatorSet.OpenComputeDevice(hv_ComDevices.Item(0), hv_DeviceHand)
            HOperatorSet.SetComputeDeviceParam(hv_DeviceHand, New HTuple("asynchronous_execution"), New HTuple("false"))
            '   HOperatorSet.InitComputeDevice(hv_DeviceHand)
            HOperatorSet.ActivateComputeDevice(hv_DeviceHand)

        End If
    End Sub

    Private Sub RunReconstData(ByRef objReconstdata As ReconstData)
        If objReconstdata.flgSuccessGenStereoModel = True Then
            If ReconstructScene2(objReconstdata.SettingData, objReconstdata.pair, False) = True Then
                objReconstdata.CalcSmooth(False)
                objReconstdata.CalcSampling(False)
                objReconstdata.RunCleanUp(False)
                objReconstdata.CalcMesh(False)
                objReconstdata.WriteObjFile(MainFrm.objFBM.ProjectPath)
                If objReconstdata.calcColorRGB() = True Then
                    objReconstdata.flgSuccess = True
                End If
            End If
        End If
    End Sub

    Private Sub ReSampleAndReMesh()
        While True
            If progressModal IsNot Nothing Then
                Exit While
            End If
        End While

        Dim objReconstData As New ReconstData(SettingsData)
        Try
            HOperatorSet.ReadObjectModel3d(MainFrm.objFBM.ProjectPath & "\Pdata\" & "ReconstResultOm3.om3", 1, New HTuple, New HTuple, objReconstData.SettingData.hv_ResultObject3D, New HTuple)
            progressModal.ProgressUp()
            '   objReconstData.GetDataFromObjectModel3d()
            objReconstData.SetRGB_to_ObjectModel3d()

            objReconstData.CalcSamplingFast(True)
            progressModal.ProgressUp()
            objReconstData.CalcMeshNoParam(True)
            progressModal.ProgressUp()
            objReconstData.GetDataFromObjectModel3d()
            progressModal.ProgressUp()
            'OBJ形式ファイルを出力
            objReconstData.OutPutObjFile()
            objReconstData.WriteObjFile(MainFrm.objFBM.ProjectPath & "\Pdata")
            progressModal.ProgressUp()
        Catch ex As Exception
            Debug.Print(ex.Message)
        Finally
            progressModal.FinishDialog()
            SettingsData.m_dbClass.DisConnect()
        End Try
      
      
    End Sub
    Private Sub AsyncRunAll_withMesh(imagePairs As List(Of List(Of ImageSet)))
        While True
            If progressModal IsNot Nothing Then
                Exit While
            End If
        End While

        Try
            ' ActiveDevice()

            Dim lstReconstData As New List(Of ReconstData)
            lstReconstData = CreateListReconstData(imagePairs)
            'GC.Collect()
            'GC.WaitForPendingFinalizers()

            progressModal.ProgressUp()
            My.Computer.FileSystem.WriteAllText(MainFrm.objFBM.ProjectPath & "\Mon_Reconstuct_Pair.txt", "RECONSTRUCT本体処理開始（並列計算）" & vbNewLine, True)
#If True Then
            Dim maxParaCnt As Integer = lstReconstData.Count
            Dim lstParaReconstData As New List(Of ReconstData)
            For Each objReconstDataItem As ReconstData In lstReconstData
                lstParaReconstData.Add(objReconstDataItem)
                If lstParaReconstData.Count = maxParaCnt Then
                    System.Threading.Tasks.Parallel.For(0, lstParaReconstData.Count, Sub(i)
                                                                                         RunReconstData(lstParaReconstData.Item(i))
                                                                                         progressModal.ProgressUp()
                                                                                     End Sub)
                    lstParaReconstData.Clear()
                End If
            Next
            If lstParaReconstData.Count > 0 Then
                System.Threading.Tasks.Parallel.For(0, lstParaReconstData.Count, Sub(i)
                                                                                     RunReconstData(lstParaReconstData.Item(i))
                                                                                     progressModal.ProgressUp()
                                                                                 End Sub)
            End If

#Else
            For Each objReconstData As ReconstData In lstReconstData
                RunReconstData(objReconstdata)
                progressModal.ProgressUp()
            Next
#End If

            My.Computer.FileSystem.WriteAllText(MainFrm.objFBM.ProjectPath & "\Mon_Reconstuct_Pair.txt", "RECONSTRUCT本体処理終了（並列計算）" & vbNewLine, True)

            '  UnionSceneWithMesh(lstReconstData)
            UnionSceneAllResult(lstReconstData)
            Dim cntSuccess As Integer = 0
            For Each objReconstData As ReconstData In lstReconstData
                If progressModal.isCancelClicked = True Then
                    Exit For
                End If
                If objReconstData.SettingData.m_dbClass.Connect("TGless.mdb") Then
                    If objReconstData.flgSuccess = True Then
                        cntSuccess += 1
                        My.Computer.FileSystem.WriteAllText(MainFrm.objFBM.ProjectPath & "\Mon_Reconstuct_Pair.txt", "成功ペアのImageIDs:" & objReconstData.SettingData.SelectedImageIDs & vbNewLine, True)
                        objReconstData.SettingData.InsertData()
                    Else
                        My.Computer.FileSystem.WriteAllText(MainFrm.objFBM.ProjectPath & "\Mon_Reconstuct_Pair.txt", "失敗ペアのImageIDs:" &
                                                            objReconstData.SettingData.SelectedImageIDs & "(" & objReconstData.SettingData.strErrorMessage & ")" & vbNewLine, True)
                    End If
                    'Try
                    '    HOperatorSet.ClearObjectModel3d(objReconstData.SettingData.hv_ResultObject3D)
                    'Catch ex As Exception
                    '    Debug.Print(ex.Message)
                    'End Try
                    objReconstData.SettingData.m_dbClass.DisConnect()
                End If
                progressModal.ProgressUp()
            Next
            My.Computer.FileSystem.WriteAllText(MainFrm.objFBM.ProjectPath & "\Mon_Reconstuct_Pair.txt", "成功ペア数：" & cntSuccess & vbNewLine, True)
            ' HOperatorSet.DeactivateAllComputeDevices()
            drawingDone = False
            Call Command_loadreconstruct()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            progressModal.FinishDialog()
            SettingsData.m_dbClass.DisConnect()
        End Try

    End Sub

    Private Sub UnionSceneWithMesh(ByVal lstReconstData As List(Of ReconstData))
        Dim savePath As String = MainFrm.objFBM.ProjectPath & "\Pdata\" & ReconstructionWindow.FileName3D
        Using file As System.IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(savePath, True)
            For Each objReconstData As ReconstData In lstReconstData
                With objReconstData
                    If .flgSuccess = True Then
                        Dim gi As Integer = 0
                        Dim fRow As String = ""
                        If .cntNormal.I > 0 Then
                            For vi As Integer = 0 To .p3D.X.Length - 1 Step 1

                                '法線ベクトルの書き込み
                                fRow = "vn " & .nVec.X(vi).D & " " & .nVec.Y(vi).D & " " & .nVec.Z(vi).D
                                file.WriteLine(fRow)

                                '3次元点座標と色情報の書き込み
                                fRow = "v " & .p3D.X(vi).D & " " & .p3D.Y(vi).D & " " & .p3D.Z(vi).D
                                fRow &= " " & .vGrayval(gi).D & " " & .vGrayval(gi + 1).D & " " & .vGrayval(gi + 2).D
                                file.WriteLine(fRow)
                                gi += 3
                            Next
                        Else
                            For vi As Integer = 0 To .p3D.X.Length - 1 Step 1
                                '3次元点座標と色情報の書き込み
                                fRow = "v " & .p3D.X(vi).D & " " & .p3D.Y(vi).D & " " & .p3D.Z(vi).D
                                fRow &= " " & .vGrayval(gi).D & " " & .vGrayval(gi + 1).D & " " & .vGrayval(gi + 2).D
                                file.WriteLine(fRow)
                                gi += 3
                            Next
                        End If
                    End If
                End With
            Next

            Dim f_Pnum As New HTuple(0)

            For Each objReconstData As ReconstData In lstReconstData
                With objReconstData
                    If .flgSuccess = True Then
                        Dim fRow As String = ""
                        'メッシの書き込み
                        If .cntTriangle.I > 0 Then
                            Dim tmpTriangle As HTuple = Nothing
                            HOperatorSet.TupleAdd(.indTriangle, f_Pnum, tmpTriangle)
                            For ti As Integer = 0 To .cntTriangle.I - 1 Step 3
                                fRow = "f " & tmpTriangle(ti).I & " " & tmpTriangle(ti + 1).I & " " & tmpTriangle(ti + 2).I
                                file.WriteLine(fRow)
                            Next
                        End If
                        HOperatorSet.TupleAdd(f_Pnum, objReconstData.cntP3d, f_Pnum)
                    End If
                End With
            Next

            file.Close()
        End Using
    End Sub

    Private Sub UnionSceneAllResult(ByVal lstReconstData As List(Of ReconstData))
        Dim hAllObject3DHandles As New HTuple
        Dim hAllObject3D As New HTuple
        '  HOperatorSet.GenEmptyObjectModel3d(hAllObject3DHandles)

        For Each objItemReconstData As ReconstData In lstReconstData
            With objItemReconstData
                If .flgSuccess = True Then
                    HOperatorSet.TupleConcat(hAllObject3DHandles, .SettingData.hv_ResultObject3D, hAllObject3DHandles)

                End If
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
            HOperatorSet.WriteObjectModel3d(hAllObject3D, "om3", MainFrm.objFBM.ProjectPath & "\Pdata\" & "ReconstOm3.om3", New HTuple, New HTuple)
#End If
            .CalcSmooth(True)
            .RunCleanUp(True)
            .CalcSampling(True)
            .CalcSamplingFast(True)
            '.CalcMesh(True)
            .CalcMeshNoParam(True)
            hAllObject3D = .SettingData.hv_ResultObject3D
#If True Then
            HOperatorSet.WriteObjectModel3d(hAllObject3D, "om3", MainFrm.objFBM.ProjectPath & "\Pdata\" & "ReconstResultOm3.om3", New HTuple, New HTuple)
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

    Private Function FindNeighborCameras(ByVal images As List(Of ImageSet)) As List(Of List(Of ImageSet))
        Dim imagePairs As List(Of List(Of ImageSet)) = New List(Of List(Of ImageSet))
        Dim hv_CameraParam As HTuple = Nothing, hv_CameraParamZero As HTuple = Nothing
        Dim ImageRegion As HObject = Nothing
        hv_CameraParam = MainFrm.objFBM.hv_CamparamOut
        hv_CameraParamZero = hv_CameraParam.TupleReplace(New HTuple(1, 2, 3, 4, 5), 0)
        HOperatorSet.GenRectangle1(ImageRegion, 0, 0, hv_CameraParam(11), hv_CameraParam(10))

        For i As Integer = 0 To images.Count - 1 Step 1
            Dim poI As HTuple = images(i).ImagePose.Pose
            Dim pyramid = GenHomMatPyramid(hv_CameraParam, poI, Double.Parse(SettingsData.bounding_box))
            Dim scoreList As Dictionary(Of Integer, Double) = New Dictionary(Of Integer, Double)
            For j As Integer = 0 To images.Count - 1 Step 1
                If i = j Then
                    Continue For
                End If
                Dim poJ As HTuple = images(j).ImagePose.Pose
                Dim iX As Double = poI(0).D, iY As Double = poI(1).D, iZ As Double = poI(2).D
                Dim jX As Double = poJ(0).D, jY As Double = poJ(1).D, jZ As Double = poJ(2).D
                Dim xp As Double = Math.Pow((jX - iX), 2)
                Dim yp As Double = Math.Pow((jY - iY), 2)
                Dim zp As Double = Math.Pow((jZ - iZ), 2)
                Dim distance As Double = Math.Sqrt(xp + yp + zp)
                Dim area As HTuple = Nothing
                Dim HM3D As HTuple = Nothing, jPoseIn As HTuple = Nothing
                Dim ReducedLOS As HTuple = Nothing, ConvexLOS As HTuple = Nothing
                HOperatorSet.PoseToHomMat3d(poJ, HM3D)
                HOperatorSet.HomMat3dInvert(HM3D, HM3D)
                HOperatorSet.HomMat3dToPose(HM3D, jPoseIn)
                HOperatorSet.ReduceObjectModel3dByView(ImageRegion, pyramid, hv_CameraParamZero, jPoseIn, ReducedLOS)
                Try
                    HOperatorSet.ConvexHullObjectModel3d(ReducedLOS, ConvexLOS)
                Catch ex As Exception
                    HOperatorSet.ClearObjectModel3d(ReducedLOS)
                    scoreList.Add(j, 0 / distance)
                    Continue For
                End Try

                HOperatorSet.ClearObjectModel3d(ReducedLOS)
                Try
                    HOperatorSet.AreaObjectModel3d(ConvexLOS, area)
                Catch ex As Exception
                    HOperatorSet.ClearObjectModel3d(ConvexLOS)
                    scoreList.Add(j, 0 / distance)
                    Continue For
                End Try

                HOperatorSet.ClearObjectModel3d(ConvexLOS)
                scoreList.Add(j, area.D / distance)
            Next
            HOperatorSet.ClearObjectModel3d(pyramid)
            Dim scoreSort = From score In scoreList Order By score.Value Descending
            Dim sortedScores As Dictionary(Of Integer, Double) = scoreSort.ToDictionary(Function(p) p.Key, Function(p) p.Value)
            Dim pair As List(Of ImageSet) = New List(Of ImageSet)
            pair.Add(images(i))
            For Each jIndex As Integer In sortedScores.Keys
                If pair.Count >= MaxPairCount Then
                    Exit For
                End If
                If sortedScores(jIndex) >= PairScoreThreshold Then
                    pair.Add(images(jIndex))
                End If
            Next
            imagePairs.Add(pair)
        Next
        HOperatorSet.ClearObj(ImageRegion)
        FindNeighborCameras = imagePairs
    End Function
    Private Function FindNeighborCameras2(ByVal images As List(Of ImageSet)) As List(Of List(Of ImageSet))
        Dim imagePairs As List(Of List(Of ImageSet)) = New List(Of List(Of ImageSet))
        Dim hv_CameraParam As HTuple = Nothing
        Dim ImageRegion As HObject = Nothing
        hv_CameraParam = MainFrm.objFBM.hv_CamparamOut

        For i As Integer = 0 To images.Count - 1 Step 1
            Dim poI As HTuple = images(i).ImagePose.Pose
            Dim scoreList As Dictionary(Of Integer, Double) = New Dictionary(Of Integer, Double)
            For j As Integer = 0 To images.Count - 1 Step 1
                If i = j Then
                    Continue For
                End If
                Dim ImgPair As New ImagePairSet(images(i), images(j))
                ImgPair.calcRelPose()
                If ImgPair.cntComCT <> -1 Then
                    scoreList.Add(j, ImgPair.cntComCT)
                End If
            Next
            Dim scoreSort = From score In scoreList Order By score.Value Descending
            Dim sortedScores As Dictionary(Of Integer, Double) = scoreSort.ToDictionary(Function(p) p.Key, Function(p) p.Value)
            Dim pair As List(Of ImageSet) = New List(Of ImageSet)
            pair.Add(images(i))
            For Each jIndex As Integer In sortedScores.Keys
                If pair.Count >= MaxPairCount Then
                    Exit For
                End If
                If sortedScores(jIndex) >= PairScoreThreshold Then
                    pair.Add(images(jIndex))
                End If
            Next
            imagePairs.Add(pair)
        Next

        FindNeighborCameras2 = imagePairs
    End Function
    Private Function FindNeighborCameras3(ByVal images As List(Of ImageSet)) As List(Of List(Of ImageSet))
        Dim imagePairs As List(Of List(Of ImageSet)) = New List(Of List(Of ImageSet))
        Dim hv_CameraParam As HTuple = Nothing, hv_CameraParamZero As HTuple = Nothing
        Dim ImageRegion As HObject = Nothing
        hv_CameraParam = MainFrm.objFBM.hv_CamparamOut
        hv_CameraParamZero = hv_CameraParam.TupleReplace(New HTuple(1, 2, 3, 4, 5), 0)
        HOperatorSet.GenRectangle1(ImageRegion, 0, 0, hv_CameraParam(11), hv_CameraParam(10))

        For i As Integer = 0 To images.Count - 1 Step 1
            Dim poI As HTuple = images(i).ImagePose.Pose
            Dim pyramid = GenHomMatPyramid(hv_CameraParam, poI, Double.Parse(SettingsData.bounding_box))
            Dim scoreList As Dictionary(Of Integer, Double) = New Dictionary(Of Integer, Double)
            For j As Integer = 0 To images.Count - 1 Step 1
                If i = j Then
                    Continue For
                End If
                If i = 33 And j = 34 Then
                    Debug.Print("ddd")
                End If
                Dim ImgPair As New ImagePairSet(images(i), images(j))
                ImgPair.calcRelPose()
                If ImgPair.cntComCT <> -1 Then

                    Dim poJ As HTuple = images(j).ImagePose.Pose
                    Dim iX As Double = poI(0).D, iY As Double = poI(1).D, iZ As Double = poI(2).D
                    Dim jX As Double = poJ(0).D, jY As Double = poJ(1).D, jZ As Double = poJ(2).D
                    Dim xp As Double = Math.Pow((jX - iX), 2)
                    Dim yp As Double = Math.Pow((jY - iY), 2)
                    Dim zp As Double = Math.Pow((jZ - iZ), 2)
                    Dim distance As Double = Math.Sqrt(xp + yp + zp)
                    Dim area As HTuple = Nothing
                    Dim HM3D As HTuple = Nothing, jPoseIn As HTuple = Nothing
                    Dim ReducedLOS As HTuple = Nothing, ConvexLOS As HTuple = Nothing
                    HOperatorSet.PoseToHomMat3d(poJ, HM3D)
                    HOperatorSet.HomMat3dInvert(HM3D, HM3D)
                    HOperatorSet.HomMat3dToPose(HM3D, jPoseIn)
                    HOperatorSet.ReduceObjectModel3dByView(ImageRegion, pyramid, hv_CameraParamZero, jPoseIn, ReducedLOS)
                    Try
                        HOperatorSet.ConvexHullObjectModel3d(ReducedLOS, ConvexLOS)
                    Catch ex As Exception
                        HOperatorSet.ClearObjectModel3d(ReducedLOS)
                        scoreList.Add(j, 0 / distance)
                        Continue For
                    End Try

                    HOperatorSet.ClearObjectModel3d(ReducedLOS)
                    Try
                        HOperatorSet.AreaObjectModel3d(ConvexLOS, area)
                    Catch ex As Exception
                        HOperatorSet.ClearObjectModel3d(ConvexLOS)
                        scoreList.Add(j, 0 / distance)
                        Continue For
                    End Try

                    HOperatorSet.ClearObjectModel3d(ConvexLOS)
                    ' scoreList.Add(j, area.D * ImgPair.cntComCT / distance)
                    ' scoreList.Add(j, ImgPair.cntComCT / distance)
                    scoreList.Add(j, 1 / distance)
                End If
            Next
            HOperatorSet.ClearObjectModel3d(pyramid)
            Dim scoreSort = From score In scoreList Order By score.Value Descending
            Dim sortedScores As Dictionary(Of Integer, Double) = scoreSort.ToDictionary(Function(p) p.Key, Function(p) p.Value)
            Dim pair As List(Of ImageSet) = New List(Of ImageSet)
            pair.Add(images(i))
            For Each jIndex As Integer In sortedScores.Keys
                If pair.Count >= MaxPairCount Then
                    Exit For
                End If
                If sortedScores(jIndex) >= PairScoreThreshold Then
                    pair.Add(images(jIndex))
                End If
            Next
            imagePairs.Add(pair)
        Next
        '   HOperatorSet.ClearObj(ImageRegion)
        ClearHObject(ImageRegion)
        FindNeighborCameras3 = imagePairs
    End Function

    Private Sub Button_Trang_Click(sender As Object, e As RoutedEventArgs)
        Dim dbClass = New CDBOperate()
        If dbClass.ConnectDB("TGLess.mdb") Then
            Try
                Dim recordSet = dbClass.CreateRecordset("SELECT TOP 1 * FROM 設定値及び結果 WHERE KoujiPath='" & MainFrm.objFBM.ProjectPath & "' ORDER BY ID DESC")
                If recordSet.RecordCount = 1 Then
                    Dim outputPath As String = recordSet(25).Value
                    If Not IsDBNull(outputPath) Then
                        Dim hv_HalconObjectModel3d As HTuple = Nothing
                        HOperatorSet.ReadObjectModel3d(outputPath, 1, New HTuple, New HTuple, hv_HalconObjectModel3d, New HTuple)
                        If hv_HalconObjectModel3d IsNot Nothing Then
                            Dim hv_TriangulatedObjectModel3D As HTuple = Nothing
                            HOperatorSet.TriangulateObjectModel3d(hv_HalconObjectModel3d, "greedy", New HTuple, New HTuple, hv_TriangulatedObjectModel3D, New HTuple)
                            HOperatorSet.ClearObjectModel3d(hv_HalconObjectModel3d)
                            HOperatorSet.WriteObjectModel3d(hv_TriangulatedObjectModel3D, "obj", outputPath, New HTuple, New HTuple)
                            HOperatorSet.ClearObjectModel3d(hv_TriangulatedObjectModel3D)
                            MsgBox("Triangulation Completed")
                            Call Command_previewreconstruct()
                        Else
                            MsgBox("Failed to load object model")
                        End If
                    End If
                Else
                    MsgBox("No scenes to smooth. Please run reconstruct scene again")
                End If
            Finally
                dbClass.DisConnectDB()
            End Try
        End If
    End Sub

    Private Sub Button_Draw_Region_Click(sender As Object, e As RoutedEventArgs)
#If True Then


        Me.Hide()
        Dim imageIndex As Integer = -1
        IOUtil.GetDrawRegion(imageIndex)
        If Not imageIndex = -1 Then
            For Each im As ImageSet In selectedImageSet
                If im.ImageId = MainFrm.objFBM.lstImages(imageIndex).ImageId Then
                    Me.ShowDialog()
                    Exit Sub
                End If
            Next
            If MainFrm.objFBM.lstImages(imageIndex).flgConnected = True Then
                selectedImageSet.Add(MainFrm.objFBM.lstImages(imageIndex))
                MsgBox(imageIndex + 1 & ": " & MainFrm.objFBM.lstImages(imageIndex).ImageName & " Selected")
            Else
                MsgBox(imageIndex + 1 & ": " & MainFrm.objFBM.lstImages(imageIndex).ImageName & " Is Not Connected")
            End If
        End If
        Me.ShowDialog()
#Else

        Dim dbClass = New CDBOperate()
        If dbClass.ConnectDB("TGLess.mdb") Then
            Try
                Dim recordSet = dbClass.CreateRecordset("SELECT TOP 1 * FROM 設定値及び結果 WHERE KoujiPath='" & MainFrm.objFBM.ProjectPath & "' ORDER BY ID DESC")
                If recordSet.RecordCount = 1 Then
                    Dim outputPath As String = recordSet(25).Value
                    If Not IsDBNull(outputPath) Then
                        Dim hv_HalconObjectModel3d As HTuple = Nothing
                        HOperatorSet.ReadObjectModel3d(outputPath, 1, New HTuple, New HTuple, hv_HalconObjectModel3d, New HTuple)
                        If hv_HalconObjectModel3d IsNot Nothing Then
                            Dim hv_TriangulatedObjectModel3D As HTuple = Nothing
                            HOperatorSet.TriangulateObjectModel3d(hv_HalconObjectModel3d, "greedy", New HTuple, New HTuple, hv_TriangulatedObjectModel3D, New HTuple)
                            HOperatorSet.ClearObjectModel3d(hv_HalconObjectModel3d)
                            HOperatorSet.WriteObjectModel3d(hv_TriangulatedObjectModel3D, "obj", outputPath & "_tt.obj", New HTuple(), New HTuple())
                            HOperatorSet.ClearObjectModel3d(hv_TriangulatedObjectModel3D)
                            MsgBox("Triangulation Completed")
                            ' Call Command_previewreconstruct()
                        Else
                            MsgBox("Failed to load object model")
                        End If
                    End If
                Else
                    MsgBox("No scenes to smooth. Please run reconstruct scene again")
                End If
            Finally
                dbClass.DisConnectDB()
            End Try
        End If
#End If

    End Sub

    Private Sub Button_OK_Click(sender As Object, e As RoutedEventArgs)
#If False Then
          Dim dbClass = New CDBOperate()
        If dbClass.ConnectDB("TGLess.mdb") Then
            Try
                Dim recordSet = dbClass.CreateRecordset("SELECT TOP 1 * FROM 設定値及び結果 WHERE KoujiPath='" & MainFrm.objFBM.ProjectPath & "' ORDER BY ID DESC")
                If recordSet.RecordCount = 1 Then
                    Dim outputPath As String = recordSet(25).Value
                    If Not IsDBNull(outputPath) Then
                        If Not UnionScene(outputPath) Then
                            Throw New Exception("Union Scene Failed")
                        Else
                            MsgBox("Union completed")
                        End If
                    End If
                Else
                    MsgBox("No scenes to union. Please run reconstruct scene again")
                End If
            Finally
                dbClass.DisConnectDB()
            End Try
        End If
        Call Command_loadreconstruct()
        Me.Close()
#Else

        If System.IO.File.Exists(MainFrm.objFBM.ProjectPath & "\Pdata\" & "ReconstResultOm3.om3") = False Then

            Exit Sub
        End If
        progressImageCount = 4

        progressThread = New Thread(Sub() Me.ReSampleAndReMesh())
        progressThread.Start()

        StartProgress()

        Command_loadreconstruct()
        MsgBox("再サンプリング処理が完了しました。")

        'hv_HalconObjectModel3d = Nothing
        'HOperatorSet.ReadObjectModel3d(MainFrm.objFBM.ProjectPath & "\Pdata\ReconstResultOm3.om3", New HTuple(1), New HTuple, New HTuple, hv_HalconObjectModel3d, New HTuple)


        HOperatorSet.ClearAllObjectModel3d()

        Me.Close()
#End If
      
    End Sub

    Private Function GenHomMatPyramid(hv_CameraParam As HTuple, hv_Pose As HTuple, Scale As Double) As HTuple
        Dim PX = New HTuple, PY = New HTuple, PZ = New HTuple, QX = New HTuple, QY = New HTuple, QZ = New HTuple, XX = New HTuple, XY = New HTuple, XZ = New HTuple
        Dim LineOfSightRow = New HTuple(0.0, 0.0, hv_CameraParam(11), hv_CameraParam(11)), LineOfSightCol = New HTuple(0.0, hv_CameraParam(10), hv_CameraParam(10), 0.0)
        Dim HomMat3D = New HTuple, HomMat3DIdentity = New HTuple, HomMat3DScale = New HTuple, Diameter = New HTuple, SamplingFactor As HTuple = New HTuple(0.03)
        Dim LineOfSightPoints = New HTuple, LineOfSightObject = New HTuple, LineOfSightObjectSample = New HTuple, LineOfSightObjectTriangulated = New HTuple, LineOfSightObjectScaled = New HTuple
        HOperatorSet.GetLineOfSight(LineOfSightRow, LineOfSightCol, hv_CameraParam, PX, PY, PZ, QX, QY, QZ)
        HOperatorSet.TupleConcat(QX, 0.0, QX)
        HOperatorSet.TupleConcat(QY, 0.0, QY)
        HOperatorSet.TupleConcat(QZ, 0.0, QZ)
        HOperatorSet.PoseToHomMat3d(hv_Pose, HomMat3D)
        HOperatorSet.AffineTransPoint3d(HomMat3D, QX, QY, QZ, XX, XY, XZ)
        HOperatorSet.GenObjectModel3dFromPoints(XX, XY, XZ, LineOfSightPoints)
        HOperatorSet.ConvexHullObjectModel3d(LineOfSightPoints, LineOfSightObject)
        HOperatorSet.ClearObjectModel3d(LineOfSightPoints)
        HOperatorSet.GetObjectModel3dParams(LineOfSightObject, "diameter_axis_aligned_bounding_box", Diameter)
        HOperatorSet.SampleObjectModel3d(LineOfSightObject, "fast", SamplingFactor * Diameter, New HTuple, New HTuple, LineOfSightObjectSample)
        HOperatorSet.ClearObjectModel3d(LineOfSightObject)
        HOperatorSet.TriangulateObjectModel3d(LineOfSightObjectSample, "greedy", New HTuple, New HTuple, LineOfSightObjectTriangulated, New HTuple)
        HOperatorSet.ClearObjectModel3d(LineOfSightObjectSample)
        HOperatorSet.HomMat3dIdentity(HomMat3DIdentity)
        Dim hScaleAtPiramid As New HTuple((Scale * 1000 / MainFrm.objFBM.pScaleMM) / hv_CameraParam(0))
        HOperatorSet.HomMat3dScale(HomMat3DIdentity, hScaleAtPiramid, hScaleAtPiramid, hScaleAtPiramid, hv_Pose(0), hv_Pose(1), hv_Pose(2), HomMat3DScale)
        HOperatorSet.AffineTransObjectModel3d(LineOfSightObjectTriangulated, HomMat3DScale, LineOfSightObjectScaled)
        HOperatorSet.ClearObjectModel3d(LineOfSightObjectTriangulated)
        GenHomMatPyramid = LineOfSightObjectScaled
    End Function

    Private Sub CameraParamSelect_Click(sender As Object, e As RoutedEventArgs)
        Dim fileDialog = New OpenFileDialog()
        'fileDialog.InitialDirectory = Environment.CurrentDirectory & "\SystemFolder\CamParam\"
        Dim result = fileDialog.ShowDialog()
        Select Case result
            Case True
                Dim file = fileDialog.FileName
                SettingsData.camera_param = file
            Case False
                'SettingsData.camera_param = ""
            Case Else
                'SettingsData.camera_param = ""
        End Select
        camera_param.GetBindingExpression(Controls.TextBox.TextProperty).UpdateTarget()
        camera_param.Focus()
        camera_param.CaretIndex = camera_param.Text.Length
        camera_param.ScrollToEnd()
    End Sub

    Private Sub binocular_num_levels_plus_Click(sender As Object, e As RoutedEventArgs)
        SettingsData.binocular_num_levels = Convert.ToString(Convert.ToDecimal(SettingsData.binocular_num_levels) + 1)
        binocular_num_levels.GetBindingExpression(Controls.TextBox.TextProperty).UpdateTarget()
    End Sub

    Private Sub binocular_num_levels_minus_Click(sender As Object, e As RoutedEventArgs)
        SettingsData.binocular_num_levels = Convert.ToString(Convert.ToDecimal(SettingsData.binocular_num_levels) - 1)
        binocular_num_levels.GetBindingExpression(Controls.TextBox.TextProperty).UpdateTarget()
    End Sub

    Private Sub binocular_mask_width_LostFocus(sender As Object, e As RoutedEventArgs)
        Dim textBox As System.Windows.Controls.TextBox = sender
        If textBox.Text <> "" And Not Integer.TryParse(textBox.Text, New Integer()) = False Then
            Dim val = Convert.ToInt32(textBox.Text)
            If val Mod 2 = 0 Then
                val = val - 1
                SettingsData.binocular_mask_width = Convert.ToString(val)
                textBox.Text = Convert.ToString(val)
                'binocular_mask_width.GetBindingExpression(textBox.TextProperty).UpdateTarget()
            End If
        End If
    End Sub

    Private Sub binocular_mask_height_LostFocus(sender As Object, e As RoutedEventArgs)
        Dim textBox As System.Windows.Controls.TextBox = sender
        If textBox.Text <> "" And Not Integer.TryParse(textBox.Text, New Integer()) = False Then
            Dim val = Convert.ToInt32(textBox.Text)
            If val Mod 2 = 0 Then
                val = val - 1
                SettingsData.binocular_mask_height = Convert.ToString(val)
                textBox.Text = Convert.ToString(val)
                'binocular_mask_height.GetBindingExpression(textBox.TextProperty).UpdateTarget()
            End If
        End If
    End Sub

    Public Function Validation()
        Dim Failed = False
        If String.IsNullOrEmpty(SettingsData.camera_param) Then
            Failed = True
        End If
        If String.IsNullOrEmpty(SettingsData.bounding_box) Or Not Double.TryParse(SettingsData.bounding_box, New Double()) Then
            Failed = True
        End If
        If String.IsNullOrEmpty(SettingsData.cleanup_min) Or Not Double.TryParse(SettingsData.cleanup_min, New Double()) Then
            Failed = True
        End If
        If String.IsNullOrEmpty(SettingsData.rectif_sub_sampling) Or Not Double.TryParse(SettingsData.rectif_sub_sampling, New Double()) Then
            Failed = True
        End If
        If String.IsNullOrEmpty(SettingsData.sub_sampling_step) Or Not Integer.TryParse(SettingsData.sub_sampling_step, New Int32()) Then
            Failed = True
        End If
        If String.IsNullOrEmpty(SettingsData.disparity_method) Then
            Failed = True
        ElseIf Not SettingsData.disparity_method = "binocular" Then
            Failed = True
        End If
        If String.IsNullOrEmpty(SettingsData.binocular_num_levels) Or Not Integer.TryParse(SettingsData.binocular_num_levels, New Int32()) Then
            Failed = True
        End If
        If String.IsNullOrEmpty(SettingsData.binocular_mask_width) Or Not Integer.TryParse(SettingsData.binocular_mask_width, New Int32()) Then
            Failed = True
        End If
        If String.IsNullOrEmpty(SettingsData.binocular_mask_height) Or Not Integer.TryParse(SettingsData.binocular_mask_height, New Int32()) Then
            Failed = True
        End If
        If String.IsNullOrEmpty(SettingsData.binocular_texture_thresh) Or Not Double.TryParse(SettingsData.binocular_texture_thresh, New Double()) Then
            Failed = True
        End If
        If String.IsNullOrEmpty(SettingsData.binocular_score_thresh) Or Not Double.TryParse(SettingsData.binocular_score_thresh, New Double()) Then
            Failed = True
        End If
        If String.IsNullOrEmpty(SettingsData.poisson_depth) Or Not Integer.TryParse(SettingsData.poisson_depth, New Int32()) Then
            Failed = True
        Else
            Dim poisson_depth = Integer.Parse(SettingsData.poisson_depth)
            If poisson_depth < 3 Or poisson_depth > 12 Then
                Failed = True
            End If
        End If
        If String.IsNullOrEmpty(SettingsData.poisson_solver_divide) Or Not Integer.TryParse(SettingsData.poisson_solver_divide, New Int32()) Then
            Failed = True
        Else
            Dim poisson_solver_divide = Integer.Parse(SettingsData.poisson_solver_divide)
            Dim poisson_depth = Integer.Parse(SettingsData.poisson_depth)
            If poisson_solver_divide < 3 Or poisson_solver_divide > poisson_depth Then
                Failed = True
            End If
        End If
        If String.IsNullOrEmpty(SettingsData.poisson_samples_per_node) Or Not Integer.TryParse(SettingsData.poisson_samples_per_node, New Int32()) Then
            Failed = True
        End If
        Validation = Failed
    End Function

    Private Sub Window_Closed(sender As Object, e As EventArgs)
        For Each si As ImageSet In selectedImageSet
            ClearHObject(si.Region)
            si.Region = Nothing
        Next
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Public Sub New()

        ' この呼び出しはデザイナーで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。

    End Sub

    Private Sub ReconstructionWindow_Activated(sender As Object, e As EventArgs) Handles Me.Activated

    End Sub

    Private Sub ReconstructionWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

    End Sub
End Class

Public Class ReconstData
    Public pair As List(Of ImageSet)
    Public SettingData As SettingsTable
    Public flgSuccess As Boolean
    Public flgSuccessGenStereoModel As Boolean

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
        Dim Images As List(Of ImageSet) = pair
        Dim ObjectModel3d As HTuple = SettingData.hv_ResultObject3D
        Dim image As New HObject
        'GC.Collect()
        'GC.WaitForPendingFinalizers()
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
        Dim savePath As String = MainFrm.objFBM.ProjectPath & "\Pdata\" & ReconstructionWindow.FileName3D
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
    End Sub
End Class

Public Class SettingsTable

    Public ReadOnly m_strTableName As String = "設定値及び結果"


    Private ReadOnly S_ID As String() = {"ID"}
    Private ReadOnly S_image_size_width As String() = {"image_size_width"}
    Private ReadOnly S_image_size_height As String() = {"image_size_height"}
    Private ReadOnly S_camera_param As String() = {"camera_param"}
    Private ReadOnly S_bounding_box As String() = {"bounding_box"}
    Private ReadOnly S_cleanup_min As String() = {"cleanup_min"}
    Private ReadOnly S_persistence As String() = {"persistence"}
    Private ReadOnly S_rectif_interpolation As String() = {"rectif_interpolation"}
    Private ReadOnly S_rectif_sub_sampling As String() = {"rectif_sub_sampling"}
    Private ReadOnly S_sub_sampling_step As String() = {"sub_sampling_step"}
    Private ReadOnly S_disparity_method As String() = {"disparity_method"}
    Private ReadOnly S_binocular_method As String() = {"binocular_method"}
    Private ReadOnly S_binocular_num_levels As String() = {"binocular_num_levels"}
    Private ReadOnly S_binocular_mask_width As String() = {"binocular_mask_width"}
    Private ReadOnly S_binocular_mask_height As String() = {"binocular_mask_height"}
    Private ReadOnly S_binocular_texture_thresh As String() = {"binocular_texture_thresh"}
    Private ReadOnly S_binocular_score_thresh As String() = {"binocular_score_thresh"}
    Private ReadOnly S_binocular_filter As String() = {"binocular_filter"}
    Private ReadOnly S_binocular_sub_disparity As String() = {"binocular_sub_disparity"}
    Private ReadOnly S_point_meshing As String() = {"point_meshing"}
    Private ReadOnly S_poisson_depth As String() = {"poisson_depth"}
    Private ReadOnly S_poisson_solver_divide As String() = {"poisson_solver_divide"}
    Private ReadOnly S_poisson_samples_per_node As String() = {"poisson_samples_per_node"}
    Private ReadOnly S_KoujiPath As String() = {"KoujiPath"}
    Private ReadOnly S_SelectedImageIDs As String() = {"SelectedImageIDs"}
    Private ReadOnly S_OutputPath As String() = {"OutputPath"}
    Private ReadOnly S_ImageHenkanTime As String() = {"ImageHenkanTime"}
    Private ReadOnly S_ReconstructTime As String() = {"ReconstructTime"}
    Private ReadOnly S_OutputTime As String() = {"OutputTime"}
    Private ReadOnly S_CreateDate As String() = {"CreateDate"}

    Private _ID As String = ""
    Public Property ID() As String
        Get
            Return _ID
        End Get
        Set(ByVal value As String)
            _ID = value
        End Set
    End Property

    Private _image_size_width As String = ""
    Public Property image_size_width() As String
        Get
            Return _image_size_width
        End Get
        Set(ByVal value As String)
            _image_size_width = value
        End Set
    End Property

    Private _image_size_height As String = ""
    Public Property image_size_height() As String
        Get
            Return _image_size_height
        End Get
        Set(ByVal value As String)
            _image_size_height = value
        End Set
    End Property

    Private _camera_param As String = ""
    Public Property camera_param() As String
        Get
            Return _camera_param
        End Get
        Set(ByVal value As String)
            _camera_param = value
        End Set
    End Property

    Private _bounding_box As String = "80"
    Public Property bounding_box() As String
        Get
            Return _bounding_box
        End Get
        Set(ByVal value As String)
            _bounding_box = value
        End Set
    End Property

    Private _cleanup_min As String = "0.05"
    Public Property cleanup_min() As String
        Get
            Return _cleanup_min
        End Get
        Set(ByVal value As String)
            _cleanup_min = value
        End Set
    End Property

    Private _persistence As Boolean = False

    Public Property persistence() As Boolean
        Get
            Return _persistence
        End Get
        Set(ByVal value As Boolean)
            _persistence = value
        End Set
    End Property

    Private _rectif_interpolation As String = "bilinear"
    Public Property rectif_interpolation() As String
        Get
            Return _rectif_interpolation
        End Get
        Set(ByVal value As String)
            _rectif_interpolation = value
        End Set
    End Property

    Private _rectif_sub_sampling As String = "1"
    Public Property rectif_sub_sampling() As String
        Get
            Return _rectif_sub_sampling
        End Get
        Set(ByVal value As String)
            _rectif_sub_sampling = value
        End Set
    End Property

    Private _sub_sampling_step As String = "3"
    Public Property sub_sampling_step() As String
        Get
            Return _sub_sampling_step
        End Get
        Set(ByVal value As String)
            _sub_sampling_step = value
        End Set
    End Property

    Private _disparity_method As String = "binocular"
    Public Property disparity_method() As String
        Get
            Return _disparity_method
        End Get
        Set(ByVal value As String)
            _disparity_method = value
        End Set
    End Property

    Private _binocular_method As String = "ncc"
    Public Property binocular_method() As String
        Get
            Return _binocular_method
        End Get
        Set(ByVal value As String)
            _binocular_method = value
        End Set
    End Property

    Private _binocular_num_levels As String = "3"
    Public Property binocular_num_levels() As String
        Get
            Return _binocular_num_levels
        End Get
        Set(ByVal value As String)
            _binocular_num_levels = value
        End Set
    End Property

    Private _binocular_mask_width As String = "21"
    Public Property binocular_mask_width() As String
        Get
            Return _binocular_mask_width
        End Get
        Set(ByVal value As String)
            _binocular_mask_width = value
        End Set
    End Property

    Private _binocular_mask_height As String = "21"
    Public Property binocular_mask_height() As String
        Get
            Return _binocular_mask_height
        End Get
        Set(ByVal value As String)
            _binocular_mask_height = value
        End Set
    End Property

    Private _binocular_texture_thresh As String = "0"
    Public Property binocular_texture_thresh() As String
        Get
            Return _binocular_texture_thresh
        End Get
        Set(ByVal value As String)
            _binocular_texture_thresh = value
        End Set
    End Property

    Private _binocular_score_thresh As String = "0.8"
    Public Property binocular_score_thresh() As String
        Get
            Return _binocular_score_thresh
        End Get
        Set(ByVal value As String)
            _binocular_score_thresh = value
        End Set
    End Property

    Private _binocular_filter As String = "left_right_check"
    Public Property binocular_filter() As String
        Get
            Return _binocular_filter
        End Get
        Set(ByVal value As String)
            _binocular_filter = value
        End Set
    End Property

    Private _binocular_sub_disparity As String = "interpolation"
    Public Property binocular_sub_disparity() As String
        Get
            Return _binocular_sub_disparity
        End Get
        Set(ByVal value As String)
            _binocular_sub_disparity = value
        End Set
    End Property

    Private _point_meshing As String = "none"
    Public Property point_meshing() As String
        Get
            Return _point_meshing
        End Get
        Set(ByVal value As String)
            _point_meshing = value
        End Set
    End Property

    Private _poisson_depth As String = "8"
    Public Property poisson_depth() As String
        Get
            Return _poisson_depth
        End Get
        Set(ByVal value As String)
            _poisson_depth = value
        End Set
    End Property

    Private _poisson_solver_divide As String = "8"
    Public Property poisson_solver_divide() As String
        Get
            Return _poisson_solver_divide
        End Get
        Set(ByVal value As String)
            _poisson_solver_divide = value
        End Set
    End Property

    Private _poisson_samples_per_node As String = "30"
    Public Property poisson_samples_per_node() As String
        Get
            Return _poisson_samples_per_node
        End Get
        Set(ByVal value As String)
            _poisson_samples_per_node = value
        End Set
    End Property

    Private _KoujiPath As String = ""
    Public Property KoujiPath() As String
        Get
            Return _KoujiPath
        End Get
        Set(ByVal value As String)
            _KoujiPath = value
        End Set
    End Property

    Private _SelectedImageIDs As String = ""
    Public Property SelectedImageIDs() As String
        Get
            Return _SelectedImageIDs
        End Get
        Set(ByVal value As String)
            _SelectedImageIDs = value
        End Set
    End Property

    Private _OutputPath As String = ""
    Public Property OutputPath() As String
        Get
            Return _OutputPath
        End Get
        Set(ByVal value As String)
            _OutputPath = value
        End Set
    End Property

    Private _ImageHenkanTime As String = ""
    Public Property ImageHenkanTime() As String
        Get
            Return _ImageHenkanTime
        End Get
        Set(ByVal value As String)
            _ImageHenkanTime = value
        End Set
    End Property

    Private _ReconstructTime As String = ""
    Public Property ReconstructTime() As String
        Get
            Return _ReconstructTime
        End Get
        Set(ByVal value As String)
            _ReconstructTime = value
        End Set
    End Property

    Private _OutputTime As String = ""
    Public Property OutputTime() As String
        Get
            Return _OutputTime
        End Get
        Set(ByVal value As String)
            _OutputTime = value
        End Set
    End Property

    Private _CreateDate As String = ""
    Public Property CreateDate() As String
        Get
            Return _CreateDate
        End Get
        Set(ByVal value As String)
            _CreateDate = value
        End Set
    End Property


    Public strFieldNames() As String
    Public strFieldTexts() As String
    Public m_dbClass As CDBOperateOLE
    Public hv_ResultObject3D As HTuple
    Public hv_StereoModelID As HTuple
    Public strErrorMessage As String = ""
    Public Sub New()

    End Sub

    Public Sub New(objSettingData As SettingsTable)


        _image_size_width = objSettingData.image_size_width
     
        _image_size_height = objSettingData.image_size_height

        _camera_param = objSettingData.camera_param

        _bounding_box = objSettingData.bounding_box

        _cleanup_min = objSettingData.cleanup_min

        _persistence = objSettingData.persistence

        _rectif_interpolation = objSettingData.rectif_interpolation

        _rectif_sub_sampling = objSettingData.rectif_sub_sampling

        _sub_sampling_step = objSettingData.sub_sampling_step

        _disparity_method = objSettingData.disparity_method

        _binocular_method = objSettingData.binocular_method

        _binocular_num_levels = objSettingData.binocular_num_levels

        _binocular_mask_width = objSettingData.binocular_mask_width

        _binocular_mask_height = objSettingData.binocular_mask_height

        _binocular_texture_thresh = objSettingData.binocular_texture_thresh

        _binocular_score_thresh = objSettingData.binocular_score_thresh

        _binocular_filter = objSettingData.binocular_filter

        _binocular_sub_disparity = objSettingData.binocular_sub_disparity

        _point_meshing = objSettingData.point_meshing

        _poisson_depth = objSettingData.poisson_depth

        _poisson_solver_divide = objSettingData.poisson_solver_divide

        _poisson_samples_per_node = objSettingData.poisson_samples_per_node

        _KoujiPath = objSettingData._KoujiPath

        _SelectedImageIDs = objSettingData.SelectedImageIDs

        _OutputPath = objSettingData.OutputPath

        _ImageHenkanTime = objSettingData.ImageHenkanTime

        _ReconstructTime = objSettingData.ReconstructTime

        _OutputTime = objSettingData.OutputTime

        _CreateDate = objSettingData.CreateDate

        m_dbClass = objSettingData.m_dbClass

    End Sub
    Public Function GetLatest()
        Dim IDR As IDataReader = CreateRecordset("SELECT TOP 1 * FROM " + m_strTableName + " ORDER BY ID DESC")
        If Not IDR Is Nothing Then
            Do While IDR.Read
                _ID = IDR.GetInt32(0)

                If Not IDR.IsDBNull(1) Then
                    _image_size_width = IDR.GetInt32(1)
                End If

                If Not IDR.IsDBNull(2) Then
                    _image_size_height = IDR.GetInt32(2)
                End If

                If Not IDR.IsDBNull(3) Then
                    _camera_param = IDR.GetString(3)
                End If

                If Not IDR.IsDBNull(4) Then
                    _bounding_box = IDR.GetDouble(4)
                End If

                If Not IDR.IsDBNull(5) Then
                    _cleanup_min = IDR.GetDouble(5)
                End If

                If Not IDR.IsDBNull(6) Then
                    _persistence = If(IDR.GetInt32(6) = 0, False, True)
                End If

                If Not IDR.IsDBNull(7) Then
                    _rectif_interpolation = IDR.GetString(7)
                End If

                If Not IDR.IsDBNull(8) Then
                    _rectif_sub_sampling = IDR.GetDouble(8)
                End If

                If Not IDR.IsDBNull(9) Then
                    _sub_sampling_step = IDR.GetInt32(9)
                End If

                If Not IDR.IsDBNull(10) Then
                    _disparity_method = IDR.GetString(10)
                End If

                If Not IDR.IsDBNull(11) Then
                    _binocular_method = IDR.GetString(11)
                End If

                If Not IDR.IsDBNull(12) Then
                    _binocular_num_levels = IDR.GetInt32(12)
                End If

                If Not IDR.IsDBNull(13) Then
                    _binocular_mask_width = IDR.GetInt32(13)
                End If

                If Not IDR.IsDBNull(14) Then
                    _binocular_mask_height = IDR.GetInt32(14)
                End If

                If Not IDR.IsDBNull(15) Then
                    _binocular_texture_thresh = IDR.GetDouble(15)
                End If

                If Not IDR.IsDBNull(16) Then
                    _binocular_score_thresh = IDR.GetDouble(16)
                End If

                If Not IDR.IsDBNull(17) Then
                    _binocular_filter = IDR.GetString(17)
                End If

                If Not IDR.IsDBNull(18) Then
                    _binocular_sub_disparity = IDR.GetString(18)
                End If

                If Not IDR.IsDBNull(19) Then
                    _point_meshing = IDR.GetString(19)
                End If

                If Not IDR.IsDBNull(20) Then
                    _poisson_depth = IDR.GetInt32(20)
                End If

                If Not IDR.IsDBNull(21) Then
                    _poisson_solver_divide = IDR.GetInt32(21)
                End If

                If Not IDR.IsDBNull(22) Then
                    _poisson_samples_per_node = IDR.GetInt32(22)
                End If

                If Not IDR.IsDBNull(23) Then
                    _KoujiPath = IDR.GetString(23)
                End If

                If Not IDR.IsDBNull(24) Then
                    _SelectedImageIDs = IDR.GetString(24)
                End If

                If Not IDR.IsDBNull(25) Then
                    _OutputPath = IDR.GetString(25)
                End If

                If Not IDR.IsDBNull(26) Then
                    _ImageHenkanTime = IDR.GetDouble(26)
                End If

                If Not IDR.IsDBNull(27) Then
                    _ReconstructTime = IDR.GetDouble(27)
                End If

                If Not IDR.IsDBNull(28) Then
                    _OutputTime = IDR.GetDouble(28)
                End If

                If Not IDR.IsDBNull(29) Then
                    _CreateDate = IDR.GetDateTime(29)
                End If

            Loop
            IDR.Close()
        End If

        GetLatest = True
    End Function



    Public Function CreateRecordset( _
    Optional ByVal strSQL As String = "" _
    ) As IDataReader

        Dim IDR As IDataReader
        IDR = m_dbClass.DoSelect(strSQL)
        CreateRecordset = IDR
    End Function

    Public Function InsertData(Optional ByRef flg_trans As Boolean = True) As Boolean

        InsertData = True

        Dim strWhere As String = "ID = " & _ID

        'CreateFieldText()
        CreateField()

        Dim lRet As Long = 0

        lRet = m_dbClass.DoInsert(strFieldNames, m_strTableName, strFieldTexts)
        If lRet = 1 Then
        Else
            m_dbClass.RollbackTrans()
            InsertData = False
        End If

    End Function



    Public Sub CreateField()

        Dim IDX As Integer = 0

        If _image_size_width <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "image_size_width"
            strFieldTexts(IDX) = "'" & _image_size_width & "'"
            IDX += 1

        End If
        If _image_size_height <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "image_size_height"
            strFieldTexts(IDX) = "'" & _image_size_height & "'"
            IDX += 1

        End If
        If _camera_param <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "camera_param"
            strFieldTexts(IDX) = "'" & _camera_param & "'"
            IDX += 1

        End If
        If _bounding_box <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "bounding_box"
            strFieldTexts(IDX) = "'" & _bounding_box & "'"
            IDX += 1

        End If
        If _cleanup_min <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "cleanup_min"
            strFieldTexts(IDX) = "'" & _cleanup_min & "'"
            IDX += 1

        End If
        If _persistence = True Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "persistence"
            strFieldTexts(IDX) = "'" & "1" & "'"
            IDX += 1

        End If
        If _rectif_interpolation <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "rectif_interpolation"
            strFieldTexts(IDX) = "'" & _rectif_interpolation & "'"
            IDX += 1

        End If
        If _rectif_sub_sampling <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "rectif_sub_sampling"
            strFieldTexts(IDX) = "'" & _rectif_sub_sampling & "'"
            IDX += 1

        End If
        If _sub_sampling_step <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "sub_sampling_step"
            strFieldTexts(IDX) = "'" & _sub_sampling_step & "'"
            IDX += 1

        End If
        If _disparity_method <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "disparity_method"
            strFieldTexts(IDX) = "'" & _disparity_method & "'"
            IDX += 1

        End If
        If _binocular_method <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "binocular_method"
            strFieldTexts(IDX) = "'" & _binocular_method & "'"
            IDX += 1

        End If
        If _binocular_num_levels <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "binocular_num_levels"
            strFieldTexts(IDX) = "'" & _binocular_num_levels & "'"
            IDX += 1

        End If
        If _binocular_mask_width <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "binocular_mask_width"
            strFieldTexts(IDX) = "'" & _binocular_mask_width & "'"
            IDX += 1

        End If
        If _binocular_mask_height <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "binocular_mask_height"
            strFieldTexts(IDX) = "'" & _binocular_mask_height & "'"
            IDX += 1

        End If
        If _binocular_texture_thresh <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "binocular_texture_thresh"
            strFieldTexts(IDX) = "'" & _binocular_texture_thresh & "'"
            IDX += 1

        End If
        If _binocular_score_thresh <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "binocular_score_thresh"
            strFieldTexts(IDX) = "'" & _binocular_score_thresh & "'"
            IDX += 1

        End If
        If _binocular_filter <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "binocular_filter"
            strFieldTexts(IDX) = "'" & _binocular_filter & "'"
            IDX += 1

        End If
        If _binocular_sub_disparity <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "binocular_sub_disparity"
            strFieldTexts(IDX) = "'" & _binocular_sub_disparity & "'"
            IDX += 1

        End If
        If _point_meshing <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "point_meshing"
            strFieldTexts(IDX) = "'" & _point_meshing & "'"
            IDX += 1

        End If
        If _poisson_depth <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "poisson_depth"
            strFieldTexts(IDX) = "'" & _poisson_depth & "'"
            IDX += 1

        End If
        If _poisson_solver_divide <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "poisson_solver_divide"
            strFieldTexts(IDX) = "'" & _poisson_solver_divide & "'"
            IDX += 1

        End If
        If _poisson_samples_per_node <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "poisson_samples_per_node"
            strFieldTexts(IDX) = "'" & _poisson_samples_per_node & "'"
            IDX += 1

        End If
        If _KoujiPath <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "KoujiPath"
            strFieldTexts(IDX) = "'" & _KoujiPath & "'"
            IDX += 1

        End If
        If _SelectedImageIDs <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "SelectedImageIDs"
            strFieldTexts(IDX) = "'" & _SelectedImageIDs & "'"
            IDX += 1

        End If
        If _OutputPath <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "OutputPath"
            strFieldTexts(IDX) = "'" & _OutputPath & "'"
            IDX += 1

        End If
        If _ImageHenkanTime <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "ImageHenkanTime"
            strFieldTexts(IDX) = "'" & _ImageHenkanTime & "'"
            IDX += 1

        End If
        If _ReconstructTime <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "ReconstructTime"
            strFieldTexts(IDX) = "'" & _ReconstructTime & "'"
            IDX += 1

        End If
        If _OutputTime <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "OutputTime"
            strFieldTexts(IDX) = "'" & _OutputTime & "'"
            IDX += 1

        End If
        If _CreateDate <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "CreateDate"
            strFieldTexts(IDX) = "'" & _CreateDate & "'"
            IDX += 1

        End If


    End Sub

End Class


