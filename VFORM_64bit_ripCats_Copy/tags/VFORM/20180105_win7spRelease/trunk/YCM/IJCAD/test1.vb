'Module UtilWindow
'    Private Declare Function FindWindowW Lib "user32" ( _
'        ByVal lpClassName As String, _
'        ByVal lpWindowName As String) As IntPtr
'    Private Declare Function SetForegroundWindow Lib "user32" (ByVal hWnd As IntPtr) As Boolean
'    Public Declare Function GetWindowTextW Lib "user32" (ByVal hwd As Long, ByVal buf As String, ByVal bln As Long) As Long


'    '<System.Runtime.InteropServices.DllImport( _
'    '    "user32.dll", _
'    '    CharSet:=System.Runtime.InteropServices.CharSet.Auto)> _
'    'Shared Function FindWindow( _
'    '    ByVal lpClassName As String, _
'    '    ByVal lpWindowName As String) As IntPtr
'    'End Function

'    '<System.Runtime.InteropServices.DllImport("user32.dll")> _
'    'Shared Function SetForegroundWindow(ByVal hWnd As IntPtr) As Boolean
'    'End Function

'    ''' <summary>
'    ''' 指定したウィンドウをアクティブにする
'    ''' </summary>
'    ''' <param name="winTitle">
'    ''' アクティブにするウィンドウのタイトル</param>
'    Public Sub ActivateWindow(ByVal winTitle As String)
'        'Dim hWnd As IntPtr = FindWindowW("ICAD.Application", Nothing) '--winTitle)
'        Dim hWnd As IntPtr = FindWindowW(Nothing, Nothing) '--winTitle)
'        If Not hWnd.Equals(IntPtr.Zero) Then
'            SetForegroundWindow(hWnd)
'        End If

'        Dim cap As String, i As Long
'        If hWnd <> 0 Then
'            cap = Space(200)
'            i = GetWindowTextW(hWnd, cap, Len(cap))
'            '    If i > 0 Then cap = Trim(Left(cap, InStr(cap, Chr(0)) - 1))
'            '    MsgBox(cap)
'            'Else
'            '    MsgBox("エラー")
'        End If
'    End Sub 'ActivateWindow

'    ''フォームのLoadイベントハンドラ
'    'Private Sub Form1_Load(ByVal sender As Object, _
'    '        ByVal e As System.EventArgs) Handles MyBase.Load
'    '    ToolTip1.SetToolTip(Button1, "こんにちは" + vbCrLf + "さようなら")

'End Module

Option Strict On

'==========このファイルはテスト用================
' 以下の名前空間をインポートする
Imports System.Diagnostics
Imports System.Runtime.InteropServices

Public Class MyProcess

    <DllImport("USER32.DLL", CharSet:=CharSet.Auto)> _
    Private Shared Function ShowWindow( _
        ByVal hWnd As System.IntPtr, _
        ByVal nCmdShow As Integer) As Integer
    End Function

    <DllImport("USER32.DLL", CharSet:=CharSet.Auto)> _
    Private Shared Function SetForegroundWindow( _
        ByVal hWnd As System.IntPtr) As Boolean
    End Function

    Private Const SW_NORMAL As Integer = 1

    ''' ------------------------------------------------------------------------------------
    ''' <summary>
    '''     同名のプロセスが起動中の場合、メイン ウィンドウをアクティブにします。</summary>
    ''' <returns>
    '''     既に起動中であれば True。それ以外は False。</returns>
    ''' ------------------------------------------------------------------------------------
    Public Shared Function ShowPrevProcess() As Boolean
        Dim hThisProcess As Process = Process.GetCurrentProcess()
        Dim hProcesses As Process() = Process.GetProcessesByName(hThisProcess.ProcessName)
        Dim iThisProcessId As Integer = hThisProcess.Id

        For Each hProcess As Process In hProcesses
            If hProcess.Id <> iThisProcessId Then
                Call ShowWindow(hProcess.MainWindowHandle, SW_NORMAL)
                Call SetForegroundWindow(hProcess.MainWindowHandle)
                Return True
            End If
        Next hProcess

        Return False
    End Function

End Class
