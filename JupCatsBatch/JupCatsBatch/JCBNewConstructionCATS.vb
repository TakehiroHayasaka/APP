Imports System.Windows.Forms
Imports System.IO
Imports System.Text

Public Class JCBNewConstructionCATS

    '工号
    Public _kogo As String = ""
    '工事名
    Public _kojiName As String = ""
    '工事フォルダ
    Public _kojiFolder As String = ""
    '構造
    Public _kozo As String = ""
    Public _kozoNo As Integer = -1
    'DSNファイル
    Public _dsnFile As String = ""

    ' OK:0、Cansel:1
    Public _isOkCansel As Integer = 1

    'Private Sub OnInit_JCBNewConstruction(sender As Object, e As EventArgs) Handles MyBase.Initialized
    'Public Sub OnInitialize(language As Integer, buttonColor As Integer, backColor As Integer)
    Public Sub Initialized()

        Me.ButtonDsn.Enabled = True
        Me.TextBoxDsn.Enabled = True
        Me.ComboBoxKozo.Items.Add("RC鈑桁")
        Me.ComboBoxKozo.Items.Add("RC箱桁")
        Me.ComboBoxKozo.Items.Add("鋼床版箱桁")

        Me.ComboBoxKozo.SelectedIndex = 0
    End Sub

    Private Sub OnClick_ButtonMakeFolder(sender As Object, e As EventArgs) Handles ButtonMakeFolder.Click

        '工号が入力されているかチェック
        _kogo = Me.TextBoxKogo.Text
        If _kogo = "" Then
            MessageBox.Show("工号を入力してください。", "新規工事登録", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        '工号に空白が含まれているかチェック
        If _kogo.Contains(" ") Or _kogo.Contains("　") Then
            MessageBox.Show("工号に空白は使用できません。", "新規工事登録", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        '工号に半角英数字以外が使用されているかチェック
        If DoubleByteChk(_kogo) <> 0 Then
            MessageBox.Show("工号には半角英数字以外は使用できません。", "新規工事登録", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        '工事名が入力されているかチェック
        _kojiName = Me.TextBoxKojiName.Text
        If _kojiName = "" Then
            MessageBox.Show("工事名を入力してください。", "新規工事登録", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        '工事名に空白が含まれているかチェック
        If _kojiName.Contains(" ") Or _kojiName.Contains("　") Then
            MessageBox.Show("工事名に空白は使用できません。", "新規工事登録", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        'DSNファイルが選択されているかチェック
        _dsnFile = Me.TextBoxDsn.Text
        If _dsnFile = "" Then
            MessageBox.Show("DSNファイルを選択してください。", "新規工事登録", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        'TODO:CATSの設定に合わせてファイルを作成する--------------------------------------------------------------------------------
        'フォルダを作成
        _kojiFolder = FunGetNewKoji(1) + Me.TextBoxKogo.Text
        If System.IO.Directory.Exists(_kojiFolder) Then
            MessageBox.Show("入力した工号の工事フォルダが既に存在します。工号に別の文字を入力してください。", "新規工事登録", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        System.IO.Directory.CreateDirectory(_kojiFolder)

        '工事フォルダにconst.infを作成
        Dim constText As String = _kojiFolder + " " + _kogo + " " + _kojiName
        Dim constInf As String = _kojiFolder + "\const.inf"
        Dim constWriter As New StreamWriter(constInf, False, Encoding.GetEncoding("Shift_JIS"))
        constWriter.WriteLine(constText)
        constWriter.Close()

        '工事フォルダにprofile.iniを作成
        Dim profileIni As String = _kojiFolder + "\profile.ini"
        Dim kozo As String = "主桁断面=" + Me.ComboBoxKozo.Text
        Dim profileWriter As New StreamWriter(profileIni, False, Encoding.GetEncoding("Shift_JIS"))
        profileWriter.WriteLine("[橋梁緒元]")
        profileWriter.WriteLine(kozo)
        profileWriter.Close()

        _kozo = Me.ComboBoxKozo.Text
        _kozoNo = GetKozoNo(_kozo)

        'DSNファイルを工事フォルダへコピー
        Dim copyFileName As String = _dsnFile
        Dim renameFileName As String = ""
        If System.IO.File.Exists(copyFileName) = True Then
            renameFileName = _kojiFolder + "\" + IO.Path.GetFileName(copyFileName)
            System.IO.File.Copy(copyFileName, renameFileName)
        End If

        If System.IO.File.Exists(renameFileName) = False Then
            MessageBox.Show("工事フォルダの作成に失敗しました。", "新規工事登録", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        subSetActiveData(_kogo, 1)
        _isOkCansel = 0

        MessageBox.Show("工事フォルダを作成しました。", "新規工事登録", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Me.Close()
    End Sub

    Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click

        _isOkCansel = 1
        Me.Close()

    End Sub

    Private Sub ButtonDsn_Click(sender As Object, e As EventArgs) Handles ButtonDsn.Click
        'ファイル名とフォルダ名の取得
        Dim dsnName As String = ""
        Dim folderName As String = ""
        If Me.TextBoxDsn.Text <> "" Then
            dsnName = IO.Path.GetFileName(Me.TextBoxDsn.Text)
            folderName = IO.Path.GetDirectoryName(Me.TextBoxDsn.Text)
        End If

        'ofd.Filter = "dsnファイル(*.dsn)|*.dsn|すべてのファイル(*.*)|*.*"
        Dim ofd As New OpenFileDialog With {
            .Title = "DSNファイルを選択してください",
            .FileName = dsnName,
            .InitialDirectory = folderName,
            .Filter = "dsnファイル(*.dsn)|*.dsn"
        }

        If ofd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Me.TextBoxDsn.Text = ofd.FileName
        End If
    End Sub
End Class
