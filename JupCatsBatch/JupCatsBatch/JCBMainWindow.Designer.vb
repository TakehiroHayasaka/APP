<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class JCBMainWindow
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(JCBMainWindow))
        Me.subWindowButton = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.allExeButton = New System.Windows.Forms.Button()
        Me.TextBoxKojiFolder = New System.Windows.Forms.TextBox()
        Me.TextBoxKojiName = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextBoxKozo = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBoxKogo = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.ButtonSetting = New System.Windows.Forms.Button()
        Me.ButtonKojiEdit = New System.Windows.Forms.Button()
        Me.ButtonKojiOpen = New System.Windows.Forms.Button()
        Me.ButtonKojiNew = New System.Windows.Forms.Button()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.PanelSubWindowToolBar = New System.Windows.Forms.Panel()
        Me.PanelSubWindowMain = New System.Windows.Forms.Panel()
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.SuspendLayout()
        '
        'subWindowButton
        '
        resources.ApplyResources(Me.subWindowButton, "subWindowButton")
        Me.subWindowButton.Cursor = System.Windows.Forms.Cursors.Hand
        Me.subWindowButton.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.subWindowButton.FlatAppearance.BorderSize = 0
        Me.subWindowButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.subWindowButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White
        Me.subWindowButton.Name = "subWindowButton"
        Me.subWindowButton.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        resources.ApplyResources(Me.Panel1, "Panel1")
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(251, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.Panel1.Controls.Add(Me.allExeButton)
        Me.Panel1.Controls.Add(Me.TextBoxKojiFolder)
        Me.Panel1.Controls.Add(Me.TextBoxKojiName)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.TextBoxKozo)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.TextBoxKogo)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Name = "Panel1"
        '
        'allExeButton
        '
        resources.ApplyResources(Me.allExeButton, "allExeButton")
        Me.allExeButton.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.allExeButton.Cursor = System.Windows.Forms.Cursors.Hand
        Me.allExeButton.FlatAppearance.BorderSize = 0
        Me.allExeButton.ForeColor = System.Drawing.Color.White
        Me.allExeButton.Name = "allExeButton"
        Me.allExeButton.UseVisualStyleBackColor = False
        '
        'TextBoxKojiFolder
        '
        resources.ApplyResources(Me.TextBoxKojiFolder, "TextBoxKojiFolder")
        Me.TextBoxKojiFolder.BackColor = System.Drawing.Color.White
        Me.TextBoxKojiFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxKojiFolder.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.TextBoxKojiFolder.Name = "TextBoxKojiFolder"
        Me.TextBoxKojiFolder.ReadOnly = True
        '
        'TextBoxKojiName
        '
        resources.ApplyResources(Me.TextBoxKojiName, "TextBoxKojiName")
        Me.TextBoxKojiName.BackColor = System.Drawing.Color.White
        Me.TextBoxKojiName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxKojiName.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.TextBoxKojiName.Name = "TextBoxKojiName"
        Me.TextBoxKojiName.ReadOnly = True
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'TextBoxKozo
        '
        resources.ApplyResources(Me.TextBoxKozo, "TextBoxKozo")
        Me.TextBoxKozo.BackColor = System.Drawing.Color.White
        Me.TextBoxKozo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxKozo.Cursor = System.Windows.Forms.Cursors.Default
        Me.TextBoxKozo.Name = "TextBoxKozo"
        Me.TextBoxKozo.ReadOnly = True
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'TextBoxKogo
        '
        resources.ApplyResources(Me.TextBoxKogo, "TextBoxKogo")
        Me.TextBoxKogo.BackColor = System.Drawing.Color.White
        Me.TextBoxKogo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxKogo.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.TextBoxKogo.Name = "TextBoxKogo"
        Me.TextBoxKogo.ReadOnly = True
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'Panel3
        '
        resources.ApplyResources(Me.Panel3, "Panel3")
        Me.Panel3.BackColor = System.Drawing.Color.White
        Me.Panel3.Controls.Add(Me.ButtonSetting)
        Me.Panel3.Controls.Add(Me.ButtonKojiEdit)
        Me.Panel3.Controls.Add(Me.ButtonKojiOpen)
        Me.Panel3.Controls.Add(Me.ButtonKojiNew)
        Me.Panel3.Name = "Panel3"
        '
        'ButtonSetting
        '
        resources.ApplyResources(Me.ButtonSetting, "ButtonSetting")
        Me.ButtonSetting.BackColor = System.Drawing.Color.White
        Me.ButtonSetting.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ButtonSetting.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.ButtonSetting.FlatAppearance.BorderSize = 0
        Me.ButtonSetting.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ButtonSetting.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ButtonSetting.Name = "ButtonSetting"
        Me.ButtonSetting.UseVisualStyleBackColor = False
        '
        'ButtonKojiEdit
        '
        resources.ApplyResources(Me.ButtonKojiEdit, "ButtonKojiEdit")
        Me.ButtonKojiEdit.BackColor = System.Drawing.Color.White
        Me.ButtonKojiEdit.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ButtonKojiEdit.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.ButtonKojiEdit.FlatAppearance.BorderSize = 0
        Me.ButtonKojiEdit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ButtonKojiEdit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ButtonKojiEdit.Name = "ButtonKojiEdit"
        Me.ButtonKojiEdit.UseVisualStyleBackColor = False
        '
        'ButtonKojiOpen
        '
        resources.ApplyResources(Me.ButtonKojiOpen, "ButtonKojiOpen")
        Me.ButtonKojiOpen.BackColor = System.Drawing.Color.White
        Me.ButtonKojiOpen.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ButtonKojiOpen.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.ButtonKojiOpen.FlatAppearance.BorderSize = 0
        Me.ButtonKojiOpen.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ButtonKojiOpen.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ButtonKojiOpen.Name = "ButtonKojiOpen"
        Me.ButtonKojiOpen.UseVisualStyleBackColor = False
        '
        'ButtonKojiNew
        '
        resources.ApplyResources(Me.ButtonKojiNew, "ButtonKojiNew")
        Me.ButtonKojiNew.BackColor = System.Drawing.Color.White
        Me.ButtonKojiNew.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ButtonKojiNew.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.ButtonKojiNew.FlatAppearance.BorderSize = 0
        Me.ButtonKojiNew.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ButtonKojiNew.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ButtonKojiNew.Name = "ButtonKojiNew"
        Me.ButtonKojiNew.UseVisualStyleBackColor = False
        '
        'Panel4
        '
        resources.ApplyResources(Me.Panel4, "Panel4")
        Me.Panel4.BackColor = System.Drawing.Color.White
        Me.Panel4.Controls.Add(Me.Label6)
        Me.Panel4.Controls.Add(Me.Label5)
        Me.Panel4.Controls.Add(Me.subWindowButton)
        Me.Panel4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Panel4.Name = "Panel4"
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.BackColor = System.Drawing.Color.Gainsboro
        Me.Label6.Name = "Label6"
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.BackColor = System.Drawing.Color.Gainsboro
        Me.Label5.Name = "Label5"
        '
        'PanelSubWindowToolBar
        '
        resources.ApplyResources(Me.PanelSubWindowToolBar, "PanelSubWindowToolBar")
        Me.PanelSubWindowToolBar.BackColor = System.Drawing.Color.White
        Me.PanelSubWindowToolBar.Name = "PanelSubWindowToolBar"
        '
        'PanelSubWindowMain
        '
        resources.ApplyResources(Me.PanelSubWindowMain, "PanelSubWindowMain")
        Me.PanelSubWindowMain.BackColor = System.Drawing.Color.Gray
        Me.PanelSubWindowMain.Name = "PanelSubWindowMain"
        '
        'JCBMainWindow
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "JCBMainWindow"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents subWindowButton As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Panel4 As Panel
    Friend WithEvents PanelSubWindowToolBar As Panel
    Friend WithEvents PanelSubWindowMain As Panel
    Public WithEvents ButtonKojiNew As Button
    Friend WithEvents ButtonKojiOpen As Button
    Friend WithEvents ButtonKojiEdit As Button
    Friend WithEvents ButtonSetting As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents TextBoxKozo As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBoxKogo As TextBox
    Friend WithEvents TextBoxKojiFolder As TextBox
    Friend WithEvents TextBoxKojiName As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents allExeButton As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
End Class
