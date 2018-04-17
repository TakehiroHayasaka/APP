Public NotInheritable Class frmProgressBar

    'TODO: このフォームは、プロジェクト デザイナ ([プロジェクト] メニューの下の [プロパティ]) の [アプリケーション] タブを使用して、

    '  アプリケーションのスプラッシュ スクリーンとして簡単に設定することができます
    Public isCancelClicked As Boolean = False

    Public Delegate Sub SetProgressUp()

    Public Delegate Sub SetFinishDialog()

    Public Sub FinishDialog()
        Dim dh As SetFinishDialog = New SetFinishDialog(AddressOf FinishDialogOwn)
        Me.Invoke(dh)
    End Sub

    Public Sub ProgressUp()
        Dim dh As SetProgressUp = New SetProgressUp(AddressOf ProgressUpOwn)
        Me.Invoke(dh)
    End Sub

    Public Sub ProgressUpOwn()
        Me.ProgressBar1.PerformStep()
    End Sub

    Public Sub FinishDialogOwn()
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.isCancelClicked = True
    End Sub
End Class
