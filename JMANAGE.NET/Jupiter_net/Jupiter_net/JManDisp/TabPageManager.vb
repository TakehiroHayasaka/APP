Imports System
Imports System.IO
Imports System.Windows.Controls

Public Class TabPageManager

    Private Class TabItemInfo
        Public _tabItem As TabItem
        Public _visible As Boolean

        Public Sub New(ByVal page As TabItem, ByVal v As Boolean)
            _tabItem = page
            _visible = v
        End Sub
    End Class

    Private _tabItemInfos As TabItemInfo() = Nothing
    Private _tabControl As TabControl = Nothing

    ''' <summary>
    ''' TabPageManagerクラスのインスタンスを作成する
    ''' </summary>
    ''' <param name="crl">基になるTabControlオブジェクト</param>
    Public Sub New(ByVal crl As TabControl)
        _tabControl = crl
        _tabItemInfos = New TabItemInfo(_tabControl.Items.Count - 1) {}
        Dim i As Integer
        For i = 0 To _tabControl.Items.Count - 1
            _tabItemInfos(i) = New TabItemInfo(_tabControl.Items(i), True)
        Next i

    End Sub

    ''' <summary>
    ''' TabPageの表示・非表示を変更する
    ''' </summary>
    ''' <param name="index">変更するTabPageのIndex番号</param>
    ''' <param name="v">表示するときはTrue。
    ''' 非表示にするときはFalse。</param>
    Public Sub ChangeTabPageVisible(ByVal index As Integer, ByVal v As Boolean)
        If _tabItemInfos(index)._visible = v Then
            Return
        End If
        _tabItemInfos(index)._visible = v
        '_tabControl.SuspendLayout()
        _tabControl.Items.Clear()
        Dim i As Integer
        For i = 0 To _tabItemInfos.Length - 1
            If _tabItemInfos(i)._visible Then
                _tabControl.Items.Add(_tabItemInfos(i)._tabItem)
            End If
        Next i
        '_tabControl.ResumeLayout()
    End Sub

    Public Sub initTabManager()
        _tabControl.Items.Clear()

        For i = 0 To _tabItemInfos.Length - 1
            _tabControl.Items.Add(_tabItemInfos(i)._tabItem)
            _tabItemInfos(i)._visible = True
        Next i
    End Sub

End Class
