Public Class CLabelText
    Public x As Double
    Public y As Double
    Public z As Double
    Public LabelName As String
    Public mid As Integer
    Public blnDraw As Boolean
    Public DispFlag As Boolean
    Public HighlightFlag As Boolean

    Public Sub New()
        x = 0.0
        y = 0.0
        z = 0.0
        mid = -1
        blnDraw = True
        DispFlag = True
        HighlightFlag = False
    End Sub
    Public Sub New(ByVal _x As Double, ByVal _y As Double, ByVal _z As Double)
        x = _x
        y = _y
        z = _z
        blnDraw = True
        DispFlag = True
        HighlightFlag = False
    End Sub
End Class
