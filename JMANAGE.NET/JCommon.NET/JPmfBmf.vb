Option Strict Off
Option Explicit On
Module JPmfBmf
	
	'***************************************************************************************************
	'使用しているWindowsAPIの宣言
	'***************************************************************************************************
	'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
    Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
	
	'***************************************************************************************************
	'ﾕｰｻﾞｰ定義定数の宣言
	'***************************************************************************************************
	Private Const MAX_CHAR As Short = 256 '最大文字数
	
	'***************************************************************************************************
	'ﾕｰｻﾞｰ定義構造体の宣言
	'***************************************************************************************************
	'---------------------------------------------------------------------------------------------------
	'2次元部材、2次元ﾌﾞﾛｯｸ両ﾏｽﾀｰﾌｧｲﾙ名検索用構造体
	'---------------------------------------------------------------------------------------------------
	Structure MAST
		Dim fcnt As Short '検索数
		Dim fname() As String 'ﾏｽﾀｰﾌｧｲﾙ名(数を再宣言する)
		Dim fcode() As String 'ﾌｧｲﾙ変換名(数を再宣言する)
	End Structure
	
	Function funINIから検索数を取得(ByRef IniFile As String) As Short
		'---------------------------------------------------------------------------------------------------
		'機能
		'   ﾏｽﾀｰﾌｧｲﾙ名検索ﾌｧｲﾙから検索数を取得する。
		'引数
		'   IniFile(I)：ﾏｽﾀｰﾌｧｲﾙ名検索ﾌｧｲﾙ名(ﾌﾙﾊﾟｽ名)
		'戻り値
		'   検索数：ﾏｽﾀｰﾌｧｲﾙ名検索ﾌｧｲﾙに記述されている検索数。
		'           ﾏｽﾀｰﾌｧｲﾙ名検索ﾌｧｲﾙに検索数のｷｰﾜｰﾄﾞが無いとき、0とする。
		'---------------------------------------------------------------------------------------------------
		Dim cnt As Short
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim buf As String
		Dim ret As Integer
		
		ret = GetPrivateProfileString("TransTable", "count", "0", ans.Value, MAX_CHAR, IniFile)
		buf = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		cnt = CShort(buf)
		
		funINIから検索数を取得 = cnt
		
	End Function
	
	Function funINIから検索テーブルを取得(ByRef IniFile As String, ByRef id As Short, ByRef fname As String, ByRef fcode As String) As Boolean
		'---------------------------------------------------------------------------------------------------
		'機能
		'   ﾏｽﾀｰﾌｧｲﾙ名検索ﾌｧｲﾙから検索するﾏｽﾀｰﾌｧｲﾙ名とﾌｧｲﾙ変換名を取得する。
		'引数
		'   IniFile(I)：ﾏｽﾀｰﾌｧｲﾙ名検索ﾌｧｲﾙ名(ﾌﾙﾊﾟｽ名)
		'   id(I)：取得するﾃｰﾌﾞﾙ番号
		'   fname(O)：ﾏｽﾀｰﾌｧｲﾙ名
		'   fcode(O)：ﾌｧｲﾙ変換名
		'戻り値
		'   True：ﾏｽﾀｰﾌｧｲﾙ名とﾌｧｲﾙ変換名を正しく取得できた。
		'   False：ﾏｽﾀｰﾌｧｲﾙ名とﾌｧｲﾙ変換名を取得できなかった。
		'---------------------------------------------------------------------------------------------------
		Dim pos As Short
		Dim st As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim buf, msg As String
		Dim ret As Integer
		Dim strFileName As String
		
		strFileName = FunGetLanguage()
		
		st = "Trans" & CStr(id)
		ret = GetPrivateProfileString("TransTable", st, "*Undefined", ans.Value, MAX_CHAR, IniFile)
		buf = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If buf = "*Undefined" Then
			funINIから検索テーブルを取得 = False
			Exit Function
		End If
		
		pos = InStr(buf, " ")
		If pos = 0 Then
			Beep()
			If Not strFileName Like "*ENG.INI" Then
				msg = "ﾏｽﾀｰﾌｧｲﾙ名検索ﾌｧｲﾙ(" & IniFile & ")の内容がおかしいです。" & Chr(10)
				msg = msg & "ﾏｽﾀｰﾌｧｲﾙ名とﾌｧｲﾙ変換名の間にﾌﾞﾗﾝｸ区切りが無い可能性があります。" & Chr(10)
				msg = msg & "ﾌｧｲﾙ内容を確認し、正しく修正して下さい。" & Chr(10)
				msg = msg & "内容 ： " & buf
			Else
				msg = "The contents of a master file name search file(" & IniFile & ")are amusing." & Chr(10)
				msg = msg & "There may be no blank pause between a master file name and a file conversion name." & Chr(10)
				msg = msg & "Please check the contents of a file and correct correctly." & Chr(10)
				msg = msg & "Contents ： " & buf
			End If
			MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
			funINIから検索テーブルを取得 = False
			Exit Function
		End If
		
		fname = Trim(Left(buf, pos - 1))
		fcode = Trim(Right(buf, Len(buf) - pos))
		
		funINIから検索テーブルを取得 = True
		
	End Function
	
	Sub SubINIからデータを取得(ByRef IniFile As String, ByRef Mast_name As MAST)
		'---------------------------------------------------------------------------------------------------
		'機能
		'   ﾏｽﾀｰﾌｧｲﾙ名検索ﾌｧｲﾙから検索数、検索するﾏｽﾀｰﾌｧｲﾙ名とﾌｧｲﾙ変換名をﾏｽﾀｰﾌｧｲﾙ名検索用構造体に格納する。
		'引数
		'   IniFile(I)：ﾏｽﾀｰﾌｧｲﾙ名検索ﾌｧｲﾙ名(ﾌﾙﾊﾟｽ名)
		'   mast_name(O)：ﾏｽﾀｰﾌｧｲﾙ名検索用構造体
		'戻り値
		'   無し
		'---------------------------------------------------------------------------------------------------
		Dim i, cnt As Short
		Dim fname, fcode As String
		
		'検索数を取得する。
		Mast_name.fcnt = funINIから検索数を取得(IniFile)

		'検索数分のﾏｽﾀｰﾌｧｲﾙ名とﾌｧｲﾙ変換名の配列を確保する。
		ReDim Mast_name.fname(Mast_name.fcnt)
		ReDim Mast_name.fcode(Mast_name.fcnt)
		
		'ﾏｽﾀｰﾌｧｲﾙ名とﾌｧｲﾙ変換名を取得する。
		cnt = 0
		For i = 1 To Mast_name.fcnt
			If funINIから検索テーブルを取得(IniFile, i, fname, fcode) = True Then
				cnt = cnt + 1
				Mast_name.fname(cnt) = fname
				Mast_name.fcode(cnt) = fcode
			End If
		Next i
		
		'ﾏｽﾀｰﾌｧｲﾙ検索ﾌｧｲﾙから正しく読み込めた数にする。
		Mast_name.fcnt = cnt
		
	End Sub
	
	Function funBMFファイルの検索(ByRef dname As String, ByRef obj As Object, ByRef bname As String) As Short
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JBMF.INIから検索する2次元ﾌﾞﾛｯｸﾏｽﾀｰﾌｧｲﾙ名とﾌｧｲﾙ変換名を取得し、
		'   工事ﾃﾞｨﾚｸﾄﾘから取得した2次元ﾌﾞﾛｯｸﾏｽﾀｰﾌｧｲﾙ(BmfMast)を検索し、画面のｺﾝﾎﾞﾎﾞｯｸｽに追加する。
		'引数
		'   dname(I)：工事ﾃﾞｨﾚｸﾄﾘ名
		'   obj(I)：ｺﾝﾎﾞﾎﾞｯｸｽのｵﾌﾞｼﾞｪｸﾄ名（ﾌｫｰﾑ名!ｺﾝﾎﾞ名）
		'   bname(I)：ｺﾝﾎﾞﾎﾞｯｸｽの初期選択する2次元ﾌﾞﾛｯｸﾏｽﾀｰﾌｧｲﾙ名。必ず大文字で渡す事。
		'戻り値
		'   検索数
		'     0：工事ﾃﾞｨﾚｸﾄﾘに2次元ﾌﾞﾛｯｸﾏｽﾀｰﾌｧｲﾙが1つも存在しない。
		'     1以上：工事ﾃﾞｨﾚｸﾄﾘに2次元ﾌﾞﾛｯｸﾏｽﾀｰﾌｧｲﾙが存在する。その数値はﾌｧｲﾙ数を表す。
		'---------------------------------------------------------------------------------------------------
		Dim cnt_bmf, i, idx_bmf As Short
		Dim IniFile As String
		Dim msg As String
		'UPGRADE_WARNING: Arrays in structure BmfMast may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim BmfMast As MAST
		Dim strFileName As String
		
		strFileName = FunGetLanguage()
		
		'JBMF.INIから検索する2次元ﾌﾞﾛｯｸﾏｽﾀｰﾌｧｲﾙ名とﾌｧｲﾙ変換名を取得する。
		IniFile = FunGetEnviron() & "JBMF.INI"
		Call SubINIからデータを取得(IniFile, BmfMast)
		
		cnt_bmf = 0
		idx_bmf = 0
		'UPGRADE_WARNING: Couldn't resolve default property of object obj.Clear. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        obj.items.Clear()
		
		For i = 1 To BmfMast.fcnt
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If Dir(dname & BmfMast.fname(i), FileAttribute.Normal) <> "" Then
				'UPGRADE_WARNING: Couldn't resolve default property of object obj.AddItem. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                obj.items.add(BmfMast.fname(i) & " <" & BmfMast.fcode(i) & ">")
				'初期選択とするため、ｺﾝﾎﾞﾎﾞｯｸｽに追加した位置を確保する。
				If UCase(BmfMast.fname(i)) = bname Then
					idx_bmf = cnt_bmf
				End If
				cnt_bmf = cnt_bmf + 1
			End If
		Next i
		
		If cnt_bmf = 0 Then
			If Not strFileName Like "*ENG.INI" Then
				msg = "指定した工号には、2次元ﾌﾞﾛｯｸﾏｽﾀｰﾌｧｲﾙが1つもありません。" & Chr(10)
			Else
				msg = "There is not a BMF master file, either." & Chr(10)
			End If
			MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
			funBMFファイルの検索 = cnt_bmf
		Else
			funBMFファイルの検索 = cnt_bmf
			'UPGRADE_WARNING: Couldn't resolve default property of object obj.ListIndex. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            obj.selectedIndex = idx_bmf
		End If
		
	End Function
	
	Function funPMFファイルの検索(ByRef dname As String, ByRef obj As Object, ByRef pname As String) As Short
		'---------------------------------------------------------------------------------------------------
		'機能
		'   JPMF.INIから検索する2次元部材ﾏｽﾀｰﾌｧｲﾙ名とﾌｧｲﾙ変換名を取得し、
		'   工事ﾃﾞｨﾚｸﾄﾘから取得した2次元部材ﾏｽﾀｰﾌｧｲﾙ(PmfMast)を検索し、画面のｺﾝﾎﾞﾎﾞｯｸｽに追加する。
		'引数
		'   dname(I)：工事ﾃﾞｨﾚｸﾄﾘ名
		'   obj(I)：ｺﾝﾎﾞﾎﾞｯｸｽのｵﾌﾞｼﾞｪｸﾄ名（ﾌｫｰﾑ名!ｺﾝﾎﾞ名）
		'   pname(I)：ｺﾝﾎﾞﾎﾞｯｸｽの初期選択する2次元部材ﾏｽﾀｰﾌｧｲﾙ名。必ず大文字で渡す事。
		'戻り値
		'   検索数
		'     0：工事ﾃﾞｨﾚｸﾄﾘに2次元部材ﾏｽﾀｰﾌｧｲﾙが1つも存在しない。
		'     1以上：工事ﾃﾞｨﾚｸﾄﾘに2次元部材ﾏｽﾀｰﾌｧｲﾙが存在する。その数値はﾌｧｲﾙ数を表す。
		'---------------------------------------------------------------------------------------------------
		Dim cnt_pmf, i, idx_pmf As Short
		Dim IniFile As String
		Dim msg As String
		'UPGRADE_WARNING: Arrays in structure PmfMast may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim PmfMast As MAST
		Dim strFileName As String
		
		strFileName = FunGetLanguage()
		
		'JPMF.INIから検索する2次元部材ﾏｽﾀｰﾌｧｲﾙ名とﾌｧｲﾙ変換名を取得する。
		IniFile = FunGetEnviron() & "JPMF.INI"
        Call SubINIからデータを取得(IniFile, PmfMast)
		
		'UPGRADE_WARNING: Couldn't resolve default property of object obj.ListCount. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        cnt_pmf = obj.items.count
		idx_pmf = 0
		
        For i = 1 To PmfMast.fcnt
            'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            If Dir(dname & PmfMast.fname(i), FileAttribute.Normal) <> "" Then
                'UPGRADE_WARNING: Couldn't resolve default property of object obj.AddItem. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                obj.Items.Add(PmfMast.fname(i) & " <" & PmfMast.fcode(i) & ">")
                '初期選択とするため、ｺﾝﾎﾞﾎﾞｯｸｽに追加した位置を確保する。
                If UCase(PmfMast.fname(i)) = pname Then
                    idx_pmf = cnt_pmf
                End If
                cnt_pmf = cnt_pmf + 1
            End If
        Next i
		
		If cnt_pmf = 0 Then
			If Not strFileName Like "*ENG.INI" Then
				msg = "指定した工号には、2次元部材ﾏｽﾀｰﾌｧｲﾙが1つもありません。" & Chr(10)
			Else
				msg = "There is not a PMF master file, either." & Chr(10)
			End If
			MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
			funPMFファイルの検索 = cnt_pmf
		Else
			funPMFファイルの検索 = cnt_pmf
			'UPGRADE_WARNING: Couldn't resolve default property of object obj.ListIndex. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            obj.selectedindex = idx_pmf
		End If
		
	End Function
	
	Function fun拡張子指定ファイルの検索(ByRef dname As String, ByRef obj As Object, ByRef ext As String) As Short
		'---------------------------------------------------------------------------------------------------
		'機能
		'   工事ﾃﾞｨﾚｸﾄﾘから拡張子指定のﾌｧｲﾙを検索し、画面のｺﾝﾎﾞﾎﾞｯｸｽに追加する。
		'引数
		'   dname(I)：工事ﾃﾞｨﾚｸﾄﾘ名
		'   obj(I)：ｺﾝﾎﾞﾎﾞｯｸｽのｵﾌﾞｼﾞｪｸﾄ名（ﾌｫｰﾑ名!ｺﾝﾎﾞ名）
		'   ext(I)：検索する拡張子
		'戻り値
		'   検索数
		'     0：工事ﾃﾞｨﾚｸﾄﾘに拡張子のﾌｧｲﾙが1つも存在しない。
		'     1以上：工事ﾃﾞｨﾚｸﾄﾘに拡張子のﾌｧｲﾙが存在する。その数値はﾌｧｲﾙ数を表す。
		'---------------------------------------------------------------------------------------------------
		Dim cnt_ext As Short
		Dim fname As String
		Dim msg As String
		
		cnt_ext = 0
		
		'JPMF.INIから検索する2次元部材ﾏｽﾀｰﾌｧｲﾙ名とﾌｧｲﾙ変換名を取得する。
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		fname = Dir(dname & "*." & ext, FileAttribute.Normal)
		Do While fname <> ""
			'UPGRADE_WARNING: Couldn't resolve default property of object obj.AddItem. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'obj.AddItem(fname)
            obj.Items.Add(fname)
			cnt_ext = cnt_ext + 1
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			fname = Dir()
		Loop 
		
		fun拡張子指定ファイルの検索 = cnt_ext
		
	End Function
	Function funPMD拡張子指定ファイルの検索(ByRef dname As String, ByRef obj As Object) As Short
		'---------------------------------------------------------------------------------------------------
		'機能
		'   工事ﾃﾞｨﾚｸﾄﾘからPMD拡張子のﾌｧｲﾙを検索し、画面のｺﾝﾎﾞﾎﾞｯｸｽに追加する。
		'   ｺﾝﾎﾞﾎﾞｯｸｽに追加するときに、同名のPMSファイルがあるときは追加しない。
		'引数
		'   dname(I)：工事ﾃﾞｨﾚｸﾄﾘ名
		'   obj(I)：ｺﾝﾎﾞﾎﾞｯｸｽのｵﾌﾞｼﾞｪｸﾄ名（ﾌｫｰﾑ名!ｺﾝﾎﾞ名）
		'戻り値
		'   検索数
		'     0：工事ﾃﾞｨﾚｸﾄﾘに拡張子のﾌｧｲﾙが1つも存在しない。
		'     1以上：工事ﾃﾞｨﾚｸﾄﾘに拡張子のﾌｧｲﾙが存在する。その数値はﾌｧｲﾙ数を表す。
		'---------------------------------------------------------------------------------------------------
		Dim cnt_ext As Short
		Dim pos, i, flgUmu As Short
		Dim fname As String
		Dim pmd_name, pms_name As String
		Dim msg As String
		
		cnt_ext = 0
		flgUmu = 0
		
		'JPMF.INIから検索する2次元部材ﾏｽﾀｰﾌｧｲﾙ名とﾌｧｲﾙ変換名を取得する。
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		fname = Dir(dname & "*.PMD", FileAttribute.Normal)
		Do While fname <> ""
			pos = InStr(UCase(fname), ".PMD")
			pmd_name = Left(fname, pos - 1)
			'UPGRADE_WARNING: Couldn't resolve default property of object obj.ListCount. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'For i = 0 To obj.ListCount - 1
            For i = 0 To obj.Items.Count() - 1
                'UPGRADE_WARNING: Couldn't resolve default property of object obj.List. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'pos = InStr(UCase(obj.List(i)), ".PMS")
                pos = InStr(UCase(obj.Items().Item(i)), ".PMS")
                If pos > 0 Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object obj.List. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'pms_name = Left(obj.List(i), pos - 1)
                    pms_name = Left(obj.Items().Item(i), pos - 1)
                    If UCase(pmd_name) = UCase(pms_name) Then
                        GoTo NEXT_PMD
                    End If
                End If
            Next i
            If flgUmu = 0 Then
                'UPGRADE_WARNING: Couldn't resolve default property of object obj.AddItem. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'obj.AddItem(fname)
                obj.Items.Add(fname)
                cnt_ext = cnt_ext + 1
            End If

NEXT_PMD:
            'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            fname = Dir()
        Loop
		
		funPMD拡張子指定ファイルの検索 = cnt_ext
		
	End Function
End Module