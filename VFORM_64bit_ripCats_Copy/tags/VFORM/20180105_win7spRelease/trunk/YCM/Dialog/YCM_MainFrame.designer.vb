<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class YCM_MainFrame
    Inherits System.Windows.Forms.UserControl

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
        Me.components = New System.ComponentModel.Container()
        Me.ImgLst = New System.Windows.Forms.ImageList(Me.components)
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.ElementHost1 = New System.Windows.Forms.Integration.ElementHost()
        Me.RibbonMenuControl14 = New YCM.RibbonMenuControl1()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.ImageSplCnt = New System.Windows.Forms.SplitContainer()
        Me.halwinSplitContnr = New System.Windows.Forms.SplitContainer()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.AxHWindowXCtrl6 = New HalconDotNet.HWindowControl()
        Me.FourImageTableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.AxHWindowXCtrl2 = New HalconDotNet.HWindowControl()
        Me.AxHWindowXCtrl3 = New HalconDotNet.HWindowControl()
        Me.AxHWindowXCtrl4 = New HalconDotNet.HWindowControl()
        Me.AxHWindowXCtrl5 = New HalconDotNet.HWindowControl()
        Me.ImageListView = New System.Windows.Forms.ListView()
        Me.SplitContainer3 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer4 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer5 = New System.Windows.Forms.SplitContainer()
        Me.LBox_Data = New System.Windows.Forms.ListBox()
        Me.TBox_Data = New System.Windows.Forms.TextBox()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        CType(Me.ImageSplCnt, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ImageSplCnt.Panel2.SuspendLayout()
        Me.ImageSplCnt.SuspendLayout()
        CType(Me.halwinSplitContnr, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.halwinSplitContnr.Panel1.SuspendLayout()
        Me.halwinSplitContnr.Panel2.SuspendLayout()
        Me.halwinSplitContnr.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.FourImageTableLayoutPanel.SuspendLayout()
        CType(Me.SplitContainer3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer3.Panel1.SuspendLayout()
        Me.SplitContainer3.SuspendLayout()
        CType(Me.SplitContainer4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer4.Panel2.SuspendLayout()
        Me.SplitContainer4.SuspendLayout()
        CType(Me.SplitContainer5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer5.Panel1.SuspendLayout()
        Me.SplitContainer5.Panel2.SuspendLayout()
        Me.SplitContainer5.SuspendLayout()
        Me.SuspendLayout()
        '
        'ImgLst
        '
        Me.ImgLst.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.ImgLst.ImageSize = New System.Drawing.Size(96, 64)
        Me.ImgLst.TransparentColor = System.Drawing.Color.Transparent
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
        Me.SplitContainer1.Panel1.AutoScroll = True
        Me.SplitContainer1.Panel1.Controls.Add(Me.ElementHost1)
        Me.SplitContainer1.Panel1MinSize = 135
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.SplitContainer2)
        Me.SplitContainer1.Size = New System.Drawing.Size(1012, 741)
        Me.SplitContainer1.SplitterDistance = 139
        Me.SplitContainer1.TabIndex = 0
        '
        'ElementHost1
        '
        Me.ElementHost1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ElementHost1.Location = New System.Drawing.Point(0, 0)
        Me.ElementHost1.Name = "ElementHost1"
        Me.ElementHost1.Size = New System.Drawing.Size(1012, 139)
        Me.ElementHost1.TabIndex = 0
        Me.ElementHost1.Text = "ElementHost1"
        Me.ElementHost1.Child = Me.RibbonMenuControl14
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.ImageSplCnt)
        Me.SplitContainer2.Panel1MinSize = 0
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.BackColor = System.Drawing.Color.LemonChiffon
        Me.SplitContainer2.Panel2.Controls.Add(Me.SplitContainer3)
        Me.SplitContainer2.Panel2MinSize = 0
        Me.SplitContainer2.Size = New System.Drawing.Size(1012, 598)
        Me.SplitContainer2.SplitterDistance = 529
        Me.SplitContainer2.TabIndex = 0
        '
        'ImageSplCnt
        '
        Me.ImageSplCnt.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ImageSplCnt.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.ImageSplCnt.Location = New System.Drawing.Point(0, 0)
        Me.ImageSplCnt.Name = "ImageSplCnt"
        Me.ImageSplCnt.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.ImageSplCnt.Panel1Collapsed = True
        '
        'ImageSplCnt.Panel2
        '
        Me.ImageSplCnt.Panel2.Controls.Add(Me.halwinSplitContnr)
        Me.ImageSplCnt.Size = New System.Drawing.Size(529, 598)
        Me.ImageSplCnt.TabIndex = 11
        '
        'halwinSplitContnr
        '
        Me.halwinSplitContnr.Dock = System.Windows.Forms.DockStyle.Fill
        Me.halwinSplitContnr.Location = New System.Drawing.Point(0, 0)
        Me.halwinSplitContnr.Name = "halwinSplitContnr"
        Me.halwinSplitContnr.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'halwinSplitContnr.Panel1
        '
        Me.halwinSplitContnr.Panel1.Controls.Add(Me.Panel1)
        Me.halwinSplitContnr.Panel1.Controls.Add(Me.FourImageTableLayoutPanel)
        '
        'halwinSplitContnr.Panel2
        '
        Me.halwinSplitContnr.Panel2.Controls.Add(Me.ImageListView)
        Me.halwinSplitContnr.Size = New System.Drawing.Size(529, 598)
        Me.halwinSplitContnr.SplitterDistance = 501
        Me.halwinSplitContnr.TabIndex = 3
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.AxHWindowXCtrl6)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(529, 501)
        Me.Panel1.TabIndex = 1
        '
        'AxHWindowXCtrl6
        '
        Me.AxHWindowXCtrl6.BackColor = System.Drawing.Color.Black
        Me.AxHWindowXCtrl6.BorderColor = System.Drawing.Color.Black
        Me.AxHWindowXCtrl6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AxHWindowXCtrl6.ImagePart = New System.Drawing.Rectangle(0, 0, 640, 480)
        Me.AxHWindowXCtrl6.Location = New System.Drawing.Point(0, 0)
        Me.AxHWindowXCtrl6.Name = "AxHWindowXCtrl6"
        Me.AxHWindowXCtrl6.Size = New System.Drawing.Size(529, 501)
        Me.AxHWindowXCtrl6.TabIndex = 6
        Me.AxHWindowXCtrl6.WindowSize = New System.Drawing.Size(529, 501)
        '
        'FourImageTableLayoutPanel
        '
        Me.FourImageTableLayoutPanel.ColumnCount = 2
        Me.FourImageTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.FourImageTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.FourImageTableLayoutPanel.Controls.Add(Me.AxHWindowXCtrl2, 0, 0)
        Me.FourImageTableLayoutPanel.Controls.Add(Me.AxHWindowXCtrl3, 1, 0)
        Me.FourImageTableLayoutPanel.Controls.Add(Me.AxHWindowXCtrl4, 0, 1)
        Me.FourImageTableLayoutPanel.Controls.Add(Me.AxHWindowXCtrl5, 1, 1)
        Me.FourImageTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FourImageTableLayoutPanel.Location = New System.Drawing.Point(0, 0)
        Me.FourImageTableLayoutPanel.Name = "FourImageTableLayoutPanel"
        Me.FourImageTableLayoutPanel.RowCount = 2
        Me.FourImageTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.FourImageTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.FourImageTableLayoutPanel.Size = New System.Drawing.Size(529, 501)
        Me.FourImageTableLayoutPanel.TabIndex = 0
        '
        'AxHWindowXCtrl2
        '
        Me.AxHWindowXCtrl2.BackColor = System.Drawing.Color.Black
        Me.AxHWindowXCtrl2.BorderColor = System.Drawing.Color.Black
        Me.AxHWindowXCtrl2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AxHWindowXCtrl2.ImagePart = New System.Drawing.Rectangle(0, 0, 640, 480)
        Me.AxHWindowXCtrl2.Location = New System.Drawing.Point(3, 3)
        Me.AxHWindowXCtrl2.Name = "AxHWindowXCtrl2"
        Me.AxHWindowXCtrl2.Size = New System.Drawing.Size(258, 244)
        Me.AxHWindowXCtrl2.TabIndex = 1
        Me.AxHWindowXCtrl2.WindowSize = New System.Drawing.Size(258, 244)
        '
        'AxHWindowXCtrl3
        '
        Me.AxHWindowXCtrl3.BackColor = System.Drawing.Color.Black
        Me.AxHWindowXCtrl3.BorderColor = System.Drawing.Color.Black
        Me.AxHWindowXCtrl3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AxHWindowXCtrl3.ImagePart = New System.Drawing.Rectangle(0, 0, 640, 480)
        Me.AxHWindowXCtrl3.Location = New System.Drawing.Point(267, 3)
        Me.AxHWindowXCtrl3.Name = "AxHWindowXCtrl3"
        Me.AxHWindowXCtrl3.Size = New System.Drawing.Size(259, 244)
        Me.AxHWindowXCtrl3.TabIndex = 2
        Me.AxHWindowXCtrl3.WindowSize = New System.Drawing.Size(259, 244)
        '
        'AxHWindowXCtrl4
        '
        Me.AxHWindowXCtrl4.BackColor = System.Drawing.Color.Black
        Me.AxHWindowXCtrl4.BorderColor = System.Drawing.Color.Black
        Me.AxHWindowXCtrl4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AxHWindowXCtrl4.ImagePart = New System.Drawing.Rectangle(0, 0, 640, 480)
        Me.AxHWindowXCtrl4.Location = New System.Drawing.Point(3, 253)
        Me.AxHWindowXCtrl4.Name = "AxHWindowXCtrl4"
        Me.AxHWindowXCtrl4.Size = New System.Drawing.Size(258, 245)
        Me.AxHWindowXCtrl4.TabIndex = 3
        Me.AxHWindowXCtrl4.WindowSize = New System.Drawing.Size(258, 245)
        '
        'AxHWindowXCtrl5
        '
        Me.AxHWindowXCtrl5.BackColor = System.Drawing.Color.Black
        Me.AxHWindowXCtrl5.BorderColor = System.Drawing.Color.Black
        Me.AxHWindowXCtrl5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AxHWindowXCtrl5.ImagePart = New System.Drawing.Rectangle(0, 0, 640, 480)
        Me.AxHWindowXCtrl5.Location = New System.Drawing.Point(267, 253)
        Me.AxHWindowXCtrl5.Name = "AxHWindowXCtrl5"
        Me.AxHWindowXCtrl5.Size = New System.Drawing.Size(259, 245)
        Me.AxHWindowXCtrl5.TabIndex = 4
        Me.AxHWindowXCtrl5.WindowSize = New System.Drawing.Size(259, 245)
        '
        'ImageListView
        '
        Me.ImageListView.BackColor = System.Drawing.SystemColors.Window
        Me.ImageListView.CheckBoxes = True
        Me.ImageListView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ImageListView.LargeImageList = Me.ImgLst
        Me.ImageListView.Location = New System.Drawing.Point(0, 0)
        Me.ImageListView.Name = "ImageListView"
        Me.ImageListView.Size = New System.Drawing.Size(529, 93)
        Me.ImageListView.TabIndex = 1
        Me.ImageListView.UseCompatibleStateImageBehavior = False
        '
        'SplitContainer3
        '
        Me.SplitContainer3.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainer3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitContainer3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.SplitContainer3.IsSplitterFixed = True
        Me.SplitContainer3.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer3.Name = "SplitContainer3"
        '
        'SplitContainer3.Panel1
        '
        Me.SplitContainer3.Panel1.BackColor = System.Drawing.Color.White
        Me.SplitContainer3.Panel1.Controls.Add(Me.SplitContainer4)
        Me.SplitContainer3.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SplitContainer3.Panel1MinSize = 0
        '
        'SplitContainer3.Panel2
        '
        Me.SplitContainer3.Panel2.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainer3.Panel2.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.SplitContainer3.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SplitContainer3.Panel2MinSize = 0
        Me.SplitContainer3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SplitContainer3.Size = New System.Drawing.Size(479, 598)
        Me.SplitContainer3.SplitterDistance = 29
        Me.SplitContainer3.TabIndex = 2
        '
        'SplitContainer4
        '
        Me.SplitContainer4.BackColor = System.Drawing.Color.White
        Me.SplitContainer4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitContainer4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer4.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer4.Name = "SplitContainer4"
        Me.SplitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer4.Panel1
        '
        Me.SplitContainer4.Panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.SplitContainer4.Panel1.Cursor = System.Windows.Forms.Cursors.Hand
        '
        'SplitContainer4.Panel2
        '
        Me.SplitContainer4.Panel2.Controls.Add(Me.SplitContainer5)
        Me.SplitContainer4.Size = New System.Drawing.Size(29, 598)
        Me.SplitContainer4.SplitterDistance = 425
        Me.SplitContainer4.TabIndex = 0
        '
        'SplitContainer5
        '
        Me.SplitContainer5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer5.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer5.Name = "SplitContainer5"
        Me.SplitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer5.Panel1
        '
        Me.SplitContainer5.Panel1.Controls.Add(Me.LBox_Data)
        '
        'SplitContainer5.Panel2
        '
        Me.SplitContainer5.Panel2.Controls.Add(Me.TBox_Data)
        Me.SplitContainer5.Size = New System.Drawing.Size(27, 167)
        Me.SplitContainer5.SplitterDistance = 136
        Me.SplitContainer5.TabIndex = 2
        '
        'LBox_Data
        '
        Me.LBox_Data.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LBox_Data.FormattingEnabled = True
        Me.LBox_Data.ItemHeight = 12
        Me.LBox_Data.Location = New System.Drawing.Point(0, 0)
        Me.LBox_Data.Name = "LBox_Data"
        Me.LBox_Data.Size = New System.Drawing.Size(27, 136)
        Me.LBox_Data.TabIndex = 1
        '
        'TBox_Data
        '
        Me.TBox_Data.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TBox_Data.Location = New System.Drawing.Point(0, 0)
        Me.TBox_Data.Name = "TBox_Data"
        Me.TBox_Data.Size = New System.Drawing.Size(27, 19)
        Me.TBox_Data.TabIndex = 0
        Me.TBox_Data.Text = ":"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'YCM_MainFrame
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "YCM_MainFrame"
        Me.Size = New System.Drawing.Size(1012, 741)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.ImageSplCnt.Panel2.ResumeLayout(False)
        CType(Me.ImageSplCnt, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ImageSplCnt.ResumeLayout(False)
        Me.halwinSplitContnr.Panel1.ResumeLayout(False)
        Me.halwinSplitContnr.Panel2.ResumeLayout(False)
        CType(Me.halwinSplitContnr, System.ComponentModel.ISupportInitialize).EndInit()
        Me.halwinSplitContnr.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.FourImageTableLayoutPanel.ResumeLayout(False)
        Me.SplitContainer3.Panel1.ResumeLayout(False)
        CType(Me.SplitContainer3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer3.ResumeLayout(False)
        Me.SplitContainer4.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer4.ResumeLayout(False)
        Me.SplitContainer5.Panel1.ResumeLayout(False)
        Me.SplitContainer5.Panel2.ResumeLayout(False)
        Me.SplitContainer5.Panel2.PerformLayout()
        CType(Me.SplitContainer5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer5.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents ImageSplCnt As System.Windows.Forms.SplitContainer
    Friend WithEvents halwinSplitContnr As System.Windows.Forms.SplitContainer
    Friend WithEvents ImageListView As System.Windows.Forms.ListView
    Friend WithEvents FourImageTableLayoutPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents SplitContainer3 As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer4 As System.Windows.Forms.SplitContainer
    Friend WithEvents LBox_Data As System.Windows.Forms.ListBox
    Friend WithEvents TBox_Data As System.Windows.Forms.TextBox
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents ElementHost1 As System.Windows.Forms.Integration.ElementHost
    Friend RibbonMenuControl14 As YCM.RibbonMenuControl1
    Friend WithEvents AxHWindowXCtrl2 As HalconDotNet.HWindowControl
    Friend WithEvents AxHWindowXCtrl5 As HalconDotNet.HWindowControl
    Friend WithEvents AxHWindowXCtrl4 As HalconDotNet.HWindowControl
    Friend WithEvents AxHWindowXCtrl3 As HalconDotNet.HWindowControl
    Friend WithEvents AxHWindowXCtrl6 As HalconDotNet.HWindowControl
    Friend WithEvents ImgLst As System.Windows.Forms.ImageList
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents SplitContainer5 As System.Windows.Forms.SplitContainer
    'Friend WithEvents PrintForm1 As Microsoft.VisualBasic.PowerPacks.Printing.PrintForm
End Class
