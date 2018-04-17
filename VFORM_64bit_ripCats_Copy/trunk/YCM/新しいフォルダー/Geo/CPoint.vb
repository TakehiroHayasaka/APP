Public Class CPoint
    Public x As Double
    Public y As Double
    Public Sub New()
        x = 0.0
        y = 0.0
    End Sub
    Public Sub New(ByVal _x As Double, ByVal _y As Double)
        x = _x
        y = _y
    End Sub
    Public Function getIntx() As Integer
        Return x
    End Function
    Public Function getInty() As Integer
        Return y
    End Function
    Public Function distTo(ByVal pnt As CPoint) As Double
        Dim r As Double = ((x - pnt.x) * (x - pnt.x)) + ((y - pnt.y) * (y - pnt.y))
        Return Math.Sqrt(r)
    End Function

    Public Shared Operator +(ByVal pnt As CPoint, ByVal vec As CVector) As CPoint
        Dim newPnt As New CPoint
        newPnt.x = pnt.x + vec.x
        newPnt.y = pnt.y + vec.y
        Return newPnt
    End Operator
    Public Shared Operator -(ByVal pnt1 As CPoint, ByVal pnt2 As CPoint) As CVector
        Dim vec As New CVector
        vec.x = pnt1.x - pnt2.x
        vec.y = pnt1.y - pnt2.y
        Return vec
    End Operator
End Class
