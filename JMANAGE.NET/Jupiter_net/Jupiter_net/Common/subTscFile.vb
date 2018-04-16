Option Strict Off
Option Explicit On
Module subTscFile
	'2009-3-13 Add by c.wu
	
	
	Sub DeleteRowsFromTscFile(ByRef strTscFile As String, ByRef strDeleteRows() As String)
		Dim filenum As Short
		Dim buf As String
		Dim fileInfo() As String
		'2009-3-17 Start by c.wu
		Dim i As Integer
		Dim iRowCount As Integer
		Dim iRowStep As Integer
		Dim iDeleteRowCount As Integer
		Dim findFlag As Boolean
		Dim iStartRowIndex As Integer
		'    Dim i               As Integer
		'    Dim iRowCount       As Integer
		'    Dim iRowStep        As Integer
		'    Dim iDeleteRowCount As Integer
		'    Dim findFlag        As Boolean
		'    Dim iStartRowIndex  As Integer
		'2009-3-17 End by c.wu
		
		iDeleteRowCount = 0
		iDeleteRowCount = UBound(strDeleteRows)
		If iDeleteRowCount <= 0 Then
			Exit Sub
		End If
		
		' Get all lines from file
		filenum = FreeFile
		iRowCount = 0
		FileOpen(filenum, strTscFile, OpenMode.Input)
		Do While Not EOF(filenum)
			buf = LineInput(filenum)
			iRowCount = iRowCount + 1
			ReDim Preserve fileInfo(iRowCount)
			fileInfo(iRowCount - 1) = buf
		Loop 
		FileClose(filenum)
		
		' Find the delete rows location
		iStartRowIndex = -1
		findFlag = False
		For iRowStep = 0 To iRowCount - 1
			If InStr(UCase(fileInfo(iRowStep)), UCase(strDeleteRows(0))) <> 0 Then
				If ((iRowStep + (iDeleteRowCount - 1)) < (iRowCount)) Then
					findFlag = True
					For i = 1 To iDeleteRowCount - 1
						If InStr(UCase(fileInfo(iRowStep + i)), UCase(strDeleteRows(i))) = 0 Then
							findFlag = False
							Exit For
						End If
					Next i
				End If
			End If
			If (findFlag = True) Then
				iStartRowIndex = iRowStep
				Exit For
			End If
		Next iRowStep
		' Haven't found the delete rows
		If (iStartRowIndex < 0) Then
			Exit Sub
		End If
		
		' Delete file
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(strTscFile, FileAttribute.Normal) <> "" Then
			Kill(strTscFile)
		End If
		
		' Append all lines except want to delete lines
		filenum = FreeFile
		FileOpen(filenum, strTscFile, OpenMode.Append)
		For i = 0 To iRowCount - 1
			If (i < iStartRowIndex) Or (i > (iStartRowIndex + iDeleteRowCount - 1)) Then
				PrintLine(filenum, fileInfo(i))
			End If
		Next 
		FileClose(filenum)
	End Sub
	
	
	Sub AppendRowsToTscFile(ByRef strTscFile As String, ByRef strAppendRows() As String)
		Dim filenum As Short
		Dim buf As String
		Dim fileInfo() As String
		'2009-3-17 Start by c.wu
		Dim i As Integer
		Dim iRowCount As Integer
		Dim iRowStep As Integer
		Dim iAppendRowCount As Integer
		Dim findFlag As Boolean
		Dim iStartRowIndex As Integer
		'    Dim i               As Integer
		'    Dim iRowCount       As Integer
		'    Dim iRowStep        As Integer
		'    Dim iAppendRowCount As Integer
		'    Dim findFlag        As Boolean
		'    Dim iStartRowIndex  As Integer
		'2009-3-17 End by c.wu
		
		iAppendRowCount = 0
		iAppendRowCount = UBound(strAppendRows)
		If iAppendRowCount <= 0 Then
			Exit Sub
		End If
		
		' Get all lines from file
		filenum = FreeFile
		iRowCount = 0
		FileOpen(filenum, strTscFile, OpenMode.Input)
		Do While Not EOF(filenum)
			buf = LineInput(filenum)
			iRowCount = iRowCount + 1
			ReDim Preserve fileInfo(iRowCount)
			fileInfo(iRowCount - 1) = buf
		Loop 
		FileClose(filenum)
		
		' Find the append rows location
		iStartRowIndex = -1
		findFlag = False
		For iRowStep = 0 To iRowCount - 1
			If InStr(UCase(fileInfo(iRowStep)), UCase(strAppendRows(0))) <> 0 Then
				If ((iRowStep + (iAppendRowCount - 1)) < (iRowCount)) Then
					findFlag = True
					For i = 1 To iAppendRowCount - 1
						If InStr(UCase(fileInfo(iRowStep + i)), UCase(strAppendRows(i))) = 0 Then
							findFlag = False
							Exit For
						End If
					Next i
				End If
			End If
			If (findFlag = True) Then
				iStartRowIndex = iRowStep
				Exit For
			End If
		Next iRowStep
		' Have found the append rows
		If (iStartRowIndex >= 0) Then
			Exit Sub
		End If
		
		'2009-3-17 Start by c.wu
		' Append the lines
		filenum = FreeFile
		FileOpen(filenum, strTscFile, OpenMode.Append)
		For i = 0 To iAppendRowCount - 1
			PrintLine(filenum, strAppendRows(i))
		Next 
		FileClose(filenum)
		'    ' Find the last row"(load "LogLib")(log_acadPrintCommandStatus "BziScriptexe" "ïîçﬁçÏê¨ÅFLispΩ∏ÿÃﬂƒé¿çs" "")"
		'    iStartRowIndex = iRowCount - 1
		'    For iRowStep = iRowCount - 1 To 0 Step -1
		'        If InStr(fileInfo(iRowStep), "log_acadPrintCommandStatus") <> 0 Then
		'            iStartRowIndex = iRowStep
		'            Exit For
		'        End If
		'    Next iRowStep
		'    If InStr(fileInfo(iStartRowIndex - 1), "FILEDIA") <> 0 Then
		'        iStartRowIndex = iStartRowIndex - 1
		'    End If
		'    If InStr(fileInfo(iStartRowIndex - 1), "NEW Y """"") <> 0 Then
		'        iStartRowIndex = iStartRowIndex - 1
		'    End If
		'
		'    ' Append the lines
		'    filenum = FreeFile
		'    Open strTscFile For Append As #filenum
		'        For iRowStep = 0 To iStartRowIndex - 1
		'            Print #filenum, fileInfo(iRowStep)
		'        Next iRowStep
		'
		'        For i = 0 To iAppendRowCount - 1
		'            Print #filenum, strAppendRows(i)
		'        Next
		'
		'        For iRowStep = iStartRowIndex To iRowCount - 1
		'            Print #filenum, fileInfo(iRowStep)
		'        Next iRowStep
		'    Close #filenum
		'2009-3-17 End by c.wu
	End Sub
	
	Sub AppendRowsToueiTorikomiToTscFile(ByRef strTscFile As String, ByRef strDba2Dwg As String, ByRef strAppendRows() As String)
		Dim filenum As Short
		Dim buf As String
		Dim fileInfo() As String
		Dim i As Integer
		Dim iRowCount As Integer
		Dim iRowStep As Integer
		Dim iAppendRowCount As Integer
		Dim findFlag As Boolean
		Dim iStartRowIndex As Integer
		
		iAppendRowCount = 0
		iAppendRowCount = UBound(strAppendRows)
		If iAppendRowCount <= 0 Then
			Exit Sub
		End If
		
		' Get all lines from file
		filenum = FreeFile
		iRowCount = 0
		FileOpen(filenum, strTscFile, OpenMode.Input)
		Do While Not EOF(filenum)
			buf = LineInput(filenum)
			iRowCount = iRowCount + 1
			ReDim Preserve fileInfo(iRowCount)
			fileInfo(iRowCount - 1) = buf
		Loop 
		FileClose(filenum)
		
		' Find the append rows location
		iStartRowIndex = -1
		findFlag = False
		For iRowStep = 0 To iRowCount - 1
			If InStr(UCase(fileInfo(iRowStep)), UCase(strAppendRows(0))) <> 0 Then
				If ((iRowStep + (iAppendRowCount - 1)) < (iRowCount)) Then
					findFlag = True
					For i = 1 To iAppendRowCount - 1
						If InStr(UCase(fileInfo(iRowStep + i)), UCase(strAppendRows(i))) = 0 Then
							findFlag = False
							Exit For
						End If
					Next i
				End If
			End If
			If (findFlag = True) Then
				iStartRowIndex = iRowStep
				Exit For
			End If
		Next iRowStep
		' Have found the append rows
		If (iStartRowIndex >= 0) Then
			Exit Sub
		End If
		
		For iRowStep = iRowCount - 1 To 0 Step -1
			If InStr(UCase(fileInfo(iRowStep)), UCase(strDba2Dwg)) <> 0 Then
				iStartRowIndex = iRowStep
				Exit For
			End If
		Next iRowStep
		
		' Delete file
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(strTscFile, FileAttribute.Normal) <> "" Then
			Kill(strTscFile)
		End If
		
		' Append all lines except want to delete lines
		filenum = FreeFile
		FileOpen(filenum, strTscFile, OpenMode.Append)
		For i = 0 To iRowCount - 1
			If (i = iStartRowIndex) Then
				For iRowStep = 0 To iAppendRowCount - 1
					PrintLine(filenum, strAppendRows(iRowStep))
				Next iRowStep
			End If
			PrintLine(filenum, fileInfo(i))
		Next 
		FileClose(filenum)
	End Sub
End Module