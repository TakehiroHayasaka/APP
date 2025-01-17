﻿Public Class KojiDataConfUserControl

    Public WorksT As Works
    Public WorksD As WorksTable
    Public WorksL As List(Of WorksTable)

    Public SunpoSetL As List(Of SunpoSetTable)                  'SunpoSetテーブルリスト


    Public m_form As String                                     '画面名

    Dim m_Keisoku_mdb_name As String = "計測データ.mdb"

    Dim m_TemplatePath As String = System.Environment.CurrentDirectory & "\計測システムフォルダ\Template"

    Public m_SystemMdbPath As String
    Public m_SystemMdbFullPath As String

    Public m_keisoku_dbclass As FBMlib.CDBOperateOLE            '計測データ.mdb
    Public m_system_dbclass As FBMlib.CDBOperateOLE             'システム設定.mdb
    Public KihonInfoSeach As WorksTable

    'Public CHani As Integer = 12
    Public CHani As Integer = 10

    Public m_histgram_temlate As String = "グラフテンプレート.xls"

    Public m_Tanpin_Template As String = "単品分析用.xls"
    Public m_Zentai_Template As String = "全体傾向分析用.xls"

    Public err_msg_excelout_err As String = "Excel出力でエラー発生"
    Public err_msg_koutei_nothing As String = "工程が全種類存在していません"

    Private m_dataTable2 As System.Data.DataTable   ' DataGridView2
    Private m_dataTable1 As System.Data.DataTable   ' DataGridView1

    Private m_columnName2Header1 As Hashtable
    Private m_columnName2Header2 As Hashtable

    Public Property parentWindow As System.Windows.Window

    'Private Sub KojiDataConf_FormClosing(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'DisConnectDB(m_system_dbclass)
    'End Sub

    Public ReadOnly Property DataTable1 As System.Data.DataTable
        Get
            Return m_dataTable1
        End Get
    End Property


    Private Sub KojiDataConf_Load(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

    End Sub
    Public Sub Load()

        '初期設定

        If SetInitForm() = False Then
            'Me.Close()
            Exit Sub
        End If

        'データ取得

        GetData()

        'システム設定mdbを切断
        DisConnectDB(m_system_dbclass)

        ''計測データmdbを切断
        'DisConnectDB(m_keisoku_dbclass)

        'ヘッダ設定

        SetGridHeadder()

        '一覧表示
        SetDataToGrid()


        'Add By Yamada Sta 20140620-------------------------------------??コネクタ計測時のみ隠したい
#If True Then
        Me.BtnKojiExcelOut.Visibility = Windows.Visibility.Hidden
        Me.ButtonTanpinKalc.Visibility = Windows.Visibility.Hidden
        Me.ZentaiBunsekiBtn.Visibility = Windows.Visibility.Hidden
#End If
        'Add By Yamada End 20140620-------------------------------------


        tmpWorksT = New Works
        If tmpWorksT.WorksL Is Nothing Then
            tmpWorksT.WorksL = New List(Of WorksTable)
        Else
            tmpWorksT.WorksL.Clear()
        End If
        For Each WK As WorksTable In WorksT.WorksL
            tmpWorksT.WorksL.Add(WK)
        Next

        If Me.DataGridView1.Columns.Count > 0 Then
            Me.DataGridView1.Columns(Me.DataGridView1.Columns.Count - 1).Visibility = System.Windows.Visibility.Hidden
        End If


    End Sub

    ''' <summary>
    ''' 初期設定

    ''' 2013/05/13 作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Function SetInitForm() As Boolean

        'Temolateフォルダのシステム設定.mdbを開く


        SetInitForm = True

        'システム設定.mdbに接続する

        If ConnectSystemDB(m_system_dbclass) = False Then
            MsgBox(err_msg_systemdb_connect, MsgBoxStyle.OkOnly)
            SetInitForm = False
            Exit Function
        End If

    End Function

    ''' <summary>
    ''' データ取得

    ''' 2013/05/15 作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetData()

        WorksT = New Works(m_system_dbclass)
        WorksT.KihonInfoCopy(WorksD.KihonL)
        WorksT.GetDataToList()
        '  WorksT.GetDataToList1()


    End Sub

    ''' <summary>
    ''' ヘッダ設定

    ''' 2013/05/15 作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetGridHeadder()

        'ヘッダ設定

        m_dataTable1 = New System.Data.DataTable
        With m_dataTable1

            '.ColumnCount = WorksD.KihonL.Count + 2
            For i As Integer = 1 To WorksD.KihonL.Count + 2
                .Columns.Add(New System.Data.DataColumn)
            Next

            m_columnName2Header1 = New Hashtable

            .Columns(0).ColumnName = "No"
            '.Columns(0).Name = "ID"
            m_columnName2Header1.Add("ID", "No")
            For i As Integer = 0 To WorksD.KihonL.Count - 1
                .Columns(i + 1).ColumnName = WorksD.KihonL(i).item_name
                '.Columns(i + 1).Name = WorksD.KihonL(i).item_cell_name
                m_columnName2Header1.Add(WorksD.KihonL(i).item_cell_name, WorksD.KihonL(i).item_name)
            Next
            'WorksListのインデックス
            .Columns(.Columns.Count - 1).ColumnName = "LIDX"
            '.Columns(.Columns.Count - 1).Name = "LIDX"
            m_columnName2Header1.Add("LIDX", "LIDX")

        End With
        'With m_dataTable1
        '    For i As Integer = 0 To DataGridView1.Columns.Count - 1
        '        Dim column As System.Data.DataColumn

        '        column = New System.Data.DataColumn
        '        column.ColumnName = DataGridView1.Columns(i).Header
        '        .Columns.Add(column)
        '    Next
        'End With
        DataGridView1.ItemsSource = m_dataTable1.DefaultView
        With DataGridView1

            .CanUserDeleteRows = False
            .CanUserAddRows = False
            .CanUserSortColumns = False

            .IsReadOnly = True

            ''.ColumnCount = WorksD.KihonL.Count + 2
            'For i As Integer = 1 To WorksD.KihonL.Count + 2
            '    .Columns.Add(New System.Windows.Controls.DataGridTextColumn)
            'Next

            'm_columnName2Header1 = New Hashtable

            '.Columns(0).Header = "No"
            ''.Columns(0).Name = "ID"
            'm_columnName2Header1.Add("ID", "No")
            'For i As Integer = 0 To WorksD.KihonL.Count - 1
            '    .Columns(i + 1).Header = WorksD.KihonL(i).item_name
            '    '.Columns(i + 1).Name = WorksD.KihonL(i).item_cell_name
            '    m_columnName2Header1.Add(WorksD.KihonL(i).item_cell_name, WorksD.KihonL(i).item_name)
            '    '書き込み禁止
            '    .Columns(i + 1).IsReadOnly = True
            '    '並び替え禁止
            '    .Columns(i + 1).CanUserSort = False
            'Next
            ''WorksListのインデックス
            '.Columns(.Columns.Count - 1).Header = "LIDX"
            ''.Columns(.Columns.Count - 1).Name = "LIDX"
            'm_columnName2Header1.Add("LIDX", "LIDX")
            ''書き込み禁止
            '.Columns(.Columns.Count - 1).IsReadOnly = True
            ''並び替え禁止
            '.Columns(.Columns.Count - 1).CanUserSort = False
            '非表示
            '.Columns(.Columns.Count - 1).Visibility = System.Windows.Visibility.Hidden

            'ヘッダを中央
            '.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        End With

        'ヘッダ設定

        m_dataTable2 = New System.Data.DataTable
        With m_dataTable2

            '.ColumnCount = WorksD.KihonL.Count + 1
            For i As Integer = 1 To WorksD.KihonL.Count + 1
                .Columns.Add(New System.Data.DataColumn)
            Next

            m_columnName2Header2 = New Hashtable

            .Columns(0).ColumnName = "No"
            '.Columns(0).Name = "ID"
            m_columnName2Header2.Add("ID", "No")
            For i As Integer = 0 To WorksD.KihonL.Count - 1
                .Columns(i + 1).ColumnName = WorksD.KihonL(i).item_name
                '.Columns(i + 1).Name = WorksD.KihonL(i).item_cell_name
                m_columnName2Header2.Add(WorksD.KihonL(i).item_cell_name, WorksD.KihonL(i).item_name)
            Next
            '.Rows.Add()

            'ヘッダを中央
            '.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        End With
        'With m_dataTable2
        '    For i As Integer = 0 To DataGridView2.Columns.Count - 1
        '        Dim column As System.Data.DataColumn

        '        column = New System.Data.DataColumn
        '        column.ColumnName = DataGridView2.Columns(i).Header
        '        .Columns.Add(column)
        '    Next
        'End With
        m_dataTable2.Rows.Add(m_dataTable2.NewRow())
        DataGridView2.ItemsSource = m_dataTable2.DefaultView
        With DataGridView2

            .CanUserDeleteRows = False
            .CanUserAddRows = False
            .CanUserSortColumns = False

            ''.ColumnCount = WorksD.KihonL.Count + 1
            'For i As Integer = 1 To WorksD.KihonL.Count + 1
            '    .Columns.Add(New System.Windows.Controls.DataGridTextColumn)
            'Next

            'm_columnName2Header2 = New Hashtable

            '.Columns(0).Header = "No"
            ''.Columns(0).Name = "ID"
            'm_columnName2Header2.Add("ID", "No")
            'For i As Integer = 0 To WorksD.KihonL.Count - 1
            '    .Columns(i + 1).Header = WorksD.KihonL(i).item_name
            '    '.Columns(i + 1).Name = WorksD.KihonL(i).item_cell_name
            '    m_columnName2Header2.Add(WorksD.KihonL(i).item_cell_name, WorksD.KihonL(i).item_name)
            '    '並び替え禁止
            '    .Columns(i + 1).CanUserSort = False
            'Next
            '.Rows.Add()

            'ヘッダを中央
            '.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        End With

    End Sub

    ''' <summary>
    ''' Grid表示
    ''' 2013/05/29 作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetDataToGrid(Optional ByRef flg_search As Boolean = False)

        Dim iRowNo As Integer = 0

        If WorksT.WorksL.Count <= 0 Then Exit Sub

        With DataGridView1

            '.Rows.Clear()
            m_dataTable1.Rows.Clear()

            '.Rows.Add(WorksT.WorksL.Count)

            For i As Integer = 0 To WorksT.WorksL.Count - 1

                '工事データ存在チェック
                If System.IO.File.Exists(WorksT.WorksL(i).SavePath & "\" & m_Keisoku_mdb_name) = False Then
                    '存在しない

                    WorksT.WorksL(i).m_folder_flg = False
                    WorksT.WorksL(i).m_search_flg = False
                Else
                    '存在する
                    If flg_search = False Then
                        '検索しない状態

                        '.Rows.Add()
                        'iRowNo = .Rows.Count - 1
                        iRowNo = m_dataTable1.Rows.Count
                        Dim row As System.Data.DataRow = m_dataTable1.NewRow()
                        For ii As Integer = 0 To WorksT.WorksL(i).KihonL.Count - 1
                            Try
                                '.Rows(iRowNo).Cells(0).Value = iRowNo + 1
                                row(0) = iRowNo + 1

                                '.Rows(iRowNo).Cells(WorksT.WorksL(i).KihonL(ii).item_cell_name).Value =
                                '    WorksT.WorksL(i).KihonL(ii).item_value
                                row(m_columnName2Header1(WorksT.WorksL(i).KihonL(ii).item_cell_name)) = _
                                    WorksT.WorksL(i).KihonL(ii).item_value
                            Catch ex As Exception
                                MsgBox("値設定エラー", MsgBoxStyle.OkOnly)
                                Exit Sub
                            End Try
                        Next
                        '.Rows(iRowNo).Cells("LIDX").Value = i
                        row(m_columnName2Header1("LIDX")) = i
                        WorksT.WorksL(i).m_folder_flg = True
                        WorksT.WorksL(i).m_search_flg = True

                        m_dataTable1.Rows.Add(row)
                    Else
                        '検索した状態

                        If WorksT.WorksL(i).m_search_flg = True Then
                            '.Rows.Add()
                            'iRowNo = .Rows.Count - 1
                            iRowNo = m_dataTable1.Rows.Count
                            Dim row As System.Data.DataRow = m_dataTable1.NewRow()
                            For ii As Integer = 0 To WorksT.WorksL(i).KihonL.Count - 1
                                Try
                                    '.Rows(iRowNo).Cells(0).Value = iRowNo + 1
                                    row(0) = iRowNo + 1

                                    '.Rows(iRowNo).Cells(WorksT.WorksL(i).KihonL(ii).item_cell_name).Value =
                                    '    WorksT.WorksL(i).KihonL(ii).item_value
                                    row(m_columnName2Header1(WorksT.WorksL(i).KihonL(ii).item_cell_name)) =
                                        WorksT.WorksL(i).KihonL(ii).item_value
                                Catch ex As Exception
                                    MsgBox("値設定エラー", MsgBoxStyle.OkOnly)
                                    Exit Sub
                                End Try
                            Next
                            '.Rows(iRowNo).Cells("LIDX").Value = i
                            row(m_columnName2Header1("LIDX")) = i
                            WorksT.WorksL(i).m_folder_flg = True
                            WorksT.WorksL(i).m_search_flg = True

                            m_dataTable1.Rows.Add(row)
                        End If
                    End If
                End If

                '.AutoSize = True

            Next

            '一覧に工事データが表示されていない場合は、出力ボタンを押せないようにする
            '(20131115 Y.Tezuka ADD) 単品分析、全体分析ボタン、OKボタンも押せないようにする
            If m_dataTable1.Rows.Count <= 0 Then
                BtnKojiExcelOut.IsEnabled = False
                ButtonTanpinKalc.IsEnabled = False
                ZentaiBunsekiBtn.IsEnabled = False
                Button2.IsEnabled = False
            Else
                BtnKojiExcelOut.IsEnabled = True
                ButtonTanpinKalc.IsEnabled = True
                ZentaiBunsekiBtn.IsEnabled = True
                Button2.IsEnabled = True
            End If

        End With

        If flg_search = False Then
            With DataGridView2

                m_dataTable2.Rows.Clear()

                m_dataTable2.Rows.Add(m_dataTable2.NewRow())

            End With
        End If

    End Sub

    Public tmpWorksT As Works

    'Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
    '    Dim i As Integer
    '    KihonInforSeach = New WorksTable
    '    KihonInforSeach.m_dbClass = m_system_dbclass
    '    KihonInforSeach.GetDataToList1()


    '    For i = 0 To WorksT.WorksL(0).KihonL.Count - 1
    '        KihonInforSeach.KihonL(i).item_value = DataGridView2.Rows(0).Cells(KihonInforSeach.KihonL(i).item_cell_name).Value
    '    Next
    '    If WorksT.WorksL Is Nothing Then
    '        WorksT.WorksL = New List(Of WorksTable)
    '    Else
    '        WorksT.WorksL.Clear()
    '    End If
    '    For Each WK As WorksTable In tmpWorksT.WorksL
    '        WorksT.WorksL.Add(WK)
    '    Next

    '    Dim n As Integer = WorksT.WorksL.Count
    '    Dim flg As Boolean = False
    '    Dim j As Integer = 0
    '    For i = n - 1 To 0 Step -1
    '        flg = False
    '        j = 0
    '        For Each KI As KihonInfoTable In WorksT.WorksL(i).KihonL
    '            If KI.item_value = KihonInforSeach.KihonL(j).item_value Then
    '                flg = True
    '                Exit For
    '            End If
    '            j += 1
    '        Next
    '        If flg = False Then
    '            WorksT.WorksL.RemoveAt(i)
    '        End If
    '    Next

    '    SetDataToGrid()


    'End Sub

#Region "絞り込み"

    '''' <summary>
    '''' 絞り込み
    '''' 2013/05/17 作成
    '''' 2013/05/29 削除
    '''' </summary>
    '''' <remarks></remarks>
    'Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

    '    With DataGridView1

    '        If .Rows.Count < 0 Then Exit Sub

    '    End With

    '    'システム設定mdbに接続する

    '    If ConnectSystemDB(m_system_dbclass) = False Then
    '        MsgBox(err_msg_systemdb_connect, MsgBoxStyle.OkOnly)
    '        Exit Sub
    '    End If

    '    Dim i As Integer
    '    KihonInforSeach = New WorksTable
    '    KihonInforSeach.m_dbClass = m_system_dbclass
    '    KihonInforSeach.GetDataToList1()

    '    'システム設定mdbに切断する
    '    DisConnectDB(m_system_dbclass)

    '    '検索値を取得する

    '    With DataGridView2
    '        For i = 0 To WorksT.WorksL(0).KihonL.Count - 1
    '            KihonInforSeach.KihonL(i).item_value = .Rows(0).Cells(KihonInforSeach.KihonL(i).item_cell_name).Value
    '        Next
    '    End With

    '    Dim iRowNo As Integer = 0

    '    With DataGridView1

    '        .Rows.Clear()

    '        For i = 0 To WorksT.WorksL.Count - 1

    '            Dim bRet As Boolean = False
    '            Dim sCount As Integer = 0
    '            For ii As Integer = 0 To WorksT.WorksL(i).KihonL.Count - 1
    '                Try
    '                    If Not KihonInforSeach.KihonL(ii).item_value Is Nothing Then
    '                        If KihonInforSeach.KihonL(ii).item_value.Trim <> "" Then
    '                            If WorksT.WorksL(i).KihonL(ii).item_value.IndexOf(KihonInforSeach.KihonL(ii).item_value) >= 0 Then
    '                                bRet = True
    '                                Exit For
    '                            End If
    '                        Else
    '                            sCount += 1
    '                        End If
    '                    Else
    '                        sCount += 1
    '                    End If
    '                Catch ex As Exception
    '                    MsgBox("値設定エラー", MsgBoxStyle.OkOnly)
    '                    Exit Sub
    '                End Try
    '            Next
    '            If bRet = True Or sCount = 12 Then
    '                .Rows.Add()
    '                iRowNo = .Rows.Count - 1
    '                .Rows(iRowNo).Cells(0).Value = iRowNo
    '                For ii As Integer = 0 To WorksT.WorksL(i).KihonL.Count - 1
    '                    .Rows(iRowNo).Cells(WorksT.WorksL(i).KihonL(ii).item_cell_name).Value =
    '                        WorksT.WorksL(i).KihonL(ii).item_value
    '                Next
    '            End If

    '        .AutoSize = True
    '        Next

    '    End With

    'End Sub

    ''' <summary>
    ''' 絞り込み
    ''' 2013/05/29 作成
    ''' 2013/05/30 削除
    ''' SQLエラー起きず、IDR.Readができないため、使用しない

    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        SearchSyori()

    End Sub

    ''' <summary>
    ''' 絞り込み処理

    ''' 2013/05/30 作成
    ''' SQLエラー起きず、IDR.Readができないため、使用しない

    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SearchSyori()

        With DataGridView1

            If m_dataTable1.Rows.Count < 0 Then Exit Sub

        End With

        Dim arySearch As New ArrayList

        '検索項目設定リスト

        KihonInfoSeach = New WorksTable
        KihonInfoSeach.m_dbClass = m_system_dbclass
        KihonInfoSeach.KihonInfoItemInit(WorksD.KihonL)

        '検索項目入力数
        Dim iInputCnt As Integer = 0

        '検索項目取得

        'With DataGridView2
        With m_dataTable2

            For iCol As Integer = 1 To .Columns.Count - 1

                If .Rows(0)(iCol) Is Nothing Then
                Else
                    If .Rows(0)(iCol) = "" Then
                        Try
                            KihonInfoSeach.KihonL(iCol - 1).item_value = ""
                        Catch ex As Exception
                        End Try
                    Else
                        iInputCnt += 1
                        Try
                            KihonInfoSeach.KihonL(iCol - 1).item_value = .Rows(0)(iCol)
                        Catch ex As Exception
                        End Try
                    End If
                End If

            Next

        End With

        ''クエリを発行して検索する
        'QuerySerach(arySearch)

        If iInputCnt > 0 Then
            'リストを検索する
            ListSearch()
            '一覧表示
            SetDataToGrid(True)
        Else
            '一覧表示
            SetDataToGrid()
        End If

    End Sub

    ''' <summary>
    ''' 絞り込み
    ''' 2013/05/29 作成
    ''' 2013/05/30 削除
    ''' SQLエラー起きず、IDR.Readができないため、使用しない

    ''' </summary>
    ''' <remarks></remarks>
    Private Sub QuerySerach(ByVal arySearch As ArrayList)

        'システム設定mdbに接続する

        If ConnectSystemDB(m_system_dbclass) = False Then
            MsgBox(err_msg_systemdb_connect, MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        WorksT = New Works(m_system_dbclass)
        WorksT.m_search_ary = arySearch
        WorksT.KihonInfoCopy(WorksD.KihonL)
        WorksT.GetSearchDataToList()

        'システム設定mdbに切断する
        DisConnectDB(m_system_dbclass)

    End Sub

    ''' <summary>
    ''' 絞り込み
    ''' メモリ内を検索する
    ''' 2013/05/30 作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ListSearch()

        Dim WCnt As Integer     'Worksカウント

        Dim KCnt As Integer     '検索項目カウント


        For WCnt = 0 To WorksT.WorksL.Count - 1

            Dim flg_parts As Boolean = True    '部分一致

            For KCnt = 0 To WorksT.WorksL(WCnt).KihonL.Count - 1
                Try
                    If KihonInfoSeach.KihonL(KCnt).item_value.Trim <> "" Then
                        If WorksT.WorksL(WCnt).KihonL(KCnt).item_value.IndexOf(KihonInfoSeach.KihonL(KCnt).item_value) >= 0 Then
                            '部分一致
                        Else
                            '一致しない

                            flg_parts = False
                        End If
                    End If
                Catch ex As Exception
                    MsgBox("値設定エラー", MsgBoxStyle.OkOnly)
                    Exit Sub
                End Try
            Next

            If flg_parts = True Then
                WorksT.WorksL(WCnt).m_search_flg = True
            Else
                WorksT.WorksL(WCnt).m_search_flg = False
            End If

        Next

    End Sub



    '''' <summary>
    '''' 絞り込み
    '''' 2013/05/29 作成
    '''' </summary>
    '''' <remarks></remarks>
    'Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

    '    With DataGridView1

    '        If .Rows.Count < 0 Then Exit Sub

    '    End With

    '    Dim arySearch As New ArrayList

    '    With DataGridView2

    '        For iCol As Integer = 1 To .Columns.Count - 1

    '            If .Rows(0).Cells(iCol).Value Is Nothing Then
    '            Else
    '                If .Rows(0).Cells(iCol).Value = "" Then
    '                Else
    '                    Try
    '                        arySearch.Add(.Rows(0).Cells(iCol).Value)
    '                        arySearch.Add(WorksD.KihonL(iCol - 1).ID)
    '                    Catch ex As Exception
    '                    End Try
    '                End If
    '            End If

    '        Next

    '    End With

    '    'システム設定mdbに接続する

    '    If ConnectSystemDB(m_system_dbclass) = False Then
    '        MsgBox(err_msg_systemdb_connect, MsgBoxStyle.OkOnly)
    '        Exit Sub
    '    End If

    '    WorksT = New Works(m_system_dbclass)
    '    WorksT.m_search_ary = arySearch
    '    WorksT.KihonInfoCopy(WorksD.KihonL)
    '    WorksT.GetSearchDataToList()

    '    'システム設定mdbに切断する
    '    DisConnectDB(m_system_dbclass)

    '    '一覧表示
    '    SetDataToGrid()

    '    'Dim i As Integer
    '    'KihonInforSeach = New WorksTable
    '    'KihonInforSeach.m_dbClass = m_system_dbclass
    '    'KihonInforSeach.GetDataToList1()

    '    ''検索値を取得する

    '    'With DataGridView2
    '    '    For i = 0 To WorksT.WorksL(0).KihonL.Count - 1
    '    '        KihonInforSeach.KihonL(i).item_value = .Rows(0).Cells(KihonInforSeach.KihonL(i).item_cell_name).Value
    '    '    Next
    '    'End With

    '    'Dim iRowNo As Integer = 0

    '    'With DataGridView1

    '    '    .Rows.Clear()

    '    '    For i = 0 To WorksT.WorksL.Count - 1

    '    '        Dim bRet As Boolean = False
    '    '        Dim sCount As Integer = 0
    '    '        For ii As Integer = 0 To WorksT.WorksL(i).KihonL.Count - 1
    '    '            Try
    '    '                If Not KihonInforSeach.KihonL(ii).item_value Is Nothing Then
    '    '                    If KihonInforSeach.KihonL(ii).item_value.Trim <> "" Then
    '    '                        If WorksT.WorksL(i).KihonL(ii).item_value.IndexOf(KihonInforSeach.KihonL(ii).item_value) >= 0 Then
    '    '                            bRet = True
    '    '                            Exit For
    '    '                        End If
    '    '                    Else
    '    '                        sCount += 1
    '    '                    End If
    '    '                Else
    '    '                    sCount += 1
    '    '                End If
    '    '            Catch ex As Exception
    '    '                MsgBox("値設定エラー", MsgBoxStyle.OkOnly)
    '    '                Exit Sub
    '    '            End Try
    '    '        Next
    '    '        If bRet = True Or sCount = 12 Then
    '    '            .Rows.Add()
    '    '            iRowNo = .Rows.Count - 1
    '    '            .Rows(iRowNo).Cells(0).Value = iRowNo
    '    '            For ii As Integer = 0 To WorksT.WorksL(i).KihonL.Count - 1
    '    '                .Rows(iRowNo).Cells(WorksT.WorksL(i).KihonL(ii).item_cell_name).Value =
    '    '                    WorksT.WorksL(i).KihonL(ii).item_value
    '    '            Next
    '    '        End If

    '    '        .AutoSize = True
    '    '    Next

    '    'End With

    'End Sub

#End Region

#Region "一覧ダブルクリック"

    Private Sub DataGridView1_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles DataGridView1.MouseDoubleClick
        openKentouKihonInfoTabDialog()
    End Sub

    Public Sub openKentouKihonInfoTabDialog()
        ' initialize
        Me.Tag = Nothing

        Dim ListIDX As Integer = 0          'WorksT.WorksLのインデックス

        dim doesOpenDialog As Boolean = False
        With DataGridView1

            'Dim iRowSel As Integer = .HitTest(e.X, e.Y).RowIndex
            Dim iRowSel As Integer = .SelectedIndex

            If iRowSel < 0 Then
            Else
                ListIDX = intField(m_dataTable1.Rows(iRowSel)(m_columnName2Header1("LIDX")))

                '選択された行の工事データの存在チェック
                If System.IO.File.Exists(WorksT.WorksL(ListIDX).SavePath & "\" & m_Keisoku_mdb_name) = False Then
                    MsgBox("選択した工事データは削除または移動されたため" & "規定値のコピーはできません。", MsgBoxStyle.OkOnly)
                Else
                    'If MsgBox("選択した工事の規定値がコピーされます。" & vbCrLf & "よろしいですか？", MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then
                    'Else

                    'End If
                    Me.Tag = WorksT.WorksL(ListIDX)
                    doesOpenDialog = True
                End If
            End If

        End With

        If doesOpenDialog Then
            ' ①初期画面から「新規」あるいは「開く」を押すと初期画面が閉じる（非表示にする）

            'Me.parentWindow.Visibility = System.Windows.Visibility.Hidden

            Dim kentou As New KentouKihonInfoTabDialog
            kentou.WindowState = Windows.WindowState.Maximized
            kentou.IsNew = False
            kentou.kojiData = Me
            'kentou.ShowDialog()
            kentou.Show()
            AddHandler kentou.Closed, AddressOf kentou_closed
            Me.parentWindow.WindowState = Windows.WindowState.Minimized
            Me.parentWindow.IsEnabled = False

            ' ①初期画面から「新規」あるいは「開く」を押すと初期画面が閉じる（非表示にする）

            'Me.parentWindow.Close()
        End If

        'Me.Dispose()
        'Me.Close()
    End Sub

    Private Sub kentou_closed(sender As Object, e As System.EventArgs)
        Me.parentWindow.IsEnabled = True
        Me.parentWindow.WindowState = Windows.WindowState.Maximized
    End Sub

#End Region


    'Kiryu Add 20160427 Sta 工事データリストの削除機能   **********************
    'xamlに右クリックでのコンテキストメニューを追加
    Private Sub DeleteItem_Clik(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)

        Dim ListIDX As Integer = 0          'WorksT.WorksLのインデックス

        Dim iRowSel As Integer = DataGridView1.SelectedIndex
        'Dim  As Generic.IList(Of System.Windows.Controls.DataGridCellInfo) = DataGridView1.SelectedCells
        If iRowSel >= 0 Then 'リストが空なら処理しない
            ListIDX = intField(m_dataTable1.Rows(iRowSel)(m_columnName2Header1("LIDX")))

            If ConnectSystemDB(m_system_dbclass) = False Then
                MsgBox(err_msg_systemdb_connect, MsgBoxStyle.OkOnly)
                Exit Sub
            End If

            Dim W_ID As String = WorksT.WorksL(ListIDX).ID.ToString
            Dim sql_txt As String = "DELETE FROM Works WHERE ID=" & W_ID
            Dim IDR As IDataReader = m_system_dbclass.DoSelect(sql_txt)

            '工事データリストを全クリアして、workテーブルを再読み込み
            m_dataTable1.Rows.Clear()
            GetData()
            SetDataToGrid()

            DisConnectDB(m_system_dbclass)
        End If


    End Sub
    'Kiryu Add 20160427 End ****************************************************

#Region "OKボタン"
    ''' <summary>
    ''' OKボタン
    ''' 2013/05/21 作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        Dim iRowSelIndex As Integer         '選択した行のインデックス
        Dim ListIDX As Integer = 0          'WorksT.WorksLのインデックス

        With DataGridView1

            'iRowSelIndex = .CurrentRow.Index
            iRowSelIndex = .SelectedIndex

            If iRowSelIndex < 0 Then Exit Sub

            ListIDX = intField(m_dataTable1.Rows(iRowSelIndex)(m_columnName2Header1("LIDX")))

        End With

        If m_form = "基本情報" Then

            '選択された行の工事データの存在チェック
            If System.IO.File.Exists(WorksT.WorksL(ListIDX).SavePath & "\" & m_Keisoku_mdb_name) = False Then
                MsgBox("選択した工事データは削除または移動されたため" & vbCrLf & "規定値のコピーはできません。", MsgBoxStyle.OkOnly)
                Exit Sub
            End If

            Me.Tag = WorksT.WorksL(ListIDX)

            'Me.Dispose()
            'Me.Close()

        ElseIf m_form = "規定値" Then

            '選択された行の工事データの存在チェック
            If System.IO.File.Exists(WorksT.WorksL(ListIDX).SavePath & "\" & m_Keisoku_mdb_name) = False Then
                MsgBox("選択した工事データは削除または移動されたため" & vbCrLf & "規定値のコピーはできません。", MsgBoxStyle.OkOnly)
                Exit Sub
            End If

            If MsgBox("選択した工事の規定値がコピーされます。" & vbCrLf & "よろしいですか？", MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then
                Exit Sub
            End If

            '計測データmdb存在チェック
            If ConnectDB(WorksT.WorksL(ListIDX).SavePath & "\" & m_Keisoku_mdb_name, m_keisoku_dbclass) = True Then
                '計測データmdb接続


                '規定値データを読み込む
                'データ取得

                Dim SSD As New SunpoSetTable
                SSD.m_dbClass = m_keisoku_dbclass
                SunpoSetL = SSD.GetDataToList()
                If SunpoSetL Is Nothing Then
                    Exit Sub
                End If

            End If

            '計測データmdb切断
            DisConnectDB(m_keisoku_dbclass)

        End If

    End Sub

#End Region

#Region "Cancelボタン"
    ''' <summary>
    ''' Cancelボタン
    ''' 2013/05/21 作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

        Me.Tag = Nothing

        'Me.Close()

    End Sub

#End Region

#Region "条件クリアボタン"

    ''' <summary>
    ''' 条件クリアボタン
    ''' 2013/05/30 作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click

        'With DataGridView2
        With m_dataTable2

            .Rows.Clear()

            .Rows.Add()

        End With

    End Sub

#End Region






















#Region "サンプル作成"

    Private SunpouKL As New List(Of SunpoKaisekiTable)

    ''' <summary>
    ''' CSV出力ボタン
    ''' テスト的に作成するので、本機能はメインの機能に影響しないようにする
    ''' 2013/05/31 作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnKojiExcelOut.Click

        'リスト取得

        If GetSunpouBunsekiList() = False Then
            Exit Sub
        End If

        ''csv出力

        'SunpouCsvOut()

        'Excel出力

        SunpouExcelOut()

        'SetExcelOut()

    End Sub

    ''' <summary>
    ''' 寸法解析データの各工事ごとの差異を取得する

    ''' 2013/06/03 作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Function GetSunpouBunsekiList() As Boolean

        GetSunpouBunsekiList = True

        'システム設定mdbへ接続

        If ConnectSystemDB(m_system_dbclass) = False Then
            MsgBox(err_msg_systemdb_connect)
            GetSunpouBunsekiList = False
            Exit Function
        End If

        Dim SunpouL As New List(Of SunpoSetTable)
        Dim SunpouD As New SunpoSetTable
        'システム設定mdbからSunpoSetデータを取得する

        SunpouD.m_dbClass = m_system_dbclass
        SunpouL = SunpouD.GetDataToList()
        SunpouKL = New List(Of SunpoKaisekiTable)
        Dim SaiL As New List(Of SaiTable)
        'Dim SunpouKL As New List(Of SunpoKaisekiTable)
        For Each SSD As SunpoSetTable In SunpouL
            Dim SKD As New SunpoKaisekiTable
            SKD.copy(SSD)
            SunpouKL.Add(SKD)
        Next

        For i As Integer = 0 To SunpouL.Count - 1
            Dim SKD As New SunpoKaisekiTable
            SKD.copy(SunpouL(i))

            'For ii As Integer = 0 To CHani + 1
            '    Dim dDeviation As Double = (dblField(SKD.KiteiMax) - dblField(SKD.KiteiMin)) / CHani
            '    Dim HSG As New HistogramTable
            '    HSG.Deviation = dDeviation
            '    If ii = 0 Then
            '        HSG.DataKukanMin = CDbl(0)
            '        HSG.DataKukanMax = dblField(SKD.KiteiMin)
            '    ElseIf ii = CHani + 1 Then
            '        HSG.DataKukanMin = dblField(SKD.KiteiMax)
            '        HSG.DataKukanMax = CDbl(0)
            '    Else
            '        HSG.DataKukanMin = dblField(SKD.KiteiMin) + (HSG.Deviation * (ii - 1))
            '        HSG.DataKukanMax = dblField(SKD.KiteiMin) + (HSG.Deviation * ii)
            '    End If
            '    HSG.Hindo = 0

            '    If SunpouKL(i).HistgramL Is Nothing Then
            '        SunpouKL(i).HistgramL = New List(Of HistogramTable)
            '    End If
            '    SunpouKL(i).HistgramL.Add(HSG)
            'Next

            For ii As Integer = 0 To CHani + 1
                Dim dDeviation As Double = (dblField(SKD.KiteiMax) - dblField(SKD.KiteiMin)) / CHani
                Dim HSG As New HistogramTable
                HSG.Deviation = dDeviation
                'If ii = CHani + 1 Then
                '    HSG.DataKukanMin = dblField(SKD.KiteiMax)
                '    HSG.DataKukanMax = CDbl(0)
                'Else
                '    HSG.DataKukanMin = dblField(SKD.KiteiMin) + (HSG.Deviation * (ii - 1))
                '    HSG.DataKukanMax = dblField(SKD.KiteiMin) + (HSG.Deviation * ii)
                'End If
                If ii = 0 Then
                    HSG.DataKukanMin = CDbl(0)
                    HSG.DataKukanMax = dblField(SKD.KiteiMin)
                ElseIf ii = CHani + 1 Then
                    HSG.DataKukanMin = dblField(SKD.KiteiMax)
                    HSG.DataKukanMin = dblField(SKD.KiteiMax)
                    HSG.DataKukanMax = CDbl(0)
                Else
                    HSG.DataKukanMin = dblField(SKD.KiteiMin) + (HSG.Deviation * (ii - 1))
                    HSG.DataKukanMax = dblField(SKD.KiteiMin) + (HSG.Deviation * ii)
                End If
                HSG.Hindo = 0

                If SunpouKL(i).HistgramL Is Nothing Then
                    SunpouKL(i).HistgramL = New List(Of HistogramTable)
                End If
                SunpouKL(i).HistgramL.Add(HSG)
            Next

        Next

        'システム設定mdbの接続解除
        DisConnectDB(m_system_dbclass)

        '各工事分
        For Each WK As WorksTable In WorksT.WorksL

            If WK.m_search_flg = True And WK.flg_Kaiseki = "1" Then

                '計測データmdbのパス
                Dim iKeisokuPath As String = WK.SavePath & "\" & m_Keisoku_mdb_name

                '計測データmdbが存在するデータ
                If System.IO.File.Exists(iKeisokuPath) = True Then

                    '保存された計測データmdbに接続

                    If ConnectDB(iKeisokuPath, m_keisoku_dbclass) = False Then
                        MsgBox(err_msg_keisokumdb_connect)
                        DisConnectDB(m_system_dbclass)
                        GetSunpouBunsekiList = False
                        Exit Function
                    End If

                    '計測データmdbからSunpouSetデータを取得する

                    Dim WorksSunpouL As New List(Of SunpoSetTable)
                    SunpouD.m_dbClass = m_keisoku_dbclass
                    WorksSunpouL = SunpouD.GetDataToList()

                    'WK.m_dbClass = m_keisoku_dbclass
                    'WK.GetDataToList()
                    'For Each ttt As KihonInfoTable In WK.KihonL

                    'Next

                    'For Each SSL As SunpoSetTable In WK.SunpoSetL

                    'Next


                    For i As Integer = 0 To WorksSunpouL.Count - 1

                        Dim SaiD As New SaiTable
                        'Worksテーブル
                        SaiD.WorksD = New WorksTable
                        SaiD.WorksD.copy(WK)
                        If SaiD.WorksKihonL Is Nothing Then
                            SaiD.WorksKihonL = New List(Of WorksKihonTable)
                        End If
                        'WorksInfoテーブル
                        For Each WKI As WorksKihonTable In WK.WorksKihonL
                            SaiD.WorksKihonL.Add(WKI)
                        Next
                        '差異 = 計測値 - 規定値
                        SaiD.sai = CDbl(WorksSunpouL(i).SunpoVal) - CDbl(WorksSunpouL(i).KiteiVal)
                        SaiD.SunpoVal = CDbl(WorksSunpouL(i).SunpoVal)

                        If SunpouKL(i).SaiL Is Nothing Then
                            SunpouKL(i).SaiL = New List(Of SaiTable)
                        End If

                        'For ii As Integer = 0 To CHani + 1
                        '    If ii = 0 Then
                        '        If SunpouKL(i).HistgramL(ii).DataKukanMax > dblField(SaiD.sai) Then
                        '            SunpouKL(i).HistgramL(ii).Hindo += 1
                        '            Exit For
                        '        End If
                        '    ElseIf ii = CHani + 1 Then
                        '        If SunpouKL(i).HistgramL(ii).DataKukanMin < dblField(SaiD.sai) Then
                        '            SunpouKL(i).HistgramL(ii).Hindo += 1
                        '            Exit For
                        '        End If
                        '    ElseIf SunpouKL(i).HistgramL(ii).DataKukanMax >= dblField(SaiD.sai) And
                        '           SunpouKL(i).HistgramL(ii).DataKukanMin <= dblField(SaiD.sai) Then
                        '        SunpouKL(i).HistgramL(ii).Hindo += 1
                        '        Exit For
                        '    End If
                        'Next

                        For ii As Integer = 0 To CHani + 1
                            If ii = 0 Then
                                If SunpouKL(i).HistgramL(ii).DataKukanMax > dblField(SaiD.sai) Then
                                    SunpouKL(i).HistgramL(ii).Hindo += 1
                                    Exit For
                                End If
                            ElseIf ii = CHani + 1 Then
                                If SunpouKL(i).HistgramL(ii).DataKukanMin < dblField(SaiD.sai) Then
                                    SunpouKL(i).HistgramL(ii).Hindo += 1
                                    Exit For
                                End If
                            ElseIf SunpouKL(i).HistgramL(ii).DataKukanMax >= dblField(SaiD.sai) And
                                   SunpouKL(i).HistgramL(ii).DataKukanMin <= dblField(SaiD.sai) Then
                                SunpouKL(i).HistgramL(ii).Hindo += 1
                                Exit For
                            End If
                        Next

                        'For ii As Integer = 0 To CHani
                        '    If ii = 0 Then
                        '        If SunpouKL(i).HistgramL(ii).DataKukanMax > dblField(SaiD.sai) Then
                        '            SunpouKL(i).HistgramL(ii).Hindo += 1
                        '            Exit For
                        '        End If
                        '    ElseIf ii = CHani Then
                        '        If (SunpouKL(i).HistgramL(ii).DataKukanMin < dblField(SaiD.sai) And
                        '            SunpouKL(i).HistgramL(ii).DataKukanMin < dblField(SaiD.sai)) Or
                        '            SunpouKL(i).HistgramL(ii).DataKukanMin < dblField(SaiD.sai) Then
                        '            SunpouKL(i).HistgramL(ii).Hindo += 1
                        '            Exit For
                        '        End If
                        '    ElseIf SunpouKL(i).HistgramL(ii).DataKukanMax >= dblField(SaiD.sai) And
                        '           SunpouKL(i).HistgramL(ii).DataKukanMin <= dblField(SaiD.sai) Then
                        '        SunpouKL(i).HistgramL(ii).Hindo += 1
                        '        Exit For
                        '    End If
                        'Next

                        SunpouKL(i).SaiL.Add(SaiD)

                    Next

                    '保存された計測データmdbの接続解除
                    DisConnectDB(m_keisoku_dbclass)

                End If

            End If

        Next

    End Function

    ''' <summary>
    ''' 寸法解析データの各工事ごとの差異を取得する

    ''' 2013/06/03 作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SunpouCsvOut()

        '保存先のExcelファイルのパス
        Dim m_csv_file_name As String = ""

        'SaveFileDialogクラスのインスタンスを作成
        Dim sfd As New SaveFileDialog()

        'はじめのファイル名を指定する

        sfd.FileName = "寸法データ.csv"
        'はじめに表示されるフォルダを指定する

        sfd.InitialDirectory = "C:\"
        '[ファイルの種類]に表示される選択肢を指定する

        sfd.Filter = "テキストファイル(*.txt;*.csv)|*.txt;*.csv|すべてのファイル(*.*)|*.*"
        '[ファイルの種類]ではじめに
        '「すべてのファイル」が選択されているようにする
        sfd.FilterIndex = 2
        'タイトルを設定する

        sfd.Title = "保存先のファイルを選択してください"
        'ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
        sfd.RestoreDirectory = True
        '既に存在するファイル名を指定したとき警告する

        'デフォルトでTrueなので指定する必要はない

        sfd.OverwritePrompt = True
        '存在しないパスが指定されたとき警告を表示する
        'デフォルトでTrueなので指定する必要はない

        sfd.CheckPathExists = True

        'ダイアログを表示する
        If sfd.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            'OKボタンがクリックされたとき

            '選択されたファイル名を表示する
            m_csv_file_name = sfd.FileName
        End If

        'If System.IO.File.Exists(m_csv_file_name) = True Then
        '    Dim mr As MsgBoxResult
        '    mr = MsgBox(System.IO.Path.GetFileName(m_csv_file_name) & "は既に存在します。" & vbCrLf & "上書きしますか？", MsgBoxStyle.YesNo)
        '    If mr = MsgBoxResult.Yes Then
        '        System.IO.File.Delete(m_csv_file_name)
        '    Else
        '        Exit Sub
        '    End If
        'End If

        'CSVファイルに書き込むときに使うEncoding
        Dim enc As System.Text.Encoding = _
            System.Text.Encoding.GetEncoding("Shift_JIS")

        '開く
        Dim sr As New System.IO.StreamWriter(m_csv_file_name, False, enc)

        For Each WK As SunpoKaisekiTable In SunpouKL

            Dim strField As String = ""
            strField = WK.SunpoName
            Dim strSaiField As String = ""
            For Each WK1 As SaiTable In WK.SaiL
                If strSaiField <> "" Then
                    strSaiField = strSaiField & ","
                End If
                strSaiField = strSaiField & WK1.sai
            Next
            Dim strHistgramField As String = ""
            For Each WKH1 As HistogramTable In WK.HistgramL
                If strHistgramField <> "" Then
                    strHistgramField = strHistgramField & ","
                End If
                strHistgramField = strHistgramField & WKH1.DataKukanMin & ","
                strHistgramField = strHistgramField & WKH1.Hindo
            Next

            If strSaiField <> "" Then
                strField = strField & ","
            End If
            strField = strField & strSaiField

            If strHistgramField <> "" Then
                strField = strField & ","
            End If
            strField = strField & strHistgramField

            sr.WriteLine(strField)
        Next

        '閉じる

        sr.Close()

        MsgBox(System.IO.Path.GetFileName(m_csv_file_name) & "を出力しました。")

    End Sub

    ''' <summary>
    ''' 寸法解析データの各工事ごとの差異を取得する

    ''' 2013/06/04 作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SunpouExcelOut()

        '保存先のCSVファイルのパス
        Dim m_excel_file_name As String = ""

        'Excel出力先を選択する

        m_excel_file_name = GetFileSelect()
        If m_excel_file_name = "" Then
            Exit Sub
        End If

        'テンプレートからコピーする
        System.IO.File.Copy(m_TemplatePath & "\" & m_histgram_temlate, m_excel_file_name, True)

        Dim xlApp As Excel.Application
        Dim xlBook As Excel.Workbook
        Dim xlSheet As Excel.Worksheet

        Dim strSheetName As String                          'シート名
        strSheetName = "Sheet1"

        Dim xlBooks As Excel.Workbooks
        Dim xlSheets As Excel.Sheets

        Try

            GetExcelPID()
            xlApp = New Excel.Application
            GetExcelNewPID()
            xlBooks = xlApp.Workbooks
            xlBook = xlBooks.Open(m_excel_file_name)
            xlSheets = xlBook.Worksheets

            For i As Integer = 1 To SunpouKL.Count
                'シートをコピーする
                xlSheets(i).Copy(After:=xlSheets(i))
                xlSheets(i + 1).Name = SunpouKL(i - 1).SunpoName

                xlSheet = xlBook.Sheets(i + 1)
                For ii As Integer = 0 To SunpouKL(i - 1).HistgramL.Count - 1

                    If ii = SunpouKL(i - 1).HistgramL.Count - 1 Then
                        xlSheet.Cells(ii + 1, 1) = SunpouKL(i - 1).HistgramL(ii).DataKukanMin & "以上"
                    Else
                        xlSheet.Cells(ii + 1, 1) = SunpouKL(i - 1).HistgramL(ii).DataKukanMax.ToString("0.000")
                    End If
                    xlSheet.Cells(ii + 1, 2) = SunpouKL(i - 1).HistgramL(ii).Hindo

                Next

                'xlSheet.Activate()
                'xlSheet.ChartObjects("グラフ 1").Activate()
                ''xlBook.ActiveChart.SeriesCollection(1).delete()
                ''xlBook.ActiveChart.SeriesCollection.NewSeries()
                'xlBook.ActiveChart.SeriesCollection(1).Name = "=""" & SunpouKL(i - 1).SunpoName & """"
                'xlBook.ActiveChart.SeriesCollection(1).Values = "=" & SunpouKL(i - 1).SunpoName & "!$B$1:$B$" & SunpouKL(i - 1).HistgramL.Count & ""
                'xlBook.ActiveChart.SeriesCollection(1).XValues = "=" & SunpouKL(i - 1).SunpoName & "!$A$1:$A$" & SunpouKL(i - 1).HistgramL.Count & ""

                xlSheet.Activate()
                xlSheet.ChartObjects("グラフ 1").Activate()
                xlBook.ActiveChart.ChartArea.Select()
                xlBook.ActiveChart.SetSourceData(Source:=xlSheet.Range("$A$1:$B$" & SunpouKL(i - 1).HistgramL.Count & ""))
                xlBook.ActiveChart.SeriesCollection(1).Name = "=""" & SunpouKL(i - 1).SunpoName & """"

            Next

            'テンプレートシートを隠す

            'xlSheets(1).visible = False

            'テンプレートシートを削除する
            xlApp.DisplayAlerts = False
            xlSheets(1).delete()

            '元データシートを表示する
            xlSheet = xlBook.Sheets.Add(After:=xlBook.Sheets(xlBook.Sheets.Count))
            xlBook.Sheets(xlBook.Sheets.Count).Name = "元データ"

            Dim iRowNo As Integer = 0
            Dim iColNo As Integer = 0

            For i As Integer = 0 To SunpouKL.Count - 1

                If i = 0 Then
                    For ii As Integer = 0 To SunpouKL(i).SaiL.Count - 1
                        For iii As Integer = 0 To SunpouKL(i).SaiL(ii).WorksKihonL.Count - 1
                            iRowNo = iii + 1
                            iColNo = 1
                            xlSheet.Cells(iRowNo, iColNo) = SunpouKL(i).SaiL(ii).WorksD.KihonL(iii).item_name
                        Next
                    Next

                    For ii As Integer = 0 To SunpouKL(i).SaiL.Count - 1

                        For iii As Integer = 0 To SunpouKL(i).SaiL(ii).WorksD.KihonL.Count - 1
                            iRowNo = iii + 1
                            iColNo = ii + 2
                            xlSheet.Cells(iRowNo, ii + 2) = SunpouKL(i).SaiL(ii).WorksD.KihonL(iii).item_value
                            xlSheet.Cells(iRowNo + 1, ii + 2) = "計測値"
                        Next

                    Next

                    xlSheet.Cells(iRowNo + 1, iColNo + 1) = "平均値"
                    xlSheet.Cells(iRowNo + 1, iColNo + 2) = "標準偏差"
                    xlSheet.Cells(iRowNo + 1, iColNo + 3) = "最大値"
                    xlSheet.Cells(iRowNo + 1, iColNo + 4) = "最小値"

                    iRowNo += 1
                End If

                xlSheet.Cells(iRowNo + 1, 1) = SunpouKL(i).SunpoName
                For ii As Integer = 0 To SunpouKL(i).SaiL.Count - 1
                    iColNo = ii + 2
                    xlSheet.Cells(iRowNo + 1, iColNo) = SunpouKL(i).SaiL(ii).SunpoVal

                Next

                xlSheet.Cells(iRowNo + 1, iColNo + 1) = "=AVERAGE(B" & iRowNo + 1 & ":" & GetColNo(iColNo) & iRowNo + 1 & ")"
                xlSheet.Cells(iRowNo + 1, iColNo + 2) = "=STDEV(B" & iRowNo + 1 & ":" & GetColNo(iColNo) & iRowNo + 1 & ")"
                xlSheet.Cells(iRowNo + 1, iColNo + 3) = "=MAX(B" & iRowNo + 1 & ":" & GetColNo(iColNo) & iRowNo + 1 & ")"
                xlSheet.Cells(iRowNo + 1, iColNo + 4) = "=MIN(B" & iRowNo + 1 & ":" & GetColNo(iColNo) & iRowNo + 1 & ")"

                iRowNo += 1

            Next

            xlSheet.Range(xlSheet.Cells(1, 1), xlSheet.Cells(65535, 256)).EntireColumn.AutoFit()
            xlSheet.Range(xlSheet.Cells(1, 1), xlSheet.Cells(iRowNo, iColNo + 4)).Borders.LineStyle = 1
            xlSheet.Columns("B:" & GetColNo(iColNo)).Hidden = True

            xlBook.Save()
            MRComObject(xlSheets)
            xlBook.Close(False)
            MRComObject(xlBook)
            MRComObject(xlBooks)
            xlApp.Quit()
            MRComObject(xlApp)

            GC.Collect()

        Catch ex As Exception

        End Try

        ExKillProcess()

        System.Diagnostics.Process.Start(m_excel_file_name)

    End Sub

    ''' <summary>
    ''' 寸法解析データの出力先を選択する

    ''' 2013/06/04 作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Function GetFileSelect() As String

        '保存先のCSVファイルのパス
        Dim m_excel_file_name As String = ""

        'SaveFileDialogクラスのインスタンスを作成
        Dim sfd As New SaveFileDialog()

        With sfd

            'はじめのファイル名を指定する

            .FileName = "寸法データ.xls"
            'はじめに表示されるフォルダを指定する

            .InitialDirectory = "C:\"
            '[ファイルの種類]に表示される選択肢を指定する

            .Filter = "テキストファイル(*.xls)|*.xls"
            '[ファイルの種類]ではじめに
            '「すべてのファイル」が選択されているようにする
            .FilterIndex = 2
            'タイトルを設定する

            .Title = "保存先のファイルを選択してください"
            'ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            .RestoreDirectory = True
            '既に存在するファイル名を指定したとき警告する

            'デフォルトでTrueなので指定する必要はない

            .OverwritePrompt = True
            '存在しないパスが指定されたとき警告を表示する
            'デフォルトでTrueなので指定する必要はない

            .CheckPathExists = True

            'ダイアログを表示する
            If .ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                'OKボタンがクリックされたとき

                '選択されたファイル名を表示する
                m_excel_file_name = .FileName
            End If

        End With

        GetFileSelect = m_excel_file_name

    End Function



    'Private Function SetExcelOut() As Boolean
    '    'Dim iColCount As Integer = UBound(IHedderName)
    '    Dim iRow As Integer = 0         '行

    '    Dim iCol As Integer = 0         '列

    '    Dim iRet As Boolean = True
    '    Dim strFilePath As String = ""
    '    'Dim strTempKaisyaBunrui As String = ""
    '    'Dim strJvBunrui As String = ""
    '    'Dim iRowSum As Integer = 0
    '    Dim i As Integer = 0
    '    Dim iIndex As Integer = 0
    '    Dim iSheetsFlg As Boolean = False
    '    'Dim iSheetIndex As Integer = 0

    '    If GetFileSelect(strFilePath) = False Then
    '        SetExcelOut = False
    '        Exit Function
    '    End If

    '    GetExcelPID()
    '    Dim xlExcel As New Excel.Application
    '    GetExcelNewPID()
    '    Dim xlBookS As Excel.Workbook
    '    Dim xlSheet As Excel.Worksheet
    '    Dim strRowAddress As Integer = 0
    '    Dim strColAddress As Integer = 0

    '    xlExcel.DisplayAlerts = False
    '    'Excelファイルの存在チェック
    '    If System.IO.File.Exists(strFilePath) = True Then

    '        xlBookS = xlExcel.Workbooks.Open(strFilePath)
    '    Else
    '        xlBookS = xlExcel.Workbooks.Add()
    '    End If

    '    'Excelを非表示にする
    '    xlExcel.Application.Visible = False

    '    Try
    '        iRow = 1

    '        For iRow = 1 To SunpouKL.Count

    '            'シートの位置を決定する

    '            iIndex = xlBookS.Sheets.Count + 1

    '            For i = 1 To xlBookS.Sheets.Count

    '                If xlBookS.Sheets(i).name = SunpouKL(iRow - 1).SunpoName Then
    '                    iIndex = i
    '                    Exit For
    '                End If

    '                'SheetXがあった場合

    '                If xlBookS.Sheets(i).name.ToString.Substring(0, 5) = "Sheet" AndAlso _
    '                    iSheetsFlg = False Then
    '                    iSheetsFlg = True
    '                    iIndex = i
    '                    Exit For
    '                End If

    '            Next

    '            If xlBookS.Sheets.Count < iIndex Then
    '                xlSheet = xlBookS.Sheets.Add(After:=xlBookS.Sheets(xlBookS.Sheets.Count))
    '            End If
    '            xlSheet = xlBookS.Sheets(iIndex)
    '            xlSheet.Name = SunpouKL(iRow - 1).SunpoName

    '            'シートをクリアする
    '            strRowAddress = xlSheet.UsedRange.End(Excel.XlDirection.xlDown).Row
    '            strColAddress = xlSheet.UsedRange.End(Excel.XlDirection.xlToRight).Column
    '            xlSheet.Range(xlSheet.Cells(1, 1), xlSheet.Cells(strRowAddress, strColAddress)).Clear()

    '            xlSheet.Shapes.Activate.AddChart.Select()


    '            'xlBookS.Sheets(iRow).Copy(After:=xlBookS.Sheets(iRow))
    '            'xlBookS.Sheets.Add()
    '            'xlBookS.Sheets(iRow + 1).Name = SunpouKL(iRow - 1).SunpoName

    '            xlSheet = xlExcel.Sheets(iRow + 1)
    '            For ii As Integer = 0 To SunpouKL(iRow - 1).HistgramL.Count - 1

    '                If ii = SunpouKL(iRow - 1).HistgramL.Count - 1 Then
    '                    xlSheet.Cells(ii + 1, 1) = SunpouKL(iRow - 1).HistgramL(ii).DataKukanMin & "以上"
    '                Else
    '                    xlSheet.Cells(ii + 1, 1) = SunpouKL(iRow - 1).HistgramL(ii).DataKukanMax.ToString("0.000")
    '                End If
    '                xlSheet.Cells(ii + 1, 2) = SunpouKL(iRow - 1).HistgramL(ii).Hindo

    '            Next

    '            xlSheet.Activate()
    '            xlSheet.ChartObjects("グラフ 1").Activate()
    '            xlExcel.ActiveChart.SeriesCollection(1).delete()
    '            xlExcel.ActiveChart.SeriesCollection.NewSeries()
    '            xlExcel.ActiveChart.SeriesCollection(1).Name = "=""" & SunpouKL(i - 1).SunpoName & """"
    '            xlExcel.ActiveChart.SeriesCollection(1).Values = "=" & SunpouKL(i - 1).SunpoName & "!$B$1:$B$" & SunpouKL(i - 1).HistgramL.Count & ""
    '            xlExcel.ActiveChart.SeriesCollection(1).XValues = "=" & SunpouKL(i - 1).SunpoName & "!$A$1:$A$" & SunpouKL(i - 1).HistgramL.Count & ""

    '        Next

    '        'ファイルを保存する

    '        If Val(xlExcel.Application.Version) < 12 Then
    '            xlBookS.SaveAs(strFilePath, Excel.XlFileFormat.xlExcel9795)
    '        Else
    '            xlBookS.SaveAs(strFilePath, Excel.XlFileFormat.xlExcel7)
    '        End If

    '    Catch ex As Exception
    '        MsgBox("Excel出力に失敗しました。")
    '        iRet = False
    '    Finally

    '        xlBookS.Close(False)
    '        System.Runtime.InteropServices.Marshal.ReleaseComObject(xlBookS)
    '        xlBookS = Nothing
    '        System.Runtime.InteropServices.Marshal.ReleaseComObject(xlSheet)
    '        xlSheet = Nothing
    '        xlExcel.DisplayAlerts = False
    '        xlExcel.Quit()
    '        System.Runtime.InteropServices.Marshal.ReleaseComObject(xlExcel)
    '        xlExcel = Nothing
    '        GC.Collect(2)
    '        ExKillProcess()
    '    End Try

    '    SetExcelOut = iRet

    'End Function


#End Region

    ''' <summary>
    ''' 単品分析用ボタン
    ''' 2013/11/13 作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ButtonTanpinKalc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonTanpinKalc.Click

        'リスト取得

        If GetBunsekiList() = False Then
            Exit Sub
        End If

        'Excel出力

        TanpinExcelOut()

    End Sub

    ''' <summary>
    ''' 分析データのリストを取得する

    ''' 2013/11/13 作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Function GetBunsekiList() As Boolean

        GetBunsekiList = True

        '各工事分
        For Each WK As WorksTable In WorksT.WorksL

            If WK.m_search_flg = True And WK.flg_Kaiseki = "1" Then

                '計測データmdbのパス
                Dim iKeisokuPath As String = WK.SavePath & "\" & m_Keisoku_mdb_name

                '計測データmdbが存在するデータ
                If System.IO.File.Exists(iKeisokuPath) = True Then

                    '保存された計測データmdbに接続

                    If ConnectDB(iKeisokuPath, m_keisoku_dbclass) = False Then
                        MsgBox(err_msg_keisokumdb_connect)
                        DisConnectDB(m_system_dbclass)
                        GetBunsekiList = False
                        Exit Function
                    End If

                    WK.m_dbClass = m_keisoku_dbclass
                    WK.GetDataToList()

                    '保存された計測データmdbの接続解除
                    DisConnectDB(m_keisoku_dbclass)

                End If
            End If
        Next

    End Function

    ''' <summary>
    ''' 単品分析データのリストをExcel出力する

    ''' 2013/11/13 作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub TanpinExcelOut()
        Dim iSts As Integer = 0

        '保存先のCSVファイルのパス
        Dim m_excel_file_name As String = ""

        'Excel出力先を選択する

        m_excel_file_name = GetFileSelect()
        If m_excel_file_name = "" Then
            Exit Sub
        End If

        'テンプレートからコピーする
        System.IO.File.Copy(m_TemplatePath & "\" & m_Tanpin_Template, m_excel_file_name, True)

        Dim xlApp As Excel.Application
        Dim xlBook As Excel.Workbook
        Dim xlSheet As Excel.Worksheet

        Dim strSheetName As String                          'シート名
        strSheetName = "単品分析"

        Dim xlBooks As Excel.Workbooks
        Dim xlSheets As Excel.Sheets
        Dim strCheckName As String
        Dim strCheckTab As String
        Try

            ' 製品状態がすべてあるかのチェック
            Dim iCheckFlg As Integer = 0
            Dim iCheckBaseFlg As Integer = 0

            strCheckTab = Common.WorksD.KihonL.Item(4).item_name
            For Each KouteiName As SubInfoTable In Common.WorksD.KihonL.Item(4).SubInfoL

                strCheckName = KouteiName.ItemValue
                iCheckBaseFlg += 1

                Dim iLoopFlg As Integer = 0
                For Each WK As WorksTable In WorksT.WorksL

                    If WK.m_search_flg = True And WK.flg_Kaiseki = "1" Then

                        ' 書き込み対象部材の検索
                        For Each KihonD As KihonInfoTable In WK.KihonL
                            If KihonD.item_name = strCheckTab And KihonD.item_value = strCheckName Then
                                iCheckFlg += 1
                                iLoopFlg = 1
                                Exit For
                            End If
                        Next
                    End If
                    If iLoopFlg <> 0 Then
                        Exit For
                    End If
                Next
            Next
            If iCheckBaseFlg <> iCheckFlg Then
                MsgBox(err_msg_koutei_nothing, MsgBoxStyle.Critical)
                Exit Sub
            End If

            GetExcelPID()
            xlApp = New Excel.Application
            GetExcelNewPID()
            xlBooks = xlApp.Workbooks
            xlBook = xlBooks.Open(m_excel_file_name)
            xlSheets = xlBook.Worksheets
            xlSheet = xlBook.Sheets(strSheetName)

            '各工事分
            Dim icol As Integer
            Dim irow As Integer
            icol = 0
            For Each WK As WorksTable In WorksT.WorksL

                If WK.m_search_flg = True And WK.flg_Kaiseki = "1" Then

                    ' 基本情報書き込み
                    irow = 0
                    For Each KihonD As KihonInfoTable In WK.KihonL

                        xlSheet.Cells(irow + 1, 2) = KihonD.item_name
                        xlSheet.Cells(irow + 1, icol + 3) = KihonD.item_value
                        irow += 1

                    Next

                    ' 寸法情報書き込み
                    irow = 7
                    For Each SunpoD As SunpoSetTable In WK.SunpoSetL

                        xlSheet.Cells(irow + 1, 2) = SunpoD.SunpoName
                        xlSheet.Cells(irow + 1, 8) = SunpoD.KiteiVal
                        xlSheet.Cells(irow + 1, icol + 3) = SunpoD.SunpoVal
                        irow += 1

                    Next
                    icol += 1
                End If
            Next

            xlBook.Save()
            MRComObject(xlSheets)
            xlBook.Close(False)
            MRComObject(xlBook)
            MRComObject(xlBooks)
            xlApp.Quit()
            MRComObject(xlApp)

            GC.Collect()

        Catch ex As Exception
            MsgBox(err_msg_excelout_err, MsgBoxStyle.Critical)
            iSts = 1
        End Try

        ExKillProcess()

        If iSts = 0 Then
            System.Diagnostics.Process.Start(m_excel_file_name)
        End If

    End Sub

    ''' <summary>
    ''' 全体傾向分析用ボタン
    ''' 2013/11/13 作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ZentaiBunsekiBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ZentaiBunsekiBtn.Click

        'リスト取得

        If GetBunsekiList() = False Then
            Exit Sub
        End If

        'Excel出力

        ZentaiExcelOut()

    End Sub



    ''' <summary>
    ''' 単品分析データのリストをExcel出力する

    ''' 2013/11/13 作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ZentaiExcelOut()
        Dim iSts As Integer = 0

        '保存先のCSVファイルのパス
        Dim m_excel_file_name As String = ""

        'Excel出力先を選択する

        m_excel_file_name = GetFileSelect()
        If m_excel_file_name = "" Then
            Exit Sub
        End If

        'テンプレートからコピーする
        System.IO.File.Copy(m_TemplatePath & "\" & m_Zentai_Template, m_excel_file_name, True)

        Dim xlApp As Excel.Application
        Dim xlBook As Excel.Workbook
        Dim xlSheet As Excel.Worksheet
        Dim xlBooks As Excel.Workbooks
        Dim xlSheets As Excel.Sheets
        Dim strSheetName As String = "全体傾向分析"

        Try

            GetExcelPID()
            xlApp = New Excel.Application
            GetExcelNewPID()
            xlBooks = xlApp.Workbooks
            xlBook = xlBooks.Open(m_excel_file_name)
            xlSheets = xlBook.Worksheets

            Dim strKouteiName As String
            Dim ioutflg As Integer

            Dim strRotno As String = ""
            Dim strWrkNo As String = ""
            Dim strKouteiTag As String
            'Dim strRotTag As String
            Dim strWrkTag As String

            strKouteiTag = Common.WorksD.KihonL.Item(4).item_name       ' 工程検索タグ名取得

            'strRotTag = Common.WorksD.KihonL.Item(1).item_name          ' ロットNo検索タグ取得

            strWrkTag = Common.WorksD.KihonL.Item(3).item_name          ' ワーク種別検索タグ取得


            For Each KouteiName As SubInfoTable In Common.WorksD.KihonL.Item(4).SubInfoL

                xlSheet = xlBook.Sheets(KouteiName.ItemValue)
                strKouteiName = KouteiName.ItemValue
                '各工事分
                Dim icol As Integer
                Dim irow As Integer
                icol = 0
                For Each WK As WorksTable In WorksT.WorksL

                    If WK.m_search_flg = True And WK.flg_Kaiseki = "1" Then

                        ' 書き込み対象部材の検索
                        ioutflg = 0
                        For Each KihonD As KihonInfoTable In WK.KihonL

                            If KihonD.item_name = strKouteiTag And KihonD.item_value = strKouteiName Then
                                ioutflg = 1
                                Exit For
                            End If

                        Next

                        ' 書き込み対象部材の場合

                        If ioutflg = 1 Then

                            ' 基本情報書き込み
                            irow = 0
                            For Each KihonD As KihonInfoTable In WK.KihonL

                                xlSheet.Cells(irow + 1, 2) = KihonD.item_name
                                xlSheet.Cells(irow + 1, icol + 3) = KihonD.item_value
                                irow += 1

                                ' ロットNo取得

                                'If KihonD.item_name = strRotTag And Len(strRotno) = 0 Then
                                '    strRotno = KihonD.item_value
                                'End If

                                ' ワークNo取得

                                If KihonD.item_name = strWrkTag And Len(strWrkNo) = 0 Then
                                    strWrkNo = KihonD.item_value
                                End If

                            Next

                            ' 寸法情報書き込み
                            irow = 7
                            xlSheet.Cells(irow, icol + 3) = "計測値"
                            For Each SunpoD As SunpoSetTable In WK.SunpoSetL

                                xlSheet.Cells(irow + 1, 2) = SunpoD.SunpoName
                                xlSheet.Cells(irow + 1, icol + 3) = SunpoD.SunpoVal
                                irow += 1

                            Next
                            icol += 1
                        End If

                    End If
                Next
            Next

            ' ロットNo、ワーク種別の書き込み
            xlSheet = xlBook.Sheets(strSheetName)
            xlSheet.Cells(1, 3) = strRotno
            xlSheet.Cells(2, 3) = strWrkNo
            ' xlSheet.Active()

            xlBook.Save()
            MRComObject(xlSheets)
            xlBook.Close(False)
            MRComObject(xlBook)
            MRComObject(xlBooks)
            xlApp.Quit()
            MRComObject(xlApp)

            GC.Collect()

        Catch ex As Exception
            MsgBox(err_msg_excelout_err, MsgBoxStyle.Critical)
            iSts = 1
        End Try

        ExKillProcess()

        If iSts = 0 Then
            System.Diagnostics.Process.Start(m_excel_file_name)
        End If

    End Sub

End Class