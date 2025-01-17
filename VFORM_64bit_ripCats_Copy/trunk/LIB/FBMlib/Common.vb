﻿Imports Microsoft.VisualBasic.FileIO
Imports HalconDotNet

Public Module Common

    Public MRS As New MatchRansac(1)
    Public PP As New PointsParam(1)
    Public Const cons_Epi As Double = 0.1
    Public Const ScaleDevEpi As Double = 0.01
    Public Const CrossSize As Integer = 10
    Public Const CrossAngle As Integer = 0.785398
    Public Const FBM_MDB As String = "dbFBM.mdb"
    Public Const YCM_TEMP_MDB As String = "計測データ_temp.mdb"
    Public Const YCM_MDB As String = "計測データ.mdb"
    Public Const YCM_SYS_MDB As String = "システム設定.mdb"       '追加　ＴＵＧＩ
    Public Const YCM_SYS_MDB_KIJYUNTBL As String = "基準座標"       '追加　ＴＵＧＩ
    Public Const YCM_SYS_FLDR As String = "\計測システムフォルダ\"        '追加　ＴＵＧＩ
    Public intCommonCTnum As Integer = 3
    Public MidRow1 As Object = Nothing
    Public MidCol1 As Object = Nothing
    Public MidRow2 As Object = Nothing
    Public MidCol2 As Object = Nothing
    Public dbClass As CDBOperateOLE
    Public epiDev As Double
    Public MeanPixErr As Double
    Public MaxPixErr As Double
    Public T_treshold As Double
    Public dblImageZoomFactor As Double = 1
    Public SingleTargetFields As String() = {"ImageID", "P2ID", "P3ID", "flgUsed", "P2D_Row", "P2D_Col", _
                                             "RayP1_X", "RayP1_Y", "RayP1_Z", "RayP2_X", "RayP2_Y", "RayP2_Z", "Dist", "ReProjectionError", "flgType"}
    Public CameraPoseFields As String() = {"ID", "imagefilename", "CX", "CY", "CZ", "CA", "CB", "CG", "flgNotUse", "flgSystemNotUse", "flgFirst", "flgSecond"}
    Public ImageCNT As Integer
    Public FirstI As Integer = 0
    Public FirstJ As Integer = 0
    Public IGFlag As Object = Nothing
    Public cntCurrentConnectedImage As Integer = 0
    'SUSANO 追加　開始　２０１５０３２０
    Public lstSTtoCT As List(Of STtoCT)
    'add by SUSANO
    Public Declare Ansi Function GetPrivateProfileInt Lib "kernel32.dll" Alias "GetPrivateProfileIntA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal nDefault As Integer, ByVal lpFileName As String) As Integer

    Public Structure STtoCT
        Public CTID As Integer
        Public flgKijyun As Integer

        Public Sub New(ByVal aa As Integer)
            CTID = -1
            flgKijyun = -1
        End Sub
    End Structure
    Public Sub GetListSTtoCT(ByVal listfilename As String)
        Dim filename As String = listfilename
        Dim fields As String()
        Dim delimiter As String = ","
        If lstSTtoCT Is Nothing Then
            lstSTtoCT = New List(Of STtoCT)
        Else
            lstSTtoCT.Clear()
        End If
        Using parser As New TextFieldParser(filename)
            parser.SetDelimiters(delimiter)
            While Not parser.EndOfData
                ' Read in the fields for the current line
                fields = parser.ReadFields()
                ' Add code here to use data in fields variable.
                If IsNumeric(fields(0)) Then
                    Dim objSTtoCT As New STtoCT(1)
                    objSTtoCT.CTID = CInt(fields(0))
                    objSTtoCT.flgKijyun = CInt(fields(1))
                    lstSTtoCT.Add(objSTtoCT)
                End If
            End While
        End Using

    End Sub
    'SUSANO 追加　終了　２０１５０３２１
    Public Structure MatchRansac

        Public GrayMatchMethod As String

        Public MaskSize As Integer

        Public RowMove As Integer

        Public ColMove As Integer

        Public RowTolerance As Integer

        Public ColTolerance As Integer

        Public Rotation As Object

        Public MatchThreshold As Double

        Public EstimationMethod As String '= "gold_standard"

        Public DistanceThreshold As Double

        Public RandSeed As Integer

        Public Sub New(ByVal D As Integer)
            My.Settings.Upgrade()
            With My.Settings

                GrayMatchMethod = .GrayMatchMethod

                MaskSize = .MaskSize

                RowMove = .RowMove

                ColMove = .ColMove

                RowTolerance = .RowTolerance

                ColTolerance = .ColTolerance

                Rotation = .Rotation

                MatchThreshold = .MatchThreshold

                EstimationMethod = .EstimationMethod

                DistanceThreshold = .DistanceThreshold

                RandSeed = .RandSeed

            End With
        End Sub

    End Structure

    Public Enum TargetType
        SingleTarget = 1
        CodedTarget = 2
    End Enum


    Public Class ConnectingPair
        Public ind1 As Integer
        Public ind2 As Integer
        Public ind3 As Integer
        Public Sub New()
            ind1 = -1
            ind2 = -1
            ind3 = -1
        End Sub
        Public Sub New(ByVal i As Integer, ByVal j As Integer, ByVal t As Integer)
            ind1 = i
            ind2 = j
            ind3 = t
        End Sub
    End Class

    Public Class MessageEventArgs

        Inherits EventArgs

        Dim m_ImageIndex As Integer
        Public Property ImageIndex() As Integer
            Get
                Return m_ImageIndex
            End Get
            Set(ByVal value As Integer)
                m_ImageIndex = value
            End Set
        End Property

        Dim m_ImageCount As Integer
        Public Property ImageCount() As Integer
            Get
                Return m_ImageCount
            End Get
            Set(ByVal value As Integer)
                m_ImageCount = value
            End Set
        End Property

        Dim m_MessageText As String
        Public Property MessageText() As String
            Get
                Return m_MessageText
            End Get
            Set(ByVal value As String)
                m_MessageText = value
            End Set
        End Property
    End Class

    ' Chapter: File
    ' Short Description: Get all image files under the given path
    Public Sub list_image_files(ByVal hv_ImageDirectory As Object, ByVal hv_Extensions As Object, _
        ByVal hv_Options As Object, ByRef hv_ImageFiles As Object)

        ' Stack for temporary control variables 
        Dim CTemp(10) As Object
        Dim SP_C As Long
        SP_C = 0

        ' Local control variables 
        Dim hv_HalconImages As Object = Nothing, hv_Halconroot As Object = Nothing
        Dim hv_OS As Object = Nothing, hv_Directories As Object = Nothing
        Dim hv_Index As Object = Nothing, hv_FileExists As Object = Nothing
        Dim hv_AllFiles As Object = Nothing, hv_i As Object = Nothing
        Dim hv_Selection As Object = Nothing

        ' Initialize local and output iconic variables 

        'This procedure returns all files in a given directory 
        'with one of the suffixes specified in Extensions.
        '
        'input parameters:
        'ImageDirectory: as the name says
        '   If a tuple of directories is given, only the images in the first
        '   existing directory are returned.
        '   If a local directory is not found, the directory is searched
        '   under %HALCONIMAGES%/ImageDirectory. If %HALCONIMAGES% is not set,
        '   %HALCONROOT%/images is used instead.
        'Extensions: A string tuple containing the extensions to be found
        '   e.g. ['png','tif',jpg'] or others
        'If Extensions is set to 'default' or the empty string '',
        '   all image suffixes supported by HALCON are used.
        'Options: as in the operator list_files, except that the 'files'
        '   option is always used. Note that the 'directories' option
        '   has no effect but increases runtime, because only files are
        '   returned.
        '
        'output parameter:
        'ImageFiles: A tuple of all found image file names
        '
        If BTuple.TupleOr(BTuple.TupleOr(BTuple.TupleEqual(hv_Extensions, Nothing), _
            BTuple.TupleEqual(hv_Extensions, "")), BTuple.TupleEqual(hv_Extensions, "default")) Then
            hv_Extensions = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat( _
                BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat( _
                BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat("ima", _
                "bmp"), "jpg"), "png"), "tiff"), "tif"), "gif"), "jpeg"), "pcx"), "pgm"), "ppm"), "pbm"), _
                "xwd"), "pnm") 'ファイルを探している?
        End If
        If BTuple.TupleEqual(hv_ImageDirectory, "") Then 'フォルダ

            hv_ImageDirectory = "." '20121220フォルダ
        End If
        HOperatorSet.TupleEnvironment("HALCONIMAGES", hv_HalconImages)
        If BTuple.TupleEqual(hv_HalconImages, "") Then
            HOperatorSet.TupleEnvironment("HALCONROOT", hv_Halconroot)
            hv_HalconImages = BTuple.TupleAdd(hv_Halconroot, "/images")
        End If
        HOperatorSet.GetSystem("operating_system", hv_OS)
        If BTuple.TupleEqual(BTuple.TupleStrLastN(BTuple.TupleStrFirstN(hv_OS, 2), 0), "Win") Then
            CTemp(SP_C) = hv_HalconImages
            SP_C = SP_C + 1
            HOperatorSet.TupleSplit(CTemp(SP_C - 1), ";", hv_HalconImages)
            SP_C = 0
        Else
            CTemp(SP_C) = hv_HalconImages
            SP_C = SP_C + 1
            HOperatorSet.TupleSplit(CTemp(SP_C - 1), ":", hv_HalconImages)
            SP_C = 0
        End If
        hv_Directories = BTuple.TupleConcat(hv_ImageDirectory, BTuple.TupleAdd(BTuple.TupleAdd( _
            hv_HalconImages, "/"), hv_ImageDirectory)) '20121220フォルダ
        hv_ImageFiles = Nothing
        For hv_Index = 0 To BTuple.TupleSub(BTuple.TupleLength(hv_Directories), 1) Step 1
            HOperatorSet.FileExists(BTuple.TupleSelect(hv_Directories, hv_Index), hv_FileExists)
            If hv_FileExists Then
                'HOperatorSet.ListFiles(BTuple.TupleSelect(hv_Directories, hv_Index), "files", hv_AllFiles)

                HOperatorSet.ListFiles(BTuple.TupleSelect(hv_Directories, hv_Index), BTuple.TupleConcat( _
                    "files", hv_Options), hv_AllFiles)
                hv_ImageFiles = Nothing
                For hv_i = 0 To BTuple.TupleSub(BTuple.TupleLength(hv_Extensions), 1) Step 1
                    HOperatorSet.TupleRegexpSelect(hv_AllFiles, BTuple.TupleConcat(BTuple.TupleAdd(BTuple.TupleAdd( _
                        ".*", BTuple.TupleSelect(hv_Extensions, hv_i)), "$"), "ignore_case"), hv_Selection)
                    hv_ImageFiles = BTuple.TupleConcat(hv_ImageFiles, hv_Selection)
#If USE_DO_EVENTS Then
        ' Please note: The call of DoEvents() is only a hack to
        ' enable VB to react on events. Please change the code
        ' so that it can handle events in a standard way.
        System.Windows.Forms.Application.DoEvents()
#End If
                Next
                CTemp(SP_C) = hv_ImageFiles '20121220画像枚数
                SP_C = SP_C + 1
                HOperatorSet.TupleRegexpReplace(CTemp(SP_C - 1), BTuple.TupleConcat("\\", "replace_all"), _
                    "/", hv_ImageFiles)
                SP_C = 0
                CTemp(SP_C) = hv_ImageFiles
                SP_C = SP_C + 1
                HOperatorSet.TupleRegexpReplace(CTemp(SP_C - 1), BTuple.TupleConcat("//", "replace_all"), _
                    "/", hv_ImageFiles)
                SP_C = 0

                Exit Sub
            End If
#If USE_DO_EVENTS Then
    ' Please note: The call of DoEvents() is only a hack to
    ' enable VB to react on events. Please change the code
    ' so that it can handle events in a standard way.
    System.Windows.Forms.Application.DoEvents()
#End If
        Next

        Exit Sub
    End Sub


    ' Chapter: Tools / Geometry
    ' Short Description: Sort tuple pairs.
    Public Sub sort_pairs(ByVal hv_T1 As Object, ByVal hv_T2 As Object, ByVal hv_SortMode As Object, _
        ByRef hv_Sorted1 As Object, ByRef hv_Sorted2 As Object)

        ' Local control variables 
        Dim hv_Indices1 As Object = Nothing, hv_Indices2 As Object = Nothing

        ' Initialize local and output iconic variables 

        'Sort tuple pairs.
        '
        'input parameters:
        'T1: first tuple
        'T2: second tuple
        'SortMode: if set to '1', sort by the first tuple,
        '   if set to '2', sort by the second tuple
        '
        If BTuple.TupleOr(BTuple.TupleEqual(hv_SortMode, "1"), BTuple.TupleEqual(hv_SortMode, _
            1)) Then
            HOperatorSet.TupleSortIndex(hv_T1, hv_Indices1)
            hv_Sorted1 = BTuple.TupleSelect(hv_T1, hv_Indices1)
            hv_Sorted2 = BTuple.TupleSelect(hv_T2, hv_Indices1)
        ElseIf BTuple.TupleOr(BTuple.TupleOr(BTuple.TupleEqual(hv_SortMode, "column"), BTuple.TupleEqual( _
            hv_SortMode, "2")), BTuple.TupleEqual(hv_SortMode, 2)) Then
            HOperatorSet.TupleSortIndex(hv_T2, hv_Indices2)
            hv_Sorted1 = BTuple.TupleSelect(hv_T1, hv_Indices2)
            hv_Sorted2 = BTuple.TupleSelect(hv_T2, hv_Indices2)
        End If

        Exit Sub
    End Sub

    ' Chapter: Tuple / Creation
    ' Short Description: This procedure generates a tuple with a sequence of equidistant values.
    Public Sub tuple_gen_sequence(ByVal hv_Start As Object, ByVal hv_End As Object, _
        ByVal hv_Step As Object, ByRef hv_Sequence As Object)

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
        hv_Sequence = BTuple.TupleAdd(BTuple.TupleSub(hv_Start, hv_Step), BTuple.TupleCumul( _
            BTuple.TupleGenConst(BTuple.TupleAdd(BTuple.TupleInt(BTuple.TupleDiv(BTuple.TupleSub( _
            hv_End, hv_Start), hv_Step)), 1), hv_Step)))

        Exit Sub
    End Sub


    ' Short Description: Estimate the affine 3D transformation between corresponding two 3D point sets
    Public Sub hom_mat_3d_from_3d_3d_point_correspondence(ByVal hv_PX As Object, ByVal hv_PY As Object, ByVal hv_PZ As Object, _
                                                          ByVal hv_QX As Object, ByVal hv_QY As Object, ByVal hv_QZ As Object, _
                                                          ByRef hv_HomMat3D As Object)

        ' Local control variables 
        Dim hv_P As Object = Nothing, hv_PShift As Object = Nothing
        Dim hv_PMean As Object = Nothing, hv_Q As Object = Nothing
        Dim hv_QShift As Object = Nothing, hv_QMean As Object = Nothing
        Dim hv_M As Object = Nothing, hv_Index As Object = Nothing
        Dim hv_PVec As Object = Nothing, hv_QVec As Object = Nothing
        Dim hv_PQ As Object = Nothing, hv_U As Object = Nothing
        Dim hv_S As Object = Nothing, hv_V As Object = Nothing
        Dim hv_R As Object = Nothing, hv_Value As Object = Nothing
        Dim hv_Value1 As Object = Nothing, hv_RPMean As Object = Nothing
        Dim hv_t As Object = Nothing, hv_HomMat3DID As Object = Nothing

        ' Initialize local and output iconic variables 

        'Create data matrix from given points and shift them to the origin.
        HOperatorSet.CreateMatrix(3, BTuple.TupleLength(hv_PX), BTuple.TupleConcat(BTuple.TupleConcat(hv_PX, hv_PY), hv_PZ), hv_P)
        shift_data_to_origin(hv_P, hv_PShift, hv_PMean)
        HOperatorSet.CreateMatrix(3, BTuple.TupleLength(hv_QX), BTuple.TupleConcat(BTuple.TupleConcat(hv_QX, hv_QY), hv_QZ), hv_Q)
        shift_data_to_origin(hv_Q, hv_QShift, hv_QMean)
        'Create matrix for rotational part.
        HOperatorSet.CreateMatrix(3, 3, 0, hv_M)
        For hv_Index = 0 To BTuple.TupleSub(BTuple.TupleLength(hv_PX), 1) Step 1
            HOperatorSet.GetSubMatrix(hv_PShift, 0, hv_Index, 3, 1, hv_PVec)
            HOperatorSet.GetSubMatrix(hv_QShift, 0, hv_Index, 3, 1, hv_QVec)
            HOperatorSet.TransposeMatrixMod(hv_QVec)
            HOperatorSet.MultMatrix(hv_PVec, hv_QVec, "AB", hv_PQ)
            HOperatorSet.AddMatrixMod(hv_M, hv_PQ)
            HOperatorSet.ClearMatrix(BTuple.TupleConcat(BTuple.TupleConcat(hv_PVec, hv_QVec), hv_PQ))
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
        If BTuple.TupleLess(hv_Value, 0).I = 1 Then
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


    Public Sub shift_data_to_origin(ByVal hv_P As Object, ByRef hv_PShift As Object, ByRef hv_PMean As Object)

        ' Local control variables 
        Dim hv_Rows As Object = Nothing, hv_Columns As Object = Nothing
        Dim hv_PSum As Object = Nothing, hv_Ones As Object = Nothing
        Dim hv_PSub As Object = Nothing

        ' Initialize local and output iconic variables 

        HOperatorSet.GetSizeMatrix(hv_P, hv_Rows, hv_Columns)
        'One could also use sum_/scale_matrix 
        'sum_matrix (P, 'rows', PSum)
        'scale_matrix (PSum, 1./Columns, PMean)
        'Determine the mean of the data set
        HOperatorSet.MeanMatrix(hv_P, "rows", hv_PMean)
        'Move data to the origin of the coordinate system.
        HOperatorSet.CreateMatrix(1, hv_Columns, 1, hv_Ones)
        HOperatorSet.MultMatrix(hv_PMean, hv_Ones, "AB", hv_PSub)
        HOperatorSet.SubMatrix(hv_P, hv_PSub, hv_PShift)
        'Delete intermediate calculation results.
        HOperatorSet.ClearMatrix(BTuple.TupleConcat(hv_PSub, hv_Ones))

        Exit Sub
    End Sub

    Public Function getFileNamefromFullPath(ByVal strFullPath As String) As String
        Dim strSplit() As String
        strSplit = strFullPath.Split("\")
        getFileNamefromFullPath = strSplit((strSplit.Length - 1))

    End Function

    Public Sub CalcScaleForRelPose(ByVal hv_RelPose12 As Object, ByVal hv_RelPose23 As Object, _
     ByVal hv_RelPose13 As Object, ByRef hv_Scale23 As Object, ByRef hv_Scale13 As Object)

        ' Stack for temporary control variables 
        Dim CTemp(10) As Object
        Dim SP_C As Long
        SP_C = 0

        ' Local control variables 
        Dim hv_Pow13 As Object = Nothing, hv_A1 As Object = Nothing
        Dim hv_HomMat3D12 As Object = Nothing, hv_HomMat3D23 As Object = Nothing
        Dim hv_HomMat3DCompose As Object = Nothing, hv_Prod13_23 As Object = Nothing
        Dim hv_B As Object = Nothing, hv_B1 As Object = Nothing
        Dim hv_A2 As Object = Nothing, hv_Pow23 As Object = Nothing
        Dim hv_B2 As Object = Nothing, hv_Prod12_13 As Object = Nothing
        Dim hv_C1 As Object = Nothing, hv_Prod12_23 As Object = Nothing
        Dim hv_C2 As Object = Nothing, hv_AMatrixID As Object = Nothing
        Dim hv_CMatrixID As Object = Nothing, hv_SMatrixID As Object = Nothing
        Dim hv_Values As Object = Nothing

        ' Initialize local and output iconic variables 

        HOperatorSet.TuplePow(hv_RelPose13, 2, hv_Pow13)
        HOperatorSet.TupleSum(BTuple.TupleSelectRange(hv_Pow13, 0, 2), hv_A1)
        HOperatorSet.PoseToHomMat3d(hv_RelPose12, hv_HomMat3D12)
        HOperatorSet.PoseToHomMat3d(hv_RelPose23, hv_HomMat3D23)
        HOperatorSet.HomMat3dCompose(hv_HomMat3D12, hv_HomMat3D23, hv_HomMat3DCompose)
        HOperatorSet.HomMat3dToPose(hv_HomMat3DCompose, hv_RelPose23)
        CTemp(SP_C) = hv_RelPose23
        SP_C = SP_C + 1
        HOperatorSet.TupleSub(CTemp(SP_C - 1), hv_RelPose12, hv_RelPose23)
        SP_C = 0
        HOperatorSet.TupleMult(hv_RelPose13, hv_RelPose23, hv_Prod13_23)
        HOperatorSet.TupleSum(BTuple.TupleSelectRange(hv_Prod13_23, 0, 2), hv_B)
        hv_B1 = BTuple.TupleSub(0, hv_B)
        hv_A2 = BTuple.TupleSub(0, hv_B)
        HOperatorSet.TuplePow(hv_RelPose23, 2, hv_Pow23)
        HOperatorSet.TupleSum(BTuple.TupleSelectRange(hv_Pow23, 0, 2), hv_B2)
        HOperatorSet.TupleMult(hv_RelPose12, hv_RelPose13, hv_Prod12_13)
        HOperatorSet.TupleSum(BTuple.TupleSelectRange(hv_Prod12_13, 0, 2), hv_C1)
        HOperatorSet.TupleMult(hv_RelPose12, hv_RelPose23, hv_Prod12_23)
        HOperatorSet.TupleSum(BTuple.TupleSelectRange(hv_Prod12_23, 0, 2), hv_C2)
        hv_C2 = BTuple.TupleSub(0, hv_C2)
        HOperatorSet.CreateMatrix(2, 2, BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(hv_A1, _
            hv_B1), hv_A2), hv_B2), hv_AMatrixID)
        HOperatorSet.CreateMatrix(2, 1, BTuple.TupleConcat(hv_C1, hv_C2), hv_CMatrixID)
        HOperatorSet.SolveMatrix(hv_AMatrixID, "general", 0, hv_CMatrixID, hv_SMatrixID)
        HOperatorSet.GetFullMatrix(hv_SMatrixID, hv_Values)
        hv_Scale13 = BTuple.TupleSelect(hv_Values, 0)
        hv_Scale23 = BTuple.TupleSelect(hv_Values, 1)
        HOperatorSet.ClearMatrix(hv_AMatrixID)
        HOperatorSet.ClearMatrix(hv_CMatrixID)
        HOperatorSet.ClearMatrix(hv_SMatrixID)

        Exit Sub
    End Sub

    Public Sub CalcBestScale(ByVal Points1 As Point3D, ByVal Points2 As Point3D, ByRef BestScale As Object)
        Dim tmpPoints As New Point3D
        Dim tmpVal1 As New Object
        Dim tmpVal2 As New Object
        Dim tmpAdd As New Object
        Dim tmpSqrt As New Object
        Dim tmpPowX As New Object
        Dim tmpPowY As New Object
        Dim tmpPowZ As New Object
        Points1.GetMult3D(Points2, tmpPoints)
        tmpVal1 = tmpPoints.GetSumXYZ()
        tmpPowX = BTuple.TuplePow(Points2.X, 2)
        tmpPowY = BTuple.TuplePow(Points2.Y, 2)
        tmpPowZ = BTuple.TuplePow(Points2.Z, 2)
        tmpAdd = BTuple.TupleAdd(tmpPowX, tmpPowY)
        tmpAdd = BTuple.TupleAdd(tmpAdd, tmpPowZ)
        tmpVal2 = BTuple.TupleSum(tmpAdd)
        BestScale = BTuple.TupleDiv(tmpVal1, tmpVal2)

    End Sub


    Public Sub CalcBestScaleWithWeight(ByVal XYZ12 As Point3D, ByVal XYZ23 As Point3D, ByVal N As Integer, ByRef hv_BestScaleWeighted As Object)

        ' Stack for temporary control variables 
        Dim CTemp(10) As Object
        Dim SP_C As Long
        SP_C = 0

        ' Local control variables 
        Dim hv_XMid23 As Object = Nothing, hv_YMid23 As Object = Nothing
        Dim hv_ZMid23 As Object = Nothing, hv_Qx1 As Object = Nothing, hv_Qy1 As Object = Nothing, hv_Qz1 As Object = Nothing

        Dim hv_DiffX As Object = Nothing, hv_DiffY As Object = Nothing
        Dim hv_DiffZ As Object = Nothing
        Dim hv_WX As Object = Nothing, hv_WY As Object = Nothing
        Dim hv_WZ As Object = Nothing, hv_ProdX As Object = Nothing
        Dim hv_ProdY As Object = Nothing, hv_ProdZ As Object = Nothing
        Dim hv_Sum2 As Object = Nothing, hv_Sum3 As Object = Nothing
        Dim hv_PowX As Object = Nothing, hv_PowY As Object = Nothing
        Dim hv_PowZ As Object = Nothing, hv_Sum4 As Object = Nothing
        Dim hv_Quot As Object = Nothing, hv_XX23 As Object = Nothing
        Dim hv_YY23 As Object = Nothing, hv_ZZ23 As Object = Nothing
        Dim hv_Sum As Object = Nothing, hv_Delta As Object = Nothing
        Dim hv_Sum1 As Object = Nothing, hv_Mean1 As Object = Nothing
        Dim hv_Deviation1 As Object = Nothing
        Dim hv_I As Integer
        Dim epi As Double = 0.000001
        ' Initialize local and output iconic variables 
        hv_XMid23 = XYZ23.X
        hv_YMid23 = XYZ23.Y
        hv_ZMid23 = XYZ23.Z
        hv_Qx1 = XYZ12.X
        hv_Qy1 = XYZ12.Y
        hv_Qz1 = XYZ12.Z

        HOperatorSet.TupleGenConst(BTuple.TupleLength(hv_XMid23), 1, hv_DiffX)
        hv_DiffY = hv_DiffX
        hv_DiffZ = hv_DiffX
        For hv_I = 1 To N Step 1
            hv_WX = BTuple.TupleDiv(1, BTuple.TupleAdd(BTuple.TupleAbs(hv_DiffX), epi))
            hv_WY = BTuple.TupleDiv(1, BTuple.TupleAdd(BTuple.TupleAbs(hv_DiffY), epi))
            hv_WZ = BTuple.TupleDiv(1, BTuple.TupleAdd(BTuple.TupleAbs(hv_DiffZ), epi))
            hv_ProdX = BTuple.TupleMult(BTuple.TupleMult(hv_XMid23, hv_Qx1), hv_WX)
            hv_ProdY = BTuple.TupleMult(BTuple.TupleMult(hv_YMid23, hv_Qy1), hv_WY)
            hv_ProdZ = BTuple.TupleMult(BTuple.TupleMult(hv_ZMid23, hv_Qz1), hv_WZ)
            hv_Sum2 = BTuple.TupleAdd(BTuple.TupleAdd(hv_ProdX, hv_ProdY), hv_ProdZ)
            hv_Sum3 = BTuple.TupleSum(hv_Sum2)

            HOperatorSet.TuplePow(hv_XMid23, 2, hv_PowX)
            HOperatorSet.TuplePow(hv_YMid23, 2, hv_PowY)
            HOperatorSet.TuplePow(hv_ZMid23, 2, hv_PowZ)
            hv_PowX = BTuple.TupleMult(hv_PowX, hv_WX)
            hv_PowY = BTuple.TupleMult(hv_PowY, hv_WY)
            hv_PowZ = BTuple.TupleMult(hv_PowZ, hv_WZ)
            hv_Sum2 = BTuple.TupleAdd(BTuple.TupleAdd(hv_PowX, hv_PowY), hv_PowZ)
            HOperatorSet.TupleSum(hv_Sum2, hv_Sum4)
            HOperatorSet.TupleDiv(hv_Sum3, hv_Sum4, hv_Quot)
            hv_BestScaleWeighted = hv_Quot

            hv_XX23 = BTuple.TupleMult(hv_XMid23, hv_BestScaleWeighted)
            hv_YY23 = BTuple.TupleMult(hv_YMid23, hv_BestScaleWeighted)
            hv_ZZ23 = BTuple.TupleMult(hv_ZMid23, hv_BestScaleWeighted)
            HOperatorSet.TupleSub(hv_XX23, hv_Qx1, hv_DiffX)
            HOperatorSet.TupleSub(hv_YY23, hv_Qy1, hv_DiffY)
            HOperatorSet.TupleSub(hv_ZZ23, hv_Qz1, hv_DiffZ)
        Next

        Exit Sub
    End Sub


    Public Sub shift_data_to_origin_weighted(ByVal hv_P As Object, ByVal hv_W As Object, _
        ByRef hv_PShift As Object, ByRef hv_PMean As Object)

        ' Local control variables 
        Dim hv_Rows As Object = Nothing, hv_Columns As Object = Nothing
        Dim hv_PSum As Object = Nothing, hv_Ones As Object = Nothing
        Dim hv_PSub As Object = Nothing, hv_Values As Object = Nothing
        Dim hv_MatrixMultID1 As Object = Nothing, hv_PX As Object = Nothing
        Dim hv_PY As Object = Nothing, hv_PZ As Object = Nothing
        Dim hv_QX As Object = Nothing, hv_QY As Object = Nothing
        Dim hv_QZ As Object = Nothing, hv_MatrixSumID As Object = Nothing
        Dim hv_WMatrixSumID As Object = Nothing


        ' Initialize local and output iconic variables 

        HOperatorSet.GetSizeMatrix(hv_P, hv_Rows, hv_Columns)
        'One could also use sum_/scale_matrix 
        'sum_matrix (P, 'rows', PSum)
        'scale_matrix (PSum, 1./Columns, PMean)
        'Determine the mean of the data set
        HOperatorSet.MultElementMatrix(hv_P, hv_W, hv_MatrixMultID1)
        HOperatorSet.SumMatrix(hv_MatrixMultID1, "rows", hv_MatrixSumID)
        HOperatorSet.SumMatrix(hv_W, "rows", hv_WMatrixSumID)
        HOperatorSet.DivElementMatrix(hv_MatrixSumID, hv_WMatrixSumID, hv_PMean)
        HOperatorSet.CreateMatrix(1, hv_Columns, 1, hv_Ones)
        HOperatorSet.MultMatrix(hv_PMean, hv_Ones, "AB", hv_PSub)
        HOperatorSet.SubMatrix(hv_P, hv_PSub, hv_PShift)
        HOperatorSet.ClearMatrix(BTuple.TupleConcat(hv_PSub, hv_Ones))
        HOperatorSet.ClearMatrix(hv_MatrixMultID1)
        HOperatorSet.ClearMatrix(hv_MatrixSumID)
        HOperatorSet.ClearMatrix(hv_WMatrixSumID)

        Exit Sub
    End Sub

    Public Sub correspond_3d_3d_weight(ByVal hv_PX As Object, ByVal hv_PY As Object, _
        ByVal hv_PZ As Object, ByVal hv_QX As Object, ByVal hv_QY As Object, ByVal hv_QZ As Object, _
        ByVal hv_WX As Object, ByVal hv_WY As Object, ByVal hv_WZ As Object, ByRef hv_HomMat3D As Object)

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
        If BTuple.TupleLess(hv_Value, 0) Then
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

    Public Sub correspond_3d_3d_point_withweight(ByVal hv_PX As Object, ByVal hv_PY As Object, _
        ByVal hv_PZ As Object, ByVal hv_QX As Object, ByVal hv_QY As Object, ByVal hv_QZ As Object, _
        ByVal hv_N As Object, ByVal hv_epi As Object, ByRef hv_HomMat3D As Object)

        ' Stack for temporary control variables 
        Dim CTemp(10) As Object
        Dim SP_C As Long
        SP_C = 0

        ' Local control variables 
        Dim hv_WX As Object = Nothing, hv_WY As Object = Nothing
        Dim hv_WZ As Object = Nothing, hv_Index As Object = Nothing
        Dim hv_Qxtest As Object = Nothing, hv_Qytest As Object = Nothing
        Dim hv_Qztest As Object = Nothing, hv_DiffX As Object = Nothing
        Dim hv_DiffY As Object = Nothing, hv_DiffZ As Object = Nothing
        Dim hv_MDX As Object = Nothing, hv_MDY As Object = Nothing, hv_MDZ As Object = Nothing

        ' Initialize local and output iconic variables 

        HOperatorSet.TupleGenConst(BTuple.TupleLength(hv_PX), 1.0#, hv_WX)
        hv_WY = hv_WX
        hv_WZ = hv_WX
        For hv_Index = 1 To hv_N Step 1
            correspond_3d_3d_weight(hv_PX, hv_PY, hv_PZ, hv_QX, hv_QY, hv_QZ, hv_WX, hv_WY, _
                hv_WZ, hv_HomMat3D)
            HOperatorSet.AffineTransPoint3d(hv_HomMat3D, hv_PX, hv_PY, hv_PZ, hv_Qxtest, hv_Qytest, _
                hv_Qztest)
            HOperatorSet.TupleSub(hv_QX, hv_Qxtest, hv_DiffX)
            HOperatorSet.TupleSub(hv_QY, hv_Qytest, hv_DiffY)
            HOperatorSet.TupleSub(hv_QZ, hv_Qztest, hv_DiffZ)
            HOperatorSet.TupleMean(hv_DiffX, hv_MDX)
            HOperatorSet.TupleMean(hv_DiffY, hv_MDY)
            HOperatorSet.TupleMean(hv_DiffZ, hv_MDZ)
            HOperatorSet.TupleSub(hv_DiffX, hv_MDX, hv_WX)
            HOperatorSet.TupleSub(hv_DiffY, hv_MDY, hv_WY)
            HOperatorSet.TupleSub(hv_DiffZ, hv_MDZ, hv_WZ)
            HOperatorSet.TupleAbs(hv_WX, hv_WX)
            HOperatorSet.TupleAbs(hv_WY, hv_WY)
            HOperatorSet.TupleAbs(hv_WZ, hv_WZ)

            CTemp(SP_C) = hv_WX
            SP_C = SP_C + 1
            HOperatorSet.TupleAdd(CTemp(SP_C - 1), hv_epi, hv_WX)
            SP_C = 0
            CTemp(SP_C) = hv_WY
            SP_C = SP_C + 1
            HOperatorSet.TupleAdd(CTemp(SP_C - 1), hv_epi, hv_WY)
            SP_C = 0
            CTemp(SP_C) = hv_WZ
            SP_C = SP_C + 1
            HOperatorSet.TupleAdd(CTemp(SP_C - 1), hv_epi, hv_WZ)
            SP_C = 0

            HOperatorSet.TuplePow(hv_WX, hv_WX, hv_WX)
            HOperatorSet.TuplePow(hv_WY, hv_WY, hv_WY)
            HOperatorSet.TuplePow(hv_WZ, hv_WZ, hv_WZ)

            CTemp(SP_C) = hv_WX
            SP_C = SP_C + 1
            HOperatorSet.TupleDiv(1, CTemp(SP_C - 1), hv_WX)
            SP_C = 0
            CTemp(SP_C) = hv_WY
            SP_C = SP_C + 1
            HOperatorSet.TupleDiv(1, CTemp(SP_C - 1), hv_WY)
            SP_C = 0
            CTemp(SP_C) = hv_WZ
            SP_C = SP_C + 1
            HOperatorSet.TupleDiv(1, CTemp(SP_C - 1), hv_WZ)
            SP_C = 0
#If USE_DO_EVENTS Then
    ' Please note: The call of DoEvents() is only a hack to
    ' enable VB to react on events. Please change the code
    ' so that it can handle events in a standard way.
    System.Windows.Forms.Application.DoEvents()
#End If
        Next

        Exit Sub
    End Sub


    Public Sub SaveTupleObj(ByVal objTuple As Object, ByVal strFullPath As String)
        Dim intLen As Integer = 0

        Try
            HOperatorSet.TupleLength(objTuple, intLen)
        Catch ex As Exception
            intLen = 0
        End Try

        If intLen = 0 Then
        Else
            'Dim hv_FileHandle As Object = Nothing
            'HOperatorSet.OpenFile(strFullPath, "output", hv_FileHandle)

            'HOperatorSet.FwriteString(hv_FileHandle, BTuple.TupleAdd(objTuple, ","))

            'HOperatorSet.CloseFile(hv_FileHandle)
            HOperatorSet.WriteTuple(objTuple, strFullPath)
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
            '    HOperatorSet.OpenFile(strFullPath, "input", hv_FileHandle)
            '    HOperatorSet.FreadString(hv_FileHandle, hv_OutString, hv_IsEOF)
            '    HOperatorSet.TupleSplit(hv_OutString, ",", hv_Substrings)
            '    HOperatorSet.TupleNumber(hv_Substrings, objTuple)
            '    HOperatorSet.CloseFile(hv_FileHandle)
            'Catch ex As Exception
            '    Dim t As Integer = 1
            'End Try
            Try
#If Halcon = "True" Then

                HOperatorSet.ReadTuple(strFullPath, objTuple)
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
        If (values Is (Nothing)) Then
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

    Public Sub WritePP_XML()
        Dim xmlSerializer As System.Xml.Serialization.XmlSerializer
        Dim strPath = My.Application.Info.DirectoryPath & "\PointsParam.xml"

        xmlSerializer = New System.Xml.Serialization.XmlSerializer(GetType(PointsParam))
        Using fileStream As New System.IO.FileStream(strPath, IO.FileMode.Create)
            xmlSerializer.Serialize(fileStream, PP)
        End Using

    End Sub

    Public Class PointsParam
        Public SigmaGrad As Integer
        Public SigmaSmooth As Integer
        Public Alpha As Double
        Public Threshold As Integer
        Public Sub New()

        End Sub
        Public Sub New(ByVal D As Integer)
            SigmaGrad = My.Settings.PP_SigmaGrad
            SigmaSmooth = My.Settings.PP_SigmaSmooth
            Alpha = My.Settings.PP_Alpha
            Threshold = My.Settings.PP_Threshold
        End Sub
    End Class

    Public Function TransCoordSystem(ByVal PG As Point3D, ByVal PX As Point3D, ByVal PY As Point3D, ByRef HomMat3d As Object) As Boolean
        Dim NewPG As New Point3D
        Dim NewPX As New Point3D
        Dim NewPY As New Point3D

        Dim XM As New Object
        Dim YM As New Object
        Dim ZM As New Object
        Dim XR As New Object
        Dim YR As New Object
        Dim ZR As New Object

        If CalcCoordSystem(PG, PX, PY, NewPG, NewPX, NewPY) Then
            XM = BTuple.TupleConcat(PG.X, BTuple.TupleConcat(PX.X, PY.X))
            YM = BTuple.TupleConcat(PG.Y, BTuple.TupleConcat(PX.Y, PY.Y))
            ZM = BTuple.TupleConcat(PG.Z, BTuple.TupleConcat(PX.Z, PY.Z))

            XR = BTuple.TupleConcat(NewPG.X, BTuple.TupleConcat(NewPX.X, NewPY.X))
            YR = BTuple.TupleConcat(NewPG.Y, BTuple.TupleConcat(NewPX.Y, NewPY.Y))
            ZR = BTuple.TupleConcat(NewPG.Z, BTuple.TupleConcat(NewPX.Z, NewPY.Z))
            hom_mat_3d_from_3d_3d_point_correspondence(XM, YM, ZM, XR, YR, ZR, HomMat3d)
        Else
            TransCoordSystem = False
            Exit Function
        End If
        TransCoordSystem = True
    End Function


    Public Function TransCoordSystemGX_ZHeimen(ByVal PG As Point3D, ByVal PX As Point3D, ByVal P1 As Point3D, ByVal P2 As Point3D, ByRef HomMat3d As Object) As Boolean
        Dim NewPG As New Point3D
        Dim NewPX As New Point3D
        Dim NewPY As New Point3D
        Dim PY As New Point3D

        Dim XM As New Object
        Dim YM As New Object
        Dim ZM As New Object
        Dim XR As New Object
        Dim YR As New Object
        Dim ZR As New Object
        Dim vecPGX As Point3D = PX.SubPoint3d(PG)
        Dim vecP12 As Point3D = P2.SubPoint3d(P1)
        Dim vecY As Point3D = vecPGX.VectorMult(vecP12)

        'Rep By Yamada Sta 20140617-----------------------------------------
        PY = vecY.AddPoint3d(PG)
        'If CalcCoordSystem(PG, PX, vecY, NewPG, NewPX, NewPY) Then
        'Rep By Yamada End 20140617-----------------------------------------
        If CalcCoordSystem(PG, PX, PY, NewPG, NewPX, NewPY) Then
            XM = BTuple.TupleConcat(PG.X, BTuple.TupleConcat(PX.X, PY.X))
            YM = BTuple.TupleConcat(PG.Y, BTuple.TupleConcat(PX.Y, PY.Y))
            ZM = BTuple.TupleConcat(PG.Z, BTuple.TupleConcat(PX.Z, PY.Z))

            XR = BTuple.TupleConcat(NewPG.X, BTuple.TupleConcat(NewPX.X, NewPY.X))
            YR = BTuple.TupleConcat(NewPG.Y, BTuple.TupleConcat(NewPX.Y, NewPY.Y))
            ZR = BTuple.TupleConcat(NewPG.Z, BTuple.TupleConcat(NewPX.Z, NewPY.Z))
            hom_mat_3d_from_3d_3d_point_correspondence(XM, YM, ZM, XR, YR, ZR, HomMat3d)
        Else
            TransCoordSystemGX_ZHeimen = False
            Exit Function
        End If
        TransCoordSystemGX_ZHeimen = True
    End Function

    Public Function TransCoordSystemGX_YHeimen(ByVal PG As Point3D, ByVal PX As Point3D, ByVal Pz1 As Point3D, ByVal Pz2 As Point3D, ByRef HomMat3d As Object) As Boolean

        Dim vecPGX As Point3D = PX.SubPoint3d(PG)
        Dim vecPz12 As Point3D = Pz2.SubPoint3d(Pz1)
        Dim vecY As Point3D = vecPz12.VectorMult(vecPGX)
        'Dim vecX As Point3D = vecY.VectorMult(vecPz12)

        ' Dim PY As Point3D = vecY.AddPoint3d(PG)
        'Dim Pxx As Point3D = PG.AddPoint3d(vecX)
        Dim PY As Point3D = PG.AddPoint3d(vecY)
        Dim NewPG As New Point3D
        Dim NewPX As New Point3D
        Dim NewPY As New Point3D
        If CalcCoordSystem(PG, PX, PY, NewPG, NewPX, NewPY) Then
            Dim XM As Object = BTuple.TupleConcat(PG.X, BTuple.TupleConcat(PX.X, PY.X))
            Dim YM As Object = BTuple.TupleConcat(PG.Y, BTuple.TupleConcat(PX.Y, PY.Y))
            Dim ZM As Object = BTuple.TupleConcat(PG.Z, BTuple.TupleConcat(PX.Z, PY.Z))

            Dim XR As Object = BTuple.TupleConcat(NewPG.X, BTuple.TupleConcat(NewPX.X, NewPY.X))
            Dim YR As Object = BTuple.TupleConcat(NewPG.Y, BTuple.TupleConcat(NewPX.Y, NewPY.Y))
            Dim ZR As Object = BTuple.TupleConcat(NewPG.Z, BTuple.TupleConcat(NewPX.Z, NewPY.Z))

            hom_mat_3d_from_3d_3d_point_correspondence(XM, YM, ZM, XR, YR, ZR, HomMat3d)

        Else
            TransCoordSystemGX_YHeimen = False
            Exit Function
        End If

        TransCoordSystemGX_YHeimen = True

    End Function



    'SUURI ADD 20141023 
    Public Function TransCoordSystemGX_YnormalVector(ByVal PG As Point3D, ByVal PX As Point3D, ByVal P1 As Point3D, ByVal P2 As Point3D, ByRef HomMat3d As Object) As Boolean
        Dim NewPG As New Point3D
        Dim NewPX As New Point3D
        Dim NewPY As New Point3D
        Dim PY As New Point3D

        Dim XM As New Object
        Dim YM As New Object
        Dim ZM As New Object
        Dim XR As New Object
        Dim YR As New Object
        Dim ZR As New Object

        Dim vecY_by_P12 As Point3D = P1.AddPoint3d(P2)

        If CalcCoordSystem(PG, PX, vecY_by_P12, NewPG, NewPX, NewPY) Then
            XM = BTuple.TupleConcat(PG.X, BTuple.TupleConcat(PX.X, PY.X))
            YM = BTuple.TupleConcat(PG.Y, BTuple.TupleConcat(PX.Y, PY.Y))
            ZM = BTuple.TupleConcat(PG.Z, BTuple.TupleConcat(PX.Z, PY.Z))

            XR = BTuple.TupleConcat(NewPG.X, BTuple.TupleConcat(NewPX.X, NewPY.X))
            YR = BTuple.TupleConcat(NewPG.Y, BTuple.TupleConcat(NewPX.Y, NewPY.Y))
            ZR = BTuple.TupleConcat(NewPG.Z, BTuple.TupleConcat(NewPX.Z, NewPY.Z))
            hom_mat_3d_from_3d_3d_point_correspondence(XM, YM, ZM, XR, YR, ZR, HomMat3d)
        Else
            TransCoordSystemGX_YnormalVector = False
            Exit Function
        End If
        TransCoordSystemGX_YnormalVector = True
    End Function


    Public Function TransCoordSystemGX_minusY(ByVal PG As Point3D, ByVal PX As Point3D, ByVal P1 As Point3D, ByVal P2 As Point3D, ByRef HomMat3d As Object) As Boolean
        Dim NewPG As New Point3D
        Dim NewPX As New Point3D
        Dim NewPY As New Point3D
        Dim PY As New Point3D

        Dim XM As New Object
        Dim YM As New Object
        Dim ZM As New Object
        Dim XR As New Object
        Dim YR As New Object
        Dim ZR As New Object
        Dim vecPGX As Point3D = PX.SubPoint3d(PG)
        Dim vecPGY As Point3D = P1.SubPoint3d(PG)
        Dim vecZ As Point3D = vecPGX.VectorMult(vecPGY)
        PY = vecPGX.VectorMult(vecZ)
        'Rep By Yamada Sta 20140617-----------------------------------------

        'If CalcCoordSystem(PG, PX, vecY, NewPG, NewPX, NewPY) Then
        'Rep By Yamada End 20140617-----------------------------------------
        If CalcCoordSystem(PG, PX, PY, NewPG, NewPX, NewPY) Then
            XM = BTuple.TupleConcat(PG.X, BTuple.TupleConcat(PX.X, PY.X))
            YM = BTuple.TupleConcat(PG.Y, BTuple.TupleConcat(PX.Y, PY.Y))
            ZM = BTuple.TupleConcat(PG.Z, BTuple.TupleConcat(PX.Z, PY.Z))

            XR = BTuple.TupleConcat(NewPG.X, BTuple.TupleConcat(NewPX.X, NewPY.X))
            YR = BTuple.TupleConcat(NewPG.Y, BTuple.TupleConcat(NewPX.Y, NewPY.Y))
            ZR = BTuple.TupleConcat(NewPG.Z, BTuple.TupleConcat(NewPX.Z, NewPY.Z))
            hom_mat_3d_from_3d_3d_point_correspondence(XM, YM, ZM, XR, YR, ZR, HomMat3d)
        Else
            TransCoordSystemGX_minusY = False
            Exit Function
        End If
        TransCoordSystemGX_minusY = True
    End Function


    Public Function CalcCoordSystem(ByVal PG As Point3D, ByVal PX As Point3D, ByVal PY As Point3D, _
                                    ByRef NewPG As Point3D, ByRef NewPX As Point3D, ByRef NewPY As Point3D) As Boolean
        Dim L As Double
        Dim R1 As Double
        Dim R2 As Double
        Dim Y_X As Double
        Dim Y_Y As Double
        Dim epiL As Double = 0.001
        PG.GetDisttoOtherPose(PX, L)
        PG.GetDisttoOtherPose(PY, R1)
        PX.GetDisttoOtherPose(PY, R2)
        Y_X = (R1 * R1 - R2 * R2 + L * L) / (2 * L)
        Y_Y = BTuple.TupleSqrt(R1 * R1 - Y_X * Y_X).D
        If Y_Y < epiL Or Y_Y < L * epiL Or L < epiL Then
            CalcCoordSystem = False
            Exit Function
        End If

        NewPG.X = BTuple.TupleGenConst(1, 0.0)
        NewPG.Y = BTuple.TupleGenConst(1, 0.0)
        NewPG.Z = BTuple.TupleGenConst(1, 0.0)

        NewPX.X = BTuple.TupleGenConst(1, L)
        NewPX.Y = BTuple.TupleGenConst(1, 0.0)
        NewPX.Z = BTuple.TupleGenConst(1, 0.0)

        NewPY.X = BTuple.TupleGenConst(1, Y_X)
        NewPY.Y = BTuple.TupleGenConst(1, Y_Y)
        NewPY.Z = BTuple.TupleGenConst(1, 0.0)
        CalcCoordSystem = True
    End Function

    Public Sub CalcRelPoseBetweenTwoPose(ByVal Pose1 As Object, ByVal Pose2 As Object, ByRef RelPose12 As Object)
        'Dim FilePath As String = "C:\temp\test111.txt"
        'Dim enc As System.Text.Encoding = System.Text.Encoding.GetEncoding("shift_jis")
        'Dim wrtLine As String = ""

        Dim homMat3d1 As Object = Nothing
        Dim homMat3d2 As Object = Nothing
        Dim homMat3dinvert As Object = Nothing
        Dim homMat3dcompose As Object = Nothing

        'wrtLine = "0000000 end"
        'System.IO.File.WriteAllText(FilePath, wrtLine, enc)

        HOperatorSet.PoseToHomMat3d(Pose1, homMat3d1)
        'wrtLine = "PoseToHomMat3d1 end"
        'System.IO.File.WriteAllText(FilePath, wrtLine, enc)

        HOperatorSet.PoseToHomMat3d(Pose2, homMat3d2)
        'wrtLine = "PoseToHomMat3d2 end"
        'System.IO.File.WriteAllText(FilePath, wrtLine, enc)

        HOperatorSet.HomMat3dInvert(homMat3d1, homMat3dinvert)
        'SUURI UPDATE START 20170628
        'HOperatorSet.HomMat3dCompose(homMat3d2, homMat3dinvert, homMat3dcompose)
        HOperatorSet.HomMat3dCompose(homMat3dinvert, homMat3d2, homMat3dcompose)
        'SUURI UPDATE END 20170628
        HOperatorSet.HomMat3dToPose(homMat3dcompose, RelPose12)
        'wrtLine = "HomMat3d end"
        'System.IO.File.WriteAllText(FilePath, wrtLine, enc)

        'Dim Kakunin As Object = Nothing
        'HOperatorSet.HomMat3dCompose(homMat3d1, homMat3dinvert, homMat3dcompose)
        ''wrtLine = "HomMat3d2 end"
        ''System.IO.File.WriteAllText(FilePath, wrtLine, enc)

        'HOperatorSet.HomMat3dToPose(homMat3dcompose, Kakunin)
    End Sub

    Public Sub CalcUnitPose(ByRef Pose1 As Object, ByRef Pose2 As Object)
        Dim dblBaseLine As Double
        Dim dblBaseLine_1 As Double

        Dim tplPoseScale As Object = Nothing

        dblBaseLine = BTuple.TupleSqrt(BTuple.TupleSum(BTuple.TupleFirstN(BTuple.TuplePow(Pose1, 2), 2))).D
        dblBaseLine_1 = 1 / dblBaseLine
        tplPoseScale = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat( _
                               BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(dblBaseLine_1, dblBaseLine_1), dblBaseLine_1), 1), 1), 1), 1)
        Pose1 = BTuple.TupleMult(Pose1, tplPoseScale)
        Pose2 = BTuple.TupleMult(Pose2, tplPoseScale)

    End Sub
    Public Sub CalcUnitPose(ByRef Pose1 As Object)
        Dim dblBaseLine As Double
        Dim dblBaseLine_1 As Double

        Dim tplPoseScale As Object = Nothing

        dblBaseLine = BTuple.TupleSqrt(BTuple.TupleSum(BTuple.TupleFirstN(BTuple.TuplePow(Pose1, 2), 2)))
        dblBaseLine_1 = 1 / dblBaseLine
        tplPoseScale = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat( _
                               BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(dblBaseLine_1, dblBaseLine_1), dblBaseLine_1), 1), 1), 1), 1)
        Pose1 = BTuple.TupleMult(Pose1, tplPoseScale)

    End Sub
    Public Function RunBA_PoseOnly(ByVal intF0 As Double, ByRef tmpCamParam As Object, ByRef Pose As Object, ByRef P2D As ImagePoints, ByRef P3d As Point3D, ByVal PointNum As Integer) As Double
        Dim objBA As New BAlib.BAmain
        Dim i As Integer
        RunBA_PoseOnly = Double.MaxValue
        objBA.SetInitData(tmpCamParam, 1, PointNum, intF0, 4, 1)
        i = 1

        Dim BA_Camdata As New BAlib.CameraData(Pose, objBA.objBAmain.CNP)

        objBA.objBAmain.Images(1) = BA_Camdata

        For i = 0 To PointNum - 1
            Dim BA_Points As New BAlib.Point3D(P3d.X(i), P3d.Y(i), P3d.Z(i))
            objBA.objBAmain.Points(i + 1) = BA_Points

            Dim BA_ImgPoints As New BAlib.Point2D(P2D.Col(i), P2D.Row(i))

            objBA.objBAmain.ImagePoints(i + 1, 1) = BA_ImgPoints
            objBA.objBAmain.Imat(i + 1, 1) = 1

        Next

        objBA.RunBA_PoseOnly()

        Dim strFile1 As String = My.Application.Info.DirectoryPath & "\ddEmat.csv"
        Dim strfile2 As String = My.Application.Info.DirectoryPath & "\dEmat.csv"
        'objBA.objBAmain.OutddEmat(strFile1)
        'objBA.objBAmain.OutdEmat(strfile2)

        Dim tmpPose As Object = objBA.objBAmain.Images(1).GenPoseBy_R_to_T
        Pose = tmpPose
        RunBA_PoseOnly = objBA.objBAmain.dblE

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

#End Region
#Region "OneCamera"
    ' Local procedures 
    Public Sub CalcCosAlpha(ByVal hv_Q1X As Object, ByVal hv_Q1Y As Object, ByVal hv_Q1Z As Object, _
        ByVal hv_Q2X As Object, ByVal hv_Q2Y As Object, ByVal hv_Q2Z As Object, ByRef hv_CosAlpha As Object)

        ' Local control variables 
        Dim hv_Result1 As Object = Nothing, hv_Result2 As Object = Nothing
        Dim hv_Result3 As Object = Nothing, hv_Sqrt2 As Object = Nothing
        Dim hv_Sqrt3 As Object = Nothing, hv_Result4 As Object = Nothing
        Dim hv_U12 As Object = Nothing, hv_CosAlpha1 As Object = Nothing

        ' Initialize local and output iconic variables 

        hv_Result1 = BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleMult(hv_Q1X, hv_Q2X), BTuple.TupleMult( _
            hv_Q1Y, hv_Q2Y)), BTuple.TupleMult(hv_Q1Z, hv_Q2Z))
        hv_Result2 = BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleMult(hv_Q1X, hv_Q1X), BTuple.TupleMult( _
            hv_Q1Y, hv_Q1Y)), BTuple.TupleMult(hv_Q1Z, hv_Q1Z))
        hv_Result3 = BTuple.TupleAdd(BTuple.TupleAdd(BTuple.TupleMult(hv_Q2X, hv_Q2X), BTuple.TupleMult( _
            hv_Q2Y, hv_Q2Y)), BTuple.TupleMult(hv_Q2Z, hv_Q2Z))
        HOperatorSet.TupleSqrt(hv_Result2, hv_Sqrt2)
        HOperatorSet.TupleSqrt(hv_Result3, hv_Sqrt3)
        hv_Result4 = BTuple.TupleMult(hv_Sqrt2, hv_Sqrt3)
        hv_CosAlpha = BTuple.TupleDiv(hv_Result1, hv_Result4)
        'U12 := pow(Q1X-Q2X,2)+pow(Q1Y-Q2Y,2)+pow(Q1Z-Q2Z,2)
        'CosAlpha1 := (Result2+Result3-U12)/(2*Sqrt2*Sqrt3)

        Exit Sub
    End Sub

    Public Sub CalcF(ByVal hv_L1 As Object, ByVal hv_L2 As Object, ByVal hv_CosA As Object, _
        ByVal hv_D As Object, ByRef hv_F As Object)
        ' Initialize local and output iconic variables 

        hv_F = BTuple.TupleSub(BTuple.TupleSub(BTuple.TupleAdd(BTuple.TuplePow(hv_L1, 2), BTuple.TuplePow( _
            hv_L2, 2)), BTuple.TupleMult(BTuple.TupleMult(BTuple.TupleMult(2, hv_L1), hv_L2), _
            hv_CosA)), BTuple.TuplePow(hv_D, 2))

        Exit Sub
    End Sub

    Public Sub CalFD(ByVal hv_L1 As Object, ByVal hv_L2 As Object, ByVal hv_CosA As Object, _
        ByRef hv_FD As Object)
        ' Initialize local and output iconic variables 

        hv_FD = BTuple.TupleSub(BTuple.TupleMult(2, hv_L1), BTuple.TupleMult(BTuple.TupleMult( _
            2, hv_L2), hv_CosA))


        Exit Sub
    End Sub

    Public Sub CalcL123(ByVal hv_CosA12 As Object, ByVal hv_CosA13 As Object, ByVal hv_CosA23 As Object, _
        ByVal hv_D12 As Object, ByVal hv_D13 As Object, ByVal hv_D23 As Object, ByVal hv_L1 As Object, _
        ByVal hv_L2 As Object, ByVal hv_L3 As Object, ByRef hv_LL1 As Object, ByRef hv_LL2 As Object, _
        ByRef hv_LL3 As Object)
        ' Stack for temporary control variables 
        Dim CTemp(10) As Object
        Dim SP_C As Long
        SP_C = 0

        ' Local control variables 
        Dim hv_Sum As Object = Nothing, hv_FD1 As Object = Nothing
        Dim hv_FD2 As Object = Nothing, hv_FD3 As Object = Nothing
        Dim hv_FD4 As Object = Nothing, hv_FD5 As Object = Nothing
        Dim hv_FD6 As Object = Nothing, hv_FD7 As Object = Nothing
        Dim hv_FD8 As Object = Nothing, hv_FD9 As Object = Nothing
        Dim hv_matFD As Object = Nothing, hv_FDMatrixID As Object = Nothing
        Dim hv_F1 As Object = Nothing, hv_F2 As Object = Nothing
        Dim hv_F3 As Object = Nothing, hv_matFT As Object = Nothing
        Dim hv_matF As Object = Nothing, hv_FMatrixID As Object = Nothing
        Dim hv_MatrixResultID As Object = Nothing, hv_DeltaLVal As Object = Nothing
        Dim hv_Abs As Object = Nothing

        ' Initialize local and output iconic variables 

        hv_Sum = 1000

        Do While BTuple.TupleGreater(hv_Sum, 0.00001)
            CalFD(hv_L1, hv_L2, hv_CosA12, hv_FD1)
            CalFD(hv_L2, hv_L1, hv_CosA12, hv_FD2)
            hv_FD3 = 0
            hv_FD4 = 0
            CalFD(hv_L2, hv_L3, hv_CosA23, hv_FD5)
            CalFD(hv_L3, hv_L2, hv_CosA23, hv_FD6)
            CalFD(hv_L1, hv_L3, hv_CosA13, hv_FD7)
            hv_FD8 = 0
            CalFD(hv_L3, hv_L1, hv_CosA13, hv_FD9)
            hv_matFD = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat( _
                BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat( _
                hv_FD1, hv_FD2), hv_FD3), hv_FD4), hv_FD5), hv_FD6), hv_FD7), hv_FD8), hv_FD9)
            HOperatorSet.CreateMatrix(3, 3, hv_matFD, hv_FDMatrixID)
            CalcF(hv_L1, hv_L2, hv_CosA12, hv_D12, hv_F1)
            CalcF(hv_L2, hv_L3, hv_CosA23, hv_D23, hv_F2)
            CalcF(hv_L1, hv_L3, hv_CosA13, hv_D13, hv_F3)
            hv_matFT = BTuple.TupleConcat(BTuple.TupleConcat(hv_F1, hv_F2), hv_F3)
            HOperatorSet.TupleMult(hv_matFT, -1, hv_matF)
            HOperatorSet.CreateMatrix(3, 1, hv_matF, hv_FMatrixID)
            HOperatorSet.SolveMatrix(hv_FDMatrixID, "general", 0, hv_FMatrixID, hv_MatrixResultID)
            HOperatorSet.GetFullMatrix(hv_MatrixResultID, hv_DeltaLVal)
            hv_L1 = BTuple.TupleAdd(hv_L1, BTuple.TupleSelect(hv_DeltaLVal, 0))
            hv_L2 = BTuple.TupleAdd(hv_L2, BTuple.TupleSelect(hv_DeltaLVal, 1))
            hv_L3 = BTuple.TupleAdd(hv_L3, BTuple.TupleSelect(hv_DeltaLVal, 2))
            HOperatorSet.TuplePow(hv_matFT, 2, hv_Abs)
            HOperatorSet.TupleSum(hv_Abs, hv_Sum)
            CTemp(SP_C) = hv_Sum
            SP_C = SP_C + 1
            HOperatorSet.TupleSqrt(CTemp(SP_C - 1), hv_Sum)
            SP_C = 0
            HOperatorSet.ClearMatrix(hv_FDMatrixID)
            HOperatorSet.ClearMatrix(hv_FMatrixID)
            HOperatorSet.ClearMatrix(hv_MatrixResultID)

        Loop

        hv_LL1 = hv_L1
        hv_LL2 = hv_L2
        hv_LL3 = hv_L3

        Exit Sub
    End Sub


    Public Sub CalcL1234(ByVal hv_CosA12 As Object, ByVal hv_CosA13 As Object, ByVal hv_CosA14 As Object, _
                         ByVal hv_CosA23 As Object, ByVal hv_CosA24 As Object, ByVal hv_CosA34 As Object, _
        ByVal hv_D12 As Object, ByVal hv_D13 As Object, ByVal hv_D14 As Object, _
        ByVal hv_D23 As Object, ByVal hv_D24 As Object, ByVal hv_D34 As Object, _
        ByVal hv_L1 As Object, ByVal hv_L2 As Object, ByVal hv_L3 As Object, ByVal hv_L4 As Object, _
        ByRef hv_LL1 As Object, ByRef hv_LL2 As Object, ByRef hv_LL3 As Object, ByRef hv_LL4 As Object)
        ' Stack for temporary control variables 
        Dim CTemp(10) As Object
        Dim SP_C As Long
        SP_C = 0

        ' Local control variables 
        Dim hv_Sum As Object = Nothing, hv_FD1 As Object = Nothing
        Dim hv_FD2 As Object = Nothing, hv_FD3 As Object = Nothing
        Dim hv_FD4 As Object = Nothing, hv_FD5 As Object = Nothing
        Dim hv_FD6 As Object = Nothing, hv_FD7 As Object = Nothing
        Dim hv_FD8 As Object = Nothing, hv_FD9 As Object = Nothing
        Dim hv_FD10 As Object = Nothing, hv_FD11 As Object = Nothing
        Dim hv_FD12 As Object = Nothing, hv_FD13 As Object = Nothing
        Dim hv_FD14 As Object = Nothing, hv_FD15 As Object = Nothing
        Dim hv_FD16 As Object = Nothing, hv_FD17 As Object = Nothing
        Dim hv_FD18 As Object = Nothing, hv_FD19 As Object = Nothing
        Dim hv_FD20 As Object = Nothing, hv_FD21 As Object = Nothing
        Dim hv_FD22 As Object = Nothing, hv_FD23 As Object = Nothing
        Dim hv_FD24 As Object = Nothing
        Dim hv_FDMatrixID As Object = Nothing
        Dim hv_F1 As Object = Nothing, hv_F2 As Object = Nothing
        Dim hv_F3 As Object = Nothing, hv_F4 As Object = Nothing
        Dim hv_F5 As Object = Nothing, hv_F6 As Object = Nothing
        ' Dim hv_matFT As Object = Nothing
        Dim hv_matF As Object = Nothing, hv_FMatrixID As Object = Nothing
        Dim hv_MatrixResultID As Object = Nothing, hv_DeltaLVal As Object = Nothing
        Dim hv_Abs As Object = Nothing

        ' Initialize local and output iconic variables 

        hv_Sum = 1000

        Do While BTuple.TupleGreater(hv_Sum, 0.0001)
            'F1
            CalFD(hv_L1, hv_L2, hv_CosA12, hv_FD1)
            CalFD(hv_L2, hv_L1, hv_CosA12, hv_FD2)
            hv_FD3 = 0
            hv_FD4 = 0
            'F2
            hv_FD5 = 0
            CalFD(hv_L2, hv_L3, hv_CosA23, hv_FD6)
            CalFD(hv_L3, hv_L2, hv_CosA23, hv_FD7)
            hv_FD8 = 0
            'F3
            CalFD(hv_L1, hv_L3, hv_CosA13, hv_FD9)
            hv_FD10 = 0
            CalFD(hv_L3, hv_L1, hv_CosA13, hv_FD11)
            hv_FD12 = 0
            'F4
            CalFD(hv_L1, hv_L4, hv_CosA14, hv_FD13)
            hv_FD14 = 0
            hv_FD15 = 0
            CalFD(hv_L4, hv_L1, hv_CosA14, hv_FD16)
            'F5
            hv_FD17 = 0
            CalFD(hv_L2, hv_L4, hv_CosA24, hv_FD18)
            hv_FD19 = 0
            CalFD(hv_L4, hv_L2, hv_CosA24, hv_FD20)
            'F6
            hv_FD21 = 0
            hv_FD22 = 0
            CalFD(hv_L3, hv_L4, hv_CosA34, hv_FD23)
            CalFD(hv_L4, hv_L3, hv_CosA34, hv_FD24)
            Dim hv_matFD() As Double = {hv_FD1, hv_FD2, hv_FD3, hv_FD4, _
                                        hv_FD5, hv_FD6, hv_FD7, hv_FD8, _
                                        hv_FD9, hv_FD10, hv_FD11, hv_FD12, _
                                        hv_FD13, hv_FD14, hv_FD15, hv_FD16, _
                                        hv_FD17, hv_FD18, hv_FD19, hv_FD20, _
                                        hv_FD21, hv_FD22, hv_FD23, hv_FD24}

            'hv_matFD = BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat( _
            '    BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat(BTuple.TupleConcat( _
            '    hv_FD1, hv_FD2), hv_FD3), hv_FD4), hv_FD5), hv_FD6), hv_FD7), hv_FD8), hv_FD9)
            HOperatorSet.CreateMatrix(6, 4, hv_matFD, hv_FDMatrixID)
            CalcF(hv_L1, hv_L2, hv_CosA12, hv_D12, hv_F1)
            CalcF(hv_L2, hv_L3, hv_CosA23, hv_D23, hv_F2)
            CalcF(hv_L1, hv_L3, hv_CosA13, hv_D13, hv_F3)
            CalcF(hv_L1, hv_L4, hv_CosA14, hv_D14, hv_F4)
            CalcF(hv_L2, hv_L4, hv_CosA24, hv_D24, hv_F5)
            CalcF(hv_L3, hv_L4, hv_CosA34, hv_D34, hv_F6)
            Dim hv_matFT() As Double = {hv_F1, hv_F2, hv_F3, hv_F4, _
                                       hv_F5, hv_F6}
            '  hv_matFT = BTuple.TupleConcat(BTuple.TupleConcat(hv_F1, hv_F2), hv_F3)
            HOperatorSet.TupleMult(hv_matFT, -1, hv_matF)
            HOperatorSet.CreateMatrix(6, 1, hv_matF, hv_FMatrixID)
            HOperatorSet.SolveMatrix(hv_FDMatrixID, "general", 0.00000000000000022204, hv_FMatrixID, hv_MatrixResultID)
            HOperatorSet.GetFullMatrix(hv_MatrixResultID, hv_DeltaLVal)
            hv_L1 = BTuple.TupleAdd(hv_L1, BTuple.TupleSelect(hv_DeltaLVal, 0))
            hv_L2 = BTuple.TupleAdd(hv_L2, BTuple.TupleSelect(hv_DeltaLVal, 1))
            hv_L3 = BTuple.TupleAdd(hv_L3, BTuple.TupleSelect(hv_DeltaLVal, 2))
            hv_L4 = BTuple.TupleAdd(hv_L4, BTuple.TupleSelect(hv_DeltaLVal, 3))
            HOperatorSet.TuplePow(hv_matFT, 2, hv_Abs)
            HOperatorSet.TupleSum(hv_Abs, hv_Sum)
            CTemp(SP_C) = hv_Sum
            SP_C = SP_C + 1
            HOperatorSet.TupleSqrt(CTemp(SP_C - 1), hv_Sum)
            SP_C = 0
            HOperatorSet.ClearMatrix(hv_FDMatrixID)
            HOperatorSet.ClearMatrix(hv_FMatrixID)
            HOperatorSet.ClearMatrix(hv_MatrixResultID)

        Loop

        hv_LL1 = hv_L1
        hv_LL2 = hv_L2
        hv_LL3 = hv_L3
        hv_LL4 = hv_L4

        Exit Sub
    End Sub
    Public Sub CalcOneCamera3P(ByVal hv_R1 As Object, ByVal hv_C1 As Object, ByVal hv_R2 As Object, _
        ByVal hv_C2 As Object, ByVal hv_R3 As Object, ByVal hv_C3 As Object, ByVal hv_CameraParam As Object, _
        ByVal hv_D12 As Object, ByVal hv_D13 As Object, ByVal hv_D23 As Object, ByRef hv_X1 As Object, _
        ByRef hv_Y1 As Object, ByRef hv_Z1 As Object, ByRef hv_X2 As Object, ByRef hv_Y2 As Object, _
        ByRef hv_Z2 As Object, ByRef hv_X3 As Object, ByRef hv_Y3 As Object, ByRef hv_Z3 As Object)

        ' Local control variables 
        Dim hv_PX As Object = Nothing, hv_PY As Object = Nothing
        Dim hv_PZ As Object = Nothing, hv_Q1X As Object = Nothing
        Dim hv_Q1Y As Object = Nothing, hv_Q1Z As Object = Nothing
        Dim hv_Q2X As Object = Nothing, hv_Q2Y As Object = Nothing
        Dim hv_Q2Z As Object = Nothing, hv_Q3X As Object = Nothing
        Dim hv_Q3Y As Object = Nothing, hv_Q3Z As Object = Nothing
        Dim hv_CosAlpha12 As Object = Nothing, hv_CosAlpha13 As Object = Nothing
        Dim hv_CosAlpha23 As Object = Nothing, hv_L As Object = Nothing
        Dim hv_L1 As Object = Nothing, hv_L2 As Object = Nothing
        Dim hv_L3 As Object = Nothing, hv_LL1 As Object = Nothing
        Dim hv_LL2 As Object = Nothing, hv_LL3 As Object = Nothing
        Dim hv_Longitude As Object = Nothing, hv_Latitude As Object = Nothing
        Dim hv_Radius As Object = Nothing

        ' Initialize local and output iconic variables 

        HOperatorSet.GetLineOfSight(hv_R1, hv_C1, hv_CameraParam, hv_PX, hv_PY, hv_PZ, hv_Q1X, _
            hv_Q1Y, hv_Q1Z)
        HOperatorSet.GetLineOfSight(hv_R2, hv_C2, hv_CameraParam, hv_PX, hv_PY, hv_PZ, hv_Q2X, _
            hv_Q2Y, hv_Q2Z)
        HOperatorSet.GetLineOfSight(hv_R3, hv_C3, hv_CameraParam, hv_PX, hv_PY, hv_PZ, hv_Q3X, _
            hv_Q3Y, hv_Q3Z)
        CalcCosAlpha(hv_Q1X, hv_Q1Y, hv_Q1Z, hv_Q2X, hv_Q2Y, hv_Q2Z, hv_CosAlpha12)
        CalcCosAlpha(hv_Q1X, hv_Q1Y, hv_Q1Z, hv_Q3X, hv_Q3Y, hv_Q3Z, hv_CosAlpha13)
        CalcCosAlpha(hv_Q2X, hv_Q2Y, hv_Q2Z, hv_Q3X, hv_Q3Y, hv_Q3Z, hv_CosAlpha23)
        'L1,L2,L3の初期値を計算
        hv_L = BTuple.TupleMult(hv_D13, BTuple.TupleSqrt(BTuple.TupleDiv(1, BTuple.TupleMult( _
            2, BTuple.TupleSub(1, hv_CosAlpha13)))))
        hv_L1 = hv_L
        hv_L2 = hv_L
        hv_L3 = hv_L
        CalcL123(hv_CosAlpha12, hv_CosAlpha13, hv_CosAlpha23, hv_D12, hv_D13, hv_D23, _
            hv_L1, hv_L2, hv_L3, hv_LL1, hv_LL2, hv_LL3)
        HOperatorSet.ConvertPoint3dCartToSpher(hv_Q1X, hv_Q1Y, hv_Q1Z, "-y", "-z", hv_Longitude, _
            hv_Latitude, hv_Radius)
        HOperatorSet.ConvertPoint3dSpherToCart(hv_Longitude, hv_Latitude, hv_LL1, "-y", "-z", hv_X1, _
            hv_Y1, hv_Z1)
        HOperatorSet.ConvertPoint3dCartToSpher(hv_Q2X, hv_Q2Y, hv_Q2Z, "-y", "-z", hv_Longitude, _
            hv_Latitude, hv_Radius)
        HOperatorSet.ConvertPoint3dSpherToCart(hv_Longitude, hv_Latitude, hv_LL2, "-y", "-z", hv_X2, _
            hv_Y2, hv_Z2)
        HOperatorSet.ConvertPoint3dCartToSpher(hv_Q3X, hv_Q3Y, hv_Q3Z, "-y", "-z", hv_Longitude, _
            hv_Latitude, hv_Radius)
        HOperatorSet.ConvertPoint3dSpherToCart(hv_Longitude, hv_Latitude, hv_LL3, "-y", "-z", hv_X3, _
            hv_Y3, hv_Z3)

        Exit Sub
    End Sub

    Public Sub CalcOneCamera4P(ByVal hv_R1 As Object, ByVal hv_C1 As Object, ByVal hv_R2 As Object, _
      ByVal hv_C2 As Object, ByVal hv_R3 As Object, ByVal hv_C3 As Object, ByVal hv_R4 As Object, ByVal hv_C4 As Object, _
      ByVal hv_CameraParam As Object, _
      ByVal hv_D12 As Object, ByVal hv_D13 As Object, ByVal hv_D14 As Object, _
      ByVal hv_D23 As Object, ByVal hv_D24 As Object, ByVal hv_D34 As Object, _
      ByRef hv_X1 As Object, _
      ByRef hv_Y1 As Object, ByRef hv_Z1 As Object, ByRef hv_X2 As Object, ByRef hv_Y2 As Object, _
      ByRef hv_Z2 As Object, ByRef hv_X3 As Object, ByRef hv_Y3 As Object, ByRef hv_Z3 As Object, _
      ByRef hv_X4 As Object, ByRef hv_Y4 As Object, ByRef hv_Z4 As Object)

        ' Local control variables 
        Dim hv_PX As Object = Nothing, hv_PY As Object = Nothing
        Dim hv_PZ As Object = Nothing, hv_Q1X As Object = Nothing
        Dim hv_Q1Y As Object = Nothing, hv_Q1Z As Object = Nothing
        Dim hv_Q2X As Object = Nothing, hv_Q2Y As Object = Nothing
        Dim hv_Q2Z As Object = Nothing, hv_Q3X As Object = Nothing
        Dim hv_Q3Y As Object = Nothing, hv_Q3Z As Object = Nothing
        Dim hv_Q4X As Object = Nothing
        Dim hv_Q4Y As Object = Nothing, hv_Q4Z As Object = Nothing
        Dim hv_CosAlpha12 As Object = Nothing, hv_CosAlpha13 As Object = Nothing
        Dim hv_CosAlpha14 As Object = Nothing
        Dim hv_CosAlpha23 As Object = Nothing, hv_CosAlpha24 As Object = Nothing
        Dim hv_CosAlpha34 As Object = Nothing
        Dim hv_L As Object = Nothing
        Dim hv_L1 As Object = Nothing, hv_L2 As Object = Nothing
        Dim hv_L3 As Object = Nothing, hv_L4 As Object = Nothing
        Dim hv_LL1 As Object = Nothing, hv_LL2 As Object = Nothing
        Dim hv_LL3 As Object = Nothing, hv_LL4 As Object = Nothing
        Dim hv_Longitude As Object = Nothing, hv_Latitude As Object = Nothing
        Dim hv_Radius As Object = Nothing

        ' Initialize local and output iconic variables 

        HOperatorSet.GetLineOfSight(hv_R1, hv_C1, hv_CameraParam, hv_PX, hv_PY, hv_PZ, hv_Q1X, hv_Q1Y, hv_Q1Z)
        HOperatorSet.GetLineOfSight(hv_R2, hv_C2, hv_CameraParam, hv_PX, hv_PY, hv_PZ, hv_Q2X, hv_Q2Y, hv_Q2Z)
        HOperatorSet.GetLineOfSight(hv_R3, hv_C3, hv_CameraParam, hv_PX, hv_PY, hv_PZ, hv_Q3X, hv_Q3Y, hv_Q3Z)
        HOperatorSet.GetLineOfSight(hv_R4, hv_C4, hv_CameraParam, hv_PX, hv_PY, hv_PZ, hv_Q4X, hv_Q4Y, hv_Q4Z)
        CalcCosAlpha(hv_Q1X, hv_Q1Y, hv_Q1Z, hv_Q2X, hv_Q2Y, hv_Q2Z, hv_CosAlpha12)
        CalcCosAlpha(hv_Q1X, hv_Q1Y, hv_Q1Z, hv_Q3X, hv_Q3Y, hv_Q3Z, hv_CosAlpha13)
        CalcCosAlpha(hv_Q1X, hv_Q1Y, hv_Q1Z, hv_Q4X, hv_Q4Y, hv_Q4Z, hv_CosAlpha14)
        CalcCosAlpha(hv_Q2X, hv_Q2Y, hv_Q2Z, hv_Q3X, hv_Q3Y, hv_Q3Z, hv_CosAlpha23)
        CalcCosAlpha(hv_Q2X, hv_Q2Y, hv_Q2Z, hv_Q4X, hv_Q4Y, hv_Q4Z, hv_CosAlpha24)
        CalcCosAlpha(hv_Q3X, hv_Q3Y, hv_Q3Z, hv_Q4X, hv_Q4Y, hv_Q4Z, hv_CosAlpha34)
        'L1,L2,L3の初期値を計算
        hv_L = BTuple.TupleMult(hv_D13, BTuple.TupleSqrt(BTuple.TupleDiv(1, BTuple.TupleMult( _
            2, BTuple.TupleSub(1, hv_CosAlpha13)))))
        hv_L1 = hv_L
        hv_L2 = hv_L
        hv_L3 = hv_L
        hv_L4 = hv_L

        CalcL1234(hv_CosAlpha12, hv_CosAlpha13, hv_CosAlpha14, hv_CosAlpha23, hv_CosAlpha24, hv_CosAlpha34, _
                  hv_D12, hv_D13, hv_D14, hv_D23, hv_D24, hv_D34, _
            hv_L1, hv_L2, hv_L3, hv_L4, hv_LL1, hv_LL2, hv_LL3, hv_LL4)
        HOperatorSet.ConvertPoint3dCartToSpher(hv_Q1X, hv_Q1Y, hv_Q1Z, "-y", "-z", hv_Longitude, hv_Latitude, hv_Radius)
        HOperatorSet.ConvertPoint3dSpherToCart(hv_Longitude, hv_Latitude, hv_LL1, "-y", "-z", hv_X1, hv_Y1, hv_Z1)
        HOperatorSet.ConvertPoint3dCartToSpher(hv_Q2X, hv_Q2Y, hv_Q2Z, "-y", "-z", hv_Longitude, hv_Latitude, hv_Radius)
        HOperatorSet.ConvertPoint3dSpherToCart(hv_Longitude, hv_Latitude, hv_LL2, "-y", "-z", hv_X2, hv_Y2, hv_Z2)
        HOperatorSet.ConvertPoint3dCartToSpher(hv_Q3X, hv_Q3Y, hv_Q3Z, "-y", "-z", hv_Longitude, hv_Latitude, hv_Radius)
        HOperatorSet.ConvertPoint3dSpherToCart(hv_Longitude, hv_Latitude, hv_LL3, "-y", "-z", hv_X3, hv_Y3, hv_Z3)
        HOperatorSet.ConvertPoint3dCartToSpher(hv_Q4X, hv_Q4Y, hv_Q4Z, "-y", "-z", hv_Longitude, hv_Latitude, hv_Radius)
        HOperatorSet.ConvertPoint3dSpherToCart(hv_Longitude, hv_Latitude, hv_LL4, "-y", "-z", hv_X4, hv_Y4, hv_Z4)

        Exit Sub
    End Sub

#End Region
    Public Function CalcSelegmel(ByVal N As Integer, ByVal M As Integer) As Integer
        CalcSelegmel = CalcFactor(N) / (CalcFactor(N - M) * CalcFactor(M))

    End Function
    Private Function CalcFactor(ByVal N As Integer) As Integer
        Dim i As Integer
        CalcFactor = 1
        For i = 1 To N
            CalcFactor = CalcFactor * i
        Next
    End Function


    Public Sub ClearHObject(ByRef obj As HObject)
        If obj Is Nothing Then
            Exit Sub
        End If
        Try
            'HOperatorSet.ClearObj(obj)
            obj.Dispose()
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

#Region "タイムモニター用"

    Public TimeMonLogPath As String = My.Application.Info.DirectoryPath & "\log.log"
    Public Sub TimeMonStart()

        Using w As System.IO.StreamWriter = System.IO.File.AppendText(TimeMonLogPath)
            w.WriteLine("START;{0};{1};{2}",
                        (New System.Diagnostics.StackTrace).GetFrame(1).GetMethod.Name,
                        (New System.Diagnostics.StackTrace).GetFrames().Count,
                        (DateTime.Now - New DateTime(2000, 1, 1)).TotalMilliseconds)
        End Using
    End Sub

    Public Sub TimeMonEnd()
        Using w As System.IO.StreamWriter = System.IO.File.AppendText(TimeMonLogPath)
            w.WriteLine("END;{0};{1};{2}",
                        (New System.Diagnostics.StackTrace).GetFrame(1).GetMethod.Name,
                        (New System.Diagnostics.StackTrace).GetFrames().Count,
                        (DateTime.Now - New DateTime(2000, 1, 1)).TotalMilliseconds)
        End Using
    End Sub


    Public Sub TimeMonOut(ByVal strOutPath As String)
        Dim lines As List(Of String) = New List(Of String)
        Dim file As System.IO.StreamReader = My.Computer.FileSystem.OpenTextFileReader(TimeMonLogPath)
        Try
            While file.Peek() >= 0
                Dim line As String = file.ReadLine()
                lines.Add(line)
            End While
        Finally
            file.Close()
        End Try
        recursiveWrite(strOutPath, lines, 0, 0)
        ' My.Computer.FileSystem.DeleteFile(TimeMonLogPath)
    End Sub

    Private Sub recursiveWrite(ByRef logFilePath As String, ByRef lines As List(Of String), start As Integer, intend As Integer)
        For i As Integer = start To lines.Count - 1
            Dim svars As [String]() = lines(i).Split(";"c)
            If svars(0).Equals("START") Then
                For k As Integer = i To lines.Count - 1
                    Dim evars As [String]() = lines(k).Split(";"c)
                    If evars(0).Equals("END") AndAlso evars(1).Equals(svars(1)) AndAlso evars(2).Equals(svars(2)) Then
                        Dim printLine As [String] = ""
                        For j As Integer = 0 To intend - 1
                            printLine += vbTab
                        Next
                        Dim elapsed As [Double] = [Double].Parse(evars(3)) - [Double].Parse(svars(3))
                        Dim elapsedTime As TimeSpan = TimeSpan.FromMilliseconds(elapsed)
                        printLine += evars(1) + " : " + elapsedTime.TotalSeconds.ToString() + " seconds"
                        Using w As System.IO.StreamWriter = System.IO.File.AppendText(logFilePath)
                            w.WriteLine(printLine)
                        End Using
                        recursiveWrite(logFilePath, lines, i + 1, intend + 1)
                        Return
                    End If
                Next
            ElseIf svars(0).Equals("END") Then
                intend -= 1
            End If
        Next
    End Sub
#End Region
End Module
