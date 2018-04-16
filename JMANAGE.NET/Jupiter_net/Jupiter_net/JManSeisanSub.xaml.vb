Imports System.Windows.Forms

Public Class JManSeisanSub

    '工事フォルダ
    Public _kojiFolder As String = ""
    'Environのフォルダ名
    Private _environFolderName As String = ""
    'ファイルタイプ(ボタンタイプ)
    Public _fileType As Integer = 0
    'ファイル名
    Public _opneFileName As String = ""
    'ボタン情報(帳票関連)
    Private _buttonInfoList As New JManDispButtonInfoList()
    '配置行数
    Private _row As Integer = 0
    Private _maxRow As Integer = 10
    '配置列数
    Private _column As Integer = 0
    Private _maxColumn As Integer = 10

    ' ボタン色設定(0=Gray,1=Green)
    Public _buttonColor As Integer = 1
    ' 背景色設定(0=Gray,1=White)
    Public _backColor As Integer = 1

    Public Sub OnInitialize(kojiFolder As String, environFolderName As String, buttonColor As Integer, backColor As Integer)

        Me._kojiFolder = kojiFolder
        Me._environFolderName = environFolderName
        Me._opneFileName = ""
        Me._buttonInfoList.clearList()
        Me.initWindowChildren()
        Me._buttonColor = buttonColor
        Me._backColor = backColor

        '背景色とボタン色を変更する。
        If backColor = 0 Then
            Me.Background = New SolidColorBrush(System.Windows.Media.Colors.DarkGray)
        Else
            Me.Background = New SolidColorBrush(System.Windows.Media.Colors.White)
        End If
        Dim buttonColor1 As SolidColorBrush
        Dim buttonColor2 As SolidColorBrush
        If _buttonColor = 0 Then
            buttonColor1 = New SolidColorBrush(System.Windows.Media.Colors.Black)
            buttonColor2 = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 90, 90, 90))
        Else
            buttonColor1 = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 164, 150))
            buttonColor2 = New SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 225, 150))
        End If
        Me.ButtonSubSample1.Background = buttonColor1
        Me.ButtonSubSample2.Background = buttonColor2
        Me.ButtonSubSample3.Background = buttonColor1
        Me.ButtonSubSample4.Background = buttonColor2
        Me.ButtonSubSample5.Background = buttonColor1
        Me.ButtonSubSample6.Background = buttonColor2

        ' JmanDisp_*.tbsを読み込む
        Dim disp As JManDispFile = New JManDispFile()
        Dim status As Integer = disp.loadSeisanSubFile()
        If status <> 0 Or disp._window._caption = "" Then
            MessageBox.Show("JmanDispSub_Seisan.wbsが読み込めないため、画面を表示できません。開発元に連絡してください。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Me.Title = disp._window._caption

        'ボタンを配置する。
        Me.createButton(disp._window)
        'グループボックスを配置する。
        Me.createGroupBox(disp._window)
        'テキストを配置する。
        Me.createTextBlock(disp._window)

        'ウインドウズサイズを変更する。
        If Me._row < Me._maxRow Then
            Dim redHeight As Integer = Me._maxRow - Me._row
            Me.Height = Me.Height + 20 - redHeight * 74
        End If
        If Me._column < Me._maxColumn Then
            Dim redWidth As Integer = Me._maxColumn - Me._column
            Me.Width = Me.Width + 10 - redWidth * 77
        End If

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   ウインドウ内の、ボタン、テキスト、グループボックスを全て削除する。
    '---------------------------------------------------------------------------------------------------
    Private Sub initWindowChildren()

        Me.ButtonSubSample1.Visibility = True
        Me.ButtonSubSample2.Visibility = True
        Me.ButtonSubSample3.Visibility = True
        Me.ButtonSubSample4.Visibility = True
        Me.ButtonSubSample5.Visibility = True
        Me.ButtonSubSample6.Visibility = True
        Me.BorderSubSample.Visibility = True
        Me.StackPanelSubSample.Visibility = True

        If Me.GridSeisanSub.Children.Count > 8 Then
            Dim i As Integer = 0
            For i = 8 To Me.GridSeisanSub.Children.Count - 1
                Me.GridSeisanSub.Children.RemoveAt(8)
            Next
        End If

        Me.Height = 810
        Me.Width = 820

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   ボタンを配置する。
    '---------------------------------------------------------------------------------------------------
    Private Sub createButton(disp As JManDispWindow)

        Dim ii As Integer = 0
        Dim jj As Integer = 0
        For ii = 0 To disp._buttonList.Count - 1
            Dim buttonRow As Integer = disp._buttonList(ii)._rowNo          '行数
            Dim buttonColumn As Integer = disp._buttonList(ii)._columnNo    '列数
            Dim buttonNum As Integer = disp._buttonList(ii)._buttonNum      'ボタン数
            Dim buttonWidth As Double = 75.0                                'ボタン幅
            Dim buttonColumnSpan As Integer = 1                             'ボタン表示列数
            Dim buttonMargin As New System.Windows.Thickness()              'ボタン隙間
            If buttonNum = 1 Then
                buttonWidth = 75.0
                buttonColumnSpan = 1
            Else
                buttonMargin.Top = 20.0
            End If
            Dim borderThickness As New System.Windows.Thickness()           'Borderの線厚
            borderThickness.Top = 1
            borderThickness.Bottom = 1
            borderThickness.Left = 1
            borderThickness.Right = 1

            '配置行列数
            If Me._row < buttonRow Then
                Me._row = buttonRow
            End If
            If Me._column < (buttonColumn + buttonNum - 1) Then
                Me._column = buttonColumn + buttonNum - 1
            End If

            For jj = 0 To disp._buttonList(ii)._detailList.Count - 1

                ' ボタンを作成する。
                Dim btnName As String = "Button_" + getAlphabet(buttonRow) + "_" + (buttonColumn + jj).ToString()
                Dim btnCaption As String = ""
                If buttonNum = 1 Then
                    If disp._buttonList(ii)._detailList(jj)._caption = "" Then
                        btnCaption = disp._buttonList(ii)._caption
                    Else
                        btnCaption = disp._buttonList(ii)._caption + vbCrLf + disp._buttonList(ii)._detailList(jj)._caption
                    End If
                Else
                    btnCaption = disp._buttonList(ii)._detailList(jj)._caption
                End If

                Dim btnNew As New System.Windows.Controls.Button()
                btnNew.Name = btnName
                btnNew.Height = 50.0
                btnNew.Width = buttonWidth
                btnNew.SetValue(Grid.RowProperty, 1 + buttonRow)
                btnNew.SetValue(Grid.ColumnProperty, 1 + buttonColumn + jj)
                btnNew.SetValue(Grid.ColumnSpanProperty, buttonColumnSpan)
                btnNew.Foreground = New SolidColorBrush(System.Windows.Media.Colors.White)
                Select Case disp._buttonList(ii)._detailList(jj)._type
                    Case 1
                        btnNew.Style = Me.ButtonSubSample1.Style
                        btnNew.Background = Me.ButtonSubSample1.Background
                    Case 2
                        btnNew.Style = Me.ButtonSubSample2.Style
                        btnNew.Background = Me.ButtonSubSample2.Background
                    Case 3
                        btnNew.Style = Me.ButtonSubSample3.Style
                        btnNew.Background = Me.ButtonSubSample3.Background
                    Case 4
                        btnNew.Style = Me.ButtonSubSample4.Style
                        btnNew.Background = Me.ButtonSubSample4.Background
                    Case 5
                        btnNew.Style = Me.ButtonSubSample5.Style
                        btnNew.Background = Me.ButtonSubSample5.Background
                    Case 6
                        btnNew.Style = Me.ButtonSubSample6.Style
                        btnNew.Background = Me.ButtonSubSample6.Background
                End Select
                btnNew.Content = btnCaption
                btnNew.FontSize = 11.0 * disp._buttonList(ii)._detailList(jj)._textHeigthRatio
                btnNew.Cursor = Me.ButtonSubSample1.Cursor
                btnNew.HorizontalAlignment = Windows.HorizontalAlignment.Center
                If buttonNum = 1 Then
                    btnNew.VerticalAlignment = Windows.VerticalAlignment.Center
                Else
                    btnNew.VerticalAlignment = Windows.VerticalAlignment.Top
                End If
                btnNew.Margin = buttonMargin

                'Gridへ追加
                Me.GridSeisanSub.Children.Add(btnNew)

                'ボタンのクリックイベントの定義
                AddHandler btnNew.Click, AddressOf OnClick_BtnNew

                'JManDispButtonInfoListへ定義を追加
                Dim buttonInfo As New JManDispButtonInfo
                buttonInfo._type = disp._buttonList(ii)._detailList(jj)._type
                buttonInfo._name = btnName
                buttonInfo._fileName = disp._buttonList(ii)._detailList(jj)._fileName
                buttonInfo._argument = disp._buttonList(ii)._detailList(jj)._argument
                _buttonInfoList.append(buttonInfo)
            Next

            'Borderとラベルを作成する。
            If buttonNum <> 1 Then
                Dim borderNew As New Border
                borderNew.Background = New SolidColorBrush(System.Windows.Media.Colors.White)
                borderNew.HorizontalAlignment = Windows.HorizontalAlignment.Stretch
                borderNew.VerticalAlignment = Windows.VerticalAlignment.Top
                borderNew.Height = 20.0
                borderNew.SetValue(Grid.RowProperty, 1 + buttonRow)
                borderNew.SetValue(Grid.ColumnProperty, 1 + buttonColumn)
                borderNew.SetValue(Grid.ColumnSpanProperty, buttonNum)
                borderNew.BorderBrush = New SolidColorBrush(System.Windows.Media.Colors.Gainsboro)
                borderNew.BorderThickness = borderThickness

                'ラベルを作成する。
                Dim labelNew As New System.Windows.Controls.Label()
                labelNew.Content = disp._buttonList(ii)._caption
                labelNew.FontSize = 11.0
                labelNew.FontWeight = Me.LabelSubSample.FontWeight
                labelNew.VerticalAlignment = Windows.VerticalAlignment.Top
                labelNew.HorizontalAlignment = Windows.HorizontalAlignment.Center
                labelNew.Padding = Me.LabelSubSample.Padding
                labelNew.SetValue(Grid.RowProperty, 1 + buttonRow)
                labelNew.SetValue(Grid.ColumnProperty, 1 + buttonColumn)
                labelNew.SetValue(Grid.ColumnSpanProperty, buttonNum)
                labelNew.Height = 20.0

                'Gridへ追加
                Me.GridSeisanSub.Children.Add(borderNew)
                Me.GridSeisanSub.Children.Add(labelNew)

            End If
        Next

    End Sub

    '---------------------- ボタンイベント ----------------------
    Private Sub OnClick_BtnNew(ByVal sender As Object, ByVal e As RoutedEventArgs)

        '_buttonInfoListRepからボタン名に一致するbuttonInfoを取得する。
        Dim button As System.Windows.Controls.Button = sender
        Dim buttonName As String = button.Name
        Dim buttonInfo As New JManDispButtonInfo()
        Dim status As Integer = _buttonInfoList.find(buttonName, buttonInfo)
        If status <> 0 Then
            MessageBox.Show("JmanDispSub_Seisan.wbsが正しく読み込めていないため、実行できません。開発元に連絡してください。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Me._fileType = buttonInfo._type

        Dim buttonFileName As String = buttonInfo._fileName
        If buttonFileName.ToUpper() Like "*.CNT" Then
            Dim cntIn As New JManCntIn()
            cntIn.OnInitialize(Me._kojiFolder, buttonInfo._fileName, buttonInfo._argument, Me._buttonColor, Me._backColor)
            cntIn.ShowDialog()
            Me._opneFileName = cntIn._dataFileName
            If cntIn._isOkCansel = 1 Then
                Return
            End If
        Else
            Me._opneFileName = buttonInfo._fileName
        End If

        If Me._opneFileName = "" Then
            MessageBox.Show("JmanDispSub_Seisan.wbsにファイル名が定義されていません。開発元に連絡してください。", "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If Me._opneFileName.ToUpper() Like "*.EXE" Then
            Dim arg As String = ""
            If buttonInfo._argument = "" Then
                arg = Me._kojiFolder
            Else
                arg = Me._kojiFolder + " " + buttonInfo._argument
            End If
            status = exeStartJupiter(Me._kojiFolder, Me._opneFileName, arg)
            If status <> 0 Then
                Dim msg As String = "実行ファイル[" + Me._opneFileName
                msg += "]が存在しません。開発元に連絡してください。"
                MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
        Else
            Dim textFileNameTmp As String = Me._kojiFolder.TrimEnd("\") + "\" + Me._opneFileName
            If System.IO.File.Exists(textFileNameTmp) <> True Then
                Dim msg As String = ""
                If Me._opneFileName = buttonInfo._argument Then
                    msg = "設定ファイル[" + Me._opneFileName + "]が工事フォルダに存在していません。" + vbCrLf + _
                          "Environフォルダから工事フォルダへコピーしてよろしいですか？"
                ElseIf buttonInfo._argument = "" Then
                    msg = "設定ファイル[" + Me._opneFileName + "]が工事フォルダに存在していません。" + vbCrLf + _
                          "新規で作成してよろしいですか？"
                Else
                    msg = "設定ファイル[" + Me._opneFileName + "]が工事フォルダに存在していません。" + vbCrLf + _
                          "入力データサンプル[" + buttonInfo._argument + "]をEnvironフォルダから工事フォルダへコピーしてよろしいですか？"
                End If
            Dim result As DialogResult = MessageBox.Show(msg, "JupiterManager", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = Forms.DialogResult.No Then
                Return
            End If
        End If
        Dim textFileName As String = copySettingFileFromEnviron(Me._kojiFolder, Me._environFolderName, Me._opneFileName, buttonInfo._argument)
        editorStart(Me._kojiFolder, textFileName, 1)
        End If

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   テキストを配置する。
    '---------------------------------------------------------------------------------------------------
    Private Sub createTextBlock(disp As JManDispWindow)

        Dim ii As Integer = 0
        For ii = 0 To disp._textList.Count - 1
            Dim textRow As Integer = disp._textList(ii)._rowNo          '行数
            Dim textColumn As Integer = disp._textList(ii)._columnNo    '列数
            Dim textNum As Integer = disp._textList(ii)._width          'テキスト列数
            Dim textMargin As New System.Windows.Thickness()            'ボタン隙間
            textMargin.Left = 10.0
            Dim textName As String = "Text_" + getAlphabet(textRow) + "_" + textColumn.ToString()

            '配置行列数
            If Me._row < textRow Then
                Me._row = textRow
            End If
            If Me._column < (textColumn + textNum - 1) Then
                Me._column = textColumn + textNum - 1
            End If

            Dim textPadding As New System.Windows.Thickness()           'Borderの線厚
            textPadding.Top = 0
            textPadding.Bottom = 0
            textPadding.Left = 5
            textPadding.Right = 5

            ' TextBlockを作成する。
            Dim txtBlockNew As New System.Windows.Controls.TextBlock()
            txtBlockNew.Name = textName
            txtBlockNew.SetValue(Grid.RowProperty, 1 + textRow)
            txtBlockNew.SetValue(Grid.ColumnProperty, 1 + textColumn)
            txtBlockNew.SetValue(Grid.ColumnSpanProperty, textNum)
            txtBlockNew.Foreground = New SolidColorBrush(System.Windows.Media.Colors.Black)
            txtBlockNew.Text = disp._textList(ii)._caption
            txtBlockNew.FontSize = 11.0
            txtBlockNew.HorizontalAlignment = Windows.HorizontalAlignment.Left
            txtBlockNew.VerticalAlignment = Windows.VerticalAlignment.Center
            txtBlockNew.Padding = textPadding
            txtBlockNew.FontWeight = Me.LabelSubSample.FontWeight
            txtBlockNew.TextWrapping = Me.TextBlockSubSample.TextWrapping

            'Gridへ追加
            Me.GridSeisanSub.Children.Add(txtBlockNew)

        Next

    End Sub

    '---------------------------------------------------------------------------------------------------
    '機能
    '   帳票関連タブにグループボックスを配置する。
    '---------------------------------------------------------------------------------------------------
    Private Sub createGroupBox(disp As JManDispWindow)

        Dim ii As Integer = 0
        For ii = 0 To disp._groupBoxList.Count - 1
            Dim groupRow As Integer = disp._groupBoxList(ii)._rowNo          '行数
            Dim groupColumn As Integer = disp._groupBoxList(ii)._columnNo    '列数
            Dim groupHeight As Integer = disp._groupBoxList(ii)._height      'グループボックス高さ
            Dim groupWidth As Integer = disp._groupBoxList(ii)._width        'グループボックス幅
            Dim groupName As String = "Group_" + getAlphabet(groupRow) + "_" + groupColumn.ToString()

            '配置行列数
            If Me._row < (groupRow + groupHeight - 1) Then
                Me._row = groupRow + groupHeight - 1
            End If
            If Me._column < (groupColumn + groupWidth - 1) Then
                Me._column = groupColumn + groupWidth - 1
            End If

            'Marginの設定
            Dim groupMargin As New System.Windows.Thickness()
            groupMargin.Top = 0.0
            groupMargin.Left = 0.0
            If groupRow <> 1 Then
                groupMargin.Top = 49.0
            End If
            If groupColumn <> 1 Then
                groupMargin.Left = 72.0
            End If

            Dim grpBoxNew As New System.Windows.Controls.GroupBox()
            grpBoxNew.Name = groupName
            grpBoxNew.Header = disp._groupBoxList(ii)._caption
            grpBoxNew.SetValue(Grid.RowProperty, groupRow)
            grpBoxNew.SetValue(Grid.RowSpanProperty, groupHeight + 2)
            grpBoxNew.SetValue(Grid.ColumnProperty, groupColumn)
            grpBoxNew.SetValue(Grid.ColumnSpanProperty, groupWidth + 2)
            grpBoxNew.VerticalAlignment = Windows.VerticalAlignment.Top
            grpBoxNew.HorizontalAlignment = Windows.HorizontalAlignment.Left
            grpBoxNew.Height = 74.0 * groupHeight + 30
            grpBoxNew.Width = 77.0 * groupWidth + 20
            grpBoxNew.Margin = groupMargin

            'Gridへ追加
            Me.GridSeisanSub.Children.Add(grpBoxNew)

        Next
    End Sub

End Class
