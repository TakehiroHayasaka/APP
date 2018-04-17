﻿Imports System
Imports System.Threading
Module YCM_Command
    'Public Event Completed As EventHandler
    Public Sub Command_distance2Point()
        Dim cp As New CLookPoint
        Dim cp1 As New GeoPoint
        Dim cp2 As New GeoPoint
        bindistance2pointstart = True
        If IOUtil.GetPoint(cp, "1点目を指示：") = -1 Then Exit Sub
        cp1.x = cp.Real_x
        cp1.y = cp.Real_y
        cp1.z = cp.Real_z
        If IOUtil.GetPoint(cp, "2点目を指示：") = -1 Then Exit Sub
        cp2.x = cp.Real_x
        cp2.y = cp.Real_y
        cp2.z = cp.Real_z
        Dim distance As Double = cp1.GetDistanceTo(cp2)
        IOUtil.WritePrompt("2点間距離：" & distance.ToString)
        bindistance2pointstart = False
    End Sub
    Public Sub SystemTitle() '20121129
        Dim SystemTitle As String
        SystemTitle = "YCM　Ver 1.0"
    End Sub
    '    Public Sub Command_Open()
    '        IOUtil.EndThread()
    '        Dim db_b As Boolean = False
    '        Dim dbPath As String = ""
    '        Dim dataArray As New ArrayList
    '        Bln_setCoord = False
    '        g_InputIsavePath = ""
    '        'OpenFileDialog1.FileName = "*.mdb"
    '        'OpenFileDialog1.Filter = "Access(*.mdb)|*.mdb"
    '        'Dim result As System.Windows.Forms.DialogResult = OpenFileDialog1.ShowDialog()
    '        'If result = System.Windows.Forms.DialogResult.OK Then
    '        '    m_strDataBasePath = OpenFileDialog1.FileName()
    '        'End If
    '        'Dim path_file As String
    '        Dim strdatabasefile As String = ""
    '        m_strDataBasePath = ""
    '        m_strDataBasePath = ComSel_SelectFolderByShell("計測データ.mdbが入っているフォルダーを指定して下さい。", False)
    '        If m_strDataBasePath <> Nothing Then
    '            strdatabasefile = m_strDataBasePath & "\計測データ.mdb"
    '            Dim iodatabasefile As New IO.FileInfo(strdatabasefile)
    '            If iodatabasefile.Exists = True Then

    '                g_InputIsavePath = m_strDataBasePath
    '                Dim strtempend As String = m_strDataBasePath & "\計測データ_Temp.mdb"
    '                FileCopy(strdatabasefile, strtempend)
    '                g_InputIsavePath = m_strDataBasePath
    '                m_strDataBasePath = strtempend
    '                Dim clsOPe As New CDBOperate

    '                If clsOPe.ConnectDB(m_strDataBasePath) = False Then
    '                    'MsgBox("Error!")
    '                    Exit Sub
    '                End If
    '                Call YCM_UpdateDBbyLayer(clsOPe)   '--「dispsetting」「userfigure」テーブルに[レイヤ名]フィールドを追加
    '                '--ins.start----------------------12.10.12
    '                ' 座標補間点情報テーブルが存在しない場合には作成する
    '                YCM_AddNewInterPosInfoTable(clsOPe)
    '                '--ins.end------------------------12.10.12
    '                clsOPe.DisConnectDB()

    '                Dim intmat As Integer
    '                Dim dblmat(1, 15) As Double
    '                YCM_ReadSystemscalesettingAcs(m_strDataBasePath)
    '                YCM_ReadSystemscalesettingAcsvalue(m_strDataBasePath)
    '                YCM_ReadSystemscalesettingAcsP1_2(m_strDataBasePath) '13.1.25　スケール設定を行った2点をDB（userscalesetting）から呼び出す

    '                intmat = YCM_ReadSystemscalesettingAcsmat(m_strDataBasePath)

    '                If intmat = 1 Then
    '                    sys_CoordInfo.mat = YCM_GetUnitMat()
    '                End If
    '                If intmat = 2 Then
    '                    For ii As Integer = 0 To 15
    '                        sys_CoordInfo.mat(ii) = CDbl(system_scalesettingmat(0, ii))
    '                    Next
    '                End If
    '                If nscalesettingvalue = 0 Then
    '                    sys_ScaleInfo.scale = 1.0
    '                Else
    '                    sys_ScaleInfo.scale = System_scalesettingvalue(0, nscalesetting - 1)
    '                End If

    '                'Dim strSQL As String = "SELECT distinct systemlabel,X,Y,Z FROM measurepoint3d  where flgLabel=1"
    '                'Dim adoRet As ADODB.Recordset

    '                ''adoRet = clsOPe.CreateRecordset(strSQL)

    '                ''If adoRet.RecordCount > 0 Then
    '                ''If db_b = False Then
    '                ''    For i As Integer = 1 To 4
    '                ''        Data_Point.DGV_DV.Columns.RemoveAt(1)
    '                ''    Next
    '                ''End If
    '                'db_b = True
    '                ''Data_Point.DGV_DV.DataSource = ZS_GetDataSource(strSQL)
    '                'adoRet = clsOPe.CreateRecordset(strSQL)
    '                'Dim iiii As Integer
    '                'If adoRet.RecordCount > 0 Then

    '                '    Do Until adoRet.EOF
    '                '        Data_Point.DGV_DV.Rows.Add()
    '                '        Data_Point.DGV_DV.Rows(iiii).Cells(1).Value = adoRet("systemlabel").Value.ToString
    '                '        Data_Point.DGV_DV.Rows(iiii).Cells(2).Value = adoRet("X").Value.ToString
    '                '        Data_Point.DGV_DV.Rows(iiii).Cells(3).Value = adoRet("Y").Value.ToString
    '                '        Data_Point.DGV_DV.Rows(iiii).Cells(4).Value = adoRet("Z").Value.ToString
    '                '        Data_Point.DGV_DV.Rows(iiii).Cells(5).Value = adoRet("Z").Value.ToString
    '                '        adoRet.MoveNext()
    '                '    Loop
    '                'End If
    '                'Data_Point.DGV_DV.Columns(0).Width = 38
    '                'Data_Point.DGV_DV.Columns(1).Width = 63
    '                'Data_Point.DGV_DV.Columns(2).Width = 85
    '                'Data_Point.DGV_DV.Columns(3).Width = 85
    '                'Data_Point.DGV_DV.Columns(4).Width = 85

    '                'Data_Point.DGV_DV.Columns(5).Visible = False
    '                'End If
    '                'F() 'or i As Integer=


    '                '要素をなくす
    '                ReDim gDrawRays(0)
    '                ReDim gDrawCamers(0)
    '                ReDim gDrawPoints(0)
    '                ReDim gDrawLabelText(0)
    '                ReDim gDrawUserLines(0)
    '                ReDim gDrawCircleNew(0)
    '                ReDim gDrawCirclepoint(0)
    '                nRays = 0
    '                nCamers = 0
    '                nLookPoints = 0
    '                nLabelText = 0
    '                nUserLines = 0
    '                ncirclepoint = 0
    '                nCircleNew = 0
    '                gScaleLine = Nothing '13.1.28スケール設定のライン

    '                Try
    '                    Command_LoadToDb()
    '                Catch ex As Exception
    '                    MessageBox.Show(ex.ToString())
    '                End Try

    '                YCM_ReadZumenInfoFrmAcs(m_strDataBasePath)
    '                YCM_ReadSystemColorAcs(m_strDataSystemPath)
    '                YCM_ReadSystemLineTypes(m_strDataSystemPath)
    '                YCM_ReadEntSetting(m_strDataBasePath)

    '                '--補間点情報を読み込む
    '                Try
    '                    YCM_ReadInterPosInfoTable(m_strDataBasePath)
    '                Catch ex As Exception
    '                    MessageBox.Show(ex.ToString())
    '                End Try

    '                nDGV = Data_Point.DGV_DV.RowCount
    '                Data_Point.DGV_DV.Rows.Clear()
    '                For i As Integer = 0 To nLookPoints - 1
    '                    Data_Point.DGV_DV.Rows.Add()
    '                    Data_Point.DGV_DV.Rows(i).Cells(0).Value = True
    '                    Data_Point.DGV_DV.Rows(i).Cells(1).Value = gDrawPoints(i).LabelName
    '                    Data_Point.DGV_DV.Rows(i).Cells(2).Value = gDrawPoints(i).Real_x
    '                    Data_Point.DGV_DV.Rows(i).Cells(3).Value = gDrawPoints(i).Real_y
    '                    Data_Point.DGV_DV.Rows(i).Cells(4).Value = gDrawPoints(i).Real_z
    '                    Data_Point.DGV_DV.Rows(i).Cells(5).Value = gDrawPoints(i).mid
    '                Next

    '                point_name = New ArrayList
    '                For i As Integer = 0 To Data_Point.DGV_DV.RowCount - 1
    '                    point_name.Add(Data_Point.DGV_DV.Rows(i).Cells(1).Value)
    '                Next
    '                If nCamers > 0 Then
    '                    For ii = 0 To nCamers - 1
    '                        All_YCM3DVIEW_camer(gDrawCamers(ii))
    '                    Next
    '                End If
    '                If nLookPoints > 0 Then
    '                    For ii = 0 To nLookPoints - 1
    '                        All_YCM3DVIEW_point(gDrawPoints(ii))
    '                    Next
    '                End If
    '                '--del.rep.start-------------------------12.10.17
    '                ' リボンメニューに表示／非表示の状態を反映
    '                Call setRibbonMenuChkOnOff()
    '#If 0 Then
    '                'If entset_point.blnVisiable Then
    '                '    MainFrm.MM_V_9_1.Checked = True
    '                'Else
    '                '    MainFrm.MM_V_9_1.Checked = False
    '                'End If

    '                '20121114計測点(ターゲット面)
    '                 If entset_point1.blnVisiable Then
    '                    MainFrm.MM_V_9_1.Checked = True
    '                Else
    '                    MainFrm.MM_V_9_1.Checked = False
    '                End If

    '                '20121114計測点(中心円)
    '                 If entset_point2.blnVisiable Then
    '                    MainFrm.MM_V_9_1.Checked = True
    '                Else
    '                    MainFrm.MM_V_9_1.Checked = False
    '                End If

    '                '20121114計測点(十字線)
    '                 If entset_point3.blnVisiable Then
    '                    MainFrm.MM_V_9_1.Checked = True
    '                Else
    '                    MainFrm.MM_V_9_1.Checked = False
    '                End If

    '                If entset_camera.blnVisiable Then
    '                    MainFrm.MM_V_9_2.Checked = True
    '                Else
    '                    MainFrm.MM_V_9_2.Checked = False
    '                End If

    '                If entset_ray.blnVisiable Then
    '                    MainFrm.MM_V_9_3.Checked = True
    '                Else
    '                    MainFrm.MM_V_9_3.Checked = False
    '                End If

    '                If entset_label.blnVisiable Then
    '                    MainFrm.MM_V_9_4.Checked = True
    '                Else
    '                    MainFrm.MM_V_9_4.Checked = False
    '                End If

    '                If entset_line.blnVisiable And entset_circle.blnVisiable Then
    '                    MainFrm.MM_V_9_5.Checked = True
    '                Else
    '                    MainFrm.MM_V_9_5.Checked = False
    '                End If
    '#End If
    '                '--del.rep.end---------------------------12.10.17

    '                sys_ScaleInfo.p1 = getDrawPointByMID(sys_ScaleInfo.p1.mid)
    '                sys_ScaleInfo.p2 = getDrawPointByMID(sys_ScaleInfo.p2.mid)
    '                YCM_GenScaleLine(sys_ScaleInfo.p1, sys_ScaleInfo.p2)
    '                'bing_InputIsave = False

    '                'sys_ScaleInfo.scale = 1.0
    '                sys_ScaleInfo.p1 = New CLookPoint
    '                sys_ScaleInfo.p2 = New CLookPoint
    '                sys_CoordInfo.pOrg = New CLookPoint
    '                sys_CoordInfo.p1 = New CLookPoint
    '                sys_CoordInfo.p2 = New CLookPoint
    '                sys_CoordInfo.strP1XYZ = "+X"
    '                sys_CoordInfo.strP2XYZ = "+Y"
    '                sys_CoordInfo.intOxyais = 1
    '                sys_DrawCircle.dblR = 0.0#
    '                sys_DrawCircle.iCombox = 0
    '                NewOrOld = 2
    '                binfirstopen = True
    '                suballcanview()
    '                bintemptest = True
    '                MainFrm.OpenMeasureData(g_InputIsavePath)
    '                MainFrm.SetCurrentLabelTo_objFBM()
    '                ZoomAll()
    '                '★20121029スケールイメージのグレイ値リセット
    '                MainFrm.scaleImage = 100
    '            Else
    '                MsgBox("計測データ.mdbが存在しません。")
    '            End If

    '        End If
    '    End Sub

    Public Sub Command_Open()
        IOUtil.EndThread()
        Dim db_b As Boolean = False
        Dim dbPath As String = ""
        Dim dataArray As New ArrayList
        Bln_setCoord = False
        g_InputIsavePath = ""
        'OpenFileDialog1.FileName = "*.mdb"
        'OpenFileDialog1.Filter = "Access(*.mdb)|*.mdb"
        'Dim result As System.Windows.Forms.DialogResult = OpenFileDialog1.ShowDialog()
        'If result = System.Windows.Forms.DialogResult.OK Then
        '    m_strDataBasePath = OpenFileDialog1.FileName()
        'End If
        'Dim path_file As String
        Dim strdatabasefile As String = ""
        m_strDataBasePath = m_koji_kanri_path
        ' m_strDataBasePath = ComSel_SelectFolderByShell("計測データ.mdbが入っているフォルダーを指定して下さい。", False)
        If m_strDataBasePath <> Nothing Then
            strdatabasefile = m_strDataBasePath & "\計測データ.mdb"
            Dim iodatabasefile As New IO.FileInfo(strdatabasefile)
            If iodatabasefile.Exists = True Then

                g_InputIsavePath = m_strDataBasePath
                Dim strtempend As String = m_strDataBasePath & "\計測データ_Temp.mdb"
                FileCopy(strdatabasefile, strtempend)
                g_InputIsavePath = m_strDataBasePath
                m_strDataBasePath = strtempend
                Dim clsOPe As New CDBOperate

                If clsOPe.ConnectDB(m_strDataBasePath) = False Then
                    'MsgBox("Error!")
                    Exit Sub
                End If
                Call YCM_UpdateDBbyLayer(clsOPe)   '--「dispsetting」「userfigure」テーブルに[レイヤ名]フィールドを追加
                '--ins.start----------------------12.10.12
                ' 座標補間点情報テーブルが存在しない場合には作成する
                YCM_AddNewInterPosInfoTable(clsOPe)
                '--ins.end------------------------12.10.12
                clsOPe.DisConnectDB()

                Dim intmat As Integer
                Dim dblmat(1, 15) As Double
                YCM_ReadSystemscalesettingAcs(m_strDataBasePath)
                YCM_ReadSystemscalesettingAcsvalue(m_strDataBasePath)
                YCM_ReadSystemscalesettingAcsP1_2(m_strDataBasePath) '13.1.25　スケール設定を行った2点をDB（userscalesetting）から呼び出す

                intmat = YCM_ReadSystemscalesettingAcsmat(m_strDataBasePath)

                If intmat = 1 Then
                    sys_CoordInfo.mat = YCM_GetUnitMat()
                End If
                If intmat = 2 Then
                    For ii As Integer = 0 To 15
                        sys_CoordInfo.mat(ii) = CDbl(system_scalesettingmat(0, ii))
                    Next
                End If
                If nscalesettingvalue = 0 Then
                    sys_ScaleInfo.scale = 1.0
                Else
                    sys_ScaleInfo.scale = System_scalesettingvalue(0, nscalesetting - 1)
                End If

                'Dim strSQL As String = "SELECT distinct systemlabel,X,Y,Z FROM measurepoint3d  where flgLabel=1"
                'Dim adoRet As ADODB.Recordset

                ''adoRet = clsOPe.CreateRecordset(strSQL)

                ''If adoRet.RecordCount > 0 Then
                ''If db_b = False Then
                ''    For i As Integer = 1 To 4
                ''        Data_Point.DGV_DV.Columns.RemoveAt(1)
                ''    Next
                ''End If
                'db_b = True
                ''Data_Point.DGV_DV.DataSource = ZS_GetDataSource(strSQL)
                'adoRet = clsOPe.CreateRecordset(strSQL)
                'Dim iiii As Integer
                'If adoRet.RecordCount > 0 Then

                '    Do Until adoRet.EOF
                '        Data_Point.DGV_DV.Rows.Add()
                '        Data_Point.DGV_DV.Rows(iiii).Cells(1).Value = adoRet("systemlabel").Value.ToString
                '        Data_Point.DGV_DV.Rows(iiii).Cells(2).Value = adoRet("X").Value.ToString
                '        Data_Point.DGV_DV.Rows(iiii).Cells(3).Value = adoRet("Y").Value.ToString
                '        Data_Point.DGV_DV.Rows(iiii).Cells(4).Value = adoRet("Z").Value.ToString
                '        Data_Point.DGV_DV.Rows(iiii).Cells(5).Value = adoRet("Z").Value.ToString
                '        adoRet.MoveNext()
                '    Loop
                'End If
                'Data_Point.DGV_DV.Columns(0).Width = 38
                'Data_Point.DGV_DV.Columns(1).Width = 63
                'Data_Point.DGV_DV.Columns(2).Width = 85
                'Data_Point.DGV_DV.Columns(3).Width = 85
                'Data_Point.DGV_DV.Columns(4).Width = 85

                'Data_Point.DGV_DV.Columns(5).Visible = False
                'End If
                'F() 'or i As Integer=


                '要素をなくす
                ReDim gDrawRays(0)
                ReDim gDrawCamers(0)
                ReDim gDrawPoints(0)
                ReDim gDrawLabelText(0)
                ReDim gDrawUserLines(0)
                ReDim gDrawCircleNew(0)
                ReDim gDrawCirclepoint(0)
                nRays = 0
                nCamers = 0
                nLookPoints = 0
                nLabelText = 0
                nUserLines = 0
                ncirclepoint = 0
                nCircleNew = 0
                gScaleLine = Nothing '13.1.28スケール設定のライン

                Try
                    Command_LoadToDb()
                Catch ex As Exception
                    MessageBox.Show(ex.ToString())
                End Try

                YCM_ReadZumenInfoFrmAcs(m_strDataBasePath)
                YCM_ReadSystemColorAcs(m_strDataSystemPath)
                YCM_ReadSystemLineTypes(m_strDataSystemPath)
                YCM_ReadEntSetting(m_strDataBasePath)

                '--補間点情報を読み込む
                Try
                    YCM_ReadInterPosInfoTable(m_strDataBasePath)
                Catch ex As Exception
                    MessageBox.Show(ex.ToString())
                End Try

                nDGV = Data_Point.DGV_DV.RowCount
                Data_Point.DGV_DV.Rows.Clear()
                For i As Integer = 0 To nLookPoints - 1
                    Data_Point.DGV_DV.Rows.Add()
                    Data_Point.DGV_DV.Rows(i).Cells(0).Value = True
                    Data_Point.DGV_DV.Rows(i).Cells(1).Value = gDrawPoints(i).LabelName
                    Data_Point.DGV_DV.Rows(i).Cells(2).Value = gDrawPoints(i).Real_x
                    Data_Point.DGV_DV.Rows(i).Cells(3).Value = gDrawPoints(i).Real_y
                    Data_Point.DGV_DV.Rows(i).Cells(4).Value = gDrawPoints(i).Real_z
                    Data_Point.DGV_DV.Rows(i).Cells(5).Value = gDrawPoints(i).mid
                Next

                point_name = New ArrayList
                For i As Integer = 0 To Data_Point.DGV_DV.RowCount - 1
                    point_name.Add(Data_Point.DGV_DV.Rows(i).Cells(1).Value)
                Next
                If nCamers > 0 Then
                    For ii = 0 To nCamers - 1
                        All_YCM3DVIEW_camer(gDrawCamers(ii))
                    Next
                End If
                If nLookPoints > 0 Then
                    For ii = 0 To nLookPoints - 1
                        All_YCM3DVIEW_point(gDrawPoints(ii))
                    Next
                End If
                '--del.rep.start-------------------------12.10.17
                ' リボンメニューに表示／非表示の状態を反映
                Call setRibbonMenuChkOnOff()
#If 0 Then
                'If entset_point.blnVisiable Then
                '    MainFrm.MM_V_9_1.Checked = True
                'Else
                '    MainFrm.MM_V_9_1.Checked = False
                'End If

                '20121114計測点(ターゲット面)
                 If entset_point1.blnVisiable Then
                    MainFrm.MM_V_9_1.Checked = True
                Else
                    MainFrm.MM_V_9_1.Checked = False
                End If

                '20121114計測点(中心円)
                 If entset_point2.blnVisiable Then
                    MainFrm.MM_V_9_1.Checked = True
                Else
                    MainFrm.MM_V_9_1.Checked = False
                End If

                '20121114計測点(十字線)
                 If entset_point3.blnVisiable Then
                    MainFrm.MM_V_9_1.Checked = True
                Else
                    MainFrm.MM_V_9_1.Checked = False
                End If

                If entset_camera.blnVisiable Then
                    MainFrm.MM_V_9_2.Checked = True
                Else
                    MainFrm.MM_V_9_2.Checked = False
                End If

                If entset_ray.blnVisiable Then
                    MainFrm.MM_V_9_3.Checked = True
                Else
                    MainFrm.MM_V_9_3.Checked = False
                End If

                If entset_label.blnVisiable Then
                    MainFrm.MM_V_9_4.Checked = True
                Else
                    MainFrm.MM_V_9_4.Checked = False
                End If

                If entset_line.blnVisiable And entset_circle.blnVisiable Then
                    MainFrm.MM_V_9_5.Checked = True
                Else
                    MainFrm.MM_V_9_5.Checked = False
                End If
#End If
                '--del.rep.end---------------------------12.10.17

                sys_ScaleInfo.p1 = getDrawPointByMID(sys_ScaleInfo.p1.mid)
                sys_ScaleInfo.p2 = getDrawPointByMID(sys_ScaleInfo.p2.mid)
                YCM_GenScaleLine(sys_ScaleInfo.p1, sys_ScaleInfo.p2)
                'bing_InputIsave = False

                'sys_ScaleInfo.scale = 1.0
                sys_ScaleInfo.p1 = New CLookPoint
                sys_ScaleInfo.p2 = New CLookPoint
                sys_CoordInfo.pOrg = New CLookPoint
                sys_CoordInfo.p1 = New CLookPoint
                sys_CoordInfo.p2 = New CLookPoint
                sys_CoordInfo.strP1XYZ = "+X"
                sys_CoordInfo.strP2XYZ = "+Y"
                sys_CoordInfo.intOxyais = 1
                sys_DrawCircle.dblR = 0.0#
                sys_DrawCircle.iCombox = 0
                NewOrOld = 2
                binfirstopen = True
                suballcanview()
                bintemptest = True
                MainFrm.OpenMeasureData(g_InputIsavePath)
                MainFrm.SetCurrentLabelTo_objFBM()
                ZoomAll()
                '★20121029スケールイメージのグレイ値リセット
                MainFrm.scaleImage = 100
            Else
                MsgBox("計測データ.mdbが存在しません。")
            End If

        End If
    End Sub


    '    Public Sub Command_new()
    '        IOUtil.EndThread()
    '        Dim db_b As Boolean = False
    '        Dim strintputpath As String
    '        g_InputImagePath = ComSel_SelectFolderByShell("画像が入っているフォルダーを指定して下さい。", False)
    '        If g_InputImagePath <> "" Then
    '            strintputpath = g_InputImagePath & "\計測データ.mdb"
    '            Dim ioinputfile As New IO.FileInfo(strintputpath)
    '            If ioinputfile.Exists = True Then
    '                MsgBox("指定されたフォルダには既に計測データが存在します。新規フォルダを選択して下さい")
    '                Exit Sub
    '            Else
    '                Dim strtempstr As String = System.Windows.Forms.Application.StartupPath & "\計測システムフォルダ\Template\計測データ.mdb"
    '                Dim strtempend As String = g_InputImagePath & "\計測データ.mdb"

    '                FileCopy(strtempstr, strtempend) 'フォルダ内にファイルをコピーする
    '                strtempend = g_InputImagePath & "\計測データ_Temp.mdb"
    '                FileCopy(strtempstr, strtempend)
    '                m_strDataBasePath = ""
    '                g_InputIsavePath = ""
    '                Dim clsOPe As New CDBOperate
    '                m_strDataBasePath = strtempend
    '                g_InputIsavePath = g_InputImagePath
    '                If clsOPe.ConnectDB(m_strDataBasePath) = False Then
    '                    'MsgBox("Error!")
    '                    Exit Sub
    '                End If

    '                Dim dblmat(1, 15) As Double
    '                Dim intmat As Integer
    '                intmat = YCM_ReadSystemscalesettingAcsmat(m_strDataBasePath)
    '                YCM_ReadSystemscalesettingAcs(m_strDataBasePath)
    '                YCM_ReadSystemscalesettingAcsvalue(m_strDataBasePath)
    '                If intmat = 1 Then
    '                    sys_CoordInfo.mat = YCM_GetUnitMat()
    '                End If
    '                If intmat = 2 Then
    '                    For ii As Integer = 0 To 15
    '                        sys_CoordInfo.mat(ii) = system_scalesettingmat(0, ii)
    '                    Next
    '                End If
    '                bing_InputIsave = False
    '                If nscalesettingvalue = 0 Then
    '                    sys_ScaleInfo.scale = 1.0
    '                Else
    '                    sys_ScaleInfo.scale = System_scalesettingvalue(0, nscalesetting - 1)
    '                End If

    '                sys_ScaleInfo.p1 = New CLookPoint
    '                sys_ScaleInfo.p2 = New CLookPoint
    '                sys_CoordInfo.pOrg = New CLookPoint
    '                sys_CoordInfo.p1 = New CLookPoint
    '                sys_CoordInfo.p2 = New CLookPoint
    '                sys_CoordInfo.strP1XYZ = "+X"
    '                sys_CoordInfo.strP2XYZ = "+Y"
    '                sys_CoordInfo.intOxyais = 1
    '                sys_Labeling.p1 = New CLookPoint
    '                sys_Labeling.p2 = New CLookPoint
    '                sys_Labeling.p3 = New CLookPoint
    '                sys_DrawCircle.dblR = 0.0#
    '                sys_DrawCircle.iCombox = 0

    '                '要素をなくす
    '                ReDim gDrawRays(0)
    '                ReDim gDrawCamers(0)
    '                ReDim gDrawPoints(0)
    '                ReDim gDrawLabelText(0)
    '                ReDim gDrawUserLines(0)
    '                ReDim gDrawCircleNew(0)
    '                ReDim gDrawCirclepoint(0)
    '                nRays = 0
    '                nCamers = 0
    '                nLookPoints = 0
    '                nLabelText = 0
    '                nUserLines = 0
    '                nCircleNew = 0
    '                ncirclepoint = 0
    '                YCM_ReadZumenInfoFrmAcs(m_strDataBasePath)
    '                YCM_ReadSystemColorAcs(m_strDataSystemPath)
    '                YCM_ReadSystemLineTypes(m_strDataSystemPath)
    '                YCM_ReadEntSetting(m_strDataBasePath)
    '                If nCamers > 0 Then
    '                    For ii = 0 To nCamers - 1
    '                        All_YCM3DVIEW_camer(gDrawCamers(ii))
    '                    Next
    '                End If
    '                If nLookPoints > 0 Then
    '                    For ii = 0 To nLookPoints - 1
    '                        All_YCM3DVIEW_point(gDrawPoints(ii))
    '                    Next
    '                End If
    '                Data_Point.DGV_DV.Rows.Clear()
    '                For i As Integer = 0 To nLookPoints - 1
    '                    Data_Point.DGV_DV.Rows.Add()
    '                    Data_Point.DGV_DV.Rows(i).Cells(0).Value = True
    '                    Data_Point.DGV_DV.Rows(i).Cells(1).Value = gDrawPoints(i).LabelName
    '                    Data_Point.DGV_DV.Rows(i).Cells(2).Value = gDrawPoints(i).Real_x
    '                    Data_Point.DGV_DV.Rows(i).Cells(3).Value = gDrawPoints(i).Real_y
    '                    Data_Point.DGV_DV.Rows(i).Cells(4).Value = gDrawPoints(i).Real_z
    '                    Data_Point.DGV_DV.Rows(i).Cells(5).Value = gDrawPoints(i).mid
    '                Next

    '                For i As Integer = 0 To Data_Point.DGV_DV.RowCount - 1
    '                    Data_Point.DGV_DV.Rows(i).Cells(0).Value = True
    '                Next

    '                point_name = New ArrayList
    '                For i As Integer = 0 To Data_Point.DGV_DV.RowCount - 1
    '                    point_name.Add(Data_Point.DGV_DV.Rows(i).Cells(1).Value)
    '                Next

    '                '--del.rep.start-------------------------12.10.17
    '                ' リボンメニューに表示／非表示の状態を反映
    '                Call setRibbonMenuChkOnOff()
    '#If 0 Then
    '                'If entset_point.blnVisiable Then
    '                '    MainFrm.MM_V_9_1.Checked = True
    '                'Else
    '                '    MainFrm.MM_V_9_1.Checked = False
    '                'End If

    '                If entset_point1.blnVisiable Then'20121114計測点(ターゲット面)
    '                    MainFrm.MM_V_9_1.Checked = True
    '                Else
    '                    MainFrm.MM_V_9_1.Checked = False
    '                End If

    '                If entset_point2.blnVisiable Then'20121114計測点(中心円)
    '                    MainFrm.MM_V_9_1.Checked = True
    '                Else
    '                    MainFrm.MM_V_9_1.Checked = False
    '                End If

    '                If entset_point3.blnVisiable Then'20121114計測点(十字線)
    '                    MainFrm.MM_V_9_1.Checked = True
    '                Else
    '                    MainFrm.MM_V_9_1.Checked = False
    '                End If

    '                If entset_camera.blnVisiable Then
    '                    MainFrm.MM_V_9_2.Checked = True
    '                Else
    '                    MainFrm.MM_V_9_2.Checked = False
    '                End If

    '                If entset_ray.blnVisiable Then
    '                    MainFrm.MM_V_9_3.Checked = True
    '                Else
    '                    MainFrm.MM_V_9_3.Checked = False
    '                End If

    '                If entset_label.blnVisiable Then
    '                    MainFrm.MM_V_9_4.Checked = True
    '                Else
    '                    MainFrm.MM_V_9_4.Checked = False
    '                End If

    '                If entset_line.blnVisiable And entset_circle.blnVisiable Then
    '                    MainFrm.MM_V_9_5.Checked = True
    '                Else
    '                    MainFrm.MM_V_9_5.Checked = False
    '                End If
    '#End If
    '                '--del.rep.end---------------------------12.10.17
    '            End If



    '            NewOrOld = 1
    '            binfirstopen = True
    '            MainFrm.NewMeasureData(g_InputIsavePath)
    '            '★20121029スケールイメージのグレイ値リセット
    '            MainFrm.scaleImage = 100
    '        End If
    '    End Sub

    Public Sub Command_new()
        IOUtil.EndThread()
        Dim db_b As Boolean = False
        Dim strintputpath As String
        g_InputImagePath = m_koji_kanri_path 'ComSel_SelectFolderByShell("画像が入っているフォルダーを指定して下さい。", False)
        If g_InputImagePath <> "" Then
            strintputpath = g_InputImagePath & "\計測データ.mdb"
            Dim ioinputfile As New IO.FileInfo(strintputpath)

            Dim strtempstr As String = strintputpath

            Dim strtempend As String = g_InputImagePath & "\計測データ_Temp.mdb"
            FileCopy(strtempstr, strtempend)
            m_strDataBasePath = ""
            g_InputIsavePath = ""
            Dim clsOPe As New CDBOperate
            m_strDataBasePath = strtempend
            g_InputIsavePath = g_InputImagePath
            If clsOPe.ConnectDB(m_strDataBasePath) = False Then
                'MsgBox("Error!")
                Exit Sub
            End If

            Dim dblmat(1, 15) As Double
            Dim intmat As Integer
            intmat = YCM_ReadSystemscalesettingAcsmat(m_strDataBasePath)
            YCM_ReadSystemscalesettingAcs(m_strDataBasePath)
            YCM_ReadSystemscalesettingAcsvalue(m_strDataBasePath)
            If intmat = 1 Then
                sys_CoordInfo.mat = YCM_GetUnitMat()
            End If
            If intmat = 2 Then
                For ii As Integer = 0 To 15
                    sys_CoordInfo.mat(ii) = system_scalesettingmat(0, ii)
                Next
            End If
            bing_InputIsave = False
            If nscalesettingvalue = 0 Then
                sys_ScaleInfo.scale = 1.0
            Else
                sys_ScaleInfo.scale = System_scalesettingvalue(0, nscalesetting - 1)
            End If

            sys_ScaleInfo.p1 = New CLookPoint
            sys_ScaleInfo.p2 = New CLookPoint
            sys_CoordInfo.pOrg = New CLookPoint
            sys_CoordInfo.p1 = New CLookPoint
            sys_CoordInfo.p2 = New CLookPoint
            sys_CoordInfo.strP1XYZ = "+X"
            sys_CoordInfo.strP2XYZ = "+Y"
            sys_CoordInfo.intOxyais = 1
            sys_Labeling.p1 = New CLookPoint
            sys_Labeling.p2 = New CLookPoint
            sys_Labeling.p3 = New CLookPoint
            sys_DrawCircle.dblR = 0.0#
            sys_DrawCircle.iCombox = 0

            '要素をなくす
            ReDim gDrawRays(0)
            ReDim gDrawCamers(0)
            ReDim gDrawPoints(0)
            ReDim gDrawLabelText(0)
            ReDim gDrawUserLines(0)
            ReDim gDrawCircleNew(0)
            ReDim gDrawCirclepoint(0)
            nRays = 0
            nCamers = 0
            nLookPoints = 0
            nLabelText = 0
            nUserLines = 0
            nCircleNew = 0
            ncirclepoint = 0
            YCM_ReadZumenInfoFrmAcs(m_strDataBasePath)
            YCM_ReadSystemColorAcs(m_strDataSystemPath)
            YCM_ReadSystemLineTypes(m_strDataSystemPath)
            YCM_ReadEntSetting(m_strDataBasePath)
            If nCamers > 0 Then
                For ii = 0 To nCamers - 1
                    All_YCM3DVIEW_camer(gDrawCamers(ii))
                Next
            End If
            If nLookPoints > 0 Then
                For ii = 0 To nLookPoints - 1
                    All_YCM3DVIEW_point(gDrawPoints(ii))
                Next
            End If
            Data_Point.DGV_DV.Rows.Clear()
            For i As Integer = 0 To nLookPoints - 1
                Data_Point.DGV_DV.Rows.Add()
                Data_Point.DGV_DV.Rows(i).Cells(0).Value = True
                Data_Point.DGV_DV.Rows(i).Cells(1).Value = gDrawPoints(i).LabelName
                Data_Point.DGV_DV.Rows(i).Cells(2).Value = gDrawPoints(i).Real_x
                Data_Point.DGV_DV.Rows(i).Cells(3).Value = gDrawPoints(i).Real_y
                Data_Point.DGV_DV.Rows(i).Cells(4).Value = gDrawPoints(i).Real_z
                Data_Point.DGV_DV.Rows(i).Cells(5).Value = gDrawPoints(i).mid
            Next

            For i As Integer = 0 To Data_Point.DGV_DV.RowCount - 1
                Data_Point.DGV_DV.Rows(i).Cells(0).Value = True
            Next

            point_name = New ArrayList
            For i As Integer = 0 To Data_Point.DGV_DV.RowCount - 1
                point_name.Add(Data_Point.DGV_DV.Rows(i).Cells(1).Value)
            Next

            '--del.rep.start-------------------------12.10.17
            ' リボンメニューに表示／非表示の状態を反映
            Call setRibbonMenuChkOnOff()
#If 0 Then
                'If entset_point.blnVisiable Then
                '    MainFrm.MM_V_9_1.Checked = True
                'Else
                '    MainFrm.MM_V_9_1.Checked = False
                'End If

                If entset_point1.blnVisiable Then'20121114計測点(ターゲット面)
                    MainFrm.MM_V_9_1.Checked = True
                Else
                    MainFrm.MM_V_9_1.Checked = False
                End If

                If entset_point2.blnVisiable Then'20121114計測点(中心円)
                    MainFrm.MM_V_9_1.Checked = True
                Else
                    MainFrm.MM_V_9_1.Checked = False
                End If

                If entset_point3.blnVisiable Then'20121114計測点(十字線)
                    MainFrm.MM_V_9_1.Checked = True
                Else
                    MainFrm.MM_V_9_1.Checked = False
                End If

                If entset_camera.blnVisiable Then
                    MainFrm.MM_V_9_2.Checked = True
                Else
                    MainFrm.MM_V_9_2.Checked = False
                End If

                If entset_ray.blnVisiable Then
                    MainFrm.MM_V_9_3.Checked = True
                Else
                    MainFrm.MM_V_9_3.Checked = False
                End If

                If entset_label.blnVisiable Then
                    MainFrm.MM_V_9_4.Checked = True
                Else
                    MainFrm.MM_V_9_4.Checked = False
                End If

                If entset_line.blnVisiable And entset_circle.blnVisiable Then
                    MainFrm.MM_V_9_5.Checked = True
                Else
                    MainFrm.MM_V_9_5.Checked = False
                End If
#End If
            '--del.rep.end---------------------------12.10.17
        End If



        NewOrOld = 1
        binfirstopen = True
        MainFrm.NewMeasureData(g_InputIsavePath)
        '★20121029スケールイメージのグレイ値リセット
        MainFrm.scaleImage = 100

    End Sub
    Public Sub Command_save()
        '        On Error Resume Next
        IOUtil.EndThread()
        If NewOrOld = 0 Then Exit Sub
        Try
            YCM_UpdateMeasurePoint3d(m_strDataBasePath)     '--計測点の保存

            YCM_SaveUserFigureToDB() '--任意図形の保存

            YCM_WriteInterPosInfoTable(m_strDataBasePath)   '--補間点情報の保存

            '20121130 SUURI 追加 Start 　
            YCM_UpdateEntSetting(m_strDataBasePath)
            '20121130 SUURI 追加 ＥＮＤ
            Kill(g_InputIsavePath & "\計測データ.mdb")
            FileCopy(g_InputIsavePath & "\計測データ_Temp.mdb", g_InputIsavePath & "\計測データ.mdb")
            'Kill(g_InputIsavePath & "\計測データ_Temp.mdb")
            'FileCopy(g_InputIsavePath & "\計測データ.mdb", g_InputIsavePath & "\計測データ_Temp.mdb")
            bing_InputIsave = True
            NewOrOld = 2
            MainFrm.objFBM.SaveProjectData()

        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
    End Sub
    Public Sub Command_saveas()
        IOUtil.EndThread()
        If NewOrOld = 0 Then Exit Sub
        Dim str_finis As String
        str_finis = ComSel_SelectFolderByShell("保存先のフォルダーを指定して下さい。", True)
        '2012.12.26=================================================================================================
        '無限ループ（現在開いているフォルダに新規フォルダ作成）⇒「別のフォルダを指定して下さい」などのメッセージを

        '※（新）が（現）+"\"のパスで始まっていればNG

        Dim GInputIsavePath As String = g_InputIsavePath

        If (GInputIsavePath.EndsWith("\")) Then
            If (str_finis.StartsWith(GInputIsavePath)) Then
                'EndsWith：文字列インスタンスの末尾が、指定した文字列（\）と一致するかどうかを判断します。

                'StartsWith：この文字列インスタンスの先頭が、指定した文字列（GInputIsavePath）と一致するかどうかを判断します。

                MsgBox("指定されたフォルダは現在開いているフォルダとなっています。別のフォルダを指定して下さい。", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
                Exit Sub
            Else
            End If
        Else
            Dim PresentPath As String = GInputIsavePath & "\"
            If (str_finis.StartsWith(PresentPath)) Then
                MsgBox("指定されたフォルダは現在開いているフォルダとなっています。別のフォルダを指定して下さい。", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
                Exit Sub
            Else
            End If
        End If
        '2012.12.26=================================================================================================
        If str_finis <> "" Then
            '20121130 SUURI 追加 Start 
            '名前付けて保存時も現在の変更を「計測データ_Temp.mdb」に保存してからフォルダごとコピーする必要がある。


            YCM_UpdateMeasurePoint3d(m_strDataBasePath)     '--計測点の保存

            YCM_SaveUserFigureToDB() '--任意図形の保存

            YCM_WriteInterPosInfoTable(m_strDataBasePath)   '--補間点情報の保存

            YCM_UpdateEntSetting(m_strDataBasePath)
            '20121130 SUURI 追加 ＥＮＤ
            YCM_CopyDir(g_InputIsavePath, str_finis)
            Kill(g_InputIsavePath & "\計測データ_Temp.mdb") '★2012.12.26無限ループエラー?
            Kill(str_finis & "\計測データ.mdb")
            FileCopy(str_finis & "\計測データ_Temp.mdb", str_finis & "\計測データ.mdb")
            Kill(str_finis & "\計測データ_Temp.mdb")
            FileCopy(str_finis & "\計測データ.mdb", str_finis & "\計測データ_Temp.mdb")
            If NewOrOld = 1 Then
                Kill(g_InputIsavePath & "\計測データ.mdb")
            End If
            m_strDataBasePath = str_finis & "\計測データ_Temp.mdb"
            bing_InputIsave = True
            g_InputIsavePath = str_finis
            'gYCM_MainFrame.Title = "YCM　Ver 1.0" & "  " & str_finis
            'YCM_MainFrame.Text = "計測システム" & "  " & str_finis & "  " & "Ver 1.0" '20121129 「Ver1.0追加」

        End If

    End Sub

    Public Sub Command_SetScale()
        Dim scalefrm As New YCM_ScaleSetting
        scalefrm.tmpFlag = True ' SUSANO ADD 20160606
        scalefrm.ShowDialog()
    End Sub
    Public Sub Command_SetChange()
        Dim setfrm As New YCM_Change
        setfrm.ShowDialog()
    End Sub
    Public Sub Command_Circle1p()
        Dim setfrm As New YCM_DrawCricle, iRet As Integer
first_lbl:
        iRet = setfrm.ShowDialog()
        If iRet = DialogResult.OK Then
            Dim cp1 As New CLookPoint
            Dim geoP As New GeoPoint
            Dim r As Double = DrawCircleR
            Dim X_angle As Double = DrawCircleXangle
            Dim Y_angle As Double = DrawCircleYangle

            ''Ｈ25.5.21Yamada試し（●_angleの代わりにVecを置き換える/法線ベクトルの定義）===
            'Dim N As New GeoVector
            'N.setXYZ(0.0, 0.0, 1.0) 'XY平面
            ''Ｈ25.5.21Yamada試し===========================================================

            While True   '--For ii As Integer = 0 To 10000
                If IOUtil.GetPoint(cp1, "点を指示：") = -1 Then Exit Sub
                If (cp1.mid < 0) Then Exit While
                ReDim Preserve gDrawCircleNew(nCircleNew)
                gDrawCircleNew(nCircleNew) = New Ccircle()
                gDrawCircleNew(nCircleNew).org = cp1.toGeopoint
                '--rep.12.3.6                gDrawCircleNew(nCircleNew).r = r
                gDrawCircleNew(nCircleNew).r = r / sys_ScaleInfo.scale
                gDrawCircleNew(nCircleNew).mid = nCircleNew
                gDrawCircleNew(nCircleNew).blnDraw = True
                gDrawCircleNew(nCircleNew).x_angle = X_angle
                gDrawCircleNew(nCircleNew).y_angle = Y_angle
                '↓

                ''Ｈ25.5.21Yamada試し（●_angleの代わりにVecを置き換える/法線ベクトルの定義）===
                'gDrawCircleNew(nCircleNew).Vec = N
                ''Ｈ25.5.21Yamada試し===========================================================

                gDrawCircleNew(nCircleNew).colorCode = entset_circle.color.code
                gDrawCircleNew(nCircleNew).lineTypeCode = entset_circle.linetype.code
                gDrawCircleNew(nCircleNew).layerName = entset_circle.layerName
                nCircleNew = nCircleNew + 1
            End While '--Next
        ElseIf iRet = DialogResult.Abort Then
            GoTo first_lbl
        End If
    End Sub
    Public Sub Command_label()
        'Dim labelfrm As New YCM_LabelSetting
        Dim sekkeifrm As New Sekkei_Keisoku
        'If sys_ScaleInfo.scale = 1.0 Then
        '    MsgBox("スケール設定してください")
        'Else
        sekkeifrm.CmbKeisokuset.Items.Clear()
        Dim strSunpoSet As New List(Of String)
        For i As Integer = 0 To WorksD.SunpoSetL.Count - 1
            strSunpoSet.Add(WorksD.SunpoSetL(i).MeasurementSet)
        Next
        For Each SS As String In strSunpoSet.Distinct.ToList()
            sekkeifrm.CmbKeisokuset.Items.Add(SS)
        Next

        sekkeifrm.ShowDialog()
        'labelfrm.ShowDialog()
        'End If

    End Sub

    '20170224 baluu add start
    Public Sub Command_reconstruct()
        '  MsgBox("No1")
        Dim reconstructfrm As New ReconstructionWindow
        '  MsgBox("No2")
        reconstructfrm.ShowDialog()
        '   MsgBox("No3")
        '  HalconDotNet.HOperatorSet.ResetObjDb(New HalconDotNet.HTuple(512), New HalconDotNet.HTuple(512), New HalconDotNet.HTuple(3))
    End Sub

    Public Sub Command_previewreconstruct()
        Dim dbClass = New CDBOperate()
        If dbClass.ConnectDB("TGLess.mdb") = True Then
            Try
                Dim recordSet = dbClass.CreateRecordset("SELECT TOP 1 * FROM 設定値及び結果 WHERE KoujiPath='" & MainFrm.objFBM.ProjectPath & "' ORDER BY ID DESC")
                If recordSet.RecordCount = 1 Then
                    Dim outputPath As String = recordSet(25).Value
                    If Not IsDBNull(outputPath) Then
                        View_Dilaog.Model3dPath = outputPath
                        View_Dilaog.ReloadModel3dPath = True
                    End If
                End If
            Finally
                dbClass.DisConnectDB()
            End Try
        End If
    End Sub

    Public Sub Command_loadreconstruct()
        Dim reconstructionModelPath = MainFrm.objFBM.ProjectPath & "\Pdata\" & ReconstructionWindow.FileName3D
        If My.Computer.FileSystem.FileExists(reconstructionModelPath) Then
            View_Dilaog.Model3dPath = reconstructionModelPath
            ' View_Dilaog.ReloadModel3dPath = True
            Dim objLoaderFactory = New ObjLoaderFactory()
            Dim ObjLoader = objLoaderFactory.Create()
            modelPoints = ObjLoader.Load(reconstructionModelPath)
            Dim converter = New ObjToMeshConverter()
            meshes = converter.Convert(modelPoints)
            If meshes.Count > 0 Then
                modelPoints = Nothing
                modelPoints = New LoadResult() 'Clearing memory
                isOnlyPoints = False
            End If
        Else
            ' MsgBox("No reconstruction data")
        End If
    End Sub
    '20170224 baluu add end
    Public Sub Command_DrawSetting()
        Dim drawfrm As New YCM_DrawSetting
        drawfrm.ShowDialog()
    End Sub
    Public Sub Command_CSVOut()
        str_OutCSVPath = Filesave_P(MainFrm.SaveFileDialog1)
        If str_OutCSVPath <> "" Then
            Dim fi_csvout As New IO.FileInfo(str_OutCSVPath)
            If fi_csvout.IsReadOnly = True And fi_csvout.Exists = True Then
                Dim intoutcsvpathresult As Integer = MsgBox("このファイルは読み込みのみですので、別の名前を指定してください")
                'Select Case intoutcsvpathresult
                '    Case 1
                '        fi_csvout.IsReadOnly = False
                '    Case 2
                '        Exit Sub
                'End Select
                Exit Sub
            End If
            Creattxt(str_OutCSVPath)
            readtxt(str_OutCSVPath, Data_Point.DGV_DV)
        End If
    End Sub
    Public Sub Command_Rotate()
        View_Dilaog.setWndMode(ControlMode.Rotate)
    End Sub
    Public Sub Command_Move()
        View_Dilaog.setWndMode(ControlMode.Move)
    End Sub

    Public Sub Command_UserLine()  '--DrawLine()
        Dim ccp1 As New CLookPoint
        Dim ccp2 As New CLookPoint
        Dim ccp3 As New CLookPoint
        If IOUtil.GetPoint(ccp1, "始点を指示：") = -1 Then Exit Sub
        If IOUtil.GetPoint(ccp2, "終点を指示：") = -1 Then Exit Sub
        ReDim Preserve gDrawUserLines(nUserLines)
        gDrawUserLines(nUserLines) = New CUserLine()
        Call gDrawUserLines(nUserLines).SetStartPnt(ccp1.x, ccp1.y, ccp1.z)
        Call gDrawUserLines(nUserLines).SetEndPnt(ccp2.x, ccp2.y, ccp2.z)
        '線種と色を設定

        gDrawUserLines(nUserLines).lineTypeCode = entset_line.linetype.code
        gDrawUserLines(nUserLines).colorCode = entset_line.color.code
        nUserLines = nUserLines + 1
        While True   '--For ii As Integer = 0 To 10000
            '--            IOUtil.GetPoint(ccp3, "終点を指示：")
            If IOUtil.GetPoint(ccp3, "終点を指示：") = -1 Then Exit Sub
            If (ccp3.mid < 0) Then Exit While
            ReDim Preserve gDrawUserLines(nUserLines)
            gDrawUserLines(nUserLines) = New CUserLine()
            Call gDrawUserLines(nUserLines).SetStartPnt(ccp2.x, ccp2.y, ccp2.z)
            Call gDrawUserLines(nUserLines).SetEndPnt(ccp3.x, ccp3.y, ccp3.z)
            '線種と色を設定

            gDrawUserLines(nUserLines).lineTypeCode = entset_line.linetype.code
            gDrawUserLines(nUserLines).colorCode = entset_line.color.code
            ccp2 = ccp3
            nUserLines = nUserLines + 1
        End While '--Next
        IOUtil.WriteCommandLine("")
    End Sub
    Public Sub Command_Circle3p()
        Dim cp1 As New CLookPoint
        Dim cp2 As New CLookPoint
        Dim cp3 As New CLookPoint
        Dim r As Double = 0.0
        Dim Org As New GeoPoint
        Dim vec As New GeoVector
        Dim vecx As New GeoVector
        Dim Pi As Double = 3.1415926536
        vecx.setXYZ(1.0, 0.0, 0.0)
        Dim vecz As New GeoVector
        vecz.setXYZ(0, 0, -1.0)
        Dim vecy As New GeoVector
        vecy.setXYZ(0.0, 1.0, 0.0)
        Dim vecxtemp As New GeoVector
        Dim vecytemp As New GeoVector
        vecxtemp.x = vec.x
        vecxtemp.y = vec.y
        vecxtemp.z = vec.z
        If IOUtil.GetPoint(cp1, "第1点を指示：") = -1 Then Exit Sub
        If IOUtil.GetPoint(cp2, "第2点を指示：") = -1 Then Exit Sub
        If IOUtil.GetPoint(cp3, "第3点を指示：") = -1 Then Exit Sub

        CcDrawCircle(cp1.toGeopoint(), cp2.toGeopoint(), cp3.toGeopoint(), Org, r, vec)
        vec.Normalize()
        vecytemp.x = 0.0
        vecytemp.y = vec.y
        vecytemp.z = vec.z
        vecytemp.Normalize()
        Dim vectemp As New GeoVector
        Dim double1 As Double
        Dim double2 As Double
        Dim double3 As Double
        ReDim Preserve gDrawCircleNew(nCircleNew)
        gDrawCircleNew(nCircleNew) = New Ccircle()
        gDrawCircleNew(nCircleNew).org = Org
        gDrawCircleNew(nCircleNew).r = r
        gDrawCircleNew(nCircleNew).mid = nCircleNew
        gDrawCircleNew(nCircleNew).blnDraw = True
        gDrawCircleNew(nCircleNew).Vec = vec

        vectemp = vec.GetOuterProduct(vecytemp)
        double1 = Math.Sqrt(vec.x * vec.x + vec.y * vec.y + vec.z * vec.z)
        double2 = Math.Sqrt(vec.y * vec.y + vec.z * vec.z)
        double3 = Math.Acos(double2 / double1)

        If vec.x > 0 And vec.y > 0 And vec.z > 0 Then
            gDrawCircleNew(nCircleNew).x_angle = vecytemp.GetSmallAngleTo(vecz) / Pi * 180
            gDrawCircleNew(nCircleNew).y_angle = -double3 / Pi * 180
        ElseIf vec.x > 0 And vec.y > 0 And vec.z < 0 Then
            gDrawCircleNew(nCircleNew).x_angle = vecytemp.GetSmallAngleTo(vecz) / Pi * 180
            gDrawCircleNew(nCircleNew).y_angle = -double3 / Pi * 180
        ElseIf vec.x > 0 And vec.y < 0 And vec.z > 0 Then
            gDrawCircleNew(nCircleNew).x_angle = vecytemp.GetSmallAngleTo(vecz) / Pi * 180
            gDrawCircleNew(nCircleNew).y_angle = double3 / Pi * 180
        ElseIf vec.x > 0 And vec.y < 0 And vec.z < 0 Then
            gDrawCircleNew(nCircleNew).x_angle = -vecytemp.GetSmallAngleTo(vecz) / Pi * 180
            gDrawCircleNew(nCircleNew).y_angle = -double3 / Pi * 180
        ElseIf vec.x < 0 And vec.y > 0 And vec.z > 0 Then
            gDrawCircleNew(nCircleNew).x_angle = vecytemp.GetSmallAngleTo(vecz) / Pi * 180
            gDrawCircleNew(nCircleNew).y_angle = double3 / Pi * 180
        ElseIf vec.x < 0 And vec.y > 0 And vec.z < 0 Then
            gDrawCircleNew(nCircleNew).x_angle = -vecytemp.GetSmallAngleTo(vecz) / Pi * 180
            gDrawCircleNew(nCircleNew).y_angle = -double3 / Pi * 180
        ElseIf vec.x < 0 And vec.y < 0 And vec.z > 0 Then
            gDrawCircleNew(nCircleNew).x_angle = -vecytemp.GetSmallAngleTo(vecz) / Pi * 180
            gDrawCircleNew(nCircleNew).y_angle = double3 / Pi * 180
        ElseIf vec.x < 0 And vec.y < 0 And vec.z < 0 Then
            gDrawCircleNew(nCircleNew).x_angle = -vecytemp.GetSmallAngleTo(vecz) / Pi * 180
            gDrawCircleNew(nCircleNew).y_angle = double3 / Pi * 180
        End If

        '20121121線種と色を設定

        gDrawCircleNew(nCircleNew).lineTypeCode = entset_circle.linetype.code
        gDrawCircleNew(nCircleNew).colorCode = entset_circle.color.code
        gDrawCircleNew(nCircleNew).layerName = entset_circle.layerName

        nCircleNew = nCircleNew + 1
    End Sub

    Public Sub Command_XY()

        'Dim geoP As New GeoPoint, geoPX_Tmp, geoPY_Tmp, geoPZ_Tmp As New GeoPoint
        'geHOperatorSet.setXYZ(0, 0, 0)
        'geoPX_Tmp.setXYZ(CSng(0 + 1.0 * 10 / 100 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom)), CSng(0), CSng(0))
        'geoPY_Tmp.setXYZ(CSng(0), CSng(0 + 1 * 10 / 100 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom)), CSng(0))
        'geoPZ_Tmp.setXYZ(CSng(0), CSng(0), CSng(0 + 1 * 10 / 100 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom)))
        'Dim mat_XYZ As GeoMatrix, mat_lib As New MatLib
        'Dim mat_dbl(0 To 15) As Double

        ''座標軸の回転Matrixを作成する
        'mat_XYZ = mat_lib.SetByDoubleMat(sys_CoordInfo.mat)
        'mat_XYZ.Invert()
        'geoP = geHOperatorSet.GetTransformed(mat_XYZ)
        'geoPX_Tmp = geoPX_Tmp.GetTransformed(mat_XYZ)
        'geoPY_Tmp = geoPY_Tmp.GetTransformed(mat_XYZ)
        'geoPZ_Tmp = geoPZ_Tmp.GetTransformed(mat_XYZ)
        xangle = 0
        yangle = 0
        ent_x = CSng(ents_centerC.x)
        ent_y = CSng(ents_centerC.y)
        ent_z = CSng(ents_centerC.z)
    End Sub
    Public Sub Command_XZ()
        xangle = -89.9 '--0
        yangle = 0  '--90
        ent_x = CSng(ents_centerC.x)
        ent_y = CSng(ents_centerC.y)
        ent_z = CSng(ents_centerC.z)
    End Sub
    Public Sub Command_YZ()
        xangle = 0  '--89
        yangle = 90 '--0
        ent_x = CSng(ents_centerC.x)
        ent_y = CSng(ents_centerC.y)
        ent_z = CSng(ents_centerC.z)
    End Sub
    Public Sub Command_camer()
        Dim camer1 As New CCamera
        If IOUtil.GetCamers(camer1, "カメラを指示：") = -1 Then Exit Sub
        Dim oXhudu As Double
        Dim oYhudu As Double

        oXhudu = Math.Asin((c_click_camers.y - c_click_camers.y_eye) / 1)
        xangle = (180 * oXhudu) / Math.PI
        oYhudu = Math.Atan((c_click_camers.x - c_click_camers.x_eye) / (c_click_camers.z - c_click_camers.z_eye))
        yangle = (180 * oYhudu) / Math.PI + 180

        ents_centerC.x = c_click_camers.x_eye
        ents_centerC.y = c_click_camers.y_eye
        ents_centerC.z = c_click_camers.z_eye


    End Sub
    Public Sub Command_area()
        Dim fram_old_x As Double = View_Dilaog.ClientSize.Width
        Dim fram_old_y As Double = View_Dilaog.ClientSize.Height
        Dim fram_new_x As Double = 0.0
        Dim fram_new_y As Double = 0.0

        fram_new_x = Math.Abs(cp_fram_end.x - cp_fram_start.x)
        fram_new_y = Math.Abs(cp_fram_end.y - cp_fram_start.y)

        Dim dblwidth_new_old As Double = fram_new_x / fram_old_x
        Dim dblhetght_new_old As Double = fram_new_y / fram_old_y
        Dim dbltemp_new_old As Double
        If dblwidth_new_old > dblhetght_new_old Then
            dbltemp_new_old = dblhetght_new_old
        Else
            dbltemp_new_old = dblwidth_new_old
        End If
        ents_centerC.x = (gppointstart.x + gppointend.x) / 2
        ents_centerC.y = (gppointstart.y + gppointend.y) / 2
        ents_centerC.z = (gppointstart.z + gppointend.z) / 2
        View_Dilaog.dblwindowLeft = View_Dilaog.dblwindowLeft * dbltemp_new_old
        View_Dilaog.dblwindowRight = View_Dilaog.dblwindowRight * dbltemp_new_old
        View_Dilaog.dblwindowTop = View_Dilaog.dblwindowTop * dbltemp_new_old
        View_Dilaog.dblwindowBottom = View_Dilaog.dblwindowBottom * dbltemp_new_old

    End Sub

    'Public Sub Command_Getpoint()
    '    'Dim labelfrm As New YCM_LabelSetting
    '    IOUtil.GetPoint(Command_cp, "計測点を指示：")
    '    labelSet.Button_1_Click()
    '    Command_wait = True
    '    'AddHandler Completed, AddressOf ThreadCompleted
    '    'RaiseEvent Completed(Nothing, Nothing)
    'End Sub
    'Public Sub ThreadCompleted(ByVal sender As Object, ByVal e As EventArgs)
    '    labelSet.Button_1_Click()
    'End Sub
    Public Sub Command_DrawCircle()
        Dim cp1 As New CLookPoint
        Dim cp2 As New CLookPoint
        Dim cp3 As New CLookPoint
        Dim r As Double = 0.0
        Dim Org As New GeoPoint
        Dim vec As New GeoVector
        Dim vecx As New GeoVector
        Dim Pi As Double = 3.1415926536
        vecx.setXYZ(1.0, 0.0, 0.0)
        Dim vecz As New GeoVector
        vecz.setXYZ(0, 0, -1.0)
        Dim vecy As New GeoVector
        vecy.setXYZ(0.0, 1.0, 0.0)
        Dim vecxtemp As New GeoVector
        Dim vecytemp As New GeoVector
        vecxtemp.x = vec.x
        vecxtemp.y = vec.y
        vecxtemp.z = vec.z
        If IOUtil.GetPoint(cp1, "第1点を指示：") = -1 Then Exit Sub
        If IOUtil.GetPoint(cp2, "第2点を指示：") = -1 Then Exit Sub
        If IOUtil.GetPoint(cp3, "第3点を指示：") = -1 Then Exit Sub

        If CcDrawCircle(cp1.toGeopoint(), cp2.toGeopoint(), cp3.toGeopoint(), Org, r, vec) = -1 Then
            Exit Sub
        End If

        vec.Normalize()
        vecytemp.x = 0.0
        vecytemp.y = vec.y
        vecytemp.z = vec.z
        vecytemp.Normalize()
        Dim vectemp As New GeoVector
        Dim double1 As Double
        Dim double2 As Double
        Dim double3 As Double
        ReDim Preserve gDrawCircleNew(nCircleNew)
        gDrawCircleNew(nCircleNew) = New Ccircle()
        gDrawCircleNew(nCircleNew).org = Org
        gDrawCircleNew(nCircleNew).r = r
        gDrawCircleNew(nCircleNew).mid = nCircleNew
        gDrawCircleNew(nCircleNew).blnDraw = True
        gDrawCircleNew(nCircleNew).Vec = vec


        vectemp = vec.GetOuterProduct(vecytemp)

        double1 = Math.Sqrt(vec.x * vec.x + vec.y * vec.y + vec.z * vec.z)

        double2 = Math.Sqrt(vec.y * vec.y + vec.z * vec.z)

        double3 = Math.Acos(double2 / double1)


        gDrawCircleNew(nCircleNew).x_angle = -vecytemp.GetSmallAngleTo(vecz) / Pi * 180
        gDrawCircleNew(nCircleNew).y_angle = -double3 / Pi * 180


        nCircleNew = nCircleNew + 1

    End Sub

    Public Sub Command_Delete()

        Dim ent As CUserEnt = Nothing
        model_SingleSelct = False
        Dim intRet As Integer
        While True

            intRet = IOUtil.GetUserEnt(ent, "ユーザー図形を指示：")
            If intRet = 0 Then
                Arrent_click.Add(ent.Copy)
            ElseIf intRet = 1 Then
                For jj As Integer = 0 To Arrent_click.Count - 1
                    If Arrent_click(jj).type = "LINE" Then
                        For kk As Integer = 0 To nUserLines - 1
                            If gDrawUserLines(kk).MID = Arrent_click(jj).line.MID Then
                                gDrawUserLines(kk).binDelete = True
                            End If
                        Next
                    Else
                        For kk As Integer = 0 To nCircleNew - 1
                            If gDrawCircleNew(kk).mid = Arrent_click(jj).circle.mid Then
                                gDrawCircleNew(kk).binDelete = True
                            End If
                        Next

                    End If
                Next
                Exit While
            Else
                Exit While
            End If
        End While

        'model_picks_over = False
        model_SingleSelct = True
    End Sub

    '=====================================================================
    ' コマンド：CADの起動

    '=====================================================================
    Public Sub Command_RunCAD()
        ' CAD起動の確認

        If (CheckICADStatus(False) <> 0) Then
            ' CADを起動する

            Call initCAD()
            Exit Sub
        End If
    End Sub

    '=====================================================================
    ' コマンド：CAD図形の選択読み込み
    '=====================================================================
    Public Sub Command_ImportCADElements()
        Dim iRet As Integer
        ' CAD起動の確認

        If (CheckICADStatus(True) <> 0) Then
            'Call ShutdownCAD()
            Exit Sub
        End If
        ' IJCADで表示されているCAD図形を選択して読み込む
        '1.CAD図形フィルタ選択ダイアログの表示
        Dim cadFilterFrm As New YCM_ElmSelectFilter
        cadFilterFrm.CheckBox_Line.Checked = True
        cadFilterFrm.CheckBox_Circle.Checked = True
        cadFilterFrm.CheckBox_ADDMode.Checked = True
        cadFilterFrm.CheckBox_Matching.Checked = False
        cadFilterFrm.ShowDialog()
        If (cadFilterFrm.DialogResult = DialogResult.OK) Then
            '2.IJCADから図形選択

            Call YCM_GetCADEnts( _
                                cadFilterFrm.CheckBox_Line.CheckState, _
                                cadFilterFrm.CheckBox_Circle.CheckState)
            '3.CAD読込み図形データに保持
            '4.CAD図形要素から端点座標値を抽出（座標系設定で使うため）

            '5.計測座標値系に変換（無単位系）

            '　4.1「sys_CoordInfo.mat」の逆マトリックスを求めて考慮
            '　4.2 尺度の考慮：１／「sys_ScaleInfo.scale」

            '       円などは半径なども、文字は文字高

            bCADElmAddMode = cadFilterFrm.CheckBox_ADDMode.CheckState
            '6. ３点マッチングの処理

            If (cadFilterFrm.CheckBox_Matching.Checked = True) Then
                iRet = CADto3DViewMatchng()
                Exit Sub
            End If

            '2.読込んだCAD図形を任意図形データに保持
            If (bCADElmAddMode = False) Then
                '2.1 読込まれているCADデータを削除
                Call deleteUserCADData(False, True)
            End If
            ' 2.2 goCADElmArrsからgDrawUserLines、gDrawCircleNewへ読込んだCAD図形を追加
            Call CADElmArrToUserDrawings()
        End If
    End Sub
    Public Function CADElmArrToUserDrawings()
        'goCADElmArrsからgDrawUserLines、gDrawCircleNewへ読込んだCAD図形を追加
        Dim gpS As New GeoPoint, gpE As New GeoPoint, gpOrg As New GeoPoint
        Dim dRad As Double, dXAng As Double, dYAng As Double
        Dim i As Long, elmType As Integer
        Dim cElm As CElment
        If (goCADElmArrs.Size > 0) Then
            For i = 0 To (goCADElmArrs.Size - 1)
                cElm = goCADElmArrs.at(i)
                elmType = cElm.ElmType
                Select Case (elmType)
                    Case ElementType.ElmLine
                        gpS = cElm.StartPoint
                        gpE = cElm.EndPoint
                        ReDim Preserve gDrawUserLines(nUserLines)
                        gDrawUserLines(nUserLines) = New CUserLine
                        gDrawUserLines(nUserLines).startPnt = gpS.Copy
                        gDrawUserLines(nUserLines).endPnt = gpE.Copy
                        gDrawUserLines(nUserLines).MID = nUserLines
                        gDrawUserLines(nUserLines).elmType = 1
                        gDrawUserLines(nUserLines).lineTypeCode = entset_line_CAD.linetype.code
                        gDrawUserLines(nUserLines).colorCode = entset_line_CAD.color.code
                        nUserLines = nUserLines + 1

                    Case ElementType.ElmCircle
                        gpOrg = cElm.Origin
                        dRad = cElm.Rad
                        dXAng = cElm.XAng
                        dYAng = cElm.YAng
                        ReDim Preserve gDrawCircleNew(nCircleNew)
                        gDrawCircleNew(nCircleNew) = New Ccircle
                        gDrawCircleNew(nCircleNew).org = gpOrg.Copy
                        gDrawCircleNew(nCircleNew).r = dRad
                        gDrawCircleNew(nCircleNew).x_angle = dXAng
                        gDrawCircleNew(nCircleNew).y_angle = dYAng
                        gDrawCircleNew(nCircleNew).mid = nCircleNew
                        gDrawCircleNew(nCircleNew).elmType = 1
                        gDrawCircleNew(nCircleNew).lineTypeCode = entset_line_CAD.linetype.code
                        gDrawCircleNew(nCircleNew).colorCode = entset_line_CAD.color.code
                        nCircleNew = nCircleNew + 1
                    Case Else
                End Select
            Next
        End If
        CADElmArrToUserDrawings = 0
    End Function
    '=====================================================================
    ' 機能：CAD上の３点と計測点の３点を指定して読込んだCAD図形の座標系を設定

    ' 作業変数：

    '   gaCADPos(3) ：CAD図面上の3点座標値
    '   
    '=====================================================================
    Public Function CADto3DViewMatchng() As Integer
        '        Dim gpCADPos(3) As GeoPoint
        Dim iGetPos As Integer
        CADto3DViewMatchng = -1
        '  6.1 CAD上の3点を入力

        iGetPos = YCM_GetCAD3Points(gpCADPos)
        If (iGetPos <> 3) Then Exit Function
        '  6.2 3DViewer上の3点を入力（ダイアログ表示）

        '  6.3 3点マッチングの処理から変換マトリックスを取得

        '  6.4 CAD読込み図形を座標変換
        IOUtil.EndThread()
        IOUtil.LibCommand("setCADcoord")

        CADto3DViewMatchng = 0
    End Function
    Public Sub Command_setCADCoord()
        Dim cadCoordSetFrm As New YCM_CADCoordSetting
        cadCoordSetFrm.Label_CAD1.Text = GeoPointToString(gpCADPos(0))
        cadCoordSetFrm.Label_CAD2.Text = GeoPointToString(gpCADPos(1))
        cadCoordSetFrm.Label_CAD3.Text = GeoPointToString(gpCADPos(2))
        cadCoordSetFrm.ShowDialog()
    End Sub

    Private Function GeoPointToString(ByRef gpPos As GeoPoint) As String
        GeoPointToString = ""
        If (Not gpPos Is Nothing) Then
            'GeoPointToString = "X：[" & Format(gpPos.x, "#.00") & "] " & _
            '                   "Y：[" & Format(gpPos.y, "#.00") & "] " & _
            '                   "Z：[" & Format(gpPos.z, "#.00") & "]"
            GeoPointToString = "(" & Format(gpPos.x, "#.00") & ", " & _
                                    Format(gpPos.y, "#.00") & ", " & _
                                    Format(gpPos.z, "#.00") & ")"
        End If
    End Function

    Public Sub CommandSunpoSetHenkou()
        Dim frmSunpoSetHenkou As New frmNinniTenHenkou
        frmSunpoSetHenkou.objSunpoItem = IOUtil.activeSunpoSet
        frmSunpoSetHenkou.ShowDialog()
    End Sub

    '=====================================================================
    ' コマンド：CAD図形に書き出し

    '=====================================================================
    'Public Sub Command_ExportCADElements()
    '    Dim cadOutFrm As New YCM_CADOutOption
    '    ' CAD起動の確認

    '    If (CheckICADStatus(True) <> 0) Then
    '        'Call ShutdownCAD()
    '        '-            Exit Sub
    '    End If
    '    '-CAD図形書出しオプションのセット    
    '    cadOutFrm.CheckBox_LookPoint.Checked = True
    '    cadOutFrm.CheckBox_Ray.Checked = False
    '    cadOutFrm.CheckBox_LabelText.Checked = True
    '    cadOutFrm.CheckBox_UserElm.Checked = True
    '    cadOutFrm.TextBox_CADFileName.Text = ""
    '    cadOutFrm.TextBox_TextH.Text = "10"
    '    cadOutFrm.ShowDialog()

    '    Dim iOutCoord As Integer  '--出力次元

    '    If (cadOutFrm.DialogResult = DialogResult.OK) Then
    '        Dim bLookPos As Boolean, bRay As Boolean, bLabelText As Boolean, bUserElm As Boolean
    '        bLookPos = cadOutFrm.CheckBox_LookPoint.Checked
    '        bRay = cadOutFrm.CheckBox_Ray.Checked
    '        bLabelText = cadOutFrm.CheckBox_LabelText.Checked()
    '        bUserElm = cadOutFrm.CheckBox_UserElm.Checked
    '        Dim icdDoc As IntelliCAD.Document
    '        Dim dSize As Double, dTextH As Double
    '        dSize = entset_point.screensize '★20121115計測点
    '        '========================================================================================================20121115
    '        'dSize = entset_point1.screensize '★20121114計測点(ターゲット面)
    '        'dSize = entset_point2.screensize '★20121114計測点(中心円)
    '        'dSize = entset_point3.screensize '★20121114計測点(十字線)
    '        '========================================================================================================20121115
    '        dTextH = CDbl(cadOutFrm.TextBox_TextH.Text)
    '        iOutCoord = (cadOutFrm.ComboBox_OutDim.SelectedIndex)

    '        icdDoc = getIcadDocument()
    '        'If (Not icdDoc Is Nothing) Then
    '        '    '--計測点の書き出し

    '        '    If (bLookPos = True) Then Call exportLookPoints(icdDoc, iOutCoord)
    '        '    '--レイの書き出し

    '        '    If (bRay = True) Then Call exportRay(icdDoc, iOutCoord)
    '        '    '--計測ラベルの書き出し

    '        '    If (bLabelText = True) Then Call exportLabelText(icdDoc, dTextH, iOutCoord)
    '        '    '--ユーザ作成図形の書き出し

    '        '    If (bUserElm = True) Then Call exportUserElm(icdDoc, iOutCoord)

    '        '    '--作成したCAD図の保存

    '        '    Call saveFile(cadOutFrm.TextBox_CADFileName.Text)
    '        '    ''Call ShutdownCAD()
    '        'End If
    '        If (Not icdDoc Is Nothing) Then
    '            Dim cad As New IJCAD7Operator(icdDoc)
    '            '--計測点の書き出し

    '            If (bLookPos = True) Then Call exportLookPoints(cad, iOutCoord)
    '            '--レイの書き出し

    '            If (bRay = True) Then Call exportRay(cad, iOutCoord)
    '            '--計測ラベルの書き出し

    '            If (bLabelText = True) Then Call exportLabelText(cad, dTextH, iOutCoord)
    '            '--ユーザ作成図形の書き出し

    '            If (bUserElm = True) Then Call exportUserElm(cad, iOutCoord)

    '            '--作成したCAD図の保存

    '            Call saveFile(cadOutFrm.TextBox_CADFileName.Text)
    '            ''Call ShutdownCAD()
    '        End If
    '    End If
    '    cadOutFrm = Nothing
    '    MsgBox("CAD図形の書き出しを終了しました。")
    'End Sub

    Public Sub DispAutoOffsetCalc()
        Dim frmOffsetCalc As New frmAutoCalcOffset
        frmOffsetCalc.ShowDialog()
    End Sub
    Public Sub CreateTargetPoint3D()
        '     MainFrm.AddUserPoint3D_FourDisp_Extra()
        ' MainFrm.BtnDel3DPoint()
        MainFrm.AddUserPoint3D_FourDisp_Extra()
    End Sub

    Public Sub CreateClickPoint3D()
        MainFrm.AddUserClickPoint3D_FourDisp_extra()

    End Sub
    Public Sub EdgeToPoint3d()
        MainFrm.GetEdgeToPoint3d()

    End Sub
    Public Sub DeletePoint3Ds()
        MainFrm.BtnDel3DPoint()
    End Sub

  

End Module
