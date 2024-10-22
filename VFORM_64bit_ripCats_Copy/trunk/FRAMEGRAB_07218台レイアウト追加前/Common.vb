﻿Imports Microsoft.VisualBasic.FileIO

Module Common
    Public Op As New HALCONXLib.HOperatorSetX
    Public Tuple As New HALCONXLib.HTupleX
    Public dbClass As CDBOperateOLE
    Public SingleTargetFields As String() = {"ImageID", "P2ID", "P3ID", "flgUsed", "P2D_Row", "P2D_Col", _
                                            "RayP1_X", "RayP1_Y", "RayP1_Z", "RayP2_X", "RayP2_Y", "RayP2_Z", "Dist", "ReProjectionError", "flgType"}
    Public Const FBM_MDB As String = "dbFBM.mdb"
    'Public Const FBM_MDB_TEST As String = "dbFBM_TEST.mdb"  '20150204山田Debug用
    Public Const YCM_TEMP_MDB As String = "計測データ_temp.mdb"
    Public Const YCM_MDB As String = "計測データ.mdb"
    'ADD BY TUGI 20150706 Sta 
    Public Const YCM_SYS_MDB As String = "システム設定.mdb"
    Public Const YCM_SYS_MDB_KIJYUNTBL As String = "基準座標"
    Public Const YCM_SYS_FLDR As String = "\計測システムフォルダ\"
    'ADD BY TUGI 20150706 End
    Public Const CrossSize As Integer = 10
    Public Const CrossAngle As Integer = 0.785398

    Public Const AllIncidentCount As Integer = 480          '(20151028 ADD)

    '  Public lstResultTarget As List(Of TargetDetect)
    Public tmptarget As TargetDetect
    Public hv_ALLCTID As New Object
    Public blnRoop As Boolean = False

    Public objShootParam As Common.ShootParam
    Public objLiveParam As Common.LiveParam
    Public strKojiFolder As String
    Public CommonTypeID As Integer = 28
    Public lstShootParam As List(Of ShootParam)
    Public objBeforeTarget As TargetDetect
    Public lstCamPose As List(Of CameraPose)

    Public Ez As EzArrayIf.EzArrayIF.EzArrayIF

    Public Sub SaveTupleObj(ByVal objTuple As Object, ByVal strFullPath As String)
        Dim intLen As Integer = 0

        Try
            Op.TupleLength(objTuple, intLen)
        Catch ex As Exception
            intLen = 0
        End Try

        If intLen = 0 Then
        Else
            'Dim hv_FileHandle As Object = Nothing
            'Op.OpenFile(strFullPath, "output", hv_FileHandle)

            'Op.FwriteString(hv_FileHandle, Tuple.TupleAdd(objTuple, ","))

            'Op.CloseFile(hv_FileHandle)
            Op.WriteTuple(objTuple, strFullPath)
            'WriteTuple：objTupleの内容をstrFullPathへ書き込む
        End If
    End Sub

    Public Function ReadTupleObj(ByRef objTuple As Object, ByVal strFullPath As String) As Boolean
        Dim fileExists As Boolean
        fileExists = My.Computer.FileSystem.FileExists(strFullPath)
        If fileExists = False Then

        Else
            'Dim hv_FileHandle As Object = Nothing
            'Dim hv_OutString As Object = Nothing
            'Dim hv_IsEOF As Object = Nothing, hv_Number As Object = Nothing
            'Dim hv_Substrings As Object = Nothing
            'Try
            '    Op.OpenFile(strFullPath, "input", hv_FileHandle)
            '    Op.FreadString(hv_FileHandle, hv_OutString, hv_IsEOF)
            '    Op.TupleSplit(hv_OutString, ",", hv_Substrings)
            '    Op.TupleNumber(hv_Substrings, objTuple)
            '    Op.CloseFile(hv_FileHandle)
            'Catch ex As Exception
            '    Dim t As Integer = 1
            'End Try
            Try
#If Halcon = 11 Then

                Op.ReadTuple(strFullPath, objTuple)
#Else
                objTuple = New Object()

                Dim filename As String = strFullPath
                Dim i As Integer = 0

                Dim fields As String()
                Dim delimiter As String = " "
                Using parser As New TextFieldParser(filename)
                    parser.SetDelimiters(delimiter)
                    While Not parser.EndOfData
                        ' Read in the fields for the current line
                        fields = parser.ReadFields()
                        ' Add code here to use data in fields variable.
                        If i = 0 Then
                            ReDim objTuple(CInt(fields(0)) - 1)
                        Else
                            Select Case fields(0)
                                Case "1"
                                    objTuple(i - 1) = CInt(fields(1))
                                Case "2"
                                    objTuple(i - 1) = CDbl(fields(1))
                            End Select

                        End If
                        i += 1

                    End While
                End Using
#End If



            Catch ex As Exception

            End Try

            'ReadTuple：strFullPathの内容を読込みobjTupleへ


        End If
        Return fileExists
    End Function


    Public Sub ExtendVar(ByRef values As Object, ByVal index As Long)
        If (values Is (System.DBNull.Value)) Then
            ReDim values(index)
        Else
            If (IsArray(values) = False) Then
                Dim TmpVar As Object
                TmpVar = values
                ReDim values(index)
                values(0) = TmpVar
            Else
                Dim len As Integer
                len = values.Length
                If (index >= len) Then
                    Dim new_arr() As Object
                    ReDim new_arr(index)
                    Array.Copy(values, new_arr, len)
                    values = new_arr
                Else
                    If Not (values.GetType.FullName = "System.Object[]") Then
                        Dim new_arr() As Object
                        ReDim new_arr(len - 1)
                        Array.Copy(values, new_arr, len)
                        values = new_arr
                    End If
                End If
            End If
        End If
    End Sub
    ' Chapter: Tuple / Creation
    ' Short Description: This procedure generates a tuple with a sequence of equidistant values.
    Public Sub tuple_gen_sequence(ByVal hv_Start As Object, ByVal hv_End As Object, _
        ByVal hv_Step As Object, ByRef hv_Sequence As Object)

        'Dim COMExpWinHandleStack As New HDevWindowStackX()
        'Dim sys As New HSystemX()
        ' Initialize local and output iconic variables 

        '
        'This procedure generates a tuple with a sequence of equidistant values.
        '[Start, Start + Step, Start + 2*Step, ... End]
        '
        'Input parameters:
        'Start: Start value of the tuple
        'End:   Maximum value for the last entry.
        '       Note that the last entry of the resulting tuple may be less than End
        'Step:  Increment value
        'Assure that Step#0 and sgn(Start-End)#sgn(Step), else an error occurs
        '
        'Output parameter:
        'Sequence: The resulting tuple [Start, Start + Step, Start + 2*Step, ... End]
        '
        hv_Sequence = Tuple.TupleAdd(Tuple.TupleSub(hv_Start, hv_Step), Tuple.TupleCumul( _
            Tuple.TupleGenConst(Tuple.TupleAdd(Tuple.TupleInt(Tuple.TupleDiv(Tuple.TupleSub( _
            hv_End, hv_Start), hv_Step)), 1), hv_Step)))

        Exit Sub
    End Sub

    Public Sub GetALLShootParam()

        Dim IDR As IDataReader
        Dim strSQL As String = "SELECT * FROM ShootParam ORDER BY ID"
        If lstShootParam Is Nothing Then
            lstShootParam = New List(Of ShootParam)
        Else
            lstShootParam.Clear()
        End If
        IDR = dbClass.DoSelect(strSQL)
        If Not IDR Is Nothing Then
            Do While IDR.Read
                Dim objShootParam As New ShootParam(IDR)
                lstShootParam.Add(objShootParam)
            Loop
            IDR.Close()
        End If
    End Sub

    Public Class ShootParam
        Public ID As Integer
        Public setteiname As String
        Public settei As Boolean
        Public flgAllTarget As Boolean
        Public gain_master As Integer
        Public exposure_min As Integer
        Public exposure_max As Integer
        Public exposure_kankaku As Integer
        Public targetthreshold_min As Integer
        Public targetthreshold_max As Integer
        Public targetthreshold_kankaku As Integer
        Public Sub New()

        End Sub
        Public Sub New(ByVal settingFileName As String)
            Dim filename As String = settingFileName
            Dim fields As String()
            Dim delimiter As String = ","
            Using parser As New TextFieldParser(filename)
                parser.SetDelimiters(delimiter)
                While Not parser.EndOfData
                    ' Read in the fields for the current line
                    fields = parser.ReadFields()
                    ' Add code here to use data in fields variable.
                    If IsNumeric(fields(0)) Then
                        gain_master = fields(0)
                        exposure_min = fields(1)
                        exposure_max = fields(2)
                        exposure_kankaku = fields(3)
                        targetthreshold_min = fields(4)
                        targetthreshold_max = fields(5)
                        targetthreshold_kankaku = fields(6)
                        flgAllTarget = True
                    End If
                End While
            End Using

        End Sub

        Public Sub New(ByRef IDR As IDataReader)
            ID = CInt(IDR.GetValue(0))
            setteiname = CStr(IDR.GetValue(1))
            settei = CBool(IDR.GetValue(2))

            gain_master = CInt(IDR.GetValue(3))
            flgAllTarget = CBool(IDR.GetValue(4))
            exposure_min = CInt(IDR.GetValue(5))
            exposure_max = CInt(IDR.GetValue(6))
            exposure_kankaku = CInt(IDR.GetValue(7))
            targetthreshold_min = CInt(IDR.GetValue(8))
            targetthreshold_max = CInt(IDR.GetValue(9))
            targetthreshold_kankaku = CInt(IDR.GetValue(10))

        End Sub
        Public Function UpdateToDb() As Boolean
            UpdateToDb = True



            Dim lRet As Long = 0
            Dim strSQL As String
            strSQL = "UPDATE ShootParam SET 設定=" & IIf(settei = True, "TRUE", "FALSE") & " WHERE ID = " & ID

            lRet = dbClass.ExecuteSQL(strSQL)

            If lRet = 1 Then

            Else
                UpdateToDb = False
            End If

        End Function
    End Class

    Public Class LiveParam
        Public isAuto As Integer
        Public gain_master As Integer
        Public exposure As Integer
        Public frameRate As Double 'SUSANO ADD 20160620
        Public ImageWidth As Integer
        Public ImageHeight As Integer
        'Public targetthreshold_min As Integer
        'Public targetthreshold_max As Integer
        'Public targetthreshold_kankaku As Integer
        Public Sub New()

        End Sub
        Public Sub New(ByVal settingFileName As String)
            Dim filename As String = settingFileName
            Dim fields As String()
            Dim delimiter As String = ","
            Using parser As New TextFieldParser(filename)
                parser.SetDelimiters(delimiter)
                While Not parser.EndOfData
                    ' Read in the fields for the current line
                    fields = parser.ReadFields()
                    ' Add code here to use data in fields variable.
                    If IsNumeric(fields(0)) Then
                        Try
                            isAuto = fields(0)
                            gain_master = fields(1)
                            exposure = fields(2)
                            frameRate = CDbl(fields(3))
                            ImageWidth = fields(4)
                            ImageHeight = fields(5)
                        Catch ex As Exception
                            MsgBox("LiveParam.csvファイルの読み込みに失敗しました。ファイル仕様を確認してください。")
                            Exit Sub
                        End Try
                      
                        'targetthreshold_min = fields(4)
                        'targetthreshold_max = fields(5)
                        'targetthreshold_kankaku = fields(6)
                    End If
                End While
            End Using

        End Sub

    End Class
    Public Function ReadFourTenTarget(ByVal rownum As Integer) As String
        Dim filename As String = My.Application.Info.DirectoryPath & "\Setting\FourTenTarget.txt"
        Dim fields As String()
        Dim delimiter As String = ","
        Dim icnt As Integer = 1
        Using parser As New TextFieldParser(filename)
            parser.SetDelimiters(delimiter)
            While Not parser.EndOfData
                ' Read in the fields for the current line
                fields = parser.ReadFields()
                ' Add code here to use data in fields variable.
                If icnt = rownum Then
                    ReadFourTenTarget = fields(0)
                    Exit Function
                End If
                icnt = icnt + 1
            End While
        End Using
    End Function
    Public Function ReadFrameGrabSetting(ByVal rownum As Integer) As String
        Dim filename As String = My.Application.Info.DirectoryPath & "\Setting\FrameGrabSetting.txt"
        Dim fields As String()
        Dim delimiter As String = ","
        Dim icnt As Integer = 1
        Using parser As New TextFieldParser(filename)
            parser.SetDelimiters(delimiter)
            While Not parser.EndOfData
                ' Read in the fields for the current line
                fields = parser.ReadFields()
                ' Add code here to use data in fields variable.
                If icnt = rownum Then
                    ReadFrameGrabSetting = fields(0)
                    Exit Function
                End If
                icnt = icnt + 1
            End While
        End Using

    End Function

#Region "DB接続関数"

    Public Function AccessConnect(ByRef dbClass As CDBOperateOLE, _
                                  ByVal strFolderName As String, _
                                  ByVal strDBName As String) As Integer
        Dim strSb As New System.Text.StringBuilder

        Dim Flg As Boolean = False

        AccessConnect = -1

        dbClass = New CDBOperateOLE()

        If Not dbClass Is Nothing Then
            Flg = dbClass.Connect(strFolderName & strDBName)
        End If

        If Flg = True Then
            AccessConnect = 1
        End If

    End Function

    Public Function AccessDisConnect() As Boolean

        dbClass.DisConnect()

    End Function

    Public Function ConnectDbFBM(ByVal strDBPath As String) As Boolean
        Dim flgConnected As Integer

        'common_db.mdbに接続
        flgConnected = AccessConnect(dbClass, strDBPath, FBM_MDB)

        If flgConnected = -1 Then
            MsgBox("Access(" & FBM_MDB & ")に接続できませんでした。", MsgBoxStyle.OkOnly, "確認")
            ConnectDbFBM = False
            dbClass = Nothing
            Exit Function
        End If
        ConnectDbFBM = True
    End Function

    Public Function ConnectDbYCM(ByVal strDBPath As String) As Boolean
        Dim flgConnected As Integer

        'common_db.mdbに接続
        flgConnected = AccessConnect(dbClass, strDBPath, YCM_TEMP_MDB)
        If flgConnected = -1 Then
            MsgBox("Access(" & YCM_TEMP_MDB & ")に接続できませんでした。", MsgBoxStyle.OkOnly, "確認")
            ConnectDbYCM = False
            Exit Function
        End If
        ConnectDbYCM = True
    End Function

    Public Function ConnectDbYCMNoTemp(ByVal strDBPath As String) As Boolean
        Dim flgConnected As Integer

        'common_db.mdbに接続
         flgConnected = AccessConnect(dbClass, strDBPath, YCM_MDB)
        If flgConnected = -1 Then
            MsgBox("Access(" & YCM_MDB & ")に接続できませんでした。", MsgBoxStyle.OkOnly, "確認")
            ConnectDbYCMNoTemp = False
            Exit Function
        End If
        ConnectDbYCMNoTemp = True
    End Function
    'ADD BY TUGI 20150706 Sta 

    Public Function ConnectDbSystemSetting(ByVal strDBPath As String) As Boolean
        Dim flgConnected As Integer

        'common_db.mdbに接続
        flgConnected = AccessConnect(dbClass, strDBPath, YCM_SYS_MDB)
        If flgConnected = -1 Then
            MsgBox("Access(" & YCM_SYS_MDB & ")に接続できませんでした。", MsgBoxStyle.OkOnly, "確認")
            ConnectDbSystemSetting = False
            Exit Function
        End If
        ConnectDbSystemSetting = True
    End Function
    Public Function ConnectDbForALLMDB(ByVal strDBPath As String, ByVal dbname As String) As Boolean
        Dim flgConnected As Integer

        'common_db.mdbに接続
        flgConnected = AccessConnect(dbClass, strDBPath, dbname)
        If flgConnected = -1 Then
            MsgBox("Access(" & YCM_SYS_MDB & ")に接続できませんでした。", MsgBoxStyle.OkOnly, "確認")
            ConnectDbForALLMDB = False
            Exit Function
        End If
        ConnectDbForALLMDB = True
    End Function
    Public Function insertIntoTblKijyunPointFromCSV(ByVal csvPath As String) As Boolean
        insertIntoTblKijyunPointFromCSV = False
        Dim sysPath As String = My.Settings.YCMFolder & "\" & YCM_SYS_FLDR
        Dim fieldNames(5) As String
        fieldNames(0) = "CTID"
        fieldNames(1) = "ラベル"
        fieldNames(2) = "X座標"
        fieldNames(3) = "Y座標"
        fieldNames(4) = "Z座標"
        fieldNames(5) = "フラグ"
        Try
            If ConnectDbSystemSetting(sysPath) Then
                dbClass.DoDelete(YCM_SYS_MDB_KIJYUNTBL)
                dbClass.BeginTrans()
                Dim fields As String()
                Dim delimiter As String = ","
                Using parser As New TextFieldParser(csvPath)
                    parser.SetDelimiters(delimiter)
                    While Not parser.EndOfData
                        fields = parser.ReadFields()
                        fields(1) = "'" & fields(1) & "'"
                        If IsNumeric(fields(3)) Then
                            dbClass.DoInsert(fieldNames, YCM_SYS_MDB_KIJYUNTBL, fields)
                        End If
                    End While
                End Using
                dbClass.CommitTrans()
                dbClass.DisConnect()
                insertIntoTblKijyunPointFromCSV = True
            End If
        Catch ex As Exception
            insertIntoTblKijyunPointFromCSV = False
        Finally
        End Try

    End Function

    Public ReadOnly Property dblField(ByVal objField As Object) As Double
        Get
            Try
                If IsNumeric(objField) = False Then
                    Return CDbl(0)
                Else
                    Return CDbl(objField)
                End If
            Catch ex As Exception
                Return CDbl(0)
            End Try
        End Get
    End Property
    Public Function GetIndexOfDataReader(ByRef strItemNames() As String, ByRef iDataReader As IDataReader) As Integer
        For Each strItem As String In strItemNames
            Try
                GetIndexOfDataReader = iDataReader.GetOrdinal(strItem)
                Exit Function
            Catch ex As Exception

            End Try
        Next
        GetIndexOfDataReader = -1
    End Function

    Public Function GetDataByIndexOfDataReader(ByRef iIndex As Integer, ByRef iDataReader As IDataReader) As String
        Try
            GetDataByIndexOfDataReader = iDataReader.GetValue(iIndex).ToString.Trim
            Exit Function
        Catch ex As Exception

        End Try
        GetDataByIndexOfDataReader = ""
    End Function
    'ADD BY TUGI 20150706 End 
#End Region

#Region "バナーセンサー関連関数"  '(20150721 Tezuka ADD) 

    'シリアルポートオープン
    Public Function Ez_PortOpen(ByVal Portname As String, ByVal BaudRate As Integer, ByVal DataBits As Integer, ByRef StopBits As Integer, ByVal Parity As Integer) As Integer

        Ez_PortOpen = 0

        If Ez Is Nothing Then
            Ez = New EzArrayIf.EzArrayIF.EzArrayIF
        End If

        Dim SerialParam As New EzArrayIf.EzArrayIF.SerialParameters
        If Portname.Length <> 0 Then
            SerialParam.PortName = Portname
        Else
            SerialParam.PortName = "COM4"
            Portname = "COM4"
        End If
        SerialParam.BaudRate = BaudRate
        SerialParam.DataBits = DataBits
        SerialParam.StopBits = StopBits
        SerialParam.Parity = Parity

        Ez.SerialParameters = SerialParam

        Dim ErrCode As Integer, ErrMsg As String = ""
        If Ez.OpenSerialPort() < 0 Then
            'TextBox2.ForeColor = Color.Red
            Ez.ReadModbusError(ErrCode, ErrMsg)
            'TextBox2.Text = "[" & CStr(ErrCode) & "]:" & ErrMsg
            ErrMsg = ErrMsg & "Port名＝" & Portname
            'MsgBox(ErrMsg, MsgBoxStyle.Exclamation)
            FrmMain.OutMessage(ErrMsg)
        Else
            'TextBox2.ForeColor = Color.Blue
            'TextBox2.Text = "シリアルポートを開きました。"
            ErrMsg = "シリアルポートを開きました。Port名＝" & Portname
            'MsgBox(ErrMsg, MsgBoxStyle.Information)
            FrmMain.OutMessage(ErrMsg)
        End If

    End Function

    'ピーク値取得
    Public Function Ez_PeakGet(ByVal SlaveAddress As Integer, ByVal Height As Integer, ByVal Pitch As Double, ByVal Regnum As Integer, ByVal Incident As Integer, ByRef Peak As Double) As Integer

        Ez_PeakGet = 0
        Peak = 0.0

        If Ez Is Nothing Then
            'MsgBox("シリアルポートがオープンされていません。", MsgBoxStyle.Exclamation)
            FrmMain.OutMessage("シリアルポートがオープンされていません。")
            Ez_PeakGet = -1
            Exit Function
        End If

        Ez.SlaveAddress = SlaveAddress
        Ez.Pitch = Pitch
        Ez.BaseHeight = Height
        Ez.HideIncidentCount = AllIncidentCount - Incident

        Dim RegParam As New EzArrayIf.EzArrayIF.RegisterParameters
        RegParam.PeakHeightAddress = Regnum
        Ez.RegisterParameters = RegParam

        Dim PeakHeight As Double
        Dim ErrCode As Integer, ErrMsg As String = ""
        If Ez.ReadPeakHeight(PeakHeight) < 0 Then
            Ez.ReadModbusError(ErrCode, ErrMsg)
            'MsgBox(ErrMsg, MsgBoxStyle.Exclamation)
            FrmMain.OutMessage(ErrMsg)
            Ez_PeakGet = -1
        Else
            Peak = PeakHeight
            FrmMain.OutMessage("ピーク値の取得に成功しました。")
        End If

    End Function

    '監視用測定値取得
    Public Function Ez_MonitorValueGet(ByVal SlaveAddress As Integer, ByVal Height As Integer, ByVal Pitch As Double, ByVal Regnum As Integer, ByVal Incident As Integer, ByRef MValue As Double) As Integer

        Ez_MonitorValueGet = 0
        MValue = 0.0

        If Ez Is Nothing Then
            'MsgBox("シリアルポートがオープンされていません。", MsgBoxStyle.Exclamation)
            FrmMain.OutMessage("シリアルポートがオープンされていません。")
            Ez_MonitorValueGet = -1
            Exit Function
        End If

        Ez.SlaveAddress = SlaveAddress
        Ez.Pitch = Pitch
        Ez.BaseHeight = Height
        Ez.HideIncidentCount = AllIncidentCount - Incident

        Dim RegParam As New EzArrayIf.EzArrayIF.RegisterParameters
        RegParam.MonitorAddress = Regnum
        Ez.RegisterParameters = RegParam

        Dim PeakHeight As Double
        Dim ErrCode As Integer, ErrMsg As String = ""
        If Ez.ReadMonitorValue(PeakHeight) < 0 Then
            Ez.ReadModbusError(ErrCode, ErrMsg)
            'MsgBox(ErrMsg, MsgBoxStyle.Exclamation)
            FrmMain.OutMessage(ErrMsg)
            Ez_MonitorValueGet = -1
        Else
            MValue = PeakHeight
            ' FrmMain.OutMessage("監視用測定値の取得に成功しました。")
        End If

    End Function

    'アクティブ計測値取得(20150915 Tezuka ADD)
    Public Function Ez_ActiveDataGet(ByVal SlaveAddress As Integer, ByVal Height As Integer, ByVal Pitch As Double, ByVal Regnum As Integer, ByRef Data As Double) As Integer

        Ez_ActiveDataGet = 0
        Data = 0.0

        If Ez Is Nothing Then
            'MsgBox("シリアルポートがオープンされていません。", MsgBoxStyle.Exclamation)
            FrmMain.OutMessage("シリアルポートがオープンされていません。")
            Ez_ActiveDataGet = -1
            Exit Function
        End If

        Ez.SlaveAddress = SlaveAddress
        Ez.Pitch = Pitch
        Ez.BaseHeight = Height

        Dim RegParam As New EzArrayIf.EzArrayIF.RegisterParameters
        RegParam.ActiveDataAddress = Regnum
        Ez.RegisterParameters = RegParam

        Dim ActiveHeight As Double
        Dim ErrCode As Integer, ErrMsg As String = ""
        If Ez.ReadActiveHeight(ActiveHeight) < 0 Then
            Ez.ReadModbusError(ErrCode, ErrMsg)
            'MsgBox(ErrMsg, MsgBoxStyle.Exclamation)
            FrmMain.OutMessage(ErrMsg)
            Ez_ActiveDataGet = -1
        Else
            Data = ActiveHeight
            FrmMain.OutMessage("アクティブ計測値の取得に成功しました。:" & Data.ToString)
        End If

    End Function

    '入光状態光軸総数取得(20150915 Tezuka ADD)
    Public Function Ez_IncidentGet(ByVal SlaveAddress As Integer, ByVal Regnum As Integer, ByRef Data As Integer) As Integer

        Ez_IncidentGet = 0
        Data = 0.0

        If Ez Is Nothing Then
            'MsgBox("シリアルポートがオープンされていません。", MsgBoxStyle.Exclamation)
            FrmMain.OutMessage("シリアルポートがオープンされていません。")
            Ez_IncidentGet = -1
            Exit Function
        End If

        Ez.SlaveAddress = SlaveAddress

        Dim RegParam As New EzArrayIf.EzArrayIF.RegisterParameters
        RegParam.IncidentAddress = Regnum
        Ez.RegisterParameters = RegParam

        Dim Incident As Integer
        Dim ErrCode As Integer, ErrMsg As String = ""
        If Ez.ReadIncident(Incident) < 0 Then
            Ez.ReadModbusError(ErrCode, ErrMsg)
            'MsgBox(ErrMsg, MsgBoxStyle.Exclamation)
            FrmMain.OutMessage(ErrMsg)
            Ez_IncidentGet = -1
        Else
            Data = Incident
            FrmMain.OutMessage("入光状態光軸総数の取得に成功しました。:" & Data.ToString)
        End If

    End Function

    'スキャンタイプ変更
    Public Function Ez_ScanTypeChange(ByVal SlaveAddress As Integer, ByVal ScanType As Integer) As Integer
        Ez_ScanTypeChange = 0

        If Ez Is Nothing Then
            'MsgBox("シリアルポートがオープンされていません。", MsgBoxStyle.Exclamation)
            FrmMain.OutMessage("シリアルポートがオープンされていません。")
            Ez_ScanTypeChange = -1
            Exit Function
        End If

        Ez.SlaveAddress = SlaveAddress

        Dim RegParam As New EzArrayIf.EzArrayIF.RegisterParameters
        RegParam.ScanTypeAddress = 1
        Ez.RegisterParameters = RegParam

        Dim ErrCode As Integer, ErrMsg As String = ""
        If ScanType = 1 Then
            If Ez.ChangeStraightType() < 0 Then
                Ez.ReadModbusError(ErrCode, ErrMsg)
                'MsgBox(ErrMsg, MsgBoxStyle.Exclamation)
                FrmMain.OutMessage(ErrMsg)
                Ez_ScanTypeChange = -1
            Else
                'MsgBox("スキャンタイプをストレートに設定しました。", MsgBoxStyle.Information)
                FrmMain.OutMessage("スキャンタイプをストレートに設定しました。")
            End If
        Else
            If Ez.ChangeSingleEdgeType() < 0 Then
                Ez.ReadModbusError(ErrCode, ErrMsg)
                'MsgBox(ErrMsg, MsgBoxStyle.Exclamation)
                FrmMain.OutMessage(ErrMsg)
                Ez_ScanTypeChange = -1
            Else
                'MsgBox("スキャンタイプをシングルエッジに設定しました。", MsgBoxStyle.Information)
                FrmMain.OutMessage("スキャンタイプをシングルエッジに設定しました。")
            End If
        End If
    End Function

    'スキャンタイプ取得
    Public Function Ez_ScanTypeGet(ByVal SlaveAddress As Integer, ByRef ScanType As Integer) As Integer
        Ez_ScanTypeGet = 0

        If Ez Is Nothing Then
            'MsgBox("シリアルポートがオープンされていません。", MsgBoxStyle.Exclamation)
            FrmMain.OutMessage("シリアルポートがオープンされていません。")
            Ez_ScanTypeGet = -1
            Exit Function
        End If

        Ez.SlaveAddress = SlaveAddress

        Dim RegParam As New EzArrayIf.EzArrayIF.RegisterParameters
        RegParam.ScanTypeAddress = 1
        Ez.RegisterParameters = RegParam

        Dim ErrCode As Integer, ErrMsg As String = ""
        If Ez.ReadScanType(ScanType) < 0 Then
            Ez.ReadModbusError(ErrCode, ErrMsg)
            'MsgBox(ErrMsg, MsgBoxStyle.Exclamation)
            FrmMain.OutMessage(ErrMsg)
            Ez_ScanTypeGet = -1
        End If

    End Function

    '稼働時間取得
    Public Function Ez_ServiceTimeGet(ByVal SlaveAddress As Integer, ByVal regnum As Integer, ByRef ServiceTime As String) As Integer

        Ez_ServiceTimeGet = 0

        If Ez Is Nothing Then
            ' MsgBox("シリアルポートがオープンされていません。", MsgBoxStyle.Exclamation)
            FrmMain.OutMessage("シリアルポートがオープンされていません。")
            Ez_ServiceTimeGet = -1
            Exit Function
        End If

        Ez.SlaveAddress = SlaveAddress

        Dim RegParam As New EzArrayIf.EzArrayIF.RegisterParameters
        RegParam.ServiceTimeAddress = regnum
        Ez.RegisterParameters = RegParam

        Dim Serv As Integer
        Dim ErrCode As Integer, ErrMsg As String = ""
        If Ez.ReadServiceTime(Serv) < 0 Then
            Ez.ReadModbusError(ErrCode, ErrMsg)
            'MsgBox(ErrMsg, MsgBoxStyle.Exclamation)
            FrmMain.OutMessage(ErrMsg)
            Ez_ServiceTimeGet = -1
        Else
            ServiceTime = CStr(Serv)
            FrmMain.OutMessage("稼動時間の取得に成功しました。")
        End If

    End Function

    'EzArrayステータス取得
    Public Function Ez_StatusGet(ByVal SlaveAddress As Integer, ByVal regnum As Integer, ByRef Message As String) As Integer

        Ez_StatusGet = 0

        If Ez Is Nothing Then
            'MsgBox("シリアルポートがオープンされていません。", MsgBoxStyle.Exclamation)
            FrmMain.OutMessage("シリアルポートがオープンされていません。")
            Ez_StatusGet = -1
            Exit Function
        End If

        Ez.SlaveAddress = SlaveAddress

        Dim RegParam As New EzArrayIf.EzArrayIF.RegisterParameters
        RegParam.ErrorCodeAddress = regnum
        Ez.RegisterParameters = RegParam

        Dim SErrCode As Integer, SErrMsg As String = ""
        Dim ErrCode As Integer, ErrMsg As String = ""
        If Ez.ReadSensorError(SErrCode, SErrMsg) < 0 Then
            Ez.ReadModbusError(ErrCode, ErrMsg)
            Message = "ステータス取得エラー：" & ErrMsg & "(" & ErrCode.ToString & ")"
            FrmMain.OutMessage("EzArrayのステータスの取得に失敗しました。")
        Else
            Message = "ステータス取得正常：" & SErrMsg & "(" & SErrCode.ToString & ")"
            FrmMain.OutMessage("EzArrayのステータスの取得に成功しました。")
        End If

    End Function

    'シリアルポートクローズ
    Public Function Ez_PortClose() As Integer
        Ez_PortClose = 0
        Ez.ClosePort()
    End Function

#End Region

    Public CameraPoseFields As String() = {"ID", "imagefilename", "CX", "CY", "CZ", "CA", "CB", "CG", "flgNotUse", "flgSystemNotUse", "flgFirst", "flgSecond", "CamParamID"}


    Public Sub ReadCameraPose()
        Dim IDR As IDataReader
        Dim strSqlText As String = ""
        'strSqlText = "SELECT ID,imagefilename,CX,CY,CZ,CA,CB,CG,flgNotUse,flgSystemNotUse,flgFirst,flgSecond,CamParamID FROM CameraPose"
        strSqlText = "SELECT ID,imagefilename,CX,CY,CZ,CA,CB,CG,flgNotUse,flgSystemNotUse FROM CameraPose"

        If lstCamPose Is Nothing Then
            lstCamPose = New List(Of CameraPose)
        Else
            lstCamPose.Clear()
        End If

        IDR = dbClass.DoSelect(strSqlText)
        If Not IDR Is Nothing Then
            Do While IDR.Read
                Dim objCamPose As New CameraPose
                objCamPose.Cid = CInt(IDR.GetValue(0))
                objCamPose.CFileName = IDR.GetValue(1)
                objCamPose.CX = CDbl(IDR.GetValue(2))
                objCamPose.CY = CDbl(IDR.GetValue(3))
                objCamPose.CZ = CDbl(IDR.GetValue(4))
                objCamPose.CA = CDbl(IDR.GetValue(5))
                objCamPose.CB = CDbl(IDR.GetValue(6))
                objCamPose.CG = CDbl(IDR.GetValue(7))
                objCamPose.CflgNotUse = CInt(IDR.GetValue(8))
                objCamPose.CflgSystemNotUse = CInt(IDR.GetValue(9))
                'objCamPose.CflgFirst = CInt(IDR.GetValue(10))
                'objCamPose.CflgSecond = CInt(IDR.GetValue(11))
                'objCamPose.CCamParamID = CInt(IDR.GetValue(12))
                lstCamPose.Add(objCamPose)
            Loop
            IDR.Close()
        End If
        AccessDisConnect()
    End Sub

    Public Sub ReadCameraPoseByKeisokuDataMDB()
        Dim IDR As IDataReader
        Dim strSqlText As String = ""
        'strSqlText = "SELECT ID,imagefilename,CX,CY,CZ,CA,CB,CG,flgNotUse,flgSystemNotUse,flgFirst,flgSecond,CamParamID FROM CameraPose"
        strSqlText = "SELECT ID,imagefilename,CX,CY,CZ,CA,CB,CG,flgNotUse,flgSystemNotUse FROM camerapos"

        If lstCamPose Is Nothing Then
            lstCamPose = New List(Of CameraPose)
        Else
            lstCamPose.Clear()
        End If

        IDR = dbClass.DoSelect(strSqlText)
        If Not IDR Is Nothing Then
            Do While IDR.Read
                Dim objCamPose As New CameraPose
                objCamPose.Cid = CInt(IDR.GetValue(0))
                objCamPose.CFileName = IDR.GetValue(1)
                objCamPose.CX = CDbl(IDR.GetValue(2))
                objCamPose.CY = CDbl(IDR.GetValue(3))
                objCamPose.CZ = CDbl(IDR.GetValue(4))
                objCamPose.CA = CDbl(IDR.GetValue(5))
                objCamPose.CB = CDbl(IDR.GetValue(6))
                objCamPose.CG = CDbl(IDR.GetValue(7))
                objCamPose.CflgNotUse = CInt(IDR.GetValue(8))
                objCamPose.CflgSystemNotUse = CInt(IDR.GetValue(9))
                'objCamPose.CflgFirst = CInt(IDR.GetValue(10))
                'objCamPose.CflgSecond = CInt(IDR.GetValue(11))
                'objCamPose.CCamParamID = CInt(IDR.GetValue(12))
                lstCamPose.Add(objCamPose)
            Loop
            IDR.Close()
        End If
        AccessDisConnect()
    End Sub


    Public Sub SaveCameraPose()
      
        If lstCamPose Is Nothing Then
            Exit Sub
        End If
        If lstCamPose.Count > 0 Then
            dbClass.DoDelete("CameraPose")
            For Each objCamPose As CameraPose In lstCamPose
                Dim strFieldData(12) As String
                strFieldData(0) = objCamPose.Cid
                strFieldData(1) = "'" & objCamPose.CFileName & "'"
                strFieldData(2) = objCamPose.CX
                strFieldData(3) = objCamPose.CY
                strFieldData(4) = objCamPose.CZ
                strFieldData(5) = objCamPose.CA
                strFieldData(6) = objCamPose.CB
                strFieldData(7) = objCamPose.CG
                strFieldData(8) = "0"
                strFieldData(9) = "0"
                strFieldData(10) = "0"
                strFieldData(11) = "0"
                strFieldData(12) = objCamPose.Cid
                dbClass.DoInsert(CameraPoseFields, "CameraPose", strFieldData)
            Next
        End If
        
        AccessDisConnect()
    End Sub

    Public Sub MonSavePose(ByVal strFname As String)
        If lstCamPose Is Nothing Then
            Exit Sub
        End If
        Dim strF As String = ""
        For Each objCamPose As CameraPose In lstCamPose
            strF = strF & objCamPose.Cid & ","
            strF = strF & objCamPose.CFileName & ","
            strF = strF & objCamPose.CX & ","
            strF = strF & objCamPose.CY & ","
            strF = strF & objCamPose.CZ & ","
            strF = strF & objCamPose.CA & ","
            strF = strF & objCamPose.CB & ","
            strF = strF & objCamPose.CG & ","
            strF = strF & objCamPose.Cid & vbNewLine
        Next
        My.Computer.FileSystem.WriteAllText(strFname, strF, False)
    End Sub

End Module
