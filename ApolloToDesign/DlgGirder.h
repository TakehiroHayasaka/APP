#pragma once

// CDlgGirder ダイアログ

class CDlgGirder : public CDialog
{
	DECLARE_DYNAMIC(CDlgGirder)

public:
	CDlgGirder(CWnd* pParent = NULL);   // 標準コンストラクタ
	virtual ~CDlgGirder();

// ダイアログ データ
	enum { IDD = DLG_Girder };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV サポート
	virtual BOOL OnInitDialog();

	DECLARE_MESSAGE_MAP()
};
