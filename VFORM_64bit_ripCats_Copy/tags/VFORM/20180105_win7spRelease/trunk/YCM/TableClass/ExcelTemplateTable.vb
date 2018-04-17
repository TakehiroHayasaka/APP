Public Class ExcelTemplateTable

    Private Const T_ExcelTemplate As String = "ExcelTemplate"

    Private ReadOnly T_ID As String() = {"ID"}                              '0  ID
    Private ReadOnly T_TypeID As String() = {"TypeID"}                      '1  名前
    Private ReadOnly T_TemplateFileName As String() = {"TemplateFileName"}  '2  Excelファイル名

    Private ReadOnly T_flg_active As String() = {"flg_active"}              '3  表示フラグ

    Private _ID As String = ""
    Public Property ID() As String
        Get
            Return _ID
        End Get
        Set(ByVal value As String)
            _ID = value
        End Set
    End Property

    Private Sub p_set_ID(ByRef IDR As IDataReader)
        _ID = GetDataByIndexOfDataReader(FIELD_INDEX(0), IDR)
    End Sub

    Private _TypeID As String = ""
    Public Property TypeID() As String
        Get
            Return _TypeID
        End Get
        Set(ByVal value As String)
            _TypeID = value
        End Set
    End Property

    Private Sub p_set_TypeID(ByRef IDR As IDataReader)
        _TypeID = GetDataByIndexOfDataReader(FIELD_INDEX(1), IDR)
    End Sub

    Private _TemplateFileName As String = ""
    Public Property TemplateFileName() As String
        Get
            Return _TemplateFileName
        End Get
        Set(ByVal value As String)
            _TemplateFileName = value
        End Set
    End Property

    Private Sub p_set_TemplateFileName(ByRef IDR As IDataReader)
        _TemplateFileName = GetDataByIndexOfDataReader(FIELD_INDEX(2), IDR)
    End Sub

    Private _flg_active As String = ""
    Public Property flg_active() As String
        Get
            Return _flg_active
        End Get
        Set(ByVal value As String)
            _flg_active = value
        End Set
    End Property

    Private Sub p_set_flg_active(ByRef IDR As IDataReader)
        _flg_active = GetDataByIndexOfDataReader(FIELD_INDEX(3), IDR)
    End Sub

    Public flgSyusei As Boolean = False
    Public flgNewInput As Boolean = False
    Private _backup As ExcelTemplateTable
    Public Const MaxFieldCnt = 3
    Public strFieldNames() As String
    Public strFieldTexts() As String

    Public m_dbClass As FBMlib.CDBOperateOLE

    Public Sub New()

    End Sub

    Public Sub BackUp()
        _backup = New ExcelTemplateTable
        With _backup
            ._ID = Me._ID                                   '0  ID
            ._TypeID = Me._TypeID                           '1  TypeID
            ._TemplateFileName = Me.TemplateFileName        '2  Excelファイル名

            ._flg_active = Me.flg_active                    '3  表示フラグ
        End With
    End Sub

    Public Sub copy(ByVal TPT As ExcelTemplateTable)

        With TPT
            _ID = ._ID                                      '0  ID
            _TypeID = ._TypeID                              '1  TypeID
            _TemplateFileName = .TemplateFileName           '2  Excelファイル名

            _flg_active = .flg_active                       '3  表示フラグ
        End With

        BackUp()

    End Sub

    Public Sub SetFieldIndex(ByRef IDR As IDataReader)
        Dim iList As New List(Of Integer)
        '順番を間違いないように！！！！

        iList.Add(GetIndexOfDataReader(T_ID, IDR))                  '0  ID
        iList.Add(GetIndexOfDataReader(T_TypeID, IDR))           '1  TypeID
        iList.Add(GetIndexOfDataReader(T_TemplateFileName, IDR))          '2  Excelファイル名

        iList.Add(GetIndexOfDataReader(T_flg_active, IDR))        '3  表示フラグ

        FIELD_INDEX = iList

    End Sub

    Public Shared FIELD_INDEX As New List(Of Integer)

    Public Function GetDataByReader(ByRef iDataReader As IDataReader)
        GetDataByReader = False

        p_set_ID(iDataReader)                                   '0  ID
        p_set_TypeID(iDataReader)                            '1	type_name
        p_set_TemplateFileName(iDataReader)                           '2  Excelファイル名

        p_set_flg_active(iDataReader)                         '3  表示フラグ

        BackUp() 'バックアップ用

        GetDataByReader = True

    End Function

    Private Sub CreateFieldName()
        ReDim strFieldNames(MaxFieldCnt - 1)

        strFieldNames(0) = "TypeID"
        strFieldNames(1) = "TemplateFileName"
        strFieldNames(2) = "flg_active"


    End Sub

    Public Sub CreateFieldText()
        ReDim strFieldTexts(MaxFieldCnt - 1)

        CreateFieldName()

        strFieldTexts(0) = _TypeID
        strFieldTexts(1) = "'" & _TemplateFileName & "'"
        strFieldTexts(2) = _flg_active

    End Sub

    Public Sub SetUpdateField()

        ReDim strFieldNames(MaxFieldCnt - 1)
        ReDim strFieldTexts(MaxFieldCnt - 1)

        strFieldNames(0) = "TypeID"
        strFieldNames(1) = "TemplateFileName"
        strFieldNames(2) = "flg_active"

        strFieldTexts(0) = _TypeID
        strFieldTexts(1) = "'" & _TemplateFileName & "'"
        strFieldTexts(2) = _flg_active

    End Sub

#Region "データ取得"

    Public Function GetDataToList() As List(Of ExcelTemplateTable)

        Dim strSql As String = "SELECT * FROM " & T_ExcelTemplate & " WHERE TypeID = " & CommonTypeID & " AND flg_Active = 1 ORDER BY ID"
        Dim IDR As IDataReader
        Dim bRet As Boolean = False
        Dim ExcelTemplateL As List(Of ExcelTemplateTable) = Nothing

        IDR = m_dbClass.DoSelect(strSql)
        If Not IDR Is Nothing Then
            ExcelTemplateL = New List(Of ExcelTemplateTable)
            Dim IDX As New ExcelTemplateTable
            IDX.SetFieldIndex(IDR)
            Do While IDR.Read
                Dim ETD As New ExcelTemplateTable
                ETD.GetDataByReader(IDR)
                ExcelTemplateL.Add(ETD)
            Loop
            IDR.Close()
        End If

        GetDataToList = ExcelTemplateL

    End Function

#End Region

#Region "Save"

    Public Function ExistsData() As Boolean

        Dim bRet As Boolean = False
        Dim lRet As Long = 0
        Dim strWhere As String = ""

        If _ID.Trim = "" Then
            ExistsData = False
            Exit Function
        End If

        strWhere = "ID = " & _ID.Trim

        lRet = m_dbClass.DoDCount("*", T_ExcelTemplate, strWhere)
        If lRet > 0 Then
            bRet = True
        End If

        ExistsData = bRet

    End Function

    Public Function SaveData(Optional ByRef flg_trans As Boolean = True) As Boolean

        Dim bRet As Boolean = True

        If ExistsData() = True Then
            'SunpoSetが登録されている場合、Update
            bRet = UpdateData(flg_trans)
        Else
            'SunpoSetが登録されている場合、Insert
            bRet = InsertData(flg_trans)
        End If

        SaveData = bRet

    End Function

#End Region

#Region "Insert"

    Public Function InsertData(Optional ByRef flg_trans As Boolean = True) As Boolean

        InsertData = True

        Dim strWhere As String = "ID = " & _ID

        CreateFieldText()

        Dim lRet As Long = 0

        lRet = m_dbClass.DoInsert(strFieldNames, T_ExcelTemplate, strFieldTexts)
        If lRet = 1 Then
        Else
            m_dbClass.RollbackTrans()
            InsertData = False
        End If

    End Function

#End Region

#Region "Update"

    Public Function UpdateData(Optional ByRef flg_trans As Boolean = True) As Boolean

        UpdateData = True

        Dim strWhere As String = "ID = " & _ID

        SetUpdateField()

        Dim lRet As Long = 0
        If flg_trans = True Then
            m_dbClass.BeginTrans()
            lRet = m_dbClass.DoUpdate(strFieldNames, T_ExcelTemplate, strFieldTexts, strWhere)
            If lRet = 1 Then
                m_dbClass.CommitTrans()
            Else
                m_dbClass.RollbackTrans()
                UpdateData = False
            End If
        Else
            lRet = m_dbClass.DoUpdate(strFieldNames, T_ExcelTemplate, strFieldTexts, strWhere)
            If lRet = 1 Then
            Else
                UpdateData = False
            End If
        End If

    End Function

#End Region

End Class
