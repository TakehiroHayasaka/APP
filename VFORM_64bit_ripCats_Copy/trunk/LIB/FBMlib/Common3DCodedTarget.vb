Public Class Common3DCodedTarget
    ''' <summary>
    ''' コードターゲットのラベル（数値のみ）
    ''' </summary>
    Private _pid As Integer
    Public Property PID() As Integer
        Get
            Return _pid
        End Get
        Set(ByVal value As Integer)
            _pid = value
        End Set
    End Property

    Public tmpImgPnt As ImagePoints
    Public lstP3d As List(Of Point3D)
    Public lstCT As List(Of CodedTarget)
    Public lstRealP3d As List(Of Point3D)
    Public NormalV As Point3D
    Public OffsetV As Point3D
    Public K1 As Integer
    Public K2 As Integer
    Public info As String ' 20160316 add
    Public CT_No As Integer
    '  Public lstST As List(Of SingleTarget)
    'Public currentLabel As String

    Public flgUsable As Boolean

    Public AllDist As Object
    Public meanerror As Object
    Public deverror As Object
    Public Sub New()
        If lstCT Is Nothing Then
            lstCT = New List(Of CodedTarget)
        Else
            lstCT.Clear()
        End If
        If lstP3d Is Nothing Then
            lstP3d = New List(Of Point3D)
        Else
            lstP3d.Clear()
        End If
        If lstRealP3d Is Nothing Then
            lstRealP3d = New List(Of Point3D)
        Else
            lstRealP3d.Clear()
        End If
        NormalV = New Point3D
        OffsetV = New Point3D


        tmpImgPnt = New ImagePoints
        PID = -1
        flgUsable = False
        _currentLabel = ""
    End Sub

    ''' <summary>
    ''' コードターゲットのラベル（CT + 数値）
    ''' </summary>
    Private _currentLabel As String
    Public Property currentLabel() As String
        Get
            If _currentLabel = "" Then
                Return systemlabel
            Else
                Return _currentLabel
            End If
        End Get
        Set(ByVal value As String)
            _currentLabel = value
        End Set
    End Property

    Public Sub Calc3dPoints()
        If lstCT.Count >= 2 Then
            Dim STs(CodedTarget.CTnoSTnum - 1) As List(Of SingleTarget)
            Dim i As Integer = 0
            For i = 0 To CodedTarget.CTnoSTnum - 1
                STs(i) = New List(Of SingleTarget)
            Next
            For Each CT As CodedTarget In lstCT
                If CT.CT_ID = 223 Then
                    CT.CT_ID = 223
                    ' Stop
                End If
                If CT.flgUsable = True Then
                    i = 0
                    For Each ST As SingleTarget In CT.lstCTtoST
                        STs(i).Add(ST)
                        i += 1
                    Next
                End If
            Next
            If STs(0).Count >= 2 Then
                If lstP3d Is Nothing Then
                    lstP3d = New List(Of Point3D)
                Else
                    lstP3d.Clear()
                End If
                If PID = 58 Then
                    PID = PID
                End If
                For i = 0 To CodedTarget.CTnoSTnum - 1
                    Dim pnt3d As New Point3D
                    CalcNearest3dPointofRays(STs(i), pnt3d, AllDist)
                    meanerror = BTuple.TupleMean(AllDist) 'tuple の平均値を返す。
                    deverror = BTuple.TupleDeviation(AllDist)
                    If meanerror > 0.1 Then
                        meanerror = meanerror
                    End If
                    lstP3d.Add(pnt3d)
                Next
                flgUsable = True
            End If

        End If
    End Sub
    Public ReadOnly Property systemlabel() As String
        Get
            If PID <> -1 Then
                systemlabel = "CT" & PID
            Else
                systemlabel = ""
            End If
        End Get
    End Property

    'SUURI ADD CTの枠の4点の座標を算出する関数　START 2017.05.07
    Public Function GenWakuCoord() As List(Of Point3D)
        GenWakuCoord = New List(Of Point3D)
        Dim hv_ALLCTID As New Object
        hv_ALLCTID = BTuple.ReadTuple(My.Application.Info.DirectoryPath & "\RectangleCT_ID.tup")
        Dim i As Integer
        Dim j As Integer
        Dim PkanKyori As Double = 10
        Dim Dummy10(4, 4) As ImagePoints
        For i = 0 To 4
            For j = 0 To 4
                Dim D As New ImagePoints
                D.Row = i * PkanKyori
                D.Col = j * PkanKyori

                Dummy10(i, j) = D

            Next
        Next

        Dim CTPointhaiti As Object = BTuple.TupleSelect(hv_ALLCTID, PID - 1)
        Dim CTHaiti(CodedTarget.CTnoSTnum - 1) As String
        CTHaiti = CStr(CTPointhaiti.S).Split("_")
        Dim II As Integer = CInt(CTHaiti(0)) / 5
        Dim JJ As Integer = CInt(CTHaiti(0)) Mod 5
        Dim P2 As New ImagePoints
        Dim Qx As New HalconDotNet.HTuple
        Dim Qy As New HalconDotNet.HTuple
        Dim Qz As New HalconDotNet.HTuple
        Dim Px As New HalconDotNet.HTuple
        Dim Py As New HalconDotNet.HTuple
        Dim Pz As New HalconDotNet.HTuple

        For i = 0 To CodedTarget.CTnoSTnum - 1
            II = CInt(CInt(CTHaiti(i)) / 5) - 1

            JJ = IIf((CInt(CTHaiti(i)) Mod 5) = 0, 4, (CInt(CTHaiti(i)) Mod 5) - 1)

            P2 = Dummy10(II, JJ)
            Px = Px.TupleConcat(P2.Col)
            Py = Py.TupleConcat(P2.Row)
            Pz = Pz.TupleConcat(New HalconDotNet.HTuple(0.0))

            Qx = Qx.TupleConcat(lstP3d.Item(i).X)
            Qy = Qy.TupleConcat(lstP3d.Item(i).Y)
            Qz = Qz.TupleConcat(lstP3d.Item(i).Z)


        Next
        Try
            Dim objHomMat3d As New HalconDotNet.HTuple
            HalconDotNet.HOperatorSet.VectorToHomMat3d("similarity", Px, Py, Pz, Qx, Qy, Qz, objHomMat3d)

            Dim WakuX As New HalconDotNet.HTuple({1, 2, 3, 4})
            Dim WakuY As New HalconDotNet.HTuple({1, 2, 3, 4})
            Dim WakuZ As New HalconDotNet.HTuple({0.0, 0.0, 0.0, 0.0})

            Dim WakuResX As New HalconDotNet.HTuple({1, 2, 3, 4})
            Dim WakuResY As New HalconDotNet.HTuple({1, 2, 3, 4})
            Dim WakuResZ As New HalconDotNet.HTuple({0.0, 0.0, 0.0, 0.0})

            HalconDotNet.HOperatorSet.AffineTransPoint3d(objHomMat3d, WakuX, WakuY, WakuZ, WakuResX, WakuResY, WakuResZ)
            For i = 0 To CodedTarget.CTnoSTnum - 1
                Dim WakuPoint As New Point3D(WakuResX.TupleSelect(i), WakuResY.TupleSelect(i), WakuResZ.TupleSelect(i))
                GenWakuCoord.Add(WakuPoint)
            Next
        Catch ex As Exception

        End Try

    End Function
    'SUURI ADD CTの枠の4点の座標を算出する関数　END 2017.05.11
End Class
