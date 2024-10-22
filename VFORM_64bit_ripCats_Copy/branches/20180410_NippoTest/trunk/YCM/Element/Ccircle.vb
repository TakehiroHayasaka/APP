﻿Public Class Ccircle
    Public x_angle As Double
    Public y_angle As Double
    Public z_angle As Double
    Public org As GeoPoint      '円中心点座標値
    Public r As Double          '半径

    '--    Public matrial As String
    Public blnSel As Boolean
    Public mid As Integer
    Public Vec As GeoVector

    Public binDelete As Boolean
    Public blnDraw As Boolean       '=True：表示
    Public elmType As Long          '=0：ユーザ作成、=1：CAD入力
    Public createType As Integer    '=0 : UserInputCircle、=1 : SunpoCircle、=2 : SekkeiCircle ' 20170208 baluu add

    Public colorCode As Integer     '色コード番号
    Public lineTypeCode As Integer  '線種コード番号
    Public layerName As String      'レイヤ名

    Public blnDelOnGlwin As Boolean '=0：未削除、    =1：削除済み 'Add Kiryu CATS/2016-a3-20160920
    Public model_pick As UInteger   '描画時の描画IDを保存         'Add Kiryu CATS/2016-a3-20160920

    Public Sub New()
        x_angle = 0.0
        y_angle = 0.0
        z_angle = 0.0
        org = New GeoPoint
        Vec = New GeoVector
        r = 0.0
        blnDraw = True
        blnSel = False
        binDelete = False
        mid = -1
        elmType = 0
        createType = 0 ' 20170208 baluu add
        colorCode = 8     '色コード番号
        lineTypeCode = 1  '線種コード番号
        layerName = "0"   'レイヤ名

    End Sub
End Class
