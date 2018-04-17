Public Class CameraParam
    Private _X0 As Double
    Private _Y0 As Double
    Private _Z0 As Double
    Private _a0 As Double
    Private _b0 As Double
    Private _g0 As Double

    Private _c As Double
    Private _xp As Double
    Private _yp As Double
    Private _k1 As Double
    Private _k2 As Double
    Private _k3 As Double
    Private _p1 As Double
    Private _p2 As Double

    Public m11 As Double
    Public m12 As Double
    Public m13 As Double
    Public m21 As Double
    Public m22 As Double
    Public m23 As Double
    Public m31 As Double
    Public m32 As Double
    Public m33 As Double
   
    Public m11_d_a As Double
    Public m12_d_a As Double
    Public m13_d_a As Double
    Public m11_d_b As Double
    Public m12_d_b As Double
    Public m13_d_b As Double
    Public m11_d_g As Double
    Public m12_d_g As Double
    Public m13_d_g As Double
 
    Public m21_d_a As Double
    Public m22_d_a As Double
    Public m23_d_a As Double
    Public m21_d_b As Double
    Public m22_d_b As Double
    Public m23_d_b As Double
    Public m21_d_g As Double
    Public m22_d_g As Double
    Public m23_d_g As Double

    Public m31_d_a As Double
    Public m32_d_a As Double
    Public m33_d_a As Double
    Public m31_d_b As Double
    Public m32_d_b As Double
    Public m33_d_b As Double
    Public m31_d_g As Double
    Public m32_d_g As Double
    Public m33_d_g As Double

    Public Sub New()

    End Sub
    'Public Sub New(ByVal X As Double, _
    '                ByVal Y As Double, _
    '                ByVal Z As Double, _
    '                ByVal a As Double, _
    '                ByVal b As Double, _
    '                ByVal g As Double)
    '    _X0 = X
    '    _Y0 = Y
    '    _Z0 = Z
    '    _a0 = a
    '    _b0 = b
    '    _g0 = g

    '    CalcM()

    'End Sub

    Public Property X0() As Double
        Get
            Return _X0
        End Get
        Set(ByVal value As Double)
            _X0 = value
        End Set
    End Property
    Public Property Y0() As Double
        Get
            Return _Y0
        End Get
        Set(ByVal value As Double)
            _Y0 = value
        End Set
    End Property
    Public Property Z0() As Double
        Get
            Return _Z0
        End Get
        Set(ByVal value As Double)
            _Z0 = value
        End Set
    End Property
    Public Property A0() As Double
        Get
            Return _a0
        End Get
        Set(ByVal value As Double)
            '_a0 = value
            _a0 = CDbl(Tuple.TupleRad(value))
        End Set
    End Property
    Public Property B0() As Double
        Get
            Return _b0
        End Get
        Set(ByVal value As Double)
            '_b0 = value
            _b0 = CDbl(Tuple.TupleRad(value))
        End Set
    End Property
    Public Property G0() As Double
        Get
            Return _g0
        End Get
        Set(ByVal value As Double)
            '_g0 = value
            _g0 = CDbl(Tuple.TupleRad(value))
        End Set
    End Property

    Public Property c() As Double
        Get
            Return _c
        End Get
        Set(ByVal value As Double)
            _c = value
        End Set
    End Property
    Public Property xp() As Double
        Get
            Return _xp
        End Get
        Set(ByVal value As Double)
            _xp = value
        End Set
    End Property

    Public Property yp() As Double
        Get
            Return _yp
        End Get
        Set(ByVal value As Double)
            _yp = value
        End Set
    End Property
    Public Property k1() As Double
        Get
            Return _k1
        End Get
        Set(ByVal value As Double)
            _k1 = value
        End Set
    End Property
    Public Property k2() As Double
        Get
            Return _k2
        End Get
        Set(ByVal value As Double)
            _k2 = value
        End Set
    End Property
    Public Property k3() As Double
        Get
            Return _k3
        End Get
        Set(ByVal value As Double)
            _k3 = value
        End Set
    End Property
    Public Property p1() As Double
        Get
            Return _p1
        End Get
        Set(ByVal value As Double)
            _p1 = value
        End Set
    End Property
    Public Property p2() As Double
        Get
            Return _p2
        End Get
        Set(ByVal value As Double)
            _p2 = value
        End Set
    End Property


    Public Sub CalcM()
        '回転行列の成分と偏微分成分を計算する。
        'sin(x)->cos(x)
        'cos(x)->-sin(x)
        m11 = Math.Cos(_b0) * Math.Cos(_g0)
        m12 = -Math.Cos(_b0) * Math.Sin(_g0)
        m13 = Math.Sin(_b0)
        m21 = Math.Sin(_a0) * Math.Sin(_b0) * Math.Cos(_g0) + Math.Cos(_a0) * Math.Sin(_g0)
        m22 = Math.Cos(_a0) * Math.Cos(_g0) - Math.Sin(_a0) * Math.Sin(_b0) * Math.Sin(_g0)
        m23 = -Math.Sin(_a0) * Math.Cos(_b0)
        m31 = Math.Sin(_a0) * Math.Sin(_g0) - Math.Cos(_a0) * Math.Sin(_b0) * Math.Cos(_g0)
        m32 = Math.Cos(_a0) * Math.Sin(_b0) * Math.Sin(_g0) + Math.Sin(_a0) * Math.Cos(_g0)
        m33 = Math.Cos(_a0) * Math.Cos(_b0)

        m11_d_a = 0
        m12_d_a = 0
        m13_d_a = 0
        m31_d_a = Math.Cos(_a0) * Math.Sin(_g0) + Math.Sin(_a0) * Math.Sin(_b0) * Math.Cos(_g0)
        m32_d_a = -Math.Sin(_a0) * Math.Sin(_b0) * Math.Sin(_g0) + Math.Cos(_a0) * Math.Cos(_g0)
        m33_d_a = -Math.Sin(_a0) * Math.Cos(_b0)

        m11_d_b = -Math.Sin(_b0) * Math.Cos(_g0)
        m12_d_b = Math.Sin(_b0) * Math.Sin(_g0)
        m13_d_b = Math.Cos(_b0)
        m31_d_b = -Math.Cos(_a0) * Math.Cos(_b0) * Math.Cos(_g0)
        m32_d_b = Math.Cos(_a0) * Math.Cos(_b0) * Math.Sin(_g0)
        m33_d_b = -Math.Cos(_a0) * Math.Sin(_b0)

        m11_d_g = -Math.Cos(_b0) * Math.Sin(_g0)
        m12_d_g = -Math.Cos(_b0) * Math.Cos(_g0)
        m13_d_g = 0
        m31_d_g = Math.Sin(_a0) * Math.Cos(_g0) + Math.Cos(_a0) * Math.Sin(_b0) * Math.Sin(_g0)
        m32_d_g = Math.Cos(_a0) * Math.Sin(_b0) * Math.Cos(_g0) - Math.Sin(_a0) * Math.Sin(_g0)
        m33_d_g = 0

        m21_d_a = Math.Cos(_a0) * Math.Sin(_b0) * Math.Cos(_g0) - Math.Sin(_a0) * Math.Sin(_g0)
        m22_d_a = -Math.Sin(_a0) * Math.Cos(_g0) - Math.Cos(_a0) * Math.Sin(_b0) * Math.Sin(_g0)
        m23_d_a = -Math.Cos(_a0) * Math.Cos(_b0)

        m21_d_b = Math.Sin(_a0) * Math.Cos(_b0) * Math.Cos(_g0)
        m22_d_b = -Math.Sin(_a0) * Math.Cos(_b0) * Math.Sin(_g0)
        m23_d_b = Math.Sin(_a0) * Math.Sin(_b0)

        m21_d_g = -Math.Sin(_a0) * Math.Sin(_b0) * Math.Sin(_g0) + Math.Cos(_a0) * Math.Cos(_g0)
        m22_d_g = -Math.Cos(_a0) * Math.Sin(_g0) - Math.Sin(_a0) * Math.Sin(_b0) * Math.Cos(_g0)
        m23_d_g = 0

    End Sub

    Public Sub SetDeltaPose(ByVal Delta As Object)
        _X0 = _X0 + CDbl(Tuple.TupleSelect(Delta, 0))
        _Y0 = _Y0 + CDbl(Tuple.TupleSelect(Delta, 1))
        _Z0 = _Z0 + CDbl(Tuple.TupleSelect(Delta, 2))
        _a0 = _a0 + CDbl(Tuple.TupleSelect(Delta, 3))
        _b0 = _b0 + CDbl(Tuple.TupleSelect(Delta, 4))
        _g0 = _g0 + CDbl(Tuple.TupleSelect(Delta, 5))

    End Sub
    Public Sub SetDeltaCamPar(ByVal Delta As Object)
        _xp = _xp + CDbl(Tuple.TupleSelect(Delta, 0))
        _yp = _yp + CDbl(Tuple.TupleSelect(Delta, 1))
        _c = _c + CDbl(Tuple.TupleSelect(Delta, 2))
        _k1 = _k1 + CDbl(Tuple.TupleSelect(Delta, 3))
        _k2 = _k2 + CDbl(Tuple.TupleSelect(Delta, 4))
        _k3 = _k3 + CDbl(Tuple.TupleSelect(Delta, 5))
        _p1 = _p1 + CDbl(Tuple.TupleSelect(Delta, 6))
        _p2 = _p2 + CDbl(Tuple.TupleSelect(Delta, 7))

    End Sub


End Class
