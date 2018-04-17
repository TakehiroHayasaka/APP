
Module CommonDB

    'DB接続

    Public dbClass As FBMlib.CDBOperateOLE

    Public m_Keisoku_mdb_name As String = "計測データ.mdb"
    Public m_System_mdb_name As String = "システム設定.mdb"

    Public m_system_path As String
    'Public m_SystemMdbPath As String = System.Environment.CurrentDirectory & "\計測システムフォルダ"
    Public m_SystemMdbPath As String = My.Application.Info.DirectoryPath & "\計測システムフォルダ"
    Public m_SystemMdbFullPath As String = m_SystemMdbPath & "\" & m_System_mdb_name

    Public Const err_msg_connect As String = "データベース接続に失敗しました。"
    Public Const err_msg_systemdb_connect As String = "システム設定mdbへの接続に失敗しました。"
    Public Const err_msg_keisokumdb_connect As String = "計測データmdbへの接続に失敗しました。"

    ' 20131118 Y.Tezuka Add 選択フラグ類追加
    Public m_flg_Zahyo As Boolean
    Public m_flg_Sunpo As Boolean
    Public m_flg_Zukei As Boolean
    Public m_flg_Senyou As Boolean
    Public m_Senyou_Id As Integer
    Public m_SenyouList_No As Integer
    Public m_CellChange As Integer = 0

    'Public m_keisokuDB_path As String = System.Environment.CurrentDirectory & "\計測データ.mdb"

    '''
    '計測データ接続

    Public Function ConnectDB(ByVal strPath As String) As Boolean

        Dim Flg As Boolean = False

        ConnectDB = -1

        dbClass = New FBMlib.CDBOperateOLE

        If Not dbClass Is Nothing Then
            Flg = dbClass.Connect(strPath)
        End If

        If Flg = True Then
            ConnectDB = 1
        End If

    End Function

    '''
    '計測データ切断
    Public Function DisConnectDB() As Boolean
        dbClass.DisConnect()
    End Function

    ''' システム設定mdb接続

    Public Function ConnectSystemDB(ByRef dbClass1 As FBMlib.CDBOperateOLE) As Boolean

        Dim Flg As Boolean = False

        ConnectSystemDB = -1

        dbClass1 = New FBMlib.CDBOperateOLE

        If Not dbClass1 Is Nothing Then
            Flg = dbClass1.Connect(m_SystemMdbFullPath)
        End If

        If Flg = True Then
            ConnectSystemDB = 1
        End If

    End Function

    '''
    '計測データ接続

    Public Function ConnectDB(ByVal strPath As String, ByRef dbClass1 As FBMlib.CDBOperateOLE) As Boolean

        Dim Flg As Boolean = False

        dbClass1 = New FBMlib.CDBOperateOLE

        If Not dbClass1 Is Nothing Then
            Flg = dbClass1.Connect(strPath)
        End If

        ConnectDB = Flg

    End Function

    '''
    '計測データ切断
    Public Function DisConnectDB(ByRef dbClass1 As FBMlib.CDBOperateOLE) As Boolean
        dbClass1.DisConnect()
    End Function


    Public ReadOnly Property intField(ByVal objField As Object) As Integer
        Get
            Try
                If IsNumeric(objField) = False Then
                    Return CInt(0)
                Else
                    Return CInt(objField)
                End If
            Catch ex As Exception
                Return CInt(0)
            End Try
        End Get
    End Property

    Public ReadOnly Property dblField(ByVal objField As Object) As Double
        Get
            Try
                If IsNumeric(objField) = False Then
                    Return CDbl(0)
                Else
                    Return CDbl(objField)
                End If
            Catch ex As Exception
                Return CDbl(0)
            End Try
        End Get
    End Property

    Public Function GetIndexOfDataReader(ByRef strItemNames() As String, ByRef iDataReader As IDataReader) As Integer
        For Each strItem As String In strItemNames
            Try
                GetIndexOfDataReader = iDataReader.GetOrdinal(strItem)
                Exit Function
            Catch ex As Exception

            End Try
        Next
        GetIndexOfDataReader = -1
    End Function

    Public Function GetDataByIndexOfDataReader(ByRef iIndex As Integer, ByRef iDataReader As IDataReader) As String
        Try
            GetDataByIndexOfDataReader = iDataReader.GetValue(iIndex).ToString.Trim
            Exit Function
        Catch ex As Exception

        End Try
        GetDataByIndexOfDataReader = ""
    End Function

    Public Function GetSQLIdentityID(ByVal strTableName As String, ByRef dbClass1 As FBMlib.CDBOperateOLE) As Integer

        Dim strSql As String
        Dim strSelect As String
        Dim strFrom As String
        Dim IDR As IDataReader
        Dim iMaxID As Integer = 0

        strSelect = "SELECT "
        strSelect = strSelect & "MAX(ID) "

        strFrom = "FROM "
        strFrom = strFrom & strTableName & " "

        strSql = strSelect & strFrom

        IDR = dbClass1.DoSelect(strSql)
        If Not IDR Is Nothing Then
            If IDR.Read = True Then
                iMaxID = intField(IDR.GetValue(0))
            End If
            IDR.Close()
        End If

        GetSQLIdentityID = iMaxID

    End Function

    Public BeforePID As Integer()
    Public MyPID As Integer

    Public Sub MRComObject(Of T As Class)(ByRef objCom As T, Optional ByVal force As Boolean = False)
        If objCom Is Nothing Then
            Return
        End If
        Try
            If System.Runtime.InteropServices.Marshal.IsComObject(objCom) Then
                If force Then
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(objCom)
                Else
                    Dim count As Integer = System.Runtime.InteropServices.Marshal.ReleaseComObject(objCom)
                End If
            End If
        Finally
            objCom = Nothing
        End Try
    End Sub

    Public Sub GetExcelPID()

        Dim bRet As Boolean = True
        Dim localByName As Process() = Process.GetProcessesByName("Excel")
        ReDim BeforePID(localByName.Length)

        If localByName.Length = 0 Then
        Else
            For i As Integer = 0 To localByName.Length - 1
                BeforePID(i) = localByName(i).Id
            Next

        End If

    End Sub

    Public Sub GetExcelNewPID()

        Dim bRet As Boolean = True
        Dim localByName As Process() = Process.GetProcessesByName("Excel")
        Dim AfterPID As Integer()
        Dim flg_Excel_PID As Boolean = False

        localByName = Process.GetProcessesByName("Excel")
        ReDim AfterPID(localByName.Length)
        For i As Integer = 0 To localByName.Length - 1
            flg_Excel_PID = False
            AfterPID(i) = localByName(i).Id
            For j As Integer = 0 To UBound(BeforePID)
                If BeforePID(j) = AfterPID(i) Then
                    flg_Excel_PID = True
                    Exit For
                End If
            Next
            If flg_Excel_PID = False Then
                MyPID = AfterPID(i)
                Exit For
            End If
        Next

    End Sub

    Public Sub ExKillProcess()

        Try
            Dim localByPID As Process = Process.GetProcessById(MyPID)
            If localByPID.Id = MyPID Then
                Process.GetProcessById(MyPID).Kill()
            End If
            localByPID.Close()
            localByPID.Dispose()
        Catch ex As Exception

        End Try

    End Sub

    Public Function GetColNo(ByVal iConNo As Integer) As String

        Dim aaa As String() = {"", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"}

        Dim rc As Integer = 0
        Dim col As Integer = iConNo

        Do While col > 26
            rc += 1
            col -= 26
        Loop

        GetColNo = aaa(rc) & aaa(col)

    End Function

End Module
