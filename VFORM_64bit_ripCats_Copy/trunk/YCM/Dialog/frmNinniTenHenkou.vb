Public Class frmNinniTenHenkou
    Public objSunpoItem As SunpoSetTable
    Dim NP1 As CLookPoint
    Dim NP2 As CLookPoint
    Dim NP3 As CLookPoint
    Private Sub frmNinniTenHenkou_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NP1 = Nothing
        NP2 = Nothing
        NP3 = Nothing

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If point_name.Count > 0 Then
            NP1 = New CLookPoint
            Me.Hide()
            If IOUtil.GetPoint(NP1, "点目を指示：") <> -1 Then
                TextBox1.Text = NP1.LabelName
            End If

            Me.Show()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If point_name.Count > 0 Then
            NP2 = New CLookPoint
            Me.Hide()
            If IOUtil.GetPoint(NP2, "点目を指示：") <> -1 Then
                TextBox2.Text = NP2.LabelName
            End If

            Me.Show()
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If point_name.Count > 0 Then
            NP3 = New CLookPoint
            Me.Hide()
            If IOUtil.GetPoint(NP3, "点目を指示：") <> -1 Then
                TextBox3.Text = NP3.LabelName
            End If

            Me.Show()
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If Not NP1 Is Nothing Then
            If NP1.LabelName <> "" Then
                '新規で点群定義する。
                Dim objTengun1 As New TenGunTeigiTable
                GenNewTengun(objTengun1, NP1)
                lstTengun.Add(objTengun1)
                'その点群のＩＤをsunposetのgunID に設定する。
                objSunpoItem.GunID1 = objTengun1.TenGunID
            End If
        End If
        If Not NP2 Is Nothing Then
            If NP2.LabelName <> "" Then
                '新規で点群定義する。
                Dim objTengun2 As New TenGunTeigiTable
                GenNewTengun(objTengun2, NP2)
                lstTengun.Add(objTengun2)
                'その点群のＩＤをsunposetのgunID に設定する。
                objSunpoItem.GunID2 = objTengun2.TenGunID
            End If
        End If
        If Not NP3 Is Nothing Then
            If NP3.LabelName <> "" Then
                '新規で点群定義する。
                Dim objTengun3 As New TenGunTeigiTable
                GenNewTengun(objTengun3, NP3)
                lstTengun.Add(objTengun3)
                'その点群のＩＤをsunposetのgunID に設定する。
                objSunpoItem.GunID3 = objTengun3.TenGunID
            End If
        End If

        objSunpoItem.CalcSunpoVal()

        Me.Close()

    End Sub

    Private Sub GenNewTengun(ByRef objTengun1 As TenGunTeigiTable, ByVal np1 As CLookPoint)
        objTengun1 = New TenGunTeigiTable
        Dim maxid As Integer = -1
        For Each TT As TenGunTeigiTable In lstTengun
            If maxid < TT.TenGunID Then
                maxid = TT.TenGunID
            End If
        Next
        objTengun1.GenNinniTengun(CommonTypeID, maxid + 1, np1.tid)
    End Sub
End Class