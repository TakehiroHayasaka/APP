﻿Public Class SingleTarget
    ''' <summary>
    ''' STの2次元画像上の座標値
    ''' </summary>
    Public P2D As ImagePoints
    Public TP2D As ImagePoints
    ''' <summary>
    ''' STの2次元画像上のID
    ''' </summary>
    Public P2ID As Integer
    ''' <summary>
    ''' STの3次元ID（3D操作ビュー）
    ''' </summary>
    Public P3ID As Integer
    ''' <summary>
    ''' 画像ID
    ''' </summary>
    Public ImageID As Integer
    ''' <summary>
    ''' STが使えるかどうか
    ''' </summary>
    Public flgUsed As Integer = 0
    ''' <summary>
    ''' レイの始点（）
    ''' </summary>
    Public RayP1 As Point3D
    ''' <summary>
    ''' レイの終点（）
    ''' </summary>
    Public RayP2 As Point3D
    ''' <summary>
    ''' rayと3Dポイント間の距離
    ''' </summary>
    Public Dist As Object 'rayと３Dポイントの間の距離
    ''' <summary>
    ''' 再投影のエラー
    ''' </summary>
    Public ReProjectionError As Object
    Public tmpPPP As List(Of SingleTarget)
    ''' <summary>
    ''' 画像上のSTのラベル
    ''' </summary>
    Public currentLabel As String

    '20140217 SUURI ADD
    Public AreaST As Double
    Public Sub New()
        P2D = New ImagePoints
        TP2D = New ImagePoints
        RayP1 = New Point3D
        RayP2 = New Point3D
        flgUsed = 0
        P2ID = -1
        P3ID = -1
        ImageID = -1
        Dist = System.DBNull.Value
        tmpPPP = New List(Of SingleTarget)
        AreaST = 0
    End Sub

    Public Sub New(ByVal ImgID As Integer, ByVal P2ID As Integer, ByVal R As Object, ByVal C As Object)
        P2D = New ImagePoints(R, C)
        TP2D = New ImagePoints
        RayP1 = New Point3D
        RayP2 = New Point3D
        flgUsed = 0
        P2ID = P2ID
        P3ID = -1
        ImageID = ImgID
        Dist = System.DBNull.Value
        tmpPPP = New List(Of SingleTarget)
        AreaST = 0
    End Sub

    Public ReadOnly Property systemlabel() As String
        Get
            If P3ID <> -1 Then
                systemlabel = "ST" & P3ID
            Else
                systemlabel = ""
            End If
        End Get
    End Property


    ''' <summary>
    ''' 未使用
    ''' </summary>
    Public Sub SaveData(ByVal strSavePath As String)
        Dim objSaveTuple As Object = Nothing
        Dim strtmpPath As String = strSavePath & P2ID
        'objSaveTuple = Tuple.TupleGenConst(0, 0)
        ExtendVar(objSaveTuple, 2)
        objSaveTuple.setvalue(P3ID, 0)
        objSaveTuple.setvalue(ImageID, 1)
        objSaveTuple.setvalue(flgUsed, 2)
        SaveTupleObj(objSaveTuple, strtmpPath & "_STIDs.tpl")
        P2D.SaveData(strtmpPath & "_P2D")
        RayP1.Save3dPoints(strtmpPath & "_RayP1")
        RayP2.Save3dPoints(strtmpPath & "_RayP2")
        SaveTupleObj(Dist, strtmpPath & "_Dist.tpl")
        SaveTupleObj(ReProjectionError, strtmpPath & "_ReProjectionError.tpl")

    End Sub


    Dim strFieldText() As String
    ''' <summary>
    ''' 作成するSTの配列の最大数（14）
    ''' </summary>
    Dim MaxFieldCnt As Integer = 14


    Public Sub CreateFieldText()
        ReDim strFieldText(MaxFieldCnt)


        strFieldText(0) = ImageID
        strFieldText(1) = P2ID
        strFieldText(2) = P3ID
        strFieldText(3) = flgUsed
        strFieldText(4) = strISDBNULL(P2D.Row)
        strFieldText(5) = strISDBNULL(P2D.Col)
        strFieldText(6) = strISDBNULL(RayP1.X)
        strFieldText(7) = strISDBNULL(RayP1.Y)
        strFieldText(8) = strISDBNULL(RayP1.Z)
        strFieldText(9) = strISDBNULL(RayP2.X)
        strFieldText(10) = strISDBNULL(RayP2.Y)
        strFieldText(11) = strISDBNULL(RayP2.Z)
        strFieldText(12) = strISDBNULL(Dist)
        strFieldText(13) = strISDBNULL(ReProjectionError)
        strFieldText(14) = 1
    End Sub


    Private Function strISDBNULL(ByVal F As Object) As String
        strISDBNULL = ""
        If F Is Nothing Then
            Exit Function
        End If
        If Not F Is DBNull.Value Then
            strISDBNULL = CStr(F)
        End If
    End Function


    Private Function objISDBNULL(ByVal F As Object) As Object
        objISDBNULL = DBNull.Value
        If F Is Nothing Then
            Exit Function
        End If
        If Not F Is DBNull.Value Then
            objISDBNULL = F
        End If
    End Function


    ''' <summary>
    ''' レコードへSTの測点情報を書き込む
    ''' </summary>
    Public Sub SaveData()
        CreateFieldText()
        If dbClass.DoInsert(SingleTargetFields, "Targets", strFieldText) < 0 Then
            MsgBox("DB登録に失敗しました。", MsgBoxStyle.OkOnly, "エラー")
            Exit Sub
        End If
    End Sub


    Public Sub ReadData(ByVal strReadPath As String)
        Dim objOtherParams As New Object
        Dim strtmpPath As String = strReadPath & P2ID
        If ReadTupleObj(objOtherParams, strtmpPath & "_STIDs.tpl") Then
            P3ID = CInt(Tuple.TupleSelect(objOtherParams, 0))
            ImageID = CInt(Tuple.TupleSelect(objOtherParams, 1))
            flgUsed = CInt(Tuple.TupleSelect(objOtherParams, 2))
        End If
        P2D.ReadData(strtmpPath & "_P2D")
        RayP1.Read3dPoints(strtmpPath & "_RayP1")
        RayP2.Read3dPoints(strtmpPath & "_RayP2")
        ReadTupleObj(Dist, strtmpPath & "_Dist.tpl")
        ReadTupleObj(ReProjectionError, strtmpPath & "_ReProjectionError.tpl")

    End Sub
    ''' <summary>
    ''' レコードからSTの測点情報を読み取る
    ''' </summary>
    Public Sub ReadData(ByRef IDR As IDataReader)

        ImageID = CInt(IDR.GetValue(0))
        P2ID = CInt(IDR.GetValue(1))
        P3ID = CInt(IDR.GetValue(2))
        flgUsed = CInt(IDR.GetValue(3))
        P2D.Row = objISDBNULL(IDR.GetValue(4))
        P2D.Col = objISDBNULL(IDR.GetValue(5))
        RayP1.X = objISDBNULL(IDR.GetValue(6))
        RayP1.Y = objISDBNULL(IDR.GetValue(7))
        RayP1.Z = objISDBNULL(IDR.GetValue(8))
        RayP2.X = objISDBNULL(IDR.GetValue(9))
        RayP2.Y = objISDBNULL(IDR.GetValue(10))
        RayP2.Z = objISDBNULL(IDR.GetValue(11))
        Dist = objISDBNULL(IDR.GetValue(12))
        ReProjectionError = objISDBNULL(IDR.GetValue(13))

    End Sub
    ''' <summary>
    ''' 同じSTがあるかどうか
    ''' </summary>
    Public Function IsSamePoint(ByVal ST As SingleTarget) As Boolean
        IsSamePoint = False
        If ST.P2ID = P2ID And ST.ImageID = ImageID Then
            IsSamePoint = True
        End If
    End Function
End Class
