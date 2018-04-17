Public Class TypeInfoTable

    Private Const T_type_ID As String = "TypeInfo"

    Private ReadOnly T_ID As String() = {"ID"}                              '0  ID
    Private ReadOnly T_type_name As String() = {"name"}                     '1  名前
    Private ReadOnly T_flg_active As String() = {"flg_active"}              '2  表示フラグ
    Private ReadOnly T_picture_name As String() = {"picture_name"}          '3  画像ファイル名
    Private ReadOnly T_flg_sekkei As String() = {"flg_sekkei"}              '4  flg_sekkei


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

    Private _type_name As String = ""
    Public Property type_name() As String
        Get
            Return _type_name
        End Get
        Set(ByVal value As String)
            _type_name = value
        End Set
    End Property

    Private Sub p_set_type_name(ByRef IDR As IDataReader)
        _type_name = GetDataByIndexOfDataReader(FIELD_INDEX(1), IDR)
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
        _flg_active = GetDataByIndexOfDataReader(FIELD_INDEX(2), IDR)
    End Sub

    Private _picture_name As String = ""
    Public Property picture_name() As String
        Get
            Return _picture_name
        End Get
        Set(ByVal value As String)
            _picture_name = value
        End Set
    End Property

    Private Sub p_set_picture_name(ByRef IDR As IDataReader)
        _picture_name = GetDataByIndexOfDataReader(FIELD_INDEX(3), IDR)
    End Sub
    '20160904 byambaa add start
    Private _flg_sekkei As String = ""
    Public Property flg_sekkei() As String
        Get
            Return _flg_sekkei
        End Get
        Set(ByVal value As String)
            _flg_sekkei = value
        End Set
    End Property

    Private Sub p_set_flg_sekkei(ByRef IDR As IDataReader)
        _flg_sekkei = GetDataByIndexOfDataReader(FIELD_INDEX(4), IDR)
    End Sub
    '20160904 byambaa add end

    Public flgSyusei As Boolean = False
    Public flgNewInput As Boolean = False
    Private _backup As TypeInfoTable
    Public Const MaxFieldCnt = 3
    Public strFieldNames() As String
    Public strFieldTexts() As String

    Public m_dbClass As FBMlib.CDBOperateOLE

    Public Sub New()

    End Sub

    Public Sub BackUp()
        _backup = New TypeInfoTable
        With _backup
            ._ID = Me._ID                           '0  ID
            ._type_name = Me._type_name             '1  type_name
            ._flg_active = Me.flg_active            '2  表示フラグ
            ._picture_name = Me.picture_name        '3  画像ファイル名
            ._flg_sekkei = Me.flg_sekkei            '4  20160904 byambaa(SUSANO) add 

        End With
    End Sub

    Public Sub copy(ByVal TPT As TypeInfoTable)

        With TPT
            _ID = ._ID                              '0  ID
            _type_name = ._type_name                '1  type_name
            _flg_active = .flg_active               '2  表示フラグ
            _picture_name = .picture_name           '3  画像名（フルパス）
            _flg_sekkei = .flg_sekkei               '4  20160904 byambaa(SUSANO) add 

        End With

        BackUp()

    End Sub

    Public Sub SetFieldIndex(ByRef IDR As IDataReader)
        Dim iList As New List(Of Integer)
        '順番を間違いないように！！！！

        iList.Add(GetIndexOfDataReader(T_ID, IDR))                  '0  ID
        iList.Add(GetIndexOfDataReader(T_type_name, IDR))           '1  type_name
        iList.Add(GetIndexOfDataReader(T_flg_active, IDR))          '2  表示フラグ
        iList.Add(GetIndexOfDataReader(T_picture_name, IDR))        '3  画像名（フルパス）
        iList.Add(GetIndexOfDataReader(T_flg_sekkei, IDR))          '4  


        FIELD_INDEX = iList

    End Sub

    Public Shared FIELD_INDEX As New List(Of Integer)

    Public Function GetDataByReader(ByRef iDataReader As IDataReader)
        GetDataByReader = False

        p_set_ID(iDataReader)                                   '0  ID
        p_set_type_name(iDataReader)                            '1	type_name
        p_set_flg_active(iDataReader)                           '2  表示フラグ
        p_set_picture_name(iDataReader)                         '3  画像名（フルパス）
        p_set_flg_sekkei(iDataReader)                           '4

        BackUp() 'バックアップ用

        GetDataByReader = True

    End Function

    Private Sub CreateFieldName()
        ReDim strFieldNames(MaxFieldCnt - 1)

        strFieldNames(0) = "type_name"
        strFieldNames(1) = "flg_active"
        strFieldNames(2) = "picture_path"
        strFieldNames(3) = "flg_sekkei"


    End Sub

    Public Sub CreateFieldText()
        ReDim strFieldTexts(MaxFieldCnt - 1)

        CreateFieldName()

        strFieldTexts(0) = "'" & _type_name & "'"
        strFieldTexts(1) = "'" & _flg_active & "'"
        strFieldTexts(2) = "'" & _picture_name & "'"
        strFieldTexts(3) = "'" & _flg_sekkei & "'"

    End Sub

    Public Sub SetUpdateField()

        ReDim strFieldNames(MaxFieldCnt - 1)
        ReDim strFieldTexts(MaxFieldCnt - 1)

        strFieldNames(0) = "type_name"
        strFieldNames(1) = "flg_active"
        strFieldNames(2) = "picture_path"
        strFieldNames(3) = "flg_sekkei"

        strFieldTexts(0) = "'" & _type_name & "'"
        strFieldTexts(1) = "'" & _flg_active & "'"
        strFieldTexts(2) = "'" & _picture_name & "'"
        strFieldTexts(3) = "'" & _flg_sekkei & "'"

    End Sub

#Region "データ取得"

    Public Function GetDataToList() As List(Of TypeInfoTable)

        Dim strSql As String = "SELECT * FROM " & T_type_ID & " WHERE flg_active = 1 ORDER BY ID"
        Dim IDR As IDataReader
        Dim bRet As Boolean = False
        Dim TypeIDL As List(Of TypeInfoTable) = Nothing

        IDR = m_dbClass.DoSelect(strSql)
        If Not IDR Is Nothing Then
            TypeIDL = New List(Of TypeInfoTable)
            Dim IDX As New TypeInfoTable
            IDX.SetFieldIndex(IDR)
            Do While IDR.Read
                Dim TYD As New TypeInfoTable
                TYD.GetDataByReader(IDR)
                TypeIDL.Add(TYD)
            Loop
            IDR.Close()
        Else
            MsgBox("データ取得できませんでした")
        End If

        GetDataToList = TypeIDL

    End Function

#End Region

#Region "Update"

    Public Function SaveData(Optional ByRef flg_trans As Boolean = True) As Boolean

        SaveData = True

        Dim strWhere As String = "ID = " & _ID

        SetUpdateField()

        Dim lRet As Long = 0
        If flg_trans = True Then
            m_dbClass.BeginTrans()
            lRet = m_dbClass.DoUpdate(strFieldNames, T_type_ID, strFieldTexts, strWhere)
            If lRet = 1 Then
                m_dbClass.CommitTrans()
            Else
                m_dbClass.RollbackTrans()
                SaveData = False
            End If
        Else
            lRet = m_dbClass.DoUpdate(strFieldNames, T_type_ID, strFieldTexts, strWhere)
            If lRet = 1 Then
            Else
                SaveData = False
            End If
        End If

    End Function

#End Region
    '20160904 byambaa ADD
    Public Function GetFlgSekkeiFromData() As List(Of TypeInfoTable)

        Dim strSql As String = "SELECT * FROM " & T_type_ID & " WHERE ID = " & CommonTypeID & ""
        Dim IDR As IDataReader
        Dim TypeIDL As List(Of TypeInfoTable) = Nothing
        'm_dbClass = New FBMlib.CDBOperateOLE

        IDR = m_dbClass.DoSelect(strSql)
        If Not IDR Is Nothing Then
            TypeIDL = New List(Of TypeInfoTable)
            Dim IDX As New TypeInfoTable
            IDX.SetFieldIndex(IDR)
            Do While IDR.Read
                Dim TYD As New TypeInfoTable
                TYD.GetDataByReader(IDR)
                TypeIDL.Add(TYD)
            Loop
            IDR.Close()
        Else
            MsgBox("データ取得できませんでした")
        End If

        GetFlgSekkeiFromData = TypeIDL
    End Function
    '20160904 byambaa ADD
End Class
