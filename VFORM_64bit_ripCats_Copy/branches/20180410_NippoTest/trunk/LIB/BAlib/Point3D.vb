﻿Public Class Point3D
    Public X As Double
    Public Y As Double
    Public Z As Double
    Public Sub New()
        X = 0
        Y = 0
        Z = 0
    End Sub
    Public Sub New(ByVal XX As Double, ByVal YY As Double, ByVal ZZ As Double)
        X = XX
        Y = YY
        Z = ZZ
    End Sub

    Public Sub Scale(ByVal S As Double)
        X = X * S
        Y = Y * S
        Z = Z * S
    End Sub
    Public Sub AddMe(ByVal objAdd As Point3D)
        X = X + objAdd.X
        Y = Y + objAdd.Y
        Z = Z + objAdd.Z
    End Sub
    Public Function AddTo(ByVal objAdd As Point3D) As Point3D
        AddTo = New Point3D
        AddTo.X = X + objAdd.X
        AddTo.Y = Y + objAdd.Y
        AddTo.Z = Z + objAdd.Z
    End Function
    Public Function VectorMult(ByVal objMult As Point3D) As Point3D
        VectorMult = New Point3D
        'y * vec.z - z * vec.y, z * vec.x - x * vec.z, x * vec.y - y * vec.x
        VectorMult.X = Y * objMult.Z - Z * objMult.Y
        VectorMult.Y = Z * objMult.X - X * objMult.Z
        VectorMult.Z = X * objMult.Y - Y * objMult.X
    End Function

    Public Sub CopyToMe(ByVal objCopy As Point3D)
        X = objCopy.X
        Y = objCopy.Y
        Z = objCopy.Z
    End Sub

    Public Function Nagasa() As Double
        Nagasa = Math.Sqrt(X * X + Y * Y + Z * Z)
    End Function

    Public Function Axis() As Object
        Axis = Nothing
        Axis = BTuple.TupleConcat(BTuple.TupleConcat(X, Y), Z)
    End Function
End Class
