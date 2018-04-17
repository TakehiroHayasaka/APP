Module CommonMod
    Public CamParam As Object
    Public F0 As Double = 10000.0
    Public PointNum As Integer = 0
    Public ImageNum As Integer = 0
    Public CameraNum As Integer = 0 'SUURI ADD 20150112
    Public CamGaibuHensu As Integer = 6
    Public CamNaibuHensu As Integer = 3
    Public PointHensu As Integer = 3
    Public KoteiHensu As Integer = 7
    Public sw As System.Diagnostics.Stopwatch
    'add by SUSANO
    Public Declare Ansi Function GetPrivateProfileInt Lib "kernel32.dll" Alias "GetPrivateProfileIntA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal nDefault As Integer, ByVal lpFileName As String) As Integer

    Public Sub JikanMonStart()
        sw = New System.Diagnostics.Stopwatch
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "JikanMonitor.mon",
                                      "********JikanMonitorStart*******" & Now.ToString & vbNewLine, True)
    End Sub
    Public Sub StopWatchResetStart()
        sw.Reset()
        sw.Start()
    End Sub
    Public Sub JikanMonitor(ByVal strMon As String)
        sw.Stop()
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "JikanMonitor.mon",
                                       strMon & sw.Elapsed.TotalSeconds & "秒" & vbNewLine, True)
    End Sub

    Public Sub JikanMonEnd()
        sw = Nothing
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "JikanMonitor.mon",
                                      "********JikanMonitorEnd*******" & Now.ToString & vbNewLine, True)
    End Sub

End Module
