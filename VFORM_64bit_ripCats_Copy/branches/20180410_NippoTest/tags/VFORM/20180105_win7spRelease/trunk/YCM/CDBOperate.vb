Imports System.Data
Imports System.Data.OleDb
Imports ADODB
Public Class CDBOperate
    'コネクションオブジェクト
    Private m_adoConn As ADODB.Connection
    'テーブル名
    Private m_strTableName As String
    Private m_nFields As Long
    Private m_strFields() As String

    '===============================================================================
    '機  能: クラスの初期化処理
    '===============================================================================
    Public Sub New()
        m_adoConn = Nothing
        m_strTableName = ""
        m_nFields = 0
        Erase m_strFields
    End Sub

    ''===============================================================================
    ''機  能: クラスの終了処理
    ''===============================================================================
    'Private Sub Dispose()
    '    If Not m_adoConn Is Nothing Then
    '        Call DisConnectDB()
    '    End If
    '    m_adoConn = Nothing
    '    m_strTableName = ""
    '    m_nFields = 0
    '    Erase m_strFields()
    'End Sub

    '===============================================================================
    '機  能: データベースに接続する
    '戻り値: True:成功, False:失敗
    '引  数:
    '       strDataSource   [I/ ]   データソース名(mdb名)
    '備  考:
    '===============================================================================
    Public Function ConnectDB(ByVal strDataSource As String) As Boolean
        Dim strDBProvider As String = ""
        Dim strConnectionString As String

        'プロバイダ名称をセットする
        If IntPtr.Size = 4 Then
            strDBProvider = "Microsoft.Jet.OLEDB.4.0"
        ElseIf IntPtr.Size = 8 Then
            strDBProvider = "Microsoft.Ace.OLEDB.12.0"
        End If

        'ConnectionStringを作成
        strConnectionString = "Provider=" & strDBProvider & ";" & _
                              "Data Source=" & strDataSource

        'コネクションオブジェクトを生成する
        m_adoConn = New ADODB.Connection
        ConnectDB = True

        On Error GoTo ErrTrap
        'DBと接続
        Call m_adoConn.Open(strConnectionString)
        On Error GoTo 0
        'データソース名をセットする
        Exit Function
ErrTrap:
        ConnectDB = False
    End Function

    '===============================================================================
    '機  能: 接続しているDBオブジェクトを切断・解放する
    '===============================================================================
    Public Sub DisConnectDB()
        On Error Resume Next
        m_adoConn.Close()
        m_adoConn = Nothing
        On Error GoTo 0
    End Sub

    '===============================================================================
    '機  能: 指定のSQLを実行する
    '戻り値: True:成功, False:失敗
    '引  数:
    '       strSQL      [I/ ]   SQL文
    '備  考:
    '===============================================================================
    Public Function ExcuteSQL( _
        ByVal strSQL As String _
    ) As Boolean
        On Error GoTo AAAAAAAA
        m_adoConn.Execute(strSQL)
        On Error GoTo 0
        ExcuteSQL = True
        Exit Function
AAAAAAAA:
        ExcuteSQL = False
        On Error GoTo 0
    End Function

    '===============================================================================
    '機  能: SQL文を指定して、データを取得する
    '戻り値: 結果セット(ADODB.Recordset)
    '引  数:
    '       strSQL      [I/ ]   SQL文
    '備  考:
    '===============================================================================
    Public Function CreateRecordset( _
    Optional ByVal strSQL As String = "" _
    ) As ADODB.Recordset
        Dim adoRst As ADODB.Recordset
        On Error GoTo Err_CreateRecordset
        If Len(strSQL) = 0 Then
            strSQL = m_strTableName
            If m_nFields > 0 Then
            End If
        End If
        'レコードセットを作成する
        adoRst = New ADODB.Recordset
        adoRst.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        adoRst.Open(strSQL, m_adoConn, CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)

        'レコードセットオブジェクトをSQL文に従って生成する。
        CreateRecordset = adoRst
        adoRst = Nothing
        On Error GoTo 0
        Exit Function
Err_CreateRecordset:
        CreateRecordset = Nothing
    End Function

    '===============================================================================
    ' 機　能：指定したテーブルの指定したフィールドの値を取得する
    ' 戻り値：なし
    ' 引　数：
    '       strField    [I/ ]   取得するフィールド
    '       strTable    [I/ ]   対象テーブル
    '       strWhere    [I/ ]   抽出条件(省略可:省略時は全レコードを対象)
    '--------------------------------------------------------------------------------
    Public Function DoDLookup( _
        ByVal strSelect As String, _
        ByVal strTable As String, _
        Optional ByVal strWhere As String = "" _
    ) As Object
        'フィールドの内容を取得
        Dim strSQL As String
        Dim objRs As ADODB.Recordset
        strSQL = "SELECT " & strSelect & " FROM " & strTable
        If Len(strWhere) > 0 Then
            strSQL = "SELECT " & strSelect & " FROM " & strTable & " WHERE " & strWhere
        End If
        objRs = CreateRecordset(strSQL)
        On Error Resume Next
        objRs.MoveFirst()
        DoDLookup = objRs.Fields(0).Value
        objRs.Close()
        On Error GoTo 0
    End Function

    '--------------------------------------------------------------------------------
    '   <機　能>
    '       指定したレコードを更新する。
    '
    '   <戻り値>
    '       (Long)
    '       更新したレコード数（エラーの時-1を返す）
    '
    '   <引　数>
    '       strFields   [I/ ]   フィールド名
    '       strTable    [I/ ]   対象テーブル
    '       vSetValues  [I/ ]   更新する値
    '       strWhere    [I/ ]   更新条件(省略可:省略時は全レコードを対象)
    '
    '   <備　考>
    '       strFields() と vSetValues() の添字が同じ
    '--------------------------------------------------------------------------------
    Public Function DoUpdate( _
    ByVal strFields() As String, _
    ByVal strTable As String, _
    ByVal vSetValues() As Object, _
    Optional ByVal strWhere As String = "" _
    ) As Long
        '--Call outputADOInfomation("CDBOperateADO.DoUpdate")
        Dim objRecordset As ADODB.Recordset
        Dim strSQL As String
        Dim nUpdateCount As Long
        Dim i As Long

        On Error GoTo err_IDBOperate_DoUpdate
        'SQLを生成
        strSQL = "select * from " & strTable
        '条件を付加
        If Len(strWhere) > 0 Then
            strSQL = strSQL & " where " & strWhere
        End If
        'レコードセットを生成
        objRecordset = CreateRecordset(strSQL)
        '-    Call outputADOInfomation("    Update対象レコードソース:" & strSQL)
        If Not objRecordset Is Nothing Then
            nUpdateCount = 0
            Do Until objRecordset.EOF
                For i = 0 To UBound(strFields)
                    '--Call outputADOInfomation("    " & strTable & "." & strFields(i) & " ← " & vSetValues(i))
                    '更新データをセット
                    objRecordset(strFields(i)).Value = vSetValues(i)
                Next i
                'データを更新
                objRecordset.Update()
                '次のレコードへ移動
                objRecordset.MoveNext()
                '更新レコード数
                nUpdateCount = nUpdateCount + 1
            Loop
        Else
            GoTo err_IDBOperate_DoUpdate
        End If

        'レコードセットを解放
        objRecordset = Nothing
        '戻り値をセット
        DoUpdate = nUpdateCount
        Exit Function
err_IDBOperate_DoUpdate:
        DoUpdate = -1
        'レコードセットを解放
        objRecordset = Nothing
        'Debug.Print "CDBOperateADO.DoUpdate:" & strTable & ":" & Err.Description
        '--    Call dispADOErrMsg("DoUpdate")
    End Function
    '--------------------------------------------------------------------------------
    '   <機　能> データベースへのコネクションオブジェクトを返す
    '   <戻り値> (ADODB.Connection)
    '       データベースへのコネクションオブジェクト（エラーの時Nothingを返す）
    '   <引　数> なし
    '   <備　考>
    '--------------------------------------------------------------------------------
    Public Function GetConnection( _
    ) As ADODB.Connection
        GetConnection = m_adoConn
    End Function
    '===============================================================================
    Public Function BeginTrans() As Integer
        BeginTrans = m_adoConn.BeginTrans()
    End Function
    Public Sub CommitTrans()
        m_adoConn.CommitTrans()
    End Sub
    Public Sub RollbackTrans()
        m_adoConn.RollbackTrans()
    End Sub


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
    ) As Long

        Dim intFieldCnt As Integer
        Dim i As Integer
        Dim strInsert As String = ""
        Dim strValue As String = ""


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



            Return Me.ExcuteSQL(strInsert & ")" & strValue & ")")

        Catch
            Return -1
        End Try

    End Function
End Class
