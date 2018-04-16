Imports System.Windows.Forms

Public Class JManDelBlk

    Private _kogo As String = ""
    Private _blockInfFileName As String = ""
    Private _orgBlockInfFileName As String = ""
    Private _dmyBlockInfFileName As String = ""
    Private _blockInf As New JManBlockInfFile()
    Private _dmyBlockInf As New JManBlockInfFile()
    Private _orgBlockInf As New JManBlockInfFile()
    '2016/11/01 Nakagawa Add Start
    Private _kozo As String = ""
    '2016/11/01 Nakagawa Add End

    '2016/11/01 Nakagawa Edit
    'Public Function OnInitialize(kojiFolder As String, buttonColor As Integer, backColor As Integer) As Integer
    Public Function OnInitialize(kojiFolder As String, buttonColor As Integer, backColor As Integer, kozo As String) As Integer

        OnInitialize = 0

        _kogo = subStringEnd(kojiFolder.TrimEnd("\"), "\")
        _blockInfFileName = kojiFolder.TrimEnd("\") + "\BLOCK.INF"
        _orgBlockInfFileName = kojiFolder.TrimEnd("\") + "\ORIGINALBLOCK.INF"
        _dmyBlockInfFileName = kojiFolder.TrimEnd("\") + "\DUMMYBLOCK.INF"
        '2016/11/01 Nakagawa Add Start
        _kozo = kozo
        '2016/11/01 Nakagawa Add End

        '背景色とボタン色を変更する。
        If backColor = 0 Then
            Me.Background = New SolidColorBrush(System.Windows.Media.Colors.DarkGray)
        Else
            Me.Background = New SolidColorBrush(System.Windows.Media.Colors.White)
        End If
        If buttonColor = 0 Then
            Me.ButtonBlockToLeft.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 50, 50, 50))
            Me.ButtonBlockToRight.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 50, 50, 50))
            Me.ButtonBlockAllToLeft.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 50, 50, 50))
            Me.ButtonDelBlockOk.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 50, 50, 50))
            Me.ButtonDelBlockCansel.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 50, 50, 50))
        Else
            Me.ButtonBlockToLeft.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 164, 150))
            Me.ButtonBlockToRight.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 164, 150))
            Me.ButtonBlockAllToLeft.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 164, 150))
            Me.ButtonDelBlockOk.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 164, 150))
            Me.ButtonDelBlockCansel.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 164, 150))
        End If

        'BLOCK.INFを読込
        Dim statusBlock As Integer = Me._blockInf.load(Me._blockInfFileName)
        If statusBlock <> 0 Then
            OnInitialize = 1
            Exit Function
        End If
        'DUMMYBLOCK.INFを読込
        Dim statusDmyBlock As Integer = Me._dmyBlockInf.load(Me._dmyBlockInfFileName)
        If statusDmyBlock = 2 Then
            OnInitialize = 1
            Exit Function
        End If
        'ORIGINALBLOCK.INFを読込
        Dim statusOrgBlock As Integer = Me._orgBlockInf.load(Me._orgBlockInfFileName)
        If statusOrgBlock = 2 Then
            OnInitialize = 1
            Exit Function
        End If

        'DUMMYBLOCK.INFが存在する場合は、BLOCK.INFからDUMMYBLOCK.INFに定義されているブロックを削除する。
        If statusDmyBlock = 0 Then
            Me._blockInf.deleteDefinedBlock(Me._dmyBlockInf)
        End If

        'ORIGINALBLOCK.INFが存在する場合は、DUMMYBLOCK.INFからORIGINALBLOCK.INFに定義されていないブロックを削除する。
        If statusDmyBlock = 0 And statusOrgBlock = 0 Then
            Me._dmyBlockInf.deleteUndefinedBlock(Me._orgBlockInf)
        End If

        '展開対象ブロックListBoxに表示する。
        Dim i As Integer = 0
        For i = 0 To Me._blockInf.size() - 1
            Me.ListBoxDevBlock.Items.Add(Me._blockInf.getAt(i)._blockName)
        Next

        '削除ブロックListBoxに表示する。
        For i = 0 To Me._dmyBlockInf.size() - 1
            Me.ListBoxDelBlock.Items.Add(Me._dmyBlockInf.getAt(i)._blockName)
        Next

    End Function

    Private Sub OnClick_ButtonDelBlockOk(sender As Object, e As RoutedEventArgs) Handles ButtonDelBlockOk.Click

        'BLOCK.INFを保存する。
        If Me._blockInf.size() = 0 Then
            MessageBox.Show("展開対象ブロックが一つもありません。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        Me._blockInf.sortRecord()
        If Me._blockInf.save(Me._blockInfFileName) <> 0 Then
            MessageBox.Show("システムエラー" + vbCrLf + "BLOCK.INFの保存に失敗しました。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        'DUMMYBLOCK.INFを保存する。
        If Me._dmyBlockInf.size() <> 0 Then
            Me._dmyBlockInf.sortRecord()
            If Me._dmyBlockInf.save(Me._dmyBlockInfFileName) <> 0 Then
                MessageBox.Show("システムエラー" + vbCrLf + "DUMMYBLOCK.INFの保存に失敗しました。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
        End If

        'ORIGINALBLOCK.INFを保存する。
        Dim orgBlockInf As New JManBlockInfFile()
        orgBlockInf.combineRecord(Me._blockInf, Me._dmyBlockInf)
        If orgBlockInf.size() <> 0 Then
            If orgBlockInf.save(Me._orgBlockInfFileName) <> 0 Then
                MessageBox.Show("システムエラー" + vbCrLf + "ORIGINALBLOCK.INFの保存に失敗しました。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
        End If

        'チャージコードの台数チェックを行う。
        '2016/11/01 Nakagawa Edit
        Dim errMsg As String = checkBlockNumber(_kogo, _kozo)
        If errMsg <> "" Then
            MessageBox.Show(errMsg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        Me._blockInf.clearRecord()
        Me._dmyBlockInf.clearRecord()
        Me._orgBlockInf.clearRecord()
        Me.Close()

    End Sub

    Private Sub OnClick_ButtonDelBlockCansel(sender As Object, e As RoutedEventArgs) Handles ButtonDelBlockCansel.Click

        Me._blockInf.clearRecord()
        Me._dmyBlockInf.clearRecord()
        Me._orgBlockInf.clearRecord()
        Me.Close()

    End Sub

    Private Sub OnClick_ButtonBlockToRight(sender As Object, e As RoutedEventArgs) Handles ButtonBlockToRight.Click

        If Me.ListBoxDevBlock.SelectedItems.Count = 0 Then
            MessageBox.Show("ブロックが選択されていません。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim i As Integer = 0
        For i = 0 To Me.ListBoxDevBlock.SelectedItems.Count - 1
            'インスタンスの更新
            Dim index As Integer = Me._blockInf.find(Me.ListBoxDevBlock.SelectedItems(0))
            If index <> -1 Then
                Me._dmyBlockInf.append(Me._blockInf.getAt(index))
                Me._blockInf.removeAt(index)
            End If

            '削除ブロックListBoxに追加する。
            Me.ListBoxDelBlock.Items.Add(Me.ListBoxDevBlock.SelectedItems(0))

            '展開対象ブロックListBoxから削除する。
            Me.ListBoxDevBlock.Items.Remove(Me.ListBoxDevBlock.SelectedItems(0))
        Next

    End Sub

    Private Sub OnClick_ButtonBlockToLeft(sender As Object, e As RoutedEventArgs) Handles ButtonBlockToLeft.Click

        If Me.ListBoxDelBlock.SelectedItems.Count = 0 Then
            MessageBox.Show("ブロックが選択されていません。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim i As Integer = 0
        For i = 0 To Me.ListBoxDelBlock.SelectedItems.Count - 1
            'インスタンスの更新
            Dim index As Integer = Me._dmyBlockInf.find(Me.ListBoxDelBlock.SelectedItems(0))
            If index <> -1 Then
                Me._blockInf.append(Me._dmyBlockInf.getAt(index))
                Me._dmyBlockInf.removeAt(index)
            End If

            '削除ブロックListBoxに追加する。
            Me.ListBoxDevBlock.Items.Add(Me.ListBoxDelBlock.SelectedItems(0))

            '展開対象ブロックListBoxから削除する。
            Me.ListBoxDelBlock.Items.Remove(Me.ListBoxDelBlock.SelectedItems(0))
        Next

    End Sub

    Private Sub OnClick_ButtonBlockAllToLeft(sender As Object, e As RoutedEventArgs) Handles ButtonBlockAllToLeft.Click

        Dim i As Integer = 0
        For i = 0 To Me.ListBoxDelBlock.Items.Count - 1
            'インスタンスの更新
            Dim index As Integer = Me._dmyBlockInf.find(Me.ListBoxDelBlock.Items(0))
            If index <> -1 Then
                Me._blockInf.append(Me._dmyBlockInf.getAt(index))
                Me._dmyBlockInf.removeAt(index)
            End If

            '削除ブロックListBoxに追加する。
            Me.ListBoxDevBlock.Items.Add(Me.ListBoxDelBlock.Items(0))

            '展開対象ブロックListBoxから削除する。
            Me.ListBoxDelBlock.Items.Remove(Me.ListBoxDelBlock.Items(0))
        Next

    End Sub
End Class
