Imports System.Windows.Forms
Imports System.IO
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Globalization

Public Class JCBMainWindow
    Private Declare Function InitializeLicense_JPT Lib "JptPrtct.dll" () As Integer

    '---------------------------------------------------------------------------------------------------
    ' JupiterかCATSかの判定
    ' 0：Jupiter
    ' 1：CATS
    '---------------------------------------------------------------------------------------------------
    Public _flagJC As Integer = 0

    '---------------------------------------------------------------------------------------------------
    ' 工事情報
    '---------------------------------------------------------------------------------------------------
    '工事フォルダ
    Private _kojiFolder As String = ""
    '工号
    Private _kojiKogo As String = ""
    '工事名
    Private _kojiName As String = ""
    '設定ファイル名
    Private _settingFileName As New JManSettingFile
    '橋種
    Private _kozo As String = ""
    Private _kozoNo As Integer = -1

    '---------------------------------------------------------------------------------------------------
    ' サブウィンドウの設定
    '---------------------------------------------------------------------------------------------------
    'MDIボタンセット
    Private _setMdiButtons As New JCBSetMDIButtons
    '実行ログ
    Private _executionLog As New JCBExecutionLog
    'ファイルエクスプローラ
    Private _fileExplore As New JCBFileExplore3
    'サブウィンドで開かれているプログラムを表示
    Public _showProgram As JCBShowSubWindowProgram
    '開閉フラグ
    Public _flagShowProgram As Integer = 0

    '---------------------------------------------------------------------------------------------------
    ' 画面設定
    '---------------------------------------------------------------------------------------------------
    'サブウィンドウの開閉フラグ
    Public _subWinFlag As Integer = 0
    '展開前の画面の縦のサイズ
    Public _windowHeight As Integer = 0
    '実行するexe設定
    Private _exeButtonSet As New JCBSetExeLoadFile

    '---------------------------------------------------------------------------------------------------
    ' Jupiter環境
    '---------------------------------------------------------------------------------------------------
    'Environのフォルダ名
    Private _environFolderName As String = ""
    'IJCADのexeファイル名
    Private _ijcadExe As String = ""
    'Excelのexeファイル名
    Private _excelExe As String = ""
    'Accessのexeファイル名
    Private _accessExe As String = ""
    'Editorのexeファイル名
    Private _editorExe As String = ""

    '---------------------------------------------------------------------------------------------------
    ' CATS環境
    '---------------------------------------------------------------------------------------------------
    'Environのフォルダ名
    Private _environFolderName_CATS As String = ""

    '---------------------------------------------------------------------------------------------------
    ' Jupiter処理用
    '---------------------------------------------------------------------------------------------------
    '現在開いているテキストファイル名
    Private _openTextFileName As String = ""
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

    '---------------------------------------------------------------------------------------------------
    ' Load時の処理
    '---------------------------------------------------------------------------------------------------
    Private Sub OnInit_MainWindow(sender As Object, e As EventArgs) Handles MyBase.Load
        'コマンドライン引数を取得する。
        'ここでJupiterかCATSか判定する
        If Environment.GetCommandLineArgs.Length > 1 Then
            Me._argment = Environment.GetCommandLineArgs(1)
        End If

        If Me._argment = "Jupiter" Then
            _flagJC = 0
        ElseIf Me._argment = "CATS" Then
            _flagJC = 1
        End If

        ' プロパティセット    
        Me.SetProperty()

        If _flagJC = 0 Then
            If Me._isNewJupiter = True Then
                Me.KojiNewDialog()
            Else
                ' 前回開いていた工事を開く。
                Me.OpenInitKoji()

                'ライセンスチェック
                Dim status As Integer = Me.checkLicense(Me._kojiFolder)
                If status = 1 Then
                    Me.kojiOpenDialog(1)
                    Return
                ElseIf status = 2 Then
                    Me.Close()
                End If

                Me._isOkLicense = True
            End If
        Else
            Me.OpenInitKoji()
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
        Dim folderName As String = SubStringEnd(kojiFolder.TrimEnd("\"), "\")

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
                MessageBox.Show(msg, "CastarJupiter・CATSバッチシステム", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                MessageBox.Show(msg, "CastarJupiter・CATSバッチシステム", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                    MessageBox.Show(msg, "CastarJupiter・CATSバッチシステム", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    checkLicense = 2
                    Exit Function
                Else
                    If オプション使用の取得(ProductSystemNo253) Then
                        '会話処理が使用可能な場合は、System.txtに工事フォルダを書き込む。
                        WriteSystemTxt(SysFile, kojiFolder.TrimEnd("\"))
                        Me._isUseKaiwa = 0
                    Else
                        '会話処理が使用不可場合は、System.txtに空白を書き込む。
                        WriteSystemTxtClose(SysFile)
                        Me._isUseKaiwa = 2
                    End If

                    '2017/03/18 Nakagawa Add Start
                    'チャージコードがFREE-JUPITERの場合
                    If typbrg = 0 Then
                        '工事別ﾌﾟﾛﾌｧｲﾙから橋梁形式を読み込む。
                        Pfile = kojiFolder.TrimEnd("\") + "\PROFILE.INI"
                        ReadProfileIni(Pfile, syugeta, chargeCode, recno)
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
                            MessageBox.Show(msg, "CastarJupiter・CATSバッチシステム", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            checkLicense = 1
                        End If
                        Exit Function
                    End If
                    '工事別ﾌﾟﾛﾌｧｲﾙから橋梁形式を読み込む。
                    Pfile = kojiFolder.TrimEnd("\") + "\PROFILE.INI"
                    ReadProfileIni(Pfile, syugeta, chargeCode, recno)
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
                        MessageBox.Show(msg, "CastarJupiter・CATSバッチシステム", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                    ReadProfileIni(Pfile, syugeta, chargeCode, recno)

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
                            MessageBox.Show(msg, "CastarJupiter・CATSバッチシステム", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            checkLicense = 2
                            Exit Function
                        End If
                    End If
                End If

                Me._isUseBatch = 3
                Me._isUseKaiwa = 3

                If Me._argment = "KAIWA" Or Me._argment = "" Then  '会話
                    If ネットワークライセンスの取得(ProductSystemNo253, 0) = True Then
                        WriteSystemTxt(SysFile, kojiFolder.TrimEnd("\"))
                        Me._isUseKaiwa = 1
                    Else
                        WriteSystemTxtClose(SysFile)
                        Me._isUseKaiwa = 3
                        If Me._argment = "KAIWA" Then
                            msg = "会話処理のライセンスが取得できませんでした。" + vbCrLf
                            msg += "会話処理が使用可能か、または最大使用数を超えていないかを確認してください。" + vbCrLf
                            MessageBox.Show(msg, "CastarJupiter・CATSバッチシステム", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                            msg += "CastarJupiter・CATSバッチシステムを起動しますが、一括処理は使用できません。"
                        Else
                            msg = "The license of batch processing was not able to be acquired." + vbCrLf
                            msg += "Please check whether batch processing can be used or it is not over the number of the maximum use." + vbCrLf
                            msg += "Batch processing cannot be used although Jupiter is performed."
                        End If
                        MessageBox.Show(msg, "CastarJupiter・CATSバッチシステム", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Me._isUseBatch = 3
                        checkLicense = 2
                        Exit Function
                    End If
                End If

            End If
        End If

        subSetActiveData(folderName, _flagJC)
        Return 0

    End Function

    '---------------------------------------------------------------------------------------------------
    '機能
    '   工事フォルダを検索する。
    '引数
    '   無し
    '戻り値
    '   無し
    '---------------------------------------------------------------------------------------------------
    Private Function SetKojiFolder() As Integer

        If _kojiFolder = "" Then
            _kojiFolder = FunGetKoji(_flagJC)
            If _kojiFolder = "" Then
                'Jmanage.iniからACTIVE_DATAが見つからない場合は、工事フォルダ内の最初のフォルダを工事フォルダとする。
                Dim newFolder As String = FunGetNewKoji2(_flagJC)
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
    '   プロパティセット
    '引数
    '   無し
    '戻り値
    '   無し
    '---------------------------------------------------------------------------------------------------
    Private Sub SetProperty()

        If _flagJC = 0 Then
            'Jupiterの環境変数からEnvironフォルダーのディレクトリを取得
            _environFolderName = FunGetEnviron(_flagJC)

            '工事情報
            Me.SetKojiFolder()
            If Me._kojiFolder = "" Then
                _isNewJupiter = True
                Return
            End If
            Dim profileFileName As String = _kojiFolder + "\profile.ini"
            Dim strTmp As String = ""
            Dim intTmp As Integer = 0
            '工事フォルダのprofile.iniから構造形式を取得
            _kozoNo = ReadProfileIni(profileFileName, _kozo, strTmp, intTmp)

            '関連アプリケーションパス
            'JMANAGE.INIに設定されているものを取得
            _ijcadExe = FunGetAcad()
            _excelExe = FunGetExcel()
            _accessExe = FunGetAccess()
            Dim editorExeTmp As String = FunGetAppNo(0)
            fun文字列抽出(editorExeTmp, "[", "]", _editorExe)

            ' 実行するexeのボタンの設定と配置。
            Me._exeButtonSet.LoadFile("ButtonSet_J.csv")
        Else
            _environFolderName_CATS = FunGetEnviron(_flagJC)

            '工事情報
            Me.SetKojiFolder()
            Dim profileFileName As String = _kojiFolder + "profile.ini"
            Dim strTmp As String = ""
            Dim intTmp As Integer = 0
            '工事フォルダのprofile.iniから構造形式を取得
            _kozoNo = ReadProfileIni(profileFileName, _kozo, strTmp, intTmp)

            ' 実行するexeのボタンの設定と配置。
            Me._exeButtonSet.LoadFile("ButtonSet_C.csv")
        End If

        Me.SetExeButton()

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   前回開いていた工事を開く。
    '引数
    '   無し
    '戻り値
    '   無し
    '---------------------------------------------------------------------------------------------------
    Private Sub OpenInitKoji()

        If _flagJC = 0 Then
            Me._settingFileName.OnInitialize(_kozoNo, _kojiFolder, _environFolderName)
        Else
            Me._settingFileName.OnInitialize(_kozoNo, _kojiFolder, _environFolderName_CATS)
        End If

        If System.IO.File.Exists(Me._settingFileName._constInfFileName) = True Then
            Dim constInf As New StreamReader(Me._settingFileName._constInfFileName, Encoding.GetEncoding("Shift_JIS"))
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
    '   工事が開かれているかチェック。
    '引数
    '   無し
    '戻り値
    '   0：開かれている
    '   1：開かれていない
    '---------------------------------------------------------------------------------------------------
    Private Function CheckOpenedKojiFolder() As Integer

        CheckOpenedKojiFolder = 0

        If Me.TextBoxKojiFolder.Text = "" Then
            MessageBox.Show(CType(My.Resources.ResourceManager.GetObject("CheckKojiFolder_msg"), String), CType(My.Resources.ResourceManager.GetObject("FormTitle"), String), MessageBoxButtons.OK, MessageBoxIcon.Error)
            CheckOpenedKojiFolder = 1
        End If

    End Function

    '---------------------------------------------------------------------------------------------------
    ' Exe実行ボタン等を配置する
    '---------------------------------------------------------------------------------------------------
    Private Sub SetExeButton()
        Dim num As Integer = _exeButtonSet._numButton
        Dim buttonInf As ArrayList = _exeButtonSet._buttonInfList
        Dim toolTip As New ToolTip With {
            .ShowAlways = False
        }

        For i As Integer = 0 To num - 1
            Dim info As JCBExeButtonInfo = CType(buttonInf(i)(0), JCBExeButtonInfo)
            Dim pointY As Integer = 325 + i * 80

            Dim button As New Button With {
                .Text = info._caption,
                .BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer)),
                .Name = (i + 1).ToString,
                .Cursor = System.Windows.Forms.Cursors.Hand,
                .Size = New System.Drawing.Size(160, 50),
                .Margin = New System.Windows.Forms.Padding(0),
                .ForeColor = System.Drawing.Color.White,
                .Location = New System.Drawing.Point(15, pointY),
                .UseVisualStyleBackColor = False
            }
            button.FlatAppearance.BorderSize = 0
            button.FlatStyle = System.Windows.Forms.FlatStyle.Flat

            toolTip.SetToolTip(button, info._caption)
            Panel1.Controls.Add(button)
            SetResultPanel(pointY, i + 1)

            'イベントハンドラとして機能するようにする
            AddHandler button.Click, AddressOf ExeButtonClick

            '矢印を追加
            If i <> num - 1 Then
                SetArrowImage(pointY, i + 1)
            End If

            '1番目のボタン以外無効とする
            If i <> 0 Then
                button.Enabled = False
            End If

        Next

        'ボタンの数が5個以上の時、ボタンの数で画面の大きさを変える
        If num > 4 Then
            Me.Height = 385 + num * 80
            _windowHeight = Me.Height
        End If

    End Sub

    '---------------------------------------------------------------------------------------------------
    ' 機能    ：  実行状況パネルを追加
    ' 引数    ：  配置Y座標、ボタン番号
    '---------------------------------------------------------------------------------------------------
    Private Sub SetResultPanel(pointY As Integer, i As Integer)
        Dim panel As New Panel With {
            .BackColor = System.Drawing.Color.White,
            .Location = New System.Drawing.Point(175, pointY),
            .Margin = New System.Windows.Forms.Padding(0),
            .Name = "exePanel" + i.ToString,
            .Size = New System.Drawing.Size(140, 50)
        }

        Dim label As New Label With {
            .AutoSize = True,
            .Font = New System.Drawing.Font("MS UI Gothic", 9),
            .Location = New System.Drawing.Point(32, 20),
            .Name = "exeLabel" + i.ToString,
            .Size = New System.Drawing.Size(63, 14),
            .Text = CType(My.Resources.ResourceManager.GetObject("Unexecuted_text"), String)
        }

        Dim pictureBox As New PictureBox With {
            .BackColor = System.Drawing.Color.White,
            .Location = New System.Drawing.Point(8, 16),
            .Margin = New System.Windows.Forms.Padding(0),
            .Name = "exePictureBox" + i.ToString,
            .Size = New System.Drawing.Size(20, 20),
            .TabStop = False,
            .Image = CType(My.Resources.ResourceManager.GetObject("_06_noExecute"), Bitmap)
        }

        panel.Controls.Add(label)
        panel.Controls.Add(pictureBox)
        Panel1.Controls.Add(panel)

    End Sub

    ' ↓画像を追加--------------------------------------------------------------------------------------
    Private Sub SetArrowImage(pointY As Integer, i As Integer)
        Dim pictureBoxY As New PictureBox With {
            .BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(251, Byte), Integer), CType(CType(248, Byte), Integer)),
            .BackgroundImage = CType(My.Resources.ResourceManager.GetObject("Y02_LOW"), Bitmap),
            .BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom,
            .Location = New System.Drawing.Point(55, pointY + 55),
            .Margin = New System.Windows.Forms.Padding(0),
            .Name = "exePictureBoxY" + i.ToString,
            .Size = New System.Drawing.Size(90, 20)
        }

        Panel1.Controls.Add(pictureBoxY)

    End Sub

    '---------------------------------------------------------------------------------------------------
    ' 機能    ：  実行状況パネルの内容を変更
    ' 引数    ：  ボタン番号,実行状況(0:未実行,1:正常終了,2:正常終了(警告あり),3:異常終了,4:実行中)
    '---------------------------------------------------------------------------------------------------
    Private Sub ChangeResultPanel(buttonNo As Integer, state As Integer)

        'lavelをさがす。子コントロールも検索する。
        Dim lavelName As String = "exeLabel" + buttonNo.ToString
        Dim cs1 As Control() = Me.Controls.Find(lavelName, True)

        'imageをさがす。子コントロールも検索する。
        Dim imageName As String = "exePictureBox" + buttonNo.ToString
        Dim cs2 As Control() = Me.Controls.Find(imageName, True)

        If cs1.Length > 0 And cs2.Length > 0 Then
            If state = 0 Then
                CType(cs1(0), Label).Text = CType(My.Resources.ResourceManager.GetObject("Unexecuted_text"), String)
                CType(cs2(0), PictureBox).Image = CType(My.Resources.ResourceManager.GetObject("_06_noExecute"), Bitmap)
            ElseIf state = 1 Then
                CType(cs1(0), Label).Text = CType(My.Resources.ResourceManager.GetObject("SuccessfulCompletion_text"), String)
                CType(cs2(0), PictureBox).Image = CType(My.Resources.ResourceManager.GetObject("_04_normal"), Bitmap)
            ElseIf state = 2 Then
                CType(cs1(0), Label).Text = CType(My.Resources.ResourceManager.GetObject("SuccessfulCompletion_Warning_text"), String)
                CType(cs2(0), PictureBox).Image = CType(My.Resources.ResourceManager.GetObject("_07_warning2"), Bitmap)
            ElseIf state = 3 Then
                CType(cs1(0), Label).Text = CType(My.Resources.ResourceManager.GetObject("AbnormalTermination_text"), String)
                CType(cs2(0), PictureBox).Image = CType(My.Resources.ResourceManager.GetObject("_05_abnormal"), Bitmap)
            ElseIf state = 4 Then
                CType(cs1(0), Label).Text = CType(My.Resources.ResourceManager.GetObject("Running_text"), String)
                CType(cs2(0), PictureBox).Image = CType(My.Resources.ResourceManager.GetObject("_08_running"), Bitmap)
            End If

        End If


    End Sub

    ' exeボタンと実行状況パネルの内容をリセット---------------------------------------------------------
    Private Sub ResetExeButton()

        Dim numButton As Integer = _exeButtonSet._numButton

        For i As Integer = 1 To numButton
            'buttonをさがす。子コントロールも検索する。
            Dim buttonName As String = i.ToString
            Dim cs1 As Control() = Me.Controls.Find(buttonName, True)

            'panelをさがす。子コントロールも検索する。
            Dim panelName As String = "exePanel" + i.ToString
            Dim cs2 As Control() = Me.Controls.Find(panelName, True)

            Panel1.Controls.Remove(cs1(0))
            Panel1.Controls.Remove(cs2(0))
        Next
    End Sub

    '---------------------------------------------------------------------------------------------------
    ' メニューボタンのイベント
    '---------------------------------------------------------------------------------------------------

    '新規工事-----------------------------------------------------------------------------------------
    Private Sub KojiNewDialog()

        If _flagJC = 0 Then
            Dim kojiNew As New JCBNewConstruction()
            kojiNew.Initialized()
            kojiNew.ShowDialog()

            Dim status As Integer = 0
            If kojiNew._isOkCansel = 0 Then
                Me.TextBoxKogo.Text = kojiNew._kogo
                Me.TextBoxKojiName.Text = kojiNew._kojiName
                Me.TextBoxKojiFolder.Text = kojiNew._kojiFolder + "\"
                _kojiFolder = kojiNew._kojiFolder + "\"
                _kojiKogo = kojiNew._kogo
                _kojiName = kojiNew._kojiName

                Me.ResetExeButton()
                Me.SetProperty()
                Me.TextBoxKozo.Text = _kozo
            End If

            If status <> 0 Or _isNewJupiter = True Then
                Me.Close()
            End If

        Else
            Dim kojiNew As New JCBNewConstructionCATS()
            kojiNew.Initialized()
            kojiNew.ShowDialog()

            If kojiNew._isOkCansel = 0 Then
                Me.TextBoxKogo.Text = kojiNew._kogo
                Me.TextBoxKojiName.Text = kojiNew._kojiName
                Me.TextBoxKojiFolder.Text = kojiNew._kojiFolder + "\"
                _kojiFolder = kojiNew._kojiFolder + "\"
                _kojiKogo = kojiNew._kogo
                _kojiName = kojiNew._kojiName

                Me.ResetExeButton()
                Me.SetProperty()
                Me.TextBoxKozo.Text = _kozo
            End If

        End If


    End Sub

    Private Sub ButtonKojiNew_Click(sender As Object, e As EventArgs) Handles ButtonKojiNew.Click
        Me.KojiNewDialog()
    End Sub

    '工事を開く--------------------------------------------------------------------------------
    Private Sub kojiOpenDialog(flg As Integer)
        'flg = 0・・・ボタンから起動
        'flg = 1・・・ライセンスチェックエラーの際に起動

        _isNewJupiter = False

        Dim kojiOpen As New JCBOpenConstruction
        kojiOpen.OnInitialize(_flagJC)
        kojiOpen.ShowDialog()

        If kojiOpen._isOkCansel = 0 Then
            If _flagJC = 0 Then
                'ライセンスチェック
                Dim status As Integer = Me.checkLicense(kojiOpen._kojiFolder)
                If status <> 0 Then
                    Me._isOkLicense = False
                    Return
                End If
                Me._isOkLicense = True
            End If

            If kojiOpen._kogo <> "" Then
                Me.TextBoxKogo.Text = kojiOpen._kogo
                Me.TextBoxKojiName.Text = kojiOpen._kojiName
                Me.TextBoxKojiFolder.Text = kojiOpen._kojiFolder + "\"
                Me.TextBoxKozo.Text = _kozo
                _kojiFolder = kojiOpen._kojiFolder + "\"
                _kojiKogo = kojiOpen._kogo
                _kojiName = kojiOpen._kojiName

                Me.ResetExeButton()
                Me.SetProperty()
            End If

        Else
            If flg = 1 Then
                Me.Close()
            End If
        End If

    End Sub

    Private Sub ButtonKojiOpen_Click(sender As Object, e As EventArgs) Handles ButtonKojiOpen.Click
        Me.kojiOpenDialog(0)

    End Sub

    '工事情報編集----------------------------------------------------------------------------------
    Private Sub ButtonKojiEdit_Click(sender As Object, e As EventArgs) Handles ButtonKojiEdit.Click
        '工事フォルダが設定されているかチェック
        If CheckOpenedKojiFolder() <> 0 Then
            Return
        End If

        If _flagJC = 0 Then
            Dim form As New JCBEditConstruction
            Dim status As Integer = form.OnInitialize(Me._kojiFolder)
            If status <> 0 Then
                Return
            End If

            form.ShowDialog()
            If form._isOkCansel = 0 Then
                Me.TextBoxKojiName.Text = form._kojiName
                Me.TextBoxKozo.Text = form._kozo
                Me._kozo = form._kozo
                Me._kozoNo = GetKozoNo(form._kozo)
                Me.SetProperty()

            End If
        Else
            Dim form As New JCBEditConstructionCATS
            Dim status As Integer = form.OnInitialize(Me._kojiFolder)
            If status <> 0 Then
                Return
            End If

            form.ShowDialog()
            If form._isOkCansel = 0 Then
                Me.TextBoxKojiName.Text = form._kojiName
                Me.TextBoxKozo.Text = form._kozo
                Me._kozo = form._kozo
                Me._kozoNo = GetKozoNo(form._kozo)
                Me.SetProperty()

            End If
        End If


    End Sub

    '設定-----------------------------------------------------------------------------------------
    Private Sub ButtonSetting_Click(sender As Object, e As EventArgs) Handles ButtonSetting.Click
        Dim form As New JCBSetting
        form.ShowDialog()

        If form._isOkCansel = 0 Then
            Me._ijcadExe = form._ijcadExeName
            Me._excelExe = form._excelExeName
            Me._accessExe = form._accessExeName
            Me._editorExe = form._editorExeName
        End If

    End Sub

    '---------------------------------------------------------------------------------------------------
    ' EXE実行ボタンのイベント
    '---------------------------------------------------------------------------------------------------

    'exe全て実行(連続実行)---------------------------------------------------------------------------------
    Private Sub AllExeButton_Click(sender As Object, e As EventArgs) Handles allExeButton.Click
        Dim buttonNo As Integer = _exeButtonSet._numButton

        For i As Integer = 1 To buttonNo
            Dim button As String = i.ToString
            Dim cs As Control() = Me.Controls.Find(button, True)
            If cs.Length > 0 Then
                CType(cs(0), Button).PerformClick()
            End If

        Next

    End Sub

    'exe個別実行------------------------------------------------------------------------------------------------
    Private Sub ExeButtonClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim buttonNo As Integer = Integer.Parse(sender.name)
        Dim buttonInf As ArrayList = _exeButtonSet._buttonInfList
        Dim exeList As ArrayList = buttonInf(buttonNo - 1)
        Dim errorFlag As Integer = 0
        Dim kojiFolder As String = Me.TextBoxKojiFolder.Text.TrimEnd("\")

        '工事フォルダが設定されているかチェック
        If CheckOpenedKojiFolder() <> 0 Then
            Return
        End If

        '実行状況のパネルを「実行中」に変更する
        Me.ChangeResultPanel(buttonNo, 4)

        For i As Integer = 0 To exeList.Count - 1
            'EXEごとのインスタンス(JCBSetExeButtonInfo)を生成
            Dim exeInfo As JCBExeButtonInfo = exeList(i)
            Dim exeName As String = exeInfo._exeName
            Dim argment As String = ""
            Dim argCount As Integer = exeInfo._argList.Count

            For j As Integer = 0 To argCount - 1
                argment = argment + " " + ReplaceString(exeInfo._argList(j))
            Next

            Dim status As Integer = ExeStartJupiter(kojiFolder, exeName, argment, Me)

            '実行ログが表示されているとき
            If SearchProgram(Me.PanelSubWindowMain.Handle, CType(My.Resources.ResourceManager.GetObject("ExecutionLog_text"), String)) = 1 Then
                If status = 1 Then
                    Dim msg As String = CType(My.Resources.ResourceManager.GetObject("ExecutionFile_text"), String) + "[" + exeName + "]"
                    msg += CType(My.Resources.ResourceManager.GetObject("DoesNotExist_text"), String)
                    _executionLog.RichTextBoxLog_AddText(msg)
                    errorFlag = 1
                Else
                    Dim logResult As Integer = _executionLog.RichTextBoxLog_AddTextFile(kojiFolder + "\" + exeInfo._logFileName, 1)
                    If logResult = 1 Then
                        '異常終了
                        If exeInfo._errorBehavior = 1 Then
                            Dim msg As String = CType(My.Resources.ResourceManager.GetObject("ExecutionFile_text"), String) + "[" + exeName + "]"
                            msg += CType(My.Resources.ResourceManager.GetObject("ErminatesAbnormally_text"), String)
                            _executionLog.RichTextBoxLog_AddText(msg)
                            errorFlag = 2
                            Exit For
                        Else
                            errorFlag = 1
                        End If
                    End If
                End If
            Else
                'DesignToJupiterがないためEXEがなくても警告とする
                If status = 1 Then
                    errorFlag = 1
                Else
                    Dim logResult As Integer = _executionLog.RichTextBoxLog_AddTextFile(kojiFolder + "\" + exeInfo._logFileName, 0)
                    If logResult = 1 Then
                        '異常終了
                        If exeInfo._errorBehavior = 1 Then
                            errorFlag = 2
                            Exit For
                        Else
                            errorFlag = 1
                        End If
                    End If
                End If
            End If

        Next

        '実行状況のパネルを実行結果に基づいて変更する
        If errorFlag = 2 Then
            '異常終了
            Me.ChangeResultPanel(buttonNo, 3)
        ElseIf errorFlag = 1 Then
            '正常終了（警告あり）
            Me.ChangeResultPanel(buttonNo, 2)
            '次のボタンを有効にする
            Dim nextButton As String = (buttonNo + 1).ToString
            Dim cs As Control() = Me.Controls.Find(nextButton, True)
            If cs.Length > 0 Then
                CType(cs(0), Button).Enabled = True
            End If
        Else
            '正常終了
            Me.ChangeResultPanel(buttonNo, 1)
            '次のボタンを有効にする
            Dim nextButton As String = (buttonNo + 1).ToString
            Dim cs As Control() = Me.Controls.Find(nextButton, True)
            If cs.Length > 0 Then
                CType(cs(0), Button).Enabled = True
            End If
        End If

    End Sub

    ' 文字列の置き換え(工事フォルダ、工号、工事名)
    Private Function ReplaceString(str As String) As String
        Dim res1 As Integer = str.IndexOf("[KOJI]")
        Dim res2 As Integer = str.IndexOf("[KOGO]")
        Dim res3 As Integer = str.IndexOf("[KNAME]")

        If res1 >= 0 Then
            Return str.Replace("[KOJI]", _kojiFolder)
        ElseIf res2 >= 0 Then
            Return str.Replace("[KOGO]", _kojiKogo)
        ElseIf res3 >= 0 Then
            Return str.Replace("[KNAME]", _kojiName)
        Else
            Return str
        End If

    End Function

    '---------------------------------------------------------------------------------------------------
    ' サブウィンドウ
    '---------------------------------------------------------------------------------------------------
    Private Sub SubWindowButton_Click(sender As Object, e As EventArgs) Handles subWindowButton.Click

        '工事フォルダが設定されているかチェック
        If CheckOpenedKojiFolder() <> 0 Then
            Return
        End If

        If _subWinFlag = 0 Then
            '開く
            Me.Width = 1200
            Me.SetMdiButtons()
            Me.Controls.Add(Me.PanelSubWindowToolBar)
            Me.Controls.Add(Me.PanelSubWindowMain)

            Me.SetShowProgramListButton()


            Dim count As Integer = _setMdiButtons.Size
            For i As Integer = 0 To count - 1
                Dim inf As JCBMDIButtonInfo = _setMdiButtons._MdiButtonList(i)
                If inf._startingOperation = "Y" Then
                    Dim buttonName As String = inf._programFunction
                    Dim cs As Control() = Me.Controls.Find(buttonName, True)
                    If cs.Length > 0 Then
                        CType(cs(0), Button).PerformClick()
                    End If
                End If
            Next

            'ユーザーがサイズを変更できるようにする
            Me.FormBorderStyle = FormBorderStyle.Sizable
            'フォームを最大化できるようにする
            Me.MaximizeBox = True
            sender.Image = CType(My.Resources.ResourceManager.GetObject("Y03_left_black2"), Bitmap)

            _subWinFlag = 1

        ElseIf _subWinFlag = 1 Then
            '閉じる
            _setMdiButtons.ClearList()
            Me.WindowState = FormWindowState.Normal
            Me.Width = 420
            Me.Height = _windowHeight
            Me.Controls.Remove(PanelSubWindowToolBar)
            Me.Controls.Remove(PanelSubWindowMain)
            Me.PanelSubWindowToolBar.Controls.Clear()
            Me.PanelSubWindowMain.Controls.Clear()
            'フォームが最大化されないようにする
            Me.MaximizeBox = False
            'サイズを変更できないようにする
            Me.FormBorderStyle = FormBorderStyle.FixedSingle
            sender.Image = CType(My.Resources.ResourceManager.GetObject("Y04_right_black2"), Bitmap)

            _subWinFlag = 0
        End If

    End Sub

    ' サブウィンドウのツールバーのクリックイベント-------------------------------------------------------
    Private Sub PanelSubWindowToolBar_Click(sender As Object, e As EventArgs) Handles PanelSubWindowToolBar.Click
        If _flagShowProgram = 1 Then
            _showProgram.Close()
            _flagShowProgram = 0
        End If
        Dim cs As Control()
        Dim buttonName As String = "ShowProgramListButton"
        cs = Me.Controls.Find(buttonName, True)
        If cs.Length > 0 Then
            CType(cs(0), Button).BackColor = System.Drawing.Color.White
        End If
    End Sub

    ' サブウィンドウのメインウィンドウのクリックイベント-----------------------------------------------
    Private Sub PanelSubWindowMain_Click(sender As Object, e As EventArgs) Handles PanelSubWindowMain.Click
        If _flagShowProgram = 1 Then
            _showProgram.Close()
            _flagShowProgram = 0
        End If
        Dim cs As Control()
        '"ShowProgramListButton"の背景色を変える
        Dim buttonName As String = "ShowProgramListButton"
        cs = Me.Controls.Find(buttonName, True)
        If cs.Length > 0 Then
            CType(cs(0), Button).BackColor = System.Drawing.Color.White
        End If
    End Sub

    ' 現在画面上にあるプログラム一覧を表示ボタンを配置する----------------------------------------------
    Private Sub SetShowProgramListButton()
        Dim toolTip As New ToolTip With {
            .ShowAlways = False
        }
        Dim locationX As Integer = Me.Width - 450
        Dim button As New Button With {
                    .Size = New Size(25, 25),
                    .Location = New Point(locationX, 55),
                    .BackColor = System.Drawing.Color.White,
                    .BackgroundImageLayout = System.Windows.Forms.ImageLayout.None,
                    .Cursor = System.Windows.Forms.Cursors.Hand,
                    .Font = New System.Drawing.Font("MS UI Gothic", 7.0!),
                    .Image = CType(My.Resources.ResourceManager.GetObject("_05_showProgramList_black2"), Bitmap),
                    .ImageAlign = System.Drawing.ContentAlignment.TopCenter,
                    .Margin = New System.Windows.Forms.Padding(0),
                    .Name = "ShowProgramListButton",
                    .TextAlign = System.Drawing.ContentAlignment.BottomCenter,
                   .UseVisualStyleBackColor = False
         }
        button.FlatAppearance.BorderSize = 0
        button.FlatAppearance.BorderColor = System.Drawing.Color.White
        button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        button.FlatStyle = System.Windows.Forms.FlatStyle.Flat

        Dim msg As String = CType(My.Resources.ResourceManager.GetObject("ShowprogramList_msg"), String)
        toolTip.SetToolTip(button, msg)
        Me.PanelSubWindowToolBar.Controls.Add(button)

        'イベントハンドラとして機能するようにする
        AddHandler button.Click, AddressOf SwitchShowProgramListPanel
        AddHandler button.Click, AddressOf SetShowProgramListButton_Click
    End Sub

    ' 現在画面上にあるプログラム一覧を表示ボタンで表示されるPanelを切り替える---------------------------
    Private Sub SwitchShowProgramListPanel()
        Dim cs As Control()
        If _flagShowProgram = 1 Then
            '閉じる
            Dim buttonName As String = "ShowProgramListButton"
            cs = Me.Controls.Find(buttonName, True)
            If cs.Length > 0 Then
                CType(cs(0), Button).BackColor = System.Drawing.Color.White
            End If
        Else
            '開く
            Dim buttonName As String = "ShowProgramListButton"
            cs = Me.Controls.Find(buttonName, True)
            If cs.Length > 0 Then
                CType(cs(0), Button).BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
            End If
        End If

    End Sub

    ' MDIボタンを配置する---------------------------------------------------------
    Private Sub SetMdiButtons()
        Dim japanese As String = "ja-JP"
        Dim english As String = "en"
        Dim cultureString As String = Thread.CurrentThread.CurrentCulture.ToString
        If cultureString = japanese Then
            _setMdiButtons.LoadFile("SetMdiButtons_Japanese.csv")
        Else
            _setMdiButtons.LoadFile("SetMdiButtons_English.csv")
        End If

        Dim count As Integer = _setMdiButtons.Size
        Dim buttonCount As Integer = 0
        Dim lineCount As Integer = 0

        Dim toolTip As New ToolTip With {
            .ShowAlways = False
        }


        For i As Integer = 0 To count - 1
            Dim info As JCBMDIButtonInfo = _setMdiButtons._MdiButtonList(i)

            '区切り線とボタンのサイズのHegihtは同じ
            If info._buttonNo = -1 Then
                '区切り線を引く
                Dim locationX As Integer = (60 * buttonCount) + (11 * lineCount) + 10
                Dim separatorLine As New Label With {
                    .Size = New System.Drawing.Size(1, 75),
                    .Location = New System.Drawing.Point(locationX, 3),
                    .BackColor = System.Drawing.Color.Gainsboro
                }

                Me.PanelSubWindowToolBar.Controls.Add(separatorLine)
                lineCount += 1
            Else
                'ボタンを配置

                Dim imageName As String = "MdiIcon\" + info._buttonIcon
                Dim locationX As Integer = (60 * buttonCount) + (11 * lineCount) + 5
                Dim button As New Button With {
                    .Size = New Size(60, 75),
                    .Location = New Point(locationX, 3),
                    .BackColor = System.Drawing.Color.White,
                    .BackgroundImageLayout = System.Windows.Forms.ImageLayout.None,
                    .Cursor = System.Windows.Forms.Cursors.Hand,
                    .Font = New System.Drawing.Font("MS UI Gothic", 7.0!),
                    .Image = System.Drawing.Image.FromFile(imageName),
                    .ImageAlign = System.Drawing.ContentAlignment.TopCenter,
                    .Margin = New System.Windows.Forms.Padding(0),
                    .Name = info._programFunction,
                    .Text = info._buttonCaption,
                    .TextAlign = System.Drawing.ContentAlignment.BottomCenter,
                    .UseVisualStyleBackColor = False
                }
                button.FlatAppearance.BorderSize = 0
                button.FlatAppearance.BorderColor = System.Drawing.Color.Gray
                button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
                button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
                button.FlatStyle = System.Windows.Forms.FlatStyle.Flat

                toolTip.SetToolTip(button, info._buttonTips)
                Me.PanelSubWindowToolBar.Controls.Add(button)

                'イベントハンドラとして機能するようにする
                AddHandler button.Click, AddressOf MdiButtonClick

                buttonCount += 1
            End If

        Next

    End Sub

    ' MDIボタンのイベント---------------------------------------------------------
    Private Sub MdiButtonClick(ByVal sender As Object, ByVal e As EventArgs)

        Dim programName As String = sender.Name
        Dim positionX As Integer = 0
        Dim positionY As Integer = 0
        Dim count As Integer = _setMdiButtons.Size

        For i As Integer = 0 To count - 1
            Dim inf As JCBMDIButtonInfo = _setMdiButtons._MdiButtonList(i)
            If programName = inf._programFunction Then
                positionX = inf._activationPositionX
                positionY = inf._activationPositionY
                Exit For
            End If
        Next

        '実行するプログラムが関数かEXEか判定
        Dim str As String() = programName.Split(".")
            Dim programType As String = str(str.Length - 1)
        If programType = "exe" Then
            Dim exeName As String = GetExeName(programName)
            If exeName <> "" Then
                Dim exe As System.Diagnostics.Process = System.Diagnostics.Process.Start(exeName)

                'MDIを実装
                If exe.HasExited = False Then
                    exe.WaitForInputIdle()
                    SetParent(exe.MainWindowHandle, Me.PanelSubWindowMain.Handle)

                End If
            End If
        Else
            Dim func As Object = GetFunction(programName)
            SetParent(func.Handle, Me.PanelSubWindowMain.Handle)
            func.Location = New Point(positionX, positionY)
            func.Show()
        End If

    End Sub

    Private Function GetFunction(arg As String) As Object
        Dim func As New Object

        If arg = "SortUD()" Then

        ElseIf arg = "SortLR()" Then

        ElseIf arg = "FileExplore()" Then
            If SearchProgram(Me.PanelSubWindowMain.Handle, CType(My.Resources.ResourceManager.GetObject("FileExplore_text"), String)) = 1 Then
                _fileExplore.Close()
            End If
            Dim newObject As New JCBFileExplore3 With {
            ._kojiFolder = Me.TextBoxKojiFolder.Text,
            ._ijcadExe = _ijcadExe
            }
            _fileExplore = newObject
            func = newObject

        ElseIf arg = "ExeLog()" Then
            If SearchProgram(Me.PanelSubWindowMain.Handle, CType(My.Resources.ResourceManager.GetObject("ExecutionLog_text"), String)) = 1 Then
                _executionLog.Close()
            End If
            Dim newObject As New JCBExecutionLog
            _executionLog = newObject
            func = newObject
        End If

        Return func

    End Function

    Private Function GetExeName(arg As String) As String
        Dim exeName As String = ""

        If arg = "CatsSetting.exe" Then
            exeName = "CatsSetting.exe"

        ElseIf arg = "kumitate.exe" Then
            exeName = "kumitate.exe"

        ElseIf arg = "CatsResultViewer.exe" Then
            exeName = "CatsResultViewer.exe"

        ElseIf arg = "notepad.exe" Then
            exeName = "notepad.exe"

        End If

        Return exeName

    End Function

    '---------------------------------------------------------------------------------------------------
    ' サブウィンドウの画面にあるプログラムを表示ボタンのクリックイベント
    '---------------------------------------------------------------------------------------------------
    Private Sub SetShowProgramListButton_Click()
        If _flagShowProgram = 0 Then
            '開く
            Dim arrayList As ArrayList = GetDisplayProgram(Me.PanelSubWindowMain.Handle)
            Dim showProgram As New JCBShowSubWindowProgram With {
                ._buttonList = arrayList
            }
            SetParent(showProgram.Handle, Me.Handle)
            Dim locationX As Integer = Me.Width - 220
            showProgram.Location = New Point(locationX, 81)
            showProgram.Show()
            _showProgram = showProgram
            _flagShowProgram = 1
        Else
            _showProgram.Close()
            _flagShowProgram = 0
        End If
    End Sub

    '---------------------------------------------------------------------------------------------------
    ' MainWindow画面のサイズ変更が行われた際の処理
    '---------------------------------------------------------------------------------------------------
    Private Sub MainWindow_SizeChanged(sender As Object, e As EventArgs) _
    Handles MyBase.SizeChanged

        'サブウィンドウのサイズを変更
        Dim c As Control = DirectCast(sender, Control)
        Me.PanelSubWindowToolBar.Width = c.Width - 420
        Me.PanelSubWindowMain.Width = c.Width - 420
        Me.PanelSubWindowMain.Height = c.Height - 110
        Me.Label5.Height = c.Height
        Me.Label6.Height = c.Height

        ' サブウィンドウの画面にあるプログラムを表示ボタン
        If _flagShowProgram = 1 Then
            _showProgram.Close()
            _flagShowProgram = 0
        End If

        Dim name1 As String = "ShowProgramListButton"
        Dim cs1 As Control() = Me.Controls.Find(name1, True)
        If cs1.Length > 0 Then
            CType(cs1(0), Button).Location = New Point(Me.Width - 450, 55)
        End If

    End Sub

    '---------------------------------------------------------------------------------------------------
    ' MainWindow終了処理
    '---------------------------------------------------------------------------------------------------
    Private Sub Closed_MainWindow(sender As Object, e As EventArgs) Handles MyBase.Closed
        Dim sysFile As String = FunGetSystemFile()
        WriteSystemTxtClose(sysFile)
        If Me._isOkLicense = True Then
            If Me._isUseBatch = 1 Then
                ネットワークライセンスの返却(251, 0)
            End If
            If Me._isUseKaiwa = 1 Then
                ネットワークライセンスの返却(253, 0)
            End If
        End If
    End Sub

    '---------------------------------------------------------------------------------------------------
    ' その他
    '---------------------------------------------------------------------------------------------------
    'MDIを実装するためのハンドル設定
    <DllImport("USER32.DLL", CharSet:=CharSet.Auto)>
    Private Shared Function SetParent(
    ByVal hWndChild As System.IntPtr,
    ByVal hWndNewParent As System.IntPtr) As System.IntPtr
    End Function

    '指定したハンドルをアクティブにする
    <DllImport("user32.dll", SetLastError:=True)>
    Public Shared Sub SwitchToThisWindow(hWnd As IntPtr, fAltTab As Boolean)
    End Sub

End Class
