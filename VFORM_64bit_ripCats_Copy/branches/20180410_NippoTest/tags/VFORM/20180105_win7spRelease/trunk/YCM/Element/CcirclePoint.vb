Public Class CcirclePoint
    Public x As Double
    Public y As Double
    Public z As Double
    Public Real_x As Double
    Public Real_y As Double
    Public Real_z As Double
    Public mid As Integer
    Public tid As Integer
    Public blnDraw As Boolean
    Public LabelName As String
    Public sortID As Integer
    Public blnDelOnGlwin As Boolean '=0：未削除、    =1：削除済み 'Add Kiryu CATS/2016-a3-20160920
    Public model_pick As UInteger   '描画時の描画IDを保存         'Add Kiryu CATS/2016-a3-20160920

    Public Sub New()
        x = 0.0
        y = 0.0
        z = 0.0
        mid = -1
        blnDraw = True
    End Sub
    Public Sub New(ByVal _x As Double, ByVal _y As Double, ByVal _z As Double)
        x = _x
        y = _y
        z = _z
        Real_x = _x
        Real_y = _y
        Real_z = _z
        blnDraw = True
    End Sub
    Public Function getx() As Double
        Return x
    End Function
    Public Function gety() As Double
        Return y
    End Function
    Public Function getz() As Double
        Return z
    End Function
    Public Function distTo(ByVal pnt As CLookPoint) As Double
        Dim r As Double = ((x - pnt.x) * (x - pnt.x)) + ((y - pnt.y) * (y - pnt.y) + (z - pnt.z) * (z - pnt.z))
        Return Math.Sqrt(r)
    End Function
End Class
