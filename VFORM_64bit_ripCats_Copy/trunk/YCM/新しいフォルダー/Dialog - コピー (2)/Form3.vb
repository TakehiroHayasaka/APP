
Imports System
Imports Microsoft.VisualBasic
Imports HalconDotNet

Public Class Form3
    Dim hv_AcqHandle As Object
    Dim winID As Object
    ' Local iconic variables 
    Dim ho_Image As HObject

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        winID = ImageWindow.HalconID
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        connectCamera()

        Do
            Call HOperatorSet.GrabImageAsync(ho_Image, hv_AcqHandle, -1)
            HOperatorSet.DispObj(ho_Image, winID)
            DispatcherHelper.DoEvents()
        Loop

    End Sub

    Private Sub connectCamera()
        Try
            'open first uEye camera:
            HOperatorSet.OpenFramegrabber("uEye", 1, 1, 0, 0, 0, 0, "default", -1, "default", -1,
                "default", "default", "default", -1, -1, hv_AcqHandle)
        Catch ex As Exception
            Debug.Print(ex.Message)

        End Try


    End Sub


End Class