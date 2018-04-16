Option Strict Off
Option Explicit On
Module XlsMake
	
	Sub 新しいSheetにコピー(ByRef InFilePath As String, ByRef InName As String, ByRef OutFilePath As String, ByRef OutName As String, ByRef SheetNo As Short, ByRef SheetName As String)
		Dim FilePathName As String
		Dim OutNameTemp As String
		Dim xlApp As Microsoft.Office.Interop.Excel.Application
		
		xlApp = New Microsoft.Office.Interop.Excel.Application
		
		OutNameTemp = OutName
		'20100913 whl 追加 start
		If CDbl(xlApp.Version) > 11 Then
			OutNameTemp = OutNameToXlsx(OutName)
		End If
		'20100913 whl 追加 end
		
		'出力Workbookを開く。
		FilePathName = OutFilePath & OutNameTemp
		'MsgBox "OutNameTemp=" & FilePathName
		xlApp.Workbooks.Open(FileName:=FilePathName)
		'MsgBox "Open1 OK"
		
		'入力Workbookを開く。
		FilePathName = InFilePath & InName
		'MsgBox "InName=" & FilePathName
		xlApp.Workbooks.Open(FileName:=FilePathName)
		'MsgBox "Open2 OK"
		'nira S 2014/4/10
		'シート名の変更がxlsにシート移動したあとに行うとOFFICE2013環境ではエラーになる。
		'よって、シート名をhtmファイルのときに変更する。
		'UPGRADE_WARNING: Couldn't resolve default property of object xlApp.Workbooks().Sheets().Name. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		xlApp.Workbooks(InName).Sheets("Sheet1").Name = SheetName
		'nira E 2014/4/10
		'MsgBox "Workbooks(InName).Sheets(Sheet1).Name=" & SheetName
		
		'ｼｰﾄ番号がﾏｲﾅｽ値のとき、ｼｰﾄ数を取得。
		If SheetNo < 0 Then
			SheetNo = xlApp.Workbooks(OutNameTemp).Sheets.Count
		End If
		'MsgBox "SheetNo=" & SheetNo
		
		'ｼｰﾄ番号の後に移動。
		'    xlApp.Sheets("Sheet1").Move After:=xlApp.Workbooks(OutName).Sheets(SheetNo) 20100913 whl 削除
		'MsgBox "Move START"
		'    If xlApp.Version <= 14 Then
		'        xlApp.Workbooks(InName).Sheets("Sheet1").Move After:=xlApp.Workbooks(OutNameTemp).Sheets(SheetNo) '20100913 whl追加
		'UPGRADE_WARNING: Couldn't resolve default property of object xlApp.Workbooks().Sheets().Move. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		xlApp.Workbooks(InName).Sheets(SheetName).Move(After:=xlApp.Workbooks(OutNameTemp).Sheets(SheetNo)) '20100913 whl追加
		'    Else
		'MsgBox "Move Activate"
		'        xlApp.Workbooks(InName).Activate
		'MsgBox "Move Select"
		'        xlApp.Workbooks(InName).Sheets(SheetName).Select
		'MsgBox "Move Move"
		'        xlApp.Workbooks(InName).Sheets(SheetName).Move After:=xlApp.Workbooks(OutNameTemp).Sheets(SheetNo) '20100913 whl追加
		'    End If
		SheetNo = SheetNo + 1
		
		'ｼｰﾄ名を変更。
		xlApp.Workbooks(OutNameTemp).Activate()
		'MsgBox "SheetName SET Select"
		'xlApp.Sheets(SheetNo).Select
		'MsgBox "SheetName SET SheetName変更前=" & SheetName
		'xlApp.Sheets(SheetNo).Name = SheetName
		'xlApp.Workbooks(OutNameTemp).Sheets(SheetNo).Name = SheetName
		'MsgBox "SheetName SET SheetName変更後=" & xlApp.Sheets(SheetNo).Name
		
		'入力Workbookを保存せず閉じる。
		xlApp.Workbooks(InName).Activate()
		xlApp.Workbooks(InName).Close(SaveChanges:=False)
		'MsgBox "InName Close"
		
		'ｱﾗｰﾄﾎﾞｯｸｽを非表示。
		xlApp.DisplayAlerts = False
		'出力Workbookを保存。
		xlApp.Workbooks(OutNameTemp).Activate()
		FilePathName = OutFilePath & OutNameTemp
		'MsgBox "FilePathName SaveAs=" & FilePathName
		xlApp.Workbooks(OutNameTemp).SaveAs(FileName:=FilePathName)
		
		'Workbookを閉じる。
		xlApp.Workbooks(OutNameTemp).Close()
		'MsgBox "OutNameTemp Close"
		'Quitﾒｿｯﾄﾞを使ってExcelを終了。
		xlApp.Quit()
		
		'ｵﾌﾞｼﾞｪｸﾄを解放。
		'UPGRADE_NOTE: Object xlApp may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		xlApp = Nothing
		
	End Sub
	
	Sub 新規Bookを作成(ByRef FilePath As String, ByRef OutName As String)
		Dim FilePathName As String
		Dim xlApp As Microsoft.Office.Interop.Excel.Application
		Dim xlBook As Microsoft.Office.Interop.Excel.Workbook
		
		xlApp = New Microsoft.Office.Interop.Excel.Application
		xlBook = xlApp.Workbooks.Add
		
		'Workbookを保存。
		'20110330 nakagawa 追加 start
		'    If xlApp.Version > 11 Then
		'        OutName = OutNameToXlsx(OutName)
		'    End If
		'20110330 nakagawa 追加 end
		FilePathName = FilePath & OutName
		
		If CDbl(xlApp.Version) < 12 Then
			xlBook.SaveAs(FileName:=FilePathName)
		Else
			Call ChengeFormatToXlsx(FilePathName, xlApp)
		End If
		
		'Workbookを閉じる。
		xlBook.Close()
		'Quitﾒｿｯﾄﾞを使ってExcelを終了。
		xlApp.Quit()
		
		'ｵﾌﾞｼﾞｪｸﾄを解放。
		'UPGRADE_NOTE: Object xlApp may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		xlApp = Nothing
		'UPGRADE_NOTE: Object xlBook may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		xlBook = Nothing
		
	End Sub
	
	Sub 標準Sheetを削除(ByRef FilePath As String, ByRef OutName As String)
		Dim i As Short
		Dim FilePathName As String
		Dim xlApp As Microsoft.Office.Interop.Excel.Application
		
		xlApp = New Microsoft.Office.Interop.Excel.Application
		
		'Workbookを開く。
		If CDbl(xlApp.Version) < 12 Then
			FilePathName = FilePath & OutName
		Else
			FilePathName = FilePath & OutName & "x"
		End If
		xlApp.Workbooks.Open(FileName:=FilePathName)
		
		If CDbl(xlApp.Version) < 12 Then
			For i = xlApp.Workbooks(OutName).Sheets.Count To 1 Step -1
				'UPGRADE_WARNING: Couldn't resolve default property of object xlApp.Sheets().Name. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				If InStr(xlApp.Sheets(i).Name, "Sheet") Then
					'ｱﾗｰﾄﾎﾞｯｸｽを非表示。
					xlApp.DisplayAlerts = False
					'ｼｰﾄを選択し削除。
					'UPGRADE_WARNING: Couldn't resolve default property of object xlApp.Sheets().Select. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					xlApp.Sheets(i).Select()
					'UPGRADE_WARNING: Couldn't resolve default property of object xlApp.Sheets().Delete. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					xlApp.Sheets(i).Delete()
				End If
			Next i
		Else
			For i = xlApp.Worksheets.Count To 1 Step -1
				'MsgBox "i=" & i & "         xlApp.Sheets(i).Name=" & xlApp.Sheets(i).Name
				'UPGRADE_WARNING: Couldn't resolve default property of object xlApp.Sheets().Name. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				If InStr(xlApp.Sheets(i).Name, "Sheet") Then
					'ｱﾗｰﾄﾎﾞｯｸｽを非表示。
					xlApp.DisplayAlerts = False
					'ｼｰﾄを選択し削除。
					'UPGRADE_WARNING: Couldn't resolve default property of object xlApp.Sheets().Select. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					xlApp.Sheets(i).Select()
					'UPGRADE_WARNING: Couldn't resolve default property of object xlApp.Sheets().Delete. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					xlApp.Sheets(i).Delete()
				End If
			Next i
		End If
		
		'MsgBox "Select"
		'最初のｼｰﾄを選択。
		'UPGRADE_WARNING: Couldn't resolve default property of object xlApp.Sheets().Select. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		xlApp.Sheets(1).Select()
		
		'ｱﾗｰﾄﾎﾞｯｸｽを非表示。
		xlApp.DisplayAlerts = False
		
		'Workbookを保存。
		'    xlApp.Workbooks(OutName).SaveAs FileName:=FilePathName 20100913 whl 削除
		
		'MsgBox "save"
		'20100913 whl追加 start
		If CDbl(xlApp.Version) < 12 Then
			xlApp.Workbooks(OutName).SaveAs(FileName:=FilePathName)
		Else
			Call ChengeFormatToXls(FilePathName, xlApp)
		End If
		'20100913 whl追加 end
		
		'MsgBox "Close"
		'Workbookを閉じる。
		xlApp.Workbooks.Close()
		
		'20100913 whl追加 start
		Dim Fsys As Object
		If CDbl(xlApp.Version) > 11 Then
			
			FilePathName = FilePathName & "x"
			Fsys = CreateObject("Scripting.FileSystemObject")
			'UPGRADE_WARNING: Couldn't resolve default property of object Fsys.DeleteFile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			Fsys.DeleteFile(FilePathName, True)
		End If
		'20100913 whl追加 end
		
		'Quitﾒｿｯﾄﾞを使ってExcelを終了。
		xlApp.Quit()
		
		'ｵﾌﾞｼﾞｪｸﾄを解放。
		'UPGRADE_NOTE: Object xlApp may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		xlApp = Nothing
		
	End Sub
	
	Sub 名前を付けて保存(ByRef InFilePath As String, ByRef InName As String, ByRef OutFilePath As String, ByRef OutName As String, ByRef SheetName As String)
		Dim FilePathName As String
		Dim xlApp As Microsoft.Office.Interop.Excel.Application
		
		xlApp = New Microsoft.Office.Interop.Excel.Application
		
		'Workbookを開く。
		FilePathName = InFilePath & InName
		xlApp.Workbooks.Open(FileName:=FilePathName)
		
		'ｼｰﾄ名を変更。
		'UPGRADE_WARNING: Couldn't resolve default property of object xlApp.Sheets().Select. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		xlApp.Sheets(1).Select()
		'UPGRADE_WARNING: Couldn't resolve default property of object xlApp.Sheets().Name. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		xlApp.Sheets(1).Name = SheetName
		
		'ｱﾗｰﾄﾎﾞｯｸｽを非表示。
		xlApp.DisplayAlerts = False
		'Workbookを保存。
		FilePathName = OutFilePath & OutName
		
		'    xlApp.Workbooks(InName).SaveAs FileName:=FilePathName, FileFormat:=xlNormal 20100913 whl 削除
		
		'20100913 whl 追加 start
		If CDbl(xlApp.Version) < 12 Then
			xlApp.Workbooks(InName).SaveAs(FileName:=FilePathName, FileFormat:=Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal)
		Else
			Call ChengeFormatToXlsx(FilePathName, xlApp)
		End If
		'20100913 whl 追加 end
		
		'Quitﾒｿｯﾄﾞを使ってExcelを終了。
		xlApp.Quit()
		
		'ｵﾌﾞｼﾞｪｸﾄを解放。
		'UPGRADE_NOTE: Object xlApp may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		xlApp = Nothing
		
	End Sub
	
	'20100913 whl追加 start
	Function OutNameToXlsx(ByRef OutName As String) As String
		
		Dim Postfix As String
		Postfix = Mid(OutName, InStr(OutName, ".") + 1)
		
		If UCase(Postfix) = UCase("xls") Then
			OutNameToXlsx = OutName & "x"
		Else
			OutNameToXlsx = OutName
		End If
		
	End Function
	
	Function FilePathNameToXls(ByRef OutName As String) As String
		
		OutName = Mid(OutName, 1, InStr(OutName, "."))
		
		OutName = OutName & "xls"
		FilePathNameToXls = OutName
		
	End Function
	
	Function ChengeFormatToXlsx(ByRef FilePathName As String, ByRef xlApp As Microsoft.Office.Interop.Excel.Application) As Object
		
		Trim(FilePathName)
		FilePathName = FilePathName & "x"
		
		xlApp.ActiveWorkbook.SaveAs(FileName:=FilePathName, FileFormat:=Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook, CreateBackup:=False)
		
	End Function
	
	Function ChengeFormatToXls(ByRef FilePathName As String, ByRef xlApp As Microsoft.Office.Interop.Excel.Application) As Object
		
		FilePathName = FilePathNameToXls(FilePathName)
		
		xlApp.ActiveWorkbook.SaveAs(FileName:=FilePathName, FileFormat:=Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel8, Password:="", WriteResPassword:="", ReadOnlyRecommended:=False, CreateBackup:=False)
		
	End Function
	'20100913 whl追加 end
End Module