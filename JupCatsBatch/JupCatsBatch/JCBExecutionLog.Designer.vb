<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class JCBExecutionLog
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(JCBExecutionLog))
        Me.RichTextBoxLog = New System.Windows.Forms.RichTextBox()
        Me.RadioButtonAbnormal = New System.Windows.Forms.RadioButton()
        Me.RadioButtonAllShow = New System.Windows.Forms.RadioButton()
        Me.RadioButtonFinishedMessage = New System.Windows.Forms.RadioButton()
        Me.RadioButtonAbnormalWorning = New System.Windows.Forms.RadioButton()
        Me.CheckBoxCombineMessages = New System.Windows.Forms.CheckBox()
        Me.openLogButton = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'RichTextBoxLog
        '
        Me.RichTextBoxLog.BackColor = System.Drawing.Color.White
        resources.ApplyResources(Me.RichTextBoxLog, "RichTextBoxLog")
        Me.RichTextBoxLog.Name = "RichTextBoxLog"
        Me.RichTextBoxLog.ReadOnly = True
        '
        'RadioButtonAbnormal
        '
        resources.ApplyResources(Me.RadioButtonAbnormal, "RadioButtonAbnormal")
        Me.RadioButtonAbnormal.Checked = True
        Me.RadioButtonAbnormal.Name = "RadioButtonAbnormal"
        Me.RadioButtonAbnormal.TabStop = True
        Me.RadioButtonAbnormal.UseVisualStyleBackColor = True
        '
        'RadioButtonAllShow
        '
        resources.ApplyResources(Me.RadioButtonAllShow, "RadioButtonAllShow")
        Me.RadioButtonAllShow.Name = "RadioButtonAllShow"
        Me.RadioButtonAllShow.UseVisualStyleBackColor = True
        '
        'RadioButtonFinishedMessage
        '
        resources.ApplyResources(Me.RadioButtonFinishedMessage, "RadioButtonFinishedMessage")
        Me.RadioButtonFinishedMessage.Name = "RadioButtonFinishedMessage"
        Me.RadioButtonFinishedMessage.UseVisualStyleBackColor = True
        '
        'RadioButtonAbnormalWorning
        '
        resources.ApplyResources(Me.RadioButtonAbnormalWorning, "RadioButtonAbnormalWorning")
        Me.RadioButtonAbnormalWorning.Name = "RadioButtonAbnormalWorning"
        Me.RadioButtonAbnormalWorning.UseVisualStyleBackColor = True
        '
        'CheckBoxCombineMessages
        '
        resources.ApplyResources(Me.CheckBoxCombineMessages, "CheckBoxCombineMessages")
        Me.CheckBoxCombineMessages.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckBoxCombineMessages.Name = "CheckBoxCombineMessages"
        Me.CheckBoxCombineMessages.UseVisualStyleBackColor = True
        '
        'openLogButton
        '
        Me.openLogButton.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.openLogButton.Cursor = System.Windows.Forms.Cursors.Hand
        Me.openLogButton.FlatAppearance.BorderSize = 0
        resources.ApplyResources(Me.openLogButton, "openLogButton")
        Me.openLogButton.ForeColor = System.Drawing.Color.White
        Me.openLogButton.Name = "openLogButton"
        Me.openLogButton.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.RadioButtonAbnormal)
        Me.Panel1.Controls.Add(Me.openLogButton)
        Me.Panel1.Controls.Add(Me.RadioButtonAbnormalWorning)
        Me.Panel1.Controls.Add(Me.CheckBoxCombineMessages)
        Me.Panel1.Controls.Add(Me.RadioButtonFinishedMessage)
        Me.Panel1.Controls.Add(Me.RadioButtonAllShow)
        resources.ApplyResources(Me.Panel1, "Panel1")
        Me.Panel1.Name = "Panel1"
        '
        'JCBExecutionLog
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(251, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.RichTextBoxLog)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Name = "JCBExecutionLog"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RichTextBoxLog As RichTextBox
    Friend WithEvents RadioButtonAbnormal As RadioButton
    Friend WithEvents RadioButtonAllShow As RadioButton
    Friend WithEvents RadioButtonFinishedMessage As RadioButton
    Friend WithEvents RadioButtonAbnormalWorning As RadioButton
    Friend WithEvents CheckBoxCombineMessages As CheckBox
    Friend WithEvents openLogButton As Button
    Friend WithEvents Panel1 As Panel
End Class
