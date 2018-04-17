Imports System
Imports System.Threading
Imports FBMlib

Public Class IOLib
    Private Declare Function GetCursorPos Lib "user32" (ByRef Po As Point) As Long
    Public t_Cmd As Thread
    Private textCmd As New TextBox
    Private textPrompt As New ListBox
    Public blnPickPoint As Boolean
    Public pick_P As New CLookPoint
    Public pick_CC As New Ccircle
    Public blnThreadRun As Boolean
    Public blnSelectMode As Boolean
    Public binselectmode_camer As Boolean
    Public strStrValue As String
    Public pick_C As New CCamera
    Public binPickCamer As Boolean
    Public binpickfram As Boolean
    Public blnSelectMode_fram As Boolean
    Public pick_F As New GeoPoint

    Public activeSunpoSet As SunpoSetTable

    Public Sub New(ByRef comObject1 As TextBox, ByRef comObject2 As ListBox)
        textCmd = comObject1
        textPrompt = comObject2
        blnThreadRun = False
        blnSelectMode = False
    End Sub
    Public Sub RunCommand()

        Dim strCommand As String = ""
        textPrompt.Items.Add(textCmd.Text)

        If Len(textCmd.Text) > 1 Then
            strCommand = Right(textCmd.Text, Len(textCmd.Text) - 1)
        End If
        textCmd.Text = ":"
        textCmd.SelectionStart = Len(textCmd.Text)
        textPrompt.SelectedIndex = textPrompt.Items.Count - 1
        If strCommand <> "" Then
            LibCommand(strCommand)
        End If
    End Sub
    Public Sub WritePrompt(ByVal strMessage As String) 'メッセージ領域
        'textPrompt.Items.Add(":" & strMessage)
        MainFrm.Histori_text(":" & strMessage)
    End Sub

    Public Sub WriteCommandLine(ByVal strMessage As String) 'コマンド入力領域
        'MainFrm.Btn_text(":" & strMessage)'13.1.16以前

        MainFrm.Btn_text(strMessage) '13.1.16
    End Sub
    Public Function EndThread() As Integer
        EndThread = 0
        'textPrompt.Items.Add(":" & strMessage)
        If Not t_Cmd Is Nothing Then
            If t_Cmd.IsAlive Then
                t_Cmd.Abort()
                If textCmd.Text <> ":" Then
                    textPrompt.Items.Add(textCmd.Text) 'textPrompt：メッセージ領域やコマンド入力領域に表示するText
                    textCmd.Text = ":"
                    textCmd.SelectionStart = Len(textCmd.Text)
                    textPrompt.SelectedIndex = textPrompt.Items.Count - 1
                End If
            End If
        End If
    End Function

    Public Sub ClearAllPrompt(ByVal strMessage As String)
        textPrompt.Items.Clear()
    End Sub
    Public Sub LibCommand(ByVal strCmd As String)
        IOUtil.EndThread()
        Select Case strCmd
            Case "setscale" 'スケール設定

                If binfirstopen = True Then
                    blnThreadRun = True
                    t_Cmd = New Thread(AddressOf Command_SetScale)
                    t_Cmd.Start()
                Else
                    MsgB_neworopen()
                End If

            Case "open" '開く
                Command_Open()
            Case "new"
                Command_new() '新規作成
            Case "save"
                If binfirstopen = True Then
                    Command_save()
                Else
                    MsgB_neworopen()
                End If

            Case "saveas" '名前をつけて保存

                If binfirstopen = True Then
                    Command_saveas()
                Else
                    MsgB_neworopen()
                End If

            Case "dist"
                If binfirstopen = True Then
                    blnThreadRun = True
                    t_Cmd = New Thread(AddressOf Command_distance2Point)
                    t_Cmd.Start()
                Else
                    MsgB_neworopen()
                End If

            Case "setcoord"
                If binfirstopen = True Then
                    blnThreadRun = True
                    t_Cmd = New Thread(AddressOf Command_SetChange)
                    t_Cmd.Start()
                Else
                    MsgB_neworopen()
                End If

            Case "drawset"
                If binfirstopen = True Then
                    Command_DrawSetting()
                Else
                    MsgB_neworopen()
                End If

            Case "outcsv"
                If binfirstopen = True Then
                    Command_CSVOut()
                Else
                    MsgB_neworopen()
                End If

            Case "label" '自動ラベリング
                If binfirstopen = True Then
                    blnThreadRun = True
                    t_Cmd = New Thread(AddressOf Command_label)
                    t_Cmd.SetApartmentState(System.Threading.ApartmentState.STA)
                    t_Cmd.Start()
                Else
                    MsgB_neworopen()
                End If

            Case "rotate"
                If binfirstopen = True Then
                    Command_Rotate()
                Else
                    MsgB_neworopen()
                End If

            Case "move"
                If binfirstopen = True Then
                    Command_Move()
                Else
                    MsgB_neworopen()
                End If

            Case "changelabel" '手動ラベリング
                If binfirstopen = True Then
                    blnThreadRun = True
                    t_Cmd = New Thread(AddressOf command_ChangeLabel)
                    t_Cmd.Start()
                Else
                    MsgB_neworopen()
                End If

            Case "userline"  '--drawline"  ユーザ線分の作成
                If binfirstopen = True Then
                    blnThreadRun = True
                    t_Cmd = New Thread(AddressOf Command_UserLine)  '--DrawLine)
                    t_Cmd.Start()
                Else
                    MsgB_neworopen()
                End If
            Case "circle1p"
                If binfirstopen = True Then
                    blnThreadRun = True
                    t_Cmd = New Thread(AddressOf Command_Circle1p)
                    t_Cmd.Start()
                Else
                    MsgB_neworopen()
                End If
            Case "circle3p"
                If binfirstopen = True Then
                    blnThreadRun = True
                    t_Cmd = New Thread(AddressOf Command_Circle3p)
                    t_Cmd.Start()
                Else
                    MsgB_neworopen()
                End If


            Case "XY"
                If binfirstopen = True Then
                    Command_XY()
                Else
                    MsgB_neworopen()
                End If

            Case "XZ"
                If binfirstopen = True Then
                    Command_XZ()
                Else
                    MsgB_neworopen()
                End If

            Case "YZ"
                If binfirstopen = True Then
                    Command_YZ()
                Else
                    MsgB_neworopen()
                End If

            Case "camer"
                If binfirstopen = True Then
                    blnThreadRun = True
                    t_Cmd = New Thread(AddressOf Command_camer)
                    t_Cmd.Start()
                Else
                    MsgB_neworopen()
                End If

            Case "area"
                If binfirstopen = True Then
                    blnThreadRun = True
                    t_Cmd = New Thread(AddressOf Command_area)
                    t_Cmd.Start()
                Else
                    MsgB_neworopen()
                End If

            Case "RunCAD"  ' CADの起動

                If binfirstopen = True Then
                    'blnThreadRun = True
                    't_Cmd = New Thread(AddressOf Command_RunCAD)
                    't_Cmd.Start()
                    Call Command_RunCAD()
                Else
                    MsgB_neworopen()
                End If
            Case "importCAD"  ' CAD図形の読込み
                If binfirstopen = True Then
                    'blnThreadRun = True
                    't_Cmd = New Thread(AddressOf Command_ImportCADElements)
                    't_Cmd.Start()
                    Call Command_ImportCADElements()
                Else
                    MsgB_neworopen()
                End If
            Case "exportCAD"  ' CAD図形の書出し

                If binfirstopen = True Then
                    'blnThreadRun = True
                    't_Cmd = New Thread(AddressOf Command_ExportCADElements)
                    't_Cmd.Start()
                    ' Call Command_ExportCADElements()
                Else
                    MsgB_neworopen()
                End If
            Case "setCADcoord"  ' CAD図形との3点マッチング
                If binfirstopen = True Then
                    blnThreadRun = True
                    t_Cmd = New Thread(AddressOf Command_setCADCoord)
                    t_Cmd.Start()
                Else
                    MsgB_neworopen()
                End If
            Case "delete"
                If binfirstopen = True Then
                    blnThreadRun = True
                    t_Cmd = New Thread(AddressOf Command_Delete)
                    t_Cmd.Start()
                    'Command_Delete()
                Else
                    MsgB_neworopen()
                End If

                '========================================移動点の作成関連 
            Case "newpointbyvec"
                '	・移動方向を2点で指定して移動点を作成
                '		command_NewPointByVec()
                If binfirstopen = True Then
                    blnThreadRun = True
                    t_Cmd = New Thread(AddressOf command_NewPointByVec)
                    t_Cmd.Start()
                Else
                    MsgB_neworopen()
                End If
            Case "newpointbyface"
                '   ・移動方向を3点で構成する面で指定して移動点を作成
                '		command_NewPointByFace()
                If binfirstopen = True Then
                    blnThreadRun = True
                    t_Cmd = New Thread(AddressOf command_NewPointByFace)
                    t_Cmd.Start()
                Else
                    MsgB_neworopen()
                End If
            Case "neweachpoint"
                '   ・2点を指定して相互に移動した点を作成
                '		command_NewEachPoint()
                If binfirstopen = True Then
                    blnThreadRun = True
                    t_Cmd = New Thread(AddressOf command_NewEachPoint)
                    t_Cmd.Start()
                Else
                    MsgB_neworopen()
                End If

            Case "newpointcenter"
                '   ・2点を指定して中間点を作成
                If binfirstopen = True Then
                    blnThreadRun = True
                    t_Cmd = New Thread(AddressOf command_NewPointCenter)
                    t_Cmd.Start()
                Else
                    MsgB_neworopen()
                End If

            Case "newpointbylist"
                '   ・リストで指定して移動点を一括作成
                '		command_NewPointByList()
                If binfirstopen = True Then
                    '--rep.12.8.28                    command_NewPointByList()
                    command_CreateMovePos()
                Else
                    MsgB_neworopen()
                End If
            Case "NinniTenShitei"
                If binfirstopen = True Then
                    blnThreadRun = True
                    t_Cmd = New Thread(AddressOf CommandSunpoSetHenkou)
                    t_Cmd.Start()
                Else
                    MsgB_neworopen()
                End If
            Case "AutoOffsetCalc"
                If binfirstopen = True Then
                    blnThreadRun = True
                    t_Cmd = New Thread(AddressOf DispAutoOffsetCalc)
                    t_Cmd.SetApartmentState(ApartmentState.STA)
                    t_Cmd.Start()
                Else
                    MsgB_neworopen()
                End If
            Case "CreateTargetPoint3d"
                If binfirstopen = True Then
                    blnThreadRun = True
                    t_Cmd = New Thread(AddressOf CreateTargetPoint3D)
                    t_Cmd.SetApartmentState(ApartmentState.STA)
                    t_Cmd.Start()
                Else
                    MsgB_neworopen()
                End If
            Case "CreateClickPoint3d"
                If binfirstopen = True Then
                    blnThreadRun = True
                    t_Cmd = New Thread(AddressOf CreateClickPoint3D)
                    t_Cmd.SetApartmentState(ApartmentState.STA)
                    t_Cmd.Start()
                Else
                    MsgB_neworopen()
                End If
            Case "EdgeToPoint3d"
                If binfirstopen = True Then
                    blnThreadRun = True
                    t_Cmd = New Thread(AddressOf EdgeToPoint3d)
                    t_Cmd.SetApartmentState(ApartmentState.STA)
                    t_Cmd.Start()
                Else
                    MsgB_neworopen()
                End If
            Case "DeletePoint3ds"
                If binfirstopen = True Then
                    blnThreadRun = True
                    t_Cmd = New Thread(AddressOf DeletePoint3Ds)
                    t_Cmd.SetApartmentState(ApartmentState.STA)
                    t_Cmd.Start()
                Else
                    MsgB_neworopen()
                End If
                '20170224 baluu add start
            Case "reconstruct"
                If binfirstopen = True Then
                    blnThreadRun = True
                    t_Cmd = New Thread(AddressOf Command_reconstruct)
                    t_Cmd.SetApartmentState(ApartmentState.STA)
                    t_Cmd.Start()
                Else
                    MsgB_neworopen()
                End If
                '20170224 baluu add end

                'Add Kiryu
            Case "reconstruct"
                If binfirstopen = True Then
                    blnThreadRun = True
                    t_Cmd = New Thread(AddressOf Command_reconstruct)
                    t_Cmd.SetApartmentState(ApartmentState.STA)
                    t_Cmd.Start()
                Else
                    MsgB_neworopen()
                End If
            Case "reconstruct"
                If binfirstopen = True Then
                    blnThreadRun = True
                    t_Cmd = New Thread(AddressOf Command_reconstruct)
                    t_Cmd.SetApartmentState(ApartmentState.STA)
                    t_Cmd.Start()
                Else
                    MsgB_neworopen()
                End If
            Case "reconstruct"
                If binfirstopen = True Then
                    blnThreadRun = True
                    t_Cmd = New Thread(AddressOf Command_reconstruct)
                    t_Cmd.SetApartmentState(ApartmentState.STA)
                    t_Cmd.Start()
                Else
                    MsgB_neworopen()

                End If


            Case Else
                textPrompt.Items.Add("そのようなコマンドがありません。")
                'textPrompt.SelectionMode = SelectionMode.One
                textPrompt.SelectedIndex = textPrompt.Items.Count - 1
                'textPrompt.SelectionMode = SelectionMode.None
        End Select

    End Sub
    'Public Function Getpoint_Only()
    '    Dim cp1 As New CLookPoint
    '    blnThreadRun = True
    '    t_Cmd = New Thread(AddressOf Command_Getpoint)
    '    t_Cmd.Start()
    '    't_Cmd.Join()
    'End Function
    Public Function GetPoint(ByRef pickPoint As CLookPoint, ByVal strPrompt As String) As Integer
        blnPickPoint = False
        blnSelectMode = True
        ''pickPoint.mid = -1
        ''pickPoint.tid = -1
        ''pickPoint.x = 0.0#
        ''pickPoint.y = 0.0#
        ''pickPoint.z = 0.0#
        WriteCommandLine(strPrompt)
        '20130117 SUURI ADD
        MainFrm.flgSelectC3D = True

        While True
            If blnSelectMode = False Then
                If strPrompt <> "" Then
                    WritePrompt(strPrompt)
                    WriteCommandLine("")
                End If
                GetPoint = -1
                Exit Function
            End If
            If blnPickPoint Then
                If bln_ClickPoint = False Then
                    WritePrompt(strPrompt)
                    WritePrompt("計測点を指定してください。")
                    WriteCommandLine(strPrompt)
                    blnPickPoint = False
                Else
                    GetPoint = 0
                    Exit While
                End If
            End If
        End While
        WriteCommandLine("")
        pickPoint = pick_P
        'Dim str_name As String
        If binfirstscalesetting = True And bindistance2pointstart = True Then
            WritePrompt("点目を指示：点：Ｐ" & pickPoint.LabelName & "，x:" & pickPoint.Real_x.ToString & "×" & sys_ScaleInfo.len.ToString & "，y:" & pickPoint.Real_y.ToString & "×" & sys_ScaleInfo.len.ToString & "，z:" & pickPoint.Real_y.ToString & "×" & sys_ScaleInfo.len.ToString)
        Else
            WritePrompt("点目を指示：点：Ｐ" & pickPoint.LabelName & "，x:" & pickPoint.Real_x.ToString & "，y:" & pickPoint.Real_y.ToString & "，z:" & pickPoint.Real_y.ToString)
        End If
        MainFrm.flgSelectC3D = False
        blnSelectMode = False
    End Function

    Public Function GetUserEnt(ByRef ent As CUserEnt, ByVal strPrompt As String) As Integer
        blnPickPoint = False
        blnSelectMode = True
        bln_EntClickPoint = False
        model_picks_over = False
        WriteCommandLine(strPrompt)
        While True
            If blnSelectMode = False Then
                If strPrompt <> "" Then
                    WritePrompt(strPrompt)
                    WriteCommandLine("")
                End If
                GetUserEnt = -1
                model_SingleSelct = True
                Exit Function
            End If
            If blnPickPoint Then
                If bln_EntClickPoint = False Then
                    WritePrompt(strPrompt)
                    WritePrompt("ユーザー図形を指定してください。")
                    WriteCommandLine(strPrompt)
                    blnPickPoint = False
                Else
                    GetUserEnt = 0
                    Exit While
                End If
            End If
            If model_picks_over Then
                blnSelectMode = False
                GetUserEnt = 1
                WriteCommandLine("")
                WritePrompt(strPrompt)
                Exit Function
            End If
        End While
        WriteCommandLine("")
        WritePrompt(strPrompt)
        ent = ent_click
        'Dim str_name As String
        'If binfirstscalesetting = True And bindistance2pointstart = True Then
        '    WritePrompt("点目を指示：点：Ｐ" & pickPoint.LabelName & "，x:" & pickPoint.Real_x.ToString & "×" & sys_ScaleInfo.len.ToString & "，y:" & pickPoint.Real_y.ToString & "×" & sys_ScaleInfo.len.ToString & "，z:" & pickPoint.Real_y.ToString & "×" & sys_ScaleInfo.len.ToString)
        'Else
        '    WritePrompt("点目を指示：点：Ｐ" & pickPoint.LabelName & "，x:" & pickPoint.Real_x.ToString & "，y:" & pickPoint.Real_y.ToString & "，z:" & pickPoint.Real_y.ToString)
        'End If
        blnSelectMode = False
    End Function
    Public Function GetFramPoint(ByRef pickframPoint As GeoPoint, ByVal strPrompt As String) As Integer
        GetFramPoint = 0
        binpickfram = False
        blnSelectMode_fram = True
        WriteCommandLine(strPrompt)
        While True

            If binpickfram Then
                If binpickfram = False Then
                    WritePrompt(strPrompt)
                    WritePrompt("点を指定してください。")
                    WriteCommandLine(strPrompt)
                    binpickfram = False
                Else
                    Exit While
                End If
            End If
        End While
        WriteCommandLine("")
        pickframPoint = pick_F
        WritePrompt("点を指示成功")
        blnSelectMode_fram = False
    End Function
    Public Function GetareaPoint1(ByVal strPrompt As String) As Integer
        GetareaPoint1 = 0
        WriteCommandLine(strPrompt)
        While True

            If intgppointarea = 1 Then
                'If intgppointarea <> 1 Then
                '    WritePrompt(strPrompt)
                '    WritePrompt("点を指定してください。")
                '    WriteCommandLine(strPrompt)
                'Else
                '    Exit While
                'End If
            End If
        End While
        WriteCommandLine("")
        WritePrompt("点を指示成功")
    End Function
    Public Function GetareaPoint2(ByVal strPrompt As String) As Integer
        GetareaPoint2 = 0
        WriteCommandLine(strPrompt)
        While True

            If intgppointarea = 2 Then
                If intgppointarea <> 2 Then
                    WritePrompt(strPrompt)
                    WritePrompt("点を指定してください。")
                    WriteCommandLine(strPrompt)
                Else
                    Exit While
                End If
            End If
        End While
        WriteCommandLine("")
        WritePrompt("点を指示成功")
    End Function

    Public Function GetCamers(ByRef pickCamer As CCamera, ByVal strPrompt As String) As Integer

        binPickCamer = False
        blnSelectMode = True
        WriteCommandLine(strPrompt)
        While True
            If blnSelectMode = False Then
                If strPrompt <> "" Then
                    WritePrompt(strPrompt)
                    WriteCommandLine("")
                End If
                GetCamers = -1
                Exit Function
            End If
            If binPickCamer Then
                If bln_ClickCamera = False Then
                    WritePrompt(strPrompt)
                    WritePrompt("カメラを指定してください。")
                    WriteCommandLine(strPrompt)
                    binPickCamer = False
                Else
                    GetCamers = 0
                    Exit While
                End If
            End If
        End While
        WriteCommandLine("")
        pickCamer = pick_C
        'WritePrompt("カメラを指示成功")
        blnSelectMode = False
    End Function

    '===================================================================
    ' strValue    [O/ ] 入力された文字

    ' strChkPrompt[I/ ] 入力フィールドの先頭文字（例："ラベル："）

    Public Function GetStringNoWait(ByRef strValue As String, ByVal strChkPrompt As String) As Integer
        Dim ind As Integer, strInputText As String
        'strInputText = MainFrm.TBox_Data.Text '13.1.28以前

        strInputText = MainFrm.TBox_Data.Text.ToUpper() '13.1.28　（山田）入力された頭文字を大文字に
        ind = InStr(strInputText, strChkPrompt) 'strInputText：（コマンド入力領域）『：ラベル：』+『ユーザー入力値』

        If (ind > 0) Then
            strValue = Mid(strInputText, ind + Len(strChkPrompt)) ' strValueはユーザー入力の頭文字 / Midは計測点名を入力し、計測点をクリック語発生?
        Else
            strValue = strInputText
        End If
        ind = InStr(strValue, ":")
        If (ind > 0) Then
            strValue = Mid(strValue, ind + Len(":"))
        End If
        GetStringNoWait = Len(strValue)
    End Function

    Public Function GetString(ByRef strValue As String, ByVal strPrompt As String) As Integer
        GetString = 0
        'blnPickPoint = False
        ChangeFinis = 0
        blnSelectMode = True
        WriteCommandLine(strPrompt)
        Me.strStrValue = MainFrm.TBox_Data.Text
        While True
            If blnSelectMode = False Then
                If strPrompt <> "" Then
                    WritePrompt(strPrompt)
                    WriteCommandLine("")
                End If
                GetString = -1
                Exit Function
            End If
            If ChangeFinis = 1 Then
                strValue = strStrValue
                WriteCommandLine("")
                WritePrompt(MainFrm.TBox_Data.Text)
                ChangeFinis = 0
                'ChangeStart = 0
                Exit While
            End If
        End While
        blnSelectMode = False
    End Function
    Public Function GetPoint1(ByVal cp As GeoPoint, ByVal cp_finis As GeoPoint) As GeoPoint
        GetPoint1 = Nothing
        Dim fdepth As Double
        Dim ObjectX, ObjectY, ObjectZ As Double
        Dim iViewPort(4) As Integer
        Dim dProjMatrix(16) As Double
        Dim dModelMatrix(16) As Double
        Dim iScreen As New Point
        'Dim iScrToWinX, iScrToWinY As Integer
        'ChangeFinis = 0
        'blnSelectMode = True
        'WriteCommandLine(strPrompt)

        glGetIntegerv(GL_VIEWPORT, iViewPort)
        glPushMatrix()
        glGetDoublev(GL_MODELVIEW_MATRIX, dModelMatrix)
        glGetDoublev(GL_PROJECTION_MATRIX, dProjMatrix)
        GetCursorPos(iScreen)
        'iScrToWinX = iScreen.X - cp.x
        'iScrToWinY = iScreen.Y - cp.y
        glReadPixels(iScreen.X, 100, 1, 1, GL_DEPTH_COMPONENT, GL_FLOAT, fdepth)
        gluUnProject(iScreen.X, (iViewPort(3) - iScreen.Y), fdepth, dModelMatrix, dProjMatrix, iViewPort, ObjectX, ObjectY, ObjectZ)
        cp_finis.x = ObjectX
        cp_finis.y = ObjectY
        cp_finis.z = ObjectZ
        glPopMatrix()
        MsgBox(ObjectX & "," & ObjectY & "," & ObjectZ)
    End Function

    Public Function GetPointNoPrompt(ByRef pickPoint As CLookPoint, ByVal strPrompt As String) As Integer
        blnPickPoint = False
        blnSelectMode = True
        'WritePrompt(strPrompt)
        'WriteCommandLine(strPrompt)
        MainFrm.flgSelectC3D = True
        While True
            If blnSelectMode = False Then
                'If strPrompt <> "" Then
                '    WritePrompt(strPrompt)
                '    'WriteCommandLine("")
                'End If
                GetPointNoPrompt = -1
                Exit Function
            End If
            If blnPickPoint Then
                If bln_ClickPoint = False Then
                    'WritePrompt(strPrompt)
                    WritePrompt("計測点を指定してください。")
                    'WriteCommandLine(strPrompt)
                    blnPickPoint = False
                Else
                    GetPointNoPrompt = 0
                    Exit While
                End If
            End If
        End While
        'WriteCommandLine("")
        pickPoint = pick_P '3D操作ビューで計測点をクリック

        'If binfirstscalesetting = True And bindistance2pointstart = True Then
        '    WritePrompt("点目を指示：点：Ｐ" & pickPoint.LabelName & "，x:" & pickPoint.Real_x.ToString & "×" & sys_ScaleInfo.len.ToString & "，y:" & pickPoint.Real_y.ToString & "×" & sys_ScaleInfo.len.ToString & "，z:" & pickPoint.Real_y.ToString & "×" & sys_ScaleInfo.len.ToString)
        'Else
        '    WritePrompt("点目を指示：点：Ｐ" & pickPoint.LabelName & "，x:" & pickPoint.Real_x.ToString & "，y:" & pickPoint.Real_y.ToString & "，z:" & pickPoint.Real_y.ToString)
        'End If
        MainFrm.flgSelectC3D = False
        blnSelectMode = False
    End Function

    Public Function GetPointNoPromptDoevent(ByRef pickPoint As CLookPoint, ByVal strPrompt As String) As Integer
        blnPickPoint = False
        blnSelectMode = True
        'WritePrompt(strPrompt)
        'WriteCommandLine(strPrompt)
        MainFrm.flgSelectC3D = True
        While True
            '  System.Threading.Thread.Sleep(100)
            System.Windows.Forms.Application.DoEvents()
            If blnSelectMode = False Then
                'If strPrompt <> "" Then
                '    WritePrompt(strPrompt)
                '    'WriteCommandLine("")
                'End If
                GetPointNoPromptDoevent = -1
                Exit Function
            End If
            If blnPickPoint Then
                If bln_ClickPoint = False Then
                    'WritePrompt(strPrompt)
                    WritePrompt("計測点を指定してください。")
                    'WriteCommandLine(strPrompt)
                    blnPickPoint = False
                Else
                    GetPointNoPromptDoevent = 0
                    Exit While
                End If
            End If
            If MainFrm.flgCreateSTEnter = True Or MainFrm.flgCreateSTCancel = True Then
                GetPointNoPromptDoevent = -1
                Exit Function
            End If
        End While
        'WriteCommandLine("")
        pickPoint = pick_P '3D操作ビューで計測点をクリック

        'If binfirstscalesetting = True And bindistance2pointstart = True Then
        '    WritePrompt("点目を指示：点：Ｐ" & pickPoint.LabelName & "，x:" & pickPoint.Real_x.ToString & "×" & sys_ScaleInfo.len.ToString & "，y:" & pickPoint.Real_y.ToString & "×" & sys_ScaleInfo.len.ToString & "，z:" & pickPoint.Real_y.ToString & "×" & sys_ScaleInfo.len.ToString)
        'Else
        '    WritePrompt("点目を指示：点：Ｐ" & pickPoint.LabelName & "，x:" & pickPoint.Real_x.ToString & "，y:" & pickPoint.Real_y.ToString & "，z:" & pickPoint.Real_y.ToString)
        'End If
        MainFrm.flgSelectC3D = False
        blnSelectMode = False
    End Function

    '20170224 baluu add start
    Public Sub GetDrawRegion(ByRef imageIndex As Integer)
        m_blnDrawRegion = True
        Dim vvkey As UShort = 0
        While True
            If vvkey = 1 Then
                m_blnDrawRegion = False
                Exit Sub
            End If
            If m_blnDrawRegion = False Then
                imageIndex = MainFrm.pSelectedImageIndex
                vvkey = GetAsyncKeyState(Keys.Escape)
                Exit Sub
            End If
        End While
    End Sub
    '20170224 baluu add end
End Class
