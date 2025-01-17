﻿Public Class FrmReg

    Private Sub FrmReg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim strNewKoujiFolder As String = Me.Tag.ToString

        Dim clsKihonInfo As New KihonInfoTable
        Dim lstKihonInfo As New List(Of KihonInfoTable)
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
                objVal1(1) = dt.ToString("yyyy/MM/dd")
            Else
                objVal1(1) = Sokutei.item_value
            End If

            Me.dgKihonInfo.Rows.Add(objVal1)
        Next
        clsKihonInfo.m_dbClass.DisConnect()

        Dim clsSunpoSet As New SunpoSetTable
        Dim lstSunpoSet As New List(Of SunpoSetTable)

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
                    objVal2(1) = FrmMain.dgMeasureResult.Item(1, 10).Value
                    objVal2(2) = ""
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

End Class