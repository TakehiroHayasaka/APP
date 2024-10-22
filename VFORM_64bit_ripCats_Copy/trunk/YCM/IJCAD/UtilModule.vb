﻿Imports System
Imports System.Text
Module UtilModule
    Declare Sub Sleep Lib "kernel32.dll" (ByVal lngMilliseconds As Long)
    '===============================================================================
    '機  能: 一定時間スリープする

    '        RumCommand等で実行されたコマンドが終了するまで待機するためのもの
    '戻り値: なし

    '引  数:
    '備  考: 内部的に空ループしているだけなので、実際に処理が終了したかどうかは不明
    '===============================================================================
    Public Function YMDdumy() As Integer
        On Error Resume Next
#If WPF Then
        DispatcherHelper.DoEvents()
#Else
        Windows.Forms.Application.DoEvents()
#End If
    End Function
    Public Sub YmdSleep()
        On Error Resume Next
#If WPF Then
        DispatcherHelper.DoEvents()
#Else
        Windows.Forms.Application.DoEvents()
#End If
        ''    Sleep (1)  ' 0.001秒

    End Sub
    Public Sub YmdSleepMsec(ByVal lMillisec As Long)
        On Error Resume Next
#If WPF Then
        DispatcherHelper.DoEvents()
#Else
        Windows.Forms.Application.DoEvents()
#End If

        'ljj sta 2014/01/15
        'Sleep(lMillisec) 

        'end

        ' System.Threading.Thread.Sleep(lMillisec)

    End Sub
    '--図面Open用
    Public Sub YmdSleepOpen()
        On Error Resume Next
#If WPF Then
        DispatcherHelper.DoEvents()
#Else
        Windows.Forms.Application.DoEvents()
#End If
        Sleep(500)  ' 0.5秒

        'System.Threading.Thread.Sleep(500)
    End Sub
    '--図面Active用
    Public Sub YmdSleepActive()
        On Error Resume Next
#If WPF Then
        DispatcherHelper.DoEvents()
#Else
        Windows.Forms.Application.DoEvents()
#End If
        Sleep(50)  ' 0.05秒

        'System.Threading.Thread.Sleep(50)
    End Sub

    '==============================================================================
    ' 機　能：指定された文字を数字と数字以外に分ける

    ' 引　数：

    '   strValue  [I/ ] 文字列
    '   iType     [ /O] =-1：文字列なし

    '                   =0：全て数字以外、=1：全て数字、=2：数字が先頭、=3：数字以外が先頭
    ' 戻り値：

    '   分割された文字列
    Public Function Util_GetDigitSplittedString( _
        ByVal strValue As String, _
        ByRef iType As Integer _
    ) As StringArray
        Dim strWork As String
        iType = -1
        strWork = strValue 'ユーザー入力頭文字（13.1.15）

        Util_GetDigitSplittedString = New StringArray
        Dim intIndex1 As Integer, intIndex2 As Integer
        Do
            intIndex1 = UtilString_FindFirstOf(strWork, "0123456789")
            intIndex2 = UtilString_FindFirstNotOf(strWork, "0123456789")
            If intIndex1 = 0 And intIndex2 = 0 Then      ' Len(strWork) は 0
                Exit Do
            End If
            If intIndex1 = 0 Then           ' strWork はすべて数字以外

                Util_GetDigitSplittedString.Append(strWork)
                If (iType = -1) Then iType = 0
                Exit Do
            ElseIf intIndex1 = 1 Then
                If intIndex2 = 0 Then       ' strWork はすべて数字

                    Util_GetDigitSplittedString.Append(strWork)
                    If (iType = -1) Then iType = 1
                    Exit Do
                Else                        ' strWork は数字が先頭
                    Util_GetDigitSplittedString.Append(Mid(strWork, 1, intIndex2 - 1))
                    strWork = Mid(strWork, intIndex2, Len(strWork) - intIndex2 + 1)
                    If (iType = -1) Then iType = 2
                End If
            Else                            ' strWork は数字以外が先頭
                Util_GetDigitSplittedString.Append(Mid(strWork, 1, intIndex1 - 1))
                strWork = Mid(strWork, intIndex1, Len(strWork) - intIndex1 + 1)
                If (iType = -1) Then iType = 3
            End If
        Loop

    End Function
    '==============================================================================
    Public Function UtilString_FindFirstOf(ByVal strValue As String, ByVal strFind As String) As Integer

        UtilString_FindFirstOf = 0

        Dim intLen As Integer
        intLen = Len(strFind)
        Dim intAt As Integer
        Dim i As Integer
        For i = 1 To intLen
            intAt = InStr(strValue, Mid(strFind, i, 1))
            If intAt <> 0 Then
                If UtilString_FindFirstOf = 0 Or intAt < UtilString_FindFirstOf Then
                    UtilString_FindFirstOf = intAt
                End If
            End If
        Next

    End Function

    '==============================================================================
    Public Function UtilString_FindFirstNotOf(ByVal strValue As String, ByVal strFind As String) As Integer

        UtilString_FindFirstNotOf = 0

        Dim intLen As Integer
        intLen = Len(strValue)
        Dim intAt As Integer
        Dim i As Integer
        For i = 1 To intLen
            intAt = InStr(strFind, Mid(strValue, i, 1))
            If intAt = 0 Then
                If UtilString_FindFirstNotOf = 0 Or i < UtilString_FindFirstNotOf Then
                    UtilString_FindFirstNotOf = i
                End If
            End If
        Next

    End Function

    Public Function isHankakuText(ByVal strTarget As String) As Boolean
        Dim num As Integer
        num = System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(strTarget)
        isHankakuText = (num = strTarget.Length)  ' =TRUE:半角文字

    End Function


End Module
