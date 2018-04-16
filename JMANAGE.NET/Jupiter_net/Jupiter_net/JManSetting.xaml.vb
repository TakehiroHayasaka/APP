Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Windows.Forms

Public Class JManSetting

    'プロパティ
    'IJCADファイルパス
    Public _ijcadExeName As String = ""
    'EXCELファイルパス
    Public _excelExeName As String = ""
    'ACCESSファイルパス
    Public _accessExeName As String = ""
    'EDITORファイルパス
    Public _editorExeName As String = ""

    ' 言語設定(0=Japanese,1=English)
    Public _language As Integer = 0
    ' ボタン色設定(0=Gray,1=Green)
    Public _buttonColor As Integer = 0
    ' 背景色設定(0=Gray,1=White)
    Public _backColor As Integer = 1

    ' OK:0、Cansel:1
    Public _isOkCansel As Integer = 0

    'DataGrid表示用
    Private _jPmfIni As New JManPmfBmfIni()
    Private _jBmfIni As New JManPmfBmfIni()
    Private _gridCollectionPmf As New JManGridCollection2()
    Private _gridCollectionBmf As New JManGridCollection2()

    Private Sub dispDataGrid(type As String)

        Dim iniFile As New JManPmfBmfIni()

        iniFile._fileHeader = type

        Dim status As Integer = iniFile.load()
        If status = 0 Then
            'DataGridにCNTファイルの内容を表示する。
            Dim i As Integer = 0
            For i = 0 To iniFile._recordList.Count - 1
                Dim iniData As New JManGridData2()
                iniData.Value1 = iniFile._recordList(i)._fileName
                iniData.Value2 = iniFile._recordList(i)._dispName
                If type = "JPMF" Then
                    _gridCollectionPmf.Add(iniData)
                Else
                    _gridCollectionBmf.Add(iniData)
                End If
            Next
        Else
            Dim iniData As New JManGridData2()
            iniData.Value1 = ""
            iniData.Value2 = ""
            If type = "JPMF" Then
                _gridCollectionPmf.Add(iniData)
            Else
                _gridCollectionBmf.Add(iniData)
            End If
        End If

        If type = "JPMF" Then
            Me.DatGridPmf.DataContext = _gridCollectionPmf
            Me._jPmfIni = iniFile
        Else
            Me.DatGridBmf.DataContext = _gridCollectionBmf
            Me._jBmfIni = iniFile
        End If

    End Sub

    Public Sub OnInitialize(language As Integer, buttonColor As Integer, backColor As Integer)

        _language = language
        _buttonColor = buttonColor
        _backColor = backColor

        _ijcadExeName = FunGetAcad()
        _excelExeName = FunGetExcel()
        _accessExeName = FunGetAccess()
        Dim editorExeTmp As String = FunGetAppNo(0)
        fun文字列抽出(editorExeTmp, "[", "]", _editorExeName)

        Me.TextBoxIjcadExe.Text = _ijcadExeName
        Me.TextBoxExcelExe.Text = _excelExeName
        Me.TextBoxAccessExe.Text = _accessExeName
        Me.TextBoxEditorExe.Text = _editorExeName

        Me.changeRadioButton()

        'JPmf.iniを読み込んで、DataGridに表示
        Me.dispDataGrid("JPMF")
        'JBmf.iniを読み込んで、DataGridに表示
        Me.dispDataGrid("JBMF")

        _isOkCansel = 0

        '背景色とボタン色を変更する。
        Me.GroupBoxBackColor.Visibility = True
        'Me.changeBackColor()
        Me.changeButtonColor()

    End Sub

    Private Sub changeBackColor()

        If _backColor = 0 Then
            Me.Background = New SolidColorBrush(System.Windows.Media.Colors.DarkGray)
        Else
            Me.Background = New SolidColorBrush(System.Windows.Media.Colors.White)
        End If

    End Sub

    Private Sub changeButtonColor()

        If _buttonColor = 0 Then
            Me.Background = New SolidColorBrush(System.Windows.Media.Colors.DarkGray)

            Me.ButtonSettingOk.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 50, 50, 50))
            Me.ButtonSettingCansel.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 50, 50, 50))
            Me.ButtonIjcadExe.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 50, 50, 50))
            Me.ButtonExcelExe.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 50, 50, 50))
            Me.ButtonAccessExe.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 50, 50, 50))
            Me.ButtonEditorExe.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 50, 50, 50))
        Else
            Me.Background = New SolidColorBrush(System.Windows.Media.Colors.White)

            Me.ButtonSettingOk.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 164, 150))
            Me.ButtonSettingCansel.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 164, 150))
            Me.ButtonIjcadExe.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 164, 150))
            Me.ButtonExcelExe.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 164, 150))
            Me.ButtonAccessExe.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 164, 150))
            Me.ButtonEditorExe.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 164, 150))
        End If

    End Sub

    Public Sub changeRadioButton()

        If _language = 0 Then
            Me.RadioLangJpn.IsChecked = True
            Me.RadioLangEng.IsChecked = False
        Else
            Me.RadioLangJpn.IsChecked = False
            Me.RadioLangEng.IsChecked = True
        End If

        If _buttonColor = 0 Then
            Me.RadioButColGray.IsChecked = True
            Me.RadioButColGreen.IsChecked = False
        Else
            Me.RadioButColGray.IsChecked = False
            Me.RadioButColGreen.IsChecked = True
        End If

        If _backColor = 0 Then
            Me.RadioBakColGray.IsChecked = True
            Me.RadioBakColWhite.IsChecked = False
        Else
            Me.RadioBakColGray.IsChecked = False
            Me.RadioBakColWhite.IsChecked = True
        End If

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    ' 　DataFridの内容をJPmf.ini,JBmf.iniに保存する。
    '引数
    '   type：(I) JPMF or JBMF
    '戻り値
    '   0：正常終了
    '   1：DataGrid内のどちらかの列が空白
    '   2：DataGrid内のファイル名の拡張子が正しくない
    '   3：DataGrid内にファイル名が重複しているデータが存在する。
    '---------------------------------------------------------------------------------------------------
    Private Function saveIniFile(type As String) As Integer

        saveIniFile = 0

        Dim dataGrid As New System.Windows.Controls.DataGrid()
        Dim jmanPmfBmf As New JManPmfBmfIni()
        Dim errMes As String = ""
        If type = "JPMF" Then
            dataGrid = Me.DatGridPmf
            jmanPmfBmf = Me._jPmfIni
            errMes = "PMFファイル名の設定で"
        Else
            dataGrid = Me.DatGridBmf
            jmanPmfBmf = Me._jBmfIni
            errMes = "BMFファイル名の設定で"
        End If

        'DataGridから情報を取得して、JManPmfBmfIniを作成する。
        Dim jmanPmfBmfNew As New JManPmfBmfIni()
        jmanPmfBmfNew._fileHeader = jmanPmfBmf._fileHeader
        Dim i As Integer = 0
        For i = 0 To dataGrid.Items.Count - 1
            Dim fileName As String = ""
            Try
                fileName = dataGrid.Items.Item(i).Value1.Trim()
            Catch ex As Exception
                fileName = ""
            End Try
            Dim dispName As String = ""
            Try
                dispName = dataGrid.Items.Item(i).Value2.Trim()
            Catch ex As Exception
                dispName = ""
            End Try

            If fileName = "" And dispName = "" Then             'ファイル名と表示名が共に空白の場合は、次の行へ
                Continue For
            ElseIf fileName = "" Then                           'ファイル名のみ空白の場合は、エラー
                errMes += (i + 1).ToString() + "行目のファイル名が空白です。ファイル名を入力してください。"
                MessageBox.Show(errMes, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
                saveIniFile = 1
                Exit Function
            ElseIf dispName = "" Then                           '表示名のみ空白の場合は、エラー
                errMes += (i + 1).ToString() + "行目の表示名が空白です。表示名を入力してください。"
                MessageBox.Show(errMes, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
                saveIniFile = 1
                Exit Function
            End If

            'ファイル名の拡張子が正しくない
            Dim ext As String = "J" + subStringEnd(fileName, ".").ToUpper()
            If type <> ext Then
                errMes += (i + 1).ToString() + "行目のファイル名の拡張子が正しくありません。"
                MessageBox.Show(errMes, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
                saveIniFile = 2
                Exit Function
            End If

            Dim record As New JManPmfBmfIniRecord()
            record._fileName = fileName
            record._dispName = dispName
            jmanPmfBmfNew._recordList.Add(record)
        Next

        'DataGridのファイル名が重複していないかを確認する。
        Dim row1 As Integer = 0
        Dim row2 As Integer = 0
        If jmanPmfBmfNew.isOverlap(row1, row2) = True Then
            errMes += row1.ToString() + "行目と" + row2.ToString() + "行目のファイル名に同じファイル名が設定されています。"
            MessageBox.Show(errMes, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            saveIniFile = 3
            Exit Function
        End If

        'DataGridの内容が更新されていなければファイルを保存しない。
        If jmanPmfBmf.isEqual(jmanPmfBmfNew) = True Then
            saveIniFile = 0
            Exit Function
        End If

        'ファイルを保存する。
        If jmanPmfBmfNew.save() <> 0 Then
            errMes = "システムエラー" + vbCrLf + type + ".iniの保存に失敗しました。"
            MessageBox.Show(errMes, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            saveIniFile = 1
        End If

    End Function

    Private Sub OnInit_ButtonSettingOk(sender As Object, e As RoutedEventArgs) Handles ButtonSettingOk.Click

        'DataGridの内容をJPmf.ini,JBmf.iniに保存する。
        If Me.saveIniFile("JPMF") <> 0 Then
            Return
        End If
        If Me.saveIniFile("JBMF") <> 0 Then
            Return
        End If

        'Jmanage.iniにアプリケーションパスを書き込み
        If _ijcadExeName <> Me.TextBoxIjcadExe.Text Then
            _ijcadExeName = Me.TextBoxIjcadExe.Text
            subJmIniSetApp("ACAD", _ijcadExeName)
        End If
        If _excelExeName <> Me.TextBoxExcelExe.Text Then
            _excelExeName = Me.TextBoxExcelExe.Text
            subJmIniSetApp("EXCEL", _excelExeName)
        End If
        If _accessExeName <> Me.TextBoxAccessExe.Text Then
            _accessExeName = Me.TextBoxAccessExe.Text
            subJmIniSetApp("ACCESS", _accessExeName)
        End If
        If _editorExeName <> Me.TextBoxEditorExe.Text Then
            _editorExeName = Me.TextBoxEditorExe.Text
            Dim editorDef As String = "EDITOR: [" + _editorExeName + "]"
            subJmIniSetAppNo(0, editorDef)
        End If

        'Jmanage.iniに言語を書き込み
        Dim lang As String
        If Me.RadioLangJpn.IsChecked = True Then
            lang = "JPN"
            _language = 0
        Else
            lang = "ENG"
            _language = 1
        End If
        subJmIniSetSection("Language", "Language", lang)

        'Jmanage.iniにボタン色を書き込み
        Dim buttonColor As String
        If Me.RadioButColGray.IsChecked = True Then
            buttonColor = "GRAY"
            _buttonColor = 0
        Else
            buttonColor = "GREEN"
            _buttonColor = 1
        End If
        subJmIniSetSection("Color", "ButtonColor", buttonColor)

        'Jmanage.iniに背景色を書き込み
        Dim backColor As String
        If Me.RadioBakColGray.IsChecked = True Then
            backColor = "GRAY"
            _backColor = 0
        Else
            backColor = "WHITE"
            _backColor = 1
        End If
        subJmIniSetSection("Color", "BackColor", backColor)

        _isOkCansel = 0
        Me.Close()

    End Sub

    Private Sub OnInit_ButtonSettingCansel(sender As Object, e As RoutedEventArgs) Handles ButtonSettingCansel.Click

        _isOkCansel = 1
        Me.Close()

    End Sub

    Private Sub OnInit_ButtonIjcadExe(sender As Object, e As RoutedEventArgs) Handles ButtonIjcadExe.Click

        'ファイル名とフォルダ名の取得
        Dim exeName As String = ""
        Dim folderName As String = ""
        If Me.TextBoxIjcadExe.Text <> "" Then
            exeName = IO.Path.GetFileName(Me.TextBoxIjcadExe.Text)
            folderName = IO.Path.GetDirectoryName(Me.TextBoxIjcadExe.Text)
        End If

        Dim ofd As New OpenFileDialog()

        ofd.Title = "CADのEXEファイルを選択してください"
        ofd.FileName = exeName
        ofd.InitialDirectory = folderName
        ofd.Filter = "exeファイル(*.exe)|*.exe|すべてのファイル(*.*)|*.*"

        If ofd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Me.TextBoxIjcadExe.Text = ofd.FileName
        End If

    End Sub

    Private Sub OnInit_ButtonExcelExe(sender As Object, e As RoutedEventArgs) Handles ButtonExcelExe.Click

        'ファイル名とフォルダ名の取得
        Dim exeName As String = ""
        Dim folderName As String = ""
        If Me.TextBoxExcelExe.Text <> "" Then
            exeName = IO.Path.GetFileName(Me.TextBoxExcelExe.Text)
            folderName = IO.Path.GetDirectoryName(Me.TextBoxExcelExe.Text)
        End If

        Dim ofd As New OpenFileDialog()

        ofd.Title = "ExcelのEXEファイルを選択してください"
        ofd.FileName = exeName
        ofd.InitialDirectory = folderName
        ofd.Filter = "exeファイル(*.exe)|*.exe|すべてのファイル(*.*)|*.*"

        If ofd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Me.TextBoxExcelExe.Text = ofd.FileName
        End If

    End Sub

    Private Sub OnInit_ButtonAccessExe(sender As Object, e As RoutedEventArgs) Handles ButtonAccessExe.Click

        'ファイル名とフォルダ名の取得
        Dim exeName As String = ""
        Dim folderName As String = ""
        If Me.TextBoxAccessExe.Text <> "" Then
            exeName = IO.Path.GetFileName(Me.TextBoxAccessExe.Text)
            folderName = IO.Path.GetDirectoryName(Me.TextBoxAccessExe.Text)
        End If

        Dim ofd As New OpenFileDialog()

        ofd.Title = "AccessのEXEファイルを選択してください"
        ofd.FileName = exeName
        ofd.InitialDirectory = folderName
        ofd.Filter = "exeファイル(*.exe)|*.exe|すべてのファイル(*.*)|*.*"

        If ofd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Me.TextBoxAccessExe.Text = ofd.FileName
        End If

    End Sub

    Private Sub OnInit_ButtonEditorExe(sender As Object, e As RoutedEventArgs) Handles ButtonEditorExe.Click

        'ファイル名とフォルダ名の取得
        Dim exeName As String = ""
        Dim folderName As String = ""
        If Me.TextBoxEditorExe.Text <> "" Then
            exeName = IO.Path.GetFileName(Me.TextBoxEditorExe.Text)
            folderName = IO.Path.GetDirectoryName(Me.TextBoxEditorExe.Text)
        End If

        Dim ofd As New OpenFileDialog()

        ofd.Title = "EditorのEXEファイルを選択してください"
        ofd.FileName = exeName
        ofd.InitialDirectory = folderName
        ofd.Filter = "exeファイル(*.exe)|*.exe|すべてのファイル(*.*)|*.*"

        If ofd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Me.TextBoxEditorExe.Text = ofd.FileName
        End If

    End Sub

    Private Sub OnClick_RadioButColGray(sender As Object, e As RoutedEventArgs) Handles RadioButColGray.Click

        Me.RadioButColGray.IsChecked = True
        Me.RadioButColGreen.IsChecked = False
        Me._buttonColor = 0

        Me.changeButtonColor()

    End Sub

    Private Sub OnClick_RadioButColGreen(sender As Object, e As RoutedEventArgs) Handles RadioButColGreen.Click

        Me.RadioButColGray.IsChecked = False
        Me.RadioButColGreen.IsChecked = True
        Me._buttonColor = 1

        Me.changeButtonColor()

    End Sub

    Private Sub OnClick_RadioBakColGray(sender As Object, e As RoutedEventArgs) Handles RadioBakColGray.Click

        Me.RadioBakColGray.IsChecked = True
        Me.RadioBakColWhite.IsChecked = False
        Me._backColor = 0

        Me.changeBackColor()

    End Sub

    Private Sub OnClick_RadioBakColWhite(sender As Object, e As RoutedEventArgs) Handles RadioBakColWhite.Click

        Me.RadioBakColGray.IsChecked = False
        Me.RadioBakColWhite.IsChecked = True
        Me._backColor = 1

        Me.changeBackColor()

    End Sub
End Class
