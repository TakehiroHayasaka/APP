Option Strict Off
Option Explicit On
Module JCharge

    '***************************************************************************************************
    '使用しているWindowsAPIの宣言
    '***************************************************************************************************
    'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
    Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer

    'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
    'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
    Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Integer

    Declare Function GetWindowsDirectory Lib "kernel32" Alias "GetWindowsDirectoryA" (ByVal lpBuffer As String, ByVal nSize As Integer) As Integer

    '***************************************************************************************************
    'ﾕｰｻﾞｰ定義定数の宣言
    '***************************************************************************************************
    '***************************************************************************************************
    '変数ｻｲｽﾞ ※各ｻｲｽﾞは'\0'挿入用に1つ大きく設定するものとする。このとき、変数ｻｲｽﾞが奇数となる。
    'これをVBで読み書きするとﾃﾞｰﾀがおかしくなるため(原因不明)、さらに1つ大きく設定する。
    '***************************************************************************************************
    '2017/07/28 Nakagawa Edit Start
    'Public Const CHARGE_SIZE As Short = 10 'ﾁｬｰｼﾞｺｰﾄﾞｻｲｽﾞ
    'Public Const KAISHA_SIZE As Short = 52 '会社名ｻｲｽﾞ
    'Public Const KJCODE_SIZE As Short = 18 '工事ｺｰﾄﾞｻｲｽﾞ
    'Public Const KJMEI_SIZE As Short = 52 '工事名ｻｲｽﾞ
    'Public Const HACCHU_SIZE As Short = 52 '発注者ｻｲｽﾞ
    'Public Const TANTOU_SIZE As Short = 22 '処理担当者ｻｲｽﾞ
    'Public Const DATE_MAX As Short = 10 '日付,重量繰り返し数
    'Public Const DATE_SIZE As Short = 12 '日付ｻｲｽﾞ

    'Private Const MAX_CHAR As Short = 256 '最大文字数
    Public Const CHARGE_SIZE As Integer = 10 'ﾁｬｰｼﾞｺｰﾄﾞｻｲｽﾞ
    Public Const KAISHA_SIZE As Integer = 52 '会社名ｻｲｽﾞ
    Public Const KJCODE_SIZE As Integer = 18 '工事ｺｰﾄﾞｻｲｽﾞ
    Public Const KJMEI_SIZE As Integer = 52 '工事名ｻｲｽﾞ
    Public Const HACCHU_SIZE As Integer = 52 '発注者ｻｲｽﾞ
    Public Const TANTOU_SIZE As Integer = 22 '処理担当者ｻｲｽﾞ
    Public Const DATE_MAX As Integer = 10 '日付,重量繰り返し数
    Public Const DATE_SIZE As Integer = 12 '日付ｻｲｽﾞ

    Private Const MAX_CHAR As Integer = 256 '最大文字数
    Private Const FILE_SIZE As Integer = 954     'CHARGE構造体サイズ
    '2017/07/28 Nakagawa Edit End

    '***************************************************************************************************
    'ﾕｰｻﾞｰ定義構造体の宣言
    '***************************************************************************************************
    '---------------------------------------------------------------------------------------------------
    '日付用構造体
    '---------------------------------------------------------------------------------------------------
    Structure sdate
        'UPGRADE_WARNING: Fixed-length string size must fit in the buffer. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="3C1E4426-0B80-443E-B943-0627CD55D48B"'
        '2017/07/28 Nakagawa Edit Start
        '<VBFixedString(DATE_SIZE),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray,SizeConst:=DATE_SIZE)> Public strdate() As Char
        <VBFixedStringAttribute(DATE_SIZE)> Dim strdate As String
        '2017/07/28 Nakagawa Edit End
    End Structure

    '---------------------------------------------------------------------------------------------------
    'CHARGE.INIﾃﾞｰﾀ用構造体
    '---------------------------------------------------------------------------------------------------
    Structure CHARGE
        '2017/07/28 Nakagawa Edit Start
        ''UPGRADE_WARNING: Fixed-length string size must fit in the buffer. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="3C1E4426-0B80-443E-B943-0627CD55D48B"'
        '<VBFixedString(CHARGE_SIZE), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=CHARGE_SIZE)> Public charge_code() As Char 'ﾁｬｰｼﾞｺｰﾄﾞ
        ''UPGRADE_WARNING: Fixed-length string size must fit in the buffer. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="3C1E4426-0B80-443E-B943-0627CD55D48B"'
        '<VBFixedString(KAISHA_SIZE), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=KAISHA_SIZE)> Public kaisha_mei() As Char '会社名
        ''UPGRADE_WARNING: Fixed-length string size must fit in the buffer. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="3C1E4426-0B80-443E-B943-0627CD55D48B"'
        '<VBFixedString(KJCODE_SIZE), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=KJCODE_SIZE)> Public kouji_code() As Char '工事ｺｰﾄﾞ
        ''UPGRADE_WARNING: Fixed-length string size must fit in the buffer. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="3C1E4426-0B80-443E-B943-0627CD55D48B"'
        '<VBFixedString(KJMEI_SIZE), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=KJMEI_SIZE)> Public kouji_mei() As Char '工事名
        ''UPGRADE_WARNING: Fixed-length string size must fit in the buffer. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="3C1E4426-0B80-443E-B943-0627CD55D48B"'
        '<VBFixedString(HACCHU_SIZE), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=HACCHU_SIZE)> Public hacchu_sha() As Char '発注者
        <VBFixedStringAttribute(CHARGE_SIZE)> Dim charge_code As String   'ﾁｬｰｼﾞｺｰﾄﾞ
        <VBFixedStringAttribute(KAISHA_SIZE)> Dim kaisha_mei As String    '会社名
        <VBFixedStringAttribute(KJCODE_SIZE)> Dim kouji_code As String    '工事ｺｰﾄﾞ
        <VBFixedStringAttribute(KJMEI_SIZE)> Dim kouji_mei As String      '工事名
        <VBFixedStringAttribute(HACCHU_SIZE)> Dim hacchu_sha As String    '発注者
        '2017/07/28 Nakagawa Edit End
        Dim keisiki As Integer '橋梁形式
        Dim gairyaku_juuryou As Double '概略重量(単位:ton)
        Dim sikan_suu As Integer '支間数
        Dim shuketa_daisuu As Integer '主桁台数
        Dim deck_daisuu As Integer 'ﾃﾞｯｷ台数
        Dim yokoketa_daisuu As Integer '横桁台数
        Dim brket_daisuu As Integer 'ﾌﾞﾗｹｯﾄ台数
        Dim nakatate_daisuu As Integer '中縦桁台数
        Dim sokutate_daisuu As Integer '側縦桁台数
        Dim taikeikou_daisuu As Integer '対頃構台数
        Dim yokokou As Integer '横構有無(1:無,2:有)
        '2017/07/28 Nakagawa Edit Start
        ''UPGRADE_WARNING: Fixed-length string size must fit in the buffer. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="3C1E4426-0B80-443E-B943-0627CD55D48B"'
        '<VBFixedString(DATE_SIZE), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=DATE_SIZE)> Public shori_kaisi() As Char '処理開始予定日
        ''UPGRADE_WARNING: Fixed-length string size must fit in the buffer. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="3C1E4426-0B80-443E-B943-0627CD55D48B"'
        '<VBFixedString(DATE_SIZE), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=DATE_SIZE)> Public shori_shuuryou() As Char '処理終了予定日
        ''UPGRADE_WARNING: Fixed-length string size must fit in the buffer. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="3C1E4426-0B80-443E-B943-0627CD55D48B"'
        '<VBFixedString(TANTOU_SIZE), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=TANTOU_SIZE)> Public shori_tantou() As Char '処理担当者
        <VBFixedStringAttribute(DATE_SIZE)> Dim shori_kaisi As String     '処理開始予定日
        <VBFixedStringAttribute(DATE_SIZE)> Dim shori_shuuryou As String  '処理終了予定日
        <VBFixedStringAttribute(TANTOU_SIZE)> Dim shori_tantou As String  '処理担当者
        '2017/07/28 Nakagawa Edit End
        Dim skl_shori_flg As Integer '骨組一括処理実行ﾌﾗｸﾞ(1:不可,2:可)
        Dim tenkai_flg As Integer '展開処理実行ﾌﾗｸﾞ(1:不可,2:可)
        '2017/07/28 Nakagawa Edit Start
        'UPGRADE_WARNING: Fixed-length string size must fit in the buffer. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="3C1E4426-0B80-443E-B943-0627CD55D48B"'
        '<VBFixedString(DATE_SIZE), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=DATE_SIZE)> Public sinki_touroku() As Char '新規登録処理日
        <VBFixedStringAttribute(DATE_SIZE)> Dim sinki_touroku As String   '新規登録処理日
        '2017/07/28 Nakagawa Edit End
        Dim skl_shori_kaisuu As Integer '骨組一括処理実行回数
        '2017/07/28 Nakagawa Edit Star
        '<VBFixedArray(DATE_MAX)> Dim skl_shori_date() As sdate '骨組一括処理実行日
        Dim skl_shori_date() As sdate                                     '骨組一括処理実行日
        '2017/07/28 Nakagawa Edit End
        Dim buzai_shori_kaisuu As Integer '部材一括処理実行回数
        '2017/07/28 Nakagawa Edit Star
        '<VBFixedArray(DATE_MAX)> Dim buzai_shori_date() As sdate '部材一括処理実行日
        Dim buzai_shori_date() As sdate                                   '部材一括処理実行日
        '2017/07/28 Nakagawa Edit End
        Dim tenkai_shori_kaisuu As Integer '展開処理実行回数
        '2017/07/28 Nakagawa Edit Star
        '<VBFixedArray(DATE_MAX)> Dim tenkai_shori_date() As sdate '展開処理実行日
        '<VBFixedArray(DATE_MAX)> Dim tenkai_jyuuryou() As Double '展開処理後のPMFの重量
        Dim tenkai_shori_date() As sdate                                  '展開処理実行日
        Dim tenkai_jyuuryou() As Double                                   '展開処理後のPMFの重量
        '2017/07/28 Nakagawa Edit End
        Dim bunrui_shori_kaisuu As Integer '分類処理実行回数
        '2017/07/28 Nakagawa Edit Star
        '<VBFixedArray(DATE_MAX)> Dim bunrui_shori_date() As sdate '分類処理実行日
        '<VBFixedArray(DATE_MAX)> Dim bunrui_jyuuryou() As Double '分類処理後のPMFの重量
        Dim bunrui_shori_date() As sdate                                  '分類処理実行日
        Dim bunrui_jyuuryou() As Double                                   '分類処理後のPMFの重量
        '2017/07/28 Nakagawa Edit End

        'UPGRADE_TODO: "Initialize" must be called to initialize instances of this structure. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="B4BFF9E0-8631-45CF-910E-62AB3970F27B"'
        Public Sub Initialize()
            'UPGRADE_WARNING: Lower bound of array skl_shori_date was changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
            ReDim skl_shori_date(DATE_MAX)
            'UPGRADE_WARNING: Lower bound of array buzai_shori_date was changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
            ReDim buzai_shori_date(DATE_MAX)
            'UPGRADE_WARNING: Lower bound of array tenkai_shori_date was changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
            ReDim tenkai_shori_date(DATE_MAX)
            'UPGRADE_WARNING: Lower bound of array tenkai_jyuuryou was changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
            ReDim tenkai_jyuuryou(DATE_MAX)
            'UPGRADE_WARNING: Lower bound of array bunrui_shori_date was changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
            ReDim bunrui_shori_date(DATE_MAX)
            'UPGRADE_WARNING: Lower bound of array bunrui_jyuuryou was changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
            ReDim bunrui_jyuuryou(DATE_MAX)
        End Sub
    End Structure

    '---------------------------------------------------------------------------------------------------
    'BLOCK.INFﾃﾞｰﾀ用構造体
    '---------------------------------------------------------------------------------------------------
    Structure KOZO
        Dim shuketa_daisuu As Integer '主桁台数
        Dim deck_daisuu As Integer 'ﾃﾞｯｷ台数
        Dim yokoketa_daisuu As Integer '横桁台数
        Dim brket_daisuu As Integer 'ﾌﾞﾗｹｯﾄ台数
        Dim tateketa_daisuu As Integer '縦桁(中、側)台数
        Dim taikeikou_daisuu As Integer '対頃構台数
        Dim yokokou As Integer '横構有無(1:無,2:有)
    End Structure

    Sub subCHARGE新規日データの格納(ByRef Kogo As String)
        '---------------------------------------------------------------------------------------------------
        '機能
        '   工号の下にある工事別ﾌﾟﾛﾌｧｲﾙから登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を読み込み、
        '   ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙのその工事の新規登録日に日付を格納する。
        '引数
        '   kogo(I)：処理対象工号（ﾃﾞｨﾚｸﾄﾘ名）
        '戻り値
        '   無し
        '---------------------------------------------------------------------------------------------------
        Dim fnum, recno As Short
        Dim fname, ProFile, WinDir As String
        'UPGRADE_NOTE: today was upgraded to today_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
        Dim today_Renamed As String
        Dim ans As New VB6.FixedLengthString(MAX_CHAR)
        Dim msg As String
        Dim strWindowsDirectoryBuffer As New VB6.FixedLengthString(260)
        Dim ret As Integer
        Dim lngWindowsDirectoryLength As Integer
        'UPGRADE_WARNING: Arrays in structure RecData may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        '2017/07/28 Nakagawa Edit Start
        'Dim RecData As CHARGE
        Dim RecData As New CHARGE()
        '2017/07/28 Nakagawa Edit End
        Dim strFileName As String

        strFileName = FunGetLanguage()

        '工事別ﾌﾟﾛﾌｧｲﾙから登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を読み込む。
        ProFile = FunGetNewKoji() & Kogo & "\PROFILE.INI"
        '登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を取得する。
        ret = GetPrivateProfileString("チャージコード", "登録ファイルレコード位置", "-999", ans.Value, MAX_CHAR, ProFile)
        recno = CShort(Left(ans.Value, InStr(ans.Value, Chr(0)) - 1))

        If recno <= 0 Then
            Exit Sub
        End If

        'Windowsﾃﾞｨﾚｸﾄﾘ（例 D:\WINNT）のﾊﾟｽ名を取得する。
        lngWindowsDirectoryLength = GetWindowsDirectory(strWindowsDirectoryBuffer.Value, Len(strWindowsDirectoryBuffer.Value))
        WinDir = Left(strWindowsDirectoryBuffer.Value, InStr(strWindowsDirectoryBuffer.Value, vbNullChar) - 1)

        'ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙ（CHARGE.INI）の登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を読み込み、新規登録日を書き込む。
        '<<<<2014/06/05 チャージファイルはc:Jupiter 直下に
        'fname = WinDir & "\CHARGE\CHARGE.INI"
        fname = get_CHARGE_FileName
        '>>>>2014/06/05 チャージファイルはc:Jupiter 直下に


        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(fname, FileAttribute.Normal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙ(CHARGE.INI)がありません。" & Chr(10)
                msg = msg & "このﾌｧｲﾙが無いと処理を継続出来ません。"
            Else
                msg = "There is no charge code registration file (CHARGE.INI). " & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        fnum = FreeFile()
        '2017/07/28 Nakagawa Edit Start
        'FileOpen(fnum, fname, OpenMode.Random, OpenAccess.ReadWrite, , Len(RecData))
        FileOpen(fnum, fname, OpenMode.Random, OpenAccess.ReadWrite, , FILE_SIZE)
        '2017/07/28 Nakagawa Edit End
        'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(fnum, RecData, recno)

        '現在の日付を取得する。
        today_Renamed = VB6.Format(Now, "yyyy/mm/dd")

        '新規登録日を格納する。
        RecData.sinki_touroku = today_Renamed

        'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FilePut(fnum, RecData, recno)
        FileClose(fnum)

    End Sub

    Sub subBLOCKINFから各種構造のデータ算出(ByRef Kogo As String, ByRef KData As KOZO)
        '---------------------------------------------------------------------------------------------------
        '機能
        '   工号の下にあるﾌﾞﾛｯｸ情報ﾌｧｲﾙから構造と種別を読み込み、
        '   各種構造の台数（主桁、ﾃﾞｯｷ、横桁、ﾌﾞﾗｹｯﾄ、縦桁(中、側)）と有無（横構）を算出し、KOZO構造体に格納する。
        '引数
        '   kogo(I)：処理対象工号（ﾃﾞｨﾚｸﾄﾘ名）
        '   KDate(O)：各種構造ﾃﾞｰﾀ（構造体）
        '戻り値
        '   無し
        '---------------------------------------------------------------------------------------------------
        Dim fnum As Short
        Dim fname As String
        Dim kz, kind As String
        Dim buf, msg As String
        Dim strFileName As String

        strFileName = FunGetLanguage()

        '構造体を初期化する。
        KData.shuketa_daisuu = 0
        KData.deck_daisuu = 0
        KData.yokoketa_daisuu = 0
        KData.brket_daisuu = 0
        KData.tateketa_daisuu = 0
        KData.taikeikou_daisuu = 0
        KData.yokokou = 1

        'ﾌﾞﾛｯｸ情報ﾌｧｲﾙから構造と種別を読み込み、各種構造のﾃﾞｰﾀを算出する。
        fname = FunGetNewKoji() & Kogo & "\BLOCK.INF"
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(fname, FileAttribute.Normal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "ﾌﾞﾛｯｸ情報ﾌｧｲﾙ(BLOCK.INF)がありません。" & Chr(10)
                msg = msg & "部材一括処理が正常に終了していません。" & Chr(10)
                msg = msg & "このﾌｧｲﾙが無いと処理を継続出来ません。"
            Else
                msg = "There is no block information file (BLOCK.INF)." & Chr(10)
                msg = msg & "Component batch processing is not completed normally." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        fnum = FreeFile
        FileOpen(fnum, fname, OpenMode.Input)
        Do While Not EOF(fnum)
            buf = LineInput(fnum)

            kz = UCase(J_ChoiceString(buf, 2)) '構造
            kind = UCase(J_ChoiceString(buf, 7)) '種別

            If ((kz = "GRD") Or (kz = "KYAKU") Or (kz = "HARI")) And (kind = "BLK") Then '主桁台数
                KData.shuketa_daisuu = KData.shuketa_daisuu + 1
            ElseIf ((kz = "DECK") Or (kz = "GDK")) And (kind = "BLK") Then  'ﾃﾞｯｷ台数または底鋼板台数
                KData.deck_daisuu = KData.deck_daisuu + 1
            ElseIf (kz = "CRS") And (kind = "BLK") Then  '横桁台数
                KData.yokoketa_daisuu = KData.yokoketa_daisuu + 1
            ElseIf (kz = "BRK") And (kind = "BLK") Then  'ﾌﾞﾗｹｯﾄ台数
                KData.brket_daisuu = KData.brket_daisuu + 1
            ElseIf ((kz = "STR") Or (kz = "GST")) And (kind = "BLK") Then  '縦桁(中、側)台数または側鋼板台数
                KData.tateketa_daisuu = KData.tateketa_daisuu + 1
            ElseIf (kz = "SWY") And (kind = "BLK") Then  '対頃構台数
                KData.taikeikou_daisuu = KData.taikeikou_daisuu + 1
            ElseIf kz = "LATE" Then  '横構有無
                KData.yokokou = 2
            End If
        Loop
        FileClose(fnum)

    End Sub

    '2007/10/09 Nakagawa Add Start
    Sub subBLOCKINFから各種構造のデータ算出_ファイル名指定(ByRef Kogo As String, ByRef KData As KOZO, ByRef blockInfName As String)
        '---------------------------------------------------------------------------------------------------
        '機能
        '   工号の下にあるﾌﾞﾛｯｸ情報ﾌｧｲﾙから構造と種別を読み込み、
        '   各種構造の台数（主桁、ﾃﾞｯｷ、横桁、ﾌﾞﾗｹｯﾄ、縦桁(中、側)）と有無（横構）を算出し、KOZO構造体に格納する。
        '引数
        '   kogo(I)：処理対象工号（ﾃﾞｨﾚｸﾄﾘ名）
        '   KDate(O)：各種構造ﾃﾞｰﾀ（構造体）
        '戻り値
        '   無し
        '---------------------------------------------------------------------------------------------------
        Dim fnum As Short
        Dim fname As String
        Dim kz, kind As String
        Dim buf, msg As String
        Dim strFileName As String

        strFileName = FunGetLanguage()

        '構造体を初期化する。
        KData.shuketa_daisuu = 0
        KData.deck_daisuu = 0
        KData.yokoketa_daisuu = 0
        KData.brket_daisuu = 0
        KData.tateketa_daisuu = 0
        KData.taikeikou_daisuu = 0
        KData.yokokou = 1

        'ﾌﾞﾛｯｸ情報ﾌｧｲﾙから構造と種別を読み込み、各種構造のﾃﾞｰﾀを算出する。
        fname = FunGetNewKoji() & Kogo & "\" & blockInfName
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(fname, FileAttribute.Normal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "ﾌﾞﾛｯｸ情報ﾌｧｲﾙ(" & blockInfName & ")がありません。" & Chr(10)
                msg = msg & "部材一括処理が正常に終了していません。" & Chr(10)
                msg = msg & "このﾌｧｲﾙが無いと処理を継続出来ません。"
            Else
                msg = "There is no block information file (BLOCK.INF)." & Chr(10)
                msg = msg & "Component batch processing is not completed normally." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        fnum = FreeFile
        FileOpen(fnum, fname, OpenMode.Input)
        Do While Not EOF(fnum)
            buf = LineInput(fnum)

            kz = UCase(J_ChoiceString(buf, 2)) '構造
            kind = UCase(J_ChoiceString(buf, 7)) '種別

            If (kz = "GRD") And (kind = "BLK") Then '主桁台数
                KData.shuketa_daisuu = KData.shuketa_daisuu + 1
            ElseIf ((kz = "DECK") Or (kz = "GDK")) And (kind = "BLK") Then  'ﾃﾞｯｷ台数または底鋼板台数
                KData.deck_daisuu = KData.deck_daisuu + 1
            ElseIf (kz = "CRS") And (kind = "BLK") Then  '横桁台数
                KData.yokoketa_daisuu = KData.yokoketa_daisuu + 1
            ElseIf (kz = "BRK") And (kind = "BLK") Then  'ﾌﾞﾗｹｯﾄ台数
                KData.brket_daisuu = KData.brket_daisuu + 1
            ElseIf ((kz = "STR") Or (kz = "GST")) And (kind = "BLK") Then  '縦桁(中、側)台数または側鋼板台数
                KData.tateketa_daisuu = KData.tateketa_daisuu + 1
            ElseIf (kz = "SWY") And (kind = "BLK") Then  '対頃構台数
                KData.taikeikou_daisuu = KData.taikeikou_daisuu + 1
            ElseIf kz = "LATE" Then  '横構有無
                KData.yokokou = 2
            End If
        Loop
        FileClose(fnum)

    End Sub
    '2007/10/09 Nakagawa Add End

    Function funBLOCKINFとDWGファイルのチェック(ByRef Kogo As String) As Boolean
        '---------------------------------------------------------------------------------------------------
        '機能
        '   工号の下にあるﾌﾞﾛｯｸ情報ﾌｧｲﾙからﾌﾞﾛｯｸ外部名を読み込み、
        '   ﾌﾞﾛｯｸ外部名に対応したDWGﾌｧｲﾙがあるか判定する。
        '引数
        '   kogo(I)：処理対象工号（ﾃﾞｨﾚｸﾄﾘ名）
        '戻り値
        '   True：ﾌﾞﾛｯｸ情報ﾌｧｲﾙ内の全てのﾌﾞﾛｯｸ外部名に対してDWGﾌｧｲﾙが存在する。
        '   False：ﾌﾞﾛｯｸ情報ﾌｧｲﾙ内のﾌﾞﾛｯｸ外部名に対してDWGﾌｧｲﾙが存在しないものがある。
        '---------------------------------------------------------------------------------------------------
        Dim fnum, cnt As Short
        Dim fname, kogodir As String
        Dim blkname, dwgname As String
        Dim buf, msg As String
        Dim strFileName As String

        strFileName = FunGetLanguage()

        funBLOCKINFとDWGファイルのチェック = False
        kogodir = FunGetNewKoji() & Kogo

        'ﾌﾞﾛｯｸ情報ﾌｧｲﾙからﾌﾞﾛｯｸ外部名を読み込み、そのﾌﾞﾛｯｸ外部名のDWGﾌｧｲﾙを検索する。
        fname = kogodir & "\BLOCK.INF"
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(fname, FileAttribute.Normal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "ﾌﾞﾛｯｸ情報ﾌｧｲﾙ(BLOCK.INF)がありません。" & Chr(10)
                msg = msg & "部材一括処理が正常に終了していません。" & Chr(10)
                msg = msg & "このﾌｧｲﾙが無いと処理を継続出来ません。"
            Else
                msg = "There is no block information file (BLOCK.INF)." & Chr(10)
                msg = msg & "Component batch processing is not completed normally." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
            Exit Function
        End If

        fnum = FreeFile
        FileOpen(fnum, fname, OpenMode.Input)
        Do While Not EOF(fnum)
            buf = LineInput(fnum)

            blkname = J_ChoiceString(buf, 1) 'ﾌﾞﾛｯｸ外部名
            dwgname = kogodir & "\" & blkname & ".DBA"
            'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            If Dir(dwgname, FileAttribute.Normal) = "" Then
                Beep()
                If Not strFileName Like "*ENG.INI" Then
                    msg = "ﾌﾞﾛｯｸ情報ﾌｧｲﾙに記述されている" & blkname & "ﾌﾞﾛｯｸに対応したDWGﾌｧｲﾙがありません。"
                Else
                    msg = "There is no DWG file corresponding to " & blkname & "Brock described by the Brock information file."
                End If
                MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
                Exit Function
            End If
        Loop
        FileClose(fnum)

        funBLOCKINFとDWGファイルのチェック = True

    End Function

    Function funCHARGE台数のチェック(ByRef Kogo As String, ByRef intChgNum As Short, ByRef intBlkNum As Short) As Short
        '---------------------------------------------------------------------------------------------------
        '機能
        '   工号の下にあるﾌﾞﾛｯｸ情報ﾌｧｲﾙから算出した各種構造の台数（横構は有無）と、
        '   ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙの各種構造の台数（横構は有無）が合っているか判定する。
        '引数
        '   kogo(I)：処理対象工号（ﾃﾞｨﾚｸﾄﾘ名）
        '戻り値
        '   0：ﾁｬｰｼﾞｺｰﾄﾞがFREE-JUPITERのため、台数のﾁｪｯｸを行わない。または、各種構造の台数が合っている。
        '   1：ﾁｬｰｼﾞｺｰﾄﾞが登録ﾌｧｲﾙに登録されていない。
        '   2：主桁の台数が違う。
        '   3：ﾃﾞｯｷの台数が違う。
        '   4：横桁の台数が違う。
        '   5：ﾌﾞﾗｹｯﾄの台数が違う。
        '   6：縦桁の台数が違う。
        '   7：対傾構の台数が違う。
        '   8：横構の有無が違う。
        '---------------------------------------------------------------------------------------------------
        Dim fnum, recno As Short
        Dim fname, ProFile, WinDir As String
        Dim ans As New VB6.FixedLengthString(MAX_CHAR)
        Dim msg As String
        Dim strWindowsDirectoryBuffer As New VB6.FixedLengthString(260)
        Dim ret, tateketa As Integer
        Dim lngWindowsDirectoryLength As Integer
        'UPGRADE_WARNING: Arrays in structure RecData may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        '2017/07/28 Nakagawa Edit Start
        'Dim RecData As CHARGE
        Dim RecData As New CHARGE()
        '2017/07/28 Nakagawa Edit End
        Dim KozoData As KOZO
        Dim strFileName As String

        strFileName = FunGetLanguage()

        funCHARGE台数のチェック = 1

        'ﾌﾞﾛｯｸ情報ﾌｧｲﾙから各種構造の台数を算出する。
        Call subBLOCKINFから各種構造のデータ算出(Kogo, KozoData)

        '工事別ﾌﾟﾛﾌｧｲﾙから登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を読み込む。
        ProFile = FunGetNewKoji() & Kogo & "\PROFILE.INI"
        '登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を取得する。
        ret = GetPrivateProfileString("チャージコード", "登録ファイルレコード位置", "-999", ans.Value, MAX_CHAR, ProFile)
        recno = CShort(Left(ans.Value, InStr(ans.Value, Chr(0)) - 1))

        If recno < 0 Then '登録ﾌｧｲﾙに登録されていないためｴﾗｰ終了する。
            funCHARGE台数のチェック = 1
            Exit Function
        ElseIf recno = 0 Then  'ﾁｬｰｼﾞｺｰﾄﾞがFREE-JUPITERのため正常終了する。
            funCHARGE台数のチェック = 0
            Exit Function
        End If

        'Windowsﾃﾞｨﾚｸﾄﾘ（例 D:\WINNT）のﾊﾟｽ名を取得する。
        lngWindowsDirectoryLength = GetWindowsDirectory(strWindowsDirectoryBuffer.Value, Len(strWindowsDirectoryBuffer.Value))
        WinDir = Left(strWindowsDirectoryBuffer.Value, InStr(strWindowsDirectoryBuffer.Value, vbNullChar) - 1)

        'ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙ（CHARGE.INI）の登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を読み込み、各種構造の台数とﾌﾞﾛｯｸ情報ﾌｧｲﾙから算出した台数をﾁｪｯｸする。
        '<<<<2014/06/05 チャージファイルはc:Jupiter 直下に
        'fname = WinDir & "\CHARGE\CHARGE.INI"
        fname = get_CHARGE_FileName
        '>>>>2014/06/05 チャージファイルはc:Jupiter 直下に


        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(fname, FileAttribute.Normal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙ(CHARGE.INI)がありません。" & Chr(10)
                msg = msg & "このﾌｧｲﾙが無いと処理を継続出来ません。"
            Else
                msg = "There is no charge code registration file (CHARGE.INI)." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
            Exit Function
        End If

        fnum = FreeFile
        '2017/07/28 Nakagawa Edit Start
        'FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , Len(RecData))
        FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , FILE_SIZE)
        '2017/07/28 Nakagawa Edit End
        'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(fnum, RecData, recno)
        tateketa = RecData.nakatate_daisuu + RecData.sokutate_daisuu

        funCHARGE台数のチェック = 2

        If RecData.shuketa_daisuu = KozoData.shuketa_daisuu Then '主桁台数
            If RecData.deck_daisuu = KozoData.deck_daisuu Then 'ﾃﾞｯｷ台数
                If RecData.yokoketa_daisuu = KozoData.yokoketa_daisuu Then '横桁台数
                    If RecData.brket_daisuu = KozoData.brket_daisuu Then 'ﾌﾞﾗｹｯﾄ台数
                        If tateketa = KozoData.tateketa_daisuu Then '縦桁(中、側)台数
                            If RecData.taikeikou_daisuu = KozoData.taikeikou_daisuu Then '対頃構台数
                                If RecData.yokokou = KozoData.yokokou Then '横構有無
                                    funCHARGE台数のチェック = 0
                                Else
                                    funCHARGE台数のチェック = 8
                                End If
                            Else
                                intChgNum = RecData.taikeikou_daisuu
                                intBlkNum = KozoData.taikeikou_daisuu
                                funCHARGE台数のチェック = 7
                            End If
                        Else
                            intChgNum = tateketa
                            intBlkNum = KozoData.tateketa_daisuu
                            funCHARGE台数のチェック = 6
                        End If
                    Else
                        intChgNum = RecData.brket_daisuu
                        intBlkNum = KozoData.brket_daisuu
                        funCHARGE台数のチェック = 5
                    End If
                Else
                    intChgNum = RecData.yokoketa_daisuu
                    intBlkNum = KozoData.yokoketa_daisuu
                    funCHARGE台数のチェック = 4
                End If
            Else
                intChgNum = RecData.deck_daisuu
                intBlkNum = KozoData.deck_daisuu
                funCHARGE台数のチェック = 3
            End If
        Else
            intChgNum = RecData.shuketa_daisuu
            intBlkNum = KozoData.shuketa_daisuu
            funCHARGE台数のチェック = 2
        End If
        FileClose(fnum)

    End Function

    '2007/10/09 Nakagawa Add Start
    Function funCHARGE台数のチェック_ファイル名指定(ByRef Kogo As String, ByRef blockInfName As String) As Short
        '---------------------------------------------------------------------------------------------------
        '機能
        '   工号の下にあるﾌﾞﾛｯｸ情報ﾌｧｲﾙから算出した各種構造の台数（横構は有無）と、
        '   ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙの各種構造の台数（横構は有無）が合っているか判定する。
        '引数
        '   kogo(I)：処理対象工号（ﾃﾞｨﾚｸﾄﾘ名）
        '戻り値
        '   0：ﾁｬｰｼﾞｺｰﾄﾞがFREE-JUPITERのため、台数のﾁｪｯｸを行わない。または、各種構造の台数が合っている。
        '   1：ﾁｬｰｼﾞｺｰﾄﾞが登録ﾌｧｲﾙに登録されていない。
        '   2：主桁の台数が違う。
        '   3：ﾃﾞｯｷの台数が違う。
        '   4：横桁の台数が違う。
        '   5：ﾌﾞﾗｹｯﾄの台数が違う。
        '   6：縦桁の台数が違う。
        '   7：対傾構の台数が違う。
        '   8：横構の有無が違う。
        '---------------------------------------------------------------------------------------------------
        Dim fnum, recno As Short
        Dim fname, ProFile, WinDir As String
        Dim ans As New VB6.FixedLengthString(MAX_CHAR)
        Dim msg As String
        Dim strWindowsDirectoryBuffer As New VB6.FixedLengthString(260)
        Dim ret, tateketa As Integer
        Dim lngWindowsDirectoryLength As Integer
        'UPGRADE_WARNING: Arrays in structure RecData may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        '2017/07/28 Nakagawa Edit Start
        'Dim RecData As CHARGE
        Dim RecData As New CHARGE()
        '2017/07/28 Nakagawa Edit End
        Dim KozoData As KOZO
        Dim strFileName As String

        strFileName = FunGetLanguage()

        funCHARGE台数のチェック_ファイル名指定 = 1

        'ﾌﾞﾛｯｸ情報ﾌｧｲﾙから各種構造の台数を算出する。
        Call subBLOCKINFから各種構造のデータ算出_ファイル名指定(Kogo, KozoData, blockInfName)

        '工事別ﾌﾟﾛﾌｧｲﾙから登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を読み込む。
        ProFile = FunGetNewKoji() & Kogo & "\PROFILE.INI"
        '登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を取得する。
        ret = GetPrivateProfileString("チャージコード", "登録ファイルレコード位置", "-999", ans.Value, MAX_CHAR, ProFile)
        recno = CShort(Left(ans.Value, InStr(ans.Value, Chr(0)) - 1))

        If recno < 0 Then '登録ﾌｧｲﾙに登録されていないためｴﾗｰ終了する。
            funCHARGE台数のチェック_ファイル名指定 = 1
            Exit Function
        ElseIf recno = 0 Then  'ﾁｬｰｼﾞｺｰﾄﾞがFREE-JUPITERのため正常終了する。
            funCHARGE台数のチェック_ファイル名指定 = 0
            Exit Function
        End If

        'Windowsﾃﾞｨﾚｸﾄﾘ（例 D:\WINNT）のﾊﾟｽ名を取得する。
        lngWindowsDirectoryLength = GetWindowsDirectory(strWindowsDirectoryBuffer.Value, Len(strWindowsDirectoryBuffer.Value))
        WinDir = Left(strWindowsDirectoryBuffer.Value, InStr(strWindowsDirectoryBuffer.Value, vbNullChar) - 1)

        'ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙ（CHARGE.INI）の登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を読み込み、各種構造の台数とﾌﾞﾛｯｸ情報ﾌｧｲﾙから算出した台数をﾁｪｯｸする。
        '<<<<2014/06/05 チャージファイルはc:Jupiter 直下に
        'fname = WinDir & "\CHARGE\CHARGE.INI"
        fname = get_CHARGE_FileName
        '>>>>2014/06/05 チャージファイルはc:Jupiter 直下に



        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(fname, FileAttribute.Normal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙ(CHARGE.INI)がありません。" & Chr(10)
                msg = msg & "このﾌｧｲﾙが無いと処理を継続出来ません。"
            Else
                msg = "There is no charge code registration file (CHARGE.INI)." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
            Exit Function
        End If

        fnum = FreeFile()
        '2017/07/28 Nakagawa Edit Start
        'FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , Len(RecData))
        FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , FILE_SIZE)
        '2017/07/28 Nakagawa Edit End
        'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(fnum, RecData, recno)
        tateketa = RecData.nakatate_daisuu + RecData.sokutate_daisuu

        funCHARGE台数のチェック_ファイル名指定 = 2

        If RecData.shuketa_daisuu = KozoData.shuketa_daisuu Then '主桁台数
            If RecData.deck_daisuu = KozoData.deck_daisuu Then 'ﾃﾞｯｷ台数
                If RecData.yokoketa_daisuu = KozoData.yokoketa_daisuu Then '横桁台数
                    If RecData.brket_daisuu = KozoData.brket_daisuu Then 'ﾌﾞﾗｹｯﾄ台数
                        If tateketa = KozoData.tateketa_daisuu Then '縦桁(中、側)台数
                            If RecData.taikeikou_daisuu = KozoData.taikeikou_daisuu Then '対頃構台数
                                If RecData.yokokou = KozoData.yokokou Then '横構有無
                                    funCHARGE台数のチェック_ファイル名指定 = 0
                                Else
                                    funCHARGE台数のチェック_ファイル名指定 = 8
                                End If
                            Else
                                funCHARGE台数のチェック_ファイル名指定 = 7
                            End If
                        Else
                            funCHARGE台数のチェック_ファイル名指定 = 6
                        End If
                    Else
                        funCHARGE台数のチェック_ファイル名指定 = 5
                    End If
                Else
                    funCHARGE台数のチェック_ファイル名指定 = 4
                End If
            Else
                funCHARGE台数のチェック_ファイル名指定 = 3
            End If
        Else
            funCHARGE台数のチェック_ファイル名指定 = 2
        End If
        FileClose(fnum)

    End Function
    '2007/10/09 Nakagawa Add End

    Sub subCHARGE実行データの格納(ByRef Kogo As String, ByRef ftyp As Short)
        '---------------------------------------------------------------------------------------------------
        '機能
        '   工号の下にある工事別ﾌﾟﾛﾌｧｲﾙから登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を読み込み、
        '   ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙのその工事の骨組、部材、展開、分類の各処理の実行ﾃﾞｰﾀを更新する。
        '   展開、分類のときは、重量ﾛｸﾞﾌｧｲﾙから総重量を読み込む。
        '引数
        '   kogo(I)：処理対象工号（ﾃﾞｨﾚｸﾄﾘ名）
        '   ftyp(I)：実行ﾃﾞｰﾀ（実行回数、実行日、PMF重量）を格納する項目ﾀｲﾌﾟ
        '            1：骨組一括処理
        '            2：部材一括処理
        '            3：展開処理
        '            4：分類処理
        '戻り値
        '   無し
        '---------------------------------------------------------------------------------------------------
        Dim recno, fnum, cnt As Short
        Dim fname, ProFile, WinDir As String
        'UPGRADE_NOTE: today was upgraded to today_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
        Dim today_Renamed, pmfweight As String
        Dim ans As New VB6.FixedLengthString(MAX_CHAR)
        Dim msg As String
        Dim strWindowsDirectoryBuffer As New VB6.FixedLengthString(260)
        Dim ret As Integer
        Dim lngWindowsDirectoryLength As Integer
        'UPGRADE_WARNING: Arrays in structure RecData may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        '2017/07/28 Nakagawa Edit Start
        'Dim RecData As CHARGE
        Dim RecData As New CHARGE()
        '2017/07/28 Nakagawa Edit End
        Dim strFileName As String

        strFileName = FunGetLanguage()

        '工事別ﾌﾟﾛﾌｧｲﾙから登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を読み込む。
        ProFile = FunGetNewKoji() & Kogo & "\PROFILE.INI"
        '登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を取得する。
        ret = GetPrivateProfileString("チャージコード", "登録ファイルレコード位置", "-999", ans.Value, MAX_CHAR, ProFile)
        recno = CShort(Left(ans.Value, InStr(ans.Value, Chr(0)) - 1))

        If recno <= 0 Then
            Exit Sub
        End If

        'Windowsﾃﾞｨﾚｸﾄﾘ（例 D:\WINNT）のﾊﾟｽ名を取得する。
        lngWindowsDirectoryLength = GetWindowsDirectory(strWindowsDirectoryBuffer.Value, Len(strWindowsDirectoryBuffer.Value))
        WinDir = Left(strWindowsDirectoryBuffer.Value, InStr(strWindowsDirectoryBuffer.Value, vbNullChar) - 1)

        'ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙ（CHARGE.INI）の登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を読み込み、各処理の実行ﾃﾞｰﾀを更新し、書き込む。
        '<<<<2014/06/05 チャージファイルはc:Jupiter 直下に
        'fname = WinDir & "\CHARGE\CHARGE.INI"
        fname = get_CHARGE_FileName
        '>>>>2014/06/05 チャージファイルはc:Jupiter 直下に



        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(fname, FileAttribute.Normal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙ(CHARGE.INI)がありません。" & Chr(10)
                msg = msg & "このﾌｧｲﾙが無いと処理を継続出来ません。"
            Else
                msg = "There is no charge code registration file (CHARGE.INI)." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        fnum = FreeFile()
        '2017/07/28 Nakagawa Edit Start
        'FileOpen(fnum, fname, OpenMode.Random, OpenAccess.ReadWrite, , Len(RecData))
        FileOpen(fnum, fname, OpenMode.Random, OpenAccess.ReadWrite, , FILE_SIZE)
        '2017/07/28 Nakagawa Edit End
        'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(fnum, RecData, recno)

        '各処理の実行回数を更新する。
        If ftyp = 1 Then '骨組一括処理
            If RecData.skl_shori_kaisuu < 0 Then RecData.skl_shori_kaisuu = 0
            RecData.skl_shori_kaisuu = RecData.skl_shori_kaisuu + 1
            cnt = RecData.skl_shori_kaisuu
        ElseIf ftyp = 2 Then  '部材一括処理
            If RecData.buzai_shori_kaisuu < 0 Then RecData.buzai_shori_kaisuu = 0
            RecData.buzai_shori_kaisuu = RecData.buzai_shori_kaisuu + 1
            cnt = RecData.buzai_shori_kaisuu
        ElseIf ftyp = 3 Then  '展開処理
            If RecData.tenkai_shori_kaisuu < 0 Then RecData.tenkai_shori_kaisuu = 0
            RecData.tenkai_shori_kaisuu = RecData.tenkai_shori_kaisuu + 1
            cnt = RecData.tenkai_shori_kaisuu
        ElseIf ftyp = 4 Then  '分類処理
            If RecData.bunrui_shori_kaisuu < 0 Then RecData.bunrui_shori_kaisuu = 0
            RecData.bunrui_shori_kaisuu = RecData.bunrui_shori_kaisuu + 1
            cnt = RecData.bunrui_shori_kaisuu
        End If

        '日付と重量を格納する配列番号を算出する。
        cnt = cnt Mod DATE_MAX
        If cnt = 0 Then
            cnt = 10
        End If

        '現在の日付を取得する。
        today_Renamed = VB6.Format(Now, "yyyy/mm/dd")

        '展開処理または分類処理のとき、工事別ﾌﾟﾛﾌｧｲﾙから総重量を読み込む。
        If (ftyp = 3) Or (ftyp = 4) Then
            ret = GetPrivateProfileString("橋梁緒元", "総重量(Kg)", "NO-WEIGHT", ans.Value, MAX_CHAR, ProFile)
            pmfweight = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
            If pmfweight = "NO-WEIGHT" Then
                Beep()
                If Not strFileName Like "*ENG.INI" Then
                    msg = "重量の算出がされていないため、総重量が分かりません。" & Chr(10)
                    msg = msg & "総重量をｾﾞﾛとします。"
                Else
                    msg = "Since calculation of weight is not carried out, gross weight is not known." & Chr(10)
                    msg = msg & "Let gross weight be zero."
                End If
                MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
                pmfweight = "0.0"
            End If
        End If

        '各処理の実行日、PMF重量を格納する。
        If ftyp = 1 Then '骨組一括処理
            RecData.skl_shori_date(cnt).strdate = today_Renamed
        ElseIf ftyp = 2 Then  '部材一括処理
            RecData.buzai_shori_date(cnt).strdate = today_Renamed
        ElseIf ftyp = 3 Then  '展開処理
            RecData.tenkai_shori_date(cnt).strdate = today_Renamed
            RecData.tenkai_jyuuryou(cnt) = CDbl(pmfweight)
        ElseIf ftyp = 4 Then  '分類処理
            RecData.bunrui_shori_date(cnt).strdate = today_Renamed
            RecData.bunrui_jyuuryou(cnt) = CDbl(pmfweight)
        End If

        'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FilePut(fnum, RecData, recno)
        FileClose(fnum)

    End Sub

    Function funCHARGE橋梁形式の取得(ByRef Kogo As String) As Integer
        '---------------------------------------------------------------------------------------------------
        '機能
        '   工号の下にある工事別ﾌﾟﾛﾌｧｲﾙから登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を読み込み、
        '   ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙの橋梁形式を取得する。
        '引数
        '   kogo(I)：処理対象工号（ﾃﾞｨﾚｸﾄﾘ名）
        '戻り値
        '   処理ﾌﾗｸﾞ
        '      -1：ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙに登録されていない。
        '       0：工事別ﾌﾟﾛﾌｧｲﾙのﾁｬｰｼﾞｺｰﾄﾞが"FREE-JUPITER"で、全形式処理可能
        '       1：RC箱桁
        '       2：鋼床版箱桁
        '       3：RC鈑桁
        '       4：鋼床版鈑桁
        '---------------------------------------------------------------------------------------------------
        Dim fnum, recno As Short
        Dim fname, ProFile, WinDir As String
        Dim ans As New VB6.FixedLengthString(MAX_CHAR)
        Dim msg As String
        Dim strWindowsDirectoryBuffer As New VB6.FixedLengthString(260)
        Dim ret As Integer
        Dim lngWindowsDirectoryLength As Integer
        'UPGRADE_WARNING: Arrays in structure RecData may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        '2017/07/28 Nakagawa Edit Start
        'Dim RecData As CHARGE
        Dim RecData As New CHARGE()
        '2017/07/28 Nakagawa Edit End
        Dim strFileName As String

        strFileName = FunGetLanguage()

        funCHARGE橋梁形式の取得 = 1

        '工事別ﾌﾟﾛﾌｧｲﾙから登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を読み込む。
        ProFile = FunGetNewKoji() & Kogo & "\PROFILE.INI"
        '登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を取得する。
        ret = GetPrivateProfileString("チャージコード", "登録ファイルレコード位置", "-999", ans.Value, MAX_CHAR, ProFile)
        recno = CShort(Left(ans.Value, InStr(ans.Value, Chr(0)) - 1))

        If recno < 0 Then '登録ﾌｧｲﾙに登録されていないため不可とする。
            funCHARGE橋梁形式の取得 = -1
            Exit Function
        ElseIf recno = 0 Then  'ﾁｬｰｼﾞｺｰﾄﾞがFREE-JUPITERのため全形式とする。
            funCHARGE橋梁形式の取得 = 0
            Exit Function
        End If

        'Windowsﾃﾞｨﾚｸﾄﾘ（例 D:\WINNT）のﾊﾟｽ名を取得する。
        lngWindowsDirectoryLength = GetWindowsDirectory(strWindowsDirectoryBuffer.Value, Len(strWindowsDirectoryBuffer.Value))
        WinDir = Left(strWindowsDirectoryBuffer.Value, InStr(strWindowsDirectoryBuffer.Value, vbNullChar) - 1)

        'ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙ（CHARGE.INI）の登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を読み込み、骨組または展開処理ﾌﾗｸﾞを取得する。
        '<<<<2014/06/05 チャージファイルはc:Jupiter 直下に
        'fname = WinDir & "\CHARGE\CHARGE.INI"
        fname = get_CHARGE_FileName
        '>>>>2014/06/05 チャージファイルはc:Jupiter 直下に

        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(fname, FileAttribute.Normal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙ(CHARGE.INI)がありません。" & Chr(10)
                msg = msg & "このﾌｧｲﾙが無いと処理を継続出来ません。"
            Else
                msg = "There is no charge code registration file (CHARGE.INI)." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
            Exit Function
        End If

        fnum = FreeFile()
        '2017/07/28 Nakagawa Edit Start
        'FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , Len(RecData))
        FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , FILE_SIZE)
        '2017/07/28 Nakagawa Edit End
        'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(fnum, RecData, recno)

        funCHARGE橋梁形式の取得 = RecData.keisiki

        FileClose(fnum)

    End Function

    Function funCHARGE処理フラグの取得(ByRef Kogo As String, ByRef ftyp As Short) As Integer
        '---------------------------------------------------------------------------------------------------
        '機能
        '   工号の下にある工事別ﾌﾟﾛﾌｧｲﾙから登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を読み込み、
        '   ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙのその工事の骨組または展開の処理ﾌﾗｸﾞを取得する。
        '引数
        '   kogo(I)：処理対象工号（ﾃﾞｨﾚｸﾄﾘ名）
        '   ftyp(I)：処理ﾌﾗｸﾞを取得する項目ﾀｲﾌﾟ
        '            1：骨組一括処理
        '            2：展開処理
        '戻り値
        '   処理ﾌﾗｸﾞ
        '       1：不可
        '       2：可
        '---------------------------------------------------------------------------------------------------
        Dim fnum, recno As Short
        Dim fname, ProFile, WinDir As String
        Dim ans As New VB6.FixedLengthString(MAX_CHAR)
        Dim msg As String
        Dim strWindowsDirectoryBuffer As New VB6.FixedLengthString(260)
        Dim ret As Integer
        Dim lngWindowsDirectoryLength As Integer
        'UPGRADE_WARNING: Arrays in structure RecData may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        '2017/07/28 Nakagawa Edit Start
        'Dim RecData As CHARGE
        Dim RecData As New CHARGE()
        '2017/07/28 Nakagawa Edit End
        Dim strFileName As String

        strFileName = FunGetLanguage()

        funCHARGE処理フラグの取得 = 1

        '工事別ﾌﾟﾛﾌｧｲﾙから登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を読み込む。
        ProFile = FunGetNewKoji() & Kogo & "\PROFILE.INI"
        '登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を取得する。
        ret = GetPrivateProfileString("チャージコード", "登録ファイルレコード位置", "-999", ans.Value, MAX_CHAR, ProFile)
        recno = CShort(Left(ans.Value, InStr(ans.Value, Chr(0)) - 1))

        If recno < 0 Then '登録ﾌｧｲﾙに登録されていないため不可とする。
            funCHARGE処理フラグの取得 = 1
            Exit Function
        ElseIf recno = 0 Then  'ﾁｬｰｼﾞｺｰﾄﾞがFREE-JUPITERのため可とする。
            funCHARGE処理フラグの取得 = 2
            Exit Function
        End If

        'Windowsﾃﾞｨﾚｸﾄﾘ（例 D:\WINNT）のﾊﾟｽ名を取得する。
        lngWindowsDirectoryLength = GetWindowsDirectory(strWindowsDirectoryBuffer.Value, Len(strWindowsDirectoryBuffer.Value))
        WinDir = Left(strWindowsDirectoryBuffer.Value, InStr(strWindowsDirectoryBuffer.Value, vbNullChar) - 1)

        'ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙ（CHARGE.INI）の登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を読み込み、骨組または展開処理ﾌﾗｸﾞを取得する。
        '<<<<2014/06/05 チャージファイルはc:Jupiter 直下に
        'fname = WinDir & "\CHARGE\CHARGE.INI"
        fname = get_CHARGE_FileName
        '>>>>2014/06/05 チャージファイルはc:Jupiter 直下に

        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(fname, FileAttribute.Normal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙ(CHARGE.INI)がありません。" & Chr(10)
                msg = msg & "このﾌｧｲﾙが無いと処理を継続出来ません。"
            Else
                msg = "There is no charge code registration file (CHARGE.INI)." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
            Exit Function
        End If

        fnum = FreeFile()
        '2017/07/28 Nakagawa Edit Start
        'FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , Len(RecData))
        FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , FILE_SIZE)
        '2017/07/28 Nakagawa Edit End
        'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(fnum, RecData, recno)

        If ftyp = 1 Then '骨組一括処理
            funCHARGE処理フラグの取得 = RecData.skl_shori_flg
        ElseIf ftyp = 2 Then  '展開処理
            funCHARGE処理フラグの取得 = RecData.tenkai_flg
        End If

        FileClose(fnum)

    End Function

    Sub subCHARGE処理フラグの変更(ByRef Kogo As String, ByRef ftyp As Short, ByRef flg As Short)
        '---------------------------------------------------------------------------------------------------
        '機能
        '   工号の下にある工事別ﾌﾟﾛﾌｧｲﾙから登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を読み込み、
        '   ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙのその工事の骨組または展開の処理ﾌﾗｸﾞを変更する。
        '引数
        '   kogo(I)：処理対象工号（ﾃﾞｨﾚｸﾄﾘ名）
        '   ftyp(I)：処理ﾌﾗｸﾞを変更する項目ﾀｲﾌﾟ
        '            1：骨組一括処理
        '            2：展開処理
        '   flg(I) ：処理ﾌﾗｸﾞ
        '            1：不可
        '            2：可
        '戻り値
        '   無し
        '---------------------------------------------------------------------------------------------------
        Dim fnum, recno As Short
        Dim fname, ProFile, WinDir As String
        Dim ans As New VB6.FixedLengthString(MAX_CHAR)
        Dim msg As String
        Dim strWindowsDirectoryBuffer As New VB6.FixedLengthString(260)
        Dim ret As Integer
        Dim lngWindowsDirectoryLength As Integer
        'UPGRADE_WARNING: Arrays in structure RecData may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        '2017/07/28 Nakagawa Edit Start
        'Dim RecData As CHARGE
        Dim RecData As New CHARGE()
        '2017/07/28 Nakagawa Edit End
        Dim strFileName As String

        strFileName = FunGetLanguage()

        '工事別ﾌﾟﾛﾌｧｲﾙから登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を読み込む。
        ProFile = FunGetNewKoji() & Kogo & "\PROFILE.INI"
        '登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を取得する。
        ret = GetPrivateProfileString("チャージコード", "登録ファイルレコード位置", "-999", ans.Value, MAX_CHAR, ProFile)
        recno = CShort(Left(ans.Value, InStr(ans.Value, Chr(0)) - 1))

        If recno <= 0 Then
            Exit Sub
        End If

        'Windowsﾃﾞｨﾚｸﾄﾘ（例 D:\WINNT）のﾊﾟｽ名を取得する。
        lngWindowsDirectoryLength = GetWindowsDirectory(strWindowsDirectoryBuffer.Value, Len(strWindowsDirectoryBuffer.Value))
        WinDir = Left(strWindowsDirectoryBuffer.Value, InStr(strWindowsDirectoryBuffer.Value, vbNullChar) - 1)

        'ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙ（CHARGE.INI）の登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を読み込み、骨組または展開処理ﾌﾗｸﾞを変更し、書き込む。

        '<<<<2014/06/05 チャージファイルはc:Jupiter 直下に
        'fname = WinDir & "\CHARGE\CHARGE.INI"
        fname = get_CHARGE_FileName
        '>>>>2014/06/05 チャージファイルはc:Jupiter 直下に


        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(fname, FileAttribute.Normal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙ(CHARGE.INI)がありません。" & Chr(10)
                msg = msg & "このﾌｧｲﾙが無いと処理を継続出来ません。"
            Else
                msg = "There is no charge code registration file (CHARGE.INI)." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        fnum = FreeFile()
        '2017/07/28 Nakagawa Edit Start
        'FileOpen(fnum, fname, OpenMode.Random, OpenAccess.ReadWrite, , Len(RecData))
        FileOpen(fnum, fname, OpenMode.Random, OpenAccess.ReadWrite, , FILE_SIZE)
        '2017/07/28 Nakagawa Edit End
        'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(fnum, RecData, recno)

        If ftyp = 1 Then '骨組一括処理
            RecData.skl_shori_flg = flg
        ElseIf ftyp = 2 Then  '展開処理
            RecData.tenkai_flg = flg
        End If

        'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FilePut(fnum, RecData, recno)
        FileClose(fnum)

    End Sub

    Function funチャージコードの有無(ByRef Kogo As String, ByRef ccode As String) As Short
        '---------------------------------------------------------------------------------------------------
        '機能
        '   工号の下にある工事別ﾌﾟﾛﾌｧｲﾙからﾁｬｰｼﾞｺｰﾄﾞを読み込み、
        '   ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙに登録されているか判定する。
        '   工事別ﾌﾟﾛﾌｧｲﾙに登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を書き込む。
        '引数
        '   kogo(I)：処理対象工号（ﾃﾞｨﾚｸﾄﾘ名）
        '   ccode(O)：工事別ﾌﾟﾛﾌｧｲﾙから読み込んだﾁｬｰｼﾞｺｰﾄﾞ
        '戻り値
        '   -999：ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙに登録されていない。
        '   0：工事別ﾌﾟﾛﾌｧｲﾙのﾁｬｰｼﾞｺｰﾄﾞが"FREE-JUPITER"で、ﾁｬｰｼﾞｺｰﾄﾞをﾁｪｯｸしない（ﾚﾝﾀﾙ契約会社、社内、YBCのとき）
        '   1以上：ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙに登録されていて、かつその数値は登録位置のﾚｺｰﾄﾞ番号
        '---------------------------------------------------------------------------------------------------
        Dim fnum, reccnt As Short
        Dim WinDir, ProFile, fname, rcode As String
        Dim ans As New VB6.FixedLengthString(MAX_CHAR)
        Dim msg As String
        Dim strWindowsDirectoryBuffer As New VB6.FixedLengthString(260)
        Dim userName, SysFile As String
        Dim ret As Integer
        Dim lngWindowsDirectoryLength As Integer
        'UPGRADE_WARNING: Arrays in structure RecData may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        '2017/07/28 Nakagawa Edit Start
        'Dim RecData As CHARGE
        Dim RecData As New CHARGE()
        '2017/07/28 Nakagawa Edit End
        Dim strFileName As String

        strFileName = FunGetLanguage()

        funチャージコードの有無 = -999

        '工事別ﾌﾟﾛﾌｧｲﾙからﾁｬｰｼﾞｺｰﾄﾞを読み込む。
        ProFile = FunGetNewKoji() & Kogo & "\PROFILE.INI"


        'ﾁｬｰｼﾞｺｰﾄﾞを取得する。
        ret = GetPrivateProfileString("チャージコード", "チャージコード", "NO-CHARGE", ans.Value, MAX_CHAR, ProFile)
        ccode = Trim(UCase(Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)))

        '工事別ﾌﾟﾛﾌｧｲﾙにﾁｬｰｼﾞｺｰﾄﾞが記述されていない場合。
        If ccode = "NO-CHARGE" Then
            '工事別ﾌﾟﾛﾌｧｲﾙに登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を書き込む。
            ret = WritePrivateProfileString("チャージコード", "登録ファイルレコード位置", CStr(funチャージコードの有無), ProFile)
            Exit Function
        End If

        reccnt = 0

        'SYSTEM.TXTﾌｧｲﾙが無いとき、ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙとﾁｪｯｸは行わない。
        SysFile = FunGetSystemFile()

        If SysFile = "" Then
            '       funチャージコードの有無 = reccnt
            '       '工事別ﾌﾟﾛﾌｧｲﾙに登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を書き込む。
            '       ret = WritePrivateProfileString("チャージコード", "登録ファイルレコード位置", CStr(funチャージコードの有無), ProFile)
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "CastarJupiterﾌﾟﾛﾃｸｼｮﾝに問題があります。" & Chr(10)
                msg = msg & "ﾌﾟﾛﾃｸｼｮﾝの装着を確かめてください。" & Chr(10)
                msg = msg & "正しく装着されている場合は、発売元に連絡して下さい。" & Chr(10)
                msg = msg & "ﾌﾟﾛｸﾞﾗﾑを終了します。" & Chr(10)
            Else
                msg = "There is a problem in CastarJupiter protection." & Chr(10)
                msg = msg & "Please confirm wearing of protection." & Chr(10)
                msg = msg & "Please inform a distributor, when equipped correctly." & Chr(10)
                msg = msg & "A program is ended." & Chr(10)
            End If
            MsgBox(msg, MsgBoxStyle.OkOnly + MsgBoxStyle.Critical)
            End
            Exit Function
        End If
        'ﾁｬｰｼﾞｺｰﾄﾞがFREE-JUPITERでNKK、IHIのとき、ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙとﾁｪｯｸは行わない。
        Call subGetUserName(userName)

        '<<<<2014/06/04 YBGでもチャージを読む機能を追加
        '   ユーザーがYBG　　でFuncGetYBCFunction = "0"　の場合は、チャージなし"
        '    If (ccode = "FREE-JUPITER") And _
        ''       ((UCase(userName) = "NKK") Or (UCase(userName) = "IHI") Or _
        ''        (UCase(userName) = "YBG")) Then
        If (ccode = "FREE-JUPITER") And ((UCase(userName) = "NKK" And FuncGetOptionFunction() = "0") Or (UCase(userName) = "IHI") Or (UCase(userName) = "YBG" And FuncGetOptionFunction() = "0")) Then
            '>>>>2014/06/04 YBGでもチャージを読む機能を追加
            funチャージコードの有無 = reccnt
            '工事別ﾌﾟﾛﾌｧｲﾙに登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を書き込む。
            ret = WritePrivateProfileString("チャージコード", "登録ファイルレコード位置", CStr(funチャージコードの有無), ProFile)
            Exit Function
        End If

        'Windowsﾃﾞｨﾚｸﾄﾘ（例 D:\WINNT）のﾊﾟｽ名を取得する。
        lngWindowsDirectoryLength = GetWindowsDirectory(strWindowsDirectoryBuffer.Value, Len(strWindowsDirectoryBuffer.Value))
        WinDir = Left(strWindowsDirectoryBuffer.Value, InStr(strWindowsDirectoryBuffer.Value, vbNullChar) - 1)

        'ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙ（CHARGE.INI）を読み込みながら、この工事のﾁｬｰｼﾞｺｰﾄﾞが登録されているかのﾁｪｯｸする。
        '<<<<2014/06/05 チャージファイルはc:Jupiter 直下に
        'fname = WinDir & "\CHARGE\CHARGE.INI"
        fname = get_CHARGE_FileName()
        '>>>>2014/06/05 チャージファイルはc:Jupiter 直下に



        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(fname, FileAttribute.Normal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙ(CHARGE.INI)がありません。" & Chr(10)
                msg = msg & "このﾌｧｲﾙが無いと処理を継続出来ません。"
            Else
                msg = "There is no charge code registration file (CHARGE.INI)." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation)
            Exit Function
        End If

        fnum = FreeFile()
        '2017/07/28 Nakagawa Edit Start
        'FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , Len(RecData))
        FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , FILE_SIZE)
        '2017/07/28 Nakagawa Edit End
        Do While Not EOF(fnum)
            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            '2017/07/28 Nakagawa Edit Start
            'FileGet(fnum, RecData)
            'reccnt = reccnt + 1
            reccnt = reccnt + 1
            Try
                FileGet(fnum, RecData, reccnt)
            Catch ex As Exception
                Exit Do
            End Try
            '2017/07/28 Nakagawa Edit End

            If InStr(RecData.charge_code, Chr(0)) > 0 Then '2014/04/10 simo 落ちないように修正した。
                rcode = Trim(UCase(Left(RecData.charge_code, InStr(RecData.charge_code, Chr(0)) - 1)))
            Else
                '2014/04/10 simo 落ちないように修正した。
                rcode = Trim(UCase(RecData.charge_code))
            End If


            If ccode = rcode Then
                funチャージコードの有無 = reccnt
                Exit Do
            End If
        Loop
        FileClose(fnum)


        '工事別ﾌﾟﾛﾌｧｲﾙに登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を書き込む。
        ret = WritePrivateProfileString("チャージコード", "登録ファイルレコード位置", CStr(funチャージコードの有無), ProFile)

    End Function

    '2017/04/04 Nakagawa Add Start
    Public Function funチャージコードの有無_展開確認(Kogo As String, ccode As String) As Integer
        '---------------------------------------------------------------------------------------------------
        '機能
        '   工号の下にある工事別ﾌﾟﾛﾌｧｲﾙからﾁｬｰｼﾞｺｰﾄﾞを読み込み、
        '   ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙに登録されているか判定する。
        '   現在の日付とチャージファイルの終了日を比較し、現在 > 終了日の場合は-999を返す。
        '引数
        '   kogo(I)：処理対象工号（ﾃﾞｨﾚｸﾄﾘ名）
        '   ccode(IO)：工事別ﾌﾟﾛﾌｧｲﾙから読み込んだﾁｬｰｼﾞｺｰﾄﾞ
        '戻り値
        '   -999：現在 > 終了日
        '   0：工事別ﾌﾟﾛﾌｧｲﾙのﾁｬｰｼﾞｺｰﾄﾞが"FREE-JUPITER"で、ﾁｬｰｼﾞｺｰﾄﾞをﾁｪｯｸしない（ﾚﾝﾀﾙ契約会社、社内、YBCのとき）
        '   1以上：ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙに登録されていて、かつその数値は登録位置のﾚｺｰﾄﾞ番号
        '---------------------------------------------------------------------------------------------------
        Dim fnum As Integer, reccnt As Integer
        Dim ProFile As String, fname As String, rcode As String
        Dim ans As New VB6.FixedLengthString(MAX_CHAR)
        Dim msg As String
        Dim userName As String, SysFile As String
        Dim ret As Long
        Dim RecData As New CHARGE()
        Dim strFileName As String

        strFileName = FunGetLanguage()

        funチャージコードの有無_展開確認 = -999

        ProFile = FunGetNewKoji() & Kogo & "\PROFILE.INI"
        Dim isNewKoji As Boolean = False
        'ﾁｬｰｼﾞｺｰﾄﾞを取得する。
        If ccode = "" Then
            '工事別ﾌﾟﾛﾌｧｲﾙからﾁｬｰｼﾞｺｰﾄﾞを読み込む。
            ret = GetPrivateProfileString("チャージコード", "チャージコード", "NO-CHARGE", ans.Value, MAX_CHAR, ProFile)
            'ccode = Trim(UCase(Left(ans, InStr(ans, Chr(0)) - 1)))
            ccode = ans.ToString.Trim
            '工事別ﾌﾟﾛﾌｧｲﾙにﾁｬｰｼﾞｺｰﾄﾞが記述されていない場合。
            If ccode = "NO-CHARGE" Then
                '工事別ﾌﾟﾛﾌｧｲﾙに登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を書き込む。
                ret = WritePrivateProfileString("チャージコード", "登録ファイルレコード位置", CStr(funチャージコードの有無_展開確認), ProFile)
                Exit Function
            End If
            isNewKoji = True
        End If

        reccnt = 0

        'SYSTEM.TXTﾌｧｲﾙが無いとき、ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙとﾁｪｯｸは行わない。
        SysFile = FunGetSystemFile()

        If SysFile = "" Then
            '       funチャージコードの有無 = reccnt
            '       '工事別ﾌﾟﾛﾌｧｲﾙに登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を書き込む。
            '       ret = WritePrivateProfileString("チャージコード", "登録ファイルレコード位置", CStr(funチャージコードの有無), ProFile)
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "CastarJupiterﾌﾟﾛﾃｸｼｮﾝに問題があります。" & Chr(10)
                msg = msg & "ﾌﾟﾛﾃｸｼｮﾝの装着を確かめてください。" & Chr(10)
                msg = msg & "正しく装着されている場合は、発売元に連絡して下さい。" & Chr(10)
                msg = msg & "ﾌﾟﾛｸﾞﾗﾑを終了します。" & Chr(10)
            Else
                msg = "There is a problem in CastarJupiter protection." & Chr(10)
                msg = msg & "Please confirm wearing of protection." & Chr(10)
                msg = msg & "Please inform a distributor, when equipped correctly." & Chr(10)
                msg = msg & "A program is ended." & Chr(10)
            End If
            MsgBox(msg, vbOKOnly + vbCritical)
            End
            Exit Function
        End If
        'ﾁｬｰｼﾞｺｰﾄﾞがFREE-JUPITERでNKK、IHIのとき、ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙとﾁｪｯｸは行わない。
        Call subGetUserName(userName)

        '<<<<2014/06/04 YBGでもチャージを読む機能を追加
        '   ユーザーがYBG　　でFuncGetYBCFunction = "0"　の場合は、チャージなし"
        '    If (ccode = "FREE-JUPITER") And _
        '       ((UCase(userName) = "NKK") Or (UCase(userName) = "IHI") Or _
        '        (UCase(userName) = "YBG")) Then
        If (ccode = "FREE-JUPITER") And _
           ((UCase(userName) = "NKK" And FuncGetOptionFunction() = "0") Or (UCase(userName) = "IHI") Or _
            (UCase(userName) = "YBG" And FuncGetOptionFunction() = "0")) Then
            '>>>>2014/06/04 YBGでもチャージを読む機能を追加
            funチャージコードの有無_展開確認 = reccnt
            '工事別ﾌﾟﾛﾌｧｲﾙに登録ﾌｧｲﾙﾚｺｰﾄﾞ位置を書き込む。
            If isNewKoji = False Then
                ret = WritePrivateProfileString("チャージコード", "登録ファイルレコード位置", CStr(funチャージコードの有無_展開確認), ProFile)
            End If
            Exit Function
        End If

        'ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙ（CHARGE.INI）を読み込みながら、この工事のﾁｬｰｼﾞｺｰﾄﾞが登録されているかのﾁｪｯｸする。
        '<<<<2014/06/05 チャージファイルはc:Jupiter 直下に
        fname = get_CHARGE_FileName()
        '>>>>2014/06/05 チャージファイルはc:Jupiter 直下に

        If Dir(fname, vbNormal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "ﾁｬｰｼﾞｺｰﾄﾞ登録ﾌｧｲﾙ(CHARGE.INI)がありません。" & Chr(10)
                msg = msg & "このﾌｧｲﾙが無いと処理を継続出来ません。"
            Else
                msg = "There is no charge code registration file (CHARGE.INI)." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, vbOKOnly + vbExclamation)
            Exit Function
        End If

        Dim iLen As Integer = Len(RecData)
        fnum = FreeFile()
        FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , Len(RecData))
        Do While Not EOF(fnum)
            reccnt = reccnt + 1
            Try
                FileGet(fnum, RecData, reccnt)
            Catch ex As Exception
                Exit Do
            End Try

            If InStr(RecData.charge_code, Chr(0)) > 0 Then '2014/04/10 simo 落ちないように修正した。
                rcode = Trim(UCase(Left(RecData.charge_code, InStr(RecData.charge_code, Chr(0)) - 1)))
            Else
                '2014/04/10 simo 落ちないように修正した。
                rcode = Trim(UCase(RecData.charge_code))
            End If

            If ccode = rcode Then
                funチャージコードの有無_展開確認 = reccnt
                '現在の日付とチャージファイルの終了日を比較し、現在 > 終了日の場合は-999を返す。
                Dim dtNow As DateTime = DateTime.Now
                Dim stNow As String = dtNow.ToString("yyyy/MM/dd")
                Dim stEnd As String = RecData.shori_shuuryou
                Dim result As Integer = Date.Compare(stNow, stEnd)
                If result = 1 Then
                    funチャージコードの有無_展開確認 = -999
                End If
                Exit Do
            End If
        Loop
        FileClose(fnum)

    End Function
    '2017/04/04 Nakagawa Add End

    Function get_CHARGE_FileName() As String
        '2014/06/09 チャージファイルを返す関数
        '第一優先をc:\Jupiter\CHARGE\CHARGE.INI"
        '第二優先をC:\Windows\charge\CHARGE.INI"とする
        '
        Dim strWindowsDirectoryBuffer As New VB6.FixedLengthString(260)
        Dim lngWindowsDirectoryLength As Integer
        Dim WinDir As String
        Dim CHARGE_FileName As String
        CHARGE_FileName = FunGetGensun() & "CHARGE\CHARGE.INI"


        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(CHARGE_FileName, FileAttribute.Normal) = "" Then
            '第二優先をC:\Windows\charge\CHARGE.INI"とする いままでどおり
            'Windowsﾃﾞｨﾚｸﾄﾘ（例 D:\WINNT）のﾊﾟｽ名を取得する。
            lngWindowsDirectoryLength = GetWindowsDirectory(strWindowsDirectoryBuffer.Value, Len(strWindowsDirectoryBuffer.Value))
            WinDir = Left(strWindowsDirectoryBuffer.Value, InStr(strWindowsDirectoryBuffer.Value, vbNullChar) - 1)
            get_CHARGE_FileName = WinDir & "\CHARGE\CHARGE.INI"
        Else
            '第一優先 c:\Jupiter\CHARGE\CHARGE.INI"があった場合
            get_CHARGE_FileName = CHARGE_FileName

        End If
    End Function
End Module