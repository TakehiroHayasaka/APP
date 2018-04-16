Public Class JManButtonFile

    '-------------- SKLタブのFile名 --------------
    Public _sklButton1File As String = ""
    Public _sklButton2File As String = ""
    Public _sklButton3File As String = ""
    Public _sklButton4File As String = ""
    Public _sklButton5File As String = ""
    Public _sklButton6File As String = ""
    Public _sklButton7File As String = ""
    '-------------- SKLタブの引数 --------------
    Public _sklButton1Arg As String = ""
    Public _sklButton2Arg As String = ""
    Public _sklButton3Arg As String = ""
    Public _sklButton4Arg As String = ""
    Public _sklButton5Arg As String = ""
    Public _sklButton6Arg As String = ""
    Public _sklButton7Arg As String = ""

End Class

Public Class JManExeFile

    '-------------- メニューボタン --------------
    'Jconsoleのexeファイル名
    Public _jconsoleExe As String = "Jconsole.exe"
    'JLogViewのexeファイル名
    Public _jlogViewExe As String = "JlogView.exe"
    'JWeightのexeファイル名
    Public _jweightExe As String = "Jweight.exe"
    'Jpmfdrwのexeファイル名
    Public _jpmfdrwExe As String = "Jpmfdrw.exe"
    'Jcloutのexeファイル名
    Public _jcloutExe As String = "Jclout.exe"
    'DxfPlotのexeファイル名
    Public _dxfPlotExe As String = "DxfPlot.exe"

    '-------------- 骨組関連 --------------
    'JHONEのexeファイル名
    Public _jhoneExe As String = "JHONE.exe"

    '-------------- 部材関連 --------------
    'JSCRBUILDのexeファイル名
    Public _jscrBuildExe As String = "JSCRBUID.EXE"
    'KanshoChkのexeファイル名
    Public _kanshoChkExe As String = "KanshoChk.exe"
    'KanshoChkのexeファイル名
    Public _kanshoChkViewerExe As String = "KanshoChkViewer.exe"
    'BuzaiToueiのexeファイル名
    Public _buzaiToueiExe As String = "BuzaiTouei.exe"
    'ToueiTorikomiのexeファイル名
    Public _toueiTorikomiExe As String = "ToueiTorikomi.exe"

    '-------------- 生産関連 --------------
    'PMF頭文字付加のexeファイル名
    Public _jPmarkExe As String = "JPMARK.EXE"
    'BMF頭文字付加のexeファイル名
    Public _jBmarkExe As String = "JBMARK.EXE"

End Class

Public Class JManSettingFile

    '-------------- 共通 --------------
    'const.infフルパス
    Public _constInfFileName As String = ""
    'Standard.datフルパス
    Public _standardFileName As String = ""
    'JlogWorkフルパス
    Public _jlogWorkFileName As String = ""
    'Block.infフルパス
    Public _blockInfFileName As String = ""
    'Block.infフルパス
    Public _orgBlockInfFileName As String = ""
    'Block.infフルパス
    Public _dmyBlockInfFileName As String = ""

    '-------------- 骨組関連 --------------
    'SklPknd.tblフルパス
    Public _sklPkndFileName As String = ""
    'SklData.datフルパス
    Public _sklDataFileName As String = ""

    '-------------- 部材関連 --------------
    'AnaBolt.datフルパス
    Public _anaBoltFileName As String = ""
    'BziHead.datフルパス
    Public _bziHeadFileName As String = ""
    'BziData.datフルパス
    Public _bziDataFileName As String = ""

    '-------------- 生産関連 --------------
    '溶接時期設定ファイル
    Public _weldStageFileName As String = ""
    'JUPITER.DATフルパス
    Public _jupiterDatFileName As String = ""
    'JUPITER.KAKフルパス
    Public _kakFileName As String = ""
    'JUPITER.SYUフルパス
    Public _syuFileName As String = ""
    'JUPITER.LOTフルパス
    Public _lotFileName As String = ""
    'JUPITER.KUB
    Public _kubFileName As String = ""
    'JUPITER.SPL
    Public _splFileName As String = ""
    'JUPITER.ANA
    Public _anaFileName As String = ""
    'JUPITER.KSE
    Public _kseFileName As String = ""
    'CUTOK.SET
    Public _cutOkFileName As String = ""
    'JUPITER.HSR
    Public _hsrFileName As String = ""
    'JUPITER.MGK
    Public _mgkFileName As String = ""
    'JUPITER.PRA
    Public _praFileName As String = ""
    'HENKEI.DEF
    Public _henkeiDefFileName As String = ""
    'PmfHosei.Tbl
    Public _pmfHoseiFileName As String = ""
    'PSORT.MRKフルパス
    Public _psortFileName As String = ""
    'BSORT.MRKフルパス
    Public _bsortFileName As String = ""
    'FMPRESET.MRK
    Public _fmpresetFileName As String = ""
    'PmkhAdd.Datフルパス
    Public _pmkhFileName As String = ""
    'BmkhAdd.Datフルパス
    Public _bmkhFileName As String = ""

    Public Sub OnInitialize(kozoNo As Integer, kojiFolder As String, environFolder As String)

        '共通設定ファイル
        Me._constInfFileName = kojiFolder + "const.inf"
        Me._standardFileName = copySettingFileFromEnviron(kojiFolder, environFolder, "Standard.dat")
        Me._jlogWorkFileName = kojiFolder + "JlogWork.mde"
        Me._blockInfFileName = kojiFolder + "Block.inf"
        Me._orgBlockInfFileName = kojiFolder + "ORGINALBLOCK.INF"
        Me._dmyBlockInfFileName = kojiFolder + "DUMMYBLOCK.INF"

        '骨組関連ファイル
        Me._sklPkndFileName = copySettingFileFromEnviron(kojiFolder, environFolder, "SKL_PKND.TBL")
        Me._sklDataFileName = kojiFolder + "SklData.dat"

        '部材関連ファイル
        Me._anaBoltFileName = copySettingFileFromEnviron(kojiFolder, environFolder, "ANABOLT.DAT")
        Me._bziHeadFileName = copySettingFileFromEnviron(kojiFolder, environFolder, "BziHead.dat")
        If kozoNo = 6 Then
            Me._bziDataFileName = kojiFolder + "SegData.dat"
        Else
            Me._bziDataFileName = kojiFolder + "BziData.dat"
        End If

        '生産関連ファイル
        Me._jupiterDatFileName = kojiFolder + "JUPITER.DAT"
        Me._kakFileName = copySettingFileFromEnviron(kojiFolder, environFolder, "JUPITER.KAK")
        Me._syuFileName = copySettingFileFromEnviron(kojiFolder, environFolder, "JUPITER.SYU")
        Me._lotFileName = copySettingFileFromEnviron(kojiFolder, environFolder, "JUPITER.LOT")
        Me._psortFileName = copySettingFileFromEnviron(kojiFolder, environFolder, "PSORT.MRK")
        Me._bsortFileName = copySettingFileFromEnviron(kojiFolder, environFolder, "BSORT.MRK")

        Me._weldStageFileName = kojiFolder + "PmfWeldStage.tbl"
        Me._kubFileName = kojiFolder + "JUPITER.KUB"
        Me._splFileName = kojiFolder + "JUPITER.SPL"
        Me._anaFileName = kojiFolder + "JUPITER.ANA"
        Me._kseFileName = kojiFolder + "JUPITER.KSE"
        Me._cutOkFileName = kojiFolder + "CUTOK.SET"
        Me._hsrFileName = kojiFolder + "JUPITER.HSR"
        Me._mgkFileName = kojiFolder + "JUPITER.MGK"
        Me._praFileName = kojiFolder + "JUPITER.PRA"
        Me._henkeiDefFileName = kojiFolder + "HENKEI.DEF"
        Me._pmfHoseiFileName = kojiFolder + "PMFHOSEI.TBL"
        Me._fmpresetFileName = kojiFolder + "FMPRESET.MRK"

        Me._pmkhFileName = kojiFolder + "PmkhAdd.Dat"
        Me._bmkhFileName = kojiFolder + "BmkhAdd.Dat"

    End Sub

End Class
