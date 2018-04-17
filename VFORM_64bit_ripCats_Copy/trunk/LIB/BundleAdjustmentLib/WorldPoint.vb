Public Class WorldPoint
    Private _X As Double
    Private _Y As Double
    Private _Z As Double
    
    Public Property X() As Double
        Get
            Return _X
        End Get
        Set(ByVal value As Double)
            _X = value
        End Set
    End Property
    Public Property Y() As Double
        Get
            Return _Y
        End Get
        Set(ByVal value As Double)
            _Y = value
        End Set
    End Property
    Public Property Z() As Double
        Get
            Return _Z
        End Get
        Set(ByVal value As Double)
            _Z = value
        End Set
    End Property
    Public Sub SetDeltaXYZ(ByVal Delta As Object)
        _X = _X + CDbl(Tuple.TupleSelect(Delta, 0))
        _Y = _Y + CDbl(Tuple.TupleSelect(Delta, 1))
        _Z = _Z + CDbl(Tuple.TupleSelect(Delta, 2))

    End Sub
End Class
