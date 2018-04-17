Public Class DrawRay
    Public startPnt As CPoint3d
    Public endPnt As CPoint3d
    Public CPID As Integer
    Public MID As Integer
    Public PointID As Integer
    Public blnDraw As Boolean

    Public Sub New()
        startPnt = New CPoint3d
        endPnt = New CPoint3d
        blnDraw = True
    End Sub
    Public Sub SetStartPnt(ByVal value_x As Double, ByVal value_y As Double, ByVal value_z As Double)
        startPnt.x = value_x
        startPnt.y = value_y
        startPnt.z = value_z

    End Sub
    Public Sub SetEndPnt(ByVal value_x As Double, ByVal value_y As Double, ByVal value_z As Double)
        endPnt.x = value_x
        endPnt.y = value_y
        endPnt.z = value_z
    End Sub

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
End Class
