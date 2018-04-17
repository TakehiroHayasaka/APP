Imports System.Runtime.InteropServices

Public Class FrmKaiseki

    Dim hwinid1 As Object
    Dim hwinid2 As Object
    Dim hvWinPrev As Object

    Dim intW As Integer
    Dim intH As Integer
    Dim intZW As Integer
    Dim intZH As Integer

    Dim ZoomFactor As Integer = 1.5
    Dim minZoomSize As Integer = 5
    Dim flgDraw As Boolean = False

    Dim Center_x As Integer = 3840 / 2
    Dim Center_y As Integer = 2748 / 2
    Const MoveVal As Integer = 100

    Dim lst_AcqHandle As List(Of Object)
    Public lstCamParam As List(Of CameraParam)

    Public PrevImageCid As Integer

    Dim AllTargetRegion1 As HALCONXLib.HUntypedObjectX = Nothing
    Dim AllTargetRegion2 As HALCONXLib.HUntypedObjectX = Nothing
    'Dim AllTargetList1 As List(Of TargetDetect)
    'Dim Alltargetlist2 As List(Of TargetDetect)

    Private Sub FrmKaiseki_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        intH = 2748
        intW = 3840

        Op.SetPart(AxHWinPrev.HalconWindow.HalconID, 0, 0, intH, intW)
        hvWinPrev = AxHWinPrev.HalconWindow.HalconID
        DispKaisekiView()

        'Me.Focus = True

    End Sub

    Private Sub FrmKaiseki_MouseWheel(sender As Object, e As MouseEventArgs) Handles Me.MouseWheel
        If ResizePart(hvWinPrev, e.Delta) = True Then
            Dim objfirstCam As CameraParam = Nothing

            For Each objCamparam As CameraParam In lstCamParam
                If (Not objCamparam.Fr Is Nothing) Then
                    If PrevImageCid = objCamparam.Cid Then
                        objfirstCam = objCamparam
                        Exit For
                    End If
                End If
            Next
            If objfirstCam Is Nothing Then
                Exit Sub
            End If
            If Not objfirstCam.hvImageResult Is Nothing Then
                Op.DispImage(objfirstCam.hvImageResult, hvWinPrev)
                DispCT(hvWinPrev, objfirstCam.objResultTarget)
            End If

        End If
    End Sub

    Private Sub btnZoomIn_Click(sender As Object, e As EventArgs) Handles btnZoomIn.Click
        BtnReSizePart(1)
        DispKaisekiView()
    End Sub

    Private Sub btnZoomOut_Click(sender As Object, e As EventArgs) Handles btnZoomOut.Click
        BtnReSizePart(-1)
        DispKaisekiView()
    End Sub

    Private Sub btnLeft_Click(sender As Object, e As EventArgs) Handles btnLeft.Click
        MovePart(-MoveVal, 0)
        DispKaisekiView()
    End Sub

    Private Sub btnRigth_Click(sender As Object, e As EventArgs) Handles btnRigth.Click
        MovePart(MoveVal, 0)
        DispKaisekiView()
    End Sub

    Private Sub btnUp_Click(sender As Object, e As EventArgs) Handles btnUp.Click
        MovePart(0, -MoveVal)
        DispKaisekiView()
    End Sub

    Private Sub btnDown_Click(sender As Object, e As EventArgs) Handles btnDown.Click
        MovePart(0, MoveVal)
        DispKaisekiView()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Function ResizePart(ByRef win As Object, ByVal Delta As Integer) As Boolean
        Dim R As Object = Nothing, C As Object = Nothing
        Dim Rw1 As Object = Nothing, Cw1 As Object = Nothing
        Dim Rw2 As Object = Nothing, Cw2 As Object = Nothing
        Dim Rd1 As Object = Nothing, Cd1 As Object = Nothing
        Dim Rd2 As Object = Nothing, Cd2 As Object = Nothing

        Dim ZF As Double
        Try

            Dim B As Object = Nothing
            Op.GetMposition(win, R, C, B)
            '( 20150820 Kiryu Add )ズーム位置を保持
            Center_x = C
            Center_y = R
            'Kiryu Add End
            Op.GetPart(win, Rw1, Cw1, Rw2, Cw2)
            If (Delta > 0) Then
                ZF = 1 / ZoomFactor
            Else
                ZF = ZoomFactor
            End If

            Cd1 = C - (C - Cw1) * ZF
            Cd2 = C + (Cw2 - C) * ZF
            Rd1 = R - (R - Rw1) * ZF
            Rd2 = Rd1 + CInt(Math.Abs(Cd2 - Cd1) * (intH / intW))

            If (Rd1 < 0) Then Rd1 = 0
            If (Cd1 < 0) Then Cd1 = 0
            If (Rd2 > intH) Then Rd2 = intH '-ZH
            If (Cd2 > intW) Then Cd2 = intW '-ZW
            If (Math.Abs(Rd1 - Rd2) > minZoomSize And Math.Abs(Cd1 - Cd2) > minZoomSize) Then
                '--表示領域の再設定

                Op.SetPart(win, Rd1, Cd1, Rd2, Cd2)
            End If

            ' Op.DispObj(hImage, win)
            ResizePart = True
        Catch ex As Exception
            ResizePart = False
        End Try
    End Function
    Private Function BtnReSizePart(ByVal ZoomBtn)
        Dim R As Object = Nothing, C As Object = Nothing
        Dim Rw1 As Object = Nothing, Cw1 As Object = Nothing
        Dim Rw2 As Object = Nothing, Cw2 As Object = Nothing
        Dim Rd1 As Object = Nothing, Cd1 As Object = Nothing
        Dim Rd2 As Object = Nothing, Cd2 As Object = Nothing

        Dim ZF As Double
        Try

            Op.GetPart(hvWinPrev, Rw1, Cw1, Rw2, Cw2)
            C = Center_x
            R = Center_y

            If (ZoomBtn > 0) Then
                ZF = 1 / ZoomFactor
            Else
                ZF = ZoomFactor
            End If

            Cd1 = C - (C - Cw1) * ZF
            Cd2 = C + (Cw2 - C) * ZF
            Rd1 = R - (R - Rw1) * ZF
            Rd2 = Rd1 + CInt(Math.Abs(Cd2 - Cd1) * (intH / intW))

            If (Rd1 < 0) Then Rd1 = 0
            If (Cd1 < 0) Then Cd1 = 0
            If (Rd2 > intH) Then Rd2 = intH '-ZH
            If (Cd2 > intW) Then Cd2 = intW '-ZW
            If (Math.Abs(Rd1 - Rd2) > minZoomSize And Math.Abs(Cd1 - Cd2) > minZoomSize) Then
                '--表示領域の再設定

                Op.SetPart(hvWinPrev, Rd1, Cd1, Rd2, Cd2)
            End If

            ' Op.DispObj(hImage, win)
            BtnReSizePart = True

        Catch ex As Exception
            BtnReSizePart = False
        End Try

    End Function
    Private Sub DispKaisekiView()
        Dim objfirstCam As CameraParam = Nothing

        For Each objCamparam As CameraParam In lstCamParam
            ''If objCamparam.objCheckBox.Checked = True And (Not objCamparam.Fr Is Nothing) Then
            If PrevImageCid = objCamparam.Cid Then
                objfirstCam = objCamparam
                Exit For
            End If
            ''End If
        Next
        If objfirstCam Is Nothing Then
            Exit Sub
        End If
        If Not objfirstCam.hvImageResult Is Nothing Then
            Op.DispImage(objfirstCam.hvImageResult, hvWinPrev)
            DispCT(hvWinPrev, objfirstCam.objResultTarget)
        End If

    End Sub
    Public Sub DispCT(ByVal win As Object, ByVal DispTarget As TargetDetect)

        Dim obj As New HALCONXLib.HUntypedObjectX
        Op.GenEmptyObj(obj)
        Op.SetFont(win, "-ＭＳ 明朝-12-")
        Op.SetDraw(win, "margin")
        Op.SetColored(win, 12)

        Dim TD As TargetDetect = DispTarget
        Try
            If win = hwinid1 Then
                Op.DispObj(AllTargetRegion1, win)
            End If
            If win = hwinid2 Then
                Op.DispObj(AllTargetRegion2, win)
            End If
        Catch ex As Exception

        End Try

        For Each CT As CodedTarget In TD.lstCT
            Marshal.ReleaseComObject(obj)

            Op.GenCrossContourXld(obj, CT.CenterPoint.Row, CT.CenterPoint.Col, CrossSize, CrossAngle)
            Op.DispObj(obj, win)

            Op.SetTposition(win, CT.CenterPoint.Row, CT.CenterPoint.Col)

            If CT.CurrentLabel = "" Then

                Op.WriteString(win, CT.systemlabel)
            Else
                Op.WriteString(win, CT.CurrentLabel)
            End If

        Next

        Op.SetColor(win, "blue")

        For Each ST As SingleTarget In TD.lstST

            Marshal.ReleaseComObject(obj)
            Op.GenCrossContourXld(obj, ST.P2D.Row, ST.P2D.Col, CrossSize, CrossAngle)
            Op.DispObj(obj, win)
            If ST.P3ID <> -1 Then

                Op.SetTposition(win, ST.P2D.Row, ST.P2D.Col)

                If ST.currentLabel = "" Then
                    Op.WriteString(win, ST.systemlabel)
                Else
                    Op.WriteString(win, ST.currentLabel)
                End If

            End If
        Next
        Marshal.ReleaseComObject(obj)

    End Sub
    Private Function MovePart(ByVal HorizontalCnt, ByVal VerticalCnt)
        Dim R As Object = Nothing, C As Object = Nothing
        Dim Rw1 As Object = Nothing, Cw1 As Object = Nothing
        Dim Rw2 As Object = Nothing, Cw2 As Object = Nothing
        Dim Rd1 As Object = Nothing, Cd1 As Object = Nothing
        Dim Rd2 As Object = Nothing, Cd2 As Object = Nothing

        Try
            Op.GetPart(hvWinPrev, Rw1, Cw1, Rw2, Cw2)
            C = Center_x
            R = Center_y
            Center_x = C + HorizontalCnt
            Center_y = R + VerticalCnt

            Cd1 = Cw1 + HorizontalCnt
            Cd2 = Cw2 + HorizontalCnt
            Rd1 = Rw1 + VerticalCnt
            Rd2 = Rw2 + VerticalCnt

            If (Rd1 < 0 Or Rd2 > intH) Then
                Rd1 = Rw1
                Rd2 = Rw2
                Center_y = R
            End If

            If (Cd1 < 0 Or Cd2 > intW) Then
                Cd1 = Cw1
                Cd2 = Cw2
                Center_x = C
            End If

            If (Math.Abs(Rd1 - Rd2) > minZoomSize And Math.Abs(Cd1 - Cd2) > minZoomSize) Then
                '--表示領域の再設定

                Op.SetPart(hvWinPrev, Rd1, Cd1, Rd2, Cd2)
            End If

            ' Op.DispObj(hImage, win)
            MovePart = True

        Catch ex As Exception
            MovePart = False
        End Try

    End Function





End Class