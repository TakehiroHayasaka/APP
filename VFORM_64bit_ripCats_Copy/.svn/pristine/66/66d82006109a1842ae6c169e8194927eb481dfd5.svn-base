Public Class TrgtDtctDebugWin
    Dim ZoomFactor As Integer = 1.5  '--10
    Dim minZoomSize As Integer = 5 '最小表示範囲ＰＩＸＥＬ
    Dim flgDraw As Boolean = False

    '( 20150805 Kiryu Add ) 解析画面ボタン操作
    Dim Center_x As Integer = 3840 / 2  '(解析画像のセンター座標初期値)
    Dim Center_y As Integer = 2748 / 2  '(解析画像のセンター座標初期値)
    Const MoveVal As Integer = 100


    Dim intW As Integer = 3840
    Dim intH As Integer = 2748

    Public Sub Disp(ByVal ho_Image As HALCONXLib.HUntypedObjectX, ByRef Hwin As Object)
        Op.DispImage(ho_Image, AxHWindowXCtrl1.HalconWindow.HalconID)
    End Sub

    Public Sub DispReg(ByVal ho_reg As HALCONXLib.HUntypedObjectX)
        Op.SetColor(AxHWindowXCtrl1.HalconWindow.HalconID, "red")
        Op.DispRegion(ho_reg, AxHWindowXCtrl1.HalconWindow.HalconID)
    End Sub

    Public Sub SetTitle(ByVal ImageID As Integer, ByVal TT As Integer)
        Me.Text = ("ImgID:" & ImageID.ToString & " TT:" & TT.ToString)
    End Sub


    Private Function ResizePart(ByVal win As Object, ByVal Delta As Integer) As Boolean
        Dim R As Object = Nothing, C As Object = Nothing
        Dim Rw1 As Object = Nothing, Cw1 As Object = Nothing
        Dim Rw2 As Object = Nothing, Cw2 As Object = Nothing
        Dim Rd1 As Object = Nothing, Cd1 As Object = Nothing
        Dim Rd2 As Object = Nothing, Cd2 As Object = Nothing

        Dim ZF As Double
        Try

            Dim B As Object = Nothing
            Op.GetMposition(win, R, C, B)

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

    Private Sub FrmMain_MouseWheel(sender As Object, e As MouseEventArgs) Handles Me.MouseWheel

        ResizePart(AxHWindowXCtrl1.HalconWindow.HalconID, e.Delta)

    End Sub


End Class