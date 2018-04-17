Public Class CVector3d
    Public x As Double
    Public y As Double
    Public z As Double
    Public Sub CVector()
        x = 0.0
        y = 0.0
        z = 0.0
    End Sub
    Public Sub CVector(ByVal _x As Double, ByVal _y As Double, ByVal _z As Double)
        x = _x
        y = _y
        z = _z
    End Sub
    Public Sub normalize()
        Dim len As Double = length()
        If len <> 0.0 Then
            x /= len
            y /= len
            z /= len
        End If
    End Sub
    Public Function length() As Double
        Return Math.Sqrt((x * x) + (y * y) + (z * z))
    End Function
    Public Shared Operator *(ByVal vec As CVector3d, ByVal val As Double) As CVector3d
        Dim newVec As New CVector3d
        newVec.x = vec.x * val
        newVec.y = vec.y * val
        newVec.z = vec.z * val
        Return newVec
    End Operator
    Public Function dotProductTo(ByVal vec As CVector3d) As Double
        Dim v1 As New CVector3d
        v1 = Me
        'v1.normalize()
        'vec.normalize()
        Return (v1.x * vec.x) + (v1.y * vec.y) + (v1.z * vec.z)
    End Function
End Class
