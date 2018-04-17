Public Class CameraInfoTable

    Private Const T_CameraInfo As String = "CameraInfo"

    Private ReadOnly T_ID As String() = {"ID"}                              '0  ID
    Private ReadOnly T_camera_name As String() = {"camera_name"}            '1  カメラ名

    Private ReadOnly T_camera_path As String() = {"camera_path"}            '2  カメラ名フォルダ
    Private ReadOnly T_flg_use As String() = {"flg_use"}                    '3  フラグ

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

    Private _camera_name As String = ""
    Public Property camera_name() As String
        Get
            Return _camera_name
        End Get
        Set(ByVal value As String)
            _camera_name = value
        End Set
    End Property

    Private Sub p_set_camera_name(ByRef IDR As IDataReader)
        _camera_name = GetDataByIndexOfDataReader(FIELD_INDEX(1), IDR)
    End Sub

    Private _camera_path As String = ""
    Public Property camera_path() As String
        Get
            Return _camera_path
        End Get
        Set(ByVal value As String)
            _camera_path = value
        End Set
    End Property

    Private Sub p_set_camera_path(ByRef IDR As IDataReader)
        _camera_path = GetDataByIndexOfDataReader(FIELD_INDEX(2), IDR)
    End Sub

    Private _flg_use As String = ""
    Public Property flg_use() As String
        Get
            Return _flg_use
        End Get
        Set(ByVal value As String)
            _flg_use = value
        End Set
    End Property

    Private Sub p_set_flg_use(ByRef IDR As IDataReader)
        _flg_use = GetDataByIndexOfDataReader(FIELD_INDEX(3), IDR)
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

    Private _value_type As String = ""
    Public Property value_type() As String
        Get
            Return _value_type
        End Get
        Set(ByVal value As String)
            _value_type = value
        End Set
    End Property

    Private _default_value As String = ""
    Public Property default_value() As String
        Get
            Return _default_value
        End Get
        Set(ByVal value As String)
            _default_value = value
        End Set
    End Property

    Public flgSyusei As Boolean = False
    Public flgNewInput As Boolean = False
    Private _backup As CameraInfoTable
    Public Const MaxFieldCnt = 3
    Public strFieldNames() As String
    Public strFieldTexts() As String

    Public m_dbClass As FBMlib.CDBOperateOLE

    Public objControl As Object

    Public Sub GetDataFromControl()
        Select Case _value_type
            Case "TEXT" ' TEXT
                Dim tmpTextBox As TextBox
                tmpTextBox = CType(objControl, TextBox)
                _item_value = tmpTextBox.Text
                Exit Select
            Case "CHECKBOX"
                Dim tmpCheckBox As DataGridViewCheckBoxCell
                tmpCheckBox = CType(objControl, DataGridViewCheckBoxCell)
                _item_value = tmpCheckBox.Selected
                Exit Select
            Case "COMBOBOX"
                Dim tmpComboBox As ComboBox
                tmpComboBox = CType(objControl, ComboBox)
                _item_value = tmpComboBox.Text
                Exit Select
        End Select
    End Sub

    Public Sub SetItemValueToControl()
        Select Case _value_type
            Case "TEXT" ' TEXT
                Dim tmpTextBox As TextBox
                tmpTextBox = CType(objControl, TextBox)
                If _item_value = "NULL" Then
                    tmpTextBox.Text = ""
                Else
                    tmpTextBox.Text = _item_value
                End If
                Exit Select
            Case "CHECKBOX"
                Dim tmpCheckBox As DataGridViewCheckBoxCell
                tmpCheckBox = CType(objControl, DataGridViewCheckBoxCell)
                If _item_value = "True" Then
                    tmpCheckBox.Selected = True
                ElseIf _item_value = "False" Then
                    tmpCheckBox.Selected = False
                Else
                    tmpCheckBox.Selected = False
                End If
                Exit Select
            Case "COMBOBOX"
                Dim tmpComboBox As ComboBox
                tmpComboBox = CType(objControl, ComboBox)
                If _item_value = "NULL" Then
                    tmpComboBox.Text = ""
                Else
                    tmpComboBox.Text = _item_value
                End If
                Exit Select
        End Select
    End Sub

    Public Sub SetDefaultValueToControl()
        Select Case _value_type
            Case "TEXT" ' TEXT
                Dim tmpTextBox As TextBox
                tmpTextBox = CType(objControl, TextBox)
                If _default_value = "NULL" Then
                    tmpTextBox.Text = ""
                Else
                    tmpTextBox.Text = _default_value
                End If
                Exit Select
            Case "CHECKBOX"
                Dim tmpCheckBox As CheckBox
                tmpCheckBox = CType(objControl, CheckBox)
                If _default_value = "" Then
                    tmpCheckBox.Checked = False
                ElseIf _default_value = "True" Then
                    tmpCheckBox.Checked = True
                Else
                    tmpCheckBox.Checked = False
                End If
                Exit Select
            Case "COMBOBOX"
                Dim tmpComboBox As ComboBox
                tmpComboBox = CType(objControl, ComboBox)
                If _default_value = "NULL" Then
                    tmpComboBox.Text = ""
                Else
                    tmpComboBox.Text = _default_value
                End If
                Exit Select
        End Select
    End Sub

    Public Sub New()

    End Sub

    Public Sub BackUp()
        _backup = New CameraInfoTable
        With _backup
            ._ID = Me._ID                           '0  ID
            ._camera_name = Me._camera_name         '1  camera_name
            ._camera_path = Me._camera_path         '2  camera_path
            ._flg_use = Me._flg_use                 '3  flg_use
        End With
    End Sub

    Public Sub copy(ByVal CMT As CameraInfoTable)

        With CMT
            _ID = ._ID                              '0  ID
            _camera_name = ._camera_name            '1  camera_name
            _camera_path = ._camera_path            '2  camera_path
            _flg_use = ._flg_use                    '3  flg_use
        End With

        BackUp()

    End Sub

    Public Sub SetFieldIndex(ByRef IDR As IDataReader)
        Dim iList As New List(Of Integer)
        '順番を間違いないように！！！！

        iList.Add(GetIndexOfDataReader(T_ID, IDR))                  '0  ID
        iList.Add(GetIndexOfDataReader(T_camera_name, IDR))         '1  camera_name
        iList.Add(GetIndexOfDataReader(T_camera_path, IDR))         '2  camera_path
        iList.Add(GetIndexOfDataReader(T_flg_use, IDR))             '3  flg_use

        FIELD_INDEX = iList

    End Sub

    Public Shared FIELD_INDEX As New List(Of Integer)

    Public Function GetDataByReader(ByRef iDataReader As IDataReader)
        GetDataByReader = False

        p_set_ID(iDataReader)                                   '0  ID
        p_set_camera_name(iDataReader)                          '1	camera_name
        p_set_camera_path(iDataReader)                          '2	camera_path
        p_set_flg_use(iDataReader)                              '3  flg_use

        BackUp() 'バックアップ用

        GetDataByReader = True

    End Function

    Private Sub CreateFieldName()
        ReDim strFieldNames(MaxFieldCnt - 1)

        strFieldNames(0) = "camera_name"
        strFieldNames(1) = "camera_path"
        strFieldNames(2) = "flg_use"


    End Sub

    Public Sub CreateFieldText()
        ReDim strFieldTexts(MaxFieldCnt - 1)

        CreateFieldName()

        strFieldTexts(0) = "'" & _camera_name & "'"
        strFieldTexts(1) = "'" & _camera_path & "'"
        If _item_value = "True" Then
            strFieldTexts(2) = "1"
        Else
            strFieldTexts(2) = "0"
        End If
        'strFieldTexts(2) = "'" & _flg_use & "'"

    End Sub

    Public Sub SetUpdateField()

        ReDim strFieldNames(MaxFieldCnt - 1)
        ReDim strFieldTexts(MaxFieldCnt - 1)

        strFieldNames(0) = "camera_name"
        strFieldNames(1) = "camera_path"
        strFieldNames(2) = "flg_use"

        strFieldTexts(0) = "'" & _camera_name & "'"
        strFieldTexts(1) = "'" & _camera_path & "'"
        strFieldTexts(2) = "'" & _flg_use & "'"

    End Sub

#Region "データ取得"

    Public Function GetDataToList() As List(Of CameraInfoTable)

        Dim strSql As String = "SELECT * FROM " & T_CameraInfo & " ORDER BY ID"
        Dim IDR As IDataReader
        Dim bRet As Boolean = False
        Dim CameraL As List(Of CameraInfoTable) = Nothing

        IDR = m_dbClass.DoSelect(strSql)
        If Not IDR Is Nothing Then
            CameraL = New List(Of CameraInfoTable)
            Dim IDX As New CameraInfoTable
            IDX.SetFieldIndex(IDR)
            Do While IDR.Read
                Dim CAD As New CameraInfoTable
                CAD.GetDataByReader(IDR)
                'Dim newCheckBox As New CheckBox()
                'Dim objS1 As New Object
                'objS1 = newCheckBox
                'CAD.objControl = objS1
                'CAD.value_type = "CHECKBOX"
                Dim newCombBox As New ComboBox()
                Dim objS1 As New Object
                objS1 = newCombBox
                CAD.objControl = objS1
                CAD.value_type = "COMBOBOX"
                CameraL.Add(CAD)
            Loop
            IDR.Close()
        End If

        GetDataToList = CameraL

    End Function

#End Region

#Region "選択データ取得"

    Public Function GetDataToSelect() As CameraInfoTable

        Dim strSql As String = "SELECT * FROM " & T_CameraInfo & " WHERE flg_use='1' ORDER BY ID"
        Dim IDR As IDataReader
        Dim bRet As Boolean = False
        Dim CameraD As CameraInfoTable = Nothing

        IDR = m_dbClass.DoSelect(strSql)
        If Not IDR Is Nothing Then
            Dim IDX As New CameraInfoTable
            IDX.SetFieldIndex(IDR)
            Do While IDR.Read
                Dim CAD As New CameraInfoTable
                CAD.GetDataByReader(IDR)
                'Dim newCheckBox As New CheckBox()
                'Dim objS1 As New Object
                'objS1 = newCheckBox
                'CAD.objControl = objS1
                'CAD.value_type = "CHECKBOX"
                Dim newCombBox As New ComboBox()
                Dim objS1 As New Object
                objS1 = newCombBox
                CAD.objControl = objS1
                CAD.value_type = "COMBOBOX"
                CameraD = CAD
            Loop
            IDR.Close()
        End If

        GetDataToSelect = CameraD

    End Function

#End Region

#Region "Save"

    Public Function ExistsData() As Boolean

        Dim bRet As Boolean = False
        Dim lRet As Long = 0
        Dim strWhere As String = ""

        'If _ID.Trim = "" Then
        If _camera_name.Trim = "" Then
            ExistsData = False
            Exit Function
        End If

        strWhere = "camera_name = '" & _camera_name.Trim & "'"
        'strWhere = "ID = " & _ID.Trim

        lRet = m_dbClass.DoDCount("*", T_CameraInfo, strWhere)
        If lRet > 0 Then
            bRet = True
        End If

        ExistsData = bRet

    End Function

    Public Function SaveData(Optional ByRef flg_trans As Boolean = True) As Boolean

        Dim bRet As Boolean = True

        If ExistsData() = True Then
            'CameraInfoが登録されている場合、Update
            bRet = UpdateData(flg_trans)
        Else
            'CameraInfoが登録されている場合、Insert
            bRet = InsertData(flg_trans)
        End If

        SaveData = bRet

    End Function

#End Region

#Region "Insert"

    Public Function InsertData(Optional ByRef flg_trans As Boolean = True) As Boolean

        InsertData = True

        CreateFieldText()

        Dim lRet As Long = 0

        lRet = m_dbClass.DoInsert(strFieldNames, T_CameraInfo, strFieldTexts)
        If lRet = 1 Then
        Else
            InsertData = False
        End If

    End Function


#End Region

#Region "Update"

    Public Function UpdateData(Optional ByRef flg_trans As Boolean = True) As Boolean

        UpdateData = True

        'Dim strWhere As String = "ID = " & _ID
        Dim strWhere As String = "camera_name = '" & _camera_name.Trim & "'"

        SetUpdateField()

        Dim lRet As Long = 0
        If flg_trans = True Then
            m_dbClass.BeginTrans()
            lRet = m_dbClass.DoUpdate(strFieldNames, T_CameraInfo, strFieldTexts, strWhere)
            If lRet = 1 Then
                m_dbClass.CommitTrans()
            Else
                m_dbClass.RollbackTrans()
                UpdateData = False
            End If
        Else
            lRet = m_dbClass.DoUpdate(strFieldNames, T_CameraInfo, strFieldTexts, strWhere)
            If lRet = 1 Then
            Else
                UpdateData = False
            End If
        End If

    End Function

    'Public Function UpdateData(Optional ByRef flg_trans As Boolean = True) As Boolean

    '    UpdateData = True

    '    Dim strWhere As String = "ID = " & _ID

    '    ReDim strFieldNames(0)
    '    ReDim strFieldTexts(0)

    '    strFieldNames(0) = "flg_use"
    '    If _item_value = "True" Then
    '        strFieldTexts(0) = "1"
    '    Else
    '        strFieldTexts(0) = "0"
    '    End If

    '    Dim lRet As Long = 0
    '    If flg_trans = True Then
    '        m_dbClass.BeginTrans()
    '        lRet = m_dbClass.DoUpdate(strFieldNames, T_CameraInfo, strFieldTexts, strWhere)
    '        If lRet = 1 Then
    '            m_dbClass.CommitTrans()
    '        Else
    '            m_dbClass.RollbackTrans()
    '            UpdateData = False
    '        End If
    '    Else
    '        lRet = m_dbClass.DoUpdate(strFieldNames, T_CameraInfo, strFieldTexts, strWhere)
    '        If lRet = 1 Then
    '        Else
    '            UpdateData = False
    '        End If
    '    End If

    'End Function


#End Region

End Class
