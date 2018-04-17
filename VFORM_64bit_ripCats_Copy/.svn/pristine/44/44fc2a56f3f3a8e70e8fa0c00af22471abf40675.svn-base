Imports FBMlib

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports YCM



'''<summary>
'''YCM_SunpoTest のテスト クラスです。すべての
'''YCM_SunpoTest 単体テストをここに含めます

'''</summary>
<TestClass()> _
Public Class YCM_SunpoTest


    Private testContextInstance As TestContext

    '''<summary>
    '''現在のテストの実行についての情報および機能を

    '''提供するテスト コンテキストを取得または設定します。

    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(value As TestContext)
            testContextInstance = value
        End Set
    End Property

#Region "追加のテスト属性"
    '
    'テストを作成するときに、次の追加属性を使用することができます:
    '
    'クラスの最初のテストを実行する前にコードを実行するには、ClassInitialize を使用
    <ClassInitialize()> _
    Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)

        'YCM.MainFrm = New YCM_MainFrame
        'Dim strPath As String = "C:\Temp\Test_Sunpo"

        'Dim strCamParam As String = "C:\121126\YCM\bin\Debug\計測システムフォルダ\CamParam\camparamD300_blue_Large_SxConst.cal"
        'YCM.MainFrm.objFBM = New FBMlib.FeatureImage(strCamParam)

        'YCM.MainFrm.objFBM.ProjectPath(False) = strPath
        ''Me.Text = Me.Text & "   " & strPath
        'YCM.MainFrm.objFBM.ReadProjectData()

        'YCM.YCM_Offset_GenData()

    End Sub
    '
    'クラスのすべてのテストを実行した後にコードを実行するには、ClassCleanup を使用
    '<ClassCleanup()>  _
    'Public Shared Sub MyClassCleanup()
    'End Sub
    '
    '各テストを実行する前にコードを実行するには、TestInitialize を使用
    '<TestInitialize()>  _
    'Public Sub MyTestInitialize()
    'End Sub
    '
    '各テストを実行した後にコードを実行するには、TestCleanup を使用
    '<TestCleanup()>  _
    'Public Sub MyTestCleanup()
    'End Sub
    '
#End Region
    'H25.5.30以下は入力値がNothing等の場合のテスト============================================================
    '↓↓↓

    '''<summary>
    '''GetSorted のテスト1
    '''入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetSortedTest_1()
        Dim NonSortedList() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim XYZ As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim SortHouhou As SortMethod = New SortMethod() ' TODO: 適切な値に初期化してください
        Dim SortedList() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim SortedListExpected() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim Basepoint As New Point3D

        YCM_Sunpo.GetSorted(NonSortedList, XYZ, SortHouhou, Basepoint, SortedList)
        Assert.AreEqual(SortedListExpected, SortedList)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''GetSorted のテスト2
    '''入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetSortedTest_2()
        Dim SortedList() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim SortedListExpected() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim XYZ As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim SortHouhou As SortMethod = New SortMethod() ' TODO: 適切な値に初期化してください
        Dim Basepoint As New Point3D

        YCM_Sunpo.GetSorted(SortedList, XYZ, SortHouhou, Basepoint)
        Assert.AreEqual(SortedListExpected, SortedList)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub
    '''<summary>
    '''GetAreaPointGroup のテスト3
    '''入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetAreaPointGroupTest3()
        Dim PointGroup() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim XYZ As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim BasePoint As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim DaiSyou As DaiSyou = New DaiSyou() ' TODO: 適切な値に初期化してください
        Dim FiltPointGroup() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim FiltPointGroupExpected() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        YCM_Sunpo.GetAreaPointGroup(PointGroup, XYZ, BasePoint, DaiSyou, FiltPointGroup)

        Assert.AreEqual(FiltPointGroupExpected, FiltPointGroup)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''GetAreaPointGroup のテスト4
    '''入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetAreaPointGroupTest4()
        Dim PointGroup() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim XYZ As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim BaseID As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim DaiSyou As DaiSyou = New DaiSyou() ' TODO: 適切な値に初期化してください
        Dim FiltPointGroup() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim FiltPointGroupExpected() As CT_Data = Nothing ' TODO: 適切な値に初期化してください

        YCM_Sunpo.GetAreaPointGroup(PointGroup, XYZ, BaseID, DaiSyou, FiltPointGroup)
        Assert.AreEqual(FiltPointGroupExpected, FiltPointGroup)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''GetAreaPointGroup のテスト1
    '''入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetAreaPointGroupTest1()
        Dim PointGroup() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim PointGroupExpected() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim XYZ As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim BasePoint As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim DaiSyou As DaiSyou = New DaiSyou() ' TODO: 適切な値に初期化してください
        YCM_Sunpo.GetAreaPointGroup(PointGroup, XYZ, BasePoint, DaiSyou)

        Assert.AreEqual(PointGroupExpected, PointGroup)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''GetAreaPointGroup のテスト2
    '''入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetAreaPointGroupTest2()
        Dim PointGroup() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim PointGroupExpected() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim XYZ As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim BaseID As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim DaiSyou As DaiSyou = New DaiSyou() ' TODO: 適切な値に初期化してください
        YCM_Sunpo.GetAreaPointGroup(PointGroup, XYZ, BaseID, DaiSyou)

        Assert.AreEqual(PointGroupExpected, PointGroup)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''KinjiPlane_PointGroup のテスト3
    '''入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_KinjiPlane_PointGroup_Test3()
        Dim arrPoint() As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim KinjiPlane As GeoPlane = Nothing ' TODO: 適切な値に初期化してください
        Dim KinjiPlaneExpected As GeoPlane = Nothing ' TODO: 適切な値に初期化してください

        YCM_Sunpo.KinjiPlane_PointGroup(arrPoint, KinjiPlane)
        Assert.AreEqual(KinjiPlaneExpected, KinjiPlane)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''ProjectPointGroup のテスト

    '''入力値がNothingだった場合 
    '''</summary>
    <TestMethod()> _
    Public Sub N_ProjectPointGroupTest()
        Dim arrPoint() As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim Plane As GeoPlane = Nothing ' TODO: 適切な値に初期化してください
        Dim ProjectPntG() As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim ProjectPntGExpected() As Point3D = Nothing ' TODO: 適切な値に初期化してください

        YCM_Sunpo.ProjectPointGroup(arrPoint, Plane, ProjectPntG)
        Assert.AreEqual(ProjectPntGExpected, ProjectPntG)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''GetPoint_ByIndex のテスト

    '''入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetPoint_ByIndexTest()
        Dim arrCT() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim Index1 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim Index2 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim arrCT_Result() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim arrCT_ResultExpected() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        YCM_Sunpo.GetPoint_ByIndex(arrCT, Index1, Index2, arrCT_Result)
        Assert.AreEqual(arrCT_ResultExpected, arrCT_Result)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''GetPoint_ByKubun のテスト

    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetPoint_ByKubunTest()
        Dim arrCT() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim K1 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim K2 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim arrCT_Result() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim arrCT_ResultExpected() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        YCM_Sunpo.GetPoint_ByKubun(arrCT, K1, K2, arrCT_Result)
        Assert.AreEqual(arrCT_ResultExpected, arrCT_Result)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''GetPoint_CTID のテスト

    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetPoint_CTIDTest()
        Dim ID As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim expected As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim actual As Point3D
        actual = YCM_Sunpo.GetPoint_CTID(ID)
        Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("このテストメソッドの正確性を確認します。")
    End Sub

    '''<summary>
    '''GetPoint_CTID_NoUnit のテスト

    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetPoint_CTID_NoUnitTest()
        Dim ID As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim expected As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim actual As Point3D
        actual = YCM_Sunpo.GetPoint_CTID_NoUnit(ID)
        Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("このテストメソッドの正確性を確認します。")
    End Sub

    '''<summary>
    '''GetPointIntersection のテスト

    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetPointIntersectionTest()
        Dim ID1 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim ID2 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim ID3 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim XYZ As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim P As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim PExpected As Point3D = Nothing ' TODO: 適切な値に初期化してください
        YCM_Sunpo.GetPointIntersection(ID1, ID2, ID3, XYZ, P)
        Assert.AreEqual(PExpected, P)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''GetPointIntersection のテスト

    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetPointIntersectionTest1()
        Dim P1 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim P2 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim P3 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim XYZ As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim P As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim PExpected As Point3D = Nothing ' TODO: 適切な値に初期化してください

        YCM_Sunpo.GetPointIntersection(P1, P2, P3, XYZ, P)
        Assert.AreEqual(PExpected, P)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''GetPoint のテスト

    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetPointTest()
        Dim P1 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim XYZ1 As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim P2 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim XYZ2 As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim P3 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim XYZ3 As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim expected As New Point3D ' TODO: 適切な値に初期化してください
        Dim actual As Point3D
        actual = YCM_Sunpo.GetPoint(P1, XYZ1, P2, XYZ2, P3, XYZ3)

        Assert.AreEqual(expected.X, actual.X)
        Assert.AreEqual(expected.Y, actual.Y)
        Assert.AreEqual(expected.Z, actual.Z)
        'Assert.Inconclusive("このテストメソッドの正確性を確認します。")
    End Sub

    '''<summary>
    '''GetPoint のテスト

    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetPointTest1()
        Dim ID1 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim XYZ1 As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim ID2 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim XYZ2 As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim ID3 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim XYZ3 As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim expected As New Point3D  ' TODO: 適切な値に初期化してください
        Dim actual As Point3D

        actual = YCM_Sunpo.GetPoint(ID1, XYZ1, ID2, XYZ2, ID3, XYZ3)
        Assert.AreEqual(expected.X, actual.X)
        Assert.AreEqual(expected.Y, actual.Y)
        Assert.AreEqual(expected.Z, actual.Z)
        'Assert.Inconclusive("このテストメソッドの正確性を確認します。")
    End Sub

    '''<summary>
    '''GetMinMaxPointCToC のテスト

    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetMinMaxPointCToCTest()
        Dim C1 As GeoCurve = Nothing ' TODO: 適切な値に初期化してください
        Dim C2 As GeoCurve = Nothing ' TODO: 適切な値に初期化してください
        Dim minPC1 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim minPC1Expected As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim maxPC1 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim maxPC1Expected As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim minPC2 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim minPC2Expected As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim maxPC2 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim maxPC2Expected As Point3D = Nothing ' TODO: 適切な値に初期化してください

        YCM_Sunpo.GetMinMaxPointCToC(C1, C2, minPC1, maxPC1, minPC2, maxPC2)
        Assert.AreEqual(minPC1Expected, minPC1)
        Assert.AreEqual(maxPC1Expected, maxPC1)
        Assert.AreEqual(minPC2Expected, minPC2)
        Assert.AreEqual(maxPC2Expected, maxPC2)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''GetDistCircletoPlane のテスト

    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetDistCircletoPlaneTest()
        Dim Circle As GeoCurve = Nothing ' TODO: 適切な値に初期化してください
        Dim Plane As GeoPlane = Nothing ' TODO: 適切な値に初期化してください
        Dim XYZ As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim DistMax As Double = 0.0! ' TODO: 適切な値に初期化してください
        Dim DistMaxExpected As Double = 0.0! ' TODO: 適切な値に初期化してください
        Dim DistMin As Double = 0.0! ' TODO: 適切な値に初期化してください
        Dim DistMinExpected As Double = 0.0! ' TODO: 適切な値に初期化してください


        YCM_Sunpo.GetDistCircletoPlane(Circle, Plane, XYZ, DistMax, DistMin)
        Assert.AreEqual(DistMaxExpected, DistMax)
        Assert.AreEqual(DistMinExpected, DistMin)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''GetDist のテスト

    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetDistTest()
        Dim P1 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim P2 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim P3 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        'Dim XYZ As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim XYZ As XYZseibun
        Dim expected As Double = 0.0! ' TODO: 適切な値に初期化してください
        Dim actual As Double

        actual = YCM_Sunpo.GetDist(P1, P2, P3, XYZ)
        Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("このテストメソッドの正確性を確認します。")
    End Sub

    '''<summary>
    '''GetDist のテスト1
    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetDistTest1()
        Dim ID1 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim ID2 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim ID3 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim XYZ As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim expected As Double = 0.0! ' TODO: 適切な値に初期化してください
        Dim actual As Double
        actual = YCM_Sunpo.GetDist(ID1, ID2, ID3, XYZ)
        Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("このテストメソッドの正確性を確認します。")
    End Sub

    '''<summary>
    '''GetDist のテスト2
    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetDistTest2()
        Dim P1 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim P2 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim P3 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim P0 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim XYZ As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim expected As Double = 0.0! ' TODO: 適切な値に初期化してください
        Dim actual As Double

        actual = YCM_Sunpo.GetDist(P1, P2, P3, P0, XYZ)
        Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("このテストメソッドの正確性を確認します。")
    End Sub

    '''<summary>
    '''GetDist のテスト3
    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetDistTest3()
        Dim ID1 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim ID2 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim ID3 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim ID0 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim XYZ As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim expected As Double = 0.0! ' TODO: 適切な値に初期化してください
        Dim actual As Double
        actual = YCM_Sunpo.GetDist(ID1, ID2, ID3, ID0, XYZ)
        Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("このテストメソッドの正確性を確認します。")
    End Sub

    '''<summary>
    '''GetDist のテスト4
    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetDistTest4()
        Dim P1 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim XYZ As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim expected As Double = 0.0! ' TODO: 適切な値に初期化してください
        Dim actual As Double
        actual = YCM_Sunpo.GetDist(P1, XYZ)
        Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("このテストメソッドの正確性を確認します。")
    End Sub

    '''<summary>
    '''GetDist のテスト5
    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetDistTest5()
        Dim P1 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim P2 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim XYZ As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim expected As Double = 0.0! ' TODO: 適切な値に初期化してください
        Dim actual As Double
        actual = YCM_Sunpo.GetDist(P1, P2, XYZ)
        Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("このテストメソッドの正確性を確認します。")
    End Sub

    '''<summary>
    '''GetDist のテスト6
    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetDistTest6()
        Dim ID1 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim ID2 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim XYZ As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim expected As Double = 0.0! ' TODO: 適切な値に初期化してください
        Dim actual As Double
        actual = YCM_Sunpo.GetDist(ID1, ID2, XYZ)
        Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("このテストメソッドの正確性を確認します。")
    End Sub

    '''<summary>
    '''GetDist のテスト7
    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetDistTest7()
        Dim Pnt0 As GeoPoint = Nothing ' TODO: 適切な値に初期化してください
        Dim PIPlane As GeoPlane = Nothing ' TODO: 適切な値に初期化してください
        Dim XYZ As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim expected As Double = 0.0! ' TODO: 適切な値に初期化してください
        Dim actual As Double

        actual = YCM_Sunpo.GetDist(Pnt0, PIPlane, XYZ)
        Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("このテストメソッドの正確性を確認します。")
    End Sub

    '''<summary>
    '''GetDist のテスト8
    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetDistTest8()
        Dim ID1 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim XYZ As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim expected As Double = 0.0! ' TODO: 適切な値に初期化してください
        Dim actual As Double
        actual = YCM_Sunpo.GetDist(ID1, XYZ)
        Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("このテストメソッドの正確性を確認します。")
    End Sub

    '''<summary>
    '''GetCircle3P のテスト

    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetCircle3PTest()
        Dim Point1 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim Point2 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim Point3 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim Circle3P As GeoCurve = Nothing ' TODO: 適切な値に初期化してください
        Dim Circle3PExpected As GeoCurve = Nothing ' TODO: 適切な値に初期化してください
        YCM_Sunpo.GetCircle3P(Point1, Point2, Point3, Circle3P)
        Assert.AreEqual(Circle3PExpected, Circle3P)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''GetCircle3P のテスト

    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetCircle3PTest1()
        Dim ID1 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim ID2 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim ID3 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim Circle3P As GeoCurve = Nothing ' TODO: 適切な値に初期化してください
        Dim Circle3PExpected As GeoCurve = Nothing ' TODO: 適切な値に初期化してください
        YCM_Sunpo.GetCircle3P(ID1, ID2, ID3, Circle3P)
        Assert.AreEqual(Circle3PExpected, Circle3P)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''GetAngleRight のテスト

    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetAngleRightTest()
        Dim ID1 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim ID2 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim XYZ As Plane = New Plane() ' TODO: 適切な値に初期化してください
        Dim expected As Double = 0.0! ' TODO: 適切な値に初期化してください
        Dim actual As Double
        actual = YCM_Sunpo.GetAngleRight(ID1, ID2, XYZ)
        Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("このテストメソッドの正確性を確認します。")
    End Sub

    '''<summary>
    '''GetAngleRight のテスト

    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_GetAngleRightTest1()
        Dim P1 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim P2 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim XYZ As Plane = New Plane() ' TODO: 適切な値に初期化してください
        Dim expected As Double = 0.0! ' TODO: 適切な値に初期化してください
        Dim actual As Double
        actual = YCM_Sunpo.GetAngleRight(P1, P2, XYZ)
        Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("このテストメソッドの正確性を確認します。")
    End Sub

    '''<summary>
    '''AddUserLine のテスト

    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_AddUserLineTest()
        Dim ID1 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim ID2 As Integer = 0 ' TODO: 適切な値に初期化してください
        YCM_Sunpo.AddUserLine(ID1, ID2)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''AddUserLine のテスト

    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_AddUserLineTest1()
        Dim P1 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim P2 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        YCM_Sunpo.AddUserLine(P1, P2)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''AddUserCircle3P のテスト

    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_AddUserCircle3PTest()
        Dim P1 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim P2 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim P3 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        YCM_Sunpo.AddUserCircle3P(P1, P2, P3)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''AddUserCircle3P のテスト

    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_AddUserCircle3PTest1()
        Dim ID1 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim ID2 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim ID3 As Integer = 0 ' TODO: 適切な値に初期化してください
        'Dim ID3 As Integer
        YCM_Sunpo.AddUserCircle3P(ID1, ID2, ID3)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''AddUserCircle1P のテスト

    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_AddUserCircle1PTest()
        Dim ID1 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim r As Double = 0.0! ' TODO: 適切な値に初期化してください
        Dim Normal As GeoVector = Nothing ' TODO: 適切な値に初期化してください

        YCM_Sunpo.AddUserCircle1P(ID1, r, Normal)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''AddUserCircle1P のテスト

    ''' 入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub N_AddUserCircle1PTest1()
        Dim P1 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim r As Double = 0.0! ' TODO: 適切な値に初期化してください
        Dim Normal As GeoVector = Nothing ' TODO: 適切な値に初期化してください
        YCM_Sunpo.AddUserCircle1P(P1, r, Normal)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub
    '''<summary>
    '''GetSortedByCTID のテスト-1
    '''入力値がNothingだった場合

    '''</summary>
    <TestMethod()> _
    Public Sub GetSortedByCTID_Test0()
        Dim NonSortedList() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim SortHouhou As SortMethod = New SortMethod() ' TODO: 適切な値に初期化してください
        Dim SortedList() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim SortedListExpected() As CT_Data = Nothing ' TODO: 適切な値に初期化してください

        YCM_Sunpo.GetSortedByCTID(NonSortedList, SortHouhou, SortedList)
        Assert.AreEqual(SortedListExpected, NonSortedList)
        Assert.AreEqual(SortedListExpected, SortedList)

    End Sub
    '↑↑↑

    'H25.5.30以上は入力値がNothing等の場合のテスト============================================================

    '''<summary>
    '''GetAreaPointGroup のテスト

    '''</summary>
    <TestMethod()> _
    Public Sub GetAreaPointGroupTest()
        Dim PointGroup() As CT_Data = arrCTData  ' TODO: 適切な値に初期化してください
        Dim XYZ As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim BasePoint As New Point3D(100, 0, 0) ' TODO: 適切な値に初期化してください
        Dim DaiSyou As DaiSyou = DaiSyou.Over   ' TODO: 適切な値に初期化してください
        Dim FiltPointGroup() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim FiltPointGroupExpected() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        XYZ = XYZseibun.X
        Dim expected As Double = 0.0! ' TODO: 適切な値に初期化してください
        Dim actual As Double
        ReDim FiltPointGroupExpected(81)

        YCM_Sunpo.GetAreaPointGroup(PointGroup, XYZ, BasePoint, DaiSyou, FiltPointGroup)

        actual = 0.0!

        Assert.AreEqual(81, FiltPointGroup.Length)
        Assert.AreEqual(expected, actual)

    End Sub

    '''<summary>
    '''GetDist のテスト1
    ''' 3点P1,P2,P3からなる平面と点P0までの距離
    '''</summary>
    <TestMethod()> _
    Public Sub GetDist_PlaneToPoint_Test1()
        Dim P1 As New Point3D(0, 0, 0) ' TODO: 適切な値に初期化してください
        Dim P2 As New Point3D(1, 0, 0) ' TODO: 適切な値に初期化してください
        Dim P3 As New Point3D(0, 1, 0) ' TODO: 適切な値に初期化してください
        Dim P0 As New Point3D(1, 1, 10) ' TODO: 適切な値に初期化してください
        Dim XYZ As XYZseibun = XYZseibun.XYZ  ' TODO: 適切な値に初期化してください
        Dim expected As Double = 10.0 ' TODO: 適切な値に初期化してください
        Dim actual As Double
        actual = YCM_Sunpo.GetDist(P1, P2, P3, P0, XYZ)

        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("このテストメソッドの正確性を確認します。")
    End Sub


    '''<summary>
    '''GetDistCircletoPlane のテスト1
    '''円と平面の最大距離・最小距離を算出する
    '''（1）面と平面は平行関係にある（XY平面）－距離はXYZ成分 
    '''</summary>
    <TestMethod()> _
    Public Sub GetDistCircletoPlane_Test1()
        Dim Circle As New GeoCurve  ' TODO: 適切な値に初期化してください
        Dim Plane As New GeoPlane ' TODO: 適切な値に初期化してください
        Dim XYZ As New XYZseibun  ' TODO: 適切な値に初期化してください
        Dim DistMax As New Double  ' TODO: 適切な値に初期化してください
        Dim DistMaxExpected As New Double  ' TODO: 適切な値に初期化してください
        Dim DistMin As New Double ' TODO: 適切な値に初期化してください
        Dim DistMinExpected As New Double ' TODO: 適切な値に初期化してください
        Dim expected As Object = Nothing ' TODO: 適切な値に初期化してください

        Dim C_O As New GeoPoint
        Dim C_N As New GeoVector
        Dim P_O As New GeoPoint
        Dim P_N As New GeoVector

        'Cicle のセット
        C_O.x = 5
        C_O.y = 5
        C_O.z = 10

        C_N.x = 0
        C_N.y = 0
        C_N.z = 1

        Circle.center = C_O
        Circle.radius = 1.0
        Circle.normal = C_N

        'Planeのセット
        P_O.x = 0
        P_O.y = 0
        P_O.z = 0

        P_N.x = 0
        P_N.y = 0
        P_N.z = 1

        Plane.SetByOriginNormal(P_O, P_N)
        XYZ = XYZseibun.XYZ

        YCM_Sunpo.GetDistCircletoPlane(Circle, Plane, XYZ, DistMax, DistMin)

        DistMaxExpected = 10
        DistMinExpected = 10

        Assert.AreEqual(DistMaxExpected, DistMax)
        Assert.AreEqual(DistMinExpected, DistMin)
        'Assert.AreEqual(expected, actual)
        ' Assert.Inconclusive("このテストメソッドの正確性を確認します。")
    End Sub

    '''<summary>
    '''GetDistCircletoPlane のテスト2
    '''円と平面の最大距離・最小距離を算出する
    '''（2）円と平面は平行関係にある（XY平面）－距離はZ成分以外

    '''</summary>
    <TestMethod()> _
    Public Sub GetDistCircletoPlane_Test2()
        Dim Circle As New GeoCurve  ' TODO: 適切な値に初期化してください
        Dim Plane As New GeoPlane ' TODO: 適切な値に初期化してください
        Dim XYZ As New XYZseibun  ' TODO: 適切な値に初期化してください
        Dim DistMax As New Double  ' TODO: 適切な値に初期化してください
        Dim DistMaxExpected As New Double  ' TODO: 適切な値に初期化してください
        Dim DistMin As New Double ' TODO: 適切な値に初期化してください
        Dim DistMinExpected As New Double ' TODO: 適切な値に初期化してください
        Dim expected As Object = Nothing ' TODO: 適切な値に初期化してください


        Dim C_O As New GeoPoint
        Dim C_N As New GeoVector
        Dim P_O As New GeoPoint
        Dim P_N As New GeoVector

        'Cicle のセット
        C_O.x = 5
        C_O.y = 5
        C_O.z = 10

        C_N.x = 0
        C_N.y = 0
        C_N.z = 1

        Circle.center = C_O
        Circle.radius = 1.0
        Circle.normal = C_N

        'Planeのセット
        P_O.x = 0
        P_O.y = 0
        P_O.z = 0

        P_N.x = 0
        P_N.y = 0
        P_N.z = 1

        Plane.SetByOriginNormal(P_O, P_N)
        XYZ = XYZseibun.XY
        YCM_Sunpo.GetDistCircletoPlane(Circle, Plane, XYZ, DistMax, DistMin)

        DistMaxExpected = 0
        DistMinExpected = 0

        Assert.AreEqual(DistMaxExpected, DistMax)
        Assert.AreEqual(DistMinExpected, DistMin)
        'Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("このテストメソッドの正確性を確認します。")
    End Sub

    '''<summary>
    '''GetDistCircletoPlane のテスト3
    '''円と平面の最大距離・最小距離を算出する
    '''（3）円（拡張した平面）と平面が垂直である－距離はXYZ成分
    '''</summary>
    <TestMethod()> _
    Public Sub GetDistCircletoPlane_Test3()
        Dim Circle As New GeoCurve  ' TODO: 適切な値に初期化してください
        Dim Plane As New GeoPlane ' TODO: 適切な値に初期化してください
        Dim XYZ As New XYZseibun  ' TODO: 適切な値に初期化してください
        Dim DistMax As New Double  ' TODO: 適切な値に初期化してください
        Dim DistMaxExpected As New Double  ' TODO: 適切な値に初期化してください
        Dim DistMin As New Double ' TODO: 適切な値に初期化してください
        Dim DistMinExpected As New Double ' TODO: 適切な値に初期化してください
        Dim expected As Object = Nothing ' TODO: 適切な値に初期化してください

        Dim C_O As New GeoPoint
        Dim C_N As New GeoVector
        Dim P_O As New GeoPoint
        Dim P_N As New GeoVector

        'Cicle のセット
        C_O.x = 5
        C_O.y = 5
        C_O.z = 10

        C_N.x = 0
        C_N.y = 0
        C_N.z = 1

        Circle.center = C_O
        Circle.radius = 1.0
        Circle.normal = C_N

        'Planeのセット
        P_O.x = 0
        P_O.y = 0
        P_O.z = 0

        P_N.x = 1
        P_N.y = 0
        P_N.z = 0

        Plane.SetByOriginNormal(P_O, P_N)
        XYZ = XYZseibun.XYZ
        YCM_Sunpo.GetDistCircletoPlane(Circle, Plane, XYZ, DistMax, DistMin)

        DistMaxExpected = 6
        DistMinExpected = 4

        Assert.AreEqual(DistMaxExpected, DistMax)
        Assert.AreEqual(DistMinExpected, DistMin)
        'Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("このテストメソッドの正確性を確認します。")
    End Sub

    '''<summary>
    '''GetDistCircletoPlane のテスト4
    '''円と平面の最大距離・最小距離を算出する
    '''（4）円（拡張した平面）と平面が垂直である－距離はX成分以外

    '''</summary>
    <TestMethod()> _
    Public Sub GetDistCircletoPlane_Test4()
        Dim Circle As New GeoCurve  ' TODO: 適切な値に初期化してください
        Dim Plane As New GeoPlane ' TODO: 適切な値に初期化してください
        Dim XYZ As New XYZseibun  ' TODO: 適切な値に初期化してください
        Dim DistMax As New Double  ' TODO: 適切な値に初期化してください
        Dim DistMaxExpected As New Double  ' TODO: 適切な値に初期化してください
        Dim DistMin As New Double ' TODO: 適切な値に初期化してください
        Dim DistMinExpected As New Double ' TODO: 適切な値に初期化してください
        Dim expected As Object = Nothing ' TODO: 適切な値に初期化してください

        Dim C_O As New GeoPoint
        Dim C_N As New GeoVector
        Dim P_O As New GeoPoint
        Dim P_N As New GeoVector

        'Cicle のセット
        C_O.x = 5
        C_O.y = 5
        C_O.z = 10

        C_N.x = 0
        C_N.y = 0
        C_N.z = 1

        Circle.center = C_O
        Circle.radius = 1.0
        Circle.normal = C_N

        'Planeのセット
        P_O.x = 0
        P_O.y = 0
        P_O.z = 0

        P_N.x = 1
        P_N.y = 0
        P_N.z = 0

        Plane.SetByOriginNormal(P_O, P_N)
        XYZ = XYZseibun.Z
        YCM_Sunpo.GetDistCircletoPlane(Circle, Plane, XYZ, DistMax, DistMin)

        DistMaxExpected = 0
        DistMinExpected = 0

        Assert.AreEqual(DistMaxExpected, DistMax)
        Assert.AreEqual(DistMinExpected, DistMin)
        'Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("このテストメソッドの正確性を確認します。")
    End Sub

    '''<summary>
    '''GetDistCircletoPlane のテスト5
    '''円と平面の最大距離・最小距離を算出する
    '''（5）円（拡張した平面）と平面が同一平面上にある
    '''</summary>
    <TestMethod()> _
    Public Sub GetDistCircletoPlane_Test5()
        Dim Circle As New GeoCurve  ' TODO: 適切な値に初期化してください
        Dim Plane As New GeoPlane ' TODO: 適切な値に初期化してください
        Dim XYZ As New XYZseibun  ' TODO: 適切な値に初期化してください
        Dim DistMax As New Double  ' TODO: 適切な値に初期化してください
        Dim DistMaxExpected As New Double  ' TODO: 適切な値に初期化してください
        Dim DistMin As New Double ' TODO: 適切な値に初期化してください
        Dim DistMinExpected As New Double ' TODO: 適切な値に初期化してください
        Dim expected As Object = Nothing ' TODO: 適切な値に初期化してください

        Dim C_O As New GeoPoint
        Dim C_N As New GeoVector
        Dim P_O As New GeoPoint
        Dim P_N As New GeoVector

        'Cicle のセット
        C_O.x = 5
        C_O.y = 5
        C_O.z = 0

        C_N.x = 0
        C_N.y = 0
        C_N.z = 1

        Circle.center = C_O
        Circle.radius = 1.0
        Circle.normal = C_N

        'Planeのセット
        P_O.x = 0
        P_O.y = 0
        P_O.z = 0

        P_N.x = 0
        P_N.y = 0
        P_N.z = 1

        Plane.SetByOriginNormal(P_O, P_N)

        XYZ = XYZseibun.XYZ
        YCM_Sunpo.GetDistCircletoPlane(Circle, Plane, XYZ, DistMax, DistMin)

        DistMaxExpected = 0
        DistMinExpected = 0

        Assert.AreEqual(DistMaxExpected, DistMax)
        Assert.AreEqual(DistMinExpected, DistMin)
        'Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("このテストメソッドの正確性を確認します。")
    End Sub
    '''<summary>
    '''GetDistCircletoPlane のテスト6
    '''円と平面の最大距離・最小距離を算出する
    '''（6）円と平面が直交かつ、円の中心が平面上にある
    '''</summary>
    <TestMethod()> _
    Public Sub GetDistCircletoPlane_Test6()
        Dim Circle As New GeoCurve  ' TODO: 適切な値に初期化してください
        Dim Plane As New GeoPlane ' TODO: 適切な値に初期化してください
        Dim XYZ As New XYZseibun  ' TODO: 適切な値に初期化してください
        Dim DistMax As New Double  ' TODO: 適切な値に初期化してください
        Dim DistMaxExpected As New Double  ' TODO: 適切な値に初期化してください
        Dim DistMin As New Double ' TODO: 適切な値に初期化してください
        Dim DistMinExpected As New Double ' TODO: 適切な値に初期化してください
        Dim expected As Object = Nothing ' TODO: 適切な値に初期化してください

        Dim C_O As New GeoPoint
        Dim C_N As New GeoVector
        Dim P_O As New GeoPoint
        Dim P_N As New GeoVector

        'Cicle のセット
        C_O.x = 5
        C_O.y = 5
        C_O.z = 0

        C_N.x = 0
        C_N.y = 1
        C_N.z = 0

        Circle.center = C_O
        Circle.radius = 1.0
        Circle.normal = C_N

        'Planeのセット
        P_O.x = 0
        P_O.y = 0
        P_O.z = 0

        P_N.x = 0
        P_N.y = 0
        P_N.z = 1

        Plane.SetByOriginNormal(P_O, P_N)

        XYZ = XYZseibun.XYZ
        YCM_Sunpo.GetDistCircletoPlane(Circle, Plane, XYZ, DistMax, DistMin)

        DistMaxExpected = 1
        DistMinExpected = 1

        Assert.AreEqual(DistMaxExpected, DistMax)
        Assert.AreEqual(DistMinExpected, DistMin)
        'Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("このテストメソッドの正確性を確認します。")
    End Sub

    '''<summary>
    '''KinjiPlane_PointGroup のテスト1
    ''' （1）点群はXY平面上に存在
    '''</summary>
    <TestMethod()> _
    Public Sub KinjiPlane_PointGroup_Test1()
        Dim arrPoint(3) As Point3D ' TODO: 適切な値に初期化してください
        Dim KinjiPlane As New GeoPlane ' TODO: 適切な値に初期化してください
        Dim KinjiPlaneExpected As New GeoPlane ' TODO: 適切な値に初期化してください

        Dim P1 As New Point3D(0, 0, 0)
        Dim P2 As New Point3D(1, 1, 0)
        Dim P3 As New Point3D(1, 2, 0)
        Dim P4 As New Point3D(1, 5, 0)
        arrPoint(0) = P1
        arrPoint(1) = P2
        arrPoint(2) = P3
        arrPoint(3) = P4
        KinjiPlaneExpected.origin.setXYZ(0, 0, 0)
        KinjiPlaneExpected.normal.setXYZ(0, 0, 1)

        YCM_Sunpo.KinjiPlane_PointGroup(arrPoint, KinjiPlane)
        KinjiPlane.normal.Normalize()


        '理論平面と求めた平面が同一平面か？

        '（1）Normalが等価か

        Assert.AreEqual(System.Math.Abs(KinjiPlaneExpected.normal.x), System.Math.Abs(KinjiPlane.normal.x), 0.001)
        Assert.AreEqual(System.Math.Abs(KinjiPlaneExpected.normal.y), System.Math.Abs(KinjiPlane.normal.y), 0.001)
        Assert.AreEqual(System.Math.Abs(KinjiPlaneExpected.normal.z), System.Math.Abs(KinjiPlane.normal.z), 0.001)

        '（2）Originが等しいか
        Assert.AreEqual(System.Math.Abs(KinjiPlaneExpected.origin.z), System.Math.Abs(KinjiPlane.origin.z), 0.001)

        '（3）理論平面のOriginと求めた平面の距離が等価であるか?　
        Assert.AreEqual(0.0, GetDist(KinjiPlane.origin, KinjiPlaneExpected, XYZseibun.XYZ), 0.001)

        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''KinjiPlane_PointGroup のテスト2
    ''' （2）Z=0の点群，Z=10の点群，2つの点群はともにXY平面と平行

    '''</summary>
    <TestMethod()> _
    Public Sub KinjiPlane_PointGroup_Test2()
        Dim arrPoint(5) As Point3D ' TODO: 適切な値に初期化してください
        Dim KinjiPlane As New GeoPlane ' TODO: 適切な値に初期化してください
        Dim KinjiPlaneExpected As New GeoPlane ' TODO: 適切な値に初期化してください

        Dim Pa1 As New Point3D(0, 0, 0)
        Dim Pa2 As New Point3D(100, 0, 0)
        Dim Pa3 As New Point3D(0, 100, 0)
        Dim Pb1 As New Point3D(0, 0, 10)
        Dim Pb2 As New Point3D(100, 0, 10)
        Dim Pb3 As New Point3D(0, 100, 10)

        arrPoint(0) = Pa1
        arrPoint(1) = Pa2
        arrPoint(2) = Pa3
        arrPoint(3) = Pb1
        arrPoint(4) = Pb2
        arrPoint(5) = Pb3

        KinjiPlaneExpected.origin.setXYZ(0, 0, 5)
        KinjiPlaneExpected.normal.setXYZ(0, 0, 1)

        YCM_Sunpo.KinjiPlane_PointGroup(arrPoint, KinjiPlane)
        KinjiPlane.normal.Normalize() '単位ベクトルに


        '理論平面と求めた平面が同一平面か？


        '（1）Normalが等しいか
        Assert.AreEqual(System.Math.Abs(KinjiPlaneExpected.normal.x), System.Math.Abs(KinjiPlane.normal.x), 0.001)
        Assert.AreEqual(System.Math.Abs(KinjiPlaneExpected.normal.y), System.Math.Abs(KinjiPlane.normal.y), 0.001)
        Assert.AreEqual(System.Math.Abs(KinjiPlaneExpected.normal.z), System.Math.Abs(KinjiPlane.normal.z), 0.001)

        '（2）Originが等しいか
        Assert.AreEqual(System.Math.Abs(KinjiPlaneExpected.origin.z), System.Math.Abs(KinjiPlane.origin.z), 0.001)

        '（3）理論平面のOriginから求めた平面の距離が0であるか?　
        Assert.AreEqual(0.0, GetDist(KinjiPlane.origin, KinjiPlaneExpected, XYZseibun.XYZ), 0.001)

        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''ProjectPointGroup のテスト

    '''（1）ある点群をXY平面（Z=0）へ投影する
    '''</summary>
    <TestMethod()> _
    Public Sub ProjectPointGroupTest()
        Dim arrPoint(2) As Point3D  ' TODO: 適切な値に初期化してください
        Dim Plane As New GeoPlane  ' TODO: 適切な値に初期化してください
        Dim ProjectPntG(2) As Point3D  ' TODO: 適切な値に初期化してください
        Dim ProjectPntGExpected(2) As Point3D  ' TODO: 適切な値に初期化してください

        '入力値
        Dim P1 As New Point3D(0.0, 0.0, 0.0)
        Dim P2 As New Point3D(100, 100, 100)
        Dim P3 As New Point3D(500, 500, 500)
        arrPoint(0) = P1
        arrPoint(1) = P2
        arrPoint(2) = P3

        '理論値
        Dim TP1 As New Point3D(0.0, 0.0, 0.0)
        Dim TP2 As New Point3D(100.0, 100.0, 0.0)
        Dim TP3 As New Point3D(500.0, 500.0, 0.0)
        ProjectPntGExpected(0) = TP1
        ProjectPntGExpected(1) = TP2
        ProjectPntGExpected(2) = TP3

        '平面
        Dim Normal As New GeoVector
        Normal.x = 0
        Normal.y = 0
        Normal.z = 1
        Dim origin As New GeoPoint
        origin.x = 0
        origin.y = 0
        origin.z = 0
        Plane.SetByOriginNormal(origin, Normal)

        YCM_Sunpo.ProjectPointGroup(arrPoint, Plane, ProjectPntG)

        Assert.AreEqual(ProjectPntGExpected(0).X, ProjectPntG(0).X)
        Assert.AreEqual(ProjectPntGExpected(0).Y, ProjectPntG(0).Y)
        Assert.AreEqual(ProjectPntGExpected(0).Z, ProjectPntG(0).Z)

        Assert.AreEqual(ProjectPntGExpected(1).X, ProjectPntG(1).X)
        Assert.AreEqual(ProjectPntGExpected(1).Y, ProjectPntG(1).Y)
        Assert.AreEqual(ProjectPntGExpected(1).Z, ProjectPntG(1).Z)

        Assert.AreEqual(ProjectPntGExpected(2).X, ProjectPntG(2).X)
        Assert.AreEqual(ProjectPntGExpected(2).Y, ProjectPntG(2).Y)
        Assert.AreEqual(ProjectPntGExpected(2).Z, ProjectPntG(2).Z)

        'Assert.AreEqual(ProjectPntGExpected, ProjectPntG)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")

    End Sub

    '''<summary>
    '''GetPoint_ByKubun のテスト1
    '''（1）K1，K2ともに定義した数値である場合

    '''</summary>
    <TestMethod()> _
    Public Sub GetPoint_ByKubun_Test1()
        Dim arrCT() As CT_Data = Nothing  ' TODO: 適切な値に初期化してください
        Dim K1 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim K2 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim arrCT_Result() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim arrCT_ResultExpected() As CT_Data = Nothing ' TODO: 適切な値に初期化してください

        arrCT = CType(arrCTData.Clone, CT_Data())
        K1 = 3
        K2 = 1

        'CSV出力確認（1）------------------------------------------------------------------------------------
        'フィルタされないCT_Data
        Dim FilterMonitor As String = ""
        Dim CT As New CT_Data

        Try
            For Each CT In arrCT
                If CT Is Nothing Then
                    FilterMonitor = FilterMonitor & vbNewLine
                    Continue For
                End If
                Dim CoordX As New Double
                Dim CoordY As New Double
                Dim CoordZ As New Double
                CoordX = CDbl(CT.CT_dat.lstRealP3d(0).X)
                CoordY = CDbl(CT.CT_dat.lstRealP3d(0).Y)
                CoordZ = CDbl(CT.CT_dat.lstRealP3d(0).Z)

                FilterMonitor = FilterMonitor & CT.CT_dat.PID & "," & CoordX & "," _
                    & CoordY & "," & CoordZ & vbNewLine
            Next

            My.Computer.FileSystem.WriteAllText("C:\Temp\Test_Sunpo\GetPoint_ByKubunTest1\入力arrCT（フィルタ前）.CSV", FilterMonitor, False)
        Catch ex As Exception

        End Try

        ''フィルタされるCT_Data
        FilterMonitor = ""
        Dim CT_R As New CT_Data
        Try
            For Each CT_R In arrCT_Result
                If CT_R Is Nothing Then
                    FilterMonitor = FilterMonitor & vbNewLine
                    Continue For
                End If
                Dim CoordX As New Double
                Dim CoordY As New Double
                Dim CoordZ As New Double
                CoordX = CDbl(CT_R.CT_dat.lstRealP3d(0).X)
                CoordY = CDbl(CT_R.CT_dat.lstRealP3d(0).Y)
                CoordZ = CDbl(CT_R.CT_dat.lstRealP3d(0).Z)

                FilterMonitor = FilterMonitor & CT_R.CT_dat.PID & "," & CoordX & "," _
                        & CoordY & "," & CoordZ & vbNewLine

            Next
            My.Computer.FileSystem.WriteAllText("C:\Temp\Test_Sunpo\GetPoint_ByKubunTest1\結果arrCT（フィルタ前）.CSV", FilterMonitor, False)
        Catch ex As Exception

        End Try
        'CSV出力確認（1）------------------------------------------------------------------------------------

        YCM_Sunpo.GetPoint_ByKubun(arrCT, K1, K2, arrCT_Result)

        'CSV出力確認（2）------------------------------------------------------------------------------------
        'フィルタされないCT_Data
        FilterMonitor = ""

        Try
            For Each CT In arrCT
                If CT Is Nothing Then
                    FilterMonitor = FilterMonitor & vbNewLine
                    Continue For
                End If
                'Coord = CT.CT_dat.lstRealP3d(0)
                Dim CoordX As New Double
                Dim CoordY As New Double
                Dim CoordZ As New Double
                CoordX = CDbl(CT.CT_dat.lstRealP3d(0).X)
                CoordY = CDbl(CT.CT_dat.lstRealP3d(0).Y)
                CoordZ = CDbl(CT.CT_dat.lstRealP3d(0).Z)

                FilterMonitor = FilterMonitor & CT.CT_dat.PID & "," & CoordX & "," _
                    & CoordY & "," & CoordZ & vbNewLine
            Next
            My.Computer.FileSystem.WriteAllText("C:\Temp\Test_Sunpo\GetPoint_ByKubunTest1\入力arrCT（フィルタ後）.CSV", FilterMonitor, False)
        Catch ex As Exception

        End Try

        'フィルタされるCT_Data
        FilterMonitor = ""

        Try
            For Each CT_R In arrCT_Result
                If CT_R Is Nothing Then
                    FilterMonitor = FilterMonitor & vbNewLine
                    Continue For
                End If
                Dim CoordX As New Double
                Dim CoordY As New Double
                Dim CoordZ As New Double
                CoordX = CDbl(CT_R.CT_dat.lstRealP3d(0).X)
                CoordY = CDbl(CT_R.CT_dat.lstRealP3d(0).Y)
                CoordZ = CDbl(CT_R.CT_dat.lstRealP3d(0).Z)
                FilterMonitor = FilterMonitor & CT_R.CT_dat.PID & "," & CoordX & "," _
                    & CoordY & "," & CoordZ & vbNewLine

            Next
            My.Computer.FileSystem.WriteAllText("C:\Temp\Test_Sunpo\GetPoint_ByKubunTest1\結果arrCT（フィルタ後）.CSV", FilterMonitor, False)
        Catch ex As Exception

        End Try

        'CSV出力確認（2）------------------------------------------------------------------------------------

        'Assert.AreEqual(arrCT_ResultExpected, arrCT_Result)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''GetPoint_ByKubun のテスト2
    '''（2）K1もしくはK2のどちらかをを用いて抽出
    '''</summary>
    <TestMethod()> _
    Public Sub GetPoint_ByKubun_Test2()
        Dim arrCT() As CT_Data = Nothing  ' TODO: 適切な値に初期化してください
        Dim K1 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim K2 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim arrCT_Result() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim arrCT_ResultExpected() As CT_Data = Nothing ' TODO: 適切な値に初期化してください

        arrCT = CType(arrCTData.Clone, CT_Data())
        'K1 = 1
        'K2 = 1

        'CSV出力確認（1）------------------------------------------------------------------------------------
        'フィルタされないCT_Data
        Dim FilterMonitor As String = ""
        Dim CT As New CT_Data

        Try
            For Each CT In arrCT
                If CT Is Nothing Then
                    FilterMonitor = FilterMonitor & vbNewLine
                    Continue For
                End If
                Dim CoordX As New Double
                Dim CoordY As New Double
                Dim CoordZ As New Double
                CoordX = CDbl(CT.CT_dat.lstRealP3d(0).X)
                CoordY = CDbl(CT.CT_dat.lstRealP3d(0).Y)
                CoordZ = CDbl(CT.CT_dat.lstRealP3d(0).Z)

                FilterMonitor = FilterMonitor & CT.CT_dat.PID & "," & CoordX & "," _
                    & CoordY & "," & CoordZ & vbNewLine
            Next

            My.Computer.FileSystem.WriteAllText("C:\Temp\Test_Sunpo\GetPoint_ByKubunTest1\入力arrCT（フィルタ前）.CSV", FilterMonitor, False)
        Catch ex As Exception

        End Try

        ''フィルタされるCT_Data
        FilterMonitor = ""
        Dim CT_R As New CT_Data
        Try
            For Each CT_R In arrCT_Result
                If CT_R Is Nothing Then
                    FilterMonitor = FilterMonitor & vbNewLine
                    Continue For
                End If
                Dim CoordX As New Double
                Dim CoordY As New Double
                Dim CoordZ As New Double
                CoordX = CDbl(CT_R.CT_dat.lstRealP3d(0).X)
                CoordY = CDbl(CT_R.CT_dat.lstRealP3d(0).Y)
                CoordZ = CDbl(CT_R.CT_dat.lstRealP3d(0).Z)

                FilterMonitor = FilterMonitor & CT_R.CT_dat.PID & "," & CoordX & "," _
                        & CoordY & "," & CoordZ & vbNewLine

            Next
            My.Computer.FileSystem.WriteAllText("C:\Temp\Test_Sunpo\GetPoint_ByKubunTest1\結果arrCT（フィルタ前）.CSV", FilterMonitor, False)
        Catch ex As Exception

        End Try
        'CSV出力確認（1）------------------------------------------------------------------------------------

        YCM_Sunpo.GetPoint_ByKubun(arrCT, K1, K2, arrCT_Result)

        'CSV出力確認（2）------------------------------------------------------------------------------------
        'フィルタされないCT_Data
        FilterMonitor = ""

        Try
            For Each CT In arrCT
                If CT Is Nothing Then
                    FilterMonitor = FilterMonitor & vbNewLine
                    Continue For
                End If
                'Coord = CT.CT_dat.lstRealP3d(0)
                Dim CoordX As New Double
                Dim CoordY As New Double
                Dim CoordZ As New Double
                CoordX = CDbl(CT.CT_dat.lstRealP3d(0).X)
                CoordY = CDbl(CT.CT_dat.lstRealP3d(0).Y)
                CoordZ = CDbl(CT.CT_dat.lstRealP3d(0).Z)

                FilterMonitor = FilterMonitor & CT.CT_dat.PID & "," & CoordX & "," _
                    & CoordY & "," & CoordZ & vbNewLine
            Next
            My.Computer.FileSystem.WriteAllText("C:\Temp\Test_Sunpo\GetPoint_ByKubunTest1\入力arrCT（フィルタ後）.CSV", FilterMonitor, False)
        Catch ex As Exception

        End Try

        'フィルタされるCT_Data
        FilterMonitor = ""

        Try
            For Each CT_R In arrCT_Result
                If CT_R Is Nothing Then
                    FilterMonitor = FilterMonitor & vbNewLine
                    Continue For
                End If
                Dim CoordX As New Double
                Dim CoordY As New Double
                Dim CoordZ As New Double
                CoordX = CDbl(CT_R.CT_dat.lstRealP3d(0).X)
                CoordY = CDbl(CT_R.CT_dat.lstRealP3d(0).Y)
                CoordZ = CDbl(CT_R.CT_dat.lstRealP3d(0).Z)
                FilterMonitor = FilterMonitor & CT_R.CT_dat.PID & "," & CoordX & "," _
                    & CoordY & "," & CoordZ & vbNewLine

            Next
            My.Computer.FileSystem.WriteAllText("C:\Temp\Test_Sunpo\GetPoint_ByKubunTest1\結果arrCT（フィルタ後）.CSV", FilterMonitor, False)
        Catch ex As Exception

        End Try

        'CSV出力確認（2）------------------------------------------------------------------------------------

        'Assert.AreEqual(arrCT_ResultExpected, arrCT_Result)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''GetPoint_ByIndex のテスト1
    ''' （1）index1＜index2
    '''</summary>
    <TestMethod()> _
    Public Sub GetPoint_ByIndex_Test1()
        Dim arrCT() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim Index1 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim Index2 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim arrCT_Result() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim arrCT_ResultExpected() As CT_Data = Nothing ' TODO: 適切な値に初期化してください

        arrCT = CType(arrCTData.Clone, CT_Data())

        Index1 = 1
        Index2 = 100

        'CSV出力確認（1）------------------------------------------------------------------------------------
        'フィルタされないCT_Data
        Dim FilterMonitor As String = ""
        Dim CT As New CT_Data

        Try
            For Each CT In arrCT
                If CT Is Nothing Then
                    FilterMonitor = FilterMonitor & vbNewLine
                    Continue For
                End If
                Dim CoordX As New Double
                Dim CoordY As New Double
                Dim CoordZ As New Double
                CoordX = CDbl(CT.CT_dat.lstRealP3d(0).X)
                CoordY = CDbl(CT.CT_dat.lstRealP3d(0).Y)
                CoordZ = CDbl(CT.CT_dat.lstRealP3d(0).Z)

                FilterMonitor = FilterMonitor & CT.CT_dat.PID & "," & CoordX & "," _
                    & CoordY & "," & CoordZ & vbNewLine
            Next

            My.Computer.FileSystem.WriteAllText("C:\Temp\Test_Sunpo\GetPoint_ByIndex_Test1\入力arrCT（フィルタ前）.CSV", FilterMonitor, False)
        Catch ex As Exception

        End Try

        ''フィルタされるCT_Data
        FilterMonitor = ""
        Dim CT_R As New CT_Data
        Try
            For Each CT_R In arrCT_Result
                If CT_R Is Nothing Then
                    FilterMonitor = FilterMonitor & vbNewLine
                    Continue For
                End If
                Dim CoordX As New Double
                Dim CoordY As New Double
                Dim CoordZ As New Double
                CoordX = CDbl(CT_R.CT_dat.lstRealP3d(0).X)
                CoordY = CDbl(CT_R.CT_dat.lstRealP3d(0).Y)
                CoordZ = CDbl(CT_R.CT_dat.lstRealP3d(0).Z)

                FilterMonitor = FilterMonitor & CT_R.CT_dat.PID & "," & CoordX & "," _
                        & CoordY & "," & CoordZ & vbNewLine

            Next
            My.Computer.FileSystem.WriteAllText("C:\Temp\Test_Sunpo\GetPoint_ByIndex_Test1\結果arrCT（フィルタ前）.CSV", FilterMonitor, False)
        Catch ex As Exception

        End Try
        'CSV出力確認（1）------------------------------------------------------------------------------------

        YCM_Sunpo.GetPoint_ByIndex(arrCT, Index1, Index2, arrCT_Result)

        'CSV出力確認（2）------------------------------------------------------------------------------------
        'フィルタされないCT_Data
        FilterMonitor = ""

        Try
            For Each CT In arrCT
                If CT Is Nothing Then
                    FilterMonitor = FilterMonitor & vbNewLine
                    Continue For
                End If
                'Coord = CT.CT_dat.lstRealP3d(0)
                Dim CoordX As New Double
                Dim CoordY As New Double
                Dim CoordZ As New Double
                CoordX = CDbl(CT.CT_dat.lstRealP3d(0).X)
                CoordY = CDbl(CT.CT_dat.lstRealP3d(0).Y)
                CoordZ = CDbl(CT.CT_dat.lstRealP3d(0).Z)

                FilterMonitor = FilterMonitor & CT.CT_dat.PID & "," & CoordX & "," _
                    & CoordY & "," & CoordZ & vbNewLine
            Next
            My.Computer.FileSystem.WriteAllText("C:\Temp\Test_Sunpo\GetPoint_ByIndex_Test1\入力arrCT（フィルタ後）.CSV", FilterMonitor, False)
        Catch ex As Exception

        End Try

        'フィルタされるCT_Data
        FilterMonitor = ""

        Try
            For Each CT_R In arrCT_Result
                If CT_R Is Nothing Then
                    FilterMonitor = FilterMonitor & vbNewLine
                    Continue For
                End If
                Dim CoordX As New Double
                Dim CoordY As New Double
                Dim CoordZ As New Double
                CoordX = CDbl(CT_R.CT_dat.lstRealP3d(0).X)
                CoordY = CDbl(CT_R.CT_dat.lstRealP3d(0).Y)
                CoordZ = CDbl(CT_R.CT_dat.lstRealP3d(0).Z)
                FilterMonitor = FilterMonitor & CT_R.CT_dat.PID & "," & CoordX & "," _
                    & CoordY & "," & CoordZ & vbNewLine

            Next
            My.Computer.FileSystem.WriteAllText("C:\Temp\Test_Sunpo\GetPoint_ByIndex_Test1\結果arrCT（フィルタ後）.CSV", FilterMonitor, False)
        Catch ex As Exception

        End Try

        'CSV出力確認（2）------------------------------------------------------------------------------------
        'Assert.AreEqual(arrCT_ResultExpected, arrCT_Result)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''GetPoint_ByIndex のテスト2
    ''' （2）index1>index2
    '''</summary>
    <TestMethod()> _
    Public Sub GetPoint_ByIndex_Test2()
        Dim arrCT() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim Index1 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim Index2 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim arrCT_Result() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim arrCT_ResultExpected() As CT_Data = Nothing ' TODO: 適切な値に初期化してください

        arrCT = CType(arrCTData.Clone, CT_Data())

        Index1 = 5
        Index2 = 1

        'CSV出力確認（1）------------------------------------------------------------------------------------
        'フィルタされないCT_Data
        Dim FilterMonitor As String = ""
        Dim CT As New CT_Data

        Try
            For Each CT In arrCT
                If CT Is Nothing Then
                    FilterMonitor = FilterMonitor & vbNewLine
                    Continue For
                End If
                Dim CoordX As New Double
                Dim CoordY As New Double
                Dim CoordZ As New Double
                CoordX = CDbl(CT.CT_dat.lstRealP3d(0).X)
                CoordY = CDbl(CT.CT_dat.lstRealP3d(0).Y)
                CoordZ = CDbl(CT.CT_dat.lstRealP3d(0).Z)

                FilterMonitor = FilterMonitor & CT.CT_dat.PID & "," & CoordX & "," _
                    & CoordY & "," & CoordZ & vbNewLine
            Next

            My.Computer.FileSystem.WriteAllText("C:\Temp\Test_Sunpo\GetPoint_ByIndex_Test1\入力arrCT（フィルタ前）.CSV", FilterMonitor, False)
        Catch ex As Exception

        End Try

        ''フィルタされるCT_Data
        FilterMonitor = ""
        Dim CT_R As New CT_Data
        Try
            For Each CT_R In arrCT_Result
                If CT_R Is Nothing Then
                    FilterMonitor = FilterMonitor & vbNewLine
                    Continue For
                End If
                Dim CoordX As New Double
                Dim CoordY As New Double
                Dim CoordZ As New Double
                CoordX = CDbl(CT_R.CT_dat.lstRealP3d(0).X)
                CoordY = CDbl(CT_R.CT_dat.lstRealP3d(0).Y)
                CoordZ = CDbl(CT_R.CT_dat.lstRealP3d(0).Z)

                FilterMonitor = FilterMonitor & CT_R.CT_dat.PID & "," & CoordX & "," _
                        & CoordY & "," & CoordZ & vbNewLine

            Next
            My.Computer.FileSystem.WriteAllText("C:\Temp\Test_Sunpo\GetPoint_ByIndex_Test1\結果arrCT（フィルタ前）.CSV", FilterMonitor, False)
        Catch ex As Exception

        End Try
        'CSV出力確認（1）------------------------------------------------------------------------------------

        YCM_Sunpo.GetPoint_ByIndex(arrCT, Index1, Index2, arrCT_Result)

        'CSV出力確認（2）------------------------------------------------------------------------------------
        'フィルタされないCT_Data
        FilterMonitor = ""

        Try
            For Each CT In arrCT
                If CT Is Nothing Then
                    FilterMonitor = FilterMonitor & vbNewLine
                    Continue For
                End If
                'Coord = CT.CT_dat.lstRealP3d(0)
                Dim CoordX As New Double
                Dim CoordY As New Double
                Dim CoordZ As New Double
                CoordX = CDbl(CT.CT_dat.lstRealP3d(0).X)
                CoordY = CDbl(CT.CT_dat.lstRealP3d(0).Y)
                CoordZ = CDbl(CT.CT_dat.lstRealP3d(0).Z)

                FilterMonitor = FilterMonitor & CT.CT_dat.PID & "," & CoordX & "," _
                    & CoordY & "," & CoordZ & vbNewLine
            Next
            My.Computer.FileSystem.WriteAllText("C:\Temp\Test_Sunpo\GetPoint_ByIndex_Test1\入力arrCT（フィルタ後）.CSV", FilterMonitor, False)
        Catch ex As Exception

        End Try

        'フィルタされるCT_Data
        FilterMonitor = ""

        Try
            For Each CT_R In arrCT_Result
                If CT_R Is Nothing Then
                    FilterMonitor = FilterMonitor & vbNewLine
                    Continue For
                End If
                Dim CoordX As New Double
                Dim CoordY As New Double
                Dim CoordZ As New Double
                CoordX = CDbl(CT_R.CT_dat.lstRealP3d(0).X)
                CoordY = CDbl(CT_R.CT_dat.lstRealP3d(0).Y)
                CoordZ = CDbl(CT_R.CT_dat.lstRealP3d(0).Z)
                FilterMonitor = FilterMonitor & CT_R.CT_dat.PID & "," & CoordX & "," _
                    & CoordY & "," & CoordZ & vbNewLine

            Next
            My.Computer.FileSystem.WriteAllText("C:\Temp\Test_Sunpo\GetPoint_ByIndex_Test1\結果arrCT（フィルタ後）.CSV", FilterMonitor, False)
        Catch ex As Exception

        End Try

        'CSV出力確認（2）------------------------------------------------------------------------------------
        'Assert.AreEqual(arrCT_ResultExpected, arrCT_Result)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub
    '''<summary>
    '''GetPoint_ByIndex のテスト3
    ''' （3）index1 = index2
    '''</summary>
    <TestMethod()> _
    Public Sub GetPoint_ByIndex_Test3()
        Dim arrCT() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim Index1 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim Index2 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim arrCT_Result() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim arrCT_ResultExpected() As CT_Data = Nothing ' TODO: 適切な値に初期化してください

        arrCT = CType(arrCTData.Clone, CT_Data())

        Index1 = 3
        Index2 = 3

        'CSV出力確認（1）------------------------------------------------------------------------------------
        'フィルタされないCT_Data
        Dim FilterMonitor As String = ""
        Dim CT As New CT_Data

        Try
            For Each CT In arrCT
                If CT Is Nothing Then
                    FilterMonitor = FilterMonitor & vbNewLine
                    Continue For
                End If
                Dim CoordX As New Double
                Dim CoordY As New Double
                Dim CoordZ As New Double
                CoordX = CDbl(CT.CT_dat.lstRealP3d(0).X)
                CoordY = CDbl(CT.CT_dat.lstRealP3d(0).Y)
                CoordZ = CDbl(CT.CT_dat.lstRealP3d(0).Z)

                FilterMonitor = FilterMonitor & CT.CT_dat.PID & "," & CoordX & "," _
                    & CoordY & "," & CoordZ & vbNewLine
            Next

            My.Computer.FileSystem.WriteAllText("C:\Temp\Test_Sunpo\GetPoint_ByIndex_Test1\入力arrCT（フィルタ前）.CSV", FilterMonitor, False)
        Catch ex As Exception

        End Try

        ''フィルタされるCT_Data
        FilterMonitor = ""
        Dim CT_R As New CT_Data
        Try
            For Each CT_R In arrCT_Result
                If CT_R Is Nothing Then
                    FilterMonitor = FilterMonitor & vbNewLine
                    Continue For
                End If
                Dim CoordX As New Double
                Dim CoordY As New Double
                Dim CoordZ As New Double
                CoordX = CDbl(CT_R.CT_dat.lstRealP3d(0).X)
                CoordY = CDbl(CT_R.CT_dat.lstRealP3d(0).Y)
                CoordZ = CDbl(CT_R.CT_dat.lstRealP3d(0).Z)

                FilterMonitor = FilterMonitor & CT_R.CT_dat.PID & "," & CoordX & "," _
                        & CoordY & "," & CoordZ & vbNewLine

            Next
            My.Computer.FileSystem.WriteAllText("C:\Temp\Test_Sunpo\GetPoint_ByIndex_Test1\結果arrCT（フィルタ前）.CSV", FilterMonitor, False)
        Catch ex As Exception

        End Try
        'CSV出力確認（1）------------------------------------------------------------------------------------

        YCM_Sunpo.GetPoint_ByIndex(arrCT, Index1, Index2, arrCT_Result)

        'CSV出力確認（2）------------------------------------------------------------------------------------
        'フィルタされないCT_Data
        FilterMonitor = ""

        Try
            For Each CT In arrCT
                If CT Is Nothing Then
                    FilterMonitor = FilterMonitor & vbNewLine
                    Continue For
                End If
                'Coord = CT.CT_dat.lstRealP3d(0)
                Dim CoordX As New Double
                Dim CoordY As New Double
                Dim CoordZ As New Double
                CoordX = CDbl(CT.CT_dat.lstRealP3d(0).X)
                CoordY = CDbl(CT.CT_dat.lstRealP3d(0).Y)
                CoordZ = CDbl(CT.CT_dat.lstRealP3d(0).Z)

                FilterMonitor = FilterMonitor & CT.CT_dat.PID & "," & CoordX & "," _
                    & CoordY & "," & CoordZ & vbNewLine
            Next
            My.Computer.FileSystem.WriteAllText("C:\Temp\Test_Sunpo\GetPoint_ByIndex_Test1\入力arrCT（フィルタ後）.CSV", FilterMonitor, False)
        Catch ex As Exception

        End Try

        'フィルタされるCT_Data
        FilterMonitor = ""

        Try
            For Each CT_R In arrCT_Result
                If CT_R Is Nothing Then
                    FilterMonitor = FilterMonitor & vbNewLine
                    Continue For
                End If
                Dim CoordX As New Double
                Dim CoordY As New Double
                Dim CoordZ As New Double
                CoordX = CDbl(CT_R.CT_dat.lstRealP3d(0).X)
                CoordY = CDbl(CT_R.CT_dat.lstRealP3d(0).Y)
                CoordZ = CDbl(CT_R.CT_dat.lstRealP3d(0).Z)
                FilterMonitor = FilterMonitor & CT_R.CT_dat.PID & "," & CoordX & "," _
                    & CoordY & "," & CoordZ & vbNewLine

            Next
            My.Computer.FileSystem.WriteAllText("C:\Temp\Test_Sunpo\GetPoint_ByIndex_Test1\結果arrCT（フィルタ後）.CSV", FilterMonitor, False)
        Catch ex As Exception

        End Try

        'CSV出力確認（2）------------------------------------------------------------------------------------
        'Assert.AreEqual(arrCT_ResultExpected, arrCT_Result)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub


    '''<summary>
    '''GetPointIntersection のテスト1
    ''' （1）平面はXY平面に並行でZ=1の位置。線分は点（5,5,1）で平面と垂直に交わる
    '''</summary>
    <TestMethod()> _
    Public Sub GetPointIntersection_Test1()
        Dim P1 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim P2 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim P3 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim XYZ As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim P As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim PExpected As Point3D = Nothing ' TODO: 適切な値に初期化してください
        P1 = New Point3D(5.0, 5.0, 10.0)
        P2 = New Point3D(5.0, 5.0, 0.0)
        P3 = New Point3D(1.0, 1.0, 1.0)
        XYZ = XYZseibun.XY
        P = New Point3D
        PExpected = New Point3D(5.0, 5.0, 1.0)
        YCM_Sunpo.GetPointIntersection(P1, P2, P3, XYZ, P)
        Assert.AreEqual(P.X, PExpected.X)
        Assert.AreEqual(P.Y, PExpected.Y)
        Assert.AreEqual(P.Z, PExpected.Z)
        'Assert.AreEqual(PExpected, P)

        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub



    '''<summary>
    '''GetPointIntersection のテスト2
    ''' （2）平面と線分は平行関係にあり、交差点無し

    '''</summary>
    <TestMethod()> _
    Public Sub GetPointIntersection_Test2()
        Dim P1 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim P2 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim P3 As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim XYZ As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim P As Point3D = Nothing ' TODO: 適切な値に初期化してください
        Dim PExpected As Point3D = Nothing ' TODO: 適切な値に初期化してください
        P1 = New Point3D(5.0, 5.0, 10.0)
        P2 = New Point3D(0.0, 0.0, 10.0)
        P3 = New Point3D(1.0, 1.0, 1.0)
        XYZ = XYZseibun.XY
        P = New Point3D
        PExpected = Nothing
        YCM_Sunpo.GetPointIntersection(P1, P2, P3, XYZ, P)
        Assert.AreEqual(P, PExpected)
        'Assert.AreEqual(P.X, PExpected.X)
        'Assert.AreEqual(P.Y, PExpected.Y)
        'Assert.AreEqual(P.Z, PExpected.Z)
        'Assert.AreEqual(PExpected, P)

        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''AddUserLine のテスト1
    ''' 
    '''</summary>
    <TestMethod()> _
    Public Sub AddUserLine_Test1()
        Dim ID1 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim ID2 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim gDrawUserLines() As CUserLine = Nothing ' TODO: 適切な値に初期化してください
        Dim gDrawUserLinesExpected() As CUserLine = Nothing ' TODO: 適切な値に初期化してください
        YCM_Sunpo.AddUserLine(ID1, ID2)
        Assert.AreEqual(gDrawUserLinesExpected, gDrawUserLines)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''AddUserCircle3P のテスト1
    ''' 
    '''</summary>
    <TestMethod()> _
    Public Sub AddUserCircle3P_Test1()
        Dim ID1 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim ID2 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim ID3 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim gDrawCircleNew() As Ccircle = Nothing ' TODO: 適切な値に初期化してください
        Dim gDrawCircleNewExpected() As Ccircle = Nothing ' TODO: 適切な値に初期化してください
        YCM_Sunpo.AddUserCircle3P(ID1, ID2, ID3)
        Assert.AreEqual(gDrawCircleNewExpected, gDrawCircleNew)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''AddUserCircle1P のテスト1
    ''' 
    '''</summary>
    <TestMethod()> _
    Public Sub AddUserCircle1P_Test1()
        Dim ID1 As Integer = 0 ' TODO: 適切な値に初期化してください
        Dim r As Double = 0.0! ' TODO: 適切な値に初期化してください
        Dim Normal As New GeoVector ' TODO: 適切な値に初期化してください
        Dim gDrawCircleNew() As Ccircle = Nothing ' TODO: 適切な値に初期化してください
        Dim gDrawCircleNewExpected() As Ccircle = Nothing ' TODO: 適切な値に初期化してください

        ID1 = 1
        r = 100
        Normal.setXYZ(0.0, 0.0, 1.0)

        YCM_Sunpo.AddUserCircle1P(ID1, r, Normal)
        Assert.AreEqual(gDrawCircleNewExpected, gDrawCircleNew)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    ' '''<summary>
    ' '''GetMinMaxPointCToC のテスト1
    ' '''</summary>
    '<TestMethod()> _
    'Public Sub GetMinMaxPointCToC_Test1()
    '    Dim C1 As New GeoCurve  ' TODO: 適切な値に初期化してください
    '    Dim C2 As New GeoCurve ' TODO: 適切な値に初期化してください
    '    Dim minPC1 As New Point3D  ' TODO: 適切な値に初期化してください
    '    Dim minPC1Expected As New Point3D ' TODO: 適切な値に初期化してください
    '    Dim maxPC1 As New Point3D ' TODO: 適切な値に初期化してください
    '    Dim maxPC1Expected As New Point3D ' TODO: 適切な値に初期化してください
    '    Dim minPC2 As New Point3D ' TODO: 適切な値に初期化してください
    '    Dim minPC2Expected As New Point3D ' TODO: 適切な値に初期化してください
    '    Dim maxPC2 As New Point3D ' TODO: 適切な値に初期化してください
    '    Dim maxPC2Expected As New Point3D ' TODO: 適切な値に初期化してください


    '    '円C1の定義
    '    C1.CurveType = GeoCurveTypeEnum.geoCircle
    '    C1.radius = 1.0
    '    C1.center.setXYZ(0.0, 1.0, 0.0)
    '    C1.normal.setXYZ(0.0, 0.0, 1.0)
    '    '円C2の定義
    '    C2.CurveType = GeoCurveTypeEnum.geoCircle
    '    C2.radius = 2.0
    '    C2.center.setXYZ(0.0, 0.0, 4.0)
    '    C2.normal.setXYZ(1.0, 0.0, 0.0)
    '    '想定される出力

    '    minPC1Expected = New Point3D(0.0, 0.0, 0.0)
    '    maxPC1Expected = New Point3D(0.0, 2.0, 0.0)
    '    minPC2Expected = New Point3D(0.0, 0.0, 2.0)
    '    maxPC2Expected = New Point3D(0.0, 0.0, 6.0)


    '    YCM_Sunpo.GetMinMaxPointCToC(C1, C2, minPC1, maxPC1, minPC2, maxPC2)
    '    Assert.AreEqual(minPC1Expected, minPC1)
    '    Assert.AreEqual(maxPC1Expected, maxPC1)
    '    Assert.AreEqual(minPC2Expected, minPC2)
    '    Assert.AreEqual(maxPC2Expected, maxPC2)
    '    Assert.Inconclusive("値を返さないメソッドは確認できません。")


    '    '---->無理



    'End Sub
    '''<summary>
    '''GetMinMaxPointCToC のテスト1
    '''</summary>
    <TestMethod()> _
    Public Sub GetMinMaxPointCToC_Test1()
        Dim C1 As New GeoCurve  ' TODO: 適切な値に初期化してください
        Dim C2 As New GeoCurve ' TODO: 適切な値に初期化してください
        Dim minPC1 As New Point3D  ' TODO: 適切な値に初期化してください
        Dim minPC1Expected As New Point3D ' TODO: 適切な値に初期化してください
        Dim maxPC1 As New Point3D ' TODO: 適切な値に初期化してください
        Dim maxPC1Expected As New Point3D ' TODO: 適切な値に初期化してください
        Dim minPC2 As New Point3D ' TODO: 適切な値に初期化してください
        Dim minPC2Expected As New Point3D ' TODO: 適切な値に初期化してください
        Dim maxPC2 As New Point3D ' TODO: 適切な値に初期化してください
        Dim maxPC2Expected As New Point3D ' TODO: 適切な値に初期化してください


        '円C1の定義
        C1.CurveType = GeoCurveTypeEnum.geoCircle
        C1.radius = 1.0
        C1.center.setXYZ(0.0, 0.0, 0.0)
        C1.normal.setXYZ(0.0, 0.0, 1.0)
        '円C2の定義
        C2.CurveType = GeoCurveTypeEnum.geoCircle
        C2.radius = System.Math.Sqrt(2.0)
        C2.center.setXYZ(0.0, 0.0, 2.5)
        C2.normal.setXYZ(-1.0, 0.0, 1.0)
        '想定される出力

        minPC1Expected = New Point3D(-1.0, 0.0, 0.0)
        maxPC1Expected = New Point3D(1.0, 0.0, 0.0)
        minPC2Expected = New Point3D(-1.0, 0.0, 1.5)
        maxPC2Expected = New Point3D(1.0, 0.0, 3.5)
        Dim dblTol As Double '許容精度（距離）

        dblTol = 0.0001

        YCM_Sunpo.GetMinMaxPointCToC(C1, C2, minPC1, maxPC1, minPC2, maxPC2)
        '最小距離となる2点
        Assert.AreEqual(Math_IsEqual(CDbl(minPC1Expected.X), CDbl(minPC1.X), dblTol), True)
        Assert.AreEqual(Math_IsEqual(CDbl(minPC1Expected.Y), CDbl(minPC1.Y), dblTol), True)
        Assert.AreEqual(Math_IsEqual(CDbl(minPC1Expected.Z), CDbl(minPC1.Z), dblTol), True)
        Assert.AreEqual(Math_IsEqual(CDbl(minPC2Expected.X), CDbl(minPC2.X), dblTol), True)
        Assert.AreEqual(Math_IsEqual(CDbl(minPC2Expected.Y), CDbl(minPC2.Y), dblTol), True)
        Assert.AreEqual(Math_IsEqual(CDbl(minPC2Expected.Z), CDbl(minPC2.Z), dblTol), True)
        '最大距離となる2点
        Assert.AreEqual(Math_IsEqual(CDbl(maxPC1Expected.X), CDbl(maxPC1.X), dblTol), True)
        Assert.AreEqual(Math_IsEqual(CDbl(maxPC1Expected.Y), CDbl(maxPC1.Y), dblTol), True)
        Assert.AreEqual(Math_IsEqual(CDbl(maxPC1Expected.Z), CDbl(maxPC1.Z), dblTol), True)
        Assert.AreEqual(Math_IsEqual(CDbl(maxPC2Expected.X), CDbl(maxPC2.X), dblTol), True)
        Assert.AreEqual(Math_IsEqual(CDbl(maxPC2Expected.Y), CDbl(maxPC2.Y), dblTol), True)
        Assert.AreEqual(Math_IsEqual(CDbl(maxPC2Expected.Z), CDbl(maxPC2.Z), dblTol), True)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")

    End Sub
    '''<summary>
    '''GetMinMaxPointCToC のテスト2
    '''</summary>
    <TestMethod()> _
    Public Sub GetMinMaxPointCToC_Test2()
        Dim C1 As New GeoCurve  ' TODO: 適切な値に初期化してください
        Dim C2 As New GeoCurve ' TODO: 適切な値に初期化してください
        Dim minPC1 As New Point3D  ' TODO: 適切な値に初期化してください
        Dim minPC1Expected As New Point3D ' TODO: 適切な値に初期化してください
        Dim maxPC1 As New Point3D ' TODO: 適切な値に初期化してください
        Dim maxPC1Expected As New Point3D ' TODO: 適切な値に初期化してください
        Dim minPC2 As New Point3D ' TODO: 適切な値に初期化してください
        Dim minPC2Expected As New Point3D ' TODO: 適切な値に初期化してください
        Dim maxPC2 As New Point3D ' TODO: 適切な値に初期化してください
        Dim maxPC2Expected As New Point3D ' TODO: 適切な値に初期化してください


        '円C1の定義
        C1.CurveType = GeoCurveTypeEnum.geoCircle
        C1.radius = 1.0
        C1.center.setXYZ(0.0, 0.0, 0.0)
        C1.normal.setXYZ(0.0, 0.0, 1.0)
        '円C2の定義
        C2.CurveType = GeoCurveTypeEnum.geoCircle
        C2.radius = 1.0
        C2.center.setXYZ(0.0, 0.0, 1.0)
        C2.normal.setXYZ(0.0, 0.0, 1.0)
        '想定される出力

        minPC1Expected = New Point3D(0.0, 0.0, 0.0)
        maxPC1Expected = New Point3D(0.0, 0.0, 0.0)
        minPC2Expected = New Point3D(0.0, 0.0, 1.0)
        maxPC2Expected = New Point3D(0.0, 0.0, 1.0)
        Dim dblTol As Double '許容精度（距離）

        dblTol = 0.0001

        YCM_Sunpo.GetMinMaxPointCToC(C1, C2, minPC1, maxPC1, minPC2, maxPC2)
        '最小距離となる2点
        'Assert.AreEqual(Math_IsEqual(CDbl(minPC1Expected.X), CDbl(minPC1.X), dblTol), True)
        'Assert.AreEqual(Math_IsEqual(CDbl(minPC1Expected.Y), CDbl(minPC1.Y), dblTol), True)
        Assert.AreEqual(Math_IsEqual(CDbl(minPC1Expected.Z), CDbl(minPC1.Z), dblTol), True)
        'Assert.AreEqual(Math_IsEqual(CDbl(minPC2Expected.X), CDbl(minPC2.X), dblTol), True)
        'Assert.AreEqual(Math_IsEqual(CDbl(minPC2Expected.Y), CDbl(minPC2.Y), dblTol), True)
        Assert.AreEqual(Math_IsEqual(CDbl(minPC2Expected.Z), CDbl(minPC2.Z), dblTol), True)
        '最大距離となる2点
        'Assert.AreEqual(Math_IsEqual(CDbl(maxPC1Expected.X), CDbl(maxPC1.X), dblTol), True)
        'Assert.AreEqual(Math_IsEqual(CDbl(maxPC1Expected.Y), CDbl(maxPC1.Y), dblTol), True)
        Assert.AreEqual(Math_IsEqual(CDbl(maxPC1Expected.Z), CDbl(maxPC1.Z), dblTol), True)
        'Assert.AreEqual(Math_IsEqual(CDbl(maxPC2Expected.X), CDbl(maxPC2.X), dblTol), True)
        'Assert.AreEqual(Math_IsEqual(CDbl(maxPC2Expected.Y), CDbl(maxPC2.Y), dblTol), True)
        Assert.AreEqual(Math_IsEqual(CDbl(maxPC2Expected.Z), CDbl(maxPC2.Z), dblTol), True)
        'Assert.Inconclusive("値を返さないメソッドは確認できません。")

    End Sub

    '''<summary>
    '''GetPoint_ByOffset のテスト1
    '''（1）帰ってくるarrCT_Result()の配列は1つ
    '''</summary>
    <TestMethod()> _
    Public Sub GetPoint_ByOffset_Test1()
        Dim arrCT() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim XYZ As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim OffsetVec As SortMethod = New SortMethod() ' TODO: 適切な値に初期化してください
        Dim arrCT_Result() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim arrCT_ResultExpected() As CT_Data = Nothing ' TODO: 適切な値に初期化してください

        'XYZ = XYZseibun.X
        'XYZ = XYZseibun.Y
        XYZ = XYZseibun.Z
        OffsetVec = SortMethod.descending

        ReDim arrCT(5)
        arrCT(0) = New CT_Data
        arrCT(1) = New CT_Data
        arrCT(2) = New CT_Data
        arrCT(3) = New CT_Data
        arrCT(4) = New CT_Data
        arrCT(5) = New CT_Data


        Dim P_0 As New Point3D(1.0, 0.0, 0.0)
        Dim P_1 As New Point3D(-1.0, 0.0, 0.0)
        Dim P_2 As New Point3D(0.0, 1.0, 0.0)
        Dim P_3 As New Point3D(0.0, -1.0, 0.0)
        Dim P_4 As New Point3D(0.0, 0.0, 1.0)
        Dim P_5 As New Point3D(0.0, 0.0, -1.0)

        ReDim arrCT_ResultExpected(0)
        arrCT_ResultExpected(0) = New CT_Data
        arrCT_ResultExpected(0).CT_dat.lstRealP3d.Add(P_0)
        arrCT_ResultExpected(0).CT_dat.OffsetV.X = 0.0
        arrCT_ResultExpected(0).CT_dat.OffsetV.Y = 0.0
        arrCT_ResultExpected(0).CT_dat.OffsetV.Z = 1.0

        arrCT(0).CT_dat.lstRealP3d.Add(P_0)
        arrCT(1).CT_dat.lstRealP3d.Add(P_1)
        arrCT(2).CT_dat.lstRealP3d.Add(P_2)
        arrCT(3).CT_dat.lstRealP3d.Add(P_3)
        arrCT(4).CT_dat.lstRealP3d.Add(P_4)
        arrCT(5).CT_dat.lstRealP3d.Add(P_5)


        arrCT(0).CT_dat.OffsetV.X = 1.0
        arrCT(0).CT_dat.OffsetV.Y = 0.0
        arrCT(0).CT_dat.OffsetV.Z = 0.0
        

        arrCT(1).CT_dat.OffsetV.X = -1.0
        arrCT(1).CT_dat.OffsetV.Y = 0.0
        arrCT(1).CT_dat.OffsetV.Z = 0.0

        arrCT(2).CT_dat.OffsetV.X = 0.0
        arrCT(2).CT_dat.OffsetV.Y = 1.0
        arrCT(2).CT_dat.OffsetV.Z = 0.0

        arrCT(3).CT_dat.OffsetV.X = 0.0
        arrCT(3).CT_dat.OffsetV.Y = -1.0
        arrCT(3).CT_dat.OffsetV.Z = 0.0

        arrCT(4).CT_dat.OffsetV.X = 0.0
        arrCT(4).CT_dat.OffsetV.Y = 0.0
        arrCT(4).CT_dat.OffsetV.Z = 1.0

        arrCT(5).CT_dat.OffsetV.X = 0.0
        arrCT(5).CT_dat.OffsetV.Y = 0.0
        arrCT(5).CT_dat.OffsetV.Z = -1.0

        YCM_Sunpo.GetPoint_ByOffset(arrCT, XYZ, OffsetVec, arrCT_Result)
        Assert.AreEqual(arrCT_ResultExpected.Length, arrCT_Result.Length)
        Assert.AreEqual(arrCT_ResultExpected(0).CT_dat.OffsetV.X, arrCT_Result(0).CT_dat.OffsetV.X)
        Assert.AreEqual(arrCT_ResultExpected(0).CT_dat.OffsetV.Y, arrCT_Result(0).CT_dat.OffsetV.Y)
        Assert.AreEqual(arrCT_ResultExpected(0).CT_dat.OffsetV.Z, arrCT_Result(0).CT_dat.OffsetV.Z)

        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''GetPoint_ByOffset のテスト2
    '''（2）帰ってくるarrCT_Result()は配列無し

    '''</summary>
    <TestMethod()> _
    Public Sub GetPoint_ByOffset_Test2()
        Dim arrCT() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim XYZ As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim OffsetVec As SortMethod = New SortMethod() ' TODO: 適切な値に初期化してください
        Dim arrCT_Result() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim arrCT_ResultExpected() As CT_Data = Nothing ' TODO: 適切な値に初期化してください

        XYZ = XYZseibun.X
        OffsetVec = SortMethod.descending

        ReDim arrCT(5)
        arrCT(0) = New CT_Data
        arrCT(1) = New CT_Data
        arrCT(2) = New CT_Data
        arrCT(3) = New CT_Data
        arrCT(4) = New CT_Data
        arrCT(5) = New CT_Data


        Dim P_0 As New Point3D(1.0, 0.0, 0.0)
        Dim P_1 As New Point3D(-1.0, 0.0, 0.0)
        Dim P_2 As New Point3D(0.0, 1.0, 0.0)
        Dim P_3 As New Point3D(0.0, -1.0, 0.0)
        Dim P_4 As New Point3D(0.0, 0.0, 1.0)
        Dim P_5 As New Point3D(0.0, 0.0, -1.0)

        ReDim arrCT_ResultExpected(0)
        arrCT_ResultExpected(0) = New CT_Data
        arrCT_ResultExpected(0).CT_dat.lstRealP3d.Add(P_0)
        arrCT_ResultExpected(0).CT_dat.OffsetV.X = 1.0
        arrCT_ResultExpected(0).CT_dat.OffsetV.Y = 0.0
        arrCT_ResultExpected(0).CT_dat.OffsetV.Z = 0.0

        arrCT(0).CT_dat.lstRealP3d.Add(P_0)
        arrCT(1).CT_dat.lstRealP3d.Add(P_1)
        arrCT(2).CT_dat.lstRealP3d.Add(P_2)
        arrCT(3).CT_dat.lstRealP3d.Add(P_3)
        arrCT(4).CT_dat.lstRealP3d.Add(P_4)
        arrCT(5).CT_dat.lstRealP3d.Add(P_5)


        'arrCT(0).CT_dat.OffsetV.X = 1.0
        'arrCT(0).CT_dat.OffsetV.Y = 0.0
        'arrCT(0).CT_dat.OffsetV.Z = 0.0
        arrCT(0).CT_dat.OffsetV.X = 0.0
        arrCT(0).CT_dat.OffsetV.Y = -2.0
        arrCT(0).CT_dat.OffsetV.Z = 0.0

        arrCT(1).CT_dat.OffsetV.X = -1.0
        arrCT(1).CT_dat.OffsetV.Y = 0.0
        arrCT(1).CT_dat.OffsetV.Z = 0.0

        arrCT(2).CT_dat.OffsetV.X = 0.0
        arrCT(2).CT_dat.OffsetV.Y = 1.0
        arrCT(2).CT_dat.OffsetV.Z = 0.0

        arrCT(3).CT_dat.OffsetV.X = 0.0
        arrCT(3).CT_dat.OffsetV.Y = -1.0
        arrCT(3).CT_dat.OffsetV.Z = 0.0

        arrCT(4).CT_dat.OffsetV.X = 0.0
        arrCT(4).CT_dat.OffsetV.Y = 0.0
        arrCT(4).CT_dat.OffsetV.Z = 1.0

        arrCT(5).CT_dat.OffsetV.X = 0.0
        arrCT(5).CT_dat.OffsetV.Y = 0.0
        arrCT(5).CT_dat.OffsetV.Z = -1.0

        YCM_Sunpo.GetPoint_ByOffset(arrCT, XYZ, OffsetVec, arrCT_Result)

        Assert.AreEqual(0, arrCT_Result.Length)

        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''GetPoint_ByOffset のテスト3
    '''（3）入力arrCT（）にオフセット値（0.0,0.0,0.0）が入っている
    '''</summary>
    <TestMethod()> _
    Public Sub GetPoint_ByOffset_Test3()
        Dim arrCT() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim XYZ As XYZseibun = New XYZseibun() ' TODO: 適切な値に初期化してください
        Dim OffsetVec As SortMethod = New SortMethod() ' TODO: 適切な値に初期化してください
        Dim arrCT_Result() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim arrCT_ResultExpected() As CT_Data = Nothing ' TODO: 適切な値に初期化してください

        XYZ = XYZseibun.X
        OffsetVec = SortMethod.descending

        ReDim arrCT(5)
        arrCT(0) = New CT_Data
        arrCT(1) = New CT_Data
        arrCT(2) = New CT_Data
        arrCT(3) = New CT_Data
        arrCT(4) = New CT_Data
        arrCT(5) = New CT_Data


        Dim P_0 As New Point3D(0.0, 0.0, 0.0)
        Dim P_1 As New Point3D(-1.0, 0.0, 0.0)
        Dim P_2 As New Point3D(0.0, 1.0, 0.0)
        Dim P_3 As New Point3D(0.0, -1.0, 0.0)
        Dim P_4 As New Point3D(0.0, 0.0, 1.0)
        Dim P_5 As New Point3D(0.0, 0.0, -1.0)

        ReDim arrCT_ResultExpected(0)
        arrCT_ResultExpected(0) = New CT_Data
        arrCT_ResultExpected(0).CT_dat.lstRealP3d.Add(P_0)
        arrCT_ResultExpected(0).CT_dat.OffsetV.X = 1.0
        arrCT_ResultExpected(0).CT_dat.OffsetV.Y = 0.0
        arrCT_ResultExpected(0).CT_dat.OffsetV.Z = 0.0

        arrCT(0).CT_dat.lstRealP3d.Add(P_0)
        arrCT(1).CT_dat.lstRealP3d.Add(P_1)
        arrCT(2).CT_dat.lstRealP3d.Add(P_2)
        arrCT(3).CT_dat.lstRealP3d.Add(P_3)
        arrCT(4).CT_dat.lstRealP3d.Add(P_4)
        arrCT(5).CT_dat.lstRealP3d.Add(P_5)


        arrCT(0).CT_dat.OffsetV.X = 0.0
        arrCT(0).CT_dat.OffsetV.Y = 0.0
        arrCT(0).CT_dat.OffsetV.Z = 0.0

        arrCT(1).CT_dat.OffsetV.X = -1.0
        arrCT(1).CT_dat.OffsetV.Y = 0.0
        arrCT(1).CT_dat.OffsetV.Z = 0.0

        arrCT(2).CT_dat.OffsetV.X = 0.0
        arrCT(2).CT_dat.OffsetV.Y = 1.0
        arrCT(2).CT_dat.OffsetV.Z = 0.0

        arrCT(3).CT_dat.OffsetV.X = 0.0
        arrCT(3).CT_dat.OffsetV.Y = -1.0
        arrCT(3).CT_dat.OffsetV.Z = 0.0

        arrCT(4).CT_dat.OffsetV.X = 0.0
        arrCT(4).CT_dat.OffsetV.Y = 0.0
        arrCT(4).CT_dat.OffsetV.Z = 1.0

        arrCT(5).CT_dat.OffsetV.X = 0.0
        arrCT(5).CT_dat.OffsetV.Y = 0.0
        arrCT(5).CT_dat.OffsetV.Z = -1.0

        YCM_Sunpo.GetPoint_ByOffset(arrCT, XYZ, OffsetVec, arrCT_Result)

        Assert.AreEqual(0, arrCT_Result.Length)

        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''GetPoint_ByPlane のテスト1
    ''' （1）交点が見つかるケース
    '''</summary>
    <TestMethod()> _
    Public Sub GetPoint_ByPlane_Test1()
        Dim Line1 As New GeoCurve
        Dim P1 As New Point3D(1.5, 0.0, 0.0)
        Dim XYZ As XYZseibun = XYZseibun.YZ
        Dim Point As Point3D = Nothing
        Dim PointExpected As New Point3D(1.5, 1.0, 0.0)

        Dim startP As New GeoPoint
        Dim endP As New GeoPoint
        startP.setXYZ(1.0, 1.0, 0.0)
        endP.setXYZ(2.0, 1.0, 0.0)
        Line1.StartPoint = startP
        Line1.EndPoint = endP

        YCM_Sunpo.GetPoint_ByPlane(Line1, P1, XYZ, Point)
        Assert.AreEqual(PointExpected.X, Point.X)
        Assert.AreEqual(PointExpected.Y, Point.Y)
        Assert.AreEqual(PointExpected.Z, Point.Z)

        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub


    '''<summary>
    '''GetPoint_ByPlane のテスト2
    ''' （2）交点が見つからないケース
    ''' →線分を直線に拡張することで交点を得る//
    '''</summary>
    <TestMethod()> _
    Public Sub GetPoint_ByPlane_Test2()
        Dim Line1 As New GeoCurve
        Dim P1 As New Point3D(5.0, 0.0, 0.0)
        Dim XYZ As XYZseibun = XYZseibun.YZ
        Dim Point As Point3D = Nothing
        Dim PointExpected As New Point3D(5.0, 1.0, 0.0)

        Dim startP As New GeoPoint
        Dim endP As New GeoPoint
        startP.setXYZ(1.0, 1.0, 0.0)
        endP.setXYZ(2.0, 1.0, 0.0)
        Line1.StartPoint = startP
        Line1.EndPoint = endP

        YCM_Sunpo.GetPoint_ByPlane(Line1, P1, XYZ, Point)
        Assert.AreEqual(PointExpected.X, Point.X)
        Assert.AreEqual(PointExpected.Y, Point.Y)
        Assert.AreEqual(PointExpected.Z, Point.Z)

        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''GetDists_ByPointsLine のテスト1
    ''' （1）

    '''</summary>
    <TestMethod()> _
    Public Sub GetDists_ByPointsLine_Test1()
        Dim Points() As CT_Data = Nothing
        Dim XYZ As XYZseibun = New XYZseibun()
        Dim Dists() As Double = Nothing
        Dim DistsExpected() As Double = Nothing

        ReDim Points(2)
        For i As Integer = 0 To 2
            Points(i) = New CT_Data
            Points(i).CT_dat = New Common3DCodedTarget
            Points(i).CT_dat.lstRealP3d = New List(Of Point3D)
            Points(i).CT_dat.lstRealP3d.Add(New Point3D)
        Next i

        Dim P0 As New Point3D(1.0, 0.0, 0.0)
        Dim P1 As New Point3D(2.0, 1.0, 0.0)
        Dim P2 As New Point3D(3.0, 0.0, 0.0)



        Points(0).CT_dat.lstRealP3d.Item(0).X = P0.X
        Points(0).CT_dat.lstRealP3d.Item(0).Y = P0.Y
        Points(0).CT_dat.lstRealP3d.Item(0).Z = P0.Z

        Points(1).CT_dat.lstRealP3d.Item(0).X = P1.X
        Points(1).CT_dat.lstRealP3d.Item(0).Y = P1.Y
        Points(1).CT_dat.lstRealP3d.Item(0).Z = P1.Z

        Points(2).CT_dat.lstRealP3d.Item(0).X = P2.X
        Points(2).CT_dat.lstRealP3d.Item(0).Y = P2.Y
        Points(2).CT_dat.lstRealP3d.Item(0).Z = P2.Z


        XYZ = XYZseibun.XY

        ReDim DistsExpected(2)
        DistsExpected(0) = 0.0
        DistsExpected(1) = 1.0
        DistsExpected(2) = 0.0

        YCM_Sunpo.GetDists_ByPointsLine(Points, XYZ, Dists)

        Assert.AreEqual(DistsExpected.Length, Dists.Length)
        Assert.AreEqual(DistsExpected(0), Dists(0), 0.0001)
        Assert.AreEqual(DistsExpected(1), Dists(1), 0.0001)
        Assert.AreEqual(DistsExpected(2), Dists(2), 0.0001)


        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''GetDists_ByPointsLine のテスト2
    ''' （2）

    '''</summary>
    <TestMethod()> _
    Public Sub GetDists_ByPointsLine_Test2()
        Dim Points() As CT_Data = Nothing
        Dim XYZ As XYZseibun = New XYZseibun()
        Dim Dists() As Double = Nothing
        Dim DistsExpected() As Double = Nothing

        ReDim Points(2)
        For i As Integer = 0 To 2
            Points(i) = New CT_Data
            Points(i).CT_dat = New Common3DCodedTarget
            Points(i).CT_dat.lstRealP3d = New List(Of Point3D)
            Points(i).CT_dat.lstRealP3d.Add(New Point3D)
        Next i

        Dim P0 As New Point3D(1.0, 0.0, 0.0)
        Dim P1 As New Point3D(2.0, 0.0, 0.0)
        Dim P2 As New Point3D(3.0, 0.0, 0.0)



        Points(0).CT_dat.lstRealP3d.Item(0).X = P0.X
        Points(0).CT_dat.lstRealP3d.Item(0).Y = P0.Y
        Points(0).CT_dat.lstRealP3d.Item(0).Z = P0.Z

        Points(1).CT_dat.lstRealP3d.Item(0).X = P1.X
        Points(1).CT_dat.lstRealP3d.Item(0).Y = P1.Y
        Points(1).CT_dat.lstRealP3d.Item(0).Z = P1.Z

        Points(2).CT_dat.lstRealP3d.Item(0).X = P2.X
        Points(2).CT_dat.lstRealP3d.Item(0).Y = P2.Y
        Points(2).CT_dat.lstRealP3d.Item(0).Z = P2.Z


        XYZ = XYZseibun.XY

        ReDim DistsExpected(2)
        DistsExpected(0) = 0.0
        DistsExpected(1) = 0.0
        DistsExpected(2) = 0.0

        YCM_Sunpo.GetDists_ByPointsLine(Points, XYZ, Dists)

        Assert.AreEqual(DistsExpected.Length, Dists.Length)
        Assert.AreEqual(DistsExpected(0), Dists(0), 0.0001)
        Assert.AreEqual(DistsExpected(1), Dists(1), 0.0001)
        Assert.AreEqual(DistsExpected(2), Dists(2), 0.0001)


        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub
    '''<summary>
    '''GetDists_ByPointsLine のテスト3
    ''' （3）

    '''</summary>
    <TestMethod()> _
    Public Sub GetDists_ByPointsLine_Test3()
        Dim Points() As CT_Data = Nothing
        Dim XYZ As XYZseibun = New XYZseibun()
        Dim Dists() As Double = Nothing
        Dim DistsExpected() As Double = Nothing

        ReDim Points(2)
        For i As Integer = 0 To 2
            Points(i) = New CT_Data
            Points(i).CT_dat = New Common3DCodedTarget
            Points(i).CT_dat.lstRealP3d = New List(Of Point3D)
            Points(i).CT_dat.lstRealP3d.Add(New Point3D)
        Next i

        Dim P0 As New Point3D(1.0, 0.0, 0.0)
        Dim P2 As New Point3D(2.0, 0.0, 0.0)
        Dim P1 As New Point3D(3.0, 1.0, 0.0)



        Points(0).CT_dat.lstRealP3d.Item(0).X = P0.X
        Points(0).CT_dat.lstRealP3d.Item(0).Y = P0.Y
        Points(0).CT_dat.lstRealP3d.Item(0).Z = P0.Z

        Points(1).CT_dat.lstRealP3d.Item(0).X = P1.X
        Points(1).CT_dat.lstRealP3d.Item(0).Y = P1.Y
        Points(1).CT_dat.lstRealP3d.Item(0).Z = P1.Z

        Points(2).CT_dat.lstRealP3d.Item(0).X = P2.X
        Points(2).CT_dat.lstRealP3d.Item(0).Y = P2.Y
        Points(2).CT_dat.lstRealP3d.Item(0).Z = P2.Z


        XYZ = XYZseibun.XY

        ReDim DistsExpected(2)
        DistsExpected(0) = 0.0
        DistsExpected(1) = 1.0
        DistsExpected(2) = 0.0

        YCM_Sunpo.GetDists_ByPointsLine(Points, XYZ, Dists)

        Assert.AreEqual(DistsExpected.Length, Dists.Length)
        Assert.AreEqual(DistsExpected(0), Dists(0), 0.0001)
        Assert.AreEqual(DistsExpected(1), Dists(1), 0.0001)
        Assert.AreEqual(DistsExpected(2), Dists(2), 0.0001)


        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''GetSortedByCTID のテスト-1
    '''（1）元々PIDが降順に並んでいるものを昇順に並べ替え

    '''</summary>
    <TestMethod()> _
    Public Sub GetSortedByCTID_Test1()
        Dim SortedList() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        'Dim SortedListExpected() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim SortHouhou As SortMethod = New SortMethod() ' TODO: 適切な値に初期化してください

        ReDim SortedList(2)
        For i As Integer = 0 To 2
            SortedList(i) = New CT_Data
            SortedList(i).CT_dat = New Common3DCodedTarget
            SortedList(i).CT_dat.lstRealP3d = New List(Of Point3D)
            SortedList(i).CT_dat.lstRealP3d.Add(New Point3D)
            SortedList(i).CT_dat.PID = 3 - i

        Next

        Dim P0 As New Point3D(1.0, 0.0, 0.0)
        Dim P1 As New Point3D(10.0, 0.0, 0.0)
        Dim P2 As New Point3D(100.0, 0.0, 0.0)

        SortedList(0).CT_dat.lstRealP3d.Item(0).X = P0.X
        SortedList(0).CT_dat.lstRealP3d.Item(0).Y = P0.Y
        SortedList(0).CT_dat.lstRealP3d.Item(0).Z = P0.Z

        SortedList(1).CT_dat.lstRealP3d.Item(0).X = P1.X
        SortedList(1).CT_dat.lstRealP3d.Item(0).Y = P1.Y
        SortedList(1).CT_dat.lstRealP3d.Item(0).Z = P1.Z

        SortedList(2).CT_dat.lstRealP3d.Item(0).X = P2.X
        SortedList(2).CT_dat.lstRealP3d.Item(0).Y = P2.Y
        SortedList(2).CT_dat.lstRealP3d.Item(0).Z = P2.Z

        SortHouhou = SortMethod.ascending

        YCM_Sunpo.GetSortedByCTID(SortedList, SortHouhou)

        Assert.AreEqual(1, SortedList(0).CT_dat.PID)
        Assert.AreEqual(100.0, SortedList(0).CT_dat.lstRealP3d.Item(0).X)

        Assert.AreEqual(2, SortedList(1).CT_dat.PID)
        Assert.AreEqual(10.0, SortedList(1).CT_dat.lstRealP3d.Item(0).X)

        Assert.AreEqual(3, SortedList(2).CT_dat.PID)
        Assert.AreEqual(1.0, SortedList(2).CT_dat.lstRealP3d.Item(0).X)

        'Assert.Inconclusive("値を返さないメソッドは確認できません。")
    End Sub

    '''<summary>
    '''GetSortedByCTID のテスト-2
    ''' （1）元々PIDが降順に並んでいるものを昇順に並べ替え

    '''</summary>
    <TestMethod()> _
    Public Sub GetSortedByCTID_Test2()
        Dim NonSortedList() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim SortHouhou As SortMethod = New SortMethod() ' TODO: 適切な値に初期化してください
        Dim SortedList() As CT_Data = Nothing ' TODO: 適切な値に初期化してください
        Dim SortedListExpected() As CT_Data = Nothing ' TODO: 適切な値に初期化してください


        ReDim NonSortedList(2)
        For i As Integer = 0 To 2
            NonSortedList(i) = New CT_Data
            NonSortedList(i).CT_dat = New Common3DCodedTarget
            NonSortedList(i).CT_dat.lstRealP3d = New List(Of Point3D)
            NonSortedList(i).CT_dat.lstRealP3d.Add(New Point3D)
            NonSortedList(i).CT_dat.PID = 3 - i

        Next

        Dim P0 As New Point3D(1.0, 0.0, 0.0)
        Dim P1 As New Point3D(10.0, 0.0, 0.0)
        Dim P2 As New Point3D(100.0, 0.0, 0.0)

        NonSortedList(0).CT_dat.lstRealP3d.Item(0).X = P0.X
        NonSortedList(0).CT_dat.lstRealP3d.Item(0).Y = P0.Y
        NonSortedList(0).CT_dat.lstRealP3d.Item(0).Z = P0.Z

        NonSortedList(1).CT_dat.lstRealP3d.Item(0).X = P1.X
        NonSortedList(1).CT_dat.lstRealP3d.Item(0).Y = P1.Y
        NonSortedList(1).CT_dat.lstRealP3d.Item(0).Z = P1.Z

        NonSortedList(2).CT_dat.lstRealP3d.Item(0).X = P2.X
        NonSortedList(2).CT_dat.lstRealP3d.Item(0).Y = P2.Y
        NonSortedList(2).CT_dat.lstRealP3d.Item(0).Z = P2.Z

        SortHouhou = SortMethod.ascending

        YCM_Sunpo.GetSortedByCTID(NonSortedList, SortHouhou, SortedList)
        'SortedList()=========================================================
        '→NonSortedList()がソーティングされたものが格納されているか確認

        Assert.AreEqual(1, SortedList(0).CT_dat.PID)
        Assert.AreEqual(100.0, SortedList(0).CT_dat.lstRealP3d.Item(0).X)

        Assert.AreEqual(2, SortedList(1).CT_dat.PID)
        Assert.AreEqual(10.0, SortedList(1).CT_dat.lstRealP3d.Item(0).X)

        Assert.AreEqual(3, SortedList(2).CT_dat.PID)
        Assert.AreEqual(1.0, SortedList(2).CT_dat.lstRealP3d.Item(0).X)
        'SortedList()=========================================================

        'NonSortedList()======================================================
        '→変わっていないことを確認

        Assert.AreEqual(3, NonSortedList(0).CT_dat.PID)
        Assert.AreEqual(1.0, NonSortedList(0).CT_dat.lstRealP3d.Item(0).X)

        Assert.AreEqual(2, NonSortedList(1).CT_dat.PID)
        Assert.AreEqual(10.0, NonSortedList(1).CT_dat.lstRealP3d.Item(0).X)

        Assert.AreEqual(1, NonSortedList(2).CT_dat.PID)
        Assert.AreEqual(100.0, NonSortedList(2).CT_dat.lstRealP3d.Item(0).X)
        'NonSortedList()======================================================
    End Sub
End Class
