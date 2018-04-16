
Public Class TabPageManager

    Private Class TabPageInfo
        Public TabPage As TabPage
        Public Visible As Boolean

        Public Sub New(ByVal page As TabPage, ByVal v As Boolean)
            TabPage = page
            Visible = v
        End Sub
    End Class

    Private _tabPageInfos As TabPageInfo() = Nothing
    Private _tabControl As System.Windows.Forms.TabControl = Nothing

    ''' <summary>
    ''' TabPageManagerクラスのインスタンスを作成する
    ''' </summary>
    ''' <param name="crl">基になるTabControlオブジェクト</param>
    Public Sub New(ByVal crl As System.Windows.Forms.TabControl)
        _tabControl = crl
        _tabPageInfos = _
            New TabPageInfo(_tabControl.TabPages.Count - 1) {}
        Dim i As Integer
        For i = 0 To _tabControl.TabPages.Count - 1
            _tabPageInfos(i) = _
                New TabPageInfo(_tabControl.TabPages(i), True)
        Next i
    End Sub

    ''' <summary>
    ''' TabPageの表示・非表示を変更する
    ''' </summary>
    ''' <param name="index">変更するTabPageのIndex番号</param>
    ''' <param name="v">表示するときはTrue。
    ''' 非表示にするときはFalse。</param>
    Public Sub ChangeTabPageVisible( _
        ByVal index As Integer, ByVal v As Boolean)
        If _tabPageInfos(index).Visible = v Then
            Return
        End If
        _tabPageInfos(index).Visible = v
        _tabControl.SuspendLayout()
        _tabControl.TabPages.Clear()
        Dim i As Integer
        For i = 0 To _tabPageInfos.Length - 1
            If _tabPageInfos(i).Visible Then
                _tabControl.TabPages.Add(_tabPageInfos(i).TabPage)
            End If
        Next i
        _tabControl.ResumeLayout()
    End Sub
End Class
