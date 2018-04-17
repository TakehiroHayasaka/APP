Public Class StringArray
    '==============================================================================
    Private Const m_IncrementNumber As Long = 10
    Private m_strAry() As String
    Private m_lngSize As Long

    Public Sub New()
        MyBase.New()
        Class_Initialize_Renamed()
    End Sub
    Protected Overrides Sub Finalize()
        Class_Terminate_Renamed()
        MyBase.Finalize()
    End Sub

    '==============================================================================
    '==============================================================================
    Private Sub Class_Initialize_Renamed()
        On Error Resume Next
        ReDim m_strAry(0)
        m_lngSize = 0
    End Sub

    '==============================================================================
    Private Sub Class_Terminate_Renamed()
        On Error Resume Next
        Erase m_strAry
    End Sub

    '==============================================================================
    '==============================================================================
    Public Function Size() As Long
        Size = m_lngSize
    End Function

    '==============================================================================
    Public Function at(ByVal lngIndex As Long) As String
        at = m_strAry(lngIndex)
    End Function
    '==============================================================================
    Public Property Front() As String
        Get
            Front = at(0)
        End Get
        Set(ByVal strValue As String)
            Call SetAt(0, strValue)
        End Set
    End Property
    '==============================================================================
    Public Property Back() As String
        Get
            Back = at(Size() - 1)
        End Get
        Set(ByVal strValue As String)
            Call SetAt(Size() - 1, strValue)
        End Set
    End Property
    '==============================================================================
    Public Sub SetAt(ByVal lngIndex As Long, ByVal strValue As String)
        m_strAry(lngIndex) = strValue
    End Sub

    '==============================================================================
    Public Function Append(ByVal strValue As String) As Long
        If UBound(m_strAry) <= m_lngSize Then
            ReDim Preserve m_strAry(m_lngSize + m_IncrementNumber)
        End If
        m_strAry(m_lngSize) = strValue
        m_lngSize = m_lngSize + 1
        Append = m_lngSize - 1
    End Function

    '==============================================================================
    Public Function AppendIf(ByVal blnValue As Boolean, ByVal strValue As String) As Long
        AppendIf = -1
        If blnValue = False Then Exit Function
        If UBound(m_strAry) <= m_lngSize Then
            ReDim Preserve m_strAry(m_lngSize + m_IncrementNumber)
        End If
        m_strAry(m_lngSize) = strValue
        m_lngSize = m_lngSize + 1
        AppendIf = m_lngSize - 1
    End Function

    '==============================================================================
    Public Sub AppendParamArray(ByVal ParamArray values())
        Dim i As Long
        Dim value As String
        For i = 0 To UBound(values)
            value = values(i)
            Call Me.Append(value)
        Next
    End Sub

    '==============================================================================
    Public Sub AppendArray(ByVal values As StringArray)
        Dim i As Long
        If (values Is Nothing) Then Exit Sub
        For i = 0 To values.Size - 1
            Call Append(values.at(i))
        Next
    End Sub

    '==============================================================================
    '==============================================================================
    Public Sub AddAt(ByVal lngIndex As Long, ByVal strAdd As String)
        If lngIndex < 0 Or Me.Size <= lngIndex Then Exit Sub
        Call Me.SetAt(lngIndex, Me.at(lngIndex) & strAdd)
    End Sub

    '==============================================================================
    Public Sub AddPrefixAt(ByVal lngIndex As Long, ByVal strPrefix As String)
        If lngIndex < 0 Or Me.Size <= lngIndex Then Exit Sub
        Call Me.SetAt(lngIndex, strPrefix & Me.at(lngIndex))
    End Sub

    '==============================================================================
    Public Sub AddPostfixAt(ByVal lngIndex As Long, ByVal strPostfix As String)
        If lngIndex < 0 Or Me.Size <= lngIndex Then Exit Sub
        Call Me.SetAt(lngIndex, Me.at(lngIndex) & strPostfix)
    End Sub

    '==============================================================================
    '==============================================================================
    Public Sub AddPrefix(ByVal strPrefix As String)
        Dim i As Long
        For i = 0 To Me.Size - 1
            Call Me.SetAt(i, strPrefix & Me.at(i))
        Next
    End Sub
    '==============================================================================
    Public Sub AddPostfix(ByVal strPostfix As String)
        Dim i As Long
        For i = 0 To Me.Size - 1
            Call Me.SetAt(i, Me.at(i) & strPostfix)
        Next
    End Sub

    '==============================================================================
    Public Function GetAddedPrefix(ByVal strPrefix As String) As StringArray
        GetAddedPrefix = Me.Clone
        Call GetAddedPrefix.AddPrefix(strPrefix)
    End Function
    '==============================================================================
    Public Function GetAddedPostfix(ByVal strPostfix As String) As StringArray
        GetAddedPostfix = Me.Clone
        Call GetAddedPostfix.AddPostfix(strPostfix)
    End Function

    '==============================================================================
    '==============================================================================
    Public Sub InsertAt(ByVal lngIndex As Long, ByVal strValue As String)
        If UBound(m_strAry) <= m_lngSize Then
            ReDim Preserve m_strAry(m_lngSize + m_IncrementNumber)
        End If
        Dim i As Long
        For i = m_lngSize - 1 To lngIndex Step -1
            m_strAry(i + 1) = m_strAry(i)
        Next
        m_strAry(lngIndex) = strValue
        m_lngSize = m_lngSize + 1
    End Sub

    '==============================================================================
    '==============================================================================
    Public Sub RemoveAt(ByVal lngIndex As Long)
        If lngIndex < 0 Then Exit Sub
        If Me.Size <= lngIndex Then Exit Sub
        Dim i As Long
        For i = lngIndex To m_lngSize - 2
            m_strAry(i) = m_strAry(i + 1)
        Next
        m_lngSize = m_lngSize - 1
    End Sub

    '==============================================================================
    Public Sub RemoveFront()
        Call Me.RemoveAt(0)
    End Sub
    '==============================================================================
    Public Sub RemoveBack()
        Call Me.RemoveAt(Me.Size - 1)
    End Sub
    '==============================================================================
    Public Sub RemovePrevOf(ByVal lngIndex As Long)
        Call Me.RemoveAt(lngIndex - 1)
    End Sub
    '==============================================================================
    Public Sub RemoveNextOf(ByVal lngIndex As Long)
        Call Me.RemoveAt(lngIndex + 1)
    End Sub

    '==============================================================================
    '==============================================================================
    Public Sub Remove(ByVal value As String)
        Dim lngIndex As Long
        Do
            lngIndex = Find(value, 0)
            If lngIndex = -1 Then Exit Do
            Call RemoveAt(lngIndex)
        Loop
    End Sub
    '==============================================================================
    Public Sub RemoveArray(ByVal values As StringArray)
        Dim i As Long
        For i = 0 To values.Size - 1
            Call Remove(values.at(i))
        Next
    End Sub
    '==============================================================================
    Public Sub RemoveString(ByVal strValue As String)
        Dim lngIndex As Long
        Do
            lngIndex = Find(strValue, 0)
            If lngIndex = -1 Then Exit Do
            Call RemoveAt(lngIndex)
        Loop
    End Sub
    '==============================================================================
    Public Sub RemoveStringArray(ByVal strArray As StringArray)
        Dim i As Long
        For i = 0 To strArray.Size - 1
            Call RemoveString(strArray.at(i))
        Next
    End Sub
    '==============================================================================
    Public Sub RemoveStringNoCase(ByVal strValue As String)
        Dim lngIndex As Long
        Do
            lngIndex = FindNoCase(strValue, 0)
            If lngIndex = -1 Then Exit Do
            Call RemoveAt(lngIndex)
        Loop
    End Sub
    '==============================================================================
    Public Sub RemoveStringArrayNoCase(ByVal strArray As StringArray)
        Dim i As Long
        For i = 0 To strArray.Size - 1
            Call RemoveStringNoCase(strArray.at(i))
        Next
    End Sub

    '==============================================================================
    '==============================================================================
    Public Sub RemoveStringLike(ByVal strValue As String, ByVal blnIsMeWC As Boolean)
        Dim lngIndex As Long
        Do
            lngIndex = FindLike(strValue, 0, blnIsMeWC)
            If lngIndex = -1 Then Exit Do
            Call RemoveAt(lngIndex)
        Loop
    End Sub
    '==============================================================================
    Public Sub RemoveStringArrayLike(ByVal strArray As StringArray, ByVal blnIsMeWC As Boolean)
        Dim i As Long
        For i = 0 To strArray.Size - 1
            Call RemoveStringLike(strArray.at(i), blnIsMeWC)
        Next
    End Sub

    '==============================================================================
    '==============================================================================
    Public Function GetRemovedString(ByVal strValue As String) As StringArray
        GetRemovedString = Me.Clone
        Call GetRemovedString.RemoveString(strValue)
    End Function
    '==============================================================================
    Public Function GetRemovedStringArray(ByVal strArray As StringArray) As StringArray
        GetRemovedStringArray = Me.Clone
        Call GetRemovedStringArray.RemoveStringArray(strArray)
    End Function
    '==============================================================================
    Public Function GetRemovedStringNoCase(ByVal strValue As String) As StringArray
        GetRemovedStringNoCase = Me.Clone
        Call GetRemovedStringNoCase.RemoveStringNoCase(strValue)
    End Function
    '==============================================================================
    Public Function GetRemovedStringArrayNoCase(ByVal strArray As StringArray) As StringArray
        GetRemovedStringArrayNoCase = Me.Clone
        Call GetRemovedStringArrayNoCase.RemoveStringArrayNoCase(strArray)
    End Function
    '==============================================================================
    Public Function GetRemovedStringLike(ByVal strValue As String, ByVal blnIsMeWC As Boolean) As StringArray
        GetRemovedStringLike = Me.Clone
        Call GetRemovedStringLike.RemoveStringLike(strValue, blnIsMeWC)
    End Function
    '==============================================================================
    Public Function GetRemovedStringArrayLike(ByVal strArray As StringArray, ByVal blnIsMeWC As Boolean) As StringArray
        GetRemovedStringArrayLike = Me.Clone
        Call GetRemovedStringArrayLike.RemoveStringArrayLike(strArray, blnIsMeWC)
    End Function

    '==============================================================================
    '==============================================================================
    Public Sub Clear()
        ReDim m_strAry(0)
        m_lngSize = 0
    End Sub

    '==============================================================================
    '==============================================================================
    Public Function Find(ByVal strValue As String, ByVal lngStartPos As Long) As Long
        Find = -1
        Dim i As Long
        For i = lngStartPos To m_lngSize - 1
            If m_strAry(i) = strValue Then
                Find = i
                Exit Function
            End If
        Next
    End Function

    '==============================================================================
    Public Function RFind(ByVal strValue As String, ByVal lngStartPos As Long) As Long
        RFind = -1
        Dim i As Long
        If lngStartPos = -1 Then lngStartPos = Size() - 1
        For i = lngStartPos To 0 Step -1
            If m_strAry(i) = strValue Then
                RFind = i
                Exit Function
            End If
        Next
    End Function

    '==============================================================================
    Public Function FindNoCase(ByVal strValue As String, ByVal lngStartPos As Long) As Long
        FindNoCase = -1
        Dim i As Long
        For i = lngStartPos To m_lngSize - 1
            If LCase(m_strAry(i)) = LCase(strValue) Then
                FindNoCase = i
                Exit Function
            End If
        Next
    End Function

    '==============================================================================
    Public Function RFindNoCase(ByVal strValue As String, ByVal lngStartPos As Long) As Long
        RFindNoCase = -1
        Dim i As Long
        If lngStartPos = -1 Then lngStartPos = Size() - 1
        For i = lngStartPos To 0 Step -1
            If LCase(m_strAry(i)) = LCase(strValue) Then
                RFindNoCase = i
                Exit Function
            End If
        Next
    End Function

    '==============================================================================
    '==============================================================================
    Public Function FindLike(ByVal strValue As String, ByVal lngStartPos As Long, ByVal blnIsMeWC As Boolean) As Long
        FindLike = -1
        Dim i As Long
        For i = lngStartPos To m_lngSize - 1
            If blnIsMeWC Then
                If strValue Like m_strAry(i) Then
                    FindLike = i
                    Exit Function
                End If
            Else
                If m_strAry(i) Like strValue Then
                    FindLike = i
                    Exit Function
                End If
            End If
        Next
    End Function


    '==============================================================================
    '==============================================================================
    Public Function BinarySearch(ByVal strValue As String, ByVal lngStartPos As Long) As Long
        BinarySearch = -1
        Dim lngAt1 As Long, lngAt2 As Long, lngAt As Long
        lngAt1 = lngStartPos
        lngAt2 = m_lngSize - 1
        Do
            lngAt = (lngAt1 + lngAt2) / 2
            'Debug.Print lngAt, lngAt1, lngAt2, m_strAry(lngAt), strValue
            Select Case StrComp(strValue, m_strAry(lngAt))
                Case -1
                    If lngAt1 = lngAt2 Then Exit Function
                    If lngAt2 = lngAt Then lngAt = lngAt1
                    lngAt2 = lngAt
                Case 0
                    BinarySearch = lngAt
                    Exit Function
                Case 1
                    If lngAt1 = lngAt2 Then Exit Function
                    If lngAt1 = lngAt Then lngAt = lngAt2
                    lngAt1 = lngAt
            End Select
        Loop
    End Function

    '==============================================================================
    '==============================================================================
    Public Function GetIsEqualTo(ByVal strings2 As StringArray) As Boolean
        If Me.Size <> strings2.Size Then Exit Function
        Dim i As Long
        For i = 0 To Me.Size - 1
            If Me.at(i) <> strings2.at(i) Then Exit Function
        Next
        GetIsEqualTo = True
    End Function

    '==============================================================================
    Public Function GetIsLikeTo(ByVal strings2 As StringArray, ByVal blnIsMeWC As Boolean) As Boolean
        If Me.Size <> strings2.Size Then Exit Function
        Dim i As Long
        For i = 0 To Me.Size - 1
            If blnIsMeWC Then
                If Not strings2.at(i) Like Me.at(i) Then Exit Function
            Else
                If Not Me.at(i) Like strings2.at(i) Then Exit Function
            End If
        Next
        GetIsLikeTo = True
    End Function

    '==============================================================================
    '==============================================================================
    Public Sub Swap(ByVal lngAt1 As Long, ByVal lngAt2 As Long)
        Dim strValue As String
        strValue = m_strAry(lngAt1)
        m_strAry(lngAt1) = m_strAry(lngAt2)
        m_strAry(lngAt2) = strValue
    End Sub

    '==============================================================================
    '==============================================================================
    Public Function Clone() As StringArray
        Clone = New StringArray
        Dim i As Long
        For i = 0 To Size() - 1
            Call Clone.Append(at(i))
        Next
    End Function

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
    Public Function GetReversed() As StringArray
        GetReversed = Me.Clone
        Call GetReversed.Reverse()
    End Function

    '==============================================================================
    '==============================================================================
    Public Function GetUnique() As StringArray
        GetUnique = Me.Clone
        Dim i As Long
        For i = Me.Size - 1 To 1 Step -1
            Dim lngIndex As Long
            lngIndex = GetUnique.Find(Me.at(i), 0)
            If lngIndex <> -1 And lngIndex < i Then Call GetUnique.RemoveAt(i)
        Next
    End Function

    '==============================================================================
    '==============================================================================
    Public Function GetCountOf(ByVal value As String) As Long
        Dim i As Long
        GetCountOf = 0
        For i = 0 To Size() - 1
            If at(i) = value Then GetCountOf = GetCountOf + 1
        Next
    End Function

    '==============================================================================
    '==============================================================================
    Public Function GetAdded(ByVal values As StringArray) As StringArray
        GetAdded = New StringArray
        Call GetAdded.AppendArray(Me)
        Call GetAdded.AppendArray(values)
    End Function

    '==============================================================================
    '==============================================================================
    Public Sub GetArray(ByVal ary() As String)
        ReDim ary(Me.Size)
        Dim i As Long
        For i = 0 To Me.Size - 1
            ary(i) = Me.at(i)
        Next
    End Sub

    '==============================================================================
    '==============================================================================
    Public Function GetMinLength() As Long
        Dim i As Long
        GetMinLength = -1
        For i = 0 To Me.Size - 1
            If GetMinLength = -1 Or GetMinLength > Len(Me.at(i)) Then GetMinLength = Len(Me.at(i))
        Next
    End Function
    '==============================================================================
    Public Function GetMaxLength() As Long
        Dim i As Long
        GetMaxLength = 0
        For i = 0 To Me.Size - 1
            If GetMaxLength < Len(Me.at(i)) Then GetMaxLength = Len(Me.at(i))
        Next
    End Function

    '==============================================================================
    '==============================================================================
    Public Function GetPrintString() As String
        Dim i As Long
        GetPrintString = "PrintClass: Size: " & Me.Size & vbCr
        For i = 0 To Me.Size - 1
            GetPrintString = GetPrintString & "String[" & i & "/" & Me.Size & "]:[" & Me.at(i) & "]" & vbCr
        Next
    End Function

End Class
