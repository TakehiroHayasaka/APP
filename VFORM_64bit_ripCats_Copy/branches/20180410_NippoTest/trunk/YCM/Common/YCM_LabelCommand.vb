Module YCM_LabelCommand
    Dim m_inputPrevLabelText As String = ""     '前回入力時のラベル

#Region "旧テストコード部分"
    Public Sub command_ChangeLabelSingle()
        Dim cp1 As New CLookPoint
        ChangeStart = 1
        Dim LabelNewName As String = ""
        While True
            If IOUtil.GetPoint(cp1, "計測点を指示：") = -1 Then Exit Sub
Label_1:
            If IOUtil.GetString(LabelNewName, "ラベル：") = -1 Then Exit Sub
            If (Len(LabelNewName) < 1) Then GoTo Label_1
            gDrawLabelText(cp1.sortID).LabelName = LabelNewName
            gDrawPoints(cp1.sortID).LabelName = LabelNewName
            Data_Point.DGV_DV.Rows(cp1.sortID).Cells(1).Value = LabelNewName
        End While
    End Sub

    '旧「手動ラベリング」
    Public Sub command_ChangeLabel_1()
        Dim strLabel As String = ""
        Dim strNo As String = ""
        Dim strLabelHead As String = ""
        Dim strLabelNew As String = ""
        Dim saStr As StringArray
        Dim cp1 As New CLookPoint
        Dim iNo As Long, iType As Integer
        '連番でラベルを設定する
        '最初のラベル文字を入力
Label_1:
        If IOUtil.GetString(strLabel, "最初のラベル：") = -1 Then Exit Sub
        If (Len(strLabel) < 1) Then GoTo Label_1
        '入力されたラベル文字から英字と数字を分離
        saStr = Util_GetDigitSplittedString(strLabel, iType)
        If (saStr.Size < 2) Then
            strLabelHead = strLabel
Label_2:
            If IOUtil.GetString(strNo, "最初の番号：") = -1 Then Exit Sub
            Try
                iNo = CLng(strNo)
            Catch ex As Exception
                '数字で入力してください
                IOUtil.WritePrompt("数値を入力してください")
                GoTo Label_2
            End Try
        Else
            strLabelHead = saStr.Front
            Try
                iNo = CLng(saStr.Back)
            Catch ex As Exception
                '英字＋数字で入力してください
                IOUtil.WritePrompt("英字＋数値で入力してください")
                GoTo Label_1
            End Try
        End If
        '変更する計測点を指示
        Dim strIDArr As New StringArray
        Dim strLabelArr As New StringArray
        While True
            If IOUtil.GetPoint(cp1, "変更する計測点を指示：") = -1 Then Exit While
            If (cp1.mid < 0) Then Exit While
            strLabelNew = strLabelHead + CStr(iNo)
            gDrawLabelText(cp1.sortID).LabelName = strLabelNew
            gDrawPoints(cp1.sortID).LabelName = strLabelNew
            Data_Point.DGV_DV.Rows(cp1.sortID).Cells(1).Value = strLabelNew
            iNo = iNo + 1
            strIDArr.Append(CStr(cp1.mid))
            strLabelArr.Append(strLabelNew)
        End While
        IOUtil.WriteCommandLine("")
        '変更したラベル情報で「measurepoint3d」テーブルを更新
        Call YCM_UpdataUserLabel(m_strDataBasePath, strIDArr, strLabelArr)
    End Sub
#End Region

    '◆コマンド
    '・手動ラベリング（ラベル入力を待たないで実行）
    '　「MainFrm.TBox_Data.Text」から直接ラベルを取得して手動ラベリングを行う
    Public Sub command_ChangeLabel()
        Dim strLabel As String = ""
        Dim strNo As String = ""
        Dim strLabelHead As String = ""
        Dim strLabelNew As String = ""
        Dim cp1 As New CLookPoint
        Dim iNo As Long
        '連番でラベルを設定する
        'Dim strPromptHead As String = "ラベル："'13.1.16以前
        Dim strPromptHead As String = "" '13.1.16

        '最初のラベル文字を入力（前回実行後の値を表示）
        Dim strPrompt As String = strPromptHead
        If (Len(m_inputPrevLabelText) > 0) Then strPrompt = strPromptHead + m_inputPrevLabelText
        IOUtil.WriteCommandLine(strPrompt)
        '変更する計測点を指示
        Dim iRet As Integer
        Dim strIDArr As New StringArray
        Dim strLabelArr As New StringArray
        'IOUtil.WritePrompt("変更する計測点を指定してください。") '13.1.15以前
        IOUtil.WritePrompt("コマンド入力領域に計測点名を[英字]もしくは[英字]＋[数値]の形式で入力し、その後3D操作ビューにて計測点を指定して下さい。") '13.1.15
        While True '13.1.15　処理の方法（新規、連番）
            If IOUtil.GetPointNoPrompt(cp1, "") = -1 Then Exit While
            If (cp1.mid < 0) Then Exit While

            iRet = ExtractLabelText(strLabelHead, iNo)
            If (iRet = 0) Then
                strLabelNew = strLabelHead + CStr(iNo)
                For i As Integer = 0 To nLabelText - 1
                    If gDrawLabelText(i).mid = cp1.mid Then
                        gDrawLabelText(i).LabelName = strLabelNew
                        Exit For
                    End If
                Next
                ' gDrawLabelText(cp1.sortID).LabelName = strLabelNew
                gDrawPoints(cp1.sortID).LabelName = strLabelNew
                Data_Point.DGV_DV.Rows(cp1.sortID).Cells(1).Value = strLabelNew
                strIDArr.Append(CStr(cp1.mid))
                strLabelArr.Append(strLabelNew)
                MainFrm.SetCurrentLabelTo_objFBM(cp1.sortID)
                '次の空き番号でラベルを作成    
                iNo = getAkiNumber(CLookPoint.posTypeMode.All, strLabelHead, strLabel)
                If (iNo > 0) Then
                    m_inputPrevLabelText = strLabel
                Else
                    iNo = iNo + 1
                    m_inputPrevLabelText = strLabelHead + CStr(iNo)
                End If

                strPrompt = strPromptHead + m_inputPrevLabelText
                IOUtil.WriteCommandLine(strPrompt)
                'IOUtil.WritePrompt("変更する計測点を指定してください。") '13.1.16以前
                IOUtil.WritePrompt("3D操作ビューにて計測点を指定して下さい。") '13.1.16
               
            End If
        End While
        IOUtil.WriteCommandLine("")
        '変更したラベル情報で「measurepoint3d」テーブルを更新
        Call YCM_UpdataUserLabel(m_strDataBasePath, strIDArr, strLabelArr)


    End Sub

    '◆共通関数
    'コマンド入力域で指定されているラベルを取得する
    ' 戻り値：=0 ：正常
    '         =-1：[英字]＋[数字]の形式になっていない
    Private Function ExtractLabelText(ByRef strText As String, ByRef iNo As Long) As Integer
        Dim strLabel As String = ""
        Dim iLen As Integer, iType As Integer, bIsHan As Boolean
        Dim saStr As StringArray
        'Dim strPromptHead As String = "ラベル："'13.1.16以前
        Dim strPromptHead As String = "" '13.1.16
        ExtractLabelText = 0
        iLen = IOUtil.GetStringNoWait(strLabel, strPromptHead) 'strLabel：ユーザー入力頭文字 / strPromptHead："ラベル："

        bIsHan = isHankakuText(strLabel) '半角かどうか判定
        If (bIsHan = False) Then
            IOUtil.WritePrompt("ラベルは、半角文字で入力してください")
            ExtractLabelText = -1
            Exit Function
        End If
        '入力されたラベル文字から英字と数字を分離
        saStr = Util_GetDigitSplittedString(strLabel, iType)
        '   iType [ /O] =-1：文字列なし
        '               =0：全て数字以外、=1：全て数字、=2：数字が先頭、=3：数字以外が先頭
        If (iType <> 0) And (iType <> 3) Then
            IOUtil.WritePrompt("ラベルは、[英字]もしくは[英字]＋[数値]の形式で入力してください")
            ExtractLabelText = -1
            Exit Function
        End If

        If (saStr.Size < 2) Then
            strText = strLabel
            iNo = getAkiNumber(CLookPoint.posTypeMode.All, strText, strLabel)
            If (iNo > 0) Then
                IOUtil.WriteCommandLine(strPromptHead + strText + CStr(iNo)) '（13.1.16）strPromptHead"ラベル："/strText：ユーザー指定頭文字/ CStr(iNo)：空き番
            Else
                iNo = 1
                IOUtil.WriteCommandLine(strPromptHead + strText + "1")
            End If
        Else
            strText = saStr.Front
            Try
                iNo = CLng(saStr.Back)
                If (CheckSamePosLabelData(CLookPoint.posTypeMode.All, strLabel) >= 0) Then
                    '既に使われているラベル
                    IOUtil.WritePrompt("ラベルは、既に使われています。別のラベルに変更してください。")
                    ExtractLabelText = -1
                End If
            Catch ex As Exception
                '英字＋数字で入力してください
                IOUtil.WritePrompt("ラベルは、[英字]もしくは[英字]＋[数値]の形式で入力してください。")
                ExtractLabelText = -1
                Exit Function
            End Try
        End If
    End Function

    '入力された頭ラベル文字の空き番号を得る
    Private Function getAkiNumber( _
                    ByVal posType As CLookPoint.posTypeMode, _
                    ByVal strLabelHead As String, _
                    ByRef strLabel As String _
    ) As Integer
        Dim iNo As Long
        getAkiNumber = -1
        iNo = 1
        Do While 1
            strLabel = strLabelHead + CStr(iNo)
            If (CheckSamePosLabelData(posType, strLabel) < 0) Then
                getAkiNumber = iNo
                Exit Function
            End If
            iNo = iNo + 1
        Loop
    End Function
End Module
