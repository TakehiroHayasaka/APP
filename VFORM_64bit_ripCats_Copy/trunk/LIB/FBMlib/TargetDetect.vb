﻿Imports System.Runtime.InteropServices
Imports HalconDotNet

Public Class TargetDetect
    ''' 

    ''' CTのLIST
    ''' 

    Public lstCT As List(Of CodedTarget)
    Public lstMismatchCT As List(Of CodedTarget)

    ''' STのLIST
    ''' 

    Public lstST As List(Of SingleTarget)
    Public Image_ID As Integer
    Public All2D As ImagePoints
    Public All2D_MismatchCT As ImagePoints
    Public All2D_ST As ImagePoints
    Public All2D_ST_Trans As ImagePoints
    Public strErrorMessageTxt As List(Of String)
    ''' 

    ''' 初期化
    ''' 

    Public Sub New()
        Image_ID = -1
        If lstCT Is Nothing Then
            lstCT = New List(Of CodedTarget)
        Else
            lstCT.Clear()
        End If

        If lstMismatchCT Is Nothing Then
            lstMismatchCT = New List(Of CodedTarget)
        Else
            lstMismatchCT.Clear()
        End If

        If lstST Is Nothing Then
            lstST = New List(Of SingleTarget)
        Else
            lstST.Clear()
        End If
        All2D = New ImagePoints
        All2D_ST = New ImagePoints
        All2D_ST_Trans = New ImagePoints
    End Sub

    ''' 

    ''' 未使用
    ''' 

    Public Sub New(ByVal ImageId As Integer)
        Image_ID = ImageId

        If lstCT Is Nothing Then
            lstCT = New List(Of CodedTarget)
        Else
            lstCT.Clear()
        End If

        If lstMismatchCT Is Nothing Then
            lstMismatchCT = New List(Of CodedTarget)
        Else
            lstMismatchCT.Clear()
        End If

        If lstST Is Nothing Then
            lstST = New List(Of SingleTarget)
        Else
            lstST.Clear()
        End If
        All2D = New ImagePoints
        All2D_ST = New ImagePoints
        All2D_ST_Trans = New ImagePoints
    End Sub
    ''' 

    ''' TargetDetectクラスのlstCTのCT測点情報を、CodedTargetクラスのリストへ書き込む。また、TargetDetectクラスのlstSTのST測点情報を、SingleTargetクラスのリストへ書き込む。
    ''' 



    Public Sub SaveData()
        'Dim strSaveFullPath As String
        'strSaveFullPath = strSavePath & "Targets" & "\"
        'Dim folderExists As Boolean
        'folderExists = My.Computer.FileSystem.DirectoryExists(strSaveFullPath)
        'If folderExists = False Then
        '    My.Computer.FileSystem.CreateDirectory(strSaveFullPath)
        'End If

        'Dim objSaveTuple As Object = Nothing
        'ExtendVar(objSaveTuple, 2)
        'objSaveBTuple.setvalue(Image_ID, 0)
        'objSaveBTuple.setvalue(lstCT.Count, 1)
        'objSaveBTuple.setvalue(lstST.Count, 2)
        'SaveTupleObj(objSaveTuple, strSaveFullPath & "Common.tpl")
        'Dim CT_IDs As Object = Nothing
        For Each CT As CodedTarget In lstCT
            CT.SaveData()
            '  CT_IDs = BTuple.TupleConcat(CT_IDs, CT.CT_ID)
        Next
        '  SaveTupleObj(CT_IDs, strSaveFullPath & "CT_IDs.tpl")
        '   Dim ST_IDs As Object = Nothing


        For Each ST As SingleTarget In lstST
            ST.SaveData()
            '   ST_IDs = BTuple.TupleConcat(ST_IDs, ST.P2ID)
        Next
        'SaveTupleObj(ST_IDs, strSaveFullPath & "ST_IDs.tpl")

        'All2D.SaveData(strSaveFullPath & "All2D")
        'All2D_ST.SaveData(strSaveFullPath & "All2D_ST")

    End Sub

    ''' 

    ''' 画像のパスと画像IDを読み込む
    ''' 

    Public Sub ReadData(ByVal strReadPath As String, ByVal ImgID As Integer)
        Dim strReadFullPath As String
        'strReadFullPath = strReadPath & "Targets" & "\"
        'Dim folderExists As Boolean
        'folderExists = My.Computer.FileSystem.DirectoryExists(strReadFullPath)
        'If folderExists = False Then
        '    Exit Sub
        'End If
        Dim i As Integer
        'Dim lstCTnum As Integer
        'Dim lstSTnum As Integer

        Dim IDR As IDataReader
        Dim strSqlText As String = ""

        'Dim objOtherParams As New Object
        'If ReadTupleObj(objOtherParams, strReadFullPath & "Common.tpl") Then
        '    Image_ID = CInt(BTuple.TupleSelect(objOtherParams, 0))
        '    lstCTnum = CInt(BTuple.TupleSelect(objOtherParams, 1))
        '    lstSTnum = CInt(BTuple.TupleSelect(objOtherParams, 2))
        'End If
        'Dim CT_IDs As Object = Nothing
        'ReadTupleObj(CT_IDs, strReadFullPath & "CT_IDs.tpl")
        'For i = 0 To lstCTnum - 1
        '    Dim CT As New CodedTarget
        '    CT.CT_ID = CInt(BTuple.TupleSelect(CT_IDs, i))
        '    CT.ReadData(strReadFullPath)
        '    lstCT.Add(CT)
        'Next
        All2D.Row = New HTuple
        All2D.Col = New HTuple

        strSqlText = "SELECT "
        Dim n As Integer = SingleTargetFields.Length
        For i = 0 To n - 2
            strSqlText = strSqlText & SingleTargetFields(i) & ","
        Next
        strSqlText = strSqlText & SingleTargetFields(n - 1) & " "
        strSqlText = strSqlText & "FROM Targets WHERE ImageID=" & ImgID & " AND flgType=2 ORDER BY P2ID,P3ID"
        IDR = dbClass.DoSelect(strSqlText)
        If Not IDR Is Nothing Then
            Do While IDR.Read
                Dim CT As New CodedTarget
                CT.ReadData(IDR)
                lstCT.Add(CT)
#If Halcon = "True" Then
                All2D.Row = BTuple.TupleConcat(All2D.Row, CT.CT_Points.Row)
                All2D.Col = BTuple.TupleConcat(All2D.Col, CT.CT_Points.Col)
#End If
            Loop
            IDR.Close()
        End If



        'Dim ST_IDs As Object = Nothing
        'ReadTupleObj(ST_IDs, strReadFullPath & "ST_IDs.tpl")
        'For i = 0 To lstSTnum - 1
        '    Dim ST As New SingleTarget
        '    ST.P2ID = CInt(BTuple.TupleSelect(ST_IDs, i))
        '    ST.ReadData(strReadFullPath)
        '    lstST.Add(ST)
        'Next

        All2D_ST.Row = Nothing
        All2D_ST.Col = Nothing
        strSqlText = "SELECT "
        For i = 0 To n - 2
            strSqlText = strSqlText & SingleTargetFields(i) & ","
        Next
        strSqlText = strSqlText & SingleTargetFields(n - 1) & " "
        strSqlText = strSqlText & "FROM Targets WHERE ImageID=" & ImgID & " AND flgType=1 ORDER BY P2ID"
        IDR = dbClass.DoSelect(strSqlText)
        If Not IDR Is Nothing Then
            Do While IDR.Read
                Dim ST As New SingleTarget
                ST.ReadData(IDR)
                lstST.Add(ST)
#If Halcon = "True" Then
                All2D.Row = BTuple.TupleConcat(All2D.Row, ST.P2D.Row)
                All2D.Col = BTuple.TupleConcat(All2D.Col, ST.P2D.Col)
                All2D_ST.Row = BTuple.TupleConcat(All2D_ST.Row, ST.P2D.Row)
                All2D_ST.Col = BTuple.TupleConcat(All2D_ST.Col, ST.P2D.Col)
#End If
            Loop
            IDR.Close()
        End If

        'All2D.ReadData(strReadFullPath & "All2D")
        'All2D_ST.ReadData(strReadFullPath & "All2D_ST")

    End Sub

    'ADD By Suurui Sta 20141225------------------------------------------------------------------- 
    ''' 

    ''' DBからターゲット情報を取り込む
    ''' 

    Public Sub ReadDataExtra(ByVal strReadPath As String, ByVal ImgID As Integer)
        'Dim strReadFullPath As String
        'strReadFullPath = strReadPath & "Targets" & "\"
        'Dim folderExists As Boolean
        'folderExists = My.Computer.FileSystem.DirectoryExists(strReadFullPath)
        'If folderExists = False Then
        '    Exit Sub
        'End If
        Dim i As Integer
        'Dim lstCTnum As Integer
        'Dim lstSTnum As Integer

        Dim IDR As IDataReader
        Dim strSqlText As String = ""

        'Dim objOtherParams As New Object
        'If ReadTupleObj(objOtherParams, strReadFullPath & "Common.tpl") Then
        '    Image_ID = CInt(BTuple.TupleSelect(objOtherParams, 0))
        '    lstCTnum = CInt(BTuple.TupleSelect(objOtherParams, 1))
        '    lstSTnum = CInt(BTuple.TupleSelect(objOtherParams, 2))
        'End If
        'Dim CT_IDs As Object = Nothing
        'ReadTupleObj(CT_IDs, strReadFullPath & "CT_IDs.tpl")
        'For i = 0 To lstCTnum - 1
        '    Dim CT As New CodedTarget
        '    CT.CT_ID = CInt(BTuple.TupleSelect(CT_IDs, i))
        '    CT.ReadData(strReadFullPath)
        '    lstCT.Add(CT)
        'Next
        All2D.Row = Nothing
        All2D.Col = Nothing
        ConnectDbFBM(strReadPath)
        'SUURI UPDATE START 20150406
        Dim n As Integer = SingleTargetFields.Length

        Dim blnAverageAllParamTarget As Integer = GetPrivateProfileInt("Kaiseki", "AllParamTarget", 0, My.Application.Info.DirectoryPath & "\vform.ini")
        If blnAverageAllParamTarget = 1 Then
            strSqlText = "SELECT ImageID,P2ID, P3ID,flgUsed, avg(P2D_Row) as PR , avg(P2D_Col) as PC "
            strSqlText = strSqlText & " FROM AllTargets "
            strSqlText = strSqlText & " WHERE ImageID=" & ImgID & " and flgType=2 "
            strSqlText = strSqlText & " GROUP BY ImageID,P2ID,P3ID,flgUsed,flgType "
            strSqlText = strSqlText & " ORDER BY P2ID,P3ID"
            '            SELECT ImageID,P2ID, P3ID,flgUsed, avg(P2D_Row) as PR , avg(P2D_Col) as PC
            'FROM AllTargets
            'WHERE ImageID=1 and flgType=2
            'GROUP BY ImageID,P2ID,P3ID,flgUsed,flgType
            'ORDER BY P2ID,P3ID
        Else


            strSqlText = "SELECT "

            For i = 0 To n - 2
                strSqlText = strSqlText & SingleTargetFields(i) & ","
            Next
            strSqlText = strSqlText & SingleTargetFields(n - 1) & " "
            strSqlText = strSqlText & "FROM Targets WHERE ImageID=" & ImgID & " AND flgType=2 ORDER BY P2ID,ID,P3ID"
        End If
        'SUURI UPDATE END 20150407
        IDR = dbClass.DoSelect(strSqlText)
        If Not IDR Is Nothing Then
            Do While IDR.Read
                Dim CT As New CodedTarget
                CT.ReadData(IDR)
                lstCT.Add(CT)
#If Halcon = "True" Then
                All2D.Row = BTuple.TupleConcat(All2D.Row, CT.CT_Points.Row)
                All2D.Col = BTuple.TupleConcat(All2D.Col, CT.CT_Points.Col)
#End If
            Loop
            IDR.Close()
        End If



        'Dim ST_IDs As Object = Nothing
        'ReadTupleObj(ST_IDs, strReadFullPath & "ST_IDs.tpl")
        'For i = 0 To lstSTnum - 1
        '    Dim ST As New SingleTarget
        '    ST.P2ID = CInt(BTuple.TupleSelect(ST_IDs, i))
        '    ST.ReadData(strReadFullPath)
        '    lstST.Add(ST)
        'Next

        All2D_ST.Row = Nothing
        All2D_ST.Col = Nothing
        strSqlText = "SELECT "
        For i = 0 To n - 2
            strSqlText = strSqlText & SingleTargetFields(i) & ","
        Next
        strSqlText = strSqlText & SingleTargetFields(n - 1) & " "
        strSqlText = strSqlText & "FROM Targets WHERE ImageID=" & ImgID & " AND flgType=1 ORDER BY P2ID"
        IDR = dbClass.DoSelect(strSqlText)
        If Not IDR Is Nothing Then
            Do While IDR.Read
                Dim ST As New SingleTarget
                ST.ReadData(IDR)
                lstST.Add(ST)
#If Halcon = "True" Then
                All2D.Row = BTuple.TupleConcat(All2D.Row, ST.P2D.Row)
                All2D.Col = BTuple.TupleConcat(All2D.Col, ST.P2D.Col)
                All2D_ST.Row = BTuple.TupleConcat(All2D_ST.Row, ST.P2D.Row)
                All2D_ST.Col = BTuple.TupleConcat(All2D_ST.Col, ST.P2D.Col)
#End If
            Loop
            IDR.Close()
        End If

        'All2D.ReadData(strReadFullPath & "All2D")
        'All2D_ST.ReadData(strReadFullPath & "All2D_ST")
        AccessDisConnect()

    End Sub
    'ADD By Suurui End 20141225------------------------------------------------------------------- 

    ''' 

    ''' 未使用
    ''' 

    Public Sub DetectCT_old(ByVal ho_Image As HObject, ByVal hv_CT_ALL_ID As Object, ByVal T_Threshold As Integer)
        ' Stack for temporary objects 
        Dim OTemp(10) As HObject
        Dim SP_O As Integer
        SP_O = 0

        ' Stack for temporary control variables 
        Dim CTemp(10) As Object
        Dim SP_C As Integer
        SP_C = 0

        ' Local iconic variables 
        Dim ho_Region As HObject = Nothing
        Dim ho_ConnectedRegions As HObject = Nothing
        Dim ho_SelectedRegions2 As HObject = Nothing
        Dim ho_SelectedRegions As HObject = Nothing
        Dim ho_RegionFillUp As HObject = Nothing
        Dim ho_RegionIntersection2 As HObject = Nothing
        Dim ho_ObjectSelected1 As HObject = Nothing
        Dim ho_RegionErosion As HObject = Nothing
        Dim ho_RegionIntersection3 As HObject = Nothing
        Dim ho_SelectedRegions4 As HObject = Nothing
        Dim ho_RegionIntersection As HObject = Nothing
        Dim ho_InnerTargets As HObject = Nothing
        Dim ho_SelectedRegions5 As HObject = Nothing
        Dim ho_RegionDifference As HObject = Nothing
        Dim ho_OuterTargets As HObject = Nothing
        Dim ho_tempObj As HObject = Nothing

        ' Local control variables 
        Dim hv_ExpDefaultCtrlDummyVar As Object = Nothing
        Dim hv_NumConnected As Object = Nothing, hv_NumHoles As Object = Nothing
        Dim hv_Number1 As Object = Nothing, hv_Rows As Object = Nothing
        Dim hv_Cols As Object = Nothing, hv_j As Object = Nothing
        Dim hv_i As Object = Nothing, hv_Row5 As Object = Nothing
        Dim hv_Column3 As Object = Nothing, hv_Phi As Object = Nothing
        Dim hv_Length11 As Object = Nothing, hv_Length21 As Object = Nothing
        Dim hv_Area2 As Object = Nothing, hv_Row2 As Object = Nothing
        Dim hv_Column As Object = Nothing, hv_Mean As Object = Nothing
        Dim hv_Temp As Object = Nothing, hv_Number2 As Object = Nothing
        Dim hv_Area As Object = Nothing, hv_Row As Object = Nothing
        Dim hv_Col As Object = Nothing, hv_Row1 As Object = Nothing
        Dim hv_Col1 As Object = Nothing, hv_Area3 As Object = Nothing
        Dim hv_Mean1 As Object = Nothing, hv_BRow As Object = Nothing
        Dim hv_BCol As Object = Nothing, hv_mindist As Object = Nothing
        Dim hv_tempIndex As Object = Nothing, hv_C0_Row As Object = Nothing
        Dim hv_C0_Col As Object = Nothing, hv_C1_Row As Object = Nothing
        Dim hv_C1_Col As Object = Nothing, hv_C2_Row As Object = Nothing
        Dim hv_C2_Col As Object = Nothing, hv_C3_Row As Object = Nothing
        Dim hv_C3_Col As Object = Nothing, hv_t As Object = Nothing
        Dim hv_Distance As Object = Nothing, hv_blnCheck As Object = Nothing
        Dim hv_Vec1_R As Object = Nothing, hv_Vec1_C As Object = Nothing
        Dim hv_Vec2_R As Object = Nothing, hv_Vec2_C As Object = Nothing
        Dim hv_Vec3_R As Object = Nothing, hv_Vec3_C As Object = Nothing
        Dim hv_Gaiseki1 As Object = Nothing, hv_Gaiseki2 As Object = Nothing
        Dim hv_Result As Object = Nothing, hv_I1 As Object = Nothing
        Dim hv_I2 As Object = Nothing, hv_I3 As Object = Nothing
        Dim hv_SCol As Object = Nothing, hv_SRow As Object = Nothing
        Dim hv_HomMat2D As Object = Nothing, hv_Covariance As Object = Nothing
        Dim hv_GRow As Object = Nothing, hv_GCol As Object = Nothing
        Dim hv_Angle As Object = Nothing, hv_Quot As Object = Nothing
        Dim hv_Int As Object = Nothing, hv_STR As Object = Nothing
        Dim hv_CTSTRCODE As Object = Nothing, hv_Indices As Object = Nothing
        Dim hv_CT_ID As Object = Nothing, hv_OutRow As Object = Nothing
        Dim hv_OutCol As Object = Nothing, hv_InRow As Object = Nothing
        Dim hv_InCol As Object = Nothing, hv_AngIndex As Object = Nothing
        'Dim hv_O1_Row As Object = Nothing, hv_O1_Col As Object = Nothing
        'Dim hv_O2_Row As Object = Nothing, hv_O2_Col As Object = Nothing
        'Dim hv_O3_Row As Object = Nothing, hv_O3_Col As Object = Nothing
        'Dim hv_O4_Row As Object = Nothing, hv_O4_Col As Object = Nothing

        ' Initialize local and output iconic variables 

        HOperatorSet.GenEmptyObj(ho_Region)
        HOperatorSet.GenEmptyObj(ho_ConnectedRegions)
        HOperatorSet.GenEmptyObj(ho_SelectedRegions2)
        HOperatorSet.GenEmptyObj(ho_SelectedRegions)
        HOperatorSet.GenEmptyObj(ho_RegionFillUp)
        HOperatorSet.GenEmptyObj(ho_RegionIntersection2)
        HOperatorSet.GenEmptyObj(ho_ObjectSelected1)
        HOperatorSet.GenEmptyObj(ho_RegionErosion)
        HOperatorSet.GenEmptyObj(ho_RegionIntersection3)
        HOperatorSet.GenEmptyObj(ho_SelectedRegions4)
        HOperatorSet.GenEmptyObj(ho_RegionIntersection)
        HOperatorSet.GenEmptyObj(ho_InnerTargets)
        HOperatorSet.GenEmptyObj(ho_SelectedRegions5)
        HOperatorSet.GenEmptyObj(ho_RegionDifference)
        HOperatorSet.GenEmptyObj(ho_OuterTargets)
        HOperatorSet.GenEmptyObj(ho_tempObj)

        If lstCT Is Nothing Then
            lstCT = New List(Of CodedTarget)
        Else
            lstCT.Clear()
        End If
        HOperatorSet.ClearObj(ho_Region)
        HOperatorSet.Threshold(ho_Image, ho_Region, T_Threshold, 255)
        HOperatorSet.VarThreshold(ho_Image, ho_Region, 30, 30, 0.2, T_Threshold, "light")

        HOperatorSet.ClearObj(ho_ConnectedRegions)
        HOperatorSet.Connection(ho_Region, ho_ConnectedRegions)
        HOperatorSet.ClearObj(ho_SelectedRegions2)
        HOperatorSet.SelectShape(ho_ConnectedRegions, ho_SelectedRegions2, "area", "and", 10, 99999)
        HOperatorSet.ClearObj(ho_SelectedRegions)
        HOperatorSet.SelectShape(ho_SelectedRegions2, ho_SelectedRegions, "holes_num", "and", 1, _
            1)
        HOperatorSet.ClearObj(ho_RegionFillUp)
        HOperatorSet.FillUp(ho_SelectedRegions, ho_RegionFillUp)
        HOperatorSet.ClearObj(ho_RegionIntersection2)
        HOperatorSet.Intersection(ho_RegionFillUp, ho_SelectedRegions2, ho_RegionIntersection2)
        HOperatorSet.ClearObj(ho_SelectedRegions2)
        HOperatorSet.SelectShape(ho_RegionIntersection2, ho_SelectedRegions2, "connect_num", "and", _
            9, 9)
        HOperatorSet.ClearObj(ho_RegionFillUp)
        HOperatorSet.FillUp(ho_SelectedRegions2, ho_RegionFillUp)
        HOperatorSet.CopyObj(ho_RegionFillUp, OTemp(SP_O), 1, -1)
        SP_O = SP_O + 1
        HOperatorSet.ClearObj(ho_RegionFillUp)
        HOperatorSet.Connection(OTemp(SP_O - 1), ho_RegionFillUp)
        HOperatorSet.ClearObj(OTemp(SP_O - 1))
        SP_O = 0
        HOperatorSet.CopyObj(ho_SelectedRegions2, OTemp(SP_O), 1, -1)
        SP_O = SP_O + 1
        HOperatorSet.ClearObj(ho_SelectedRegions2)
        HOperatorSet.Connection(OTemp(SP_O - 1), ho_SelectedRegions2)
        HOperatorSet.ClearObj(OTemp(SP_O - 1))
        SP_O = 0
        HOperatorSet.ConnectAndHoles(ho_RegionIntersection2, hv_NumConnected, hv_NumHoles)
        HOperatorSet.CountObj(ho_RegionFillUp, hv_Number1)
        hv_Rows = Nothing
        hv_Cols = Nothing
        hv_j = 0
        For hv_i = 1 To hv_Number1 Step 1
            HOperatorSet.ClearObj(ho_ObjectSelected1)
            HOperatorSet.SelectObj(ho_RegionFillUp, ho_ObjectSelected1, hv_i)
            HOperatorSet.SmallestRectangle2(ho_ObjectSelected1, hv_Row5, hv_Column3, hv_Phi, hv_Length11, _
                hv_Length21)
            HOperatorSet.ClearObj(ho_RegionErosion)
            HOperatorSet.GenEllipse(ho_RegionErosion, hv_Row5, hv_Column3, hv_Phi, BTuple.TupleDiv(hv_Length11, _
                2), BTuple.TupleDiv(hv_Length21, 2))
            HOperatorSet.ClearObj(ho_RegionIntersection3)
            HOperatorSet.Intersection(ho_SelectedRegions2, ho_ObjectSelected1, ho_RegionIntersection3 _
                )
            HOperatorSet.ClearObj(ho_SelectedRegions4)
            HOperatorSet.SelectShape(ho_RegionIntersection3, ho_SelectedRegions4, "area", "and", 1, _
                99999)
            HOperatorSet.ClearObj(ho_RegionIntersection)
            HOperatorSet.Intersection(ho_SelectedRegions4, ho_RegionErosion, ho_RegionIntersection)
            HOperatorSet.CopyObj(ho_RegionIntersection, OTemp(SP_O), 1, -1)
            SP_O = SP_O + 1
            HOperatorSet.ClearObj(ho_RegionIntersection)
            HOperatorSet.SelectShape(OTemp(SP_O - 1), ho_RegionIntersection, "area", "and", 1, 99999)
            HOperatorSet.ClearObj(OTemp(SP_O - 1))
            SP_O = 0
            HOperatorSet.AreaCenter(ho_RegionIntersection, hv_Area2, hv_Row2, hv_Column)
            HOperatorSet.TupleMean(hv_Area2, hv_Mean)
            If BTuple.TupleLessEqual(BTuple.TupleLength(hv_Area2), 4) = 1 Then
                HOperatorSet.ClearObj(ho_InnerTargets)
                HOperatorSet.SelectShape(ho_RegionIntersection, ho_InnerTargets, "area", "and", 1, _
                    9999999)
            Else
                HOperatorSet.ClearObj(ho_InnerTargets)
                HOperatorSet.SelectShape(ho_RegionIntersection, ho_InnerTargets, "area", "and", BTuple.TupleSub( _
                    hv_Mean, 10), 9999999)
            End If
            HOperatorSet.CountObj(ho_InnerTargets, hv_Number2)
            If BTuple.TupleEqual(hv_Number2, 4) = 1 Then
                HOperatorSet.AreaCenter(ho_ObjectSelected1, hv_Area, hv_Row, hv_Col)
                HOperatorSet.ClearObj(ho_tempObj)
                HOperatorSet.DilationCircle(ho_InnerTargets, ho_tempObj, 2)
                HOperatorSet.AreaCenterGray(ho_tempObj, ho_Image, hv_Area, hv_Row1, hv_Col1)
                '外側のターゲットを抽出
                HOperatorSet.AreaCenter(ho_SelectedRegions4, hv_Area3, hv_ExpDefaultCtrlDummyVar, hv_ExpDefaultCtrlDummyVar)
                HOperatorSet.TupleMean(hv_Area3, hv_Mean1)
                HOperatorSet.ClearObj(ho_SelectedRegions5)
                HOperatorSet.SelectShape(ho_SelectedRegions4, ho_SelectedRegions5, "area", "and", 1, _
                    hv_Mean1)
                HOperatorSet.ClearObj(ho_RegionDifference)
                HOperatorSet.Difference(ho_SelectedRegions5, ho_InnerTargets, ho_RegionDifference _
                    )
                HOperatorSet.ClearObj(ho_OuterTargets)
                HOperatorSet.SelectShape(ho_RegionDifference, ho_OuterTargets, "area", "and", 1, 99999)
                HOperatorSet.ClearObj(ho_tempObj)
                HOperatorSet.DilationCircle(ho_OuterTargets, ho_tempObj, 2)
                HOperatorSet.AreaCenterGray(ho_tempObj, ho_Image, hv_ExpDefaultCtrlDummyVar, hv_OutRow, hv_OutCol)
                '外側のターゲットを抽出
                hv_mindist = 999999
                hv_tempIndex = -1
                hv_C0_Row = 0
                hv_C0_Col = 0
                hv_C1_Row = 0
                hv_C1_Col = 0
                hv_C2_Row = 0
                hv_C2_Col = 0
                hv_C3_Row = 0
                hv_C3_Col = 0
                For hv_t = 0 To 3 Step 1
                    HOperatorSet.DistancePp(hv_Row, hv_Col, BTuple.TupleSelect(hv_Row1, hv_t), BTuple.TupleSelect( _
                        hv_Col1, hv_t), hv_Distance)
                    If BTuple.TupleGreater(hv_mindist, hv_Distance) = 1 Then
                        hv_C0_Row = BTuple.TupleSelect(hv_Row1, hv_t)
                        hv_C0_Col = BTuple.TupleSelect(hv_Col1, hv_t)
                        hv_tempIndex = hv_t
                        hv_mindist = hv_Distance
                    End If
                Next
                CTemp(SP_C) = hv_Rows
                SP_C = SP_C + 1
                HOperatorSet.TupleConcat(CTemp(SP_C - 1), hv_C0_Row, hv_Rows)
                SP_C = 0
                CTemp(SP_C) = hv_Cols
                SP_C = SP_C + 1
                HOperatorSet.TupleConcat(CTemp(SP_C - 1), hv_C0_Col, hv_Cols)
                SP_C = 0
                CTemp(SP_C) = hv_Row1
                SP_C = SP_C + 1
                HOperatorSet.TupleRemove(CTemp(SP_C - 1), hv_tempIndex, hv_Row1)
                SP_C = 0
                CTemp(SP_C) = hv_Col1
                SP_C = SP_C + 1
                HOperatorSet.TupleRemove(CTemp(SP_C - 1), hv_tempIndex, hv_Col1)
                SP_C = 0
                hv_blnCheck = -1
                hv_Vec1_R = BTuple.TupleSub(BTuple.TupleSelect(hv_Row1, 0), hv_C0_Row)
                hv_Vec1_C = BTuple.TupleSub(BTuple.TupleSelect(hv_Col1, 0), hv_C0_Col)
                hv_Vec2_R = BTuple.TupleSub(BTuple.TupleSelect(hv_Row1, 1), hv_C0_Row)
                hv_Vec2_C = BTuple.TupleSub(BTuple.TupleSelect(hv_Col1, 1), hv_C0_Col)
                hv_Vec3_R = BTuple.TupleSub(BTuple.TupleSelect(hv_Row1, 2), hv_C0_Row)
                hv_Vec3_C = BTuple.TupleSub(BTuple.TupleSelect(hv_Col1, 2), hv_C0_Col)
                CalcGaiseki(hv_Vec1_R, hv_Vec1_C, hv_Vec2_R, hv_Vec2_C, hv_Gaiseki1)
                CalcGaiseki(hv_Vec1_R, hv_Vec1_C, hv_Vec3_R, hv_Vec3_C, hv_Gaiseki2)
                hv_Result = BTuple.TupleMult(hv_Gaiseki1, hv_Gaiseki2)
                If BTuple.TupleGreater(hv_Result, 0) = 1 Then
                    CalcGaiseki(hv_Vec2_R, hv_Vec2_C, hv_Vec1_R, hv_Vec1_C, hv_Gaiseki1)
                    CalcGaiseki(hv_Vec2_R, hv_Vec2_C, hv_Vec3_R, hv_Vec3_C, hv_Gaiseki2)
                    hv_Result = BTuple.TupleMult(hv_Gaiseki1, hv_Gaiseki2)
                    If BTuple.TupleGreater(hv_Result, 0) = 1 Then
                        CalcGaiseki(hv_Vec3_R, hv_Vec3_C, hv_Vec1_R, hv_Vec1_C, hv_Gaiseki1)
                        CalcGaiseki(hv_Vec3_R, hv_Vec3_C, hv_Vec2_R, hv_Vec2_C, hv_Gaiseki2)
                        hv_I1 = 2
                        If BTuple.TupleLess(hv_Gaiseki1, 0) = 1 Then
                            hv_I2 = 0
                            hv_I3 = 1
                        Else
                            hv_I2 = 1
                            hv_I3 = 0
                        End If
                    Else
                        hv_I1 = 1
                        If BTuple.TupleLess(hv_Gaiseki1, 0) = 1 Then
                            hv_I2 = 0
                            hv_I3 = 2
                        Else
                            hv_I2 = 2
                            hv_I3 = 0
                        End If
                    End If
                Else
                    hv_I1 = 0
                    If BTuple.TupleLess(hv_Gaiseki1, 0) = 1 Then
                        hv_I2 = 1
                        hv_I3 = 2
                    Else
                        hv_I2 = 2
                        hv_I3 = 1
                    End If
                End If
                hv_C1_Row = BTuple.TupleSelect(hv_Row1, hv_I1)
                hv_C1_Col = BTuple.TupleSelect(hv_Col1, hv_I1)
                hv_C2_Row = BTuple.TupleSelect(hv_Row1, hv_I2)
                hv_C2_Col = BTuple.TupleSelect(hv_Col1, hv_I2)
                hv_C3_Row = BTuple.TupleSelect(hv_Row1, hv_I3)
                hv_C3_Col = BTuple.TupleSelect(hv_Col1, hv_I3)
                hv_InRow = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(hv_C0_Row, _
                    hv_C1_Row), hv_C2_Row), hv_C3_Row)
                hv_InCol = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(hv_C0_Col, _
                    hv_C1_Col), hv_C2_Col), hv_C3_Col)
                hv_SCol = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(500.0#, 512.0#), _
                    506.0#), 506)
                hv_SRow = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(500.0#, 500.0#), _
                    489.608), 510.392)
                HOperatorSet.VectorToProjHomMat2d(hv_InRow, hv_InCol, hv_SRow, hv_SCol, "gold_standard", _
                    Nothing, Nothing, Nothing, Nothing, _
                    Nothing, Nothing, hv_HomMat2D, hv_Covariance)
                HOperatorSet.ProjectiveTransPixel(hv_HomMat2D, hv_OutRow, hv_OutCol, hv_BRow, hv_BCol)
                hv_GRow = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(500.0#, 500.0#), _
                    500.0#), 500.0#)
                hv_GCol = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(500.0#, 500.0#), _
                    500.0#), 500.0#)
                HOperatorSet.AngleLx(hv_GRow, hv_GCol, hv_BRow, hv_BCol, hv_Angle)
                CTemp(SP_C) = hv_Angle
                SP_C = SP_C + 1
                HOperatorSet.TupleDeg(CTemp(SP_C - 1), hv_Angle)
                SP_C = 0
                CTemp(SP_C) = hv_Angle
                SP_C = SP_C + 1
                HOperatorSet.TupleAdd(CTemp(SP_C - 1), 180, hv_Angle)
                SP_C = 0
                HOperatorSet.TupleSortIndex(hv_Angle, hv_AngIndex)
                CTemp(SP_C) = hv_Angle
                SP_C = SP_C + 1
                HOperatorSet.TupleSort(CTemp(SP_C - 1), hv_Angle)
                SP_C = 0
                HOperatorSet.TupleDiv(hv_Angle, 30, hv_Quot)
                HOperatorSet.TupleRound(hv_Quot, hv_Int)
                HOperatorSet.TupleString(hv_Int, "d", hv_STR)
                hv_CTSTRCODE = BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleSelect( _
                    hv_STR, 0), BTuple.TupleSelect(hv_STR, 1)), BTuple.TupleSelect(hv_STR, 2)), BTuple.TupleSelect( _
                    hv_STR, 3))
                HOperatorSet.TupleFind(hv_CT_ALL_ID, hv_CTSTRCODE, hv_Indices)
                hv_CT_ID = BTuple.TupleAdd(hv_Indices, 1)
                'hv_O1_Row = BTuple.TupleSelect(hv_OutRow, BTuple.TupleSelect(hv_AngIndex, 0))
                'hv_O1_Col = BTuple.TupleSelect(hv_OutCol, BTuple.TupleSelect(hv_AngIndex, 0))
                'hv_O2_Row = BTuple.TupleSelect(hv_OutRow, BTuple.TupleSelect(hv_AngIndex, 1))
                'hv_O2_Col = BTuple.TupleSelect(hv_OutCol, BTuple.TupleSelect(hv_AngIndex, 1))
                'hv_O3_Row = BTuple.TupleSelect(hv_OutRow, BTuple.TupleSelect(hv_AngIndex, 2))
                'hv_O3_Col = BTuple.TupleSelect(hv_OutCol, BTuple.TupleSelect(hv_AngIndex, 2))
                'hv_O4_Row = BTuple.TupleSelect(hv_OutRow, BTuple.TupleSelect(hv_AngIndex, 3))
                'hv_O4_Col = BTuple.TupleSelect(hv_OutCol, BTuple.TupleSelect(hv_AngIndex, 3))
                hv_OutRow = BTuple.TupleSelect(hv_OutRow, hv_AngIndex)
                hv_OutCol = BTuple.TupleSelect(hv_OutCol, hv_AngIndex)

                Dim NewCT As New CodedTarget
                NewCT.CT_ID = CInt(hv_CT_ID)

                NewCT.CT_Points.Row = BTuple.TupleConcat(hv_InRow, hv_OutRow)
                NewCT.CT_Points.Col = BTuple.TupleConcat(hv_InCol, hv_OutCol)
                lstCT.Add(NewCT)
                All2D.Row = BTuple.TupleConcat(All2D.Row, NewCT.CT_Points.Row)
                All2D.Col = BTuple.TupleConcat(All2D.Col, NewCT.CT_Points.Col)
            End If
        Next
        HOperatorSet.ClearObj(ho_Region)
        HOperatorSet.ClearObj(ho_ConnectedRegions)
        HOperatorSet.ClearObj(ho_SelectedRegions2)
        HOperatorSet.ClearObj(ho_SelectedRegions)
        HOperatorSet.ClearObj(ho_RegionFillUp)
        HOperatorSet.ClearObj(ho_RegionIntersection2)
        HOperatorSet.ClearObj(ho_ObjectSelected1)
        HOperatorSet.ClearObj(ho_RegionErosion)
        HOperatorSet.ClearObj(ho_RegionIntersection3)
        HOperatorSet.ClearObj(ho_SelectedRegions4)
        HOperatorSet.ClearObj(ho_RegionIntersection)
        HOperatorSet.ClearObj(ho_InnerTargets)
        HOperatorSet.ClearObj(ho_SelectedRegions5)
        HOperatorSet.ClearObj(ho_RegionDifference)
        HOperatorSet.ClearObj(ho_OuterTargets)
        HOperatorSet.ClearObj(ho_tempObj)


    End Sub

    Private Sub CalcGaiseki(ByVal hv_R1 As Object, ByVal hv_C1 As Object, ByVal hv_R2 As Object, ByVal hv_C2 As Object, ByRef hv_Gaiseki As Object)
        hv_Gaiseki = BTuple.TupleSub(BTuple.TupleMult(hv_R2, hv_C1), BTuple.TupleMult(hv_R1, hv_C2)) '2 つの tuple の差分を取る。
    End Sub


    ''' 

    ''' ST，CTの抽出（一括処理の実行プログラム）
    ''' 

    ''' Main procedure 
    Public Sub DetectTargets(ByVal ImageID As Integer, ByVal ho_Image As HObject, ByVal hv_CT_IDs As Object, ByVal T_Threshold As Integer)
        ' Stack for temporary control variables 
        Dim CTemp(10) As Object
        Dim SP_C As Integer
        SP_C = 0

        ' Stack for temporary objects 
        Dim OTemp(10) As HObject
        Dim SP_O As Integer
        SP_O = 0


        ' Local iconic variables 
        Dim ho_Region As HObject = Nothing, ho_ConnectedRegions As HObject = Nothing
        Dim ho_SelectedRegions As HObject = Nothing
        Dim ho_RegionFillUp As HObject = Nothing, ho_RegionIntersection As HObject = Nothing
        Dim ho_SelectedRegions2 As HObject = Nothing
        Dim ho_ConnectedRegions1 As HObject = Nothing
        Dim ho_ConnectedRegions2 As HObject = Nothing
        Dim ho_ObjectSelected As HObject = Nothing
        Dim ho_ConnectedRegions3 As HObject = Nothing
        Dim ho_Waku As HObject = Nothing, ho_FillWaku As HObject = Nothing
        Dim ho_Contour As HObject = Nothing, ho_RegressContours As HObject = Nothing
        Dim ho_UnionContours As HObject = Nothing, ho_ContoursSplit As HObject = Nothing
        Dim ho_RegionIntersection1 As HObject = Nothing
        Dim ho_RegionDifference As HObject = Nothing
        Dim ho_ConnectedRegions4 As HObject = Nothing
        Dim ho_TransRegions As HObject = Nothing
        Dim ho_RegionDifference1 As HObject = Nothing
        Dim ho_InTarget As HObject = Nothing
        Dim ho_ObjectSelected1 As HObject = Nothing
        Dim ho_SelectedRegionsT As HObject = Nothing
        Dim ho_SingleTargetRegion As HObject = Nothing
        Dim ho_Difference As HObject = Nothing
        Dim ho_CT_Regions As HObject = Nothing
        Dim ho_tempObj As HObject = Nothing
        Dim ho_ImageChannel1 As HObject = Nothing
        ' Local control variables 
        Dim hv_WindowHandle As Object = Nothing
        Dim hv_Number As Object = Nothing, hv_Index As Object = Nothing
        Dim hv_objNum As Object = Nothing, hv_Rows As Object = Nothing
        Dim hv_Columns As Object = Nothing, hv_RowBegin As Object = Nothing
        Dim hv_ColBegin As Object = Nothing, hv_RowEnd As Object = Nothing
        Dim hv_ColEnd As Object = Nothing, hv_Nr As Object = Nothing
        Dim hv_Nc As Object = Nothing, hv_Dist As Object = Nothing
        Dim hv_IsParallel As Object = Nothing, hv_Area As Object = Nothing
        Dim hv_Row As Object = Nothing, hv_Column As Object = Nothing
        Dim hv_Sorted As Object = Nothing, hv_SortedAreaIndex As Object = Nothing
        Dim hv_AnaRow As Object = Nothing, hv_AnaColumn As Object = Nothing
        Dim hv_DiffR As Object = Nothing, hv_DiffC As Object = Nothing
        Dim hv_PowR As Object = Nothing, hv_PowC As Object = Nothing
        Dim hv_Sum As Object = Nothing, hv_Distance As Object = Nothing
        Dim hv_Indices As Object = Nothing, hv_tempIndex As Object = Nothing
        Dim hv_C0_Row As Object = Nothing, hv_C0_Col As Object = Nothing
        Dim hv_C1_Row As Object = Nothing, hv_C1_Col As Object = Nothing
        Dim hv_C2_Row As Object = Nothing, hv_C2_Col As Object = Nothing
        Dim hv_C3_Row As Object = Nothing, hv_C3_Col As Object = Nothing
        Dim hv_blnCheck As Object = Nothing, hv_Vec1_R As Object = Nothing
        Dim hv_Vec1_C As Object = Nothing, hv_Vec2_R As Object = Nothing
        Dim hv_Vec2_C As Object = Nothing, hv_Vec3_R As Object = Nothing
        Dim hv_Vec3_C As Object = Nothing, hv_Gaiseki1 As Object = Nothing
        Dim hv_Gaiseki2 As Object = Nothing, hv_Result As Object = Nothing
        Dim hv_I1 As Object = Nothing, hv_I2 As Object = Nothing
        Dim hv_I3 As Object = Nothing, hv_OutRow As Object = Nothing
        Dim hv_OutCol As Object = Nothing, hv_SRow As Object = Nothing
        Dim hv_SCol As Object = Nothing, hv_HomMat2D As Object = Nothing
        Dim hv_Covariance As Object = Nothing, hv_MeanArea As Object = Nothing
        Dim hv_InArea As Object = Nothing, hv_InRow As Object = Nothing
        Dim hv_InColumn As Object = Nothing, hv_RowTrans As Object = Nothing
        Dim hv_ColTrans As Object = Nothing, hv_RowIndex As Object = Nothing
        Dim hv_ColIndex As Object = Nothing, hv_CT_Number As Object = Nothing
        Dim hv_strCT_Number As Object = Nothing, hv_CT_ID_Index As Object = Nothing
        Dim hv_CT_ID As Object = Nothing, hv_Length As Object = Nothing
        Dim hv_Indices1 As Object = Nothing, hv_Selected As Object = Nothing
        Dim hv_Sorted1 As Object = Nothing, hv_CT_NumberIndex As Object = Nothing
        Dim hv_CTRow As Object = Nothing, hv_CTCol As Object = Nothing
        Dim hv_ST_num As Object = Nothing, hv_AreaST As Object = Nothing
        Dim hv_ind As Object = Nothing, hv_ST_Rows As Object = Nothing
        Dim hv_ST_Cols As Object = Nothing, hv_ST_Row As Object = Nothing
        Dim hv_ST_Col As Object = Nothing

        ' Initialize local and output iconic variables 

        HOperatorSet.GenEmptyObj(ho_Region)
        HOperatorSet.GenEmptyObj(ho_ConnectedRegions)
        HOperatorSet.GenEmptyObj(ho_SelectedRegions)
        HOperatorSet.GenEmptyObj(ho_RegionFillUp)
        HOperatorSet.GenEmptyObj(ho_RegionIntersection)
        HOperatorSet.GenEmptyObj(ho_SelectedRegions2)
        HOperatorSet.GenEmptyObj(ho_ConnectedRegions1)
        HOperatorSet.GenEmptyObj(ho_ConnectedRegions2)
        HOperatorSet.GenEmptyObj(ho_ObjectSelected)
        HOperatorSet.GenEmptyObj(ho_ConnectedRegions3)
        HOperatorSet.GenEmptyObj(ho_Waku)
        HOperatorSet.GenEmptyObj(ho_FillWaku)
        HOperatorSet.GenEmptyObj(ho_Contour)
        HOperatorSet.GenEmptyObj(ho_RegressContours)
        HOperatorSet.GenEmptyObj(ho_UnionContours)
        HOperatorSet.GenEmptyObj(ho_ContoursSplit)
        HOperatorSet.GenEmptyObj(ho_RegionIntersection1)
        HOperatorSet.GenEmptyObj(ho_RegionDifference)
        HOperatorSet.GenEmptyObj(ho_ConnectedRegions4)
        HOperatorSet.GenEmptyObj(ho_RegionDifference1)
        HOperatorSet.GenEmptyObj(ho_InTarget)
        HOperatorSet.GenEmptyObj(ho_ObjectSelected1)

        HOperatorSet.GenEmptyObj(ho_CT_Regions)
        HOperatorSet.GenEmptyObj(ho_SelectedRegionsT)
        HOperatorSet.GenEmptyObj(ho_SingleTargetRegion)
        HOperatorSet.GenEmptyObj(ho_Difference)
        HOperatorSet.GenEmptyObj(ho_tempObj)

        HOperatorSet.GenEmptyObj(ho_ImageChannel1)
        All2D.Row = Nothing
        All2D.Col = Nothing

        If lstCT Is Nothing Then
            lstCT = New List(Of CodedTarget)
        Else
            lstCT.Clear()
        End If

        'mean_image (Image, Image, 9, 9)

        ' HOperatorSet.Threshold(ho_Image, ho_Region, T_Threshold, 255)

        'HOperatorSet.VarThreshold(ho_Image, ho_Region, 10, 10, 0.2, T_Threshold, "light")
        'HOperatorSet.ClearObj(ho_tempObj)
        'HOperatorSet.BinThreshold(ho_Image, ho_tempObj)
        'HOperatorSet.ClearObj(ho_Region)
        'HOperatorSet.Complement(ho_tempObj, ho_Region)
        HOperatorSet.ClearObj(ho_Region)
        HOperatorSet.ClearObj(ho_ImageChannel1)
        GetFirstRegionG(ho_Image, ho_Region, ho_ImageChannel1, T_Threshold)
        'HOperatorSet.Threshold(ho_Image, ho_Region, 100, 255)
        HOperatorSet.ClearObj(ho_ConnectedRegions)
        HOperatorSet.Connection(ho_Region, ho_ConnectedRegions)
        HOperatorSet.ClearObj(ho_SelectedRegions)
        HOperatorSet.SelectShape(ho_ConnectedRegions, ho_SelectedRegions, "holes_num", "and", 2, 2)
        HOperatorSet.ClearObj(ho_RegionFillUp)
        HOperatorSet.FillUp(ho_SelectedRegions, ho_RegionFillUp)
        HOperatorSet.ClearObj(ho_RegionIntersection)
        HOperatorSet.Intersection(ho_RegionFillUp, ho_ConnectedRegions, ho_RegionIntersection)
        HOperatorSet.ClearObj(ho_SelectedRegions2)
        HOperatorSet.SelectShape(ho_RegionIntersection, ho_SelectedRegions2, "connect_num", "and", 5, 5)
        HOperatorSet.ClearObj(ho_RegionFillUp)
        HOperatorSet.FillUp(ho_SelectedRegions2, ho_RegionFillUp)
        HOperatorSet.ClearObj(ho_ConnectedRegions1)
        HOperatorSet.Connection(ho_RegionFillUp, ho_ConnectedRegions1)
        HOperatorSet.ClearObj(ho_ConnectedRegions2)
        HOperatorSet.Connection(ho_SelectedRegions2, ho_ConnectedRegions2)
        HOperatorSet.CountObj(ho_RegionFillUp, hv_Number)
        HOperatorSet.ClearObj(ho_CT_Regions)
        HOperatorSet.GenEmptyObj(ho_CT_Regions)

        For hv_Index = 1 To hv_Number Step 1
            HOperatorSet.ClearObj(ho_ObjectSelected)
            HOperatorSet.SelectObj(ho_SelectedRegions2, ho_ObjectSelected, hv_Index)
            HOperatorSet.ClearObj(ho_ConnectedRegions3)
            HOperatorSet.Connection(ho_ObjectSelected, ho_ConnectedRegions3)
            HOperatorSet.CountObj(ho_ConnectedRegions3, hv_objNum)
            If BTuple.TupleEqual(hv_objNum, 5) = 1 Then
                HOperatorSet.ClearObj(ho_Waku)
                HOperatorSet.SelectShape(ho_ConnectedRegions3, ho_Waku, "holes_num", "and", 2, 2)
                HOperatorSet.ClearObj(ho_FillWaku)
                HOperatorSet.FillUp(ho_Waku, ho_FillWaku)
                '枠の４頂点を抽出
                HOperatorSet.ClearObj(ho_Contour)
                HOperatorSet.GenContourRegionXld(ho_FillWaku, ho_Contour, "border")
                'get_region_polygon (FillWaku, 5, Rows, Columns)
                'gen_contour_polygon_xld (Contour, Rows, Columns)
                '必ず四角を見つけること！！！！！！！！
                'まだまだ
                HOperatorSet.ClearObj(ho_RegressContours)
                HOperatorSet.RegressContoursXld(ho_Contour, ho_RegressContours, "median", 1)
                HOperatorSet.ClearObj(ho_UnionContours)
                HOperatorSet.UnionCollinearContoursXld(ho_RegressContours, ho_UnionContours, 10, 2, 1, 0.1, "attr_keep")
                HOperatorSet.ClearObj(ho_ContoursSplit)
                HOperatorSet.SegmentContoursXld(ho_UnionContours, ho_ContoursSplit, "lines", 5, 4, 2)
                HOperatorSet.LengthXld(ho_ContoursSplit, hv_Length)
                HOperatorSet.TupleSortIndex(hv_Length, hv_Indices1)
                HOperatorSet.TupleLastN(hv_Indices1, BTuple.TupleSub(BTuple.TupleLength(hv_Indices1), 4), hv_Selected)
                HOperatorSet.TupleSort(hv_Selected, hv_Sorted1)
                HOperatorSet.ClearObj(ho_ObjectSelected1)
                HOperatorSet.SelectObj(ho_ContoursSplit, ho_ObjectSelected1, BTuple.TupleAdd(hv_Sorted1, 1))

                HOperatorSet.FitLineContourXld(ho_ObjectSelected1, "tukey", -1, 0, 5, 2, hv_RowBegin, _
                    hv_ColBegin, hv_RowEnd, hv_ColEnd, hv_Nr, hv_Nc, hv_Dist)
                HOperatorSet.IntersectionLl(hv_RowBegin, hv_ColBegin, hv_RowEnd, hv_ColEnd, BTuple.TupleConcat( _
                    BTuple.TupleSelectRange(hv_RowBegin, 1, 3), BTuple.TupleSelect(hv_RowBegin, _
                    0)), BTuple.TupleConcat(BTuple.TupleSelectRange(hv_ColBegin, 1, 3), BTuple.TupleSelect( _
                    hv_ColBegin, 0)), BTuple.TupleConcat(BTuple.TupleSelectRange(hv_RowEnd, 1, _
                    3), BTuple.TupleSelect(hv_RowEnd, 0)), BTuple.TupleConcat(BTuple.TupleSelectRange( _
                    hv_ColEnd, 1, 3), BTuple.TupleSelect(hv_ColEnd, 0)), hv_Rows, hv_Columns, _
                    hv_IsParallel)
                'tuple_first_n (Rows, |Rows|-2, Rows)
                'tuple_first_n (Columns, |Columns|-2, Columns)

                '頂点１を抽出
                'まず、頂点付近の孔を調べる。
                HOperatorSet.ClearObj(ho_RegionIntersection1)
                HOperatorSet.Intersection(ho_Waku, ho_FillWaku, ho_RegionIntersection1)
                HOperatorSet.ClearObj(ho_RegionDifference)
                HOperatorSet.Difference(ho_FillWaku, ho_Waku, ho_RegionDifference)
                HOperatorSet.ClearObj(ho_ConnectedRegions4)
                HOperatorSet.Connection(ho_RegionDifference, ho_ConnectedRegions4)
                HOperatorSet.AreaCenter(ho_ConnectedRegions4, hv_Area, hv_Row, hv_Column)
                HOperatorSet.TupleSort(hv_Area, hv_Sorted)
                HOperatorSet.TupleSortIndex(hv_Area, hv_SortedAreaIndex)
                HOperatorSet.TupleSelect(hv_Row, BTuple.TupleSelect(hv_SortedAreaIndex, 0), hv_AnaRow)
                HOperatorSet.TupleSelect(hv_Column, BTuple.TupleSelect(hv_SortedAreaIndex, 0), hv_AnaColumn)

                '次に、その孔から一番近い頂点を頂点１とする。
                HOperatorSet.TupleSub(hv_Rows, hv_AnaRow, hv_DiffR)
                HOperatorSet.TupleSub(hv_Columns, hv_AnaColumn, hv_DiffC)
                HOperatorSet.TuplePow(hv_DiffR, 2, hv_PowR)
                HOperatorSet.TuplePow(hv_DiffC, 2, hv_PowC)
                HOperatorSet.TupleAdd(hv_PowR, hv_PowC, hv_Sum)
                HOperatorSet.TupleSqrt(hv_Sum, hv_Distance)
                HOperatorSet.TupleSortIndex(hv_Distance, hv_Indices)
                hv_tempIndex = BTuple.TupleSelect(hv_Indices, 0)
                hv_C0_Row = BTuple.TupleSelect(hv_Rows, hv_tempIndex)
                hv_C0_Col = BTuple.TupleSelect(hv_Columns, hv_tempIndex)
                'その次に、残りの３頂点を特定する。
                hv_C1_Row = 0
                hv_C1_Col = 0
                hv_C2_Row = 0
                hv_C2_Col = 0
                hv_C3_Row = 0
                hv_C3_Col = 0
                CTemp(SP_C) = hv_Rows
                SP_C = SP_C + 1
                HOperatorSet.TupleRemove(CTemp(SP_C - 1), hv_tempIndex, hv_Rows)
                SP_C = 0
                CTemp(SP_C) = hv_Columns
                SP_C = SP_C + 1
                HOperatorSet.TupleRemove(CTemp(SP_C - 1), hv_tempIndex, hv_Columns)
                SP_C = 0
                hv_blnCheck = -1
                hv_Vec1_R = BTuple.TupleSub(BTuple.TupleSelect(hv_Rows, 0), hv_C0_Row)
                hv_Vec1_C = BTuple.TupleSub(BTuple.TupleSelect(hv_Columns, 0), hv_C0_Col)
                hv_Vec2_R = BTuple.TupleSub(BTuple.TupleSelect(hv_Rows, 1), hv_C0_Row)
                hv_Vec2_C = BTuple.TupleSub(BTuple.TupleSelect(hv_Columns, 1), hv_C0_Col)
                hv_Vec3_R = BTuple.TupleSub(BTuple.TupleSelect(hv_Rows, 2), hv_C0_Row)
                hv_Vec3_C = BTuple.TupleSub(BTuple.TupleSelect(hv_Columns, 2), hv_C0_Col)
                CalcGaiseki(hv_Vec1_R, hv_Vec1_C, hv_Vec2_R, hv_Vec2_C, hv_Gaiseki1)
                CalcGaiseki(hv_Vec1_R, hv_Vec1_C, hv_Vec3_R, hv_Vec3_C, hv_Gaiseki2)
                hv_Result = BTuple.TupleMult(hv_Gaiseki1, hv_Gaiseki2)
                If BTuple.TupleGreater(hv_Result, 0) = 1 Then
                    CalcGaiseki(hv_Vec2_R, hv_Vec2_C, hv_Vec1_R, hv_Vec1_C, hv_Gaiseki1)
                    CalcGaiseki(hv_Vec2_R, hv_Vec2_C, hv_Vec3_R, hv_Vec3_C, hv_Gaiseki2)
                    hv_Result = BTuple.TupleMult(hv_Gaiseki1, hv_Gaiseki2)
                    If BTuple.TupleGreater(hv_Result, 0) = 1 Then
                        CalcGaiseki(hv_Vec3_R, hv_Vec3_C, hv_Vec1_R, hv_Vec1_C, hv_Gaiseki1)
                        CalcGaiseki(hv_Vec3_R, hv_Vec3_C, hv_Vec2_R, hv_Vec2_C, hv_Gaiseki2)
                        hv_I1 = 2
                        If BTuple.TupleLess(hv_Gaiseki1, 0) = 1 Then
                            hv_I2 = 0
                            hv_I3 = 1
                        Else
                            hv_I2 = 1
                            hv_I3 = 0
                        End If
                    Else
                        hv_I1 = 1
                        If BTuple.TupleLess(hv_Gaiseki1, 0) = 1 Then
                            hv_I2 = 0
                            hv_I3 = 2
                        Else
                            hv_I2 = 2
                            hv_I3 = 0
                        End If
                    End If
                Else
                    hv_I1 = 0
                    If BTuple.TupleLess(hv_Gaiseki1, 0) = 1 Then
                        hv_I2 = 1
                        hv_I3 = 2
                    Else
                        hv_I2 = 2
                        hv_I3 = 1
                    End If
                End If
                hv_C1_Row = BTuple.TupleSelect(hv_Rows, hv_I1)
                hv_C1_Col = BTuple.TupleSelect(hv_Columns, hv_I1)
                hv_C2_Row = BTuple.TupleSelect(hv_Rows, hv_I2)
                hv_C2_Col = BTuple.TupleSelect(hv_Columns, hv_I2)
                hv_C3_Row = BTuple.TupleSelect(hv_Rows, hv_I3)
                hv_C3_Col = BTuple.TupleSelect(hv_Columns, hv_I3)
                hv_OutRow = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(hv_C0_Row, _
                    hv_C1_Row), hv_C2_Row), hv_C3_Row)
                hv_OutCol = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(hv_C0_Col, _
                    hv_C1_Col), hv_C2_Col), hv_C3_Col)

                '枠の4頂点で射影変換
                hv_SRow = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(500, 950), _
                    500), 950)
                hv_SCol = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(500, 950), _
                    950), 500)
                HOperatorSet.VectorToProjHomMat2d(hv_OutRow, hv_OutCol, hv_SRow, hv_SCol, "gold_standard", _
                    Nothing, Nothing, Nothing, Nothing, _
                    Nothing, Nothing, hv_HomMat2D, hv_Covariance)
                'projective_trans_region (ConnectedRegions3, TransRegions, HomMat2D, 'bilinear')
                '枠内の4点を抽出
                HOperatorSet.ClearObj(ho_RegionDifference1)
                HOperatorSet.Difference(ho_ConnectedRegions3, ho_Waku, ho_RegionDifference1)
                HOperatorSet.AreaCenter(ho_ConnectedRegions3, hv_Area, hv_Row, hv_Column)
                HOperatorSet.TupleMean(hv_Area, hv_MeanArea)
                HOperatorSet.ClearObj(ho_InTarget)
                HOperatorSet.SelectShape(ho_ConnectedRegions3, ho_InTarget, "area", "and", 8, hv_MeanArea)

                'Rep By Suuri 20150309 Sta --------------CT代表点を近傍STに----- 
                'Get2DCoord(ho_ImageChannel1, ho_InTarget, hv_InRow, hv_InColumn)
                Get2DCoord(ho_ImageChannel1, ho_InTarget, hv_InRow, hv_InColumn, hv_AreaST)
                'Rep By Suuri 20150309 End --------------CT代表点を近傍STに-----

                'HOperatorSet.ClearObj(ho_tempObj)
                'HOperatorSet.DilationCircle(ho_InTarget, ho_tempObj, 1.5)
                'HOperatorSet.AreaCenterGray(ho_tempObj, ho_ImageChannel1, hv_InArea, hv_InRow, hv_InColumn)
                If BTuple.TupleLength(hv_InRow) <> 4 Then
                    Continue For
                End If
                'HOperatorSet.AreaCenter(ho_InTarget, hv_InArea, hv_InRow, hv_InColumn)

                '枠内の４点の配置を調べる
                HOperatorSet.ProjectiveTransPixel(hv_HomMat2D, hv_InRow, hv_InColumn, hv_RowTrans, _
                    hv_ColTrans)
                CTemp(SP_C) = hv_RowTrans
                SP_C = SP_C + 1
                HOperatorSet.TupleSub(CTemp(SP_C - 1), 625, hv_RowTrans)
                SP_C = 0
                CTemp(SP_C) = hv_ColTrans
                SP_C = SP_C + 1
                HOperatorSet.TupleSub(CTemp(SP_C - 1), 625, hv_ColTrans)
                SP_C = 0
                CTemp(SP_C) = hv_RowTrans
                SP_C = SP_C + 1
                HOperatorSet.TupleDiv(CTemp(SP_C - 1), 50, hv_RowTrans)
                SP_C = 0
                CTemp(SP_C) = hv_ColTrans
                SP_C = SP_C + 1
                HOperatorSet.TupleDiv(CTemp(SP_C - 1), 50, hv_ColTrans)
                SP_C = 0
                HOperatorSet.TupleRound(hv_RowTrans, hv_RowIndex)
                HOperatorSet.TupleRound(hv_ColTrans, hv_ColIndex)
                CTemp(SP_C) = hv_RowIndex
                SP_C = SP_C + 1
                HOperatorSet.TupleAdd(CTemp(SP_C - 1), 1, hv_RowIndex)
                SP_C = 0
                CTemp(SP_C) = hv_ColIndex
                SP_C = SP_C + 1
                HOperatorSet.TupleAdd(CTemp(SP_C - 1), 1, hv_ColIndex)
                SP_C = 0
                hv_CT_Number = BTuple.TupleAdd(BTuple.TupleMult(5, BTuple.TupleSub(hv_RowIndex, _
                    1)), hv_ColIndex)
                HOperatorSet.TupleSortIndex(hv_CT_Number, hv_CT_NumberIndex)

                CTemp(SP_C) = hv_CT_Number
                SP_C = SP_C + 1
                HOperatorSet.TupleSort(CTemp(SP_C - 1), hv_CT_Number)
                SP_C = 0
                hv_strCT_Number = BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleAdd( _
                    BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleSelect(hv_CT_Number, 0), "_"), _
                    BTuple.TupleSelect(hv_CT_Number, 1)), "_"), BTuple.TupleSelect(hv_CT_Number, _
                    2)), "_"), BTuple.TupleSelect(hv_CT_Number, 3))
                '配置によって、CTの番号を特定する。
                HOperatorSet.TupleFind(hv_CT_IDs, hv_strCT_Number, hv_CT_ID_Index)
                If hv_CT_ID_Index = -1 Then
                    ' Stop
                    Continue For
                End If
                hv_CT_ID = BTuple.TupleAdd(hv_CT_ID_Index, 1)
                If hv_CT_ID = 47 Then
                    ' Continue For
                End If
                hv_CTRow = BTuple.TupleSelect(hv_InRow, hv_CT_NumberIndex)
                hv_CTCol = BTuple.TupleSelect(hv_InColumn, hv_CT_NumberIndex)

                Dim NewCT As New CodedTarget
                NewCT.CT_ID = CInt(hv_CT_ID)
                NewCT.ImageID = ImageID
                NewCT.CT_Points.Row = hv_CTRow
                NewCT.CT_Points.Col = hv_CTCol
                NewCT.SetlstCTtoST()
                lstCT.Add(NewCT)
                All2D.Row = BTuple.TupleConcat(All2D.Row, NewCT.CT_Points.Row)
                All2D.Col = BTuple.TupleConcat(All2D.Col, NewCT.CT_Points.Col)

                HOperatorSet.CopyObj(ho_CT_Regions, OTemp(SP_O), 1, -1)
                SP_O = SP_O + 1
                HOperatorSet.ClearObj(ho_CT_Regions)
                HOperatorSet.ConcatObj(OTemp(SP_O - 1), ho_ObjectSelected, ho_CT_Regions)
                HOperatorSet.ClearObj(OTemp(SP_O - 1))
                SP_O = 0

            End If

        Next
        HOperatorSet.CountObj(ho_CT_Regions, hv_ST_num)
        HOperatorSet.CopyObj(ho_CT_Regions, OTemp(SP_O), 1, -1)

        SP_O = SP_O + 1
        HOperatorSet.ClearObj(ho_CT_Regions)
        HOperatorSet.FillUp(OTemp(SP_O - 1), ho_CT_Regions)
        HOperatorSet.ClearObj(OTemp(SP_O - 1))
        SP_O = 0

        'HOperatorSet.ClearObj(ho_Region)
        'HOperatorSet.Threshold(ho_Image, ho_Region, T_Threshold, 255)
        'HOperatorSet.ClearObj(ho_ConnectedRegions)
        'HOperatorSet.Connection(ho_Region, ho_ConnectedRegions)

        HOperatorSet.ClearObj(ho_Difference)
        HOperatorSet.Difference(ho_ConnectedRegions, ho_CT_Regions, ho_Difference)
        HOperatorSet.ClearObj(ho_SingleTargetRegion)
        HOperatorSet.SelectShape(ho_Difference, ho_SingleTargetRegion, _
                       BTuple.TupleConcat(BTuple.TupleConcat( _
                        "holes_num", "circularity"), "area"), _
                        "and", _
                        BTuple.TupleConcat(BTuple.TupleConcat( _
                        0, 0.3), 10), _
                        BTuple.TupleConcat(BTuple.TupleConcat( _
                        0, 1), 999999))

        'VBMコードターゲットを除外する
#If 1 Then
        Dim ho_RegionDilation As HObject = Nothing
        Dim ho_RegionUnion As HObject = Nothing
        Dim ho_ConnectedRegions5 As HObject = Nothing
        Dim ho_RegionIntersection2 As HObject = Nothing
        HOperatorSet.GenEmptyObj(ho_RegionDilation)
        HOperatorSet.ClearObj(ho_RegionDilation)
        HOperatorSet.GenEmptyObj(ho_RegionUnion)
        HOperatorSet.ClearObj(ho_RegionUnion)
        HOperatorSet.GenEmptyObj(ho_ConnectedRegions5)
        HOperatorSet.ClearObj(ho_ConnectedRegions5)
        HOperatorSet.GenEmptyObj(ho_RegionIntersection2)
        HOperatorSet.ClearObj(ho_RegionIntersection2)

        HOperatorSet.DilationSeq(ho_SingleTargetRegion, ho_RegionDilation, "c", 3)
        HOperatorSet.Union1(ho_RegionDilation, ho_RegionUnion)
        HOperatorSet.Connection(ho_RegionUnion, ho_ConnectedRegions5)
        HOperatorSet.Intersection(ho_ConnectedRegions5, ho_SingleTargetRegion, ho_RegionIntersection2)
        HOperatorSet.SelectShape(ho_RegionIntersection2, ho_SingleTargetRegion, "connect_num", "and", 1, 1)
#End If
        '   HOperatorSet.CountObj(ho_SingleTargetRegion, hv_ST_num)
        '  HOperatorSet.AreaCenter(ho_SingleTargetRegion, hv_AreaST, hv_ST_Rows, hv_ST_Cols)
        ' HOperatorSet.ClearObj(ho_tempObj)
        '  HOperatorSet.DilationCircle(ho_SingleTargetRegion, ho_tempObj, 1.5)
        ' HOperatorSet.AreaCenterGray(ho_tempObj, ho_ImageChannel1, hv_AreaST, hv_ST_Rows, hv_ST_Cols)

        'Rep By Suuri 20150309 Sta --------------CT代表点を近傍STに----- 
        'Get2DCoord(ho_ImageChannel1, ho_SingleTargetRegion, hv_ST_Rows, hv_ST_Cols)
        Get2DCoord(ho_ImageChannel1, ho_SingleTargetRegion, hv_ST_Rows, hv_ST_Cols, hv_AreaST)
        'Rep By Suuri 20150309 End --------------CT代表点を近傍STに----- 

        hv_ST_num = BTuple.TupleLength(hv_ST_Rows)
        '  HOperatorSet.AreaCenter(ho_SingleTargetRegion, hv_AreaST, hv_ST_Rows, hv_ST_Cols)
        If lstST Is Nothing Then
            lstST = New List(Of SingleTarget)
        Else
            lstST.Clear()
        End If
        All2D.Row = BTuple.TupleConcat(All2D.Row, hv_ST_Rows)
        All2D.Col = BTuple.TupleConcat(All2D.Col, hv_ST_Cols)
        For hv_ind = 0 To BTuple.TupleSub(hv_ST_num, 1) Step 1
            HOperatorSet.TupleSelect(hv_ST_Rows, hv_ind, hv_ST_Row)
            HOperatorSet.TupleSelect(hv_ST_Cols, hv_ind, hv_ST_Col)
            Dim NewST As New SingleTarget
            NewST.P2D.Row = hv_ST_Row
            NewST.P2D.Col = hv_ST_Col
            NewST.P2ID = hv_ind + 1
            NewST.ImageID = ImageID
            NewST.flgUsed = 0
            lstST.Add(NewST)
        Next
        All2D_ST.Row = hv_ST_Rows
        All2D_ST.Col = hv_ST_Cols

        HOperatorSet.ClearObj(ho_Region)
        HOperatorSet.ClearObj(ho_ConnectedRegions)
        HOperatorSet.ClearObj(ho_SelectedRegions)
        HOperatorSet.ClearObj(ho_RegionFillUp)
        HOperatorSet.ClearObj(ho_RegionIntersection)
        HOperatorSet.ClearObj(ho_SelectedRegions2)
        HOperatorSet.ClearObj(ho_ConnectedRegions1)
        HOperatorSet.ClearObj(ho_ConnectedRegions2)
        HOperatorSet.ClearObj(ho_ObjectSelected)
        HOperatorSet.ClearObj(ho_ConnectedRegions3)
        HOperatorSet.ClearObj(ho_Waku)
        HOperatorSet.ClearObj(ho_FillWaku)
        HOperatorSet.ClearObj(ho_Contour)
        HOperatorSet.ClearObj(ho_RegressContours)
        HOperatorSet.ClearObj(ho_UnionContours)
        HOperatorSet.ClearObj(ho_ContoursSplit)
        HOperatorSet.ClearObj(ho_RegionIntersection1)
        HOperatorSet.ClearObj(ho_RegionDifference)
        HOperatorSet.ClearObj(ho_ConnectedRegions4)
        HOperatorSet.ClearObj(ho_RegionDifference1)
        HOperatorSet.ClearObj(ho_InTarget)
        HOperatorSet.ClearObj(ho_CT_Regions)
        HOperatorSet.ClearObj(ho_SelectedRegionsT)
        HOperatorSet.ClearObj(ho_SingleTargetRegion)
        HOperatorSet.ClearObj(ho_Difference)
        HOperatorSet.ClearObj(ho_tempObj)
    End Sub

    ''' 

    ''' ST，CTの抽出（枠の外枠の4頂点ではなく内枠の4頂点を抽出するように変更）
    ''' 

    ''' Main procedure 
    ''' 枠の外枠の4頂点ではなく内枠の4頂点を抽出するように変更　20120404
    Public Sub DetectTargetsOther(ByVal ImageID As Integer,
                                  ByVal ho_Image As HObject,
                                  ByVal hv_CT_IDs As Object,
                                  ByVal T_Threshold As Integer,
                                  ByVal CTargetType As Integer)
        Try


            ' Stack for temporary control variables 
            Dim CTemp(10) As Object
            Dim SP_C As Integer
            SP_C = 0

            ' Stack for temporary objects 
            Dim OTemp(10) As HObject
            Dim SP_O As Integer
            SP_O = 0

            'test kiryu マルチスレッド化
            'HOperatorSet.SetSystem("parallelize_operators", True)
            'HOperatorSet.GetSystem("thread_num", 8)

            ' Local iconic variables 
            Dim ho_Region As HObject = Nothing, ho_ConnectedRegions As HObject = Nothing
            Dim ho_SelectedRegions As HObject = Nothing
            Dim ho_RegionFillUp As HObject = Nothing, ho_RegionIntersection As HObject = Nothing
            Dim ho_SelectedRegions2 As HObject = Nothing
            Dim ho_ConnectedRegions1 As HObject = Nothing
            Dim ho_ConnectedRegions2 As HObject = Nothing
            Dim ho_ObjectSelected As HObject = Nothing
            Dim ho_ConnectedRegions3 As HObject = Nothing
            Dim ho_Waku As HObject = Nothing, ho_FillWaku As HObject = Nothing
            Dim ho_Contour As HObject = Nothing, ho_RegressContours As HObject = Nothing
            Dim ho_UnionContours As HObject = Nothing, ho_ContoursSplit As HObject = Nothing
            Dim ho_RegionIntersection1 As HObject = Nothing
            Dim ho_RegionDifference As HObject = Nothing
            Dim ho_ConnectedRegions4 As HObject = Nothing
            Dim ho_TransRegions As HObject = Nothing
            Dim ho_RegionDifference1 As HObject = Nothing
            Dim ho_InTarget As HObject = Nothing
            Dim ho_ObjectSelected1 As HObject = Nothing
            Dim ho_SelectedRegionsT As HObject = Nothing
            Dim ho_SingleTargetRegion As HObject = Nothing
            Dim ho_Difference As HObject = Nothing
            Dim ho_CT_Regions As HObject = Nothing
            Dim ho_tempObj As HObject = Nothing
            Dim ho_ImageChannel1 As HObject = Nothing
            Dim ho_ContourT As HObject = Nothing

            ' Local control variables 
            Dim hv_WindowHandle As Object = Nothing
            Dim hv_Number As Object = Nothing, hv_Index As Object = Nothing
            Dim hv_objNum As Object = Nothing, hv_Rows As Object = Nothing
            Dim hv_Columns As Object = Nothing, hv_RowBegin As Object = Nothing
            Dim hv_ColBegin As Object = Nothing, hv_RowEnd As Object = Nothing
            Dim hv_ColEnd As Object = Nothing, hv_Nr As Object = Nothing
            Dim hv_Nc As Object = Nothing, hv_Dist As Object = Nothing
            Dim hv_IsParallel As Object = Nothing, hv_Area As Object = Nothing
            Dim hv_Row As Object = Nothing, hv_Column As Object = Nothing
            Dim hv_Sorted As Object = Nothing, hv_SortedAreaIndex As Object = Nothing
            Dim hv_AnaRow As Object = Nothing, hv_AnaColumn As Object = Nothing
            Dim hv_DiffR As Object = Nothing, hv_DiffC As Object = Nothing
            Dim hv_PowR As Object = Nothing, hv_PowC As Object = Nothing
            Dim hv_Sum As Object = Nothing, hv_Distance As Object = Nothing
            Dim hv_Indices As Object = Nothing, hv_tempIndex As Object = Nothing
            Dim hv_C0_Row As Object = Nothing, hv_C0_Col As Object = Nothing
            Dim hv_C1_Row As Object = Nothing, hv_C1_Col As Object = Nothing
            Dim hv_C2_Row As Object = Nothing, hv_C2_Col As Object = Nothing
            Dim hv_C3_Row As Object = Nothing, hv_C3_Col As Object = Nothing
            Dim hv_blnCheck As Object = Nothing, hv_Vec1_R As Object = Nothing
            Dim hv_Vec1_C As Object = Nothing, hv_Vec2_R As Object = Nothing
            Dim hv_Vec2_C As Object = Nothing, hv_Vec3_R As Object = Nothing
            Dim hv_Vec3_C As Object = Nothing, hv_Gaiseki1 As Object = Nothing
            Dim hv_Gaiseki2 As Object = Nothing, hv_Result As Object = Nothing
            Dim hv_I1 As Object = Nothing, hv_I2 As Object = Nothing
            Dim hv_I3 As Object = Nothing, hv_OutRow As Object = Nothing
            Dim hv_OutCol As Object = Nothing, hv_SRow As Object = Nothing
            Dim hv_SCol As Object = Nothing, hv_HomMat2D As Object = Nothing
            Dim hv_Covariance As Object = Nothing, hv_MeanArea As Object = Nothing
            Dim hv_InArea As Object = Nothing, hv_InRow As Object = Nothing
            Dim hv_InColumn As Object = Nothing, hv_RowTrans As Object = Nothing
            Dim hv_ColTrans As Object = Nothing, hv_RowIndex As Object = Nothing
            Dim hv_ColIndex As Object = Nothing, hv_CT_Number As Object = Nothing
            Dim hv_strCT_Number As Object = Nothing, hv_CT_ID_Index As Object = Nothing
            Dim hv_CT_ID As Object = Nothing, hv_Length As Object = Nothing
            Dim hv_Indices1 As Object = Nothing, hv_Selected As Object = Nothing
            Dim hv_Sorted1 As Object = Nothing, hv_CT_NumberIndex As Object = Nothing
            Dim hv_CTRow As Object = Nothing, hv_CTCol As Object = Nothing
            Dim hv_ST_num As Object = Nothing, hv_AreaST As Object = Nothing
            Dim hv_ind As Object = Nothing, hv_ST_Rows As Object = Nothing
            Dim hv_ST_Cols As Object = Nothing, hv_ST_Row As Object = Nothing
            Dim hv_ST_Col As Object = Nothing
            Dim hv_Length1 As Object = Nothing
            Dim hv_Indices2 As Object = Nothing
            Dim hv_Inverted As Object = Nothing
            Dim hv_Selected1 As Object = Nothing
            Dim hv_Sum1 As Object = Nothing
            Dim hv_sys As Object = Nothing

            HOperatorSet.GenEmptyObj(ho_Region)
            HOperatorSet.GenEmptyObj(ho_ConnectedRegions)
            HOperatorSet.GenEmptyObj(ho_SelectedRegions)
            HOperatorSet.GenEmptyObj(ho_RegionFillUp)
            HOperatorSet.GenEmptyObj(ho_RegionIntersection)
            HOperatorSet.GenEmptyObj(ho_SelectedRegions2)
            HOperatorSet.GenEmptyObj(ho_ConnectedRegions1)
            HOperatorSet.GenEmptyObj(ho_ConnectedRegions2)
            HOperatorSet.GenEmptyObj(ho_ObjectSelected)
            HOperatorSet.GenEmptyObj(ho_ConnectedRegions3)
            HOperatorSet.GenEmptyObj(ho_Waku)
            HOperatorSet.GenEmptyObj(ho_FillWaku)
            HOperatorSet.GenEmptyObj(ho_Contour)
            HOperatorSet.GenEmptyObj(ho_RegressContours)
            HOperatorSet.GenEmptyObj(ho_UnionContours)
            HOperatorSet.GenEmptyObj(ho_ContoursSplit)
            HOperatorSet.GenEmptyObj(ho_RegionIntersection1)
            HOperatorSet.GenEmptyObj(ho_RegionDifference)
            HOperatorSet.GenEmptyObj(ho_ConnectedRegions4)
            HOperatorSet.GenEmptyObj(ho_RegionDifference1)
            HOperatorSet.GenEmptyObj(ho_InTarget)
            HOperatorSet.GenEmptyObj(ho_ObjectSelected1)

            HOperatorSet.GenEmptyObj(ho_CT_Regions)
            HOperatorSet.GenEmptyObj(ho_SelectedRegionsT)
            HOperatorSet.GenEmptyObj(ho_SingleTargetRegion)
            HOperatorSet.GenEmptyObj(ho_Difference)
            HOperatorSet.GenEmptyObj(ho_tempObj)

            HOperatorSet.GenEmptyObj(ho_ImageChannel1)

            HOperatorSet.GenEmptyObj(ho_ContourT)



            All2D.Row = Nothing
            All2D.Col = Nothing

            If lstCT Is Nothing Then
                lstCT = New List(Of CodedTarget)
            Else
                lstCT.Clear()
            End If
            If strErrorMessageTxt Is Nothing Then
                strErrorMessageTxt = New List(Of String)
            Else
                strErrorMessageTxt.Clear()
            End If
            'mean_image (Image, Image, 9, 9)

            ' HOperatorSet.Threshold(ho_Image, ho_Region, T_Threshold, 255)

            'HOperatorSet.VarThreshold(ho_Image, ho_Region, 10, 10, 0.2, T_Threshold, "light")
            'HOperatorSet.ClearObj(ho_tempObj)
            'HOperatorSet.BinThreshold(ho_Image, ho_tempObj)
            'HOperatorSet.ClearObj(ho_Region)
            'HOperatorSet.Complement(ho_tempObj, ho_Region)
            ClearHObject(ho_Region)
            ClearHObject(ho_ImageChannel1)
            GetFirstRegionG(ho_Image, ho_Region, ho_ImageChannel1, T_Threshold) '
            'HOperatorSet.Threshold(ho_Image, ho_Region, 100, 255)
            ClearHObject(ho_ConnectedRegions)
            HOperatorSet.Connection(ho_Region, ho_ConnectedRegions)
            ClearHObject(ho_SelectedRegions)
            HOperatorSet.SelectShape(ho_ConnectedRegions, ho_SelectedRegions, "holes_num", "and", 2, 2)
            ClearHObject(ho_RegionFillUp)
            HOperatorSet.FillUp(ho_SelectedRegions, ho_RegionFillUp)
            ClearHObject(ho_RegionIntersection)
            HOperatorSet.Intersection(ho_RegionFillUp, ho_ConnectedRegions, ho_RegionIntersection)

            HOperatorSet.GetSystem("parallelize_operators", hv_sys)
            HOperatorSet.GetSystem("tsp_used_thread_num", hv_sys)
            HOperatorSet.GetSystem("tsp_used_split_levels", hv_sys)

            ClearHObject(ho_SelectedRegions2)
            HOperatorSet.SelectShape(ho_RegionIntersection, ho_SelectedRegions2, "connect_num", "and", 5, 5)
            ClearHObject(ho_RegionFillUp)
            HOperatorSet.FillUp(ho_SelectedRegions2, ho_RegionFillUp)
            ClearHObject(ho_ConnectedRegions1)
            HOperatorSet.Connection(ho_RegionFillUp, ho_ConnectedRegions1)
            ClearHObject(ho_ConnectedRegions2)
            HOperatorSet.Connection(ho_SelectedRegions2, ho_ConnectedRegions2)
            HOperatorSet.CountObj(ho_RegionFillUp, hv_Number)
            ClearHObject(ho_CT_Regions)
            HOperatorSet.GenEmptyObj(ho_CT_Regions)

            For hv_Index = 1 To hv_Number.I Step 1
                Try
                    ClearHObject(ho_ObjectSelected)
                    HOperatorSet.SelectObj(ho_SelectedRegions2, ho_ObjectSelected, New HTuple(hv_Index))

                    ClearHObject(ho_ConnectedRegions3)
                    HOperatorSet.Connection(ho_ObjectSelected, ho_ConnectedRegions3)
                    HOperatorSet.CountObj(ho_ConnectedRegions3, hv_objNum)

                    If BTuple.TupleEqual(hv_objNum, 5).I = 1 Then
                        ClearHObject(ho_Waku)
                        HOperatorSet.SelectShape(ho_ConnectedRegions3, ho_Waku, "holes_num", "and", 2, 2)
                        ClearHObject(ho_FillWaku)
                        HOperatorSet.FillUp(ho_Waku, ho_FillWaku)
                        '枠の４頂点を抽出
                        '枠の外枠の4頂点ではなく内枠の4頂点を抽出するように変更　20120404
                        ClearHObject(ho_ContourT)
                        HOperatorSet.GenContourRegionXld(ho_Waku, ho_ContourT, "border_holes")
                        HOperatorSet.LengthXld(ho_ContourT, hv_Length1)
                        HOperatorSet.TupleSortIndex(hv_Length1, hv_Indices2)
                        HOperatorSet.TupleInverse(hv_Indices2, hv_Inverted)
                        HOperatorSet.TupleSelect(hv_Inverted, 1, hv_Selected1)
                        HOperatorSet.TupleAdd(hv_Selected1, 1, hv_Sum1)
                        ClearHObject(ho_Contour)
                        HOperatorSet.SelectObj(ho_ContourT, ho_Contour, hv_Sum1)

                        'get_region_polygon (FillWaku, 5, Rows, Columns)
                        'gen_contour_polygon_xld (Contour, Rows, Columns)
                        '必ず四角を見つけること！！！！！！！！
                        'まだまだ
                        ClearHObject(ho_RegressContours)
                        HOperatorSet.RegressContoursXld(ho_Contour, ho_RegressContours, "median", 1)
                        ClearHObject(ho_UnionContours)
                        HOperatorSet.UnionCollinearContoursXld(ho_RegressContours, ho_UnionContours, 10, 2, 1, 0.1, "attr_keep")
                        ClearHObject(ho_ContoursSplit)
                        HOperatorSet.SegmentContoursXld(ho_UnionContours, ho_ContoursSplit, "lines", 5, 4, 2)
                        HOperatorSet.LengthXld(ho_ContoursSplit, hv_Length)
                        HOperatorSet.TupleSortIndex(hv_Length, hv_Indices1)
                        HOperatorSet.TupleLastN(hv_Indices1, BTuple.TupleSub(BTuple.TupleLength(hv_Indices1), 4), hv_Selected)
                        HOperatorSet.TupleSort(hv_Selected, hv_Sorted1)
                        ClearHObject(ho_ObjectSelected1)

                        Dim hv_Selected_L As Object = Nothing
                        hv_Selected_L = BTuple.TupleAdd(hv_Sorted1, 1)


                        Try

                            Dim i As Integer
                            HOperatorSet.GenEmptyObj(ho_ObjectSelected1)
                            For i = 1 To BTuple.TupleLength(hv_Selected_L).I
                                Dim ho_tmpobj As HObject = Nothing
                                HOperatorSet.GenEmptyObj(ho_tmpobj)
                                HOperatorSet.SelectObj(ho_ContoursSplit, ho_tmpobj, BTuple.TupleSelect(hv_Selected_L, i - 1))
                                HOperatorSet.ConcatObj(ho_ObjectSelected1, ho_tmpobj, ho_ObjectSelected1)
                                ClearHObject(ho_tmpobj)
                            Next

                            Dim selectedCount = ho_ObjectSelected1.CountObj()
                            If selectedCount < 4 Then
                                Continue For
                            End If

                        Catch ex As Exception
                            Dim sss As String = ""
                            sss = ex.Message
                            Continue For 'TODO
                        End Try


                        HOperatorSet.FitLineContourXld(ho_ObjectSelected1, "tukey", -1, 0, 5, 2, hv_RowBegin, _
                        hv_ColBegin, hv_RowEnd, hv_ColEnd, hv_Nr, hv_Nc, hv_Dist)
                        HOperatorSet.IntersectionLl(hv_RowBegin, hv_ColBegin, hv_RowEnd, hv_ColEnd, BTuple.TupleConcat( _
                            BTuple.TupleSelectRange(hv_RowBegin, 1, 3), BTuple.TupleSelect(hv_RowBegin, _
                            0)), BTuple.TupleConcat(BTuple.TupleSelectRange(hv_ColBegin, 1, 3), BTuple.TupleSelect( _
                            hv_ColBegin, 0)), BTuple.TupleConcat(BTuple.TupleSelectRange(hv_RowEnd, 1, _
                            3), BTuple.TupleSelect(hv_RowEnd, 0)), BTuple.TupleConcat(BTuple.TupleSelectRange( _
                            hv_ColEnd, 1, 3), BTuple.TupleSelect(hv_ColEnd, 0)), hv_Rows, hv_Columns, _
                            hv_IsParallel)
                        'tuple_first_n (Rows, |Rows|-2, Rows)
                        'tuple_first_n (Columns, |Columns|-2, Columns)

                        '頂点１を抽出
                        'まず、頂点付近の孔を調べる。
                        ClearHObject(ho_RegionIntersection1)
                        HOperatorSet.Intersection(ho_Waku, ho_FillWaku, ho_RegionIntersection1)
                        ClearHObject(ho_RegionDifference)
                        HOperatorSet.Difference(ho_FillWaku, ho_Waku, ho_RegionDifference)
                        ClearHObject(ho_ConnectedRegions4)
                        HOperatorSet.Connection(ho_RegionDifference, ho_ConnectedRegions4)
                        HOperatorSet.AreaCenter(ho_ConnectedRegions4, hv_Area, hv_Row, hv_Column)
                        HOperatorSet.TupleSort(hv_Area, hv_Sorted)
                        HOperatorSet.TupleSortIndex(hv_Area, hv_SortedAreaIndex)
                        HOperatorSet.TupleSelect(hv_Row, BTuple.TupleSelect(hv_SortedAreaIndex, 0), hv_AnaRow)
                        HOperatorSet.TupleSelect(hv_Column, BTuple.TupleSelect(hv_SortedAreaIndex, 0), hv_AnaColumn)

                        '次に、その孔から一番近い頂点を頂点１とする。
                        HOperatorSet.TupleSub(hv_Rows, hv_AnaRow, hv_DiffR)
                        HOperatorSet.TupleSub(hv_Columns, hv_AnaColumn, hv_DiffC)
                        HOperatorSet.TuplePow(hv_DiffR, 2, hv_PowR)
                        HOperatorSet.TuplePow(hv_DiffC, 2, hv_PowC)
                        HOperatorSet.TupleAdd(hv_PowR, hv_PowC, hv_Sum)
                        HOperatorSet.TupleSqrt(hv_Sum, hv_Distance)
                        HOperatorSet.TupleSortIndex(hv_Distance, hv_Indices)
                        hv_tempIndex = BTuple.TupleSelect(hv_Indices, 0)
                        hv_C0_Row = BTuple.TupleSelect(hv_Rows, hv_tempIndex)
                        hv_C0_Col = BTuple.TupleSelect(hv_Columns, hv_tempIndex)
                        'その次に、残りの３頂点を特定する。
                        hv_C1_Row = 0
                        hv_C1_Col = 0
                        hv_C2_Row = 0
                        hv_C2_Col = 0
                        hv_C3_Row = 0
                        hv_C3_Col = 0
                        CTemp(SP_C) = hv_Rows
                        SP_C = SP_C + 1
                        HOperatorSet.TupleRemove(CTemp(SP_C - 1), hv_tempIndex, hv_Rows)
                        SP_C = 0
                        CTemp(SP_C) = hv_Columns
                        SP_C = SP_C + 1
                        HOperatorSet.TupleRemove(CTemp(SP_C - 1), hv_tempIndex, hv_Columns)
                        SP_C = 0
                        hv_blnCheck = -1
                        hv_Vec1_R = BTuple.TupleSub(BTuple.TupleSelect(hv_Rows, 0), hv_C0_Row)
                        hv_Vec1_C = BTuple.TupleSub(BTuple.TupleSelect(hv_Columns, 0), hv_C0_Col)
                        hv_Vec2_R = BTuple.TupleSub(BTuple.TupleSelect(hv_Rows, 1), hv_C0_Row)
                        hv_Vec2_C = BTuple.TupleSub(BTuple.TupleSelect(hv_Columns, 1), hv_C0_Col)
                        hv_Vec3_R = BTuple.TupleSub(BTuple.TupleSelect(hv_Rows, 2), hv_C0_Row)
                        hv_Vec3_C = BTuple.TupleSub(BTuple.TupleSelect(hv_Columns, 2), hv_C0_Col)
                        CalcGaiseki(hv_Vec1_R, hv_Vec1_C, hv_Vec2_R, hv_Vec2_C, hv_Gaiseki1)
                        CalcGaiseki(hv_Vec1_R, hv_Vec1_C, hv_Vec3_R, hv_Vec3_C, hv_Gaiseki2)
                        hv_Result = BTuple.TupleMult(hv_Gaiseki1, hv_Gaiseki2)
                        If BTuple.TupleGreater(hv_Result, 0).I = 1 Then
                            CalcGaiseki(hv_Vec2_R, hv_Vec2_C, hv_Vec1_R, hv_Vec1_C, hv_Gaiseki1)
                            CalcGaiseki(hv_Vec2_R, hv_Vec2_C, hv_Vec3_R, hv_Vec3_C, hv_Gaiseki2)
                            hv_Result = BTuple.TupleMult(hv_Gaiseki1, hv_Gaiseki2)
                            If BTuple.TupleGreater(hv_Result, 0).I = 1 Then
                                CalcGaiseki(hv_Vec3_R, hv_Vec3_C, hv_Vec1_R, hv_Vec1_C, hv_Gaiseki1)
                                CalcGaiseki(hv_Vec3_R, hv_Vec3_C, hv_Vec2_R, hv_Vec2_C, hv_Gaiseki2)
                                hv_I1 = 2
                                If BTuple.TupleLess(hv_Gaiseki1, 0).I = 1 Then
                                    hv_I2 = 0
                                    hv_I3 = 1
                                Else
                                    hv_I2 = 1
                                    hv_I3 = 0
                                End If
                            Else
                                hv_I1 = 1
                                If BTuple.TupleLess(hv_Gaiseki1, 0).I = 1 Then
                                    hv_I2 = 0
                                    hv_I3 = 2
                                Else
                                    hv_I2 = 2
                                    hv_I3 = 0
                                End If
                            End If
                        Else
                            hv_I1 = 0
                            If BTuple.TupleLess(hv_Gaiseki1, 0).I = 1 Then
                                hv_I2 = 1
                                hv_I3 = 2
                            Else
                                hv_I2 = 2
                                hv_I3 = 1
                            End If
                        End If
                        hv_C1_Row = BTuple.TupleSelect(hv_Rows, hv_I1)
                        hv_C1_Col = BTuple.TupleSelect(hv_Columns, hv_I1)
                        hv_C2_Row = BTuple.TupleSelect(hv_Rows, hv_I2)
                        hv_C2_Col = BTuple.TupleSelect(hv_Columns, hv_I2)
                        hv_C3_Row = BTuple.TupleSelect(hv_Rows, hv_I3)
                        hv_C3_Col = BTuple.TupleSelect(hv_Columns, hv_I3)
                        hv_OutRow = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(hv_C0_Row, _
                            hv_C1_Row), hv_C2_Row), hv_C3_Row)
                        hv_OutCol = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(hv_C0_Col, _
                            hv_C1_Col), hv_C2_Col), hv_C3_Col)

                        '枠の4頂点で射影変換
                        hv_SRow = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(550, 900), _
                            550), 900)
                        If CTargetType = 0 Then
                            hv_SCol = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(550, 900), _
                                900), 550)
                        Else
                            hv_SCol = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(550 + 7.855, 900), _
                              900 + 7.855), 550)
                        End If
                        '20160420 不具合修正 SUSANO ADD START
                        'CTフレームを正対させる処理に失敗すればその後の処理を進める必要はない。
                        'つまり、このCTは認識不可とする。
                        Try
                            HOperatorSet.VectorToProjHomMat2d(hv_OutRow, hv_OutCol, hv_SRow, hv_SCol, "gold_standard", _
                         New HTuple, New HTuple, New HTuple, New HTuple, _
                         New HTuple, New HTuple, hv_HomMat2D, hv_Covariance)
                        Catch ex As Exception
                            Continue For
                        End Try
                        '20160420 不具合修正 SUSANO ADD START
                        'projective_trans_region (ConnectedRegions3, TransRegions, HomMat2D, 'bilinear')
                        '枠内の4点を抽出
                        ClearHObject(ho_RegionDifference1)
                        HOperatorSet.Difference(ho_ConnectedRegions3, ho_Waku, ho_RegionDifference1)
                        HOperatorSet.AreaCenter(ho_ConnectedRegions3, hv_Area, hv_Row, hv_Column)
                        HOperatorSet.TupleMean(hv_Area, hv_MeanArea)
                        ClearHObject(ho_InTarget)
                        '面積下限変更　15→9
                        HOperatorSet.SelectShape(ho_ConnectedRegions3, ho_InTarget, "area", "and", 9, hv_MeanArea)

                        'Rep By Suuri 20150309 Sta ------CT代表点を近傍STに--- 
                        'Get2DCoord(ho_ImageChannel1, ho_InTarget, hv_InRow, hv_InColumn)
                        Get2DCoord(ho_ImageChannel1, ho_InTarget, hv_InRow, hv_InColumn, hv_AreaST)
                        'Rep By Suuri 20150309 End ------CT代表点を近傍STに---

                        'ClearHObject(ho_tempObj)
                        'HOperatorSet.DilationCircle(ho_InTarget, ho_tempObj, 1.5)
                        'HOperatorSet.AreaCenterGray(ho_tempObj, ho_ImageChannel1, hv_InArea, hv_InRow, hv_InColumn)
                        If BTuple.TupleLength(hv_InRow).I <> 4 Then
                            Continue For
                        End If
                        'HOperatorSet.AreaCenter(ho_InTarget, hv_InArea, hv_InRow, hv_InColumn)

                        '枠内の４点の配置を調べる
                        HOperatorSet.ProjectiveTransPixel(hv_HomMat2D, hv_InRow, hv_InColumn, hv_RowTrans, _
                            hv_ColTrans)
                        CTemp(SP_C) = hv_RowTrans
                        SP_C = SP_C + 1
                        HOperatorSet.TupleSub(CTemp(SP_C - 1), 625, hv_RowTrans)
                        SP_C = 0
                        CTemp(SP_C) = hv_ColTrans
                        SP_C = SP_C + 1
                        HOperatorSet.TupleSub(CTemp(SP_C - 1), 625, hv_ColTrans)
                        SP_C = 0
                        CTemp(SP_C) = hv_RowTrans
                        SP_C = SP_C + 1
                        HOperatorSet.TupleDiv(CTemp(SP_C - 1), 50, hv_RowTrans)
                        SP_C = 0
                        CTemp(SP_C) = hv_ColTrans
                        SP_C = SP_C + 1
                        HOperatorSet.TupleDiv(CTemp(SP_C - 1), 50, hv_ColTrans)
                        SP_C = 0
                        HOperatorSet.TupleRound(hv_RowTrans, hv_RowIndex)
                        HOperatorSet.TupleRound(hv_ColTrans, hv_ColIndex)
                        CTemp(SP_C) = hv_RowIndex
                        SP_C = SP_C + 1
                        HOperatorSet.TupleAdd(CTemp(SP_C - 1), 1, hv_RowIndex)
                        SP_C = 0
                        CTemp(SP_C) = hv_ColIndex
                        SP_C = SP_C + 1
                        HOperatorSet.TupleAdd(CTemp(SP_C - 1), 1, hv_ColIndex)
                        SP_C = 0
                        hv_CT_Number = BTuple.TupleAdd(BTuple.TupleMult(5, BTuple.TupleSub(hv_RowIndex, _
                            1)), hv_ColIndex)
                        HOperatorSet.TupleSortIndex(hv_CT_Number, hv_CT_NumberIndex)

                        CTemp(SP_C) = hv_CT_Number
                        SP_C = SP_C + 1
                        HOperatorSet.TupleSort(CTemp(SP_C - 1), hv_CT_Number)
                        SP_C = 0
                        hv_strCT_Number = BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleAdd( _
                            BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleSelect(hv_CT_Number, 0), "_"), _
                            BTuple.TupleSelect(hv_CT_Number, 1)), "_"), BTuple.TupleSelect(hv_CT_Number, _
                            2)), "_"), BTuple.TupleSelect(hv_CT_Number, 3))
                        '配置によって、CTの番号を特定する。
                        HOperatorSet.TupleFind(hv_CT_IDs, hv_strCT_Number, hv_CT_ID_Index)
                        If hv_CT_ID_Index.I = -1 Then
                            ' Stop
                            Continue For
                        End If
                        hv_CT_ID = BTuple.TupleAdd(hv_CT_ID_Index, 1)
                        If hv_CT_ID.I = 47 Then
                            ' Continue For
                        End If
                        hv_CTRow = BTuple.TupleSelect(hv_InRow, hv_CT_NumberIndex)
                        hv_CTCol = BTuple.TupleSelect(hv_InColumn, hv_CT_NumberIndex)

                        Dim NewCT As New CodedTarget
                        NewCT.CT_ID = CInt(hv_CT_ID.I)
                        NewCT.ImageID = ImageID
                        NewCT.CT_Points.Row = hv_CTRow
                        NewCT.CT_Points.Col = hv_CTCol
                        NewCT.AllST_Area = BTuple.TupleSum(hv_AreaST).D 'SUURI ADD 20150217 CT代表点をSTに
                        NewCT.SetlstCTtoST()
                        Dim BadCT As Boolean = False
                        For Each CT As CodedTarget In lstCT
#If DEBUG Then
                            'If NewCT.CT_ID = 84 Then
                            '    NewCT.CT_ID = 11
                            'End If
#End If
                            If CT.CT_ID = NewCT.CT_ID Then
                                '  BadCT = True
                                strErrorMessageTxt.Add(CT.CT_ID)
                                Exit For

                            End If
                        Next
                        If BadCT = False Then
                            If NewCT.CT_ID < 500 Then '500番台は追加しない　20151021　ＡＤＤ
                                lstCT.Add(NewCT)
                                All2D.Row = BTuple.TupleConcat(All2D.Row, NewCT.CT_Points.Row)
                                All2D.Col = BTuple.TupleConcat(All2D.Col, NewCT.CT_Points.Col)
                            End If
                        End If

                        HOperatorSet.CopyObj(ho_CT_Regions, OTemp(SP_O), 1, -1)
                        SP_O = SP_O + 1
                        ClearHObject(ho_CT_Regions)
                        HOperatorSet.ConcatObj(OTemp(SP_O - 1), ho_ObjectSelected, ho_CT_Regions)
                        ClearHObject(OTemp(SP_O - 1))
                        SP_O = 0

                    End If
                Catch ex As Exception
                    Dim ss = ""
                End Try
            Next

            HOperatorSet.CountObj(ho_CT_Regions, hv_ST_num)
            HOperatorSet.CopyObj(ho_CT_Regions, OTemp(SP_O), 1, -1)


            SP_O = SP_O + 1
            ClearHObject(ho_CT_Regions)
            HOperatorSet.FillUp(OTemp(SP_O - 1), ho_CT_Regions)
            ClearHObject(OTemp(SP_O - 1))
            SP_O = 0

            'ClearHObject(ho_Region)
            'HOperatorSet.Threshold(ho_Image, ho_Region, T_Threshold, 255)
            'ClearHObject(ho_ConnectedRegions)
            'HOperatorSet.Connection(ho_Region, ho_ConnectedRegions)

            ClearHObject(ho_Difference)
            HOperatorSet.Difference(ho_ConnectedRegions, ho_CT_Regions, ho_Difference)
            ClearHObject(ho_SingleTargetRegion)
            Dim param1 As Object = Nothing
            Dim parammin As Object = Nothing
            Dim parammax As Object = Nothing
            param1 = BTuple.TupleConcat(BTuple.TupleConcat("holes_num", "circularity"), "area")
            parammin = BTuple.TupleConcat(BTuple.TupleConcat(0, 0.3), 10.0)
            parammax = BTuple.TupleConcat(BTuple.TupleConcat(0, 1.0), 999999.0)

            HOperatorSet.SelectShape(ho_Difference, ho_SingleTargetRegion, "holes_num", "and", 0, 0)
            HOperatorSet.SelectShape(ho_SingleTargetRegion, ho_SingleTargetRegion, "circularity", "and", 0.3, 1.0)
            HOperatorSet.SelectShape(ho_SingleTargetRegion, ho_SingleTargetRegion, "area", "and", 10, 999999.0)


            '   HOperatorSet.SelectShape(ho_Difference, ho_SingleTargetRegion, param1, "and", parammin, parammax)

            'HOperatorSet.SelectShape(ho_Difference, ho_SingleTargetRegion, _
            '              BTuple.TupleConcat(BTuple.TupleConcat( _
            '               "holes_num", "circularity"), "area"), _
            '               "and", _
            '               BTuple.TupleConcat(BTuple.TupleConcat( _
            '               0, 0.3), 10), _
            '               BTuple.TupleConcat(BTuple.TupleConcat( _
            '               0, 1.0), 999999))
            'HOperatorSet.SelectShape(ho_Difference, ho_SingleTargetRegion, _
            '             BTuple.TupleConcat("holes_num", "area"), _
            '              "and", BTuple.TupleConcat(0, 10), BTuple.TupleConcat(0, 999999))

            'VBMコードターゲットを除外する
#If 0 Then
            'Dim ho_RegionDilation As HObject = Nothing
            'Dim ho_RegionUnion As HObject = Nothing
            'Dim ho_ConnectedRegions5 As HObject = Nothing
            'Dim ho_RegionIntersection2 As HObject = Nothing
            'Dim ho_NonVBMCTRegion As HObject = Nothing

            'HOperatorSet.GenEmptyObj(ho_RegionDilation)
            'ClearHObject(ho_RegionDilation)
            'HOperatorSet.GenEmptyObj(ho_RegionUnion)
            'ClearHObject(ho_RegionUnion)
            'HOperatorSet.GenEmptyObj(ho_ConnectedRegions5)
            'ClearHObject(ho_ConnectedRegions5)
            'HOperatorSet.GenEmptyObj(ho_RegionIntersection2)
            'ClearHObject(ho_RegionIntersection2)
            'HOperatorSet.GenEmptyObj(ho_NonVBMCTRegion)
            'ClearHObject(ho_NonVBMCTRegion)

            'HOperatorSet.DilationSeq(ho_SingleTargetRegion, ho_RegionDilation, "c", 4)
            'HOperatorSet.Union1(ho_RegionDilation, ho_RegionUnion)
            'HOperatorSet.Connection(ho_RegionUnion, ho_ConnectedRegions5)
            'HOperatorSet.Intersection(ho_ConnectedRegions5, ho_SingleTargetRegion, ho_RegionIntersection2)
            'HOperatorSet.SelectShape(ho_RegionIntersection2, ho_NonVBMCTRegion, "connect_num", "and", 1, 6)
            'HOperatorSet.Connection(ho_NonVBMCTRegion, ho_SingleTargetRegion)
            'ClearHObject(ho_NonVBMCTRegion)
            Dim ho_RegionDilation As HObject = Nothing
            Dim ho_RegionUnion As HObject = Nothing
            Dim ho_ConnectedRegions5 As HObject = Nothing
            Dim ho_RegionIntersection2 As HObject = Nothing
            Dim ho_NonVBMCTRegion As HObject = Nothing
            Dim ho_objFourTenTarget As HObject = Nothing, ho_objConnectionFourTenTarget As HObject = Nothing
            Dim ho_SelectedRegions1 As HObject = Nothing, ho_SelectedRegions3 As HObject = Nothing

            Dim hv_FourTen_num As Object = Nothing, hv_Index1 As Object = Nothing
            Dim hv_Number1 As Object = Nothing


            HOperatorSet.GenEmptyObj(ho_RegionDilation)
            ClearHObject(ho_RegionDilation)
            HOperatorSet.GenEmptyObj(ho_RegionUnion)
            ClearHObject(ho_RegionUnion)
            HOperatorSet.GenEmptyObj(ho_ConnectedRegions5)
            ClearHObject(ho_ConnectedRegions5)
            HOperatorSet.GenEmptyObj(ho_RegionIntersection2)
            ClearHObject(ho_RegionIntersection2)
            HOperatorSet.GenEmptyObj(ho_NonVBMCTRegion)
            ClearHObject(ho_NonVBMCTRegion)

            HOperatorSet.DilationSeq(ho_SingleTargetRegion, ho_RegionDilation, "c", 10)
            HOperatorSet.Union1(ho_RegionDilation, ho_RegionUnion)
            HOperatorSet.Connection(ho_RegionUnion, ho_ConnectedRegions5)
            HOperatorSet.Intersection(ho_ConnectedRegions5, ho_SingleTargetRegion, ho_RegionIntersection2)
            HOperatorSet.SelectShape(ho_RegionIntersection2, ho_NonVBMCTRegion, "connect_num", "and", 4, 4)

            HOperatorSet.CountObj(ho_NonVBMCTRegion, hv_FourTen_num)

            For hv_Index1 = 1 To hv_FourTen_num Step 1

                Call HOperatorSet.SelectObj(ho_NonVBMCTRegion, ho_objFourTenTarget, hv_Index1)
                Call HOperatorSet.Connection(ho_objFourTenTarget, ho_objConnectionFourTenTarget)

                Call HOperatorSet.SelectShape(ho_objConnectionFourTenTarget, ho_SelectedRegions1, "area", _
                    "and", 10, 99999)

                Call HOperatorSet.SelectShape(ho_SelectedRegions1, ho_SelectedRegions3, "circularity", _
                    "and", 0.7, 1)

                Call HOperatorSet.CountObj(ho_SelectedRegions3, hv_Number1)

                If BTuple.TupleEqual(hv_Number1, 4) Then

                    Get2DCoord(ho_ImageChannel1, ho_SelectedRegions3, hv_CTRow, hv_CTCol, hv_AreaST)

                    Dim hvAreaSort As Object = Nothing
                    hvAreaSort = BTuple.TupleSortIndex(hv_AreaST)
                    Dim hvArea012 As Object = Nothing
                    hvArea012 = (hv_AreaST(hvAreaSort(1)) + hv_AreaST(hvAreaSort(2)) + hv_AreaST(hvAreaSort(3))) / 3
                    Dim hvArea3 As Object = Nothing
                    hvArea3 = hv_AreaST(hvAreaSort(0))
                    If hvArea012 * 0.75 > hvArea3 Then


                        Dim R0 As Object = Nothing
                        Dim C0 As Object = Nothing
                        Dim R1 As Object = Nothing
                        Dim C1 As Object = Nothing
                        Dim R2 As Object = Nothing
                        Dim C2 As Object = Nothing
                        Dim R3 As Object = Nothing
                        Dim C3 As Object = Nothing
                        FourTenTarget1234(hv_CTRow, hv_CTCol, hvAreaSort(0), R0, C0, R1, C1, R2, C2, R3, C3)
                        hv_CTRow = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(R0, R1), R2), R3)
                        hv_CTCol = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(C0, C1), C2), C3)

                        hv_OutRow = BTuple.TupleConcat(BTuple.TupleConcat(R0, R1), R2)
                        hv_OutCol = BTuple.TupleConcat(BTuple.TupleConcat(C0, C1), C2)

                        '大3点で射影変換
                        hv_SRow = BTuple.TupleConcat(BTuple.TupleConcat(0, 0), 400)
                        hv_SCol = BTuple.TupleConcat(BTuple.TupleConcat(0, 400), 0)
                        HOperatorSet.VectorToHomMat2d(hv_OutRow, hv_OutCol, hv_SRow, hv_SCol, hv_HomMat2D)

                        HOperatorSet.AffineTransPoint2D(hv_HomMat2D, R3, C3, hv_RowTrans, hv_ColTrans)
                        'R3 = hv_RowTrans(3) - 280
                        'C3 = hv_ColTrans(3) - 280

                        CTemp(SP_C) = hv_RowTrans
                        SP_C = SP_C + 1
                        HOperatorSet.TupleSub(CTemp(SP_C - 1), 280, hv_RowTrans)
                        SP_C = 0
                        CTemp(SP_C) = hv_ColTrans
                        SP_C = SP_C + 1
                        HOperatorSet.TupleSub(CTemp(SP_C - 1), 280, hv_ColTrans)
                        SP_C = 0
                        CTemp(SP_C) = hv_RowTrans
                        SP_C = SP_C + 1
                        HOperatorSet.TupleDiv(CTemp(SP_C - 1), 120, hv_RowTrans)
                        SP_C = 0
                        CTemp(SP_C) = hv_ColTrans
                        SP_C = SP_C + 1
                        HOperatorSet.TupleDiv(CTemp(SP_C - 1), 120, hv_ColTrans)
                        SP_C = 0
                        HOperatorSet.TupleRound(hv_RowTrans, hv_RowIndex)
                        HOperatorSet.TupleRound(hv_ColTrans, hv_ColIndex)
                        CTemp(SP_C) = hv_RowIndex
                        SP_C = SP_C + 1
                        HOperatorSet.TupleAdd(CTemp(SP_C - 1), 1, hv_RowIndex)
                        SP_C = 0
                        CTemp(SP_C) = hv_ColIndex
                        SP_C = SP_C + 1
                        HOperatorSet.TupleAdd(CTemp(SP_C - 1), 1, hv_ColIndex)
                        SP_C = 0
                        If hv_RowIndex = 0 Or hv_ColIndex = 0 Then
                            If hv_RowIndex = 0 Then
                                hv_CT_Number = 17
                            End If
                            If hv_ColIndex = 0 Then
                                hv_CT_Number = 18
                            End If
                        Else
                            hv_CT_Number = BTuple.TupleAdd(BTuple.TupleMult(4, BTuple.TupleSub(hv_RowIndex, _
                          1)), hv_ColIndex)
                        End If

                        HOperatorSet.TupleSortIndex(hv_CT_Number, hv_CT_NumberIndex)

                        CTemp(SP_C) = hv_CT_Number
                        SP_C = SP_C + 1
                        HOperatorSet.TupleSort(CTemp(SP_C - 1), hv_CT_Number)
                        SP_C = 0
                        hv_strCT_Number = hv_CT_Number
                        If hv_CT_Number < 19 And hv_CT_Number > 0 Then
                            hv_strCT_Number = hv_CT_Number

                            Dim NewCT As New CodedTarget
                            NewCT.CT_ID = CInt(hv_CT_Number) + 400
                            NewCT.ImageID = ImageID
                            NewCT.CT_Points.Row = hv_CTRow
                            NewCT.CT_Points.Col = hv_CTCol
                            NewCT.AllST_Area = BTuple.TupleSum(hv_AreaST) 'SUURI ADD 20150217
                            NewCT.SetlstCTtoST()
                            lstCT.Add(NewCT)
                            All2D.Row = BTuple.TupleConcat(All2D.Row, NewCT.CT_Points.Row)
                            All2D.Col = BTuple.TupleConcat(All2D.Col, NewCT.CT_Points.Col)
                        End If
                    End If
                End If

            Next

            ClearHObject(ho_ConnectedRegions5)
            ClearHObject(ho_NonVBMCTRegion)

#End If
            '   HOperatorSet.CountObj(ho_SingleTargetRegion, hv_ST_num)
            '  HOperatorSet.AreaCenter(ho_SingleTargetRegion, hv_AreaST, hv_ST_Rows, hv_ST_Cols)
            ' ClearHObject(ho_tempObj)
            '  HOperatorSet.DilationCircle(ho_SingleTargetRegion, ho_tempObj, 1.5)
            ' HOperatorSet.AreaCenterGray(ho_tempObj, ho_ImageChannel1, hv_AreaST, hv_ST_Rows, hv_ST_Cols)

            'Rep By Suuri 20150309 Sta ------CT代表点を近傍STに---
            'Get2DCoord(ho_ImageChannel1, ho_SingleTargetRegion, hv_ST_Rows, hv_ST_Cols)
            Get2DCoord(ho_ImageChannel1, ho_SingleTargetRegion, hv_ST_Rows, hv_ST_Cols, hv_AreaST)
            'Rep By Suuri 20150309 End ------CT代表点を近傍STに---

            hv_ST_num = BTuple.TupleLength(hv_ST_Rows)
            '  HOperatorSet.AreaCenter(ho_SingleTargetRegion, hv_AreaST, hv_ST_Rows, hv_ST_Cols)
            If lstST Is Nothing Then
                lstST = New List(Of SingleTarget)
            Else
                lstST.Clear()
            End If
            All2D.Row = BTuple.TupleConcat(All2D.Row, hv_ST_Rows)
            All2D.Col = BTuple.TupleConcat(All2D.Col, hv_ST_Cols)
            For hv_ind = 0 To BTuple.TupleSub(hv_ST_num, 1) Step 1
                HOperatorSet.TupleSelect(hv_ST_Rows, hv_ind, hv_ST_Row)
                HOperatorSet.TupleSelect(hv_ST_Cols, hv_ind, hv_ST_Col)
                Dim NewST As New SingleTarget
                NewST.P2D.Row = hv_ST_Row
                NewST.P2D.Col = hv_ST_Col
                NewST.P2ID = hv_ind.I + 1
                NewST.ImageID = ImageID
                NewST.flgUsed = 0
                HOperatorSet.TupleSelect(hv_AreaST, hv_ind, NewST.AreaST)
                '  NewST.AreaST = hv_AreaST(hv_ind) 'SUURI ADD 20150217
                lstST.Add(NewST)
            Next

            'SUURI ADD 20150307	CT代表点を近傍STに
#If True Then
            Dim intKeisuuKyori As Integer
            intKeisuuKyori = GetPrivateProfileInt("Kaiseki", "STtoCT", 0, My.Application.Info.DirectoryPath & "\vform.ini")
            'SUURI UPDATE START 20150322
            GetListSTtoCT(My.Application.Info.DirectoryPath & "\STtoCTlist.csv")
            Dim ii As Integer
            Dim nn As Integer = lstCT.Count

            For ii = nn - 1 To 0 Step -1
                Dim objCT As CodedTarget
                objCT = lstCT.Item(ii)
                'Next
                'For Each objCT As CodedTarget In lstCT
                Dim mindistST As Double = Double.MaxValue
                Dim minST As SingleTarget = Nothing

                '対象のＣＴに一番近いＳＴを取得
                For Each objST As SingleTarget In lstST
                    Dim objKyori As Object = Nothing
                    'ただし、そのＳＴの面積がＣＴの内部点の全面積より大きい場合
                    'つまり、ある程度大きさのあるＳＴを選択する。
                    If objST.AreaST > objCT.AllST_Area Then
                        objCT.lstCTtoST.Item(0).P2D.CalcDistToInputPoint(objST.P2D.Row,
                                                                         objST.P2D.Col,
                                                                         objKyori)
                        If objKyori < mindistST Then
                            mindistST = objKyori
                            minST = objST
                        End If
                    End If
                Next

                '一番近いＳＴがＣＴの内部点間の距離より小さいければＣＴの一番目の点（代表点）に置き換える
                'つまりある程度CTに近いＳＴを選択する。
                If CInt(intKeisuuKyori) > 0 Then
                    'SUURI ADD 20150321
                    Dim flgSyoriOK As Boolean = False
                    For Each objSTtoCT As STtoCT In lstSTtoCT
                        If objCT.CT_ID = objSTtoCT.CTID Then
                            flgSyoriOK = True
                        End If
                    Next
                    If flgSyoriOK = True Then
                        If mindistST < objCT.GetAllPointsKyoriPixel * CInt(intKeisuuKyori) Then
                            objCT.lstCTtoST.Item(0).P2D.CopyToMe(minST.P2D)
                            objCT.CT_Points.Row(0) = minST.P2D.Row
                            objCT.CT_Points.Col(0) = minST.P2D.Col
                            objCT.CenterPoint.CopyToMe(minST.P2D)
                        Else
                            lstCT.RemoveAt(ii)

                        End If
                    End If
                End If
            Next
            'SUURI UPDATE END 20150323
#End If
            'SUURI ADD 20150307 
            All2D_ST.Row = hv_ST_Rows
            All2D_ST.Col = hv_ST_Cols

            ClearHObject(ho_Region)
            ClearHObject(ho_ConnectedRegions)
            ClearHObject(ho_SelectedRegions)
            ClearHObject(ho_RegionFillUp)
            ClearHObject(ho_RegionIntersection)
            ClearHObject(ho_SelectedRegions2)
            ClearHObject(ho_ConnectedRegions1)
            ClearHObject(ho_ConnectedRegions2)
            ClearHObject(ho_ObjectSelected)
            ClearHObject(ho_ConnectedRegions3)
            ClearHObject(ho_Waku)
            ClearHObject(ho_FillWaku)
            ClearHObject(ho_Contour)
            ClearHObject(ho_RegressContours)
            ClearHObject(ho_UnionContours)
            ClearHObject(ho_ContoursSplit)
            ClearHObject(ho_RegionIntersection1)
            ClearHObject(ho_RegionDifference)
            ClearHObject(ho_ConnectedRegions4)
            ClearHObject(ho_RegionDifference1)
            ClearHObject(ho_InTarget)
            ClearHObject(ho_CT_Regions)
            ClearHObject(ho_SelectedRegionsT)

            ClearHObject(ho_SingleTargetRegion)
            ClearHObject(ho_Difference)
            ClearHObject(ho_tempObj)
            ClearHObject(ho_ContourT)
        Catch ex As Exception
            ex.ToString()
            Exit Sub
        End Try
    End Sub

    Public Sub DetectTargetsOther_ver2(ByVal ImageID As Integer,
                                  ByVal ho_Image As HObject,
                                  ByVal hv_CT_IDs As Object,
                                  ByVal T_Threshold As Integer,
                                  ByVal CTargetType As Integer,
                                  ByVal DTPram As TargetDetectParam)
        Try

            ' Stack for temporary control variables 
            Dim CTemp(10) As Object
            Dim SP_C As Integer
            SP_C = 0

            ' Stack for temporary objects 
            Dim OTemp(10) As HObject
            Dim SP_O As Integer
            SP_O = 0

            'test kiryu マルチスレッド化
            'HOperatorSet.SetSystem("parallelize_operators", True)
            'HOperatorSet.GetSystem("thread_num", 8)

            ' Local iconic variables 
            Dim ho_Region As HObject = Nothing, ho_ConnectedRegions As HObject = Nothing
            Dim ho_SelectedRegions As HObject = Nothing
            Dim ho_RegionFillUp As HObject = Nothing, ho_RegionIntersection As HObject = Nothing
            Dim ho_SelectedRegions2 As HObject = Nothing
            Dim ho_ConnectedRegions1 As HObject = Nothing
            Dim ho_ConnectedRegions2 As HObject = Nothing
            Dim ho_ObjectSelected As HObject = Nothing
            Dim ho_ConnectedRegions3 As HObject = Nothing
            Dim ho_Waku As HObject = Nothing, ho_FillWaku As HObject = Nothing
            Dim ho_Contour As HObject = Nothing, ho_RegressContours As HObject = Nothing
            Dim ho_UnionContours As HObject = Nothing, ho_ContoursSplit As HObject = Nothing
            Dim ho_RegionIntersection1 As HObject = Nothing
            Dim ho_RegionDifference As HObject = Nothing
            Dim ho_ConnectedRegions4 As HObject = Nothing
            Dim ho_TransRegions As HObject = Nothing
            Dim ho_RegionDifference1 As HObject = Nothing
            Dim ho_InTarget As HObject = Nothing
            Dim ho_ObjectSelected1 As HObject = Nothing
            Dim ho_SelectedRegionsT As HObject = Nothing
            Dim ho_SingleTargetRegion As HObject = Nothing
            Dim ho_Difference As HObject = Nothing
            Dim ho_CT_Regions As HObject = Nothing
            Dim ho_tempObj As HObject = Nothing
            Dim ho_ImageChannel1 As HObject = Nothing
            Dim ho_ContourT As HObject = Nothing

            ' Local control variables 
            Dim hv_WindowHandle As Object = Nothing
            Dim hv_Number As Object = Nothing, hv_Index As Object = Nothing
            Dim hv_objNum As Object = Nothing, hv_Rows As Object = Nothing
            Dim hv_Columns As Object = Nothing, hv_RowBegin As Object = Nothing
            Dim hv_ColBegin As Object = Nothing, hv_RowEnd As Object = Nothing
            Dim hv_ColEnd As Object = Nothing, hv_Nr As Object = Nothing
            Dim hv_Nc As Object = Nothing, hv_Dist As Object = Nothing
            Dim hv_IsParallel As Object = Nothing, hv_Area As Object = Nothing
            Dim hv_Row As Object = Nothing, hv_Column As Object = Nothing
            Dim hv_Sorted As Object = Nothing, hv_SortedAreaIndex As Object = Nothing
            Dim hv_AnaRow As Object = Nothing, hv_AnaColumn As Object = Nothing
            Dim hv_DiffR As Object = Nothing, hv_DiffC As Object = Nothing
            Dim hv_PowR As Object = Nothing, hv_PowC As Object = Nothing
            Dim hv_Sum As Object = Nothing, hv_Distance As Object = Nothing
            Dim hv_Indices As Object = Nothing, hv_tempIndex As Object = Nothing
            Dim hv_C0_Row As Object = Nothing, hv_C0_Col As Object = Nothing
            Dim hv_C1_Row As Object = Nothing, hv_C1_Col As Object = Nothing
            Dim hv_C2_Row As Object = Nothing, hv_C2_Col As Object = Nothing
            Dim hv_C3_Row As Object = Nothing, hv_C3_Col As Object = Nothing
            Dim hv_blnCheck As Object = Nothing, hv_Vec1_R As Object = Nothing
            Dim hv_Vec1_C As Object = Nothing, hv_Vec2_R As Object = Nothing
            Dim hv_Vec2_C As Object = Nothing, hv_Vec3_R As Object = Nothing
            Dim hv_Vec3_C As Object = Nothing, hv_Gaiseki1 As Object = Nothing
            Dim hv_Gaiseki2 As Object = Nothing, hv_Result As Object = Nothing
            Dim hv_I1 As Object = Nothing, hv_I2 As Object = Nothing
            Dim hv_I3 As Object = Nothing, hv_OutRow As Object = Nothing
            Dim hv_OutCol As Object = Nothing, hv_SRow As Object = Nothing
            Dim hv_SCol As Object = Nothing, hv_HomMat2D As Object = Nothing
            Dim hv_Covariance As Object = Nothing, hv_MeanArea As Object = Nothing
            Dim hv_InArea As Object = Nothing, hv_InRow As Object = Nothing
            Dim hv_InColumn As Object = Nothing, hv_RowTrans As Object = Nothing
            Dim hv_ColTrans As Object = Nothing, hv_RowIndex As Object = Nothing
            Dim hv_ColIndex As Object = Nothing, hv_CT_Number As Object = Nothing
            Dim hv_strCT_Number As Object = Nothing, hv_CT_ID_Index As Object = Nothing
            Dim hv_CT_ID As Object = Nothing, hv_Length As Object = Nothing
            Dim hv_Indices1 As Object = Nothing, hv_Selected As Object = Nothing
            Dim hv_Sorted1 As Object = Nothing, hv_CT_NumberIndex As Object = Nothing
            Dim hv_CTRow As Object = Nothing, hv_CTCol As Object = Nothing
            Dim hv_ST_num As Object = Nothing, hv_AreaST As Object = Nothing
            Dim hv_ind As Object = Nothing, hv_ST_Rows As Object = Nothing
            Dim hv_ST_Cols As Object = Nothing, hv_ST_Row As Object = Nothing
            Dim hv_ST_Col As Object = Nothing
            Dim hv_Length1 As Object = Nothing
            Dim hv_Indices2 As Object = Nothing
            Dim hv_Inverted As Object = Nothing
            Dim hv_Selected1 As Object = Nothing
            Dim hv_Sum1 As Object = Nothing
            Dim hv_sys As Object = Nothing

            HOperatorSet.GenEmptyObj(ho_Region)
            HOperatorSet.GenEmptyObj(ho_ConnectedRegions)
            HOperatorSet.GenEmptyObj(ho_SelectedRegions)
            HOperatorSet.GenEmptyObj(ho_RegionFillUp)
            HOperatorSet.GenEmptyObj(ho_RegionIntersection)
            HOperatorSet.GenEmptyObj(ho_SelectedRegions2)
            HOperatorSet.GenEmptyObj(ho_ConnectedRegions1)
            HOperatorSet.GenEmptyObj(ho_ConnectedRegions2)
            HOperatorSet.GenEmptyObj(ho_ObjectSelected)
            HOperatorSet.GenEmptyObj(ho_ConnectedRegions3)
            HOperatorSet.GenEmptyObj(ho_Waku)
            HOperatorSet.GenEmptyObj(ho_FillWaku)
            HOperatorSet.GenEmptyObj(ho_Contour)
            HOperatorSet.GenEmptyObj(ho_RegressContours)
            HOperatorSet.GenEmptyObj(ho_UnionContours)
            HOperatorSet.GenEmptyObj(ho_ContoursSplit)
            HOperatorSet.GenEmptyObj(ho_RegionIntersection1)
            HOperatorSet.GenEmptyObj(ho_RegionDifference)
            HOperatorSet.GenEmptyObj(ho_ConnectedRegions4)
            HOperatorSet.GenEmptyObj(ho_RegionDifference1)
            HOperatorSet.GenEmptyObj(ho_InTarget)
            HOperatorSet.GenEmptyObj(ho_ObjectSelected1)

            HOperatorSet.GenEmptyObj(ho_CT_Regions)
            HOperatorSet.GenEmptyObj(ho_SelectedRegionsT)
            HOperatorSet.GenEmptyObj(ho_SingleTargetRegion)
            HOperatorSet.GenEmptyObj(ho_Difference)
            HOperatorSet.GenEmptyObj(ho_tempObj)

            HOperatorSet.GenEmptyObj(ho_ImageChannel1)

            HOperatorSet.GenEmptyObj(ho_ContourT)



            All2D.Row = Nothing
            All2D.Col = Nothing

            If lstCT Is Nothing Then
                lstCT = New List(Of CodedTarget)
            Else
                lstCT.Clear()
            End If
            If strErrorMessageTxt Is Nothing Then
                strErrorMessageTxt = New List(Of String)
            Else
                strErrorMessageTxt.Clear()
            End If

            ClearHObject(ho_Region)
            ClearHObject(ho_ImageChannel1)
            GetFirstRegionG(ho_Image, ho_Region, ho_ImageChannel1, T_Threshold) '
            'HOperatorSet.Threshold(ho_Image, ho_Region, 100, 255)
            ClearHObject(ho_ConnectedRegions)
            HOperatorSet.Connection(ho_Region, ho_ConnectedRegions)
            ClearHObject(ho_SelectedRegions)
            HOperatorSet.SelectShape(ho_ConnectedRegions, ho_SelectedRegions, "holes_num", "and", 2, 2)
            ClearHObject(ho_RegionFillUp)
            HOperatorSet.FillUp(ho_SelectedRegions, ho_RegionFillUp)
            ClearHObject(ho_RegionIntersection)
            HOperatorSet.Intersection(ho_RegionFillUp, ho_ConnectedRegions, ho_RegionIntersection)

            HOperatorSet.GetSystem("parallelize_operators", hv_sys)
            HOperatorSet.GetSystem("tsp_used_thread_num", hv_sys)
            HOperatorSet.GetSystem("tsp_used_split_levels", hv_sys)

            ClearHObject(ho_SelectedRegions2)
            HOperatorSet.SelectShape(ho_RegionIntersection, ho_SelectedRegions2, "connect_num", "and", 5, 5)
            ClearHObject(ho_RegionFillUp)
            HOperatorSet.FillUp(ho_SelectedRegions2, ho_RegionFillUp)
            ClearHObject(ho_ConnectedRegions1)
            HOperatorSet.Connection(ho_RegionFillUp, ho_ConnectedRegions1)
            ClearHObject(ho_ConnectedRegions2)
            HOperatorSet.Connection(ho_SelectedRegions2, ho_ConnectedRegions2)
            HOperatorSet.CountObj(ho_RegionFillUp, hv_Number)
            ClearHObject(ho_CT_Regions)
            HOperatorSet.GenEmptyObj(ho_CT_Regions)

            For hv_Index = 1 To hv_Number.I Step 1
                Try
                    ClearHObject(ho_ObjectSelected)
                    HOperatorSet.SelectObj(ho_SelectedRegions2, ho_ObjectSelected, New HTuple(hv_Index))

                    ClearHObject(ho_ConnectedRegions3)
                    HOperatorSet.Connection(ho_ObjectSelected, ho_ConnectedRegions3)
                    HOperatorSet.CountObj(ho_ConnectedRegions3, hv_objNum)

                    If BTuple.TupleEqual(hv_objNum, 5).I = 1 Then
                        ClearHObject(ho_Waku)
                        HOperatorSet.SelectShape(ho_ConnectedRegions3, ho_Waku, "holes_num", "and", 2, 2)
                        ClearHObject(ho_FillWaku)
                        HOperatorSet.FillUp(ho_Waku, ho_FillWaku)
                        '枠の４頂点を抽出
                        '枠の外枠の4頂点ではなく内枠の4頂点を抽出するように変更　20120404
                        ClearHObject(ho_ContourT)
                        HOperatorSet.GenContourRegionXld(ho_Waku, ho_ContourT, "border_holes")
                        HOperatorSet.LengthXld(ho_ContourT, hv_Length1)
                        HOperatorSet.TupleSortIndex(hv_Length1, hv_Indices2)
                        HOperatorSet.TupleInverse(hv_Indices2, hv_Inverted)
                        HOperatorSet.TupleSelect(hv_Inverted, 1, hv_Selected1)
                        HOperatorSet.TupleAdd(hv_Selected1, 1, hv_Sum1)
                        ClearHObject(ho_Contour)
                        HOperatorSet.SelectObj(ho_ContourT, ho_Contour, hv_Sum1)

                        'get_region_polygon (FillWaku, 5, Rows, Columns)
                        'gen_contour_polygon_xld (Contour, Rows, Columns)
                        '必ず四角を見つけること！！！！！！！！
                        'まだまだ
                        ClearHObject(ho_RegressContours)
                        HOperatorSet.RegressContoursXld(ho_Contour, ho_RegressContours, "median", 1)
                        ClearHObject(ho_UnionContours)
                        HOperatorSet.UnionCollinearContoursXld(ho_RegressContours, ho_UnionContours, 10, 2, 1, 0.1, "attr_keep")
                        ClearHObject(ho_ContoursSplit)
                        HOperatorSet.SegmentContoursXld(ho_UnionContours, ho_ContoursSplit, DTPram.Mode, DTPram.SmoothCont, DTPram.MaxLineDist1, DTPram.MaxLineDist2)
                        HOperatorSet.LengthXld(ho_ContoursSplit, hv_Length)
                        HOperatorSet.TupleSortIndex(hv_Length, hv_Indices1)
                        HOperatorSet.TupleLastN(hv_Indices1, BTuple.TupleSub(BTuple.TupleLength(hv_Indices1), 4), hv_Selected)
                        HOperatorSet.TupleSort(hv_Selected, hv_Sorted1)
                        ClearHObject(ho_ObjectSelected1)

                        Dim hv_Selected_L As Object = Nothing
                        hv_Selected_L = BTuple.TupleAdd(hv_Sorted1, 1)


                        Try

                            Dim i As Integer
                            HOperatorSet.GenEmptyObj(ho_ObjectSelected1)
                            For i = 1 To BTuple.TupleLength(hv_Selected_L).I
                                Dim ho_tmpobj As HObject = Nothing
                                HOperatorSet.GenEmptyObj(ho_tmpobj)
                                HOperatorSet.SelectObj(ho_ContoursSplit, ho_tmpobj, BTuple.TupleSelect(hv_Selected_L, i - 1))
                                HOperatorSet.ConcatObj(ho_ObjectSelected1, ho_tmpobj, ho_ObjectSelected1)
                                ClearHObject(ho_tmpobj)
                            Next

                            Dim selectedCount = ho_ObjectSelected1.CountObj()
                            If selectedCount < 4 Then
                                Continue For
                            End If

                        Catch ex As Exception
                            Dim sss As String = ""
                            sss = ex.Message
                            Continue For 'TODO
                        End Try


                        HOperatorSet.FitLineContourXld(ho_ObjectSelected1, "tukey", -1, 0, 5, 2, hv_RowBegin, _
                        hv_ColBegin, hv_RowEnd, hv_ColEnd, hv_Nr, hv_Nc, hv_Dist)
                        HOperatorSet.IntersectionLl(hv_RowBegin, hv_ColBegin, hv_RowEnd, hv_ColEnd, BTuple.TupleConcat( _
                            BTuple.TupleSelectRange(hv_RowBegin, 1, 3), BTuple.TupleSelect(hv_RowBegin, _
                            0)), BTuple.TupleConcat(BTuple.TupleSelectRange(hv_ColBegin, 1, 3), BTuple.TupleSelect( _
                            hv_ColBegin, 0)), BTuple.TupleConcat(BTuple.TupleSelectRange(hv_RowEnd, 1, _
                            3), BTuple.TupleSelect(hv_RowEnd, 0)), BTuple.TupleConcat(BTuple.TupleSelectRange( _
                            hv_ColEnd, 1, 3), BTuple.TupleSelect(hv_ColEnd, 0)), hv_Rows, hv_Columns, _
                            hv_IsParallel)
                        'tuple_first_n (Rows, |Rows|-2, Rows)
                        'tuple_first_n (Columns, |Columns|-2, Columns)

                        '頂点１を抽出
                        'まず、頂点付近の孔を調べる。
                        ClearHObject(ho_RegionIntersection1)
                        HOperatorSet.Intersection(ho_Waku, ho_FillWaku, ho_RegionIntersection1)
                        ClearHObject(ho_RegionDifference)
                        HOperatorSet.Difference(ho_FillWaku, ho_Waku, ho_RegionDifference)
                        ClearHObject(ho_ConnectedRegions4)
                        HOperatorSet.Connection(ho_RegionDifference, ho_ConnectedRegions4)
                        HOperatorSet.AreaCenter(ho_ConnectedRegions4, hv_Area, hv_Row, hv_Column)
                        HOperatorSet.TupleSort(hv_Area, hv_Sorted)
                        HOperatorSet.TupleSortIndex(hv_Area, hv_SortedAreaIndex)
                        HOperatorSet.TupleSelect(hv_Row, BTuple.TupleSelect(hv_SortedAreaIndex, 0), hv_AnaRow)
                        HOperatorSet.TupleSelect(hv_Column, BTuple.TupleSelect(hv_SortedAreaIndex, 0), hv_AnaColumn)

                        '次に、その孔から一番近い頂点を頂点１とする。
                        HOperatorSet.TupleSub(hv_Rows, hv_AnaRow, hv_DiffR)
                        HOperatorSet.TupleSub(hv_Columns, hv_AnaColumn, hv_DiffC)
                        HOperatorSet.TuplePow(hv_DiffR, 2, hv_PowR)
                        HOperatorSet.TuplePow(hv_DiffC, 2, hv_PowC)
                        HOperatorSet.TupleAdd(hv_PowR, hv_PowC, hv_Sum)
                        HOperatorSet.TupleSqrt(hv_Sum, hv_Distance)
                        HOperatorSet.TupleSortIndex(hv_Distance, hv_Indices)
                        hv_tempIndex = BTuple.TupleSelect(hv_Indices, 0)
                        hv_C0_Row = BTuple.TupleSelect(hv_Rows, hv_tempIndex)
                        hv_C0_Col = BTuple.TupleSelect(hv_Columns, hv_tempIndex)
                        'その次に、残りの３頂点を特定する。
                        hv_C1_Row = 0
                        hv_C1_Col = 0
                        hv_C2_Row = 0
                        hv_C2_Col = 0
                        hv_C3_Row = 0
                        hv_C3_Col = 0
                        CTemp(SP_C) = hv_Rows
                        SP_C = SP_C + 1
                        HOperatorSet.TupleRemove(CTemp(SP_C - 1), hv_tempIndex, hv_Rows)
                        SP_C = 0
                        CTemp(SP_C) = hv_Columns
                        SP_C = SP_C + 1
                        HOperatorSet.TupleRemove(CTemp(SP_C - 1), hv_tempIndex, hv_Columns)
                        SP_C = 0
                        hv_blnCheck = -1
                        hv_Vec1_R = BTuple.TupleSub(BTuple.TupleSelect(hv_Rows, 0), hv_C0_Row)
                        hv_Vec1_C = BTuple.TupleSub(BTuple.TupleSelect(hv_Columns, 0), hv_C0_Col)
                        hv_Vec2_R = BTuple.TupleSub(BTuple.TupleSelect(hv_Rows, 1), hv_C0_Row)
                        hv_Vec2_C = BTuple.TupleSub(BTuple.TupleSelect(hv_Columns, 1), hv_C0_Col)
                        hv_Vec3_R = BTuple.TupleSub(BTuple.TupleSelect(hv_Rows, 2), hv_C0_Row)
                        hv_Vec3_C = BTuple.TupleSub(BTuple.TupleSelect(hv_Columns, 2), hv_C0_Col)
                        CalcGaiseki(hv_Vec1_R, hv_Vec1_C, hv_Vec2_R, hv_Vec2_C, hv_Gaiseki1)
                        CalcGaiseki(hv_Vec1_R, hv_Vec1_C, hv_Vec3_R, hv_Vec3_C, hv_Gaiseki2)
                        hv_Result = BTuple.TupleMult(hv_Gaiseki1, hv_Gaiseki2)
                        If BTuple.TupleGreater(hv_Result, 0).I = 1 Then
                            CalcGaiseki(hv_Vec2_R, hv_Vec2_C, hv_Vec1_R, hv_Vec1_C, hv_Gaiseki1)
                            CalcGaiseki(hv_Vec2_R, hv_Vec2_C, hv_Vec3_R, hv_Vec3_C, hv_Gaiseki2)
                            hv_Result = BTuple.TupleMult(hv_Gaiseki1, hv_Gaiseki2)
                            If BTuple.TupleGreater(hv_Result, 0).I = 1 Then
                                CalcGaiseki(hv_Vec3_R, hv_Vec3_C, hv_Vec1_R, hv_Vec1_C, hv_Gaiseki1)
                                CalcGaiseki(hv_Vec3_R, hv_Vec3_C, hv_Vec2_R, hv_Vec2_C, hv_Gaiseki2)
                                hv_I1 = 2
                                If BTuple.TupleLess(hv_Gaiseki1, 0).I = 1 Then
                                    hv_I2 = 0
                                    hv_I3 = 1
                                Else
                                    hv_I2 = 1
                                    hv_I3 = 0
                                End If
                            Else
                                hv_I1 = 1
                                If BTuple.TupleLess(hv_Gaiseki1, 0).I = 1 Then
                                    hv_I2 = 0
                                    hv_I3 = 2
                                Else
                                    hv_I2 = 2
                                    hv_I3 = 0
                                End If
                            End If
                        Else
                            hv_I1 = 0
                            If BTuple.TupleLess(hv_Gaiseki1, 0).I = 1 Then
                                hv_I2 = 1
                                hv_I3 = 2
                            Else
                                hv_I2 = 2
                                hv_I3 = 1
                            End If
                        End If
                        hv_C1_Row = BTuple.TupleSelect(hv_Rows, hv_I1)
                        hv_C1_Col = BTuple.TupleSelect(hv_Columns, hv_I1)
                        hv_C2_Row = BTuple.TupleSelect(hv_Rows, hv_I2)
                        hv_C2_Col = BTuple.TupleSelect(hv_Columns, hv_I2)
                        hv_C3_Row = BTuple.TupleSelect(hv_Rows, hv_I3)
                        hv_C3_Col = BTuple.TupleSelect(hv_Columns, hv_I3)
                        hv_OutRow = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(hv_C0_Row, _
                            hv_C1_Row), hv_C2_Row), hv_C3_Row)
                        hv_OutCol = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(hv_C0_Col, _
                            hv_C1_Col), hv_C2_Col), hv_C3_Col)



                        '枠の4頂点で射影変換

                        hv_SRow = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(0, DTPram.TargetUchiWakuSize), 0), DTPram.TargetUchiWakuSize)
                        hv_SCol = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(0, DTPram.TargetUchiWakuSize), DTPram.TargetUchiWakuSize), 0)
                        '20160420 不具合修正 SUSANO ADD START
                        'CTフレームを正対させる処理に失敗すればその後の処理を進める必要はない。
                        'つまり、このCTは認識不可とする。
                        Try
                            HOperatorSet.VectorToProjHomMat2d(hv_OutRow, hv_OutCol, hv_SRow, hv_SCol, "gold_standard", _
                         New HTuple, New HTuple, New HTuple, New HTuple, _
                         New HTuple, New HTuple, hv_HomMat2D, hv_Covariance)
                        Catch ex As Exception
                            Continue For
                        End Try
                        '20160420 不具合修正 SUSANO ADD START
                        'projective_trans_region (ConnectedRegions3, TransRegions, HomMat2D, 'bilinear')
                        '枠内の4点を抽出
                        ClearHObject(ho_RegionDifference1)
                        HOperatorSet.Difference(ho_ConnectedRegions3, ho_Waku, ho_RegionDifference1)
                        HOperatorSet.AreaCenter(ho_ConnectedRegions3, hv_Area, hv_Row, hv_Column)
                        HOperatorSet.TupleMean(hv_Area, hv_MeanArea)
                        ClearHObject(ho_InTarget)
                        '面積下限変更　15→9
                        HOperatorSet.SelectShape(ho_ConnectedRegions3, ho_InTarget, "area", "and", DTPram.SSAreaThreshold, hv_MeanArea)

                        'Rep By Suuri 20150309 Sta ------CT代表点を近傍STに--- 
                        Get2DCoord(ho_ImageChannel1, ho_InTarget, hv_InRow, hv_InColumn, hv_AreaST)
                        'Rep By Suuri 20150309 End ------CT代表点を近傍STに---

                        'ClearHObject(ho_tempObj)
                        If BTuple.TupleLength(hv_InRow).I <> 4 Then
                            Continue For
                        End If
                        'HOperatorSet.AreaCenter(ho_InTarget, hv_InArea, hv_InRow, hv_InColumn)

                        '枠内の４点の配置を調べる
                        HOperatorSet.ProjectiveTransPixel(hv_HomMat2D, hv_InRow, hv_InColumn, hv_RowTrans, hv_ColTrans)
                        CTemp(SP_C) = hv_RowTrans
                        SP_C = SP_C + 1
                        HOperatorSet.TupleSub(CTemp(SP_C - 1), DTPram.PaternGridInterval + DTPram.PaternGrid / 2, hv_RowTrans)
                        SP_C = 0
                        CTemp(SP_C) = hv_ColTrans
                        SP_C = SP_C + 1
                        HOperatorSet.TupleSub(CTemp(SP_C - 1), DTPram.PaternGridInterval + DTPram.PaternGrid / 2, hv_ColTrans)
                        SP_C = 0
                        CTemp(SP_C) = hv_RowTrans
                        SP_C = SP_C + 1
                        HOperatorSet.TupleDiv(CTemp(SP_C - 1), DTPram.PaternGrid, hv_RowTrans)
                        SP_C = 0
                        CTemp(SP_C) = hv_ColTrans
                        SP_C = SP_C + 1
                        HOperatorSet.TupleDiv(CTemp(SP_C - 1), DTPram.PaternGrid, hv_ColTrans)
                        SP_C = 0
                        HOperatorSet.TupleRound(hv_RowTrans, hv_RowIndex)
                        HOperatorSet.TupleRound(hv_ColTrans, hv_ColIndex)
                        CTemp(SP_C) = hv_RowIndex
                        SP_C = SP_C + 1
                        HOperatorSet.TupleAdd(CTemp(SP_C - 1), 1, hv_RowIndex)
                        SP_C = 0
                        CTemp(SP_C) = hv_ColIndex
                        SP_C = SP_C + 1
                        HOperatorSet.TupleAdd(CTemp(SP_C - 1), 1, hv_ColIndex)
                        SP_C = 0
                        hv_CT_Number = BTuple.TupleAdd(BTuple.TupleMult(5, BTuple.TupleSub(hv_RowIndex, _
                            1)), hv_ColIndex)
                        HOperatorSet.TupleSortIndex(hv_CT_Number, hv_CT_NumberIndex)


                        CTemp(SP_C) = hv_CT_Number
                        SP_C = SP_C + 1
                        HOperatorSet.TupleSort(CTemp(SP_C - 1), hv_CT_Number)
                        SP_C = 0
                        hv_strCT_Number = BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleAdd( _
                            BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleSelect(hv_CT_Number, 0), "_"), _
                            BTuple.TupleSelect(hv_CT_Number, 1)), "_"), BTuple.TupleSelect(hv_CT_Number, _
                            2)), "_"), BTuple.TupleSelect(hv_CT_Number, 3))
                        '配置によって、CTの番号を特定する。
                        HOperatorSet.TupleFind(hv_CT_IDs, hv_strCT_Number, hv_CT_ID_Index)
                        If hv_CT_ID_Index.I = -1 Then
                            ' Stop
                            Dim MismatchCT As New CodedTarget

                            'AnCT.CT_ID = CInt(Tuple.TupleAdd(hv_strCT_Number, 1))
                            MismatchCT.MismatchCT_Name = hv_strCT_Number.ToString
                            MismatchCT.ImageID = ImageID
                            MismatchCT.CT_Points.Row = BTuple.TupleSelect(hv_InRow, hv_CT_NumberIndex)
                            MismatchCT.CT_Points.Col = BTuple.TupleSelect(hv_InColumn, hv_CT_NumberIndex)
                            lstMismatchCT.Add(MismatchCT)
                            All2D_MismatchCT.Row = BTuple.TupleConcat(All2D_MismatchCT.Row, MismatchCT.CT_Points.Row)
                            All2D_MismatchCT.Col = BTuple.TupleConcat(All2D_MismatchCT.Col, MismatchCT.CT_Points.Col)

                            Continue For

                        End If
                        hv_CT_ID = BTuple.TupleAdd(hv_CT_ID_Index, 1)
                        If hv_CT_ID.I = 1319 Then
                            ' Continue For
                            Dim ddd As Integer = 1
                        End If
                        hv_CTRow = BTuple.TupleSelect(hv_InRow, hv_CT_NumberIndex)
                        hv_CTCol = BTuple.TupleSelect(hv_InColumn, hv_CT_NumberIndex)

                        Dim NewCT As New CodedTarget
                        NewCT.CT_ID = CInt(hv_CT_ID.I)
                        NewCT.ImageID = ImageID
                        NewCT.CT_Points.Row = hv_CTRow
                        NewCT.CT_Points.Col = hv_CTCol
                        NewCT.AllST_Area = BTuple.TupleSum(hv_AreaST).D 'SUURI ADD 20150217 CT代表点をSTに
                        NewCT.SetlstCTtoST()
                        Dim BadCT As Boolean = False
                        For Each CT As CodedTarget In lstCT

                            If CT.CT_ID = NewCT.CT_ID Then
                                '  BadCT = True
                                strErrorMessageTxt.Add(CT.CT_ID)
                                Exit For

                            End If
                        Next
                        If BadCT = False Then
                            ' If NewCT.CT_ID <= 500 Then '500番台は追加しない　20151021　ＡＤＤ
                            lstCT.Add(NewCT)
                            All2D.Row = BTuple.TupleConcat(All2D.Row, NewCT.CT_Points.Row)
                            All2D.Col = BTuple.TupleConcat(All2D.Col, NewCT.CT_Points.Col)
                            'End If
                        End If

                        HOperatorSet.CopyObj(ho_CT_Regions, OTemp(SP_O), 1, -1)
                        SP_O = SP_O + 1
                        ClearHObject(ho_CT_Regions)
                        HOperatorSet.ConcatObj(OTemp(SP_O - 1), ho_ObjectSelected, ho_CT_Regions)
                        ClearHObject(OTemp(SP_O - 1))
                        SP_O = 0

                    End If
                Catch ex As Exception
                    Dim ss = ""
                End Try
            Next

            HOperatorSet.CountObj(ho_CT_Regions, hv_ST_num)
            HOperatorSet.CopyObj(ho_CT_Regions, OTemp(SP_O), 1, -1)


            SP_O = SP_O + 1
            ClearHObject(ho_CT_Regions)
            HOperatorSet.FillUp(OTemp(SP_O - 1), ho_CT_Regions)
            ClearHObject(OTemp(SP_O - 1))
            SP_O = 0

            ClearHObject(ho_Difference)
            HOperatorSet.Difference(ho_ConnectedRegions, ho_CT_Regions, ho_Difference)
            ClearHObject(ho_SingleTargetRegion)
            Dim param1 As Object = Nothing
            Dim parammin As Object = Nothing
            Dim parammax As Object = Nothing
            param1 = BTuple.TupleConcat(BTuple.TupleConcat("holes_num", "circularity"), "area")
            parammin = BTuple.TupleConcat(BTuple.TupleConcat(0, 0.3), 10.0)
            parammax = BTuple.TupleConcat(BTuple.TupleConcat(0, 1.0), 999999.0)

            HOperatorSet.SelectShape(ho_Difference, ho_SingleTargetRegion, "holes_num", "and", 0, 0)
            HOperatorSet.SelectShape(ho_SingleTargetRegion, ho_SingleTargetRegion, "circularity", "and", 0.3, 1.0)
            HOperatorSet.SelectShape(ho_SingleTargetRegion, ho_SingleTargetRegion, "area", "and", 10, 999999.0)


            'Rep By Suuri 20150309 Sta ------CT代表点を近傍STに---
            Get2DCoord(ho_ImageChannel1, ho_SingleTargetRegion, hv_ST_Rows, hv_ST_Cols, hv_AreaST)
            'Rep By Suuri 20150309 End ------CT代表点を近傍STに---

            hv_ST_num = BTuple.TupleLength(hv_ST_Rows)
            If lstST Is Nothing Then
                lstST = New List(Of SingleTarget)
            Else
                lstST.Clear()
            End If
            All2D.Row = BTuple.TupleConcat(All2D.Row, hv_ST_Rows)
            All2D.Col = BTuple.TupleConcat(All2D.Col, hv_ST_Cols)
            For hv_ind = 0 To BTuple.TupleSub(hv_ST_num, 1) Step 1
                HOperatorSet.TupleSelect(hv_ST_Rows, hv_ind, hv_ST_Row)
                HOperatorSet.TupleSelect(hv_ST_Cols, hv_ind, hv_ST_Col)
                Dim NewST As New SingleTarget
                NewST.P2D.Row = hv_ST_Row
                NewST.P2D.Col = hv_ST_Col
                NewST.P2ID = hv_ind.I + 1
                NewST.ImageID = ImageID
                NewST.flgUsed = 0
                HOperatorSet.TupleSelect(hv_AreaST, hv_ind, NewST.AreaST)
                '  NewST.AreaST = hv_AreaST(hv_ind) 'SUURI ADD 20150217
                lstST.Add(NewST)
            Next

            'SUURI ADD 20150307	CT代表点を近傍STに
#If True Then
            Dim intKeisuuKyori As Integer
            intKeisuuKyori = GetPrivateProfileInt("Kaiseki", "STtoCT", 0, My.Application.Info.DirectoryPath & "\vform.ini")
            'SUURI UPDATE START 20150322
            GetListSTtoCT(My.Application.Info.DirectoryPath & "\STtoCTlist.csv")
            Dim ii As Integer
            Dim nn As Integer = lstCT.Count

            For ii = nn - 1 To 0 Step -1
                Dim objCT As CodedTarget
                objCT = lstCT.Item(ii)
                'Next
                'For Each objCT As CodedTarget In lstCT
                Dim mindistST As Double = Double.MaxValue
                Dim minST As SingleTarget = Nothing

                '対象のＣＴに一番近いＳＴを取得
                For Each objST As SingleTarget In lstST
                    Dim objKyori As Object = Nothing
                    'ただし、そのＳＴの面積がＣＴの内部点の全面積より大きい場合
                    'つまり、ある程度大きさのあるＳＴを選択する。
                    If objST.AreaST > objCT.AllST_Area Then
                        objCT.lstCTtoST.Item(0).P2D.CalcDistToInputPoint(objST.P2D.Row,
                                                                         objST.P2D.Col,
                                                                         objKyori)
                        If objKyori < mindistST Then
                            mindistST = objKyori
                            minST = objST
                        End If
                    End If
                Next

                '一番近いＳＴがＣＴの内部点間の距離より小さいければＣＴの一番目の点（代表点）に置き換える
                'つまりある程度CTに近いＳＴを選択する。
                If CInt(intKeisuuKyori) > 0 Then
                    'SUURI ADD 20150321
                    Dim flgSyoriOK As Boolean = False
                    For Each objSTtoCT As STtoCT In lstSTtoCT
                        If objCT.CT_ID = objSTtoCT.CTID Then
                            flgSyoriOK = True
                        End If
                    Next
                    If flgSyoriOK = True Then
                        If mindistST < objCT.GetAllPointsKyoriPixel * CInt(intKeisuuKyori) Then
                            objCT.lstCTtoST.Item(0).P2D.CopyToMe(minST.P2D)
                            objCT.CT_Points.Row(0) = minST.P2D.Row
                            objCT.CT_Points.Col(0) = minST.P2D.Col
                            objCT.CenterPoint.CopyToMe(minST.P2D)
                        Else
                            lstCT.RemoveAt(ii)

                        End If
                    End If
                End If
            Next
            'SUURI UPDATE END 20150323
#End If
            'SUURI ADD 20150307 
            All2D_ST.Row = hv_ST_Rows
            All2D_ST.Col = hv_ST_Cols

            ClearHObject(ho_Region)
            ClearHObject(ho_ConnectedRegions)
            ClearHObject(ho_SelectedRegions)
            ClearHObject(ho_RegionFillUp)
            ClearHObject(ho_RegionIntersection)
            ClearHObject(ho_SelectedRegions2)
            ClearHObject(ho_ConnectedRegions1)
            ClearHObject(ho_ConnectedRegions2)
            ClearHObject(ho_ObjectSelected)
            ClearHObject(ho_ConnectedRegions3)
            ClearHObject(ho_Waku)
            ClearHObject(ho_FillWaku)
            ClearHObject(ho_Contour)
            ClearHObject(ho_RegressContours)
            ClearHObject(ho_UnionContours)
            ClearHObject(ho_ContoursSplit)
            ClearHObject(ho_RegionIntersection1)
            ClearHObject(ho_RegionDifference)
            ClearHObject(ho_ConnectedRegions4)
            ClearHObject(ho_RegionDifference1)
            ClearHObject(ho_InTarget)
            ClearHObject(ho_CT_Regions)
            ClearHObject(ho_SelectedRegionsT)

            ClearHObject(ho_SingleTargetRegion)
            ClearHObject(ho_Difference)
            ClearHObject(ho_tempObj)
            ClearHObject(ho_ContourT)

        Catch ex As Exception
            ex.ToString()
            Exit Sub
        End Try
    End Sub
    Private Sub FourTenTarget1234(ByVal hv_Rows As Object,
                               ByVal hv_Columns As Object,
                               ByVal hv_tempIndex As Object,
                               ByRef R0 As Object,
                               ByRef C0 As Object,
                               ByRef R1 As Object,
                               ByRef C1 As Object,
                               ByRef R2 As Object,
                               ByRef C2 As Object,
                               ByRef R3 As Object,
                               ByRef C3 As Object)

        Dim CTemp(10) As Object
        Dim SP_C As Integer
        SP_C = 0

        Dim hv_C0_Row As Object = Nothing, hv_C0_Col As Object = Nothing
        Dim hv_C1_Row As Object = Nothing, hv_C1_Col As Object = Nothing
        Dim hv_C2_Row As Object = Nothing, hv_C2_Col As Object = Nothing
        Dim hv_C3_Row As Object = Nothing, hv_C3_Col As Object = Nothing
        Dim hv_blnCheck As Object = Nothing, hv_Vec1_R As Object = Nothing
        Dim hv_Vec1_C As Object = Nothing, hv_Vec2_R As Object = Nothing
        Dim hv_Vec2_C As Object = Nothing, hv_Vec3_R As Object = Nothing
        Dim hv_Vec3_C As Object = Nothing, hv_Gaiseki1 As Object = Nothing
        Dim hv_Gaiseki2 As Object = Nothing, hv_Result As Object = Nothing
        Dim hv_I1 As Object = Nothing, hv_I2 As Object = Nothing
        Dim hv_I3 As Object = Nothing

        hv_C0_Row = BTuple.TupleSelect(hv_Rows, hv_tempIndex)
        hv_C0_Col = BTuple.TupleSelect(hv_Columns, hv_tempIndex)
        'その次に、残りの３頂点を特定する。
        hv_C1_Row = 0
        hv_C1_Col = 0
        hv_C2_Row = 0
        hv_C2_Col = 0
        hv_C3_Row = 0
        hv_C3_Col = 0
        CTemp(SP_C) = hv_Rows
        SP_C = SP_C + 1
        HOperatorSet.TupleRemove(CTemp(SP_C - 1), hv_tempIndex, hv_Rows)
        SP_C = 0
        CTemp(SP_C) = hv_Columns
        SP_C = SP_C + 1
        HOperatorSet.TupleRemove(CTemp(SP_C - 1), hv_tempIndex, hv_Columns)
        SP_C = 0
        hv_blnCheck = -1
        hv_Vec1_R = BTuple.TupleSub(BTuple.TupleSelect(hv_Rows, 0), hv_C0_Row)
        hv_Vec1_C = BTuple.TupleSub(BTuple.TupleSelect(hv_Columns, 0), hv_C0_Col)
        hv_Vec2_R = BTuple.TupleSub(BTuple.TupleSelect(hv_Rows, 1), hv_C0_Row)
        hv_Vec2_C = BTuple.TupleSub(BTuple.TupleSelect(hv_Columns, 1), hv_C0_Col)
        hv_Vec3_R = BTuple.TupleSub(BTuple.TupleSelect(hv_Rows, 2), hv_C0_Row)
        hv_Vec3_C = BTuple.TupleSub(BTuple.TupleSelect(hv_Columns, 2), hv_C0_Col)
        CalcGaiseki(hv_Vec1_R, hv_Vec1_C, hv_Vec2_R, hv_Vec2_C, hv_Gaiseki1)
        CalcGaiseki(hv_Vec1_R, hv_Vec1_C, hv_Vec3_R, hv_Vec3_C, hv_Gaiseki2)
        hv_Result = BTuple.TupleMult(hv_Gaiseki1, hv_Gaiseki2)
        If BTuple.TupleGreater(hv_Result, 0) = 1 Then
            CalcGaiseki(hv_Vec2_R, hv_Vec2_C, hv_Vec1_R, hv_Vec1_C, hv_Gaiseki1)
            CalcGaiseki(hv_Vec2_R, hv_Vec2_C, hv_Vec3_R, hv_Vec3_C, hv_Gaiseki2)
            hv_Result = BTuple.TupleMult(hv_Gaiseki1, hv_Gaiseki2)
            If BTuple.TupleGreater(hv_Result, 0) = 1 Then
                CalcGaiseki(hv_Vec3_R, hv_Vec3_C, hv_Vec1_R, hv_Vec1_C, hv_Gaiseki1)
                CalcGaiseki(hv_Vec3_R, hv_Vec3_C, hv_Vec2_R, hv_Vec2_C, hv_Gaiseki2)
                hv_I1 = 2
                If BTuple.TupleLess(hv_Gaiseki1, 0) = 1 Then
                    hv_I2 = 0
                    hv_I3 = 1
                Else
                    hv_I2 = 1
                    hv_I3 = 0
                End If
            Else
                hv_I1 = 1
                If BTuple.TupleLess(hv_Gaiseki1, 0) = 1 Then
                    hv_I2 = 0
                    hv_I3 = 2
                Else
                    hv_I2 = 2
                    hv_I3 = 0
                End If
            End If
        Else
            hv_I1 = 0
            If BTuple.TupleLess(hv_Gaiseki1, 0) = 1 Then
                hv_I2 = 1
                hv_I3 = 2
            Else
                hv_I2 = 2
                hv_I3 = 1
            End If
        End If
        hv_C1_Row = BTuple.TupleSelect(hv_Rows, hv_I1)
        hv_C1_Col = BTuple.TupleSelect(hv_Columns, hv_I1)
        hv_C2_Row = BTuple.TupleSelect(hv_Rows, hv_I2)
        hv_C2_Col = BTuple.TupleSelect(hv_Columns, hv_I2)
        hv_C3_Row = BTuple.TupleSelect(hv_Rows, hv_I3)
        hv_C3_Col = BTuple.TupleSelect(hv_Columns, hv_I3)

        R0 = hv_C1_Row
        C0 = hv_C1_Col
        R1 = hv_C3_Row
        C1 = hv_C3_Col
        R2 = hv_C2_Row
        C2 = hv_C2_Col
        R3 = hv_C0_Row
        C3 = hv_C0_Col

        'R3 = hv_C3_Row
        'C3 = hv_C3_Col
        'R2 = hv_C2_Row
        'C2 = hv_C2_Col
        'R1 = hv_C1_Row
        'C1 = hv_C1_Col
        'R0 = hv_C0_Row
        'C0 = hv_C0_Col

    End Sub


    ''' 

    ''' 未使用
    ''' 

    ''' 3チャンネル画像に対してBinThresholdを掛け3つの領域を結合する。
    Private Sub GetFirstRegion(ByRef ho_Image As HObject, ByRef ho_Region As HObject)
        Dim ho_Image1 As HObject = Nothing
        Dim ho_Image2 As HObject = Nothing
        Dim ho_Image3 As HObject = Nothing
        Dim ho_Region1 As HObject = Nothing, ho_RegionComplement1 As HObject = Nothing
        Dim ho_Region2 As HObject = Nothing, ho_RegionComplement2 As HObject = Nothing
        Dim ho_Region3 As HObject = Nothing, ho_RegionComplement3 As HObject = Nothing
        Dim ho_RegionUnion As HObject = Nothing

        ' Initialize local and output iconic variables 
        HOperatorSet.GenEmptyObj(ho_Image1)
        HOperatorSet.GenEmptyObj(ho_Image2)
        HOperatorSet.GenEmptyObj(ho_Image3)
        HOperatorSet.GenEmptyObj(ho_Region1)
        HOperatorSet.GenEmptyObj(ho_RegionComplement1)
        HOperatorSet.GenEmptyObj(ho_Region2)
        HOperatorSet.GenEmptyObj(ho_RegionComplement2)
        HOperatorSet.GenEmptyObj(ho_Region3)
        HOperatorSet.GenEmptyObj(ho_RegionComplement3)
        HOperatorSet.GenEmptyObj(ho_RegionUnion)

        HOperatorSet.ClearObj(ho_Image1)
        HOperatorSet.ClearObj(ho_Image2)
        HOperatorSet.ClearObj(ho_Image3)
        HOperatorSet.Decompose3(ho_Image, ho_Image1, ho_Image2, ho_Image3)
        HOperatorSet.ClearObj(ho_Region1)
        HOperatorSet.BinThreshold(ho_Image1, ho_Region1)
        HOperatorSet.ClearObj(ho_RegionComplement1)
        HOperatorSet.Complement(ho_Region1, ho_RegionComplement1)
        HOperatorSet.ClearObj(ho_Region2)
        HOperatorSet.BinThreshold(ho_Image2, ho_Region2)
        HOperatorSet.ClearObj(ho_RegionComplement2)
        HOperatorSet.Complement(ho_Region2, ho_RegionComplement2)
        HOperatorSet.ClearObj(ho_Region3)
        HOperatorSet.BinThreshold(ho_Image3, ho_Region3)
        HOperatorSet.ClearObj(ho_RegionComplement3)
        HOperatorSet.Complement(ho_Region3, ho_RegionComplement3)
        HOperatorSet.ClearObj(ho_RegionUnion)
        HOperatorSet.Union2(ho_RegionComplement1, ho_RegionComplement2, ho_RegionUnion)
        HOperatorSet.ClearObj(ho_Region)
        HOperatorSet.Union2(ho_RegionUnion, ho_RegionComplement3, ho_Region)

        HOperatorSet.ClearObj(ho_Image1)
        HOperatorSet.ClearObj(ho_Image2)
        HOperatorSet.ClearObj(ho_Image3)
        HOperatorSet.ClearObj(ho_Region1)
        HOperatorSet.ClearObj(ho_RegionComplement1)
        HOperatorSet.ClearObj(ho_Region2)
        HOperatorSet.ClearObj(ho_RegionComplement2)
        HOperatorSet.ClearObj(ho_Region3)
        HOperatorSet.ClearObj(ho_RegionComplement3)
        HOperatorSet.ClearObj(ho_RegionUnion)
        
    End Sub

    ''' 

    ''' '3チャンネル画像のGreen画像のみに対してBinThresholdを掛けて領域を抽出する。
    ''' 

    Private Sub GetFirstRegionG(ByRef ho_Image As HObject, _
                                ByRef ho_Region As HObject, _
                                ByRef ImageChannel1 As HObject, _
                                ByVal intThreshold As Integer)
        ' Dim ho_Image2 As HObject = Nothing
        Dim ho_Region2 As HObject = Nothing
        Dim ho_EmphasizeImage As HObject = Nothing

        HOperatorSet.GenEmptyObj(ImageChannel1)
        HOperatorSet.GenEmptyObj(ho_Region2)
        HOperatorSet.GenEmptyObj(ho_EmphasizeImage)
        Dim HH As New HTuple
        Dim WW As New HTuple
        HOperatorSet.GetImagePointer1(ho_Image, Nothing, Nothing, WW, HH)
        Debug.Print(WW.D & "   " & HH.D)
        ClearHObject(ImageChannel1)
        HOperatorSet.Decompose3(ho_Image, Nothing, ImageChannel1, Nothing)
        ClearHObject(ho_EmphasizeImage)
        HOperatorSet.Emphasize(ImageChannel1, ho_EmphasizeImage, 7, 7, 1)

        ClearHObject(ho_Region2)
        HOperatorSet.BinThreshold(ho_EmphasizeImage, ho_Region2)
        HOperatorSet.Complement(ho_Region2, ho_Region)
        If intThreshold > 1 Then
            ' HOperatorSet.Threshold(ho_Image, ho_Region, intThreshold, 255)
            HOperatorSet.Threshold(ho_EmphasizeImage, ho_Region, intThreshold, 255)
        End If

        'HOperatorSet.ClearObj(ho_Image2)
        ClearHObject(ho_Region2)
        ClearHObject(ho_EmphasizeImage)
    End Sub

    'Rep By Suuri 20150309 Sta---CT代表点を近傍STに 
    'Private Sub Get2DCoord(ByRef ho_Image As HObject, ByRef InTarget As HObject, ByRef Row As Object, ByRef Col As Object)
    Private Sub Get2DCoord(ByRef ho_Image As HObject, ByRef InTarget As HObject, ByRef Row As Object, ByRef Col As Object, ByRef hvArea As Object)
        'Rep By Suuri 20150309 End---CT代表点を近傍STに

        'Dim ho_RegionUnion As HObject = Nothing
        'Dim ho_ReduceImage As HObject = Nothing
        'Dim ho_Edges As HObject = Nothing
        'HOperatorSet.GenEmptyObj(ho_RegionUnion)
        'HOperatorSet.GenEmptyObj(ho_ReduceImage)
        'HOperatorSet.GenEmptyObj(ho_Edges)
        'HOperatorSet.ClearObj(ho_RegionUnion)
        'HOperatorSet.Union1(ho_DilationCircle, ho_RegionUnion)
        'HOperatorSet.ClearObj(ho_ReduceImage)
        'HOperatorSet.ReduceDomain(ho_Image, ho_RegionUnion, ho_ReduceImage)
        'HOperatorSet.AutoThreshold (ho_Dilation
        'HOperatorSet.ClearObj(ho_Edges)
        'HOperatorSet.EdgesColorSubPix(ho_ReduceImage, ho_Edges, "canny", 1, 20, 40)
        'Try
        '    HOperatorSet.FitEllipseContourXld(ho_Edges, "fitzgibbon", -1, 0, 0, 200, 3, 2, Row, Col, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        'Catch ex As Exception
        'End Try
        'HOperatorSet.EdgesSubPix(ho_ReduceImage, ho_Edges, "canny", 1, 20, 40)
        '  HOperatorSet.AreaCenterXld(ho_Edges, Nothing, Row, Col, Nothing)
        ' HOperatorSet.AreaCenterPointsXld(ho_Edges, Nothing, Row, Col)
        'HOperatorSet.ClearObj(ho_RegionUnion)
        'HOperatorSet.ClearObj(ho_ReduceImage)
        'HOperatorSet.ClearObj(ho_Edges)


        Dim ho_DilationCircle As HObject = Nothing
        HOperatorSet.GenEmptyObj(ho_DilationCircle)
        ClearHObject(ho_DilationCircle)
        HOperatorSet.DilationCircle(InTarget, ho_DilationCircle, 1.5)

        'Rep By Suuri 20150309 Sta -----CT代表点を近傍STに
        'HOperatorSet.AreaCenterGray(ho_DilationCircle, ho_Image, Nothing, Row, Col)
        Try
            HOperatorSet.AreaCenterGray(ho_DilationCircle, ho_Image, hvArea, Row, Col)
        Catch ex As Exception
            Exit Sub
        End Try

        'Rep By Suuri 20150309 End -----CT代表点を近傍STに

        ClearHObject(ho_DilationCircle)

        ' HOperatorSet.AreaCenterGray(InTarget, ho_Image, Nothing, Row, Col)

    End Sub

    Public Function GetST_MaxID() As Integer
        GetST_MaxID = 0
        For Each ST As SingleTarget In lstST
            If GetST_MaxID < ST.P2ID Then
                GetST_MaxID = ST.P2ID
            End If
        Next
    End Function
End Class