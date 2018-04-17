Imports Microsoft.VisualBasic.FileIO
Imports FBMlib
Imports HalconDotNet

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
            FrmMainHiroshima.OutMessage(ErrMsg)
        Else
            'TextBox2.ForeColor = Color.Blue
            'TextBox2.Text = "シリアルポートを開きました。"
            ErrMsg = "シリアルポートを開きました。Port名＝" & Portname
            'MsgBox(ErrMsg, MsgBoxStyle.Information)
            FrmMainHiroshima.OutMessage(ErrMsg)
        End If

    End Function

    'ピーク値取得
    Public Function Ez_PeakGet(ByVal SlaveAddress As Integer, ByVal Height As Integer, ByVal Pitch As Double, ByVal Regnum As Integer, ByVal Incident As Integer, ByRef Peak As Double) As Integer

        Ez_PeakGet = 0
        Peak = 0.0

        If Ez Is Nothing Then
            'MsgBox("シリアルポートがオープンされていません。", MsgBoxStyle.Exclamation)
            FrmMainHiroshima.OutMessage("シリアルポートがオープンされていません。")
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
            FrmMainHiroshima.OutMessage(ErrMsg)
            Ez_PeakGet = -1
        Else
            Peak = PeakHeight
            FrmMainHiroshima.OutMessage("ピーク値の取得に成功しました。")
        End If

    End Function

    '監視用測定値取得
    Public Function Ez_MonitorValueGet(ByVal SlaveAddress As Integer, ByVal Height As Integer, ByVal Pitch As Double, ByVal Regnum As Integer, ByVal Incident As Integer, ByRef MValue As Double) As Integer

        Ez_MonitorValueGet = 0
        MValue = 0.0

        If Ez Is Nothing Then
            'MsgBox("シリアルポートがオープンされていません。", MsgBoxStyle.Exclamation)
            FrmMainHiroshima.OutMessage("シリアルポートがオープンされていません。")
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
            FrmMainHiroshima.OutMessage(ErrMsg)
            Ez_MonitorValueGet = -1
        Else
            MValue = PeakHeight
            ' FrmMainHiroshima.OutMessage("監視用測定値の取得に成功しました。")
        End If

    End Function

    'アクティブ計測値取得(20150915 Tezuka ADD)
    Public Function Ez_ActiveDataGet(ByVal SlaveAddress As Integer, ByVal Height As Integer, ByVal Pitch As Double, ByVal Regnum As Integer, ByRef Data As Double) As Integer

        Ez_ActiveDataGet = 0
        Data = 0.0

        If Ez Is Nothing Then
            'MsgBox("シリアルポートがオープンされていません。", MsgBoxStyle.Exclamation)
            FrmMainHiroshima.OutMessage("シリアルポートがオープンされていません。")
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
            FrmMainHiroshima.OutMessage(ErrMsg)
            Ez_ActiveDataGet = -1
        Else
            Data = ActiveHeight
            FrmMainHiroshima.OutMessage("アクティブ計測値の取得に成功しました。:" & Data.ToString)
        End If

    End Function

    '入光状態光軸総数取得(20150915 Tezuka ADD)
    Public Function Ez_IncidentGet(ByVal SlaveAddress As Integer, ByVal Regnum As Integer, ByRef Data As Integer) As Integer

        Ez_IncidentGet = 0
        Data = 0.0

        If Ez Is Nothing Then
            'MsgBox("シリアルポートがオープンされていません。", MsgBoxStyle.Exclamation)
            FrmMainHiroshima.OutMessage("シリアルポートがオープンされていません。")
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
            FrmMainHiroshima.OutMessage(ErrMsg)
            Ez_IncidentGet = -1
        Else
            Data = Incident
            FrmMainHiroshima.OutMessage("入光状態光軸総数の取得に成功しました。:" & Data.ToString)
        End If

    End Function

    'スキャンタイプ変更
    Public Function Ez_ScanTypeChange(ByVal SlaveAddress As Integer, ByVal ScanType As Integer) As Integer
        Ez_ScanTypeChange = 0

        If Ez Is Nothing Then
            'MsgBox("シリアルポートがオープンされていません。", MsgBoxStyle.Exclamation)
            FrmMainHiroshima.OutMessage("シリアルポートがオープンされていません。")
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
                FrmMainHiroshima.OutMessage(ErrMsg)
                Ez_ScanTypeChange = -1
            Else
                'MsgBox("スキャンタイプをストレートに設定しました。", MsgBoxStyle.Information)
                FrmMainHiroshima.OutMessage("スキャンタイプをストレートに設定しました。")
            End If
        Else
            If Ez.ChangeSingleEdgeType() < 0 Then
                Ez.ReadModbusError(ErrCode, ErrMsg)
                'MsgBox(ErrMsg, MsgBoxStyle.Exclamation)
                FrmMainHiroshima.OutMessage(ErrMsg)
                Ez_ScanTypeChange = -1
            Else
                'MsgBox("スキャンタイプをシングルエッジに設定しました。", MsgBoxStyle.Information)
                FrmMainHiroshima.OutMessage("スキャンタイプをシングルエッジに設定しました。")
            End If
        End If
    End Function

    'スキャンタイプ取得
    Public Function Ez_ScanTypeGet(ByVal SlaveAddress As Integer, ByRef ScanType As Integer) As Integer
        Ez_ScanTypeGet = 0

        If Ez Is Nothing Then
            'MsgBox("シリアルポートがオープンされていません。", MsgBoxStyle.Exclamation)
            FrmMainHiroshima.OutMessage("シリアルポートがオープンされていません。")
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
            FrmMainHiroshima.OutMessage(ErrMsg)
            Ez_ScanTypeGet = -1
        End If

    End Function

    '稼働時間取得
    Public Function Ez_ServiceTimeGet(ByVal SlaveAddress As Integer, ByVal regnum As Integer, ByRef ServiceTime As String) As Integer

        Ez_ServiceTimeGet = 0

        If Ez Is Nothing Then
            ' MsgBox("シリアルポートがオープンされていません。", MsgBoxStyle.Exclamation)
            FrmMainHiroshima.OutMessage("シリアルポートがオープンされていません。")
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
            FrmMainHiroshima.OutMessage(ErrMsg)
            Ez_ServiceTimeGet = -1
        Else
            ServiceTime = CStr(Serv - 983040) 'Edit Kiryu 新明和広島で上位レジスタの異常値をクリアできないため、下位レジスタのみ読み取り
            FrmMainHiroshima.OutMessage("稼動時間の取得に成功しました。")
        End If

    End Function

    'EzArrayステータス取得
    Public Function Ez_StatusGet(ByVal SlaveAddress As Integer, ByVal regnum As Integer, ByRef Message As String) As Integer

        Ez_StatusGet = 0

        If Ez Is Nothing Then
            'MsgBox("シリアルポートがオープンされていません。", MsgBoxStyle.Exclamation)
            FrmMainHiroshima.OutMessage("シリアルポートがオープンされていません。")
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
            FrmMainHiroshima.OutMessage("EzArrayのステータスの取得に失敗しました。")
        Else
            Message = "ステータス取得正常：" & SErrMsg & "(" & SErrCode.ToString & ")"
            FrmMainHiroshima.OutMessage("EzArrayのステータスの取得に成功しました。")
        End If

    End Function

    'シリアルポートクローズ
    Public Function Ez_PortClose() As Integer
        Ez_PortClose = 0
        Ez.ClosePort()
    End Function

#End Region

    Public CameraPoseFields As String() = {"ID", "imagefilename", "CX", "CY", "CZ", "CA", "CB", "CG", "flgNotUse", "flgSystemNotUse", "flgFirst", "flgSecond", "CamParamID", "UpdateDate"}


    Public Sub ReadCameraPose()
        Dim IDR As IDataReader
        Dim strSqlText As String = ""
        'strSqlText = "SELECT ID,imagefilename,CX,CY,CZ,CA,CB,CG,flgNotUse,flgSystemNotUse,flgFirst,flgSecond,CamParamID FROM CameraPose"
        strSqlText = "SELECT ID,imagefilename,CX,CY,CZ,CA,CB,CG,flgNotUse,flgSystemNotUse,UpdateDate FROM CameraPose"

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
                objCamPose.CUpdateDate = CStr(IDR.GetValue(10))
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


    Public Sub SaveCameraPose(ByVal SavedDate As String)

        If lstCamPose Is Nothing Then
            Exit Sub
        End If
        If lstCamPose.Count > 0 Then
            dbClass.DoDelete("CameraPose")
            For Each objCamPose As CameraPose In lstCamPose
                Dim strFieldData(13) As String
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
                strFieldData(13) = "'" & SavedDate & "'"
                'strFieldData(13) = "'" & Now.ToString("yyyy/MM/dd hh:mm:ss") & "'"

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


    Public BeforePID As Integer()
    Public MyPID As Integer

    Public Sub MRComObject(Of T As Class)(ByRef objCom As T, Optional ByVal force As Boolean = False)
        If objCom Is Nothing Then
            Return
        End If
        Try
            If System.Runtime.InteropServices.Marshal.IsComObject(objCom) Then
                If force Then
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(objCom)
                Else
                    Dim count As Integer = System.Runtime.InteropServices.Marshal.ReleaseComObject(objCom)
                End If
            End If
        Finally
            objCom = Nothing
        End Try
    End Sub

    Public Sub GetExcelPID()

        Dim bRet As Boolean = True
        Dim localByName As Process() = Process.GetProcessesByName("Excel")
        ReDim BeforePID(localByName.Length)

        If localByName.Length = 0 Then
        Else
            For i As Integer = 0 To localByName.Length - 1
                BeforePID(i) = localByName(i).Id
            Next

        End If

    End Sub

    Public Sub GetExcelNewPID()

        Dim bRet As Boolean = True
        Dim localByName As Process() = Process.GetProcessesByName("Excel")
        Dim AfterPID As Integer()
        Dim flg_Excel_PID As Boolean = False

        localByName = Process.GetProcessesByName("Excel")
        ReDim AfterPID(localByName.Length)
        For i As Integer = 0 To localByName.Length - 1
            flg_Excel_PID = False
            AfterPID(i) = localByName(i).Id
            For j As Integer = 0 To UBound(BeforePID)
                If BeforePID(j) = AfterPID(i) Then
                    flg_Excel_PID = True
                    Exit For
                End If
            Next
            If flg_Excel_PID = False Then
                MyPID = AfterPID(i)
                Exit For
            End If
        Next

    End Sub

    Public Sub ExKillProcess()

        Try
            Dim localByPID As Process = Process.GetProcessById(MyPID)
            If localByPID.Id = MyPID Then
                Process.GetProcessById(MyPID).Kill()
            End If
            localByPID.Close()
            localByPID.Dispose()
        Catch ex As Exception

        End Try

    End Sub


    '20170330 baluu add start
    Public Sub correspond_3d_3d_weight(ByVal hv_PX As HTuple, ByVal hv_PY As HTuple, _
        ByVal hv_PZ As HTuple, ByVal hv_QX As HTuple, ByVal hv_QY As HTuple, ByVal hv_QZ As HTuple, _
        ByVal hv_WX As HTuple, ByVal hv_WY As HTuple, ByVal hv_WZ As HTuple, ByRef hv_HomMat3D As HTuple)

        ' Local control variables 
        Dim hv_P As Object = Nothing, hv_W As Object = Nothing
        Dim hv_PShift As Object = Nothing, hv_PMean As Object = Nothing
        Dim hv_Q As Object = Nothing, hv_QShift As Object = Nothing
        Dim hv_QMean As Object = Nothing, hv_M As Object = Nothing
        Dim hv_Index As Object = Nothing, hv_PVec As Object = Nothing
        Dim hv_QVec As Object = Nothing, hv_WVec As Object = Nothing
        Dim hv_Values As Object = Nothing, hv_PQ As Object = Nothing
        Dim hv_U As Object = Nothing, hv_S As Object = Nothing
        Dim hv_V As Object = Nothing, hv_R As Object = Nothing
        Dim hv_Value As Object = Nothing, hv_Value1 As Object = Nothing
        Dim hv_RPMean As Object = Nothing, hv_t As Object = Nothing
        Dim hv_HomMat3DID As Object = Nothing

        ' Initialize local and output iconic variables 

        HOperatorSet.CreateMatrix(3, BTuple.TupleLength(hv_WX), BTuple.TupleConcat(BTuple.TupleConcat( _
            hv_WX, hv_WY), hv_WZ), hv_W)
        HOperatorSet.CreateMatrix(3, BTuple.TupleLength(hv_PX), BTuple.TupleConcat(BTuple.TupleConcat( _
            hv_PX, hv_PY), hv_PZ), hv_P)
        shift_data_to_origin_weighted(hv_P, hv_W, hv_PShift, hv_PMean)
        'shift_data_to_origin (P, PShift, PMean)
        HOperatorSet.CreateMatrix(3, BTuple.TupleLength(hv_QX), BTuple.TupleConcat(BTuple.TupleConcat( _
            hv_QX, hv_QY), hv_QZ), hv_Q)
        shift_data_to_origin_weighted(hv_Q, hv_W, hv_QShift, hv_QMean)
        'shift_data_to_origin (Q, QShift, QMean)
        'Create matrix for rotational part.
        HOperatorSet.CreateMatrix(3, 3, 0, hv_M)
        For hv_Index = 0 To BTuple.TupleSub(BTuple.TupleLength(hv_PX), 1) Step 1
            HOperatorSet.GetSubMatrix(hv_PShift, 0, hv_Index, 3, 1, hv_PVec)
            HOperatorSet.GetSubMatrix(hv_QShift, 0, hv_Index, 3, 1, hv_QVec)
            HOperatorSet.GetSubMatrix(hv_W, 0, hv_Index, 3, 1, hv_WVec)
            HOperatorSet.MultElementMatrixMod(hv_QVec, hv_WVec)
            HOperatorSet.MultElementMatrixMod(hv_PVec, hv_WVec)
            HOperatorSet.TransposeMatrixMod(hv_QVec)
            HOperatorSet.MultMatrix(hv_PVec, hv_QVec, "AB", hv_PQ)
            HOperatorSet.AddMatrixMod(hv_M, hv_PQ)
            HOperatorSet.ClearMatrix(BTuple.TupleConcat(BTuple.TupleConcat(hv_PVec, hv_QVec), hv_PQ))
            HOperatorSet.ClearMatrix(hv_WVec)

#If USE_DO_EVENTS Then
    ' Please note: The call of DoEvents() is only a hack to
    ' enable VB to react on events. Please change the code
    ' so that it can handle events in a standard way.
    System.Windows.Forms.Application.DoEvents()
#End If
        Next
        'The left and right orthogonal matrices are extracted with SVD.
        HOperatorSet.SvdMatrix(hv_M, "full", "both", hv_U, hv_S, hv_V)
        HOperatorSet.TransposeMatrixMod(hv_U)
        'They give us the rotation.
        HOperatorSet.MultMatrix(hv_V, hv_U, "AB", hv_R)
        'Check: The determinant of a rotation matrix must be 1 by definition.
        HOperatorSet.DeterminantMatrix(hv_R, "general", hv_Value)
        If BTuple.TupleLess(hv_Value, 0).I Then
            HOperatorSet.GetValueMatrix(hv_V, BTuple.TupleConcat(BTuple.TupleConcat(0, 1), 2), BTuple.TupleConcat( _
                BTuple.TupleConcat(2, 2), 2), hv_Value1)
            HOperatorSet.SetValueMatrix(hv_V, BTuple.TupleConcat(BTuple.TupleConcat(0, 1), 2), BTuple.TupleConcat( _
                BTuple.TupleConcat(2, 2), 2), BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleNeg(BTuple.TupleSelect( _
                hv_Value1, 0)), BTuple.TupleNeg(BTuple.TupleSelect(hv_Value1, 1))), BTuple.TupleNeg(BTuple.TupleSelect( _
                hv_Value1, 2))))
            HOperatorSet.ClearMatrix(hv_R)
            HOperatorSet.MultMatrix(hv_V, hv_U, "AB", hv_R)
        End If
        'Extract final translational part.
        HOperatorSet.MultMatrix(hv_R, hv_PMean, "AB", hv_RPMean)
        HOperatorSet.SubMatrix(hv_QMean, hv_RPMean, hv_t)
        '
        'Create final affine matrix from rotation and translation.
        HOperatorSet.CreateMatrix(3, 4, 0, hv_HomMat3DID)
        HOperatorSet.SetSubMatrix(hv_HomMat3DID, hv_R, 0, 0)
        HOperatorSet.SetSubMatrix(hv_HomMat3DID, hv_t, 0, 3)
        HOperatorSet.GetFullMatrix(hv_HomMat3DID, hv_HomMat3D)
        'Delete all matrices that are used.
        HOperatorSet.ClearMatrix(BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat( _
            BTuple.TupleConcat(hv_P, hv_PShift), hv_PMean), hv_Q), hv_QShift), hv_QMean))
        HOperatorSet.ClearMatrix(BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat( _
            BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(hv_M, hv_PVec), hv_QVec), _
            hv_U), hv_S), hv_V), hv_R), hv_RPMean))
        HOperatorSet.ClearMatrix(BTuple.TupleConcat(hv_t, hv_HomMat3DID))

        Exit Sub
    End Sub


    '===============================================================
    ' 機　能：3点と軸方向を指定して座標変換マトリックスを取得する

    ' 戻り値：=0：正常終了

    '         =1：入力パラメタエラー（軸方向の組合せ）

    '         =2：入力パラメタエラー（  ）

    ' 引　数：

    '    posOrg   [I/ ] 原点
    '    pos1     [I/ ] 点１

    '    pos2     [I/ ] 点２

    '    iAxis1   [I/ ] 点１の軸方向（＋X:=+1、－X:=-1、＋Y:=+2、－Y:=-2、＋Z:=+3、－Z:=-3）

    '    iAxis2   [I/ ] 点２の軸方向（同上）

    '    iFixPos  [I/ ] 固定軸（=1:点１／=2:点２）

    '    dblMat() [ /O] 座標変換マトリックス（4×4）

    ' 備　考：

    '===============================================================
    Public Function YCM_Get3PosMatrix( _
        ByRef posOrg As CLookPoint, _
        ByRef pos1 As CLookPoint, _
        ByRef pos2 As CLookPoint, _
        ByRef iAxis1 As Integer, _
        ByRef iAxis2 As Integer, _
        ByRef iFixPos As Integer, _
        ByRef dblMat() As Double _
    ) As Integer
        'On Error GoTo Err_lbl
        Dim Xj(2) As Double, Yj(2) As Double, Zj(2) As Double '--設計点：Xj(),Yj(),Zj() baluu edit start (6 -> 2)
        Dim Xs(2) As Double, Ys(2) As Double, Zs(2) As Double '--計測点：Xs(),Ys(),Zs()
        Dim Xi(2) As Double, Yi(2) As Double, Zi(2) As Double '--重み　：Xi(),Yi(),Zi()
        Dim Xa(2) As Double, Ya(2) As Double, Za(2) As Double '--重みを考慮したジャストフィット計算結果：Xa(),Ya(),Za() baluu edit end

        YCM_Get3PosMatrix = -1

        '--1.指定された点と軸方向のチェック（指定された2点の軸方向が同じでない事を確認
        If (Math.Abs(iAxis1 - iAxis2) <= 0) Then
            YCM_Get3PosMatrix = 1
            Exit Function
        End If

        '--2.指定された３点で基準となる座標系を求める

        Dim dl1 As Double, dl2 As Double
        Xj(0) = 0.0# : Yj(0) = 0.0# : Zj(0) = 0.0#
        Xi(0) = 1.0# : Yi(0) = 1.0# : Zi(0) = 1.0#

        'Xi(0) = 100.0# : Yi(0) = 100.0# : Zi(0) = 100.0#
        'start 26:7:2011   16:31 by jf.w
        dl1 = pos1.distTo(posOrg) * Math.Sign(iAxis1)
        dl2 = pos2.distTo(posOrg) * Math.Sign(iAxis2)
        'dl1 = pos1.distTo(posOrg) * Sgn(iAxis1)
        'dl2 = pos2.distTo(posOrg) * Sgn(iAxis2)
        'end 26:7:2011   16:31 by jf.w
        Xi(1) = 1.0# : Yi(1) = 1.0# : Zi(1) = 1.0#
        Xi(2) = 1.0# : Yi(2) = 1.0# : Zi(2) = 1.0#
        Dim geoL As New GeoCurve, geoL_Tmp As New GeoCurve

        Dim geoVecParel As New GeoVector
        Dim geo_P As New GeoPoint, geo_PT As New GeoPoint
        Dim geoPOrg As New GeoPoint

        geoPOrg.setXYZ(posOrg.x, posOrg.y, posOrg.z)
        Dim lx As Double, ly As Double
        Dim vec1 As New GeoVector, vec2 As New GeoVector
        geoL.StartPoint.setXYZ(posOrg.x, posOrg.y, posOrg.z)

        If iFixPos = 1 Then
            Select Case (iAxis1)
                Case 1, -1
                    Xj(1) = dl1 : Yj(1) = 0.0# : Zj(1) = 0.0#

                Case 2, -2
                    Xj(1) = 0.0# : Yj(1) = dl1 : Zj(1) = 0.0#
                    'Xi(1) = 0.0# : Yi(1) = 100.0# : Zi(1) = 0.0#
                Case 3, -3
                    Xj(1) = 0.0# : Yj(1) = 0.0# : Zj(1) = dl1
                    'Xi(1) = 0.0# : Yi(1) = 0.0# : Zi(1) = 100.0#
                Case Else
                    YCM_Get3PosMatrix = 2
                    Exit Function
            End Select

            geoL.EndPoint.setXYZ(pos1.x, pos1.y, pos1.z)
            geo_P.setXYZ(pos2.x, pos2.y, pos2.z)
            geo_PT = geoL.GetPerpendicularFoot(geo_P)

            lx = geo_PT.GetDistanceTo(geoPOrg)
            ly = geo_PT.GetDistanceTo(geo_P)

            vec1.setXYZ(pos1.x - posOrg.x, pos1.y - posOrg.y, pos1.z - posOrg.z)
            vec2.setXYZ(geo_PT.x - posOrg.x, geo_PT.y - posOrg.y, geo_PT.z - posOrg.z)

            If vec1.GetInnerProduct(vec2) < 0 Then
                'If geoL.GetIsPointOnCurve(geo_PT, 0.001) = False Then
                lx = -lx
            End If

            Select Case (iAxis1)
                Case 1, -1
                    Xj(2) = lx * iAxis1
                Case 2, -2
                    Yj(2) = lx * iAxis1 / 2.0
                Case 3, -3
                    Zj(2) = lx * iAxis1 / 3.0
                Case Else
                    YCM_Get3PosMatrix = 2
                    Exit Function
            End Select

            Select Case (iAxis2)
                Case 1, -1
                    Xj(2) = ly * iAxis2
                Case 2, -2
                    Yj(2) = ly * iAxis2 / 2.0
                Case 3, -3
                    Zj(2) = ly * iAxis2 / 3.0
                Case Else
                    YCM_Get3PosMatrix = 2
                    Exit Function
            End Select

        Else
            geoL.EndPoint.setXYZ(pos2.x, pos2.y, pos2.z)
            geo_P.setXYZ(pos1.x, pos1.y, pos1.z)
            geo_PT = geoL.GetPerpendicularFoot(geo_P)
            ly = geo_PT.GetDistanceTo(geoPOrg)
            lx = geo_PT.GetDistanceTo(geo_P)
            If geoL.GetIsPointOnCurve(geo_P, 0.001) = False Then
                lx = -lx
            End If
            Select Case (iAxis1)
                Case 1, -1
                    Xj(1) = lx * iAxis1
                Case 2, -2
                    Yj(1) = lx * iAxis1 / 2.0
                Case 3, -3
                    Zj(1) = lx * iAxis1 / 3.0
                Case Else
                    YCM_Get3PosMatrix = 3
                    Exit Function
            End Select

            Select Case (iAxis2)
                Case 1, -1
                    Xj(1) = ly * iAxis2
                Case 2, -2
                    Yj(1) = ly * iAxis2 / 2.0
                Case 3, -3
                    Zj(1) = ly * iAxis2 / 3.0
                Case Else
                    YCM_Get3PosMatrix = 3
                    Exit Function
            End Select

            Select Case (iAxis2)
                Case 1, -1
                    Xj(2) = dl2 : Yj(2) = 0.0# : Zj(2) = 0.0#
                    'Xi(2) = 100.0# : Yi(2) = 0.0# : Zi(2) = 0.0#
                Case 2, -2
                    Xj(2) = 0.0# : Yj(2) = dl2 : Zj(2) = 0.0#
                    'Xi(2) = 0.0# : Yi(2) = 100.0# : Zi(2) = 0.0#
                Case 3, -3
                    Xj(2) = 0.0# : Yj(2) = 0.0# : Zj(2) = dl2
                    'Xi(2) = 0.0# : Yi(2) = 0.0# : Zi(2) = 100.0#
                Case Else
                    YCM_Get3PosMatrix = 3
                    Exit Function
            End Select

        End If
        '--3.計測点座標値の設定

        '-3.1 計測点３点
        Xs(0) = posOrg.x : Ys(0) = posOrg.y : Zs(0) = posOrg.z
        Xs(1) = pos1.x : Ys(1) = pos1.y : Zs(1) = pos1.z
        Xs(2) = pos2.x : Ys(2) = pos2.y : Zs(2) = pos2.z
        '-3.2 計測点に単位座標系のデータを追加
        '20170330 baluu del start
        'Xs(3) = 0.0# : Ys(3) = 0.0# : Zs(3) = 0.0#
        'Xs(4) = 1.0# : Ys(4) = 0.0# : Zs(4) = 0.0#
        'Xs(5) = 0.0# : Ys(5) = 1.0# : Zs(5) = 0.0#
        'Xs(6) = 0.0# : Ys(6) = 0.0# : Zs(6) = 1.0#
        '20170330 baluu del end
        '--4.重みを考慮したジャストフィット計算：結果：Xa(),Ya(),Za()
        Dim num As Integer
        num = 7
        'Dim hhmm As New HTuple
        'correspond_3d_3d_weight(Xs, Ys, Zs, Xj, Yj, Zj, Xi, Yi, Zi, hhmm)

        'iRet = OmomiExcute(num, Xi(0), Yi(0), Zi(0), Xj(0), Yj(0), Zj(0), Xa(0), Ya(0), Za(0), Xs(0), Ys(0), Zs(0))
        'ReDim Preserve Xs(2)
        'ReDim Preserve Ys(2)
        'ReDim Preserve Zs(2)
        'ReDim Preserve Xj(2)
        'ReDim Preserve Yj(2)
        'ReDim Preserve Zj(2)
        'ReDim Preserve Xi(2)
        'ReDim Preserve Yi(2)
        'ReDim Preserve Zi(2)
        Dim gmat As New GeoMatrix
        Dim hhmm As HTuple = Nothing
        correspond_3d_3d_weight(Xs, Ys, Zs, Xj, Yj, Zj, Xi, Yi, Zi, hhmm)
        Dim i As Integer, j As Integer
        For i = 1 To 4
            For j = 1 To 4
                gmat.SetAt(i, j, 0.0)
            Next j
        Next i
        For i = 1 To 4
            For j = 1 To 3
                gmat.SetAt(i, j, hhmm((j - 1) * 4 + (i - 1)))
            Next j
        Next i
        gmat.SetAt(4, 4, 1.0)
        '20170330 baluu del start
        'If iRet <> 0 Then
        'End If

        '--5.座標変換マトリックスを作成する

        'Dim gpOrg As New GeoPoint
        'Dim gvX As New GeoVector, gvY As New GeoVector, gvZ As New GeoVector
        'Call gpOrg.setXYZ(Xa(3), Ya(3), Za(3))
        'Call gvX.setXYZ(Xa(4), Ya(4), Za(4))
        'Call gvY.setXYZ(Xa(5), Ya(5), Za(5))
        'Call gvZ.setXYZ(Xa(6), Ya(6), Za(6))
        'Call gvX.SubtractPoint(gpOrg)
        'Call gvY.SubtractPoint(gpOrg)
        'Call gvZ.SubtractPoint(gpOrg)
        'Call gmat.SetCoordSystem(gpOrg, gvX, gvY, gvZ)
        ''gpOrg.Transform()
        ''gmat.PrintClass
        'Call gmat.Invert()
        ''sys_CoordInfo.mat_geo = gmat.Copy
        ''gmat.PrintClass
        '20170330 baluu del end

        '--6.結果を変換
        Dim ind As Integer
        ReDim dblMat(0 To 4 * 4)
        ind = -1
        For i = 1 To 4
            For j = 1 To 4
                ind = ind + 1
                dblMat(ind) = gmat.GetAt(i, j)
            Next j
        Next i
        YCM_Get3PosMatrix = 0
        Exit Function
Err_lbl:
    End Function



End Module
