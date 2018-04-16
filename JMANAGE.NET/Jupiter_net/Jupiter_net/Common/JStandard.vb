Option Strict Off
Option Explicit On
Module JStandard
	
	Function 標準値ファイルの読み込み(ByRef dname As String, ByRef keyword As String, ByRef nrec As Short, ByRef drec As String) As Short
		'---------------------------------------------------------------------------------------------------
		'機能
		'   標準値ﾌｧｲﾙからｷｰﾜｰﾄﾞのﾚｺｰﾄﾞ番号のﾃﾞｰﾀを読み込む。
		'   標準値ﾌｧｲﾙは、ｶﾚﾝﾄﾃﾞｨﾚｸﾄﾘ、次に(GENSUN_DIR)\ENVIRONの順に有無を判断する。
		'引数
		'   dname(I)：ｶﾚﾝﾄﾃﾞｨﾚｸﾄﾘ名
		'   keyword(I)：検索するｷｰﾜｰﾄﾞ(FLG、WEB等)。必ず大文字で渡す事。
		'   nrec(I)：ｷｰﾜｰﾄﾞ内の取得するﾃﾞｰﾀのﾚｺｰﾄﾞ番号
		'   drec(O)：目的のﾚｺｰﾄﾞのﾃﾞｰﾀ
		'戻り値
		'   0：ｷｰﾜｰﾄﾞのﾚｺｰﾄﾞ番号のﾃﾞｰﾀを読み込めた。
		'   1：標準値ﾌｧｲﾙが存在しない。
		'   2：標準値ﾌｧｲﾙ内にｷｰﾜｰﾄﾞが見つからない。
		'   3：ｷｰﾜｰﾄﾞ内にﾚｺｰﾄﾞ番号のﾃﾞｰﾀが見つからない。
		'---------------------------------------------------------------------------------------------------
		Dim flg, fnum, nr As Short
		Dim fname, kw As String
		Dim buf, msg As String
		Dim bo As Boolean
		Dim strFileName As String
		
		strFileName = FunGetLanguage()
		
		'標準値ﾌｧｲﾙ内にｷｰﾜｰﾄﾞが有るかどうかのﾌﾗｸﾞ
		flg = 0

        ' 2015/07/22 Nakagawa Edit Start
        kw = ""
        ' 2015/07/22 Nakagawa Edit End

		'ｶﾚﾝﾄﾃﾞｨﾚｸﾄﾘに標準値ﾌｧｲﾙが有るときは、それを読み込む。無いときは、GENSUN_DIRのENVIRONの標準値ﾌｧｲﾙを読み込む。
		fname = dname & "STANDARD.DAT"
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(fname, FileAttribute.Normal) = "" Then
			fname = FunGetEnviron() & "STANDARD.DAT"
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If Dir(fname, FileAttribute.Normal) = "" Then
				Beep()
				If Not strFileName Like "*ENG.INI" Then
					msg = "標準値ﾌｧｲﾙ(STANDARD.DAT)がｶﾚﾝﾄﾃﾞｨﾚｸﾄﾘにもENVIRONﾃﾞｨﾚｸﾄﾘにも有りません。" & Chr(10)
					msg = msg & "CastarJupiterの実行環境が正しくｲﾝｽﾄｰﾙされていません。"
				Else
					msg = "A standard value file (STANDARD.DAT) is not in a current directory and an ENVIRON directory." & Chr(10)
					msg = msg & "The execution environment of CastarJupiter is not installed correctly."
				End If
				MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
				標準値ファイルの読み込み = 1
				Exit Function
			End If
		End If
		
		fnum = FreeFile
		FileOpen(fnum, fname, OpenMode.Input)
		Do While Not EOF(fnum)
			buf = LineInput(fnum)
			If Mid(buf, 1, 1) = ";" Then GoTo NEXT_L1
			
			'先頭文字が￥であるかどうか判定し、目的のｷｰﾜｰﾄﾞか判定する。
			If Mid(buf, 1, 1) = "\" Then
				bo = fun文字列抽出(buf, "\", " ", kw)
				If keyword = UCase(kw) Then
					flg = 1
					nr = 0
					Do While Not EOF(fnum)
						buf = LineInput(fnum)
						If Mid(buf, 1, 1) = ";" Then GoTo NEXT_L2
						If buf = "" Then GoTo NEXT_L2
						
						'次のｷｰﾜｰﾄﾞが見つかったとき、読み込みを終了する。
						If Mid(buf, 1, 1) = "\" Then GoTo EXIT_FILE
						
						'目的のﾚｺｰﾄﾞ番号位置であるかどうか判定し、ﾃﾞｰﾀを格納する。
						nr = nr + 1
						If nrec = nr Then
							flg = 2
							drec = buf
							FileClose(fnum)
							標準値ファイルの読み込み = 0
							Exit Function
						End If
NEXT_L2: 
					Loop 
				End If
			End If
NEXT_L1: 
		Loop 
		
EXIT_FILE: 
		
		FileClose(fnum)
		
		If flg = 0 Then
			Beep()
			If Not strFileName Like "*ENG.INI" Then
				msg = "標準値ﾌｧｲﾙ(" & fname & ")内にｷｰﾜｰﾄﾞ(" & keyword & ")と一致する項目が有りません。" & Chr(10)
				msg = msg & "標準値ﾌｧｲﾙが古い可能性が有ります。最新のものと入れ替えて下さい。"
			Else
				msg = "The item which is in agreement with a keyword(" & keyword & ")is not in a standard value file(" & fname & ")." & Chr(10)
				msg = msg & "There is a possibility that a standard value file is old.Please change for the newest thing."
			End If
			MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
			標準値ファイルの読み込み = 2
			Exit Function
		End If
		If flg = 1 Then
			Beep()
			If Not strFileName Like "*ENG.INI" Then
				msg = "標準値ﾌｧｲﾙ(" & fname & ")内にｷｰﾜｰﾄﾞ(" & keyword & ")の" & nrec & "行目で取得できる項目が有りません。" & Chr(10)
				msg = msg & "標準値ﾌｧｲﾙが古い可能性が有ります。最新のものと入れ替えて下さい。"
			Else
				msg = "An item acquirable by the " & nrec & " th line of keyword(" & keyword & ")is not in a standard value file(" & fname & ")." & Chr(10)
				msg = msg & "There is a possibility that a standard value file is old.Please change for the newest thing."
			End If
			MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
			標準値ファイルの読み込み = 2
			Exit Function
		End If
		
		'標準値ﾌｧｲﾙにｷｰﾜｰﾄﾞは見つかったが、ﾚｺｰﾄﾞ番号のﾃﾞｰﾀが見つからないとき、ここまで処理が来る。
		標準値ファイルの読み込み = 3
		
	End Function
End Module