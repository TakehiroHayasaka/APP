Public Class CUserEnt
    Public line As CUserLine
    Public circle As Ccircle
    Public type As String
    Public Sub New()
        line = New CUserLine
        circle = New Ccircle
        type = "INITAL"
    End Sub
    Public Sub TransToLine(ByVal valLine As CUserLine)
        line = valLine
        type = "LINE"
    End Sub
    Public Sub TransToCIRCLE(ByVal valCir As Ccircle)
        circle = valCir
        type = "CIRCLE"
    End Sub
    Public Function Copy() As CUserEnt
        Copy = New CUserEnt
        Copy.line = Me.line
        Copy.circle = Me.circle
        Copy.type = Me.type

    End Function
End Class
