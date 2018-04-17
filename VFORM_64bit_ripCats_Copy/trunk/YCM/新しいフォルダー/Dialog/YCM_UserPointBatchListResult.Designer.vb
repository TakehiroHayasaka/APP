<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class YCM_UserPointBatchListResult
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.Button_AllSel = New System.Windows.Forms.Button
        Me.Button_UnSel = New System.Windows.Forms.Button
        Me.Create = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.strMoveDir = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.BasePoint = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.MoveValue = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.NewPoint = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Result = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(453, 411)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(229, 27)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(23, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 21)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "実行"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(138, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 21)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "キャンセル"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Create, Me.strMoveDir, Me.BasePoint, Me.MoveValue, Me.NewPoint, Me.Result})
        Me.DataGridView1.Location = New System.Drawing.Point(12, 12)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowTemplate.Height = 21
        Me.DataGridView1.Size = New System.Drawing.Size(670, 393)
        Me.DataGridView1.TabIndex = 1
        '
        'Button_AllSel
        '
        Me.Button_AllSel.Location = New System.Drawing.Point(27, 414)
        Me.Button_AllSel.Name = "Button_AllSel"
        Me.Button_AllSel.Size = New System.Drawing.Size(55, 22)
        Me.Button_AllSel.TabIndex = 2
        Me.Button_AllSel.Text = "全選択"
        Me.Button_AllSel.UseVisualStyleBackColor = True
        '
        'Button_UnSel
        '
        Me.Button_UnSel.Location = New System.Drawing.Point(97, 414)
        Me.Button_UnSel.Name = "Button_UnSel"
        Me.Button_UnSel.Size = New System.Drawing.Size(49, 23)
        Me.Button_UnSel.TabIndex = 3
        Me.Button_UnSel.Text = "全解除"
        Me.Button_UnSel.UseVisualStyleBackColor = True
        '
        'Create
        '
        Me.Create.Frozen = True
        Me.Create.HeaderText = "作成"
        Me.Create.Name = "Create"
        Me.Create.Width = 50
        '
        'strMoveDir
        '
        Me.strMoveDir.HeaderText = "移動方向"
        Me.strMoveDir.Name = "strMoveDir"
        Me.strMoveDir.ReadOnly = True
        Me.strMoveDir.Width = 130
        '
        'BasePoint
        '
        Me.BasePoint.HeaderText = "基準計測点"
        Me.BasePoint.Name = "BasePoint"
        Me.BasePoint.ReadOnly = True
        Me.BasePoint.Width = 90
        '
        'MoveValue
        '
        Me.MoveValue.HeaderText = "移動量"
        Me.MoveValue.Name = "MoveValue"
        Me.MoveValue.ReadOnly = True
        Me.MoveValue.Width = 70
        '
        'NewPoint
        '
        Me.NewPoint.HeaderText = "新計測点"
        Me.NewPoint.Name = "NewPoint"
        Me.NewPoint.ReadOnly = True
        Me.NewPoint.Width = 80
        '
        'Result
        '
        Me.Result.HeaderText = "実行結果"
        Me.Result.Name = "Result"
        Me.Result.ReadOnly = True
        Me.Result.Width = 250
        '
        'YCM_UserPointBatchListResult
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(694, 449)
        Me.Controls.Add(Me.Button_UnSel)
        Me.Controls.Add(Me.Button_AllSel)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "YCM_UserPointBatchListResult"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "移動点一括処理（処理結果確認）"
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents Button_AllSel As System.Windows.Forms.Button
    Friend WithEvents Button_UnSel As System.Windows.Forms.Button
    Friend WithEvents Create As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents strMoveDir As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents BasePoint As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MoveValue As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NewPoint As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Result As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
