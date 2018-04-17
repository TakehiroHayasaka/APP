Public Class CPoint3d
    Public x As Double
    Public y As Double
    Public z As Double
    Public Sub New()
        x = 0.0
        y = 0.0
        z = 0.0
    End Sub
    Public Sub New(ByVal _x As Double, ByVal _y As Double, ByVal _z As Double)
        x = _x
        y = _y
        z = _z
    End Sub
    Public Function getIntx() As Integer
        Return x
    End Function
    Public Function getInty() As Integer
        Return y
    End Function
    Public Function getIntz() As Integer
        Return z
    End Function
    Public Function distTo(ByVal pnt As CPoint3d) As Double
        Dim r As Double = ((x - pnt.x) * (x - pnt.x)) + ((y - pnt.y) * (y - pnt.y) + (z - pnt.z) * (z - pnt.z))
        Return Math.Sqrt(r)
    End Function

    Public Shared Operator +(ByVal pnt As CPoint3d, ByVal vec As CVector3d) As CPoint3d
        Dim newPnt As New CPoint3d
        newPnt.x = pnt.x + vec.x
        newPnt.y = pnt.y + vec.y
        newPnt.z = pnt.z + vec.z
        Return newPnt
    End Operator
    Public Shared Operator -(ByVal pnt1 As CPoint3d, ByVal pnt2 As CPoint3d) As CVector3d
        Dim vec As New CVector3d
        vec.x = pnt1.x - pnt2.x
        vec.y = pnt1.y - pnt2.y
        vec.z = pnt1.z - pnt2.z
        Return vec
    End Operator
End Class
