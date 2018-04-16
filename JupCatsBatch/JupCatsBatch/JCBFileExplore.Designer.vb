<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class JCBFileExplore
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(JCBFileExplore))
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ButtonSetting = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.TreeViewFileType = New System.Windows.Forms.TreeView()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.White
        Me.Button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.Button1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.Button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Font = New System.Drawing.Font("MS UI Gothic", 7.0!)
        Me.Button1.Image = CType(resources.GetObject("Button1.Image"), System.Drawing.Image)
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.Button1.Location = New System.Drawing.Point(0, -2)
        Me.Button1.Margin = New System.Windows.Forms.Padding(0)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(60, 70)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "フィルタ"
        Me.Button1.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.Button1.UseVisualStyleBackColor = False
        '
        'ButtonSetting
        '
        Me.ButtonSetting.BackColor = System.Drawing.Color.White
        Me.ButtonSetting.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonSetting.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ButtonSetting.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ButtonSetting.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.ButtonSetting.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White
        Me.ButtonSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonSetting.Font = New System.Drawing.Font("MS UI Gothic", 7.0!)
        Me.ButtonSetting.Image = CType(resources.GetObject("ButtonSetting.Image"), System.Drawing.Image)
        Me.ButtonSetting.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonSetting.Location = New System.Drawing.Point(60, -2)
        Me.ButtonSetting.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonSetting.Name = "ButtonSetting"
        Me.ButtonSetting.Size = New System.Drawing.Size(60, 70)
        Me.ButtonSetting.TabIndex = 5
        Me.ButtonSetting.Text = "設定"
        Me.ButtonSetting.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ButtonSetting.UseVisualStyleBackColor = False
        '
        'Panel2
        '
        Me.Panel2.AutoScroll = True
        Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.TreeViewFileType)
        Me.Panel2.Controls.Add(Me.ListView1)
        Me.Panel2.Location = New System.Drawing.Point(9, 87)
        Me.Panel2.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(883, 472)
        Me.Panel2.TabIndex = 6
        '
        'TreeViewFileType
        '
        Me.TreeViewFileType.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TreeViewFileType.Indent = 19
        Me.TreeViewFileType.ItemHeight = 25
        Me.TreeViewFileType.Location = New System.Drawing.Point(0, 0)
        Me.TreeViewFileType.Margin = New System.Windows.Forms.Padding(0)
        Me.TreeViewFileType.MinimumSize = New System.Drawing.Size(0, 468)
        Me.TreeViewFileType.Name = "TreeViewFileType"
        Me.TreeViewFileType.Scrollable = False
        Me.TreeViewFileType.ShowLines = False
        Me.TreeViewFileType.Size = New System.Drawing.Size(190, 468)
        Me.TreeViewFileType.TabIndex = 2
        '
        'ListView1
        '
        Me.ListView1.BackColor = System.Drawing.Color.White
        Me.ListView1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ListView1.GridLines = True
        Me.ListView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.ListView1.Location = New System.Drawing.Point(192, 0)
        Me.ListView1.Margin = New System.Windows.Forms.Padding(0)
        Me.ListView1.MinimumSize = New System.Drawing.Size(0, 468)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Scrollable = False
        Me.ListView1.Size = New System.Drawing.Size(671, 468)
        Me.ListView1.TabIndex = 0
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'JCBFileExplore
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(254, Byte), Integer), CType(CType(239, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(905, 568)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.ButtonSetting)
        Me.Controls.Add(Me.Button1)
        Me.Name = "JCBFileExplore"
        Me.Text = "ファイルエクスプローラ"
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents Button1 As Button
    Friend WithEvents ButtonSetting As Button
    Friend WithEvents Panel2 As Panel
    Friend WithEvents TreeViewFileType As TreeView
    Friend WithEvents ListView1 As ListView
End Class
