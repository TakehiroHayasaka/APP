Public Class KiteitiInput

    Public m_Keisoku_mdb_path As String                         '計測データ.mdbパス
    Public m_keisoku_dbclass As FBMlib.CDBOperateOLE            '計測データ.mdb
    Public m_system_dbclass As FBMlib.CDBOperateOLE             'システム設定.mdb

    Public SunpoSetL As List(Of SunpoSetTable)                  'SunpoSetテーブルリスト


    Private Sub KiteitiInput_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        DisConnectDB(m_keisoku_dbclass)
    End Sub

    Private Sub KiteitiInput_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '初期設定

        If SetInitFormKitei() = False Then
            Exit Sub
        End If

        'グリッドヘッダ設定

        SetGridHeaddderKitei()

        'データを取得してグリッドに表示
        SetDataKitei()

    End Sub

    ''' <summary>
    ''' 初期設定

    ''' 2013/05/15 作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Function SetInitFormKitei() As Boolean

        SetInitFormKitei = True

        '計測データ.mdbに接続する

        If ConnectDB(m_Keisoku_mdb_path, m_keisoku_dbclass) = False Then
            MsgBox("データベースを開くことができません。")
            SetInitFormKitei = False
            Exit Function
        End If

    End Function

    ''' <summary>
    ''' グリッドヘッダ設定

    ''' 2013/05/15 作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetGridHeaddderKitei()

        With DataGridView1

            'ヘッダを中央
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            '並び替え禁止
            For Each T As DataGridViewColumn In DataGridView1.Columns
                T.SortMode = DataGridViewColumnSortMode.NotSortable
            Next

        End With

    End Sub

    ''' <summary>
    ''' データを取得してグリッドに表示
    ''' 2013/05/15 作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetDataKitei()

        'データ取得

        Dim SSD As New SunpoSetTable
        SSD.m_dbClass = m_keisoku_dbclass
        SunpoSetL = SSD.GetDataToList()
        If SunpoSetL Is Nothing Then
            Exit Sub
        End If

        Dim iRowNo As Integer = 0

        'データ表示
        With DataGridView1

            .Rows.Clear()

            .Rows.Add(SunpoSetL.Count)

            For i As Integer = 0 To SunpoSetL.Count - 1

                iRowNo = i + 1
                Try
                    .Rows(i).Cells(0).Value = iRowNo                        'No
                    .Rows(i).Cells(1).Value = SunpoSetL(i).SunpoMark        '部位

                    .Rows(i).Cells(2).Value = SunpoSetL(i).SunpoName        '検査項目
                    .Rows(i).Cells(3).Value = SunpoSetL(i).KiteiVal         '規定値
                    .Rows(i).Cells(4).Value = SunpoSetL(i).KiteiMin         '最小許容値
                    .Rows(i).Cells(5).Value = SunpoSetL(i).KiteiMax         '最大許容値
                Catch ex As Exception
                    MsgBox("値設定エラー", MsgBoxStyle.OkOnly)
                    Exit Sub
                End Try

            Next

        End With

    End Sub

#Region "保存"

    ''' <summary>
    ''' 保存ボタン
    ''' 2013/05/15 作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveKitei.Click

        SaveSunpo()

    End Sub

    ''' <summary>
    ''' 保存処理

    ''' 2013/05/15 作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Function SaveSunpo() As Boolean

        Dim bRet As Boolean = True

        SaveSunpo = True

        'グリッドから規定値・最小許容値・最大許容値を取得する

        GetDataFromGrid()

        '規定値データ(SunpoSet)更新
        If SaveSunpoData() = False Then
            SaveSunpo = False
        End If

    End Function

    ''' <summary>
    ''' グリッドから規定値・最小許容値・最大許容値を取得

    ''' 2013/05/15 作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetDataFromGrid()

        With DataGridView1

            For i As Integer = 0 To .Rows.Count - 1

                '規定値
                SunpoSetL(i).KiteiVal = .Rows(i).Cells("KiteiVal").Value
                '最小許容値
                SunpoSetL(i).KiteiMin = .Rows(i).Cells("KiteiMin").Value
                '最大許容値
                SunpoSetL(i).KiteiMax = .Rows(i).Cells("KiteiMax").Value

            Next

        End With

    End Sub

    ''' <summary>
    ''' 規定値データ(SunpoSet)更新
    ''' 2013/05/15 作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Function SaveSunpoData() As Boolean

        m_keisoku_dbclass.BeginTrans()

        For i As Integer = 0 To SunpoSetL.Count - 1

            SunpoSetL(i).m_dbClass = m_keisoku_dbclass
            If SunpoSetL(i).SaveData(False) = False Then
                m_keisoku_dbclass.RollbackTrans()
                SaveSunpoData = False
                Exit For
            End If

        Next

        m_keisoku_dbclass.CommitTrans()

        SaveSunpoData = True

    End Function

#End Region

#Region "工事データ一覧"

    ''' <summary>
    ''' 工事データ一覧ボタン
    ''' 2013/05/15 作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

    End Sub

#End Region

#Region "閉じる"

    ''' <summary>
    ''' 閉じるボタン
    ''' 2013/05/15 作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        DisConnectDB(m_keisoku_dbclass)

        Me.Dispose()
        Me.Close()

    End Sub

#End Region

End Class