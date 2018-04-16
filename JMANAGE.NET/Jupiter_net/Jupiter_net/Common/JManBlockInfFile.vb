Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Windows.Forms

'---------------------------------------------------------------------------------------------------
'Block.infのクラス
'---------------------------------------------------------------------------------------------------
Public Class JManBlockInfRecord
    Implements System.IComparable

    'ブロック名
    Public _blockName As String = ""
    'ブロックコード構造
    Public _blockCodeKozo As String = ""
    'ブロックコード桁/桁間
    Public _blockCodeKeta As String = ""
    'ブロックコード横断線
    Public _blockCodeOudan As String = ""
    'ブロックコード縦断線
    Public _blockCodeJudan As String = ""
    'ブロックコード種別
    Public _blockCodeKind As String = ""
    'ブロックコード補助1
    Public _blockCodeHojo1 As String = ""
    'ブロックコード補助2
    Public _blockCodeHojo2 As String = ""
    'ブロックコード補助3
    Public _blockCodeHojo3 As String = ""
    'ブロックコード補助4
    Public _blockCodeHojo4 As String = ""
    '部材数
    Public _pieceNum As Integer = 0

    Public Function initialize(text As String) As Integer

        initialize = 0

        Dim re As Regex
        re = New Regex("\s+")
        Dim texts As String() = re.Split(text)
        If texts.Count <> 19 Then
            initialize = 1
            Exit Function
        End If

        Me._blockName = texts(1)
        Me._blockCodeKozo = texts(2)
        Me._blockCodeKeta = texts(4)
        Me._blockCodeOudan = texts(5)
        Me._blockCodeJudan = texts(6)
        Me._blockCodeKind = texts(7)
        Me._blockCodeHojo1 = texts(9)
        Me._blockCodeHojo2 = texts(10)
        Me._blockCodeHojo3 = texts(11)
        Me._blockCodeHojo4 = texts(12)

        If Integer.TryParse(texts(16), Me._pieceNum) <> True Then
            initialize = 1
            Exit Function
        End If

    End Function

    Public Function toStringRecord() As String

        toStringRecord = "$ "
        toStringRecord += Me._blockName + " "
        toStringRecord += Me._blockCodeKozo + " "
        toStringRecord += Me._blockName + " "
        toStringRecord += Me._blockCodeKeta + " "
        toStringRecord += Me._blockCodeOudan + " "
        toStringRecord += Me._blockCodeJudan + " "
        toStringRecord += Me._blockCodeKind + " - "
        toStringRecord += Me._blockCodeHojo1 + " "
        toStringRecord += Me._blockCodeHojo2 + " "
        toStringRecord += Me._blockCodeHojo3 + " "
        toStringRecord += Me._blockCodeHojo4 + " 0 0 0 "
        toStringRecord += Me._pieceNum.ToString() + " 0 0"

    End Function

    Public Function CompareTo(bir As Object) As Integer Implements System.IComparable.CompareTo

        'Nothingより大きい
        If bir Is Nothing Then
            Return 1
        End If

        Return Me._blockName.CompareTo(bir._blockName)

    End Function

End Class

Public Class JManBlockInfFile

    Private _recordList As New ArrayList()

    Public Sub append(rec As JManBlockInfRecord)

        _recordList.Add(rec)

    End Sub

    Public Sub removeAt(index As Integer)

        _recordList.RemoveAt(index)

    End Sub

    Public Function getAt(index As Integer) As JManBlockInfRecord

        getAt = _recordList(index)

    End Function

    Public Function size() As Integer

        size = _recordList.Count

    End Function

    Public Sub clearRecord()

        _recordList.Clear()

    End Sub

    Public Function find(blockName As String) As Integer

        find = -1

        Dim i As Integer = 0
        For i = 0 To Me._recordList.Count - 1
            If blockName = Me._recordList(i)._blockName Then
                find = i
                Exit Function
            End If
        Next

    End Function

    Public Function load(fileName As String) As Integer

        load = 0

        Dim status As Integer = 0
        If System.IO.File.Exists(fileName) = True Then
            Dim blockInf As New StreamReader(fileName, Encoding.GetEncoding("Shift_JIS"))
            While (blockInf.Peek() >= 0)
                Dim lineText As String = blockInf.ReadLine().Trim()
                removeComment(lineText)
                If lineText = "" Then
                    Continue While
                End If

                Dim record As New JManBlockInfRecord()
                status = record.initialize(lineText)
                If status <> 0 Then
                    load = 2
                    Exit Function
                End If
                Me._recordList.Add(record)
            End While
            blockInf.Close()
        Else
            load = 1
        End If

        Me._recordList.Sort()

    End Function

    Public Function save(fileName As String) As Integer

        save = 0

        Dim i As Integer = 0
        Dim blockInfWriter As New StreamWriter(fileName, False, Encoding.GetEncoding("Shift_JIS"))
        For i = 0 To Me._recordList.Count - 1
            blockInfWriter.WriteLine(Me._recordList(i).toStringRecord())
        Next
        blockInfWriter.Close()

    End Function

    Public Sub deleteDefinedBlock(dmyBlockInf As JManBlockInfFile)

        Dim i As Integer = 0
        For i = 0 To dmyBlockInf._recordList.Count - 1
            Dim index As Integer = Me.find(dmyBlockInf._recordList(i)._blockName)
            If index <> -1 Then
                Me.removeAt(index)
            End If
        Next

    End Sub

    Public Sub deleteUndefinedBlock(orgBlockInf As JManBlockInfFile)

        Dim i As Integer = 0
        For i = 0 To Me._recordList.Count - 1
            Dim index As Integer = orgBlockInf.find(Me._recordList(i)._blockName)
            If index = -1 Then
                Me.removeAt(i)
                i = i - 1
            End If
        Next
    End Sub

    Public Sub combineRecord(inf1 As JManBlockInfFile, inf2 As JManBlockInfFile)

        Dim i As Integer = 0
        For i = 0 To inf1._recordList.Count - 1
            Me._recordList.Add(inf1._recordList(i))
        Next
        For i = 0 To inf2._recordList.Count - 1
            Me._recordList.Add(inf2._recordList(i))
        Next

        Me._recordList.Sort()
    End Sub

    Public Sub sortRecord()

        Me._recordList.Sort()

    End Sub

End Class
