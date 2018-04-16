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
	
	Private Declare Function ReadProtectData_JPT2 Lib "JptPrtct.dll" (ByVal SysId As Integer, ByVal SubSysId As Integer, ByRef opt As Integer, ByRef maxUse As Integer, ByRef inUse As Integer) As Integer
	'システム番号(99)、サブシステム番号より、バージョン番号、最大使用可能数、使用数を返す
	'result:正常終了==0/異常終了==その他
	
	'***************************************************************************************************
	'ﾕｰｻﾞｰ定義定数の宣言
	'***************************************************************************************************
	Public Const ProductSystemNo251 As Short = 251
	Public Const ProductSystemNo252 As Short = 252
	Public Const ProductSystemNo253 As Short = 253
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
		Dim maxUse, opt, inUse As Integer
		
		オプション使用の取得 = False
		
		If ReadProtectData_YTI(isys, ProductSubSystemNo0, lim, limcnt, usr, ver) = 1 Then
			オプション使用の取得 = True
			Exit Function
		End If
		
		If ReadProtectData_JPT2(JupiterSystemNo, isys, opt, maxUse, inUse) = 0 Then
			オプション使用の取得 = True
		End If
		
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
		Dim i, No As Short
		Dim lim, limcnt As Integer
		Dim usr, ver As Short
		Dim maxUse, opt, inUse As Integer
		
		断面使用の確認 = False
		
		'ｻﾌﾞｼｽﾃﾑ番号の最大値(1(RC箱桁)+2(鋼床版箱桁)+4(RC鈑桁)+8(汎用))
		No = 15
		
		For i = 0 To No
			If ReadProtectData_YTI(isys, i, lim, limcnt, usr, ver) = 1 Then
				If i = 0 Then
					'全ての断面が使用可能である。
					断面使用の確認 = True
				Else
					'使用可能な断面番号が加算されて格納されている。
					'RC箱桁とRC鈑桁が使用可能であれば、1(RC箱桁)+4(RC鈑桁)=5となる。
					If (i And isub) = isub Then
						断面使用の確認 = True
					End If
				End If
				Exit Function
			End If
		Next i
		
		If ReadProtectData_JPT2(JupiterSystemNo, isys, opt, maxUse, inUse) = 0 Then
			If opt = 0 Then
				断面使用の確認 = True
			Else
				If (opt And isub) = isub Then
					断面使用の確認 = True
				End If
			End If
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
		'戻り値
		'   TRUE：情報が書き込まれている(使用可能)。
		'   FALSE：情報が書き込まれていない、またはｾﾝﾁﾈﾙが異常(使用不可能)。
		'---------------------------------------------------------------------------------------------------
		Dim i, No As Short
		Dim lim, limcnt As Integer
		Dim usr, ver As Short
		Dim maxUse, opt, inUse As Integer
		
		Jupiterユーザー確認 = False
		
		'ｻﾌﾞｼｽﾃﾑ番号の最大値(1(RC箱桁)+2(鋼床版箱桁)+4(RC鈑桁)+8(汎用))
		No = 15
		
		For i = 0 To No
			If ReadProtectData_YTI(isys, i, lim, limcnt, usr, ver) = 1 Then
				osub = i
				Jupiterユーザー確認 = True
				Exit Function
			End If
		Next i
		
		If ReadProtectData_JPT2(JupiterSystemNo, isys, opt, maxUse, inUse) = 0 Then
			osub = opt
			Jupiterユーザー確認 = True
			Exit Function
		End If
		
	End Function
End Module