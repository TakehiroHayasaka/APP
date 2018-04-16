Option Strict Off
Option Explicit On
Module JSentinel
	
	'***************************************************************************************************
	' 使用しているDLLの外部ﾌﾟﾛｼｰｼﾞｬの宣言
	'***************************************************************************************************
	'***************************************************************************************************
	' ｾﾝﾁﾈﾙの情報読み込み
	'***************************************************************************************************
	Declare Function ReadProtectData_YTI Lib "ytiPrtct.dll" (ByVal No As Short, ByVal subno As Short, ByRef limdate As Integer, ByRef limcnt As Integer, ByRef usr As Short, ByRef ver As Short) As Short
	'ｼｽﾃﾑNo.ｻﾌﾞｼｽﾃﾑNo.で
	'使用期限日(シリアル値)、
	'制限カウンタ値、
	'ユーザタイプ、
	'バージョンNo.を返す
	'(result:正常終了==1/異常終了==0/ｵｰﾙﾏｲﾃｨがあった==2)
	'ｵｰﾙﾏｲﾃｨがあった場合、引数としては何も返さない。
	
	Declare Function EncodeDate_YTI Lib "ytiPrtct.dll" (ByVal yy As Short, ByVal mm As Short, ByVal dd As Short) As Integer
	'日付(YYMMDD)で、プロテクト用日付シリアル値を求める
	
	Declare Function SetLastAccessDate_YTI Lib "ytiPrtct.dll" (ByVal No As Short, ByVal subno As Short) As Short
	'ｼｽﾃﾑNo.ｻﾌﾞｼｽﾃﾑNo.で最後にアクセスされた日付をチェック（戻されていないか）
	'OKであれば現在の日付をセットする
	'result:異常終了==0/正常終了==1/ｵｰﾙﾏｲﾃｨ==2/日付が戻された==-1
	'ｵｰﾙﾏｲﾃｨがあった場合、日付は変更されない。
	
	Declare Function Dec_Counter_YTI Lib "ytiPrtct.dll" (ByVal No As Short, ByVal subno As Short) As Short
	'ｼｽﾃﾑNo.ｻﾌﾞｼｽﾃﾑNo.で指定されたレコードのカウンタを１減らす
	'result:異常終了==0/正常終了==1/ｵｰﾙﾏｲﾃｨ==2
	'ｵｰﾙﾏｲﾃｨがあった場合、カウンタは変更されない。
	
	Private Declare Function ReadProtectData_JPT2 Lib "JptPrtct.dll" (ByVal SysID As Integer, ByVal SubSysID As Integer, ByRef OPT As Integer, ByRef maxUse As Integer, ByRef inUse As Integer) As Integer
	Private Declare Function ReadProtectData_JPT2_PmfEdit Lib "JptPrtct.dll" (ByVal SysID As Integer, ByVal SubSysID As Integer, ByRef OPT As Integer, ByRef maxUse As Integer, ByRef inUse As Integer) As Integer
	'--ins '10.09.13 mimori st
	Private Declare Function ReadProtectData_JPT2_General Lib "JptPrtct.dll" (ByVal SysID As Integer, ByVal SubSysID As Integer, ByRef OPT As Integer, ByRef maxUse As Integer, ByRef inUse As Integer, ByVal LicenseFileGen As String) As Integer
	'--ins '10.09.13 mimori ed
	'システム番号(99)、サブシステム番号より、バージョン番号、最大使用可能数、使用数を返す
	'result:正常終了==0/異常終了==その他
	
	Private Declare Function RegOpenKeyEx Lib "ADVAPI32"  Alias "RegOpenKeyExA"(ByVal hKey As Integer, ByVal lpSubKey As String, ByVal ulOptions As Integer, ByVal samDesired As Integer, ByRef phkResult As Integer) As Integer
	'レジストリのキーを開く
	'result:正常終了==0/異常終了==その他
	
	Private Declare Function RegQueryValueExStr Lib "ADVAPI32"  Alias "RegQueryValueExA"(ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByVal lpType As Integer, ByVal lpData As String, ByRef lpcbData As Integer) As Integer
	'レジストリの値を取得する
	'result:正常終了==0/異常終了==その他
	
	Private Declare Function RegCloseKey Lib "ADVAPI32" (ByVal hKey As Integer) As Integer
	'レジストリのハンドルを開放する
	'result:正常終了==0/異常終了==その他
	
	Private Declare Function InitializeLicense_JPT Lib "JptPrtct.dll" () As Integer
	'--ins 10.09.16 mimori st
	Private Declare Function InitializeLicense_JPT_General Lib "JptPrtct.dll" (ByVal LicenseFileGen As String) As Integer
	'--ins 10.09.16 mimori ed
	'ネットワークライセンスの初期化を行う
	'result:正常終了==0/異常終了==その他
	
	Private Declare Function BorrowLicense_JPT Lib "JptPrtct.dll" (ByVal SubSysID As Integer, ByVal OPT As Integer) As Integer
	'ネットワークライセンスの取得(使用数+1)を行う
	'result:正常終了==0/異常終了==その他
	
	Public Declare Function GetLicense_JPT Lib "JptPrtct.dll" (ByVal SubSysID As Integer, ByRef OPT As Integer) As Integer
	'ネットワークライセンスのオプション(断面タイプ)を取得する
	'result:正常終了==0/異常終了==その他
	
	Private Declare Function ReturnLicense_JPT Lib "JptPrtct.dll" (ByVal SubSysID As Integer, ByVal OPT As Integer) As Integer
	'ネットワークライセンスの返却(使用数-1)を行う
	'result:正常終了==0/異常終了==その他
	
	Private Declare Function FinishLicense_JPT Lib "JptPrtct.dll" () As Integer
	'ネットワークライセンスの終了処理を行う
	'result:正常終了==0/異常終了==その他
	
	Private Declare Function GetUses_JPT Lib "JptPrtct.dll" (ByVal SubSysID As Integer, ByVal OPT As Integer, ByRef maxUse As Integer, ByRef inUse As Integer) As Integer
	'--ins 10.09.13 mimori st
	Private Declare Function GetUses_JPT_General Lib "JptPrtct.dll" (ByVal SubSysID As Integer, ByVal OPT As Integer, ByRef maxUse As Integer, ByRef inUse As Integer, ByVal LicenseFileName As String) As Integer
	'--ins 10.09.13 mimori ed
	'ネットワークライセンスの最大使用数、現在の使用数を取得する
	'result:正常終了==0/異常終了==その他
	
	''2012/03/15
	'Private Declare Function casCatchOption Lib "casProtectLib.dll" _
	''                ( _
	''                    ByVal SysID As Integer, ByVal SubSysID As Integer _
	''                ) As Long
	''使用可能な桁の構造形式を取得する。
	''result:使用可能な桁の構造形式
	
	'***************************************************************************************************
	'ﾕｰｻﾞｰ定義定数の宣言
	'***************************************************************************************************
	Public Const ProductSystemNo251 As Short = 251
	Public Const ProductSystemNo252 As Short = 252
	Public Const ProductSystemNo253 As Short = 253
	Public Const ProductSystemNo254 As Short = 254
	Public Const ProductSubSystemNo0 As Short = 0
	Public Const ProductSubSystemNo1 As Short = 1
	Public Const ProductSubSystemNo2 As Short = 2
	Public Const ProductSubSystemNo4 As Short = 4
	Public Const ProductSubSystemNo8 As Short = 8
	Public Const ProductSubSystemNo16 As Short = 16
	
	Public Const JupiterSystemNo As Short = 99
	
	Public Const TypRcB As Short = 1 'RC箱桁のｾﾝﾁﾈﾙ番号
	Public Const TypDkB As Short = 2 '鋼床版箱桁のｾﾝﾁﾈﾙ番号
	Public Const TypRcI As Short = 4 'RC鈑桁のｾﾝﾁﾈﾙ番号
	Public Const TypGen As Short = 8 '汎用のｾﾝﾁﾈﾙ番号
	Public Const TypPier As Short = 16 '鋼製脚のｾﾝﾁﾈﾙ番号
	Public Const TypPSlb As Short = 32 '合成床版のｾﾝﾁﾈﾙ番号　2012/03/20 simo 追加
    Public Const TypWWeb As Short = 64 '波形ウェブのｾﾝﾁﾈﾙ番号　2015/09/15 Nakagawa 追加
    Public Const TypSeg As Short = 128 '鋼製セグメントのｾﾝﾁﾈﾙ番号　2015/09/15 Nakagawa 追加

	Function オプション使用の取得(ByRef isys As Short) As Boolean
		'---------------------------------------------------------------------------------------------------
		'機能
		'   ｾﾝﾁﾈﾙのCastarJupiterの情報(ｼｽﾃﾑ番号=isys)が書き込まれている(使用可能)か確認する。
		'引数
		'   isys(I)：使用可能であるｵﾌﾟｼｮﾝに対応した番号
		'           253：会話処理が使用可能
		'戻り値
		'   TRUE：情報が取得出来た。
		'   FALSE：情報が取得出来なかった。
		'---------------------------------------------------------------------------------------------------
        Dim lim, limcnt As Integer
        Dim usr, ver As Short
        Dim maxUse, OPT, inUse As Integer

        'mod by xiaoyun.z 20140408 sta
        'オプション使用の取得 = True
		
        オプション使用の取得 = False

        If ライセンスタイプの取得() = 3 Then
            '旧センチネル版
            'If ReadProtectData_YTI(isys, ProductSubSystemNo0, lim, limcnt, usr, ver) = 1 Then
            '    オプション使用の取得 = True
            '    Exit Function
            'End If

            If ReadProtectData_JPT2(JupiterSystemNo, isys, OPT, maxUse, inUse) = 0 Then
                If maxUse > 0 Then '2012/10/04 simo
                    オプション使用の取得 = True
                End If '2012/10/04 simo
            End If
        Else
            'ネットワーク対応版
            If GetUses_JPT(isys, OPT, maxUse, inUse) = 0 Then
                If maxUse <> 0 And maxUse >= inUse Then
                    オプション使用の取得 = True
                End If
            End If
        End If
        'mod by xiaoyun.z 20140408 end
		
	End Function
	
	Function オプション機能の取得(ByRef isys As Short) As Boolean
		'---------------------------------------------------------------------------------------------------
		'機能
		'   ｾﾝﾁﾈﾙのCastarJupiterの情報(ｼｽﾃﾑ番号=isys)が書き込まれている(使用可能)か確認する。
		'引数
		'   isys(I)：使用可能であるｵﾌﾟｼｮﾝに対応した番号
		'           253：会話処理が使用可能
		'戻り値
		'   TRUE：情報が取得出来た。
		'   FALSE：情報が取得出来なかった。
		'---------------------------------------------------------------------------------------------------
        'Dim lim As Long, limcnt As Long
        'Dim usr As Integer, ver As Integer
        Dim OPT As Long, maxUse As Long, inUse As Long

        'mod by xiaoyun.z 20150408 sta
        'オプション機能の取得 = True
		
        オプション機能の取得 = False

        Dim LicenseFileName As String = ""
        If isys = 42 Or isys = 102 Then LicenseFileName = "FlgKegaki.LIC"

        If ライセンスタイプの取得() = 3 Then
            If ReadProtectData_JPT2_General(JupiterSystemNo, isys, OPT, maxUse, inUse, LicenseFileName) = 0 Then
                オプション機能の取得 = True
            End If
        Else
            If GetUses_JPT_General(isys, OPT, maxUse, inUse, LicenseFileName) = 0 Then
                オプション機能の取得 = True
            End If
        End If
        'mod by xiaoyun.z 20150408 end
		
	End Function
	
	
	Function 断面使用の確認(ByRef isys As Short, ByRef isub As Short) As Boolean
		'---------------------------------------------------------------------------------------------------
		'機能
		'   ﾏﾈｰｼﾞｬｰ実行(新規工事、既存工事)時の断面(RC箱桁、鋼床版箱桁、RC鈑桁 他)が、
		'   ｾﾝﾁﾈﾙのCastarJupiterの情報(ｼｽﾃﾑ番号=isys、ｻﾌﾞｼｽﾃﾑ番号=isub)に書き込まれている(使用可能)か確認する。
		'引数
		'   isys(I)：JupiterﾏﾈｰｼﾞｬｰかJupiter入力ｼｽﾃﾑかの別
		'            251：Jupiterﾏﾈｰｼﾞｬｰ
		'            252：Jupiter入力ｼｽﾃﾑ
		'   isub(I)：選択した断面に対応した番号、または、この組み合わせ
		'            1：RC箱桁
		'            2：鋼床版箱桁
		'            4：RC鈑桁
		'            8：汎用
		'戻り値
		'   TRUE：情報が書き込まれている(使用可能)。
		'   FALSE：情報が書き込まれていない、またはｾﾝﾁﾈﾙが異常(使用不可能)。
		'---------------------------------------------------------------------------------------------------
        Dim i As Integer, No As Integer
        Dim lim As Long, limcnt As Long
        Dim usr As Integer, ver As Integer
        Dim OPT As Long, maxUse As Long, inUse As Long

        '断面使用の確認 = True
        断面使用の確認 = False

        If ライセンスタイプの取得() = 3 Then
            '旧センチネル版
            'ｻﾌﾞｼｽﾃﾑ番号の最大値(1(RC箱桁)+2(鋼床版箱桁)+4(RC鈑桁)+8(汎用))
            'No = 15

            'For i = 0 To No
            '    If ReadProtectData_YTI(isys, i, lim, limcnt, usr, ver) = 1 Then
            '        If i = 0 Then
            '            '全ての断面が使用可能である。
            '            断面使用の確認 = True
            '        Else
            '            '使用可能な断面番号が加算されて格納されている。
            '            'RC箱桁とRC鈑桁が使用可能であれば、1(RC箱桁)+4(RC鈑桁)=5となる。
            '            If (i And isub) = isub Then
            '                断面使用の確認 = True
            '            End If
            '        End If
            '        Exit Function
            '    End If
            'Next i

            If ReadProtectData_JPT2(JupiterSystemNo, isys, OPT, maxUse, inUse) = 0 Then
                If OPT = 0 Then
                    断面使用の確認 = True
                Else
                    If (OPT And isub) = isub Then
                        断面使用の確認 = True
                    End If
                End If
            End If
        Else
            'ネットワーク対応版
            OPT = isub
            断面使用の確認 = False
            If GetLicense_JPT(isys, OPT) = 0 Then
                断面使用の確認 = True
            End If

            '        If GetLicense_JPT(isys, opt) = 0 Then
            '            If opt = 0 Then
            '                '<<<<2012/03/15 simo
            '                '断面使用の確認 = True
            '                断面使用の確認 = False
            '                '>>>>2012/03/15 simo
            '            Else
            '                If (opt And isub) = isub Then
            '                    断面使用の確認 = True
            '                End If
            '            End If
            '        End If
        End If
		
	End Function
	
	Function Jupiterユーザー確認(ByRef isys As Short, ByRef osub As Short) As Boolean
		'---------------------------------------------------------------------------------------------------
		'機能
		'   ｾﾝﾁﾈﾙにCastarJupiterの情報(ｼｽﾃﾑ番号=isys、ｻﾌﾞｼｽﾃﾑ番号=*)が書き込まれているか確認する。
		'   ｻﾌﾞｼｽﾃﾑ番号は、0:全形式、1:RC箱桁、2:鋼床版箱桁、4:RC鈑桁、およびこの組み合わせが書き込まれている。
		'   Jupiterﾏﾈｰｼﾞｬｰの場合、各ｱﾌﾟﾘのﾀｲﾏｰｺﾝﾄﾛｰﾙで常に確認する。
		'引数
		'   isys(I)：JupiterﾏﾈｰｼﾞｬｰかJupiter入力ｼｽﾃﾑかの別
		'            251：Jupiterﾏﾈｰｼﾞｬｰ
		'            252：Jupiter入力ｼｽﾃﾑ
		'   osub(O)：登録されている断面に対応した番号、または、この組み合わせ
		'            1：RC箱桁
		'            2：鋼床版箱桁
		'            4：RC鈑桁
        '            8：汎用
        '           
		'戻り値
		'   TRUE：情報が書き込まれている(使用可能)。
		'   FALSE：情報が書き込まれていない、またはｾﾝﾁﾈﾙが異常(使用不可能)。
		'---------------------------------------------------------------------------------------------------
        Dim i As Integer, No As Integer
        Dim lim As Long, limcnt As Long
        Dim usr As Integer, ver As Integer
        Dim OPT As Long, maxUse As Long, inUse As Long

        'Jupiterユーザー確認 = True
        Jupiterユーザー確認 = False

        If ライセンスタイプの取得() = 3 Then
            '旧センチネル版
            'ｻﾌﾞｼｽﾃﾑ番号の最大値(1(RC箱桁)+2(鋼床版箱桁)+4(RC鈑桁)+8(汎用))
            'No = 15

            'For i = 0 To No
            '    If ReadProtectData_YTI(isys, i, lim, limcnt, usr, ver) = 1 Then
            '        osub = i
            '        Jupiterユーザー確認 = True
            '        Exit Function
            '    End If
            'Next i

            If ReadProtectData_JPT2(JupiterSystemNo, isys, OPT, maxUse, inUse) = 0 Then
                osub = OPT
                Jupiterユーザー確認 = True
            End If
        Else
            'ネットワーク対応版
            '<<<<2012/03/16 simo GetLicense_JPT の仕様変更のため
            If isys = 251 Or isys = 252 Then
                osub = 0
                If GetLicense_JPT(isys, 1) = 0 Then osub = osub + 1
                If GetLicense_JPT(isys, 2) = 0 Then osub = osub + 2
                If GetLicense_JPT(isys, 4) = 0 Then osub = osub + 4
                If GetLicense_JPT(isys, 8) = 0 Then osub = osub + 8
                If GetLicense_JPT(isys, 16) = 0 Then osub = osub + 16
                If GetLicense_JPT(isys, 32) = 0 Then osub = osub + 32
                If GetLicense_JPT(isys, 64) = 0 Then osub = osub + 64
                If osub <> 0 Then
                    Jupiterユーザー確認 = True
                End If
                '<<<<2012/03/16 simo GetLicense_JPT の仕様変更のため
            ElseIf GetLicense_JPT(isys, OPT) = 0 Then
                osub = OPT
                Jupiterユーザー確認 = True
            End If
        End If
		
	End Function
	
	Function ライセンスタイプの取得() As Integer
		'---------------------------------------------------------------------------------------------------
		'機能
		'   レジストリからライセンスのタイプを取得する。
		'引数
		'   なし
		'戻り値
		'   0:スタンドアローン版
		'   1:ネットワーク版サーバー
		'   2:ネットワーク版クライアント
		'   3:旧センチネル版
		'---------------------------------------------------------------------------------------------------
		Dim hKey As Integer
		Dim valuedata As String
		Dim length As Integer
		
		ライセンスタイプの取得 = 3
		
		'    'USBﾌﾗｯｼｭﾒﾓﾘ(ｽﾀﾝﾄﾞｱﾛﾝ版)が存在する場合「ﾗｲｾﾝｽﾀｲﾌﾟ=3」と同じ処理をする。
		'    If InitializeLicense_JPT <> 0 Then
		
		'HKEY_LOCAL_MACHINEのSoftware\Yti\Jupiterからライセンス情報を取得する
        If RegOpenKeyEx(&H80000002, "Software\Yti\Jupiter", 0, 1, hKey) <> 0 Then
            Exit Function
        End If
		
		valuedata = New String(Chr(0), 250)
		length = Len(valuedata)
		
		If RegQueryValueExStr(hKey, "LicenseType", 0, 0, valuedata, length) <> 0 Then
            RegCloseKey(hKey)
			Exit Function
		End If
		
		ライセンスタイプの取得 = Val(valuedata)
		
		'    'USBﾌﾗｯｼｭﾒﾓﾘ(ｽﾀﾝﾄﾞｱﾛﾝ版)が存在する場合「ﾗｲｾﾝｽﾀｲﾌﾟ=3」と同じ処理をする。
		'    End If
		
        RegCloseKey(hKey)
	End Function
	
	Function ネットワークライセンスの初期化() As Boolean
		'---------------------------------------------------------------------------------------------------
		'機能
		'   ネットワークライセンスの初期化を行う。
		'   ネットワーク版サーバー、クライアントはライセンスモニターで行うため、スタンドアローン版のみ行う。
		'引数
		'   なし
		'戻り値
		'   TRUE：初期化に成功した
		'   FALSE：初期化に失敗した
		'---------------------------------------------------------------------------------------------------
		ネットワークライセンスの初期化 = False
		
		If ライセンスタイプの取得() = 0 Then
			If InitializeLicense_JPT() = 0 Then
				ネットワークライセンスの初期化 = True
			End If
			Call InitializeLicense_JPT_General("FlgKegaki.lic")
		Else
			ネットワークライセンスの初期化 = True
		End If
	End Function
	
	Function ネットワークライセンスの終了() As Boolean
		'---------------------------------------------------------------------------------------------------
		'機能
		'   ネットワークライセンスの終了処理を行う。
		'   ネットワーク版サーバー、クライアントはライセンスモニターで行うため、スタンドアローン版のみ行う。
		'引数
		'   なし
		'戻り値
		'   TRUE：終了処理に成功した
		'   FALSE：終了処理に失敗した
		'---------------------------------------------------------------------------------------------------
		ネットワークライセンスの終了 = False
		
		If ライセンスタイプの取得() = 0 Then
			If FinishLicense_JPT() = 0 Then
				ネットワークライセンスの終了 = True
			End If
		Else
			ネットワークライセンスの終了 = True
		End If
	End Function
	
	Function ネットワークライセンスの取得(ByRef isys As Short, ByRef isub As Short) As Boolean
		'---------------------------------------------------------------------------------------------------
		'機能
		'   ネットワークライセンスの取得(使用数+1)を行う。
		'引数
		'   isys(I)：ライセンスのシステム番号
		'            251：Jupiterマネージャー
		'            252：Jupiter入力システム
		'            253：会話処理
		'   isub(I)：断面タイプ
		'            1：RC箱桁
		'            2：鋼床版箱桁
		'            4：RC鈑桁
		'            8：汎用
		'戻り値
		'   TRUE：取得に成功した
		'   FALSE：取得に失敗した
		'---------------------------------------------------------------------------------------------------
		ネットワークライセンスの取得 = False
		
		If BorrowLicense_JPT(isys, isub) = 0 Then
			ネットワークライセンスの取得 = True
		End If
	End Function
	
	Function ネットワークライセンスの返却(ByRef isys As Short, ByRef isub As Short) As Boolean
		'---------------------------------------------------------------------------------------------------
		'機能
		'   ネットワークライセンスの返却(使用数-1)を行う。
		'引数
		'   isys(I)：ライセンスのシステム番号
		'            251：Jupiterマネージャー
		'            252：Jupiter入力システム
		'            253：会話処理
		'   isub(I)：断面タイプ
		'            1：RC箱桁
		'            2：鋼床版箱桁
		'            4：RC鈑桁
		'            8：汎用
		'戻り値
		'   TRUE：返却に成功した
		'   FALSE：返却に失敗した
		'---------------------------------------------------------------------------------------------------
		ネットワークライセンスの返却 = False
		
		If ReturnLicense_JPT(isys, isub) = 0 Then
			ネットワークライセンスの返却 = True
		End If
	End Function
	
	'Function 桁形式(isys As Integer, isub As Integer) As Boolean
	''---------------------------------------------------------------------------------------------------
	''機能
	''   桁形式の使用可否を判定する。
	''引数
	''   isys(I)：ライセンスのシステム番号
	''            251：Jupiterマネージャー
	''            252：Jupiter入力システム
	''            253：会話処理
	''   isub(I)：断面タイプ
	''            1：RC箱桁
	''            2：鋼床版箱桁
	''            4：RC鈑桁
	''            8：汎用
	''戻り値
	''   TRUE ：使用可
	''   FALSE：使用不可
	''---------------------------------------------------------------------------------------------------
	'    桁形式 = False
	'
	'    If casCatchOption() = 0 Then
	'        桁形式 = True
	'    End If
	'End Function
End Module