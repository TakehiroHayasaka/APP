Public Class FrmDetail


    Private Sub FrmDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: このコード行はデータを 'Db1DataSet1.TTT' テーブルに読み込みます。必要に応じて移動、または削除をしてください。
        Me.TTTTableAdapter.Fill(Me.Db1DataSet1.TTT)
        'TODO: このコード行はデータを 'Db1DataSet.BBB' テーブルに読み込みます。必要に応じて移動、または削除をしてください。
        Me.BBBTableAdapter.Fill(Me.Db1DataSet.BBB)

    End Sub
End Class