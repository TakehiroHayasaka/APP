'「measurepoint3d」テーブル
'	ID			：シーケンシャル番号
'	TID			：ターゲットID（ターゲット毎にユニーク）

'	Type		：＝１：シングルターゲット、＝２：コードターゲット
'	systemlabel	：システムラベル
'	userlabel	：ユーザ定義ラベル
'	currentlabel：カレントラベル（3Dビューに表示するラベル）

'	X	Y	Z	：座標値（無単位）

'	meanerror	：

'	deverror	：

'	flgDisplay	：＝１：表示、＝０：非表示
'	flgLabel	：＝１：代表マーカ（親マーカ）


Public Class CLookPoint
    Public Enum posTypeMode
        Auto = 0    '自動作成された計測点
        User = 1    'ユーザが追加した計測点
        All = 9     '全て
    End Enum
    Public x As Double              '（無単位）

    Public y As Double
    Public z As Double
    Public Real_x As Double         '（座標変換後）

    Public Real_y As Double
    Public Real_z As Double
    Public mid As Long  '--Integer      '「ID」ユニークな番号
    Public tid As Long  '--Integer      '「TID」ターゲット番号（同一番号あり）

    Public blnDraw As Boolean           '=True：表示、=False：非表示
    Public LabelName As String          'ラベル（カレントなラベル名）

    Public sortID As Long  '--Integer   '読込み時のシーケンシャル番号
    Public posType As posTypeMode       '=0：自動作成された計測点、=1：ユーザが追加した点
    Public colorCode As Integer         '色コード番号
    '--ins.start--------------------
    Public flgLabel As Integer  '[flgLabel]＝１：代表マーカ
    Public type As Integer      '[Type]ターゲット種別 =1:シングルターゲット、=2:コードターゲット、=9：ユーザ追加点
    Public mode As Integer      '=0：変更なし、=1:変更(自動側点のラベルを変更)、=2:追加(ユーザ追加点)
    '--ins.end----------------------

    '20170217 baluu add start
    Public createType As Integer '=0 : KaisekiPoint、=1 : UserInputPoint、=2 : SekkeiPoint
    '20170217 baluu add end

    Public cVx As Double        'カメラの方向（平均）

    Public cVy As Double
    Public cVz As Double

    Public Sub New()
        x = 0.0
        y = 0.0
        z = 0.0
        mid = -1
        blnDraw = True
        Real_x = 0.0
        Real_y = 0.0
        Real_z = 0.0
        posType = posTypeMode.Auto
        mode = 0
        flgLabel = 1
        cVx = 0.0 : cVy = 0.0 : cVz = 0.0
        createType = 0 '20170217 baluu add
    End Sub
    Public Sub New(ByVal _x As Double, ByVal _y As Double, ByVal _z As Double)
        x = _x
        y = _y
        z = _z
        Real_x = _x
        Real_y = _y
        Real_z = _z
        blnDraw = True
        posType = posTypeMode.Auto
        createType = 0 '20170217 baluu add
    End Sub

    'Public Sub New(ByVal objNewItem As Sekkei_Keisoku.ZahyoChiItems, ByVal tmpscale As Double)
    '    LabelName = objNewItem.SekkeiLabel1
    '    x = objNewItem.SekkeiTenX / tmpscale
    '    y = objNewItem.SekkeiTenY / tmpscale
    '    z = objNewItem.SekkeiTenZ / tmpscale
    '    Real_x = objNewItem.SekkeiTenX
    '    Real_y = objNewItem.SekkeiTenY
    '    Real_z = objNewItem.SekkeiTenZ
    '    mid = -1
    '    blnDraw = True
    '    posType = posTypeMode.Auto
    '    mode = 0
    '    flgLabel = 1
    '    cVx = 0.0 : cVy = 0.0 : cVz = 0.0
    'End Sub
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
    Public Function toGeopoint() As GeoPoint
        Dim geo As New GeoPoint
        geo.x = x
        geo.y = y
        geo.z = z
        Return geo
    End Function
    Public Function RealToGeopoint() As GeoPoint
        Dim geo As New GeoPoint
        geo.x = Real_x
        geo.y = Real_y
        geo.z = Real_z
        Return geo
    End Function
End Class
