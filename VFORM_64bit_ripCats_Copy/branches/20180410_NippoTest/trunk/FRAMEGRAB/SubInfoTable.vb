﻿Public Class SubInfoTable

    Private Const K_subinfo As String = "SubInfo"

    Private ReadOnly S_ID As String() = {"S_ID", "ID"}
    Private ReadOnly S_KI_ID As String() = {"S_KI_ID", "KI_ID"}
    Private ReadOnly S_ItemValue As String() = {"S_ItemValue", "ItemValue"}

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
        _KI_ID = GetDataByIndexOfDataReader(FIELD_INDEX(1), IDR)
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
        _ItemValue = GetDataByIndexOfDataReader(FIELD_INDEX(2), IDR)
    End Sub

    Public flgSyusei As Boolean = False
    Public flgNewInput As Boolean = False
    Private _backup As SubInfoTable
    Public Const MaxFieldCnt = 2
    Public strFieldNames() As String
    Public strFieldTexts() As String

    'Public SubL As List(Of SubInfoTable)

    Public Sub New()

    End Sub

    Public Sub BackUp()
        _backup = New SubInfoTable
        With _backup
            ._ID = Me._ID                           '0  ID
            ._KI_ID = Me._KI_ID                     '1  KI_ID
            ._ItemValue = Me._ItemValue             '2  ItemValue
        End With
    End Sub

    Public Sub copy(ByVal SIT As SubInfoTable)

        With SIT
            _ID = ._ID                              '0  ID
            _KI_ID = ._KI_ID                        '1  KI_ID
            _ItemValue = ._ItemValue                '2  ItemValue
        End With

        BackUp()

    End Sub

    Public Sub SetFieldIndex(ByRef IDR As IDataReader)
        Dim iList As New List(Of Integer)
        '順番を間違いないように！！！！

        iList.Add(GetIndexOfDataReader(S_ID, IDR))              '0  ID
        iList.Add(GetIndexOfDataReader(S_KI_ID, IDR))           '1  KI_ID
        iList.Add(GetIndexOfDataReader(S_ItemValue, IDR))       '2  ItemValue

        FIELD_INDEX = iList

    End Sub

    Public Shared FIELD_INDEX As New List(Of Integer)

    Public Function GetDataByReader(ByRef iDataReader As IDataReader)
        GetDataByReader = False

        p_set_ID(iDataReader)                                   '0  ID
        p_set_KI_ID(iDataReader)                                '1	KI_ID
        p_set_ItemValue(iDataReader)                            '2	ItemValue

        BackUp() 'バックアップ用

        GetDataByReader = True

    End Function

    Private Sub CreateInsertFieldName()
        ReDim strFieldNames(MaxFieldCnt)

        strFieldNames(0) = "ID"
        strFieldNames(1) = "KI_ID"
        strFieldNames(2) = "ItemValue"

    End Sub

    Public Sub CreateInsertFieldText()
        ReDim strFieldTexts(MaxFieldCnt)

        CreateInsertFieldName()

        strFieldTexts(0) = _ID
        strFieldTexts(1) = _KI_ID
        strFieldTexts(2) = "'" & _ItemValue & "'"

    End Sub

    Private Sub CreateUpdateFieldName()
        ReDim strFieldNames(MaxFieldCnt - 1)

        strFieldNames(0) = "KI_ID"
        strFieldNames(1) = "ItemValue"

    End Sub

    Public Sub CreateUpdateFieldText()
        ReDim strFieldTexts(MaxFieldCnt - 1)

        CreateUpdateFieldName()

        strFieldTexts(0) = _KI_ID
        strFieldTexts(1) = "'" & _ItemValue & "'"

    End Sub

#Region "データ取得"

    Public Function GetDataToList() As List(Of SubInfoTable)

        Dim strSql As String = "SELECT * FROM " & K_subinfo & " ORDER BY ID"
        Dim IDR As IDataReader
        Dim bRet As Boolean = False
        Dim SubL As New List(Of SubInfoTable)

        IDR = dbClass.DoSelect(strSql)
        If Not IDR Is Nothing Then
            Dim IDX As New SubInfoTable
            IDX.SetFieldIndex(IDR)
            Do While IDR.Read
                Dim SID As New SubInfoTable
                SID.GetDataByReader(IDR)
                SubL.Add(SID)
                bRet = True
            Loop
            IDR.Close()
        End If

        GetDataToList = SubL

    End Function


#End Region

End Class
