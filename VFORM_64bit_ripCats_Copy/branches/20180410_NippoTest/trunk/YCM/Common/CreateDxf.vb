Public Class Form1

    'ljj add  str 2014/1/20

    'Public Function CreateDxf(ByVal StartPoint As Teigha.Core.OdGePoint3d, ByVal EndPoint As Teigha.Core.OdGePoint3d)


    '    Dim Serv As Teigha.Core.ExSystemServices = New Teigha.Core.ExSystemServices()
    '    Dim _hostApp As MyHostAppServ = New MyHostAppServ()
    '    Teigha.TD.TD_Db.odInitialize(Serv)

    '    Dim pDb As Teigha.TD.OdDbDatabase = _hostApp.createDatabase(True, Teigha.TD.MeasurementValue.kEnglish)

    '    Dim strDxfName As String = "D:\MyProject\RevitAll\testOD\sssbbb2.dxf"

    '    Dim fileType As Teigha.TD.SaveType = Teigha.TD.SaveType.kDwg
    '    fileType = Teigha.TD.SaveType.kDxf


    '    'Dim StartPoint As New Teigha.Core.OdGePoint3d(0, 0, 0)
    '    'Dim EndPoint As New Teigha.Core.OdGePoint3d(10, 10, 0)
    '    addLines(pDb.getModelSpaceId(), 0, 0, StartPoint, EndPoint)

    '    Dim outVer As Teigha.Core.DwgVersion = Teigha.Core.DwgVersion.vAC15

    '    pDb.writeFile(strDxfName, fileType, outVer, True)

    '    Teigha.TD.TD_Db.odUninitialize()

    'End Function


    'Private Function addLines(ByVal btrId As Teigha.TD.OdDbObjectId, ByVal boxRow As Int32, ByVal boxCol As Int32,
    '                          ByVal StartPoint As Teigha.Core.OdGePoint3d, ByVal EndPoint As Teigha.Core.OdGePoint3d)


    '    '/**********************************************************************/
    '    '/* Open the Block Table Record                                        */
    '    '/**********************************************************************/
    '    Dim bBTR As Teigha.TD.OdDbBlockTableRecord = btrId.safeOpenObject(Teigha.TD.OpenMode.kForWrite)


    '    '/**********************************************************************/
    '    '/* Get the origin and size of the box                                 */
    '    '/**********************************************************************/
    '    'Dim point1 As New Teigha.Core.OdGePoint3d(0, 0, 0)
    '    'Dim point2 As New Teigha.Core.OdGePoint3d(10, 10, 0)

    '    Dim pLine As Teigha.TD.OdDbLine
    '    pLine = Teigha.TD.OdDbLine.createObject()

    '    pLine.setDatabaseDefaults(bBTR.database())
    '    bBTR.appendOdDbEntity(pLine)

    '    pLine.setStartPoint(StartPoint)
    '    pLine.setEndPoint(EndPoint)

    'End Function

End Class
'end