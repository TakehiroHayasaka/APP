
Imports System.Windows.Forms
Imports System.IO
Imports System.Text
Imports System.Windows.Documents
Imports System.Windows.Controls

Class MainWindow

    Private Declare Function InitializeLicense_JPT Lib "JptPrtct.dll" () As Integer

    'プロパティ

    '---------------------------------------------------------------------------------------------------
    ' Jupiter環境
    '---------------------------------------------------------------------------------------------------
    'Environのフォルダ名
    Private _environFolderName As String = ""
    'プログラム名
    Private _exeFileName As New JManExeFile
    ''Jconsoleのexeファイル名
    'Private _jconsoleExe As String = "Jconsole.exe"
    ''JLogViewのexeファイル名
    'Private _jlogViewExe As String = "JlogView.exe"
    ''JWeightのexeファイル名
    'Private _jweightExe As String = "Jweight.exe"
    ''Jpmfdrwのexeファイル名
    'Private _jpmfdrwExe As String = "Jpmfdrw.exe"
    ''Jcloutのexeファイル名
    'Private _jcloutExe As String = "Jclout.exe"
    ''DxfPlotのexeファイル名
    'Private _dxfPlotExe As String = "DxfPlot.exe"
    'IJCADのexeファイル名
    Private _ijcadExe As String = ""
    'Excelのexeファイル名
    Private _excelExe As String = ""
    'Accessのexeファイル名
    Private _accessExe As String = ""
    'Editorのexeファイル名
    Private _editorExe As String = ""

    '---------------------------------------------------------------------------------------------------
    ' 工事情報
    '---------------------------------------------------------------------------------------------------
    '工事フォルダ
    Private _kojiFolder As String = ""
    '設定ファイル名
    Private _setingFileName As New JManSettingFile
    '橋梁形式
    Private _kozo As String = ""
    Private _kozoNo As Integer = -1

    '---------------------------------------------------------------------------------------------------
    ' 画面設定
    '---------------------------------------------------------------------------------------------------
    ' 言語設定(0=Japanese,1=English)
    Public _language As Integer = 1
    ' ボタン色設定(0=Gray,1=Green)
    Public _buttonColor As Integer = 1
    ' 背景色設定(0=Gray,1=White)
    Public _backColor As Integer = 1
    'タブマネージャー
    Private _tabManager As TabPageManager = Nothing
    '定義しているタブ数
    Private _tabNum As Integer = 3
    'ボタン情報(帳票関連)
    Private _buttonInfoListRep As New JManDispButtonInfoList()

    '---------------------------------------------------------------------------------------------------
    ' 処理用
    '---------------------------------------------------------------------------------------------------
    '現在開いているテキストファイル名
    Private _openTextFileName As String = ""
    'リッチテキストボックスが変更されたかどうかのフラグ
    'Private _isChangedOpenTextFile As Boolean = False
    Private _isChangedOpenTextFileSkl As Boolean = False
    Private _isChangedOpenTextFileBzi As Boolean = False
    Private _isChangedOpenTextFileSeisan As Boolean = False
    Private _isChangedOpenTextFileRepA1 As Boolean = False
    Private _isChangedOpenTextFileRepA2 As Boolean = False
    Private _isChangedOpenTextFileRepA3 As Boolean = False
    Private _isChangedOpenTextFileRepA4 As Boolean = False
    'ボタンファイル名
    Private _buttonFile As New JManButtonFile
    'Jupiterインストール直後かどうかのフラグ
    Private _isNewJupiter As Boolean = False
    'コマンドライン引数
    Private _argment As String = ""
    '会話のみのライセンスかどうかのフラグ
    'Private _isKaiwaOnly As Boolean = False
    '一括処理を使用できるかどうかのフラグ(0:使用可能(スタンドアロン版)、1:使用可能(ネットワーク版)、2:使用不可(スタンドアロン版)、3:使用不可(ネットワーク版)
    Private _isUseBatch As Integer = 0
    '会話を使用できるかどうかのフラグ(0:使用可能(スタンドアロン版)、1:使用可能(ネットワーク版)、2:使用不可(スタンドアロン版)、3:使用不可(ネットワーク版)
    Private _isUseKaiwa As Integer = 0
    'ライセンスチェックがOKだったかどうかのフラグ(ネットワークランセンスを返却する際に使用)
    Private _isOkLicense As Boolean = False

    Private Function checkOpenedKojiFolder() As Integer

        checkOpenedKojiFolder = 0

        If Me.TextBoxKojiFolder.Text = "" Then
            MessageBox.Show("[開く]ボタンで工事フォルダを選択してください。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            checkOpenedKojiFolder = 1
        End If

    End Function

    Private Sub OnInit_MainWindow(sender As Object, e As EventArgs) Handles MyBase.Initialized

        'コマンドライン引数を取得する。
        If Environment.GetCommandLineArgs.Length > 1 Then
            Me._argment = Environment.GetCommandLineArgs(1)
        End If

        'タブマネージャー
        _tabManager = New TabPageManager(Me.TabCtl)

        ' プロパティに値をセットする。
        Me.setProperty()

        ' ボタン色、背景色、アイコンを変更する
        Me.changeColor()

        If Me._isNewJupiter = True Then
            Me.kojiNewDialog()
        Else
            ' 前回開いていた工事を開く。
            Me.openInitKoji()

            'ライセンスチェック
            Dim status As Integer = Me.checkLicense(Me._kojiFolder)
            If status = 1 Then
                Me.kojiOpenDialog(1)
                Return
            ElseIf status = 2 Then
                Me.Close()
            End If

            Me._isOkLicense = True

            ' JmanDisp_*.tbsを読み込み、タブの表示を変更する。
            status = Me.dispTab()
            If status <> 0 Then
                Me.Close()
            End If
        End If

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   ライセンスチェック
    '引数
    '   無し
    '戻り値
    '   0:OK
    '   1:NG(工事を開くダイアログボックスを開く)
    '   2:NG(起動できない)
    '---------------------------------------------------------------------------------------------------
    Private Function checkLicense(kojiFolder As String) As Integer

        Dim osub, recno, typbrg As Short
        Dim chargeCode As String = ""
        Dim brgname As String = ""
        Dim Pfile As String = ""
        Dim syugeta As String = ""
        Dim msg As String
        Dim strFileName As String
        Dim flgErr As Short
        Dim SysFile As String
        Dim folderName As String = subStringEnd(kojiFolder.TrimEnd("\"), "\")

        checkLicense = 0
        strFileName = FunGetLanguage()

        '処理工事の橋梁形式とｾﾝﾁﾈﾙの実行可能橋梁形式を比較する。
        'ﾌｧｲﾙがあるときﾌﾟﾛﾃｸｼｮﾝ判定をする。
        Me._isUseBatch = 0
        Me._isUseKaiwa = 0
        'Me._isKaiwaOnly = False
        SysFile = FunGetSystemFile()
        If SysFile <> "" Then
            '選択工事のﾁｬｰｼﾞｺｰﾄﾞをﾁｪｯｸする。
            recno = funチャージコードの有無(folderName, chargeCode)
            If recno < 0 Then
                If chargeCode = "FREE-JUPITER" Then
                    chargeCode = ""
                End If

                Beep()
                If Not strFileName Like "*ENG.INI" Then
                    msg = "入力されたチャージコード[" & chargeCode & "]がCHARGE.INIに登録されていません。" & Chr(10)
                    msg = msg & "チャージコードの登録を行わないと処理が出来ません。" & Chr(10)
                    msg = msg & "登録を行って下さい。"
                Else
                    msg = "Inputted charge code （" & chargeCode & "）is not registered into a charge code registration file." & Chr(10)
                    msg = msg & "Processing is impossible unless it registers a charge code" & Chr(10)
                    msg = msg & "Please register."
                End If
                MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
                checkLicense = 1
                Exit Function
            End If

            typbrg = funCHARGE橋梁形式の取得(folderName)
            If typbrg < 0 Then

                Beep()
                If Not strFileName Like "*ENG.INI" Then
                    msg = "CHARGE.INIに構造形式が登録されていません。" & Chr(10)
                    msg = msg & "CHARGE.INIの内容を確認してください。"
                Else
                    msg = "Structure form is not registered into a charge code registration file (CHARGE.INI)." & Chr(10)
                    msg = msg & "Please check the contents of a charge code registration file."
                End If
                MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
                checkLicense = 1
                Exit Function
            End If

            If ライセンスタイプの取得() = 3 Then   'スタンドアロン版
                Call Jupiterユーザー確認(251, osub)
                'ライセンスを初期化(USBからライセンスファイルをコピー)して、認証を行う.
                If InitializeLicense_JPT() <> 0 Then
                    Beep()
                    If Not strFileName Like "*ENG.INI" Then
                        msg = "CastarJupiterプロテクトに問題があります。" & Chr(10)
                        msg = msg & "USBプロテクトの装着を確かめてください。" & Chr(10)
                        msg = msg & "正しく装着されている場合は、発売元に連絡して下さい。"
                    Else
                        msg = "There is a problem in CastarJupiter protection." & Chr(10)
                        msg = msg & "Please confirm wearing of protection." & Chr(10)
                        msg = msg & "Please inform a distributor, when equipped correctly." & Chr(10)
                        msg = msg & "Please check the contents of protection." & Chr(10)
                    End If
                    MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    checkLicense = 2
                    Exit Function
                Else
                    If オプション使用の取得(ProductSystemNo253) Then
                        '会話処理が使用可能な場合は、System.txtに工事フォルダを書き込む。
                        writeSystemTxt(SysFile, kojiFolder.TrimEnd("\"))
                        Me._isUseKaiwa = 0
                    Else
                        '会話処理が使用不可場合は、System.txtに空白を書き込む。
                        writeSystemTxtClose(SysFile)
                        Me._isUseKaiwa = 2
                    End If

                    '2017/03/18 Nakagawa Add Start
                    'チャージコードがFREE-JUPITERの場合
                    If typbrg = 0 Then
                        '工事別ﾌﾟﾛﾌｧｲﾙから橋梁形式を読み込む。
                        Pfile = kojiFolder.TrimEnd("\") + "\PROFILE.INI"
                        readProfileIni(Pfile, syugeta, chargeCode, recno)
                        If syugeta = "RC箱桁" Then
                            typbrg = TypRcB
                        ElseIf syugeta = "鋼床版箱桁" Then
                            typbrg = TypDkB
                        ElseIf syugeta = "RC鈑桁" Then
                            typbrg = TypRcI
                        ElseIf syugeta = "汎用" Then
                            typbrg = TypGen
                        ElseIf syugeta = "鋼製橋脚" Then
                            typbrg = TypPier
                        ElseIf syugeta = "合成床版" Then
                            typbrg = TypPSlb
                        ElseIf syugeta = "波形ウェブ" Then
                            typbrg = TypWWeb
                        ElseIf syugeta = "鋼製セグメント" Then
                            typbrg = TypSeg
                        End If
                    End If
                    '2017/03/18 Nakagawa Add End

                    If (osub And typbrg) <> typbrg Then
                        If Me._isUseKaiwa = 0 Then
                            Me._isUseBatch = 2
                        Else
                            If typbrg = TypRcB Then
                                brgname = "RC箱桁"
                            ElseIf typbrg = TypDkB Then
                                brgname = "鋼床版箱桁"
                            ElseIf typbrg = TypRcI Then
                                brgname = "RC鈑桁"
                            ElseIf typbrg = TypGen Then
                                brgname = "汎用"
                            ElseIf typbrg = TypPier Then
                                brgname = "鋼製橋脚"
                            ElseIf typbrg = TypPSlb Then
                                brgname = "合成床版"
                            ElseIf typbrg = TypWWeb Then
                                brgname = "波形ウェブ"
                            ElseIf typbrg = TypSeg Then
                                brgname = "鋼製セグメント"
                            End If
                            Beep()
                            If Not strFileName Like "*ENG.INI" Then
                                msg = "CHARGE.INIに登録されている構造形式[ " & brgname & " ]は" & Chr(10)
                                msg = msg & "装着されているUSBプロテクトでは実行できません。" & Chr(10)
                                msg = msg & "CHARGE.INI、USBプロテクトの内容を確認してください。"
                            Else
                                msg = "The structure form(" & brgname & ") registered into the charge code registration file (CHARGE) " & Chr(10)
                                msg = msg & "cannot perform by this protection." & Chr(10)
                                msg = msg & "Please check the contents of a charge code registration file and protection."
                            End If
                            MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            checkLicense = 1
                        End If
                        Exit Function
                    End If
                    '工事別ﾌﾟﾛﾌｧｲﾙから橋梁形式を読み込む。
                    Pfile = kojiFolder.TrimEnd("\") + "\PROFILE.INI"
                    readProfileIni(Pfile, syugeta, chargeCode, recno)
                    flgErr = 0
                    If (typbrg = TypRcB) And (syugeta <> "RC箱桁") Then
                        brgname = "RC箱桁"
                        flgErr = 1
                    ElseIf (typbrg = TypDkB) And (syugeta <> "鋼床版箱桁") Then
                        brgname = "鋼床版箱桁"
                        flgErr = 1
                    ElseIf (typbrg = TypRcI) And (syugeta <> "RC鈑桁") Then
                        brgname = "RC鈑桁"
                        flgErr = 1
                    ElseIf (typbrg = TypPier) And (syugeta <> "鋼製橋脚") Then
                        brgname = "鋼製橋脚"
                        flgErr = 1
                    ElseIf (typbrg = TypPSlb) And (syugeta <> "合成床版") Then
                        brgname = "合成床版"
                        flgErr = 1
                    ElseIf (typbrg = TypWWeb) And (syugeta <> "波形ウェブ") Then
                        brgname = "波形ウェブ"
                        flgErr = 1
                    ElseIf (typbrg = TypSeg) And (syugeta <> "鋼製セグメント") Then
                        brgname = "鋼製セグメント"
                        flgErr = 1
                    End If
                    If flgErr = 1 Then
                        Beep()
                        If Not strFileName Like "*ENG.INI" Then
                            msg = "CHARGE.INIに登録されている構造形式[ " & brgname & " ]と" & Chr(10)
                            msg = msg & "選択した工事の構造形式[ " & syugeta & " ]が合っていません。" & Chr(10)
                            msg = msg & "CHARGE.INI、選択工事の内容を確認してください。"
                        Else
                            msg = "The structure form (" & brgname & ")registered into the charge code registration file (CHARGE.INI)" & Chr(10)
                            msg = msg & "and the structure form(" & syugeta & ")of clue origin are not correct." & Chr(10)
                            msg = msg & "Please check the contents of charge code registration file and clue origin."
                        End If
                        MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        checkLicense = 1
                        Exit Function
                    End If
                End If
                'ここでは「ネットワークライセンスの取得」をしない。
                'Jupiterマネージャーをロードするときに取得する。
                '**************************************************************************************************************
            Else       'ネットワーク版
                Call Jupiterユーザー確認(251, osub)
                'チャージがFREE-JUPITERの場合はPROFILE.INIから橋梁形式を取得する
                If typbrg = 0 Then
                    Pfile = kojiFolder.TrimEnd("\") + "\PROFILE.INI"
                    readProfileIni(Pfile, syugeta, chargeCode, recno)

                    If syugeta = "RC箱桁" Then
                        typbrg = TypRcB
                    ElseIf syugeta = "RC鈑桁" Then
                        typbrg = TypRcI
                    ElseIf syugeta = "鋼床版箱桁" Then
                        typbrg = TypDkB
                    ElseIf syugeta = "汎用" Then
                        typbrg = TypGen
                    ElseIf syugeta = "鋼製橋脚" Then
                        typbrg = TypPier
                    ElseIf syugeta = "合成床版" Then
                        typbrg = TypPSlb
                    ElseIf syugeta = "波形ウェブ" Then
                        typbrg = TypWWeb
                    ElseIf syugeta = "鋼製セグメント" Then
                        typbrg = TypSeg
                    End If
                End If

                If ((typbrg > 0) And (typbrg < 8)) Or (typbrg = 16) Or (typbrg = 32) Or (typbrg = 64) Or (typbrg = 128) Then
                    If (osub And typbrg) <> typbrg Then
                        If typbrg = TypRcB Then
                            brgname = "RC箱桁"
                        ElseIf typbrg = TypDkB Then
                            brgname = "鋼床版箱桁"
                        ElseIf typbrg = TypRcI Then
                            brgname = "RC鈑桁"
                        ElseIf typbrg = TypGen Then
                            brgname = "汎用"
                        ElseIf typbrg = TypPier Then
                            brgname = "鋼製橋脚"
                        ElseIf typbrg = TypPSlb Then
                            brgname = "合成床版"
                        ElseIf typbrg = TypWWeb Then
                            brgname = "波形ウェブ"
                        ElseIf typbrg = TypSeg Then
                            brgname = "鋼製セグメント"
                        End If

                        '----- 2012/03/15 桁形式の使用可否をを判定する。 -----
                        'ﾌｧｲﾙがあるとき断面使用の確認をする。
                        '                                typbrg = funCHARGE橋梁形式の取得(KojiName)
                        If 断面使用の確認(251, typbrg) = False Then 'SysNo = 251
                            Beep()
                            If Not strFileName Like "*ENG.INI" Then
                                msg = "構造形式で「" & brgname & "」を選択したが、このCastarJupiterのﾊﾞｰｼﾞｮﾝでは実行出来ません。" & Chr(10)
                                msg = msg & brgname & "が使用可能なﾊﾞｰｼﾞｮﾝを御利用下さい。"
                            Else
                                msg = "Although「" & brgname & "」was chosen in structure form, it cannot perform in the version of this CastarJupiter." & Chr(10)
                                msg = msg & "Please use the version which " & brgname & " can use."
                            End If
                            MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            checkLicense = 2
                            Exit Function
                        End If
                    End If
                End If

                Me._isUseBatch = 3
                Me._isUseKaiwa = 3

                If Me._argment = "KAIWA" Or Me._argment = "" Then  '会話
                    If ネットワークライセンスの取得(ProductSystemNo253, 0) = True Then
                        writeSystemTxt(SysFile, kojiFolder.TrimEnd("\"))
                        Me._isUseKaiwa = 1
                    Else
                        writeSystemTxtClose(SysFile)
                        Me._isUseKaiwa = 3
                        If Me._argment = "KAIWA" Then
                            msg = "会話処理のライセンスが取得できませんでした。" + vbCrLf
                            msg += "会話処理が使用可能か、または最大使用数を超えていないかを確認してください。" + vbCrLf
                            MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            checkLicense = 2
                            Exit Function
                        End If
                    End If
                End If

                If Me._argment = "BATCH" Or Me._argment = "" Then      '部材一括
                    If ネットワークライセンスの取得(ProductSystemNo251, typbrg) = True Then
                        Me._isUseBatch = 1
                    Else
                        If Not strFileName Like "*ENG.INI" Then
                            msg = "一括処理のライセンスが取得できませんでした。" + vbCrLf
                            msg += "一括処理が使用可能か、または最大使用数を超えていないかを確認してください。" + vbCrLf
                            msg += "JupiterManagerを起動しますが、一括処理は使用できません。"
                        Else
                            msg = "The license of batch processing was not able to be acquired." + vbCrLf
                            msg += "Please check whether batch processing can be used or it is not over the number of the maximum use." + vbCrLf
                            msg += "Batch processing cannot be used although Jupiter is performed."
                        End If
                        MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Me._isUseBatch = 3
                        checkLicense = 2
                        Exit Function
                    End If
                End If

            End If
        End If

        subSetActiveData(folderName)

    End Function

    '---------------------------------------------------------------------------------------------------
    '機能
    '   工事フォルダを検索する。
    '引数
    '   無し
    '戻り値
    '   無し
    '---------------------------------------------------------------------------------------------------
    Private Function setKojiFolder() As Integer

        If _kojiFolder = "" Then
            _kojiFolder = FunGetKoji()
            If _kojiFolder = "" Then
                'Jmanage.iniからACTIVE_DATAが見つからない場合は、工事フォルダ内の最初のフォルダを工事フォルダとする。
                Dim newFolder As String = FunGetNewKoji2()
                If newFolder = "" Then
                    Return 1
                End If
                Dim subFolders As String() = System.IO.Directory.GetDirectories(newFolder, "*")
                Dim i As Integer
                For i = 0 To subFolders.Count() - 1
                    Dim profileName As String = subFolders(i).TrimEnd("\") + "\Profile.ini"
                    If System.IO.File.Exists(profileName) Then
                        _kojiFolder = subFolders(i) + "\"
                        Return 0
                    End If
                Next
            End If
        End If

        Return 1

    End Function


    '---------------------------------------------------------------------------------------------------
    '機能
    '   プロパティに値をセットする。
    '引数
    '   無し
    '戻り値
    '   無し
    '---------------------------------------------------------------------------------------------------
    Private Sub setProperty()

        _environFolderName = FunGetEnviron()

        '工事情報
        Me.setKojiFolder()
        If Me._kojiFolder = "" Then
            _isNewJupiter = True
            Return
        End If
        Dim profileFileName As String = _kojiFolder + "\profile.ini"
        Dim strTmp As String = ""
        Dim intTmp As Integer = 0
        _kozoNo = readProfileIni(profileFileName, _kozo, strTmp, intTmp)

        '設定ファイル
        Me._setingFileName.OnInitialize(Me._kozoNo, Me._kojiFolder, Me._environFolderName)
        If Me._kozoNo = 6 Then
            Me.LabelBziData.Text = "部材データ(SegData.dat)を確認・編集します。"
        Else
            Me.LabelBziData.Text = "部材データ(BziData.dat)を確認・編集します。"
        End If

        '関連アプリケーションパス
        _ijcadExe = FunGetAcad()
        _excelExe = FunGetExcel()
        _accessExe = FunGetAccess()
        Dim editorExeTmp As String = FunGetAppNo(0)
        fun文字列抽出(editorExeTmp, "[", "]", _editorExe)

        '画面設定
        Dim strLang As String = ""
        subGetOther("Language", "Language", "JPN", strLang)
        If strLang = "JPN" Then
            _language = 0
        Else
            _language = 1
        End If

        Dim strButColor As String = ""
        subGetOther("Color", "ButtonColor", "GRAY", strButColor)
        If strButColor = "GRAY" Then
            _buttonColor = 0
        Else
            _buttonColor = 1
        End If

        'Dim strBakColor As String = ""
        'subGetOther("Color", "BackColor", "GRAY", strBakColor)
        'If strBakColor = "GRAY" Then
        '    _backColor = 0
        'Else
        '    _backColor = 1
        'End If

        '_isChangedOpenTextFile = False
        _isChangedOpenTextFileSkl = False
        Me._isChangedOpenTextFileBzi = False
        Me._isChangedOpenTextFileSeisan = False
        Me._isChangedOpenTextFileRepA1 = False
        Me._isChangedOpenTextFileRepA2 = False
        Me._isChangedOpenTextFileRepA3 = False
        Me._isChangedOpenTextFileRepA4 = False

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   前回開いていた工事を開く。
    '引数
    '   無し
    '戻り値
    '   無し
    '---------------------------------------------------------------------------------------------------
    Private Sub openInitKoji()

        If System.IO.File.Exists(Me._setingFileName._constInfFileName) = True Then
            Dim constInf As New StreamReader(Me._setingFileName._constInfFileName, Encoding.GetEncoding("Shift_JIS"))
            Dim lineText As String = constInf.ReadLine()
            constInf.Close()
            '工事フォルダからフォルダ名を取得する。(最後の\以降の文字列を取得する)
            Me.TextBoxKogo.Text = J_ChoiceString(lineText, 1)
            Me.TextBoxKojiName.Text = J_ChoiceString(lineText, 2)
            Me.TextBoxKojiFolder.Text = _kojiFolder
            Me.TextBoxKozo.Text = _kozo
        End If

    End Sub
    '---------------------------------------------------------------------------------------------------
    '機能
    '   ボタン色、背景色、アイコンを変更する
    '引数
    '   無し
    '戻り値
    '   無し
    '---------------------------------------------------------------------------------------------------
    Private Sub changeColor()

        'ブラシの設定
        Dim backBrush As SolidColorBrush
        Dim tabBackBrush As SolidColorBrush
        Dim tabBorderBrush As SolidColorBrush
        Dim tabStyle As New Style()
        If _buttonColor = 0 Then
            backBrush = New SolidColorBrush(System.Windows.Media.Colors.DarkGray)
            tabBackBrush = New SolidColorBrush(System.Windows.Media.Colors.Gainsboro)
            tabBorderBrush = New SolidColorBrush(System.Windows.Media.Colors.Gainsboro)
            tabStyle = Application.Current.FindResource("TabItemStyle")
        Else
            backBrush = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 164, 150))
            tabBackBrush = New SolidColorBrush(System.Windows.Media.Colors.White)
            tabBorderBrush = New SolidColorBrush(System.Windows.Media.Colors.Gainsboro)
            tabStyle = Application.Current.FindResource("TabItemStyleGreen")
        End If

        'タブのスタイルを設定
        Me.TabSkl.Style = tabStyle
        Me.TabBzi.Style = tabStyle
        Me.TabSeisan.Style = tabStyle
        Me.TabRepA1.Style = tabStyle
        Me.TabRepA2.Style = tabStyle
        Me.TabRepA3.Style = tabStyle
        Me.TabRepA4.Style = tabStyle
        Me.TabRepB1.Style = tabStyle
        Me.TabRepB2.Style = tabStyle
        Me.TabRepB3.Style = tabStyle

        '背景を変更する。
        Me.Background = backBrush
        Me.TabCtl.Background = tabBackBrush
        Me.TabCtl.BorderBrush = tabBorderBrush

        'ボタンのアイコンを変更する
        changeIcon(1)
        changeIcon(2)
        changeIcon(3)
        changeIcon(4)
        changeIcon(5)
        changeIcon(6)
        changeIcon(7)
        changeIcon(8)
        changeIcon(9)
        changeIcon(10)
        changeIcon(11)
        changeIcon(12)
        changeIcon(13)
        changeIcon(16)
        changeIcon(17)
        changeIcon(18)
        changeIcon(19)

        '矢印の画像を変更する。
        changeIcon(101)
        changeIcon(102)
        changeIcon(103)

        'ボタンの色を変更する。
        Me.changeButtonColor()

    End Sub
    '---------------------------------------------------------------------------------------------------
    '機能
    '   指定したボタンのアイコンを変更する
    '引数
    '   buttonNo(I)：ボタン番号
    '戻り値
    '   無し
    '---------------------------------------------------------------------------------------------------
    Private Sub changeIcon(buttonNo As Integer)

        Dim endName As String = ""
        If _buttonColor = 1 Then
            endName = "_G"
        End If

        Dim iconFileName As String = ""
        Select Case UCase(buttonNo)
            Case 1
                iconFileName = "MenuIcon/01_New" + endName + ".png"
            Case 2
                iconFileName = "MenuIcon/02_Open" + endName + ".png"
            Case 3
                iconFileName = "MenuIcon/03_KojiEdit" + endName + ".png"
            Case 4
                iconFileName = "MenuIcon/04_Jconsole" + endName + ".png"
            Case 5
                iconFileName = "MenuIcon/05_Log" + endName + ".png"
            Case 6
                iconFileName = "MenuIcon/06_Standard" + endName + ".png"
            Case 7
                iconFileName = "MenuIcon/07_Weight" + endName + ".png"
            Case 8
                iconFileName = "MenuIcon/08_Pmf" + endName + ".png"
            Case 9
                iconFileName = "MenuIcon/09_Cl" + endName + ".png"
            Case 10
                iconFileName = "MenuIcon/10_Dxf" + endName + ".png"
            Case 11
                iconFileName = "MenuIcon/11_IJCAD" + endName + ".png"
            Case 12
                iconFileName = "MenuIcon/12_EXCEL" + endName + ".png"
            Case 13
                iconFileName = "MenuIcon/13_ACCESS" + endName + ".png"
            Case 16
                iconFileName = "MenuIcon/16_SETTING" + endName + ".png"
            Case 17
                iconFileName = "MenuIcon/17_HELP" + endName + ".png"
            Case 18
                iconFileName = "MenuIcon/18_SAVE" + endName + ".png"
            Case 19
                iconFileName = "MenuIcon/19_TxtEdit" + endName + ".png"
            Case 101
                iconFileName = "Picture/Y01_LOW" + endName + ".png"
            Case 102
                iconFileName = "Picture/Y02_LOW" + endName + ".png"
            Case 103
                iconFileName = "Picture/Y03_LOW" + endName + ".png"
            Case Else
                Return
        End Select

        Dim bmpImg As New BitmapImage()
        bmpImg.BeginInit()
        bmpImg.UriSource = New Uri(iconFileName, UriKind.Relative)
        bmpImg.EndInit()

        Select Case UCase(buttonNo)
            Case 1
                Me.Image_New.Source = bmpImg
            Case 2
                Me.Image_Open.Source = bmpImg
            Case 3
                Me.Image_KojiEdit.Source = bmpImg
            Case 4
                Me.Image_Jconsole.Source = bmpImg
            Case 5
                Me.Image_Log.Source = bmpImg
            Case 6
                Me.Image_Standard.Source = bmpImg
            Case 7
                Me.Image_Weight.Source = bmpImg
            Case 8
                Me.Image_Pmf.Source = bmpImg
            Case 9
                Me.Image_Cl.Source = bmpImg
            Case 10
                Me.Image_Dxf.Source = bmpImg
            Case 11
                Me.Image_IJCAD.Source = bmpImg
            Case 12
                Me.Image_EXCEL.Source = bmpImg
            Case 13
                Me.Image_ACCESS.Source = bmpImg
            Case 16
                Me.Image_SETTING.Source = bmpImg
            Case 17
                Me.Image_HELP.Source = bmpImg
            Case 18
                Me.Image_SklSave.Source = bmpImg
                Me.Image_BziSave.Source = bmpImg
                Me.Image_SeisanSave.Source = bmpImg
                Me.Image_RepA1Save.Source = bmpImg
                Me.Image_RepA2Save.Source = bmpImg
                Me.Image_RepA3Save.Source = bmpImg
                Me.Image_RepA4Save.Source = bmpImg
            Case 19
                Me.Image_SklEdit.Source = bmpImg
                Me.Image_BziEdit.Source = bmpImg
                Me.Image_SeisanEdit.Source = bmpImg
                Me.Image_RepA1Edit.Source = bmpImg
                Me.Image_RepA2Edit.Source = bmpImg
                Me.Image_RepA3Edit.Source = bmpImg
                Me.Image_RepA4Edit.Source = bmpImg
            Case 101
                Me.ImageSklLow1.Source = bmpImg
                Me.ImageSklLow2.Source = bmpImg
                Me.ImageBziLow1.Source = bmpImg
                Me.ImageBziLow2.Source = bmpImg
                Me.ImageBziKaiwaLow1.Source = bmpImg
                Me.ImageBziKaiwaLow2.Source = bmpImg
                Me.ImageSeiLow9.Source = bmpImg
                Me.ImageSeiLow10.Source = bmpImg
                Me.ImageSeiLow11.Source = bmpImg
            Case 102
                Me.ImageSeiLow1.Source = bmpImg
                Me.ImageSeiLow2.Source = bmpImg
                Me.ImageSeiLow3.Source = bmpImg
                Me.ImageSeiLow4.Source = bmpImg
                Me.ImageSeiLow5.Source = bmpImg
                Me.ImageSeiLow6.Source = bmpImg
            Case 103
                Me.ImageSeiLow7.Source = bmpImg
                Me.ImageSeiLow8.Source = bmpImg
            Case Else
                Return
        End Select

    End Sub
    '---------------------------------------------------------------------------------------------------
    '機能
    '   指定したボタンの色を変更する
    '引数
    '戻り値
    '   無し
    '---------------------------------------------------------------------------------------------------
    Private Sub changeButtonColor()

        Dim buttonColor1 As SolidColorBrush
        Dim buttonColor2 As SolidColorBrush
        If _buttonColor = 0 Then
            buttonColor1 = New SolidColorBrush(System.Windows.Media.Colors.Black)
            buttonColor2 = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 90, 90, 90))
        Else
            buttonColor1 = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 164, 150))
            buttonColor2 = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 225, 150))
        End If

        '骨組タブ
        Me.ButtonSample1.Background = buttonColor1
        Me.ButtonSample2.Background = buttonColor2
        Me.ButtonSample3.Background = buttonColor1
        Me.ButtonSample4.Background = buttonColor2
        Me.ButtonSample5.Background = buttonColor1
        Me.ButtonSample6.Background = buttonColor2
        Me.ButtonSklPknd.Background = buttonColor2
        Me.ButtonSklData.Background = buttonColor1
        Me.ButtonSkmake.Background = buttonColor1
        Me.ButtonSklReport1.Background = buttonColor1
        Me.ButtonSklReport2.Background = buttonColor1
        Me.ButtonSklReport3.Background = buttonColor1
        Me.ButtonSklReport4.Background = buttonColor1
        Me.ButtonSklReport5.Background = buttonColor1
        Me.ButtonSklReport6.Background = buttonColor1
        Me.ButtonSklReport7.Background = buttonColor1

        '部材タブ
        Me.ButtonBziSample1.Background = buttonColor1
        Me.ButtonBziSample2.Background = buttonColor2
        Me.ButtonBziSample3.Background = buttonColor1
        Me.ButtonBziSample4.Background = buttonColor2
        Me.ButtonBziSample5.Background = buttonColor1
        Me.ButtonBziSample6.Background = buttonColor2
        Me.ButtonAnaBolt.Background = buttonColor2
        Me.ButtonBziHead.Background = buttonColor2
        Me.ButtonBziData.Background = buttonColor1
        Me.ButtonBziBatch.Background = buttonColor1
        Me.ButtonKaiwaRireki.Background = buttonColor2
        Me.ButtonKaiwaScr.Background = buttonColor2
        Me.ButtonBziKaiwa.Background = buttonColor2
        Me.ButtonBziKaiwaBatch.Background = buttonColor2
        Me.ButtonKanshoChk.Background = buttonColor1
        Me.ButtonKanchoChkView.Background = buttonColor1
        Me.ButtonBziTouei.Background = buttonColor2
        Me.ButtonToueiTorikomi.Background = buttonColor2

        '生産関連タブ
        Me.ButtonSeiSample1.Background = buttonColor1
        Me.ButtonSeiSample2.Background = buttonColor2
        Me.ButtonSeiSample3.Background = buttonColor1
        Me.ButtonSeiSample4.Background = buttonColor2
        Me.ButtonSeiSample5.Background = buttonColor1
        Me.ButtonSeiSample6.Background = buttonColor2
        Me.ButtonDelBlk.Background = buttonColor2
        Me.ButtonKak.Background = buttonColor1
        Me.ButtonSyu.Background = buttonColor1
        Me.ButtonLot.Background = buttonColor2
        Me.ButtonPsort.Background = buttonColor1
        Me.ButtonBsort.Background = buttonColor1
        Me.ButtonPmkHeadSet.Background = buttonColor2
        Me.ButtonBmkHeadSet.Background = buttonColor2
        Me.ButtonSeisanSetOther.Background = buttonColor2
        Me.ButtonDevDeform.Background = buttonColor1
        Me.ButtonSort.Background = buttonColor1
        Me.ButtonSeisanBatch.Background = buttonColor2
        Me.ButtonPmkHead.Background = buttonColor2
        Me.ButtonBmkHead.Background = buttonColor2

        '帳票関連Bタブ
        Me.ButtonRepSample1A1.Background = buttonColor1
        Me.ButtonRepSample2A1.Background = buttonColor2
        Me.ButtonRepSample3A1.Background = buttonColor1
        Me.ButtonRepSample4A1.Background = buttonColor2
        Me.ButtonRepSample5A1.Background = buttonColor1
        Me.ButtonRepSample6A1.Background = buttonColor2
        Me.ButtonRepSample1A2.Background = buttonColor1
        Me.ButtonRepSample2A2.Background = buttonColor2
        Me.ButtonRepSample3A2.Background = buttonColor1
        Me.ButtonRepSample4A2.Background = buttonColor2
        Me.ButtonRepSample5A2.Background = buttonColor1
        Me.ButtonRepSample6A2.Background = buttonColor2
        Me.ButtonRepSample1A3.Background = buttonColor1
        Me.ButtonRepSample2A3.Background = buttonColor2
        Me.ButtonRepSample3A3.Background = buttonColor1
        Me.ButtonRepSample4A3.Background = buttonColor2
        Me.ButtonRepSample5A3.Background = buttonColor1
        Me.ButtonRepSample6A3.Background = buttonColor2
        Me.ButtonRepSample1A4.Background = buttonColor1
        Me.ButtonRepSample2A4.Background = buttonColor2
        Me.ButtonRepSample3A4.Background = buttonColor1
        Me.ButtonRepSample4A4.Background = buttonColor2
        Me.ButtonRepSample5A4.Background = buttonColor1
        Me.ButtonRepSample6A4.Background = buttonColor2

        '帳票関連Bタブ
        Me.ButtonRepSample1B1.Background = buttonColor1
        Me.ButtonRepSample2B1.Background = buttonColor2
        Me.ButtonRepSample3B1.Background = buttonColor1
        Me.ButtonRepSample4B1.Background = buttonColor2
        Me.ButtonRepSample5B1.Background = buttonColor1
        Me.ButtonRepSample6B1.Background = buttonColor2
        Me.ButtonRepSample1B2.Background = buttonColor1
        Me.ButtonRepSample2B2.Background = buttonColor2
        Me.ButtonRepSample3B2.Background = buttonColor1
        Me.ButtonRepSample4B2.Background = buttonColor2
        Me.ButtonRepSample5B2.Background = buttonColor1
        Me.ButtonRepSample6B2.Background = buttonColor2
        Me.ButtonRepSample1B3.Background = buttonColor1
        Me.ButtonRepSample2B3.Background = buttonColor2
        Me.ButtonRepSample3B3.Background = buttonColor1
        Me.ButtonRepSample4B3.Background = buttonColor2
        Me.ButtonRepSample5B3.Background = buttonColor1
        Me.ButtonRepSample6B3.Background = buttonColor2

        '帳票関連動的配置ボタン
        Me.changeButtonColorRep("A1", buttonColor1, buttonColor2)
        Me.changeButtonColorRep("A2", buttonColor1, buttonColor2)
        Me.changeButtonColorRep("A3", buttonColor1, buttonColor2)
        Me.changeButtonColorRep("A4", buttonColor1, buttonColor2)
        Me.changeButtonColorRep("B1", buttonColor1, buttonColor2)
        Me.changeButtonColorRep("B2", buttonColor1, buttonColor2)
        Me.changeButtonColorRep("B3", buttonColor1, buttonColor2)

    End Sub
    '---------------------------------------------------------------------------------------------------
    '機能
    '   帳票関連ボタンの色を変更する
    '引数
    '戻り値
    '   無し
    '---------------------------------------------------------------------------------------------------
    Private Sub changeButtonColorRep(repType As String, buttonColor1 As SolidColorBrush, buttonColor2 As SolidColorBrush)
        Dim gridRep As New Grid()
        getRepGrid(repType, gridRep)
        Dim i As Integer = 0
        If gridRep.Children.Count <> 0 Then
            For Each obj In gridRep.Children
                Dim buttonInfo As New JManDispButtonInfo()
                Dim isFound As Integer = Me._buttonInfoListRep.find(obj.Name, buttonInfo)
                If isFound = 0 Then
                    Select Case buttonInfo._type
                        Case 1
                            obj.Background = buttonColor1
                        Case 3
                            obj.Background = buttonColor1
                        Case 5
                            obj.Background = buttonColor1
                        Case 2
                            obj.Background = buttonColor2
                        Case 4
                            obj.Background = buttonColor2
                        Case 6
                            obj.Background = buttonColor2
                    End Select
                End If
            Next
        End If

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   JmanDisp_*.tbsを読み込み、タブの表示を変更する。
    '引数
    '   無し
    '戻り値
    '   無し
    '---------------------------------------------------------------------------------------------------
    Private Function dispTab() As Integer

        dispTab = 0
        Dim i As Integer = 0

        'タブを全て表示状態にする。
        _tabManager.initTabManager()

        ' JmanDisp_*.tbsを読み込む
        Dim disp As JManDispFile = New JManDispFile()
        Dim status As Integer = disp.loadFile(Me._kozoNo)
        If status <> 0 Then
            MessageBox.Show("JmanDispファイルが読み込めないため、画面を表示できません。開発元に連絡してください。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            dispTab = 1
            Exit Function
        End If

        '骨組関連タブ
        Me.dispSklInit()
        Dim jtabSkl As New JManDispTab
        status = disp.findTab("SKL", jtabSkl)
        If status = 0 And Me._isUseBatch < 2 Then
            'タブのヘッダーを変更
            Me.TabSkl.Header = jtabSkl._caption
            'Buttonの表示・非表示・キャプションの変更
            Me.dispSklButton(jtabSkl)
            'Labelの表示・非表示・キャプションの変更
            Me.dispSklLabel(jtabSkl)
            'グループボックスのサイズ変更
            Me.dispSklGroupBox(jtabSkl)
        Else
            '骨組関連タブを非表示にする。
            _tabManager.ChangeTabPageVisible(0, False)
        End If

        '部材関連タブ
        Me.dispBziInit()
        Dim jtabBzi As New JManDispTab
        status = disp.findMatchTab("BZI*", jtabBzi)
        If status = 0 And Me._isUseBatch < 2 Then
            'タブのヘッダーを変更
            Me.TabBzi.Header = jtabBzi._caption

            'ライセンスを参照してボタンの表示・非表示を変更
            '会話処理
            If Me._isUseKaiwa < 2 Then
                Me.GroupBoxBziKaiwa.Visibility = False
            Else
                Dim msg As String = ""
                If Me._isUseKaiwa = 2 Then
                    msg = "ご使用のUSBプロテクトには会話処理のライセンスがありません。" + vbCrLf
                    msg += "JupiterManagerにて骨組一括、部材一括、生産処理、帳票作成は可能ですが、会話処理は使用できません。"
                ElseIf Me._isUseKaiwa = 3 And Me._argment <> "BATCH" Then
                    msg = "会話処理のライセンスが取得できませんでした。" + vbCrLf
                    msg += "会話処理が使用可能か、または最大使用数を超えていないかを確認してください。" & Chr(10)
                    msg += "JupiterManagerにて骨組一括、部材一括、生産処理、帳票作成は可能ですが、会話処理は使用できません。"
                End If
                If msg <> "" Then
                    MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                Me.GroupBoxBziKaiwa.Visibility = True
            End If
            If Me._kozoNo = 6 Then
                Me.GroupBoxBziOption.Visibility = True
            Else
                'フランジ面罫書
                If オプション機能の取得(42) = True Then
                    Me.ButtonBziTouei.Visibility = False
                    Me.ButtonToueiTorikomi.Visibility = False
                    Me.BorderFlgKegaki.Visibility = False
                    Me.LabelBziFlgKegaki.Visibility = False
                Else
                    Me.ButtonBziTouei.Visibility = True
                    Me.ButtonToueiTorikomi.Visibility = True
                    Me.BorderFlgKegaki.Visibility = True
                    Me.LabelBziFlgKegaki.Visibility = True
                End If
            End If
        Else
            '部材関連タブを非表示にする。
            _tabManager.ChangeTabPageVisible(1, False)
        End If

        '生産関連タブ
        Me.dispSeisanInit()
        Dim jtabSeisan As New JManDispTab
        status = disp.findTab("SEISAN", jtabSeisan)
        If status = 0 And Me._isUseBatch < 2 Then
            'タブのヘッダーを変更
            Me.TabSeisan.Header = jtabSeisan._caption
        Else
            '生産関連タブを非表示にする。
            _tabManager.ChangeTabPageVisible(2, False)
        End If
        Me.SeisanRichTextBox.Document.Blocks.Clear()

        '帳票関連Aタブ
        Me._buttonInfoListRep.clearList()
        Me.dispRepAInit()
        Me.deleteRebTabChildren("A1")
        Me.deleteRebTabChildren("A2")
        Me.deleteRebTabChildren("A3")
        Me.deleteRebTabChildren("A4")
        Dim jtabRepAList As New ArrayList
        If Me._isUseBatch < 2 Then
            disp.findTabList("REP1", jtabRepAList)
        End If
        For i = 0 To 3
            If i < jtabRepAList.Count Then
                Dim repName As String = "A" + (i + 1).ToString()
                'タブのヘッダーを変更
                Select Case repName
                    Case "A1"
                        Me.TabRepA1.Header = jtabRepAList(i)._caption
                    Case "A2"
                        Me.TabRepA2.Header = jtabRepAList(i)._caption
                    Case "A3"
                        Me.TabRepA3.Header = jtabRepAList(i)._caption
                    Case "A4"
                        Me.TabRepA4.Header = jtabRepAList(i)._caption
                End Select
                'タブにボタンを追加
                Me.createRepButton(repName, jtabRepAList(i))
                'タブにテキストブロックを追加
                Me.createRepTextBlock(repName, jtabRepAList(i))
                'タブにグループボックスを追加
                Me.createRepGroupBox(repName, jtabRepAList(i))
            Else
                '未定義タブを非表示にする。
                _tabManager.ChangeTabPageVisible(3 + i, False)
            End If
        Next

        '帳票関連Bタブ
        Me.deleteRebTabChildren("B1")
        Me.deleteRebTabChildren("B2")
        Me.deleteRebTabChildren("B3")
        Dim jtabRepBList As New ArrayList
        If Me._isUseBatch < 2 Then
            disp.findTabList("REP2", jtabRepBList)
        End If
        For i = 0 To 2
            If i < jtabRepBList.Count Then
                Dim repName As String = "B" + (i + 1).ToString()
                'タブのヘッダーを変更
                Select Case repName
                    Case "B1"
                        Me.TabRepB1.Header = jtabRepBList(i)._caption
                    Case "B2"
                        Me.TabRepB2.Header = jtabRepBList(i)._caption
                    Case "B3"
                        Me.TabRepB3.Header = jtabRepBList(i)._caption
                End Select
                'タブにボタンを追加
                Me.createRepButton(repName, jtabRepBList(i))
                'タブにテキストブロックを追加
                Me.createRepTextBlock(repName, jtabRepBList(i))
                'タブにグループボックスを追加
                Me.createRepGroupBox(repName, jtabRepBList(i))
            Else
                '未定義タブを非表示にする。
                _tabManager.ChangeTabPageVisible(7 + i, False)
            End If
        Next

        'スタンドアロン版で会話のみの場合は、メッセージを出力
        If Me._isUseBatch = 2 And Me._isUseKaiwa = 0 Then
            Dim msg As String = ""
            '2017/03/18 Nakagawa Edit Start
            'msg = "ご使用のUSBプロテクトは会話処理のみのライセンスです。" + vbCrLf
            msg = "ご使用のUSBプロテクトでは、選択した工事の構造形式が使用できないライセンスまたは会話処理のみのライセンスです。" + vbCrLf
            '2017/03/18 Nakagawa Edit End
            msg += "JupiterManagerを起動しますが、メニューボタン以外は使用できません。" + vbCrLf
            msg += "メニューボタンよりIJCADを起動し、会話処理を行ってください。"
            MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Function

    '---------------------------------------------------------------------------------------------------
    '機能
    '   帳票関連タブ内の、ボタン、テキスト、グループボックスを全て削除する。
    '---------------------------------------------------------------------------------------------------
    Private Sub deleteRebTabChildren(buttonNameHead As String)

        Dim gridRep As New Grid()
        getRepGrid(buttonNameHead, gridRep)

        Dim sIdx As Integer = 0
        If buttonNameHead Like "A*" Then
            sIdx = 2
        Else
            sIdx = 1
        End If

        If gridRep.Children.Count > sIdx Then
            Dim i As Integer = 0
            'For i = sIdx To gridRep.Children.Count - 1
            'gridRep.Children.RemoveAt(sIdx)
            For i = 0 To gridRep.Children.Count - 1
                gridRep.Children.RemoveAt(0)
            Next
        End If

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   帳票関連タブから配置用のグリッドを取得する。
    '---------------------------------------------------------------------------------------------------
    Private Sub getRepGrid(buttonNameHead As String, ByRef gridRep As Grid)

        Select Case buttonNameHead
            Case "A1"
                gridRep = Me.GridRepA1
            Case "A2"
                gridRep = Me.GridRepA2
            Case "A3"
                gridRep = Me.GridRepA3
            Case "A4"
                gridRep = Me.GridRepA4
            Case "B1"
                gridRep = Me.GridRepB1
            Case "B2"
                gridRep = Me.GridRepB2
            Case "B3"
                gridRep = Me.GridRepB3
        End Select

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   帳票関連タブにボタンを配置する。
    '---------------------------------------------------------------------------------------------------
    Private Sub createRepButton(buttonNameHead As String, disp As JManDispTab)

        Dim gridRep As New Grid()
        getRepGrid(buttonNameHead, gridRep)

        Dim ii As Integer = 0
        Dim jj As Integer = 0
        For ii = 0 To disp._buttonList.Count - 1
            Dim buttonRow As Integer = disp._buttonList(ii)._rowNo          '行数
            Dim buttonColumn As Integer = disp._buttonList(ii)._columnNo    '列数
            Dim buttonNum As Integer = disp._buttonList(ii)._buttonNum      'ボタン数
            Dim buttonWidth As Double = 75.0                                'ボタン幅
            Dim buttonColumnSpan As Integer = 1                             'ボタン表示列数
            Dim buttonMargin As New System.Windows.Thickness()              'ボタン隙間
            If buttonNum = 1 Then
                buttonWidth = 150.0
                buttonColumnSpan = 2
            Else
                buttonMargin.Top = 20.0
            End If
            Dim borderThickness As New System.Windows.Thickness()           'Borderの線厚
            borderThickness.Top = 1
            borderThickness.Bottom = 1
            borderThickness.Left = 1
            borderThickness.Right = 1

            For jj = 0 To disp._buttonList(ii)._detailList.Count - 1

                ' ボタンを作成する。
                Dim btnName As String = buttonNameHead + "_" + getAlphabet(buttonRow) + "_" + (buttonColumn + jj).ToString()
                Dim btnCaption As String = ""
                If buttonNum = 1 Then
                    If disp._buttonList(ii)._detailList(jj)._caption = "" Then
                        btnCaption = disp._buttonList(ii)._caption
                    Else
                        btnCaption = disp._buttonList(ii)._caption + vbCrLf + disp._buttonList(ii)._detailList(jj)._caption
                    End If
                Else
                    btnCaption = disp._buttonList(ii)._detailList(jj)._caption
                End If

                Dim btnNew As New System.Windows.Controls.Button()
                btnNew.Name = btnName
                btnNew.Height = 50.0
                btnNew.Width = buttonWidth
                btnNew.SetValue(Grid.RowProperty, 1 + buttonRow)
                btnNew.SetValue(Grid.ColumnProperty, 1 + buttonColumn + jj)
                btnNew.SetValue(Grid.ColumnSpanProperty, buttonColumnSpan)
                btnNew.Foreground = New SolidColorBrush(System.Windows.Media.Colors.White)
                Select Case disp._buttonList(ii)._detailList(jj)._type
                    Case 1
                        btnNew.Style = Me.ButtonBziSample1.Style
                        btnNew.Background = Me.ButtonBziSample1.Background
                    Case 2
                        btnNew.Style = Me.ButtonBziSample2.Style
                        btnNew.Background = Me.ButtonBziSample2.Background
                    Case 3
                        btnNew.Style = Me.ButtonBziSample3.Style
                        btnNew.Background = Me.ButtonBziSample3.Background
                    Case 4
                        btnNew.Style = Me.ButtonBziSample4.Style
                        btnNew.Background = Me.ButtonBziSample4.Background
                    Case 5
                        btnNew.Style = Me.ButtonBziSample5.Style
                        btnNew.Background = Me.ButtonBziSample5.Background
                    Case 6
                        btnNew.Style = Me.ButtonBziSample6.Style
                        btnNew.Background = Me.ButtonBziSample6.Background
                End Select
                btnNew.Content = btnCaption
                btnNew.FontSize = 11.0 * disp._buttonList(ii)._detailList(jj)._textHeigthRatio
                btnNew.Cursor = Me.ButtonBziData.Cursor
                btnNew.HorizontalAlignment = Windows.HorizontalAlignment.Center
                If buttonNum = 1 Then
                    btnNew.VerticalAlignment = Windows.VerticalAlignment.Center
                Else
                    btnNew.VerticalAlignment = Windows.VerticalAlignment.Top
                End If
                btnNew.Margin = buttonMargin

                'Gridへ追加
                gridRep.Children.Add(btnNew)

                'ボタンのクリックイベントの定義
                AddHandler btnNew.Click, AddressOf OnClick_BtnNew

                'JManDispButtonInfoListへ定義を追加
                Dim buttonInfo As New JManDispButtonInfo
                buttonInfo._type = disp._buttonList(ii)._detailList(jj)._type
                buttonInfo._name = btnName
                buttonInfo._fileName = disp._buttonList(ii)._detailList(jj)._fileName
                buttonInfo._argument = disp._buttonList(ii)._detailList(jj)._argument
                _buttonInfoListRep.append(buttonInfo)
            Next

            'Borderとラベルを作成する。
            If buttonNum <> 1 Then
                Dim borderNew As New Border
                borderNew.Background = New SolidColorBrush(System.Windows.Media.Colors.White)
                borderNew.HorizontalAlignment = Windows.HorizontalAlignment.Stretch
                borderNew.VerticalAlignment = Windows.VerticalAlignment.Top
                borderNew.Height = 20.0
                borderNew.SetValue(Grid.RowProperty, 1 + buttonRow)
                borderNew.SetValue(Grid.ColumnProperty, 1 + buttonColumn)
                borderNew.SetValue(Grid.ColumnSpanProperty, buttonNum)
                borderNew.BorderBrush = New SolidColorBrush(System.Windows.Media.Colors.Gainsboro)
                borderNew.BorderThickness = borderThickness

                'ラベルを作成する。
                Dim labelNew As New System.Windows.Controls.Label()
                labelNew.Content = disp._buttonList(ii)._caption
                labelNew.FontSize = 11.0
                labelNew.FontWeight = Me.LabelBziSet.FontWeight
                labelNew.VerticalAlignment = Windows.VerticalAlignment.Top
                labelNew.HorizontalAlignment = Windows.HorizontalAlignment.Center
                labelNew.Padding = Me.LabelBziSet.Padding
                labelNew.SetValue(Grid.RowProperty, 1 + buttonRow)
                labelNew.SetValue(Grid.ColumnProperty, 1 + buttonColumn)
                labelNew.SetValue(Grid.ColumnSpanProperty, buttonNum)
                labelNew.Height = 20.0

                'Gridへ追加
                gridRep.Children.Add(borderNew)
                gridRep.Children.Add(labelNew)

            End If
        Next

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   帳票関連タブにテキストを配置する。
    '---------------------------------------------------------------------------------------------------
    Private Sub createRepTextBlock(buttonNameHead As String, disp As JManDispTab)

        Dim gridRep As New Grid()
        getRepGrid(buttonNameHead, gridRep)

        Dim ii As Integer = 0
        For ii = 0 To disp._textList.Count - 1
            Dim textRow As Integer = disp._textList(ii)._rowNo          '行数
            Dim textColumn As Integer = disp._textList(ii)._columnNo    '列数
            Dim textNum As Integer = disp._textList(ii)._width          'テキスト列数
            Dim textMargin As New System.Windows.Thickness()            'ボタン隙間
            textMargin.Left = 10.0
            Dim textName As String = "T" + buttonNameHead + "_" + getAlphabet(textRow) + "_" + textColumn.ToString()

            ' TextBlockを作成する。
            Dim txtBlockNew As New System.Windows.Controls.TextBlock()
            txtBlockNew.Name = textName
            txtBlockNew.SetValue(Grid.RowProperty, 1 + textRow)
            txtBlockNew.SetValue(Grid.ColumnProperty, 1 + textColumn)
            txtBlockNew.SetValue(Grid.ColumnSpanProperty, textNum)
            txtBlockNew.Foreground = New SolidColorBrush(System.Windows.Media.Colors.Black)
            txtBlockNew.Text = disp._textList(ii)._caption
            txtBlockNew.FontSize = 11.0
            txtBlockNew.HorizontalAlignment = Windows.HorizontalAlignment.Left
            txtBlockNew.VerticalAlignment = Windows.VerticalAlignment.Center
            txtBlockNew.Padding = Me.LabelBziSet.Padding
            txtBlockNew.FontWeight = Me.LabelBziSet.FontWeight
            txtBlockNew.TextWrapping = Me.LabelBziSet.TextWrapping

            'Gridへ追加
            gridRep.Children.Add(txtBlockNew)

        Next

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   帳票関連タブにグループボックスを配置する。
    '---------------------------------------------------------------------------------------------------
    Private Sub createRepGroupBox(buttonNameHead As String, disp As JManDispTab)

        Dim gridRep As New Grid()
        getRepGrid(buttonNameHead, gridRep)

        Dim ii As Integer = 0
        For ii = 0 To disp._groupBoxList.Count - 1
            Dim groupRow As Integer = disp._groupBoxList(ii)._rowNo          '行数
            Dim groupColumn As Integer = disp._groupBoxList(ii)._columnNo    '列数
            Dim groupHeight As Integer = disp._groupBoxList(ii)._height      'グループボックス高さ
            Dim groupWidth As Integer = disp._groupBoxList(ii)._width        'グループボックス幅
            Dim groupName As String = "G" + buttonNameHead + "_" + getAlphabet(groupRow) + "_" + groupColumn.ToString()

            'Marginの設定
            Dim groupMargin As New System.Windows.Thickness()
            groupMargin.Top = 0.0
            groupMargin.Left = 0.0
            If groupRow <> 1 Then
                groupMargin.Top = 49.0
            End If
            If groupColumn <> 1 Then
                groupMargin.Left = 72.0
            End If

            Dim grpBoxNew As New System.Windows.Controls.GroupBox()
            grpBoxNew.Name = groupName
            grpBoxNew.Header = disp._groupBoxList(ii)._caption
            grpBoxNew.SetValue(Grid.RowProperty, groupRow)
            grpBoxNew.SetValue(Grid.RowSpanProperty, groupHeight + 2)
            grpBoxNew.SetValue(Grid.ColumnProperty, groupColumn)
            grpBoxNew.SetValue(Grid.ColumnSpanProperty, groupWidth + 2)
            grpBoxNew.VerticalAlignment = Windows.VerticalAlignment.Top
            grpBoxNew.HorizontalAlignment = Windows.HorizontalAlignment.Left
            grpBoxNew.Height = 74.0 * groupHeight + 30
            grpBoxNew.Width = 77.0 * groupWidth + 20
            grpBoxNew.Margin = groupMargin

            'Gridへ追加
            gridRep.Children.Add(grpBoxNew)

        Next
    End Sub


    '---------------------------------------------------------------------------------------------------
    '機能
    '   SKLタブのボタン・テキストの表示状態を初期化する
    '---------------------------------------------------------------------------------------------------
    Private Sub dispSklInit()

        Me.TabSkl.Header = " 骨組関連 "

        Me.ButtonSklReport1.Visibility = False
        Me.ButtonSklReport2.Visibility = False
        Me.ButtonSklReport3.Visibility = False
        Me.ButtonSklReport4.Visibility = False
        Me.ButtonSklReport5.Visibility = False
        Me.ButtonSklReport6.Visibility = False
        Me.ButtonSklReport7.Visibility = False

        Me.LabelSklReport1.Visibility = False
        Me.LabelSklReport2.Visibility = False
        Me.LabelSklReport3.Visibility = False
        Me.LabelSklReport4.Visibility = False
        Me.LabelSklReport5.Visibility = False
        Me.LabelSklReport6.Visibility = False
        Me.LabelSklReport7.Visibility = False

        Me.GroupBoxSkReport.Height = 415

        Me.LabelSklFileName.Content = ""
        Me.SklRichTextBox.Document.Blocks.Clear()
        Me.SklRichTextBox.IsEnabled = False
        '_isChangedOpenTextFile = False
        _isChangedOpenTextFileSkl = False

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   BZIタブのボタン・テキストの表示状態を初期化する
    '---------------------------------------------------------------------------------------------------
    Private Sub dispBziInit()

        Me.TabBzi.Header = " 3D部材作成 "

        Me.GroupBoxBziKaiwa.Visibility = False
        Me.ButtonBziTouei.Visibility = False
        Me.ButtonToueiTorikomi.Visibility = False
        Me.BorderFlgKegaki.Visibility = False
        Me.LabelBziFlgKegaki.Visibility = False

        Me.LabelBziFileName.Content = ""
        Me.BziRichTextBox.Document.Blocks.Clear()
        Me.BziRichTextBox.IsEnabled = False
        '_isChangedOpenTextFile = False
        _isChangedOpenTextFileBzi = False

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   SEISANタブのボタン・テキストの表示状態を初期化する
    '---------------------------------------------------------------------------------------------------
    Private Sub dispSeisanInit()

        Me.TabSeisan.Header = " 展開・生産 "
        Me.LabelSeisanFileName.Content = ""
        Me.SeisanRichTextBox.Document.Blocks.Clear()
        Me.SeisanRichTextBox.IsEnabled = False
        '_isChangedOpenTextFile = False
        _isChangedOpenTextFileSeisan = False

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   RepAタブのボタン・テキストの表示状態を初期化する
    '---------------------------------------------------------------------------------------------------
    Private Sub dispRepAInit()

        Me.TabRepA1.Header = " 帳票関連A1 "
        Me.LabelRepFileNameA1.Content = ""
        Me.RepRichTextBoxA1.Document.Blocks.Clear()
        Me.RepRichTextBoxA1.IsEnabled = False

        Me.TabRepA2.Header = " 帳票関連A2 "
        Me.LabelRepFileNameA2.Content = ""
        Me.RepRichTextBoxA2.Document.Blocks.Clear()
        Me.RepRichTextBoxA2.IsEnabled = False

        Me.TabRepA3.Header = " 帳票関連A3 "
        Me.LabelRepFileNameA3.Content = ""
        Me.RepRichTextBoxA3.Document.Blocks.Clear()
        Me.RepRichTextBoxA3.IsEnabled = False

        Me.TabRepA4.Header = " 帳票関連A4 "
        Me.LabelRepFileNameA4.Content = ""
        Me.RepRichTextBoxA4.Document.Blocks.Clear()
        Me.RepRichTextBoxA4.IsEnabled = False

        '_isChangedOpenTextFile = False
        _isChangedOpenTextFileRepA1 = False
        _isChangedOpenTextFileRepA2 = False
        _isChangedOpenTextFileRepA3 = False
        _isChangedOpenTextFileRepA4 = False

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   SKLタブのボタンの表示
    '---------------------------------------------------------------------------------------------------
    Private Sub dispSklButton(tab As JManDispTab)
        'Button1の表示・非表示・キャプションの変更
        If tab._buttonList.Count < 1 Then
            Me.ButtonSklReport1.Visibility = True
        Else
            Me.ButtonSklReport1.Visibility = False
            Me.ButtonSklReport1.Content = tab._buttonList(0)._detailList(0)._caption
            Me.ButtonSklReport1.FontSize = Me.ButtonSklReport1.FontSize * tab._buttonList(0)._detailList(0)._textHeigthRatio
            Me._buttonFile._sklButton1File = tab._buttonList(0)._detailList(0)._fileName
            Me._buttonFile._sklButton1Arg = tab._buttonList(0)._detailList(0)._argument
        End If
        'Button2の表示・非表示・キャプションの変更
        If tab._buttonList.Count < 2 Then
            Me.ButtonSklReport2.Visibility = True
        Else
            Me.ButtonSklReport2.Visibility = False
            Me.ButtonSklReport2.Content = tab._buttonList(1)._detailList(0)._caption
            Me.ButtonSklReport2.FontSize = Me.ButtonSklReport2.FontSize * tab._buttonList(1)._detailList(0)._textHeigthRatio
            Me._buttonFile._sklButton2File = tab._buttonList(1)._detailList(0)._fileName
            Me._buttonFile._sklButton2Arg = tab._buttonList(1)._detailList(0)._argument
        End If
        'Button3の表示・非表示・キャプションの変更
        If tab._buttonList.Count < 3 Then
            Me.ButtonSklReport3.Visibility = True
        Else
            Me.ButtonSklReport3.Visibility = False
            Me.ButtonSklReport3.Content = tab._buttonList(2)._detailList(0)._caption
            Me.ButtonSklReport3.FontSize = Me.ButtonSklReport3.FontSize * tab._buttonList(2)._detailList(0)._textHeigthRatio
            Me._buttonFile._sklButton3File = tab._buttonList(2)._detailList(0)._fileName
            Me._buttonFile._sklButton3Arg = tab._buttonList(2)._detailList(0)._argument
        End If
        'Button4の表示・非表示・キャプションの変更
        If tab._buttonList.Count < 4 Then
            Me.ButtonSklReport4.Visibility = True
        Else
            Me.ButtonSklReport4.Visibility = False
            Me.ButtonSklReport4.Content = tab._buttonList(3)._detailList(0)._caption
            Me.ButtonSklReport4.FontSize = Me.ButtonSklReport4.FontSize * tab._buttonList(3)._detailList(0)._textHeigthRatio
            Me._buttonFile._sklButton4File = tab._buttonList(3)._detailList(0)._fileName
            Me._buttonFile._sklButton4Arg = tab._buttonList(3)._detailList(0)._argument
        End If
        'Button5の表示・非表示・キャプションの変更
        If tab._buttonList.Count < 5 Then
            Me.ButtonSklReport5.Visibility = True
        Else
            Me.ButtonSklReport5.Visibility = False
            Me.ButtonSklReport5.Content = tab._buttonList(4)._detailList(0)._caption
            Me.ButtonSklReport5.FontSize = Me.ButtonSklReport5.FontSize * tab._buttonList(4)._detailList(0)._textHeigthRatio
            Me._buttonFile._sklButton5File = tab._buttonList(4)._detailList(0)._fileName
            Me._buttonFile._sklButton5Arg = tab._buttonList(4)._detailList(0)._argument
        End If
        'Button6の表示・非表示・キャプションの変更
        If tab._buttonList.Count < 6 Then
            Me.ButtonSklReport6.Visibility = True
        Else
            Me.ButtonSklReport6.Visibility = False
            Me.ButtonSklReport6.Content = tab._buttonList(5)._detailList(0)._caption
            Me.ButtonSklReport6.FontSize = Me.ButtonSklReport6.FontSize * tab._buttonList(5)._detailList(0)._textHeigthRatio
            Me._buttonFile._sklButton6File = tab._buttonList(5)._detailList(0)._fileName
            Me._buttonFile._sklButton6Arg = tab._buttonList(5)._detailList(0)._argument
        End If
        'Button7の表示・非表示・キャプションの変更
        If tab._buttonList.Count < 7 Then
            Me.ButtonSklReport7.Visibility = True
        Else
            Me.ButtonSklReport7.Visibility = False
            Me.ButtonSklReport7.Content = tab._buttonList(6)._detailList(0)._caption
            Me.ButtonSklReport7.FontSize = Me.ButtonSklReport7.FontSize * tab._buttonList(6)._detailList(0)._textHeigthRatio
            Me._buttonFile._sklButton7File = tab._buttonList(6)._detailList(0)._fileName
            Me._buttonFile._sklButton7Arg = tab._buttonList(6)._detailList(0)._argument
        End If
    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   SKLタブのテキストの表示
    '---------------------------------------------------------------------------------------------------
    Private Sub dispSklLabel(tab As JManDispTab)
        'Label1の表示・非表示・キャプションの変更
        If tab._textList.Count < 1 Then
            Me.LabelSklReport1.Visibility = True
        Else
            Me.LabelSklReport1.Visibility = False
            Me.LabelSklReport1.Text = tab._textList(0)._caption
        End If
        'Label2の表示・非表示・キャプションの変更
        If tab._textList.Count < 2 Then
            Me.LabelSklReport2.Visibility = True
        Else
            Me.LabelSklReport2.Visibility = False
            Me.LabelSklReport2.Text = tab._textList(1)._caption
        End If
        'Label3の表示・非表示・キャプションの変更
        If tab._textList.Count < 3 Then
            Me.LabelSklReport3.Visibility = True
        Else
            Me.LabelSklReport3.Visibility = False
            Me.LabelSklReport3.Text = tab._textList(2)._caption
        End If
        'Label4の表示・非表示・キャプションの変更
        If tab._textList.Count < 4 Then
            Me.LabelSklReport4.Visibility = True
        Else
            Me.LabelSklReport4.Visibility = False
            Me.LabelSklReport4.Text = tab._textList(3)._caption
        End If
        'Label5の表示・非表示・キャプションの変更
        If tab._textList.Count < 5 Then
            Me.LabelSklReport5.Visibility = True
        Else
            Me.LabelSklReport5.Visibility = False
            Me.LabelSklReport5.Text = tab._textList(4)._caption
        End If
        'Label6の表示・非表示・キャプションの変更
        If tab._textList.Count < 6 Then
            Me.LabelSklReport6.Visibility = True
        Else
            Me.LabelSklReport6.Visibility = False
            Me.LabelSklReport6.Text = tab._textList(5)._caption
        End If
        'Label7の表示・非表示・キャプションの変更
        If tab._textList.Count < 7 Then
            Me.LabelSklReport7.Visibility = True
        Else
            Me.LabelSklReport7.Visibility = False
            Me.LabelSklReport7.Text = tab._textList(6)._caption
        End If
    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   SKLタブのグループボックスのサイズ変更
    '---------------------------------------------------------------------------------------------------
    Private Sub dispSklGroupBox(tab As JManDispTab)

        Dim btnSize As Integer = tab._buttonList.Count
        Dim txtSize As Integer = tab._textList.Count
        Dim size As Integer = btnSize
        If btnSize < txtSize Then
            size = txtSize
        End If

        Dim groupHeight As Double = Me.GroupBoxSkReport.Height
        Me.GroupBoxSkReport.Height = groupHeight - (7 - size) * 55.0

    End Sub

    Private Sub kojiNewDialog()

        Dim kojiNew As New JManKojiNew()
        'kojiNew.OnInitialize(_language, _buttonColor, _backColor)
        kojiNew.OnInitialize(_language, _buttonColor, _buttonColor)
        kojiNew.ShowDialog()

        Dim status As Integer = 0
        If kojiNew._isOkCansel = 0 Then
            Me.TextBoxKogo.Text = kojiNew._kogo
            Me.TextBoxKojiName.Text = kojiNew._kojiName
            Me.TextBoxKojiFolder.Text = kojiNew._kojiFolder + "\"
            _kojiFolder = kojiNew._kojiFolder + "\"

            Me.setProperty()
            Me.TextBoxKozo.Text = _kozo

            ' JmanDisp_*.tbsを読み込み、タブの表示を変更する。
            status = Me.dispTab()
            _isNewJupiter = False
        End If

        If status <> 0 Or _isNewJupiter = True Then
            Me.Close()
        End If

    End Sub

    '---------------------------------------------------------------------------------------------------
    ' メニューボタンのイベント
    '---------------------------------------------------------------------------------------------------
    Private Sub OnClick_ButtonNew(sender As Object, e As RoutedEventArgs) Handles ButtonNew.Click

        Me.kojiNewDialog()

    End Sub

    Private Sub kojiOpenDialog(flg As Integer)
        'flg = 0・・・ボタンから起動
        'flg = 1・・・ライセンスチェックエラーの際に起動

        _isNewJupiter = False

        Dim kojiOpen As New JManKojiOpen()
        'kojiOpen.OnInitialize(_language, _buttonColor, _backColor)
        kojiOpen.OnInitialize(_language, _buttonColor, _buttonColor)
        kojiOpen.ShowDialog()

        If kojiOpen._isOkCansel = 0 Then
            'ライセンスチェック
            Dim status As Integer = Me.checkLicense(kojiOpen._kojiFolder)
            If status <> 0 Then
                Me._isOkLicense = False
                Return
            End If
            Me._isOkLicense = True

            If kojiOpen._kogo <> "" Then
                Me.TextBoxKogo.Text = kojiOpen._kogo
                Me.TextBoxKojiName.Text = kojiOpen._kojiName
                Me.TextBoxKojiFolder.Text = kojiOpen._kojiFolder + "\"
                _kojiFolder = kojiOpen._kojiFolder + "\"

                Me.setProperty()
                Me.TextBoxKozo.Text = _kozo
            End If

            ' JmanDisp_*.tbsを読み込み、タブの表示を変更する。
            status = Me.dispTab()
            If status <> 0 Then
                Me.Close()
            End If
        Else
            If flg = 1 Then
                Me.Close()
            End If
        End If

    End Sub

    Private Sub OnClick_ButtonOpen(sender As Object, e As RoutedEventArgs) Handles ButtonOpen.Click

        Me.kojiOpenDialog(0)

    End Sub

    Private Sub OnClick_ButtonKojiEdit(sender As Object, e As RoutedEventArgs) Handles ButtonKojiEdit.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        Dim kojiEdit As New JManKojiEdit()
        Dim status As Integer = kojiEdit.OnInitialize(Me.TextBoxKojiFolder.Text, Me._buttonColor, Me._buttonColor)
        If status <> 0 Then
            Return
        End If

        kojiEdit.ShowDialog()
        If kojiEdit._isOkCansel = 0 Then
            Me.TextBoxKojiName.Text = kojiEdit._kojiName
            Me.TextBoxKozo.Text = kojiEdit._kozo
            Me._kozo = kojiEdit._kozo
            Me._kozoNo = getKozoNo(kojiEdit._kozo)

            Me.setProperty()

        End If

    End Sub

    Private Sub OnClick_ButtonJconsole(sender As Object, e As RoutedEventArgs) Handles ButtonJconsole.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        Dim status As Integer = exeStartJupiter(Me.TextBoxKojiFolder.Text, _exeFileName._jconsoleExe, Me.TextBoxKojiFolder.Text)
        If status <> 0 Then
            Dim msg As String = "実行ファイル[" + _exeFileName._jconsoleExe
            msg += "]が存在しません。開発元に連絡してください。"
            MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    Private Sub OnClick_ButtonLog(sender As Object, e As RoutedEventArgs) Handles ButtonLog.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        Dim arg As String = "JupiterManager " + Me.TextBoxKojiFolder.Text

        Dim status As Integer = exeStartJupiter(Me.TextBoxKojiFolder.Text, _exeFileName._jlogViewExe, arg)
        If status <> 0 Then
            Dim msg As String = "実行ファイル[" + _exeFileName._jlogViewExe
            msg += "]が存在しません。開発元に連絡してください。"
            MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    Private Sub OnClick_ButtonStandard(sender As Object, e As RoutedEventArgs) Handles ButtonStandard.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        If _editorExe = "" Then
            MessageBox.Show("EDITORのパスが設定されていません。設定ボタンでアプリケーションのパスを設定してください。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        exeStart(Me.TextBoxKojiFolder.Text, _editorExe, Me._setingFileName._standardFileName, 1)

    End Sub

    Private Sub OnClick_ButtonWeight(sender As Object, e As RoutedEventArgs) Handles ButtonWeight.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        Dim status As Integer = exeStartJupiter(Me.TextBoxKojiFolder.Text, _exeFileName._jweightExe, Me.TextBoxKojiFolder.Text)
        If status <> 0 Then
            Dim msg As String = "実行ファイル[" + _exeFileName._jweightExe
            msg += "]が存在しません。開発元に連絡してください。"
            MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    Private Sub OnClick_ButtonPmf(sender As Object, e As RoutedEventArgs) Handles ButtonPmf.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        Dim status As Integer = exeStartJupiter(Me.TextBoxKojiFolder.Text, _exeFileName._jpmfdrwExe, Me.TextBoxKojiFolder.Text)
        If status <> 0 Then
            Dim msg As String = "実行ファイル[" + _exeFileName._jpmfdrwExe
            msg += "]が存在しません。開発元に連絡してください。"
            MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    Private Sub OnClick_ButtonCl(sender As Object, e As RoutedEventArgs) Handles ButtonCl.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        '引数を設定
        Dim arg As String = Me.TextBoxKojiFolder.Text + "*.CL"

        Dim status As Integer = exeStartJupiter(Me.TextBoxKojiFolder.Text, _exeFileName._jcloutExe, arg)
        If status <> 0 Then
            Dim msg As String = "実行ファイル[" + _exeFileName._jcloutExe
            msg += "]が存在しません。開発元に連絡してください。"
            MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    Private Sub OnClick_ButtonDxf(sender As Object, e As RoutedEventArgs) Handles ButtonDxf.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        '引数を設定
        Dim arg As String = "2 " + Me.TextBoxKojiFolder.Text

        Dim status As Integer = exeStartJupiter(Me.TextBoxKojiFolder.Text, _exeFileName._dxfPlotExe, arg)
        If status <> 0 Then
            Dim msg As String = "実行ファイル[" + _exeFileName._dxfPlotExe
            msg += "]が存在しません。開発元に連絡してください。"
            MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    Private Sub OnClick_ButtonIJCAD(sender As Object, e As RoutedEventArgs) Handles ButtonIJCAD.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        If _ijcadExe = "" Then
            MessageBox.Show("IJCADのパスが設定されていません。設定ボタンでアプリケーションのパスを設定してください。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        exeStart(Me.TextBoxKojiFolder.Text, _ijcadExe, "")

    End Sub

    Private Sub OnClick_ButtonEXCEL(sender As Object, e As RoutedEventArgs) Handles ButtonEXCEL.Click

        If _excelExe = "" Then
            MessageBox.Show("EXCELのパスが設定されていません。設定ボタンでアプリケーションのパスを設定してください。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        exeStart(Me.TextBoxKojiFolder.Text, _excelExe, "")

    End Sub

    Private Sub OnClick_ButtonACCESS(sender As Object, e As RoutedEventArgs) Handles ButtonACCESS.Click

        If _accessExe = "" Then
            MessageBox.Show("ACCESSのパスが設定されていません。設定ボタンでアプリケーションのパスを設定してください。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        exeStart(Me.TextBoxKojiFolder.Text, _accessExe, "")

    End Sub

    Private Sub OnClick_ButtonSetting(sender As Object, e As RoutedEventArgs) Handles ButtonSetting.Click
        Dim setting As New JManSetting()
        'setting.OnInitialize(_language, _buttonColor, _backColor)
        setting.OnInitialize(_language, _buttonColor, _buttonColor)
        setting.ShowDialog()
        If setting._isOkCansel = 0 Then
            Me._ijcadExe = setting._ijcadExeName
            Me._excelExe = setting._excelExeName
            Me._accessExe = setting._accessExeName
            Me._editorExe = setting._editorExeName
            Me._editorExe = setting._editorExeName
            Me._language = setting._language
            Me._buttonColor = setting._buttonColor
            Me._backColor = setting._buttonColor
            'ボタン色、背景色、アイコンを変更する
            Me.changeColor()
        End If

    End Sub

    '---------------------------------------------------------------------------------------------------
    ' リッチテキストボックス関連イベント
    '---------------------------------------------------------------------------------------------------
    Private Sub TextChanged_SklRichTextBox(sender As Object, e As TextChangedEventArgs) Handles SklRichTextBox.TextChanged
        'Me._isChangedOpenTextFile = True
        _isChangedOpenTextFileSkl = True
    End Sub

    Private Sub TextChanged_BziRichTextBox(sender As Object, e As TextChangedEventArgs) Handles BziRichTextBox.TextChanged
        'Me._isChangedOpenTextFile = True
        _isChangedOpenTextFileBzi = True
    End Sub

    Private Sub TextChanged_SeisanRichTextBox(sender As Object, e As TextChangedEventArgs) Handles SeisanRichTextBox.TextChanged
        'Me._isChangedOpenTextFile = True
        _isChangedOpenTextFileSeisan = True
    End Sub

    Private Sub TextChanged_RepRichTextBoxA1(sender As Object, e As TextChangedEventArgs) Handles RepRichTextBoxA1.TextChanged
        'Me._isChangedOpenTextFile = True
        _isChangedOpenTextFileRepA1 = True
    End Sub

    Private Sub TextChanged_RepRichTextBoxA2(sender As Object, e As TextChangedEventArgs) Handles RepRichTextBoxA2.TextChanged
        'Me._isChangedOpenTextFile = True
        _isChangedOpenTextFileRepA2 = True
    End Sub

    Private Sub TextChanged_RepRichTextBoxA3(sender As Object, e As TextChangedEventArgs) Handles RepRichTextBoxA3.TextChanged
        'Me._isChangedOpenTextFile = True
        _isChangedOpenTextFileRepA3 = True
    End Sub

    Private Sub TextChanged_RepRichTextBoxA4(sender As Object, e As TextChangedEventArgs) Handles RepRichTextBoxA4.TextChanged
        'Me._isChangedOpenTextFile = True
        _isChangedOpenTextFileRepA4 = True
    End Sub


    '---------------------------------------------------------------------------------------------------
    ' 骨組関連タブ ボタンイベント
    '---------------------------------------------------------------------------------------------------

    Private Function getIsChangedOpenTextFile(type As String) As Boolean

        getIsChangedOpenTextFile = False

        Select Case type
            Case "SKL"
                getIsChangedOpenTextFile = _isChangedOpenTextFileSkl
            Case "BZI"
                getIsChangedOpenTextFile = _isChangedOpenTextFileBzi
            Case "SEISAN"
                getIsChangedOpenTextFile = _isChangedOpenTextFileSeisan
            Case "RepA1"
                getIsChangedOpenTextFile = _isChangedOpenTextFileRepA1
            Case "RepA2"
                getIsChangedOpenTextFile = _isChangedOpenTextFileRepA2
            Case "RepA3"
                getIsChangedOpenTextFile = _isChangedOpenTextFileRepA3
            Case "RepA4"
                getIsChangedOpenTextFile = _isChangedOpenTextFileRepA4
        End Select

    End Function

    Private Sub setIsChangedOpenTextFile(type As String, isChangedOpenTextFile As Boolean)

        Select Case type
            Case "SKL"
                _isChangedOpenTextFileSkl = isChangedOpenTextFile
            Case "BZI"
                _isChangedOpenTextFileBzi = isChangedOpenTextFile
            Case "SEISAN"
                _isChangedOpenTextFileSeisan = isChangedOpenTextFile
            Case "RepA1"
                _isChangedOpenTextFileRepA1 = isChangedOpenTextFile
            Case "RepA2"
                _isChangedOpenTextFileRepA2 = isChangedOpenTextFile
            Case "RepA3"
                _isChangedOpenTextFileRepA3 = isChangedOpenTextFile
            Case "RepA4"
                _isChangedOpenTextFileRepA4 = isChangedOpenTextFile
        End Select

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    ' 　リッチテキストボックスが更新されている場合は、現在開いているテキストファイルを保存する。
    '引数
    '   tabName：(I)タブ名
    '   isNeedMsg：(I)メッセージフラグ
    '                 0:メッセージなし
    '                 1:メッセージタイプ1
    '                 2:メッセージタイプ2
    '---------------------------------------------------------------------------------------------------
    Private Sub saveOpenTextFile(tabName As String, Optional isNeedMsg As Integer = 1)

        Dim isChangedOpenTextFile As Boolean = Me.getIsChangedOpenTextFile(tabName)

        If Me._openTextFileName = "" Or isChangedOpenTextFile = False Then
            Return
        End If

        Dim message As String = ""

        '設定ファイルが存在するかチェック
        If System.IO.File.Exists(Me._openTextFileName) Then
            'メッセージを作成
            If isNeedMsg = 1 Then
                message = "データが更新されています。"
                message += Me._openTextFileName
                message += "を更新してよろしいですか？"
            ElseIf isNeedMsg = 2 Then
                message = Me._openTextFileName
                message += "を更新してよろしいですか？"
            End If
            If isNeedMsg <> 0 Then
                'ダイアログで更新してよいか問い合わせ
                Dim result As DialogResult = MessageBox.Show(message, "JupiterManager", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If result = Forms.DialogResult.No Then
                    Return
                End If
            End If
            System.IO.File.Delete(Me._openTextFileName)
        Else
            'メッセージを作成
            message = Me._openTextFileName
            message += "が工事フォルダに存在しないため、新規で作成します。"
            'ダイアログで更新してよいか問い合わせ
            MessageBox.Show(message, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        End If

        'リッチテキストボックスから全てのテキストを取得
        Dim richText As New System.Windows.Controls.RichTextBox()
        Select Case tabName
            Case "SKL"
                richText = Me.SklRichTextBox
            Case "BZI"
                richText = Me.BziRichTextBox
            Case "SEISAN"
                richText = Me.SeisanRichTextBox
            Case "RepA1"
                richText = Me.RepRichTextBoxA1
            Case "RepA2"
                richText = Me.RepRichTextBoxA2
            Case "RepA3"
                richText = Me.RepRichTextBoxA3
            Case "RepA4"
                richText = Me.RepRichTextBoxA4
            Case Else
                Return
        End Select

        '保存中のダイアログ表示
        Dim waitDlg As New JManWaitSaveDlg()
        waitDlg.OnInitialize(Me._buttonColor)
        waitDlg.Show()

        Dim writer As New StreamWriter(Me._openTextFileName, False, Encoding.GetEncoding("Shift_JIS"))
        If richText.Document.Blocks.Count < 1000 Then
            ' 行数が1000行以内であれば、全テキストを一括で書き込み
            richText.SelectAll()
            Dim textData As String = richText.Selection.Text
            writer.Write(textData)
        Else
            ' 行数が1000行を超える場合は、1行づつ書き込み(行数が多いとStringの制限を超えるため)
            Dim i As Integer = 0
            For i = 0 To richText.Document.Blocks.Count - 1
                Dim para As New Paragraph()
                para = richText.Document.Blocks(i)
                Dim textData As String = New TextRange(para.ContentStart, para.ContentEnd).Text
                writer.WriteLine(textData)
            Next
        End If
        writer.Close()
        waitDlg.Close()

        '_isChangedOpenTextFile = False
        Me.setIsChangedOpenTextFile(tabName, False)

        MessageBox.Show("ファイルを保存しました。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)

    End Sub

    Private Sub OnClick_ButtonSklPknd(sender As Object, e As RoutedEventArgs) Handles ButtonSklPknd.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        Me.openRichTextBox("SKL_PKND.TBL", "SKL")

    End Sub

    Private Sub OnClick_ButtonSklData(sender As Object, e As RoutedEventArgs) Handles ButtonSklData.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        Me.openRichTextBox("SklData.dat", "SKL", True)

    End Sub

    Private Sub OnClick_ButtonSkmake(sender As Object, e As RoutedEventArgs) Handles ButtonSkmake.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        '引数を設定
        Dim arg As String = Me.TextBoxKojiFolder.Text.TrimEnd("\") + " " + _kozo

        Dim status As Integer = exeStartJupiter(Me.TextBoxKojiFolder.Text, _exeFileName._jhoneExe, arg)
        If status <> 0 Then
            Dim msg As String = "実行ファイル[" + _exeFileName._jhoneExe
            msg += "]が存在しません。開発元に連絡してください。"
            MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    Private Sub OnClick_ButtonSklReport1(sender As Object, e As RoutedEventArgs) Handles ButtonSklReport1.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        '引数を設定
        Dim arg As String = Me.TextBoxKojiFolder.Text
        If Me._buttonFile._sklButton1Arg <> "" Then
            arg += " " + Me._buttonFile._sklButton1Arg
        End If

        Dim status As Integer = exeStartJupiter(Me.TextBoxKojiFolder.Text, Me._buttonFile._sklButton1File, arg)
        If status <> 0 Then
            Dim msg As String = "実行ファイル[" + Me._buttonFile._sklButton1File
            msg += "]が存在しません。開発元に連絡してください。"
            MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    Private Sub OnClick_ButtonSklReport2(sender As Object, e As RoutedEventArgs) Handles ButtonSklReport2.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        '引数を設定
        Dim arg As String = Me.TextBoxKojiFolder.Text
        If Me._buttonFile._sklButton2Arg <> "" Then
            arg += " " + Me._buttonFile._sklButton2Arg
        End If

        Dim status As Integer = exeStartJupiter(Me.TextBoxKojiFolder.Text, Me._buttonFile._sklButton2File, arg)
        If status <> 0 Then
            Dim msg As String = "実行ファイル[" + Me._buttonFile._sklButton2File
            msg += "]が存在しません。開発元に連絡してください。"
            MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    Private Sub OnClick_ButtonSklReport3(sender As Object, e As RoutedEventArgs) Handles ButtonSklReport3.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        '引数を設定
        Dim arg As String = Me.TextBoxKojiFolder.Text
        If Me._buttonFile._sklButton3Arg <> "" Then
            arg += " " + Me._buttonFile._sklButton3Arg
        End If

        Dim status As Integer = exeStartJupiter(Me.TextBoxKojiFolder.Text, Me._buttonFile._sklButton3File, arg)
        If status <> 0 Then
            Dim msg As String = "実行ファイル[" + Me._buttonFile._sklButton3File
            msg += "]が存在しません。開発元に連絡してください。"
            MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    Private Sub OnClick_ButtonSklReport4(sender As Object, e As RoutedEventArgs) Handles ButtonSklReport4.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        '引数を設定
        Dim arg As String = Me.TextBoxKojiFolder.Text
        If Me._buttonFile._sklButton4Arg <> "" Then
            arg += " " + Me._buttonFile._sklButton4Arg
        End If

        Dim status As Integer = exeStartJupiter(Me.TextBoxKojiFolder.Text, Me._buttonFile._sklButton4File, arg)
        If status <> 0 Then
            Dim msg As String = "実行ファイル[" + Me._buttonFile._sklButton4File
            msg += "]が存在しません。開発元に連絡してください。"
            MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    Private Sub OnClick_ButtonSklReport5(sender As Object, e As RoutedEventArgs) Handles ButtonSklReport5.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        '引数を設定
        Dim arg As String = Me.TextBoxKojiFolder.Text
        If Me._buttonFile._sklButton5Arg <> "" Then
            arg += " " + Me._buttonFile._sklButton5Arg
        End If

        Dim status As Integer = exeStartJupiter(Me.TextBoxKojiFolder.Text, Me._buttonFile._sklButton5File, arg)
        If status <> 0 Then
            Dim msg As String = "実行ファイル[" + Me._buttonFile._sklButton5File
            msg += "]が存在しません。開発元に連絡してください。"
            MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    Private Sub OnClick_ButtonSklReport6(sender As Object, e As RoutedEventArgs) Handles ButtonSklReport6.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        '引数を設定
        Dim arg As String = Me.TextBoxKojiFolder.Text
        If Me._buttonFile._sklButton6Arg <> "" Then
            arg += " " + Me._buttonFile._sklButton6Arg
        End If

        Dim status As Integer = exeStartJupiter(Me.TextBoxKojiFolder.Text, Me._buttonFile._sklButton6File, arg)
        If status <> 0 Then
            Dim msg As String = "実行ファイル[" + Me._buttonFile._sklButton6File
            msg += "]が存在しません。開発元に連絡してください。"
            MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    Private Sub OnClick_ButtonSklReport7(sender As Object, e As RoutedEventArgs) Handles ButtonSklReport7.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        '引数を設定
        Dim arg As String = Me.TextBoxKojiFolder.Text
        If Me._buttonFile._sklButton7Arg <> "" Then
            arg += " " + Me._buttonFile._sklButton7Arg
        End If

        Dim status As Integer = exeStartJupiter(Me.TextBoxKojiFolder.Text, Me._buttonFile._sklButton7File, arg)
        If status <> 0 Then
            Dim msg As String = "実行ファイル[" + Me._buttonFile._sklButton7File
            msg += "]が存在しません。開発元に連絡してください。"
            MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub
    Private Function getSettingFileFullPath(fileName As String) As String

        getSettingFileFullPath = ""

        If fileName.Contains("\") = True Then
            getSettingFileFullPath = fileName
            Exit Function
        End If

        Dim fileNameTmp As String = Me._kojiFolder.TrimEnd("\") + "\" + fileName

        Select Case fileNameTmp.ToUpper()
            Case Me._setingFileName._sklPkndFileName.ToUpper()
                'SKL_PKND.TBL
                getSettingFileFullPath = Me._setingFileName._sklPkndFileName
            Case Me._setingFileName._sklDataFileName.ToUpper()
                'SklData.dat
                getSettingFileFullPath = Me._setingFileName._sklDataFileName
            Case Me._setingFileName._anaBoltFileName.ToUpper()
                'ANABOLT.DAT
                getSettingFileFullPath = Me._setingFileName._anaBoltFileName
            Case Me._setingFileName._bziHeadFileName.ToUpper()
                'BziHead.dat
                getSettingFileFullPath = Me._setingFileName._bziHeadFileName
            Case Me._setingFileName._bziDataFileName.ToUpper()
                'BziData.dat
                getSettingFileFullPath = Me._setingFileName._bziDataFileName
            Case Me._setingFileName._kakFileName.ToUpper()
                'JUPITER.KAK
                getSettingFileFullPath = Me._setingFileName._kakFileName
            Case Me._setingFileName._syuFileName.ToUpper()
                'JUPITER.SYU
                getSettingFileFullPath = Me._setingFileName._syuFileName
            Case Me._setingFileName._lotFileName.ToUpper()
                'JUPITER.LOT
                getSettingFileFullPath = Me._setingFileName._lotFileName
            Case Me._setingFileName._psortFileName.ToUpper()
                'PSORT.MRK
                getSettingFileFullPath = Me._setingFileName._psortFileName
            Case Me._setingFileName._bsortFileName.ToUpper()
                'BSORT.MRK
                getSettingFileFullPath = Me._setingFileName._bsortFileName
            Case Me._setingFileName._pmkhFileName.ToUpper()
                'PmkhAdd.Dat
                getSettingFileFullPath = Me._setingFileName._pmkhFileName
            Case Me._setingFileName._bmkhFileName.ToUpper()
                'BmkhAdd.Dat
                getSettingFileFullPath = Me._setingFileName._bmkhFileName
            Case Else
                getSettingFileFullPath = fileNameTmp
        End Select

    End Function


    Private Sub openRichTextBox(fileName As String, type As String, Optional isNew As Boolean = False)

        If type = "SKL" Then
            If Me.LabelSklFileName.Content = fileName Then
                Return
            End If
        ElseIf type = "BZI" Then
            If Me.LabelBziFileName.Content = fileName Then
                Return
            End If
        ElseIf type = "SEISAN" Then
            If Me.LabelSeisanFileName.Content = fileName Then
                Return
            End If
        ElseIf type = "RepA1" Then
            If Me.LabelRepFileNameA1.Content = fileName Then
                Return
            End If
        ElseIf type = "RepA2" Then
            If Me.LabelRepFileNameA2.Content = fileName Then
                Return
            End If
        ElseIf type = "RepA3" Then
            If Me.LabelRepFileNameA3.Content = fileName Then
                Return
            End If
        ElseIf type = "RepA4" Then
            If Me.LabelRepFileNameA4.Content = fileName Then
                Return
            End If
        End If

        saveOpenTextFile(type)

        Dim richText As New System.Windows.Controls.RichTextBox()
        If type = "SKL" Then
            richText = Me.SklRichTextBox
        ElseIf type = "BZI" Then
            richText = Me.BziRichTextBox
        ElseIf type = "SEISAN" Then
            richText = Me.SeisanRichTextBox
        ElseIf type = "RepA1" Then
            richText = Me.RepRichTextBoxA1
        ElseIf type = "RepA2" Then
            richText = Me.RepRichTextBoxA2
        ElseIf type = "RepA3" Then
            richText = Me.RepRichTextBoxA3
        ElseIf type = "RepA4" Then
            richText = Me.RepRichTextBoxA4
        End If

        'リッチテキストボックスの中身をクリアする。
        richText.Document.Blocks.Clear()
        'If type = "SKL" Then
        '    Me.SklRichTextBox.Document.Blocks.Clear()
        'ElseIf type = "BZI" Then
        '    Me.BziRichTextBox.Document.Blocks.Clear()
        'ElseIf type = "SEISAN" Then
        '    Me.SeisanRichTextBox.Document.Blocks.Clear()
        'ElseIf type = "RepA1" Then
        '    Me.RepRichTextBoxA1.Document.Blocks.Clear()
        'ElseIf type = "RepA2" Then
        '    Me.RepRichTextBoxA2.Document.Blocks.Clear()
        'ElseIf type = "RepA3" Then
        '    Me.RepRichTextBoxA3.Document.Blocks.Clear()
        'ElseIf type = "RepA4" Then
        '    Me.RepRichTextBoxA4.Document.Blocks.Clear()
        'End If

        'ファイル名のフルパスを取得する。
        Dim settinFileName As String = Me.getSettingFileFullPath(fileName)

        'ファイルを読み込み。
        Dim texts As New ArrayList()
        Dim status As Integer = getAllTextListFromFile(settinFileName, texts)
        If status = 0 Then
            '行数が5000行を超える場合は、リッチテキストボックスへの保存に時間がかかる可能性が高いので、
            'エディタで表示するかを確認する。
            If texts.Count > 5000 Then
                Dim msg As String = fileName + "は行数が多いため、表示および保存に時間がかかる可能性があります。" + vbCrLf
                msg += "テキストボックスに表示してもよろしいですか？" + vbCrLf
                msg += "※[いいえ]を選択するとエディタでファイルを開きます。"
                Dim result As MessageBoxResult = MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2)
                If result = MessageBoxResult.No Then
                    exeStart(Me.TextBoxKojiFolder.Text, _editorExe, settinFileName, 1)
                    Return
                ElseIf result = MessageBoxResult.Cancel Then
                    Me.setIsChangedOpenTextFile(type, False)
                    Return
                End If
            End If


            'リッチテキストボックスに表示
            Dim waitDlg As New JManWaitDlg()
            waitDlg.OnInitialize(Me._buttonColor)
            waitDlg.Show()
            richText.IsEnabled = False
            Dim i As Integer = 0
            For i = 0 To texts.Count - 1
                Dim text As String = texts(i) + vbCrLf
                If i = 0 Then       'なぜか1行目のみ改行が認識されないための対応。
                    text += vbCrLf
                End If
                richText.AppendText(text)
            Next
            richText.IsEnabled = True
            If type = "SKL" Then
                Me.LabelSklFileName.Content = fileName
            ElseIf type = "BZI" Then
                Me.LabelBziFileName.Content = fileName
            ElseIf type = "SEISAN" Then
                Me.LabelSeisanFileName.Content = fileName
            ElseIf type = "RepA1" Then
                Me.LabelRepFileNameA1.Content = fileName
            ElseIf type = "RepA2" Then
                Me.LabelRepFileNameA2.Content = fileName
            ElseIf type = "RepA3" Then
                Me.LabelRepFileNameA3.Content = fileName
            ElseIf type = "RepA4" Then
                Me.LabelRepFileNameA4.Content = fileName
            End If
            waitDlg.Close()

            Me._openTextFileName = settinFileName
        Else
            If isNew = True Then
                Dim fileNameDisp As String = ""
                Dim result As MessageBoxResult = MessageBox.Show(fileName + "が存在しません。新規で作成してよろしいですか？", "JupiterManager", _
                                                                 MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk)
                If result = MessageBoxResult.OK Then
                    Me._openTextFileName = settinFileName
                    fileNameDisp = fileName
                End If
                richText.IsEnabled = True
                If type = "SKL" Then
                    Me.LabelSklFileName.Content = fileNameDisp
                ElseIf type = "BZI" Then
                    Me.LabelBziFileName.Content = fileNameDisp
                ElseIf type = "SEISAN" Then
                    Me.LabelSeisanFileName.Content = fileNameDisp
                ElseIf type = "RepA1" Then
                    Me.LabelRepFileNameA1.Content = fileNameDisp
                ElseIf type = "RepA2" Then
                    Me.LabelRepFileNameA2.Content = fileNameDisp
                ElseIf type = "RepA3" Then
                    Me.LabelRepFileNameA3.Content = fileNameDisp
                ElseIf type = "RepA4" Then
                    Me.LabelRepFileNameA4.Content = fileNameDisp
                End If
            Else
                Me._openTextFileName = ""
                MessageBox.Show(fileName + "が存在しないため、表示できません。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
        'Dim text As String = ""
        'Dim status As Integer = getAllTextFromFile(settinFileName, text)
        'If status = 0 Then
        '    'リッチテキストボックスに表示
        '    If type = "SKL" Then
        '        Dim waitDlg As New JManWaitDlg()
        '        waitDlg.OnInitialize(Me._buttonColor)
        '        Me.SklRichTextBox.IsEnabled = False
        '        waitDlg.Show()
        '        Me.SklRichTextBox.AppendText(text)
        '        Me.SklRichTextBox.IsEnabled = True
        '        waitDlg.Close()
        '        Me.LabelSklFileName.Content = fileName
        '    ElseIf type = "BZI" Then
        '        Dim waitDlg As New JManWaitDlg()
        '        waitDlg.OnInitialize(Me._buttonColor)
        '        Me.BziRichTextBox.IsEnabled = False
        '        waitDlg.Show()
        '        Me.BziRichTextBox.AppendText(text)
        '        Me.BziRichTextBox.IsEnabled = True
        '        waitDlg.Close()
        '        Me.LabelBziFileName.Content = fileName
        '    ElseIf type = "SEISAN" Then
        '        Me.SeisanRichTextBox.IsEnabled = False
        '        Me.SeisanRichTextBox.AppendText(text)
        '        Me.SeisanRichTextBox.IsEnabled = True
        '        Me.LabelSeisanFileName.Content = fileName
        '    ElseIf type = "RepA1" Then
        '        Me.RepRichTextBoxA1.IsEnabled = False
        '        Me.RepRichTextBoxA1.AppendText(text)
        '        Me.RepRichTextBoxA1.IsEnabled = True
        '        Me.LabelRepFileNameA1.Content = fileName
        '    ElseIf type = "RepA2" Then
        '        Me.RepRichTextBoxA2.IsEnabled = False
        '        Me.RepRichTextBoxA2.AppendText(text)
        '        Me.RepRichTextBoxA2.IsEnabled = True
        '        Me.LabelRepFileNameA2.Content = fileName
        '    ElseIf type = "RepA3" Then
        '        Me.RepRichTextBoxA3.IsEnabled = False
        '        Me.RepRichTextBoxA3.AppendText(text)
        '        Me.RepRichTextBoxA3.IsEnabled = True
        '        Me.LabelRepFileNameA3.Content = fileName
        '    ElseIf type = "RepA4" Then
        '        Me.RepRichTextBoxA4.IsEnabled = False
        '        Me.RepRichTextBoxA4.AppendText(text)
        '        Me.RepRichTextBoxA4.IsEnabled = True
        '        Me.LabelRepFileNameA4.Content = fileName
        '    End If

        '    Me._openTextFileName = settinFileName
        'Else
        '    If isNew = True Then
        '        Dim fileNameDisp As String = ""
        '        Dim result As MessageBoxResult = MessageBox.Show(fileName + "が存在しません。新規で作成してよろしいですか？", "JupiterManager", _
        '                                                         MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk)
        '        If result = MessageBoxResult.OK Then
        '            Me._openTextFileName = settinFileName
        '            fileNameDisp = fileName
        '        End If
        '        If type = "SKL" Then
        '            Me.LabelSklFileName.Content = fileNameDisp
        '            Me.SklRichTextBox.IsEnabled = True
        '        ElseIf type = "BZI" Then
        '            Me.LabelBziFileName.Content = fileNameDisp
        '            Me.BziRichTextBox.IsEnabled = True
        '        ElseIf type = "SEISAN" Then
        '            Me.LabelSeisanFileName.Content = fileNameDisp
        '            Me.SeisanRichTextBox.IsEnabled = True
        '        ElseIf type = "RepA1" Then
        '            Me.LabelRepFileNameA1.Content = fileNameDisp
        '            Me.RepRichTextBoxA1.IsEnabled = True
        '        ElseIf type = "RepA2" Then
        '            Me.LabelRepFileNameA2.Content = fileNameDisp
        '            Me.RepRichTextBoxA2.IsEnabled = True
        '        ElseIf type = "RepA3" Then
        '            Me.LabelRepFileNameA3.Content = fileNameDisp
        '            Me.RepRichTextBoxA3.IsEnabled = True
        '        ElseIf type = "RepA4" Then
        '            Me.LabelRepFileNameA4.Content = fileNameDisp
        '            Me.RepRichTextBoxA4.IsEnabled = True
        '        End If
        '    Else
        '        Me._openTextFileName = ""
        '        MessageBox.Show(fileName + "が存在しないため、表示できません。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    End If
        'End If

        'Me._isChangedOpenTextFile = False
        Me.setIsChangedOpenTextFile(type, False)

    End Sub

    Private Sub OnClick_ButtonAnaBolt(sender As Object, e As RoutedEventArgs) Handles ButtonAnaBolt.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        Me.openRichTextBox("ANABOLT.DAT", "BZI")

    End Sub

    Private Sub OnClick_ButtonBziHead(sender As Object, e As RoutedEventArgs) Handles ButtonBziHead.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        If Me._kozoNo = 6 Then
            MessageBox.Show("鋼製セグメントではBziHead.datの編集は必要ありません。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        Else
            Me.openRichTextBox("BziHead.dat", "BZI")
        End If

    End Sub

    Private Sub OnClick_ButtonBziData(sender As Object, e As RoutedEventArgs) Handles ButtonBziData.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        If Me._kozoNo = 6 Then
            Me.openRichTextBox("SegData.dat", "BZI", True)
        Else
            Me.openRichTextBox("BziData.dat", "BZI", True)
        End If

    End Sub

    ' Block.inf、ORIGINALBLOCK.INF、DUMMYBLOCK.INFを削除する。
    Private Function deleteBlockInfFile() As Integer
        If System.IO.File.Exists(Me._setingFileName._blockInfFileName) Then
            System.IO.File.Delete(Me._setingFileName._blockInfFileName)
        End If
        If System.IO.File.Exists(Me._setingFileName._orgBlockInfFileName) Then
            System.IO.File.Delete(Me._setingFileName._orgBlockInfFileName)
        End If
        If System.IO.File.Exists(Me._setingFileName._dmyBlockInfFileName) Then
            Dim msg As String = "以前展開処理を行ったときの削除ブロックを保存したファイルが存在します。"
            msg += vbCrLf
            msg += "ファイルを削除しますか？"
            msg += vbCrLf
            msg += "※ファイルを削除すると展開処理時に再度削除ブロックの定義が必要となります。"

            Dim result As DialogResult = MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If result = Forms.DialogResult.Yes Then
                System.IO.File.Delete(Me._setingFileName._dmyBlockInfFileName)
            ElseIf result = Forms.DialogResult.Cancel Then
                Return 1
            End If
        End If

        Return 0

    End Function

    Private Sub OnClick_ButtonBziBatch(sender As Object, e As RoutedEventArgs) Handles ButtonBziBatch.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        ' Block.inf、ORIGINALBLOCK.INF、DUMMYBLOCK.INFを削除する。
        Dim status As Integer = Me.deleteBlockInfFile()
        If status <> 0 Then
            Return
        End If

        'SCRファイルを作成
        Dim scrFileName As String = ""
        createBziBatchScr(Me._kozoNo, Me._kojiFolder, scrFileName)

        'Jconsoleでスクリプトを実行
        Call SCRIPT送信(scrFileName, "/L /A部材一括")

    End Sub

    Private Sub OnClick_ButtonKaiwaRireki(sender As Object, e As RoutedEventArgs) Handles ButtonKaiwaRireki.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        'Jmanage.iniからAccessのパスを取得する。
        Dim exeName As String = FunGetAccess()

        If exeName = "" Then
            MessageBox.Show("ACCESSのパスが設定されていません。設定ボタンでアプリケーションのパスを設定してください。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        'JlogWorkが存在するかチェック
        If System.IO.File.Exists(Me._setingFileName._jlogWorkFileName) Then
            exeStart(Me.TextBoxKojiFolder.Text, exeName, Me._setingFileName._jlogWorkFileName)
        Else
            MessageBox.Show("工事フォルダに履歴作成データベースが存在しません。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    Private Sub OnClick_ButtonKaiwaScr(sender As Object, e As RoutedEventArgs) Handles ButtonKaiwaScr.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        'JSCRBUID.EXEを起動
        Dim status As Integer = exeStartJupiter(Me.TextBoxKojiFolder.Text, _exeFileName._jscrBuildExe, Me.TextBoxKojiFolder.Text)
        If status <> 0 Then
            Dim msg As String = "実行ファイル[" + _exeFileName._jscrBuildExe
            msg += "]が存在しません。開発元に連絡してください。"
            MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    Private Sub OnClick_ButtonBziKaiwa(sender As Object, e As RoutedEventArgs) Handles ButtonBziKaiwa.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        Dim selectTsc As New JManSelectTsc()
        Dim status As Integer = selectTsc.OnInitialize(Me.TextBoxKojiFolder.Text, 0, Me._buttonColor, Me._buttonColor)
        If status <> 0 Then
            MessageBox.Show("実行可能なスクリプトファイルが存在しません。『スクリプト定義』でスクリプトファイルを作成してください。", _
                            "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        selectTsc.ShowDialog()
        If selectTsc._tscFileName = "" Then
            Return
        End If

        'SCRファイルを作成
        Dim scrFileName As String = ""
        createBziKaiwaScr(Me._kojiFolder, selectTsc._tscFileName, scrFileName)

        'Jconsoleでスクリプトを実行
        Call SCRIPT送信(scrFileName, "/L /A会話処理")

    End Sub

    Private Sub OnClick_ButtonBziKaiwaBatch(sender As Object, e As RoutedEventArgs) Handles ButtonBziKaiwaBatch.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        Dim selectTsc As New JManSelectTsc()
        Dim status As Integer = selectTsc.OnInitialize(Me.TextBoxKojiFolder.Text, 1, Me._buttonColor, Me._buttonColor)
        If status <> 0 Then
            MessageBox.Show("実行可能なスクリプトファイルが存在しません。『スクリプト定義』で部材一括処理と会話処理を合わせたスクリプトファイルを作成してください。", _
                            "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        selectTsc.ShowDialog()
        If selectTsc._tscFileName = "" Then
            Return
        End If

        ' Block.inf、ORIGINALBLOCK.INF、DUMMYBLOCK.INFを削除する。
        status = Me.deleteBlockInfFile()
        If status <> 0 Then
            Return
        End If

        Dim scrFileName As String = ""
        createBziBatchKaiwaScr(Me._kojiFolder, selectTsc._tscFileName, scrFileName)

        'Jconsoleでスクリプトを実行
        Call SCRIPT送信(scrFileName, "/L /A部材一括+会話処理")

    End Sub

    Private Sub OnClick_ButtonKanshoChk(sender As Object, e As RoutedEventArgs) Handles ButtonKanshoChk.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        'KanshoChk.exeを起動
        Dim status As Integer = exeStartJupiter(Me.TextBoxKojiFolder.Text, _exeFileName._kanshoChkExe, "")
        If status <> 0 Then
            Dim msg As String = "実行ファイル[" + _exeFileName._kanshoChkExe
            msg += "]が存在しません。開発元に連絡してください。"
            MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    Private Sub OnClick_ButtonKanchoChkView(sender As Object, e As RoutedEventArgs) Handles ButtonKanchoChkView.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        'KanshoChk.exeを起動
        Dim status As Integer = exeStartJupiter(Me.TextBoxKojiFolder.Text, _exeFileName._kanshoChkViewerExe, "")
        If status <> 0 Then
            Dim msg As String = "実行ファイル[" + _exeFileName._kanshoChkViewerExe
            msg += "]が存在しません。開発元に連絡してください。"
            MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    Private Sub OnClick_ButtonBziTouei(sender As Object, e As RoutedEventArgs) Handles ButtonBziTouei.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        '引数を設定
        Dim arg As String = Me.TextBoxKojiFolder.Text

        Dim status As Integer = exeStartJupiter(Me.TextBoxKojiFolder.Text, Me._exeFileName._buzaiToueiExe, arg)
        If status <> 0 Then
            Dim msg As String = "実行ファイル[" + _exeFileName._buzaiToueiExe
            msg += "]が存在しません。開発元に連絡してください。"
            MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    Private Sub OnClick_ButtonToueiTorikomi(sender As Object, e As RoutedEventArgs) Handles ButtonToueiTorikomi.Click
        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        '引数を設定
        Dim arg As String = Me.TextBoxKojiFolder.Text

        Dim status As Integer = exeStartJupiter(Me.TextBoxKojiFolder.Text, Me._exeFileName._toueiTorikomiExe, arg)
        If status <> 0 Then
            Dim msg As String = "実行ファイル[" + _exeFileName._toueiTorikomiExe
            msg += "]が存在しません。開発元に連絡してください。"
            MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    Private Sub OnClick_ButtonDelBlk(sender As Object, e As RoutedEventArgs) Handles ButtonDelBlk.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        Dim jmanBlk As New JManDelBlk
        '2016/11/01 Nakagawa Edit
        'jmanBlk.OnInitialize(Me.TextBoxKojiFolder.Text, Me._buttonColor, Me._buttonColor)
        jmanBlk.OnInitialize(Me.TextBoxKojiFolder.Text, Me._buttonColor, Me._buttonColor, Me.TextBoxKozo.Text)
        jmanBlk.ShowDialog()

    End Sub

    Private Sub OnClick_ButtonKak(sender As Object, e As RoutedEventArgs) Handles ButtonKak.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        Me.openRichTextBox("JUPITER.KAK", "SEISAN", True)

    End Sub

    Private Sub OnClick_ButtonSyu(sender As Object, e As RoutedEventArgs) Handles ButtonSyu.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        Me.openRichTextBox("JUPITER.SYU", "SEISAN", True)

    End Sub

    Private Sub OnClick_ButtonLot(sender As Object, e As RoutedEventArgs) Handles ButtonLot.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        Me.openRichTextBox("JUPITER.LOT", "SEISAN", True)

    End Sub

    Private Sub OnClick_ButtonPsort(sender As Object, e As RoutedEventArgs) Handles ButtonPsort.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        Me.openRichTextBox("PSORT.MRK", "SEISAN")

    End Sub

    Private Sub OnClick_ButtonBsort(sender As Object, e As RoutedEventArgs) Handles ButtonBsort.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        Me.openRichTextBox("BSORT.MRK", "SEISAN")

    End Sub

    Private Sub OnClick_ButtonPmkHeadSet(sender As Object, e As RoutedEventArgs) Handles ButtonPmkHeadSet.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        Me.openRichTextBox("PmkhAdd.Dat", "SEISAN", True)

    End Sub

    Private Sub OnClick_ButtonBmkHeadSet(sender As Object, e As RoutedEventArgs) Handles ButtonBmkHeadSet.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        Me.openRichTextBox("BmkhAdd.Dat", "SEISAN", True)

    End Sub

    Private Sub OnClick_ButtonSeisanSetOther(sender As Object, e As RoutedEventArgs) Handles ButtonSeisanSetOther.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        Dim seisanSub As New JManSeisanSub()
        seisanSub.OnInitialize(Me.TextBoxKojiFolder.Text, Me._environFolderName, Me._buttonColor, Me._buttonColor)
        seisanSub.ShowDialog()

    End Sub

    Private Sub OnClick_ButtonDevDeform(sender As Object, e As RoutedEventArgs) Handles ButtonDevDeform.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        'チャージコードの台数チェックを行う。
        ' 2016/11/01 Nakagawa Edit Start
        'Dim errMsg As String = checkBlockNumber(Me.TextBoxKogo.Text)
        Dim errMsg As String = checkBlockNumber(Me.TextBoxKogo.Text, Me.TextBoxKozo.Text)
        ' 2016/11/01 Nakagawa Edit End
        If errMsg <> "" Then
            MessageBox.Show(errMsg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        '作成中
        'If (System.Windows.Forms.Control.ModifierKeys And Keys.Shift) = Keys.Shift Then
        Dim dev2Sort As New JManDev2Sort()
        ' 2017/03/18 Nakagawa Edit Start
        'dev2Sort.OnInitialize(Me.TextBoxKojiFolder.Text, 0, Me._setingFileName, Me._buttonColor, Me._buttonColor)
        dev2Sort.OnInitialize(Me.TextBoxKojiFolder.Text, 0, Me._kozoNo, Me._setingFileName, Me._buttonColor, Me._buttonColor)
        ' 2017/03/18 Nakagawa Edit End
        dev2Sort.ShowDialog()
        If dev2Sort._isOkCanselError = 0 Then
            'Jconsoleでスクリプトを実行
            Call SCRIPT送信(dev2Sort._scrFileName, "/L /A部材展開～部材変形")
        End If
        'End If

    End Sub

    Private Sub OnClick_ButtonSeisanBatch(sender As Object, e As RoutedEventArgs) Handles ButtonSeisanBatch.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        'チャージコードの台数チェックを行う。
        ' 2016/11/01 Nakagawa Edit Start
        'Dim errMsg As String = checkBlockNumber(Me.TextBoxKogo.Text)
        Dim errMsg As String = checkBlockNumber(Me.TextBoxKogo.Text, Me.TextBoxKozo.Text)
        ' 2016/11/01 Nakagawa Edit End
        If errMsg <> "" Then
            MessageBox.Show(errMsg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        '作成中
        'If (System.Windows.Forms.Control.ModifierKeys And Keys.Shift) = Keys.Shift Then
        Dim dev2Sort As New JManDev2Sort()
        ' 2017/03/18 Nakagawa Edit Start
        'dev2Sort.OnInitialize(Me.TextBoxKojiFolder.Text, 2, Me._setingFileName, Me._buttonColor, Me._buttonColor)
        dev2Sort.OnInitialize(Me.TextBoxKojiFolder.Text, 2, Me._kozoNo, Me._setingFileName, Me._buttonColor, Me._buttonColor)
        ' 2017/03/18 Nakagawa Edit End
        dev2Sort.ShowDialog()
        If dev2Sort._isOkCanselError = 0 Then
            'Jconsoleでスクリプトを実行
            Call SCRIPT送信(dev2Sort._scrFileName, "/L /A部材展開～部材分類")
        End If
        'End If

    End Sub

    Private Sub OnClick_ButtonSort(sender As Object, e As RoutedEventArgs) Handles ButtonSort.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        'HENKEI.PMFがあるかチェック
        Dim henkeiPmfFileName As String = Me.TextBoxKojiFolder.Text.TrimEnd("\") + "\HENKEI.PMF"
        If System.IO.File.Exists(henkeiPmfFileName) = False Then
            MessageBox.Show("HENKEI.PMFがありません。変形処理が正常に終了しているか確認してください。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        'ZOKUSEI.BMFがあるかチェック
        Dim zokuseiBmfFileName As String = Me.TextBoxKojiFolder.Text.TrimEnd("\") + "\ZOKUSEI.BMF"
        If System.IO.File.Exists(zokuseiBmfFileName) = False Then
            MessageBox.Show("ZOKUSEI.BMFがありません。変形処理が正常に終了しているか確認してください。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' 2017/03/18 Nakagawa Delete Start
        'If (System.Windows.Forms.Control.ModifierKeys And Keys.Shift) = Keys.Shift Then
        ' 2017/03/18 Nakagawa Delete End
        Dim dev2Sort As New JManDev2Sort()
        ' 2017/03/18 Nakagawa Edit Start
        'dev2Sort.OnInitialize(Me.TextBoxKojiFolder.Text, 1, Me._setingFileName, Me._buttonColor, Me._buttonColor)
        dev2Sort.OnInitialize(Me.TextBoxKojiFolder.Text, 1, Me._kozoNo, Me._setingFileName, Me._buttonColor, Me._buttonColor)
        ' 2017/03/18 Nakagawa Edit End
        dev2Sort.ShowDialog()
        If dev2Sort._isOkCanselError = 0 Then
            'Jconsoleでスクリプトを実行
            Call SCRIPT送信(dev2Sort._scrFileName, "/L /A部材分類")
        End If
        ' 2017/03/18 Nakagawa Delete Start
        'Else
        ''スクリプトファイルを作成
        'Dim scrFileName As String = ""
        'createSortScr(Me._kojiFolder, scrFileName)

        ''Jconsoleでスクリプトを実行
        'Call SCRIPT送信(scrFileName, "/L /A部材分類")
        'End If
        ' 2017/03/18 Nakagawa Delete End

    End Sub

    Private Sub OnClick_ButtonPmkHead(sender As Object, e As RoutedEventArgs) Handles ButtonPmkHead.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        '引数を設定
        Dim arg As String = Me.TextBoxKojiFolder.Text

        Dim status As Integer = exeStartJupiter(Me.TextBoxKojiFolder.Text, Me._exeFileName._jPmarkExe, arg)
        If status <> 0 Then
            Dim msg As String = "実行ファイル[" + Me._exeFileName._jPmarkExe
            msg += "]が存在しません。開発元に連絡してください。"
            MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    Private Sub OnClick_ButtonBmkHead(sender As Object, e As RoutedEventArgs) Handles ButtonBmkHead.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        '引数を設定
        Dim arg As String = Me.TextBoxKojiFolder.Text

        Dim status As Integer = exeStartJupiter(Me.TextBoxKojiFolder.Text, Me._exeFileName._jBmarkExe, arg)
        If status <> 0 Then
            Dim msg As String = "実行ファイル[" + Me._exeFileName._jBmarkExe
            msg += "]が存在しません。開発元に連絡してください。"
            MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    '---------------------- 帳票関連のボタンイベント ----------------------
    Private Sub OnClick_BtnNew(ByVal sender As Object, ByVal e As RoutedEventArgs)

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        '_buttonInfoListRepからボタン名に一致するbuttonInfoを取得する。
        Dim button As System.Windows.Controls.Button = sender
        Dim buttonName As String = button.Name
        Dim buttonInfo As New JManDispButtonInfo()
        Dim status As Integer = _buttonInfoListRep.find(buttonName, buttonInfo)
        If status <> 0 Then
            MessageBox.Show("JmanDispファイルが正しく読み込めていないため、実行できません。開発元に連絡してください。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim arg As String = ""
        If buttonInfo._argument = "" Then
            arg = Me.TextBoxKojiFolder.Text
        Else
            arg = Me.TextBoxKojiFolder.Text + " " + buttonInfo._argument
        End If

        If buttonInfo._type = 3 Or buttonInfo._type = 4 Then
            'ボタン名から表示するリッチテキストボックスを決定する。
            Dim repTextBoxName As String = ""
            If buttonName Like "A1*" Then
                repTextBoxName = "RepA1"
            ElseIf buttonName Like "A2*" Then
                repTextBoxName = "RepA2"
            ElseIf buttonName Like "A3*" Then
                repTextBoxName = "RepA3"
            ElseIf buttonName Like "A4*" Then
                repTextBoxName = "RepA4"
            ElseIf buttonName Like "B1*" Then
                repTextBoxName = "RepB1"
            ElseIf buttonName Like "B2*" Then
                repTextBoxName = "RepB2"
            ElseIf buttonName Like "B3*" Then
                repTextBoxName = "RepB3"
            End If

            Dim buttonFileName As String = buttonInfo._fileName
            If buttonFileName.ToUpper() Like "*.CNT" Then
                Dim cntIn As New JManCntIn()
                cntIn.OnInitialize(Me.TextBoxKojiFolder.Text, buttonInfo._fileName, buttonInfo._argument, Me._buttonColor, Me._buttonColor)
                cntIn.ShowDialog()
                If cntIn._dataFileName <> "" Then
                    Me.openRichTextBox(cntIn._dataFileName, repTextBoxName, True)
                End If
            Else
                'リッチテキストボックスにファイルの内容を表示する。
                Me.openRichTextBox(buttonInfo._fileName, repTextBoxName, True)
            End If
        Else
            status = exeStartJupiter(Me.TextBoxKojiFolder.Text, buttonInfo._fileName, arg)
            If status <> 0 Then
                Dim msg As String = "実行ファイル[" + buttonInfo._fileName
                msg += "]が存在しません。開発元に連絡してください。"
                MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If

    End Sub

    Private Sub OnClick_ButtonSklSave(sender As Object, e As RoutedEventArgs) Handles ButtonSklSave.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        'リッチテキストボックスが空の場合はファイルを開かない。
        If Me.isOpenTextFile("SKL") = False Then
            MessageBox.Show("設定ファイルが表示されていないので、ファイルを保存することができません。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        saveOpenTextFile("SKL", 2)

    End Sub

    Private Sub OnClick_ButtonBziSave(sender As Object, e As RoutedEventArgs) Handles ButtonBziSave.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        'リッチテキストボックスが空の場合はファイルを開かない。
        If Me.isOpenTextFile("BZI") = False Then
            MessageBox.Show("設定ファイルが表示されていないので、ファイルを保存することができません。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        saveOpenTextFile("BZI", 2)

    End Sub

    Private Sub OnClick_ButtonSeisanSave(sender As Object, e As RoutedEventArgs) Handles ButtonSeisanSave.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        'リッチテキストボックスが空の場合はファイルを開かない。
        If Me.isOpenTextFile("SEISAN") = False Then
            MessageBox.Show("設定ファイルが表示されていないので、ファイルを保存することができません。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        saveOpenTextFile("SEISAN", 2)

    End Sub

    Private Sub OnClick_ButtonRepA1Save(sender As Object, e As RoutedEventArgs) Handles ButtonRepA1Save.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        'リッチテキストボックスが空の場合はファイルを開かない。
        If Me.isOpenTextFile("RepA1") = False Then
            MessageBox.Show("設定ファイルが表示されていないので、ファイルを保存することができません。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        saveOpenTextFile("RepA1", 2)

    End Sub

    Private Sub OnClick_ButtonRepA2Save(sender As Object, e As RoutedEventArgs) Handles ButtonRepA2Save.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        'リッチテキストボックスが空の場合はファイルを開かない。
        If Me.isOpenTextFile("RepA2") = False Then
            MessageBox.Show("設定ファイルが表示されていないので、ファイルを保存することができません。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        saveOpenTextFile("RepA2", 2)

    End Sub

    Private Sub OnClick_ButtonRepA3Save(sender As Object, e As RoutedEventArgs) Handles ButtonRepA3Save.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        'リッチテキストボックスが空の場合はファイルを開かない。
        If Me.isOpenTextFile("RepA3") = False Then
            MessageBox.Show("設定ファイルが表示されていないので、ファイルを保存することができません。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        saveOpenTextFile("RepA3", 2)

    End Sub

    Private Sub OnClick_ButtonRepA4Save(sender As Object, e As RoutedEventArgs) Handles ButtonRepA4Save.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        'リッチテキストボックスが空の場合はファイルを開かない。
        If Me.isOpenTextFile("RepA4") = False Then
            MessageBox.Show("設定ファイルが表示されていないので、ファイルを保存することができません。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        saveOpenTextFile("RepA4", 2)

    End Sub

    Private Function isOpenTextFile(type As String) As Boolean

        isOpenTextFile = True

        Select Case type
            Case "SKL"
                If Me.LabelSklFileName.Content = "" Then
                    isOpenTextFile = False
                End If
            Case "BZI"
                If Me.LabelBziFileName.Content = "" Then
                    isOpenTextFile = False
                End If
            Case "SEISAN"
                If Me.LabelSeisanFileName.Content = "" Then
                    isOpenTextFile = False
                End If
            Case "RepA1"
                If Me.LabelRepFileNameA1.Content = "" Then
                    isOpenTextFile = False
                End If
            Case "RepA2"
                If Me.LabelRepFileNameA2.Content = "" Then
                    isOpenTextFile = False
                End If
            Case "RepA3"
                If Me.LabelRepFileNameA3.Content = "" Then
                    isOpenTextFile = False
                End If
            Case "RepA4"
                If Me.LabelRepFileNameA4.Content = "" Then
                    isOpenTextFile = False
                End If
        End Select

    End Function

    Private Sub openTextDataForEditor(type As String)

        'リッチテキストボックスが空の場合はファイルを開かない。
        If Me.isOpenTextFile(type) = False Then
            MessageBox.Show("設定ファイルが表示されていないので、ファイルを開くことができません。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim isChangedOpenTextFile As Boolean = Me.getIsChangedOpenTextFile(type)

        'If Me._isChangedOpenTextFile = True Then
        If isChangedOpenTextFile = True Then
            Dim message As String = "テキストボックス内のデータが更新されています。" + vbCrLf + "更新を保存してからファイルを開きますか？"
            Dim result As DialogResult = MessageBox.Show(message, "JupiterManager", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
            If result = Forms.DialogResult.Yes Then
                saveOpenTextFile(type, 0)
            ElseIf result = Forms.DialogResult.Cancel Then
                Return
            End If
        End If

        If _editorExe = "" Then
            MessageBox.Show("EDITORのパスが設定されていません。設定ボタンでアプリケーションのパスを設定してください。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim dateBefore As DateTime
        If System.IO.File.Exists(Me._openTextFileName) = True Then
            dateBefore = System.IO.File.GetLastWriteTime(Me._openTextFileName)
        End If
        Dim status As Integer = editorStart(Me._kojiFolder, Me._openTextFileName, 1)

        If status = 1 Then
            MessageBox.Show("EDITORのパスが設定されていません。設定ボタンでアプリケーションのパスを設定してください。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        Dim dateAfter As DateTime = System.IO.File.GetLastWriteTime(Me._openTextFileName)

        If dateBefore <> dateAfter Then
            Me.openRichTextBox(Me._openTextFileName, type)
        End If

    End Sub

    Private Sub OnClick_ButtonSklEdit(sender As Object, e As RoutedEventArgs) Handles ButtonSklEdit.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        Me.openTextDataForEditor("SKL")

    End Sub

    Private Sub OnClick_ButtonBziEdit(sender As Object, e As RoutedEventArgs) Handles ButtonBziEdit.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        Me.openTextDataForEditor("BZI")

    End Sub

    Private Sub OnClick_ButtonSeisanEdit(sender As Object, e As RoutedEventArgs) Handles ButtonSeisanEdit.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        Me.openTextDataForEditor("SEISAN")

    End Sub

    Private Sub OnClick_ButtonRepA1Edit(sender As Object, e As RoutedEventArgs) Handles ButtonRepA1Edit.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        Me.openTextDataForEditor("RepA1")

    End Sub

    Private Sub OnClick_ButtonRepA2Edit(sender As Object, e As RoutedEventArgs) Handles ButtonRepA2Edit.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        Me.openTextDataForEditor("RepA2")

    End Sub

    Private Sub OnClick_ButtonRepA3Edit(sender As Object, e As RoutedEventArgs) Handles ButtonRepA3Edit.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        Me.openTextDataForEditor("RepA3")

    End Sub

    Private Sub OnClick_ButtonRepA4Edit(sender As Object, e As RoutedEventArgs) Handles ButtonRepA4Edit.Click

        '工事フォルダが設定されているかチェック
        If checkOpenedKojiFolder() <> 0 Then
            Return
        End If

        Me.openTextDataForEditor("RepA4")

    End Sub

    Private Sub Closed_MainWindow(sender As Object, e As EventArgs) Handles MyBase.Closed

        Dim sysFile As String = FunGetSystemFile()
        writeSystemTxtClose(sysFile)

        If Me._isOkLicense = True Then
            If Me._isUseBatch = 1 Then
                ネットワークライセンスの返却(251, 0)
            End If
            If Me._isUseKaiwa = 1 Then
                ネットワークライセンスの返却(253, 0)
            End If
        End If

    End Sub


End Class


