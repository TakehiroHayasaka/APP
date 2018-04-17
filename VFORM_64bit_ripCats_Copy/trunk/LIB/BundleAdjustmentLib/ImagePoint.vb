Public Class ImagePoint
    Private _u As Double
    Private _v As Double
    Public Property U() As Double
        Get
            Return _u
        End Get
        Set(ByVal value As Double)
            _u = value
        End Set
    End Property
    Public Property V() As Double
        Get
            Return _v
        End Get
        Set(ByVal value As Double)
            _v = value
        End Set
    End Property


End Class
