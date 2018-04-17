<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmDetail
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
        Me.components = New System.ComponentModel.Container()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.Db1DataSet = New WindowsApplication1.db1DataSet()
        Me.BBBBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.BBBTableAdapter = New WindowsApplication1.db1DataSetTableAdapters.BBBTableAdapter()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.Db1DataSet1 = New WindowsApplication1.db1DataSet1()
        Me.TTTBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.TTTTableAdapter = New WindowsApplication1.db1DataSet1TableAdapters.TTTTableAdapter()
        Me.Field1DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Field2DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.DataGridView2 = New System.Windows.Forms.DataGridView()
        Me.Field1DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Field2DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        CType(Me.Db1DataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BBBBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Db1DataSet1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TTTBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel2, 0, 2)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 68.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(430, 561)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.DataGridView1, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Panel2, 0, 1)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 182)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 2
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(424, 376)
        Me.TableLayoutPanel2.TabIndex = 1
        '
        'Db1DataSet
        '
        Me.Db1DataSet.DataSetName = "db1DataSet"
        Me.Db1DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'BBBBindingSource
        '
        Me.BBBBindingSource.DataMember = "BBB"
        Me.BBBBindingSource.DataSource = Me.Db1DataSet
        '
        'BBBTableAdapter
        '
        Me.BBBTableAdapter.ClearBeforeFill = True
        '
        'DataGridView1
        '
        Me.DataGridView1.AutoGenerateColumns = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Field1DataGridViewTextBoxColumn, Me.Field2DataGridViewTextBoxColumn})
        Me.DataGridView1.DataSource = Me.TTTBindingSource
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView1.Location = New System.Drawing.Point(3, 3)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(418, 182)
        Me.DataGridView1.TabIndex = 0
        '
        'Db1DataSet1
        '
        Me.Db1DataSet1.DataSetName = "db1DataSet1"
        Me.Db1DataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'TTTBindingSource
        '
        Me.TTTBindingSource.DataMember = "TTT"
        Me.TTTBindingSource.DataSource = Me.Db1DataSet1
        '
        'TTTTableAdapter
        '
        Me.TTTTableAdapter.ClearBeforeFill = True
        '
        'Field1DataGridViewTextBoxColumn
        '
        Me.Field1DataGridViewTextBoxColumn.DataPropertyName = "Field1"
        Me.Field1DataGridViewTextBoxColumn.HeaderText = "測点"
        Me.Field1DataGridViewTextBoxColumn.Name = "Field1DataGridViewTextBoxColumn"
        Me.Field1DataGridViewTextBoxColumn.Width = 150
        '
        'Field2DataGridViewTextBoxColumn
        '
        Me.Field2DataGridViewTextBoxColumn.DataPropertyName = "Field2"
        Me.Field2DataGridViewTextBoxColumn.HeaderText = "認識"
        Me.Field2DataGridViewTextBoxColumn.Name = "Field2DataGridViewTextBoxColumn"
        Me.Field2DataGridViewTextBoxColumn.Width = 150
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.DataGridView2)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(3, 191)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(418, 182)
        Me.Panel2.TabIndex = 1
        '
        'DataGridView2
        '
        Me.DataGridView2.AutoGenerateColumns = False
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Field1DataGridViewTextBoxColumn1, Me.Field2DataGridViewTextBoxColumn1})
        Me.DataGridView2.DataSource = Me.BBBBindingSource
        Me.DataGridView2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView2.Location = New System.Drawing.Point(0, 0)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.Size = New System.Drawing.Size(418, 182)
        Me.DataGridView2.TabIndex = 0
        '
        'Field1DataGridViewTextBoxColumn1
        '
        Me.Field1DataGridViewTextBoxColumn1.DataPropertyName = "Field1"
        Me.Field1DataGridViewTextBoxColumn1.HeaderText = "基準点"
        Me.Field1DataGridViewTextBoxColumn1.Name = "Field1DataGridViewTextBoxColumn1"
        Me.Field1DataGridViewTextBoxColumn1.Width = 150
        '
        'Field2DataGridViewTextBoxColumn1
        '
        Me.Field2DataGridViewTextBoxColumn1.DataPropertyName = "Field2"
        Me.Field2DataGridViewTextBoxColumn1.HeaderText = ""
        Me.Field2DataGridViewTextBoxColumn1.Name = "Field2DataGridViewTextBoxColumn1"
        Me.Field2DataGridViewTextBoxColumn1.Width = 150
        '
        'Panel1
        '
        Me.Panel1.BackgroundImage = Global.WindowsApplication1.My.Resources.Resources.トラック1
        Me.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 14)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(424, 162)
        Me.Panel1.TabIndex = 0
        '
        'FrmDetail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(430, 561)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.MinimumSize = New System.Drawing.Size(400, 500)
        Me.Name = "FrmDetail"
        Me.Text = "詳細"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        CType(Me.Db1DataSet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BBBBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Db1DataSet1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TTTBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Db1DataSet As WindowsApplication1.db1DataSet
    Friend WithEvents BBBBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents BBBTableAdapter As WindowsApplication1.db1DataSetTableAdapters.BBBTableAdapter
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents Db1DataSet1 As WindowsApplication1.db1DataSet1
    Friend WithEvents TTTBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents TTTTableAdapter As WindowsApplication1.db1DataSet1TableAdapters.TTTTableAdapter
    Friend WithEvents Field1DataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Field2DataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents Field1DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Field2DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
