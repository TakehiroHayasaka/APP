﻿Imports Microsoft.VisualBasic.FileIO
Imports FBMlib
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports HalconDotNet



Public Enum SunpoKeisanHouhou
    PointToPoint = 1
    LineToPoint = 2
    MidPointToMidPoint = 3
    CCenterToCCenter = 4
    CircleToCircle_Max = 5
    CircleToCircle_Min = 6
    CircleRadius = 7
    PgunToPgun = 8 '後でPointToPointに置き換える
    PlaneToPoint = 9
    PointToPoint_XYZsabun = 10
    Toori = 11
    MainRaleCamber = 12
    PgunToPgun_no_XYZsabun = 13 '二つの点群の指定された成分においての差の最大値を得る
    CreateCircle = 14
    CreateCircle_On_OXY = 15  'OXY
    CircleDiameter = 16
    KyoriPlusRaduis = 17
    Point = 18
End Enum

Public Enum TengunBunruiShiyouHouhou
    OnePoint = 1
    MultiPoint = 2
    CircleCenter = 3
    MidPoint = 4
    MeanPoint = 5
    LineToPointNoFoot = 6
    LineToPointNoPlane = 7
    NormalVectorByThreePoint = 8   '20141022追加　SUURI
End Enum
Public Enum TengunSyoriHouhou
    OnePoint = 0
    Sorting = 1
    BaseTenYoriDaisyo = 2
    HaninSitei = 3
    Bunrui = 4
    OffsetVector = 5
    SonoMama = 6
    SortingByCTID = 7
    SingleToCT = 8
    NinniST = 9
    NinniCT = 10

End Enum

Public Class CToffset

    Public CT_ID As Integer
    Public flg_Use As Boolean
    Public offset_val As GeoVector
    Public kubun1 As Integer
    Public kubun2 As Integer
    Public info As String
    Public CT_No As Integer

    Public Sub New()
        CT_ID = -1
        flg_Use = False
        offset_val = New GeoVector
        kubun1 = -1
        kubun2 = -1
        info = ""
    End Sub
    Public Sub New(ByVal strFields() As String)
        CT_ID = strFields(0)
        flg_Use = CBool(strFields(1))
        offset_val = New GeoVector
        offset_val.setXYZ(CDbl(strFields(2)), CDbl(strFields(3)), CDbl(strFields(4)))
        kubun1 = CInt(strFields(5))
        kubun2 = CInt(strFields(6))
        info = strFields(7)
    End Sub

    '2013/05/15 SUZUKI ADD 
    Public Sub GetDataByReader(ByVal IDR As IDataReader)

        CT_ID = CInt(IDR.GetValue(0).ToString.Trim)
        flg_Use = CBool(IDR.GetValue(1).ToString.Trim)
        offset_val = New GeoVector
        offset_val.setXYZ(CDbl(IDR.GetValue(2)),
                          CDbl(IDR.GetValue(3)),
                          CDbl(IDR.GetValue(4)))
        kubun1 = CInt(IDR.GetValue(5).ToString.Trim)
        kubun2 = CInt(IDR.GetValue(6).ToString.Trim)
        info = IDR.GetValue(7).ToString.Trim

    End Sub

    Public Sub CsvOut(ByVal strFileName As String, ByRef system_dbclass As FBMlib.CDBOperateOLE)


        Dim strSql As String = "SELECT CT_NO,flg_Use,m_x,m_y,m_z,Kubun1,Kubun2,Info FROM CT_Bunrui WHERE TypeID = " & CommonTypeID

        My.Computer.FileSystem.WriteAllText(strFileName, "CT_NO,flg_Use,m_x,m_y,m_z,Kubun1,Kubun2,Info" + vbNewLine, True)
        Dim IDR As IDataReader = system_dbclass.DoSelect(strSql)
        Dim strKekka As String = ""
        If Not IDR Is Nothing Then
            Do While IDR.Read
                'Dim CToffsetD As New CToffset
                Me.GetDataByReader(IDR)
                strKekka = CT_ID & "," & flg_Use & "," & offset_val.x() & "," & offset_val.y() & "," & offset_val.z() & "," & kubun1 & "," & kubun2 & "," & info & vbNewLine
                My.Computer.FileSystem.WriteAllText(strFileName, strKekka, True)

            Loop
            IDR.Close()
        End If



        'Dim strKekka As String = ""
        'strKekka = CT_ID & "," & flg_Use & "," & offset_val.x() & "," & offset_val.y() & "," & offset_val.z() & "," & kubun1 & "," & kubun2 & "," & info & "," & CT_No & vbNewLine

        ''My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & strFileName, strKekka, True)
        'My.Computer.FileSystem.WriteAllText(strFileName, strKekka, True)


    End Sub

    Public Sub CsvInput(ByVal strFileName As String, ByRef dbclass As FBMlib.CDBOperateOLE, ByVal TypeID As Integer)
        'For Each CTOS As CToffset In arrCToffsets
        '    CTOS = New CToffset
        'Next
        'Dim filename As String = My.Application.Info.DirectoryPath & "\" & strFileName
        Dim filename As String = strFileName
        Dim fields As String()
        Dim delimiter As String = ","
        Using parser As New TextFieldParser(filename, System.Text.Encoding.Default)
            parser.SetDelimiters(delimiter)
            fields = parser.ReadFields() 'ヘッダの読み飛ばし
            While Not parser.EndOfData
                ' Read in the fields for the current line
                fields = parser.ReadFields()
                ' Add code here to use data in fields variable.
                If IsNumeric(fields(0)) Then
                    Dim objCToffset As New CToffset(fields)

                    Dim strSql As String = "INSERT INTO CT_Bunrui(CT_NO,flg_Use,m_x,m_y,m_z,Kubun1,Kubun2,TypeID,Info) " & _
               "VALUES(" & objCToffset.CT_ID & "," & CInt(objCToffset.flg_Use) * -1 & "," & objCToffset.offset_val.x & "," & objCToffset.offset_val.y & "," & objCToffset.offset_val.z & "," & objCToffset.kubun1 & "," & objCToffset.kubun2 & "," & TypeID & ",'" & objCToffset.info & "')"
                    dbclass.ExecuteSQL(strSql)


                End If
            End While
        End Using

    End Sub

End Class



Public Class CT_ScaleSettingData
    Public CT_ID1 As Integer
    Public CT_ID2 As Integer
    Public CT_kan_dist As Double

    Public CT1 As FBMlib.Common3DCodedTarget
    Public CT2 As FBMlib.Common3DCodedTarget

    Public Sub New(ByVal ID1 As Integer, ByVal ID2 As Integer, ByVal Dist As Double)
        CT_ID1 = ID1
        CT_ID2 = ID2
        CT_kan_dist = Dist
    End Sub

    '2013/05/15 SUZUKI ADD 
    Public Sub New()

    End Sub

    '2013/05/15 SUZUKI ADD 
    Public Sub GetDataByReader(ByVal IDR As IDataReader)

        CT_ID1 = CInt(IDR.GetValue(0).ToString.Trim)
        CT_ID2 = CInt(IDR.GetValue(1).ToString.Trim)
        CT_kan_dist = CDbl(IDR.GetValue(2))

    End Sub

End Class
Public Class CT_Data
    Public CT_dat As Common3DCodedTarget
    Public SekDat As Common3DCodedTarget
    Public CToffsetval As CToffset
    Public CToffsetResult As GeoPoint

    Public Sub New()
        CT_dat = New Common3DCodedTarget
        SekDat = New Common3DCodedTarget
        CToffsetval = New CToffset
        CToffsetResult = New GeoPoint
    End Sub
    Public Sub CtDatToSekDat(ByVal objSek_Kei As clsSekkeiKeisokuTaiyo)
        'SekDat.lstP3d.Add(New Point3D(0, 0, 0))
        Dim tmpCLp As CLookPoint = getDrawPointByTID(CT_dat.PID)
        If (Not tmpCLp Is Nothing) And (Not objSek_Kei Is Nothing) Then
            SekDat = New Common3DCodedTarget
            SekDat.lstP3d = New List(Of Point3D)
            Dim dX, dY, dZ As Double
            dX = objSek_Kei.SekkeiTenX / ScaleToMM
            dY = objSek_Kei.SekkeiTenY / ScaleToMM
            dZ = objSek_Kei.SekkeiTenZ / ScaleToMM
            SekDat.lstP3d.Add(New Point3D(New HTuple(dX), New HTuple(dY), New HTuple(dZ)))

            SekDat.lstRealP3d = New List(Of Point3D)
            dX = objSek_Kei.SekkeiTenX
            dY = objSek_Kei.SekkeiTenY
            dZ = objSek_Kei.SekkeiTenZ
            SekDat.lstRealP3d.Add(New Point3D(New HTuple(dX), New HTuple(dY), New HTuple(dZ)))

        End If

    End Sub




End Class

Public Class CT_CoordSettingData
    Public Property CoordSetID As Integer
    Public Property CT_GenID As Integer
    Public Property CT_XID As Integer
    Public Property CT_YID As Integer
    Public Property CT_ZID As Integer
    Public Property XYorXZorYZ As Integer
    Public Property CT_ID1 As Integer
    Public Property CT_ID2 As Integer
    Public Property CT_ID3 As Integer
    Public Property CT_Active As Integer

    'Public CT_Gen As Common3DCodedTarget
    'Public CT_X As Common3DCodedTarget
    'Public CT_Y As Common3DCodedTarget
    'Public CT_Z As Common3DCodedTarget

    'Public CT1 As Common3DCodedTarget
    'Public CT2 As Common3DCodedTarget
    'Public CT3 As Common3DCodedTarget

    Public Sub New(ByVal Fields() As String, arrCT_Data() As CT_Data)
        CoordSetID = 0
        CT_GenID = 0
        CT_XID = 0
        CT_YID = 0
        CT_ZID = 0
        CT_XID = 0
        CT_YID = 0
        CT_ZID = 0
        XYorXZorYZ = 0
        CT_ID1 = 0
        CT_ID2 = 0
        CT_ID3 = 0
        CT_Active = 0
        Try
            CoordSetID = Fields(0)
            CT_GenID = Fields(1)
            CT_XID = Fields(2)
            CT_YID = Fields(3)
            CT_ZID = Fields(4)
            XYorXZorYZ = Fields(5)
            CT_ID1 = Fields(6)
            CT_ID2 = Fields(7)
            CT_ID3 = Fields(8)
            CT_Active = Fields(9)
            'If CT_GenID > 0 Then
            '    CT_Gen = arrCT_Data(CT_GenID).CT_dat
            'End If
            'If CT_XID > 0 Then
            '    CT_X = arrCT_Data(CT_XID).CT_dat
            'End If
            'If CT_YID > 0 Then
            '    CT_Y = arrCT_Data(CT_YID).CT_dat
            'End If
            'If CT_ZID > 0 Then
            '    CT_Z = arrCT_Data(CT_ZID).CT_dat
            'End If
            'If CT_ID1 > 0 Then
            '    CT1 = arrCT_Data(CT_ID1).CT_dat
            'End If
            'If CT_ID2 > 0 Then
            '    CT2 = arrCT_Data(CT_ID2).CT_dat
            'End If

            'If CT_ID3 > 0 Then
            '    CT3 = arrCT_Data(CT_ID3).CT_dat
            'End If
        Catch ex As Exception

        End Try


    End Sub

    '2013/05/15 SUZUKI ADD 
    Public Sub New()

    End Sub

    '2013/05/15 SUZUKI ADD 
    Public Sub GetDataByReader(ByVal IDR As IDataReader)

        CoordSetID = CInt(IDR.GetValue(0).ToString.Trim)
        CT_GenID = CInt(IDR.GetValue(1).ToString.Trim)
        CT_XID = CInt(IDR.GetValue(2).ToString.Trim)
        CT_YID = CInt(IDR.GetValue(3).ToString.Trim)
        CT_ZID = CInt(IDR.GetValue(4).ToString.Trim)
        XYorXZorYZ = CInt(IDR.GetValue(5).ToString.Trim)
        CT_ID1 = CInt(IDR.GetValue(6).ToString.Trim)
        CT_ID2 = CInt(IDR.GetValue(7).ToString.Trim)
        CT_ID3 = CInt(IDR.GetValue(8).ToString.Trim)
        CT_Active = CInt(IDR.GetValue(9).ToString.Trim)

    End Sub

    '2013/05/15 SUZUKI ADD 
    Public Sub GetDataByReader(ByVal IDR As IDataReader, ByVal arrCT_Data() As CT_Data)

        GetDataByReader(IDR)

        'Try
        '    If CT_GenID > 0 Then
        '        CT_Gen = arrCT_Data(CT_GenID).CT_dat
        '    End If
        '    If CT_XID > 0 Then
        '        CT_X = arrCT_Data(CT_XID).CT_dat
        '    End If
        '    If CT_YID > 0 Then
        '        CT_Y = arrCT_Data(CT_YID).CT_dat
        '    End If
        '    If CT_ZID > 0 Then
        '        CT_Z = arrCT_Data(CT_ZID).CT_dat
        '    End If
        '    If CT_ID1 > 0 Then
        '        CT1 = arrCT_Data(CT_ID1).CT_dat
        '    End If
        '    If CT_ID2 > 0 Then
        '        CT2 = arrCT_Data(CT_ID2).CT_dat
        '    End If
        '    If CT_ID3 > 0 Then
        '        CT3 = arrCT_Data(CT_ID3).CT_dat
        '    End If

        'Catch ex As Exception

        'End Try

    End Sub

    Public Sub CsvOut(ByVal strFileName As String, ByRef system_dbclass As FBMlib.CDBOperateOLE)

        Dim strSql As String = "SELECT * FROM CT_CoordSetting WHERE TypeID = " & CommonTypeID & " ORDER BY ID"

        My.Computer.FileSystem.WriteAllText(strFileName, "ID,CT_GenID,CT_XID,CT_YID,CT_ZID,XYorXZorYZ,CT_ID1,CT_ID2,CT_ID3,CT_Active" + vbNewLine, True)
        Dim IDR As IDataReader = system_dbclass.DoSelect(strSql)
        Dim strKekka As String = ""

        If Not IDR Is Nothing Then
            Do While IDR.Read
                'Dim CToffsetD As New CToffset
                Me.GetDataByReader(IDR)
                strKekka = CoordSetID & "," & CT_GenID & "," & CT_XID & "," & CT_YID & "," & CT_ZID & "," & XYorXZorYZ & "," & CT_ID1 & "," & CT_ID2 & "," & CT_ID3 & "," & CT_Active & vbNewLine     'CoordSetID & "," &
                My.Computer.FileSystem.WriteAllText(strFileName, strKekka, True)
            Loop
            IDR.Close()
        End If

        'Dim strKekka As String = ""
        'strKekka = CoordSetID & "," & CT_GenID & "," & CT_XID & "," & CT_YID & "," & CT_ZID & "," & XYorXZorYZ & "," & CT_ID1 & "," & CT_ID2 & "," & CT_ID3 & "," & CT_Active & vbNewLine

        ''My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & strFileName, strKekka, True)
        'My.Computer.FileSystem.WriteAllText(strFileName, strKekka, True)

    End Sub

    Public Sub CsvInput(ByVal strFileName As String, ByRef dbclass As FBMlib.CDBOperateOLE, ByVal TypeID As Integer)
        'Dim filename As String = My.Application.Info.DirectoryPath & "\" & strFileName
        Dim filename As String = strFileName
        Dim fields As String()
        Dim delimiter As String = ","
        Dim i As Integer = 0
        Using parser As New TextFieldParser(filename, System.Text.Encoding.Default)
            parser.SetDelimiters(delimiter)
            fields = parser.ReadFields() 'ヘッダの読み飛ばし

            While Not parser.EndOfData
                ' Read in the fields for the current line
                fields = parser.ReadFields()
                ' Add code here to use data in fields variable.
                If IsNumeric(fields(0)) Then
                    Dim tmp As New CT_CoordSettingData(fields, Nothing)
                    Dim strSql As String = "INSERT INTO CT_CoordSetting(CT_GenID,CT_XID,CT_YID,CT_ZID,XYorXZorYZ,CT_ID1,CT_ID2,CT_ID3,TypeID,CT_Active) " & _
               "VALUES(" & tmp.CT_GenID & "," & tmp.CT_XID & "," & tmp.CT_YID & "," & tmp.CT_ZID & "," & tmp.XYorXZorYZ & "," & tmp.CT_ID1 & "," & tmp.CT_ID2 & "," & tmp.CT_ID3 & "," & TypeID & "," & tmp.CT_Active & ")"
                    dbclass.ExecuteSQL(strSql)


                End If
            End While
        End Using

    End Sub


End Class

Public Class CT_SunpoSetting
    Public Property SunpoID As Integer
    Public Property SunpoMark As String
    Public Property SunpoName As String
    Public Property SunpoCalcHouhou As Integer
    Public Property CT_ID1 As Integer
    Public Property CT_ID2 As Integer
    Public Property CT_ID3 As Integer
    Public Property seibunXYZ As Integer
    Public Property CT_Active As Integer
    Public Property SunpoVal As Double
    Public Property GunID1 As Integer
    Public Property GunID2 As Integer
    Public Property GunID3 As Integer
    Public Property TypeID As Integer

    Public Sub New()
        _SunpoID = 0
        _SunpoMark = ""
        _SunpoName = ""
        _CT_ID1 = 0
        _CT_ID2 = 0
        _CT_ID3 = 0
        _SunpoCalcHouhou = 0
        _seibunXYZ = 0
        _CT_Active = 0
        _GunID1 = 0
        _GunID2 = 0
        _GunID3 = 0
        _TypeID = 0
    End Sub
    Public Sub New(ByVal Fields() As String)
        _SunpoID = 0
        _SunpoMark = ""
        _SunpoName = ""
        _CT_ID1 = 0
        _CT_ID2 = 0
        _CT_ID3 = 0
        _SunpoCalcHouhou = 0
        _seibunXYZ = 0
        _CT_Active = 0
        _GunID1 = 0
        _GunID2 = 0
        _GunID3 = 0
        _TypeID = 0
        Try
            _SunpoID = CInt(Fields(0))
            _SunpoMark = Fields(1)
            _SunpoName = Fields(2)
            _SunpoCalcHouhou = Fields(3)
            _GunID1 = Fields(4)
            _GunID2 = Fields(5)
            _GunID3 = Fields(6)
            _CT_ID1 = Fields(7)
            _CT_ID2 = Fields(8)
            _CT_ID3 = Fields(9)
            _seibunXYZ = Fields(10)
            _CT_Active = Fields(11)
            _TypeID = Fields(12)
        Catch ex As Exception

        End Try
    End Sub

    Public Function CalcSunpoVal() As Integer
        Select Case SunpoCalcHouhou
            Case SunpoKeisanHouhou.LineToPoint
                SunpoVal = GetDist(CT_ID1, CT_ID2, CT_ID3, seibunXYZ)
            Case SunpoKeisanHouhou.PointToPoint
                SunpoVal = GetDist(CT_ID1, CT_ID2, seibunXYZ)
        End Select
    End Function

End Class

'Public Class TenGunTeigi
'    Public Property TypeID As Integer
'    Public Property TenGunID As Integer
'    Public Tengun() As CT_Data
'    Public Property MotoGunID As Integer
'    Private _MotoGun() As CT_Data

'    Public ReadOnly Property MotoGun() As CT_Data()
'        Get
'            If MotoGunID = 0 Then
'                Return arrCTData
'            Else
'                Return lstTengun.Item(MotoGunID - 1).Tengun
'            End If
'        End Get
'    End Property

'    Public Property SyoriHouhouID As TengunSyoriHouhou
'    Public Property SortingHouhou As SortMethod
'    Public Property objXYZ As XYZseibun
'    Public Property BasePointID As Integer
'    Public Property objDaisyo As DaiSyou
'    Public Property Index1 As Integer
'    Public Property Index2 As Integer
'    Public Property Bunrui1 As Integer
'    Public Property Bunrui2 As Integer
'    Public Property flgBunrui As TengunBunruiShiyouHouhou
'    Public Property flgOnlyOne As Boolean
'    Public Property strName As String
'    Public Property strBikou As String
'    Public Sub New()
'        TypeID = -1
'        TenGunID = -1
'        MotoGunID = -1
'        SyoriHouhouID = -1
'        SortingHouhou = -1
'        objXYZ = -1
'        BasePointID = -1
'        objDaisyo = -1
'        Index1 = -1
'        Index2 = -1
'        Bunrui1 = -1
'        Bunrui2 = -1
'        flgBunrui = -1
'        flgOnlyOne = False
'        strName = ""
'        strBikou = ""

'    End Sub

'    Public Sub New(ByVal Fields() As String)
'        TypeID = -1
'        TenGunID = -1
'        MotoGunID = -1
'        SyoriHouhouID = -1
'        SortingHouhou = -1
'        objXYZ = -1
'        BasePointID = -1
'        objDaisyo = -1
'        Index1 = -1
'        Index2 = -1
'        Bunrui1 = -1
'        Bunrui2 = -1
'        flgBunrui = -1
'        flgOnlyOne = False
'        strName = ""
'        strBikou = ""
'        Try
'            TypeID = CInt(Fields(0))
'        Catch ex As Exception

'        End Try
'        Try
'            TenGunID = CInt(Fields(1))
'        Catch ex As Exception

'        End Try
'        Try
'            MotoGunID = CInt(Fields(2))
'        Catch ex As Exception

'        End Try

'        Try
'            SyoriHouhouID = CInt(Fields(3))
'        Catch ex As Exception

'        End Try

'        Try
'            SortingHouhou = CInt(Fields(4))
'        Catch ex As Exception

'        End Try

'        Try
'            objXYZ = CInt(Fields(5))
'        Catch ex As Exception

'        End Try

'        Try
'            BasePointID = CInt(Fields(6))
'        Catch ex As Exception

'        End Try

'        Try
'            objDaisyo = CInt(Fields(7))
'        Catch ex As Exception

'        End Try

'        Try
'            Index1 = CInt(Fields(8))
'        Catch ex As Exception

'        End Try

'        Try
'            Index2 = CInt(Fields(9))
'        Catch ex As Exception

'        End Try

'        Try
'            Bunrui1 = CInt(Fields(10))
'        Catch ex As Exception

'        End Try

'        Try
'            Bunrui2 = CInt(Fields(11))
'        Catch ex As Exception

'        End Try
'        Try
'            flgBunrui = CInt(Fields(12))
'        Catch ex As Exception

'        End Try
'        Try
'            flgOnlyOne = CBool(Fields(13))
'        Catch ex As Exception

'        End Try

'        Try
'            strName = Fields(14)
'        Catch ex As Exception

'        End Try

'        Try
'            strBikou = Fields(15)
'        Catch ex As Exception

'        End Try
'    End Sub

'    Public Sub CalcTenGun()
'        Select Case SyoriHouhouID
'            Case TengunSyoriHouhou.OnePoint
'                ReDim Tengun(0)
'                Tengun(0) = MotoGun(BasePointID)
'            Case TengunSyoriHouhou.Sorting
'                GetSorted(MotoGun, objXYZ, SortingHouhou, Tengun)
'            Case TengunSyoriHouhou.BaseTenYoriDaisyo
'                GetAreaPointGroup(MotoGun, objXYZ, BasePointID, objDaisyo, Tengun)
'            Case TengunSyoriHouhou.HaninSitei
'                GetPoint_ByIndex(MotoGun, Index1, Index2, Tengun)
'            Case TengunSyoriHouhou.Bunrui
'                GetPoint_ByKubun(MotoGun, Bunrui1, Bunrui2, Tengun)
'        End Select
'    End Sub

'    Public Function GetFromTengun() As Point3D
'        GetFromTengun = Nothing

'        CalcTenGun()
'        Select Case flgBunrui
'            Case TengunBunruiShiyouHouhou.OnePoint
'                GetFromTengun = Tengun(0).CT_dat.lstRealP3d(0)
'            Case TengunBunruiShiyouHouhou.MultiPoint

'            Case TengunBunruiShiyouHouhou.CircleCenter     '元点から演算計算しているため、メモリ上の同期はとれない。注意！！！

'                If Tengun.Count = 3 Then
'                    Dim Circ As New GeoCurve
'                    YCM_Sunpo.GetCircle3P(Tengun(0).CT_dat.lstRealP3d(0),
'                                          Tengun(1).CT_dat.lstRealP3d(0),
'                                          Tengun(2).CT_dat.lstRealP3d(0), Circ)
'                    GetFromTengun = Circ.center.GetP3D
'                End If
'            Case TengunBunruiShiyouHouhou.MidPoint

'            Case TengunBunruiShiyouHouhou.MeanPoint

'        End Select

'    End Function

'    Public Function GetFromTengunNoUnit() As Point3D
'        GetFromTengunNoUnit = Nothing

'        CalcTenGun()
'        Select Case flgBunrui
'            Case TengunBunruiShiyouHouhou.OnePoint
'                GetFromTengunNoUnit = Tengun(0).CT_dat.lstP3d(0)
'            Case TengunBunruiShiyouHouhou.MultiPoint

'            Case TengunBunruiShiyouHouhou.CircleCenter
'                If Tengun.Count = 3 Then
'                    Dim Circ As New GeoCurve
'                    YCM_Sunpo.GetCircle3P(Tengun(0).CT_dat.lstP3d(0),
'                                          Tengun(1).CT_dat.lstP3d(0),
'                                          Tengun(2).CT_dat.lstP3d(0), Circ)
'                    GetFromTengunNoUnit = Circ.center.GetP3D
'                End If
'            Case TengunBunruiShiyouHouhou.MidPoint

'            Case TengunBunruiShiyouHouhou.MeanPoint

'        End Select

'    End Function

'End Class

Module CT_OFFSET_Kanren
    Dim dblLevelAdj As Double
    Public TargetMaxNumber As Integer = 1503
    Public arrCToffsets(TargetMaxNumber) As CToffset
    Public arrCTData(TargetMaxNumber) As CT_Data
    Public arrSTtoCTData(TargetMaxNumber) As CT_Data
    Public lstCT_Scale As List(Of CT_ScaleSettingData)
    Public lstCT_Coord As List(Of CT_CoordSettingData)
    '  Public lstCT_Sunpo As List(Of CT_SunpoSetting)
    Public lstTengun As List(Of TenGunTeigiTable)
    Public CamToRealHomMat3D As Object = Nothing
    Public CamToNoUnitHomMat3D As Object = Nothing
    Public flgManualScaleSetting As Boolean = False 'SUSANO ADD 20160520
    Public flgManualScaleAndOffset As Boolean = False 'SUSANO ADD 20160525
    Public ScaleToMM As Double = -1
    Public Const strCT_Bunrui As String = "CT_Bunrui.csv"
    Public Const strCT_ScaleSetting As String = "CT_ScaleSetting.csv"
    Public Const strCT_CoordSetting As String = "CT_CoordSetting.csv"
    Public Const strCT_SunpoSetting As String = "CT_SunpoSetting.csv"
    Public Const strTenGunTeigi As String = "TenGunTeigi.csv"
    Public Const strSystemMdbFile As String = "システム設定.mdb"          '2013/05/15 Suzuki ADD
    Private Sub Read_Type()
        Dim fileContents As String
        fileContents = My.Computer.FileSystem.ReadAllText(My.Application.Info.DirectoryPath & "\" & "TypeID.txt")
        Try
            CommonTypeID = CInt(fileContents)
        Catch ex As Exception
            CommonTypeID = 1
        End Try
    End Sub
    Private Sub Read_CTBunrui()
        For Each CTOS As CToffset In arrCToffsets
            CTOS = New CToffset
        Next
        Dim filename As String = My.Application.Info.DirectoryPath & "\" & strCT_Bunrui
        Dim fields As String()
        Dim delimiter As String = ","
        Dim i As Integer = 0
        Using parser As New TextFieldParser(filename)
            parser.SetDelimiters(delimiter)
            While Not parser.EndOfData
                ' Read in the fields for the current line
                fields = parser.ReadFields()
                ' Add code here to use data in fields variable.
                If IsNumeric(fields(0)) Then
                    Dim objCToffset As New CToffset(fields)
                    arrCToffsets(objCToffset.CT_ID) = objCToffset

                End If
            End While
        End Using

    End Sub

    Public Sub Read_CTBunrui2(ByVal TableName As String)
        For Each CTOS As CToffset In arrCToffsets
            CTOS = New CToffset
        Next
        Dim filename As String = My.Application.Info.DirectoryPath & "\" & TableName
        Dim fields As String()
        Dim delimiter As String = ","
        Dim i As Integer = 0
        Using parser As New TextFieldParser(filename)
            parser.SetDelimiters(delimiter)
            While Not parser.EndOfData
                ' Read in the fields for the current line
                fields = parser.ReadFields()
                ' Add code here to use data in fields variable.
                If IsNumeric(fields(0)) Then
                    Dim objCToffset As New CToffset(fields)
                    arrCToffsets(objCToffset.CT_ID) = objCToffset

                End If
            End While
        End Using

    End Sub


    '2013/05/15 Suzuki ADD
    'csvファイルの読み込みからDBを読み込む
    Private Sub Read_CTBunruiDB(Optional ByRef m_dbclass As CDBOperateOLE = Nothing)
        For Each CTOS As CToffset In arrCToffsets
            CTOS = New CToffset
        Next

        'システム設定DBへ接続

        Dim DBPath As String = My.Application.Info.DirectoryPath & "\計測システムフォルダ\" & strSystemMdbFile
        If m_dbclass Is Nothing Then
            If ConnectDB(DBPath, m_dbclass) = False Then
                MsgBox(strSystemMdbFile & "への接続に失敗しました。")
                Exit Sub
            End If
        End If

        '  Dim IDX As Integer = 0
        Dim strSql As String = "SELECT CT_NO,flg_Use,m_x,m_y,m_z,Kubun1,Kubun2,Info FROM CT_Bunrui WHERE TypeID = " & CommonTypeID
        Dim IDR As IDataReader
        IDR = m_dbclass.DoSelect(strSql)
        If Not IDR Is Nothing Then
            Do While IDR.Read
                Dim CToffsetD As New CToffset
                CToffsetD.GetDataByReader(IDR)
                arrCToffsets(CToffsetD.CT_ID) = CToffsetD
                'IDX += 1
            Loop
            IDR.Close()
        End If

        'システム設定DBへ切断
        DisConnectDB(m_dbclass)

    End Sub

    Private Sub Read_CTScaleSetting()

        Dim filename As String = My.Application.Info.DirectoryPath & "\" & strCT_ScaleSetting
        Dim fields As String()
        Dim delimiter As String = ","
        Using parser As New TextFieldParser(filename)
            parser.SetDelimiters(delimiter)
            While Not parser.EndOfData
                ' Read in the fields for the current line
                fields = parser.ReadFields()
                ' Add code here to use data in fields variable.
                If IsNumeric(fields(0)) Then
                    Dim objCTScale As New CT_ScaleSettingData(fields(0), fields(1), fields(2))
                    lstCT_Scale.Add(objCTScale)
                End If
            End While
        End Using

    End Sub

    '2013/05/15 Suzuki ADD
    'csvファイルの読み込みからDBを読み込む
    Private Sub Read_CTScaleSettingDB(Optional ByRef m_dbclass As CDBOperateOLE = Nothing)

        'システム設定DBへ接続

        Dim DBPath As String = My.Application.Info.DirectoryPath & "\計測システムフォルダ\" & strSystemMdbFile
        If m_dbclass Is Nothing Then
            If ConnectDB(DBPath, m_dbclass) = False Then
                MsgBox(strSystemMdbFile & "への接続に失敗しました。")
                Exit Sub
            End If
        End If

        Dim strSql As String = "SELECT * FROM CT_ScaleSetting WHERE TypeID = " & CommonTypeID & " ORDER BY CT_ID1"
        Dim IDR As IDataReader
        IDR = m_dbclass.DoSelect(strSql)
        If Not IDR Is Nothing Then
            Do While IDR.Read
                Dim lstCT_ScaleD As New CT_ScaleSettingData
                lstCT_ScaleD.GetDataByReader(IDR)
                lstCT_Scale.Add(lstCT_ScaleD)
            Loop
            IDR.Close()
        End If

        'システム設定DBへ切断
        DisConnectDB(m_dbclass)

    End Sub

    Private Sub CalcOffSet(ByVal blnSyori As Boolean, ByRef objFI As FBMlib.FeatureImage)
        Dim P1 As New GeoPoint
        Dim P2 As New GeoPoint
        Dim P3 As New GeoPoint
        Dim V12 As GeoVector
        Dim V13 As GeoVector

        Dim vecX As GeoVector
        Dim vecY As GeoVector
        Dim vecZ As GeoVector
        Dim vecOffSet As GeoVector

        'For Each CTD As CT_Data In arrCTData
        '    If Not CTD Is Nothing Then

        '        Try
        '            P1.SetFBM_3DPoint(CTD.CT_dat.lstRealP3d(0))
        '            P2.SetFBM_3DPoint(CTD.CT_dat.lstRealP3d(1))
        '            P3.SetFBM_3DPoint(CTD.CT_dat.lstRealP3d(2))
        '            V12 = P2.GetSubtracted(P1)
        '            V13 = P3.GetSubtracted(P1)
        '            'Z方向ベクトル計算

        '            vecZ = V12.GetOuterProduct(V13)
        '            vecZ.Normalize()
        '            vecZ.Mul(CTD.CToffsetval.offset_val.z)

        '            V12.Negate()
        '            V12.Normalize()
        '            'X方向ベクトル計算

        '            vecX = V12.GetMul(CTD.CToffsetval.offset_val.x)
        '            'Y方向ベクトル計算

        '            vecY = vecX.GetOuterProduct(vecZ)
        '            vecY.Normalize()
        '            vecY.Mul(CTD.CToffsetval.offset_val.y)
        '            'オフセット方向ベクトル計算

        '            vecOffSet = vecX.GetAdded(vecY)
        '            vecOffSet = vecOffSet.GetAdded(vecZ)

        '            vecZ.Normalize()
        '            vecZ.Negate()
        '            CTD.CT_dat.NormalV.X = vecZ.x
        '            CTD.CT_dat.NormalV.Y = vecZ.y
        '            CTD.CT_dat.NormalV.Z = vecZ.z
        '            CTD.CT_dat.OffsetV.X = vecOffSet.x
        '            CTD.CT_dat.OffsetV.Y = vecOffSet.y
        '            CTD.CT_dat.OffsetV.Z = vecOffSet.z
        '            If blnSyori = True Then
        '                For Each CTP As FBMlib.Point3D In CTD.CT_dat.lstRealP3d
        '                    Dim PP As New GeoPoint
        '                    PP.SetFBM_3DPoint(CTP)
        '                    CTP.CopyToMe(PP.GetMoved(vecOffSet).GetP3D)
        '                Next

        '            End If
        '        Catch ex As Exception

        '        End Try

        '    End If
        'Next
        For Each C3DCT As FBMlib.Common3DCodedTarget In objFI.lstCommon3dCT
            If C3DCT.flgUsable = True Then
                'If C3DCT.PID >= 500 Then Continue For	'Del By Suuri 20150323
                Dim CTD As CT_Data = arrCTData(C3DCT.PID)
                If Not CTD Is Nothing Then
                    Try
                        If C3DCT.PID = 413 Then
                            Dim sss As Integer = 1
                        End If
                        P1.SetFBM_3DPoint(CTD.CT_dat.lstRealP3d(0))
                        P2.SetFBM_3DPoint(CTD.CT_dat.lstRealP3d(1))
                        P3.SetFBM_3DPoint(CTD.CT_dat.lstRealP3d(2))
                        V12 = P2.GetSubtracted(P1)
                        V13 = P3.GetSubtracted(P1)
                        'Z方向ベクトル計算()
                        Dim flgZminus As Boolean = False
                        If (CTD.CToffsetval.offset_val.z < 0) Then
                            flgZminus = True
                            CTD.CToffsetval.offset_val.z = Math.Abs(CTD.CToffsetval.offset_val.z)
                        End If
                        vecZ = V12.GetOuterProduct(V13)
                        vecZ.Normalize()
                        vecZ.Mul(CTD.CToffsetval.offset_val.z)

                        V12.Negate()
                        V12.Normalize()
                        'X方向ベクトル計算

                        vecX = V12.GetMul(CTD.CToffsetval.offset_val.x)
                        'Y方向ベクトル計算
#If True Then
                        vecY = V12.GetOuterProduct(vecZ)
                        vecY.Normalize()
                        vecY.Mul(CTD.CToffsetval.offset_val.y)
#Else
                        V13.Normalize()

                        vecY = V13.GetMultipled(CTD.CToffsetval.offset_val.y)
#End If


                        'オフセット方向ベクトル計算
                        If flgZminus = True Then
                            vecZ.Negate()
                        End If
                        vecOffSet = vecX.GetAdded(vecY)
                        vecOffSet = vecOffSet.GetAdded(vecZ)

                        vecZ.Normalize()
                        vecZ.Negate()
                        CTD.CT_dat.NormalV.X = vecZ.x
                        CTD.CT_dat.NormalV.Y = vecZ.y
                        CTD.CT_dat.NormalV.Z = vecZ.z
                        CTD.CT_dat.OffsetV.X = vecOffSet.x
                        CTD.CT_dat.OffsetV.Y = vecOffSet.y
                        CTD.CT_dat.OffsetV.Z = vecOffSet.z
                        If blnSyori = True Then
                            For Each CTP As FBMlib.Point3D In CTD.CT_dat.lstRealP3d
                                Dim PP As New GeoPoint
                                PP.SetFBM_3DPoint(CTP)
                                CTP.CopyToMe(PP.GetMoved(vecOffSet).GetP3D)
                            Next

                        End If
                    Catch ex As Exception

                    End Try
                End If
            End If
        Next

    End Sub

    Private Sub ReadCoordSetting()
        Dim filename As String = My.Application.Info.DirectoryPath & "\" & strCT_CoordSetting
        Dim fields As String()
        Dim delimiter As String = ","
        Using parser As New TextFieldParser(filename)
            parser.SetDelimiters(delimiter)
            While Not parser.EndOfData
                ' Read in the fields for the current line
                fields = parser.ReadFields()
                ' Add code here to use data in fields variable.
                If IsNumeric(fields(0)) Then
                    Dim objCTCoord As New CT_CoordSettingData(fields, arrCTData)

                    If objCTCoord.CT_Active = 1 Then
                        lstCT_Coord.Add(objCTCoord)
                    End If
                End If
            End While
        End Using
    End Sub

    '2013/05/15 Suzuki ADD
    'csvファイルの読み込みからDBを読み込む
    Private Sub ReadCoordSettingDB(Optional ByRef m_dbclass As CDBOperateOLE = Nothing)

        If lstCT_Coord Is Nothing Then
            lstCT_Coord = New List(Of CT_CoordSettingData)
        Else
            lstCT_Coord.Clear()
        End If

        'システム設定DBへ接続

        Dim DBPath As String = My.Application.Info.DirectoryPath & "\計測システムフォルダ\" & strSystemMdbFile
        If m_dbclass Is Nothing Then
            If ConnectDB(DBPath, m_dbclass) = False Then
                MsgBox(strSystemMdbFile & "への接続に失敗しました。")
                Exit Sub
            End If
        End If

        Dim strSql As String = "SELECT * FROM CT_CoordSetting WHERE TypeID = " & CommonTypeID & " ORDER BY ID"
        Dim IDR As IDataReader
        IDR = m_dbclass.DoSelect(strSql)
        If Not IDR Is Nothing Then
            Do While IDR.Read
                Dim CTCoordD As New CT_CoordSettingData
                CTCoordD.GetDataByReader(IDR, arrCTData)
                If CTCoordD.CT_Active = 1 Then
                    lstCT_Coord.Add(CTCoordD)
                End If
            Loop
            IDR.Close()
        End If

        'システム設定DBへ切断
        DisConnectDB(m_dbclass)

    End Sub

    Private Sub TransCoordSys()
        If CamToRealHomMat3D Is Nothing Then
            Exit Sub
        End If
        Dim i As Integer = 0
        For Each ISI As ImageSet In MainFrm.objFBM.lstImages
            If ISI.flgConnected = True Then
                MainFrm.objFBM.CalcReProjectionErrorOneImage(ISI)
            End If
        Next

        For Each C3DCT As Common3DCodedTarget In MainFrm.objFBM.lstCommon3dCT
            i = 0
            For Each P3D As Point3D In C3DCT.lstP3d
                'P3D.SetScale(ScaleToMM)
                'HOperatorSet.AffineTransPoint3D(CamToRealHomMat3D, P3D.X, P3D.Y, P3D.Z, P3D.X, P3D.Y, P3D.Z)
                P3D.CopyToMe(C3DCT.lstRealP3d(i))
                P3D.SetScale((1 / ScaleToMM))
                i += 1
            Next
        Next
        If My.Settings.blnCTonly = False Then
            For Each C3DST As Common3DSingleTarget In MainFrm.objFBM.lstCommon3dST
                C3DST.P3d.CopyToMe(C3DST.realP3d)
                C3DST.P3d.SetScale((1 / ScaleToMM))
            Next
        End If
        For Each ISI As ImageSet In MainFrm.objFBM.lstImages
            If ISI.flgConnected = True Then
                Dim HM3D As Object = Nothing
                HOperatorSet.PoseToHomMat3d(ISI.ImagePose.Pose, HM3D)
                HOperatorSet.HomMat3dScale(HM3D, ScaleToMM, ScaleToMM, ScaleToMM, 0.0, 0.0, 0.0, HM3D)
                HOperatorSet.HomMat3dCompose(CamToRealHomMat3D, HM3D, HM3D)
                HOperatorSet.HomMat3dScale(HM3D, (1 / ScaleToMM), (1 / ScaleToMM), (1 / ScaleToMM), 0.0, 0.0, 0.0, HM3D)
                HOperatorSet.HomMat3dToPose(HM3D, ISI.ImagePose.Pose)
            End If
        Next

        For Each ISI As ImageSet In MainFrm.objFBM.lstImages
            If ISI.flgConnected = True Then
                MainFrm.objFBM.CalcReProjectionErrorOneImage(ISI)
            End If
        Next


        If sys_ScaleInfo.p1 Is Nothing Or sys_ScaleInfo.p2 Is Nothing Then
            For Each CTSC As CT_ScaleSettingData In lstCT_Scale
                If Not CTSC.CT1 Is Nothing Then
                    If Not CTSC.CT2 Is Nothing Then
                        sys_ScaleInfo.p1 = New CLookPoint(CDbl(CTSC.CT1.lstP3d(0).X), CDbl(CTSC.CT1.lstP3d(0).Y), CDbl(CTSC.CT1.lstP3d(0).Z))
                        sys_ScaleInfo.p2 = New CLookPoint(CDbl(CTSC.CT2.lstP3d(0).X), CDbl(CTSC.CT2.lstP3d(0).Y), CDbl(CTSC.CT2.lstP3d(0).Z))
                        sys_ScaleInfo.p1.LabelName = CTSC.CT1.currentLabel
                        sys_ScaleInfo.p2.LabelName = CTSC.CT2.currentLabel
                        sys_ScaleInfo.len = CTSC.CT_kan_dist
                        sys_ScaleInfo.scale = ScaleToMM
                        If nLookPoints > 0 Then
                            Dim scalefrm As New YCM_ScaleSetting
                            scalefrm.tmpFlag = False ' SUSANO ADD 20160606
                            scalefrm.Show()
                            scalefrm.CbBscalelong.Text = sys_ScaleInfo.len
                            scalefrm.Button3.PerformClick()
                        End If
                        Exit For
                    End If
                End If
            Next
        ElseIf sys_ScaleInfo.p1.LabelName Is Nothing Or sys_ScaleInfo.p2.LabelName Is Nothing Then
            For Each CTSC As CT_ScaleSettingData In lstCT_Scale
                If Not CTSC.CT1 Is Nothing Then
                    If Not CTSC.CT2 Is Nothing Then
                        sys_ScaleInfo.p1 = New CLookPoint(CDbl(CTSC.CT1.lstP3d(0).X), CDbl(CTSC.CT1.lstP3d(0).Y), CDbl(CTSC.CT1.lstP3d(0).Z))
                        sys_ScaleInfo.p2 = New CLookPoint(CDbl(CTSC.CT2.lstP3d(0).X), CDbl(CTSC.CT2.lstP3d(0).Y), CDbl(CTSC.CT2.lstP3d(0).Z))
                        sys_ScaleInfo.p1.LabelName = CTSC.CT1.currentLabel
                        sys_ScaleInfo.p2.LabelName = CTSC.CT2.currentLabel
                        sys_ScaleInfo.len = CTSC.CT_kan_dist
                        sys_ScaleInfo.scale = ScaleToMM
                        If nLookPoints > 0 Then
                            Dim scalefrm As New YCM_ScaleSetting
                            scalefrm.tmpFlag = False ' SUSANO ADD 20160606
                            scalefrm.Show()
                            scalefrm.CbBscalelong.Text = sys_ScaleInfo.len
                            scalefrm.Button3.PerformClick()
                        End If
                        Exit For
                    End If
                End If
            Next
        Else
            If nLookPoints > 0 Then
                Dim scalefrm As New YCM_ScaleSetting
                scalefrm.tmpFlag = False ' SUSANO ADD 20160606
                scalefrm.Show()
                scalefrm.CbBscalelong.Text = sys_ScaleInfo.len
                scalefrm.Button3.PerformClick()
            End If
        End If



    End Sub

    Private Sub CalcCoordMat()
        Dim objCTCoord As CT_CoordSettingData = lstCT_Coord(0)
        '座標系定義用の点抽出
        Dim XY_PG As New Point3D
        Dim XY_PX As New Point3D
        Dim XY_PY As New Point3D
        Dim PG As New Point3D
        Dim PX As New Point3D
        Dim PY As New Point3D
        Dim PZ As New Point3D
        If objCTCoord.CT_ID1 > 0 Then
            XY_PG.CopyToMe(lstTengun(objCTCoord.CT_ID1 - 1).GetFromTengun)
        End If
        If objCTCoord.CT_ID2 > 0 Then
            XY_PX.CopyToMe(lstTengun(objCTCoord.CT_ID2 - 1).GetFromTengun)
        End If
        If objCTCoord.CT_ID3 > 0 Then
            XY_PY.CopyToMe(lstTengun(objCTCoord.CT_ID3 - 1).GetFromTengun)
        End If
        If objCTCoord.CT_GenID > 0 Then
            PG.CopyToMe(lstTengun(objCTCoord.CT_GenID - 1).GetFromTengun)
        End If
        If objCTCoord.CT_XID > 0 Then
            PX.CopyToMe(lstTengun(objCTCoord.CT_XID - 1).GetFromTengun)
        End If
        If objCTCoord.CT_YID > 0 Then
            PY.CopyToMe(lstTengun(objCTCoord.CT_YID - 1).GetFromTengun)
        End If
        If objCTCoord.CT_ZID > 0 Then
            PZ.CopyToMe(lstTengun(objCTCoord.CT_ZID - 1).GetFromTengun)
        End If
        HOperatorSet.HomMat3dIdentity(CamToRealHomMat3D)
        If objCTCoord.XYorXZorYZ = 0 Then
            If PG Is Nothing Or PX Is Nothing Or PY Is Nothing Then
                Exit Sub
            End If
            If PG.IsDBNULL Or PX.IsDBNULL Or PY.IsDBNULL Then
                Exit Sub
            End If

            '平面設定
            HOperatorSet.HomMat3dIdentity(CamToRealHomMat3D)
            TransCoordSystem(PG, PX, PY, CamToRealHomMat3D)
            For Each C3DCT As Common3DCodedTarget In MainFrm.objFBM.lstCommon3dCT
                For Each P3D As Point3D In C3DCT.lstRealP3d
                    HOperatorSet.AffineTransPoint3d(CamToRealHomMat3D, P3D.X, P3D.Y, P3D.Z, P3D.X, P3D.Y, P3D.Z)
                Next
            Next
            If My.Settings.blnCTonly = False Then
                For Each C3DST As Common3DSingleTarget In MainFrm.objFBM.lstCommon3dST
                    HOperatorSet.AffineTransPoint3d(CamToRealHomMat3D,
                                          C3DST.realP3d.X, C3DST.realP3d.Y, C3DST.realP3d.Z,
                                          C3DST.realP3d.X, C3DST.realP3d.Y, C3DST.realP3d.Z)
                Next
            End If

        ElseIf objCTCoord.XYorXZorYZ = 1 Then
            If PG Is Nothing Or PX Is Nothing Then
                CamToRealHomMat3D = Nothing
                Exit Sub
            End If
            If XY_PG Is Nothing Or XY_PX Is Nothing Or XY_PY Is Nothing Then
                CamToRealHomMat3D = Nothing
                Exit Sub
            End If
            If PG.IsDBNULL Or PX.IsDBNULL Then
                CamToRealHomMat3D = Nothing
                Exit Sub
            End If
            If XY_PG.IsDBNULL Or XY_PX.IsDBNULL Or XY_PY.IsDBNULL Then
                CamToRealHomMat3D = Nothing
                Exit Sub
            End If
            Dim tmpHomMat3d As Object = Nothing

            '平面設定
            HOperatorSet.HomMat3dIdentity(CamToRealHomMat3D)
            HOperatorSet.HomMat3dIdentity(tmpHomMat3d)
            TransCoordSystem(XY_PG, XY_PX, XY_PY, tmpHomMat3d)
            For Each C3DCT As Common3DCodedTarget In MainFrm.objFBM.lstCommon3dCT
                For Each P3D As Point3D In C3DCT.lstRealP3d
                    HOperatorSet.AffineTransPoint3d(tmpHomMat3d, P3D.X, P3D.Y, P3D.Z, P3D.X, P3D.Y, P3D.Z)
                Next
            Next
            '＊＊＊＊＊＊＊レベル調整処理　20130824　SUURI ADD START ＊＊＊＊＊＊＊＊＊

#If True Then


            HOperatorSet.HomMat3dCompose(tmpHomMat3d, CamToRealHomMat3D, CamToRealHomMat3D)
            HOperatorSet.HomMat3dIdentity(tmpHomMat3d)

            CalcTenGun(False)

            If objCTCoord.CT_GenID > 0 Then
                PG.CopyToMe(lstTengun(objCTCoord.CT_GenID - 1).GetFromTengun)
            End If
            If objCTCoord.CT_XID > 0 Then
                PX.CopyToMe(lstTengun(objCTCoord.CT_XID - 1).GetFromTengun)
            End If
            If objCTCoord.CT_ID1 > 0 Then
                XY_PG.CopyToMe(lstTengun(objCTCoord.CT_ID1 - 1).GetFromTengun)
            End If
            If objCTCoord.CT_ID2 > 0 Then
                XY_PX.CopyToMe(lstTengun(objCTCoord.CT_ID2 - 1).GetFromTengun)
            End If
            If objCTCoord.CT_ID3 > 0 Then
                XY_PY.CopyToMe(lstTengun(objCTCoord.CT_ID3 - 1).GetFromTengun)
            End If

            '高さ調整値を取得

            For Each objSunpo As SunpoSetTable In WorksD.SunpoSetL
                If objSunpo.SunpoMark = "Leveladj" Then
                    dblLevelAdj = objSunpo.KiteiVal
                End If
            Next
            '回転軸を決定

            Dim RotateAxis As New GeoVector(XY_PY.X, XY_PY.Y, XY_PY.Z)
            Dim hRotateAxis As Object
            hRotateAxis = BTuple.TupleConcat(RotateAxis.x, RotateAxis.y)
            hRotateAxis = BTuple.TupleConcat(hRotateAxis, RotateAxis.z)
            '回転角度を決定

            Dim sinA As Double = dblLevelAdj / XY_PX.X
            Dim leveladjAngle As Double = BTuple.TupleAsin(sinA).D

            HOperatorSet.HomMat3dRotate(tmpHomMat3d, -leveladjAngle, hRotateAxis, 0, 0, 0, tmpHomMat3d)

            For Each C3DCT As Common3DCodedTarget In MainFrm.objFBM.lstCommon3dCT
                For Each P3D As Point3D In C3DCT.lstRealP3d
                    HOperatorSet.AffineTransPoint3d(tmpHomMat3d, P3D.X, P3D.Y, P3D.Z, P3D.X, P3D.Y, P3D.Z)
                Next
            Next
#End If
            '＊＊＊＊＊＊＊レベル調整処理　20130824　SUURI ADD END＊＊＊＊＊＊＊＊＊

            CalcTenGun(False)

            If objCTCoord.CT_GenID > 0 Then
                PG.CopyToMe(lstTengun(objCTCoord.CT_GenID - 1).GetFromTengun)
            End If
            If objCTCoord.CT_XID > 0 Then
                PX.CopyToMe(lstTengun(objCTCoord.CT_XID - 1).GetFromTengun)
            End If
            If objCTCoord.CT_YID > 0 Then
                PY.CopyToMe(lstTengun(objCTCoord.CT_YID - 1).GetFromTengun)
            End If


            'HOperatorSet.AffineTransPoint3D(tmpHomMat3d, PG.X, PG.Y, PG.Z, PG.X, PG.Y, PG.Z)
            'HOperatorSet.AffineTransPoint3D(tmpHomMat3d, PX.X, PX.Y, PX.Z, PX.X, PX.Y, PX.Z)
            HOperatorSet.HomMat3dCompose(tmpHomMat3d, CamToRealHomMat3D, CamToRealHomMat3D)
            '原点設定（平行移動）

            HOperatorSet.HomMat3dIdentity(tmpHomMat3d)
            HOperatorSet.HomMat3dTranslate(tmpHomMat3d, PG.X, PG.Y, 0.0, tmpHomMat3d) ' PG.Z, tmpHomMat3d)
            ' HOperatorSet.HomMat3dTranslate(CamToRealHomMat3D, PG.X, PG.Y, PG.Z, CamToRealHomMat3D)
            HOperatorSet.HomMat3dInvert(tmpHomMat3d, tmpHomMat3d)
            ' HOperatorSet.HomMat3dInvert(CamToRealHomMat3D, CamToRealHomMat3D)

            HOperatorSet.HomMat3dCompose(tmpHomMat3d, CamToRealHomMat3D, CamToRealHomMat3D)
            For Each C3DCT As Common3DCodedTarget In MainFrm.objFBM.lstCommon3dCT
                For Each P3D As Point3D In C3DCT.lstRealP3d
                    HOperatorSet.AffineTransPoint3d(tmpHomMat3d, P3D.X, P3D.Y, P3D.Z, P3D.X, P3D.Y, P3D.Z)
                Next
            Next
            HOperatorSet.AffineTransPoint3d(tmpHomMat3d, PX.X, PX.Y, PX.Z, PX.X, PX.Y, PX.Z)
            'Ｘ軸あわせ（回転）

            HOperatorSet.HomMat3dIdentity(tmpHomMat3d)
            Dim Phi As Double = GetAngleRight(PX, New Point3D(New HTuple(1.0), New HTuple(0.0), New HTuple(0.0)), Plane.XY)

            HOperatorSet.HomMat3dRotateLocal(tmpHomMat3d, Phi, "z", tmpHomMat3d)
            ' HOperatorSet.HomMat3dRotateLocal(CamToRealHomMat3D, Phi, "z", CamToRealHomMat3D)
            HOperatorSet.HomMat3dInvert(tmpHomMat3d, tmpHomMat3d)
            ' HOperatorSet.HomMat3dInvert(CamToRealHomMat3D, CamToRealHomMat3D)
            For Each C3DCT As Common3DCodedTarget In MainFrm.objFBM.lstCommon3dCT
                For Each P3D As Point3D In C3DCT.lstRealP3d
                    HOperatorSet.AffineTransPoint3d(tmpHomMat3d, P3D.X, P3D.Y, P3D.Z, P3D.X, P3D.Y, P3D.Z)
                Next
            Next
            HOperatorSet.HomMat3dCompose(tmpHomMat3d, CamToRealHomMat3D, CamToRealHomMat3D)

        ElseIf objCTCoord.XYorXZorYZ = 2 Then
            If PG Is Nothing Or PX Is Nothing Then
                'H25.6.28　Yamada　コメントアウト

                'MsgBox("座標系定義に失敗しました")
                CamToRealHomMat3D = Nothing
                Exit Sub
            End If
            If XY_PG Is Nothing Or XY_PX Is Nothing Or XY_PY Is Nothing Then
                'H25.6.28　Yamada　コメントアウト

                'MsgBox("座標系定義に失敗しました")
                CamToRealHomMat3D = Nothing
                Exit Sub
            End If
            If PG.IsDBNULL Or PY.IsDBNULL Then
                'H25.6.28　Yamada　コメントアウト

                'MsgBox("座標系定義に失敗しました")
                CamToRealHomMat3D = Nothing
                Exit Sub
            End If
            If XY_PG.IsDBNULL Or XY_PX.IsDBNULL Or XY_PY.IsDBNULL Then
                'H25.6.28　Yamada　コメントアウト

                'MsgBox("座標系定義に失敗しました")
                CamToRealHomMat3D = Nothing
                Exit Sub
            End If
            Dim tmpHomMat3d As Object = Nothing

            '平面設定

            HOperatorSet.HomMat3dIdentity(CamToRealHomMat3D)
            HOperatorSet.HomMat3dIdentity(tmpHomMat3d)
            TransCoordSystem(XY_PG, XY_PX, XY_PY, tmpHomMat3d)
            For Each C3DCT As Common3DCodedTarget In MainFrm.objFBM.lstCommon3dCT
                For Each P3D As Point3D In C3DCT.lstRealP3d
                    HOperatorSet.AffineTransPoint3d(tmpHomMat3d, P3D.X, P3D.Y, P3D.Z, P3D.X, P3D.Y, P3D.Z)
                Next
            Next
            CalcTenGun(False)

            If objCTCoord.CT_GenID > 0 Then
                PG.CopyToMe(lstTengun(objCTCoord.CT_GenID - 1).GetFromTengun)
            End If
            If objCTCoord.CT_XID > 0 Then
                PX.CopyToMe(lstTengun(objCTCoord.CT_XID - 1).GetFromTengun)
            End If
            If objCTCoord.CT_YID > 0 Then
                PY.CopyToMe(lstTengun(objCTCoord.CT_YID - 1).GetFromTengun)
            End If
            'HOperatorSet.AffineTransPoint3D(tmpHomMat3d, PG.X, PG.Y, PG.Z, PG.X, PG.Y, PG.Z)
            'HOperatorSet.AffineTransPoint3D(tmpHomMat3d, PX.X, PX.Y, PX.Z, PX.X, PX.Y, PX.Z)
            HOperatorSet.HomMat3dCompose(tmpHomMat3d, CamToRealHomMat3D, CamToRealHomMat3D)
            '原点設定（平行移動）

            HOperatorSet.HomMat3dIdentity(tmpHomMat3d)
            HOperatorSet.HomMat3dTranslate(tmpHomMat3d, PG.X, PG.Y, -82.6, tmpHomMat3d) ' PG.Z, tmpHomMat3d)
            ' HOperatorSet.HomMat3dTranslate(CamToRealHomMat3D, PG.X, PG.Y, PG.Z, CamToRealHomMat3D)
            HOperatorSet.HomMat3dInvert(tmpHomMat3d, tmpHomMat3d)
            ' HOperatorSet.HomMat3dInvert(CamToRealHomMat3D, CamToRealHomMat3D)

            HOperatorSet.HomMat3dCompose(tmpHomMat3d, CamToRealHomMat3D, CamToRealHomMat3D)
            For Each C3DCT As Common3DCodedTarget In MainFrm.objFBM.lstCommon3dCT
                For Each P3D As Point3D In C3DCT.lstRealP3d
                    HOperatorSet.AffineTransPoint3d(tmpHomMat3d, P3D.X, P3D.Y, P3D.Z, P3D.X, P3D.Y, P3D.Z)
                Next
            Next
            HOperatorSet.AffineTransPoint3d(tmpHomMat3d, PY.X, PY.Y, PY.Z, PY.X, PY.Y, PY.Z)
            'Y軸あわせ（回転）

            HOperatorSet.HomMat3dIdentity(tmpHomMat3d)
            Dim Phi As Object = GetAngleRight(PY, New Point3D(New HTuple(0.0), New HTuple(1.0), New HTuple(0.0)), Plane.XY)

            HOperatorSet.HomMat3dRotateLocal(tmpHomMat3d, Phi, "z", tmpHomMat3d)
            ' HOperatorSet.HomMat3dRotateLocal(CamToRealHomMat3D, Phi, "z", CamToRealHomMat3D)
            HOperatorSet.HomMat3dInvert(tmpHomMat3d, tmpHomMat3d)
            ' HOperatorSet.HomMat3dInvert(CamToRealHomMat3D, CamToRealHomMat3D)
            For Each C3DCT As Common3DCodedTarget In MainFrm.objFBM.lstCommon3dCT
                For Each P3D As Point3D In C3DCT.lstRealP3d
                    HOperatorSet.AffineTransPoint3d(tmpHomMat3d, P3D.X, P3D.Y, P3D.Z, P3D.X, P3D.Y, P3D.Z)
                Next
            Next
            HOperatorSet.HomMat3dCompose(tmpHomMat3d, CamToRealHomMat3D, CamToRealHomMat3D)
        ElseIf objCTCoord.XYorXZorYZ = 3 Then
            If PG Is Nothing Or PX Is Nothing Or PY Is Nothing Or PZ Is Nothing Then
                'H25.6.28　Yamada　コメントアウト

                'MsgBox("座標系定義に失敗しました")
                ' CamToRealHomMat3D = Nothing
                Exit Sub
            End If
            If PG.IsDBNULL Or PX.IsDBNULL Or PY.IsDBNULL Or PZ.IsDBNULL Then
                'H25.6.28　Yamada　コメントアウト

                'MsgBox("座標系定義に失敗しました")
                ' CamToRealHomMat3D = Nothing
                Exit Sub
            End If
            '平面設定

            HOperatorSet.HomMat3dIdentity(CamToRealHomMat3D)
            TransCoordSystemGX_ZHeimen(PG, PX, PY, PZ, CamToRealHomMat3D)

            For Each C3DCT As Common3DCodedTarget In MainFrm.objFBM.lstCommon3dCT
                For Each P3D As Point3D In C3DCT.lstRealP3d
                    HOperatorSet.AffineTransPoint3d(CamToRealHomMat3D, P3D.X, P3D.Y, P3D.Z, P3D.X, P3D.Y, P3D.Z)
                Next
            Next
            If My.Settings.blnCTonly = False Then
                For Each C3DST As Common3DSingleTarget In MainFrm.objFBM.lstCommon3dST
                    HOperatorSet.AffineTransPoint3d(CamToRealHomMat3D,
                                          C3DST.realP3d.X, C3DST.realP3d.Y, C3DST.realP3d.Z,
                                          C3DST.realP3d.X, C3DST.realP3d.Y, C3DST.realP3d.Z)
                Next
            End If
        ElseIf objCTCoord.XYorXZorYZ = 4 Then
            If PG Is Nothing Or PX Is Nothing Or PY Is Nothing Then
                'H25.6.28　Yamada　コメントアウト

                'MsgBox("座標系定義に失敗しました")
                ' CamToRealHomMat3D = Nothing
                Exit Sub
            End If
            If PG.IsDBNULL Or PX.IsDBNULL Or PY.IsDBNULL Then
                'H25.6.28　Yamada　コメントアウト

                'MsgBox("座標系定義に失敗しました")
                ' CamToRealHomMat3D = Nothing
                Exit Sub
            End If
            HOperatorSet.HomMat3dIdentity(CamToRealHomMat3D)
            Dim Pgenten As Point3D = GetLineToPointNoFootP(PG, PX, PY)
            TransCoordSystemGX_minusY(Pgenten, PX, PY, PZ, CamToRealHomMat3D)
            For Each C3DCT As Common3DCodedTarget In MainFrm.objFBM.lstCommon3dCT
                For Each P3D As Point3D In C3DCT.lstRealP3d
                    HOperatorSet.AffineTransPoint3d(CamToRealHomMat3D, P3D.X, P3D.Y, P3D.Z, P3D.X, P3D.Y, P3D.Z)
                Next
            Next
            If My.Settings.blnCTonly = False Then
                For Each C3DST As Common3DSingleTarget In MainFrm.objFBM.lstCommon3dST
                    HOperatorSet.AffineTransPoint3d(CamToRealHomMat3D,
                                          C3DST.realP3d.X, C3DST.realP3d.Y, C3DST.realP3d.Z,
                                          C3DST.realP3d.X, C3DST.realP3d.Y, C3DST.realP3d.Z)
                Next
            End If
        ElseIf objCTCoord.XYorXZorYZ = 5 Then

            If PG Is Nothing Or PX Is Nothing Or PY Is Nothing Or PZ Is Nothing Then
                'H25.6.28　Yamada　コメントアウト

                'MsgBox("座標系定義に失敗しました")
                ' CamToRealHomMat3D = Nothing
                Exit Sub
            End If
            If PG.IsDBNULL Or PX.IsDBNULL Or PY.IsDBNULL Or PZ.IsDBNULL Then
                'H25.6.28　Yamada　コメントアウト

                'MsgBox("座標系定義に失敗しました")
                ' CamToRealHomMat3D = Nothing
                Exit Sub
            End If
            '平面設定

            HOperatorSet.HomMat3dIdentity(CamToRealHomMat3D)
            TransCoordSystemGX_ZHeimen(PG, PX, PY, PZ, CamToRealHomMat3D)
            For Each C3DCT As Common3DCodedTarget In MainFrm.objFBM.lstCommon3dCT
                For Each P3D As Point3D In C3DCT.lstRealP3d
                    HOperatorSet.AffineTransPoint3d(CamToRealHomMat3D, P3D.X, P3D.Y, P3D.Z, P3D.X, P3D.Y, P3D.Z)
                Next
            Next
            If My.Settings.blnCTonly = False Then
                For Each C3DST As Common3DSingleTarget In MainFrm.objFBM.lstCommon3dST
                    HOperatorSet.AffineTransPoint3d(CamToRealHomMat3D,
                                          C3DST.realP3d.X, C3DST.realP3d.Y, C3DST.realP3d.Z,
                                          C3DST.realP3d.X, C3DST.realP3d.Y, C3DST.realP3d.Z)
                Next
            End If

        ElseIf objCTCoord.XYorXZorYZ = 6 Then
            If PG Is Nothing Or PX Is Nothing Then
                'H25.6.28　Yamada　コメントアウト

                'MsgBox("座標系定義に失敗しました")
                CamToRealHomMat3D = Nothing
                Exit Sub
            End If
            If XY_PG Is Nothing Or XY_PX Is Nothing Or XY_PY Is Nothing Then
                'H25.6.28　Yamada　コメントアウト

                'MsgBox("座標系定義に失敗しました")
                CamToRealHomMat3D = Nothing
                Exit Sub
            End If

            'Rep By Suuri 20150323 Sta -----------
            'If PG.IsDBNULL Or PY.IsDBNULL Then
            If PG.IsDBNULL Or PY.IsDBNULL Then
                'Rep By Suuri 20150323 End -----------

                'H25.6.28　Yamada　コメントアウト

                'MsgBox("座標系定義に失敗しました")
                CamToRealHomMat3D = Nothing
                Exit Sub
            End If
            If XY_PG.IsDBNULL Or XY_PX.IsDBNULL Or XY_PY.IsDBNULL Then
                'H25.6.28　Yamada　コメントアウト

                'MsgBox("座標系定義に失敗しました")
                CamToRealHomMat3D = Nothing
                Exit Sub
            End If
            Dim tmpHomMat3d As Object = Nothing

            '平面設定

            HOperatorSet.HomMat3dIdentity(CamToRealHomMat3D)
            HOperatorSet.HomMat3dIdentity(tmpHomMat3d)
            TransCoordSystem(XY_PG, XY_PX, XY_PY, tmpHomMat3d)
            For Each C3DCT As Common3DCodedTarget In MainFrm.objFBM.lstCommon3dCT
                For Each P3D As Point3D In C3DCT.lstRealP3d
                    HOperatorSet.AffineTransPoint3d(tmpHomMat3d, P3D.X, P3D.Y, P3D.Z, P3D.X, P3D.Y, P3D.Z)
                Next
            Next
            '＊＊＊＊＊＊＊レベル調整処理　20130824　SUURI ADD START ＊＊＊＊＊＊＊＊＊

#If False Then


            HOperatorSet.HomMat3dCompose(tmpHomMat3d, CamToRealHomMat3D, CamToRealHomMat3D)
            HOperatorSet.HomMat3dIdentity(tmpHomMat3d)

            CalcTenGun(False)

            If objCTCoord.CT_GenID > 0 Then
                PG.CopyToMe(lstTengun(objCTCoord.CT_GenID - 1).GetFromTengun)
            End If
            If objCTCoord.CT_XID > 0 Then
                PX.CopyToMe(lstTengun(objCTCoord.CT_XID - 1).GetFromTengun)
            End If
            If objCTCoord.CT_ID1 > 0 Then
                XY_PG.CopyToMe(lstTengun(objCTCoord.CT_ID1 - 1).GetFromTengun)
            End If
            If objCTCoord.CT_ID2 > 0 Then
                XY_PX.CopyToMe(lstTengun(objCTCoord.CT_ID2 - 1).GetFromTengun)
            End If
            If objCTCoord.CT_ID3 > 0 Then
                XY_PY.CopyToMe(lstTengun(objCTCoord.CT_ID3 - 1).GetFromTengun)
            End If

            '高さ調整値を取得

            For Each objSunpo As SunpoSetTable In WorksD.SunpoSetL
                If objSunpo.SunpoMark = "Leveladj" Then
                    dblLevelAdj = objSunpo.KiteiVal
                End If
            Next
            '回転軸を決定

            Dim RotateAxis As New GeoVector(XY_PY.X, XY_PY.Y, XY_PY.Z)
            Dim hRotateAxis As Object
            hRotateAxis = BTuple.TupleConcat(RotateAxis.x, RotateAxis.y)
            hRotateAxis = BTuple.TupleConcat(hRotateAxis, RotateAxis.z)
            '回転角度を決定

            Dim sinA As Double = dblLevelAdj / XY_PX.X
            Dim leveladjAngle As Double = BTuple.TupleAsin(sinA)

            HOperatorSet.HomMat3dRotate(tmpHomMat3d, -leveladjAngle, hRotateAxis, 0, 0, 0, tmpHomMat3d)

            For Each C3DCT As Common3DCodedTarget In MainFrm.objFBM.lstCommon3dCT
                For Each P3D As Point3D In C3DCT.lstRealP3d
                    HOperatorSet.AffineTransPoint3D(tmpHomMat3d, P3D.X, P3D.Y, P3D.Z, P3D.X, P3D.Y, P3D.Z)
                Next
            Next
#End If
            '＊＊＊＊＊＊＊レベル調整処理　20130824　SUURI ADD END＊＊＊＊＊＊＊＊＊

            CalcTenGun(False)

            If objCTCoord.CT_GenID > 0 Then
                PG.CopyToMe(lstTengun(objCTCoord.CT_GenID - 1).GetFromTengun)
            End If
            If objCTCoord.CT_XID > 0 Then
                PX.CopyToMe(lstTengun(objCTCoord.CT_XID - 1).GetFromTengun)
            End If
            If objCTCoord.CT_YID > 0 Then
                PY.CopyToMe(lstTengun(objCTCoord.CT_YID - 1).GetFromTengun)
            End If
            'HOperatorSet.AffineTransPoint3D(tmpHomMat3d, PG.X, PG.Y, PG.Z, PG.X, PG.Y, PG.Z)
            'HOperatorSet.AffineTransPoint3D(tmpHomMat3d, PX.X, PX.Y, PX.Z, PX.X, PX.Y, PX.Z)
            HOperatorSet.HomMat3dCompose(tmpHomMat3d, CamToRealHomMat3D, CamToRealHomMat3D)
            '原点設定（平行移動）

            HOperatorSet.HomMat3dIdentity(tmpHomMat3d)

            'Rep By Suuri 20150323 Sta ----
            'HOperatorSet.HomMat3dTranslate(tmpHomMat3d, PG.X, PG.Y, 0.0, tmpHomMat3d) ' PG.Z, tmpHomMat3d)
            HOperatorSet.HomMat3dTranslate(tmpHomMat3d, PG.X, PG.Y, 0, tmpHomMat3d) ' PG.Z, tmpHomMat3d)
            'Rep By Suuri 20150323 End ----

            ' HOperatorSet.HomMat3dTranslate(CamToRealHomMat3D, PG.X, PG.Y, PG.Z, CamToRealHomMat3D)
            HOperatorSet.HomMat3dInvert(tmpHomMat3d, tmpHomMat3d)
            ' HOperatorSet.HomMat3dInvert(CamToRealHomMat3D, CamToRealHomMat3D)

            HOperatorSet.HomMat3dCompose(tmpHomMat3d, CamToRealHomMat3D, CamToRealHomMat3D)
            For Each C3DCT As Common3DCodedTarget In MainFrm.objFBM.lstCommon3dCT
                For Each P3D As Point3D In C3DCT.lstRealP3d
                    HOperatorSet.AffineTransPoint3d(tmpHomMat3d, P3D.X, P3D.Y, P3D.Z, P3D.X, P3D.Y, P3D.Z)
                Next
            Next
            HOperatorSet.AffineTransPoint3d(tmpHomMat3d, PX.X, PX.Y, PX.Z, PX.X, PX.Y, PX.Z)

            'Add by Suuri 20150323 Sta ------------
            If My.Settings.blnCTonly = False Then
                For Each C3DST As Common3DSingleTarget In MainFrm.objFBM.lstCommon3dST
                    HOperatorSet.AffineTransPoint3d(tmpHomMat3d,
                                          C3DST.realP3d.X, C3DST.realP3d.Y, C3DST.realP3d.Z,
                                          C3DST.realP3d.X, C3DST.realP3d.Y, C3DST.realP3d.Z)
                Next
            End If
            'Add by Suuri 20150323 End ------------

            'X軸あわせ（回転）

            HOperatorSet.HomMat3dIdentity(tmpHomMat3d)
            Dim Phi As Object = GetAngleRight(PX, New Point3D(New HTuple(1.0), New HTuple(0.0), New HTuple(0.0)), Plane.XY)

            HOperatorSet.HomMat3dRotateLocal(tmpHomMat3d, Phi, "z", tmpHomMat3d)
            ' HOperatorSet.HomMat3dRotateLocal(CamToRealHomMat3D, Phi, "z", CamToRealHomMat3D)
            HOperatorSet.HomMat3dInvert(tmpHomMat3d, tmpHomMat3d)
            ' HOperatorSet.HomMat3dInvert(CamToRealHomMat3D, CamToRealHomMat3D)
            For Each C3DCT As Common3DCodedTarget In MainFrm.objFBM.lstCommon3dCT
                For Each P3D As Point3D In C3DCT.lstRealP3d
                    HOperatorSet.AffineTransPoint3d(tmpHomMat3d, P3D.X, P3D.Y, P3D.Z, P3D.X, P3D.Y, P3D.Z)
                Next
            Next

            'Add by Suuri 20150323 Sta ------------
            If My.Settings.blnCTonly = False Then
                For Each C3DST As Common3DSingleTarget In MainFrm.objFBM.lstCommon3dST
                    HOperatorSet.AffineTransPoint3d(tmpHomMat3d,
                                          C3DST.realP3d.X, C3DST.realP3d.Y, C3DST.realP3d.Z,
                                          C3DST.realP3d.X, C3DST.realP3d.Y, C3DST.realP3d.Z)
                Next
            End If
            'Add by Suuri 20150323 End ------------

            HOperatorSet.HomMat3dCompose(tmpHomMat3d, CamToRealHomMat3D, CamToRealHomMat3D)

        ElseIf objCTCoord.XYorXZorYZ = 7 Then   'Nippo用 垂直軸優先 座標系
            If PG Is Nothing Or PX Is Nothing Or PY Is Nothing Or PZ Is Nothing Then
                log_Print("[座標系設定 XYorXZorYZ = 7] 指定されたポイントに値が設定されていません", "V")
                Exit Sub
            End If
            If PG.IsDBNULL Or PX.IsDBNULL Or PY.IsDBNULL Or PZ.IsDBNULL Then
                log_Print("[座標系設定 XYorXZorYZ = 7] 指定されたポイントに値が設定されていません", "V")
                Exit Sub
            End If

            '平面設定
            HOperatorSet.HomMat3dIdentity(CamToRealHomMat3D)
            TransCoordSystemGX_YHeimen(PG, PX, PY, PZ, CamToRealHomMat3D) 'PY = Pz1 , PZ = Pz2
            For Each C3DCT As Common3DCodedTarget In MainFrm.objFBM.lstCommon3dCT
                For Each P3D As Point3D In C3DCT.lstRealP3d
                    HOperatorSet.AffineTransPoint3d(CamToRealHomMat3D,
                                                    P3D.X, P3D.Y, P3D.Z,
                                                    P3D.X, P3D.Y, P3D.Z)
                Next
            Next
            If My.Settings.blnCTonly = False Then
                For Each C3DST As Common3DSingleTarget In MainFrm.objFBM.lstCommon3dST
                    HOperatorSet.AffineTransPoint3d(CamToRealHomMat3D,
                                          C3DST.realP3d.X, C3DST.realP3d.Y, C3DST.realP3d.Z,
                                          C3DST.realP3d.X, C3DST.realP3d.Y, C3DST.realP3d.Z)
                Next
            End If
        End If


    End Sub


    Private Sub CalcCoordMatNoUnit()
        Dim objCTCoord As CT_CoordSettingData = lstCT_Coord(0)
        '座標系定義用の点抽出
        Dim XY_PG As Point3D = Nothing
        Dim XY_PX As Point3D = Nothing
        Dim XY_PY As Point3D = Nothing
        Dim PG As Point3D = Nothing
        Dim PX As Point3D = Nothing
        Dim PY As Point3D = Nothing

        If objCTCoord.CT_ID1 > 0 Then
            XY_PG = lstTengun(objCTCoord.CT_ID1 - 1).GetFromTengunNoUnit
        End If
        If objCTCoord.CT_ID2 > 0 Then
            XY_PX = lstTengun(objCTCoord.CT_ID2 - 1).GetFromTengunNoUnit
        End If
        If objCTCoord.CT_ID3 > 0 Then
            XY_PY = lstTengun(objCTCoord.CT_ID3 - 1).GetFromTengunNoUnit
        End If
        If objCTCoord.CT_GenID > 0 Then
            PG = lstTengun(objCTCoord.CT_GenID - 1).GetFromTengunNoUnit
        End If
        If objCTCoord.CT_XID > 0 Then
            PX = lstTengun(objCTCoord.CT_XID - 1).GetFromTengunNoUnit
        End If
        If objCTCoord.CT_YID > 0 Then
            PY = lstTengun(objCTCoord.CT_YID - 1).GetFromTengunNoUnit
        End If
        If PG Is Nothing Or PX Is Nothing Or PY Is Nothing Then
            'H25.6.28　Yamada　コメントアウト

            'MsgBox("座標系定義に失敗しました")
            Exit Sub
        End If
        If objCTCoord.XYorXZorYZ = 0 Then

            '平面設定

            HOperatorSet.HomMat3dIdentity(CamToNoUnitHomMat3D)
            TransCoordSystem(PG, PX, PY, CamToNoUnitHomMat3D)

        Else
            If XY_PG Is Nothing Or XY_PX Is Nothing Or XY_PY Is Nothing Then
                'H25.6.28　Yamada　コメントアウト

                'MsgBox("座標系定義に失敗しました")
                Exit Sub
            End If
            '平面設定

            HOperatorSet.HomMat3dIdentity(CamToRealHomMat3D)
            TransCoordSystem(XY_PG, XY_PX, XY_PY, CamToRealHomMat3D)
            For Each C3DCT As Common3DCodedTarget In MainFrm.objFBM.lstCommon3dCT
                For Each P3D As Point3D In C3DCT.lstRealP3d
                    HOperatorSet.AffineTransPoint3d(CamToRealHomMat3D, P3D.X, P3D.Y, P3D.Z, P3D.X, P3D.Y, P3D.Z)
                Next
            Next

            '原点設定（平行移動）

            HOperatorSet.HomMat3dIdentity(CamToRealHomMat3D)
            HOperatorSet.HomMat3dTranslate(CamToRealHomMat3D, PG.X, PG.Y, PG.Z, CamToRealHomMat3D)
            HOperatorSet.HomMat3dInvert(CamToRealHomMat3D, CamToRealHomMat3D)

            For Each C3DCT As Common3DCodedTarget In MainFrm.objFBM.lstCommon3dCT
                For Each P3D As Point3D In C3DCT.lstRealP3d
                    HOperatorSet.AffineTransPoint3d(CamToRealHomMat3D, P3D.X, P3D.Y, P3D.Z, P3D.X, P3D.Y, P3D.Z)
                Next
            Next

            'Ｘ軸あわせ（回転）

            HOperatorSet.HomMat3dIdentity(CamToRealHomMat3D)
            Dim Phi As Object = GetAngleRight(PX, New Point3D(New HTuple(1.0), New HTuple(0.0), New HTuple(0.0)), Plane.XY)

            HOperatorSet.HomMat3dRotateLocal(CamToRealHomMat3D, Phi, "z", CamToRealHomMat3D)
            HOperatorSet.HomMat3dInvert(CamToRealHomMat3D, CamToRealHomMat3D)
            For Each C3DCT As Common3DCodedTarget In MainFrm.objFBM.lstCommon3dCT
                For Each P3D As Point3D In C3DCT.lstRealP3d
                    HOperatorSet.AffineTransPoint3d(CamToRealHomMat3D, P3D.X, P3D.Y, P3D.Z, P3D.X, P3D.Y, P3D.Z)
                Next
            Next

        End If

    End Sub



    ' 2013/05/28 suzuki 作成
    Private Sub ReadTenGunTeigiDB(Optional ByRef m_dbclass As CDBOperateOLE = Nothing)
        If lstTengun Is Nothing Then
            lstTengun = New List(Of TenGunTeigiTable)
        Else
            lstTengun.Clear()
        End If

        'システム設定DBへ接続

        Dim DBPath As String = My.Application.Info.DirectoryPath & "\計測システムフォルダ\" & strSystemMdbFile
        If m_dbclass Is Nothing Then
            If ConnectDB(DBPath, m_dbclass) = False Then
                MsgBox(strSystemMdbFile & "への接続に失敗しました。")
                Exit Sub
            End If
        End If

        '  Dim TGL As New List(Of TenGunTeigiTable)
        Dim TGD As New TenGunTeigiTable
        TGD.m_dbClass = m_dbclass
        lstTengun = TGD.GetDataToList()

        'システム設定DBへ切断
        DisConnectDB(m_dbclass)

    End Sub
    Public Sub GenTengunSingleToCT(ByRef objFI As FBMlib.FeatureImage, ByRef arrCT_Result() As CT_Data)
        arrCT_Result = Nothing
        If objFI.lstCommon3dST Is Nothing Then
            Exit Sub
        End If
        Dim intL As Integer
        Dim i As Integer = 0
        intL = objFI.lstCommon3dST.Count

        ReDim arrCT_Result(intL - 1)

        For Each C3DST As FBMlib.Common3DSingleTarget In objFI.lstCommon3dST
            Dim CTD As New CT_Data
            CTD.CT_dat = New Common3DCodedTarget
            CTD.CT_dat.PID = C3DST.TID
            CTD.CT_dat.lstRealP3d = New List(Of FBMlib.Point3D)
            CTD.CT_dat.lstRealP3d.Add(C3DST.realP3d)
            CTD.CT_dat.lstP3d = New List(Of FBMlib.Point3D)
            CTD.CT_dat.lstP3d.Add(C3DST.P3d)
            arrCT_Result(i) = CTD
            i += 1
        Next
    End Sub
    Private Sub CalcTenGun(ByVal blnSyori As Boolean)
        For Each objTGT As TenGunTeigiTable In lstTengun

            objTGT.CalcTenGun(blnSyori)

        Next

    End Sub

    Public Function GetTengunByGunID(ByVal ID As Integer) As CT_Data()
        GetTengunByGunID = Nothing
        For Each TGD As TenGunTeigiTable In lstTengun
            If ID = TGD.TenGunID Then
                Return TGD.Tengun
                Exit For
            End If
        Next

    End Function

    Public Function GetGunTeigiTableDataByGunID(ByVal ID As Integer) As TenGunTeigiTable
        GetGunTeigiTableDataByGunID = Nothing
        For Each TGD As TenGunTeigiTable In lstTengun
            If ID = TGD.TenGunID Then
                Return TGD
                Exit For
            End If
        Next

    End Function

    Private Sub WriteToSunpoSetting()
        For Each objTGT As TenGunTeigiTable In lstTengun
            If objTGT.flgOnlyOne = True Then
                For Each objSS As SunpoSetTable In WorksD.SunpoSetL
                    If objSS.GunID1 = objTGT.TenGunID Then
                        Try
                            objSS.CT_ID1 = objTGT.Tengun(0).CT_dat.PID
                        Catch ex As Exception

                        End Try
                    End If
                    If objSS.GunID2 = objTGT.TenGunID Then
                        Try
                            objSS.CT_ID2 = objTGT.Tengun(0).CT_dat.PID
                        Catch ex As Exception
                        End Try
                    End If
                    If objSS.GunID3 = objTGT.TenGunID Then
                        Try
                            objSS.CT_ID3 = objTGT.Tengun(0).CT_dat.PID
                        Catch ex As Exception
                        End Try

                    End If
                Next
            End If
        Next

    End Sub
    'Private Sub ReadSunpoSetting()
    '    Dim filename As String = My.Application.Info.DirectoryPath & "\" & strCT_SunpoSetting
    '    Dim fields As String()
    '    Dim delimiter As String = ","
    '    Using parser As New TextFieldParser(filename, System.Text.Encoding.Default)
    '        parser.SetDelimiters(delimiter)
    '        While Not parser.EndOfData
    '            ' Read in the fields for the current line
    '            fields = parser.ReadFields()
    '            ' Add code here to use data in fields variable.
    '            If IsNumeric(fields(0)) Then
    '                Dim objSunposet As New CT_SunpoSetting(fields)
    '                If objSunposet.CT_Active = 1 And objSunposet.TypeID = CommonTypeID Then
    '                    lstCT_Sunpo.Add(objSunposet)
    '                End If
    '            End If
    '        End While
    '    End Using
    'End Sub

    Public Sub CalcSunpo()
        TimeMonStart()

        If ScaleToMM = -1 Then
            TimeMonEnd()
            Exit Sub
        End If
        'For Each objSunpo As CT_SunpoSetting In lstCT_Sunpo
        '    objSunpo.CalcSunpoVal()
        'Next

        '20170208 baluu del start
        'nCircleNew = 0
        'ReDim gDrawCircleNew(0)
        'ReDim gDrawUserLines(0)
        'nUserLines = 0
        '20170208 baluu del end
        '20170208 baluu add start
        If Not gDrawUserLines Is Nothing Then
            Dim tempuserlines As CUserLine() = gDrawUserLines.Clone
            nUserLines = 0
            ReDim gDrawUserLines(0)
            For Each uline As CUserLine In tempuserlines
                If Not uline Is Nothing Then
                    If Not uline.createType = 1 And Not uline.createType = 2 Then '20170306 baluu edit (Or -> And)
                        ReDim Preserve gDrawUserLines(nUserLines)
                        gDrawUserLines(nUserLines) = uline
                        nUserLines += 1
                    End If
                End If
            Next
        Else
            ReDim gDrawUserLines(0)
            nUserLines = 0
        End If

        If Not gDrawCircleNew Is Nothing Then
            Dim tempusercircles As Ccircle() = gDrawCircleNew.Clone
            nCircleNew = 0
            ReDim gDrawCircleNew(0)
            For Each ucircle As Ccircle In tempusercircles
                If Not ucircle Is Nothing Then
                    If Not ucircle.createType = 1 And Not ucircle.createType = 2 Then '20170306 baluu edit (Or -> And)
                        ReDim Preserve gDrawCircleNew(nCircleNew)
                        gDrawCircleNew(nCircleNew) = ucircle
                        nCircleNew += 1
                    End If
                End If
            Next
        Else
            ReDim gDrawCircleNew(0)
            nCircleNew = 0
        End If
        '20170208 baluu add end

        For Each objSunpo As SunpoSetTable In WorksD.SunpoSetL
            'Trace.WriteLine(objSunpo.SunpoMark & " " & objSunpo.SunpoName &
            '                " " & objSunpo.GunID1 & "->" & objSunpo.CT_ID1 &
            '                  " " & objSunpo.GunID2 & "->" & objSunpo.CT_ID2 &
            '                   " " & objSunpo.GunID3 & "->" & objSunpo.CT_ID3)
            If objSunpo.SunpoID = 9 Then
                objSunpo.SunpoID = objSunpo.SunpoID
            End If
            objSunpo.CalcSunpoVal()
            ' 20161109 baluu ADD stat
            If flgSUEOKI = False Then
                objSunpo.CalcSekVal()
            End If
            ' 20161109 baluu ADD end
            '(20140129 Tezuka ADD)
            If objSunpo.flgKeisan <> "0" Then
                ZukeiColLTypeChg(objSunpo)
                ZukeiColLTypeChgBySek(objSunpo)
            End If

            If (objSunpo.SunpoID = 24 Or objSunpo.SunpoID = 26) And (objSunpo.TypeID = 1 Or objSunpo.TypeID = 7) Then
                objSunpo.SunpoVal = objSunpo.SunpoVal - dblLevelAdj
            End If

            If objSunpo.SunpoCellName = "" Then
            Else
                Try
                    objSunpo.SunpoVal = objSunpo.SunpoVal + CDbl(objSunpo.SunpoCellName)
                Catch ex As Exception

                End Try

            End If
        Next
        TimeMonEnd()
    End Sub

    '(20140129 Tezuka ADD)
    '３Ｄ図形の線種、線色を寸法確認画面の設定値に変更する
    Public Function ZukeiColLTypeChg(ByRef objSnpu As SunpoSetTable) As Boolean

        ZukeiColLTypeChg = True

        If objSnpu.objZu IsNot Nothing Then
            For Each TT As Object In objSnpu.objZu
                If TT IsNot Nothing Then
                    If TT.GetType().Name.ToString = "CUserLine" Then    'オブジェクトが線

                        '色コードの設定
                        If objSnpu.ZU_colorID <> "" Then
                            If objSnpu.ZU_colorID = 0 Then
                                gDrawUserLines(TT.mid).colorCode = entset_line.color.code   'デフォルト色コード設定
                            Else
                                gDrawUserLines(TT.mid).colorCode = objSnpu.ZU_colorID       '色コード変更
                            End If
                        Else
                            objSnpu.ZU_colorID = gDrawUserLines(TT.mid).colorCode
                        End If

                        '線種コードの設定
                        If objSnpu.ZU_LineTypeID <> "" Then
                            If objSnpu.ZU_LineTypeID = 0 Then
                                gDrawUserLines(TT.mid).lineTypeCode = entset_line.linetype.code 'デフォルト線種コード設定
                            Else
                                gDrawUserLines(TT.mid).lineTypeCode = objSnpu.ZU_LineTypeID     '線種コード変更
                            End If
                        Else
                            objSnpu.ZU_LineTypeID = gDrawUserLines(TT.mid).lineTypeCode
                        End If
                        If objSnpu.flgOutZu = "1" Then
                            gDrawUserLines(TT.mid).blnDraw = True
                        Else
                            gDrawUserLines(TT.mid).blnDraw = False

                        End If
                        If objSnpu.ZU_layer <> "" Then
                            gDrawUserLines(TT.mid).layerName = objSnpu.ZU_layer

                        End If
                    ElseIf TT.GetType().Name.ToString = "Ccircle" Then  'オブジェクトが円

                        '色コードの設定
                        If objSnpu.ZU_colorID <> "" Then
                            If objSnpu.ZU_colorID = 0 Then
                                gDrawCircleNew(TT.mid).colorCode = entset_circle.color.code   'デフォルト色コード設定
                            Else
                                gDrawCircleNew(TT.mid).colorCode = objSnpu.ZU_colorID       '色コード変更
                            End If
                        Else
                            objSnpu.ZU_colorID = gDrawCircleNew(TT.mid).colorCode
                        End If

                        '線種コードの設定
                        If objSnpu.ZU_LineTypeID <> "" Then
                            If objSnpu.ZU_LineTypeID = 0 Then
                                gDrawCircleNew(TT.mid).lineTypeCode = entset_circle.linetype.code 'デフォルト線種コード設定
                            Else
                                gDrawCircleNew(TT.mid).lineTypeCode = objSnpu.ZU_LineTypeID     '線種コード変更
                            End If
                        Else
                            objSnpu.ZU_LineTypeID = gDrawCircleNew(TT.mid).lineTypeCode
                        End If
                        If objSnpu.flgOutZu = "1" Then
                            gDrawCircleNew(TT.mid).blnDraw = True
                        Else
                            gDrawCircleNew(TT.mid).blnDraw = False

                        End If

                        If objSnpu.ZU_layer <> "" Then
                            gDrawCircleNew(TT.mid).layerName = objSnpu.ZU_layer
                        End If
                    End If
                End If
            Next
        End If

    End Function

    '20161110 baluu ADD start
    Public Function ZukeiColLTypeChgBySek(ByRef objSnpu As SunpoSetTable) As Boolean

        ZukeiColLTypeChgBySek = True

        If objSnpu.objZuSek IsNot Nothing Then
            For Each TT As Object In objSnpu.objZuSek
                If TT IsNot Nothing Then
                    If TT.GetType().Name.ToString = "CUserLine" And gDrawUserLines.Count > 0 And gDrawUserLines(0) IsNot Nothing Then    'オブジェクトが線

                        '色コードの設定
                        If objSnpu.sek_ZU_colorID <> "" Then
                            If objSnpu.sek_ZU_colorID = 0 Then
                                gDrawUserLines(TT.mid).colorCode = entset_line.color.code   'デフォルト色コード設定
                            Else
                                gDrawUserLines(TT.mid).colorCode = objSnpu.sek_ZU_colorID       '色コード変更
                            End If
                        Else
                            objSnpu.sek_ZU_colorID = gDrawUserLines(TT.mid).colorCode
                        End If

                        '線種コードの設定
                        If objSnpu.sek_ZU_LineTypeID <> "" Then
                            If objSnpu.sek_ZU_LineTypeID = 0 Then
                                gDrawUserLines(TT.mid).lineTypeCode = entset_line.linetype.code 'デフォルト線種コード設定
                            Else
                                gDrawUserLines(TT.mid).lineTypeCode = objSnpu.sek_ZU_LineTypeID     '線種コード変更
                            End If
                        Else
                            objSnpu.sek_ZU_LineTypeID = gDrawUserLines(TT.mid).lineTypeCode
                        End If
                        If objSnpu.sek_flgOutZu = "1" Then
                            gDrawUserLines(TT.mid).blnDraw = True
                        Else
                            gDrawUserLines(TT.mid).blnDraw = False

                        End If
                        If objSnpu.ZU_layer <> "" Then
                            gDrawUserLines(TT.mid).layerName = objSnpu.sek_ZU_layer

                        End If
                    ElseIf TT.GetType().Name.ToString = "Ccircle" And gDrawCircleNew.Count > 0 Then  'オブジェクトが円

                        '色コードの設定
                        If objSnpu.sek_ZU_colorID <> "" Then
                            If objSnpu.sek_ZU_colorID = 0 Then
                                gDrawCircleNew(TT.mid).colorCode = entset_circle.color.code   'デフォルト色コード設定
                            Else
                                gDrawCircleNew(TT.mid).colorCode = objSnpu.sek_ZU_colorID       '色コード変更
                            End If
                        Else
                            objSnpu.sek_ZU_colorID = gDrawCircleNew(TT.mid).colorCode
                        End If

                        '線種コードの設定
                        If objSnpu.sek_ZU_LineTypeID <> "" Then
                            If objSnpu.sek_ZU_LineTypeID = 0 Then
                                gDrawCircleNew(TT.mid).lineTypeCode = entset_circle.linetype.code 'デフォルト線種コード設定
                            Else
                                gDrawCircleNew(TT.mid).lineTypeCode = objSnpu.sek_ZU_LineTypeID     '線種コード変更
                            End If
                        Else
                            objSnpu.sek_ZU_LineTypeID = gDrawCircleNew(TT.mid).lineTypeCode
                        End If
                        If objSnpu.sek_flgOutZu = "1" Then
                            gDrawCircleNew(TT.mid).blnDraw = True
                        Else
                            gDrawCircleNew(TT.mid).blnDraw = False

                        End If

                        If objSnpu.ZU_layer <> "" Then
                            gDrawCircleNew(TT.mid).layerName = objSnpu.sek_ZU_layer
                        End If
                    End If
                End If
            Next
        End If

    End Function
    '20161110 baluu ADD end

    'Private Sub OutSunpo(ByVal strFileName As String)

    '    Dim strKekka As String = ""
    '    strKekka = strKekka & "寸法番号,寸法記号,寸法箇所名,寸法値" & vbNewLine

    '    For Each objSunpo As CT_SunpoSetting In lstCT_Sunpo
    '        strKekka = strKekka & objSunpo.SunpoID &
    '            "," & objSunpo.SunpoMark &
    '            "," & objSunpo.SunpoName &
    '            "," & objSunpo.SunpoVal & vbNewLine
    '    Next
    '    My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & strFileName, strKekka, False)
    '    System.Diagnostics.Process.Start(My.Application.Info.DirectoryPath & "\" & strFileName)
    '    ' Shell("excel.exe" & " " & My.Application.Info.DirectoryPath & "\" & strFileName, AppWinStyle.MaximizedFocus, False)

    'End Sub

    Private Sub OutRealP3dKekka(ByVal strFileName As String, ByRef objFI As FBMlib.FeatureImage)
        Dim strKekka As String = ""
        For Each C3DCT As FBMlib.Common3DCodedTarget In objFI.lstCommon3dCT
            If C3DCT.flgUsable = True Then
                strKekka = strKekka & C3DCT.PID & "," & C3DCT.lstRealP3d.Item(0).X.D & "," & C3DCT.lstRealP3d.Item(0).Y.D & "," & C3DCT.lstRealP3d.Item(0).Z.D & vbNewLine
            End If
        Next
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & strFileName, strKekka, False)
    End Sub
    Private Sub OutNormalVector(ByVal strFileName As String)
        Dim strKekka As String = ""
        For Each CTD As CT_Data In arrCTData
            If Not CTD Is Nothing Then
                strKekka = strKekka & CTD.CT_dat.PID & "," & CTD.CT_dat.NormalV.X.D & "," & CTD.CT_dat.NormalV.Y.D & "," & CTD.CT_dat.NormalV.Z.D & vbNewLine
            End If
        Next
        My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & strFileName, strKekka, False)
    End Sub
    Public Sub YCM_Offset_GenData(ByRef objFI As FBMlib.FeatureImage)
        Dim i As Integer = 0

        For Each CTD As CT_Data In arrCTData
            CTD = New CT_Data
        Next

        If lstCT_Scale Is Nothing Then
            lstCT_Scale = New List(Of CT_ScaleSettingData)
        Else
            lstCT_Scale.Clear()
        End If
        If lstCT_Coord Is Nothing Then
            lstCT_Coord = New List(Of CT_CoordSettingData)
        Else
            lstCT_Coord.Clear()
        End If
        'If lstCT_Sunpo Is Nothing Then
        '    lstCT_Sunpo = New List(Of CT_SunpoSetting)
        'Else
        '    lstCT_Sunpo.Clear()
        'End If



        Read_Type()

        'Read_CTBunrui()            '2013/05/15 DEL
        'Read_CTScaleSetting()      '2013/05/15 DEL
        Read_CTBunruiDB()           '2013/05/15 ADD
        Read_CTScaleSettingDB()     '2013/05/15 ADD

        For Each C3DCT As FBMlib.Common3DCodedTarget In MainFrm.objFBM.lstCommon3dCT
            If C3DCT.flgUsable = True Then
                arrCTData(C3DCT.PID) = New CT_Data
                arrCTData(C3DCT.PID).CT_dat = C3DCT
                arrCTData(C3DCT.PID).CToffsetval = arrCToffsets(C3DCT.PID)
                For Each CTSC As CT_ScaleSettingData In lstCT_Scale
                    If C3DCT.PID = CTSC.CT_ID1 Then
                        CTSC.CT1 = C3DCT
                    End If
                    If C3DCT.PID = CTSC.CT_ID2 Then
                        CTSC.CT2 = C3DCT
                    End If
                Next
            End If
        Next
        ScaleToMM = -1
        For Each CTSC As CT_ScaleSettingData In lstCT_Scale
            If Not CTSC.CT1 Is Nothing Then
                If Not CTSC.CT2 Is Nothing Then
                    Dim dblNoUnitDist As Double
                    CTSC.CT1.lstP3d(0).GetDisttoOtherPose(CTSC.CT2.lstP3d(0), dblNoUnitDist)
                    ScaleToMM = CTSC.CT_kan_dist / dblNoUnitDist
                    Exit For
                End If
            End If
        Next
        If ScaleToMM <> -1 Then
            For Each C3DCT As FBMlib.Common3DCodedTarget In MainFrm.objFBM.lstCommon3dCT
                If C3DCT.flgUsable = True Then
                    C3DCT.lstRealP3d.Clear()
                    For i = 0 To FBMlib.CodedTarget.CTnoSTnum - 1
                        C3DCT.lstRealP3d.Add(C3DCT.lstP3d(i).SetScale2(ScaleToMM))
                    Next
                End If
            Next
        Else
            'H25.6.28Yamadaコメントアウト

            'MsgBox("スケール設定が失敗しました。!")
            Exit Sub
        End If

        OutRealP3dKekka("RealP3D_ScaleSetMonitor.csv", objFI)
        CalcOffSet(True, objFI)
        OutRealP3dKekka("RealP3D_OffsetMonitor.csv", objFI)


        'ReadCoordSetting()
        ReadCoordSettingDB()
        CalcCoordMat()
        OutRealP3dKekka("RealP3D_CoordSetMonitor.csv", objFI)

        ' ReadTenGunTeigi()
        'ReadTenGunTeigiDB()         '2013/05/28 Suzuki ADD 

        'H25.6.28Yamada；False→MsgBox表示なし

        'CalcTenGun(True)
        CalcTenGun(False)

        ' ReadSunpoSetting()
        ' My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & "MMM.txt", lstCT_Sunpo.Count, True)
        WriteToSunpoSetting()

        CalcSunpo()

    End Sub

    Private Function YCM_Scale(ByRef objFI As FBMlib.FeatureImage, Optional blnNoRedim As Boolean = False) As Boolean
        YCM_Scale = False
        If lstCT_Scale Is Nothing Then
            lstCT_Scale = New List(Of CT_ScaleSettingData)
        Else
            lstCT_Scale.Clear()
        End If

        If blnNoRedim = False Then
            ReDim arrCTData(TargetMaxNumber)
        End If

        YCM_ReadSekkeiKeisokuDataTable(MainFrm.objFBM.ProjectPath & "\" & YCM_MDB)

        Read_CTBunruiDB()           '2013/05/15 ADD
        '20130824 SUURI  ADD START　スケールテーブルを見ないでSunpoSetテーブルを見てスケール設定するように変更
        ' Read_CTScaleSettingDB()     '2013/05/15 ADD　－＞DELETE
        For Each objSunpo As SunpoSetTable In WorksD.SunpoSetL
            If objSunpo.SunpoMark = "S12" Or objSunpo.SunpoMark = "S34" Or objSunpo.SunpoMark = "S56" Or objSunpo.SunpoMark = "S-A" Or objSunpo.SunpoMark = "S-B" Or objSunpo.SunpoMark = "S-C" Or objSunpo.SunpoMark = "S-D" Or objSunpo.SunpoMark = "S-E" Then
                Dim lstCT_ScaleD As New CT_ScaleSettingData
                lstCT_ScaleD.CT_ID1 = objSunpo.CT_ID1
                lstCT_ScaleD.CT_ID2 = objSunpo.CT_ID2
                lstCT_ScaleD.CT_kan_dist = objSunpo.KiteiVal
                lstCT_Scale.Add(lstCT_ScaleD)
            End If
        Next
        '20130824 SUURI  ADD　END
        For Each C3DCT As FBMlib.Common3DCodedTarget In objFI.lstCommon3dCT

            'Rep By Suuri 20150323 Sta ---------- 
            'If C3DCT.flgUsable = True And C3DCT.PID <= 500 Then
            If C3DCT.flgUsable = True Then
                'Rep By Suuri 20150323 End ----------

                arrCTData(C3DCT.PID) = New CT_Data
                arrCTData(C3DCT.PID).CT_dat = C3DCT
                If arrCToffsets(C3DCT.PID) Is Nothing Then
                    Continue For
                End If
                arrCTData(C3DCT.PID).CToffsetval = arrCToffsets(C3DCT.PID)

                C3DCT.K1 = arrCTData(C3DCT.PID).CToffsetval.kubun1
                C3DCT.K2 = arrCTData(C3DCT.PID).CToffsetval.kubun2
                C3DCT.info = arrCTData(C3DCT.PID).CToffsetval.info '20160316 add
                C3DCT.CT_No = arrCTData(C3DCT.PID).CToffsetval.CT_No '20160317 add
                For Each CTSC As CT_ScaleSettingData In lstCT_Scale
                    If C3DCT.PID = CTSC.CT_ID1 Then
                        CTSC.CT1 = C3DCT
                    End If
                    If C3DCT.PID = CTSC.CT_ID2 Then
                        CTSC.CT2 = C3DCT
                    End If
                Next
            End If
        Next
        ScaleToMM = -1

        'SUSANO ADD START 20160525
        '手動スケール設定されたかどうかを調べる
        'If ScaleToMM = -1 Then
        If IO.File.Exists(MainFrm.objFBM.ProjectPath & "\" & YCM_TEMP_MDB) = True Then
            YCM_ReadSystemscalesettingAcs(MainFrm.objFBM.ProjectPath & "\" & YCM_TEMP_MDB)
            YCM_ReadSystemscalesettingAcsvalue(MainFrm.objFBM.ProjectPath & "\" & YCM_TEMP_MDB)
            YCM_ReadSystemscalesettingAcsP1_2(MainFrm.objFBM.ProjectPath & "\" & YCM_TEMP_MDB) '13.1
            If nscalesettingvalue = 0 Then
                For Each CTSC As CT_ScaleSettingData In lstCT_Scale
                    If Not CTSC.CT1 Is Nothing Then
                        If Not CTSC.CT2 Is Nothing Then
                            Dim dblNoUnitDist As Double
                            CTSC.CT1.lstP3d(0).GetDisttoOtherPose(CTSC.CT2.lstP3d(0), dblNoUnitDist)
                            ScaleToMM = CTSC.CT_kan_dist / dblNoUnitDist
                            sys_ScaleInfo.p1 = New CLookPoint(CDbl(CTSC.CT1.lstP3d(0).X), CDbl(CTSC.CT1.lstP3d(0).Y), CDbl(CTSC.CT1.lstP3d(0).Z))
                            sys_ScaleInfo.p2 = New CLookPoint(CDbl(CTSC.CT2.lstP3d(0).X), CDbl(CTSC.CT2.lstP3d(0).Y), CDbl(CTSC.CT2.lstP3d(0).Z))
                            sys_ScaleInfo.p1.LabelName = CTSC.CT1.currentLabel
                            sys_ScaleInfo.p2.LabelName = CTSC.CT2.currentLabel
                            GetPosFromLabelName(CLookPoint.posTypeMode.All, CTSC.CT1.currentLabel, sys_ScaleInfo.p1)
                            GetPosFromLabelName(CLookPoint.posTypeMode.All, CTSC.CT2.currentLabel, sys_ScaleInfo.p2)
                            sys_ScaleInfo.len = CTSC.CT_kan_dist
                            sys_ScaleInfo.scale = ScaleToMM
                            Exit For
                        End If
                    End If
                Next
            Else
                sys_ScaleInfo.len = System_scalesetting(0, nscalesettingvalue - 1)
                sys_ScaleInfo.scale = System_scalesettingvalue(0, nscalesettingvalue - 1)
                sys_ScaleInfo.p1 = getDrawPointByMID(sys_ScaleInfo.p1.mid)
                sys_ScaleInfo.p2 = getDrawPointByMID(sys_ScaleInfo.p2.mid)
                ScaleToMM = sys_ScaleInfo.scale
            End If
        ElseIf IO.File.Exists(MainFrm.objFBM.ProjectPath & "\" & YCM_MDB) = True Then
            YCM_ReadSystemscalesettingAcs(MainFrm.objFBM.ProjectPath & "\" & YCM_MDB)
            YCM_ReadSystemscalesettingAcsvalue(MainFrm.objFBM.ProjectPath & "\" & YCM_MDB)
            YCM_ReadSystemscalesettingAcsP1_2(MainFrm.objFBM.ProjectPath & "\" & YCM_MDB) '13.1
            If nscalesettingvalue = 0 Then
                For Each CTSC As CT_ScaleSettingData In lstCT_Scale
                    If Not CTSC.CT1 Is Nothing Then
                        If Not CTSC.CT2 Is Nothing Then
                            Dim dblNoUnitDist As Double
                            CTSC.CT1.lstP3d(0).GetDisttoOtherPose(CTSC.CT2.lstP3d(0), dblNoUnitDist)
                            ScaleToMM = CTSC.CT_kan_dist / dblNoUnitDist
                            sys_ScaleInfo.p1 = New CLookPoint(CDbl(CTSC.CT1.lstP3d(0).X), CDbl(CTSC.CT1.lstP3d(0).Y), CDbl(CTSC.CT1.lstP3d(0).Z))
                            sys_ScaleInfo.p2 = New CLookPoint(CDbl(CTSC.CT2.lstP3d(0).X), CDbl(CTSC.CT2.lstP3d(0).Y), CDbl(CTSC.CT2.lstP3d(0).Z))
                            sys_ScaleInfo.p1.LabelName = CTSC.CT1.currentLabel
                            sys_ScaleInfo.p2.LabelName = CTSC.CT2.currentLabel
                            GetPosFromLabelName(CLookPoint.posTypeMode.All, CTSC.CT1.currentLabel, sys_ScaleInfo.p1)
                            GetPosFromLabelName(CLookPoint.posTypeMode.All, CTSC.CT2.currentLabel, sys_ScaleInfo.p2)
                            sys_ScaleInfo.len = CTSC.CT_kan_dist
                            sys_ScaleInfo.scale = ScaleToMM
                            Exit For
                        End If
                    End If
                Next
            Else
                sys_ScaleInfo.len = System_scalesetting(0, nscalesettingvalue - 1)
                sys_ScaleInfo.scale = System_scalesettingvalue(0, nscalesettingvalue - 1)
                sys_ScaleInfo.p1 = getDrawPointByMID(sys_ScaleInfo.p1.mid)
                sys_ScaleInfo.p2 = getDrawPointByMID(sys_ScaleInfo.p2.mid)
                ScaleToMM = sys_ScaleInfo.scale
            End If
        End If

        '  End If
        'SUSANO ADD END 20160525
        If ScaleToMM <> -1 Then
            For Each C3DCT As FBMlib.Common3DCodedTarget In objFI.lstCommon3dCT
                If C3DCT.flgUsable = True Then
                    C3DCT.lstRealP3d.Clear()
                    For i = 0 To FBMlib.CodedTarget.CTnoSTnum - 1
                        C3DCT.lstRealP3d.Add(C3DCT.lstP3d(i).SetScale2(ScaleToMM))
                    Next
                End If
            Next
            If My.Settings.blnCTonly = False Then
                For Each C3DST As Common3DSingleTarget In objFI.lstCommon3dST
                    ' HOperatorSet.AffineTransPoint3D(CamToRealHomMat3D, C3DST.P3d.X, C3DST.P3d.Y, C3DST.P3d.Z, C3DST.P3d.X, C3DST.P3d.Y, C3DST.P3d.Z)
                    C3DST.realP3d.CopyToMe(C3DST.P3d.SetScale2(ScaleToMM))
                Next
            End If
            YCM_Scale = True
        Else
            'H25.6.28Yamadaコメントアウト

            'MsgBox("スケール設定が失敗しました。!")
            'SUSANO ADD START 20160525
            Dim strMess As String = "自動スケール設定に失敗しました。" & vbNewLine
            For Each Sc As CT_ScaleSettingData In lstCT_Scale
                strMess = strMess & "基準定規(CT" & Sc.CT_ID1 & ",CT" & Sc.CT_ID2 & ")" & vbNewLine
            Next
            strMess = strMess & "が正しく設置されていることを確認してください。" & vbNewLine
            strMess = strMess & "手動でスケール設定を行いますか？（設定を行わない場合再撮影が必要です）"
            If MsgBox(strMess, MsgBoxStyle.YesNo, "確認") = MsgBoxResult.Yes Then
                flgManualScaleSetting = True
            End If
            'SUSANO ADD END 20160525
            Exit Function
        End If
        objFI.pScaleMM = ScaleToMM

        For Each C3DCT As FBMlib.Common3DCodedTarget In objFI.lstCommon3dCT
            If C3DCT.flgUsable = True Then
#If True Then
                arrCTData(C3DCT.PID).CtDatToSekDat(GetSekkeiKeisokuTaiyoByID(C3DCT.PID))
#End If
            End If
        Next



        OutRealP3dKekka("RealP3D_ScaleSetMonitor.csv", objFI)
    End Function

    Public Function YCM_ScaleAndOffset(ByRef objFI As FBMlib.FeatureImage, Optional blnNoRedim As Boolean = False) As Boolean
        YCM_ScaleAndOffset = True
        If YCM_Scale(objFI, blnNoRedim) = False Then    '★
            YCM_ScaleAndOffset = False
            Exit Function
        End If

        CalcOffSet(True, objFI)
        'SUEOKI　専用処理　測定値補正
        'If flgSUEOKI = True Then
        '    SueokiHosei(objFI)
        'End If
        OutRealP3dKekka("RealP3D_OffsetMonitor.csv", objFI)
    End Function
    Public Sub YCM_Offset_GenData2(ByRef objFI As FBMlib.FeatureImage)
        TimeMonStart()

        Dim i As Integer = 0

        'For Each CTD As CT_Data In arrCTData
        '    CTD = New CT_Data
        'Next

        If YCM_ScaleAndOffset(objFI) = False Then
            TimeMonEnd()
            Exit Sub
        End If


        ' ReadTenGunTeigi()
        ' ReadTenGunTeigiDB()      ’工事データを開く及び新規時に読込む   '2013/05/28 Suzuki ADD 000000
        CalcTenGun(False)
        'ReadCoordSetting()
        ReadCoordSettingDB()
        CalcCoordMat()
        TransCoordSys()

        If flgSUEOKI = True Then
            SueokiHosei(objFI)
        End If

        OutRealP3dKekka("RealP3D_CoordSetMonitor.csv", objFI)
        CalcOffSet(False, objFI)
        'H25.6.28Yamada；False→MsgBox表示なし

        'CalcTenGun(True)
        GenTengunSingleToCT(objFI, arrSTtoCTData)
        CalcTenGun(False)
        '  ReadSunpoSetting()

        WriteToSunpoSetting()

        CalcSunpo()

        TimeMonEnd()
    End Sub
    Public Sub YCM_Offset_GenDataNoCoordTrans(ByRef objFI As FBMlib.FeatureImage)
        Dim i As Integer = 0

        'For Each CTD As CT_Data In arrCTData
        '    CTD = New CT_Data
        'Next

        If YCM_ScaleAndOffset(objFI) = False Then
            Exit Sub
        End If


        ' ReadTenGunTeigi()
        ' ReadTenGunTeigiDB()      ’工事データを開く及び新規時に読込む   '2013/05/28 Suzuki ADD 000000
        CalcTenGun(False)
        'ReadCoordSetting()
        ReadCoordSettingDB()
        ' CalcCoordMat()
        ' TransCoordSys()

        If flgSUEOKI = True Then
            SueokiHosei(objFI)
        End If

        OutRealP3dKekka("RealP3D_CoordSetMonitor.csv", objFI)
        CalcOffSet(False, objFI)
        'H25.6.28Yamada；False→MsgBox表示なし

        'CalcTenGun(True)
        GenTengunSingleToCT(objFI, arrSTtoCTData)
        CalcTenGun(False)
        '  ReadSunpoSetting()

        WriteToSunpoSetting()

        CalcSunpo()

    End Sub

    Public Sub YCM_Offset_ReadData(ByRef objFI As FBMlib.FeatureImage)
        Dim i As Integer = 0

        'For Each CTD As CT_Data In arrCTData
        '    CTD = New CT_Data
        'Next


        If YCM_Scale(objFI) = False Then
            '   MsgBox("スケール設定に失敗しました。", MsgBoxStyle.OkOnly, "確認")
            Exit Sub
        End If
        CalcOffSet(False, objFI)
        OutRealP3dKekka("RealP3D_OffsetMonitor.csv", objFI)

        GenTengunSingleToCT(objFI, arrSTtoCTData)
        ' ReadTenGunTeigi()
        ' ReadTenGunTeigiDB()   ’ 工事データを開く及び新規時に読込む     '2013/05/28 Suzuki ADD 000000
        CalcTenGun(False)
        'ReadCoordSetting()
        ReadCoordSettingDB()
        'CalcCoordMat()
        'TransCoordSys()
        'OutRealP3dKekka("RealP3D_CoordSetMonitor.csv", objFI)
        'CalcOffSet(False, objFI)
        'H25.6.28Yamada；False→MsgBox表示なし
        'CalcTenGun(True)
        'GenTengunSingleToCT(objFI, arrSTtoCTData)
        CalcTenGun(False)
        '  ReadSunpoSetting()

        WriteToSunpoSetting()

        CalcSunpo()

    End Sub
    Private Sub SueokiHosei(ByRef objFI As FeatureImage)

        SueokiHoseiNaibuByZ(objFI, My.Application.Info.DirectoryPath & "\SueokiHoseiAllways.csv")
        SueokiHoseiNaibu(objFI, My.Application.Info.DirectoryPath & "\SueokiHoseiOnlyOne.csv")
        Try
            My.Computer.FileSystem.DeleteFile(My.Application.Info.DirectoryPath & "\SueokiHoseiOnlyOne.csv", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub SueokiHoseiNaibu(ByRef objFI As FeatureImage, ByVal strFHoseName As String)
        Dim fileContents As String
        Try
            fileContents = My.Computer.FileSystem.ReadAllText(strFHoseName)
        Catch ex As Exception
            Exit Sub
        End Try

        Dim strSplit() As String
        strSplit = fileContents.Split(",")
        Dim moveX As Double = CDbl(strSplit(0))
        Dim baseX As Double = CDbl(strSplit(1))
        Dim moveY As Double = CDbl(strSplit(2))
        Dim baseY As Double = CDbl(strSplit(3))
        Dim scaleX As Double = (1 + moveX / baseX)
        Dim scaleY As Double = (1 + moveY / baseY)
        For Each C3DCT As FBMlib.Common3DCodedTarget In objFI.lstCommon3dCT
            If C3DCT.flgUsable = True Then
                'C3DCT.lstRealP3d.Clear()
                For i = 0 To FBMlib.CodedTarget.CTnoSTnum - 1
                    C3DCT.lstRealP3d(i).SetScaleXY(scaleX, scaleY)
                Next
            End If
        Next
        If My.Settings.blnCTonly = False Then
            For Each C3DST As Common3DSingleTarget In objFI.lstCommon3dST
                C3DST.realP3d.SetScaleXY(scaleX, scaleY)
            Next
        End If
    End Sub

    'SUSANO ADD START 20150929
    Private Sub SueokiHoseiNaibuByZ(ByRef objFI As FeatureImage, ByVal strFHoseName As String)
        Dim fileContents As String
        Try
            fileContents = My.Computer.FileSystem.ReadAllText(strFHoseName)
        Catch ex As Exception
            Exit Sub
        End Try

        Dim strSplit() As String
        strSplit = fileContents.Split(",")
        Dim Dx1 As Double = CDbl(strSplit(0))
        Dim Lx1 As Double = CDbl(strSplit(1))
        Dim Zx1 As Double = CDbl(strSplit(2))
        Dim Dx2 As Double = CDbl(strSplit(3))
        Dim Lx2 As Double = CDbl(strSplit(4))
        Dim Zx2 As Double = CDbl(strSplit(5))

        Dim Dy1 As Double = CDbl(strSplit(6))
        Dim Ly1 As Double = CDbl(strSplit(7))
        Dim Zy1 As Double = CDbl(strSplit(8))
        Dim Dy2 As Double = CDbl(strSplit(9))
        Dim Ly2 As Double = CDbl(strSplit(10))
        Dim Zy2 As Double = CDbl(strSplit(11))

        For Each C3DCT As FBMlib.Common3DCodedTarget In objFI.lstCommon3dCT
            If C3DCT.flgUsable = True Then
                'C3DCT.lstRealP3d.Clear()
                If C3DCT.PID = 402 Then
                    Debug.Print("402")
                End If
                For i = 0 To FBMlib.CodedTarget.CTnoSTnum - 1
                    C3DCT.lstRealP3d(i).SetScaleXY(Dx1, Lx1, Zx1, Dx2, Lx2, Zx2, Dy1, Ly1, Zy1, Dy2, Ly2, Zy2)
                Next
            End If
        Next
        If My.Settings.blnCTonly = False Then
            For Each C3DST As Common3DSingleTarget In objFI.lstCommon3dST
                C3DST.realP3d.SetScaleXY(Dx1, Lx1, Zx1, Dx2, Lx2, Zx2, Dy1, Ly1, Zy1, Dy2, Ly2, Zy2)
            Next
        End If
    End Sub
    'SUSANO ADD END 20150929
    Public lstSek_KeiData As List(Of clsSekkeiKeisokuTaiyo)


    Private Function YCM_ReadSekkeiKeisokuDataTable(ByVal strDBPath As String) As Integer
        Dim clsOPe As New CDBOperate
        Dim adoRet As ADODB.Recordset
        If clsOPe.ConnectDB(strDBPath) = False Then
            YCM_ReadSekkeiKeisokuDataTable = -1
        End If
        Dim strSQL As String

        If (Not ExistsTable(clsOPe, "[SekkeiKeisokuData]")) Then
            Exit Function
        End If
        If lstSek_KeiData Is Nothing Then
            lstSek_KeiData = New List(Of clsSekkeiKeisokuTaiyo)
        Else
            lstSek_KeiData.Clear()
        End If


        strSQL = "SELECT * From SekkeiKeisokuData"
        adoRet = clsOPe.CreateRecordset(strSQL)
        If adoRet Is Nothing Then
            Exit Function
        End If
        If adoRet.RecordCount > 0 Then
            Do Until adoRet.EOF
                With lstSek_KeiData
                    Dim objNew As New clsSekkeiKeisokuTaiyo
                    objNew.No = adoRet("ID").Value
                    objNew.SekkeiLabel1 = adoRet("Sekkei").Value
                    objNew.SokutenMei1 = adoRet("SokutenMei").Value
                    objNew.SekkeiTenX = adoRet("SekkeiTenX").Value
                    objNew.SekkeiTenY = adoRet("SekkeiTenY").Value
                    objNew.SekkeiTenZ = adoRet("SekkeiTenZ").Value
                    objNew.KeisokuTenX = adoRet("KeisokuTenX").Value
                    objNew.KeisokuTenY = adoRet("KeisokuTenY").Value
                    objNew.KeisokuTenZ = adoRet("KeisokuTenZ").Value
                    objNew.IchiAwaseXw = adoRet("IchiAwaseXw").Value
                    objNew.IchiAwaseYw = adoRet("IchiAwaseYw").Value
                    objNew.IchiAwaseZw = adoRet("IchiAwaseZw").Value
                    objNew.DiffX = adoRet("DiffX").Value
                    objNew.DiffY = adoRet("DiffY").Value
                    objNew.DiffZ = adoRet("DiffZ").Value
                    objNew.DiffL = adoRet("DiffL").Value
                    objNew.SokutenID = adoRet("SokutenID").Value

                    .Add(objNew)
                End With
                adoRet.MoveNext()
            Loop

        End If
        clsOPe.DisConnectDB()
        YCM_ReadSekkeiKeisokuDataTable = 0


    End Function

    Private Function GetSekkeiKeisokuTaiyoByID(ByVal IID As String) As clsSekkeiKeisokuTaiyo
        GetSekkeiKeisokuTaiyoByID = Nothing
        If lstSek_KeiData Is Nothing Then
            Exit Function
        End If
        For Each SKTI As clsSekkeiKeisokuTaiyo In lstSek_KeiData
            If SKTI.SokutenID = IID Then
                GetSekkeiKeisokuTaiyoByID = SKTI
                Exit Function
            End If
        Next
    End Function
End Module

Public Class clsSekkeiKeisokuTaiyo
    Public Property No() As Integer             'No
    Public Property SekkeiLabel1() As String     '設計ラベル
    ' Public Property SokutenMei1() As String      '測点名
    Private _SokutenMei1 As String
    Public Property SokutenMei1() As String '測点名
        Get
            Return _SokutenMei1
        End Get
        Set(ByVal value As String)
            _SokutenMei1 = value
        End Set
    End Property
    Private _SokutenID As String
    Public Property SokutenID() As String '計測点ID
        Get
            Return _SokutenID
        End Get
        Set(ByVal value As String)
            _SokutenID = value
        End Set
    End Property

    Public Property SekkeiTenX() As Double      '設計点X
    Public Property SekkeiTenY() As Double      '設計点Y
    Public Property SekkeiTenZ() As Double      '設計点Z
    Public Property KeisokuTenX() As Double     '計測点X
    Public Property KeisokuTenY() As Double   '計測点Y
    Public Property KeisokuTenZ() As Double    '計測点Z
    Public Property IchiAwaseXw() As Double    '位置合わせの重みX
    Public Property IchiAwaseYw() As Double '位置合わせの重みY
    Public Property IchiAwaseZw() As Double '位置合わせの重みZ
    Public Property DiffX() As Double '差（計測点-設計点）X
    Public Property DiffY() As Double '差（計測点-設計点）Y
    Public Property DiffZ() As Double '差（計測点-設計点）Z
    Public Property DiffL() As Double '差（計測点-設計点）L



    Private _IsMissMatch As Boolean
    Public Property IsMissMatch() As Boolean
        Get

            If SokutenMei1 = "" Then
                Return True
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            _IsMissMatch = value
        End Set
    End Property

    Public Shared Property SekkeiLabel As System.Collections.ObjectModel.ObservableCollection(Of String)
    Public Shared Property SokutenMei As List(Of String)

    Public Sub New()

    End Sub



End Class
