Module CommonMod
    Public Op As New HALCONXLib.HOperatorSetX
    Public Tuple As New HALCONXLib.HTupleX
    ' Chapter: Tuple / Creation
    ' Short Description: This procedure generates a tuple with a sequence of equidistant values.
    Public Sub tuple_gen_sequence(ByVal hv_Start As Object, ByVal hv_End As Object, _
        ByVal hv_Step As Object, ByRef hv_Sequence As Object)
        ' Initialize local and output iconic variables 

        '
        'This procedure generates a tuple with a sequence of equidistant values.
        '[Start, Start + Step, Start + 2*Step, ... End]
        '
        'Input parameters:
        'Start: Start value of the tuple
        'End:   Maximum value for the last entry.
        '       Note that the last entry of the resulting tuple may be less than End
        'Step:  Increment value
        'Assure that Step#0 and sgn(Start-End)#sgn(Step), else an error occurs
        '
        'Output parameter:
        'Sequence: The resulting tuple [Start, Start + Step, Start + 2*Step, ... End]
        '
        hv_Sequence = Tuple.TupleAdd(Tuple.TupleSub(hv_Start, hv_Step), Tuple.TupleCumul( _
            Tuple.TupleGenConst(Tuple.TupleAdd(Tuple.TupleInt(Tuple.TupleDiv(Tuple.TupleSub( _
            hv_End, hv_Start), hv_Step)), 1), hv_Step)))

        Exit Sub
    End Sub
End Module
