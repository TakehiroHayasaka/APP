Imports System.Globalization

Public Class ObjTool

    Public Shared Function ParseInvariantFloat(floatString As String) As Single
        Return Single.Parse(floatString, CultureInfo.InvariantCulture.NumberFormat)
    End Function


    Public Shared Function ParseInvariantInt(intString As String) As Integer
        Return Integer.Parse(intString, CultureInfo.InvariantCulture.NumberFormat)
    End Function

    Public Shared Function EqualsInvariantCultureIgnoreCase(str As String, s As String) As Boolean
        Return str.Equals(s, StringComparison.InvariantCultureIgnoreCase)
    End Function

    Public Shared Function IsNullOrEmpty(str As String) As Boolean
        Return String.IsNullOrEmpty(str)
    End Function

    Public Shared Function InlineAssignHelper(Of T)(ByRef target As T, ByVal value As T) As T
        target = value
        Return value
    End Function
End Class
