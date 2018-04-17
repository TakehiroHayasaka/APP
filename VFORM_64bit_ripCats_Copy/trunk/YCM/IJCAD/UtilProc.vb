Module UtilProc
    Private Declare Function SetForegroundWindow Lib "user32" (ByVal hWnd As IntPtr) As Boolean
    Private Declare Function ShowWindow Lib "user32" ( _
        ByVal hWnd As System.IntPtr, _
        ByVal nCmdShow As Integer) As Integer
    Private Const SW_NORMAL As Integer = 1

    Sub SetForegroundProc(ByVal strProcName As String)
        ' 実行中のすべてのプロセスを取得する
        Dim hProcesses As System.Diagnostics.Process() = System.Diagnostics.Process.GetProcesses()
        ' コンピュータ名を指定すると、別のコンピュータのプロセスが取得可能
        'hProcesses = System.Diagnostics.Process.GetProcesses("MachineName")

        Dim stPrompt As String = String.Empty
        Dim hWnd As IntPtr = 0
        ' 取得できたプロセスからプロセス名を取得する
        For Each hProcess As System.Diagnostics.Process In hProcesses
            stPrompt &= hProcess.ProcessName & System.Environment.NewLine
            If (hProcess.ProcessName = strProcName) Then  '--"icad") Then
                hWnd = hProcess.MainWindowHandle
            End If
        Next hProcess
        If (hWnd <> 0) Then
            Call ShowWindow(hWnd, SW_NORMAL)
            Call SetForegroundWindow(hWnd)
        End If
        ' 実行中のすべてのプロセス名を表示する
        ''MessageBox.Show(stPrompt)
    End Sub
End Module
