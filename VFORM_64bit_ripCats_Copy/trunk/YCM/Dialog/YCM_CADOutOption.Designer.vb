<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class YCM_CADOutOption
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

    'Windows フォーム デザイナで必要です。

    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。

    'Windows フォーム デザイナを使用して変更できます。  
    'コード エディタを使って変更しないでください。

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.CheckBox_LookPoint = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Ray = New System.Windows.Forms.CheckBox()
        Me.CheckBox_LabelText = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.CheckBox_Reconstruction = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Information = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox_TextH = New System.Windows.Forms.TextBox()
        Me.CheckBox_UserElm = New System.Windows.Forms.CheckBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Button_SelFile = New System.Windows.Forms.Button()
        Me.TextBox_CADFileName = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.StatusStrip = New System.Windows.Forms.StatusStrip()
        Me.ComboBox_OutDim = New System.Windows.Forms.ComboBox()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(366, 205)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 27)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 21)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 21)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "キャンセル"
        '
        'CheckBox_LookPoint
        '
        Me.CheckBox_LookPoint.AutoSize = True
        Me.CheckBox_LookPoint.Location = New System.Drawing.Point(16, 18)
        Me.CheckBox_LookPoint.Name = "CheckBox_LookPoint"
        Me.CheckBox_LookPoint.Size = New System.Drawing.Size(93, 16)
        Me.CheckBox_LookPoint.TabIndex = 1
        Me.CheckBox_LookPoint.Text = "計測点を出力"
        Me.CheckBox_LookPoint.UseVisualStyleBackColor = True
        '
        'CheckBox_Ray
        '
        Me.CheckBox_Ray.AutoSize = True
        Me.CheckBox_Ray.Location = New System.Drawing.Point(16, 40)
        Me.CheckBox_Ray.Name = "CheckBox_Ray"
        Me.CheckBox_Ray.Size = New System.Drawing.Size(75, 16)
        Me.CheckBox_Ray.TabIndex = 2
        Me.CheckBox_Ray.Text = "レイを出力"
        Me.CheckBox_Ray.UseVisualStyleBackColor = True
        '
        'CheckBox_LabelText
        '
        Me.CheckBox_LabelText.AutoSize = True
        Me.CheckBox_LabelText.Location = New System.Drawing.Point(16, 62)
        Me.CheckBox_LabelText.Name = "CheckBox_LabelText"
        Me.CheckBox_LabelText.Size = New System.Drawing.Size(109, 16)
        Me.CheckBox_LabelText.TabIndex = 3
        Me.CheckBox_LabelText.Text = "ラベル文字を出力"
        Me.CheckBox_LabelText.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.CheckBox_Reconstruction)
        Me.GroupBox1.Controls.Add(Me.CheckBox_Information)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.TextBox_TextH)
        Me.GroupBox1.Controls.Add(Me.CheckBox_UserElm)
        Me.GroupBox1.Controls.Add(Me.CheckBox_LookPoint)
        Me.GroupBox1.Controls.Add(Me.CheckBox_LabelText)
        Me.GroupBox1.Controls.Add(Me.CheckBox_Ray)
        Me.GroupBox1.Location = New System.Drawing.Point(11, 77)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(504, 119)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "CAD出力図形の選択"
        '
        'CheckBox_Reconstruction
        '
        Me.CheckBox_Reconstruction.AutoSize = True
        Me.CheckBox_Reconstruction.Location = New System.Drawing.Point(349, 43)
        Me.CheckBox_Reconstruction.Name = "CheckBox_Reconstruction"
        Me.CheckBox_Reconstruction.Size = New System.Drawing.Size(152, 16)
        Me.CheckBox_Reconstruction.TabIndex = 8
        Me.CheckBox_Reconstruction.Text = "3D Reconstructed Points"
        Me.CheckBox_Reconstruction.UseVisualStyleBackColor = True
        '
        'CheckBox_Information
        '
        Me.CheckBox_Information.AutoSize = True
        Me.CheckBox_Information.Location = New System.Drawing.Point(349, 18)
        Me.CheckBox_Information.Name = "CheckBox_Information"
        Me.CheckBox_Information.Size = New System.Drawing.Size(72, 16)
        Me.CheckBox_Information.TabIndex = 7
        Me.CheckBox_Information.Text = "基本情報"
        Me.CheckBox_Information.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(161, 63)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(41, 12)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "文字高"
        '
        'TextBox_TextH
        '
        Me.TextBox_TextH.Location = New System.Drawing.Point(208, 60)
        Me.TextBox_TextH.Name = "TextBox_TextH"
        Me.TextBox_TextH.Size = New System.Drawing.Size(75, 19)
        Me.TextBox_TextH.TabIndex = 5
        '
        'CheckBox_UserElm
        '
        Me.CheckBox_UserElm.AutoSize = True
        Me.CheckBox_UserElm.Location = New System.Drawing.Point(16, 84)
        Me.CheckBox_UserElm.Name = "CheckBox_UserElm"
        Me.CheckBox_UserElm.Size = New System.Drawing.Size(135, 16)
        Me.CheckBox_UserElm.TabIndex = 4
        Me.CheckBox_UserElm.Text = "ユーザ作成図形を出力"
        Me.CheckBox_UserElm.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Button_SelFile)
        Me.GroupBox2.Controls.Add(Me.TextBox_CADFileName)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Location = New System.Drawing.Point(11, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(504, 59)
        Me.GroupBox2.TabIndex = 5
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "図面ファイル名"
        '
        'Button_SelFile
        '
        Me.Button_SelFile.Location = New System.Drawing.Point(434, 21)
        Me.Button_SelFile.Name = "Button_SelFile"
        Me.Button_SelFile.Size = New System.Drawing.Size(56, 24)
        Me.Button_SelFile.TabIndex = 2
        Me.Button_SelFile.Text = "参照..."
        Me.Button_SelFile.UseVisualStyleBackColor = True
        '
        'TextBox_CADFileName
        '
        Me.TextBox_CADFileName.Location = New System.Drawing.Point(98, 24)
        Me.TextBox_CADFileName.Name = "TextBox_CADFileName"
        Me.TextBox_CADFileName.Size = New System.Drawing.Size(320, 19)
        Me.TextBox_CADFileName.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(14, 27)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(75, 12)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "図面ファイル名"
        '
        'StatusStrip
        '
        Me.StatusStrip.Location = New System.Drawing.Point(0, 235)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Size = New System.Drawing.Size(524, 22)
        Me.StatusStrip.TabIndex = 6
        Me.StatusStrip.Text = "StatusStrip1"
        '
        'ComboBox_OutDim
        '
        Me.ComboBox_OutDim.FormattingEnabled = True
        Me.ComboBox_OutDim.Items.AddRange(New Object() {"３次元", "XY平面", "XZ平面", "YZ平面"})
        Me.ComboBox_OutDim.Location = New System.Drawing.Point(109, 202)
        Me.ComboBox_OutDim.Name = "ComboBox_OutDim"
        Me.ComboBox_OutDim.Size = New System.Drawing.Size(103, 20)
        Me.ComboBox_OutDim.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(25, 205)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(72, 12)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "作成する次元"
        '
        'YCM_CADOutOption
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(524, 257)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ComboBox_OutDim)
        Me.Controls.Add(Me.StatusStrip)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "YCM_CADOutOption"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "CAD図書出し"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents CheckBox_LookPoint As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Ray As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_LabelText As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox_TextH As System.Windows.Forms.TextBox
    Friend WithEvents CheckBox_UserElm As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox_CADFileName As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents ComboBox_OutDim As System.Windows.Forms.ComboBox
    Friend WithEvents Button_SelFile As System.Windows.Forms.Button
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents CheckBox_Information As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Reconstruction As System.Windows.Forms.CheckBox

End Class
