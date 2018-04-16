Imports System.Windows.Forms
Imports System.IO
Imports System.Text

Public Class JCBEditConstructionCATS
    Private _kojiFolder As String = ""
    Private _kogo As String = ""
    Public _kojiName As String = ""
    Public _kozo As String = ""
    Private _recno As Integer = 0
    Public _kozoNo As Integer = 0
    Public _isOkCansel As Integer = 0   '0:変更ボタンをクリック、1:キャンセルボタンをクリック
    ' 0：Jupiter  1：CATS
    Private _flagJC As Integer = 0

    Public Function OnInitialize(folder As String) As Integer

        _kojiFolder = folder
        OnInitialize = 0

        Dim profileFileName As String = folder.TrimEnd("\") + "\profile.ini"
        Dim constFileName As String = folder.TrimEnd("\") + "\const.inf"

        OnInitialize = 0
        ' Profile.iniを読み込み、橋梁形式とチャージコードを取得

        _kozoNo = ReadProfileIni(profileFileName, _kozo, "FREE-JUPITER", _recno)
        If _kozoNo = -1 Then
            MessageBox.Show("工号、工事名、橋梁形式が取得できませんでした。工事フォルダを開いているか確認してください。", "工事情報編集", MessageBoxButtons.OK, MessageBoxIcon.Error)
            OnInitialize = 1
            Exit Function
        End If

        ' const.infを読み込み、工号と工事名を取得
        If System.IO.File.Exists(constFileName) = True Then
            Dim constIni As New StreamReader(constFileName, Encoding.GetEncoding("Shift_JIS"))
            Dim lineText As String = constIni.ReadLine().Trim()
            constIni.Close()
            _kogo = J_ChoiceString(lineText, 1)
            _kojiName = J_ChoiceString(lineText, 2)
        Else
            MessageBox.Show("工号、工事名、橋梁形式が取得できませんでした。工事フォルダを開いているか確認してください。", "工事情報編集", MessageBoxButtons.OK, MessageBoxIcon.Error)
            OnInitialize = 1
            Exit Function
        End If

        Me.TextBoxEditKogo.Text = _kogo
        Me.TextBoxEditKojiName.Text = _kojiName
        Me.TextBoxEditKozo.Text = _kozo

    End Function

    Private Sub ButtonEdit_Click(sender As Object, e As EventArgs) Handles ButtonEdit.Click
        If Me.TextBoxEditKogo.Text = _kogo And Me.TextBoxEditKojiName.Text = _kojiName And Me.TextBoxEditKozo.Text = _kozo Then
            Me.Close()
        End If

        _isOkCansel = 1

        '工号が入力されているかチェック
        _kogo = Me.TextBoxEditKogo.Text
        If _kogo = "" Then
            MessageBox.Show("工号を入力してください。", "工事情報編集", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        '工号に空白が含まれているかチェック
        If _kogo.Contains(" ") Or _kogo.Contains("　") Then
            MessageBox.Show("工号に空白は使用できません。", "工事情報編集", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        '工号に半角英数字以外が使用されているかチェック
        If DoubleByteChk(_kogo) <> 0 Then
            MessageBox.Show("工号には半角英数字以外は使用できません。", "工事情報編集", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        '工事名が入力されているかチェック
        _kojiName = Me.TextBoxEditKojiName.Text
        If _kojiName = "" Then
            MessageBox.Show("工事名を入力してください。", "工事情報編集", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        '工事名に空白が含まれているかチェック
        If _kojiName.Contains(" ") Or _kojiName.Contains("　") Then
            MessageBox.Show("工事名に空白は使用できません。", "工事情報編集", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        _kozo = Me.TextBoxEditKozo.Text

        '工事フォルダにconst.infを作成
        Dim constText As String = _kojiFolder.TrimEnd("\") + " " + _kogo + " " + _kojiName
        Dim constInf As String = _kojiFolder.TrimEnd("\") + "\const.inf"
        Dim constWriter As New StreamWriter(constInf, False, Encoding.GetEncoding("Shift_JIS"))
        constWriter.WriteLine(constText)
        constWriter.Close()

        '工事フォルダにprofile.iniを作成
        Dim profileIni As String = _kojiFolder.TrimEnd("\") + "\profile.ini"
        Dim kozo As String = "主桁断面=" + _kozo
        Dim profileWriter As New StreamWriter(profileIni, False, Encoding.GetEncoding("Shift_JIS"))
        profileWriter.WriteLine("[橋梁緒元]")
        profileWriter.WriteLine(kozo)
        profileWriter.Close()

        _isOkCansel = 0
        Me.Close()
    End Sub

    Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click

        _isOkCansel = 1
        Me.Close()
    End Sub

End Class