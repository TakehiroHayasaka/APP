<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class JCBNewConstruction
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(JCBNewConstruction))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBoxKogo = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TextBoxKojiName = New System.Windows.Forms.TextBox()
        Me.TextBoxCharge = New System.Windows.Forms.TextBox()
        Me.ComboBoxKozo = New System.Windows.Forms.ComboBox()
        Me.ButtonMakeFolder = New System.Windows.Forms.Button()
        Me.ButtonCancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'TextBoxKogo
        '
        resources.ApplyResources(Me.TextBoxKogo, "TextBoxKogo")
        Me.TextBoxKogo.Name = "TextBoxKogo"
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
        'TextBoxKojiName
        '
        resources.ApplyResources(Me.TextBoxKojiName, "TextBoxKojiName")
        Me.TextBoxKojiName.Name = "TextBoxKojiName"
        '
        'TextBoxCharge
        '
        resources.ApplyResources(Me.TextBoxCharge, "TextBoxCharge")
        Me.TextBoxCharge.Name = "TextBoxCharge"
        '
        'ComboBoxKozo
        '
        Me.ComboBoxKozo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxKozo.FormattingEnabled = True
        resources.ApplyResources(Me.ComboBoxKozo, "ComboBoxKozo")
        Me.ComboBoxKozo.Name = "ComboBoxKozo"
        '
        'ButtonMakeFolder
        '
        Me.ButtonMakeFolder.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.ButtonMakeFolder.FlatAppearance.BorderSize = 0
        resources.ApplyResources(Me.ButtonMakeFolder, "ButtonMakeFolder")
        Me.ButtonMakeFolder.ForeColor = System.Drawing.Color.White
        Me.ButtonMakeFolder.Name = "ButtonMakeFolder"
        Me.ButtonMakeFolder.UseVisualStyleBackColor = False
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
        'JCBNewConstruction
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(251, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.ButtonMakeFolder)
        Me.Controls.Add(Me.ComboBoxKozo)
        Me.Controls.Add(Me.TextBoxCharge)
        Me.Controls.Add(Me.TextBoxKojiName)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TextBoxKogo)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "JCBNewConstruction"
        Me.ShowInTaskbar = False
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents TextBoxKogo As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents TextBoxKojiName As TextBox
    Friend WithEvents TextBoxCharge As TextBox
    Friend WithEvents ComboBoxKozo As ComboBox
    Friend WithEvents ButtonMakeFolder As Button
    Friend WithEvents ButtonCancel As Button
End Class
