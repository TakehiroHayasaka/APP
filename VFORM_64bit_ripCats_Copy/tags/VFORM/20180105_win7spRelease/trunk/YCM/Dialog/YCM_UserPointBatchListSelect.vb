Imports System.Windows.Forms
Public Class YCM_UserPointBatchListSelect
    Public m_Return As Boolean

    Private Sub YCM_UserPointBatchListSelect_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TB_Path.Text = MovePointBatchListFileName
    End Sub

    '「処理結果確認」移動点一括処理の仮実行
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        'Me.Close()
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

        '処理結果ダイアログに値を入れる
        Dim frmResult As New YCM_UserPointBatchListResult
        frmResult.DataGridView1.Rows.Clear()
        Dim ii As Long
        If (posListArr.Count > 0) Then
            For ii = 0 To posListArr.Count - 1
                batchPosList = posListArr(ii)
                frmResult.DataGridView1.Rows.Add()
                frmResult.DataGridView1.Rows(ii).Cells(0).Value = batchPosList.bCreate
                frmResult.DataGridView1.Rows(ii).Cells(1).Value = batchPosList.strMoveDir
                frmResult.DataGridView1.Rows(ii).Cells(2).Value = batchPosList.strbasePoslabel
                frmResult.DataGridView1.Rows(ii).Cells(3).Value = batchPosList.dMove
                frmResult.DataGridView1.Rows(ii).Cells(4).Value = batchPosList.strNewPosLabel
                frmResult.DataGridView1.Rows(ii).Cells(5).Value = batchPosList.strResult
                If (batchPosList.iResult <> 0) Then
                    frmResult.DataGridView1.Rows(ii).Cells(0).ReadOnly = True
                End If
            Next
        End If

        '結果一覧の表示
        Me.Hide()
        frmResult.ShowDialog()
        If frmResult.m_Return Then
            '「キャンセル」
            'Me.Show()
        Else
            Me.Close()
            '「実行」
            'batchPosListから新規計測点を追加する
            ''3. 新計測点の作成
            'Dim iPosIndex As Long
            'iPosIndex = AddNewUserPoint(移動点, 新ラベル)

        End If
        m_Return = False
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

    '「キャンセル」
    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
        m_Return = True
    End Sub

    '「選択」実行リストファイルの選択
    Private Sub B_Sel_List_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles B_Sel_List.Click
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

    '「編集」実行リストファイルの編集
    Private Sub B_Edit_List_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles B_Edit_List.Click
        Dim listFile As String
        listFile = TB_Path.Text
        'ファイルが存在するかをチェック
        Dim fileExists As Boolean
        fileExists = bIsListFileExist(listFile)
        If fileExists = False Then Exit Sub
        ' ファイルを指定してメモ帳を起動する
        System.Diagnostics.Process.Start("Notepad", listFile)
    End Sub

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
            Case "2P"
                '起点、終点、移動量、（移動する点、新ラベル）～から、移動点情報を追加する
                iRet = addPosListFor2PLine(strLine, posListArr)
            Case "3P"
                '点1、点2、点3、移動量、（移動する点、新ラベル）～から、移動点情報を追加する
                iRet = addPosListFor3PLine(strLine, posListArr)
            Case "DP"
                '点1、点2、移動量、（新ラベル1、新ラベル2）～から、移動点情報を追加する
                iRet = addPosListForDPLine(strLine, posListArr)
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

End Class
