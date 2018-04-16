Option Strict Off
Option Explicit On
Module subRegEditProfiles
	
	Declare Function RegOpenKey Lib "advapi32.dll"  Alias "RegOpenKeyA"(ByVal hKey As Integer, ByVal lpSubKey As String, ByRef phkResult As Integer) As Integer
	Declare Function RegOpenKeyEx Lib "advapi32.dll"  Alias "RegOpenKeyExA"(ByVal hKey As Integer, ByVal lpSubKey As String, ByVal ulOptions As Integer, ByVal samDesired As Integer, ByRef phkResult As Integer) As Integer
	Declare Function RegSetValue Lib "advapi32.dll"  Alias "RegSetValueA"(ByVal hKey As Integer, ByVal lpSubKey As String, ByVal dwType As Integer, ByVal lpData As String, ByVal cbData As Integer) As Integer
	'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
    Declare Function RegSetValueEx Lib "advapi32.dll" Alias "RegSetValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal Reserved As Integer, ByVal dwType As Integer, ByRef lpData As Object, ByVal cbData As Integer) As Integer
	Declare Function RegQueryValue Lib "advapi32.dll"  Alias "RegQueryValueA"(ByVal hKey As Integer, ByVal lpSubKey As String, ByVal lpValue As String, ByRef lpcbValue As Integer) As Integer
	'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
    Declare Function RegQueryValueEx Lib "advapi32.dll" Alias "RegQueryValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByRef lpData As Object, ByRef lpcbData As Integer) As Integer
	Declare Function RegCloseKey Lib "advapi32.dll" (ByVal hKey As Integer) As Integer
	Declare Function RegCreateKey Lib "advapi32.dll"  Alias "RegCreateKeyA"(ByVal hKey As Integer, ByVal lpSubKey As String, ByRef phkResult As Integer) As Integer
	Declare Function RegDeleteKey Lib "advapi32.dll"  Alias "RegDeleteKeyA"(ByVal hKey As Integer, ByVal lpSubKey As String) As Integer
	Declare Function RegDeleteValue Lib "advapi32.dll"  Alias "RegDeleteValueA"(ByVal hKey As Integer, ByVal lpValueName As String) As Integer
	
	Public Const HKEY_CLASSES_ROOT As Integer = &H80000000
	Public Const HKEY_CURRENT_CONFIG As Integer = &H80000005
	Public Const HKEY_CURRENT_USER As Integer = &H80000001
	Public Const HKEY_DYN_DATA As Integer = &H80000006
	Public Const HKEY_LOCAL_MACHINE As Integer = &H80000002
	Public Const HKEY_PERFORMANCE_DATA As Integer = &H80000004
	Public Const HKEY_USERS As Integer = &H80000003
	
	Public Const REG_NONE As Short = 0
	Public Const REG_SZ As Short = 1
	Public Const REG_EXPAND_SZ As Short = 2
	Public Const REG_BINARY As Short = 3
	Public Const REG_DWORD As Short = 4
	Public Const REG_DWORD_LITTLE_ENDIAN As Short = 4
	Public Const REG_DWORD_BIG_ENDIAN As Short = 5
	Public Const REG_LINK As Short = 6
	Public Const REG_MULTI_SZ As Short = 7
	Public Const REG_RESOURCE_LIST As Short = 8
	Public Const REG_FULL_RESOURCE_DESCRIPTOR As Short = 9
	Public Const REG_RESOURCE_REQUIREMENTS_LIST As Short = 10
#If WinNT Then
	'UPGRADE_NOTE: #If #EndIf block was not upgraded because the expression WinNT did not evaluate to True or was not evaluated. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="27EE2C3C-05AF-4C04-B2AF-657B4FB6B5FC"'
	Public Const KEY_EVENT = &H1
	Public Const KEY_NOTIFY = &H10
	Public Const KEY_QUERY_VALUE = &H1
	Public Const KEY_SET_VALUE = &H2
	Public Const KEY_CREATE_SUB_KEY = &H4
	Public Const KEY_ENUMERATE_SUB_KEYS = &H8
	Public Const KEY_CREATE_LINK = &H20
	Public Const KEY_READ = ((STANDARD_RIGHTS_READ Or KEY_QUERY_VALUE Or KEY_ENUMERATE_SUB_KEYS Or KEY_NOTIFY) And (Not SYNCHRONIZE))
	Public Const KEY_EXECUTE = (KEY_READ)
	Public Const KEY_WRITE = ((STANDARD_RIGHTS_WRITE Or KEY_SET_VALUE Or KEY_CREATE_SUB_KEY) And (Not SYNCHRONIZE))
	Public Const KEY_ALL_ACCESS = ((STANDARD_RIGHTS_ALL Or KEY_QUERY_VALUE Or KEY_SET_VALUE Or KEY_CREATE_SUB_KEY Or KEY_ENUMERATE_SUB_KEYS Or KEY_NOTIFY Or KEY_CREATE_LINK) And (Not SYNCHRONIZE))
#End If
	
	Const ERROR_SUCCESS As Short = 0
	Const REGAGENT_NOKEY As Short = -1002
	
	Public glStatus As Integer
	
	Function setKeyStringValue(ByVal plKey As Integer, ByVal psKey As String, ByVal psSubKey As String, ByVal psKeyValue As String) As Integer
		
		Dim llKeyID As Integer
		glStatus = ERROR_SUCCESS ' Sucess
		
		' Judgement parament
		If Len(psKey) = 0 Then
			glStatus = REGAGENT_NOKEY
            ' 2015/07/22 Nakagawa Edit Start
            setKeyStringValue = glStatus
            ' 2015/07/22 Nakagawa Edit End
            Exit Function
		End If
		
		' Open key
		glStatus = RegOpenKey(plKey, psKey, llKeyID)
		If glStatus = ERROR_SUCCESS Then 'Open sucess
			If Len(psKeyValue) = 0 Then 'Is NULL?
				glStatus = RegSetValueEx(llKeyID, psSubKey, 0, REG_SZ, 0, 0)
			Else ' Is normal
				glStatus = RegSetValueEx(llKeyID, psSubKey, 0, REG_SZ, psKeyValue, Len(psKeyValue) + 1)
			End If
			glStatus = RegCloseKey(llKeyID)
		Else
			glStatus = REGAGENT_NOKEY
		End If
		
		setKeyStringValue = glStatus
		
	End Function
	
	Function setRegEditProfiles(ByRef profileKey As String, ByRef profileValue As String) As Integer
		Dim regEditProfiles As String
		Dim status As Integer
		
		regEditProfiles = FunGetRegEditProfiles()
		status = setKeyStringValue(HKEY_CURRENT_USER, regEditProfiles & "Default\Config", profileKey, profileValue)
		
		setRegEditProfiles = status
		
	End Function
End Module