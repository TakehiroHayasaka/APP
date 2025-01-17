﻿Public Class FrmReg
    Dim lstKihonInfo As New List(Of KihonInfoTable)
    Dim lstSunpoSet As New List(Of SunpoSetTable)

    Private Sub FrmReg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim strNewKoujiFolder As String = Me.Tag.ToString

        Dim clsKihonInfo As New KihonInfoTable

        Dim flg As Boolean
        If clsKihonInfo.m_dbClass Is Nothing Then
            clsKihonInfo.m_dbClass = New CDBOperateOLE
        End If

        flg = clsKihonInfo.m_dbClass.Connect(strNewKoujiFolder & "\計測データ.mdb")
        lstKihonInfo = clsKihonInfo.GetDataToList()

        For Each Sokutei As KihonInfoTable In lstKihonInfo
            Dim objVal1(1) As Object
            objVal1(0) = Sokutei.item_name
            If Sokutei.value_type.Trim = "DATETIME" Then
                Dim dt As Date
                dt = Now
                'objVal1(1) = dt.ToString("yyyy/MM/dd")
                objVal1(1) = dt.ToString()

            Else
                objVal1(1) = Sokutei.item_value
            End If

            Me.dgKihonInfo.Rows.Add(objVal1)
        Next
        clsKihonInfo.m_dbClass.DisConnect()

        Dim clsSunpoSet As New SunpoSetTable
     
        If clsSunpoSet.m_dbClass Is Nothing Then
            clsSunpoSet.m_dbClass = New CDBOperateOLE
        End If

        flg = clsSunpoSet.m_dbClass.Connect(strNewKoujiFolder & "\計測データ.mdb")
        lstSunpoSet = clsSunpoSet.GetDataToList()
        If lstSunpoSet Is Nothing Then
            Me.Close()
            Exit Sub
        End If
        For Each sun As SunpoSetTable In lstSunpoSet
            If Not (sun.ZU_layer = "Scale") Then 'Add Kiryu 20151105 基準尺非表示(SunpoID 1～6非表示)

                Dim objVal2(2) As Object
                If sun.SunpoName = "車高" Then
                    objVal2(0) = sun.SunpoName
                    objVal2(1) = FrmMainHiroshima.dgMeasureResult.Item(1, 2).Value
                    ' objVal2(1) = Me.Parent.dgMeasureResult.Item(1, 10).Value
                    objVal2(2) = ""
                    sun.SunpoVal = objVal2(1)
                    Me.dgSokutechi.Rows.Add(objVal2)
                Else
                    objVal2(0) = sun.SunpoName
                    objVal2(1) = Math.Round(CDbl(sun.SunpoVal) + 0.5, 0)
                    objVal2(2) = ""
                    Me.dgSokutechi.Rows.Add(objVal2)
                End If
            End If
        Next
        clsSunpoSet.m_dbClass.DisConnect()

        'Dim objVal(1) As Object
        'objVal(0) = "車高"
        'objVal(1) = FrmMain.dgMeasureResult.Item(1, 10).Value
        'Me.dgSokutechi.Rows.Add(objVal)

        Me.txtGetFolderShare.Text = My.Settings.shareFolder.ToString
        Me.txtGetFolderLocal.Text = My.Settings.localFolder.ToString
    End Sub

    Private Sub btnGetFolderShare_Click(sender As Object, e As EventArgs) Handles btnGetFolderShare.Click
        'FolderBrowserDialogクラスのインスタンスを作成
        Dim fbd As New FolderBrowserDialog

        '上部に表示する説明テキストを指定する
        fbd.Description = "共有フォルダを指定してください。"
        'ルートフォルダを指定する
        'デフォルトでDesktop
        fbd.RootFolder = Environment.SpecialFolder.Desktop
        '最初に選択するフォルダを指定する
        'RootFolder以下にあるフォルダである必要がある
        fbd.SelectedPath = "C:\Windows"
        'ユーザーが新しいフォルダを作成できるようにする
        'デフォルトでTrue
        fbd.ShowNewFolderButton = True

        'ダイアログを表示する
        If fbd.ShowDialog(Me) = DialogResult.OK Then
            '選択されたフォルダを表示する
            Me.txtGetFolderShare.Text = fbd.SelectedPath
        End If

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        saveFolderPath()
        Me.Close()
    End Sub

    Private Sub btnGetFolderLocal_Click(sender As Object, e As EventArgs) Handles btnGetFolderLocal.Click
        'FolderBrowserDialogクラスのインスタンスを作成
        Dim fbd As New FolderBrowserDialog

        '上部に表示する説明テキストを指定する
        fbd.Description = "ローカルを指定してください。"
        'ルートフォルダを指定する
        'デフォルトでDesktop
        fbd.RootFolder = Environment.SpecialFolder.Desktop
        '最初に選択するフォルダを指定する
        'RootFolder以下にあるフォルダである必要がある
        fbd.SelectedPath = "C:\Windows"
        'ユーザーが新しいフォルダを作成できるようにする
        'デフォルトでTrue
        fbd.ShowNewFolderButton = True

        'ダイアログを表示する
        If fbd.ShowDialog(Me) = DialogResult.OK Then
            '選択されたフォルダを表示する
            Me.txtGetFolderLocal.Text = fbd.SelectedPath
        End If
    End Sub

    Private Sub btnRegist_Click(sender As Object, e As EventArgs) Handles btnRegist.Click
        Dim ret As Integer
        ret = validatePath()

        Dim dt As Date
        dt = Now

        Dim dtToString As String
        dtToString = dt.ToString("yyMMdd-HHmm")

        Dim koban As String
        Dim shinaban As String
        koban = Me.dgKihonInfo.Item(1, 1).Value
        'shinaban = Me.dgKihonInfo.Item(1, 2).Value
        'koban = Me.dgSokutechi.Item(1, 1).Value 'グリッドがからの状態は何もしないように変更が必要
        'shinaban = Me.dgSokutechi.Item(1, 2).Value


        'Dim crtDirInfo As String = dtToString & "_[" & koban & "][" & shinaban & "]"
        Dim crtDirInfo As String = dtToString & "_[" & koban & "]"


        Try
            Dim strLocal As String
            Dim strshare As String
            Dim strnewfolder As String
            Dim strFileName As String
            strFileName = "寸法測定値_" & dtToString & ".csv"
            Select Case ret
                Case 0
                    MsgBox("選択されたフォルダは存在しません")
                Case 1
                    ' フォルダ (ディレクトリ) を作成する
                    strshare = Me.txtGetFolderShare.Text & "\" & crtDirInfo
                    System.IO.Directory.CreateDirectory(strshare)
                    createCSV(strshare, strFileName)
                    MsgBox("共有フォルダに登録しました。")
                Case 2
                    ' フォルダ (ディレクトリ) を作成する
                    strLocal = Me.txtGetFolderLocal.Text & "\" & crtDirInfo
                    System.IO.Directory.CreateDirectory(strLocal)
                    createCSV(strLocal, strFileName)
                    MsgBox("ローカルフォルダに登録しました。")
                Case 3
                    ' フォルダ (ディレクトリ) を作成する
                    strshare = Me.txtGetFolderShare.Text & "\" & crtDirInfo
                    System.IO.Directory.CreateDirectory(strshare)
                    createCSV(strshare, strFileName)
                    ' フォルダ (ディレクトリ) を作成する
                    strLocal = Me.txtGetFolderLocal.Text & "\" & crtDirInfo
                    System.IO.Directory.CreateDirectory(strLocal)
                    createCSV(strLocal, strFileName)
                    MsgBox("登録しました。")

            End Select
            'MsgBox("登録しました。")
            saveFolderPath()
        Catch ex As Exception
            MsgBox("フォルダ作成、もしくはファイル作成に失敗しました。")
        End Try

    End Sub
    'Return 0 共有。ローカルフォルダが両方設定されていない
    'Return 1 共有フォルダが両方設定されていない
    'Return 2 ローカルフォルダが両方設定されていない
    'Return 3 共有。ローカルフォルダが両方設定されている
    Private Function validatePath() As Integer
        Dim isLocal, isShare As Boolean
        Dim ret As Integer = 0
        If Me.txtGetFolderLocal.Text <> "" And System.IO.Directory.Exists(Me.txtGetFolderLocal.Text) Then
            isLocal = True
        End If
        If Me.txtGetFolderShare.Text <> "" And System.IO.Directory.Exists(Me.txtGetFolderShare.Text) Then
            isShare = True
        End If

        If isLocal And isShare Then
            Return 3
            Exit Function
        ElseIf isLocal Then
            'msgbox 
            Return 2
            Exit Function
        ElseIf isShare Then
            'msgbox 
            Return 1
            Exit Function
        End If
        'msgbox 
        Return ret
    End Function
    Private Sub createCSV(ByVal strPath As String, ByVal strFileName As String)
        '(20150807 Tezuka Change) Excel表示したときの文字化けを防ぐ変更
        'Dim sw As New System.IO.StreamWriter(strPath & "\" & strFileName)
        Dim sw As New System.IO.StreamWriter(strPath & "\" & strFileName, False, System.Text.Encoding.GetEncoding("shift_jis"))

        Dim line1Arr As String()
        Dim line2Arr As String()
        Dim i As Integer

        ReDim Preserve line1Arr(2)
        ReDim Preserve line2Arr(2)

        For i = 0 To 2
            line1Arr(i) = Me.dgKihonInfo.Item(0, i).Value
            line2Arr(i) = Me.dgKihonInfo.Item(1, i).Value
        Next i
        For i = 1 To Me.dgSokutechi.Rows.Count
            ReDim Preserve line1Arr(3 + 2 * i)
            ReDim Preserve line2Arr(3 + 2 * i)
            'ReDim Preserve line1Arr(i)
            'ReDim Preserve line2Arr(i)

            '**削除(Add Kiryu 20151105)**
            'line1Arr(2 + 2 * i - 1) = Me.dgSokutechi.Item(0, i - 1).Value & "(自)"
            'line1Arr(2 + 2 * i) = Me.dgSokutechi.Item(0, i - 1).Value & "(手)"
            'line2Arr(2 + 2 * i - 1) = Me.dgSokutechi.Item(1, i - 1).Value
            'line2Arr(2 + 2 * i) = Me.dgSokutechi.Item(2, i - 1).Value’
            '**削除(End Kiryu 20151105)**

            'Add Kiryu 20151105 csv出力を測定値のみに
            line1Arr(3 + i - 1) = Me.dgSokutechi.Item(0, i - 1).Value
            line2Arr(3 + i - 1) = Me.dgSokutechi.Item(1, i - 1).Value

        Next i



        Dim s2 As String
        s2 = String.Join(",", line1Arr)
        sw.WriteLine(s2)

        s2 = String.Join(",", line2Arr)
        sw.WriteLine(s2)

        sw.Close()

    End Sub
    Private Sub saveFolderPath()

        My.Settings.shareFolder = Me.txtGetFolderShare.Text
        My.Settings.localFolder = Me.txtGetFolderLocal.Text

    End Sub

    Private Sub btnPrint_Click(sender As System.Object, e As System.EventArgs) Handles btnPrint.Click
        Dim strNewKoujiFolder As String = Me.Tag.ToString
        Dim i As Integer = 0

        For Each Sokutei As KihonInfoTable In lstKihonInfo
            Sokutei.item_value = Me.dgKihonInfo.Item(1, i).Value
            i = i + 1
        Next
      
        Dim clsSunpoSet As New SunpoSetTable
        clsSunpoSet.m_dbClass = New CDBOperateOLE
        If clsSunpoSet.m_dbClass.Connect(My.Settings.YCMFolder & "\計測システムフォルダ\システム設定.mdb") = False Then
            MsgBox("システム設定データベースに接続できませんでした。")
            Exit Sub
        End If
        'SunpoSetセルテーブルリスト
        Dim SunpoSetCellL As List(Of SunpoSetTable)
        SunpoSetCellL = clsSunpoSet.GetCellDataToList()

        'Excelテンプレートファイルを工事フォルダにコピー
        Dim strExcelTemplateFileName As String
        Dim strChyohyoExcelFilePath As String = strNewKoujiFolder & "\Chyohyo.xls"
        strExcelTemplateFileName = clsSunpoSet.GetExcelTemplateFileName
        clsSunpoSet.m_dbClass.DisConnect()
        Try
            'テンプレートを出力先にコピーする
            System.IO.File.Copy(My.Settings.YCMFolder & "\計測システムフォルダ\Template\" & strExcelTemplateFileName & ".xls", strChyohyoExcelFilePath, True)
        Catch ex As System.IO.IOException
            Exit Sub
        Catch ex As Exception
            Exit Sub
        End Try

        SetMessageToExcel(strChyohyoExcelFilePath, lstKihonInfo, lstSunpoSet, SunpoSetCellL)

        MsgBox("帳票印刷完了しました。")

    End Sub
   

    Public Sub SetMessageToExcel(ByVal strFilePath As String, ByVal KihonL As List(Of KihonInfoTable), ByVal SunpoSetL As List(Of SunpoSetTable), ByVal SunpoSetCellL As List(Of SunpoSetTable))

        Dim xlApp As Excel.Application
        Dim xlBook As Excel.Workbook
        Dim xlSheet As Excel.Worksheet

        Dim strFilename As String                             'ファイル名(フルパス)
        strFilename = strFilePath                             'ファイル名をセット

        Dim strSheetName As String                            'シート名
        strSheetName = "Sheet1"

        Dim xlBooks As Excel.Workbooks
        Dim xlSheets As Excel.Sheets

        Try

            GetExcelPID()
            xlApp = New Excel.Application
            GetExcelNewPID()
            xlBooks = xlApp.Workbooks
            xlBook = xlBooks.Open(strFilename)
            xlSheets = xlBook.Worksheets
            xlSheet = xlSheets.Item(strSheetName)

            '基本情報を設定する

            For i As Integer = 0 To KihonL.Count - 1
                Try
                    Dim xlRangeItem As Excel.Range
                    xlRangeItem = xlSheet.Range(KihonL(i).item_cell_name)
                    xlRangeItem.Value = KihonL(i).item_value
                    MRComObject(xlRangeItem)
                Catch ex As Exception
                End Try
            Next

            For i As Integer = 0 To SunpoSetCellL.Count - 1
                Dim indexSunposet As Integer = -1

                Dim t As Integer = 0
                Dim intSunpoId As Integer = SunpoSetCellL(i).SunpoID

                For Each SSL As SunpoSetTable In SunpoSetL

                    If SSL.SunpoID = intSunpoId Then
                        'If MeasurementSet = SSL.MeasurementSet Or MeasurementSet = "" Or SSL.MeasurementSet = "" Then
                        indexSunposet = t
                        'End If
                    End If
                    t += 1
                Next
                If SunpoSetCellL(i).SunpoMark = "g" Then
                    Dim s As Integer = -1
                End If
                If SunpoSetCellL(i).CT_Active = "1" Then

                    '規定値
                    Try
                        Dim xlRangeVal As Excel.Range
                        xlRangeVal = xlSheet.Range(SunpoSetCellL(i).KiteiVal)
                        xlRangeVal.Value = SunpoSetL(indexSunposet).KiteiVal
                        MRComObject(xlRangeVal)
                    Catch ex As Exception
                    End Try
                    '最小許容値
                    Try
                        Dim xlRangeMin As Excel.Range
                        xlRangeMin = xlSheet.Range(SunpoSetCellL(i).KiteiMin)
                        xlRangeMin.Value = SunpoSetL(indexSunposet).KiteiMin
                        MRComObject(xlRangeMin)
                    Catch ex As Exception
                    End Try
                    '最大許容値
                    Try
                        Dim xlRangeMax As Excel.Range
                        xlRangeMax = xlSheet.Range(SunpoSetCellL(i).KiteiMax)
                        xlRangeMax.Value = SunpoSetL(indexSunposet).KiteiMax
                        MRComObject(xlRangeMax)
                    Catch ex As Exception
                    End Try
                    '計測値
                    Try
                        Dim xlRangeSVal As Excel.Range
                        xlRangeSVal = xlSheet.Range(SunpoSetCellL(i).SunpoVal)
                        xlRangeSVal.Value = SunpoSetL(indexSunposet).SunpoVal
                        MRComObject(xlRangeSVal)
                    Catch ex As Exception
                    End Try
                    '合否
                    Try
                        Dim xlRangeGouhi As Excel.Range
                        xlRangeGouhi = xlSheet.Range(SunpoSetCellL(i).flg_gouhi)
                        If SunpoSetL(indexSunposet).flg_gouhi = "1" Then
                            xlRangeGouhi.Value = "合"
                        Else
                            xlRangeGouhi.Value = "否"
                        End If
                        MRComObject(xlRangeGouhi)
                    Catch ex As Exception
                    End Try
                End If
            Next

            Dim rc As Integer = 0

            xlApp.DisplayAlerts = False
            xlBook.Save()
            xlSheet.PrintOut()
            MRComObject(xlSheet)
            MRComObject(xlSheets)
            xlBook.Close(False)
            MRComObject(xlBook)
            MRComObject(xlBooks)
            xlApp.Quit()
            MRComObject(xlApp)

            GC.Collect()

        Catch ex As Exception

        End Try

        ExKillProcess()

        '  System.Diagnostics.Process.Start(strFilePath)

    End Sub

End Class