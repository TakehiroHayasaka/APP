﻿
Imports System.Runtime.InteropServices
Imports System
Imports System.IO
Imports System.Drawing
Imports System.Diagnostics
Imports Microsoft.VisualBasic.FileIO
Imports System.Windows.FrameworkElement
Imports System.Text
Imports System.Text.RegularExpressions
Imports HalconDotNet

Public Class Sekkei_Keisoku
    Public SekkeiDataList As New List(Of SekkeiData)
    Public Shared controlData As New getControlData
    Dim pos1 As New CLookPoint
    Dim pos2 As New CLookPoint
    Dim pos3 As New CLookPoint
    Private Const con1 = 10000
    Dim l_count As Integer
    Dim List As New List(Of String)

    Dim items As New System.Collections.ObjectModel.ObservableCollection(Of ZahyoChiItems)

    Dim intMaxRecord As Integer
    Dim Xsekkei(0 To con1) As Double, Ysekkei(0 To con1) As Double, Zsekkei(0 To con1) As Double, _
    Xweight(0 To con1) As Double, Yweight(0 To con1) As Double, Zweight(0 To con1) As Double, _
    XresKeisoku(0 To con1) As Double, YresKeisoku(0 To con1) As Double, ZresKeisoku(0 To con1) As Double, _
    Xkeisoku(0 To con1) As Double, Ykeisoku(0 To con1) As Double, Zkeisoku(0 To con1) As Double
    Dim str_lableName(0 To con1) As String, bln_LableUse(0 To con1) As Boolean
    Dim m_KeisokuTempMdbPath As String = m_koji_kanri_path & "\" & m_Keisoku_mdb_name

    Dim setteiTableFlg As Integer = 0
    Dim dataTableFlg As Integer = 0
    Class SekkeiData
        Public Property S_ID() As Integer
        Public Property S_Name() As String
        Public Property S_X() As Double
        Public Property S_Y() As Double
        Public Property S_Z() As Double
        Public Property K_PID() As Integer
        Public Property S_KijunFlg() As Integer
        Public Property S_Setsumei() As String
        Public Sub New(ByVal datas() As String, ByVal count As Integer, ByVal mojiText As String)
            S_ID = count
            S_Name = mojiText & datas(0)
            S_X = datas(1)
            S_Y = datas(2)
            S_Z = datas(3)
            S_KijunFlg = datas(4)
            S_Setsumei = datas(5)
        End Sub
        Public Sub New(ByVal ZCI As ZahyoChiItems)
            S_ID = ZCI.No
            S_Name = ZCI.SekkeiLabel1
            S_X = ZCI.SekkeiTenX
            S_Y = ZCI.SekkeiTenY
            S_Z = ZCI.SekkeiTenZ
            S_KijunFlg = 0
            S_Setsumei = ""
        End Sub
    End Class
    Public Class ZahyoChiItems   ' dataGridZahyo
        Public Property No() As Integer             'No
        Public Property SekkeiLabel1() As String     '設計ラベル
        ' Public Property SokutenMei1() As String      '測点名
        Private _SokutenMei1 As String
        Public Property SokutenMei1() As String '測点名
            Get
                Return _SokutenMei1
            End Get
            Set(ByVal value As String)
                For i As Long = 0 To nLookPoints - 1
                    If (gDrawPoints(i).LabelName = value) Then
                        _SokutenID = gDrawPoints(i).tid
                        Exit For
                    End If
                Next
                _SokutenMei1 = value
            End Set
        End Property
        Private _SokutenID As String
        Public ReadOnly Property SokutenID() As String '計測点ID
            Get
                Return _SokutenID
            End Get
        End Property

        Public Property SekkeiTenX() As Double      '設計点X
        Public Property SekkeiTenY() As Double      '設計点Y
        Public Property SekkeiTenZ() As Double      '設計点Z
        Public Property KeisokuTenX() As Double     '計測点X
        Public Property KeisokuTenY() As Double   '計測点Y
        Public Property KeisokuTenZ() As Double    '計測点Z
        Public Property IchiAwaseXw() As Double    '位置合わせの重みX
        Public Property IchiAwaseYw() As Double '位置合わせの重みY
        Public Property IchiAwaseZw() As Double '位置合わせの重みZ
        Public Property DiffX() As Double '差（計測点-設計点）X
        Public Property DiffY() As Double '差（計測点-設計点）Y
        Public Property DiffZ() As Double '差（計測点-設計点）Z
        Public Property DiffL() As Double '差（計測点-設計点）L

        Private _IsOverDiff As Boolean
        Public Property IsOverDiff() As Boolean
            Get
                If controlData.MinX < DiffX And DiffX < controlData.MaxX And controlData.MinY < DiffY And DiffY < controlData.MaxY And controlData.MinZ < DiffZ And DiffZ < controlData.MaxZ And controlData.MinL < DiffL And DiffL < controlData.MaxL Then
                    Return False
                Else
                    Return True
                End If
            End Get
            Set(ByVal value As Boolean)
                _IsOverDiff = value
            End Set
        End Property

        Private _IsMissMatch As Boolean
        Public Property IsMissMatch() As Boolean
            Get

                If SokutenMei1 = "" Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                _IsMissMatch = value
            End Set
        End Property

        Public isRenamed As Boolean = False ' 20170220 baluu add

        Public Shared Property SekkeiLabel As System.Collections.ObjectModel.ObservableCollection(Of String)
        Public Shared Property SokutenMei As List(Of String)

        Public Sub New(ByVal sss As List(Of String))
            SokutenMei = sss
        End Sub

        '20170221 baluu add start
        Public Sub New()

        End Sub
        '20170221 baluu add end

        Public Property colorBrush() As System.Windows.Media.Brush
            Set(ByVal value As System.Windows.Media.Brush)
                Throw New Exception("Unexpected call of dataGridZahyo.colorBrush=?")
            End Set
            Get
                If IsMissMatch() = True Then
                    Return System.Windows.Media.Brushes.Yellow
                End If
                If IsOverDiff() = True Then
                    Return System.Windows.Media.Brushes.Pink
                End If

            End Get
        End Property
    End Class
    Public Class getControlData
        Public Property Ten1Keisoku() As String
        Public Property Ten2Keisoku() As String
        Public Property Ten3Keisoku() As String
        Public Property Ten1Sekkei() As String
        Public Property Ten2Sekkei() As String
        Public Property Ten3Sekkei() As String
        Public Property MaxX() As String
        Public Property MaxY() As String
        Public Property MaxZ() As String
        Public Property MaxL() As String
        Public Property MinX() As String
        Public Property MinY() As String
        Public Property MinZ() As String
        Public Property MinL() As String
        Public Property Sekkei_Label() As String
    End Class
    '  Public m_getControlData As New List(Of getControlData)
    Private m_ZahyoChiItems As New System.Collections.ObjectModel.ObservableCollection(Of ZahyoChiItems)
    'Private Sub wpfSekkeiKeizoku_Loaded(sender As Object, e As System.) Handles wpfSekkeiKeizoku.Loaded
    '    座標値比較　読込
    '    Zahyouchi()
    'End Sub
    'Private Sub Zahyouchi()
    '    Dim items As New System.Collections.ObjectModel.ObservableCollection(Of ZahyoChiItems)
    '    Dim i As Integer = 0
    '    With items
    '        .Clear()

    '        For Each item In SekkeiDataList
    '            .Add(New ZahyoChiItems)
    '            Try
    '                .Item(i).No = item.S_ID
    '                .Item(i).SekkeiLabel1 = LabelingMoji.Text & item.S_Name
    '                .Item(i).SekkeiTenX = item.S_X
    '                .Item(i).SekkeiTenY = item.S_Y
    '                .Item(i).SekkeiTenZ = item.S_Z

    '            Catch ex As Exception
    '                MsgBox("値設定エラー", MsgBoxStyle.OkOnly)
    '                Exit Sub
    '            End Try
    '            i = i + 1
    '        Next

    '    End With

    '    With m_ZahyoChiItems
    '        .Clear()
    '        For ii As Integer = 0 To items.Count - 1
    '            .Add(items(ii))
    '        Next
    '    End With
    '    dataGridZahyo.ItemsSource = m_ZahyoChiItems

    'End Sub

    Private Sub wpfSekkeiKeizoku_Loaded(sender As Object, e As Windows.RoutedEventArgs) Handles wpfSekkeiKeizoku.Loaded
        For n As Integer = 0 To point_name.Count - 1
            If gDrawPoints(n).flgLabel = 1 Then
                List.Add(gDrawPoints(n).LabelName)
            End If
        Next
        List.Add("")
        List.Sort()
        cmbTen1keisoku.ItemsSource = List
        cmbTen2keisoku.ItemsSource = List
        cmbTen3keisoku.ItemsSource = List

        Txt_MaxX.Text = GetPrivateProfileInt("DesignValueCapture", "MAX_X", 0, My.Application.Info.DirectoryPath & "\vform.ini")
        Txt_MaxY.Text = GetPrivateProfileInt("DesignValueCapture", "MAX_Y", 0, My.Application.Info.DirectoryPath & "\vform.ini")
        Txt_MaxZ.Text = GetPrivateProfileInt("DesignValueCapture", "MAX_Z", 0, My.Application.Info.DirectoryPath & "\vform.ini")
        Txt_MaxL.Text = GetPrivateProfileInt("DesignValueCapture", "MAX_L", 0, My.Application.Info.DirectoryPath & "\vform.ini")
        Txt_MinX.Text = GetPrivateProfileInt("DesignValueCapture", "MIN_X", 0, My.Application.Info.DirectoryPath & "\vform.ini")
        Txt_MinY.Text = GetPrivateProfileInt("DesignValueCapture", "MIN_Y", 0, My.Application.Info.DirectoryPath & "\vform.ini")
        Txt_MinZ.Text = GetPrivateProfileInt("DesignValueCapture", "MIN_Z", 0, My.Application.Info.DirectoryPath & "\vform.ini")
        Txt_MinL.Text = GetPrivateProfileInt("DesignValueCapture", "MIN_L", 0, My.Application.Info.DirectoryPath & "\vform.ini")

        Txt_SekkeiLabel.Text = GetPrivateProfileInt("DesignValueCapture", "LABEL_SARCH_LENGE", 0, My.Application.Info.DirectoryPath & "\vform.ini")

        SetDataToControlFromDB()

        Txt_SekkeiFile.Focus()
        Txt_SekkeiFile.SelectionStart = Txt_SekkeiFile.Text.Length

    End Sub

    Private Sub SetDataToControlFromDB()

        YCM_ReadSekkeiKeisokuDataTable(m_KeisokuTempMdbPath)
        If Not m_ZahyoChiItems Is Nothing Then
            cmbTen1Sekkei.Items.Clear()
            cmbTen2sekkei.Items.Clear()
            cmbTen3Sekkei.Items.Clear()
            If m_ZahyoChiItems.Count > 0 Then
                If SekkeiDataList Is Nothing Then
                    SekkeiDataList = New List(Of SekkeiData)
                Else
                    SekkeiDataList.Clear()
                End If
                For Each ZCI As ZahyoChiItems In m_ZahyoChiItems
                    Dim newSD As New SekkeiData(ZCI)
                    SekkeiDataList.Add(newSD)
                Next

                For Each item In SekkeiDataList
                    cmbTen1Sekkei.Items.Add(item.S_Name)
                    cmbTen2sekkei.Items.Add(item.S_Name)
                    cmbTen3Sekkei.Items.Add(item.S_Name)
                Next
            End If
        End If
        YCM_ReadSekkeiKeisokuSetteiTable(m_KeisokuTempMdbPath)
        ' YCM_ReadSekkeiKeisokuDataTable(m_KeisokuTempMdbPath)
    End Sub
    Private Function YCM_ReadSekkeiKeisokuSetteiTable(ByVal strDBPath As String) As Integer
        Dim clsOPe As New CDBOperate
        Dim adoRet As ADODB.Recordset
        If clsOPe.ConnectDB(strDBPath) = False Then
            YCM_ReadSekkeiKeisokuSetteiTable = -1
        End If
        Dim strSQL As String

        If (Not ExistsTable(clsOPe, "[SekkeiKeisokuSettei]")) Then
            Exit Function
        End If
        '20170215 baluu del start
        'If CmbKeisokuset.SelectedItem = "" Then
        '    Txt_SekkeiFile.Text = ""
        '    LabelingMoji.Text = ""
        '    cmbTen1keisoku.SelectedValue = ""
        '    cmbTen2keisoku.SelectedValue = ""
        '    cmbTen3keisoku.SelectedValue = ""
        '    cmbTen1Sekkei.SelectedValue = ""
        '    cmbTen2sekkei.SelectedValue = ""
        '    cmbTen3Sekkei.SelectedValue = ""
        '    Txt_MaxX_Omomi.Text = ""
        '    Txt_MaxY_Omomi.Text = ""
        '    Txt_MaxZ_Omomi.Text = ""
        '    Txt_MaxL_Omomi.Text = ""
        '    Txt_HeikinX.Text = ""
        '    Txt_HeikinY.Text = ""
        '    Txt_HeikinZ.Text = ""
        '    Txt_HeikinL.Text = ""
        '    Txt_MinX_Omomi.Text = ""
        '    Txt_MinY_Omomi.Text = ""
        '    Txt_MinZ_Omomi.Text = ""
        '    Txt_MinL_Omomi.Text = ""
        '    Txt_hyojyunX.Text = ""
        '    Txt_hyojyunY.Text = ""
        '    Txt_hyojyunZ.Text = ""
        '    Txt_hyojyunL.Text = ""
        'End If
        '20170215 baluu del end
        strSQL = "SELECT * From [SekkeiKeisokuSettei] WHERE KeisokuSet = '" & strCurrentKeisokuSet & "'"
        adoRet = clsOPe.CreateRecordset(strSQL)
        If adoRet.RecordCount > 0 Then
            Do Until adoRet.EOF
                Txt_SekkeiFile.Text = adoRet("FileName").Value
                '20161116 baluu add start
                Txt_SekkeiFile.Select(Txt_SekkeiFile.Text.Length, 0)
                Txt_SekkeiFile.Focus()
                '20161116 baluu add end
                LabelingMoji.Text = adoRet("Moji").Value
                cmbTen1keisoku.SelectedValue = adoRet("Ten1Keisoku").Value
                cmbTen2keisoku.SelectedValue = adoRet("Ten2Keisoku").Value
                cmbTen3keisoku.SelectedValue = adoRet("Ten3Keisoku").Value
                '20161115 baluu add start
                For Each item In m_ZahyoChiItems
                    If adoRet("Ten1Keisoku").Value = "CT" & item.SokutenID Then
                        cmbTen1keisoku.SelectedValue = item.SokutenMei1

                    End If
                    If adoRet("Ten2Keisoku").Value = "CT" & item.SokutenID Then
                        cmbTen2keisoku.SelectedValue = item.SokutenMei1
                    End If
                    If adoRet("Ten3Keisoku").Value = "CT" & item.SokutenID Then
                        cmbTen3keisoku.SelectedValue = item.SokutenMei1
                    End If
                Next
                '20161115 baluu add end
                cmbTen1Sekkei.SelectedValue = adoRet("Ten1Sekkei").Value
                cmbTen2sekkei.SelectedValue = adoRet("Ten2Sekkei").Value
                cmbTen3Sekkei.SelectedValue = adoRet("Ten3Sekkei").Value
                adoRet.MoveNext()
            Loop
            setteiTableFlg = 1
        End If
        clsOPe.DisConnectDB()
        YCM_ReadSekkeiKeisokuSetteiTable = 0
    End Function

    Private Function YCM_ReadSekkeiKeisokuDataTable(ByVal strDBPath As String) As Integer
        Dim clsOPe As New CDBOperate
        Dim adoRet As ADODB.Recordset
        If clsOPe.ConnectDB(strDBPath) = False Then
            YCM_ReadSekkeiKeisokuDataTable = -1
        End If
        Dim strSQL As String

        If (Not ExistsTable(clsOPe, "[SekkeiKeisokuData]")) Then
            Exit Function
        End If
        m_ZahyoChiItems.Clear()

        strSQL = "SELECT * From [SekkeiKeisokuData] WHERE KeisokuSet = '" & strCurrentKeisokuSet & "'"
        adoRet = clsOPe.CreateRecordset(strSQL)
        If adoRet Is Nothing Then
            Exit Function
        End If
        If adoRet.RecordCount > 0 Then
            Do Until adoRet.EOF
                With m_ZahyoChiItems
                    Dim objNew As New ZahyoChiItems(List)
                    objNew.No = adoRet("ID").Value
                    objNew.SekkeiLabel1 = adoRet("Sekkei").Value
                    objNew.SokutenMei1 = adoRet("SokutenMei").Value
                    objNew.SekkeiTenX = adoRet("SekkeiTenX").Value
                    objNew.SekkeiTenY = adoRet("SekkeiTenY").Value
                    objNew.SekkeiTenZ = adoRet("SekkeiTenZ").Value
                    objNew.KeisokuTenX = adoRet("KeisokuTenX").Value
                    objNew.KeisokuTenY = adoRet("KeisokuTenY").Value
                    objNew.KeisokuTenZ = adoRet("KeisokuTenZ").Value
                    objNew.IchiAwaseXw = adoRet("IchiAwaseXw").Value
                    objNew.IchiAwaseYw = adoRet("IchiAwaseYw").Value
                    objNew.IchiAwaseZw = adoRet("IchiAwaseZw").Value
                    objNew.DiffX = adoRet("DiffX").Value
                    objNew.DiffY = adoRet("DiffY").Value
                    objNew.DiffZ = adoRet("DiffZ").Value
                    objNew.DiffL = adoRet("DiffL").Value

                    If objNew.SokutenMei1 = "" Then
                        objNew.IsMissMatch = True
                    Else
                        If objNew.KeisokuTenX = 0.0 Then
                            objNew.IsOverDiff = True
                        End If
                    End If

                    .Add(objNew)
                End With
                adoRet.MoveNext()
            Loop
            dataTableFlg = 1
        End If
        clsOPe.DisConnectDB()
        YCM_ReadSekkeiKeisokuDataTable = 0

        dataGridZahyo.ItemsSource = m_ZahyoChiItems
        dataGridZahyo.Items.Refresh()
        calcAverage()
        calcValues()
    End Function
    Private Sub btnSekkeiFile_Click(sender As Object, e As Windows.RoutedEventArgs) Handles btnSekkeiFile.Click
        Dim OpenFileDialog As New OpenFileDialog
        Dim LabelMoji As String = ""
        LabelMoji = LabelingMoji.Text

        OpenFileDialog.FileName = "*.csv"
        OpenFileDialog.Filter = "*.csv|*.csv"

        Dim result As System.Windows.Forms.DialogResult = OpenFileDialog.ShowDialog()
        If result = System.Windows.Forms.DialogResult.OK Then
            csv_path = OpenFileDialog.FileName()
            Txt_SekkeiFile.Text = csv_path
            '20161116 baluu add start
            Txt_SekkeiFile.Select(Txt_SekkeiFile.Text.Length, 0)
            Txt_SekkeiFile.Focus()
            '20161116 baluu add end
            If readDataFromFile(csv_path) = True Then
                cmbTen1Sekkei.Items.Clear()
                cmbTen2sekkei.Items.Clear()
                cmbTen3Sekkei.Items.Clear()
                For Each item In SekkeiDataList
                    cmbTen1Sekkei.Items.Add(item.S_Name)
                    cmbTen2sekkei.Items.Add(item.S_Name)
                    cmbTen3Sekkei.Items.Add(item.S_Name)

                    If item.S_KijunFlg = 1 Then
                        cmbTen1Sekkei.SelectedItem = item.S_Name
                    ElseIf item.S_KijunFlg = 2 Then
                        cmbTen2sekkei.SelectedItem = item.S_Name
                    ElseIf item.S_KijunFlg = 3 Then
                        cmbTen3Sekkei.SelectedItem = item.S_Name
                    End If

                Next
            End If
        End If
    End Sub
    Public Function readDataFromFile(ByVal filename As String) As Boolean
        readDataFromFile = False

        Dim fields As String()
        Dim sb As StringBuilder
        sb = New StringBuilder(500)
        Dim sb1 As StringBuilder
        sb1 = New StringBuilder(500)
        Dim fileDelimeter As Integer = GetPrivateProfileString("DesignValueCapture", "Kugiru", "", sb, sb.Capacity, My.Application.Info.DirectoryPath & "\vform.ini")
        Dim csvFormat As Integer = GetPrivateProfileString("DesignValueCapture", "CSV_FORMAT", "", sb1, sb1.Capacity, My.Application.Info.DirectoryPath & "\vform.ini")
        Dim orderOfData As String = sb1.ToString
        Dim isFormatOK As Boolean = True

        Dim delimiter As String = sb.ToString
        Dim i As Integer = 0
        Dim labelMoji As String = LabelingMoji.Text
        Dim count As Integer = 0
        Dim fileOrderList() As String = orderOfData.Split(sb.ToString)
        Dim tmpSekkeiDataList = New List(Of SekkeiData)

        Using parser As New TextFieldParser(filename, System.Text.Encoding.Default)
            parser.SetDelimiters(delimiter)
            While Not parser.EndOfData
                fields = parser.ReadFields()

                If i = 0 Then
                    '20161116 baluu add start
                    For kk As Integer = 0 To fields.Length - 1
                        If Array.IndexOf(fileOrderList, fields(kk)) = -1 Then
                            isFormatOK = False
                            Exit While
                        End If
                    Next
                    For jj As Integer = 0 To fileOrderList.Length - 1
                        If Array.IndexOf(fields, fileOrderList(jj)) = -1 Then
                            isFormatOK = False
                            Exit While
                        End If
                    Next
                    '20161116 baluu add end
                    i += 1
                    Continue While
                End If
                count = count + 1
                Dim fieldData(5) As String
                For ii As Integer = 0 To fileOrderList.Count - 1
                    If Trim(fileOrderList(ii)) = "LABEL" Then '20161116 baluu edit
                        fieldData(0) = fields(ii)
                    ElseIf Trim(fileOrderList(ii)) = "X" Then
                        fieldData(1) = fields(ii)
                    ElseIf Trim(fileOrderList(ii)) = "Y" Then
                        fieldData(2) = fields(ii)
                    ElseIf Trim(fileOrderList(ii)) = "Z" Then
                        fieldData(3) = fields(ii)
                    End If
                Next

                If fields.Count > 4 Then
                    fieldData(4) = fields(4)
                    If fields.Count > 5 Then
                        fieldData(5) = fields(5)
                    End If
                End If

                If IsNumeric(fieldData(1)) And IsNumeric(fieldData(2)) And IsNumeric(fieldData(3)) Then
                    Dim newobj As New SekkeiData(fieldData, count, labelMoji)
                    tmpSekkeiDataList.Add(newobj)
                    readDataFromFile = True
                Else
                    isFormatOK = False
                    Exit While
                End If

            End While
        End Using

        If isFormatOK = False Then
            MsgBox("設計値ファイルの書式が正しくありません。")
            Exit Function
        Else
            SekkeiDataList = New List(Of SekkeiData)
            SekkeiDataList = tmpSekkeiDataList
        End If



    End Function

    Private Sub btnKeisokuten1_Click(sender As Object, e As Windows.RoutedEventArgs) Handles btnKeisokuten1.Click
        If point_name.Count > 0 Then
            Me.Hide()
            IOUtil.GetPoint(pos1, "点目を指示：")
            sys_Labeling.p1 = pos1

            'LB_P1.Text = sys_Labeling.p1.LabelName

            'C_Set_P1.Text = pos1.LabelName  '13.1.29　選択した計測点をC_Set_P1のTextに
            cmbTen1keisoku.SelectedValue = pos1.LabelName
            '<コンボボックスSelectedIndexChanged>イベントに

            Me.ShowDialog()
        End If
    End Sub

    Private Sub btnKeisokuten2_Click(sender As Object, e As Windows.RoutedEventArgs) Handles btnKeisokuten2.Click
        If point_name.Count > 0 Then
            Me.Hide()
            IOUtil.GetPoint(pos2, "点目を指示：")
            sys_Labeling.p2 = pos2
            'LB_P2.Text = sys_Labeling.p2.LabelName

            'C_Set_P2.Text = pos2.LabelName  '13.1.29　選択した計測点をC_Set_P2のTextに
            cmbTen2keisoku.SelectedValue = pos2.LabelName
            '<コンボボックスSelectedIndexChanged>イベントに

            Me.ShowDialog()
        End If
    End Sub

    Private Sub btnKeisokuten3_Click(sender As Object, e As Windows.RoutedEventArgs) Handles btnKeisokuten3.Click
        If point_name.Count > 0 Then
            Me.Hide()
            IOUtil.GetPoint(pos3, "点目を指示：")
            sys_Labeling.p3 = pos3
            'LB_P3.Text = sys_Labeling.p3.LabelName

            'C_Set_P3.Text = pos3.LabelName  '13.1.29　選択した計測点をC_Set_P2のTextに
            cmbTen3keisoku.SelectedValue = pos3.LabelName
            '<コンボボックスSelectedIndexChanged>イベントに

            Me.ShowDialog()
        End If
    End Sub

    Private Sub btn_3tenawase_Click(sender As Object, e As Windows.RoutedEventArgs) Handles btn_3tenawase.Click
        If checkCmbKeisokuValue() = True Then
            If checkCmbSekkeiValue() = True Then
                getSetteiDataFromControl()
                ReLabelingSyori()
            End If
        End If
    End Sub
    Private Function checkCmbKeisokuValue() As Boolean
        If cmbTen1keisoku.SelectedValue = cmbTen2keisoku.SelectedValue Or cmbTen1keisoku.SelectedValue = cmbTen3keisoku.SelectedValue Or cmbTen2keisoku.SelectedValue = cmbTen3keisoku.SelectedValue Then
            MsgBox("計測点名が重複しています。")
            checkCmbKeisokuValue = False
            Exit Function
        Else
            If cmbTen1keisoku.SelectedValue = "" Or cmbTen2keisoku.SelectedValue = "" Or cmbTen3keisoku.SelectedValue = "" Then
                MsgBox("３点位置合わせに使用する点の数が足りません。")
                checkCmbKeisokuValue = False
                Exit Function
            End If
        End If
        checkCmbKeisokuValue = True
    End Function
    Private Function checkCmbSekkeiValue() As Boolean
        If cmbTen1Sekkei.SelectedValue = cmbTen2sekkei.SelectedValue Or cmbTen1Sekkei.SelectedValue = cmbTen3Sekkei.SelectedValue Or cmbTen2sekkei.SelectedValue = cmbTen3Sekkei.SelectedValue Then
            MsgBox("設計ラべルが重複しています。")
            checkCmbSekkeiValue = False
            Exit Function
        Else
            If cmbTen1Sekkei.SelectedValue = "" Or cmbTen2sekkei.SelectedValue = "" Or cmbTen3Sekkei.SelectedValue = "" Then
                MsgBox("３点位置合わせに使用する点の数が足りません。")
                checkCmbSekkeiValue = False
                Exit Function
            End If
        End If
        checkCmbSekkeiValue = True
    End Function
    Private Sub getSetteiDataFromControl()
        controlData.Ten1Keisoku = cmbTen1keisoku.SelectedValue
        Dim CP1 As New CLookPoint
        Dim ind1 As Integer
        ind1 = GetPosFromLabelName(CLookPoint.posTypeMode.All, controlData.Ten1Keisoku, CP1)
        sys_Labeling.p1 = CP1

        controlData.Ten2Keisoku = cmbTen2keisoku.SelectedValue
        Dim CP2 As New CLookPoint
        Dim ind2 As Integer
        ind2 = GetPosFromLabelName(CLookPoint.posTypeMode.All, controlData.Ten2Keisoku, CP2)
        sys_Labeling.p2 = CP2

        controlData.Ten3Keisoku = cmbTen3keisoku.SelectedValue
        Dim CP3 As New CLookPoint
        Dim ind3 As Integer
        ind3 = GetPosFromLabelName(CLookPoint.posTypeMode.All, controlData.Ten3Keisoku, CP3)
        sys_Labeling.p3 = CP3

        controlData.Ten1Sekkei = cmbTen1Sekkei.SelectedValue
        controlData.Ten2Sekkei = cmbTen2sekkei.SelectedValue
        controlData.Ten3Sekkei = cmbTen3Sekkei.SelectedValue

        controlData.MaxX = Txt_MaxX.Text
        controlData.MaxY = Txt_MaxY.Text
        controlData.MaxZ = Txt_MaxZ.Text
        controlData.MaxL = Txt_MaxL.Text
        controlData.MinX = Txt_MinX.Text
        controlData.MinY = Txt_MinY.Text
        controlData.MinZ = Txt_MinZ.Text
        controlData.MinL = Txt_MinL.Text

        controlData.Sekkei_Label = Txt_SekkeiLabel.Text
    End Sub
    Private Function checkDuplicatedValOfKeisokuCmb() As Boolean
        Dim cmbKeisokuValList As New List(Of String)
        Dim tmpcmbKeisokuValList As New List(Of String)
        Dim duplicateVal As New List(Of String)
        Dim sekkeiLabelOfDupItem As New List(Of String)
        Dim str As String = ""

        For ii As Integer = 0 To m_ZahyoChiItems.Count - 1
            If m_ZahyoChiItems.Item(ii).SokutenMei1 <> "" Then
                cmbKeisokuValList.Add(m_ZahyoChiItems.Item(ii).SokutenMei1)
            End If
        Next
        tmpcmbKeisokuValList = cmbKeisokuValList.Distinct.ToList

        For Each item As String In cmbKeisokuValList
            If tmpcmbKeisokuValList.Contains(item) Then
                tmpcmbKeisokuValList.Remove(item)
            Else
                duplicateVal.Add(item)
            End If
        Next

        If duplicateVal.Count > 0 Then
            For Each dupItem As String In duplicateVal
                For ii As Integer = 0 To m_ZahyoChiItems.Count - 1
                    If m_ZahyoChiItems.Item(ii).SokutenMei1 = dupItem Then
                        sekkeiLabelOfDupItem.Add(m_ZahyoChiItems.Item(ii).SekkeiLabel1)
                    End If
                Next
                Exit For
            Next
            checkDuplicatedValOfKeisokuCmb = True
            For Each sekkeiItem In sekkeiLabelOfDupItem
                str = str & sekkeiItem & ","
            Next

            MsgBox("設計ラベル　" & str & "で計測点名" & duplicateVal(0) & "が重複しています。" & vbNewLine & "重複が無くなるように再設定してください。")
            Exit Function
        End If

    End Function

    Private Sub ReLabelingSyori()
        '13.1.30修正（LB_P○⇒C_Set_P○に）================================================================================================
        '3点の座標をsys_Labelingから取得する
        ReDim Xsekkei(0 To con1), Ysekkei(0 To con1), Zsekkei(0 To con1), _
           Xweight(0 To con1), Yweight(0 To con1), Zweight(0 To con1), _
           XresKeisoku(0 To con1), YresKeisoku(0 To con1), ZresKeisoku(0 To con1), _
           Xkeisoku(0 To con1), Ykeisoku(0 To con1), Zkeisoku(0 To con1)
        ReDim str_lableName(0 To con1), bln_LableUse(0 To con1)

        Xkeisoku(0) = sys_Labeling.p1.Real_x : Ykeisoku(0) = sys_Labeling.p1.Real_y : Zkeisoku(0) = sys_Labeling.p1.Real_z
        Xkeisoku(1) = sys_Labeling.p2.Real_x : Ykeisoku(1) = sys_Labeling.p2.Real_y : Zkeisoku(1) = sys_Labeling.p2.Real_z
        Xkeisoku(2) = sys_Labeling.p3.Real_x : Ykeisoku(2) = sys_Labeling.p3.Real_y : Zkeisoku(2) = sys_Labeling.p3.Real_z
        bln_LableUse(0) = True : bln_LableUse(1) = True : bln_LableUse(2) = True
        str_lableName(0) = sys_Labeling.p1.LabelName
        str_lableName(1) = sys_Labeling.p2.LabelName
        str_lableName(2) = sys_Labeling.p3.LabelName
        l_count = 0

        'グリッドから計測値を取得する
        'Dim tmpSokutenMei As New System.Collections.ObjectModel.ObservableCollection(Of String)
        For ii = 0 To nLookPoints - 1
            If gDrawPoints(ii).LabelName <> sys_Labeling.p1.LabelName And _
               gDrawPoints(ii).LabelName <> sys_Labeling.p2.LabelName And _
               gDrawPoints(ii).LabelName <> sys_Labeling.p3.LabelName Then
                Xkeisoku(3 + l_count) = gDrawPoints(ii).Real_x
                Ykeisoku(3 + l_count) = gDrawPoints(ii).Real_y
                Zkeisoku(3 + l_count) = gDrawPoints(ii).Real_z
                str_lableName(3 + l_count) = gDrawPoints(ii).LabelName
                bln_LableUse(3 + l_count) = False
                l_count = l_count + 1
            End If
        Next
        intMaxRecord = l_count + 3

        '重みを設定する

        Xweight(0) = 1.0# : Yweight(0) = 1.0# : Zweight(0) = 1.0#
        Xweight(1) = 1.0# : Yweight(1) = 1.0# : Zweight(1) = 1.0#
        Xweight(2) = 1.0# : Yweight(2) = 1.0# : Zweight(2) = 1.0#

        'グリッドから設計値を取得する

        l_count = 0

        For Each item In SekkeiDataList
            If item.S_Name = controlData.Ten1Sekkei Then
                Xsekkei(0) = item.S_X
                Ysekkei(0) = item.S_Y
                Zsekkei(0) = item.S_Z

                GoTo Nextii
            End If
            If item.S_Name = controlData.Ten2Sekkei Then
                Xsekkei(1) = item.S_X
                Ysekkei(1) = item.S_Y
                Zsekkei(1) = item.S_Z

                bln_LableUse(1) = True
                GoTo Nextii
            End If
            If item.S_Name = controlData.Ten3Sekkei Then
                Xsekkei(2) = item.S_X
                Ysekkei(2) = item.S_Y
                Zsekkei(2) = item.S_Z

                bln_LableUse(2) = True
                GoTo Nextii
            End If
            Xsekkei(3 + l_count) = item.S_X
            Ysekkei(3 + l_count) = item.S_Y
            Zsekkei(3 + l_count) = item.S_Z

            l_count = l_count + 1
Nextii:
        Next

        If intMaxRecord < l_count + 3 Then
            intMaxRecord = l_count + 3
        End If
        Dim num As Integer
        'Dim fb As Integer '20170330 baluu del
        Dim geoP_Keisoku As New GeoPoint, geoP_Sekkei As New GeoPoint, geoP_Min As New GeoPoint
        num = intMaxRecord
        '設計値により結果を計算する
        '20170330 baluu add start
        Dim hhmm As HTuple = Nothing
        correspond_3d_3d_weight(Xkeisoku, Ykeisoku, Zkeisoku, Xsekkei, Ysekkei, Zsekkei, Xweight, Yweight, Zweight, hhmm)
        HOperatorSet.AffineTransPoint3d(hhmm, Xkeisoku, Ykeisoku, Zkeisoku, XresKeisoku, YresKeisoku, ZresKeisoku)
        '20170330 baluu add end
        'fb = OmomiExcute(num, Xweight(0), Yweight(0), Zweight(0), Xsekkei(0), Ysekkei(0), Zsekkei(0), XresKeisoku(0), YresKeisoku(0), ZresKeisoku(0), Xkeisoku(0), Ykeisoku(0), Zkeisoku(0)) '20170330 baluu del


        Dim i As Integer = 0
        Dim dis_Tmp As Double = Double.MaxValue
        Dim min_dist As Double = Double.MaxValue
        With items
            .Clear()

            For Each item In SekkeiDataList
                Dim objNew As New ZahyoChiItems(List)

                If item.S_Name = controlData.Ten1Sekkei Then
                    objNew.SokutenMei1 = controlData.Ten1Keisoku
                    objNew.KeisokuTenX = XresKeisoku(0)
                    objNew.KeisokuTenY = YresKeisoku(0)
                    objNew.KeisokuTenZ = ZresKeisoku(0)

                ElseIf item.S_Name = controlData.Ten2Sekkei Then
                    objNew.SokutenMei1 = controlData.Ten2Keisoku
                    objNew.KeisokuTenX = XresKeisoku(1)
                    objNew.KeisokuTenY = YresKeisoku(1)
                    objNew.KeisokuTenZ = ZresKeisoku(1)

                ElseIf item.S_Name = controlData.Ten3Sekkei Then
                    objNew.SokutenMei1 = controlData.Ten3Keisoku
                    objNew.KeisokuTenX = XresKeisoku(2)
                    objNew.KeisokuTenY = YresKeisoku(2)
                    objNew.KeisokuTenZ = ZresKeisoku(2)

                Else
                    geoP_Sekkei.setXYZ(item.S_X, item.S_Y, item.S_Z)
                    Dim dis_index As Integer = -1
                    dis_Tmp = Double.MaxValue
                    min_dist = Double.MaxValue
                    For ii = 0 To nLookPoints - 1
                        Dim tempDrawPoint As CLookPoint = New CLookPoint '20161130 baluu add start
                        For jj = 0 To gDrawPoints.Length - 1
                            If str_lableName(ii) = gDrawPoints(jj).LabelName Then
                                tempDrawPoint = gDrawPoints(jj)

                                Exit For
                            End If
                        Next '20161130 baluu add end
                        If (tempDrawPoint.type = 2 And tempDrawPoint.flgLabel = 1) Or tempDrawPoint.type = 1 Then '20161129 baluu add start '20161130 baluu edit '20170120 baluu edit
                            Dim isTrueVal As Boolean = False
                            geoP_Keisoku.setXYZ(XresKeisoku(ii), YresKeisoku(ii), ZresKeisoku(ii))
                            dis_Tmp = geoP_Sekkei.GetDistanceTo(geoP_Keisoku)
                            If dis_Tmp < min_dist Then
                                min_dist = dis_Tmp
                                dis_index = ii
                            End If
                        End If '20161129 baluu add end
                    Next

                    If dis_index <> -1 Then
                        bln_LableUse(dis_index) = True
                        objNew.SokutenMei1 = str_lableName(dis_index)
                        objNew.KeisokuTenX = XresKeisoku(dis_index)
                        objNew.KeisokuTenY = YresKeisoku(dis_index)
                        objNew.KeisokuTenZ = ZresKeisoku(dis_index)
                    End If

                End If

                Dim diffX As Double
                Dim diffY As Double
                Dim diffZ As Double
                Dim diffL As Double
                Dim geoP_Diff As New GeoPoint
                diffX = objNew.KeisokuTenX - item.S_X
                diffY = objNew.KeisokuTenY - item.S_Y
                diffZ = objNew.KeisokuTenZ - item.S_Z
                diffL = Math.Sqrt(diffX * diffX + diffY * diffY + diffZ * diffZ)

                Dim maxX As Double = controlData.MaxX
                Dim maxY As Double = controlData.MaxY
                Dim maxZ As Double = controlData.MaxZ
                Dim maxL As Double = controlData.MaxL

                Dim minX As Double = controlData.MinX
                Dim minY As Double = controlData.MinY
                Dim minZ As Double = controlData.MinZ
                Dim minL As Double = controlData.MinL

                Dim SekkeiLabel As Double = controlData.Sekkei_Label

                If diffL < SekkeiLabel Then
                    objNew.IsMissMatch = False
                    'Else
                    objNew.DiffX = diffX
                    objNew.DiffY = diffY
                    objNew.DiffZ = diffZ
                    objNew.DiffL = diffL
                    '20161128 baluu add start
                    If objNew.SokutenMei1 = cmbTen1keisoku.SelectedValue Or
                       objNew.SokutenMei1 = cmbTen2keisoku.SelectedValue Or
                       objNew.SokutenMei1 = cmbTen3keisoku.SelectedValue Then
                        objNew.IchiAwaseXw = 1.0
                        objNew.IchiAwaseYw = 1.0
                        objNew.IchiAwaseZw = 1.0
                    Else
                        objNew.IchiAwaseXw = 0.0 '20161128 baluu edit
                        objNew.IchiAwaseYw = 0.0 '20161128 baluu edit
                        objNew.IchiAwaseZw = 0.0 '20161128 baluu edit
                    End If
                    '20161128 baluu add end

                Else
                    objNew.SokutenMei1 = ""
                    objNew.KeisokuTenX = 0.0
                    objNew.KeisokuTenY = 0.0
                    objNew.KeisokuTenZ = 0.0
                    objNew.IchiAwaseXw = 0.0
                    objNew.IchiAwaseYw = 0.0
                    objNew.IchiAwaseZw = 0.0
                    objNew.DiffX = 0.0
                    objNew.DiffY = 0.0
                    objNew.DiffZ = 0.0
                    objNew.DiffL = 0.0

                    objNew.IsOverDiff = True
                    objNew.IsMissMatch = True
                End If

                If minX < diffX And diffX < maxX And minY < diffY And diffY < maxY And minZ < diffZ And diffZ < maxZ And minL < diffL And diffL < maxL Then
                    objNew.IsOverDiff = False
                Else
                    objNew.IsOverDiff = True
                End If

                Try
                    objNew.No = item.S_ID
                    objNew.SekkeiLabel1 = item.S_Name
                    objNew.SekkeiTenX = item.S_X
                    objNew.SekkeiTenY = item.S_Y
                    objNew.SekkeiTenZ = item.S_Z
                    'objNew.DiffX = diffX
                    'objNew.DiffY = diffY
                    'objNew.DiffZ = diffZ
                    'objNew.DiffL = diffL
                    'objNew.IchiAwaseXw = 1.0
                    'objNew.IchiAwaseYw = 1.0
                    'objNew.IchiAwaseZw = 1.0

                Catch ex As Exception
                    MsgBox("値設定エラー", MsgBoxStyle.OkOnly)
                    Exit Sub
                End Try

                .Add(objNew)
                i = i + 1
            Next

        End With

        With m_ZahyoChiItems
            .Clear()
            For ii As Integer = 0 To items.Count - 1

                items(ii).SekkeiTenX = items(ii).SekkeiTenX.ToString("N2")
                items(ii).SekkeiTenY = items(ii).SekkeiTenY.ToString("N2")
                items(ii).SekkeiTenZ = items(ii).SekkeiTenZ.ToString("N2")

                items(ii).KeisokuTenX = items(ii).KeisokuTenX.ToString("N2")
                items(ii).KeisokuTenY = items(ii).KeisokuTenY.ToString("N2")
                items(ii).KeisokuTenZ = items(ii).KeisokuTenZ.ToString("N2")

                items(ii).DiffX = items(ii).DiffX.ToString("N2")
                items(ii).DiffY = items(ii).DiffY.ToString("N2")
                items(ii).DiffZ = items(ii).DiffZ.ToString("N2")
                items(ii).DiffL = items(ii).DiffL.ToString("N2")
                .Add(items(ii))
            Next
        End With

        dataGridZahyo.ItemsSource = m_ZahyoChiItems
        calcAverage()
        calcValues()

    End Sub

    Private Sub calcAverage()
        Dim sumX As Double = 0.0
        Dim sumY As Double = 0.0
        Dim sumZ As Double = 0.0

        If m_ZahyoChiItems.Count = 0 Then
            Exit Sub
        End If
        For ii As Integer = 0 To m_ZahyoChiItems.Count - 1
            sumX = sumX + m_ZahyoChiItems(ii).DiffX
            sumY = sumY + m_ZahyoChiItems(ii).DiffY
            sumZ = sumZ + m_ZahyoChiItems(ii).DiffZ
        Next

        Dim averageX As Double = sumX / m_ZahyoChiItems.Count
        Dim averageY As Double = sumY / m_ZahyoChiItems.Count
        Dim averageZ As Double = sumZ / m_ZahyoChiItems.Count
        '20170209 baluu edit start
        Txt_HeikinX.Text = averageX.ToString("N1") 'N2 -> N1
        Txt_HeikinY.Text = averageY.ToString("N1") 'N2 -> N1
        Txt_HeikinZ.Text = averageZ.ToString("N1") 'N2 -> N1
        Txt_HeikinL.Text = Math.Sqrt(averageX * averageX + averageY * averageY + averageZ * averageZ).ToString("N1") 'N2 -> N1
        '20170209 baluu edit end
    End Sub
    Private Sub calcValues()
        Dim largest As Double = Integer.MaxValue
        Dim smallest As Double = Integer.MinValue
        Dim StDevX As New List(Of Double)
        Dim StDevXAvg As Double
        Dim StDevY As New List(Of Double)
        Dim StDevYAvg As Double
        Dim StDevZ As New List(Of Double)
        Dim StDevZAvg As Double
        Dim listX As New List(Of Double)
        Dim listY As New List(Of Double)
        Dim listZ As New List(Of Double)
        If m_ZahyoChiItems.Count <= 0 Then
            Exit Sub
        End If
        For ii As Integer = 0 To m_ZahyoChiItems.Count - 1
            listX.Add(m_ZahyoChiItems(ii).DiffX)
            listY.Add(m_ZahyoChiItems(ii).DiffY)
            listZ.Add(m_ZahyoChiItems(ii).DiffZ)

            StDevX.Add(Math.Pow(m_ZahyoChiItems(ii).DiffX - Txt_HeikinX.Text, 2))
            StDevY.Add(Math.Pow(m_ZahyoChiItems(ii).DiffY - Txt_HeikinY.Text, 2))
            StDevZ.Add(Math.Pow(m_ZahyoChiItems(ii).DiffZ - Txt_HeikinZ.Text, 2))
        Next
        '20170209 baluu edit start
        Txt_MaxX_Omomi.Text = listX.Max.ToString("N1") 'toString("N1")
        Txt_MaxY_Omomi.Text = listY.Max.ToString("N1") 'toString("N1")
        Txt_MaxZ_Omomi.Text = listZ.Max.ToString("N1") 'toString("N1")

        Txt_MinX_Omomi.Text = listX.Min.ToString("N1") 'toString("N1")
        Txt_MinY_Omomi.Text = listY.Min.ToString("N1") 'toString("N1")
        Txt_MinZ_Omomi.Text = listZ.Min.ToString("N1") 'toString("N1")

        Txt_MaxL_Omomi.Text = Math.Sqrt(listX.Max * listX.Max + listY.Max * listY.Max + listZ.Max * listZ.Max).ToString("N1") 'N2 -> N1
        Txt_MinL_Omomi.Text = Math.Sqrt(listX.Min * listX.Min + listY.Min * listY.Min + listZ.Min * listZ.Min).ToString("N1") 'N2 -> N1

        StDevXAvg = StDevX.Average()
        Txt_hyojyunX.Text = Math.Sqrt(StDevXAvg).ToString("N1") 'N2 -> N1

        StDevYAvg = StDevY.Average()
        Txt_hyojyunY.Text = Math.Sqrt(StDevYAvg).ToString("N1") 'N2 -> N1

        StDevZAvg = StDevZ.Average()
        Txt_hyojyunZ.Text = Math.Sqrt(StDevZAvg).ToString("N1") 'N2 -> N1

        Txt_hyojyunL.Text = Math.Sqrt(Txt_hyojyunX.Text * Txt_hyojyunX.Text + Txt_hyojyunY.Text * Txt_hyojyunY.Text + Txt_hyojyunZ.Text * Txt_hyojyunZ.Text).ToString("N1") 'N2 -> N1
        '20170209 baluu edit end
    End Sub
    Private Sub Button_Click(sender As Object, e As Windows.RoutedEventArgs)
        Dim Zitems As ZahyoChiItems = dataGridZahyo.SelectedItem
        Dim back_X As Double = Zitems.IchiAwaseXw
        Zitems.IchiAwaseXw = Math.Round(back_X - 0.1, 1) '20161129 baluu edit
        If Zitems.IchiAwaseXw < 0 Then
            Zitems.IchiAwaseXw = 0
        End If
        dataGridZahyo.ItemsSource = m_ZahyoChiItems
        dataGridZahyo.Items.Refresh()
    End Sub

    Private Sub Button_Click_1(sender As Object, e As Windows.RoutedEventArgs)
        Dim Zitems As ZahyoChiItems = dataGridZahyo.SelectedItem
        Dim next_X As Double = Zitems.IchiAwaseXw
        Zitems.IchiAwaseXw = next_X + 0.1
        dataGridZahyo.ItemsSource = m_ZahyoChiItems
        dataGridZahyo.Items.Refresh()
    End Sub

    Private Sub Button_Click_2(sender As Object, e As Windows.RoutedEventArgs)
        Dim Zitems As ZahyoChiItems = dataGridZahyo.SelectedItem
        Dim back_Y As Double = Zitems.IchiAwaseYw
        Zitems.IchiAwaseYw = Math.Round(back_Y - 0.1, 1) '20161129 baluu edit
        If Zitems.IchiAwaseYw < 0 Then
            Zitems.IchiAwaseYw = 0
        End If
        dataGridZahyo.ItemsSource = m_ZahyoChiItems
        dataGridZahyo.Items.Refresh()
    End Sub

    Private Sub Button_Click_3(sender As Object, e As Windows.RoutedEventArgs)
        Dim Zitems As ZahyoChiItems = dataGridZahyo.SelectedItem
        Dim next_Y As Double = Zitems.IchiAwaseYw
        Zitems.IchiAwaseYw = next_Y + 0.1
        dataGridZahyo.ItemsSource = m_ZahyoChiItems
        dataGridZahyo.Items.Refresh()
    End Sub

    Private Sub Button_Click_4(sender As Object, e As Windows.RoutedEventArgs)
        Dim Zitems As ZahyoChiItems = dataGridZahyo.SelectedItem
        Dim back_Z As Double = Zitems.IchiAwaseZw
        Zitems.IchiAwaseZw = Math.Round(back_Z - 0.1, 1) '20161129 baluu edit
        If Zitems.IchiAwaseZw < 0 Then
            Zitems.IchiAwaseZw = 0
        End If
        dataGridZahyo.ItemsSource = m_ZahyoChiItems
        dataGridZahyo.Items.Refresh()
    End Sub

    Private Sub Button_Click_5(sender As Object, e As Windows.RoutedEventArgs)
        Dim Zitems As ZahyoChiItems = dataGridZahyo.SelectedItem
        Dim next_Z As Double = Zitems.IchiAwaseZw
        Zitems.IchiAwaseZw = next_Z + 0.1
        dataGridZahyo.ItemsSource = m_ZahyoChiItems
        dataGridZahyo.Items.Refresh()
    End Sub
    Private Sub Button_KeisokuCmb_Click(sender As Object, e As Windows.RoutedEventArgs)
        If point_name.Count > 0 Then
            Me.Hide()
            Dim pos As New CLookPoint
            IOUtil.GetPoint(pos, "点目を指示：")

            Dim Zitems As ZahyoChiItems = dataGridZahyo.SelectedItem
            Zitems.SokutenMei1 = pos.LabelName
            dataGridZahyo.Items.Refresh()

            Me.ShowDialog()
        End If

    End Sub
    Private Sub btn_End_Click(sender As Object, e As Windows.RoutedEventArgs) Handles btn_End.Click
        Me.Close()
    End Sub

    Private Sub btn_OmomiAwase_Click(sender As Object, e As Windows.RoutedEventArgs) Handles btn_OmomiAwase.Click
        If checkDuplicatedValOfKeisokuCmb() = False Then
            getSetteiDataFromControl()
            ReOmomiAwaseSyori()
        End If
    End Sub
    Private Sub ReOmomiAwaseSyori()
        ReDim Xsekkei(0 To con1), Ysekkei(0 To con1), Zsekkei(0 To con1), _
             Xweight(0 To con1), Yweight(0 To con1), Zweight(0 To con1), _
             XresKeisoku(0 To con1), YresKeisoku(0 To con1), ZresKeisoku(0 To con1), _
             Xkeisoku(0 To con1), Ykeisoku(0 To con1), Zkeisoku(0 To con1)
        ReDim str_lableName(0 To con1), bln_LableUse(0 To con1)

        Dim ik As Integer = 0
        For ii As Integer = 0 To m_ZahyoChiItems.Count - 1
            If m_ZahyoChiItems.Item(ii).SokutenMei1 <> "" Then
                Dim CLP1 As New CLookPoint
                Dim ind1 As Integer
                ind1 = GetPosFromLabelName(CLookPoint.posTypeMode.All, m_ZahyoChiItems.Item(ii).SokutenMei1, CLP1)
                Xkeisoku(ik) = CLP1.Real_x
                Ykeisoku(ik) = CLP1.Real_y
                Zkeisoku(ik) = CLP1.Real_z

                Xweight(ik) = m_ZahyoChiItems.Item(ii).IchiAwaseXw
                Yweight(ik) = m_ZahyoChiItems.Item(ii).IchiAwaseYw
                Zweight(ik) = m_ZahyoChiItems.Item(ii).IchiAwaseZw

                Xsekkei(ik) = m_ZahyoChiItems.Item(ii).SekkeiTenX
                Ysekkei(ik) = m_ZahyoChiItems.Item(ii).SekkeiTenY
                Zsekkei(ik) = m_ZahyoChiItems.Item(ii).SekkeiTenZ

                bln_LableUse(ik) = True
                str_lableName(ik) = CLP1.LabelName
                ik = ik + 1
            End If
        Next

        Dim ij As Integer
        For ii = 0 To nLookPoints - 1
            Dim blnIsAru As Boolean = False
            For ij = 0 To m_ZahyoChiItems.Count - 1
                If m_ZahyoChiItems.Item(ij).SokutenMei1 = gDrawPoints(ii).LabelName Then
                    blnIsAru = True
                    Exit For
                End If
            Next ij
            If blnIsAru = False Then
                Xkeisoku(ik) = gDrawPoints(ii).Real_x
                Ykeisoku(ik) = gDrawPoints(ii).Real_y
                Zkeisoku(ik) = gDrawPoints(ii).Real_z

                Xweight(ik) = 0.0
                Yweight(ik) = 0.0
                Zweight(ik) = 0.0

                bln_LableUse(ik) = False
                str_lableName(ik) = gDrawPoints(ii).LabelName
                ik = ik + 1
            End If
        Next
        intMaxRecord = ik

        Dim num As Integer
        'Dim fb As Integer '20170330 baluu del
        Dim geoP_Keisoku As New GeoPoint, geoP_Sekkei As New GeoPoint, geoP_Min As New GeoPoint
        num = intMaxRecord
        '設計値により結果を計算する
        Dim hhmm As HTuple = Nothing
        correspond_3d_3d_weight(Xkeisoku, Ykeisoku, Zkeisoku, Xsekkei, Ysekkei, Zsekkei, Xweight, Yweight, Zweight, hhmm)
        HOperatorSet.AffineTransPoint3d(hhmm, Xkeisoku, Ykeisoku, Zkeisoku, XresKeisoku, YresKeisoku, ZresKeisoku)
        'fb = OmomiExcute(num, Xweight(0), Yweight(0), Zweight(0), Xsekkei(0), Ysekkei(0), Zsekkei(0), XresKeisoku(0), YresKeisoku(0), ZresKeisoku(0), Xkeisoku(0), Ykeisoku(0), Zkeisoku(0)) '20170330 baluu del


        For ii As Integer = 0 To m_ZahyoChiItems.Count - 1

        Next
        Dim dis_Tmp As Double
        Dim min_dist As Double

        For ii As Integer = 0 To m_ZahyoChiItems.Count - 1
            Dim objReNew As ZahyoChiItems = m_ZahyoChiItems.Item(ii)

            geoP_Sekkei.setXYZ(objReNew.SekkeiTenX, objReNew.SekkeiTenY, objReNew.SekkeiTenZ)
            Dim dis_index As Integer = -1
            min_dist = Double.MaxValue
            For ij = 0 To num - 1
                Dim isTrueVal As Boolean = False
                geoP_Keisoku.setXYZ(XresKeisoku(ii), YresKeisoku(ii), ZresKeisoku(ii))
                dis_Tmp = geoP_Sekkei.GetDistanceTo(geoP_Keisoku)
                If dis_Tmp < min_dist Then
                    min_dist = dis_Tmp
                    dis_index = ii
                End If
            Next
            Dim SekkeiLabel As Double = controlData.Sekkei_Label
            If min_dist < SekkeiLabel Then
                bln_LableUse(dis_index) = True
                objReNew.SokutenMei1 = str_lableName(dis_index)
                objReNew.KeisokuTenX = XresKeisoku(dis_index)
                objReNew.KeisokuTenY = YresKeisoku(dis_index)
                objReNew.KeisokuTenZ = ZresKeisoku(dis_index)
            End If

            Dim diffX As Double
            Dim diffY As Double
            Dim diffZ As Double
            Dim diffL As Double
            Dim geoP_Diff As New GeoPoint
            diffX = objReNew.KeisokuTenX - objReNew.SekkeiTenX
            diffY = objReNew.KeisokuTenY - objReNew.SekkeiTenY
            diffZ = objReNew.KeisokuTenZ - objReNew.SekkeiTenZ
            diffL = Math.Sqrt(diffX * diffX + diffY * diffY + diffZ * diffZ)

            Dim maxX As Double = controlData.MaxX
            Dim maxY As Double = controlData.MaxY
            Dim maxZ As Double = controlData.MaxZ
            Dim maxL As Double = controlData.MaxL

            Dim minX As Double = controlData.MinX
            Dim minY As Double = controlData.MinY
            Dim minZ As Double = controlData.MinZ
            Dim minL As Double = controlData.MinL



            If diffL < SekkeiLabel Then
                objReNew.IsMissMatch = False
                'Else

            Else
                objReNew.SokutenMei1 = ""
                objReNew.KeisokuTenX = 0.0
                objReNew.KeisokuTenY = 0.0
                objReNew.KeisokuTenZ = 0.0
                objReNew.IsOverDiff = True
                objReNew.IsMissMatch = True
            End If

            If minX < diffX And diffX < maxX And minY < diffY And diffY < maxY And minZ < diffZ And diffZ < maxZ And minL < diffL And diffL < maxL Then
                objReNew.IsOverDiff = False
            Else
                objReNew.IsOverDiff = True
            End If

            Try
                objReNew.DiffX = diffX
                objReNew.DiffY = diffY
                objReNew.DiffZ = diffZ
                objReNew.DiffL = diffL
            Catch ex As Exception
                MsgBox("値設定エラー", MsgBoxStyle.OkOnly)
                Exit Sub
            End Try



            objReNew.SekkeiTenX = objReNew.SekkeiTenX.ToString("N2")
            objReNew.SekkeiTenY = objReNew.SekkeiTenY.ToString("N2")
            objReNew.SekkeiTenZ = objReNew.SekkeiTenZ.ToString("N2")

            objReNew.KeisokuTenX = objReNew.KeisokuTenX.ToString("N2")
            objReNew.KeisokuTenY = objReNew.KeisokuTenY.ToString("N2")
            objReNew.KeisokuTenZ = objReNew.KeisokuTenZ.ToString("N2")



            objReNew.IchiAwaseXw = objReNew.IchiAwaseXw.ToString("N2")
            objReNew.IchiAwaseYw = objReNew.IchiAwaseYw.ToString("N2")
            objReNew.IchiAwaseZw = objReNew.IchiAwaseZw.ToString("N2")

            objReNew.DiffX = objReNew.DiffX.ToString("N2")
            objReNew.DiffY = objReNew.DiffY.ToString("N2")
            objReNew.DiffZ = objReNew.DiffZ.ToString("N2")
            objReNew.DiffL = objReNew.DiffL.ToString("N2")

            m_ZahyoChiItems.Item(ii) = objReNew

        Next
        dataGridZahyo.ItemsSource = m_ZahyoChiItems
        dataGridZahyo.Items.Refresh()

    End Sub
    Private Sub Btn_OK_Click(sender As Object, e As Windows.RoutedEventArgs) Handles Btn_OK.Click

        If checkDuplicatedValOfKeisokuCmb() = True Then
            Exit Sub
        End If

        nSekPoints = 0    '3Dview 設計点表示用
        'ReDim gCurrUserMovePoints(m_ZahyoChiItems.Count - 1) '20170221 baluu del

        ReDim gDrawSekPoints(m_ZahyoChiItems.Count - 1)
        Dim tmpscale As Double = sys_ScaleInfo.scale
        If IO.File.Exists(MainFrm.objFBM.ProjectPath & "\" & FBMlib.Common.YCM_TEMP_MDB) = True Then
            ' YCM_ReadSystemscalesettingAcs(MainFrm.objFBM.ProjectPath & "\" & FBMlib.Common.YCM_TEMP_MDB)
            YCM_ReadSystemscalesettingAcsvalue(MainFrm.objFBM.ProjectPath & "\" & FBMlib.Common.YCM_TEMP_MDB)
            ' YCM_ReadSystemscalesettingAcsP1_2(MainFrm.objFBM.ProjectPath & "\" & FBMlib.Common.YCM_TEMP_MDB) '13.1
            If nscalesettingvalue > 0 Then
                tmpscale = System_scalesettingvalue(0, nscalesettingvalue - 1)
            End If
        End If


        Dim arrupdaatlabel As New List(Of String)
        Dim arrupdatalabelnew As New List(Of String)
        For Each ZCI As ZahyoChiItems In m_ZahyoChiItems
            'If ZCI.SokutenMei1 <> "" Then 20170217 baluu del
            For jj As Integer = 0 To nLookPoints - 1
                If Data_Point.DGV_DV.Rows(jj).Cells(1).Value = ZCI.SokutenMei1 Then

                End If

                '20170220 baluu add start
                If gDrawPoints(jj).tid = ZCI.SokutenID And gDrawPoints(jj).flgLabel = 1 And ZCI.isRenamed = True Then
                    arrupdaatlabel.Add(gDrawPoints(jj).LabelName)
                    arrupdatalabelnew.Add(ZCI.SokutenMei1)
                    gDrawPoints(jj).LabelName = ZCI.SokutenMei1
                    '20170220 baluu add end

                ElseIf gDrawPoints(jj).LabelName = ZCI.SokutenMei1 Then

                    arrupdaatlabel.Add(ZCI.SokutenMei1)
                    arrupdatalabelnew.Add(ZCI.SekkeiLabel1)
                    Data_Point.DGV_DV.Rows(jj).Cells(1).Value = ZCI.SekkeiLabel1


                    gDrawPoints(jj).LabelName = ZCI.SekkeiLabel1
                    ZCI.SokutenMei1 = ZCI.SekkeiLabel1
                    'gDrawLabelText(jj).LabelName = data_point_result.Rows(ii).Cells(1).Value


                    'Data_Point.DGV_DV.Rows(jj).Cells(1).Value = data_point_result.Rows(ii).Cells(1).Value
                End If


            Next


            Dim objNewLookPoint As New CLookPoint(ZCI, tmpscale)
            objNewLookPoint.createType = 2 '20170217 baluu add
            gDrawSekPoints(nSekPoints) = objNewLookPoint
            'gCurrUserMovePoints(nSekPoints) = objNewLookPoint '20170221 baluu del
            nSekPoints = nSekPoints + 1
            'nCurrUserMovePoints = nSekPoints '20170221 baluu del
            'End If '20170217 baluu del 
        Next

        For ii As Integer = 0 To nLookPoints - 1
            For jj As Integer = 0 To nLabelText - 1
                If gDrawPoints(ii).mid = gDrawLabelText(jj).mid Then
                    gDrawLabelText(jj).LabelName = gDrawPoints(ii).LabelName
                End If
            Next
        Next
        YCM_Updatalabelresult(m_KeisokuTempMdbPath, arrupdaatlabel, arrupdatalabelnew)
        MainFrm.SetCurrentLabelTo_objFBM()

        Dim dbo As New CDBOperate

        If dbo.ConnectDB(m_KeisokuTempMdbPath) = False Then
            MsgBox("データベースを開くことができません。")
            Exit Sub
        End If

        If (dbo Is Nothing) Then Exit Sub

        Dim strSQL As String

        YCM_AddNewSekkeiKeisokuSetteiTable(dbo)
        ' YCM_ReadSekkeiKeisokuSetteiTable(m_KeisokuTempMdbPath)

        'If setteiTableFlg = 0 Then
        strSQL = "DELETE FROM SekkeiKeisokuSettei WHERE KeisokuSet = '" & strCurrentKeisokuSet & "'"
        dbo.ExcuteSQL(strSQL)

        strSQL = "INSERT INTO SekkeiKeisokuSettei " & _
                 " (KeisokuSet,FileName,Moji,Ten1Keisoku,Ten2Keisoku,Ten3Keisoku," & _
                 " Ten1Sekkei,Ten2Sekkei,Ten3Sekkei)" & _
                 " VALUES ("

        strSQL = strSQL + "'" & strCurrentKeisokuSet & "'"
        strSQL = strSQL + ",'" & Txt_SekkeiFile.Text & "'"
        strSQL = strSQL + ",'" & LabelingMoji.Text & "'"

        strSQL = strSQL + ",'" & cmbTen1keisoku.SelectedValue & "'"
        strSQL = strSQL + ",'" & cmbTen2keisoku.SelectedValue & "'"
        strSQL = strSQL + ",'" & cmbTen3keisoku.SelectedValue & "'"

        strSQL = strSQL + ",'" & cmbTen1Sekkei.SelectedValue & "'"
        strSQL = strSQL + ",'" & cmbTen2sekkei.SelectedValue & "'"
        strSQL = strSQL + ",'" & cmbTen3Sekkei.SelectedValue & "');"
        dbo.ExcuteSQL(strSQL)
        'Else
        '    Dim strSQL1 As String = "UPDATE SekkeiKeisokuSettei " & _
        '                  " set FileName = '" & Txt_SekkeiFile.Text & _
        '                   " ,Moji = '" & LabelingMoji.Text & _
        '                    ",Ten1Keisoku = '" & cmbTen1keisoku.SelectedValue & _
        '                     " , Ten2Keisoku = '" & cmbTen2keisoku.SelectedValue & _
        '                      " ,Ten3Keisoku = '" & cmbTen3keisoku.SelectedValue & _
        '                       " , Ten1Sekkei = '" & cmbTen1Sekkei.SelectedValue & _
        '                        " , Ten2Sekkei = '" & cmbTen2sekkei.SelectedValue & _
        '                        " , Ten3Sekkei = '" & cmbTen3Sekkei.SelectedValue & ";"
        '    dbo.ExcuteSQL(strSQL1)
        'End If

        YCM_AddNewSekkeiKeisokuDataTable(dbo)
        'YCM_ReadSekkeiKeisokuDataTable(m_KeisokuTempMdbPath)

        'If dataTableFlg = 0 Then
        strSQL = "DELETE FROM SekkeiKeisokuData WHERE KeisokuSet='" & strCurrentKeisokuSet & "'"
        dbo.ExcuteSQL(strSQL)
        For ii As Integer = 0 To m_ZahyoChiItems.Count - 1
            strSQL = "INSERT INTO SekkeiKeisokuData " & _
               " (ID,KeisokuSet,Sekkei,SokutenMei,SokutenID, SekkeiTenX,SekkeiTenY,SekkeiTenZ," & _
               " KeisokuTenX,KeisokuTenY,KeisokuTenZ," & _
             " IchiAwaseXw,IchiAwaseYw,IchiAwaseZw," & _
             " DiffX,DiffY,DiffZ,DiffL)" & _
               " VALUES ("

            strSQL = strSQL + "'" & m_ZahyoChiItems.Item(ii).No & "'"
            strSQL = strSQL + ",'" & strCurrentKeisokuSet & "'"
            strSQL = strSQL + ",'" & m_ZahyoChiItems.Item(ii).SekkeiLabel1 & "'"
            strSQL = strSQL + ",'" & m_ZahyoChiItems.Item(ii).SokutenMei1 & "'"
            strSQL = strSQL + ",'" & m_ZahyoChiItems.Item(ii).SokutenID & "'"
            strSQL = strSQL + ",'" & m_ZahyoChiItems.Item(ii).SekkeiTenX & "'"
            strSQL = strSQL + ",'" & m_ZahyoChiItems.Item(ii).SekkeiTenY & "'"
            strSQL = strSQL + ",'" & m_ZahyoChiItems.Item(ii).SekkeiTenZ & "'"

            strSQL = strSQL + ",'" & m_ZahyoChiItems.Item(ii).KeisokuTenX & "'"
            strSQL = strSQL + ",'" & m_ZahyoChiItems.Item(ii).KeisokuTenY & "'"
            strSQL = strSQL + ",'" & m_ZahyoChiItems.Item(ii).KeisokuTenZ & "'"

            strSQL = strSQL + ",'" & m_ZahyoChiItems.Item(ii).IchiAwaseXw & "'"
            strSQL = strSQL + ",'" & m_ZahyoChiItems.Item(ii).IchiAwaseYw & "'"
            strSQL = strSQL + ",'" & m_ZahyoChiItems.Item(ii).IchiAwaseZw & "'"

            strSQL = strSQL + ",'" & m_ZahyoChiItems.Item(ii).DiffX & "'"
            strSQL = strSQL + ",'" & m_ZahyoChiItems.Item(ii).DiffY & "'"
            strSQL = strSQL + ",'" & m_ZahyoChiItems.Item(ii).DiffZ & "'"
            strSQL = strSQL + ",'" & m_ZahyoChiItems.Item(ii).DiffL & "');"
            dbo.ExcuteSQL(strSQL)
        Next
        'Else
        '    For ii As Integer = 0 To m_ZahyoChiItems.Count - 1
        '        Dim strSQL2 As String = "UPDATE SekkeiKeisokuData " & _
        '                 " set ID = '" & m_ZahyoChiItems.Item(ii).No & _
        '                  " ,Sekkei = '" & m_ZahyoChiItems.Item(ii).SekkeiLabel1 & _
        '                   ",SokutenMei = '" & m_ZahyoChiItems.Item(ii).SokutenMei1 & _
        '                    " , SekkeiTenX = '" & m_ZahyoChiItems.Item(ii).SekkeiTenX & _
        '                     " ,SekkeiTenY = '" & m_ZahyoChiItems.Item(ii).SekkeiTenY & _
        '                      " , SekkeiTenZ = '" & m_ZahyoChiItems.Item(ii).SekkeiTenZ & _
        '                       " , KeisokuTenX = '" & m_ZahyoChiItems.Item(ii).KeisokuTenX & _
        '                    " , KeisokuTenY = '" & m_ZahyoChiItems.Item(ii).KeisokuTenY & _
        '                     " ,KeisokuTenZ = '" & m_ZahyoChiItems.Item(ii).KeisokuTenZ & _
        '                      " , IchiAwaseXw = '" & m_ZahyoChiItems.Item(ii).IchiAwaseXw & _
        '                       " , IchiAwaseYw = '" & m_ZahyoChiItems.Item(ii).IchiAwaseYw & _
        '                         " , IchiAwaseZw = '" & m_ZahyoChiItems.Item(ii).IchiAwaseZw & _
        '                       " , DiffX = '" & m_ZahyoChiItems.Item(ii).DiffX & _
        '                          " , DiffY = '" & m_ZahyoChiItems.Item(ii).DiffY & _
        '                       " , DiffZ = '" & m_ZahyoChiItems.Item(ii).DiffZ & _
        '                       " , DiffL = '" & m_ZahyoChiItems.Item(ii).DiffL & ";"

        '        dbo.ExcuteSQL(strSQL2)
        '    Next
        'End If

        ' 20161109 baluu ADD start
        YCM_ReadSekkeiKeisokuDataTable1(MainFrm.objFBM.ProjectPath & "\" & "計測データ.mdb")

        For Each C3DCT As FBMlib.Common3DCodedTarget In MainFrm.objFBM.lstCommon3dCT
            If C3DCT.flgUsable = True Then
#If True Then
                arrCTData(C3DCT.PID).CtDatToSekDat(GetSekkeiKeisokuTaiyoByID(C3DCT.PID))
#End If
            End If
        Next
        ' 20161109 baluu ADD end

        dbo.DisConnectDB()

        CalcSunpo()

        Me.Close()

    End Sub


    ' 20161109 baluu ADD start

    Public lstSek_KeiData As List(Of clsSekkeiKeisokuTaiyo)

    Private Function GetSekkeiKeisokuTaiyoByID(ByVal IID As String) As clsSekkeiKeisokuTaiyo
        GetSekkeiKeisokuTaiyoByID = Nothing
        If lstSek_KeiData Is Nothing Then
            Exit Function
        End If
        For Each SKTI As clsSekkeiKeisokuTaiyo In lstSek_KeiData
            If SKTI.SokutenID = IID Then
                GetSekkeiKeisokuTaiyoByID = SKTI
                Exit Function
            End If
        Next
    End Function


    Private Function YCM_ReadSekkeiKeisokuDataTable1(ByVal strDBPath As String) As Integer
        Dim clsOPe As New CDBOperate
        Dim adoRet As ADODB.Recordset
        If clsOPe.ConnectDB(strDBPath) = False Then
            YCM_ReadSekkeiKeisokuDataTable1 = -1
        End If
        Dim strSQL As String

        If (Not ExistsTable(clsOPe, "[SekkeiKeisokuData]")) Then
            Exit Function
        End If
        If lstSek_KeiData Is Nothing Then
            lstSek_KeiData = New List(Of clsSekkeiKeisokuTaiyo)
        Else
            lstSek_KeiData.Clear()
        End If


        strSQL = "SELECT * From SekkeiKeisokuData"
        adoRet = clsOPe.CreateRecordset(strSQL)
        If adoRet Is Nothing Then
            Exit Function
        End If
        If adoRet.RecordCount > 0 Then
            Do Until adoRet.EOF
                With lstSek_KeiData
                    Dim objNew As New clsSekkeiKeisokuTaiyo
                    objNew.No = adoRet("ID").Value
                    objNew.SekkeiLabel1 = adoRet("Sekkei").Value
                    objNew.SokutenMei1 = adoRet("SokutenMei").Value
                    objNew.SekkeiTenX = adoRet("SekkeiTenX").Value
                    objNew.SekkeiTenY = adoRet("SekkeiTenY").Value
                    objNew.SekkeiTenZ = adoRet("SekkeiTenZ").Value
                    objNew.KeisokuTenX = adoRet("KeisokuTenX").Value
                    objNew.KeisokuTenY = adoRet("KeisokuTenY").Value
                    objNew.KeisokuTenZ = adoRet("KeisokuTenZ").Value
                    objNew.IchiAwaseXw = adoRet("IchiAwaseXw").Value
                    objNew.IchiAwaseYw = adoRet("IchiAwaseYw").Value
                    objNew.IchiAwaseZw = adoRet("IchiAwaseZw").Value
                    objNew.DiffX = adoRet("DiffX").Value
                    objNew.DiffY = adoRet("DiffY").Value
                    objNew.DiffZ = adoRet("DiffZ").Value
                    objNew.DiffL = adoRet("DiffL").Value
                    objNew.SokutenID = adoRet("SokutenID").Value

                    .Add(objNew)
                End With
                adoRet.MoveNext()
            Loop

        End If
        clsOPe.DisConnectDB()
        YCM_ReadSekkeiKeisokuDataTable1 = 0


    End Function

    ' 20161109 baluu ADD end

    Private Function YCM_AddNewSekkeiKeisokuSetteiTable(ByVal clsOPe As CDBOperate) As Integer
        YCM_AddNewSekkeiKeisokuSetteiTable = -1
        Dim strSQL1 As String

        If (Not ExistsTable(clsOPe, "[SekkeiKeisokuSettei]")) Then
            strSQL1 = "CREATE TABLE " & "SekkeiKeisokuSettei" & " ("
            strSQL1 = strSQL1 & "KeisokuSet     VARCHAR(250),"
            strSQL1 = strSQL1 & "FileName       VARCHAR(250),"
            strSQL1 = strSQL1 & "Moji           VARCHAR(250),"
            strSQL1 = strSQL1 & "Ten1Keisoku  VARCHAR(250),"
            strSQL1 = strSQL1 & "Ten2Keisoku  VARCHAR(250),"
            strSQL1 = strSQL1 & "Ten3Keisoku  VARCHAR(250),"

            strSQL1 = strSQL1 & "Ten1Sekkei   VARCHAR(250),"
            strSQL1 = strSQL1 & "Ten2Sekkei   VARCHAR(250),"
            strSQL1 = strSQL1 & "Ten3Sekkei    VARCHAR(250)"

            strSQL1 = strSQL1 & ")"
            Call clsOPe.ExcuteSQL(strSQL1)
        End If
        YCM_AddNewSekkeiKeisokuSetteiTable = 0
    End Function

    Private Function YCM_AddNewSekkeiKeisokuDataTable(ByVal clsOPe1 As CDBOperate) As Integer
        YCM_AddNewSekkeiKeisokuDataTable = -1
        Dim strSQL As String

        If (Not ExistsTable(clsOPe1, "[SekkeiKeisokuData]")) Then
            strSQL = "CREATE TABLE " & "SekkeiKeisokuData" & " ("
            strSQL = strSQL & "ID       LONG,"
            strSQL = strSQL & "KeisokuSet   VARCHAR(250),"
            strSQL = strSQL & "Sekkei   VARCHAR(250),"
            strSQL = strSQL & "SokutenMei  VARCHAR(250),"
            strSQL = strSQL & "SekkeiTenX  DOUBLE PRECISION,"
            strSQL = strSQL & "SekkeiTenY  DOUBLE PRECISION,"
            strSQL = strSQL & "SekkeiTenZ  DOUBLE PRECISION,"
            strSQL = strSQL & "KeisokuTenX  DOUBLE PRECISION,"
            strSQL = strSQL & "KeisokuTenY  DOUBLE PRECISION,"
            strSQL = strSQL & "KeisokuTenZ  DOUBLE PRECISION,"
            strSQL = strSQL & "IchiAwaseXw   DOUBLE PRECISION,"
            strSQL = strSQL & "IchiAwaseYw   DOUBLE PRECISION,"
            strSQL = strSQL & "IchiAwaseZw   DOUBLE PRECISION,"
            strSQL = strSQL & "DiffX   DOUBLE PRECISION,"
            strSQL = strSQL & "DiffY   DOUBLE PRECISION,"
            strSQL = strSQL & "DiffZ   DOUBLE PRECISION,"
            strSQL = strSQL & "DiffL   DOUBLE PRECISION"

            strSQL = strSQL & ")"
            Call clsOPe1.ExcuteSQL(strSQL)
        End If
        YCM_AddNewSekkeiKeisokuDataTable = 0
    End Function

    Private Sub Btn_CSVoutput_Click(sender As Object, e As Windows.RoutedEventArgs) Handles Btn_CSVoutput.Click

        Dim FSD As New SaveFileDialog
        FSD.InitialDirectory = m_koji_kanri_path & "\"
        FSD.Filter = "csv files (*.csv)|*.csv"
        If FSD.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim strOutputCSVfilename As String
            strOutputCSVfilename = FSD.FileName
            CSVOutPut(strOutputCSVfilename)
        End If
    End Sub

    'CSV出力
    Private Sub CSVOutPut(ByVal strOutPutFilePath As String)
        Dim rowC As Integer

        Dim i As Integer
        rowC = m_ZahyoChiItems.Count
        If rowC = 0 Then
            MsgBox("出力するデータがありません")
        Else

            Dim strNaiyo As String = ""
            Dim strKugiri As String = ","
            'create Header row
            strNaiyo = strNaiyo & "No" & strKugiri
            strNaiyo = strNaiyo & "設計ラベル" & strKugiri
            strNaiyo = strNaiyo & "測点名" & strKugiri '20161116 baluu edit
            strNaiyo = strNaiyo & "設計点X" & strKugiri
            strNaiyo = strNaiyo & "設計点Y" & strKugiri
            strNaiyo = strNaiyo & "設計点Z" & strKugiri
            strNaiyo = strNaiyo & "計測点X" & strKugiri
            strNaiyo = strNaiyo & "計測点Y" & strKugiri
            strNaiyo = strNaiyo & "計測点Z" & strKugiri
            strNaiyo = strNaiyo & "位置合わせの重みXw" & strKugiri
            strNaiyo = strNaiyo & "位置合わせの重みYw" & strKugiri
            strNaiyo = strNaiyo & "位置合わせの重みZw" & strKugiri
            strNaiyo = strNaiyo & "差(計測点-設計点)X" & strKugiri
            strNaiyo = strNaiyo & "差(計測点-設計点)Y" & strKugiri
            strNaiyo = strNaiyo & "差(計測点-設計点)Z" & strKugiri
            strNaiyo = strNaiyo & "差(計測点-設計点)L" & vbNewLine


            'create Body
            For i = 0 To rowC - 1
                strNaiyo = strNaiyo & m_ZahyoChiItems(i).No.ToString & strKugiri
                strNaiyo = strNaiyo & m_ZahyoChiItems(i).SekkeiLabel1.ToString & strKugiri
                strNaiyo = strNaiyo & m_ZahyoChiItems(i).SokutenMei1.ToString & strKugiri
                strNaiyo = strNaiyo & m_ZahyoChiItems(i).SekkeiTenX.ToString & strKugiri
                strNaiyo = strNaiyo & m_ZahyoChiItems(i).SekkeiTenY.ToString & strKugiri
                strNaiyo = strNaiyo & m_ZahyoChiItems(i).SekkeiTenZ.ToString & strKugiri

                strNaiyo = strNaiyo & m_ZahyoChiItems(i).KeisokuTenX.ToString & strKugiri
                strNaiyo = strNaiyo & m_ZahyoChiItems(i).KeisokuTenY.ToString & strKugiri
                strNaiyo = strNaiyo & m_ZahyoChiItems(i).KeisokuTenZ.ToString & strKugiri

                strNaiyo = strNaiyo & m_ZahyoChiItems(i).IchiAwaseXw.ToString & strKugiri
                strNaiyo = strNaiyo & m_ZahyoChiItems(i).IchiAwaseYw.ToString & strKugiri
                strNaiyo = strNaiyo & m_ZahyoChiItems(i).IchiAwaseZw.ToString & strKugiri

                strNaiyo = strNaiyo & m_ZahyoChiItems(i).DiffX.ToString & strKugiri
                strNaiyo = strNaiyo & m_ZahyoChiItems(i).DiffY.ToString & strKugiri
                strNaiyo = strNaiyo & m_ZahyoChiItems(i).DiffZ.ToString & strKugiri
                strNaiyo = strNaiyo & m_ZahyoChiItems(i).DiffL.ToString & vbNewLine
            Next
            My.Computer.FileSystem.WriteAllText(strOutPutFilePath, strNaiyo, False) '20161130 baluu edit
        End If

    End Sub

    Dim strCurrentKeisokuSet As String = ""

    Private Sub CmbKeisokuset_SelectionChanged(sender As Object, e As Windows.Controls.SelectionChangedEventArgs) Handles CmbKeisokuset.SelectionChanged
        strCurrentKeisokuSet = CmbKeisokuset.SelectedValue
        SetDataToControlFromDB()
    End Sub
    'Private Sub CmbKeisokuset_TouchDown(sender As Object, e As Windows.Input.TouchEventArgs) Handles CmbKeisokuset.TouchDown
    '    strCurrentKeisokuSet = CmbKeisokuset.SelectedValue
    'End Sub

    Private Sub ComboBox_DropDownClosed(sender As Object, e As EventArgs)
        Dim Zitems As ZahyoChiItems = dataGridZahyo.SelectedItem
        If Zitems Is Nothing Then
            Exit Sub
        End If
        Dim cmbVal As String = Zitems.SokutenMei1
        For ii = 0 To nLookPoints - 1
            If gDrawPoints(ii).LabelName = cmbVal Then
                'Zitems.KeisokuTenX = gDrawPoints(ii).Real_x
                'Zitems.KeisokuTenY = gDrawPoints(ii).Real_y
                'Zitems.KeisokuTenZ = gDrawPoints(ii).Real_z

                Zitems.KeisokuTenX = 0.0
                Zitems.KeisokuTenY = 0.0
                Zitems.KeisokuTenZ = 0.0
            End If
        Next
        dataGridZahyo.ItemsSource = m_ZahyoChiItems
        dataGridZahyo.Items.Refresh()
    End Sub

    Private Sub Ichiawasew_TextChanged(sender As System.Windows.Controls.TextBox, e As Windows.Controls.TextChangedEventArgs)
        If Not Regex.IsMatch(sender.Text, "^[+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?$") Then
            sender.Text = Regex.Replace(sender.Text, "[^0-9|\.]", "")


            sender.Focus()
            MsgBox("文字入力できません")
        End If
    End Sub

    '20170215 baluu add start
    Private Sub dataGridZahyo_Selected(sender As Object, e As Windows.RoutedEventArgs) Handles dataGridZahyo.SelectionChanged
        If dataGridZahyo.SelectedItems.Count = 1 Then
            Dim item As ZahyoChiItems = dataGridZahyo.SelectedItem
            For i As Integer = 0 To gDrawPoints.Count - 1 Step 1
                If gDrawPoints(i).tid = item.SokutenID Then
                    model_pick = 1 + i

                    MainFrm.ResetImageWindow()
                    Exit For
                End If
            Next
        End If
    End Sub
    '20170215 baluu add end


    '20170220 baluu add start
    Private Sub ComboBox_KeyUp(sender As Object, e As Windows.Input.KeyEventArgs)
        Dim comboBox As System.Windows.Controls.ComboBox = sender
        If comboBox.SelectedIndex = -1 Then
            Dim item As ZahyoChiItems = comboBox.DataContext
            item.SokutenMei1 = comboBox.Text
            item.isRenamed = True
        Else
            Dim item As ZahyoChiItems = comboBox.DataContext
            item.isRenamed = False
        End If
    End Sub
    '20170220 baluu add end
End Class
