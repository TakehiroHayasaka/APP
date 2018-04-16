Option Strict Off
Option Explicit On
Module JString
	
	Function fun文字列抽出(ByRef idata As String, ByRef titles As String, ByRef terminate As String, ByRef ans As String) As Boolean
		'-----------------------------------------------------------------
		' 文字列idataの中からtitleとterminateで囲まれた文字列を抽出する。
		' terminate文字がidata内にない場合は、title以降の文字列全を出力する。
		'-----------------------------------------------------------------
		Dim pos As String
		Dim Rword As String

		fun文字列抽出 = False
		
		pos = CStr(InStr(idata, titles))
		If CDbl(pos) = 0 Then Exit Function
		
		pos = CStr(CDbl(pos) + Len(titles))
		Rword = Right(idata, Len(idata) - CDbl(pos) + 1)
		
		pos = CStr(InStr(Rword, terminate))
		If CDbl(pos) > 0 Then
			ans = Left(Rword, CDbl(pos) - 1)
		Else
			ans = Rword
		End If
		fun文字列抽出 = True
		
	End Function
	
	Function J_ChoiceString(ByRef st_data As String, ByRef nn As Short) As String
		'-----------------------------------------------------------------
		'機能
		'   空白をﾃﾞｨﾐｯﾀとした場合、nn番目の文字列を抽出する
		'-----------------------------------------------------------------
		Dim RRword, buf, LLword, buf2 As String
		Dim i, pos As Short

        ' 2015/07/22 Nakagawa Edit Start
        LLword = ""
        ' 2015/07/22 Nakagawa Edit End

		buf = Trim(st_data)
		buf2 = J_TransChar(buf, Chr(9), " ")
		buf = J_RemoveDoubleChar(buf2, " ")
		buf = Trim(buf)
		
		For i = 0 To nn
			pos = InStr(buf, " ")
			If pos = 0 Then pos = InStr(buf, Chr(9))
			
			If pos > 0 Then
				LLword = Left(buf, pos - 1)
				RRword = Right(buf, Len(buf) - pos)
			Else
				LLword = buf
				Exit For
			End If
			buf = RRword
		Next i
		J_ChoiceString = LLword
		
	End Function
	
	Function J_MidString(ByRef idata As String, ByRef delimit As String) As String
		'-----------------------------------------------------------------
		'機能
		'   文字列idataで、ﾃﾞｨﾐｯﾀ文字delimitで挟まれた文字列を抽出する。
		'-----------------------------------------------------------------
		Dim Rword As String
		Dim pos, pos2 As Short
		
		pos = InStr(idata, delimit)
		If pos > 0 Then
			Rword = Right(idata, Len(idata) - pos)
			pos2 = InStr(Rword, delimit)
			If pos2 > 0 Then
				J_MidString = Left(Rword, pos2 - 1)
				Exit Function
			End If
		End If
		J_MidString = ""
		
	End Function
	
	Function J_RemoveDoubleChar(ByRef src As String, ByRef cch As String) As String
		'-----------------------------------------------------------------
		'機能
		'   連続重複文字の削除処理。文字列src内の文字cchがﾗｯﾌﾟしていた場合。
		'-----------------------------------------------------------------
		Dim i, nn As Short
		Dim dst As String
		Dim s1, s2 As String
		Dim j, k As Short
		
		dst = ""
		nn = Len(src)
		i = 1
		Do 
			If i > nn Then Exit Do
			
			s1 = Mid(src, i, 1)
			k = 0
			If s1 = cch Then
				For j = i + 1 To nn
					s2 = Mid(src, j, 1)
					If s1 <> s2 Then Exit For
					k = k + 1
				Next j
			End If
			dst = dst & s1
			i = i + 1 + k
		Loop 
		J_RemoveDoubleChar = dst
		
	End Function
	
	Function J_TransChar(ByRef src As String, ByRef srcC As String, ByRef dstC As String) As String
		'-----------------------------------------------------------------
		'機能
		'   文字列の置き換え処理。文字列src内の文字srcCをdstCに置き換える
		'-----------------------------------------------------------------
		Dim dst As String
		Dim i, nn As Short
		Dim cch As String
		
		dst = ""
		nn = Len(src)
		For i = 1 To nn
			cch = Mid(src, i, 1)
			If cch = srcC Then
				dst = dst & dstC
			Else
				dst = dst & cch
			End If
		Next i
		J_TransChar = dst
		
	End Function
	
	Function J_NWord(ByRef src As String) As Short
		'-----------------------------------------------------------------
		'機能
		'   ﾌﾞﾗﾝｸ区切りで文字列が何個あるか
		'-----------------------------------------------------------------
		Dim nWord, pos As Short
		Dim buf, buf2 As String
		Dim LLword, RRword As String
		
		nWord = 0
		
		buf = Trim(src)
		buf2 = J_TransChar(buf, Chr(9), " ")
		buf = J_RemoveDoubleChar(buf2, " ")
		buf = Trim(buf)
		
		If buf = "" Then
			J_NWord = 0
			Exit Function
		End If
		
		Do 
			pos = InStr(buf, " ")
			If pos = 0 Then
				nWord = nWord + 1
				Exit Do
			End If
			
			nWord = nWord + 1
			LLword = Left(buf, pos - 1)
			RRword = Right(buf, Len(buf) - pos)
			buf = RRword
		Loop 
		
		J_NWord = nWord
		
	End Function
	
	'--------------------------------------------------------
	'   関数名  : DoubleByteChk
	'   用途    : 文字列内に2ﾊﾞｲﾄ文字が含まれているかどうかを調べる
	'   引数    : strCheckString 調査対象文字列
	'   戻り値  : 2ﾊﾞｲﾄ文字を発見した位置
	'--------------------------------------------------------
	Function DoubleByteChk(ByRef strCheckString As String) As Integer
		Dim i As Integer
		'調査対象文字列の長さを格納
		Dim lngCheckSize As Integer
		'ANSIへの変換後の文字を格納
		Dim lngANSIStr As Integer
		
		DoubleByteChk = 0
		lngCheckSize = Len(strCheckString)
		
		For i = 1 To lngCheckSize
			'StrConvでUnicodeからANSIへと変換
			'UPGRADE_ISSUE: Constant vbFromUnicode was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"'
            'UPGRADE_ISSUE: LenB function is not supported. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"'
            'mod by xiaoyun.z 20150408 sta
            'lngANSIStr = Strings.LenB(StrConv(Mid(strCheckString, i, 1), vbFromUnicode))
            ' 2015/07/22 Nakagawa Edit Start
            'lngANSIStr = StrConv(Mid(strCheckString, i, 1), 128)
            lngANSIStr = LenB(Mid(strCheckString, i, 1))
            ' 2015/07/22 Nakagawa Edit End
            'mod by xiaoyun.z 20150408 end
			'変換後の文字が2ﾊﾞｲﾄかどうか判断
			If lngANSIStr = 2 Then
				DoubleByteChk = i
				Exit For
			End If
		Next i
		
	End Function

    Public Function LenB(ByVal str As String) As Long
        'Shift JISに変換したときに必要なバイト数を返す
        Return System.Text.Encoding.GetEncoding(932).GetByteCount(str)
    End Function

End Module