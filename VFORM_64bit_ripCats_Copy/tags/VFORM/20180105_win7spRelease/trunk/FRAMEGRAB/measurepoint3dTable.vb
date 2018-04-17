Public Class measurepoint3dTable

    Private Const S_measurepoint3d As String = "measurepoint3d"
    Private Const S_sunpo_cell_set As String = "SunpoSetCell"

    Private ReadOnly S_ID As String() = {"ID"}
    Private ReadOnly S_TID As String() = {"TID"}
    Private ReadOnly S_Type As String() = {"Type"}

    Private ReadOnly S_systemlabel As String() = {"systemlabel"}

    Private ReadOnly S_userlabel As String() = {"userlabel"}

    Private ReadOnly S_currentlabel As String() = {"currentlabel"}

    Private ReadOnly S_X As String() = {"X"}

    Private ReadOnly S_Y As String() = {"Y"}                      '

    Private ReadOnly S_Z As String() = {"Z"}                      '

    Private ReadOnly S_meanerror As String() = {"meanerror"}                '9 
    Private ReadOnly S_deverror As String() = {"deverror"}                '10 P2
    Private ReadOnly S_flgDisplay As String() = {"flgDisplay"}                  '11 P3
    Private ReadOnly S_flgLabel As String() = {"flgLabel"}                      '12 


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

    Private _TID As String = ""
    Public Property TID() As String
        Get
            Return _TID
        End Get
        Set(ByVal value As String)
            _TID = value
        End Set
    End Property

    Private Sub p_set_TID(ByRef IDR As IDataReader)
        _TID = GetDataByIndexOfDataReader(FIELD_INDEX(1), IDR)
    End Sub

    Private _Type As String = ""
    Public Property Type() As String
        Get
            Return _Type
        End Get
        Set(ByVal value As String)
            _Type = value
        End Set
    End Property

    Private Sub p_set_Type(ByRef IDR As IDataReader)
        _Type = GetDataByIndexOfDataReader(FIELD_INDEX(2), IDR)
    End Sub

    Private _systemlabel As String = ""
    Public Property systemlabel() As String
        Get
            Return _systemlabel
        End Get
        Set(ByVal value As String)
            _systemlabel = value
        End Set
    End Property

    Private Sub p_set_systemlabel(ByRef IDR As IDataReader)
        _systemlabel = GetDataByIndexOfDataReader(FIELD_INDEX(3), IDR)
    End Sub

    Private _userlabel As String = ""
    Public Property userlabel() As String
        Get
            Return _userlabel
        End Get
        Set(ByVal value As String)
            _userlabel = value
        End Set
    End Property

    Private Sub p_set_userlabel(ByRef IDR As IDataReader)
        _userlabel = GetDataByIndexOfDataReader(FIELD_INDEX(4), IDR)
    End Sub

    Private _currentlabel As String = ""
    Public Property currentlabel() As String
        Get
            Return _currentlabel
        End Get
        Set(ByVal value As String)
            _currentlabel = value
        End Set
    End Property

    Private Sub p_set_currentlabel(ByRef IDR As IDataReader)
        _currentlabel = GetDataByIndexOfDataReader(FIELD_INDEX(5), IDR)
    End Sub

    Private _X As String = ""
    Public Property X() As String
        Get
            Return _X
        End Get
        Set(ByVal value As String)
            _X = value
        End Set
    End Property

    Private Sub p_set_X(ByRef IDR As IDataReader)
        _X = GetDataByIndexOfDataReader(FIELD_INDEX(6), IDR)
    End Sub

    Private _Y As String = ""
    Public Property Y() As String
        Get
            Return _Y
        End Get
        Set(ByVal value As String)
            _Y = value
        End Set
    End Property

    Private Sub p_set_Y(ByRef IDR As IDataReader)
        _Y = GetDataByIndexOfDataReader(FIELD_INDEX(7), IDR)
    End Sub

    Private _Z As String = ""
    Public Property Z() As String
        Get
            Return _Z
        End Get
        Set(ByVal value As String)
            _Z = value
        End Set
    End Property

    Private Sub p_set_Z(ByRef IDR As IDataReader)
        _Z = GetDataByIndexOfDataReader(FIELD_INDEX(8), IDR)
    End Sub

    Private _meanerror As String = ""
    Public Property meanerror() As String
        Get
            Return _meanerror
        End Get
        Set(ByVal value As String)
            _meanerror = value
        End Set
    End Property

    Private Sub p_set_meanerror(ByRef IDR As IDataReader)
        _meanerror = GetDataByIndexOfDataReader(FIELD_INDEX(9), IDR)
    End Sub

    Private _deverror As String = ""
    Public Property deverror() As String
        Get
            Return _deverror
        End Get
        Set(ByVal value As String)
            _deverror = value
        End Set
    End Property

    Private Sub p_set_deverror(ByRef IDR As IDataReader)
        _deverror = GetDataByIndexOfDataReader(FIELD_INDEX(9), IDR)
    End Sub

    Private _flgDisplay As String = ""
    Public Property flgDisplay() As String
        Get
            Return _flgDisplay
        End Get
        Set(ByVal value As String)
            _flgDisplay = value
        End Set
    End Property

    Private Sub p_set_flgDisplay(ByRef IDR As IDataReader)
        _flgDisplay = GetDataByIndexOfDataReader(FIELD_INDEX(10), IDR)
    End Sub

    Private _flgLabel As String = ""
    Public Property flgLabel() As String
        Get
            Return _flgLabel
        End Get
        Set(ByVal value As String)
            _flgLabel = value
        End Set
    End Property
    'susano add 20151109 start
    Private _sokutenmei As String = ""
    Public Property sokutenmei() As String
        Get
            Return _sokutenmei
        End Get
        Set(ByVal value As String)
            _sokutenmei = value
        End Set
    End Property
    'susano add 20151109 end
    Private Sub p_set_flgLabel(ByRef IDR As IDataReader)
        _flgLabel = GetDataByIndexOfDataReader(FIELD_INDEX(11), IDR)
    End Sub

    
    '(20150105 Tezuka Add)End

    Public flgSyusei As Boolean = False
    Public flgNewInput As Boolean = False
    Private _backup As SunpoSetTable
    Public Const MaxFieldCnt = 26
    Public strFieldNames() As String
    Public strFieldTexts() As String
    Public lstCT_ID As List(Of Integer)
    Public objZu As Object()

    Public m_dbClass As CDBOperateOLE

    Public Sub New()
        lstCT_ID = New List(Of Integer)

    End Sub

    Public Sub SetFieldIndex(ByRef IDR As IDataReader)
        Dim iList As New List(Of Integer)
        '順番を間違いないように！！！！

        iList.Add(GetIndexOfDataReader(S_ID, IDR))                  '0  ID
        iList.Add(GetIndexOfDataReader(S_TID, IDR))             '1  ID
        iList.Add(GetIndexOfDataReader(S_Type, IDR))           '2  SunpoPart
        iList.Add(GetIndexOfDataReader(S_systemlabel, IDR))           '3  Type
        iList.Add(GetIndexOfDataReader(S_userlabel, IDR))       '4  systemlabel
        iList.Add(GetIndexOfDataReader(S_currentlabel, IDR))     '5  寸法算出方法(2点間距離：１、線と点の距離：２）

        iList.Add(GetIndexOfDataReader(S_X, IDR))              '6  点群ＩＤ１

        iList.Add(GetIndexOfDataReader(S_Y, IDR))              '7  点群ＩＤ２


        iList.Add(GetIndexOfDataReader(S_Z, IDR))           '9  P1
        iList.Add(GetIndexOfDataReader(S_meanerror, IDR))           '10 P2
        iList.Add(GetIndexOfDataReader(S_deverror, IDR))            '11 P3
        iList.Add(GetIndexOfDataReader(S_flgDisplay, IDR))              '12 
        iList.Add(GetIndexOfDataReader(S_flgLabel, IDR))              '13 

        FIELD_INDEX = iList

    End Sub


    Public Shared FIELD_INDEX As New List(Of Integer)

    Public Function GetDataByReader(ByRef iDataReader As IDataReader)
        GetDataByReader = False

        p_set_ID(iDataReader)                                   '0  ID
        p_set_TID(iDataReader)                              '1	ID
        p_set_Type(iDataReader)                            '2	SunpoPart
        p_set_systemlabel(iDataReader)                            '3	Type
        p_set_userlabel(iDataReader)                        '4	systemlabel
        p_set_currentlabel(iDataReader)                      '5  寸法算出方法(2点間距離：１、線と点の距離：２）

        p_set_X(iDataReader)                               '6  点群ＩＤ１

        p_set_Y(iDataReader)                               '7  点群ＩＤ２

        p_set_Z(iDataReader)                               '8  点群ＩＤ３

        p_set_meanerror(iDataReader)                            '9  P1
        p_set_deverror(iDataReader)   '10 P2
        p_set_flgDisplay(iDataReader)

        p_set_flgLabel(iDataReader)                             '11 P3
        GetDataByReader = True

    End Function

    Private Sub CreateFieldName()
        ReDim strFieldNames(MaxFieldCnt - 1)

        strFieldNames(0) = "ID"
        strFieldNames(1) = "TID"
        strFieldNames(2) = "Type"
        strFieldNames(3) = "systemlabel"
        strFieldNames(4) = "userlabel"                      '4  寸法算出方法(2点間距離：１、線と点の距離：２）"
        strFieldNames(5) = "currentlabel"                               '5  点群ＩＤ１"
        strFieldNames(6) = "X"                               '6  点群ＩＤ２"
        strFieldNames(7) = "Y"                               '7  点群ＩＤ３"
        strFieldNames(8) = "Z"                            '8  P1"
        strFieldNames(9) = "meanerror"                            '9  P2"
        strFieldNames(10) = "deverror"                             '10 P3"
        strFieldNames(11) = "flgDisplay"                               '11 "
        strFieldNames(12) = "flgLabel"                               '12 "

    End Sub

    Public Sub CreateFieldText()
        ReDim strFieldTexts(MaxFieldCnt - 1)

        CreateFieldName()

        strFieldTexts(0) = _ID
        strFieldTexts(1) = _TID
        strFieldTexts(2) = _Type
        strFieldTexts(3) = "'" & _systemlabel & "'"
        strFieldTexts(4) = "'" & _userlabel & "'"                    '4  寸法算出方法(2点間距離：１、線と点の距離：２）"
        strFieldTexts(5) = "'" & _currentlabel & "'"                            '5  点群ＩＤ１"
        strFieldTexts(6) = _X                               '6  点群ＩＤ２"
        strFieldTexts(7) = _Y                               '7  点群ＩＤ３"
        strFieldTexts(8) = _Z                            '8  P1"
        strFieldTexts(9) = _meanerror                            '9  P2"
        strFieldTexts(10) = _deverror                             '10 P3"
        strFieldTexts(11) = _flgDisplay                               '11 "
        strFieldTexts(12) = _flgLabel                               '12 "


    End Sub

    Public Sub CreateField()

        Dim IDX As Integer = 0

        ReDim strFieldNames(IDX)
        ReDim strFieldTexts(IDX)
        strFieldNames(IDX) = "ID"
        strFieldTexts(IDX) = _ID
        IDX += 1

        ReDim Preserve strFieldNames(IDX)
        ReDim Preserve strFieldTexts(IDX)
        strFieldNames(IDX) = "TID"
        strFieldTexts(IDX) = "'" & _TID & "'"
        IDX += 1

        ReDim Preserve strFieldNames(IDX)
        ReDim Preserve strFieldTexts(IDX)
        strFieldNames(IDX) = "Type"
        strFieldTexts(IDX) = "'" & _Type & "'"
        IDX += 1

        ReDim Preserve strFieldNames(IDX)
        ReDim Preserve strFieldTexts(IDX)
        strFieldNames(IDX) = "systemlabel"
        strFieldTexts(IDX) = "'" & _systemlabel & "'"
        IDX += 1

        If _userlabel <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "userlabel"
            strFieldTexts(IDX) = _userlabel
            IDX += 1
        End If

        If _currentlabel <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "currentlabel"
            strFieldTexts(IDX) = _currentlabel
            IDX += 1
        End If

        If _X <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "X"
            strFieldTexts(IDX) = _X
            IDX += 1
        End If

        If _Y <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "Y"
            strFieldTexts(IDX) = _Y
            IDX += 1
        End If

        If _Z <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "Z"
            strFieldTexts(IDX) = _Z
            IDX += 1
        End If

        If _meanerror <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "meanerror"
            strFieldTexts(IDX) = _meanerror
            IDX += 1
        End If

        If _deverror <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "deverror"
            strFieldTexts(IDX) = dblField(_deverror)
            IDX += 1
        End If

        If _flgDisplay <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "flgDisplay"
            strFieldTexts(IDX) = _flgDisplay
            IDX += 1
        End If

        If _flgLabel <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "flgLabel"
            strFieldTexts(IDX) = _flgLabel
            IDX += 1
        End If

       

    End Sub

#Region "データ取得"

    Public Function GetDataToList() As List(Of measurepoint3dTable)

        Dim strSql As String = "SELECT * FROM " & S_measurepoint3d & " ORDER BY ID"
        Dim IDR As IDataReader
        Dim bRet As Boolean = False
        Dim SunpoL As List(Of measurepoint3dTable) = Nothing

        IDR = m_dbClass.DoSelect(strSql)
        If Not IDR Is Nothing Then
            SunpoL = New List(Of measurepoint3dTable)
            Dim IDX As New measurepoint3dTable
            IDX.SetFieldIndex(IDR)
            Do While IDR.Read
                Dim SSD As New measurepoint3dTable
                SSD.GetDataByReader(IDR)
                SunpoL.Add(SSD)
            Loop
            IDR.Close()
        End If

        GetDataToList = SunpoL

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

        lRet = m_dbClass.DoDCount("*", S_measurepoint3d, strWhere)
        If lRet > 0 Then
            bRet = True
        End If

        ExistsData = bRet

    End Function

    Public Function SaveData(Optional ByRef flg_trans As Boolean = True) As Boolean

        Dim bRet As Boolean = True

        If ExistsData() = True Then
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

        'CreateFieldText()
        CreateField()

        Dim lRet As Long = 0

        lRet = m_dbClass.DoInsert(strFieldNames, S_measurepoint3d, strFieldTexts)
        If lRet = 1 Then
        Else
            m_dbClass.RollbackTrans()
            InsertData = False
        End If

    End Function

#End Region


End Class
