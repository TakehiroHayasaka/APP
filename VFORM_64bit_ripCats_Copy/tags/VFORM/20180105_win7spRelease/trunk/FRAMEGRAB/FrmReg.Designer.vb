﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmReg
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
        Me.col1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TableLayoutPanel7 = New System.Windows.Forms.TableLayoutPanel()
        Me.btnRegist = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanel5 = New System.Windows.Forms.TableLayoutPanel()
        Me.txtGetFolderLocal = New System.Windows.Forms.TextBox()
        Me.btnGetFolderLocal = New System.Windows.Forms.Button()
        Me.btnGetFolderShare = New System.Windows.Forms.Button()
        Me.txtGetFolderShare = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.dgKihonInfo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgSokutechi, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel7.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.TableLayoutPanel5.SuspendLayout()
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
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(4)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 7
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1.449275!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.61539!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1.444043!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 56.15385!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1.538462!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
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
        'col1
        '
        Me.col1.HeaderText = "測定箇所"
        Me.col1.Name = "col1"
        Me.col1.ReadOnly = True
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
        '
        'TableLayoutPanel7
        '
        Me.TableLayoutPanel7.ColumnCount = 4
        Me.TableLayoutPanel7.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.07744!))
        Me.TableLayoutPanel7.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel7.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.25589!))
        Me.TableLayoutPanel7.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel7.Controls.Add(Me.btnRegist, 2, 0)
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
        'btnRegist
        '
        Me.btnRegist.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.btnRegist.Location = New System.Drawing.Point(347, 5)
        Me.btnRegist.Name = "btnRegist"
        Me.btnRegist.Size = New System.Drawing.Size(65, 23)
        Me.btnRegist.TabIndex = 0
        Me.btnRegist.Text = "登録"
        Me.btnRegist.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.btnCancel.Location = New System.Drawing.Point(418, 5)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.btnPrint.Location = New System.Drawing.Point(278, 5)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(63, 23)
        Me.btnPrint.TabIndex = 2
        Me.btnPrint.Text = "印刷"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.TableLayoutPanel5)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(11, 393)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(498, 124)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "登録先"
        '
        'TableLayoutPanel5
        '
        Me.TableLayoutPanel5.ColumnCount = 2
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73.0!))
        Me.TableLayoutPanel5.Controls.Add(Me.txtGetFolderLocal, 0, 3)
        Me.TableLayoutPanel5.Controls.Add(Me.btnGetFolderLocal, 1, 3)
        Me.TableLayoutPanel5.Controls.Add(Me.btnGetFolderShare, 1, 1)
        Me.TableLayoutPanel5.Controls.Add(Me.txtGetFolderShare, 0, 1)
        Me.TableLayoutPanel5.Controls.Add(Me.Label3, 0, 2)
        Me.TableLayoutPanel5.Controls.Add(Me.Label2, 0, 0)
        Me.TableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel5.Location = New System.Drawing.Point(3, 19)
        Me.TableLayoutPanel5.Name = "TableLayoutPanel5"
        Me.TableLayoutPanel5.RowCount = 4
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18.62745!))
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 36.27451!))
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.64706!))
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 26.31579!))
        Me.TableLayoutPanel5.Size = New System.Drawing.Size(492, 102)
        Me.TableLayoutPanel5.TabIndex = 5
        '
        'txtGetFolderLocal
        '
        Me.txtGetFolderLocal.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtGetFolderLocal.Location = New System.Drawing.Point(3, 77)
        Me.txtGetFolderLocal.Name = "txtGetFolderLocal"
        Me.txtGetFolderLocal.Size = New System.Drawing.Size(413, 23)
        Me.txtGetFolderLocal.TabIndex = 1
        '
        'btnGetFolderLocal
        '
        Me.btnGetFolderLocal.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGetFolderLocal.Location = New System.Drawing.Point(422, 77)
        Me.btnGetFolderLocal.Name = "btnGetFolderLocal"
        Me.btnGetFolderLocal.Size = New System.Drawing.Size(67, 22)
        Me.btnGetFolderLocal.TabIndex = 2
        Me.btnGetFolderLocal.Text = "参照..."
        Me.btnGetFolderLocal.UseVisualStyleBackColor = True
        '
        'btnGetFolderShare
        '
        Me.btnGetFolderShare.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGetFolderShare.Location = New System.Drawing.Point(422, 26)
        Me.btnGetFolderShare.Name = "btnGetFolderShare"
        Me.btnGetFolderShare.Size = New System.Drawing.Size(67, 22)
        Me.btnGetFolderShare.TabIndex = 3
        Me.btnGetFolderShare.Text = "参照..."
        Me.btnGetFolderShare.UseVisualStyleBackColor = True
        '
        'txtGetFolderShare
        '
        Me.txtGetFolderShare.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtGetFolderShare.Location = New System.Drawing.Point(3, 26)
        Me.txtGetFolderShare.Name = "txtGetFolderShare"
        Me.txtGetFolderShare.Size = New System.Drawing.Size(413, 23)
        Me.txtGetFolderShare.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 56)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(413, 17)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "ローカルフォルダ"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 1)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(413, 17)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "共有フォルダ"
        '
        'FrmReg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(521, 562)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MinimumSize = New System.Drawing.Size(400, 600)
        Me.Name = "FrmReg"
        Me.Tag = "D:\tuvshin-work\20150731割込み\Data130826165926560955"
        Me.Text = "測定値登録"
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.dgKihonInfo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgSokutechi, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel7.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.TableLayoutPanel5.ResumeLayout(False)
        Me.TableLayoutPanel5.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents dgKihonInfo As System.Windows.Forms.DataGridView
    Friend WithEvents dgSokutechi As System.Windows.Forms.DataGridView
    Friend WithEvents TableLayoutPanel5 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnGetFolderLocal As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtGetFolderShare As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnGetFolderShare As System.Windows.Forms.Button
    Friend WithEvents txtGetFolderLocal As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents TableLayoutPanel7 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnRegist As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents col1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents col2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents btnPrint As System.Windows.Forms.Button
End Class
