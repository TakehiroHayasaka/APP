﻿Public Class frmMeasureResult

    Public Property BaseNinsikiHiritu As Double
    Public Property MeasureNinsikiHiritu As Double
    'susano add 20151103 start
    Public Property KeikokuSaryou As Double
    Public Property SokteiLogs As String
    'susano add 20151105 start
    Dim strKoujiFolder As String

    Public Sub LoadInfo(ByVal strNewKoujiFolder As String)
        '     Dim strNewKoujiFolder As String = Me.Tag
        Dim clsmeasurepoint3d As New measurepoint3dTable
        Dim lstmeasurepoint3d As New List(Of measurepoint3dTable)
        Dim lstBasePoint3d As New List(Of measurepoint3dTable)
        SokteiLogs = ""
        Dim iOK As Integer
        BaseNinsikiHiritu = 0.0
        MeasureNinsikiHiritu = 0.0
        strKoujiFolder = strNewKoujiFolder

        Dim flg As Boolean
        If clsmeasurepoint3d.m_dbClass Is Nothing Then
            clsmeasurepoint3d.m_dbClass = New CDBOperateOLE
        End If

        flg = clsmeasurepoint3d.m_dbClass.Connect(strNewKoujiFolder & "\計測データ.mdb")
        lstmeasurepoint3d = clsmeasurepoint3d.GetDataToList()
        clsmeasurepoint3d.m_dbClass.DisConnect()

        If lstmeasurepoint3d Is Nothing Then
            Me.Close()

            Exit Sub
        End If
        '計測データ.mdbよりスケール値を取り出す
        Dim dReaderScale As System.Data.IDataReader
        Dim dblScale As Double
        If ConnectDbForALLMDB(strNewKoujiFolder, "\計測データ.mdb") Then
            dReaderScale = dbClass.DoSelect("SELECT * FROM scalevalue")
            While dReaderScale.Read()
                dblScale = CDbl(dReaderScale.GetValue(0))
            End While
            dReaderScale.Close()
        End If
        dblScale = 1.0
        '計測点データにスケール値をかける
        Dim lstmeasurepoint3d_wrk As New List(Of measurepoint3dTable)
        For Each Sokutei As measurepoint3dTable In lstmeasurepoint3d
            Dim x, y, z As Double
            x = CDbl(Sokutei.X) * dblScale
            y = CDbl(Sokutei.Y) * dblScale
            z = CDbl(Sokutei.Z) * dblScale
            Sokutei.X = x.ToString
            Sokutei.Y = y.ToString
            Sokutei.Z = z.ToString
            lstmeasurepoint3d_wrk.Add(Sokutei)
        Next
        lstmeasurepoint3d.Clear()
        lstmeasurepoint3d = lstmeasurepoint3d_wrk

        Dim sysPath As String = My.Settings.YCMFolder & YCM_SYS_FLDR
        Dim dReaderKijyun As System.Data.IDataReader
        Dim dReaderBunrui As System.Data.IDataReader
        Dim lstCTBunrui As New List(Of measurepoint3dTable)
        'DataGridView1.Rows.Clear()
        'DataGridView2.Rows.Clear()

        If ConnectDbSystemSetting(sysPath) Then
            'dReaderBunrui = dbClass.DoSelect("SELECT CT_NO,Info FROM CT_Bunrui WHERE TypeID=28 ORDER BY ID")
            'While dReaderBunrui.Read()
            '    Dim objCTBunrui As New measurepoint3dTable
            '    objCTBunrui.ID = dReaderBunrui.GetValue(0)
            '    objCTBunrui.systemlabel = dReaderBunrui.GetValue(1)
            '    lstCTBunrui.Add(objCTBunrui)
            'End While
            'dReaderBunrui.Close()
            'For Each objBunrui As measurepoint3dTable In lstCTBunrui

            '    Dim blnNinsikiOK As Boolean = False

            '    For Each Sokutei As measurepoint3dTable In lstmeasurepoint3d
            '        Dim sysl As String
            '        Dim x, y, z As Double
            '        sysl = Sokutei.systemlabel
            '        If sysl = "CT" & objBunrui.ID Then


            '            Dim strSys1 As String
            '            strSys1 = Join(Split(sysl, "_"), "")

            '            x = CDbl(Sokutei.X)
            '            y = CDbl(Sokutei.Y)
            '            z = CDbl(Sokutei.Z)
            '            Dim objVal1(5) As Object
            '            objVal1(0) = sysl
            '            objVal1(1) = "○"
            '            objVal1(2) = objBunrui.systemlabel
            '            objVal1(3) = Math.Round(x, 1)
            '            objVal1(4) = Math.Round(y, 1)
            '            objVal1(5) = Math.Round(z, 1)
            '            Me.DataGridView2.Rows.Add(objVal1)
            '        End If
            '    Next
            'Next

            '(20150806 Tezuka ADD) CT***_**を除く修正
            'Susano add 20151109 start
            Dim strSql As String = "SELECT A1.*, B.Info " & _
                                    "FROM (select A.* , IIF(InStr(A.ラベル,""_"")>0,right(Left(A.ラベル,InStr(A.ラベル,""_"")-1),len(Left(A.ラベル,InStr(A.ラベル,""_"")-1))-2),right(A.ラベル,len(A.ラベル)-2)) as CT_NO from  " & YCM_SYS_MDB_KIJYUNTBL & " AS A) AS A1, " & _
                                    " CT_Bunrui AS B " & _
                                    " WHERE B.TypeID = 28 And A1.CT_NO = CStr(B.CT_NO) order by B.Info,A1.CTID"
            'Susano add 20151109 End
            'dReaderKijyun = dbClass.DoSelect("SELECT * FROM " & YCM_SYS_MDB_KIJYUNTBL & " order by CTID")
            dReaderKijyun = dbClass.DoSelect(strSql)
            While dReaderKijyun.Read()
                Dim strLabel As String
                Dim xx, yy, zz As Integer
                Dim intKijyunFlag As Integer
                intKijyunFlag = CInt(dReaderKijyun.GetValue(5))
                If intKijyunFlag = 1 Then
                    strLabel = dReaderKijyun.GetValue(1).ToString
                    If strLabel.IndexOf("_") = -1 Then          '(20150806 Tezuka ADD)
                        Dim strSys2 As String
                        strSys2 = Join(Split(strLabel, "_"), "")
                        xx = CDbl(dReaderKijyun.GetValue(2))
                        yy = CDbl(dReaderKijyun.GetValue(3))
                        zz = CDbl(dReaderKijyun.GetValue(4))
                        Dim objBasePoint As New measurepoint3dTable
                        'Susano add 20151109 start
                        objBasePoint.sokutenmei = dReaderKijyun.GetValue(7).ToString
                        'Susano add 20151109 End
                        objBasePoint.systemlabel = strLabel
                        objBasePoint.X = xx
                        objBasePoint.Y = yy
                        objBasePoint.Z = zz
                        lstBasePoint3d.Add(objBasePoint)
                        'If strSys1 = strSys2 Then
                        '    Dim objVal2(7) As Object
                        '    objVal2(0) = strLabel
                        '    objVal2(1) = "○"
                        '    objVal2(2) = Math.Round(xx, 1)
                        '    objVal2(3) = Math.Round(yy, 1)
                        '    objVal2(4) = Math.Round(zz, 1)
                        '    objVal2(5) = Math.Round(xx - X, 1)
                        '    objVal2(6) = Math.Round(yy - Y, 1)
                        '    objVal2(7) = Math.Round(zz - Z, 1)
                        '    Me.DataGridView1.Rows.Add(objVal2)
                        '    isExistFlag = True
                        'End If
                    End If                                   '(20150806 Tezuka ADD)
                End If
            End While
            dReaderKijyun.Close()

            dReaderBunrui = dbClass.DoSelect("SELECT CT_NO,Info FROM CT_Bunrui WHERE TypeID=28 ORDER BY Info")
            While dReaderBunrui.Read()
                Dim objCTBunrui As New measurepoint3dTable
                objCTBunrui.ID = dReaderBunrui.GetValue(0)
                objCTBunrui.systemlabel = dReaderBunrui.GetValue(1)
                lstCTBunrui.Add(objCTBunrui)
            End While
            dReaderBunrui.Close()
            'For Each SystemBasePoint As measurepoint3dTable In lstBasePoint3d
            '    Dim objCTBunrui As New measurepoint3dTable
            '    objCTBunrui.ID = SystemBasePoint.ID
            '    objCTBunrui.systemlabel = SystemBasePoint.systemlabel
            '    lstCTBunrui.Add(objCTBunrui)
            'Next
            iOK = 0
            Dim iM As Integer = 0
            '(20150810 Tezuka ADD) 伸縮率テキストボックスに値を設定する
            TextBox1.Text = My.Settings.MeasPointMove_x.ToString
            TextBox4.Text = My.Settings.MeasPointBase_x.ToString
            TextBox2.Text = My.Settings.MeasPointMove_y.ToString
            TextBox3.Text = My.Settings.MeasPointBase_y.ToString
            TxtCSVName.Text = My.Settings.BasePointFile
            txtSokuFile.Text = My.Settings.MeasurePointFile

            For Each objBunrui As measurepoint3dTable In lstCTBunrui

                Dim blnNinsikiOK As Boolean = False

                For Each Sokutei As measurepoint3dTable In lstmeasurepoint3d
                    Dim sysl As String
                    Dim x, y, z As Double
                    sysl = Sokutei.systemlabel

                    If sysl = "CT" & objBunrui.ID Or sysl = objBunrui.systemlabel Then

                        Dim strSys1 As String
                        strSys1 = Join(Split(sysl, "_"), "")

                        x = CDbl(Sokutei.X)
                        y = CDbl(Sokutei.Y)
                        z = CDbl(Sokutei.Z)
                        Dim objVal1(5) As Object
                        'susano rep 20151109 start
                        objVal1(1) = sysl
                        objVal1(2) = "○"
                        objVal1(0) = objBunrui.systemlabel
                        'susano rep 20151109 End
                        objVal1(3) = Math.Round(x, 1)
                        objVal1(4) = Math.Round(y, 1)
                        objVal1(5) = Math.Round(z, 1)
                        Me.DataGridView2.Rows.Add(objVal1)
                        blnNinsikiOK = True
                        If CInt(objBunrui.ID) > 400 Then
                            iM += 1
                        End If
                        iOK += 1
                    End If
                Next
                If blnNinsikiOK = False Then
                    Dim objVal1(5) As Object
                    'susano rep 20151109 start
                    objVal1(1) = "CT" & objBunrui.ID
                    objVal1(2) = "×"
                    objVal1(0) = objBunrui.systemlabel
                    'susano rep 20151109 End
                    objVal1(3) = "－"
                    objVal1(4) = "－"
                    objVal1(5) = "－"
                    Me.DataGridView2.Rows.Add(objVal1)
                End If
            Next
            'MeasureNinsikiHiritu = iOK / lstCTBunrui.Count        '(20150806 Tezuka ADD) 測点認識率
            MeasureNinsikiHiritu = iM 'SUSANO ADD 20151029

            '座標変換
            Dim MotoBasePoint As New Point3D
            Dim HikakuBasePoint As New Point3D
            For Each objBasePoint As measurepoint3dTable In lstBasePoint3d
                For Each Sokutei As measurepoint3dTable In lstmeasurepoint3d
                    If objBasePoint.systemlabel = Sokutei.systemlabel Then
                        MotoBasePoint.ConcatToMe(New Point3D(CDbl(objBasePoint.X), CDbl(objBasePoint.Y), CDbl(objBasePoint.Z)))
                        HikakuBasePoint.ConcatToMe(New Point3D(CDbl(Sokutei.X), CDbl(Sokutei.Y), CDbl(Sokutei.Z)))
                    End If
                Next
            Next

            Dim objHommat3d As Object = Nothing
            Try
                Op.VectorToHomMat3d("rigid", HikakuBasePoint.X, HikakuBasePoint.Y, HikakuBasePoint.Z,
                                      MotoBasePoint.X, MotoBasePoint.Y, MotoBasePoint.Z, objHommat3d)
                Op.AffineTransPoint3D(objHommat3d, HikakuBasePoint.X, HikakuBasePoint.Y, HikakuBasePoint.Z, HikakuBasePoint.X, HikakuBasePoint.Y, HikakuBasePoint.Z)


            Catch ex As Exception
                Dim a As Integer = 11
            End Try
            Dim ii As Integer = 0
            For Each objBasePoint As measurepoint3dTable In lstBasePoint3d
                For Each Sokutei As measurepoint3dTable In lstmeasurepoint3d
                    If objBasePoint.systemlabel = Sokutei.systemlabel Then
                        Sokutei.X = Tuple.TupleSelect(HikakuBasePoint.X, ii)
                        Sokutei.Y = Tuple.TupleSelect(HikakuBasePoint.Y, ii)
                        Sokutei.Z = Tuple.TupleSelect(HikakuBasePoint.Z, ii)
                        ii += 1
                    End If
                Next
            Next
            iOK = 0
            'Susano add 20151109 start
            Dim nowdate As String
            nowdate = Now.ToString("yyyy/MM/dd HH:mm:ss")
            'Susano add 20151109 end
            For Each objBasePoint As measurepoint3dTable In lstBasePoint3d
                Dim objVal2(8) As Object
                'Susano rep 20151109 start
                Dim x1, y1, z1 As Double
                objVal2(0) = objBasePoint.sokutenmei
                objVal2(1) = objBasePoint.systemlabel
                Dim blnNinshikiOK As Boolean = False
                For Each Sokutei As measurepoint3dTable In lstmeasurepoint3d
                    If objBasePoint.systemlabel = Sokutei.systemlabel Then
                        blnNinshikiOK = True
                        x1 = Math.Round(CDbl(objBasePoint.X) - CDbl(Sokutei.X), 1)
                        y1 = Math.Round(CDbl(objBasePoint.Y) - CDbl(Sokutei.Y), 1)
                        z1 = Math.Round(CDbl(objBasePoint.Z) - CDbl(Sokutei.Z), 1)
                        objVal2(6) = x1
                        objVal2(7) = y1
                        objVal2(8) = z1
                        Exit For
                    End If
                Next
                If blnNinshikiOK = True Then
                    objVal2(2) = "○"
                    iOK += 1
                    If KeikokuSaryou < Math.Sqrt(x1 * x1 + y1 * y1 + z1 * z1) Then
                        SokteiLogs = SokteiLogs & nowdate & "  測点名" & objBasePoint.sokutenmei & "  認識○ (誤差量 距離：" & Math.Round(Math.Sqrt(x1 * x1 + y1 * y1 + z1 * z1), 1) & _
                                    " ΔX：" & x1 & " ΔY：" & y1 & " ΔZ：" & z1 & " )" & vbCrLf
                    End If
                Else
                    objVal2(2) = "×"
                    SokteiLogs = SokteiLogs & nowdate & "  測点名" & objBasePoint.sokutenmei & "  認識×" & vbCrLf
                End If

                objVal2(3) = Math.Round(CDbl(objBasePoint.X), 1)
                objVal2(4) = Math.Round(CDbl(objBasePoint.Y), 1)
                objVal2(5) = Math.Round(CDbl(objBasePoint.Z), 1)
                'Susano rep 20151109 End
                Me.DataGridView1.Rows.Add(objVal2)

            Next
            BaseNinsikiHiritu = iOK / lstBasePoint3d.Count        '(20150806 Tezuka ADD) 基準点認識率

            dbClass.DisConnect()
        End If

        Me.Refresh()
    End Sub
    Private Sub frmMeasureResult_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadInfo(Me.Tag)
    End Sub

    Private Sub BtnClose_Click(sender As Object, e As EventArgs) Handles BtnClose.Click
        My.Settings.MeasPointMove_x = CDbl(TextBox1.Text)
        My.Settings.MeasPointBase_x = CDbl(TextBox4.Text)
        My.Settings.MeasPointMove_y = CDbl(TextBox2.Text)
        My.Settings.MeasPointBase_y = CDbl(TextBox3.Text)
        My.Settings.BasePointFile = TxtCSVName.Text
        My.Settings.MeasurePointFile = txtSokuFile.Text
        My.Settings.Save()
        Me.Close()
    End Sub

    Private Sub BtnCSVOut_Click(sender As Object, e As EventArgs) Handles BtnCSVOut.Click
        Dim strHeadertxt = "測点名,内部番号,認識,X,Y,Z,ΔX,ΔY,ΔZ"
        'SaveCSV_fromDataGridView(DataGridView1, strHeadertxt)
        SaveCSV_fromDataGridView_InFname(TxtCSVName.Text, DataGridView1, strHeadertxt)

    End Sub

    Private Sub SaveCSV_fromDataGridView(ByVal DGV As DataGridView, Optional ByVal strHeaderTxt As String = "")
        Dim sfd1 As New System.Windows.Forms.SaveFileDialog
        sfd1.Filter = "CSV ファイル | *.csv"
        If sfd1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dim strSavePath As String = sfd1.FileName

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
            My.Computer.FileSystem.WriteAllText(strSavePath, strFileBuf, False)
        End If
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

    Private Sub BtnSokuCsv_Click(sender As Object, e As EventArgs) Handles BtnSokuCsv.Click
        Dim strHeadertxt = "測点名,内部番号,認識,箇所,X,Y,Z"
        'SaveCSV_fromDataGridView(DataGridView2, strHeadertxt)
        SaveCSV_fromDataGridView_InFname(txtSokuFile.Text, DataGridView2, strHeadertxt)

    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        'FolderBrowserDialogクラスのインスタンスを作成
        Dim fbd As New SaveFileDialog

        '上部に表示する説明テキストを指定する
        fbd.Title = "保存先のフォルダ、ファイル名を指定してください。"
        'ルートフォルダを指定する
        'デフォルトでDesktop
        Dim Fname As String = My.Settings.BasePointFile
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
        My.Settings.BasePointFile = Me.TxtCSVName.Text
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        'FolderBrowserDialogクラスのインスタンスを作成
        Dim fbd As New SaveFileDialog

        '上部に表示する説明テキストを指定する
        fbd.Title = "保存先のフォルダ、ファイル名を指定してください。"
        'ルートフォルダを指定する
        'デフォルトでDesktop
        Dim Fname As String = My.Settings.MeasurePointFile
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
            Me.txtSokuFile.Text = fbd.FileName
        End If

        '保存ファイル名をセーブする
        My.Settings.MeasurePointFile = Me.txtSokuFile.Text
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button_Hosei.Click
        Dim dblCalcX, dblCalcY As Double
        Dim clsmeasurepoint3d As New measurepoint3dTable
        Dim lstmeasurepoint3d As New List(Of measurepoint3dTable)

        If (CDbl(TextBox4.Text) = 0.0) Or (CDbl(TextBox3.Text) = 0.0) Then
            MsgBox("X方向伸縮率、または、Y方向伸縮率の分母の数値には、0以外の数値を入力してください。", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        '伸縮率の値を設定に書き込む
        My.Settings.MeasPointMove_x = CDbl(TextBox1.Text)
        My.Settings.MeasPointBase_x = CDbl(TextBox4.Text)
        My.Settings.MeasPointMove_y = CDbl(TextBox2.Text)
        My.Settings.MeasPointBase_y = CDbl(TextBox3.Text)
        My.Settings.Save()

        '伸縮率の計算
        dblCalcX = 1.0 + (My.Settings.MeasPointMove_x / My.Settings.MeasPointBase_x)
        dblCalcY = 1.0 + (My.Settings.MeasPointMove_y / My.Settings.MeasPointBase_y)

        '測定点をDBより読み込む
        Dim flg As Boolean
        If clsmeasurepoint3d.m_dbClass Is Nothing Then
            clsmeasurepoint3d.m_dbClass = New CDBOperateOLE
        End If

        flg = clsmeasurepoint3d.m_dbClass.Connect(strKoujiFolder & "\計測データ.mdb")
        lstmeasurepoint3d = clsmeasurepoint3d.GetDataToList()
        If lstmeasurepoint3d Is Nothing Then
            Me.Close()

            Exit Sub
        End If

        Dim sysPath As String = My.Settings.YCMFolder & YCM_SYS_FLDR
        If ConnectDbSystemSetting(sysPath) Then

            Dim dReaderKijyun As System.Data.IDataReader
            Dim dReaderBunrui As System.Data.IDataReader
            Dim lstCTBunrui As New List(Of measurepoint3dTable)
            Dim lstBasePoint3d As New List(Of measurepoint3dTable)

            '基準座標テーブルより基準点を読み込む
            dReaderKijyun = dbClass.DoSelect("SELECT * FROM " & YCM_SYS_MDB_KIJYUNTBL & " order by CTID")
            While dReaderKijyun.Read()
                Dim strLabel As String
                Dim xx, yy, zz As Integer
                Dim intKijyunFlag As Integer
                intKijyunFlag = CInt(dReaderKijyun.GetValue(5))
                If intKijyunFlag = 1 Then
                    strLabel = dReaderKijyun.GetValue(1).ToString
                    If strLabel.IndexOf("_") = -1 Then          '(20150806 Tezuka ADD)
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
                    End If                                   '(20150806 Tezuka ADD)
                End If
            End While

            'CT_Bunruiテーブルより設定されているターゲットを読み込む
            'susano rep 20151109 start
            dReaderBunrui = dbClass.DoSelect("SELECT CT_NO,Info FROM CT_Bunrui WHERE TypeID=28 ORDER BY Info")
            'susano rep 20151109 end
            While dReaderBunrui.Read()
                Dim objCTBunrui As New measurepoint3dTable
                objCTBunrui.ID = dReaderBunrui.GetValue(0)
                objCTBunrui.systemlabel = dReaderBunrui.GetValue(1)
                lstCTBunrui.Add(objCTBunrui)
            End While
            dReaderBunrui.Close()
            For Each SystemBasePoint As measurepoint3dTable In lstBasePoint3d
                Dim objCTBunrui As New measurepoint3dTable
                objCTBunrui.ID = SystemBasePoint.ID
                objCTBunrui.systemlabel = SystemBasePoint.systemlabel
                lstCTBunrui.Add(objCTBunrui)
            Next

            '測定点表を作成する
            Me.DataGridView2.Rows.Clear()
            For Each objBunrui As measurepoint3dTable In lstCTBunrui

                Dim blnNinsikiOK As Boolean = False
                For Each Sokutei As measurepoint3dTable In lstmeasurepoint3d
                    Dim sysl As String
                    Dim x, y, z As Double
                    sysl = Sokutei.systemlabel
                    If sysl = "CT" & objBunrui.ID Or sysl = objBunrui.systemlabel Then
                        Dim strSys1 As String
                        strSys1 = Join(Split(sysl, "_"), "")

                        x = CDbl(Sokutei.X) * dblCalcX
                        y = CDbl(Sokutei.Y) * dblCalcY
                        z = CDbl(Sokutei.Z)
                        Dim objVal1(5) As Object
                        'susano rep 20151109 start
                        objVal1(1) = sysl
                        objVal1(2) = "○"
                        objVal1(0) = objBunrui.systemlabel
                        'susano rep 20151109 end 
                        objVal1(3) = Math.Round(x, 1)
                        objVal1(4) = Math.Round(y, 1)
                        objVal1(5) = Math.Round(z, 1)
                        Me.DataGridView2.Rows.Add(objVal1)
                        blnNinsikiOK = True
                    End If
                Next
                If blnNinsikiOK = False Then
                    Dim objVal1(5) As Object
                    'susano rep 20151109 start
                    objVal1(1) = "CT" & objBunrui.ID
                    objVal1(2) = "×"
                    objVal1(0) = objBunrui.systemlabel
                    'susano rep 20151109 End
                    objVal1(3) = "－"
                    objVal1(4) = "－"
                    objVal1(5) = "－"
                    Me.DataGridView2.Rows.Add(objVal1)
                End If
            Next
        End If

        clsmeasurepoint3d.m_dbClass.DisConnect() '20150902 add
      
        FrmMain.KaisekiAndView()


    End Sub


End Class