<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class JCBEditConstructionCATS
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
        Me.TextBoxEditKozo = New System.Windows.Forms.TextBox()
        Me.ButtonEdit = New System.Windows.Forms.Button()
        Me.TextBoxEditKojiName = New System.Windows.Forms.TextBox()
        Me.TextBoxEditKogo = New System.Windows.Forms.TextBox()
        Me.ButtonCancel = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'TextBoxEditKozo
        '
        Me.TextBoxEditKozo.Enabled = False
        Me.TextBoxEditKozo.Location = New System.Drawing.Point(110, 74)
        Me.TextBoxEditKozo.Margin = New System.Windows.Forms.Padding(0)
        Me.TextBoxEditKozo.Name = "TextBoxEditKozo"
        Me.TextBoxEditKozo.Size = New System.Drawing.Size(227, 19)
        Me.TextBoxEditKozo.TabIndex = 28
        '
        'ButtonEdit
        '
        Me.ButtonEdit.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.ButtonEdit.FlatAppearance.BorderSize = 0
        Me.ButtonEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonEdit.ForeColor = System.Drawing.Color.White
        Me.ButtonEdit.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ButtonEdit.Location = New System.Drawing.Point(173, 110)
        Me.ButtonEdit.Name = "ButtonEdit"
        Me.ButtonEdit.Size = New System.Drawing.Size(79, 25)
        Me.ButtonEdit.TabIndex = 27
        Me.ButtonEdit.Text = "変更"
        Me.ButtonEdit.UseVisualStyleBackColor = False
        '
        'TextBoxEditKojiName
        '
        Me.TextBoxEditKojiName.Location = New System.Drawing.Point(110, 44)
        Me.TextBoxEditKojiName.Margin = New System.Windows.Forms.Padding(0)
        Me.TextBoxEditKojiName.Name = "TextBoxEditKojiName"
        Me.TextBoxEditKojiName.Size = New System.Drawing.Size(227, 19)
        Me.TextBoxEditKojiName.TabIndex = 26
        '
        'TextBoxEditKogo
        '
        Me.TextBoxEditKogo.Enabled = False
        Me.TextBoxEditKogo.Location = New System.Drawing.Point(110, 14)
        Me.TextBoxEditKogo.Margin = New System.Windows.Forms.Padding(0)
        Me.TextBoxEditKogo.Name = "TextBoxEditKogo"
        Me.TextBoxEditKogo.Size = New System.Drawing.Size(227, 19)
        Me.TextBoxEditKogo.TabIndex = 24
        '
        'ButtonCancel
        '
        Me.ButtonCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.ButtonCancel.FlatAppearance.BorderSize = 0
        Me.ButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonCancel.ForeColor = System.Drawing.Color.White
        Me.ButtonCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ButtonCancel.Location = New System.Drawing.Point(258, 110)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(79, 25)
        Me.ButtonCancel.TabIndex = 23
        Me.ButtonCancel.Text = "CANCEL"
        Me.ButtonCancel.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.Label4.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label4.Location = New System.Drawing.Point(10, 80)
        Me.Label4.Margin = New System.Windows.Forms.Padding(0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(35, 14)
        Me.Label4.TabIndex = 22
        Me.Label4.Text = "橋種"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.Label2.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label2.Location = New System.Drawing.Point(10, 50)
        Me.Label2.Margin = New System.Windows.Forms.Padding(0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(49, 14)
        Me.Label2.TabIndex = 20
        Me.Label2.Text = "工事名"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.Label1.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label1.Location = New System.Drawing.Point(10, 20)
        Me.Label1.Margin = New System.Windows.Forms.Padding(0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(35, 14)
        Me.Label1.TabIndex = 19
        Me.Label1.Text = "工号"
        '
        'JCBEditConstructionCATS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(251, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(354, 148)
        Me.Controls.Add(Me.TextBoxEditKozo)
        Me.Controls.Add(Me.ButtonEdit)
        Me.Controls.Add(Me.TextBoxEditKojiName)
        Me.Controls.Add(Me.TextBoxEditKogo)
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "JCBEditConstructionCATS"
        Me.Text = "JCBEditConstructionCATS"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TextBoxEditKozo As TextBox
    Friend WithEvents ButtonEdit As Button
    Friend WithEvents TextBoxEditKojiName As TextBox
    Friend WithEvents TextBoxEditKogo As TextBox
    Friend WithEvents ButtonCancel As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
End Class
