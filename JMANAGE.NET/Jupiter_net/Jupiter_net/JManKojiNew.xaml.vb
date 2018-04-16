﻿Imports System.Windows.Forms
Imports System.IO
Imports System.Text

Public Class JManKojiNew

    '工号
    Public _kogo As String = ""
    '工事名
    Public _kojiName As String = ""
    '工事フォルダ
    Public _kojiFolder As String = ""
    '橋梁形式
    Public _kozo As String = ""
    Public _kozoNo As Integer = -1

    ' OK:0、Cansel:1
    Public _isOkCansel As Integer = 1

    Private Sub readCopyFileLst(ByVal KozoIdx As Integer)

        Me.ListBoxCopyFile.Items.Clear()

        Dim kozo As String = ""
        If KozoIdx = 0 Or KozoIdx = 5 Then  'RC鈑桁または波形ウェブ
            kozo = "[RcI]"
        ElseIf KozoIdx = 1 Then             'RC箱桁
            kozo = "[RcB]"
        ElseIf KozoIdx = 2 Then             '鋼床版箱桁
            kozo = "[DkB]"
        ElseIf KozoIdx = 3 Then             '合成床版
            kozo = "[Gdk]"
        ElseIf KozoIdx = 4 Then             '鋼製橋脚
            kozo = "[Pier]"
        ElseIf KozoIdx = 6 Then             '鋼製セグメント
            kozo = "[Seg]"
        End If

        Dim copyFileName As String = FunGetGensun() + "Environ\CopyFile.lst"
        If System.IO.File.Exists(copyFileName) = True Then
            Dim copyFile As New StreamReader(copyFileName, Encoding.GetEncoding("Shift_JIS"))
            Dim isSeg As Boolean = False
            While (copyFile.Peek() >= 0)
                Dim lineText As String = copyFile.ReadLine().Trim()
                If lineText = "" Then
                    Continue While
                End If
                If lineText = kozo Then
                    isSeg = True
                    Continue While
                Else
                    If lineText.StartsWith("[") Then
                        isSeg = False
                    End If
                End If

                If isSeg = True Then
                    Dim fileName As String = J_ChoiceString(lineText, 0)
                    Me.ListBoxCopyFile.Items.Add(fileName)
                End If
            End While
            copyFile.Close()
        End If

    End Sub

    Private Sub readGetCopyFileLst(ByRef orgFileList As List(Of String), ByRef renameFileList As List(Of String))

        Dim copyFileName As String = FunGetGensun() + "Environ\CopyFile.lst"
        Dim KozoIdx As Integer = Me.ComboBoxKozo.SelectedIndex

        Dim kozo As String = ""
        If KozoIdx = 0 Or KozoIdx = 5 Then  'RC鈑桁または波形ウェブ
            kozo = "[RcI]"
        ElseIf KozoIdx = 1 Then             'RC箱桁
            kozo = "[RcB]"
        ElseIf KozoIdx = 2 Then             '鋼床版箱桁
            kozo = "[DkB]"
        ElseIf KozoIdx = 3 Then             '合成床版
            kozo = "[Gdk]"
        ElseIf KozoIdx = 4 Then             '鋼製橋脚
            kozo = "[Pier]"
        ElseIf KozoIdx = 6 Then             '鋼製セグメント
            kozo = "[Seg]"
        End If

        If System.IO.File.Exists(copyFileName) = True Then
            Dim copyFile As New StreamReader(copyFileName, Encoding.GetEncoding("Shift_JIS"))
            Dim isSeg As Boolean = False
            While (copyFile.Peek() >= 0)
                Dim lineText As String = copyFile.ReadLine().Trim()
                If lineText = "" Then
                    Continue While
                End If
                If lineText = kozo Then
                    isSeg = True
                    Continue While
                Else
                    If lineText.StartsWith("[") Then
                        isSeg = False
                    End If
                End If

                If isSeg = True Then
                    Dim orgName As String = J_ChoiceString(lineText, 0)
                    orgFileList.Add(orgName)
                    Dim renameName As String = ""
                    If J_NWord(lineText) >= 2 Then
                        renameName = J_ChoiceString(lineText, 1)
                    End If
                    renameFileList.Add(renameName)
                End If
            End While
            copyFile.Close()
        End If

    End Sub

    'Private Sub OnInit_JManKojiNew(sender As Object, e As EventArgs) Handles MyBase.Initialized
    Public Sub OnInitialize(language As Integer, buttonColor As Integer, backColor As Integer)

        '背景色とボタン色を変更する。
        If backColor = 0 Then
            Me.Background = New SolidColorBrush(System.Windows.Media.Colors.DarkGray)
        Else
            Me.Background = New SolidColorBrush(System.Windows.Media.Colors.White)
        End If
        If buttonColor = 0 Then
            Me.ButtonMakeFolder.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 50, 50, 50))
            Me.ButtonKojiNewCansel.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 50, 50, 50))
        Else
            Me.ButtonMakeFolder.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 164, 150))
            Me.ButtonKojiNewCansel.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 164, 150))
        End If

        'CopyFile.lstを開いてリストボックスに表示(SelectionChanged_ComboBoxKozoが自動で呼ばれるらしい)
        Me.ComboBoxKozo.SelectedIndex = 0

    End Sub

    Private Sub SelectionChanged_ComboBoxKozo(sender As Object, e As SelectionChangedEventArgs) Handles ComboBoxKozo.SelectionChanged

        'CopyFile.lstを開いてリストボックスに表示
        Dim KozoIdx As Integer = Me.ComboBoxKozo.SelectedIndex
        Me.readCopyFileLst(KozoIdx)

    End Sub

    Private Sub OnClick_ButtonMakeFolder(sender As Object, e As RoutedEventArgs) Handles ButtonMakeFolder.Click

        '工号が入力されているかチェック
        _kogo = Me.TextBoxKogo.Text
        If _kogo = "" Then
            MessageBox.Show("工号を入力してください。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        '工号に空白が含まれているかチェック
        If _kogo.Contains(" ") Or _kogo.Contains("　") Then
            MessageBox.Show("工号に空白は使用できません。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        '工号に半角英数字以外が使用されているかチェック
        If DoubleByteChk(_kogo) <> 0 Then
            MessageBox.Show("工号には半角英数字以外は使用できません。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        '工事名が入力されているかチェック
        _kojiName = Me.TextBoxKojiName.Text
        If _kojiName = "" Then
            MessageBox.Show("工事名を入力してください。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        '工事名に空白が含まれているかチェック
        If _kojiName.Contains(" ") Or _kojiName.Contains("　") Then
            MessageBox.Show("工事名に空白は使用できません。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        'チャージコードが入力されているかチェック
        Dim user As String = ""
        subGetUserName(user)
        Dim chargeCode As String = ""
        If Me.TextBoxCharge.Text = "" Then
            If user <> "YBG" And user <> "NKK" Then
                MessageBox.Show("チャージコードを入力してください。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            chargeCode = "FREE-JUPITER"
        Else
            chargeCode = Me.TextBoxCharge.Text
        End If

        'チャージコードがCharge.iniに存在するかチェック
        Dim recno As Integer = -999
        If chargeCode <> "FREE-JUPITER" Then
            Dim chargeKogo As String = ""
            Dim chargeKozo As String = ""
            recno = funチャージコードの有無新規工事(chargeCode, chargeKogo, chargeKozo)
            If recno = -999 Then
                Dim msg As String = "チャージコード[" + chargeCode + "]がCharge.iniに存在しないため、工事を作成できません。"
                MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            If chargeKogo.CompareTo(_kogo) <> 0 Then
                Dim msg As String = "Charge.iniにてチャージコード[" + chargeCode + "]の工号[" + chargeKogo.TrimEnd() + "]が入力の工号[" + _kogo + "]と異なります。"
                MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            If chargeKozo.CompareTo(Me.ComboBoxKozo.Text) <> 0 Then
                Dim msg As String = "Charge.iniにてチャージコード[" + chargeCode + "]の構造形式[" + chargeKozo + "]が入力の構造形式[" _
                                    + Me.ComboBoxKozo.Text + "]と異なります。"
                MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
        End If

        'フォルダを作成
        _kojiFolder = FunGetNewKoji() + Me.TextBoxKogo.Text
        If System.IO.Directory.Exists(_kojiFolder) Then
            MessageBox.Show("入力した工号の工事フォルダが既に存在します。工号に別の文字を入力してください。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
        Dim chargeText As String = "チャージコード=" + chargeCode
        Dim chargeRecord As String = "登録ファイルレコード位置=" + CStr(recno)
        Dim profileWriter As New StreamWriter(profileIni, False, Encoding.GetEncoding("Shift_JIS"))
        profileWriter.WriteLine("[橋梁緒元]")
        profileWriter.WriteLine(kozo)
        profileWriter.WriteLine("[チャージコード]")
        profileWriter.WriteLine(chargeText)
        profileWriter.WriteLine(chargeRecord)
        profileWriter.Close()
        _kozo = Me.ComboBoxKozo.Text
        _kozoNo = getKozoNo(_kozo)

        Dim orgFileList As New List(Of String)
        Dim renameFileList As New List(Of String)
        readGetCopyFileLst(orgFileList, renameFileList)

        If orgFileList.Count <> renameFileList.Count Then
            MessageBox.Show("工事フォルダの作成に失敗しました。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        'ファイルを工事フォルダへコピー
        Dim idx As Integer = 0
        For idx = 0 To Me.ListBoxCopyFile.Items.Count - 1
            Dim fileName As String = Me.ListBoxCopyFile.Items(idx)
            Dim findIdx As Integer = orgFileList.IndexOf(fileName)
            Dim copyFileName As String = FunGetGensun() + orgFileList(findIdx)
            If System.IO.File.Exists(copyFileName) = True Then
                Dim renameFileName As String = renameFileList(findIdx)
                If renameFileName = "" Then
                    renameFileName = _kojiFolder + "\" + IO.Path.GetFileName(orgFileList(findIdx))
                Else
                    renameFileName = _kojiFolder + "\" + IO.Path.GetFileName(renameFileList(findIdx))
                End If
                System.IO.File.Copy(copyFileName, renameFileName)
            End If
        Next

        subSetActiveData(_kogo)
        _isOkCansel = 0

        MessageBox.Show("工事フォルダを作成しました。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Me.Close()

    End Sub

    Private Sub OnClick_ButtonKojiNewCansel(sender As Object, e As RoutedEventArgs) Handles ButtonKojiNewCansel.Click

        _isOkCansel = 1
        Me.Close()

    End Sub
End Class