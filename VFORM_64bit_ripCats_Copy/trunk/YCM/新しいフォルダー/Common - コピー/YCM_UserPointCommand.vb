Imports System.Windows.Forms
Module YCM_UserPointCommand
    Dim m_inputPrevMoveValue As Double = 0.0    '前回入力時の移動量

#If 0 Then
    '＜コマンド＞

    '	・移動方向を2点で指定して移動点を作成
    '		command_NewPointByVec()
    '   ・移動方向を3点で構成する面で指定して移動点を作成
    '		command_NewPointByFace()
    '   ・2点を指定して相互に移動した点を作成
    '		command_NewEachPoint()
    '   ・リストで指定して移動点を一括作成
    '		command_NewPointByList()
    '＜基本関数＞点の座標値は全て無単位

    '	CreateNewUserPointByVec(点1,点2,移動量,移動対象点,新ラベル,新計測点)
    '	CreateNewUserPointByFace(点1,点2,点3,移動量,移動対象点,新ラベル)
    '	CreateNewUserPointEach(点1,点2,移動量,新ラベル1,新ラベル2)
    '＜チェック関連＞

    '   作成する計測点のラベルが既に使われているかをチェック
#End If

    '◆コマンド

    '==========================================================================
    '・移動方向を2点で指定して移動点を作成
    Public Sub command_NewPointByVec()
        Dim ccp1 As New CLookPoint, ccp2 As New CLookPoint, ccpBase As New CLookPoint
        Dim ccpNew As CLookPoint = Nothing
        Dim dMove As Double, strNewLabel As String = ""
        Dim iLoop As Long, iRet As Integer

        '1. 移動方向を示す2点を入力

        If IOUtil.GetPoint(ccp1, "始点を指示：") = -1 Then GoTo Exit_lbl 'Exit Sub
        If (ccp1.mid < 0) Then GoTo Exit_lbl 'Exit Sub
        If IOUtil.GetPoint(ccp2, "終点を指示：") = -1 Then GoTo Exit_lbl 'Exit Sub
        If (ccp2.mid < 0) Then GoTo Exit_lbl 'Exit Sub

        '2. 移動量を入力（前回値を表示）m_inputPrevMoveValue
        If (inputMoveValue(dMove) = -1) Then GoTo Exit_lbl 'Exit Sub

        iLoop = 0
        '3. 処理を繰り返し
        While True
            '3.1 移動対象点の入力（初回省略時は点2から作成）

            '--rep.12.8.28            If IOUtil.GetPoint(ccpBase, "移動対象点を指示：") = -1 Then Exit Sub
            If IOUtil.GetPoint(ccpBase, "移動対象点を指示：") = -1 Then Exit While

            If (ccpBase.mid < 0) Then
                If (iLoop = 0) Then
                    ccpBase = ccp2
                Else
                    Exit While
                End If
            End If
            '3.2 新ラベルの表示・入力

            If (GetNewLabel(ccpBase, strNewLabel) = -1) Then GoTo Exit_lbl 'Exit Sub
            '3.3 移動点の作成
            iRet = CreateNewUserPointByVec(ccp1, ccp2, dMove, ccpBase, strNewLabel, ccpNew)
            iLoop = iLoop + 1
        End While
Exit_lbl:
        IOUtil.WriteCommandLine("")
        command_CreateMovePos()
    End Sub

    '・移動方向を3点で構成する面で指定して移動点を作成
    Public Sub command_NewPointByFace()
        Dim ccp1 As New CLookPoint, ccp2 As New CLookPoint, ccp3 As New CLookPoint, ccpBase As New CLookPoint
        Dim ccpNew As CLookPoint = Nothing
        Dim dMove As Double, strNewLabel As String = ""
        Dim iLoop As Long, iRet As Integer

        '1. 移動方向を決める3点を入力

        If IOUtil.GetPoint(ccp1, "点1を指示：") = -1 Then GoTo Exit_lbl 'Exit Sub
        If (ccp1.mid < 0) Then GoTo Exit_lbl 'Exit Sub
        If IOUtil.GetPoint(ccp2, "点2を指示：") = -1 Then GoTo Exit_lbl 'Exit Sub
        If (ccp2.mid < 0) Then GoTo Exit_lbl 'Exit Sub
        If IOUtil.GetPoint(ccp3, "点3を指示：") = -1 Then GoTo Exit_lbl 'Exit Sub
        If (ccp3.mid < 0) Then GoTo Exit_lbl 'Exit Sub

        '2. 移動量を入力（前回値を表示）m_inputPrevMoveValue
        If (inputMoveValue(dMove) = -1) Then GoTo Exit_lbl 'Exit Sub

        '3. 面を構成する3点も移動するか確認（デフォルト"N"）

        ' 3.1 面を構成する3点から移動点を作成、新ラベルは？？？


        iLoop = 0
        '4. 処理を繰り返し
        While True
            '4.1 移動対象点の入力（初回省略時は点2から作成）

            '--rep.12.8.29            If IOUtil.GetPoint(ccpBase, "移動対象点を指示：") = -1 Then Exit Sub
            If IOUtil.GetPoint(ccpBase, "移動対象点を指示：") = -1 Then Exit While
            If (ccpBase.mid < 0) Then
                If (iLoop = 0) Then
                    ccpBase = ccp2
                Else
                    Exit While
                End If
            End If
            '4.2 新ラベルの表示・入力

            '--rep.12.8.29            If (GetNewLabel(ccpBase, strNewLabel) = -1) Then Exit Sub
            If (GetNewLabel(ccpBase, strNewLabel) = -1) Then Exit While
            '4.3 移動点の作成
            iRet = CreateNewUserPointByFace(ccp1, ccp2, ccp3, dMove, ccpBase, strNewLabel, ccpNew)
            iLoop = iLoop + 1
        End While
Exit_lbl:
        IOUtil.WriteCommandLine("")
        command_CreateMovePos()
    End Sub

    '・2点を指定して相互に移動した点を作成
    Public Sub command_NewEachPoint()
        Dim ccp1 As New CLookPoint, ccp2 As New CLookPoint
        Dim ccpNew1 As CLookPoint = Nothing
        Dim ccpNew2 As CLookPoint = Nothing
        Dim dMove As Double
        Dim strNewLabel1 As String = ""
        Dim strNewLabel2 As String = ""
        Dim iLoop As Long, iRet As Integer

        iLoop = 0
        '1. 処理を繰り返し
        While True
            '1.1 移動対象の2点を入力

            If IOUtil.GetPoint(ccp1, "始点を指示：") = -1 Then Exit While
            If (ccp1.mid < 0) Then Exit While
            If IOUtil.GetPoint(ccp2, "終点を指示：") = -1 Then Exit While
            If (ccp2.mid < 0) Then Exit While

            '1.2 移動量を入力（前回値を表示）m_inputPrevMoveValue
            If (inputMoveValue(dMove) = -1) Then Exit While
            '1.3 新ラベルの表示・入力

            If (GetNewLabel(ccp1, strNewLabel1) = -1) Then Exit While
            If (GetNewLabel(ccp2, strNewLabel2) = -1) Then Exit While

            '1.4 移動点の作成
            iRet = CreateNewUserPointEach(ccp1, ccp2, dMove, strNewLabel1, strNewLabel2, ccpNew1, ccpNew2)

            iLoop = iLoop + 1
        End While
        IOUtil.WriteCommandLine("")
        command_CreateMovePos()
    End Sub

    '・2点を指定して2点の中間点を作成
    Public Sub command_NewPointCenter()
        Dim ccp1 As New CLookPoint, ccp2 As New CLookPoint
        Dim ccpNew1 As CLookPoint = Nothing
        Dim strNewLabel1 As String = ""
        Dim iLoop As Long, iRet As Integer

        iLoop = 0
        '1. 処理を繰り返し
        While True
            '1.1 移動対象の2点を入力

            If IOUtil.GetPoint(ccp1, "始点を指示：") = -1 Then Exit While
            If (ccp1.mid < 0) Then Exit While
            If IOUtil.GetPoint(ccp2, "終点を指示：") = -1 Then Exit While
            If (ccp2.mid < 0) Then Exit While

            '1.2 移動量を入力（前回値を表示）m_inputPrevMoveValue

            '1.3 新ラベルの表示・入力

            If (GetNewLabel(ccp1, strNewLabel1) = -1) Then Exit While

            '1.4 中間点の作成
            iRet = CreateNewUserPointCenter(ccp1, ccp2, strNewLabel1, ccpNew1)

            iLoop = iLoop + 1
        End While
        IOUtil.WriteCommandLine("")
        command_CreateMovePos()
    End Sub

    '・リストで指定して移動点を一括作成 →8/28削除する
    Public Sub command_NewPointByList()
        '1. 移動点一括処理ダイアログを表示、実行リストファイルを取得

        '2. 移動点の仮作成処理

        '3. 処理結果確認ダイアログの表示
        Dim selfrm As New YCM_UserPointBatchListSelect
        'selfrm.TB_Path.Text = MovePointBatchListFileName
        selfrm.ShowDialog()
        'If selfrm.m_Return Then
        '    selfrm.Close()
        'Else
        '    selfrm.Show()
        'End If
    End Sub

    '--ins.start--------------------------12.8.28 k.y
    '移動点作成メインダイアログ
    Public Sub command_CreateMovePos()
        '1. 移動点作成ダイアログを表示
        Dim selfrm As New YCM_CreateMovePos
        selfrm.ShowDialog()
        'If selfrm.m_Return Then
        '    selfrm.Close()
        'Else
        '    selfrm.Show()
        'End If
    End Sub


#If 0 Then '__________________________________________________
        Call memoStart()
    Private Sub memoStart()
        ' メモ帳を起動する

        System.Diagnostics.Process.Start("Notepad")
        ' ファイルを指定してメモ帳を起動する

        System.Diagnostics.Process.Start("Notepad", "C:\Hoge.txt")
        ' 規定のエディタで Bitmap ファイルを開く (関連付け起動)
        System.Diagnostics.Process.Start("C:\Hoge.txt")
    End Sub
    Private Sub fileselect()
        ' OpenFileDialog の新しいインスタンスを生成する (デザイナから追加している場合は必要ない)
        Dim OpenFileDialog1 As New OpenFileDialog()

        OpenFileDialog1.Title = "移動点作成実行リストファイルの選択"
        OpenFileDialog1.InitialDirectory = "C:\"    '初期表示ディレクトリ
        OpenFileDialog1.FileName = "初期表示するファイル名をココに書く"
        OpenFileDialog1.Filter = "テキスト ファイル|*.txt;*.log|すべてのファイル|*.*"
        OpenFileDialog1.FilterIndex = 2     'ファイル種類の設定（初期値 1）

        OpenFileDialog1.RestoreDirectory = True     'ダイアログボックスを閉じる前に現在のディレクトリを復元（初期値 False）

        OpenFileDialog1.Multiselect = False      '複数ファイル選択

        OpenFileDialog1.ShowHelp = False         '「ヘルプ」ボタン表示
        OpenFileDialog1.ShowReadOnly = False     '「読み取り専用」チェックボックスの表示
        OpenFileDialog1.ReadOnlyChecked = False   '「読み取り専用」チェック   

        ' 存在しないファイルを指定した場合は警告を表示する (初期値 True)
        'OpenFileDialog1.CheckFileExists = True
        ' 存在しないパスを指定した場合は警告を表示する (初期値 True)
        'OpenFileDialog1.CheckPathExists = True
        ' 拡張子を指定しない場合は自動的に拡張子を付加する (初期値 True)
        'OpenFileDialog1.AddExtension = True
        ' 有効な Win32 ファイル名だけを受け入れるようにする (初期値 True)
        'OpenFileDialog1.ValidateNames = True

        ' ダイアログを表示し、戻り値が [OK] の場合は、選択したファイルを表示する
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            MessageBox.Show(OpenFileDialog1.FileName)
        End If
        ' 不要になった時点で破棄する (正しくは オブジェクトの破棄を保証する を参照)
        OpenFileDialog1.Dispose()
    End Sub
#End If '_____________________________________________________

#If 0 Then '__________________________________________________
    '===============================================================================
    ' 機能：指定された実行リストファイルから処理結果を作成する    
    '       MovePointBatchListFileName「実行リストファイル」

    ' 引数： strBatchListFName [I/ ] 実行リストファイル名

    '           処理数
    '           基準計測点
    '           移動量
    '           新計測点
    '           実行結果
    ' 戻り値：

    '===============================================================================
    Public Function calcNewPointBatch( _
        ByVal strBatchListFName As String, _
        ByRef result As Integer _
    ) As Integer
        '1.実行リストファイルの存在チェック
        '2.実行リストファイル内容のチェック
        '3.実行リストファイル
    End Function

    ' 機能：実行ファイルリストの内容を解析

    Public Function devideListFileItem() As Integer
        'MovePointBatchListFileName
        '方法、点１、点２、点３、移動量、（移動点、新ラベル）繰り返し
    End Function

    '指定された計測ラベルから計測点を得る
    'GetPosFromLabelName
    '新計測点のチェック（既に存在しているかをチェック）

#End If '_____________________________________________________



    '===============================================================================
    '◆基本関数
    '===============================================================================
    '・移動量の入力（前回値を表示）m_inputPrevMoveValue
    Public Function inputMoveValue(ByRef dMove As Double) As Integer
        Dim strMoveVal As String, strPrompt As String
        '2. 移動量を入力（前回値を表示）m_inputPrevMoveValue
        strMoveVal = CStr(m_inputPrevMoveValue)
        strPrompt = "移動量：" + strMoveVal
Label_1:
        If IOUtil.GetString(strMoveVal, strPrompt) = -1 Then Exit Function

        If (Len(strMoveVal) < 1) Then GoTo Label_1
        Try
            dMove = CDbl(strMoveVal)
        Catch ex As Exception
            '移動量は実数値で入力してください
            IOUtil.WritePrompt("移動量は実数値で入力してください")
            GoTo Label_1
        End Try
    End Function

    '・新ラベルの表示、入力

    Public Function GetNewLabel(ByVal cp As CLookPoint, ByRef strLabel As String) As Integer
        'cp点のラベルを表示して、新ラベルを入力する

        Dim strPromptHead As String = "ラベル："
        Dim prompt As String, ラベル名 As String = ""
        Dim strMsg As String
        Dim ii As Integer, iLen As Integer
        prompt = strPromptHead
        If (getLabelNameByMID(cp.mid, ラベル名) > 0) Then
            prompt += ラベル名

        End If
label_1:
        If IOUtil.GetString(strLabel, prompt) = -1 Then
            Exit Function
        End If
        If (Len(strLabel) < 1) Then GoTo Label_1
        '入力されたラベルの整理

        ii = InStr(strLabel, strPromptHead)
        If (ii > 0) Then
            iLen = Len(strLabel)
            ii = ii + Len(strPromptHead)
            If ((iLen - ii + 1) > 0) Then
                strLabel = Mid(strLabel, ii, (iLen - ii + 1))
            Else
            End If
        Else
            strLabel = ラベル名 + strLabel
        End If
        '既に使われているラベルかどうかをチェック
        If (CheckSamePosLabelData(CLookPoint.posTypeMode.All, strLabel) >= 0) Then
            strMsg = "ラベル[" & strLabel & "]は既に使われています。"
            '-            IOUtil.WriteCommandLine(strMsg)
            MsgBox(strMsg, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
            GoTo Label_1
        End If
    End Function
    '機能：点1から点2に向かう単位方向ベクトルを得る
    Public Function getGeoVecBy2Point( _
        ByVal 点1 As CLookPoint, ByVal 点2 As CLookPoint, _
        ByRef gvVec As GeoVector _
    ) As Integer
        '1. 点1から点2への単位方向ベクトルを得る
        Dim gpPos1 As GeoPoint, gpPos2 As GeoPoint
        gpPos1 = 点1.toGeopoint()
        gpPos2 = 点2.toGeopoint()
        gvVec = gpPos2.GetSubtracted(gpPos1)
        gvVec.Normalize()
        getGeoVecBy2Point = 0
    End Function

    '・移動方向を2点で指定して移動点を作成
    Public Function CreateNewUserPointByVec( _
        ByVal 点1 As CLookPoint, ByVal 点2 As CLookPoint, _
        ByVal 移動量 As Double, _
        ByVal 移動対象点 As CLookPoint, _
        ByVal 新ラベル As String, _
        ByRef 新計測点 As CLookPoint _
    ) As Integer
        CreateNewUserPointByVec = -1
        '1. 点1から点2への単位方向ベクトルを得る
        Dim gvVec As GeoVector
        Dim gpPos1 As GeoPoint, gpPos2 As GeoPoint
        gpPos1 = 点1.toGeopoint()
        gpPos2 = 点2.toGeopoint()
        gvVec = gpPos2.GetSubtracted(gpPos1)
        gvVec.Normalize()
        '2. 新計測点の作成
        Dim iRet As Integer
        Dim gpPos As GeoPoint
        gpPos = 移動対象点.toGeopoint()
        iRet = AddNewUserPointByVector(gpPos, gvVec, 移動量, 新ラベル, 新計測点)
        '--ins.start----------------------------12.8.28 k,y    
        If (iRet = 0) Then
            ReDim Preserve gAddMovePos(gNumAddMovePos + 1)
            ReDim gAddMovePos(gNumAddMovePos).dirBasePos(2)
            gAddMovePos(gNumAddMovePos).iType = 2
            gAddMovePos(gNumAddMovePos).dirBasePos(0) = 点1
            gAddMovePos(gNumAddMovePos).dirBasePos(1) = 点2
            gAddMovePos(gNumAddMovePos).dMoveValue = 移動量
            gAddMovePos(gNumAddMovePos).basePos = 移動対象点
            gAddMovePos(gNumAddMovePos).newPos = 新計測点
            gAddMovePos(gNumAddMovePos).strResult = "正常"
            gNumAddMovePos = gNumAddMovePos + 1
        End If
        '--ins.end------------------------------12.8.28 k,y    
        CreateNewUserPointByVec = iRet
    End Function

    '・移動方向を3点で構成する面で指定して移動点を作成
    '　CreateNewUserPointByFace(↓点1,↓点2,↓点3,↓移動量,↓移動対象点,↓新ラベル,↑新計測点)
    Public Function CreateNewUserPointByFace( _
        ByVal 点1 As CLookPoint, ByVal 点2 As CLookPoint, ByVal 点3 As CLookPoint, _
        ByVal 移動量 As Double, _
        ByVal 移動対象点 As CLookPoint, _
        ByVal 新ラベル As String, _
        ByRef 新計測点 As CLookPoint _
    ) As Integer
        CreateNewUserPointByFace = -1
        '1. 点1,2,3から面の法線ベクトルを得る
        Dim gvVec As GeoVector
        Dim gpPlane As GeoPlane
        Dim gpPos1 As GeoPoint, gpPos2 As GeoPoint, gpPos3 As GeoPoint
        gpPos1 = 点1.toGeopoint()
        gpPos2 = 点2.toGeopoint()
        gpPos3 = 点3.toGeopoint()
        '1.1 3点から面の法線ベクトルを得る
        gpPlane = GeoPlane_By3Points(gpPos1, gpPos2, gpPos3)
        gvVec = gpPlane.normal
        gvVec.Normalize()
        '3. 新計測点の作成
        Dim iRet As Integer
        Dim gpPos As GeoPoint
        gpPos = 移動対象点.toGeopoint()
        iRet = AddNewUserPointByVector(gpPos, gvVec, 移動量, 新ラベル, 新計測点)
        '--ins.start----------------------------12.8.28 k,y    
        If (iRet = 0) Then
            ReDim Preserve gAddMovePos(gNumAddMovePos + 1)
            ReDim gAddMovePos(gNumAddMovePos).dirBasePos(3)
            gAddMovePos(gNumAddMovePos).iType = 3
            gAddMovePos(gNumAddMovePos).dirBasePos(0) = 点1
            gAddMovePos(gNumAddMovePos).dirBasePos(1) = 点2
            gAddMovePos(gNumAddMovePos).dirBasePos(2) = 点3
            gAddMovePos(gNumAddMovePos).dMoveValue = 移動量
            gAddMovePos(gNumAddMovePos).basePos = 移動対象点
            gAddMovePos(gNumAddMovePos).newPos = 新計測点
            gAddMovePos(gNumAddMovePos).strResult = "正常"
            gNumAddMovePos = gNumAddMovePos + 1
        End If
        '--ins.end------------------------------12.8.28 k,y    
        CreateNewUserPointByFace = iRet
    End Function

    '・2点を指定して相互に移動した点を作成
    '	CreateNewUserPointEach(↓点1,↓点2,↓移動量,↓新ラベル1,↓新ラベル2,↑新計測点1,↑新計測点2)
    Public Function CreateNewUserPointEach( _
        ByVal 点1 As CLookPoint, ByVal 点2 As CLookPoint, _
        ByVal 移動量 As Double, _
        ByVal 新ラベル1 As String, _
        ByVal 新ラベル2 As String, _
        ByRef 新計測点1 As CLookPoint, _
        ByRef 新計測点2 As CLookPoint _
    ) As Integer
        CreateNewUserPointEach = -1
    End Function

    '・2点で指定された中間点を作成
    Public Function CreateNewUserPointCenter( _
        ByVal 点1 As CLookPoint, ByVal 点2 As CLookPoint, _
        ByVal 新ラベル As String, _
        ByRef 新計測点 As CLookPoint _
    ) As Integer
        CreateNewUserPointCenter = -1
        '1. 点1から点2への単位方向ベクトルを得る
        Dim gvVec As GeoVector
        Dim gpPos1 As GeoPoint, gpPos2 As GeoPoint
        gpPos1 = 点1.toGeopoint()
        gpPos2 = 点2.toGeopoint()
        gvVec = gpPos2.GetSubtracted(gpPos1)
        gvVec.Normalize()
        '2. 新計測点の作成
        Dim iRet As Integer
        Dim 移動量 As Double
        If (Sys_Setting.sys_ScaleInfo.scale > 0.0) Then
            移動量 = (gpPos1.GetDistanceTo(gpPos2) * 0.5) * Sys_Setting.sys_ScaleInfo.scale
        Else
            移動量 = (gpPos1.GetDistanceTo(gpPos2) * 0.5)
        End If
        iRet = AddNewUserPointByVector(gpPos1, gvVec, 移動量, 新ラベル, 新計測点)
        '--ins.start----------------------------12.8.28 k,y    
        If (iRet = 0) Then
            ReDim Preserve gAddMovePos(gNumAddMovePos + 1)
            ReDim gAddMovePos(gNumAddMovePos).dirBasePos(2)
            gAddMovePos(gNumAddMovePos).iType = 4
            gAddMovePos(gNumAddMovePos).dirBasePos(0) = 点1
            gAddMovePos(gNumAddMovePos).dirBasePos(1) = 点2
            gAddMovePos(gNumAddMovePos).dMoveValue = 移動量
            gAddMovePos(gNumAddMovePos).basePos = 点1
            gAddMovePos(gNumAddMovePos).newPos = 新計測点
            gAddMovePos(gNumAddMovePos).strResult = "正常"
            gNumAddMovePos = gNumAddMovePos + 1
        End If
        '--ins.end------------------------------12.8.28 k,y    
        CreateNewUserPointCenter = iRet
    End Function


    '◆基本関数（共通）

    ' 機能：指定点から方向ベクトルと移動量を指定して新規計測点を作成
    Public Function AddNewUserPointByVector( _
        ByVal 基準点 As GeoPoint, _
        ByVal 方向ベクトル As GeoVector, _
        ByVal 移動量 As Double, _
        ByVal 新ラベル As String, _
        ByRef 新計測点 As CLookPoint _
    ) As Integer
        AddNewUserPointByVector = -1
        '2. 移動量を考慮した方向ベクトルを計算し新計測点座標を求める

        Dim 移動点 As GeoPoint
        方向ベクトル.Normalize()
        If (Sys_Setting.sys_ScaleInfo.scale > 0.0) Then
            方向ベクトル.Multiple(移動量 / Sys_Setting.sys_ScaleInfo.scale)
        Else
            方向ベクトル.Multiple(移動量)
        End If
        移動点 = 基準点.GetAddedVec(方向ベクトル)

        '3. 新計測点の作成
        Dim iPosIndex As Long
        iPosIndex = AddNewUserPoint(移動点, 新ラベル)
        If (iPosIndex >= 0) Then
            新計測点 = gDrawPoints(iPosIndex)
            AddNewUserPointByVector = 0
        Else
            新計測点 = Nothing
        End If
    End Function

    ' 機能：新しい計測点の作成
    ' 引数：

    '   座標点  [I/ ] 作成する計測点の座標値（無単位）

    '   ラベル  [I/ ] 作成する計測点のラベル
    ' 戻り値：=-1：エラー、>=0：作成した計測点のインデックス
    Public Function AddNewUserPoint(ByVal 座標点 As GeoPoint, ByVal ラベル As String) As Integer
        AddNewUserPoint = -1
        Dim maxMID As Long = 0
        If (nLookPoints > 0) Then
            maxMID = gDrawPoints(nLookPoints - 1).mid
            '[measurepoint3d]テーブルから[ID]の最大値を取得

        End If
        ReDim Preserve gDrawPoints(nLookPoints)
        gDrawPoints(nLookPoints) = New CLookPoint(座標点.x, 座標点.y, 座標点.z)
        gDrawPoints(nLookPoints).mid = maxMID + 1
        gDrawPoints(nLookPoints).tid = 0    '[TID]は０で設定

        gDrawPoints(nLookPoints).LabelName = ラベル
        gDrawPoints(nLookPoints).sortID = nLookPoints
        gDrawPoints(nLookPoints).posType = CLookPoint.posTypeMode.User
        gDrawPoints(nLookPoints).mode = 2   '追加点
        gDrawPoints(nLookPoints).type = 9   'ユーザ点
        gDrawPoints(nLookPoints).flgLabel = 1   '代表点
        gDrawPoints(nLookPoints).createType = 1 '20170221 baluu add

        Dim gmat As New GeoMatrix
        Call matToGeoMat(gmat)
        座標点.Transform(gmat)
        gDrawPoints(nLookPoints).Real_x = 座標点.x * sys_ScaleInfo.scale
        gDrawPoints(nLookPoints).Real_y = 座標点.y * sys_ScaleInfo.scale
        gDrawPoints(nLookPoints).Real_z = 座標点.z * sys_ScaleInfo.scale
        AddNewUserPoint = nLookPoints
        nLookPoints = nLookPoints + 1

#If 1 Then  '-----------------------------
        'ラベルリストに作成点を追加
        ReDim Preserve gDrawLabelText(nLabelText)
        gDrawLabelText(nLabelText) = New CLabelText(座標点.x, 座標点.y, 座標点.z)
        gDrawLabelText(nLabelText).mid = maxMID + 1
        gDrawLabelText(nLabelText).LabelName = ラベル
        nLabelText = nLabelText + 1

        '座標値リストに作成点を追加
        Call addPointDGV()

        'カレントデータベースにデータを保存

        '「measurepoint3d」テーブルに計測点を追加
        '  Call YCM_AppendUserPoint(m_strDataBasePath, gDrawPoints(nLookPoints-1))
#End If
    End Function
    Public Function YCM_AppendUserPoint(ByVal strDBPath As String, ByVal cp As CLookPoint) As Integer
        'Dim strSQL As String, ii As Long
        'Dim blnRun As Boolean
        'Dim clsOPe As New CDBOperate
        YCM_AppendUserPoint = -1
        'If (saIDArr.Size < 1) Then Exit Function
        'If (saLabelArr.Size < 1) Then Exit Function
        'If (saIDArr.Size <> saLabelArr.Size) Then Exit Function
        'If clsOPe.ConnectDB(strDBPath) = False Then
        '    YCM_AppendUserPoint = -1
        'End If
        'For ii = 0 To saIDArr.Size - 1
        '    strSQL = "UPDATE measurepoint3d set " & _
        '            "userlabel = '" & saLabelArr.at(ii) & _
        '            "',currentlabel = '" & saLabelArr.at(ii) & _
        '            "' WHERE ID = " & CLng(saIDArr.at(ii))
        '    blnRun = clsOPe.ExcuteSQL(strSQL)
        'Next
        'clsOPe.DisConnectDB()
#If 0 Then
        ID
    	TID
    	Type=999
    	systemlabel=""
    	userlabel=""
    	currentlabel=userlabel
    	X
        Y
    	Z
    	meanerror=0
    	deverror=0
    	flgDisplay=1
    	flgLabel=1

        strSQL = "SELECT MAX(ID) AS MAXID FROM measurepoint3d;"
        adoRet = clsOPe.CreateRecordset(strSQL)
        If Not UtilAdo_IsZeroSize(adoRecSet) Then MaxID = UtilAdo_GetLong(adoRecSet, "MAXID") + 1


        '新規計測点の追加
        strSQL = "INSERT INTO measurepoint3d (" & _
                 "ID, TID, Type, systemlabel, userlabel, currentlabel, X, Y, Z" & _
                 ", meanerror, deverror, flgDisplay, flgLabel" & _
                 ") VALUES(" & _
                 scalevalue & _
                 ")"
        adoRet = clsOPe.CreateRecordset(strSQL)


        strSQL = "SELECT * FROM measurepoint3d where flgLabel=1"
        adoRet = clsOPe.CreateRecordset(strSQL)
        If adoRet.RecordCount > 0 Then
            Do Until adoRet.EOF
                ReDim Preserve gDrawPoints(nLookPoints)
                gDrawPoints(nLookPoints) = New CLookPoint(adoRet("X").Value, adoRet("Y").Value, adoRet("Z").Value)
                gDrawPoints(nLookPoints).mid = adoRet("ID").Value
                gDrawPoints(nLookPoints).tid = adoRet("TID").Value
                gDrawPoints(nLookPoints).LabelName = adoRet("currentlabel").Value
                gDrawPoints(nLookPoints).sortID = nLookPoints
                nLookPoints = nLookPoints + 1
                adoRet.MoveNext()
            Loop
        End If
        YCM_UpDateLookPointReal()
    '-----------------------------------------
        Dim clsOPe As New CDBOperate
        Dim adoRet As ADODB.Recordset
        If clsOPe.ConnectDB(strDBPath) = False Then
            'MsgBox("データベースを開くことができません。")
            YCM_UpdataSystemscalevalueAcs = -1
        End If
        Dim strSQL As String
        strSQL = "DELETE * FROM scalevalue"
        clsOPe.ExcuteSQL(strSQL)
        strSQL = "INSERt INTO scalevalue(スケール値) VALUES(" & scalevalue & ")"
        adoRet = clsOPe.CreateRecordset(strSQL)
        clsOPe.DisConnectDB()
#End If
        YCM_AppendUserPoint = 0
    End Function

    Private Sub addPointDGV()
        Dim ind As Long
        Dim value As Boolean
        value = Control.CheckForIllegalCrossThreadCalls
        Control.CheckForIllegalCrossThreadCalls = False
        Data_Point.DGV_DV.AllowUserToAddRows = True
        Data_Point.DGV_DV.Rows.Add()
        Data_Point.DGV_DV.AllowUserToAddRows = False
        ind = Data_Point.DGV_DV.Rows.Count - 1
        Data_Point.DGV_DV.Rows(ind).Cells(0).Value = True
        Data_Point.DGV_DV.Rows(ind).Cells(1).Value = gDrawPoints(nLookPoints - 1).LabelName
        Data_Point.DGV_DV.Rows(ind).Cells(2).Value = gDrawPoints(nLookPoints - 1).Real_x
        Data_Point.DGV_DV.Rows(ind).Cells(3).Value = gDrawPoints(nLookPoints - 1).Real_y
        Data_Point.DGV_DV.Rows(ind).Cells(4).Value = gDrawPoints(nLookPoints - 1).Real_z
        Data_Point.DGV_DV.Rows(ind).Cells(5).Value = gDrawPoints(nLookPoints - 1).mid
        Control.CheckForIllegalCrossThreadCalls = value
    End Sub

    Public Sub matToGeoMat(ByRef gmat As GeoMatrix)
        'sys_CoordInfo.matから、GeoMatrixを得る
        Dim ind As Integer, i As Integer, j As Integer
        ind = -1
        For i = 1 To 4
            For j = 1 To 4
                ind = ind + 1
                Call gmat.SetAt(i, j, sys_CoordInfo.mat(ind))
            Next j
        Next i
    End Sub

    '＜チェック関連＞

    ' 機能：計測点ラベルと同じデータが、追加された計測点データ内に存在するかをチェックする
    ' 引数：

    '   posType     [I/ ] 調べる計測点タイプ

    '   strPosLabel [I/ ] ラベル
    ' 戻り値：=-1：なし、>=0：あり

    Public Function CheckSamePosLabelData( _
        ByVal posType As CLookPoint.posTypeMode, _
        ByVal strPosLabel As String _
    ) As Integer
        CheckSamePosLabelData = -1
        For i As Long = 0 To (nLookPoints - 1)
            '追加された計測点のラベルについて同じラベルがないかチェックする
            If ((posType = CLookPoint.posTypeMode.All) Or _
                (posType = gDrawPoints(i).posType)) Then
                If (gDrawPoints(i).LabelName = strPosLabel) Then
                    CheckSamePosLabelData = i
                    Exit Function
                End If
            End If
        Next
    End Function

    ' 機能：計測点ラベルを指定して計測点を得る
    ' 引数：

    '   posType     [I/ ] 調べる計測点タイプ

    '   strPosLabel [I/ ] ラベル
    '   Pos         [ /O] 得られた計測点
    ' 戻り値：=-1：なし、>=0：あり

    Public Function GetPosFromLabelName( _
        ByVal posType As CLookPoint.posTypeMode, _
        ByVal strPosLabel As String, _
        ByRef Pos As CLookPoint _
    ) As Integer
        GetPosFromLabelName = -1
        Dim iIndex As Long
        '指定されたラベルを持つ計測点を得る
        iIndex = CheckSamePosLabelData(posType, strPosLabel)
        If (iIndex >= 0) Then
            Pos = New CLookPoint
            Pos = gDrawPoints(iIndex)
            GetPosFromLabelName = iIndex
            Exit Function
        End If
    End Function

    'MIDを指定してラベルを得る
    Private Function getLabelNameByMID(ByVal MID As Long, ByRef ラベル As String) As Long
        Dim ii As Long
        getLabelNameByMID = -1
        ラベル = ""
        If nLabelText > 0 Then
            For ii = 0 To nLabelText - 1
                If (gDrawLabelText(ii).mid = MID) Then
                    ラベル = gDrawLabelText(ii).LabelName
                    getLabelNameByMID = ii
                    Exit Function
                End If
            Next
        End If
    End Function
End Module
