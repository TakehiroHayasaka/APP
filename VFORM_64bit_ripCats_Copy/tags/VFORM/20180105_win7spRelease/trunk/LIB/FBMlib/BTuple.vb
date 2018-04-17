Imports HalconDotNet

Public Class BTuple

    Public Shared Function GetDouble(ByVal obj As Object) As Double
        If obj Is Nothing Then
            GetDouble = Nothing
            Exit Function
        End If
        If obj.GetType Is GetType(Double) Then
            GetDouble = obj
        Else
            GetDouble = obj.D
        End If
    End Function

    Public Shared Function CreateTuple(ByVal obj As Object)
        If obj Is Nothing Then
            CreateTuple = New HTuple
            Exit Function
        End If
        CreateTuple = New HTuple(obj)
    End Function


    Public Shared Function ObjArrayToTuple(ByVal obj As Object)
        Dim tuple As New HTuple
        For Each o As Object In obj
            HOperatorSet.TupleConcat(tuple, New HTuple(o), tuple)
        Next
        ObjArrayToTuple = tuple
    End Function

    Public Shared Function TupleAsin(tup As Object)
        Dim ref = New HTuple
        If tup Is Nothing Then
            tup = New HTuple
        End If
        If Not tup.GetType() = GetType(HTuple) Then
            tup = New HTuple(tup)
        End If
        HOperatorSet.TupleAsin(tup, ref)
        TupleAsin = ref
    End Function

    Public Shared Function TupleMax(tup As Object)
        Dim ref = New HTuple
        If tup Is Nothing Then
            tup = New HTuple
        End If
        If Not tup.GetType() = GetType(HTuple) Then
            tup = New HTuple(tup)
        End If
        HOperatorSet.TupleMax(tup, ref)
        TupleMax = ref
    End Function

    Public Shared Function TupleRad(tup As Object)
        Dim ref = New HTuple
        If tup Is Nothing Then
            tup = New HTuple
        End If
        If Not tup.GetType() = GetType(HTuple) Then
            tup = New HTuple(tup)
        End If
        HOperatorSet.TupleRad(tup, ref)
        TupleRad = ref
    End Function

    Public Shared Function TupleNotEqual(tup1 As Object, tup2 As Object)
        Dim ref = New HTuple
        If tup1 Is Nothing Then
            tup1 = New HTuple
        End If
        If Not tup1.GetType() = GetType(HTuple) Then
            tup1 = New HTuple(tup1)
        End If
        If tup2 Is Nothing Then
            tup2 = New HTuple
        End If
        If Not tup2.GetType() = GetType(HTuple) Then
            tup2 = New HTuple(tup2)
        End If
        HOperatorSet.TupleNotEqual(tup1, tup2, ref)
        TupleNotEqual = ref
    End Function

    Public Shared Function TupleMin(tup As Object)
        Dim ref = New HTuple
        If tup Is Nothing Then
            tup = New HTuple
        End If
        If Not tup.GetType() = GetType(HTuple) Then
            tup = New HTuple(tup)
        End If
        HOperatorSet.TupleMin(tup, ref)
        TupleMin = ref
    End Function

    Public Shared Function TupleUniq(tup As Object)
        Dim ref = New HTuple
        If tup Is Nothing Then
            tup = New HTuple
        End If
        If Not tup.GetType() = GetType(HTuple) Then
            tup = New HTuple(tup)
        End If
        HOperatorSet.TupleUniq(tup, ref)
        TupleUniq = ref
    End Function

    Public Shared Function TupleSort(tup As Object)
        Dim ref = New HTuple
        If tup Is Nothing Then
            tup = New HTuple
        End If
        If Not tup.GetType() = GetType(HTuple) Then
            tup = New HTuple(tup)
        End If
        HOperatorSet.TupleSort(tup, ref)
        TupleSort = ref
    End Function

    Public Shared Function ReadTuple(name As Object)
        Dim ref = New HTuple
        If name Is Nothing Then
            name = New HTuple
        End If
        If Not name.GetType() = GetType(HTuple) Then
            name = New HTuple(name)
        End If
        HOperatorSet.ReadTuple(name, ref)
        ReadTuple = ref
    End Function


    Public Shared Function TupleLastN(tup As Object, index As Object)
        Dim ref = New HTuple
        If tup Is Nothing Then
            tup = New HTuple
        End If
        If Not tup.GetType() = GetType(HTuple) Then
            tup = New HTuple(tup)
        End If
        If index Is Nothing Then
            index = New HTuple
        End If
        If Not index.GetType() = GetType(HTuple) Then
            index = New HTuple(index)
        End If
        HOperatorSet.TupleLastN(tup, index, ref)
        TupleLastN = ref
    End Function

    Public Shared Function TupleLength(tup As Object)
        Dim ref = New HTuple
        If tup Is Nothing Then
            tup = New HTuple
        End If
        If Not tup.GetType() = GetType(HTuple) Then
            tup = New HTuple(tup)
        End If
        HOperatorSet.TupleLength(tup, ref)
        TupleLength = ref
    End Function

    Public Shared Function TupleSqrt(tup As Object)
        Dim ref = New HTuple
        If tup Is Nothing Then
            tup = New HTuple
        End If
        If Not tup.GetType() = GetType(HTuple) Then
            tup = New HTuple(tup)
        End If
        HOperatorSet.TupleSqrt(tup, ref)
        TupleSqrt = ref
    End Function

    Public Shared Function TupleAdd(tup1 As Object, tup2 As Object)
        Dim ref = New HTuple
        If tup1 Is Nothing Then
            tup1 = New HTuple
        End If
        If Not tup1.GetType() = GetType(HTuple) Then
            tup1 = New HTuple(tup1)
        End If
        If tup2 Is Nothing Then
            tup2 = New HTuple
        End If
        If Not tup2.GetType() = GetType(HTuple) Then
            tup2 = New HTuple(tup2)
        End If
        HOperatorSet.TupleAdd(tup1, tup2, ref)
        TupleAdd = ref
    End Function

    Public Shared Function TuplePow(tup1 As Object, tup2 As Object)
        Dim ref = New HTuple
        If tup1 Is Nothing Then
            tup1 = New HTuple
        End If
        If Not tup1.GetType() = GetType(HTuple) Then
            tup1 = New HTuple(tup1)
        End If
        If tup2 Is Nothing Then
            tup2 = New HTuple
        End If
        If Not tup2.GetType() = GetType(HTuple) Then
            tup2 = New HTuple(tup2)
        End If
        HOperatorSet.TuplePow(tup1, tup2, ref)
        TuplePow = ref
    End Function

    Public Shared Function TupleSum(tup As Object)
        Dim ref = New HTuple
        If tup Is Nothing Then
            tup = New HTuple
        End If
        If Not tup.GetType() = GetType(HTuple) Then
            tup = New HTuple(tup)
        End If

        HOperatorSet.TupleSum(tup, ref)
        TupleSum = ref
    End Function

    Public Shared Function TupleSub(tup1 As Object, tup2 As Object)
        Dim ref = New HTuple
        If tup1 Is Nothing Then
            tup1 = New HTuple
        End If
        If Not tup1.GetType() = GetType(HTuple) Then
            tup1 = New HTuple(tup1)
        End If
        If tup2 Is Nothing Then
            tup2 = New HTuple
        End If
        If Not tup2.GetType() = GetType(HTuple) Then
            tup2 = New HTuple(tup2)
        End If
        HOperatorSet.TupleSub(tup1, tup2, ref)
        TupleSub = ref
    End Function

    Public Shared Function TupleMult(tup1 As Object, tup2 As Object)
        Dim ref = New HTuple
        If tup1 Is Nothing Then
            tup1 = New HTuple
        End If
        If Not tup1.GetType() = GetType(HTuple) Then
            tup1 = New HTuple(tup1)
        End If
        If tup2 Is Nothing Then
            tup2 = New HTuple
        End If
        If Not tup2.GetType() = GetType(HTuple) Then
            tup2 = New HTuple(tup2)
        End If
        HOperatorSet.TupleMult(tup1, tup2, ref)
        TupleMult = ref
    End Function

    Public Shared Function TupleDiv(tup1 As Object, tup2 As Object)
        Dim ref = New HTuple
        If tup1 Is Nothing Then
            tup1 = New HTuple
        End If
        If Not tup1.GetType() = GetType(HTuple) Then
            tup1 = New HTuple(tup1)
        End If
        If tup2 Is Nothing Then
            tup2 = New HTuple
        End If
        If Not tup2.GetType() = GetType(HTuple) Then
            tup2 = New HTuple(tup2)
        End If
        HOperatorSet.TupleDiv(tup1, tup2, ref)
        TupleDiv = ref
    End Function

    Public Shared Function TupleAbs(tup As Object)
        Dim ref = New HTuple
        If tup Is Nothing Then
            tup = New HTuple
        End If
        If Not tup.GetType() = GetType(HTuple) Then
            tup = New HTuple(tup)
        End If
        HOperatorSet.TupleAbs(tup, ref)
        TupleAbs = ref
    End Function

    Public Shared Function TupleSgn(tup As Object)
        Dim ref = New HTuple
        If tup Is Nothing Then
            tup = New HTuple
        End If
        If Not tup.GetType() = GetType(HTuple) Then
            tup = New HTuple(tup)
        End If
        HOperatorSet.TupleSgn(tup, ref)
        TupleSgn = ref
    End Function

    Public Shared Function TupleSelect(tup As Object, ByVal index As Object)
        Dim ref = New HTuple
        If tup Is Nothing Then
            tup = New HTuple
        End If
        If Not tup.GetType() = GetType(HTuple) Then
            tup = New HTuple(tup)
        End If
        If index Is Nothing Then
            index = New HTuple
        End If
        If Not index.GetType() = GetType(HTuple) Then
            index = New HTuple(index)
        End If
        HOperatorSet.TupleSelect(tup, index, ref)
        TupleSelect = ref
    End Function

    Public Shared Function TupleRemove(tup As Object, index As Object)
        Dim ref = New HTuple
        If tup Is Nothing Then
            tup = New HTuple
        End If
        If Not tup.GetType() = GetType(HTuple) Then
            tup = New HTuple(tup)
        End If
        If index Is Nothing Then
            index = New HTuple
        End If
        If Not index.GetType() = GetType(HTuple) Then
            index = New HTuple(index)
        End If
        HOperatorSet.TupleRemove(tup, index, ref)
        TupleRemove = ref
    End Function

    Public Shared Function TupleGenConst(length As Object, constVal As Object)
        Dim ref = New HTuple
        If length Is Nothing Then
            length = New HTuple
        End If
        If Not length.GetType() = GetType(HTuple) Then
            length = New HTuple(length)
        End If
        If constVal Is Nothing Then
            constVal = New HTuple
        End If
        If Not constVal.GetType() = GetType(HTuple) Then
            constVal = New HTuple(constVal)
        End If
        HOperatorSet.TupleGenConst(length, constVal, ref)
        TupleGenConst = ref
    End Function

    Public Shared Function TupleMean(tup As Object)
        Dim ref = New HTuple
        If tup Is Nothing Then
            tup = New HTuple
        End If
        If Not tup.GetType() = GetType(HTuple) Then
            tup = New HTuple(tup)
        End If
        HOperatorSet.TupleMean(tup, ref)
        TupleMean = ref
    End Function

    Public Shared Function WriteTuple(tup As Object, fileName As Object)
        Dim ref = New HTuple
        If tup Is Nothing Then
            tup = New HTuple
        End If
        If Not tup.GetType() = GetType(HTuple) Then
            tup = New HTuple(tup)
        End If
        If fileName Is Nothing Then
            fileName = New HTuple
        End If
        If Not fileName.GetType() = GetType(HTuple) Then
            fileName = New HTuple(fileName)
        End If
        HOperatorSet.WriteTuple(tup, fileName)
        WriteTuple = ref
    End Function

    Public Shared Function TupleDeviation(tup As Object)
        Dim ref = New HTuple
        If tup Is Nothing Then
            tup = New HTuple
        End If
        If Not tup.GetType() = GetType(HTuple) Then
            tup = New HTuple(tup)
        End If
        HOperatorSet.TupleDeviation(tup, ref)
        TupleDeviation = ref
    End Function

    Public Shared Function TupleConcat(tup1 As Object, tup2 As Object)
        Dim ref = New HTuple
        If tup1 Is Nothing Then
            tup1 = New HTuple
        End If
        If Not tup1.GetType() = GetType(HTuple) Then
            tup1 = New HTuple(tup1)
        End If
        If tup2 Is Nothing Then
            tup2 = New HTuple
        End If
        If Not tup2.GetType() = GetType(HTuple) Then
            tup2 = New HTuple(tup2)
        End If
        HOperatorSet.TupleConcat(tup1, tup2, ref)
        TupleConcat = ref
    End Function

    Public Shared Function TupleSelectRange(tup As Object, li As Object, ri As Object)
        Dim ref = New HTuple
        If tup Is Nothing Then
            tup = New HTuple
        End If
        If Not tup.GetType() = GetType(HTuple) Then
            tup = New HTuple(tup)
        End If
        If li Is Nothing Then
            li = New HTuple
        End If
        If Not li.GetType() = GetType(HTuple) Then
            li = New HTuple(li)
        End If
        If ri Is Nothing Then
            ri = New HTuple
        End If
        If Not ri.GetType() = GetType(HTuple) Then
            ri = New HTuple(ri)
        End If
        HOperatorSet.TupleSelectRange(tup, li, ri, ref)
        TupleSelectRange = ref
    End Function

    Public Shared Function TupleSortIndex(tup As Object)
        Dim ref = New HTuple
        If tup Is Nothing Then
            tup = New HTuple
        End If
        If Not tup.GetType() = GetType(HTuple) Then
            tup = New HTuple(tup)
        End If
        HOperatorSet.TupleSortIndex(tup, ref)
        TupleSortIndex = ref
    End Function

    Public Shared Function TupleLessEqual(tup1 As Object, tup2 As Object)
        Dim ref = New HTuple
        If tup1 Is Nothing Then
            tup1 = New HTuple
        End If
        If Not tup1.GetType() = GetType(HTuple) Then
            tup1 = New HTuple(tup1)
        End If
        If tup2 Is Nothing Then
            tup2 = New HTuple
        End If
        If Not tup2.GetType() = GetType(HTuple) Then
            tup2 = New HTuple(tup2)
        End If
        HOperatorSet.TupleLessEqual(tup1, tup2, ref)
        TupleLessEqual = ref
    End Function

    Public Shared Function TupleEqual(tup1 As Object, tup2 As Object)
        Dim ref = New HTuple
        If tup1 Is Nothing Then
            tup1 = New HTuple
        End If
        If Not tup1.GetType() = GetType(HTuple) Then
            tup1 = New HTuple(tup1)
        End If
        If tup2 Is Nothing Then
            tup2 = New HTuple
        End If
        If Not tup2.GetType() = GetType(HTuple) Then
            tup2 = New HTuple(tup2)
        End If
        HOperatorSet.TupleEqual(tup1, tup2, ref)
        TupleEqual = ref
    End Function

    Public Shared Function TupleGreater(tup1 As Object, tup2 As Object)
        Dim ref = New HTuple
        If tup1 Is Nothing Then
            tup1 = New HTuple
        End If
        If Not tup1.GetType() = GetType(HTuple) Then
            tup1 = New HTuple(tup1)
        End If
        If tup2 Is Nothing Then
            tup2 = New HTuple
        End If
        If Not tup2.GetType() = GetType(HTuple) Then
            tup2 = New HTuple(tup2)
        End If
        HOperatorSet.TupleGreater(tup1, tup2, ref)
        TupleGreater = ref
    End Function

    Public Shared Function TupleLess(tup1 As Object, tup2 As Object)
        Dim ref = New HTuple
        If tup1 Is Nothing Then
            tup1 = New HTuple
        End If
        If Not tup1.GetType() = GetType(HTuple) Then
            tup1 = New HTuple(tup1)
        End If
        If tup2 Is Nothing Then
            tup2 = New HTuple
        End If
        If Not tup2.GetType() = GetType(HTuple) Then
            tup2 = New HTuple(tup2)
        End If
        HOperatorSet.TupleLess(tup1, tup2, ref)
        TupleLess = ref
    End Function

    Public Shared Function TupleOr(tup1 As Object, tup2 As Object)
        Dim ref = New HTuple
        If tup1 Is Nothing Then
            tup1 = New HTuple
        End If
        If Not tup1.GetType() = GetType(HTuple) Then
            tup1 = New HTuple(tup1)
        End If
        If tup2 Is Nothing Then
            tup2 = New HTuple
        End If
        If Not tup2.GetType() = GetType(HTuple) Then
            tup2 = New HTuple(tup2)
        End If
        HOperatorSet.TupleOr(tup1, tup2, ref)
        TupleOr = ref
    End Function

    Public Shared Function TupleFirstN(tup1 As Object, tup2 As Object)
        Dim ref = New HTuple
        If tup1 Is Nothing Then
            tup1 = New HTuple
        End If
        If Not tup1.GetType() = GetType(HTuple) Then
            tup1 = New HTuple(tup1)
        End If
        If tup2 Is Nothing Then
            tup2 = New HTuple
        End If
        If Not tup2.GetType() = GetType(HTuple) Then
            tup2 = New HTuple(tup2)
        End If
        HOperatorSet.TupleFirstN(tup1, tup2, ref)
        TupleFirstN = ref
    End Function

    Public Shared Function TupleStrLastN(tup1 As Object, tup2 As Object)
        Dim ref = New HTuple
        If tup1 Is Nothing Then
            tup1 = New HTuple
        End If
        If Not tup1.GetType() = GetType(HTuple) Then
            tup1 = New HTuple(tup1)
        End If
        If tup2 Is Nothing Then
            tup2 = New HTuple
        End If
        If Not tup2.GetType() = GetType(HTuple) Then
            tup2 = New HTuple(tup2)
        End If
        HOperatorSet.TupleStrLastN(tup1, tup2, ref)
        TupleStrLastN = ref
    End Function

    Public Shared Function TupleStrFirstN(tup1 As Object, tup2 As Object)
        Dim ref = New HTuple
        If tup1 Is Nothing Then
            tup1 = New HTuple
        End If
        If Not tup1.GetType() = GetType(HTuple) Then
            tup1 = New HTuple(tup1)
        End If
        If tup2 Is Nothing Then
            tup2 = New HTuple
        End If
        If Not tup2.GetType() = GetType(HTuple) Then
            tup2 = New HTuple(tup2)
        End If
        HOperatorSet.TupleStrFirstN(tup1, tup2, ref)
        TupleStrFirstN = ref
    End Function

    Public Shared Function TupleCumul(tup As Object)
        Dim ref = New HTuple
        If tup Is Nothing Then
            tup = New HTuple
        End If
        If Not tup.GetType() = GetType(HTuple) Then
            tup = New HTuple(tup)
        End If
        HOperatorSet.TupleCumul(tup, ref)
        TupleCumul = ref
    End Function

    Public Shared Function TupleInt(tup As Object)
        Dim ref = New HTuple
        If tup Is Nothing Then
            tup = New HTuple
        End If
        If Not tup.GetType() = GetType(HTuple) Then
            tup = New HTuple(tup)
        End If
        HOperatorSet.TupleInt(tup, ref)
        TupleInt = ref
    End Function

    Public Shared Function TupleNeg(tup As Object)
        Dim ref = New HTuple
        If tup Is Nothing Then
            tup = New HTuple
        End If
        If Not tup.GetType() = GetType(HTuple) Then
            tup = New HTuple(tup)
        End If
        HOperatorSet.TupleNeg(tup, ref)
        TupleNeg = ref
    End Function

    Public Shared Function TupleFind(tup1 As Object, tup2 As Object)
        Dim ref = New HTuple
        If tup1 Is Nothing Then
            tup1 = New HTuple
        End If
        If Not tup1.GetType() = GetType(HTuple) Then
            tup1 = New HTuple(tup1)
        End If
        If tup2 Is Nothing Then
            tup2 = New HTuple
        End If
        If Not tup2.GetType() = GetType(HTuple) Then
            tup2 = New HTuple(tup2)
        End If
        HOperatorSet.TupleFind(tup1, tup2, ref)
        TupleFind = ref
    End Function

    Public Shared Function TupleReplace(tup1 As Object, index As Object, tup2 As Object)
        Dim ref As New HTuple
        HOperatorSet.TupleReplace(tup1, index, tup2, ref)
        TupleReplace = ref
    End Function

End Class
