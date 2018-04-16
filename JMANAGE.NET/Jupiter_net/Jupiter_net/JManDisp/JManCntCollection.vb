Imports System.ComponentModel
Imports System.Collections.ObjectModel

' 表示する個々のデータ（データバインド可能）
Public Class JManCntData
    Implements INotifyPropertyChanged

    ' Indexプロパティ
    Private _index As String
    Public Property Index As String
        Get
            Return _index
        End Get
        Set(value As String)
            _index = value : OnPropertyChanged("Index")
        End Set
    End Property

    ' Valueプロパティ
    Private _value As String
    Public Property Value As String
        Get
            Return _value
        End Get
        Set(value As String)
            _value = value : OnPropertyChanged("Value")
        End Set
    End Property

    ' INotifyPropertyChangedインターフェースの実装
    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) _
      Implements INotifyPropertyChanged.PropertyChanged
    Private Sub OnPropertyChanged(pName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(pName))
    End Sub
End Class

Public Class JManCntCollection
    Inherits ObservableCollection(Of JManCntData)

End Class
