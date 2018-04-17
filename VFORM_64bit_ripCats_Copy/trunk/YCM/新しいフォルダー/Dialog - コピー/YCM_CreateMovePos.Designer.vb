<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class YCM_CreateMovePos
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使用して変更できます。  
    'コード エディタを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Button_Del2 = New System.Windows.Forms.Button()
        Me.Button_Add2 = New System.Windows.Forms.Button()
        Me.DGV_MoveList_2Pos = New System.Windows.Forms.DataGridView()
        Me.Check = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.StartPos = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.EndPos = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MoveValue = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BasePos = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NewPos = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Message = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.Button_Del3 = New System.Windows.Forms.Button()
        Me.Button_Add3 = New System.Windows.Forms.Button()
        Me.DGV_MoveList_3Pos = New System.Windows.Forms.DataGridView()
        Me.DataGridViewCheckBoxColumn1 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.Pos1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Pos2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Pos3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MoveValue3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BasePos3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NewPos3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Message3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.Button_Del2C = New System.Windows.Forms.Button()
        Me.Button_Add2C = New System.Windows.Forms.Button()
        Me.DGV_MoveList_2C = New System.Windows.Forms.DataGridView()
        Me.DataGridViewCheckBoxColumn2 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.DGV_MoveList_2Pos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        CType(Me.DGV_MoveList_3Pos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage3.SuspendLayout()
        CType(Me.DGV_MoveList_2C, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Location = New System.Drawing.Point(8, 11)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(692, 587)
        Me.TabControl1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Button_Del2)
        Me.TabPage1.Controls.Add(Me.Button_Add2)
        Me.TabPage1.Controls.Add(Me.DGV_MoveList_2Pos)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(684, 561)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "2点指定移動点"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Button_Del2
        '
        Me.Button_Del2.Location = New System.Drawing.Point(97, 427)
        Me.Button_Del2.Name = "Button_Del2"
        Me.Button_Del2.Size = New System.Drawing.Size(95, 24)
        Me.Button_Del2.TabIndex = 6
        Me.Button_Del2.Text = "選択行の削除"
        Me.Button_Del2.UseVisualStyleBackColor = True
        '
        'Button_Add2
        '
        Me.Button_Add2.Location = New System.Drawing.Point(11, 427)
        Me.Button_Add2.Name = "Button_Add2"
        Me.Button_Add2.Size = New System.Drawing.Size(80, 24)
        Me.Button_Add2.TabIndex = 5
        Me.Button_Add2.Text = "追加"
        Me.Button_Add2.UseVisualStyleBackColor = True
        '
        'DGV_MoveList_2Pos
        '
        Me.DGV_MoveList_2Pos.AllowUserToAddRows = False
        Me.DGV_MoveList_2Pos.AllowUserToDeleteRows = False
        Me.DGV_MoveList_2Pos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGV_MoveList_2Pos.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Check, Me.StartPos, Me.EndPos, Me.MoveValue, Me.BasePos, Me.NewPos, Me.Message})
        Me.DGV_MoveList_2Pos.Location = New System.Drawing.Point(0, 6)
        Me.DGV_MoveList_2Pos.Name = "DGV_MoveList_2Pos"
        Me.DGV_MoveList_2Pos.RowHeadersVisible = False
        Me.DGV_MoveList_2Pos.RowTemplate.Height = 21
        Me.DGV_MoveList_2Pos.Size = New System.Drawing.Size(675, 415)
        Me.DGV_MoveList_2Pos.TabIndex = 2
        '
        'Check
        '
        Me.Check.HeaderText = ""
        Me.Check.Name = "Check"
        Me.Check.Width = 40
        '
        'StartPos
        '
        Me.StartPos.HeaderText = "始点"
        Me.StartPos.Name = "StartPos"
        Me.StartPos.ReadOnly = True
        Me.StartPos.Width = 80
        '
        'EndPos
        '
        Me.EndPos.HeaderText = "終点"
        Me.EndPos.Name = "EndPos"
        Me.EndPos.ReadOnly = True
        Me.EndPos.Width = 80
        '
        'MoveValue
        '
        Me.MoveValue.HeaderText = "移動量"
        Me.MoveValue.Name = "MoveValue"
        Me.MoveValue.ReadOnly = True
        Me.MoveValue.Width = 70
        '
        'BasePos
        '
        Me.BasePos.HeaderText = "基準点"
        Me.BasePos.Name = "BasePos"
        Me.BasePos.ReadOnly = True
        Me.BasePos.Width = 80
        '
        'NewPos
        '
        Me.NewPos.HeaderText = "新計測点"
        Me.NewPos.Name = "NewPos"
        Me.NewPos.ReadOnly = True
        Me.NewPos.Width = 80
        '
        'Message
        '
        Me.Message.HeaderText = "実行結果"
        Me.Message.Name = "Message"
        Me.Message.ReadOnly = True
        Me.Message.Width = 200
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.Button_Del3)
        Me.TabPage2.Controls.Add(Me.Button_Add3)
        Me.TabPage2.Controls.Add(Me.DGV_MoveList_3Pos)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(684, 561)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "3点指定移動点"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'Button_Del3
        '
        Me.Button_Del3.Location = New System.Drawing.Point(97, 442)
        Me.Button_Del3.Name = "Button_Del3"
        Me.Button_Del3.Size = New System.Drawing.Size(95, 24)
        Me.Button_Del3.TabIndex = 7
        Me.Button_Del3.Text = "選択行の削除"
        Me.Button_Del3.UseVisualStyleBackColor = True
        '
        'Button_Add3
        '
        Me.Button_Add3.Location = New System.Drawing.Point(11, 442)
        Me.Button_Add3.Name = "Button_Add3"
        Me.Button_Add3.Size = New System.Drawing.Size(80, 24)
        Me.Button_Add3.TabIndex = 4
        Me.Button_Add3.Text = "追加"
        Me.Button_Add3.UseVisualStyleBackColor = True
        '
        'DGV_MoveList_3Pos
        '
        Me.DGV_MoveList_3Pos.AllowUserToAddRows = False
        Me.DGV_MoveList_3Pos.AllowUserToDeleteRows = False
        Me.DGV_MoveList_3Pos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGV_MoveList_3Pos.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewCheckBoxColumn1, Me.Pos1, Me.Pos2, Me.Pos3, Me.MoveValue3, Me.BasePos3, Me.NewPos3, Me.Message3})
        Me.DGV_MoveList_3Pos.Location = New System.Drawing.Point(0, 6)
        Me.DGV_MoveList_3Pos.Name = "DGV_MoveList_3Pos"
        Me.DGV_MoveList_3Pos.RowHeadersVisible = False
        Me.DGV_MoveList_3Pos.RowTemplate.Height = 21
        Me.DGV_MoveList_3Pos.Size = New System.Drawing.Size(675, 430)
        Me.DGV_MoveList_3Pos.TabIndex = 3
        '
        'DataGridViewCheckBoxColumn1
        '
        Me.DataGridViewCheckBoxColumn1.HeaderText = ""
        Me.DataGridViewCheckBoxColumn1.Name = "DataGridViewCheckBoxColumn1"
        Me.DataGridViewCheckBoxColumn1.Width = 40
        '
        'Pos1
        '
        Me.Pos1.HeaderText = "点1"
        Me.Pos1.Name = "Pos1"
        Me.Pos1.Width = 60
        '
        'Pos2
        '
        Me.Pos2.HeaderText = "点2"
        Me.Pos2.Name = "Pos2"
        Me.Pos2.Width = 60
        '
        'Pos3
        '
        Me.Pos3.HeaderText = "点3"
        Me.Pos3.Name = "Pos3"
        Me.Pos3.Width = 60
        '
        'MoveValue3
        '
        Me.MoveValue3.HeaderText = "移動量"
        Me.MoveValue3.Name = "MoveValue3"
        Me.MoveValue3.Width = 70
        '
        'BasePos3
        '
        Me.BasePos3.HeaderText = "基準点"
        Me.BasePos3.Name = "BasePos3"
        Me.BasePos3.Width = 80
        '
        'NewPos3
        '
        Me.NewPos3.HeaderText = "新計測点"
        Me.NewPos3.Name = "NewPos3"
        Me.NewPos3.Width = 80
        '
        'Message3
        '
        Me.Message3.HeaderText = "実行結果"
        Me.Message3.Name = "Message3"
        Me.Message3.ReadOnly = True
        Me.Message3.Width = 200
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.Button_Del2C)
        Me.TabPage3.Controls.Add(Me.Button_Add2C)
        Me.TabPage3.Controls.Add(Me.DGV_MoveList_2C)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(684, 561)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "2点中間点"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'Button_Del2C
        '
        Me.Button_Del2C.Location = New System.Drawing.Point(100, 427)
        Me.Button_Del2C.Name = "Button_Del2C"
        Me.Button_Del2C.Size = New System.Drawing.Size(95, 24)
        Me.Button_Del2C.TabIndex = 9
        Me.Button_Del2C.Text = "選択行の削除"
        Me.Button_Del2C.UseVisualStyleBackColor = True
        '
        'Button_Add2C
        '
        Me.Button_Add2C.Location = New System.Drawing.Point(14, 427)
        Me.Button_Add2C.Name = "Button_Add2C"
        Me.Button_Add2C.Size = New System.Drawing.Size(80, 24)
        Me.Button_Add2C.TabIndex = 8
        Me.Button_Add2C.Text = "追加"
        Me.Button_Add2C.UseVisualStyleBackColor = True
        '
        'DGV_MoveList_2C
        '
        Me.DGV_MoveList_2C.AllowUserToAddRows = False
        Me.DGV_MoveList_2C.AllowUserToDeleteRows = False
        Me.DGV_MoveList_2C.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGV_MoveList_2C.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewCheckBoxColumn2, Me.DataGridViewTextBoxColumn2, Me.DataGridViewTextBoxColumn3, Me.DataGridViewTextBoxColumn6, Me.DataGridViewTextBoxColumn7})
        Me.DGV_MoveList_2C.Location = New System.Drawing.Point(3, 6)
        Me.DGV_MoveList_2C.Name = "DGV_MoveList_2C"
        Me.DGV_MoveList_2C.RowHeadersVisible = False
        Me.DGV_MoveList_2C.RowTemplate.Height = 21
        Me.DGV_MoveList_2C.Size = New System.Drawing.Size(675, 415)
        Me.DGV_MoveList_2C.TabIndex = 7
        '
        'DataGridViewCheckBoxColumn2
        '
        Me.DataGridViewCheckBoxColumn2.HeaderText = ""
        Me.DataGridViewCheckBoxColumn2.Name = "DataGridViewCheckBoxColumn2"
        Me.DataGridViewCheckBoxColumn2.Width = 40
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.HeaderText = "始点"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Width = 80
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.HeaderText = "終点"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.Width = 80
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.HeaderText = "新計測点"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.ReadOnly = True
        Me.DataGridViewTextBoxColumn6.Width = 80
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.HeaderText = "実行結果"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.ReadOnly = True
        Me.DataGridViewTextBoxColumn7.Width = 200
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'YCM_CreateMovePos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(705, 604)
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "YCM_CreateMovePos"
        Me.Text = "移動点の作成"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        CType(Me.DGV_MoveList_2Pos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        CType(Me.DGV_MoveList_3Pos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage3.ResumeLayout(False)
        CType(Me.DGV_MoveList_2C, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents DGV_MoveList_2Pos As System.Windows.Forms.DataGridView
    Friend WithEvents DGV_MoveList_3Pos As System.Windows.Forms.DataGridView
    Friend WithEvents Button_Add2 As System.Windows.Forms.Button
    Friend WithEvents Button_Add3 As System.Windows.Forms.Button
    Friend WithEvents Button_Del2 As System.Windows.Forms.Button
    Friend WithEvents DataGridViewCheckBoxColumn1 As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Pos1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Pos2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Pos3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MoveValue3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents BasePos3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NewPos3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Message3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Button_Del3 As System.Windows.Forms.Button
    Friend WithEvents Check As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents StartPos As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents EndPos As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MoveValue As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents BasePos As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NewPos As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Message As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Button_Del2C As System.Windows.Forms.Button
    Friend WithEvents Button_Add2C As System.Windows.Forms.Button
    Friend WithEvents DGV_MoveList_2C As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridViewCheckBoxColumn2 As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
