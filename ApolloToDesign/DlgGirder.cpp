// DlgGirder.cpp : 実装ファイル
//

#include "stdafx.h"
#include "ApolloToDesign_main.h"
#include "DlgGirder.h"

// CDlgGirder ダイアログ

IMPLEMENT_DYNAMIC(CDlgGirder, CDialog)

CDlgGirder::CDlgGirder(CWnd* pParent /*=NULL*/)
	: CDialog(CDlgGirder::IDD, pParent)
{
}

CDlgGirder::~CDlgGirder()
{
}

void CDlgGirder::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CDlgGirder, CDialog)
END_MESSAGE_MAP()

BOOL CDlgGirder::OnInitDialog() 
{
	CDialog::OnInitDialog();
	((CComboBox*)GetDlgItem(IDC_GRD7))->AddString(_T("タイプ１"));
	((CComboBox*)GetDlgItem(IDC_GRD7))->AddString(_T("タイプ２"));
	((CComboBox*)GetDlgItem(IDC_GRD7))->AddString(_T("タイプ３"));
	((CComboBox*)GetDlgItem(IDC_GRD8))->AddString(_T("タイプ１"));
	((CComboBox*)GetDlgItem(IDC_GRD8))->AddString(_T("タイプ２"));
	((CComboBox*)GetDlgItem(IDC_GRD8))->AddString(_T("タイプ３"));

	return TRUE;
}

