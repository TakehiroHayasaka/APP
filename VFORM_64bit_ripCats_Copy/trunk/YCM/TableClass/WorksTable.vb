Public Class WorksTable

    Private Const K_works As String = "Works"
    Private Const K_kihoninfo As String = "Kihoninfo"
    Private Const K_SunpoSet As String = "SunpoSet"
    Private Const K_CameraInfo As String = "CameraInfo"
    Private Const K_SunpoCalcHouhou As String = "SunpoCalcHouhou"

    Private ReadOnly W_ID As String() = {"WK_ID", "ID"}                                 '0  ID
    Private ReadOnly W_SavePath As String() = {"WK_SavePath", "SavePath"}               '1  工事データ保存先
    Private ReadOnly W_CreateDate As String() = {"WK_CreateDate", "CreateDate"}         '2  工事データ作成日
    Private ReadOnly W_MeasureDate As String() = {"WK_MeasureDate", "MeasureDate"}      '3  計測日
    Private ReadOnly W_TypeID As String() = {"WK_TypeID", "TypeID"}                     '4  TypeID
    Private ReadOnly W_flg_Kihon As String() = {"WK_flg_Kihon", "flg_Kihon"}            '5  基本情報入力フラグ
    Private ReadOnly W_flg_Kiteiti As String() = {"WK_flg_Kiteiti", "flg_Kiteiti"}      '6  規定値入力フラグ
    Private ReadOnly W_flg_Gazou As String() = {"WK_flg_Gazou", "flg_Gazou"}            '7  画像入力フラグ
    Private ReadOnly W_flg_Kaiseki As String() = {"WK_flg_Kaiseki", "flg_Kaiseki"}      '8  解析入力フラグ
    Private ReadOnly W_flg_Kakunin As String() = {"WK_flg_Kakunin", "flg_Kakunin"}      '9  寸法確認入力フラグ
    Private ReadOnly W_flg_Out As String() = {"WK_flg_Out", "flg_Out"}                  '10 検査表出力フラグ

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

    Private _SavePath As String = ""
    Public Property SavePath() As String
        Get
            Return _SavePath
        End Get
        Set(ByVal value As String)
            _SavePath = value
        End Set
    End Property

    Private Sub p_set_SavePath(ByRef IDR As IDataReader)
        _SavePath = GetDataByIndexOfDataReader(FIELD_INDEX(1), IDR)
    End Sub

    Private _CreateDate As String = ""
    Public Property CreateDate() As String
        Get
            Return _CreateDate
        End Get
        Set(ByVal value As String)
            _CreateDate = value
        End Set
    End Property

    Private Sub p_set_CreateDate(ByRef IDR As IDataReader)
        _CreateDate = GetDataByIndexOfDataReader(FIELD_INDEX(2), IDR)
    End Sub

    Private _MeasureDate As String = ""
    Public Property MeasureDate() As String
        Get
            Return _MeasureDate
        End Get
        Set(ByVal value As String)
            _MeasureDate = value
        End Set
    End Property

    Private Sub p_set_MeasureDate(ByRef IDR As IDataReader)
        _MeasureDate = GetDataByIndexOfDataReader(FIELD_INDEX(3), IDR)
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

    Private _flg_Kihon As String = ""
    Public Property flg_Kihon() As String
        Get
            Return _flg_Kihon
        End Get
        Set(ByVal value As String)
            _flg_Kihon = value
        End Set
    End Property

    Private Sub p_set_flg_Kihon(ByRef IDR As IDataReader)
        _flg_Kihon = GetDataByIndexOfDataReader(FIELD_INDEX(5), IDR)
    End Sub

    Private _flg_Kiteiti As String = ""
    Public Property flg_Kiteiti() As String
        Get
            Return _flg_Kiteiti
        End Get
        Set(ByVal value As String)
            _flg_Kiteiti = value
        End Set
    End Property

    Private Sub p_set_flg_Kiteiti(ByRef IDR As IDataReader)
        _flg_Kiteiti = GetDataByIndexOfDataReader(FIELD_INDEX(6), IDR)
    End Sub

    Private _flg_Gazou As String = ""
    Public Property flg_Gazou() As String
        Get
            Return _flg_Gazou
        End Get
        Set(ByVal value As String)
            _flg_Gazou = value
        End Set
    End Property

    Private Sub p_set_flg_Gazou(ByRef IDR As IDataReader)
        _flg_Gazou = GetDataByIndexOfDataReader(FIELD_INDEX(7), IDR)
    End Sub

    Private _flg_Kaiseki As String = ""
    Public Property flg_Kaiseki() As String
        Get
            Return _flg_Kaiseki
        End Get
        Set(ByVal value As String)
            _flg_Kaiseki = value
        End Set
    End Property

    Private Sub p_set_flg_Kaiseki(ByRef IDR As IDataReader)
        _flg_Kaiseki = GetDataByIndexOfDataReader(FIELD_INDEX(8), IDR)
    End Sub

    Private _flg_Kakunin As String = ""
    Public Property flg_Kakunin() As String
        Get
            Return _flg_Kakunin
        End Get
        Set(ByVal value As String)
            _flg_Kakunin = value
        End Set
    End Property

    Private Sub p_set_flg_Kakunin(ByRef IDR As IDataReader)
        _flg_Kakunin = GetDataByIndexOfDataReader(FIELD_INDEX(9), IDR)
    End Sub

    Private _flg_Out As String = ""
    Public Property flg_Out() As String
        Get
            Return _flg_Out
        End Get
        Set(ByVal value As String)
            _flg_Out = value
        End Set
    End Property

    Private Sub p_set_flg_Out(ByRef IDR As IDataReader)
        _flg_Out = GetDataByIndexOfDataReader(FIELD_INDEX(10), IDR)
    End Sub

    Public flgSyusei As Boolean = False
    Public flgNewInput As Boolean = False
    Private _backup As WorksTable
    Public Const MaxFieldCnt = 10
    Public strFieldNames() As String
    Public strFieldTexts() As String

    Public m_dbClass As FBMlib.CDBOperateOLE

    Public WorksKihonL As List(Of WorksKihonTable)              'WorksKihonテーブルリスト


    Public KihonL As List(Of KihonInfoTable)                    'KihonInfoテーブルリスト


    Public SubInfoL As List(Of SubInfoTable)                    'SubInfoTableテーブルリスト


    Public SunpoSetL As List(Of SunpoSetTable)                  'SunpoSetテーブルリスト


    Public SunpoSetCellL As List(Of SunpoSetTable)              'SunpoSetセルテーブルリスト


    Public CameraL As List(Of CameraInfoTable)                  'CameraInfoテーブルリスト


    Public ExcelTemplateL As List(Of ExcelTemplateTable)        'ExcelTemplateテーブルリスト


    Public CameraD As CameraInfoTable                           '選択したカメラ情報

    Public SunpoCalcHouhouL As List(Of SunpoCalcHouhouTable)    '寸法計算方法テーブルリスト


    Public m_folder_flg As Boolean                              '工事データ保存先存在フラグ(true:存在　false:存在しない)
    Public m_search_flg As Boolean                              '検索フラグ(true:表示　false:非表示)

    Public Sub New()

    End Sub

    Public Sub BackUp()
        _backup = New WorksTable
        With _backup
            ._ID = Me._ID                           '0  ID
            ._SavePath = Me._SavePath               '1  SavePath
            ._CreateDate = Me._CreateDate           '2  CreatePath
            ._MeasureDate = Me._MeasureDate         '3  MeasureDate
            ._TypeID = Me._TypeID                   '4  TypeID
            ._flg_Kihon = Me._flg_Kihon             '5  flg_Kihon 基本情報入力フラグ
            ._flg_Kiteiti = Me._flg_Kiteiti         '6  flg_Kiteiti 規定値入力フラグ
            ._flg_Gazou = Me._flg_Gazou             '7  flg_Gazou 画像入力フラグ
            ._flg_Kaiseki = Me._flg_Kaiseki         '8  flg_Kaiseki 解析入力フラグ
            ._flg_Kakunin = Me._flg_Kakunin         '9  flg_Kakunin 寸法確認入力フラグ
            ._flg_Out = Me._flg_Out                 '10 flg_Out 検査表出力フラグ
        End With
    End Sub

    Public Sub copy(ByVal WKT As WorksTable)

        With WKT
            _ID = ._ID                              '0  ID
            _SavePath = ._SavePath                  '1  SavePath
            _CreateDate = ._CreateDate              '2  CreatePath
            _MeasureDate = ._MeasureDate            '3  MeasureDate
            _TypeID = ._TypeID                      '4  TypeID
            _flg_Kihon = ._flg_Kihon                '5  flg_Kihon 基本情報入力フラグ
            _flg_Kiteiti = ._flg_Kiteiti            '6  flg_Kiteiti 規定値入力フラグ
            _flg_Gazou = ._flg_Gazou                '7  flg_Gazou 画像入力フラグ
            _flg_Kaiseki = ._flg_Kaiseki            '8  flg_Kaiseki 解析入力フラグ
            _flg_Kakunin = ._flg_Kakunin            '9  flg_Kakunin 寸法確認入力フラグ
            _flg_Out = ._flg_Out                    '10 flg_Out 検査表出力フラグ
            KihonInfoCopy(.KihonL)
            'SunpoCalcHouhouCopy(.SunpoCalcHouhouL)
            'If KihonL Is Nothing Then
            '    KihonL = New List(Of KihonInfoTable)
            'Else
            '    KihonL.Clear()
            'End If
            'For i As Long = 0 To .KihonL.Count - 1
            '    KihonL.Add(.KihonL(i))
            'Next
        End With

        BackUp()

    End Sub

    Public Sub KihonInfoCopy(ByVal KIL As List(Of KihonInfoTable))

        If KihonL Is Nothing Then
            KihonL = New List(Of KihonInfoTable)
        Else
            KihonL.Clear()
        End If

        With KIL
            For i As Long = 0 To .Count - 1
                Dim KID As New KihonInfoTable
                KID.copy(KIL(i))
                KihonL.Add(KID)
            Next
        End With

    End Sub

    'Public Sub SunpoCalcHouhouCopy(ByVal SCH As List(Of SunpoCalcHouhouTable))

    '    If SunpoCalcHouhouL Is Nothing Then
    '        SunpoCalcHouhouL = New List(Of SunpoCalcHouhouTable)
    '    Else
    '        SunpoCalcHouhouL.Clear()
    '    End If

    '    With SCH
    '        For i As Long = 0 To .Count - 1
    '            Dim SID As New SunpoCalcHouhouTable
    '            SID.copy(SCH(i))
    '            SunpoCalcHouhouL.Add(SID)
    '        Next
    '    End With

    'End Sub

    Public Sub KihonInfoItemInit(ByVal KIL As List(Of KihonInfoTable))

        If KihonL Is Nothing Then
            KihonL = New List(Of KihonInfoTable)
        Else
            KihonL.Clear()
        End If

        With KIL
            For i As Long = 0 To .Count - 1
                Dim KID As New KihonInfoTable
                KID.copy(KIL(i))
                KID.item_value = ""
                KihonL.Add(KID)
            Next
        End With

    End Sub

    Public Sub SetFieldIndex(ByRef IDR As IDataReader)
        Dim iList As New List(Of Integer)
        '順番を間違いないように！！！！

        iList.Add(GetIndexOfDataReader(W_ID, IDR))                  '0  ID
        iList.Add(GetIndexOfDataReader(W_SavePath, IDR))            '1  SavePath
        iList.Add(GetIndexOfDataReader(W_CreateDate, IDR))          '2  CreateDate
        iList.Add(GetIndexOfDataReader(W_MeasureDate, IDR))         '3  MeasureDate
        iList.Add(GetIndexOfDataReader(W_TypeID, IDR))              '4  TypeID
        iList.Add(GetIndexOfDataReader(W_flg_Kihon, IDR))           '5  flg_Kihon 基本情報入力フラグ
        iList.Add(GetIndexOfDataReader(W_flg_Kiteiti, IDR))         '6  flg_Kiteiti 規定値入力フラグ
        iList.Add(GetIndexOfDataReader(W_flg_Gazou, IDR))           '7  flg_Gazou 画像入力フラグ
        iList.Add(GetIndexOfDataReader(W_flg_Kaiseki, IDR))         '8  flg_Kaiseki 解析入力フラグ
        iList.Add(GetIndexOfDataReader(W_flg_Kakunin, IDR))         '9  flg_Kakunin 寸法確認入力フラグ
        iList.Add(GetIndexOfDataReader(W_flg_Out, IDR))             '10 flg_Out 検査表出力フラグ

        FIELD_INDEX = iList

    End Sub

    Public Shared FIELD_INDEX As New List(Of Integer)

    Public Function GetDataByReader(ByRef iDataReader As IDataReader)
        GetDataByReader = False

        p_set_ID(iDataReader)                                   '0  ID
        p_set_SavePath(iDataReader)                             '1	SavePath
        p_set_CreateDate(iDataReader)                           '2	CreateDate
        p_set_MeasureDate(iDataReader)                          '3	MeasureDate
        p_set_TypeID(iDataReader)                               '4	TypeID
        p_set_flg_Kihon(iDataReader)                            '5  flg_Kihon 基本情報入力フラグ
        p_set_flg_Kiteiti(iDataReader)                          '6  flg_Kiteiti 規定値入力フラグ
        p_set_flg_Gazou(iDataReader)                            '7  flg_Gazou 画像入力フラグ
        p_set_flg_Kaiseki(iDataReader)                          '8  flg_Kaiseki 解析入力フラグ
        p_set_flg_Kakunin(iDataReader)                          '9  flg_Kakunin 寸法確認入力フラグ
        p_set_flg_Out(iDataReader)                              '10 flg_Out 検査表出力フラグ

        BackUp() 'バックアップ用

        GetDataByReader = True

    End Function

    Private Sub CreateFieldName()
        ReDim strFieldNames(MaxFieldCnt - 1)

        strFieldNames(0) = "SavePath"                           '1	SavePath
        strFieldNames(1) = "CreateDate"                         '2	CreateDate
        strFieldNames(2) = "MeasureDate"                        '3	MeasureDate
        strFieldNames(3) = "TypeID"                             '4	TypeID
        strFieldNames(4) = "flg_Kihon"                          '5  flg_Kihon 基本情報入力フラグ"
        strFieldNames(5) = "flg_Kiteiti"                        '6  flg_Kiteiti 規定値入力フラグ"
        strFieldNames(6) = "flg_Gazou"                          '7  flg_Gazou 画像入力フラグ"
        strFieldNames(7) = "flg_Kaiseki"                        '8  flg_Kaiseki 解析入力フラグ"
        strFieldNames(8) = "flg_Kakunin"                        '9  flg_Kakunin 寸法確認入力フラグ"
        strFieldNames(9) = "flg_Out"                            '10 flg_Out 検査表出力フラグ"

    End Sub

    Public Sub CreateFieldText()
        ReDim strFieldTexts(MaxFieldCnt - 1)

        CreateFieldName()

        strFieldTexts(0) = "'" & _SavePath & "'"                '1	SavePath
        strFieldTexts(1) = "'" & _CreateDate & "'"              '2	CreateDate
        strFieldTexts(2) = "'" & _MeasureDate & "'"             '3	MeasureDate
        strFieldTexts(3) = _TypeID                              '4	TypeID
        strFieldTexts(4) = _flg_Kihon                           '5  flg_Kihon 基本情報入力フラグ"
        strFieldTexts(5) = _flg_Kiteiti                         '6  flg_Kiteiti 規定値入力フラグ"
        strFieldTexts(6) = _flg_Gazou                           '7  flg_Gazou 画像入力フラグ"
        strFieldTexts(7) = _flg_Kaiseki                         '8  flg_Kaiseki 解析入力フラグ"
        strFieldTexts(8) = _flg_Kakunin                         '9  flg_Kakunin 寸法確認入力フラグ"
        strFieldTexts(9) = _flg_Out                             '10 flg_Out 検査表出力フラグ"

    End Sub

    Private Sub SetField()

        Dim IDX As Long = 0

        If _SavePath.Trim <> "" Then
            ReDim strFieldNames(IDX)
            ReDim strFieldTexts(IDX)
            strFieldNames(IDX) = "SavePath"
            strFieldTexts(IDX) = "'" & _SavePath & "'"
            IDX += 1
        End If
        If _CreateDate.Trim <> "" Then
            If IDX = 0 Then
                ReDim strFieldNames(IDX)
                ReDim strFieldTexts(IDX)
            Else
                ReDim Preserve strFieldNames(IDX)
                ReDim Preserve strFieldTexts(IDX)
            End If
            strFieldNames(IDX) = "CreateDate"
            strFieldTexts(IDX) = "'" & _CreateDate & "'"
            IDX += 1
        End If
        If _MeasureDate.Trim <> "" Then
            If IDX = 0 Then
                ReDim strFieldNames(IDX)
                ReDim strFieldTexts(IDX)
            Else
                ReDim Preserve strFieldNames(IDX)
                ReDim Preserve strFieldTexts(IDX)
            End If
            strFieldNames(IDX) = "MeasureDate"
            strFieldTexts(IDX) = "'" & _MeasureDate & "'"
            IDX += 1
        End If
        If IDX = 0 Then
            ReDim strFieldNames(IDX)
            ReDim strFieldTexts(IDX)
        Else
            ReDim Preserve strFieldNames(IDX)
            ReDim Preserve strFieldTexts(IDX)
        End If
        strFieldNames(IDX) = "TypeID"
        strFieldTexts(IDX) = _TypeID
        IDX += 1

        ReDim Preserve strFieldNames(IDX)
        ReDim Preserve strFieldTexts(IDX)
        strFieldNames(IDX) = "flg_Kihon"
        strFieldTexts(IDX) = _flg_Kihon                            '5  flg_Kihon 基本情報入力フラグ"
        IDX += 1

        ReDim Preserve strFieldNames(IDX)
        ReDim Preserve strFieldTexts(IDX)
        strFieldNames(IDX) = "flg_Kiteiti"
        strFieldTexts(IDX) = _flg_Kiteiti                          '6  flg_Kiteiti 規定値入力フラグ"
        IDX += 1

        ReDim Preserve strFieldNames(IDX)
        ReDim Preserve strFieldTexts(IDX)
        strFieldNames(IDX) = "flg_Gazou"
        strFieldTexts(IDX) = _flg_Gazou                            '7  flg_Gazou 画像入力フラグ"
        IDX += 1

        ReDim Preserve strFieldNames(IDX)
        ReDim Preserve strFieldTexts(IDX)
        strFieldNames(IDX) = "flg_Kaiseki"
        strFieldTexts(IDX) = _flg_Kaiseki                          '8  flg_Kaiseki 解析入力フラグ"
        IDX += 1

        ReDim Preserve strFieldNames(IDX)
        ReDim Preserve strFieldTexts(IDX)
        strFieldNames(IDX) = "flg_Kakunin"
        strFieldTexts(IDX) = _flg_Kakunin                          '9  flg_Kakunin 寸法確認入力フラグ"
        IDX += 1

        ReDim Preserve strFieldNames(IDX)
        ReDim Preserve strFieldTexts(IDX)
        strFieldNames(IDX) = "flg_Out"
        strFieldTexts(IDX) = _flg_Out                              '10 flg_Out 検査表出力フラグ"
        IDX += 1

    End Sub

    'SUURI ADD START 20140914
    'DXF出力ファイル名の初期値を決定
    Public Function GetDefaultDXGFileName() As String
        GetDefaultDXGFileName = ""
        Dim strTmp As String = ""
        Dim flgNoSettei As Boolean = True
        Dim strarrNames(KihonL.Count) As String
        Try
            For Each KID As KihonInfoTable In KihonL
                If KID.defaultDXFnameflg <> "" Then
                    flgNoSettei = False
                    If IsNumeric(KID.defaultDXFnameflg) Then
                        Dim intIndex As Integer
                        intIndex = CInt(KID.defaultDXFnameflg)
                        strarrNames(intIndex) = KID.item_value
                    End If
                End If
            Next
            Dim i As Integer
            For i = 1 To KihonL.Count
                If strarrNames(i) <> "" Then
                    If strTmp = "" Then
                        strTmp = strTmp & strarrNames(i)
                    Else
                        strTmp = strTmp & "_" & strarrNames(i)
                    End If
                End If
            Next
            GetDefaultDXGFileName = strTmp & ".dxf"

        Catch ex As Exception
            flgNoSettei = True
        End Try
      
        If flgNoSettei = True Then
            GetDefaultDXGFileName = "Drawing1.dxf"
        End If

        'Add By Yamada 20140922 Sta -------------日本車輌
        '基本情報で未入力の場合
        If GetDefaultDXGFileName = ".dxf" Then
            GetDefaultDXGFileName = "Drawing1.dxf"
        End If
        'Add By Yamada 20140922 Sta -------------日本車輌

    End Function
    'SUURI ADD END 20140914

#Region "データ取得"

    ''' <summary>
    ''' KihonInfoテーブルリスト

    ''' SunpoSetテーブルリスト

    ''' CameraInfoテーブルリストを取得

    ''' 2013/05/23 作成
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetDataToList() As Boolean

        GetDataToList = True
        Dim sw As New System.Diagnostics.Stopwatch
        sw.Start()
        'WorksKihonテーブルリストを取得

        If GetWorksKihonDataToList() = False Then
            GetDataToList = False
            Exit Function
        End If
        sw.Stop()
        Trace.WriteLine("GetWorksKihonDataToList" & sw.Elapsed.TotalSeconds)
        sw.Reset()
        sw.Start()
        'KihonInfoテーブルリストを取得

        If GetKihonInfoDataToList() = False Then
            GetDataToList = False
            Exit Function
        End If
        sw.Stop()
        Trace.WriteLine("GetKihonInfoDataToList" & sw.Elapsed.TotalSeconds)
        sw.Reset()
        sw.Start()
        'SunpoSetテーブルリストを取得

        If GetSunpoSetDataToList() = False Then
            GetDataToList = False
            Exit Function
        End If
        sw.Stop()
        Trace.WriteLine("GetSunpoSetDataToList" & sw.Elapsed.TotalSeconds)
        sw.Reset()
        sw.Start()
        'SunpoSetCellテーブルリストを取得

        If GetSunpoSetCellDataToList() = False Then
            GetDataToList = False
            Exit Function
        End If
        sw.Stop()
        Trace.WriteLine("GetSunpoSetCellDataToList" & sw.Elapsed.TotalSeconds)
        sw.Reset()
        sw.Start()
        'CameraInfoテーブルリストを取得

        If GetCameraInfoDataToList() = False Then
            GetDataToList = False
            Exit Function
        End If
        sw.Stop()
        Trace.WriteLine("GetCameraInfoDataToList" & sw.Elapsed.TotalSeconds)
        sw.Reset()
        sw.Start()
        'ExcelTemplateテーブルリストを取得

        If GetExcelTemplateDataToList() = False Then
            GetDataToList = False
            Exit Function
        End If
        sw.Stop()
        Trace.WriteLine("GetExcelTemplateDataToList" & sw.Elapsed.TotalSeconds)
        sw.Reset()
        sw.Start()
        'SunpoCalcHouhouテーブルリストを取得

        If GetSunpoCalcHouhouDataToList() = False Then
            GetDataToList = False
            Exit Function
        End If
        sw.Stop()
        Trace.WriteLine("GetSunpoCalcHpuhouDataToList" & sw.Elapsed.TotalSeconds)

    End Function

    ''' <summary>
    ''' KihonInfoテーブルリストを取得

    ''' 2013/05/28 作成
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetWorksDataToList() As List(Of WorksTable)

        Dim strSql As String = "SELECT * FROM " & K_works & " WHERE TypeID = " & CommonTypeID & " ORDER BY ID"
        Dim IDR As IDataReader
        Dim bRet As Boolean = False
        Dim WorksL As New List(Of WorksTable)

        IDR = m_dbClass.DoSelect(strSql)
        If Not IDR Is Nothing Then
            Dim IDX As New WorksTable
            IDX.SetFieldIndex(IDR)
            If WorksL Is Nothing Then
                WorksL = New List(Of WorksTable)
            Else
                WorksL.Clear()
            End If
            Do While IDR.Read
                Dim WKD As New WorksTable
                WKD.GetDataByReader(IDR)
                WKD.GetKihonInfoData()
                WorksL.Add(WKD)
                bRet = True
            Loop
            IDR.Close()
        End If

        GetWorksDataToList = WorksL

    End Function

    ''' <summary>
    ''' WorksKihonテーブルリストを取得

    ''' 2013/05/29 作成
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetWorksKihonDataToList() As Boolean

        Dim WKD As New WorksKihonTable
        WKD.m_dbClass = m_dbClass
        WKD.Work_ID = _ID
        WorksKihonL = WKD.GetDataToList()

        GetWorksKihonDataToList = True

    End Function

    ''' <summary>
    ''' KihonInfoテーブルリストを取得

    ''' 2013/05/23 作成
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetKihonInfoDataToList() As Boolean

        Dim KID As New KihonInfoTable
        KID.m_dbClass = m_dbClass
        KihonL = KID.GetDataToList()

        GetKihonInfoDataToList = True

        'Dim strSql As String = "SELECT * FROM " & K_kihoninfo & " WHERE TypeID = " & CommonTypeID & " ORDER BY DispSortID"
        'Dim IDR As IDataReader
        'Dim bRet As Boolean = False

        'IDR = m_dbClass.DoSelect(strSql)
        'If Not IDR Is Nothing Then
        '    Dim IDX As New KihonInfoTable
        '    IDX.SetFieldIndex(IDR)
        '    If KihonL Is Nothing Then
        '        KihonL = New List(Of KihonInfoTable)
        '    Else
        '        KihonL.Clear()
        '    End If
        '    Do While IDR.Read
        '        Dim KID As New KihonInfoTable
        '        KID.GetDataByReader(IDR)
        '        KihonL.Add(KID)
        '        bRet = True
        '    Loop
        '    IDR.Close()
        'End If

        'GetKihonInfoDataToList = bRet

    End Function

    ''' <summary>
    ''' SunpoSetテーブルリストを取得

    ''' 2013/05/23 作成
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetSunpoSetDataToList() As Boolean

        Dim SPD As New SunpoSetTable
        SPD.m_dbClass = m_dbClass
        SunpoSetL = SPD.GetDataToList()

        GetSunpoSetDataToList = True

        'Dim strSql As String = "SELECT * FROM " & K_SunpoSet & " WHERE TypeID = " & CommonTypeID & " ORDER BY ID"
        'Dim IDR As IDataReader
        'Dim bRet As Boolean = False

        'IDR = m_dbClass.DoSelect(strSql)
        'If Not IDR Is Nothing Then
        '    Dim IDX As New SunpoSetTable
        '    IDX.SetFieldIndex(IDR)
        '    If SunpoSetL Is Nothing Then
        '        SunpoSetL = New List(Of SunpoSetTable)
        '    Else
        '        SunpoSetL.Clear()
        '    End If
        '    Do While IDR.Read
        '        Dim SPD As New SunpoSetTable
        '        SPD.GetDataByReader(IDR)
        '        SunpoSetL.Add(SPD)
        '        bRet = True
        '    Loop
        '    IDR.Close()
        'End If

        'GetSunpoSetDataToList = bRet

    End Function

    ''' <summary>
    ''' SunpoSetCellテーブルリストを取得

    ''' 2013/05/27 作成
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetExcelTemplateDataToList() As Boolean

        Dim ETD As New ExcelTemplateTable
        ETD.m_dbClass = m_dbClass
        ExcelTemplateL = ETD.GetDataToList()

        GetExcelTemplateDataToList = True

    End Function

    ''' <summary>
    ''' SunpoSetCellテーブルリストを取得

    ''' 2013/05/27 作成
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetSunpoCalcHouhouDataToList() As Boolean

        Dim SCT As New SunpoCalcHouhouTable
        SCT.m_dbClass = m_dbClass
        SunpoCalcHouhouL = SCT.GetDataToList()

        GetSunpoCalcHouhouDataToList = True

    End Function

    ''' <summary>
    ''' CameraInfoテーブルリストを取得

    ''' 2013/05/23 作成
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetCameraInfoDataToList() As Boolean

        Dim CMD As New CameraInfoTable
        CMD.m_dbClass = m_dbClass
        CameraL = CMD.GetDataToList()

        '(20140528 Tezuka ADD) 前選択項目の取り出し
        CameraD = CMD.GetDataToSelect()

        GetCameraInfoDataToList = True

        'Dim strSql As String = "SELECT * FROM " & K_CameraInfo & " ORDER BY ID"
        'Dim IDR As IDataReader
        'Dim bRet As Boolean = False

        'IDR = m_dbClass.DoSelect(strSql)
        'If Not IDR Is Nothing Then
        '    Dim IDX As New CameraInfoTable
        '    IDX.SetFieldIndex(IDR)
        '    If CameraL Is Nothing Then
        '        CameraL = New List(Of CameraInfoTable)
        '    Else
        '        CameraL.Clear()
        '    End If
        '    Do While IDR.Read
        '        Dim CAD As New CameraInfoTable
        '        CAD.GetDataByReader(IDR)
        '        CameraL.Add(CAD)
        '        bRet = True
        '    Loop
        '    IDR.Close()
        'End If

        'GetCameraInfoDataToList = bRet

    End Function

    ''' <summary>
    ''' SunpoSetCellテーブルリストを取得

    ''' 2013/05/27 作成
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetSunpoSetCellDataToList() As Boolean

        Dim SSD As New SunpoSetTable
        SSD.m_dbClass = m_dbClass
        SunpoSetCellL = SSD.GetCellDataToList()

        GetSunpoSetCellDataToList = True

    End Function

    ''' <summary>
    ''' SavePathに保存された計測データmdbからKihonInfoテーブルリストを取得

    ''' 2013/05/28 作成
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetKihonInfoData() As Boolean

        GetKihonInfoData = True

        Dim m_dbclass1 As New FBMlib.CDBOperateOLE
        '計測データmdbに接続

        If ConnectDB(_SavePath & "\" & m_Keisoku_mdb_name, m_dbclass1) = False Then
            GetKihonInfoData = False
            Exit Function
        End If

        Dim KID As New KihonInfoTable
        KID.m_dbClass = m_dbclass1
        KihonL = KID.GetDataToList()

        '計測データmdbを切断
        DisConnectDB(m_dbclass1)

    End Function

    Public Function GetDataToList1() As Boolean

        Dim strSql As String = "SELECT * FROM " & K_kihoninfo & " WHERE TypeID = " & CommonTypeID & " ORDER BY DispSortID"
        Dim IDR As IDataReader
        Dim bRet As Boolean = False

        IDR = m_dbClass.DoSelect(strSql)
        If Not IDR Is Nothing Then
            Dim IDX As New KihonInfoTable
            IDX.SetFieldIndex(IDR)
            If KihonL Is Nothing Then
                KihonL = New List(Of KihonInfoTable)
            Else
                KihonL.Clear()
            End If
            Do While IDR.Read
                Dim KID As New KihonInfoTable
                KID.GetDataByReader(IDR)
                KihonL.Add(KID)
                bRet = True
            Loop
            IDR.Close()
        End If

        GetDataToList1 = bRet

    End Function

    'Public Function GetDataToList() As Boolean

    '    Dim strSql As String = "SELECT * FROM " & K_works & " ORDER BY ID"
    '    Dim IDR As IDataReader
    '    Dim bRet As Boolean = False

    '    IDR = m_dbClass.DoSelect(strSql)
    '    If Not IDR Is Nothing Then
    '        Dim IDX As New WorksTable
    '        IDX.SetFieldIndex(IDR)
    '        If WorksL Is Nothing Then
    '            WorksL = New List(Of WorksTable)
    '        Else
    '            WorksL.Clear()
    '        End If
    '        Do While IDR.Read
    '            Dim WKD As New WorksTable
    '            WKD.GetDataByReader(IDR)
    '            WorksL.Add(WKD)
    '            bRet = True
    '        Loop
    '        IDR.Close()
    '    End If

    '    GetDataToList = bRet

    'End Function

    ''' <summary>
    ''' SunpoSetテーブルリストのみ取得
    ''' 2015/03/13 作成
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetSunpoDataToList() As Boolean

        GetSunpoDataToList = True

        'SunpoSetテーブルリストを取得
        If GetSunpoSetDataToList() = False Then
            GetSunpoDataToList = False
            Exit Function
        End If

        'SunpoSetCellテーブルリストを取得
        If GetSunpoSetCellDataToList() = False Then
            GetSunpoDataToList = False
            Exit Function
        End If

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

        If _ID.Trim = "" Then
            ExistsData = False
            Exit Function
        End If

        strWhere = "ID = " & _ID.Trim

        lRet = m_dbClass.DoDCount("*", K_works, strWhere)
        If lRet > 0 Then
            bRet = True
        End If

        ExistsData = bRet

    End Function

#End Region

#Region "Insert"

    Public Function InsertData(Optional ByRef flg_trans As Boolean = True) As Boolean

        InsertData = True

        SetField()

        Dim lRet As Long = 0

        If flg_trans = True Then
            m_dbClass.BeginTrans()
            lRet = m_dbClass.DoInsert(strFieldNames, K_works, strFieldTexts)
            If lRet = 1 Then
                _ID = GetSQLIdentityID(K_works, m_dbClass)
                m_dbClass.CommitTrans()
            Else
                m_dbClass.RollbackTrans()
                InsertData = False
                Exit Function
            End If
        Else
            lRet = m_dbClass.DoInsert(strFieldNames, K_works, strFieldTexts)
            If lRet = 1 Then
                _ID = GetSQLIdentityID(K_works, m_dbClass)
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

        Dim strWhere As String = "ID = " & _ID

        ReDim strFieldNames(1)
        ReDim strFieldTexts(1)

        strFieldNames(0) = "SavePath"
        strFieldNames(1) = "CreateDate"

        strFieldTexts(0) = "'" & _SavePath & "'"
        strFieldTexts(1) = "'" & _CreateDate & "'"

        With m_dbClass
            If flg_trans = True Then
                .BeginTrans()
                lRet = .DoUpdate(strFieldNames, K_works, strFieldTexts, strWhere)
                If lRet = 1 Then
                    .CommitTrans()
                Else
                    .RollbackTrans()
                    bRet = False
                End If
            Else
                lRet = .DoUpdate(strFieldNames, K_works, strFieldTexts, strWhere)
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

    '        SetField()

    '        Dim lRet As Long = 0
    '        lRet = m_dbClass.DoInsert(strFieldNames, K_works, strFieldTexts)
    '        If lRet = 1 Then
    '            _ID = GetSQLIdentityID(K_works, m_dbClass)
    '        Else
    '            InsertData = False
    '        End If

    '    End Function

    '#End Region

#Region "基本情報"

    ''' <summary>
    ''' 基本情報処理フラグ設定

    ''' 2013/05/24 作成
    ''' </summary>
    ''' <param name="flg_connect">
    ''' システム設定mdb接続状態

    ''' True:接続済み
    ''' False:未接続

    ''' </param>
    ''' <remarks></remarks>
    Public Function SetKihonFlg(Optional ByRef flg_connect As Boolean = True) As Boolean

        SetKihonFlg = True

        ReDim strFieldNames(0)
        ReDim strFieldTexts(0)

        strFieldNames(0) = "flg_Kihon"
        strFieldTexts(0) = "1"
        _flg_Kihon = "1"

        If flg_connect = False Then
            'システムDBに接続されていない場合

            If ConnectSystemDB(m_dbClass) = False Then
                SetKihonFlg = False
                Exit Function
            End If
        End If

        Dim strWhere As String = "ID = " & _ID

        Dim lRet As Long = 0

        With m_dbClass
            .BeginTrans()
            lRet = .DoUpdate(strFieldNames, K_works, strFieldTexts, strWhere)
            If lRet = 1 Then
                .CommitTrans()
            Else
                .RollbackTrans()
                SetKihonFlg = False
            End If
        End With

    End Function

    ''' <summary>
    ''' 規定値処理フラグ設定

    ''' 2013/05/24 作成
    ''' </summary>
    ''' <param name="flg_connect">
    ''' システム設定mdb接続状態

    ''' True:接続済み
    ''' False:未接続

    ''' </param>
    ''' <remarks></remarks>
    Public Function SetKiteitiFlg(Optional ByRef flg_connect As Boolean = True) As Boolean

        SetKiteitiFlg = True

        ReDim strFieldNames(0)
        ReDim strFieldTexts(0)

        strFieldNames(0) = "flg_Kiteiti"
        strFieldTexts(0) = "1"

        _flg_Kiteiti = "1"

        If flg_connect = False Then
            'システムDBに接続されていない場合

            If ConnectSystemDB(m_dbClass) = False Then
                SetKiteitiFlg = False
                Exit Function
            End If
        End If

        Dim strWhere As String = "ID = " & _ID

        Dim lRet As Long = 0

        With m_dbClass
            .BeginTrans()
            lRet = .DoUpdate(strFieldNames, K_works, strFieldTexts, strWhere)
            If lRet = 1 Then
                .CommitTrans()
            Else
                .RollbackTrans()
                SetKiteitiFlg = False
            End If
        End With

    End Function

    ''' <summary>
    ''' 画像処理フラグ設定

    ''' 2013/05/24 作成
    ''' </summary>
    ''' <param name="flg_connect">
    ''' システム設定mdb接続状態

    ''' True:接続済み
    ''' False:未接続

    ''' </param>
    ''' <remarks></remarks>
    Public Function SetGazouFlg(Optional ByRef flg_connect As Boolean = True) As Boolean

        SetGazouFlg = True

        ReDim strFieldNames(0)
        ReDim strFieldTexts(0)

        strFieldNames(0) = "flg_Gazou"
        strFieldTexts(0) = "1"

        _flg_Gazou = "1"

        If flg_connect = False Then
            'システムDBに接続されていない場合

            If ConnectSystemDB(m_dbClass) = False Then
                SetGazouFlg = False
                Exit Function
            End If
        End If

        Dim strWhere As String = "ID = " & _ID

        Dim lRet As Long = 0

        With m_dbClass
            .BeginTrans()
            lRet = .DoUpdate(strFieldNames, K_works, strFieldTexts, strWhere)
            If lRet = 1 Then
                .CommitTrans()
            Else
                .RollbackTrans()
                SetGazouFlg = False
            End If
        End With

    End Function

    ''' <summary>
    ''' 確認処理フラグ設定

    ''' 2013/05/24 作成
    ''' </summary>
    ''' <param name="flg_connect">
    ''' システム設定mdb接続状態

    ''' True:接続済み
    ''' False:未接続

    ''' </param>
    ''' <remarks></remarks>
    Public Function SetKakuninFlg(Optional ByRef flg_connect As Boolean = True) As Boolean
        TimeMonStart()

        SetKakuninFlg = True

        ReDim strFieldNames(0)
        ReDim strFieldTexts(0)

        strFieldNames(0) = "flg_Kakunin"
        strFieldTexts(0) = "1"
        _flg_Kakunin = "1"

        If flg_connect = False Then
            'システムDBに接続されていない場合

            If ConnectSystemDB(m_dbClass) = False Then
                SetKakuninFlg = False
                Exit Function
            End If
        End If

        Dim strWhere As String = "ID = " & _ID

        Dim lRet As Long = 0

        With m_dbClass
            .BeginTrans()
            lRet = .DoUpdate(strFieldNames, K_works, strFieldTexts, strWhere)
            If lRet = 1 Then
                .CommitTrans()
            Else
                .RollbackTrans()
                SetKakuninFlg = False
            End If
        End With

        TimeMonEnd()

    End Function

    ''' <summary>
    ''' 寸法解析処理フラグ設定

    ''' 2013/05/24 作成
    ''' </summary>
    ''' <param name="flg_connect">
    ''' システム設定mdb接続状態

    ''' True:接続済み
    ''' False:未接続

    ''' </param>
    ''' <remarks></remarks>
    Public Function SetKaisekiFlg(Optional ByRef flg_connect As Boolean = True) As Boolean
        TimeMonStart()

        SetKaisekiFlg = True

        ReDim strFieldNames(0)
        ReDim strFieldTexts(0)

        strFieldNames(0) = "flg_Kaiseki"
        strFieldTexts(0) = "1"
        _flg_Kaiseki = "1"

        If flg_connect = False Then
            'システムDBに接続されていない場合

            If ConnectSystemDB(m_dbClass) = False Then
                SetKaisekiFlg = False
                Exit Function
            End If
        End If

        Dim strWhere As String = "ID = " & _ID

        Dim lRet As Long = 0

        With m_dbClass
            .BeginTrans()
            lRet = .DoUpdate(strFieldNames, K_works, strFieldTexts, strWhere)
            If lRet = 1 Then
                .CommitTrans()
            Else
                .RollbackTrans()
                SetKaisekiFlg = False
            End If
        End With

        TimeMonEnd()
    End Function

    ''' <summary>
    ''' 検査表出力処理フラグ設定

    ''' 2013/05/24 作成
    ''' </summary>
    ''' <param name="flg_connect">
    ''' システム設定mdb接続状態

    ''' True:接続済み
    ''' False:未接続

    ''' </param>
    ''' <remarks></remarks>
    Public Function SetOutFlg(Optional ByRef flg_connect As Boolean = True) As Boolean

        SetOutFlg = True

        ReDim strFieldNames(0)
        ReDim strFieldTexts(0)

        strFieldNames(0) = "flg_Out"
        strFieldTexts(0) = "1"
        _flg_Out = "1"

        If flg_connect = False Then
            'システムDBに接続されていない場合

            If ConnectSystemDB(m_dbClass) = False Then
                SetOutFlg = False
                Exit Function
            End If
        End If

        Dim strWhere As String = "ID = " & _ID

        Dim lRet As Long = 0

        With m_dbClass
            .BeginTrans()
            lRet = .DoUpdate(strFieldNames, K_works, strFieldTexts, strWhere)
            If lRet = 1 Then
                .CommitTrans()
            Else
                .RollbackTrans()
                SetOutFlg = False
            End If
        End With

    End Function

#End Region


End Class
