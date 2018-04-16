Public Class JCBFileExplore
    '---------------------------------------------------------------------------------------------------
    ' プロパティ
    '---------------------------------------------------------------------------------------------------
    '設定ファイル
    Private _setFileExplore As New JCBSetFileExplore
    '要素のリスト
    Private _elementList As New ArrayList
    '要素の数
    Private _elementCount As Integer = 0
    'ツリービューNodeの開閉list
    Private _openNodeList As New ArrayList


    '---------------------------------------------------------------------------------------------------
    ' フォームロード時の処理
    '---------------------------------------------------------------------------------------------------
    Private Sub Form_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        _setFileExplore.LoadFile("C:\Users\t.hayasaka\Desktop\fileExploreSettingFile.csv")
        _elementList = _setFileExplore._fileExploreInfList
        _elementCount = _setFileExplore.size()

        'ツリービュー作成
        MakeTreeView()
        'ツリービューの開閉情報を取得
        CallRecursive(TreeViewFileType)

        'リストビュー作成
        MakeListView()

    End Sub

    '---------------------------------------------------------------------------------------------------
    ' ツリービュー開閉情報取得再帰処理
    '---------------------------------------------------------------------------------------------------
    Private Sub PrintRecursive(ByVal n As TreeNode)
        If n.IsVisible = True Then
            _openNodeList.Add(n.Text)
        End If

        Dim aNode As TreeNode
        For Each aNode In n.Nodes
            PrintRecursive(aNode)
        Next
    End Sub

    Private Sub CallRecursive(ByVal aTreeView As TreeView)
        _openNodeList.Clear()

        Dim n As TreeNode
        For Each n In aTreeView.Nodes
            PrintRecursive(n)
        Next
    End Sub

    '---------------------------------------------------------------------------------------------------
    ' リストビュー作成
    '---------------------------------------------------------------------------------------------------
    Public Function MakeListView() As Integer

        With ListView1
            '項目を表示する方法を設定
            '各項目に関する詳しい情報が各列に配置されます。(詳細表示)
            .View = View.Details

            '全てのアイテムを削除する
            .Items.Clear()

            Dim dummyImageList As New ImageList With {
                .ImageSize = New Size(1, 23.2)
            }
            ListView1.SmallImageList = dummyImageList

            .Columns.Add("データフォルダ", 210, HorizontalAlignment.Left)
            .Columns.Add("ファイル名", 210, HorizontalAlignment.Left)
            .Columns.Add("ステータス", 60, HorizontalAlignment.Left)
            .Columns.Add("更新日", 70, HorizontalAlignment.Left)
            .Columns.Add("サイズ", 40, HorizontalAlignment.Left)
            .Columns.Add("最終保存者", 80, HorizontalAlignment.Left)

            Dim showCount As Integer = 0

            For i = 1 To _elementCount
                Dim info As JCBFileExploreInfo = _elementList(i - 1)

                'ツリービューに要素が表示されているかチェック
                Dim foundIndex As Integer = _openNodeList.IndexOf(info._fileType)

                If foundIndex >= 0 Then
                    'アイテムを追加
                    .Items.Add(info._dataFolder, showCount)
                    'サブアイテムを追加
                    .Items(showCount).SubItems.Add(info._fileName.ToString)
                    .Items(showCount).SubItems.Add(info._status.ToString)
                    .Items(showCount).SubItems.Add(info._updateDate.ToString)
                    .Items(showCount).SubItems.Add(info._size.ToString + "KB")
                    .Items(showCount).SubItems.Add(info._lastSavedPerson.ToString)

                    showCount += 1

                End If

            Next

            '項目数に応じて高さを調節
            ListView1.Height = showCount * 25 + 100

        End With

        'リストビューとツリービューの高さを揃える
        TreeViewFileType.Height = ListView1.Height

        Return 0

    End Function

    '---------------------------------------------------------------------------------------------------
    ' ツリービュー作成
    '---------------------------------------------------------------------------------------------------
    Public Function MakeTreeView()

        '1つ前の情報
        Dim previosInfo As New JCBFileExploreInfo
        '1つ後の情報
        Dim nextInfo As New JCBFileExploreInfo
        '親の番号
        Dim parentNo As Integer = -1
        '親の番号
        Dim childNo As Integer = -1

        For i = 1 To _elementCount

            Dim info As JCBFileExploreInfo = _elementList(i - 1)

            '1つ後の情報
            If i < _elementCount Then
                nextInfo = _elementList(i)
            End If


            With TreeViewFileType

                If i = 1 Then
                    TreeViewFileType.Nodes.Add(info._fileType)
                    parentNo += 1
                    Continue For
                End If

                If info._noElement1 <> nextInfo._noElement1 And info._noElement1 <> previosInfo._noElement1 Then
                    ' 親
                    TreeViewFileType.Nodes.Add(info._fileType)
                    parentNo += 1
                    childNo = -1
                ElseIf info._noElement1 <> previosInfo._noElement1 Then
                    ' 親
                    TreeViewFileType.Nodes.Add(info._fileType)
                    parentNo += 1
                    childNo = -1
                ElseIf info._noElement1 <> nextInfo._noElement1 And info._noElement2 <> previosInfo._noElement2 Then
                    ' 子
                    TreeViewFileType.Nodes(parentNo).Nodes.Add(info._fileType)
                    childNo += 1
                ElseIf info._noElement1 = nextInfo._noElement1 Then
                    If info._noElement2 <> previosInfo._noElement2 Then
                        ' 子
                        TreeViewFileType.Nodes(parentNo).Nodes.Add(info._fileType)
                        childNo += 1
                    ElseIf info._noElement1 = nextInfo._noElement1 And info._noElement2 = previosInfo._noElement2 Then
                        '孫
                        TreeViewFileType.Nodes(parentNo).Nodes(childNo).Nodes.Add(info._fileType)
                    ElseIf info._noElement2 = nextInfo._noElement2 Then
                        '孫
                        TreeViewFileType.Nodes(parentNo).Nodes(childNo).Nodes.Add(info._fileType)
                    End If

                End If

            End With

            '1つ前の情報
            previosInfo = info

        Next

        Return 0

    End Function

    'ノードのイベント---------------------------------------------------------------------------------
    Sub TreeViewFileType_NodeMouseClick(ByVal sender As Object,
    ByVal e As TreeNodeMouseClickEventArgs) _
    Handles TreeViewFileType.NodeMouseClick

        CallRecursive(TreeViewFileType)
        MakeListView()

    End Sub

    Sub TreeViewFileType_NodeMouseDoubleClick(ByVal sender As Object,
    ByVal e As TreeNodeMouseClickEventArgs) _
    Handles TreeViewFileType.NodeMouseDoubleClick

        CallRecursive(TreeViewFileType)
        MakeListView()

    End Sub

    '---------------------------------------------------------------------------------------------------
    ' MainWindow画面のサイズ変更が行われた際の処理
    '---------------------------------------------------------------------------------------------------
    'SizeChangedイベントハンドラ
    Private Sub Window_SizeChanged(sender As Object, e As EventArgs) _
    Handles MyBase.SizeChanged

        'ログウィンドウのサイズを変更
        Dim c As Control = DirectCast(sender, Control)
        Panel2.Height = c.Height - 140

        If Location.Y = 0 Then
            Location = New Point(Location.X, 30)
        End If

    End Sub

End Class