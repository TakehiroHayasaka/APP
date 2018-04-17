<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Data_Vertex
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Btn_AllSelect = New System.Windows.Forms.Button()
        Me.Btn_AllKaijyo = New System.Windows.Forms.Button()
        Me.DGV_DV = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SortID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Btn_Hochi = New System.Windows.Forms.Button()
        Me.Btn_CSOut = New System.Windows.Forms.Button()
        Me.Combo_Child = New System.Windows.Forms.ComboBox()
        Me.Combo_Single = New System.Windows.Forms.ComboBox()
        Me.Combo_Scale = New System.Windows.Forms.ComboBox()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        CType(Me.DGV_DV, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Btn_AllSelect
        '
        Me.Btn_AllSelect.Location = New System.Drawing.Point(11, 12)
        Me.Btn_AllSelect.Name = "Btn_AllSelect"
        Me.Btn_AllSelect.Size = New System.Drawing.Size(60, 23)
        Me.Btn_AllSelect.TabIndex = 0
        Me.Btn_AllSelect.Text = "全選択"
        Me.Btn_AllSelect.UseVisualStyleBackColor = True
        '
        'Btn_AllKaijyo
        '
        Me.Btn_AllKaijyo.Location = New System.Drawing.Point(77, 12)
        Me.Btn_AllKaijyo.Name = "Btn_AllKaijyo"
        Me.Btn_AllKaijyo.Size = New System.Drawing.Size(60, 23)
        Me.Btn_AllKaijyo.TabIndex = 1
        Me.Btn_AllKaijyo.Text = "全解除"
        Me.Btn_AllKaijyo.UseVisualStyleBackColor = True
        '
        'DGV_DV
        '
        Me.DGV_DV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.DGV_DV.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column4, Me.Column5, Me.ID, Me.SortID})
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle6.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.NullValue = Nothing
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DGV_DV.DefaultCellStyle = DataGridViewCellStyle6
        Me.DGV_DV.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DGV_DV.Location = New System.Drawing.Point(0, 0)
        Me.DGV_DV.Name = "DGV_DV"
        Me.DGV_DV.RowHeadersWidth = 20
        Me.DGV_DV.RowTemplate.Height = 21
        Me.DGV_DV.Size = New System.Drawing.Size(437, 446)
        Me.DGV_DV.TabIndex = 2
        '
        'Column1
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.NullValue = "False"
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle1
        Me.Column1.FalseValue = "False"
        Me.Column1.HeaderText = "表示"
        Me.Column1.IndeterminateValue = ""
        Me.Column1.Name = "Column1"
        Me.Column1.TrueValue = "True"
        Me.Column1.Width = 47
        '
        'Column2
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle2
        Me.Column2.HeaderText = " 測点名"
        Me.Column2.Name = "Column2"
        Me.Column2.Width = 63
        '
        'Column3
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle3.Format = "N2"
        DataGridViewCellStyle3.NullValue = Nothing
        Me.Column3.DefaultCellStyle = DataGridViewCellStyle3
        Me.Column3.HeaderText = "　X"
        Me.Column3.Name = "Column3"
        Me.Column3.Width = 82
        '
        'Column4
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle4.Format = "N2"
        DataGridViewCellStyle4.NullValue = Nothing
        Me.Column4.DefaultCellStyle = DataGridViewCellStyle4
        Me.Column4.HeaderText = "　Y"
        Me.Column4.Name = "Column4"
        Me.Column4.Width = 82
        '
        'Column5
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle5.Format = "N2"
        DataGridViewCellStyle5.NullValue = Nothing
        Me.Column5.DefaultCellStyle = DataGridViewCellStyle5
        Me.Column5.HeaderText = "　Z"
        Me.Column5.Name = "Column5"
        Me.Column5.Width = 82
        '
        'ID
        '
        Me.ID.HeaderText = "ID"
        Me.ID.Name = "ID"
        Me.ID.Visible = False
        '
        'SortID
        '
        Me.SortID.HeaderText = "ソート用"
        Me.SortID.Name = "SortID"
        Me.SortID.Visible = False
        Me.SortID.Width = 50
        '
        'Btn_Hochi
        '
        Me.Btn_Hochi.Location = New System.Drawing.Point(271, 12)
        Me.Btn_Hochi.Name = "Btn_Hochi"
        Me.Btn_Hochi.Size = New System.Drawing.Size(60, 23)
        Me.Btn_Hochi.TabIndex = 3
        Me.Btn_Hochi.Text = "分離"
        Me.Btn_Hochi.UseVisualStyleBackColor = True
        Me.Btn_Hochi.Visible = False
        '
        'Btn_CSOut
        '
        Me.Btn_CSOut.Location = New System.Drawing.Point(143, 12)
        Me.Btn_CSOut.Name = "Btn_CSOut"
        Me.Btn_CSOut.Size = New System.Drawing.Size(68, 23)
        Me.Btn_CSOut.TabIndex = 4
        Me.Btn_CSOut.Text = "CSV出力"
        Me.Btn_CSOut.UseVisualStyleBackColor = True
        '
        'Combo_Child
        '
        Me.Combo_Child.DropDownWidth = 137
        Me.Combo_Child.FormattingEnabled = True
        Me.Combo_Child.Items.AddRange(New Object() {"CT小番号表示", "CT小番号非表示"})
        Me.Combo_Child.Location = New System.Drawing.Point(10, 47)
        Me.Combo_Child.Name = "Combo_Child"
        Me.Combo_Child.Size = New System.Drawing.Size(126, 20)
        Me.Combo_Child.TabIndex = 5
        '
        'Combo_Single
        '
        Me.Combo_Single.DropDownWidth = 92
        Me.Combo_Single.FormattingEnabled = True
        Me.Combo_Single.Items.AddRange(New Object() {"ST表示", "ST非表示"})
        Me.Combo_Single.Location = New System.Drawing.Point(143, 47)
        Me.Combo_Single.Name = "Combo_Single"
        Me.Combo_Single.Size = New System.Drawing.Size(130, 20)
        Me.Combo_Single.TabIndex = 6
        '
        'Combo_Scale
        '
        Me.Combo_Scale.FormattingEnabled = True
        Me.Combo_Scale.Items.AddRange(New Object() {"定規用CT表示", "定規用CT非表示"})
        Me.Combo_Scale.Location = New System.Drawing.Point(10, 78)
        Me.Combo_Scale.Name = "Combo_Scale"
        Me.Combo_Scale.Size = New System.Drawing.Size(262, 20)
        Me.Combo_Scale.TabIndex = 7
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.Combo_Scale)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Btn_Hochi)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Btn_AllKaijyo)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Combo_Single)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Btn_CSOut)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Combo_Child)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Btn_AllSelect)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.DGV_DV)
        Me.SplitContainer1.Size = New System.Drawing.Size(437, 585)
        Me.SplitContainer1.SplitterDistance = 135
        Me.SplitContainer1.TabIndex = 8
        '
        'Data_Vertex
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(437, 585)
        Me.Controls.Add(Me.SplitContainer1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Data_Vertex"
        Me.Text = "計測点座標値"
        CType(Me.DGV_DV, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Btn_AllSelect As System.Windows.Forms.Button
    Friend WithEvents Btn_AllKaijyo As System.Windows.Forms.Button
    Friend WithEvents DGV_DV As System.Windows.Forms.DataGridView
    Friend WithEvents Btn_Hochi As System.Windows.Forms.Button
    Friend WithEvents Btn_CSOut As System.Windows.Forms.Button
    Friend WithEvents Combo_Child As System.Windows.Forms.ComboBox
    Friend WithEvents Combo_Single As System.Windows.Forms.ComboBox
    Friend WithEvents Combo_Scale As System.Windows.Forms.ComboBox
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SortID As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
