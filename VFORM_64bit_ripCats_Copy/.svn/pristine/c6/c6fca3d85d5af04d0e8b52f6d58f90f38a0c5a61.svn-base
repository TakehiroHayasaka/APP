Public Class CLineDim
    Inherits CShape
    Public leftPoint As New CPoint
    Public rightPoint As New CPoint
    Public leftLegLength As Double
    Public rightLegLength As Double
    Public circleRadius As Double

    Public drawLeftLeg As Boolean = True
    Public drawRightLeg As Boolean = True
    Public drawLeftCircle As Boolean = True
    Public drawRightCircle As Boolean = True
    Public isUpDim As Boolean = True
    Public setToReversal As Boolean = False
    'add-s リカ 2010/11/12
    Public setTotext As Boolean = False
    'add-e リカ 2010/11/12
    Public text As String

    Dim bodyLine As New CLine
    Dim leftLeg As New CLine
    Dim rightLeg As New CLine
    Dim leftCircle As New CCircle
    Dim rightCircle As New CCircle
    'Dim dimMarkStr As New CMarkString
    Public dimStrToLineMargin As Double = 0


    Public linePen As System.Drawing.Pen
    Public Shared s_gp As System.Drawing.Graphics = Nothing
    Public stringBrush As System.Drawing.Brush = Brushes.Black

    'Public Sub drawLine()
    '    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
    '    If Not CLine.s_gp Is Nothing Then
    '        drawLine(CLine.s_gp)
    '    End If
    'End Sub
    Public Sub drawLine(ByRef gp As System.Drawing.Graphics)
        update()

        'bodyLine.drawLine()

        'If drawLeftLeg = True Then
        '    leftLeg.drawLine()
        'End If

        'If drawRightLeg = True Then
        '    rightLeg.drawLine()
        'End If

        'If drawLeftCircle = True Then
        '    leftCircle.drawCircle()
        'End If

        'If drawRightCircle = True Then
        '    rightCircle.drawCircle()
        'End If
        'dimMarkStr.drawString()
    End Sub
    Public Sub update()
        If enableHighScale = False Then
            highScale = scale
        End If

        'main line
        bodyLine.startPnt.x = leftPoint.x * scale
        bodyLine.startPnt.y = leftPoint.y * highScale
        bodyLine.endPnt.x = rightPoint.x * scale
        bodyLine.endPnt.y = rightPoint.y * highScale
        bodyLine.linePen = linePen

        'left line
        Dim vec As CVector = rightPoint - leftPoint
        vec.normalize()
        If setToReversal = True Then
            vec.rotateBy(-CShape.c_pi / 2.0)
        Else
            vec.rotateBy(CShape.c_pi / 2.0)
        End If


        Dim leftLegPnt As CPoint = leftPoint + vec * leftLegLength
        leftLeg.startPnt.x = leftPoint.x * scale
        leftLeg.startPnt.y = leftPoint.y * highScale
        leftLeg.endPnt.x = leftLegPnt.x * scale
        leftLeg.endPnt.y = leftLegPnt.y * highScale
        leftLeg.linePen = linePen

        'right line
        vec = leftPoint - rightPoint
        vec.normalize()
        If setToReversal = True Then
            vec.rotateBy(CShape.c_pi / 2.0)
        Else
            vec.rotateBy(-CShape.c_pi / 2.0)
        End If


        Dim rightLegPnt As CPoint = rightPoint + vec * rightLegLength
        rightLeg.startPnt.x = rightPoint.x * scale
        rightLeg.startPnt.y = rightPoint.y * highScale
        rightLeg.endPnt.x = rightLegPnt.x * scale
        rightLeg.endPnt.y = rightLegPnt.y * highScale
        rightLeg.linePen = linePen

        'left point
        leftCircle.centerPoint.x = leftPoint.x * scale
        leftCircle.centerPoint.y = leftPoint.y * highScale
        leftCircle.radius = circleRadius * scale
        leftCircle.linePen = linePen
        leftCircle.fillBrush = stringBrush
        leftCircle.all = True

        'right point
        rightCircle.centerPoint.x = rightPoint.x * scale
        rightCircle.centerPoint.y = rightPoint.y * highScale
        rightCircle.radius = circleRadius * scale
        rightCircle.linePen = linePen
        rightCircle.fillBrush = stringBrush
        rightCircle.all = True

        'dimMarkStr.height = 15
        'dimMarkStr.width = 6
        'dimMarkStr.str = text
        'dimMarkStr.gc = s_gp
        'mod-s リカ 2010/11/12
        'If setTotext = False Then
        '    dimMarkStr.startPoint.x = rightLeg.startPnt.x - (rightLeg.startPnt.distTo(leftLeg.startPnt) / 2.0) - (dimMarkStr.getStrPixWidth() / 2.0)
        'Else
        '    dimMarkStr.startPoint.x = leftLeg.startPnt.x - dimStrToLineMargin + dimMarkStr.width / 2.0
        'End If
        ''mod-e リカ 2010/11/12

        'dimMarkStr.stringBrush = stringBrush


        'dimMarkStr.scale = 1.0
        'dimMarkStr.highScale = 1.0
        'dimMarkStr.needRotate = False
        ''mod-s リカ 2010/11/12
        'If setTotext = False Then
        '    dimMarkStr.startPoint.y = leftLeg.startPnt.y - dimStrToLineMargin - dimMarkStr.getStrPixHeight()
        'Else
        '    dimMarkStr.startPoint.y = rightLeg.startPnt.y - (rightLeg.startPnt.distTo(leftLeg.startPnt) / 2.0) - (dimMarkStr.getStrPixHeight() / 2.0)
        'End If
        'mod-e リカ 2010/11/12
            'dimMarkStr.angle = 0

    End Sub
End Class
