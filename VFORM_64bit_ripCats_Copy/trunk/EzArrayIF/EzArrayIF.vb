'' EZ-ARRAY用インタフェースクラス
Option Explicit On

Imports FieldTalk.Modbus.Master

Namespace EzArrayIF
    Public Structure SerialParameters
        Public PortName As String               'ポート名
        Public BaudRate As Integer              'ボーレート
        Public Parity As Integer                'パリティ
        Public DataBits As Integer              'データビット
        Public StopBits As Integer              'ストップビット
    End Structure

    Public Structure RegisterParameters
        Public PeakHeightAddress As Integer     'ピーク測定値の読み取りアドレス
        Public MonitorAddress As Integer        '監視用レジスタアドレス
        Public ErrorCodeAddress As Integer      'エラーコードの読み取りアドレス
        Public ServiceTimeAddress As Integer    '稼働時間の読み取りアドレス
        Public ScanTypeAddress As Integer       'スキャンタイプの変更アドレス
        Public ActiveDataAddress As Integer     'アクティブ計測値の読み取りアドレス(20150915 Tezuka ADD)
        Public IncidentAddress As Integer       '入光状態光軸総数の読み取りアドレス(20150916 Tezuka ADD)
    End Structure

    Public Class EzArrayIF
        Public Const SCAN_TYPE_STRAIGHT As Integer = 1
        Public Const SCAN_TYPE_SINGLE_EDGE As Integer = 2

        Private _rtuMasterProtocol As MbusRtuMasterProtocol         'Modbusマスタープロトコル(RTU)
        Private _modbusErrorCode As Integer                         'Modbusエラーコード
        Private _modbusErrorMessage As String                       'Modbusエラーメッセージ

        Public Property SerialParameters As SerialParameters        'シリアル通信のパラメータ
        Public Property SlaveAddress As Integer                     'Modbusスレーブアドレス
        Public Property BaseHeight As Integer                       '基準高さ(mm)
        Public Property Pitch As Double                             'センサーのピッチ(mm)
        Public Property RegisterParameters As RegisterParameters    'EZ-ARRAYのレジスター
        Public Property HideIncidentCount As Integer                '隠れている光軸数(20151028 ADD)

        '**
        '*  シリアルポートを開く.
        '*
        '*  @return =0:正常終了、<0:エラー終了  
        '*'
        Public Function OpenSerialPort() As Integer
            ClearErrorCode()

            'すでにポートが開いている場合は閉じる
            If Not _rtuMasterProtocol Is Nothing Then
                If _rtuMasterProtocol.isOpen() Then _rtuMasterProtocol.closeProtocol()
                _rtuMasterProtocol = Nothing
            End If

            'ポートを開く
            Try
                _rtuMasterProtocol = New MbusRtuMasterProtocol
            Catch ex As Exception
                _modbusErrorCode = -1
                _modbusErrorMessage = "Modbusプロトコルの作成に失敗しました。(" & ex.Message & ")"
                Return -1
            End Try

            Dim Ret As Integer
            Ret = _rtuMasterProtocol.openProtocol(SerialParameters.PortName, SerialParameters.BaudRate, SerialParameters.DataBits,
                                                  SerialParameters.StopBits, SerialParameters.Parity)
            If Not Ret = BusProtocolErrors.FTALK_SUCCESS Then
                _modbusErrorCode = Ret
                _modbusErrorMessage = "シリアルポートを開けませんでした。(" & BusProtocolErrors.getBusProtocolErrorText(Ret) & ")"
                Return -1
            End If

            Return 0
        End Function

        '**
        '*  ポートを閉じる.
        '*'
        Public Sub ClosePort()
            If Not _rtuMasterProtocol Is Nothing Then
                _rtuMasterProtocol.closeProtocol()
                _rtuMasterProtocol = Nothing
            End If
        End Sub

        '**
        '*  ポートが開いているかどうか.
        '*
        '*  @return =True:開いている、=False:閉じている
        '*'
        Public Function IsOpen() As Boolean
            If _rtuMasterProtocol Is Nothing THen
                Return False
            End If

            Return _rtuMasterProtocol.isOpen()
        End Function

        '**
        '*  ピーク測定値を読み取る.
        '*  ※レジスター値はリセットされる
        '*
        '*  @param  PeakHeight      [O]ピーク測定値(mm)
        '*  @return =0:正常終了、<0:エラー終了
        '*'
        Public Function ReadPeakHeight(ByRef PeakHeight As Double) As Integer
            ClearErrorCode()

            If _rtuMasterProtocol Is Nothing OrElse Not _rtuMasterProtocol.isOpen() THen
                _modbusErrorCode = -1
                _modbusErrorMessage = "ポートが開いていません。"
                Return -1
            End If

            '入力レジスターから値を取得する
            Dim ReadVals(0) As Short
            Dim Ret As Integer, NumRdRegs As Integer
            NumRdRegs = 1

            Ret = _rtuMasterProtocol.readInputRegisters(SlaveAddress, RegisterParameters.PeakHeightAddress,
                                                        ReadVals, NumRdRegs)
            If Not Ret = BusProtocolErrors.FTALK_SUCCESS Then
                _modbusErrorCode = Ret
                _modbusErrorMessage = "ピーク測定値の読み取りに失敗しました。(" & BusProtocolErrors.getBusProtocolErrorText(Ret) & ")"
                Return -1
            End If

            'ピーク測定値を計算する
            'If ReadVals(0) = 0.0 Then
            If ReadVals(0) <= HideIncidentCount * 4 Then        '(20151028 ADD)
                PeakHeight = 0.0
            Else
                PeakHeight = ReadVals(0) / 4 * Pitch + BaseHeight
            End If

            Return 0
        End Function

        '**
        '*  監視用レジスタの値を読み取る.
        '*
        '*  @param  Monitor      [O]監視用測定値(mm)
        '*  @return =0:正常終了、<0:エラー終了
        '*'
        Public Function ReadMonitorValue(ByRef Monitor As Double) As Integer
            ClearErrorCode()

            If _rtuMasterProtocol Is Nothing OrElse Not _rtuMasterProtocol.isOpen() Then
                _modbusErrorCode = -1
                _modbusErrorMessage = "ポートが開いていません。"
                Return -1
            End If

            '入力レジスターから値を取得する
            Dim ReadVals(0) As Short
            Dim Ret As Integer, NumRdRegs As Integer
            NumRdRegs = 1

            Ret = _rtuMasterProtocol.readInputRegisters(SlaveAddress, RegisterParameters.MonitorAddress,
                                                        ReadVals, NumRdRegs)
            If Not Ret = BusProtocolErrors.FTALK_SUCCESS Then
                _modbusErrorCode = Ret
                _modbusErrorMessage = "監視用測定値の読み取りに失敗しました。(" & BusProtocolErrors.getBusProtocolErrorText(Ret) & ")"
                Return -1
            End If

            'ピーク測定値を計算する
            'If ReadVals(0) = 0.0 Then
            If ReadVals(0) <= HideIncidentCount * 4 Then        '(20151028 ADD)
                Monitor = 0.0
            Else
                Monitor = ReadVals(0) / 4 * Pitch + BaseHeight
            End If

            Return 0
        End Function

        '**
        '*  アクティブ計測値を読み取る.
        '* (20150915 Tezuka ADD)
        '*  @param  ActiveHeight [O]アクティブ測定値(mm)
        '*  @return =0:正常終了、<0:エラー終了
        '*'
        Public Function ReadActiveHeight(ByRef ActiveHeight As Double) As Integer
            ClearErrorCode()

            If _rtuMasterProtocol Is Nothing OrElse Not _rtuMasterProtocol.isOpen() Then
                _modbusErrorCode = -1
                _modbusErrorMessage = "ポートが開いていません。"
                Return -1
            End If

            '入力レジスターから値を取得する
            Dim ReadVals(0) As Short
            Dim Ret As Integer, NumRdRegs As Integer
            NumRdRegs = 1

            Ret = _rtuMasterProtocol.readInputRegisters(SlaveAddress, RegisterParameters.ActiveDataAddress,
                                                        ReadVals, NumRdRegs)
            If Not Ret = BusProtocolErrors.FTALK_SUCCESS Then
                _modbusErrorCode = Ret
                _modbusErrorMessage = "アクティブ計測値の読み取りに失敗しました。(" & BusProtocolErrors.getBusProtocolErrorText(Ret) & ")"
                Return -1
            End If

            'ピーク測定値を計算する
            If ReadVals(0) = 0.0 Then
                ActiveHeight = 0.0
            Else
                ActiveHeight = ReadVals(0)
            End If

            Return 0
        End Function

        '**
        '*  入光状態光軸総数を読み取る.
        '* (20150916 Tezuka ADD)
        '*  @param  Incident　　 [O]入光状態光軸総数
        '*  @return =0:正常終了、<0:エラー終了
        '*'
        Public Function ReadIncident(ByRef Incident As Integer) As Integer
            ClearErrorCode()

            If _rtuMasterProtocol Is Nothing OrElse Not _rtuMasterProtocol.isOpen() Then
                _modbusErrorCode = -1
                _modbusErrorMessage = "ポートが開いていません。"
                Return -1
            End If

            '入力レジスターから値を取得する
            Dim ReadVals(0) As Short
            Dim Ret As Integer, NumRdRegs As Integer
            NumRdRegs = 1

            Ret = _rtuMasterProtocol.readInputRegisters(SlaveAddress, RegisterParameters.IncidentAddress,
                                                        ReadVals, NumRdRegs)
            If Not Ret = BusProtocolErrors.FTALK_SUCCESS Then
                _modbusErrorCode = Ret
                _modbusErrorMessage = "入光状態光軸総数の読み取りに失敗しました。(" & BusProtocolErrors.getBusProtocolErrorText(Ret) & ")"
                Return -1
            End If

            '入光状態光軸総数を計算する
            If ReadVals(0) = 0.0 Then
                Incident = 0
            Else
                Incident = Int(ReadVals(0) / 4)
                'Incident = Int(ReadVals(0))
            End If

            Return 0
        End Function

        '**
        '*  スキャンタイプをシングルエッジにする.
        '*
        '*  @return =0:正常終了、<0:エラー終了
        '*'
        Public Function ChangeSingleEdgeType() As Integer
            Return ChangeScanType(SCAN_TYPE_SINGLE_EDGE)
        End Function

        '**
        '*  スキャンタイプをストレートにする.
        '*
        '*  @return =0:正常終了、<0:エラー終了
        '*'
        Public Function ChangeStraightType() As Integer
            Return ChangeScanType(SCAN_TYPE_STRAIGHT)
        End Function

        '**
        '*  EZ-ARRAYのエラーを読み取る.
        '*
        '*  @param  Code        [O]エラーコード
        '*  @param  Message     [O]エラーメッセージ
        '*  @return =0:正常終了、<0:エラー終了
        '*'
        Public Function ReadSensorError(ByRef Code As Integer, ByRef Message As String) As Integer
            ClearErrorCode()

            If _rtuMasterProtocol Is Nothing OrElse Not _rtuMasterProtocol.isOpen() Then
                _modbusErrorCode = -1
                _modbusErrorMessage = "ポートが開いていません。"
                Return -1
            End If

            '入力レジスターから値を取得する
            Dim ReadVals(0) As Short
            Dim Ret As Integer, NumRdRegs As Integer
            NumRdRegs = 1

            Ret = _rtuMasterProtocol.readInputRegisters(SlaveAddress, RegisterParameters.ErrorCodeAddress,
                                                        ReadVals, NumRdRegs)
            If Not Ret = BusProtocolErrors.FTALK_SUCCESS Then
                _modbusErrorCode = Ret
                _modbusErrorMessage = "エリアセンサーのエラーの読み取りに失敗しました。(" & BusProtocolErrors.getBusProtocolErrorText(Ret) & ")"
                Return -1
            End If

            'エラーメッセージをセットする
            Code = (ReadVals(0) And &HFF00) >> 8
            Select Case Code
            Case 0
                Message = "システムOK"
            Case 1
                Message = "受光器EEPROMハード故障"
            Case 2
                Message = "受光器アライメント/ブランキング設定エラー"
            Case 3, 6, 7, 8, 9
                Message = "予備" & CStr(Code)
            Case 4
                Message = "投光器または配線の問題"
            Case 5
                Message = "投光器チャネルエラー"
            Case 10
                Message = "スキャンと計測モードが不適合"
            Case Else
                Message = "未定義のエラー"
            End Select

            Return 0
        End Function

        '**
        '*  稼働時間を読み取る.
        '*
        '*  @param  ServiceTime     [O]稼働時間(H)
        '*  @return =0:正常終了、<0:エラー終了
        '*'
        Public Function ReadServiceTime(ByRef ServiceTime As Integer) As Integer
            ClearErrorCode()

            If _rtuMasterProtocol Is Nothing OrElse Not _rtuMasterProtocol.isOpen() Then
                _modbusErrorCode = -1
                _modbusErrorMessage = "ポートが開いていません。"
                Return -1
            End If

            '保持レジスターから値を取得する
            Dim ReadVals(1) As Short
            Dim Ret As Integer, NumRdRegs As Integer
            NumRdRegs = 2

            Ret = _rtuMasterProtocol.readMultipleRegisters(SlaveAddress, RegisterParameters.ServiceTimeAddress,
                                                           ReadVals, NumRdRegs)
            If Not Ret = BusProtocolErrors.FTALK_SUCCESS Then
                _modbusErrorCode = Ret
                _modbusErrorMessage = "稼働時間の読み取りに失敗しました。(" & BusProtocolErrors.getBusProtocolErrorText(Ret) & ")"
                Return -1
            End If

            '稼働時間を計算する
            ServiceTime = ReadVals(1)
            ServiceTime = ServiceTime << 16
            ServiceTime = ServiceTime + ReadVals(0)

            Return 0
        End Function

        '**
        '*  Modbus通信エラーを読み取る.
        '*
        '*  @param  Code        [O]エラーコード
        '*  @param  Message     [O]エラーメッセージ
        '*  @return =0:正常終了、<0:エラー終了
        '*'
        Public Function ReadModbusError(ByRef Code As Integer, ByRef Message As String) As Integer
            Code = _modbusErrorCode
            Message = _modbusErrorMessage

            Return 0
        End Function

        '**
        '*  スキャンタイプを読み取る.
        '*
        '*  @param  ScanType     [O]スキャンタイプ
        '*  @return =0:正常終了、<0:エラー終了
        '*'
        Public Function ReadScanType(ByRef ScanType As Integer) As Integer
            ClearErrorCode()

            If _rtuMasterProtocol Is Nothing OrElse Not _rtuMasterProtocol.isOpen() Then
                _modbusErrorCode = -1
                _modbusErrorMessage = "ポートが開いていません。"
                Return -1
            End If

            '保持レジスターから値を取得する
            Dim ReadVals(0) As Short
            Dim Ret As Integer, NumRdRegs As Integer
            NumRdRegs = 1

            Ret = _rtuMasterProtocol.readMultipleRegisters(SlaveAddress, RegisterParameters.ScanTypeAddress,
                                                           ReadVals, NumRdRegs)
            If Not Ret = BusProtocolErrors.FTALK_SUCCESS Then
                _modbusErrorCode = Ret
                _modbusErrorMessage = "スキャンタイプの取得に失敗しました。(" & BusProtocolErrors.getBusProtocolErrorText(Ret) & ")"
                Return -1
            End If

            ScanType = ReadVals(0)

            Return 0
        End Function

        Private Sub ClearErrorCode()
            _modbusErrorCode = 0
            _modbusErrorMessage = ""
        End Sub

        Private Function ChangeScanType(ByVal ScanType As Integer) As Integer
            ClearErrorCode()

            If _rtuMasterProtocol Is Nothing OrElse Not _rtuMasterProtocol.isOpen() Then
                _modbusErrorCode = -1
                _modbusErrorMessage = "ポートが開いていません。"
                Return -1
            End If

            '保持レジスターから値を取得する
            Dim ReadVals(0) As Short
            Dim Ret As Integer, NumRdRegs As Integer
            NumRdRegs = 1

            Ret = _rtuMasterProtocol.readMultipleRegisters(SlaveAddress, RegisterParameters.ScanTypeAddress,
                                                           ReadVals, NumRdRegs)
            If Not Ret = BusProtocolErrors.FTALK_SUCCESS Then
                _modbusErrorCode = Ret
                _modbusErrorMessage = "スキャンタイプの変更に失敗しました。(" & BusProtocolErrors.getBusProtocolErrorText(Ret) & ")"
                Return -1
            End If

            '保持レジスターの値を変更する
            Dim WriteVals(0) As Short
            Dim NumWtRegs As Integer
            NumWtRegs = 1

            WriteVals(0) = ReadVals(0) And &HFF00
            WriteVals(0) = WriteVals(0) Or ScanType

            Ret = _rtuMasterProtocol.writeMultipleRegisters(SlaveAddress, RegisterParameters.ScanTypeAddress,
                                                            WriteVals, NumWtRegs)
            If Not Ret = BusProtocolErrors.FTALK_SUCCESS Then
                _modbusErrorCode = Ret
                _modbusErrorMessage = "スキャンタイプの変更に失敗しました。(" & BusProtocolErrors.getBusProtocolErrorText(Ret) & ")"
                Return -1
            End If

            Return 0
        End Function
    End Class

End Namespace