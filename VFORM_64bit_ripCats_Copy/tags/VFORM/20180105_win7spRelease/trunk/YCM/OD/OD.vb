


Public Class ODEntity
    Implements ICADEntity

    Protected Sub New()
    End Sub

    Public Shared Function createObject(ByVal objID As Teigha.TD.OdDbObjectId) As ODEntity
        Dim obj As Teigha.TD.OdDbObject = objID.safeOpenObject(Teigha.TD.OpenMode.kForWrite)

        Dim ent As ODEntity
        If TypeOf (obj) Is Teigha.TD.OdDbCircle Then
            ent = New ODCircle(obj)
        Else
            ent = New ODEntity
        End If

        Return ent
    End Function

    Public Overridable Sub Move(ByRef a As GeoPoint, ByRef origin As GeoPoint) Implements ICADEntity.Move
    End Sub

    Public Overridable Sub Rotate3D(ByRef origin As GeoPoint, ByRef dir As GeoPoint, ByVal angle As Double) Implements ICADEntity.Rotate3D
    End Sub

    Public Sub Update() Implements ICADEntity.Update
    End Sub

    Public Shared Function CreateVectorFr(ByRef pt As GeoPoint) As Teigha.Core.OdGeVector3d
        Dim iPos As New Teigha.Core.OdGeVector3d(pt.x, pt.y, pt.z)
        Return iPos
    End Function
End Class

Public Class ODCircle
    Inherits ODEntity

    Private m_obj As Teigha.TD.OdDbCircle

    Public Sub New(ByVal obj As Teigha.TD.OdDbCircle)
        m_obj = obj
    End Sub

    Public Overrides Sub Move(ByRef a As GeoPoint, ByRef origin As GeoPoint)
        Dim pt1 As Teigha.Core.OdGePoint3d = ODOperator.CreatePointFr(a)
        Dim pt2 As Teigha.Core.OdGePoint3d = ODOperator.CreatePointFr(origin)

        Dim newCenter As Teigha.Core.OdGePoint3d = m_obj.center()
        newCenter += pt1 - pt2

        m_obj.setCenter(newCenter)
    End Sub

    Public Overrides Sub Rotate3D(ByRef origin As GeoPoint, ByRef dir As GeoPoint, ByVal angle As Double)
        Dim ptOrigin As Teigha.Core.OdGePoint3d = ODOperator.CreatePointFr(origin)
        Dim vecDir As Teigha.Core.OdGeVector3d = CreateVectorFr(dir)

        Dim newCenter As Teigha.Core.OdGePoint3d = m_obj.center()
        If True Then
            Dim vec1 As Teigha.Core.OdGeVector3d = ptOrigin - newCenter
            vec1 = vec1.rotateBy(angle, vecDir)
            newCenter = ptOrigin - vec1
        End If

        Dim newNormal As Teigha.Core.OdGeVector3d = m_obj.normal()
        newNormal = newNormal.rotateBy(angle, vecDir)

        m_obj.setCenter(newCenter)
        m_obj.setNormal(newNormal)
    End Sub
End Class


Public Class ODOperator
    Implements ICADOperator

    Private m_db As Teigha.TD.OdDbDatabase
    Public Sub New(ByRef db As Teigha.TD.OdDbDatabase)
        m_db = db
    End Sub

    Public Sub CreateLayer(ByVal strLayerName As String) Implements ICADOperator.CreateLayer
        Dim layerTable As Teigha.TD.OdDbLayerTable = _
            m_db.getLayerTableId.safeOpenObject(Teigha.TD.OpenMode.kForWrite)

        ' Create layer if it doesn't exist
        If Not layerTable.has(strLayerName) Then
            Dim layer As Teigha.TD.OdDbLayerTableRecord = Teigha.TD.OdDbLayerTableRecord.createObject()
            layer.setName(strLayerName)
            layerTable.add(layer)
        End If
    End Sub

    Public Sub CreateLayerEntSet(ByVal entset As Sys_Setting.EntSetting) Implements ICADOperator.CreateLayerEntSet
        Dim layerTable As Teigha.TD.OdDbLayerTable = _
            m_db.getLayerTableId.safeOpenObject(Teigha.TD.OpenMode.kForWrite)

        Dim layer As Teigha.TD.OdDbLayerTableRecord

        ' Create layer if it doesn't exist
        If Not layerTable.has(entset.layerName) Then
            layer = Teigha.TD.OdDbLayerTableRecord.createObject()
            layer.setName(entset.layerName)
            layerTable.add(layer)
        Else
            layer = layerTable.getAt(entset.layerName).safeOpenObject(Teigha.TD.OpenMode.kForWrite)
        End If

        ' レイヤの色を設定

        Dim color As New Teigha.TD.OdCmColor()
        color.setRGB(entset.color.dbl_red * 255, _
                     entset.color.dbl_green * 255, _
                     entset.color.dbl_blue * 255)
        layer.setColor(color)
    End Sub

    Public Sub AddPoint(ByRef cpos As CLookPoint, ByVal strLayerName As String) Implements ICADOperator.AddPoint
        Dim bBTR As Teigha.TD.OdDbBlockTableRecord = _
            m_db.getModelSpaceId.safeOpenObject(Teigha.TD.OpenMode.kForWrite)

        Dim pt As Teigha.TD.OdDbPoint = Teigha.TD.OdDbPoint.createObject()
        pt.setDatabaseDefaults(m_db)
        pt.setPosition(CreatePointFr(cpos))
        pt.setLayer(strLayerName)

        bBTR.appendOdDbEntity(pt)
    End Sub

    Public Shared Function CreatePointFr(ByRef pt As GeoPoint) As Teigha.Core.OdGePoint3d
        Dim iPos As New Teigha.Core.OdGePoint3d(pt.x, pt.y, pt.z)
        Return iPos
    End Function
    Private Shared Function CreatePointFr(ByRef pt As CLookPoint) As Teigha.Core.OdGePoint3d
        Dim iPos As New Teigha.Core.OdGePoint3d(pt.x, pt.y, pt.z)
        Return iPos
    End Function

    Public Sub AddLine(ByRef pRS As GeoPoint, ByRef pRE As GeoPoint, ByVal strLayerName As String, ByVal Mcolor As Sys_Setting.ModelColor) Implements ICADOperator.AddLine
        ' Open the Block Table Record
        Dim bBTR As Teigha.TD.OdDbBlockTableRecord = _
            m_db.getModelSpaceId.safeOpenObject(Teigha.TD.OpenMode.kForWrite)

        Dim pLine As Teigha.TD.OdDbLine = Teigha.TD.OdDbLine.createObject()
        pLine.setDatabaseDefaults(m_db)
        pLine.setStartPoint(CreatePointFr(pRS))
        pLine.setEndPoint(CreatePointFr(pRE))
        pLine.setLayer(strLayerName)

        Dim TDcolor As New Teigha.TD.OdCmColor()
        TDcolor.setRGB(Mcolor.dbl_red * 255, _
                     Mcolor.dbl_green * 255, _
                     Mcolor.dbl_blue * 255)
        pLine.setColor(TDcolor)
        'pLine.setLinetype(1)

        bBTR.appendOdDbEntity(pLine)
    End Sub

    Public Sub AddText(ByVal text As String, ByRef loc As GeoPoint, ByVal dH As Double, ByVal strLayerName As String) Implements ICADOperator.AddText
        Dim bBTR As Teigha.TD.OdDbBlockTableRecord = _
            m_db.getModelSpaceId.safeOpenObject(Teigha.TD.OpenMode.kForWrite)

        Dim odText As Teigha.TD.OdDbText = Teigha.TD.OdDbText.createObject()
        odText.setDatabaseDefaults(m_db)
        odText.setTextString(text)
        odText.setHorizontalMode(Teigha.TD.TextHorzMode.kTextMid)
        odText.setVerticalMode(Teigha.TD.TextVertMode.kTextVertMid)
        odText.setPosition(CreatePointFr(loc))
        odText.setAlignmentPoint(CreatePointFr(loc))
        odText.setHeight(dH)
        'odText.setTextStyle()
        odText.setLayer(strLayerName)

        bBTR.appendOdDbEntity(odText)
    End Sub
' 2014-5-30 str by Ljj
    Public Sub AddText_Information(ByVal text As String, ByRef loc As GeoPoint, ByVal dH As Double, ByVal strLayerName As String) Implements ICADOperator.AddText_Information
        Dim bBTR As Teigha.TD.OdDbBlockTableRecord = _
            m_db.getModelSpaceId.safeOpenObject(Teigha.TD.OpenMode.kForWrite)
        Dim odText As Teigha.TD.OdDbText = Teigha.TD.OdDbText.createObject()
        odText.setDatabaseDefaults(m_db)
        odText.setTextString(text)
        'odText.setHorizontalMode(Teigha.TD.TextHorzMode.kTextMid)
        'odText.setVerticalMode(Teigha.TD.TextVertMode.kTextVertMid)
        odText.setPosition(CreatePointFr(loc))
        odText.setAlignmentPoint(CreatePointFr(loc))
        odText.setHeight(dH)
        'odText.setTextStyle()
        odText.setLayer(strLayerName)

        bBTR.appendOdDbEntity(odText)
    End Sub
    ' 2014-5-30 end by Ljj 

    Public Function AddCircle(ByRef loc As GeoPoint, ByVal radius As Double, ByVal Nor As GeoVector, ByVal strLayerName As String, ByVal MColor As Sys_Setting.ModelColor) As ICADEntity Implements ICADOperator.AddCircle
        Dim bBTR As Teigha.TD.OdDbBlockTableRecord = _
            m_db.getModelSpaceId.safeOpenObject(Teigha.TD.OpenMode.kForWrite)

        Dim odCircle As Teigha.TD.OdDbCircle = Teigha.TD.OdDbCircle.createObject()
        odCircle.setDatabaseDefaults(m_db)
        odCircle.setCenter(CreatePointFr(loc))
        odCircle.setRadius(radius)
        odCircle.setLayer(strLayerName)
        'odCircle.normal.x = Nor.x
        'odCircle.normal.y = Nor.y
        'odCircle.normal.z = Nor.z
        odCircle.setNormal(New Teigha.Core.OdGeVector3d(Nor.x, Nor.y, Nor.z))

        Dim TDcolor As New Teigha.TD.OdCmColor()
        TDcolor.setRGB(MColor.dbl_red * 255, _
                     MColor.dbl_green * 255, _
                     MColor.dbl_blue * 255)
        odCircle.setColor(TDcolor)

        Dim geVector As Teigha.Core.OdGeVector3d = odCircle.normal()

        Dim objID As Teigha.TD.OdDbObjectId = bBTR.appendOdDbEntity(odCircle)

        Dim ent As ODEntity = ODEntity.createObject(objID)
        Return ent
    End Function
End Class