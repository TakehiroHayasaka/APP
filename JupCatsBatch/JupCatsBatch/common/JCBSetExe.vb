Imports System.IO
Imports System.Text

Public Class JCBSetExeLoadFile

    Public _readList As New ArrayList         '読み込んでインスタンスにした要素を格納
    Public _buttonInfList As New ArrayList    'ボタンに関するリスト
    Public _numButton As Integer = 0          'ボタンの数

    '---------------------------------------------------------------------------------------------------
    '機能
    '   ファイルからEXEボタンに関する情報を読み込む
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

                    RelateInfo(line)

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

            'ボタンの番号ごとに配列に格納する
            MakeBottonInfList()
        Else
            LoadFile = 1
        End If

    End Function

    '---------------------------------------------------------------------------------------------------
    '機能
    '   ファイルから読み込んだテキストとボタンに関連する情報を結びつける
    '引数
    '   なし
    '戻り値
    '   なし
    '---------------------------------------------------------------------------------------------------
    Private Sub RelateInfo(str As String)
        'カンマ区切りで分割して配列に格納する
        Dim array As String() = str.Split(","c)
        Dim size As Integer = array.Length
        Dim info As New JCBExeButtonInfo

        '項目数は6つ以上必要
        If size >= 6 And TryParseToInteger(array(0)) <> False Then
            info._buttonNo = Integer.Parse(array(0))
            info._caption = array(1)
            info._process = array(2)
            info._logFileName = array(3)
            If TryParseToInteger(array(4)) <> False Then
                info._errorBehavior = Integer.Parse(array(4))
            End If
            info._exeName = array(5)

            For i As Integer = 6 To size - 1
                If array(i) = "" Then
                    Exit For
                End If
                info._argList.Add(array(i))
            Next

            _readList.Add(info)

            If (info._buttonNo > _numButton) Then
                _numButton = info._buttonNo
            End If
        End If

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   ボタンの種類ごとにインスタンスを配列に格納する
    '引数
    '   なし
    '戻り値
    '   なし
    '---------------------------------------------------------------------------------------------------
    Private Sub MakeBottonInfList()

        For i As Integer = 1 To _numButton
            Dim array As New ArrayList
            For j As Integer = 0 To _readList.Count - 1
                Dim info As JCBExeButtonInfo = _readList(j)
                'ボタンの番号が一致した際
                If info._buttonNo = i Then
                    array.Add(info)
                End If
            Next
            'ボタンの番号ごとに配列に格納する
            _buttonInfList.Add(array)
        Next

    End Sub

End Class

Public Class JCBExeButtonInfo

    Public _buttonNo As Integer = 0                'ボタンNo
    Public _caption As String = ""                 'ボタンキャプション
    Public _process As String = ""                 '処理内容
    Public _logFileName As String = ""             'ログファイル名
    Public _errorBehavior As Integer = 0           'エラー時の動作
    Public _exeName As String = ""                 '実行EXE
    Public _argList As New ArrayList               '実行ファイルの引数のリスト

End Class
