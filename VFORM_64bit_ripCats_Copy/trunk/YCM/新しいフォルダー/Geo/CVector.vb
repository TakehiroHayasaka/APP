Public Class CVector
    Public x As Double
    Public y As Double
    Public Sub CVector()
        x = 0.0
        y = 0.0
    End Sub
    Public Sub CVector(ByVal _x As Double, ByVal _y As Double)
        x = _x
        y = _y
    End Sub
    Public Sub rotateBy(ByVal degreeAngle As Double)
        Dim xx As Double = x
        Dim yy As Double = y
        x = xx * Math.Cos(degreeAngle) - yy * Math.Sin(degreeAngle)
        y = xx * Math.Sin(degreeAngle) - yy * Math.Cos(degreeAngle)
    End Sub
    Public Function angle() As Double
        Dim ang As Double
        If x <> 0 Then
            ang = Math.Abs(Math.Atan(y / x))
        Else
            ang = CShape.c_pi / 2.0
        End If
        If x >= 0.0 Then
            If y > 0.0 Then
                ang = CShape.c_pi * 2.0 - ang
            End If
        Else
            If y <= 0.0 Then
                ang = CShape.c_pi - ang
            Else
                ang = CShape.c_pi + ang
            End If
        End If
        Return ang
    End Function
    Public Function angleTo(ByVal vec As CVector) As Double
        Dim newVec As New CVector
        newVec = Me
        newVec.rotateBy(-vec.angle())
        Return newVec.angle()
    End Function
    Public Sub normalize()
        Dim len As Double = length()
        If len <> 0.0 Then
            x /= len
            y /= len
        End If
    End Sub
    Public Function length() As Double
        Return Math.Sqrt((x * x) + (y * y))
    End Function
    Public Shared Operator *(ByVal vec As CVector, ByVal val As Double) As CVector
        Dim newVec As New CVector
        newVec.x = vec.x * val
        newVec.y = vec.y * val
        Return newVec
    End Operator
    Public Function dotProductTo(ByVal vec As CVector) As Double
        Dim v1 As New CVector
        v1 = Me
        'v1.normalize()
        'vec.normalize()
        Return v1.x * vec.x + (v1.y * vec.y)
    End Function
    Public Function toRadian(ByVal degreeAngle As Double) As Double

    End Function

End Class
