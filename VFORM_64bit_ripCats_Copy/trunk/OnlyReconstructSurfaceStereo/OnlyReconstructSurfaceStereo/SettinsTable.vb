Imports HalconDotNet
Public Class SettingsTable

    Public ReadOnly m_strTableName As String = "設定値及び結果"


    Private ReadOnly S_ID As String() = {"ID"}
    Private ReadOnly S_image_size_width As String() = {"image_size_width"}
    Private ReadOnly S_image_size_height As String() = {"image_size_height"}
    Private ReadOnly S_camera_param As String() = {"camera_param"}
    Private ReadOnly S_bounding_box As String() = {"bounding_box"}
    Private ReadOnly S_cleanup_min As String() = {"cleanup_min"}
    Private ReadOnly S_persistence As String() = {"persistence"}
    Private ReadOnly S_rectif_interpolation As String() = {"rectif_interpolation"}
    Private ReadOnly S_rectif_sub_sampling As String() = {"rectif_sub_sampling"}
    Private ReadOnly S_sub_sampling_step As String() = {"sub_sampling_step"}
    Private ReadOnly S_disparity_method As String() = {"disparity_method"}
    Private ReadOnly S_binocular_method As String() = {"binocular_method"}
    Private ReadOnly S_binocular_num_levels As String() = {"binocular_num_levels"}
    Private ReadOnly S_binocular_mask_width As String() = {"binocular_mask_width"}
    Private ReadOnly S_binocular_mask_height As String() = {"binocular_mask_height"}
    Private ReadOnly S_binocular_texture_thresh As String() = {"binocular_texture_thresh"}
    Private ReadOnly S_binocular_score_thresh As String() = {"binocular_score_thresh"}
    Private ReadOnly S_binocular_filter As String() = {"binocular_filter"}
    Private ReadOnly S_binocular_sub_disparity As String() = {"binocular_sub_disparity"}
    Private ReadOnly S_point_meshing As String() = {"point_meshing"}
    Private ReadOnly S_poisson_depth As String() = {"poisson_depth"}
    Private ReadOnly S_poisson_solver_divide As String() = {"poisson_solver_divide"}
    Private ReadOnly S_poisson_samples_per_node As String() = {"poisson_samples_per_node"}
    Private ReadOnly S_KoujiPath As String() = {"KoujiPath"}
    Private ReadOnly S_SelectedImageIDs As String() = {"SelectedImageIDs"}
    Private ReadOnly S_OutputPath As String() = {"OutputPath"}
    Private ReadOnly S_ImageHenkanTime As String() = {"ImageHenkanTime"}
    Private ReadOnly S_ReconstructTime As String() = {"ReconstructTime"}
    Private ReadOnly S_OutputTime As String() = {"OutputTime"}
    Private ReadOnly S_CreateDate As String() = {"CreateDate"}

    Private _ID As String = ""
    Public Property ID() As String
        Get
            Return _ID
        End Get
        Set(ByVal value As String)
            _ID = value
        End Set
    End Property

    Private _image_size_width As String = ""
    Public Property image_size_width() As String
        Get
            Return _image_size_width
        End Get
        Set(ByVal value As String)
            _image_size_width = value
        End Set
    End Property

    Private _image_size_height As String = ""
    Public Property image_size_height() As String
        Get
            Return _image_size_height
        End Get
        Set(ByVal value As String)
            _image_size_height = value
        End Set
    End Property

    Private _camera_param As String = ""
    Public Property camera_param() As String
        Get
            Return _camera_param
        End Get
        Set(ByVal value As String)
            _camera_param = value
        End Set
    End Property

    Private _bounding_box As String = "80"
    Public Property bounding_box() As String
        Get
            Return _bounding_box
        End Get
        Set(ByVal value As String)
            _bounding_box = value
        End Set
    End Property

    Private _cleanup_min As String = "0.05"
    Public Property cleanup_min() As String
        Get
            Return _cleanup_min
        End Get
        Set(ByVal value As String)
            _cleanup_min = value
        End Set
    End Property

    Private _persistence As Boolean = False

    Public Property persistence() As Boolean
        Get
            Return _persistence
        End Get
        Set(ByVal value As Boolean)
            _persistence = value
        End Set
    End Property

    Private _rectif_interpolation As String = "bilinear"
    Public Property rectif_interpolation() As String
        Get
            Return _rectif_interpolation
        End Get
        Set(ByVal value As String)
            _rectif_interpolation = value
        End Set
    End Property

    Private _rectif_sub_sampling As String = "1"
    Public Property rectif_sub_sampling() As String
        Get
            Return _rectif_sub_sampling
        End Get
        Set(ByVal value As String)
            _rectif_sub_sampling = value
        End Set
    End Property

    Private _sub_sampling_step As String = "3"
    Public Property sub_sampling_step() As String
        Get
            Return _sub_sampling_step
        End Get
        Set(ByVal value As String)
            _sub_sampling_step = value
        End Set
    End Property

    Private _disparity_method As String = "binocular"
    Public Property disparity_method() As String
        Get
            Return _disparity_method
        End Get
        Set(ByVal value As String)
            _disparity_method = value
        End Set
    End Property

    Private _binocular_method As String = "ncc"
    Public Property binocular_method() As String
        Get
            Return _binocular_method
        End Get
        Set(ByVal value As String)
            _binocular_method = value
        End Set
    End Property

    Private _binocular_num_levels As String = "3"
    Public Property binocular_num_levels() As String
        Get
            Return _binocular_num_levels
        End Get
        Set(ByVal value As String)
            _binocular_num_levels = value
        End Set
    End Property

    Private _binocular_mask_width As String = "21"
    Public Property binocular_mask_width() As String
        Get
            Return _binocular_mask_width
        End Get
        Set(ByVal value As String)
            _binocular_mask_width = value
        End Set
    End Property

    Private _binocular_mask_height As String = "21"
    Public Property binocular_mask_height() As String
        Get
            Return _binocular_mask_height
        End Get
        Set(ByVal value As String)
            _binocular_mask_height = value
        End Set
    End Property

    Private _binocular_texture_thresh As String = "0"
    Public Property binocular_texture_thresh() As String
        Get
            Return _binocular_texture_thresh
        End Get
        Set(ByVal value As String)
            _binocular_texture_thresh = value
        End Set
    End Property

    Private _binocular_score_thresh As String = "0.8"
    Public Property binocular_score_thresh() As String
        Get
            Return _binocular_score_thresh
        End Get
        Set(ByVal value As String)
            _binocular_score_thresh = value
        End Set
    End Property

    Private _binocular_filter As String = "left_right_check"
    Public Property binocular_filter() As String
        Get
            Return _binocular_filter
        End Get
        Set(ByVal value As String)
            _binocular_filter = value
        End Set
    End Property

    Private _binocular_sub_disparity As String = "interpolation"
    Public Property binocular_sub_disparity() As String
        Get
            Return _binocular_sub_disparity
        End Get
        Set(ByVal value As String)
            _binocular_sub_disparity = value
        End Set
    End Property

    Private _point_meshing As String = "none"
    Public Property point_meshing() As String
        Get
            Return _point_meshing
        End Get
        Set(ByVal value As String)
            _point_meshing = value
        End Set
    End Property

    Private _poisson_depth As String = "8"
    Public Property poisson_depth() As String
        Get
            Return _poisson_depth
        End Get
        Set(ByVal value As String)
            _poisson_depth = value
        End Set
    End Property

    Private _poisson_solver_divide As String = "8"
    Public Property poisson_solver_divide() As String
        Get
            Return _poisson_solver_divide
        End Get
        Set(ByVal value As String)
            _poisson_solver_divide = value
        End Set
    End Property

    Private _poisson_samples_per_node As String = "30"
    Public Property poisson_samples_per_node() As String
        Get
            Return _poisson_samples_per_node
        End Get
        Set(ByVal value As String)
            _poisson_samples_per_node = value
        End Set
    End Property

    Private _KoujiPath As String = ""
    Public Property KoujiPath() As String
        Get
            Return _KoujiPath
        End Get
        Set(ByVal value As String)
            _KoujiPath = value
        End Set
    End Property

    Private _SelectedImageIDs As String = ""
    Public Property SelectedImageIDs() As String
        Get
            Return _SelectedImageIDs
        End Get
        Set(ByVal value As String)
            _SelectedImageIDs = value
        End Set
    End Property

    Private _OutputPath As String = ""
    Public Property OutputPath() As String
        Get
            Return _OutputPath
        End Get
        Set(ByVal value As String)
            _OutputPath = value
        End Set
    End Property

    Private _ImageHenkanTime As String = ""
    Public Property ImageHenkanTime() As String
        Get
            Return _ImageHenkanTime
        End Get
        Set(ByVal value As String)
            _ImageHenkanTime = value
        End Set
    End Property

    Private _ReconstructTime As String = ""
    Public Property ReconstructTime() As String
        Get
            Return _ReconstructTime
        End Get
        Set(ByVal value As String)
            _ReconstructTime = value
        End Set
    End Property

    Private _OutputTime As String = ""
    Public Property OutputTime() As String
        Get
            Return _OutputTime
        End Get
        Set(ByVal value As String)
            _OutputTime = value
        End Set
    End Property

    Private _CreateDate As String = ""
    Public Property CreateDate() As String
        Get
            Return _CreateDate
        End Get
        Set(ByVal value As String)
            _CreateDate = value
        End Set
    End Property


    Public strFieldNames() As String
    Public strFieldTexts() As String
    Public m_dbClass As CDBOperateOLE
    Public hv_ResultObject3D As HTuple
    Public hv_StereoModelID As HTuple
    Public strErrorMessage As String = ""
    Public Sub New()

    End Sub

    Public Sub New(objSettingData As SettingsTable)


        _image_size_width = objSettingData.image_size_width

        _image_size_height = objSettingData.image_size_height

        _camera_param = objSettingData.camera_param

        _bounding_box = objSettingData.bounding_box

        _cleanup_min = objSettingData.cleanup_min

        _persistence = objSettingData.persistence

        _rectif_interpolation = objSettingData.rectif_interpolation

        _rectif_sub_sampling = objSettingData.rectif_sub_sampling

        _sub_sampling_step = objSettingData.sub_sampling_step

        _disparity_method = objSettingData.disparity_method

        _binocular_method = objSettingData.binocular_method

        _binocular_num_levels = objSettingData.binocular_num_levels

        _binocular_mask_width = objSettingData.binocular_mask_width

        _binocular_mask_height = objSettingData.binocular_mask_height

        _binocular_texture_thresh = objSettingData.binocular_texture_thresh

        _binocular_score_thresh = objSettingData.binocular_score_thresh

        _binocular_filter = objSettingData.binocular_filter

        _binocular_sub_disparity = objSettingData.binocular_sub_disparity

        _point_meshing = objSettingData.point_meshing

        _poisson_depth = objSettingData.poisson_depth

        _poisson_solver_divide = objSettingData.poisson_solver_divide

        _poisson_samples_per_node = objSettingData.poisson_samples_per_node

        _KoujiPath = objSettingData._KoujiPath

        _SelectedImageIDs = objSettingData.SelectedImageIDs

        _OutputPath = objSettingData.OutputPath

        _ImageHenkanTime = objSettingData.ImageHenkanTime

        _ReconstructTime = objSettingData.ReconstructTime

        _OutputTime = objSettingData.OutputTime

        _CreateDate = objSettingData.CreateDate

        m_dbClass = objSettingData.m_dbClass

    End Sub
    Public Function GetLatest()
        ' MsgBox("n1")
        '   Dim IDR As IDataReader = CreateRecordset("SELECT TOP 1 * FROM " + m_strTableName + " ORDER BY ID DESC")
        Dim IDR As IDataReader = CreateRecordset("SELECT * FROM " + m_strTableName + " ORDER BY ID DESC")
        ' MsgBox("n2")
        If Not IDR Is Nothing Then
            Do While IDR.Read
                _ID = IDR.GetInt32(0)

                If Not IDR.IsDBNull(1) Then
                    _image_size_width = IDR.GetInt32(1)
                End If

                If Not IDR.IsDBNull(2) Then
                    _image_size_height = IDR.GetInt32(2)
                End If

                If Not IDR.IsDBNull(3) Then
                    _camera_param = IDR.GetString(3)
                End If

                If Not IDR.IsDBNull(4) Then
                    _bounding_box = IDR.GetDouble(4)
                End If

                If Not IDR.IsDBNull(5) Then
                    _cleanup_min = IDR.GetDouble(5)
                End If

                If Not IDR.IsDBNull(6) Then
                    _persistence = If(IDR.GetInt32(6) = 0, False, True)
                End If

                If Not IDR.IsDBNull(7) Then
                    _rectif_interpolation = IDR.GetString(7)
                End If

                If Not IDR.IsDBNull(8) Then
                    _rectif_sub_sampling = IDR.GetDouble(8)
                End If

                If Not IDR.IsDBNull(9) Then
                    _sub_sampling_step = IDR.GetInt32(9)
                End If

                If Not IDR.IsDBNull(10) Then
                    _disparity_method = IDR.GetString(10)
                End If

                If Not IDR.IsDBNull(11) Then
                    _binocular_method = IDR.GetString(11)
                End If

                If Not IDR.IsDBNull(12) Then
                    _binocular_num_levels = IDR.GetInt32(12)
                End If

                If Not IDR.IsDBNull(13) Then
                    _binocular_mask_width = IDR.GetInt32(13)
                End If

                If Not IDR.IsDBNull(14) Then
                    _binocular_mask_height = IDR.GetInt32(14)
                End If

                If Not IDR.IsDBNull(15) Then
                    _binocular_texture_thresh = IDR.GetDouble(15)
                End If

                If Not IDR.IsDBNull(16) Then
                    _binocular_score_thresh = IDR.GetDouble(16)
                End If

                If Not IDR.IsDBNull(17) Then
                    _binocular_filter = IDR.GetString(17)
                End If

                If Not IDR.IsDBNull(18) Then
                    _binocular_sub_disparity = IDR.GetString(18)
                End If

                If Not IDR.IsDBNull(19) Then
                    _point_meshing = IDR.GetString(19)
                End If

                If Not IDR.IsDBNull(20) Then
                    _poisson_depth = IDR.GetInt32(20)
                End If

                If Not IDR.IsDBNull(21) Then
                    _poisson_solver_divide = IDR.GetInt32(21)
                End If

                If Not IDR.IsDBNull(22) Then
                    _poisson_samples_per_node = IDR.GetInt32(22)
                End If

                If Not IDR.IsDBNull(23) Then
                    _KoujiPath = IDR.GetString(23)
                End If

                If Not IDR.IsDBNull(24) Then
                    _SelectedImageIDs = IDR.GetString(24)
                End If

                If Not IDR.IsDBNull(25) Then
                    _OutputPath = IDR.GetString(25)
                End If

                If Not IDR.IsDBNull(26) Then
                    _ImageHenkanTime = IDR.GetDouble(26)
                End If

                If Not IDR.IsDBNull(27) Then
                    _ReconstructTime = IDR.GetDouble(27)
                End If

                If Not IDR.IsDBNull(28) Then
                    _OutputTime = IDR.GetDouble(28)
                End If

                If Not IDR.IsDBNull(29) Then
                    _CreateDate = IDR.GetDateTime(29)
                End If
                Exit Do
            Loop
            IDR.Close()
        End If

        GetLatest = True
    End Function



    Public Function CreateRecordset( _
    Optional ByVal strSQL As String = "" _
    ) As IDataReader

        Dim IDR As IDataReader
        IDR = m_dbClass.DoSelect(strSQL)

        CreateRecordset = IDR

    End Function

    Public Function InsertData(Optional ByRef flg_trans As Boolean = True) As Boolean

        InsertData = True

        Dim strWhere As String = "ID = " & _ID

        'CreateFieldText()
        CreateField()

        Dim lRet As Long = 0

        lRet = m_dbClass.DoInsert(strFieldNames, m_strTableName, strFieldTexts)
        If lRet = 1 Then
        Else
            m_dbClass.RollbackTrans()
            InsertData = False
        End If

    End Function



    Public Sub CreateField()

        Dim IDX As Integer = 0

        If _image_size_width <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "image_size_width"
            strFieldTexts(IDX) = "'" & _image_size_width & "'"
            IDX += 1

        End If
        If _image_size_height <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "image_size_height"
            strFieldTexts(IDX) = "'" & _image_size_height & "'"
            IDX += 1

        End If
        If _camera_param <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "camera_param"
            strFieldTexts(IDX) = "'" & _camera_param & "'"
            IDX += 1

        End If
        If _bounding_box <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "bounding_box"
            strFieldTexts(IDX) = "'" & _bounding_box & "'"
            IDX += 1

        End If
        If _cleanup_min <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "cleanup_min"
            strFieldTexts(IDX) = "'" & _cleanup_min & "'"
            IDX += 1

        End If
        If _persistence = True Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "persistence"
            strFieldTexts(IDX) = "'" & "1" & "'"
            IDX += 1

        End If
        If _rectif_interpolation <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "rectif_interpolation"
            strFieldTexts(IDX) = "'" & _rectif_interpolation & "'"
            IDX += 1

        End If
        If _rectif_sub_sampling <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "rectif_sub_sampling"
            strFieldTexts(IDX) = "'" & _rectif_sub_sampling & "'"
            IDX += 1

        End If
        If _sub_sampling_step <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "sub_sampling_step"
            strFieldTexts(IDX) = "'" & _sub_sampling_step & "'"
            IDX += 1

        End If
        If _disparity_method <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "disparity_method"
            strFieldTexts(IDX) = "'" & _disparity_method & "'"
            IDX += 1

        End If
        If _binocular_method <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "binocular_method"
            strFieldTexts(IDX) = "'" & _binocular_method & "'"
            IDX += 1

        End If
        If _binocular_num_levels <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "binocular_num_levels"
            strFieldTexts(IDX) = "'" & _binocular_num_levels & "'"
            IDX += 1

        End If
        If _binocular_mask_width <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "binocular_mask_width"
            strFieldTexts(IDX) = "'" & _binocular_mask_width & "'"
            IDX += 1

        End If
        If _binocular_mask_height <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "binocular_mask_height"
            strFieldTexts(IDX) = "'" & _binocular_mask_height & "'"
            IDX += 1

        End If
        If _binocular_texture_thresh <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "binocular_texture_thresh"
            strFieldTexts(IDX) = "'" & _binocular_texture_thresh & "'"
            IDX += 1

        End If
        If _binocular_score_thresh <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "binocular_score_thresh"
            strFieldTexts(IDX) = "'" & _binocular_score_thresh & "'"
            IDX += 1

        End If
        If _binocular_filter <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "binocular_filter"
            strFieldTexts(IDX) = "'" & _binocular_filter & "'"
            IDX += 1

        End If
        If _binocular_sub_disparity <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "binocular_sub_disparity"
            strFieldTexts(IDX) = "'" & _binocular_sub_disparity & "'"
            IDX += 1

        End If
        If _point_meshing <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "point_meshing"
            strFieldTexts(IDX) = "'" & _point_meshing & "'"
            IDX += 1

        End If
        If _poisson_depth <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "poisson_depth"
            strFieldTexts(IDX) = "'" & _poisson_depth & "'"
            IDX += 1

        End If
        If _poisson_solver_divide <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "poisson_solver_divide"
            strFieldTexts(IDX) = "'" & _poisson_solver_divide & "'"
            IDX += 1

        End If
        If _poisson_samples_per_node <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "poisson_samples_per_node"
            strFieldTexts(IDX) = "'" & _poisson_samples_per_node & "'"
            IDX += 1

        End If
        If _KoujiPath <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "KoujiPath"
            strFieldTexts(IDX) = "'" & _KoujiPath & "'"
            IDX += 1

        End If
        If _SelectedImageIDs <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "SelectedImageIDs"
            strFieldTexts(IDX) = "'" & _SelectedImageIDs & "'"
            IDX += 1

        End If
        If _OutputPath <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "OutputPath"
            strFieldTexts(IDX) = "'" & _OutputPath & "'"
            IDX += 1

        End If
        If _ImageHenkanTime <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "ImageHenkanTime"
            strFieldTexts(IDX) = "'" & _ImageHenkanTime & "'"
            IDX += 1

        End If
        If _ReconstructTime <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "ReconstructTime"
            strFieldTexts(IDX) = "'" & _ReconstructTime & "'"
            IDX += 1

        End If
        If _OutputTime <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "OutputTime"
            strFieldTexts(IDX) = "'" & _OutputTime & "'"
            IDX += 1

        End If
        If _CreateDate <> "" Then
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
            strFieldNames(IDX) = "CreateDate"
            strFieldTexts(IDX) = "'" & _CreateDate & "'"
            IDX += 1

        End If


    End Sub

End Class


