﻿Module IJCAD7
    '--CAD関連
    'Public g_IJcadApp As IntelliCAD.Application = Nothing
    'Public icadDoc As IntelliCAD.Document = Nothing
    Public g_IJcadApp As Object = Nothing
    Public icadDoc As Object = Nothing
    Public icadVersion As String = ""   ' "7.1.～":IJCAD8、"7-1109.05"など：IJCAD7
    '--
    'Private Declare Function SetParent Lib "user32" _
    '                (ByVal hWndChild As Long, ByVal hWndNewParent As Long) As Long
    'Private Declare Function GetParent Lib "user32" (ByVal hwnd As Long) As Long
    'Private Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As Long) As Long

    '============================================================
    ' 機能：CADが起動されているかをチェック
    '     bMsg=True：エラー表示する
    '============================================================
    Public Function CheckICADStatus(ByVal bMsg As Boolean) As Integer
        Try
            g_IJcadApp.Visible = True
            icadVersion = g_IJcadApp.Version
            CheckICADStatus = 0
        Catch ex As Exception
            If (bMsg = True) Then
                MsgBox("IntelliCADが起動されていません。")
            End If
            CheckICADStatus = 2
            Exit Function
        End Try
    End Function

    '============================================================
    ' 機能：CADのドキュメントを取得

    '============================================================
    Public Function getIcadDocument() As IntelliCAD.Document
        Dim ii As Long = 0
        getIcadDocument = Nothing
        Do While True
            Try
                Call g_IJcadApp.RunCommand("NEW;")
                YmdSleepOpen()
                getIcadDocument = g_IJcadApp.ActiveDocument
                YmdSleepActive()
                Exit Do
            Catch ex As Exception
                'MsgBox("CAD図面が開かれていません。")
                'getIcadDocument = Nothing
                'Exit Function
            End Try
            ii = ii + 1
            YmdSleepMsec(500)
            If (ii > 20) Then
                MsgBox("CAD図面が開かれていません。処理を中止します。")
                getIcadDocument = Nothing
                Exit Function
            End If
        Loop
    End Function

    '============================================================
    ' 機能：CADを起動

    '============================================================
    Public Sub initCAD()
#If 1 Then
        Try
            g_IJcadApp = CreateObject("Icad.Application")
            g_IJcadApp.Visible = True
            icadVersion = g_IJcadApp.Version
        Catch ex As Exception
            MsgBox("IntelliCADが起動できません。")
            Exit Sub
        End Try
#Else
        Dim ICADobj As Object
        Try
            'Dim iDlg As New InfoDialog
            'iDlg.LabelMsg.Text = "IJCADを起動中です"
            'iDlg.Show()
            'g_IJcadApp = GetObject("", "ICAD.Application")
            ICADobj = GetObject("", "ICAD.Application")
            g_IJcadApp = ICADobj
            'g_IJcadApp = CreateObject("Icad.Application")
            g_IJcadApp.Visible = True
            icadVersion = g_IJcadApp.Version
            'iDlg.Close()
        Catch ex As Exception
            Try
                'g_IJcadApp = CreateObject("ICAD.Application")
                ICADobj = CreateObject("ICAD.Application")
                g_IJcadApp = ICADobj
                g_IJcadApp.Visible = True
                icadVersion = g_IJcadApp.Version
            Catch ex2 As Exception
                MsgBox("IntelliCADが起動できません。")
                Exit Sub
            End Try
            'MsgBox("IntelliCADが起動できません。")
            'Exit Sub
        End Try
#End If
    End Sub

    Public Function getIJCADVersion() As String
        If (Left(icadVersion, 3) = "7.1") Then
            getIJCADVersion = "8"
        Else
            getIJCADVersion = "7"
        End If
    End Function
    '============================================================
    ' 機能：CADをシャットダウン
    '============================================================
    Sub ShutdownCAD()
        ShutDownApp(g_IJcadApp)
        g_IJcadApp = Nothing
    End Sub
    Sub ShutDownApp(ByVal appToShutDown As Object)
        If Not appToShutDown Is Nothing Then
            appToShutDown.ActiveDocument.SaveAs("c:\DeleteMe.dwg")
            appToShutDown.Quit()
            appToShutDown = Nothing
        End If
    End Sub

    '============================================================
    ' 機能：CAD図面の保存

    '============================================================
    Sub saveFile(ByVal strFileName As String)
        Try
            If Not g_IJcadApp Is Nothing Then
                g_IJcadApp.ActiveDocument.SaveAs(strFileName)
            End If
        Catch ex As Exception
            MsgBox("指定された図面[" & strFileName & "]が保存できません")
            Exit Sub
        End Try
    End Sub
    '===============================================================================
    '機  能:プロンプト表示
    '===============================================================================
    Public Sub YmdPrompt(ByVal strPrompt As String)
        On Error Resume Next
        IJCAD7.g_IJcadApp.ActiveDocument.Utility.Prompt(strPrompt)
        YmdSleepMsec(50)
#If WPF Then
        DispatcherHelper.DoEvents() : DispatcherHelper.DoEvents() : DispatcherHelper.DoEvents()
#Else
        Windows.Forms.Application.DoEvents() : Windows.Forms.Application.DoEvents() : Windows.Forms.Application.DoEvents()
#End If
    End Sub

    ''===============================================================================
    ''機  能: 画層を作成する
    ''戻り値:=True：成功、=False：失敗

    ''引  数:icadDoc         [I/O]   ICADドキュメントオブジェクト

    ''       strLayerName    [I/ ]   画層名

    ''備  考:
    ''===============================================================================
    'Public Function CreateLayer( _
    '    ByRef icadDoc As IntelliCAD.Document, _
    '    ByVal strLayerName As String _
    ') As Boolean
    '    Dim IcadLayer As IntelliCAD.Layer
    '    On Error Resume Next
    '    IcadLayer = icadDoc.Layers(strLayerName)
    '    If IcadLayer Is Nothing Then
    '        IcadLayer = icadDoc.Layers.Add(strLayerName)
    '        If IcadLayer Is Nothing Then
    '            CreateLayer = False
    '        Else
    '            CreateLayer = True
    '        End If
    '    Else
    '        CreateLayer = True   'すでに指定した画層が存在しています。

    '    End If
    'End Function

    ''===============================================================================
    ''機  能: 画層を作成する
    ''戻り値:=True：成功、=False：失敗

    ''引  数:icadDoc         [I/O]   ICADドキュメントオブジェクト

    ''       strLayerName    [I/ ]   画層名

    ''備  考:
    ''===============================================================================
    'Public Function CreateLayerEntSet( _
    '    ByRef icadDoc As IntelliCAD.Document, _
    '    ByVal entset As EntSetting _
    ') As Boolean
    '    On Error Resume Next
    '    Dim strCmd As String
    '    Dim strLayerName As String
    '    Dim IcadLayer As IntelliCAD.Layer
    '    strLayerName = entset.layerName
    '    IcadLayer = icadDoc.Layers(strLayerName)
    '    If IcadLayer Is Nothing Then
    '        'IcadLayer = icadDoc.Layers.Add(strLayerName)
    '        'If IcadLayer Is Nothing Then
    '        '    CreateLayerEntSet = False
    '        '    Exit Function
    '        'Else
    '        '    CreateLayerEntSet = True
    '        'End If
    '        strCmd = "-LAYER N " & (strLayerName) & ";;"
    '        Call g_IJcadApp.RunCommand(strCmd)
    '        YmdSleep()
    '        CreateLayerEntSet = True
    '    Else
    '        CreateLayerEntSet = True   'すでに指定した画層が存在しています。

    '    End If

    '    ' レイヤの色、線種を設定

    '    strCmd = "-LAYER C T " & _
    '            (entset.color.dbl_red * 255) & "," & _
    '            (entset.color.dbl_green * 255) & "," & _
    '            (entset.color.dbl_blue * 255) & _
    '            " " & strLayerName & ";;"
    '    Call g_IJcadApp.RunCommand(strCmd)
    '    YmdSleep()

    '    '        IcadLayer.color.ColorIndex = entset.color.code
    '    'entset.linewidth
    '    'entset.linetype.code
    'End Function

    '==============================================================================
    '機　能：図形の指示
    '戻り値：0 正常終了 1 キャンセル終了 -1 エラー終了

    '引　数：

    '      　icdEntity            [ /O]   図形データ
    '      　strPrompt            [I/ ]   表示するプロンプト（指定なしの場合、"図形を選択"）

    '      　intFilterType        [I/ ]   フィルタのタイプの配列
    '      　vntFilterData        [I/ ]   フィルタの値の配列
    '備　考：

    '==============================================================================
    Public Function YcmGetIJEntity( _
        ByRef icdEntity As IntelliCAD.SelectionSet, _
        ByVal strPrompt As String, _
        ByRef vntFilterType As Object, _
        ByRef vntFilterData As Object _
    ) As Integer
        Dim icdDoc As IntelliCAD.Document
        Dim icdPickedPoint As IntelliCAD.Point
        Dim icdSelset_Tmp As IntelliCAD.SelectionSet
        Dim icdEntityArray(0) As IntelliCAD.Entity
        Dim strSSGetName As String
        Dim strSSGetName2 As String
        Dim i As Integer
        Dim j As Integer
        Dim blnSelect As Boolean
        Dim blnResult As Boolean
        Dim intFilterCnt As Integer
        Dim vntFilterTypeTmp As Object
        Dim vntFilterDataTmp As Object
        On Error GoTo labError

        '初期化

        YcmGetIJEntity = -1
        blnSelect = False

        Dim old_Val As Object
        icdDoc = g_IJcadApp.ActiveDocument
        old_Val = icdDoc.GetVariable("OSMODE")
        Call icdDoc.SetVariable("OSMODE", 0)    '--スナップはオフ


        intFilterCnt = UBound(vntFilterType, 1)
        'フィルター情報を保管
        vntFilterTypeTmp = vntFilterType
        vntFilterDataTmp = vntFilterData

        Do
            YmdSleep()    '--続けて入力されるとコケルため
            strSSGetName = "TMP_" & icdDoc.SelectionSets.Count
            icdSelset_Tmp = g_IJcadApp.ActiveDocument.SelectionSets.Add(strSSGetName)
            'フィルターを設定し、選択実行

            For i = 0 To intFilterCnt
                vntFilterType(i) = vntFilterTypeTmp(i)
                vntFilterData(i) = vntFilterDataTmp(i)
            Next i
            Call YmdPrompt(strPrompt)
            Call icdSelset_Tmp.SelectOnScreen(vntFilterType, vntFilterData)
            If (icdDoc.Active = False) Then
                ' コマンド実行中に図面が閉じられたらコマンドを終了させる
                End
            End If

            '選択されている場合

            If icdSelset_Tmp.Count <> 0 Then
                icdSelset_Tmp.Highlight(False)
                '選択されていない場合キャンセル終了

            Else
                YcmGetIJEntity = 1
                icdSelset_Tmp.Clear()
                icdSelset_Tmp.Delete()
                icdSelset_Tmp = Nothing
                Call icdDoc.SetVariable("OSMODE", old_Val)
                Exit Function
            End If
            '選択されている場合

            If icdSelset_Tmp.Count > 0 Then
                strSSGetName2 = "TMP_" & icdDoc.SelectionSets.Count
                icdEntity = icdDoc.SelectionSets.Add(strSSGetName2)
                On Error Resume Next
                If (getIJCADVersion() = "8") Then
                    For i = 1 To icdSelset_Tmp.Count
                        icdEntityArray(0) = icdSelset_Tmp.Item(i - 1)
                        Call icdEntity.AddItems(icdEntityArray)
                    Next i
                Else
                    For i = 1 To icdSelset_Tmp.Count
                        icdEntityArray(0) = icdSelset_Tmp.Item(i)
                        Call icdEntity.AddItems(icdEntityArray)
                    Next i
                End If
            End If
            icdSelset_Tmp.Clear()
            icdSelset_Tmp.Delete()
            icdSelset_Tmp = Nothing
        Loop Until icdEntity.Count > 0

        Call icdDoc.SetVariable("OSMODE", old_Val)
        YcmGetIJEntity = 0
        Exit Function
labError:
        If (icdDoc.Active = False) Then
            ' コマンド実行中に図面が閉じられたらコマンドを終了させる
            End
        End If
        Call icdDoc.SetVariable("OSMODE", old_Val)
        '選択中のエラーの場合キャンセル終了

        If blnSelect = True Then
            YcmGetIJEntity = 1
        End If
        If Not icdEntity Is Nothing Then
            icdEntity.Clear()
            icdEntity.Delete()
            icdEntity = Nothing
        End If
        If Not icdSelset_Tmp Is Nothing Then
            icdSelset_Tmp.Clear()
            icdSelset_Tmp.Delete()
            icdSelset_Tmp = Nothing
        End If
    End Function
    'Private Sub transOutCoord(ByVal iOutCoord As Integer, ByRef x As Double, ByRef y As Double, ByRef z As Double)
    '    'iOutCoord = 0:3次元、=1:XY、=2:XZ、=3:YZ
    '    Select Case iOutCoord
    '        Case 0  ' 3次元

    '            Exit Sub
    '        Case 1  ' XY平面
    '            z = 0.0#
    '        Case 2  ' XZ平面
    '            y = z
    '            z = 0.0#
    '        Case 3  ' YZ平面
    '            x = y
    '            y = z
    '            z = 0.0#
    '    End Select
    'End Sub

    ''============================================================
    '' 機能：計測点のCAD出力

    ''============================================================
    ''★20121114計測点(CAD出力は?⇒ターゲット面・中心円・十字線すべて必要か?)
    'Public Sub exportLookPoints(ByRef icdDoc As IntelliCAD.Document, ByVal iOutCoord As Integer)
    '    Dim ii As Long
    '    Dim cpos As CLookPoint
    '    Dim iPos As IntelliCAD.Point, icdEnt As IntelliCAD.Entity
    '    If (icdDoc Is Nothing) Then Exit Sub
    '    If nLookPoints > 0 Then
    '        If entset_point.blnVisiable Then
    '            CreateLayer(icdDoc, entset_point.layerName)  '"LOOKPOINT")
    '            CreateLayerEntSet(icdDoc, entset_point)
    '            For ii = 0 To nLookPoints - 1
    '                If gDrawPoints(ii).blnDraw Then
    '                    If gDrawPoints(ii).type <> 2 Then   'コードターゲットは表示しない

    '                        cpos = gDrawPoints(ii)
    '                        '--出力次元による座標値の調整
    '                        Call transOutCoord(iOutCoord, cpos.Real_x, cpos.Real_y, cpos.Real_z)
    '                        iPos = g_IJcadApp.Library.CreatePoint(cpos.Real_x, cpos.Real_y, cpos.Real_z)
    '                        icdEnt = icdDoc.ModelSpace.AddPointEntity(iPos)
    '                        icdEnt.Layer = entset_point.layerName   '"LOOKPOINT"
    '                        icdEnt.Update()
    '                    End If
    '                End If
    '            Next
    '        End If
    '    End If
    '    '★20121114ひとまずentset_point2＝中心円で・・・・
    '    'If nLookPoints > 0 Then
    '    '    If entset_point2.blnVisiable Then
    '    '        'CreateLayer(icdDoc, "LOOKPOINT")
    '    '        CreateLayerEntSet(icdDoc, entset_point2)
    '    '        For ii = 0 To nLookPoints - 1
    '    '            If gDrawPoints(ii).blnDraw Then
    '    '                cpos = gDrawPoints(ii)
    '    '                '--出力次元による座標値の調整
    '    '                Call transOutCoord(iOutCoord, cpos.Real_x, cpos.Real_y, cpos.Real_z)
    '    '                iPos = g_IJcadApp.Library.CreatePoint(cpos.Real_x, cpos.Real_y, cpos.Real_z)
    '    '                icdEnt = icdDoc.ModelSpace.AddPointEntity(iPos)
    '    '                icdEnt.Layer = entset_point2.layerName   '"LOOKPOINT"
    '    '                icdEnt.Update()
    '    '            End If
    '    '        Next
    '    '    End If
    '    'End If
    'End Sub

    ''============================================================
    '' 機能：計測ラベル文字のCAD出力

    ''============================================================
    'Public Sub exportLabelText(ByRef icdDoc As IntelliCAD.Document, ByVal dH As Double, ByVal iOutCoord As Integer)
    '    Dim ii As Long
    '    Dim pRS As New GeoPoint
    '    Dim iPS As IntelliCAD.Point, icdEnt As IntelliCAD.Text
    '    If (icdDoc Is Nothing) Then Exit Sub
    '    If nLabelText > 0 Then
    '        If entset_label.blnVisiable Then
    '            CreateLayer(icdDoc, entset_label.layerName) ' "LABELTEXT")
    '            CreateLayerEntSet(icdDoc, entset_label)
    '            For ii = 0 To nLabelText - 1
    '                If gDrawLabelText(ii).blnDraw Then
    '                    'gDrawLabelText(ii).mid()
    '                    Call pRS.setXYZ(gDrawLabelText(ii).x, gDrawLabelText(ii).y, gDrawLabelText(ii).z)
    '                    localToGlobalPoint(pRS, sys_CoordInfo.mat)
    '                    '--出力次元による座標値の調整
    '                    Call transOutCoord(iOutCoord, pRS.x, pRS.y, pRS.z)
    '                    iPS = g_IJcadApp.Library.CreatePoint(pRS.x, pRS.y, pRS.z)
    '                    icdEnt = icdDoc.ModelSpace.AddText(gDrawLabelText(ii).LabelName, iPS, dH)
    '                    With icdEnt
    '                        .Update()
    '                        .Layer = entset_label.layerName '"LABELTEXT"
    '                        '#If IJCAD = 7 Then
    '                        '                            .color = IntelliCAD.Colors.vicWhite
    '                        '#Else
    '                        '                            .color.ColorIndex = IntelliCAD.Colors.vicWhite
    '                        '#End If
    '                        .HorizontalAlignment = IntelliCAD.HorizontalAlignment.vicHorizontalAlignmentCenter
    '                        .VerticalAlignment = IntelliCAD.VerticalAlignment.vicVerticalAlignmentMiddle
    '                        .Rotation = 0.0#
    '                        .StyleName = "Standard"
    '                        .ScaleFactor = 1.0#
    '                        .TextAlignmentPoint = iPS
    '                        .Update()
    '                    End With
    '                End If
    '            Next
    '        End If
    '    End If
    'End Sub
    ''============================================================
    '' 機能：レイのCAD出力

    ''============================================================
    'Public Sub exportRay(ByRef icdDoc As IntelliCAD.Document, ByVal iOutCoord As Integer)
    '    Dim ii As Long
    '    Dim pRS As New GeoPoint
    '    Dim iPS As IntelliCAD.Point, iPE As IntelliCAD.Point
    '    Dim icdEnt As IntelliCAD.Entity
    '    If (icdDoc Is Nothing) Then Exit Sub
    '    If nRays > 0 Then
    '        If entset_ray.blnVisiable Then
    '            CreateLayer(icdDoc, entset_ray.layerName) ' "RAY")
    '            CreateLayerEntSet(icdDoc, entset_ray)
    '            For ii = 0 To nRays - 1
    '                If gDrawRays(ii).blnDraw Then
    '                    pRS = gDrawRays(ii).startPnt.Copy
    '                    localToGlobalPoint(pRS, sys_CoordInfo.mat)
    '                    '--出力次元による座標値の調整
    '                    Call transOutCoord(iOutCoord, pRS.x, pRS.y, pRS.z)
    '                    iPS = g_IJcadApp.Library.CreatePoint(pRS.x, pRS.y, pRS.z)
    '                    pRS = gDrawRays(ii).endPnt.Copy
    '                    localToGlobalPoint(pRS, sys_CoordInfo.mat)
    '                    Call transOutCoord(iOutCoord, pRS.x, pRS.y, pRS.z)
    '                    iPE = g_IJcadApp.Library.CreatePoint(pRS.x, pRS.y, pRS.z)
    '                    icdEnt = icdDoc.ModelSpace.AddLine(iPS, iPE)
    '                    icdEnt.Layer = entset_ray.layerName '"RAY"
    '                    icdEnt.Update()
    '                End If
    '            Next
    '        End If
    '    End If
    'End Sub

    ''============================================================
    '' 機能：ユーザ作成図形のCAD出力

    ''============================================================
    'Public Sub exportUserElm(ByRef icdDoc As IntelliCAD.Document, ByVal iOutCoord As Integer)
    '    Dim mcol As ModelColor
    '    Dim ii As Long
    '    Dim pRS As New GeoPoint
    '    Dim dblRad As Double, dblAng As Double
    '    Dim iPS As IntelliCAD.Point, iPE As IntelliCAD.Point
    '    Dim icdEnt As IntelliCAD.Entity
    '    If (icdDoc Is Nothing) Then Exit Sub
    '    '--        CreateLayer(icdDoc, "USER")
    '    CreateLayer(icdDoc, entset_line_CAD.layerName)
    '    CreateLayer(icdDoc, entset_circle_CAD.layerName)
    '    CreateLayer(icdDoc, entset_line.layerName)
    '    CreateLayer(icdDoc, entset_circle.layerName)
    '    CreateLayerEntSet(icdDoc, entset_line_CAD)
    '    CreateLayerEntSet(icdDoc, entset_circle_CAD)
    '    CreateLayerEntSet(icdDoc, entset_line)
    '    CreateLayerEntSet(icdDoc, entset_circle)

    '    'g_IJcadApp.Version
    '    '線分図形の出力

    '    If nUserLines > 0 Then
    '        If entset_line.blnVisiable Then
    '            For ii = 0 To nUserLines - 1
    '                If (gDrawUserLines(ii).binDelete = False) Then
    '                    If gDrawUserLines(ii).blnDraw Then
    '                        pRS = gDrawUserLines(ii).startPnt.Copy
    '                        localToGlobalPoint(pRS, sys_CoordInfo.mat)
    '                        '--出力次元による座標値の調整
    '                        Call transOutCoord(iOutCoord, pRS.x, pRS.y, pRS.z)
    '                        iPS = g_IJcadApp.Library.CreatePoint(pRS.x, pRS.y, pRS.z)
    '                        pRS = gDrawUserLines(ii).endPnt.Copy
    '                        localToGlobalPoint(pRS, sys_CoordInfo.mat)
    '                        Call transOutCoord(iOutCoord, pRS.x, pRS.y, pRS.z)
    '                        iPE = g_IJcadApp.Library.CreatePoint(pRS.x, pRS.y, pRS.z)
    '                        icdEnt = icdDoc.ModelSpace.AddLine(iPS, iPE)


    '                        '--                        icdEnt.Layer = "USER"
    '                        If (gDrawUserLines(ii).elmType = 0) Then
    '                            icdEnt.Layer = entset_line.layerName
    '                        Else
    '                            icdEnt.Layer = entset_line_CAD.layerName
    '                        End If
    '                        Try
    '                            'icdEnt.Linetype = gDrawUserLines(ii).lineTypeCode
    '                            mcol = YCM_GetColorInfoByCode(gDrawUserLines(ii).colorCode)
    '                            '    Call icdEnt.color.SetRGB(CInt(mcol.dbl_red * 255), CInt(mcol.dbl_green * 255), CInt(mcol.dbl_blue * 255))
    '                            ''icdEnt.color.ColorIndex = gDrawUserLines(ii).colorCode
    '                        Catch ex As Exception
    '                        End Try
    '                        icdEnt.Update()
    '                    End If
    '                End If
    '            Next
    '        End If
    '    End If
    '    '円図形の出力

    '    If (nCircleNew > 0) Then
    '        If entset_circle.blnVisiable Then
    '            For ii = 0 To nCircleNew - 1
    '                If (gDrawCircleNew(ii).binDelete = False) Then
    '                    If gDrawCircleNew(ii).blnDraw Then
    '                        pRS = gDrawCircleNew(ii).org.Copy
    '                        localToGlobalPoint(pRS, sys_CoordInfo.mat)
    '                        '--出力次元による座標値の調整
    '                        Call transOutCoord(iOutCoord, pRS.x, pRS.y, pRS.z)
    '                        iPS = g_IJcadApp.Library.CreatePoint(pRS.x, pRS.y, pRS.z)
    '                        dblRad = gDrawCircleNew(ii).r * sys_ScaleInfo.scale
    '                        icdEnt = icdDoc.ModelSpace.AddCircle(iPS, dblRad)
    '                        icdEnt.Update()
    '                        'iPE = g_IJcadApp.Library.CreatePoint((pRS.x + 1000.0#), pRS.y, pRS.z)
    '                        If (iOutCoord = 0) Then
    '                            Dim iOrg As IntelliCAD.Point, iDir As IntelliCAD.Point
    '                            iOrg = g_IJcadApp.Library.CreatePoint(0.0, 0.0, 0.0)
    '                            iDir = g_IJcadApp.Library.CreatePoint(1000.0, 0.0, 0.0)
    '                            dblAng = DegToRadian(gDrawCircleNew(ii).x_angle)
    '                            If (Math.Abs(dblAng) > 0.0#) Then
    '                                'icdEnt.Rotate3D(iPS, iPE, dblAng)
    '                                icdEnt.Move(iPS, iOrg)
    '                                icdEnt.Update()
    '                                icdEnt.Rotate3D(iOrg, iDir, dblAng)
    '                                icdEnt.Update()
    '                                icdEnt.Move(iOrg, iPS)
    '                                icdEnt.Update()
    '                            End If
    '                            'iPE = g_IJcadApp.Library.CreatePoint(pRS.x, (pRS.y + 1000.0#), pRS.z)
    '                            iDir = g_IJcadApp.Library.CreatePoint(0.0, 1000.0, 0.0)
    '                            dblAng = DegToRadian(gDrawCircleNew(ii).y_angle)
    '                            If (Math.Abs(dblAng) > 0.0#) Then
    '                                'icdEnt.Rotate3D(iPS, iPE, dblAng)
    '                                icdEnt.Move(iPS, iOrg)
    '                                icdEnt.Update()
    '                                icdEnt.Rotate3D(iOrg, iDir, dblAng)
    '                                icdEnt.Update()
    '                                icdEnt.Move(iOrg, iPS)
    '                                icdEnt.Update()
    '                            End If
    '                        End If

    '                        If (gDrawCircleNew(ii).elmType = 0) Then
    '                            icdEnt.Layer = entset_circle.layerName
    '                        Else
    '                            icdEnt.Layer = entset_circle_CAD.layerName
    '                        End If
    '                        Try
    '                            'icdEnt.Linetype = gDrawCircleNew(ii).lineTypeCode
    '                            mcol = YCM_GetColorInfoByCode(gDrawCircleNew(ii).colorCode)
    '                            '     Call icdEnt.color.SetRGB(CInt(mcol.dbl_red * 255), CInt(mcol.dbl_green * 255), CInt(mcol.dbl_blue * 255))
    '                            ''icdEnt.color.ColorIndex = gDrawCircleNew(ii).colorCode
    '                        Catch ex As Exception
    '                        End Try
    '                        icdEnt.Update()
    '                    End If
    '                End If
    '            Next
    '            'CStr(entset_circle.color.code)
    '            'CStr(entset_circle.linetype.code)
    '        End If
    '    End If
    'End Sub

    ''Private Function DegToRadian(ByVal dblAng As Double) As Double
    ''    DegToRadian = (dblAng * 3.141592654) / 180.0#
    ''End Function

    ''============================================================
    '' 機能：無単位系から設計座標系への変換
    ''============================================================
    'Private Function localToGlobalPoint(ByRef gpPos As GeoPoint, ByVal mat() As Double) As Integer
    '    Dim lookmat(0 To 3) As Double
    '    Dim XM(4, 4) As Double
    '    lookmat(0) = gpPos.x : lookmat(1) = gpPos.y : lookmat(2) = gpPos.z : lookmat(3) = 0
    '    lookmat(0) = gpPos.x * mat(0) + gpPos.y * mat(4) + gpPos.z * mat(8) + 1.0 * mat(12)
    '    lookmat(1) = gpPos.x * mat(1) + gpPos.y * mat(5) + gpPos.z * mat(9) + 1.0 * mat(13)
    '    lookmat(2) = gpPos.x * mat(2) + gpPos.y * mat(6) + gpPos.z * mat(10) + 1.0 * mat(14)
    '    gpPos.x = lookmat(0) * sys_ScaleInfo.scale : gpPos.y = lookmat(1) * sys_ScaleInfo.scale : gpPos.z = lookmat(2) * sys_ScaleInfo.scale
    'End Function

    '============================================================
    ' 機能：設計座標系から無単位系への変換
    '============================================================
    Private Function globalToLocalPoint(ByRef gpPos As GeoPoint, ByVal mat() As Double) As Integer
        Dim lookmat(0 To 3) As Double
        Dim gmMat As New GeoMatrix
        Dim dScale As Double
        Dim i As Integer, j As Integer, ind As Integer
        ind = -1
        For i = 1 To 4
            For j = 1 To 4
                ind = ind + 1
                Call gmMat.SetAt(i, j, mat(ind))
            Next j
        Next i
        gmMat.Invert()
        ind = -1
        For i = 1 To 4
            For j = 1 To 4
                ind = ind + 1
                mat(ind) = gmMat.GetAt(i, j)
            Next j
        Next i
        dScale = 1.0
        If (sys_ScaleInfo.scale > 0.0) Then dScale = 1.0 / sys_ScaleInfo.scale
        lookmat(0) = gpPos.x : lookmat(1) = gpPos.y : lookmat(2) = gpPos.z : lookmat(3) = 0
        lookmat(0) = gpPos.x * mat(0) + gpPos.y * mat(4) + gpPos.z * mat(8) + 1.0 * mat(12)
        lookmat(1) = gpPos.x * mat(1) + gpPos.y * mat(5) + gpPos.z * mat(9) + 1.0 * mat(13)
        lookmat(2) = gpPos.x * mat(2) + gpPos.y * mat(6) + gpPos.z * mat(10) + 1.0 * mat(14)
        gpPos.x = lookmat(0) * dScale : gpPos.y = lookmat(1) * dScale : gpPos.z = lookmat(2) * dScale
    End Function

    '========================================================
    ' 機能：IJCADから複数図形を選択しgoCADElmArrsに保持する
    '========================================================
    Public Sub YCM_GetCADEnts( _
        ByVal bSelLine As Boolean, _
        ByVal bSelCircle As Boolean _
    )
        Dim iRet As Integer
        Dim icdSSet As IntelliCAD.SelectionSet
        icdSSet = Nothing
        '1.IJCADからの図形選択

        iRet = YCM_GetEntity(icdSSet, bSelLine, bSelCircle)
        If (icdSSet.Count > 0) Then
            MsgBox("選択された図形数[" & icdSSet.Count & "]")
        Else
            MsgBox("何も選択されていません")
            Exit Sub
        End If

        If (goCADElmArrs Is Nothing) Then
            goCADElmArrs = New CObjectArray
        Else
            goCADElmArrs.Clear()
        End If

        '3.CAD図形要素から端点座標値を抽出（座標系設定で使うため）

        Dim gpS As GeoPoint, gpE As GeoPoint
        Dim gpOrg As GeoPoint, dRad As Double, dXAng As Double, dYAng As Double
        Dim i As Long
        Dim icdEnt As IntelliCAD.Entity = Nothing
        On Error Resume Next
        For i = 1 To icdSSet.Count
            If (getIJCADVersion() = "8") Then
                icdEnt = icdSSet.Item(i - 1)
            Else
                icdEnt = icdSSet.Item(i)
            End If
            Select Case (icdEnt.EntityType)
                Case IntelliCAD.EntityType.vicLine
                    Dim icdLine As IntelliCAD.Line
                    icdLine = icdEnt
                    gpS = New GeoPoint
                    gpE = New GeoPoint
                    Call gpS.SetIcadPoint(icdLine.StartPoint)
                    Call gpE.SetIcadPoint(icdLine.EndPoint)
                    globalToLocalPoint(gpS, sys_CoordInfo.mat)
                    globalToLocalPoint(gpE, sys_CoordInfo.mat)
                    iRet = goCADElmArrs.AddLine(gpS, gpE)
                    ''--del.start---------------------------------
                    'ReDim Preserve gDrawUserLines(nUserLines)
                    'gDrawUserLines(nUserLines) = New CUserLine
                    'gDrawUserLines(nUserLines).startPnt = gpS.Copy
                    'gDrawUserLines(nUserLines).endPnt = gpE.Copy
                    'gDrawUserLines(nUserLines).MID = nUserLines
                    'gDrawUserLines(nUserLines).elmType = 1
                    'gDrawUserLines(nUserLines).lineTypeCode = entset_line_CAD.linetype.code
                    'gDrawUserLines(nUserLines).colorCode = entset_line_CAD.color.code
                    'nUserLines = nUserLines + 1
                    ''--del.end-----------------------------------
                Case IntelliCAD.EntityType.vicLWPolyline
                Case IntelliCAD.EntityType.vic3dPoly
                Case IntelliCAD.EntityType.vicPolyline
                Case IntelliCAD.EntityType.vicCircle
                    Dim icdCir As IntelliCAD.Circle
                    icdCir = icdEnt
                    gpOrg = New GeoPoint
                    Call gpOrg.SetIcadPoint(icdCir.Center)
                    globalToLocalPoint(gpOrg, sys_CoordInfo.mat)
                    dRad = icdCir.Radius / sys_ScaleInfo.scale
                    dXAng = 0.0 : dYAng = 0.0
                    '--ins.start------------------
#If 1 Then
                    Dim gVecX As New GeoVector, gVecY As New GeoVector
                    Dim gVecCir As New GeoVector

                    Call gVecX.SetXY(1.0#, 0.0#)
                    Call gVecY.SetXY(0.0#, 1.0#)

                    Call gVecCir.SetXY(icdCir.Normal.z, icdCir.Normal.y)
                    dXAng = gVecCir.GetAngleTo(gVecX, gVecCir)
                    dXAng = gVecCir.GetSmallAngleTo(gVecX)

                    Call gVecCir.SetXY(icdCir.Normal.x, icdCir.Normal.z)
                    dYAng = gVecCir.GetAngleTo(gVecY, gVecCir)
                    dYAng = gVecCir.GetSmallAngleTo(gVecY)

#End If
                    '--ins.end--------------------
                    iRet = goCADElmArrs.AddCircle(gpOrg, dRad, dXAng, dYAng)
                    ''--del.start---------------------------------
                    'ReDim Preserve gDrawCircleNew(nCircleNew)
                    'gDrawCircleNew(nCircleNew) = New Ccircle
                    'gDrawCircleNew(nCircleNew).org = gpOrg.Copy
                    'gDrawCircleNew(nCircleNew).r = dRad
                    'gDrawCircleNew(nCircleNew).x_angle = dXAng
                    'gDrawCircleNew(nCircleNew).y_angle = dYAng
                    'gDrawCircleNew(nCircleNew).mid = nCircleNew
                    'gDrawCircleNew(nCircleNew).elmType = 1
                    'gDrawCircleNew(nCircleNew).lineTypeCode = entset_line_CAD.linetype.code
                    'gDrawCircleNew(nCircleNew).colorCode = entset_line_CAD.color.code
                    'nCircleNew = nCircleNew + 1
                    ''--del.end-----------------------------------
                Case Else
            End Select
        Next
        '4.計測座標値系に変換（無単位系）

        '　4.1「sys_CoordInfo.mat」の逆マトリックスを求めて考慮
        '　4.2 尺度の考慮：１／「sys_ScaleInfo.scale」

        '       円などは半径なども、文字は文字高

    End Sub
    Public Sub deleteUserCADData(ByVal bDelUser As Boolean, ByVal bDelCAD As Boolean)
        '読込まれているCAD図形データを削除する
        Dim bDelete As Boolean

        '線分図形
        If nUserLines > 0 Then
            For ii = (nUserLines - 1) To 0 Step -1
                bDelete = (gDrawUserLines(ii).elmType = 0) And bDelUser
                bDelete = bDelete Or (gDrawUserLines(ii).elmType = 1) And bDelCAD
                If (bDelete = True) Then
                    nUserLines = nUserLines - 1
                    ReDim Preserve gDrawUserLines(nUserLines)
                End If
            Next
        End If
        '円図形
        If (nCircleNew > 0) Then
            For ii = 0 To nCircleNew - 1
                bDelete = (gDrawCircleNew(ii).elmType = 0) And bDelUser
                bDelete = bDelete Or (gDrawCircleNew(ii).elmType = 1) And bDelCAD
                If (bDelete = True) Then
                    nCircleNew = nCircleNew - 1
                    ReDim Preserve gDrawCircleNew(nCircleNew)
                End If
            Next
        End If
    End Sub

    '========================================================
    ' 機能：複数図形の選択

    '========================================================
    Public Function YCM_GetEntity( _
        ByRef icdSSet As IntelliCAD.SelectionSet, _
        ByVal bSelLine As Boolean, _
        ByVal bSelCircle As Boolean _
    ) As Integer
        Dim i As Long
        Dim icdEnt As IntelliCAD.Entity = Nothing
        YCM_GetEntity = -1
        icdSSet = Nothing
        If (CheckICADStatus(False) <> 0) Then Exit Function
        Call SetForegroundProc("icad")

        '選択条件を作成する
        Dim strPrompt As String
        Dim intFilterType(5) As Integer
        Dim vntFilterData(5) As Object
        i = 0
        intFilterType(0) = -4 : vntFilterData(0) = "<OR"
        If (bSelLine) Then
            i = i + 1
            intFilterType(i) = 0 : vntFilterData(i) = "LINE"
        End If
        If (bSelCircle) Then
            i = i + 1
            intFilterType(i) = 0 : vntFilterData(i) = "CIRCLE"
        End If
        i = i + 1
        intFilterType(i) = -4 : vntFilterData(i) = "OR>"
        ReDim Preserve intFilterType(i)
        ReDim Preserve vntFilterData(i)

        Dim iRet As Integer
        strPrompt = "取込む図形を選択："
        Try
            iRet = YcmGetIJEntity(icdSSet, strPrompt, intFilterType, vntFilterData)
            YCM_GetEntity = iRet
        Catch ex As Exception
        End Try
        YCM_GetEntity = 0
    End Function
    '==============================================================================
    '機　能：点の指示
    '戻り値：0:正常終了, 1:キャンセル終了, -1:エラー終了

    '引　数：

    '      　icdPoint   [ /O] 点データ
    '      　strPrompt  [I/ ] 表示するプロンプト（指定なしの場合、"図形を選択"）

    '備　考：

    '==============================================================================
    Private Function GetPoint( _
        ByRef icdPoint As IntelliCAD.Point, _
        ByVal strPrompt As String _
    ) As Integer
        Dim icdDoc As IntelliCAD.Document
        On Error GoTo Err_GetPoint
        icdDoc = g_IJcadApp.ActiveDocument
        GetPoint = 1
        icdPoint = icdDoc.Utility.GetPoint(Nothing, strPrompt)
        GetPoint = 0
        Exit Function
Err_GetPoint:
        GetPoint = -1
    End Function

    '========================================================
    ' 機　能：CAD図面から３点を選択する

    ' 戻り値：nn:入力点数, -1:キャンセル終了

    ' 引　数：

    '       gpPos()  [/O] 入力された３点
    '========================================================
    Public Function YCM_GetCAD3Points(ByRef gpPos() As GeoPoint) As Integer
        Dim iRet As Integer
        Dim icdPos As IntelliCAD.Point = Nothing
        Dim strPrompt As String
        YCM_GetCAD3Points = -1
        MsgBox("CAD図面で座標値を一致させるための３点を指示してください", MsgBoxStyle.Information + MsgBoxStyle.OkOnly)

        strPrompt = "１点目を指示："
        iRet = GetPoint(icdPos, strPrompt)
        If (iRet <> 0) Then GoTo End_Label
        gpPos(0) = New GeoPoint
        gpPos(0).SetIcadPoint(icdPos)
        YCM_GetCAD3Points = 1
Input_Pos2:
        strPrompt = "２点目を指示："
        iRet = GetPoint(icdPos, strPrompt)
        If (iRet <> 0) Then GoTo End_Label
        gpPos(1) = New GeoPoint
        gpPos(1).SetIcadPoint(icdPos)
        If (gpPos(0).IsEqualTo(gpPos(1), 0.001) = True) Then
            '2点が一致しているため再入力

            MsgBox("１点目と同一点が指示されました、再度２点目を指示してください", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
            gpPos(1) = Nothing
            GoTo Input_Pos2
        End If
        YCM_GetCAD3Points = 2

Input_Pos3:
        strPrompt = "３点目を指示："
        iRet = GetPoint(icdPos, strPrompt)
        If (iRet <> 0) Then GoTo End_Label
        gpPos(2) = New GeoPoint
        gpPos(2).SetIcadPoint(icdPos)
        If (gpPos(0).IsEqualTo(gpPos(2), 0.001) = True) Then
            '2点が一致しているため再入力

            MsgBox("１点目と同一点が指示されました、再度３点目を指示してください", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
            gpPos(2) = Nothing
            GoTo Input_Pos2
        End If
        If (gpPos(1).IsEqualTo(gpPos(2), 0.001) = True) Then
            '2点が一致しているため再入力

            MsgBox("２点目と同一点が指示されました、再度３点目を指示してください", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
            gpPos(2) = Nothing
            GoTo Input_Pos2
        End If

        YCM_GetCAD3Points = 3
        Exit Function
End_Label:
        YCM_GetCAD3Points = -1
    End Function


#Region "IJCAD機能テスト部分"
    '========================================================
    ' 機能：座標選択テスト

    '========================================================
    Sub test_getpos()
        Dim gpPos(3) As GeoPoint
        Dim iRet As Integer
        Dim icdPos As IntelliCAD.Point = Nothing
        Dim strPrompt As String
        strPrompt = "１点目を指示："
        iRet = GetPoint(icdPos, strPrompt)
        If (iRet = 0) Then
            gpPos(0) = New GeoPoint
            gpPos(0).SetIcadPoint(icdPos)
        End If
        strPrompt = "２点目を指示："
        If (iRet = 0) Then
            iRet = GetPoint(icdPos, strPrompt)
            gpPos(1) = New GeoPoint
            gpPos(1).SetIcadPoint(icdPos)
        End If
        If (iRet = 0) Then
            strPrompt = "３点目を指示："
            iRet = GetPoint(icdPos, strPrompt)
            gpPos(2) = New GeoPoint
            gpPos(2).SetIcadPoint(icdPos)
        End If
    End Sub
    '+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++以下はテスト用
    '3点円

    'cpos = gDrawPoints(0)
    'iPS = g_IJcadApp.Library.CreatePoint(cpos.x, cpos.y, cpos.z)
    'cpos = gDrawPoints(1)
    'iPE = g_IJcadApp.Library.CreatePoint(cpos.x, cpos.y, cpos.z)
    'cpos = gDrawPoints(2)
    'iPC = g_IJcadApp.Library.CreatePoint(cpos.x, cpos.y, cpos.z)
    'icdEnt = icdDoc.ModelSpace.AddCircle3pt(iPS, iPE, iPC)
    'icdEnt.Update()

    '========================================================
    ' 機能：選択テスト

    '========================================================
    Sub test_Pos()
        On Error Resume Next
        CheckICADStatus(True)
        If (g_IJcadApp Is Nothing) Then Exit Sub
        Dim Pt1, Pt2 As IntelliCAD.Point
        g_IJcadApp.Visible = True
        Pt1 = g_IJcadApp.ActiveDocument.Utility.GetPoint(, "1点目を指示:")
        MsgBox("PT1[" & Pt1.x & "][" & Pt1.y & "]")
    End Sub
    Sub test()
        Dim icdSSet As IntelliCAD.SelectionSet
        icdSSet = Nothing
        If (g_IJcadApp Is Nothing) Then Exit Sub
        Dim FilterType(0 To 1) As Integer
        Dim FilterData(0 To 1) As Object
        ' 図形の種類(DXFコード0)フィルタを"LINE"に指定

        FilterType(0) = 0
        FilterData(0) = "LINE"
        ' 画層名(DXFコード8)ファイルタを"0"に指定

        FilterType(1) = 8
        FilterData(1) = "0"
        ' フィルタを使用して作図ウィンドウ上でオブジェクトを選択

        g_IJcadApp.Visible = True
        Call icdSSet.SelectOnScreen(FilterType, FilterData)
    End Sub
    '========================================================
    ' 機能：複数図形の選択テスト

    '========================================================
    Sub test_GetEnt()
        Dim i As Long
        Dim icdEnt As IntelliCAD.Entity = Nothing
        Dim icdSSet As IntelliCAD.SelectionSet
        icdSSet = Nothing
        CheckICADStatus(True)
        If (g_IJcadApp Is Nothing) Then Exit Sub
        '選択条件を作成する
        Dim strPrompt As String
        Dim intFilterType(0 To 2) As Integer
        Dim vntFilterData(0 To 2) As Object
        intFilterType(0) = -4 : vntFilterData(0) = "<OR"
        intFilterType(1) = 0 : vntFilterData(1) = "LINE"
        intFilterType(2) = -4 : vntFilterData(2) = "OR>"

        Dim iRet As Integer
        g_IJcadApp.Visible = True
        strPrompt = "取込む図形を選択："
        '図形選択

        Try
            iRet = YcmGetIJEntity(icdSSet, strPrompt, intFilterType, vntFilterData)
            MsgBox("normal-iRet[" & iRet & "] icdSSet.count[" & icdSSet.Count & "]")
            For i = 0 To icdSSet.Count - 1
                icdEnt = icdSSet.Item(i)
                Select Case (icdEnt.EntityType)
                    Case IntelliCAD.EntityType.vicLine
                    Case IntelliCAD.EntityType.vicLWPolyline
                    Case IntelliCAD.EntityType.vic3dPoly
                    Case IntelliCAD.EntityType.vicPolyline
                    Case Else
                End Select
            Next
        Catch ex As Exception
        End Try
        MsgBox("iRet[" & iRet & "] icdSSet.count[" & icdSSet.Count & "]")
    End Sub

#If 0 Then '++++++++++++++++++++++++++++
    Sub test()
        c_click_camers = New CCamera
        If nCamers > 0 Then
            If entset_camera.blnVisiable Then
                For ii = 0 To nCamers - 1
                    If gDrawCamers(ii).blnDraw Then
                        glLoadName(Camera_Name + ii)
                        YCM_DrawCamer(gDrawCamers(ii))
                        If model_pick = Camera_Name + ii Then
                            c_click_camers = gDrawCamers(ii)
                        End If

                    End If
                Next
            End If
        End If
        If c_click_camers.blnSel Then bln_ClickCamera = True
        'glDisable(GL_LIGHT0)
        'glDisable(GL_LIGHTING)
    End Sub
#End If '+++++++++++++++++++++++++++++++++++++++++
#If 0 Then '+++++++++++++++++++++++++++++++++++++++++++++++++
    Dim icdDoc As IntelliCAD.Document
    Set icdDoc = Application.ActiveDocument
    Dim icdWPt As New IntelliCAD.Point
    Dim icdWLin As IntelliCAD.Line
    Set icdWPt = IntelliCAD.Library.CreatePoint(0, 0, 0)
    icdWPt.Y = icdPos.Y
    icdWPt.Z = icdPos.Z
    icdWPt.X = icdPos.X + 1000#
    Set icdWLin = icdDoc.ModelSpace.AddLine(icdPos, icdWPt)
    '--------------------------------
    '点の作成 0:点
    Set icdwPointEnt = icdDoc.ModelSpace.AddPointEntity(udtwInfo(i).icdStartPoint)
    '--------------------------------
    '挿入点
    Set icdInsertionPoint = Library.CreatePoint(icdEndPoint.X _
                                                    - dblDistance * Cos(dblAngle) _
                                                    + lngScale * Sin(dblAngle), _
                                                icdEndPoint.Y _
                                                    - dblDistance * Sin(dblAngle) _
                                                    - lngScale * Cos(dblAngle))
    '描画
    Set icdDesignMark = icdDoc.ModelSpace.AddText(strDesignMark, _
                                                    icdInsertionPoint, _
                                                    CDbl(lngScale * 3&))
    icdDesignMark.Update
    'スタイル
    icdDesignMark.StyleName = "Standard"
    icdDesignMark.ScaleFactor = icdDoc.TextStyles("Standard").Width
    '回転
    Call icdDesignMark.Rotate(icdInsertionPoint, dblAngle)
    '合せ位置
    icdDesignMark.HorizontalAlignment = vicHorizontalAlignmentCenter
    icdDesignMark.VerticalAlignment = vicVerticalAlignmentTop
    fncDrawingBraceDesignMark = True
    icdDesignMark.Update
#End If '++++++++++++++++++++++++++++++++++++++++++++++++++++
    ''--この関数は使えない

    'Sub setForeGroundWin()
    '    Dim setParentRet, setWndLongRet As Long
    '    Dim icadWnd As IntelliCAD.Window
    '    'Dim wndPosRet As Long
    '    '****************************************
    '    If CheckICADStatus() <> 0 Then
    '        Exit Sub
    '    End If
    '    icadWnd = g_IJcadApp.ActiveWindow
    '    setParentRet = GetParent(icadWnd.WindowHandle32)
    '    SetForegroundWindow(setParentRet)
    '    'setParentRet = SetParent(Me.hwnd, setParentRet)
    '    'SetForegroundWindow(Me.hwnd)
    '    'If Err() <> 0.0# Then
    '    '    MsgBox("すみません、親ウィンドウを設置できません。")
    '    'End If
    'End Sub
#End Region



End Module



Public Class IJCAD7Entity
    Implements ICADEntity

    Private m_icdEnt As IntelliCAD.Entity

    Public Sub New(ByRef icdEnt As IntelliCAD.Entity)
        m_icdEnt = icdEnt
    End Sub

    Private Shared Function CreatePointFr(ByRef pt As GeoPoint) As IntelliCAD.Point
        Dim iPos As IntelliCAD.Point = g_IJcadApp.Library.CreatePoint(pt.x, pt.y, pt.z)
        Return iPos
    End Function

    Public Sub Move(ByRef a As GeoPoint, ByRef origin As GeoPoint) Implements ICADEntity.Move
        m_icdEnt.Move(CreatePointFr(a), CreatePointFr(origin))
    End Sub

    Public Sub Rotate3D(ByRef origin As GeoPoint, ByRef dir As GeoPoint, ByVal angle As Double) Implements ICADEntity.Rotate3D
        m_icdEnt.Rotate3D(CreatePointFr(origin), CreatePointFr(dir), angle)
    End Sub

    Public Sub Update() Implements ICADEntity.Update
        m_icdEnt.Update()
    End Sub
End Class

'Public Class IJCAD7Operator
'    Implements ICADOperator

'    Private m_icdDoc As IntelliCAD.Document

'    Public Sub New(ByRef icdDoc As IntelliCAD.Document)
'        m_icdDoc = icdDoc
'    End Sub

'    Public Sub CreateLayer(ByVal strLayerName As String) Implements ICADOperator.CreateLayer
'        If (m_icdDoc Is Nothing) Then Exit Sub

'        Dim IcadLayer As IntelliCAD.Layer
'        On Error Resume Next
'        IcadLayer = icadDoc.Layers(strLayerName)
'        If IcadLayer Is Nothing Then
'            IcadLayer = icadDoc.Layers.Add(strLayerName)
'        End If
'    End Sub

'    Public Sub CreateLayerEntSet(ByVal entset As Sys_Setting.EntSetting) Implements ICADOperator.CreateLayerEntSet
'        If (m_icdDoc Is Nothing) Then Exit Sub

'        On Error Resume Next
'        Dim strCmd As String
'        Dim strLayerName As String
'        Dim IcadLayer As IntelliCAD.Layer
'        strLayerName = entset.layerName
'        IcadLayer = icadDoc.Layers(strLayerName)
'        If IcadLayer Is Nothing Then
'            strCmd = "-LAYER N " & (strLayerName) & ";;"
'            Call g_IJcadApp.RunCommand(strCmd)
'            YmdSleep()
'        End If

'        ' レイヤの色、線種を設定

'        strCmd = "-LAYER C T " & _
'                (entset.color.dbl_red * 255) & "," & _
'                (entset.color.dbl_green * 255) & "," & _
'                (entset.color.dbl_blue * 255) & _
'                " " & strLayerName & ";;"
'        Call g_IJcadApp.RunCommand(strCmd)
'        YmdSleep()
'    End Sub

'    Public Sub AddPoint(ByRef cpos As CLookPoint, ByVal strLayerName As String) Implements ICADOperator.AddPoint
'        If (m_icdDoc Is Nothing) Then Exit Sub

'        Dim iPos As IntelliCAD.Point, icdEnt As IntelliCAD.Entity
'        iPos = g_IJcadApp.Library.CreatePoint(cpos.Real_x, cpos.Real_y, cpos.Real_z)
'        icdEnt = m_icdDoc.ModelSpace.AddPointEntity(iPos)
'        icdEnt.Layer = strLayerName   '"LOOKPOINT"
'        icdEnt.Update()
'    End Sub

'    Public Sub AddLine(ByRef pRS As GeoPoint, ByRef pRE As GeoPoint, ByVal strLayerName As String, ByVal Mcolor As Sys_Setting.ModelColor) Implements ICADOperator.AddLine
'        If (m_icdDoc Is Nothing) Then Exit Sub

'        Dim icdEnt As IntelliCAD.Entity
'        Dim iPS As IntelliCAD.Point, iPE As IntelliCAD.Point
'        iPS = g_IJcadApp.Library.CreatePoint(pRS.x, pRS.y, pRS.z)

'        iPE = g_IJcadApp.Library.CreatePoint(pRE.x, pRE.y, pRE.z)
'        icdEnt = m_icdDoc.ModelSpace.AddLine(iPS, iPE)
'        icdEnt.Layer = strLayerName '"RAY"
'        icdEnt.Update()
'    End Sub

'    Public Sub AddText(ByVal text As String, ByRef loc As GeoPoint, _
'                       ByVal dH As Double, ByVal strLayerName As String) Implements ICADOperator.AddText
'        If (m_icdDoc Is Nothing) Then Exit Sub

'        Dim iPS As IntelliCAD.Point, icdEnt As IntelliCAD.Text
'        iPS = g_IJcadApp.Library.CreatePoint(loc.x, loc.y, loc.z)
'        icdEnt = m_icdDoc.ModelSpace.AddText(text, iPS, dH)
'        With icdEnt
'            .Update()
'            .Layer = strLayerName '"LABELTEXT"
'            '#If IJCAD = 7 Then
'            '                            .color = IntelliCAD.Colors.vicWhite
'            '#Else
'            '                            .color.ColorIndex = IntelliCAD.Colors.vicWhite
'            '#End If
'            .HorizontalAlignment = IntelliCAD.HorizontalAlignment.vicHorizontalAlignmentCenter
'            .VerticalAlignment = IntelliCAD.VerticalAlignment.vicVerticalAlignmentMiddle
'            .Rotation = 0.0#
'            .StyleName = "Standard"
'            .ScaleFactor = 1.0#
'            .TextAlignmentPoint = iPS
'            .Update()
'        End With
'    End Sub

'    Public Function AddCircle(ByRef loc As GeoPoint, ByVal radius As Double, ByVal strLayerName As String, ByVal MColor As Sys_Setting.ModelColor) As ICADEntity Implements ICADOperator.AddCircle
'        Dim iPS As IntelliCAD.Point
'        Dim icdEnt As IntelliCAD.Entity
'        iPS = g_IJcadApp.Library.CreatePoint(loc.x, loc.y, loc.z)
'        icdEnt = m_icdDoc.ModelSpace.AddCircle(iPS, radius)
'        icdEnt.Layer = strLayerName
'        icdEnt.Update()

'        Dim iEnt As New IJCAD7Entity(icdEnt)

'        Return iEnt
'    End Function
'End Class
