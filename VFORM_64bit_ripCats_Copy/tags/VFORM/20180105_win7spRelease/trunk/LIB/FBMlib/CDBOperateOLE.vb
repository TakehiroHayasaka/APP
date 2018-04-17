'=================================================(C)Copyright YTI Inc., 2007====
'   パッケージ名　　: DB接続パッケージ
'   クラス名　　　　: DB接続クラス(OLE)
'   バージョン　　　:
'--------------------------------------------------------------------------------
'     作　成　者　　: kawasaki        2007.05.14
'     改　　　訂　　:
'================================================================================
'【Microsoft Access】
'.NET Framework Data Provider for OLE DB　 
Imports System.Data
Imports System.Data.OleDb
'Imports YTI.Kikan.Library.DB




Public Class CDBOperateOLE

    Implements IDBOperate

    'OLEDB接続
    Dim m_cnnOLEDB As OleDbConnection
    Dim m_cnnString As String
    '---Ins.Start-----------------------------------------(2008.04.18) kawasaki
    Dim m_strErrMsg As String
    '---Ins. End -----------------------------------------(2008.04.18) kawasaki

    'トランザクション処理
    Dim m_Transaction As OleDbTransaction
    Dim m_bIsExecTransaction As Boolean

    Dim m_daOleDB As OleDbDataAdapter
    Dim m_dsOleDB As DataSet


    '--------------------------------------------------------------------------------
    '   <機　能>
    '       DBとの接続を行う（Access）
    '
    '   <戻り値>
    '       True：接続成功
    '       False：接続失敗
    '
    '   <引　数>
    '       strDataSource   [I/ ]   データソース名（必須）
    '       strUserID       [I/ ]   ユーザ名(省略可)
    '       strPassword     [I/ ]   パスワード(省略可)
    '       strCnnString    [I/ ]   その他に必要な接続文字列(省略可)
    '--------------------------------------------------------------------------------
    Public Function Connect( _
            ByVal strDataSource As String, _
            Optional ByVal strUserID As String = "", _
            Optional ByVal strPassword As String = "", _
            Optional ByVal strCnnString As String = "" _
    ) As Boolean Implements IDBOperate.Connect


        Try

            '接続文字列作成
            Call ConnectionString(strDataSource, strUserID, strPassword, strCnnString)

            'コネクションオブジェクトを生成
            m_cnnOLEDB = New OleDbConnection

            'DBと接続
            m_cnnOLEDB.ConnectionString = m_cnnString
            m_cnnOLEDB.Open()

            Return True

        Catch
            m_cnnOLEDB = Nothing
            Return False
        End Try
    End Function

    '--------------------------------------------------------------------------------
    '   <機　能>
    '       AccessDBとの接続を切断する
    '
    '   <戻り値>
    '       なし
    '
    '   <引　数>
    '       なし
    '--------------------------------------------------------------------------------
    Public Sub DisConnect() Implements IDBOperate.DisConnect
        On Error GoTo Disconnect_Err

        If m_cnnOLEDB.State = ConnectionState.Open Then
            m_cnnOLEDB.Close()
        End If

Disconnect_Err:
        m_cnnOLEDB = Nothing
    End Sub

    '--------------------------------------------------------------------------------
    '   <機　能>
    '       DBとの接続文字列を作成する
    '
    '   <戻り値>
    '       なし
    '
    '   <引　数>
    '       strDataSource   [I/ ]   データソース名
    '       strUserID       [I/ ]   ユーザ名(省略可)
    '       strPassword     [I/ ]   パスワード(省略可)
    '       strCnnString    [I/ ]   その他に必要な接続文字列(省略可)
    '--------------------------------------------------------------------------------
    Public Sub ConnectionString( _
                    ByVal strDataSource As String, _
                    Optional ByVal strUserID As String = "", _
                    Optional ByVal strPassword As String = "", _
                    Optional ByVal strCnnString As String = "" _
    ) Implements IDBOperate.ConnectionString

        On Error Resume Next

        'm_cnnString = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
        '                      "Data Source=" & strDataSource & ";"
        If IntPtr.Size = 4 Then
            m_cnnString = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
                                  "Data Source=" & strDataSource & ";"
        ElseIf IntPtr.Size = 8 Then
            m_cnnString = "Provider=Microsoft.ACE.OLEDB.12.0;" & _
                              "Data Source=" & strDataSource & ";"
        End If

        If strUserID <> "" Then
            m_cnnString = m_cnnString & "User ID=" & strUserID & ";"
        End If

        If strPassword <> "" Then
            m_cnnString = m_cnnString & "Jet OLEDB:Database Password=" & strPassword & ";"
        End If

        If strCnnString <> "" Then
            m_cnnString = m_cnnString & strCnnString
        End If

    End Sub

    '--------------------------------------------------------------------------------
    '   <機　能>
    '       SQL文を実行する（レコードを返さない）
    '
    '   <戻り値>
    '       なし
    '
    '   <引　数>
    '       なし
    '--------------------------------------------------------------------------------
    Public Function ExecuteSQL( _
                    ByVal strSQL As String _
    ) As Long Implements IDBOperate.ExecuteSQL

        Dim cmd As New OleDbCommand

        Try

            cmd.Connection = m_cnnOLEDB
            cmd.CommandText = strSQL
            cmd.Transaction = m_Transaction

            Return cmd.ExecuteNonQuery()

        Catch ex As Exception
            MsgBox(ex.Message)
            Return -1
        End Try

    End Function

    '--------------------------------------------------------------------------------
    '   <機　能>
    '       SQL文を実行する（集計結果を返す）
    '
    '   <戻り値>
    '       集計結果
    '
    '   <引　数>
    '       strSQL  [I/ ]   SQL文
    '--------------------------------------------------------------------------------
    Function ExecuteSQL2( _
                    ByVal strSQL As String _
    ) As Object Implements IDBOperate.ExecuteSQL2

        Dim cmd As New OleDbCommand

        Try

            cmd.Connection = m_cnnOLEDB
            cmd.CommandText = strSQL
            cmd.Transaction = m_Transaction

            Return cmd.ExecuteScalar()

        Catch
            Return Nothing
        End Try
    End Function

    '--------------------------------------------------------------------------------
    '   <機　能>
    '       テーブル名、フィールド名を指定してそのレコード数を返す（Access）
    '
    '   <戻り値>
    '       レコード数
    '
    '   <引　数>
    '       strSelect   [I/ ]   フィールド名
    '       strTable    [I/ ]   対象テーブル
    '       strWhere    [I/ ]   抽出条件(省略可:省略時は全レコードを対象)
    '--------------------------------------------------------------------------------
    Public Function DoDCount( _
                ByVal strSelect As String, _
                ByVal strTable As String, _
                Optional ByVal strWhere As String = "" _
    ) As Long Implements IDBOperate.DoDCount

        Dim cmd As New OleDbCommand
        Dim strSql As String

        Try

            strSql = "SELECT COUNT(" & strSelect & ") FROM " & strTable
            If Len(strWhere) > 0 Then
                strSql = strSql & " WHERE " & strWhere
            End If

            cmd.Connection = m_cnnOLEDB
            cmd.CommandText = strSql
            cmd.Transaction = m_Transaction

            Return CLng(cmd.ExecuteScalar)

        Catch
            Return -1
        End Try
    End Function

    '--------------------------------------------------------------------------------
    '   <機　能>
    '       トランザクション開始処理
    '
    '   <戻り値>
    '
    '   <引　数>
    '
    '--------------------------------------------------------------------------------
    Public Sub BeginTrans() Implements IDBOperate.BeginTrans
        On Error Resume Next

        If m_bIsExecTransaction = False Then
            m_Transaction = m_cnnOLEDB.BeginTransaction()
            m_bIsExecTransaction = True
        End If
    End Sub

    '--------------------------------------------------------------------------------
    '   <機　能>
    '       コミット処理
    '
    '   <戻り値>
    '
    '   <引　数>
    '
    '--------------------------------------------------------------------------------
    Public Sub CommitTrans() Implements IDBOperate.CommitTrans
        On Error Resume Next

        If m_bIsExecTransaction = True Then
            m_Transaction.Commit()
            m_bIsExecTransaction = False
        End If
    End Sub

    '--------------------------------------------------------------------------------
    '   <機　能>
    '       ロールバック処理
    '
    '   <戻り値>
    '
    '   <引　数>
    '
    '--------------------------------------------------------------------------------
    Public Sub RollbackTrans() Implements IDBOperate.RollbackTrans
        On Error Resume Next

        If m_bIsExecTransaction = True Then
            m_Transaction.Rollback()
            m_bIsExecTransaction = False
        End If
    End Sub

    '--------------------------------------------------------------------------------
    '   <機　能>
    '       レコードの選択（Access）
    '
    '   <戻り値>
    '       DataReaderオブジェクト
    '
    '   <引　数>
    '       strSQL      [I/ ]   検索SQL
    '--------------------------------------------------------------------------------
    Public Function DoSelect( _
                    ByVal strSQL As String _
    ) As IDataReader Implements IDBOperate.DoSelect

        Dim cmd As New OleDbCommand

        Try

            cmd.Connection = m_cnnOLEDB
            cmd.CommandText = strSQL
            cmd.Transaction = m_Transaction

            Return cmd.ExecuteReader

        Catch
            Return Nothing
        End Try
    End Function

    '--------------------------------------------------------------------------------
    '   <機　能>
    '       レコードの削除（Access）
    '
    '   <戻り値>
    '       実行レコード数
    '
    '   <引　数>
    '       strTable    [I/ ]   テーブル名
    '       strWhere    [I/ ]   削除条件（省略時：全データ対象）
    '--------------------------------------------------------------------------------
    Public Function DoDelete( _
                ByVal strTable As String, _
                Optional ByVal strWhere As String = "" _
    ) As Long Implements IDBOperate.DoDelete

        Dim cmd As New OleDbCommand
        Dim strSQL As String

        Try

            'SQLを生成
            strSQL = "DELETE FROM " & strTable
            If Len(strWhere) > 0 Then
                strSQL = strSQL & " WHERE " & strWhere
            End If

            cmd.Connection = m_cnnOLEDB
            cmd.CommandText = strSQL
            cmd.Transaction = m_Transaction

            Return cmd.ExecuteNonQuery()

        Catch ex As Exception
            MsgBox(ex.Message)
            Return -1
        End Try
    End Function

    '--------------------------------------------------------------------------------
    '   <機　能>
    '       特定カラムの値取得
    '
    '   <戻り値>
    '       検索値
    '
    '   <引　数>
    '       strSelect   [I/ ]   検索項目
    '       strTable    [I/ ]   検索テーブル名
    '       strWhere    [I/ ]   検索条件（省略可）
    '--------------------------------------------------------------------------------
    Public Function DoDLookup( _
                    ByVal strSelect As String, _
                    ByVal strTable As String, _
                    Optional ByVal strWhere As String = "" _
    ) As Object Implements IDBOperate.DoDLookup

        'フィールドの内容を取得
        Dim strSQL As String
        Dim cmd As New OleDbCommand
        Dim dr As OleDbDataReader

        Try

            '検索SQLの作成
            strSQL = "SELECT " & strSelect & " FROM " & strTable
            If Len(strWhere) > 0 Then
                strSQL = "SELECT " & strSelect & " FROM " & strTable & " WHERE " & strWhere
            End If

            '値の検索
            cmd.Connection = m_cnnOLEDB
            cmd.CommandText = strSQL
            cmd.Transaction = m_Transaction
            dr = cmd.ExecuteReader

            dr.Read()
            Return dr(0)

        Catch
            Return Nothing
        End Try
    End Function

    '--------------------------------------------------------------------------------
    '   <機　能>
    '       レコードの更新（Access）
    '
    '   <戻り値>
    '       実行レコード数
    '
    '   <引　数>
    '       strFields() [I/ ]   更新フィールド名
    '       strTable    [I/ ]   テーブル名
    '       strValues() [I/ ]   更新値
    '       strWhere    [I/ ]   更新条件（省略時：全データ対象）
    '--------------------------------------------------------------------------------
    Public Function DoUpdate( _
                ByVal strFields() As String, _
                ByVal strTable As String, _
                ByVal vSetValues() As String, _
                Optional ByVal strWhere As String = "" _
    ) As Long Implements IDBOperate.DoUpdate

        Dim cmd As New OleDbCommand
        Dim strSQL As String
        Dim i As Integer

        Try

            'SQLを生成
            strSQL = "UPDATE " & strTable

            For i = 0 To UBound(strFields)
                If i = 0 Then
                    strSQL = strSQL & " SET "
                Else
                    strSQL = strSQL & ", "
                End If

                If vSetValues(i) = "" Then
                    strSQL = strSQL & strFields(i) & "=Null"
                Else
                    strSQL = strSQL & strFields(i) & "=" & vSetValues(i)
                End If
            Next

            If Len(strWhere) > 0 Then
                strSQL = strSQL & " WHERE " & strWhere
            End If

            cmd.Connection = m_cnnOLEDB
            cmd.CommandText = strSQL
            cmd.Transaction = m_Transaction

            Return cmd.ExecuteNonQuery()

        Catch
            Return -1
        End Try
    End Function

    '--------------------------------------------------------------------------------
    '   <機　能>
    '       レコードの登録
    '
    '   <戻り値>
    '       実行レコード数
    '
    '   <引　数>
    '       strFields() [I/ ]   登録フィールド名
    '       strTable    [I/ ]   テーブル名
    '       strValues() [I/ ]   登録値
    '--------------------------------------------------------------------------------
    Function DoInsert( _
                    ByVal strFields() As String, _
                    ByVal strTable As String, _
                    ByVal vSetValues() As String _
    ) As Long Implements IDBOperate.DoInsert

        Dim intFieldCnt As Integer
        Dim i As Integer
        Dim strInsert As String = ""
        Dim strValue As String = ""
        Dim cmd As New OleDbCommand

        Try

            intFieldCnt = UBound(strFields)
            For i = 0 To intFieldCnt
                Select Case i
                    Case 0
                        strInsert = "INSERT INTO " & strTable & " ("
                        strValue = " VALUES ("
                    Case Else
                        strInsert = strInsert & ","
                        strValue = strValue & ","
                End Select

                strInsert = strInsert & strFields(i)
                If vSetValues(i).Length = 0 Then
                    strValue = strValue & "null"
                Else
                    strValue = strValue & vSetValues(i)
                End If
            Next

            cmd.Connection = m_cnnOLEDB
            cmd.CommandText = strInsert & ")" & strValue & ")"
            cmd.Transaction = m_Transaction

            Return cmd.ExecuteNonQuery()

        Catch
            Return -1
        End Try
    End Function

    '--------------------------------------------------------------------------------
    '   <機　能>
    '       DataSetオブジェクトを取得する
    '
    '   <戻り値>
    '       DataSetオブジェクトの取得
    '
    '   <引　数>
    '       strSQL      [I/ ]   検索SQL
    '       strTable    [I/ ]   対象テーブル名
    '       bPropertyFlg[I/ ]   属性取得Flg
    '--------------------------------------------------------------------------------
    '---Rep.Start-----------------------------------------(2007.08.17) kawasaki
    'Public Function DoDataSet( _
    '                ByVal strSql As String, _
    '                ByVal strTable As String _
    ') As DataSet Implements IDBOperate.DoDataSet
    '-----------------------------------------------------
    Function DoDataSet( _
                    ByVal strSQL As String, _
                    Optional ByVal strTable As String = "", _
                    Optional ByVal bPropertyFlg As Boolean = False _
                    ) As DataSet Implements IDBOperate.DoDataSet
        '---Rep. End -----------------------------------------(2007.08.17) kawasaki
        Try

            m_daOleDB = Nothing
            m_dsOleDB = Nothing

            m_daOleDB = New OleDbDataAdapter(strSQL, m_cnnString)
            m_dsOleDB = New DataSet
            '---Ins.Start-----------------------------------------(2007.08.17) kawasaki
            '属性も取得する
            If bPropertyFlg = True Then
                m_daOleDB.MissingSchemaAction = MissingSchemaAction.AddWithKey
            End If
            '---Ins. End -----------------------------------------(2007.08.17) kawasaki
            If strTable.Length <> 0 Then
                m_daOleDB.Fill(m_dsOleDB, strTable)
                Return m_dsOleDB
            Else
                m_daOleDB.Fill(m_dsOleDB)
                Return m_dsOleDB
            End If

        Catch
            Return Nothing
        End Try
    End Function

    '--------------------------------------------------------------------------------
    '   <機　能>
    '       データ型による識別子追加
    '
    '   <戻り値>
    '       データ識別子付き文字列
    '
    '   <引　数>
    '       strCellValue    [I/ ]   データ
    '       strCellType     [I/ ]   データタイプ
    '--------------------------------------------------------------------------------
    Public Function checkDataType( _
                        ByVal strCellValue As String, _
                        ByVal strCellType As String _
    ) As String Implements IDBOperate.checkDataType

        If strCellValue.Length > 0 Then
            Select Case strCellType
                Case "String", "System.String"
                    Return "'" & strCellValue & "'"
                Case "DateTime", "System.DateTime"
                    Return "#" & strCellValue & "#"
                Case Else
                    Return strCellValue
            End Select
        Else
            Return ""
        End If

    End Function

    '--------------------------------------------------------------------------------
    '   <機　能>
    '       プロパティ値(DBコネクションオブジェクト)
    '
    '   <戻り値>
    '       なし
    '
    '   <引　数>
    '       なし
    '--------------------------------------------------------------------------------
    Public Property DbConnection() As System.Data.IDbConnection Implements IDBOperate.DbConnection
        Get
            Return m_cnnOLEDB
        End Get
        Set(ByVal value As System.Data.IDbConnection)
            m_cnnOLEDB = CType(value, OleDbConnection)
        End Set
    End Property


    '---Ins.Start-----------------------------------------(2007.09.28) kawasaki
    '--------------------------------------------------------------------------------
    '   <機　能>
    '       プロパティ値(DBトランザクションオブジェクト)
    '
    '   <戻り値>
    '       なし
    '
    '   <引　数>
    '       なし
    '--------------------------------------------------------------------------------
    Public Property DbTansaction() As System.Data.IDbTransaction Implements IDBOperate.DbTansaction
        Get
            Return m_Transaction
        End Get
        Set(ByVal value As System.Data.IDbTransaction)
            m_Transaction = CType(value, OleDbTransaction)
        End Set
    End Property
    '---Ins. End -----------------------------------------(2007.09.28) kawasaki

    '<!--2007/12/13 T44
    Public Function getConnectionObject() As Object Implements IDBOperate.getConnectionObject
        Return Me.m_cnnOLEDB
    End Function
    '-->

    '---Ins.Start-----------------------------------------(2008.02.29) kawasaki
    '--------------------------------------------------------------------------------
    '   <機　能>
    '       レコードの選択（パラメータ付）
    '
    '   <戻り値>
    '       DataReaderオブジェクト
    '
    '   <引　数>
    '       strSQL      [I/ ]   検索SQL
    '       strParaName()   [I/ ] パラメータ名
    '       strParaVal()    [I/ ] パラメータ値
    '--------------------------------------------------------------------------------
    Public Function DoSelectParameter(ByVal strSQL As String, ByVal strParaName() As String, ByVal objParaVal() As Object) As System.Data.IDataReader Implements IDBOperate.DoSelectParameter

        Dim cmd As New OleDbCommand

        Try

            cmd.Connection = m_cnnOLEDB
            cmd.CommandText = strSQL
            cmd.Transaction = m_Transaction
            For i As Integer = 0 To strParaName.Length - 1
                cmd.Parameters.Add(New OleDbParameter(strParaName(i), objParaVal(i)))
            Next

            Return cmd.ExecuteReader

        Catch
            Return Nothing
        End Try
    End Function
    '---Ins. End -----------------------------------------(2008.02.29) kawasaki

    '---Ins.Start-----------------------------------------(2008.04.18) kawasaki
    '--------------------------------------------------------------------------------
    '   <機　能>
    '       Blob型のデータInsert
    '
    '   <戻り値>
    '       True    正常終了
    '       False   異常終了
    '
    '   <引　数>
    '       strSQL          [I/ ]   実行SQL
    '       strParaName     [I/ ]   パラメータ名
    '       strParaVal()    [I/ ]   パラメータ値
    '--------------------------------------------------------------------------------
    Public Function DoInsert_Blob( _
                                ByVal strSQL As String, _
                                ByVal strParaName As String, _
                                ByVal bytParaVal As Byte() _
    ) As Long Implements IDBOperate.DoInsert_Blob

    End Function


    '--------------------------------------------------------------------------------
    '   <機　能>
    '       プロパティ（接続文字列）
    '
    '   <戻り値>
    '       なし
    '
    '   <引　数>
    '       なし
    '--------------------------------------------------------------------------------
    Public Property DBConnectionString() As String Implements IDBOperate.DBConnectionString
        Get
            Return m_cnnString
        End Get
        Set(ByVal value As String)
            m_cnnString = value
        End Set
    End Property

    '--------------------------------------------------------------------------------
    '   <機　能>
    '       プロパティ（エラーメッセージ）
    '
    '   <戻り値>
    '       なし
    '
    '   <引　数>
    '       なし
    '--------------------------------------------------------------------------------
    Public ReadOnly Property ErrorMessage() As String Implements IDBOperate.ErrorMessage
        Get
            If m_strErrMsg.Chars(m_strErrMsg.Length - 1) = Chr(10) Then
                m_strErrMsg = Mid(m_strErrMsg, 1, m_strErrMsg.Length - 1)
            End If
            Return m_strErrMsg
        End Get
    End Property
    '---Ins. End -----------------------------------------(2008.04.18) kawasaki

    Public Function ExecuteSQL3(ByVal strSQL As String) As Long Implements IDBOperate.ExecuteSQL3

    End Function

    'Public Function ExecuteSQL4(ByVal strSQL As String) As Object Implements IDBOperate.ExecuteSQL4

    'End Function
End Class
