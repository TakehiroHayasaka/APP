Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Windows.Forms

Public Class JManTscRecord

    Public _lspFileName As String = ""      'LSPファイル名
    Public _defName As String = ""          '定義名
    Public _runName As String = ""          '実行名
    Public _detail As String = ""           '詳細
    Public _isBatch As Integer = 0          '0：部材一括処理なし、1：部材一括処理あり

    Public Function initialize(text As String) As Integer

        Dim texts As String() = text.Split(",")
        If texts.Count < 4 Then
            Return 1
        End If

        Me._lspFileName = texts(0).Replace("""", "")
        Me._defName = texts(1).Replace("""", "")
        Me._runName = texts(2).Replace("""", "")
        Me._detail = texts(3).Replace("""", "")

        If Me._defName = "部材一括処理" Then
            Me._isBatch = 1
        End If

        Return 0

    End Function

    Public Function toStringRecord() As String

        toStringRecord = ""

        If Me._defName <> "" Then
            toStringRecord = Me._defName + "："
        End If
        If Me._runName <> "" Then
            toStringRecord += "『" + Me._runName + "』 "
        End If
        toStringRecord += "『" + Me._detail + "』"

    End Function

End Class

Public Class JManTscFile

    Public _fileName As String = ""
    Public _recordList As New ArrayList()
    Public _isBatch As Integer = 0          '0：部材一括処理なし、1：部材一括処理あり

    Public Function size() As Integer

        Return Me._recordList.Count()

    End Function

    Public Function load(fileName As String) As Integer

        Dim status As Integer = 0
        If System.IO.File.Exists(fileName) = True Then
            Dim tscFile As New StreamReader(fileName, Encoding.GetEncoding("Shift_JIS"))
            While (tscFile.Peek() >= 0)
                Dim lineText As String = tscFile.ReadLine().Trim()
                removeComment(lineText)
                If lineText = "" Then
                    Continue While
                End If

                Dim record As New JManTscRecord()
                status = record.initialize(lineText)
                If status <> 0 Then
                    Me._recordList.Clear()
                    Return 1
                End If

                If Me._isBatch = 0 Then
                    Me._isBatch = record._isBatch
                End If

                Me._recordList.Add(record)

            End While
            tscFile.Close()
        Else
            Return 1
        End If

        Return 0

    End Function

End Class
