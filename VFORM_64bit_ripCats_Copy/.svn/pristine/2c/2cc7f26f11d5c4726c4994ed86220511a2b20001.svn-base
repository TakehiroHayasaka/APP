Option Explicit On
Imports System.Runtime.InteropServices
Imports System
Imports Microsoft.VisualBasic.FileIO
Imports System.Text
Imports System.IO
Imports FBMlib
Imports HalconDotNet

Module Common
    '20170330 baluu del start
    'Public Declare Function OmomiExcute Lib _
    '"Omomi.dll" (ByVal num As Int32, _
    '        ByRef Xi As Double, ByRef Yi As Double, ByRef Zi As Double, _
    '        ByRef Xj As Double, ByRef Yj As Double, ByRef Zj As Double, _
    '        ByRef Xa As Double, ByRef Ya As Double, ByRef Za As Double, _
    '        ByRef Xs As Double, ByRef Ys As Double, ByRef Zs As Double) As Int32
    '20170330 baluu del end
    '////////////////////////////////////////////////////////////////////////////////
    Public Declare Function casInitLicense Lib "casProtectLib.dll" (ByVal Mode As Byte, ByVal LicenseFile As String) As Integer
    Public Declare Function casCatchLicense Lib "casProtectLib.dll" (ByVal SysID As Byte, ByVal SubSysID As Byte, ByRef LicData As CAS_LICENSE_DATA) As Integer
    Public Declare Function casAuthorizeLicense Lib "casProtectLib.dll" (ByVal Mode As Byte) As Integer
    Public Declare Function casReleaseLicense Lib "casProtectLib.dll" (ByVal SysID As Byte, ByVal SubSysID As Byte, ByRef LicData As CAS_LICENSE_DATA) As Integer
    Public Declare Function GetAsyncKeyState Lib "user32" (ByVal vKey As Int32) As UShort
    'add by SUSANO
    Public Declare Ansi Function GetPrivateProfileInt Lib "kernel32.dll" Alias "GetPrivateProfileIntA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal nDefault As Integer, ByVal lpFileName As String) As Integer
    Public Declare Ansi Function GetPrivateProfileString Lib "kernel32.dll" Alias "GetPrivateProfileStringA" (ByVal section As String, ByVal key As String, ByVal def As String, ByVal retVal As StringBuilder, ByVal size As Integer, ByVal filePath As String) As Integer
    Public Structure CAS_LICENSE_DATA
        Public Flg As Byte
        Public SysID As Byte         '
        Public SubSysID As Byte      '
        Public Opt As Byte
        Public MaxUse As Byte
        Public InUse As Byte
        Public Info1 As Byte
        Public Info2 As Byte
        ''                              
        Public Comment As String

    End Structure

    Public MainFrm As YCM_MainFrame
    Public View_Dilaog As New YCM_3DView
    Public Data_Point As New Data_Vertex
    Public WorksD As WorksTable
    Public scaleImage As Double = 100
    Public Mean As Object = Nothing
    '--ins.start-------------------------------12.8.28 k.y
    '--移動点一括処理

    Public MovePointBatchListFileName As String = ""    '実行リストファイル
    Public gCurrUserMovePoints() As CLookPoint          '仮表示用の点群
    Public nCurrUserMovePoints As Long                  '表示する仮表示用点数

    Public AddData_Point As New YCM_CreateMovePos       '移動点の作成ダイアログ
    Public gAddStartDrawPointIndex As Long = -999999            '一括処理で追加する前の座標値数
    Public gCurrCreateMovePosTabPage As Integer = 0     'カレントなタブページインデックス  

    Public gKojiDataConf As New KojiDataConf
    Public gYCM_MainFrame As New YCM_MainFrame
    'Public gYCM_3DView As New YCM_3DView
    Public gMMainGamen As New MMainGamen
    Public gfrmProgressBar As New frmProgressBar
    Public gExtraMainForm As New ExtraMainForm
    Public reconstructfrm As ReconstructionWindow
#If 1 Then
    '＜移動点作成保持用＞

    Public Structure st_AddMovePos
        Dim isDelete As Boolean         '=True：削除行

        Dim iType As Integer            '作成タイプ：integer：=2：2点から作成、=3:3点の面から作成、=4:2点の中間

        Dim dirBasePos() As CLookPoint  '移動方向基準点（2～3）：CLookPoint：

        Dim basePos As CLookPoint       '基準点：CLookPoint： SUURI->画像表示時にこの点が写っていればNewPosを投影表示させる

        Dim dMoveValue As Double        '移動量：Double：

        Dim newPos As CLookPoint        '新基準点：CLookPoint：（posType=1で作成）

        Dim strResult As String         '実行結果：String:
    End Structure
    Public gAddMovePos() As st_AddMovePos   '追加した移動点の情報
    Public gNumAddMovePos As Long           '追加した移動点数
#End If
    '--ins.end---------------------------------12.8.28 k.y
    'Public bCreate As New ArrayList             '処理結果（作成）

    'Public basePosName = New ArrayList          '処理結果（基準計測点）

    'Public moveValue = New ArrayList            '処理結果（移動量）

    'Public newPosName = New ArrayList           '処理結果（新計測点）

    'Public sResult = New ArrayList              '処理結果（実行結果）

    '--    Public frmMovePosListSel As New YCM_UserPointBatchListSelect    '移動点一括処理

    '--    Public frmMovePosListResult As New YCM_UserPointBatchListResult '移動点一括処理結果
    '--    Public Move_Point As New YCM_MovePosList        '移動点一括処理結果

    '自動ラベリング
    Public Label_Set As New YCM_LabelSetting
    Public Enum ControlMode
        Normal = 0
        Rotate = 1
        Move = 2
    End Enum
    Public Structure UserEnt
        Dim line As CUserLine
        Dim circle As Ccircle
    End Structure
    '13.1.23背景の色を統一（山田）

    Public FormBackColor As System.Drawing.Color = System.Drawing.Color.FromArgb(255, 50, 50, 50)
    '--ins.start----------------------
    Public bIsOtherDlgOpen As Boolean = False '=True:Mainフレームで他のダイアログを表示中
    Dim gDrawLookPointType As Integer = 1 '計測点の表示方法 =1：Solid、=2：Cross、=3：ターゲット面+中心円+十字線

    Public Structure CADCoordChange
        Dim p1 As CLookPoint
        Dim p2 As CLookPoint
        Dim p3 As CLookPoint
        Dim mat() As Double
        Dim mat_geo As GeoMatrix
    End Structure

    Public bCADElmAddMode As Boolean        '=True:CAD図形読込時に追加読込する
    Public CAD_CoordInfo As CADCoordChange  'CAD図面との3点マッチング関係

    Public gpCADPos(3) As GeoPoint          'マッチングを行なうCAD図面上の3点
    Public goCADElmArrs As CObjectArray     'CAD図から読込んだEntity列（読込時の作業用）

    ''Public nUserLineStart As Long           'CAD図形読込前の線分数
    ''Public nCircleNewStart As Long          'CAD図形読込前の円数
    '--ins.end------------------------
    Public csv_path As String   'ラベリングダイアログに、設計データのパス
    Public xangle As Double     '回転時のX軸の角度
    Public yangle As Double     '回転時のY軸の角度

    Public xangle_Yais As Double '回転用の中間パラメータ
    Public yangle_Yais As Double '回転用の中間パラメータ
    Public xangle_Xais As Double '回転用の中間パラメータ
    Public yangle_Xais As Double '回転用の中間パラメータ
    Public r_tmp As Double '回転用の中間パラメータ

    Public pnt_Xais As New GeoPoint
    Public pnt_Yais As New GeoPoint
    Public Vec_Screen_X As New GeoVector '回転用の中間パラメータ(スクリーンのベクトルX軸)
    Public Vec_Screen_Y As New GeoVector '回転用の中間パラメータ(スクリーンのベクトルY軸)

    Public ChangeFinis As Integer = 0
    Public ChangeStart As Integer = 0
    Public gDrawRays() As CRay          '表示するレイ群
    Public gDrawCamers() As CCamera     '表示するカメラ群
    Public gDrawPoints() As CLookPoint  '表示する点群
    Public gDrawLabelText() As CLabelText   '表示するラベル群
    Public gDrawUserLines() As CUserLine    '表示するユーザーライン群
    Public gDrawCirclepoint() As CcirclePoint '表示するユーザー円点群
    Public gDrawCircleNew() As Ccircle  '表示するユーザー円群
    Public gDrawCircle() As Ccircle     '表示するユーザー円群（未使用）

    Public temp_point As New CLookPoint
    Public nUserLines As Long   '表示するユーザーライン数
    Public nRays As Long        '表示するレイ数
    Public nCamers As Long      '表示するカメラ数
    Public nLookPoints As Long  '表示する点数
    Public nLabelText As Long   '表示するラベル数
    Public nCircle As Long      '表示するユーザー円数
    Public ncirclepoint As Long '表示するユーザー円点数
    Public nCircleNew As Long
    'Public ents_centerP As New GeoPoint
    Public ents_centerC As New GeoPoint

    Public gScaleLine As CUserLine '表示するスケールライン


    'SUSANO ADD START
    Public nSekPoints As Long = 0
    Public gDrawSekPoints() As CLookPoint
    Public gDrawSekLabelText() As CLabelText
    'SUSANO ADD END 


    Public cent_cp As New GeoPoint 'すべての実体の中心点

    'Public quantibiaoshi As Double = 1

    Public first_C_lookpoint As Boolean = True '初回に視点の中心点を計算するかどうか
    Public first_C_camer As Boolean = True '初回にカメラ群の中心点を計算するかどうか
    'Public first_P As Boolean = True
    Public binfirstscalesetting As Boolean = False '初回にスケール設定ダイアログを実行するかどうか
    Public bindistance2pointstart As Boolean = False '初回に両点距離機能を実行するかどうか
    Public cp_fram_start As New GeoPoint '範囲指定機能の中間パラメータ
    Public cp_fram_end As New GeoPoint '範囲指定機能の中間パラメータ
    Public cp_fins_start As New GeoPoint '範囲指定機能の中間パラメータ
    Public cp_fins_end As New GeoPoint '範囲指定機能の中間パラメータ
    Public intdataviewdraw As Integer '計測点グリッドのカレント行

    Public bindataviewclickmode As Boolean = False

    Public binfrmdataview As Boolean '計測点グリッドの状態フラグ
    Public binfrmdataviewclosed As Boolean '計測点グリッドの状態フラグ
    Public bdsdataview As BindingSource
    Public binarrdata As New ArrayList
    Public alldbldatacount As Integer
    Public m_blnCommandArea As Boolean
    Public m_blnDrawRegion As Boolean '20170224 baluu add
    Public m_blnSelectDelArea As Boolean = False '20170222 baluu add
    Public gppointstart As New GeoPoint

    Public bincirclestart As Boolean '円を作成するかどうかのフラグ
    Public intcircle As Integer
    Public gpcircle1 As New GeoPoint '円の3点
    Public gpcircle2 As New GeoPoint '円の3点
    Public gpcircle3 As New GeoPoint '円の3点

    Public gppointend As New GeoPoint

    Public resultMMNew(16) As Double '回転時の中間パラメータ

    Public intgppointarea As Integer '範囲指定を実行するかどうかのフラグの一つ

    Public nCamers_x_max As Double 'カメラ群の最小矩形の右上点のX座標

    Public nCamers_x_min As Double 'カメラ群の最小矩形の左下点のX座標

    Public nCamers_y_max As Double 'カメラ群の最小矩形の右上点のY座標

    Public nCamers_y_min As Double 'カメラ群の最小矩形の左下点のY座標

    Public nCamers_z_max As Double 'カメラ群の最小矩形の右上点のZ座標

    Public nCamers_z_min As Double 'カメラ群の最小矩形の左下点のZ座標

    Public nPoints_x_max As Double '計測点群の最小矩形の右上点のX座標

    Public nPoints_x_min As Double '計測点群の最小矩形の左下点のX座標

    Public nPoints_y_max As Double '計測点群の最小矩形の右上点のY座標

    Public nPoints_y_min As Double '計測点群の最小矩形の左下点のY座標

    Public nPoints_z_max As Double '計測点群の最小矩形の右上点のZ座標

    Public nPoints_z_min As Double '計測点群の最小矩形の左下点のZ座標


    Public View_Dilaog_dblwindowLeft As Double 'ビューの左値
    Public View_Dilaog_dblwindowRight As Double 'ビューの右値
    Public View_Dilaog_dblwindowBottom As Double 'ビューの下値
    Public View_Dilaog_dblwindowTop As Double 'ビューの上値

    Public eye_x As Single '視点のX値
    Public eye_y As Single '視点のY値
    Public eye_z As Single '視点のZ値
    Public ent_x As Single '物点のX値
    Public ent_y As Single '物点のX値
    Public ent_z As Single '物点のX値
    'Public eye_x_dong As Single
    'Public eye_y_dong As Single
    'Public eye_z_dong As Single
    'Public ent_x_dong As Single
    'Public ent_y_dong As Single
    'Public ent_z_dong As Single
    Public nDGV As Long 'グリッドの行数

    Public scalelong As Double = 1000
    'Public Point_d_P1 As String
    'Public Point_d_P2 As String
    'Public Point_Start As Boolean = False
    'Public Point_Start_k As Integer = 0
    Public model_pick As UInteger 'クリックで選択する実体の名前
    Public camera_model_pick As UInteger '20170215 baluu add
    Public model_picks As New ArrayList 'クリックで選択する実体の名前
    Public model_SingleSelct As Boolean = True
    Public model_over As Boolean = False
    Public model_picks_over As Boolean = False
    Public point_name As New ArrayList 'クリックでの点の情報を記録する
    Public IOUtil As IOLib 'IOモジュール
    Public LineWidth_Ray As Double = 0.5F 'レイ線のWidth
    Public LineWidth_Line As Double = 0.5F 'ライン線のWidth
    Public Scale_LineWidth_Line As Double = 3.0F 'スケールライン線のWidth
    Public circleWidth_Line As Double = 0.5F '円のライン線のWidth
    Public m_strDataBasePath As String 'データベースのパス
    Public m_strDataSystemPath As String = System.Windows.Forms.Application.StartupPath & "\計測システムフォルダ\システム設定.mdb"
    Public m_strDataTmpPath As String = System.Windows.Forms.Application.StartupPath & "\計測システムフォルダ\Template\計測データ.mdb"
    Public c_click As New CLookPoint 'クリック選択の機能に、クリックした計測点
    Public ent_click As New CUserEnt 'クリック選択の機能に、クリックした計測点
    Public Arrent_click As New ArrayList
    Public c_click_camers As New CCamera 'クリック選択の機能に、クリックしたカメラ
    Public bln_ClickPoint As Boolean = False '計測点をクリックするかどうか
    Public bln_EntClickPoint As Boolean = False '計測点をクリックするかどうか
    Public bln_ClickRay As Boolean = False 'レイをクリックするかどうか
    Public bln_ClickCamera As Boolean = False 'カメラをクリックするかどうか
    Public str_OutCSVPath As String '出力のCSVパス
    'Public cp1_temp As New CLookPoint
    'Public cp2_temp As New CLookPoint
    'Public set_DblCoord() As Double
    Public Bln_setCoord As Boolean
    'Public g_InputImagePath As String
    Public g_InputNewDbPath As String = System.Windows.Forms.Application.StartupPath & "\New\計測データ.mdb"
    Public g_InputTempPath As String = System.Windows.Forms.Application.StartupPath & "\Temp_Data"
    Public g_InputImagePath As String '新規場合に、指定されるパス
    Public NewOrOld As Integer = 0
    Public binfirstopen As Boolean
    Public g_InputIsavePath As String
    Public bing_InputIsave As Boolean = False
    Public str As New ArrayList
    Public str_new As New ArrayList
    Public centX As Double, centY As Double
    Public getpoint_again As Integer
    Public maxandmin_all(8) As Double '0:max x,1:min x,2:max y,3:min y,4:max z,5:min z,6:cent x,7:cent y,8:cent z,
    Public camer_xmax As CCamera
    Public camer_xmin As CCamera
    Public camer_ymax As CCamera
    Public camer_ymin As CCamera
    Public camer_zmax As CCamera
    Public camer_zmin As CCamera
    Public point_xmax As CLookPoint
    Public point_xmin As CLookPoint
    Public point_ymax As CLookPoint
    Public point_ymin As CLookPoint
    Public point_zmax As CLookPoint
    Public point_zmin As CLookPoint
    Public C_X_MaxDis As Double
    Public C_Y_MaxDis As Double
    Public C_Z_MaxDis As Double
    Public C_X_MinDis As Double
    Public C_Y_MinDis As Double
    Public C_Z_MinDis As Double
    Public P_X_MaxDis As Double
    Public P_Y_MaxDis As Double
    Public P_Z_MaxDis As Double
    Public P_X_MinDis As Double
    Public P_Y_MinDis As Double
    Public P_Z_MinDis As Double
    Public pointnear As Double = 0.00001
    Public pointfar As Double = 100000
    Public bintemptest As Boolean = False

    Public DrawCircleXangle As Double
    Public DrawCircleYangle As Double
    Public DrawCircleR As Double

    Public strLastProjectPath As String = ""

    Public aadsfasdf As System.Drawing.Color = System.Drawing.Color.FromArgb(1, 2, 3, 4)

    Public CommonTypeID As Integer = 28
    Public flgSUEOKI As Boolean = False
    Public Function ZS_GetDataSource(ByVal strSQL As String) As BindingSource
        ZS_GetDataSource = New BindingSource
        Dim connStr As String = _
        "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_strDataBasePath
        '以上の設定からデータアダプターを生成


        Dim Adapter As New System.Data.OleDb.OleDbDataAdapter
        Adapter = New System.Data.OleDb.OleDbDataAdapter(strSQL, connStr)

        '▼データの読み込み
        Dim Table As New DataTable()
        Adapter.Fill(Table)
        'Table.Columns(13).GetType()
        '▼データソースを設定してDataGridViewにデータを表示
        ZS_GetDataSource.DataSource = Table
    End Function
    Public Function YCM_ReadZumenInfoFrmAcs(ByVal strDBPath As String) As Integer
        Dim clsOPe As New CDBOperate
        Dim adoRet As ADODB.Recordset

        If clsOPe.ConnectDB(strDBPath) = False Then
            MsgBox("データベースを開くことができません。")
            YCM_ReadZumenInfoFrmAcs = -1
        End If

        Dim strSQL As String
        'レイ
        strSQL = "SELECT measurepoint3d.X,measurepoint3d.Y,measurepoint3d.Z,camerapos.CX,camerapos.CY,camerapos.CZ,imagepoint3d.CPID,imagepoint3d.MPTID,measurepoint3d.ID "
        strSQL = strSQL + " FROM (imagepoint3d INNER JOIN camerapos ON imagepoint3d.CPID = camerapos.ID) INNER JOIN measurepoint3d ON"
        strSQL = strSQL + " imagepoint3d.MPTID = measurepoint3d.TID WHERE measurepoint3d.flgLabel=1"
        adoRet = clsOPe.CreateRecordset(strSQL)
        If adoRet.RecordCount > 0 Then
            Do Until adoRet.EOF
                ReDim Preserve gDrawRays(nRays)
                gDrawRays(nRays) = New CRay()

                Call gDrawRays(nRays).SetEndPnt(adoRet("X").Value, adoRet("Y").Value, adoRet("Z").Value)
                Call gDrawRays(nRays).SetStartPnt(adoRet("CX").Value, adoRet("CY").Value, adoRet("CZ").Value)
                gDrawRays(nRays).CPID = CInt(adoRet("CPID").Value)  'カメラID
                gDrawRays(nRays).MID = CInt(adoRet("MPTID").Value)  '計測点TID
                gDrawRays(nRays).PointID = CInt(adoRet("ID").Value) '計測点ID
                nRays = nRays + 1
                adoRet.MoveNext()
            Loop
        End If

        strSQL = "SELECT * FROM camerapos"
        adoRet = clsOPe.CreateRecordset(strSQL)
        If adoRet.RecordCount > 0 Then
            Do Until adoRet.EOF
                ReDim Preserve gDrawCamers(nCamers)
                gDrawCamers(nCamers) = New CCamera(adoRet("CX").Value, adoRet("CY").Value, adoRet("CZ").Value)
                gDrawCamers(nCamers).x_angle = adoRet("CA").Value
                gDrawCamers(nCamers).y_angle = adoRet("CB").Value
                gDrawCamers(nCamers).z_angle = adoRet("CG").Value
                gDrawCamers(nCamers).ID = adoRet("ID").Value
                nCamers = nCamers + 1
                adoRet.MoveNext()
            Loop
        End If

        '20170217 baluu add start
        strSQL = "select create_type from measurepoint3d"
        adoRet = clsOPe.CreateRecordset(strSQL)
        If adoRet Is Nothing Then
            If Not clsOPe.ExcuteSQL("ALTER TABLE measurepoint3d ADD COLUMN create_type INTEGER") Then
                clsOPe.DisConnectDB()
                'EVERYTHING WENT WRONG
                Exit Function
            End If
        Else
            adoRet.Close()
        End If
        '20170217 baluu add end

        '--rep.12.10.11        strSQL = "SELECT * FROM measurepoint3d where flgLabel=1"
        strSQL = "SELECT * FROM measurepoint3d"
        adoRet = clsOPe.CreateRecordset(strSQL)
        If adoRet.RecordCount > 0 Then
            Do Until adoRet.EOF
                ReDim Preserve gDrawPoints(nLookPoints)
                gDrawPoints(nLookPoints) = New CLookPoint(adoRet("X").Value, adoRet("Y").Value, adoRet("Z").Value)
                gDrawPoints(nLookPoints).mid = adoRet("ID").Value
                gDrawPoints(nLookPoints).tid = adoRet("TID").Value
                gDrawPoints(nLookPoints).LabelName = Trim(adoRet("currentlabel").Value)
                gDrawPoints(nLookPoints).sortID = nLookPoints
                '--ins.start--------------12.10.11
                gDrawPoints(nLookPoints).type = adoRet("Type").Value
                gDrawPoints(nLookPoints).flgLabel = adoRet("flgLabel").Value
                gDrawPoints(nLookPoints).mode = 0
                gDrawPoints(nLookPoints).posType = IIf(gDrawPoints(nLookPoints).type = 9, 1, 0)
                '--ins.end----------------12.10.11
                '20170217 baluu add start
                If adoRet("create_type").Value Is DBNull.Value Then
                    gDrawPoints(nLookPoints).createType = 0
                Else
                    gDrawPoints(nLookPoints).createType = adoRet("create_type").Value
                End If
                '20170217 baluu add end
                nLookPoints = nLookPoints + 1
                adoRet.MoveNext()
            Loop
        End If
        YCM_UpDateLookPointReal()

        strSQL = "SELECT * FROM measurepoint3d where flgLabel=1"
        adoRet = clsOPe.CreateRecordset(strSQL)
        If adoRet.RecordCount > 0 Then
            Do Until adoRet.EOF
                ReDim Preserve gDrawLabelText(nLabelText)
                gDrawLabelText(nLabelText) = New CLabelText(adoRet("X").Value, adoRet("Y").Value, adoRet("Z").Value)
                gDrawLabelText(nLabelText).mid = adoRet("ID").Value
                gDrawLabelText(nLabelText).LabelName = Trim(adoRet("currentlabel").Value)
                nLabelText = nLabelText + 1
                adoRet.MoveNext()
            Loop
        End If

        '20170221 baluu add start
        strSQL = "SELECT * FROM [SekkeiKeisokuData]"
        adoRet = clsOPe.CreateRecordset(strSQL)
        If adoRet.RecordCount > 0 Then
            Do Until adoRet.EOF
                ReDim Preserve gDrawSekPoints(nSekPoints)
                Dim objNew As New Sekkei_Keisoku.ZahyoChiItems()
                objNew.No = adoRet("ID").Value
                objNew.SekkeiLabel1 = adoRet("Sekkei").Value
                objNew.SokutenMei1 = adoRet("SokutenMei").Value
                objNew.SekkeiTenX = adoRet("SekkeiTenX").Value
                objNew.SekkeiTenY = adoRet("SekkeiTenY").Value
                objNew.SekkeiTenZ = adoRet("SekkeiTenZ").Value
                objNew.KeisokuTenX = adoRet("KeisokuTenX").Value
                objNew.KeisokuTenY = adoRet("KeisokuTenY").Value
                objNew.KeisokuTenZ = adoRet("KeisokuTenZ").Value
                objNew.IchiAwaseXw = adoRet("IchiAwaseXw").Value
                objNew.IchiAwaseYw = adoRet("IchiAwaseYw").Value
                objNew.IchiAwaseZw = adoRet("IchiAwaseZw").Value
                objNew.DiffX = adoRet("DiffX").Value
                objNew.DiffY = adoRet("DiffY").Value
                objNew.DiffZ = adoRet("DiffZ").Value
                objNew.DiffL = adoRet("DiffL").Value
                Dim objNewLookPoint As New CLookPoint(objNew, sys_ScaleInfo.scale)
                objNewLookPoint.createType = 2 '20170217 baluu add
                gDrawSekPoints(nSekPoints) = objNewLookPoint
                'gCurrUserMovePoints(nSekPoints) = objNewLookPoint '20170221 baluu del
                nSekPoints = nSekPoints + 1
                adoRet.MoveNext()
            Loop
        End If
        '20170221 baluu add end

        clsOPe.DisConnectDB()
        '--rep.start-----------------------------
        '計測点からのカメラ平均方向を計算しておく12.10.31/山田
        Dim ii As Long, jj As Long ', iMPTID As Long, iPosID As Long
        Dim cVx As Double, cVy As Double, cVz As Double, nPos As Long
        Dim wlen As Double
        For ii = 0 To nLookPoints - 1
            If (gDrawPoints(ii).type = 1) Or (gDrawPoints(ii).type = 2) Then 'シングルターゲットについて
                gDrawPoints(ii).cVx = -99999999999.0#
                gDrawPoints(ii).cVy = -99999999999.0#
                gDrawPoints(ii).cVz = -99999999999.0#
                cVx = 0
                cVy = 0
                cVz = 0
                nPos = 0
                For jj = 0 To nRays - 1
                    If gDrawPoints(ii).tid = gDrawRays(jj).MID Then
                        cVx = cVx + gDrawRays(jj).startPnt.x
                        cVy = cVy + gDrawRays(jj).startPnt.y
                        cVz = cVz + gDrawRays(jj).startPnt.z
                        nPos = nPos + 1
                    End If
                Next
                If nPos > 0 Then
                    cVx = (cVx / nPos)
                    cVy = (cVy / nPos)
                    cVz = (cVz / nPos)
                    '★20121101カメラ座標(cV○)-計測点座標(gDrawPoints(ii).○)：ベクトル
                    cVx = cVx - gDrawPoints(ii).x
                    cVy = cVy - gDrawPoints(ii).y
                    cVz = cVz - gDrawPoints(ii).z
                    '★20121101ベクトルの長さ

                    wlen = System.Math.Sqrt(cVx * cVx + cVy * cVy + cVz * cVz)
                    '単位ベクトルへ
                    cVx = cVx / wlen
                    cVy = cVy / wlen
                    cVz = cVz / wlen
                    gDrawPoints(ii).cVx = cVx
                    gDrawPoints(ii).cVy = cVy
                    gDrawPoints(ii).cVz = cVz
                End If
            End If
        Next

        '----------------------------------------12.11.19以前(山田)
        'For ii = 0 To nRays - 1
        '    iMPTID = gDrawRays(ii).MID
        '    iPosID = getPointIdFromMId(iMPTID)
        '    If gDrawPoints(iPosID).cVx <= -99999999999.0# Then
        '        cVx = gDrawRays(ii).startPnt.x
        '        cVy = gDrawRays(ii).startPnt.y
        '        cVz = gDrawRays(ii).startPnt.z
        '        nPos = 1
        '        For jj = 0 To nRays - 1
        '            If ii <> jj Then
        '                If (gDrawRays(jj).MID = iMPTID) Then
        '                    cVx = cVx + gDrawRays(jj).startPnt.x
        '                    cVy = cVy + gDrawRays(jj).startPnt.y
        '                    cVz = cVz + gDrawRays(jj).startPnt.z
        '                    nPos = nPos + 1
        '                End If
        '            End If
        '        Next
        '        If nPos > 0 Then
        '            cVx = (cVx / nPos)
        '            cVy = (cVy / nPos)
        '            cVz = (cVz / nPos)
        '            '★20121101カメラ座標(cV○)-計測点座標(gDrawRays(ii).endPnt.○)：ベクトル
        '            cVx = cVx - gDrawRays(ii).endPnt.x
        '            cVy = cVy - gDrawRays(ii).endPnt.y
        '            cVz = cVz - gDrawRays(ii).endPnt.z
        '            '★20121101ベクトルの長さ

        '            wlen = System.Math.Sqrt(cVx * cVx + cVy * cVy + cVz * cVz)
        '            '単位ベクトルへ
        '            cVx = cVx / wlen
        '            cVy = cVy / wlen
        '            cVz = cVz / wlen
        '            Call setPointIdFromMId(iMPTID, cVx, cVy, cVz)
        '        End If
        '    End If

        'Next ii
        '----------------------------------------12.11.19以前(山田)

        '--rep.end-------------------------------
        ''--rep.start-----------------------------12.10.30/山田
        ''計測点からのカメラ平均方向を計算しておく
        'Dim ii As Long, jj As Long, iMPTID As Long, iCPID As Long, iPosID As Long
        'Dim wCamera As CCamera
        'Dim cVx As Double, cVy As Double, cVz As Double, nPos As Long
        'For ii = 0 To nRays - 1
        '    iMPTID = gDrawRays(ii).MID
        '    cVx = 0.0 : cVy = 0.0 : cVz = 0.0
        '    nPos = 0
        '    For jj = 0 To nRays - 1
        '        If (ii <> jj) Then
        '            If (gDrawRays(jj).MID = iMPTID) Then
        '                iCPID = gDrawRays(jj).CPID
        '                'CPIDを指定してカメラベクトルを得る
        '                wCamera = getCameraByCPID(iCPID)
        '                nPos = nPos + 1
        '                cVx = cVx + wCamera.x_angle
        '                cVy = cVy + wCamera.y_angle
        '                cVz = cVz + wCamera.z_angle
        '                wCamera = Nothing
        '            End If
        '        End If
        '    Next
        '    If (nPos > 0) Then
        '        cVx = cVx / CDbl(nPos)
        '        cVy = cVy / CDbl(nPos)
        '        cVz = cVz / CDbl(nPos)
        '        iPosID = gDrawRays(ii).PointID
        '        gDrawPoints(iPosID).cVx = cVx
        '        gDrawPoints(iPosID).cVy = cVy
        '        gDrawPoints(iPosID).cVz = cVz
        '    End If
        'Next ii
        ''--rep.end-------------------------------12.10.23




    End Function
    '★20121031
    Public Function getPointIdFromMId(ByVal iMPTID As Long) As Long
        getPointIdFromMId = -1
        For ii = 0 To nLookPoints - 1
            If (gDrawPoints(ii).tid = iMPTID) Then
                getPointIdFromMId = gDrawPoints(ii).sortID
                Exit Function
            End If
        Next
    End Function
    ' カメラIDを指定してカメラデータを得る
    Public Function getCameraByCPID(ByVal cpid As Long) As CCamera
        Dim ii As Long
        getCameraByCPID = New CCamera
        For ii = 0 To nCamers - 1
            If (gDrawCamers(ii).ID = cpid) Then
                getCameraByCPID = gDrawCamers(ii)
                Exit Function
            End If
        Next
    End Function
    Public Sub setPointIdFromMId(ByVal iMPTID As Long, ByVal cVx As Double, ByVal cVy As Double, ByVal cVz As Double)
        For ii = 0 To nLookPoints - 1
            If (gDrawPoints(ii).tid = iMPTID) Then
                gDrawPoints(ii).cVx = cVx
                gDrawPoints(ii).cVy = cVy
                gDrawPoints(ii).cVz = cVz
            End If
        Next
    End Sub

    Public Function YCM_UpdataSystemscalesettingAcs(ByVal strDBPath As String, ByVal cp1 As CLookPoint, ByVal cp2 As CLookPoint, ByVal scalelong As Double, ByVal scalevalue As Double) As Integer
        Dim clsOPe As New CDBOperate
        Dim adoRet As ADODB.Recordset
        If clsOPe.ConnectDB(strDBPath) = False Then
            YCM_UpdataSystemscalesettingAcs = -1
        End If
        Dim strSQL As String
        If cp1.mid > 0 And cp2.mid > 0 Then
            strSQL = "INSERt INTO userscalesetting(PID1,PID2,スケール長,スケール値) VALUES(" & cp1.mid & "," & cp2.mid & "," & scalelong & "," & scalevalue & ")"
            adoRet = clsOPe.CreateRecordset(strSQL)
        End If
        clsOPe.DisConnectDB()
    End Function

    Public Function YCM_UpdataSystemscalevalueAcs(ByVal strDBPath As String, ByVal scalevalue As Double) As Integer
        Dim clsOPe As New CDBOperate
        Dim adoRet As ADODB.Recordset
        If clsOPe.ConnectDB(strDBPath) = False Then
            'MsgBox("データベースを開くことができません。")
            YCM_UpdataSystemscalevalueAcs = -1
        End If
        Dim strSQL As String
        strSQL = "DELETE * FROM scalevalue"
        clsOPe.ExcuteSQL(strSQL)
        strSQL = "INSERt INTO scalevalue(スケール値) VALUES(" & scalevalue & ")"
        adoRet = clsOPe.CreateRecordset(strSQL)
        clsOPe.DisConnectDB()
    End Function
    Public Sub YCM_DrawCamer(ByVal came As CCamera, ByVal blnSelected As Boolean) '20170209 baluu edit ( added blnSelected)
        'z = z - 0.05
        Dim x As Double, y As Double, z As Double
        Dim anglex As Double, angley As Double, anglez As Double
        Dim mat_lib As New MatLib
        x = came.x : y = came.y : z = came.z
        anglex = came.x_angle : angley = came.y_angle : anglez = came.z_angle
        Dim k_mat As Integer = 4
        'Dim n As Integer = 20
        'Dim r As Double = 0.03
        'Dim Pi As Double = 3.1415926536
        'Dim k As Integer = 100
        Dim resultM(k_mat, k_mat) As Double
        Dim resultMNew(k_mat * k_mat) As Double
        Dim rotateXM(k_mat, k_mat) As Double, rotateYM(k_mat, k_mat) As Double, rotateZM(k_mat, k_mat) As Double
        Dim moveM(k_mat, k_mat) As Double
        rotateXM = mat_lib.GetRoateXMat(anglex)
        rotateYM = mat_lib.GetRoateYMat(angley)
        rotateZM = mat_lib.GetRoateZMat(anglez)
        moveM = mat_lib.GetMoveMat(x, y, z)
        resultM = mat_lib.Multiply(rotateZM, rotateYM)
        resultM = mat_lib.Multiply(resultM, rotateXM)
        resultM = mat_lib.Multiply(resultM, moveM)

        resultMNew = mat_lib.Mat2OpenglMat(resultM)
        temp_point.x = 0.0
        temp_point.y = 0.0
        temp_point.z = 1.0
        YCM_LookPointMatPoint(temp_point, resultMNew)
        came.x_eye = temp_point.Real_x
        came.y_eye = temp_point.Real_y
        came.z_eye = temp_point.Real_z

        glPushMatrix()

        glMultMatrixd(resultMNew)
        glRotated(180, 0, 0, 1) ' 20121120　SUURI 修正　カメラの向きを調整
        glScalef(10 * entset_camera.screensize / 100.0 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom), _
                 10.0 * entset_camera.screensize / 100.0 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom), _
                 10.0 * entset_camera.screensize / 100.0 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom))

        'glColorMaterial(GL_FRONT_AND_BACK, GL_AMBIENT_AND_DIFFUSE)
        'glEnable(GL_COLOR_MATERIAL)
        '13.1.31（修正）===========================================================================================
        'カメラの描画

        'カメラ本体

        Dim W As Double = 0.1F '本体の幅*0.5
        Dim H As Double = 0.06F '本体高さ*0.5
        Dim T As Double = 0.04F '本体厚さ*0.5

        'シャッター
        Dim W0 As Double = -0.09F
        Dim W1 As Double = W0 + 0.05F 'W1-W0=シャッター幅（0.03）

        Dim H0 As Double = 0.06F
        Dim H1 As Double = H0 + 0.02F 'H1-H0=シャッター高さ（0.01）

        Dim T0 As Double = 0.0F
        Dim T1 As Double = T0 + 0.02F 'T1-T0=シャッター厚さ（0.01）


        'レンズ
        Dim n As Integer = 10.0F 'レンズの上面、下面の角数
        Dim r As Double = 0.04F 'レンズの上面、下面の半径

        Dim Pi As Double = 3.1415926536
        Dim height As Double = T * 1.7F  'レンズの厚さ
        Dim steps As Double = 2 * Pi / n '角の数

        '<<カメラ本体>>
        'Dim TempColor As System.Drawing.Color = Color.SteelBlue
        'Dim Mat_Color1() As Single = {TempColor.R / 255, TempColor.G / 255, TempColor.B / 255, 1.0}
        'Dim Red As Single = TempColor.R / 255
        'Dim Green As Single = TempColor.G / 255
        'Dim Blue As Single = TempColor.B / 255
        Dim Mat_Color1() As Single = GetCameraColor({entset_camera.color.dbl_red, entset_camera.color.dbl_green, entset_camera.color.dbl_blue, 1.0}, blnSelected) '一時コマンド（消すな!!）'20170209 baluu edit

        glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_Color1)
        glMaterialfv(GL_FRONT, GL_DIFFUSE, {0.26, 0.26, 0.26, 1.0})
        glMaterialfv(GL_FRONT, GL_SPECULAR, {1.0, 1.0, 1.0, 1.0})
        glMaterialf(GL_FRONT, GL_SHININESS, 30)

        '①Top（y=H）

        GL.glBegin(GL_QUADS)
        GL.glNormal3f(0.0, 1.0, 0.0) '法線ベクトル
        GL.glVertex3f(W, H, -T)
        GL.glVertex3f(-W, H, -T)
        GL.glVertex3f(-W, H, T)
        GL.glVertex3f(W, H, T)
        GL.glEnd()
        '②Bottom（y=-H）

        GL.glBegin(GL_QUADS)
        GL.glNormal3f(0.0, -1.0, 0.0) '法線ベクトル
        GL.glVertex3f(W, -H, T)
        GL.glVertex3f(-W, -H, T)
        GL.glVertex3f(-W, -H, -T)
        GL.glVertex3f(W, -H, -T)
        GL.glEnd()
        '③Front(Z=T)
        GL.glBegin(GL_QUADS)
        GL.glNormal3f(0.0, 0.0, 1.0) '法線ベクトル
        GL.glVertex3f(W, H, T)
        GL.glVertex3f(-W, H, T)
        GL.glVertex3f(-W, -H, T)
        GL.glVertex3f(W, -H, T)
        GL.glEnd()
        '④Back(Z=-T)
        GL.glBegin(GL_QUADS)
        GL.glNormal3f(0.0, 0.0, -1.0) '法線ベクトル
        GL.glVertex3f(-W, H, -T)
        GL.glVertex3f(W, H, -T)
        GL.glVertex3f(W, -H, -T)
        GL.glVertex3f(-W, -H, -T)
        GL.glEnd()
        '⑤Left(x=-W)
        GL.glBegin(GL_QUADS)
        GL.glNormal3f(-1.0, 0.0, 0.0) '法線ベクトル
        GL.glVertex3f(-W, H, T)
        GL.glVertex3f(-W, H, -T)
        GL.glVertex3f(-W, -H, -T)
        GL.glVertex3f(-W, -H, T)
        GL.glEnd()
        '
        '⑥Right(x = W)
        GL.glBegin(GL_QUADS)
        GL.glNormal3f(1.0, 0.0, 0.0) '法線ベクトル
        GL.glVertex3f(W, H, -T)
        GL.glVertex3f(W, H, T)
        GL.glVertex3f(W, -H, T)
        GL.glVertex3f(W, -H, -T)
        GL.glEnd()

        '<<シャッター>>
        Dim Mat_Color3() As Single = GetCameraColor({entset_camera.color3.dbl_red, entset_camera.color3.dbl_green, entset_camera.color3.dbl_blue, 1.0}, blnSelected) '20170209 baluu edit
        glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_Color3)

        '①Top（y=H1）

        GL.glBegin(GL_QUADS)
        GL.glNormal3f(0.0, 1.0, 0.0) '法線ベクトル
        GL.glVertex3f(W1, H1, T0)
        GL.glVertex3f(W0, H1, T0)
        GL.glVertex3f(W0, H1, T1)
        GL.glVertex3f(W1, H1, T1)
        GL.glEnd()
        '②Bottom（y=H0）

        GL.glBegin(GL_QUADS)
        GL.glNormal3f(0.0, -1.0, 0.0) '法線ベクトル
        GL.glVertex3f(W1, H0, T1)
        GL.glVertex3f(W0, H0, T1)
        GL.glVertex3f(W0, H0, T0)
        GL.glVertex3f(W1, H0, T0)
        GL.glEnd()
        '③Front(Z=T1)
        GL.glBegin(GL_QUADS)
        GL.glNormal3f(0.0, 0.0, 1.0) '法線ベクトル
        GL.glVertex3f(W1, H1, T1)
        GL.glVertex3f(W0, H1, T1)
        GL.glVertex3f(W0, H0, T1)
        GL.glVertex3f(W1, H0, T1)
        GL.glEnd()
        '④Back(Z=T0)
        GL.glBegin(GL_QUADS)
        GL.glNormal3f(0.0, 0.0, -1.0) '法線ベクトル
        GL.glVertex3f(W0, H1, T0)
        GL.glVertex3f(W1, H1, T0)
        GL.glVertex3f(W1, H0, T0)
        GL.glVertex3f(W0, H0, T0)
        GL.glEnd()
        '⑤Left(x=W0)
        GL.glBegin(GL_QUADS)
        GL.glNormal3f(-1.0, 0.0, 0.0) '法線ベクトル
        GL.glVertex3f(W0, H1, T1)
        GL.glVertex3f(W0, H1, T0)
        GL.glVertex3f(W0, H0, T0)
        GL.glVertex3f(W0, H0, T1)
        GL.glEnd()
        '
        '⑥Right(x=W1)
        GL.glBegin(GL_QUADS)
        GL.glNormal3f(1.0, 0.0, 0.0) '法線ベクトル
        GL.glVertex3f(W1, H1, T0)
        GL.glVertex3f(W1, H1, T1)
        GL.glVertex3f(W1, H0, T1)
        GL.glVertex3f(W1, H0, T0)
        GL.glEnd()

        '<<レンズ1>>
        Dim Mat_Color2() As Single = GetCameraColor({entset_camera.color2.dbl_red, entset_camera.color2.dbl_green, entset_camera.color2.dbl_blue, 1.0}, blnSelected) '20170209 baluu edit
        glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_Color2)

        '①円柱上面（Z=height）

        GL.glBegin(GL_POLYGON)
        GL.glNormal3d(0.0, 0.0, 1.0) '法線ベクトル
        For i As Double = 0 To n - 1 Step 1  'n；n角形の頂点の数
            Dim A As Double = steps * i
            glVertex3d(r * Math.Cos(A), r * Math.Sin(A), height)
        Next
        GL.glEnd()

        '②円柱下面（Z=0）※上面とは頂点を辿る順番が逆回りとする
        GL.glBegin(GL_POLYGON)
        '  GL.glColor3f(entset_point.color2.dbl_red, entset_point.color2.dbl_green, entset_point.color2.dbl_blue)
        GL.glNormal3d(0.0, 0.0, -1.0) '法線ベクトル
        For i As Double = n - 1 To 0 Step -1 'n；n角形の頂点の数
            Dim A As Double = steps * i
            glVertex3d(r * Math.Cos(A), r * Math.Sin(A), 0)
        Next
        GL.glEnd()

        '③円柱側面
        GL.glBegin(GL_QUAD_STRIP)
        '   GL.glColor3d(entset_point.color2.dbl_red, entset_point.color2.dbl_green, entset_point.color2.dbl_blue)
        For i As Double = 0 To n
            Dim A As Double = steps * i
            glNormal3d(r * Math.Cos(A), r * Math.Sin(A), 0) '法線ベクトル
            glVertex3f(r * Math.Cos(A), r * Math.Sin(A), height)
            glVertex3f(r * Math.Cos(A), r * Math.Sin(A), 0)
        Next
        GL.glEnd()

        '追加==========================================================================================================
        '<<レンズ2>>
        Dim Mat_Color4() As Single = {0.7, 0.7, 0.7, 1.0}
        glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_Color4)
        glMaterialf(GL_FRONT, GL_SHININESS, 100)

        '①円柱上面（Z=height）

        GL.glBegin(GL_POLYGON)
        GL.glNormal3d(0.0, 0.0, 1.0) '法線ベクトル
        For i As Double = 0 To n - 1 Step 1  'n；n角形の頂点の数
            Dim A As Double = steps * i
            glVertex3d(r * 0.8 * Math.Cos(A), r * 0.8 * Math.Sin(A), height * 1.05)
        Next
        GL.glEnd()

        '②円柱下面（Z=0）※上面とは頂点を辿る順番が逆回りとする
        GL.glBegin(GL_POLYGON)
        '  GL.glColor3f(entset_point.color2.dbl_red, entset_point.color2.dbl_green, entset_point.color2.dbl_blue)
        GL.glNormal3d(0.0, 0.0, -1.0) '法線ベクトル
        For i As Double = n - 1 To 0 Step -1 'n；n角形の頂点の数
            Dim A As Double = steps * i
            glVertex3d(r * 0.8 * Math.Cos(A), r * 0.8 * Math.Sin(A), 0)
        Next
        GL.glEnd()

        '③円柱側面
        GL.glBegin(GL_QUAD_STRIP)
        '   GL.glColor3d(entset_point.color2.dbl_red, entset_point.color2.dbl_green, entset_point.color2.dbl_blue)
        For i As Double = 0 To n
            Dim A As Double = steps * i
            glNormal3d(r * 0.8 * Math.Cos(A), r * 0.8 * Math.Sin(A), 0) '法線ベクトル
            glVertex3f(r * 0.8 * Math.Cos(A), r * 0.8 * Math.Sin(A), height * 1.05)
            glVertex3f(r * 0.8 * Math.Cos(A), r * 0.8 * Math.Sin(A), 0)
        Next
        GL.glEnd()




        '13.1.31（修正）===========================================================================================

        ''13.1.31以前（修正前）====================================================================================
        ''レンズ
        'GL.glBegin(GL_POLYGON)
        'GL.glColor3f(entset_camera.color2.dbl_red, entset_camera.color2.dbl_green, entset_camera.color2.dbl_blue)
        'For j As Integer = 1 To k
        '    For i As Integer = 0 To n - 1
        '        glVertex3f(r * Math.Cos(2 * Pi / n * i), r * Math.Sin(2 * Pi / n * i), 0.01 + CDbl(0.06 / 100) * j)
        '    Next
        'Next
        'GL.glEnd()

        'GL.glBegin(GL_POLYGON)
        'GL.glColor3f(entset_camera.color2.dbl_red, entset_camera.color2.dbl_green, entset_camera.color2.dbl_blue)
        'For i As Integer = 0 To n - 1
        '    glVertex3f(r * Math.Cos(2 * Pi / n * i), r * Math.Sin(2 * Pi / n * i), 0.01 + 0.06)
        'Next

        'GL.glEnd()
        ''シャッター
        'GL.glBegin(GL_QUADS)
        'GL.glColor3f(entset_camera.color3.dbl_red, entset_camera.color3.dbl_green, entset_camera.color3.dbl_blue)
        'GL.glVertex3f(-0.07F + 0, 0.06F + 0, 0.0F + 0) '// top right (top)
        'GL.glVertex3f(-0.09F + 0, 0.06F + 0, 0.0F + 0) '// top left (top)
        'GL.glVertex3f(-0.09F + 0, 0.06F + 0, 0.008F + 0) '// bottom left (top)
        'GL.glVertex3f(-0.07F + 0, 0.06F + 0, 0.008F + 0) '// bottom right (top)

        'GL.glColor3f(entset_camera.color3.dbl_red, entset_camera.color3.dbl_green, entset_camera.color3.dbl_blue)
        'GL.glVertex3f(-0.07F + 0, 0.068F + 0, 0.0F + 0) '// top right (top)
        'GL.glVertex3f(-0.09F + 0, 0.068F + 0, 0.0F + 0) '// top left (top)
        'GL.glVertex3f(-0.09F + 0, 0.068F + 0, 0.008F + 0) '// bottom left (top)
        'GL.glVertex3f(-0.07F + 0, 0.068F + 0, 0.008F + 0) '// bottom right (top)

        'GL.glColor3f(entset_camera.color3.dbl_red, entset_camera.color3.dbl_green, entset_camera.color3.dbl_blue)
        'GL.glVertex3f(-0.07F + 0, 0.068F + 0, 0.008F + 0) '// top right (top)
        'GL.glVertex3f(-0.09F + 0, 0.068F + 0, 0.008F + 0) '// top left (top)
        'GL.glVertex3f(-0.09F + 0, 0.06F + 0, 0.008F + 0) '// bottom left (top)
        'GL.glVertex3f(-0.07F + 0, 0.06F + 0, 0.008F + 0) '// bottom right (top)

        'GL.glColor3f(entset_camera.color3.dbl_red, entset_camera.color3.dbl_green, entset_camera.color3.dbl_blue)
        'GL.glVertex3f(-0.07F + 0, 0.068F + 0, 0.0F + 0) '// top right (top)
        'GL.glVertex3f(-0.09F + 0, 0.068F + 0, 0.0F + 0) '// top left (top)
        'GL.glVertex3f(-0.09F + 0, 0.06F + 0, 0.008F + 0) '// bottom left (top)
        'GL.glVertex3f(-0.07F + 0, 0.06F + 0, 0.008F + 0) '// bottom right (top)

        'GL.glColor3f(entset_camera.color3.dbl_red, entset_camera.color3.dbl_green, entset_camera.color3.dbl_blue)
        'GL.glVertex3f(-0.09F + 0, 0.068F + 0, 0.008F + 0) '// top right (top)
        'GL.glVertex3f(-0.09F + 0, 0.068F + 0, 0.0F + 0) '// top left (top)
        'GL.glVertex3f(-0.09F + 0, 0.06F + 0, 0.0F + 0) '// bottom left (top)
        'GL.glVertex3f(-0.09F + 0, 0.06F + 0, 0.008F + 0) '// bottom right (top)

        'GL.glColor3f(entset_camera.color3.dbl_red, entset_camera.color3.dbl_green, entset_camera.color3.dbl_blue)
        'GL.glVertex3f(-0.07F + 0, 0.068F + 0, 0.008F + 0) '// top right (top)
        'GL.glVertex3f(-0.07F + 0, 0.068F + 0, 0.0F + 0) '// top left (top)
        'GL.glVertex3f(-0.07F + 0, 0.06F + 0, 0.0F + 0) '// bottom left (top)
        'GL.glVertex3f(-0.07F + 0, 0.06F + 0, 0.008F + 0) '// bottom right (top)

        'GL.glEnd()
        ''カメラ本体

        'GL.glBegin(GL_QUADS)    '// start drawing a quad
        'GL.glColor3f(entset_camera.color.dbl_red, entset_camera.color.dbl_green, entset_camera.color.dbl_blue)
        ''GL.glColor3f(0.0F, 1.0F, 0.0F) '// green top
        'GL.glVertex3f(0.1F + 0, 0.06F + 0, -0.02F + 0) '// top right (top)
        'GL.glVertex3f(-0.1F + 0, 0.06F + 0, -0.02F + 0) '// top left (top)
        'GL.glVertex3f(-0.1F + 0, 0.06F + 0, 0.02F + 0) '// bottom left (top)
        'GL.glVertex3f(0.1F + 0, 0.06F + 0, 0.02F + 0) '// bottom right (top)

        'GL.glColor3f(entset_camera.color.dbl_red, entset_camera.color.dbl_green, entset_camera.color.dbl_blue)
        ''GL.glColor3f(1.0F, 0.5F, 0.0F) '// orange
        'GL.glVertex3f(0.1F + 0, -0.06F + 0, 0.02F + 0) '// top right (bottom)
        'GL.glVertex3f(-0.1F + 0, -0.06F + 0, 0.02F + 0) '// top left (bottom)
        'GL.glVertex3f(-0.1F + 0, -0.06F + 0, -0.02F + 0) '// bottom left (bottom)
        'GL.glVertex3f(0.1F + 0, -0.06F + 0, -0.02F + 0) '// bottom right (bottom)

        'GL.glColor3f(entset_camera.color.dbl_red, entset_camera.color.dbl_green, entset_camera.color.dbl_blue)
        ''GL.glColor3f(1.0F, 0.0F, 0.0F) '// red
        'GL.glVertex3f(0.1F + 0, 0.06F + 0, 0.02F + 0) '// top right (front)
        'GL.glVertex3f(-0.1F + 0, 0.06F + 0, 0.02F + 0) '// top left (front)
        'GL.glVertex3f(-0.1F + 0, -0.06F + 0, 0.02F + 0) '// bottom left (front)
        'GL.glVertex3f(0.1F + 0, -0.06F + 0, 0.02F + 0) '// bottom right (front)

        'GL.glColor3f(entset_camera.color.dbl_red, entset_camera.color.dbl_green, entset_camera.color.dbl_blue)
        ''GL.glColor3f(1.0F, 1.0F, 0.0F)  '// yellow
        'GL.glVertex3f(-0.1F + 0, 0.06F + 0, -0.02F + 0) '// top right (back)
        'GL.glVertex3f(0.1F + 0, 0.06F + 0, -0.02F + 0) '// top left (back)
        'GL.glVertex3f(0.1F + 0, -0.06F + 0, -0.02F + 0) '// bottom left (back)
        'GL.glVertex3f(-0.1F + 0, -0.06F + 0, -0.02F + 0) '// bottom right (back)

        'GL.glColor3f(entset_camera.color.dbl_red, entset_camera.color.dbl_green, entset_camera.color.dbl_blue)
        ''GL.glColor3f(0.0F, 0.0F, 1.0F) '// blue
        'GL.glVertex3f(-0.1F + 0, 0.06F + 0, 0.02F + 0) '// top right (left)
        'GL.glVertex3f(-0.1F + 0, 0.06F + 0, -0.02F + 0) '// top left (left)
        'GL.glVertex3f(-0.1F + 0, -0.06F + 0, -0.02F + 0) '// bottom left (left)
        'GL.glVertex3f(-0.1F + 0, -0.06F + 0, 0.02F + 0) '// bottom right (left)

        'GL.glColor3f(entset_camera.color.dbl_red, entset_camera.color.dbl_green, entset_camera.color.dbl_blue)
        '' GL.glColor3f(1.0F, 0.0F, 1.0F) '// violett
        'GL.glVertex3f(0.1F + 0, 0.06F + 0, -0.02F + 0) '// top right (right)
        'GL.glVertex3f(0.1F + 0, 0.06F + 0, 0.02F + 0) '// top left (right)
        'GL.glVertex3f(0.1F + 0, -0.06F + 0, 0.02F + 0) '// bottom left (right)
        'GL.glVertex3f(0.1F + 0, -0.06F + 0, -0.02F + 0) '// bottom right (right)
        'GL.glEnd()
        ''13.1.31以前（修正前）====================================================================================
        glPopMatrix()
        glDisable(GL_COLOR_MATERIAL_PARAMETER)
    End Sub

    Public Function YCM_DrawRay(ByVal line As CRay, ByVal blnSelect As Boolean) As Integer
        YCM_DrawRay = 0
        glLineWidth(LineWidth_Ray)

        'glPushMatrix()
        glEnable(GL_LINE_STIPPLE)
        If blnSelect Then
            glLineStipple(1, 4369)
        Else
            glLineStipple(entset_ray.linetype.kyori, CUShort(Strings.Replace(entset_ray.linetype.pattern, "0x", "&H")))
        End If
        glBegin(GL_LINES)

        '--ins.rep.12.11.23
        Dim mcol As ModelColor
        mcol = YCM_GetColorInfoByCode(entset_ray.color.code)

        If blnSelect Then
            'glColor3f(1.0F, 1.0F, 1.0F) '13.2.1修正
            Dim Mat_Color1() As Single = {1.0F, 1.0F, 1.0F, 1.0}
            glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_Color1)

        Else
            '--rep.12.1123            glColor3f(entset_ray.color.dbl_red, entset_ray.color.dbl_green, entset_ray.color.dbl_blue)
            'glColor3f(mcol.dbl_red, mcol.dbl_green, mcol.dbl_blue)　'13.2.1修正
            Dim Mat_RayColor() As Single = {mcol.dbl_red, mcol.dbl_green, mcol.dbl_blue}
            glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_RayColor)

        End If
        glVertex3d(CSng(line.startPnt.x), CSng(line.startPnt.y), CSng(line.startPnt.z))


        If blnSelect Then
            'glColor3f(1.0F, 1.0F, 1.0F)'13.2.1修正
            Dim Mat_Color1() As Single = {1.0F, 1.0F, 1.0F, 1.0}
            glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_Color1)

        Else
            '--rep.12.11.23            glColor3f(entset_ray.color.dbl_red, entset_ray.color.dbl_green, entset_ray.color.dbl_blue)
            'glColor3f(mcol.dbl_red, mcol.dbl_green, mcol.dbl_blue)'13.2.1修正
            Dim Mat_RayColor() As Single = {mcol.dbl_red, mcol.dbl_green, mcol.dbl_blue}
            glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_RayColor)

        End If

        glVertex3d(CSng(line.endPnt.x), CSng(line.endPnt.y), CSng(line.endPnt.z))
        glEnd()
        glDisable(GL_LINE_STIPPLE)
        'glPopMatrix()
    End Function

    Public Function YCM_DrawUserLine(ByVal line As CUserLine, ByVal blnSelect As Boolean) As Integer
        YCM_DrawUserLine = 0
        'glLineWidth(LineWidth_Line * 5) '線分の幅


        glEnable(GL_LINE_STIPPLE)
        Dim mline As LineType '線分の線種の設定

        mline = YCM_GetLineTypeInfoByCode(line.lineTypeCode)
        If blnSelect Then
            glLineStipple(1, 4369) '0x1111 - 破線パターン0001000100010001
        Else
            glLineStipple(entset_line.linetype.kyori, CUShort(Strings.Replace(mline.pattern, "0x", "&H")))
            'glLineStipple(entset_line.linetype.kyori,  CUShort(Strings.Replace(entset_line.linetype.pattern, "0x", "&H")))
        End If
        glBegin(GL_LINES)
        Dim mcol As ModelColor '線分の色
        mcol = YCM_GetColorInfoByCode(line.colorCode)
        If blnSelect Then
            'glColor3f(1.0F, 1.0F, 1.0F)'13.2.1修正
            Dim Mat_Color1() As Single = {1.0F, 1.0F, 1.0F, 1.0}
            glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_Color1)
        Else
            '--rep.start-------------------
            'glColor3f(entset_line.color.dbl_red, entset_line.color.dbl_green, entset_line.color.dbl_blue)
            ' glColor3f(mcol.dbl_red, mcol.dbl_green, mcol.dbl_blue)'13.2.1修正
            Dim Mat_ScaleLineColor() As Single = {mcol.dbl_red, mcol.dbl_green, mcol.dbl_blue, 1.0}
            glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_ScaleLineColor)
            '--rep.end---------------------
        End If


        glVertex3d(CSng(line.startPnt.x), CSng(line.startPnt.y), CSng(line.startPnt.z))

        If blnSelect Then
            'glColor3f(1.0F, 1.0F, 1.0F)'13.2.1修正
            Dim Mat_Color1() As Single = {1.0F, 1.0F, 1.0F, 1.0}
            glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_Color1)
        Else
            '--rep.start-------------------
            'glColor3f(entset_line.color.dbl_red, entset_line.color.dbl_green, entset_line.color.dbl_blue)
            '  glColor3f(mcol.dbl_red, mcol.dbl_green, mcol.dbl_blue)'13.2.1修正
            Dim Mat_ScaleLineColor() As Single = {mcol.dbl_red, mcol.dbl_green, mcol.dbl_blue, 1.0}
            glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_ScaleLineColor)
            '--rep.end---------------------
        End If

        glVertex3d(CSng(line.endPnt.x), CSng(line.endPnt.y), CSng(line.endPnt.z))
        glEnd()
        glDisable(GL_LINE_STIPPLE)
    End Function

    '13.1.25　スケール設定を行った2点間に線分を作成
    '(SP1：1点目　，　SP2：2点目/13.1.28以前cp1,cp2)
    Public Sub YCM_GenScaleLine(ByVal SP1 As CLookPoint, ByVal SP2 As CLookPoint)

        If (SP1 Is Nothing Or SP2 Is Nothing) Then
            Exit Sub
        End If

        gScaleLine = New CUserLine()
        Call gScaleLine.SetStartPnt(SP1.x, SP1.y, SP1.z)
        Call gScaleLine.SetEndPnt(SP2.x, SP2.y, SP2.z)
        '線種と色を設定


        'Dim Mat_ScaleLineColor() As Single = {1.0, 0, 0, 0}
        'glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_ScaleLineColor)
        'glMaterialfv(GL_FRONT, GL_DIFFUSE, {0, 0, 0, 0})
        'glMaterialfv(GL_FRONT, GL_SPECULAR, {0, 0, 0, 0})
        'glMaterialf(GL_FRONT, GL_SHININESS, 0)


        gScaleLine.colorCode = 2 '線の色：2=赤
        gScaleLine.lineTypeCode = 1 '線種：1=実線（2=破線）

        'gScaleLine.lineWidth = 10.0F
        'glLineWidth(Scale_LineWidth_Line) '線分の幅

    End Sub

    '★20121029シングルターゲットについて
    Public Function YCM_DrawLookPoint(ByVal lookpoint As CLookPoint, ByVal blnSelect As Boolean) As Integer
        YCM_DrawLookPoint = 0
        'SUSANO ADD START 20160901
        'CTの補助点を表示しない
        If lookpoint.flgLabel = 0 Then
            YCM_DrawLookPoint = 1
            Exit Function
        End If
        'SUSANO ADD END 20160901
        If (lookpoint.type > 2) Then
            If (entset_pointUser.blnVisiable = True) Then
                YCM_DrawLookPoint = drawLookPointSolid(lookpoint, blnSelect) 'Solid(任意追加点)
                YCM_DrawLookPoint = 1
            End If
        Else
            'If (entset_point.blnVisiable = True) Then'13.1.8コメントに
            If (gDrawLookPointType = 1) Then
                YCM_DrawLookPoint = drawLookPointSolid(lookpoint, blnSelect) '①Solid
            ElseIf (gDrawLookPointType = 2) Then
                YCM_DrawLookPoint = drawLookPointCross(lookpoint, blnSelect) '②Cross
            Else
                YCM_DrawLookPoint = drawLookPointPlane(lookpoint, blnSelect) '③ターゲット面+中心円
            End If
            YCM_DrawLookPoint = 1
            'End If
        End If
    End Function
    '★20121029③ターゲット面+中心円
    Public Function drawLookPointPlane(ByVal lookpoint As CLookPoint, ByVal blnSelect As Boolean) As Integer
        ''★20121029山田追加
        Dim entset As EntSetting
        If (lookpoint.posType = 0) Then
            entset = entset_point '計測点
        Else
            '===================================================================================================20121115
            ''★20121114変更
            'Dim entset As EntSetting
            'If (lookpoint.posType = 0) Then
            '    entset = entset_point1 '★20121114計測点(ターゲット面)
            '    entset = entset_point2 '★20121114計測点(中心円)
            '    entset = entset_point3 '★20121114計測点(十字線)
            'Else
            '===================================================================================================20121115
            entset = entset_pointUser '追加計測点（任意追加点）

        End If
        glLineWidth(1.0F)
        If blnSelect Then
            glEnable(GL_LINE_STIPPLE) '線の描画の際に現在の線スティプルパターンが用いられる
            glLineStipple(1, 4369)
        End If
        Dim k_mat As Integer = 4
        Dim x As Double, y As Double, z As Double
        ' Dim anglex As Double, angley As Double, anglez As Double
        Dim mat_lib As New MatLib
        Dim resultM(k_mat, k_mat) As Double
        Dim resultMNew(k_mat * k_mat) As Double
        Dim rotateXM(k_mat, k_mat) As Double, rotateYM(k_mat, k_mat) As Double, rotateZM(k_mat, k_mat) As Double
        Dim moveM(k_mat, k_mat) As Double
        Dim n As Integer = 10 '多角形の角の数
        Dim Pi As Double = 3.1415926536
        Dim k As Integer = 100

        '★20121031座標変換修正
        'Dim r2 As Double
        'r2 = (1.0# / 2.0#) * dl '十字線の1辺の半分の長さ

        'Dim t As Double = 0.01 'Planeから中心円・十字線までの離れ(ターゲットの表裏を区別)
        x = lookpoint.x : y = lookpoint.y : z = lookpoint.z

        '20121119平均方向(法線ベクトル)の回転軸となるベクトルDと回転角R
        Dim R As Double
        Dim cVx As Double
        Dim cVy As Double
        Dim cVz As Double
        Dim vec1 As New GeoVector
        Dim vec2 As New GeoVector
        Dim VVector As New GeoVector
        cVx = lookpoint.cVx
        cVy = lookpoint.cVy
        cVz = lookpoint.cVz

        'R = Math.Acos(((cVx * cVx) + (cVy * cVy)) / ((cVx * cVx * cVx * cVx) + (cVy * cVy * cVy * cVy) + (2 * cVx * cVx * cVy * cVy) + (cVx * cVx * cVz * cVz) + (cVy * cVy * cVz * cVz)) ^ 0.5)
        'Dx = ((cVy * cVy) / ((cVx * cVx) + (cVy * cVy))) ^ 0.5
        'Dy = ((cVx * cVx) / ((cVx * cVx) + (cVy * cVy))) ^ 0.

        '20121119スーリさん
        '  glEnable(GL_COLOR_MATERIAL)
        vec1.setXYZ(cVx, cVy, cVz) '平均ベクトル(単位ベクトル)
        vec2.setXYZ(0, 0, 1.0F) 'Z軸(単位ベクトル)
        VVector = vec2.GetOuterProduct(vec1) '外積を得る(GeoVector参照)Z軸⇒平均ベクトルへ
        R = Math.Acos(vec1.GetInnerProduct(vec2) / (vec1.GetLength * vec2.GetLength)) '回転角算出(内積：GeoVector参照)

        '★20121030以前　CV○はカメラの方向(平均)
        drawLookPointPlane = 0
        'glEnable(GL_DEPTH_TEST) '陰面消去
        'glEnable(GL_CULL_FACE) '背面消去
        'glClearColor(0, 0, 0, 1.0F)
        'glClear(GL_COLOR_BUFFER_BIT Or GL_DEPTH_BUFFER_BIT)
        glPushMatrix() 'モデルビュー変換行列の保存

        'glMultMatrixd(resultMNew)

        '20121119スーリさん
        'RadianからDegreeにする。

        R = R * 180 / Pi
        glTranslated(x, y, z) '平均移動

        glRotated(R, VVector.x, VVector.y, VVector.z) '回転

        '拡大縮小

        glScalef(10 * entset.screensize / 100.0 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom), _
                   10.0 * entset.screensize / 100.0 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom), _
                   10.0 * entset.screensize / 100.0 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom))
        '----------------------------------------------------------------------------------------------------
        '○計測点(ターゲット面)
        '20121127まで=======================================================================================================================
        ''座標変換後計測点LookPointを原点とみなして(0±dl,0±dl,0)
        ''Z方向はターゲットの厚さ方向

        'GL.glBegin(GL_QUADS)    '// start drawing a quad
        'GL.glColor3f(entset_point.color.dbl_red, entset_point.color.dbl_green, entset_point.color.dbl_blue) '★20121114計測点(ターゲット面)
        'GL.glNormal3f(lookpoint.cVx, lookpoint.cVy, lookpoint.cVz)
        'GL.glVertex3f(dl, dl, 0) '// top right
        'GL.glVertex3f(-dl, dl, 0) '// top left
        'GL.glVertex3f(-dl, -dl, 0) '// bottom left
        'GL.glVertex3f(+dl, -dl, 0) '// bottom right
        'GL.glEnd()
        '20121127まで=======================================================================================================================

        '○計測点(ターゲット面)=======カメラ参考

        Dim dl As Double 'ターゲットの形状(1辺の長さの半分)
        dl = 0.5F ' 0.5 * entset.screensize / 100 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom)
        Dim t As Double = 0.1F * dl 'ターゲット面の厚さ (以前は Dim dt As Double =-0.2 * dl)

        Dim Mat_Color1() As Single = {entset_point.color.dbl_red, entset_point.color.dbl_green, entset_point.color.dbl_blue, 1.0}

        glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_Color1)
        glMaterialfv(GL_FRONT, GL_DIFFUSE, {0.26, 0.26, 0.26, 1.0})
        glMaterialfv(GL_FRONT, GL_SPECULAR, {1.0, 1.0, 1.0, 1.0})
        glMaterialf(GL_FRONT, GL_SHININESS, 30)
        '①Top（y=dl）

        GL.glBegin(GL_QUADS) '20121128
        ' GL.glColor3f(entset_point.color.dbl_red, entset_point.color.dbl_green, entset_point.color.dbl_blue)
        GL.glNormal3f(0.0, 1.0, 0.0) '法線ベクトル
        GL.glVertex3f(dl, dl, -t) 'top right
        GL.glVertex3f(-dl, dl, -t) 'top left
        GL.glVertex3f(-dl, dl, t) 'bottom left
        GL.glVertex3f(dl, dl, t) 'bottom right
        GL.glEnd()

        '②Bottom（y=-dl）

        GL.glBegin(GL_QUADS) '20121128
        ' GL.glColor3f(entset_point.color.dbl_red, entset_point.color.dbl_green, entset_point.color.dbl_blue)
        GL.glNormal3f(0.0, -1.0, 0.0) '法線ベクトル
        GL.glVertex3f(dl, -dl, t) 'top right
        GL.glVertex3f(-dl, -dl, t) 'top left
        GL.glVertex3f(-dl, -dl, -t) 'bottom lef
        GL.glVertex3f(dl, -dl, -t) 'bottom righ
        GL.glEnd()

        '③Front（z=t）

        GL.glBegin(GL_QUADS) '20121128
        '  GL.glColor3f(entset_point.color.dbl_red, entset_point.color.dbl_green, entset_point.color.dbl_blue)
        GL.glNormal3f(0.0, 0.0, 1.0) '法線ベクトル
        GL.glVertex3f(dl, dl, t) 'top right
        GL.glVertex3f(-dl, dl, t) 'top left
        GL.glVertex3f(-dl, -dl, t) 'bottom left
        GL.glVertex3f(dl, -dl, t) 'bottom right
        GL.glEnd()

        '④Back（z=-t）

        GL.glBegin(GL_QUADS) '20121128
        '  GL.glColor3f(entset_point.color.dbl_red, entset_point.color.dbl_green, entset_point.color.dbl_blue)
        GL.glNormal3f(0.0, 0.0, -1.0) '法線ベクトル
        GL.glVertex3f(-dl, dl, -t) 'top right
        GL.glVertex3f(dl, dl, -t) 'top left
        GL.glVertex3f(dl, -dl, -t) 'bottom left
        GL.glVertex3f(-dl, -dl, -t) 'bottom right
        GL.glEnd()

        '⑤Left（x=-dl）

        GL.glBegin(GL_QUADS) '20121128
        '  GL.glColor3f(entset_point.color.dbl_red, entset_point.color.dbl_green, entset_point.color.dbl_blue)
        GL.glNormal3f(-1.0, 0.0, 0.0) '法線ベクトル
        GL.glVertex3f(-dl, dl, t) 'top right
        GL.glVertex3f(-dl, dl, -t) 'top left
        GL.glVertex3f(-dl, -dl, -t) 'bottom left
        GL.glVertex3f(-dl, -dl, t) 'bottom right
        GL.glEnd()

        '⑥right（x=dl）

        GL.glBegin(GL_QUADS) '20121128
        '   GL.glColor3f(entset_point.color.dbl_red, entset_point.color.dbl_green, entset_point.color.dbl_blue)
        GL.glNormal3f(1.0, 0.0, 0.0) '法線ベクトル
        GL.glVertex3f(dl, dl, -t) 'top right
        GL.glVertex3f(dl, dl, t) 'top left
        GL.glVertex3f(dl, -dl, t) 'bottom left
        GL.glVertex3f(dl, -dl, -t) 'bottom right
        GL.glEnd()
        '13.1.22============================================================================================================================



        '----新20121114
        ''-----------------------------------------------------------------------------------------------------
        ''★20121030以前　ターゲット面はX-Y平面(古)
        'glBegin(GL_QUADS)
        'If blnSelect Then
        '    glColor3f(1.0F, 1.0F, 1.0F)
        'Else
        '    glColor3f(entset.color.dbl_red, entset.color.dbl_green, entset.color.dbl_blue)
        'End If
        ''Dim dl As Double
        ''dl = 0.5 * entset.screensize / 100 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom)
        'glVertex3d(CSng(lookpoint.x - dl), CSng(lookpoint.y - dl), CSng(lookpoint.z))
        'glVertex3d(CSng(lookpoint.x - dl), CSng(lookpoint.y + dl), CSng(lookpoint.z))
        'glVertex3d(CSng(lookpoint.x + dl), CSng(lookpoint.y + dl), CSng(lookpoint.z))
        'glVertex3d(CSng(lookpoint.x + dl), CSng(lookpoint.y - dl), CSng(lookpoint.z))
        'glBegin(GL_QUADS)
        'GL.glEnd()
        'glPopMatrix()
        'glDisable(GL_LINE_STIPPLE)
        ''-----------------------------------------------------------------------------------------------------

        '○計測点(中心円)=======
        '20121127まで=======================================================================================================================
        'GL.glBegin(GL_POLYGON)
        '  GL.glColor3f(entset_point.color2.dbl_red, entset_point.color2.dbl_green, entset_point.color2.dbl_blue) '★20121115計測点(中心円)
        'For i As Integer = 0 To n - 1
        '    glVertex3f(r1 * Math.Cos(2 * Pi / n * i), r1 * Math.Sin(2 * Pi / n * i), t)
        'Next
        ' glutSolidSphere(dt * 2, 20, 20)
        '   GL.glEnd()
        '20121127まで=======================================================================================================================


        ''○計測点(中心円2013.1.18以前)=======================================================================================================================
        'Dim rt As Double = r1 * 0.1 '中心円（円柱）の半径

        'Dim height As Double = r1 * 0.2 '中心円（円柱）の厚さ
        'GL.glBegin(GL_POLYGON)
        'GL.glColor3f(entset_point.color2.dbl_red, entset_point.color2.dbl_green, entset_point.color2.dbl_blue)
        'k = 5
        'For j As Integer = 1 To k '円柱の厚さ（高さ）方向の分割
        '    For i As Integer = 0 To n - 1
        '        glVertex3f(r1 * Math.Cos(2 * Pi / n * i), r1 * Math.Sin(2 * Pi / n * i), CDbl(rt) * j)
        '    Next
        'Next
        ''GL.glEnd()
        ''GL.glBegin(GL_POLYGON)
        ''GL.glColor3f(entset_point.color2.dbl_red, entset_point.color2.dbl_green, entset_point.color2.dbl_blue)
        'For i As Integer = 0 To n - 1
        '    glVertex3f(r1 * Math.Cos(2 * Pi / n * i), r1 * Math.Sin(2 * Pi / n * i), rt)
        'Next
        'GL.glEnd()
        'glDisable(GL_LINE_STIPPLE)
        ''○計測点(中心円2013.1.18以前)=======================================================================================================================

        '○計測点(中心円13.1.23)=======================================================================================================================
        Dim MatColor2() As Single = {entset_point.color2.dbl_red, entset_point.color2.dbl_green, entset_point.color2.dbl_blue, 1.0}
        glMaterialfv(GL_FRONT, GL_AMBIENT, MatColor2)

        Dim r1 As Double
        r1 = (1.0# / 2.0#) * dl
        'Dim rt As Double = r1 * 0.1 '中心円（円柱）の半径

        Dim height As Double = t * 3.0F '中心円（円柱）の厚さ
        Dim steps As Double = 2 * Pi / n '角の数
        '①円柱上面（Z=height）

        GL.glBegin(GL_POLYGON)
        '  GL.glColor3d(entset_point.color2.dbl_red, entset_point.color2.dbl_green, entset_point.color2.dbl_blue)
        GL.glNormal3d(0.0, 0.0, 1.0) '法線ベクトル
        For i As Double = 0 To n - 1 Step 1  'n；n角形の頂点の数
            Dim A As Double = steps * i
            glVertex3d(r1 * Math.Cos(A), r1 * Math.Sin(A), height)
        Next
        GL.glEnd()

        '②円柱下面（Z=0）※上面とは頂点を辿る順番が逆回りとする
        GL.glBegin(GL_POLYGON)
        '  GL.glColor3f(entset_point.color2.dbl_red, entset_point.color2.dbl_green, entset_point.color2.dbl_blue)
        GL.glNormal3d(0.0, 0.0, -1.0) '法線ベクトル
        For i As Double = n - 1 To 0 Step -1 'n；n角形の頂点の数
            Dim A As Double = steps * i
            glVertex3d(r1 * Math.Cos(A), r1 * Math.Sin(A), 0)
        Next
        GL.glEnd()

        '③円柱側面
        GL.glBegin(GL_QUAD_STRIP)
        '   GL.glColor3d(entset_point.color2.dbl_red, entset_point.color2.dbl_green, entset_point.color2.dbl_blue)
        For i As Double = 0 To n
            Dim A As Double = steps * i
            glNormal3d(r1 * Math.Cos(A), r1 * Math.Sin(A), 0) '法線ベクトル
            glVertex3f(r1 * Math.Cos(A), r1 * Math.Sin(A), height)
            glVertex3f(r1 * Math.Cos(A), r1 * Math.Sin(A), 0)
        Next
        GL.glEnd()

        ' glDisable(GL_LINE_STIPPLE)
        '○計測点(中心円13.1.23)=======================================================================================================================

        ''○計測点(中心円13.1.18)=======================================================================================================================
        'Dim r1 As Double
        'r1 = (1.0# / 2.0#) * dl
        ''Dim rt As Double = r1 * 0.1 '中心円（円柱）の半径

        ''Dim height As Double = r1 '中心円（円柱）の厚さ
        ''①円柱下面（Z=0）

        'glNormal3d(0.0, 0.0, -1.0) '法線ベクトル
        'GL.glBegin(GL_POLYGON)
        'GL.glColor3f(entset_point.color2.dbl_red, entset_point.color2.dbl_green, entset_point.color2.dbl_blue)
        'For i As Double = n To 0 Step -1 'n；n角形の頂点の数
        '    Dim A As Double = 2 * Pi / n * i
        '    glVertex3f(r1 * Math.Cos(A), r1 * Math.Sin(A), 0)
        'Next
        'GL.glEnd()
        ''②円柱側面
        'GL.glBegin(GL_QUAD_STRIP)
        'GL.glColor3d(entset_point.color2.dbl_red, entset_point.color2.dbl_green, entset_point.color2.dbl_blue)
        'For i As Double = 0 To n
        '    Dim A As Double = 2 * Pi / n * i
        '    glNormal3d(r1 * Math.Cos(A), r1 * Math.Sin(A), 0)

        '    glVertex3d(r1 * Math.Cos(A), r1 * Math.Sin(A), 0)
        '    glVertex3d(r1 * Math.Cos(A), r1 * Math.Sin(A), t * 10.0F)
        'Next
        'GL.glEnd()
        ''③円柱上面（Z=height）

        'glNormal3d(0.0, 0.0, 1.0) '法線ベクトル
        'GL.glBegin(GL_POLYGON)
        'GL.glColor3d(entset_point.color2.dbl_red, entset_point.color2.dbl_green, entset_point.color2.dbl_blue)
        'For i As Double = 0 To n - 1 Step 1  'n；n角形の頂点の数
        '    Dim A As Double = 2 * Pi / n * i
        '    glVertex3d(r1 * Math.Cos(A), r1 * Math.Sin(A), t * 10.0F)
        'Next
        'GL.glEnd()
        '' glDisable(GL_LINE_STIPPLE)
        ''○計測点(中心円13.1.18)=======================================================================================================================


        ''-----------------------------------------------------------------------------------------------------
        ''○計測点(十字線)==========後々コメントに
        'GL.glBegin(GL_LINES)
        'GL.glColor3f(entset_point.color3.dbl_red, entset_point.color3.dbl_green, entset_point.color3.dbl_blue) '★20121115計測点(十字線)
        'glVertex3d(0, r2, t) 'TOP
        'glVertex3d(0, -r2, t) 'BUTTOM
        'glVertex3d(r2, 0, t) 'RIGHT
        'glVertex3d(-r2, 0, t) 'LEFT
        'GL.glEnd()
        glPopMatrix() 'モデルビュー変換行列の復帰
        glDisable(GL_COLOR_MATERIAL)
        glFlush()
        ''-----------------------------------------------------------------------------------------------------
        ' ''○平均方向へラインを表示
        ''GL.glBegin(GL_LINES)
        ''GL.glColor3f(entset_pointUser.color.dbl_red, entset_pointUser.color.dbl_green, entset_pointUser.color.dbl_blue)
        ''glVertex3d(lookpoint.x, lookpoint.y, lookpoint.z) '始点：計測点
        ''glVertex3d(lookpoint.x + (lookpoint.cVx * 3), lookpoint.y + (lookpoint.cVy * 3), lookpoint.z + (lookpoint.cVz * 3)) '終点：平均ベクトル(単位)方向の点
        ''GL.glEnd()
        ' glDisable(GL_LINE_STIPPLE)


    End Function
    '①Solid
    Private Function drawLookPointSolid(ByVal lookpoint As CLookPoint, ByVal blnSelect As Boolean) As Integer
        If blnSelect Then
            'glColor3f(1.0F, 1.0F, 1.0F) '13.2.1修正
            Dim Mat_Color1() As Single = {1.0F, 1.0F, 1.0F, 1.0}
            glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_Color1)
        Else
            If (lookpoint.posType = 0) Then
                'glColor3f(entset_point.color.dbl_red, entset_point.color.dbl_green, entset_point.color.dbl_blue) '13.2.1修正
                Dim Mat_lookpointColor1() As Single = {entset_point.color.dbl_red, entset_point.color.dbl_green, entset_point.color.dbl_blue, 1.0}
                glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_lookpointColor1)
            Else
                'glColor3f(entset_pointUser.color.dbl_red, entset_pointUser.color.dbl_green, entset_pointUser.color.dbl_blue) '13.2.1修正
                Dim Mat_entset_pointUserColor1() As Single = {entset_pointUser.color.dbl_red, entset_pointUser.color.dbl_green, entset_pointUser.color.dbl_blue, 1.0}
                glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_entset_pointUserColor1)
            End If
        End If
        glPushMatrix()
        Dim spher_color() As Single = {0.0, 0.8F, 0.8F, 1.0}
        Dim spher_specl() As Single = {0, 0, 0, 1}
        '====================================================
        'glColorMaterial(GL_FRONT_AND_BACK, GL_AMBIENT_AND_DIFFUSE)
        'glEnable(GL_COLOR_MATERIAL)
        'glMaterialfv(GL_FRONT, GL_DIFFUSE, spher_color)
        'glMaterialfv(GL_FRONT, GL_SPECULAR, spher_specl)
        'glMaterialf(GL_FRONT, GL_SHININESS, 100.0)
        '====================================================
        glTranslatef(lookpoint.x, lookpoint.y, lookpoint.z)
        'glutSolidSphereとglutWireSphere はそれぞれソリッドの球，ワイヤフレームの球を描画します．

        glutSolidSphere(entset_point.screensize / 25 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom), 20, 20) '計測点
        'glutSolidSphere(entset_pointUser.screensize / 20 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom), 20, 20) '計測点
        glPopMatrix()
        'glPopMatrix()
        'glFlush()
    End Function
    '②Cross
    '13.2.1　山田　修正<<glColor3f()⇒glMaterialfv()>>は保留
    Public Function drawLookPointCross(ByVal lookpoint As CLookPoint, ByVal blnSelect As Boolean) As Integer
        drawLookPointCross = 0
        Dim entset As EntSetting
        If (lookpoint.posType = 0) Then
            entset = entset_point '計測点
        Else
            entset = entset_pointUser
        End If
        glLineWidth(1.0F)
        If blnSelect Then
            glEnable(GL_LINE_STIPPLE)
            glLineStipple(1, 4369)
        End If

        glBegin(GL_LINES)
        If blnSelect Then
            glColor3f(1.0F, 1.0F, 1.0F)
        Else
            glColor3f(entset.color.dbl_red, entset.color.dbl_green, entset.color.dbl_blue)
        End If
        glVertex3d(CSng(lookpoint.x - 0.5 * entset.screensize / 100 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom)), CSng(lookpoint.y), CSng(lookpoint.z))
        If blnSelect Then
            glColor3f(1.0F, 1.0F, 1.0F)
        Else
            glColor3f(entset.color.dbl_red, entset.color.dbl_green, entset.color.dbl_blue)
        End If
        glVertex3d(CSng(lookpoint.x + 0.5 * entset.screensize / 100 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom)), CSng(lookpoint.y), CSng(lookpoint.z))
        glEnd()

        glBegin(GL_LINES)

        If blnSelect Then
            glColor3f(1.0F, 1.0F, 1.0F)
        Else
            glColor3f(entset.color.dbl_red, entset.color.dbl_green, entset.color.dbl_blue)
        End If
        glVertex3d(CSng(lookpoint.x), CSng(lookpoint.y - 0.5 * entset.screensize / 100 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom)), CSng(lookpoint.z))

        If blnSelect Then
            glColor3f(1.0F, 1.0F, 1.0F)
        Else
            glColor3f(entset.color.dbl_red, entset.color.dbl_green, entset.color.dbl_blue)
        End If
        glVertex3d(CSng(lookpoint.x), CSng(lookpoint.y + 0.5 * entset.screensize / 100 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom)), CSng(lookpoint.z))
        glEnd()

        glBegin(GL_LINES)

        If blnSelect Then
            glColor3f(1.0F, 1.0F, 1.0F)
        Else
            glColor3f(entset.color.dbl_red, entset.color.dbl_green, entset.color.dbl_blue)
        End If
        glVertex3d(CSng(lookpoint.x), CSng(lookpoint.y), CSng(lookpoint.z - 0.5 * entset.screensize / 100 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom)))
        If blnSelect Then
            glColor3f(1.0F, 1.0F, 1.0F)
        Else
            glColor3f(entset.color.dbl_red, entset.color.dbl_green, entset.color.dbl_blue)
        End If
        glVertex3d(CSng(lookpoint.x), CSng(lookpoint.y), CSng(lookpoint.z + 0.5 * entset.screensize / 100 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom)))
        glEnd()
        glDisable(GL_LINE_STIPPLE)
    End Function

    Public Function YCM_DrawCircle(ByVal gppoint As Ccircle, ByVal blnSelect As Boolean) As Integer
        YCM_DrawCircle = 0
        Dim n As Integer = 20
        Dim r As Double = gppoint.r
        Dim Pi As Double = 3.1415926536
        Dim k As Integer = 100
        Dim dblLinelength As Double = 0.02
        Dim k_mat As Integer = 4
        Dim mat_lib As New MatLib
        Dim resultM(k_mat, k_mat) As Double
        Dim resultMNew(k_mat * k_mat) As Double
        Dim rotateXM(k_mat, k_mat) As Double, rotateYM(k_mat, k_mat) As Double, rotateZM(k_mat, k_mat) As Double
        Dim moveM(k_mat, k_mat) As Double
        rotateXM = mat_lib.GetRoateYMat(gppoint.x_angle)
        rotateYM = mat_lib.GetRoateXMat(gppoint.y_angle)
        rotateZM = mat_lib.GetRoateXMat(0)
        moveM = mat_lib.GetMoveMat(0.0, 0.0, 0.0)
        resultM = mat_lib.Multiply(rotateZM, rotateYM)
        resultM = mat_lib.Multiply(resultM, rotateXM)
        resultM = mat_lib.Multiply(resultM, moveM)
        resultMNew = mat_lib.Mat2OpenglMat(resultM)
        glMatrixMode(GL_MODELVIEW)
        glEnable(GL_LINE_STIPPLE)
        If blnSelect Then
            glLineStipple(1, 4369)
        Else
            glLineStipple(entset_circle.linetype.kyori, CUShort(Strings.Replace(entset_circle.linetype.pattern, "0x", "&H")))
        End If

        glPushMatrix()
        glMultMatrixd(resultMNew)

        glBegin(GL_LINE_LOOP)
        For ii As Integer = 0 To 1000
            glVertex3d(gppoint.org.x + r * Math.Cos(2 * Pi / 1000 * ii), gppoint.org.y + r * Math.Sin(2 * Pi / 1000 * ii), gppoint.org.z)
        Next
        glEnd()
        glPopMatrix()
        glDisable(GL_LINE_STIPPLE)
    End Function
    Public Function YCM_DrawCircleNew(ByVal gppoint As Ccircle, ByVal blnSelect As Boolean) As Integer
        YCM_DrawCircleNew = 0
        Dim n As Integer = 20
        Dim r As Double = gppoint.r
        Dim Pi As Double = 3.1415926536
        Dim k As Integer = 100
        Dim dblLinelength As Double = 0.02
        Dim k_mat As Integer = 4
        Dim mat_lib As New MatLib
        Dim resultM(k_mat, k_mat) As Double
        Dim resultMNew(k_mat * k_mat) As Double
        Dim rotateXM(k_mat, k_mat) As Double, rotateYM(k_mat, k_mat) As Double, rotateZM(k_mat, k_mat) As Double
        Dim moveM(k_mat, k_mat) As Double
        rotateXM = mat_lib.GetRoateXMat(gppoint.x_angle)
        rotateYM = mat_lib.GetRoateYMat(gppoint.y_angle)
        rotateZM = mat_lib.GetRoateXMat(0)
        moveM = mat_lib.GetMoveMat(gppoint.org.x, gppoint.org.y, gppoint.org.z)
        resultM = mat_lib.Multiply(rotateZM, rotateYM)
        resultM = mat_lib.Multiply(resultM, rotateXM)
        resultM = mat_lib.Multiply(resultM, moveM)
        resultMNew = mat_lib.Mat2OpenglMat(resultM)
        glMatrixMode(GL_MODELVIEW)
        glEnable(GL_LINE_STIPPLE)


        ' vec1.setXYZ(gppoint.Vec.x, gppoint.Vec.y, gppoint.Vec.z) '平均ベクトル(単位ベクトル)
        Dim vec2 As New GeoVector
        Dim VVector As New GeoVector
        Dim rR As Double = 0

        vec2.setXYZ(0, 0, 1.0F) 'Z軸(単位ベクトル)
        VVector = vec2.GetOuterProduct(gppoint.Vec) '外積を得る(GeoVector参照)Z軸⇒平均ベクトルへ
        rR = Math.Acos(gppoint.Vec.GetInnerProduct(vec2) / (gppoint.Vec.GetLength * vec2.GetLength)) '回転角算出(内積：GeoVector参照)


        'RadianからDegreeにする。

        rR = rR * 180 / Pi

        glPushMatrix() 'モデルビュー変換行列の保存

        glTranslated(gppoint.org.x, gppoint.org.y, gppoint.org.z) '平均移動

        glRotated(rR, VVector.x, VVector.y, VVector.z) '回転



        Dim mline As LineType '20121127円の線種の設定

        mline = YCM_GetLineTypeInfoByCode(gppoint.lineTypeCode)
        If blnSelect Then
            glLineStipple(1, 4369)
        Else
            glLineStipple(entset_circle.linetype.kyori, CUShort(Strings.Replace(mline.pattern, "0x", "&H")))
            'glLineStipple(entset_circle.linetype.kyori, CUShort(Strings.Replace(entset_circle.linetype.pattern, "0x", "&H")))
        End If
        ' glPushMatrix()
        ' glMultMatrixd(resultMNew)

        '--rep.start-------------------
        Dim mcol As ModelColor
        mcol = YCM_GetColorInfoByCode(gppoint.colorCode)
        '--rep.end---------------------
        glBegin(GL_LINE_LOOP)
        For ii As Integer = 0 To 1000
            If blnSelect Then
                'glColor3f(1.0F, 1.0F, 1.0F) '13.2.1修正
                Dim Mat_Color1() As Single = {1.0F, 1.0F, 1.0F, 1.0}
                glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_Color1)
            Else
                '--rep.   glColor3f(entset_circle.color.dbl_red, entset_circle.color.dbl_green, entset_circle.color.dbl_blue)
                'glColor3f(mcol.dbl_red, mcol.dbl_green, mcol.dbl_blue) '13.2.1修正
                Dim Mat_gppointColor() As Single = {mcol.dbl_red, mcol.dbl_green, mcol.dbl_blue}
                glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_gppointColor)
            End If
            glVertex3d(r * Math.Cos(2.0# * Pi / 1000.0# * ii), r * Math.Sin(2.0# * Pi / 1000.0# * ii), 0.0)
        Next
        glEnd()
        glPopMatrix()
        glDisable(GL_LINE_STIPPLE)
    End Function
    Public Function YCM_DrawCirclepoint(ByVal gppoint As CcirclePoint, ByVal xxangle As Double, ByVal yyangle As Double, ByVal blnSelect As Boolean) As Integer
        YCM_DrawCirclepoint = 0
        Dim dblLinelength As Double = 0.02
        Dim k_mat As Integer = 4
        Dim mat_lib As New MatLib
        Dim resultM(k_mat, k_mat) As Double
        Dim resultMNew(k_mat * k_mat) As Double
        Dim rotateXM(k_mat, k_mat) As Double, rotateYM(k_mat, k_mat) As Double, rotateZM(k_mat, k_mat) As Double
        Dim moveM(k_mat, k_mat) As Double
        rotateXM = mat_lib.GetRoateYMat(yyangle)
        rotateYM = mat_lib.GetRoateXMat(-xxangle)
        rotateZM = mat_lib.GetRoateXMat(0)
        moveM = mat_lib.GetMoveMat(0.0, 0.0, 0.0)
        resultM = mat_lib.Multiply(rotateZM, rotateYM)
        resultM = mat_lib.Multiply(resultM, rotateXM)
        resultM = mat_lib.Multiply(resultM, moveM)
        resultMNew = mat_lib.Mat2OpenglMat(resultM)
        glMatrixMode(GL_MODELVIEW)
        glPushMatrix()
        glMultMatrixd(resultMNew)
        glLineWidth(1.0F)
        glEnable(GL_LINE_STIPPLE)
        If blnSelect Then
            glLineStipple(1, 4369)
        Else
            glLineStipple(entset_circle.linetype.kyori, CUShort(Strings.Replace(entset_circle.linetype.pattern, "0x", "&H")))
        End If

        glBegin(GL_LINES)
        'glColor3f(1.0F, 1.0F, 1.0F) '13.2.1修正
        Dim Mat_Color1() As Single = {1.0F, 1.0F, 1.0F, 1.0}
        glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_Color1)
        glVertex3d(CSng(gppoint.x - dblLinelength), CSng(gppoint.y), CSng(gppoint.z))
        'glColor3f(1.0F, 1.0F, 1.0F) '13.2.1修正
        glVertex3d(CSng(gppoint.x + dblLinelength), CSng(gppoint.y), CSng(gppoint.z))
        glEnd()

        glBegin(GL_LINES)
        'glColor3f(1.0F, 1.0F, 1.0F) '13.2.1修正
        glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_Color1)
        glVertex3d(CSng(gppoint.x), CSng(gppoint.y - dblLinelength), CSng(gppoint.z))
        'glColor3f(1.0F, 1.0F, 1.0F) '13.2.1修正
        glVertex3d(CSng(gppoint.x), CSng(gppoint.y + dblLinelength), CSng(gppoint.z))
        glEnd()

        glBegin(GL_LINES)
        'glColor3f(1.0F, 1.0F, 1.0F) '13.2.1修正
        glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_Color1)
        glVertex3d(CSng(gppoint.x), CSng(gppoint.y), CSng(gppoint.z - dblLinelength))
        'glColor3f(1.0F, 1.0F, 1.0F) '13.2.1修正
        glVertex3d(CSng(gppoint.x), CSng(gppoint.y), CSng(gppoint.z + dblLinelength))
        glEnd()

        glPopMatrix()
        glDisable(GL_LINE_STIPPLE)
    End Function

    'Public Function YCM_DrawLabelText(ByVal LabelText As CLabelText, ByVal blnSelect As Boolean) As Integer '20170215 baluu del
    Public Function YCM_DrawLabelText(ByVal LabelText As CLabelText, ByVal blnSelect As Boolean, Optional ByVal largeFont As Boolean = False) As Integer '20170215 baluu add
        YCM_DrawLabelText = 0
        Dim n As Integer = 20
        Dim r As Double = 0.03
        Dim Pi As Double = 3.1415926536
        Dim k As Integer = 100
        glLineWidth(1.0F)
        'Dim intTextWidth As Integer
        'glPushMatrix()
        If blnSelect Then
            'glEnable(GL_BLEND)
            'glEnable(GL_LINE_SMOOTH)
            glEnable(GL_LINE_STIPPLE)
            glLineStipple(1, 4369)
        End If
        Dim k_mat As Integer = 4
        Dim mat_lib As New MatLib
        Dim resultM(k_mat, k_mat) As Double
        Dim resultMNew(k_mat * k_mat) As Double
        Dim rotateXM(k_mat, k_mat) As Double, rotateYM(k_mat, k_mat) As Double, rotateZM(k_mat, k_mat) As Double
        Dim moveM(k_mat, k_mat) As Double
        rotateXM = mat_lib.GetRoateYMat(yangle)
        rotateYM = mat_lib.GetRoateXMat(-xangle)
        rotateZM = mat_lib.GetRoateXMat(0)
        moveM = mat_lib.GetMoveMat(LabelText.x, LabelText.y, LabelText.z)
        resultM = mat_lib.Multiply(rotateZM, rotateYM)
        resultM = mat_lib.Multiply(resultM, rotateXM)
        resultM = mat_lib.Multiply(resultM, moveM)
        resultMNew = mat_lib.Mat2OpenglMat(resultM)
        glMatrixMode(GL_MODELVIEW)
        glPushMatrix()
        'glColor3f(entset_label.color.dbl_red, entset_label.color.dbl_green, entset_label.color.dbl_blue)'13.2.1修正

        'Edit Kiryu 計測点ハイライト
        Dim Mat_labelColor() As Single
        If LabelText.HighlightFlag Then
            Mat_labelColor = {1, 0.501, 0, 1.0}
            'Dim Mat_labelColor() As Single = {entset_label.color.dbl_red, 0, 0, 1.0}
        Else
            Mat_labelColor = {entset_label.color.dbl_red, entset_label.color.dbl_green, entset_label.color.dbl_blue, 1.0}
        End If
        'Edit Kiryu 計測点ハイライト

        glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_labelColor)
        Dim label_chararr As Char()
        label_chararr = LabelText.LabelName.ToCharArray


        If True Then       'Edit Kiryu 2017
            glMultMatrixd(resultMNew)
            '  glTranslated(LabelText.x, LabelText.y, LabelText.z)
            'If largeFont = True Then
            'glScalef(1 / 64 * entset_label.screensize / 100.0 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom) * 2 / 3, 1 / 64 * entset_label.screensize / 100.0 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom) * 2 / 3, 1.0 * 2 / 3)
            'Else
            glScalef(1 / 84 * entset_label.screensize / 100.0 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom) * 2 / 3, 1 / 84 * entset_label.screensize / 100.0 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom) * 2 / 3, 1.0 * 2 / 3)
            'End If


            glutStrokeCharacter(GLUT_STROKE_ROMAN, Asc(" "))
            glutStrokeCharacter(GLUT_STROKE_ROMAN, Asc(" "))
            For i As Integer = 0 To LabelText.LabelName.Length - 1
                'intTextWidth = glutStrokeWidth(GLUT_STROKE_ROMAN, Asc(label_chararr(i).ToString))
                glutStrokeCharacter(GLUT_STROKE_ROMAN, Asc(label_chararr(i).ToString))
            Next
        Else

            glRasterPos3f(LabelText.x, LabelText.y, LabelText.z)
            glutBitmapCharacter(GLUT_BITMAP_HELVETICA_10, Asc(" "))
            glutBitmapCharacter(GLUT_BITMAP_HELVETICA_10, Asc(" "))
            For i As Integer = 0 To LabelText.LabelName.Length - 1
                glutBitmapCharacter(GLUT_BITMAP_HELVETICA_10, Asc(label_chararr(i).ToString))
            Next
        End If



        glPopMatrix()
        glDisable(GL_LINE_STIPPLE)
    End Function

    Public Function YCM_Drawarea(ByVal geopoint1 As GeoPoint, ByVal geopoint2 As GeoPoint) As Integer
        YCM_Drawarea = 0
        Dim k_mat As Integer = 4
        Dim n As Integer = 20
        Dim r As Double = 0.03
        Dim Pi As Double = 3.1415926536
        Dim k As Integer = 100
        glLineWidth(1.0F)
        'Dim intTextWidth As Integer
        'glPushMatrix()
        'If blnSelect Then
        '    'glEnable(GL_BLEND)
        '    'glEnable(GL_LINE_SMOOTH)
        '    glLineStipple(1, 4369)
        'End If
        Dim mat_lib As New MatLib
        Dim resultM(k_mat, k_mat) As Double
        Dim resultMNew(k_mat * k_mat) As Double
        Dim rotateXM(k_mat, k_mat) As Double, rotateYM(k_mat, k_mat) As Double, rotateZM(k_mat, k_mat) As Double
        Dim moveM(k_mat, k_mat) As Double
        rotateXM = mat_lib.GetRoateYMat(yangle)
        rotateYM = mat_lib.GetRoateXMat(-xangle)
        rotateZM = mat_lib.GetRoateXMat(0)
        'moveM = mat_lib.GetMoveMat(0.0, 0.0, 0.0)
        resultM = mat_lib.Multiply(rotateZM, rotateYM)
        resultM = mat_lib.Multiply(resultM, rotateXM)
        'resultM = mat_lib.Multiply(resultM, moveM)
        resultMNew = mat_lib.Mat2OpenglMat(resultM)
        resultMMNew = resultMNew
        glMatrixMode(GL_MODELVIEW)
        glPushMatrix()
        glEnable(GL_LINE_STIPPLE)
        glLineStipple(1, 4369)
        glMultMatrixd(resultMNew)
        glBegin(GL_LINES)
        'glColor3f(1.0F, 1.0F, 1.0F) '13.2.1修正
        Dim Mat_areaColor() As Single = {1.0F, 1.0F, 1.0F, 1.0}
        glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_areaColor)
        glVertex3d(geopoint1.x, geopoint1.y, geopoint1.z)
        'glColor3f(1.0F, 1.0F, 1.0F) '13.2.1修正

        glVertex3d(geopoint2.x, geopoint2.y, geopoint2.z)
        glEnd()
        glPopMatrix()
    End Function

    Public Function YCM_DrawLabelTextXYZ(ByVal dblcolor() As Double, ByVal dblvec() As Double, ByVal str As String) As Integer
        YCM_DrawLabelTextXYZ = 0
        Dim k_mat As Integer = 4
        Dim n As Integer = 20
        Dim r As Double = 0.03
        Dim Pi As Double = 3.1415926536
        Dim k As Integer = 100
        glLineWidth(1.5F)
        'Dim intTextWidth As Integer
        Dim mat_lib As New MatLib
        Dim resultM(k_mat, k_mat) As Double
        Dim resultMNew(k_mat * k_mat) As Double
        Dim rotateXM(k_mat, k_mat) As Double, rotateYM(k_mat, k_mat) As Double, rotateZM(k_mat, k_mat) As Double
        Dim moveM(k_mat, k_mat) As Double
        rotateXM = mat_lib.GetRoateYMat(yangle)
        rotateYM = mat_lib.GetRoateXMat(-xangle)
        rotateZM = mat_lib.GetRoateXMat(0)
        moveM = mat_lib.GetMoveMat(CSng(dblvec(0)), CSng(dblvec(1)), CSng(dblvec(2)))
        resultM = mat_lib.Multiply(rotateZM, rotateYM)
        resultM = mat_lib.Multiply(resultM, rotateXM)
        resultM = mat_lib.Multiply(resultM, moveM)
        resultMNew = mat_lib.Mat2OpenglMat(resultM)
        glMatrixMode(GL_MODELVIEW)
        glPushMatrix()
        glMultMatrixd(resultMNew)
        'glColor3f(dblcolor(0), dblcolor(1), dblcolor(2)) '13.2.1修正
        Dim Mat_LabelTextXYZColor() As Single = {dblcolor(0), dblcolor(1), dblcolor(2), 1.0}
        glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_LabelTextXYZColor)
        'glTranslated(LabelText.x, LabelText.y, LabelText.z)
        glScalef(1 / 84 * entset_label.screensize / 100.0 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom), 1 / 84 * entset_label.screensize / 100.0 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom), 1)

        glutStrokeCharacter(GLUT_STROKE_ROMAN, Asc(str.ToString))

        glPopMatrix()
    End Function

    '20170105 baluu add start
    Public hv_HalconObjectModel3d As HTuple '20170322 baluu add
    Public hv_PlaneObject3d As HTuple '20170710 SUURI Add
    Public modelPoints As LoadResult
    Public meshes As List(Of ObjMesh) = New List(Of ObjMesh)
    Public isOnlyPoints As Boolean = True
    Public drawingDone As Boolean = False
    Public Function YCM_DrawObjModel3d(ByVal fileName As String, ByVal reload As Boolean)
        If modelPoints Is Nothing Or reload Then
            'Dim objLoaderFactory = New ObjLoaderFactory()
            'Dim ObjLoader = objLoaderFactory.Create()
            'modelPoints = ObjLoader.Load(fileName)
            'Dim converter = New ObjToMeshConverter()
            'meshes.Clear()
            'meshes = converter.Convert(modelPoints)
            'If meshes.Count > 0 Then
            '    modelPoints = Nothing
            '    modelPoints = New LoadResult() 'Clearing memory
            '    isOnlyPoints = False
            'End If

            'If hv_HalconObjectModel3d Is Nothing Then
            '    HOperatorSet.GenEmptyObjectModel3d(hv_HalconObjectModel3d)
            'Else
            '    HOperatorSet.ClearObjectModel3d(hv_HalconObjectModel3d)
            '    HOperatorSet.GenEmptyObjectModel3d(hv_HalconObjectModel3d)
            'End If
            'HOperatorSet.ReadObjectModel3d(fileName, 1, New HTuple, New HTuple, hv_HalconObjectModel3d, New HTuple) '20170322 baluu add
        End If
        'If meshes.Count > 0 Then
        '    modelPoints = Nothing
        '    modelPoints = New LoadResult() 'Clearing memory
        '    isOnlyPoints = False
        'End If

        'glEnable(GL_DEPTH_TEST)
        '   glMatrixMode(GL_MODELVIEW)

        'クリア
        'glClear(GL_COLOR_BUFFER_BIT)
        'glClear(GL_DEPTH_BUFFER_BIT)
        glEnable(GL_DEPTH_TEST)

        glPushMatrix()
        If isOnlyPoints Then
            glDisable(GL_LIGHTING)
            glPointSize(0.5)
            glBegin(GL_POINTS)
            For Each vertex In modelPoints.Vertices
                If vertex.r = Nothing Then
                    glColor3f(1.0F, 1.0F, 0.0F)
                Else
                    glColor3f(vertex.r, vertex.g, vertex.b)
                End If

                glVertex3f(vertex.x, vertex.y, vertex.z)
            Next
            glEnable(GL_LIGHTING)
            glClear(GL_COLOR)
            glEnd()
        Else
         

            glDisable(GL_LIGHTING)

            glDisable(GL_CULL_FACE)
            glBegin(GL_TRIANGLES)


            For Each mesh In meshes
                For i = 0 To mesh.Triangles.Count - 1 Step 3
                    Dim triangle1 = mesh.Triangles(i)
                    Dim triangle2 = mesh.Triangles(i + 1)
                    Dim triangle3 = mesh.Triangles(i + 2)
                    'Dim normal = mesh.Normals(i)

                    'glNormal3f(normal.x, normal.y, normal.z)
                    ' glBegin(GL_POLYGON)
                    glColor3f(triangle1.r, triangle1.g, triangle1.b)
                    glVertex3f(triangle1.x, triangle1.y, triangle1.z)
                    glColor3f(triangle2.r, triangle2.g, triangle2.b)
                    glVertex3f(triangle2.x, triangle2.y, triangle2.z)
                    glColor3f(triangle3.r, triangle3.g, triangle3.b)
                    glVertex3f(triangle3.x, triangle3.y, triangle3.z)
                    '   glClear(GL_DEPTH_BUFFER_BIT)
                    'glColor3f(triangle1.r, triangle1.g, triangle1.b)
                    'glVertex3f(triangle1.x, triangle1.y, triangle1.z)
                    'glColor3f(triangle3.r, triangle3.g, triangle3.b)
                    'glVertex3f(triangle3.x, triangle3.y, triangle3.z)
                    'glColor3f(triangle2.r, triangle2.g, triangle2.b)
                    'glVertex3f(triangle2.x, triangle2.y, triangle2.z)
                    ' glEnd()
                Next
            Next
            glEnd()
            glEnable(GL_LIGHTING)
            glClear(GL_COLOR)
            glEnable(GL_CULL_FACE)
            glFlush()
        End If

        glPopMatrix()
        drawingDone = True
        Return True
    End Function



    '20170105 baluu add end

    Public Sub FitPlanes(ByVal hv_ObjectModel3d As HTuple, ByRef hv_Planes As HTuple, Optional maxcurvature_diff As Double = 0.005)

        ' Local control variables 
        Dim hv_ObjectModel3DOut As HTuple = New HTuple, hv_NumPoints As HTuple = New HTuple
        Dim hv_ObjectModel3DOut1 As HTuple = New HTuple, hv_ParamValue As HTuple = New HTuple
        Dim hv_Length As HTuple = New HTuple, hv_TempPlanes As HTuple = New HTuple
        Dim hv_Index As HTuple = New HTuple, hv_CopiedObjectModel3D As HTuple = New HTuple
        Dim hv_TriangulatedObjectModel3D As HTuple = New HTuple
        Dim hv_ConvexObjectModel3d As New HTuple
        Dim hv_Information As HTuple = New HTuple

        HOperatorSet.PrepareObjectModel3d(hv_ObjectModel3d, New HTuple("segmentation"), _
           New HTuple("false"), New HTuple("max_area_holes"), New HTuple(10))
        HOperatorSet.SegmentObjectModel3d(hv_ObjectModel3d, (((New HTuple(New HTuple("max_orientation_diff"))).TupleConcat( _
            New HTuple("max_curvature_diff"))).TupleConcat(New HTuple("min_area"))).TupleConcat( _
            New HTuple("output_xyz_mapping")), (((New HTuple(0.25)).TupleConcat(maxcurvature_diff)).TupleConcat( _
            100)).TupleConcat(New HTuple("false")), hv_ObjectModel3DOut)
        'HOperatorSet.SegmentObjectModel3d(hv_ObjectModel3d,
        '    ((((((((New HTuple(New HTuple("max_orientation_diff"))).TupleConcat( _
        '    New HTuple("max_curvature_diff"))).TupleConcat(New HTuple("min_area"))).TupleConcat( _
        '    New HTuple("output_xyz_mapping"))).TupleConcat(New HTuple("fitting"))).TupleConcat( _
        '    New HTuple("fitting_algorithm"))).TupleConcat(New HTuple("primitive_type"))).TupleConcat( _
        '    New HTuple("min_radius"))).TupleConcat(New HTuple("max_radius")),
        '    ((((((((New HTuple(0.1)).TupleConcat( _
        '    0.05)).TupleConcat(100)).TupleConcat(New HTuple("false"))).TupleConcat(New HTuple("true"))).TupleConcat( _
        '    New HTuple("least_squares"))).TupleConcat(New HTuple("cylinder"))).TupleConcat( _
        '    15)).TupleConcat(100), hv_ObjectModel3DOut)

        HOperatorSet.FitPrimitivesObjectModel3d(hv_ObjectModel3DOut,
            (((New HTuple(New HTuple("fitting_algorithm"))).TupleConcat( _
            New HTuple("primitive_type"))).TupleConcat(New HTuple("min_radius"))).TupleConcat( _
            New HTuple("max_radius")),
            (((New HTuple(New HTuple("least_squares_tukey"))).TupleConcat( _
            New HTuple("plane"))).TupleConcat(15)).TupleConcat(100), hv_ObjectModel3DOut1)

        HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DOut1, New HTuple("num_points"), hv_NumPoints)
        HOperatorSet.TupleLength(hv_NumPoints, hv_Length)
        hv_Planes = New HTuple()

        For hv_Index = (New HTuple(0)) To hv_Length.TupleSub(New HTuple(1)) Step (New HTuple(1))
            Try
                HOperatorSet.CopyObjectModel3d(hv_ObjectModel3DOut1.TupleSelect(hv_Index), New HTuple("primitive_plane"), hv_CopiedObjectModel3D)

                HOperatorSet.ConvexHullObjectModel3d(hv_CopiedObjectModel3D, hv_ConvexObjectModel3d)
                HOperatorSet.ClearObjectModel3d(hv_CopiedObjectModel3D)
                HOperatorSet.TupleConcat(hv_TempPlanes, hv_ConvexObjectModel3d, hv_TempPlanes)

            Catch ex As Exception

            End Try

        Next
        hv_Planes = hv_TempPlanes
        '  HOperatorSet.UnionObjectModel3d(hv_TempPlanes, New HTuple("points_surface"), hv_Planes)
        ' HOperatorSet.CopyObjectModel3d(hv_TempPlanes, New HTuple("all"), hv_Planes)
        HOperatorSet.ClearObjectModel3d(hv_ObjectModel3DOut)
        HOperatorSet.ClearObjectModel3d(hv_ObjectModel3DOut1)
        ' HOperatorSet.ClearObjectModel3d(hv_TempPlanes)

    End Sub

    Public Sub FitPlanesOptimal(ByVal hv_ObjectModel3d As HTuple, ByRef hv_Planes As HTuple)

        ' Local control variables 
        Dim hv_ObjectModel3DOut As HTuple = New HTuple, hv_NumPoints As HTuple = New HTuple
        Dim hv_ObjectModel3DOut1 As New HTuple
        Dim hv_ParamValue As HTuple = New HTuple
        Dim hv_Length As HTuple = New HTuple, hv_TempPlanes As HTuple = New HTuple
        Dim hv_Index As HTuple = New HTuple, hv_CopiedObjectModel3D As HTuple = New HTuple
        Dim hv_TriangulatedObjectModel3D As HTuple = New HTuple
        Dim hv_ConvexObjectModel3d As New HTuple
        Dim hv_Information As HTuple = New HTuple

        HOperatorSet.PrepareObjectModel3d(hv_ObjectModel3d, New HTuple("segmentation"), _
           New HTuple("false"), New HTuple("max_area_holes"), New HTuple(10))
        'HOperatorSet.SegmentObjectModel3d(hv_ObjectModel3d, (((New HTuple(New HTuple("max_orientation_diff"))).TupleConcat( _
        '    New HTuple("max_curvature_diff"))).TupleConcat(New HTuple("min_area"))).TupleConcat( _
        '    New HTuple("output_xyz_mapping")), (((New HTuple(0.25)).TupleConcat(maxcurvature_diff)).TupleConcat( _
        '    100)).TupleConcat(New HTuple("false")), hv_ObjectModel3DOut)

        Dim maxOrientationDiff As Double = 0.1
        Dim maxCurvatureDiff As Double = 0.005
        Dim i As Double = 0
        Dim j As Double = 0
        Dim hv_RMS As New HTuple
        Dim hv_meanRMS As New HTuple
        Dim hv_MinRMS As New HTuple(999.9)
        Dim hv_countObj As New HTuple
        Dim hv_score As New HTuple
        For i = 0.2 To 0.3 Step 0.05 'maxOrientationDiff
            For j = 0.005 To 0.1 Step 0.05 'maxCurvatureDiff


                'HOperatorSet.SegmentObjectModel3d(hv_ObjectModel3d,
                '  ((((((((New HTuple(New HTuple("max_orientation_diff"))).TupleConcat( _
                '  New HTuple("max_curvature_diff"))).TupleConcat(New HTuple("min_area"))).TupleConcat( _
                '  New HTuple("output_xyz_mapping"))).TupleConcat(New HTuple("fitting"))).TupleConcat( _
                '  New HTuple("fitting_algorithm"))).TupleConcat(New HTuple("primitive_type"))).TupleConcat( _
                '  New HTuple("min_radius"))).TupleConcat(New HTuple("max_radius")),
                '  ((((((((New HTuple(i)).TupleConcat( _
                '  j)).TupleConcat(200)).TupleConcat(New HTuple("false"))).TupleConcat(New HTuple("true"))).TupleConcat( _
                '  New HTuple("least_squares_tukey"))).TupleConcat(New HTuple("plane"))).TupleConcat( _
                '  15)).TupleConcat(200), hv_ObjectModel3DOut)

                HOperatorSet.SegmentObjectModel3d(hv_ObjectModel3d, (((New HTuple(New HTuple("max_orientation_diff"))).TupleConcat( _
                   New HTuple("max_curvature_diff"))).TupleConcat(New HTuple("min_area"))).TupleConcat( _
                   New HTuple("output_xyz_mapping")), (((New HTuple(i)).TupleConcat(j)).TupleConcat( _
                   100)).TupleConcat(New HTuple("false")), hv_ObjectModel3DOut)
                HOperatorSet.FitPrimitivesObjectModel3d(hv_ObjectModel3DOut,
                  (((New HTuple(New HTuple("fitting_algorithm"))).TupleConcat( _
                  New HTuple("primitive_type"))).TupleConcat(New HTuple("min_radius"))).TupleConcat( _
                  New HTuple("max_radius")),
                  (((New HTuple(New HTuple("least_squares_tukey"))).TupleConcat( _
                  New HTuple("plane"))).TupleConcat(15)).TupleConcat(100), hv_ObjectModel3DOut1)


                HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DOut1, "primitive_rms", hv_RMS)
                HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DOut1, "num_points", hv_countObj)
                HOperatorSet.ClearObjectModel3d(hv_ObjectModel3DOut)
                HOperatorSet.ClearObjectModel3d(hv_ObjectModel3DOut1)
                HOperatorSet.TupleMean(hv_RMS, hv_meanRMS)
                HOperatorSet.TupleDiv(hv_RMS, hv_countObj, hv_score)
                HOperatorSet.TupleMean(hv_score, hv_score)
                If hv_MinRMS.D > hv_score.D Then
                    hv_MinRMS = hv_score
                    maxOrientationDiff = i
                    maxCurvatureDiff = j
                End If
            Next
        Next
        'HOperatorSet.SegmentObjectModel3d(hv_ObjectModel3d,
        '           ((((((((New HTuple(New HTuple("max_orientation_diff"))).TupleConcat( _
        '           New HTuple("max_curvature_diff"))).TupleConcat(New HTuple("min_area"))).TupleConcat( _
        '           New HTuple("output_xyz_mapping"))).TupleConcat(New HTuple("fitting"))).TupleConcat( _
        '           New HTuple("fitting_algorithm"))).TupleConcat(New HTuple("primitive_type"))).TupleConcat( _
        '           New HTuple("min_radius"))).TupleConcat(New HTuple("max_radius")),
        '           ((((((((New HTuple(maxOrientationDiff)).TupleConcat( _
        '           maxCurvatureDiff)).TupleConcat(200)).TupleConcat(New HTuple("false"))).TupleConcat(New HTuple("true"))).TupleConcat( _
        '           New HTuple("least_squares_tukey"))).TupleConcat(New HTuple("plane"))).TupleConcat( _
        '           15)).TupleConcat(100), hv_ObjectModel3DOut)

        HOperatorSet.SegmentObjectModel3d(hv_ObjectModel3d, (((New HTuple(New HTuple("max_orientation_diff"))).TupleConcat( _
                  New HTuple("max_curvature_diff"))).TupleConcat(New HTuple("min_area"))).TupleConcat( _
                  New HTuple("output_xyz_mapping")), (((New HTuple(maxOrientationDiff)).TupleConcat(maxCurvatureDiff)).TupleConcat( _
                  100)).TupleConcat(New HTuple("false")), hv_ObjectModel3DOut)

        HOperatorSet.FitPrimitivesObjectModel3d(hv_ObjectModel3DOut,
            (((New HTuple(New HTuple("fitting_algorithm"))).TupleConcat( _
            New HTuple("primitive_type"))).TupleConcat(New HTuple("min_radius"))).TupleConcat( _
            New HTuple("max_radius")),
            (((New HTuple(New HTuple("least_squares_tukey"))).TupleConcat( _
            New HTuple("plane"))).TupleConcat(15)).TupleConcat(100), hv_ObjectModel3DOut1)

        HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DOut1, New HTuple("num_points"), hv_NumPoints)
        HOperatorSet.TupleLength(hv_NumPoints, hv_Length)
        hv_Planes = New HTuple()

        For hv_Index = (New HTuple(0)) To hv_Length.TupleSub(New HTuple(1)) Step (New HTuple(1))
            Try
                HOperatorSet.CopyObjectModel3d(hv_ObjectModel3DOut1.TupleSelect(hv_Index), New HTuple("primitive_plane"), hv_CopiedObjectModel3D)
                HOperatorSet.ConvexHullObjectModel3d(hv_CopiedObjectModel3D, hv_ConvexObjectModel3d)
                HOperatorSet.ClearObjectModel3d(hv_CopiedObjectModel3D)
                HOperatorSet.TupleConcat(hv_TempPlanes, hv_ConvexObjectModel3d, hv_TempPlanes)

            Catch ex As Exception

            End Try

        Next
        hv_Planes = hv_TempPlanes
        '   HOperatorSet.UnionObjectModel3d(hv_TempPlanes, New HTuple("points_surface"), hv_Planes)
        ' HOperatorSet.CopyObjectModel3d(hv_TempPlanes, New HTuple("all"), hv_Planes)
        HOperatorSet.ClearObjectModel3d(hv_ObjectModel3DOut)
        HOperatorSet.ClearObjectModel3d(hv_ObjectModel3DOut1)
        ' HOperatorSet.ClearObjectModel3d(hv_TempPlanes)

    End Sub

    Public Sub FitCylinders(ByVal hv_ObjectModel3d As HTuple, ByRef hv_Planes As HTuple, Optional maxcurvature_diff As Double = 0.005)

        Dim hv_ObjectModel3DOut As HTuple = New HTuple, hv_NumPoints As HTuple = New HTuple
        Dim hv_ObjectModel3DOut1 As HTuple = New HTuple
        Dim hv_Length As HTuple = New HTuple, hv_TempPlanes As HTuple = New HTuple
        Dim hv_Index As HTuple = New HTuple, hv_CopiedObjectModel3D As HTuple = New HTuple
        Dim hv_ConvexObjectModel3d As New HTuple


        HOperatorSet.PrepareObjectModel3d(hv_ObjectModel3d, New HTuple("segmentation"), _
            New HTuple("false"), New HTuple("max_area_holes"), New HTuple(10))
        'HOperatorSet.SegmentObjectModel3d(hv_ObjectModel3d, (((New HTuple(New HTuple("max_orientation_diff"))).TupleConcat( _
        '    New HTuple("max_curvature_diff"))).TupleConcat(New HTuple("min_area"))).TupleConcat( _
        '    New HTuple("output_xyz_mapping")), (((New HTuple(0.1)).TupleConcat(0.05)).TupleConcat( _
        '    100)).TupleConcat(New HTuple("false")), hv_ObjectModel3DOut)
        HOperatorSet.SegmentObjectModel3d(hv_ObjectModel3d,
            ((((((((New HTuple(New HTuple("max_orientation_diff"))).TupleConcat( _
            New HTuple("max_curvature_diff"))).TupleConcat(New HTuple("min_area"))).TupleConcat( _
            New HTuple("output_xyz_mapping"))).TupleConcat(New HTuple("fitting"))).TupleConcat( _
            New HTuple("fitting_algorithm"))).TupleConcat(New HTuple("primitive_type"))).TupleConcat( _
            New HTuple("min_radius"))).TupleConcat(New HTuple("max_radius")),
            ((((((((New HTuple(0.1)).TupleConcat( _
            0.05)).TupleConcat(100)).TupleConcat(New HTuple("false"))).TupleConcat(New HTuple("true"))).TupleConcat( _
            New HTuple("least_squares"))).TupleConcat(New HTuple("cylinder"))).TupleConcat( _
            15)).TupleConcat(100), hv_ObjectModel3DOut)

        HOperatorSet.FitPrimitivesObjectModel3d(hv_ObjectModel3DOut,
            (((New HTuple(New HTuple("fitting_algorithm"))).TupleConcat( _
            New HTuple("primitive_type"))).TupleConcat(New HTuple("min_radius"))).TupleConcat( _
            New HTuple("max_radius")),
            (((New HTuple(New HTuple("least_squares"))).TupleConcat( _
            New HTuple("cylinder"))).TupleConcat(15)).TupleConcat(100), hv_ObjectModel3DOut1)

        HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DOut1, New HTuple("num_points"), hv_NumPoints)
        HOperatorSet.TupleLength(hv_NumPoints, hv_Length)
        hv_Planes = New HTuple()

        For hv_Index = (New HTuple(0)) To hv_Length.TupleSub(New HTuple(1)) Step (New HTuple(1))
            Try
                HOperatorSet.CopyObjectModel3d(hv_ObjectModel3DOut1.TupleSelect(hv_Index), New HTuple("primitive_cylinder"), hv_CopiedObjectModel3D)
                HOperatorSet.ConvexHullObjectModel3d(hv_CopiedObjectModel3D, hv_ConvexObjectModel3d)
                HOperatorSet.ClearObjectModel3d(hv_CopiedObjectModel3D)
                HOperatorSet.TupleConcat(hv_TempPlanes, hv_ConvexObjectModel3d, hv_TempPlanes)

            Catch ex As Exception

            End Try

        Next
        hv_Planes = hv_TempPlanes
        'HOperatorSet.UnionObjectModel3d(hv_TempPlanes, New HTuple("points_surface"), hv_Planes)

        HOperatorSet.ClearObjectModel3d(hv_ObjectModel3DOut)
        HOperatorSet.ClearObjectModel3d(hv_ObjectModel3DOut1)
        'HOperatorSet.ClearObjectModel3d(hv_TempPlanes)

    End Sub

    Public Sub GenPlaneBySelectedObject3d(ByVal winHand As HTuple, ByVal ImageIndex As Integer, ByVal iType As Integer, ByRef hv_Object3d As HTuple)
        Dim ho_UserDrawRegion As HObject = Nothing
        Dim hv_ReducedObject3d As New HTuple
        Dim hv_ThisImagePose As New HTuple
        Dim hv_TriangeledObject3d As New HTuple
        Dim hv_ResultObject3d As New HTuple
        Dim hv_OneTimePlanes As New HTuple
        Dim hv_unionObject3d As New HTuple
        HOperatorSet.ReadObjectModel3d(MainFrm.objFBM.ProjectPath & "\Pdata\ReconstResultOm3.om3", New HTuple(1), New HTuple, New HTuple, hv_ResultObject3d, New HTuple)

        HOperatorSet.PoseInvert(MainFrm.objFBM.lstImages(ImageIndex).ImagePose.ReConstWorldPose, hv_ThisImagePose)
        HOperatorSet.DrawRegion(ho_UserDrawRegion, winHand)
        HOperatorSet.ReduceObjectModel3dByView(ho_UserDrawRegion, hv_ResultObject3d, MainFrm.objFBM.hv_CamparamOut, hv_ThisImagePose, hv_ReducedObject3d)
        HOperatorSet.ClearObjectModel3d(hv_ResultObject3d)
        HOperatorSet.TriangulateObjectModel3d(hv_ReducedObject3d, "greedy", New HTuple, New HTuple, hv_TriangeledObject3d, New HTuple)
        HOperatorSet.ClearObjectModel3d(hv_ReducedObject3d)
        'Dim hv_TempObject3d As New HTuple
        'HOperatorSet.CopyObjectModel3d(hv_Object3d, "all", hv_TempObject3d)
        'HOperatorSet.ClearObjectModel3d(hv_Object3d)
        Dim maxcurvadiff As Double = 0.005
        For i As Integer = 0 To 10
            If iType = 0 Then
                FitPlanes(hv_TriangeledObject3d, hv_OneTimePlanes, maxcurvadiff)
            End If
            If iType = 1 Then
                FitCylinders(hv_TriangeledObject3d, hv_OneTimePlanes, maxcurvadiff)
            End If
            Dim hv_IsAruPlane As New HTuple
            HOperatorSet.GetObjectModel3dParams(hv_OneTimePlanes, "num_points", hv_IsAruPlane)
            If hv_IsAruPlane.Length > 0 Then
                Exit For
            Else
                maxcurvadiff = maxcurvadiff * 10
            End If
        Next

        HOperatorSet.ClearObjectModel3d(hv_TriangeledObject3d)
        HOperatorSet.TupleConcat(hv_Object3d, hv_OneTimePlanes, hv_Object3d)

        'HOperatorSet.UnionObjectModel3d(hv_unionObject3d, "points_surface", hv_Object3d)
        'HOperatorSet.ClearObjectModel3d(hv_unionObject3d)
    End Sub

    Public Function Filesave_P(ByVal SFD As SaveFileDialog) As String
        Dim Fs_P_Str As String = ""
        SFD.FileName = "座標値"
        SFD.Filter = "テキストファイル(*.csv)|*.csv"
        'SFD.Title = "名前をつけてtxtファイルの保存"
        SFD.Title = "CSV出力"
        SFD.RestoreDirectory = True
        SFD.CheckFileExists = False
        SFD.CheckPathExists = True
        Dim result As System.Windows.Forms.DialogResult = SFD.ShowDialog()
        If result = System.Windows.Forms.DialogResult.OK Then
            If SFD.FileName <> "" Then
                Console.WriteLine(SFD.FileName)
                Fs_P_Str = SFD.FileName
            Else
            End If
        End If
        Return Fs_P_Str
    End Function
    Public Function Filesave_CAD(ByVal SFD As SaveFileDialog, Optional ByVal filter As String = "CAD図面ファイル(*.dwg)|*.dwg") As String
        Dim Fs_P_Str As String = ""

        'ljj add str 2014/1/15
        'SFD.FileName = "Drawing1.dwg"

        'Rep By Yamada 20140922 Sta 日本車輌-------------
        'SFD.FileName = "Drawing1"
        SFD.FileName = WorksD.GetDefaultDXGFileName()
        'Rep By Yamada 20140922 End 日本車輌-------------

        'SFD.Filter = "CAD図面ファイル(*.dwg)|*.dwg"
        'SFD.Filter = "CAD図面ファイル(*.dwg)|*.dwg|CAD図面ファイル(*.dxf)|*.dxf"
        SFD.Filter = filter
        'end

        SFD.Title = "CAD図面ファイルの指定"
        SFD.RestoreDirectory = True
        SFD.CheckFileExists = False
        SFD.CheckPathExists = True
        Dim result As System.Windows.Forms.DialogResult = SFD.ShowDialog()
        If result = System.Windows.Forms.DialogResult.OK Then
            If SFD.FileName <> "" Then
                Console.WriteLine(SFD.FileName)
                Fs_P_Str = SFD.FileName
            Else
            End If
        End If
        Return Fs_P_Str
    End Function
    Public Sub Creattxt(ByVal Cdtxt_str As String)
        'ToDo Kiryu ファイルがすでに開かれている場合のエラー処理追加
        Using fs As New System.IO.FileStream(Cdtxt_str, System.IO.FileMode.Create)
            Dim dind As System.IO.StreamReader = New System.IO.StreamReader(fs)
            dind.Close()
        End Using
    End Sub
    Public Sub readtxt(ByVal Cdtxt_str As String, ByVal DGV_txt As DataGridView)
        Dim NAME As Object = "No.,ラベル,X,Y,Z"
        FileOpen(3, Cdtxt_str, OpenMode.Append)
        PrintLine(3, NAME)
        FileClose(3)

        'ADD By Suuri 20150323 Sta-------- 
        FBMlib.GetListSTtoCT(My.Application.Info.DirectoryPath & "\STtoCTlist.csv")
        Dim actCTID As Integer
        'ADD By Suuri 20150323 End-------- 

        For i As Integer = 0 To DGV_txt.RowCount - 1
            If DGV_txt.Rows(i).Cells(0).Value = True Then
                FileOpen(3, Cdtxt_str, OpenMode.Append)
                Write(3, i + 1)
                Print(3, DGV_txt.Rows(i).Cells(1).Value)
                Print(3, ",")
                Write(3, String.Format("{0:F2}", DGV_txt.Rows(i).Cells(2).Value))
                Write(3, String.Format("{0:F2}", DGV_txt.Rows(i).Cells(3).Value))

                'Rep By Suuri 20150323 Sta -----------------------
                Dim blnCsvOut As Integer = GetPrivateProfileInt("Sueoki", "CSVOUTKIJUN", 0, My.Application.Info.DirectoryPath & "\vform.ini")
                If blnCsvOut = 0 Then
                    WriteLine(3, String.Format("{0:F2}", DGV_txt.Rows(i).Cells(4).Value))
                Else
                    Write(3, String.Format("{0:F2}", DGV_txt.Rows(i).Cells(4).Value))

                    If IsNumeric(CStr(DGV_txt.Rows(i).Cells(1).Value).Replace("CT", "").Split("_")(0)) Then
                        actCTID = CInt(CStr(DGV_txt.Rows(i).Cells(1).Value).Replace("CT", "").Split("_")(0))
                        Dim flgKijyun As Boolean = False
                        For Each objSTtoCT In FBMlib.lstSTtoCT
                            If objSTtoCT.CTID = actCTID Then
                                If objSTtoCT.flgKijyun = 1 Then
                                    flgKijyun = True
                                End If
                            End If
                        Next
                        If flgKijyun = True Then
                            WriteLine(3, 1)
                        Else
                            WriteLine(3, 0)
                        End If
                    Else
                        WriteLine(3, 0)
                    End If
                End If
                'Rep By Suuri 20150323 End -----------------------

                FileClose(3)
            End If
        Next
        FileClose(3)
        Process.Start(Cdtxt_str)
    End Sub

    Public Sub readlabeltxt(ByVal Cdtxt_str As String, ByVal DGV_txt As DataGridView)
        Dim strtemp As String
        For i As Integer = 0 To DGV_txt.RowCount - 1
            If DGV_txt.RowCount > 0 Then
                FileOpen(3, Cdtxt_str, OpenMode.Append)
                'Write(3, i + 1)
                'Print(3, DGV_txt.Rows(i).Cells(1).Value)
                'Print(3, ",")
                If DGV_txt.Rows(i).Cells(0).Value = Nothing Then
                    strtemp = ""
                Else
                    strtemp = DGV_txt.Rows(i).Cells(0).Value.ToString
                End If

                Write(3, strtemp)
                If DGV_txt.Rows(i).Cells(1).Value = Nothing Then
                    strtemp = ""
                Else
                    strtemp = DGV_txt.Rows(i).Cells(1).Value.ToString
                End If
                Write(3, strtemp)
                If DGV_txt.Rows(i).Cells(2).Value = Nothing Then
                    strtemp = ""
                Else
                    strtemp = DGV_txt.Rows(i).Cells(2).Value.ToString
                End If
                Write(3, strtemp)
                If DGV_txt.Rows(i).Cells(3).Value = Nothing Then
                    strtemp = ""
                Else
                    strtemp = DGV_txt.Rows(i).Cells(3).Value.ToString
                End If
                Write(3, strtemp)
                If DGV_txt.Rows(i).Cells(4).Value = Nothing Then
                    strtemp = ""
                Else
                    strtemp = DGV_txt.Rows(i).Cells(4).Value.ToString
                End If
                Write(3, strtemp)
                If DGV_txt.Rows(i).Cells(5).Value = Nothing Then
                    strtemp = ""
                Else
                    strtemp = DGV_txt.Rows(i).Cells(5).Value.ToString
                End If
                Write(3, strtemp)
                If DGV_txt.Rows(i).Cells(6).Value = Nothing Then
                    strtemp = ""
                Else
                    strtemp = DGV_txt.Rows(i).Cells(6).Value.ToString
                End If
                Write(3, strtemp)
                If DGV_txt.Rows(i).Cells(7).Value = Nothing Then
                    strtemp = ""
                Else
                    strtemp = DGV_txt.Rows(i).Cells(7).Value.ToString
                End If
                Write(3, strtemp)
                If DGV_txt.Rows(i).Cells(8).Value = Nothing Then
                    strtemp = ""
                Else
                    strtemp = DGV_txt.Rows(i).Cells(8).Value.ToString
                End If
                Write(3, strtemp)
                If DGV_txt.Rows(i).Cells(9).Value = Nothing Then
                    strtemp = ""
                Else
                    strtemp = DGV_txt.Rows(i).Cells(9).Value.ToString
                End If
                Write(3, strtemp)
                If DGV_txt.Rows(i).Cells(10).Value = Nothing Then
                    strtemp = ""
                Else
                    strtemp = DGV_txt.Rows(i).Cells(10).Value.ToString
                End If
                Write(3, strtemp)
                If DGV_txt.Rows(i).Cells(11).Value = Nothing Then
                    strtemp = ""
                Else
                    strtemp = DGV_txt.Rows(i).Cells(11).Value.ToString
                End If
                WriteLine(3, strtemp)

                'Write(3, DGV_txt.Rows(i).Cells(0).Value.ToString)
                'Write(3, DGV_txt.Rows(i).Cells(1).Value.ToString)
                'Write(3, DGV_txt.Rows(i).Cells(2).Value.ToString)
                'Write(3, DGV_txt.Rows(i).Cells(3).Value.ToString)
                'Write(3, DGV_txt.Rows(i).Cells(4).Value.ToString)
                'Write(3, DGV_txt.Rows(i).Cells(5).Value.ToString)
                'Write(3, DGV_txt.Rows(i).Cells(6).Value.ToString)
                'Write(3, DGV_txt.Rows(i).Cells(7).Value.ToString)
                'Write(3, DGV_txt.Rows(i).Cells(8).Value.ToString)
                'Write(3, DGV_txt.Rows(i).Cells(9).Value.ToString)
                'Write(3, DGV_txt.Rows(i).Cells(10).Value.ToString)
                'WriteLine(3, DGV_txt.Rows(i).Cells(11).Value.ToString)
                FileClose(3)
            End If
        Next
        FileClose(3)
        Process.Start(Cdtxt_str)
    End Sub
    Public Function ComSel_SelectFolderByShell(ByVal Description As String, ByVal ShowNewFolderButton As Boolean)
        On Error GoTo ErrHandle
        ' Dim objShell As New Shell32.Shell, objFolder As Shell32.Folder
        Dim objFBD As New System.Windows.Forms.FolderBrowserDialog 'ユーザーにフォルダーを選択するよう要求します。このクラスは継承できません。

        objFBD.Description = Description '13.1.17 (山田) Descriptionプロパティ：ダイアログボックスの上部の表示するメッセージの設定

        objFBD.ShowNewFolderButton = ShowNewFolderButton '13.1.17 (山田) ShowNewFolderButtonプロパティ：True：「新しいフォルダの作成」表示/False：〃非表示
        ComSel_SelectFolderByShell = ""
        bIsOtherDlgOpen = True
        strLastProjectPath = My.Settings.strLastProjPath
        objFBD.SelectedPath = strLastProjectPath
        If objFBD.ShowDialog = DialogResult.OK Then 'ShowDialog：既定のオーナーを使用してコモン ダイアログ ボックスを実行します、DialogResult：ダイアログ ボックスの戻り値を示す識別子を指定します。

            ComSel_SelectFolderByShell = objFBD.SelectedPath '2012.12.26「名前を付けて保存」より、新しいフォルダのパス
            strLastProjectPath = ComSel_SelectFolderByShell '2012.12.26「名前を付けて保存」より、新しいフォルダのパス
            My.Settings.strLastProjPath = strLastProjectPath '2012.12.26「名前を付けて保存」、現在開いているファイルのパス⇒新しいフォルダのパス
            My.Settings.Save() 'save：アプリケーション設定プロパティの現在の値を格納します。

            bIsOtherDlgOpen = False '=True:Mainフレームで他のダイアログを表示中
            Exit Function
        Else
            Exit Function
        End If
        'objFolder = objShell.BrowseForFolder(0, "フォルダを選択してください", 0, strLastProjectPath)
        'If objFolder Is Nothing Then
        '    ComSel_SelectFolderByShell = ""
        '    bIsOtherDlgOpen = False
        '    Exit Function
        'End If

ErrHandle:
        MsgBox("ComSel_SelectFolderByShell:" & Err.Description)
    End Function
    Public Function All_YCM3DVIEW_camer(ByVal camer As CCamera) As GeoPoint
        All_YCM3DVIEW_camer = Nothing
        If first_C_camer = True Then
            first_C_camer = False
            nCamers_x_max = camer.x
            nCamers_x_min = camer.x
            nCamers_y_max = camer.y
            nCamers_y_min = camer.y
            nCamers_z_max = camer.z
            nCamers_z_min = camer.z
        End If
        If camer.x >= nCamers_x_max Then
            nCamers_x_max = camer.x
            camer_xmax = camer
        End If
        If camer.y >= nCamers_y_max Then
            nCamers_y_max = camer.y
            camer_ymax = camer
        End If
        If camer.z >= nCamers_z_max Then
            nCamers_z_max = camer.z
            camer_zmax = camer
        End If
        If camer.x <= nCamers_x_min Then
            nCamers_x_min = camer.x
            camer_xmin = camer
        End If
        If camer.y <= nCamers_y_min Then
            nCamers_y_min = camer.y
            camer_ymin = camer
        End If
        If camer.z <= nCamers_z_min Then
            nCamers_z_min = camer.z
            camer_zmin = camer
        End If
        ents_centerC.setXYZ(CDbl((nCamers_x_max + nCamers_x_min) / 2), CDbl((nCamers_y_max + nCamers_y_min) / 2), CDbl((nCamers_z_max + nCamers_z_min) / 2))
        maxandmin_all(0) = nCamers_x_max
        maxandmin_all(1) = nCamers_x_min
        maxandmin_all(2) = nCamers_y_max
        maxandmin_all(3) = nCamers_y_min
        maxandmin_all(4) = nCamers_z_max
        maxandmin_all(5) = nCamers_z_min

        Return ents_centerC
    End Function
    Public Function All_YCM3DVIEW_point(ByVal lookpoint As CLookPoint) As Integer
        All_YCM3DVIEW_point = 0
        If first_C_lookpoint = True Then
            first_C_lookpoint = False
            nPoints_x_max = lookpoint.x
            nPoints_x_min = lookpoint.x
            nPoints_y_max = lookpoint.y
            nPoints_y_min = lookpoint.y
            nPoints_z_max = lookpoint.z
            nPoints_z_min = lookpoint.z
        End If
        If lookpoint.x >= nPoints_x_max Then
            nPoints_x_max = lookpoint.x
            point_xmax = lookpoint
        End If
        If lookpoint.y >= nPoints_y_max Then
            nPoints_y_max = lookpoint.y
            point_ymax = lookpoint
        End If
        If lookpoint.z >= nPoints_z_max Then
            nPoints_z_max = lookpoint.z
            point_zmax = lookpoint
        End If
        If lookpoint.x <= nPoints_x_min Then
            nPoints_x_min = lookpoint.x
            point_xmin = lookpoint
        End If
        If lookpoint.y <= nPoints_y_min Then
            nPoints_y_min = lookpoint.y
            point_ymin = lookpoint
        End If
        If lookpoint.z <= nPoints_z_min Then
            nPoints_z_min = lookpoint.z
            point_zmin = lookpoint
        End If
        If nPoints_x_max > nCamers_x_max Then
            maxandmin_all(0) = nPoints_x_max
        End If
        If nPoints_x_min < nCamers_x_min Then
            maxandmin_all(1) = nPoints_x_min
        End If
        If nPoints_y_max > nCamers_y_max Then
            maxandmin_all(2) = nPoints_y_max
        End If
        If nPoints_y_min < nCamers_y_min Then
            maxandmin_all(3) = nPoints_y_min
        End If
        If nPoints_z_max > nCamers_z_max Then
            maxandmin_all(4) = nPoints_z_max
        End If
        If nPoints_z_min < nCamers_z_min Then
            maxandmin_all(5) = nPoints_z_min
        End If
        'If (maxandmin_all(0) > 0 And maxandmin_all(1) < 0) Or (maxandmin_all(0) < 0 And maxandmin_all(1) > 0) Then
        '    maxandmin_all(6) = maxandmin_all(0) + maxandmin_all(1)
        'Else
        maxandmin_all(6) = (maxandmin_all(0) + maxandmin_all(1)) / 2
        'End If
        'If (maxandmin_all(2) > 0 And maxandmin_all(3) < 0) Or (maxandmin_all(2) < 0 And maxandmin_all(3) > 0) Then
        '    maxandmin_all(7) = maxandmin_all(2) + maxandmin_all(3)
        'Else
        maxandmin_all(7) = (maxandmin_all(2) + maxandmin_all(3)) / 2
        'End If
        'If (maxandmin_all(4) > 0 And maxandmin_all(5) < 0) Or (maxandmin_all(4) < 0 And maxandmin_all(5) > 0) Then
        '    maxandmin_all(8) = maxandmin_all(4) + maxandmin_all(5)
        'Else
        maxandmin_all(8) = (maxandmin_all(4) + maxandmin_all(5)) / 2

        cent_cp.x = maxandmin_all(6)
        cent_cp.y = maxandmin_all(7)
        cent_cp.z = maxandmin_all(8)
        'Public C_X_MaxDis As Double
        'Public C_Y_MaxDis As Double
        'Public C_Z_MaxDis As Double
        'Public C_X_MinDis As Double
        'Public C_Y_MinDis As Double
        'Public C_Z_MinDis As Double
        'Public P_X_MaxDis As Double
        'Public P_Y_MaxDis As Double
        'Public P_Z_MaxDis As Double
        'Public P_X_MinDis As Double
        'Public P_Y_MinDis As Double
        'Public P_Z_MinDis As Double
        Dim temp As New GeoPoint
        temp.x = camer_xmax.x
        temp.y = camer_xmax.y
        temp.z = camer_xmax.z
        C_X_MaxDis = temp.GetDistanceTo(cent_cp)
        temp.x = camer_xmin.x
        temp.y = camer_xmin.y
        temp.z = camer_xmin.z
        C_X_MinDis = temp.GetDistanceTo(cent_cp)
        temp.x = camer_ymax.x
        temp.y = camer_ymax.y
        temp.z = camer_ymax.z
        C_Y_MaxDis = temp.GetDistanceTo(cent_cp)
        temp.x = camer_ymin.x
        temp.y = camer_ymin.y
        temp.z = camer_ymin.z
        C_Y_MinDis = temp.GetDistanceTo(cent_cp)
        temp.x = camer_zmax.x
        temp.y = camer_zmax.y
        temp.z = camer_zmax.z
        C_Z_MaxDis = temp.GetDistanceTo(cent_cp)
        temp.x = camer_zmin.x
        temp.y = camer_zmin.y
        temp.z = camer_zmin.z
        C_Z_MinDis = temp.GetDistanceTo(cent_cp)
        temp.x = point_xmax.x
        temp.y = point_xmax.y
        temp.z = point_xmax.z
        P_X_MaxDis = temp.GetDistanceTo(cent_cp)
        temp.x = point_xmin.x
        temp.y = point_xmin.y
        temp.z = point_xmin.z
        P_X_MinDis = temp.GetDistanceTo(cent_cp)
        temp.x = point_ymax.x
        temp.y = point_ymax.y
        temp.z = point_ymax.z
        P_Y_MaxDis = temp.GetDistanceTo(cent_cp)
        temp.x = point_ymin.x
        temp.y = point_ymin.y
        temp.z = point_ymin.z
        P_Y_MinDis = temp.GetDistanceTo(cent_cp)
        temp.x = point_zmax.x
        temp.y = point_zmax.y
        temp.z = point_zmax.z
        P_Z_MaxDis = temp.GetDistanceTo(cent_cp)
        temp.x = point_zmin.x
        temp.y = point_zmin.y
        temp.z = point_zmin.z
        P_Z_MinDis = temp.GetDistanceTo(cent_cp)

        If C_X_MaxDis > C_X_MinDis Then

        End If
        'End If
    End Function

    '===============================================================
    ' 機　能：3点と軸方向を指定して座標変換マトリックスを取得する

    ' 戻り値：=0：正常終了

    '         =1：入力パラメタエラー（軸方向の組合せ）

    '         =2：入力パラメタエラー（  ）

    ' 引　数：

    '    posOrg   [I/ ] 原点
    '    pos1     [I/ ] 点１

    '    pos2     [I/ ] 点２

    '    iAxis1   [I/ ] 点１の軸方向（＋X:=+1、－X:=-1、＋Y:=+2、－Y:=-2、＋Z:=+3、－Z:=-3）

    '    iAxis2   [I/ ] 点２の軸方向（同上）

    '    iFixPos  [I/ ] 固定軸（=1:点１／=2:点２）

    '    dblMat() [ /O] 座標変換マトリックス（4×4）

    ' 備　考：

    '===============================================================
    Public Function YCM_Get3PosMatrix( _
        ByRef posOrg As CLookPoint, _
        ByRef pos1 As CLookPoint, _
        ByRef pos2 As CLookPoint, _
        ByRef iAxis1 As Integer, _
        ByRef iAxis2 As Integer, _
        ByRef iFixPos As Integer, _
        ByRef dblMat() As Double _
    ) As Integer
        'On Error GoTo Err_lbl
        Dim Xj(2) As Double, Yj(2) As Double, Zj(2) As Double '--設計点：Xj(),Yj(),Zj() baluu edit start (6 -> 2)
        Dim Xs(2) As Double, Ys(2) As Double, Zs(2) As Double '--計測点：Xs(),Ys(),Zs()
        Dim Xi(2) As Double, Yi(2) As Double, Zi(2) As Double '--重み　：Xi(),Yi(),Zi()
        Dim Xa(2) As Double, Ya(2) As Double, Za(2) As Double '--重みを考慮したジャストフィット計算結果：Xa(),Ya(),Za() baluu edit end

        YCM_Get3PosMatrix = -1

        '--1.指定された点と軸方向のチェック（指定された2点の軸方向が同じでない事を確認
        If (Math.Abs(iAxis1 - iAxis2) <= 0) Then
            YCM_Get3PosMatrix = 1
            Exit Function
        End If

        '--2.指定された３点で基準となる座標系を求める

        Dim dl1 As Double, dl2 As Double
        Xj(0) = 0.0# : Yj(0) = 0.0# : Zj(0) = 0.0#
        Xi(0) = 1.0# : Yi(0) = 1.0# : Zi(0) = 1.0#

        'Xi(0) = 100.0# : Yi(0) = 100.0# : Zi(0) = 100.0#
        'start 26:7:2011   16:31 by jf.w
        dl1 = pos1.distTo(posOrg) * Math.Sign(iAxis1)
        dl2 = pos2.distTo(posOrg) * Math.Sign(iAxis2)
        'dl1 = pos1.distTo(posOrg) * Sgn(iAxis1)
        'dl2 = pos2.distTo(posOrg) * Sgn(iAxis2)
        'end 26:7:2011   16:31 by jf.w
        Xi(1) = 1.0# : Yi(1) = 1.0# : Zi(1) = 1.0#
        Xi(2) = 1.0# : Yi(2) = 1.0# : Zi(2) = 1.0#
        Dim geoL As New GeoCurve, geoL_Tmp As New GeoCurve

        Dim geoVecParel As New GeoVector
        Dim geo_P As New GeoPoint, geo_PT As New GeoPoint
        Dim geoPOrg As New GeoPoint

        geoPOrg.setXYZ(posOrg.x, posOrg.y, posOrg.z)
        Dim lx As Double, ly As Double
        Dim vec1 As New GeoVector, vec2 As New GeoVector
        geoL.StartPoint.setXYZ(posOrg.x, posOrg.y, posOrg.z)

        If iFixPos = 1 Then
            Select Case (iAxis1)
                Case 1, -1
                    Xj(1) = dl1 : Yj(1) = 0.0# : Zj(1) = 0.0#

                Case 2, -2
                    Xj(1) = 0.0# : Yj(1) = dl1 : Zj(1) = 0.0#
                    'Xi(1) = 0.0# : Yi(1) = 100.0# : Zi(1) = 0.0#
                Case 3, -3
                    Xj(1) = 0.0# : Yj(1) = 0.0# : Zj(1) = dl1
                    'Xi(1) = 0.0# : Yi(1) = 0.0# : Zi(1) = 100.0#
                Case Else
                    YCM_Get3PosMatrix = 2
                    Exit Function
            End Select

            geoL.EndPoint.setXYZ(pos1.x, pos1.y, pos1.z)
            geo_P.setXYZ(pos2.x, pos2.y, pos2.z)
            geo_PT = geoL.GetPerpendicularFoot(geo_P)

            lx = geo_PT.GetDistanceTo(geoPOrg)
            ly = geo_PT.GetDistanceTo(geo_P)

            vec1.setXYZ(pos1.x - posOrg.x, pos1.y - posOrg.y, pos1.z - posOrg.z)
            vec2.setXYZ(geo_PT.x - posOrg.x, geo_PT.y - posOrg.y, geo_PT.z - posOrg.z)

            If vec1.GetInnerProduct(vec2) < 0 Then
                'If geoL.GetIsPointOnCurve(geo_PT, 0.001) = False Then
                lx = -lx
            End If

            Select Case (iAxis1)
                Case 1, -1
                    Xj(2) = lx * iAxis1
                Case 2, -2
                    Yj(2) = lx * iAxis1 / 2.0
                Case 3, -3
                    Zj(2) = lx * iAxis1 / 3.0
                Case Else
                    YCM_Get3PosMatrix = 2
                    Exit Function
            End Select

            Select Case (iAxis2)
                Case 1, -1
                    Xj(2) = ly * iAxis2
                Case 2, -2
                    Yj(2) = ly * iAxis2 / 2.0
                Case 3, -3
                    Zj(2) = ly * iAxis2 / 3.0
                Case Else
                    YCM_Get3PosMatrix = 2
                    Exit Function
            End Select

        Else
            geoL.EndPoint.setXYZ(pos2.x, pos2.y, pos2.z)
            geo_P.setXYZ(pos1.x, pos1.y, pos1.z)
            geo_PT = geoL.GetPerpendicularFoot(geo_P)
            ly = geo_PT.GetDistanceTo(geoPOrg)
            lx = geo_PT.GetDistanceTo(geo_P)
            If geoL.GetIsPointOnCurve(geo_P, 0.001) = False Then
                lx = -lx
            End If
            Select Case (iAxis1)
                Case 1, -1
                    Xj(1) = lx * iAxis1
                Case 2, -2
                    Yj(1) = lx * iAxis1 / 2.0
                Case 3, -3
                    Zj(1) = lx * iAxis1 / 3.0
                Case Else
                    YCM_Get3PosMatrix = 3
                    Exit Function
            End Select

            Select Case (iAxis2)
                Case 1, -1
                    Xj(1) = ly * iAxis2
                Case 2, -2
                    Yj(1) = ly * iAxis2 / 2.0
                Case 3, -3
                    Zj(1) = ly * iAxis2 / 3.0
                Case Else
                    YCM_Get3PosMatrix = 3
                    Exit Function
            End Select

            Select Case (iAxis2)
                Case 1, -1
                    Xj(2) = dl2 : Yj(2) = 0.0# : Zj(2) = 0.0#
                    'Xi(2) = 100.0# : Yi(2) = 0.0# : Zi(2) = 0.0#
                Case 2, -2
                    Xj(2) = 0.0# : Yj(2) = dl2 : Zj(2) = 0.0#
                    'Xi(2) = 0.0# : Yi(2) = 100.0# : Zi(2) = 0.0#
                Case 3, -3
                    Xj(2) = 0.0# : Yj(2) = 0.0# : Zj(2) = dl2
                    'Xi(2) = 0.0# : Yi(2) = 0.0# : Zi(2) = 100.0#
                Case Else
                    YCM_Get3PosMatrix = 3
                    Exit Function
            End Select

        End If
        '--3.計測点座標値の設定

        '-3.1 計測点３点
        Xs(0) = posOrg.x : Ys(0) = posOrg.y : Zs(0) = posOrg.z
        Xs(1) = pos1.x : Ys(1) = pos1.y : Zs(1) = pos1.z
        Xs(2) = pos2.x : Ys(2) = pos2.y : Zs(2) = pos2.z
        '-3.2 計測点に単位座標系のデータを追加
        '20170330 baluu del start
        'Xs(3) = 0.0# : Ys(3) = 0.0# : Zs(3) = 0.0#
        'Xs(4) = 1.0# : Ys(4) = 0.0# : Zs(4) = 0.0#
        'Xs(5) = 0.0# : Ys(5) = 1.0# : Zs(5) = 0.0#
        'Xs(6) = 0.0# : Ys(6) = 0.0# : Zs(6) = 1.0#
        '20170330 baluu del end
        '--4.重みを考慮したジャストフィット計算：結果：Xa(),Ya(),Za()
        Dim num As Integer
        num = 7
        'Dim hhmm As New HTuple
        'correspond_3d_3d_weight(Xs, Ys, Zs, Xj, Yj, Zj, Xi, Yi, Zi, hhmm)

        'iRet = OmomiExcute(num, Xi(0), Yi(0), Zi(0), Xj(0), Yj(0), Zj(0), Xa(0), Ya(0), Za(0), Xs(0), Ys(0), Zs(0))
        'ReDim Preserve Xs(2)
        'ReDim Preserve Ys(2)
        'ReDim Preserve Zs(2)
        'ReDim Preserve Xj(2)
        'ReDim Preserve Yj(2)
        'ReDim Preserve Zj(2)
        'ReDim Preserve Xi(2)
        'ReDim Preserve Yi(2)
        'ReDim Preserve Zi(2)
        Dim gmat As New GeoMatrix
        Dim hhmm As HTuple = Nothing
        correspond_3d_3d_weight(Xs, Ys, Zs, Xj, Yj, Zj, Xi, Yi, Zi, hhmm)
        Dim i As Integer, j As Integer
        For i = 1 To 4
            For j = 1 To 4
                gmat.SetAt(i, j, 0.0)
            Next j
        Next i
        For i = 1 To 4
            For j = 1 To 3
                gmat.SetAt(i, j, hhmm((j - 1) * 4 + (i - 1)))
            Next j
        Next i
        gmat.SetAt(4, 4, 1.0)
        '20170330 baluu del start
        'If iRet <> 0 Then
        'End If

        '--5.座標変換マトリックスを作成する

        'Dim gpOrg As New GeoPoint
        'Dim gvX As New GeoVector, gvY As New GeoVector, gvZ As New GeoVector
        'Call gpOrg.setXYZ(Xa(3), Ya(3), Za(3))
        'Call gvX.setXYZ(Xa(4), Ya(4), Za(4))
        'Call gvY.setXYZ(Xa(5), Ya(5), Za(5))
        'Call gvZ.setXYZ(Xa(6), Ya(6), Za(6))
        'Call gvX.SubtractPoint(gpOrg)
        'Call gvY.SubtractPoint(gpOrg)
        'Call gvZ.SubtractPoint(gpOrg)
        'Call gmat.SetCoordSystem(gpOrg, gvX, gvY, gvZ)
        ''gpOrg.Transform()
        ''gmat.PrintClass
        'Call gmat.Invert()
        ''sys_CoordInfo.mat_geo = gmat.Copy
        ''gmat.PrintClass
        '20170330 baluu del end

        '--6.結果を変換
        Dim ind As Integer
        ReDim dblMat(0 To 4 * 4)
        ind = -1
        For i = 1 To 4
            For j = 1 To 4
                ind = ind + 1
                dblMat(ind) = gmat.GetAt(i, j)
            Next j
        Next i
        YCM_Get3PosMatrix = 0
        Exit Function
Err_lbl:
    End Function
    Public Function YCM_UpDateLookPointReal() As Integer
        YCM_UpDateLookPointReal = 0
        For ii = 0 To nLookPoints - 1
            YCM_LookPointMatPoint(gDrawPoints(ii), sys_CoordInfo.mat)
            gDrawPoints(ii).Real_x = gDrawPoints(ii).Real_x * sys_ScaleInfo.scale
            gDrawPoints(ii).Real_y = gDrawPoints(ii).Real_y * sys_ScaleInfo.scale
            gDrawPoints(ii).Real_z = gDrawPoints(ii).Real_z * sys_ScaleInfo.scale
        Next
    End Function
    Public Function YCM_UpDateLookPointDateGrid() As Integer
        YCM_UpDateLookPointDateGrid = 0
        Data_Point.DGV_DV.Rows.Clear()
        For ii = 0 To nLookPoints - 1
            'Data_Point.DGV_DV.Rows.Add()

            gDrawPoints(ii).Real_x = gDrawPoints(ii).Real_x * sys_ScaleInfo.scale
            gDrawPoints(ii).Real_y = gDrawPoints(ii).Real_y * sys_ScaleInfo.scale
            gDrawPoints(ii).Real_z = gDrawPoints(ii).Real_z * sys_ScaleInfo.scale
        Next
    End Function
    Public Function YCM_GetUnitMat() As Double()
        Dim matunit(16) As Double
        matunit(0) = 1 : matunit(1) = 0 : matunit(2) = 0 : matunit(3) = 0
        matunit(4) = 0 : matunit(5) = 1 : matunit(6) = 0 : matunit(7) = 0
        matunit(8) = 0 : matunit(9) = 0 : matunit(10) = 1 : matunit(11) = 0
        matunit(12) = 0 : matunit(13) = 0 : matunit(14) = 0 : matunit(15) = 1
        Return matunit
    End Function
    Public Function YCM_LookPointMatGeoPoint(ByRef lookpoint As GeoPoint, ByVal mat() As Double) As Integer
        YCM_LookPointMatGeoPoint = 0
        Dim lookmat(0 To 3) As Double
        Dim XM(4, 4) As Double
        lookmat(0) = lookpoint.x : lookmat(1) = lookpoint.y : lookmat(2) = lookpoint.z : lookmat(3) = 0

        lookmat(0) = lookpoint.x * mat(0) + lookpoint.y * mat(4) + lookpoint.z * mat(8) + 1.0 * mat(12)
        lookmat(1) = lookpoint.x * mat(1) + lookpoint.y * mat(5) + lookpoint.z * mat(9) + 1.0 * mat(13)
        lookmat(2) = lookpoint.x * mat(2) + lookpoint.y * mat(6) + lookpoint.z * mat(10) + 1.0 * mat(14)

        lookpoint.x = lookmat(0) : lookpoint.y = lookmat(1) : lookpoint.z = lookmat(2)
    End Function
    Public Function YCM_LookPointMatPoint(ByRef lookpoint As CLookPoint, ByVal mat() As Double) As Integer

        Dim lookmat(0 To 3) As Double
        Dim XM(4, 4) As Double
        lookmat(0) = lookpoint.x : lookmat(1) = lookpoint.y : lookmat(2) = lookpoint.z : lookmat(3) = 0

        lookmat(0) = lookpoint.x * mat(0) + lookpoint.y * mat(4) + lookpoint.z * mat(8) + 1.0 * mat(12)
        lookmat(1) = lookpoint.x * mat(1) + lookpoint.y * mat(5) + lookpoint.z * mat(9) + 1.0 * mat(13)
        lookmat(2) = lookpoint.x * mat(2) + lookpoint.y * mat(6) + lookpoint.z * mat(10) + 1.0 * mat(14)

        lookpoint.Real_x = lookmat(0) : lookpoint.Real_y = lookmat(1) : lookpoint.Real_z = lookmat(2)

    End Function
    Public Function YCM_LookPointMatPoint_New(ByRef lookpoint As CLookPoint, ByVal mat() As Double) As Integer

        Dim geoP As New GeoPoint
        geoP.setXYZ(lookpoint.x, lookpoint.y, lookpoint.z)
        geoP.Transform(sys_CoordInfo.mat_geo)

        lookpoint.Real_x = geoP.x
        lookpoint.Real_y = geoP.y
        lookpoint.Real_z = geoP.z

    End Function

    Public Function YCM_RayPointMatRay(ByRef elemray As CRay, ByVal mat() As Double) As Integer

        Dim lookmat(0 To 3) As Double

        lookmat(0) = elemray.startPnt.x * mat(0) + elemray.startPnt.y * mat(4) + elemray.startPnt.z * mat(8) + 1.0 * mat(12)
        lookmat(1) = elemray.startPnt.x * mat(1) + elemray.startPnt.y * mat(5) + elemray.startPnt.z * mat(9) + 1.0 * mat(13)
        lookmat(2) = elemray.startPnt.x * mat(2) + elemray.startPnt.y * mat(6) + elemray.startPnt.z * mat(10) + 1.0 * mat(14)

        elemray.startPnt.x = lookmat(0) : elemray.startPnt.y = lookmat(1) : elemray.startPnt.z = lookmat(2)

        lookmat(0) = elemray.endPnt.x * mat(0) + elemray.endPnt.y * mat(4) + elemray.endPnt.z * mat(8) + 1.0 * mat(12)
        lookmat(1) = elemray.endPnt.x * mat(1) + elemray.endPnt.y * mat(5) + elemray.endPnt.z * mat(9) + 1.0 * mat(13)
        lookmat(2) = elemray.endPnt.x * mat(2) + elemray.endPnt.y * mat(6) + elemray.endPnt.z * mat(10) + 1.0 * mat(14)

        elemray.endPnt.x = lookmat(0) : elemray.endPnt.y = lookmat(1) : elemray.endPnt.z = lookmat(2)

    End Function
    Public Function YCM_CameraPointMatPoint(ByRef elecamera As CCamera, ByVal mat() As Double) As Integer

        Dim lookmat(0 To 3) As Double

        lookmat(0) = elecamera.x * mat(0) + elecamera.y * mat(4) + elecamera.z * mat(8) + 1.0 * mat(12)
        lookmat(1) = elecamera.x * mat(1) + elecamera.y * mat(5) + elecamera.z * mat(9) + 1.0 * mat(13)
        lookmat(2) = elecamera.x * mat(2) + elecamera.y * mat(6) + elecamera.z * mat(10) + 1.0 * mat(14)

        elecamera.x = lookmat(0) : elecamera.y = lookmat(1) : elecamera.z = lookmat(2)

    End Function
    Public Function YCM_LabelPointMatPoint(ByRef elelabel As CLabelText, ByVal mat() As Double) As Integer

        Dim lookmat(0 To 3) As Double

        lookmat(0) = elelabel.x * mat(0) + elelabel.y * mat(4) + elelabel.z * mat(8) + 1.0 * mat(12)
        lookmat(1) = elelabel.x * mat(1) + elelabel.y * mat(5) + elelabel.z * mat(9) + 1.0 * mat(13)
        lookmat(2) = elelabel.x * mat(2) + elelabel.y * mat(6) + elelabel.z * mat(10) + 1.0 * mat(14)

        elelabel.x = lookmat(0) : elelabel.y = lookmat(1) : elelabel.z = lookmat(2)

    End Function
    Public Function YCM_PickPoint2TruePoint(ByVal screenx As Integer, ByVal screeny As Integer, ByRef posx As Double, ByRef posy As Double, ByRef posz As Double) As Integer
        Dim fdepth As Double
        Dim ObjectX, ObjectY, ObjectZ As Double
        Dim iViewPort(4) As Integer
        Dim dProjMatrix(16) As Double
        Dim dModelMatrix(16) As Double
        Dim k_mat As Integer = 4
        Dim mat_lib As New MatLib
        Dim resultM(k_mat, k_mat) As Double
        Dim resultMNew(k_mat * k_mat) As Double
        Dim rotateXM(k_mat, k_mat) As Double, rotateYM(k_mat, k_mat) As Double, rotateZM(k_mat, k_mat) As Double
        Dim moveM(k_mat, k_mat) As Double
        rotateXM = mat_lib.GetRoateYMat(yangle)
        rotateYM = mat_lib.GetRoateXMat(-xangle)
        rotateZM = mat_lib.GetRoateXMat(0)
        'moveM = mat_lib.GetMoveMat(0.0, 0.0, 0.0)
        resultM = mat_lib.Multiply(rotateZM, rotateYM)
        resultM = mat_lib.Multiply(resultM, rotateXM)
        'resultM = mat_lib.Multiply(resultM, moveM)
        resultMNew = mat_lib.Mat2OpenglMat(resultM)

        glPushMatrix()
        glMultMatrixd(resultMNew)
        glGetDoublev(GL_MODELVIEW_MATRIX, dModelMatrix)
        glGetDoublev(GL_PROJECTION_MATRIX, dProjMatrix)

        glGetIntegerv(GL_VIEWPORT, iViewPort)
        'glReadBuffer(GL_BACK)
        'cp.x = iScreen.X
        'cp.y = iScreen.Y
        'iScrToWinX = iScreen.X - cp.x
        'iScrToWinY = iScreen.Y - cp.y
        Dim winx As Double
        Dim winy As Integer
        winx = CDbl(screenx)
        winy = CDbl(iViewPort(3) - screeny)
        fdepth = 0

        glReadPixels(screenx, screeny - iViewPort(3), 1, 1, GL_DEPTH_COMPONENT, GL_FLOAT, fdepth)

        gluUnProject(screenx, iViewPort(3) - screeny, fdepth, dModelMatrix, dProjMatrix, iViewPort, ObjectX, ObjectY, ObjectZ)
        posx = ObjectX
        posy = ObjectY
        posz = ObjectZ - 100
        glPopMatrix()
        'MsgBox(ObjectX & "," & ObjectY & "," & ObjectZ)
    End Function

    Public Sub YMC_3DViewReDraw(ByVal strDataPath As String)

        TimeMonStart()


        Dim db_b As Boolean = False
        Dim clsOPe As New CDBOperate
        m_strDataBasePath = strDataPath
        If clsOPe.ConnectDB(m_strDataBasePath) = False Then
            MsgBox("DB OpenError!" & m_strDataBasePath)
            Exit Sub
        End If
        'clsOPe.DisConnectDB()

        Dim strSQL As String = "SELECT currentlabel as 測点名,X,Y,Z,ID FROM measurepoint3d  where flgLabel=1"
        'Dim adoRet As ADODB.Recordset

        Dim intmat As Integer
        Dim dblmat(1, 15) As Double
        YCM_ReadSystemscalesettingAcs(m_strDataBasePath)
        YCM_ReadSystemscalesettingAcsvalue(m_strDataBasePath)
        intmat = YCM_ReadSystemscalesettingAcsmat(m_strDataBasePath)

        If intmat = 1 Then
            sys_CoordInfo.mat = YCM_GetUnitMat()
        End If
        If intmat = 2 Then
            For ii As Integer = 0 To 15
                sys_CoordInfo.mat(ii) = CDbl(system_scalesettingmat(0, ii))
            Next
        End If
        If nscalesettingvalue = 0 Then
            sys_ScaleInfo.scale = 1.0
        Else
            sys_ScaleInfo.scale = System_scalesettingvalue(0, nscalesetting - 1)
        End If
        'adoRet = clsOPe.CreateRecordset(strSQL)

        'If adoRet.RecordCount > 0 Then
        'If db_b = False Then
        'For i As Integer = 1 To 4
        '    Data_Point.DGV_DV.Columns.RemoveAt(1)
        'Next
        'End If
        'db_b = True
        'Data_Point.DGV_DV.DataSource = ZS_GetDataSource(strSQL)
        'Data_Point.DGV_DV.Columns(0).Width = 38
        'Data_Point.DGV_DV.Columns(1).Width = 63
        'Data_Point.DGV_DV.Columns(2).Width = 85
        'Data_Point.DGV_DV.Columns(3).Width = 85
        'Data_Point.DGV_DV.Columns(4).Width = 85
        'Data_Point.DGV_DV.Columns(5).Visible = False
        ''End If
        'F() 'or i As Integer=
        'nDGV = Data_Point.DGV_DV.RowCount
        'For i As Integer = 0 To Data_Point.DGV_DV.RowCount - 1
        '    Data_Point.DGV_DV.Rows(i).Cells(0).Value = True
        'Next
        'Data_Point = New Data_Vertex
        Data_Point.DGV_DV.Rows.Clear()
        '要素をなくす
        ReDim gDrawRays(0)
        ReDim gDrawCamers(0)
        ReDim gDrawPoints(0)
        ReDim gDrawSekPoints(0) '20170221 baluu add
        ReDim gDrawLabelText(0)
        ReDim gDrawSekLabelText(0) '20170221 baluu add
        ReDim gDrawUserLines(0)
        ReDim gDrawCircleNew(0)
        nRays = 0
        nCamers = 0
        nLookPoints = 0
        nSekPoints = 0 '20170221 baluu add
        nLabelText = 0
        nUserLines = 0
        nCircleNew = 0
        Try
            Command_LoadToDb()
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
        'Data_Point.DGV_DV.DataSource = Nothing
        YCM_ReadZumenInfoFrmAcs(m_strDataBasePath)
        YCM_ReadSystemColorAcs(m_strDataSystemPath)
        YCM_ReadSystemLineTypes(m_strDataSystemPath)
        YCM_ReadEntSetting(m_strDataBasePath)

        '(20160714 Tezuka ADD) 定規のCT番号を得る
        Dim iNum As Integer
        Dim strCtNo(100) As String
        iNum = YCM_ScaleCTNumberGet(m_strDataBasePath, strCtNo)
        '(20160714 Tezuka ADD End)

        Dim iCnt As Integer = 0
        For i As Integer = 0 To nLookPoints - 1

            '(20151207 Tezuka ADD) 小番号の表示・非表示
            If Data_Point.Combo_Child.SelectedIndex = 1 Then
                If gDrawPoints(i).LabelName.IndexOf("_"c) > 0 Then
                    Continue For
                End If
            End If

            '(20151207 Tezuka ADD) STの表示・非表示
            If Data_Point.Combo_Single.SelectedIndex = 1 Then
                If gDrawPoints(i).LabelName.Substring(0, 2) = "ST" Then
                    Continue For
                End If
            End If

            '(20160714 Tezuka ADD) 定規の表示・非表示
            Dim iFlg As Integer = 0
            Dim strLabel As String
            Dim iAnder As Integer
            If (Data_Point.Combo_Scale.SelectedIndex = 1) And (iNum > 0) Then
                For j As Integer = 1 To iNum
                    iAnder = gDrawPoints(i).LabelName.IndexOf("_"c)
                    If iAnder > 0 Then
                        strLabel = Trim(gDrawPoints(i).LabelName.Substring(0, iAnder))
                    Else
                        strLabel = Trim(gDrawPoints(i).LabelName)
                    End If
                    If strLabel = Trim(strCtNo(j)) Then
                        iFlg = 1
                        Exit For
                    End If
                Next j
                If iFlg = 1 Then
                    Continue For
                End If
            End If

            Data_Point.DGV_DV.Rows.Add()
            'Data_Point.DGV_DV.Rows(i).Cells(0).Value = True
            'Data_Point.DGV_DV.Rows(i).Cells(1).Value = gDrawPoints(i).LabelName
            'Data_Point.DGV_DV.Rows(i).Cells(2).Value = gDrawPoints(i).Real_x
            'Data_Point.DGV_DV.Rows(i).Cells(3).Value = gDrawPoints(i).Real_y
            'Data_Point.DGV_DV.Rows(i).Cells(4).Value = gDrawPoints(i).Real_z
            'Data_Point.DGV_DV.Rows(i).Cells(5).Value = gDrawPoints(i).mid
            Data_Point.DGV_DV.Rows(iCnt).Cells(0).Value = True
            Data_Point.DGV_DV.Rows(iCnt).Cells(1).Value = gDrawPoints(i).LabelName
            Data_Point.DGV_DV.Rows(iCnt).Cells(2).Value = gDrawPoints(i).Real_x
            Data_Point.DGV_DV.Rows(iCnt).Cells(3).Value = gDrawPoints(i).Real_y
            Data_Point.DGV_DV.Rows(iCnt).Cells(4).Value = gDrawPoints(i).Real_z
            Data_Point.DGV_DV.Rows(iCnt).Cells(5).Value = gDrawPoints(i).mid
            'Data_Point.DGV_DV.Rows(iCnt).Cells(6).Value = gDrawPoints(i).LabelName.Substring(0, 2) & "0" & gDrawPoints(i).LabelName.Substring(2)
            Dim intAnder As Integer = gDrawPoints(i).LabelName.IndexOf("_"c)
            Dim strTid As String
            Dim strTid2 As String
            If intAnder > 0 Then
                strTid = Trim(gDrawPoints(i).LabelName.Substring(2, intAnder - 2))
                strTid2 = Trim(gDrawPoints(i).LabelName.Substring(intAnder + 1))
            Else
                strTid = Trim(gDrawPoints(i).LabelName.Substring(2))
                strTid2 = ""
            End If
            Dim strTTid As String = ""
            If strTid.Count = 1 Then
                strTTid = gDrawPoints(i).LabelName.Substring(0, 2) & "00000" & gDrawPoints(i).LabelName.Substring(2) & strTid2
            ElseIf strTid.Count = 2 Then
                strTTid = gDrawPoints(i).LabelName.Substring(0, 2) & "0000" & gDrawPoints(i).LabelName.Substring(2) & strTid2
            ElseIf strTid.Count = 3 Then
                strTTid = gDrawPoints(i).LabelName.Substring(0, 2) & "000" & gDrawPoints(i).LabelName.Substring(2) & strTid2
            ElseIf strTid.Count = 4 Then
                strTTid = gDrawPoints(i).LabelName.Substring(0, 2) & "00" & gDrawPoints(i).LabelName.Substring(2) & strTid2
            ElseIf strTid.Count = 5 Then
                strTTid = gDrawPoints(i).LabelName.Substring(0, 2) & "0" & gDrawPoints(i).LabelName.Substring(2) & strTid2
            Else
                strTTid = gDrawPoints(i).LabelName & strTid2
            End If
            Data_Point.DGV_DV.Rows(iCnt).Cells(6).Value = strTTid
            iCnt += 1
        Next
        point_name = New ArrayList
        For i As Integer = 0 To Data_Point.DGV_DV.RowCount - 1
            point_name.Add(Data_Point.DGV_DV.Rows(i).Cells(1).Value)
        Next
        If nCamers > 0 Then
            For ii = 0 To nCamers - 1
                All_YCM3DVIEW_camer(gDrawCamers(ii))
            Next
        End If
        If nLookPoints > 0 Then
            For ii = 0 To nLookPoints - 1

                '(20151207 Tezuka ADD) STの表示・非表示
                If Data_Point.Combo_Single.SelectedIndex = 1 Then
                    If gDrawPoints(ii).LabelName.Substring(0, 2) = "ST" Then
                        Continue For
                    End If
                End If

                '(20160714 Tezuka ADD) 定規の表示・非表示
                Dim iFlg As Integer = 0
                Dim strLabel As String
                Dim iAnder As Integer
                If (Data_Point.Combo_Scale.SelectedIndex = 1) And (iNum > 0) Then
                    For jj As Integer = 1 To iNum
                        iAnder = gDrawPoints(ii).LabelName.IndexOf("_"c)
                        If iAnder > 0 Then
                            strLabel = Trim(gDrawPoints(ii).LabelName.Substring(0, iAnder))
                        Else
                            strLabel = Trim(gDrawPoints(ii).LabelName)
                        End If
                        If strLabel = Trim(strCtNo(jj)) Then
                            iFlg = 1
                            Exit For
                        End If
                    Next jj
                    If iFlg = 1 Then
                        Continue For
                    End If
                End If

                All_YCM3DVIEW_point(gDrawPoints(ii))
            Next
        End If

        '--del.rep.start-------------------------12.10.17
#If 0 Then
        If entset_point.blnVisiable Then
            MainFrm.MM_V_9_1.Checked = True
        Else
            MainFrm.MM_V_9_1.Checked = False
        End If
        ''================================================================20121115
        'If entset_point1.blnVisiable Then'★20121114計測点(ターゲット面)
        '    MainFrm.MM_V_9_1.Checked = True
        'Else
        '    MainFrm.MM_V_9_1.Checked = False
        'End If

        'If entset_point2.blnVisiable Then'★20121114計測点(中心円)
        '    MainFrm.MM_V_9_1.Checked = True
        'Else
        '    MainFrm.MM_V_9_1.Checked = False
        'End If

        'If entset_point3.blnVisiable Then'★20121114計測点(十字線)
        '    MainFrm.MM_V_9_1.Checked = True
        'Else
        '    MainFrm.MM_V_9_1.Checked = False
        'End If
        ''================================================================20121115

        If entset_camera.blnVisiable Then
            MainFrm.MM_V_9_2.Checked = True
        Else
            MainFrm.MM_V_9_2.Checked = False
        End If

        If entset_ray.blnVisiable Then
            MainFrm.MM_V_9_3.Checked = True
        Else
            MainFrm.MM_V_9_3.Checked = False
        End If

        If entset_label.blnVisiable Then
            MainFrm.MM_V_9_4.Checked = True
        Else
            MainFrm.MM_V_9_4.Checked = False
        End If

        If entset_line.blnVisiable And entset_circle.blnVisiable Then
            MainFrm.MM_V_9_5.Checked = True
        Else
            MainFrm.MM_V_9_5.Checked = False
        End If
#End If
        '--del.rep.end---------------------------12.10.17
        'SUSANO add start 20160818
        If (Not sys_ScaleInfo.p1 Is Nothing And Not sys_ScaleInfo.p2 Is Nothing) Then
            sys_ScaleInfo.p1 = getDrawPointByMID(sys_ScaleInfo.p1.mid)
            sys_ScaleInfo.p2 = getDrawPointByMID(sys_ScaleInfo.p2.mid)
        End If
        YCM_GenScaleLine(sys_ScaleInfo.p1, sys_ScaleInfo.p2)
        'SUSANO add end 20160818

        sys_ScaleInfo.p1 = New CLookPoint
        sys_ScaleInfo.p2 = New CLookPoint
        sys_CoordInfo.pOrg = New CLookPoint
        sys_CoordInfo.p1 = New CLookPoint
        sys_CoordInfo.p2 = New CLookPoint
        sys_CoordInfo.strP1XYZ = "+X"
        sys_CoordInfo.strP2XYZ = "+Y"
        sys_CoordInfo.intOxyais = 1
        sys_DrawCircle.dblR = 0.0#
        sys_DrawCircle.iCombox = 0
        NewOrOld = 2
        binfirstopen = True
        suballcanview()
        bintemptest = True

        ' X-Z表示、最大表示、レイなし

        entset_ray.blnVisiable = True
        '--del.rep.start-------------------------12.10.17
        ' リボンメニューに表示／非表示の状態を反映
        Call setRibbonMenuChkOnOff()
#If 0 Then
        MainFrm.MM_V_9_3.Checked = False
        'MainFrm.MM_V_8_2.Checked = True
#End If
        '--del.rep.end---------------------------12.10.17
        Call ZoomAll()
        Call Command_XY()
        Call ZoomAll()

        NewOrOld = 2

        TimeMonEnd()

    End Sub



    Public Sub YMC_3DViewReDraw_NoKousin(ByVal strDataPath As String)
        Dim db_b As Boolean = False
        Dim clsOPe As New CDBOperate
        m_strDataBasePath = strDataPath
        If clsOPe.ConnectDB(m_strDataBasePath) = False Then
            MsgBox("DB OpenError!" & m_strDataBasePath)
            Exit Sub
        End If



        Dim dblmat(1, 15) As Double

        Data_Point.DGV_DV.Rows.Clear()
        '要素をなくす
        ReDim gDrawRays(0)
        ReDim gDrawCamers(0)
        ReDim gDrawPoints(0)
        ReDim gDrawSekPoints(0) '20170221 baluu add
        ReDim gDrawLabelText(0)
        ReDim gDrawSekLabelText(0) '20170221 baluu add
        ReDim gDrawUserLines(0)
        ReDim gDrawCircleNew(0)
        nRays = 0
        nCamers = 0
        nLookPoints = 0
        nSekPoints = 0 '20170221 baluu add
        nLabelText = 0
        nUserLines = 0
        nCircleNew = 0
        Try
            Command_LoadToDb()
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
        'Data_Point.DGV_DV.DataSource = Nothing
        YCM_ReadZumenInfoFrmAcs(m_strDataBasePath)
        'YCM_ReadSystemColorAcs(m_strDataSystemPath)
        'YCM_ReadSystemLineTypes(m_strDataSystemPath)
        'YCM_ReadEntSetting(m_strDataBasePath)
        For i As Integer = 0 To nLookPoints - 1
            Data_Point.DGV_DV.Rows.Add()
            Data_Point.DGV_DV.Rows(i).Cells(0).Value = True
            Data_Point.DGV_DV.Rows(i).Cells(1).Value = gDrawPoints(i).LabelName
            Data_Point.DGV_DV.Rows(i).Cells(2).Value = gDrawPoints(i).Real_x
            Data_Point.DGV_DV.Rows(i).Cells(3).Value = gDrawPoints(i).Real_y
            Data_Point.DGV_DV.Rows(i).Cells(4).Value = gDrawPoints(i).Real_z
            Data_Point.DGV_DV.Rows(i).Cells(5).Value = gDrawPoints(i).mid
        Next
        point_name = New ArrayList
        For i As Integer = 0 To Data_Point.DGV_DV.RowCount - 1
            point_name.Add(Data_Point.DGV_DV.Rows(i).Cells(1).Value)
        Next
        'If nCamers > 0 Then
        '    For ii = 0 To nCamers - 1
        '        All_YCM3DVIEW_camer(gDrawCamers(ii))
        '    Next
        'End If
        'If nLookPoints > 0 Then
        '    For ii = 0 To nLookPoints - 1
        '        All_YCM3DVIEW_point(gDrawPoints(ii))
        '    Next
        'End If



        NewOrOld = 2
    End Sub



    Public Sub ZoomAll() '全体表示
        '--org.start-------------
        '        If binfirstopen = True Then
        '            View_Dilaog.dblwindowLeft = View_Dilaog_dblwindowLeft
        '            View_Dilaog.dblwindowRight = View_Dilaog_dblwindowRight
        '            View_Dilaog.dblwindowBottom = View_Dilaog_dblwindowBottom
        '            View_Dilaog.dblwindowTop = View_Dilaog_dblwindowTop
        '
        '            ents_centerC.x = cent_cp.x  'maxandmin_all(6)
        '            ents_centerC.y = cent_cp.y  ' maxandmin_all(7)
        '            ents_centerC.z = cent_cp.z  'maxandmin_all(8)
        '        End If
        '        'xangle = 0
        '        'yangle = 0
        '--org.end----------------
        Dim k_mat As Integer = 4
        Dim mat_lib2 As New MatLib
        Dim resultM(k_mat, k_mat) As Double
        Dim resultMNew(k_mat * k_mat) As Double
        Dim rotateXM(k_mat, k_mat) As Double, rotateYM(k_mat, k_mat) As Double, rotateZM(k_mat, k_mat) As Double
        Dim moveM(k_mat, k_mat) As Double
        rotateXM = mat_lib2.GetRoateYMat(-yangle)
        rotateYM = mat_lib2.GetRoateXMat(xangle)
        'rotateZM = mat_lib2.GetRoateZMat(0)
        'moveM = mat_lib.GetMoveMat(0.0, 0.0, 0.0)
        resultM = mat_lib2.Multiply(rotateXM, rotateYM)
        'resultM = mat_lib2.Multiply(resultM, rotateZM)
        'resultM = mat_li2b.Multiply(resultM, moveM)
        resultMNew = mat_lib2.Mat2OpenglMat(resultM)
        Dim dblallpoint(2, nLookPoints + nCamers - 1) As Double
        'Dim dblpoint(nLookPoints - 1) As CLookPoint
        'Dim dblcamer(nCamers - 1) As CLookPoint
        'ReDim dblpoint(nLookPoints - 1)
        'ReDim dblcamer(nCamers - 1)
        Dim geoP As New GeoPoint
        For ii As Integer = 0 To nLookPoints - 1
            If gDrawPoints(ii).blnDraw = True Then
                geoP.setXYZ(gDrawPoints(ii).x, gDrawPoints(ii).y, gDrawPoints(ii).z)
                YCM_LookPointMatGeoPoint(geoP, resultMNew)
            End If
            dblallpoint(0, ii) = geoP.x
            dblallpoint(1, ii) = geoP.y
            dblallpoint(2, ii) = geoP.z

        Next
        For ii As Integer = nLookPoints To nLookPoints + nCamers - 1
            If gDrawCamers(ii - nLookPoints).blnDraw = True Then
                geoP.setXYZ(gDrawCamers(ii - nLookPoints).x, gDrawCamers(ii - nLookPoints).y, gDrawCamers(ii - nLookPoints).z)
                YCM_LookPointMatGeoPoint(geoP, resultMNew)
            End If
            dblallpoint(0, ii) = geoP.x
            dblallpoint(1, ii) = geoP.y
            dblallpoint(2, ii) = geoP.z
        Next
        YCM_AllEntitesScreen(dblallpoint)
    End Sub


    Public Function YCM_UpdataCoordsettingvalue(ByVal strDBPath As String, ByVal arrupdata As ArrayList)
        Dim clsOPe As New CDBOperate
        Dim adoRet As ADODB.Recordset
        If clsOPe.ConnectDB(strDBPath) = False Then
            YCM_UpdataCoordsettingvalue = -1
        End If
        Dim strSQL As String
        strSQL = "DELETE * FROM transformsetting"
        clsOPe.ExcuteSQL(strSQL)

        strSQL = "INSERt INTO transformsetting(PID1,PID2,PID3,PID1方向ID,PID2方向ID,PID3方向ID,固定軸ID) VALUES(" & arrupdata(0) & "," & arrupdata(1) & "," & arrupdata(2) & "," & arrupdata(3) & "," & arrupdata(4) & "," & arrupdata(5) & "," & arrupdata(6) & ")"
        adoRet = clsOPe.CreateRecordset(strSQL)
        clsOPe.DisConnectDB()
        YCM_UpdataCoordsettingvalue = 2
    End Function
    Public Function YCM_UpdataCoordsetting(ByVal strDBPath As String, ByVal dblupdatamat() As Double)
        Dim clsOPe As New CDBOperate
        Dim adoRet As ADODB.Recordset
        If clsOPe.ConnectDB(strDBPath) = False Then
            YCM_UpdataCoordsetting = -1
        End If
        Dim strSQL As String
        strSQL = "DELETE * FROM transformmatrix"
        clsOPe.ExcuteSQL(strSQL)
        strSQL = "INSERt INTO transformmatrix(mat11,mat12,mat13,mat14,mat21,mat22,mat23,mat24,mat31,mat32,mat33,mat34,mat41,mat42,mat43,mat44) VALUES(" & dblupdatamat(0) & "," & dblupdatamat(1) & "," & dblupdatamat(2) & "," & dblupdatamat(3) & "," & dblupdatamat(4) & "," & dblupdatamat(5) & "," & dblupdatamat(6) & "," & dblupdatamat(7) & "," & dblupdatamat(8) & "," & dblupdatamat(9) & "," & dblupdatamat(10) & "," & dblupdatamat(11) & "," & dblupdatamat(12) & "," & dblupdatamat(13) & "," & dblupdatamat(14) & "," & dblupdatamat(15) & ")"
        adoRet = clsOPe.CreateRecordset(strSQL)
        clsOPe.DisConnectDB()
        YCM_UpdataCoordsetting = 2
    End Function
    Public Function YCM_Updatalabelresult(ByVal strDBPath As String, ByVal arrupdata As ArrayList, ByVal arrupdatanew As ArrayList)
        Dim clsOPe As New CDBOperate
        'Dim adoRet As ADODB.Recordset
        If clsOPe.ConnectDB(strDBPath) = False Then
            YCM_Updatalabelresult = -1
        End If
        Dim strSQL As String
        'Dim strSQLFnis As String
        Dim blnRun As Boolean
        For ii As Integer = 0 To arrupdatanew.Count - 1
            strSQL = "UPDATE measurepoint3d set userlabel = '" & arrupdatanew(ii).value & "',currentlabel = '" & arrupdatanew(ii).value & "' WHERE currentlabel = '" & arrupdata(ii).value & "'"
            blnRun = clsOPe.ExcuteSQL(strSQL)
        Next
        'strSQLFnis = strSQL & strSQLFnis
        'adoRet = clsOPe.CreateRecordset(strSQLFnis)
        clsOPe.DisConnectDB()
        YCM_Updatalabelresult = 2
    End Function

    'SUSANO ADD START
    Public Function YCM_Updatalabelresult(ByVal strDBPath As String, ByVal arrupdata As List(Of String), ByVal arrupdatanew As List(Of String))
        Dim clsOPe As New CDBOperate
        If clsOPe.ConnectDB(strDBPath) = False Then
            YCM_Updatalabelresult = -1
        End If
        Dim strSQL As String
        Dim blnRun As Boolean
        For ii As Integer = 0 To arrupdatanew.Count - 1
            strSQL = "UPDATE measurepoint3d set userlabel = '" & arrupdatanew.Item(ii) & "',currentlabel = '" & arrupdatanew.Item(ii) & "' WHERE currentlabel = '" & arrupdata.Item(ii) & "'"
            blnRun = clsOPe.ExcuteSQL(strSQL)
        Next
        clsOPe.DisConnectDB()
        YCM_Updatalabelresult = 2
    End Function

    'SUSANO ADD END
    Public Function YCM_UpdataUserLabel(ByVal strDBPath As String, ByVal saIDArr As StringArray, ByVal saLabelArr As StringArray) As Integer
        Dim strSQL As String, ii As Long
        Dim blnRun As Boolean
        Dim clsOPe As New CDBOperate
        YCM_UpdataUserLabel = -1
        If (saIDArr.Size < 1) Then Exit Function
        If (saLabelArr.Size < 1) Then Exit Function
        If (saIDArr.Size <> saLabelArr.Size) Then Exit Function
        If clsOPe.ConnectDB(strDBPath) = False Then
            YCM_UpdataUserLabel = -1
        End If
        For ii = 0 To saIDArr.Size - 1
            strSQL = "UPDATE measurepoint3d set " & _
                    "userlabel = '" & saLabelArr.at(ii) & _
                    "',currentlabel = '" & Trim(saLabelArr.at(ii)) & _
                    "' WHERE ID = " & CLng(saIDArr.at(ii))
            blnRun = clsOPe.ExcuteSQL(strSQL)
        Next
        clsOPe.DisConnectDB()
        YCM_UpdataUserLabel = 0
    End Function

    Public Sub MsgB_neworopen()
        MsgBox("ファイルを開いてください。 ")
    End Sub
    Public Sub suballcanview()
        Dim max As Double
        max = maxandmin_all(0)
        For ii As Double = 1 To 5
            If maxandmin_all(ii) > maxandmin_all(ii - 1) Then
                max = maxandmin_all(ii)
            End If
        Next

        Dim min As Double
        min = maxandmin_all(0)
        For ii As Double = 1 To 5
            If maxandmin_all(ii) < maxandmin_all(ii - 1) Then
                min = maxandmin_all(ii)
            End If
        Next

        Dim width As Double

        If (max > 0 And min < 0) Or (max < 0 And min > 0) Then
            width = Math.Abs(max) + Math.Abs(min)
        Else
            width = Math.Abs(max) - Math.Abs(min)
        End If
        Dim max_X_maxdis As Double
        If C_X_MaxDis > P_X_MaxDis Then
            max_X_maxdis = C_X_MaxDis
        Else
            max_X_maxdis = P_X_MaxDis
        End If
        Dim max_y_maxdis As Double
        If C_Y_MaxDis > P_Y_MaxDis Then
            max_y_maxdis = C_Y_MaxDis
        Else
            max_y_maxdis = P_Y_MaxDis
        End If
        Dim max_z_maxdis As Double
        If C_Z_MaxDis > P_Z_MaxDis Then
            max_z_maxdis = C_Z_MaxDis
        Else
            max_z_maxdis = P_Z_MaxDis
        End If

        If max_X_maxdis < max_y_maxdis Then
            max_X_maxdis = max_y_maxdis
        End If


        If max_X_maxdis < max_z_maxdis Then
            max_X_maxdis = max_z_maxdis
        End If

        Dim dblRight_Left As Double = View_Dilaog.dblwindowRight / View_Dilaog.dblwindowLeft
        Dim dblBottom_Left As Double = View_Dilaog.dblwindowBottom / View_Dilaog.dblwindowLeft
        Dim dblTop_Left As Double = View_Dilaog.dblwindowTop / View_Dilaog.dblwindowLeft

        max_X_maxdis = max_X_maxdis
        'View_Dilaog.dblwindowLeft = -max_X_maxdis
        'View_Dilaog.dblwindowRight = max_X_maxdis * Math.Abs(dblRight_Left)
        'View_Dilaog.dblwindowBottom = -max_X_maxdis * Math.Abs(dblBottom_Left)
        'View_Dilaog.dblwindowTop = max_X_maxdis * Math.Abs(dblTop_Left)

        View_Dilaog_dblwindowLeft = -max_X_maxdis
        View_Dilaog_dblwindowRight = max_X_maxdis * Math.Abs(dblRight_Left)
        View_Dilaog_dblwindowBottom = -max_X_maxdis * Math.Abs(dblBottom_Left)
        View_Dilaog_dblwindowTop = max_X_maxdis * Math.Abs(dblTop_Left)
    End Sub
    Public Function YCM_AllEntitesScreen(ByVal dblall(,) As Double) As Integer
        On Error Resume Next
        YCM_AllEntitesScreen = 0
        Dim intindexall(5) As Integer
        Dim pi As Double = 3.14159265
        Dim xmax As Double = dblall(0, 0)
        Dim ymax As Double = dblall(1, 0)
        Dim xmin As Double = dblall(0, 0)
        Dim ymin As Double = dblall(1, 0)
        Dim zmin As Double = dblall(2, 0)
        Dim zmax As Double = dblall(2, 0)
        For ii As Integer = 0 To 5
            intindexall(ii) = 0
        Next
        For ii As Integer = 0 To nCamers + nLookPoints - 1
            If dblall(0, ii) > xmax Then
                xmax = dblall(0, ii)
                intindexall(0) = ii
            End If
            If dblall(1, ii) > ymax Then
                ymax = dblall(1, ii)
                intindexall(2) = ii
            End If
            If dblall(0, ii) < xmin Then
                xmin = dblall(0, ii)
                intindexall(1) = ii
            End If
            If dblall(1, ii) < ymin Then
                ymin = dblall(1, ii)
                intindexall(3) = ii
            End If
            If dblall(2, ii) < zmin Then
                zmin = dblall(2, ii)
                intindexall(5) = ii
            End If
            If dblall(2, ii) > zmax Then
                zmax = dblall(2, ii)
                intindexall(4) = ii
            End If
        Next
        Dim k_mat As Integer = 4
        Dim mat_lib As New MatLib
        Dim resultM(k_mat, k_mat) As Double
        Dim resultMNew(k_mat * k_mat) As Double
        Dim rotateXM(k_mat, k_mat) As Double, rotateYM(k_mat, k_mat) As Double, rotateZM(k_mat, k_mat) As Double
        Dim moveM(k_mat, k_mat) As Double

        rotateXM = mat_lib.GetRoateYMat(yangle)
        rotateYM = mat_lib.GetRoateXMat(-xangle)
        rotateZM = mat_lib.GetRoateXMat(0)
        moveM = mat_lib.GetMoveMat(0.0, 0.0, 0.0)
        resultM = mat_lib.Multiply(rotateZM, rotateYM)
        resultM = mat_lib.Multiply(resultM, rotateXM)
        resultM = mat_lib.Multiply(resultM, moveM)
        resultMNew = mat_lib.Mat2OpenglMat(resultM)

        Dim Wud_Center As New GeoPoint
        Wud_Center.setXYZ(xmin / 2 + xmax / 2, ymax / 2 + ymin / 2, zmin / 2 + zmax / 2)
        YCM_LookPointMatGeoPoint(Wud_Center, resultMNew)

        ents_centerC.x = Wud_Center.x
        ents_centerC.y = Wud_Center.y
        ents_centerC.z = Wud_Center.z

        Dim dblwh As Double = Math.Abs((View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom)) / Math.Abs((View_Dilaog.dblwindowRight - View_Dilaog.dblwindowLeft))
        Dim dbltemp As Double = 0.5
        If Math.Abs((xmax - xmin)) * dblwh > Math.Abs((ymax - ymin)) Then
            View_Dilaog.dblwindowLeft = -Math.Abs((xmax - xmin)) * dbltemp
            View_Dilaog.dblwindowRight = Math.Abs((xmax - xmin)) * dbltemp
            View_Dilaog.dblwindowBottom = -Math.Abs((xmax - xmin)) * dblwh * dbltemp
            View_Dilaog.dblwindowTop = Math.Abs((xmax - xmin)) * dblwh * dbltemp
        Else
            View_Dilaog.dblwindowLeft = -Math.Abs((ymax - ymin)) / dblwh * dbltemp
            View_Dilaog.dblwindowRight = Math.Abs((ymax - ymin)) / dblwh * dbltemp
            View_Dilaog.dblwindowBottom = -Math.Abs((ymax - ymin)) * dbltemp
            View_Dilaog.dblwindowTop = Math.Abs((ymax - ymin)) * dbltemp
        End If
    End Function

    '20170209 baluu add start
    Private Function GetCameraColor(ByVal color As Single(), ByVal blnSelected As Boolean) As Single()
        If blnSelected = True Then
            'Return negative colors
            GetCameraColor = {1 - color(0), 1 - color(1), 1 - color(2), 1.0}
        Else
            GetCameraColor = color
        End If
    End Function
    '20170209 baluu add end

#Region "DB関連"

    Public Function YCM_SaveUserFigureToDB() As Integer
        TimeMonStart()

        Dim dbo As New CDBOperate
        Dim strSQL As String
        If dbo.ConnectDB(m_strDataBasePath) = False Then
            YCM_SaveUserFigureToDB = -1
            Exit Function
        End If
        '古いデータを削除
        strSQL = "delete from [userfigure]"
        dbo.ExcuteSQL(strSQL)
        '線分図形データの保存

        For ii As Integer = 0 To nUserLines - 1
            strSQL = "INSERT INTO userfigure " & _
                        " (図形タイプ,レイヤ名,表示フラグ,削除フラグ,線種コード,色コード,Data1,Data2,Data3,Data4,Data5,Data6,create_type)" & _
                        " VALUES (" '20170208 baluu edit (added create_type column)
            If (gDrawUserLines(ii).elmType = 0) Then '--任意図形
                strSQL = strSQL + "'" & CStr(1) & "'" & "," & _
                                  "'" & CStr(entset_line.layerName) & "'" & ","
            Else
                strSQL = strSQL + "'" & CStr(11) & "'" & "," & _
                                  "'" & CStr(entset_line_CAD.layerName) & "'" & ","
            End If
            strSQL = strSQL + "'" & CStr(IIf(gDrawUserLines(ii).blnDraw = True, 1, 0)) & "'" & "," & _
                              "'" & CStr(IIf(gDrawUserLines(ii).binDelete = True, 1, 0)) & "'" & ","
            strSQL = strSQL + "'" & CStr(gDrawUserLines(ii).lineTypeCode) & "'" & "," & _
                              "'" & CStr(gDrawUserLines(ii).colorCode) & "'" & ","
            strSQL = strSQL + "'" & CStr(gDrawUserLines(ii).startPnt.x) & "'" & "," & _
                              "'" & CStr(gDrawUserLines(ii).startPnt.y) & "'" & "," & _
                              "'" & CStr(gDrawUserLines(ii).startPnt.z) & "'" & "," & _
                              "'" & CStr(gDrawUserLines(ii).endPnt.x) & "'" & "," & _
                              "'" & CStr(gDrawUserLines(ii).endPnt.y) & "'" & "," & _
                              "'" & CStr(gDrawUserLines(ii).endPnt.z) & "'" & "," & _
                              "'" & CStr(gDrawUserLines(ii).createType) & "');" '20170208 baluu edit ( added CStr(gDrawUserLines(ii).createType) )
            dbo.ExcuteSQL(strSQL)
        Next
        '円図形データの保存

        For ii As Integer = 0 To nCircleNew - 1
            strSQL = "INSERT INTO userfigure " & _
                        " (図形タイプ,レイヤ名,表示フラグ,削除フラグ,線種コード,色コード,Data1,Data2,Data3,Data4,Data5,Data6,Data7,Data8,Data9,create_type)" & _
                        " VALUES (" '20170208 baluu edit (added create_type column)
            If (gDrawCircleNew(ii).elmType = 0) Then '--任意図形
                strSQL = strSQL + "'" & CStr(2) & "'" & "," & _
                                  "'" & CStr(entset_circle.layerName) & "'" & ","
            Else
                strSQL = strSQL + "'" & CStr(12) & "'" & "," & _
                                  "'" & CStr(entset_circle_CAD.layerName) & "'" & ","
            End If
            strSQL = strSQL + "'" & CStr(IIf(gDrawCircleNew(ii).blnDraw = True, 1, 0)) & "'" & "," & _
                              "'" & CStr(IIf(gDrawCircleNew(ii).binDelete = True, 1, 0)) & "'" & ","
            strSQL = strSQL + "'" & CStr(gDrawCircleNew(ii).lineTypeCode) & "'" & "," & _
                              "'" & CStr(gDrawCircleNew(ii).colorCode) & "'" & ","
            strSQL = strSQL + "'" & CStr(gDrawCircleNew(ii).org.x) & "'" & "," & _
                              "'" & CStr(gDrawCircleNew(ii).org.y) & "'" & "," & _
                              "'" & CStr(gDrawCircleNew(ii).org.z) & "'" & "," & _
                              "'" & CStr(gDrawCircleNew(ii).r) & "'" & "," & _
                              "'" & CStr(gDrawCircleNew(ii).x_angle) & "'" & "," & _
                              "'" & CStr(gDrawCircleNew(ii).y_angle) & "'" & "," & _
                              "'" & CStr(gDrawCircleNew(ii).Vec.x) & "'" & "," & _
                              "'" & CStr(gDrawCircleNew(ii).Vec.y) & "'" & "," & _
                              "'" & CStr(gDrawCircleNew(ii).Vec.z) & "'" & "," & _
                              "'" & CStr(gDrawCircleNew(ii).createType) & "');" '20170208 baluu edit ( added CStr(gDrawUserLines(ii).createType) )
            dbo.ExcuteSQL(strSQL)
        Next
        dbo.DisConnectDB()
        YCM_SaveUserFigureToDB = 0

        TimeMonEnd()
    End Function
    Public Sub Command_LoadToDb()
        Dim elmType As Integer, flg As Integer
        Dim Cdb As New CDBOperate
        Cdb.ConnectDB(m_strDataBasePath)
        Dim strSql As String = ""
        Dim adoRet As ADODB.Recordset

        '20170208 baluu add start
        strSql = "select create_type from userfigure"
        adoRet = Cdb.CreateRecordset(strSql)
        If adoRet Is Nothing Then
            If Not Cdb.ExcuteSQL("ALTER TABLE userfigure ADD COLUMN create_type INTEGER") Then
                Cdb.DisConnectDB()
                Exit Sub
            End If
        Else
            adoRet.Close()
        End If


        '20170208 baluu add end
        strSql = "select * from userfigure where userfigure.図形タイプ = 1 OR userfigure.図形タイプ = 11"

        adoRet = Cdb.CreateRecordset(strSql)
        If adoRet Is Nothing Then
        Else
            If adoRet.RecordCount > 0 Then
                Do Until adoRet.EOF
                    ReDim Preserve gDrawUserLines(nUserLines)
                    gDrawUserLines(nUserLines) = New CUserLine
                    elmType = adoRet("図形タイプ").Value
                    gDrawUserLines(nUserLines).elmType = IIf((elmType < 10), 0, 1)
                    flg = adoRet("表示フラグ").Value
                    gDrawUserLines(nUserLines).blnDraw = IIf((flg = 1), True, False)
                    flg = adoRet("削除フラグ").Value
                    gDrawUserLines(nUserLines).binDelete = IIf((flg = 1), True, False)
                    gDrawUserLines(nUserLines).lineTypeCode = adoRet("線種コード").Value
                    gDrawUserLines(nUserLines).colorCode = adoRet("色コード").Value
                    gDrawUserLines(nUserLines).startPnt.x = adoRet("Data1").Value
                    gDrawUserLines(nUserLines).startPnt.y = adoRet("Data2").Value
                    gDrawUserLines(nUserLines).startPnt.z = adoRet("Data3").Value
                    gDrawUserLines(nUserLines).endPnt.x = adoRet("Data4").Value
                    gDrawUserLines(nUserLines).endPnt.y = adoRet("Data5").Value
                    gDrawUserLines(nUserLines).endPnt.z = adoRet("Data6").Value
                    '20170208 baluu add start
                    If adoRet("create_type").Value Is DBNull.Value Then
                        gDrawUserLines(nUserLines).createType = 0
                    Else
                        gDrawUserLines(nUserLines).createType = adoRet("create_type").Value
                    End If
                    '20170208 baluu add end
                    gDrawUserLines(nUserLines).MID = nUserLines
                    gDrawUserLines(nUserLines).blnDraw = True
                    nUserLines = nUserLines + 1
                    adoRet.MoveNext()
                Loop
            End If
        End If

        strSql = "select * from userfigure where userfigure.図形タイプ = 2 OR userfigure.図形タイプ = 12"
        adoRet = Cdb.CreateRecordset(strSql)
        If adoRet Is Nothing Then
        Else
            If adoRet.RecordCount > 0 Then
                Do Until adoRet.EOF
                    ReDim Preserve gDrawCircleNew(nCircleNew)
                    gDrawCircleNew(nCircleNew) = New Ccircle
                    elmType = adoRet("図形タイプ").Value
                    gDrawCircleNew(nCircleNew).elmType = IIf((elmType < 10), 0, 2)
                    flg = adoRet("表示フラグ").Value
                    gDrawCircleNew(nCircleNew).blnDraw = IIf((flg = 1), True, False)
                    flg = adoRet("削除フラグ").Value
                    gDrawCircleNew(nCircleNew).binDelete = IIf((flg = 1), True, False)
                    gDrawCircleNew(nCircleNew).lineTypeCode = adoRet("線種コード").Value
                    gDrawCircleNew(nCircleNew).colorCode = adoRet("色コード").Value
                    gDrawCircleNew(nCircleNew).org.x = adoRet("Data1").Value
                    gDrawCircleNew(nCircleNew).org.y = adoRet("Data2").Value
                    gDrawCircleNew(nCircleNew).org.z = adoRet("Data3").Value
                    gDrawCircleNew(nCircleNew).r = adoRet("Data4").Value
                    gDrawCircleNew(nCircleNew).x_angle = adoRet("Data5").Value
                    gDrawCircleNew(nCircleNew).y_angle = adoRet("Data6").Value
                    gDrawCircleNew(nCircleNew).Vec.x = adoRet("Data7").Value
                    gDrawCircleNew(nCircleNew).Vec.y = adoRet("Data8").Value
                    gDrawCircleNew(nCircleNew).Vec.z = adoRet("Data9").Value
                    '20170208 baluu add start
                    If adoRet("create_type").Value Is DBNull.Value Then
                        gDrawCircleNew(nCircleNew).createType = 0
                    Else
                        gDrawCircleNew(nCircleNew).createType = adoRet("create_type").Value
                    End If
                    '20170208 baluu add end
                    gDrawCircleNew(nCircleNew).mid = nCircleNew
                    gDrawCircleNew(nCircleNew).blnDraw = True
                    nCircleNew = nCircleNew + 1
                    adoRet.MoveNext()
                Loop
            End If
        End If
        Cdb.DisConnectDB()
    End Sub

#If 1 Then  '-----作成中-----
    '「measurepoint3d」テーブルの更新
    Public Function YCM_UpdateMeasurePoint3d( _
        ByVal strDBPath As String _
    )
        On Error GoTo labError
        Dim clsOPe As New CDBOperate
        If clsOPe.ConnectDB(strDBPath) = False Then
            YCM_UpdateMeasurePoint3d = -1
        End If
        Dim strSQL As String = ""
        Dim blnRun As Boolean
        For ii As Integer = 0 To nLookPoints - 1
            '=0：変更なし、=1:変更(自動側点のラベルを変更)、=2:追加(ユーザ追加点)
            If (gDrawPoints(ii).mode = 1) Then
                strSQL = "UPDATE measurepoint3d " & _
                         " set userlabel = '" & Trim(gDrawPoints(ii).LabelName) & _
                         "',currentlabel = '" & Trim(gDrawPoints(ii).LabelName) & _
                         "' WHERE ID = " & gDrawPoints(ii).mid & ";"
                blnRun = clsOPe.ExcuteSQL(strSQL)
            ElseIf (gDrawPoints(ii).mode = 2) Then
                strSQL = "INSERT INTO measurepoint3d " & _
                         " (ID,TID,Type,systemlabel,currentlabel,X,Y,Z,meanerror,deverror,flgDisplay,flgLabel,create_type)" & _
                         " VALUES (" '20170217 baluu edit ( added create_type column )
                strSQL = strSQL + "'" & CStr(ii + 1) & "'"
                strSQL = strSQL + ",'0'"
                strSQL = strSQL + ",'9'"
                strSQL = strSQL + ",'" & Trim(gDrawPoints(ii).LabelName) & "'"
                strSQL = strSQL + ",'" & Trim(gDrawPoints(ii).LabelName) & "'"
                strSQL = strSQL + ",'" & gDrawPoints(ii).x & "'"
                strSQL = strSQL + ",'" & gDrawPoints(ii).y & "'"
                strSQL = strSQL + ",'" & gDrawPoints(ii).z & "'"
                strSQL = strSQL + ",'0'"
                strSQL = strSQL + ",'0'"
                strSQL = strSQL + ",'" & IIf(gDrawPoints(ii).blnDraw = True, 1, 0) & "'"
                strSQL = strSQL + ",'" & gDrawPoints(ii).flgLabel & "'" '20170217 baluu add
                strSQL = strSQL + ",'" & gDrawPoints(ii).createType & "');" '20170217 baluu edit (flgLabel -> createType)

                blnRun = clsOPe.ExcuteSQL(strSQL)
            End If
        Next ii
        clsOPe.DisConnectDB()
        YCM_UpdateMeasurePoint3d = 2
labError:
    End Function

    '機能：「計測データ.mdb」から座標補間点用のテーブルを読み込む
    Public Function YCM_ReadInterPosInfoTable(ByVal strDBPath As String) As Integer
        On Error GoTo labError
        Dim clsOPe As New CDBOperate
        Dim adoRet As ADODB.Recordset
        If clsOPe.ConnectDB(strDBPath) = False Then
            YCM_ReadInterPosInfoTable = -1
        End If
        Dim strSQL As String
        Dim dirBPIndex(3) As Long
        Dim basePosID As Long, newPosID As Long

        gNumAddMovePos = 0
        If (Not ExistsTable(clsOPe, "[interposinfo]")) Then
            Exit Function
        End If
        strSQL = "SELECT * From [interposinfo]"
        adoRet = clsOPe.CreateRecordset(strSQL)
        If adoRet.RecordCount > 0 Then
            Do Until adoRet.EOF
                ReDim Preserve gAddMovePos(gNumAddMovePos)
                ReDim Preserve gAddMovePos(gNumAddMovePos).dirBasePos(2)
                gAddMovePos(gNumAddMovePos).isDelete = adoRet("flgDel").Value
                gAddMovePos(gNumAddMovePos).iType = adoRet("interposType").Value
                gAddMovePos(gNumAddMovePos).dMoveValue = adoRet("dMoveValue").Value
                gAddMovePos(gNumAddMovePos).strResult = IIf(IsDBNull(adoRet("strResult").Value), "", adoRet("strResult").Value)

                dirBPIndex(0) = IIf(IsDBNull(adoRet("dirBPID1").Value), 0, adoRet("dirBPID1").Value)
                dirBPIndex(1) = IIf(IsDBNull(adoRet("dirBPID2").Value), 0, adoRet("dirBPID2").Value)
                dirBPIndex(2) = IIf(IsDBNull(adoRet("dirBPID3").Value), 0, adoRet("dirBPID3").Value)
                basePosID = IIf(IsDBNull(adoRet("basePosID").Value), 0, adoRet("basePosID").Value)
                newPosID = IIf(IsDBNull(adoRet("newPosID").Value), 0, adoRet("newPosID").Value)

                gAddMovePos(gNumAddMovePos).dirBasePos(0) = getDrawPointByMID(dirBPIndex(0)) 'gDrawPoints(1) 'dirBPIndex(0)から計測点を取得

                gAddMovePos(gNumAddMovePos).dirBasePos(1) = getDrawPointByMID(dirBPIndex(1)) 'gDrawPoints(2) 'dirBPIndex(1)から計測点を取得

                gAddMovePos(gNumAddMovePos).dirBasePos(2) = getDrawPointByMID(dirBPIndex(2)) 'gDrawPoints(2) 'dirBPIndex(2)から計測点を取得

                gAddMovePos(gNumAddMovePos).basePos = getDrawPointByMID(basePosID) 'gDrawPoints(3) 'basePosIDから計測点を取得

                gAddMovePos(gNumAddMovePos).newPos = getDrawPointByMID(newPosID) 'gDrawPoints(4)  'newPosIDから計測点を取得


                gNumAddMovePos = gNumAddMovePos + 1
                adoRet.MoveNext()
            Loop
        End If
        clsOPe.DisConnectDB()
        YCM_ReadInterPosInfoTable = 0
labError:
    End Function

    '機能：「計測データ.mdb」に座標補間点用のテーブルを書き出す

    Public Function YCM_WriteInterPosInfoTable(ByVal strDBPath As String) As Integer
        On Error GoTo labError
        Dim dbo As New CDBOperate
        Dim strSQL As String
        If dbo.ConnectDB(strDBPath) = False Then
            YCM_WriteInterPosInfoTable = -1
            Exit Function
        End If
        '補間点用テーブルが無ければ作成する
        YCM_AddNewInterPosInfoTable(dbo)

        '古いデータを削除
        strSQL = "delete from [interposinfo]"
        dbo.ExcuteSQL(strSQL)
        '座標補間点データの保存

        For ii As Long = 0 To gNumAddMovePos - 1
            'If (gAddMovePos(ii).iType <> 3) Then
            '    strSQL = "INSERT INTO interposinfo " & _
            '             " (ID,flgDel,interposType,dMoveValue," & _
            '             " basePosID,newPosID,dirBPID1,dirBPID2,strResult)" & _
            '             " VALUES ("
            'Else
            strSQL = "INSERT INTO interposinfo " & _
                     " (ID,flgDel,interposType,dMoveValue," & _
                     " basePosID,newPosID,dirBPID1,dirBPID2,dirBPID3,strResult)" & _
                     " VALUES ("
            'End If
            strSQL = strSQL + "'" & CStr(ii + 1) & "'"
            strSQL = strSQL + ",'" & CStr(IIf(gAddMovePos(ii).isDelete = True, 1, 0)) & "'"
            strSQL = strSQL + ",'" & CStr(gAddMovePos(ii).iType) & "'"
            strSQL = strSQL + ",'" & CStr(gAddMovePos(ii).dMoveValue) & "'"
            strSQL = strSQL + ",'" & CStr(gAddMovePos(ii).basePos.mid) & "'"
            strSQL = strSQL + ",'" & CStr(gAddMovePos(ii).newPos.mid) & "'"

            strSQL = strSQL + ",'" & CStr(gAddMovePos(ii).dirBasePos(0).mid) & "'"
            strSQL = strSQL + ",'" & CStr(gAddMovePos(ii).dirBasePos(1).mid) & "'"
            If (gAddMovePos(ii).iType = 3) Then
                strSQL = strSQL + ",'" & CStr(gAddMovePos(ii).dirBasePos(2).mid) & "'"
            Else
                strSQL = strSQL + ",'0'"
            End If
            strSQL = strSQL + ",'" & (gAddMovePos(ii).strResult) & "');"
            dbo.ExcuteSQL(strSQL)
        Next
        dbo.DisConnectDB()
        YCM_WriteInterPosInfoTable = 0
labError:
    End Function

    '機能：「計測データ.mdb」に座標補間点用のテーブルが無い場合に作成する
    Public Function YCM_AddNewInterPosInfoTable( _
        ByVal clsOPe As CDBOperate _
    ) As Integer
        On Error GoTo labError
        YCM_AddNewInterPosInfoTable = -1
        If (clsOPe Is Nothing) Then Exit Function

        '「interposinfo」テーブルが無い場合の処理を追加
        Dim strSQL As String
        'Dim adoRecSet As ADODB.Recordset
        'strSQL = "SELECT * FROM interposinfo"
        'adoRecSet = clsOPe.CreateRecordset(strSQL)
        'If adoRecSet Is Nothing Then
        If (Not ExistsTable(clsOPe, "[interposinfo]")) Then
            '座標補間点管理テーブル
            strSQL = "CREATE TABLE " & "interposinfo" & " ("
            strSQL = strSQL & "ID            LONG,"         'ID
            strSQL = strSQL & "flgDel        INTEGER,"      '削除フラグ
            strSQL = strSQL & "interposType  INTEGER,"      '補間点タイプ


            strSQL = strSQL & "dirBPID1   LONG,"      '方向基準点1ID
            strSQL = strSQL & "dirBPID2   LONG,"      '方向基準点2ID
            strSQL = strSQL & "dirBPID3   LONG,"      '方向基準点3ID

            strSQL = strSQL & "dMoveValue   DOUBLE PRECISION," '移動量
            strSQL = strSQL & "basePosID    LONG,"      '基準点ID
            strSQL = strSQL & "newPosID     LONG,"      '作成された点ID

            'strSQL = strSQL & "dirBPLabel    VARCHAR(50),"
            'strSQL = strSQL & "dirBPX1       DOUBLE PRECISION,"
            'strSQL = strSQL & "dirBPY1       DOUBLE PRECISION,"
            'strSQL = strSQL & "dirBPZ1       DOUBLE PRECISION,"

            'strSQL = strSQL & "dirBP1X2       DOUBLE PRECISION,"
            'strSQL = strSQL & "dirBP1Y2       DOUBLE PRECISION,"
            'strSQL = strSQL & "dirBP1Z2       DOUBLE PRECISION,"

            strSQL = strSQL & "strResult      VARCHAR(250)"
            strSQL = strSQL & ")"
            Call clsOPe.ExcuteSQL(strSQL)
        End If
        YCM_AddNewInterPosInfoTable = 0
labError:
    End Function

    '==============================================================================
    '機　能：テーブルが存在するか調べる '+++++++++++>>この関数は駄目
    '戻り値：存在する=True,存在しない=False
    '引　数：

    '        clsDBOpe       [I/ ]   データベースのオペレータ
    '        strTable       [I/ ]   テーブル名

    '==============================================================================
    Public Function ExistsTable( _
        ByRef clsDBOpe As CDBOperate, _
        ByVal strTable As String _
    ) As Boolean
        On Error GoTo labError
        ExistsTable = False
        Dim adoRS As New ADODB.Recordset
#If 1 Then '---------------------
        Dim strSQL As String
        strSQL = "SELECT * From " & strTable
        adoRS = clsDBOpe.CreateRecordset(strSQL)
        If adoRS.RecordCount > 0 Then
            ExistsTable = True
        End If
#Else
        Dim myArr() As Object = {vbEmpty, vbEmpty, strTable, vbEmpty}
        Dim adoCN As New ADODB.Connection
        adoCN = clsDBOpe.GetConnection
        '--        adoRS = adoCN.OpenSchema(SchemaEnum.adSchemaTables, Array(vbEmpty, vbEmpty, strTable, vbEmpty))
        '-        adoRS = adoCN.OpenSchema(SchemaEnum.adSchemaTables, myArr)
        adoRS = adoCN.OpenSchema(SchemaEnum.adSchemaTables)
        ''+++++++++++++++++++++++++++
        ''Do Until adoRS.EOF
        ''    'Debug.Print("Table name: " & _
        ''    '   adoRS!TABLE_NAME & vbCr & _
        ''    '   "Table type: " & adoRS!TABLE_TYPE & vbCr)
        ''    adoRS.MoveNext()
        ''Loop
        '' clean up
        '' adoRS.Close()
        ''+++++++++++++++++++++++++++
        ExistsTable = Not adoRS.EOF
#End If
labError:
    End Function
#End If
#End Region

    ' MIDを指定してgDrawPoints()から計測点を取得する

    Public Function getDrawPointByMID(ByVal mid As Long) As CLookPoint
        getDrawPointByMID = Nothing
        For ii As Long = 0 To (nLookPoints - 1)
            If (gDrawPoints(ii).mid = mid) Then
                getDrawPointByMID = gDrawPoints(ii)
                Exit For
            End If
        Next
    End Function

    Public Function getDrawPointByTID(ByVal tid As Long) As CLookPoint
        getDrawPointByTID = Nothing
        For ii As Long = 0 To (nLookPoints - 1)
            If (gDrawPoints(ii).tid = tid) Then
                getDrawPointByTID = gDrawPoints(ii)
                Exit For
            End If
        Next
    End Function

    Public Sub setRibbonMenuChkOnOff()
        ' リボンメニューに表示／非表示の状態を反映
        'MainFrm.RibbonMenuControl14.setRbnChkBoxOnOff(1, entset_point.blnVisiable)


        MainFrm.RibbonMenuControl14.setRbnChkBoxOnOff(1, entset_point.blnVisiable)
        MainFrm.RibbonMenuControl14.setRbnChkBoxOnOff(2, entset_pointUser.blnVisiable)
        MainFrm.RibbonMenuControl14.setRbnChkBoxOnOff(3, entset_camera.blnVisiable)
        MainFrm.RibbonMenuControl14.setRbnChkBoxOnOff(4, entset_ray.blnVisiable)
        MainFrm.RibbonMenuControl14.setRbnChkBoxOnOff(5, entset_label.blnVisiable)
        'MainFrm.RibbonMenuControl14.setRbnChkBoxOnOff(6, (entset_line.blnVisiable And entset_circle.blnVisiable))
        MainFrm.RibbonMenuControl14.setRbnChkBoxOnOff(6, entset_line.blnVisiable)
        MainFrm.RibbonMenuControl14.setRbnChkBoxOnOff(7, entset_circle.blnVisiable)
        MainFrm.RibbonMenuControl14.setRbnChkBoxOnOff(8, entset_line_CAD.blnVisiable)
        MainFrm.RibbonMenuControl14.setRbnChkBoxOnOff(9, entset_circle_CAD.blnVisiable)
        MainFrm.RibbonMenuControl14.setRbnChkBoxOnOff(10, True)
        MainFrm.RibbonMenuControl14.setRbnChkBoxOnOff(11, CordTargetIsvisible)

    End Sub

    Public Function LicenseCheck() As Boolean
        'Rep By Yamada 20150319 Sta----------
        '#If False Then
#If USER = "TRUE" Then
        LicenseCheck = False

        Dim Licdata As New CAS_LICENSE_DATA
        Dim dum As String = "RESET"
        Dim ret As Long
        ret = casInitLicense(0, dum)
        If Not ret = 0 Then
            Exit Function
        End If

        ret = casAuthorizeLicense(0)
        If Not ret = 0 Then

            Exit Function
        End If
        ret = casCatchLicense(3, 200, Licdata)
        If Not ret = 0 Then

            Exit Function
        End If
        ret = casReleaseLicense(3, 200, Licdata)
        If Not ret = 0 Then
            Exit Function
        End If
#End If
        LicenseCheck = True
    End Function
    'H25.6.28Yamada修正

    'Public Sub RunKaisekiProg()

    '(入力)blnFrmOut
    'False：画像タブ「次へ」/True:「検査表出力」

    Public Sub RunKaisekiProg()

        'MsgBox(System.Reflection.MethodBase.GetCurrentMethod.Name)

        TimeMonStart()
        '' C:\Documents and Settings\5002110\My Documents\My Pictures\SPAR\SysTEST
        'MainFrm = New YCM_MainFrame
        ''   m_koji_kanri_path = "C:\Documents and Settings\5002110\My Documents\My Pictures\oosaka koujyo\OosakaTest\コピー ～ Image1"
        'MainFrm.Tag = "1"
        'MainFrm.Show()

        MainFrm.FileNew()

        MainFrm.BtnAnaBatch()
        'MainFrm.FileSave()
        MainFrm.ChgView3DView()
        'MainFrm.ChgViewImage1_3DView()

        '  CalcSunpo()
        MainFrm.objFBM.SaveToMeasureDataDB(MainFrm.objFBM.ProjectPath & "\")
        YCM_SaveUserFigureToDB()
        YMC_3DViewReDraw(m_strDataBasePath)
        YCM_Offset_GenData2(MainFrm.objFBM) ' SUURI 20130523 ADD
        MainFrm.objFBM.SaveToMeasureDataDB(MainFrm.objFBM.ProjectPath & "\")
        YCM_SaveUserFigureToDB()
        YMC_3DViewReDraw(m_strDataBasePath)
        MainFrm.FileSave()
        CalcSunpo()
        '(m_strDataBasePath)
        '   YCM_Offset_GenData2() ' SUURI 20130523 ADD
        ' MainFrm.Visible = False
        OutErrorMessageTxt()
        TimeMonEnd()

        'MsgBox(System.Reflection.MethodBase.GetCurrentMethod.Name)

    End Sub

    Public Sub RunReadKaisekiProg()

        'If Me.MainFrm <> Nothing Then

        'End If
        'If MainFrm =  Then

        'End If
        'MainFrm.Close()
        ' C:\Documents and Settings\5002110\My Documents\My Pictures\SPAR\SysTEST
        ' MainFrm = New YCM_MainFrame

        MainFrm.Tag = "1"

        MainFrm.Show()

        '   MainFrm.FileNew()
        MainFrm.FileOpen()
        MainFrm.ChgView3DView()
        MainFrm.ChgViewImage1_3DView()
        YmdSleep()
        ' MainFrm.BtnAnaBatch()
        'YCM_Offset_GenData2(MainFrm.objFBM) ' SUURI 20130523 ADD
        YCM_Offset_ReadData(MainFrm.objFBM)
        'CalcSunpo()
        ' MainFrm.Visible = False
        MainFrm.objFBM.SaveToMeasureDataDB(MainFrm.objFBM.ProjectPath & "\")

        YCM_SaveUserFigureToDB()
        YMC_3DViewReDraw(m_strDataBasePath)

        CalcSunpo()
        OutErrorMessageTxt()
    End Sub
    Private Sub OutErrorMessageTxt()
        Dim strErrorMessTxtFilePath As String = MainFrm.objFBM.ProjectPath & "\ErrorMessage.txt"
        If IO.File.Exists(strErrorMessTxtFilePath) = True Then
            'Dim fileContents As String
            'fileContents = My.Computer.FileSystem.ReadAllText(strErrorMessTxtFilePath)

            Dim filename As String = strErrorMessTxtFilePath
            Dim strLines As String
            Using parser As New TextFieldParser(filename)
                While Not parser.EndOfData
                    ' Read in the fields for the current line
                    strLines = parser.ReadLine
                    ' Add code here to use data in fields variable.
                    IOUtil.WritePrompt(strLines)
                End While
            End Using

            '  MainFrm.setTextMessage(fileContents)
        End If
    End Sub
    Public Sub ReadKaisekiData()
        ' C:\Documents and Settings\5002110\My Documents\My Pictures\SPAR\SysTEST
        MainFrm = New YCM_MainFrame
        ' Dim fileContents As String
        ' fileContents = My.Computer.FileSystem.ReadAllText(My.Application.Info.DirectoryPath & "\" & "MMM.txt")
        '  m_koji_kanri_path = fileContents
        'MainFrm.Tag = "1"

        'MainFrm.Show() '
        'MainFrm.Hide()

        ''   MainFrm.FileNew()
        'MainFrm.FileOpen()
        MainFrm.OpenMeasureNaibuData(m_koji_kanri_path)

        ' MainFrm.OpenMeasureData(m_koji_kanri_path)
        ' MainFrm.BtnAnaBatch()
        'YCM_Offset_GenData2(MainFrm.objFBM) ' SUURI 20130523 ADD
        YCM_Offset_ReadData(MainFrm.objFBM)
        'CalcSunpo()
        ' MainFrm.Visible = False
    End Sub

    '(20150105 Tezuka ADD) SunpoSetテーブルに[flgScale]フィールドが無い場合に追加する
    Public Function AddDBflgScale(ByVal mdb As FBMlib.CDBOperateOLE) As Integer
        Dim iRtn As Long
        AddDBflgScale = -1
        If (mdb Is Nothing) Then Exit Function

        '「SunpoSet」テーブルに[flgScale]フィールドが無い場合の追加
        Dim strSQL As String
        strSQL = "SELECT flgScale FROM SunpoSet"
        Dim IDR As IDataReader = mdb.DoSelect(strSQL)
        If IDR Is Nothing Then
            mdb.BeginTrans()
            strSQL = "ALTER TABLE [SunpoSet] ADD flgScale INT;"
            iRtn = mdb.ExecuteSQL(strSQL)
            If iRtn >= 0 Then
                strSQL = "UPDATE [SunpoSet] SET flgScale=-1 WHERE flgScale Is Null;"
                iRtn = mdb.ExecuteSQL(strSQL)
                If iRtn >= 0 Then
                    mdb.CommitTrans()
                Else
                    mdb.RollbackTrans()
                End If
            Else
                mdb.RollbackTrans()
            End If
            AddDBflgScale = 0
        End If
        'AddDBflgScale = 0
    End Function

    Public Function ChgDBflgScale(ByVal mdb As FBMlib.CDBOperateOLE) As Integer
        ChgDBflgScale = -1
        Dim iRtn As Integer
        If (mdb Is Nothing) Then Exit Function

        Dim strSQL As String
        strSQL = "UPDATE [SunpoSet] SET flgScale=0 WHERE SunpoMark IN ('S12','S34','S56','S-A','S-B');"
        iRtn = mdb.ExecuteSQL(strSQL)
        If iRtn >= 0 Then
            mdb.CommitTrans()
        Else
            mdb.RollbackTrans()
        End If

        ChgDBflgScale = 0
    End Function


    Public Sub SueokiSyori(ByVal cmds() As String)

        'Dim iSts As Integer = 0

        ''選択行の取得

        'iSts = SelectGet()
        'If iSts < 0 Then
        '    Exit Sub
        'End If

        '' ①初期画面から「新規」あるいは「開く」を押すと初期画面が閉じる（非表示にする）

        ''Me.Visibility = System.Windows.Visibility.Hidden

        'frmkihon = New KentouKihonInfoTabDialog
        'frmkihon.WindowState = Windows.WindowState.Maximized
        'frmkihon.IsNew = True
        'frmkihon.ShowDialog()

        '' ①初期画面から「新規」あるいは「開く」を押すと初期画面が閉じる（非表示にする）

        ''Me.Close()


    End Sub

    '20170322 baluu add start
    Private Function CalculateRegion(ByVal hv_CameraParam As HTuple, ByVal rectangle As HObject) As HObject
        If rectangle Is Nothing Then
            CalculateRegion = rectangle
            Exit Function
        End If
        Dim widthScale As Double, heightScale As Double
        Dim rRows = New HTuple, rCols = New HTuple
        Dim scaledRegion = New HObject
        Try
            widthScale = hv_CameraParam(10).D / MainFrm.objFBM.hv_CamparamOut(10).D
            heightScale = hv_CameraParam(11).D / MainFrm.objFBM.hv_CamparamOut(11).D

            HOperatorSet.ZoomRegion(rectangle, scaledRegion, widthScale, heightScale)
            CalculateRegion = scaledRegion
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    '20170322 baluu add end

    '20170302 baluu add start
    Private Const hv_ConnectDistance As Double = 2
    Private Const hv_CMinBoundingBox As Double = 0.05
    Private Const hv_CMaxBoundingBox As Double = 10000
    Private Const flgCalcByRelPose As Boolean = True
    Public Function ReconstructScene(ByRef SettingsData As SettingsTable, ByVal images As List(Of ImageSet), Optional ByVal showMsg As Boolean = True) As Boolean
        ReconstructScene = False

        Dim hv_T0 As HTuple = Nothing, hv_T1 As HTuple = Nothing, numPoints As HTuple = Nothing
        Dim hv_CameraSetupModelID As HTuple = Nothing, hv_CameraParam As HTuple = Nothing, hv_StereoModelID As HTuple = Nothing
        Dim ho_Images As HObject = Nothing
        Dim hv_ObjectModel3D As HTuple = Nothing, HM3D As HTuple = Nothing, HM3DI As HTuple = Nothing
        Dim hv_PoseIn As HTuple = Nothing, hv_PoseOut As HTuple = Nothing

        HOperatorSet.GenEmptyObj(ho_Images)
        Try
            HOperatorSet.ReadCamPar(SettingsData.camera_param, hv_CameraParam)
        Catch ex As Exception
            HOperatorSet.ReadTuple(SettingsData.camera_param, hv_CameraParam)
        End Try

        If BTuple.TupleLength(hv_CameraParam).I <= 0 Then
            If showMsg Then
                MsgBox("Invalid Camera Parameters")
            End If
            Throw New Exception("Invalid Camera Parameters")
            Exit Function
        End If

        Try
            HOperatorSet.CreateCameraSetupModel(images.Count, hv_CameraSetupModelID)
            For index As Integer = 0 To images.Count - 1
                HOperatorSet.SetCameraSetupCamParam(hv_CameraSetupModelID, index, "area_scan_polynomial", hv_CameraParam, images(index).ImagePose.Pose)
            Next

            HOperatorSet.CreateStereoModel(hv_CameraSetupModelID, "surface_pairwise", New HTuple, New HTuple, hv_StereoModelID)
#If DEBUG Then
            HOperatorSet.WriteCameraSetupModel(hv_CameraSetupModelID, MainFrm.objFBM.ProjectPath & "\" & SettingsData.SelectedImageIDs & ".csm")

#End If
            HOperatorSet.ClearCameraSetupModel(hv_CameraSetupModelID)
            '======================================SetStereoModel=========================================
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "persistence", New HTuple(IIf(SettingsData.persistence, 1, 0)))
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "rectif_interpolation", SettingsData.rectif_interpolation)
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "rectif_sub_sampling", New HTuple(1))  'Convert.ToDouble(SettingsData.rectif_sub_sampling)) 1に固定
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "sub_sampling_step", CInt(SettingsData.sub_sampling_step))
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "disparity_method", SettingsData.disparity_method)
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_method", SettingsData.binocular_method)
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_num_levels", CInt(SettingsData.binocular_num_levels))
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_mask_width", CInt(SettingsData.binocular_mask_width))
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_mask_height", CInt(SettingsData.binocular_mask_height))
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_texture_thresh", Convert.ToDouble(SettingsData.binocular_texture_thresh))
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_score_thresh", Convert.ToDouble(SettingsData.binocular_score_thresh))
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_filter", SettingsData.binocular_filter)
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_sub_disparity", SettingsData.binocular_sub_disparity)
            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "point_meshing", SettingsData.point_meshing)
            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "poisson_depth", Convert.ToInt32(SettingsData.poisson_depth))
            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "poisson_solver_divide", Convert.ToInt32(SettingsData.poisson_solver_divide))
            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "poisson_samples_per_node", Convert.ToInt32(SettingsData.poisson_samples_per_node))
            '======================================SetStereoModel=========================================
            HOperatorSet.CountSeconds(hv_T0)
            '======================================ReadMultiViewStereoImages==============================
            Dim ho_ZImages As HObject = Nothing, ho_SImages As HObject = Nothing
            HOperatorSet.GenEmptyObj(ho_Images)
            HOperatorSet.GenEmptyObj(ho_ZImages)
            Dim files As New HTuple()
            For i As Integer = 0 To images.Count - 1
                files(i) = images(i).ImageFullPath
            Next
            HOperatorSet.ReadImage(ho_Images, files)
            Dim ho_ImageChannel1 As HObject = Nothing, ho_ImageChannel2 As HObject = Nothing, ho_ImageChannel3 As HObject = Nothing
            HOperatorSet.Decompose3(ho_Images, ho_ImageChannel1, ho_ImageChannel2, ho_ImageChannel3)
            ho_ImageChannel1.Dispose()
            ho_ImageChannel3.Dispose()
            HOperatorSet.ZoomImageSize(ho_ImageChannel2, ho_ZImages, hv_CameraParam(10), hv_CameraParam(11), New HTuple("constant"))
            ho_ImageChannel2.Dispose()
            'pair双方の輝度値を合わせる。
            HOperatorSet.LocalMax(ho_ZImages, ho_ImageChannel2)
            Dim scaleVal As New HTuple
            ' HOperatorSet.GetGrayval(ho_ImageChannel2, New HTuple(0), New HTuple(0), scaleVal)
            HOperatorSet.ScaleImageMax(ho_ZImages, ho_SImages)
            ho_ImageChannel2.Dispose()
            ho_ZImages.Dispose()
            ho_Images = ho_SImages

            '======================================ReadMultiViewStereoImages===============================
            HOperatorSet.CountSeconds(hv_T1)
            SettingsData.ImageHenkanTime = BTuple.TupleSub(hv_T1, hv_T0).ToString()

            '======================================CalculateBoundingBox====================================
            Dim ImageRegion As HObject = Nothing, DirectBoundingBox As HTuple = Nothing
            Dim ReducedBox As HTuple = Nothing, TempBox As HTuple = Nothing, ConvexBox As HTuple = Nothing, SampleBox As HTuple = Nothing, TriangulatedBox As HTuple = Nothing
            Dim margin As Integer = 10
            ' HOperatorSet.GenRectangle1(ImageRegion, 0, 0, BTuple.TupleSelect(hv_CameraParam, 11), BTuple.TupleSelect(hv_CameraParam, 10))
            HOperatorSet.GenRectangle1(ImageRegion, margin, margin, BTuple.TupleSelect(hv_CameraParam, 11) - margin, BTuple.TupleSelect(hv_CameraParam, 10) - margin)
            '======================================GenHomMatPyramid====================================
            Dim PX As HTuple = Nothing, PY As HTuple = Nothing, PZ As HTuple = Nothing
            Dim QX As HTuple = Nothing, QY As HTuple = Nothing, QZ As HTuple = Nothing
            Dim XX As HTuple = Nothing, XY As HTuple = Nothing, XZ As HTuple = Nothing

            Dim LineOfSightRow As HTuple = Nothing

            LineOfSightRow = BTuple.TupleConcat(LineOfSightRow, margin)
            LineOfSightRow = BTuple.TupleConcat(LineOfSightRow, margin)
            LineOfSightRow = BTuple.TupleConcat(LineOfSightRow, BTuple.TupleSelect(hv_CameraParam, 11) - margin)
            LineOfSightRow = BTuple.TupleConcat(LineOfSightRow, BTuple.TupleSelect(hv_CameraParam, 11) - margin)
            Dim LineOfSightCol As HTuple = Nothing
            LineOfSightCol = BTuple.TupleConcat(LineOfSightCol, margin)
            LineOfSightCol = BTuple.TupleConcat(LineOfSightCol, BTuple.TupleSelect(hv_CameraParam, 10) - margin)
            LineOfSightCol = BTuple.TupleConcat(LineOfSightCol, BTuple.TupleSelect(hv_CameraParam, 10) - margin)
            LineOfSightCol = BTuple.TupleConcat(LineOfSightCol, margin)

            Dim HomMat3D As HTuple = Nothing, HomMat3DIdentity As HTuple = Nothing, HomMat3DScale As HTuple = Nothing, Diameter As HTuple = Nothing
            Dim LineOfSightPoints As HTuple = Nothing, LineOfSightObject As HTuple = Nothing
            Dim LineOfSightObjectSample As HTuple = Nothing, LineOfSightObjectTriangulated As HTuple = Nothing, LineOfSightObjectScaled As HTuple = Nothing

            If Not images(0).Region Is Nothing Then 'TODO
                Dim r_Rows As HTuple = Nothing, r_Cols As HTuple = Nothing
                HOperatorSet.GetRegionConvex(CalculateRegion(hv_CameraParam, images(0).Region), r_Rows, r_Cols)
                LineOfSightRow = r_Rows
                LineOfSightCol = r_Cols
            End If

            HOperatorSet.GetLineOfSight(LineOfSightRow, LineOfSightCol, hv_CameraParam, PX, PY, PZ, QX, QY, QZ)
            HOperatorSet.TupleConcat(QX, 0.0, QX)
            HOperatorSet.TupleConcat(QY, 0.0, QY)
            HOperatorSet.TupleConcat(QZ, 0.0, QZ)
            HOperatorSet.PoseToHomMat3d(images(0).ImagePose.Pose, HomMat3D)
            HOperatorSet.AffineTransPoint3D(HomMat3D, QX, QY, QZ, XX, XY, XZ)
            HOperatorSet.GenObjectModel3DFromPoints(XX, XY, XZ, LineOfSightPoints)
            HOperatorSet.ConvexHullObjectModel3D(LineOfSightPoints, LineOfSightObject)
            HOperatorSet.ClearObjectModel3D(LineOfSightPoints)
            HOperatorSet.GetObjectModel3DParams(LineOfSightObject, "diameter_axis_aligned_bounding_box", Diameter)
            HOperatorSet.SampleObjectModel3d(LineOfSightObject, "fast", BTuple.TupleMult(ReconstructionWindow.SamplingFactor, Diameter), New HTuple, New HTuple, LineOfSightObjectSample)
            HOperatorSet.ClearObjectModel3D(LineOfSightObject)
            HOperatorSet.TriangulateObjectModel3d(LineOfSightObjectSample, "greedy", New HTuple, New HTuple, LineOfSightObjectTriangulated, New HTuple)
            HOperatorSet.ClearObjectModel3D(LineOfSightObjectSample)
            HOperatorSet.HomMat3dIdentity(HomMat3DIdentity)
            HOperatorSet.HomMat3dScale(HomMat3DIdentity, Convert.ToDouble(SettingsData.bounding_box), Convert.ToDouble(SettingsData.bounding_box), Convert.ToDouble(SettingsData.bounding_box), _
                             BTuple.TupleSelect(images(0).ImagePose.Pose, 0), _
                             BTuple.TupleSelect(images(0).ImagePose.Pose, 1), _
                             BTuple.TupleSelect(images(0).ImagePose.Pose, 2), HomMat3DScale)
            HOperatorSet.AffineTransObjectModel3D(LineOfSightObjectTriangulated, HomMat3DScale, LineOfSightObjectScaled)
            HOperatorSet.ClearObjectModel3D(LineOfSightObjectTriangulated)
            '======================================GenHomMatPyramid====================================

            HOperatorSet.CopyObjectModel3D(LineOfSightObjectScaled, "all", ConvexBox)
            HOperatorSet.ClearObjectModel3D(LineOfSightObjectScaled)
            Dim hv_ZeroCameraParam As HTuple = Nothing
            HOperatorSet.TupleReplace(hv_CameraParam, New HTuple(1, 2, 3, 4, 5), 0, hv_ZeroCameraParam)
            For i As Integer = 1 To images.Count - 1
                HOperatorSet.CopyObjectModel3d(ConvexBox, "all", TempBox)
                HOperatorSet.ClearObjectModel3d(ConvexBox)
                HOperatorSet.PoseToHomMat3d(images(i).ImagePose.Pose, HM3D)
                HOperatorSet.HomMat3dInvert(HM3D, HM3DI)
                HOperatorSet.HomMat3dToPose(HM3DI, hv_PoseIn)
                If images(i).Region Is Nothing Then
                    HOperatorSet.ReduceObjectModel3dByView(ImageRegion, TempBox, hv_ZeroCameraParam, hv_PoseIn, ReducedBox)

                Else
                    HOperatorSet.ReduceObjectModel3dByView(CalculateRegion(hv_CameraParam, images(i).Region), TempBox, hv_ZeroCameraParam, hv_PoseIn, ReducedBox)
                End If
                HOperatorSet.ClearObjectModel3d(TempBox)
                HOperatorSet.GetObjectModel3dParams(ReducedBox, "num_points", numPoints)
                If numPoints.I <= 0 Then
                    If showMsg Then
                        MsgBox("バウンディングボックス算出でエラー発生しました。「" & images(i).ImageId & ":" & images(i).ImageName & "」画像のレジョンの設定に誤りがあります。")
                    End If

                    Exit Function
                End If
                'HOperatorSet.GetObjectModel3dParams(ReducedBox, "bounding_box1", DirectBoundingBox)
                HOperatorSet.ConvexHullObjectModel3d(ReducedBox, ConvexBox)
                HOperatorSet.ClearObjectModel3d(ReducedBox)
                HOperatorSet.GetObjectModel3dParams(ConvexBox, "diameter_axis_aligned_bounding_box", Diameter)
                HOperatorSet.SampleObjectModel3d(ConvexBox, "fast", BTuple.TupleMult(ReconstructionWindow.SamplingFactor, Diameter), New HTuple, New HTuple, SampleBox)
                HOperatorSet.ClearObjectModel3d(ConvexBox)
                HOperatorSet.TriangulateObjectModel3d(SampleBox, "greedy", New HTuple, New HTuple, TriangulatedBox, New HTuple)
                HOperatorSet.ClearObjectModel3d(SampleBox)
                HOperatorSet.CopyObjectModel3d(TriangulatedBox, "all", ConvexBox)
                HOperatorSet.ClearObjectModel3d(TriangulatedBox)
                HOperatorSet.GetObjectModel3dParams(ConvexBox, "bounding_box1", DirectBoundingBox)

            Next

            HOperatorSet.ClearObj(ImageRegion)
            ' HOperatorSet.GetObjectModel3dParams(ConvexBox, "bounding_box1", DirectBoundingBox)
            'Dim ttttbb As HTuple = New HTuple(0.0, 0.0, 0.0, 0.0, 0.0, -1.0)
            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "bounding_box", ttttbb)
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "bounding_box", DirectBoundingBox)
            HOperatorSet.ClearObjectModel3d(ConvexBox)
#If DEBUG Then
            HOperatorSet.WriteTuple(DirectBoundingBox, MainFrm.objFBM.ProjectPath & "\" & SettingsData.SelectedImageIDs & "boundingbox.tpl")

#End If
            '======================================CalculateBoundingBox====================================

            '======================================SetImagePair============================================
            Dim imagesCount As Int32 = 0
            imagesCount = ho_Images.CountObj()
            For i As Integer = 0 To imagesCount - 2 Step 1
                HOperatorSet.SetStereoModelImagePairs(hv_StereoModelID, i, i + 1)
            Next
            '======================================SetImagePair============================================

            HOperatorSet.CountSeconds(hv_T0)
            HOperatorSet.ReconstructSurfaceStereo(ho_Images, hv_StereoModelID, hv_ObjectModel3D)
            HOperatorSet.CountSeconds(hv_T1)
            SettingsData.ReconstructTime = BTuple.TupleSub(hv_T1, hv_T0).ToString()
            HOperatorSet.ClearStereoModel(hv_StereoModelID)
            ho_Images.Dispose()
            HOperatorSet.CountSeconds(hv_T0)
            HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3D, "num_points", numPoints)
            '======================================Reduce3D============================================
            Dim hv_ReducedObject As HTuple
            Dim hv_Temp3d As HTuple = hv_ObjectModel3D
            Dim hv_Temp3d2 As HTuple = Nothing
            For Each iset As ImageSet In images
                If Not iset.Region Is Nothing Then
                    HOperatorSet.CopyObjectModel3d(hv_Temp3d, "all", hv_Temp3d2)
                    HOperatorSet.ClearObjectModel3d(hv_Temp3d)
                    HOperatorSet.PoseToHomMat3d(iset.ImagePose.Pose, HM3D)
                    HOperatorSet.HomMat3dInvert(HM3D, HM3DI)
                    HOperatorSet.HomMat3dToPose(HM3DI, hv_PoseIn)
                    HOperatorSet.ReduceObjectModel3dByView(CalculateRegion(hv_CameraParam, iset.Region), hv_Temp3d2, hv_CameraParam, hv_PoseIn, hv_Temp3d)
                    HOperatorSet.ClearObjectModel3d(hv_Temp3d2)
                End If
            Next
            hv_ReducedObject = hv_Temp3d
            '======================================Reduce3D============================================

            'Dim hv_SmoothModel3D As HTuple = hv_ReducedObject
            'Dim hv_SmoothModel3D As HTuple = Nothing
            'HOperatorSet.SmoothObjectModel3d(hv_ReducedObject, "mls", New HTuple("mls_force_inwards", "mls_kNN"), New HTuple("true", 20), hv_SmoothModel3D)
            'HOperatorSet.ClearObjectModel3d(hv_ReducedObject)
            HOperatorSet.GetObjectModel3dParams(hv_ReducedObject, "num_points", numPoints)
            If BTuple.TupleInt(numPoints) > 0 Then
                '======================================Cleanup============================================
                Dim hv_SmoothObject3D As HTuple = Nothing
                Dim hv_Connected As HTuple = Nothing, hv_Selected As HTuple = Nothing
                'HOperatorSet.SmoothObjectModel3d(hv_HalconObjectModel3d, "mls", New HTuple("mls_force_inwards", "mls_kNN"), _
                '                                 New HTuple("true", 400), hv_SmoothObject3D)
                HOperatorSet.ConnectionObjectModel3d(hv_ReducedObject, "distance_3d", hv_ConnectDistance, hv_Connected)
                HOperatorSet.ClearObjectModel3d(hv_ReducedObject)
                HOperatorSet.SelectObjectModel3d(hv_Connected, "diameter_bounding_box", "and", Double.Parse(SettingsData.cleanup_min), hv_CMaxBoundingBox, hv_Selected)
                HOperatorSet.ClearObjectModel3d(hv_Connected)
                HOperatorSet.UnionObjectModel3d(hv_Selected, "points_surface", hv_SmoothObject3D)
                HOperatorSet.ClearObjectModel3d(hv_Selected)
                hv_ReducedObject = hv_SmoothObject3D
                '======================================Cleanups============================================

                '======================================Save3DModel==========================================

                Dim currentDate = System.DateTime.Now.ToString("yyyyMMddHHmmss")
                If Not System.IO.Directory.Exists(MainFrm.objFBM.ProjectPath & "\Pdata\" & currentDate) Then
                    System.IO.Directory.CreateDirectory(MainFrm.objFBM.ProjectPath & "\Pdata\" & currentDate)
                End If
                Dim savePath = MainFrm.objFBM.ProjectPath & "\Pdata\" & currentDate & "\"
                Dim fileName = "ReconstructionResult.obj"
                SettingsData.OutputPath = savePath & fileName
                ' WriteObjFileWithColor(images(0).ImageFullPath, images(0).Region, images(0).ImagePose.Pose, images(0).objCamparam.Camparam, hv_ReducedObject, New HTuple(savePath & fileName))
                WriteObjFileWithColor(images, hv_ReducedObject, New HTuple(savePath & fileName))
                'HOperatorSet.WriteObjectModel3d(hv_ReducedObject, "obj", New HTuple(savePath & fileName), New HTuple, New HTuple)
                If Not System.IO.File.Exists(savePath & fileName) Then
                    If showMsg Then
                        MsgBox("Failed to write 3d object model")
                    End If
                End If
                HOperatorSet.ClearObjectModel3d(hv_ReducedObject)

                '======================================Save3DModel==========================================
                HOperatorSet.CountSeconds(hv_T1)
                SettingsData.OutputTime = BTuple.TupleSub(hv_T1, hv_T0).ToString()
                ReconstructScene = True
            Else
                Throw New Exception("Not Enough Points To Visualize")
                Exit Function
            End If

        Catch ex As Exception
            If showMsg Then
                MsgBox("Reconstruction Failed: " & ex.Message)
            End If
            Exit Function
        End Try
    End Function

    Public Sub ReconstMaeSyori1(ByVal ho_Images As HObject, ByRef ImageResult As HObject)
        Dim ho_ImageChannel1 As HObject = Nothing, ho_ImageChannel2 As HObject = Nothing, ho_ImageChannel3 As HObject = Nothing
        Dim ImageResult1 As HObject = Nothing, ImageResult2 As HObject = Nothing, ImageResult3 As HObject = Nothing

        HOperatorSet.Decompose3(ho_Images, ho_ImageChannel1, ho_ImageChannel2, ho_ImageChannel3)
        HOperatorSet.TransFromRgb(ho_ImageChannel1, ho_ImageChannel2, ho_ImageChannel3, ImageResult1, ImageResult2, ImageResult3, "hsv")
        HOperatorSet.Compose3(ImageResult1, ImageResult2, ImageResult3, ImageResult)
        ClearHObject(ho_ImageChannel1)
        ClearHObject(ho_ImageChannel2)
        ClearHObject(ho_ImageChannel3)
        ClearHObject(ImageResult1)
        ClearHObject(ImageResult2)
        ClearHObject(ImageResult3)
    End Sub
    Public Sub ReconstMaeSyori2(ByVal ho_Images As HObject, ByRef ImageResult As HObject)
        Dim ho_ImageTrans As HObject = Nothing
        Dim hv_Tmat As New HTuple
        Dim hv_Tmatinv As New HTuple
        Dim hv_Mean As New HTuple
        Dim hv_Cov As New HTuple
        Dim hv_InfoPerComp As New HTuple
        HOperatorSet.GenPrincipalCompTrans(ho_Images, hv_Tmat, hv_Tmatinv, hv_Mean, hv_Cov, hv_InfoPerComp)
        HOperatorSet.LinearTransColor(ho_Images, ho_ImageTrans, hv_Tmat)
        HOperatorSet.ConvertImageType(ho_ImageTrans, ImageResult, "byte")
        ClearHObject(ho_ImageTrans)
    End Sub
    Public Sub ReconstMaeSyori3(ByVal ho_Images As HObject, ByVal MW As HTuple, ByVal MH As HTuple, ByRef ImageResult As HObject)
        Dim hv_ImageRankN As HObject = Nothing
        HOperatorSet.RankN(ho_Images, hv_ImageRankN, 3)
        HOperatorSet.RankRect(hv_ImageRankN, ImageResult, MW, MH, New HTuple(Int(MW.I * MH.I / 2)))
        ClearHObject(hv_ImageRankN)
    End Sub

    Public Sub ReconstMaeSyori4(ByVal ho_Images As HObject, ByRef ImageResult As HObject)
        HOperatorSet.RankN(ho_Images, ImageResult, 3)
    End Sub

    Public Sub ReconstMaeSyori5(ByVal ho_Images As HObject, ByRef ImageResult As HObject)
        Dim hv_ImageRankN As HObject = Nothing
        HOperatorSet.RankN(ho_Images, hv_ImageRankN, 3)
        HOperatorSet.Illuminate(hv_ImageRankN, ImageResult, 100, 100, 0.7)
        ClearHObject(hv_ImageRankN)
    End Sub
    Public Sub ReconstMaeSyori6(ByVal ho_Images As HObject, ByRef ImageResult As HObject)
        HOperatorSet.Illuminate(ho_Images, ImageResult, 100, 100, 0.7)
    End Sub
    Public Sub ReconstMaeSyori7(ByVal ho_Images As HObject, ByRef ImageResult As HObject)
        Dim hv_ImageSmooth As HObject = Nothing
        HOperatorSet.SmoothImage(ho_Images, hv_ImageSmooth, "gauss", 1)
        HOperatorSet.Emphasize(hv_ImageSmooth, ImageResult, 7, 7, 1)
        ClearHObject(hv_ImageSmooth)
    End Sub
    Public Function ReconstructScene2(ByRef SettingsData As SettingsTable, ByVal images As List(Of ImageSet), Optional ByVal showMsg As Boolean = True) As Boolean
        ReconstructScene2 = False
        'GC.Collect()
        'GC.WaitForPendingFinalizers()
        '  Dim OP As New HOperatorSet
        Dim hv_T0 As HTuple = Nothing, hv_T1 As HTuple = Nothing, numPoints As HTuple = Nothing
        Dim hv_StereoModelID As HTuple = SettingsData.hv_StereoModelID

        Dim hv_CameraParam As HTuple = Nothing
        Dim ho_Images As HObject = Nothing
        Dim hv_ObjectModel3D As HTuple = Nothing, HM3D As HTuple = Nothing, HM3DI As HTuple = Nothing
        Dim hv_PoseIn As HTuple = Nothing, hv_PoseOut As HTuple = Nothing
        Dim hv_OnePose As HTuple = images(0).ImagePose.Pose
        Dim hv_OnePoseHomMat As HTuple = Nothing
        HOperatorSet.TupleMult(images(0).ImagePose.Pose, New HTuple({MainFrm.objFBM.pScaleMM, MainFrm.objFBM.pScaleMM, MainFrm.objFBM.pScaleMM, 1.0, 1.0, 1.0, 1.0}),
                                                                               hv_OnePose)
        HOperatorSet.PoseToHomMat3d(hv_OnePose, hv_OnePoseHomMat)

        Dim imagesCount As Int32 = 0
        imagesCount = images.Count
        Try
            For i As Integer = 0 To imagesCount - 2 Step 1
                HOperatorSet.SetStereoModelImagePairs(hv_StereoModelID, i, i + 1)
            Next
        Catch ex As Exception
            HOperatorSet.ClearStereoModel(hv_StereoModelID)
            SettingsData.hv_StereoModelID = Nothing
            Exit Function
        End Try

        HOperatorSet.GenEmptyObj(ho_Images)
        If False Then
            Try
                HOperatorSet.ReadCamPar(SettingsData.camera_param, hv_CameraParam)
            Catch ex As Exception
                HOperatorSet.ReadTuple(SettingsData.camera_param, hv_CameraParam)
            End Try
        Else
            hv_CameraParam = MainFrm.objFBM.hv_CamparamOut
        End If
        If BTuple.TupleLength(hv_CameraParam).I <= 0 Then
            If showMsg Then
                MsgBox("Invalid Camera Parameters")
            End If
            Throw New Exception("Invalid Camera Parameters")
            Exit Function
        End If

        Try

            HOperatorSet.CountSeconds(hv_T0)
            '======================================ReadMultiViewStereoImages==============================
            Dim ho_ReconstImage As HObject = Nothing
            HOperatorSet.GenEmptyObj(ho_Images)
            HOperatorSet.GenEmptyObj(ho_ReconstImage)
            Dim files As New HTuple()
            For i As Integer = 0 To images.Count - 1
                files(i) = images(i).ImageFullPath
            Next
            HOperatorSet.ReadImage(ho_Images, files)
            '前処理
            '  ReconstMaeSyori1(ho_Images, ho_ReconstImage)
            ' ReconstMaeSyori2(ho_Images, ho_ReconstImage)
            ' ReconstMaeSyori3(ho_Images, CInt(SettingsData.binocular_mask_width), CInt(SettingsData.binocular_mask_height), ho_ReconstImage)
            ' ReconstMaeSyori4(ho_Images, ho_ReconstImage)
            'ReconstMaeSyori5(ho_Images, ho_ReconstImage)
            ReconstMaeSyori6(ho_Images, ho_ReconstImage)
            'ReconstMaeSyori7(ho_Images, ho_ReconstImage)
            ClearHObject(ho_Images)


            '======================================ReadMultiViewStereoImages===============================
            HOperatorSet.CountSeconds(hv_T1)
            SettingsData.ImageHenkanTime = BTuple.TupleSub(hv_T1, hv_T0).ToString()

            HOperatorSet.CountSeconds(hv_T0)
            ' MsgBox("ReconstructSurfaceStereo start")
#If True Then
            HOperatorSet.ReconstructSurfaceStereo(ho_ReconstImage, hv_StereoModelID, hv_ObjectModel3D)
#Else
            'Dim p As System.Diagnostics.Process = System.Diagnostics.Process.Start("OnlyReconstructSurfaceStereo.exe", images(0).ImageFullPath & " " & images(1).ImageFullPath & " " & SettingsData.SelectedImageIDs & " " & hv_StereoModelID.L)
            'p.WaitForExit()
            HOperatorSet.ReadObjectModel3d(MainFrm.objFBM.ProjectPath & "\" & SettingsData.SelectedImageIDs & "_object3d.obj", 1, New HTuple, New HTuple, hv_ObjectModel3D, New HTuple)
#End If
            HOperatorSet.CountSeconds(hv_T1)
            SettingsData.ReconstructTime = BTuple.TupleSub(hv_T1, hv_T0).ToString()
            HOperatorSet.ClearStereoModel(hv_StereoModelID)

            '  MsgBox("ReconstructSurfaceStereo end")
            ClearHObject(ho_ReconstImage)
            HOperatorSet.CountSeconds(hv_T0)
            HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3D, "num_points", numPoints)
            '======================================Reduce3D============================================
            Dim hv_ReducedObject As HTuple
            Dim hv_Temp3d As HTuple = hv_ObjectModel3D
            Dim hv_Temp3d2 As HTuple = Nothing
            For Each iset As ImageSet In images
                If Not iset.Region Is Nothing Then
                    HOperatorSet.CopyObjectModel3d(hv_Temp3d, "all", hv_Temp3d2)
                    HOperatorSet.ClearObjectModel3d(hv_Temp3d)
                    HOperatorSet.PoseToHomMat3d(iset.ImagePose.ReConstPose, HM3D)
                    HOperatorSet.HomMat3dInvert(HM3D, HM3DI)
                    HOperatorSet.HomMat3dToPose(HM3DI, hv_PoseIn)
                    HOperatorSet.ReduceObjectModel3dByView(CalculateRegion(hv_CameraParam, iset.Region), hv_Temp3d2, hv_CameraParam, hv_PoseIn, hv_Temp3d)
                    HOperatorSet.ClearObjectModel3d(hv_Temp3d2)
                End If
            Next
            hv_ReducedObject = hv_Temp3d
            '======================================Reduce3D============================================

            'Dim hv_SmoothModel3D As HTuple = hv_ReducedObject
            'Dim hv_SmoothModel3D As HTuple = Nothing
            'HOperatorSet.SmoothObjectModel3d(hv_ReducedObject, "mls", New HTuple("mls_force_inwards", "mls_kNN"), New HTuple("true", 20), hv_SmoothModel3D)
            'HOperatorSet.ClearObjectModel3d(hv_ReducedObject)
            HOperatorSet.GetObjectModel3dParams(hv_ReducedObject, "num_points", numPoints)
            If BTuple.TupleInt(numPoints) > 0 Then
                '======================================Cleanup============================================
                Dim hv_TempObject3D As HTuple = Nothing
                'Dim hv_Connected As HTuple = Nothing, hv_Selected As HTuple = Nothing
                ''HOperatorSet.SmoothObjectModel3d(hv_HalconObjectModel3d, "mls", New HTuple("mls_force_inwards", "mls_kNN"), _
                ''                                 New HTuple("true", 400), hv_SmoothObject3D)
                'HOperatorSet.ConnectionObjectModel3d(hv_ReducedObject, "distance_3d", hv_ConnectDistance, hv_Connected)
                'HOperatorSet.ClearObjectModel3d(hv_ReducedObject)
                'HOperatorSet.SelectObjectModel3d(hv_Connected, "diameter_bounding_box", "and", Double.Parse(SettingsData.cleanup_min), hv_CMaxBoundingBox, hv_Selected)
                'HOperatorSet.ClearObjectModel3d(hv_Connected)
                'HOperatorSet.UnionObjectModel3d(hv_Selected, "points_surface", hv_SmoothObject3D)
                'HOperatorSet.ClearObjectModel3d(hv_Selected)
                'HOperatorSet.GetObjectModel3dParams(hv_SmoothObject3D, "num_points", numPoints)
                'If BTuple.TupleInt(numPoints) = 0 Then
                '    Exit Function
                'End If

                HOperatorSet.CopyObjectModel3d(hv_ReducedObject, "all", hv_TempObject3D)
                HOperatorSet.ClearObjectModel3d(hv_ReducedObject)
                'hv_ReducedObject = hv_SmoothObject3D
                '======================================Cleanups============================================

                '======================================Moto ni Modosu============================================
                If flgCalcByRelPose = True Then
                    HOperatorSet.AffineTransObjectModel3d(hv_TempObject3D, hv_OnePoseHomMat, hv_ReducedObject)
                Else
                    HOperatorSet.CopyObjectModel3d(hv_TempObject3D, "all", hv_ReducedObject)
                End If
                HOperatorSet.ClearObjectModel3d(hv_TempObject3D)
                '======================================Moto ni Modosu============================================


                '======================================Save3DModel==========================================

                'Dim currentDate As String = SettingsData.SelectedImageIDs & "_" & System.DateTime.Now.ToString("yyyyMMddHHmmss")
                'If Not System.IO.Directory.Exists(MainFrm.objFBM.ProjectPath & "\Pdata\" & currentDate) Then
                '    System.IO.Directory.CreateDirectory(MainFrm.objFBM.ProjectPath & "\Pdata\" & currentDate)
                'End If
                'Dim savePath = MainFrm.objFBM.ProjectPath & "\Pdata\" & currentDate & "\"
                'Dim fileName = "ReconstructionResult.obj"
                'SettingsData.OutputPath = savePath & fileName

                '' WriteObjFileWithColor(images(0).ImageFullPath, images(0).Region, images(0).ImagePose.Pose, images(0).objCamparam.Camparam, hv_ReducedObject, New HTuple(savePath & fileName))
                'WriteObjFileWithColor(images, hv_ReducedObject, New HTuple(savePath & fileName))
                ''HOperatorSet.WriteObjectModel3d(hv_ReducedObject, "obj", New HTuple(savePath & fileName), New HTuple, New HTuple)
                'If Not System.IO.File.Exists(savePath & fileName) Then
                '    If showMsg Then
                '        MsgBox("Failed to write 3d object model")
                '    End If
                'End If
                'HOperatorSet.ClearObjectModel3d(hv_ReducedObject)
                'Dim hv_TriangulatedObjectModel3D As HTuple = Nothing
                'HOperatorSet.TriangulateObjectModel3d(hv_ReducedObject, "greedy", New HTuple, New HTuple, hv_TriangulatedObjectModel3D, New HTuple)
                'HOperatorSet.ClearObjectModel3d(hv_ReducedObject)

                SettingsData.hv_ResultObject3D = hv_ReducedObject

                '======================================Save3DModel==========================================
                HOperatorSet.CountSeconds(hv_T1)
                SettingsData.OutputTime = BTuple.TupleSub(hv_T1, hv_T0).ToString()

                HOperatorSet.GetObjectModel3dParams(SettingsData.hv_ResultObject3D, "num_points", numPoints)
                If BTuple.TupleInt(numPoints) > 0 Then
                    ReconstructScene2 = True
                End If
            Else
                SettingsData.strErrorMessage = "3次元点群を算出しませんでした。"
                Throw New Exception("Not Enough Points To Visualize")
                Exit Function
            End If

        Catch ex As Exception

            SettingsData.strErrorMessage = ex.Message
            If showMsg Then
                MsgBox("Reconstruction Failed: " & ex.Message)
            End If
            Exit Function
        End Try
    End Function

    Public Function GenStereoModel(ByRef SettingsData As SettingsTable, ByVal images As List(Of ImageSet), Optional ByVal showMsg As Boolean = True, Optional ByRef flgRepeat As Boolean = False)
        GenStereoModel = False


        Dim hv_T0 As HTuple = Nothing, hv_T1 As HTuple = Nothing, numPoints As HTuple = Nothing
        Dim hv_CameraSetupModelID As HTuple = Nothing, hv_CameraParam As HTuple = Nothing, hv_StereoModelID As HTuple = Nothing
        Dim ho_Images As HObject = Nothing
        Dim hv_ObjectModel3D As HTuple = Nothing, HM3D As HTuple = Nothing, HM3DI As HTuple = Nothing
        Dim hv_PoseIn As HTuple = Nothing, hv_PoseOut As HTuple = Nothing

        HOperatorSet.GenEmptyObj(ho_Images)
        If False Then


            Try
                HOperatorSet.ReadCamPar(SettingsData.camera_param, hv_CameraParam)
                ' HOperatorSet.ReadCamPar(SettingsData.camera_param, hv_CameraParam)
            Catch ex As Exception
                HOperatorSet.ReadTuple(SettingsData.camera_param, hv_CameraParam)
            End Try
        Else
            hv_CameraParam = MainFrm.objFBM.hv_CamparamOut
        End If
        If BTuple.TupleLength(hv_CameraParam).I <= 0 Then
            If showMsg Then
                MsgBox("Invalid Camera Parameters")
            End If
            Throw New Exception("Invalid Camera Parameters")
            Exit Function
        End If
        Dim hv_HomMatModosu As HTuple = Nothing
        Dim hv_BoundingBoxByOnlyCT As New HTuple
        Try
            HOperatorSet.CreateCameraSetupModel(images.Count, hv_CameraSetupModelID)
            'Dim hv_OnePoseHomMat As HTuple = Nothing
            'Dim hv_OnePose As HTuple = images(0).ImagePose.Pose
            'Dim hv_OnePoseInvert As HTuple = Nothing

            'HOperatorSet.PoseToHomMat3d(hv_OnePose, hv_OnePoseHomMat)
            'HOperatorSet.HomMat3dInvert(hv_OnePoseHomMat, hv_HomMatModosu)
            'HOperatorSet.PoseInvert(hv_OnePose, hv_OnePoseInvert)
            If flgCalcByRelPose = True Then
                Dim tmpImagePair As New ImagePairSet(images(0), images(1))
                tmpImagePair.GetRelPoseFrom2Pose()
                If tmpImagePair.cntComCT <> -1 Then
                    Dim hv_XYZ As New HTuple
                    Dim hv_Con As New HTuple
                    HOperatorSet.GenObjectModel3dFromPoints(tmpImagePair.PairPose.X, tmpImagePair.PairPose.Y, tmpImagePair.PairPose.Z, hv_XYZ)
                    HOperatorSet.ConvexHullObjectModel3d(hv_XYZ, hv_Con)
                    HOperatorSet.ClearObjectModel3d(hv_XYZ)
                    HOperatorSet.GetObjectModel3dParams(hv_Con, "bounding_box1", hv_BoundingBoxByOnlyCT)
                    HOperatorSet.ClearObjectModel3d(hv_Con)
                Else
                    Exit Function
                End If
                Dim tmpRelPose As New HTuple '= tmpImagePair.GetRelPoseFrom2Pose
                '  Dim tmpRelPose As HTuple = tmpImagePair.RelPose(False)
                'If tmpRelPose Is Nothing Then
                '    Exit Function
                'End If

                'HOperatorSet.PoseCompose(images(0).ImagePose.Pose, hv_OnePoseInvert, images(0).ImagePose.ReConstPose)
                'images(1).ImagePose.ReConstPose = tmpRelPose
                'HOperatorSet.TupleMult(tmpRelPose, New HTuple({MainFrm.objFBM.pScaleMM, MainFrm.objFBM.pScaleMM, MainFrm.objFBM.pScaleMM, 1.0, 1.0, 1.0, 1.0}),
                '                                                              images(1).ImagePose.ReConstPose)
                For index As Integer = 0 To images.Count - 1
                    HOperatorSet.TupleMult(images(index).ImagePose.Pose, New HTuple({MainFrm.objFBM.pScaleMM, MainFrm.objFBM.pScaleMM, MainFrm.objFBM.pScaleMM, 1.0, 1.0, 1.0, 1.0}),
                                                                                    images(index).ImagePose.ReConstWorldPose)
                Next
#If DEBUG Then
                HOperatorSet.WritePose(images(0).ImagePose.ReConstWorldPose, MainFrm.objFBM.ProjectPath & "\" & images(0).ImageId & "_pose.pos")
                HOperatorSet.WritePose(images(1).ImagePose.ReConstWorldPose, MainFrm.objFBM.ProjectPath & "\" & images(1).ImageId & "_pose.pos")
#End If
                CalcRelPoseBetweenTwoPose(images(0).ImagePose.ReConstWorldPose, images(1).ImagePose.ReConstWorldPose, tmpRelPose)
                HOperatorSet.CreatePose(0, 0, 0, 0, 0, 0, "Rp+T", "gba", "point", images(0).ImagePose.ReConstPose)
                images(1).ImagePose.ReConstPose = tmpRelPose

            Else
                For index As Integer = 0 To images.Count - 1
                    HOperatorSet.TupleMult(images(index).ImagePose.Pose, New HTuple({MainFrm.objFBM.pScaleMM, MainFrm.objFBM.pScaleMM, MainFrm.objFBM.pScaleMM, 1.0, 1.0, 1.0, 1.0}),
                                                                                    images(index).ImagePose.ReConstWorldPose)
                    images(index).ImagePose.ReConstPose = images(index).ImagePose.ReConstWorldPose
                Next
            End If


            'HOperatorSet.WriteTuple(tmpImagePair.PairPose.RelPose, MainFrm.objFBM.ProjectPath & "\" & tmpImagePair.IS1.ImageId & "," & tmpImagePair.IS2.ImageId & "_relpose" & ".pss")
            For index As Integer = 0 To images.Count - 1

                '   HOperatorSet.WriteTuple(images(index).ImagePose.Pose, MainFrm.objFBM.ProjectPath & "\" & images(index).ImageId & ".pss")

                HOperatorSet.SetCameraSetupCamParam(hv_CameraSetupModelID, index, "area_scan_polynomial", hv_CameraParam, images(index).ImagePose.ReConstPose)
            Next

            HOperatorSet.CreateStereoModel(hv_CameraSetupModelID, "surface_pairwise", New HTuple, New HTuple, hv_StereoModelID)
#If DEBUG Then
            HOperatorSet.WriteCameraSetupModel(hv_CameraSetupModelID, MainFrm.objFBM.ProjectPath & "\" & SettingsData.SelectedImageIDs & ".csm")
#End If
            HOperatorSet.ClearCameraSetupModel(hv_CameraSetupModelID)
            '======================================SetStereoModel=========================================
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "persistence", New HTuple(IIf(SettingsData.persistence, 1, 0)))
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "rectif_interpolation", SettingsData.rectif_interpolation)
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "rectif_sub_sampling", Convert.ToDouble(SettingsData.rectif_sub_sampling))
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "sub_sampling_step", New HTuple(1)) ' CInt(SettingsData.sub_sampling_step)) '1に固定
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "disparity_method", SettingsData.disparity_method)
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_method", SettingsData.binocular_method)
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_num_levels", CInt(SettingsData.binocular_num_levels))
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_mask_width", CInt(SettingsData.binocular_mask_width))
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_mask_height", CInt(SettingsData.binocular_mask_height))
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_texture_thresh", Convert.ToDouble(SettingsData.binocular_texture_thresh))
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_score_thresh", Convert.ToDouble(SettingsData.binocular_score_thresh))
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_filter", SettingsData.binocular_filter)
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "binocular_sub_disparity", SettingsData.binocular_sub_disparity)
            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "point_meshing", SettingsData.point_meshing)
            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "poisson_depth", Convert.ToInt32(SettingsData.poisson_depth))
            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "poisson_solver_divide", Convert.ToInt32(SettingsData.poisson_solver_divide))
            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "poisson_samples_per_node", Convert.ToInt32(SettingsData.poisson_samples_per_node))
            '======================================SetStereoModel=========================================

            '======================================CalculateBoundingBox====================================
            Dim ImageRegion As HObject = Nothing, DirectBoundingBox As HTuple = Nothing
            Dim ReducedBox As HTuple = Nothing, TempBox As HTuple = Nothing, ConvexBox As HTuple = Nothing, SampleBox As HTuple = Nothing, TriangulatedBox As HTuple = Nothing
            Dim margin As Integer = 10
            ' HOperatorSet.GenRectangle1(ImageRegion, 0, 0, BTuple.TupleSelect(hv_CameraParam, 11), BTuple.TupleSelect(hv_CameraParam, 10))
            HOperatorSet.GenEmptyRegion(ImageRegion)
            HOperatorSet.GenRectangle1(ImageRegion, margin, margin, BTuple.TupleSelect(hv_CameraParam, 11) - margin, BTuple.TupleSelect(hv_CameraParam, 10) - margin)
            '======================================GenHomMatPyramid====================================
            Dim PX As HTuple = Nothing, PY As HTuple = Nothing, PZ As HTuple = Nothing
            Dim QX As HTuple = Nothing, QY As HTuple = Nothing, QZ As HTuple = Nothing
            Dim XX As HTuple = Nothing, XY As HTuple = Nothing, XZ As HTuple = Nothing

            Dim LineOfSightRow As HTuple = Nothing

            LineOfSightRow = BTuple.TupleConcat(LineOfSightRow, margin)
            LineOfSightRow = BTuple.TupleConcat(LineOfSightRow, margin)
            LineOfSightRow = BTuple.TupleConcat(LineOfSightRow, BTuple.TupleSelect(hv_CameraParam, 11) - margin)
            LineOfSightRow = BTuple.TupleConcat(LineOfSightRow, BTuple.TupleSelect(hv_CameraParam, 11) - margin)
            Dim LineOfSightCol As HTuple = Nothing
            LineOfSightCol = BTuple.TupleConcat(LineOfSightCol, margin)
            LineOfSightCol = BTuple.TupleConcat(LineOfSightCol, BTuple.TupleSelect(hv_CameraParam, 10) - margin)
            LineOfSightCol = BTuple.TupleConcat(LineOfSightCol, BTuple.TupleSelect(hv_CameraParam, 10) - margin)
            LineOfSightCol = BTuple.TupleConcat(LineOfSightCol, margin)

            Dim HomMat3D As HTuple = Nothing, HomMat3DIdentity As HTuple = Nothing, HomMat3DScale As HTuple = Nothing, Diameter As HTuple = Nothing
            Dim LineOfSightPoints As HTuple = Nothing, LineOfSightObject As HTuple = Nothing
            Dim LineOfSightObjectSample As HTuple = Nothing, LineOfSightObjectTriangulated As HTuple = Nothing, LineOfSightObjectScaled As HTuple = Nothing

            If Not images(0).Region Is Nothing Then 'TODO
                Dim r_Rows As HTuple = Nothing, r_Cols As HTuple = Nothing
                HOperatorSet.GetRegionConvex(CalculateRegion(hv_CameraParam, images(0).Region), r_Rows, r_Cols)
                LineOfSightRow = r_Rows
                LineOfSightCol = r_Cols
            End If

            HOperatorSet.GetLineOfSight(LineOfSightRow, LineOfSightCol, hv_CameraParam, PX, PY, PZ, QX, QY, QZ)
            HOperatorSet.TupleConcat(QX, 0.0, QX)
            HOperatorSet.TupleConcat(QY, 0.0, QY)
            HOperatorSet.TupleConcat(QZ, 0.0, QZ)
            HOperatorSet.PoseToHomMat3d(images(0).ImagePose.ReConstPose, HomMat3D)
            HOperatorSet.AffineTransPoint3d(HomMat3D, QX, QY, QZ, XX, XY, XZ)
            HOperatorSet.GenObjectModel3dFromPoints(XX, XY, XZ, LineOfSightPoints)
            HOperatorSet.ConvexHullObjectModel3d(LineOfSightPoints, LineOfSightObject)
            HOperatorSet.ClearObjectModel3d(LineOfSightPoints)
            ' Dim hScaleAtPiramid As New HTuple((SettingsData.bounding_box * 1000 / MainFrm.objFBM.pScaleMM) / hv_CameraParam(0))
            Dim hScaleAtPiramid As New HTuple((SettingsData.bounding_box * 1000) / hv_CameraParam(0))
            Try
                HOperatorSet.HomMat3dIdentity(HomMat3DIdentity)
                HOperatorSet.HomMat3dScale(HomMat3DIdentity, hScaleAtPiramid, hScaleAtPiramid, hScaleAtPiramid, _
                                 BTuple.TupleSelect(images(0).ImagePose.ReConstPose, 0), _
                                 BTuple.TupleSelect(images(0).ImagePose.ReConstPose, 1), _
                                 BTuple.TupleSelect(images(0).ImagePose.ReConstPose, 2), HomMat3DScale)
                HOperatorSet.AffineTransObjectModel3d(LineOfSightObject, HomMat3DScale, LineOfSightObjectScaled)
                HOperatorSet.ClearObjectModel3d(LineOfSightObject)

                HOperatorSet.GetObjectModel3dParams(LineOfSightObjectScaled, "diameter_axis_aligned_bounding_box", Diameter)
                HOperatorSet.SampleObjectModel3d(LineOfSightObjectScaled, "fast", BTuple.TupleMult(ReconstructionWindow.SamplingFactor, Diameter), New HTuple, New HTuple, LineOfSightObjectSample)
                HOperatorSet.ClearObjectModel3d(LineOfSightObjectScaled)
                HOperatorSet.TriangulateObjectModel3d(LineOfSightObjectSample, "greedy", New HTuple, New HTuple, LineOfSightObjectTriangulated, New HTuple)
                HOperatorSet.ClearObjectModel3d(LineOfSightObjectSample)

                '======================================GenHomMatPyramid====================================
                HOperatorSet.CopyObjectModel3d(LineOfSightObjectTriangulated, "all", ConvexBox)
                HOperatorSet.ClearObjectModel3d(LineOfSightObjectTriangulated)
                Dim hv_ZeroCameraParam As HTuple = Nothing
                HOperatorSet.TupleReplace(hv_CameraParam, New HTuple(1, 2, 3, 4, 5), 0, hv_ZeroCameraParam)
                For i As Integer = 1 To images.Count - 1
                    HOperatorSet.CopyObjectModel3d(ConvexBox, "all", TempBox)
                    HOperatorSet.ClearObjectModel3d(ConvexBox)
                    HOperatorSet.GetObjectModel3dParams(TempBox, "num_points", numPoints)
                    If numPoints.I <= 0 Then
                        Debug.Print("")
                    End If
                    HOperatorSet.PoseToHomMat3d(images(i).ImagePose.ReConstPose, HM3D)
                    HOperatorSet.HomMat3dInvert(HM3D, HM3DI)
                    HOperatorSet.HomMat3dToPose(HM3DI, hv_PoseIn)
                    If images(i).Region Is Nothing Then
                        HOperatorSet.ReduceObjectModel3dByView(ImageRegion, TempBox, hv_ZeroCameraParam, hv_PoseIn, ReducedBox)

                    Else
                        HOperatorSet.ReduceObjectModel3dByView(CalculateRegion(hv_CameraParam, images(i).Region), TempBox, hv_ZeroCameraParam, hv_PoseIn, ReducedBox)
                    End If
                    HOperatorSet.ClearObjectModel3d(TempBox)
                    HOperatorSet.GetObjectModel3dParams(ReducedBox, "num_points", numPoints)
                    If numPoints.I <= 0 Then
                        If showMsg Then
                            MsgBox("バウンディングボックス算出でエラー発生しました。「" & images(i).ImageId & ":" & images(i).ImageName & "」画像のレジョンの設定に誤りがあります。")
                        End If

                        Exit Function
                    End If
                    'HOperatorSet.GetObjectModel3dParams(ReducedBox, "bounding_box1", DirectBoundingBox)
                    HOperatorSet.ConvexHullObjectModel3d(ReducedBox, ConvexBox)
                    HOperatorSet.ClearObjectModel3d(ReducedBox)
                    HOperatorSet.GetObjectModel3dParams(ConvexBox, "diameter_axis_aligned_bounding_box", Diameter)
                    HOperatorSet.SampleObjectModel3d(ConvexBox, "fast", BTuple.TupleMult(ReconstructionWindow.SamplingFactor, Diameter), New HTuple, New HTuple, SampleBox)
                    HOperatorSet.ClearObjectModel3d(ConvexBox)
                    HOperatorSet.TriangulateObjectModel3d(SampleBox, "greedy", New HTuple, New HTuple, TriangulatedBox, New HTuple)
                    HOperatorSet.ClearObjectModel3d(SampleBox)
                    HOperatorSet.CopyObjectModel3d(TriangulatedBox, "all", ConvexBox)
                    HOperatorSet.ClearObjectModel3d(TriangulatedBox)
                    HOperatorSet.GetObjectModel3dParams(ConvexBox, "bounding_box1", DirectBoundingBox)

                Next

            Catch ex As Exception
                flgRepeat = True
                If Not hv_StereoModelID Is Nothing Then
                    HOperatorSet.ClearStereoModel(hv_StereoModelID)
                End If

                SettingsData.hv_StereoModelID = Nothing
            End Try
         
         

            '####バウンディングボックスがカメラを中に含んいないかを調べる。
            Dim hvCameraInBoundinbox As HTuple = Nothing
            Dim isCameraInBoundingbox As Boolean = False
            Dim boxpose As HTuple = Nothing
            Dim boxL1 As HTuple = Nothing
            Dim boxL2 As HTuple = Nothing
            Dim boxL3 As HTuple = Nothing
            Dim hv_BoundBox As HTuple = Nothing
            Dim hv_BBzoomfactor As New HTuple(1.0)
            HOperatorSet.SmallestBoundingBoxObjectModel3d(ConvexBox, "axis_aligned", boxpose, boxL1, boxL2, boxL3)
            Do
                HOperatorSet.GenEmptyObjectModel3d(hv_BoundBox)
                HOperatorSet.GenBoxObjectModel3d(boxpose, boxL1 * hv_BBzoomfactor, boxL2 * hv_BBzoomfactor, boxL3 * hv_BBzoomfactor, hv_BoundBox)
                For index As Integer = 0 To images.Count - 1
                    HOperatorSet.IntersectPlaneObjectModel3d(hv_BoundBox, images(index).ImagePose.ReConstPose, hvCameraInBoundinbox)
                    HOperatorSet.GetObjectModel3dParams(hvCameraInBoundinbox, "num_points", numPoints)
                    If BTuple.TupleInt(numPoints) > 0 Then
                        isCameraInBoundingbox = True
                    End If
                    HOperatorSet.ClearObjectModel3d(hvCameraInBoundinbox)
                Next

                If isCameraInBoundingbox = False Or hv_BBzoomfactor < 0.1 Then
                    Exit Do
                Else
                    hv_BBzoomfactor = hv_BBzoomfactor - 0.1
                    isCameraInBoundingbox = False
                End If
                HOperatorSet.ClearObjectModel3d(hv_BoundBox)
            Loop
            HOperatorSet.GetObjectModel3dParams(hv_BoundBox, "bounding_box1", DirectBoundingBox)
            HOperatorSet.ClearObjectModel3d(hv_BoundBox)
            '####バウンディングボックスがカメラを中に含んいないかを調べる。

            If flgCalcByRelPose = True Then
                HOperatorSet.TupleMult(hv_BoundingBoxByOnlyCT, MainFrm.objFBM.pScaleMM, hv_BoundingBoxByOnlyCT)
                'SettingsData.bounding_boxの値をメートル単位とし、CTのみで計算したバウンディングボックスを奥行方向に伸ばす量とする。
                '手前
                DirectBoundingBox(2) = hv_BoundingBoxByOnlyCT(2) - 500
                '奥
                DirectBoundingBox(5) = hv_BoundingBoxByOnlyCT(5) + SettingsData.bounding_box * 1000
                ' DirectBoundingBox = hv_BoundingBoxByOnlyCT
                '   DirectBoundingBox(2) = hv_BoundingBoxByOnlyCT(2)

            End If

          
       
            '  HOperatorSet.ClearObj(ImageRegion)
            ClearHObject(ImageRegion)
            ' HOperatorSet.GetObjectModel3dParams(ConvexBox, "bounding_box1", DirectBoundingBox)
            'Dim TemaewoTouku As HTuple = New HTuple(1.0, 1.0, 2.0, 1.0, 1.0, 1.0)
            ''  TemaewoTouku = New HTuple(0.5, 0.5, 2.0, 0.5, 0.5, 0.5)
            'HOperatorSet.TupleMult(DirectBoundingBox, TemaewoTouku, DirectBoundingBox)
            'HOperatorSet.SetStereoModelParam(hv_StereoModelID, "bounding_box", ttttbb)
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "bounding_box", DirectBoundingBox)
            HOperatorSet.ClearObjectModel3d(ConvexBox)
#If DEBUG Then
            HOperatorSet.WriteTuple(DirectBoundingBox, MainFrm.objFBM.ProjectPath & "\" & SettingsData.SelectedImageIDs & "boundingbox.tpl")

#End If
            '======================================CalculateBoundingBox====================================

            ' ''======================================SetImagePair============================================
            ''Dim imagesCount As Int32 = 0
            ''imagesCount = images.Count
            ''Try
            ''    For i As Integer = 0 To imagesCount - 2 Step 1
            ''        HOperatorSet.SetStereoModelImagePairs(hv_StereoModelID, i, i + 1)
            ''    Next
            ''Catch ex As Exception
            ''    HOperatorSet.ClearStereoModel(hv_StereoModelID)
            ''    SettingsData.hv_StereoModelID = Nothing
            ''    Exit Function
            ''End Try

            ' ''======================================SetImagePair============================================
            SettingsData.hv_StereoModelID = hv_StereoModelID

            GenStereoModel = True
        Catch ex As Exception
            flgRepeat = True
            If Not hv_StereoModelID Is Nothing Then
                HOperatorSet.ClearStereoModel(hv_StereoModelID)
            End If

            SettingsData.hv_StereoModelID = Nothing

        End Try




    End Function
    Private Sub WriteObjFileWithColor(ByVal imagePath As String, ByVal region As HObject, ByVal pose As HTuple, ByVal CamParam As HTuple, ByVal ObjectModel3d As HTuple, ByVal FileName As String)
        Dim image As HObject = Nothing
        HOperatorSet.ReadImage(image, imagePath)
        Dim HomMat3d As HTuple = Nothing, HomMat3dInvert As HTuple = Nothing, ObjectModelTrans As HTuple = Nothing
        HOperatorSet.PoseToHomMat3d(pose, HomMat3d)
        HOperatorSet.HomMat3dInvert(HomMat3d, HomMat3dInvert)
        HOperatorSet.AffineTransObjectModel3d(ObjectModel3d, HomMat3dInvert, ObjectModelTrans)

        Dim X As HTuple = Nothing, Y As HTuple = Nothing, Z As HTuple = Nothing
        HOperatorSet.GetObjectModel3dParams(ObjectModelTrans, "point_coord_x", X)
        HOperatorSet.GetObjectModel3dParams(ObjectModelTrans, "point_coord_y", Y)
        HOperatorSet.GetObjectModel3dParams(ObjectModelTrans, "point_coord_z", Z)
        Dim Row As HTuple = Nothing, Col As HTuple = Nothing
        HOperatorSet.Project3dPoint(X, Y, Z, CamParam, Row, Col)
        'Dim RowMax As HTuple = Nothing, ColMax As HTuple = Nothing
        'Dim RowMin As HTuple = Nothing, ColMin As HTuple = Nothing

        'HOperatorSet.TupleGenConst(Row.Length, CamParam(11) - 1, RowMax)
        'HOperatorSet.TupleGenConst(Row.Length, 0, RowMin)
        'HOperatorSet.TupleGenConst(Col.Length, CamParam(10) - 1, ColMax)
        'HOperatorSet.TupleGenConst(Col.Length, 0, ColMin)

        'HOperatorSet.TupleMin2(RowMax, Row, Row)
        'HOperatorSet.TupleMax2(RowMin, Row, Row)
        'HOperatorSet.TupleMin2(ColMax, Col, Col)
        'HOperatorSet.TupleMax2(ColMin, Col, Col)

        Dim Grayval As HTuple = Nothing
        Try
            HOperatorSet.GetGrayval(image, Row, Col, Grayval)
        Catch ex As Exception
            HOperatorSet.TupleGenConst(Row.TupleLength * 3, New HTuple(0.0), Grayval)
        End Try

        ClearHObject(image)
        'HOperatorSet.GetGrayvalInterpolated(image, Row, Col, "bilinear", Grayval)
        Dim gi = 0
        'Dim objFileHandle As HTuple = Nothing
        'HOperatorSet.OpenFile(FileName, "output", objFileHandle)

        HOperatorSet.GetObjectModel3dParams(ObjectModel3d, "point_coord_x", X)
        HOperatorSet.GetObjectModel3dParams(ObjectModel3d, "point_coord_y", Y)
        HOperatorSet.GetObjectModel3dParams(ObjectModel3d, "point_coord_z", Z)
        Dim file As System.IO.StreamWriter
        file = My.Computer.FileSystem.OpenTextFileWriter(FileName, True)
        For vi As Integer = 0 To X.Length - 1 Step 1
            Dim fRow As String = "v " & X(vi).D & " " & Y(vi).D & " " & Z(vi).D
            fRow &= " " & (Grayval(gi).D / 255) & " " & (Grayval(gi + 1).D / 255) & " " & (Grayval(gi + 2).D / 255)
            gi += 3
            file.WriteLine(fRow)
            'HOperatorSet.FwriteString(objFileHandle, fRow)
        Next
        'HOperatorSet.CloseFile(objFileHandle)
        file.Close()
    End Sub

    Public Sub WriteObjFileWithColor(ByRef Images As List(Of ImageSet), ByVal ObjectModel3d As HTuple, ByVal FileName As String)
        Dim image As HObject = Nothing
        For Each OneImage As ImageSet In Images
            HOperatorSet.ReadImage(image, OneImage.ImageFullPath)
            Dim HomMat3d As HTuple = Nothing, HomMat3dInvert As HTuple = Nothing, ObjectModelTrans As HTuple = Nothing
            HOperatorSet.PoseToHomMat3d(OneImage.ImagePose.Pose, HomMat3d)
            HOperatorSet.HomMat3dInvert(HomMat3d, HomMat3dInvert)
            HOperatorSet.AffineTransObjectModel3d(ObjectModel3d, HomMat3dInvert, ObjectModelTrans)

            Dim X As HTuple = Nothing, Y As HTuple = Nothing, Z As HTuple = Nothing
            HOperatorSet.GetObjectModel3dParams(ObjectModelTrans, "point_coord_x", X)
            HOperatorSet.GetObjectModel3dParams(ObjectModelTrans, "point_coord_y", Y)
            HOperatorSet.GetObjectModel3dParams(ObjectModelTrans, "point_coord_z", Z)
            Dim Row As HTuple = Nothing, Col As HTuple = Nothing
            HOperatorSet.Project3dPoint(X, Y, Z, OneImage.objCamparam.Camparam, Row, Col)
            'Dim RowMax As HTuple = Nothing, ColMax As HTuple = Nothing
            'Dim RowMin As HTuple = Nothing, ColMin As HTuple = Nothing

            'HOperatorSet.TupleGenConst(Row.Length, CamParam(11) - 1, RowMax)
            'HOperatorSet.TupleGenConst(Row.Length, 0, RowMin)
            'HOperatorSet.TupleGenConst(Col.Length, CamParam(10) - 1, ColMax)
            'HOperatorSet.TupleGenConst(Col.Length, 0, ColMin)

            'HOperatorSet.TupleMin2(RowMax, Row, Row)
            'HOperatorSet.TupleMax2(RowMin, Row, Row)
            'HOperatorSet.TupleMin2(ColMax, Col, Col)
            'HOperatorSet.TupleMax2(ColMin, Col, Col)

            Dim Grayval As HTuple = Nothing
            Try
                HOperatorSet.GetGrayval(image, Row, Col, Grayval)
            Catch ex As Exception
                '              HOperatorSet.TupleGenConst(Row.TupleLength * 3, New HTuple(0.0), Grayval)
                HOperatorSet.ClearObjectModel3d(ObjectModelTrans)
                Continue For
            End Try

            ClearHObject(image)
            'HOperatorSet.GetGrayvalInterpolated(image, Row, Col, "bilinear", Grayval)
            Dim gi = 0
            'Dim objFileHandle As HTuple = Nothing
            'HOperatorSet.OpenFile(FileName, "output", objFileHandle)


            HOperatorSet.GetObjectModel3dParams(ObjectModel3d, "point_coord_x", X)
            HOperatorSet.GetObjectModel3dParams(ObjectModel3d, "point_coord_y", Y)
            HOperatorSet.GetObjectModel3dParams(ObjectModel3d, "point_coord_z", Z)
            'HOperatorSet.WriteTuple(X, FileName & "_X.tpl")
            'HOperatorSet.WriteTuple(Y, FileName & "_Y.tpl")
            'HOperatorSet.WriteTuple(Z, FileName & "_Z.tpl")
            'HOperatorSet.WriteTuple(Grayval, FileName & "_G.tpl")
#If False Then
            Dim hv_X As HTuple = New HTuple, hv_Y As HTuple = New HTuple
            Dim hv_Z As HTuple = New HTuple, hv_G As HTuple = New HTuple
            Dim hv_Length As HTuple = New HTuple, hv_SequenceX As HTuple = New HTuple
            Dim hv_SequenceY As HTuple = New HTuple, hv_SequenceZ As HTuple = New HTuple
            Dim hv_GX As HTuple = New HTuple, hv_GY As HTuple = New HTuple
            Dim hv_GZ As HTuple = New HTuple, hv_Result As HTuple = New HTuple
            Dim hv_FileHandle As HTuple = New HTuple
            HOperatorSet.TupleMult(Grayval, New HTuple(1.0), hv_G)
            HOperatorSet.TupleLength(hv_G, hv_Length)
            HOperatorSet.TupleGenSequence(New HTuple(0), hv_Length.TupleSub(New HTuple(1)), New HTuple(3), hv_SequenceX)
            HOperatorSet.TupleGenSequence(New HTuple(1), hv_Length.TupleSub(New HTuple(1)), New HTuple(3), hv_SequenceY)
            HOperatorSet.TupleGenSequence(New HTuple(2), hv_Length.TupleSub(New HTuple(1)), New HTuple(3), hv_SequenceZ)
            HOperatorSet.TupleSelect(hv_G, hv_SequenceX, hv_GX)
            HOperatorSet.TupleSelect(hv_G, hv_SequenceY, hv_GY)
            HOperatorSet.TupleSelect(hv_G, hv_SequenceZ, hv_GZ)
            HOperatorSet.TupleDiv(hv_GX, New HTuple(255.0), hv_GX)
            HOperatorSet.TupleDiv(hv_GY, New HTuple(255.0), hv_GY)
            HOperatorSet.TupleDiv(hv_GZ, New HTuple(255.0), hv_GZ)
            hv_Result = (((((((((((((((((((((((New HTuple("v ")).TupleAdd(X))).TupleAdd( _
                New HTuple(" ")))).TupleAdd(Y))).TupleAdd(New HTuple(" ")))).TupleAdd(Z))).TupleAdd( _
                New HTuple(" ")))).TupleAdd(hv_GX))).TupleAdd(New HTuple(" ")))).TupleAdd(hv_GY))).TupleAdd( _
                New HTuple(" ")))).TupleAdd(hv_GZ))).TupleAdd(New HTuple("" + Chr(10)))
            HOperatorSet.OpenFile(New HTuple(FileName), New HTuple("output"), hv_FileHandle)
            HOperatorSet.FwriteString(hv_FileHandle, hv_Result)
            HOperatorSet.CloseFile(hv_FileHandle)
#Else
            'Dim file As System.IO.StreamWriter
            'file = My.Computer.FileSystem.OpenTextFileWriter(FileName, True)
            'For vi As Integer = 0 To X.Length - 1 Step 1
            '    Dim fRow As String = "v " & X(vi).D & " " & Y(vi).D & " " & Z(vi).D
            '    fRow &= " " & (Grayval(gi).D / 255) & " " & (Grayval(gi + 1).D / 255) & " " & (Grayval(gi + 2).D / 255)
            '    gi += 3
            '    file.WriteLine(fRow)
            'Next
            'file.Close()


            Using file As System.IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(FileName, True)
                For vi As Integer = 0 To X.Length - 1 Step 1
                    Dim fRow As String = "v " & X(vi).D & " " & Y(vi).D & " " & Z(vi).D
                    fRow &= " " & (Grayval(gi).D / 255) & " " & (Grayval(gi + 1).D / 255) & " " & (Grayval(gi + 2).D / 255)
                    gi += 3
                    file.WriteLine(fRow)
                Next
                file.Close()
            End Using

            'Dim fN As Integer = X.Length
            'Try
            '    For vi As Integer = 0 To fN - 1 Step 1
            '        Dim fRow As String = "v " & X(vi).D & " " & Y(vi).D & " " & Z(vi).D
            '        fRow &= " " & (Grayval(gi).D / 255) & " " & (Grayval(gi + 1).D / 255) & " " & (Grayval(gi + 2).D / 255)
            '        gi += 3
            '        My.Computer.FileSystem.WriteAllText(FileName, fRow & vbNewLine, True)
            '    Next
            'Catch ex As Exception
            '    MsgBox(ex.Message)
            'End Try


#End If

            HOperatorSet.ClearObjectModel3d(ObjectModelTrans)
            Exit For
        Next

    End Sub

    Public Sub WriteObjFileWithColorAndMesh(ByRef Images As List(Of ImageSet), ByVal ObjectModel3d As HTuple, ByVal FileName As String)
        Dim image As HObject = Nothing
        For Each OneImage As ImageSet In Images
            HOperatorSet.ReadImage(image, OneImage.ImageFullPath)
            Dim HomMat3d As HTuple = Nothing, HomMat3dInvert As HTuple = Nothing, ObjectModelTrans As HTuple = Nothing
            HOperatorSet.PoseToHomMat3d(OneImage.ImagePose.Pose, HomMat3d)
            HOperatorSet.HomMat3dInvert(HomMat3d, HomMat3dInvert)
            HOperatorSet.AffineTransObjectModel3d(ObjectModel3d, HomMat3dInvert, ObjectModelTrans)

            Dim X As HTuple = Nothing, Y As HTuple = Nothing, Z As HTuple = Nothing
            HOperatorSet.GetObjectModel3dParams(ObjectModelTrans, "point_coord_x", X)
            HOperatorSet.GetObjectModel3dParams(ObjectModelTrans, "point_coord_y", Y)
            HOperatorSet.GetObjectModel3dParams(ObjectModelTrans, "point_coord_z", Z)
            Dim Row As HTuple = Nothing, Col As HTuple = Nothing
            HOperatorSet.Project3dPoint(X, Y, Z, OneImage.objCamparam.Camparam, Row, Col)
            'Dim RowMax As HTuple = Nothing, ColMax As HTuple = Nothing
            'Dim RowMin As HTuple = Nothing, ColMin As HTuple = Nothing

            'HOperatorSet.TupleGenConst(Row.Length, CamParam(11) - 1, RowMax)
            'HOperatorSet.TupleGenConst(Row.Length, 0, RowMin)
            'HOperatorSet.TupleGenConst(Col.Length, CamParam(10) - 1, ColMax)
            'HOperatorSet.TupleGenConst(Col.Length, 0, ColMin)

            'HOperatorSet.TupleMin2(RowMax, Row, Row)
            'HOperatorSet.TupleMax2(RowMin, Row, Row)
            'HOperatorSet.TupleMin2(ColMax, Col, Col)
            'HOperatorSet.TupleMax2(ColMin, Col, Col)

            Dim Grayval As HTuple = Nothing
            Try
                HOperatorSet.GetGrayval(image, Row, Col, Grayval)
            Catch ex As Exception
                '              HOperatorSet.TupleGenConst(Row.TupleLength * 3, New HTuple(0.0), Grayval)
                HOperatorSet.ClearObjectModel3d(ObjectModelTrans)
                Continue For
            End Try

            ClearHObject(image)
            'HOperatorSet.GetGrayvalInterpolated(image, Row, Col, "bilinear", Grayval)

            'Dim objFileHandle As HTuple = Nothing
            'HOperatorSet.OpenFile(FileName, "output", objFileHandle)



            Dim hTriangle As New HTuple
            HOperatorSet.GetObjectModel3dParams(ObjectModel3d, "triangles", hTriangle)

            HOperatorSet.GetObjectModel3dParams(ObjectModel3d, "point_coord_x", X)
            HOperatorSet.GetObjectModel3dParams(ObjectModel3d, "point_coord_y", Y)
            HOperatorSet.GetObjectModel3dParams(ObjectModel3d, "point_coord_z", Z)
            Dim nX As New HTuple, nY As New HTuple, nZ As New HTuple
            HOperatorSet.GetObjectModel3dParams(ObjectModel3d, "point_normal_x", nX)
            HOperatorSet.GetObjectModel3dParams(ObjectModel3d, "point_normal_y", nY)
            HOperatorSet.GetObjectModel3dParams(ObjectModel3d, "point_normal_z", nZ)

            HOperatorSet.ClearObjectModel3d(ObjectModel3d)
            'HOperatorSet.WriteTuple(X, FileName & "_X.tpl")
            'HOperatorSet.WriteTuple(Y, FileName & "_Y.tpl")
            'HOperatorSet.WriteTuple(Z, FileName & "_Z.tpl")
            'HOperatorSet.WriteTuple(Grayval, FileName & "_G.tpl")
#If False Then
            Dim hv_X As HTuple = New HTuple, hv_Y As HTuple = New HTuple
            Dim hv_Z As HTuple = New HTuple, hv_G As HTuple = New HTuple
            Dim hv_Length As HTuple = New HTuple, hv_SequenceX As HTuple = New HTuple
            Dim hv_SequenceY As HTuple = New HTuple, hv_SequenceZ As HTuple = New HTuple
            Dim hv_GX As HTuple = New HTuple, hv_GY As HTuple = New HTuple
            Dim hv_GZ As HTuple = New HTuple, hv_Result As HTuple = New HTuple
            Dim hv_FileHandle As HTuple = New HTuple
            HOperatorSet.TupleMult(Grayval, New HTuple(1.0), hv_G)
            HOperatorSet.TupleLength(hv_G, hv_Length)
            HOperatorSet.TupleGenSequence(New HTuple(0), hv_Length.TupleSub(New HTuple(1)), New HTuple(3), hv_SequenceX)
            HOperatorSet.TupleGenSequence(New HTuple(1), hv_Length.TupleSub(New HTuple(1)), New HTuple(3), hv_SequenceY)
            HOperatorSet.TupleGenSequence(New HTuple(2), hv_Length.TupleSub(New HTuple(1)), New HTuple(3), hv_SequenceZ)
            HOperatorSet.TupleSelect(hv_G, hv_SequenceX, hv_GX)
            HOperatorSet.TupleSelect(hv_G, hv_SequenceY, hv_GY)
            HOperatorSet.TupleSelect(hv_G, hv_SequenceZ, hv_GZ)
            HOperatorSet.TupleDiv(hv_GX, New HTuple(255.0), hv_GX)
            HOperatorSet.TupleDiv(hv_GY, New HTuple(255.0), hv_GY)
            HOperatorSet.TupleDiv(hv_GZ, New HTuple(255.0), hv_GZ)
            hv_Result = (((((((((((((((((((((((New HTuple("v ")).TupleAdd(X))).TupleAdd( _
                New HTuple(" ")))).TupleAdd(Y))).TupleAdd(New HTuple(" ")))).TupleAdd(Z))).TupleAdd( _
                New HTuple(" ")))).TupleAdd(hv_GX))).TupleAdd(New HTuple(" ")))).TupleAdd(hv_GY))).TupleAdd( _
                New HTuple(" ")))).TupleAdd(hv_GZ))).TupleAdd(New HTuple("" + Chr(10)))
            HOperatorSet.OpenFile(New HTuple(FileName), New HTuple("output"), hv_FileHandle)
            HOperatorSet.FwriteString(hv_FileHandle, hv_Result)
            HOperatorSet.CloseFile(hv_FileHandle)
#Else
            'Dim file As System.IO.StreamWriter
            'file = My.Computer.FileSystem.OpenTextFileWriter(FileName, True)
            'For vi As Integer = 0 To X.Length - 1 Step 1
            '    Dim fRow As String = "v " & X(vi).D & " " & Y(vi).D & " " & Z(vi).D
            '    fRow &= " " & (Grayval(gi).D / 255) & " " & (Grayval(gi + 1).D / 255) & " " & (Grayval(gi + 2).D / 255)
            '    gi += 3
            '    file.WriteLine(fRow)
            'Next
            'file.Close()
            Dim normal_cnt As New HTuple
            HOperatorSet.TupleLength(nX, normal_cnt)
            Dim triangle_cnt As New HTuple
            HOperatorSet.TupleLength(hTriangle, triangle_cnt)
            HOperatorSet.TupleAdd(hTriangle, New HTuple(1), hTriangle)
            HOperatorSet.TupleDiv(Grayval, New HTuple(255.0), Grayval)

            Dim gi = 0
            Using file As System.IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(FileName, True)
                Dim fRow As String = ""
                If normal_cnt.I > 0 Then
                    For vi As Integer = 0 To X.Length - 1 Step 1

                        '法線ベクトルの書き込み
                        fRow = "vn " & nX(vi).D & " " & nY(vi).D & " " & nZ(vi).D
                        file.WriteLine(fRow)

                        '3次元点座標と色情報の書き込み
                        fRow = "v " & X(vi).D & " " & Y(vi).D & " " & Z(vi).D
                        fRow &= " " & Grayval(gi).D & " " & Grayval(gi + 1).D & " " & Grayval(gi + 2).D
                        file.WriteLine(fRow)
                        gi += 3
                    Next
                Else
                    For vi As Integer = 0 To X.Length - 1 Step 1
                        '3次元点座標と色情報の書き込み
                        fRow = "v " & X(vi).D & " " & Y(vi).D & " " & Z(vi).D
                        fRow &= " " & Grayval(gi).D & " " & Grayval(gi + 1).D & " " & Grayval(gi + 2).D
                        file.WriteLine(fRow)
                        gi += 3
                    Next
                End If
                'メッシの書き込み
                If triangle_cnt.I > 0 Then
                    For ti As Integer = 0 To triangle_cnt - 1 Step 3
                        fRow = "f " & hTriangle(ti).I & " " & hTriangle(ti + 1).I & " " & hTriangle(ti + 2).I
                        file.WriteLine(fRow)
                    Next
                End If
                file.Close()
            End Using

            'Dim fN As Integer = X.Length
            'Try
            '    For vi As Integer = 0 To fN - 1 Step 1
            '        Dim fRow As String = "v " & X(vi).D & " " & Y(vi).D & " " & Z(vi).D
            '        fRow &= " " & (Grayval(gi).D / 255) & " " & (Grayval(gi + 1).D / 255) & " " & (Grayval(gi + 2).D / 255)
            '        gi += 3
            '        My.Computer.FileSystem.WriteAllText(FileName, fRow & vbNewLine, True)
            '    Next
            'Catch ex As Exception
            '    MsgBox(ex.Message)
            'End Try


#End If

            HOperatorSet.ClearObjectModel3d(ObjectModelTrans)
            Exit For
        Next

    End Sub

    Public Function UnionScene(ByVal scenePath As String) As Boolean
        UnionScene = False
        Dim savePath As String = MainFrm.objFBM.ProjectPath & "\Pdata\" & ReconstructionWindow.FileName3D
        Using reader = File.OpenRead(scenePath)
            Using writer = New FileStream(savePath, FileMode.Append)
                Dim b = reader.ReadByte()
                While b <> -1
                    writer.WriteByte(CByte(b))
                    b = reader.ReadByte()
                End While
            End Using
        End Using
        'Dim objectModel3D As Object = Nothing
        'Dim PreviousObjectModel As Object = Nothing
        'Dim UnionObjectModel As Object = Nothing
        'HOperatorSet.ReadObjectModel3d(scenePath, 1, New HTuple, New HTuple, objectModel3D, New HTuple)
        'Dim savePath As String = MainFrm.objFBM.ProjectPath & "\Pdata\" & ReconstructionWindow.FileName3D

        'If System.IO.File.Exists(savePath) Then
        '    HOperatorSet.ReadObjectModel3d(savePath, BTuple.TupleAdd(0, 1), "file_type", "obj", PreviousObjectModel, New HTuple)
        'End If
        'HOperatorSet.UnionObjectModel3D(BTuple.TupleConcat(objectModel3D, PreviousObjectModel), "points_surface", UnionObjectModel)
        'If Not objectModel3D Is Nothing Then
        '    HOperatorSet.ClearObjectModel3d(objectModel3D)
        'End If

        'If Not PreviousObjectModel Is Nothing Then
        '    HOperatorSet.ClearObjectModel3d(PreviousObjectModel)
        'End If

        'objectModel3D = UnionObjectModel
        'HOperatorSet.WriteObjectModel3d(UnionObjectModel, "obj", savePath, New HTuple, New HTuple)
        If Not System.IO.File.Exists(savePath) Then
            MsgBox("Failed to write 3d object model")
        Else
            UnionScene = True
        End If
    End Function
    '20170302 baluu add end
    '20170612 SUURI ADD start
    Public Function UnionScene2(ByVal scenePath As String, ByRef writer As FileStream) As Boolean
        UnionScene2 = False
        Dim savePath As String = MainFrm.objFBM.ProjectPath & "\Pdata\" & ReconstructionWindow.FileName3D
        Using reader = File.OpenRead(scenePath)
            Dim b = reader.ReadByte()
            While b <> -1
                writer.WriteByte(CByte(b))
                b = reader.ReadByte()
            End While
        End Using

        'Dim objectModel3D As Object = Nothing
        'Dim PreviousObjectModel As Object = Nothing
        'Dim UnionObjectModel As Object = Nothing
        'HOperatorSet.ReadObjectModel3d(scenePath, 1, New HTuple, New HTuple, objectModel3D, New HTuple)
        'Dim savePath As String = MainFrm.objFBM.ProjectPath & "\Pdata\" & ReconstructionWindow.FileName3D

        'If System.IO.File.Exists(savePath) Then
        '    HOperatorSet.ReadObjectModel3d(savePath, BTuple.TupleAdd(0, 1), "file_type", "obj", PreviousObjectModel, New HTuple)
        'End If
        'HOperatorSet.UnionObjectModel3D(BTuple.TupleConcat(objectModel3D, PreviousObjectModel), "points_surface", UnionObjectModel)
        'If Not objectModel3D Is Nothing Then
        '    HOperatorSet.ClearObjectModel3d(objectModel3D)
        'End If

        'If Not PreviousObjectModel Is Nothing Then
        '    HOperatorSet.ClearObjectModel3d(PreviousObjectModel)
        'End If

        'objectModel3D = UnionObjectModel
        'HOperatorSet.WriteObjectModel3d(UnionObjectModel, "obj", savePath, New HTuple, New HTuple)
        If Not System.IO.File.Exists(savePath) Then
            MsgBox("Failed to write 3d object model")
        Else
            UnionScene2 = True
        End If
    End Function
    '20170612 SUURI ADD end
    'add by SUSANO 
    Public Class SekkeiData
        Public PointName As String
        Public Point3d As FBMlib.Point3D
        Public flgKijyun As Integer
        Public strInfo As String
        Public Sub New()
            PointName = ""
            Point3d = New FBMlib.Point3D
            flgKijyun = 0
            strInfo = ""
        End Sub
    End Class
    'add by SUSANO 
    Public Class BunkatsuLabeling
        Public BuzaiName As String
        Public SekkeiFileName As String
        Public HeadStr As String
        Public Kubun1 As Integer
        Public lstCommonCT As List(Of FBMlib.Common3DCodedTarget)
        Public lstSekkeiData As List(Of SekkeiData)
        Public Sub New()
            BuzaiName = ""
            SekkeiFileName = ""
            HeadStr = ""
            Kubun1 = -1
            lstCommonCT = New List(Of FBMlib.Common3DCodedTarget)
            lstSekkeiData = New List(Of SekkeiData)
        End Sub
        'add by SUSANO 
        Public Function ReadSekkeiData(ByVal strFileName As String) As Boolean
            ReadSekkeiData = False
            If lstSekkeiData Is Nothing Then
                lstSekkeiData = New List(Of SekkeiData)
            Else
                lstSekkeiData.Clear()
            End If
            Dim filename As String = strFileName
            Dim fields As String()
            Dim delimiter As String = ","
            Dim i As Integer = 0
            Using parser As New TextFieldParser(filename)
                parser.SetDelimiters(delimiter)
                While Not parser.EndOfData
                    fields = parser.ReadFields()
                    If i = 0 Then
                        i += 1
                        Continue While
                    End If
                    If fields.Count() = 6 Then
                        Dim newObjSekkeidata As New SekkeiData

                        newObjSekkeidata.PointName = fields(0)
                        If IsNumeric(fields(1)) And IsNumeric(fields(2)) And IsNumeric(fields(3)) Then
                            newObjSekkeidata.Point3d.X = CDbl(fields(1))
                            newObjSekkeidata.Point3d.Y = CDbl(fields(2))
                            newObjSekkeidata.Point3d.Z = CDbl(fields(3))
                            newObjSekkeidata.flgKijyun = CInt(fields(4))
                            newObjSekkeidata.strInfo = fields(5)
                            lstSekkeiData.Add(newObjSekkeidata)
                        Else
                            Exit Function
                        End If
                    Else
                        Exit Function
                    End If

                End While
            End Using
            If lstSekkeiData.Count <> 0 Then
                ReadSekkeiData = True
            Else
                ReadSekkeiData = False
            End If

        End Function
    End Class

    '機能：定規用のCT番号をDBより取得する
    '(20160714 Tezuka ADD)
    Public Function YCM_ScaleCTNumberGet(ByVal strDataPath As String, ByRef strCtNo() As String) As Integer
        YCM_ScaleCTNumberGet = 0

        'SunpoSetテーブルより定規のターゲットIDを得る
        Dim clsOPe As New CDBOperate
        Dim adoRet As ADODB.Recordset
        Dim iCtNoId(100) As Integer
        Dim iCnt As Integer = 0
        m_strDataBasePath = strDataPath
        If clsOPe.ConnectDB(m_strDataBasePath) = False Then
            MsgBox("DB OpenError!" & m_strDataBasePath)
            Exit Function
        End If
        Dim strSQL As String = "SELECT CT_ID1, CT_ID2 FROM SunpoSet WHERE CT_ID1>0 AND CT_ID2>0"
        adoRet = clsOPe.CreateRecordset(strSQL)
        If adoRet.RecordCount > 0 Then

            Do Until adoRet.EOF
                iCnt += 1
                strCtNo(iCnt) = "CT" & adoRet("CT_ID1").Value.ToString
                iCnt += 1
                strCtNo(iCnt) = "CT" & adoRet("CT_ID2").Value.ToString
                adoRet.MoveNext()
            Loop
        End If
        clsOPe.DisConnectDB()

        YCM_ScaleCTNumberGet = iCnt
    End Function

    '20170330 baluu add start
    Public Sub correspond_3d_3d_weight(ByVal hv_PX As HTuple, ByVal hv_PY As HTuple, _
        ByVal hv_PZ As HTuple, ByVal hv_QX As HTuple, ByVal hv_QY As HTuple, ByVal hv_QZ As HTuple, _
        ByVal hv_WX As HTuple, ByVal hv_WY As HTuple, ByVal hv_WZ As HTuple, ByRef hv_HomMat3D As HTuple)

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
        If BTuple.TupleLess(hv_Value, 0).I Then
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




    '20170330 baluu add end

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
        My.Computer.FileSystem.CopyFile(TimeMonLogPath, TimeMonLogPath & Now.ToFileTime())
        My.Computer.FileSystem.DeleteFile(TimeMonLogPath)
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


    'susano commit test 20170605

End Module

Module HDevelopExportReduceObjectModel3dByView

    Dim gIsSinglePose As HTuple
    Dim gInfoDecor As HTuple
    Dim gInfoPos As HTuple
    Dim gTitlePos As HTuple
    Dim gTitleDecor As HTuple
    Dim gAlphaDeselected As HTuple
    Dim gTerminationButtonLabel As HTuple
    Dim gDispObjOffset As HTuple
    Dim gLabelsDecor As HTuple
    Dim gUsesOpenGL As HTuple


    Public Sub HDevelopStop()
    End Sub

    Function ExpGetGlobalVar_gIsSinglePose() As HTuple
        Return gIsSinglePose
    End Function
    Sub ExpSetGlobalVar_gIsSinglePose(ByRef val As HTuple)
        gIsSinglePose = val
    End Sub

    Function ExpGetGlobalVar_gInfoDecor() As HTuple
        Return gInfoDecor
    End Function
    Sub ExpSetGlobalVar_gInfoDecor(ByRef val As HTuple)
        gInfoDecor = val
    End Sub

    Function ExpGetGlobalVar_gInfoPos() As HTuple
        Return gInfoPos
    End Function
    Sub ExpSetGlobalVar_gInfoPos(ByRef val As HTuple)
        gInfoPos = val
    End Sub

    Function ExpGetGlobalVar_gTitlePos() As HTuple
        Return gTitlePos
    End Function
    Sub ExpSetGlobalVar_gTitlePos(ByRef val As HTuple)
        gTitlePos = val
    End Sub

    Function ExpGetGlobalVar_gTitleDecor() As HTuple
        Return gTitleDecor
    End Function
    Sub ExpSetGlobalVar_gTitleDecor(ByRef val As HTuple)
        gTitleDecor = val
    End Sub

    Function ExpGetGlobalVar_gAlphaDeselected() As HTuple
        Return gAlphaDeselected
    End Function
    Sub ExpSetGlobalVar_gAlphaDeselected(ByRef val As HTuple)
        gAlphaDeselected = val
    End Sub

    Function ExpGetGlobalVar_gTerminationButtonLabel() As HTuple
        Return gTerminationButtonLabel
    End Function
    Sub ExpSetGlobalVar_gTerminationButtonLabel(ByRef val As HTuple)
        gTerminationButtonLabel = val
    End Sub

    Function ExpGetGlobalVar_gDispObjOffset() As HTuple
        Return gDispObjOffset
    End Function
    Sub ExpSetGlobalVar_gDispObjOffset(ByRef val As HTuple)
        gDispObjOffset = val
    End Sub

    Function ExpGetGlobalVar_gLabelsDecor() As HTuple
        Return gLabelsDecor
    End Function
    Sub ExpSetGlobalVar_gLabelsDecor(ByRef val As HTuple)
        gLabelsDecor = val
    End Sub

    Function ExpGetGlobalVar_gUsesOpenGL() As HTuple
        Return gUsesOpenGL
    End Function
    Sub ExpSetGlobalVar_gUsesOpenGL(ByRef val As HTuple)
        gUsesOpenGL = val
    End Sub

    ' Procedures 
    ' Chapter: Develop
    ' Short Description: Switch dev_update_pc, dev_update_var and dev_update_window to 'off'. 
    Public Sub dev_update_off()
        ' Initialize local and output iconic variables 

        'This procedure sets different update settings to 'off'.
        'This is useful to get the best performance and reduce overhead.
        '
        ' dev_update_pc(...); only in hdevelop
        ' dev_update_var(...); only in hdevelop
        ' dev_update_window(...); only in hdevelop

        Exit Sub
    End Sub

    ' Chapter: Graphics / Text
    ' Short Description: This procedure writes a text message. 
    Public Sub disp_message(ByVal hv_WindowHandle As HTuple, ByVal hv_String As HTuple, _
        ByVal hv_CoordSystem As HTuple, ByVal hv_Row As HTuple, ByVal hv_Column As HTuple, _
        ByVal hv_Color As HTuple, ByVal hv_Box As HTuple)


        ' Local control variables 
        Dim hv_Red As HTuple = New HTuple, hv_Green As HTuple = New HTuple
        Dim hv_Blue As HTuple = New HTuple, hv_Row1Part As HTuple = New HTuple
        Dim hv_Column1Part As HTuple = New HTuple, hv_Row2Part As HTuple = New HTuple
        Dim hv_Column2Part As HTuple = New HTuple, hv_RowWin As HTuple = New HTuple
        Dim hv_ColumnWin As HTuple = New HTuple, hv_WidthWin As HTuple = New HTuple
        Dim hv_HeightWin As HTuple = New HTuple, hv_MaxAscent As HTuple = New HTuple
        Dim hv_MaxDescent As HTuple = New HTuple, hv_MaxWidth As HTuple = New HTuple
        Dim hv_MaxHeight As HTuple = New HTuple, hv_R1 As HTuple = New HTuple
        Dim hv_C1 As HTuple = New HTuple, hv_FactorRow As HTuple = New HTuple
        Dim hv_FactorColumn As HTuple = New HTuple, hv_Width As HTuple = New HTuple
        Dim hv_Index As HTuple = New HTuple, hv_Ascent As HTuple = New HTuple
        Dim hv_Descent As HTuple = New HTuple, hv_W As HTuple = New HTuple
        Dim hv_H As HTuple = New HTuple, hv_FrameHeight As HTuple = New HTuple
        Dim hv_FrameWidth As HTuple = New HTuple, hv_R2 As HTuple = New HTuple
        Dim hv_C2 As HTuple = New HTuple, hv_DrawMode As HTuple = New HTuple
        Dim hv_Exception As HTuple = New HTuple, hv_CurrentColor As HTuple = New HTuple

        Dim hv_Color_COPY_INP_TMP As HTuple
        hv_Color_COPY_INP_TMP = hv_Color.Clone()
        Dim hv_Column_COPY_INP_TMP As HTuple
        hv_Column_COPY_INP_TMP = hv_Column.Clone()
        Dim hv_Row_COPY_INP_TMP As HTuple
        hv_Row_COPY_INP_TMP = hv_Row.Clone()
        Dim hv_String_COPY_INP_TMP As HTuple
        hv_String_COPY_INP_TMP = hv_String.Clone()

        ' Initialize local and output iconic variables 

        'This procedure displays text in a graphics window.
        '
        'Input parameters:
        'WindowHandle: The WindowHandle of the graphics window, where
        '   the message should be displayed
        'String: A tuple of strings containing the text message to be displayed
        'CoordSystem: If set to 'window', the text position is given
        '   with respect to the window coordinate system.
        '   If set to 'image', image coordinates are used.
        '   (This may be useful in zoomed images.)
        'Row: The row coordinate of the desired text position
        '   If set to -1, a default value of 12 is used.
        'Column: The column coordinate of the desired text position
        '   If set to -1, a default value of 12 is used.
        'Color: defines the color of the text as string.
        '   If set to [], '' or 'auto' the currently set color is used.
        '   If a tuple of strings is passed, the colors are used cyclically
        '   for each new textline.
        'Box: If set to 'true', the text is written within a white box.
        '
        'prepare window
        HOperatorSet.GetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue)
        HOperatorSet.GetPart(hv_WindowHandle, hv_Row1Part, hv_Column1Part, hv_Row2Part, _
            hv_Column2Part)
        HOperatorSet.GetWindowExtents(hv_WindowHandle, hv_RowWin, hv_ColumnWin, hv_WidthWin, _
            hv_HeightWin)
        HOperatorSet.SetPart(hv_WindowHandle, New HTuple(0), New HTuple(0), hv_HeightWin.TupleSub( _
            New HTuple(1)), hv_WidthWin.TupleSub(New HTuple(1)))
        '
        'default settings
        If New HTuple(hv_Row_COPY_INP_TMP.TupleEqual(New HTuple(-1))).I() Then
            hv_Row_COPY_INP_TMP = New HTuple(12)
        End If
        If New HTuple(hv_Column_COPY_INP_TMP.TupleEqual(New HTuple(-1))).I() Then
            hv_Column_COPY_INP_TMP = New HTuple(12)
        End If
        If New HTuple(hv_Color_COPY_INP_TMP.TupleEqual(New HTuple())).I() Then
            hv_Color_COPY_INP_TMP = New HTuple("")
        End If
        '
        hv_String_COPY_INP_TMP = (((((New HTuple("")).TupleAdd(hv_String_COPY_INP_TMP))).TupleAdd( _
            New HTuple("")))).TupleSplit(New HTuple("" + Chr(10)))
        '
        'Estimate extentions of text depending on font size.
        HOperatorSet.GetFontExtents(hv_WindowHandle, hv_MaxAscent, hv_MaxDescent, hv_MaxWidth, _
            hv_MaxHeight)
        If New HTuple(hv_CoordSystem.TupleEqual(New HTuple("window"))).I() Then
            hv_R1 = hv_Row_COPY_INP_TMP.Clone()
            hv_C1 = hv_Column_COPY_INP_TMP.Clone()
        Else
            'transform image to window coordinates
            hv_FactorRow = (((New HTuple(1.0)).TupleMult(hv_HeightWin))).TupleDiv(((hv_Row2Part.TupleSub( _
                hv_Row1Part))).TupleAdd(New HTuple(1)))
            hv_FactorColumn = (((New HTuple(1.0)).TupleMult(hv_WidthWin))).TupleDiv(((hv_Column2Part.TupleSub( _
                hv_Column1Part))).TupleAdd(New HTuple(1)))
            hv_R1 = ((((hv_Row_COPY_INP_TMP.TupleSub(hv_Row1Part))).TupleAdd(New HTuple(0.5)))).TupleMult( _
                hv_FactorRow)
            hv_C1 = ((((hv_Column_COPY_INP_TMP.TupleSub(hv_Column1Part))).TupleAdd(New HTuple(0.5)))).TupleMult( _
                hv_FactorColumn)
        End If
        '
        'display text box depending on text size
        If New HTuple(hv_Box.TupleEqual(New HTuple("true"))).I() Then
            'calculate box extents
            hv_String_COPY_INP_TMP = (((New HTuple(" ")).TupleAdd(hv_String_COPY_INP_TMP))).TupleAdd( _
                New HTuple(" "))
            hv_Width = New HTuple()
            For hv_Index = (New HTuple(0)) To ((New HTuple( _
                hv_String_COPY_INP_TMP.TupleLength()))).TupleSub(New HTuple(1)) Step (New HTuple(1))
                HOperatorSet.GetStringExtents(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect( _
                    hv_Index), hv_Ascent, hv_Descent, hv_W, hv_H)
                hv_Width = hv_Width.TupleConcat(hv_W)
#If USE_DO_EVENTS Then
        ' Please note: The call of DoEvents() is only a hack to
        ' enable VB to react on events. Please change the code
        ' so that it can handle events in a standard way.
        System.Windows.Forms.Application.DoEvents()
#End If
            Next
            hv_FrameHeight = hv_MaxHeight.TupleMult(New HTuple(hv_String_COPY_INP_TMP.TupleLength( _
                )))
            hv_FrameWidth = (((New HTuple(0)).TupleConcat(hv_Width))).TupleMax()
            hv_R2 = hv_R1.TupleAdd(hv_FrameHeight)
            hv_C2 = hv_C1.TupleAdd(hv_FrameWidth)
            'display rectangles
            HOperatorSet.GetDraw(hv_WindowHandle, hv_DrawMode)
            HOperatorSet.SetDraw(hv_WindowHandle, New HTuple("fill"))
            HOperatorSet.SetColor(hv_WindowHandle, New HTuple("light gray"))
            HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1.TupleAdd(New HTuple(3)), _
                hv_C1.TupleAdd(New HTuple(3)), hv_R2.TupleAdd(New HTuple(3)), hv_C2.TupleAdd( _
                New HTuple(3)))
            HOperatorSet.SetColor(hv_WindowHandle, New HTuple("white"))
            HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1, hv_C1, hv_R2, hv_C2)
            HOperatorSet.SetDraw(hv_WindowHandle, hv_DrawMode)
        ElseIf New HTuple(hv_Box.TupleNotEqual(New HTuple("false"))).I() Then
            hv_Exception = New HTuple("Wrong value of control parameter Box")
            Throw New HalconException(hv_Exception)
        End If
        'Write text.
        For hv_Index = (New HTuple(0)) To ( _
            (New HTuple(hv_String_COPY_INP_TMP.TupleLength()))).TupleSub(New HTuple(1)) Step (New HTuple(1))
            hv_CurrentColor = hv_Color_COPY_INP_TMP.TupleSelect(hv_Index.TupleMod(New HTuple( _
                hv_Color_COPY_INP_TMP.TupleLength())))
            If ((New HTuple(hv_CurrentColor.TupleNotEqual(New HTuple(""))))).TupleAnd(New HTuple( _
                hv_CurrentColor.TupleNotEqual(New HTuple("auto")))).I() Then
                HOperatorSet.SetColor(hv_WindowHandle, hv_CurrentColor)
            Else
                HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue)
            End If
            hv_Row_COPY_INP_TMP = hv_R1.TupleAdd(hv_MaxHeight.TupleMult(hv_Index))
            HOperatorSet.SetTposition(hv_WindowHandle, hv_Row_COPY_INP_TMP, hv_C1)
            HOperatorSet.WriteString(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect( _
                hv_Index))
#If USE_DO_EVENTS Then
      ' Please note: The call of DoEvents() is only a hack to
      ' enable VB to react on events. Please change the code
      ' so that it can handle events in a standard way.
      System.Windows.Forms.Application.DoEvents()
#End If
        Next
        'reset changed window settings
        HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue)
        HOperatorSet.SetPart(hv_WindowHandle, hv_Row1Part, hv_Column1Part, hv_Row2Part, _
            hv_Column2Part)

        Exit Sub
    End Sub

    ' Chapter: Graphics / Text
    ' Short Description: Set font independent of OS 
    Public Sub set_display_font(ByVal hv_WindowHandle As HTuple, ByVal hv_Size As HTuple, _
        ByVal hv_Font As HTuple, ByVal hv_Bold As HTuple, ByVal hv_Slant As HTuple)


        ' Local control variables 
        Dim hv_OS As HTuple = New HTuple, hv_Exception As HTuple = New HTuple
        Dim hv_BoldString As HTuple = New HTuple, hv_SlantString As HTuple = New HTuple
        Dim hv_AllowedFontSizes As HTuple = New HTuple, hv_Distances As HTuple = New HTuple
        Dim hv_Indices As HTuple = New HTuple, hv_Fonts As HTuple = New HTuple
        Dim hv_FontSelRegexp As HTuple = New HTuple, hv_FontsCourier As HTuple = New HTuple

        Dim hv_Bold_COPY_INP_TMP As HTuple
        hv_Bold_COPY_INP_TMP = hv_Bold.Clone()
        Dim hv_Font_COPY_INP_TMP As HTuple
        hv_Font_COPY_INP_TMP = hv_Font.Clone()
        Dim hv_Size_COPY_INP_TMP As HTuple
        hv_Size_COPY_INP_TMP = hv_Size.Clone()
        Dim hv_Slant_COPY_INP_TMP As HTuple
        hv_Slant_COPY_INP_TMP = hv_Slant.Clone()

        ' Initialize local and output iconic variables 

        'This procedure sets the text font of the current window with
        'the specified attributes.
        'It is assumed that following fonts are installed on the system:
        'Windows: Courier New, Arial Times New Roman
        'Mac OS X: CourierNewPS, Arial, TimesNewRomanPS
        'Linux: courier, helvetica, times
        'Because fonts are displayed smaller on Linux than on Windows,
        'a scaling factor of 1.25 is used the get comparable results.
        'For Linux, only a limited number of font sizes is supported,
        'to get comparable results, it is recommended to use one of the
        'following sizes: 9, 11, 14, 16, 20, 27
        '(which will be mapped internally on Linux systems to 11, 14, 17, 20, 25, 34)
        '
        'Input parameters:
        'WindowHandle: The graphics window for which the font will be set
        'Size: The font size. If Size=-1, the default of 16 is used.
        'Bold: If set to 'true', a bold font is used
        'Slant: If set to 'true', a slanted font is used
        '
        HOperatorSet.GetSystem(New HTuple("operating_system"), hv_OS)
        ' dev_get_preferences(...); only in hdevelop
        ' dev_set_preferences(...); only in hdevelop
        If ((New HTuple(hv_Size_COPY_INP_TMP.TupleEqual(New HTuple())))).TupleOr(New HTuple( _
            hv_Size_COPY_INP_TMP.TupleEqual(New HTuple(-1)))).I() Then
            hv_Size_COPY_INP_TMP = New HTuple(16)
        End If
        If New HTuple(((hv_OS.TupleSubstr(New HTuple(0), New HTuple(2)))).TupleEqual(New HTuple("Win"))).I() Then
            'Set font on Windows systems
            If ((((New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("mono"))))).TupleOr( _
                New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("Courier")))))).TupleOr( _
                New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("courier")))).I() Then
                hv_Font_COPY_INP_TMP = New HTuple("Courier New")
            ElseIf New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("sans"))).I() Then
                hv_Font_COPY_INP_TMP = New HTuple("Arial")
            ElseIf New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("serif"))).I() Then
                hv_Font_COPY_INP_TMP = New HTuple("Times New Roman")
            End If
            If New HTuple(hv_Bold_COPY_INP_TMP.TupleEqual(New HTuple("true"))).I() Then
                hv_Bold_COPY_INP_TMP = New HTuple(1)
            ElseIf New HTuple(hv_Bold_COPY_INP_TMP.TupleEqual(New HTuple("false"))).I() Then
                hv_Bold_COPY_INP_TMP = New HTuple(0)
            Else
                hv_Exception = New HTuple("Wrong value of control parameter Bold")
                Throw New HalconException(hv_Exception)
            End If
            If New HTuple(hv_Slant_COPY_INP_TMP.TupleEqual(New HTuple("true"))).I() Then
                hv_Slant_COPY_INP_TMP = New HTuple(1)
            ElseIf New HTuple(hv_Slant_COPY_INP_TMP.TupleEqual(New HTuple("false"))).I() Then
                hv_Slant_COPY_INP_TMP = New HTuple(0)
            Else
                hv_Exception = New HTuple("Wrong value of control parameter Slant")
                Throw New HalconException(hv_Exception)
            End If
            Try
                HOperatorSet.SetFont(hv_WindowHandle, (((((((((((((((New HTuple("-")).TupleAdd( _
                    hv_Font_COPY_INP_TMP))).TupleAdd(New HTuple("-")))).TupleAdd(hv_Size_COPY_INP_TMP))).TupleAdd( _
                    New HTuple("-*-")))).TupleAdd(hv_Slant_COPY_INP_TMP))).TupleAdd(New HTuple("-*-*-")))).TupleAdd( _
                    hv_Bold_COPY_INP_TMP))).TupleAdd(New HTuple("-")))
                ' catch (Exception) 
            Catch HDevExpDefaultException1 As HalconException
                HDevExpDefaultException1.ToHTuple(hv_Exception)
                'throw (Exception)
            End Try
        ElseIf New HTuple(((hv_OS.TupleSubstr(New HTuple(0), New HTuple(2)))).TupleEqual( _
            New HTuple("Dar"))).I() Then
            'Set font on Mac OS X systems
            If New HTuple(hv_Bold_COPY_INP_TMP.TupleEqual(New HTuple("true"))).I() Then
                hv_BoldString = New HTuple("Bold")
            ElseIf New HTuple(hv_Bold_COPY_INP_TMP.TupleEqual(New HTuple("false"))).I() Then
                hv_BoldString = New HTuple("")
            Else
                hv_Exception = New HTuple("Wrong value of control parameter Bold")
                Throw New HalconException(hv_Exception)
            End If
            If New HTuple(hv_Slant_COPY_INP_TMP.TupleEqual(New HTuple("true"))).I() Then
                hv_SlantString = New HTuple("Italic")
            ElseIf New HTuple(hv_Slant_COPY_INP_TMP.TupleEqual(New HTuple("false"))).I() Then
                hv_SlantString = New HTuple("")
            Else
                hv_Exception = New HTuple("Wrong value of control parameter Slant")
                Throw New HalconException(hv_Exception)
            End If
            If ((((New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("mono"))))).TupleOr( _
                New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("Courier")))))).TupleOr( _
                New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("courier")))).I() Then
                hv_Font_COPY_INP_TMP = New HTuple("CourierNewPS")
            ElseIf New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("sans"))).I() Then
                hv_Font_COPY_INP_TMP = New HTuple("Arial")
            ElseIf New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("serif"))).I() Then
                hv_Font_COPY_INP_TMP = New HTuple("TimesNewRomanPS")
            End If
            If ((New HTuple(hv_Bold_COPY_INP_TMP.TupleEqual(New HTuple("true"))))).TupleOr( _
                New HTuple(hv_Slant_COPY_INP_TMP.TupleEqual(New HTuple("true")))).I() Then
                hv_Font_COPY_INP_TMP = ((((hv_Font_COPY_INP_TMP.TupleAdd(New HTuple("-")))).TupleAdd( _
                    hv_BoldString))).TupleAdd(hv_SlantString)
            End If
            hv_Font_COPY_INP_TMP = hv_Font_COPY_INP_TMP.TupleAdd(New HTuple("MT"))
            Try
                HOperatorSet.SetFont(hv_WindowHandle, ((hv_Font_COPY_INP_TMP.TupleAdd(New HTuple("-")))).TupleAdd( _
                    hv_Size_COPY_INP_TMP))
                ' catch (Exception) 
            Catch HDevExpDefaultException1 As HalconException
                HDevExpDefaultException1.ToHTuple(hv_Exception)
                'throw (Exception)
            End Try
        Else
            'Set font for UNIX systems
            hv_Size_COPY_INP_TMP = hv_Size_COPY_INP_TMP.TupleMult(New HTuple(1.25))
            hv_AllowedFontSizes = (((((New HTuple(11)).TupleConcat(14)).TupleConcat(17)).TupleConcat( _
                20)).TupleConcat(25)).TupleConcat(34)
            If New HTuple(((hv_AllowedFontSizes.TupleFind(hv_Size_COPY_INP_TMP))).TupleEqual( _
                New HTuple(-1))).I() Then
                hv_Distances = ((hv_AllowedFontSizes.TupleSub(hv_Size_COPY_INP_TMP))).TupleAbs( _
                    )
                HOperatorSet.TupleSortIndex(hv_Distances, hv_Indices)
                hv_Size_COPY_INP_TMP = hv_AllowedFontSizes.TupleSelect(hv_Indices.TupleSelect( _
                    New HTuple(0)))
            End If
            If ((New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("mono"))))).TupleOr( _
                New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("Courier")))).I() Then
                hv_Font_COPY_INP_TMP = New HTuple("courier")
            ElseIf New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("sans"))).I() Then
                hv_Font_COPY_INP_TMP = New HTuple("helvetica")
            ElseIf New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("serif"))).I() Then
                hv_Font_COPY_INP_TMP = New HTuple("times")
            End If
            If New HTuple(hv_Bold_COPY_INP_TMP.TupleEqual(New HTuple("true"))).I() Then
                hv_Bold_COPY_INP_TMP = New HTuple("bold")
            ElseIf New HTuple(hv_Bold_COPY_INP_TMP.TupleEqual(New HTuple("false"))).I() Then
                hv_Bold_COPY_INP_TMP = New HTuple("medium")
            Else
                hv_Exception = New HTuple("Wrong value of control parameter Bold")
                Throw New HalconException(hv_Exception)
            End If
            If New HTuple(hv_Slant_COPY_INP_TMP.TupleEqual(New HTuple("true"))).I() Then
                If New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("times"))).I() Then
                    hv_Slant_COPY_INP_TMP = New HTuple("i")
                Else
                    hv_Slant_COPY_INP_TMP = New HTuple("o")
                End If
            ElseIf New HTuple(hv_Slant_COPY_INP_TMP.TupleEqual(New HTuple("false"))).I() Then
                hv_Slant_COPY_INP_TMP = New HTuple("r")
            Else
                hv_Exception = New HTuple("Wrong value of control parameter Slant")
                Throw New HalconException(hv_Exception)
            End If
            Try
                HOperatorSet.SetFont(hv_WindowHandle, (((((((((((((((New HTuple("-adobe-")).TupleAdd( _
                    hv_Font_COPY_INP_TMP))).TupleAdd(New HTuple("-")))).TupleAdd(hv_Bold_COPY_INP_TMP))).TupleAdd( _
                    New HTuple("-")))).TupleAdd(hv_Slant_COPY_INP_TMP))).TupleAdd(New HTuple("-normal-*-")))).TupleAdd( _
                    hv_Size_COPY_INP_TMP))).TupleAdd(New HTuple("-*-*-*-*-*-*-*")))
                ' catch (Exception) 
            Catch HDevExpDefaultException1 As HalconException
                HDevExpDefaultException1.ToHTuple(hv_Exception)
                If ((New HTuple(((hv_OS.TupleSubstr(New HTuple(0), New HTuple(4)))).TupleEqual( _
                    New HTuple("Linux"))))).TupleAnd(New HTuple(hv_Font_COPY_INP_TMP.TupleEqual( _
                    New HTuple("courier")))).I() Then
                    HOperatorSet.QueryFont(hv_WindowHandle, hv_Fonts)
                    hv_FontSelRegexp = (((((New HTuple("^-[^-]*-[^-]*[Cc]ourier[^-]*-")).TupleAdd( _
                        hv_Bold_COPY_INP_TMP))).TupleAdd(New HTuple("-")))).TupleAdd(hv_Slant_COPY_INP_TMP)
                    hv_FontsCourier = ((hv_Fonts.TupleRegexpSelect(hv_FontSelRegexp))).TupleRegexpMatch( _
                        hv_FontSelRegexp)
                    If New HTuple(((New HTuple(hv_FontsCourier.TupleLength()))).TupleEqual( _
                        New HTuple(0))).I() Then
                        hv_Exception = New HTuple("Wrong font name")
                        'throw (Exception)
                    Else
                        Try
                            HOperatorSet.SetFont(hv_WindowHandle, ((((((hv_FontsCourier.TupleSelect( _
                                New HTuple(0)))).TupleAdd(New HTuple("-normal-*-")))).TupleAdd( _
                                hv_Size_COPY_INP_TMP))).TupleAdd(New HTuple("-*-*-*-*-*-*-*")))
                            ' catch (Exception) 
                        Catch HDevExpDefaultException2 As HalconException
                            HDevExpDefaultException2.ToHTuple(hv_Exception)
                            'throw (Exception)
                        End Try
                    End If
                End If
                'throw (Exception)
            End Try
        End If
        ' dev_set_preferences(...); only in hdevelop

        Exit Sub
    End Sub

    ' Chapter: Graphics / Output
    ' Short Description: Reflect the pose change that was introduced by the user by moving the mouse 
    Public Sub analyze_graph_event(ByVal ho_BackgroundImage As HObject, ByVal hv_MouseMapping As HTuple, _
        ByVal hv_Button As HTuple, ByVal hv_Row As HTuple, ByVal hv_Column As HTuple, _
        ByVal hv_WindowHandle As HTuple, ByVal hv_WindowHandleBuffer As HTuple, ByVal hv_VirtualTrackball As HTuple, _
        ByVal hv_TrackballSize As HTuple, ByVal hv_SelectedObjectIn As HTuple, ByVal hv_ObjectModel3DID As HTuple, _
        ByVal hv_CamParam As HTuple, ByVal hv_Labels As HTuple, ByVal hv_Title As HTuple, _
        ByVal hv_Information As HTuple, ByVal hv_GenParamName As HTuple, ByVal hv_GenParamValue As HTuple, _
        ByVal hv_PosesIn As HTuple, ByVal hv_ButtonHoldIn As HTuple, ByVal hv_TBCenter As HTuple, _
        ByVal hv_TBSize As HTuple, ByVal hv_WindowCenteredRotationlIn As HTuple, ByVal hv_MaxNumModels As HTuple, _
        ByRef hv_PosesOut As HTuple, ByRef hv_SelectedObjectOut As HTuple, ByRef hv_ButtonHoldOut As HTuple, _
        ByRef hv_WindowCenteredRotationOut As HTuple)



        ' Local iconic variables 
        Dim ho_ImageDump As HObject = Nothing


        ' Local control variables 
        Dim ExpTmpLocalVar_gIsSinglePose As HTuple = New HTuple
        Dim hv_VisualizeTB As HTuple = New HTuple, hv_InvLog2 As HTuple = New HTuple
        Dim hv_Seconds As HTuple = New HTuple, hv_Indices As HTuple = New HTuple
        Dim hv_OpenGlEnabled As HTuple = New HTuple, hv_ModelIndex As HTuple = New HTuple
        Dim hv_Exception1 As HTuple = New HTuple, hv_HomMat3DIdentity As HTuple = New HTuple
        Dim hv_NumModels As HTuple = New HTuple, hv_Width As HTuple = New HTuple
        Dim hv_Height As HTuple = New HTuple, hv_MinImageSize As HTuple = New HTuple
        Dim hv_TrackballRadiusPixel As HTuple = New HTuple
        Dim hv_TrackballCenterRow As HTuple = New HTuple, hv_TrackballCenterCol As HTuple = New HTuple
        Dim hv_NumChannels As HTuple = New HTuple, hv_ColorImage As HTuple = New HTuple
        Dim hv_BAnd As HTuple = New HTuple, hv_SensFactor As HTuple = New HTuple
        Dim hv_IsButtonTrans As HTuple = New HTuple, hv_IsButtonRot As HTuple = New HTuple
        Dim hv_IsButtonDist As HTuple = New HTuple, hv_MRow1 As HTuple = New HTuple
        Dim hv_MCol1 As HTuple = New HTuple, hv_ButtonLoop As HTuple = New HTuple
        Dim hv_MRow2 As HTuple = New HTuple, hv_MCol2 As HTuple = New HTuple
        Dim hv_PX As HTuple = New HTuple, hv_PY As HTuple = New HTuple
        Dim hv_PZ As HTuple = New HTuple, hv_QX1 As HTuple = New HTuple
        Dim hv_QY1 As HTuple = New HTuple, hv_QZ1 As HTuple = New HTuple
        Dim hv_QX2 As HTuple = New HTuple, hv_QY2 As HTuple = New HTuple
        Dim hv_QZ2 As HTuple = New HTuple, hv_Len As HTuple = New HTuple
        Dim hv_Dist As HTuple = New HTuple, hv_Translate As HTuple = New HTuple
        Dim hv_Index As HTuple = New HTuple, hv_PoseIn As HTuple = New HTuple
        Dim hv_HomMat3DIn As HTuple = New HTuple, hv_HomMat3DOut As HTuple = New HTuple
        Dim hv_PoseOut As HTuple = New HTuple, hv_Sequence As HTuple = New HTuple
        Dim hv_Mod As HTuple = New HTuple, hv_SequenceReal As HTuple = New HTuple
        Dim hv_Sequence2Int As HTuple = New HTuple, hv_Selected As HTuple = New HTuple
        Dim hv_InvSelected As HTuple = New HTuple, hv_Exception As HTuple = New HTuple
        Dim hv_DRow As HTuple = New HTuple, hv_TranslateZ As HTuple = New HTuple
        Dim hv_ChangePoseOf As HTuple = New HTuple, hv_TrackballCenterCamOut As HTuple = New HTuple
        Dim hv_MX1 As HTuple = New HTuple, hv_MY1 As HTuple = New HTuple
        Dim hv_MX2 As HTuple = New HTuple, hv_MY2 As HTuple = New HTuple
        Dim hv_RelQuaternion As HTuple = New HTuple, hv_HomMat3DRotRel As HTuple = New HTuple
        Dim hv_HomMat3DInTmp1 As HTuple = New HTuple, hv_HomMat3DInTmp As HTuple = New HTuple
        Dim hv_PosesOut2 As HTuple = New HTuple

        Dim hv_Column_COPY_INP_TMP As HTuple
        hv_Column_COPY_INP_TMP = hv_Column.Clone()
        Dim hv_GenParamName_COPY_INP_TMP As HTuple
        hv_GenParamName_COPY_INP_TMP = hv_GenParamName.Clone()
        Dim hv_GenParamValue_COPY_INP_TMP As HTuple
        hv_GenParamValue_COPY_INP_TMP = hv_GenParamValue.Clone()
        Dim hv_PosesIn_COPY_INP_TMP As HTuple
        hv_PosesIn_COPY_INP_TMP = hv_PosesIn.Clone()
        Dim hv_Row_COPY_INP_TMP As HTuple
        hv_Row_COPY_INP_TMP = hv_Row.Clone()
        Dim hv_TBCenter_COPY_INP_TMP As HTuple
        hv_TBCenter_COPY_INP_TMP = hv_TBCenter.Clone()
        Dim hv_TBSize_COPY_INP_TMP As HTuple
        hv_TBSize_COPY_INP_TMP = hv_TBSize.Clone()

        ' Initialize local and output iconic variables 
        HOperatorSet.GenEmptyObj(ho_ImageDump)

        Try
            'This procedure reflects
            '- the pose change that was introduced by the user by
            '  moving the mouse
            '- the selection of a single object
            '
            'global tuple gIsSinglePose
            '
            hv_ButtonHoldOut = hv_ButtonHoldIn.Clone()
            hv_PosesOut = hv_PosesIn_COPY_INP_TMP.Clone()
            hv_SelectedObjectOut = hv_SelectedObjectIn.Clone()
            hv_WindowCenteredRotationOut = hv_WindowCenteredRotationlIn.Clone()
            hv_VisualizeTB = New HTuple(((hv_SelectedObjectOut.TupleMax())).TupleNotEqual( _
                New HTuple(0)))
            hv_InvLog2 = (New HTuple(1.0)).TupleDiv((New HTuple(2)).TupleLog())
            '
            If New HTuple(hv_Button.TupleEqual(hv_MouseMapping.TupleSelect(New HTuple(6)))).I() Then
                If hv_ButtonHoldOut.I() Then
                    ho_ImageDump.Dispose()

                    Exit Sub
                End If
                'Ctrl (16) + Alt (32) + left mouse button (1) => Toggle rotation center position
                'If WindowCenteredRotation is not 1, set it to 1, otherwise, set it to 2
                HOperatorSet.CountSeconds(hv_Seconds)
                If New HTuple(hv_WindowCenteredRotationOut.TupleEqual(New HTuple(1))).I() Then
                    hv_WindowCenteredRotationOut = New HTuple(2)
                Else
                    hv_WindowCenteredRotationOut = New HTuple(1)
                End If
                hv_ButtonHoldOut = New HTuple(1)
                ho_ImageDump.Dispose()

                Exit Sub
            End If
            If ((New HTuple(hv_Button.TupleEqual(hv_MouseMapping.TupleSelect(New HTuple(5)))))).TupleAnd( _
                New HTuple(((New HTuple(hv_ObjectModel3DID.TupleLength()))).TupleLessEqual( _
                hv_MaxNumModels))).I() Then
                If hv_ButtonHoldOut.I() Then
                    ho_ImageDump.Dispose()

                    Exit Sub
                End If
                'Ctrl (16) + left mouse button (1) => Select an object
                Try
                    hv_Indices = hv_GenParamName_COPY_INP_TMP.TupleFind(New HTuple("opengl"))
                    If ((New HTuple(hv_Indices.TupleNotEqual(New HTuple(-1))))).TupleAnd(New HTuple( _
                        hv_Indices.TupleNotEqual(New HTuple()))).I() Then
                        hv_OpenGlEnabled = hv_GenParamValue_COPY_INP_TMP.TupleSelect(hv_Indices.TupleSelect( _
                            New HTuple(0)))
                        hv_GenParamName_COPY_INP_TMP = hv_GenParamName_COPY_INP_TMP.TupleRemove( _
                            hv_Indices)
                        hv_GenParamValue_COPY_INP_TMP = hv_GenParamValue_COPY_INP_TMP.TupleRemove( _
                            hv_Indices)
                    End If
                    HOperatorSet.DispObjectModel3d(hv_WindowHandleBuffer, hv_ObjectModel3DID, _
                        hv_CamParam, hv_PosesOut, hv_GenParamName_COPY_INP_TMP.TupleConcat( _
                        New HTuple("object_index_persistence")), hv_GenParamValue_COPY_INP_TMP.TupleConcat( _
                        New HTuple("true")))
                    HOperatorSet.GetDispObjectModel3dInfo(hv_WindowHandleBuffer, hv_Row_COPY_INP_TMP, _
                        hv_Column_COPY_INP_TMP, New HTuple("object_index"), hv_ModelIndex)
                    ' catch (Exception1) 
                Catch HDevExpDefaultException1 As HalconException
                    HDevExpDefaultException1.ToHTuple(hv_Exception1)
                    '* NO OpenGL, no selection possible
                    ho_ImageDump.Dispose()

                    Exit Sub
                End Try
                If New HTuple(hv_ModelIndex.TupleEqual(New HTuple(-1))).I() Then
                    'Background click:
                    If New HTuple(((hv_SelectedObjectOut.TupleSum())).TupleEqual(New HTuple( _
                        hv_SelectedObjectOut.TupleLength()))).I() Then
                        'If all objects are already selected, deselect all
                        hv_SelectedObjectOut = HTuple.TupleGenConst(New HTuple(hv_ObjectModel3DID.TupleLength( _
                            )), New HTuple(0))
                    Else
                        'Otherwise select all
                        hv_SelectedObjectOut = HTuple.TupleGenConst(New HTuple(hv_ObjectModel3DID.TupleLength( _
                            )), New HTuple(1))
                    End If
                Else
                    'Object click:
                    If IsNothing(hv_SelectedObjectOut) Then
                        hv_SelectedObjectOut = New HTuple
                    End If
                    hv_SelectedObjectOut(hv_ModelIndex) = ((hv_SelectedObjectOut.TupleSelect( _
                        hv_ModelIndex))).TupleNot()
                End If
                hv_ButtonHoldOut = New HTuple(1)
            Else
                'Change the pose
                HOperatorSet.HomMat3dIdentity(hv_HomMat3DIdentity)
                hv_NumModels = New HTuple(hv_ObjectModel3DID.TupleLength())
                hv_Width = hv_CamParam.TupleSelect(((New HTuple(hv_CamParam.TupleLength()))).TupleSub( _
                    New HTuple(2)))
                hv_Height = hv_CamParam.TupleSelect(((New HTuple(hv_CamParam.TupleLength( _
                    )))).TupleSub(New HTuple(1)))
                hv_MinImageSize = ((hv_Width.TupleConcat(hv_Height))).TupleMin()
                hv_TrackballRadiusPixel = ((hv_TrackballSize.TupleMult(hv_MinImageSize))).TupleDiv( _
                    New HTuple(2.0))
                'Set trackball fixed in the center of the window
                hv_TrackballCenterRow = hv_Height.TupleDiv(New HTuple(2))
                hv_TrackballCenterCol = hv_Width.TupleDiv(New HTuple(2))
                If New HTuple(((New HTuple(hv_ObjectModel3DID.TupleLength()))).TupleLess( _
                    hv_MaxNumModels)).I() Then
                    If New HTuple(hv_WindowCenteredRotationOut.TupleEqual(New HTuple(1))).I() Then
                        get_trackball_center_fixed(hv_SelectedObjectIn, hv_TrackballCenterRow, _
                            hv_TrackballCenterCol, hv_TrackballRadiusPixel, hv_ObjectModel3DID, _
                            hv_PosesIn_COPY_INP_TMP, hv_WindowHandleBuffer, hv_CamParam, hv_GenParamName_COPY_INP_TMP, _
                            hv_GenParamValue_COPY_INP_TMP, hv_TBCenter_COPY_INP_TMP, hv_TBSize_COPY_INP_TMP)
                    Else
                        get_trackball_center(hv_SelectedObjectIn, hv_TrackballRadiusPixel, hv_ObjectModel3DID, _
                            hv_PosesIn_COPY_INP_TMP, hv_TBCenter_COPY_INP_TMP, hv_TBSize_COPY_INP_TMP)
                    End If
                End If
                If ((New HTuple(((hv_SelectedObjectOut.TupleMin())).TupleEqual(New HTuple(0))))).TupleAnd( _
                    New HTuple(((hv_SelectedObjectOut.TupleMax())).TupleEqual(New HTuple(1)))).I() Then
                    'At this point, multiple objects do not necessary have the same
                    'pose any more. Consequently, we have to return a tuple of poses
                    'as output of visualize_object_model_3d
                    ExpTmpLocalVar_gIsSinglePose = New HTuple(0)
                    ExpSetGlobalVar_gIsSinglePose(ExpTmpLocalVar_gIsSinglePose)
                End If
                HOperatorSet.CountChannels(ho_BackgroundImage, hv_NumChannels)
                hv_ColorImage = New HTuple(hv_NumChannels.TupleEqual(New HTuple(3)))
                'Alt (32) => lower sensitivity
                HOperatorSet.TupleRsh(hv_Button, New HTuple(5), hv_BAnd)
                If hv_BAnd.TupleMod(New HTuple(2)).I() Then
                    hv_SensFactor = New HTuple(0.1)
                Else
                    hv_SensFactor = New HTuple(1.0)
                End If
                hv_IsButtonTrans = ((New HTuple(((hv_MouseMapping.TupleSelect(New HTuple(0)))).TupleEqual( _
                    hv_Button)))).TupleOr(New HTuple((((New HTuple(32)).TupleAdd(hv_MouseMapping.TupleSelect( _
                    New HTuple(0))))).TupleEqual(hv_Button)))
                hv_IsButtonRot = ((New HTuple(((hv_MouseMapping.TupleSelect(New HTuple(1)))).TupleEqual( _
                    hv_Button)))).TupleOr(New HTuple((((New HTuple(32)).TupleAdd(hv_MouseMapping.TupleSelect( _
                    New HTuple(1))))).TupleEqual(hv_Button)))
                hv_IsButtonDist = ((((((((((New HTuple(((hv_MouseMapping.TupleSelect(New HTuple(2)))).TupleEqual( _
                    hv_Button)))).TupleOr(New HTuple((((New HTuple(32)).TupleAdd(hv_MouseMapping.TupleSelect( _
                    New HTuple(2))))).TupleEqual(hv_Button))))).TupleOr(New HTuple(((hv_MouseMapping.TupleSelect( _
                    New HTuple(3)))).TupleEqual(hv_Button))))).TupleOr(New HTuple((((New HTuple(32)).TupleAdd( _
                    hv_MouseMapping.TupleSelect(New HTuple(3))))).TupleEqual(hv_Button))))).TupleOr( _
                    New HTuple(((hv_MouseMapping.TupleSelect(New HTuple(4)))).TupleEqual( _
                    hv_Button))))).TupleOr(New HTuple((((New HTuple(32)).TupleAdd(hv_MouseMapping.TupleSelect( _
                    New HTuple(4))))).TupleEqual(hv_Button)))
                If hv_IsButtonTrans.I() Then
                    'Translate in XY-direction
                    hv_MRow1 = hv_Row_COPY_INP_TMP.Clone()
                    hv_MCol1 = hv_Column_COPY_INP_TMP.Clone()
                    Do While hv_IsButtonTrans.I()
                        Try
                            HOperatorSet.GetMpositionSubPix(hv_WindowHandle, hv_Row_COPY_INP_TMP, _
                                hv_Column_COPY_INP_TMP, hv_ButtonLoop)
                            hv_IsButtonTrans = New HTuple(hv_ButtonLoop.TupleEqual(hv_Button))
                            hv_MRow2 = hv_MRow1.TupleAdd(((hv_Row_COPY_INP_TMP.TupleSub(hv_MRow1))).TupleMult( _
                                hv_SensFactor))
                            hv_MCol2 = hv_MCol1.TupleAdd(((hv_Column_COPY_INP_TMP.TupleSub(hv_MCol1))).TupleMult( _
                                hv_SensFactor))
                            HOperatorSet.GetLineOfSight(hv_MRow1, hv_MCol1, hv_CamParam, hv_PX, _
                                hv_PY, hv_PZ, hv_QX1, hv_QY1, hv_QZ1)
                            HOperatorSet.GetLineOfSight(hv_MRow2, hv_MCol2, hv_CamParam, hv_PX, _
                                hv_PY, hv_PZ, hv_QX2, hv_QY2, hv_QZ2)
                            hv_Len = ((((((hv_QX1.TupleMult(hv_QX1))).TupleAdd(hv_QY1.TupleMult( _
                                hv_QY1)))).TupleAdd(hv_QZ1.TupleMult(hv_QZ1)))).TupleSqrt()
                            hv_Dist = ((((((((hv_TBCenter_COPY_INP_TMP.TupleSelect(New HTuple(0)))).TupleMult( _
                                hv_TBCenter_COPY_INP_TMP.TupleSelect(New HTuple(0))))).TupleAdd( _
                                ((hv_TBCenter_COPY_INP_TMP.TupleSelect(New HTuple(1)))).TupleMult( _
                                hv_TBCenter_COPY_INP_TMP.TupleSelect(New HTuple(1)))))).TupleAdd( _
                                ((hv_TBCenter_COPY_INP_TMP.TupleSelect(New HTuple(2)))).TupleMult( _
                                hv_TBCenter_COPY_INP_TMP.TupleSelect(New HTuple(2)))))).TupleSqrt( _
                                )
                            hv_Translate = ((((((((hv_QX2.TupleSub(hv_QX1))).TupleConcat(hv_QY2.TupleSub( _
                                hv_QY1)))).TupleConcat(hv_QZ2.TupleSub(hv_QZ1)))).TupleMult(hv_Dist))).TupleDiv( _
                                hv_Len)
                            hv_PosesOut = New HTuple()
                            If New HTuple(hv_NumModels.TupleLessEqual(hv_MaxNumModels)).I() Then
                                For hv_Index = (New HTuple(0)) To _
                                    hv_NumModels.TupleSub(New HTuple(1)) Step (New HTuple(1))
                                    hv_PoseIn = hv_PosesIn_COPY_INP_TMP.TupleSelectRange(hv_Index.TupleMult( _
                                        New HTuple(7)), ((hv_Index.TupleMult(New HTuple(7)))).TupleAdd( _
                                        New HTuple(6)))
                                    If hv_SelectedObjectOut.TupleSelect(hv_Index).I() Then
                                        HOperatorSet.PoseToHomMat3d(hv_PoseIn, hv_HomMat3DIn)
                                        HOperatorSet.HomMat3dTranslate(hv_HomMat3DIn, hv_Translate.TupleSelect( _
                                            New HTuple(0)), hv_Translate.TupleSelect(New HTuple(1)), _
                                            hv_Translate.TupleSelect(New HTuple(2)), hv_HomMat3DOut)
                                        HOperatorSet.HomMat3dToPose(hv_HomMat3DOut, hv_PoseOut)
                                    Else
                                        hv_PoseOut = hv_PoseIn.Clone()
                                    End If
                                    hv_PosesOut = hv_PosesOut.TupleConcat(hv_PoseOut)
#If USE_DO_EVENTS Then
                  ' Please note: The call of DoEvents() is only a hack to
                  ' enable VB to react on events. Please change the code
                  ' so that it can handle events in a standard way.
                  System.Windows.Forms.Application.DoEvents()
#End If
                                Next
                            Else
                                HOperatorSet.TupleFind(hv_SelectedObjectOut, New HTuple(1), hv_Indices)
                                hv_PoseIn = hv_PosesIn_COPY_INP_TMP.TupleSelectRange(((hv_Indices.TupleSelect( _
                                    New HTuple(0)))).TupleMult(New HTuple(7)), ((((hv_Indices.TupleSelect( _
                                    New HTuple(0)))).TupleMult(New HTuple(7)))).TupleAdd(New HTuple(6)))
                                HOperatorSet.PoseToHomMat3d(hv_PoseIn, hv_HomMat3DIn)
                                HOperatorSet.HomMat3dTranslate(hv_HomMat3DIn, hv_Translate.TupleSelect( _
                                    New HTuple(0)), hv_Translate.TupleSelect(New HTuple(1)), hv_Translate.TupleSelect( _
                                    New HTuple(2)), hv_HomMat3DOut)
                                HOperatorSet.HomMat3dToPose(hv_HomMat3DOut, hv_PoseOut)
                                hv_Sequence = HTuple.TupleGenSequence(New HTuple(0), ((hv_NumModels.TupleMult( _
                                    New HTuple(7)))).TupleSub(New HTuple(1)), New HTuple(1))
                                HOperatorSet.TupleMod(hv_Sequence, New HTuple(7), hv_Mod)
                                hv_SequenceReal = HTuple.TupleGenSequence(New HTuple(0), hv_NumModels.TupleSub( _
                                    (New HTuple(1.0)).TupleDiv(New HTuple(7.0))), (New HTuple(1.0)).TupleDiv( _
                                    New HTuple(7.0)))
                                hv_Sequence2Int = hv_SequenceReal.TupleInt()
                                HOperatorSet.TupleSelect(hv_SelectedObjectOut, hv_Sequence2Int, hv_Selected)
                                hv_InvSelected = (New HTuple(1)).TupleSub(hv_Selected)
                                HOperatorSet.TupleSelect(hv_PoseOut, hv_Mod, hv_PosesOut)
                                hv_PosesOut = ((hv_PosesOut.TupleMult(hv_Selected))).TupleAdd(hv_PosesIn_COPY_INP_TMP.TupleMult( _
                                    hv_InvSelected))
                            End If
                            dump_image_output(ho_BackgroundImage, hv_WindowHandleBuffer, hv_ObjectModel3DID, _
                                hv_GenParamName_COPY_INP_TMP, hv_GenParamValue_COPY_INP_TMP, hv_CamParam, _
                                hv_PosesOut, hv_ColorImage, hv_Title, hv_Information, hv_Labels, _
                                hv_VisualizeTB, New HTuple("true"), hv_TrackballCenterRow, hv_TrackballCenterCol, _
                                hv_TBSize_COPY_INP_TMP, hv_SelectedObjectOut, New HTuple(hv_WindowCenteredRotationOut.TupleEqual( _
                                New HTuple(1))), hv_TBCenter_COPY_INP_TMP)
                            ho_ImageDump.Dispose()
                            HOperatorSet.DumpWindowImage(ho_ImageDump, hv_WindowHandleBuffer)
                            HDevWindowStack.SetActive(hv_WindowHandle)
                            If (HDevWindowStack.IsOpen()) Then
                                HOperatorSet.DispObj(ho_ImageDump, HDevWindowStack.GetActive())
                            End If
                            '
                            hv_MRow1 = hv_Row_COPY_INP_TMP.Clone()
                            hv_MCol1 = hv_Column_COPY_INP_TMP.Clone()
                            hv_PosesIn_COPY_INP_TMP = hv_PosesOut.Clone()
                            ' catch (Exception) 
                        Catch HDevExpDefaultException1 As HalconException
                            HDevExpDefaultException1.ToHTuple(hv_Exception)
                            'Keep waiting
                        End Try
#If USE_DO_EVENTS Then
            ' Please note: The call of DoEvents() is only a hack to
            ' enable VB to react on events. Please change the code
            ' so that it can handle events in a standard way.
            System.Windows.Forms.Application.DoEvents()
#End If
                    Loop
                ElseIf hv_IsButtonDist.I() Then
                    'Change the Z distance
                    hv_MRow1 = hv_Row_COPY_INP_TMP.Clone()
                    Do While hv_IsButtonDist.I()
                        Try
                            HOperatorSet.GetMpositionSubPix(hv_WindowHandle, hv_Row_COPY_INP_TMP, _
                                hv_Column_COPY_INP_TMP, hv_ButtonLoop)
                            hv_IsButtonDist = New HTuple(hv_ButtonLoop.TupleEqual(hv_Button))
                            hv_MRow2 = hv_Row_COPY_INP_TMP.Clone()
                            hv_DRow = hv_MRow2.TupleSub(hv_MRow1)
                            hv_Dist = ((((((((hv_TBCenter_COPY_INP_TMP.TupleSelect(New HTuple(0)))).TupleMult( _
                                hv_TBCenter_COPY_INP_TMP.TupleSelect(New HTuple(0))))).TupleAdd( _
                                ((hv_TBCenter_COPY_INP_TMP.TupleSelect(New HTuple(1)))).TupleMult( _
                                hv_TBCenter_COPY_INP_TMP.TupleSelect(New HTuple(1)))))).TupleAdd( _
                                ((hv_TBCenter_COPY_INP_TMP.TupleSelect(New HTuple(2)))).TupleMult( _
                                hv_TBCenter_COPY_INP_TMP.TupleSelect(New HTuple(2)))))).TupleSqrt( _
                                )
                            hv_TranslateZ = ((((((hv_Dist.TupleNeg())).TupleMult(hv_DRow))).TupleMult( _
                                New HTuple(0.003)))).TupleMult(hv_SensFactor)
                            If IsNothing(hv_TBCenter_COPY_INP_TMP) Then
                                hv_TBCenter_COPY_INP_TMP = New HTuple
                            End If
                            hv_TBCenter_COPY_INP_TMP(New HTuple(2)) = ((hv_TBCenter_COPY_INP_TMP.TupleSelect( _
                                New HTuple(2)))).TupleAdd(hv_TranslateZ)
                            hv_PosesOut = New HTuple()
                            If New HTuple(hv_NumModels.TupleLessEqual(hv_MaxNumModels)).I() Then
                                For hv_Index = (New HTuple(0)) To _
                                    hv_NumModels.TupleSub(New HTuple(1)) Step (New HTuple(1))
                                    hv_PoseIn = hv_PosesIn_COPY_INP_TMP.TupleSelectRange(hv_Index.TupleMult( _
                                        New HTuple(7)), ((hv_Index.TupleMult(New HTuple(7)))).TupleAdd( _
                                        New HTuple(6)))
                                    If hv_SelectedObjectOut.TupleSelect(hv_Index).I() Then
                                        'Transform the whole scene or selected object only
                                        HOperatorSet.PoseToHomMat3d(hv_PoseIn, hv_HomMat3DIn)
                                        HOperatorSet.HomMat3dTranslate(hv_HomMat3DIn, New HTuple(0), _
                                            New HTuple(0), hv_TranslateZ, hv_HomMat3DOut)
                                        HOperatorSet.HomMat3dToPose(hv_HomMat3DOut, hv_PoseOut)
                                    Else
                                        hv_PoseOut = hv_PoseIn.Clone()
                                    End If
                                    hv_PosesOut = hv_PosesOut.TupleConcat(hv_PoseOut)
#If USE_DO_EVENTS Then
                  ' Please note: The call of DoEvents() is only a hack to
                  ' enable VB to react on events. Please change the code
                  ' so that it can handle events in a standard way.
                  System.Windows.Forms.Application.DoEvents()
#End If
                                Next
                            Else
                                HOperatorSet.TupleFind(hv_SelectedObjectOut, New HTuple(1), hv_Indices)
                                hv_PoseIn = hv_PosesIn_COPY_INP_TMP.TupleSelectRange(((hv_Indices.TupleSelect( _
                                    New HTuple(0)))).TupleMult(New HTuple(7)), ((((hv_Indices.TupleSelect( _
                                    New HTuple(0)))).TupleMult(New HTuple(7)))).TupleAdd(New HTuple(6)))
                                HOperatorSet.PoseToHomMat3d(hv_PoseIn, hv_HomMat3DIn)
                                HOperatorSet.HomMat3dTranslate(hv_HomMat3DIn, New HTuple(0), New HTuple(0), _
                                    hv_TranslateZ, hv_HomMat3DOut)
                                HOperatorSet.HomMat3dToPose(hv_HomMat3DOut, hv_PoseOut)
                                hv_Sequence = HTuple.TupleGenSequence(New HTuple(0), ((hv_NumModels.TupleMult( _
                                    New HTuple(7)))).TupleSub(New HTuple(1)), New HTuple(1))
                                HOperatorSet.TupleMod(hv_Sequence, New HTuple(7), hv_Mod)
                                hv_SequenceReal = HTuple.TupleGenSequence(New HTuple(0), hv_NumModels.TupleSub( _
                                    (New HTuple(1.0)).TupleDiv(New HTuple(7.0))), (New HTuple(1.0)).TupleDiv( _
                                    New HTuple(7.0)))
                                hv_Sequence2Int = hv_SequenceReal.TupleInt()
                                HOperatorSet.TupleSelect(hv_SelectedObjectOut, hv_Sequence2Int, hv_Selected)
                                hv_InvSelected = (New HTuple(1)).TupleSub(hv_Selected)
                                HOperatorSet.TupleSelect(hv_PoseOut, hv_Mod, hv_PosesOut)
                                hv_PosesOut = ((hv_PosesOut.TupleMult(hv_Selected))).TupleAdd(hv_PosesIn_COPY_INP_TMP.TupleMult( _
                                    hv_InvSelected))
                            End If
                            dump_image_output(ho_BackgroundImage, hv_WindowHandleBuffer, hv_ObjectModel3DID, _
                                hv_GenParamName_COPY_INP_TMP, hv_GenParamValue_COPY_INP_TMP, hv_CamParam, _
                                hv_PosesOut, hv_ColorImage, hv_Title, hv_Information, hv_Labels, _
                                hv_VisualizeTB, New HTuple("true"), hv_TrackballCenterRow, hv_TrackballCenterCol, _
                                hv_TBSize_COPY_INP_TMP, hv_SelectedObjectOut, hv_WindowCenteredRotationOut, _
                                hv_TBCenter_COPY_INP_TMP)
                            ho_ImageDump.Dispose()
                            HOperatorSet.DumpWindowImage(ho_ImageDump, hv_WindowHandleBuffer)
                            HDevWindowStack.SetActive(hv_WindowHandle)
                            If (HDevWindowStack.IsOpen()) Then
                                HOperatorSet.DispObj(ho_ImageDump, HDevWindowStack.GetActive())
                            End If
                            '
                            hv_MRow1 = hv_Row_COPY_INP_TMP.Clone()
                            hv_PosesIn_COPY_INP_TMP = hv_PosesOut.Clone()
                            If New HTuple(hv_ChangePoseOf.TupleEqual(New HTuple("all_objects"))).I() Then
                                hv_TrackballCenterCamOut = hv_TBCenter_COPY_INP_TMP.Clone()
                            End If
                            ' catch (Exception) 
                        Catch HDevExpDefaultException1 As HalconException
                            HDevExpDefaultException1.ToHTuple(hv_Exception)
                            'Keep waiting
                        End Try
#If USE_DO_EVENTS Then
            ' Please note: The call of DoEvents() is only a hack to
            ' enable VB to react on events. Please change the code
            ' so that it can handle events in a standard way.
            System.Windows.Forms.Application.DoEvents()
#End If
                    Loop
                ElseIf hv_IsButtonRot.I() Then
                    'Rotate the object
                    hv_MRow1 = hv_Row_COPY_INP_TMP.Clone()
                    hv_MCol1 = hv_Column_COPY_INP_TMP.Clone()
                    Do While hv_IsButtonRot.I()
                        Try
                            HOperatorSet.GetMpositionSubPix(hv_WindowHandle, hv_Row_COPY_INP_TMP, _
                                hv_Column_COPY_INP_TMP, hv_ButtonLoop)
                            hv_IsButtonRot = New HTuple(hv_ButtonLoop.TupleEqual(hv_Button))
                            hv_MRow2 = hv_Row_COPY_INP_TMP.Clone()
                            hv_MCol2 = hv_Column_COPY_INP_TMP.Clone()
                            'Transform the pixel coordinates to relative image coordinates
                            hv_MX1 = ((hv_TrackballCenterCol.TupleSub(hv_MCol1))).TupleDiv((New HTuple(0.5)).TupleMult( _
                                hv_MinImageSize))
                            hv_MY1 = ((hv_TrackballCenterRow.TupleSub(hv_MRow1))).TupleDiv((New HTuple(0.5)).TupleMult( _
                                hv_MinImageSize))
                            hv_MX2 = ((hv_TrackballCenterCol.TupleSub(hv_MCol2))).TupleDiv((New HTuple(0.5)).TupleMult( _
                                hv_MinImageSize))
                            hv_MY2 = ((hv_TrackballCenterRow.TupleSub(hv_MRow2))).TupleDiv((New HTuple(0.5)).TupleMult( _
                                hv_MinImageSize))
                            'Compute the quaternion rotation that corresponds to the mouse
                            'movement
                            trackball(hv_MX1, hv_MY1, hv_MX2, hv_MY2, hv_VirtualTrackball, hv_TrackballSize, _
                                hv_SensFactor, hv_RelQuaternion)
                            'Transform the quaternion to a rotation matrix
                            HOperatorSet.QuatToHomMat3d(hv_RelQuaternion, hv_HomMat3DRotRel)
                            hv_PosesOut = New HTuple()
                            If New HTuple(hv_NumModels.TupleLessEqual(hv_MaxNumModels)).I() Then
                                For hv_Index = (New HTuple(0)) To _
                                    hv_NumModels.TupleSub(New HTuple(1)) Step (New HTuple(1))
                                    hv_PoseIn = hv_PosesIn_COPY_INP_TMP.TupleSelectRange(hv_Index.TupleMult( _
                                        New HTuple(7)), ((hv_Index.TupleMult(New HTuple(7)))).TupleAdd( _
                                        New HTuple(6)))
                                    If hv_SelectedObjectOut.TupleSelect(hv_Index).I() Then
                                        'Transform the whole scene or selected object only
                                        HOperatorSet.PoseToHomMat3d(hv_PoseIn, hv_HomMat3DIn)
                                        HOperatorSet.HomMat3dTranslate(hv_HomMat3DIn, ((hv_TBCenter_COPY_INP_TMP.TupleSelect( _
                                            New HTuple(0)))).TupleNeg(), ((hv_TBCenter_COPY_INP_TMP.TupleSelect( _
                                            New HTuple(1)))).TupleNeg(), ((hv_TBCenter_COPY_INP_TMP.TupleSelect( _
                                            New HTuple(2)))).TupleNeg(), hv_HomMat3DIn)
                                        HOperatorSet.HomMat3dCompose(hv_HomMat3DRotRel, hv_HomMat3DIn, _
                                            hv_HomMat3DIn)
                                        HOperatorSet.HomMat3dTranslate(hv_HomMat3DIn, hv_TBCenter_COPY_INP_TMP.TupleSelect( _
                                            New HTuple(0)), hv_TBCenter_COPY_INP_TMP.TupleSelect(New HTuple(1)), _
                                            hv_TBCenter_COPY_INP_TMP.TupleSelect(New HTuple(2)), hv_HomMat3DOut)
                                        HOperatorSet.HomMat3dToPose(hv_HomMat3DOut, hv_PoseOut)
                                    Else
                                        hv_PoseOut = hv_PoseIn.Clone()
                                    End If
                                    hv_PosesOut = hv_PosesOut.TupleConcat(hv_PoseOut)
#If USE_DO_EVENTS Then
                  ' Please note: The call of DoEvents() is only a hack to
                  ' enable VB to react on events. Please change the code
                  ' so that it can handle events in a standard way.
                  System.Windows.Forms.Application.DoEvents()
#End If
                                Next
                            Else
                                HOperatorSet.TupleFind(hv_SelectedObjectOut, New HTuple(1), hv_Indices)
                                hv_PoseIn = hv_PosesIn_COPY_INP_TMP.TupleSelectRange(((hv_Indices.TupleSelect( _
                                    New HTuple(0)))).TupleMult(New HTuple(7)), ((((hv_Indices.TupleSelect( _
                                    New HTuple(0)))).TupleMult(New HTuple(7)))).TupleAdd(New HTuple(6)))
                                HOperatorSet.PoseToHomMat3d(hv_PoseIn, hv_HomMat3DIn)
                                HOperatorSet.HomMat3dTranslate(hv_HomMat3DIn, ((hv_TBCenter_COPY_INP_TMP.TupleSelect( _
                                    New HTuple(0)))).TupleNeg(), ((hv_TBCenter_COPY_INP_TMP.TupleSelect( _
                                    New HTuple(1)))).TupleNeg(), ((hv_TBCenter_COPY_INP_TMP.TupleSelect( _
                                    New HTuple(2)))).TupleNeg(), hv_HomMat3DInTmp1)
                                HOperatorSet.HomMat3dCompose(hv_HomMat3DRotRel, hv_HomMat3DInTmp1, _
                                    hv_HomMat3DInTmp)
                                HOperatorSet.HomMat3dTranslate(hv_HomMat3DInTmp, hv_TBCenter_COPY_INP_TMP.TupleSelect( _
                                    New HTuple(0)), hv_TBCenter_COPY_INP_TMP.TupleSelect(New HTuple(1)), _
                                    hv_TBCenter_COPY_INP_TMP.TupleSelect(New HTuple(2)), hv_HomMat3DOut)
                                HOperatorSet.HomMat3dToPose(hv_HomMat3DOut, hv_PoseOut)
                                hv_Sequence = HTuple.TupleGenSequence(New HTuple(0), ((hv_NumModels.TupleMult( _
                                    New HTuple(7)))).TupleSub(New HTuple(1)), New HTuple(1))
                                HOperatorSet.TupleMod(hv_Sequence, New HTuple(7), hv_Mod)
                                hv_SequenceReal = HTuple.TupleGenSequence(New HTuple(0), hv_NumModels.TupleSub( _
                                    (New HTuple(1.0)).TupleDiv(New HTuple(7.0))), (New HTuple(1.0)).TupleDiv( _
                                    New HTuple(7.0)))
                                hv_Sequence2Int = hv_SequenceReal.TupleInt()
                                HOperatorSet.TupleSelect(hv_SelectedObjectOut, hv_Sequence2Int, hv_Selected)
                                hv_InvSelected = (New HTuple(1)).TupleSub(hv_Selected)
                                HOperatorSet.TupleSelect(hv_PoseOut, hv_Mod, hv_PosesOut)
                                hv_PosesOut2 = ((hv_PosesOut.TupleMult(hv_Selected))).TupleAdd(hv_PosesIn_COPY_INP_TMP.TupleMult( _
                                    hv_InvSelected))
                                hv_PosesOut = hv_PosesOut2.Clone()
                            End If
                            dump_image_output(ho_BackgroundImage, hv_WindowHandleBuffer, hv_ObjectModel3DID, _
                                hv_GenParamName_COPY_INP_TMP, hv_GenParamValue_COPY_INP_TMP, hv_CamParam, _
                                hv_PosesOut, hv_ColorImage, hv_Title, hv_Information, hv_Labels, _
                                hv_VisualizeTB, New HTuple("true"), hv_TrackballCenterRow, hv_TrackballCenterCol, _
                                hv_TBSize_COPY_INP_TMP, hv_SelectedObjectOut, hv_WindowCenteredRotationOut, _
                                hv_TBCenter_COPY_INP_TMP)
                            ho_ImageDump.Dispose()
                            HOperatorSet.DumpWindowImage(ho_ImageDump, hv_WindowHandleBuffer)
                            HDevWindowStack.SetActive(hv_WindowHandle)
                            If (HDevWindowStack.IsOpen()) Then
                                HOperatorSet.DispObj(ho_ImageDump, HDevWindowStack.GetActive())
                            End If
                            '
                            hv_MRow1 = hv_Row_COPY_INP_TMP.Clone()
                            hv_MCol1 = hv_Column_COPY_INP_TMP.Clone()
                            hv_PosesIn_COPY_INP_TMP = hv_PosesOut.Clone()
                            ' catch (Exception) 
                        Catch HDevExpDefaultException1 As HalconException
                            HDevExpDefaultException1.ToHTuple(hv_Exception)
                            'Keep waiting
                        End Try
#If USE_DO_EVENTS Then
            ' Please note: The call of DoEvents() is only a hack to
            ' enable VB to react on events. Please change the code
            ' so that it can handle events in a standard way.
            System.Windows.Forms.Application.DoEvents()
#End If
                    Loop
                End If
                hv_PosesOut = hv_PosesIn_COPY_INP_TMP.Clone()
            End If
            ho_ImageDump.Dispose()

            Exit Sub
        Catch HDevExpDefaultException As HalconException
            ho_ImageDump.Dispose()

            Throw HDevExpDefaultException
        End Try
    End Sub

    ' Chapter: Graphics / Output
    Public Sub disp_title_and_information(ByVal hv_WindowHandle As HTuple, ByVal hv_Title As HTuple, _
        ByVal hv_Information As HTuple)


        ' Local control variables 
        Dim hv_WinRow As HTuple = New HTuple, hv_WinColumn As HTuple = New HTuple
        Dim hv_WinWidth As HTuple = New HTuple, hv_WinHeight As HTuple = New HTuple
        Dim hv_NumTitleLines As HTuple = New HTuple, hv_Row As HTuple = New HTuple
        Dim hv_Column As HTuple = New HTuple, hv_TextWidth As HTuple = New HTuple
        Dim hv_NumInfoLines As HTuple = New HTuple, hv_Ascent As HTuple = New HTuple
        Dim hv_Descent As HTuple = New HTuple, hv_Width As HTuple = New HTuple
        Dim hv_Height As HTuple = New HTuple

        Dim hv_Information_COPY_INP_TMP As HTuple
        hv_Information_COPY_INP_TMP = hv_Information.Clone()
        Dim hv_Title_COPY_INP_TMP As HTuple
        hv_Title_COPY_INP_TMP = hv_Title.Clone()

        ' Initialize local and output iconic variables 

        'global tuple gInfoDecor
        'global tuple gInfoPos
        'global tuple gTitlePos
        'global tuple gTitleDecor
        '
        HOperatorSet.GetWindowExtents(hv_WindowHandle, hv_WinRow, hv_WinColumn, hv_WinWidth, _
            hv_WinHeight)
        hv_Title_COPY_INP_TMP = (((((New HTuple("")).TupleAdd(hv_Title_COPY_INP_TMP))).TupleAdd( _
            New HTuple("")))).TupleSplit(New HTuple("" + Chr(10)))
        hv_NumTitleLines = New HTuple(hv_Title_COPY_INP_TMP.TupleLength())
        If New HTuple(hv_NumTitleLines.TupleGreater(New HTuple(0))).I() Then
            hv_Row = New HTuple(12)
            If New HTuple(ExpGetGlobalVar_gTitlePos().TupleEqual(New HTuple("UpperLeft"))).I() Then
                hv_Column = New HTuple(12)
            ElseIf New HTuple(ExpGetGlobalVar_gTitlePos().TupleEqual(New HTuple("UpperCenter"))).I() Then
                max_line_width(hv_WindowHandle, hv_Title_COPY_INP_TMP, hv_TextWidth)
                hv_Column = ((hv_WinWidth.TupleDiv(New HTuple(2)))).TupleSub(hv_TextWidth.TupleDiv( _
                    New HTuple(2)))
            ElseIf New HTuple(ExpGetGlobalVar_gTitlePos().TupleEqual(New HTuple("UpperRight"))).I() Then
                If New HTuple(((ExpGetGlobalVar_gTitleDecor().TupleSelect(New HTuple(1)))).TupleEqual( _
                    New HTuple("true"))).I() Then
                    max_line_width(hv_WindowHandle, hv_Title_COPY_INP_TMP.TupleAdd(New HTuple("  ")), _
                        hv_TextWidth)
                Else
                    max_line_width(hv_WindowHandle, hv_Title_COPY_INP_TMP, hv_TextWidth)
                End If
                hv_Column = ((hv_WinWidth.TupleSub(hv_TextWidth))).TupleSub(New HTuple(10))
            Else
                'Unknown position!
                HDevelopStop()
            End If
            disp_message(hv_WindowHandle, hv_Title_COPY_INP_TMP, New HTuple("window"), _
                hv_Row, hv_Column, ExpGetGlobalVar_gTitleDecor().TupleSelect(New HTuple(0)), _
                ExpGetGlobalVar_gTitleDecor().TupleSelect(New HTuple(1)))
        End If
        hv_Information_COPY_INP_TMP = (((((New HTuple("")).TupleAdd(hv_Information_COPY_INP_TMP))).TupleAdd( _
            New HTuple("")))).TupleSplit(New HTuple("" + Chr(10)))
        hv_NumInfoLines = New HTuple(hv_Information_COPY_INP_TMP.TupleLength())
        If New HTuple(hv_NumInfoLines.TupleGreater(New HTuple(0))).I() Then
            If New HTuple(ExpGetGlobalVar_gInfoPos().TupleEqual(New HTuple("UpperLeft"))).I() Then
                hv_Row = New HTuple(12)
                hv_Column = New HTuple(12)
            ElseIf New HTuple(ExpGetGlobalVar_gInfoPos().TupleEqual(New HTuple("UpperRight"))).I() Then
                If New HTuple(((ExpGetGlobalVar_gInfoDecor().TupleSelect(New HTuple(1)))).TupleEqual( _
                    New HTuple("true"))).I() Then
                    max_line_width(hv_WindowHandle, hv_Information_COPY_INP_TMP.TupleAdd(New HTuple("  ")), _
                        hv_TextWidth)
                Else
                    max_line_width(hv_WindowHandle, hv_Information_COPY_INP_TMP, hv_TextWidth)
                End If
                hv_Row = New HTuple(12)
                hv_Column = ((hv_WinWidth.TupleSub(hv_TextWidth))).TupleSub(New HTuple(12))
            ElseIf New HTuple(ExpGetGlobalVar_gInfoPos().TupleEqual(New HTuple("LowerLeft"))).I() Then
                HOperatorSet.GetStringExtents(hv_WindowHandle, hv_Information_COPY_INP_TMP, _
                    hv_Ascent, hv_Descent, hv_Width, hv_Height)
                hv_Row = ((hv_WinHeight.TupleSub(hv_NumInfoLines.TupleMult(hv_Height)))).TupleSub( _
                    New HTuple(12))
                hv_Column = New HTuple(12)
            Else
                'Unknown position!
                HDevelopStop()
            End If
            disp_message(hv_WindowHandle, hv_Information_COPY_INP_TMP, New HTuple("window"), _
                hv_Row, hv_Column, ExpGetGlobalVar_gInfoDecor().TupleSelect(New HTuple(0)), _
                ExpGetGlobalVar_gInfoDecor().TupleSelect(New HTuple(1)))
        End If
        '

        Exit Sub
    End Sub

    ' Chapter: Graphics / Output
    ' Short Description: Can replace disp_object_model_3d if there is no OpenGL available. 
    Public Sub disp_object_model_no_opengl(ByRef ho_ModelContours As HObject, ByVal hv_ObjectModel3DID As HTuple, _
        ByVal hv_GenParamName As HTuple, ByVal hv_GenParamValue As HTuple, ByVal hv_WindowHandleBuffer As HTuple, _
        ByVal hv_CamParam As HTuple, ByVal hv_PosesOut As HTuple)


        ' Local control variables 
        Dim hv_Idx As HTuple = New HTuple, hv_CustomParamName As HTuple = New HTuple
        Dim hv_CustomParamValue As HTuple = New HTuple, hv_Font As HTuple = New HTuple
        Dim hv_IndicesDispBackGround As HTuple = New HTuple
        Dim hv_Indices As HTuple = New HTuple, hv_HasPolygons As HTuple = New HTuple
        Dim hv_HasTri As HTuple = New HTuple, hv_HasPoints As HTuple = New HTuple
        Dim hv_HasLines As HTuple = New HTuple, hv_NumPoints As HTuple = New HTuple
        Dim hv_IsPrimitive As HTuple = New HTuple, hv_Center As HTuple = New HTuple
        Dim hv_Diameter As HTuple = New HTuple, hv_OpenGlHiddenSurface As HTuple = New HTuple
        Dim hv_CenterX As HTuple = New HTuple, hv_CenterY As HTuple = New HTuple
        Dim hv_CenterZ As HTuple = New HTuple, hv_PosObjectsZ As HTuple = New HTuple
        Dim hv_I As HTuple = New HTuple, hv_Pose As HTuple = New HTuple
        Dim hv_HomMat3DObj As HTuple = New HTuple, hv_PosObjCenterX As HTuple = New HTuple
        Dim hv_PosObjCenterY As HTuple = New HTuple, hv_PosObjCenterZ As HTuple = New HTuple
        Dim hv_PosObjectsX As HTuple = New HTuple, hv_PosObjectsY As HTuple = New HTuple
        Dim hv_Color As HTuple = New HTuple, hv_Indices1 As HTuple = New HTuple
        Dim hv_IndicesIntensities As HTuple = New HTuple, hv_Indices2 As HTuple = New HTuple
        Dim hv_J As HTuple = New HTuple, hv_Indices3 As HTuple = New HTuple
        Dim hv_HomMat3D As HTuple = New HTuple, hv_SampledObjectModel3D As HTuple = New HTuple
        Dim hv_X As HTuple = New HTuple, hv_Y As HTuple = New HTuple
        Dim hv_Z As HTuple = New HTuple, hv_HomMat3D1 As HTuple = New HTuple
        Dim hv_Qx As HTuple = New HTuple, hv_Qy As HTuple = New HTuple
        Dim hv_Qz As HTuple = New HTuple, hv_Row As HTuple = New HTuple
        Dim hv_Column As HTuple = New HTuple, hv_ObjectModel3DConvexHull As HTuple = New HTuple
        Dim hv_Exception As HTuple = New HTuple

        ' Initialize local and output iconic variables 
        HOperatorSet.GenEmptyObj(ho_ModelContours)

        'This procedure allows to use project_object_model_3d to simulate a disp_object_model_3d
        'call foor smal objects, Large objects are sampled down to display.
        hv_Idx = hv_GenParamName.TupleFind(New HTuple("point_size"))
        If ((New HTuple(hv_Idx.TupleLength()))).TupleAnd(New HTuple(hv_Idx.TupleNotEqual( _
            New HTuple(-1)))).I() Then
            hv_CustomParamName = New HTuple("point_size")
            hv_CustomParamValue = hv_GenParamValue.TupleSelect(hv_Idx)
            If New HTuple(hv_CustomParamValue.TupleEqual(New HTuple(1))).I() Then
                hv_CustomParamValue = New HTuple(0)
            End If
        Else
            hv_CustomParamName = New HTuple()
            hv_CustomParamValue = New HTuple()
        End If
        HOperatorSet.GetFont(hv_WindowHandleBuffer, hv_Font)
        HOperatorSet.TupleFind(hv_GenParamName, New HTuple("disp_background"), hv_IndicesDispBackGround)
        If New HTuple(hv_IndicesDispBackGround.TupleNotEqual(New HTuple(-1))).I() Then
            HOperatorSet.TupleFind(hv_GenParamName.TupleSelect(hv_IndicesDispBackGround), _
                New HTuple("false"), hv_Indices)
            If New HTuple(hv_Indices.TupleNotEqual(New HTuple(-1))).I() Then
                HOperatorSet.ClearWindow(hv_WindowHandleBuffer)
            End If
        End If
        set_display_font(hv_WindowHandleBuffer, New HTuple(11), New HTuple("mono"), New HTuple("false"), _
            New HTuple("false"))
        disp_message(hv_WindowHandleBuffer, New HTuple("OpenGL missing!"), New HTuple("image"), _
            New HTuple(5), ((hv_CamParam.TupleSelect(New HTuple(6)))).TupleSub(New HTuple(130)), _
            New HTuple("red"), New HTuple("false"))
        HOperatorSet.SetFont(hv_WindowHandleBuffer, hv_Font)
        HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, New HTuple("has_polygons"), _
            hv_HasPolygons)
        HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, New HTuple("has_triangles"), _
            hv_HasTri)
        HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, New HTuple("has_points"), _
            hv_HasPoints)
        HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, New HTuple("has_lines"), _
            hv_HasLines)
        HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, New HTuple("num_points"), _
            hv_NumPoints)
        HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, New HTuple("has_primitive_data"), _
            hv_IsPrimitive)
        HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, New HTuple("center"), _
            hv_Center)
        HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, New HTuple("diameter"), _
            hv_Diameter)
        HOperatorSet.GetSystem(New HTuple("opengl_hidden_surface_removal_enable"), hv_OpenGlHiddenSurface)
        HOperatorSet.SetSystem(New HTuple("opengl_hidden_surface_removal_enable"), New HTuple("false"))
        'Sort the objects by inverse z
        hv_CenterX = hv_Center.TupleSelect(HTuple.TupleGenSequence(New HTuple(0), ((New HTuple( _
            hv_Center.TupleLength()))).TupleSub(New HTuple(1)), New HTuple(3)))
        hv_CenterY = hv_Center.TupleSelect((HTuple.TupleGenSequence(New HTuple(0), ((New HTuple( _
            hv_Center.TupleLength()))).TupleSub(New HTuple(1)), New HTuple(3))).TupleAdd( _
            New HTuple(1)))
        hv_CenterZ = hv_Center.TupleSelect((HTuple.TupleGenSequence(New HTuple(0), ((New HTuple( _
            hv_Center.TupleLength()))).TupleSub(New HTuple(1)), New HTuple(3))).TupleAdd( _
            New HTuple(2)))
        hv_PosObjectsZ = New HTuple()
        If New HTuple(((New HTuple(hv_PosesOut.TupleLength()))).TupleGreater(New HTuple(7))).I() Then
            For hv_I = (New HTuple(0)) To ( _
                (New HTuple(hv_ObjectModel3DID.TupleLength()))).TupleSub(New HTuple(1)) Step (New HTuple(1))
                hv_Pose = hv_PosesOut.TupleSelectRange(hv_I.TupleMult(New HTuple(7)), ((hv_I.TupleMult( _
                    New HTuple(7)))).TupleAdd(New HTuple(6)))
                HOperatorSet.PoseToHomMat3d(hv_Pose, hv_HomMat3DObj)
                HOperatorSet.AffineTransPoint3d(hv_HomMat3DObj, hv_CenterX.TupleSelect(hv_I), _
                    hv_CenterY.TupleSelect(hv_I), hv_CenterZ.TupleSelect(hv_I), hv_PosObjCenterX, _
                    hv_PosObjCenterY, hv_PosObjCenterZ)
                hv_PosObjectsZ = hv_PosObjectsZ.TupleConcat(hv_PosObjCenterZ)
#If USE_DO_EVENTS Then
        ' Please note: The call of DoEvents() is only a hack to
        ' enable VB to react on events. Please change the code
        ' so that it can handle events in a standard way.
        System.Windows.Forms.Application.DoEvents()
#End If
            Next
        Else
            hv_Pose = hv_PosesOut.TupleSelectRange(New HTuple(0), New HTuple(6))
            HOperatorSet.PoseToHomMat3d(hv_Pose, hv_HomMat3DObj)
            HOperatorSet.AffineTransPoint3d(hv_HomMat3DObj, hv_CenterX, hv_CenterY, hv_CenterZ, _
                hv_PosObjectsX, hv_PosObjectsY, hv_PosObjectsZ)
        End If
        hv_Idx = ((hv_PosObjectsZ.TupleSortIndex())).TupleInverse()
        hv_Color = New HTuple("white")
        If New HTuple(((New HTuple(hv_GenParamName.TupleLength()))).TupleGreater(New HTuple(0))).I() Then
            HOperatorSet.TupleFind(hv_GenParamName, New HTuple("colored"), hv_Indices1)
            HOperatorSet.TupleFind(hv_GenParamName, New HTuple("intensity"), hv_IndicesIntensities)
            HOperatorSet.TupleFind(hv_GenParamName, New HTuple("color"), hv_Indices2)
            If New HTuple(((hv_Indices1.TupleSelect(New HTuple(0)))).TupleNotEqual(New HTuple(-1))).I() Then
                If New HTuple(((hv_GenParamValue.TupleSelect(hv_Indices1.TupleSelect(New HTuple(0))))).TupleEqual( _
                    New HTuple(3))).I() Then
                    hv_Color = ((New HTuple(New HTuple("red"))).TupleConcat(New HTuple("green"))).TupleConcat( _
                        New HTuple("blue"))
                ElseIf New HTuple(((hv_GenParamValue.TupleSelect(hv_Indices1.TupleSelect( _
                    New HTuple(0))))).TupleEqual(New HTuple(6))).I() Then
                    hv_Color = (((((New HTuple(New HTuple("red"))).TupleConcat(New HTuple("green"))).TupleConcat( _
                        New HTuple("blue"))).TupleConcat(New HTuple("cyan"))).TupleConcat(New HTuple("magenta"))).TupleConcat( _
                        New HTuple("yellow"))
                ElseIf New HTuple(((hv_GenParamValue.TupleSelect(hv_Indices1.TupleSelect( _
                    New HTuple(0))))).TupleEqual(New HTuple(12))).I() Then
                    hv_Color = (((((((((((New HTuple(New HTuple("red"))).TupleConcat(New HTuple("green"))).TupleConcat( _
                        New HTuple("blue"))).TupleConcat(New HTuple("cyan"))).TupleConcat(New HTuple("magenta"))).TupleConcat( _
                        New HTuple("yellow"))).TupleConcat(New HTuple("coral"))).TupleConcat( _
                        New HTuple("slate blue"))).TupleConcat(New HTuple("spring green"))).TupleConcat( _
                        New HTuple("orange red"))).TupleConcat(New HTuple("pink"))).TupleConcat( _
                        New HTuple("gold"))
                End If
            ElseIf New HTuple(((hv_Indices2.TupleSelect(New HTuple(0)))).TupleNotEqual( _
                New HTuple(-1))).I() Then
                hv_Color = hv_GenParamValue.TupleSelect(hv_Indices2.TupleSelect(New HTuple(0)))
            ElseIf New HTuple(((hv_IndicesIntensities.TupleSelect(New HTuple(0)))).TupleNotEqual( _
                New HTuple(-1))).I() Then
            End If
        End If
        For hv_J = (New HTuple(0)) To ( _
            (New HTuple(hv_ObjectModel3DID.TupleLength()))).TupleSub(New HTuple(1)) Step (New HTuple(1))
            hv_I = hv_Idx.TupleSelect(hv_J)
            If ((((((New HTuple(((hv_HasPolygons.TupleSelect(hv_I))).TupleEqual(New HTuple("true"))))).TupleOr( _
                New HTuple(((hv_HasTri.TupleSelect(hv_I))).TupleEqual(New HTuple("true")))))).TupleOr( _
                New HTuple(((hv_HasPoints.TupleSelect(hv_I))).TupleEqual(New HTuple("true")))))).TupleOr( _
                New HTuple(((hv_HasLines.TupleSelect(hv_I))).TupleEqual(New HTuple("true")))).I() Then
                If New HTuple(((New HTuple(hv_GenParamName.TupleLength()))).TupleGreater( _
                    New HTuple(0))).I() Then
                    HOperatorSet.TupleFind(hv_GenParamName, (New HTuple("color_")).TupleAdd( _
                        hv_I), hv_Indices3)
                    If New HTuple(((hv_Indices3.TupleSelect(New HTuple(0)))).TupleNotEqual( _
                        New HTuple(-1))).I() Then
                        HOperatorSet.SetColor(hv_WindowHandleBuffer, hv_GenParamValue.TupleSelect( _
                            hv_Indices3.TupleSelect(New HTuple(0))))
                    Else
                        HOperatorSet.SetColor(hv_WindowHandleBuffer, hv_Color.TupleSelect(hv_I.TupleMod( _
                            New HTuple(hv_Color.TupleLength()))))
                    End If
                End If
                If New HTuple(((New HTuple(hv_PosesOut.TupleLength()))).TupleGreaterEqual( _
                    ((hv_I.TupleMult(New HTuple(7)))).TupleAdd(New HTuple(6)))).I() Then
                    hv_Pose = hv_PosesOut.TupleSelectRange(hv_I.TupleMult(New HTuple(7)), (( _
                        hv_I.TupleMult(New HTuple(7)))).TupleAdd(New HTuple(6)))
                Else
                    hv_Pose = hv_PosesOut.TupleSelectRange(New HTuple(0), New HTuple(6))
                End If
                If New HTuple(((hv_NumPoints.TupleSelect(hv_I))).TupleLess(New HTuple(10000))).I() Then
                    ho_ModelContours.Dispose()
                    HOperatorSet.ProjectObjectModel3d(ho_ModelContours, hv_ObjectModel3DID.TupleSelect( _
                        hv_I), hv_CamParam, hv_Pose, hv_CustomParamName, hv_CustomParamValue)
                    HOperatorSet.DispObj(ho_ModelContours, hv_WindowHandleBuffer)
                Else
                    HOperatorSet.PoseToHomMat3d(hv_Pose, hv_HomMat3D)
                    HOperatorSet.SampleObjectModel3d(hv_ObjectModel3DID.TupleSelect(hv_I), _
                        New HTuple("fast"), (New HTuple(0.01)).TupleMult(hv_Diameter.TupleSelect( _
                        hv_I)), New HTuple(), New HTuple(), hv_SampledObjectModel3D)
                    ho_ModelContours.Dispose()
                    HOperatorSet.ProjectObjectModel3d(ho_ModelContours, hv_SampledObjectModel3D, _
                        hv_CamParam, hv_Pose, New HTuple("point_size"), New HTuple(1))
                    HOperatorSet.GetObjectModel3dParams(hv_SampledObjectModel3D, New HTuple("point_coord_x"), _
                        hv_X)
                    HOperatorSet.GetObjectModel3dParams(hv_SampledObjectModel3D, New HTuple("point_coord_y"), _
                        hv_Y)
                    HOperatorSet.GetObjectModel3dParams(hv_SampledObjectModel3D, New HTuple("point_coord_z"), _
                        hv_Z)
                    HOperatorSet.PoseToHomMat3d(hv_Pose, hv_HomMat3D1)
                    HOperatorSet.AffineTransPoint3d(hv_HomMat3D1, hv_X, hv_Y, hv_Z, hv_Qx, _
                        hv_Qy, hv_Qz)
                    HOperatorSet.Project3dPoint(hv_Qx, hv_Qy, hv_Qz, hv_CamParam, hv_Row, hv_Column)
                    HOperatorSet.DispObj(ho_ModelContours, hv_WindowHandleBuffer)
                    HOperatorSet.ClearObjectModel3d(hv_SampledObjectModel3D)
                End If
            Else
                If New HTuple(((New HTuple(hv_GenParamName.TupleLength()))).TupleGreater( _
                    New HTuple(0))).I() Then
                    HOperatorSet.TupleFind(hv_GenParamName, (New HTuple("color_")).TupleAdd( _
                        hv_I), hv_Indices3)
                    If New HTuple(((hv_Indices3.TupleSelect(New HTuple(0)))).TupleNotEqual( _
                        New HTuple(-1))).I() Then
                        HOperatorSet.SetColor(hv_WindowHandleBuffer, hv_GenParamValue.TupleSelect( _
                            hv_Indices3.TupleSelect(New HTuple(0))))
                    Else
                        HOperatorSet.SetColor(hv_WindowHandleBuffer, hv_Color.TupleSelect(hv_I.TupleMod( _
                            New HTuple(hv_Color.TupleLength()))))
                    End If
                End If
                If New HTuple(((New HTuple(hv_PosesOut.TupleLength()))).TupleGreaterEqual( _
                    ((hv_I.TupleMult(New HTuple(7)))).TupleAdd(New HTuple(6)))).I() Then
                    hv_Pose = hv_PosesOut.TupleSelectRange(hv_I.TupleMult(New HTuple(7)), (( _
                        hv_I.TupleMult(New HTuple(7)))).TupleAdd(New HTuple(6)))
                Else
                    hv_Pose = hv_PosesOut.TupleSelectRange(New HTuple(0), New HTuple(6))
                End If
                If New HTuple(((hv_IsPrimitive.TupleSelect(hv_I))).TupleEqual(New HTuple("true"))).I() Then
                    Try
                        HOperatorSet.ConvexHullObjectModel3d(hv_ObjectModel3DID.TupleSelect(hv_I), _
                            hv_ObjectModel3DConvexHull)
                        If New HTuple(((hv_NumPoints.TupleSelect(hv_I))).TupleLess(New HTuple(10000))).I() Then
                            ho_ModelContours.Dispose()
                            HOperatorSet.ProjectObjectModel3d(ho_ModelContours, hv_ObjectModel3DConvexHull, _
                                hv_CamParam, hv_Pose, hv_CustomParamName, hv_CustomParamValue)
                            HOperatorSet.DispObj(ho_ModelContours, hv_WindowHandleBuffer)
                        Else
                            HOperatorSet.PoseToHomMat3d(hv_Pose, hv_HomMat3D)
                            HOperatorSet.SampleObjectModel3d(hv_ObjectModel3DConvexHull, New HTuple("fast"), _
                                (New HTuple(0.01)).TupleMult(hv_Diameter.TupleSelect(hv_I)), New HTuple(), _
                                New HTuple(), hv_SampledObjectModel3D)
                            ho_ModelContours.Dispose()
                            HOperatorSet.ProjectObjectModel3d(ho_ModelContours, hv_SampledObjectModel3D, _
                                hv_CamParam, hv_Pose, New HTuple("point_size"), New HTuple(1))
                            HOperatorSet.DispObj(ho_ModelContours, hv_WindowHandleBuffer)
                            HOperatorSet.ClearObjectModel3d(hv_SampledObjectModel3D)
                        End If
                        HOperatorSet.ClearObjectModel3d(hv_ObjectModel3DConvexHull)
                        ' catch (Exception) 
                    Catch HDevExpDefaultException1 As HalconException
                        HDevExpDefaultException1.ToHTuple(hv_Exception)
                    End Try
                End If
            End If
#If USE_DO_EVENTS Then
      ' Please note: The call of DoEvents() is only a hack to
      ' enable VB to react on events. Please change the code
      ' so that it can handle events in a standard way.
      System.Windows.Forms.Application.DoEvents()
#End If
        Next
        HOperatorSet.SetSystem(New HTuple("opengl_hidden_surface_removal_enable"), hv_OpenGlHiddenSurface)

        Exit Sub
    End Sub

    ' Chapter: Graphics / Output
    ' Short Description: Determine the optimum distance of the object to obtain a reasonable visualization 
    Public Sub determine_optimum_pose_distance(ByVal hv_ObjectModel3DID As HTuple, _
        ByVal hv_CamParam As HTuple, ByVal hv_ImageCoverage As HTuple, ByVal hv_PoseIn As HTuple, _
        ByRef hv_PoseOut As HTuple)


        ' Local control variables 
        Dim hv_NumModels As HTuple = New HTuple, hv_Rows As HTuple = New HTuple
        Dim hv_Cols As HTuple = New HTuple, hv_MinMinZ As HTuple = New HTuple
        Dim hv_BB As HTuple = New HTuple, hv_Seq As HTuple = New HTuple
        Dim hv_DXMax As HTuple = New HTuple, hv_DYMax As HTuple = New HTuple
        Dim hv_DZMax As HTuple = New HTuple, hv_Diameter As HTuple = New HTuple
        Dim hv_ZAdd As HTuple = New HTuple, hv_IBB As HTuple = New HTuple
        Dim hv_BB0 As HTuple = New HTuple, hv_BB1 As HTuple = New HTuple
        Dim hv_BB2 As HTuple = New HTuple, hv_BB3 As HTuple = New HTuple
        Dim hv_BB4 As HTuple = New HTuple, hv_BB5 As HTuple = New HTuple
        Dim hv_X As HTuple = New HTuple, hv_Y As HTuple = New HTuple
        Dim hv_Z As HTuple = New HTuple, hv_PoseInter As HTuple = New HTuple
        Dim hv_HomMat3D As HTuple = New HTuple, hv_CX As HTuple = New HTuple
        Dim hv_CY As HTuple = New HTuple, hv_CZ As HTuple = New HTuple
        Dim hv_DR As HTuple = New HTuple, hv_DC As HTuple = New HTuple
        Dim hv_MaxDist As HTuple = New HTuple, hv_HomMat3DRotate As HTuple = New HTuple
        Dim hv_MinImageSize As HTuple = New HTuple, hv_Zs As HTuple = New HTuple
        Dim hv_ZDiff As HTuple = New HTuple, hv_ScaleZ As HTuple = New HTuple
        Dim hv_ZNew As HTuple = New HTuple

        ' Initialize local and output iconic variables 

        'Determine the optimum distance of the object to obtain
        'a reasonable visualization
        '
        hv_NumModels = New HTuple(hv_ObjectModel3DID.TupleLength())
        hv_Rows = New HTuple()
        hv_Cols = New HTuple()
        hv_MinMinZ = New HTuple(1.0E+30)
        HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, New HTuple("bounding_box1"), _
            hv_BB)
        'Calculate diameter over all objects to be visualized
        hv_Seq = HTuple.TupleGenSequence(New HTuple(0), ((New HTuple(hv_BB.TupleLength( _
            )))).TupleSub(New HTuple(1)), New HTuple(6))
        hv_DXMax = ((((hv_BB.TupleSelect(hv_Seq.TupleAdd(New HTuple(3))))).TupleMax())).TupleSub( _
            ((hv_BB.TupleSelect(hv_Seq))).TupleMin())
        hv_DYMax = ((((hv_BB.TupleSelect(hv_Seq.TupleAdd(New HTuple(4))))).TupleMax())).TupleSub( _
            ((hv_BB.TupleSelect(hv_Seq.TupleAdd(New HTuple(1))))).TupleMin())
        hv_DZMax = ((((hv_BB.TupleSelect(hv_Seq.TupleAdd(New HTuple(5))))).TupleMax())).TupleSub( _
            ((hv_BB.TupleSelect(hv_Seq.TupleAdd(New HTuple(2))))).TupleMin())
        hv_Diameter = ((((((hv_DXMax.TupleMult(hv_DXMax))).TupleAdd(hv_DYMax.TupleMult( _
            hv_DYMax)))).TupleAdd(hv_DZMax.TupleMult(hv_DZMax)))).TupleSqrt()
        If New HTuple(((((hv_BB.TupleAbs())).TupleSum())).TupleEqual(New HTuple(0.0))).I() Then
            hv_BB = (((((((HTuple.TupleRand(New HTuple(3))).TupleMult(New HTuple(1.0E-20)))).TupleAbs( _
                ))).TupleNeg())).TupleConcat((((HTuple.TupleRand(New HTuple(3))).TupleMult( _
                New HTuple(1.0E-20)))).TupleAbs())
        End If
        'Allow the visualization of single points or extremely small objects
        hv_ZAdd = New HTuple(0.0)
        If New HTuple(((hv_Diameter.TupleMax())).TupleLess(New HTuple(0.0000000001))).I() Then
            hv_ZAdd = New HTuple(0.01)
        End If
        'Set extremely small diameters to 1e-10 to avoid CZ == 0.0, which would lead
        'to projection errors
        If New HTuple(((hv_Diameter.TupleMin())).TupleLess(New HTuple(0.0000000001))).I() Then
            hv_Diameter = hv_Diameter.TupleSub(((((((((hv_Diameter.TupleSub(New HTuple(0.0000000001)))).TupleSgn( _
                ))).TupleSub(New HTuple(1)))).TupleSgn())).TupleMult(New HTuple(0.0000000001)))
        End If
        hv_IBB = HTuple.TupleGenSequence(New HTuple(0), ((New HTuple(hv_BB.TupleLength( _
            )))).TupleSub(New HTuple(1)), New HTuple(6))
        hv_BB0 = hv_BB.TupleSelect(hv_IBB)
        hv_BB1 = hv_BB.TupleSelect(hv_IBB.TupleAdd(New HTuple(1)))
        hv_BB2 = hv_BB.TupleSelect(hv_IBB.TupleAdd(New HTuple(2)))
        hv_BB3 = hv_BB.TupleSelect(hv_IBB.TupleAdd(New HTuple(3)))
        hv_BB4 = hv_BB.TupleSelect(hv_IBB.TupleAdd(New HTuple(4)))
        hv_BB5 = hv_BB.TupleSelect(hv_IBB.TupleAdd(New HTuple(5)))
        hv_X = ((((((((((((hv_BB0.TupleConcat(hv_BB3))).TupleConcat(hv_BB0))).TupleConcat( _
            hv_BB0))).TupleConcat(hv_BB3))).TupleConcat(hv_BB3))).TupleConcat(hv_BB0))).TupleConcat( _
            hv_BB3)
        hv_Y = ((((((((((((hv_BB1.TupleConcat(hv_BB1))).TupleConcat(hv_BB4))).TupleConcat( _
            hv_BB1))).TupleConcat(hv_BB4))).TupleConcat(hv_BB1))).TupleConcat(hv_BB4))).TupleConcat( _
            hv_BB4)
        hv_Z = ((((((((((((hv_BB2.TupleConcat(hv_BB2))).TupleConcat(hv_BB2))).TupleConcat( _
            hv_BB5))).TupleConcat(hv_BB2))).TupleConcat(hv_BB5))).TupleConcat(hv_BB5))).TupleConcat( _
            hv_BB5)
        hv_PoseInter = hv_PoseIn.TupleReplace(New HTuple(2), ((((hv_Z.TupleMin())).TupleNeg( _
            ))).TupleAdd((New HTuple(2)).TupleMult(hv_Diameter.TupleMax())))
        HOperatorSet.PoseToHomMat3d(hv_PoseInter, hv_HomMat3D)
        'Determine the maximum extention of the projection
        HOperatorSet.AffineTransPoint3d(hv_HomMat3D, hv_X, hv_Y, hv_Z, hv_CX, hv_CY, _
            hv_CZ)
        HOperatorSet.Project3dPoint(hv_CX, hv_CY, hv_CZ, hv_CamParam, hv_Rows, hv_Cols)
        hv_MinMinZ = hv_CZ.TupleMin()
        hv_DR = hv_Rows.TupleSub(hv_CamParam.TupleSelect(((New HTuple(hv_CamParam.TupleLength( _
            )))).TupleSub(New HTuple(3))))
        hv_DC = hv_Cols.TupleSub(hv_CamParam.TupleSelect(((New HTuple(hv_CamParam.TupleLength( _
            )))).TupleSub(New HTuple(4))))
        hv_DR = ((hv_DR.TupleMax())).TupleSub(hv_DR.TupleMin())
        hv_DC = ((hv_DC.TupleMax())).TupleSub(hv_DC.TupleMin())
        hv_MaxDist = ((((hv_DR.TupleMult(hv_DR))).TupleAdd(hv_DC.TupleMult(hv_DC)))).TupleSqrt( _
            )
        '
        If New HTuple(hv_MaxDist.TupleLess(New HTuple(0.0000000001))).I() Then
            'If the object has no extension in the above projection (looking along
            'a line), we determine the extension of the object in a rotated view
            HOperatorSet.HomMat3dRotateLocal(hv_HomMat3D, (New HTuple(90)).TupleRad(), _
                New HTuple("x"), hv_HomMat3DRotate)
            HOperatorSet.AffineTransPoint3d(hv_HomMat3DRotate, hv_X, hv_Y, hv_Z, hv_CX, _
                hv_CY, hv_CZ)
            HOperatorSet.Project3dPoint(hv_CX, hv_CY, hv_CZ, hv_CamParam, hv_Rows, hv_Cols)
            hv_DR = hv_Rows.TupleSub(hv_CamParam.TupleSelect(((New HTuple(hv_CamParam.TupleLength( _
                )))).TupleSub(New HTuple(3))))
            hv_DC = hv_Cols.TupleSub(hv_CamParam.TupleSelect(((New HTuple(hv_CamParam.TupleLength( _
                )))).TupleSub(New HTuple(4))))
            hv_DR = ((hv_DR.TupleMax())).TupleSub(hv_DR.TupleMin())
            hv_DC = ((hv_DC.TupleMax())).TupleSub(hv_DC.TupleMin())
            hv_MaxDist = ((hv_MaxDist.TupleConcat(((((hv_DR.TupleMult(hv_DR))).TupleAdd( _
                hv_DC.TupleMult(hv_DC)))).TupleSqrt()))).TupleMax()
        End If
        '
        hv_MinImageSize = ((((hv_CamParam.TupleSelect(((New HTuple(hv_CamParam.TupleLength( _
            )))).TupleSub(New HTuple(2))))).TupleConcat(hv_CamParam.TupleSelect(((New HTuple( _
            hv_CamParam.TupleLength()))).TupleSub(New HTuple(1)))))).TupleMin()
        '
        hv_Z = hv_PoseInter.TupleSelect(New HTuple(2))
        hv_Zs = hv_MinMinZ.Clone()
        hv_ZDiff = hv_Z.TupleSub(hv_Zs)
        hv_ScaleZ = hv_MaxDist.TupleDiv((((((New HTuple(0.5)).TupleMult(hv_MinImageSize))).TupleMult( _
            hv_ImageCoverage))).TupleMult(New HTuple(2.0)))
        hv_ZNew = ((((hv_ScaleZ.TupleMult(hv_Zs))).TupleAdd(hv_ZDiff))).TupleAdd(hv_ZAdd)
        hv_PoseOut = hv_PoseInter.TupleReplace(New HTuple(2), hv_ZNew)
        '

        Exit Sub
    End Sub

    ' Chapter: Graphics / Output
    ' Short Description: Renders 3d object models in a buffer window. 
    Public Sub dump_image_output(ByVal ho_BackgroundImage As HObject, ByVal hv_WindowHandleBuffer As HTuple, _
        ByVal hv_ObjectModel3DID As HTuple, ByVal hv_GenParamName As HTuple, ByVal hv_GenParamValue As HTuple, _
        ByVal hv_CamParam As HTuple, ByVal hv_Poses As HTuple, ByVal hv_ColorImage As HTuple, _
        ByVal hv_Title As HTuple, ByVal hv_Information As HTuple, ByVal hv_Labels As HTuple, _
        ByVal hv_VisualizeTrackball As HTuple, ByVal hv_DisplayContinueButton As HTuple, _
        ByVal hv_TrackballCenterRow As HTuple, ByVal hv_TrackballCenterCol As HTuple, _
        ByVal hv_TrackballRadiusPixel As HTuple, ByVal hv_SelectedObject As HTuple, _
        ByVal hv_VisualizeRotationCenter As HTuple, ByVal hv_RotationCenter As HTuple)



        ' Local iconic variables 
        Dim ho_ModelContours As HObject = Nothing
        Dim ho_Image As HObject = Nothing, ho_TrackballContour As HObject = Nothing
        Dim ho_CrossRotCenter As HObject = Nothing


        ' Local control variables 
        Dim ExpTmpLocalVar_gUsesOpenGL As HTuple = New HTuple
        Dim hv_OpenGlEnabled As HTuple = New HTuple, hv_Indices As HTuple = New HTuple
        Dim hv_Exception As HTuple = New HTuple, hv_Index As HTuple = New HTuple
        Dim hv_Position As HTuple = New HTuple, hv_PosIdx As HTuple = New HTuple
        Dim hv_Substrings As HTuple = New HTuple, hv_I As HTuple = New HTuple
        Dim hv_HasExtended As HTuple = New HTuple, hv_ExtendedAttributeNames As HTuple = New HTuple
        Dim hv_Matches As HTuple = New HTuple, hv_TransObject As HTuple = New HTuple
        Dim hv_TransAlpha As HTuple = New HTuple, hv_Sequence As HTuple = New HTuple
        Dim hv_Exception1 As HTuple = New HTuple, hv_PosesOut As HTuple = New HTuple
        Dim hv_Pose As HTuple = New HTuple, hv_HomMat3D As HTuple = New HTuple
        Dim hv_Center As HTuple = New HTuple, hv_CenterCamX As HTuple = New HTuple
        Dim hv_CenterCamY As HTuple = New HTuple, hv_CenterCamZ As HTuple = New HTuple
        Dim hv_CenterRow As HTuple = New HTuple, hv_CenterCol As HTuple = New HTuple
        Dim hv_Label As HTuple = New HTuple, hv_Ascent As HTuple = New HTuple
        Dim hv_Descent As HTuple = New HTuple, hv_TextWidth As HTuple = New HTuple
        Dim hv_TextHeight As HTuple = New HTuple, hv_RotCenterRow As HTuple = New HTuple
        Dim hv_RotCenterCol As HTuple = New HTuple, hv_Orientation As HTuple = New HTuple
        Dim hv_Colors As HTuple = New HTuple

        Dim hv_GenParamName_COPY_INP_TMP As HTuple
        hv_GenParamName_COPY_INP_TMP = hv_GenParamName.Clone()
        Dim hv_GenParamValue_COPY_INP_TMP As HTuple
        hv_GenParamValue_COPY_INP_TMP = hv_GenParamValue.Clone()
        Dim hv_RotationCenter_COPY_INP_TMP As HTuple
        hv_RotationCenter_COPY_INP_TMP = hv_RotationCenter.Clone()

        ' Initialize local and output iconic variables 
        HOperatorSet.GenEmptyObj(ho_ModelContours)
        HOperatorSet.GenEmptyObj(ho_Image)
        HOperatorSet.GenEmptyObj(ho_TrackballContour)
        HOperatorSet.GenEmptyObj(ho_CrossRotCenter)

        Try
            'global tuple gAlphaDeselected
            'global tuple gTerminationButtonLabel
            'global tuple gDispObjOffset
            'global tuple gLabelsDecor
            'global tuple gUsesOpenGL
            '
            'Display background image
            HOperatorSet.ClearWindow(hv_WindowHandleBuffer)
            If hv_ColorImage.I() Then
                HOperatorSet.DispColor(ho_BackgroundImage, hv_WindowHandleBuffer)
            Else
                HOperatorSet.DispImage(ho_BackgroundImage, hv_WindowHandleBuffer)
            End If
            '
            'Display objects
            hv_OpenGlEnabled = New HTuple("true")
            hv_Indices = hv_GenParamName_COPY_INP_TMP.TupleFind(New HTuple("opengl"))
            If ((New HTuple(hv_Indices.TupleNotEqual(New HTuple(-1))))).TupleAnd(New HTuple( _
                hv_Indices.TupleNotEqual(New HTuple()))).I() Then
                hv_OpenGlEnabled = hv_GenParamValue_COPY_INP_TMP.TupleSelect(hv_Indices.TupleSelect( _
                    New HTuple(0)))
                hv_GenParamName_COPY_INP_TMP = hv_GenParamName_COPY_INP_TMP.TupleRemove(hv_Indices)
                hv_GenParamValue_COPY_INP_TMP = hv_GenParamValue_COPY_INP_TMP.TupleRemove( _
                    hv_Indices)
            End If
            If New HTuple(((hv_SelectedObject.TupleSum())).TupleEqual(New HTuple(hv_SelectedObject.TupleLength( _
                )))).I() Then
                If New HTuple(ExpGetGlobalVar_gUsesOpenGL().TupleEqual(New HTuple("true"))).I() Then
                    If New HTuple(hv_OpenGlEnabled.TupleEqual(New HTuple("false"))).I() Then
                        ExpTmpLocalVar_gUsesOpenGL = New HTuple("false")
                        ExpSetGlobalVar_gUsesOpenGL(ExpTmpLocalVar_gUsesOpenGL)
                    End If
                    Try
                        HOperatorSet.DispObjectModel3d(hv_WindowHandleBuffer, hv_ObjectModel3DID, _
                            hv_CamParam, hv_Poses, hv_GenParamName_COPY_INP_TMP, hv_GenParamValue_COPY_INP_TMP)
                        ' catch (Exception) 
                    Catch HDevExpDefaultException1 As HalconException
                        HDevExpDefaultException1.ToHTuple(hv_Exception)
                        If ((((((New HTuple(((hv_Exception.TupleSelect(New HTuple(0)))).TupleEqual( _
                            New HTuple(1306))))).TupleOr(New HTuple(((hv_Exception.TupleSelect( _
                            New HTuple(0)))).TupleEqual(New HTuple(1305)))))).TupleOr(New HTuple(( _
                            (hv_Exception.TupleSelect(New HTuple(0)))).TupleEqual(New HTuple(1406)))))).TupleOr( _
                            New HTuple(((hv_Exception.TupleSelect(New HTuple(0)))).TupleEqual( _
                            New HTuple(1405)))).I() Then
                            If New HTuple(((New HTuple(hv_GenParamName_COPY_INP_TMP.TupleLength( _
                                )))).TupleEqual(New HTuple(hv_GenParamValue_COPY_INP_TMP.TupleLength( _
                                )))).I() Then
                                'This case means we have a Parameter with structure parameter_x with x > |ObjectModel3DID|-1
                                For hv_Index = New HTuple( _
                                    hv_ObjectModel3DID.TupleLength()) To (((New HTuple(2)).TupleMult( _
                                    New HTuple(hv_ObjectModel3DID.TupleLength())))).TupleAdd(New HTuple(1)) Step (New HTuple(1))
                                    HOperatorSet.TupleStrstr(hv_GenParamName_COPY_INP_TMP, (New HTuple("")).TupleAdd( _
                                        hv_Index), hv_Position)
                                    For hv_PosIdx = (New HTuple(0)) To ( _
                                        (New HTuple(hv_Position.TupleLength()))).TupleSub(New HTuple(1)) Step (New HTuple(1))
                                        If New HTuple(((hv_Position.TupleSelect(hv_PosIdx))).TupleNotEqual( _
                                            New HTuple(-1))).I() Then
                                            Throw New HalconException(( _
                                                ((((New HTuple("One of the parameters is refferring to a non-existing object model 3D:" + Chr(10))).TupleAdd( _
                                                hv_GenParamName_COPY_INP_TMP.TupleSelect(hv_PosIdx)))).TupleAdd( _
                                                New HTuple(" -> ")))).TupleAdd(hv_GenParamValue_COPY_INP_TMP.TupleSelect( _
                                                hv_PosIdx)))
                                        End If
#If USE_DO_EVENTS Then
                    ' Please note: The call of DoEvents() is only a hack to
                    ' enable VB to react on events. Please change the code
                    ' so that it can handle events in a standard way.
                    System.Windows.Forms.Application.DoEvents()
#End If
                                    Next
#If USE_DO_EVENTS Then
                  ' Please note: The call of DoEvents() is only a hack to
                  ' enable VB to react on events. Please change the code
                  ' so that it can handle events in a standard way.
                  System.Windows.Forms.Application.DoEvents()
#End If
                                Next
                                'Test for non-existing extended attributes:
                                HOperatorSet.TupleStrstr(hv_GenParamName_COPY_INP_TMP, New HTuple("intensity"), _
                                    hv_Position)
                                For hv_PosIdx = (New HTuple(0)) To ( _
                                    (New HTuple(hv_Position.TupleLength()))).TupleSub(New HTuple(1)) Step (New HTuple(1))
                                    If New HTuple(((hv_Position.TupleSelect(hv_PosIdx))).TupleNotEqual( _
                                        New HTuple(-1))).I() Then
                                        HOperatorSet.TupleSplit(hv_GenParamName_COPY_INP_TMP.TupleSelect( _
                                            hv_PosIdx), New HTuple("_"), hv_Substrings)
                                        If ((New HTuple(((New HTuple(hv_Substrings.TupleLength()))).TupleGreater( _
                                            New HTuple(1))))).TupleAnd(((hv_Substrings.TupleSelect(New HTuple(1)))).TupleIsNumber( _
                                            )).I() Then
                                            hv_I = ((hv_Substrings.TupleSelect(New HTuple(1)))).TupleNumber( _
                                                )
                                            HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID.TupleSelect( _
                                                hv_I), New HTuple("has_extended_attribute"), hv_HasExtended)
                                            If hv_HasExtended.I() Then
                                                HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID.TupleSelect( _
                                                    hv_I), New HTuple("extended_attribute_names"), hv_ExtendedAttributeNames)
                                                HOperatorSet.TupleFind(hv_ExtendedAttributeNames, hv_GenParamValue_COPY_INP_TMP.TupleSelect( _
                                                    hv_PosIdx), hv_Matches)
                                            End If
                                            If ((hv_HasExtended.TupleNot())).TupleOr(((New HTuple(hv_Matches.TupleEqual( _
                                                New HTuple(-1))))).TupleOr(New HTuple(((New HTuple(hv_Matches.TupleLength( _
                                                )))).TupleEqual(New HTuple(0))))).I() Then
                                                Throw New HalconException(( _
                                                    ((((((((New HTuple("One of the parameters is refferring to an extended attribute that is not contained in the object model 3d with the handle ")).TupleAdd( _
                                                    hv_ObjectModel3DID.TupleSelect(hv_I)))).TupleAdd(New HTuple(":" + Chr(10))))).TupleAdd( _
                                                    hv_GenParamName_COPY_INP_TMP.TupleSelect(hv_PosIdx)))).TupleAdd( _
                                                    New HTuple(" -> ")))).TupleAdd(hv_GenParamValue_COPY_INP_TMP.TupleSelect( _
                                                    hv_PosIdx)))
                                            End If
                                        Else
                                            For hv_I = (New HTuple(0)) To ( _
                                                (New HTuple(hv_ObjectModel3DID.TupleLength()))).TupleSub( _
                                                New HTuple(1)) Step (New HTuple(1))
                                                HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID.TupleSelect( _
                                                    hv_I), New HTuple("extended_attribute_names"), hv_ExtendedAttributeNames)
                                                HOperatorSet.TupleFind(hv_ExtendedAttributeNames, hv_GenParamValue_COPY_INP_TMP.TupleSelect( _
                                                    hv_PosIdx), hv_Matches)
                                                If ((New HTuple(hv_Matches.TupleEqual(New HTuple(-1))))).TupleOr( _
                                                    New HTuple(((New HTuple(hv_Matches.TupleLength()))).TupleEqual( _
                                                    New HTuple(0)))).I() Then
                                                    Throw New HalconException(( _
                                                        ((((New HTuple("One of the parameters is refferring to an extended attribute that is not contained in all object models:" + Chr(10))).TupleAdd( _
                                                        hv_GenParamName_COPY_INP_TMP.TupleSelect(hv_PosIdx)))).TupleAdd( _
                                                        New HTuple(" -> ")))).TupleAdd(hv_GenParamValue_COPY_INP_TMP.TupleSelect( _
                                                        hv_PosIdx)))
                                                End If
#If USE_DO_EVENTS Then
                        ' Please note: The call of DoEvents() is only a hack to
                        ' enable VB to react on events. Please change the code
                        ' so that it can handle events in a standard way.
                        System.Windows.Forms.Application.DoEvents()
#End If
                                            Next
                                        End If
                                    End If
#If USE_DO_EVENTS Then
                  ' Please note: The call of DoEvents() is only a hack to
                  ' enable VB to react on events. Please change the code
                  ' so that it can handle events in a standard way.
                  System.Windows.Forms.Application.DoEvents()
#End If
                                Next
                                '
                                Throw New HalconException(( _
                                    ((New HTuple("Wrong generic parameters for display" + Chr(10))).TupleAdd( _
                                    New HTuple("Wrong Values are:" + Chr(10))))).TupleAdd(((((((( _
                                    ((New HTuple("    ")).TupleAdd(((hv_GenParamName_COPY_INP_TMP.TupleAdd( _
                                    New HTuple(" -> ")))).TupleAdd(hv_GenParamValue_COPY_INP_TMP)))).TupleAdd( _
                                    New HTuple("" + Chr(10))))).TupleSum())).TupleAdd(New HTuple("Exeption was:" + Chr(10) + "    ")))).TupleAdd( _
                                    hv_Exception.TupleSelect(New HTuple(2)))))
                            Else
                                Throw New HalconException(hv_Exception)
                            End If
                        ElseIf ((((New HTuple(((hv_Exception.TupleSelect(New HTuple(0)))).TupleEqual( _
                            New HTuple(5185))))).TupleOr(New HTuple(((hv_Exception.TupleSelect( _
                            New HTuple(0)))).TupleEqual(New HTuple(5188)))))).TupleOr(New HTuple(( _
                            (hv_Exception.TupleSelect(New HTuple(0)))).TupleEqual(New HTuple(5187)))).I() Then
                            ExpTmpLocalVar_gUsesOpenGL = New HTuple("false")
                            ExpSetGlobalVar_gUsesOpenGL(ExpTmpLocalVar_gUsesOpenGL)
                        Else
                            Throw New HalconException(hv_Exception)
                        End If
                    End Try
                End If
                If New HTuple(ExpGetGlobalVar_gUsesOpenGL().TupleEqual(New HTuple("false"))).I() Then
                    '* NO OpenGL, use fallback
                    ho_ModelContours.Dispose()
                    disp_object_model_no_opengl(ho_ModelContours, hv_ObjectModel3DID, hv_GenParamName_COPY_INP_TMP, _
                        hv_GenParamValue_COPY_INP_TMP, hv_WindowHandleBuffer, hv_CamParam, _
                        hv_Poses)
                End If
            Else
                hv_TransObject = New HTuple()
                hv_TransAlpha = New HTuple()
                hv_Sequence = HTuple.TupleGenSequence(New HTuple(0), ((New HTuple(hv_ObjectModel3DID.TupleLength( _
                    )))).TupleSub(New HTuple(1)), New HTuple(1))
                hv_TransObject = (New HTuple("alpha_")).TupleAdd(hv_Sequence)
                If New HTuple(((hv_SelectedObject.TupleSum())).TupleGreater(((New HTuple( _
                    hv_SelectedObject.TupleLength()))).TupleDiv(New HTuple(2)))).I() Then
                    hv_TransAlpha = HTuple.TupleGenConst(New HTuple(hv_Sequence.TupleLength( _
                        )), New HTuple(1.0))
                    hv_Indices = hv_SelectedObject.TupleFind(New HTuple(0))
                    If New HTuple(((hv_Indices.TupleSelect(New HTuple(0)))).TupleGreaterEqual( _
                        New HTuple(0))).I() Then
                        For hv_Index = (New HTuple(0)) To ( _
                            (New HTuple(hv_Indices.TupleLength()))).TupleSub(New HTuple(1)) Step (New HTuple(1))
                            If IsNothing(hv_TransAlpha) Then
                                hv_TransAlpha = New HTuple
                            End If
                            hv_TransAlpha(hv_Indices.TupleSelect(hv_Index)) = ExpGetGlobalVar_gAlphaDeselected()
#If USE_DO_EVENTS Then
              ' Please note: The call of DoEvents() is only a hack to
              ' enable VB to react on events. Please change the code
              ' so that it can handle events in a standard way.
              System.Windows.Forms.Application.DoEvents()
#End If
                        Next
                    End If
                Else
                    hv_TransAlpha = HTuple.TupleGenConst(New HTuple(hv_Sequence.TupleLength( _
                        )), ExpGetGlobalVar_gAlphaDeselected())
                    hv_Indices = hv_SelectedObject.TupleFind(New HTuple(1))
                    If New HTuple(((hv_Indices.TupleSelect(New HTuple(0)))).TupleGreaterEqual( _
                        New HTuple(0))).I() Then
                        For hv_Index = (New HTuple(0)) To ( _
                            (New HTuple(hv_Indices.TupleLength()))).TupleSub(New HTuple(1)) Step (New HTuple(1))
                            If IsNothing(hv_TransAlpha) Then
                                hv_TransAlpha = New HTuple
                            End If
                            hv_TransAlpha(hv_Indices.TupleSelect(hv_Index)) = New HTuple(1.0)
#If USE_DO_EVENTS Then
              ' Please note: The call of DoEvents() is only a hack to
              ' enable VB to react on events. Please change the code
              ' so that it can handle events in a standard way.
              System.Windows.Forms.Application.DoEvents()
#End If
                        Next
                    End If
                End If
                Try
                    If New HTuple(hv_OpenGlEnabled.TupleEqual(New HTuple("false"))).I() Then
                        Throw New HalconException(New HTuple())
                    End If
                    HOperatorSet.DispObjectModel3d(hv_WindowHandleBuffer, hv_ObjectModel3DID, _
                        hv_CamParam, hv_Poses, hv_GenParamName_COPY_INP_TMP.TupleConcat(hv_TransObject), _
                        hv_GenParamValue_COPY_INP_TMP.TupleConcat(hv_TransAlpha))
                    ' catch (Exception1) 
                Catch HDevExpDefaultException1 As HalconException
                    HDevExpDefaultException1.ToHTuple(hv_Exception1)
                    '* NO OpenGL, use fallback
                    ho_ModelContours.Dispose()
                    disp_object_model_no_opengl(ho_ModelContours, hv_ObjectModel3DID, hv_GenParamName_COPY_INP_TMP, _
                        hv_GenParamValue_COPY_INP_TMP, hv_WindowHandleBuffer, hv_CamParam, _
                        hv_PosesOut)
                End Try
            End If
            ho_Image.Dispose()
            HOperatorSet.DumpWindowImage(ho_Image, hv_WindowHandleBuffer)
            '
            'Display labels
            If New HTuple(hv_Labels.TupleNotEqual(New HTuple(0))).I() Then
                HOperatorSet.SetColor(hv_WindowHandleBuffer, ExpGetGlobalVar_gLabelsDecor().TupleSelect( _
                    New HTuple(0)))
                For hv_Index = (New HTuple(0)) To ((New HTuple( _
                    hv_ObjectModel3DID.TupleLength()))).TupleSub(New HTuple(1)) Step (New HTuple(1))
                    'Project the center point of the current model
                    hv_Pose = hv_Poses.TupleSelectRange(hv_Index.TupleMult(New HTuple(7)), ( _
                        (hv_Index.TupleMult(New HTuple(7)))).TupleAdd(New HTuple(6)))
                    HOperatorSet.PoseToHomMat3d(hv_Pose, hv_HomMat3D)
                    HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID.TupleSelect(hv_Index), _
                        New HTuple("center"), hv_Center)
                    HOperatorSet.AffineTransPoint3d(hv_HomMat3D, hv_Center.TupleSelect(New HTuple(0)), _
                        hv_Center.TupleSelect(New HTuple(1)), hv_Center.TupleSelect(New HTuple(2)), _
                        hv_CenterCamX, hv_CenterCamY, hv_CenterCamZ)
                    HOperatorSet.Project3dPoint(hv_CenterCamX, hv_CenterCamY, hv_CenterCamZ, _
                        hv_CamParam, hv_CenterRow, hv_CenterCol)
                    hv_Label = hv_Labels.TupleSelect(hv_Index)
                    If New HTuple(hv_Label.TupleNotEqual(New HTuple(""))).I() Then
                        HOperatorSet.GetStringExtents(hv_WindowHandleBuffer, hv_Label, hv_Ascent, _
                            hv_Descent, hv_TextWidth, hv_TextHeight)
                        disp_message(hv_WindowHandleBuffer, hv_Label, New HTuple("window"), ( _
                            (hv_CenterRow.TupleSub(hv_TextHeight.TupleDiv(New HTuple(2))))).TupleAdd( _
                            ExpGetGlobalVar_gDispObjOffset().TupleSelect(New HTuple(0))), ((hv_CenterCol.TupleSub( _
                            hv_TextWidth.TupleDiv(New HTuple(2))))).TupleAdd(ExpGetGlobalVar_gDispObjOffset().TupleSelect( _
                            New HTuple(1))), New HTuple(), ExpGetGlobalVar_gLabelsDecor().TupleSelect( _
                            New HTuple(1)))
                    End If
#If USE_DO_EVENTS Then
          ' Please note: The call of DoEvents() is only a hack to
          ' enable VB to react on events. Please change the code
          ' so that it can handle events in a standard way.
          System.Windows.Forms.Application.DoEvents()
#End If
                Next
            End If
            '
            'Visualize the trackball if desired
            If hv_VisualizeTrackball.I() Then
                HOperatorSet.SetLineWidth(hv_WindowHandleBuffer, New HTuple(1))
                ho_TrackballContour.Dispose()
                HOperatorSet.GenEllipseContourXld(ho_TrackballContour, hv_TrackballCenterRow, _
                    hv_TrackballCenterCol, New HTuple(0), hv_TrackballRadiusPixel, hv_TrackballRadiusPixel, _
                    New HTuple(0), New HTuple(6.28318), New HTuple("positive"), New HTuple(1.5))
                HOperatorSet.SetColor(hv_WindowHandleBuffer, New HTuple("dim gray"))
                HOperatorSet.DispXld(ho_TrackballContour, hv_WindowHandleBuffer)
            End If
            '
            'Visualize the rotation center if desired
            If ((New HTuple(hv_VisualizeRotationCenter.TupleNotEqual(New HTuple(0))))).TupleAnd( _
                New HTuple(((New HTuple(hv_RotationCenter_COPY_INP_TMP.TupleLength()))).TupleEqual( _
                New HTuple(3)))).I() Then
                If New HTuple(((hv_RotationCenter_COPY_INP_TMP.TupleSelect(New HTuple(2)))).TupleLess( _
                    New HTuple(0.0000000001))).I() Then
                    If IsNothing(hv_RotationCenter_COPY_INP_TMP) Then
                        hv_RotationCenter_COPY_INP_TMP = New HTuple
                    End If
                    hv_RotationCenter_COPY_INP_TMP(New HTuple(2)) = New HTuple(0.0000000001)
                End If
                HOperatorSet.Project3dPoint(hv_RotationCenter_COPY_INP_TMP.TupleSelect(New HTuple(0)), _
                    hv_RotationCenter_COPY_INP_TMP.TupleSelect(New HTuple(1)), hv_RotationCenter_COPY_INP_TMP.TupleSelect( _
                    New HTuple(2)), hv_CamParam, hv_RotCenterRow, hv_RotCenterCol)
                hv_Orientation = (New HTuple(90)).TupleRad()
                If New HTuple(hv_VisualizeRotationCenter.TupleEqual(New HTuple(1))).I() Then
                    hv_Orientation = (New HTuple(45)).TupleRad()
                End If
                ho_CrossRotCenter.Dispose()
                HOperatorSet.GenCrossContourXld(ho_CrossRotCenter, hv_RotCenterRow, hv_RotCenterCol, _
                    hv_TrackballRadiusPixel.TupleDiv(New HTuple(25.0)), hv_Orientation)
                HOperatorSet.SetLineWidth(hv_WindowHandleBuffer, New HTuple(3))
                HOperatorSet.QueryColor(hv_WindowHandleBuffer, hv_Colors)
                HOperatorSet.SetColor(hv_WindowHandleBuffer, New HTuple("light gray"))
                HOperatorSet.DispXld(ho_CrossRotCenter, hv_WindowHandleBuffer)
                HOperatorSet.SetLineWidth(hv_WindowHandleBuffer, New HTuple(1))
                HOperatorSet.SetColor(hv_WindowHandleBuffer, New HTuple("dim gray"))
                HOperatorSet.DispXld(ho_CrossRotCenter, hv_WindowHandleBuffer)
            End If
            '
            'Display title
            disp_title_and_information(hv_WindowHandleBuffer, hv_Title, hv_Information)
            '
            'Display the 'Exit' button
            If New HTuple(hv_DisplayContinueButton.TupleEqual(New HTuple("true"))).I() Then
                disp_continue_button(hv_WindowHandleBuffer)
            End If
            '
            ho_ModelContours.Dispose()
            ho_Image.Dispose()
            ho_TrackballContour.Dispose()
            ho_CrossRotCenter.Dispose()

            Exit Sub
        Catch HDevExpDefaultException As HalconException
            ho_ModelContours.Dispose()
            ho_Image.Dispose()
            ho_TrackballContour.Dispose()
            ho_CrossRotCenter.Dispose()

            Throw HDevExpDefaultException
        End Try
    End Sub

    ' Chapter: Graphics / Output
    ' Short Description: Get the center of the virtual trackback that is used to move the camera. 
    Public Sub get_trackball_center(ByVal hv_SelectedObject As HTuple, ByVal hv_TrackballRadiusPixel As HTuple, _
        ByVal hv_ObjectModel3D As HTuple, ByVal hv_Poses As HTuple, ByRef hv_TBCenter As HTuple, _
        ByRef hv_TBSize As HTuple)


        ' Local control variables 
        Dim hv_NumModels As HTuple = New HTuple, hv_Centers As HTuple = New HTuple
        Dim hv_Diameter As HTuple = New HTuple, hv_MD As HTuple = New HTuple
        Dim hv_Weight As HTuple = New HTuple, hv_SumW As HTuple = New HTuple
        Dim hv_Index As HTuple = New HTuple, hv_ObjectModel3DIDSelected As HTuple = New HTuple
        Dim hv_PoseSelected As HTuple = New HTuple, hv_HomMat3D As HTuple = New HTuple
        Dim hv_TBCenterCamX As HTuple = New HTuple, hv_TBCenterCamY As HTuple = New HTuple
        Dim hv_TBCenterCamZ As HTuple = New HTuple, hv_InvSum As HTuple = New HTuple

        ' Initialize local and output iconic variables 

        hv_NumModels = New HTuple(hv_ObjectModel3D.TupleLength())
        If IsNothing(hv_TBCenter) Then
            hv_TBCenter = New HTuple
        End If
        hv_TBCenter(New HTuple(0)) = New HTuple(0)
        If IsNothing(hv_TBCenter) Then
            hv_TBCenter = New HTuple
        End If
        hv_TBCenter(New HTuple(1)) = New HTuple(0)
        If IsNothing(hv_TBCenter) Then
            hv_TBCenter = New HTuple
        End If
        hv_TBCenter(New HTuple(2)) = New HTuple(0)
        HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3D, New HTuple("center"), hv_Centers)
        HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3D, New HTuple("diameter_axis_aligned_bounding_box"), _
            hv_Diameter)
        'Normalize Diameter to use it as weights for a weighted mean of the individual centers
        hv_MD = hv_Diameter.TupleMean()
        If New HTuple(hv_MD.TupleGreater(New HTuple(0.0000000001))).I() Then
            hv_Weight = hv_Diameter.TupleDiv(hv_MD)
        Else
            hv_Weight = hv_Diameter.Clone()
        End If
        hv_SumW = ((hv_Weight.TupleSelectMask(((hv_SelectedObject.TupleSgn())).TupleAbs( _
            )))).TupleSum()
        If New HTuple(hv_SumW.TupleLess(New HTuple(0.0000000001))).I() Then
            hv_Weight = HTuple.TupleGenConst(New HTuple(hv_Weight.TupleLength()), New HTuple(1.0))
            hv_SumW = ((hv_Weight.TupleSelectMask(((hv_SelectedObject.TupleSgn())).TupleAbs( _
                )))).TupleSum()
        End If
        For hv_Index = (New HTuple(0)) To hv_NumModels.TupleSub( _
            New HTuple(1)) Step (New HTuple(1))
            If hv_SelectedObject.TupleSelect(hv_Index).I() Then
                hv_ObjectModel3DIDSelected = hv_ObjectModel3D.TupleSelect(hv_Index)
                hv_PoseSelected = hv_Poses.TupleSelectRange(hv_Index.TupleMult(New HTuple(7)), _
                    ((hv_Index.TupleMult(New HTuple(7)))).TupleAdd(New HTuple(6)))
                HOperatorSet.PoseToHomMat3d(hv_PoseSelected, hv_HomMat3D)
                HOperatorSet.AffineTransPoint3d(hv_HomMat3D, hv_Centers.TupleSelect(((hv_Index.TupleMult( _
                    New HTuple(3)))).TupleAdd(New HTuple(0))), hv_Centers.TupleSelect(((hv_Index.TupleMult( _
                    New HTuple(3)))).TupleAdd(New HTuple(1))), hv_Centers.TupleSelect(((hv_Index.TupleMult( _
                    New HTuple(3)))).TupleAdd(New HTuple(2))), hv_TBCenterCamX, hv_TBCenterCamY, _
                    hv_TBCenterCamZ)
                If IsNothing(hv_TBCenter) Then
                    hv_TBCenter = New HTuple
                End If
                hv_TBCenter(New HTuple(0)) = ((hv_TBCenter.TupleSelect(New HTuple(0)))).TupleAdd( _
                    hv_TBCenterCamX.TupleMult(hv_Weight.TupleSelect(hv_Index)))
                If IsNothing(hv_TBCenter) Then
                    hv_TBCenter = New HTuple
                End If
                hv_TBCenter(New HTuple(1)) = ((hv_TBCenter.TupleSelect(New HTuple(1)))).TupleAdd( _
                    hv_TBCenterCamY.TupleMult(hv_Weight.TupleSelect(hv_Index)))
                If IsNothing(hv_TBCenter) Then
                    hv_TBCenter = New HTuple
                End If
                hv_TBCenter(New HTuple(2)) = ((hv_TBCenter.TupleSelect(New HTuple(2)))).TupleAdd( _
                    hv_TBCenterCamZ.TupleMult(hv_Weight.TupleSelect(hv_Index)))
            End If
#If USE_DO_EVENTS Then
      ' Please note: The call of DoEvents() is only a hack to
      ' enable VB to react on events. Please change the code
      ' so that it can handle events in a standard way.
      System.Windows.Forms.Application.DoEvents()
#End If
        Next
        If New HTuple(((hv_SelectedObject.TupleMax())).TupleNotEqual(New HTuple(0))).I() Then
            hv_InvSum = (New HTuple(1.0)).TupleDiv(hv_SumW)
            If IsNothing(hv_TBCenter) Then
                hv_TBCenter = New HTuple
            End If
            hv_TBCenter(New HTuple(0)) = ((hv_TBCenter.TupleSelect(New HTuple(0)))).TupleMult( _
                hv_InvSum)
            If IsNothing(hv_TBCenter) Then
                hv_TBCenter = New HTuple
            End If
            hv_TBCenter(New HTuple(1)) = ((hv_TBCenter.TupleSelect(New HTuple(1)))).TupleMult( _
                hv_InvSum)
            If IsNothing(hv_TBCenter) Then
                hv_TBCenter = New HTuple
            End If
            hv_TBCenter(New HTuple(2)) = ((hv_TBCenter.TupleSelect(New HTuple(2)))).TupleMult( _
                hv_InvSum)
            hv_TBSize = (((New HTuple(0.5)).TupleAdd((((New HTuple(0.5)).TupleMult(hv_SelectedObject.TupleSum( _
                )))).TupleDiv(hv_NumModels)))).TupleMult(hv_TrackballRadiusPixel)
        Else
            hv_TBCenter = New HTuple()
            hv_TBSize = New HTuple(0)
        End If

        Exit Sub
    End Sub

    ' Chapter: Graphics / Output
    ' Short Description: Project an image point onto the trackball 
    Public Sub project_point_on_trackball(ByVal hv_X As HTuple, ByVal hv_Y As HTuple, _
        ByVal hv_VirtualTrackball As HTuple, ByVal hv_TrackballSize As HTuple, ByRef hv_V As HTuple)


        ' Local control variables 
        Dim hv_R As HTuple = New HTuple, hv_XP As HTuple = New HTuple
        Dim hv_YP As HTuple = New HTuple, hv_ZP As HTuple = New HTuple

        ' Initialize local and output iconic variables 

        If New HTuple(hv_VirtualTrackball.TupleEqual(New HTuple("shoemake"))).I() Then
            'Virtual Trackball according to Shoemake
            hv_R = ((((hv_X.TupleMult(hv_X))).TupleAdd(hv_Y.TupleMult(hv_Y)))).TupleSqrt( _
                )
            If New HTuple(hv_R.TupleLessEqual(hv_TrackballSize)).I() Then
                hv_XP = hv_X.Clone()
                hv_YP = hv_Y.Clone()
                hv_ZP = ((((hv_TrackballSize.TupleMult(hv_TrackballSize))).TupleSub(hv_R.TupleMult( _
                    hv_R)))).TupleSqrt()
            Else
                hv_XP = ((hv_X.TupleMult(hv_TrackballSize))).TupleDiv(hv_R)
                hv_YP = ((hv_Y.TupleMult(hv_TrackballSize))).TupleDiv(hv_R)
                hv_ZP = New HTuple(0)
            End If
        Else
            'Virtual Trackball according to Bell
            hv_R = ((((hv_X.TupleMult(hv_X))).TupleAdd(hv_Y.TupleMult(hv_Y)))).TupleSqrt( _
                )
            If New HTuple(hv_R.TupleLessEqual(hv_TrackballSize.TupleMult(New HTuple(0.70710678)))).I() Then
                hv_XP = hv_X.Clone()
                hv_YP = hv_Y.Clone()
                hv_ZP = ((((hv_TrackballSize.TupleMult(hv_TrackballSize))).TupleSub(hv_R.TupleMult( _
                    hv_R)))).TupleSqrt()
            Else
                hv_XP = hv_X.Clone()
                hv_YP = hv_Y.Clone()
                hv_ZP = (((((New HTuple(0.6)).TupleMult(hv_TrackballSize))).TupleMult(hv_TrackballSize))).TupleDiv( _
                    hv_R)
            End If
        End If
        hv_V = ((hv_XP.TupleConcat(hv_YP))).TupleConcat(hv_ZP)

        Exit Sub
    End Sub

    ' Chapter: Graphics / Output
    ' Short Description: Get string extends of several lines. 
    Public Sub max_line_width(ByVal hv_WindowHandle As HTuple, ByVal hv_Lines As HTuple, _
        ByRef hv_MaxWidth As HTuple)


        ' Local control variables 
        Dim hv_Index As HTuple = New HTuple, hv_Ascent As HTuple = New HTuple
        Dim hv_Descent As HTuple = New HTuple, hv_LineWidth As HTuple = New HTuple
        Dim hv_LineHeight As HTuple = New HTuple

        ' Initialize local and output iconic variables 

        hv_MaxWidth = New HTuple(0)
        For hv_Index = (New HTuple(0)) To ((New HTuple( _
            hv_Lines.TupleLength()))).TupleSub(New HTuple(1)) Step (New HTuple(1))
            HOperatorSet.GetStringExtents(hv_WindowHandle, hv_Lines.TupleSelect(hv_Index), _
                hv_Ascent, hv_Descent, hv_LineWidth, hv_LineHeight)
            hv_MaxWidth = ((hv_LineWidth.TupleConcat(hv_MaxWidth))).TupleMax()
#If USE_DO_EVENTS Then
      ' Please note: The call of DoEvents() is only a hack to
      ' enable VB to react on events. Please change the code
      ' so that it can handle events in a standard way.
      System.Windows.Forms.Application.DoEvents()
#End If
        Next

        Exit Sub
    End Sub

    ' Chapter: Graphics / Output
    ' Short Description: Compute the 3d rotation from the mose movement 
    Public Sub trackball(ByVal hv_MX1 As HTuple, ByVal hv_MY1 As HTuple, ByVal hv_MX2 As HTuple, _
        ByVal hv_MY2 As HTuple, ByVal hv_VirtualTrackball As HTuple, ByVal hv_TrackballSize As HTuple, _
        ByVal hv_SensFactor As HTuple, ByRef hv_QuatRotation As HTuple)


        ' Local control variables 
        Dim hv_P2 As HTuple = New HTuple, hv_P1 As HTuple = New HTuple
        Dim hv_D As HTuple = New HTuple, hv_T As HTuple = New HTuple
        Dim hv_RotAngle As HTuple = New HTuple, hv_RotAxis As HTuple = New HTuple
        Dim hv_Len As HTuple = New HTuple

        ' Initialize local and output iconic variables 

        'Compute the 3d rotation from the mouse movement
        '
        If ((New HTuple(hv_MX1.TupleEqual(hv_MX2)))).TupleAnd(New HTuple(hv_MY1.TupleEqual( _
            hv_MY2))).I() Then
            hv_QuatRotation = (((New HTuple(1)).TupleConcat(0)).TupleConcat(0)).TupleConcat( _
                0)

            Exit Sub
        End If
        'Project the image point onto the trackball
        project_point_on_trackball(hv_MX1, hv_MY1, hv_VirtualTrackball, hv_TrackballSize, _
            hv_P1)
        project_point_on_trackball(hv_MX2, hv_MY2, hv_VirtualTrackball, hv_TrackballSize, _
            hv_P2)
        'The cross product of the projected points defines the rotation axis
        tuple_vector_cross_product(hv_P1, hv_P2, hv_RotAxis)
        'Compute the rotation angle
        hv_D = hv_P2.TupleSub(hv_P1)
        hv_T = ((((((hv_D.TupleMult(hv_D))).TupleSum())).TupleSqrt())).TupleDiv((New HTuple(2.0)).TupleMult( _
            hv_TrackballSize))
        If New HTuple(hv_T.TupleGreater(New HTuple(1.0))).I() Then
            hv_T = New HTuple(1.0)
        End If
        If New HTuple(hv_T.TupleLess(New HTuple(-1.0))).I() Then
            hv_T = New HTuple(-1.0)
        End If
        hv_RotAngle = (((New HTuple(2.0)).TupleMult(hv_T.TupleAsin()))).TupleMult(hv_SensFactor)
        hv_Len = ((((hv_RotAxis.TupleMult(hv_RotAxis))).TupleSum())).TupleSqrt()
        If New HTuple(hv_Len.TupleGreater(New HTuple(0.0))).I() Then
            hv_RotAxis = hv_RotAxis.TupleDiv(hv_Len)
        End If
        HOperatorSet.AxisAngleToQuat(hv_RotAxis.TupleSelect(New HTuple(0)), hv_RotAxis.TupleSelect( _
            New HTuple(1)), hv_RotAxis.TupleSelect(New HTuple(2)), hv_RotAngle, hv_QuatRotation)

        Exit Sub
    End Sub

    ' Chapter: Tuple / Arithmetic
    ' Short Description: Calculates the cross product of two vectors of length 3. 
    Public Sub tuple_vector_cross_product(ByVal hv_V1 As HTuple, ByVal hv_V2 As HTuple, _
        ByRef hv_VC As HTuple)

        ' Initialize local and output iconic variables 

        'The caller must ensure that the length of both input vectors is 3
        hv_VC = ((((hv_V1.TupleSelect(New HTuple(1)))).TupleMult(hv_V2.TupleSelect(New HTuple(2))))).TupleSub( _
            ((hv_V1.TupleSelect(New HTuple(2)))).TupleMult(hv_V2.TupleSelect(New HTuple(1))))
        hv_VC = hv_VC.TupleConcat(((((hv_V1.TupleSelect(New HTuple(2)))).TupleMult(hv_V2.TupleSelect( _
            New HTuple(0))))).TupleSub(((hv_V1.TupleSelect(New HTuple(0)))).TupleMult( _
            hv_V2.TupleSelect(New HTuple(2)))))
        hv_VC = hv_VC.TupleConcat(((((hv_V1.TupleSelect(New HTuple(0)))).TupleMult(hv_V2.TupleSelect( _
            New HTuple(1))))).TupleSub(((hv_V1.TupleSelect(New HTuple(1)))).TupleMult( _
            hv_V2.TupleSelect(New HTuple(0)))))

        Exit Sub
    End Sub

    ' Chapter: Graphics / Output
    ' Short Description: Get the center of the virtual trackback that is used to move the camera (version for inspection_mode = 'surface'). 
    Public Sub get_trackball_center_fixed(ByVal hv_SelectedObject As HTuple, ByVal hv_TrackballCenterRow As HTuple, _
        ByVal hv_TrackballCenterCol As HTuple, ByVal hv_TrackballRadiusPixel As HTuple, _
        ByVal hv_ObjectModel3DID As HTuple, ByVal hv_Poses As HTuple, ByVal hv_WindowHandleBuffer As HTuple, _
        ByVal hv_CamParam As HTuple, ByVal hv_GenParamName As HTuple, ByVal hv_GenParamValue As HTuple, _
        ByRef hv_TBCenter As HTuple, ByRef hv_TBSize As HTuple)


        ' Local iconic variables 
        Dim ho_RegionCenter As HObject = Nothing, ho_DistanceImage As HObject = Nothing
        Dim ho_Domain As HObject = Nothing


        ' Local control variables 
        Dim hv_NumModels As HTuple = New HTuple, hv_Width As HTuple = New HTuple
        Dim hv_Height As HTuple = New HTuple, hv_SelectPose As HTuple = New HTuple
        Dim hv_Index1 As HTuple = New HTuple, hv_Rows As HTuple = New HTuple
        Dim hv_Columns As HTuple = New HTuple, hv_Grayval As HTuple = New HTuple
        Dim hv_IndicesG As HTuple = New HTuple, hv_Value As HTuple = New HTuple
        Dim hv_Pos As HTuple = New HTuple

        ' Initialize local and output iconic variables 
        HOperatorSet.GenEmptyObj(ho_RegionCenter)
        HOperatorSet.GenEmptyObj(ho_DistanceImage)
        HOperatorSet.GenEmptyObj(ho_Domain)

        Try
            'Determine the trackball center for the fixed trackball
            hv_NumModels = New HTuple(hv_ObjectModel3DID.TupleLength())
            hv_Width = hv_CamParam.TupleSelect(((New HTuple(hv_CamParam.TupleLength()))).TupleSub( _
                New HTuple(2)))
            hv_Height = hv_CamParam.TupleSelect(((New HTuple(hv_CamParam.TupleLength()))).TupleSub( _
                New HTuple(1)))
            '
            'Project the selected objects
            hv_SelectPose = New HTuple()
            For hv_Index1 = (New HTuple(0)) To ((New HTuple( _
                hv_SelectedObject.TupleLength()))).TupleSub(New HTuple(1)) Step (New HTuple(1))
                hv_SelectPose = hv_SelectPose.TupleConcat(HTuple.TupleGenConst(New HTuple(7), _
                    hv_SelectedObject.TupleSelect(hv_Index1)))
#If USE_DO_EVENTS Then
        ' Please note: The call of DoEvents() is only a hack to
        ' enable VB to react on events. Please change the code
        ' so that it can handle events in a standard way.
        System.Windows.Forms.Application.DoEvents()
#End If
            Next
            HOperatorSet.DispObjectModel3d(hv_WindowHandleBuffer, hv_ObjectModel3DID.TupleSelectMask( _
                hv_SelectedObject), hv_CamParam, hv_Poses.TupleSelectMask(hv_SelectPose), _
                hv_GenParamName.TupleConcat(New HTuple("depth_persistence")), hv_GenParamValue.TupleConcat( _
                New HTuple("true")))
            '
            'determine the depth of the object point that appears closest to the trackball
            'center
            ho_RegionCenter.Dispose()
            HOperatorSet.GenRegionPoints(ho_RegionCenter, hv_TrackballCenterRow, hv_TrackballCenterCol)
            ho_DistanceImage.Dispose()
            HOperatorSet.DistanceTransform(ho_RegionCenter, ho_DistanceImage, New HTuple("chamfer-3-4-unnormalized"), _
                New HTuple("false"), hv_Width, hv_Height)
            ho_Domain.Dispose()
            HOperatorSet.GetDomain(ho_DistanceImage, ho_Domain)
            HOperatorSet.GetRegionPoints(ho_Domain, hv_Rows, hv_Columns)
            HOperatorSet.GetGrayval(ho_DistanceImage, hv_Rows, hv_Columns, hv_Grayval)
            HOperatorSet.TupleSortIndex(hv_Grayval, hv_IndicesG)
            HOperatorSet.GetDispObjectModel3dInfo(hv_WindowHandleBuffer, hv_Rows.TupleSelect( _
                hv_IndicesG), hv_Columns.TupleSelect(hv_IndicesG), New HTuple("depth"), _
                hv_Value)
            HOperatorSet.TupleFind(hv_Value.TupleSgn(), New HTuple(1), hv_Pos)
            '
            'set TBCenter
            If New HTuple(hv_Pos.TupleNotEqual(New HTuple(-1))).I() Then
                'if the object is visible in the image
                hv_TBCenter = ((New HTuple(0)).TupleConcat(0)).TupleConcat(hv_Value.TupleSelect( _
                    hv_Pos.TupleSelect(New HTuple(0))))
            Else
                'if the object is not visible in the image, set the z coordinate to -1
                'to indicate, the the previous z value should be used instead
                hv_TBCenter = ((New HTuple(0)).TupleConcat(0)).TupleConcat(-1)
            End If
            '
            If New HTuple(((hv_SelectedObject.TupleMax())).TupleNotEqual(New HTuple(0))).I() Then
                hv_TBSize = (((New HTuple(0.5)).TupleAdd((((New HTuple(0.5)).TupleMult(hv_SelectedObject.TupleSum( _
                    )))).TupleDiv(hv_NumModels)))).TupleMult(hv_TrackballRadiusPixel)
            Else
                hv_TBCenter = New HTuple()
                hv_TBSize = New HTuple(0)
            End If
            ho_RegionCenter.Dispose()
            ho_DistanceImage.Dispose()
            ho_Domain.Dispose()

            Exit Sub
        Catch HDevExpDefaultException As HalconException
            ho_RegionCenter.Dispose()
            ho_DistanceImage.Dispose()
            ho_Domain.Dispose()

            Throw HDevExpDefaultException
        End Try
    End Sub

    ' Chapter: Graphics / Text
    ' Short Description: This procedure writes a text message. 
    Public Sub disp_text_button(ByVal hv_WindowHandle As HTuple, ByVal hv_String As HTuple, _
        ByVal hv_CoordSystem As HTuple, ByVal hv_Row As HTuple, ByVal hv_Column As HTuple, _
        ByVal hv_TextColor As HTuple, ByVal hv_ButtonColor As HTuple)


        ' Local iconic variables 
        Dim ho_UpperLeft As HObject = Nothing, ho_LowerRight As HObject = Nothing
        Dim ho_Rectangle As HObject = Nothing


        ' Local control variables 
        Dim hv_Red As HTuple = New HTuple, hv_Green As HTuple = New HTuple
        Dim hv_Blue As HTuple = New HTuple, hv_Row1Part As HTuple = New HTuple
        Dim hv_Column1Part As HTuple = New HTuple, hv_Row2Part As HTuple = New HTuple
        Dim hv_Column2Part As HTuple = New HTuple, hv_RowWin As HTuple = New HTuple
        Dim hv_ColumnWin As HTuple = New HTuple, hv_WidthWin As HTuple = New HTuple
        Dim hv_HeightWin As HTuple = New HTuple, hv_MaxAscent As HTuple = New HTuple
        Dim hv_MaxDescent As HTuple = New HTuple, hv_MaxWidth As HTuple = New HTuple
        Dim hv_MaxHeight As HTuple = New HTuple, hv_R1 As HTuple = New HTuple
        Dim hv_C1 As HTuple = New HTuple, hv_FactorRow As HTuple = New HTuple
        Dim hv_FactorColumn As HTuple = New HTuple, hv_Width As HTuple = New HTuple
        Dim hv_Index As HTuple = New HTuple, hv_Ascent As HTuple = New HTuple
        Dim hv_Descent As HTuple = New HTuple, hv_W As HTuple = New HTuple
        Dim hv_H As HTuple = New HTuple, hv_FrameHeight As HTuple = New HTuple
        Dim hv_FrameWidth As HTuple = New HTuple, hv_R2 As HTuple = New HTuple
        Dim hv_C2 As HTuple = New HTuple, hv_ClipRegion As HTuple = New HTuple
        Dim hv_DrawMode As HTuple = New HTuple, hv_CurrentColor As HTuple = New HTuple

        Dim hv_Column_COPY_INP_TMP As HTuple
        hv_Column_COPY_INP_TMP = hv_Column.Clone()
        Dim hv_Row_COPY_INP_TMP As HTuple
        hv_Row_COPY_INP_TMP = hv_Row.Clone()
        Dim hv_String_COPY_INP_TMP As HTuple
        hv_String_COPY_INP_TMP = hv_String.Clone()
        Dim hv_TextColor_COPY_INP_TMP As HTuple
        hv_TextColor_COPY_INP_TMP = hv_TextColor.Clone()

        ' Initialize local and output iconic variables 
        HOperatorSet.GenEmptyObj(ho_UpperLeft)
        HOperatorSet.GenEmptyObj(ho_LowerRight)
        HOperatorSet.GenEmptyObj(ho_Rectangle)

        Try
            'This procedure displays text in a graphics window.
            '
            'Input parameters:
            'WindowHandle: The WindowHandle of the graphics window, where
            '   the message should be displayed
            'String: A tuple of strings containing the text message to be displayed
            'CoordSystem: If set to 'window', the text position is given
            '   with respect to the window coordinate system.
            '   If set to 'image', image coordinates are used.
            '   (This may be useful in zoomed images.)
            'Row: The row coordinate of the desired text position
            '   If set to -1, a default value of 12 is used.
            'Column: The column coordinate of the desired text position
            '   If set to -1, a default value of 12 is used.
            'Color: defines the color of the text as string.
            '   If set to [], '' or 'auto' the currently set color is used.
            '   If a tuple of strings is passed, the colors are used cyclically
            '   for each new textline.
            'Box: If set to 'true', the text is written within a white box.
            '
            'prepare window
            HOperatorSet.GetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue)
            HOperatorSet.GetPart(hv_WindowHandle, hv_Row1Part, hv_Column1Part, hv_Row2Part, _
                hv_Column2Part)
            HOperatorSet.GetWindowExtents(hv_WindowHandle, hv_RowWin, hv_ColumnWin, hv_WidthWin, _
                hv_HeightWin)
            HOperatorSet.SetPart(hv_WindowHandle, New HTuple(0), New HTuple(0), hv_HeightWin.TupleSub( _
                New HTuple(1)), hv_WidthWin.TupleSub(New HTuple(1)))
            '
            'default settings
            If New HTuple(hv_Row_COPY_INP_TMP.TupleEqual(New HTuple(-1))).I() Then
                hv_Row_COPY_INP_TMP = New HTuple(12)
            End If
            If New HTuple(hv_Column_COPY_INP_TMP.TupleEqual(New HTuple(-1))).I() Then
                hv_Column_COPY_INP_TMP = New HTuple(12)
            End If
            If New HTuple(hv_TextColor_COPY_INP_TMP.TupleEqual(New HTuple())).I() Then
                hv_TextColor_COPY_INP_TMP = New HTuple("")
            End If
            '
            hv_String_COPY_INP_TMP = (((((New HTuple("")).TupleAdd(hv_String_COPY_INP_TMP))).TupleAdd( _
                New HTuple("")))).TupleSplit(New HTuple("" + Chr(10)))
            '
            'Estimate extentions of text depending on font size.
            HOperatorSet.GetFontExtents(hv_WindowHandle, hv_MaxAscent, hv_MaxDescent, hv_MaxWidth, _
                hv_MaxHeight)
            If New HTuple(hv_CoordSystem.TupleEqual(New HTuple("window"))).I() Then
                hv_R1 = hv_Row_COPY_INP_TMP.Clone()
                hv_C1 = hv_Column_COPY_INP_TMP.Clone()
            Else
                'transform image to window coordinates
                hv_FactorRow = (((New HTuple(1.0)).TupleMult(hv_HeightWin))).TupleDiv(((hv_Row2Part.TupleSub( _
                    hv_Row1Part))).TupleAdd(New HTuple(1)))
                hv_FactorColumn = (((New HTuple(1.0)).TupleMult(hv_WidthWin))).TupleDiv(( _
                    (hv_Column2Part.TupleSub(hv_Column1Part))).TupleAdd(New HTuple(1)))
                hv_R1 = ((((hv_Row_COPY_INP_TMP.TupleSub(hv_Row1Part))).TupleAdd(New HTuple(0.5)))).TupleMult( _
                    hv_FactorRow)
                hv_C1 = ((((hv_Column_COPY_INP_TMP.TupleSub(hv_Column1Part))).TupleAdd(New HTuple(0.5)))).TupleMult( _
                    hv_FactorColumn)
            End If
            '
            'display text box depending on text size
            '
            'calculate box extents
            hv_String_COPY_INP_TMP = (((New HTuple(" ")).TupleAdd(hv_String_COPY_INP_TMP))).TupleAdd( _
                New HTuple(" "))
            hv_Width = New HTuple()
            For hv_Index = (New HTuple(0)) To ((New HTuple( _
                hv_String_COPY_INP_TMP.TupleLength()))).TupleSub(New HTuple(1)) Step (New HTuple(1))
                HOperatorSet.GetStringExtents(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect( _
                    hv_Index), hv_Ascent, hv_Descent, hv_W, hv_H)
                hv_Width = hv_Width.TupleConcat(hv_W)
#If USE_DO_EVENTS Then
        ' Please note: The call of DoEvents() is only a hack to
        ' enable VB to react on events. Please change the code
        ' so that it can handle events in a standard way.
        System.Windows.Forms.Application.DoEvents()
#End If
            Next
            hv_FrameHeight = hv_MaxHeight.TupleMult(New HTuple(hv_String_COPY_INP_TMP.TupleLength( _
                )))
            hv_FrameWidth = (((New HTuple(0)).TupleConcat(hv_Width))).TupleMax()
            hv_R2 = hv_R1.TupleAdd(hv_FrameHeight)
            hv_C2 = hv_C1.TupleAdd(hv_FrameWidth)
            'display rectangles
            HOperatorSet.GetSystem(New HTuple("clip_region"), hv_ClipRegion)
            HOperatorSet.SetSystem(New HTuple("clip_region"), New HTuple("false"))
            HOperatorSet.GetDraw(hv_WindowHandle, hv_DrawMode)
            HOperatorSet.SetDraw(hv_WindowHandle, New HTuple("fill"))
            ho_UpperLeft.Dispose()
            HOperatorSet.GenRegionPolygonFilled(ho_UpperLeft, ((((((((hv_R1.TupleSub(New HTuple(3)))).TupleConcat( _
                hv_R1.TupleSub(New HTuple(3))))).TupleConcat(hv_R1))).TupleConcat(hv_R2))).TupleConcat( _
                hv_R2.TupleAdd(New HTuple(3))), ((((((((hv_C1.TupleSub(New HTuple(3)))).TupleConcat( _
                hv_C2.TupleAdd(New HTuple(3))))).TupleConcat(hv_C2))).TupleConcat(hv_C1))).TupleConcat( _
                hv_C1.TupleSub(New HTuple(3))))
            ho_LowerRight.Dispose()
            HOperatorSet.GenRegionPolygonFilled(ho_LowerRight, ((((((((hv_R2.TupleAdd(New HTuple(3)))).TupleConcat( _
                hv_R1.TupleSub(New HTuple(3))))).TupleConcat(hv_R1))).TupleConcat(hv_R2))).TupleConcat( _
                hv_R2.TupleAdd(New HTuple(3))), ((((((((hv_C2.TupleAdd(New HTuple(3)))).TupleConcat( _
                hv_C2.TupleAdd(New HTuple(3))))).TupleConcat(hv_C2))).TupleConcat(hv_C1))).TupleConcat( _
                hv_C1.TupleSub(New HTuple(3))))
            ho_Rectangle.Dispose()
            HOperatorSet.GenRectangle1(ho_Rectangle, hv_R1, hv_C1, hv_R2, hv_C2)
            HOperatorSet.SetColor(hv_WindowHandle, New HTuple("light gray"))
            HOperatorSet.DispObj(ho_UpperLeft, hv_WindowHandle)
            HOperatorSet.SetColor(hv_WindowHandle, New HTuple("dim gray"))
            HOperatorSet.DispObj(ho_LowerRight, hv_WindowHandle)
            HOperatorSet.SetColor(hv_WindowHandle, hv_ButtonColor)
            HOperatorSet.DispObj(ho_Rectangle, hv_WindowHandle)
            HOperatorSet.SetDraw(hv_WindowHandle, hv_DrawMode)
            HOperatorSet.SetSystem(New HTuple("clip_region"), hv_ClipRegion)
            'Write text.
            For hv_Index = (New HTuple(0)) To ( _
                (New HTuple(hv_String_COPY_INP_TMP.TupleLength()))).TupleSub(New HTuple(1)) Step (New HTuple(1))
                hv_CurrentColor = hv_TextColor_COPY_INP_TMP.TupleSelect(hv_Index.TupleMod( _
                    New HTuple(hv_TextColor_COPY_INP_TMP.TupleLength())))
                If ((New HTuple(hv_CurrentColor.TupleNotEqual(New HTuple(""))))).TupleAnd( _
                    New HTuple(hv_CurrentColor.TupleNotEqual(New HTuple("auto")))).I() Then
                    HOperatorSet.SetColor(hv_WindowHandle, hv_CurrentColor)
                Else
                    HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue)
                End If
                hv_Row_COPY_INP_TMP = hv_R1.TupleAdd(hv_MaxHeight.TupleMult(hv_Index))
                HOperatorSet.SetTposition(hv_WindowHandle, hv_Row_COPY_INP_TMP, hv_C1)
                HOperatorSet.WriteString(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect( _
                    hv_Index))
#If USE_DO_EVENTS Then
        ' Please note: The call of DoEvents() is only a hack to
        ' enable VB to react on events. Please change the code
        ' so that it can handle events in a standard way.
        System.Windows.Forms.Application.DoEvents()
#End If
            Next
            'reset changed window settings
            HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue)
            HOperatorSet.SetPart(hv_WindowHandle, hv_Row1Part, hv_Column1Part, hv_Row2Part, _
                hv_Column2Part)
            ho_UpperLeft.Dispose()
            ho_LowerRight.Dispose()
            ho_Rectangle.Dispose()

            Exit Sub
        Catch HDevExpDefaultException As HalconException
            ho_UpperLeft.Dispose()
            ho_LowerRight.Dispose()
            ho_Rectangle.Dispose()

            Throw HDevExpDefaultException
        End Try
    End Sub

    ' Chapter: Graphics / Output
    ' Short Description: Compute the center of all given 3D object models. 
    Public Sub get_object_models_center(ByVal hv_ObjectModel3DID As HTuple, ByRef hv_Center As HTuple)


        ' Local control variables 
        Dim hv_Diameter As HTuple = New HTuple, hv_MD As HTuple = New HTuple
        Dim hv_Weight As HTuple = New HTuple, hv_SumW As HTuple = New HTuple
        Dim hv_Index As HTuple = New HTuple, hv_ObjectModel3DIDSelected As HTuple = New HTuple
        Dim hv_C As HTuple = New HTuple, hv_InvSum As HTuple = New HTuple

        ' Initialize local and output iconic variables 

        'Compute the mean of all model centers (weighted by the diameter of the object models)
        If New HTuple(((New HTuple(hv_ObjectModel3DID.TupleLength()))).TupleGreater(New HTuple(0))).I() Then
            HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, New HTuple("diameter_axis_aligned_bounding_box"), _
                hv_Diameter)
            'Normalize Diameter to use it as weights for a weighted mean of the individual centers
            hv_MD = hv_Diameter.TupleMean()
            If New HTuple(hv_MD.TupleGreater(New HTuple(0.0000000001))).I() Then
                hv_Weight = hv_Diameter.TupleDiv(hv_MD)
            Else
                hv_Weight = hv_Diameter.Clone()
            End If
            hv_SumW = hv_Weight.TupleSum()
            If New HTuple(hv_SumW.TupleLess(New HTuple(0.0000000001))).I() Then
                hv_Weight = HTuple.TupleGenConst(New HTuple(hv_Weight.TupleLength()), New HTuple(1.0))
                hv_SumW = hv_Weight.TupleSum()
            End If
            hv_Center = ((New HTuple(0)).TupleConcat(0)).TupleConcat(0)
            For hv_Index = (New HTuple(0)) To ( _
                (New HTuple(hv_ObjectModel3DID.TupleLength()))).TupleSub(New HTuple(1)) Step (New HTuple(1))
                hv_ObjectModel3DIDSelected = hv_ObjectModel3DID.TupleSelect(hv_Index)
                HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DIDSelected, New HTuple("center"), _
                    hv_C)
                If IsNothing(hv_Center) Then
                    hv_Center = New HTuple
                End If
                hv_Center(New HTuple(0)) = ((hv_Center.TupleSelect(New HTuple(0)))).TupleAdd( _
                    ((hv_C.TupleSelect(New HTuple(0)))).TupleMult(hv_Weight.TupleSelect(hv_Index)))
                If IsNothing(hv_Center) Then
                    hv_Center = New HTuple
                End If
                hv_Center(New HTuple(1)) = ((hv_Center.TupleSelect(New HTuple(1)))).TupleAdd( _
                    ((hv_C.TupleSelect(New HTuple(1)))).TupleMult(hv_Weight.TupleSelect(hv_Index)))
                If IsNothing(hv_Center) Then
                    hv_Center = New HTuple
                End If
                hv_Center(New HTuple(2)) = ((hv_Center.TupleSelect(New HTuple(2)))).TupleAdd( _
                    ((hv_C.TupleSelect(New HTuple(2)))).TupleMult(hv_Weight.TupleSelect(hv_Index)))
#If USE_DO_EVENTS Then
        ' Please note: The call of DoEvents() is only a hack to
        ' enable VB to react on events. Please change the code
        ' so that it can handle events in a standard way.
        System.Windows.Forms.Application.DoEvents()
#End If
            Next
            hv_InvSum = (New HTuple(1.0)).TupleDiv(hv_SumW)
            If IsNothing(hv_Center) Then
                hv_Center = New HTuple
            End If
            hv_Center(New HTuple(0)) = ((hv_Center.TupleSelect(New HTuple(0)))).TupleMult( _
                hv_InvSum)
            If IsNothing(hv_Center) Then
                hv_Center = New HTuple
            End If
            hv_Center(New HTuple(1)) = ((hv_Center.TupleSelect(New HTuple(1)))).TupleMult( _
                hv_InvSum)
            If IsNothing(hv_Center) Then
                hv_Center = New HTuple
            End If
            hv_Center(New HTuple(2)) = ((hv_Center.TupleSelect(New HTuple(2)))).TupleMult( _
                hv_InvSum)
        Else
            hv_Center = New HTuple()
        End If

        Exit Sub
    End Sub

    ' Chapter: Graphics / Output
    ' Short Description: Displays a continue button. 
    Public Sub disp_continue_button(ByVal hv_WindowHandle As HTuple)


        ' Local control variables 
        Dim hv_ContinueMessage As HTuple = New HTuple
        Dim hv_Row As HTuple = New HTuple, hv_Column As HTuple = New HTuple
        Dim hv_Width As HTuple = New HTuple, hv_Height As HTuple = New HTuple
        Dim hv_Ascent As HTuple = New HTuple, hv_Descent As HTuple = New HTuple
        Dim hv_TextWidth As HTuple = New HTuple, hv_TextHeight As HTuple = New HTuple

        ' Initialize local and output iconic variables 

        'This procedure displays a 'Continue' text button
        'in the lower right corner of the screen.
        'It uses the procedure disp_message.
        '
        'Input parameters:
        'WindowHandle: The window, where the text shall be displayed
        hv_ContinueMessage = New HTuple("Continue")
        HOperatorSet.GetWindowExtents(hv_WindowHandle, hv_Row, hv_Column, hv_Width, hv_Height)
        HOperatorSet.GetStringExtents(hv_WindowHandle, (((New HTuple(" ")).TupleAdd(hv_ContinueMessage))).TupleAdd( _
            New HTuple(" ")), hv_Ascent, hv_Descent, hv_TextWidth, hv_TextHeight)
        disp_text_button(hv_WindowHandle, hv_ContinueMessage, New HTuple("window"), ( _
            (hv_Height.TupleSub(hv_TextHeight))).TupleSub(New HTuple(12)), ((hv_Width.TupleSub( _
            hv_TextWidth))).TupleSub(New HTuple(12)), New HTuple("black"), New HTuple("gray"))

        Exit Sub
    End Sub

    ' Chapter: Graphics / Output
    ' Short Description: Interactively display 3D object models 
    Public Sub visualize_object_model_3d(ByVal hv_WindowHandle As HTuple, ByVal hv_ObjectModel3D As HTuple, _
        ByVal hv_CamParam As HTuple, ByVal hv_PoseIn As HTuple, ByVal hv_GenParamName As HTuple, _
        ByVal hv_GenParamValue As HTuple, ByVal hv_Title As HTuple, ByVal hv_Label As HTuple, _
        ByVal hv_Information As HTuple, ByRef hv_PoseOut As HTuple)


        ' Local iconic variables 
        Dim ho_Image As HObject = Nothing, ho_ImageDump As HObject = Nothing


        ' Local control variables 
        Dim ExpTmpLocalVar_gDispObjOffset As HTuple = New HTuple
        Dim ExpTmpLocalVar_gLabelsDecor As HTuple = New HTuple
        Dim ExpTmpLocalVar_gInfoDecor As HTuple = New HTuple
        Dim ExpTmpLocalVar_gInfoPos As HTuple = New HTuple
        Dim ExpTmpLocalVar_gTitlePos As HTuple = New HTuple
        Dim ExpTmpLocalVar_gTitleDecor As HTuple = New HTuple
        Dim ExpTmpLocalVar_gTerminationButtonLabel As HTuple = New HTuple
        Dim ExpTmpLocalVar_gAlphaDeselected As HTuple = New HTuple
        Dim ExpTmpLocalVar_gIsSinglePose As HTuple = New HTuple
        Dim ExpTmpLocalVar_gUsesOpenGL As HTuple = New HTuple
        Dim hv_TrackballSize As HTuple = New HTuple, hv_VirtualTrackball As HTuple = New HTuple
        Dim hv_MouseMapping As HTuple = New HTuple, hv_MaxNumModels As HTuple = New HTuple
        Dim hv_WindowCenteredRotation As HTuple = New HTuple
        Dim hv_NumModels As HTuple = New HTuple, hv_SelectedObject As HTuple = New HTuple
        Dim hv_ClipRegion As HTuple = New HTuple, hv_CPLength As HTuple = New HTuple
        Dim hv_RowNotUsed As HTuple = New HTuple, hv_ColumnNotUsed As HTuple = New HTuple
        Dim hv_Width As HTuple = New HTuple, hv_Height As HTuple = New HTuple
        Dim hv_CamWidth As HTuple = New HTuple, hv_CamHeight As HTuple = New HTuple
        Dim hv_Scale As HTuple = New HTuple, hv_Indices As HTuple = New HTuple
        Dim hv_UseBackground As HTuple = New HTuple, hv_Center As HTuple = New HTuple
        Dim hv_Poses As HTuple = New HTuple, hv_HomMat3Ds As HTuple = New HTuple
        Dim hv_Sequence As HTuple = New HTuple, hv_PoseEstimated As HTuple = New HTuple
        Dim hv_WindowHandleBuffer As HTuple = New HTuple, hv_Font As HTuple = New HTuple
        Dim hv_Exception As HTuple = New HTuple, hv_OpenGLInfo As HTuple = New HTuple
        Dim hv_DummyObjectModel3D As HTuple = New HTuple, hv_MinImageSize As HTuple = New HTuple
        Dim hv_TrackballRadiusPixel As HTuple = New HTuple
        Dim hv_Ascent As HTuple = New HTuple, hv_Descent As HTuple = New HTuple
        Dim hv_TextWidth As HTuple = New HTuple, hv_TextHeight As HTuple = New HTuple
        Dim hv_NumChannels As HTuple = New HTuple, hv_ColorImage As HTuple = New HTuple
        Dim hv_HomMat3D As HTuple = New HTuple, hv_Qx As HTuple = New HTuple
        Dim hv_Qy As HTuple = New HTuple, hv_Qz As HTuple = New HTuple
        Dim hv_TBCenter As HTuple = New HTuple, hv_TBSize As HTuple = New HTuple
        Dim hv_ButtonHold As HTuple = New HTuple, hv_VisualizeTB As HTuple = New HTuple
        Dim hv_MaxIndex As HTuple = New HTuple, hv_TrackballCenterRow As HTuple = New HTuple
        Dim hv_TrackballCenterCol As HTuple = New HTuple, hv_GraphEvent As HTuple = New HTuple
        Dim hv_Exit As HTuple = New HTuple, hv_GraphButtonRow As HTuple = New HTuple
        Dim hv_GraphButtonColumn As HTuple = New HTuple, hv_GraphButton As HTuple = New HTuple

        Dim hv_CamParam_COPY_INP_TMP As HTuple
        hv_CamParam_COPY_INP_TMP = hv_CamParam.Clone()
        Dim hv_GenParamName_COPY_INP_TMP As HTuple
        hv_GenParamName_COPY_INP_TMP = hv_GenParamName.Clone()
        Dim hv_GenParamValue_COPY_INP_TMP As HTuple
        hv_GenParamValue_COPY_INP_TMP = hv_GenParamValue.Clone()
        Dim hv_Label_COPY_INP_TMP As HTuple
        hv_Label_COPY_INP_TMP = hv_Label.Clone()
        Dim hv_PoseIn_COPY_INP_TMP As HTuple
        hv_PoseIn_COPY_INP_TMP = hv_PoseIn.Clone()

        ' Initialize local and output iconic variables 
        HOperatorSet.GenEmptyObj(ho_Image)
        HOperatorSet.GenEmptyObj(ho_ImageDump)

        Try
            'The procedure visualize_object_model_3d can be used to display
            'one or more 3d object models and to interactively modify
            'the object poses by using the mouse.
            '
            'The pose can be modified by moving the mouse while
            'pressing a mouse button. The default settings are:
            '
            ' Left mouse button:   Modify the object orientation
            ' Shift+ left mouse button  or
            ' center mouse button: Modify the object distance
            ' Right mouse button:  Modify the object position
            ' Ctrl + Left mouse button: (De-)select object(s)
            ' Alt + Mouse button: Low mouse sensitiviy
            ' (Default may be changed with the variable MouseMapping below)
            '
            'In GenParamName and GenParamValue all generic Paramters
            'of disp_object_model_3d are supported.
            '
            '**********************************************************
            'Define global variables
            '**********************************************************
            '
            'global def tuple gDispObjOffset
            'global def tuple gLabelsDecor
            'global def tuple gInfoDecor
            'global def tuple gInfoPos
            'global def tuple gTitlePos
            'global def tuple gTitleDecor
            'global def tuple gTerminationButtonLabel
            'global def tuple gAlphaDeselected
            'global def tuple gIsSinglePose
            'global def tuple gUsesOpenGL
            '
            '**********************************************************
            'First some user defines that may be adapted if desired
            '**********************************************************
            '
            'TrackballSize defines the diameter of the trackball in
            'the image with respect to the smaller image dimension.
            hv_TrackballSize = New HTuple(0.8)
            '
            'VirtualTrackball defines the type of virtual trackball that
            'shall be used ('shoemake' or 'bell').
            hv_VirtualTrackball = New HTuple("shoemake")
            'VirtualTrackball := 'bell'
            '
            'Functionality of mouse buttons
            '    1: Left Button
            '    2: Middle Button
            '    4: Right Button
            '    5: Left+Right Mousebutton
            '  8+x: Shift + Mousebutton
            ' 16+x: Ctrl + Mousebutton
            ' 48+x: Ctrl + Alt + Mousebutton
            'in the order [Translate, Rotate, Scale, ScaleAlternative1, ScaleAlternative2, SelectObjects, ToggleSelectionMode]
            hv_MouseMapping = ((((((New HTuple(17)).TupleConcat(1)).TupleConcat(2)).TupleConcat( _
                5)).TupleConcat(9)).TupleConcat(4)).TupleConcat(49)
            '
            'The labels of the objects appear next to their projected
            'center. With gDispObjOffset a fixed offset is added
            '                  R,  C
            ExpTmpLocalVar_gDispObjOffset = (New HTuple(-30)).TupleConcat(0)
            ExpSetGlobalVar_gDispObjOffset(ExpTmpLocalVar_gDispObjOffset)
            '
            'Customize the decoration of the different text elements
            '              Color,   Box
            ExpTmpLocalVar_gInfoDecor = (New HTuple(New HTuple("white"))).TupleConcat(New HTuple("false"))
            ExpSetGlobalVar_gInfoDecor(ExpTmpLocalVar_gInfoDecor)
            ExpTmpLocalVar_gLabelsDecor = (New HTuple(New HTuple("white"))).TupleConcat( _
                New HTuple("false"))
            ExpSetGlobalVar_gLabelsDecor(ExpTmpLocalVar_gLabelsDecor)
            ExpTmpLocalVar_gTitleDecor = (New HTuple(New HTuple("black"))).TupleConcat( _
                New HTuple("true"))
            ExpSetGlobalVar_gTitleDecor(ExpTmpLocalVar_gTitleDecor)
            '
            'Customize the position of some text elements
            '  gInfoPos has one of the values
            '  {'UpperLeft', 'LowerLeft', 'UpperRight'}
            ExpTmpLocalVar_gInfoPos = New HTuple("LowerLeft")
            ExpSetGlobalVar_gInfoPos(ExpTmpLocalVar_gInfoPos)
            '  gTitlePos has one of the values
            '  {'UpperLeft', 'UpperCenter', 'UpperRight'}
            ExpTmpLocalVar_gTitlePos = New HTuple("UpperLeft")
            ExpSetGlobalVar_gTitlePos(ExpTmpLocalVar_gTitlePos)
            'Alpha value (=1-transparency) that is used for visualizing
            'the objects that are not selected
            ExpTmpLocalVar_gAlphaDeselected = New HTuple(0.3)
            ExpSetGlobalVar_gAlphaDeselected(ExpTmpLocalVar_gAlphaDeselected)
            'Customize the exit box label
            ExpTmpLocalVar_gTerminationButtonLabel = New HTuple(" Continue ")
            ExpSetGlobalVar_gTerminationButtonLabel(ExpTmpLocalVar_gTerminationButtonLabel)
            'Number of 3D Object models that can be handled individually
            'if there are more models passed then this number, some calculations
            'are performed differently. And the individual handling of models is not
            'supported anymore
            hv_MaxNumModels = New HTuple(5)
            'Defines the default for the initial state of the rotation center:
            '(1) The rotation center is fixed in the center of the image and lies
            '    on the surface of the object.
            '(2) The rotation center lies in the center of the object.
            hv_WindowCenteredRotation = New HTuple(2)
            '
            '**********************************************************
            '
            'Initialize some values
            hv_NumModels = New HTuple(hv_ObjectModel3D.TupleLength())
            hv_SelectedObject = HTuple.TupleGenConst(hv_NumModels, New HTuple(1))
            '
            'Apply some system settings
            ' dev_set_preferences(...); only in hdevelop
            ' dev_get_preferences(...); only in hdevelop
            ' dev_set_preferences(...); only in hdevelop
            ' dev_get_preferences(...); only in hdevelop
            ' dev_set_preferences(...); only in hdevelop
            HOperatorSet.GetSystem(New HTuple("clip_region"), hv_ClipRegion)
            HOperatorSet.SetSystem(New HTuple("clip_region"), New HTuple("false"))
            dev_update_off()
            '
            'Refactor camera parameters to fit to window size
            '
            hv_CPLength = New HTuple(hv_CamParam_COPY_INP_TMP.TupleLength())
            HOperatorSet.GetWindowExtents(hv_WindowHandle, hv_RowNotUsed, hv_ColumnNotUsed, _
                hv_Width, hv_Height)
            If New HTuple(hv_CPLength.TupleEqual(New HTuple(0))).I() Then
                hv_CamParam_COPY_INP_TMP = ((((((((((New HTuple(0.06)).TupleConcat(0)).TupleConcat( _
                    0.0000085)).TupleConcat(0.0000085)).TupleConcat(hv_Width.TupleDiv(New HTuple(2))))).TupleConcat( _
                    hv_Height.TupleDiv(New HTuple(2))))).TupleConcat(hv_Width))).TupleConcat( _
                    hv_Height)
                hv_CPLength = New HTuple(hv_CamParam_COPY_INP_TMP.TupleLength())
            Else
                hv_CamWidth = ((hv_CamParam_COPY_INP_TMP.TupleSelect(hv_CPLength.TupleSub( _
                    New HTuple(2))))).TupleReal()
                hv_CamHeight = ((hv_CamParam_COPY_INP_TMP.TupleSelect(hv_CPLength.TupleSub( _
                    New HTuple(1))))).TupleReal()
                hv_Scale = ((((hv_Width.TupleDiv(hv_CamWidth))).TupleConcat(hv_Height.TupleDiv( _
                    hv_CamHeight)))).TupleMin()
                If IsNothing(hv_CamParam_COPY_INP_TMP) Then
                    hv_CamParam_COPY_INP_TMP = New HTuple
                End If
                hv_CamParam_COPY_INP_TMP(hv_CPLength.TupleSub(New HTuple(6))) = ((hv_CamParam_COPY_INP_TMP.TupleSelect( _
                    hv_CPLength.TupleSub(New HTuple(6))))).TupleDiv(hv_Scale)
                If IsNothing(hv_CamParam_COPY_INP_TMP) Then
                    hv_CamParam_COPY_INP_TMP = New HTuple
                End If
                hv_CamParam_COPY_INP_TMP(hv_CPLength.TupleSub(New HTuple(5))) = ((hv_CamParam_COPY_INP_TMP.TupleSelect( _
                    hv_CPLength.TupleSub(New HTuple(5))))).TupleDiv(hv_Scale)
                If IsNothing(hv_CamParam_COPY_INP_TMP) Then
                    hv_CamParam_COPY_INP_TMP = New HTuple
                End If
                hv_CamParam_COPY_INP_TMP(hv_CPLength.TupleSub(New HTuple(4))) = ((hv_CamParam_COPY_INP_TMP.TupleSelect( _
                    hv_CPLength.TupleSub(New HTuple(4))))).TupleMult(hv_Scale)
                If IsNothing(hv_CamParam_COPY_INP_TMP) Then
                    hv_CamParam_COPY_INP_TMP = New HTuple
                End If
                hv_CamParam_COPY_INP_TMP(hv_CPLength.TupleSub(New HTuple(3))) = ((hv_CamParam_COPY_INP_TMP.TupleSelect( _
                    hv_CPLength.TupleSub(New HTuple(3))))).TupleMult(hv_Scale)
                If IsNothing(hv_CamParam_COPY_INP_TMP) Then
                    hv_CamParam_COPY_INP_TMP = New HTuple
                End If
                hv_CamParam_COPY_INP_TMP(hv_CPLength.TupleSub(New HTuple(2))) = ((((hv_CamParam_COPY_INP_TMP.TupleSelect( _
                    hv_CPLength.TupleSub(New HTuple(2))))).TupleMult(hv_Scale))).TupleInt( _
                    )
                If IsNothing(hv_CamParam_COPY_INP_TMP) Then
                    hv_CamParam_COPY_INP_TMP = New HTuple
                End If
                hv_CamParam_COPY_INP_TMP(hv_CPLength.TupleSub(New HTuple(1))) = ((((hv_CamParam_COPY_INP_TMP.TupleSelect( _
                    hv_CPLength.TupleSub(New HTuple(1))))).TupleMult(hv_Scale))).TupleInt( _
                    )
            End If
            '
            'Check the generic parameters for window_centered_rotation
            '(Note that the default is set above to WindowCenteredRotation := 2)
            hv_Indices = hv_GenParamName_COPY_INP_TMP.TupleFind(New HTuple("inspection_mode"))
            If ((New HTuple(hv_Indices.TupleNotEqual(New HTuple(-1))))).TupleAnd(New HTuple( _
                hv_Indices.TupleNotEqual(New HTuple()))).I() Then
                If New HTuple(((hv_GenParamValue_COPY_INP_TMP.TupleSelect(hv_Indices.TupleSelect( _
                    New HTuple(0))))).TupleEqual(New HTuple("surface"))).I() Then
                    hv_WindowCenteredRotation = New HTuple(1)
                ElseIf New HTuple(((hv_GenParamValue_COPY_INP_TMP.TupleSelect(hv_Indices.TupleSelect( _
                    New HTuple(0))))).TupleEqual(New HTuple("standard"))).I() Then
                    hv_WindowCenteredRotation = New HTuple(2)
                Else
                    'Wrong parameter value, use default value
                End If
                hv_GenParamName_COPY_INP_TMP = hv_GenParamName_COPY_INP_TMP.TupleRemove(hv_Indices)
                hv_GenParamValue_COPY_INP_TMP = hv_GenParamValue_COPY_INP_TMP.TupleRemove( _
                    hv_Indices)
            End If
            '
            'Check the generic parameters for use_background
            hv_UseBackground = New HTuple("true")
            hv_Indices = hv_GenParamName_COPY_INP_TMP.TupleFind(New HTuple("use_background"))
            If ((New HTuple(hv_Indices.TupleNotEqual(New HTuple(-1))))).TupleAnd(New HTuple( _
                hv_Indices.TupleNotEqual(New HTuple()))).I() Then
                hv_UseBackground = hv_GenParamValue_COPY_INP_TMP.TupleSelect(hv_Indices.TupleSelect( _
                    New HTuple(0)))
                hv_GenParamName_COPY_INP_TMP = hv_GenParamName_COPY_INP_TMP.TupleRemove(hv_Indices)
                hv_GenParamValue_COPY_INP_TMP = hv_GenParamValue_COPY_INP_TMP.TupleRemove( _
                    hv_Indices)
            End If
            '
            'Read and check the parameter Label for each object
            If New HTuple(((New HTuple(hv_Label_COPY_INP_TMP.TupleLength()))).TupleEqual( _
                New HTuple(0))).I() Then
                hv_Label_COPY_INP_TMP = New HTuple(0)
            ElseIf New HTuple(((New HTuple(hv_Label_COPY_INP_TMP.TupleLength()))).TupleEqual( _
                New HTuple(1))).I() Then
                hv_Label_COPY_INP_TMP = HTuple.TupleGenConst(hv_NumModels, hv_Label_COPY_INP_TMP)
            Else
                If New HTuple(((New HTuple(hv_Label_COPY_INP_TMP.TupleLength()))).TupleNotEqual( _
                    hv_NumModels)).I() Then
                    'Error: Number of elements in Label does not match the
                    'number of object models
                    HDevelopStop()
                End If
            End If
            '
            'Read and check the parameter PoseIn for each object
            get_object_models_center(hv_ObjectModel3D, hv_Center)
            If New HTuple(((New HTuple(hv_PoseIn_COPY_INP_TMP.TupleLength()))).TupleEqual( _
                New HTuple(0))).I() Then
                'If no pose was specified by the caller, automatically calculate
                'a pose that is appropriate for the visualization.
                'Set the initial model reference pose. The orientation is parallel
                'to the object coordinate system, the position is at the center
                'of gravity of all models.
                HOperatorSet.CreatePose(((hv_Center.TupleSelect(New HTuple(0)))).TupleNeg( _
                    ), ((hv_Center.TupleSelect(New HTuple(1)))).TupleNeg(), ((hv_Center.TupleSelect( _
                    New HTuple(2)))).TupleNeg(), New HTuple(0), New HTuple(0), New HTuple(0), _
                    New HTuple("Rp+T"), New HTuple("gba"), New HTuple("point"), hv_PoseIn_COPY_INP_TMP)
                determine_optimum_pose_distance(hv_ObjectModel3D, hv_CamParam_COPY_INP_TMP, _
                    New HTuple(0.9), hv_PoseIn_COPY_INP_TMP, hv_PoseEstimated)
                hv_Poses = New HTuple()
                hv_HomMat3Ds = New HTuple()
                hv_Sequence = HTuple.TupleGenSequence(New HTuple(0), ((hv_NumModels.TupleMult( _
                    New HTuple(7)))).TupleSub(New HTuple(1)), New HTuple(1))
                hv_Poses = hv_PoseEstimated.TupleSelect(hv_Sequence.TupleMod(New HTuple(7)))
                ExpTmpLocalVar_gIsSinglePose = New HTuple(1)
                ExpSetGlobalVar_gIsSinglePose(ExpTmpLocalVar_gIsSinglePose)
            ElseIf New HTuple(((New HTuple(hv_PoseIn_COPY_INP_TMP.TupleLength()))).TupleEqual( _
                New HTuple(7))).I() Then
                hv_Poses = New HTuple()
                hv_HomMat3Ds = New HTuple()
                hv_Sequence = HTuple.TupleGenSequence(New HTuple(0), ((hv_NumModels.TupleMult( _
                    New HTuple(7)))).TupleSub(New HTuple(1)), New HTuple(1))
                hv_Poses = hv_PoseIn_COPY_INP_TMP.TupleSelect(hv_Sequence.TupleMod(New HTuple(7)))
                ExpTmpLocalVar_gIsSinglePose = New HTuple(1)
                ExpSetGlobalVar_gIsSinglePose(ExpTmpLocalVar_gIsSinglePose)
            Else
                If New HTuple(((New HTuple(hv_PoseIn_COPY_INP_TMP.TupleLength()))).TupleNotEqual( _
                    ((New HTuple(hv_ObjectModel3D.TupleLength()))).TupleMult(New HTuple(7)))).I() Then
                    'Error: Wrong number of values of input control parameter 'PoseIn'
                    HDevelopStop()
                Else
                    hv_Poses = hv_PoseIn_COPY_INP_TMP.Clone()
                End If
                ExpTmpLocalVar_gIsSinglePose = New HTuple(0)
                ExpSetGlobalVar_gIsSinglePose(ExpTmpLocalVar_gIsSinglePose)
            End If
            '
            'Open (invisible) buffer window to avoid flickering
            HOperatorSet.OpenWindow(New HTuple(0), New HTuple(0), hv_Width, hv_Height, _
                New HTuple(0), New HTuple("buffer"), New HTuple(""), hv_WindowHandleBuffer)
            HOperatorSet.SetPart(hv_WindowHandleBuffer, New HTuple(0), New HTuple(0), hv_Height.TupleSub( _
                New HTuple(1)), hv_Width.TupleSub(New HTuple(1)))
            HOperatorSet.GetFont(hv_WindowHandle, hv_Font)
            Try
                HOperatorSet.SetFont(hv_WindowHandleBuffer, hv_Font)
                ' catch (Exception) 
            Catch HDevExpDefaultException1 As HalconException
                HDevExpDefaultException1.ToHTuple(hv_Exception)
            End Try
            '
            ' Is OpenGL available?
            ExpTmpLocalVar_gUsesOpenGL = New HTuple("true")
            ExpSetGlobalVar_gUsesOpenGL(ExpTmpLocalVar_gUsesOpenGL)
            hv_Indices = hv_GenParamName_COPY_INP_TMP.TupleFind(New HTuple("opengl"))
            If ((New HTuple(hv_Indices.TupleNotEqual(New HTuple(-1))))).TupleAnd(New HTuple( _
                hv_Indices.TupleNotEqual(New HTuple()))).I() Then
                ExpTmpLocalVar_gUsesOpenGL = hv_GenParamValue_COPY_INP_TMP.TupleSelect(hv_Indices.TupleSelect( _
                    New HTuple(0)))
                ExpSetGlobalVar_gUsesOpenGL(ExpTmpLocalVar_gUsesOpenGL)
            Else
                HOperatorSet.GetSystem(New HTuple("opengl_info"), hv_OpenGLInfo)
                If New HTuple(hv_OpenGLInfo.TupleEqual(New HTuple("No OpenGL support included."))).I() Then
                    ExpTmpLocalVar_gUsesOpenGL = New HTuple("false")
                    ExpSetGlobalVar_gUsesOpenGL(ExpTmpLocalVar_gUsesOpenGL)
                Else
                    HOperatorSet.GenObjectModel3dFromPoints(New HTuple(0), New HTuple(0), New HTuple(0), _
                        hv_DummyObjectModel3D)
                    Try
                        HOperatorSet.DispObjectModel3d(hv_WindowHandleBuffer, hv_DummyObjectModel3D, _
                            New HTuple(), New HTuple(), New HTuple(), New HTuple())
                        ' catch (Exception) 
                    Catch HDevExpDefaultException1 As HalconException
                        HDevExpDefaultException1.ToHTuple(hv_Exception)
                        ExpTmpLocalVar_gUsesOpenGL = New HTuple("false")
                        ExpSetGlobalVar_gUsesOpenGL(ExpTmpLocalVar_gUsesOpenGL)
                    End Try
                    HOperatorSet.ClearObjectModel3d(hv_DummyObjectModel3D)
                End If
            End If
            '
            'Compute the trackball
            hv_MinImageSize = ((hv_Width.TupleConcat(hv_Height))).TupleMin()
            hv_TrackballRadiusPixel = ((hv_TrackballSize.TupleMult(hv_MinImageSize))).TupleDiv( _
                New HTuple(2.0))
            '
            'Measure the text extents for the continue button in the
            'graphics window
            HOperatorSet.GetStringExtents(hv_WindowHandleBuffer, ExpGetGlobalVar_gTerminationButtonLabel().TupleAdd( _
                New HTuple("  ")), hv_Ascent, hv_Descent, hv_TextWidth, hv_TextHeight)
            '
            'Store background image
            If New HTuple(hv_UseBackground.TupleEqual(New HTuple("false"))).I() Then
                HOperatorSet.ClearWindow(hv_WindowHandle)
            End If
            ho_Image.Dispose()
            HOperatorSet.DumpWindowImage(ho_Image, hv_WindowHandle)
            '
            'Special treatment for color background images necessary
            HOperatorSet.CountChannels(ho_Image, hv_NumChannels)
            hv_ColorImage = New HTuple(hv_NumChannels.TupleEqual(New HTuple(3)))
            hv_GenParamName_COPY_INP_TMP = hv_GenParamName_COPY_INP_TMP.TupleConcat(New HTuple("disp_background"))
            hv_GenParamValue_COPY_INP_TMP = hv_GenParamValue_COPY_INP_TMP.TupleConcat(New HTuple("true"))
            '
            'Start the visualization loop
            HOperatorSet.PoseToHomMat3d(hv_Poses.TupleSelectRange(New HTuple(0), New HTuple(6)), _
                hv_HomMat3D)
            HOperatorSet.AffineTransPoint3d(hv_HomMat3D, hv_Center.TupleSelect(New HTuple(0)), _
                hv_Center.TupleSelect(New HTuple(1)), hv_Center.TupleSelect(New HTuple(2)), _
                hv_Qx, hv_Qy, hv_Qz)
            hv_TBCenter = ((hv_Qx.TupleConcat(hv_Qy))).TupleConcat(hv_Qz)
            hv_TBSize = (((New HTuple(0.5)).TupleAdd((((New HTuple(0.5)).TupleMult(hv_SelectedObject.TupleSum( _
                )))).TupleDiv(hv_NumModels)))).TupleMult(hv_TrackballRadiusPixel)
            hv_ButtonHold = New HTuple(0)
            Do While (New HTuple(1)).I()
                hv_VisualizeTB = New HTuple(((hv_SelectedObject.TupleMax())).TupleNotEqual( _
                    New HTuple(0)))
                hv_MaxIndex = ((((((New HTuple(hv_ObjectModel3D.TupleLength()))).TupleConcat( _
                    hv_MaxNumModels))).TupleMin())).TupleSub(New HTuple(1))
                'Set trackball fixed in the center of the window
                hv_TrackballCenterRow = hv_Height.TupleDiv(New HTuple(2))
                hv_TrackballCenterCol = hv_Width.TupleDiv(New HTuple(2))
                If New HTuple(hv_WindowCenteredRotation.TupleEqual(New HTuple(1))).I() Then
                    Try
                        get_trackball_center_fixed(hv_SelectedObject.TupleSelectRange(New HTuple(0), _
                            hv_MaxIndex), hv_TrackballCenterRow, hv_TrackballCenterCol, hv_TrackballRadiusPixel, _
                            hv_ObjectModel3D.TupleSelectRange(New HTuple(0), hv_MaxIndex), hv_Poses.TupleSelectRange( _
                            New HTuple(0), ((((hv_MaxIndex.TupleAdd(New HTuple(1)))).TupleMult( _
                            New HTuple(7)))).TupleSub(New HTuple(1))), hv_WindowHandleBuffer, _
                            hv_CamParam_COPY_INP_TMP, hv_GenParamName_COPY_INP_TMP, hv_GenParamValue_COPY_INP_TMP, _
                            hv_TBCenter, hv_TBSize)
                        ' catch (Exception) 
                    Catch HDevExpDefaultException1 As HalconException
                        HDevExpDefaultException1.ToHTuple(hv_Exception)
                        disp_message(hv_WindowHandle, New HTuple("Surface inspection mode is not available."), _
                            New HTuple("image"), New HTuple(5), New HTuple(20), New HTuple("red"), _
                            New HTuple("true"))
                        hv_WindowCenteredRotation = New HTuple(2)
                        get_trackball_center(hv_SelectedObject.TupleSelectRange(New HTuple(0), _
                            hv_MaxIndex), hv_TrackballRadiusPixel, hv_ObjectModel3D.TupleSelectRange( _
                            New HTuple(0), hv_MaxIndex), hv_Poses.TupleSelectRange(New HTuple(0), _
                            ((((hv_MaxIndex.TupleAdd(New HTuple(1)))).TupleMult(New HTuple(7)))).TupleSub( _
                            New HTuple(1))), hv_TBCenter, hv_TBSize)
                        HOperatorSet.WaitSeconds(New HTuple(1))
                    End Try
                Else
                    get_trackball_center(hv_SelectedObject.TupleSelectRange(New HTuple(0), hv_MaxIndex), _
                        hv_TrackballRadiusPixel, hv_ObjectModel3D.TupleSelectRange(New HTuple(0), _
                        hv_MaxIndex), hv_Poses.TupleSelectRange(New HTuple(0), ((((hv_MaxIndex.TupleAdd( _
                        New HTuple(1)))).TupleMult(New HTuple(7)))).TupleSub(New HTuple(1))), _
                        hv_TBCenter, hv_TBSize)
                End If
                dump_image_output(ho_Image, hv_WindowHandleBuffer, hv_ObjectModel3D, hv_GenParamName_COPY_INP_TMP, _
                    hv_GenParamValue_COPY_INP_TMP, hv_CamParam_COPY_INP_TMP, hv_Poses, hv_ColorImage, _
                    hv_Title, hv_Information, hv_Label_COPY_INP_TMP, hv_VisualizeTB, New HTuple("true"), _
                    hv_TrackballCenterRow, hv_TrackballCenterCol, hv_TBSize, hv_SelectedObject, _
                    hv_WindowCenteredRotation, hv_TBCenter)
                ho_ImageDump.Dispose()
                HOperatorSet.DumpWindowImage(ho_ImageDump, hv_WindowHandleBuffer)
                HDevWindowStack.SetActive(hv_WindowHandle)
                If (HDevWindowStack.IsOpen()) Then
                    HOperatorSet.DispObj(ho_ImageDump, HDevWindowStack.GetActive())
                End If
                '
                'Check for mouse events
                hv_GraphEvent = New HTuple(0)
                hv_Exit = New HTuple(0)
                Do While (New HTuple(1)).I()
                    '
                    'Check graphic event
                    Try
                        HOperatorSet.GetMpositionSubPix(hv_WindowHandle, hv_GraphButtonRow, hv_GraphButtonColumn, _
                            hv_GraphButton)
                        If New HTuple(hv_GraphButton.TupleNotEqual(New HTuple(0))).I() Then
                            If ((((((New HTuple(hv_GraphButtonRow.TupleGreater(((hv_Height.TupleSub( _
                                hv_TextHeight))).TupleSub(New HTuple(13)))))).TupleAnd(New HTuple( _
                                hv_GraphButtonRow.TupleLess(hv_Height))))).TupleAnd(New HTuple( _
                                hv_GraphButtonColumn.TupleGreater(((hv_Width.TupleSub(hv_TextWidth))).TupleSub( _
                                New HTuple(13))))))).TupleAnd(New HTuple(hv_GraphButtonColumn.TupleLess( _
                                hv_Width))).I() Then
                                hv_Exit = New HTuple(1)
                                Exit Do
                            End If
                            hv_GraphEvent = New HTuple(1)
                            Exit Do
                        Else
                            hv_ButtonHold = New HTuple(0)
                        End If
                        ' catch (Exception) 
                    Catch HDevExpDefaultException1 As HalconException
                        HDevExpDefaultException1.ToHTuple(hv_Exception)
                        'Keep waiting
                    End Try
#If USE_DO_EVENTS Then
          ' Please note: The call of DoEvents() is only a hack to
          ' enable VB to react on events. Please change the code
          ' so that it can handle events in a standard way.
          System.Windows.Forms.Application.DoEvents()
#End If
                Loop
                If hv_GraphEvent.I() Then
                    analyze_graph_event(ho_Image, hv_MouseMapping, hv_GraphButton, hv_GraphButtonRow, _
                        hv_GraphButtonColumn, hv_WindowHandle, hv_WindowHandleBuffer, hv_VirtualTrackball, _
                        hv_TrackballSize, hv_SelectedObject, hv_ObjectModel3D, hv_CamParam_COPY_INP_TMP, _
                        hv_Label_COPY_INP_TMP, hv_Title, hv_Information, hv_GenParamName_COPY_INP_TMP, _
                        hv_GenParamValue_COPY_INP_TMP, hv_Poses, hv_ButtonHold, hv_TBCenter, _
                        hv_TBSize, hv_WindowCenteredRotation, hv_MaxNumModels, hv_Poses, hv_SelectedObject, _
                        hv_ButtonHold, hv_WindowCenteredRotation)
                End If
                If hv_Exit.I() Then
                    Exit Do
                End If
#If USE_DO_EVENTS Then
        ' Please note: The call of DoEvents() is only a hack to
        ' enable VB to react on events. Please change the code
        ' so that it can handle events in a standard way.
        System.Windows.Forms.Application.DoEvents()
#End If
            Loop
            '
            'Compute the output pose
            If ExpGetGlobalVar_gIsSinglePose().I() Then
                hv_PoseOut = hv_Poses.TupleSelectRange(New HTuple(0), New HTuple(6))
            Else
                hv_PoseOut = hv_Poses.Clone()
            End If
            '
            'Clean up
            HOperatorSet.SetSystem(New HTuple("clip_region"), hv_ClipRegion)
            HOperatorSet.CloseWindow(hv_WindowHandleBuffer)
            ' dev_set_preferences(...); only in hdevelop
            ' dev_set_preferences(...); only in hdevelop
            ' dev_set_preferences(...); only in hdevelop
            HDevWindowStack.SetActive(hv_WindowHandle)
            If New HTuple(hv_UseBackground.TupleEqual(New HTuple("true"))).I() Then
                dump_image_output(ho_Image, hv_WindowHandle, hv_ObjectModel3D, hv_GenParamName_COPY_INP_TMP, _
                    hv_GenParamValue_COPY_INP_TMP, hv_CamParam_COPY_INP_TMP, hv_Poses, hv_ColorImage, _
                    hv_Title, New HTuple(), hv_Label_COPY_INP_TMP, New HTuple(0), New HTuple("false"), _
                    hv_TrackballCenterRow, hv_TrackballCenterCol, hv_TBSize, hv_SelectedObject, _
                    hv_WindowCenteredRotation, hv_TBCenter)
            Else
                If (HDevWindowStack.IsOpen()) Then
                    HOperatorSet.ClearWindow(HDevWindowStack.GetActive())
                End If
            End If
            ho_Image.Dispose()
            ho_ImageDump.Dispose()

            Exit Sub
        Catch HDevExpDefaultException As HalconException
            ho_Image.Dispose()
            ho_ImageDump.Dispose()

        End Try
    End Sub


End Module
