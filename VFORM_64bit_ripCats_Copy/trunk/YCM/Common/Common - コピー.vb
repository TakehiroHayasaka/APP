﻿Option Explicit On
Imports System.Runtime.InteropServices
Imports System
Imports Microsoft.VisualBasic.FileIO
Imports System.Text
Imports System.IO
Imports FBMlib
Imports HalconDotNet

Module Common
    Public Declare Function OmomiExcute Lib _
    "Omomi.dll" (ByVal num As Integer, _
            ByRef Xi As Double, ByRef Yi As Double, ByRef Zi As Double, _
            ByRef Xj As Double, ByRef Yj As Double, ByRef Zj As Double, _
            ByRef Xa As Double, ByRef Ya As Double, ByRef Za As Double, _
            ByRef Xs As Double, ByRef Ys As Double, ByRef Zs As Double) As Integer
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

        'glutSolidSphere(entset_point.screensize / 25 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom), 20, 20) '計測点
        glutSolidSphere(entset_pointUser.screensize / 20 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom), 20, 20) '計測点
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


        If False Then
            glMultMatrixd(resultMNew)
            '  glTranslated(LabelText.x, LabelText.y, LabelText.z)
            If largeFont = True Then
                glScalef(1 / 64 * entset_label.screensize / 100.0 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom) * 2 / 3, 1 / 64 * entset_label.screensize / 100.0 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom) * 2 / 3, 1.0 * 2 / 3)
            Else
                glScalef(1 / 84 * entset_label.screensize / 100.0 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom) * 2 / 3, 1 / 84 * entset_label.screensize / 100.0 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom) * 2 / 3, 1.0 * 2 / 3)
            End If


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
    Public modelPoints As LoadResult
    Public meshes As List(Of ObjMesh) = New List(Of ObjMesh)
    Private isOnlyPoints As Boolean = True
    Public Function YCM_DrawObjModel3d(ByVal fileName As String, ByVal reload As Boolean)
        If modelPoints Is Nothing Or reload Then
            Dim objLoaderFactory = New ObjLoaderFactory()
            Dim ObjLoader = objLoaderFactory.Create()
            modelPoints = ObjLoader.Load(fileName)
            Dim converter = New ObjToMeshConverter()
            meshes = converter.Convert(modelPoints)
            If meshes.Count > 0 Then
                modelPoints = Nothing
                modelPoints = New LoadResult() 'Clearing memory
                isOnlyPoints = False
            End If
        End If

        glMatrixMode(GL_MODELVIEW)
        glPushMatrix()
        If isOnlyPoints Then
            glBegin(GL_POINTS)
            glPointSize(0.1)
            glColor3ub(255, 0, 0)
            For Each vertex In modelPoints.Vertices
                glVertex3f(vertex.x, vertex.y, vertex.z)
            Next

        Else
            glBegin(GL_TRIANGLES)
            For Each mesh In meshes
                For i = 0 To mesh.Triangles.Count - 1 Step 3
                    Dim triangle1 = mesh.Triangles(i)
                    Dim triangle2 = mesh.Triangles(i + 1)
                    Dim triangle3 = mesh.Triangles(i + 2)
                    Dim normal = mesh.Normals(i)
                    glNormal3f(normal.x, normal.y, normal.z)
                    glVertex3f(triangle1.x, triangle1.y, triangle1.z)
                    glVertex3f(triangle2.x, triangle2.y, triangle2.z)
                    glVertex3f(triangle3.x, triangle3.y, triangle3.z)
                Next
            Next
        End If
        glEnd()
        glPopMatrix()
        Return True
    End Function



    '20170105 baluu add end

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
        Dim Xj(6) As Double, Yj(6) As Double, Zj(6) As Double '--設計点：Xj(),Yj(),Zj()
        Dim Xs(6) As Double, Ys(6) As Double, Zs(6) As Double '--計測点：Xs(),Ys(),Zs()
        Dim Xi(6) As Double, Yi(6) As Double, Zi(6) As Double '--重み　：Xi(),Yi(),Zi()
        Dim Xa(6) As Double, Ya(6) As Double, Za(6) As Double '--重みを考慮したジャストフィット計算結果：Xa(),Ya(),Za()

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
        Xs(3) = 0.0# : Ys(3) = 0.0# : Zs(3) = 0.0#
        Xs(4) = 1.0# : Ys(4) = 0.0# : Zs(4) = 0.0#
        Xs(5) = 0.0# : Ys(5) = 1.0# : Zs(5) = 0.0#
        Xs(6) = 0.0# : Ys(6) = 0.0# : Zs(6) = 1.0#

        '--4.重みを考慮したジャストフィット計算：結果：Xa(),Ya(),Za()
        Dim num As Integer
        Dim iRet As Integer
        num = 7

        iRet = OmomiExcute(num, Xi(0), Yi(0), Zi(0), Xj(0), Yj(0), Zj(0), Xa(0), Ya(0), Za(0), Xs(0), Ys(0), Zs(0))
        If iRet <> 0 Then
        End If

        '--5.座標変換マトリックスを作成する
        Dim gmat As New GeoMatrix
        Dim gpOrg As New GeoPoint
        Dim gvX As New GeoVector, gvY As New GeoVector, gvZ As New GeoVector
        Call gpOrg.setXYZ(Xa(3), Ya(3), Za(3))
        Call gvX.setXYZ(Xa(4), Ya(4), Za(4))
        Call gvY.setXYZ(Xa(5), Ya(5), Za(5))
        Call gvZ.setXYZ(Xa(6), Ya(6), Za(6))
        Call gvX.SubtractPoint(gpOrg)
        Call gvY.SubtractPoint(gpOrg)
        Call gvZ.SubtractPoint(gpOrg)
        Call gmat.SetCoordSystem(gpOrg, gvX, gvY, gvZ)
        'gpOrg.Transform()
        'gmat.PrintClass
        Call gmat.Invert()
        'sys_CoordInfo.mat_geo = gmat.Copy
        'gmat.PrintClass

        '--6.結果を変換
        Dim ind As Integer, i As Integer, j As Integer
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

    '20170302 baluu add start

    Public Function ReconstructScene(ByRef SettingsData As SettingsTable, ByVal images As List(Of ImageSet)) As Boolean
        ReconstructScene = False

        Dim hv_T0 As Object = Nothing, hv_T1 As Object = Nothing, numPoints As Object = Nothing
        Dim hv_CameraSetupModelID As Object = Nothing, hv_CameraParam As Object = Nothing, hv_StereoModelID As Object = Nothing
        Dim ho_Images As Object = Nothing
        Dim hv_ObjectModel3D As Object = Nothing, hv_BoundingBoxObject As Object = Nothing, hv_ObjectModel3DNormals As Object = Nothing, HM3D As Object = Nothing, HM3DI As Object = Nothing
        Dim hv_PoseIn As Object = Nothing, hv_PoseOut As Object = Nothing

        HOperatorSet.GenEmptyObj(ho_Images)
        Try
            HOperatorSet.ReadCamPar(SettingsData.camera_param, hv_CameraParam)
        Catch ex As Exception
            HOperatorSet.ReadTuple(SettingsData.camera_param, hv_CameraParam)
        End Try

        If BTuple.TupleLength(hv_CameraParam).I <= 0 Then
            MsgBox("Invalid Camera Parameters")
            Throw New Exception("Invalid Camera Parameters")
            Exit Function
        End If

        Try
            HOperatorSet.CreateCameraSetupModel(images.Count, hv_CameraSetupModelID)
            For index As Integer = 0 To images.Count - 1
                HOperatorSet.SetCameraSetupCamParam(hv_CameraSetupModelID, index, "area_scan_polynomial", hv_CameraParam, images(index).ImagePose.Pose)
            Next
            HOperatorSet.CreateStereoModel(hv_CameraSetupModelID, "surface_pairwise", New HTuple, New HTuple, hv_StereoModelID)
            '======================================SetStereoModel=========================================
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "persistence", New HTuple(IIf(SettingsData.persistence, 1, 0)))
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "rectif_interpolation", SettingsData.rectif_interpolation)
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "rectif_sub_sampling", Convert.ToDouble(SettingsData.rectif_sub_sampling))
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
            Dim OTemp(10) As Object
            Dim ho_Img As Object = Nothing, ho_ZImg As Object = Nothing
            HOperatorSet.GenEmptyObj(ho_Images)
            HOperatorSet.GenEmptyObj(ho_Img)
            HOperatorSet.GenEmptyObj(ho_ZImg)
            Try
                ClearHObject(ho_Images)
                HOperatorSet.ReadImage(ho_Images, images(0).ImageFullPath)
                HOperatorSet.ZoomImageSize(ho_Images, ho_Images, BTuple.TupleSelect(hv_CameraParam, 10), BTuple.TupleSelect(hv_CameraParam, 11), "constant")
                For i As Integer = 1 To images.Count - 1
                    HOperatorSet.ReadImage(ho_Img, images(i).ImageFullPath)
                    HOperatorSet.ZoomImageSize(ho_Img, ho_ZImg, BTuple.TupleSelect(hv_CameraParam, 10), BTuple.TupleSelect(hv_CameraParam, 11), "constant")
                    HOperatorSet.ConcatObj(ho_Images, ho_ZImg, OTemp(0))
                    ClearHObject(ho_Images)
                    ho_Images = OTemp(0)
                    ClearHObject(ho_Img)
                    ClearHObject(ho_ZImg)
                Next
            Catch ex As Exception
                Throw ex
            End Try

            '======================================ReadMultiViewStereoImages===============================
            HOperatorSet.CountSeconds(hv_T1)
            SettingsData.ImageHenkanTime = BTuple.TupleSub(hv_T1, hv_T0).ToString()

            '======================================CalculateBoundingBox====================================
            Dim ImageRegion As Object = Nothing, DirectBoundingBox As Object = Nothing
            Dim ReducedBox As Object = Nothing, TempBox As Object = Nothing, ConvexBox As Object = Nothing, SampleBox As Object = Nothing, TriangulatedBox As Object = Nothing

            HOperatorSet.GenRectangle1(ImageRegion, 0, 0, BTuple.TupleSelect(hv_CameraParam, 11), BTuple.TupleSelect(hv_CameraParam, 10))
            '======================================GenHomMatPyramid====================================
            Dim PX As Object = Nothing, PY As Object = Nothing, PZ As Object = Nothing
            Dim QX As Object = Nothing, QY As Object = Nothing, QZ As Object = Nothing
            Dim XX As Object = Nothing, XY As Object = Nothing, XZ As Object = Nothing

            Dim LineOfSightRow As Object = Nothing
            LineOfSightRow = BTuple.TupleConcat(LineOfSightRow, 0.0)
            LineOfSightRow = BTuple.TupleConcat(LineOfSightRow, 0.0)
            LineOfSightRow = BTuple.TupleConcat(LineOfSightRow, BTuple.TupleSelect(hv_CameraParam, 11))
            LineOfSightRow = BTuple.TupleConcat(LineOfSightRow, BTuple.TupleSelect(hv_CameraParam, 11))
            Dim LineOfSightCol As Object = Nothing
            LineOfSightCol = BTuple.TupleConcat(LineOfSightCol, 0.0)
            LineOfSightCol = BTuple.TupleConcat(LineOfSightCol, BTuple.TupleSelect(hv_CameraParam, 10))
            LineOfSightCol = BTuple.TupleConcat(LineOfSightCol, BTuple.TupleSelect(hv_CameraParam, 10))
            LineOfSightCol = BTuple.TupleConcat(LineOfSightCol, 0.0)

            Dim HomMat3D As Object = Nothing, HomMat3DIdentity As Object = Nothing, HomMat3DScale As Object = Nothing, Diameter As Object = Nothing
            Dim LineOfSightPoints As Object = Nothing, LineOfSightObject As Object = Nothing
            Dim LineOfSightObjectSample As Object = Nothing, LineOfSightObjectTriangulated As Object = Nothing, LineOfSightObjectScaled As Object = Nothing

            If Not images(0).Region Is Nothing Then 'TODO
                Dim r_Rows As Object = Nothing, r_Cols As Object = Nothing
                HOperatorSet.GetRegionConvex(images(0).Region, r_Rows, r_Cols)
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
            HOperatorSet.TupleReplace(hv_CameraParam, New HTuple(1, 2, 3, 4, 5), 0, hv_CameraParam)
            For i As Integer = 1 To images.Count - 1
                HOperatorSet.CopyObjectModel3d(ConvexBox, "all", TempBox)
                HOperatorSet.ClearObjectModel3d(ConvexBox)
                HOperatorSet.PoseToHomMat3d(images(i).ImagePose.Pose, HM3D)
                HOperatorSet.HomMat3dInvert(HM3D, HM3DI)
                HOperatorSet.HomMat3dToPose(HM3DI, hv_PoseIn)
                If images(i).Region Is Nothing Then
                    HOperatorSet.ReduceObjectModel3dByView(ImageRegion, TempBox, hv_CameraParam, hv_PoseIn, ReducedBox)
                Else
                    HOperatorSet.ReduceObjectModel3dByView(images(i).Region, TempBox, hv_CameraParam, hv_PoseIn, ReducedBox)
                End If
                HOperatorSet.ClearObjectModel3d(TempBox)
                HOperatorSet.ConvexHullObjectModel3d(ReducedBox, ConvexBox)
                HOperatorSet.ClearObjectModel3d(ReducedBox)
                HOperatorSet.GetObjectModel3dParams(ConvexBox, "diameter_axis_aligned_bounding_box", Diameter)
                HOperatorSet.SampleObjectModel3d(ConvexBox, "fast", BTuple.TupleMult(ReconstructionWindow.SamplingFactor, Diameter), New HTuple, New HTuple, SampleBox)
                HOperatorSet.ClearObjectModel3d(ConvexBox)
                HOperatorSet.TriangulateObjectModel3d(SampleBox, "greedy", New HTuple, New HTuple, TriangulatedBox, New HTuple)
                HOperatorSet.ClearObjectModel3d(SampleBox)
                HOperatorSet.CopyObjectModel3d(TriangulatedBox, "all", ConvexBox)
                HOperatorSet.ClearObjectModel3d(TriangulatedBox)
            Next

            HOperatorSet.ClearObj(ImageRegion)
            HOperatorSet.GetObjectModel3DParams(ConvexBox, "bounding_box1", DirectBoundingBox)
            HOperatorSet.SetStereoModelParam(hv_StereoModelID, "bounding_box", DirectBoundingBox)
            HOperatorSet.ClearObjectModel3D(ConvexBox)
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
            HOperatorSet.GetObjectModel3DParams(hv_ObjectModel3D, "num_points", numPoints)
            If BTuple.TupleInt(numPoints) > 0 Then
                '======================================Save3DModel==========================================

                Dim currentDate = System.DateTime.Now.ToString("yyyyMMddHHmmss")
                If Not System.IO.Directory.Exists(MainFrm.objFBM.ProjectPath & "\Pdata\" & currentDate) Then
                    System.IO.Directory.CreateDirectory(MainFrm.objFBM.ProjectPath & "\Pdata\" & currentDate)
                End If
                Dim savePath = MainFrm.objFBM.ProjectPath & "\Pdata\" & currentDate & "\"
                Dim fileName = "ReconstructionResult.obj"
                SettingsData.OutputPath = savePath & fileName
                HOperatorSet.WriteObjectModel3d(hv_ObjectModel3D, "obj", New HTuple(savePath & fileName), New HTuple, New HTuple)
                If Not System.IO.File.Exists(savePath & fileName) Then
                    MsgBox("Failed to write 3d object model")
                End If
                HOperatorSet.ClearObjectModel3d(hv_ObjectModel3D)

                '======================================Save3DModel==========================================
                HOperatorSet.CountSeconds(hv_T1)
                SettingsData.OutputTime = BTuple.TupleSub(hv_T1, hv_T0).ToString()
                ReconstructScene = True
            Else
                MsgBox("Not Enough Points To Visualize")
                Throw New Exception("Not Enough Points To Visualize")
                Exit Function
            End If
            ClearHObject(hv_BoundingBoxObject)
        Catch ex As Exception
            MsgBox("Reconstruction Failed")
            Throw ex
            Exit Function
        End Try
    End Function

    Public Function UnionScene(ByVal scenePath As String) As Boolean
        UnionScene = False
        Dim objectModel3D As Object = Nothing
        Dim PreviousObjectModel As Object = Nothing
        Dim UnionObjectModel As Object = Nothing
        HOperatorSet.ReadObjectModel3d(scenePath, 1, New HTuple, New HTuple, objectModel3D, New HTuple)
        Dim savePath As String = MainFrm.objFBM.ProjectPath & "\Pdata\" & ReconstructionWindow.FileName3D

        If System.IO.File.Exists(savePath) Then
            HOperatorSet.ReadObjectModel3d(savePath, BTuple.TupleAdd(0, 1), "file_type", "obj", PreviousObjectModel, New HTuple)
        End If
        HOperatorSet.UnionObjectModel3D(BTuple.TupleConcat(objectModel3D, PreviousObjectModel), "points_surface", UnionObjectModel)
        HOperatorSet.ClearObjectModel3D(objectModel3D)
        HOperatorSet.ClearObjectModel3D(PreviousObjectModel)
        objectModel3D = UnionObjectModel
        HOperatorSet.WriteObjectModel3d(UnionObjectModel, "obj", savePath, New HTuple, New HTuple)
        If Not System.IO.File.Exists(savePath) Then
            MsgBox("Failed to write 3d object model")
        Else
            UnionScene = True
        End If
    End Function
    '20170302 baluu add end

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

End Module

