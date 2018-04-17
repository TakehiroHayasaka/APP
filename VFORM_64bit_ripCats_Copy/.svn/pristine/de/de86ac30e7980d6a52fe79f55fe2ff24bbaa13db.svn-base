
Public Interface ICADEntity
    Sub Move(ByRef a As GeoPoint, ByRef origin As GeoPoint)
    Sub Rotate3D(ByRef origin As GeoPoint, ByRef dir As GeoPoint, ByVal angle As Double)
    Sub Update()
End Interface

Public Interface ICADOperator
    Sub CreateLayer(ByVal strLayerName As String)
    Sub CreateLayerEntSet(ByVal entset As Sys_Setting.EntSetting)
    Sub AddPoint(ByRef cpos As CLookPoint, ByVal strLayerName As String)
    Sub AddLine(ByRef pRS As GeoPoint, ByRef pRE As GeoPoint, ByVal strLayerName As String, ByVal Mcolor As Sys_Setting.ModelColor) '線種を追加
    Sub AddText(ByVal text As String, ByRef loc As GeoPoint, ByVal dH As Double, ByVal strLayerName As String)
    Function AddCircle(ByRef loc As GeoPoint, ByVal radius As Double, ByVal Nor As GeoVector, ByVal strLayerName As String, ByVal Mcolor As Sys_Setting.ModelColor) As ICADEntity
    ' 2014-5-30 str by Ljj
    Sub AddText_Information(ByVal text As String, ByRef loc As GeoPoint, ByVal dH As Double, ByVal strLayerName As String)

    ' 2014-5-30 end by Ljj
End Interface


Module OutputCAD
    'Private Sub transOutCoord(ByVal iOutCoord As Integer, ByRef x As Double, ByRef y As Double, ByRef z As Double)
    Public Sub transOutCoord(ByVal iOutCoord As Integer, ByRef x As Double, ByRef y As Double, ByRef z As Double)
        'iOutCoord = 0:3次元、=1:XY、=2:XZ、=3:YZ
        Select Case iOutCoord
            Case 0  ' 3次元

                Exit Sub
            Case 1  ' XY平面
                z = 0.0#
            Case 2  ' XZ平面
                y = z
                z = 0.0#
            Case 3  ' YZ平面
                x = y
                y = z
                z = 0.0#
        End Select
    End Sub

    Private Function DegToRadian(ByVal dblAng As Double) As Double
        DegToRadian = (dblAng * 3.141592654) / 180.0#
    End Function

    '============================================================
    ' 機能：無単位系から設計座標系への変換
    '============================================================
    'Private Function localToGlobalPoint(ByRef gpPos As GeoPoint, ByVal mat() As Double) As Integer
    Public Function localToGlobalPoint(ByRef gpPos As GeoPoint, ByVal mat() As Double) As Integer
        Dim lookmat(0 To 3) As Double
        Dim XM(4, 4) As Double
        lookmat(0) = gpPos.x : lookmat(1) = gpPos.y : lookmat(2) = gpPos.z : lookmat(3) = 0
        lookmat(0) = gpPos.x * mat(0) + gpPos.y * mat(4) + gpPos.z * mat(8) + 1.0 * mat(12)
        lookmat(1) = gpPos.x * mat(1) + gpPos.y * mat(5) + gpPos.z * mat(9) + 1.0 * mat(13)
        lookmat(2) = gpPos.x * mat(2) + gpPos.y * mat(6) + gpPos.z * mat(10) + 1.0 * mat(14)
        gpPos.x = lookmat(0) * sys_ScaleInfo.scale : gpPos.y = lookmat(1) * sys_ScaleInfo.scale : gpPos.z = lookmat(2) * sys_ScaleInfo.scale
    End Function

    '20170214 baluu add start
    Public Function localToGlobalLookPoint(ByRef lookP As CLookPoint, ByVal mat() As Double) As Integer
        Dim lookmat(0 To 3) As Double
        Dim XM(4, 4) As Double
        lookmat(0) = lookP.x : lookmat(1) = lookP.y : lookmat(2) = lookP.z : lookmat(3) = 0
        lookmat(0) = lookP.x * mat(0) + lookP.y * mat(4) + lookP.z * mat(8) + 1.0 * mat(12)
        lookmat(1) = lookP.x * mat(1) + lookP.y * mat(5) + lookP.z * mat(9) + 1.0 * mat(13)
        lookmat(2) = lookP.x * mat(2) + lookP.y * mat(6) + lookP.z * mat(10) + 1.0 * mat(14)
        lookP.x = lookmat(0) * sys_ScaleInfo.scale : lookP.y = lookmat(1) * sys_ScaleInfo.scale : lookP.z = lookmat(2) * sys_ScaleInfo.scale
    End Function
    '20170214 baluu add end

    '============================================================
    ' 機能：計測点のCAD出力

    '============================================================
    '★20121114計測点(CAD出力は?⇒ターゲット面・中心円・十字線すべて必要か?)
    Public Sub exportLookPoints(ByRef cad As ICADOperator, ByVal iOutCoord As Integer)
        Dim ii As Long
        Dim cpos As CLookPoint
        If nLookPoints > 0 Then
            If entset_point.blnVisiable Then
                cad.CreateLayer(entset_point.layerName)  '"LOOKPOINT")
                cad.CreateLayerEntSet(entset_point)

                For ii = 0 To nLookPoints - 1
                    If gDrawPoints(ii).blnDraw Then
                        If gDrawPoints(ii).type <> 2 Then   'コードターゲットは表示しない

                            cpos = gDrawPoints(ii)
                            '--出力次元による座標値の調整
                            Call transOutCoord(iOutCoord, cpos.Real_x, cpos.Real_y, cpos.Real_z)

                            cad.AddPoint(cpos, entset_point.layerName)
                        End If
                    End If
                Next
            End If
        End If
    End Sub

    '============================================================
    ' 機能：レイのCAD出力

    '============================================================
    Public Sub exportRay(ByRef cad As ICADOperator, ByVal iOutCoord As Integer)
        Dim ii As Long
        Dim pRS As New GeoPoint
        Dim pRE As New GeoPoint
        If nRays > 0 Then
            If entset_ray.blnVisiable Then
                cad.CreateLayer(entset_ray.layerName) ' "RAY")
                cad.CreateLayerEntSet(entset_ray)
                For ii = 0 To nRays - 1
                    If gDrawRays(ii).blnDraw Then
                        ' start point
                        pRS = gDrawRays(ii).startPnt.Copy
                        localToGlobalPoint(pRS, sys_CoordInfo.mat)
                        '--出力次元による座標値の調整
                        Call transOutCoord(iOutCoord, pRS.x, pRS.y, pRS.z)

                        ' end point
                        pRE = gDrawRays(ii).endPnt.Copy
                        localToGlobalPoint(pRE, sys_CoordInfo.mat)
                        '--出力次元による座標値の調整
                        Call transOutCoord(iOutCoord, pRE.x, pRE.y, pRE.z)

                        cad.AddLine(pRS, pRE, entset_ray.layerName, entset_ray.color)
                    End If
                Next
            End If
        End If
    End Sub

    '============================================================
    ' 機能：計測ラベル文字のCAD出力

    '============================================================
    Public Sub exportLabelText(ByRef cad As ICADOperator, ByVal dH As Double, ByVal iOutCoord As Integer)
        Dim ii As Long
        Dim pRS As New GeoPoint
        If nLabelText > 0 Then
            If entset_label.blnVisiable Then
                cad.CreateLayer(entset_label.layerName) ' "LABELTEXT")
                cad.CreateLayerEntSet(entset_label)

                For ii = 0 To nLabelText - 1

                    '(20151207 Tezuka ADD) STの出力制御
                    If Data_Point.Combo_Single.SelectedIndex = 1 Then
                        If gDrawLabelText(ii).LabelName.Substring(0, 2) = "ST" Then
                            Continue For
                        End If
                    End If

                    If gDrawLabelText(ii).blnDraw Then
                        'gDrawLabelText(ii).mid()
                        Call pRS.setXYZ(gDrawLabelText(ii).x, gDrawLabelText(ii).y, gDrawLabelText(ii).z)
                        localToGlobalPoint(pRS, sys_CoordInfo.mat)
                        '--出力次元による座標値の調整
                        Call transOutCoord(iOutCoord, pRS.x, pRS.y, pRS.z)

                        cad.AddText(gDrawLabelText(ii).LabelName, pRS, dH, entset_label.layerName)

                    End If
                Next
            End If
        End If
    End Sub

    Public Sub GetPts(ByVal loc As GeoPoint)
        Return
    End Sub

    '============================================================
    ' 機能：ユーザ作成図形のCAD出力

    '============================================================
    Public Sub exportUserElm(ByRef cad As ICADOperator, ByVal iOutCoord As Integer)
        Dim mcol As ModelColor
        Dim ii As Long
        Dim pRS As New GeoPoint
        Dim pRE As New GeoPoint
        Dim dblRad As Double, dblAng As Double

        '--        CreateLayer(icdDoc, "USER")
        'cad.CreateLayer(entset_line_CAD.layerName)
        'cad.CreateLayer(entset_circle_CAD.layerName)
        'cad.CreateLayer(entset_line.layerName)
        'cad.CreateLayer(entset_circle.layerName)
        cad.CreateLayerEntSet(entset_line_CAD)
        cad.CreateLayerEntSet(entset_circle_CAD)
        cad.CreateLayerEntSet(entset_line)
        cad.CreateLayerEntSet(entset_circle)

        'g_IJcadApp.Version
        '線分図形の出力

        If nUserLines > 0 Then
            If entset_line.blnVisiable Then
                For ii = 0 To nUserLines - 1
                    If (gDrawUserLines(ii).binDelete = False) Then
                        If (gDrawUserLines(ii).blnDelOnGlwin = False) Then 'Add Kiryu 3Dビューワで削除した図形は出力しない CATS2016-a3-20160920
                            If gDrawUserLines(ii).blnDraw Then
                                ' start point
                                pRS = gDrawUserLines(ii).startPnt.Copy

                                localToGlobalPoint(pRS, sys_CoordInfo.mat)
                                '--出力次元による座標値の調整
                                Call transOutCoord(iOutCoord, pRS.x, pRS.y, pRS.z)
                                ' end point
                                pRE = gDrawUserLines(ii).endPnt.Copy
                                localToGlobalPoint(pRE, sys_CoordInfo.mat)
                                Call transOutCoord(iOutCoord, pRE.x, pRE.y, pRE.z)

                                '--                        icdEnt.Layer = "USER"
                                Dim layerName As String

                                'Rep By Suuri Sta 20140811 -------------------------
                                'If (gDrawUserLines(ii).elmType = 0) Then
                                '    layerName = entset_line.layerName
                                'Else
                                '    layerName = entset_line_CAD.layerName
                                'End If
                                layerName = gDrawUserLines(ii).layerName
                                cad.CreateLayer(layerName)
                                'Rep By Suuri End 20140811 -------------------------

                                Try
                                    'icdEnt.Linetype = gDrawUserLines(ii).lineTypeCode
                                    mcol = YCM_GetColorInfoByCode(gDrawUserLines(ii).colorCode)
                                    cad.AddLine(pRS, pRE, layerName, mcol)

                                    '    Call icdEnt.color.SetRGB(CInt(mcol.dbl_red * 255), CInt(mcol.dbl_green * 255), CInt(mcol.dbl_blue * 255))
                                    ''icdEnt.color.ColorIndex = gDrawUserLines(ii).colorCode
                                Catch ex As Exception
                                End Try
                            End If
                        End If
                    End If
                Next
            End If
        End If
        '円図形の出力

        If (nCircleNew > 0) Then
            If entset_circle.blnVisiable Then
                For ii = 0 To nCircleNew - 1
                    If (gDrawCircleNew(ii).binDelete = False) Then
                        If gDrawCircleNew(ii).blnDraw Then
                            If (gDrawCircleNew(ii).blnDelOnGlwin = False) Then 'Add Kiryu 3Dビューワで削除した図形は出力しない CATS2016-a3-20160920
                                pRS = gDrawCircleNew(ii).org.Copy
                                localToGlobalPoint(pRS, sys_CoordInfo.mat)
                                '--出力次元による座標値の調整
                                Call transOutCoord(iOutCoord, pRS.x, pRS.y, pRS.z)
                                dblRad = gDrawCircleNew(ii).r * sys_ScaleInfo.scale

                                Dim layerName As String
                                If (gDrawCircleNew(ii).elmType = 0) Then
                                    layerName = entset_circle.layerName
                                Else
                                    layerName = entset_circle_CAD.layerName
                                End If
                                'Add By Suuri Sta 20140811 ------------------ 
                                layerName = gDrawCircleNew(ii).layerName
                                cad.CreateLayer(layerName)
                                'Add By Suuri End 20140811 ------------------
                                Try
                                    'icdEnt.Linetype = gDrawCircleNew(ii).lineTypeCode
                                    mcol = YCM_GetColorInfoByCode(gDrawCircleNew(ii).colorCode)
                                    'Call icdEnt.color.SetRGB(CInt(mcol.dbl_red * 255), CInt(mcol.dbl_green * 255), CInt(mcol.dbl_blue * 255))
                                    'icdEnt.color.ColorIndex = gDrawCircleNew(ii).colorCode

                                Catch ex As Exception
                                End Try

                                Dim icdEnt As ICADEntity = cad.AddCircle(pRS, dblRad, gDrawCircleNew(ii).Vec, layerName, mcol)

                                'iPE = g_IJcadApp.Library.CreatePoint((pRS.x + 1000.0#), pRS.y, pRS.z)
                                'If (iOutCoord = 0) Then
                                '    Dim iOrg As New GeoPoint()
                                '    Dim iDir As New GeoPoint()
                                '    iOrg.setXYZ(0.0, 0.0, 0.0)
                                '    iDir.setXYZ(1000.0, 0.0, 0.0)
                                '    dblAng = DegToRadian(gDrawCircleNew(ii).x_angle)
                                '    If (Math.Abs(dblAng) > 0.0#) Then
                                '        'icdEnt.Rotate3D(iPS, iPE, dblAng)
                                '        icdEnt.Move(pRS, iOrg)
                                '        icdEnt.Update()
                                '        icdEnt.Rotate3D(iOrg, iDir, dblAng)
                                '        icdEnt.Update()
                                '        icdEnt.Move(iOrg, pRS)
                                '        icdEnt.Update()
                                '    End If
                                '    'iPE = g_IJcadApp.Library.CreatePoint(pRS.x, (pRS.y + 1000.0#), pRS.z)
                                '    'iDir = g_IJcadApp.Library.CreatePoint(0.0, 1000.0, 0.0)
                                '    iDir.setXYZ(0.0, 1000.0, 0.0)
                                '    dblAng = DegToRadian(gDrawCircleNew(ii).y_angle)
                                '    If (Math.Abs(dblAng) > 0.0#) Then
                                '        'icdEnt.Rotate3D(iPS, iPE, dblAng)
                                '        icdEnt.Move(pRS, iOrg)
                                '        icdEnt.Update()
                                '        icdEnt.Rotate3D(iOrg, iDir, dblAng)
                                '        icdEnt.Update()
                                '        icdEnt.Move(iOrg, pRS)
                                '        icdEnt.Update()
                                '    End If
                                'End If



                            End If
                        End If
                    End If
                Next
                'CStr(entset_circle.color.code)
                'CStr(entset_circle.linetype.code)
            End If
        End If

        Return

    End Sub

    '20170214 baluu add start
    Public Sub exportReconstructedObject(ByRef cad As ICADOperator, ByVal iOutCoord As Integer)
        Dim layerName = "ReconstructedObject"
        cad.CreateLayer(layerName) 'TOBEADDEDASSETTING
        If Not modelPoints Is Nothing Then
            For Each vertex As ObjVertex In modelPoints.Vertices
                Dim point As New CLookPoint(vertex.x, vertex.y, vertex.z)
                localToGlobalLookPoint(point, sys_CoordInfo.mat)
                Call transOutCoord(iOutCoord, point.x, point.y, point.z)
                cad.AddPoint(point, layerName)
            Next
        End If
    End Sub
    '20170214 baluu add end



    Public Sub exportLookPoints(ByRef file As System.IO.StreamWriter)
        Dim ii As Long
        Dim cpos As CLookPoint
        If nLookPoints > 0 Then
            If entset_point.blnVisiable Then

                For ii = 0 To nLookPoints - 1
                    If gDrawPoints(ii).blnDraw Then
                        ' If gDrawPoints(ii).type <> 2 Then   'コードターゲットは表示しない

                        cpos = gDrawPoints(ii)
                        file.WriteLine("  0")
                        file.WriteLine("POINT")
                        file.WriteLine("  8")
                        file.WriteLine("0")

                        '点
                        file.WriteLine(" 10")
                        file.WriteLine(cpos.x * sys_ScaleInfo.scale)
                        file.WriteLine(" 20")
                        file.WriteLine(cpos.y * sys_ScaleInfo.scale)
                        file.WriteLine(" 30")
                        file.WriteLine(cpos.z * sys_ScaleInfo.scale)

                        'End If
                    End If
                Next
            End If
        End If
    End Sub

    Public Sub exportRay(ByRef file As System.IO.StreamWriter)

    End Sub

    Public Sub exportLabelText(ByRef file As System.IO.StreamWriter, ByVal dH As Double)
        Dim pRS As New GeoPoint
        If nLabelText > 0 Then
            If entset_label.blnVisiable Then
                For ii = 0 To nLabelText - 1
                    If Data_Point.Combo_Single.SelectedIndex = 1 Then
                        If gDrawLabelText(ii).LabelName.Substring(0, 2) = "ST" Then
                            Continue For
                        End If
                    End If

                    If gDrawLabelText(ii).blnDraw Then
                        'gDrawLabelText(ii).mid()
                        Call pRS.setXYZ(gDrawLabelText(ii).x, gDrawLabelText(ii).y, gDrawLabelText(ii).z)
                        localToGlobalPoint(pRS, sys_CoordInfo.mat)
                        file.WriteLine("  0")
                        file.WriteLine("TEXT")
                        file.WriteLine("  8")
                        file.WriteLine("0")
                        file.WriteLine("  1")
                        file.WriteLine(gDrawLabelText(ii).LabelName)
                        '点
                        file.WriteLine(" 10")
                        file.WriteLine(pRS.x)
                        file.WriteLine(" 20")
                        file.WriteLine(pRS.y)
                        file.WriteLine(" 30")
                        file.WriteLine(pRS.z)
                        file.WriteLine(" 40")
                        file.WriteLine(dH)
                    End If
                Next
            End If
        End If
    End Sub

    Public Sub exportUserElm(ByRef file As System.IO.StreamWriter)
        Dim mcol As ModelColor
        Dim ii As Long
        Dim pRS As New GeoPoint
        Dim pRE As New GeoPoint
        Dim dblRad As Double, dblAng As Double

    
        'g_IJcadApp.Version
        '線分図形の出力

        If nUserLines > 0 Then
            If entset_line.blnVisiable Then
                For ii = 0 To nUserLines - 1
                    If (gDrawUserLines(ii).binDelete = False) Then
                        If (gDrawUserLines(ii).blnDelOnGlwin = False) Then 'Add Kiryu 3Dビューワで削除した図形は出力しない CATS2016-a3-20160920
                            If gDrawUserLines(ii).blnDraw Then
                                ' start point
                                pRS = gDrawUserLines(ii).startPnt.Copy

                                localToGlobalPoint(pRS, sys_CoordInfo.mat)
                             
                                ' end point
                                pRE = gDrawUserLines(ii).endPnt.Copy
                                localToGlobalPoint(pRE, sys_CoordInfo.mat)

                                Dim layerName As String

                                layerName = gDrawUserLines(ii).layerName

                                Try
                                    'icdEnt.Linetype = gDrawUserLines(ii).lineTypeCode
                                    mcol = YCM_GetColorInfoByCode(gDrawUserLines(ii).colorCode)
                                    file.WriteLine("  0")
                                    file.WriteLine("LINE")
                                    file.WriteLine("  8")
                                    file.WriteLine("0")
                                    '1点目
                                    file.WriteLine(" 10")
                                    file.WriteLine(pRS.x)
                                    file.WriteLine(" 20")
                                    file.WriteLine(pRS.y)
                                    file.WriteLine(" 30")
                                    file.WriteLine(pRS.z)
                                    '2点
                                    file.WriteLine(" 11")
                                    file.WriteLine(pRE.x)
                                    file.WriteLine(" 21")
                                    file.WriteLine(pRE.y)
                                    file.WriteLine(" 31")
                                    file.WriteLine(pRE.z)
                                    file.WriteLine(" 62")
                                    file.WriteLine(mcol.code)
                                Catch ex As Exception
                                End Try
                            End If
                        End If
                    End If
                Next
            End If
        End If
        '円図形の出力

        If (nCircleNew > 0) Then
            If entset_circle.blnVisiable Then
                For ii = 0 To nCircleNew - 1
                    If (gDrawCircleNew(ii).binDelete = False) Then
                        If gDrawCircleNew(ii).blnDraw Then
                            If (gDrawCircleNew(ii).blnDelOnGlwin = False) Then 'Add Kiryu 3Dビューワで削除した図形は出力しない CATS2016-a3-20160920
                                pRS = gDrawCircleNew(ii).org.Copy
                                localToGlobalPoint(pRS, sys_CoordInfo.mat)
                                dblRad = gDrawCircleNew(ii).r * sys_ScaleInfo.scale

                                Dim layerName As String
                                If (gDrawCircleNew(ii).elmType = 0) Then
                                    layerName = entset_circle.layerName
                                Else
                                    layerName = entset_circle_CAD.layerName
                                End If

                                layerName = gDrawCircleNew(ii).layerName
                              
                                Try
                                    mcol = YCM_GetColorInfoByCode(gDrawCircleNew(ii).colorCode)
                                Catch ex As Exception
                                End Try

                                '   Dim icdEnt As ICADEntity = cad.AddCircle(pRS, dblRad, gDrawCircleNew(ii).Vec, layerName, mcol)

                                file.WriteLine("  0")
                                file.WriteLine("CIRCLE")
                                file.WriteLine("  8")
                                file.WriteLine("0")
                                file.WriteLine(" 40")
                                file.WriteLine(dblRad)
                                '1点目
                                file.WriteLine(" 10")
                                file.WriteLine(pRS.x)
                                file.WriteLine(" 20")
                                file.WriteLine(pRS.y)
                                file.WriteLine(" 30")
                                file.WriteLine(pRS.z)
                                file.WriteLine(" 62")
                                file.WriteLine(mcol.code)

                            End If
                        End If
                    End If
                Next
            End If
        End If

        Return

    End Sub

    Public Sub exportReconstructedObject(ByRef file As System.IO.StreamWriter)

     

        For Each mesh In meshes
            For i = 0 To mesh.Triangles.Count - 1 Step 3
                Dim triangle1 = mesh.Triangles(i)
                Dim triangle2 = mesh.Triangles(i + 1)
                Dim triangle3 = mesh.Triangles(i + 2)
                file.WriteLine("  0")
                file.WriteLine("3DFACE")
                file.WriteLine("  8")
                file.WriteLine("0")
                '1点目
                file.WriteLine(" 10")
                file.WriteLine(triangle1.x * ScaleToMM)
                file.WriteLine(" 20")
                file.WriteLine(triangle1.y * ScaleToMM)
                file.WriteLine(" 30")
                file.WriteLine(triangle1.z * ScaleToMM)
                '2点目
                file.WriteLine(" 11")
                file.WriteLine(triangle2.x * ScaleToMM)
                file.WriteLine(" 21")
                file.WriteLine(triangle2.y * ScaleToMM)
                file.WriteLine(" 31")
                file.WriteLine(triangle2.z * ScaleToMM)
                '3点目
                file.WriteLine(" 12")
                file.WriteLine(triangle3.x * ScaleToMM)
                file.WriteLine(" 22")
                file.WriteLine(triangle3.y * ScaleToMM)
                file.WriteLine(" 32")
                file.WriteLine(triangle3.z * ScaleToMM)
                '4点目兼1点目
                file.WriteLine(" 13")
                file.WriteLine(triangle1.x * ScaleToMM)
                file.WriteLine(" 23")
                file.WriteLine(triangle1.y * ScaleToMM)
                file.WriteLine(" 33")
                file.WriteLine(triangle1.z * ScaleToMM)
            Next
        Next
    
    End Sub

End Module

