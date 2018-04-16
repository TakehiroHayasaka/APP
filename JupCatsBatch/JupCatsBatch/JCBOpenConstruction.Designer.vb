<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class JCBOpenConstruction
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(JCBOpenConstruction))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ListBoxSelectKoji = New System.Windows.Forms.ListBox()
        Me.ButtonCancel = New System.Windows.Forms.Button()
        Me.ButtonKojiOpen = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'ListBoxSelectKoji
        '
        resources.ApplyResources(Me.ListBoxSelectKoji, "ListBoxSelectKoji")
        Me.ListBoxSelectKoji.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ListBoxSelectKoji.Cursor = System.Windows.Forms.Cursors.Default
        Me.ListBoxSelectKoji.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ListBoxSelectKoji.FormattingEnabled = True
        Me.ListBoxSelectKoji.Name = "ListBoxSelectKoji"
        '
        'ButtonCancel
        '
        resources.ApplyResources(Me.ButtonCancel, "ButtonCancel")
        Me.ButtonCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.ButtonCancel.FlatAppearance.BorderSize = 0
        Me.ButtonCancel.ForeColor = System.Drawing.Color.White
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.UseVisualStyleBackColor = False
        '
        'ButtonKojiOpen
        '
        resources.ApplyResources(Me.ButtonKojiOpen, "ButtonKojiOpen")
        Me.ButtonKojiOpen.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.ButtonKojiOpen.FlatAppearance.BorderSize = 0
        Me.ButtonKojiOpen.ForeColor = System.Drawing.Color.White
        Me.ButtonKojiOpen.Name = "ButtonKojiOpen"
        Me.ButtonKojiOpen.UseVisualStyleBackColor = False
        '
        'JCBOpenConstruction
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(251, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.Controls.Add(Me.ButtonKojiOpen)
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.ListBoxSelectKoji)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "JCBOpenConstruction"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents ListBoxSelectKoji As ListBox
    Friend WithEvents ButtonCancel As Button
    Friend WithEvents ButtonKojiOpen As Button
End Class
