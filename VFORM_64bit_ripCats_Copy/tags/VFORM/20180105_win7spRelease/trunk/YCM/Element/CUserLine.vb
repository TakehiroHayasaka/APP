﻿Public Class CUserLine
    Public startPnt As GeoPoint     '始点座標値
    Public endPnt As GeoPoint       '終点座標値
    Public CPID As Long
    Public MID As Long
    Public PointID As Long
    Public blnDraw As Boolean       '=True：表示
    Public binDelete As Boolean
    Public elmType As Long          '=0：ユーザ作成、=1：CAD入力
    Public createType As Integer    '=0 : UserInputLine、=1 : SunpoLine、=2 : SekkeiLine ' 20170208 baluu add

    Public blnDelOnGlwin As Boolean '=0：未削除、    =1：削除済み 'Add Kiryu CATS/2016-a3-20160920
    Public model_pick As UInteger   '描画時の描画IDを保存         'Add Kiryu CATS/2016-a3-20160920

    Public colorCode As Integer     '色コード番号
    'Public lineWidth As Double = 10.0F   '線の幅コード番号（13.1.25）

    Public lineTypeCode As Integer  '線種コード番号
    Public layerName As String      'レイヤ名


    Public Sub New()
        startPnt = New GeoPoint
        endPnt = New GeoPoint
        blnDraw = True
        elmType = 0
        createType = 0 '20170208 baluu add
        colorCode = 8     '色コード番号
        'lineWidth = 10.0F  '線の幅

        lineTypeCode = 1  '線種コード番号
        layerName = "0"   'レイヤ名

        MID = -1
        binDelete = False
        blnDelOnGlwin = False
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
        Return startPnt.GetDistanceTo(endPnt)
    End Function
    Public Function pointOnLine(ByVal pnt As GeoPoint) As Boolean
        Dim dist1 As Double = startPnt.GetDistanceTo(pnt)
        Dim dist2 As Double = endPnt.GetDistanceTo(pnt)
        Dim dblLength As Double = length()
        If Math.Abs((dist1 + dist2) - dblLength) < 0.001 Then
            'If dist1 + dist2 = length() Then
            Return True
        End If
    End Function
    Public Function horizontalLap(ByVal line As CUserLine) As Boolean
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
