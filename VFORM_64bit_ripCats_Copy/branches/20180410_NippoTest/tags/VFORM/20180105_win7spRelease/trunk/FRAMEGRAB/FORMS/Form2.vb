Imports System.Runtime.InteropServices
Public Class Form2

    Public lstImages As List(Of ImageSet)
    Public Event ImageReaded(ByVal sender As Object, ByVal e As EventArgs)
    Dim hwinid1 As Object
    Dim hwinid2 As Object
    Public WithEvents objFBM As New FeatureImage
    'Friend WithEvents ImgLst As System.Windows.Forms.ImageList
    Dim intW As Integer
    Dim intH As Integer

    'Dim lst_AcqHandle As List(Of Object)
    Dim lstCamParam As List(Of CameraParam)

    'Dim blnCamLive As Boolean
    Dim ho_Image As HALCONXLib.HUntypedObjectX
    Dim lst_resultImage As List(Of HALCONXLib.HUntypedObjectX)


    Dim resultImageFolder As String = My.Application.Info.DirectoryPath & "\ResultImage\"
    Dim thumImageFolder As String = My.Application.Info.DirectoryPath & "\ResultImage\Pdata\"   '未使用
    Dim selectImageFolder As String = My.Application.Info.DirectoryPath & "\SelectImage\"
    Dim indexResultImage As Integer
    Dim resultImageFileName As String = "ShootImage"


    '20150203 ADD BY Yamada Sta--------YCMのCommon.vb 
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
    '20150203 ADD BY Yamada End--------YCMのCommon.vb 


    '20150202 ADD By Yamada 画像選択機能 
    Public Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        hwinid1 = AxHWindowXCtrl1.HalconWindow.HalconID
        hwinid2 = AxHWindowXCtrl2.HalconWindow.HalconID

        Try

            Op.SetPart(hwinid1, 0, 0, 2748, 3840)
            Op.SetPart(hwinid2, 0, 0, 2748, 3840)

            tmptarget = New TargetDetect

            hv_ALLCTID = Tuple.ReadTuple(My.Application.Info.DirectoryPath & "\RectangleCT_ID.tup")

            'indexResultImage = 0

            My.Computer.FileSystem.CopyFile(FBM_MDB, ".\SelectImage\" & FBM_MDB, True)
            ConnectDbFBM(resultImageFolder)

            '１．画像リスト作成
            NewImageListView(resultImageFolder)

            'Op.ReadImage(ho_Image, resultImageFolder & "\ShootImage002")
            Op.ReadImage(ho_Image, objFBM.lstImages(0).ImageFullPath)
            '２．画像表示
            Op.DispObj(ho_Image, hwinid2)
            'CheckBox2.Text = "ShootImage002"

            '３．ターゲット情報取得
            'tmptarget.ReadData(resultImageFolder, 2)
            tmptarget.ReadData(resultImageFolder, objFBM.lstImages(0).ImageId)
            '４．CT表示
            Form1.DispCT(hwinid2, tmptarget)
        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try

    End Sub
    Public Sub NewImageListView(ByVal strPath As String)
        objFBM = New FeatureImage
        objFBM.ProjectPath(True) = strPath
        Dim iRet As Integer
        iRet = objFBM.ErrCode
        If (iRet <> 0) Then
            Select Case iRet
                Case 1
                    MsgBox("指定されたフォルダには画像が入っていません", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
                Case Else
                    MsgBox("objFBM.ProjectPath　Error", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
            End Select
            Exit Sub
        End If

        Dim i As Integer
        Dim n As Integer = CInt(objFBM.lstImages.Count)
        Dim ImgLst As New System.Windows.Forms.ImageList
        ImageListView2.Clear()
        ImgLst.Images.Clear()
        For i = 0 To n - 1
            Dim smallimage As System.Drawing.Image
            smallimage = System.Drawing.Image.FromFile(objFBM.lstImages.Item(i).ImageSmallFullPath & ".jpg")
            ImgLst.Images.Add(smallimage)
            'ImageListView2.Items.Add(i + 1 & ": " & objFBM.lstImages.Item(i).ImageName, i)
            ImageListView2.Items.Add(i + 1 & ": " & objFBM.lstImages.Item(i).ImageName, i)

        Next
        intW = CInt(objFBM.hv_Width) - 1
        intH = CInt(objFBM.hv_Height) - 1
    End Sub
    '20150203 ADD BY Yamada Sta--------YCMのCommon.vb 
    Public Function getFileNamefromFullPath(ByVal strFullPath As String) As String
        Dim strSplit() As String
        strSplit = strFullPath.Split("\")
        getFileNamefromFullPath = strSplit((strSplit.Length - 1))

    End Function
    '20150203 ADD BY Yamada End--------YCMのCommon.vb

    Private Sub CheckBox2_CheckedChanged(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Sub BtnOut_Click(sender As System.Object, e As System.EventArgs) Handles BtnOut.Click
        Dim hvImageResult As HALCONXLib.HUntypedObjectX = Nothing
        Dim ResultTarget As TargetDetect = Nothing
        '１．チェックされた画像ファイル、dbFBMをSelectImageフォルダに出力

        'Op.WriteImage(hvImageResult, "jpeg", 0, resultImageFolder & resultImageFileName & Format(indexResultImage, "000") & ".jpg")
        'If Not dbClass Is Nothing Then

        '    '20150130 ADD By Yamada Sta ---------------------------- 
        '    '重複レコード削除（上書き対応）
        '    ResultTarget.DeleteData(indexResultImage) 'Targetsテーブル
        '    Form1.DeleteCameraPoseTable(indexResultImage) 'CameraPoseテーブル
        '    '20150130 ADD By Yamada End ----------------------------

        '    'レコード登録
        '    ResultTarget.SaveData() 'Targetsテーブル
        '    Form1.SaveCameraPoseTable(objCam, indexResultImage) 'CameraPoseテーブル
        'End If
        'MsgBox("完了しました。")

    End Sub

    Private Sub ImageListView2_ItemCheck(sender As Object, e As System.Windows.Forms.ItemCheckEventArgs) Handles ImageListView2.ItemCheck
        MsgBox("!")
    End Sub


    Private Sub ImageListView2_ItemSelectionChanged(sender As Object, e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs) Handles ImageListView2.ItemSelectionChanged

    End Sub

    Private Sub InitializeComponent()
        Me.SuspendLayout()
        '
        'Form2
        '
        Me.ClientSize = New System.Drawing.Size(524, 347)
        Me.Name = "Form2"
        Me.ResumeLayout(False)

    End Sub
End Class