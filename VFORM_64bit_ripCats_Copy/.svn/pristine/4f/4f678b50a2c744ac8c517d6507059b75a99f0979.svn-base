Module mdlJupCatsLog


    ''' <summary>
    '''     ログファイルにコマンドステータスを出力する
    ''' </summary>
    ''' <param name="strAppName">引数1：処理名</param>
    ''' <param name="status">引数2：ステータス
    ''' "Start"=実行開始
    ''' "End"=正常終了
    ''' "Fail"=異常終了
    ''' "Cancel"=コマンドキャンセル </param>
    ''' <param name="sysName">引数3：システム(J:Jupiter、C:Cats、 V:VFORM)</param>
    ''' <remarks></remarks>
    Sub log_CommandStartus(ByVal strAppName As String, ByVal status As String, ByVal sysName As String)

        '日付を取得
        Dim tmpStr As String = ""
        Dim logDate As String = Now.ToString("yyyy/MM/dd HH:mm:ss")

        'ログファイルのフルパスを取得
        Dim logFileName As String = get_LogFileName(sysName)
        If logFileName = "" Then
            Return
        End If

        'ログファイルに出力する処理名を作成
        Dim logAppName As String = ""
        If sysName = "J" Then
            logAppName = "CastarJupiter (" + strAppName + ") "
        ElseIf sysName = "C" Then
            logAppName = "CATS (" + strAppName + ") "
        ElseIf sysName = "V" Then
            logAppName = "VFORM(" + strAppName + ") "
        Else
            Return
        End If

        'ログファイルに文字を出力
        Using m_file As IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(logFileName, True, System.Text.Encoding.GetEncoding("shift_jis"))
            tmpStr = vbCrLf + log_GetStatusStr(status) & logAppName & "<" & logDate & ">"
            m_file.WriteLine(tmpStr)
            m_file.Close()
        End Using

    End Sub

    ''' <summary>
    '''       ログファイルに文字列を出力する
    ''' </summary>
    ''' <param name="str">引数1：出力する文字列</param>
    ''' <param name="sysName">引数2：システム(J:Jupiter、C:Cats、V:VFORM)</param>
    ''' <remarks></remarks>
    Sub log_Print(ByVal str As String, ByVal sysName As String)

        'ログファイルのフルパスを取得
        Dim stCurrentDir As String = System.IO.Directory.GetCurrentDirectory()
        Dim logFileName As String = get_LogFileName(sysName)
        If logFileName = "" Then
            Return
        End If

        'ログファイルに文字を出力
        Using m_file As IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(logFileName, True, System.Text.Encoding.GetEncoding("shift_jis"))
            m_file.WriteLine(str)
            m_file.Close()
        End Using

    End Sub

    Private Function get_LogFileName(ByVal sysName As String) As String

        Dim stCurrentDir As String = System.IO.Directory.GetCurrentDirectory()
        Dim logFileName As String = ""
        If sysName = "J" Then
            logFileName = stCurrentDir.TrimEnd("\") + "\Jupiter.log"
        ElseIf sysName = "C" Then
            logFileName = stCurrentDir.TrimEnd("\") + "\Cats.log"
        ElseIf sysName = "V" Then
            logFileName = stCurrentDir.TrimEnd("\") + "\VFORM.log"
        Else
            Return ""
        End If

        Return logFileName

    End Function

    ''' <summary>
    ''' status: Start=0, End=1, fail=2
    ''' </summary>
    ''' <param name="status"></param>
    ''' <returns></returns>
    Private Function log_GetStatusStr(ByVal status As String) As String
        log_GetStatusStr = ""
        If status = "Start" Then
            log_GetStatusStr = "◎ ｺﾏﾝﾄﾞ実行開始 : "
        ElseIf status = "End" Then
            log_GetStatusStr = "◎ ｺﾏﾝﾄﾞ正常終了 : "
        ElseIf status = "Fail" Then
            log_GetStatusStr = "× ｺﾏﾝﾄﾞ異常終了 : "
        ElseIf status = "Cancel" Then
            log_GetStatusStr = "◎ ｺﾏﾝﾄﾞｷｬﾝｾﾙ終了 : "
        End If
    End Function
End Module
