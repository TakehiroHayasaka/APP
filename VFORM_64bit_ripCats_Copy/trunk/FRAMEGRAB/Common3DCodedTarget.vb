﻿Public Class Common3DCodedTarget
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

                For i = 0 To CodedTarget.CTnoSTnum - 1
                    Dim pnt3d As New Point3D
                    CalcNearest3dPointofRays(STs(i), pnt3d, AllDist)
                    meanerror = Tuple.TupleMean(AllDist) 'tuple の平均値を返す。
                    deverror = Tuple.TupleDeviation(AllDist)
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

End Class
