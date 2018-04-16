Imports System.Windows.Forms
Imports System.IO
Imports System.Text

Public Class JCBEditConstruction
    Private _kojiFolder As String = ""
    Private _kogo As String = ""
    Public _kojiName As String = ""
    Private _chargeCode As String = ""
    Public _kozo As String = ""
    Private _recno As Integer = 0
    Public _kozoNo As Integer = 0
    Public _isOkCansel As Integer = 0   '0:変更ボタンをクリック、1:キャンセルボタンをクリック

    Public Function OnInitialize(folder As String) As Integer

        _kojiFolder = folder
        OnInitialize = 0

        Dim profileFileName As String = folder.TrimEnd("\") + "\profile.ini"
        Dim constFileName As String = folder.TrimEnd("\") + "\const.inf"

        OnInitialize = 0
        ' Profile.iniを読み込み、橋梁形式とチャージコードを取得

        _kozoNo = ReadProfileIni(profileFileName, _kozo, _chargeCode, _recno)
        If _kozoNo = -1 Then
            MessageBox.Show("工号、工事名、チャージコード、橋梁形式が取得できませんでした。工事フォルダを開いているか確認してください。", "工事情報編集", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
            MessageBox.Show("工号、工事名、チャージコード、橋梁形式が取得できませんでした。工事フォルダを開いているか確認してください。", "工事情報編集", MessageBoxButtons.OK, MessageBoxIcon.Error)
            OnInitialize = 1
            Exit Function
        End If

        Me.TextBoxEditKogo.Text = _kogo
        Me.TextBoxEditKojiName.Text = _kojiName
        Me.TextBoxEditChargeCode.Text = _chargeCode
        Me.TextBoxEditKozo.Text = _kozo

    End Function

    Private Sub ButtonEdit_Click(sender As Object, e As EventArgs) Handles ButtonEdit.Click
        If Me.TextBoxEditKogo.Text = _kogo And Me.TextBoxEditKojiName.Text = _kojiName And Me.TextBoxEditChargeCode.Text = _chargeCode And Me.TextBoxEditKozo.Text = _kozo Then
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

        'チャージコードが入力されているかチェック
        Dim user As String = ""
        subGetUserName(user)
        If Me.TextBoxEditChargeCode.Text = "" Then
            If user <> "YBG" And user <> "NKK" Then
                MessageBox.Show("チャージコードを入力してください。", "工事情報編集", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            _chargeCode = "FREE-JUPITER"
        Else
            _chargeCode = Me.TextBoxEditChargeCode.Text
        End If

        _kozo = Me.TextBoxEditKozo.Text

        'チャージコードがCharge.iniに存在するかチェック
        Dim recno As Integer = -999
        If _chargeCode <> "FREE-JUPITER" Then
            Dim chargeKogo As String = ""
            Dim chargeKozo As String = ""
            recno = funチャージコードの有無新規工事(_chargeCode, chargeKogo, chargeKozo)
            If recno = -999 Then
                Dim msg As String = "チャージコード[" + _chargeCode + "]がCharge.iniに存在しないため、工事を作成できません。"
                MessageBox.Show(msg, "工事情報編集", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            If chargeKogo.CompareTo(_kogo) <> 0 Then
                Dim msg As String = "Charge.iniにてチャージコード[" + _chargeCode + "]の工号[" + chargeKogo.TrimEnd() + "]が入力の工号[" + _kogo + "]と異なります。"
                MessageBox.Show(msg, "工事情報編集", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            If chargeKozo.CompareTo(_kozo) <> 0 Then
                Dim msg As String = "Charge.iniにてチャージコード[" + _chargeCode + "]の構造形式[" + chargeKozo + "]が入力の構造形式[" _
                                    + _kozo + "]と異なります。"
                MessageBox.Show(msg, "工事情報編集", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
        End If

        '工事フォルダにconst.infを作成
        Dim constText As String = _kojiFolder.TrimEnd("\") + " " + _kogo + " " + _kojiName
        Dim constInf As String = _kojiFolder.TrimEnd("\") + "\const.inf"
        Dim constWriter As New StreamWriter(constInf, False, Encoding.GetEncoding("Shift_JIS"))
        constWriter.WriteLine(constText)
        constWriter.Close()

        '工事フォルダにprofile.iniを作成
        Dim profileIni As String = _kojiFolder.TrimEnd("\") + "\profile.ini"
        Dim kozo As String = "主桁断面=" + _kozo
        Dim chargeText As String = "チャージコード=" + _chargeCode
        Dim chargeRecord As String = "登録ファイルレコード位置=" + CStr(recno)
        Dim profileWriter As New StreamWriter(profileIni, False, Encoding.GetEncoding("Shift_JIS"))
        profileWriter.WriteLine("[橋梁緒元]")
        profileWriter.WriteLine(kozo)
        profileWriter.WriteLine("[チャージコード]")
        profileWriter.WriteLine(chargeText)
        profileWriter.WriteLine(chargeRecord)
        profileWriter.Close()

        _isOkCansel = 0
        Me.Close()
    End Sub

    Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click

        _isOkCansel = 1
        Me.Close()
    End Sub

    Private Sub JCBEditConstruction_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class