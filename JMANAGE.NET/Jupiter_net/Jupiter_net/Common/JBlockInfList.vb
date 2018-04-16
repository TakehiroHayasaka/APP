Option Strict Off
Option Explicit On
Friend Class JBlockInfList
	'==============================================================================
	'
	'  JBlockInfList
	'
	'==============================================================================
	
	'==============================================================================
	'==============================================================================
	Private Const m_IncrementNumber As Short = 10
	Private m_objAry() As JBlockInf
	Private m_intSize As Short
	Private m_lngIndex As Integer
	
	'==============================================================================
	'==============================================================================
	'UPGRADE_NOTE: Class_Initialize was upgraded to Class_Initialize_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
	Private Sub Class_Initialize_Renamed()
		ReDim m_objAry(0)
		m_intSize = 0
		m_lngIndex = 0
	End Sub
	Public Sub New()
		MyBase.New()
		Class_Initialize_Renamed()
	End Sub
	
	'==============================================================================
	'UPGRADE_NOTE: Class_Terminate was upgraded to Class_Terminate_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
	Private Sub Class_Terminate_Renamed()
		On Error Resume Next
		Erase m_objAry
	End Sub
	Protected Overrides Sub Finalize()
		Class_Terminate_Renamed()
		MyBase.Finalize()
	End Sub
	
	'==============================================================================
	'==============================================================================
	Public Function Size() As Short
		Size = m_intSize
	End Function
	
	'==============================================================================
	'==============================================================================
	Public Function At(ByRef intIndex As Short) As JBlockInf
		' インデクスが範囲外の場合は Nothing を返す
        If intIndex < 0 Or Size() <= intIndex Then
            ' 2015/07/22 Nakagawa Add Start
            At = New JBlockInf
            ' 2015/07/22 Nakagawa Add End
            Exit Function
        End If
		
		At = m_objAry(intIndex).Copy
	End Function
	
	'==============================================================================
	'==============================================================================
	Public Function Append(ByRef value As JBlockInf) As Short
		If UBound(m_objAry) <= m_intSize Then
			ReDim Preserve m_objAry(m_intSize + m_IncrementNumber)
		End If
		
		m_objAry(m_intSize) = value
        m_intSize = m_intSize + 1
        ' 2015/07/22 Nakagawa Add Start
        Append = 0
        ' 2015/07/22 Nakagawa Add End
    End Function
	
	'==============================================================================
	'==============================================================================
	Public Function Find(ByVal blk As String) As Short
		'見つからないときは-1を返す
		Find = -1
		Dim i As Short
		'UPGRADE_NOTE: str was upgraded to str_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
		Dim str_Renamed As String
		
		For i = 0 To m_intSize - 1
			str_Renamed = At(i).blockName
			If str_Renamed = blk Then
				Find = i
				Exit Function
			End If
			str_Renamed = ""
		Next 
	End Function
	
	'==============================================================================
	'==============================================================================
	Public Function Load(ByVal fileName As String) As Short
		Dim fnum As Short
		
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		Dim blk As String
		Dim buf As String
		Dim obj As New JBlockInf
		If Dir(fileName, FileAttribute.Normal) <> "" Then
			
			fnum = FreeFile
			FileOpen(fnum, fileName, OpenMode.Input)
			Do While Not EOF(fnum)
				
				buf = LineInput(fnum)
				
				blk = J_ChoiceString(buf, 1)
				
				Call obj.SetMember(blk, buf)
				Call Me.Append(obj.Copy)
			Loop 
			FileClose(fnum)
			
		End If
        ' 2015/07/22 Nakagawa Add Start
        Load = 0
        ' 2015/07/22 Nakagawa Add End
    End Function
End Class