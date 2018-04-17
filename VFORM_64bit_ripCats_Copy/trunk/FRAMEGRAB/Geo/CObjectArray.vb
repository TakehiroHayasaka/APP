Public Class CObjectArray
    '==============================================================================
    Private Const m_IncrementNumber As Long = 10
    Private m_objAry() As Object
    Private m_lngSize As Long
    Private m_lngIndex As Long

    '==============================================================================
    Public Sub New()
        On Error Resume Next
        ReDim m_objAry(0)
        m_lngSize = 0
        m_lngIndex = 0
    End Sub
    '==============================================================================
    Protected Overrides Sub Finalize()
        On Error Resume Next
        Call NothingAll()
    End Sub
    '==============================================================================
    Public Sub NothingAll()
        On Error Resume Next
        Dim i As Long, num As Long
        num = IIf(Me.Size > UBound(m_objAry), UBound(m_objAry), Me.Size)
        If (num > 0) Then
            For i = 0 To num - 1
                m_objAry(i) = Nothing
            Next
        End If
        Erase m_objAry
        ReDim m_objAry(0)
        m_lngSize = 0
        m_lngIndex = 0
    End Sub

    '==============================================================================
    Public Function Copy() As CObjectArray
        Copy = New CObjectArray
        Call Copy.AppendArray(Me)
    End Function
    '==============================================================================
    Public Function Size() As Long
        Size = m_lngSize
    End Function
    '==============================================================================
    Public Function at(ByVal lngIndex As Long) As Object
        ' インデクスが範囲外の場合は Nothing を返す
        If lngIndex = -1 Then lngIndex = index
        If lngIndex < 0 Or Size() <= lngIndex Then
            at = Nothing
            Exit Function
        End If
        at = m_objAry(lngIndex)
    End Function
    '==============================================================================
    Public Function AtNextOf(ByVal lngIndex As Long) As Object
        If lngIndex = -1 Then lngIndex = index
        AtNextOf = at(lngIndex + 1)
    End Function
    '==============================================================================
    Public Function AtPrevOf(ByVal lngIndex As Long) As Object
        If lngIndex = -1 Then lngIndex = index
        AtPrevOf = at(lngIndex - 1)
    End Function
    '==============================================================================
    Public Function Front() As Object
        Front = at(0)
    End Function
    '==============================================================================
    Public Function Back() As Object
        Back = at(Size - 1)
    End Function
    '==============================================================================
    Public Sub SetAt(ByVal lngIndex As Long, ByVal value As Object)
        m_objAry(IIf(lngIndex = -1, index, lngIndex)) = value
    End Sub
    '==============================================================================
    Public Sub SetAtNextOf(ByVal lngIndex As Long, ByVal value As Object)
        Call SetAt(IIf(lngIndex = -1, index, lngIndex) + 1, value)
    End Sub
    '==============================================================================
    Public Sub SetAtPrevOf(ByVal lngIndex As Long, ByVal value As Object)
        Call SetAt(IIf(lngIndex = -1, index, lngIndex) - 1, value)
    End Sub

    '==============================================================================
    Public Function AddLine(ByVal gpStart As GeoPoint, ByVal gpEnd As GeoPoint) As Long
        Dim celm As CElment, iRet As Integer
        cElm = New CElment(ElementType.ElmLine)
        iRet = celm.SetLine(gpStart, gpEnd)
        AddLine = Me.Append(celm)
    End Function
    Public Function AddCircle(ByVal gpOrg As GeoPoint, ByVal rad As Double, Optional ByVal xAng As Double = 0, Optional ByVal yAng As Double = 0) As Long
        Dim celm As CElment, iRet As Integer
        celm = New CElment(ElementType.ElmCircle)
        iRet = celm.SetCircle(gpOrg, rad, xAng, yAng)
        AddCircle = Me.Append(celm)
    End Function

    '==============================================================================
    Public Function Append(ByVal value As Object) As Long
        On Error GoTo Err_Lbl
        If UBound(m_objAry) <= m_lngSize Then
            ReDim Preserve m_objAry(m_lngSize + m_IncrementNumber)
        End If
        m_objAry(m_lngSize) = value
        m_lngSize = m_lngSize + 1
        Exit Function
Err_Lbl:
        Debug.Print("ObjectArray.Append[" & Err.Description & "]")
    End Function

    '==============================================================================
    Public Function AppendIf(ByVal blnValue As Boolean, ByVal value As Object) As Long
        AppendIf = -1
        If blnValue = False Then Exit Function
        If UBound(m_objAry) <= m_lngSize Then
            ReDim Preserve m_objAry(m_lngSize + m_IncrementNumber)
        End If
        m_objAry(m_lngSize) = value
        m_lngSize = m_lngSize + 1
        AppendIf = m_lngSize - 1
    End Function

    '==============================================================================
    Public Sub AppendParamArray(ByVal ParamArray values())
        Dim i As Long
        Dim obj As Object
        For i = 0 To UBound(values)
            obj = values(i)
            Call Me.Append(obj)
        Next
    End Sub

    '==============================================================================
    Public Sub AppendArray(ByVal values As CObjectArray)
        Dim i As Long
        If (values Is Nothing) Then Exit Sub
        For i = 0 To values.Size - 1
            Call Append(values.at(i))
        Next
    End Sub

    '==============================================================================
    Public Sub InsertAt(ByVal lngIndex As Long, ByVal value As Object)
        If UBound(m_objAry) <= m_lngSize Then
            ReDim Preserve m_objAry(m_lngSize + m_IncrementNumber)
        End If
        Dim i As Long
        For i = m_lngSize - 1 To IIf(lngIndex = -1, index, lngIndex) Step -1
            m_objAry(i + 1) = m_objAry(i)
        Next
        m_objAry(IIf(lngIndex = -1, index, lngIndex)) = value
        m_lngSize = m_lngSize + 1
    End Sub

    '==============================================================================
    Public Sub InsertArrayAt(ByVal lngIndex As Long, ByVal values As CObjectArray)
        Dim i As Long
        For i = values.Size - 1 To 0 Step -1
            Call InsertAt(IIf(lngIndex = -1, index, lngIndex), values.at(i))
        Next
    End Sub

    '==============================================================================
    Public Sub RemoveAt(ByVal lngIndex As Long)
        Dim i As Long
        For i = IIf(lngIndex = -1, index, lngIndex) To m_lngSize - 2
            m_objAry(i) = m_objAry(i + 1)
        Next
        m_lngSize = m_lngSize - 1
    End Sub

    '==============================================================================
    Public Sub RemoveNothing()
        Dim i As Long
        For i = Me.Size - 1 To 0 Step -1
            If Me.at(i) Is Nothing Then Call Me.RemoveAt(i)
        Next
    End Sub

    '==============================================================================
    Public Sub RemoveObject(ByVal removeObj As Object)
        Dim lngIndex As Long
        Do
            lngIndex = Find(removeObj, 0)
            If lngIndex = -1 Then Exit Do
            Call RemoveAt(lngIndex)
        Loop
    End Sub
    '==============================================================================
    Public Sub RemoveObjectArray(ByVal removeObjs As CObjectArray)
        Dim i As Long
        For i = 0 To removeObjs.Size - 1
            Call RemoveObject(removeObjs.at(i))
        Next
    End Sub

    '==============================================================================
    Public Sub Clear()
        Call NothingAll()
    End Sub

    '==============================================================================
    '==============================================================================
    Public Sub Swap(ByVal lngAt1 As Long, ByVal lngAt2 As Long)
        Dim objValue As Object
        objValue = m_objAry(lngAt1)
        m_objAry(lngAt1) = m_objAry(lngAt2)
        m_objAry(lngAt2) = objValue
    End Sub

    '==============================================================================
    '==============================================================================
    Public Sub Reverse()
        Dim i As Long
        Dim lngSize As Long
        lngSize = Size() / 2 - 1
        For i = 0 To lngSize
            Call Swap(i, Size() - 1 - i)
        Next
    End Sub

    '==============================================================================
    '==============================================================================
    Public Property index() As Long
        Get
            Return m_lngIndex
        End Get
        Set(ByVal value As Long)
            m_lngIndex = value
        End Set
    End Property
    '==============================================================================
    '==============================================================================
    Public Sub start()
        index = 0
    End Sub

    '==============================================================================
    Public Sub RevStart()
        index = Size() - 1
    End Sub

    '==============================================================================
    Public Sub MovePrev()
        index = index - 1
    End Sub

    '==============================================================================
    Public Sub MoveNext()
        index = index + 1
    End Sub

    '==============================================================================
    Public Function IsEnd() As Boolean
        IsEnd = (index >= Size())
    End Function

    '==============================================================================
    Public Function IsRevEnd() As Boolean
        IsRevEnd = (index < 0)
    End Function

    '==============================================================================
    '==============================================================================
    Public Function IsFirstIndex() As Boolean
        IsFirstIndex = (index = 0)
    End Function

    '==============================================================================
    Public Function IsLastIndex() As Boolean
        IsLastIndex = (index = Size() - 1)
    End Function

    '==============================================================================
    '==============================================================================
    Public Function GetAdded(ByVal values As CObjectArray) As CObjectArray
        GetAdded = New CObjectArray
        Call GetAdded.AppendArray(Me)
        Call GetAdded.AppendArray(values)
    End Function
    '==============================================================================
    '==============================================================================
    Public Function GetAnd(ByVal ary As CObjectArray) As CObjectArray
        GetAnd = New CObjectArray
        Dim i As Long
        For i = 0 To ary.Size - 1
            Call GetAnd.AppendIf(Me.Find(ary.at(i), 0) <> -1, ary.at(i))
        Next
    End Function

    '==============================================================================
    '==============================================================================
    Public Function GetUnique() As CObjectArray
        GetUnique = Me.Copy
        Dim i As Long
        For i = Me.Size - 1 To 1 Step -1
            Dim lngIndex As Long
            lngIndex = GetUnique.Find(Me.at(i), 0)
            If lngIndex <> -1 And lngIndex < i Then Call GetUnique.RemoveAt(i)
        Next
    End Function

    '==============================================================================
    '==============================================================================
    Public Function Find(ByVal obj As Object, ByVal lngStartPos As Long) As Long
        Find = -1
        Dim i As Long
        For i = lngStartPos To m_lngSize - 1
            If m_objAry(i) Is obj = True Then
                Find = i
                Exit Function
            End If
        Next
    End Function


End Class
