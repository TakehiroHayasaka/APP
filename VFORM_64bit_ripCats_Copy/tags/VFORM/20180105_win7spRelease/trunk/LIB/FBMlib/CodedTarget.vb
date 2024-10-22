﻿Imports HalconDotNet

Public Class CodedTarget
    ''' <summary>
    ''' CTのsystemlabel()のID（番号）
    ''' </summary>
    Public CT_ID As Integer
    ''' <summary>
    ''' 画像ID
    ''' </summary>
    Public ImageID As Integer
    ''' <summary>
    ''' CTの2次元座標（画像）
    ''' </summary>
    Public CT_Points As ImagePoints
    ''' <summary>
    ''' CT内のSTのLIST
    ''' </summary>
    ''' Public CT_3dPoint As Point3D
    Public lstCTtoST As List(Of SingleTarget)
    ''' <summary>
    ''' CT内のSTの数（Constで4）
    ''' </summary>
    Public Const CTnoSTnum As Integer = 4
    ''' <summary>
    ''' CTが使えるかどうか
    ''' </summary>
    Public flgUsable As Boolean
    ''' <summary>
    ''' 画像上のCTのラベル
    ''' </summary>
    Public CurrentLabel As String
    Public myC3DCT As Common3DCodedTarget

    Public MismatchCT_Name As String


    'SUURI ADD 20150217	CT代表点を近傍STに
    Public AllST_Area As Double

    Public Function GetAllPointsKyoriPixel() As Double
        Dim retKyoriPixel As Double = 0
        Try
            Dim tmpst As SingleTarget = lstCTtoST.Item(0)
            For Each objst As SingleTarget In lstCTtoST
                Dim tmpDist As Double
                tmpst.P2D.CalcDistToInputPoint(objst.P2D.Row, objst.P2D.Col, tmpDist)
                retKyoriPixel += tmpDist
            Next

            Return retKyoriPixel
        Catch ex As Exception
            Return Double.MaxValue
        End Try
    End Function
    'SUURI ADD 20150217
    ''' <summary>
    ''' 初期化
    ''' </summary>
    Public Sub New()
        CT_ID = -1
        If lstCTtoST Is Nothing Then
            lstCTtoST = New List(Of SingleTarget)
        Else
            lstCTtoST.Clear()
        End If
        CT_Points = New ImagePoints
        myC3DCT = Nothing
        '   CT_3dPoint = New Point3D
        ImageID = -1
        flgUsable = False
        CurrentLabel = ""
        AllST_Area = 0
    End Sub

    ''' <summary>
    ''' 画像上のCTの座標値（2次元）
    ''' </summary>
    Public ReadOnly Property CenterPoint() As ImagePoints

        Get
            Dim NewObj As New ImagePoints
            NewObj.Row = BTuple.TupleSelect(CT_Points.Row, 0)
            NewObj.Col = BTuple.TupleSelect(CT_Points.Col, 0)
            Return NewObj
        End Get

    End Property
    ''' <summary>
    ''' 『CT』+ CT_ID
    ''' </summary>
    Public ReadOnly Property systemlabel() As String
        Get
            Return "CT" & CT_ID
        End Get
    End Property
    ''' <summary>
    ''' 未使用
    ''' </summary>
    Public Sub SaveData(ByVal strSavePath As String)
        Dim objSaveTuple As Object = Nothing
        Dim strtmpPath As String = strSavePath & CT_ID
        CT_Points.SaveData(strtmpPath & "_CT_Points")
        '  CT_3dPoint.Save3dPoints(strtmpPath & "_CT_3dPoint")
    End Sub

    Dim strFieldText() As String
    ''' <summary>
    ''' 作成する配列の最大数（14）
    ''' </summary>
    Dim MaxFieldCnt As Integer = 14
    ''' <summary>
    ''' CT内のSTのフィールドを作成する
    ''' </summary>
    Public Sub CreateFieldText(ByVal ind As Integer)
        ReDim strFieldText(MaxFieldCnt)


        strFieldText(0) = ImageID
        strFieldText(1) = CT_ID
        strFieldText(2) = ind
        strFieldText(3) = 1
        'strFieldText(4) = strISDBNULL(CT_Points.Row(ind))
        'strFieldText(5) = strISDBNULL(CT_Points.Col(ind))
        'strFieldText(6) = ""
        'strFieldText(7) = ""
        'strFieldText(8) = ""
        'strFieldText(9) = ""
        'strFieldText(10) = ""
        'strFieldText(11) = ""
        'strFieldText(12) = ""
        'strFieldText(13) = ""
        strFieldText(4) = strISDBNULL(lstCTtoST.Item(ind).P2D.Row.D)
        strFieldText(5) = strISDBNULL(lstCTtoST.Item(ind).P2D.Col.D)
        strFieldText(6) = strISDBNULL(BTuple.GetDouble(lstCTtoST.Item(ind).RayP1.X))
        strFieldText(7) = strISDBNULL(BTuple.GetDouble(lstCTtoST.Item(ind).RayP1.Y))
        strFieldText(8) = strISDBNULL(BTuple.GetDouble(lstCTtoST.Item(ind).RayP1.Z))
        strFieldText(9) = strISDBNULL(BTuple.GetDouble(lstCTtoST.Item(ind).RayP2.X))
        strFieldText(10) = strISDBNULL(BTuple.GetDouble(lstCTtoST.Item(ind).RayP2.Y))
        strFieldText(11) = strISDBNULL(BTuple.GetDouble(lstCTtoST.Item(ind).RayP2.Z))
        strFieldText(12) = strISDBNULL(BTuple.GetDouble(lstCTtoST.Item(ind).Dist))
        strFieldText(13) = strISDBNULL(BTuple.GetDouble(lstCTtoST.Item(ind).ReProjectionError))
        strFieldText(14) = 2
    End Sub
    Private Function strISDBNULL(ByVal F As Object) As String
        strISDBNULL = ""
        If IsDBNull(F) Then
            Exit Function
        End If
        If Not F Is Nothing Then
            strISDBNULL = CStr(F)
        End If
    End Function
    Private Function objISDBNULL(ByVal F As Object) As Object
        objISDBNULL = Nothing
        If IsDBNull(F) Then
            Exit Function
        End If
        If Not F Is Nothing Then
            objISDBNULL = F
        End If
    End Function
    ''' <summary>
    ''' ST(CT内の)測点情報をレコードへ書き込む
    ''' </summary>
    Public Sub SaveData()
        Dim n As Integer = CTnoSTnum
        Dim i As Integer
        For i = 0 To n - 1
            CreateFieldText(i)
            If dbClass.DoInsert(SingleTargetFields, "Targets", strFieldText) < 0 Then
                MsgBox("DB登録に失敗しました。", MsgBoxStyle.OkOnly, "エラー")
                Exit Sub
            End If
        Next

    End Sub
    ''' <summary>
    ''' 未使用
    ''' </summary>
    Public Sub ReadData(ByVal strReadPath As String) 'H25.4.8 未使用（山田）
        Dim objOtherParams As New Object
        Dim strtmpPath As String = strReadPath & CT_ID

        CT_Points.ReadData(strtmpPath & "_CT_Points")
        ' CT_3dPoint.Read3dPoints(strtmpPath & "_CT_3dPoint")
    End Sub
    ''' <summary>
    ''' レコードからCTの測点情報を読み込む
    ''' </summary>
    Public Sub ReadData(ByRef IDR As IDataReader)
        Dim n As Integer = CTnoSTnum
        Dim i As Integer
        ImageID = CInt(IDR.GetValue(0))
        CT_ID = CInt(IDR.GetValue(1))
        flgUsable = CBool(IDR.GetValue(2))
        CT_Points.Row = Nothing
        CT_Points.Col = Nothing
        CT_Points.Row = New HalconDotNet.HTuple
        CT_Points.Col = New HalconDotNet.HTuple
        For i = 0 To n - 1
            Dim R As Object
            Dim C As Object

            R = CDbl(IDR.GetValue(4))
            C = CDbl(IDR.GetValue(5))
            'CT_Points.Row = BTuple.TupleConcat(CT_Points.Row, R)
            'CT_Points.Col = BTuple.TupleConcat(CT_Points.Col, C)
            CT_Points.Row = CT_Points.Row.TupleInsert(i, New HTuple(R))
            CT_Points.Col = CT_Points.Col.TupleInsert(i, New HTuple(C))
            If i < n - 1 Then
                IDR.Read()
            End If
        Next
        SetlstCTtoST()

    End Sub
    Public Sub SetlstCTtoST()
        Dim n As Integer = CInt(CT_Points.CountPoints)
        Dim i As Integer
        If lstCTtoST Is Nothing Then
            lstCTtoST = New List(Of SingleTarget)
        Else
            lstCTtoST.Clear()
        End If

        For i = 0 To n - 1
            Dim ST As New SingleTarget
            ST.P2ID = i + 1
            ST.ImageID = ImageID
            ST.flgUsed = 1
            ST.P2D.Row = CT_Points.Row(i)
            ST.P2D.Col = CT_Points.Col(i)
            If i = 0 Then
                ST.currentLabel = "CT" & CT_ID
            Else
                ST.currentLabel = "CT" & CT_ID & "_" & i + 1
            End If
            lstCTtoST.Add(ST)
        Next
    End Sub
End Class
