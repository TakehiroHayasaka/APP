Option Strict Off
Option Explicit On

Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.CompilerServices

Module JManCommon


    '---------------------------------------------------------------------------------------------------
    '機能
    '   EXEを起動する。
    '引数
    '   exeName：EXEファイル名
    '   arg：プログラム引数
    '   wait：0=プロセス終了まで待機、1=待機しない
    '戻り値
    '   無し
    '---------------------------------------------------------------------------------------------------
    Sub exeStart(kojiFolder As String, exeName As String, arg As String, Optional wait As Integer = 0)

        ' ProcessStartInfo の新しいインスタンスを生成する
        Dim process As New System.Diagnostics.ProcessStartInfo()
        ' 起動するアプリケーションを設定する
        process.FileName = exeName
        ' コマンドライン引数を設定する
        If arg <> "" Then
            process.Arguments = arg
        End If
        ' 新しいウィンドウを作成するかどうかを設定する (初期値 False)
        process.CreateNoWindow = True
        ' シェルを使用するかどうか設定する (初期値 True)
        process.UseShellExecute = False
        ' 起動できなかった時にエラーダイアログを表示するかどうかを設定する (初期値 False)
        process.ErrorDialog = True
        ' アプリケーションを起動する時の動詞を設定する
        process.Verb = "Open"
        ' 起動ディレクトリを設定する
        process.WorkingDirectory = kojiFolder
        ' 起動時のウィンドウの状態を設定する
        process.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal     '通常
        ' ProcessStartInfo を指定して起動する
        Dim pr As System.Diagnostics.Process = System.Diagnostics.Process.Start(process)

        'プロセスが終了するまで待機する。
        If wait = 1 Then
            pr.WaitForExit()
        End If

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   Jupiterのプログラム(EXE)を起動する。(プログラムの検索順序＝工事フォルダ→(GENSUN_DIR)\Bin)
    '引数
    '   exeNameJupiter：EXEファイル名
    '   arg：プログラム引数
    '戻り値
    '   無し
    '---------------------------------------------------------------------------------------------------
    Function exeStartJupiter(kojiFolder As String, exeNameJupiter As String, arg As String) As Integer
        'Jupiterのプログラムを起動する。

        'プログラムのファイル名を作成する。
        Dim exeName As String = kojiFolder + exeNameJupiter
        If System.IO.File.Exists(exeName) = False Then
            '工事フォルダ内にEXEファイルが存在しない。
            exeName = FunGetBin() + exeNameJupiter
            If System.IO.File.Exists(exeName) = False Then
                Return 1
            End If
        End If

        exeStart(kojiFolder, exeName, arg, 1)

        Return 0

    End Function

    '---------------------------------------------------------------------------------------------------
    '機能
    '   TEXTデータをエディタで起動する。(プログラムの検索順序＝工事フォルダ→(GENSUN_DIR)\Bin)
    '引数
    '   fileName：Txtファイル名
    '   wait：0=プロセス終了まで待機、1=待機しない
    '戻り値
    '   無し
    '---------------------------------------------------------------------------------------------------
    Function editorStart(kojiFolder As String, fileName As String, Optional wait As Integer = 0)

        editorStart = 0

        'Jmanage.iniからEditorのパスを取得する。
        Dim exeNameTmp As String = FunGetAppNo(0)

        Dim exeName As String = ""
        fun文字列抽出(exeNameTmp, "[", "]", exeName)

        If exeName = "" Then
            editorStart = 1
            Exit Function
        End If

        Dim fileNamePathNon = subStringEnd(fileName, "\")

        exeStart(kojiFolder, exeName, fileNamePathNon, wait)

    End Function

    '---------------------------------------------------------------------------------------------------
    '機能
    '   Profile.iniを読み込み、橋梁形式,チャージコード,チャージコード登録位置を取得
    '引数
    '   profileFileName：ファイル名
    '   kozo：橋梁形式
    '   chargeCode：チャージコード
    '   recno：チャージコード登録位置
    '戻り値
    '   橋梁形式番号(0:RC鈑桁、1:RC箱桁、2:鋼床版箱桁、3:合成床版、4:鋼製橋脚、5:波形ウェブ、6:鋼製セグメント
    '   エラーの場合は-1
    '---------------------------------------------------------------------------------------------------
    Function readProfileIni(profileFileName As String, ByRef kozo As String, ByRef chargeCode As String, ByRef recno As Integer) As Integer

        readProfileIni = -1

        Dim findIndex As Integer = 0
        ' Profile.iniを読み込み、橋梁形式とチャージコードを取得
        If System.IO.File.Exists(profileFileName) = True Then
            Dim profileIni As New StreamReader(profileFileName, Encoding.GetEncoding("Shift_JIS"))
            While (profileIni.Peek() >= 0)
                Dim lineText As String = profileIni.ReadLine().Trim()
                If lineText Like "主桁断面=*" Then
                    Dim kozoArray As String() = lineText.Split("=")
                    kozo = kozoArray(kozoArray.Length() - 1)
                    findIndex = findIndex + 1
                ElseIf lineText Like "チャージコード=*" Then
                    Dim chargeCodeArray As String() = lineText.Split("=")
                    chargeCode = chargeCodeArray(chargeCodeArray.Length() - 1)
                    If chargeCode = "FREE-JUPITER" Then
                        chargeCode = ""
                    End If
                    findIndex = findIndex + 1
                ElseIf lineText Like "登録ファイルレコード位置=*" Then
                    Dim recnoArray As String() = lineText.Split("=")
                    recno = Integer.Parse(recnoArray(recnoArray.Length() - 1))
                    findIndex = findIndex + 1
                End If
            End While
            profileIni.Close()
        Else
            Exit Function
        End If

        readProfileIni = getKozoNo(kozo)

    End Function

    Function getKozo(kozoNo As Integer) As String

        getKozo = ""

        If kozoNo = 0 Then
            getKozo = "RC鈑桁"
        ElseIf kozoNo = 1 Then
            getKozo = "RC箱桁"
        ElseIf kozoNo = 2 Then
            getKozo = "鋼床版箱桁"
        ElseIf kozoNo = 3 Then
            getKozo = "合成床版"
        ElseIf kozoNo = 4 Then
            getKozo = "鋼製橋脚"
        ElseIf kozoNo = 5 Then
            getKozo = "波形ウェブ"
        ElseIf kozoNo = 6 Then
            getKozo = "鋼製セグメント"
        End If

    End Function

    Function getKozoNo(kozo As String) As Integer

        getKozoNo = -1

        If kozo = "RC鈑桁" Then
            getKozoNo = 0
        ElseIf kozo = "RC箱桁" Then
            getKozoNo = 1
        ElseIf kozo = "鋼床版箱桁" Then
            getKozoNo = 2
        ElseIf kozo = "合成床版" Then
            getKozoNo = 3
        ElseIf kozo = "鋼製橋脚" Then
            getKozoNo = 4
        ElseIf kozo = "波形ウェブ" Then
            getKozoNo = 5
        ElseIf kozo = "鋼製セグメント" Then
            getKozoNo = 6
        End If

    End Function


    '---------------------------------------------------------------------------------------------------
    '機能
    '   テキストファイルを読み込み、ファイル内のテキストを全て取得する。
    '引数
    '   textFileName：(I)テキストファイル名
    '   text：(O)テキスト
    '戻り値
    '   0：正常終了
    '   1：異常終了
    '---------------------------------------------------------------------------------------------------
    Function getAllTextFromFile(textFileName As String, ByRef text As String) As Integer

        getAllTextFromFile = 0
        If System.IO.File.Exists(textFileName) Then
            '部材データを読み込み
            Dim textData As New StreamReader(textFileName, Encoding.GetEncoding("Shift_JIS"))
            text = textData.ReadToEnd()
            textData.Close()
        Else
            getAllTextFromFile = 1
        End If

    End Function

    '---------------------------------------------------------------------------------------------------
    '機能
    '   テキストファイルを読み込み、ファイル内のテキストリストを全て取得する。
    '引数
    '   textFileName：(I)テキストファイル名
    '   text：(O)テキスト
    '戻り値
    '   0：正常終了
    '   1：異常終了
    '---------------------------------------------------------------------------------------------------
    Function getAllTextListFromFile(textFileName As String, ByRef texts As ArrayList) As Integer

        getAllTextListFromFile = 0
        If System.IO.File.Exists(textFileName) Then
            '部材データを読み込み
            Dim textData As New StreamReader(textFileName, Encoding.GetEncoding("Shift_JIS"))
            While (textData.Peek() >= 0)
                texts.Add(textData.ReadLine())
            End While
            textData.Close()
        Else
            getAllTextListFromFile = 1
        End If

    End Function

    '---------------------------------------------------------------------------------------------------
    '機能
    '   テキストからキャプションを作成する
    '引数
    '   text：(I)テキスト
    '   caption：(O)キャプション
    '戻り値
    '   0：正常終了
    '   1：異常終了
    '---------------------------------------------------------------------------------------------------
    Function createCaption(text As String, ByRef caption As String) As Integer

        createCaption = 0

        Dim cs As Char = text(0)
        Dim ce As Char = text(text.Length - 1)
        If cs <> """" Or ce <> """" Then
            createCaption = 1
            Exit Function
        End If

        Dim captionList As New ArrayList

        Dim idx As Integer = text.IndexOf("+")
        If idx = -1 Then
            caption = text.Replace("""", "")
            Exit Function
        End If

        caption = ""
        Dim textList As String() = Split(text, "+")
        Dim i As Integer = 0
        For i = 0 To textList.Length - 1
            Dim textTmp As String = textList(i)
            caption += textTmp.Replace("""", "")
            If i <> textList.Length - 1 Then
                caption += vbCrLf
            End If
        Next

    End Function

    '---------------------------------------------------------------------------------------------------
    '機能
    '   テキストからキャプションを作成する
    '引数
    '   text：(I)テキスト
    '   caption：(O)キャプション
    '戻り値
    '   0：正常終了
    '   1：異常終了
    '---------------------------------------------------------------------------------------------------
    Function getRowColumnNo(text As String, ByRef rowNo As Integer, ByRef columnNo As Integer) As Integer

        getRowColumnNo = 0
        Dim textTmp1 As String = text
        Dim textTmp2 As String = text

        ' []の数を調べる(2以外であればエラー)
        Dim pNum1 As Integer = textTmp1.Length - textTmp1.Replace("[", "").Length
        Dim pNum2 As Integer = textTmp2.Length - textTmp2.Replace("]", "").Length
        If pNum1 <> 2 Or pNum2 <> 2 Then
            getRowColumnNo = 1
            Exit Function
        End If

        ' 最初の[]内の数値を取得
        Dim sidx1 As Integer = text.IndexOf("[") + 1
        Dim eidx1 As Integer = text.IndexOf("]")
        Dim rowText As String = text.Substring(sidx1, eidx1 - sidx1)
        If Integer.TryParse(rowText, rowNo) <> True Then
            getRowColumnNo = 1
            Exit Function
        End If

        ' 最初の[]内の数値を取得
        Dim sidx2 As Integer = text.LastIndexOf("[") + 1
        Dim eidx2 As Integer = text.LastIndexOf("]")
        Dim columnText As String = text.Substring(sidx2, eidx2 - sidx2)
        If Integer.TryParse(columnText, columnNo) <> True Then
            getRowColumnNo = 1
            Exit Function
        End If

    End Function

    '---------------------------------------------------------------------------------------------------
    '機能
    '   Environフォルダ内の設定ファイルを工事フォルダにコピーし、フルパスのファイル名を返す
    '引数
    '   kojiFolder：(I)工事フォルダ
    '   env：(I)Environフォルダ
    '   fileName：(I)ファイル名(パスなし)
    '戻り値
    '   フルパスのファイル名
    '---------------------------------------------------------------------------------------------------
    Public Function copySettingFileFromEnviron(kojiFolder As String, env As String, fileName As String, Optional copyFileName As String = "") As String

        copySettingFileFromEnviron = ""

        Dim kojiFileName As String = kojiFolder.TrimEnd("\") + "\" + fileName
        Dim envFileName As String = ""
        If copyFileName = "" Then
            envFileName = env.TrimEnd("\") + "\" + fileName
        Else
            envFileName = env.TrimEnd("\") + "\" + copyFileName
        End If

        If Not System.IO.File.Exists(kojiFileName) Then
            If System.IO.File.Exists(envFileName) Then
                System.IO.File.Copy(envFileName, kojiFileName)
            End If
        End If

        copySettingFileFromEnviron = kojiFileName

    End Function

    Public Function getAlphabet(i As Integer) As String

        getAlphabet = "-"

        Select Case i
            Case 1
                getAlphabet = "A"
            Case 2
                getAlphabet = "B"
            Case 3
                getAlphabet = "C"
            Case 4
                getAlphabet = "D"
            Case 5
                getAlphabet = "E"
            Case 6
                getAlphabet = "F"
            Case 7
                getAlphabet = "G"
            Case 8
                getAlphabet = "H"
            Case 9
                getAlphabet = "I"
            Case 10
                getAlphabet = "J"
            Case 11
                getAlphabet = "K"
            Case 12
                getAlphabet = "L"
            Case 13
                getAlphabet = "M"
            Case 14
                getAlphabet = "N"
            Case 15
                getAlphabet = "O"
            Case 16
                getAlphabet = "P"
            Case 17
                getAlphabet = "Q"
            Case 18
                getAlphabet = "R"
            Case 19
                getAlphabet = "S"
            Case 20
                getAlphabet = "T"
            Case 21
                getAlphabet = "U"
            Case 22
                getAlphabet = "V"
            Case 23
                getAlphabet = "W"
            Case 24
                getAlphabet = "X"
            Case 25
                getAlphabet = "Y"
            Case 26
                getAlphabet = "Z"
        End Select

    End Function

    Public Sub removeComment(ByRef text As String)

        Dim TextTmp As String = text

        Dim tIdx1 As Integer = text.IndexOf(";")
        Dim tIdx2 As Integer = text.IndexOf("//")
        Dim tIdx3 As Integer = text.IndexOf("/*")

        If tIdx1 = -1 And tIdx2 = -1 And tIdx3 = -1 Then
            Return
        End If

        Dim tIdxList As New ArrayList
        tIdxList.Add(tIdx1)
        tIdxList.Add(tIdx2)
        tIdxList.Add(tIdx3)
        tIdxList.Sort()

        Dim tIdx As Integer = 0
        Dim i As Integer = 0
        For i = 0 To tIdxList.Count - 1
            If tIdxList(i) <> -1 Then
                tIdx = tIdxList(i)
                Exit For
            End If
        Next

        text = TextTmp.Remove(tIdx)

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   テキストから指定した文字以降の文字を抜き出す。
    '引数
    '   text：(I)テキスト
    '   delim：(I)指定文字
    '戻り値
    '   0：正常終了
    '   1：異常終了
    '---------------------------------------------------------------------------------------------------
    Public Function subStringEnd(text As String, delim As String) As String

        subStringEnd = text

        Dim index As Integer = text.LastIndexOf(delim)
        If index >= 0 And index <> text.Length - 1 Then
            subStringEnd = text.Substring(index + 1)
        End If

    End Function

    ' 2016/11/01 Nakagawa Edit Start
    'Public Function checkBlockNumber(kogo As String) As String
    Public Function checkBlockNumber(kogo As String, type As String) As String
        ' 2016/11/01 Nakagawa Edit End

        Dim userName As String = ""
        subGetUserName(userName)
        If userName = "YBG" Or userName = "NKK" Then
            Return ""
        End If

        Dim errMsg As String = ""
        ' 2016/11/01 Nakagawa Add Start
        If type = "鋼製セグメント" Then
            Dim chargeCode As String = ""
            Dim status As Integer = funチャージコードの有無_展開確認(kogo, chargeCode)
            If status = -999 Then
                errMsg = "チャージコードの期限が切れているため、展開を行うことができません。再度チャージコードの申請を行ってください。"
            End If
        Else
            ' 2016/11/01 Nakagawa Add End
            'チャージコードの台数チェックを行う。
            Dim intChgNum As Integer = 0
            Dim intBlkNum As Integer = 0
            Dim status As Integer = funCHARGE台数のチェック(kogo, intChgNum, intBlkNum)
            Select Case status
                Case 1
                    errMsg = "Charge.iniにチャージコードが登録されていません。"
                Case 2
                    errMsg = "主桁の台数がCharge.iniに登録されている台数と異なります。" + vbCrLf
                    errMsg += "  展開対象ブロック数                  ：" + intBlkNum.ToString() + vbCrLf
                    errMsg += "  Charge.iniに登録されているブロック数：" + intChgNum.ToString() + vbCrLf
                Case 3
                    errMsg = "デッキの台数がCharge.iniに登録されている台数と異なります。" + vbCrLf
                    errMsg += "  展開対象ブロック数                  ：" + intBlkNum.ToString() + vbCrLf
                    errMsg += "  Charge.iniに登録されているブロック数：" + intChgNum.ToString() + vbCrLf
                Case 4
                    errMsg = "横桁の台数がCharge.iniに登録されている台数と異なります。" + vbCrLf
                    errMsg += "  展開対象ブロック数                  ：" + intBlkNum.ToString() + vbCrLf
                    errMsg += "  Charge.iniに登録されているブロック数：" + intChgNum.ToString() + vbCrLf
                Case 5
                    errMsg = "ブラケットの台数がCharge.iniに登録されている台数と異なります。" + vbCrLf
                    errMsg += "  展開対象ブロック数                  ：" + intBlkNum.ToString() + vbCrLf
                    errMsg += "  Charge.iniに登録されているブロック数：" + intChgNum.ToString() + vbCrLf
                Case 6
                    errMsg = "縦桁の台数がCharge.iniに登録されている台数と異なります。" + vbCrLf
                    errMsg += "  展開対象ブロック数                  ：" + intBlkNum.ToString() + vbCrLf
                    errMsg += "  Charge.iniに登録されているブロック数：" + intChgNum.ToString() + vbCrLf
                Case 7
                    errMsg = "対傾構の台数がCharge.iniに登録されている台数と異なります。" + vbCrLf
                    errMsg += "  展開対象ブロック数                  ：" + intBlkNum.ToString() + vbCrLf
                    errMsg += "  Charge.iniに登録されているブロック数：" + intChgNum.ToString() + vbCrLf
                Case 8
                    errMsg = "横構の有無がCharge.iniと異なります。"
            End Select
            ' 2016/11/01 Nakagawa Add Start
        End If
        ' 2016/11/01 Nakagawa Add End

        Return errMsg

    End Function

    '---------------------------------------------------------------------------------------------------
    '機能
    '   System.txtに工事フォルダ情報を書込む
    '引数
    '   SysFile(I):System.txtのパス
    '   kojiFolder(I):工号
    '戻り値
    '   無し
    '---------------------------------------------------------------------------------------------------
    Sub writeSystemTxt(sysFile As String, kojiFolder As String)

        Dim bytes1() As Byte = System.BitConverter.GetBytes(kojiFolder.Length())
        Dim bytes2() As Byte = System.Text.Encoding.GetEncoding(932).GetBytes(kojiFolder)
        Dim bytes3() As Byte = System.BitConverter.GetBytes(kojiFolder.Length() / 2)

        Dim fs As New System.IO.FileStream(sysFile, System.IO.FileMode.Create, System.IO.FileAccess.Write)
        fs.Write(bytes1, 0, bytes1.Length)
        fs.Write(bytes2, 0, bytes2.Length)
        fs.Write(bytes3, 0, bytes3.Length)
        fs.Close()

        'Open SysFile For Binary Access Write As #fnum
        '    Put #fnum, , Len(cdir)
        '    Put #fnum, , cdir
        '    Put #fnum, , Len(cdir) / 2
        'Close #fnum

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   System.txtに空白を書込む(JupiterManagerを閉じたときの処理)
    '引数
    '   SysFile(I):System.txtのパス
    '戻り値
    '   無し
    '---------------------------------------------------------------------------------------------------
    Sub writeSystemTxtClose(sysFile As String)

        Dim str As String = " "

        Dim bytes() As Byte = System.Text.Encoding.GetEncoding(932).GetBytes(str)

        Dim fs As New System.IO.FileStream(sysFile, System.IO.FileMode.Create, System.IO.FileAccess.Write)
        fs.Write(bytes, 0, bytes.Length)
        fs.Close()

    End Sub

End Module

