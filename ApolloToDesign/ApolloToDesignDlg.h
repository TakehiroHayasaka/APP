// ApolloToDesignDlg.h : ヘッダー ファイル
//

#include "MyTabCtrl.h"
#include "AtdGirderCommon.h"
#include "AtdCrossBeamCommon.h"
#include "resource.h"
#include <string>
#include <fstream>
using namespace std;

#if !defined(AFX_APOLLOTODESIGNDLG_H__5A0CA2F4_7761_40F0_8933_C33B3125ACAD__INCLUDED_)
#define AFX_APOLLOTODESIGNDLG_H__5A0CA2F4_7761_40F0_8933_C33B3125ACAD__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

/////////////////////////////////////////////////////////////////////////////
// CApolloToDesignDlg ダイアログ

class CApolloToDesignDlg : public CDialog
{
// 構築
public:
	CApolloToDesignDlg(CWnd* pParent = NULL);	// 標準のコンストラクタ
// ダイアログ データ
	//{{AFX_DATA(CApolloToDesignDlg)
	enum { IDD = IDD_APOLLOTODESIGN_DIALOG };
	CMyTabCtrl	m_MainTab;
	CString	m_MdbFilePath;
	CString	m_CsvFilePath;
	//主桁
	//ジョイントクリアランス
	CString	m_UflgJc;			//上フランジのジョイントクリアランス
	CString	m_WebJc;			//ウェブのジョイントクリアランス
	CString	m_LflgJc;			//下フランジのジョイントクリアランス
	//材端形状
	int		m_UflgZaitanKeijo;					//上フランジ材端形状
	int		m_LflgZaitanKeijo;					//下フランジ材端形状
	CString	m_UpdownFlgZaitanKeijoTachiageRyo;	//上下フランジ材端形状立上げ量
	//ソールプレート
	CString	m_SolePlateKyotyokuFreeSpace;		//ソールプレートの橋直方向空き量
	CString	m_SolePlateKyojikuFreeSpace;		//ソールプレートの橋軸方向空き量
	//その他
	CString	m_ItatsugiZureRyo;					//板継ズレ量
	CString	m_LflgKakuhukubuTaper;				//下フランジ拡幅部のテーパー勾配
	CString	m_WebHoleSlopeLowerLimitGrd;		//ウェブ孔の孔勾配の下限値
	//垂直補剛材
	CString	m_ShitenVsCutWu;					//支点上垂直補剛材の溶接辺側上側切欠
	CString	m_ShitenVsCutWd;					//支点上垂直補剛材の溶接辺側下側切欠
	CString	m_ShitenVsCutFu;					//支点上垂直補剛材の上側切欠
	CString	m_KakutenVsCutWu;					//格点上垂直補剛材の溶接辺側上側切欠
	CString	m_KakutenVsCutWd;					//格点上垂直補剛材の溶接辺側下側切欠
	CString	m_KakutenVsCutFu;					//格点上垂直補剛材の上側切欠
	CString	m_MiddleVsCutWu;					//中間垂直補剛材の溶接辺側上側切欠
	CString	m_MiddleVsCutWd;					//中間垂直補剛材（圧縮タイプ）の溶接辺側下側切欠
	CString	m_MiddleVsFreeSpace;				//中間垂直補剛材（引張タイプ）の下側空き量
	//水平補剛材
	CString	m_HsFreeSpaceVs;					//水平補剛材の垂直補剛材部、横桁ウェブ部空き量
	CString	m_HsFreeSpaceSpl;					//水平補剛材の添接板部空き量
	CString	m_HsFreeSpaceCbf;					//水平補剛材の横桁フランジ部空き量
	CString	m_HsFreeSpaceCbfUlimit;				//水平補剛材の横桁フランジからの高さ寸法上限
	CString	m_HsSnipSizeVs;						//水平補剛材の垂直補剛材部のスニップサイズ
	CString	m_HsSnipSizeSpl;					//水平補剛材の添接部のスニップサイズ
	CString	m_HsSnipSizeCbf;					//水平補剛材の横桁フランジ部のスニップサイズ
	//添接板
	CString	m_UflgSplKyojikuZaitan;				//上フランジ添接板の橋軸方向材端
	CString	m_UflgOutsideSplKyotyokuZaitan;		//上フランジ外側添接板の橋直方向材端
	CString	m_UflgInsideSplKyotyokuZaitan;		//上フランジ内側添接板の橋直方向材端
	CString	m_WebSplKyojikuZaitan;				//ウェブ添接板の橋軸方向材端
	CString	m_WebSplHeightZaitan;				//ウェブ添接板の高さ方向材端
	CString	m_LflgSplKyojikuZaitan;				//下フランジ添接板の橋軸方向材端
	CString	m_LflgSplKyotyokuZaitan;			//下フランジ添接板の橋直方向材端
	//横桁
	//ジョイントクリアランス
	CString	m_ShitenUflgJc;		//支点上横桁（BH）上フランジのジョイントクリアランス
	CString	m_ShitenWebJc;		//支点上横桁（BH）ウェブのジョイントクリアランス
	CString	m_ShitenLflgJc;		//支点上横桁（BH）下フランジのジョイントクリアランス
	CString	m_KakutenHflgJc;	//格点上横桁（H鋼）フランジのジョイントクリアランス
	CString	m_KakutenHwebJc;	//格点上横桁（H鋼）ウェブのジョイントクリアランス
	//コネクションプレート
	CString	m_ShitenConnCut;				//主桁ウェブ付きコネクション（支点上）の溶接辺側切欠
	CString	m_ShitenConnFillet;				//主桁ウェブ付きコネクション（支点上）のフィレットのRサイズ
	CString	m_ShitenConnTachiageryo;		//主桁ウェブ付きコネクション（支点上）の溶接辺立上げ量
	CString	m_KakutenConnCut;				//主桁ウェブ付きコネクション（格点上）の溶接辺側切欠を指定
	CString	m_KakutenConnFillet;			//主桁ウェブ付きコネクション（格点上）のフィレットのRサイズ
	CString	m_KakutenConnTachiageryo;		//主桁ウェブ付きコネクション（格点上）の溶接辺立上げ量
	//垂直補剛材
	CString	m_CvsCutWu;						//横桁付垂直補剛材の溶接辺側上側切欠
	CString	m_CvsCutWd;						//横桁付垂直補剛材の溶接辺側下側切欠
	//その他
	CString	m_WebHoleSlopeLowerLimitCrs;	//ウェブ孔の孔勾配の下限値
	int		m_FlgSectionType;				//フランジ切口の方向
	//材端形状
	CString	m_ShitenUflgSplKyojikuZaitan;		//支点上横桁上フランジ添接板の橋軸方向材端
	CString	m_ShitenUflgSplKyotyokuZaitan;		//支点上横桁上フランジ添接板の橋直方向材端
	CString	m_ShitenWebSplKyotyokuZaitan;		//支点上横桁ウェブ添接板の橋直方向材端
	CString	m_ShitenWebSplHeightZaitan;			//支点上横桁ウェブ添接板の高さ方向材端
	CString	m_ShitenLflgSplKyojikuZaitan;		//支点上横桁下フランジ添接板の橋軸方向材端
	CString	m_ShitenLflgSplKyotyokuZaitan;		//支点上横桁下フランジ添接板の橋直方向材端
	CString	m_ShitenConnKyojikuZaitan;			//支点上コネクションの橋軸方向材端
	CString	m_ShitenConnKyoutyokuZaitan;		//支点上コネクションの橋直方向材端
	CString	m_KakutenUflgSplKyojikuZaitan;		//格点上横桁上フランジ添接板の橋軸方向材端
	CString	m_KakutenUflgSplKyotyokuZaitan;		//格点上横桁上フランジ添接板の橋直方向材端
	CString	m_KakutenWebSplKyotyokuZaitan;		//格点上横桁ウェブ添接板の橋直方向材端
	CString	m_KakutenWebSplHeightZaitan;		//格点上横桁ウェブ添接板の高さ方向材端
	CString	m_KakutenLflgSplKyojikuZaitan;		//格点上横桁下フランジ添接板の橋軸方向材端
	CString	m_KakutenLflgSplKyotyokuZaitan;		//格点上横桁下フランジ添接板の橋直方向材端
	CString	m_KakutenConnKyojikuZaitan;			//格点上コネクションの橋軸方向材端
	CString	m_KakutenConnKyoutyokuZaitan;		//格点上コネクションの橋直方向材端
		// メモ: この位置に ClassWizard によってデータ メンバが追加されます。
	//}}AFX_DATA

	// ClassWizard は仮想関数のオーバーライドを生成します。
	//{{AFX_VIRTUAL(CApolloToDesignDlg)
protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV のサポート
	//}}AFX_VIRTUAL

// インプリメンテーション
protected:
	HICON m_hIcon;

	// 生成されたメッセージ マップ関数
	//{{AFX_MSG(CApolloToDesignDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg void OnMdbFileBrowse();
	virtual void OnOK();
	virtual void OnCancel();
	afx_msg BOOL storePreviousInput(AtdGirderCommon agc, AtdCrossBeamCommon acc, string fileInputStorage);
	afx_msg BOOL readPreviousInput(const int type, string fileInputStorage);
	afx_msg void initializeGui(const int type);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

private:
	bool fileExists(LPCTSTR filename);
	void printMessage(CString& result);

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ は前行の直前に追加の宣言を挿入します。

#endif // !defined(AFX_APOLLOTODESIGNDLG_H__5A0CA2F4_7761_40F0_8933_C33B3125ACAD__INCLUDED_)
