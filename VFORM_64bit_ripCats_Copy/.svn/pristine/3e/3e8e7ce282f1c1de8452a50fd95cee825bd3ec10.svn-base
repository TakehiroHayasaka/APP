Public Class Objects

    Private _id As Integer
    Public Property ID() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
        End Set
    End Property

    Private _objectname As String
    Public Property ObjectName() As String
        Get
            Return _objectname
        End Get
        Set(ByVal value As String)
            _objectname = value
        End Set
    End Property

    Private _objecttype As Integer
    Public Property ObjectType() As Integer
        Get
            Return _objecttype
        End Get
        Set(ByVal value As Integer)
            _objecttype = value
        End Set
    End Property

    Private _p1_id As Integer
    Public Property P1_ID() As Integer
        Get
            Return _p1_id
        End Get
        Set(ByVal value As Integer)
            _p1_id = value
        End Set
    End Property

    Private _p2_id As Integer
    Public Property P2_ID() As Integer
        Get
            Return _p2_id
        End Get
        Set(ByVal value As Integer)
            _p2_id = value
        End Set
    End Property

    Private _p3_id As Integer
    Public Property P3_ID() As Integer
        Get
            Return _p3_id
        End Get
        Set(ByVal value As Integer)
            _p3_id = value
        End Set
    End Property
    Public Sub New()
        _id = -1
        _objectname = ""
        _objecttype = -1
        _p1_id = -1
        _p2_id = -1
        _p3_id = -1
    End Sub

    Dim strFieldNames() As String
    Dim strFieldText() As String
    Dim MaxFieldCnt As Integer = 5

    Private Sub CreateFieldName()
        ReDim strFieldNames(MaxFieldCnt)

        strFieldNames(0) = "Object_ID"
        strFieldNames(1) = "Object_Name"
        strFieldNames(2) = "Object_Type"
        strFieldNames(3) = "Point1_ID"
        strFieldNames(4) = "Point2_ID"
        strFieldNames(5) = "Point3_ID"

    End Sub

    Public Sub CreateFieldText()
        ReDim strFieldText(MaxFieldCnt)

        CreateFieldName()

        strFieldText(0) = _id
        strFieldText(1) = "'" & _objectname & "'"
        strFieldText(2) = _objecttype
        strFieldText(3) = _p1_id + 1
        strFieldText(4) = _p2_id + 1
        strFieldText(5) = _p3_id + 1

    End Sub

    Public Sub SaveObj_toDb()
        CreateFieldText()
        If dbClass.DoInsert(strFieldNames, "Objects", strFieldText) < 0 Then
            MsgBox("DB登録に失敗しました。", MsgBoxStyle.OkOnly, "エラー")
            Exit Sub
        End If
    End Sub
    Public Sub ReadObj_toDb(ByRef IDR As IDataReader)
        _id = IDR.GetValue(0)
        _objectname = IDR.GetValue(1).ToString
        _objecttype = IDR.GetValue(2)
        _p1_id = IDR.GetValue(3) - 1
        _p2_id = IDR.GetValue(4) - 1
        _p3_id = IDR.GetValue(5) - 1
    End Sub
End Class
