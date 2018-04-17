<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form2
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
        'Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form2))
        ''Me.BtnOut = New System.Windows.Forms.Button()
        ''Me.Label1 = New System.Windows.Forms.Label()
        ''Me.Label2 = New System.Windows.Forms.Label()
        ''Me.ImageListView2 = New System.Windows.Forms.ListView()
        ''Me.ImageListView1 = New System.Windows.Forms.ListView()
        ''Me.AxHWindowXCtrl1 = New AxHALCONXLib.AxHWindowXCtrl()
        ''Me.AxHWindowXCtrl2 = New AxHALCONXLib.AxHWindowXCtrl()
        ''CType(Me.AxHWindowXCtrl1, System.ComponentModel.ISupportInitialize).BeginInit()
        ''CType(Me.AxHWindowXCtrl2, System.ComponentModel.ISupportInitialize).BeginInit()
        ''Me.SuspendLayout()
        ' ''
        ' ''BtnOut
        ' ''
        ''Me.BtnOut.Location = New System.Drawing.Point(938, 618)
        ''Me.BtnOut.Name = "BtnOut"
        ''Me.BtnOut.Size = New System.Drawing.Size(75, 35)
        ''Me.BtnOut.TabIndex = 5
        ''Me.BtnOut.Text = "出力"
        ''Me.BtnOut.UseVisualStyleBackColor = True
        ' ''
        ' ''Label1
        ' ''
        ''Me.Label1.AutoSize = True
        ''Me.Label1.Location = New System.Drawing.Point(12, 21)
        ''Me.Label1.Name = "Label1"
        ''Me.Label1.Size = New System.Drawing.Size(36, 12)
        ''Me.Label1.TabIndex = 6
        ''Me.Label1.Text = "カメラ1"
        ' ''
        ' ''Label2
        ' ''
        ''Me.Label2.AutoSize = True
        ''Me.Label2.Location = New System.Drawing.Point(524, 21)
        ''Me.Label2.Name = "Label2"
        ''Me.Label2.Size = New System.Drawing.Size(36, 12)
        ''Me.Label2.TabIndex = 7
        ''Me.Label2.Text = "カメラ2"
        ' ''
        ' ''ImageListView2
        ' ''
        ''Me.ImageListView2.CheckBoxes = True
        ''Me.ImageListView2.Location = New System.Drawing.Point(526, 377)
        ''Me.ImageListView2.Name = "ImageListView2"
        ''Me.ImageListView2.Size = New System.Drawing.Size(487, 235)
        ''Me.ImageListView2.TabIndex = 8
        ''Me.ImageListView2.UseCompatibleStateImageBehavior = False
        ' ''
        ' ''ImageListView1
        ' ''
        ''Me.ImageListView1.CheckBoxes = True
        ''Me.ImageListView1.Location = New System.Drawing.Point(14, 377)
        ''Me.ImageListView1.Name = "ImageListView1"
        ''Me.ImageListView1.Size = New System.Drawing.Size(487, 235)
        ''Me.ImageListView1.TabIndex = 8
        ''Me.ImageListView1.UseCompatibleStateImageBehavior = False
        ' ''
        ' ''AxHWindowXCtrl1
        ' ''
        ''Me.AxHWindowXCtrl1.Enabled = True
        ''Me.AxHWindowXCtrl1.Location = New System.Drawing.Point(12, 36)
        ''Me.AxHWindowXCtrl1.Name = "AxHWindowXCtrl1"
        ''Me.AxHWindowXCtrl1.OcxState = CType(resources.GetObject("AxHWindowXCtrl1.OcxState"), System.Windows.Forms.AxHost.State)
        ''Me.AxHWindowXCtrl1.Size = New System.Drawing.Size(487, 313)
        ''Me.AxHWindowXCtrl1.TabIndex = 3
        ' ''
        ' ''AxHWindowXCtrl2
        ' ''
        ''Me.AxHWindowXCtrl2.Enabled = True
        ''Me.AxHWindowXCtrl2.Location = New System.Drawing.Point(526, 36)
        ''Me.AxHWindowXCtrl2.Name = "AxHWindowXCtrl2"
        ''Me.AxHWindowXCtrl2.OcxState = CType(resources.GetObject("AxHWindowXCtrl2.OcxState"), System.Windows.Forms.AxHost.State)
        ''Me.AxHWindowXCtrl2.Size = New System.Drawing.Size(487, 313)
        ''Me.AxHWindowXCtrl2.TabIndex = 3
        ''
        ''Form2
        ''
        'Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        'Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        ''Me.ClientSize = New System.Drawing.Size(1060, 665)
        ''Me.Controls.Add(Me.ImageListView1)
        ''Me.Controls.Add(Me.ImageListView2)
        ''Me.Controls.Add(Me.Label2)
        ''Me.Controls.Add(Me.Label1)
        ''Me.Controls.Add(Me.BtnOut)
        ''Me.Controls.Add(Me.AxHWindowXCtrl1)
        ''Me.Controls.Add(Me.AxHWindowXCtrl2)
        'Me.Name = "Form2"
        'Me.Text = "FrameGrab Ver0.0.1"
        ''CType(Me.AxHWindowXCtrl1, System.ComponentModel.ISupportInitialize).EndInit()
        ''CType(Me.AxHWindowXCtrl2, System.ComponentModel.ISupportInitialize).EndInit()
        'Me.ResumeLayout(False)
        'Me.PerformLayout()

    End Sub
    Friend WithEvents AxHWindowXCtrl2 As AxHALCONXLib.AxHWindowXCtrl
    Friend WithEvents BtnOut As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ImageListView2 As System.Windows.Forms.ListView
    Friend WithEvents AxHWindowXCtrl1 As AxHALCONXLib.AxHWindowXCtrl
    Friend WithEvents ImageListView1 As System.Windows.Forms.ListView
End Class
