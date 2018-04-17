Public Module Sys_Setting
    Public Structure ModelColor
        Dim strName As String
        Dim code As Integer
        Dim dbl_red As Double
        Dim dbl_green As Double
        Dim dbl_blue As Double
        Dim strHalconColorName As String '20121127 追加　ＳＵＵＲＩ
    End Structure
    Public Structure LineType
        Dim strName As String
        Dim code As Integer
        Dim kyori As Integer
        Dim pattern As String
        Dim strHalconLineType As Integer   '20150401 Tezuka ADD
    End Structure
    Public Structure EntSetting
        Dim strTypeName As String
        Dim blnVisiable As Boolean
        Dim color As ModelColor
        Dim color2 As ModelColor
        Dim color3 As ModelColor
        Dim linetype As LineType
        Dim maxscale As Double
        Dim minscale As Double
        Dim screensize As Double
        Dim konomi As Double
        Dim strComent As String
        Dim linewidth As Double
        Dim layerName As String  'レイヤ名

    End Structure
    'Public Class EntSetting
    '    Public strTypeName As String
    '    Public blnVisiable As Boolean
    '    Public color As ModelColor
    '    Public color2 As ModelColor
    '    Public color3 As ModelColor
    '    Public linetype As LineType
    '    Public maxscale As Double
    '    Public minscale As Double
    '    Public screensize As Double
    '    Public konomi As Double
    '    Public strComent As String
    '    Public linewidth As Double
    '    Public layerName As String  'レイヤ名

    'End Class
    Public Structure SysLineTypeRecord
        Dim strLineTypeName As String
        Dim code As Integer
        Dim kyori As Integer
        Dim pattern As String
    End Structure
    Public Structure SysColorRecord
        Dim strColorName As String
        Dim code As Integer
        Dim red As Double
        Dim green As Double
        Dim blue As Double
        Dim strComent As String
        Dim strHalconColorName As String '20121127 追加　ＳＵＵＲＩ
    End Structure
    Public Structure SysScaleInfo
        Public p1 As CLookPoint
        Public p2 As CLookPoint
        Dim len As Double
        Dim scale As Double
    End Structure
    Public Structure SysCoordChange
        Dim pOrg As CLookPoint
        Dim p1 As CLookPoint
        Dim p2 As CLookPoint
        Dim strP1XYZ As String
        Dim strP2XYZ As String
        Dim intOxyais As Integer
        Dim mat() As Double
        Dim mat_geo As GeoMatrix
    End Structure
    Public Structure SysLabelling
        Dim p1 As CLookPoint
        Dim p2 As CLookPoint
        Dim p3 As CLookPoint
        Dim strCSVPath As String
        Dim blnsyslabel As Boolean
    End Structure
    Public Structure SysDrawCircle
        Dim iCombox As Integer
        Dim dblR As Double
    End Structure

    Public entset_ray As EntSetting         'レイ
    Public entset_point As EntSetting       '★20121115計測点
    Public CordTargetIsvisible As Boolean = False
    '================================================================================================20121115
    'Public entset_point1 As EntSetting       '★2012111420121114計測点(ターゲット面)/カメラを参考にすべきか？(レンズ、シャッターがない)
    'Public entset_point2 As EntSetting       '★2012111420121114計測点(中心円)
    'Public entset_point3 As EntSetting       '★2012111420121114計測点(十字線)
    '================================================================================================20121115
    Public entset_pointUser As EntSetting   '追加計測点（任意追加点）

    Public entset_camera As EntSetting      'カメラ
    Public entset_label As EntSetting       'ラベル
    Public entset_line As EntSetting        '任意図形（線）

    Public entset_scale_line As EntSetting  'スケールライン
    Public entset_circle As EntSetting      '任意図形（円）

    Public entset_line_CAD As EntSetting     'CAD図形（線）

    Public entset_circle_CAD As EntSetting   'CAD図形（円）

    Public sys_color() As SysColorRecord
    Public sys_LineType() As SysLineTypeRecord

    Public sys_ScaleInfo As SysScaleInfo
    Public sys_CoordInfo As SysCoordChange
    Public sys_Labeling As SysLabelling
    Public sys_DrawCircle As SysDrawCircle

    Public System_scalesetting(,) As String
    Public System_scalesettingvalue(,) As String
    Public system_scalesettingmat(,) As String
    Public nLineType As Long
    Public ncolor As Long
    Public nscalesetting As Long
    Public nscalesettingvalue As Long
    Public nscalesettintmat As Long
    Public Const Dashed_Lenght As Integer = 5

    Public Function YCM_ReadEntSetting(ByVal strDBPath As String) As Integer
        Dim clsOPe As New CDBOperate
        Dim adoRet As ADODB.Recordset
        If clsOPe.ConnectDB(strDBPath) = False Then
            YCM_ReadEntSetting = -1
        End If
        Dim strSQL As String
        strSQL = "SELECT * FROM dispsetting"
        adoRet = clsOPe.CreateRecordset(strSQL)
        If adoRet.RecordCount > 0 Then
            Do Until adoRet.EOF
                If adoRet("種別").Value = "レイ" Then
                    Call readEntSetting(adoRet, "レイ", entset_ray)
                    '★20121115計測点
                ElseIf adoRet("種別").Value = "計測点" Then
                    Call readEntSetting(adoRet, "計測点", entset_point)
                ElseIf adoRet("種別").Value = "中心円" Then
                    entset_point.color2 = YCM_GetColorInfoByCode(adoRet("色コード").Value)
                ElseIf adoRet("種別").Value = "十字線" Then
                    entset_point.color3 = YCM_GetColorInfoByCode(adoRet("色コード").Value)

                    '================================================================================================20121115
                    '    '★20121114追加(カメラを参考に作るべきか?/山田)
                    'ElseIf adoRet("種別").Value = "計測点(ターゲット面)" Then '★20121114追加
                    '    Call readEntSetting(adoRet, "計測点(ターゲット面)", entset_point1)
                    'ElseIf adoRet("種別").Value = "計測点(中心円)" Then
                    '    Call readEntSetting(adoRet, "計測点(中心円)", entset_point2)
                    'ElseIf adoRet("種別").Value = "計測点(十字線)" Then
                    '    Call readEntSetting(adoRet, "計測点(十字線)", entset_point3)
                    '================================================================================================20121115

                ElseIf adoRet("種別").Value = "追加計測点" Then
                    Call readEntSetting(adoRet, "追加計測点", entset_pointUser)
                ElseIf adoRet("種別").Value = "ラベル" Then
                    Call readEntSetting(adoRet, "ラベル", entset_label)
                    '★カメラ
                ElseIf adoRet("種別").Value = "カメラ" Then
                    Call readEntSetting(adoRet, "カメラ", entset_camera)
                ElseIf adoRet("種別").Value = "カメラレンズ" Then
                    entset_camera.color2 = YCM_GetColorInfoByCode(adoRet("色コード").Value)
                ElseIf adoRet("種別").Value = "カメラシャッター" Then
                    entset_camera.color3 = YCM_GetColorInfoByCode(adoRet("色コード").Value)

                ElseIf adoRet("種別").Value = "ライン" Then
                    Call readEntSetting(adoRet, "ライン", entset_line)
                ElseIf adoRet("種別").Value = "円" Then
                    Call readEntSetting(adoRet, "円", entset_circle)
                ElseIf adoRet("種別").Value = "CADライン" Then
                    Call readEntSetting(adoRet, "CADライン", entset_line_CAD)
                ElseIf adoRet("種別").Value = "CAD円" Then
                    Call readEntSetting(adoRet, "CAD円", entset_circle_CAD)
                End If
                adoRet.MoveNext()
            Loop
        End If
        clsOPe.DisConnectDB()
        YCM_ReadEntSetting = 0
    End Function

    Private Function readEntSetting(ByRef adoRet As ADODB.Recordset, ByVal strKind As String, ByRef entset As EntSetting) As Integer
        entset.strTypeName = strKind
        If adoRet("表示フラグ").Value = 1 Then
            entset.blnVisiable = True
        Else
            entset.blnVisiable = False
        End If
        entset.color = YCM_GetColorInfoByCode(adoRet("色コード").Value)
        entset.linetype = YCM_GetLineTypeInfoByCode(adoRet("線種コード").Value)
        entset.minscale = IIf(IsDBNull(adoRet("最小比率").Value), 1.0#, (adoRet("最小比率").Value))
        entset.maxscale = IIf(IsDBNull(adoRet("最大比率").Value), 1.0#, (adoRet("最大比率").Value))
        entset.screensize = IIf(IsDBNull(adoRet("表示サイズ").Value), 10.0#, (adoRet("表示サイズ").Value))
        'entset.screensize = IIf(IsDBNull(adoRet("表示サイズ").Value), 1.0#, (adoRet("表示サイズ").Value))元のコード

        entset.konomi = IIf(IsDBNull(adoRet("刻み").Value), 1.0#, (adoRet("刻み").Value))
        entset.strComent = IIf(IsDBNull(adoRet("備考").Value), "", adoRet("備考").Value)
        entset.layerName = IIf(IsDBNull(adoRet("レイヤ名").Value), "Default", adoRet("レイヤ名").Value)
        readEntSetting = 0
    End Function

    Public Function YCM_UpdateEntSetting(ByVal strDBPath As String) As Integer
        Dim strSQL As String
        Dim clsOPe As New CDBOperate
        Dim adoRet As ADODB.Recordset
        If clsOPe.ConnectDB(strDBPath) = False Then
            YCM_UpdateEntSetting = -1
            Exit Function
        End If

        Call updateEntSetting(clsOPe, "計測点", entset_point)
        strSQL = "SELECT * FROM dispsetting WHERE 種別='中心円'"
        adoRet = clsOPe.CreateRecordset(strSQL)
        If adoRet.RecordCount > 0 Then
            strSQL = "UPDATE dispsetting SET 表示フラグ=" + IIf(entset_point.blnVisiable, "1", "0")
            strSQL = strSQL + ",色コード=" + CStr(entset_point.color2.code) + " WHERE 種別='中心円'"
            clsOPe.ExcuteSQL(strSQL)
        Else
            strSQL = "INSERT INTO dispsetting(種別,表示フラグ,色コード,備考) VALUES('中心円',"
            strSQL = strSQL + IIf(entset_point.blnVisiable, "1", "0") + "," + CStr(entset_point.color2.code)
            strSQL = strSQL + ",'')"
            clsOPe.ExcuteSQL(strSQL)
        End If
        strSQL = "SELECT * FROM dispsetting WHERE 種別='十字線'"
        adoRet = clsOPe.CreateRecordset(strSQL)
        If adoRet.RecordCount > 0 Then
            strSQL = "UPDATE dispsetting SET 表示フラグ=" + IIf(entset_point.blnVisiable, "1", "0")
            strSQL = strSQL + ",色コード=" + CStr(entset_point.color3.code) + " WHERE 種別='十字線'"
            clsOPe.ExcuteSQL(strSQL)
        Else
            strSQL = "INSERT INTO dispsetting(種別,表示フラグ,色コード,備考) VALUES('十字線',"
            strSQL = strSQL + IIf(entset_point.blnVisiable, "1", "0") + "," + CStr(entset_point.color3.code)
            strSQL = strSQL + ",'')"
            clsOPe.ExcuteSQL(strSQL)
        End If
        '===================================================================================20121115
        ''★20121114追加(カメラを参考に作るべきか?/山田)
        'Call updateEntSetting(clsOPe, "計測点(ターゲット面)", entset_point1) '★20121114
        'Call updateEntSetting(clsOPe, "計測点(中心円)", entset_point1) '★20121114
        'Call updateEntSetting(clsOPe, "計測点(十字線)", entset_point1) '★20121114
        '===================================================================================20121115

        Call updateEntSetting(clsOPe, "追加計測点", entset_pointUser)

        Call updateEntSetting(clsOPe, "カメラ", entset_camera)
        strSQL = "SELECT * FROM dispsetting WHERE 種別='カメラレンズ'"
        adoRet = clsOPe.CreateRecordset(strSQL)
        If adoRet.RecordCount > 0 Then
            strSQL = "UPDATE dispsetting SET 表示フラグ=" + IIf(entset_camera.blnVisiable, "1", "0")
            strSQL = strSQL + ",色コード=" + CStr(entset_camera.color2.code) + " WHERE 種別='カメラレンズ'"
            clsOPe.ExcuteSQL(strSQL)
        Else
            strSQL = "INSERT INTO dispsetting(種別,表示フラグ,色コード,備考) VALUES('カメラレンズ',"
            strSQL = strSQL + IIf(entset_camera.blnVisiable, "1", "0") + "," + CStr(entset_camera.color2.code)
            strSQL = strSQL + ",'')"
            clsOPe.ExcuteSQL(strSQL)
        End If
        strSQL = "SELECT * FROM dispsetting WHERE 種別='カメラシャッター'"
        adoRet = clsOPe.CreateRecordset(strSQL)
        If adoRet.RecordCount > 0 Then
            strSQL = "UPDATE dispsetting SET 表示フラグ=" + IIf(entset_camera.blnVisiable, "1", "0")
            strSQL = strSQL + ",色コード=" + CStr(entset_camera.color3.code) + " WHERE 種別='カメラシャッター'"
            clsOPe.ExcuteSQL(strSQL)
        Else
            strSQL = "INSERT INTO dispsetting(種別,表示フラグ,色コード,備考) VALUES('カメラシャッター',"
            strSQL = strSQL + IIf(entset_camera.blnVisiable, "1", "0") + "," + CStr(entset_camera.color3.code)
            strSQL = strSQL + ",'')"
            clsOPe.ExcuteSQL(strSQL)
        End If

        Call updateEntSetting(clsOPe, "ラベル", entset_label)
        Call updateEntSetting(clsOPe, "レイ", entset_ray)

        ' 任意図形、CAD図形分

        Call updateEntSetting(clsOPe, "ライン", entset_line)
        Call updateEntSetting(clsOPe, "円", entset_circle)
        Call updateEntSetting(clsOPe, "CADライン", entset_line_CAD)
        Call updateEntSetting(clsOPe, "CAD円", entset_circle_CAD)
        clsOPe.DisConnectDB()
    End Function
    Private Function updateEntSetting(ByRef clsOPe As CDBOperate, ByVal strKind As String, ByVal entset As EntSetting) As Integer
        Dim adoRet As ADODB.Recordset
        Dim strSQL As String
        strSQL = "SELECT * FROM dispsetting WHERE 種別='" + strKind + "'"
        adoRet = clsOPe.CreateRecordset(strSQL)
        If adoRet.RecordCount > 0 Then
            strSQL = "UPDATE dispsetting SET 表示フラグ=" + IIf(entset.blnVisiable, "1", "0")
            strSQL = strSQL + ",レイヤ名='" + CStr(entset.layerName) + "'"
            strSQL = strSQL + ",色コード=" + CStr(entset.color.code) + ",線種コード='" + CStr(entset.linetype.code)
            strSQL = strSQL + "',最小比率=" + CStr(entset.minscale) + ",最大比率=" + CStr(entset.maxscale)
            strSQL = strSQL + ",表示サイズ=" + CStr(entset.screensize) + ",刻み=" + CStr(entset.konomi) + " WHERE 種別='" + strKind + "'"
            clsOPe.ExcuteSQL(strSQL)
        Else
            strSQL = "INSERT INTO dispsetting(種別,レイヤ名,表示フラグ,色コード,線種コード,最小比率,最大比率,表示サイズ,刻み,備考) VALUES('" + strKind + "',"
            strSQL = strSQL + "'" + CStr(entset.layerName) + "',"
            strSQL = strSQL + IIf(entset.blnVisiable, "1", "0") + "," + CStr(entset.color.code) + ",'" + CStr(entset.linetype.code)
            strSQL = strSQL + "'," + CStr(entset.minscale) + "," + CStr(entset.maxscale) + "," + CStr(entset.screensize)
            strSQL = strSQL + "," + CStr(entset.konomi) + ",'')"
            clsOPe.ExcuteSQL(strSQL)
        End If
        updateEntSetting = 0
    End Function

    Public Function YCM_GetColorInfoByCode(ByVal code As Integer) As ModelColor
        YCM_GetColorInfoByCode = Nothing
        If ncolor > 0 Then
            For ii As Integer = 0 To ncolor - 1
                If sys_color(ii).code = code Then
                    YCM_GetColorInfoByCode.strName = sys_color(ii).strColorName
                    YCM_GetColorInfoByCode.code = code
                    YCM_GetColorInfoByCode.dbl_red = sys_color(ii).red
                    YCM_GetColorInfoByCode.dbl_green = sys_color(ii).green
                    YCM_GetColorInfoByCode.dbl_blue = sys_color(ii).blue
                    YCM_GetColorInfoByCode.strHalconColorName = sys_color(ii).strHalconColorName '20121127 追加　ＳＵＵＲＩ
                    Exit Function
                End If
            Next
        End If
    End Function
    Public Function YCM_GetLineTypeInfoByCode(ByVal code As Integer) As LineType
        YCM_GetLineTypeInfoByCode = Nothing
        If nLineType > 0 Then
            For ii As Integer = 0 To nLineType - 1
                If sys_LineType(ii).code = code Then
                    YCM_GetLineTypeInfoByCode.strName = sys_LineType(ii).strLineTypeName
                    YCM_GetLineTypeInfoByCode.code = code
                    YCM_GetLineTypeInfoByCode.kyori = sys_LineType(ii).kyori
                    YCM_GetLineTypeInfoByCode.pattern = sys_LineType(ii).pattern
                    '20150401 Tezuka ADD
                    If Trim(sys_LineType(ii).strLineTypeName) = "DASHED" Then
                        YCM_GetLineTypeInfoByCode.strHalconLineType = Dashed_Lenght
                    Else
                        YCM_GetLineTypeInfoByCode.strHalconLineType = 0
                    End If
                    Exit Function
                End If
            Next
        End If
    End Function
    Public Function YCM_GetColorInfoByName(ByVal name As String) As ModelColor
        YCM_GetColorInfoByName = Nothing
        If ncolor > 0 Then
            For ii As Integer = 0 To ncolor - 1
                If sys_color(ii).strColorName = name Then
                    YCM_GetColorInfoByName.strName = sys_color(ii).strColorName
                    YCM_GetColorInfoByName.code = sys_color(ii).code
                    YCM_GetColorInfoByName.dbl_red = sys_color(ii).red
                    YCM_GetColorInfoByName.dbl_green = sys_color(ii).green
                    YCM_GetColorInfoByName.dbl_blue = sys_color(ii).blue
                    YCM_GetColorInfoByName.strHalconColorName = sys_color(ii).strHalconColorName '20121127 追加　ＳＵＵＲＩ
                    Exit Function
                End If
            Next
        End If
    End Function
    Public Function YCM_GetLineTypeInfoByName(ByVal name As String) As LineType
        YCM_GetLineTypeInfoByName = Nothing
        If nLineType > 0 Then
            For ii As Integer = 0 To nLineType - 1
                If sys_LineType(ii).strLineTypeName = name Then
                    YCM_GetLineTypeInfoByName.strName = sys_LineType(ii).strLineTypeName
                    YCM_GetLineTypeInfoByName.code = sys_LineType(ii).code
                    YCM_GetLineTypeInfoByName.kyori = sys_LineType(ii).kyori
                    YCM_GetLineTypeInfoByName.pattern = sys_LineType(ii).pattern
                    '20150401 Tezuka ADD
                    If Trim(sys_LineType(ii).strLineTypeName) = "DASHED" Then
                        YCM_GetLineTypeInfoByName.strHalconLineType = Dashed_Lenght
                    Else
                        YCM_GetLineTypeInfoByName.strHalconLineType = 0
                    End If
                    Exit Function
                End If
            Next
        End If
    End Function

    Public Function YCM_ReadSystemscalesettingAcs(ByVal strDBPath As String) As Integer
        Dim clsOPe As New CDBOperate
        Dim adoRet As ADODB.Recordset
        If clsOPe.ConnectDB(strDBPath) = False Then
            YCM_ReadSystemscalesettingAcs = -1
        End If
        'ReDim System_scalesetting(0, 0)
        nscalesetting = 0
        Dim strSQL As String

        strSQL = "SELECT スケール長"
        'distinct
        strSQL = strSQL + " FROM userscalesetting ORDER BY ID"
        adoRet = clsOPe.CreateRecordset(strSQL)
        If adoRet.RecordCount > 0 Then

            Do Until adoRet.EOF
                ReDim Preserve System_scalesetting(1, nscalesetting)
                System_scalesetting(1, nscalesetting) = New Double()
                System_scalesetting(0, nscalesetting) = adoRet("スケール長").Value.ToString
                nscalesetting = nscalesetting + 1
                adoRet.MoveNext()
            Loop
        End If
        clsOPe.DisConnectDB()
    End Function
    Public Function YCM_ReadSystemscalesettingAcsvalue(ByVal strDBPath As String) As Integer
        Dim clsOPe As New CDBOperate
        Dim adoRet As ADODB.Recordset
        If clsOPe.ConnectDB(strDBPath) = False Then
            YCM_ReadSystemscalesettingAcsvalue = -1
        End If
        'ReDim System_scalesetting(0, 0)
        nscalesettingvalue = 0
        Dim strSQL As String

        strSQL = "SELECT スケール値"
        'distinct
        strSQL = strSQL + " FROM userscalesetting ORDER BY ID"
        adoRet = clsOPe.CreateRecordset(strSQL)
        If adoRet.RecordCount > 0 Then

            Do Until adoRet.EOF
                ReDim Preserve System_scalesettingvalue(1, nscalesettingvalue)
                System_scalesettingvalue(1, nscalesettingvalue) = New Double()
                System_scalesettingvalue(0, nscalesettingvalue) = adoRet("スケール値").Value.ToString
                nscalesettingvalue = nscalesettingvalue + 1
                adoRet.MoveNext()
            Loop
        End If
        clsOPe.DisConnectDB()
    End Function
    '13.1.25　スケール設定を行った2点をDB（userscalesetting）から呼び出す

    Public Function YCM_ReadSystemscalesettingAcsP1_2(ByVal strDBPath As String) As Integer
        Dim clsOPe As New CDBOperate
        Dim adoRet As ADODB.Recordset
        If clsOPe.ConnectDB(strDBPath) = False Then
            YCM_ReadSystemscalesettingAcsP1_2 = -1
        End If
        'ReDim System_scalesetting(0, 0)

        Dim strSQL As String

        strSQL = "SELECT PID1,PID2"
        strSQL = strSQL + " FROM userscalesetting ORDER BY ID"
        sys_ScaleInfo.p1 = New CLookPoint
        sys_ScaleInfo.p2 = New CLookPoint

        adoRet = clsOPe.CreateRecordset(strSQL)
        If adoRet.RecordCount > 0 Then

            Do Until adoRet.EOF

                sys_ScaleInfo.p1.mid = CLng(adoRet("PID1").Value.ToString)
                sys_ScaleInfo.p2.mid = CLng(adoRet("PID2").Value.ToString)

                adoRet.MoveNext()
            Loop
        End If


        clsOPe.DisConnectDB()
    End Function

    Public Function YCM_ReadSystemscalesettingAcsmat(ByVal strDBPath As String) As Integer
        Dim clsOPe As New CDBOperate
        Dim adoRet As ADODB.Recordset
        If clsOPe.ConnectDB(strDBPath) = False Then
            YCM_ReadSystemscalesettingAcsmat = -1
        End If
        'ReDim System_scalesetting(0, 0)
        Dim nmat As Integer = 15
        'nscalesettintmat = 15
        Dim strSQL As String
        Dim intnmat As Integer
        strSQL = "SELECT distinct mat11,mat12,mat13,mat14,mat21,mat22,mat23,mat24,mat31,mat32,mat33,mat34,mat41,mat42,mat43,mat44"
        'distinct
        strSQL = strSQL + " FROM transformmatrix"
        adoRet = clsOPe.CreateRecordset(strSQL)
        If adoRet.RecordCount > 0 Then

            Do Until adoRet.EOF
                ReDim Preserve system_scalesettingmat(1, nmat)
                system_scalesettingmat(1, nmat) = New Double()
                system_scalesettingmat(0, nscalesettintmat) = adoRet("mat11").Value.ToString
                system_scalesettingmat(0, nscalesettintmat + 1) = adoRet("mat12").Value.ToString
                system_scalesettingmat(0, nscalesettintmat + 2) = adoRet("mat13").Value.ToString
                system_scalesettingmat(0, nscalesettintmat + 3) = adoRet("mat14").Value.ToString
                system_scalesettingmat(0, nscalesettintmat + 4) = adoRet("mat21").Value.ToString
                system_scalesettingmat(0, nscalesettintmat + 5) = adoRet("mat22").Value.ToString
                system_scalesettingmat(0, nscalesettintmat + 6) = adoRet("mat23").Value.ToString
                system_scalesettingmat(0, nscalesettintmat + 7) = adoRet("mat24").Value.ToString
                system_scalesettingmat(0, nscalesettintmat + 8) = adoRet("mat31").Value.ToString
                system_scalesettingmat(0, nscalesettintmat + 9) = adoRet("mat32").Value.ToString
                system_scalesettingmat(0, nscalesettintmat + 10) = adoRet("mat33").Value.ToString
                system_scalesettingmat(0, nscalesettintmat + 11) = adoRet("mat34").Value.ToString
                system_scalesettingmat(0, nscalesettintmat + 12) = adoRet("mat41").Value.ToString
                system_scalesettingmat(0, nscalesettintmat + 13) = adoRet("mat42").Value.ToString
                system_scalesettingmat(0, nscalesettintmat + 14) = adoRet("mat43").Value.ToString
                system_scalesettingmat(0, nscalesettintmat + 15) = adoRet("mat44").Value.ToString
                intnmat = intnmat + 1
                adoRet.MoveNext()
            Loop
        End If

        clsOPe.DisConnectDB()
        If intnmat = 0 Then
            YCM_ReadSystemscalesettingAcsmat = 1
        Else
            YCM_ReadSystemscalesettingAcsmat = 2
        End If
    End Function
    Public Function YCM_ReadSystemColorAcs(ByVal strDBPath As String) As Integer
        Dim clsOPe As New CDBOperate
        Dim adoRet As ADODB.Recordset
        If clsOPe.ConnectDB(strDBPath) = False Then
            YCM_ReadSystemColorAcs = -1
        End If
        Dim strSQL As String

        ncolor = 0          '20140129 Tezuka ADD

        strSQL = "SELECT 定数名,色コード,Red,Green,Blue,HalconColorName,備考"
        strSQL = strSQL + " FROM 色コードマスタ"
        adoRet = clsOPe.CreateRecordset(strSQL)
        If adoRet.RecordCount > 0 Then

            Do Until adoRet.EOF
                ReDim Preserve sys_color(ncolor)
                sys_color(ncolor).strColorName = adoRet("定数名").Value
                sys_color(ncolor).code = adoRet("色コード").Value
                sys_color(ncolor).red = adoRet("Red").Value
                sys_color(ncolor).green = adoRet("Green").Value
                sys_color(ncolor).blue = adoRet("Blue").Value
                sys_color(ncolor).strComent = IIf(IsDBNull(adoRet("備考").Value), "", adoRet("備考").Value)
                sys_color(ncolor).strHalconColorName = adoRet("HalconColorName").Value
                ncolor = ncolor + 1
                adoRet.MoveNext()
            Loop
        End If
        clsOPe.DisConnectDB()
    End Function
    Public Function YCM_ReadSystemLineTypes(ByVal strDBPath As String) As Integer
        Dim clsOPe As New CDBOperate
        Dim adoRet As ADODB.Recordset
        If clsOPe.ConnectDB(strDBPath) = False Then
            YCM_ReadSystemLineTypes = -1
        End If
        nLineType = 0
        Dim strSQL As String

        strSQL = "SELECT 定数名,線種コード,破線の間隔,破線のパターン"
        strSQL = strSQL + " FROM 線種コードマスタ"
        adoRet = clsOPe.CreateRecordset(strSQL)
        If adoRet.RecordCount > 0 Then

            Do Until adoRet.EOF
                ReDim Preserve sys_LineType(nLineType)
                sys_LineType(nLineType).strLineTypeName = adoRet("定数名").Value
                sys_LineType(nLineType).code = adoRet("線種コード").Value
                sys_LineType(nLineType).kyori = adoRet("破線の間隔").Value
                sys_LineType(nLineType).pattern = adoRet("破線のパターン").Value
                nLineType = nLineType + 1
                adoRet.MoveNext()
            Loop
        End If
        clsOPe.DisConnectDB()
        YCM_ReadSystemLineTypes = 0
    End Function

    Public Function YCM_UpdateDBbyLayer(ByVal clsOPe As CDBOperate) As Integer
        Dim adoRecSet As ADODB.Recordset
        YCM_UpdateDBbyLayer = -1
        If (clsOPe Is Nothing) Then Exit Function

        '「dispsetting」「userfigure」テーブルに[レイヤ]フィールドが無い場合の処理を追加
        Dim strSQL As String
        strSQL = "SELECT レイヤ名 FROM dispsetting"
        adoRecSet = clsOPe.CreateRecordset(strSQL)
        If adoRecSet Is Nothing Then
            strSQL = "ALTER TABLE [dispsetting] ADD レイヤ名 STRING(255);"
            Call clsOPe.ExcuteSQL(strSQL)
            strSQL = "UPDATE [dispsetting] SET レイヤ名='Default' WHERE レイヤ名 Is Null;"
            Call clsOPe.ExcuteSQL(strSQL)
        End If
        strSQL = "SELECT レイヤ名 FROM userfigure"
        adoRecSet = clsOPe.CreateRecordset(strSQL)
        If adoRecSet Is Nothing Then
            strSQL = "ALTER TABLE [userfigure] ADD レイヤ名 STRING(255);"
            Call clsOPe.ExcuteSQL(strSQL)
            strSQL = "UPDATE [userfigure] SET レイヤ名='Default' WHERE レイヤ名 Is Null;"
            Call clsOPe.ExcuteSQL(strSQL)
        End If
        YCM_UpdateDBbyLayer = 0
    End Function

End Module
