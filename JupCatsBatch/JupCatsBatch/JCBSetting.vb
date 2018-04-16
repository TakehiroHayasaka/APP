Imports System.Threading
Imports System.Globalization

Public Class JCBSetting
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


    Public Sub Load_ButtonSetting(sender As Object, e As EventArgs) Handles MyBase.Load
        '言語の設定
        Dim japanese As String = "ja-JP"
        Dim english As String = "en"
        Dim cultureString As String = Thread.CurrentThread.CurrentCulture.ToString
        If cultureString = japanese Then
            Me.RadioButtonJapanese.Checked = True
            Me.RadioButtonEnglish.Checked = False
        Else
            Me.RadioButtonJapanese.Checked = False
            Me.RadioButtonEnglish.Checked = True
        End If

        _ijcadExeName = FunGetAcad()
        _excelExeName = FunGetExcel()
        _accessExeName = FunGetAccess()
        Dim editorExeTmp As String = FunGetAppNo(0)
        fun文字列抽出(editorExeTmp, "[", "]", _editorExeName)

        Me.TextBoxIjcadExe.Text = _ijcadExeName
        Me.TextBoxExcelExe.Text = _excelExeName
        Me.TextBoxAccessExe.Text = _accessExeName
        Me.TextBoxEditorExe.Text = _editorExeName

        _isOkCansel = 0

    End Sub

    Private Sub OnInit_ButtonSettingOk(sender As Object, e As EventArgs) Handles ButtonSettingOk.Click

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


        '言語の設定
        Dim culture As String = ""
        If RadioButtonJapanese.Checked = True Then
            culture = "ja-JP"
        Else
            culture = "en"
        End If
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture)
        Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture

        _isOkCansel = 0
        Me.Close()

    End Sub

    Private Sub OnInit_ButtonSettingCansel(sender As Object, e As EventArgs) Handles ButtonSettingCansel.Click

        _isOkCansel = 1
        Me.Close()

    End Sub

    Private Sub OnInit_ButtonIjcadExe(sender As Object, e As EventArgs) Handles ButtonIjcadExe.Click

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

    Private Sub OnInit_ButtonExcelExe(sender As Object, e As EventArgs) Handles ButtonExcelExe.Click

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

    Private Sub OnInit_ButtonAccessExe(sender As Object, e As EventArgs) Handles ButtonAccessExe.Click

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

    Private Sub OnInit_ButtonEditorExe(sender As Object, e As EventArgs) Handles ButtonEditorExe.Click

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

    Private Sub RadioButtonJapanese_Click(sender As Object, e As EventArgs) Handles RadioButtonJapanese.Click
        RadioButtonJapanese.Checked = True
        RadioButtonEnglish.Checked = False
    End Sub

    Private Sub RadioButtonEnglish_Click(sender As Object, e As EventArgs) Handles RadioButtonEnglish.Click
        RadioButtonJapanese.Checked = False
        RadioButtonEnglish.Checked = True
    End Sub
End Class