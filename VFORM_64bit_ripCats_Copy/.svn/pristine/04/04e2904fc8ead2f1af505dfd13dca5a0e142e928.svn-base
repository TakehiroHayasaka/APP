Imports System.Runtime.InteropServices

'20150204 ADD ByY Yamada 
Public Class Form3
    Dim hwinid1 As Object
    Dim lstCamPose As List(Of CameraPose)
    Dim intW As Integer
    Dim intH As Integer
    Dim ho_Image As HALCONXLib.HUntypedObjectX
    Dim lst_SelectImage As List(Of HALCONXLib.HUntypedObjectX)
    Dim selectImageFolder As String = ""

    Dim preImage As String '表示するイメージファイル名
    Dim preID As Integer '表示するイメージのID
    Dim strNotOutIDs() As String '出力しないイメージのID群
    Dim strOutImages() As String '出力するイメージのファイル名群
    Dim strOutIDs() As String '出力するイメージのID群

    '20150205 Rep By Yamada Sta ------- 
    'tmptarget = New TargetDetect
    Dim preTarget As New TargetDetect
    '20150205 Rep By Yamada Sta -------

    '20150203 ADD BY Yamada Sta--------YCMのCommon.vb 
    Public Class MessageEventArgs
        Inherits EventArgs
        Dim m_ImageIndex As Integer
        Public Property ImageIndex() As Integer
            Get
                Return m_ImageIndex
            End Get
            Set(ByVal value As Integer)
                m_ImageIndex = value
            End Set
        End Property

        Dim m_ImageCount As Integer
        Public Property ImageCount() As Integer
            Get
                Return m_ImageCount
            End Get
            Set(ByVal value As Integer)
                m_ImageCount = value
            End Set
        End Property

        Dim m_MessageText As String
        Public Property MessageText() As String
            Get
                Return m_MessageText
            End Get
            Set(ByVal value As String)
                m_MessageText = value
            End Set
        End Property
    End Class

    '20150216 ADD By Yamada Sta ----------------------------------------------------------------
    '[機能]Form3を表示する度にコールするとエラーが発生するため、Form1を表示する初回のみコールする
    Public Sub Form3_Initialize()
        hwinid1 = AxHWindowXCtrl1.HalconWindow.HalconID
    End Sub

    Private Sub Form3_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        DataGridView1.Rows.Clear()
    End Sub
    '20150216 ADD By Yamada End ----------------------------------------------------------------


    '20150212 ADD BY Yamada Sta---------------------
    'Private Sub Form3_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
    '    MsgBox("ed")
    '    Me.Visible = False
    'End Sub

    'Private Sub Form3_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    '    MsgBox("ing")
    '    Me.Visible = False
    'End Sub
    '20150212 ADD BY Yamada End---------------------

    Private Sub Form3_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        '20150219 Del By Yamada Form3_Initialize()へ（Form1からコール）
        hwinid1 = AxHWindowXCtrl1.HalconWindow.HalconID '画像選択の再開時のエラーをバトスーリさんに確認
        intH = 2748
        intW = 3840
        Op.SetPart(hwinid1, 0, 0, intH, intW)

        TextBox1.Text = strKojiFolder & "\" & Date.Now.ToString("yyyyMMddHHmmss")

        On Error GoTo La_exit
        '１．データベース接続
        'ConnectDbFBM(My.Application.Info.DirectoryPath & "\ResultImage\")
        ConnectDbFBM(strKojiFolder & "\")
        '２．データ取得
        ReadCameraPose()
        '３．DataGridに表示
        SetDataGrid()

        Exit Sub
La_exit:
        MsgBox("フォームロードエラー", MsgBoxStyle.Critical)
    End Sub
    '20140205 Rep By Yamada Sta ------------------------------------------------------------------------
    '選択した画像ファイルとdbFBMを出力する
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

        On Error GoTo La_exit
        Dim ErrMsg As String = ""
        Dim intRows As Integer = 0
        Dim intNumSelect As Integer = 0
        Dim intNumNonSelect As Integer = 0

        '１．DataGridView1からチェックが付いている行のIDと画像ファイル名を取得

        '20150212 ADD BY Yamada Sta---------------------
        ErrMsg = "出力フォルダを指定して下さい。"
        If TextBox1.Text = "" Then
            MsgBox(ErrMsg, MsgBoxStyle.Critical)
            Exit Sub
        End If
        '20150212 ADD BY Yamada End---------------------

        If My.Computer.FileSystem.DirectoryExists(TextBox1.Text) = False Then
            My.Computer.FileSystem.CreateDirectory(TextBox1.Text)
        End If
        selectImageFolder = TextBox1.Text & "\"

        With DataGridView1
            intRows = .RowCount
            strOutImages = Nothing
            strOutIDs = Nothing
            strNotOutIDs = Nothing
            ReDim strOutImages(intRows - 1)
            ReDim strOutIDs(intRows - 1)
            ReDim strNotOutIDs(intRows - 1)

            'チェック有りの画像ファイル名取得
            ErrMsg = "画層ファイル名とID取得中"
            intNumSelect = 0
            intNumNonSelect = 0
            For i = 0 To intRows - 1
                If .Rows(i).Cells(0).Value = True Then
                    strOutIDs(intNumSelect) = CStr(.Rows(i).Cells(1).Value)
                    strOutImages(intNumSelect) = CStr(.Rows(i).Cells(2).Value)
                    intNumSelect += 1
                Else
                    strNotOutIDs(intNumNonSelect) = CStr(.Rows(i).Cells(1).Value)
                    intNumNonSelect += 1
                End If
            Next

            '20150212 ADD BY Yamada Sta---------------------
            ErrMsg = "出力する画像を選択して下さい。"
            If intNumSelect = 0 Then
                MsgBox(ErrMsg, MsgBoxStyle.Critical)
                Exit Sub
            End If
            '20150212 ADD BY Yamada Sta---------------------

            'j = 0
            'For i = 0 To intRows - 1
            '    If .Rows(i).Cells(0).Value = False Then
            '        strNotOutIDs(j) = CStr(.Rows(i).Cells(1).Value)
            '        'strOutImages(j) = CStr(.Rows(i).Cells(2).Value)
            '        j = j + 1
            '    End If
            'Next
        End With

        '２．dbFBM.mdbをResultImageフォルダーからコピー
        ErrMsg = "dbFBM.mdbをResultImageフォルダーからコピー"


        IO.File.Copy(strKojiFolder & "\" & FBM_MDB, selectImageFolder & FBM_MDB, True)
        ConnectDbFBM(selectImageFolder)

        '３．画像ファイルのコピー
        '        Try
        ErrMsg = "画像ファイルのコピー"
        For i = 0 To intNumSelect - 1
            'If strOutImages(i) <> Nothing Then
            IO.File.Copy(strKojiFolder & "\" & strOutImages(i), selectImageFolder & strOutImages(i), True)
            'End If
        Next
        '        Catch ex As Exception

        '        End Try

        '４．CameraPose、Targetsテーブル内の不要なレコードを削除
        '        Try
        ErrMsg = "mdbの内部を更新"
        For i = 0 To (intRows - intNumSelect) - 1
            'CameraPoseテーブル
            'If strNotOutIDs(i) <> Nothing Then
            DeleteCameraPoseTable(strNotOutIDs(i), False)
            'Targetsテーブル
            tmptarget.DeleteData(strNotOutIDs(i), False)
            tmptarget.DeleteDataAllTarget(strNotOutIDs(i), False) 'SUURI ADD 20150410
            'End If
        Next
        'Catch ex As Exception
        'End Try

        '５．CameraPose、TargetsテーブルのID、ImageIDを更新
        Dim strFields() As String
        Dim vSetValues() As String
        Dim strWhere As String
        ReDim strFields(0)
        ReDim vSetValues(0)

        'Try
        ErrMsg = "mdbのテーブルを更新"
        For i = 0 To intNumSelect - 1
            'CameraPoseテーブルの更新
            'If strOutIDs(i) <> Nothing Then
            strFields(0) = "ID" 'フィールド
            vSetValues(0) = i + 1
            strWhere = "ID = " & strOutIDs(i)
            dbClass.DoUpdate(strFields, "CameraPose", vSetValues, strWhere)
            'Targetsテーブルの更新
            strFields(0) = "ImageID" 'フィールド
            strWhere = "ImageID = " & strOutIDs(i)
            dbClass.DoUpdate(strFields, "Targets", vSetValues, strWhere)
            dbClass.DoUpdate(strFields, "AllTargets", vSetValues, strWhere) 'SUURI ADD 20150410
            'End If
        Next
        'Catch ex As Exception

        'End Try

        AccessDisConnect()

        If MsgBox("完了しました。フォルダーを開きますか?", MsgBoxStyle.OkCancel Or MsgBoxStyle.Question) = MsgBoxResult.Ok Then
            System.Diagnostics.Process.Start(TextBox1.Text)
        End If
        Exit Sub '正常終了

La_exit:  '異常終了
        MsgBox("エラー終了 Button1_Click  :" & ErrMsg, MsgBoxStyle.Critical)
    End Sub

    ''選択した画像ファイルとdbFBMを出力する
    'Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

    '    '１．DataGridView1からチェックが付いている行のIDと画像ファイル名を取得
    '    With DataGridView1
    '        Dim n As Integer = .RowCount
    '        Dim j As Integer = 0
    '        strOutImages = Nothing
    '        strOutIDs = Nothing
    '        ReDim strOutImages(n - 2)
    '        ReDim strOutIDs(n - 2)

    '        '選択された画像ファイル名、ID取得
    '        For i = 0 To n - 2
    '            If .Rows(i).Cells(0).Value = True Then
    '                strOutIDs(j) = CStr(.Rows(i).Cells(1).Value)
    '                strOutImages(j) = CStr(.Rows(i).Cells(2).Value)
    '                j = +1
    '            End If
    '        Next

    '        j = 0

    '    End With

    '    '２．dbFBM.mdbをResultImageフォルダーからコピー
    '    IO.File.Copy(resultImageFolder & FBM_MDB, selectImageFolder & FBM_MDB, True)
    '    ConnectDbFBM(selectImageFolder)

    '    '３．画像ファイルのコピー
    '    Try
    '        For i = 0 To strOutImages.Count - 2
    '            IO.File.Copy(resultImageFolder & strOutImages(i), selectImageFolder & strOutImages(i), True)
    '        Next
    '    Catch ex As Exception

    '    End Try






    '    '４．CameraPose、Targetsテーブル内の不要なレコードを削除
    '    Try
    '        For i = 0 To strNotOutIDs.Count - 2
    '            'CameraPoseテーブル
    '            If strNotOutIDs(i) <> Nothing Then
    '                DeleteCameraPoseTable(strNotOutIDs(i), False)
    '                'Targetsテーブル
    '                tmptarget.DeleteData(strNotOutIDs(i), False)
    '            End If
    '        Next
    '    Catch ex As Exception

    '    End Try

    '    '５．CameraPose、Targetsテーブル内のレコードのID、ImageIDを連番に更新




    '    AccessDisConnect()

    '    MsgBox("完了しました")

    'End Sub
    '20140205 Rep By Yamada End ------------------------------------------------------------------------

    Private Sub DataGridView1_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Dim i As Integer = e.RowIndex
        'MsgBox(i)
        Op.SetPart(hwinid1, 0, 0, 2748, 3840)

        preTarget = New TargetDetect


        hv_ALLCTID = Tuple.ReadTuple(My.Application.Info.DirectoryPath & "\RectangleCT_ID.tup")

        '１．選択行の値を取得
        With DataGridView1
            preImage = CStr(.Rows(i).Cells(2).Value)
            preID = CInt(.Rows(i).Cells(1).Value)
        End With

        ''２．DBに接続
        ConnectDbFBM(strKojiFolder & "\")

        '３．画像表示
        Op.ReadImage(ho_Image, strKojiFolder & "\" & preImage)
        Op.DispObj(ho_Image, hwinid1)

        '４．ターゲット情報取得
        preTarget.ReadData(strKojiFolder & "\", preID)

        '５．CT表示
        'Form1.DispCT(hwinid1, preTarget)

        AccessDisConnect()
    End Sub

    'CameraPoseからデータ取得
    Public Sub ReadCameraPose()
        Dim IDR As IDataReader
        Dim strSqlText As String = ""
        strSqlText = "SELECT ID,imagefilename,CX,CY,CZ,CA,CB,CG,flgNotUse,flgSystemNotUse,flgFirst,flgSecond,CamParamID FROM CameraPose"

        If lstCamPose Is Nothing Then
            lstCamPose = New List(Of CameraPose)
        Else
            lstCamPose.Clear()
        End If

        IDR = dbClass.DoSelect(strSqlText)
        If Not IDR Is Nothing Then
            Do While IDR.Read
                Dim objCamPose As New CameraPose
                objCamPose.Cid = CInt(IDR.GetValue(0))
                objCamPose.CFileName = IDR.GetValue(1)
                objCamPose.CX = CInt(IDR.GetValue(2))
                objCamPose.CY = CInt(IDR.GetValue(3))
                objCamPose.CZ = CInt(IDR.GetValue(4))
                objCamPose.CA = CInt(IDR.GetValue(5))
                objCamPose.CB = CInt(IDR.GetValue(6))
                objCamPose.CG = CInt(IDR.GetValue(7))
                objCamPose.CflgNotUse = CInt(IDR.GetValue(8))
                objCamPose.CflgSystemNotUse = CInt(IDR.GetValue(9))
                objCamPose.CflgFirst = CInt(IDR.GetValue(10))
                objCamPose.CflgSecond = CInt(IDR.GetValue(11))
                objCamPose.CCamParamID = CInt(IDR.GetValue(12))
                lstCamPose.Add(objCamPose)
            Loop
            IDR.Close()
        End If
        AccessDisConnect()
    End Sub

    'DataGridにバインド
    Private Sub SetDataGrid()

        Dim n As Integer = lstCamPose.Count
        With DataGridView1
            .ColumnCount = 14
            For i = 0 To n - 1
                .Rows().Add()
                .Rows(i).Cells(0).Value = False
                .Rows(i).Cells(1).Value = lstCamPose(i).Cid
                .Rows(i).Cells(2).Value = CStr(lstCamPose(i).CFileName)
                '.Rows(i).Cells(3).Value = lstCamPose(i).CX
                '.Rows(i).Cells(4).Value = lstCamPose(i).CY
                '.Rows(i).Cells(5).Value = lstCamPose(i).CZ
                '.Rows(i).Cells(6).Value = lstCamPose(i).CA
                '.Rows(i).Cells(7).Value = lstCamPose(i).CB
                '.Rows(i).Cells(8).Value = lstCamPose(i).CZ
                '.Rows(i).Cells(9).Value = lstCamPose(i).CflgNotUse
                '.Rows(i).Cells(10).Value = lstCamPose(i).CflgSystemNotUse
                '.Rows(i).Cells(11).Value = lstCamPose(i).CflgFirst
                '.Rows(i).Cells(12).Value = lstCamPose(i).CflgSecond
                .Rows(i).Cells(13).Value = lstCamPose(i).CCamParamID
            Next

            'ADD By 20150316 "ID"でソーティング追加　＞＞VFORMの画像描画でエラーが起きるため
            .Sort(Column2, System.ComponentModel.ListSortDirection.Ascending)

        End With

    End Sub

    'CameraPoseテーブル内のレコードを削除
    Public Sub DeleteCameraPoseTable(ByVal strID As String, ByRef blnNot As Boolean)
        Dim strWhere As String = ""

        If strID <> "" Then
            If blnNot = True Then
                strWhere = "NOT ID=" & strID
            Else
                strWhere = "ID=" & strID
            End If
        End If
        

        If dbClass.DoDelete("CameraPose", strWhere) < 0 Then
            'MsgBox("DB更新に失敗しました。", MsgBoxStyle.OkOnly, "エラー")
            Exit Sub
        End If
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Dim m_fbd As FolderBrowserDialog        '新規フォルダ作成可
        m_fbd = New FolderBrowserDialog
        With m_fbd
            Dim iDialogResult As System.Windows.Forms.DialogResult

            m_fbd.SelectedPath = strKojiFolder
            iDialogResult = .ShowDialog()
            If iDialogResult = System.Windows.Forms.DialogResult.OK Then
                ''選択されたフォルダを表示する
                TextBox1.Text = m_fbd.SelectedPath
            Else
                Exit Sub
            End If
        End With

    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs)
        Me.Hide()
    End Sub



#Region "ZoomKanren"

   
    Dim intZW As Integer
    Dim intZH As Integer

    Public Ctrl_MouseWheel As Integer = 0

    Dim ZoomFactor As Integer = 1.5  '--10
    Dim minZoomSize As Integer = 5 '最小表示範囲ＰＩＸＥＬ
    Dim flgDraw As Boolean = False
    Private Function ResizePart(ByRef win As Object, ByVal Delta As Integer) As Boolean
        Dim R As Object = Nothing, C As Object = Nothing
        Dim Rw1 As Object = Nothing, Cw1 As Object = Nothing
        Dim Rw2 As Object = Nothing, Cw2 As Object = Nothing
        Dim Rd1 As Object = Nothing, Cd1 As Object = Nothing
        Dim Rd2 As Object = Nothing, Cd2 As Object = Nothing

        Dim ZF As Double
        Try

            Op.GetMposition(win, R, C, Nothing)
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


#End Region

    Private Sub Form3_MouseWheel(sender As Object, e As MouseEventArgs) Handles Me.MouseWheel
        'If ResizePart(hwinid1, e.Delta) = True Then
        '    '      DispImage(winpreview, ImageListView.SelectedIndices.Item(0))

        '    Form1.DispCT(hwinid1, preTarget)
        'End If

        If ResizePart(hwinid1, e.Delta) = True Then
            If Not preTarget Is Nothing Then
                '   Op.DispImage(ho_Image, hwinid1)
                Op.DispObj(ho_Image, hwinid1)
                'Form1.DispCT(hwinid1, preTarget)
            End If

        End If
    End Sub


    Private Sub AxHWindowXCtrl1_MouseDownEvent(sender As Object, e As AxHALCONXLib._HWindowXCtrlEvents_MouseDownEvent) Handles AxHWindowXCtrl1.MouseDownEvent
        'Stop
        Try
            Dim R As Object = Nothing, C As Object = Nothing
            Dim mBtn As Object = Nothing
            Op.GetMposition(hwinid1, R, C, mBtn)

            Dim objDist As Object = DBNull.Value
            Dim minVal As Double = Double.MaxValue
            Dim tmpSt As SingleTarget = Nothing

            If mBtn = 4 Then
                For Each ST As SingleTarget In preTarget.lstST
                    ST.P2D.CalcDistToInputPoint(R, C, objDist)
                    If objDist < minVal Then
                        minVal = objDist
                        tmpSt = ST
                    End If
                Next
                Dim frmSTtoCT As New STtoCTpoint
                frmSTtoCT.SelectedST = tmpSt
                frmSTtoCT.ShowDialog()
                If frmSTtoCT.SelectedCT <> "" Then
                    ConnectDbFBM(strKojiFolder & "\")
                    preTarget.OneSTtoCTpoint(tmpSt, frmSTtoCT.SelectedCT)
                    ' preTarget.SaveData()        'Targetsテーブル
                    AccessDisConnect()
                End If
            End If

        Catch ex As Exception
            Dim tt As Integer = 1
            MsgBox("画面1クリックイベントエラー")

        End Try
    End Sub
End Class