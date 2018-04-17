Imports HalconDotNet

Public Class ImagePoints

    ''' <summary>
    ''' 2次元画像の行の要素（Row：縦に進む）
    ''' </summary>
    Public Row As HTuple
    ''' <summary>
    ''' 2次元画像の列の要素（Column：横に進む）
    ''' </summary>
    Public Col As HTuple
    ''' <summary>
    ''' 測点数
    ''' </summary>
    Public ReadOnly Property CountPoints() As Object
        Get
            'Dim tttemp() As Object
            'tttemp = CObj(Row)
            Return UBound(Row.DArr) + 1
            'Return tttemp.Length   '(行)の要素数=測点数を返す。
        End Get
    End Property

    ''' <summary>
    ''' Row，Colの初期化
    ''' </summary>
    Public Sub New()
        Row = Nothing
        Col = Nothing
    End Sub
    Public Sub New(ByVal R As HTuple, ByVal C As HTuple)
        Row = R
        Col = C
    End Sub
    ''' <summary>
    ''' ※わからない（H25.4.8 山田）（？）
    ''' </summary>
    Public Sub GetPointsByCoord(ByVal SRow As Object, ByVal SCol As Object, ByRef Index As Object)
        Dim ind1 As Object = Nothing
        Dim ind2 As Object = Nothing
        Dim ind As Object = Nothing
        Dim i As Integer
        ind1 = BTuple.TupleFind(Row, SRow)

        If BTuple.TupleLength(ind1) = 1 Then
            Index = ind1 '要素数 ＝ 1
            Exit Sub
        End If
        ind2 = BTuple.TupleFind(Col, SCol)
        If BTuple.TupleLength(ind2) = 1 Then
            Index = ind2 '要素数 ＝ 1
            Exit Sub
        End If
        For i = 0 To BTuple.TupleLength(ind1) - 1
            ind = BTuple.TupleFind(ind2, BTuple.TupleSelect(ind1, i))
            If BTuple.TupleSelect(ind, 0) = -1 Then
            Else
                Index = BTuple.TupleSelect(ind1, i)
            End If
        Next

        '①TupleFind（t1,t2,indices）
        '：1 つ目の tuple”t1” 内から 2 つ目の tuple”t2” を探索し、1 つ目の入力 tuple”t1” に一致した要素のインデックスを ”indices”に返します。

        '（例）
        'TupleFind(HTuple t1, HTuple t2, out HTuple indices)
        '入力するtuple， ”t1”が [3,4,5,6,1,2,3,4,0] ， ”t2”が [3,4]⇒出力するtuple”indices”は[0,6] 


        '②TupleLength(tuple, length)
        '：入力 ”tuple”の要素数を”length”に返します。

        '③TupleSelect(tuple, index, selected)
        '：入力”tuple”の一つ、または複数の要素”index”を選択し、”selected”へ返す。
        'H25.4.8　山田

    End Sub

    ''' <summary>
    ''' 2点間の距離を計算
    ''' </summary>
    Public Sub CalcDistToInputPoint(ByVal R As Object, ByVal C As Object, ByRef Dist As Object)
        Dist = BTuple.TupleSqrt(BTuple.TupleAdd(BTuple.TuplePow(BTuple.TupleSub(Row, R), 2), _
                       BTuple.TuplePow(BTuple.TupleSub(Col, C), 2))).D

        'R：？
        'C：？
        'Dist：2点間の距離

        '2点は（Col,Row），（C,R）⇒???


        'TupleSqrt：tupleの平方根を計算する。
        'TupleAdd：2つのtuple の和を計算する。
        'TuplePow：入力tupleべき値を計算する。
        '  （例）TuplePow(HTuple t1, HTuple t2, out HTuple pow)
        '   (t1)^t2
        'TupleSub：2つのtupleの差分を計算。
        'H25.4.8（山田）



    End Sub

    ''' <summary>
    ''' ※データの書き込み（？）
    ''' </summary>
    Public Sub SaveData(ByVal strPath As String)

        SaveTupleObj(Row, strPath & "_Row.tpl")
        SaveTupleObj(Col, strPath & "_Col.tpl")

    End Sub
    ''' <summary>
    ''' ※データの読込み（？）
    ''' </summary>
    Public Sub ReadData(ByVal StrPath As String)
        ReadTupleObj(Row, StrPath & "_Row.tpl")
        ReadTupleObj(Col, StrPath & "_Col.tpl")
    End Sub

    ''' <summary>
    ''' ※2つの配列や要素を連結する（？）
    ''' </summary>
    Public Sub ConcatToMe(ByVal ImpP As ImagePoints)
        Row = BTuple.TupleConcat(Row, ImpP.Row)
        Col = BTuple.TupleConcat(Col, ImpP.Col)

        'TupleConcat：2つのtupleを連結する

    End Sub

    Public Sub CopyToMe(ByVal objP2D As ImagePoints)
        Row = objP2D.Row
        Col = objP2D.Col
    End Sub
End Class
