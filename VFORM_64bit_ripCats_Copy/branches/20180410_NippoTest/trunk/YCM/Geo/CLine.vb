Public Class CLine
    Inherits CShape

    Public startPnt As CPoint3d
    Public endPnt As CPoint3d
    Public linePen As System.Drawing.Pen
    Public blnDraw As Boolean
    Public Shared s_gp As System.Drawing.Graphics = Nothing

    Public Sub New()
        startPnt = New CPoint3d
        endPnt = New CPoint3d
        blnDraw = False
        'linePen = Pens.Black
        'linePen.DashStyle = Drawing2D.DashStyle.DashDot
    End Sub
    Public Sub SetStartPnt(ByVal value_x As Double, ByVal value_y As Double, ByVal value_z As Double)
        startPnt.x = value_x
        startPnt.y = value_y
        startPnt.z = value_z
        'startPnt = New CPoint3d
        'endPnt = New CPoint3d
        'blnDraw = False
        'linePen = Pens.Black
        'linePen.DashStyle = Drawing2D.DashStyle.DashDot
    End Sub
    Public Sub SetEndPnt(ByVal value_x As Double, ByVal value_y As Double, ByVal value_z As Double)
        endPnt.x = value_x
        endPnt.y = value_y
        endPnt.z = value_z
        'linePen = Pens.Black
        'linePen.DashStyle = Drawing2D.DashStyle.DashDot
    End Sub

    'Public Sub drawLine()
    '    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
    '    If Not CLine.s_gp Is Nothing Then
    '        drawLine(CLine.s_gp)
    '    End If
    'End Sub
    'Public Sub drawLine()

    '    gp.DrawLine(linePen, startPnt.getIntx(), startPnt.getInty(), endPnt.getIntx(), endPnt.getInty())
    'End Sub
    Public Function length() As Double
        Return startPnt.distTo(endPnt)
    End Function
    Public Function pointOnLine(ByVal pnt As CPoint3d) As Boolean
        Dim dist1 As Double = startPnt.distTo(pnt)
        Dim dist2 As Double = endPnt.distTo(pnt)
        Dim dblLength As Double = length()
        If Math.Abs((dist1 + dist2) - dblLength) < 0.001 Then
            'If dist1 + dist2 = length() Then
            Return True
        End If
    End Function

    Public Function pointDist(ByVal pnt As CPoint3d) As Double
        Dim vec1 As CVector3d = pnt - startPnt
        Dim vec2 As CVector3d = endPnt - startPnt

        Dim dotp As Double = vec2.dotProductTo(vec1)
        Dim K As Double = dotp / (vec2.length() * vec2.length())
        vec2 = vec2 * K
        Dim newPnt As CPoint3d = startPnt + vec2
        Return pnt.distTo(newPnt)
    End Function

    Public Function getMiddlePoint() As CPoint3d
        Dim vec As CVector3d = endPnt - startPnt
        vec.normalize()
        Return startPnt + vec * (length() / 2.0)
    End Function

    Public Function horizontalLap(ByVal line As CLine) As Boolean
        If length() = line.length() Then
            If line.startPnt.x = startPnt.x Then
                Return True
            End If
        End If
        If line.startPnt.x > startPnt.x And line.startPnt.x < endPnt.x Then
            Return True
        End If
        If line.endPnt.x > startPnt.x And line.endPnt.x < endPnt.x Then
            Return True
        End If
        If startPnt.x > line.startPnt.x And startPnt.x < line.endPnt.x Then
            Return True
        End If
        If endPnt.x > line.startPnt.x And endPnt.x < line.endPnt.x Then
            Return True
        End If

        Return False
    End Function

    'Public Function intersectWith(ByVal line As CLine, Optional ByVal extendType As String = CShape.extendBoth) As CPoint
    '    If startPnt.x = endPnt.x And startPnt.y = endPnt.y Then
    '        Return Nothing
    '    End If

    '    If line.startPnt.x = line.endPnt.x And line.startPnt.y = line.endPnt.y Then
    '        Return Nothing
    '    End If

    '    Dim vec1 As CVector3d = endPnt - startPnt
    '    Dim vec2 As CVector3d = line.endPnt - line.startPnt
    '    vec1.normalize()
    '    vec2.normalize()

    '    If vec1.x = vec2.x And vec1.y = vec2.y Then
    '        Return Nothing
    '    End If

    '    Dim a1, b1, a2, b2 As Double

    '    a1 = 0.0
    '    If endPnt.x <> startPnt.x Then
    '        a1 = (endPnt.y - startPnt.y) / (endPnt.x - startPnt.x)
    '    End If

    '    b1 = startPnt.y - a1 * startPnt.x

    '    a2 = 0.0
    '    If line.endPnt.x <> line.startPnt.x Then
    '        a2 = (line.endPnt.y - line.startPnt.y) / (line.endPnt.x - line.startPnt.x)
    '    End If

    '    b2 = line.startPnt.y - a2 * line.startPnt.x

    '    Dim resultPnt As New CPoint

    '    If endPnt.x = startPnt.x Then
    '        resultPnt.x = startPnt.x
    '        resultPnt.y = a2 * resultPnt.x + b2
    '    ElseIf line.endPnt.x = line.startPnt.x Then
    '        resultPnt.x = line.startPnt.x
    '        resultPnt.y = a1 * resultPnt.x + b1
    '    Else
    '        resultPnt.x = (b2 - b1) / (a1 - a2)
    '        resultPnt.y = a1 * resultPnt.x + b1
    '    End If

    '    If extendType = CShape.noExtend Or extendType = CShape.extendArg Then
    '        If pointOnLine(resultPnt) = False Then
    '            Return Nothing
    '        End If
    '    End If

    '    If extendType = CShape.noExtend Or extendType = CShape.extendThis Then
    '        If line.pointOnLine(resultPnt) = False Then
    '            Return Nothing
    '        End If
    '    End If

    '    Return resultPnt
    'End Function
End Class
