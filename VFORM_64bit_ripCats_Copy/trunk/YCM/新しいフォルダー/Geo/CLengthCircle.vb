Public Class CLengthCircle
    Inherits CShape
    Public width As Double
    Public length As Double
    Public centrePoint As New CPoint
    Public radius As Integer
    Public circleCentrePoint As New CPoint
    Public lengthUpStartPoint As New CPoint
    Public lengthDownStartPoint As New CPoint
    Public L_line As New CLine
    Public R_line As New CLine

    Public linePen As New Pen(Color.Black, 1)
    Public earsePen As New Pen(Color.Gray, 5)
    Public fillBrush As New SolidBrush(Color.Gray)

    Public Shared s_gp As System.Drawing.Graphics = Nothing
    'Public Sub drawLengthCircle()
    '    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
    '    If Not CLengthCircle.s_gp Is Nothing Then
    '        drawLengthCircle(CCircleCornerRect.s_gp)
    '    End If
    'End Sub
    Public Sub drawLengthCircle(ByRef gp As System.Drawing.Graphics)
        update()
        Dim gph As System.Drawing.Drawing2D.GraphicsPath
        gph = GetLengthCirclePath()
        s_gp.FillPath(fillBrush, gph)
        s_gp.DrawPath(linePen, gph)
        s_gp.DrawLine(earsePen, L_line.endPnt.getIntx(), L_line.endPnt.getInty(), R_line.endPnt.getIntx(), R_line.endPnt.getInty())
    End Sub
    Public Sub update()
        If enableHighScale = False Then
            highScale = scale
        End If
        setRadius()
        setCircleCenter()
        setLengthStartPoint()
        setLinePoint()
    End Sub
    Public Sub setRadius()
        'radius = width / 2 * scale
    End Sub
    Public Sub setCircleCenter()
        circleCentrePoint.x = centrePoint.x
        circleCentrePoint.y = (centrePoint.y - width / 2)
    End Sub
    Public Sub setLengthStartPoint()
        lengthUpStartPoint.x = (centrePoint.x - width / 2)
        lengthUpStartPoint.y = (centrePoint.y - length / 2)

        lengthDownStartPoint.x = (centrePoint.x - width / 2)
        lengthDownStartPoint.y = (centrePoint.y + length / 2 - radius * 2)
    End Sub
    Private Sub setLinePoint()
        L_line.startPnt.x = (centrePoint.x - width / 2)
        L_line.startPnt.y = (centrePoint.y - length / 2 + radius)
        L_line.endPnt.x = (centrePoint.x - width / 2)
        L_line.endPnt.y = (centrePoint.y + (length - radius * 2) / 2)

        R_line.startPnt.x = (centrePoint.x + width / 2)
        R_line.startPnt.y = (centrePoint.y - (length - radius * 2) / 2)
        R_line.endPnt.x = (centrePoint.x + width / 2)
        R_line.endPnt.y = (centrePoint.y + (length - radius * 2) / 2)
    End Sub
    Public Function GetLengthCirclePath() As System.Drawing.Drawing2D.GraphicsPath

        Dim upRect As Rectangle = New Rectangle(lengthUpStartPoint.x, lengthUpStartPoint.y, radius * 2, radius * 2)
        Dim downRect As Rectangle = New Rectangle(lengthDownStartPoint.x, lengthDownStartPoint.y, radius * 2, radius * 2)

        Dim path As System.Drawing.Drawing2D.GraphicsPath
        path = New System.Drawing.Drawing2D.GraphicsPath
        path.AddArc(upRect, 0, -180)
        path.AddLine(L_line.startPnt.getIntx(), L_line.startPnt.getInty(), L_line.endPnt.getIntx(), L_line.endPnt.getInty())
        path.AddArc(downRect, 0, 180)
        path.AddLine(R_line.endPnt.getIntx(), R_line.endPnt.getInty(), R_line.startPnt.getIntx(), R_line.startPnt.getInty())
        path.CloseFigure()
        Return path
    End Function
End Class
