Option Strict Off
Option Explicit On
Module JJmIni
	
	'***************************************************************************************************
	'使用しているWindowsAPIの宣言
	'***************************************************************************************************
	'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
    Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
	
	'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
	'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
    Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Integer
	
	'***************************************************************************************************
	'ﾕｰｻﾞｰ定義定数の宣言
    '***************************************************************************************************
    'mod by xiaoyun.z 20150408 sta
    'Private Const MAX_CHAR As Short = 256 '最大文字数
    Private Const MAX_CHAR As Integer = 256 '最大文字数
    'mod by xiaoyun.z 20150408 end
	
	Function FunGetAcad() As String
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIに設定されているACADのﾌﾙﾊﾟｽ名を取得する。
		'引数
		'   無し
		'戻り値
		'   ACADのﾌﾙﾊﾟｽ名。
		'   設定されていないとき、またはｱﾌﾟﾘｹｰｼｮﾝが無いとき、""を返す。
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim PathName As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString("外部アプリケーション", "ACAD", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		PathName = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If PathName = "NO_KEY" Then
			FunGetAcad = ""
		Else
			On Error GoTo NO_DRIVE
			
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If Dir(PathName, FileAttribute.Normal) = "" Then
				FunGetAcad = ""
			Else
				FunGetAcad = PathName
			End If
		End If
		
		Exit Function
		
NO_DRIVE: 
		FunGetAcad = ""
		
	End Function
	Function FunGetCadWindowTitle() As String
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIに設定されているCADのWindiwTitleを取得する。
		'引数
		'   無し
		'戻り値
		'   CADのWindowTitle名。
		'   設定されていないとき、またはｱﾌﾟﾘｹｰｼｮﾝが無いとき、"AutoCAD"を返す。
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim cadWindowTitle As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString("インストールタイプ", "CadWindowTitle", "AutoCAD", ans.Value, MAX_CHAR, IniFile)
		cadWindowTitle = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		FunGetCadWindowTitle = cadWindowTitle
		
		Exit Function
	End Function
	Function FunGetJconsole() As String
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIに設定されているJconsole=ON/OFF(省略時)を取得する。
		'引数
		'   無し
		'戻り値
		'   JCONSOLE版で実行するか否か。
		'   設定されていないとき、"OFF"を返す。
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim jconsole As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString("インストールタイプ", "JCONSOLE", "OFF", ans.Value, MAX_CHAR, IniFile)
		jconsole = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		FunGetJconsole = jconsole
		
		Exit Function
	End Function
	
	Function FunGetRegEditProfiles() As String
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIに設定されているRegEditのProfilesパスを取得する。
		'引数
		'   無し
		'戻り値
		'   RegEditのProfilesパス
		'   設定されていないとき、またはｱﾌﾟﾘｹｰｼｮﾝが無いとき、""を返す。
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim regEditProfiles As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString("インストールタイプ", "RegEditProfiles", "", ans.Value, MAX_CHAR, IniFile)
		regEditProfiles = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		FunGetRegEditProfiles = regEditProfiles
		
		Exit Function
	End Function
	
	
	Function FunGetAutoCadVersion() As String
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIに設定されているAutoCADのバージョンを取得する。
		'引数
		'   無し
		'戻り値
		'   AutoCADのバージョン
		'   設定されていないとき、またはｱﾌﾟﾘｹｰｼｮﾝが無いとき、""を返す。
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim autoCadVersion As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString("インストールタイプ", "AutoCAD", "", ans.Value, MAX_CHAR, IniFile)
		autoCadVersion = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		FunGetAutoCadVersion = autoCadVersion
		
		Exit Function
		
	End Function
	
	Function FunGetAccess() As String
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIに設定されているACCESSのﾌﾙﾊﾟｽ名を取得する。
		'引数
		'   無し
		'戻り値
		'   ACCESSのﾌﾙﾊﾟｽ名。
		'   設定されていないとき、またはｱﾌﾟﾘｹｰｼｮﾝが無いとき、""を返す。
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim PathName As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString("外部アプリケーション", "ACCESS", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		PathName = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If PathName = "NO_KEY" Then
			FunGetAccess = ""
		Else
			On Error GoTo NO_DRIVE
			
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If Dir(PathName, FileAttribute.Normal) = "" Then
				FunGetAccess = ""
			Else
				FunGetAccess = PathName
			End If
		End If
		
		Exit Function
		
NO_DRIVE: 
		FunGetAccess = ""
		
	End Function
	
	Function FunGetActiveData() As String
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIに設定されている[ACTIVE_DATA]の工事名を取得する。
		'引数
		'   無し
		'戻り値
		'   [ACTIVE_DATA]の工事名。
		'   設定されていないとき、""を返す。
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim PathName As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString("ACTIVE_DATA", "KOGO", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		PathName = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If PathName = "NO_KEY" Then
			FunGetActiveData = ""
		Else
			FunGetActiveData = PathName
		End If
		
	End Function
	
	Function FunGetApp(ByRef AppName As String) As String
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIに設定されているAPP[*]の内、AppNameに該当するｱﾌﾟﾘｹｰｼｮﾝのﾌﾙﾊﾟｽ名を取得する。
		'引数
		'   AppName(I)：ｱﾌﾟﾘｹｰｼｮﾝのｷｰﾜｰﾄﾞ
		'戻り値
		'   AppNameのﾌﾙﾊﾟｽ名。
		'   設定されていないとき、またはｱﾌﾟﾘｹｰｼｮﾝが無いとき、""を返す。
		'---------------------------------------------------------------------------------------------------
		Dim i, pos As Short
		Dim AppCnt As Short
		Dim PathName, AppKey As String
		
		FunGetApp = ""
		
		AppCnt = FunGetAppCnt()
		
		For i = 0 To AppCnt - 1
			PathName = FunGetAppNo(i)
			
			pos = InStr(PathName, ":")
			If pos > 0 Then
				AppKey = Left(PathName, pos - 1)
				If UCase(AppKey) = UCase(AppName) Then
					If fun文字列抽出(PathName, "[", "]", PathName) = True Then
						On Error GoTo NO_DRIVE
						
						'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
						If Dir(PathName, FileAttribute.Normal) = "" Then
							FunGetApp = ""
						Else
							FunGetApp = PathName
						End If
						Exit For
					End If
				End If
			End If
		Next i
		
		Exit Function
		
NO_DRIVE: 
		FunGetApp = ""
		
	End Function
	
	Function FunGetAppCnt() As Short
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIに設定されているｱﾌﾟﾘｹｰｼｮﾝの設定数を取得する。
		'引数
		'   無し
		'戻り値
		'   AppCountのﾃﾞｰﾀ。
		'   設定されていないとき、0を返す。
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim Data As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString("外部アプリケーション", "AppCount", "0", ans.Value, MAX_CHAR, IniFile)
		Data = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		FunGetAppCnt = CShort(Data)
		
	End Function
	
	Function FunGetAppNo(ByRef No As Short) As String
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIに設定されているｱﾌﾟﾘｹｰｼｮﾝの設定を取得する。
		'引数
		'   No(I)：ｱﾌﾟﾘｹｰｼｮﾝの設定番号
		'戻り値
		'   ｱﾌﾟﾘｹｰｼｮﾝのｷｰとﾌﾙﾊﾟｽ名。
		'   設定されていないとき、""を返す。
		'---------------------------------------------------------------------------------------------------
		Dim IniFile, AppNo As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim PathName As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		AppNo = "App[" & No & "]"
		
		ret = GetPrivateProfileString("外部アプリケーション", AppNo, "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		PathName = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If PathName = "NO_KEY" Then
			FunGetAppNo = ""
		Else
			FunGetAppNo = PathName
		End If
		
	End Function
	
	Function FunGetBin() As String
		'---------------------------------------------------------------------------------------------------
		'機能
		'   ｱﾌﾟﾘｹｰｼｮﾝが格納されているBINﾃﾞｨﾚｸﾄﾘを設定する。
		'引数
		'   無し
		'戻り値
		'   BINのﾌﾙﾊﾟｽ名。
		'   ﾃﾞｨﾚｸﾄﾘが無いとき、""を返す。
		'---------------------------------------------------------------------------------------------------
		Dim PathName As String
		
		PathName = FunGetGensun() & "BIN\"
		
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(PathName, FileAttribute.Directory) = "" Then
			FunGetBin = ""
		Else
			FunGetBin = PathName
		End If
		
	End Function
	
	Function FunGetCopyListFile() As String
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIに設定されている[CopyListFile]のﾃﾞｰﾀを取得する。
		'   このﾃﾞｰﾀは、新規工事を作成するときｶﾚﾝﾄﾃﾞｨﾚｸﾄﾘにｺﾋﾟｰするﾌｧｲﾙ名が記述してある。
		'引数
		'   無し
		'戻り値
		'   [CopyListFile]のﾌﾙﾊﾟｽ名。
		'   設定されていないとき、またはﾌｧｲﾙが無いとき、""を返す。
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim Data, PathName As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString("CopyListFile", "FileName", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		Data = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If Data = "NO_KEY" Then
			FunGetCopyListFile = ""
		Else
			PathName = FunGetGensun() & Data
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If Dir(PathName, FileAttribute.Normal) = "" Then
				FunGetCopyListFile = ""
			Else
				FunGetCopyListFile = PathName
			End If
		End If
		
	End Function
	
	Function FunGetDcl() As String
		'---------------------------------------------------------------------------------------------------
		'機能
		'   AutoCADのﾀﾞｲｱﾛｸﾞﾌｧｲﾙが格納されているDCLﾃﾞｨﾚｸﾄﾘを設定する。
		'引数
		'   無し
		'戻り値
		'   DCLのﾌﾙﾊﾟｽ名。
		'   ﾃﾞｨﾚｸﾄﾘが無いとき、""を返す。
		'---------------------------------------------------------------------------------------------------
		Dim PathName As String
		
		PathName = FunGetGensun() & "DCL\"
		
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(PathName, FileAttribute.Directory) = "" Then
			FunGetDcl = ""
		Else
			FunGetDcl = PathName
		End If
		
	End Function
	
	Function FunGetEnviron() As String
		'---------------------------------------------------------------------------------------------------
		'機能
		'   設定ﾌｧｲﾙが格納されているENVIRONﾃﾞｨﾚｸﾄﾘを設定する。
		'引数
		'   無し
		'戻り値
		'   ENVIRONのﾌﾙﾊﾟｽ名。
		'   ﾃﾞｨﾚｸﾄﾘが無いとき、""を返す。
		'---------------------------------------------------------------------------------------------------
		Dim PathName As String
		
		PathName = FunGetGensun() & "ENVIRON\"
		
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(PathName, FileAttribute.Directory) = "" Then
			FunGetEnviron = ""
		Else
			FunGetEnviron = PathName
		End If
		
	End Function
	
	Function FunGetExcel() As String
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIに設定されているEXCELのﾌﾙﾊﾟｽ名を取得する。
		'引数
		'   無し
		'戻り値
		'   EXCELのﾌﾙﾊﾟｽ名。
		'   設定されていないとき、またはｱﾌﾟﾘｹｰｼｮﾝが無いとき、""を返す。
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim PathName As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString("外部アプリケーション", "EXCEL", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		PathName = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If PathName = "NO_KEY" Then
			FunGetExcel = ""
		Else
			On Error GoTo NO_DRIVE
			
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If Dir(PathName, FileAttribute.Normal) = "" Then
				FunGetExcel = ""
			Else
				FunGetExcel = PathName
			End If
		End If
		
		Exit Function
		
NO_DRIVE: 
		FunGetExcel = ""
		
	End Function
	
	Function FunGetForm() As String
		'---------------------------------------------------------------------------------------------------
		'機能
		'   帳票書式ﾌｧｲﾙが格納されているFORMﾃﾞｨﾚｸﾄﾘを設定する。
		'引数
		'   無し
		'戻り値
		'   FORMのﾌﾙﾊﾟｽ名。
		'   ﾃﾞｨﾚｸﾄﾘが無いとき、""を返す。
		'---------------------------------------------------------------------------------------------------
		Dim PathName As String
		
		PathName = FunGetGensun() & "FORM\"
		
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(PathName, FileAttribute.Directory) = "" Then
			FunGetForm = ""
		Else
			FunGetForm = PathName
		End If
		
	End Function
	
	Function FunGetGensun() As String
		'---------------------------------------------------------------------------------------------------
		'機能
		'   環境変数GENSUN_DIRを取得する。ﾊﾟｽ名の最後に\が無い場合は追加する。
		'引数
		'   無し
		'戻り値
		'   GENSUN_DIRのﾊﾟｽ名。
		'   設定されていないとき、またはﾃﾞｨﾚｸﾄﾘが無いとき、""を返す。
		'---------------------------------------------------------------------------------------------------
		Dim PathName As String
		
		PathName = Environ("GENSUN_DIR")
		
		If PathName = "" Then
			FunGetGensun = ""
		Else
			If Mid(PathName, Len(PathName), 1) <> "\" Then
				PathName = PathName & "\"
			End If
			
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If Dir(PathName, FileAttribute.Directory) = "" Then
				FunGetGensun = ""
			Else
				FunGetGensun = PathName
			End If
		End If
		
	End Function
	
	Function FunGetIcon() As String
		'---------------------------------------------------------------------------------------------------
		'機能
		'   ｱｲｺﾝﾌｧｲﾙが格納されているICONﾃﾞｨﾚｸﾄﾘを設定する。
		'引数
		'   無し
		'戻り値
		'   ICONのﾌﾙﾊﾟｽ名。
		'   ﾃﾞｨﾚｸﾄﾘが無いとき、""を返す。
		'---------------------------------------------------------------------------------------------------
		Dim PathName As String
		
		PathName = FunGetGensun() & "ICON\"
		
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(PathName, FileAttribute.Directory) = "" Then
			FunGetIcon = ""
		Else
			FunGetIcon = PathName
		End If
		
	End Function
	
	Function FunGetJlog() As String
		'---------------------------------------------------------------------------------------------------
		'機能
		'   操作履歴ﾌｧｲﾙが格納されているJLOGﾃﾞｨﾚｸﾄﾘを設定する。
		'引数
		'   無し
		'戻り値
		'   JLOGのﾌﾙﾊﾟｽ名。
		'   ﾃﾞｨﾚｸﾄﾘが無いとき、""を返す。
		'---------------------------------------------------------------------------------------------------
		Dim PathName As String
		
		PathName = FunGetGensun() & "JLOG\"
		
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(PathName, FileAttribute.Directory) = "" Then
			FunGetJlog = ""
		Else
			FunGetJlog = PathName
		End If
		
	End Function
	
	Function FunGetKoji() As String
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIに設定されている工事のﾌﾙﾊﾟｽ名を取得する。
		'   工事ﾃﾞｨﾚｸﾄﾘは、[新規工事]と[ACTIVE_DATA]を合わせたﾊﾟｽ名となる。
		'引数
		'   無し
		'戻り値
		'   工事のﾌﾙﾊﾟｽ名。
		'   設定されていないとき、またはﾃﾞｨﾚｸﾄﾘが無いとき、""を返す。
		'---------------------------------------------------------------------------------------------------
		Dim PathName, KogoName As String
		
		PathName = FunGetNewKoji()
		If PathName = "" Then
			FunGetKoji = ""
			Exit Function
		End If
		
		KogoName = FunGetActiveData()
		If KogoName = "" Then
			FunGetKoji = ""
		Else
			PathName = PathName & KogoName & "\"
			
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If Dir(PathName, FileAttribute.Directory) = "" Then
				FunGetKoji = ""
			Else
				FunGetKoji = PathName
			End If
		End If
		
	End Function
	
	Function FunGetMenu() As String
		'---------------------------------------------------------------------------------------------------
		'機能
		'   ﾒﾆｭｰﾌｧｲﾙが格納されているMENUﾃﾞｨﾚｸﾄﾘを設定する。
		'引数
		'   無し
		'戻り値
		'   MENUのﾌﾙﾊﾟｽ名。
		'   ﾃﾞｨﾚｸﾄﾘが無いとき、""を返す。
		'---------------------------------------------------------------------------------------------------
		Dim PathName As String
		
		PathName = FunGetGensun() & "MENU\"
		
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(PathName, FileAttribute.Directory) = "" Then
			FunGetMenu = ""
		Else
			FunGetMenu = PathName
		End If
		
	End Function
	
	Function FunGetNewKoji() As String
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIに設定されている[新規工事]のﾊﾟｽ名を取得する。
		'引数
		'   無し
		'戻り値
		'   [新規工事]のﾊﾟｽ名。
		'   設定されていないとき、またはﾃﾞｨﾚｸﾄﾘが無いとき、""を返す。
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim PathName As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString("新規工事", "PATH", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		PathName = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If PathName = "NO_KEY" Then
			FunGetNewKoji = ""
		Else
			If Mid(PathName, Len(PathName), 1) <> "\" Then
				PathName = PathName & "\"
			End If
			
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If Dir(PathName, FileAttribute.Directory) = "" Then
				FunGetNewKoji = ""
			Else
				FunGetNewKoji = PathName
			End If
		End If
		
	End Function

    ' 2015/0/916 Nakagawa Add Start
    Function FunGetNewKoji2() As String
        '---------------------------------------------------------------------------------------------------
        '機能
        '   JMANAGE.INIに設定されている[新規工事]のﾊﾟｽ名を取得する。
        '引数
        '   無し
        '戻り値
        '   [新規工事]のﾊﾟｽ名。
        '   設定されていないとき""を返す。(工事フォルダ内にフォルダが一つもなくてもよい
        '---------------------------------------------------------------------------------------------------
        Dim IniFile As String
        Dim ans As New VB6.FixedLengthString(MAX_CHAR)
        Dim PathName As String
        Dim ret As Integer

        IniFile = FunGetEnviron() & "JMANAGE.INI"

        ret = GetPrivateProfileString("新規工事", "PATH", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
        PathName = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)

        If PathName = "NO_KEY" Then
            FunGetNewKoji2 = ""
        Else
            If Mid(PathName, Len(PathName), 1) <> "\" Then
                PathName = PathName & "\"
            End If

            'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FunGetNewKoji2 = PathName
        End If

    End Function
    ' 2015/0/916 Nakagawa Add End

	Function FunGetSystemFile() As String
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIに設定されている[SystemFile]のﾃﾞｰﾀを取得する。
		'   このﾃﾞｰﾀは、ﾌｧｲﾙの有無によりｾﾝﾁﾈﾙﾛｯｸを判断するかを決定する。
		'引数
		'   無し
		'戻り値
		'   [SystemFile]のﾌﾙﾊﾟｽ名。
		'   設定されていないとき、またはﾌｧｲﾙが無いとき、""を返す。
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim Data, PathName As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString("SystemFile", "FileName", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		Data = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If Data = "NO_KEY" Then
			FunGetSystemFile = ""
		Else
			PathName = FunGetGensun() & Data
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If Dir(PathName, FileAttribute.Normal) = "" Then
				FunGetSystemFile = ""
			Else
				FunGetSystemFile = PathName
			End If
		End If
		
	End Function
	
	
	Function FunGetWaitingTime() As Short
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIに設定されている[WaitingTime]のﾃﾞｰﾀを取得する。
		'   このﾃﾞｰﾀは、Jupiterﾏﾈｰｼﾞｬｰと同時にAutoCADを実行するときのAutoCADの待ち時間を設定してある。
		'引数
		'   無し
		'戻り値
		'   [WaitingTime]のﾃﾞｰﾀ。
		'   設定されていないとき、0を返す。
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim Data As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString("WaitingTime", "SEC", "0", ans.Value, MAX_CHAR, IniFile)
		Data = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		FunGetWaitingTime = CShort(Data)
		
	End Function
	
	Sub subGetFtp(ByRef SendOn As String, ByRef User As String, ByRef Pass As String)
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIに設定されている[FTP]の転送先ｱﾄﾞﾚｽ、ﾕｰｻﾞｰ名、ﾊﾟｽﾜｰﾄﾞを取得する。
		'引数
		'   SendOn(O)：転送先ｱﾄﾞﾚｽ
		'   User(O)：ﾕｰｻﾞｰ名
		'   Pass(O)：ﾊﾟｽﾜｰﾄﾞ
		'戻り値
		'   無し
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim Data As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		'転送先ｱﾄﾞﾚｽ
		ret = GetPrivateProfileString("FTP", "SendOn", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		Data = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If Data = "NO_KEY" Then
			SendOn = ""
		Else
			SendOn = Data
		End If
		
		'ﾕｰｻﾞｰ名
		ret = GetPrivateProfileString("FTP", "User", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		Data = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If Data = "NO_KEY" Then
			User = ""
		Else
			User = Data
		End If
		
		'ﾊﾟｽﾜｰﾄﾞ
		ret = GetPrivateProfileString("FTP", "Pass", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		Data = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If Data = "NO_KEY" Then
			Pass = ""
		Else
			Pass = Data
		End If
		
	End Sub
	
	Sub subSetActiveData(ByRef Kogo As String)
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIの[ACTIVE_DATA]に工事名を設定する。
		'引数
		'   Kogo(I)：工事名
		'戻り値
		'   無し
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		'工事名
		ret = WritePrivateProfileString("ACTIVE_DATA", "KOGO", Kogo, IniFile)
		
	End Sub
	
	Sub subSetFtp(ByRef User As String, ByRef Pass As String)
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIの[FTP]にﾕｰｻﾞｰ名、ﾊﾟｽﾜｰﾄﾞを設定する。
		'引数
		'   User(I)：ﾕｰｻﾞｰ名
		'   Pass(I)：ﾊﾟｽﾜｰﾄﾞ
		'戻り値
		'   無し
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		'ﾕｰｻﾞｰ名
		ret = WritePrivateProfileString("FTP", "User", User, IniFile)
		
		'ﾊﾟｽﾜｰﾄﾞ
		ret = WritePrivateProfileString("FTP", "Pass", Pass, IniFile)
		
	End Sub
	
	Sub subSetReportFile(ByRef fname As String)
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIの[帳票]に選択ﾌｧｲﾙを設定する。
		'引数
		'   Fname(I)：選択ﾌｧｲﾙ
		'戻り値
		'   無し
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		'選択ﾌｧｲﾙ
		ret = WritePrivateProfileString("帳票", "選択ﾌｧｲﾙ", fname, IniFile)
		
	End Sub
	
	Sub subSetReportKanri(ByRef Kanri As String)
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIの[帳票]に選択管理名を設定する。
		'引数
		'   Kanri(I)：選択管理名
		'戻り値
		'   無し
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		'選択管理名
		ret = WritePrivateProfileString("帳票", "選択管理名", Kanri, IniFile)
		
	End Sub
	
	Sub subSetReportPaper(ByRef Size As String, ByRef Orientation As String)
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIの[帳票]にﾍﾟｰﾊﾟｰｻｲｽﾞ、縦横向きを設定する。
		'引数
		'   Size(I)：ﾍﾟｰﾊﾟｰｻｲｽﾞ
		'   Orientation(I)：縦横向き
		'戻り値
		'   無し
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		'ﾍﾟｰﾊﾟｰｻｲｽﾞ
		ret = WritePrivateProfileString("帳票", "PaperSize", Size, IniFile)
		
		'縦横向き
		ret = WritePrivateProfileString("帳票", "PaperOrientation", Orientation, IniFile)
		
	End Sub
	
	Sub subSetReportType(ByRef Rtype As String)
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIの[帳票]に帳票ﾀｲﾌﾟを設定する。
		'引数
		'   Rtype(I)：帳票ﾀｲﾌﾟ
		'戻り値
		'   無し
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		'帳票ﾀｲﾌﾟ
		ret = WritePrivateProfileString("帳票", "選択帳票", Rtype, IniFile)
		
	End Sub
	
	Sub subSetPaperSize(ByRef Size As String)
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIの[SystemPrinter]にﾍﾟｰﾊﾟｰｻｲｽﾞを設定する。
		'引数
		'   Size(I)：ﾍﾟｰﾊﾟｰｻｲｽﾞ
		'戻り値
		'   無し
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		'ﾍﾟｰﾊﾟｰｻｲｽﾞ
		ret = WritePrivateProfileString("SystemPrinter", "PaperSize", Size, IniFile)
		
	End Sub
	
	Sub subGetPaperSize(ByRef Size As String)
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIの[SystemPrinter]のﾍﾟｰﾊﾟｰｻｲｽﾞを取得する。
		'引数
		'   Size(O)：ﾍﾟｰﾊﾟｰｻｲｽﾞ
		'戻り値
		'   無し
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim Data As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		'ﾌﾟﾘﾝﾀｰの用紙ｻｲｽﾞ
		ret = GetPrivateProfileString("SystemPrinter", "PaperSize", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		Data = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If Data = "NO_KEY" Then
			Size = "A4"
		Else
			Size = Data
		End If
		
	End Sub
	
	Sub subGetUserName(ByRef userName As String)
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIに設定されているﾕｰｻﾞ名を取得する。
		'引数
		'   UserName(O)：ﾕｰｻﾞ名
		'戻り値
		'   無し
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim Data As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		'ﾕｰｻﾞ名
		ret = GetPrivateProfileString("ユーザ名", "ユーザ名", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		Data = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If Data = "NO_KEY" Then
			userName = ""
		Else
			userName = Data
		End If
		
	End Sub
	
	Function FunGetCasClient() As String
		'---------------------------------------------------------------------------------------------------
		'機能
		'   環境変数CASTAR_CLIENTを取得する。ﾊﾟｽ名の最後に\が無い場合は追加する。
		'引数
		'   無し
		'戻り値
		'   CASTAR_CLIENTのﾊﾟｽ名。
		'   設定されていないとき、またはﾃﾞｨﾚｸﾄﾘが無いとき、""を返す。
		'---------------------------------------------------------------------------------------------------
		Dim PathName As String
		
		PathName = Environ("CASTAR_CLIENT")
		
		If PathName = "" Then
			FunGetCasClient = ""
		Else
			If Mid(PathName, Len(PathName), 1) <> "\" Then
				PathName = PathName & "\"
			End If
			
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If Dir(PathName, FileAttribute.Directory) = "" Then
				FunGetCasClient = ""
			Else
				FunGetCasClient = PathName
			End If
		End If
		
	End Function
	
	Function FunGetCasUser() As String
		'---------------------------------------------------------------------------------------------------
		'機能
		'   環境変数CASTAR_USERを取得する。ﾊﾟｽ名の最後に\が無い場合は追加する。
		'引数
		'   無し
		'戻り値
		'   CASTAR_USERのﾊﾟｽ名。
		'   設定されていないとき、またはﾃﾞｨﾚｸﾄﾘが無いとき、""を返す。
		'---------------------------------------------------------------------------------------------------
		Dim PathName As String
		
		PathName = Environ("CASTAR_USER")
		
		If PathName = "" Then
			FunGetCasUser = ""
		Else
			If Mid(PathName, Len(PathName), 1) <> "\" Then
				PathName = PathName & "\"
			End If
			
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If Dir(PathName, FileAttribute.Directory) = "" Then
				FunGetCasUser = ""
			Else
				FunGetCasUser = PathName
			End If
		End If
		
	End Function
	
	Function FunGetCasBin() As String
		'---------------------------------------------------------------------------------------------------
		'機能
		'   CASTARｱﾌﾟﾘｹｰｼｮﾝが格納されているBINﾃﾞｨﾚｸﾄﾘを設定する。
		'引数
		'   無し
		'戻り値
		'   BINのﾌﾙﾊﾟｽ名。
		'   ﾃﾞｨﾚｸﾄﾘが無いとき、""を返す。
		'---------------------------------------------------------------------------------------------------
		Dim PathName As String
		
		PathName = FunGetCasClient() & "BIN\"
		
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(PathName, FileAttribute.Directory) = "" Then
			FunGetCasBin = ""
		Else
			FunGetCasBin = PathName
		End If
		
	End Function
	
	Sub subGetOther(ByRef strAppName As String, ByRef strKeyName As String, ByRef strDefault As String, ByRef strOutData As String)
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIに設定されているﾃﾞｰﾀを取得する。
		'引数
		'   strAppName(I)：ｱﾌﾟﾘｹｰｼｮﾝ名
		'   strKeyName(I)：ｷｰ名
		'   strDefault(I)：ｱﾌﾟﾘｹｰｼｮﾝ名、ｷｰ名が無いときの初期値
		'   strOutData(O)：ﾃﾞｰﾀ
		'戻り値
		'   無し
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim Data As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString(strAppName, strKeyName, "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		Data = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If Data = "NO_KEY" Then
			strOutData = strDefault
		Else
			strOutData = Data
		End If
		
	End Sub
	
	Function FunGetInputBin() As String
		'---------------------------------------------------------------------------------------------------
		'機能
		'   入力システムが格納されているBINﾃﾞｨﾚｸﾄﾘを設定する。
		'引数
		'   無し
		'戻り値
		'   BINのﾌﾙﾊﾟｽ名。
		'   ﾃﾞｨﾚｸﾄﾘが無いとき、""を返す。
		'---------------------------------------------------------------------------------------------------
		Dim PathName As String
		
		PathName = FunGetGensun() & "JP_InputData\"
		
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(PathName, FileAttribute.Directory) = "" Then
			FunGetInputBin = ""
		Else
			FunGetInputBin = PathName
		End If
		
	End Function
	
	' 2012-4-17 add by c.wu:
	Function FunGetLanguage() As String
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIに設定されている[Language]の名を取得する。
		'引数
		'   無し
		'戻り値
		'   [Language]のﾊﾟｽ名。
		'   設定されていないとき、またはﾃﾞｨﾚｸﾄﾘが無いとき、"JPN"を返す。
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
        Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim PathName, strEnviron As String
		Dim ret As Integer
		
		strEnviron = FunGetEnviron()
        IniFile = strEnviron & "JMANAGE.INI"

        ret = GetPrivateProfileString("Language", "Language", "JPN", ans.Value, MAX_CHAR, IniFile)

        PathName = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
        PathName = strEnviron & PathName & ".ini"
        FunGetLanguage = StrConv(PathName, VbStrConv.Uppercase)
		
	End Function
	
	Function FunGetTransLate(ByRef strAppName As String, ByRef strKeyName As String, ByRef strDefault As String) As String
		
		Dim ans As New VB6.FixedLengthString(256)
		Dim strFileName As String
		Dim ret As Integer
		
		strFileName = FunGetLanguage()
		
		If Not strFileName Like "*JPN.INI" Then
			ret = GetPrivateProfileString(strAppName, strKeyName, strDefault, ans.Value, 256, strFileName)
			FunGetTransLate = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		Else
			FunGetTransLate = strDefault
		End If
	End Function
	
	Function FunJudgeJpnFlg() As Integer
		Dim strFileName As String
		strFileName = FunGetLanguage()
		
		If Not strFileName Like "*JPN.INI" Then
			'英語版としてもENG.INIがなければ日本語版とする
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If Dir(FunGetEnviron() & "ENG.INI", FileAttribute.Normal) = "" Then
				FunJudgeJpnFlg = 0
			Else
				FunJudgeJpnFlg = 1
			End If
		Else
			FunJudgeJpnFlg = 0
		End If
	End Function
	
	Function FuncGetOptionFunction() As String
		'2014/06/04 simo 追加
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JMANAGE.INIに設定されているOptionFunctionを取得する。
		'引数
		'   無し
		'戻り値
		'   YBCFunction
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim Data As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		'ﾕｰｻﾞ名
		ret = GetPrivateProfileString("OptionFunction", "OptionFunction", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		Data = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If Data = "NO_KEY" Then
			FuncGetOptionFunction = "0"
		Else
			FuncGetOptionFunction = Trim(Data)
		End If
		
    End Function

    '2015/07/23 Nakagawa Add Start
    Sub subJmIniSetApp(kind As String, exeName As String)
        '---------------------------------------------------------------------------------------------------
        '機能
        '   JMANAGE.INIの[外部アプリケーション]にACADのパスを設定する。
        '引数
        '   kind(I)：種別(ACAD,EXCEL,ACCESS)
        '   exeName(I)：パス
        '戻り値
        '   無し
        '---------------------------------------------------------------------------------------------------
        Dim IniFile As String
        Dim ret As Integer

        IniFile = FunGetEnviron() & "JMANAGE.INI"
        ret = WritePrivateProfileString("外部アプリケーション", kind, exeName, IniFile)

    End Sub

    Sub subJmIniSetAppNo(no As Integer, exeName As String)
        '---------------------------------------------------------------------------------------------------
        '機能
        '   JMANAGE.INIの[外部アプリケーション]にACADのパスを設定する。
        '引数
        '   no(I)：アプリケーション番号(0:Editor,1:FTP)
        '   exeName(I)：パス
        '戻り値
        '   無し
        '---------------------------------------------------------------------------------------------------
        Dim IniFile As String
        Dim ret As Integer

        IniFile = FunGetEnviron() & "JMANAGE.INI"
        Dim appNo As String = "App[" & no & "]"
        ret = WritePrivateProfileString("外部アプリケーション", appNo, exeName, IniFile)

    End Sub

    Sub subJmIniSetSection(apName As String, keyName As String, param As String)
        '---------------------------------------------------------------------------------------------------
        '機能
        '   JMANAGE.INIに指定した内容を書き込む
        '引数
        '   apName(I)：セクション名
        '   keyName(I)：項目名
        '   param(I)：値
        '戻り値
        '   無し
        '---------------------------------------------------------------------------------------------------
        Dim IniFile As String
        Dim ret As Integer

        IniFile = FunGetEnviron() & "JMANAGE.INI"
        ret = WritePrivateProfileString(apName, keyName, param, IniFile)

    End Sub
    '2015/07/23 Nakagawa Add End

End Module