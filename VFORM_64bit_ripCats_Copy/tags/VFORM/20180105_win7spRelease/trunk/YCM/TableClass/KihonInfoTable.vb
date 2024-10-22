﻿Imports Microsoft.VisualBasic.FileIO

Public Class KihonInfoTable

    Private Const K_kihoninfo As String = "Kihoninfo"

    Private ReadOnly K_ID As String() = {"K_ID", "ID"}
    Private ReadOnly K_item_name As String() = {"K_Name", "Name", "item_name"}
    Private ReadOnly K_item_cell_name As String() = {"K_CellName", "CellName", "item_cell_name"}
    Private ReadOnly K_item_value As String() = {"K_ItemValue", "ItemValue"}
    Private ReadOnly K_value_type As String() = {"K_ValueType", "ValueType", "value_type"}
    Private ReadOnly K_default_value As String() = {"K_DefaultValue", "DefaultValue", "default_value"}
    Private ReadOnly K_disp_sort_ID As String() = {"K_DispSortID", "DispSortID", "disp_sort_ID"}
    Private ReadOnly K_update_date As String() = {"K_UpdateDate", "UpdateDate", "update_date"}
    Private ReadOnly K_works_In As String() = {"K_WorksIn", "WorksIn", "works_In"}
    Private ReadOnly K_flg_input As String() = {"K_flgInput", "flgInput", "flg_input"}
    Private ReadOnly K_TypeID As String() = {"K_TypeID", "TypeID", "type_ID"}
    'SUURI ADD 20140914
    Private ReadOnly K_defaultDXFnameflg As String() = {"K_defaultDXFnameflg", "defaultDXFnameflg", "DefaultDXFnameflg"}

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

    Private _item_name As String = ""
    Public Property item_name() As String
        Get
            Return _item_name
        End Get
        Set(ByVal value As String)
            _item_name = value
        End Set
    End Property

    Private Sub p_set_item_name(ByRef IDR As IDataReader)
        _item_name = GetDataByIndexOfDataReader(FIELD_INDEX(1), IDR)
    End Sub

    Private _item_cell_name As String = ""
    Public Property item_cell_name() As String
        Get
            Return _item_cell_name
        End Get
        Set(ByVal value As String)
            _item_cell_name = value
        End Set
    End Property

    Private Sub p_set_item_cell_name(ByRef IDR As IDataReader)
        _item_cell_name = GetDataByIndexOfDataReader(FIELD_INDEX(2), IDR)
    End Sub

    Private _item_value As String = ""
    Public Property item_value() As String
        Get
            Return _item_value
        End Get
        Set(ByVal value As String)
            _item_value = value
        End Set
    End Property

    Private Sub p_set_item_value(ByRef IDR As IDataReader)
        _item_value = GetDataByIndexOfDataReader(FIELD_INDEX(3), IDR)
    End Sub

    Private _value_type As String = ""
    Public Property value_type() As String
        Get
            Return _value_type
        End Get
        Set(ByVal value As String)
            _value_type = value
        End Set
    End Property

    Private Sub p_set_value_type(ByRef IDR As IDataReader)
        _value_type = GetDataByIndexOfDataReader(FIELD_INDEX(4), IDR)
    End Sub

    Private _default_value As String = ""
    Public Property default_value() As String
        Get
            Return _default_value
        End Get
        Set(ByVal value As String)
            _default_value = value
        End Set
    End Property

    Private Sub p_set_default_value(ByRef IDR As IDataReader)
        _default_value = GetDataByIndexOfDataReader(FIELD_INDEX(5), IDR)
    End Sub

    Private _disp_sort_ID As String = ""
    Public Property disp_sort_ID() As String
        Get
            Return _disp_sort_ID
        End Get
        Set(ByVal value As String)
            _disp_sort_ID = value
        End Set
    End Property

    Private Sub p_set_disp_sort_ID(ByRef IDR As IDataReader)
        _disp_sort_ID = GetDataByIndexOfDataReader(FIELD_INDEX(6), IDR)
    End Sub

    Private _update_date As String = ""
    Public Property update_date() As String
        Get
            Return _update_date
        End Get
        Set(ByVal value As String)
            _update_date = value
        End Set
    End Property

    Private Sub p_set_update_date(ByRef IDR As IDataReader)
        _update_date = GetDataByIndexOfDataReader(FIELD_INDEX(7), IDR)
    End Sub

    Private _works_in As String = ""
    Public Property works_in() As String
        Get
            Return _works_in
        End Get
        Set(ByVal value As String)
            _works_in = value
        End Set
    End Property

    Private Sub p_set_works_in(ByRef IDR As IDataReader)
        _works_in = GetDataByIndexOfDataReader(FIELD_INDEX(8), IDR)
    End Sub

    Private _flg_input As String = ""
    Public Property flg_input() As String
        Get
            Return _flg_input
        End Get
        Set(ByVal value As String)
            _flg_input = value
        End Set
    End Property

    Private Sub p_set_flg_input(ByRef IDR As IDataReader)
        _flg_input = GetDataByIndexOfDataReader(FIELD_INDEX(9), IDR)
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
        _TypeID = GetDataByIndexOfDataReader(FIELD_INDEX(10), IDR)
    End Sub

    'SUURI ADD 20140914
    Private _defaultDXFnameflg As String
    Public Property defaultDXFnameflg() As String
        Get
            Return _defaultDXFnameflg
        End Get
        Set(ByVal value As String)
            _defaultDXFnameflg = value
        End Set
    End Property

    Private Sub p_set_defaultDXFnameflg(ByRef IDR As IDataReader)
        _defaultDXFnameflg = GetDataByIndexOfDataReader(FIELD_INDEX(11), IDR)
    End Sub

    Public flgSyusei As Boolean = False
    Public flgNewInput As Boolean = False
    Private _backup As KihonInfoTable
    Public Const MaxFieldCnt = 11
    Public strFieldNames() As String
    Public strFieldTexts() As String

    Public m_dbClass As FBMlib.CDBOperateOLE

    Public SubInfoL As List(Of SubInfoTable)

    'Public KihonL As List(Of KihonInfoTable)
    Public objControl As Object

    Public Sub GetDataFromControl()
        Select Case _value_type
            Case "TEXT" ' TEXT
                Dim tmpTextBox As System.Windows.Controls.TextBox
                tmpTextBox = CType(objControl, System.Windows.Controls.TextBox)
                _item_value = tmpTextBox.Text
                Exit Select
            Case "COMBOBOX"
                Dim tmpComboBox As System.Windows.Controls.ComboBox
                tmpComboBox = CType(objControl, System.Windows.Controls.ComboBox)
                _item_value = tmpComboBox.Text
                Exit Select
            Case "DATETIME"
                Dim tmpDatetimePicker As System.Windows.Controls.DatePicker
                tmpDatetimePicker = CType(objControl, System.Windows.Controls.DatePicker)
                _item_value = tmpDatetimePicker.SelectedDate
                Exit Select
        End Select
    End Sub

    Public Sub SetItemValueToControl()
        Select Case _value_type
            Case "TEXT" ' TEXT
                Dim tmpTextBox As System.Windows.Controls.TextBox
                tmpTextBox = CType(objControl, System.Windows.Controls.TextBox)
                If _item_value = "NULL" Then
                    tmpTextBox.Text = ""
                Else
                    tmpTextBox.Text = _item_value
                End If
                Exit Select
            Case "COMBOBOX"
                Dim tmpComboBox As System.Windows.Controls.ComboBox
                tmpComboBox = CType(objControl, System.Windows.Controls.ComboBox)
                If _item_value = "NULL" Then
                    tmpComboBox.Text = ""
                Else
                    tmpComboBox.SelectedIndex = -1
                    tmpComboBox.Text = _item_value
                End If
                Exit Select
            Case "DATETIME"
                Dim tmpDatetimePicker As System.Windows.Controls.DatePicker
                tmpDatetimePicker = CType(objControl, System.Windows.Controls.DatePicker)
                If _item_value = "NULL" Then
                    tmpDatetimePicker.SelectedDate = Now
                Else
                    tmpDatetimePicker.SelectedDate = _item_value
                End If
                Exit Select
        End Select
    End Sub

    Public Sub SetDefaultValueToControl()
        Select Case _value_type
            Case "TEXT" ' TEXT
                Dim tmpTextBox As System.Windows.Controls.TextBox
                tmpTextBox = CType(objControl, System.Windows.Controls.TextBox)
                If _default_value = "NULL" Then
                    tmpTextBox.Text = ""
                Else
                    tmpTextBox.Text = _default_value
                End If
                Exit Select
            Case "COMBOBOX"
                Dim tmpComboBox As System.Windows.Controls.ComboBox
                tmpComboBox = CType(objControl, System.Windows.Controls.ComboBox)
                If _default_value = "NULL" Then
                    tmpComboBox.Text = ""
                Else
                    tmpComboBox.Text = _default_value
                End If
                Exit Select
            Case "DATETIME"
                Dim tmpDatetimePicker As System.Windows.Controls.DatePicker
                tmpDatetimePicker = CType(objControl, System.Windows.Controls.DatePicker)
                If _default_value = "NULL" Then
                    tmpDatetimePicker.SelectedDate = Now
                Else
                    tmpDatetimePicker.SelectedDate = _default_value
                End If
                Exit Select
        End Select
    End Sub

    Public Sub New()

    End Sub

    'Add Kiryu 
    Public Sub New(ByVal strFields() As String)
        _ID = strFields(0)                               '0  ID
        _item_name = strFields(1)                 '1  item_name
        _item_cell_name = strFields(2)       '2  item_cell_name
        _item_value = strFields(3)               '3  item_value
        _value_type = strFields(4)               '4  value_type
        _default_value = strFields(5)         '5  default_value
        _disp_sort_ID = strFields(6)           '6  disp_sort_ID
        _update_date = strFields(7)             '7  update_date
        _works_in = strFields(8)                   '8  works_In
        _flg_input = strFields(9)                 '9  flg_input
        '_TypeID = strFields(10)                       '10 type_ID
        _defaultDXFnameflg = strFields(10) '11 _defaultDXFnameflg

    End Sub


    Public Sub BackUp()
        _backup = New KihonInfoTable
        With _backup
            ._ID = Me._ID                               '0  ID
            ._item_name = Me._item_name                 '1  item_name
            ._item_cell_name = Me._item_cell_name       '2  item_cell_name
            ._item_value = Me._item_value               '3  item_value
            ._value_type = Me._value_type               '4  value_type
            ._default_value = Me._default_value         '5  default_value
            ._disp_sort_ID = Me._disp_sort_ID           '6  disp_sort_ID
            ._update_date = Me._update_date             '7  update_date
            ._works_in = Me._works_in                   '8  works_In
            ._flg_input = Me._flg_input                 '9  flg_input
            ._TypeID = Me._TypeID                       '10 type_ID
            ._defaultDXFnameflg = Me._defaultDXFnameflg '11 defaultDXFnameflg ADD 20140914
        End With
    End Sub

    Public Sub copy(ByVal KIT As KihonInfoTable)

        With KIT
            _ID = ._ID                               '0  ID
            _item_name = ._item_name                 '1  item_name
            _item_cell_name = ._item_cell_name       '2  item_cell_name
            _item_value = ._item_value               '3  item_value
            _value_type = ._value_type               '4  value_type
            _default_value = ._default_value         '5  default_value
            _disp_sort_ID = ._disp_sort_ID           '6  disp_sort_ID
            _update_date = ._update_date             '7  update_date
            _works_in = ._works_in                   '8  works_In
            _flg_input = ._flg_input                 '9  flg_input
            _TypeID = ._TypeID                       '10 type_ID
            objControl = .objControl                 '   objControl
            _defaultDXFnameflg = ._defaultDXFnameflg '11 _defaultDXFnameflg
        End With

        BackUp()

    End Sub

    Public Sub SetFieldIndex(ByRef IDR As IDataReader)
        Dim iList As New List(Of Integer)
        '順番を間違いないように！！！！

        iList.Add(GetIndexOfDataReader(K_ID, IDR))                  '0  ID
        iList.Add(GetIndexOfDataReader(K_item_name, IDR))           '1  item_name
        iList.Add(GetIndexOfDataReader(K_item_cell_name, IDR))      '2  item_cell_name
        iList.Add(GetIndexOfDataReader(K_item_value, IDR))          '3  item_value
        iList.Add(GetIndexOfDataReader(K_value_type, IDR))          '4  value_type
        iList.Add(GetIndexOfDataReader(K_default_value, IDR))       '5  default_value
        iList.Add(GetIndexOfDataReader(K_disp_sort_ID, IDR))        '6  disp_sort_ID
        iList.Add(GetIndexOfDataReader(K_update_date, IDR))         '7  update_date
        iList.Add(GetIndexOfDataReader(K_works_In, IDR))            '8  works_In
        iList.Add(GetIndexOfDataReader(K_flg_input, IDR))           '9  flg_input
        iList.Add(GetIndexOfDataReader(K_TypeID, IDR))              '10 type_ID
        iList.Add(GetIndexOfDataReader(K_defaultDXFnameflg, IDR))   '11 _defaultDXFnameflg
        FIELD_INDEX = iList

    End Sub

    Public Shared FIELD_INDEX As New List(Of Integer)

    Public Function GetDataByReader(ByRef iDataReader As IDataReader)
        GetDataByReader = False

        p_set_ID(iDataReader)                                       '0  ID
        p_set_item_name(iDataReader)                                '1	item_name
        p_set_item_cell_name(iDataReader)                           '2	item_cell_name
        p_set_item_value(iDataReader)                               '3	item_value
        p_set_value_type(iDataReader)                               '4	value_type
        p_set_default_value(iDataReader)                            '5	default_value
        p_set_disp_sort_ID(iDataReader)                             '6	disp_sort_ID
        p_set_update_date(iDataReader)                              '7	update_date
        p_set_works_in(iDataReader)                                 '8	works_In
        p_set_flg_input(iDataReader)                                '9	flg_input
        p_set_TypeID(iDataReader)                                   '10 type_ID
        p_set_defaultDXFnameflg(iDataReader)                        '11 defaultDXFnameflg 
        BackUp() 'バックアップ用

        GetDataByReader = True

    End Function

    Private Sub CreateInsertFieldName()
        ReDim strFieldNames(MaxFieldCnt)

        strFieldNames(0) = "ID"
        strFieldNames(1) = "Name"
        strFieldNames(2) = "CellName"
        strFieldNames(3) = "ItemValue"
        strFieldNames(4) = "ValueType"
        strFieldNames(5) = "DefaultValue"
        strFieldNames(6) = "DispSortID"
        strFieldNames(7) = "UpdateDate"
        strFieldNames(8) = "WorksIn"
        strFieldNames(9) = "flgInput"
        strFieldNames(10) = "TypeID"
        strFieldNames(11) = "defaultDXFnameflg"  'SUURI ADD 20140914

    End Sub

    Public Sub CreateInsertFieldText()
        ReDim strFieldTexts(MaxFieldCnt)

        CreateInsertFieldName()

        strFieldTexts(0) = _ID
        strFieldTexts(1) = "'" & _item_name & "'"
        strFieldTexts(2) = "'" & _item_cell_name & "'"
        strFieldTexts(3) = "'" & _item_value & "'"
        strFieldTexts(4) = "'" & _value_type & "'"
        strFieldTexts(5) = "'" & _default_value & "'"
        strFieldTexts(6) = _disp_sort_ID
        strFieldTexts(7) = "'" & _update_date & "'"
        strFieldTexts(8) = "'" & _works_in & "'"
        strFieldTexts(9) = "'" & _flg_input & "'"
        strFieldTexts(10) = _TypeID
        strFieldTexts(11) = _defaultDXFnameflg     'SUURI ADD 20140914

    End Sub

    Private Sub CreateUpdateFieldName()
        ReDim strFieldNames(MaxFieldCnt - 1)

        strFieldNames(0) = "Name"
        strFieldNames(1) = "CellName"
        strFieldNames(2) = "ItemValue"
        strFieldNames(3) = "ValueType"
        strFieldNames(4) = "DefaultValue"
        strFieldNames(5) = "DdispSortID"
        strFieldNames(6) = "UpdateDate"
        strFieldNames(7) = "WorksIn"
        strFieldNames(8) = "flgInput"
        strFieldNames(9) = "TypeID"
        strFieldNames(10) = "defaultDXFnameflg"    'SUURI ADD 20140914

    End Sub

    Public Sub CreateUpdateFieldText()
        ReDim strFieldTexts(MaxFieldCnt - 1)

        CreateUpdateFieldName()

        strFieldTexts(0) = "'" & _item_name & "'"
        strFieldTexts(1) = "'" & _item_cell_name & "'"
        strFieldTexts(2) = "'" & _item_value & "'"
        strFieldTexts(3) = "'" & _value_type & "'"
        strFieldTexts(4) = "'" & _default_value & "'"
        strFieldTexts(5) = _disp_sort_ID
        strFieldTexts(6) = "'" & _update_date & "'"
        strFieldTexts(7) = "'" & _works_in & "'"
        strFieldTexts(8) = "'" & _flg_input & "'"
        strFieldTexts(9) = _TypeID
        strFieldTexts(10) = "'" & _defaultDXFnameflg & "'"    'SUURI ADD 20140914

    End Sub

#Region "データ取得"

    '''' 2013/05/29 DEL
    'Public Function GetDataToList() As List(Of KihonInfoTable)

    '    Dim strSql As String = "SELECT * FROM " & K_kihoninfo & " WHERE TypeID = " & CommonTypeID & " ORDER BY DispSortID"
    '    Dim IDR As IDataReader
    '    Dim bRet As Boolean = False
    '    Dim KihonL As New List(Of KihonInfoTable)

    '    IDR = m_dbClass.DoSelect(strSql)
    '    If Not IDR Is Nothing Then
    '        Dim IDX As New KihonInfoTable
    '        IDX.SetFieldIndex(IDR)
    '        Do While IDR.Read
    '            Dim KID As New KihonInfoTable
    '            KID.GetDataByReader(IDR)
    '            KihonL.Add(KID)
    '            bRet = True
    '        Loop
    '        IDR.Close()
    '    End If

    '    GetDataToList = KihonL

    'End Function

    Public Function GetDataToList() As List(Of KihonInfoTable)

        Dim strSql As String = SQLTEXT()
        Dim IDR As IDataReader
        Dim bRet As Boolean = False
        Dim KihonL As New List(Of KihonInfoTable)
        Dim tempID As String = ""

        IDR = m_dbClass.DoSelect(strSql)
        If Not IDR Is Nothing Then
            Dim KIDX As New KihonInfoTable
            KIDX.SetFieldIndex(IDR)
            Dim SIDX As New SubInfoTable
            SIDX.SetFieldIndex(IDR)
            Do While IDR.Read
                Dim KID As New KihonInfoTable
                KID.GetDataByReader(IDR)
                Dim SID As New SubInfoTable
                SID.GetDataByReader(IDR)

                If tempID = KID.ID Then
                    KihonL(KihonL.Count - 1).SubInfoL.Add(SID)
                Else
                    If KID.SubInfoL Is Nothing Then
                        KID.SubInfoL = New List(Of SubInfoTable)
                    End If
                    KID.SubInfoL.Add(SID)
                    KihonL.Add(KID)
                End If
                tempID = KID.ID
                bRet = True

            Loop
            IDR.Close()
        End If

        GetDataToList = KihonL

    End Function

    Public Function SQLTEXT() As String

        Dim strSql As String
        Dim strSelect As String
        Dim strFrom As String
        Dim strWhere As String
        Dim strOrderBy As String

        strSelect = "SELECT "
        strSelect = strSelect & "KihonInfo.ID AS K_ID,"
        strSelect = strSelect & "KihonInfo.Name AS K_Name,"
        strSelect = strSelect & "KihonInfo.CellName AS K_CellName,"
        strSelect = strSelect & "KihonInfo.ItemValue AS K_ItemValue,"
        strSelect = strSelect & "KihonInfo.ValueType AS K_ValueType,"
        strSelect = strSelect & "KihonInfo.DefaultValue AS K_DefaultValue,"
        strSelect = strSelect & "KihonInfo.DispSortID AS K_DispSortID,"
        strSelect = strSelect & "KihonInfo.UpdateDate AS K_UpdateDate,"
        strSelect = strSelect & "KihonInfo.WorksIn AS K_WorksIn,"
        strSelect = strSelect & "KihonInfo.flgInput AS K_flgInput,"
        strSelect = strSelect & "KihonInfo.TypeID AS K_TypeID,"
        strSelect = strSelect & "KihonInfo.defaultDXFnameflg as K_defaultDXFnameflg,"  'SUURI ADD 20140914
        strSelect = strSelect & "SubInfo.ID AS S_ID,"
        strSelect = strSelect & "SubInfo.KI_ID AS S_KI_ID,"
        strSelect = strSelect & "SubInfo.ItemValue AS S_ItemValue "

        strFrom = "FROM "
        strFrom = strFrom & "KihonInfo LEFT JOIN SubInfo ON "
        strFrom = strFrom & "KihonInfo.ID = SubInfo.KI_ID "

        strWhere = "WHERE "
        strWhere = strWhere & "KihonInfo.TypeID = " & CommonTypeID & " "

        strOrderBy = "ORDER BY "
        strOrderBy = strOrderBy & "KihonInfo.DispSortID,"
        strOrderBy = strOrderBy & "SubInfo.ID"

        strSql = strSelect & strFrom & strWhere & strOrderBy

        SQLTEXT = strSql

    End Function

#End Region

#Region "データ更新"

    Public Function SaveData(Optional ByRef flg_trans As Boolean = True) As Boolean

        Dim bRet As Boolean = True

        If ExistsData() = True Then
            'KihonInfoが登録されている場合、Update
            bRet = UpdateData(flg_trans)
        Else
            'KihonInfoが登録されている場合、Insert
            bRet = InsertData(flg_trans)
        End If

        SaveData = bRet

    End Function

    Public Function ExistsData() As Boolean

        Dim bRet As Boolean = False
        Dim lRet As Long = 0
        Dim strWhere As String = ""

        If _ID.Trim = "" Then
            ExistsData = False
            Exit Function
        End If

        strWhere = "ID = " & _ID.Trim

        lRet = m_dbClass.DoDCount("*", K_kihoninfo, strWhere)
        If lRet > 0 Then
            bRet = True
        End If

        ExistsData = bRet

    End Function

#End Region

#Region "Insert"

    Public Function InsertData(Optional ByRef flg_trans As Boolean = True) As Boolean

        InsertData = True

        Dim strWhere As String = "ID = " & _ID

        CreateInsertFieldText()

        Dim lRet As Long = 0

        lRet = m_dbClass.DoInsert(strFieldNames, K_kihoninfo, strFieldTexts)
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

        Dim lRet As Long = 0
        Dim bRet As Boolean = True

        Dim strWhere As String = "ID = " & _ID

        ReDim strFieldNames(1)
        ReDim strFieldTexts(1)

        strFieldNames(0) = "ItemValue"
        strFieldNames(1) = "UpdateDate"

        strFieldTexts(0) = "'" & _item_value & "'"
        strFieldTexts(1) = "'" & _update_date & "'"

        With m_dbClass
            If flg_trans = True Then
                .BeginTrans()
                lRet = .DoUpdate(strFieldNames, K_kihoninfo, strFieldTexts, strWhere)
                If lRet = 1 Then
                    .CommitTrans()
                Else
                    .RollbackTrans()
                    bRet = False
                End If
            Else
                lRet = .DoUpdate(strFieldNames, K_kihoninfo, strFieldTexts, strWhere)
                If lRet = 1 Then
                Else
                    bRet = False
                End If
            End If
        End With

    End Function

#End Region

    Public Sub CsvOut(ByVal strFileName As String, ByRef system_dbclass As FBMlib.CDBOperateOLE)


        Dim strSql As String = Me.SQLTEXT()
        Dim bRet As Boolean = False
        Dim KihonL As New List(Of KihonInfoTable)
        Dim tempID As String = ""
        Dim strKekka As String = ""

        My.Computer.FileSystem.WriteAllText(strFileName, "ID,item_name,item_cell_name,item_value,value_type,default_value,disp_sort_ID,update_date,works_in,flg_input,defaultDXFnameflg" + vbNewLine, True)

        Dim IDR As IDataReader = system_dbclass.DoSelect(strSql)
        If Not IDR Is Nothing Then
            Me.SetFieldIndex(IDR)
            'Dim SIDX As New SubInfoTable
            'SIDX.SetFieldIndex(IDR)
            Do While IDR.Read
                'Dim KID As New KihonInfoTable
                Me.GetDataByReader(IDR)
                'Dim SID As New SubInfoTable
                'SID.GetDataByReader(IDR)

                KihonL.Add(Me)
                'KID.CsvOut(ExportDirPath + "\" + "db_out_KihonInfo.csv")
                strKekka = _ID & "," & _item_name & "," & _item_cell_name & "," & _item_value & "," & _value_type & "," & _default_value _
           & "," & _disp_sort_ID & "," & _update_date & "," & _works_in & "," & _flg_input & "," & _defaultDXFnameflg & vbNewLine
                My.Computer.FileSystem.WriteAllText(strFileName, strKekka, True)

            Loop
            IDR.Close()
        End If


        'Dim strKekka As String = ""
        'strKekka = _ID & "," & _item_name & "," & _item_cell_name & "," & _item_value & "," & _value_type & "," & _default_value _
        '               & "," & _disp_sort_ID & "," & _update_date & "," & _works_in & "," & _flg_input & "," & _defaultDXFnameflg & vbNewLine

        ''My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & strFileName, strKekka, True)
        'My.Computer.FileSystem.WriteAllText(strFileName, strKekka, True)

    End Sub

    Public Sub CsvInport(ByVal strFileName As String, ByRef dbclass As FBMlib.CDBOperateOLE, ByVal TypeID As Integer)
        'Dim filename As String = My.Application.Info.DirectoryPath & "\" & strFileName
        Dim filename As String = strFileName
        Dim fields As String()
        Dim delimiter As String = ","
        Dim i As Integer = 0

        Dim strSelect As String

        strSelect = "ID,"
        strSelect = strSelect & "Name,"
        strSelect = strSelect & "CellName,"
        strSelect = strSelect & "ItemValue,"
        strSelect = strSelect & "ValueType,"
        strSelect = strSelect & "DefaultValue,"
        strSelect = strSelect & "DispSortID,"
        'strSelect = strSelect & "UpdateDate"
        strSelect = strSelect & "WorksIn,"
        strSelect = strSelect & "flgInput,"
        strSelect = strSelect & "TypeID,"
        strSelect = strSelect & "defaultDXFnameflg"


        Dim strSelMax As String = "SELECT MAX( ID ) FROM KihonInfo  "
        Dim IDR As IDataReader = dbclass.DoSelect(strSelMax)
        IDR.Read()
        Dim MaxID As Integer = IDR.GetValue(0)

        Using parser As New TextFieldParser(filename, System.Text.Encoding.Default)
            parser.SetDelimiters(delimiter)
            fields = parser.ReadFields() 'ヘッダの読み飛ばし
            While Not parser.EndOfData
                ' Read in the fields for the current line
                fields = parser.ReadFields()
                ' Add code here to use data in fields variable.
                If IsNumeric(fields(0)) Then
                    Dim tmp As New KihonInfoTable(fields)
                    Dim strValue As String
                    With tmp
                        MaxID = MaxID + 1
                        strValue = MaxID.ToString & ","
                        strValue = strValue & "'" & ._item_name & "',"
                        strValue = strValue & "'" & ._item_cell_name & "',"
                        strValue = strValue & "'" & ._item_value & "',"
                        strValue = strValue & "'" & ._value_type & "',"
                        strValue = strValue & "'" & ._default_value & "',"
                        strValue = strValue & ._disp_sort_ID & ","
                        'strValue = strValue & "'" & ._update_date & "',"
                        strValue = strValue & "'" & ._works_in & "',"
                        strValue = strValue & "'" & ._flg_input & "',"
                        strValue = strValue & TypeID & ","
                        strValue = strValue & "'" & ._defaultDXFnameflg & "'"

                        Dim strSql As String = " INSERT INTO KihonInfo( " & strSelect & " ) " & " VALUES( " & strValue & " ) "

                        dbclass.ExecuteSQL(strSql)
                    End With

                End If
            End While
        End Using


    End Sub



End Class
