Public Class CShape
    Public scale As Double = 1.0 ' X
    Public highScale As Double = 1.0 ' Y
    Protected enableHighScale As Boolean = False
    Public Shared c_pi As Double = 3.14159265

    'Extend type
    Public Const noExtend As String = "noExtend"
    Public Const extendArg As String = "extendArg"
    Public Const extendThis As String = "extendThis"
    Public Const extendBoth As String = "extendBoth"

    Public Sub enableYScale(ByVal isEnable As Boolean)
        enableHighScale = isEnable
    End Sub
    Public Shared Function getcolorDotPen(ByVal color As System.Drawing.Color) As System.Drawing.Pen
        Dim cdPen As System.Drawing.Pen
        cdPen = New System.Drawing.Pen(color)
        cdPen.DashStyle = Drawing2D.DashStyle.Dash
        Return cdPen
    End Function
End Class
