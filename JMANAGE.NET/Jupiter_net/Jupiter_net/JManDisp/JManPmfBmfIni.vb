Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Windows.Forms

'---------------------------------------------------------------------------------------------------
'JPMF.ini、JBMF.iniのクラス
'---------------------------------------------------------------------------------------------------
Public Class JManPmfBmfIniRecord

    Public _fileName As String = ""
    Public _dispName As String = ""

    Public Sub initialize(texts As String())

        _fileName = subStringEnd(texts(0), "=")

        Dim i As Integer = 0
        For i = 1 To texts.Count - 1
            _dispName += texts(i)
            If i <> texts.Count - 1 Then
                _dispName += " "
            End If
        Next

    End Sub

    Public Function isEqual(rec As JManPmfBmfIniRecord) As Boolean

        isEqual = False

        If Me._fileName = rec._fileName And Me._dispName = rec._dispName Then
            isEqual = True
        End If

    End Function

End Class

Public Class JManPmfBmfIni

    Public _fileHeader As String = ""     ' JPMF or JBMF
    Public _recordList As New ArrayList()

    Public Function load() As Integer

        load = 0

        If Me._fileHeader.ToUpper() <> "JPMF" And Me._fileHeader.ToUpper() <> "JBMF" Then
            load = 1
            Exit Function
        End If

        Dim fileName As String = FunGetEnviron().TrimEnd("\") + "\" + Me._fileHeader + ".ini"

        '連続している空白を一つにする。
        Dim re As Regex
        re = New Regex("\s+") '空白文字の連続で区切られる

        If System.IO.File.Exists(fileName) = True Then
            Dim iniFile As New StreamReader(fileName, Encoding.GetEncoding("Shift_JIS"))
            While (iniFile.Peek() >= 0)
                Dim lineText As String = iniFile.ReadLine().Trim()
                removeComment(lineText)
                If lineText = "" Then
                    Continue While
                End If

                Dim texts As String() = re.Split(lineText)
                If texts.Count = 1 Then
                ElseIf texts.Count >= 2 Then
                    Dim record As New JManPmfBmfIniRecord()
                    record.initialize(texts)
                    _recordList.Add(record)
                End If

            End While
            iniFile.Close()
        Else
            load = 1
        End If

    End Function

    Public Function save() As Integer

        save = 0

        If Me._fileHeader.ToUpper() <> "JPMF" And Me._fileHeader.ToUpper() <> "JBMF" Then
            save = 1
            Exit Function
        End If

        If Me._recordList.Count = 0 Then
            save = 1
            Exit Function
        End If

        Dim fileName As String = FunGetEnviron().TrimEnd("\") + "\" + Me._fileHeader + ".ini"

        Dim i As Integer = 0
        Dim fileWriter As New StreamWriter(fileName, False, Encoding.GetEncoding("Shift_JIS"))
        fileWriter.WriteLine("[TransTable]")
        fileWriter.WriteLine("count=" + _recordList.Count.ToString())
        For i = 0 To _recordList.Count - 1
            Dim text As String = "Trans" + (i + 1).ToString() + "=" + _recordList(i)._fileName + " " + _recordList(i)._dispName
            fileWriter.WriteLine(text)
        Next
        fileWriter.Close()


    End Function

    Public Function find(fileName As String) As Integer

        find = -1

        Dim i As Integer = 0
        For i = 0 To Me._recordList.Count - 1
            If fileName = _recordList(i)._fileName Then
                find = i
                Exit Function
            End If
        Next

    End Function

    Public Function isEqual(item As JManPmfBmfIni) As Boolean

        isEqual = True

        If Me._recordList.Count <> item._recordList.Count Then
            isEqual = False
            Exit Function
        End If

        Dim i As Integer = 0
        For i = 0 To item._recordList.Count - 1
            Dim index As Integer = Me.find(item._recordList(i)._fileName)
            If index = -1 Then
                isEqual = False
                Exit Function
            End If

            If Me._recordList(index).isEqual(item._recordList(i)) = False Then
                isEqual = False
                Exit Function
            End If
        Next

    End Function

    Public Function isOverlap(ByRef row1 As Integer, ByRef row2 As Integer) As Boolean

        isOverlap = False

        Dim i As Integer = 0
        Dim j As Integer = 0
        For i = 0 To Me._recordList.Count - 1
            For j = 0 To Me._recordList.Count - 1
                If i = j Then
                    Continue For
                End If
                If Me._recordList(i)._fileName.ToUpper() = Me._recordList(j)._fileName.ToUpper() Then
                    isOverlap = True
                    row1 = i + 1
                    row2 = j + 1
                    Exit Function
                End If
            Next
        Next

    End Function

End Class
