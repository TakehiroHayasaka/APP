<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class YCM_MovePosList
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
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.OK_Button = New System.Windows.Forms.Button
        Me.DGV_MoveList = New System.Windows.Forms.DataGridView
        Me.Check = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.BasePos = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.MoveValue = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.NewPos = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Message = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.DGV_MoveList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(343, 500)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(203, 27)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(118, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 21)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "キャンセル"
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(17, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 21)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "実行"
        '
        'DGV_MoveList
        '
        Me.DGV_MoveList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGV_MoveList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Check, Me.BasePos, Me.MoveValue, Me.NewPos, Me.Message})
        Me.DGV_MoveList.Location = New System.Drawing.Point(12, 12)
        Me.DGV_MoveList.Name = "DGV_MoveList"
        Me.DGV_MoveList.RowTemplate.Height = 21
        Me.DGV_MoveList.Size = New System.Drawing.Size(534, 480)
        Me.DGV_MoveList.TabIndex = 1
        '
        'Check
        '
        Me.Check.HeaderText = "作成"
        Me.Check.Name = "Check"
        Me.Check.Width = 40
        '
        'BasePos
        '
        Me.BasePos.HeaderText = "基準計測点"
        Me.BasePos.Name = "BasePos"
        Me.BasePos.Width = 90
        '
        'MoveValue
        '
        Me.MoveValue.HeaderText = "移動量"
        Me.MoveValue.Name = "MoveValue"
        Me.MoveValue.Width = 70
        '
        'NewPos
        '
        Me.NewPos.HeaderText = "新計測点"
        Me.NewPos.Name = "NewPos"
        Me.NewPos.Width = 80
        '
        'Message
        '
        Me.Message.HeaderText = "実行結果"
        Me.Message.Name = "Message"
        Me.Message.ReadOnly = True
        Me.Message.Width = 200
        '
        'YCM_MovePosList
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(559, 529)
        Me.Controls.Add(Me.DGV_MoveList)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "YCM_MovePosList"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "YCM_MovePosList"
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.DGV_MoveList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents DGV_MoveList As System.Windows.Forms.DataGridView
    Friend WithEvents Check As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents BasePos As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MoveValue As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NewPos As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Message As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
