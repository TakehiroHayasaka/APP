
Imports System.Runtime.InteropServices
Imports System
Imports System.IO
Imports System.Diagnostics
Imports Microsoft.VisualBasic.FileIO
Imports System.Text
Imports System.Text.RegularExpressions

Public Enum TGType As Integer
    OldTG = 0 '1～1503番までの旧型ターゲット(撮影限界距離4m)
    NewTG = 1 '753～1503番までの新型ターゲット(撮影限界距離約5m)
    FreeTG = 2
End Enum


Public Class TargetDetectParam

    '使用するターゲットの形状を定義 

    'OldTG設定
    Public Const OldTargetUchiWakuSize As Integer = 350
    Public Const OldPaternGrid As Integer = 50
    Public Const OldPaternGridInterval As Integer = 50

    'NewTG設定
    Public Const NewTargetUchiWakuSize As Integer = 490
    Public Const NewPaternGrid As Integer = 66
    Public Const NewPaternGridInterval As Integer = 86

    'FreeTG設定
    Public TargetUchiWakuSize As Integer
    Public PaternGrid As Integer
    Public PaternGridInterval As Integer


    'iniファイル　文字列読込用バッファサイズ
    Private Const BufSize As Integer = 256

    'ターゲット全般設定
    Public TargetType As Integer '新旧ターゲットの選択

    'ターゲット抽出 全般
    '銀丸サイズ閾値
    Public SSAreaThreshold As Integer

    'segment_contours_xld
    Public Mode As String
    Public SmoothCont As Integer
    Public MaxLineDist1 As Double
    Public MaxLineDist2 As Double
    Public Declare Ansi Function GetPrivateProfileString Lib "kernel32.dll" Alias "GetPrivateProfileStringA" (ByVal section As String, ByVal key As String, ByVal def As String, ByVal retVal As StringBuilder, ByVal size As Integer, ByVal filePath As String) As Integer

    '*****************************
    '4点TG認識パラメータ
    '*****************************
    Public minAreaFourTen As Integer
    Public maxAreaFourTen As Integer
    Public dblsetAreaHiritu As Double
    Public dilationRadius As Integer
    Public dblsetArea012 As Double
    Public dblsetArea012_dev As Double
    Public dblsetAreaBigST As Double
    Public STConnectionRimit As Integer
    Public circularity As Double


    Public Sub New()

        'RoadFourTenTarget()

        'Try
        '    ReadVFORMini()
        'Catch ex As Exception
        '    MsgBox("VFORM.iniの読込に失敗しました。(カメラパラメータ設定)")
        'End Try

    End Sub

    Public Sub RoadFourTenTargetTxt()

        dilationRadius = CInt(ReadFourTenTarget(5))
        minAreaFourTen = CInt(ReadFourTenTarget(6))
        maxAreaFourTen = CInt(ReadFourTenTarget(7))

        dblsetAreaHiritu = CDbl(ReadFourTenTarget(1))
        dblsetArea012 = CDbl(ReadFourTenTarget(2))
        dblsetArea012_dev = CDbl(ReadFourTenTarget(3))

        dblsetAreaBigST = CDbl(ReadFourTenTarget(4))

        STConnectionRimit = CDbl(ReadFourTenTarget(8))
        circularity = CDbl(ReadFourTenTarget(9))

    End Sub


    'Public Sub ReadVFORMini()
    '    'ターゲット全般設定
    '    Dim Buf As StringBuilder = New StringBuilder(BufSize)
    '    GetPrivateProfileString("TargetDetectParam", "TagetType", "", Buf, Buf.Capacity, My.Application.Info.DirectoryPath & "\vform.ini")
    '    Dim dow As TGType
    '    If [Enum].TryParse(Of TGType)(Buf.ToString, dow) Then
    '        TargetType = dow
    '    Else
    '        MsgBox("TGTypeを設定に失敗しました。OldCTに設定します。")
    '        TargetType = TGType.OldTG
    '    End If

    '    '銀丸サイズ閾値
    '    SSAreaThreshold = GetPrivateProfileInt("TargetDetectParam", "SSAreaThreshold", -1, My.Application.Info.DirectoryPath & "\vform.ini")


    '    'segment_contours_xld
    '    Dim sb1 As StringBuilder = New StringBuilder(BufSize)
    '    Dim Mode_size As Long
    '    Mode_size = GetPrivateProfileString("TargetDetectParam", "Mode", "", sb1, sb1.Capacity, My.Application.Info.DirectoryPath & "\vform.ini")
    '    Mode = sb1.ToString
    '    SmoothCont = GetPrivateProfileInt("TargetDetectParam", "SmoothCont", -1, My.Application.Info.DirectoryPath & "\vform.ini")
    '    MaxLineDist1 = GetPrivateProfileInt("TargetDetectParam", "MaxLineDist1", -1, My.Application.Info.DirectoryPath & "\vform.ini")
    '    MaxLineDist2 = GetPrivateProfileInt("TargetDetectParam", "MaxLineDist2", -1, My.Application.Info.DirectoryPath & "\vform.ini")

    '    '設定したTGタイプで読み込むパラメータを変更
    '    Select Case TargetType
    '        Case TGType.OldTG
    '            TargetUchiWakuSize = OldTargetUchiWakuSize
    '            PaternGrid = OldPaternGrid
    '            PaternGridInterval = OldPaternGridInterval
    '        Case TGType.NewTG
    '            TargetUchiWakuSize = NewTargetUchiWakuSize
    '            PaternGrid = NewPaternGrid
    '            PaternGridInterval = NewPaternGridInterval
    '        Case TGType.FreeTG
    '            TargetUchiWakuSize = GetPrivateProfileInt("TargetDetectParam", "TargetUchiWakuSize", -1, My.Application.Info.DirectoryPath & "\vform.ini")
    '            PaternGrid = GetPrivateProfileInt("TargetDetectParam", "PaternGrid", -1, My.Application.Info.DirectoryPath & "\vform.ini")
    '            PaternGridInterval = GetPrivateProfileInt("TargetDetectParam", "PaternGridInterval", -1, My.Application.Info.DirectoryPath & "\vform.ini")
    '    End Select

    'End Sub

End Class
