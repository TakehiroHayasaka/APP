
Public Class Common3DSingleTarget
    ''' <summary>
    ''' STのラベル（数値のみ）
    ''' </summary>
    Public PID As Integer
    Public P3d As Point3D
    Public realP3d As Point3D
    ''' <summary>
    ''' STの画像上（2次元）の座標値
    ''' </summary>
    Public tmpImgPnt As ImagePoints
    Public lstST As List(Of SingleTarget)
    ''' <summary>
    ''' 1計測点に対する全計測点要素
    ''' </summary>
    Public AllDist As Object
    ''' <summary>
    ''' STの3次元座標値のズレ（距離）の平均
    ''' </summary>
    Public meanerror As Object
    ''' <summary>
    ''' STの3次元座標値の標準偏差を返す。
    ''' </summary>
    Public deverror As Object

    Public maxProjErrST As SingleTarget

    ''' <summary>
    ''' 初期化
    ''' </summary>

    Public Sub New()
        If lstST Is Nothing Then
            lstST = New List(Of SingleTarget)
        Else
            lstST.Clear()
        End If

        P3d = New Point3D
        realP3d = New Point3D
        tmpImgPnt = New ImagePoints
        PID = -1
        AllDist = Nothing
        _currentLabel = ""
    End Sub
    Public Sub RemoveBadData()
        Dim i As Integer
        For i = lstST.Count - 1 To 0 Step -1
            Dim ST As SingleTarget
            ST = lstST.Item(i)

            If ST.P2ID = -1 Or ST.ImageID = -1 Or (ST.RayP1.X Is Nothing) Then
                lstST.RemoveAt(i)
            End If
        Next
    End Sub
    Private _currentLabel As String
    ''' <summary>
    ''' _currentLabelに番号を振る
    ''' </summary>
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
    Private _STtype As Integer = 1
    Public Property STtype() As Integer
        Get
            Return _STtype
        End Get
        Set(value As Integer)
            _STtype = value
        End Set
    End Property

    ''' <summary>
    ''' 「ST」+番号を振る
    ''' </summary>
    Public ReadOnly Property systemlabel() As String
        Get
            If PID <> -1 Then
                If _STtype = 1 Then
                    systemlabel = "ST" & PID
                Else
                    systemlabel = "UP" & PID
                End If

            Else
                systemlabel = ""
            End If
        End Get
    End Property
    Public ReadOnly Property TID() As Integer
        Get
            Return PID + 10000
        End Get
    End Property

    ''' <summary>
    ''' STを計算する
    ''' </summary>
    Public Function Calc3dPoints() As Boolean
        Calc3dPoints = False
        '  RemoveBadData()
        If lstST.Count >= 2 Then '計測点につき2枚以上だとOK
            If CalcNearest3dPointofRays(lstST, P3d, AllDist) = True Then
                meanerror = BTuple.TupleMean(AllDist) 'tuple の平均値を返す。
                deverror = BTuple.TupleDeviation(AllDist) ' tuple の要素の標準偏差を返す。
            Else
                Exit Function
            End If
        End If
        Calc3dPoints = True
    End Function

    ''' <summary>
    ''' STを削除するかどうか
    ''' </summary>
    Public Function CheckAndRemoveBadSingleTarget(ByVal KyoyoGosa As Double) As Boolean
        CheckAndRemoveBadSingleTarget = True
        Dim i As Integer = 0
        Dim IsRemove As Boolean = False
        Dim maxDist As Double = 0
        Dim maxDistIndex As Integer = -1
        Dim maxReProjErr As Double = 0
        Dim maxReProjErrIndex As Integer = -1
        i = 0
        For Each ST As SingleTarget In lstST
            If ST.Dist > maxDist Then
                maxDist = ST.Dist.D
                maxDistIndex = i
            End If
            If ST.ReProjectionError > maxReProjErr Then
                maxReProjErr = ST.ReProjectionError.D
                maxReProjErrIndex = i
            End If
            i += 1
        Next
        If lstST.Count = 2 Then
            If maxDist > KyoyoGosa Then
                RemoveSTByIndex(maxDistIndex)
                IsRemove = True
            End If
        Else
            If maxDist < KyoyoGosa Then
                If (maxDist > (meanerror + deverror * 5).D Or maxReProjErr > 1) Then '20170605 SUURI コンバートミスを修正
                    If maxDistIndex = maxReProjErrIndex Then
                        RemoveSTByIndex(maxDistIndex)
                    Else
                        If maxDist / (meanerror + deverror * 5).D > maxReProjErrIndex / 1 Then
                            RemoveSTByIndex(maxDistIndex)
                        Else
                            RemoveSTByIndex(maxReProjErrIndex)
                        End If
                    End If
                    IsRemove = True
                End If
            Else
                RemoveSTByIndex(maxDistIndex)
                IsRemove = True
            End If
        End If
        'If maxReProjErr > 0.5 Then
        '    lstST.Item(maxReProjErrIndex).P3ID = -1
        '    lstST.Item(maxReProjErrIndex).flgUsed = 0
        '    lstST.RemoveAt(maxReProjErrIndex)
        '    IsRemove = True
        'End If

        If IsRemove = False Or lstST.Count < 2 Then
            CheckAndRemoveBadSingleTarget = False
        End If

    End Function

    ''' <summary>
    ''' 指定したIndexからSTを削除する
    ''' </summary>
    Private Sub RemoveSTByIndex(ByVal Index As Integer)
        If lstST.Item(Index).ImageID = 1 And lstST.Item(Index).P2ID = 82 Then
            Index = Index
        End If
        lstST.RemoveAt(Index) '指定したインデックスにある要素を削除します。
    End Sub

    ''' <summary>
    ''' 指定したSTからSTを削除する
    ''' </summary>
    Public Sub RemoveSTByST(ByVal ST As SingleTarget)
        Dim i As Integer
        For i = lstST.Count - 1 To 0 Step -1
            If lstST.Item(i).IsSamePoint(ST) Then
                lstST.RemoveAt(i) '指定したインデックスにある要素を削除します。
                Exit For
            End If
        Next
    End Sub

    ''' <summary>
    ''' 同じSTがあるかどうか
    ''' </summary>
    Public Function SamePointAri(ByVal objST As SingleTarget) As Boolean
        SamePointAri = False
        For Each ST As SingleTarget In lstST
            If ST.IsSamePoint(objST) Then
                SamePointAri = True
                Exit For
            End If
        Next

    End Function
    ''' <summary>
    ''' 未使用
    ''' </summary>
    Public Function SameImagePointAri(ByVal objST As SingleTarget) As Boolean
        SameImagePointAri = False
        For Each ST As SingleTarget In lstST
            If ST.ImageID = objST.ImageID Then
                SameImagePointAri = True
                Exit For
            End If
        Next

    End Function
End Class
