﻿Public Class frmBasePointInfo

    Dim bs As BindingSource
    Dim ists As Integer

    Dim lstSokuteiData As New List(Of SunpoSetTable)
    Private Sub frmBasePointInfo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: このコード行はデータを 'DbFBMDataSet.ShootParam' テーブルに読み込みます。必要に応じて移動、または削除をしてください。
        Me.ShootParamTableAdapter.Fill(Me.DbFBMDataSet.ShootParam)
        fillDataGridFromDb()

        Dim fileContents As String
        fileContents = My.Computer.FileSystem.ReadAllText(My.Application.Info.DirectoryPath & "\flgCTari.txt")
        If IsNumeric(fileContents.Split(" ")(0)) Then
            If CInt(fileContents.Split(" ")(0)) = 1 Then
                RadioButton1.Checked = True
                RadioButton2.Checked = False
            Else
                RadioButton1.Checked = False
                RadioButton2.Checked = True
            End If
            TextBox11.Text = fileContents.Split(" ")(1)
        End If
        If IsNumeric(fileContents.Split(" ")(1)) Then
            TextBox11.Text = fileContents.Split(" ")(1)
        End If

        ' FillDataShootParam()

        CarDetectAndResetSettingFromFile()

        '(20150731 Tezuka ADD Start) 車高センサー設定タブの追加
        SensorSetting()
        TxtCSVName.Text = My.Settings.BaseCoordFile     '(20150807 Tezuka ADD)
        Me.TopMost = True

        '***(20151109 Kiryu Add) コメントアウト***
        '(20150810 Tezuka ADD) 測定値補正タブの初期値設定
        'txtDx1.Text = My.Settings.Dx1.ToString
        'txtLx1.Text = My.Settings.Lx1.ToString
        'txtZx1.Text = My.Settings.Zx1.ToString
        'txtDx2.Text = My.Settings.Dx2.ToString
        'txtLx2.Text = My.Settings.Lx2.ToString
        'txtZx2.Text = My.Settings.Zx2.ToString
        'txtDy1.Text = My.Settings.Dy1.ToString
        'txtLy1.Text = My.Settings.Ly1.ToString
        'txtZy1.Text = My.Settings.Zy1.ToString
        'txtDy2.Text = My.Settings.Dy2.ToString
        'txtLy2.Text = My.Settings.Ly2.ToString
        'txtZy2.Text = My.Settings.Zy2.ToString
        '***    END     ***
		txtAlartGosa.Text = My.Settings.AlartGosa.ToString


        '( 20151109 Kiryu Add ) 測定値補正の初期値をcsvから読み込み
        Dim fileContents_Hosei As String
        Try
            fileContents_Hosei = My.Computer.FileSystem.ReadAllText(My.Settings.YCMFolder & "\SueokiHoseiAllways.csv")
        Catch ex As Exception
            Exit Sub
        End Try

        Dim strSplit() As String
        strSplit = fileContents_Hosei.Split(",")
        txtDx1.Text = strSplit(0)
        txtLx1.Text = strSplit(1)
        txtZx1.Text = strSplit(2)
        txtDx2.Text = strSplit(3)
        txtLx2.Text = strSplit(4)
        txtZx2.Text = strSplit(5)

        txtDy1.Text = strSplit(6)
        txtLy1.Text = strSplit(7)
        txtZy1.Text = strSplit(8)
        txtDy2.Text = strSplit(9)
        txtLy2.Text = strSplit(10)
        txtZy2.Text = strSplit(11)
        '**************Kiryu Add End****************************


        Dim clsSunpoData As New SunpoSetTable

        Dim flg As Boolean
        If clsSunpoData.m_dbClass Is Nothing Then
            clsSunpoData.m_dbClass = New CDBOperateOLE
        End If

        flg = clsSunpoData.m_dbClass.Connect(My.Settings.YCMFolder & "\計測システムフォルダ\システム設定.mdb")
        lstSokuteiData = clsSunpoData.GetDataToList()
        Me.DGVKitei.Rows.Clear()
        For Each Sokutei As SunpoSetTable In lstSokuteiData
            Dim objVal1(2) As Object
            objVal1(0) = Sokutei.SunpoName
            '   objVal1(1) = Sokutei.KiteiVal
            objVal1(1) = Sokutei.KiteiMin
            objVal1(2) = Sokutei.KiteiMax
            Me.DGVKitei.Rows.Add(objVal1)
        Next

        clsSunpoData.m_dbClass.DisConnect()

        ReadViewOffsetAddData()

        Me.Refresh()

    End Sub
    Dim lstOffsetAdd As New List(Of CT_Offset_Add_object)

    Private Sub ReadViewOffsetAddData()
        dbClass.Connect(My.Settings.YCMFolder & "\計測システムフォルダ\システム設定.mdb")
        Dim IDR As System.Data.IDataReader
        Dim strSQL As String
        strSQL = "SELECT CT_NO,m_x,m_y,m_z,m_x_add,m_y_add,m_z_add,Info FROM CT_Offset_Add WHERE TypeID = " & CommonTypeID & " ORDER BY Info"
        lstOffsetAdd = New List(Of CT_Offset_Add_object)
        IDR = dbClass.DoSelect(strSQL)
        If Not IDR Is Nothing Then
            While IDR.Read()
                Dim objOffSetAdd As New CT_Offset_Add_object

                objOffSetAdd.CTNO = CInt(IDR.GetValue(0))
                objOffSetAdd.m_x = CDbl(IDR.GetValue(1))
                objOffSetAdd.m_y = CDbl(IDR.GetValue(2))
                objOffSetAdd.m_z = CDbl(IDR.GetValue(3))
                objOffSetAdd.m_x_add = CDbl(IDR.GetValue(4))
                objOffSetAdd.m_y_add = CDbl(IDR.GetValue(5))
                objOffSetAdd.m_z_add = CDbl(IDR.GetValue(6))
                objOffSetAdd.Info = IDR.GetValue(7)
                lstOffsetAdd.Add(objOffSetAdd)
            End While
            IDR.Close()
        End If
        dbClass.DisConnect()
        DGV_SystemSettei.Rows.Clear()
        DGV_OffsetAdd.Rows.Clear()
        For Each objOffSetAdd As CT_Offset_Add_object In lstOffsetAdd
            Dim objSysVal(4) As Object
            objSysVal(0) = objOffSetAdd.Info
            objSysVal(1) = "CT" & objOffSetAdd.CTNO
            objSysVal(2) = objOffSetAdd.m_x
            objSysVal(3) = objOffSetAdd.m_y
            objSysVal(4) = objOffSetAdd.m_z
            DGV_SystemSettei.Rows.Add(objSysVal)
            Dim objoffVal(4) As Object
            objoffVal(0) = objOffSetAdd.Info
            objoffVal(1) = "CT" & objOffSetAdd.CTNO
            objoffVal(2) = objOffSetAdd.m_x_add
            objoffVal(3) = objOffSetAdd.m_y_add
            objoffVal(4) = objOffSetAdd.m_z_add
            DGV_OffsetAdd.Rows.Add(objoffVal)
        Next

    End Sub

    Private Class CT_Offset_Add_object
        Public CTNO As Integer
        Public m_x As Double
        Public m_y As Double
        Public m_z As Double
        Public m_x_add As Double
        Public m_y_add As Double
        Public m_z_add As Double
        Public Info As String
        Public Sub New()
            CTNO = -1
            m_x = 0
            m_y = 0
            m_z = 0
            m_x_add = 0
            m_y_add = 0
            m_z_add = 0
            Info = ""
        End Sub
    End Class
    Private Sub FillDataShootParam()
        Dim i As Integer = 1
        bs = New BindingSource(lstShootParam, String.Empty)
        DGV_ShootParam.DataSource = bs


    End Sub
    Private Sub BtnYomiKomi_Click(sender As Object, e As EventArgs) Handles BtnYomiKomi.Click
        ''ADD by tugi 20150706 Sta

        'Dim ofd As New OpenFileDialog()

        'ofd.FileName = ""
        'ofd.InitialDirectory = "C:\"

        'ofd.Filter = "CSVファイル(*.csv;*.CSV)|*.csv;*.CSV"
        'ofd.FilterIndex = 2

        'ofd.Title = "CSVファイルを選択してください"

        'ofd.RestoreDirectory = True

        'ofd.CheckFileExists = True

        'ofd.CheckPathExists = True

        'If ofd.ShowDialog() = DialogResult.OK Then
        '    If insertIntoTblKijyunPointFromCSV(ofd.FileName) Then
        '        MsgBox("CSVファイルを正常に取り込みました。")
        '        fillDataGridFromDb()
        '    Else
        '        MsgBox("CSVファイルの取り込みに失敗しました。")
        '    End If
        'End If
        ''ADD by tugi 20150706 End
        Dim fileExists As Boolean
        fileExists = My.Computer.FileSystem.FileExists(TxtFileName.Text)
        If fileExists = True Then
            If insertIntoTblKijyunPointFromCSV(TxtFileName.Text) Then
                MsgBox("CSVファイルを正常に取り込みました。")
                fillDataGridFromDb()
            Else
                MsgBox("CSVファイルの取り込みに失敗しました。")
            End If
        Else
            MsgBox("基準点CSVファイルが存在しません。")
        End If
    End Sub

    Private Sub BtnClose_Click(sender As Object, e As EventArgs) Handles BtnClose.Click
        '(20150807 Tezuka Change) 画面更新内容がDBに反映されるように変更
        'Me.ShootParamBindingSource.ResetBindings(False)
        Me.ShootParamBindingSource.EndEdit()
        Me.ShootParamTableAdapter.Update(Me.DbFBMDataSet.ShootParam)

        Dim blnCTari As Boolean
        Dim intSearchKyori As Integer = 0
        If IsNumeric(TextBox11.Text) Then
            intSearchKyori = CInt(TextBox11.Text)
        End If
        If RadioButton1.Checked = True Then
            blnCTari = True
        Else
            blnCTari = False
        End If
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\flgCTari.txt", IIf(blnCTari = True, 1, 0) & " " & intSearchKyori, False)

        '(20150810 Tezuka ADD) 伸縮率を設定に書き込む
        'My.Settings.Dx1 = CDbl(txtDx1.Text)
        'My.Settings.Lx1 = CDbl(txtLx1.Text)
        'My.Settings.Zx1 = CDbl(txtDx2.Text)
        'My.Settings.Dx2 = CDbl(txtLx2.Text)

        '(20151109 Kiryu Add) コメントアウト
        'My.Settings.Dx1 = CDbl(txtDx1.Text)
        'My.Settings.Lx1 = CDbl(txtLx1.Text)
        'My.Settings.Zx1 = CDbl(txtZx1.Text)
        'My.Settings.Dx2 = CDbl(txtDx2.Text)
        'My.Settings.Lx2 = CDbl(txtLx2.Text)
        'My.Settings.Zx2 = CDbl(txtZx2.Text)
        'My.Settings.Dy1 = CDbl(txtDy1.Text)
        'My.Settings.Ly1 = CDbl(txtLy1.Text)
        'My.Settings.Zy1 = CDbl(txtZy1.Text)
        'My.Settings.Dy2 = CDbl(txtDy2.Text)
        'My.Settings.Ly2 = CDbl(txtLy2.Text)
        'My.Settings.Zy2 = CDbl(txtZy2.Text)
        'Kiryu Add End*************
         My.Settings.AlartGosa = CDbl(txtAlartGosa.Text)

        My.Settings.Save()
        'My.Computer.FileSystem.WriteAllText(My.Settings.YCMFolder & "\SueokiHoseiAllways.csv",
        '                                    My.Settings.Dx1 & "," & My.Settings.Lx1 & "," & My.Settings.Zx1 & "," & My.Settings.Dx2, False)
        '***20151109 Kiryu Add コメントアウト***
        'My.Computer.FileSystem.WriteAllText(My.Settings.YCMFolder & "\SueokiHoseiAllways.csv",
        '                                    My.Settings.Dx1 & "," & My.Settings.Lx1 & "," & My.Settings.Zx1 & "," &
        '                                    My.Settings.Dx2 & "," & My.Settings.Lx2 & "," & My.Settings.Zx2 & "," &
        '                                    My.Settings.Dy1 & "," & My.Settings.Ly1 & "," & My.Settings.Zy1 & "," &
        '                                    My.Settings.Dy2 & "," & My.Settings.Ly2 & "," & My.Settings.Zy2, False)
        '***Kiryu Add End***
        '***20151109 Kiryu Add 伸縮率を\YCMForlder\SueokiHoseiAllways.csvに保存***
        My.Computer.FileSystem.WriteAllText(My.Settings.YCMFolder & "\SueokiHoseiAllways.csv",
                                            txtDx1.Text & "," & txtLx1.Text & "," & txtZx1.Text & "," &
                                            txtDx2.Text & "," & txtLx2.Text & "," & txtZx2.Text & "," &
                                            txtDy1.Text & "," & txtLy1.Text & "," & txtZy1.Text & "," &
                                            txtDy2.Text & "," & txtLy2.Text & "," & txtZy2.Text, False)
        '***Kiryu Add End***



        Dim tmpdbClass As New CDBOperateOLE
        tmpdbClass.Connect(My.Settings.YCMFolder & "\計測システムフォルダ\システム設定.mdb")

        For Each sokutedata As SunpoSetTable In lstSokuteiData
            sokutedata.m_dbClass = tmpdbClass
            If sokutedata.UpdateData = False Then

            End If
        Next

        tmpdbClass.DisConnect()

        UpdateOffSetVal()

        CarDetectAndResetSettingToFile()

        Me.Close()
    End Sub
    Private Sub UpdateOffSetVal()
        Dim i As Integer = 0
        For Each objOffSetAdd As CT_Offset_Add_object In lstOffsetAdd
            Try
                objOffSetAdd.m_x_add = CDbl(DGV_OffsetAdd(2, i).Value)
                objOffSetAdd.m_y_add = CDbl(DGV_OffsetAdd(3, i).Value)
                objOffSetAdd.m_z_add = CDbl(DGV_OffsetAdd(4, i).Value)
            Catch ex As Exception

            End Try
           
            i += 1
        Next


        Dim tmpdbClass As New CDBOperateOLE
        tmpdbClass.Connect(My.Settings.YCMFolder & "\計測システムフォルダ\システム設定.mdb")

        Dim strSqlUpdate As String = ""
        For Each objOffSetAdd As CT_Offset_Add_object In lstOffsetAdd
            strSqlUpdate = "UPDATE CT_Offset_ADD SET m_x_add = " & objOffSetAdd.m_x_add &
                                                   ",m_y_add = " & objOffSetAdd.m_y_add &
                                                   ",m_z_add = " & objOffSetAdd.m_z_add &
                                                   " WHERE TypeID = " & CommonTypeID &
                                                   " AND CT_NO = " & objOffSetAdd.CTNO

            tmpdbClass.ExecuteSQL(strSqlUpdate)

            strSqlUpdate = "UPDATE CT_Bunrui SET m_x = " & objOffSetAdd.m_x + objOffSetAdd.m_x_add &
                                                   ",m_y = " & objOffSetAdd.m_y + objOffSetAdd.m_y_add &
                                                   ",m_z = " & objOffSetAdd.m_z + objOffSetAdd.m_z_add &
                                                   " WHERE TypeID = " & CommonTypeID &
                                                   " AND CT_NO = " & objOffSetAdd.CTNO

            tmpdbClass.ExecuteSQL(strSqlUpdate)

        Next
        tmpdbClass.DisConnect()
    End Sub
    Private Sub fillDataGridFromDb()
        Dim fiFeatureImage As New FeatureImage
        Dim sysPath As String = My.Application.Info.DirectoryPath & YCM_SYS_FLDR
        Dim objDatas(3) As Object
        Me.dgKijyunPoint.Rows.Clear()

        For Each aPart As Common3DCodedTarget In fiFeatureImage.lstBasePoint
            objDatas(0) = aPart.PID
            objDatas(1) = aPart.lstP3d.Item(0).X
            objDatas(2) = aPart.lstP3d.Item(0).Y
            objDatas(3) = aPart.lstP3d.Item(0).Z
            Me.dgKijyunPoint.Rows.Add(objDatas)
        Next
    End Sub

    Private Sub DGV_ShootParam_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGV_ShootParam.CellContentClick

    End Sub
    Private Sub CarDetectAndResetSettingToFile()
        Dim strFileString As String = ""
        strFileString = strFileString & ComboBox1.Text & vbNewLine
        strFileString = strFileString & ComboBox2.Text & vbNewLine
        strFileString = strFileString & TextBox8.Text & vbNewLine
        strFileString = strFileString & TextBox28.Text & vbNewLine
        strFileString = strFileString & TextBox10.Text & vbNewLine
        strFileString = strFileString & TextBox9.Text & vbNewLine
        strFileString = strFileString & "0" & vbNewLine
        strFileString = strFileString & "1" & vbNewLine
        strFileString = strFileString & "1行目：車有無判定するカメラ1台目の番号（CarDetectCamera.txt）" & vbNewLine
        strFileString = strFileString & "2行目：車有無判定するカメラ2台目の番号（CarDetectCamera.txt）" & vbNewLine
        strFileString = strFileString & "3行目：車有無判定用反応値の大きい値（CarDetectThreshold.txt）1台目カメラの反応値" & vbNewLine
        strFileString = strFileString & "4行目：車有無判定用反応値の小さい値（CarDetectThreshold.txt）２台目カメラの反応値" & vbNewLine
        strFileString = strFileString & "5行目：車有無判定用　2台目カメラから1台目カメラからの判定待ちフレーム数" & vbNewLine
        strFileString = strFileString & "6行目：退出時リセット待ち時間" & vbNewLine
        strFileString = strFileString & "7行目：ライブ映像の出力 ＯＮ：1、ＯＦＦ：0" & vbNewLine
        strFileString = strFileString & "8行目：センサー常時監視によるリセット機能 ＯＮ：1、ＯＦＦ：0" & vbNewLine
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\Setting\FrameGrabSetting.txt", strFileString, False)

    End Sub
    Private Sub CarDetectAndResetSettingFromFile()
        ComboBox1.Items.Clear()
        ComboBox2.Items.Clear()
        For i As Integer = 1 To 6
            ComboBox1.Items.Add(i)
            ComboBox2.Items.Add(i)
        Next
        ComboBox1.Text = ReadFrameGrabSetting(1)
        ComboBox2.Text = ReadFrameGrabSetting(2)
        TextBox8.Text = ReadFrameGrabSetting(3)
        TextBox28.Text = ReadFrameGrabSetting(4)
        TextBox10.Text = ReadFrameGrabSetting(5)
        TextBox9.Text = ReadFrameGrabSetting(6)

    End Sub
    '(20150731 Tezuka ADD) 車高センサー設定タブの設定
    Private Sub SensorSetting()
        Dim SettingFile As String = My.Application.Info.DirectoryPath & "\Setting\Sensor_Setting.txt"
        FrmMain.SenserSet = New FrmMain.Sensser_setting(SettingFile)
        Dim sMsg As String = ""
        If FrmMain.SenserSet Is Nothing Then
            sMsg = "シリアルポートがオープンされていません。"
            FrmMain.OutMessage(sMsg)
            Exit Sub
        End If

        ists = 0
        TextBox17.Text = FrmMain.SenserSet.PortNumber
        If FrmMain.SenserSet.BaudRate = 9600 Then
            R_Rate_9600.Checked = True
            R_Rate_19200.Checked = False
            R_Rate_38400.Checked = False
        ElseIf FrmMain.SenserSet.BaudRate = 19200 Then
            R_Rate_9600.Checked = False
            R_Rate_19200.Checked = True
            R_Rate_38400.Checked = False
        ElseIf FrmMain.SenserSet.BaudRate = 38400 Then
            R_Rate_9600.Checked = False
            R_Rate_19200.Checked = False
            R_Rate_38400.Checked = True
        End If
        If FrmMain.SenserSet.Parity = "even" Then
            R_Parity_even.Checked = True
            R_Parity_odd.Checked = False
            R_Parity_none.Checked = False
        ElseIf FrmMain.SenserSet.Parity = "Odd" Then
            R_Parity_even.Checked = False
            R_Parity_odd.Checked = True
            R_Parity_none.Checked = False
        ElseIf FrmMain.SenserSet.Parity = "None" Then
            R_Parity_even.Checked = False
            R_Parity_odd.Checked = False
            R_Parity_none.Checked = True
        End If
        TextBox18.Text = FrmMain.SenserSet.DataBits.ToString
        TextBox19.Text = FrmMain.SenserSet.StopBits.ToString
        TextBox20.Text = FrmMain.SenserSet.SlaveAddress.ToString
        TextBox21.Text = FrmMain.SenserSet.kijunHeight.ToString
        TextBox22.Text = FrmMain.SenserSet.SenserPitch.ToString("#0.00")
        TextBox23.Text = FrmMain.SenserSet.PeakAddress.ToString
        TextBox24.Text = FrmMain.SenserSet.ErrcodeAddress.ToString
        TextBox25.Text = FrmMain.SenserSet.ServiceAddress.ToString
        TextBox26.Text = FrmMain.SenserSet.ScanAddress.ToString
        TextBox27.Text = "0"
        TextBox12.Text = FrmMain.SenserSet.MonitorAddress
        TextBox13.Text = Format(FrmMain.SenserSet.ActiveInterval, "0.0")
        TextBox14.Text = Format(FrmMain.SenserSet.CarExistInterval, "0.0")
        TextBox15.Text = FrmMain.SenserSet.IncidentAddress
        TextBox16.Text = FrmMain.SenserSet.IncidentAll

        'センサー常時監視モードの設定有無により表示／非表示を変える
        'If FrmMain.DoveModeFlg <> 1 Then
        '    Label20.Visible = False
        '    Label21.Visible = False
        '    Label22.Visible = False
        '    TextBox12.Visible = False
        '    TextBox13.Visible = False
        '    TextBox14.Visible = False
        'Else
        Label20.Visible = True
        Label21.Visible = True
        Label22.Visible = True
        TextBox12.Visible = True
        TextBox13.Visible = True
        TextBox14.Visible = True
        'End If

        '現在のスキャンタイプを取得し、画面に反映する
        Dim Scan As Integer
        FrmMain.SerialPort_ScanTypeGet(Scan)
        If Scan = 1 Then
            RadioButton4.Checked = True
            RadioButton3.Checked = False
        ElseIf Scan = 2 Then
            RadioButton3.Checked = True
            RadioButton4.Checked = False
        Else
            RadioButton3.Checked = False
            RadioButton4.Checked = False
        End If

        ists = 1
    End Sub

    '(20150731 Tezuka ADD) 稼働時間の取得
    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs)
        Dim sMsg As String = ""
        If FrmMain.SenserSet Is Nothing Then
            sMsg = "シリアルポートをオープンされていません。"
            FrmMain.OutMessage(sMsg)
            Exit Sub
        End If

        Dim Serv As String = ""
        FrmMain.SerialPort_ServiceTimeGet(Serv)

        TextBox27.Text = Serv

    End Sub

    '(20150731 Tezuka ADD) 各種設定値の変更
    'Private Sub TextBox17_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox17.TextChanged
    '    If ists <> 0 Then
    '        If TextBox17.Text <> FrmMain.SenserSet.PortNumber Then
    '            FrmMain.SenserSet.PortNumber = TextBox17.Text

    '            'シリアルポートを閉じて開きなおす
    '            FrmMain.SerialPort_SetFileWrite()
    '            FrmMain.SerialPort_Close()
    '            FrmMain.SerialPort_Open()
    '            'If Trim(TextBox17.Text) <> "" Then
    '            'Dim PortName As String = "COM" & FrmMain.SenserSet.PortNumber
    '            'Dim Parity As Integer
    '            'If FrmMain.SenserSet.Parity = "None" Then
    '            '    Parity = 0
    '            'ElseIf FrmMain.SenserSet.Parity = "Odd" Then
    '            '    Parity = 1
    '            'Else
    '            '    Parity = 2
    '            'End If
    '            'Dim sts As Integer = Ez_PortOpen(PortName, FrmMain.SenserSet.BaudRate, FrmMain.SenserSet.DataBits, FrmMain.SenserSet.StopBits, Parity)
    '            'End If
    '        End If
    '    Else
    '        ists = 1
    '    End If
    'End Sub

    Private Sub R_Rate_9600_CheckedChanged(sender As System.Object, e As System.EventArgs)
        Dim iFlg As Integer = 0
        If R_Rate_9600.Checked = True Then
            FrmMain.SenserSet.BaudRate = 9600
            If ists = 1 Then
                ists = 0
                iFlg = 1
            End If
            R_Rate_19200.Checked = False
            R_Rate_38400.Checked = False
            If iFlg = 1 Then
                ists = 1
            End If
        End If

        If ists = 1 Then
            'シリアルポートを閉じて開きなおす
            FrmMain.SerialPort_SetFileWrite()
            FrmMain.SerialPort_Close()
            FrmMain.SerialPort_Open()
        End If

    End Sub

    Private Sub R_Rate_19200_CheckedChanged(sender As System.Object, e As System.EventArgs)
        Dim iFlg As Integer = 0
        If R_Rate_19200.Checked = True Then
            FrmMain.SenserSet.BaudRate = 19200
            If ists = 1 Then
                ists = 0
                iFlg = 1
            End If
            R_Rate_9600.Checked = False
            R_Rate_38400.Checked = False
            If iFlg = 1 Then
                ists = 1
            End If
        End If

        If ists = 1 Then
            'シリアルポートを閉じて開きなおす
            FrmMain.SerialPort_SetFileWrite()
            FrmMain.SerialPort_Close()
            FrmMain.SerialPort_Open()
        End If

    End Sub

    Private Sub R_Rate_38400_CheckedChanged(sender As System.Object, e As System.EventArgs)
        Dim iFlg As Integer = 0
        If R_Rate_38400.Checked = True Then
            FrmMain.SenserSet.BaudRate = 38400
            If ists = 1 Then
                ists = 0
                iFlg = 1
            End If
            R_Rate_19200.Checked = False
            R_Rate_9600.Checked = False
            If iFlg = 1 Then
                ists = 1
            End If
        End If

        If ists = 1 Then
            'シリアルポートを閉じて開きなおす
            FrmMain.SerialPort_SetFileWrite()
            FrmMain.SerialPort_Close()
            FrmMain.SerialPort_Open()
        End If

    End Sub

    Private Sub R_Parity_even_CheckedChanged(sender As System.Object, e As System.EventArgs)
        Dim iFlg As Integer = 0
        If R_Parity_even.Checked = True Then
            FrmMain.SenserSet.Parity = "even"
            If ists = 1 Then
                ists = 0
                iFlg = 1
            End If
            R_Parity_odd.Checked = False
            R_Parity_none.Checked = False
            If iFlg = 1 Then
                ists = 1
            End If
        End If

        If ists = 1 Then
            'シリアルポートを閉じて開きなおす
            FrmMain.SerialPort_SetFileWrite()
            FrmMain.SerialPort_Close()
            FrmMain.SerialPort_Open()
        End If

    End Sub

    Private Sub R_Parity_odd_CheckedChanged(sender As System.Object, e As System.EventArgs)
        Dim iFlg As Integer = 0
        If R_Parity_odd.Checked = True Then
            FrmMain.SenserSet.Parity = "Odd"
            If ists = 1 Then
                ists = 0
                iFlg = 1
            End If
            R_Parity_even.Checked = False
            R_Parity_none.Checked = False
            If iFlg = 1 Then
                ists = 1
            End If
        End If

        If ists = 1 Then
            'シリアルポートを閉じて開きなおす
            FrmMain.SerialPort_SetFileWrite()
            FrmMain.SerialPort_Close()
            FrmMain.SerialPort_Open()
        End If

    End Sub

    Private Sub R_Parity_none_CheckedChanged(sender As System.Object, e As System.EventArgs)
        Dim iFlg As Integer = 0
        If R_Parity_none.Checked = True Then
            FrmMain.SenserSet.Parity = "None"
            If ists = 1 Then
                ists = 0
                iFlg = 1
            End If
            R_Parity_odd.Checked = False
            R_Parity_even.Checked = False
            If iFlg = 1 Then
                ists = 1
            End If
        End If

        If ists = 1 Then
            'シリアルポートを閉じて開きなおす
            FrmMain.SerialPort_SetFileWrite()
            FrmMain.SerialPort_Close()
            FrmMain.SerialPort_Open()
        End If

    End Sub

    Private Sub TextBox18_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox18.TextChanged
        FrmMain.SenserSet.DataBits = CInt(TextBox18.Text)

        If ists = 1 Then
            'シリアルポートを閉じて開きなおす
            FrmMain.SerialPort_SetFileWrite()
            FrmMain.SerialPort_Close()
            FrmMain.SerialPort_Open()
        End If

    End Sub

    Private Sub TextBox19_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox19.TextChanged
        FrmMain.SenserSet.StopBits = CInt(TextBox19.Text)

        If ists = 1 Then
            'シリアルポートを閉じて開きなおす
            FrmMain.SerialPort_SetFileWrite()
            FrmMain.SerialPort_Close()
            FrmMain.SerialPort_Open()
        End If

    End Sub

    Private Sub TextBox20_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox20.TextChanged
        FrmMain.SenserSet.SlaveAddress = CInt(TextBox20.Text)
    End Sub

    Private Sub TextBox21_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox21.TextChanged
        FrmMain.SenserSet.kijunHeight = CInt(TextBox21.Text)
    End Sub

    Private Sub TextBox22_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox22.TextChanged
        FrmMain.SenserSet.SenserPitch = CDbl(TextBox22.Text)
    End Sub

    Private Sub TextBox23_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox23.TextChanged
        FrmMain.SenserSet.PeakAddress = CInt(TextBox23.Text)
    End Sub

    Private Sub TextBox24_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox24.TextChanged
        FrmMain.SenserSet.ErrcodeAddress = CInt(TextBox24.Text)
    End Sub

    Private Sub TextBox25_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox25.TextChanged
        FrmMain.SenserSet.ServiceAddress = CInt(TextBox25.Text)
    End Sub

    Private Sub TextBox26_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox26.TextChanged
        FrmMain.SenserSet.ScanAddress = CInt(TextBox26.Text)
    End Sub

    Private Sub frmBasePointInfo_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        FrmMain.SerialPort_SetFileWrite()
    End Sub

    Private Sub TextBox17_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = System.Windows.Forms.Keys.Enter Then
            If TextBox17.Text <> FrmMain.SenserSet.PortNumber Then
                FrmMain.SenserSet.PortNumber = TextBox17.Text

                'シリアルポートを閉じて開きなおす
                FrmMain.SerialPort_SetFileWrite()
                FrmMain.SerialPort_Close()
                FrmMain.SerialPort_Open()
            End If
        End If
        Me.TopMost = True
    End Sub

    Private Sub BtnCSVout_Click(sender As System.Object, e As System.EventArgs) Handles BtnCSVout.Click
        Dim strHeadertxt = "標準点番号,X,Y,Z"
        SaveCSV_fromDataGridView_InFname(TxtCSVName.Text, dgKijyunPoint, strHeadertxt)

    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button_Fname.Click
        'FolderBrowserDialogクラスのインスタンスを作成
        Dim fbd As New SaveFileDialog

        '上部に表示する説明テキストを指定する
        fbd.Title = "保存先のフォルダ、ファイル名を指定してください。"
        'ルートフォルダを指定する
        'デフォルトでDesktop
        Dim Fname As String = My.Settings.BaseCoordFile
        Dim DefFolder As String = Fname.Substring(0, InStrRev(Fname, "\"c) - 1)
        Dim DefFile As String = Fname.Substring(InStrRev(Fname, "\"c))
        If System.IO.Directory.Exists(DefFolder) = True Then
            fbd.InitialDirectory = DefFolder
        Else
            fbd.InitialDirectory = My.Settings.KoujiRootFolder
        End If
        fbd.FileName = DefFile
        fbd.Filter = "csvファイル(*.csv)|*.csv"
        fbd.RestoreDirectory = True
        fbd.OverwritePrompt = True
        fbd.CheckPathExists = True

        'ダイアログを表示する
        If fbd.ShowDialog(Me) = DialogResult.OK Then
            '選択されたフォルダを表示する
            Me.TxtCSVName.Text = fbd.FileName
        End If

        '保存ファイル名をセーブする
        My.Settings.BaseCoordFile = Me.TxtCSVName.Text
    End Sub

    '(20150807 Tezuka ADD) ファイル名指定でCSV出力する
    Private Sub SaveCSV_fromDataGridView_InFname(ByVal Fname As String, ByVal DGV As DataGridView, Optional ByVal strHeaderTxt As String = "")
        Dim strSavePath As String = Fname

        Dim strFileBuf As String = ""
        'DataGridView1.Item(0, 0).Value
        With DGV
            Dim i As Integer
            Dim j As Integer
            If strHeaderTxt <> "" Then
                strFileBuf = strHeaderTxt & vbNewLine
            End If
            For i = 0 To .RowCount - 2
                For j = 0 To .ColumnCount - 1
                    If j = .ColumnCount - 1 Then
                        strFileBuf = strFileBuf & .Item(j, i).Value
                    Else
                        strFileBuf = strFileBuf & .Item(j, i).Value & ","
                    End If
                Next
                strFileBuf = strFileBuf & vbNewLine
            Next
        End With
        Try
            My.Computer.FileSystem.WriteAllText(strSavePath, strFileBuf, False)
        Catch ex As Exception
            FrmMain.OutMessage("CSVファイル出力に失敗しました。ファイル名：" & strSavePath)
            Exit Sub
        End Try

        FrmMain.OutMessage("CSVファイルを出力しました。ファイル名：" & strSavePath)
    End Sub

    Private Sub BtnRun_Click(sender As Object, e As EventArgs) Handles BtnRun.Click
        Try
            Dim BP_HoseiID As Integer = CInt(TxtHosei.Text)

            Dim BP_TekiseiID1 As Integer = CInt(TextBox1.Text)

            Dim BP_TekiseiID2 As Integer = CInt(TextBox2.Text)

            Dim BP_TekiseiID3 As Integer = CInt(TextBox3.Text)

            Dim BP_H_T_1 As Double = CDbl(TextBox6.Text)

            Dim BP_H_T_2 As Double = CDbl(TextBox5.Text)

            Dim BP_H_T_3 As Double = CDbl(TextBox4.Text)

            Dim lstBasePoint3d As New List(Of measurepoint3dTable)

            Dim dReaderKijyun As System.Data.IDataReader
            If Not dbClass Is Nothing Then
                AccessDisConnect()
            End If
            ConnectDbSystemSetting(My.Settings.YCMFolder & "\計測システムフォルダ\")
            dReaderKijyun = dbClass.DoSelect("SELECT * FROM " & YCM_SYS_MDB_KIJYUNTBL & " order by CTID")
            While dReaderKijyun.Read()
                Dim strLabel As String
                Dim xx, yy, zz As Integer
                Dim intKijyunFlag As Integer
                intKijyunFlag = CInt(dReaderKijyun.GetValue(5))
                If intKijyunFlag = 1 Then
                    strLabel = dReaderKijyun.GetValue(1).ToString
                    If strLabel.IndexOf("_") = -1 Then
                        Dim strSys2 As String
                        strSys2 = Join(Split(strLabel, "_"), "")
                        xx = CDbl(dReaderKijyun.GetValue(2))
                        yy = CDbl(dReaderKijyun.GetValue(3))
                        zz = CDbl(dReaderKijyun.GetValue(4))
                        Dim objBasePoint As New measurepoint3dTable
                        objBasePoint.systemlabel = strLabel
                        objBasePoint.X = xx
                        objBasePoint.Y = yy
                        objBasePoint.Z = zz
                        lstBasePoint3d.Add(objBasePoint)
                    End If
                End If
            End While
            dReaderKijyun.Close()

            Dim BP_Hosei As Point3D = Nothing

            Dim BP_Tekisei1 As Point3D = Nothing

            Dim BP_Tekisei2 As Point3D = Nothing

            Dim BP_Tekisei3 As Point3D = Nothing
            Dim ResBP As New measurepoint3dTable

            For Each BP As measurepoint3dTable In lstBasePoint3d
                If BP.systemlabel = "CT" & BP_HoseiID Then
                    BP_Hosei = New Point3D(CDbl(BP.X), CDbl(BP.Y), CDbl(BP.Z))
                    ResBP = BP
                End If
                If BP.systemlabel = "CT" & BP_TekiseiID1 Then
                    BP_Tekisei1 = New Point3D(BP.X, BP.Y, BP.Z)
                End If
                If BP.systemlabel = "CT" & BP_TekiseiID2 Then
                    BP_Tekisei2 = New Point3D(BP.X, BP.Y, BP.Z)
                End If
                If BP.systemlabel = "CT" & BP_TekiseiID3 Then
                    BP_Tekisei3 = New Point3D(BP.X, BP.Y, BP.Z)
                End If
            Next

            Dim kouten As New List(Of Point3D)
            Dim targetTen As New List(Of Point3D)
            kouten = cci(BP_Tekisei1, BP_H_T_1, BP_Tekisei2, BP_H_T_2)
            'Dim kouten10 As New List(Of Point3D)
            'kouten10 = cci1(BP_Tekisei1, BP_H_T_1, BP_Tekisei2, BP_H_T_2)

            Call getPoints(kouten, BP_Hosei, targetTen)

            Dim kouten1 As New List(Of Point3D)
            kouten1 = cci(BP_Tekisei2, BP_H_T_2, BP_Tekisei3, BP_H_T_3)
            Call getPoints(kouten1, BP_Hosei, targetTen)

            Dim kouten2 As New List(Of Point3D)
            kouten2 = cci(BP_Tekisei3, BP_H_T_3, BP_Tekisei1, BP_H_T_1)
            Call getPoints(kouten2, BP_Hosei, targetTen)

            Dim cnt As Integer = targetTen.Count
            Dim x As Double = 0
            Dim y As Double = 0
            Dim z As Double = 0
            For Each pp As Point3D In targetTen
                x += pp.X
                y += pp.Y
                z += pp.Z
            Next

            'Dim BP_Vec1 As New Point3D(BP_Hosei.X - BP_Tekisei1.X, BP_Hosei.Y - BP_Tekisei1.Y, BP_Hosei.Z - BP_Tekisei1.Z)


            'Dim BP_Vec2 As New Point3D(BP_Hosei.X - BP_Tekisei2.X, BP_Hosei.Y - BP_Tekisei2.Y, BP_Hosei.Z - BP_Tekisei2.Z)

            'Dim BP_Vec3 As New Point3D(BP_Hosei.X - BP_Tekisei3.X, BP_Hosei.Y - BP_Tekisei3.Y, BP_Hosei.Z - BP_Tekisei3.Z)
            'Dim V1Len As Double
            'Dim V2Len As Double
            'Dim V3Len As Double
            'BP_Vec1.GetDisttoOtherPose(New Point3D(0, 0, 0), V1Len)
            'BP_Vec2.GetDisttoOtherPose(New Point3D(0, 0, 0), V2Len)
            'BP_Vec3.GetDisttoOtherPose(New Point3D(0, 0, 0), V3Len)

            'Dim BP_Vec11 As New Point3D(BP_Vec1.X / V1Len * BP_H_T_1 * (-1), BP_Vec1.Y / V1Len * BP_H_T_1 * (-1), BP_Vec1.Z / V1Len * BP_H_T_1 * (-1))

            'Dim BP_Vec22 As New Point3D(BP_Vec2.X / V2Len * BP_H_T_2 * (-1), BP_Vec2.Y / V2Len * BP_H_T_2 * (-1), BP_Vec2.Z / V2Len * BP_H_T_2 * (-1))

            'Dim BP_Vec33 As New Point3D(BP_Vec3.X / V3Len * BP_H_T_3 * (-1), BP_Vec3.Y / V3Len * BP_H_T_3 * (-1), BP_Vec3.Z / V3Len * BP_H_T_3 * (-1))

            'BP_Vec11 = BP_Vec11.SubPoint3d(BP_Vec1)
            'BP_Vec22 = BP_Vec22.SubPoint3d(BP_Vec2)
            'BP_Vec33 = BP_Vec33.SubPoint3d(BP_Vec3)

            'Dim BpHos As New Point3D
            'BpHos = New Point3D(x / cnt, y / cnt, z / cnt)
            Dim HoseiKekka As Point3D

            HoseiKekka = New Point3D(x / cnt, y / cnt, z / cnt)
            ResBP.X = HoseiKekka.X
            ResBP.Y = HoseiKekka.Y
            Dim strFe() As String = {"X座標", "Y座標"}
            Dim strFV() As String = {ResBP.X, ResBP.Y}

            Dim strSql As String = "UPDATE " & YCM_SYS_MDB_KIJYUNTBL & " SET X = '" & ResBP.X & "' , Y='" & ResBP.Y & "' WHERE ラベル = " & ResBP.systemlabel & ""
            If dbClass.DoUpdate(strFe, YCM_SYS_MDB_KIJYUNTBL, strFV, "ラベル = '" & ResBP.systemlabel & "'") = 1 Then
            Else
                MsgBox("基準点補正結果の登録に失敗しました。")
            End If
            AccessDisConnect()
            fillDataGridFromDb()
        Catch ex As Exception
            MsgBox("入力値が不正です。")
        End Try


    End Sub
    '円と直線の交点
    Private Function cli(ByVal a As Double, ByVal b As Double, ByVal c As Double, ByVal x1 As Double, ByVal y1 As Double, ByVal r As Double, ByVal z As Double) As List(Of Point3D)
        Dim l As Double = a * a + b * b
        Dim k As Double = a * x1 + b * y1 + c
        Dim d As Double = l * r * r - k * k
        Dim kouten As List(Of Point3D)
        kouten = New List(Of Point3D)
        If d > 0 Then
            Dim ds As Double = Math.Sqrt(d)
            Dim apl As Double = a / l
            Dim bpl As Double = b / l
            Dim xc As Double = x1 - apl * k
            Dim yc As Double = y1 - bpl * k
            Dim xd As Double = bpl * ds
            Dim yd As Double = apl * ds

            kouten.Add(New Point3D(xc - xd, yc + yd, z))
            kouten.Add(New Point3D(xc + xd, yc - yd, z))

        ElseIf d = 0 Then
            kouten.Add(New Point3D(x1 - a * k / l, y1 - b * k / l, z))
        Else

        End If
        Return kouten
    End Function
    Private Function cci1(ByVal ten1 As Point3D, ByVal r1 As Double, ByVal ten2 As Point3D, ByVal r2 As Double) As List(Of Point3D)
        Dim x1 As Double = ten1.X
        Dim x2 As Double = ten2.X
        Dim y1 As Double = ten1.Y
        Dim y2 As Double = ten2.Y
        Dim z As Double = ten1.Z
        Dim a As Double = x1 - x2
        Dim b As Double = y1 - y2
        Dim c As Double = 0.5 * ((r1 - r2) * (r1 + r2) - a * (x1 + x2) - b * (y1 + y2))
        Return cli(a, b, c, x1, y1, r1, Z)
    End Function
    '円と円の交点
    Private Function cci(ByVal ten1 As Point3D, ByVal r1 As Double, ByVal ten2 As Point3D, ByVal r2 As Double) As List(Of Point3D)

        Dim kouten As List(Of Point3D)
        kouten = New List(Of Point3D)

        Dim x1 As Double = ten1.X
        Dim x2 As Double = ten2.X
        Dim y1 As Double = ten1.Y
        Dim y2 As Double = ten2.Y
        Dim z As Double = 0 'ten1.Z
        '中心点間の長さ：
        Dim d As Double = Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1))
        Dim a As Double = Math.Atan2(y2 - y1, x2 - x1)
        Dim b As Double
        If Math.Abs(r1 - r2) < d And d < r1 + r2 Then
            '円は交わっている
            '交点は2つ
            b = Math.Acos((d * d + r1 * r1 - r2 * r2) / (2 * d * r1))
            'P1 = (r1*cos(a+θ), r1*sin(a+θ))
            'P2 = (r1*cos(a-θ), r1*sin(a-θ)) 
            kouten.Add(New Point3D(x1 + r1 * Math.Cos(a + b), y1 + r1 * Math.Sin(a + b), z))
            kouten.Add(New Point3D(x1 + r1 * Math.Cos(a - b), y1 + r1 * Math.Sin(a - b), z))

        ElseIf (Math.Abs(r1 - r2) = d Or r1 + r2 = d) And d <> 0 Then
        '円は外接か内接し一致せず
        '交点は1つ
            kouten.Add(New Point3D(x1 + r1 * Math.Cos(a), y1 + r1 * Math.Sin(a), z))
        End If

            Return kouten
    End Function
    '点と点距離
    Private Function pointToPointDistance(ByRef ten1 As Point3D, ByRef ten2 As Point3D) As Double
        Dim kyori As Double
        Dim d As Double
        d = (ten1.X - ten2.X) * (ten1.X - ten2.X) + (ten1.Y - ten2.Y) * (ten1.Y - ten2.Y) + (ten1.Z - ten2.Z) * (ten1.Z - ten2.Z)
        kyori = Math.Sqrt(d)
        Return kyori
    End Function
    '一番近い点を取得
    Private Sub getPoints(ByRef pnts As List(Of Point3D), ByRef BP_Hosei As Point3D, ByRef targets As List(Of Point3D))
        Dim d1 As Double
        Dim d2 As Double
        If pnts.Count = 2 Then
            d1 = pointToPointDistance(pnts.Item(0), BP_Hosei)
            d2 = pointToPointDistance(pnts.Item(1), BP_Hosei)
            If d1 < d2 Then
                targets.Add(pnts.Item(0))
            Else
                targets.Add(pnts.Item(1))
            End If
        ElseIf pnts.Count = 1 Then
            targets.Add(pnts.Item(0))
        Else
        End If
    End Sub


    Private Sub RadioButton3_CheckedChanged(sender As System.Object, e As System.EventArgs)
        Dim Scan As Integer

        If ists = 1 Then
            If RadioButton3.Checked = True Then
                RadioButton4.Checked = False
                Scan = 2
                FrmMain.SerialPort_ScanTypeChange(Scan)
            End If
        End If
    End Sub

    Private Sub RadioButton4_CheckedChanged(sender As System.Object, e As System.EventArgs)
        Dim Scan As Integer

        If ists = 1 Then
            If RadioButton4.Checked = True Then
                RadioButton3.Checked = False
                Scan = 1
                FrmMain.SerialPort_ScanTypeChange(Scan)
            End If
        End If
    End Sub

    Private Sub TextBox12_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox12.TextChanged
        FrmMain.SenserSet.MonitorAddress = CInt(TextBox12.Text)
    End Sub

    Private Sub TextBox13_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox13.TextChanged
        FrmMain.SenserSet.ActiveInterval = CDbl(TextBox13.Text)
    End Sub

    Private Sub TextBox14_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox14.TextChanged
        FrmMain.SenserSet.CarExistInterval = CDbl(TextBox14.Text)
    End Sub

    Private Sub TextBox15_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox15.TextChanged
        FrmMain.SenserSet.IncidentAddress = CInt(TextBox15.Text)
    End Sub

    Private Sub TextBox16_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox16.TextChanged
        FrmMain.SenserSet.IncidentAll = CInt(TextBox16.Text)
        FrmMain.Label1.ForeColor = Color.Blue
        FrmMain.Label1.Text = "測定前"
        FrmMain.TextBox1.Text = "( --- / " & FrmMain.SenserSet.IncidentAll.ToString & " )"
    End Sub

    Private Sub TextBox17_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox17.TextChanged
        FrmMain.SenserSet.PortNumber = TextBox17.Text
    End Sub

    Private Sub DGVKitei_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVKitei.CellContentClick
     
    End Sub

    Private Sub DGVKitei_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DGVKitei.CellValueChanged
        If e.ColumnIndex > 0 Then

            Dim changedVal As Double
            Try
                changedVal = CDbl(DGVKitei(e.ColumnIndex, e.RowIndex).Value)
            Catch ex As Exception
                Exit Sub

            End Try
            'If e.ColumnIndex = 1 Then
            '    lstSokuteiData.Item(e.RowIndex).KiteiVal = changedVal
            'End If

            If e.ColumnIndex = 1 Then
                lstSokuteiData.Item(e.RowIndex).KiteiMin = changedVal
            End If

            If e.ColumnIndex = 2 Then
                lstSokuteiData.Item(e.RowIndex).KiteiMax = changedVal
            End If

        End If
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Dim ofd As New OpenFileDialog()

        ofd.FileName = ""
        ofd.InitialDirectory = "C:\"

        ofd.Filter = "CSVファイル(*.csv;*.CSV)|*.csv;*.CSV"
        ofd.FilterIndex = 2

        ofd.Title = "CSVファイルを選択してください"

        ofd.RestoreDirectory = True

        ofd.CheckFileExists = True

        ofd.CheckPathExists = True

        If ofd.ShowDialog() = DialogResult.OK Then
            TxtFileName.Text = ofd.FileName
        End If
    End Sub

End Class
