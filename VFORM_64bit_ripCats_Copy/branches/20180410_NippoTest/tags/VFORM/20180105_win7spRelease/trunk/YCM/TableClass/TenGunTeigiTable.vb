﻿Imports FBMlib
Imports Microsoft.VisualBasic.FileIO


Public Class TenGunTeigiTable

    Private Const T_tengunteigi As String = "TenGunTeigi"

    Private ReadOnly T_ID As String() = {"ID"}                                          '0  ID
    Private ReadOnly T_Type_ID As String() = {"タイプＩＤ"}                                 '1  TypeID
    Private ReadOnly T_TenGunID As String() = {"点群ＩＤ", "TenGunID"}                  '2  点群ＩＤ
    Private ReadOnly T_MotoGunID As String() = {"元点群ＩＤ", "MotoGunID"}               '3  元点群ＩＤ
    Private ReadOnly T_SyoriHouhouID As String() = {"処理方法ＩＤ", "SyoriHouhouID"}      '4  処理方法ＩＤ
    Private ReadOnly T_SortingHouhou As String() = {"ソーティング順", "SortingHouhou"}     '5  ソーティング順

    Private ReadOnly T_objXYZ As String() = {"ＸＹＺ成分", "objXYZ"}                         '6  ＸＹＺ成分
    Private ReadOnly T_BasePointID As String() = {"基準点ＩＤ", "BasePointID"}               '7  基準点ＩＤ
    Private ReadOnly T_objDaisyo As String() = {"大小指定", "objDaisyo"}                    '8  大小指定

    Private ReadOnly T_Index1 As String() = {"範囲１", "Index1"}                           '9  範囲１

    Private ReadOnly T_Index2 As String() = {"範囲２", "Index2"}                           '10 範囲２

    Private ReadOnly T_Bunrui1 As String() = {"分類１", "Bunrui1"}                         '11 分類１

    Private ReadOnly T_Bunrui2 As String() = {"分類２", "Bunrui2"}                         '12 分類２

    Private ReadOnly T_flgBunrui As String() = {"分類使用方法", "flgBunrui"}                '13 分類使用方法

    Private ReadOnly T_flgOnlyOne As String() = {"flgOnlyOne"}                              '14 flgOnlyOne
    Private ReadOnly T_TenGunName As String() = {"TenGunName"}                                 '15 TenGunName
    Private ReadOnly T_Bikou As String() = {"備考", "Bikou"}                           '16 備考


    'ID
    Private _ID As Integer = 0
    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Private Sub p_set_ID(ByRef IDR As IDataReader)
        _ID = CInt(GetDataByIndexOfDataReader(FIELD_INDEX(0), IDR))
    End Sub

    'TypeID
    Private _TypeID As Integer = -1
    Public Property TypeID() As Integer
        Get
            Return _TypeID
        End Get
        Set(ByVal value As Integer)
            _TypeID = value
        End Set
    End Property

    Private Sub p_set_TypeID(ByRef IDR As IDataReader)
        If IsNumeric(GetDataByIndexOfDataReader(FIELD_INDEX(1), IDR)) = True Then
            _TypeID = GetDataByIndexOfDataReader(FIELD_INDEX(1), IDR)
        End If
    End Sub

    '点群ＩＤ
    Private _TenGunID As Integer = -1
    Public Property TenGunID() As Integer
        Get
            Return _TenGunID
        End Get
        Set(ByVal value As Integer)
            _TenGunID = value
        End Set
    End Property

    Private Sub p_set_TenGunID(ByRef IDR As IDataReader)
        If IsNumeric(GetDataByIndexOfDataReader(FIELD_INDEX(2), IDR)) = True Then
            _TenGunID = CInt(GetDataByIndexOfDataReader(FIELD_INDEX(2), IDR))
        End If
    End Sub

    Public Tengun() As CT_Data

    '元点群ＩＤ
    Private _MotoGunID As Integer = -1
    Public Property MotoGunID() As Integer
        Get
            Return _MotoGunID
        End Get
        Set(ByVal value As Integer)
            _MotoGunID = value
        End Set
    End Property

    Private Sub p_set_MotoGunID(ByRef IDR As IDataReader)
        If IsNumeric(GetDataByIndexOfDataReader(FIELD_INDEX(3), IDR)) = True Then
            _MotoGunID = CInt(GetDataByIndexOfDataReader(FIELD_INDEX(3), IDR))
        End If
    End Sub
    Private _MotoGun() As CT_Data

    Public ReadOnly Property MotoGun() As CT_Data()
        Get
            If MotoGunID = 0 Then
                Return arrCTData
            Else
                Return lstTengun.Item(MotoGunID - 1).Tengun
            End If
        End Get
    End Property

    '処理方法ＩＤ
    Private _SyoriHouhouID As TengunSyoriHouhou = -1
    Public Property SyoriHouhouID() As TengunSyoriHouhou
        Get
            Return _SyoriHouhouID
        End Get
        Set(ByVal value As TengunSyoriHouhou)
            _SyoriHouhouID = value
        End Set
    End Property

    Private Sub p_set_SyoriHouhouID(ByRef IDR As IDataReader)
        If IsNumeric(GetDataByIndexOfDataReader(FIELD_INDEX(4), IDR)) = True Then
            _SyoriHouhouID = GetDataByIndexOfDataReader(FIELD_INDEX(4), IDR)
        End If
    End Sub

    'ソーティング順

    Private _SortingHouhou As SortMethod = -1
    Public Property SortingHouhou() As SortMethod
        Get
            Return _SortingHouhou
        End Get
        Set(ByVal value As SortMethod)
            _SortingHouhou = value
        End Set
    End Property

    Private Sub p_set_SortingHouhou(ByRef IDR As IDataReader)
        If IsNumeric(GetDataByIndexOfDataReader(FIELD_INDEX(5), IDR)) = True Then
            _SortingHouhou = GetDataByIndexOfDataReader(FIELD_INDEX(5), IDR)
        End If
    End Sub

    'ＸＹＺ成分
    Private _objXYZ As XYZseibun = -1
    Public Property objXYZ() As XYZseibun
        Get
            Return _objXYZ
        End Get
        Set(ByVal value As XYZseibun)
            _objXYZ = value
        End Set
    End Property

    Private Sub p_set_objXYZ(ByRef IDR As IDataReader)
        If IsNumeric(GetDataByIndexOfDataReader(FIELD_INDEX(6), IDR)) = True Then
            _objXYZ = GetDataByIndexOfDataReader(FIELD_INDEX(6), IDR)
        End If
    End Sub

    '基準点ＩＤ
    Private _BasePointID As Integer = -1
    Public Property BasePointID() As Integer
        Get
            Return _BasePointID
        End Get
        Set(ByVal value As Integer)
            _BasePointID = value
        End Set
    End Property

    Private Sub p_set_BasePointID(ByRef IDR As IDataReader)
        If IsNumeric(GetDataByIndexOfDataReader(FIELD_INDEX(7), IDR)) = True Then
            _BasePointID = GetDataByIndexOfDataReader(FIELD_INDEX(7), IDR)
        End If
    End Sub

    '大小指定

    Private _objDaisyo As DaiSyou = -1
    Public Property objDaisyo() As DaiSyou
        Get
            Return _objDaisyo
        End Get
        Set(ByVal value As DaiSyou)
            _objDaisyo = value
        End Set
    End Property

    Private Sub p_set_objDaisyo(ByRef IDR As IDataReader)
        If IsNumeric(GetDataByIndexOfDataReader(FIELD_INDEX(8), IDR)) = True Then
            _objDaisyo = GetDataByIndexOfDataReader(FIELD_INDEX(8), IDR)
        End If
    End Sub

    '範囲１

    Private _Index1 As Integer = -1
    Public Property Index1() As Integer
        Get
            Return _Index1
        End Get
        Set(ByVal value As Integer)
            _Index1 = value
        End Set
    End Property

    Private Sub p_set_Index1(ByRef IDR As IDataReader)
        If IsNumeric(GetDataByIndexOfDataReader(FIELD_INDEX(9), IDR)) = True Then
            _Index1 = CInt(GetDataByIndexOfDataReader(FIELD_INDEX(9), IDR))
        End If
    End Sub

    '範囲２

    Private _Index2 As Integer = -1
    Public Property Index2() As Integer
        Get
            Return _Index2
        End Get
        Set(ByVal value As Integer)
            _Index2 = value
        End Set
    End Property

    Private Sub p_set_Index2(ByRef IDR As IDataReader)
        If IsNumeric(GetDataByIndexOfDataReader(FIELD_INDEX(10), IDR)) = True Then
            _Index2 = CInt(GetDataByIndexOfDataReader(FIELD_INDEX(10), IDR))
        End If
    End Sub

    '分類１

    Private _Bunrui1 As Integer = -1
    Public Property Bunrui1() As Integer
        Get
            Return _Bunrui1
        End Get
        Set(ByVal value As Integer)
            _Bunrui1 = value
        End Set
    End Property

    Private Sub p_set_Bunrui1(ByRef IDR As IDataReader)
        If IsNumeric(GetDataByIndexOfDataReader(FIELD_INDEX(11), IDR)) = True Then
            _Bunrui1 = CInt(GetDataByIndexOfDataReader(FIELD_INDEX(11), IDR))
        End If
    End Sub

    '分類２

    Private _Bunrui2 As Integer = -1
    Public Property Bunrui2() As Integer
        Get
            Return _Bunrui2
        End Get
        Set(ByVal value As Integer)
            _Bunrui2 = value
        End Set
    End Property

    Private Sub p_set_Bunrui2(ByRef IDR As IDataReader)
        If IsNumeric(GetDataByIndexOfDataReader(FIELD_INDEX(12), IDR)) = True Then
            _Bunrui2 = CInt(GetDataByIndexOfDataReader(FIELD_INDEX(12), IDR))
        End If
    End Sub

    '分類使用方法

    Private _flgBunrui As TengunBunruiShiyouHouhou = -1
    Public Property flgBunrui() As TengunBunruiShiyouHouhou
        Get
            Return _flgBunrui
        End Get
        Set(ByVal value As TengunBunruiShiyouHouhou)
            _flgBunrui = value
        End Set
    End Property

    Private Sub p_set_flgBunrui(ByRef IDR As IDataReader)
        If IsNumeric(GetDataByIndexOfDataReader(FIELD_INDEX(13), IDR)) = True Then
            _flgBunrui = GetDataByIndexOfDataReader(FIELD_INDEX(13), IDR)
        End If
    End Sub

    'flgOnlyOne
    Private _flgOnlyOne As Boolean = False
    Public Property flgOnlyOne() As Boolean
        Get
            Return _flgOnlyOne
        End Get
        Set(ByVal value As Boolean)
            _flgOnlyOne = value
        End Set
    End Property

    Private Sub p_set_flgOnlyOne(ByRef IDR As IDataReader)
        If GetDataByIndexOfDataReader(FIELD_INDEX(14), IDR) = "TRUE" Then
            _flgOnlyOne = True
        Else
            _flgOnlyOne = False
        End If
    End Sub

    'TenGunName
    Private _strName As String = ""
    Public Property strName() As String
        Get
            Return _strName
        End Get
        Set(ByVal value As String)
            _strName = value
        End Set
    End Property

    Private Sub p_set_TenGunName(ByRef IDR As IDataReader)
        _strName = GetDataByIndexOfDataReader(FIELD_INDEX(15), IDR)
    End Sub

    '備考

    Private _strBikou As String = ""
    Public Property strBikou() As String
        Get
            Return _strBikou
        End Get
        Set(ByVal value As String)
            _strBikou = value
        End Set
    End Property

    Private Sub p_set_Bikou(ByRef IDR As IDataReader)
        _strBikou = GetDataByIndexOfDataReader(FIELD_INDEX(16), IDR)
    End Sub

    Public flgSyusei As Boolean = False
    Public flgNewInput As Boolean = False
    Private _backup As TenGunTeigiTable
    Public Const MaxFieldCnt = 16
    Public strFieldNames() As String
    Public strFieldTexts() As String

    Public m_dbClass As FBMlib.CDBOperateOLE

    Public Sub New()
        TypeID = -1
        TenGunID = -1
        MotoGunID = -1
        SyoriHouhouID = -1
        SortingHouhou = -1
        objXYZ = -1
        BasePointID = -1
        objDaisyo = -1
        Index1 = -1
        Index2 = -1
        Bunrui1 = -1
        Bunrui2 = -1
        flgBunrui = -1
        flgOnlyOne = "False"
        strName = ""
        strBikou = ""
    End Sub

    Public Sub New(ByVal strFields() As String)
        'TypeID = strFields(0)
        TenGunID = strFields(0)
        MotoGunID = strFields(1)
        SyoriHouhouID = strFields(2)
        SortingHouhou = strFields(3)
        objXYZ = strFields(4)
        BasePointID = strFields(5)
        objDaisyo = strFields(6)
        Index1 = strFields(7)
        Index2 = strFields(8)
        Bunrui1 = strFields(9)
        Bunrui2 = strFields(10)
        flgBunrui = strFields(11)
        flgOnlyOne = strFields(12)
        strName = strFields(13)
        strBikou = strFields(14)
    End Sub


    Public Sub BackUp()
        _backup = New TenGunTeigiTable
        With _backup
            ._ID = Me._ID                               '0  ID
            ._TypeID = Me._TypeID                       '1  TypeID
            ._TenGunID = Me._TenGunID                   '2  点群ＩＤ
            ._MotoGunID = Me._MotoGunID                 '3  元点群ＩＤ
            ._SyoriHouhouID = Me._SyoriHouhouID         '4  処理方法ＩＤ
            ._SortingHouhou = Me._SortingHouhou         '5  ソーティング順

            ._objXYZ = Me._objXYZ                       '6  ＸＹＺ成分
            ._BasePointID = Me._BasePointID             '7  基準点ＩＤ
            ._objDaisyo = Me._objDaisyo                 '8  大小指定

            ._Index1 = Me._Index1                       '9  範囲１

            ._Index2 = Me._Index2                       '10 範囲２

            ._Bunrui1 = Me._Bunrui1                     '11 分類１

            ._Bunrui2 = Me._Bunrui2                     '12 分類２

            ._flgBunrui = Me._flgBunrui                 '13 分類使用方法

            ._flgOnlyOne = Me._flgOnlyOne               '14 flgOnlyOne
            ._strName = Me._strName                     '15 TenGunName
            ._strBikou = Me._strBikou                   '16 備考

        End With
    End Sub

    Public Sub copy(ByVal TGT As TenGunTeigiTable)

        With TGT
            _ID = ._ID                                  '0  ID
            _TypeID = ._TypeID                          '1  TypeID
            _TenGunID = ._TenGunID                      '2  点群ＩＤ
            _MotoGunID = ._MotoGunID                    '3  元点群ＩＤ
            _SyoriHouhouID = ._SyoriHouhouID            '4  処理方法ＩＤ
            _SortingHouhou = ._SortingHouhou            '5  ソーティング順

            _objXYZ = ._objXYZ                          '6  ＸＹＺ成分
            _BasePointID = ._BasePointID                '7  基準点ＩＤ
            _objDaisyo = ._objDaisyo                    '8  大小指定

            _Index1 = ._Index1                          '9  範囲１

            _Index2 = ._Index2                          '10 範囲２

            _Bunrui1 = ._Bunrui1                        '11 分類１

            _Bunrui2 = ._Bunrui2                        '12 分類２

            _flgBunrui = ._flgBunrui                    '13 分類使用方法

            _flgOnlyOne = ._flgOnlyOne                  '14 flgOnlyOne
            _strName = ._strName                        '15 TenGunName
            _strBikou = ._strBikou                      '16 備考

        End With

        BackUp()

    End Sub

    Public Sub SetFieldIndex(ByRef IDR As IDataReader)
        Dim iList As New List(Of Integer)
        '順番を間違いないように！！！！

        iList.Add(GetIndexOfDataReader(T_ID, IDR))                  '0  ID
        iList.Add(GetIndexOfDataReader(T_Type_ID, IDR))             '1  TypeID
        iList.Add(GetIndexOfDataReader(T_TenGunID, IDR))            '2  点群ＩＤ
        iList.Add(GetIndexOfDataReader(T_MotoGunID, IDR))           '3  元点群ＩＤ
        iList.Add(GetIndexOfDataReader(T_SyoriHouhouID, IDR))       '4  処理方法ＩＤ
        iList.Add(GetIndexOfDataReader(T_SortingHouhou, IDR))       '5  ソーティング順

        iList.Add(GetIndexOfDataReader(T_objXYZ, IDR))              '6  ＸＹＺ成分
        iList.Add(GetIndexOfDataReader(T_BasePointID, IDR))         '7  基準点ＩＤ
        iList.Add(GetIndexOfDataReader(T_objDaisyo, IDR))           '8  大小指定

        iList.Add(GetIndexOfDataReader(T_Index1, IDR))              '9  範囲１

        iList.Add(GetIndexOfDataReader(T_Index2, IDR))              '10 範囲２

        iList.Add(GetIndexOfDataReader(T_Bunrui1, IDR))             '11 分類１

        iList.Add(GetIndexOfDataReader(T_Bunrui2, IDR))             '12 分類２

        iList.Add(GetIndexOfDataReader(T_flgBunrui, IDR))           '13 分類使用方法

        iList.Add(GetIndexOfDataReader(T_flgOnlyOne, IDR))          '14 flgOnlyOne
        iList.Add(GetIndexOfDataReader(T_TenGunName, IDR))          '15 TenGunName
        iList.Add(GetIndexOfDataReader(T_Bikou, IDR))               '16 備考


        FIELD_INDEX = iList

    End Sub

    Public Shared FIELD_INDEX As New List(Of Integer)

    Public Function GetDataByReader(ByRef iDataReader As IDataReader)
        GetDataByReader = False

        p_set_ID(iDataReader)                                   '0  ID
        p_set_TypeID(iDataReader)                               '1  TypeID
        p_set_TenGunID(iDataReader)                             '2  点群ＩＤ
        p_set_MotoGunID(iDataReader)                            '3  元点群ＩＤ
        p_set_SyoriHouhouID(iDataReader)                        '4  処理方法ＩＤ
        p_set_SortingHouhou(iDataReader)                        '5  ソーティング順

        p_set_objXYZ(iDataReader)                               '6  ＸＹＺ成分
        p_set_BasePointID(iDataReader)                          '7  基準点ＩＤ
        p_set_objDaisyo(iDataReader)                            '8  大小指定

        p_set_Index1(iDataReader)                               '9  範囲１

        p_set_Index2(iDataReader)                               '10 範囲２

        p_set_Bunrui1(iDataReader)                              '11 分類１

        p_set_Bunrui2(iDataReader)                              '12 分類２

        p_set_flgBunrui(iDataReader)                            '13 分類使用方法

        p_set_flgOnlyOne(iDataReader)                           '14 flgOnlyOne
        p_set_TenGunName(iDataReader)                           '15 TenGunName
        p_set_Bikou(iDataReader)                                '16 備考


        BackUp() 'バックアップ用

        GetDataByReader = True

    End Function

    Private Sub CreateFieldName()
        ReDim strFieldNames(MaxFieldCnt - 1)

        strFieldNames(0) = "タイプＩＤ"                 '1  TypeID
        strFieldNames(1) = "点群ID"               '2  点群ＩＤ
        strFieldNames(2) = "元点群ＩＤ"               '3  元点群ＩＤ
        strFieldNames(3) = "処理方法ＩＤ"             '4  処理方法ＩＤ
        strFieldNames(4) = "ソーティング順"            '5  ソーティング順

        strFieldNames(5) = "ＸＹＺ成分"              '6  ＸＹＺ成分
        strFieldNames(6) = "基準点ＩＤ"              '7  基準点ＩＤ
        strFieldNames(7) = "大小指定"               '8  大小指定

        strFieldNames(8) = "範囲１"                '9  範囲１

        strFieldNames(9) = "範囲２"                '10 範囲２

        strFieldNames(10) = "分類１"               '11 分類１

        strFieldNames(11) = "分類２"               '12 分類２

        strFieldNames(12) = "分類使用方法"            '13 分類使用方法

        strFieldNames(13) = "flgOnlyOne"                '14 flgOnlyOne
        strFieldNames(14) = "TenGunName"                '15 TenGunName
        strFieldNames(15) = "備考"                    '16 備考


    End Sub

    Public Sub CreateFieldText()
        ReDim strFieldTexts(MaxFieldCnt - 1)

        CreateFieldName()

        strFieldTexts(0) = _TypeID                     '1  TypeID
        strFieldTexts(1) = _TenGunID                    '2  点群ＩＤ
        strFieldTexts(2) = _MotoGunID                   '3  元点群ＩＤ
        strFieldTexts(3) = _SyoriHouhouID               '4  処理方法ＩＤ
        strFieldTexts(4) = _SortingHouhou               '5  ソーティング順

        strFieldTexts(5) = _objXYZ                      '6  ＸＹＺ成分
        strFieldTexts(6) = _BasePointID                 '7  基準点ＩＤ
        strFieldTexts(7) = _objDaisyo                   '8  大小指定

        strFieldTexts(8) = _Index1                      '9  範囲１

        strFieldTexts(9) = _Index2                      '10 範囲２

        strFieldTexts(10) = _Bunrui1                    '11 分類１

        strFieldTexts(11) = _Bunrui2                    '12 分類２

        strFieldTexts(12) = _flgBunrui                  '13 分類使用方法

        strFieldTexts(13) = "'" & IIf(_flgOnlyOne = True, "TRUE", "FALSE") & "'"     '14 flgOnlyOne
        strFieldTexts(14) = "'" & _strName & "'"        '15 TenGunName
        strFieldTexts(15) = "'" & _strBikou & "'"       '16 備考


    End Sub

#Region "データ取得"

    Public Function GetDataToList() As List(Of TenGunTeigiTable)

        Dim strSql As String = "SELECT * FROM " & T_tengunteigi & " WHERE タイプＩＤ = " & CommonTypeID & " ORDER BY ID"
        Dim IDR As IDataReader
        Dim bRet As Boolean = False
        Dim TenGunTeigiL As List(Of TenGunTeigiTable) = Nothing

        IDR = m_dbClass.DoSelect(strSql)
        If Not IDR Is Nothing Then
            TenGunTeigiL = New List(Of TenGunTeigiTable)
            Dim IDX As New TenGunTeigiTable
            IDX.SetFieldIndex(IDR)
            Do While IDR.Read
                Dim TGD As New TenGunTeigiTable
                TGD.GetDataByReader(IDR)
                TenGunTeigiL.Add(TGD)
            Loop
            IDR.Close()
        End If

        GetDataToList = TenGunTeigiL

    End Function

#End Region

#Region "Save"

    Public Function ExistsData() As Boolean

        Dim bRet As Boolean = False
        Dim lRet As Long = 0
        Dim strWhere As String = ""

        If IsNumeric(_ID) = False Then
            ExistsData = False
            Exit Function
        End If

        strWhere = "点群ID = " & _TenGunID

        lRet = m_dbClass.DoDCount("*", T_tengunteigi, strWhere)
        If lRet > 0 Then
            bRet = True
        End If

        ExistsData = bRet

    End Function

    Public Function SaveData(Optional ByRef flg_trans As Boolean = True) As Boolean

        Dim bRet As Boolean = True

        If ExistsData() = True Then
            'TenGunTeigiが登録されている場合、Update
            bRet = UpdateData(flg_trans)
        Else
            'TenGunTeigiが登録されている場合、Insert
            bRet = InsertData(flg_trans)
        End If

        SaveData = bRet

    End Function

#End Region

#Region "Insert"

    Public Function InsertData(Optional ByRef flg_trans As Boolean = True) As Boolean

        InsertData = True

        CreateFieldText()

        Dim lRet As Long = 0

        lRet = m_dbClass.DoInsert(strFieldNames, T_tengunteigi, strFieldTexts)
        If lRet = 1 Then
        Else
            InsertData = False
        End If

    End Function


#End Region

#Region "Update"

    Public Function UpdateData(Optional ByRef flg_trans As Boolean = True) As Boolean

        UpdateData = True

        Dim strWhere As String = "ID = " & _ID

        CreateFieldText()

        Dim lRet As Long = 0
        If flg_trans = True Then
            m_dbClass.BeginTrans()
            lRet = m_dbClass.DoUpdate(strFieldNames, T_tengunteigi, strFieldTexts, strWhere)
            If lRet = 1 Then
                m_dbClass.CommitTrans()
            Else
                m_dbClass.RollbackTrans()
                UpdateData = False
            End If
        Else
            lRet = m_dbClass.DoUpdate(strFieldNames, T_tengunteigi, strFieldTexts, strWhere)
            If lRet = 1 Then
            Else
                UpdateData = False
            End If
        End If

    End Function

#End Region


    Public Sub CalcTenGun(ByVal blnSyori As Boolean)
        Dim GID As Integer = TenGunID
        Dim Mid As Integer = MotoGunID
        Dim BasePoint As New Point3D

        If GID = 71 Then
            GID = GID
        End If
        Select Case SyoriHouhouID
            Case TengunSyoriHouhou.OnePoint
                ReDim Tengun(0)
                Tengun(0) = MotoGun(BasePointID)
            Case TengunSyoriHouhou.Sorting
                If (objXYZ = XYZseibun.XY) Or (objXYZ = XYZseibun.XZ) Or (objXYZ = XYZseibun.YZ) Or (objXYZ = XYZseibun.XYZ) Then
                    BasePoint = GetGunTeigiTableDataByGunID(BasePointID).GetFromTengun
                End If
                GetSorted(MotoGun, objXYZ, SortingHouhou, BasePoint, Tengun)
            Case TengunSyoriHouhou.BaseTenYoriDaisyo
                'GetAreaPointGroup(MotoGun, objXYZ, GetGunTeigiTableDataByGunID(BasePointID).Tengun(0).CT_dat.PID, objDaisyo, Tengun)
                GetAreaPointGroup(MotoGun, objXYZ, GetGunTeigiTableDataByGunID(BasePointID).GetFromTengun, objDaisyo, Tengun)
            Case TengunSyoriHouhou.HaninSitei
                GetPoint_ByIndex(MotoGun, Index1, Index2, Tengun)
            Case TengunSyoriHouhou.Bunrui
                GetPoint_ByKubun(MotoGun, Bunrui1, Bunrui2, Tengun)
            Case TengunSyoriHouhou.OffsetVector
                GetPoint_ByOffset(MotoGun, objXYZ, SortingHouhou, Tengun)
            Case TengunSyoriHouhou.SonoMama
                Tengun = MotoGun.Clone
            Case TengunSyoriHouhou.SortingByCTID
                GetSortedByCTID(MotoGun, SortingHouhou, Tengun)
            Case TengunSyoriHouhou.SingleToCT
                Tengun = arrSTtoCTData
            Case TengunSyoriHouhou.NinniST
                For Each objCTD As CT_Data In arrSTtoCTData
                    If BasePointID = objCTD.CT_dat.PID Then
                        ReDim Tengun(0)
                        Tengun(0) = objCTD
                        Exit For

                    End If
                Next
            Case TengunSyoriHouhou.NinniCT

        End Select
        If blnSyori = True Then
            If Tengun Is Nothing Then

                If MsgBox(strName & "が見つかりませんでした。", MsgBoxStyle.OkOnly, "VFORM") = MsgBoxResult.Ok Then

                End If
            End If
        End If
    End Sub

    Public Function GetFromTengun() As Point3D
        GetFromTengun = Nothing

        CalcTenGun(False)
        Try
            If Tengun Is Nothing Then
                'H25.6.28 Yamadaコメントアウト

                'MsgBox(_strName & "を抽出失敗しました。")
                Exit Function
            Else
                If Tengun(0) Is Nothing Then
                    Exit Function
                End If
            End If
        Catch ex As Exception
            Exit Function
        End Try
     
        Select Case flgBunrui
            Case TengunBunruiShiyouHouhou.OnePoint
                If flgOnlyOne = True Then
                    If Tengun.Length <> 1 Then
                        'H25.6.28 Yamadaコメントアウト

                        'MsgBox(_strName & "を抽出失敗しました。")
                    Else
                        ' MsgBox(_strName & "を抽出失敗しました。")
                        GetFromTengun = Tengun(0).CT_dat.lstRealP3d(0)
                    End If
                End If

            Case TengunBunruiShiyouHouhou.MultiPoint
                If flgOnlyOne = True Then
                    Select Case Tengun.Length
                        Case 0
                            'H25.6.28 Yamadaコメントアウト

                            'MsgBox(_strName & "を抽出失敗しました。")
                        Case 1
                            GetFromTengun = Tengun(0).CT_dat.lstRealP3d(0)
                        Case 2
                            GetFromTengun = YCM_Sunpo.GetPoint(Tengun(0).CT_dat.lstRealP3d(0), XYZseibun.X,
                                                           Tengun(1).CT_dat.lstRealP3d(0), XYZseibun.Y, New Point3D(0, 0, 0), XYZseibun.Z)
                        Case Is > 2
                            'H25.6.28 Yamadaコメントアウト

                            'MsgBox(_strName & "を抽出したが、複数点が見つかりました。")
                    End Select
                End If
            Case TengunBunruiShiyouHouhou.CircleCenter     '元点から演算計算しているため、メモリ上の同期はとれない。注意！！！

                If Tengun.Count = 3 Then
                    Dim Circ As New GeoCurve
                    YCM_Sunpo.GetCircle3P(Tengun(0).CT_dat.lstRealP3d(0),
                                          Tengun(1).CT_dat.lstRealP3d(0),
                                          Tengun(2).CT_dat.lstRealP3d(0), Circ)
                    GetFromTengun = Circ.center.GetP3D
                End If
            Case TengunBunruiShiyouHouhou.MidPoint
                If Tengun.Count = 2 Then
                    GetFromTengun = New Point3D
                    GetFromTengun.GetMidTen(Tengun(0).CT_dat.lstRealP3d(0), Tengun(1).CT_dat.lstRealP3d(0))
                Else
                    'H25.6.28 Yamadaコメントアウト

                    'MsgBox(strName & "を抽出失敗しました。")
                End If
            Case TengunBunruiShiyouHouhou.MeanPoint
                If Tengun.Count > 0 Then
                    Dim tmp As New Point3D
                    For Each TP As CT_Data In Tengun
                        tmp.ConcatToMe(TP.CT_dat.lstRealP3d(0))
                    Next
                    GetFromTengun = tmp.GetMean()
                End If
            Case TengunBunruiShiyouHouhou.LineToPointNoFoot
                If Tengun.Length = 3 Then
                    GetFromTengun = GetLineToPointNoFootP(Tengun(0).CT_dat.lstRealP3d(0), Tengun(1).CT_dat.lstRealP3d(0), Tengun(2).CT_dat.lstRealP3d(0))
                End If
            Case TengunBunruiShiyouHouhou.LineToPointNoPlane
                If Tengun.Length = 2 Then
                    GetPoint_ByPlane(Tengun(0).CT_dat.lstRealP3d(0), Tengun(1).CT_dat.lstRealP3d(0),
                                      GetGunTeigiTableDataByGunID(BasePointID).Tengun(0).CT_dat.lstRealP3d(0), objXYZ, GetFromTengun)
                End If
            Case TengunBunruiShiyouHouhou.NormalVectorByThreePoint  'SUURI ADD 20141022
                If Tengun.Length = 3 Then
                    Dim Circ As New GeoCurve
                    YCM_Sunpo.GetCircle3P(Tengun(0).CT_dat.lstRealP3d(0),
                                          Tengun(1).CT_dat.lstRealP3d(0),
                                          Tengun(2).CT_dat.lstRealP3d(0), Circ)
                    GetFromTengun = New Point3D(Circ.normal.x, Circ.normal.y, Circ.normal.z)

                End If
        End Select

    End Function

    Public Function GetFromTengunNoUnit() As Point3D
        GetFromTengunNoUnit = Nothing

        CalcTenGun(False)
        If Tengun Is Nothing Then
            Exit Function
        End If
        Select Case flgBunrui
            Case TengunBunruiShiyouHouhou.OnePoint
                If flgOnlyOne = True Then
                    If Tengun.Length = 1 Then
                        GetFromTengunNoUnit = New Point3D

                        GetFromTengunNoUnit.CopyToMe(Tengun(0).CT_dat.lstP3d(0))

                    End If
                End If

            Case TengunBunruiShiyouHouhou.MultiPoint
                If flgOnlyOne = True Then
                    Select Case Tengun.Length
                        Case 0
                            'H25.6.28 Yamadaコメントアウト

                            'MsgBox(_strName & "を抽出失敗しました。")
                        Case 1
                            GetFromTengunNoUnit.CopyToMe(Tengun(0).CT_dat.lstP3d(0))
                        Case 2
                            GetFromTengunNoUnit = YCM_Sunpo.GetPoint(Tengun(0).CT_dat.lstP3d(0), XYZseibun.X,
                                                           Tengun(1).CT_dat.lstP3d(0), XYZseibun.Y, New Point3D(0, 0, 0), XYZseibun.Z)
                        Case Is > 2
                            'H25.6.28 Yamadaコメントアウト

                            'MsgBox(_strName & "を抽出したが、複数点が見つかりました。")
                    End Select
                End If
            Case TengunBunruiShiyouHouhou.CircleCenter
                If Tengun.Count = 3 Then
                    Dim Circ As New GeoCurve
                    YCM_Sunpo.GetCircle3P(Tengun(0).CT_dat.lstP3d(0),
                                          Tengun(1).CT_dat.lstP3d(0),
                                          Tengun(2).CT_dat.lstP3d(0), Circ)
                    GetFromTengunNoUnit = Circ.center.GetP3D
                End If
            Case TengunBunruiShiyouHouhou.MidPoint
                If Tengun.Count = 2 Then
                    GetFromTengunNoUnit = New Point3D
                    GetFromTengunNoUnit.GetMidTen(Tengun(0).CT_dat.lstP3d(0), Tengun(1).CT_dat.lstP3d(0))
                Else
                    'H25.6.28 Yamadaコメントアウト

                    'MsgBox(strName & "を抽出失敗しました。")
                End If
            Case TengunBunruiShiyouHouhou.MeanPoint
                If Tengun.Count > 0 Then
                    Dim tmp As New Point3D
                    For Each TP As CT_Data In Tengun
                        tmp.ConcatToMe(TP.CT_dat.lstP3d(0))
                    Next
                    GetFromTengunNoUnit = tmp.GetMean()
                End If
            Case TengunBunruiShiyouHouhou.LineToPointNoFoot
                If Tengun.Length = 3 Then
                     GetFromTengunNoUnit = GetLineToPointNoFootP(Tengun(0).CT_dat.lstP3d(0), Tengun(1).CT_dat.lstP3d(0), Tengun(2).CT_dat.lstP3d(0))
                End If
            Case TengunBunruiShiyouHouhou.LineToPointNoPlane
                If Tengun.Length = 2 Then
                    GetPoint_ByPlane(Tengun(0).CT_dat.lstRealP3d(0), Tengun(1).CT_dat.lstP3d(0),
                                      GetGunTeigiTableDataByGunID(BasePointID).Tengun(0).CT_dat.lstP3d(0), objXYZ, GetFromTengunNoUnit)
                End If
        End Select

    End Function


    ' 20161109 baluu ADD start

    Public Function GetFromTengunBySek() As Point3D
        GetFromTengunBySek = Nothing

        CalcTenGun(False)
        Try
            If Tengun Is Nothing Then
                'H25.6.28 Yamadaコメントアウト

                'MsgBox(_strName & "を抽出失敗しました。")
                Exit Function
            Else
                If Tengun(0) Is Nothing Then
                    Exit Function
                End If
            End If
        Catch ex As Exception
            Exit Function
        End Try

        Select Case flgBunrui
            Case TengunBunruiShiyouHouhou.OnePoint
                If flgOnlyOne = True Then
                    If Tengun.Length <> 1 Then
                        'H25.6.28 Yamadaコメントアウト

                        'MsgBox(_strName & "を抽出失敗しました。")
                    Else
                        ' MsgBox(_strName & "を抽出失敗しました。")
                        If Not Tengun(0).SekDat Is Nothing Then
                            If Not Tengun(0).SekDat.lstRealP3d Is Nothing Then
                                If Tengun(0).SekDat.lstRealP3d.Count > 0 Then
                                    GetFromTengunBySek = Tengun(0).SekDat.lstRealP3d(0)
                                End If
                            End If
                        End If
                    End If
                End If

            Case TengunBunruiShiyouHouhou.MultiPoint
                        If flgOnlyOne = True Then
                            Select Case Tengun.Length
                                Case 0
                                    'H25.6.28 Yamadaコメントアウト

                                    'MsgBox(_strName & "を抽出失敗しました。")
                                Case 1
                            If Not Tengun(0).SekDat Is Nothing Then
                                If Not Tengun(0).SekDat.lstRealP3d Is Nothing Then
                                    If Tengun(0).SekDat.lstRealP3d.Count > 0 Then
                                        GetFromTengunBySek = Tengun(0).SekDat.lstRealP3d(0)
                                    End If
                                End If
                            End If
                        Case 2
                            If Not Tengun(0).SekDat Is Nothing And Tengun(1).SekDat Is Nothing Then
                                If Not Tengun(0).SekDat.lstRealP3d Is Nothing And Tengun(1).SekDat.lstRealP3d Is Nothing Then
                                    If Tengun(0).SekDat.lstRealP3d.Count > 0 And Tengun(1).SekDat.lstRealP3d.Count > 0 Then
                                        GetFromTengunBySek = YCM_Sunpo.GetPoint(Tengun(0).SekDat.lstRealP3d(0), XYZseibun.X,
                                                            Tengun(1).SekDat.lstRealP3d(0), XYZseibun.Y, New Point3D(0, 0, 0), XYZseibun.Z)
                                    End If
                                End If
                            End If
                            
                                Case Is > 2
                                    'H25.6.28 Yamadaコメントアウト

                                    'MsgBox(_strName & "を抽出したが、複数点が見つかりました。")
                            End Select
                        End If
            Case TengunBunruiShiyouHouhou.CircleCenter     '元点から演算計算しているため、メモリ上の同期はとれない。注意！！！

                        If Tengun.Count = 3 Then
                    If Not Tengun(0).SekDat Is Nothing And Tengun(1).SekDat Is Nothing And Tengun(2).SekDat Is Nothing Then
                        If Not Tengun(0).SekDat.lstRealP3d Is Nothing And Tengun(1).SekDat.lstRealP3d Is Nothing And Tengun(2).SekDat.lstRealP3d Is Nothing Then
                            If Tengun(0).SekDat.lstRealP3d.Count > 0 And Tengun(1).SekDat.lstRealP3d.Count > 0 And Tengun(2).SekDat.lstRealP3d.Count > 0 Then
                                Dim Circ As New GeoCurve
                                YCM_Sunpo.GetCircle3P(Tengun(0).SekDat.lstRealP3d(0),
                                                      Tengun(1).SekDat.lstRealP3d(0),
                                                      Tengun(2).SekDat.lstRealP3d(0), Circ)
                                GetFromTengunBySek = Circ.center.GetP3D
                            End If
                        End If
                    End If
                    
                        End If
            Case TengunBunruiShiyouHouhou.MidPoint
                        If Tengun.Count = 2 Then
                    If Not Tengun(0).SekDat Is Nothing And Tengun(1).SekDat Is Nothing Then
                        If Not Tengun(0).SekDat.lstRealP3d Is Nothing And Tengun(1).SekDat.lstRealP3d Is Nothing Then
                            If Tengun(0).SekDat.lstRealP3d.Count > 0 And Tengun(1).SekDat.lstRealP3d.Count > 0 Then
                                GetFromTengunBySek = New Point3D
                                GetFromTengun.GetMidTen(Tengun(0).SekDat.lstRealP3d(0), Tengun(1).SekDat.lstRealP3d(0))
                            End If
                        End If
                    End If
                    
                        Else
                            'H25.6.28 Yamadaコメントアウト

                            'MsgBox(strName & "を抽出失敗しました。")
                        End If
            Case TengunBunruiShiyouHouhou.MeanPoint
                        If Tengun.Count > 0 Then
                            Dim tmp As New Point3D
                            For Each TP As CT_Data In Tengun
                        If Not TP.SekDat Is Nothing Then
                            If Not TP.SekDat.lstRealP3d Is Nothing Then
                                If TP.SekDat.lstRealP3d.Count > 0 Then
                                    tmp.ConcatToMe(TP.SekDat.lstRealP3d(0))
                                End If
                            End If
                        End If

                            Next
                            GetFromTengunBySek = tmp.GetMean()
                        End If
            Case TengunBunruiShiyouHouhou.LineToPointNoFoot
                        If Tengun.Length = 3 Then
                    If Not Tengun(0).SekDat Is Nothing And Tengun(1).SekDat Is Nothing And Tengun(2).SekDat Is Nothing Then
                        If Not Tengun(0).SekDat.lstRealP3d Is Nothing And Tengun(1).SekDat.lstRealP3d Is Nothing And Tengun(2).SekDat.lstRealP3d Is Nothing Then
                            If Tengun(0).SekDat.lstRealP3d.Count > 0 And Tengun(1).SekDat.lstRealP3d.Count > 0 And Tengun(2).SekDat.lstRealP3d.Count > 0 Then
                                GetFromTengunBySek = GetLineToPointNoFootP(Tengun(0).SekDat.lstRealP3d(0), Tengun(1).SekDat.lstRealP3d(0), Tengun(2).SekDat.lstRealP3d(0))
                            End If
                        End If
                    End If

                        End If
            Case TengunBunruiShiyouHouhou.LineToPointNoPlane
                        If Tengun.Length = 2 Then
                    If Not Tengun(0).SekDat Is Nothing And Tengun(1).SekDat Is Nothing Then
                        If Not Tengun(0).SekDat.lstRealP3d Is Nothing And Tengun(1).SekDat.lstRealP3d Is Nothing Then
                            If Tengun(0).SekDat.lstRealP3d.Count > 0 And Tengun(1).SekDat.lstRealP3d.Count > 0 Then
                                GetPoint_ByPlane(Tengun(0).SekDat.lstRealP3d(0), Tengun(1).SekDat.lstRealP3d(0),
                                              GetGunTeigiTableDataByGunID(BasePointID).Tengun(0).SekDat.lstRealP3d(0), objXYZ, GetFromTengunBySek)
                            End If
                        End If
                    End If
                    
                        End If
            Case TengunBunruiShiyouHouhou.NormalVectorByThreePoint  'SUURI ADD 20141022
                        If Tengun.Length = 3 Then
                    If Not Tengun(0).SekDat Is Nothing And Tengun(1).SekDat Is Nothing And Tengun(2).SekDat Is Nothing Then
                        If Not Tengun(0).SekDat.lstRealP3d Is Nothing And Tengun(1).SekDat.lstRealP3d Is Nothing And Tengun(2).SekDat.lstRealP3d Is Nothing Then
                            If Tengun(0).SekDat.lstRealP3d.Count > 0 And Tengun(1).SekDat.lstRealP3d.Count > 0 And Tengun(2).SekDat.lstRealP3d.Count > 0 Then
                                Dim Circ As New GeoCurve
                                YCM_Sunpo.GetCircle3P(Tengun(0).CT_dat.lstRealP3d(0),
                                                      Tengun(1).CT_dat.lstRealP3d(0),
                                                      Tengun(2).CT_dat.lstRealP3d(0), Circ)
                                GetFromTengunBySek = New Point3D(Circ.normal.x, Circ.normal.y, Circ.normal.z)
                            End If
                        End If
                    End If
                    

                        End If
        End Select

    End Function

    Public Function GetFromTengunNoUnitBySek() As Point3D
        GetFromTengunNoUnitBySek = Nothing

        CalcTenGun(False)
        If Tengun Is Nothing Then
            Exit Function
        End If
        Select Case flgBunrui
            Case TengunBunruiShiyouHouhou.OnePoint
                If flgOnlyOne = True Then
                    If Tengun.Length = 1 Then
                        If Not Tengun(0).SekDat Is Nothing Then
                            If Not Tengun(0).SekDat.lstP3d Is Nothing Then
                                If Tengun(0).SekDat.lstP3d.Count > 0 Then
                                    GetFromTengunNoUnitBySek = New Point3D
                                    GetFromTengunNoUnitBySek.CopyToMe(Tengun(0).SekDat.lstP3d(0))
                                End If
                            End If
                        End If
                    End If
                End If

            Case TengunBunruiShiyouHouhou.MultiPoint
                If flgOnlyOne = True Then
                    Select Case Tengun.Length
                        Case 0
                            'H25.6.28 Yamadaコメントアウト

                            'MsgBox(_strName & "を抽出失敗しました。")
                        Case 1
                            If Not Tengun(0).SekDat Is Nothing Then
                                If Not Tengun(0).SekDat.lstP3d Is Nothing Then
                                    If Tengun(0).SekDat.lstP3d.Count > 0 Then
                                        GetFromTengunNoUnitBySek.CopyToMe(Tengun(0).SekDat.lstP3d(0))
                                    End If
                                End If
                            End If

                        Case 2
                            If Not Tengun(0).SekDat Is Nothing And Tengun(1).SekDat Is Nothing Then
                                If Not Tengun(0).SekDat.lstP3d Is Nothing And Tengun(1).SekDat.lstP3d Is Nothing Then
                                    If Tengun(0).SekDat.lstP3d.Count > 0 And Tengun(1).SekDat.lstP3d.Count > 0 Then
                                        GetFromTengunNoUnitBySek = YCM_Sunpo.GetPoint(Tengun(0).SekDat.lstP3d(0), XYZseibun.X,
                                                           Tengun(1).SekDat.lstP3d(0), XYZseibun.Y, New Point3D(0, 0, 0), XYZseibun.Z)
                                    End If
                                End If
                            End If
                        Case Is > 2
                            'H25.6.28 Yamadaコメントアウト

                            'MsgBox(_strName & "を抽出したが、複数点が見つかりました。")
                    End Select
                End If
            Case TengunBunruiShiyouHouhou.CircleCenter
                If Tengun.Count = 3 Then
                    If Not Tengun(0).SekDat Is Nothing And Tengun(1).SekDat Is Nothing And Tengun(2).SekDat Is Nothing Then
                        If Not Tengun(0).SekDat.lstP3d Is Nothing And Tengun(1).SekDat.lstP3d Is Nothing And Tengun(2).SekDat.lstP3d Is Nothing Then
                            If Tengun(0).SekDat.lstP3d.Count > 0 And Tengun(1).SekDat.lstP3d.Count > 0 And Tengun(2).SekDat.lstP3d.Count > 0 Then
                                Dim Circ As New GeoCurve
                                YCM_Sunpo.GetCircle3P(Tengun(0).SekDat.lstP3d(0),
                                                      Tengun(1).SekDat.lstP3d(0),
                                                      Tengun(2).SekDat.lstP3d(0), Circ)
                                GetFromTengunNoUnitBySek = Circ.center.GetP3D
                            End If
                        End If
                    End If
                    
                End If
            Case TengunBunruiShiyouHouhou.MidPoint
                If Tengun.Count = 2 Then
                    If Not Tengun(0).SekDat Is Nothing And Tengun(1).SekDat Is Nothing Then
                        If Not Tengun(0).SekDat.lstP3d Is Nothing And Tengun(1).SekDat.lstP3d Is Nothing Then
                            If Tengun(0).SekDat.lstP3d.Count > 0 And Tengun(1).SekDat.lstP3d.Count > 0 Then
                                GetFromTengunNoUnitBySek = New Point3D
                                GetFromTengunNoUnitBySek.GetMidTen(Tengun(0).SekDat.lstP3d(0), Tengun(1).SekDat.lstP3d(0))
                            End If
                        End If
                    End If
                Else
                    'H25.6.28 Yamadaコメントアウト

                    'MsgBox(strName & "を抽出失敗しました。")
                End If
            Case TengunBunruiShiyouHouhou.MeanPoint
                If Tengun.Count > 0 Then
                    If Not Tengun(0).SekDat Is Nothing Then
                        If Not Tengun(0).SekDat.lstP3d Is Nothing Then
                            If Tengun(0).SekDat.lstP3d.Count > 0 Then
                                Dim tmp As New Point3D
                                For Each TP As CT_Data In Tengun
                                    tmp.ConcatToMe(TP.SekDat.lstP3d(0))
                                Next
                                GetFromTengunNoUnitBySek = tmp.GetMean()
                            End If
                        End If
                    End If
                End If
            Case TengunBunruiShiyouHouhou.LineToPointNoFoot
                If Tengun.Length = 3 Then
                    If Not Tengun(0).SekDat Is Nothing And Tengun(1).SekDat Is Nothing And Tengun(2).SekDat Is Nothing Then
                        If Not Tengun(0).SekDat.lstP3d Is Nothing And Tengun(1).SekDat.lstP3d Is Nothing And Tengun(2).SekDat.lstP3d Is Nothing Then
                            If Tengun(0).SekDat.lstP3d.Count > 0 And Tengun(1).SekDat.lstP3d.Count > 0 And Tengun(2).SekDat.lstP3d.Count > 0 Then
                                GetFromTengunNoUnitBySek = GetLineToPointNoFootP(Tengun(0).SekDat.lstP3d(0), Tengun(1).SekDat.lstP3d(0), Tengun(2).SekDat.lstP3d(0))
                            End If
                        End If
                    End If
                End If
            Case TengunBunruiShiyouHouhou.LineToPointNoPlane
                If Tengun.Length = 2 Then
                    If Not Tengun(0).SekDat Is Nothing And Tengun(1).SekDat Is Nothing Then
                        If Not Tengun(0).SekDat.lstP3d Is Nothing And Tengun(1).SekDat.lstP3d Is Nothing Then
                            If Tengun(0).SekDat.lstP3d.Count > 0 And Tengun(1).SekDat.lstP3d.Count > 0 Then
                                GetPoint_ByPlane(Tengun(0).SekDat.lstRealP3d(0), Tengun(1).SekDat.lstP3d(0),
                                      GetGunTeigiTableDataByGunID(BasePointID).Tengun(0).SekDat.lstP3d(0), objXYZ, GetFromTengunNoUnit)
                            End If
                        End If
                    End If
                End If
        End Select

    End Function


    ' 20161109 baluu ADD end

    Public Sub GenNinniTengun(ByVal intTypeID As Integer, ByVal intNewTengunID As Integer, ByVal intSTID As Integer)
        TypeID = intTypeID
        TenGunID = intNewTengunID
        MotoGunID = 0
        SyoriHouhouID = TengunSyoriHouhou.NinniST
        SortingHouhou = -1
        objXYZ = -1
        BasePointID = intSTID
        objDaisyo = -1
        Index1 = -1
        Index2 = -1
        Bunrui1 = -1
        Bunrui2 = -1
        flgBunrui = TengunBunruiShiyouHouhou.OnePoint
        flgOnlyOne = "True"
        strName = ""
        strBikou = ""
    End Sub

    Public Sub CsvOut(ByVal strFileName As String, ByRef system_dbclass As FBMlib.CDBOperateOLE)


        Dim strKekka As String = ""

        Dim strSelect As String = T_TenGunID(0)
        strSelect = strSelect & "," & T_MotoGunID(0)
        strSelect = strSelect & "," & T_SyoriHouhouID(0)
        strSelect = strSelect & "," & T_SortingHouhou(0)
        strSelect = strSelect & "," & T_objXYZ(0)
        strSelect = strSelect & "," & T_BasePointID(0)
        strSelect = strSelect & "," & T_objDaisyo(0)
        strSelect = strSelect & "," & T_Index1(0)
        strSelect = strSelect & "," & T_Index2(0)
        strSelect = strSelect & "," & T_Bunrui1(0)
        strSelect = strSelect & "," & T_Bunrui2(0)
        strSelect = strSelect & "," & T_flgBunrui(0)
        strSelect = strSelect & "," & T_flgOnlyOne(0)
        strSelect = strSelect & "," & T_TenGunName(0)
        strSelect = strSelect & "," & T_Bikou(0)
        My.Computer.FileSystem.WriteAllText(strFileName, strSelect + vbNewLine, True)

        Dim strSql As String = "SELECT * FROM " & "TenGunTeigi" & " WHERE タイプＩＤ = " & CommonTypeID & " ORDER BY ID"

        Dim IDR As IDataReader = system_dbclass.DoSelect(strSql)
        If Not IDR Is Nothing Then
            Dim IDX As New TenGunTeigiTable
            IDX.SetFieldIndex(IDR)
            Do While IDR.Read
                'Dim TGD As New TenGunTeigiTable
                Me.GetDataByReader(IDR)
                'TenGunTeigiL.Add(TGD)
                'TGD.CsvOut(ExportDirPath + "\" + "db_out_TengunTeigiTable.csv")
                strKekka = TenGunID _
                 & "," & MotoGunID _
                 & "," & SyoriHouhouID _
                 & "," & SortingHouhou _
                 & "," & objXYZ _
                 & "," & BasePointID _
                 & "," & objDaisyo _
                 & "," & Index1 _
                 & "," & Index2 _
                 & "," & Bunrui1 _
                 & "," & Bunrui2 _
                 & "," & flgBunrui _
                 & "," & flgOnlyOne _
                 & "," & strName _
                 & "," & strBikou _
                & vbNewLine

                My.Computer.FileSystem.WriteAllText(strFileName, strKekka, True)

            Loop
            IDR.Close()
        End If


        'strKekka = TenGunID _
        ' & "," & MotoGunID _
        ' & "," & SyoriHouhouID _
        ' & "," & SortingHouhou _
        ' & "," & objXYZ _
        ' & "," & BasePointID _
        ' & "," & objDaisyo _
        ' & "," & Index1 _
        ' & "," & Index2 _
        ' & "," & Bunrui1 _
        ' & "," & Bunrui2 _
        ' & "," & flgBunrui _
        ' & "," & flgOnlyOne _
        ' & "," & strName _
        ' & "," & strBikou _
        '& vbNewLine

        ''My.Computer.FileSystem.WriteAllText(My.Application.Info.DirectoryPath & "\" & strFileName, strKekka, True)
        'My.Computer.FileSystem.WriteAllText(strFileName, strKekka, True)

    End Sub

    Public Sub CsvInport(ByVal strFileName As String, ByRef dbclass As FBMlib.CDBOperateOLE, ByVal TypeID As Integer)
        'Dim filename As String = My.Application.Info.DirectoryPath & "\" & strFileName
        Dim filename As String = strFileName
        Dim fields As String()
        Dim delimiter As String = ","

        Dim strSelect As String

        'strSelect = T_ID(0)
        strSelect = T_Type_ID(0)
        strSelect = strSelect & "," & T_TenGunID(0)
        strSelect = strSelect & "," & T_MotoGunID(0)
        strSelect = strSelect & "," & T_SyoriHouhouID(0)
        strSelect = strSelect & "," & T_SortingHouhou(0)
        strSelect = strSelect & "," & T_objXYZ(0)
        strSelect = strSelect & "," & T_BasePointID(0)
        strSelect = strSelect & "," & T_objDaisyo(0)
        strSelect = strSelect & "," & T_Index1(0)
        strSelect = strSelect & "," & T_Index2(0)
        strSelect = strSelect & "," & T_Bunrui1(0)
        strSelect = strSelect & "," & T_Bunrui2(0)
        strSelect = strSelect & "," & T_flgBunrui(0)
        strSelect = strSelect & "," & T_flgOnlyOne(0)
        strSelect = strSelect & "," & T_TenGunName(0)
        strSelect = strSelect & "," & T_Bikou(0)

        Using parser As New TextFieldParser(filename, System.Text.Encoding.Default)
            parser.SetDelimiters(delimiter)
            fields = parser.ReadFields() 'ヘッダの読み飛ばし

            While Not parser.EndOfData
                ' Read in the fields for the current line
                fields = parser.ReadFields()
                ' Add code here to use data in fields variable.
                If IsNumeric(fields(0)) Then
                    Dim tmp As New TenGunTeigiTable(fields)
                    Dim strValue As String
                    With tmp
                        strValue = TypeID
                        strValue = strValue & "," & .TenGunID
                        strValue = strValue & "," & .MotoGunID
                        strValue = strValue & "," & .SyoriHouhouID
                        strValue = strValue & "," & .SortingHouhou
                        strValue = strValue & "," & .objXYZ
                        strValue = strValue & "," & .BasePointID
                        strValue = strValue & "," & .objDaisyo
                        strValue = strValue & "," & .Index1
                        strValue = strValue & "," & .Index2
                        strValue = strValue & "," & .Bunrui1
                        strValue = strValue & "," & .Bunrui2
                        strValue = strValue & "," & .flgBunrui
                        strValue = strValue & ",'" & .flgOnlyOne.ToString.ToUpper() & "'"
                        strValue = strValue & ",'" & .strName & "'"
                        strValue = strValue & ",'" & .strBikou & "'"


                        Dim strSql As String = " INSERT INTO TenGunTeigi( " & strSelect & " ) " & " VALUES( " & strValue & " ) "

                        dbclass.ExecuteSQL(strSql)
                    End With

                End If
            End While
        End Using


    End Sub


End Class
