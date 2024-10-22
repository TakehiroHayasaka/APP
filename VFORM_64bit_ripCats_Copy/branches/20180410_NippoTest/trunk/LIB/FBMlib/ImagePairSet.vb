﻿Imports HalconDotNet

Public Class ImagePairSet
    ''' <summary>
    ''' Pair(Image1-Image2)のID
    ''' </summary>
    Public Pair_ID As Integer
    ''' <summary>
    ''' Image1自身
    ''' </summary>
    Public IS1 As ImageSet
    ''' <summary>
    ''' Image２自身(Image1とImage2はPair)
    ''' </summary>
    Public IS2 As ImageSet
    ''' <summary>
    ''' Image1のID
    ''' </summary>
    Public IP1 As ImagePoints
    ''' <summary>
    ''' Image2のID
    ''' </summary>
    Public IP2 As ImagePoints

    ''' <summary>
    ''' Image1に映っている共通CTのIDのリスト
    ''' </summary>
    Public lstIS1_ComCT As List(Of CodedTarget)
    ''' <summary>
    ''' Image2に映っている共通CTのIDのリスト
    ''' </summary>
    Public lstIS2_ComCT As List(Of CodedTarget)
    ''' <summary>
    ''' Image1に映っている共通STのIDのリスト（未使用?）
    ''' </summary>
    Public lstIS1_ComST As List(Of SingleTarget)
    ''' <summary>
    ''' Image2に映っている共通STのIDのリスト（未使用?）
    ''' </summary>
    Public lstIS2_ComST As List(Of SingleTarget)

    ''' <summary>
    ''' Image1,2に映っている共通CTのIDのリスト
    ''' </summary>
    Public lstCommonCT_ID As List(Of Integer)
    ''' <summary>
    ''' 2つのImageの共通CT
    ''' </summary>
    ''' Public lstCommonST_ID As List(Of Integer)
    Public cntComCT As Integer
    ''' <summary>
    ''' カメラの内部パラメータ
    ''' </summary>
    ' Public Camparam As Object 20150115 SUURI DEL
    ''' <summary>
    ''' Image1を基準にみたImage2の相対Pose
    ''' </summary>
    Public PairPose As CameraPose
    ''' <summary>
    ''' 基礎行列 (ファンダメンタル行列)
    ''' </summary>
    Public PairFMat As Object
    Public flgChecked As Boolean
    Public ComScale As Object
    Public Hyoka As Double
    ''' <summary>
    ''' エピポーラ線
    ''' </summary>
    Public EpiLine As ImageLine
    Public TaiouTenIndex As List(Of Object)
    Public Sub New()
        cntComCT = 0
        Pair_ID = 0
        If lstCommonCT_ID Is Nothing Then
            lstCommonCT_ID = New List(Of Integer)
        Else
            lstCommonCT_ID.Clear()
        End If

        If lstIS1_ComCT Is Nothing Then
            lstIS1_ComCT = New List(Of CodedTarget)
        Else
            lstIS1_ComCT.Clear()
        End If

        If lstIS2_ComCT Is Nothing Then
            lstIS2_ComCT = New List(Of CodedTarget)
        Else
            lstIS2_ComCT.Clear()
        End If

        'If lstCommonST_ID Is Nothing Then
        '    lstCommonST_ID = New List(Of Integer)
        'Else
        '    lstCommonST_ID.Clear()
        'End If

        If lstIS1_ComST Is Nothing Then
            lstIS1_ComST = New List(Of SingleTarget)
        Else
            lstIS1_ComST.Clear()
        End If

        If lstIS2_ComST Is Nothing Then
            lstIS2_ComST = New List(Of SingleTarget)
        Else
            lstIS2_ComST.Clear()
        End If
        If TaiouTenIndex Is Nothing Then
            TaiouTenIndex = New List(Of Object)
        Else
            TaiouTenIndex.Clear()
        End If
        '  Camparam = Nothing '20150115 SUURI DEL
        ComScale = Nothing
        PairFMat = Nothing
        flgChecked = False
        IP1 = New ImagePoints
        IP2 = New ImagePoints
    End Sub
    Public Sub New(ByRef _IS1 As ImageSet, ByRef _IS2 As ImageSet)
        IS1 = _IS1
        IS2 = _IS2
        cntComCT = 0
        Pair_ID = 0
        If lstCommonCT_ID Is Nothing Then
            lstCommonCT_ID = New List(Of Integer)
        Else
            lstCommonCT_ID.Clear()
        End If

        If lstIS1_ComCT Is Nothing Then
            lstIS1_ComCT = New List(Of CodedTarget)
        Else
            lstIS1_ComCT.Clear()
        End If

        If lstIS2_ComCT Is Nothing Then
            lstIS2_ComCT = New List(Of CodedTarget)
        Else
            lstIS2_ComCT.Clear()
        End If

        'If lstCommonST_ID Is Nothing Then
        '    lstCommonST_ID = New List(Of Integer)
        'Else
        '    lstCommonST_ID.Clear()
        'End If

        If lstIS1_ComST Is Nothing Then
            lstIS1_ComST = New List(Of SingleTarget)
        Else
            lstIS1_ComST.Clear()
        End If

        If lstIS2_ComST Is Nothing Then
            lstIS2_ComST = New List(Of SingleTarget)
        Else
            lstIS2_ComST.Clear()
        End If
        If TaiouTenIndex Is Nothing Then
            TaiouTenIndex = New List(Of Object)
        Else
            TaiouTenIndex.Clear()
        End If

        ' Camparam = Nothing '20150115 SUURI DEL
        ComScale = Nothing
        PairFMat = Nothing
        PairPose = New CameraPose
        EpiLine = New ImageLine
        flgChecked = False
        IP1 = New ImagePoints
        IP2 = New ImagePoints
        GetCommonCT()
    End Sub

    Public ReadOnly Property IS1_ID() As Integer
        Get
            Return IS1.ImageId
        End Get
    End Property

    Public ReadOnly Property IS2_ID() As Integer
        Get
            Return IS2.ImageId
        End Get
    End Property

    Public ReadOnly Property RelPose(Optional ByVal blnPairRelPose As Boolean = False) As HTuple
        Get
            RelPose = New HTuple
            If IS1.ImageId <> 1 Then
                Debug.Print("")
            End If
            If blnPairRelPose = True Then
                RelPose = PairPose.RelPose
            Else
                If IS1.flgConnected = True And IS2.flgConnected = True Then
                    CalcRelPoseBetweenTwoPose(IS1.ImagePose.Pose, IS2.ImagePose.Pose, RelPose)
                Else
                    RelPose = PairPose.RelPose
                End If
            End If

        End Get
    End Property
    Public ReadOnly Property PairIsConnected() As Boolean
        Get
            If IS1.flgConnected = True And IS2.flgConnected = True Then
                Return True
            Else
                Return False
            End If
        End Get

    End Property
    Private Sub GetCommonCT()
        cntComCT = 0

        For Each objCT1 As CodedTarget In IS1.Targets.lstCT
            For Each objCT2 As CodedTarget In IS2.Targets.lstCT
                If objCT1.CT_ID = objCT2.CT_ID Then
                    lstCommonCT_ID.Add(objCT1.CT_ID)
                    lstIS1_ComCT.Add(objCT1)
                    lstIS2_ComCT.Add(objCT2)
                    cntComCT += 1
                    Exit For
                End If
            Next
        Next

    End Sub

    Public Function GetCommonST() As Boolean
        If Not lstIS1_ComST Is Nothing Then
            If lstIS1_ComST.Count > 0 Then
                GetCommonST = False
                Exit Function
            End If
        End If
        For Each ST1 As SingleTarget In IS1.Targets.lstST
            If ST1.P3ID = -1 Then
                Continue For
            End If
            For Each ST2 As SingleTarget In IS2.Targets.lstST
                If ST2.P3ID = -1 Then
                    Continue For
                End If
                If ST1.P3ID = ST2.P3ID Then
                    'lstCommonST_ID.Add(ST1.P3ID)
                    lstIS1_ComST.Add(ST1)
                    lstIS2_ComST.Add(ST2)
                    Exit For
                End If
            Next
        Next
        GetCommonST = True
    End Function
    Private Function GetImagePoints_from_CTList(ByRef lstCT As List(Of CodedTarget)) As ImagePoints
        GetImagePoints_from_CTList = New ImagePoints
        Dim N As Integer = lstCT.Count * CodedTarget.CTnoSTnum 'CT内のSTの合計
        Dim Row As New HTuple
        Dim Col As New HTuple
        Dim i As Integer = 0
        Dim cnt As Integer = 0
        If lstCT Is Nothing Then
            Exit Function
        Else
            If lstCT.Count = 0 Then
                Exit Function
            End If
        End If
        For Each CT As CodedTarget In lstCT 'CT内のSTの2次元座標値を格納
            For i = 0 To CodedTarget.CTnoSTnum - 1
                Row(cnt) = CT.CT_Points.Row(i)
                Col(cnt) = CT.CT_Points.Col(i)
                cnt += 1
            Next
        Next
        GetImagePoints_from_CTList.Row = Row 'CT内のSTの2次元座標値（縦方向?）
        GetImagePoints_from_CTList.Col = Col 'CT内のSTの2次元座標値（横方向?）

    End Function
    Private Function GetImagePoints_from_STList(ByRef lstST As List(Of SingleTarget)) As ImagePoints
        Dim N As Integer = lstST.Count
        Dim Row(N - 1) As Object
        Dim Col(N - 1) As Object
        Dim cnt As Integer = 0
        GetImagePoints_from_STList = New ImagePoints
        If lstST Is Nothing Then
            Exit Function
        Else
            If lstST.Count = 0 Then
                Exit Function
            End If
        End If
        For Each ST As SingleTarget In lstST
            Row(cnt) = ST.P2D.Row
            Col(cnt) = ST.P2D.Col
            cnt += 1
        Next
    End Function
    ''' <summary>
    ''' 相対Poseを算出
    ''' </summary>
    Public Sub calcRelPose()


        IP1 = GetImagePoints_from_CTList(lstIS1_ComCT)
        IP2 = GetImagePoints_from_CTList(lstIS2_ComCT)
        If Not lstIS1_ComST Is Nothing Then
            If lstIS1_ComST.Count <> 0 Then
                Dim ST_IP1 As ImagePoints = GetImagePoints_from_STList(lstIS1_ComST)
                Dim ST_IP2 As ImagePoints = GetImagePoints_from_STList(lstIS2_ComST)
                IP1.ConcatToMe(ST_IP1)
                IP2.ConcatToMe(ST_IP2)
            End If
        End If
        PairPose.hError = Nothing
        Try
            'VectorToRelPose ：画像の点対応で与えられる 2 つのカメラと既知のカメラパラメーター間の相対位置を計算し、3 次元空間点を再構成する。
            '20150115 SUURI Rep Sta 複数カメラ内部パラメータの取扱機能------------------------------
            'HOperatorSet.VectorToRelPose(IP1.Row, IP1.Col, IP2.Row, IP2.Col, _
            '              Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Camparam, Camparam, "gold_standard", _
            '              PairPose.RelPose, PairPose.CovRelPose, PairPose.hError, _
            '              PairPose.X, PairPose.Y, PairPose.Z, PairPose.CovXYZ) '質問
            '
            HOperatorSet.VectorToRelPose(IP1.Row, IP1.Col, IP2.Row, IP2.Col, _
                          New HTuple, New HTuple, New HTuple, New HTuple, New HTuple, New HTuple, IS1.objCamparam.Camparam, IS2.objCamparam.Camparam, "gold_standard", _
                          PairPose.RelPose, PairPose.CovRelPose, PairPose.hError, _
                          PairPose.X, PairPose.Y, PairPose.Z, PairPose.CovXYZ)
            '20150115 SUURI Rep End 複数カメラ内部パラメータの取扱機能------------------------------
        Catch ex As Exception

            ''            ADD By Yamada 20150113 Sta --------------------------------------------------------------------------------------------- 
            '#If DEBUG Then
            '            MsgBox("画像" & IS1.ImageName & "と画像" & IS2.ImageName & "のペアから、カメラの相対位置を算出できませんでした。", MsgBoxStyle.Exclamation & MsgBoxStyle.OkOnly)
            '#End If
            ''            ADD By Yamada 20150113 END --------------------------------------------------------------------------------------------- 

            cntComCT = -1
            Hyoka = 0
            Exit Sub
        End Try

        If BTuple.TupleLength(PairPose.hError).I = 1 Then
            If BTuple.TupleMean(BTuple.TupleAbs(PairPose.Z)) > 20 Then
                cntComCT = -1
                Hyoka = 0
                Exit Sub
            End If
            If PairPose.hError > 1 Then
                Dim t As Integer = 1
                cntComCT = -1
                Hyoka = 0
            Else
                'RelPoseToFundamentalMatrix ：2 つのカメラの相対方向から基礎行列を計算する。

                '20150115 SUURI Rep Sta 複数カメラ内部パラメータの取扱機能------------------------------
                ' HOperatorSet.RelPoseToFundamentalMatrix(PairPose.RelPose, PairPose.CovRelPose, Camparam, Camparam, PairFMat, Nothing) '質問
                HOperatorSet.RelPoseToFundamentalMatrix(PairPose.RelPose, PairPose.CovRelPose,
                                              IS1.objCamparam.Camparam, IS2.objCamparam.Camparam, PairFMat, New HTuple) '質問
                '20150115 SUURI Rep End 複数カメラ内部パラメータの取扱機能------------------------------

                Hyoka = (BTuple.TupleLength(IP1.Row) * (ImageCNT / PairPose.hError) * (ImageCNT / BTuple.TupleMean(PairPose.Z))).D '質問
                'Dim tmpPose As Object = Nothing
                'Dim tmpQuality As Object = Nothing
                'Dim tmphommat As Object = Nothing
                'Dim tmpDiff As Object = Nothing


                'HOperatorSet.VectorToPose(PairPose.X, PairPose.Y, PairPose.Z, IP2.Row, IP2.Col, Camparam, "iterative", "error", tmpPose, tmpQuality)
                'HOperatorSet.PoseToHomMat3d(tmpPose, tmphommat)
                'HOperatorSet.HomMat3dInvert(tmphommat, tmphommat)
                'HOperatorSet.HomMat3dToPose(tmphommat, tmpPose)
                'tmpDiff = BTuple.TupleSub(PairPose.RelPose, tmpPose)
                ''Trace.WriteLine(IS1.ImageId & "と" & IS2.ImageId & "で" & PairPose.RelPose(0) & "," & PairPose.RelPose(1) & "," & PairPose.RelPose(2) & "," & PairPose.RelPose(3) & "," & PairPose.RelPose(4) & "," & PairPose.RelPose(5) & vbNewLine & _
                ''                tmpPose(0) & "," & tmpPose(1) & "," & tmpPose(2) & "," & tmpPose(3) & "," & tmpPose(4) & "," & tmpPose(5) & vbNewLine & _
                ''                tmpDiff(0) & "," & tmpDiff(1) & "," & tmpDiff(2) & "," & tmpDiff(3) & "," & tmpDiff(4) & "," & tmpDiff(5))
                'Trace.WriteLine(IS1.ImageId & "と" & IS2.ImageId & "でQuality:" & tmpQuality)
                'Trace.WriteLine(PairPose.RelPose(0) & "," & tmpPose(0) & "," & tmpDiff(0))
                'Trace.WriteLine(PairPose.RelPose(1) & "," & tmpPose(1) & "," & tmpDiff(1))
                'Trace.WriteLine(PairPose.RelPose(2) & "," & tmpPose(2) & "," & tmpDiff(2))
                'Trace.WriteLine(PairPose.RelPose(3) & "," & tmpPose(3) & "," & tmpDiff(3))
                'Trace.WriteLine(PairPose.RelPose(4) & "," & tmpPose(4) & "," & tmpDiff(4))
                'Trace.WriteLine(PairPose.RelPose(5) & "," & tmpPose(5) & "," & tmpDiff(5))
                'If BTuple.TupleDeviation(tmpDiff) > 1 Then
                '    Stop
                'End If
            End If
        Else

            'Dim Dist1 As Object = Nothing
            'Dim Dist2 As Object = Nothing
            'Dim tmp3D1 As New Point3D
            'Dim tmp3D2 As New Point3D
            'Try
            '    HOperatorSet.IntersectLinesOfSight(Camparam, Camparam, BTuple.TupleFirstN(PairPose.RelPose, 6), IP1.Row, IP1.Col, IP2.Row, IP2.Col, tmp3D1.X, tmp3D1.Y, tmp3D1.Z, Dist1)

            'Catch ex As Exception
            '    tmp3D1.Z = -1
            'End Try
            'Try
            '    HOperatorSet.IntersectLinesOfSight(Camparam, Camparam, BTuple.TupleLastN(PairPose.RelPose, 7), IP1.Row, IP1.Col, IP2.Row, IP2.Col, tmp3D2.X, tmp3D2.Y, tmp3D2.Z, Dist2)

            'Catch ex As Exception
            '    tmp3D2.Z = -1
            'End Try

            'If BTuple.TupleMean(tmp3D1.Z) > BTuple.TupleMean(tmp3D2.Z) Then
            '    If BTuple.TupleMean(tmp3D1.Z) = -1 Then
            '        cntComCT = -1
            '        Hyoka = 0
            '        Exit Sub
            '    End If
            '    PairPose.RelPose = BTuple.TupleFirstN(PairPose.RelPose, 6)
            '    PairPose.hError = PairPose.hError(0)
            '    PairPose.X = BTuple.TupleFirstN(PairPose.X, cntComCT * CodedTarget.CTnoSTnum - 1)
            '    PairPose.Y = BTuple.TupleFirstN(PairPose.Y, cntComCT * CodedTarget.CTnoSTnum - 1)
            '    PairPose.Z = BTuple.TupleFirstN(PairPose.Z, cntComCT * CodedTarget.CTnoSTnum - 1)
            'Else
            '    If BTuple.TupleMean(tmp3D1.Z) = -1 Then
            '        cntComCT = -1
            '        Hyoka = 0
            '        Exit Sub
            '    End If
            '    PairPose.RelPose = BTuple.TupleLastN(PairPose.RelPose, 7)
            '    PairPose.hError = PairPose.hError(1)
            '    PairPose.X = BTuple.TupleLastN(PairPose.X, cntComCT * CodedTarget.CTnoSTnum)
            '    PairPose.Y = BTuple.TupleLastN(PairPose.Y, cntComCT * CodedTarget.CTnoSTnum)
            '    PairPose.Z = BTuple.TupleLastN(PairPose.Z, cntComCT * CodedTarget.CTnoSTnum)

            'End If
            'If BTuple.TupleMean(PairPose.Z) > 20 Then
            '    cntComCT = -1
            '    Hyoka = 0
            '    Exit Sub
            'End If
            'If PairPose.hError > 1 Then
            '    Dim t As Integer = 1
            '    cntComCT = -1
            '    Hyoka = 0
            'Else
            '    HOperatorSet.RelPoseToFundamentalMatrix(PairPose.RelPose, Nothing, Camparam, Camparam, PairFMat, Nothing)
            '    Hyoka = BTuple.TupleLength(IP1.Row) * (ImageCNT / PairPose.hError) * (ImageCNT / BTuple.TupleMean(PairPose.Z))
            'End If
            cntComCT = -1
            Hyoka = 0

            If False Then
                BTuple.WriteTuple(IP1.Row, My.Application.Info.DirectoryPath & "\R1.tpl")
                BTuple.WriteTuple(IP1.Col, My.Application.Info.DirectoryPath & "\C1.tpl")
                BTuple.WriteTuple(IP2.Row, My.Application.Info.DirectoryPath & "\R2.tpl")
                BTuple.WriteTuple(IP2.Col, My.Application.Info.DirectoryPath & "\C2.tpl")
                BTuple.WriteTuple(IS1.ImageFullPath, My.Application.Info.DirectoryPath & "\I1.tpl")
                BTuple.WriteTuple(IS2.ImageFullPath, My.Application.Info.DirectoryPath & "\I2.tpl")
                'BTuple.WriteTuple(Camparam, My.Application.Info.DirectoryPath & "\Cam.tpl") '20150115 SUURI DEL
            End If
#If DEBUG Then
            Dim strText As String
            strText = ""
            strText = IS1.ImageId & "," & IS1.ImageName & "to" & IS2.ImageId & "," & IS2.ImageName & "が決まらない" & vbNewLine
            My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\RelPoseMonitor.txt", strText, True)
#End If

        End If
    End Sub

    Public Function GetCommonCTcount(ByRef IPS As ImagePairSet) As Integer
        GetCommonCTcount = 0
        For Each ct_id As Integer In IPS.lstCommonCT_ID
            For Each my_ct_id As Integer In lstCommonCT_ID
                If ct_id = my_ct_id Then
                    GetCommonCTcount += 1
                    Exit For
                End If
            Next
        Next
    End Function

    Public Function GetCommonCT3dPoint_of_2Pair(ByRef OtherIPS As ImagePairSet, ByRef MyCT_3dPoint As Point3D, ByRef OtherCT_3dPoint As Point3D) As Boolean
        Dim MyIP1 As New ImagePoints
        Dim MyIP2 As New ImagePoints
        Dim OtherIP1 As New ImagePoints
        Dim OtherIP2 As New ImagePoints
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim cmnCTnum As Integer = 0
        GetCommonCT3dPoint_of_2Pair = False
        For Each myCT1 As CodedTarget In lstIS1_ComCT
            j = 0
            For Each otherCT2 As CodedTarget In OtherIPS.lstIS1_ComCT
                If myCT1.CT_ID = otherCT2.CT_ID Then
                    MyIP1.ConcatToMe(myCT1.CT_Points)
                    MyIP2.ConcatToMe(lstIS2_ComCT.Item(i).CT_Points)
                    OtherIP1.ConcatToMe(otherCT2.CT_Points)
                    OtherIP2.ConcatToMe(OtherIPS.lstIS2_ComCT.Item(j).CT_Points)
                    cmnCTnum += 1
                    Exit For
                End If
                j += 1
            Next
            i += 1
        Next
        If cmnCTnum < intCommonCTnum Then
            Exit Function
        End If
        i = 0
        j = 0
        For Each myST1 As SingleTarget In lstIS1_ComST
            j = 0
            For Each otherST2 As SingleTarget In OtherIPS.lstIS1_ComST
                If myST1.P3ID = otherST2.P3ID Then
                    MyIP1.ConcatToMe(myST1.P2D)
                    MyIP2.ConcatToMe(lstIS2_ComST.Item(i).P2D)
                    OtherIP1.ConcatToMe(otherST2.P2D)
                    OtherIP2.ConcatToMe(OtherIPS.lstIS2_ComST.Item(j).P2D)
                    Exit For
                End If
                j += 1
            Next
            i += 1
        Next

        Try

            '20150115 SUURI Rep Sta 複数カメラ内部パラメータの取扱機能------------------------------
            'HOperatorSet.IntersectLinesOfSight(Camparam, Camparam, RelPose(True), MyIP1.Row, MyIP1.Col, MyIP2.Row, MyIP2.Col, _
            '                   MyCT_3dPoint.X, MyCT_3dPoint.Y, MyCT_3dPoint.Z, MyCT_3dPoint.MyDist)
            'HOperatorSet.IntersectLinesOfSight(OtherIPS.Camparam, OtherIPS.Camparam, OtherIPS.RelPose(True), OtherIP1.Row, OtherIP1.Col, OtherIP2.Row, OtherIP2.Col, _
            '                         OtherCT_3dPoint.X, OtherCT_3dPoint.Y, OtherCT_3dPoint.Z, OtherCT_3dPoint.MyDist)
            HOperatorSet.IntersectLinesOfSight(IS1.objCamparam.Camparam, IS2.objCamparam.Camparam, RelPose(True), MyIP1.Row, MyIP1.Col, MyIP2.Row, MyIP2.Col, _
                              MyCT_3dPoint.X, MyCT_3dPoint.Y, MyCT_3dPoint.Z, MyCT_3dPoint.MyDist)
            HOperatorSet.IntersectLinesOfSight(OtherIPS.IS1.objCamparam.Camparam, OtherIPS.IS2.objCamparam.Camparam, OtherIPS.RelPose(True), OtherIP1.Row, OtherIP1.Col, OtherIP2.Row, OtherIP2.Col, _
                                     OtherCT_3dPoint.X, OtherCT_3dPoint.Y, OtherCT_3dPoint.Z, OtherCT_3dPoint.MyDist)
            '20150115 SUURI Rep End 複数カメラ内部パラメータの取扱機能------------------------------

        Catch ex As Exception
            Exit Function
        End Try
        GetCommonCT3dPoint_of_2Pair = True

    End Function

    Public Sub CreateCommon3DCT(ByRef lstCommon3dCT As List(Of Common3DCodedTarget))
        Dim MyIP1 As New ImagePoints
        Dim MyIP2 As New ImagePoints
        Dim OtherIP1 As New ImagePoints
        Dim OtherIP2 As New ImagePoints
        Dim MyCT_3dPoint As New Point3D
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim cmnCTnum As Integer = 0

        For Each myCT1 As CodedTarget In lstIS1_ComCT

            MyIP1 = myCT1.CT_Points
            MyIP2 = lstIS2_ComCT.Item(i).CT_Points

            Try
                '20150115 SUURI Rep Sta 複数カメラ内部パラメータの取扱機能------------------------------
                'HOperatorSet.IntersectLinesOfSight(Camparam, Camparam, RelPose(True), MyIP1.Row, MyIP1.Col, MyIP2.Row, MyIP2.Col, _
                '                   MyCT_3dPoint.X, MyCT_3dPoint.Y, MyCT_3dPoint.Z, MyCT_3dPoint.MyDist)
                HOperatorSet.IntersectLinesOfSight(IS1.objCamparam.Camparam, IS2.objCamparam.Camparam, RelPose(True), MyIP1.Row, MyIP1.Col, MyIP2.Row, MyIP2.Col, _
                                  MyCT_3dPoint.X, MyCT_3dPoint.Y, MyCT_3dPoint.Z, MyCT_3dPoint.MyDist)
                '20150115 SUURI Rep End 複数カメラ内部パラメータの取扱機能------------------------------

            Catch ex As Exception
                Exit Sub
            End Try
            Dim C3DCT As New Common3DCodedTarget
            C3DCT.PID = myCT1.CT_ID
            C3DCT.lstCT.Add(myCT1)
            C3DCT.lstCT.Add(lstIS2_ComCT.Item(i))
            For j = 0 To CodedTarget.CTnoSTnum - 1
                Dim XYZ As New Point3D
                XYZ.X = BTuple.TupleSelect(MyCT_3dPoint.X, j)
                XYZ.Y = BTuple.TupleSelect(MyCT_3dPoint.Y, j)
                XYZ.Z = BTuple.TupleSelect(MyCT_3dPoint.Z, j)

                C3DCT.lstP3d.Add(XYZ)
            Next
            lstCommon3dCT.Add(C3DCT)
            i += 1
        Next

    End Sub

    Public Function CalcPoseByCommonCT3dPoint(ByRef OtherIPS As ImagePairSet, ByRef lstCommon3dCT As List(Of Common3DCodedTarget), _
                                               ByRef Pose As Object, ByRef Quality As Object) As Boolean
        Dim MyCT_3dPoint As New Point3D
        Dim MyIP1 As New ImagePoints
        Dim OtherIP2 As New ImagePoints
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim t As Integer = 0
        Dim cmnCTnum As Integer = 0
        CalcPoseByCommonCT3dPoint = False
        For Each myCT1 As CodedTarget In lstIS1_ComCT
            j = 0
            For Each otherCT2 As CodedTarget In OtherIPS.lstIS1_ComCT
                If myCT1.CT_ID = otherCT2.CT_ID Then
                    For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
                        If myCT1.CT_ID = C3DCT.PID Then
                            For t = 0 To CodedTarget.CTnoSTnum - 1
                                MyCT_3dPoint.ConcatToMe(C3DCT.lstP3d.Item(t))
                            Next

                            OtherIP2.ConcatToMe(OtherIPS.lstIS2_ComCT.Item(j).CT_Points)
                        End If
                    Next
                    cmnCTnum += 1
                    Exit For
                End If
                j += 1
            Next
            i += 1
        Next

        If cmnCTnum < intCommonCTnum Then
            Exit Function
        End If

        Dim hv_PoseOut As New Object

        '20150115 SUURI Rep Sta 複数カメラ内部パラメータの取扱機能------------------------------
        'HOperatorSet.VectorToPose(MyCT_3dPoint.X, MyCT_3dPoint.Y, MyCT_3dPoint.Z, OtherIP2.Row, OtherIP2.Col, _
        '  Camparam, "iterative", "error", Pose, Quality)
        HOperatorSet.VectorToPose(MyCT_3dPoint.X, MyCT_3dPoint.Y, MyCT_3dPoint.Z, OtherIP2.Row, OtherIP2.Col, _
         IS2.objCamparam.Camparam, "iterative", "error", Pose, Quality)
        '20150115 SUURI Rep End 複数カメラ内部パラメータの取扱機能------------------------------

        CalcPoseByCommonCT3dPoint = True

    End Function

    Public Function CalcPoseByCommonCTandBestScale(ByRef lstCommon3dCT As List(Of Common3DCodedTarget), _
                                                    ByRef Pose As Object, ByRef Quality As Object, _
                                                    ByRef ComScale As Object, ByVal ScaleMM As Double) As Boolean
        Dim MyCT1_IP As New ImagePoints
        Dim MyCT2_IP As New ImagePoints
        Dim CommonCT3d As New Point3D
        Dim i As Integer = 0
        Dim cnttmpComCT As Integer = 0
        Dim t As Integer
        Dim N As Integer = IIf(lstIS1_ComCT.Count > lstCommon3dCT.Count, lstIS1_ComCT.Count, lstCommon3dCT.Count) * CodedTarget.CTnoSTnum - 1
        'Dim X(N) As Object
        'Dim Y(N) As Object
        'Dim Z(N) As Object
        Dim X As New HTuple
        Dim Y As New HTuple
        Dim Z As New HTuple
        'Dim Row1(N) As Object
        'Dim Col1(N) As Object
        'Dim Row2(N) As Object
        'Dim Col2(N) As Object
        Dim Row1 As New HTuple
        Dim Col1 As New HTuple
        Dim Row2 As New HTuple
        Dim Col2 As New HTuple
        Dim cnt As Integer = 0
        Dim HomMatInvert As Object = Nothing

        CalcPoseByCommonCTandBestScale = False
        For Each MyCT1 As CodedTarget In lstIS1_ComCT
            If MyCT1.myC3DCT.flgUsable = True Then
                For t = 0 To CodedTarget.CTnoSTnum - 1
                    X(cnt) = MyCT1.myC3DCT.lstP3d.Item(t).X
                    Y(cnt) = MyCT1.myC3DCT.lstP3d.Item(t).Y
                    Z(cnt) = MyCT1.myC3DCT.lstP3d.Item(t).Z
                    Row1(cnt) = MyCT1.CT_Points.Row(t)
                    Col1(cnt) = MyCT1.CT_Points.Col(t)
                    Row2(cnt) = lstIS2_ComCT.Item(i).CT_Points.Row(t)
                    Col2(cnt) = lstIS2_ComCT.Item(i).CT_Points.Col(t)
                    cnt += 1
                Next
                cnttmpComCT += 1
            End If

            'For Each C3DCT As Common3DCodedTarget In lstCommon3dCT
            '    If C3DCT.flgUsable = True Then
            '        'If C3DCT.PID = 223 Then
            '        '    C3DCT.PID = 223
            '        'End If
            '        If MyCT1.CT_ID = C3DCT.PID Then
            '            For t = 0 To CodedTarget.CTnoSTnum - 1
            '                'CommonCT3d.ConcatToMe(C3DCT.lstP3d.Item(t))
            '                X(cnt) = C3DCT.lstP3d.Item(t).X
            '                Y(cnt) = C3DCT.lstP3d.Item(t).Y
            '                Z(cnt) = C3DCT.lstP3d.Item(t).Z
            '                Row1(cnt) = MyCT1.CT_Points.Row(t)
            '                Col1(cnt) = MyCT1.CT_Points.Col(t)
            '                Row2(cnt) = lstIS2_ComCT.Item(i).CT_Points.Row(t)
            '                Col2(cnt) = lstIS2_ComCT.Item(i).CT_Points.Col(t)
            '                cnt += 1
            '            Next
            '            ' MyCT1_IP.ConcatToMe(MyCT1.CT_Points)
            '            ' MyCT2_IP.ConcatToMe(lstIS2_ComCT.Item(i).CT_Points)
            '            cnttmpComCT += 1
            '        End If
            '    End If
            'Next
            i = i + 1
        Next
        cnt -= 1
        If cnt <= 0 Then
            Exit Function
        End If
        HOperatorSet.TupleSelectRange(X, 0, cnt, X)
        HOperatorSet.TupleSelectRange(Y, 0, cnt, Y)
        HOperatorSet.TupleSelectRange(Z, 0, cnt, Z)
        HOperatorSet.TupleSelectRange(Row1, 0, cnt, Row1)
        HOperatorSet.TupleSelectRange(Col1, 0, cnt, Col1)
        HOperatorSet.TupleSelectRange(Row2, 0, cnt, Row2)
        HOperatorSet.TupleSelectRange(Col2, 0, cnt, Col2)
        'ReDim Preserve Y(cnt)
        'ReDim Preserve Z(cnt)

        CommonCT3d.X = X
        CommonCT3d.Y = Y
        CommonCT3d.Z = Z
        MyCT1_IP.Row = Row1
        MyCT1_IP.Col = Col1
        MyCT2_IP.Row = Row2
        MyCT2_IP.Col = Col2

        If cnttmpComCT < intCommonCTnum Then
            Exit Function
        End If
        Dim HomMat As Object = Nothing
        PairPose.hError = Nothing

        '20150115 SUURI Rep Sta 複数カメラ内部パラメータの取扱機能------------------------------
        'HOperatorSet.VectorToRelPose(MyCT2_IP.Row, MyCT2_IP.Col, MyCT1_IP.Row, MyCT1_IP.Col, _
        '                  Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Camparam, Camparam, "gold_standard", _
        '                  PairPose.RelPose, PairPose.CovRelPose, PairPose.hError, _
        '                  PairPose.X, PairPose.Y, PairPose.Z, PairPose.CovXYZ)
        Try
            HOperatorSet.VectorToRelPose(MyCT2_IP.Row, MyCT2_IP.Col, MyCT1_IP.Row, MyCT1_IP.Col, _
                       New HTuple, New HTuple, New HTuple, New HTuple, New HTuple, New HTuple, IS2.objCamparam.Camparam, IS1.objCamparam.Camparam, "gold_standard", _
                       PairPose.RelPose, PairPose.CovRelPose, PairPose.hError, _
                       PairPose.X, PairPose.Y, PairPose.Z, PairPose.CovXYZ)
        Catch ex As Exception
            cnttmpComCT = -1
            Exit Function
        End Try

        '20150115 SUURI Rep End 複数カメラ内部パラメータの取扱機能------------------------------

        If BTuple.TupleLength(PairPose.hError).I = 1 Then
            If BTuple.TupleDeviation(PairPose.Z) > 50 Or BTuple.TupleMean(PairPose.Z) > 50 Then
                cnttmpComCT = -1
                Exit Function
            End If
            If PairPose.hError > 1 Then
                Dim tt As Integer = 1
                cnttmpComCT = -1
            End If
        Else
            'Dim tmp3D As New Point3D

            'HOperatorSet.PoseToHomMat3d(IS1.ImagePose.Pose, HomMat)
            'HOperatorSet.HomMat3dInvert(HomMat, HomMat)
            'HOperatorSet.AffineTransPoint3D(HomMat, CommonCT3d.X, CommonCT3d.Y, CommonCT3d.Z, tmp3D.X, tmp3D.Y, tmp3D.Z)
            'HOperatorSet.VectorToPose(tmp3D.X, tmp3D.Y, tmp3D.Z, MyCT2_IP.Row, MyCT2_IP.Col, Camparam, "iterative", "error", PairPose.RelPose, Quality)
            'HOperatorSet.PoseToHomMat3d(PairPose.RelPose, HomMat)
            'HOperatorSet.HomMat3dInvert(HomMat, HomMat)
            'HOperatorSet.HomMat3dToPose(HomMat, PairPose.RelPose)
            'CalcUnitPose(PairPose.RelPose, Pose)


            'HOperatorSet.VectorToPose(CommonCT3d.X, CommonCT3d.Y, CommonCT3d.Z, MyCT2_IP.Row, MyCT2_IP.Col, Camparam, "iterative", "error", PairPose.RelPose, Quality)
            'HOperatorSet.PoseToHomMat3d(PairPose.RelPose, HomMatInvert)
            'HOperatorSet.HomMat3dInvert(HomMatInvert, HomMatInvert)
            'HOperatorSet.HomMat3dToPose(HomMatInvert, Pose)
            'GoTo linejump

            ' HOperatorSet.IntersectLinesOfSight(Camparam, Camparam, PairPose.RelPose, MyCT2_IP.Row, MyCT2_IP.Col, MyCT1_IP.Row, MyCT1_IP.Col, PairPose.X, PairPose.Y, PairPose.Z, Quality)

            'If PairPose.hError(0) < PairPose.hError(1) Then

            '    PairPose.RelPose = BTuple.TupleFirstN(PairPose.RelPose, 6)
            '    PairPose.hError = PairPose.hError(0)
            '    PairPose.X = BTuple.TupleFirstN(PairPose.X, cnttmpComCT * CodedTarget.CTnoSTnum - 1)
            '    PairPose.Y = BTuple.TupleFirstN(PairPose.Y, cnttmpComCT * CodedTarget.CTnoSTnum - 1)
            '    PairPose.Z = BTuple.TupleFirstN(PairPose.Z, cnttmpComCT * CodedTarget.CTnoSTnum - 1)
            'Else
            '    PairPose.RelPose = BTuple.TupleLastN(PairPose.RelPose, 7)
            '    PairPose.hError = PairPose.hError(1)
            '    PairPose.X = BTuple.TupleLastN(PairPose.X, cnttmpComCT * CodedTarget.CTnoSTnum)
            '    PairPose.Y = BTuple.TupleLastN(PairPose.Y, cnttmpComCT * CodedTarget.CTnoSTnum)
            '    PairPose.Z = BTuple.TupleLastN(PairPose.Z, cnttmpComCT * CodedTarget.CTnoSTnum)

            'End If
            'If BTuple.TupleDeviation(PairPose.Z) > 20 Or BTuple.TupleMean(PairPose.Z) > 20 Then
            '    cnttmpComCT = -1
            '    Exit Function
            'End If
            'If PairPose.hError > 0.5 Then
            '    Dim tt As Integer = 1
            '    cnttmpComCT = -1
            'End If

            cnttmpComCT = -1
        End If

        If cnttmpComCT < intCommonCTnum Then
            Exit Function
        End If

        Dim MyCT_3d As New Point3D(PairPose.X, PairPose.Y, PairPose.Z)
        'Dim MyCT_3dT As New Point3D
        Dim Scale As Object = Nothing


        Dim Q1 As Object = Nothing
        Dim Q2 As Object = Nothing
        Dim Q12 As Object = Nothing
        'HOperatorSet.VectorToHomMat3d("similarity", MyCT_3d.X, MyCT_3d.Y, MyCT_3d.Z, CommonCT3d.X, CommonCT3d.Y, CommonCT3d.Z, HomMat)
        'HOperatorSet.AffineTransPoint3D(HomMat, MyCT_3d.X, MyCT_3d.Y, MyCT_3d.Z, MyCT_3dT.X, MyCT_3dT.Y, MyCT_3dT.Z)
        'MyCT_3dT.GetDisttoOtherPose(CommonCT3d, Quality)
        'Q1 = BTuple.TupleMean(Quality)
        'HOperatorSet.HomMat3dToPose(HomMat, Q2)

        MyCT_3d.CalcScale(CommonCT3d, ComScale)
        MyCT_3d.SetScale(ComScale.D)
        hom_mat_3d_from_3d_3d_point_correspondence(MyCT_3d.X, MyCT_3d.Y, MyCT_3d.Z, CommonCT3d.X, CommonCT3d.Y, CommonCT3d.Z, HomMat)
        HOperatorSet.AffineTransPoint3d(HomMat, MyCT_3d.X, MyCT_3d.Y, MyCT_3d.Z, MyCT_3d.X, MyCT_3d.Y, MyCT_3d.Z)
        MyCT_3d.GetDisttoOtherPose(CommonCT3d, Quality)
        Q1 = BTuple.TupleMean(Quality)
        'Rep By Suuri Sta 20140826 
        If Q1 > 20 * ScaleMM Then
            'If Q1 > 10 * ScaleMM Then
            'Rep By Suuri End 20140826 
            Exit Function
        End If

        'CalcBestHomMat_Scale(HomMat, ComScale, New Point3D(PairPose.X, PairPose.Y, PairPose.Z), CommonCT3d)
        'MyCT_3d = New Point3D(PairPose.X, PairPose.Y, PairPose.Z)
        'MyCT_3d.SetScale(ComScale)
        'HOperatorSet.AffineTransPoint3D(HomMat, MyCT_3d.X, MyCT_3d.Y, MyCT_3d.Z, MyCT_3d.X, MyCT_3d.Y, MyCT_3d.Z)
        'MyCT_3d.GetDisttoOtherPose(CommonCT3d, Q1)
        'Quality = BTuple.TupleMean(Q1)
        'If Quality > 10 * ScaleMM Then
        '    Exit Function
        'End If
        HOperatorSet.HomMat3dToPose(HomMat, Pose)


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

        CalcPoseByCommonCTandBestScale = True

    End Function

    Public Sub gen_epilines(ByVal hv_Width As Object, ByVal hv_Height As Object)
        ' Local control variables 
        Dim hv_MatrixF As Object = Nothing, hv_Tcount As Object = Nothing
        Dim hv_Ones As Object = Nothing, hv_MatrixID As Object = Nothing
        Dim hv_MatrixMultID As Object = Nothing, hv_Values As Object = Nothing
        Dim hv_V1 As Object = Nothing, hv_V2 As Object = Nothing
        Dim hv_V3 As Object = Nothing
        Dim i As Integer = 0
        Dim hv_R1 As Object = Nothing, hv_C1 As Object = Nothing, hv_R2 As Object = Nothing, hv_C2 As Object = Nothing
        Dim hv_Row As Object = Nothing, hv_Column As Object = Nothing
        ' Initialize local and output iconic variables 
        hv_Row = IS1.Targets.All2D_ST_Trans.Row
        hv_Column = IS1.Targets.All2D_ST_Trans.Col

        If PairFMat Is Nothing Then
            Exit Sub
        End If
        HOperatorSet.CreateMatrix(3, 3, PairFMat, hv_MatrixF)
        hv_Tcount = BTuple.TupleLength(hv_Row)
        HOperatorSet.TupleGenConst(hv_Tcount, 1, hv_Ones)
        HOperatorSet.CreateMatrix(3, hv_Tcount, BTuple.TupleConcat(BTuple.TupleConcat(hv_Column, hv_Row), hv_Ones), hv_MatrixID)
        HOperatorSet.MultMatrix(hv_MatrixF, hv_MatrixID, "AB", hv_MatrixMultID)
        HOperatorSet.GetFullMatrix(hv_MatrixMultID, hv_Values)
        hv_V1 = BTuple.TupleSelectRange(hv_Values, 0, BTuple.TupleSub(hv_Tcount, 1))
        hv_V2 = BTuple.TupleSelectRange(hv_Values, hv_Tcount, BTuple.TupleSub(BTuple.TupleMult(hv_Tcount, 2), 1))
        hv_V3 = BTuple.TupleSelectRange(hv_Values, BTuple.TupleMult(hv_Tcount, 2), BTuple.TupleSub(BTuple.TupleMult(hv_Tcount, 3), 1))
        HOperatorSet.TupleGenConst(hv_Tcount, hv_Width * 1.0, hv_C1)
        HOperatorSet.TupleGenConst(hv_Tcount, 0.0, hv_C2)

        hv_R1 = BTuple.TupleDiv(BTuple.TupleSub(BTuple.TupleMult(-1, hv_V3), BTuple.TupleMult(hv_V1, hv_C1)), hv_V2)
        hv_R2 = BTuple.TupleDiv(BTuple.TupleSub(BTuple.TupleMult(-1, hv_V3), BTuple.TupleMult(hv_V1, hv_C2)), hv_V2)
        For i = 0 To hv_Tcount.I - 1
            ' hv_R1 = BTuple.TupleSelect(hv_R1, i)

            If BTuple.TupleSelect(hv_R1, i) > 0 And BTuple.TupleSelect(hv_R1, i) < hv_Height And BTuple.TupleSelect(hv_R2, i) > 0 And BTuple.TupleSelect(hv_R2, i) < hv_Height Then
                'Stop
            Else
                If hv_Tcount.I = 1 Then
                    hv_R1 = hv_Height * 1.0
                    hv_R2 = New HTuple(0.0)
                    hv_C1 = BTuple.TupleDiv(BTuple.TupleSub(BTuple.TupleMult(-1, hv_V3), BTuple.TupleMult(hv_V2, hv_R1)), hv_V1)
                    hv_C2 = BTuple.TupleDiv(BTuple.TupleSub(BTuple.TupleMult(-1, hv_V3), BTuple.TupleMult(hv_V2, hv_R2)), hv_V1)
                Else
                    hv_R1(i) = hv_Height * 1.0
                    hv_R2(i) = New HTuple(0.0)
                    hv_C1(i) = BTuple.TupleDiv(BTuple.TupleSub(BTuple.TupleMult(-1, BTuple.TupleSelect(hv_V3, i)), BTuple.TupleMult(BTuple.TupleSelect(hv_V2, i), BTuple.TupleSelect(hv_R1, i))), BTuple.TupleSelect(hv_V1, i))
                    hv_C2(i) = BTuple.TupleDiv(BTuple.TupleSub(BTuple.TupleMult(-1, BTuple.TupleSelect(hv_V3, i)), BTuple.TupleMult(BTuple.TupleSelect(hv_V2, i), BTuple.TupleSelect(hv_R2, i))), BTuple.TupleSelect(hv_V1, i))
                End If

            End If
        Next

        EpiLine.R1 = hv_R1
        EpiLine.R2 = hv_R2
        EpiLine.C1 = hv_C1
        EpiLine.C2 = hv_C2


        HOperatorSet.ClearMatrix(hv_MatrixMultID)
        HOperatorSet.ClearMatrix(hv_MatrixF)
        HOperatorSet.ClearMatrix(hv_MatrixID)

    End Sub

    Public Sub CalcTaiouTenIndex()
        If TaiouTenIndex Is Nothing Then
            TaiouTenIndex = New List(Of Object)
        Else
            TaiouTenIndex.Clear()
        End If
        For Each ST As SingleTarget In IS1.Targets.lstST
            Dim ObjIndex As Object = Nothing
            getST_forEpiline(ST.P2ID - 1, ObjIndex)
            TaiouTenIndex.Add(ObjIndex)
        Next
    End Sub

    Public Sub getST_forEpiline(ByVal index As Integer, ByRef hv_ind_dist As Object)

        Dim hv_dist12 As Object = Nothing '点と線の距離

        '  Dim strtemp As String = ""
        ' strtemp = strtemp & ST.P2D.Row & "," & ST.P2D.Col & "," & line12.R1 & "," & line12.C1 & "," & line12.R2 & "," & line12.C2 & vbNewLine
        'My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\EpilineMonitor.csv", strtemp, True)
        HOperatorSet.DistancePl(IS2.Targets.All2D_ST.Row, IS2.Targets.All2D_ST.Col, BTuple.TupleSelect(EpiLine.R1, index), _
                      BTuple.TupleSelect(EpiLine.C1, index), BTuple.TupleSelect(EpiLine.R2, index), BTuple.TupleSelect(EpiLine.C2, index), hv_dist12)
        ' HOperatorSet.TupleFind(BTuple.TupleSgn(BTuple.TupleSub(hv_dist12, 2)), -1, hv_ind_dist)
        hv_ind_dist = BTuple.TupleSelect(BTuple.TupleSortIndex(hv_dist12), 0)
        If BTuple.TupleSelect(hv_dist12, hv_ind_dist) > 1 Then
            hv_ind_dist = -1
        End If
    End Sub
    Public Sub GetEpilineIndexNearestThisPoint(ByVal ST As SingleTarget, ByRef Index As Object)
        Dim hv_dist12 As Object = Nothing '点と線の距離
        HOperatorSet.DistancePl(ST.TP2D.Row, ST.TP2D.Col, EpiLine.R1, EpiLine.C1, EpiLine.R2, EpiLine.C2, hv_dist12) '1 つの点と 1 本の直線間の距離を計算する。 
        ' HOperatorSet.TupleFind(BTuple.TupleSgn(BTuple.TupleSub(hv_dist12, 10)), -1, Index)
        Index = BTuple.TupleSelect(BTuple.TupleSortIndex(hv_dist12), 0)
        If BTuple.TupleSelect(hv_dist12, Index) > 10 Then
            Index = -1
        End If
    End Sub
    Public Sub CalcFmatByPose12()
        Dim RelPose12 As Object = Nothing
        CalcRelPoseBetweenTwoPose(IS1.ImagePose.Pose, IS2.ImagePose.Pose, RelPose12)
        CalcUnitPose(RelPose12)

        '20150115 SUURI Rep Sta 複数カメラ内部パラメータの取扱機能------------------------------
        ' HOperatorSet.RelPoseToFundamentalMatrix(RelPose12, Nothing, Camparam, Camparam, PairFMat, Nothing)
        HOperatorSet.RelPoseToFundamentalMatrix(RelPose12, Nothing, IS1.objCamparam.Camparam, IS2.objCamparam.Camparam, PairFMat, Nothing)
        '20150115 SUURI Rep End 複数カメラ内部パラメータの取扱機能------------------------------

    End Sub

    Public Function CalcFmatTransed(ByVal CamParamOut2 As Object) As Boolean
        Dim PairFMatError As Object = Nothing
        '20150115 SUURI Rep Sta 複数カメラ内部パラメータの取扱機能------------------------------
        'HOperatorSet.ChangeRadialDistortionPoints(IP1.Row, IP1.Col, Camparam, CamParamOut2, IP1.Row, IP1.Col)
        'HOperatorSet.ChangeRadialDistortionPoints(IP2.Row, IP2.Col, Camparam, CamParamOut2, IP2.Row, IP2.Col)
        HOperatorSet.ChangeRadialDistortionPoints(IP1.Row, IP1.Col, IS1.objCamparam.Camparam, IS1.objCamparam.CamParamZero, IP1.Row, IP1.Col)
        HOperatorSet.ChangeRadialDistortionPoints(IP2.Row, IP2.Col, IS2.objCamparam.Camparam, IS2.objCamparam.CamParamZero, IP2.Row, IP2.Col)
        '20150115 SUURI Rep End 複数カメラ内部パラメータの取扱機能------------------------------

        Try
            HOperatorSet.VectorToFundamentalMatrix(IP1.Row, IP1.Col, IP2.Row, IP2.Col, _
                                    New HTuple, New HTuple, New HTuple, New HTuple, New HTuple, New HTuple, _
                                    "gold_standard", PairFMat, New HTuple, PairFMatError, New HTuple, New HTuple, New HTuple, New HTuple, New HTuple)


        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function
    Public Sub MonitorPairInfo(ByVal projectpath As String)
        Dim strtxt As String
        Try
            strtxt = ""
            strtxt = strtxt & Pair_ID & "," & IS1.ImageId & "," & IS2.ImageId & ","
            strtxt = strtxt & PairPose.RelPose(0).D & ","
            strtxt = strtxt & PairPose.RelPose(1).D & ","
            strtxt = strtxt & PairPose.RelPose(2).D & ","
            strtxt = strtxt & PairPose.RelPose(3).D & ","
            strtxt = strtxt & PairPose.RelPose(4).D & ","
            strtxt = strtxt & PairPose.RelPose(5).D & vbNewLine
            My.Computer.FileSystem.WriteAllText(projectpath & "\MonitorPairInfo.csv", strtxt, True)
        Catch ex As Exception
            Exit Sub
        End Try

    End Sub

    'SUURI ADD 20150125
    Public Function CalcPoseByBasePointandBestScale(ByVal lstBasePoint As List(Of Common3DCodedTarget),
                                                    ByRef lstCommon3dCT As List(Of Common3DCodedTarget), _
                                                  ByRef Hommat As Object, ByRef Quality As Object, _
                                                  ByRef ComScale As Object, ByVal ScaleMM As Double) As Boolean
        Dim MyCT1_IP As New ImagePoints
        Dim MyCT2_IP As New ImagePoints
        Dim CommonCT3d As New Point3D
        Dim BaseCT3d As New Point3D

        Dim cnttmpComCT As Integer = 0
        Dim t As Integer
        Dim N As Integer = IIf(lstIS1_ComCT.Count > lstCommon3dCT.Count, lstIS1_ComCT.Count, lstCommon3dCT.Count) * CodedTarget.CTnoSTnum - 1
        Dim X(N) As Object
        Dim Y(N) As Object
        Dim Z(N) As Object
        Dim M As Integer = lstBasePoint.Count * CodedTarget.CTnoSTnum - 1
        Dim XB(N) As Object
        Dim YB(N) As Object
        Dim ZB(N) As Object
        Dim cnt As Integer = 0
        Dim HomMatInvert As Object = Nothing

        CalcPoseByBasePointandBestScale = False

        For Each objC3D As Common3DCodedTarget In lstBasePoint
            For Each MyCT1 As CodedTarget In lstIS1_ComCT
                If MyCT1.myC3DCT.flgUsable = True Then
                    If MyCT1.myC3DCT.PID = objC3D.PID Then
                        For t = 0 To CodedTarget.CTnoSTnum - 1
                            X(cnt) = MyCT1.myC3DCT.lstP3d.Item(t).X
                            Y(cnt) = MyCT1.myC3DCT.lstP3d.Item(t).Y
                            Z(cnt) = MyCT1.myC3DCT.lstP3d.Item(t).Z
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

        Dim MyCT_3d As New Point3D(PairPose.X, PairPose.Y, PairPose.Z)
        'Dim MyCT_3dT As New Point3D
        Dim Scale As Object = Nothing
        CommonCT3d.CalcScale(BaseCT3d, ComScale)
        ' CommonCT3d.SetScale(ComScale)
        ' 'rigid', 'similarity', 'affine', 'projective' 
        HOperatorSet.VectorToHomMat3d("affine", CommonCT3d.X, CommonCT3d.Y, CommonCT3d.Z, BaseCT3d.X, BaseCT3d.Y, BaseCT3d.Z, Hommat)
        HOperatorSet.AffineTransPoint3d(Hommat, CommonCT3d.X, CommonCT3d.Y, CommonCT3d.Z, CommonCT3d.X, CommonCT3d.Y, CommonCT3d.Z)

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

        CalcPoseByBasePointandBestScale = True

    End Function

    Public Function GetRelPoseFrom2Pose() As HTuple
        GetRelPoseFrom2Pose = Nothing
        Dim P1XYZ As HTuple = Nothing, P2XYZ As HTuple = Nothing
        Dim h123 As New HTuple({0, 1, 2})
        Dim Diff As HTuple = Nothing
        Dim Pow As HTuple = Nothing
        Dim Sum As HTuple = Nothing
        Dim Scale As HTuple = Nothing
        Dim P12Kyori As HTuple = Nothing
        HOperatorSet.TupleSelectRange(IS1.ImagePose.Pose, 0, 2, P1XYZ)
        HOperatorSet.TupleSelectRange(IS2.ImagePose.Pose, 0, 2, P2XYZ)
        HOperatorSet.TupleSub(P1XYZ, P2XYZ, Diff)
        HOperatorSet.TuplePow(Diff, 2, Pow)
        HOperatorSet.TupleSum(Pow, Sum)
        HOperatorSet.TupleSqrt(Sum, P12Kyori)
        calcRelPose()
        If cntComCT <> -1 Then
            Scale = New HTuple({P12Kyori.D, P12Kyori.D, P12Kyori.D, 1.0, 1.0, 1.0, 1.0})
            HOperatorSet.TupleMult(PairPose.RelPose, Scale, GetRelPoseFrom2Pose)
            PairPose.SetScale(P12Kyori.D)
        End If

    End Function
End Class
