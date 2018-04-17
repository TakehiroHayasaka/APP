Imports System.IO
Imports Microsoft.VisualBasic.FileIO

Public Class FrmReg_hiroshima
    Dim lstKihonInfo As New List(Of KihonInfoTable)
    Dim lstSunpoSet As New List(Of SunpoSetTable)

    Private TemplateFolder As String = System.IO.Directory.GetParent(My.Settings.YCMFolder).ToString & "\Template\"
    Private strCurrentDir As String = System.IO.Directory.GetCurrentDirectory()
    Private strShogenDataFilePath As String = System.IO.Directory.GetParent(strCurrentDir).ToString


    Private Sub FrmReg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim strNewKoujiFolder As String = Me.Tag.ToString

        '計測データ.mdbから計測値を読込み、DGにセット
        Dim clsSunpoSet As New SunpoSetTable

        If clsSunpoSet.m_dbClass Is Nothing Then
            clsSunpoSet.m_dbClass = New CDBOperateOLE
        End If

        Dim flg As Boolean
        flg = clsSunpoSet.m_dbClass.Connect(strNewKoujiFolder & "\計測データ.mdb")
        lstSunpoSet = clsSunpoSet.GetDataToList()
        clsSunpoSet.m_dbClass.DisConnect()

        If lstSunpoSet Is Nothing Then
            Me.Close()
            Exit Sub
        End If

        For Each sun As SunpoSetTable In lstSunpoSet
            If Not (sun.ZU_layer = "Scale") Then 'Add Kiryu 20151105 基準尺非表示(SunpoID 1～6非表示)

                Dim objVal2(2) As Object
                If sun.SunpoName = "全高" Then
                    objVal2(0) = sun.SunpoName
                    objVal2(1) = FrmMainHiroshima.dgMeasureResult.Item(1, 2).Value
                    objVal2(2) = ""
                    sun.SunpoVal = objVal2(1)
                    Me.dgSokutechi.Rows.Add(objVal2)
                ElseIf sun.SunpoID = 13 Then
                    'skip
                Else
                    objVal2(0) = sun.SunpoName
                    'objVal2(1) = Math.Round(CDbl(sun.SunpoVal) + 0.5, 0)
                    objVal2(1) = Math.Round(CDbl(sun.SunpoVal), 0)
                    objVal2(2) = ""
                    Me.dgSokutechi.Rows.Add(objVal2)
                End If
            End If
        Next

        SetDgKihonInfo(strNewKoujiFolder)


        dgSokutechi.Item(0, 4).Value = "オフセット"
        ' dgSokutechi.Item(0, 5).Value = "オフセット(左)"
        dgSokutechi.Item(0, 10 - 1).Value = "リヤバンパー：横方向入り込み(左)"
        dgSokutechi.Item(0, 11 - 1).Value = "リヤバンパー：横方向入り込み(右)"

        dgSokutechi.Item(1, 4).Value = lstSunpoSet(8).ExSunpoVal
        ' dgSokutechi.Item(1, 5).Value = lstSunpoSet(9).ExSunpoVal
        dgSokutechi.Item(1, 10 - 1).Value = lstSunpoSet(14).ExSunpoVal
        dgSokutechi.Item(1, 11 - 1).Value = lstSunpoSet(15).ExSunpoVal

        roadExcelTemplateName(TemplateFolder)


        'Dim objVal(1) As Object
        'objVal(0) = "車高"
        'objVal(1) = FrmMain.dgMeasureResult.Item(1, 10).Value
        'Me.dgSokutechi.Rows.Add(objVal)

        'Me.txtGetFolderShare.Text = My.Settings.shareFolder.ToString
        Me.txtGetFolderLocal.Text = My.Settings.localFolder.ToString
    End Sub

    Private Sub SetDgKihonInfo(ByVal strNewKoujiFolder As String)
        Dim clsKihonInfo As New KihonInfoTable

        Dim flg As Boolean
        If clsKihonInfo.m_dbClass Is Nothing Then
            clsKihonInfo.m_dbClass = New CDBOperateOLE
        End If

        flg = clsKihonInfo.m_dbClass.Connect(strNewKoujiFolder & "\計測データ.mdb")
        lstKihonInfo = clsKihonInfo.GetDataToList()
        clsKihonInfo.m_dbClass.DisConnect()

        If lstKihonInfo Is Nothing Then
            Me.Close()
            Exit Sub
        End If

        For i As Integer = 0 To 2
            Dim objVal1(1) As Object
            objVal1(0) = lstKihonInfo(i).item_name
            If lstKihonInfo(i).value_type.Trim = "DATETIME" Then
                Dim dt As Date
                dt = Now
                'objVal1(1) = dt.ToString("yyyy/MM/dd")
                objVal1(1) = dt.ToString()

            Else
                objVal1(1) = lstKihonInfo(i).item_value
            End If

            Me.dgKihonInfo.Rows.Add(objVal1)

        Next

    End Sub

    Private Sub roadExcelTemplateName(ByVal Folder As String)
        Dim strFiles() As String
        Dim strFile As String

        Try
            strFiles = Directory.GetFiles(Folder, "*.xls")
        Catch ex As Exception
            MsgBox("Templateフォルダが見つかりませんでした。フォルダ名やフォルダの保存場所をご確認ください。")
            Exit Sub
        End Try

        For Each strFile In strFiles
            CmbSelectExcelTmplate.Items.Add(Path.GetFileName(strFile))
        Next

        CmbSelectExcelTmplate.Text = CmbSelectExcelTmplate.GetItemText("STD.xls")

        CmbSelectExcelTmplate.DropDownStyle = ComboBoxStyle.DropDownList

    End Sub

    Private Sub btnGetFolderShare_Click(sender As Object, e As EventArgs)
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
            'Me.txtGetFolderShare.Text = fbd.SelectedPath
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

    Private Sub btnRegist_Click()
        Dim dt As Date
        dt = Now

        Dim dtToString As String
        dtToString = dt.ToString("yyMMdd-HHmm")

        Dim koban As String
        koban = Me.dgKihonInfo.Item(1, 1).Value

        Try
            Dim strLocal As String
            Dim strFileName As String
            strFileName = koban & ".csv"

            ' フォルダ (ディレクトリ) を作成する
            strLocal = Me.txtGetFolderLocal.Text
            createCSV(strLocal, strFileName)

            saveFolderPath()
        Catch ex As Exception
            'MsgBox(ex.Message)
            MsgBox("指定した登録先フォルダが見つかりませんでした。")
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
        'If Me.txtGetFolderShare.Text <> "" And System.IO.Directory.Exists(Me.txtGetFolderShare.Text) Then
        '    isShare = True
        'End If

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


        ReDim Preserve line1Arr(3 + 2 * Me.dgSokutechi.Rows.Count + 4)
        ReDim Preserve line2Arr(3 + 2 * Me.dgSokutechi.Rows.Count + 4)

        For i = 1 To Me.dgSokutechi.Rows.Count
            'ReDim Preserve line1Arr(3 + 2 * i)
            'ReDim Preserve line2Arr(3 + 2 * i)

            'Add Kiryu 20151105 csv出力を測定値のみに
            line1Arr(3 + i - 1) = Me.dgSokutechi.Item(0, i - 1).Value
            line2Arr(3 + i - 1) = Me.dgSokutechi.Item(1, i - 1).Value
        Next i

        '基準定規、精度確認定規を追加
        i = 0
        For Each sun As SunpoSetTable In lstSunpoSet
            If (sun.ZU_layer = "Scale") Then 'Add Kiryu 20151105 基準尺非表示(SunpoID 1～6非表示)
                line1Arr(3 + i + Me.dgSokutechi.Rows.Count) = sun.SunpoName
                line2Arr(3 + i + Me.dgSokutechi.Rows.Count) = (sun.SunpoVal - sun.KiteiVal).ToString("#0.0")
                i = i + 1
            End If
        Next

        Dim s2 As String
        s2 = String.Join(",", line1Arr)
        sw.WriteLine(s2)

        s2 = String.Join(",", line2Arr)
        sw.WriteLine(s2)

        sw.Close()

    End Sub
    Private Sub saveFolderPath()

        'My.Settings.shareFolder = Me.txtGetFolderShare.Text
        My.Settings.localFolder = Me.txtGetFolderLocal.Text

    End Sub

    Private Sub btnPrint_Click(sender As System.Object, e As System.EventArgs) Handles btnPrint.Click

        Dim koban As String = Me.dgKihonInfo.Item(1, 1).Value

        If koban = "" Then
            MsgBox("工番が入力されていません！", MessageBoxIcon.Error)
            Exit Sub
        End If

        If CmbSelectExcelTmplate.SelectedItem = "" Then
            MsgBox("帳票テンプレートが選択されていません！", MessageBoxIcon.Error)
            Exit Sub
        End If

        If Not System.IO.Directory.Exists(txtGetFolderLocal.Text) Then
            MsgBox("指定した登録先フォルダが存在しません。")
            Exit Sub
        End If


        Dim ShogenData As String()
        ShogenData = FindKobanByShogenDateCSV(strShogenDataFilePath & "\車輌諸元設計寸法データ.csv", ",", koban)



        'If ShogenData Is Nothing Then
        '    Dim ret As DialogResult = MsgBox("車輌諸元設計寸法データから工番が見つかりませんでした。" & vbCrLf & "測定結果Excelを表示しますか？(設計寸法は出力されません。)", MessageBoxButtons.YesNo)
        '    If ret = DialogResult.No Then
        '        Exit Sub
        '    End If
        'Else
        '    For i As Integer = 0 To 2
        '        lstKihonInfo(i).item_value = Me.dgKihonInfo.Item(1, i).Value
        '    Next
        '    For i As Integer = 3 To ShogenData.Count - 1
        '        lstKihonInfo(i).item_value = ShogenData(i - 2)
        '    Next
        'End If

        If ShogenData Is Nothing Then
            Dim ret As DialogResult = MsgBox("車輌諸元設計寸法データから工番が見つかりませんでした。" & vbCrLf & "測定結果Excelを表示しますか？(設計寸法は出力されません。)", MessageBoxButtons.YesNo)
            If ret = DialogResult.No Then
                Exit Sub
            End If
        Else
            For i As Integer = 3 To ShogenData.Count - 1
                lstKihonInfo(i).item_value = ShogenData(i - 2)
            Next
        End If

        For i As Integer = 0 To 2
            lstKihonInfo(i).item_value = Me.dgKihonInfo.Item(1, i).Value
        Next

        'lstKihonInfo(ShogenData.Count + 3 - 1).item_value = IO.Path.GetFileName(Me.Tag.ToString)
        lstKihonInfo(lstKihonInfo.Count - 1).item_value = IO.Path.GetFileName(Me.Tag.ToString)



        'Dim strNewKoujiFolder As String = Me.Tag.ToString
        'Dim clsKihonInfo As New KihonInfoTable
        'Dim flg As Boolean
        'If clsKihonInfo.m_dbClass Is Nothing Then
        '    clsKihonInfo.m_dbClass = New CDBOperateOLE
        'End If
        'flg = clsKihonInfo.m_dbClass.Connect(strNewKoujiFolder & "\計測データ.mdb")

        'Dim strSql As String
        'For Each kihon As KihonInfoTable In lstKihonInfo

        '    strSql = "UPDATE KihonInfo Set ItemValue = " & kihon.item_value & " WHERE ID =" & kihon.ID
        '    clsKihonInfo.m_dbClass.ExecuteSQL(strSql)

        'Next

        ''lstKihonInfo = clsKihonInfo.GetDataToList()
        'clsKihonInfo.m_dbClass.DisConnect()





        SetChyouhyou(CmbSelectExcelTmplate.SelectedItem, txtGetFolderLocal.Text & "\" & koban & ".xls")

    End Sub


    'Add Kiryu 20170908 
    '入力
    'strFileName :車輌諸元寸法データ.csvのパス
    'delimiter   :デリミター(csvなので","を使用)
    'koban       :検索した工番
    '出力
    'Return      : 検索に成功した場合 工番の見つかった行全て
    '            : 検索に失敗した場合 Nothing
    Private Function FindKobanByShogenDateCSV(ByVal strFileName As String, ByVal delimiter As String, ByVal koban As String)

        Dim Fields As String()
        Dim Header As String()

        Try
            Using parser As New TextFieldParser(strFileName, System.Text.Encoding.Default)
                parser.SetDelimiters(delimiter)
                Header = parser.ReadFields() 'ヘッダの読込み
                While Not parser.EndOfData
                    ' Read in the fields for the current line
                    Fields = parser.ReadFields()
                    If Fields(0) = koban Then
                        Return Fields
                    End If
                End While
            End Using

            Return Nothing
        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        End Try


    End Function

    ''
    'strExcelTemplateFileName　：　Excelテンプレートのコピー元フォルダ
    'strChyohyoExcelFilePath
    Private Sub SetChyouhyou(ByVal strExcelTemplateFileName As String, ByVal strChyohyoExcelFilePath As String)

        Dim strNewKoujiFolder As String = Me.Tag.ToString
        Dim i As Integer = 0

        Dim clsSunpoSet As New SunpoSetTable
        clsSunpoSet.m_dbClass = New CDBOperateOLE
        If clsSunpoSet.m_dbClass.Connect(My.Settings.YCMFolder & "\計測システムフォルダ\システム設定.mdb") = False Then
            MsgBox("システム設定データベースに接続できませんでした。")
            Exit Sub
        End If
        'SunpoSetセルテーブルリスト
        Dim SunpoSetCellL As List(Of SunpoSetTable)
        SunpoSetCellL = clsSunpoSet.GetCellDataToList()

        clsSunpoSet.m_dbClass.DisConnect()

        Try
            If System.IO.File.Exists(strChyohyoExcelFilePath) Then
                'ファイルがすでに存在する場合
                Dim ret As DialogResult = MsgBox(Path.GetFileName(strChyohyoExcelFilePath) & "はすでに存在します。上書きしてもよろしいですか？", MessageBoxButtons.YesNo)
                If ret = DialogResult.Yes Then
                    '上書きしますか→はい　上書き処理
                    System.IO.File.Copy(TemplateFolder & strExcelTemplateFileName, strChyohyoExcelFilePath, True)
                    btnRegist_Click()
                Else
                    '上書きしますか→いいえ tmpフォルダを作成し、その下にコピー。Excel終了後にtmpごと自動削除
                    Dim tmpFolderPath As String = System.IO.Path.GetDirectoryName(strChyohyoExcelFilePath) & "\tmp\"
                    Dim koban As String = Path.GetFileName(strChyohyoExcelFilePath)

                    System.IO.Directory.CreateDirectory(tmpFolderPath)
                    System.IO.File.Copy(TemplateFolder & strExcelTemplateFileName, tmpFolderPath & koban, True)

                    Dim SheetName1 As String = Path.GetFileNameWithoutExtension(strExcelTemplateFileName)
                    SetMessageToExcel(tmpFolderPath & koban, lstKihonInfo, lstSunpoSet, SunpoSetCellL, SheetName1)

                    btnRegist_Click()

                    Dim pExcel As System.Diagnostics.Process = System.Diagnostics.Process.Start(tmpFolderPath & koban)
                    pExcel.WaitForExit()
                    'MsgBox("Del Sta")
                    'System.IO.Directory.Delete(tmpFolderPath, True)
                    Application.UseWaitCursor = Not Application.UseWaitCursor
                    Dim di As New System.IO.DirectoryInfo(tmpFolderPath)
                    di.Delete(True)
                    Application.UseWaitCursor = Not Application.UseWaitCursor
                    'MsgBox("Del End")

                    Exit Sub
                End If
            Else
                'ファイルが存在しなければテンプレートを登録先フォルダにコピー
                System.IO.File.Copy(TemplateFolder & strExcelTemplateFileName, strChyohyoExcelFilePath, False)
                btnRegist_Click()
            End If
        Catch ex As System.IO.IOException
            MsgBox("すでにエクセルが開かれています。")
            Exit Sub
        Catch ex As Exception
            MsgBox(ex.Message)
            Exit Sub
        End Try


        Dim SheetName As String = Path.GetFileNameWithoutExtension(strExcelTemplateFileName)

        SetMessageToExcel(strChyohyoExcelFilePath, lstKihonInfo, lstSunpoSet, SunpoSetCellL, SheetName)

        Try
            System.Diagnostics.Process.Start(strChyohyoExcelFilePath)
        Catch ex As Exception
            MsgBox("予期せぬエラーによりエクセルの起動に失敗しました。", MessageBoxIcon.Error)
        End Try


        'MsgBox("帳票印刷完了しました。")

    End Sub


    Public Sub SetMessageToExcel(ByVal strFilePath As String, ByVal KihonL As List(Of KihonInfoTable), ByVal SunpoSetL As List(Of SunpoSetTable), ByVal SunpoSetCellL As List(Of SunpoSetTable), ByVal SheetName As String)

        Dim xlApp As Excel.Application
        Dim xlBook As Excel.Workbook
        Dim xlSheet As Excel.Worksheet

        Dim strFilename As String                             'ファイル名(フルパス)
        strFilename = strFilePath                             'ファイル名をセット

        'Dim strSheetName As String                            'シート名
        'strSheetName = "STD"

        'strSheetName = 

        Dim xlBooks As Excel.Workbooks
        Dim xlSheets As Excel.Sheets

        Try

            GetExcelPID()
            xlApp = New Excel.Application
            GetExcelNewPID()
            xlBooks = xlApp.Workbooks
            xlBook = xlBooks.Open(strFilename)
            xlSheets = xlBook.Worksheets
            xlSheet = xlSheets.Item(SheetName)



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
                    '計測値加工データ
                    Try
                        Dim xlRangeExSVal As Excel.Range
                        xlRangeExSVal = xlSheet.Range(SunpoSetCellL(i).ExSunpoVal)
                        xlRangeExSVal.Value = SunpoSetL(indexSunposet).ExSunpoVal
                        MRComObject(xlRangeExSVal)
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
            'xlSheet.Activate()
            'xlSheet.PrintOut()


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

    End Sub

End Class