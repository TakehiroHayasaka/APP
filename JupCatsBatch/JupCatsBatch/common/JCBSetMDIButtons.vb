Imports System.IO
Imports System.Text

Public Class JCBSetMDIButtons
    Public _MdiButtonList As New ArrayList

    '---------------------------------------------------------------------------------------------------
    '機能
    '   ファイルからMDIボタンに関連する情報を読み込む
    '引数
    '   ファイル名
    '戻り値
    '   0   OK
    '   1   NO
    '---------------------------------------------------------------------------------------------------

    Public Function LoadFile(fileName As String) As Integer

        LoadFile = 0

        If System.IO.File.Exists(fileName) = True Then

            Try
                Dim file As New StreamReader(fileName, Encoding.GetEncoding("Shift_JIS"))
                While (file.Peek() >= 0)
                    Dim line As String = file.ReadLine().Trim()
                    RemoveComment(line)
                    If line = "" Then
                        Continue While
                    End If

                    Dim res As Integer = Me.RelateInf(line)

                End While
                file.Close()

            Catch ex As Exception
                'ファイルの読み込みに失敗した際
                Dim msg As String = "設定ファイル読み込み時にエラーが発生しました。" + Environment.NewLine + "設定ファイル[" + fileName + "]を確認してください。"
                MessageBox.Show(msg,
                                "CatarJupiter・CATSバッチシステム",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error)
                LoadFile = 1
                Throw
            End Try

        Else
            LoadFile = 1
        End If
    End Function

    Public Function Size() As Integer

        Return _MdiButtonList.Count

    End Function

    Public Sub ClearList()

        _MdiButtonList.Clear()

    End Sub

    'ファイルから読み込んだテキストとボタンに関連する情報を結びつける
    Public Function RelateInf(str As String) As Integer
        'カンマ区切りで分割して配列に格納する
        Dim array As String() = str.Split(","c)
        Dim size As Integer = array.Length
        Dim info As New JCBMDIButtonInfo

        If array(0) = "-" Then
            '区切り線
            info._buttonNo = -1

            _MdiButtonList.Add(info)

            Return 0

        ElseIf size >= 8 And TryParseToInteger(array(0)) <> False Then
            info._buttonNo = Integer.Parse(array(0))
            info._buttonCaption = array(1)
            info._buttonIcon = array(2)
            info._buttonTips = array(3)
            info._startingOperation = array(4)

            If TryParseToInteger(array(5)) <> False And TryParseToInteger(array(6)) <> False Then
                info._activationPositionX = Integer.Parse(array(5))
                info._activationPositionY = Integer.Parse(array(6))
            End If

            info._programFunction = array(7)

            For i As Integer = 8 To size - 1
                If array(i) = "" Then
                    Exit For
                End If
                info._argument.Add(array(i))
            Next

            _MdiButtonList.Add(info)

            Return 0
        Else
            Return 1
        End If

    End Function

End Class

Public Class JCBMDIButtonInfo

    Public _buttonNo As Integer = 0                'ボタンNo("-"の際に-1を代入で区切り線)
    Public _buttonCaption As String = ""           'ボタンキャプション
    Public _buttonIcon As String = ""              'ボタンアイコンファイル
    Public _buttonTips As String = ""              'ボタンTIPS
    Public _startingOperation As String = ""       '起動時の動作
    Public _activationPositionX As Integer = 0     '起動位置X
    Public _activationPositionY As Integer = 0     '起動位置Y
    Public _programFunction As String = ""         '実行プログラムまたは関数名
    Public _argument As New ArrayList              '引数(n)

End Class

