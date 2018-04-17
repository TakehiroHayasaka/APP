Imports System.Threading
Imports Microsoft.VisualBasic.FileIO
Imports System.Text.RegularExpressions
Imports HalconDotNet

Public Class YCM_LabelSetting
    Dim pos1 As New CLookPoint
    Dim pos2 As New CLookPoint
    Dim pos3 As New CLookPoint
    Private Const con1 = 10000
    Private Const mmKyoyoGosa As Double = 5
    Dim lstBunkatsu As New List(Of BunkatsuLabeling)
    'add by byambaa
    Dim FileNameList As New List(Of String)

    Private Sub YCM_LabelSetting_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DataGridView1.RowHeadersVisible = False
        DataGridView1.AllowUserToResizeColumns = False
        DataGridView1.AllowUserToResizeRows = False
        Label_Set = Me
        On Error Resume Next
        LB_P1.Text = "1点目を選択してください" ' sys_Labeling.p1.LabelName
        LB_P2.Text = "2点目を選択してください" 'sys_Labeling.p2.LabelName
        LB_P3.Text = "3点目を選択してください" 'sys_Labeling.p3.LabelName
        DataGridView1.DefaultCellStyle.Format = "0.000"

        '13.1.29追加===============================================================================
        If nLookPoints <= 0 Then
            MsgBox("計測点がありません。", MsgBoxStyle.Critical)
            Me.Close()
            Exit Sub
        End If
        'ComboBoxに計測点を追加
        C_Set_P1.Items.Add("") '1行目は空白
        C_Set_P2.Items.Add("") '1行目は空白
        C_Set_P3.Items.Add("") '1行目は空白
        For n As Integer = 0 To point_name.Count - 1
            C_Set_P1.Items.Add(gDrawPoints(n).LabelName)
            C_Set_P2.Items.Add(gDrawPoints(n).LabelName)
            C_Set_P3.Items.Add(gDrawPoints(n).LabelName)
        Next

        '13.1.29追加===============================================================================

        'If str_new.Count > 0 Then
        '    For ii As Integer = 0 To str_new.Count - 1
        '        'If UBound(str_new(ii)) = 3 Then
        '        DataGridView1.Rows.Add()
        '        DataGridView1.Rows(ii).Cells(1).Value = str_new(ii)(0)
        '        DataGridView1.Rows(ii).Cells(2).Value = str_new(ii)(1)
        '        DataGridView1.Rows(ii).Cells(3).Value = str_new(ii)(2)
        '        DataGridView1.Rows(ii).Cells(4).Value = str_new(ii)(3)
        '        'End If
        '    Next
        'End If

        'add by SUSANO 
        For Each objC3DCT As FBMlib.Common3DCodedTarget In MainFrm.objFBM.lstCommon3dCT
            Dim newBunkatsu As New BunkatsuLabeling
            Dim blnExist As Boolean = False
            For Each objBunkatsu As BunkatsuLabeling In lstBunkatsu
                If objBunkatsu.Kubun1 = objC3DCT.K1 Then
                    blnExist = True
                    objBunkatsu.lstCommonCT.Add(objC3DCT)
                    Exit For
                End If
            Next
            If blnExist = False Then
                newBunkatsu.Kubun1 = objC3DCT.K1
                newBunkatsu.lstCommonCT.Add(objC3DCT)
                lstBunkatsu.Add(newBunkatsu)
            End If
        Next
        Dim ii As Integer = 0

        For ii = lstBunkatsu.Count - 1 To 0 Step -1
            Dim objBunkatsu As BunkatsuLabeling = lstBunkatsu.Item(ii)
            Dim count As Integer = 0
            For Each objCommonCT As FBMlib.Common3DCodedTarget In objBunkatsu.lstCommonCT
                If objCommonCT.K2 = 901 Then
                    count = count + 1
                Else
                    objBunkatsu.BuzaiName = objCommonCT.info
                End If
                If objCommonCT.K2 = 902 Then
                    count = count + 1
                Else
                    objBunkatsu.BuzaiName = objCommonCT.info
                End If
                If objCommonCT.K2 = 903 Then
                    count = count + 1
                Else
                    objBunkatsu.BuzaiName = objCommonCT.info
                End If
            Next
            If count <> 3 Then
                lstBunkatsu.Remove(objBunkatsu)
            End If

        Next

        ii = 0
        For Each objBunkatsu As BunkatsuLabeling In lstBunkatsu

            DataGridView2.Rows.Add()
            DataGridView2.Rows(ii).Cells(0).Value = objBunkatsu.BuzaiName
            ii = ii + 1
        Next

    End Sub
    'add by SUSANO 
    Private Sub RunBunkatsu()

        For Each objBunkatsu As BunkatsuLabeling In lstBunkatsu

            DataGridView1.Rows.Clear()
            If objBunkatsu.lstSekkeiData.Count > 0 Then
                For ii As Integer = 0 To objBunkatsu.lstSekkeiData.Count - 1
                    Dim objSekkeiData As SekkeiData = objBunkatsu.lstSekkeiData.Item(ii)

                    DataGridView1.Rows.Add()
                    If objSekkeiData.flgKijyun = 1 Then
                        For Each objC3DCT As FBMlib.Common3DCodedTarget In objBunkatsu.lstCommonCT
                            If objC3DCT.K2 = 901 Then
                                C_Set_P1.Text = objC3DCT.currentLabel
                                '   DataGridView1.Rows(ii).Cells(0).Value = objC3DCT.currentLabel
                                Exit For
                            End If
                        Next
                    End If
                    If objSekkeiData.flgKijyun = 2 Then
                        For Each objC3DCT As FBMlib.Common3DCodedTarget In objBunkatsu.lstCommonCT
                            If objC3DCT.K2 = 902 Then
                                C_Set_P2.Text = objC3DCT.currentLabel
                                '   DataGridView1.Rows(ii).Cells(0).Value = objC3DCT.currentLabel
                                Exit For
                            End If
                        Next
                    End If
                    If objSekkeiData.flgKijyun = 3 Then
                        For Each objC3DCT As FBMlib.Common3DCodedTarget In objBunkatsu.lstCommonCT
                            If objC3DCT.K2 = 903 Then
                                C_Set_P3.Text = objC3DCT.currentLabel
                                '   DataGridView1.Rows(ii).Cells(0).Value = objC3DCT.currentLabel
                                Exit For
                            End If
                        Next
                    End If
                    DataGridView1.Rows(ii).Cells(1).Value = objBunkatsu.HeadStr & objSekkeiData.PointName
                    DataGridView1.Rows(ii).Cells(2).Value = objSekkeiData.Point3d.X
                    DataGridView1.Rows(ii).Cells(3).Value = objSekkeiData.Point3d.Y
                    DataGridView1.Rows(ii).Cells(4).Value = objSekkeiData.Point3d.Z

                Next
            End If

            If objBunkatsu.lstSekkeiData.Count > 0 Then
                For ii As Integer = 0 To objBunkatsu.lstSekkeiData.Count - 1
                    Dim objSekkeiData As SekkeiData = objBunkatsu.lstSekkeiData.Item(ii)

                    If objSekkeiData.flgKijyun = 1 Then
                        For Each objC3DCT As FBMlib.Common3DCodedTarget In objBunkatsu.lstCommonCT
                            If objC3DCT.K2 = 901 Then
                                ' C_Set_P1.Text = objC3DCT.currentLabel
                                DataGridView1.Rows(ii).Cells(0).Value = objC3DCT.currentLabel
                                Exit For
                            End If
                        Next
                    End If
                    If objSekkeiData.flgKijyun = 2 Then
                        For Each objC3DCT As FBMlib.Common3DCodedTarget In objBunkatsu.lstCommonCT
                            If objC3DCT.K2 = 902 Then
                                ' C_Set_P2.Text = objC3DCT.currentLabel
                                DataGridView1.Rows(ii).Cells(0).Value = objC3DCT.currentLabel
                                Exit For
                            End If
                        Next
                    End If
                    If objSekkeiData.flgKijyun = 3 Then
                        For Each objC3DCT As FBMlib.Common3DCodedTarget In objBunkatsu.lstCommonCT
                            If objC3DCT.K2 = 903 Then
                                '  C_Set_P3.Text = objC3DCT.currentLabel
                                DataGridView1.Rows(ii).Cells(0).Value = objC3DCT.currentLabel
                                Exit For
                            End If
                        Next
                    End If
                    'DataGridView1.Rows(ii).Cells(1).Value = bunlbl.HeadStr & objSekkeiData.PointName
                    'DataGridView1.Rows(ii).Cells(2).Value = objSekkeiData.Point3d.X
                    'DataGridView1.Rows(ii).Cells(3).Value = objSekkeiData.Point3d.Y
                    'DataGridView1.Rows(ii).Cells(4).Value = objSekkeiData.Point3d.Z

                Next
            End If


            '結果により、結果ダイアログに値を入れる
            Dim frmResult As New YCM_LabelResult
            ReLabelingSyori(frmResult)
            frmResult.OKsyori()
        Next
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles B_Close.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles B_Set_Dsv.Click
        OpenFileDialog1.FileName = "*.csv"
        OpenFileDialog1.Filter = "*.csv|*.csv"
        'On Error GoTo ErrHandle

        Dim St As String '13.1.28追加

        Dim result As System.Windows.Forms.DialogResult = OpenFileDialog1.ShowDialog()
        If result = System.Windows.Forms.DialogResult.OK Then
            str = New ArrayList
            str_new = New ArrayList
            csv_path = OpenFileDialog1.FileName()
            TB_Path.Text = csv_path
            Dim a As System.IO.StreamReader
            a = New System.IO.StreamReader(csv_path)

            '13.1.28設計ラベルの全角・半角の判定====================================================
            str.Add("") '1行ダミーを呼び飛ばす（タイトルの行）

            a.ReadLine() '1行ダミーを呼び飛ばす（タイトルの行）

            While a.Peek >= 0
                'str.Add(Trim(a.ReadLine))'13.1.28以前

                St = Trim(a.ReadLine).ToUpper()
                Dim StA As String = StrConv(St, vbNarrow) 'StrConv(文字列,vbNarrow)文字列を半角に変換する
                If (St <> StA) Then
                    MsgBox("設計ラベルは、半角文字で入力してください", MsgBoxStyle.Critical)
                    Exit Sub
                End If
                str.Add(St)

                'ToUpper() : 小文字は大文字に()
                'Peek：次の1文字を読み込むメソッド。これ以上読み込むデータがない場合"-1"を返す
                'ReadLine：次の1行を読み込むメソッド。これ以上読み込むデータがない場合"Nothing"を返す
            End While
            '===================================================================================

            For ii As Integer = 1 To str.Count - 1
                str_new.Add(Split(str(ii), ","))
            Next
            DataGridView1.Rows.Clear()
            If str_new.Count > 0 Then
                For ii As Integer = 0 To str_new.Count - 1
                    'If UBound(str_new(ii)) = 3 Then
                    DataGridView1.Rows.Add()
                    DataGridView1.Rows(ii).Cells(1).Value = str_new(ii)(0)
                    DataGridView1.Rows(ii).Cells(2).Value = CDbl(str_new(ii)(1))
                    DataGridView1.Rows(ii).Cells(3).Value = CDbl(str_new(ii)(2))
                    DataGridView1.Rows(ii).Cells(4).Value = CDbl(str_new(ii)(3))
                    'End If
                Next
            End If
        End If

        'ErrHandle:
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles B_Set_P1.Click
        If point_name.Count > 0 Then
            Me.Hide()
            IOUtil.GetPoint(pos1, "点目を指示：")
            sys_Labeling.p1 = pos1
            'add by byambaa
            'If lstBunkatsu.Item(0).lstCommonCT.Item(0).K2 = 901 And lstBunkatsu.Item(0).lstSekkeiData.Item(0).flgKijyun = 1 Then
            '    'lstBunkatsu.Item(0).lstCommonCT.Item(0).CT_No = lstBunkatsu.Item(0).lstSekkeiData.Item(0).PointName
            '    C_Set_P1.Text = lstBunkatsu.Item(0).lstCommonCT.Item(0).CT_No
            'End If

            LB_P1.Text = sys_Labeling.p1.LabelName

            C_Set_P1.Text = pos1.LabelName  '13.1.29　選択した計測点をC_Set_P1のTextに
            '<コンボボックスSelectedIndexChanged>イベントに

            ''13.1.28以前（修正前）=======================================
            'Dim column As New DataGridViewComboBoxColumn()
            'column.Items.Add(LB_P1.Text)
            'column.Items.Add(LB_P2.Text)
            'column.Items.Add(LB_P3.Text)
            'column.DataPropertyName = "計測点"
            'DataGridView1.Columns.Insert(0, column)
            'DataGridView1.Columns(0).Width = 60
            'DataGridView1.Columns.RemoveAt(1)
            'column.Name = "計測点"
            ''13.1.28以前（修正前）=======================================

            Me.Show()
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles B_Set_P2.Click
        If point_name.Count > 0 Then
            Me.Hide()
            IOUtil.GetPoint(pos2, "点目を指示：")
            sys_Labeling.p2 = pos2
            LB_P2.Text = sys_Labeling.p2.LabelName

            C_Set_P2.Text = pos2.LabelName  '13.1.29　選択した計測点をC_Set_P2のTextに
            '<コンボボックスSelectedIndexChanged>イベントに

            ''13.1.28以前（修正前）=======================================
            'Dim column As New DataGridViewComboBoxColumn()
            'column.Items.Add(LB_P1.Text)
            'column.Items.Add(LB_P2.Text)
            'column.Items.Add(LB_P3.Text)
            'column.DataPropertyName = "計測点"
            'DataGridView1.Columns.Insert(0, column)
            'DataGridView1.Columns(0).Width = 60
            'DataGridView1.Columns.RemoveAt(1)
            'column.Name = "計測点"
            ''13.1.28以前（修正前）=======================================

            Me.Show()
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles B_Set_P3.Click
        If point_name.Count > 0 Then
            Me.Hide()
            IOUtil.GetPoint(pos3, "点目を指示：")
            sys_Labeling.p3 = pos3
            LB_P3.Text = sys_Labeling.p3.LabelName

            C_Set_P3.Text = pos3.LabelName  '13.1.29　選択した計測点をC_Set_P3のTextに
            '<コンボボックスSelectedIndexChanged>イベントに

            ''13.1.28以前修正前=======================================
            'Dim column As New DataGridViewComboBoxColumn()
            'column.Items.Add(LB_P1.Text)
            'column.Items.Add(LB_P2.Text)
            'column.Items.Add(LB_P3.Text)
            'column.DataPropertyName = "計測点"
            'DataGridView1.Columns.Insert(0, column)
            'DataGridView1.Columns(0).Width = 60
            'DataGridView1.Columns.RemoveAt(1)
            'column.Name = "計測点"
            ''13.1.28以前修正前=======================================

            Me.Show()
        End If
    End Sub
    Public Sub Button_1_Click()
        'pos1 = Command_cp
        LB_P3.Text = pos1.LabelName
    End Sub

    Private Sub DataGridView1_CellValueChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellValueChanged
        Dim iii As Integer
        iii = e.RowIndex
        For ii As Integer = 0 To DataGridView1.Rows.Count - 1

            '13.1.30修正（LB_P○⇒C_Set_P○に）================================================================================================
            If DataGridView1.Rows(iii).Cells(0).Value = C_Set_P1.Text And DataGridView1.Rows(ii).Cells(0).Value = C_Set_P1.Text And ii <> iii Then
                DataGridView1.Rows(ii).Cells(0).Value = ""
            End If
            If DataGridView1.Rows(iii).Cells(0).Value = C_Set_P2.Text And DataGridView1.Rows(ii).Cells(0).Value = C_Set_P2.Text And ii <> iii Then
                DataGridView1.Rows(ii).Cells(0).Value = ""
            End If
            If DataGridView1.Rows(iii).Cells(0).Value = C_Set_P3.Text And DataGridView1.Rows(ii).Cells(0).Value = C_Set_P3.Text And ii <> iii Then
                DataGridView1.Rows(ii).Cells(0).Value = ""
            End If
            '13.1.30修正（LB_P○⇒C_Set_P○に）================================================================================================

            ''13.1.28以前（修正前）=============================================================================================================
            'If DataGridView1.Rows(iii).Cells(0).Value = LB_P1.Text And DataGridView1.Rows(ii).Cells(0).Value = LB_P1.Text And ii <> iii Then
            '    DataGridView1.Rows(ii).Cells(0).Value = ""
            'End If
            'If DataGridView1.Rows(iii).Cells(0).Value = LB_P2.Text And DataGridView1.Rows(ii).Cells(0).Value = LB_P2.Text And ii <> iii Then
            '    DataGridView1.Rows(ii).Cells(0).Value = ""
            'End If
            'If DataGridView1.Rows(iii).Cells(0).Value = LB_P3.Text And DataGridView1.Rows(ii).Cells(0).Value = LB_P3.Text And ii <> iii Then
            '    DataGridView1.Rows(ii).Cells(0).Value = ""
            'End If
            ''13.1.28以前（修正前）=============================================================================================================
        Next
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles B_Ok.Click
        If RadioButton1.Checked = True Then
            Run()
        End If
        If RadioButton2.Checked = True Then
            RunBunkatsu()
            MsgBox("自動レラベリング処理完了しました。")
            Me.Close()
        End If
    End Sub

    Private Function Labelling_Check() As Integer

        Labelling_Check = -1

        If sys_Labeling.p1.LabelName = "" Then
            MsgBox("点1を選択してください。", MsgBoxStyle.Critical)
            Exit Function
        End If

        If sys_Labeling.p2.LabelName = "" Then
            MsgBox("点2を選択してください。", MsgBoxStyle.Critical)
            Exit Function
        End If

        If sys_Labeling.p3.LabelName = "" Then
            MsgBox("点3を選択してください。", MsgBoxStyle.Critical)
            Exit Function
        End If

        If nLookPoints = 0 Then
            MsgBox("計測データを開いてください。", MsgBoxStyle.Critical)
            Exit Function
        End If

        Dim blnFindP As Boolean = False
        For ii As Integer = 0 To nLookPoints - 1
            If gDrawPoints(ii).LabelName = sys_Labeling.p1.LabelName Then
                blnFindP = True
                Exit For
            End If
        Next
        If blnFindP = False Then
            MsgBox("点1のラベルが見つかりませんので、点1を改めて選択してください。", MsgBoxStyle.Critical)
            Exit Function
        End If

        blnFindP = False
        For ii As Integer = 0 To nLookPoints - 1
            If gDrawPoints(ii).LabelName = sys_Labeling.p2.LabelName Then
                blnFindP = True
                Exit For
            End If
        Next
        If blnFindP = False Then
            MsgBox("点2のラベルが見つかりませんので、点2を改めて選択してください。", MsgBoxStyle.Critical)
            Exit Function
        End If

        blnFindP = False
        For ii As Integer = 0 To nLookPoints - 1
            If gDrawPoints(ii).LabelName = sys_Labeling.p3.LabelName Then
                blnFindP = True
                Exit For
            End If
        Next
        If blnFindP = False Then
            MsgBox("点3のラベルが見つかりませんので、原点を改めて選択してください。", MsgBoxStyle.Critical)
            Exit Function
        End If


        If (sys_Labeling.p1.LabelName = sys_Labeling.p2.LabelName) Or _
            (sys_Labeling.p1.LabelName = sys_Labeling.p3.LabelName) Or _
            (sys_Labeling.p3.LabelName = sys_Labeling.p2.LabelName) Then
            MsgBox("点1、点2と点3は同名のラベルを持っていますので、再選択してください。", MsgBoxStyle.Critical)
            Exit Function
        End If

        If TB_Path.Text = "" Then
            MsgBox("設計データファイルを選択してください。", MsgBoxStyle.Critical)
            Exit Function
        End If

        If DataGridView1.Rows.Count >= 3 Then
            Dim blnP1 As Boolean = False, blnP2 As Boolean = False, blnP3 As Boolean = False
            For ii As Integer = 0 To DataGridView1.Rows.Count - 1

                '13.1.30修正（LB_P○⇒C_Set_P○に）============================
                If DataGridView1.Rows(ii).Cells(0).Value = C_Set_P1.Text Then
                    blnP1 = True
                End If
                If DataGridView1.Rows(ii).Cells(0).Value = C_Set_P2.Text Then
                    blnP2 = True
                End If
                If DataGridView1.Rows(ii).Cells(0).Value = C_Set_P3.Text Then
                    blnP3 = True
                End If
                '13.1.30修正（LB_P○⇒C_Set_P○に）============================

                ''13.1.28以前（修正前）========================================
                'If DataGridView1.Rows(ii).Cells(0).Value = LB_P1.Text Then
                '    blnP1 = True
                'End If
                'If DataGridView1.Rows(ii).Cells(0).Value = LB_P2.Text Then
                '    blnP2 = True
                'End If
                'If DataGridView1.Rows(ii).Cells(0).Value = LB_P3.Text Then
                '    blnP3 = True
                'End If
                ''13.1.28以前（修正前）========================================

            Next

            If blnP1 = False Then
                MsgBox("設計データの点1を指定してください。", MsgBoxStyle.Critical)
                Exit Function
            End If
            If blnP2 = False Then
                MsgBox("設計データの点2を指定してください。", MsgBoxStyle.Critical)
                Exit Function
            End If
            If blnP3 = False Then
                MsgBox("設計データの点3を指定してください。", MsgBoxStyle.Critical)
                Exit Function
            End If
        Else
            MsgBox("設計データファイルに三つ点が必要ですので、設計データファイルを再指定してください。", MsgBoxStyle.Critical)
            Exit Function

        End If

        Labelling_Check = 0
    End Function

    '13.1.29　C_Set_P1コンボボックスの選択が変わったら
    Private Sub C_Set_P1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles C_Set_P1.SelectedIndexChanged

        Dim strmark1 As String = C_Set_P1.Text
        Dim CP1 As New CLookPoint
        Dim ind1 As Integer
        ind1 = GetPosFromLabelName(CLookPoint.posTypeMode.All, strmark1, CP1)
        If ind1 < 0 Then
            MsgBox("指定されたラベルに対応する計測点が見つかりません。" & vbCrLf & "ラベルを確認して下さい。", MsgBoxStyle.Critical)
            Exit Sub
        End If
        sys_Labeling.p1 = CP1
        '以下、DataGridView1(設計データ)のComboBoxに選択した計測点名を追加=======
        Dim column As New DataGridViewComboBoxColumn()

        column.Items.Add(C_Set_P2.Text)
        column.Items.Add(C_Set_P3.Text)
        column.DataPropertyName = "計測点"
        DataGridView1.Columns.Insert(0, column)
        DataGridView1.Columns(0).Width = 60
        DataGridView1.Columns.RemoveAt(1)
        column.Name = "計測点"
        '13.1.29=================================================================

    End Sub
    '13.1.29　C_Set_P2コンボボックスの選択が変わったら
    Private Sub C_Set_P2_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles C_Set_P2.SelectedIndexChanged

        Dim strmark2 As String = C_Set_P2.Text
        Dim CP2 As New CLookPoint
        Dim ind2 As Integer
        ind2 = GetPosFromLabelName(CLookPoint.posTypeMode.All, strmark2, CP2)
        If ind2 < 0 Then
            MsgBox("指定されたラベルに対応する計測点が見つかりません。" & vbCrLf & "ラベルを確認して下さい。", MsgBoxStyle.Critical)
            Exit Sub
        End If
        sys_Labeling.p2 = CP2
        '以下、DataGridView1(設計データ)のComboBoxに選択した計測点名を追加=======
        Dim column As New DataGridViewComboBoxColumn()
        column.Items.Add(C_Set_P1.Text)
        column.Items.Add(C_Set_P2.Text)
        column.Items.Add(C_Set_P3.Text)
        column.DataPropertyName = "計測点"
        DataGridView1.Columns.Insert(0, column)
        DataGridView1.Columns(0).Width = 60
        DataGridView1.Columns.RemoveAt(1)
        column.Name = "計測点"
        '13.1.29=================================================================

    End Sub
    '13.1.29　C_Set_P3コンボボックスの選択が変わったら
    Private Sub C_Set_P3_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles C_Set_P3.SelectedIndexChanged

        Dim strmark3 As String = C_Set_P3.Text
        Dim CP3 As New CLookPoint
        Dim ind3 As Integer
        ind3 = GetPosFromLabelName(CLookPoint.posTypeMode.All, strmark3, CP3)
        If ind3 < 0 Then
            MsgBox("指定されたラベルに対応する計測点が見つかりません。" & vbCrLf & "ラベルを確認して下さい。", MsgBoxStyle.Critical)
            Exit Sub
        End If
        sys_Labeling.p3 = CP3

        '以下、DataGridView1(設計データ)のComboBoxに選択した計測点名を追加=======
        Dim column As New DataGridViewComboBoxColumn()
        column.Items.Add(C_Set_P1.Text)
        column.Items.Add(C_Set_P2.Text)
        column.Items.Add(C_Set_P3.Text)
        column.DataPropertyName = "計測点"
        DataGridView1.Columns.Insert(0, column)
        DataGridView1.Columns(0).Width = 60
        DataGridView1.Columns.RemoveAt(1)
        column.Name = "計測点"
        '13.1.29=================================================================

    End Sub
    Private Function Run()

        'sys_Labeling変数がここで一番重要なので、ダイアログのすべてのパラメータは
        'この構造体に格納する


        '各値をチェックする
        If Labelling_Check() = -1 Then Exit Function

        '結果により、結果ダイアログに値を入れる
        Dim frmResult As New YCM_LabelResult
        ReLabelingSyori(frmResult)

        Me.Hide()
        frmResult.ShowDialog()
        If frmResult.m_Return Then
            Me.Close()
            Me.Show()
        Else
            Me.Close()
        End If

    End Function

    Private Sub ReLabelingSyori(ByRef frmResult As YCM_LabelResult)
        Dim intMaxRecord As Integer
        Dim Xj(0 To con1) As Double, Yj(0 To con1) As Double, Zj(0 To con1) As Double, _
        Xi(0 To con1) As Double, Yi(0 To con1) As Double, Zi(0 To con1) As Double, _
        Xa(0 To con1) As Double, Ya(0 To con1) As Double, Za(0 To con1) As Double, _
        Xs(0 To con1) As Double, Ys(0 To con1) As Double, Zs(0 To con1) As Double
        Dim str_lableName(0 To con1) As String, bln_LableUse(0 To con1) As Boolean
        Dim l_count As Integer

        '13.1.30修正（LB_P○⇒C_Set_P○に）================================================================================================
        '3点の座標をsys_Labelingから取得する

        Xs(0) = sys_Labeling.p1.Real_x : Ys(0) = sys_Labeling.p1.Real_y : Zs(0) = sys_Labeling.p1.Real_z
        Xs(1) = sys_Labeling.p2.Real_x : Ys(1) = sys_Labeling.p2.Real_y : Zs(1) = sys_Labeling.p2.Real_z
        Xs(2) = sys_Labeling.p3.Real_x : Ys(2) = sys_Labeling.p3.Real_y : Zs(2) = sys_Labeling.p3.Real_z
        bln_LableUse(0) = True : bln_LableUse(1) = True : bln_LableUse(2) = True
        str_lableName(0) = sys_Labeling.p1.LabelName
        str_lableName(1) = sys_Labeling.p2.LabelName
        str_lableName(2) = sys_Labeling.p3.LabelName
        l_count = 0

        'グリッドから計測値を取得する

        For ii = 0 To nLookPoints - 1
            If gDrawPoints(ii).LabelName <> sys_Labeling.p1.LabelName And _
               gDrawPoints(ii).LabelName <> sys_Labeling.p2.LabelName And _
               gDrawPoints(ii).LabelName <> sys_Labeling.p3.LabelName Then
                Xs(3 + l_count) = gDrawPoints(ii).Real_x
                Ys(3 + l_count) = gDrawPoints(ii).Real_y
                Zs(3 + l_count) = gDrawPoints(ii).Real_z
                str_lableName(3 + l_count) = gDrawPoints(ii).LabelName
                bln_LableUse(3 + l_count) = False
                l_count = l_count + 1
            End If
        Next
        intMaxRecord = l_count + 3

        '重みを設定する

        Xi(0) = 1.0# : Yi(0) = 1.0# : Zi(0) = 1.0#
        Xi(1) = 1.0# : Yi(1) = 1.0# : Zi(1) = 1.0#
        Xi(2) = 1.0# : Yi(2) = 1.0# : Zi(2) = 1.0#

        'グリッドから設計値を取得する

        l_count = 0

        For ii As Integer = 0 To DataGridView1.Rows.Count - 1
            If DataGridView1.Rows(ii).Cells(0).Value = C_Set_P1.Text Then
                Xj(0) = DataGridView1.Rows(ii).Cells(2).Value
                Yj(0) = DataGridView1.Rows(ii).Cells(3).Value
                Zj(0) = DataGridView1.Rows(ii).Cells(4).Value

                GoTo Nextii
            End If
            If DataGridView1.Rows(ii).Cells(0).Value = C_Set_P2.Text Then
                Xj(1) = DataGridView1.Rows(ii).Cells(2).Value
                Yj(1) = DataGridView1.Rows(ii).Cells(3).Value
                Zj(1) = DataGridView1.Rows(ii).Cells(4).Value

                bln_LableUse(1) = True
                GoTo Nextii
            End If
            If DataGridView1.Rows(ii).Cells(0).Value = C_Set_P3.Text Then
                Xj(2) = DataGridView1.Rows(ii).Cells(2).Value
                Yj(2) = DataGridView1.Rows(ii).Cells(3).Value
                Zj(2) = DataGridView1.Rows(ii).Cells(4).Value

                bln_LableUse(2) = True
                GoTo Nextii
            End If
            Xj(3 + l_count) = DataGridView1.Rows(ii).Cells(2).Value
            Yj(3 + l_count) = DataGridView1.Rows(ii).Cells(3).Value
            Zj(3 + l_count) = DataGridView1.Rows(ii).Cells(4).Value

            l_count = l_count + 1
Nextii:
        Next

        If intMaxRecord < l_count + 3 Then
            intMaxRecord = l_count + 3
        End If
        Dim num As Integer
        'Dim fb As Integer '20170330 baluu del
        Dim geoP As New GeoPoint, geoP_T As New GeoPoint, geoP_Min As New GeoPoint
        num = intMaxRecord
        '設計値により結果を計算する

        Dim hhmm As HTuple = Nothing
        correspond_3d_3d_weight(Xs, Ys, Zs, Xj, Yj, Zj, Xi, Yi, Zi, hhmm)
        HOperatorSet.AffineTransPoint3d(hhmm, Xs, Ys, Zs, Xa, Ya, Za)

        'fb = OmomiExcute(num, Xi(0), Yi(0), Zi(0), Xj(0), Yj(0), Zj(0), Xa(0), Ya(0), Za(0), Xs(0), Ys(0), Zs(0)) '20170330 baluu del


        l_count = 1
        For ii As Integer = 0 To DataGridView1.Rows.Count - 1
            frmResult.data_point_result.Rows.Add()
            frmResult.data_point_result.Rows(ii).Cells(0).Value = CStr(l_count)
            frmResult.data_point_result.Rows(ii).Cells(1).Value = DataGridView1.Rows(ii).Cells(1).Value
            frmResult.data_point_result.Rows(ii).Cells(3).Value = DataGridView1.Rows(ii).Cells(2).Value
            frmResult.data_point_result.Rows(ii).Cells(4).Value = DataGridView1.Rows(ii).Cells(3).Value
            frmResult.data_point_result.Rows(ii).Cells(5).Value = DataGridView1.Rows(ii).Cells(4).Value
            If DataGridView1.Rows(ii).Cells(0).Value = C_Set_P1.Text Then
                frmResult.data_point_result.Rows(ii).Cells(2).Value = C_Set_P1.Text
                frmResult.data_point_result.Rows(ii).Cells(6).Value = Xa(0)
                frmResult.data_point_result.Rows(ii).Cells(7).Value = Ya(0)
                frmResult.data_point_result.Rows(ii).Cells(8).Value = Za(0)

            ElseIf DataGridView1.Rows(ii).Cells(0).Value = C_Set_P2.Text Then
                frmResult.data_point_result.Rows(ii).Cells(2).Value = C_Set_P2.Text
                frmResult.data_point_result.Rows(ii).Cells(6).Value = Xa(1)
                frmResult.data_point_result.Rows(ii).Cells(7).Value = Ya(1)
                frmResult.data_point_result.Rows(ii).Cells(8).Value = Za(1)

            ElseIf DataGridView1.Rows(ii).Cells(0).Value = C_Set_P3.Text Then
                frmResult.data_point_result.Rows(ii).Cells(2).Value = C_Set_P3.Text
                frmResult.data_point_result.Rows(ii).Cells(6).Value = Xa(2)
                frmResult.data_point_result.Rows(ii).Cells(7).Value = Ya(2)
                frmResult.data_point_result.Rows(ii).Cells(8).Value = Za(2)
            Else

                '近い点を探し出す

                geoP_T.setXYZ(DataGridView1.Rows(ii).Cells(2).Value, DataGridView1.Rows(ii).Cells(3).Value, DataGridView1.Rows(ii).Cells(4).Value)
                Dim blnFirstSet As Boolean = True
                Dim distance As Double = 0.0#

                Dim dis_Tmp As Double, dis_index As Integer = -1
                For kk As Integer = 3 To Data_Point.DGV_DV.Rows.Count - 1
                    If bln_LableUse(kk) = False Then
                        geoP.setXYZ(Xa(kk), Ya(kk), Za(kk))
                        dis_Tmp = geoP.GetDistanceTo(geoP_T)
                        'dis_index = kk
                        If blnFirstSet Then

                            distance = dis_Tmp
                            If distance < mmKyoyoGosa Then
                                geoP_Min = geoP.Copy
                                dis_index = kk
                            End If

                            blnFirstSet = False
                        Else
                            If dis_Tmp < distance And dis_Tmp < mmKyoyoGosa Then
                                geoP_Min = geoP.Copy
                                distance = dis_Tmp
                                dis_index = kk
                            End If
                        End If
                    End If
                Next

                If dis_index <> -1 Then
                    bln_LableUse(dis_index) = True
                    frmResult.data_point_result.Rows(ii).Cells(2).Value = str_lableName(dis_index)
                    frmResult.data_point_result.Rows(ii).Cells(6).Value = geoP_Min.x
                    frmResult.data_point_result.Rows(ii).Cells(7).Value = geoP_Min.y
                    frmResult.data_point_result.Rows(ii).Cells(8).Value = geoP_Min.z
                End If
            End If

            If frmResult.data_point_result.Rows(ii).Cells(2).Value <> "" And _
               frmResult.data_point_result.Rows(ii).Cells(1).Value <> "" Then
                frmResult.data_point_result.Rows(ii).Cells(9).Value = frmResult.data_point_result.Rows(ii).Cells(3).Value - frmResult.data_point_result.Rows(ii).Cells(6).Value
                frmResult.data_point_result.Rows(ii).Cells(10).Value = frmResult.data_point_result.Rows(ii).Cells(4).Value - frmResult.data_point_result.Rows(ii).Cells(7).Value
                frmResult.data_point_result.Rows(ii).Cells(11).Value = frmResult.data_point_result.Rows(ii).Cells(5).Value - frmResult.data_point_result.Rows(ii).Cells(8).Value
            End If

            l_count = l_count + 1
        Next

        '残りのデータを表示する
        For ii As Integer = 0 To Data_Point.DGV_DV.Rows.Count - 1
            If bln_LableUse(ii) = False Then
                frmResult.data_point_result.Rows.Add()
                frmResult.data_point_result.Rows(l_count - 1).Cells(0).Value = CStr(l_count)
                frmResult.data_point_result.Rows(l_count - 1).Cells(2).Value = str_lableName(ii)
                frmResult.data_point_result.Rows(l_count - 1).Cells(6).Value = Xa(ii)
                frmResult.data_point_result.Rows(l_count - 1).Cells(7).Value = Ya(ii)
                frmResult.data_point_result.Rows(l_count - 1).Cells(8).Value = Za(ii)
                l_count = l_count + 1
            End If
        Next
    End Sub
    'add by byambaa 2016-03-16
    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked = True Then
            GroupBox2.Enabled = True
            GroupBox3.Enabled = False
            '20160407 Kiryu Add Disable時グレーアウト
            DataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.LightGray
            DataGridView2.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray

        Else
            If RadioButton2.Checked = True Then
                GroupBox3.Enabled = True
                GroupBox2.Enabled = False
                '20160407 Kiryu Add Disable時グレーアウト
                DataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.Black
                DataGridView2.DefaultCellStyle.ForeColor = System.Drawing.Color.Black

            End If
        End If
    End Sub

    Private Sub DataGridView2_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellContentClick

        Dim ofd As New OpenFileDialog
        If e.ColumnIndex = 2 Then
            'ofd.ShowDialog()
            If ofd.ShowDialog() = DialogResult.OK Then

                'FileNameList.Add(ofd.FileName)
                If lstBunkatsu.Item(e.RowIndex).ReadSekkeiData(ofd.FileName) = True Then
                    lstBunkatsu.Item(e.RowIndex).SekkeiFileName = ofd.FileName
                    DataGridView2.Item(e.ColumnIndex - 1, e.RowIndex).Value = ofd.FileName
                Else
                    MsgBox("設計データが読み込めませんでした。" & vbCrLf & "指定したファイルが設計データの形式(書式)ではありません。")
                End If
            End If
        End If

    End Sub
    Private Sub DataGridView2_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellEndEdit
        If (e.ColumnIndex = 3) Then
            Dim value As String = DataGridView2.Rows(e.RowIndex).Cells(e.ColumnIndex).Value

            Dim match As Boolean = Regex.IsMatch(value, "[a-zA-Z]")
            Dim match1 As Boolean = Regex.IsMatch(value, "[0-9]")

            If match = True And match1 = False Then
                lstBunkatsu.Item(e.RowIndex).HeadStr = value
                DataGridView2.Item(e.ColumnIndex, e.RowIndex).Value = value
            Else
                DataGridView2.Item(e.ColumnIndex, e.RowIndex).Value = ""
                MsgBox("ラベリング用頭文字には数字を設定できません。")
            End If

        End If
    End Sub


End Class