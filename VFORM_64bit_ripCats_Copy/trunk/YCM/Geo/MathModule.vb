Option Strict Off
Option Explicit On
Module MathModule

    '==============================================================================
    '==============================================================================
    Public Function Math_Pai() As Double
        Math_Pai = 3.14159265358979
    End Function

    '==============================================================================
    Public Function Math_ToRadian(ByRef dblAngle As Double) As Double
        Math_ToRadian = dblAngle * Math_Pai / 180.0#
    End Function

    '==============================================================================
    Public Function Math_ToDegree(ByRef dblAngle As Double) As Double
        Math_ToDegree = dblAngle * 180.0# / Math_Pai
    End Function

    '==============================================================================
    '==============================================================================
    Public Function Math_Compare(ByRef dblValue1 As Double, ByRef dblValue2 As Double, ByRef dblEpsilon As Double) As Short
        If System.Math.Abs(dblValue1 - dblValue2) < dblEpsilon Then
            Math_Compare = 0
        ElseIf dblValue1 < dblValue2 Then
            Math_Compare = -1
        Else
            Math_Compare = 1
        End If
    End Function

    '==============================================================================
    Public Function Math_IsEqual(ByRef dblValue1 As Double, ByRef dblValue2 As Double, ByRef dblEpsilon As Double) As Boolean
        If Math_Compare(dblValue1, dblValue2, dblEpsilon) = 0 Then Math_IsEqual = True
    End Function

    '==============================================================================
    Public Function Math_IsNotEqual(ByRef dblValue1 As Double, ByRef dblValue2 As Double, ByRef dblEpsilon As Double) As Boolean
        If Math_Compare(dblValue1, dblValue2, dblEpsilon) <> 0 Then Math_IsNotEqual = True
    End Function

    '==============================================================================
    '==============================================================================
    Public Function Math_Acos(ByRef dblValue As Double) As Double
        Dim dblEpsilon As Double
        dblEpsilon = 0.000000001
        Math_Acos = 0
        If System.Math.Abs(dblValue) - dblEpsilon > 1.0# Then ' 定義不可
            Exit Function
        End If
        If 1.0# < dblValue And Math_IsEqual(dblValue, 1.0#, dblEpsilon) Then ' 1.0 以上で,誤差範囲内の値を救済する
            dblValue = 1.0#
        ElseIf dblValue < -1.0# And Math_IsEqual(dblValue, -1.0#, dblEpsilon) Then  ' -1.0 以下で,誤差範囲内の値を救済する
            dblValue = -1.0#
        End If
        'Math_Acos = Acos(dblValue)
        If dblValue = 1.0# Then
            Math_Acos = 0.0#
        ElseIf dblValue = -1.0# Then
            Math_Acos = Math_Pai
        Else
            Math_Acos = System.Math.Atan(-dblValue / System.Math.Sqrt(-dblValue * dblValue + 1)) + 2 * System.Math.Atan(1)
        End If
    End Function
End Module
