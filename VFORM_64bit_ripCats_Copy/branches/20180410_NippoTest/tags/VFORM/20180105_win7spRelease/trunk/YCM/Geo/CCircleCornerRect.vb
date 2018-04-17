Public Class CCircleCornerRect
    Inherits CShape
    Public startPoint As New CPoint
    Public centrePoint As New CPoint
    Public radius As Double
    Public inWidth As Double
    Public outWidth As Double
    Public outLength As Double
    Public rect As Rectangle
    Public linePen As New Pen(Color.Black, 1)
    Public fillBrush As New SolidBrush(Color.Gray)
    Public Shared s_gp As System.Drawing.Graphics = Nothing
    'Public Sub drawCircelCornerRect()
    '    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
    '    If Not CCircleCornerRect.s_gp Is Nothing Then
    '        drawCircelCornerRect(CCircleCornerRect.s_gp)
    '    End If
    'End Sub
    Public Sub setRect()
        rect = New Rectangle(startPoint.x, startPoint.y, outWidth, outLength)
    End Sub
    Public Function GetRoundedRectPath(ByVal rect As Rectangle, ByVal radius As Double) As System.Drawing.Drawing2D.GraphicsPath
        ' Start by lxh 2010-11-18
        rect.Offset(-1, -1)
        radius *= 2.0

        Dim RoundRect As New Rectangle(rect.Location, New Size(radius, radius))
        'Dim RoundRect As New Rectangle(rect.Location, New Size(radius - 1, radius - 1))
        ' End by lxh 2010-11-18
        Dim path As New System.Drawing.Drawing2D.GraphicsPath
        If radius <> 0 Then
            path.AddArc(RoundRect, 180, 90)     'ç∂è„äp

            RoundRect.X = rect.Right - radius   'âEè„äp
            path.AddArc(RoundRect, 270, 90)

            RoundRect.Y = rect.Bottom - radius  'âEâ∫äp
            path.AddArc(RoundRect, 0, 90)

            RoundRect.X = rect.Left             'ç∂â∫äp
            path.AddArc(RoundRect, 90, 90)
        End If
        path.CloseFigure()
        Return path

    End Function
    Public Sub drawCircelCornerRect(ByRef gp As System.Drawing.Graphics)
        Dim gph As System.Drawing.Drawing2D.GraphicsPath
        setRect()
        gph = GetRoundedRectPath(rect, radius)
        gp.FillPath(fillBrush, gph)
        gp.DrawPath(linePen, gph)

    End Sub
End Class
