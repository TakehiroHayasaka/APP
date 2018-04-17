Imports System.Windows.Forms
Public Class YCM_CreateMovePosAll
    Public m_Return As Boolean
    'Public gAddStartDrawPointIndex As Long = -999999            '一括処理で追加する前の座標値数
    Private m_AddMovePos() As st_AddMovePos  '一括処理で作成した点の情報、「実行」時にgAddMovePos()に移行する 

    Private Sub YCM_CreateMovePos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ind As Long
#If 0 Then '++test.test.test++++++++++++++++++++++++++++++++++++
        gNumAddMovePos = 4
        ReDim gAddMovePos(4)
        ReDim gAddMovePos(0).dirBasePos(2)
        ReDim gAddMovePos(1).dirBasePos(2)
        ReDim gAddMovePos(2).dirBasePos(2)
        ReDim gAddMovePos(3).dirBasePos(2)
        gAddMovePos(0).iType = 2
        gAddMovePos(0).dirBasePos(0) = gDrawPoints(1)
        gAddMovePos(0).dirBasePos(1) = gDrawPoints(2)
        gAddMovePos(0).dMoveValue = 100
        gAddMovePos(0).basePos = gDrawPoints(3)
        gAddMovePos(0).newPos = gDrawPoints(4)
        gAddMovePos(0).strResult = "正常11"
        gAddMovePos(1).iType = 2
        gAddMovePos(1).dirBasePos(0) = gDrawPoints(5)
        gAddMovePos(1).dirBasePos(1) = gDrawPoints(6)
        gAddMovePos(1).dMoveValue = 200
        gAddMovePos(1).basePos = gDrawPoints(7)
        gAddMovePos(1).newPos = gDrawPoints(8)
        gAddMovePos(1).strResult = "正常12"
        '------------------------------------------
        gAddMovePos(2).iType = 4
        gAddMovePos(2).dirBasePos(0) = gDrawPoints(9)
        gAddMovePos(2).dirBasePos(1) = gDrawPoints(10)
        gAddMovePos(2).dMoveValue = 0
        gAddMovePos(2).basePos = Nothing
        gAddMovePos(2).newPos = gDrawPoints(11)
        gAddMovePos(2).strResult = "正常41"
        gAddMovePos(3).iType = 4
        gAddMovePos(3).dirBasePos(0) = gDrawPoints(3)
        gAddMovePos(3).dirBasePos(1) = gDrawPoints(4)
        gAddMovePos(3).dMoveValue = 0
        gAddMovePos(3).basePos = Nothing
        gAddMovePos(3).newPos = gDrawPoints(5)
        gAddMovePos(3).strResult = "正常42"
#End If '++test.test.test+++++++++++++++++++++++++++++++++++++

        '各タブ内のDataGridの初期化（gAddMovePos As st_AddMovePosから）

        Me.DGV_MoveList_2Pos.Rows.Clear()
        ' 2点指定の移動点リスト

        ind = 0
        For i As Long = 0 To gNumAddMovePos - 1
            If (gAddMovePos(i).iType = 2) And (gAddMovePos(i).isDelete = False) Then
                With Me.DGV_MoveList_2Pos   'AddData_Point.DGV_MoveList_2Pos
                    .Rows.Add()
                    .Rows(ind).Cells(0).Value = False
                    .Rows(ind).Cells(1).Value = gAddMovePos(i).dirBasePos(0).LabelName
                    .Rows(ind).Cells(2).Value = gAddMovePos(i).dirBasePos(1).LabelName
                    .Rows(ind).Cells(3).Value = gAddMovePos(i).dMoveValue
                    .Rows(ind).Cells(4).Value = gAddMovePos(i).basePos.LabelName
                    .Rows(ind).Cells(5).Value = gAddMovePos(i).newPos.LabelName
                    .Rows(ind).Cells(6).Value = gAddMovePos(i).strResult
                    ind = ind + 1
                End With
            End If
        Next
        Me.DGV_MoveList_2Pos.Refresh()

        ' 3点指定の移動点リスト

        Me.DGV_MoveList_3Pos.Rows.Clear()
        ind = 0
        For i As Long = 0 To gNumAddMovePos - 1
            If (gAddMovePos(i).iType = 3) And (gAddMovePos(i).isDelete = False) Then
                With Me.DGV_MoveList_3Pos
                    .Rows.Add()
                    .Rows(ind).Cells(0).Value = False
                    .Rows(ind).Cells(1).Value = gAddMovePos(i).dirBasePos(0).LabelName
                    .Rows(ind).Cells(2).Value = gAddMovePos(i).dirBasePos(1).LabelName
                    .Rows(ind).Cells(3).Value = gAddMovePos(i).dirBasePos(2).LabelName
                    .Rows(ind).Cells(4).Value = gAddMovePos(i).dMoveValue
                    .Rows(ind).Cells(5).Value = gAddMovePos(i).basePos.LabelName
                    .Rows(ind).Cells(6).Value = gAddMovePos(i).newPos.LabelName
                    .Rows(ind).Cells(7).Value = gAddMovePos(i).strResult
                    ind = ind + 1
                End With
            End If
        Next
        Me.DGV_MoveList_3Pos.Refresh()

        ' 2点中間の移動点リスト

        Me.DGV_MoveList_2C.Rows.Clear()
        ind = 0
        For i As Long = 0 To gNumAddMovePos - 1
            If (gAddMovePos(i).iType = 4) And (gAddMovePos(i).isDelete = False) Then
                With Me.DGV_MoveList_2C
                    .Rows.Add()
                    .Rows(ind).Cells(0).Value = False
                    .Rows(ind).Cells(1).Value = gAddMovePos(i).dirBasePos(0).LabelName
                    .Rows(ind).Cells(2).Value = gAddMovePos(i).dirBasePos(1).LabelName
                    .Rows(ind).Cells(3).Value = gAddMovePos(i).newPos.LabelName
                    .Rows(ind).Cells(4).Value = gAddMovePos(i).strResult
                    ind = ind + 1
                End With
            End If
        Next
        Me.DGV_MoveList_2C.Refresh()

        ' 一括処理結果のリストDGV_BatchMove
        gAddStartDrawPointIndex = nLookPoints  '一括処理で追加する前の座標値数
        Me.DGV_BatchMove.Rows.Clear()

        '選択されていたタブを表示
        Me.TabControl1.SelectTab(gCurrCreateMovePosTabPage)
    End Sub

    '=================================================
    ' 移動方向を2点で指定して移動点を作成
    '=================================================
    Private Sub Button_Add2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Add2.Click
        Me.Hide()
        IOUtil.EndThread()
        IOUtil.LibCommand("newpointbyvec")
        gCurrCreateMovePosTabPage = 0
    End Sub

    Private Sub Button_Del2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Del2.Click
        '選択行のデータを削除する
        Dim ind As Long
        For i As Long = 0 To gNumAddMovePos - 1
            If (gAddMovePos(i).iType = 2) And (gAddMovePos(i).isDelete = False) Then
                With Me.DGV_MoveList_2Pos   'AddData_Point.DGV_MoveList_2Pos
                    If (.Rows(i).Cells(0).Value = True) Then
                        '削除しようとしている新計測点を他で使っていないかを確認

                        '→他で使っている場合には削除できない


                        gAddMovePos(i).isDelete = True
                        '行の削除
                        ind = .Rows(i).Index
                        .Rows.RemoveAt(ind)
                        ''                    If .SelectedRows.Count > 0 AndAlso _
                        ''                        Not .SelectedRows(0).Index = .Rows.Count - 1 Then
                        ''                        .Rows.RemoveAt(.SelectedRows(0).Index)
                        ''                    End If
                    End If
                End With
            End If
        Next
        Me.DGV_MoveList_2Pos.Refresh()
    End Sub

    '=================================================
    ' 移動方向を3点で指定した平面で指定して移動点を作成
    '=================================================
    Private Sub Button_Add3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Add3.Click
        Me.Hide()
        IOUtil.EndThread()
        IOUtil.LibCommand("newpointbyface")
        gCurrCreateMovePosTabPage = 1
    End Sub

    Private Sub Button_Del3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Del3.Click
        '選択行のデータを削除する
        Dim ind As Long
        For i As Long = 0 To gNumAddMovePos - 1
            If (gAddMovePos(i).iType = 3) And (gAddMovePos(i).isDelete = False) Then
                With Me.DGV_MoveList_3Pos   'AddData_Point.DGV_MoveList_2Pos
                    If (.Rows(i).Cells(0).Value = True) Then
                        '削除しようとしている新計測点を他で使っていないかを確認

                        '→他で使っている場合には削除できない


                        gAddMovePos(i).isDelete = True
                        '行の削除
                        ind = .Rows(i).Index
                        .Rows.RemoveAt(ind)
                        ''                    If .SelectedRows.Count > 0 AndAlso _
                        ''                        Not .SelectedRows(0).Index = .Rows.Count - 1 Then
                        ''                        .Rows.RemoveAt(.SelectedRows(0).Index)
                        ''                    End If
                    End If
                End With
            End If
        Next
        Me.DGV_MoveList_3Pos.Refresh()
    End Sub

    '=================================================
    ' 指定された2点の中間点を作成
    '=================================================
    Private Sub Button_Add2C_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Add2C.Click
        Me.Hide()
        IOUtil.EndThread()
        IOUtil.LibCommand("newpointcenter")
        gCurrCreateMovePosTabPage = 2
    End Sub

    Private Sub Button_Del2C_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Del2C.Click
        '選択行のデータを削除する
        Dim ind As Long
        For i As Long = 0 To gNumAddMovePos - 1
            If (gAddMovePos(i).iType = 4) And (gAddMovePos(i).isDelete = False) Then
                With Me.DGV_MoveList_2C   'AddData_Point.DGV_MoveList_2Pos
                    If (.Rows(i).Cells(0).Value = True) Then
                        '削除しようとしている新計測点を他で使っていないかを確認

                        '→他で使っている場合には削除できない


                        gAddMovePos(i).isDelete = True
                        '行の削除
                        ind = .Rows(i).Index
                        .Rows.RemoveAt(ind)
                        ''                    If .SelectedRows.Count > 0 AndAlso _
                        ''                        Not .SelectedRows(0).Index = .Rows.Count - 1 Then
                        ''                        .Rows.RemoveAt(.SelectedRows(0).Index)
                        ''                    End If
                    End If
                End With
            End If
        Next
        Me.DGV_MoveList_2C.Refresh()
    End Sub

    '=================================================
    ' 一括処理で移動点を作成
    '=================================================
    Private Sub B_Sel_List_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles B_Sel_List.Click
        '「リストの選択」

        '実行リストファイルの選択

        OpenFileDialog1.FileName = "*.txt"
        OpenFileDialog1.Filter = "*.txt|*.txt"
        'On Error GoTo ErrHandle

        Dim result As System.Windows.Forms.DialogResult = OpenFileDialog1.ShowDialog()
        If result = System.Windows.Forms.DialogResult.OK Then
            str = New ArrayList
            str_new = New ArrayList
            MovePointBatchListFileName = OpenFileDialog1.FileName()
            TB_Path.Text = MovePointBatchListFileName
        End If
        'ErrHandle:
    End Sub

    Private Sub B_Edit_List_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles B_Edit_List.Click
        '「リストの編集」

        Dim listFile As String
        listFile = TB_Path.Text
        'ファイルが存在するかをチェック
        Dim fileExists As Boolean
        fileExists = bIsListFileExist(listFile)
        If fileExists = False Then Exit Sub
        ' ファイルを指定してメモ帳を起動する

        System.Diagnostics.Process.Start("Notepad", listFile)
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        '「処理結果確認」移動点一括処理の仮実行

        '--Me.DialogResult = System.Windows.Forms.DialogResult.OK
        '--Me.Close()
        Dim listFile As String
        Dim fileExists As Boolean
        '「実行リストファイル」が存在するかをチェック
        listFile = TB_Path.Text
        fileExists = bIsListFileExist(listFile)
        If fileExists = False Then Exit Sub

        '指定された実行リストファイルから処理結果を作成する  
        Dim posListArr As New ArrayList
        Dim batchPosList As type_batchList
        Dim iRet As Integer
        iRet = checkBatchListFile(listFile, posListArr)

        '処理結果から、仮表示用の新規計測点を作成する
        'Public gCurrUserMovePoints() As CLookPoint          '仮表示用の点群
        Call addCurrMovePointsByPosList(posListArr)

        '処理結果DataGridに値を入れる
        Me.DGV_BatchMove.Rows.Clear()
        Dim ii As Long
        If (posListArr.Count > 0) Then
            For ii = 0 To posListArr.Count - 1
                batchPosList = posListArr(ii)
                With Me.DGV_BatchMove
                    .Rows.Add()
                    .Rows(ii).Cells(0).Value = batchPosList.bCreate
                    .Rows(ii).Cells(1).Value = batchPosList.strMoveDir
                    .Rows(ii).Cells(2).Value = batchPosList.strbasePoslabel
                    .Rows(ii).Cells(3).Value = batchPosList.dMove
                    .Rows(ii).Cells(4).Value = batchPosList.strNewPosLabel
                    .Rows(ii).Cells(5).Value = batchPosList.strResult
                    If (batchPosList.iResult <> 0) Then
                        .Rows(ii).Cells(0).ReadOnly = True
                    End If
                End With
            Next
            Me.DGV_BatchMove.Refresh()
        End If
        m_Return = False
    End Sub

    Private Sub Button_AllSel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_AllSel.Click
        '全て選択

        Dim ii As Long, strErr As String
        For ii = 0 To Me.DGV_BatchMove.Rows.Count - 1
            strErr = Me.DGV_BatchMove.Rows(ii).Cells(5).Value
            If (Len(strErr) <= 0) Then
                Me.DGV_BatchMove.Rows(ii).Cells(0).Value = True
            End If
        Next
    End Sub

    Private Sub Button_UnSel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_UnSel.Click
        '選択解除
        Dim ii As Long
        For ii = 0 To Me.DGV_BatchMove.Rows.Count - 1
            Me.DGV_BatchMove.Rows(ii).Cells(0).Value = False
        Next
    End Sub

    Private Sub Button_BatchOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_BatchOK.Click
        '実行（移動点の一括作成）

        gCurrCreateMovePosTabPage = 3
        '1.「実行」新規計測点は既にgDrawPoints()に登録済みなので以下のインデックスのみを設定

        gAddStartDrawPointIndex = nLookPoints  '一括処理で追加する前の座標値数をセット

        '2.他のタブへの情報保持
        '--------------作成必要----------------
        ' 他のタブに情報を渡す



    End Sub

    '処理結果リストから仮表示点を作成する
    Private Sub addCurrMovePointsByPosList(ByVal posListArr As ArrayList)
        Dim batchPosList As type_batchList
        Dim newPos As New CLookPoint
        Dim iRet As Integer
        Dim gpNewPos As GeoPoint
        nCurrUserMovePoints = 0
        ReDim Preserve gCurrUserMovePoints(nCurrUserMovePoints)
        If (posListArr.Count > 0) Then
            For ii = 0 To posListArr.Count - 1
                batchPosList = posListArr(ii)
                If (batchPosList.iResult = 0) Then
                    gpNewPos = batchPosList.newPos.toGeopoint()
                    iRet = AddCurrUserPoint(gpNewPos, batchPosList.strNewPosLabel)
                End If
            Next
        End If
    End Sub

    ' 仮表示点に新規の点を追加する
    Public Function AddCurrUserPoint(ByVal 座標点 As GeoPoint, ByVal ラベル As String) As Integer
        AddCurrUserPoint = -1
        Dim maxMID As Long = 0
        If (nCurrUserMovePoints > 0) Then
            maxMID = gCurrUserMovePoints(nCurrUserMovePoints - 1).mid
        End If
        ReDim Preserve gCurrUserMovePoints(nCurrUserMovePoints)
        gCurrUserMovePoints(nCurrUserMovePoints) = New CLookPoint(座標点.x, 座標点.y, 座標点.z)
        gCurrUserMovePoints(nCurrUserMovePoints).mid = maxMID + 1
        gCurrUserMovePoints(nCurrUserMovePoints).tid = 0    '[TID]は０で設定

        gCurrUserMovePoints(nCurrUserMovePoints).LabelName = ラベル
        gCurrUserMovePoints(nCurrUserMovePoints).sortID = nCurrUserMovePoints
        gCurrUserMovePoints(nCurrUserMovePoints).posType = CLookPoint.posTypeMode.User

        Dim gmat As New GeoMatrix
        Call matToGeoMat(gmat)
        座標点.Transform(gmat)
        gCurrUserMovePoints(nCurrUserMovePoints).Real_x = 座標点.x * sys_ScaleInfo.scale
        gCurrUserMovePoints(nCurrUserMovePoints).Real_y = 座標点.y * sys_ScaleInfo.scale
        gCurrUserMovePoints(nCurrUserMovePoints).Real_z = 座標点.z * sys_ScaleInfo.scale
        AddCurrUserPoint = nCurrUserMovePoints
        nCurrUserMovePoints = nCurrUserMovePoints + 1
    End Function

    '実行リストファイルの存在確認

    Private Function bIsListFileExist(ByVal strListFileName As String) As Boolean
        'ファイルが存在するかをチェック
        Dim fileExists As Boolean
        bIsListFileExist = False
        fileExists = My.Computer.FileSystem.FileExists(strListFileName)
        If fileExists = False Then
            MsgBox("実行リストファイルが存在しません。実行リストファイルを指定してください。", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation)
            Exit Function
        End If
        bIsListFileExist = True
    End Function

    '実行リストファイルのシンタックスチェック
    Private Function checkBatchListFile( _
        ByVal strListFileName As String, _
        ByRef posListArr As ArrayList _
    ) As Integer
        Dim ii As Long
        Dim ioR As System.IO.StreamReader
        Dim strLine As String, strList As New ArrayList, strDivList As New ArrayList
        checkBatchListFile = -1
        '1. 実行リストファイル有効行の読み込み
        ioR = New System.IO.StreamReader(strListFileName)
        While ioR.Peek >= 0
            strLine = Trim(ioR.ReadLine)
            If (Len(strLine) > 0) Then
                If (strLine(0) <> ";") Then
                    strList.Add(strLine)
                End If
            End If
        End While
        For ii = 0 To strList.Count - 1
            strDivList.Add(Split(strList(ii), ","))
        Next ii

        '2. 実行リスト内容の解析

        Dim strArr As Array, iRet As Integer
        For ii = 0 To strList.Count - 1
            strArr = strDivList(ii)
            '2.1 1行分の実行リストの解析

            iRet = checkExecuteList(strArr, posListArr)
            If (iRet <> 0) Then
            End If
        Next ii
        checkBatchListFile = 0
    End Function

    '--移動点作成用データ構造
    Public Structure type_batchList
        Public bCreate As Boolean           '0)作成フラグ（=TRUE：作成）

        Public strMoveDir As String         '1)移動方向（コメント）

        Public strbasePoslabel As String    '2)基準計測点ラベル
        Public basePos As CLookPoint        '  移動基準点
        Public gvDirVec As GeoVector        '  移動方向ベクトル
        Public dMove As Double              '3)移動量
        Public strNewPosLabel As String     '4)新計測点ラベル
        Public newPos As CLookPoint         '  移動点
        Public strResult As String          '5)エラー情報
        Public iResult As Integer           '=0:正常、<>0：エラー
    End Structure

    '1行分の実行リスト内容をチェックする
    Private Function checkExecuteList( _
        ByVal strLine As Array, _
        ByRef posListArr As ArrayList _
    ) As Integer
        Dim strCmd As String, num As Integer, iRet As Integer

        checkExecuteList = -1
        num = strLine.Length
        If (num < 5) Then Exit Function
        '2.1 1行分の実行リストの解析

        strCmd = strLine(0)
        Select Case strCmd
            Case "2P"   '2点から
                '起点、終点、移動量、（移動する点、新ラベル）～から、移動点情報を追加する
                iRet = addPosListFor2PLine(strLine, posListArr)
            Case "3P"   '3点の面から
                '点1、点2、点3、移動量、（移動する点、新ラベル）～から、移動点情報を追加する
                iRet = addPosListFor3PLine(strLine, posListArr)
            Case "DP"   '2点を相互に
                '点1、点2、移動量、（新ラベル1、新ラベル2）～から、移動点情報を追加する
                iRet = addPosListForDPLine(strLine, posListArr)
            Case "2C"   '2点の中間点

            Case Else
                Exit Function
        End Select
        checkExecuteList = 0
    End Function

    '2点指定の実行リストから、移動点作成情報を追加する
    Public Function addPosListFor2PLine(ByVal strLine As Array, ByRef posListArr As ArrayList) As Integer
        Dim num As Integer
        Dim strMoveDir As String
        Dim strPos1Label As String, strPos2Label As String
        Dim dMove As Double
        Dim numPos As Integer, ind1 As Long, ind2 As Long, iPos As Long, iRet As Integer
        Dim Pos1 As CLookPoint = Nothing, Pos2 As CLookPoint = Nothing
        Dim gvDirVec As GeoVector = Nothing
        Dim batchPosList As type_batchList

        addPosListFor2PLine = -1
        num = strLine.Length
        If (num < 5) Then Exit Function

        '起点、終点、移動量、（移動する点、新ラベル）～

        strPos1Label = strLine(1)
        strPos2Label = strLine(2)
        dMove = CDbl(strLine(3))
        strMoveDir = strPos1Label + "→" + strPos2Label
        '起点終点をチェックし、移動方向ベクトルgvDirVec を求める

        ind1 = GetPosFromLabelName(CLookPoint.posTypeMode.All, strPos1Label, Pos1)
        ind2 = GetPosFromLabelName(CLookPoint.posTypeMode.All, strPos2Label, Pos2)
        If (ind1 < 0) Or (ind2 < 0) Then
            '起点、終点が存在しない

            With batchPosList
                .bCreate = False
                .strMoveDir = strMoveDir
                .strbasePoslabel = strPos1Label
                .basePos = Nothing
                .gvDirVec = Nothing
                .dMove = dMove
                .strNewPosLabel = ""
                .newPos = Nothing
                .strResult = "2点指定の起点または終点がありません"
                .iResult = 1
            End With
            posListArr.Add(batchPosList)
            Exit Function
        End If
        iRet = getGeoVecBy2Point(Pos1, Pos2, gvDirVec)
        If (iRet <> 0) Then
            '移動方向が決められない

            With batchPosList
                .bCreate = False
                .strMoveDir = strMoveDir
                .strbasePoslabel = strPos1Label + "&" + strPos2Label
                .basePos = Nothing
                .gvDirVec = Nothing
                .dMove = dMove
                .strNewPosLabel = ""
                .newPos = Nothing
                .strResult = "2点から移動方向が決まりません"
                .iResult = 1
            End With
            posListArr.Add(batchPosList)
            Exit Function
        End If

        '1)基準点,方向,移動量,新ラベル
        numPos = Int((num - 4) / 2)
        iPos = 3
        iRet = addPosListForMove(strLine, numPos, iPos, gvDirVec, dMove, strMoveDir, posListArr)

        addPosListFor2PLine = 0
    End Function

    '3点指定の実行リストから、移動点作成情報を追加する
    Public Function addPosListFor3PLine(ByVal strLine As Array, ByRef posListArr As ArrayList) As Integer
        Dim num As Integer
        Dim strMoveDir As String
        Dim strPos1Label As String, strPos2Label As String, strPos3Label As String
        Dim dMove As Double
        Dim numPos As Integer, ind1 As Long, ind2 As Long, ind3 As Long, iPos As Long, iRet As Integer
        Dim Pos1 As CLookPoint = Nothing, Pos2 As CLookPoint = Nothing, Pos3 As CLookPoint = Nothing
        Dim gvDirVec As GeoVector = Nothing
        Dim batchPosList As type_batchList

        addPosListFor3PLine = -1
        num = strLine.Length
        If (num < 5) Then Exit Function

        '点1、点2、点3、移動量、（移動する点、新ラベル）～

        strPos1Label = strLine(1)
        strPos2Label = strLine(2)
        strPos3Label = strLine(3)
        dMove = CDbl(strLine(4))
        strMoveDir = strPos1Label + "：" + strPos2Label + "：" + strPos3Label

        '1. 3点をチェックし、gvDirVec を求める

        ind1 = GetPosFromLabelName(CLookPoint.posTypeMode.All, strPos1Label, Pos1)
        ind2 = GetPosFromLabelName(CLookPoint.posTypeMode.All, strPos2Label, Pos2)
        ind3 = GetPosFromLabelName(CLookPoint.posTypeMode.All, strPos3Label, Pos3)
        If (ind1 < 0) Or (ind2 < 0) Or (ind3 < 0) Then
            '点が存在しない

            With batchPosList
                .bCreate = False
                .strMoveDir = strMoveDir
                .strbasePoslabel = strPos1Label
                .basePos = Nothing
                .gvDirVec = Nothing
                .dMove = dMove
                .strNewPosLabel = ""
                .newPos = Nothing
                .strResult = "3点指定のいづれかの点がありません"
                .iResult = 1
            End With
            posListArr.Add(batchPosList)
            Exit Function
        End If
        '1.1 3点から面の法線ベクトルを得る
        Dim gpPlane As GeoPlane
        Dim gpPos1 As GeoPoint, gpPos2 As GeoPoint, gpPos3 As GeoPoint
        gpPos1 = Pos1.toGeopoint()
        gpPos2 = Pos2.toGeopoint()
        gpPos3 = Pos3.toGeopoint()
        gpPlane = GeoPlane_By3Points(gpPos1, gpPos2, gpPos3)
        gvDirVec = gpPlane.normal
        gvDirVec.Normalize()

        '1)基準点,方向,移動量,新ラベル
        numPos = Int((num - 5) / 2)
        iPos = 4
        iRet = addPosListForMove(strLine, numPos, iPos, gvDirVec, dMove, strMoveDir, posListArr)

        addPosListFor3PLine = 0
    End Function
    '両点指定の実行リストから、移動点作成情報を追加する
    Public Function addPosListForDPLine(ByVal strLine As Array, ByRef posListArr As ArrayList) As Integer
        Dim num As Integer
        Dim strMoveDir As String
        Dim strPos1Label As String, strPos2Label As String
        Dim dMove As Double
        Dim ind1 As Long, ind2 As Long, iPos As Long, iRet As Integer
        Dim Pos1 As CLookPoint = Nothing, Pos2 As CLookPoint = Nothing, basePos As CLookPoint = Nothing
        Dim gvDirVec As GeoVector = Nothing
        Dim batchPosList As type_batchList

        addPosListForDPLine = -1
        num = strLine.Length
        If (num < 5) Then Exit Function

        '点1、点2、移動量、（新ラベル1、新ラベル2）～

        strPos1Label = strLine(1)
        strPos2Label = strLine(2)
        dMove = CDbl(strLine(3))
        '1. 始点から終点方向

        strMoveDir = strPos1Label + "→" + strPos2Label
        '起点終点をチェックし、移動方向ベクトルgvDirVec を求める

        ind1 = GetPosFromLabelName(CLookPoint.posTypeMode.All, strPos1Label, Pos1)
        ind2 = GetPosFromLabelName(CLookPoint.posTypeMode.All, strPos2Label, Pos2)
        If (ind1 < 0) Or (ind2 < 0) Then
            '起点、終点が存在しない

            With batchPosList
                .bCreate = False
                .strMoveDir = strMoveDir
                .strbasePoslabel = strPos1Label
                .basePos = Nothing
                .gvDirVec = Nothing
                .dMove = dMove
                .strNewPosLabel = ""
                .newPos = Nothing
                .strResult = "両端点指定のいづれかの点がありません"
                .iResult = 1
            End With
            posListArr.Add(batchPosList)
            Exit Function
        End If
        iRet = getGeoVecBy2Point(Pos1, Pos2, gvDirVec)
        If (iRet <> 0) Then
            '移動方向が決められない

            With batchPosList
                .bCreate = False
                .strMoveDir = strMoveDir
                .strbasePoslabel = strPos1Label + "&" + strPos2Label
                .basePos = Nothing
                .gvDirVec = Nothing
                .dMove = dMove
                .strNewPosLabel = ""
                .newPos = Nothing
                .strResult = "2点から移動方向が決まりません"
                .iResult = 1
            End With
            posListArr.Add(batchPosList)
            Exit Function
        End If

        '1)基準点,方向,移動量,新ラベル
        Dim strNewPos As String
        iPos = 4
        strNewPos = strLine(iPos)   '新ラベル
        iRet = addPosListForMove2(strPos1Label, strNewPos, gvDirVec, dMove, strMoveDir, posListArr)


        '2. 終点から始点方向___________________________
        strMoveDir = strPos2Label + "→" + strPos1Label
        gvDirVec = Nothing
        iRet = getGeoVecBy2Point(Pos2, Pos1, gvDirVec)

        '2)基準点,方向,移動量,新ラベル
        iPos = 5
        strNewPos = strLine(iPos)   '新ラベル
        iRet = addPosListForMove2(strPos2Label, strNewPos, gvDirVec, dMove, strMoveDir, posListArr)

        addPosListForDPLine = 0
    End Function

    '「基準点,方向,移動量,新ラベル」で移動点作成情報を追加する
    Public Function addPosListForMove( _
        ByVal strLine As Array, _
        ByVal numPos As Long, _
        ByVal iPos As Long, _
        ByVal gvDirVec As GeoVector, _
        ByVal dMove As Double, _
        ByVal strMoveDir As String, _
        ByRef posListArr As ArrayList _
    ) As Integer
        Dim ind1 As Long, iRet As Integer
        Dim strPos As String, strNewPos As String
        Dim batchPosList As type_batchList
        Dim basePos As CLookPoint = Nothing
        Dim gpBasePos As New GeoPoint, cpNewPos As CLookPoint = Nothing

        For i = 1 To numPos
            iPos = iPos + 1
            strPos = strLine(iPos)      '移動基準点
            basePos = Nothing
            ind1 = GetPosFromLabelName(CLookPoint.posTypeMode.All, strPos, basePos)
            If (ind1 < 0) Then
                '移動基準点が存在しない

                With batchPosList
                    .bCreate = False
                    .strMoveDir = strMoveDir
                    .strbasePoslabel = strPos
                    .basePos = Nothing
                    .gvDirVec = Nothing
                    .dMove = dMove
                    .strNewPosLabel = ""
                    .newPos = Nothing
                    .strResult = "移動基準点がありません"
                    .iResult = 1
                End With
                posListArr.Add(batchPosList)
                Exit Function
            End If

            iPos = iPos + 1
            strNewPos = strLine(iPos)   '新ラベル
            '新ラベルのチェック
            ind1 = CheckSamePosLabelData(CLookPoint.posTypeMode.All, strNewPos)
            If (ind1 > 0) Then
                '既にラベルが存在する
                With batchPosList
                    .bCreate = False
                    .strMoveDir = strMoveDir
                    .strbasePoslabel = strPos
                    .basePos = Nothing
                    .gvDirVec = Nothing
                    .dMove = dMove
                    .strNewPosLabel = strNewPos
                    .newPos = Nothing
                    .strResult = "既に作成済みのラベルが指定されています"
                    .iResult = 1
                End With
                posListArr.Add(batchPosList)
                Exit Function
            End If
            gpBasePos = Nothing
            gpBasePos = basePos.toGeopoint()
            cpNewPos = Nothing
            iRet = AddNewUserPointByVector(gpBasePos, gvDirVec, dMove, strNewPos, cpNewPos)
            If (iRet <> 0) Then
                '新規移動点が作成できない

                With batchPosList
                    .bCreate = False
                    .strMoveDir = strMoveDir
                    .strbasePoslabel = strPos
                    .basePos = basePos
                    .gvDirVec = gvDirVec
                    .dMove = dMove
                    .strNewPosLabel = strNewPos
                    .newPos = cpNewPos
                    .strResult = "移動点が作成できません"
                    .iResult = 1
                End With
                posListArr.Add(batchPosList)
                Exit Function
            End If

            With batchPosList
                .bCreate = True
                .strMoveDir = strMoveDir
                .strbasePoslabel = strPos
                .basePos = basePos
                .gvDirVec = gvDirVec
                .dMove = dMove
                .strNewPosLabel = strNewPos
                .newPos = cpNewPos
                .strResult = ""
                .iResult = 0
            End With
            posListArr.Add(batchPosList)
        Next i
    End Function

    '「基準点,方向,移動量,新ラベル」で移動点作成情報を追加する
    Public Function addPosListForMove2( _
        ByVal strPos As String, _
        ByVal strNewPos As String, _
        ByVal gvDirVec As GeoVector, _
        ByVal dMove As Double, _
        ByVal strMoveDir As String, _
        ByRef posListArr As ArrayList _
    ) As Integer
        Dim ind1 As Long, iRet As Integer
        Dim batchPosList As type_batchList
        Dim basePos As CLookPoint = Nothing
        Dim gpBasePos As GeoPoint, cpNewPos As CLookPoint = Nothing

        basePos = Nothing
        ind1 = GetPosFromLabelName(CLookPoint.posTypeMode.All, strPos, basePos)
        If (ind1 < 0) Then
            '移動基準点が存在しない

            With batchPosList
                .bCreate = False
                .strMoveDir = strMoveDir
                .strbasePoslabel = strPos
                .basePos = Nothing
                .gvDirVec = Nothing
                .dMove = dMove
                .strNewPosLabel = ""
                .newPos = Nothing
                .strResult = "移動基準点がありません"
                .iResult = 1
            End With
            posListArr.Add(batchPosList)
            Exit Function
        End If

        '新ラベルのチェック
        ind1 = CheckSamePosLabelData(CLookPoint.posTypeMode.All, strNewPos)
        If (ind1 > 0) Then
            '既にラベルが存在する
            With batchPosList
                .bCreate = False
                .strMoveDir = strMoveDir
                .strbasePoslabel = strPos
                .basePos = Nothing
                .gvDirVec = Nothing
                .dMove = dMove
                .strNewPosLabel = strNewPos
                .newPos = Nothing
                .strResult = "既に作成済みのラベルが指定されています"
                .iResult = 1
            End With
            posListArr.Add(batchPosList)
            Exit Function
        End If
        gpBasePos = Nothing
        gpBasePos = basePos.toGeopoint()
        cpNewPos = Nothing
        iRet = AddNewUserPointByVector(gpBasePos, gvDirVec, dMove, strNewPos, cpNewPos)
        If (iRet <> 0) Then
            '新規移動点が作成できない

            With batchPosList
                .bCreate = False
                .strMoveDir = strMoveDir
                .strbasePoslabel = strPos
                .basePos = basePos
                .gvDirVec = gvDirVec
                .dMove = dMove
                .strNewPosLabel = strNewPos
                .newPos = cpNewPos
                .strResult = "移動点が作成できません"
                .iResult = 1
            End With
            posListArr.Add(batchPosList)
            Exit Function
        End If
        With batchPosList
            .bCreate = True
            .strMoveDir = strMoveDir
            .strbasePoslabel = strPos
            .basePos = basePos
            .gvDirVec = gvDirVec
            .dMove = dMove
            .strNewPosLabel = strNewPos
            .newPos = cpNewPos
            .strResult = ""
            .iResult = 0
        End With
        posListArr.Add(batchPosList)
    End Function

    Private Sub TabPage4_Click(sender As Object, e As System.EventArgs) Handles TabPage4.Click
        '        TabControl1
        Debug.Print("")
    End Sub

    Private Sub TabPage4_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabPage4.LostFocus
        '実行リストにある情報を削除
        '一時的に追加した計測点を削除する
        '(iAddStartIndex + 1)～ nLookPoints を削除する '一括処理で追加する前の座標値数
        If ((gAddStartDrawPointIndex + 1) < nLookPoints) Then

        End If
    End Sub

    Private Sub YCM_CreateMovePos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Dim iRet As Integer
        Dim mid As Long
        '削除された補間点は、ここで削除する
        For ii As Long = 0 To gNumAddMovePos - 1
            If (gAddMovePos(ii).isDelete = True) Then
                '1. 削除する点が他で使われていないかどうかを確認する

                If (isUsedMovePos(ii) = False) Then
                    '2. gAddMovePos(i).newPos　をgDrawPoints()から削除し、nLookPointsをカウントダウン
                    mid = gAddMovePos(ii).newPos.mid
                    iRet = deleteDrawPoint(mid)
                End If
            End If
        Next
    End Sub
    '2. gAddMovePos(i).newPos　をgDrawPoints()から削除し、nLookPointsをカウントダウン
    Private Function deleteDrawPoint(ByVal mid As Long) As Integer
        Dim iDel As Long
        deleteDrawPoint = -1
        iDel = -1
        For ii As Long = 0 To (nLookPoints - 1)
            If (gDrawPoints(ii).mid = mid) Then
                iDel = ii
                Exit For
            End If
        Next
        If (iDel < 0) Then Exit Function

        ' iDel以降を1つずつ詰める
        nLookPoints = nLookPoints - 1
        For ii = iDel To (nLookPoints - 1)
            gDrawPoints(ii) = gDrawPoints(ii + 1)
        Next
        deleteDrawPoint = iDel
    End Function

    '1. 削除する点が他で使われていないかどうかを確認する

    ' =True：他で使っている
    Private Function isUsedMovePos(ByVal ind) As Boolean
        Dim mid As Long, num As Integer
        isUsedMovePos = False
        mid = gAddMovePos(ind).newPos.mid
        For ii As Long = 0 To gNumAddMovePos - 1
            If (ii <> ind) Then
                If (gAddMovePos(ii).isDelete = False) Then
                    num = UBound(gAddMovePos(ii).dirBasePos)
                    For jj As Integer = 0 To num - 1
                        If (mid = gAddMovePos(ii).dirBasePos(jj).mid) Then
                            ' 他で使っている
                            isUsedMovePos = True
                            Exit Function
                        End If
                    Next
                    If (mid = gAddMovePos(ii).basePos.mid) Then
                        ' 他で使っている
                        isUsedMovePos = True
                        Exit Function
                    End If
                End If
            End If
        Next
    End Function

End Class