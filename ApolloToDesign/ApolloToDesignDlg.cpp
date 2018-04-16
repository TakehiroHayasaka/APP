// ApolloToDesignDlg.cpp : インプリメンテーション ファイル
//

#include "stdafx.h"
#include "ApolloToDesign_main.h"
#include "ApolloToDesignDlg.h"
#include "StringTokenizer.h"
#include "LogPrint.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

#define INPUTSTORAGE "\\ApolloToDesign.gmn"

HINSTANCE hTheApp;
extern CWnd* m_pMainWnd;       // main window

/////////////////////////////////////////////////////////////////////////////
// アプリケーションのバージョン情報で使われている CAboutDlg ダイアログ

class CAboutDlg : public CDialog
{
public:
	CAboutDlg();

// ダイアログ データ
	//{{AFX_DATA(CAboutDlg)
	enum { IDD = IDD_ABOUTBOX };
	//}}AFX_DATA

	// ClassWizard は仮想関数のオーバーライドを生成します
	//{{AFX_VIRTUAL(CAboutDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV のサポート
	//}}AFX_VIRTUAL

// インプリメンテーション
protected:
	//{{AFX_MSG(CAboutDlg)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialog(CAboutDlg::IDD)
{
	//{{AFX_DATA_INIT(CAboutDlg)
	//}}AFX_DATA_INIT
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CAboutDlg)
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialog)
	//{{AFX_MSG_MAP(CAboutDlg)
		// メッセージ ハンドラがありません。
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CApolloToDesignDlg ダイアログ

CApolloToDesignDlg::CApolloToDesignDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CApolloToDesignDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CApolloToDesignDlg)
		// メモ: この位置に ClassWizard によってメンバの初期化が追加されます。
	m_MdbFilePath = _T("");
	m_CsvFilePath = _T("");
	//ジョイントクリアランス
	m_UflgJc = _T("");			//上フランジのジョイントクリアランス
	m_WebJc = _T("");			//ウェブのジョイントクリアランス
	m_LflgJc = _T("");			//下フランジのジョイントクリアランス
	//材端形状
	m_UflgZaitanKeijo = 0;		//上フランジ材端形状
	m_LflgZaitanKeijo = 0;		//下フランジ材端形状
	m_UpdownFlgZaitanKeijoTachiageRyo = _T("");	//上下フランジ材端形状立上げ量
	//ソールプレート
	m_SolePlateKyotyokuFreeSpace = _T("");		//ソールプレートの橋直方向空き量
	m_SolePlateKyojikuFreeSpace = _T("");		//ソールプレートの橋軸方向空き量
	//その他
	m_ItatsugiZureRyo = _T("");					//板継ズレ量
	m_LflgKakuhukubuTaper = _T("");				//下フランジ拡幅部のテーパー勾配
	m_WebHoleSlopeLowerLimitGrd = _T("");		//ウェブ孔の孔勾配の下限値
	//垂直補剛材
	m_ShitenVsCutWu = _T("");					//支点上垂直補剛材の溶接辺側上側切欠
	m_ShitenVsCutWd = _T("");					//支点上垂直補剛材の溶接辺側下側切欠
	m_ShitenVsCutFu = _T("");					//支点上垂直補剛材の上側切欠
	m_KakutenVsCutWu = _T("");					//格点上垂直補剛材の溶接辺側上側切欠
	m_KakutenVsCutWd = _T("");					//格点上垂直補剛材の溶接辺側下側切欠
	m_KakutenVsCutFu = _T("");					//格点上垂直補剛材の上側切欠
	m_MiddleVsCutWu = _T("");					//中間垂直補剛材の溶接辺側上側切欠
	m_MiddleVsCutWd = _T("");					//中間垂直補剛材（圧縮タイプ）の溶接辺側下側切欠
	m_MiddleVsFreeSpace = _T("");				//中間垂直補剛材（引張タイプ）の下側空き量
	//水平補剛材
	m_HsFreeSpaceVs = _T("");					//水平補剛材の垂直補剛材部、横桁ウェブ部空き量
	m_HsFreeSpaceSpl = _T("");					//水平補剛材の添接板部空き量
	m_HsFreeSpaceCbf = _T("");					//水平補剛材の横桁フランジ部空き量
	m_HsFreeSpaceCbfUlimit = _T("");			//水平補剛材の横桁フランジからの高さ寸法上限
	m_HsSnipSizeVs = _T("");					//水平補剛材の垂直補剛材部のスニップサイズ
	m_HsSnipSizeSpl = _T("");					//水平補剛材の添接部のスニップサイズ
	m_HsSnipSizeCbf = _T("");					//水平補剛材の横桁フランジ部のスニップサイズ
	//添接板
	m_UflgSplKyojikuZaitan = _T("");			//上フランジ添接板の橋軸方向材端
	m_UflgOutsideSplKyotyokuZaitan = _T("");	//上フランジ外側添接板の橋直方向材端
	m_UflgInsideSplKyotyokuZaitan = _T("");		//上フランジ内側添接板の橋直方向材端
	m_WebSplKyojikuZaitan = _T("");				//ウェブ添接板の橋軸方向材端
	m_WebSplHeightZaitan = _T("");				//ウェブ添接板の高さ方向材端
	m_LflgSplKyojikuZaitan = _T("");			//下フランジ添接板の橋軸方向材端
	m_LflgSplKyotyokuZaitan = _T("");			//下フランジ添接板の橋直方向材端
	m_ShitenUflgJc = _T("");		//支点上横桁（BH）上フランジのジョイントクリアランス
	m_ShitenWebJc = _T("");			//支点上横桁（BH）ウェブのジョイントクリアランス
	m_ShitenLflgJc = _T("");		//支点上横桁（BH）下フランジのジョイントクリアランス
	m_KakutenHflgJc = _T("");		//格点上横桁（H鋼）フランジのジョイントクリアランス
	m_KakutenHwebJc = _T("");		//格点上横桁（H鋼）ウェブのジョイントクリアランス
	//コネクションプレート
	m_ShitenConnCut = _T("");			//主桁ウェブ付きコネクション（支点上）の溶接辺側切欠
	m_ShitenConnFillet = _T("");		//主桁ウェブ付きコネクション（支点上）のフィレットのRサイズ
	m_ShitenConnTachiageryo = _T("");	//主桁ウェブ付きコネクション（支点上）の溶接辺立上げ量
	m_KakutenConnCut = _T("");			//主桁ウェブ付きコネクション（格点上）の溶接辺側切欠を指定
	m_KakutenConnFillet = _T("");		//主桁ウェブ付きコネクション（格点上）のフィレットのRサイズ
	m_KakutenConnTachiageryo = _T("");	//主桁ウェブ付きコネクション（格点上）の溶接辺立上げ量
	//垂直補剛材
	m_CvsCutWu = _T("");					//横桁付垂直補剛材の溶接辺側上側切欠
	m_CvsCutWd = _T("");					//横桁付垂直補剛材の溶接辺側下側切欠
	//その他
	m_WebHoleSlopeLowerLimitCrs = _T("");	//ウェブ孔の孔勾配の下限値
	m_FlgSectionType = 0;					//フランジ切口の方向
	//材端形状
	m_ShitenUflgSplKyojikuZaitan = _T("");		//支点上横桁上フランジ添接板の橋軸方向材端
	m_ShitenUflgSplKyotyokuZaitan = _T("");		//支点上横桁上フランジ添接板の橋直方向材端
	m_ShitenWebSplKyotyokuZaitan = _T("");		//支点上横桁ウェブ添接板の橋直方向材端
	m_ShitenWebSplHeightZaitan = _T("");		//支点上横桁ウェブ添接板の高さ方向材端
	m_ShitenLflgSplKyojikuZaitan = _T("");		//支点上横桁下フランジ添接板の橋軸方向材端
	m_ShitenLflgSplKyotyokuZaitan = _T("");		//支点上横桁下フランジ添接板の橋直方向材端
	m_ShitenConnKyojikuZaitan = _T("");			//支点上コネクションの橋軸方向材端
	m_ShitenConnKyoutyokuZaitan = _T("");		//支点上コネクションの橋直方向材端
	m_KakutenUflgSplKyojikuZaitan = _T("");		//格点上横桁上フランジ添接板の橋軸方向材端
	m_KakutenUflgSplKyotyokuZaitan = _T("");	//格点上横桁上フランジ添接板の橋直方向材端
	m_KakutenWebSplKyotyokuZaitan = _T("");		//格点上横桁ウェブ添接板の橋直方向材端
	m_KakutenWebSplHeightZaitan = _T("");		//格点上横桁ウェブ添接板の高さ方向材端
	m_KakutenLflgSplKyojikuZaitan = _T("");		//格点上横桁下フランジ添接板の橋軸方向材端
	m_KakutenLflgSplKyotyokuZaitan = _T("");	//格点上横桁下フランジ添接板の橋直方向材端
	m_KakutenConnKyojikuZaitan = _T("");		//格点上コネクションの橋軸方向材端
	//}}AFX_DATA_INIT
	// メモ: LoadIcon は Win32 の DestroyIcon のサブシーケンスを要求しません。
	m_hIcon = ::LoadIcon(hTheApp, MAKEINTRESOURCE(IDR_MAINFRAME));
}

void CApolloToDesignDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CApolloToDesignDlg)
	DDX_Control(pDX, IDC_MAINTAB, m_MainTab);
	DDX_Text(pDX, IDC_MDBFILEPATH, m_MdbFilePath);
	DDX_Text(pDX, IDC_CSVFILEPATH, m_CsvFilePath);
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CApolloToDesignDlg, CDialog)
	//{{AFX_MSG_MAP(CApolloToDesignDlg)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_MDBFILEBROWSE, OnMdbFileBrowse)
	ON_WM_CLOSE()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CApolloToDesignDlg メッセージ ハンドラ
BOOL CApolloToDesignDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// IDM_ABOUTBOX はコマンド メニューの範囲でなければなりません。
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		CString strAboutMenu;
		strAboutMenu.LoadString(IDS_ABOUTBOX);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// このダイアログ用のアイコンを設定します。フレームワークはアプリケーションのメイン
	// ウィンドウがダイアログでない時は自動的に設定しません。
	SetIcon(m_hIcon, TRUE);			// 大きいアイコンを設定
	SetIcon(m_hIcon, FALSE);		// 小さいアイコンを設定
	
	// TODO: 特別な初期化を行う時はこの場所に追加してください。
	m_MainTab.InitDialogs();
	m_MainTab.InsertItem(0, _T("主桁共通詳細"));
	m_MainTab.InsertItem(1, _T("横桁共通詳細"));
	m_MainTab.SetCurSel(0);
	m_MainTab.InitTabDialog();
	//
	initializeGui(0);

	return TRUE;  // TRUE を返すとコントロールに設定したフォーカスは失われません。
}

void CApolloToDesignDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialog::OnSysCommand(nID, lParam);
	}
}

// もしダイアログボックスに最小化ボタンを追加するならば、アイコンを描画する
// コードを以下に記述する必要があります。MFC アプリケーションは document/view
// モデルを使っているので、この処理はフレームワークにより自動的に処理されます。

void CApolloToDesignDlg::OnPaint() 
{
	if (IsIconic())
	{
		CPaintDC dc(this); // 描画用のデバイス コンテキスト

		SendMessage(WM_ICONERASEBKGND, (WPARAM) dc.GetSafeHdc(), 0);

		// クライアントの矩形領域内の中央
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// アイコンを描画します。
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}

// システムは、ユーザーが最小化ウィンドウをドラッグしている間、
// カーソルを表示するためにここを呼び出します。
HCURSOR CApolloToDesignDlg::OnQueryDragIcon()
{
	return (HCURSOR) m_hIcon;
}

void CApolloToDesignDlg::OnMdbFileBrowse() 
{
	UpdateData(TRUE);

	CFileDialog fileDlg(TRUE,_T("mdb"),_T("*.mdb"),OFN_HIDEREADONLY|OFN_OVERWRITEPROMPT,_T("(*.mdb)|*.mdb||"),NULL);
	if (fileDlg.DoModal()==IDOK)
	{
		m_MdbFilePath = fileDlg.GetPathName();
		string sekkeiFilePath = m_MdbFilePath.GetBuffer();
		string dataDir = "";
		if(sekkeiFilePath != "") {
			StringTokenizer strToken(sekkeiFilePath,"\\");
			if (strToken.size2() > 1) {
				int idx = strToken.size2()-2;
				for(int i=0;i<strToken.size2()-2;i++) {
					dataDir += strToken[i];
					dataDir += "\\";
				}
			}
		}
		SetCurrentDirectory(dataDir.c_str());
		initializeGui(1);
	}

	UpdateData(FALSE);
}

void CApolloToDesignDlg::OnOK() 
{
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD3)->GetWindowText(m_UflgJc);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD4)->GetWindowText(m_WebJc);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD5)->GetWindowText(m_LflgJc);
	m_UflgZaitanKeijo = ((CComboBox*)m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD7))->GetCurSel();
	m_LflgZaitanKeijo = ((CComboBox*)m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD8))->GetCurSel();
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD9)->GetWindowText(m_UpdownFlgZaitanKeijoTachiageRyo);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD11)->GetWindowText(m_SolePlateKyotyokuFreeSpace);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD12)->GetWindowText(m_SolePlateKyojikuFreeSpace);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD6)->GetWindowText(m_ItatsugiZureRyo);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD10)->GetWindowText(m_LflgKakuhukubuTaper);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD36)->GetWindowText(m_WebHoleSlopeLowerLimitGrd);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD20)->GetWindowText(m_ShitenVsCutWu);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD21)->GetWindowText(m_ShitenVsCutWd);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD22)->GetWindowText(m_ShitenVsCutFu);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD23)->GetWindowText(m_KakutenVsCutWu);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD24)->GetWindowText(m_KakutenVsCutWd);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD25)->GetWindowText(m_KakutenVsCutFu);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD26)->GetWindowText(m_MiddleVsCutWu);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD27)->GetWindowText(m_MiddleVsCutWd);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD28)->GetWindowText(m_MiddleVsFreeSpace);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD13)->GetWindowText(m_HsFreeSpaceVs);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD14)->GetWindowText(m_HsFreeSpaceSpl);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD15)->GetWindowText(m_HsFreeSpaceCbf);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD16)->GetWindowText(m_HsFreeSpaceCbfUlimit);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD17)->GetWindowText(m_HsSnipSizeVs);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD18)->GetWindowText(m_HsSnipSizeSpl);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD19)->GetWindowText(m_HsSnipSizeCbf);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD29)->GetWindowText(m_UflgSplKyojikuZaitan);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD30)->GetWindowText(m_UflgOutsideSplKyotyokuZaitan);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD31)->GetWindowText(m_UflgInsideSplKyotyokuZaitan);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD32)->GetWindowText(m_WebSplKyojikuZaitan);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD33)->GetWindowText(m_WebSplHeightZaitan);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD34)->GetWindowText(m_LflgSplKyojikuZaitan);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD35)->GetWindowText(m_LflgSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS1)->GetWindowText(m_ShitenUflgJc);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS2)->GetWindowText(m_ShitenWebJc);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS3)->GetWindowText(m_ShitenLflgJc);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS4)->GetWindowText(m_KakutenHflgJc);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS5)->GetWindowText(m_KakutenHwebJc);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS6)->GetWindowText(m_ShitenConnCut);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS7)->GetWindowText(m_ShitenConnFillet);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS8)->GetWindowText(m_ShitenConnTachiageryo);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS9)->GetWindowText(m_KakutenConnCut);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS10)->GetWindowText(m_KakutenConnFillet);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS11)->GetWindowText(m_KakutenConnTachiageryo);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS12)->GetWindowText(m_CvsCutWu);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS13)->GetWindowText(m_CvsCutWd);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS30)->GetWindowText(m_WebHoleSlopeLowerLimitCrs);
	m_FlgSectionType = ((CComboBox*)m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS31))->GetCurSel();
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS14)->GetWindowText(m_ShitenUflgSplKyojikuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS15)->GetWindowText(m_ShitenUflgSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS16)->GetWindowText(m_ShitenWebSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS17)->GetWindowText(m_ShitenWebSplHeightZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS18)->GetWindowText(m_ShitenLflgSplKyojikuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS19)->GetWindowText(m_ShitenLflgSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS20)->GetWindowText(m_ShitenConnKyojikuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS21)->GetWindowText(m_ShitenConnKyoutyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS22)->GetWindowText(m_KakutenUflgSplKyojikuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS23)->GetWindowText(m_KakutenUflgSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS24)->GetWindowText(m_KakutenWebSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS25)->GetWindowText(m_KakutenWebSplHeightZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS26)->GetWindowText(m_KakutenLflgSplKyojikuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS27)->GetWindowText(m_KakutenLflgSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS28)->GetWindowText(m_KakutenConnKyojikuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS29)->GetWindowText(m_KakutenConnKyoutyokuZaitan);
	if(m_UflgJc == "") {			//上フランジのジョイントクリアランス
		AfxMessageBox(_T("上フランジのジョイントクリアランスを入力してください。"));
		return;
	}
	if(m_WebJc == "") {			//ウェブのジョイントクリアランス
		AfxMessageBox(_T("ウェブのジョイントクリアランスを入力してください。"));
		return;
	}
	if(m_LflgJc == "") {			//下フランジのジョイントクリアランス
		AfxMessageBox(_T("下フランジのジョイントクリアランスを入力してください。"));
		return;
	}
	if(m_UpdownFlgZaitanKeijoTachiageRyo == "") {	//上下フランジ材端形状立上げ量
		AfxMessageBox(_T("上下フランジ材端形状立上げ量を入力してください。"));
		return;
	}
	if(m_SolePlateKyotyokuFreeSpace == "") {		//ソールプレートの橋直方向空き量
		AfxMessageBox(_T("ソールプレートの橋直方向空き量を入力してください。"));
		return;
	}
	if(m_SolePlateKyojikuFreeSpace == "") {		//ソールプレートの橋軸方向空き量
		AfxMessageBox(_T("ソールプレートの橋軸方向空き量を入力してください。"));
		return;
	}
	if(m_ItatsugiZureRyo == "") {					//板継ズレ量
		AfxMessageBox(_T("板継ズレ量を入力してください。"));
		return;
	}
	if(m_LflgKakuhukubuTaper == "") {				//下フランジ拡幅部のテーパー勾配
		AfxMessageBox(_T("下フランジ拡幅部のテーパー勾配を入力してください。"));
		return;
	}
	if(m_WebHoleSlopeLowerLimitGrd == "") {		//ウェブ孔の孔勾配の下限値
		AfxMessageBox(_T("ウェブ孔の孔勾配の下限値を入力してください。"));
		return;
	}
	if(m_ShitenVsCutWu == "") {					//支点上垂直補剛材の溶接辺側上側切欠
		AfxMessageBox(_T("支点上垂直補剛材の溶接辺側上側切欠を入力してください。"));
		return;
	}
	if(m_ShitenVsCutWd == "") {					//支点上垂直補剛材の溶接辺側下側切欠
		AfxMessageBox(_T("支点上垂直補剛材の溶接辺側下側切欠を入力してください。"));
		return;
	}
	if(m_ShitenVsCutFu == "") {					//支点上垂直補剛材の上側切欠
		AfxMessageBox(_T("支点上垂直補剛材の上側切欠を入力してください。"));
		return;
	}
	if(m_KakutenVsCutWu == "") {					//格点上垂直補剛材の溶接辺側上側切欠
		AfxMessageBox(_T("格点上垂直補剛材の溶接辺側上側切欠を入力してください。"));
		return;
	}
	if(m_KakutenVsCutWd == "") {					//格点上垂直補剛材の溶接辺側下側切欠
		AfxMessageBox(_T("格点上垂直補剛材の溶接辺側下側切欠を入力してください。"));
		return;
	}
	if(m_KakutenVsCutFu == "") {					//格点上垂直補剛材の上側切欠
		AfxMessageBox(_T("格点上垂直補剛材の上側切欠を入力してください。"));
		return;
	}
	if(m_MiddleVsCutWu == "") {					//中間垂直補剛材の溶接辺側上側切欠
		AfxMessageBox(_T("中間垂直補剛材の溶接辺側上側切欠を入力してください。"));
		return;
	}
	if(m_MiddleVsCutWd == "") {					//中間垂直補剛材（圧縮タイプ）の溶接辺側下側切欠
		AfxMessageBox(_T("中間垂直補剛材（圧縮タイプ）の溶接辺側下側切欠を入力してください。"));
		return;
	}
	if(m_MiddleVsFreeSpace == "") {				//中間垂直補剛材（引張タイプ）の下側空き量
		AfxMessageBox(_T("中間垂直補剛材（引張タイプ）の下側空き量を入力してください。"));
		return;
	}
	if(m_HsFreeSpaceVs == "") {					//水平補剛材の垂直補剛材部、横桁ウェブ部空き量
		AfxMessageBox(_T("水平補剛材の垂直補剛材部、横桁ウェブ部空き量を入力してください。"));
		return;
	}
	if(m_HsFreeSpaceSpl == "") {					//水平補剛材の添接板部空き量
		AfxMessageBox(_T("水平補剛材の添接板部空き量を入力してください。"));
		return;
	}
	if(m_HsFreeSpaceCbf == "") {					//水平補剛材の横桁フランジ部空き量
		AfxMessageBox(_T("水平補剛材の横桁フランジ部空き量を入力してください。"));
		return;
	}
	if(m_HsFreeSpaceCbfUlimit == "") {			//水平補剛材の横桁フランジからの高さ寸法上限
		AfxMessageBox(_T("水平補剛材の横桁フランジからの高さ寸法上限を入力してください。"));
		return;
	}
	if(m_HsSnipSizeVs == "") {					//水平補剛材の垂直補剛材部のスニップサイズ
		AfxMessageBox(_T("水平補剛材の垂直補剛材部のスニップサイズを入力してください。"));
		return;
	}
	if(m_HsSnipSizeSpl == "") {					//水平補剛材の添接部のスニップサイズ
		AfxMessageBox(_T("水平補剛材の添接部のスニップサイズを入力してください。"));
		return;
	}
	if(m_HsSnipSizeCbf == "") {					//水平補剛材の横桁フランジ部のスニップサイズ
		AfxMessageBox(_T("水平補剛材の横桁フランジ部のスニップサイズを入力してください。"));
		return;
	}
	if(m_UflgSplKyojikuZaitan == "") {			//上フランジ添接板の橋軸方向材端
		AfxMessageBox(_T("上フランジ添接板の橋軸方向材端を入力してください。"));
		return;
	}
	if(m_UflgOutsideSplKyotyokuZaitan == "") {	//上フランジ外側添接板の橋直方向材端
		AfxMessageBox(_T("上フランジ外側添接板の橋直方向材端を入力してください。"));
		return;
	}
	if(m_UflgInsideSplKyotyokuZaitan == "") {		//上フランジ内側添接板の橋直方向材端
		AfxMessageBox(_T("上フランジ内側添接板の橋直方向材端を入力してください。"));
		return;
	}
	if(m_WebSplKyojikuZaitan == "") {				//ウェブ添接板の橋軸方向材端
		AfxMessageBox(_T("ウェブ添接板の橋軸方向材端を入力してください。"));
		return;
	}
	if(m_WebSplHeightZaitan == "") {				//ウェブ添接板の高さ方向材端
		AfxMessageBox(_T("ウェブ添接板の高さ方向材端を入力してください。"));
		return;
	}
	if(m_LflgSplKyojikuZaitan == "") {			//下フランジ添接板の橋軸方向材端
		AfxMessageBox(_T("下フランジ添接板の橋軸方向材端を入力してください。"));
		return;
	}
	if(m_LflgSplKyotyokuZaitan == "") {			//下フランジ添接板の橋直方向材端
		AfxMessageBox(_T("下フランジ添接板の橋直方向材端を入力してください。"));
		return;
	}
	if(m_ShitenUflgJc == "") {		//支点上横桁（BH）上フランジのジョイントクリアランス
		AfxMessageBox(_T("支点上横桁（BH）上フランジのジョイントクリアランスを入力してください。"));
		return;
	}
	if(m_ShitenWebJc == "") {			//支点上横桁（BH）ウェブのジョイントクリアランス
		AfxMessageBox(_T("支点上横桁（BH）ウェブのジョイントクリアランスを入力してください。"));
		return;
	}
	if(m_ShitenLflgJc == "") {		//支点上横桁（BH）下フランジのジョイントクリアランス
		AfxMessageBox(_T("支点上横桁（BH）下フランジのジョイントクリアランスを入力してください。"));
		return;
	}
	if(m_KakutenHflgJc == "") {		//格点上横桁（H鋼）フランジのジョイントクリアランス
		AfxMessageBox(_T("格点上横桁（H鋼）フランジのジョイントクリアランスを入力してください。"));
		return;
	}
	if(m_KakutenHwebJc == "") {		//格点上横桁（H鋼）ウェブのジョイントクリアランス
		AfxMessageBox(_T("格点上横桁（H鋼）ウェブのジョイントクリアランスを入力してください。"));
		return;
	}
	if(m_ShitenConnCut == "") {			//主桁ウェブ付きコネクション（支点上）の溶接辺側切欠
		AfxMessageBox(_T("主桁ウェブ付きコネクション（支点上）の溶接辺側切欠を入力してください。"));
		return;
	}
	if(m_ShitenConnFillet == "") {		//主桁ウェブ付きコネクション（支点上）のフィレットのRサイズ
		AfxMessageBox(_T("主桁ウェブ付きコネクション（支点上）のフィレットのRサイズを入力してください。"));
		return;
	}
	if(m_ShitenConnTachiageryo == "") {	//主桁ウェブ付きコネクション（支点上）の溶接辺立上げ量
		AfxMessageBox(_T("主桁ウェブ付きコネクション（支点上）の溶接辺立上げ量を入力してください。"));
		return;
	}
	if(m_KakutenConnCut == "") {			//主桁ウェブ付きコネクション（格点上）の溶接辺側切欠を指定
		AfxMessageBox(_T("主桁ウェブ付きコネクション（格点上）の溶接辺側切欠を指定を入力してください。"));
		return;
	}
	if(m_KakutenConnFillet == "") {		//主桁ウェブ付きコネクション（格点上）のフィレットのRサイズ
		AfxMessageBox(_T("主桁ウェブ付きコネクション（格点上）のフィレットのRサイズを入力してください。"));
		return;
	}
	if(m_KakutenConnTachiageryo == "") {	//主桁ウェブ付きコネクション（格点上）の溶接辺立上げ量
		AfxMessageBox(_T("主桁ウェブ付きコネクション（格点上）の溶接辺立上げ量を入力してください。"));
		return;
	}
	if(m_CvsCutWu == "") {					//横桁付垂直補剛材の溶接辺側上側切欠
		AfxMessageBox(_T("横桁付垂直補剛材の溶接辺側上側切欠を入力してください。"));
		return;
	}
	if(m_CvsCutWd == "") {					//横桁付垂直補剛材の溶接辺側下側切欠
		AfxMessageBox(_T("横桁付垂直補剛材の溶接辺側下側切欠を入力してください。"));
		return;
	}
	if(m_WebHoleSlopeLowerLimitCrs == "") {	//ウェブ孔の孔勾配の下限値
		AfxMessageBox(_T("ウェブ孔の孔勾配の下限値を入力してください。"));
		return;
	}
	if(m_ShitenUflgSplKyojikuZaitan == "") {		//支点上横桁上フランジ添接板の橋軸方向材端
		AfxMessageBox(_T("支点上横桁上フランジ添接板の橋軸方向材端を入力してください。"));
		return;
	}
	if(m_ShitenUflgSplKyotyokuZaitan == "") {		//支点上横桁上フランジ添接板の橋直方向材端
		AfxMessageBox(_T("支点上横桁上フランジ添接板の橋直方向材端を入力してください。"));
		return;
	}
	if(m_ShitenWebSplKyotyokuZaitan == "") {		//支点上横桁ウェブ添接板の橋直方向材端
		AfxMessageBox(_T("支点上横桁ウェブ添接板の橋直方向材端を入力してください。"));
		return;
	}
	if(m_ShitenWebSplHeightZaitan == "") {		//支点上横桁ウェブ添接板の高さ方向材端
		AfxMessageBox(_T("支点上横桁ウェブ添接板の高さ方向材端を入力してください。"));
		return;
	}
	if(m_ShitenLflgSplKyojikuZaitan == "") {		//支点上横桁下フランジ添接板の橋軸方向材端
		AfxMessageBox(_T("支点上横桁下フランジ添接板の橋軸方向材端を入力してください。"));
		return;
	}
	if(m_ShitenLflgSplKyotyokuZaitan == "") {		//支点上横桁下フランジ添接板の橋直方向材端
		AfxMessageBox(_T("支点上横桁下フランジ添接板の橋直方向材端を入力してください。"));
		return;
	}
	if(m_ShitenConnKyojikuZaitan == "") {			//支点上コネクションの橋軸方向材端
		AfxMessageBox(_T("支点上コネクションの橋軸方向材端を入力してください。"));
		return;
	}
	if(m_ShitenConnKyoutyokuZaitan == "") {		//支点上コネクションの橋直方向材端
		AfxMessageBox(_T("支点上コネクションの橋直方向材端を入力してください。"));
		return;
	}
	if(m_KakutenUflgSplKyojikuZaitan == "") {		//格点上横桁上フランジ添接板の橋軸方向材端
		AfxMessageBox(_T("格点上横桁上フランジ添接板の橋軸方向材端を入力してください。"));
		return;
	}
	if(m_KakutenUflgSplKyotyokuZaitan == "") {	//格点上横桁上フランジ添接板の橋直方向材端
		AfxMessageBox(_T("格点上横桁上フランジ添接板の橋直方向材端を入力してください。"));
		return;
	}
	if(m_KakutenWebSplKyotyokuZaitan == "") {		//格点上横桁ウェブ添接板の橋直方向材端
		AfxMessageBox(_T("格点上横桁ウェブ添接板の橋直方向材端を入力してください。"));
		return;
	}
	if(m_KakutenWebSplHeightZaitan == "") {		//格点上横桁ウェブ添接板の高さ方向材端
		AfxMessageBox(_T("格点上横桁ウェブ添接板の高さ方向材端を入力してください。"));
		return;
	}
	if(m_KakutenLflgSplKyojikuZaitan == "") {		//格点上横桁下フランジ添接板の橋軸方向材端
		AfxMessageBox(_T("格点上横桁下フランジ添接板の橋軸方向材端を入力してください。"));
		return;
	}
	if(m_KakutenLflgSplKyotyokuZaitan == "") {	//格点上横桁下フランジ添接板の橋直方向材端
		AfxMessageBox(_T("格点上横桁下フランジ添接板の橋直方向材端を入力してください。"));
		return;
	}
	if(m_KakutenConnKyojikuZaitan == "") {		//格点上コネクションの橋軸方向材端
		AfxMessageBox(_T("格点上コネクションの橋軸方向材端を入力してください。"));
		return;
	}
	AtdGirderCommon agc;
	//ジョイントクリアランス
	agc.setUflgJc(atof(m_UflgJc));	//上フランジのジョイントクリアランス
	agc.setWebJc(atof(m_WebJc));	//ウェブのジョイントクリアランス
	agc.setLflgJc(atof(m_LflgJc));	//下フランジのジョイントクリアランス
	//材端形状
	EnTypeNo enTypeNo;
	if(m_UflgZaitanKeijo == 0) {
		enTypeNo = TYPE1;	//	タイプ1
	} else if(m_UflgZaitanKeijo == 1) {
		enTypeNo = TYPE2;	//	タイプ2
	} else if(m_UflgZaitanKeijo == 2) {
		enTypeNo = TYPE3;	//	タイプ3
	} else {
		enTypeNo = ERR_ETN;	//	エラーチェック用
	}
	agc.setUflgZaitanKeijo(enTypeNo);	//上フランジ材端形状
	if(m_LflgZaitanKeijo == 0) {
		enTypeNo = TYPE1;	//	タイプ1
	} else if(m_LflgZaitanKeijo == 1) {
		enTypeNo = TYPE2;	//	タイプ2
	} else if(m_LflgZaitanKeijo == 2) {
		enTypeNo = TYPE3;	//	タイプ3
	} else {
		enTypeNo = ERR_ETN;	//	エラーチェック用
	}
	agc.setLflgZaitanKeijo(enTypeNo);	//下フランジ材端形状
	agc.setUpdownFlgZaitanKeijoTachiageRyo(atof(m_UpdownFlgZaitanKeijoTachiageRyo));	//上下フランジ材端形状立上げ量
	//ソールプレート
	agc.setSolePlateKyotyokuFreeSpace(atof(m_SolePlateKyotyokuFreeSpace));	//ソールプレートの橋直方向空き量
	agc.setSolePlateKyojikuFreeSpace(atof(m_SolePlateKyojikuFreeSpace));	//ソールプレートの橋軸方向空き量
	//その他
	agc.setItatsugiZureRyo(atof(m_ItatsugiZureRyo));	//板継ズレ量
	agc.setLflgKakuhukubuTaper(atof(m_LflgKakuhukubuTaper));	//下フランジ拡幅部のテーパー勾配
	agc.setWebHoleSlopeLowerLimitGrd(atof(m_WebHoleSlopeLowerLimitGrd));	//ウェブ孔の孔勾配の下限値
	//垂直補剛材
	agc.setShitenVsCutWu(m_ShitenVsCutWu.GetBuffer());	//支点上垂直補剛材の溶接辺側上側切欠
	agc.setShitenVsCutWd(m_ShitenVsCutWd.GetBuffer());	//支点上垂直補剛材の溶接辺側下側切欠
	agc.setShitenVsCutFu(m_ShitenVsCutFu.GetBuffer());	//支点上垂直補剛材の上側切欠
	agc.setKakutenVsCutWu(m_KakutenVsCutWu.GetBuffer());	//格点上垂直補剛材の溶接辺側上側切欠
	agc.setKakutenVsCutWd(m_KakutenVsCutWd.GetBuffer());	//格点上垂直補剛材の溶接辺側下側切欠
	agc.setKakutenVsCutFu(m_KakutenVsCutFu.GetBuffer());	//格点上垂直補剛材の上側切欠
	agc.setMiddleVsCutWu(m_MiddleVsCutWu.GetBuffer());	//中間垂直補剛材の溶接辺側上側切欠
	agc.setMiddleVsCutWd(m_MiddleVsCutWd.GetBuffer());	//中間垂直補剛材（圧縮タイプ）の溶接辺側下側切欠
	agc.setMiddleVsFreeSpace(atof(m_MiddleVsFreeSpace));	//中間垂直補剛材（引張タイプ）の下側空き量
	//水平補剛材
	agc.setHsFreeSpaceVs(atof(m_HsFreeSpaceVs));	//水平補剛材の垂直補剛材部、横桁ウェブ部空き量
	agc.setHsFreeSpaceSpl(atof(m_HsFreeSpaceSpl));	//水平補剛材の添接板部空き量
	agc.setHsFreeSpaceCbf(atof(m_HsFreeSpaceCbf));	//水平補剛材の横桁フランジ部空き量
	agc.setHsFreeSpaceCbfUlimit(atof(m_HsFreeSpaceCbfUlimit));	//水平補剛材の横桁フランジからの高さ寸法上限
	agc.setHsSnipSizeVs(m_HsSnipSizeVs.GetBuffer());	//水平補剛材の垂直補剛材部のスニップサイズ
	agc.setHsSnipSizeSpl(m_HsSnipSizeSpl.GetBuffer());	//水平補剛材の添接部のスニップサイズ
	agc.setHsSnipSizeCbf(m_HsSnipSizeCbf.GetBuffer());	//水平補剛材の横桁フランジ部のスニップサイズ
	//添接板
	agc.setUflgSplKyojikuZaitan(atof(m_UflgSplKyojikuZaitan));	//上フランジ添接板の橋軸方向材端
	agc.setUflgOutsideSplKyotyokuZaitan(atof(m_UflgOutsideSplKyotyokuZaitan));	//上フランジ外側添接板の橋直方向材端
	agc.setUflgInsideSplKyotyokuZaitan(atof(m_UflgInsideSplKyotyokuZaitan));	//上フランジ内側添接板の橋直方向材端
	agc.setWebSplKyojikuZaitan(atof(m_WebSplKyojikuZaitan));	//ウェブ添接板の橋軸方向材端
	agc.setWebSplHeightZaitan(atof(m_WebSplHeightZaitan));	//ウェブ添接板の高さ方向材端
	agc.setLflgSplKyojikuZaitan(atof(m_LflgSplKyojikuZaitan));	//下フランジ添接板の橋軸方向材端
	agc.setLflgSplKyotyokuZaitan(atof(m_LflgSplKyotyokuZaitan));	//下フランジ添接板の橋直方向材端
	//
	AtdCrossBeamCommon acc;
	//ジョイントクリアランス
	acc.setShitenUflgJc(atof(m_ShitenUflgJc));	//支点上横桁（BH）上フランジのジョイントクリアランス
	acc.setShitenWebJc(atof(m_ShitenWebJc));	//支点上横桁（BH）ウェブのジョイントクリアランス
	acc.setShitenLflgJc(atof(m_ShitenLflgJc));	//支点上横桁（BH）下フランジのジョイントクリアランス
	acc.setKakutenHflgJc(atof(m_KakutenHflgJc));	//格点上横桁（H鋼）フランジのジョイントクリアランス
	acc.setKakutenHwebJc(atof(m_KakutenHwebJc));	//格点上横桁（H鋼）ウェブのジョイントクリアランス
	//コネクションプレート
	acc.setShitenConnCut(m_ShitenConnCut.GetBuffer());	//主桁ウェブ付きコネクション（支点上）の溶接辺側切欠
	acc.setShitenConnFillet(atof(m_ShitenConnFillet));	//主桁ウェブ付きコネクション（支点上）のフィレットのRサイズ
	acc.setShitenConnTachiageryo(atof(m_ShitenConnTachiageryo));	//主桁ウェブ付きコネクション（支点上）の溶接辺立上げ量
	acc.setKakutenConnCut(m_KakutenConnCut.GetBuffer());	//主桁ウェブ付きコネクション（格点上）の溶接辺側切欠を指定
	acc.setKakutenConnFillet(atof(m_KakutenConnFillet));	//主桁ウェブ付きコネクション（格点上）のフィレットのRサイズ
	acc.setKakutenConnTachiageryo(atof(m_KakutenConnTachiageryo));	//主桁ウェブ付きコネクション（格点上）の溶接辺立上げ量
	//垂直補剛材
	acc.setCvsCutWu(m_CvsCutWu.GetBuffer());	//横桁付垂直補剛材の溶接辺側上側切欠
	acc.setCvsCutWd(m_CvsCutWd.GetBuffer());	//横桁付垂直補剛材の溶接辺側下側切欠
	//その他
	acc.setWebHoleSlopeLowerLimitCrs(atof(m_WebHoleSlopeLowerLimitCrs));	//ウェブ孔の孔勾配の下限値
	if(m_FlgSectionType == 0) {
		enTypeNo = TYPE1;	//	タイプ1
	} else if(m_FlgSectionType == 1) {
		enTypeNo = TYPE2;	//	タイプ2
	} else if(m_FlgSectionType == 2) {
		enTypeNo = TYPE3;	//	タイプ3
	} else {
		enTypeNo = ERR_ETN;	//	エラーチェック用
	}
	acc.setFlgSectionType(enTypeNo);	//フランジ切口の方向
	//材端形状
	acc.setShitenUflgSplKyojikuZaitan(atof(m_ShitenUflgSplKyojikuZaitan));	//支点上横桁上フランジ添接板の橋軸方向材端
	acc.setShitenUflgSplKyotyokuZaitan(atof(m_ShitenUflgSplKyotyokuZaitan));	//支点上横桁上フランジ添接板の橋直方向材端
	acc.setShitenWebSplKyotyokuZaitan(atof(m_ShitenWebSplKyotyokuZaitan));	//支点上横桁ウェブ添接板の橋直方向材端
	acc.setShitenWebSplHeightZaitan(atof(m_ShitenWebSplHeightZaitan));	//支点上横桁ウェブ添接板の高さ方向材端
	acc.setShitenLflgSplKyojikuZaitan(atof(m_ShitenLflgSplKyojikuZaitan));	//支点上横桁下フランジ添接板の橋軸方向材端
	acc.setShitenLflgSplKyotyokuZaitan(atof(m_ShitenLflgSplKyotyokuZaitan));	//支点上横桁下フランジ添接板の橋直方向材端
	acc.setShitenConnKyojikuZaitan(atof(m_ShitenConnKyojikuZaitan));	//支点上コネクションの橋軸方向材端
	acc.setShitenConnKyoutyokuZaitan(atof(m_ShitenConnKyoutyokuZaitan));	//支点上コネクションの橋直方向材端
	acc.setKakutenUflgSplKyojikuZaitan(atof(m_KakutenUflgSplKyojikuZaitan));	//格点上横桁上フランジ添接板の橋軸方向材端
	acc.setKakutenUflgSplKyotyokuZaitan(atof(m_KakutenUflgSplKyotyokuZaitan));	//格点上横桁上フランジ添接板の橋直方向材端
	acc.setKakutenWebSplKyotyokuZaitan(atof(m_KakutenWebSplKyotyokuZaitan));	//格点上横桁ウェブ添接板の橋直方向材端
	acc.setKakutenWebSplHeightZaitan(atof(m_KakutenWebSplHeightZaitan));	//格点上横桁ウェブ添接板の高さ方向材端
	acc.setKakutenLflgSplKyojikuZaitan(atof(m_KakutenLflgSplKyojikuZaitan));	//格点上横桁下フランジ添接板の橋軸方向材端
	acc.setKakutenLflgSplKyotyokuZaitan(atof(m_KakutenLflgSplKyotyokuZaitan));	//格点上横桁下フランジ添接板の橋直方向材端
	acc.setKakutenConnKyojikuZaitan(atof(m_KakutenConnKyojikuZaitan));	//格点上コネクションの橋軸方向材端
	acc.setKakutenConnKyoutyokuZaitan(atof(m_KakutenConnKyoutyokuZaitan));	//格点上コネクションの橋直方向材端
	//
	string sekkeiFilePath = m_MdbFilePath.GetBuffer();
	string csvFileName = m_CsvFilePath.GetBuffer();
	string dataDir = "";
	if(sekkeiFilePath == "") {
		AfxMessageBox(_T("参照MDBファイル名を指定してください。"));
		return;
	} else {
		StringTokenizer strToken(sekkeiFilePath,"\\");
		if (strToken.size2() > 1) {
			int idx = strToken.size2()-2;
			for(int i=0;i<strToken.size2()-2;i++) {
				dataDir += strToken[i];
				dataDir += "\\";
			}
		}
	}
	SetCurrentDirectory(dataDir.c_str());
	string seizuFilePath = dataDir + "Draw\\DGFile.mdb";
	CString strMsg;
	if (!fileExists(sekkeiFilePath.c_str())) {
		strMsg.Format("参照MDBファイル(%s)は存在しません。", sekkeiFilePath.c_str());
		AfxMessageBox(strMsg);
		return;
	}
	if (!fileExists(seizuFilePath.c_str())) {
		strMsg.Format("設計ファイル(%s)が存在しません。", seizuFilePath.c_str());
		AfxMessageBox(strMsg);
		return;
	}
	if(csvFileName == "") {	//入力されていない
		AfxMessageBox(_T("出力ファイル名を指定してください。"));
		return;
	} else {
		if (fileExists(csvFileName.c_str())) {	//既に存在する場合は確認
			strMsg.Format("指定した出力ファイル(%s)が存在します。上書きして実行して良いですか？",csvFileName.c_str());
			if(MessageBox(strMsg,_T("ご注意"),MB_YESNO|MB_ICONEXCLAMATION) != IDYES) {
				return;
			}
			CFile OpenData;
			BOOL FileCheck=TRUE;
			FileCheck=OpenData.Open(csvFileName.c_str(),CFile::modeRead);
			if(FileCheck==TRUE) {
				OpenData.Close();
			} else {
				strMsg.Format("出力DXFファイル[%s]を閉じてから実行してください。", csvFileName.c_str());
				AfxMessageBox(strMsg);
				return;
			}
		}
	}

	//Set folder directory path
	TCHAR path[MAX_PATH];
	GetCurrentDirectory(MAX_PATH, path);
	string fileInputStorage = CString(path) + INPUTSTORAGE;
	storePreviousInput(agc, acc, fileInputStorage);

	//実行
	CString cstr="START";
	printMessage(cstr);
	if (ApolloToDesign_Main(sekkeiFilePath, seizuFilePath, csvFileName, agc, acc) != JPT_OK) {
		cstr="ERROR";
		printMessage(cstr);
		AfxMessageBox("× ｺﾏﾝﾄﾞ異常終了：APOLLOデータ⇒設計情報属性ファイル(ApolloToDesign)",MB_OK|MB_ICONSTOP);
		return;
	} else {
		cstr="END";
		printMessage(cstr);
		MessageBox("◎ ｺﾏﾝﾄﾞ実行終了：APOLLOデータ⇒設計情報属性ファイル(ApolloToDesign)",MB_OK);
		return;
	}

	CDialog::OnOK();
}

void CApolloToDesignDlg::OnCancel() 
{
	CDialog::OnCancel();
}

BOOL CApolloToDesignDlg::storePreviousInput(AtdGirderCommon agc, AtdCrossBeamCommon acc, string fileInputStorage)
{
	ofstream fStore;
	fStore.open(fileInputStorage.c_str());
	if (!fStore)
		return FALSE;
	fStore<<"MdbFilePath: "<<m_MdbFilePath<<"\n";
	fStore<<"CsvFilePath: "<<m_CsvFilePath<<"\n";
	fStore<<"UflgJc: "<<m_UflgJc<<"\n";
	fStore<<"WebJc: "<<m_WebJc<<"\n";
	fStore<<"LflgJc: "<<m_LflgJc<<"\n";
	fStore<<"UflgZaitanKeijo: "<<m_UflgZaitanKeijo<<"\n";
	fStore<<"LflgZaitanKeijo: "<<m_LflgZaitanKeijo<<"\n";
	fStore<<"UpdownFlgZaitanKeijoTachiageRyo: "<<m_UpdownFlgZaitanKeijoTachiageRyo<<"\n";
	fStore<<"SolePlateKyotyokuFreeSpace: "<<m_SolePlateKyotyokuFreeSpace<<"\n";
	fStore<<"SolePlateKyojikuFreeSpace: "<<m_SolePlateKyojikuFreeSpace<<"\n";
	fStore<<"ItatsugiZureRyo: "<<m_ItatsugiZureRyo<<"\n";
	fStore<<"LflgKakuhukubuTaper: "<<m_LflgKakuhukubuTaper<<"\n";
	fStore<<"WebHoleSlopeLowerLimitGrd: "<<m_WebHoleSlopeLowerLimitGrd<<"\n";
	fStore<<"ShitenVsCutWu: "<<m_ShitenVsCutWu<<"\n";
	fStore<<"ShitenVsCutWd: "<<m_ShitenVsCutWd<<"\n";
	fStore<<"ShitenVsCutFu: "<<m_ShitenVsCutFu<<"\n";
	fStore<<"KakutenVsCutWu: "<<m_KakutenVsCutWu<<"\n";
	fStore<<"KakutenVsCutWd: "<<m_KakutenVsCutWd<<"\n";
	fStore<<"KakutenVsCutFu: "<<m_KakutenVsCutFu<<"\n";
	fStore<<"MiddleVsCutWu: "<<m_MiddleVsCutWu<<"\n";
	fStore<<"MiddleVsCutWd: "<<m_MiddleVsCutWd<<"\n";
	fStore<<"MiddleVsFreeSpace: "<<m_MiddleVsFreeSpace<<"\n";
	fStore<<"HsFreeSpaceVs: "<<m_HsFreeSpaceVs<<"\n";
	fStore<<"HsFreeSpaceSpl: "<<m_HsFreeSpaceSpl<<"\n";
	fStore<<"HsFreeSpaceCbf: "<<m_HsFreeSpaceCbf<<"\n";
	fStore<<"HsFreeSpaceCbfUlimit: "<<m_HsFreeSpaceCbfUlimit<<"\n";
	fStore<<"HsSnipSizeVs: "<<m_HsSnipSizeVs<<"\n";
	fStore<<"HsSnipSizeSpl: "<<m_HsSnipSizeSpl<<"\n";
	fStore<<"HsSnipSizeCbf: "<<m_HsSnipSizeCbf<<"\n";
	fStore<<"UflgSplKyojikuZaitan: "<<m_UflgSplKyojikuZaitan<<"\n";
	fStore<<"UflgOutsideSplKyotyokuZaitan: "<<m_UflgOutsideSplKyotyokuZaitan<<"\n";
	fStore<<"UflgInsideSplKyotyokuZaitan: "<<m_UflgInsideSplKyotyokuZaitan<<"\n";
	fStore<<"WebSplKyojikuZaitan: "<<m_WebSplKyojikuZaitan<<"\n";
	fStore<<"WebSplHeightZaitan: "<<m_WebSplHeightZaitan<<"\n";
	fStore<<"LflgSplKyojikuZaitan: "<<m_LflgSplKyojikuZaitan<<"\n";
	fStore<<"LflgSplKyotyokuZaitan: "<<m_LflgSplKyotyokuZaitan<<"\n";
	fStore<<"ShitenUflgJc: "<<m_ShitenUflgJc<<"\n";
	fStore<<"ShitenWebJc: "<<m_ShitenWebJc<<"\n";
	fStore<<"ShitenLflgJc: "<<m_ShitenLflgJc<<"\n";
	fStore<<"KakutenHflgJc: "<<m_KakutenHflgJc<<"\n";
	fStore<<"KakutenHwebJc: "<<m_KakutenHwebJc<<"\n";
	fStore<<"ShitenUflgJc: "<<m_ShitenUflgJc<<"\n";
	fStore<<"ShitenWebJc: "<<m_ShitenWebJc<<"\n";
	fStore<<"ShitenLflgJc: "<<m_ShitenLflgJc<<"\n";
	fStore<<"KakutenHflgJc: "<<m_KakutenHflgJc<<"\n";
	fStore<<"KakutenHwebJc: "<<m_KakutenHwebJc<<"\n";
	fStore<<"ShitenConnCut: "<<m_ShitenConnCut<<"\n";
	fStore<<"ShitenConnFillet: "<<m_ShitenConnFillet<<"\n";
	fStore<<"ShitenConnTachiageryo: "<<m_ShitenConnTachiageryo<<"\n";
	fStore<<"KakutenConnCut: "<<m_KakutenConnCut<<"\n";
	fStore<<"KakutenConnFillet: "<<m_KakutenConnFillet<<"\n";
	fStore<<"KakutenConnTachiageryo: "<<m_KakutenConnTachiageryo<<"\n";
	fStore<<"CvsCutWu: "<<m_CvsCutWu<<"\n";
	fStore<<"CvsCutWd: "<<m_CvsCutWd<<"\n";
	fStore<<"WebHoleSlopeLowerLimitCrs: "<<m_WebHoleSlopeLowerLimitCrs<<"\n";
	fStore<<"FlgSectionType: "<<m_FlgSectionType<<"\n";
	fStore<<"ShitenUflgSplKyojikuZaitan: "<<m_ShitenUflgSplKyojikuZaitan<<"\n";
	fStore<<"ShitenUflgSplKyotyokuZaitan: "<<m_ShitenUflgSplKyotyokuZaitan<<"\n";
	fStore<<"ShitenWebSplKyotyokuZaitan: "<<m_ShitenWebSplKyotyokuZaitan<<"\n";
	fStore<<"ShitenWebSplHeightZaitan: "<<m_ShitenWebSplHeightZaitan<<"\n";
	fStore<<"ShitenLflgSplKyojikuZaitan: "<<m_ShitenLflgSplKyojikuZaitan<<"\n";
	fStore<<"ShitenLflgSplKyotyokuZaitan: "<<m_ShitenLflgSplKyotyokuZaitan<<"\n";
	fStore<<"ShitenConnKyojikuZaitan: "<<m_ShitenConnKyojikuZaitan<<"\n";
	fStore<<"ShitenConnKyoutyokuZaitan: "<<m_ShitenConnKyoutyokuZaitan<<"\n";
	fStore<<"KakutenUflgSplKyojikuZaitan: "<<m_KakutenUflgSplKyojikuZaitan<<"\n";
	fStore<<"KakutenUflgSplKyotyokuZaitan: "<<m_KakutenUflgSplKyotyokuZaitan<<"\n";
	fStore<<"KakutenWebSplKyotyokuZaitan: "<<m_KakutenWebSplKyotyokuZaitan<<"\n";
	fStore<<"KakutenWebSplHeightZaitan: "<<m_KakutenWebSplHeightZaitan<<"\n";
	fStore<<"KakutenLflgSplKyojikuZaitan: "<<m_KakutenLflgSplKyojikuZaitan<<"\n";
	fStore<<"KakutenLflgSplKyotyokuZaitan: "<<m_KakutenLflgSplKyotyokuZaitan<<"\n";
	fStore<<"KakutenConnKyojikuZaitan: "<<m_KakutenConnKyojikuZaitan<<"\n";
	fStore<<"KakutenConnKyoutyokuZaitan: "<<m_KakutenConnKyoutyokuZaitan<<"\n";
	fStore.close();

	return TRUE;
}

BOOL CApolloToDesignDlg::readPreviousInput(const int type, string fileInputStorage)
{	
	ifstream fStore;
	string line;
	fStore.open(fileInputStorage.c_str());
	if (!fStore)
		return FALSE;

	while ( getline(fStore,line) )
	{
		StringTokenizer strToken(line," ");
		if (strToken.size2() > 1) {
			if(type == 0) {
				if(strToken[0].compare("MdbFilePath:") == 0) 
					m_MdbFilePath = strToken[1].c_str();
			}
			if(strToken[0].compare("CsvFilePath:") == 0) 
				m_CsvFilePath = strToken[1].c_str();
			if(strToken[0].compare("UflgJc:") == 0) 
				m_UflgJc = strToken[1].c_str();
			if(strToken[0].compare("WebJc:") == 0) 
				m_WebJc = strToken[1].c_str();
			if(strToken[0].compare("LflgJc:") == 0) 
				m_LflgJc = strToken[1].c_str();
			if(strToken[0].compare("UflgZaitanKeijo:") == 0) 
				m_UflgZaitanKeijo = atoi(strToken[1].c_str());
			if(strToken[0].compare("LflgZaitanKeijo:") == 0) 
				m_LflgZaitanKeijo = atoi(strToken[1].c_str());
			if(strToken[0].compare("UpdownFlgZaitanKeijoTachiageRyo:") == 0) 
				m_UpdownFlgZaitanKeijoTachiageRyo = strToken[1].c_str();
			if(strToken[0].compare("SolePlateKyotyokuFreeSpace:") == 0) 
				m_SolePlateKyotyokuFreeSpace = strToken[1].c_str();
			if(strToken[0].compare("SolePlateKyojikuFreeSpace:") == 0) 
				m_SolePlateKyojikuFreeSpace = strToken[1].c_str();
			if(strToken[0].compare("ItatsugiZureRyo:") == 0) 
				m_ItatsugiZureRyo = strToken[1].c_str();
			if(strToken[0].compare("LflgKakuhukubuTaper:") == 0) 
				m_LflgKakuhukubuTaper = strToken[1].c_str();
			if(strToken[0].compare("WebHoleSlopeLowerLimitGrd:") == 0) 
				m_WebHoleSlopeLowerLimitGrd = strToken[1].c_str();
			if(strToken[0].compare("ShitenVsCutWu:") == 0) 
				m_ShitenVsCutWu = strToken[1].c_str();
			if(strToken[0].compare("ShitenVsCutWd:") == 0) 
				m_ShitenVsCutWd = strToken[1].c_str();
			if(strToken[0].compare("ShitenVsCutFu:") == 0) 
				m_ShitenVsCutFu = strToken[1].c_str();
			if(strToken[0].compare("KakutenVsCutWu:") == 0) 
				m_KakutenVsCutWu = strToken[1].c_str();
			if(strToken[0].compare("KakutenVsCutWd:") == 0) 
				m_KakutenVsCutWd = strToken[1].c_str();
			if(strToken[0].compare("KakutenVsCutFu:") == 0) 
				m_KakutenVsCutFu = strToken[1].c_str();
			if(strToken[0].compare("MiddleVsCutWu:") == 0) 
				m_MiddleVsCutWu = strToken[1].c_str();
			if(strToken[0].compare("MiddleVsCutWd:") == 0) 
				m_MiddleVsCutWd = strToken[1].c_str();
			if(strToken[0].compare("MiddleVsFreeSpace:") == 0) 
				m_MiddleVsFreeSpace = strToken[1].c_str();
			if(strToken[0].compare("HsFreeSpaceVs:") == 0) 
				m_HsFreeSpaceVs = strToken[1].c_str();
			if(strToken[0].compare("HsFreeSpaceSpl:") == 0) 
				m_HsFreeSpaceSpl = strToken[1].c_str();
			if(strToken[0].compare("HsFreeSpaceCbf:") == 0) 
				m_HsFreeSpaceCbf = strToken[1].c_str();
			if(strToken[0].compare("HsFreeSpaceCbfUlimit:") == 0) 
				m_HsFreeSpaceCbfUlimit = strToken[1].c_str();
			if(strToken[0].compare("HsSnipSizeVs:") == 0) 
				m_HsSnipSizeVs = strToken[1].c_str();
			if(strToken[0].compare("HsSnipSizeSpl:") == 0) 
				m_HsSnipSizeSpl = strToken[1].c_str();
			if(strToken[0].compare("HsSnipSizeCbf:") == 0) 
				m_HsSnipSizeCbf = strToken[1].c_str();
			if(strToken[0].compare("UflgSplKyojikuZaitan:") == 0) 
				m_UflgSplKyojikuZaitan = strToken[1].c_str();
			if(strToken[0].compare("UflgOutsideSplKyotyokuZaitan:") == 0) 
				m_UflgOutsideSplKyotyokuZaitan = strToken[1].c_str();
			if(strToken[0].compare("UflgInsideSplKyotyokuZaitan:") == 0) 
				m_UflgInsideSplKyotyokuZaitan = strToken[1].c_str();
			if(strToken[0].compare("WebSplKyojikuZaitan:") == 0) 
				m_WebSplKyojikuZaitan = strToken[1].c_str();
			if(strToken[0].compare("WebSplHeightZaitan:") == 0) 
				m_WebSplHeightZaitan = strToken[1].c_str();
			if(strToken[0].compare("LflgSplKyojikuZaitan:") == 0) 
				m_LflgSplKyojikuZaitan = strToken[1].c_str();
			if(strToken[0].compare("LflgSplKyotyokuZaitan:") == 0) 
				m_LflgSplKyotyokuZaitan = strToken[1].c_str();
			if(strToken[0].compare("ShitenUflgJc:") == 0) 
				m_ShitenUflgJc = strToken[1].c_str();
			if(strToken[0].compare("ShitenWebJc:") == 0) 
				m_ShitenWebJc = strToken[1].c_str();
			if(strToken[0].compare("ShitenLflgJc:") == 0) 
				m_ShitenLflgJc = strToken[1].c_str();
			if(strToken[0].compare("KakutenHflgJc:") == 0) 
				m_KakutenHflgJc = strToken[1].c_str();
			if(strToken[0].compare("KakutenHwebJc:") == 0) 
				m_KakutenHwebJc = strToken[1].c_str();
			if(strToken[0].compare("ShitenUflgJc:") == 0) 
				m_ShitenUflgJc = strToken[1].c_str();
			if(strToken[0].compare("ShitenWebJc:") == 0) 
				m_ShitenWebJc = strToken[1].c_str();
			if(strToken[0].compare("ShitenLflgJc:") == 0) 
				m_ShitenLflgJc = strToken[1].c_str();
			if(strToken[0].compare("KakutenHflgJc:") == 0) 
				m_KakutenHflgJc = strToken[1].c_str();
			if(strToken[0].compare("KakutenHwebJc:") == 0) 
				m_KakutenHwebJc = strToken[1].c_str();
			if(strToken[0].compare("ShitenConnCut:") == 0) 
				m_ShitenConnCut = strToken[1].c_str();
			if(strToken[0].compare("ShitenConnFillet:") == 0) 
				m_ShitenConnFillet = strToken[1].c_str();
			if(strToken[0].compare("ShitenConnTachiageryo:") == 0) 
				m_ShitenConnTachiageryo = strToken[1].c_str();
			if(strToken[0].compare("KakutenConnCut:") == 0) 
				m_KakutenConnCut = strToken[1].c_str();
			if(strToken[0].compare("KakutenConnFillet:") == 0) 
				m_KakutenConnFillet = strToken[1].c_str();
			if(strToken[0].compare("KakutenConnTachiageryo:") == 0) 
				m_KakutenConnTachiageryo = strToken[1].c_str();
			if(strToken[0].compare("CvsCutWu:") == 0) 
				m_CvsCutWu = strToken[1].c_str();
			if(strToken[0].compare("CvsCutWd:") == 0) 
				m_CvsCutWd = strToken[1].c_str();
			if(strToken[0].compare("WebHoleSlopeLowerLimitCrs:") == 0) 
				m_WebHoleSlopeLowerLimitCrs = strToken[1].c_str();
			if(strToken[0].compare("FlgSectionType:") == 0) 
				m_FlgSectionType = atoi(strToken[1].c_str());
			if(strToken[0].compare("ShitenUflgSplKyojikuZaitan:") == 0) 
				m_ShitenUflgSplKyojikuZaitan = strToken[1].c_str();
			if(strToken[0].compare("ShitenUflgSplKyotyokuZaitan:") == 0) 
				m_ShitenUflgSplKyotyokuZaitan = strToken[1].c_str();
			if(strToken[0].compare("ShitenWebSplKyotyokuZaitan:") == 0) 
				m_ShitenWebSplKyotyokuZaitan = strToken[1].c_str();
			if(strToken[0].compare("ShitenWebSplHeightZaitan:") == 0) 
				m_ShitenWebSplHeightZaitan = strToken[1].c_str();
			if(strToken[0].compare("ShitenLflgSplKyojikuZaitan:") == 0) 
				m_ShitenLflgSplKyojikuZaitan = strToken[1].c_str();
			if(strToken[0].compare("ShitenLflgSplKyotyokuZaitan:") == 0) 
				m_ShitenLflgSplKyotyokuZaitan = strToken[1].c_str();
			if(strToken[0].compare("ShitenConnKyojikuZaitan:") == 0) 
				m_ShitenConnKyojikuZaitan = strToken[1].c_str();
			if(strToken[0].compare("ShitenConnKyoutyokuZaitan:") == 0) 
				m_ShitenConnKyoutyokuZaitan = strToken[1].c_str();
			if(strToken[0].compare("KakutenUflgSplKyojikuZaitan:") == 0) 
				m_KakutenUflgSplKyojikuZaitan = strToken[1].c_str();
			if(strToken[0].compare("KakutenUflgSplKyotyokuZaitan:") == 0) 
				m_KakutenUflgSplKyotyokuZaitan = strToken[1].c_str();
			if(strToken[0].compare("KakutenWebSplKyotyokuZaitan:") == 0) 
				m_KakutenWebSplKyotyokuZaitan = strToken[1].c_str();
			if(strToken[0].compare("KakutenWebSplHeightZaitan:") == 0) 
				m_KakutenWebSplHeightZaitan = strToken[1].c_str();
			if(strToken[0].compare("KakutenLflgSplKyojikuZaitan:") == 0) 
				m_KakutenLflgSplKyojikuZaitan = strToken[1].c_str();
			if(strToken[0].compare("KakutenLflgSplKyotyokuZaitan:") == 0) 
				m_KakutenLflgSplKyotyokuZaitan = strToken[1].c_str();
			if(strToken[0].compare("KakutenConnKyojikuZaitan:") == 0) 
				m_KakutenConnKyojikuZaitan = strToken[1].c_str();
			if(strToken[0].compare("KakutenConnKyoutyokuZaitan:") == 0) 
				m_KakutenConnKyoutyokuZaitan = strToken[1].c_str();
		}
	}

	return TRUE;
}

void CApolloToDesignDlg::initializeGui(const int type)
{
	// TODO: Add extra initialization here
	WIN32_FIND_DATA fileNameData;
	HANDLE hFile;
	
	//Set folder directory path
	TCHAR path[MAX_PATH];
	GetCurrentDirectory(MAX_PATH, path);
	string fileStorage = CString(path) + INPUTSTORAGE;
	hFile = FindFirstFile(fileStorage.c_str(), &fileNameData);
	if (hFile == INVALID_HANDLE_VALUE)
	{
		m_CsvFilePath = "DesignInfoAttr.CSV";
		//ジョイントクリアランス
		m_UflgJc = "0.0";			//上フランジのジョイントクリアランス
		m_WebJc = "0.0";			//ウェブのジョイントクリアランス
		m_LflgJc = "0.0";			//下フランジのジョイントクリアランス
		//材端形状
		m_UflgZaitanKeijo = 0;		//上フランジ材端形状
		m_LflgZaitanKeijo = 0;		//下フランジ材端形状
		m_UpdownFlgZaitanKeijoTachiageRyo = _T("0.0");	//上下フランジ材端形状立上げ量
		//ソールプレート
		m_SolePlateKyotyokuFreeSpace = _T("15.0");		//ソールプレートの橋直方向空き量
		m_SolePlateKyojikuFreeSpace = _T("20.0");		//ソールプレートの橋軸方向空き量
		//その他
		m_ItatsugiZureRyo = _T("100.0");				//板継ズレ量
		m_LflgKakuhukubuTaper = _T("5");				//下フランジ拡幅部のテーパー勾配
		m_WebHoleSlopeLowerLimitGrd = _T("3.0");		//ウェブ孔の孔勾配の下限値
		//垂直補剛材
		m_ShitenVsCutWu = _T("15C");					//支点上垂直補剛材の溶接辺側上側切欠
		m_ShitenVsCutWd = _T("15C");					//支点上垂直補剛材の溶接辺側下側切欠
		m_ShitenVsCutFu = _T("10-45");						//支点上垂直補剛材の上側切欠
		m_KakutenVsCutWu = _T("35R");					//格点上垂直補剛材の溶接辺側上側切欠
		m_KakutenVsCutWd = _T("35R");					//格点上垂直補剛材の溶接辺側下側切欠
		m_KakutenVsCutFu = _T("10-45");					//格点上垂直補剛材の上側切欠
		m_MiddleVsCutWu = _T("35R");					//中間垂直補剛材の溶接辺側上側切欠
		m_MiddleVsCutWd = _T("35R");					//中間垂直補剛材（圧縮タイプ）の溶接辺側下側切欠
		m_MiddleVsFreeSpace = _T("35.0");				//中間垂直補剛材（引張タイプ）の下側空き量
		//水平補剛材
		m_HsFreeSpaceVs = _T("35.0");					//水平補剛材の垂直補剛材部、横桁ウェブ部空き量
		m_HsFreeSpaceSpl = _T("20.0");					//水平補剛材の添接板部空き量
		m_HsFreeSpaceCbf = _T("20.0");					//水平補剛材の横桁フランジ部空き量
		m_HsFreeSpaceCbfUlimit = _T("100.0");			//水平補剛材の横桁フランジからの高さ寸法上限
		m_HsSnipSizeVs = _T("10-45");					//水平補剛材の垂直補剛材部のスニップサイズ
		m_HsSnipSizeSpl = _T("10-45");					//水平補剛材の添接部のスニップサイズ
		m_HsSnipSizeCbf = _T("10-45");					//水平補剛材の横桁フランジ部のスニップサイズ
		//添接板
		m_UflgSplKyojikuZaitan = _T("40.0");			//上フランジ添接板の橋軸方向材端
		m_UflgOutsideSplKyotyokuZaitan = _T("40.0");	//上フランジ外側添接板の橋直方向材端
		m_UflgInsideSplKyotyokuZaitan = _T("40.0");		//上フランジ内側添接板の橋直方向材端
		m_WebSplKyojikuZaitan = _T("40.0");				//ウェブ添接板の橋軸方向材端
		m_WebSplHeightZaitan = _T("40.0");				//ウェブ添接板の高さ方向材端
		m_LflgSplKyojikuZaitan = _T("40.0");			//下フランジ添接板の橋軸方向材端
		m_LflgSplKyotyokuZaitan = _T("40.0");			//下フランジ添接板の橋直方向材端
		m_ShitenUflgJc = "10.0";		//支点上横桁（BH）上フランジのジョイントクリアランス
		m_ShitenWebJc = "10.0";			//支点上横桁（BH）ウェブのジョイントクリアランス
		m_ShitenLflgJc = "10.0";		//支点上横桁（BH）下フランジのジョイントクリアランス
		m_KakutenHflgJc = "10.0";		//格点上横桁（H鋼）フランジのジョイントクリアランス
		m_KakutenHwebJc = "10.0";		//格点上横桁（H鋼）ウェブのジョイントクリアランス
		//コネクションプレート
		m_ShitenConnCut = _T("15C");			//主桁ウェブ付きコネクション（支点上）の溶接辺側切欠
		m_ShitenConnFillet = _T("100.0");		//主桁ウェブ付きコネクション（支点上）のフィレットのRサイズ
		m_ShitenConnTachiageryo = _T("20.0");	//主桁ウェブ付きコネクション（支点上）の溶接辺立上げ量
		m_KakutenConnCut = _T("15C");			//主桁ウェブ付きコネクション（格点上）の溶接辺側切欠を指定
		m_KakutenConnFillet = _T("100.0");		//主桁ウェブ付きコネクション（格点上）のフィレットのRサイズ
		m_KakutenConnTachiageryo = _T("20.0");	//主桁ウェブ付きコネクション（格点上）の溶接辺立上げ量
		//垂直補剛材
		m_CvsCutWu = _T("35R");					//横桁付垂直補剛材の溶接辺側上側切欠
		m_CvsCutWd = _T("35R");					//横桁付垂直補剛材の溶接辺側下側切欠
		//その他
		m_WebHoleSlopeLowerLimitCrs = _T("3.0");	//ウェブ孔の孔勾配の下限値
		m_FlgSectionType = 0;						//フランジ切口の方向
		//材端形状
		m_ShitenUflgSplKyojikuZaitan = _T("40.0");		//支点上横桁上フランジ添接板の橋軸方向材端
		m_ShitenUflgSplKyotyokuZaitan = _T("40.0");		//支点上横桁上フランジ添接板の橋直方向材端
		m_ShitenWebSplKyotyokuZaitan = _T("40.0");		//支点上横桁ウェブ添接板の橋直方向材端
		m_ShitenWebSplHeightZaitan = _T("40.0");		//支点上横桁ウェブ添接板の高さ方向材端
		m_ShitenLflgSplKyojikuZaitan = _T("40.0");		//支点上横桁下フランジ添接板の橋軸方向材端
		m_ShitenLflgSplKyotyokuZaitan = _T("40.0");		//支点上横桁下フランジ添接板の橋直方向材端
		m_ShitenConnKyojikuZaitan = _T("40.0");			//支点上コネクションの橋軸方向材端
		m_ShitenConnKyoutyokuZaitan = _T("40.0");		//支点上コネクションの橋直方向材端
		m_KakutenUflgSplKyojikuZaitan = _T("40.0");		//格点上横桁上フランジ添接板の橋軸方向材端
		m_KakutenUflgSplKyotyokuZaitan = _T("40.0");	//格点上横桁上フランジ添接板の橋直方向材端
		m_KakutenWebSplKyotyokuZaitan = _T("40.0");		//格点上横桁ウェブ添接板の橋直方向材端
		m_KakutenWebSplHeightZaitan = _T("40.0");		//格点上横桁ウェブ添接板の高さ方向材端
		m_KakutenLflgSplKyojikuZaitan = _T("40.0");		//格点上横桁下フランジ添接板の橋軸方向材端
		m_KakutenLflgSplKyotyokuZaitan = _T("40.0");	//格点上横桁下フランジ添接板の橋直方向材端
		m_KakutenConnKyojikuZaitan = _T("40.0");		//格点上コネクションの橋軸方向材端
		m_KakutenConnKyoutyokuZaitan = _T("40.0");		//格点上コネクションの橋直方向材端
	} else {
		readPreviousInput(type, fileStorage);
	}
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD3)->SetWindowText(m_UflgJc);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD4)->SetWindowText(m_WebJc);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD5)->SetWindowText(m_LflgJc);
	((CComboBox*)m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD7))->SetCurSel(m_UflgZaitanKeijo);
	((CComboBox*)m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD8))->SetCurSel(m_LflgZaitanKeijo);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD9)->SetWindowText(m_UpdownFlgZaitanKeijoTachiageRyo);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD11)->SetWindowText(m_SolePlateKyotyokuFreeSpace);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD12)->SetWindowText(m_SolePlateKyojikuFreeSpace);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD6)->SetWindowText(m_ItatsugiZureRyo);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD10)->SetWindowText(m_LflgKakuhukubuTaper);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD36)->SetWindowText(m_WebHoleSlopeLowerLimitGrd);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD20)->SetWindowText(m_ShitenVsCutWu);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD21)->SetWindowText(m_ShitenVsCutWd);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD22)->SetWindowText(m_ShitenVsCutFu);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD23)->SetWindowText(m_KakutenVsCutWu);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD24)->SetWindowText(m_KakutenVsCutWd);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD25)->SetWindowText(m_KakutenVsCutFu);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD26)->SetWindowText(m_MiddleVsCutWu);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD27)->SetWindowText(m_MiddleVsCutWd);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD28)->SetWindowText(m_MiddleVsFreeSpace);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD13)->SetWindowText(m_HsFreeSpaceVs);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD14)->SetWindowText(m_HsFreeSpaceSpl);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD15)->SetWindowText(m_HsFreeSpaceCbf);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD16)->SetWindowText(m_HsFreeSpaceCbfUlimit);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD17)->SetWindowText(m_HsSnipSizeVs);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD18)->SetWindowText(m_HsSnipSizeSpl);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD19)->SetWindowText(m_HsSnipSizeCbf);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD29)->SetWindowText(m_UflgSplKyojikuZaitan);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD30)->SetWindowText(m_UflgOutsideSplKyotyokuZaitan);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD31)->SetWindowText(m_UflgInsideSplKyotyokuZaitan);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD32)->SetWindowText(m_WebSplKyojikuZaitan);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD33)->SetWindowText(m_WebSplHeightZaitan);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD34)->SetWindowText(m_LflgSplKyojikuZaitan);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD35)->SetWindowText(m_LflgSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS1)->SetWindowText(m_ShitenUflgJc);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS2)->SetWindowText(m_ShitenWebJc);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS3)->SetWindowText(m_ShitenLflgJc);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS4)->SetWindowText(m_KakutenHflgJc);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS5)->SetWindowText(m_KakutenHwebJc);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS6)->SetWindowText(m_ShitenConnCut);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS7)->SetWindowText(m_ShitenConnFillet);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS8)->SetWindowText(m_ShitenConnTachiageryo);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS9)->SetWindowText(m_KakutenConnCut);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS10)->SetWindowText(m_KakutenConnFillet);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS11)->SetWindowText(m_KakutenConnTachiageryo);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS12)->SetWindowText(m_CvsCutWu);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS13)->SetWindowText(m_CvsCutWd);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS30)->SetWindowText(m_WebHoleSlopeLowerLimitCrs);
	((CComboBox*)m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS31))->SetCurSel(m_FlgSectionType);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS14)->SetWindowText(m_ShitenUflgSplKyojikuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS15)->SetWindowText(m_ShitenUflgSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS16)->SetWindowText(m_ShitenWebSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS17)->SetWindowText(m_ShitenWebSplHeightZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS18)->SetWindowText(m_ShitenLflgSplKyojikuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS19)->SetWindowText(m_ShitenLflgSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS20)->SetWindowText(m_ShitenConnKyojikuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS21)->SetWindowText(m_ShitenConnKyoutyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS22)->SetWindowText(m_KakutenUflgSplKyojikuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS23)->SetWindowText(m_KakutenUflgSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS24)->SetWindowText(m_KakutenWebSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS25)->SetWindowText(m_KakutenWebSplHeightZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS26)->SetWindowText(m_KakutenLflgSplKyojikuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS27)->SetWindowText(m_KakutenLflgSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS28)->SetWindowText(m_KakutenConnKyojikuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS29)->SetWindowText(m_KakutenConnKyoutyokuZaitan);

	UpdateData(FALSE);
}

bool CApolloToDesignDlg::fileExists( LPCTSTR filename )
{
	bool result;
	WIN32_FIND_DATA wfd;
	HANDLE hFile = FindFirstFile( filename, &wfd );

	if( hFile == INVALID_HANDLE_VALUE ){
		result = false;
	}else{
		result = true;
	}

	FindClose( hFile );
	return result;
}

void CApolloToDesignDlg::printMessage(
	CString& result		//START,END,ERROR 
)
{
	time_t     ltime;
	struct tm  *ltm;

	time(&ltime);
	ltm = localtime(&ltime);
	char strdate[64],strtime[64];
	sprintf(strdate,"%u/%02u/%02u",ltm->tm_year+1900,++ltm->tm_mon,ltm->tm_mday);
	sprintf(strtime," %02u:%02u:%02u",ltm->tm_hour,ltm->tm_min,ltm->tm_sec);
	string sd = strdate;
	string st = strtime;
	
	stringstream str;
	str << "<" << sd << st << ">" << endl;

	string message;
	static char buf[128] = {""};

	if ( "START" == result ) {
		sprintf(buf, "ApolloToDesign.exe Build No.<%s %s>\n",__DATE__,__TIME__ );
		log_MsgPrintf(buf);
		message = "◎ ｺﾏﾝﾄﾞ実行開始：APOLLOデータ⇒設計情報属性ファイル(ApolloToDesign) " + str.str() + "\n";
	} else if ( "END" == result ) {
		message = "◎ ｺﾏﾝﾄﾞ正常終了：APOLLOデータ⇒設計情報属性ファイル(ApolloToDesign) " + str.str() + "\n";
	} else {
		message = "× ｺﾏﾝﾄﾞ異常終了：APOLLOデータ⇒設計情報属性ファイル(ApolloToDesign) " + str.str() + "\n";
	}
	log_MsgPrintf(message.c_str());
}

