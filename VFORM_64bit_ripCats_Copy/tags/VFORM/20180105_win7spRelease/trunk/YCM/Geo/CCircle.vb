Public Class CCircle
    Public centerPoint As New CPoint
    Public radius As Double
    Public linePen As New Pen(Color.Black, 1)
    Public fillBrush As New SolidBrush(Color.White)
    Public drawLineOnly As Boolean = True
    Public all As Boolean = False
    Public Shared s_gp As System.Drawing.Graphics = Nothing

    'Public Sub drawCircle()
    '    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
    '    If Not CCircle.s_gp Is Nothing Then
    '        drawCircle(CCircle.s_gp)
    '    End If
    'End Sub
    Public Sub drawCircle(ByRef gp As System.Drawing.Graphics)
        Dim pTmp As New CPoint
        pTmp.x = centerPoint.x - radius
        pTmp.y = centerPoint.y - radius


        'gp.DrawArc(linePen, pTmp.getIntx(), pTmp.getInty(), CInt(radius * 2.0), CInt(radius * 2.0), 0, 360)
        If drawLineOnly = True Then
            gp.DrawEllipse(linePen, pTmp.getIntx(), pTmp.getInty(), CInt(radius * 2.0), CInt(radius * 2.0))
        Else
            gp.FillEllipse(fillBrush, pTmp.getIntx(), pTmp.getInty(), CInt(radius * 2.0), CInt(radius * 2.0))
        End If

        If all = True Then
            gp.FillEllipse(fillBrush, pTmp.getIntx(), pTmp.getInty(), CInt(radius * 2.0), CInt(radius * 2.0))
            gp.DrawEllipse(linePen, pTmp.getIntx(), pTmp.getInty(), CInt(radius * 2.0), CInt(radius * 2.0))
        End If

    End Sub
    Public Function pointOnCircle(ByVal pnt As CPoint) As Boolean
        Return pnt.distTo(centerPoint) <= radius
    End Function
End Class
