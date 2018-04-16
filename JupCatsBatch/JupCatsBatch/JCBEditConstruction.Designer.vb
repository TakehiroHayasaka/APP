<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class JCBEditConstruction
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(JCBEditConstruction))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ButtonCancel = New System.Windows.Forms.Button()
        Me.TextBoxEditKogo = New System.Windows.Forms.TextBox()
        Me.TextBoxEditChargeCode = New System.Windows.Forms.TextBox()
        Me.TextBoxEditKojiName = New System.Windows.Forms.TextBox()
        Me.ButtonEdit = New System.Windows.Forms.Button()
        Me.TextBoxEditKozo = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'ButtonCancel
        '
        Me.ButtonCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.ButtonCancel.FlatAppearance.BorderSize = 0
        resources.ApplyResources(Me.ButtonCancel, "ButtonCancel")
        Me.ButtonCancel.ForeColor = System.Drawing.Color.White
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.UseVisualStyleBackColor = False
        '
        'TextBoxEditKogo
        '
        resources.ApplyResources(Me.TextBoxEditKogo, "TextBoxEditKogo")
        Me.TextBoxEditKogo.Name = "TextBoxEditKogo"
        '
        'TextBoxEditChargeCode
        '
        resources.ApplyResources(Me.TextBoxEditChargeCode, "TextBoxEditChargeCode")
        Me.TextBoxEditChargeCode.Name = "TextBoxEditChargeCode"
        '
        'TextBoxEditKojiName
        '
        resources.ApplyResources(Me.TextBoxEditKojiName, "TextBoxEditKojiName")
        Me.TextBoxEditKojiName.Name = "TextBoxEditKojiName"
        '
        'ButtonEdit
        '
        Me.ButtonEdit.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.ButtonEdit.FlatAppearance.BorderSize = 0
        resources.ApplyResources(Me.ButtonEdit, "ButtonEdit")
        Me.ButtonEdit.ForeColor = System.Drawing.Color.White
        Me.ButtonEdit.Name = "ButtonEdit"
        Me.ButtonEdit.UseVisualStyleBackColor = False
        '
        'TextBoxEditKozo
        '
        resources.ApplyResources(Me.TextBoxEditKozo, "TextBoxEditKozo")
        Me.TextBoxEditKozo.Name = "TextBoxEditKozo"
        '
        'JCBEditConstruction
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(251, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.Controls.Add(Me.TextBoxEditKozo)
        Me.Controls.Add(Me.ButtonEdit)
        Me.Controls.Add(Me.TextBoxEditKojiName)
        Me.Controls.Add(Me.TextBoxEditChargeCode)
        Me.Controls.Add(Me.TextBoxEditKogo)
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "JCBEditConstruction"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents ButtonCancel As Button
    Friend WithEvents TextBoxEditKogo As TextBox
    Friend WithEvents TextBoxEditChargeCode As TextBox
    Friend WithEvents TextBoxEditKojiName As TextBox
    Friend WithEvents ButtonEdit As Button
    Friend WithEvents TextBoxEditKozo As TextBox
End Class
