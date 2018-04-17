Imports System.Runtime.InteropServices
Imports HalconDotNet

Public Class RansacPoints
    Public RansacPoints As ImagePoints
    Public RansacPointsIndex As Object
    Public hx_RansacCross As HObject
    Public Sub New()
        RansacPoints = New ImagePoints
        RansacPointsIndex = Nothing
        hx_RansacCross = Nothing
        HOperatorSet.GenEmptyObj(hx_RansacCross)

    End Sub
    Public Sub SubSet(ByRef FeaturePoints As Object)
        RansacPoints.Row = BTuple.TupleSelect(FeaturePoints.Row, RansacPointsIndex)
        RansacPoints.Col = BTuple.TupleSelect(FeaturePoints.Col, RansacPointsIndex)
        HOperatorSet.ClearObj(hx_RansacCross)
        HOperatorSet.GenCrossContourXld(hx_RansacCross, RansacPoints.Row, RansacPoints.Col, CrossSize, CrossAngle)
    End Sub
    Public Sub IndexClear()
        ' RansacPointsIndex = BTuple.TupleGenConst(0, 0)
        RansacPointsIndex = Nothing
    End Sub
    Public Function GetCross() As HObject
        GetCross = hx_RansacCross.CopyObj(1, -1)
    End Function

    Public Sub SaveData(ByVal strPath As String)
        SaveTupleObj(RansacPointsIndex, strPath & "_RansacPointsIndex.tpl")
        RansacPoints.SaveData(strPath & "_RansacPoints")
    End Sub
    Public Sub ReadData(ByVal StrPath As String)
        On Error Resume Next
        ReadTupleObj(RansacPointsIndex, StrPath & "_RansacPointsIndex.tpl")
        RansacPoints.ReadData(StrPath & "_RansacPoints")
        HOperatorSet.ClearObj(hx_RansacCross)
        HOperatorSet.GenCrossContourXld(hx_RansacCross, RansacPoints.Row, RansacPoints.Col, CrossSize, CrossAngle)
    End Sub

End Class