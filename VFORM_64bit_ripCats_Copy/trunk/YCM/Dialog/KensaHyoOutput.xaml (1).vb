Imports System.IO
Public Class KensaHyoOutput
    Private m_folder As New FolderBrowserDialog             'フォルダ指定用
    Public m_TemplateExcelFile As String
    Public m_TemplatePath As String = System.Environment.CurrentDirectory & "\計測システムフォルダ\Template"
    Private mSet As String
    Public Property MeasurementSet As String
        Get
            Return mSet
        End Get
        Set(value As String)
            mSet = value
        End Set
    End Property

    Private Sub BtnExcelFolder_Click(sender As Object, e As Windows.RoutedEventArgs) Handles BtnExcelFolder.Click
        ''FolderBrowserDialogクラスのインスタンスを作成
        'Dim fbd As New FolderBrowserDialog

        'With fbd

        '    '上部に表示する説明テキストを指定する

        '    .Description = "フォルダを指定してください。"
        '    'ルートフォルダを指定する(デフォルトでDesktop)
        '    .RootFolder = Environment.SpecialFolder.Desktop
        '    '最初に選択するフォルダを指定する

        '    'RootFolder以下にあるフォルダである必要がある
        '    .SelectedPath = TxtExcelFolder.Text.Trim
        '    'ユーザーが新しいフォルダを作成できるようにする(デフォルトでTrue)
        '    .ShowNewFolderButton = True

        '    'ダイアログを表示する
        '    Dim iDialogResult As DialogResult
        '    iDialogResult = .ShowDialog(Me)
        '    If iDialogResult = DialogResult.OK Then
        '        '選択されたフォルダを表示する
        '        TxtExcelFolder.Text = .SelectedPath
        '    Else
        '        Exit Sub
        '    End If

        'End With

#If True Then 'SUURI ADD 20140915
        With m_folder
            'If Directory.Exists(TxtExcelFolder.Text) = True Then
            .SelectedPath = TxtExcelFolder.Text.Trim
            '.FileName = TxtOutFileName.Text

            'ダイアログを表示する
            Dim iDialogResult As System.Windows.Forms.DialogResult
            iDialogResult = .ShowDialog()
            If iDialogResult = System.Windows.Forms.DialogResult.OK Then
                '選択されたフォルダを表示する
                'TxtExcelFolder.Text = YCM_GetFolderPath_fromFullPath(.FileName)
                TxtExcelFolder.Text = .SelectedPath
            Else
                Exit Sub
            End If
            'Else
            'MsgBox("出力先フォルダが存在しません。")
            'Exit Sub
            'End If

        End With
#Else
         With m_fbd
            '上部に表示する説明テキストを指定する

            .Description = "フォルダを指定してください。"
            '最初に選択するフォルダを指定する

            'RootFolder以下にあるフォルダである必要がある
            .SelectedPath = TxtExcelFolder.Text.Trim

            'ダイアログを表示する
            Dim iDialogResult As System.Windows.Forms.DialogResult
            iDialogResult = .ShowDialog()
            If iDialogResult = System.Windows.Forms.DialogResult.OK Then
                '選択されたフォルダを表示する
                TxtExcelFolder.Text = .SelectedPath
            Else
                Exit Sub
            End If

        End With
#End If
    End Sub

    Private Sub BtnExcelOut_Click(sender As Object, e As Windows.RoutedEventArgs) Handles BtnExcelOut.Click
        '検査表の出力

        If SetKensaOut() = False Then
            Exit Sub
        End If

        '検査表出力処理フラグを更新する
        WorksD.SetOutFlg(False)

        'MsgBox("検査表を出力しました。", MsgBoxStyle.OkOnly)
        Me.Close() ' 20161116 baluu add
    End Sub

    Private Sub KensaHyoOutput_Loaded(sender As Object, e As Windows.RoutedEventArgs) Handles Me.Loaded
        ''寸法確認処理フラグを更新する
        'WorksD.SetKakuninFlg(False)

        ''寸法確認データを保存する

        'kentouKihon.SaveSunpoData(False)

        ''寸法確認画面へ
        'kentouKihon.SetSunpouKakunin()
        ''KekkaKakuninKentou()

        ''任意計測用寸法確認画面構築

        'If m_flg_Sunpo = True Or m_flg_Zahyo = True Or m_flg_Zukei = True Then
        '    kentouKihon.SupoKakuninGridDraw()
        'End If

        If m_flg_Senyou = True Then
            SetOutTemplate()
        End If
    End Sub

    

    Private Sub BtnT7Back_Click(sender As Object, e As Windows.RoutedEventArgs) Handles BtnT7Back.Click
        Me.Close()
    End Sub
    Public Function SetKensaOut() As Boolean

        SetKensaOut = True

        '出力先のフォルダ存在チェック
        If System.IO.Directory.Exists(TxtExcelFolder.Text) = False Then
            SetKensaOut = False
            MsgBox("出力先フォルダが存在しません。")
            Exit Function
        End If

        If CommonTypeID = 24 Then
            Sunpo_CsvOut(TxtExcelFolder.Text.Trim, TxtOutFileName.Text.Trim)
            Exit Function
        End If

        'テンプレートの入力チェック
        If CmbExcelTemplate.Text.Trim = "" Then
            SetKensaOut = False
            MsgBox("テンプレートを選択して下さい。")
            Exit Function
        End If

        m_TemplateExcelFile = WorksD.ExcelTemplateL(0).TemplateFileName & ".xls"

        Try
            'テンプレートを出力先にコピーする
            System.IO.File.Copy(m_TemplatePath & "\" & m_TemplateExcelFile,
                                TxtExcelFolder.Text & "\" & TxtOutFileName.Text & ".xls", True)

        Catch ex As System.IO.IOException
            '既に開かれている可能性あり
            SetKensaOut = False
            Exit Function

        Catch ex As Exception
            SetKensaOut = False
            Exit Function
        End Try

        ''出力Excelファイル名の存在チェックを行う
        'If System.IO.File.Exists(TxtExcelFolder.Text & "\" & TxtOutFileName.Text & ".xls") = True Then
        '    SetKensaOut = True
        '    Exit Function
        'End If

        ''テンプレートを出力先にコピーする
        'System.IO.File.Copy(m_TemplatePath & "\" & m_TemplateExcelFile,
        '                    TxtExcelFolder.Text & "\" & TxtOutFileName.Text & ".xls", True)

        '各項目をExcelに設定する

        SetMessageToExcel(TxtExcelFolder.Text & "\" & TxtOutFileName.Text & ".xls")

    End Function
    Public Sub SetOutTemplate()

        With CmbExcelTemplate

            .Items.Clear()

            For i As Integer = 0 To WorksD.ExcelTemplateL.Count - 1

                .Items.Add(WorksD.ExcelTemplateL(i).TemplateFileName)
                If i = 0 Then
                    .SelectedItem = WorksD.ExcelTemplateL(i).TemplateFileName
                End If

            Next

        End With


    End Sub
    Public Sub Sunpo_CsvOut(ByVal path As String, ByVal fname As String)
        Dim i As Integer
        Dim syasyu As String = ""
        Dim maker As String = ""
        Dim WrtStr As String = ""
        Dim WrtHdr As String = ""
        Dim strA As String = ""
        Dim strB As String = ""
        Dim strC As String = ""
        Dim strD As String = ""
        Dim strE As String = ""
        Dim strH As String = ""
        Dim Fname_Out As String = ""

        'KihonInfoから出力値主得
        For i = 0 To WorksD.KihonL.Count - 1
            If WorksD.KihonL(i).item_name.Trim = "計測箇所名" Then
                syasyu = WorksD.KihonL(i).item_value
            End If
            If WorksD.KihonL(i).item_name.Trim = "計測者" Then
                maker = WorksD.KihonL(i).item_value
            End If
        Next i

        'SunpoSetから出力値取得
        For i = 0 To WorksD.SunpoSetL.Count - 1
            If WorksD.SunpoSetL(i).SunpoMark.Trim = "L2" Then
                strH = WorksD.SunpoSetL(i).SunpoVal.Trim
            ElseIf WorksD.SunpoSetL(i).SunpoMark.Trim = "L3" Then
                strD = WorksD.SunpoSetL(i).SunpoVal.Trim
            ElseIf WorksD.SunpoSetL(i).SunpoMark.Trim = "L4" Then
                strE = WorksD.SunpoSetL(i).SunpoVal.Trim
            ElseIf WorksD.SunpoSetL(i).SunpoMark.Trim = "L5" Then
                strB = WorksD.SunpoSetL(i).SunpoVal.Trim
            ElseIf WorksD.SunpoSetL(i).SunpoMark.Trim = "L6" Then
                strC = WorksD.SunpoSetL(i).SunpoVal.Trim
            ElseIf WorksD.SunpoSetL(i).SunpoMark.Trim = "L7" Then
                strA = WorksD.SunpoSetL(i).SunpoVal.Trim
            End If
        Next i

        '出力イメージ作成
        WrtHdr = "車種名,メーカーID,,全幅,幅(⑤),高さ(⑥),前面高さ(ハーフ)(③),,,,,,,後部高さ(ハーフ)(④),,,タイヤから後部(②),"
        WrtStr = syasyu.Trim & "," & maker & ",NULL," & _
                 strA & "," & strB & "," & strC & "," & _
                 strD & ",null,null,null,0,null,null," & _
                 strE & ",null,null," & strH & ",null"

        'CSVファイルに書き込む
        Fname_Out = path & "\" & fname & ".csv"
        Try
            Dim sw As New System.IO.StreamWriter(Fname_Out, False, System.Text.Encoding.UTF8)
            sw.WriteLine(WrtHdr)
            sw.WriteLine(WrtStr)
            sw.Close()
        Catch e As Exception
            MsgBox("CSVファイルの出力に失敗しました。", MsgBoxStyle.Exclamation, "CSVファイル出力")
            Exit Sub
        End Try

        MsgBox("CSVファイルの出力に成功しました。", MsgBoxStyle.Information, "CSVファイル出力")

    End Sub
    Public Sub SetMessageToExcel(ByVal strFilePath As String)

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

            For i As Integer = 0 To WorksD.KihonL.Count - 1
                Try
                    Dim xlRangeItem As Excel.Range
                    xlRangeItem = xlSheet.Range(WorksD.KihonL(i).item_cell_name)
                    xlRangeItem.Value = WorksD.KihonL(i).item_value
                    MRComObject(xlRangeItem)
                Catch ex As Exception
                End Try
            Next

            For i As Integer = 0 To WorksD.SunpoSetCellL.Count - 1
                Dim indexSunposet As Integer = -1

                Dim t As Integer = 0
                Dim intSunpoId As Integer = WorksD.SunpoSetCellL(i).SunpoID

                For Each SSL As SunpoSetTable In WorksD.SunpoSetL

                    If SSL.SunpoID = intSunpoId Then
                        If MeasurementSet = SSL.MeasurementSet Or MeasurementSet = "" Or SSL.MeasurementSet = "" Then
                            indexSunposet = t
                        End If
                    End If
                    t += 1
                Next
                If WorksD.SunpoSetCellL(i).SunpoMark = "g" Then
                    Dim s As Integer = -1
                End If
                If WorksD.SunpoSetCellL(i).CT_Active = "1" Then

                    '規定値
                    Try
                        Dim xlRangeVal As Excel.Range
                        xlRangeVal = xlSheet.Range(WorksD.SunpoSetCellL(i).KiteiVal)
                        xlRangeVal.Value = WorksD.SunpoSetL(indexSunposet).KiteiVal
                        MRComObject(xlRangeVal)
                    Catch ex As Exception
                    End Try
                    '最小許容値
                    Try
                        Dim xlRangeMin As Excel.Range
                        xlRangeMin = xlSheet.Range(WorksD.SunpoSetCellL(i).KiteiMin)
                        xlRangeMin.Value = WorksD.SunpoSetL(indexSunposet).KiteiMin
                        MRComObject(xlRangeMin)
                    Catch ex As Exception
                    End Try
                    '最大許容値
                    Try
                        Dim xlRangeMax As Excel.Range
                        xlRangeMax = xlSheet.Range(WorksD.SunpoSetCellL(i).KiteiMax)
                        xlRangeMax.Value = WorksD.SunpoSetL(indexSunposet).KiteiMax
                        MRComObject(xlRangeMax)
                    Catch ex As Exception
                    End Try
                    '計測値
                    Try
                        Dim xlRangeSVal As Excel.Range
                        xlRangeSVal = xlSheet.Range(WorksD.SunpoSetCellL(i).SunpoVal)
                        xlRangeSVal.Value = WorksD.SunpoSetL(indexSunposet).SunpoVal
                        MRComObject(xlRangeSVal)
                    Catch ex As Exception
                    End Try
                    '合否
                    Try
                        Dim xlRangeGouhi As Excel.Range
                        xlRangeGouhi = xlSheet.Range(WorksD.SunpoSetCellL(i).flg_gouhi)
                        If WorksD.SunpoSetL(indexSunposet).flg_gouhi = "1" Then
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

        System.Diagnostics.Process.Start(strFilePath)

    End Sub

    '20161116 baluu add start
    Private Sub Window_Closing(sender As Object, e As System.ComponentModel.CancelEventArgs)
        e.Cancel = True
        Me.Visibility = Windows.Visibility.Hidden
    End Sub
    '20161116 baluu add end
End Class
