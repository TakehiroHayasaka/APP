<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmReg_hiroshima
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

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.dgKihonInfo = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgSokutechi = New System.Windows.Forms.DataGridView()
        Me.TableLayoutPanel7 = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnGetFolderLocal = New System.Windows.Forms.Button()
        Me.txtGetFolderLocal = New System.Windows.Forms.TextBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.CmbSelectExcelTmplate = New System.Windows.Forms.ComboBox()
        Me.col1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.dgKihonInfo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgSokutechi, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel7.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.535509!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 96.73705!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.535509!))
        Me.TableLayoutPanel1.Controls.Add(Me.dgKihonInfo, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.dgSokutechi, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel7, 1, 6)
        Me.TableLayoutPanel1.Controls.Add(Me.GroupBox1, 1, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.GroupBox2, 1, 4)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(4)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 7
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1.449275!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.61539!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1.444043!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 56.15385!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.07692!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.46154!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(521, 562)
        Me.TableLayoutPanel1.TabIndex = 0
        Me.TableLayoutPanel1.Tag = "D:\tuvshin-work\20150731割込み\Data130826165926560955"
        '
        'dgKihonInfo
        '
        Me.dgKihonInfo.AllowUserToAddRows = False
        Me.dgKihonInfo.ColumnHeadersHeight = 5
        Me.dgKihonInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgKihonInfo.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2})
        Me.dgKihonInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgKihonInfo.Location = New System.Drawing.Point(11, 10)
        Me.dgKihonInfo.Name = "dgKihonInfo"
        Me.dgKihonInfo.RowHeadersWidth = 5
        Me.dgKihonInfo.RowTemplate.Height = 21
        Me.dgKihonInfo.Size = New System.Drawing.Size(498, 70)
        Me.dgKihonInfo.TabIndex = 0
        '
        'Column1
        '
        Me.Column1.HeaderText = ""
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Width = 235
        '
        'Column2
        '
        Me.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.Column2.HeaderText = ""
        Me.Column2.Name = "Column2"
        '
        'dgSokutechi
        '
        Me.dgSokutechi.AllowUserToAddRows = False
        Me.dgSokutechi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgSokutechi.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.col1, Me.col2})
        Me.dgSokutechi.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgSokutechi.Location = New System.Drawing.Point(11, 93)
        Me.dgSokutechi.Name = "dgSokutechi"
        Me.dgSokutechi.RowHeadersWidth = 5
        Me.dgSokutechi.RowTemplate.Height = 21
        Me.dgSokutechi.Size = New System.Drawing.Size(498, 286)
        Me.dgSokutechi.TabIndex = 1
        '
        'TableLayoutPanel7
        '
        Me.TableLayoutPanel7.ColumnCount = 4
        Me.TableLayoutPanel7.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.53846!))
        Me.TableLayoutPanel7.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.53846!))
        Me.TableLayoutPanel7.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.927711!))
        Me.TableLayoutPanel7.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel7.Controls.Add(Me.btnCancel, 3, 0)
        Me.TableLayoutPanel7.Controls.Add(Me.btnPrint, 1, 0)
        Me.TableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.TableLayoutPanel7.Location = New System.Drawing.Point(11, 525)
        Me.TableLayoutPanel7.Name = "TableLayoutPanel7"
        Me.TableLayoutPanel7.RowCount = 1
        Me.TableLayoutPanel7.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel7.Size = New System.Drawing.Size(498, 34)
        Me.TableLayoutPanel7.TabIndex = 7
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.btnCancel.Location = New System.Drawing.Point(419, 5)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.btnPrint.Location = New System.Drawing.Point(259, 5)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(143, 23)
        Me.btnPrint.TabIndex = 2
        Me.btnPrint.Text = "測定結果表示・登録"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnGetFolderLocal)
        Me.GroupBox1.Controls.Add(Me.txtGetFolderLocal)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(11, 453)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(498, 64)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "登録先フォルダ"
        '
        'btnGetFolderLocal
        '
        Me.btnGetFolderLocal.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGetFolderLocal.Location = New System.Drawing.Point(425, 28)
        Me.btnGetFolderLocal.Name = "btnGetFolderLocal"
        Me.btnGetFolderLocal.Size = New System.Drawing.Size(67, 23)
        Me.btnGetFolderLocal.TabIndex = 2
        Me.btnGetFolderLocal.Text = "参照..."
        Me.btnGetFolderLocal.UseVisualStyleBackColor = True
        '
        'txtGetFolderLocal
        '
        Me.txtGetFolderLocal.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtGetFolderLocal.Location = New System.Drawing.Point(6, 28)
        Me.txtGetFolderLocal.Name = "txtGetFolderLocal"
        Me.txtGetFolderLocal.Size = New System.Drawing.Size(413, 23)
        Me.txtGetFolderLocal.TabIndex = 1
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.CmbSelectExcelTmplate)
        Me.GroupBox2.Location = New System.Drawing.Point(11, 385)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(498, 62)
        Me.GroupBox2.TabIndex = 9
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "測定結果Excelテンプレート選択"
        '
        'CmbSelectExcelTmplate
        '
        Me.CmbSelectExcelTmplate.FormattingEnabled = True
        Me.CmbSelectExcelTmplate.Location = New System.Drawing.Point(6, 26)
        Me.CmbSelectExcelTmplate.Name = "CmbSelectExcelTmplate"
        Me.CmbSelectExcelTmplate.Size = New System.Drawing.Size(486, 24)
        Me.CmbSelectExcelTmplate.TabIndex = 0
        '
        'col1
        '
        Me.col1.HeaderText = "測定箇所"
        Me.col1.Name = "col1"
        Me.col1.ReadOnly = True
        Me.col1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.col1.Width = 235
        '
        'col2
        '
        Me.col2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle1.Format = "N0"
        DataGridViewCellStyle1.NullValue = Nothing
        Me.col2.DefaultCellStyle = DataGridViewCellStyle1
        Me.col2.HeaderText = "測定値"
        Me.col2.Name = "col2"
        Me.col2.ReadOnly = True
        Me.col2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'FrmReg_hiroshima
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(521, 562)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MinimumSize = New System.Drawing.Size(400, 600)
        Me.Name = "FrmReg_hiroshima"
        Me.Tag = "D:\tuvshin-work\20150731割込み\Data130826165926560955"
        Me.Text = "測定値 印刷・登録"
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.dgKihonInfo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgSokutechi, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel7.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents dgKihonInfo As System.Windows.Forms.DataGridView
    Friend WithEvents dgSokutechi As System.Windows.Forms.DataGridView
    Friend WithEvents btnGetFolderLocal As System.Windows.Forms.Button
    Friend WithEvents txtGetFolderLocal As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents TableLayoutPanel7 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents CmbSelectExcelTmplate As System.Windows.Forms.ComboBox
    Friend WithEvents col1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents col2 As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
