Imports System.Runtime.InteropServices
Imports HalconDotNet

Public Class MeasurePoint
    Public Pnt As Point3D
    Public strPointLabel As String
    Private _ind1 As Integer
    Private _ind2 As Integer

    Public Property Ind1(ByVal lstImages As List(Of ImageSet)) As Integer
        Get
            Return _ind1
        End Get
        Set(ByVal value As Integer)
            _ind1 = value
            IS1 = lstImages.Item(value)
        End Set
    End Property
    Public Property Ind2(ByVal lstImages As List(Of ImageSet)) As Integer
        Get
            Return _ind2
        End Get
        Set(ByVal value As Integer)
            _ind2 = value
            IS2 = lstImages.Item(value)
        End Set
    End Property

    Public IS1 As ImageSet
    Public IS2 As ImageSet
    Public IS1_ImagePoint As ImagePoints
    Public IS2_ImagePoint As ImagePoints
    Public tmpImagePoint As ImagePoints
    Public Cross1 As HObject = Nothing
    Public Cross2 As HObject = Nothing
    Public flgSingle As Integer
    Public Sub New()
        Pnt = New Point3D
        strPointLabel = ""
        IS1_ImagePoint = New ImagePoints
        IS2_ImagePoint = New ImagePoints
        tmpImagePoint = New ImagePoints
        HOperatorSet.GenEmptyObj(Cross1)
        HOperatorSet.GenEmptyObj(Cross2)
        flgSingle = 0
    End Sub
    Public Sub New(ByRef lstImages As List(Of ImageSet), ByVal index1 As Integer, ByRef index2 As Integer)
        Pnt = New Point3D
        _ind1 = index1
        _ind2 = index2
        IS1 = lstImages.Item(index1)
        IS2 = lstImages.Item(index2)
        IS1_ImagePoint = New ImagePoints
        IS2_ImagePoint = New ImagePoints
        tmpImagePoint = New ImagePoints
        strPointLabel = ""
        HOperatorSet.GenEmptyObj(Cross1)
        HOperatorSet.GenEmptyObj(Cross2)
        flgSingle = 0
    End Sub
    Public Function Calc3dCoord(ByVal CamPar As Object) As Boolean
        Dim Dist As New Object
        Dim HomMat3d As New Object

        Try

            HOperatorSet.IntersectLinesOfSight(CamPar, CamPar, IS2.VectorPose.Pose, IS1_ImagePoint.Row, IS1_ImagePoint.Col, _
               IS2_ImagePoint.Row, IS2_ImagePoint.Col, Pnt.X, Pnt.Y, Pnt.Z, Dist)
            Pnt.SetScale(IS2.CommonScale)

            'HOperatorSet.AffineTransPoint3D(IS2.CommonHomMat3d, Pnt.X, Pnt.Y, Pnt.Z, Pnt.X, Pnt.Y, Pnt.Z)

            HOperatorSet.PoseToHomMat3d(IS1.ImagePose.Pose, HomMat3d)

            HOperatorSet.AffineTransPoint3d(HomMat3d, Pnt.X, Pnt.Y, Pnt.Z, Pnt.X, Pnt.Y, Pnt.Z)

        Catch ex As Exception
            Calc3dCoord = False
            Exit Function
        End Try

        Calc3dCoord = True

    End Function
    Public Sub GenCross()
        On Error Resume Next
        HOperatorSet.ClearObj(Cross1)
        HOperatorSet.ClearObj(Cross2)
        HOperatorSet.GenCrossContourXld(Cross1, IS1_ImagePoint.Row, IS1_ImagePoint.Col, CrossSize, CrossAngle)
        HOperatorSet.GenCrossContourXld(Cross2, IS2_ImagePoint.Row, IS2_ImagePoint.Col, CrossSize, CrossAngle)
    End Sub

    Public Sub SaveMeasurePoint(ByVal strPath As String, ByVal index As Integer)
        Dim strFullPath As String
        strFullPath = strPath & "3DPoints\" & "3DP_" & index
        Dim folderExists As Boolean
        folderExists = My.Computer.FileSystem.DirectoryExists(strPath & "3DPoints\")
        If folderExists = False Then
            My.Computer.FileSystem.CreateDirectory(strPath & "3DPoints\")
        End If

        Pnt.Save3dPoints(strFullPath)
        IS1_ImagePoint.SaveData(strFullPath & "_IS1")
        IS2_ImagePoint.SaveData(strFullPath & "_IS2")
        Dim objSaveTuple As Object = Nothing
        ExtendVar(objSaveTuple, 2)
        objSaveTuple.setvalue(strPointLabel, 0)
        objSaveTuple.setvalue(_ind1, 1)
        objSaveTuple.setvalue(_ind2, 2)
        SaveTupleObj(objSaveTuple, strFullPath & "_OtherParams.tpl")

    End Sub

    Public Sub ReadMeasurePoint(ByVal strPath As String, ByVal index As Integer, ByRef lstImages As List(Of ImageSet))
        Dim strFullPath As String
        strFullPath = strPath & "3DPoints\" & "3DP_" & index
        Dim folderExists As Boolean
        folderExists = My.Computer.FileSystem.DirectoryExists(strPath & "3DPoints\")
        If folderExists = False Then
            Exit Sub
        End If
        Pnt.Read3dPoints(strFullPath)
        IS1_ImagePoint.ReadData(strFullPath & "_IS1")
        IS2_ImagePoint.ReadData(strFullPath & "_IS2")
        Dim objReadTuple As Object = Nothing
        ReadTupleObj(objReadTuple, strFullPath & "_OtherParams.tpl")
        strPointLabel = CStr(BTuple.TupleSelect(objReadTuple, 0))
        Ind1(lstImages) = CInt(BTuple.TupleSelect(objReadTuple, 1))
        Ind2(lstImages) = CInt(BTuple.TupleSelect(objReadTuple, 2))
        GenCross()

        '_ind1 = index1
        '_ind2 = index2
        'IS1 = lstImages.Item(index1)
        'IS2 = lstImages.Item(index2)

    End Sub

    Dim strFieldNames() As String
    Dim strFieldText() As String
    Dim MaxFieldCnt As Integer = 11

    Private Sub CreateFieldName()
        ReDim strFieldNames(MaxFieldCnt)

        strFieldNames(0) = "P_ID"
        strFieldNames(1) = "Image1_id"
        strFieldNames(2) = "Image2_id"
        strFieldNames(3) = "P_Name"
        strFieldNames(4) = "Row1"
        strFieldNames(5) = "Col1"
        strFieldNames(6) = "Row2"
        strFieldNames(7) = "Col2"
        strFieldNames(8) = "flg_Single"
        strFieldNames(9) = "X"
        strFieldNames(10) = "Y"
        strFieldNames(11) = "Z"

    End Sub

    Public Sub CreateFieldText(ByVal index As Integer, ByVal dblScale As Double)
        ReDim strFieldText(MaxFieldCnt)

        CreateFieldName()

        strFieldText(0) = index
        strFieldText(1) = _ind1
        strFieldText(2) = _ind2
        strFieldText(3) = "'" & strPointLabel & "'"
        strFieldText(4) = IS1_ImagePoint.Row
        strFieldText(5) = IS1_ImagePoint.Col
        strFieldText(6) = IS2_ImagePoint.Row
        strFieldText(7) = IS2_ImagePoint.Col
        strFieldText(8) = flgSingle
        strFieldText(9) = Pnt.X * dblScale
        strFieldText(10) = Pnt.Y * dblScale
        strFieldText(11) = Pnt.Z * dblScale

    End Sub

    Public Sub SaveMP_toDb(ByVal index As Integer, ByVal dblScale As Double, ByVal TCS_HomMat3d As Object)
        Dim HomMam3dInvert As New Object
        HOperatorSet.AffineTransPoint3d(TCS_HomMat3d, Pnt.X, Pnt.Y, Pnt.Z, Pnt.X, Pnt.Y, Pnt.Z)
        CreateFieldText(index, dblScale)
        HOperatorSet.HomMat3dInvert(TCS_HomMat3d, HomMam3dInvert)
        HOperatorSet.AffineTransPoint3d(HomMam3dInvert, Pnt.X, Pnt.Y, Pnt.Z, Pnt.X, Pnt.Y, Pnt.Z)
        If dbClass.DoInsert(strFieldNames, "3DPoints", strFieldText) < 0 Then
            MsgBox("DB登録に失敗しました。", MsgBoxStyle.OkOnly, "エラー")
            Exit Sub
        End If

    End Sub
End Class