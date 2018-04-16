#pragma once

// CDlgCrossBeam ダイアログ

class CDlgCrossBeam : public CDialog
{
	DECLARE_DYNAMIC(CDlgCrossBeam)

public:
	CDlgCrossBeam(CWnd* pParent = NULL);   // 標準コンストラクタ
	virtual ~CDlgCrossBeam();

// ダイアログ データ
	enum { IDD = DLG_CrossBeam };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV サポート
	virtual BOOL OnInitDialog();

	DECLARE_MESSAGE_MAP()
};

