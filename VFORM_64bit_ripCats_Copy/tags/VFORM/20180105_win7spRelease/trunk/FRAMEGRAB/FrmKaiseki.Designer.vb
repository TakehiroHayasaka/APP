<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmKaiseki
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmKaiseki))
        Me.btnZoomIn = New System.Windows.Forms.Button()
        Me.btnZoomOut = New System.Windows.Forms.Button()
        Me.btnLeft = New System.Windows.Forms.Button()
        Me.btnRigth = New System.Windows.Forms.Button()
        Me.btnUp = New System.Windows.Forms.Button()
        Me.btnDown = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.AxHWinPrev = New AxHALCONXLib.AxHWindowXCtrl()
        CType(Me.AxHWinPrev, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnZoomIn
        '
        Me.btnZoomIn.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnZoomIn.Location = New System.Drawing.Point(16, 368)
        Me.btnZoomIn.Name = "btnZoomIn"
        Me.btnZoomIn.Size = New System.Drawing.Size(72, 44)
        Me.btnZoomIn.TabIndex = 1
        Me.btnZoomIn.Text = "+"
        Me.btnZoomIn.UseVisualStyleBackColor = True
        '
        'btnZoomOut
        '
        Me.btnZoomOut.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnZoomOut.Location = New System.Drawing.Point(104, 368)
        Me.btnZoomOut.Name = "btnZoomOut"
        Me.btnZoomOut.Size = New System.Drawing.Size(72, 44)
        Me.btnZoomOut.TabIndex = 2
        Me.btnZoomOut.Text = "-"
        Me.btnZoomOut.UseVisualStyleBackColor = True
        '
        'btnLeft
        '
        Me.btnLeft.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLeft.Location = New System.Drawing.Point(192, 368)
        Me.btnLeft.Name = "btnLeft"
        Me.btnLeft.Size = New System.Drawing.Size(72, 44)
        Me.btnLeft.TabIndex = 3
        Me.btnLeft.Text = "←"
        Me.btnLeft.UseVisualStyleBackColor = True
        '
        'btnRigth
        '
        Me.btnRigth.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRigth.Location = New System.Drawing.Point(280, 368)
        Me.btnRigth.Name = "btnRigth"
        Me.btnRigth.Size = New System.Drawing.Size(72, 44)
        Me.btnRigth.TabIndex = 4
        Me.btnRigth.Text = "→"
        Me.btnRigth.UseVisualStyleBackColor = True
        '
        'btnUp
        '
        Me.btnUp.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUp.Location = New System.Drawing.Point(368, 368)
        Me.btnUp.Name = "btnUp"
        Me.btnUp.Size = New System.Drawing.Size(72, 44)
        Me.btnUp.TabIndex = 5
        Me.btnUp.Text = "↑"
        Me.btnUp.UseVisualStyleBackColor = True
        '
        'btnDown
        '
        Me.btnDown.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDown.Location = New System.Drawing.Point(456, 368)
        Me.btnDown.Name = "btnDown"
        Me.btnDown.Size = New System.Drawing.Size(72, 44)
        Me.btnDown.TabIndex = 6
        Me.btnDown.Text = "↓"
        Me.btnDown.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(456, 419)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(72, 44)
        Me.btnClose.TabIndex = 7
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'AxHWinPrev
        '
        Me.AxHWinPrev.Enabled = True
        Me.AxHWinPrev.Location = New System.Drawing.Point(8, 8)
        Me.AxHWinPrev.Name = "AxHWinPrev"
        Me.AxHWinPrev.OcxState = CType(resources.GetObject("AxHWinPrev.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxHWinPrev.Size = New System.Drawing.Size(536, 352)
        Me.AxHWinPrev.TabIndex = 0
        '
        'FrmKaiseki
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(564, 472)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnDown)
        Me.Controls.Add(Me.btnUp)
        Me.Controls.Add(Me.btnRigth)
        Me.Controls.Add(Me.btnLeft)
        Me.Controls.Add(Me.btnZoomOut)
        Me.Controls.Add(Me.btnZoomIn)
        Me.Controls.Add(Me.AxHWinPrev)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(570, 500)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(570, 500)
        Me.Name = "FrmKaiseki"
        Me.Text = " 解析画像  "
        CType(Me.AxHWinPrev, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents AxHWinPrev As AxHALCONXLib.AxHWindowXCtrl
    Friend WithEvents btnZoomIn As System.Windows.Forms.Button
    Friend WithEvents btnZoomOut As System.Windows.Forms.Button
    Friend WithEvents btnLeft As System.Windows.Forms.Button
    Friend WithEvents btnRigth As System.Windows.Forms.Button
    Friend WithEvents btnUp As System.Windows.Forms.Button
    Friend WithEvents btnDown As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
End Class
