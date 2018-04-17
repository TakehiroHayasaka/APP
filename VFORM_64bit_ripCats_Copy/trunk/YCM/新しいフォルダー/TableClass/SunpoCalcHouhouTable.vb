Public Class SunpoCalcHouhouTable

    Private Const T_SunpoCalcHouhou As String = "SunpoCalcHouhou"

    Private ReadOnly T_ID As String() = {"ID"}                              '0  ID
    Private ReadOnly T_SchID As String() = {"SchID"}                        '1  SchID
    Private ReadOnly T_CalcName As String() = {"Calcname"}                  '2  名前
    Private ReadOnly T_PreviewImagePath As String() = {"PreviewImagePath"}  '3  イメージパス
    Private ReadOnly T_TypeID As String() = {"TypeID"}                      '4  

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

    Private _SchID As String = ""
    Public Property SchID() As String
        Get
            Return _SchID
        End Get
        Set(ByVal value As String)
            _SchID = value
        End Set
    End Property

    Private Sub p_set_SchID(ByRef IDR As IDataReader)
        _SchID = GetDataByIndexOfDataReader(FIELD_INDEX(1), IDR)
    End Sub

    Private _Calcname As String = ""
    Public Property Calcname() As String
        Get
            Return _Calcname
        End Get
        Set(ByVal value As String)
            _Calcname = value
        End Set
    End Property

    Private Sub p_set_CalcName(ByRef IDR As IDataReader)
        _Calcname = GetDataByIndexOfDataReader(FIELD_INDEX(2), IDR)
    End Sub

    Private _PreviewImagePath As String = ""
    Public Property PreviewImagePath() As String
        Get
            Return _PreviewImagePath
        End Get
        Set(ByVal value As String)
            _PreviewImagePath = value
        End Set
    End Property

    Private Sub p_set_PreviewImagePath(ByRef IDR As IDataReader)
        _PreviewImagePath = GetDataByIndexOfDataReader(FIELD_INDEX(3), IDR)
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
        _TypeID = GetDataByIndexOfDataReader(FIELD_INDEX(4), IDR)
    End Sub

    Public flgSyusei As Boolean = False
    Public flgNewInput As Boolean = False
    Private _backup As SunpoCalcHouhouTable
    Public Const MaxFieldCnt = 4
    Public strFieldNames() As String
    Public strFieldTexts() As String

    Public m_dbClass As FBMlib.CDBOperateOLE

    Public Sub New()

    End Sub

    Public Sub BackUp()
        _backup = New SunpoCalcHouhouTable
        With _backup
            ._ID = Me._ID                                   '0  ID
            ._SchID = Me._SchID                             '1  SchID
            ._Calcname = Me.Calcname                        '2  Excelファイル名

            ._PreviewImagePath = Me.PreviewImagePath        '3  表示フラグ
            ._TypeID = Me._TypeID                           '4  TypeID
        End With
    End Sub

    Public Sub copy(ByVal TPT As SunpoCalcHouhouTable)

        With TPT
            _ID = ._ID                                      '0  ID
            _SchID = ._SchID                                '1  SchID
            _Calcname = .Calcname                           '2  表示フラグ
            _PreviewImagePath = .PreviewImagePath           '3  Excelファイル名

            _TypeID = ._TypeID                              '4  TypeID
        End With

        BackUp()

    End Sub

    Public Sub SetFieldIndex(ByRef IDR As IDataReader)
        Dim iList As New List(Of Integer)
        '順番を間違いないように！！！！

        iList.Add(GetIndexOfDataReader(T_ID, IDR))                  '0  ID
        iList.Add(GetIndexOfDataReader(T_SchID, IDR))               '1  SchID
        iList.Add(GetIndexOfDataReader(T_CalcName, IDR))            '2  Excelファイル名

        iList.Add(GetIndexOfDataReader(T_PreviewImagePath, IDR))    '3  表示フラグ
        iList.Add(GetIndexOfDataReader(T_TypeID, IDR))              '4  TypeID

        FIELD_INDEX = iList

    End Sub

    Public Shared FIELD_INDEX As New List(Of Integer)

    Public Function GetDataByReader(ByRef iDataReader As IDataReader)
        GetDataByReader = False

        p_set_ID(iDataReader)                                   '0  ID
        p_set_SchID(iDataReader)                                '1  SchID
        p_set_CalcName(iDataReader)                             '2  表示フラグ
        p_set_PreviewImagePath(iDataReader)                     '3  Excelファイル名

        p_set_TypeID(iDataReader)                               '4	type_name
        BackUp() 'バックアップ用

        GetDataByReader = True

    End Function

    Private Sub CreateFieldName()
        ReDim strFieldNames(MaxFieldCnt)

        strFieldNames(0) = "ID"
        strFieldNames(1) = "SchID"
        strFieldNames(2) = "CalcName"
        strFieldNames(3) = "PreviewImagePath"
        strFieldNames(4) = "TypeID"

    End Sub

    Public Sub CreateFieldText()
        ReDim strFieldTexts(MaxFieldCnt)

        CreateFieldName()

        strFieldTexts(0) = _ID
        strFieldTexts(1) = _SchID
        strFieldTexts(2) = "'" & _Calcname & "'"
        strFieldTexts(3) = "'" & _PreviewImagePath & "'"
        strFieldTexts(4) = _TypeID

    End Sub

    Public Sub SetUpdateField()

        ReDim strFieldNames(MaxFieldCnt)
        ReDim strFieldTexts(MaxFieldCnt)

        strFieldNames(0) = "ID"
        strFieldNames(1) = "SchID"
        strFieldNames(2) = "CalcName"
        strFieldNames(3) = "PreviewImagePath"
        strFieldNames(4) = "TypeID"

        strFieldTexts(0) = _ID
        strFieldTexts(1) = _SchID
        strFieldTexts(2) = "'" & _Calcname & "'"
        strFieldTexts(3) = "'" & _PreviewImagePath & "'"
        strFieldTexts(4) = _TypeID

    End Sub

#Region "データ取得"

    Public Function GetDataToList() As List(Of SunpoCalcHouhouTable)

        Dim strSql As String = "SELECT * FROM " & T_SunpoCalcHouhou & " WHERE TypeID = " & CommonTypeID & " ORDER BY SchID"
        Dim IDR As IDataReader
        Dim bRet As Boolean = False
        Dim SunpoCalcHouhouL As List(Of SunpoCalcHouhouTable) = Nothing

        IDR = m_dbClass.DoSelect(strSql)
        If Not IDR Is Nothing Then
            SunpoCalcHouhouL = New List(Of SunpoCalcHouhouTable)
            Dim IDX As New SunpoCalcHouhouTable
            IDX.SetFieldIndex(IDR)
            Do While IDR.Read
                Dim ETD As New SunpoCalcHouhouTable
                ETD.GetDataByReader(IDR)
                SunpoCalcHouhouL.Add(ETD)
            Loop
            IDR.Close()
        End If

        GetDataToList = SunpoCalcHouhouL

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

        lRet = m_dbClass.DoDCount("*", T_SunpoCalcHouhou, strWhere)
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

        lRet = m_dbClass.DoInsert(strFieldNames, T_SunpoCalcHouhou, strFieldTexts)
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
            lRet = m_dbClass.DoUpdate(strFieldNames, T_SunpoCalcHouhou, strFieldTexts, strWhere)
            If lRet = 1 Then
                m_dbClass.CommitTrans()
            Else
                m_dbClass.RollbackTrans()
                UpdateData = False
            End If
        Else
            lRet = m_dbClass.DoUpdate(strFieldNames, T_SunpoCalcHouhou, strFieldTexts, strWhere)
            If lRet = 1 Then
            Else
                UpdateData = False
            End If
        End If

    End Function

#End Region

End Class
