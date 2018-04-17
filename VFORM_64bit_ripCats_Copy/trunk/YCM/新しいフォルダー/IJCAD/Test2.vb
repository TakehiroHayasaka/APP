Module Test2
    Dim numElms As Integer
    Dim cElms(10) As CElment
    Dim cElmObjs As CObjectArray

    Public Sub 図形保持テスト()
        Call 図形セット()
        Call 図形ゲット()
    End Sub
    Public Sub 図形セット()
        Dim gpS As GeoPoint, gpE As GeoPoint, gpOrg As GeoPoint
        numElms = 2
        gpS = New GeoPoint
        gpE = New GeoPoint
        gpS.setXYZ(100, 200, 300)
        gpE.setXYZ(10, 20, 30)

        cElms(0) = New CElment(ElementType.ElmLine)
        celms(0).StartPoint = gpS
        celms(0).EndPoint = gpE

        gpOrg = New GeoPoint
        gpOrg.setXYZ(50, 60, 70)
        cElms(1) = New CElment(ElementType.ElmCircle)
        celms(1).Origin = gpOrg
        celms(1).Rad = 250.0#

        Dim cElm As CElment, iRet As Long
        cElmObjs = New CObjectArray
        cElm = New CElment(ElementType.ElmLine)
        cElm.StartPoint = gpS
        cElm.EndPoint = gpE
        iRet = cElmObjs.Append(cElm)

        cElm = New CElment(ElementType.ElmCircle)
        cElm.Origin = gpOrg
        cElm.Rad = 250.0#
        iRet = cElmObjs.Append(cElm)

        iRet = cElmObjs.AddLine(gpE, gpS)
        iRet = cElmObjs.AddCircle(gpOrg, 125.0#, 55.5, 65.2)

    End Sub
    Sub 図形ゲット()
        Dim gpS As GeoPoint, gpE As GeoPoint, gpOrg As GeoPoint
        Dim dRad As Double
        Dim i As Integer, elmType As Integer
        Dim cElm As CElment
        If (numElms > 0) Then
            For i = 0 To numElms - 1
                elmType = cElms(i).ElmType
                cElm = cElms(i)
                Select Case (elmType)
                    Case ElementType.ElmLine
                        gpS = cElm.StartPoint
                        gpE = cElm.EndPoint
                    Case ElementType.ElmCircle
                        gpOrg = cElm.Origin
                        dRad = cElm.Rad
                    Case Else
                End Select
            Next
        End If

        If (cElmObjs.Size > 0) Then
            For i = 0 To (cElmObjs.Size - 1)
                cElm = cElmObjs.at(i)
                elmType = cElm.ElmType
                Select Case (elmType)
                    Case ElementType.ElmLine
                        gpS = cElm.StartPoint
                        gpE = cElm.EndPoint
                    Case ElementType.ElmCircle
                        gpOrg = cElm.Origin
                        dRad = cElm.Rad
                    Case Else
                End Select
            Next
        End If
    End Sub
End Module
