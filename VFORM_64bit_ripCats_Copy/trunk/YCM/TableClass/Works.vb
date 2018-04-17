Public Class Works

    Private Const K_works As String = "Works"
    Private Const K_kihoninfo As String = "Kihoninfo"

    Private ReadOnly W_ID As String() = {"ID"}
    Private ReadOnly W_SavePath As String() = {"SavePath"}
    Private ReadOnly W_CreateDate As String() = {"CreateDate"}
    Private ReadOnly W_MeasureDate As String() = {"MeasureDate"}


    Public flgSyusei As Boolean = False
    Public flgNewInput As Boolean = False
    Private _backup As WorksTable
    Public Const MaxFieldCnt = 3
    Public strFieldNames() As String
    Public strFieldTexts() As String

    Public m_dbClass As FBMlib.CDBOperateOLE

    Public WorksD As WorksTable
    Public WorksL As List(Of WorksTable)
    Public KihonL As List(Of KihonInfoTable)
    Public WKihonL As List(Of KihonInfoTable)

    Public m_search_ary As ArrayList

    Public Sub New()

    End Sub

    Public Sub New(ByVal m_system_dbClass As FBMlib.CDBOperateOLE)
        m_dbClass = m_system_dbClass
    End Sub

    Public Sub KihonInfoCopy(ByVal KIT As List(Of KihonInfoTable))

        With KIT
            WKihonL = New List(Of KihonInfoTable)
            For i As Long = 0 To .Count - 1
                Dim KID As New KihonInfoTable
                KID.copy(KIT(i))
                KID.item_value = ""
                WKihonL.Add(KID)
            Next
        End With

    End Sub

#Region "データ取得"
    Public Function GetDataToList() As Boolean

        Dim strSql As String = SQLTEXT()
        Dim IDR As IDataReader
        Dim bRet As Boolean = False

        Dim tempWorkID As String = ""

        IDR = m_dbClass.DoSelect(strSql)
        If Not IDR Is Nothing Then
            Dim WIDX As New WorksTable
            WIDX.SetFieldIndex(IDR)
            Dim WIIDX As New WorksKihonTable
            WIIDX.SetFieldIndex(IDR)
            If WorksL Is Nothing Then
                WorksL = New List(Of WorksTable)
            Else
                WorksL.Clear()
            End If
            Do While IDR.Read
                Dim WKD As New WorksTable
                WKD.GetDataByReader(IDR)
                WKD.m_search_flg = True

                If tempWorkID = WKD.ID Then
                    WKD.KihonInfoCopy(WKihonL)      'kihonInfoを作成する
                    Dim WKID As New WorksKihonTable
                    WKID.GetDataByReader(IDR)
                    For i As Integer = 0 To WKD.KihonL.Count - 1
                        If WKD.ID = WKID.Work_ID And WKD.KihonL(i).ID = WKID.KI_ID Then
                            WorksL(WorksL.Count - 1).KihonL(i).item_value = WKID.ItemValue
                            Exit For
                        End If
                    Next
                    WorksL(WorksL.Count - 1).WorksKihonL.Add(WKID)
                Else
                    WKD.KihonInfoCopy(WKihonL)      'kihonInfoを作成する
                    Dim WKID As New WorksKihonTable
                    WKID.GetDataByReader(IDR)
                    For i As Integer = 0 To WKD.KihonL.Count - 1
                        If WKD.ID = WKID.Work_ID And WKD.KihonL(i).ID = WKID.KI_ID Then
                            WKD.KihonL(i).item_value = WKID.ItemValue
                            Exit For
                        End If
                    Next
                    If WKD.WorksKihonL Is Nothing Then
                        WKD.WorksKihonL = New List(Of WorksKihonTable)
                    End If
                    WKD.WorksKihonL.Add(WKID)
                    WorksL.Add(WKD)
                End If
                tempWorkID = WKD.ID
            Loop
            IDR.Close()
        End If

        GetDataToList = bRet

    End Function

    Public Function GetDataToList1() As Boolean

        Dim WKD As New WorksTable
        WKD.m_dbClass = m_dbClass
        Dim WorksL1 As New List(Of WorksTable)
        WorksL1 = WKD.GetWorksDataToList()

        If WorksL Is Nothing Then
            WorksL = New List(Of WorksTable)
        Else
            WorksL.Clear()
        End If

        For i As Integer = 0 To WorksL1.Count - 1

            If WorksL1(i).GetKihonInfoData() = True Then
                For ii As Integer = 0 To WorksL1(i).KihonL.Count - 1
                    WorksL1(i).KihonL(ii).objControl = WKihonL(ii).objControl
                Next
                WorksL.Add(WorksL1(i))
            End If

        Next

    End Function

    ''' <summary>
    ''' 絞り込み条件でのWorksテーブルリストを取得

    ''' 2013/05/29 作成
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetSearchDataToList() As Boolean

        Dim IDR As IDataReader
        Dim bRet As Boolean = False

        Dim strSql As String = SQLTEXT1()

        IDR = m_dbClass.DoSelect(strSql)
        If Not IDR Is Nothing Then
            Dim WIDX As New WorksTable
            WIDX.SetFieldIndex(IDR)
            Dim WIIDX As New WorksKihonTable
            WIIDX.SetFieldIndex(IDR)
            If WorksL Is Nothing Then
                WorksL = New List(Of WorksTable)
            Else
                WorksL.Clear()
            End If

            Dim tempWorkID As String = ""

            Do While IDR.Read
                Dim WKD As New WorksTable
                WKD.GetDataByReader(IDR)

                If tempWorkID = WKD.ID Then
                    WKD.KihonInfoCopy(WKihonL)      'kihonInfoを作成する
                    Dim WKID As New WorksKihonTable
                    WKID.GetDataByReader(IDR)
                    For i As Integer = 0 To WKD.KihonL.Count - 1
                        If WKD.ID = WKID.Work_ID And WKD.KihonL(i).ID = WKID.KI_ID Then
                            WorksL(WorksL.Count - 1).KihonL(i).item_value = WKID.ItemValue
                            Exit For
                        End If
                    Next
                Else
                    WKD.KihonInfoCopy(WKihonL)      'kihonInfoを作成する
                    Dim WKID As New WorksKihonTable
                    WKID.GetDataByReader(IDR)
                    For i As Integer = 0 To WKD.KihonL.Count - 1
                        If WKD.ID = WKID.Work_ID And WKD.KihonL(i).ID = WKID.KI_ID Then
                            WKD.KihonL(i).item_value = WKID.ItemValue
                            Exit For
                        End If
                    Next
                    WorksL.Add(WKD)
                End If
                tempWorkID = WKD.ID
            Loop
            IDR.Close()
        End If

        GetSearchDataToList = bRet

    End Function

#End Region

#Region "SQL"

    Private Function SQLTEXT() As String

        Dim strSql As String
        Dim strSelect As String
        Dim strFrom As String
        Dim strWhere As String
        Dim strOrderBy As String

        strSelect = "SELECT "
        strSelect = strSelect & "Works.ID AS WK_ID, "
        strSelect = strSelect & "Works.SavePath AS WK_SavePath, "
        strSelect = strSelect & "Works.CreateDate AS WK_CreateDate, "
        strSelect = strSelect & "Works.MeasureDate AS WK_MeasureDate, "
        strSelect = strSelect & "Works.TypeID AS WK_TypeID, "
        strSelect = strSelect & "Works.flg_Kihon AS WK_flg_Kihon, "
        strSelect = strSelect & "Works.flg_Kiteiti AS WK_flg_Kiteiti, "
        strSelect = strSelect & "Works.flg_Gazou AS WK_flg_Gazou, "
        strSelect = strSelect & "Works.flg_kaiseki AS WK_flg_kaiseki, "
        strSelect = strSelect & "Works.flg_kakunin AS WK_flg_kakunin, "
        strSelect = strSelect & "Works.flg_Out AS WK_flg_Out, "
        strSelect = strSelect & "WorksKihon.ID AS WKI_ID, "
        strSelect = strSelect & "WorksKihon.WorkID AS WKI_WorkID, "
        strSelect = strSelect & "WorksKihon.KI_ID AS WKI_KI_ID, "
        strSelect = strSelect & "WorksKihon.ItemValue AS WKI_ItemValue "

        strFrom = "FROM "
        strFrom = strFrom & "Works INNER JOIN WorksKihon ON "
        strFrom = strFrom & "Works.ID = WorksKihon.WorkID "

        strWhere = "WHERE "
        strWhere = strWhere & "Works.TypeID = " & CommonTypeID & " "

        strOrderBy = "ORDER BY "
        strOrderBy = strOrderBy & "Works.ID, "
        strOrderBy = strOrderBy & "WorksKihon.ID "

        strSql = strSelect & strFrom & strWhere & strOrderBy

        SQLTEXT = strSql

    End Function

    Public Function SQLTEXT1() As String

        Dim strWorkIDSql As String = ""
        'Dim strSelect1 As String = "SELECT * FROM ("
        'Dim strSubSelect1 As String = "SELECT * FROM WorksKihon WHERE WorkID IN ("
        Dim strSelect1 As String = "SELECT WorkID FROM ("
        Dim strSubSelect1 As String = "SELECT WorkID FROM WorksKihon WHERE WorkID IN ("
        Dim strWhere1 As String = "WHERE WorkID IN ("
        Dim strSelectID As String = "SELECT WorkID "
        Dim strFrom1 As String = "FROM ("
        Dim strGroupBy As String = "GROUP BY WorkID"

        '検索対象のWork.IDを取得するSQL
        If m_search_ary.Count >= 2 Then
            strWorkIDSql = strSubSelect1 & GetSubQuary(m_search_ary(0), m_search_ary(1))
        End If

        For i As Integer = 2 To m_search_ary.Count - 1
            strWorkIDSql = strSelect1 & strWorkIDSql & ") " & strWhere1 & GetSubQuary(m_search_ary(i), m_search_ary(i + 1))
            i += 1
        Next

        If strWorkIDSql <> "" Then
            strWorkIDSql = strSelectID & strFrom1 & strWorkIDSql & ") " & strGroupBy
        End If

        ''Worksからデータを取得するSQL
        Dim strSql As String
        Dim strSelect As String
        Dim strFrom As String
        Dim strWhere As String
        Dim strOrderBy As String

        strSelect = "SELECT "
        strSelect = strSelect & "WK.ID AS WK_ID, "
        strSelect = strSelect & "WK.SavePath AS WK_SavePath, "
        strSelect = strSelect & "WK.CreateDate AS WK_CreateDate, "
        strSelect = strSelect & "WK.MeasureDate AS WK_MeasureDate, "
        strSelect = strSelect & "WK.TypeID AS WK_TypeID, "
        strSelect = strSelect & "WK.flg_Kihon AS WK_flg_Kihon, "
        strSelect = strSelect & "WK.flg_Kiteiti AS WK_flg_Kiteiti, "
        strSelect = strSelect & "WK.flg_Gazou AS WK_flg_Gazou, "
        strSelect = strSelect & "WK.flg_kaiseki AS WK_flg_kaiseki, "
        strSelect = strSelect & "WK.flg_kakunin AS WK_flg_kakunin, "
        strSelect = strSelect & "WK.flg_Out AS WK_flg_Out, "
        strSelect = strSelect & "WKK.ID AS WKI_ID, "
        strSelect = strSelect & "WKK.WorkID AS WKI_WorkID, "
        strSelect = strSelect & "WKK.KI_ID AS WKI_KI_ID, "
        strSelect = strSelect & "WKK.ItemValue AS WKI_ItemValue "

        strFrom = "FROM "
        strFrom = strFrom & "Works AS WK INNER JOIN WorksKihon AS WKK ON "
        strFrom = strFrom & "WK.ID = WKK.WorkID "

        strWhere = "WHERE "
        strWhere = strWhere & "WK.TypeID = " & CommonTypeID & " "
        If strWorkIDSql <> "" Then
            strWhere = strWhere & "AND WK.ID IN (" & strWorkIDSql & ") "
        End If

        strOrderBy = "ORDER BY "
        strOrderBy = strOrderBy & "WK.ID, "
        strOrderBy = strOrderBy & "WKK.ID "

        strSql = strSelect & strFrom & strWhere & strOrderBy

        SQLTEXT1 = strSql

    End Function

    Private Function GetSubQuary(ByVal iItem As String, ByVal iK_ID As String) As String

        Dim strSql As String

        strSql = "SELECT "
        strSql = strSql & "WorksKihon.WorkID "
        strSql = strSql & "FROM WorksKihon "
        strSql = strSql & "WHERE "
        strSql = strSql & "((WorksKihon.KI_ID = " & iK_ID & ") And (WorksKihon.ItemValue Like '*" & iItem & "*'))"
        strSql = strSql & ")"

        GetSubQuary = strSql

    End Function

    'Public Function SQLTEXT1() As String

    '    Dim strWorkIDSql As String = ""
    '    'Dim strSelect1 As String = "SELECT * FROM ("
    '    'Dim strSubSelect1 As String = "SELECT * FROM WorksKihon WHERE WorkID IN ("
    '    Dim strSelect1 As String = "SELECT WorkID FROM ("
    '    'Dim strSubSelect1 As String = "SELECT WorkID FROM WorksKihon WHERE WorkID IN ("
    '    Dim strSubSelect1 As String = "SELECT WorksKihon_0.WorkID FROM WorksKihon AS WorksKihon_0 WHERE WorksKihon_0.WorkID IN ("
    '    Dim strWhere1 As String = "WHERE WorkID IN ("
    '    Dim strSelectID As String = "SELECT WK1.WorkID "
    '    Dim strFrom1 As String = "FROM ("
    '    Dim strGroupBy As String = "GROUP BY WK1.WorkID"

    '    Dim iCnt As Integer = 1

    '    '検索対象のWork.IDを取得するSQL
    '    If m_search_ary.Count >= 2 Then
    '        strWorkIDSql = strSubSelect1 & GetSubQuary(m_search_ary(0), m_search_ary(1), iCnt)
    '    End If

    '    For i As Integer = 2 To m_search_ary.Count - 1
    '        iCnt += 1
    '        strWorkIDSql = strSelect1 & strWorkIDSql & ") " & strWhere1 & GetSubQuary(m_search_ary(i), m_search_ary(i + 1), iCnt)
    '        i += 1
    '    Next

    '    'strWorkIDSql = strSelectID & strFrom1 & strWorkIDSql & ") " & strGroupBy
    '    If strWorkIDSql <> "" Then
    '        strWorkIDSql = strSelectID & strFrom1 & strWorkIDSql & ") AS WK1 " & strGroupBy
    '    End If

    '    ''Worksからデータを取得するSQL
    '    Dim strSql As String
    '    Dim strSelect As String
    '    Dim strFrom As String
    '    Dim strWhere As String
    '    Dim strOrderBy As String

    '    strSelect = "SELECT "
    '    strSelect = strSelect & "WK.ID AS WK_ID, "
    '    strSelect = strSelect & "WK.SavePath AS WK_SavePath, "
    '    strSelect = strSelect & "WK.CreateDate AS WK_CreateDate, "
    '    strSelect = strSelect & "WK.MeasureDate AS WK_MeasureDate, "
    '    strSelect = strSelect & "WK.TypeID AS WK_TypeID, "
    '    strSelect = strSelect & "WK.flg_Kihon AS WK_flg_Kihon, "
    '    strSelect = strSelect & "WK.flg_Kiteiti AS WK_flg_Kiteiti, "
    '    strSelect = strSelect & "WK.flg_Gazou AS WK_flg_Gazou, "
    '    strSelect = strSelect & "WK.flg_kaiseki AS WK_flg_kaiseki, "
    '    strSelect = strSelect & "WK.flg_kakunin AS WK_flg_kakunin, "
    '    strSelect = strSelect & "WK.flg_Out AS WK_flg_Out, "
    '    strSelect = strSelect & "WKK.ID AS WKI_ID, "
    '    strSelect = strSelect & "WKK.WorkID AS WKI_WorkID, "
    '    strSelect = strSelect & "WKK.KI_ID AS WKI_KI_ID, "
    '    strSelect = strSelect & "WKK.ItemValue AS WKI_ItemValue "

    '    strFrom = "FROM "
    '    strFrom = strFrom & "Works AS WK INNER JOIN WorksKihon AS WKK ON "
    '    strFrom = strFrom & "WK.ID = WKK.WorkID "

    '    strWhere = "WHERE "
    '    strWhere = strWhere & "WK.TypeID = " & CommonTypeID & " "
    '    If strWorkIDSql <> "" Then
    '        strWhere = strWhere & "AND WK.ID IN (" & strWorkIDSql & ") "
    '    End If

    '    strOrderBy = "ORDER BY "
    '    strOrderBy = strOrderBy & "WK.ID, "
    '    strOrderBy = strOrderBy & "WKK.ID "

    '    strSql = strSelect & strFrom & strWhere & strOrderBy

    '    SQLTEXT1 = strSql

    'End Function

    'Private Function GetSubQuary(ByVal iItem As String,
    '                             ByVal iK_ID As String,
    '                             ByVal iCnt As Integer) As String

    '    Dim strSql As String
    '    Dim strTableNmae As String = "WorksKihon_" & iCnt

    '    strSql = "SELECT "
    '    strSql = strSql & strTableNmae & ".WorkID "
    '    strSql = strSql & "FROM WorksKihon AS " & strTableNmae & " "
    '    strSql = strSql & "WHERE "
    '    strSql = strSql & "((" & strTableNmae & ".KI_ID = " & iK_ID & ") And (" & strTableNmae & ".ItemValue Like '*" & iItem & "*'))"
    '    strSql = strSql & ")"

    '    GetSubQuary = strSql

    'End Function

#End Region

End Class
