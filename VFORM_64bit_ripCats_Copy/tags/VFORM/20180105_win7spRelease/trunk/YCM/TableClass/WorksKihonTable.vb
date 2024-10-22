﻿Public Class WorksKihonTable

    Private Const K_works_kihon As String = "WorksKihon"

    Private ReadOnly W_ID As String() = {"WKI_ID", "ID"}
    Private ReadOnly W_Work_ID As String() = {"WKI_WorkID", "WorkID"}
    Private ReadOnly W_KI_ID As String() = {"WKI_KI_ID", "KI_ID"}
    Private ReadOnly W_ItemValue As String() = {"WKI_ItemValue", "ItemValue"}

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

    Private _Work_ID As String = ""
    Public Property Work_ID() As String
        Get
            Return _Work_ID
        End Get
        Set(ByVal value As String)
            _Work_ID = value
        End Set
    End Property

    Private Sub p_set_Work_ID(ByRef IDR As IDataReader)
        _Work_ID = GetDataByIndexOfDataReader(FIELD_INDEX(1), IDR)
    End Sub

    Private _KI_ID As String = ""
    Public Property KI_ID() As String
        Get
            Return _KI_ID
        End Get
        Set(ByVal value As String)
            _KI_ID = value
        End Set
    End Property

    Private Sub p_set_KI_ID(ByRef IDR As IDataReader)
        _KI_ID = GetDataByIndexOfDataReader(FIELD_INDEX(2), IDR)
    End Sub

    Private _ItemValue As String = ""
    Public Property ItemValue() As String
        Get
            Return _ItemValue
        End Get
        Set(ByVal value As String)
            _ItemValue = value
        End Set
    End Property

    Private Sub p_set_ItemValue(ByRef IDR As IDataReader)
        _ItemValue = GetDataByIndexOfDataReader(FIELD_INDEX(3), IDR)
    End Sub

    Public flgSyusei As Boolean = False
    Public flgNewInput As Boolean = False
    Private _backup As WorksKihonTable
    Public Const MaxFieldCnt = 3
    Public strFieldNames() As String
    Public strFieldTexts() As String

    Public m_dbClass As FBMlib.CDBOperateOLE

    Public Sub New()

    End Sub

    Public Sub BackUp()
        _backup = New WorksKihonTable
        With _backup
            ._ID = Me._ID                           '0  ID
            ._Work_ID = Me._Work_ID                 '1  Work_ID
            ._KI_ID = Me._KI_ID                     '2  KI_ID
            ._ItemValue = Me._ItemValue             '3  ItemValue
        End With
    End Sub

    Public Sub copy(ByVal WKT As WorksKihonTable)

        With WKT
            _ID = ._ID                              '0  ID
            _Work_ID = ._Work_ID                    '1  Work_ID
            _KI_ID = ._KI_ID                        '2  KI_ID
            _ItemValue = ._ItemValue                '3  ItemValue
        End With

        BackUp()

    End Sub

    Public Sub SetFieldIndex(ByRef IDR As IDataReader)
        Dim iList As New List(Of Integer)
        '順番を間違いないように！！！！

        iList.Add(GetIndexOfDataReader(W_ID, IDR))                  '0  ID
        iList.Add(GetIndexOfDataReader(W_Work_ID, IDR))             '1  WorkID
        iList.Add(GetIndexOfDataReader(W_KI_ID, IDR))               '2  KI_ID
        iList.Add(GetIndexOfDataReader(W_ItemValue, IDR))           '3  ItemValue

        FIELD_INDEX = iList

    End Sub

    Public Shared FIELD_INDEX As New List(Of Integer)

    Public Function GetDataByReader(ByRef iDataReader As IDataReader)
        GetDataByReader = False

        p_set_ID(iDataReader)                                   '0  ID
        p_set_Work_ID(iDataReader)                              '1	WorkID
        p_set_KI_ID(iDataReader)                                '2	KI_ID
        p_set_ItemValue(iDataReader)                            '3	ItemValue

        BackUp() 'バックアップ用

        GetDataByReader = True

    End Function

    Private Sub CreateFieldName()
        ReDim strFieldNames(MaxFieldCnt - 1)

        strFieldNames(0) = "WorkID"
        strFieldNames(1) = "KI_ID"
        strFieldNames(2) = "ItemValue"

    End Sub

    Public Sub CreateFieldText()
        ReDim strFieldTexts(MaxFieldCnt - 1)

        CreateFieldName()

        strFieldTexts(0) = _Work_ID
        strFieldTexts(1) = _KI_ID
        strFieldTexts(2) = "'" & _ItemValue & "'"

    End Sub

#Region "データ取得"

    Public Function GetDataToList() As List(Of WorksKihonTable)

        Dim strSql As String = "SELECT * FROM " & K_works_kihon & " WHERE WorkID = " & _Work_ID & " ORDER BY ID"
        Dim IDR As IDataReader
        Dim bRet As Boolean = False

        Dim WorksKihonL As New List(Of WorksKihonTable)

        IDR = m_dbClass.DoSelect(strSql)
        If Not IDR Is Nothing Then
            Dim IDX As New WorksKihonTable
            IDX.SetFieldIndex(IDR)
            Do While IDR.Read
                Dim WKD As New WorksKihonTable
                WKD.GetDataByReader(IDR)
                WorksKihonL.Add(WKD)
                bRet = True
            Loop
            IDR.Close()
        End If

        GetDataToList = WorksKihonL

    End Function


#End Region

#Region "データ更新"

    Public Function SaveData(Optional ByRef flg_trans As Boolean = True) As Boolean

        Dim bRet As Boolean = True

        If ExistsData() = True Then
            'WorksKihonが登録されている場合、Update
            bRet = UpdateData(flg_trans)
        Else
            'WorksKihonが登録されている場合、Insert
            bRet = InsertData(flg_trans)
        End If

        SaveData = bRet

    End Function

    Public Function ExistsData() As Boolean

        Dim bRet As Boolean = False
        Dim lRet As Long = 0
        Dim strWhere As String = ""

        'If _ID.Trim = "" Then
        '    ExistsData = False
        '    Exit Function
        'End If

        strWhere = "WorkID = " & _Work_ID.Trim & " AND KI_ID = " & _KI_ID.Trim

        lRet = m_dbClass.DoDCount("*", K_works_kihon, strWhere)
        If lRet > 0 Then
            bRet = True
        End If

        ExistsData = bRet

    End Function

#End Region

#Region "Insert"

    Public Function InsertData(Optional ByRef flg_trans As Boolean = True) As Boolean

        InsertData = True

        CreateFieldText()

        Dim lRet As Long = 0

        If flg_trans = True Then
            m_dbClass.BeginTrans()
            lRet = m_dbClass.DoInsert(strFieldNames, K_works_kihon, strFieldTexts)
            If lRet = 1 Then
                m_dbClass.CommitTrans()
            Else
                m_dbClass.RollbackTrans()
                InsertData = False
                Exit Function
            End If
        Else
            lRet = m_dbClass.DoInsert(strFieldNames, K_works_kihon, strFieldTexts)
            If lRet = 1 Then
            Else
                InsertData = False
            End If
        End If

    End Function

#End Region

#Region "Update"

    Public Function UpdateData(Optional ByRef flg_trans As Boolean = True) As Boolean

        UpdateData = True

        Dim lRet As Long = 0
        Dim bRet As Boolean = True

        Dim strWhere As String = "WorkID = " & _Work_ID.Trim & " AND KI_ID = " & _KI_ID.Trim

        ReDim strFieldNames(0)
        ReDim strFieldTexts(0)

        strFieldNames(0) = "ItemValue"

        strFieldTexts(0) = "'" & _ItemValue & "'"

        With m_dbClass
            If flg_trans = True Then
                .BeginTrans()
                lRet = .DoUpdate(strFieldNames, K_works_kihon, strFieldTexts, strWhere)
                If lRet = 1 Then
                    .CommitTrans()
                Else
                    .RollbackTrans()
                    bRet = False
                End If
            Else
                lRet = .DoUpdate(strFieldNames, K_works_kihon, strFieldTexts, strWhere)
                If lRet = 1 Then
                Else
                    bRet = False
                End If
            End If
        End With

    End Function

#End Region

    '#Region "Insert"

    '    Public Function InsertData() As Boolean

    '        InsertData = True

    '        CreateFieldText()

    '        Dim lRet As Long = 0
    '        lRet = m_dbClass.DoInsert(strFieldNames, K_works_kihon, strFieldTexts)
    '        If lRet = 1 Then
    '        Else
    '            InsertData = False
    '        End If

    '    End Function

    '#End Region

End Class
