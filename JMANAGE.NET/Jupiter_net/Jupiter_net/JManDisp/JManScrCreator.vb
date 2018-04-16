Option Strict Off
Option Explicit On

Imports System.IO
Imports System.Text

Module JManScrCreator

    '---------------------------------------------------------------------------------------------------
    '機能
    '   部材一括前処理＋部材作成のスクリプトファイルを作成する。
    '引数
    '   kozo：(I)構造形式
    '   kojiFolder：(I)工事フォルダ
    '   scrFileName：(O)スクリプトファイル名
    '戻り値
    '---------------------------------------------------------------------------------------------------
    Sub createBziBatchScr(kozo As Integer, kojiFolder As String, ByRef scrFileName As String)

        If kozo = 3 Then        '合成床版の場合
            scrFileName = kojiFolder.TrimEnd("\") + "\JBZILSPSCRGDK.SCR"
        ElseIf kozo = 6 Then    '鋼製セグメントの場合
            scrFileName = kojiFolder.TrimEnd("\") + "\JBUZAI.SCR"
        Else                    'RC鈑桁,RC箱桁,鋼床版,鋼製橋脚の場合
            scrFileName = kojiFolder.TrimEnd("\") + "\JBUZAI.SCR"
        End If

        Dim scrWriter As New StreamWriter(scrFileName, False, Encoding.GetEncoding("Shift_JIS"))
        scrWriter.WriteLine("FILEDIA 0")
        scrWriter.WriteLine("(ARXLOAD ""JM_TOOL"" """")")
        scrWriter.WriteLine("JM_CHDIR " + kojiFolder)
        scrWriter.WriteLine("(ARXUNLOAD ""JM_TOOL"" """")")

        If kozo = 3 Then        '合成床版の場合
            scrWriter.WriteLine("SCRIPT XBziLispScriptGDK")
        ElseIf kozo = 6 Then    '鋼製セグメントの場合
            scrWriter.WriteLine("SCRIPT XBziLispScriptSeg")
        Else                    'RC鈑桁,RC箱桁,鋼床版,鋼製橋脚の場合
            scrWriter.WriteLine("SCRIPT XBziLispScript")
        End If

        scrWriter.Close()

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   会話処理のスクリプトファイルを作成する。
    '引数
    '   kojiFolder：(I)工事フォルダ
    '   tscFileName：(I)TSCファイル名
    '   scrFileName：(O)スクリプトファイル名
    '戻り値
    '---------------------------------------------------------------------------------------------------
    Sub createBziKaiwaScr(kojiFolder As String, tscFileName As String, ByRef scrFileName As String)

        scrFileName = kojiFolder.TrimEnd("\") + "\JBziKaiwa.SCR"

        Dim scrWriter As New StreamWriter(scrFileName, False, Encoding.GetEncoding("Shift_JIS"))
        scrWriter.WriteLine("FILEDIA 0")
        scrWriter.WriteLine("(ARXLOAD ""JM_TOOL"" """")")
        scrWriter.WriteLine("JM_CHDIR " + kojiFolder)
        scrWriter.WriteLine("(ARXUNLOAD ""JM_TOOL"" """")")
        scrWriter.WriteLine("SCRIPT " + tscFileName)
        scrWriter.WriteLine("FILEDIA 1")

        scrWriter.Close()

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   TSC(SCR)ファイルに、"SCR_DEVELOP2SORT"(分類まで連続実行)の定義が存在する場合は削除する。
    '引数
    '   tscFileNamePath：(I)TSC(SCR)ファイル名(フルパス)
    '戻り値
    '---------------------------------------------------------------------------------------------------
    Sub deleteRowsTscFile(tscFileNamePath As String, deleteRows As ArrayList)

        Dim tscLines As New ArrayList()
        Dim isFound As Boolean = False
        Dim deleteRowIdx As Integer = 0
        If System.IO.File.Exists(tscFileNamePath) = True Then
            Dim tscFile As New StreamReader(tscFileNamePath, Encoding.GetEncoding("Shift_JIS"))
            While (tscFile.Peek() >= 0)
                Dim lineText As String = tscFile.ReadLine().Trim()
                removeComment(lineText)
                If lineText = "" Then
                    Continue While
                End If

                Dim isFoundRow As Boolean = False
                If isFound = False Then
                    If lineText = deleteRows(0) Then
                        isFoundRow = True
                        isFound = True
                        deleteRowIdx = deleteRowIdx + 1
                    End If
                Else
                    If deleteRowIdx < deleteRows.Count And lineText = deleteRows(deleteRowIdx) Then
                        isFoundRow = True
                        deleteRowIdx = deleteRowIdx + 1
                    End If
                End If
                If isFoundRow = False Then
                    tscLines.Add(lineText)
                End If
            End While
            tscFile.Close()
        End If

        If isFound = False Then
            Return
        End If

        Dim scrWriter As New StreamWriter(tscFileNamePath, False, Encoding.GetEncoding("Shift_JIS"))
        Dim j As Integer = 0
        For j = 0 To tscLines.Count - 1
            scrWriter.WriteLine(tscLines(j))
        Next
        scrWriter.Close()

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   TSC(SCR)ファイルから分類まで連続実行、干渉チェック、投影図取込の定義を削除する。
    '引数
    '   kojiFolder：(I)工事フォルダ
    '   tscFileName：(I)TSCファイル名
    '戻り値
    '---------------------------------------------------------------------------------------------------
    Sub deleteRowsScrDevelop2Sort(kojiFolder As String, tscFileName As String)

        Dim deleteRows As New ArrayList()
        deleteRows.Add("(ARXLOAD ""BLKSCR"" """")")
        deleteRows.Add("SCR_DEVELOP2SORT")
        deleteRows.Add("*")
        deleteRows.Add("(ARXUNLOAD ""BLKSCR"" """")")

        Dim scrFileName As String = kojiFolder.TrimEnd("\") + "\" + tscFileName + ".SCR"
        deleteRowsTscFile(scrFileName, deleteRows)

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   TSC(SCR)ファイルに、"KanshoChk"の定義が存在する場合は削除する。
    '引数
    '   kojiFolder：(I)工事フォルダ
    '   tscFileName：(I)TSCファイル名
    '戻り値
    '---------------------------------------------------------------------------------------------------
    Sub deleteRowsKanshoChk(kojiFolder As String, tscFileName As String)

        Dim deleteRows As New ArrayList()
        deleteRows.Add("(LOAD ""XKanshoChk"") XKanshoChk")

        Dim scrFileName As String = kojiFolder.TrimEnd("\") + "\" + tscFileName + ".SCR"
        deleteRowsTscFile(scrFileName, deleteRows)

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   TSC(SCR)ファイルに、"ToueiTorikomi"の定義が存在する場合は削除する。
    '引数
    '   kojiFolder：(I)工事フォルダ
    '   tscFileName：(I)TSCファイル名
    '戻り値
    '---------------------------------------------------------------------------------------------------
    Sub deleteRowsToueiTorikomi(kojiFolder As String, tscFileName As String)

        Dim deleteRows As New ArrayList()
        deleteRows.Add("(ARXLOAD ""ToueiTorikomi"" """")")
        deleteRows.Add("ToueiTorikomi")
        deleteRows.Add("ToueiTorikomi.gmn")
        deleteRows.Add("(ARXUNLOAD ""ToueiTorikomi"")")

        Dim scrFileName As String = kojiFolder.TrimEnd("\") + "\" + tscFileName + ".SCR"
        deleteRowsTscFile(scrFileName, deleteRows)

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   部材一括+会話処理のスクリプトファイルを作成する。
    '引数
    '   kojiFolder：(I)工事フォルダ
    '   tscFileName：(I)TSCファイル名
    '   scrFileName：(O)スクリプトファイル名
    '戻り値
    '---------------------------------------------------------------------------------------------------
    Sub createBziBatchKaiwaScr(kojiFolder As String, tscFileName As String, ByRef scrFileName As String)

        'TSC(SCR)ファイルから分類まで連続実行、干渉チェック、投影図取込の定義を削除する。
        deleteRowsScrDevelop2Sort(kojiFolder, tscFileName)
        deleteRowsKanshoChk(kojiFolder, tscFileName)
        deleteRowsToueiTorikomi(kojiFolder, tscFileName)

        scrFileName = kojiFolder.TrimEnd("\") + "\JBUZAI.SCR"

        Dim scrWriter As New StreamWriter(scrFileName, False, Encoding.GetEncoding("Shift_JIS"))
        scrWriter.WriteLine("FILEDIA 0")
        scrWriter.WriteLine("(ARXLOAD ""JM_TOOL"" """")")
        scrWriter.WriteLine("JM_CHDIR " + kojiFolder)
        scrWriter.WriteLine("(ARXUNLOAD ""JM_TOOL"" """")")
        scrWriter.WriteLine("(LOAD ""XBziBatch"") (C:BziLispScriptinput """ + tscFileName + """)")
        scrWriter.WriteLine("SCRIPT XBziLispScriptinput")
        scrWriter.WriteLine("FILEDIA 1")
        scrWriter.Close()

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   部材分類処理のスクリプトファイルを作成する。
    '引数
    '   kozo：(I)構造形式
    '   kojiFolder：(I)工事フォルダ
    '   scrFileName：(O)スクリプトファイル名
    '戻り値
    '---------------------------------------------------------------------------------------------------
    Sub createSortScr(kojiFolder As String, ByRef scrFileName As String)

        'スクリプトファイルを作成
        scrFileName = kojiFolder.TrimEnd("\") + "\JSORT.SCR"
        If System.IO.File.Exists(scrFileName) Then
            System.IO.File.Delete(scrFileName)
        End If
        Dim scrWriter As New StreamWriter(scrFileName, False, Encoding.GetEncoding("Shift_JIS"))
        scrWriter.WriteLine("FILEDIA 0")
        scrWriter.WriteLine("(ARXLOAD ""JM_TOOL"" """")")
        scrWriter.WriteLine("JM_CHDIR " + kojiFolder)
        scrWriter.WriteLine("(ARXUNLOAD ""JM_TOOL"" """")")
        '部材分類の実行
        scrWriter.WriteLine("(ARXLOAD ""PSORT"" """")")
        scrWriter.WriteLine("PSORT")
        scrWriter.WriteLine("N")
        scrWriter.WriteLine("HENKEI.PMF")
        scrWriter.WriteLine("TmpSortPmf.Tmp")
        scrWriter.WriteLine("N")
        scrWriter.WriteLine("(ARXUNLOAD ""PSORT"")")
        'ブロック分類の実行
        scrWriter.WriteLine("(ARXLOAD ""BSORT"" """")")
        scrWriter.WriteLine("BSORT")
        scrWriter.WriteLine("ZOKUSEI.BMF")
        scrWriter.WriteLine("TmpSortPmf.Tmp")
        scrWriter.WriteLine("TmpSortBmf.Tmp")
        scrWriter.WriteLine("BSORT.MRK")
        scrWriter.WriteLine("SWAYHONE.CSV")
        scrWriter.WriteLine("SWAYSORT.CSV")
        scrWriter.WriteLine("(ARXUNLOAD ""BSORT"" """")")
        '部組マーク変更の実行
        scrWriter.WriteLine("(ARXLOAD ""MRKMOD"" """")")
        scrWriter.WriteLine("MRKMOD")
        scrWriter.WriteLine("TmpSortPmf.Tmp")
        scrWriter.WriteLine("SORT.PMF")
        scrWriter.WriteLine("TmpSortBmf.Tmp")
        scrWriter.WriteLine("SORT.BMF")
        scrWriter.WriteLine("(ARXUNLOAD ""MRKMOD"" """")")
        'マーキン面付加の実行
        scrWriter.WriteLine("(ARXLOAD ""BdMarking"")")
        scrWriter.WriteLine("BdMarking")
        scrWriter.WriteLine("SORT.PMF")
        scrWriter.WriteLine("SORT.PMF")
        scrWriter.WriteLine("(ARXUNLOAD ""BdMarking"")")
        'ネット率算出の実行
        scrWriter.WriteLine("(ARXLOAD ""PMFNETIN"" """")")
        scrWriter.WriteLine("PMFNETIN")
        scrWriter.WriteLine("SORT.PMF")
        scrWriter.WriteLine("SORT.PMF")
        scrWriter.WriteLine("(ARXUNLOAD ""PMFNETIN"" """")")
        '重量計算の実行
        scrWriter.WriteLine("(ARXLOAD ""PMFWEIGHT"" """")")
        scrWriter.WriteLine("PMFWEIGHT")
        scrWriter.WriteLine("SORT.PMF")
        scrWriter.WriteLine("A")
        scrWriter.WriteLine("N")
        scrWriter.WriteLine("(ARXUNLOAD ""PMFWEIGHT"" """")")
        scrWriter.Close()

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   部材展開～部材分類処理のスクリプトファイルを作成する。
    '引数
    '   kojiFolder：(I)工事フォルダ
    '   scrFileName：(O)スクリプトファイル名
    '戻り値
    '---------------------------------------------------------------------------------------------------
    ' 2017/03/18 Nakagawa Add Start
    'Sub createDev2SortScr( _
    '                        kojiFolder As String, _
    '                        isRunDevelop As Boolean, _
    '                        isRunWeldStage As Boolean, _
    '                        isRunZok As Boolean, _
    '                        isRunDeform As Boolean, _
    '                        isRunPmfHosei As Boolean, _
    '                        isRunPsort As Boolean, _
    '                        isRunBsort As Boolean, _
    '                        isRunMarking As Boolean, _
    '                        ByRef scrFileName As String, _
    '                        ByRef errMsg As String _
    '                        )
    Sub createDev2SortScr( _
                    kojiFolder As String, _
                    isRunDevelop As Boolean, _
                    isRunWeldStage As Boolean, _
                    isRunZok As Boolean, _
                    isRunDeform As Boolean, _
                    isRunPmfHosei As Boolean, _
                    isRunSegPmfBzi As Boolean, _
                    isRunPsort As Boolean, _
                    isRunBsort As Boolean, _
                    isRunMarking As Boolean, _
                    ByRef scrFileName As String, _
                    ByRef errMsg As String _
                    )
        ' 2017/03/18 Nakagawa Add End

        'Block.infを読み込む。
        Dim blockInfFileName As String = kojiFolder.TrimEnd("\") + "\BLOCK.INF"
        Dim blockInfFile As New JManBlockInfFile()
        Dim status As Integer = blockInfFile.load(blockInfFileName)
        If status <> 0 Then
            errMsg = "工事フォルダにBLOCK.infが存在していません。部材作成が正常終了しているか確認してください。"
            Return
        End If

        'スクリプトファイルを作成
        scrFileName = kojiFolder.TrimEnd("\") + "\JSeisan.SCR"
        If System.IO.File.Exists(scrFileName) Then
            System.IO.File.Delete(scrFileName)
        End If

        '展開処理
        Dim scrWriter As New StreamWriter(scrFileName, False, Encoding.GetEncoding("Shift_JIS"))
        scrWriter.WriteLine("(ARXLOAD ""JM_TOOL"" """")")
        scrWriter.WriteLine("JM_CHDIR " + kojiFolder)
        scrWriter.WriteLine("(ARXUNLOAD ""JM_TOOL"" """")")
        '展開処理
        If isRunDevelop = True Then
            scrWriter.WriteLine("(arxload ""PROTECTCHECK"")")
            scrWriter.WriteLine("PROTECTCHECK")
            scrWriter.WriteLine("(arxunload ""PROTECTCHECK"")")
            'blkscrの実行
            scrWriter.WriteLine("(arxload ""blkscr"")")
            scrWriter.WriteLine("SCR_DEVELOP")
            scrWriter.WriteLine("*")
            scrWriter.WriteLine("(arxunload ""blkscr"")")
            scrWriter.WriteLine("SCRIPT DEVELOP0.SCR")
            '溶接時期変更
            If isRunWeldStage = True Then
                scrWriter.WriteLine("(ARXLOAD ""WELDSTAGE"" """")")
                scrWriter.WriteLine("(ARXLOAD ""PMF2BMF"" """")")
                Dim i As Integer = 0
                For i = 0 To blockInfFile.size() - 1
                    Dim blockName As String = blockInfFile.getAt(i)._blockName
                    scrWriter.WriteLine("WELDSTAGE")
                    scrWriter.WriteLine(blockName + ".PMF")
                    scrWriter.WriteLine(blockName + ".PMF")
                    scrWriter.WriteLine("PMF2BMF")
                    scrWriter.WriteLine(blockName)
                Next
                scrWriter.WriteLine("(ARXUNLOAD ""WELDSTAGE"" """")")
                scrWriter.WriteLine("(ARXUNLOAD ""PMF2BMF"" """")")
            End If
            'PMFBINDの実行
            scrWriter.WriteLine("(ARXLOAD ""PMFBIND"" """")")
            scrWriter.WriteLine("PMFBIND")
            scrWriter.WriteLine("*")
            scrWriter.WriteLine("TENKAI.PMF")
            scrWriter.WriteLine("TENKAI.BMF")
            scrWriter.WriteLine("(ARXUNLOAD ""PMFBIND"" """")")
            'ネット率算出の実行
            scrWriter.WriteLine("(ARXLOAD ""PMFNETIN"" """")")
            scrWriter.WriteLine("PMFNETIN")
            scrWriter.WriteLine("TENKAI.PMF")
            scrWriter.WriteLine("TENKAI.PMF")
            scrWriter.WriteLine("(ARXUNLOAD ""PMFNETIN"" """")")
            '重量計算の実行
            scrWriter.WriteLine("(ARXLOAD ""PMFWEIGHT"" """")")
            scrWriter.WriteLine("PMFWEIGHT")
            scrWriter.WriteLine("TENKAI.PMF")
            scrWriter.WriteLine("A")
            scrWriter.WriteLine("N")
            scrWriter.WriteLine("(ARXUNLOAD ""PMFWEIGHT"" """")")
        End If

        '属性付加
        Dim zokPmfFileName As String = "TENKAI.PMF"
        Dim zokBmfFileName As String = "TENKAI.BMF"
        If isRunZok = True Then
            scrWriter.WriteLine("(ARXLOAD ""ZOKUSEI"")")
            scrWriter.WriteLine("ZOKUSEI")
            scrWriter.WriteLine("TENKAI.PMF")
            scrWriter.WriteLine("ZOKUSEI.PMF")
            scrWriter.WriteLine("TENKAI.BMF")
            scrWriter.WriteLine("ZOKUSEI.BMF")
            scrWriter.WriteLine("JUPITER.DAT")
            scrWriter.WriteLine("(ARXUNLOAD ""ZOKUSEI"")")
            zokPmfFileName = "ZOKUSEI.PMF"
            zokBmfFileName = "ZOKUSEI.BMF"
        End If

        '変形処理
        If isRunDeform = True Then
            Dim henkeiPmfFileName As String = "HENKEI.PMF"
            If isRunPmfHosei = True Then
                henkeiPmfFileName = "TmpHenkeiPmf.Tmp"
            End If

            scrWriter.WriteLine("(ARXLOAD ""DEFORM"")")
            scrWriter.WriteLine("DEFORM")
            scrWriter.WriteLine("HENKEI.DEF")
            scrWriter.WriteLine(zokPmfFileName)
            scrWriter.WriteLine(henkeiPmfFileName)
            scrWriter.WriteLine("(ARXUNLOAD ""DEFORM"")")
            If isRunPmfHosei = True Then
                scrWriter.WriteLine("(ARXLOAD ""PMFHOSEI"")")
                scrWriter.WriteLine("PMFHOSEI")
                scrWriter.WriteLine(henkeiPmfFileName)
                scrWriter.WriteLine("PMFHOSEI.TBL")
                scrWriter.WriteLine("HENKEI.PMF")
                scrWriter.WriteLine("(ARXUNLOAD ""PMFHOSEI"")")
            End If
        End If

        ' 2017/03/18 Nakagawa Add Start
        If isRunSegPmfBzi = True Then
            Dim henkeiPmfFileName As String = "HENKEI.PMF"
            Dim SegDataFileName As String = "SegData.dat"

            scrWriter.WriteLine("(ARXLOAD ""SEGPMFBZI"")")
            scrWriter.WriteLine("SEGPMFBZI")
            scrWriter.WriteLine(henkeiPmfFileName)
            scrWriter.WriteLine(SegDataFileName)
            scrWriter.WriteLine(henkeiPmfFileName)
            scrWriter.WriteLine("(ARXUNLOAD ""SEGPMFBZI"")")
        End If
        ' 2017/03/18 Nakagawa Add End

        '分類
        Dim psortPmfFileName As String = "SORT.PMF"
        If isRunBsort = True Then
            psortPmfFileName = "TmpSortPmf.Tmp"
        End If
        If isRunPsort = True Then
            '部材分類の実行
            scrWriter.WriteLine("(ARXLOAD ""PSORT"" """")")
            scrWriter.WriteLine("PSORT")
            scrWriter.WriteLine("N")
            scrWriter.WriteLine("HENKEI.PMF")
            scrWriter.WriteLine(psortPmfFileName)
            scrWriter.WriteLine("N")
            scrWriter.WriteLine("(ARXUNLOAD ""PSORT"")")
            '対傾構分類の実行
            scrWriter.WriteLine("(ARXLOAD ""SWAYSORT"" """")")
            scrWriter.WriteLine("SWAYSORT")
            scrWriter.WriteLine("SWAYHONE.CSV")
            scrWriter.WriteLine("(ARXUNLOAD ""SWAYSORT"" """")")
        End If
        If isRunBsort = True Then
            'ブロック分類の実行
            scrWriter.WriteLine("(ARXLOAD ""BSORT"" """")")
            scrWriter.WriteLine("BSORT")
            scrWriter.WriteLine("ZOKUSEI.BMF")
            scrWriter.WriteLine("TmpSortPmf.Tmp")
            scrWriter.WriteLine("TmpSortBmf.Tmp")
            scrWriter.WriteLine("BSORT.MRK")
            scrWriter.WriteLine("SWAYHONE.CSV")
            scrWriter.WriteLine("SWAYSORT.CSV")
            scrWriter.WriteLine("(ARXUNLOAD ""BSORT"" """")")
            '部組マーク変更の実行
            scrWriter.WriteLine("(ARXLOAD ""MRKMOD"" """")")
            scrWriter.WriteLine("MRKMOD")
            scrWriter.WriteLine("TmpSortPmf.Tmp")
            scrWriter.WriteLine("SORT.PMF")
            scrWriter.WriteLine("TmpSortBmf.Tmp")
            scrWriter.WriteLine("SORT.BMF")
            scrWriter.WriteLine("(ARXUNLOAD ""MRKMOD"" """")")
            '現場マーク付加
            Dim user As String = ""
            subGetUserName(user)
            scrWriter.WriteLine("(ARXLOAD ""FMPRESET"" """")")
            scrWriter.WriteLine("FMPRESET")
            scrWriter.WriteLine("SORT.BMF")
            scrWriter.WriteLine("SORT.PMF")
            scrWriter.WriteLine("SEISAKUALL.SKL")
            scrWriter.WriteLine("SORT.BMF")
            scrWriter.WriteLine("SORT.PMF")
            scrWriter.WriteLine("FMPRESET.MRK")
            scrWriter.WriteLine("(ARXUNLOAD ""FMPRESET"" """")")
        End If

        If isRunMarking = True Then
            'マーキン面付加の実行
            scrWriter.WriteLine("(ARXLOAD ""BdMarking"")")
            scrWriter.WriteLine("BdMarking")
            scrWriter.WriteLine("SORT.PMF")
            scrWriter.WriteLine("SORT.PMF")
            scrWriter.WriteLine("(ARXUNLOAD ""BdMarking"")")
        End If

        If isRunPsort = True Then
            '縦リブライズ線削除の実行
            scrWriter.WriteLine("(ARXLOAD ""RiseDel"")")
            scrWriter.WriteLine("RiseDel")
            scrWriter.WriteLine("SORT.PMF")
            scrWriter.WriteLine("SORT.PMF")
            scrWriter.WriteLine("(ARXUNLOAD ""RiseDel"")")

            'ネット率算出の実行
            scrWriter.WriteLine("(ARXLOAD ""PMFNETIN"" """")")
            scrWriter.WriteLine("PMFNETIN")
            scrWriter.WriteLine("SORT.PMF")
            scrWriter.WriteLine("SORT.PMF")
            scrWriter.WriteLine("(ARXUNLOAD ""PMFNETIN"" """")")
            '重量計算の実行
            scrWriter.WriteLine("(ARXLOAD ""PMFWEIGHT"" """")")
            scrWriter.WriteLine("PMFWEIGHT")
            scrWriter.WriteLine("SORT.PMF")
            scrWriter.WriteLine("A")
            scrWriter.WriteLine("N")
            scrWriter.WriteLine("(ARXUNLOAD ""PMFWEIGHT"" """")")
        End If

        scrWriter.Close()

    End Sub

End Module
