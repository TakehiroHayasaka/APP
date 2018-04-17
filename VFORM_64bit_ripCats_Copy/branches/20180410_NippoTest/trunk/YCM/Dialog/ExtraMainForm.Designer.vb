<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ExtraMainForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ExtraMainForm))
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.Button8 = New System.Windows.Forms.Button()
        Me.pnlOut = New System.Windows.Forms.Panel()
        Me.pnlKakunin = New System.Windows.Forms.Panel()
        Me.pnl解析 = New System.Windows.Forms.Panel()
        Me.pnl画像 = New System.Windows.Forms.Panel()
        Me.pnlKitei = New System.Windows.Forms.Panel()
        Me.pnlKihon = New System.Windows.Forms.Panel()
        Me.Button9 = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.pnlKihon.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.Button6)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Button5)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Button2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Button4)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Button3)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Button1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.SplitContainer2)
        Me.SplitContainer1.Size = New System.Drawing.Size(948, 668)
        Me.SplitContainer1.SplitterDistance = 91
        Me.SplitContainer1.TabIndex = 0
        '
        'Button6
        '
        Me.Button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button6.Font = New System.Drawing.Font("MS UI Gothic", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button6.Location = New System.Drawing.Point(787, 3)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(149, 69)
        Me.Button6.TabIndex = 6
        Me.Button6.Text = "出力"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button5.Font = New System.Drawing.Font("MS UI Gothic", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button5.Location = New System.Drawing.Point(632, 3)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(149, 69)
        Me.Button5.TabIndex = 5
        Me.Button5.Text = "確認"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button2.Font = New System.Drawing.Font("MS UI Gothic", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button2.Location = New System.Drawing.Point(477, 3)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(149, 69)
        Me.Button2.TabIndex = 4
        Me.Button2.Text = "解析"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button4.Font = New System.Drawing.Font("MS UI Gothic", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button4.Location = New System.Drawing.Point(322, 3)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(149, 69)
        Me.Button4.TabIndex = 3
        Me.Button4.Text = "画像"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button3.Font = New System.Drawing.Font("MS UI Gothic", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button3.Location = New System.Drawing.Point(167, 3)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(149, 69)
        Me.Button3.TabIndex = 2
        Me.Button3.Text = "規定値"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Font = New System.Drawing.Font("MS UI Gothic", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button1.Location = New System.Drawing.Point(12, 3)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(149, 69)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "基本情報"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.Button8)
        Me.SplitContainer2.Panel1.Controls.Add(Me.pnlOut)
        Me.SplitContainer2.Panel1.Controls.Add(Me.pnlKakunin)
        Me.SplitContainer2.Panel1.Controls.Add(Me.pnl解析)
        Me.SplitContainer2.Panel1.Controls.Add(Me.pnl画像)
        Me.SplitContainer2.Panel1.Controls.Add(Me.pnlKitei)
        Me.SplitContainer2.Panel1.Controls.Add(Me.pnlKihon)
        Me.SplitContainer2.Size = New System.Drawing.Size(948, 573)
        Me.SplitContainer2.SplitterDistance = 507
        Me.SplitContainer2.TabIndex = 0
        '
        'Button8
        '
        Me.Button8.Location = New System.Drawing.Point(756, 380)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(180, 46)
        Me.Button8.TabIndex = 6
        Me.Button8.Text = "次へ"
        Me.Button8.UseVisualStyleBackColor = True
        '
        'pnlOut
        '
        Me.pnlOut.Location = New System.Drawing.Point(769, 39)
        Me.pnlOut.Name = "pnlOut"
        Me.pnlOut.Size = New System.Drawing.Size(123, 97)
        Me.pnlOut.TabIndex = 5
        '
        'pnlKakunin
        '
        Me.pnlKakunin.Location = New System.Drawing.Point(614, 39)
        Me.pnlKakunin.Name = "pnlKakunin"
        Me.pnlKakunin.Size = New System.Drawing.Size(123, 97)
        Me.pnlKakunin.TabIndex = 4
        '
        'pnl解析
        '
        Me.pnl解析.Location = New System.Drawing.Point(469, 39)
        Me.pnl解析.Name = "pnl解析"
        Me.pnl解析.Size = New System.Drawing.Size(123, 97)
        Me.pnl解析.TabIndex = 3
        '
        'pnl画像
        '
        Me.pnl画像.Location = New System.Drawing.Point(316, 39)
        Me.pnl画像.Name = "pnl画像"
        Me.pnl画像.Size = New System.Drawing.Size(123, 97)
        Me.pnl画像.TabIndex = 2
        '
        'pnlKitei
        '
        Me.pnlKitei.Location = New System.Drawing.Point(170, 39)
        Me.pnlKitei.Name = "pnlKitei"
        Me.pnlKitei.Size = New System.Drawing.Size(123, 97)
        Me.pnlKitei.TabIndex = 1
        '
        'pnlKihon
        '
        Me.pnlKihon.Controls.Add(Me.Button9)
        Me.pnlKihon.Controls.Add(Me.TextBox1)
        Me.pnlKihon.Controls.Add(Me.Button7)
        Me.pnlKihon.Controls.Add(Me.PictureBox1)
        Me.pnlKihon.Location = New System.Drawing.Point(3, 3)
        Me.pnlKihon.Name = "pnlKihon"
        Me.pnlKihon.Size = New System.Drawing.Size(631, 423)
        Me.pnlKihon.TabIndex = 0
        '
        'Button9
        '
        Me.Button9.Location = New System.Drawing.Point(393, 12)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(75, 23)
        Me.Button9.TabIndex = 3
        Me.Button9.Text = "Button9"
        Me.Button9.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(27, 16)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(350, 19)
        Me.TextBox1.TabIndex = 2
        '
        'Button7
        '
        Me.Button7.Location = New System.Drawing.Point(409, 374)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(180, 46)
        Me.Button7.TabIndex = 1
        Me.Button7.Text = "他の工事データから読込"
        Me.Button7.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(17, 48)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(386, 372)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'ExtraMainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(948, 668)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "ExtraMainForm"
        Me.Text = "VForm 京浜産業"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.pnlKihon.ResumeLayout(False)
        Me.pnlKihon.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents pnlOut As System.Windows.Forms.Panel
    Friend WithEvents pnlKakunin As System.Windows.Forms.Panel
    Friend WithEvents pnl解析 As System.Windows.Forms.Panel
    Friend WithEvents pnl画像 As System.Windows.Forms.Panel
    Friend WithEvents pnlKitei As System.Windows.Forms.Panel
    Friend WithEvents pnlKihon As System.Windows.Forms.Panel
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Button8 As System.Windows.Forms.Button
    Friend WithEvents Button9 As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
End Class
