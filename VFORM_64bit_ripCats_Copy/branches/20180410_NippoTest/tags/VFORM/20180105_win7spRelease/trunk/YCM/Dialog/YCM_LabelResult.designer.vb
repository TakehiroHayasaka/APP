<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class YCM_LabelResult
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
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.data_point_result = New System.Windows.Forms.DataGridView()
        Me.No = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.設計ラベル = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.測点名 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.X1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Y1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Z1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.X2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Y2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Z2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.X3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Y3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Z3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CSVOUT = New System.Windows.Forms.Button()
        Me.OK = New System.Windows.Forms.Button()
        Me.Rreturn = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        CType(Me.data_point_result, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(24, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(59, 12)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "設計デ一タ"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(114, 10)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(54, 12)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "pathname"
        '
        'data_point_result
        '
        Me.data_point_result.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.data_point_result.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.No, Me.設計ラベル, Me.測点名, Me.X1, Me.Y1, Me.Z1, Me.X2, Me.Y2, Me.Z2, Me.X3, Me.Y3, Me.Z3})
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle13.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle13.Format = "N3"
        DataGridViewCellStyle13.NullValue = Nothing
        DataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.data_point_result.DefaultCellStyle = DataGridViewCellStyle13
        Me.data_point_result.Location = New System.Drawing.Point(12, 48)
        Me.data_point_result.Name = "data_point_result"
        Me.data_point_result.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.data_point_result.RowHeadersVisible = False
        Me.data_point_result.RowTemplate.Height = 21
        Me.data_point_result.Size = New System.Drawing.Size(760, 297)
        Me.data_point_result.TabIndex = 3
        '
        'No
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.No.DefaultCellStyle = DataGridViewCellStyle1
        Me.No.HeaderText = "No"
        Me.No.Name = "No"
        Me.No.Width = 40
        '
        '設計ラベル
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.設計ラベル.DefaultCellStyle = DataGridViewCellStyle2
        Me.設計ラベル.HeaderText = "設計ラベル"
        Me.設計ラベル.Name = "設計ラベル"
        Me.設計ラベル.Width = 80
        '
        '測点名
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.測点名.DefaultCellStyle = DataGridViewCellStyle3
        Me.測点名.HeaderText = "測点名"
        Me.測点名.Name = "測点名"
        Me.測点名.Width = 80
        '
        'X1
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle4.Format = "N3"
        DataGridViewCellStyle4.NullValue = Nothing
        Me.X1.DefaultCellStyle = DataGridViewCellStyle4
        Me.X1.HeaderText = "X"
        Me.X1.Name = "X1"
        Me.X1.Width = 60
        '
        'Y1
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle5.Format = "N3"
        DataGridViewCellStyle5.NullValue = Nothing
        Me.Y1.DefaultCellStyle = DataGridViewCellStyle5
        Me.Y1.HeaderText = "Y"
        Me.Y1.Name = "Y1"
        Me.Y1.Width = 60
        '
        'Z1
        '
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle6.Format = "N3"
        DataGridViewCellStyle6.NullValue = Nothing
        Me.Z1.DefaultCellStyle = DataGridViewCellStyle6
        Me.Z1.HeaderText = "Z"
        Me.Z1.Name = "Z1"
        Me.Z1.Width = 60
        '
        'X2
        '
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle7.Format = "N3"
        DataGridViewCellStyle7.NullValue = Nothing
        Me.X2.DefaultCellStyle = DataGridViewCellStyle7
        Me.X2.HeaderText = "X"
        Me.X2.Name = "X2"
        Me.X2.Width = 60
        '
        'Y2
        '
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle8.Format = "N3"
        DataGridViewCellStyle8.NullValue = Nothing
        Me.Y2.DefaultCellStyle = DataGridViewCellStyle8
        Me.Y2.HeaderText = "Y"
        Me.Y2.Name = "Y2"
        Me.Y2.Width = 60
        '
        'Z2
        '
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle9.Format = "N3"
        DataGridViewCellStyle9.NullValue = Nothing
        Me.Z2.DefaultCellStyle = DataGridViewCellStyle9
        Me.Z2.HeaderText = "Z"
        Me.Z2.Name = "Z2"
        Me.Z2.Width = 60
        '
        'X3
        '
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle10.Format = "N3"
        DataGridViewCellStyle10.NullValue = Nothing
        Me.X3.DefaultCellStyle = DataGridViewCellStyle10
        Me.X3.HeaderText = "X"
        Me.X3.Name = "X3"
        Me.X3.Width = 60
        '
        'Y3
        '
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle11.Format = "N3"
        DataGridViewCellStyle11.NullValue = Nothing
        Me.Y3.DefaultCellStyle = DataGridViewCellStyle11
        Me.Y3.HeaderText = "Y"
        Me.Y3.Name = "Y3"
        Me.Y3.Width = 60
        '
        'Z3
        '
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle12.Format = "N3"
        DataGridViewCellStyle12.NullValue = Nothing
        Me.Z3.DefaultCellStyle = DataGridViewCellStyle12
        Me.Z3.HeaderText = "Z"
        Me.Z3.Name = "Z3"
        Me.Z3.Width = 60
        '
        'CSVOUT
        '
        Me.CSVOUT.Location = New System.Drawing.Point(111, 354)
        Me.CSVOUT.Name = "CSVOUT"
        Me.CSVOUT.Size = New System.Drawing.Size(75, 23)
        Me.CSVOUT.TabIndex = 4
        Me.CSVOUT.Text = "CSV出力"
        Me.CSVOUT.UseVisualStyleBackColor = True
        '
        'OK
        '
        Me.OK.Location = New System.Drawing.Point(556, 354)
        Me.OK.Name = "OK"
        Me.OK.Size = New System.Drawing.Size(75, 23)
        Me.OK.TabIndex = 5
        Me.OK.Text = "確定"
        Me.OK.UseVisualStyleBackColor = True
        '
        'Rreturn
        '
        Me.Rreturn.Location = New System.Drawing.Point(673, 354)
        Me.Rreturn.Name = "Rreturn"
        Me.Rreturn.Size = New System.Drawing.Size(75, 23)
        Me.Rreturn.TabIndex = 6
        Me.Rreturn.Text = "戻る"
        Me.Rreturn.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label3.Location = New System.Drawing.Point(210, 31)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(183, 19)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "設計点"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label4.Location = New System.Drawing.Point(392, 31)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(180, 19)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "計測点"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label5.Location = New System.Drawing.Point(571, 31)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(183, 19)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "差(設計点－計測点)"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'YCM_LabelResult
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 389)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Rreturn)
        Me.Controls.Add(Me.OK)
        Me.Controls.Add(Me.CSVOUT)
        Me.Controls.Add(Me.data_point_result)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "YCM_LabelResult"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "自動ラベリング結果の確認"
        CType(Me.data_point_result, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents data_point_result As System.Windows.Forms.DataGridView
    Friend WithEvents No As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents 設計ラベル As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents 測点名 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents X1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Y1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Z1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents X2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Y2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Z2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents X3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Y3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Z3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CSVOUT As System.Windows.Forms.Button
    Friend WithEvents OK As System.Windows.Forms.Button
    Friend WithEvents Rreturn As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
End Class
