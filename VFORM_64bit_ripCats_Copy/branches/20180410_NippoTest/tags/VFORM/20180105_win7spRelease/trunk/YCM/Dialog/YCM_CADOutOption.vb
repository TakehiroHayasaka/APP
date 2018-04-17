Imports System.Windows.Forms
Public Class YCM_CADOutOption
    Private Const mc_filterDwg As String = "CAD図面ファイル(*.dwg)|*.dwg"
    Private Const mc_filterDxf As String = "CAD図面ファイル(*.dxf)|*.dxf"

    Private m_isDwgFilter As Boolean = True

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub YCM_CADOutOption_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'ComboBox_OutDim.SelectedIndex = 0
    End Sub

    Private Sub Button_SelFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_SelFile.Click
        Dim strCADFileName As String
        Dim filter As String
        If m_isDwgFilter Then
            filter = mc_filterDwg
        Else
            filter = mc_filterDxf
        End If
        Me.SaveFileDialog1.InitialDirectory = MainFrm.objFBM.ProjectPath
        strCADFileName = Filesave_CAD(Me.SaveFileDialog1, filter)
        If strCADFileName <> "" Then
            Dim fi_out As New IO.FileInfo(strCADFileName)
            If fi_out.IsReadOnly = True And fi_out.Exists = True Then
                Dim iOutFileResult As Integer = MsgBox("この図面ファイルは読込専用なので、別の図面ファイル名を指定してください。")
                Exit Sub
            End If
            Me.TextBox_CADFileName.Text = strCADFileName
        End If
    End Sub

    Public Sub isDwgFilter(ByVal val As Boolean)
        m_isDwgFilter = val
    End Sub

End Class
