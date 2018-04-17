'Option Strict Off
Option Explicit On
Public Class GeoLine


    '==============================================================================
    Public GPstartpoint As GeoPoint
    Public GPendpoint As GeoPoint
    Public Lgppoint As GeoPoint
    Public Lvector As GeoVector

    Public Sub New()
        GPstartpoint = New GeoPoint
        GPendpoint = New GeoPoint
        Lgppoint = New GeoPoint
        Lvector = New GeoVector
    End Sub

End Class