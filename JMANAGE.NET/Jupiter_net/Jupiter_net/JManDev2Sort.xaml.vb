Imports System.Windows.Forms
Imports System.IO
Imports System.Text

Public Class JManDev2Sort

    Private _kojiFolder As String = ""
    Private _type As Integer = 2
    Private _settingFile As New JManSettingFile()
    Public _isOkCanselError As Integer = 1          '0:OK、1:Cansel、2:Error
    Public _scrFileName As String = ""

    '---------------------------------------------------------------------------------------------------
    '機能
    '   画面を初期化する。
    '引数
    '   type:(I) 0：展開～変形、1：分類、2：展開～分類
    '---------------------------------------------------------------------------------------------------
    ' 2017/03/18 Nakagawa Edit Start
    'Public Sub OnInitialize(kojiFolder As String, type As Integer, settingFile As JManSettingFile, buttonColor As Integer, backColor As Integer)
    Public Sub OnInitialize(kojiFolder As String, type As Integer, kozoNo As Integer, settingFile As JManSettingFile, buttonColor As Integer, backColor As Integer)
        ' 2017/03/18 Nakagawa Edit End

        Me._kojiFolder = kojiFolder
        Me._type = type
        Me._settingFile = settingFile

        '背景色とボタン色を変更する。
        If backColor = 0 Then
            Me.Background = New SolidColorBrush(System.Windows.Media.Colors.DarkGray)
        Else
            Me.Background = New SolidColorBrush(System.Windows.Media.Colors.White)
        End If
        If buttonColor = 0 Then
            Me.ButtonDev2SortOk.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 50, 50, 50))
            Me.ButtonDev2SortCansel.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 50, 50, 50))
            Me.ButtonDev2SortAllDel.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 50, 50, 50))
            Me.ButtonDev2SortSelectAll.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 50, 50, 50))
            Me.ButtonDev2SortSelectStd.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 50, 50, 50))
        Else
            Me.ButtonDev2SortOk.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 164, 150))
            Me.ButtonDev2SortCansel.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 164, 150))
            Me.ButtonDev2SortAllDel.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 164, 150))
            Me.ButtonDev2SortSelectAll.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 164, 150))
            Me.ButtonDev2SortSelectStd.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 164, 150))
        End If

        Me.setCheckBoxSelectStd()

        '設定ファイルがある場合のみチェックボックスを有効にする。
        '溶接時期設定
        If System.IO.File.Exists(settingFile._weldStageFileName) <> True Then
            Me.CheckBoxWeldStage.IsChecked = False
            Me.CheckBoxWeldStage.IsEnabled = False
        End If
        '加工代
        If System.IO.File.Exists(settingFile._kakFileName) <> True Then
            Me.CheckBoxKak.IsChecked = False
            Me.CheckBoxKak.IsEnabled = False
        End If
        '収縮
        If System.IO.File.Exists(settingFile._syuFileName) <> True Then
            Me.CheckBoxSyu.IsChecked = False
            Me.CheckBoxSyu.IsEnabled = False
        End If
        'ロット
        If System.IO.File.Exists(settingFile._lotFileName) <> True Then
            Me.CheckBoxLot.IsChecked = False
            Me.CheckBoxLot.IsEnabled = False
        End If
        '製作区分
        If System.IO.File.Exists(settingFile._kubFileName) <> True Then
            Me.CheckBoxKub.IsChecked = False
            Me.CheckBoxKub.IsEnabled = False
        End If
        '実測データ
        If System.IO.File.Exists(settingFile._splFileName) <> True Then
            Me.CheckBoxSpl.IsChecked = False
            Me.CheckBoxSpl.IsEnabled = False
        End If
        '孔明け方法
        If System.IO.File.Exists(settingFile._anaFileName) <> True Then
            Me.CheckBoxAna.IsChecked = False
            Me.CheckBoxAna.IsEnabled = False
        End If
        '罫書・切断方法
        If System.IO.File.Exists(settingFile._kseFileName) <> True Then
            Me.CheckBoxKse.IsChecked = False
            Me.CheckBoxKse.IsEnabled = False
        End If
        '切断可能板厚設定
        If System.IO.File.Exists(settingFile._cutOkFileName) <> True Then
            Me.CheckBoxThc.IsChecked = False
            Me.CheckBoxThc.IsEnabled = False
        End If
        '表面処理
        If System.IO.File.Exists(settingFile._hsrFileName) <> True Then
            Me.CheckBoxHsr.IsChecked = False
            Me.CheckBoxHsr.IsEnabled = False
        End If
        '摩擦面剥離
        If System.IO.File.Exists(settingFile._mgkFileName) <> True Then
            Me.CheckBoxMgk.IsChecked = False
            Me.CheckBoxMgk.IsEnabled = False
        End If
        'ソーティングパラメータ
        If System.IO.File.Exists(settingFile._praFileName) <> True Then
            Me.CheckBoxPra.IsChecked = False
            Me.CheckBoxPra.IsEnabled = False
        End If
        'PMF補正
        If System.IO.File.Exists(settingFile._pmfHoseiFileName) <> True Then
            Me.CheckBoxPmfHosei.IsChecked = False
            Me.CheckBoxPmfHosei.IsEnabled = False
        End If

        ' 2017/03/18 Nakagawa Add Start
        If kozoNo <> 6 Then
            Me.CheckBoxSegPmfBzi.IsChecked = False
            ' 2017/03/18 Nakagawa Add End
            If type = 0 Then        '展開～変形
                ' 2017/03/18 Nakagawa Add Start
                Me.GroupBoxSegPmfBzi.Visibility = True
                ' 2017/03/18 Nakagawa Add End
                Me.GroupBoxSort.Visibility = True
                ' 2017/03/18 Nakagawa Edit Start
                'Me.Height = Me.Height - 102.5
                'Me.ButtonDev2SortOk.SetValue(Grid.RowProperty, 8)
                'Me.ButtonDev2SortCansel.SetValue(Grid.RowProperty, 8)
                Me.Height = Me.Height - 152.5
                Me.GridDev2Sort.RowDefinitions.Item(8).Height = New GridLength(10, GridUnitType.Pixel)
                Me.GridDev2Sort.RowDefinitions.Item(9).Height = New GridLength(1.0, GridUnitType.Pixel)
                Me.GridDev2Sort.RowDefinitions.Item(10).Height = New GridLength(30, GridUnitType.Pixel)
                Me.ButtonDev2SortOk.SetValue(Grid.RowProperty, 10)
                Me.ButtonDev2SortCansel.SetValue(Grid.RowProperty, 10)
                ' 2017/03/18 Nakagawa Edit End
            ElseIf type = 1 Then    '分類
                Me.GroupBoxDevelop.Visibility = True
                Me.GroupBoxZokusei.Visibility = True
                Me.GroupBoxDeform.Visibility = True

                Me.GridDev2Sort.RowDefinitions.Item(2).Height = New GridLength(102.5, GridUnitType.Pixel)
                Me.GroupBoxSort.SetValue(Grid.RowProperty, 2)
                ' 2017/03/18 Nakagawa Edit Start
                'Me.Height = Me.Height - 415
                'Me.ButtonDev2SortOk.SetValue(Grid.RowProperty, 4)
                'Me.ButtonDev2SortCansel.SetValue(Grid.RowProperty, 4)
                Me.Height = Me.Height - 465
                Me.GridDev2Sort.RowDefinitions.Item(4).Height = New GridLength(30, GridUnitType.Pixel)
                Me.ButtonDev2SortOk.SetValue(Grid.RowProperty, 4)
                Me.ButtonDev2SortCansel.SetValue(Grid.RowProperty, 4)
                ' 2017/03/18 Nakagawa Edit End
                ' 2017/03/18 Nakagawa Add Start
            ElseIf type = 2 Then    '展開～分類
                Me.GroupBoxSegPmfBzi.Visibility = True

                Me.Height = Me.Height - 50
                Me.GridDev2Sort.RowDefinitions.Item(8).Height = New GridLength(102.5, GridUnitType.Pixel)
                Me.GroupBoxSort.SetValue(Grid.RowProperty, 8)
                Me.GridDev2Sort.RowDefinitions.Item(9).Height = New GridLength(10, GridUnitType.Pixel)
                Me.GridDev2Sort.RowDefinitions.Item(10).Height = New GridLength(1.0, GridUnitType.Pixel)
                Me.GridDev2Sort.RowDefinitions.Item(11).Height = New GridLength(30, GridUnitType.Pixel)
                Me.ButtonDev2SortOk.SetValue(Grid.RowProperty, 11)
                Me.ButtonDev2SortCansel.SetValue(Grid.RowProperty, 11)
            End If
        Else
            Dim isDifine As Boolean = isDifineSePmfBzi(kojiFolder)
            If isDifine = True Then
                Me.CheckBoxSegPmfBzi.IsChecked = True
                Me.CheckBoxSegPmfBzi.IsEnabled = True
                Me.TextSegPmfBzi.IsEnabled = True
            Else
                Me.CheckBoxSegPmfBzi.IsChecked = False
                Me.CheckBoxSegPmfBzi.IsEnabled = False
                Me.TextSegPmfBzi.IsEnabled = False
            End If
            If type = 0 Then        '展開～変形
                Me.GroupBoxSort.Visibility = True

                Me.Height = Me.Height - 102.5
                Me.ButtonDev2SortOk.SetValue(Grid.RowProperty, 10)
                Me.ButtonDev2SortCansel.SetValue(Grid.RowProperty, 10)
            ElseIf type = 1 Then    '分類
                Me.GroupBoxDevelop.Visibility = True
                Me.GroupBoxZokusei.Visibility = True
                Me.GroupBoxDeform.Visibility = True
                Me.GroupBoxSegPmfBzi.Visibility = True

                Me.GridDev2Sort.RowDefinitions.Item(2).Height = New GridLength(102.5, GridUnitType.Pixel)
                Me.GroupBoxSort.SetValue(Grid.RowProperty, 2)
                Me.Height = Me.Height - 465
                Me.ButtonDev2SortOk.SetValue(Grid.RowProperty, 4)
                Me.ButtonDev2SortCansel.SetValue(Grid.RowProperty, 4)
            End If
        End If
        ' 2017/03/18 Nakagawa Add End

    End Sub

    ' 2017/03/18 Nakagawa Add Start
    Private Function isDifineSePmfBzi(kojiFolder As String) As Boolean
        Dim fileName As String = kojiFolder.TrimEnd + "\SegData.dat"
        Dim isFoundTouroku As Boolean = False
        Dim isFoundHaichi As Boolean = False
        If System.IO.File.Exists(fileName) = True Then
            Dim datFile As New StreamReader(fileName, Encoding.GetEncoding("Shift_JIS"))
            While (datFile.Peek() >= 0)
                Dim lineText As String = datFile.ReadLine().Trim()
                removeComment(lineText)
                If lineText = "" Then
                    Continue While
                End If

                If lineText.IndexOf("SEG-PMFBZI-TOUROKU") <> -1 Then
                    isFoundTouroku = True
                End If
                If lineText.IndexOf("SEG-PMFBZI-HAICHI") <> -1 Then
                    isFoundHaichi = True
                End If
                If isFoundTouroku And isFoundHaichi Then
                    Return True
                End If
            End While
            datFile.Close()
        End If

        Return False
    End Function
    ' 2017/03/18 Nakagawa Add End

    Private Sub setCheckBoxSelectStd()

        ' 展開関連
        If Me.GroupBoxDevelop.Visibility = False Then
            Me.CheckBoxDevelop.IsChecked = True
            Me.CheckBoxWeldStage.IsChecked = False
        End If

        '属性付加関連
        If Me.GroupBoxZokusei.Visibility = False Then
            If System.IO.File.Exists(Me._settingFile._kakFileName) = True Then
                Me.CheckBoxKak.IsChecked = True
            End If
            If System.IO.File.Exists(Me._settingFile._syuFileName) = True Then
                Me.CheckBoxSyu.IsChecked = True
            End If
            If System.IO.File.Exists(Me._settingFile._lotFileName) = True Then
                Me.CheckBoxLot.IsChecked = True
            End If
            Me.CheckBoxKub.IsChecked = False
            Me.CheckBoxSpl.IsChecked = False
            Me.CheckBoxAna.IsChecked = False
            Me.CheckBoxKse.IsChecked = False
            Me.CheckBoxThc.IsChecked = False
            Me.CheckBoxHsr.IsChecked = False
            Me.CheckBoxMgk.IsChecked = False
            Me.CheckBoxPra.IsChecked = False
        End If

        '変形関連
        If Me.GroupBoxDeform.Visibility = False Then
            Me.CheckBoxDeform.IsChecked = True
            If System.IO.File.Exists(Me._settingFile._pmfHoseiFileName) = True Then
                Me.CheckBoxPmfHosei.IsChecked = True
            End If
        End If

        '分類関連
        If Me.GroupBoxSort.Visibility = False Then
            Me.CheckBoxPsort.IsChecked = True
            Me.CheckBoxBsort.IsChecked = True
            Me.CheckBoxMarking.IsChecked = True
        End If

    End Sub

    Private Sub OnClick_ButtonDev2SortSelectStd(sender As Object, e As RoutedEventArgs) Handles ButtonDev2SortSelectStd.Click

        Me.setCheckBoxSelectStd()

    End Sub

    Private Sub OnClick_ButtonDev2SortSelectAll(sender As Object, e As RoutedEventArgs) Handles ButtonDev2SortSelectAll.Click

        ' 展開関連
        If Me.GroupBoxDevelop.Visibility = False Then
            Me.CheckBoxDevelop.IsChecked = True
            If System.IO.File.Exists(Me._settingFile._weldStageFileName) = True Then
                Me.CheckBoxWeldStage.IsChecked = True
            End If
        End If

        '属性付加関連
        If Me.GroupBoxZokusei.Visibility = False Then
            If System.IO.File.Exists(Me._settingFile._kakFileName) = True Then
                Me.CheckBoxKak.IsChecked = True
            End If
            If System.IO.File.Exists(Me._settingFile._syuFileName) = True Then
                Me.CheckBoxSyu.IsChecked = True
            End If
            If System.IO.File.Exists(Me._settingFile._lotFileName) = True Then
                Me.CheckBoxLot.IsChecked = True
            End If
            If System.IO.File.Exists(Me._settingFile._kubFileName) = True Then
                Me.CheckBoxKub.IsChecked = True
            End If
            If System.IO.File.Exists(Me._settingFile._splFileName) = True Then
                Me.CheckBoxSpl.IsChecked = True
            End If
            If System.IO.File.Exists(Me._settingFile._anaFileName) = True Then
                Me.CheckBoxAna.IsChecked = True
            End If
            If System.IO.File.Exists(Me._settingFile._kseFileName) = True Then
                Me.CheckBoxKse.IsChecked = True
            End If
            If System.IO.File.Exists(Me._settingFile._cutOkFileName) = True Then
                Me.CheckBoxThc.IsChecked = True
            End If
            If System.IO.File.Exists(Me._settingFile._hsrFileName) = True Then
                Me.CheckBoxHsr.IsChecked = True
            End If
            If System.IO.File.Exists(Me._settingFile._mgkFileName) = True Then
                Me.CheckBoxMgk.IsChecked = True
            End If
            If System.IO.File.Exists(Me._settingFile._praFileName) = True Then
                Me.CheckBoxPra.IsChecked = True
            End If
        End If

        '変形関連
        If Me.GroupBoxDeform.Visibility = False Then
            Me.CheckBoxDeform.IsChecked = True
            If System.IO.File.Exists(Me._settingFile._pmfHoseiFileName) = True Then
                Me.CheckBoxPmfHosei.IsChecked = True
            End If
        End If

        ' 2017/03/18 Nakagawa Add Start
        '鋼製セグメント関連
        If Me.GroupBoxSegPmfBzi.Visibility = False Then
            Me.CheckBoxSegPmfBzi.IsChecked = True
        End If
        ' 2017/03/18 Nakagawa Add End

        '分類関連
        If Me.GroupBoxSort.Visibility = False Then
            Me.CheckBoxPsort.IsChecked = True
            Me.CheckBoxBsort.IsChecked = True
            Me.CheckBoxMarking.IsChecked = True
        End If

    End Sub

    Private Sub OnClick_ButtonDev2SortAllDel(sender As Object, e As RoutedEventArgs) Handles ButtonDev2SortAllDel.Click

        ' 展開関連
        If Me.GroupBoxDevelop.Visibility = False Then
            Me.CheckBoxDevelop.IsChecked = False
            Me.CheckBoxWeldStage.IsChecked = False
        End If

        '属性付加関連
        If Me.GroupBoxZokusei.Visibility = False Then
            Me.CheckBoxKak.IsChecked = False
            Me.CheckBoxSyu.IsChecked = False
            Me.CheckBoxLot.IsChecked = False
            Me.CheckBoxKub.IsChecked = False
            Me.CheckBoxSpl.IsChecked = False
            Me.CheckBoxAna.IsChecked = False
            Me.CheckBoxKse.IsChecked = False
            Me.CheckBoxThc.IsChecked = False
            Me.CheckBoxHsr.IsChecked = False
            Me.CheckBoxMgk.IsChecked = False
            Me.CheckBoxPra.IsChecked = False
        End If

        '変形関連
        If Me.GroupBoxDeform.Visibility = False Then
            Me.CheckBoxDeform.IsChecked = False
            Me.CheckBoxPmfHosei.IsChecked = False
        End If

        ' 2017/03/18 Nakagawa Add Start
        '鋼製セグメント関連
        If Me.GroupBoxSegPmfBzi.Visibility = False Then
            Me.CheckBoxSegPmfBzi.IsChecked = False
        End If
        ' 2017/03/18 Nakagawa Add End

        '分類関連
        If Me.GroupBoxSort.Visibility = False Then
            Me.CheckBoxPsort.IsChecked = False
            Me.CheckBoxBsort.IsChecked = False
            Me.CheckBoxMarking.IsChecked = False
        End If

    End Sub

    Private Sub OnClick_ButtonDev2SortCansel(sender As Object, e As RoutedEventArgs) Handles ButtonDev2SortCansel.Click

        _isOkCanselError = 1
        Me.Close()

    End Sub

    Private Function createJupiterDat() As Boolean

        Dim isChecked As Boolean = False

        Dim jupiterWriter As New StreamWriter(Me._settingFile._jupiterDatFileName, False, Encoding.GetEncoding("Shift_JIS"))
        '加工代
        If Me.CheckBoxKak.IsChecked = True And System.IO.File.Exists(Me._settingFile._kakFileName) = True Then
            jupiterWriter.WriteLine("JUPITER.KAK")
            isChecked = True
        End If
        '収縮
        If Me.CheckBoxSyu.IsChecked = True And System.IO.File.Exists(Me._settingFile._syuFileName) = True Then
            jupiterWriter.WriteLine("JUPITER.SYU")
            isChecked = True
        End If
        'ロット
        If Me.CheckBoxLot.IsChecked = True And System.IO.File.Exists(Me._settingFile._lotFileName) = True Then
            jupiterWriter.WriteLine("JUPITER.LOT")
            isChecked = True
        End If
        '製作区分
        If Me.CheckBoxKub.IsChecked = True And System.IO.File.Exists(Me._settingFile._kubFileName) = True Then
            jupiterWriter.WriteLine("JUPITER.KUB")
            isChecked = True
        End If
        '実測データ
        If Me.CheckBoxSpl.IsChecked = True And System.IO.File.Exists(Me._settingFile._splFileName) = True Then
            jupiterWriter.WriteLine("JUPITER.SPL")
            isChecked = True
        End If
        '孔明け方法
        If Me.CheckBoxAna.IsChecked = True And System.IO.File.Exists(Me._settingFile._anaFileName) = True Then
            jupiterWriter.WriteLine("JUPITER.ANA")
            isChecked = True
        End If
        '罫書・切断方法
        If Me.CheckBoxKse.IsChecked = True And System.IO.File.Exists(Me._settingFile._kseFileName) = True Then
            jupiterWriter.WriteLine("JUPITER.KSE")
            isChecked = True
        End If
        '切断可能板厚設定
        If Me.CheckBoxThc.IsChecked = True And System.IO.File.Exists(Me._settingFile._cutOkFileName) = True Then
            jupiterWriter.WriteLine("CUTOK.SET")
            isChecked = True
        End If
        '表面処理
        If Me.CheckBoxHsr.IsChecked = True And System.IO.File.Exists(Me._settingFile._hsrFileName) = True Then
            jupiterWriter.WriteLine("JUPITER.HSR")
            isChecked = True
        End If
        '摩擦面剥離
        If Me.CheckBoxMgk.IsChecked = True And System.IO.File.Exists(Me._settingFile._mgkFileName) = True Then
            jupiterWriter.WriteLine("JUPITER.MGK")
            isChecked = True
        End If
        'ソーティングパラメータ
        If Me.CheckBoxPra.IsChecked = True And System.IO.File.Exists(Me._settingFile._praFileName) = True Then
            jupiterWriter.WriteLine("JUPITER.PRA")
            isChecked = True
        End If

        jupiterWriter.Close()

        Return isChecked

    End Function

    Private Sub OnClick_ButtonDev2SortOk(sender As Object, e As RoutedEventArgs) Handles ButtonDev2SortOk.Click

        Dim errMsg As String = ""
        If Me._type = 0 Then        '展開～変形までを実行
            Dim isRunDevelop As Boolean = Me.CheckBoxDevelop.IsChecked
            Dim isRunWeldStage As Boolean = Me.CheckBoxWeldStage.IsChecked
            Dim isRunZok As Boolean = Me.createJupiterDat()
            Dim isRunDeform As Boolean = Me.CheckBoxDeform.IsChecked
            Dim isRunPmfHosei As Boolean = Me.CheckBoxPmfHosei.IsChecked
            ' 2017/03/18 Nakagawa Edit Start
            'createDev2SortScr(Me._kojiFolder, isRunDevelop, isRunWeldStage, isRunZok, isRunDeform, isRunPmfHosei, _
            '                  False, False, False, Me._scrFileName, errMsg)
            Dim isRunSegPmfBzi As Boolean = Me.CheckBoxSegPmfBzi.IsChecked
            createDev2SortScr(Me._kojiFolder, isRunDevelop, isRunWeldStage, isRunZok, isRunDeform, isRunPmfHosei, isRunSegPmfBzi, _
                              False, False, False, Me._scrFileName, errMsg)
            ' 2017/03/18 Nakagawa Edit End
        ElseIf Me._type = 1 Then    '分類を実行
            Dim isRunPsort As Boolean = Me.CheckBoxPsort.IsChecked
            Dim isRunBsort As Boolean = Me.CheckBoxBsort.IsChecked
            Dim isRunMarking As Boolean = Me.CheckBoxMarking.IsChecked
            ' 2017/03/18 Nakagawa Edit Start
            'createDev2SortScr(Me._kojiFolder, False, False, False, False, False, _
            '                  isRunPsort, isRunBsort, isRunMarking, Me._scrFileName, errMsg)
            createDev2SortScr(Me._kojiFolder, False, False, False, False, False, False, _
                              isRunPsort, isRunBsort, isRunMarking, Me._scrFileName, errMsg)
            ' 2017/03/18 Nakagawa Edit End
        ElseIf Me._type = 2 Then    '展開～分類までを実行
            Dim isRunDevelop As Boolean = Me.CheckBoxDevelop.IsChecked
            Dim isRunWeldStage As Boolean = Me.CheckBoxWeldStage.IsChecked
            Dim isRunZok As Boolean = Me.createJupiterDat()
            Dim isRunDeform As Boolean = Me.CheckBoxDeform.IsChecked
            Dim isRunPmfHosei As Boolean = Me.CheckBoxPmfHosei.IsChecked
            ' 2017/03/18 Nakagawa Add Start
            Dim isRunSegPmfBzi As Boolean = Me.CheckBoxSegPmfBzi.IsChecked
            ' 2017/03/18 Nakagawa Add End
            Dim isRunPsort As Boolean = Me.CheckBoxPsort.IsChecked
            Dim isRunBsort As Boolean = Me.CheckBoxBsort.IsChecked
            Dim isRunMarking As Boolean = Me.CheckBoxMarking.IsChecked
            ' 2017/03/18 Nakagawa Edit Start
            'createDev2SortScr(Me._kojiFolder, isRunDevelop, isRunWeldStage, isRunZok, isRunDeform, isRunPmfHosei, _
            '                  isRunPsort, isRunBsort, isRunMarking, Me._scrFileName, errMsg)
            createDev2SortScr(Me._kojiFolder, isRunDevelop, isRunWeldStage, isRunZok, isRunDeform, isRunPmfHosei, isRunSegPmfBzi, _
                              isRunPsort, isRunBsort, isRunMarking, Me._scrFileName, errMsg)
            ' 2017/03/18 Nakagawa Edit End
        End If

        If errMsg <> "" Then
            MessageBox.Show(errMsg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            _isOkCanselError = 2
        Else
            _isOkCanselError = 0
        End If

        Me.Close()

    End Sub
End Class
