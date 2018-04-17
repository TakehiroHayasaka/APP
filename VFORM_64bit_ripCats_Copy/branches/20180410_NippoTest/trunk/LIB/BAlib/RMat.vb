Public Class RMat
    Public R11 As Double
    Public R12 As Double
    Public R13 As Double
    Public R21 As Double
    Public R22 As Double
    Public R23 As Double
    Public R31 As Double
    Public R32 As Double
    Public R33 As Double

    Public Sub New()

    End Sub

    Public Sub Set_R_ByHomMat(ByVal HomMat As Object)
        R11 = HomMat.TupleSelect(0).D
        R12 = HomMat.TupleSelect(1).D
        R13 = HomMat.TupleSelect(2).D
        R21 = HomMat.TupleSelect(4).D
        R22 = HomMat.TupleSelect(5).D
        R23 = HomMat.TupleSelect(6).D
        R31 = HomMat.TupleSelect(8).D
        R32 = HomMat.TupleSelect(9).D
        R33 = HomMat.TupleSelect(10).D
    End Sub

    Public Function Multi_By_Rmat(ByVal objR As RMat) As RMat
        Multi_By_Rmat = New RMat
        With Multi_By_Rmat
            .R11 = Me.R11 * objR.R11 + Me.R12 * objR.R21 + Me.R13 * objR.R31
            .R12 = Me.R11 * objR.R12 + Me.R12 * objR.R22 + Me.R13 * objR.R32
            .R13 = Me.R11 * objR.R13 + Me.R12 * objR.R23 + Me.R13 * objR.R33
            .R21 = Me.R21 * objR.R11 + Me.R22 * objR.R21 + Me.R23 * objR.R31
            .R22 = Me.R21 * objR.R12 + Me.R22 * objR.R22 + Me.R23 * objR.R32
            .R23 = Me.R21 * objR.R13 + Me.R22 * objR.R23 + Me.R23 * objR.R33
            .R31 = Me.R31 * objR.R11 + Me.R32 * objR.R21 + Me.R33 * objR.R31
            .R32 = Me.R31 * objR.R12 + Me.R32 * objR.R22 + Me.R33 * objR.R32
            .R33 = Me.R31 * objR.R13 + Me.R32 * objR.R23 + Me.R33 * objR.R33
        End With
    End Function

    Public Sub CopyToMe(ByVal objR As RMat)
        R11 = objR.R11
        R12 = objR.R12
        R13 = objR.R13
        R21 = objR.R21
        R22 = objR.R22
        R23 = objR.R23
        R31 = objR.R31
        R32 = objR.R32
        R33 = objR.R33
    End Sub
End Class
