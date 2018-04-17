<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class YCM_ScaleSetting
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label_P1 = New System.Windows.Forms.Label()
        Me.Label_P2 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.CbBscalelong = New System.Windows.Forms.ComboBox()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.ComboBox2 = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(32, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(47, 12)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "計測点1"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(32, 67)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 12)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "計測点2"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(98, 27)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(54, 23)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "選択..."
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(98, 62)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(54, 23)
        Me.Button2.TabIndex = 3
        Me.Button2.Text = "選択..."
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(191, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(33, 12)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "ラベル"
        '
        'Label_P1
        '
        Me.Label_P1.AutoSize = True
        Me.Label_P1.Location = New System.Drawing.Point(284, 32)
        Me.Label_P1.Name = "Label_P1"
        Me.Label_P1.Size = New System.Drawing.Size(18, 12)
        Me.Label_P1.TabIndex = 5
        Me.Label_P1.Text = "P1"
        Me.Label_P1.Visible = False
        '
        'Label_P2
        '
        Me.Label_P2.AutoSize = True
        Me.Label_P2.Location = New System.Drawing.Point(284, 67)
        Me.Label_P2.Name = "Label_P2"
        Me.Label_P2.Size = New System.Drawing.Size(18, 12)
        Me.Label_P2.TabIndex = 6
        Me.Label_P2.Text = "P2"
        Me.Label_P2.Visible = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(26, 104)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(87, 12)
        Me.Label6.TabIndex = 7
        Me.Label6.Text = "スケ―ル長（mm）"
        '
        'CbBscalelong
        '
        Me.CbBscalelong.FormattingEnabled = True
        Me.CbBscalelong.Location = New System.Drawing.Point(125, 101)
        Me.CbBscalelong.Name = "CbBscalelong"
        Me.CbBscalelong.Size = New System.Drawing.Size(121, 20)
        Me.CbBscalelong.TabIndex = 8
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(63, 143)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(74, 24)
        Me.Button3.TabIndex = 9
        Me.Button3.Text = "設定"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(161, 143)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(84, 24)
        Me.Button4.TabIndex = 10
        Me.Button4.Text = "キャンセル"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(169, 28)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(98, 20)
        Me.ComboBox1.Sorted = True
        Me.ComboBox1.TabIndex = 11
        '
        'ComboBox2
        '
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Location = New System.Drawing.Point(169, 63)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(98, 20)
        Me.ComboBox2.Sorted = True
        Me.ComboBox2.TabIndex = 11
        '
        'YCM_ScaleSetting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(299, 194)
        Me.Controls.Add(Me.ComboBox2)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.CbBscalelong)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label_P2)
        Me.Controls.Add(Me.Label_P1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "YCM_ScaleSetting"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "スケ―ル設定"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label_P1 As System.Windows.Forms.Label
    Friend WithEvents Label_P2 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents CbBscalelong As System.Windows.Forms.ComboBox
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
End Class
