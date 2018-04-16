<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class JCBFileExplore3
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(JCBFileExplore3))
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.TreeGridView3 = New AdvancedDataGridView.TreeGridView()
        Me.fileType = New AdvancedDataGridView.TreeGridColumn()
        Me.dateFolder = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.fileName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.status = New System.Windows.Forms.DataGridViewImageColumn()
        Me.UpdateDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.size = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FinalSavePerson = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemShow = New System.Windows.Forms.ToolStripMenuItem()
        Me.ButtonFilter = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemOpenFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemPrint = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemOutputPDF = New System.Windows.Forms.ToolStripMenuItem()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me.TreeGridView3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip2.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TreeGridView3
        '
        Me.TreeGridView3.AllowUserToAddRows = False
        Me.TreeGridView3.AllowUserToDeleteRows = False
        Me.TreeGridView3.AllowUserToResizeColumns = False
        Me.TreeGridView3.AllowUserToResizeRows = False
        Me.TreeGridView3.BackgroundColor = System.Drawing.Color.White
        Me.TreeGridView3.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TreeGridView3.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None
        Me.TreeGridView3.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle7.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.Color.Silver
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.TreeGridView3.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle7
        Me.TreeGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.TreeGridView3.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.fileType, Me.dateFolder, Me.fileName, Me.status, Me.UpdateDate, Me.size, Me.FinalSavePerson})
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle11.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(217, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.TreeGridView3.DefaultCellStyle = DataGridViewCellStyle11
        Me.TreeGridView3.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.TreeGridView3.GridColor = System.Drawing.Color.Gainsboro
        Me.TreeGridView3.ImageList = Nothing
        resources.ApplyResources(Me.TreeGridView3, "TreeGridView3")
        Me.TreeGridView3.Name = "TreeGridView3"
        Me.TreeGridView3.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle12.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle12.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.TreeGridView3.RowHeadersDefaultCellStyle = DataGridViewCellStyle12
        Me.TreeGridView3.RowHeadersVisible = False
        Me.TreeGridView3.ShowLines = False
        '
        'fileType
        '
        Me.fileType.DefaultNodeImage = Nothing
        Me.fileType.FillWeight = 200.0!
        resources.ApplyResources(Me.fileType, "fileType")
        Me.fileType.Name = "fileType"
        Me.fileType.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.fileType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'dateFolder
        '
        DataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black
        Me.dateFolder.DefaultCellStyle = DataGridViewCellStyle8
        Me.dateFolder.FillWeight = 180.0!
        resources.ApplyResources(Me.dateFolder, "dateFolder")
        Me.dateFolder.Name = "dateFolder"
        Me.dateFolder.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dateFolder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'fileName
        '
        Me.fileName.FillWeight = 180.0!
        resources.ApplyResources(Me.fileName, "fileName")
        Me.fileName.Name = "fileName"
        Me.fileName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'status
        '
        Me.status.FillWeight = 60.0!
        resources.ApplyResources(Me.status, "status")
        Me.status.Name = "status"
        '
        'UpdateDate
        '
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.UpdateDate.DefaultCellStyle = DataGridViewCellStyle9
        resources.ApplyResources(Me.UpdateDate, "UpdateDate")
        Me.UpdateDate.Name = "UpdateDate"
        Me.UpdateDate.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.UpdateDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'size
        '
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.size.DefaultCellStyle = DataGridViewCellStyle10
        Me.size.FillWeight = 50.0!
        resources.ApplyResources(Me.size, "size")
        Me.size.Name = "size"
        Me.size.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'FinalSavePerson
        '
        resources.ApplyResources(Me.FinalSavePerson, "FinalSavePerson")
        Me.FinalSavePerson.Name = "FinalSavePerson"
        Me.FinalSavePerson.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemShow})
        Me.ContextMenuStrip2.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        resources.ApplyResources(Me.ContextMenuStrip2, "ContextMenuStrip2")
        '
        'ToolStripMenuItemShow
        '
        Me.ToolStripMenuItemShow.Name = "ToolStripMenuItemShow"
        resources.ApplyResources(Me.ToolStripMenuItemShow, "ToolStripMenuItemShow")
        '
        'ButtonFilter
        '
        Me.ButtonFilter.BackColor = System.Drawing.Color.White
        resources.ApplyResources(Me.ButtonFilter, "ButtonFilter")
        Me.ButtonFilter.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ButtonFilter.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ButtonFilter.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.ButtonFilter.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White
        Me.ButtonFilter.Name = "ButtonFilter"
        Me.ButtonFilter.UseVisualStyleBackColor = False
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.White
        resources.ApplyResources(Me.Button1, "Button1")
        Me.Button1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.Button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White
        Me.Button1.Name = "Button1"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemOpenFile, Me.ToolStripMenuItemPrint, Me.ToolStripMenuItemOutputPDF})
        Me.ContextMenuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        resources.ApplyResources(Me.ContextMenuStrip1, "ContextMenuStrip1")
        '
        'ToolStripMenuItemOpenFile
        '
        Me.ToolStripMenuItemOpenFile.Name = "ToolStripMenuItemOpenFile"
        resources.ApplyResources(Me.ToolStripMenuItemOpenFile, "ToolStripMenuItemOpenFile")
        '
        'ToolStripMenuItemPrint
        '
        Me.ToolStripMenuItemPrint.Name = "ToolStripMenuItemPrint"
        resources.ApplyResources(Me.ToolStripMenuItemPrint, "ToolStripMenuItemPrint")
        '
        'ToolStripMenuItemOutputPDF
        '
        Me.ToolStripMenuItemOutputPDF.Name = "ToolStripMenuItemOutputPDF"
        resources.ApplyResources(Me.ToolStripMenuItemOutputPDF, "ToolStripMenuItemOutputPDF")
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.Controls.Add(Me.ButtonFilter)
        Me.Panel1.Controls.Add(Me.Button1)
        resources.ApplyResources(Me.Panel1, "Panel1")
        Me.Panel1.Name = "Panel1"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Gainsboro
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'JCBFileExplore3
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(251, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.TreeGridView3)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Name = "JCBFileExplore3"
        CType(Me.TreeGridView3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip2.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TreeGridView3 As AdvancedDataGridView.TreeGridView
    Friend WithEvents ButtonFilter As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents ToolStripMenuItemOpenFile As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemPrint As ToolStripMenuItem
    Friend WithEvents ContextMenuStrip2 As ContextMenuStrip
    Friend WithEvents ToolStripMenuItemShow As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemOutputPDF As ToolStripMenuItem
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents fileType As AdvancedDataGridView.TreeGridColumn
    Friend WithEvents dateFolder As DataGridViewTextBoxColumn
    Friend WithEvents fileName As DataGridViewTextBoxColumn
    Friend WithEvents status As DataGridViewImageColumn
    Friend WithEvents UpdateDate As DataGridViewTextBoxColumn
    Friend WithEvents size As DataGridViewTextBoxColumn
    Friend WithEvents FinalSavePerson As DataGridViewTextBoxColumn
End Class
