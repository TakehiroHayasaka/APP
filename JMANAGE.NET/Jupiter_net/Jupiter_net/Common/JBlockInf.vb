Option Strict Off
Option Explicit On
Friend Class JBlockInf
	'==============================================================================
	'
	'  JBlockInf
	'
	'==============================================================================
	
	'==============================================================================
	'==============================================================================
	Private m_blockName As String
	Private m_line As String
	
	'==============================================================================
	'==============================================================================
	'UPGRADE_NOTE: Class_Initialize was upgraded to Class_Initialize_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
	Public Sub Class_Initialize_Renamed()
		On Error Resume Next
		m_blockName = ""
		m_line = ""
	End Sub
	Public Sub New()
		MyBase.New()
		Class_Initialize_Renamed()
	End Sub
	
	'==============================================================================
	'UPGRADE_NOTE: Class_Terminate was upgraded to Class_Terminate_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
	Public Sub Class_Terminate_Renamed()
		On Error Resume Next
	End Sub
	Protected Overrides Sub Finalize()
		Class_Terminate_Renamed()
		MyBase.Finalize()
	End Sub
	
	'==============================================================================
	'==============================================================================
	
	'==============================================================================
	'==============================================================================
	Public Property blockName() As String
		Get
			blockName = m_blockName
		End Get
		Set(ByVal Value As String)
			m_blockName = Value
		End Set
	End Property
	
	
	Public Property line() As String
		Get
			line = m_line
		End Get
		Set(ByVal Value As String)
			m_line = Value
		End Set
	End Property
	
	'==============================================================================
	Public Sub SetMember(ByRef blockName As String, ByRef line As String)
		m_blockName = blockName
		m_line = line
	End Sub
	
	'==============================================================================
	Public Sub Assign(ByRef blockInf As JBlockInf)
		m_blockName = blockInf.blockName()
		m_line = blockInf.line()
	End Sub
	Public Function Copy() As JBlockInf
		Copy = New JBlockInf
		Call Copy.Assign(Me)
	End Function
End Class