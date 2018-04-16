Public Class JCBFileExplore3
    '設定ファイル
    Private _setFileExplore As New JCBSetFileExplore
    '要素のリスト
    Private _elementList As New ArrayList
    '要素の数
    Private _elementCount As Integer = 0
    '工事フォルダ
    Public _kojiFolder As String = ""
    'IJCADのexeファイル名
    Public _ijcadExe As String = ""


    Private Sub JCBFileExplore3_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        _setFileExplore._kojiFolder = _kojiFolder
        _setFileExplore.LoadFile("fileExploreSettingFile.csv")
        _elementList = _setFileExplore._fileExploreInfList
        _elementCount = _setFileExplore.Size()

        If _kojiFolder <> "" Then
            'ツリーグリットビュー作成
            MakeTreeGridView()
        End If

    End Sub

    '---------------------------------------------------------------------------------------------------
    ' ツリーグリットビュー作成
    '---------------------------------------------------------------------------------------------------
    Public Function MakeTreeGridView() As Integer

        '1つ前の情報
        Dim previosInfo As New JCBFileExploreInfo
        '1つ後の情報
        Dim nextInfo As New JCBFileExploreInfo
        '親の番号
        Dim parentNo As Integer = -1
        '子の番号
        Dim childNo As Integer = -1
        '1つのセルを選択するとその行全てを選択状態にする
        TreeGridView3.SelectionMode = 0

        For i = 1 To _elementCount

            Dim info As JCBFileExploreInfo = _elementList(i - 1)

            If i < _elementCount Then
                nextInfo = _elementList(i)
            End If


            With TreeGridView3


                If i = 1 Then
                    ' 親
                    AddParent(info)
                    parentNo += 1
                    Continue For
                End If

                If info._noElement1 <> nextInfo._noElement1 And info._noElement1 <> previosInfo._noElement1 Then
                    ' 親
                    AddParent(info)
                    parentNo += 1
                    childNo = -1
                ElseIf info._noElement1 <> previosInfo._noElement1 Then
                    ' 親
                    AddParent(info)
                    parentNo += 1
                    childNo = -1
                ElseIf info._noElement1 <> nextInfo._noElement1 And info._noElement2 <> previosInfo._noElement2 Then
                    ' 子
                    AddChild(info, parentNo)
                    childNo += 1
                ElseIf info._noElement1 = nextInfo._noElement1 Then
                    If info._noElement2 <> previosInfo._noElement2 Then
                        ' 子
                        AddChild(info, parentNo)
                        childNo += 1
                    ElseIf info._noElement1 = nextInfo._noElement1 And info._noElement2 = previosInfo._noElement2 Then
                        '孫
                        AddGrandson(info, parentNo, childNo)
                    ElseIf info._noElement2 = nextInfo._noElement2 Then
                        '孫
                        AddGrandson(info, parentNo, childNo)
                    End If

                End If

            End With

            previosInfo = info

        Next

        Return 0
    End Function

    Sub AddParent(info As JCBFileExploreInfo)
        With TreeGridView3
            If info._fileName = "" And info._dataFolder = "" Then
                Dim img As New Bitmap(25, 25)
                .Nodes.Add(info._fileType, "", "", img, "", "", "")
            Else
                If info._status = 0 Then

                    'Dim appIcon As Icon = GetFileIcon(info._fileName)
                    'Dim bmp As Bitmap = appIcon.ToBitmap

                    'Dim graphics As Graphics = Graphics.FromImage(bmp)

                    '' 画像を貼り付け
                    'graphics.DrawImage(bmp, New Point(0, 0))

                    '' テキストを画像の上から貼り付け
                    'graphics.DrawString(info._fileType, New Font("Tahoma", 10.5F), Brushes.Blue, 5, 5)
                    'Dim a As New DataGridViewTextBoxCell
                    'Dim str As String = info._fileType

                    Dim img As Image = CType(My.Resources.ResourceManager.GetObject("_10_status_ok"), Bitmap)
                    .Nodes.Add(info._fileType, info._dataFolder, info._fileName, img, info._updateDate, info._size.ToString + "KB", info._lastSavedPerson)

                ElseIf info._status = 1 Then

                    Dim img As Image = CType(My.Resources.ResourceManager.GetObject("_11_status_bad"), Bitmap)
                    .Nodes.Add(info._fileType, info._dataFolder, info._fileName, img, info._updateDate, info._size.ToString + "KB", info._lastSavedPerson)
                Else
                    Dim img As Image = CType(My.Resources.ResourceManager.GetObject("_12_status_no"), Bitmap)
                    .Nodes.Add(info._fileType, info._dataFolder, info._fileName, img, info._updateDate, "", info._lastSavedPerson)
                End If
            End If
        End With
    End Sub

    Sub AddChild(info As JCBFileExploreInfo, parentNo As Integer)
        With TreeGridView3

            If info._fileName = "" And info._dataFolder = "" Then
                Dim img As New Bitmap(25, 25)
                .Nodes(parentNo).Nodes.Add(info._fileType, "", "", img, "", "", "")
            Else
                If info._status = 0 Then
                    Dim img As Image = CType(My.Resources.ResourceManager.GetObject("_10_status_ok"), Bitmap)
                    .Nodes(parentNo).Nodes.Add(info._fileType, info._dataFolder, info._fileName, img, info._updateDate, info._size.ToString + "KB", info._lastSavedPerson)
                ElseIf info._status = 1 Then
                    Dim img As Image = CType(My.Resources.ResourceManager.GetObject("_11_status_bad"), Bitmap)
                    .Nodes(parentNo).Nodes.Add(info._fileType, info._dataFolder, info._fileName, img, info._updateDate, info._size.ToString + "KB", info._lastSavedPerson)
                Else
                    Dim img As Image = CType(My.Resources.ResourceManager.GetObject("_12_status_no"), Bitmap)
                    .Nodes(parentNo).Nodes.Add(info._fileType, info._dataFolder, info._fileName, img, info._updateDate, "", info._lastSavedPerson)
                End If
            End If

        End With
    End Sub

    Sub AddGrandson(info As JCBFileExploreInfo, parentNo As Integer, childNo As Integer)
        With TreeGridView3

            If info._fileName = "" And info._dataFolder = "" Then
                Dim img As New Bitmap(25, 25)
                .Nodes(parentNo).Nodes(childNo).Nodes.Add(info._fileType, "", "", img, "", "", "")
            Else
                If info._status = 0 Then
                    Dim img As Image = CType(My.Resources.ResourceManager.GetObject("_10_status_ok"), Bitmap)
                    .Nodes(parentNo).Nodes(childNo).Nodes.Add(info._fileType, info._dataFolder, info._fileName, img, info._updateDate, info._size.ToString + "KB", info._lastSavedPerson)
                ElseIf info._status = 1 Then
                    Dim img As Image = CType(My.Resources.ResourceManager.GetObject("_11_status_bad"), Bitmap)
                    .Nodes(parentNo).Nodes(childNo).Nodes.Add(info._fileType, info._dataFolder, info._fileName, img, info._updateDate, info._size.ToString + "KB", info._lastSavedPerson)
                Else
                    Dim img As Image = CType(My.Resources.ResourceManager.GetObject("_12_status_no"), Bitmap)
                    .Nodes(parentNo).Nodes(childNo).Nodes.Add(info._fileType, info._dataFolder, info._fileName, img, info._updateDate, "", info._lastSavedPerson)
                End If
            End If

        End With
    End Sub

    Function GetFileIcon(fileName As String)
        Dim path As String = _kojiFolder + fileName
        If System.IO.File.Exists(path) = True Then
            Dim appIcon As Icon = System.Drawing.Icon.ExtractAssociatedIcon(path)
            Return appIcon
        End If
        Return 1
    End Function

    '---------------------------------------------------------------------------------------------------
    ' cellをマウス左でダブルクリックした際のイベント
    '---------------------------------------------------------------------------------------------------
    Private Sub TreeGridView1_CellMouseDoubleClick(ByVal sender As Object, ByVal e As MouseEventArgs) Handles TreeGridView3.CellMouseDoubleClick

        If e.Button = MouseButtons.Left Then
            Try
                Dim selectedRow As DataGridViewSelectedRowCollection = TreeGridView3.SelectedRows
                Dim fileTypeCell As DataGridViewCell = selectedRow(0).Cells(0)
                Dim dataFolderCell As DataGridViewCell = selectedRow(0).Cells(1)
                Dim fileNameCell As DataGridViewCell = selectedRow(0).Cells(2)
                Dim fileType As String = fileTypeCell.Value
                Dim dataFolder As String = dataFolderCell.Value
                Dim fileName As String = fileNameCell.Value

                Dim filePath As String = CreateFilePath(_kojiFolder, dataFolder, fileName)

                Dim info As JCBFileExploreInfo = _setFileExplore.FindJCBFileExploreInfo(fileType)

                If System.IO.File.Exists(filePath) = True Then
                    If info._displayProgram1 <> "" Then
                        '表示プログラム1でファイルを開く
                        '現段階ではijcadのみ可能
                        If info._displayProgram1 = "gcad.exe" Then
                            System.Diagnostics.Process.Start(_ijcadExe, filePath)
                        End If

                    Else
                        '関連付けられたアプリケーションでファイルを開く
                        Dim p As System.Diagnostics.Process = System.Diagnostics.Process.Start(filePath)
                    End If
                End If
            Catch ex As Exception

            End Try
        End If


    End Sub

    '---------------------------------------------------------------------------------------------------
    ' cellを右クリックした際のイベント
    '---------------------------------------------------------------------------------------------------
    Private Sub TreeGridView1_CellMouseClick(ByVal sender As Object, ByVal e As MouseEventArgs) Handles TreeGridView3.CellMouseClick

        If e.Button = MouseButtons.Right Then
            Dim p As System.Drawing.Point = System.Windows.Forms.Cursor.Position
            Me.ContextMenuStrip1.Show(p)
        End If

    End Sub

    '---------------------------------------------------------------------------------------------------
    ' headerを右クリックした際のイベント
    '---------------------------------------------------------------------------------------------------
    Private Sub TreeGridView1_ColumnHeaderMouseClick(sender As Object, e As MouseEventArgs) Handles TreeGridView3.ColumnHeaderMouseClick
        If e.Button = MouseButtons.Right Then
            Dim p As System.Drawing.Point = System.Windows.Forms.Cursor.Position
            Me.ContextMenuStrip2.Show(p)
        End If

    End Sub

    '---------------------------------------------------------------------------------------------------
    ' 閉じた際のイベント
    '---------------------------------------------------------------------------------------------------
    Private Sub JCBFileExplore3_Closed(sender As Object, e As EventArgs) Handles MyBase.Closed
        Dim name As String = "FileExplore()"
        Dim cs As Control() = Me.Controls.Find(name, True)
        If cs.Length > 0 Then
            CType(cs(0), Button).BackColor = System.Drawing.Color.White
        End If

    End Sub

    '---------------------------------------------------------------------------------------------------
    ' 画面のサイズが変更された際の処理
    '---------------------------------------------------------------------------------------------------
    Private Sub Window_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        'TreeGridView3のサイズを変更
        Dim c As Control = DirectCast(sender, Control)
        TreeGridView3.Height = c.Height - 140
        TreeGridView3.Width = c.Width - 40

        If Location.Y = 0 Then
            Location = New Point(Location.X, 30)
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

    End Sub
End Class