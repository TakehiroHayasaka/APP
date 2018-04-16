Imports System.Windows.Forms
Imports System.IO
Imports System.Text

Public Class JCBOpenConstruction
    '工号
    Public _kogo As String = ""
    '工事名
    Public _kojiName As String = ""
    '工事フォルダ
    Public _kojiFolder As String = ""
    '橋梁形式
    Public _kozo As String = ""
    Public _kozoNo As Integer = -1
    '工事リスト
    Private _kojiList As New List(Of String)

    Public _isOkCansel As Integer = 1

    ' 0：Jupiter  1：CATS
    Private _flagJC As Integer = 0


    'Private Sub OnInit_KojiOpen(sender As Object, e As EventArgs) Handles MyBase.Initialized
    Public Sub OnInitialize(flagJC As Integer)

        _flagJC = flagJC

        Me.ListBoxSelectKoji.Items.Clear()

        OnInitialize()


    End Sub

    Private Sub OnInitialize()
        'サブフォルダ一覧の取得
        Dim folder As String = FunGetNewKoji(_flagJC)

        Dim subFolders() As String = Directory.GetFileSystemEntries(folder)
        'Dim kojiList As New List(Of String)
        Dim i As Integer = 0
        For i = 0 To subFolders.Length - 1
            Dim constFileName As String = subFolders(i) + "\const.inf"
            If System.IO.File.Exists(constFileName) = True Then
                Dim constInf As New StreamReader(constFileName, Encoding.GetEncoding("Shift_JIS"))
                Dim lineText As String = constInf.ReadLine()
                constInf.Close()
                '工事フォルダからフォルダ名を取得する。(最後の\以降の文字列を取得する)
                Dim folderNameArray As String() = subFolders(i).Split("\")
                Dim folderName As String = folderNameArray(folderNameArray.Count() - 1)
                Dim koji As String = folderName + "  " + J_ChoiceString(lineText, 1) + "  " + J_ChoiceString(lineText, 2) + "  " + subFolders(i)
                _kojiList.Add(koji)
            End If
        Next

        _kojiList.Sort()

        Dim defaultKoji As String = FunGetKoji(0)
        Dim defaultIdx As Integer = 0
        Dim defaultKojiName As String = ""
        For i = 0 To _kojiList.Count - 1
            Dim koji As String = J_ChoiceString(_kojiList(i), 0) + "  工号 [ " + J_ChoiceString(_kojiList(i), 1) + " ]  工事名 『 " + J_ChoiceString(_kojiList(i), 2) + " 』"
            Me.ListBoxSelectKoji.Items.Add(koji)
            Dim kojiFolder As String = J_ChoiceString(_kojiList(i), 3) + "\"
            If defaultKoji = kojiFolder Then
                defaultIdx = i
                defaultKojiName = koji
            End If
        Next

        Me.ListBoxSelectKoji.SelectedIndex = defaultIdx
        Me.ListBoxSelectKoji.ScrollAlwaysVisible = False
    End Sub

    Private Function KojiOpen() As Integer

        Dim idx As Integer = Me.ListBoxSelectKoji.SelectedIndex

        If idx = -1 Then
            MessageBox.Show("工事を選択してください。", "工事を開く", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return 1
        End If
        If idx >= _kojiList.Count Then
            MessageBox.Show("システムエラー(原因不明)", "工事を開く", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return 1
        End If

        _kogo = J_ChoiceString(_kojiList(idx), 1)
        _kojiName = J_ChoiceString(_kojiList(idx), 2)
        _kojiFolder = J_ChoiceString(_kojiList(idx), 3)
        Dim kogo As String = J_ChoiceString(_kojiList(idx), 0)

        'profile.iniを読み込んで、橋梁形式を取得する。
        Dim profileFileName As String = _kojiFolder.TrimEnd("\") + "\profile.ini"
        Dim strTmp As String = ""
        Dim intTmp As Integer = 0
        _kozoNo = ReadProfileIni(profileFileName, _kozo, strTmp, intTmp)
        If _kozoNo = -1 Then
            Dim msg As String = "工事フォルダにProfile.iniが存在しないため、工事を開くことができません。"
            MessageBox.Show(msg, "工事を開く", MessageBoxButtons.OK, MessageBoxIcon.Error)
            _kogo = ""
            _kojiName = ""
            _kojiFolder = ""
            _kozo = ""
            _kozoNo = -1
            Return 1
        End If

        'charge.iniにチャージコードが登録されているか確認する。
        'Dim user As String = ""
        'subGetUserName(user)
        'If user <> "YBG" And user <> "NKK" Then
        '    Dim chargeCode As String = ""
        '    Dim status As Integer = funチャージコードの有無(_kogo, chargeCode)
        '    If status = -999 Then
        '        Dim msg As String = "チャージコード[" + chargeCode + "]がCharge.iniに存在しないため、工事を開くことができません。"
        '        MessageBox.Show(msg, "工事を開く", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '        _kogo = ""
        '        _kojiName = ""
        '        _kojiFolder = ""
        '        _kozo = ""
        '        _kozoNo = -1
        '        Return 1
        '    End If
        'End If

        _isOkCansel = 0

        Return 0

    End Function

    Private Sub ButtonKojiOpen_Click(sender As Object, e As EventArgs) Handles ButtonKojiOpen.Click
        If Me.KojiOpen() = 0 Then
            Me.Close()
        End If

    End Sub

    Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click
        _kogo = ""
        _kojiName = ""
        _kojiFolder = ""
        _kojiList.Clear()

        _isOkCansel = 1

        Me.Close()

    End Sub

    Private Sub OnDoubleClick_SelectKoji(sender As Object, e As EventArgs) Handles ListBoxSelectKoji.MouseDoubleClick

        If Me.KojiOpen() = 0 Then
            Me.Close()
        End If

    End Sub


End Class
