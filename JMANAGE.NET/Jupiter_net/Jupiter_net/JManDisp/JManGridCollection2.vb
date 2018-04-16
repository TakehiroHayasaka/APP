Imports System.ComponentModel
Imports System.Collections.ObjectModel

' DataGridに表示するためのクラス(2列用)
Public Class JManGridData2
    Implements INotifyPropertyChanged

    ' Value1プロパティ
    Private _value1 As String
    Public Property Value1 As String
        Get
            Return _value1
        End Get
        Set(value As String)
            _value1 = value : OnPropertyChanged("Value1")
        End Set
    End Property

    ' Valueプロパティ
    Private _value2 As String
    Public Property Value2 As String
        Get
            Return _value2
        End Get
        Set(value As String)
            _value2 = value : OnPropertyChanged("Value2")
        End Set
    End Property

    ' INotifyPropertyChangedインターフェースの実装
    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) _
      Implements INotifyPropertyChanged.PropertyChanged
    Private Sub OnPropertyChanged(pName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(pName))
    End Sub
End Class

Public Class JManGridCollection2
    Inherits ObservableCollection(Of JManGridData2)

End Class
