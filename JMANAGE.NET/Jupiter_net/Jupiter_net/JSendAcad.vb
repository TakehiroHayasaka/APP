'Attribute VB_Name = "JSendAcad"
Option Explicit

Imports System
Imports System.Data
Imports System.Text
Imports System.Runtime.InteropServices
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports System.Windows.Forms.Application

Module JSendAcad
    Private Delegate Function D_EnumWindowsProc(ByVal hWnd As Integer, ByVal lParam As Integer) As Long
    Private Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Long)
    Private Declare Sub keybd_event Lib "user32" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Long, ByVal dwExtraInfo As Long)
    Private Const KEYEVENTF_KEYUP As Long = &H2
    Private Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As Long) As Long
    Private Declare Function GetWindowText Lib "user32" Alias "GetWindowTextA" (ByVal hwnd As Long, ByVal lpString As String, ByVal cch As Long) As Long
    Private Declare Function EnumWindows Lib "user32" (ByVal lpEnumFunc As D_EnumWindowsProc, ByVal lParam As Long) As Long
    Private Declare Function GetParent Lib "user32" (ByVal hwnd As Long) As Long
    Private Declare Function SetActiveWindow Lib "user32" (ByVal hwnd As Long) As Long
    Private FindWinTitle As String  '検出するWindowのタイトル
    Private FindWinWnd As Long      '検出したWindowのハンドル
    Private Const MAX_CHAR = 256    '最大文字数
    Private nSendKeys As Long '=0:未初期化、=1:SendKeysを使用、=2:keybd_eventを使用（デフォルト）
    '※0,2以外は1と同じ
    Private nSleepTime As Long 'SendMsgにおけるSleep時間(ms)
    Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" _
        ( _
            <MarshalAs(UnmanagedType.LPStr)> ByVal lpApplicationName As String, _
            <MarshalAs(UnmanagedType.LPStr)> ByVal lpKeyName As String, _
            <MarshalAs(UnmanagedType.LPStr)> ByVal lpDefault As String, _
            <MarshalAs(UnmanagedType.LPStr)> ByVal lpReturnedString As StringBuilder, _
            ByVal nSize As Integer, _
            <MarshalAs(UnmanagedType.LPStr)> ByVal lpFileName As String _
        ) As Long

    '   高スペックのPCなどで、メッセージ送信が正常に動作しない問題の対応
    '   AppActivateやSendKeysメソッドは使用せずに、API関数にてWindowのActive化やキー送信を行う

    '---------------------------------------------------------------------------------------------------
    '機能
    '   ActiveなWindowに指定されたコードのキーを送信する
    '引数
    '   KeyCode(I)：キーコード
    '戻り値
    '   無し
    '---------------------------------------------------------------------------------------------------
    Private Sub SendOneKey(ByVal KeyCode As Long, ByVal blnShift As Boolean)

        If blnShift Then Call keybd_event(System.Windows.Forms.Keys.ShiftKey, 0, 0, 0)
        Call keybd_event(KeyCode, 0, 0, 0)
        Call keybd_event(KeyCode, 0, KEYEVENTF_KEYUP, 0)
        If blnShift Then Call keybd_event(System.Windows.Forms.Keys.ShiftKey, 0, KEYEVENTF_KEYUP, 0)

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   指定されたタイトルのWindowにメッセージを送信する
    '引数
    '   WinTitle(I)：Windowタイトル
    '   Message(I)：送信するメッセージ
    '戻り値
    '   無し
    '---------------------------------------------------------------------------------------------------
    Public Sub SendMsg(ByVal WinTitle As String, ByVal Message As String)
        Dim i As Integer
        Dim strMsg As String
        Dim strKey As String
        Dim lngKeyCode As Long
        Dim blnKeyOK As Boolean
        Dim strFileName As String

        strFileName = FunGetLanguage()

        FindWindowByTitle(WinTitle)
        SendOneKey(System.Windows.Forms.Keys.Escape, False)
        SendOneKey(System.Windows.Forms.Keys.Escape, False)
        SendOneKey(System.Windows.Forms.Keys.Escape, False)

        If Len(Message) = 0 Then Exit Sub '--ins  '11.05.27 mitsu

        FindWindowByTitle(WinTitle)
        Sleep(100)
        DoEvents()
        strMsg = UCase(Message)
        For i = 1 To Len(strMsg)
NextKey:
            strKey = Mid(strMsg, i, 1)
            Dim blnShift As Boolean
            blnShift = False
            If strKey = " " Or strKey = Chr(13) Then
                lngKeyCode = System.Windows.Forms.Keys.Return
            ElseIf strKey = "." Then
                lngKeyCode = System.Windows.Forms.Keys.Decimal
            ElseIf strKey = "+" Then
                lngKeyCode = System.Windows.Forms.Keys.Add
            ElseIf strKey = "\" Or strKey = "/" Then
                lngKeyCode = System.Windows.Forms.Keys.Divide
            ElseIf strKey = "*" Then
                lngKeyCode = System.Windows.Forms.Keys.Multiply
            ElseIf strKey = "-" Then
                lngKeyCode = System.Windows.Forms.Keys.Subtract
            ElseIf strKey = ":" Then
                lngKeyCode = 186

            ElseIf strKey = "!" Then
                lngKeyCode = System.Windows.Forms.Keys.D1
                blnShift = True
            ElseIf strKey = "#" Then
                lngKeyCode = System.Windows.Forms.Keys.D3
                blnShift = True
            ElseIf strKey = "$" Then
                lngKeyCode = System.Windows.Forms.Keys.D4
                blnShift = True
            ElseIf strKey = "%" Then
                lngKeyCode = System.Windows.Forms.Keys.D5
                blnShift = True
            ElseIf strKey = "&" Then
                lngKeyCode = System.Windows.Forms.Keys.D6
                blnShift = True
            ElseIf strKey = "'" Then
                lngKeyCode = System.Windows.Forms.Keys.D7
                blnShift = True
            ElseIf strKey = "(" Then
                lngKeyCode = System.Windows.Forms.Keys.D8
                blnShift = True
            ElseIf strKey = ")" Then
                lngKeyCode = System.Windows.Forms.Keys.D8
                blnShift = True
            ElseIf strKey = "=" Then
                lngKeyCode = 189
                blnShift = True
            ElseIf strKey = "^" Then
                lngKeyCode = 222
            ElseIf strKey = "~" Then
                lngKeyCode = 222
                blnShift = True
            ElseIf strKey = "@" Then
                lngKeyCode = 192
            ElseIf strKey = "`" Then
                lngKeyCode = 192
                blnShift = True
            ElseIf strKey = "[" Then
                lngKeyCode = 219
            ElseIf strKey = "{" Then
                lngKeyCode = 219
                blnShift = True
            ElseIf strKey = ";" Then
                lngKeyCode = 187
            ElseIf strKey = "]" Then
                lngKeyCode = 221
            ElseIf strKey = "}" Then
                lngKeyCode = 221
                blnShift = True
            ElseIf strKey = "," Then
                lngKeyCode = 188
            ElseIf strKey = "_" Then
                lngKeyCode = 226
                blnShift = True
            Else
                lngKeyCode = AscW(strKey)
                blnKeyOK = False
                If System.Windows.Forms.Keys.D0 <= lngKeyCode And lngKeyCode <= System.Windows.Forms.Keys.D9 Then blnKeyOK = True '数字ならOK
                If System.Windows.Forms.Keys.A <= lngKeyCode And lngKeyCode <= System.Windows.Forms.Keys.Z Then blnKeyOK = True 'アルファベットならOK
                If blnKeyOK = False Then
                    ' 送れないキーがある場合は処理を終了する
                    If Not strFileName Like "*ENG.INI" Then
                        MsgBox("キー（" & strKey & "）を送信できません。" & vbCr & "ファイル名にスペースや２バイト文字が使用されていないか確認してください。", vbExclamation)
                    Else
                        MsgBox("A key（" & strKey & "）cannot be transmitted." & vbCr & "Please check whether neither the space nor the double byte character is used for the file name.", vbExclamation)
                    End If
                    Exit Sub
                End If
            End If
            SendOneKey(lngKeyCode, blnShift)
            Sleep(nSleepTime)
            DoEvents()
        Next i
        If lngKeyCode <> System.Windows.Forms.Keys.Return Then
            '最後のキーが[Enter]ではない場合
            SendOneKey(System.Windows.Forms.Keys.Return, False)
            Sleep(nSleepTime)
            DoEvents()
        End If

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   指定されたタイトルのWindowを検索し、Activeにする
    '引数
    '   WinTitle(I)：Windowタイトル
    '戻り値
    '   無し
    '---------------------------------------------------------------------------------------------------
    Private Sub FindWindowByTitle(ByVal WinTitle As String)
        FindWinTitle = WinTitle
        Call EnumWindows(AddressOf EnumWinProc, 0&)

        If (FindWinWnd <> 0&) Then
            If SetForegroundWindow(FindWinWnd) > 0 Then
                SetActiveWindow(FindWinWnd)
            Else
                Debug.Print("SetForegroundWindow NG!")
            End If
            Sleep(nSleepTime)
        End If

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   Windowsの全ハンドルを取得する
    '引数
    '   API関数 EnumWindows に渡す情報
    '   hWnd(O) ：コールバック関数へのポインタ
    '   Param(O)：アプリケーション定義の値
    '戻り値
    '   True/False
    '---------------------------------------------------------------------------------------------------
    Public Function EnumWinProc(ByVal hwnd As Long, Param As Long) As Boolean
        'Dim lngTrd As Long  'スレッド
        'Dim lngPrs As Long  'プロセス

        'Trueの間は、Windowsに存在するハンドルを最後まで取得しようとする
        EnumWinProc = True

        '子ウィンドウは未処理
        If Not (GetParent(hwnd) = 0) Then GoTo PGMEND

        'ウィンドウのタイトルを取得
        Dim strBuf As String = ""
        Dim strTitle As String = ""
        Dim lngLen As Long
        strBuf = ""
        lngLen = GetWindowText(hwnd, strBuf, 256)
        If lngLen > 0 Then
            strTitle = Left(strBuf, lngLen)
        End If

        '同じプロセスだとしたら
        If strTitle Like FindWinTitle & "*" Then
            '取得してきたハンドルを記憶
            FindWinWnd = hwnd
            'これ以上のハンドルは取得しないでもいいので、Falseをセット
            EnumWinProc = False
        End If

PGMEND:
    End Function

    '---------------------------------------------------------------------------------------------------
    '機能
    '   指定したWindowにメッセージを送信する
    '引数
    '   WinTitle(I) ：メッセージを送信するWindowのタイトル
    '   Message(I)：送信するメッセージ
    '戻り値
    '   True/False
    '---------------------------------------------------------------------------------------------------
    Function Message送信(WinTitle As String, Message As String) As Boolean
        Message送信 = False
        On Error GoTo ErrHandle
        SendMsg(WinTitle, Message)

        Message送信 = True
        Exit Function

ErrHandle:

    End Function

    Function AutoCADへ送信(ScrFile As String) As Boolean
        '---------------------------------------------------------------------------------------------------
        '機能
        '   AutoCADにｽｸﾘﾌﾟﾄﾌｧｲﾙを送信する。
        '引数
        '   ScrFile(I)：ｽｸﾘﾌﾟﾄﾌｧｲﾙ名
        '戻り値
        '   無し
        '---------------------------------------------------------------------------------------------------
        Dim i As Integer
        Dim cmd1 As String
        Dim buf As String, msg As String
        Dim cadWindowTitle As String
        Dim strScrSec_def As String
        Dim strScrSec As String = ""
        Dim intScrSec As Integer
        Dim PauseTime As New Timer()
        Dim strDelay As String
        Dim strFileName As String

        strFileName = FunGetLanguage()

        cadWindowTitle = FunGetCadWindowTitle()
        AutoCADへ送信 = False

        On Error GoTo ACAD_ENABLED

        ' 初期化
        If nSendKeys = 0 Then
            nSendKeys = GetSendKeysMethod()
            nSleepTime = GetSleepTime()
        End If

        If nSendKeys = 2 Then
            ' keybd_eventを使用してメッセージ送信
            '************************************************************************************************************************
            '   SendKeysメソッドは、高スペックのPCだと正しく送信できない場合があるため、使用しない
            '   API関数 keybd_event を使用して、キーを1つずつ送信する
            '   この関数では ":"や"\"が送信できないため、スクリプトファイル名は、パスや拡張子を取り除いたファイル名のみで指定する
            '   パスは、直前にカレントフォルダを
            '************************************************************************************************************************
            '    Dim strScrPath As String
            '    Dim strScrName As String
            '    strScrPath = VBA.Left(ScrFile, InStrRev(ScrFile, "\") - 1)
            '    strScrName = VBA.Right(ScrFile, Len(ScrFile) - InStrRev(ScrFile, "\"))
            '    If StrComp(Right(strScrName, 4), ".SCR", vbTextCompare) = 0 Then
            '        strScrName = VBA.Left(strScrName, Len(strScrName) - 4)  '拡張子を取り除く
            '    End If
            '    VBA.ChDir strScrPath               'カレントフォルダをスクリプトファイルのパスに設定する

            cmd1 = "FILEDIA 0"
            Call SendMsg(cadWindowTitle, cmd1)

            '    cmd1 = "SCRIPT " & strScrName
            cmd1 = "SCRIPT " & ScrFile
            Call SendMsg(cadWindowTitle, cmd1)

        Else
            ' SendKeysを使用してメッセージ送信
            If cadWindowTitle Like "I*" Then
                'AppActivate(cadWindowTitle, False)
                'SendKeys("{ESC}{ESC}{ESC}{ESC}{ESC}{ESC}", True)
                AppActivate(cadWindowTitle)
                SendKeys.SendWait("{ESC}{ESC}{ESC}{ESC}{ESC}{ESC}")

                cmd1 = "FILEDIA " & "0" & Chr(13) & Chr(10)
                'AppActivate(cadWindowTitle, False)
                'SendKeys(cmd1, True)
                AppActivate(cadWindowTitle)
                SendKeys.SendWait(cmd1)

                'AppActivate(cadWindowTitle, False)
                'SendKeys("{ESC}{ESC}{ESC}{ESC}{ESC}{ESC}", True)
                AppActivate(cadWindowTitle)
                SendKeys.SendWait("{ESC}{ESC}{ESC}{ESC}{ESC}{ESC}")

                PauseTime.Interval = 0.5                ' 中断時間を設定します。
                PauseTime.Enabled = True
                'StartTime = Timer              ' 中断の開始時刻を設定します。
                'Do While Timer < StartTime + PauseTime
                'Loop

                cmd1 = "FILEDIA " & "0" & Chr(13) & Chr(10)
                'AppActivate(cadWindowTitle, False)
                'SendKeys(cmd1, True)
                AppActivate(cadWindowTitle)
                SendKeys.SendWait(cmd1)

                'AppActivate(cadWindowTitle, False)
                'SendKeys("{ESC}{ESC}{ESC}{ESC}{ESC}{ESC}", True)
                AppActivate(cadWindowTitle)
                SendKeys.SendWait("{ESC}{ESC}{ESC}{ESC}{ESC}{ESC}")

                cmd1 = "SCRIPT " & ScrFile & Chr(13) & Chr(10)
                SendKeys.SendWait(cmd1)
            Else
                strScrSec_def = "0"
                Call subGetOther("WaitingTime", "SCR", strScrSec_def, strScrSec)
                intScrSec = Int(CDbl(strScrSec) * 1000)
                strDelay = "DELAY " & intScrSec & " "

                If intScrSec > 0 Then
                    'AppActivate(cadWindowTitle, False)
                    'SendKeys("{ESC}{ESC}{ESC}", True)
                    AppActivate(cadWindowTitle)
                    SendKeys.SendWait("{ESC}{ESC}{ESC}")

                    'AppActivate(cadWindowTitle, False)
                    'SendKeys(strDelay, True)
                    AppActivate(cadWindowTitle)
                    SendKeys.SendWait(strDelay)

                    cmd1 = "FILEDIA "
                    'AppActivate(cadWindowTitle, False)
                    'SendKeys(cmd1, True)
                    AppActivate(cadWindowTitle)
                    SendKeys.SendWait(cmd1)

                    '            PauseTime = dblScrSec          ' 中断時間を設定します。
                    '            StartTime = Timer              ' 中断の開始時刻を設定します。
                    '            Do While Timer < StartTime + PauseTime
                    '            Loop

                    cmd1 = "0 "
                    'AppActivate(cadWindowTitle, False)
                    'SendKeys(cmd1, True)
                    AppActivate(cadWindowTitle)
                    SendKeys.SendWait(cmd1)

                    '            PauseTime = dblScrSec          ' 中断時間を設定します。
                    '            StartTime = Timer              ' 中断の開始時刻を設定します。
                    '            Do While Timer < StartTime + PauseTime
                    '            Loop

                    cmd1 = "FILEDIA "
                    'AppActivate(cadWindowTitle, False)
                    'SendKeys(cmd1, True)
                    AppActivate(cadWindowTitle)
                    SendKeys.SendWait(cmd1)

                    '            PauseTime = dblScrSec          ' 中断時間を設定します。
                    '            StartTime = Timer              ' 中断の開始時刻を設定します。
                    '            Do While Timer < StartTime + PauseTime
                    '            Loop

                    cmd1 = "0 "
                    'AppActivate(cadWindowTitle, False)
                    'SendKeys(cmd1, True)
                    AppActivate(cadWindowTitle)
                    SendKeys.SendWait(cmd1)

                    '            PauseTime = dblScrSec          ' 中断時間を設定します。
                    '            StartTime = Timer              ' 中断の開始時刻を設定します。
                    '            Do While Timer < StartTime + PauseTime
                    '            Loop
                Else
                    'AppActivate(cadWindowTitle, False)
                    'SendKeys("{ESC}{ESC}{ESC}", True)
                    AppActivate(cadWindowTitle)
                    SendKeys.SendWait("{ESC}{ESC}{ESC}")

                    cmd1 = "FILEDIA " & "0 "
                    'AppActivate(cadWindowTitle, False)
                    'SendKeys(cmd1, True)
                    AppActivate(cadWindowTitle)
                    SendKeys.SendWait(cmd1)

                    'AppActivate(cadWindowTitle, False)
                    'SendKeys("{ESC}{ESC}{ESC}", True)
                    AppActivate(cadWindowTitle)
                    SendKeys.SendWait("{ESC}{ESC}{ESC}")

                    cmd1 = "FILEDIA " & "0 "
                    'AppActivate(cadWindowTitle, False)
                    'SendKeys(cmd1, True)
                    AppActivate(cadWindowTitle)
                    SendKeys.SendWait(cmd1)

                    'AppActivate(cadWindowTitle, False)
                    'SendKeys("{ESC}{ESC}{ESC}", True)
                    AppActivate(cadWindowTitle)
                    SendKeys.SendWait("{ESC}{ESC}{ESC}")
                End If

                cmd1 = "SCRIPT " & ScrFile & Chr(13) & Chr(10)
                'SendKeys(cmd1, True)
                SendKeys.SendWait(cmd1)
        End If
        End If

        On Error GoTo ACAD_ENABLED
        DoEvents()

        AutoCADへ送信 = True

        Exit Function

ACAD_ENABLED:
        Beep()
        If Not strFileName Like "*ENG.INI" Then
            msg = cadWindowTitle & "に" & ScrFile & "の内容を送信出来ません。" & Chr(10)
            msg = msg & cadWindowTitle & "が実行されているか確認してください。"
        Else
            msg = "The contents of " & ScrFile & "cannot be transmitted to " & cadWindowTitle & Chr(10)
            msg = msg & "Please check whether cad is performed."
        End If
        MsgBox(msg, vbOKOnly + vbCritical)
        Resume ERR_END

ERR_END:

    End Function

    Function SCRIPT送信(ScrFile As String, Options As String) As Boolean
        '---------------------------------------------------------------------------------------------------
        '機能
        '   CADまたはJCONSOLEへｽｸﾘﾌﾟﾄﾌｧｲﾙを送信する。
        '引数
        '   ScrFile(I)：ｽｸﾘﾌﾟﾄﾌｧｲﾙ名
        '   AppName(I)：ｱﾌﾟﾘｹｰｼｮﾝ名
        '   Lock   (I)：多重起動禁止　True：禁止,False：許可
        '戻り値
        '   無し
        '---------------------------------------------------------------------------------------------------
        Dim pos As Integer
        Dim msg As String
        Dim jconsole As String
        Dim strKoji As String = ""
        Dim strFileName As String

        strFileName = FunGetLanguage()

        jconsole = FunGetJconsole()

        SCRIPT送信 = False
        On Error GoTo SCRIPT_ENABLED

        If jconsole = "ON" Then
            SCRIPT送信 = True
            Dim sHnd As Long
            Dim AppName As String

            pos = InStrRev(ScrFile, "\")
            If pos <> 0 Then
                strKoji = Left(ScrFile, pos)
            End If
            AppName = strKoji & "JCONSOLE.EXE"
            If Dir(AppName, vbNormal) = "" Then
                AppName = FunGetBin() & "JCONSOLE.EXE"
            End If
            sHnd = Shell(AppName & " " & ScrFile & " " & Options, vbNormalFocus)
        Else
            SCRIPT送信 = AutoCADへ送信(ScrFile)
        End If
        Exit Function

SCRIPT_ENABLED:
        Beep()
        If Not strFileName Like "*ENG.INI" Then
            msg = ScrFile & "の内容を送信出来ません。" & Chr(10)
        Else
            msg = "The contents cannot be transmitted." & Chr(10)
        End If
        MsgBox(msg, vbOKOnly + vbCritical)
        Resume ERR_END

ERR_END:

    End Function

    '---------------------------------------------------------------------------------------------------
    '機能
    '   AutoCAD/IJCADにメッセージを送信する
    '引数
    '   cmd：送信するメッセージ
    '戻り値
    '   なし
    '---------------------------------------------------------------------------------------------------
    Public Sub COMMAND送信(cmd As String)
        Dim cadWindowTitle As String

        ' 初期化
        If nSendKeys = 0 Then
            nSendKeys = GetSendKeysMethod()
            nSleepTime = GetSleepTime()
        End If

        If nSendKeys = 2 Then
            cadWindowTitle = FunGetCadWindowTitle()
            Call SendMsg(cadWindowTitle, cmd)
        Else
            'SendKeys(cmd, True)
            SendKeys.SendWait(cmd)
        End If
    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   AutoCAD/IJCADにメッセージを送信する方法を決定する
    '引数
    '   なし
    '戻り値
    '   =1:SendKeysを使用、=2:keybd_eventを使用
    '---------------------------------------------------------------------------------------------------
    Private Function GetSendKeysMethod() As Long
        On Error GoTo labError
        GetSendKeysMethod = 2

        ' メッセージ送信の方法を決定する
        Dim IniFile As String
        Dim ans As New StringBuilder(MAX_CHAR)
        Dim strSendKeys As String
        Dim ret As Long

        ' Jmanage.iniから読み出し
        IniFile = FunGetEnviron() & "JMANAGE.INI"

        ' デフォルトは2:keybd_eventを使用
        ret = GetPrivateProfileString("Option", "SendKeys", "2", ans, MAX_CHAR, IniFile)
        'strSendKeys = Left(ans, InStr(ans, Chr(0)) - 1)
        strSendKeys = ans.ToString.Trim
        If strSendKeys = "1" Then
            GetSendKeysMethod = 1
        Else
            GetSendKeysMethod = 2
        End If

labError:
    End Function

    '---------------------------------------------------------------------------------------------------
    '機能
    '   AutoCAD/IJCADにメッセージを送信するときのスリープ時間を決定する
    '引数
    '   なし
    '戻り値
    '   スリープ時間(ms)（デフォルト50）
    '---------------------------------------------------------------------------------------------------
    Private Function GetSleepTime() As Long
        On Error GoTo labError
        GetSleepTime = 50

        ' メッセージ送信の方法を決定する
        Dim IniFile As String
        Dim ans As New StringBuilder(MAX_CHAR)
        Dim strSleepTime As String
        Dim ret As Long

        ' Jmanage.iniから読み出し
        IniFile = FunGetEnviron() & "JMANAGE.INI"

        ' デフォルトは2:keybd_eventを使用
        ret = GetPrivateProfileString("Option", "SendKeysSleepTime", "50", ans, MAX_CHAR, IniFile)
        'strSleepTime = Left(ans, InStr(ans, Chr(0)) - 1)
        strSleepTime = ans.ToString.Trim
        If Val(strSleepTime) > 0 Then
            GetSleepTime = Val(strSleepTime)
        Else
            GetSleepTime = 50
        End If
labError:
    End Function
End Module