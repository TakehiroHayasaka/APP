<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class JCBNewConstructionCATS
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
        Me.ButtonDsn = New System.Windows.Forms.Button()
        Me.TextBoxDsn = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ButtonCancel = New System.Windows.Forms.Button()
        Me.ButtonMakeFolder = New System.Windows.Forms.Button()
        Me.ComboBoxKozo = New System.Windows.Forms.ComboBox()
        Me.TextBoxKojiName = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBoxKogo = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'ButtonDsn
        '
        Me.ButtonDsn.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.ButtonDsn.FlatAppearance.BorderSize = 0
        Me.ButtonDsn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonDsn.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.ButtonDsn.ForeColor = System.Drawing.Color.White
        Me.ButtonDsn.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ButtonDsn.Location = New System.Drawing.Point(347, 103)
        Me.ButtonDsn.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonDsn.Name = "ButtonDsn"
        Me.ButtonDsn.Size = New System.Drawing.Size(27, 21)
        Me.ButtonDsn.TabIndex = 28
        Me.ButtonDsn.Text = "..."
        Me.ButtonDsn.UseVisualStyleBackColor = False
        '
        'TextBoxDsn
        '
        Me.TextBoxDsn.Location = New System.Drawing.Point(110, 105)
        Me.TextBoxDsn.Margin = New System.Windows.Forms.Padding(0)
        Me.TextBoxDsn.Name = "TextBoxDsn"
        Me.TextBoxDsn.Size = New System.Drawing.Size(227, 19)
        Me.TextBoxDsn.TabIndex = 27
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("MS UI Gothic", 10.0!)
        Me.Label5.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label5.Location = New System.Drawing.Point(10, 110)
        Me.Label5.Margin = New System.Windows.Forms.Padding(0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(71, 14)
        Me.Label5.TabIndex = 26
        Me.Label5.Text = "DSNファイル"
        '
        'ButtonCancel
        '
        Me.ButtonCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.ButtonCancel.FlatAppearance.BorderSize = 0
        Me.ButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonCancel.ForeColor = System.Drawing.Color.White
        Me.ButtonCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ButtonCancel.Location = New System.Drawing.Point(295, 144)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(79, 25)
        Me.ButtonCancel.TabIndex = 25
        Me.ButtonCancel.Text = "CANCEL"
        Me.ButtonCancel.UseVisualStyleBackColor = False
        '
        'ButtonMakeFolder
        '
        Me.ButtonMakeFolder.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.ButtonMakeFolder.FlatAppearance.BorderSize = 0
        Me.ButtonMakeFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonMakeFolder.ForeColor = System.Drawing.Color.White
        Me.ButtonMakeFolder.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ButtonMakeFolder.Location = New System.Drawing.Point(212, 144)
        Me.ButtonMakeFolder.Name = "ButtonMakeFolder"
        Me.ButtonMakeFolder.Size = New System.Drawing.Size(79, 25)
        Me.ButtonMakeFolder.TabIndex = 24
        Me.ButtonMakeFolder.Text = "工事作成"
        Me.ButtonMakeFolder.UseVisualStyleBackColor = False
        '
        'ComboBoxKozo
        '
        Me.ComboBoxKozo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxKozo.FormattingEnabled = True
        Me.ComboBoxKozo.Location = New System.Drawing.Point(110, 75)
        Me.ComboBoxKozo.Name = "ComboBoxKozo"
        Me.ComboBoxKozo.Size = New System.Drawing.Size(108, 20)
        Me.ComboBoxKozo.TabIndex = 22
        '
        'TextBoxKojiName
        '
        Me.TextBoxKojiName.Location = New System.Drawing.Point(110, 45)
        Me.TextBoxKojiName.Margin = New System.Windows.Forms.Padding(0)
        Me.TextBoxKojiName.Name = "TextBoxKojiName"
        Me.TextBoxKojiName.Size = New System.Drawing.Size(227, 19)
        Me.TextBoxKojiName.TabIndex = 21
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
        Me.Label4.TabIndex = 20
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
        Me.Label2.TabIndex = 18
        Me.Label2.Text = "工事名"
        '
        'TextBoxKogo
        '
        Me.TextBoxKogo.Location = New System.Drawing.Point(110, 15)
        Me.TextBoxKogo.Margin = New System.Windows.Forms.Padding(0)
        Me.TextBoxKogo.Name = "TextBoxKogo"
        Me.TextBoxKogo.Size = New System.Drawing.Size(227, 19)
        Me.TextBoxKogo.TabIndex = 17
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
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "工号"
        '
        'JCBNewConstructionCATS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(251, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(384, 181)
        Me.Controls.Add(Me.ButtonDsn)
        Me.Controls.Add(Me.TextBoxDsn)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.ButtonMakeFolder)
        Me.Controls.Add(Me.ComboBoxKozo)
        Me.Controls.Add(Me.TextBoxKojiName)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TextBoxKogo)
        Me.Controls.Add(Me.Label1)
        Me.Name = "JCBNewConstructionCATS"
        Me.Text = "新規工事登録"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ButtonDsn As Button
    Friend WithEvents TextBoxDsn As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents ButtonCancel As Button
    Friend WithEvents ButtonMakeFolder As Button
    Friend WithEvents ComboBoxKozo As ComboBox
    Friend WithEvents TextBoxKojiName As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBoxKogo As TextBox
    Friend WithEvents Label1 As Label
End Class
