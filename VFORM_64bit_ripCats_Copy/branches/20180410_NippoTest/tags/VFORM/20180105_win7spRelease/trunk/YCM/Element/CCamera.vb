Public Class CCamera
    Public x As Double
    Public y As Double
    Public z As Double
    Public x_angle As Double
    Public y_angle As Double
    Public z_angle As Double
    Public x_eye As Double
    Public y_eye As Double
    Public z_eye As Double
    Public matrial As String
    Public blnDraw As Boolean
    Public blnSel As Boolean
    Public ID As Long
    Public Sub New()
        x = 0.0
        y = 0.0
        z = 0.0
        x_angle = 0.0
        y_angle = 0.0
        z_angle = 0.0
        x_eye = 0.0
        y_eye = 0.0
        z_eye = 0.0
        blnDraw = True
        blnSel = False
    End Sub
    Public Sub New(ByVal _x As Double, ByVal _y As Double, ByVal _z As Double)
        x = _x
        y = _y
        z = _z
        blnDraw = True
        blnSel = True
    End Sub
    Public Function getIntx() As Integer
        Return x
    End Function
    Public Function getInty() As Integer
        Return y
    End Function
    Public Function getIntz() As Integer
        Return z
    End Function
    Public Function distTo(ByVal pnt As GeoPoint) As Double
        Dim r As Double = ((x - pnt.x) * (x - pnt.x)) + ((y - pnt.y) * (y - pnt.y) + (z - pnt.z) * (z - pnt.z))
        Return Math.Sqrt(r)
    End Function
End Class
