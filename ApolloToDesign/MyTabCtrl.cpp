// MyTabCtrl.cpp : implementation file
//

#include "stdafx.h"
#include "MyTabCtrl.h"
#include "resource.h"
#include "DlgCrossBeam.h"
#include "DlgGirder.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CMyTabCtrl

CMyTabCtrl::CMyTabCtrl()
{
	m_DialogID.push_back(DLG_Girder);
	m_DialogID.push_back(DLG_CrossBeam);

	m_Dialog.push_back(new CDlgGirder());
	m_Dialog.push_back(new CDlgCrossBeam());

	m_nPageCount = m_Dialog.size();
}

CMyTabCtrl::~CMyTabCtrl()
{
	std::vector<CDialog*>::iterator it = m_Dialog.begin();

	for (; it != m_Dialog.end(); ++it)
	{
		delete (*it);
	}

	m_Dialog.clear();
}

void CMyTabCtrl::InitDialogs()
{
	m_Dialog[0]->Create(m_DialogID[0],GetParent());
	m_Dialog[1]->Create(m_DialogID[1],GetParent());
}

BEGIN_MESSAGE_MAP(CMyTabCtrl, CTabCtrl)
	//{{AFX_MSG_MAP(CMyTabCtrl)
	ON_NOTIFY_REFLECT(TCN_SELCHANGE, OnSelchange)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CMyTabCtrl message handlers

void CMyTabCtrl::OnSelchange(NMHDR* pNMHDR, LRESULT* pResult) 
{
	// TODO: Add your control notification handler code here
	ActivateTabDialogs();
	*pResult = 0;
}

void CMyTabCtrl::ActivateTabDialogs()
{
	int nSel = GetCurSel();
	if(m_Dialog[nSel]->m_hWnd)
		m_Dialog[nSel]->ShowWindow(SW_HIDE);
	
	CRect l_rectClient;
	CRect l_rectWnd;
	
	GetClientRect(l_rectClient);
	AdjustRect(FALSE,l_rectClient);
	GetWindowRect(l_rectWnd);
	GetParent()->ScreenToClient(l_rectWnd);
	l_rectClient.OffsetRect(l_rectWnd.left,l_rectWnd.top);
	for(int nCount=0; nCount < m_nPageCount; nCount++){
		m_Dialog[nCount]->SetWindowPos(&wndTop, l_rectClient.left,l_rectClient.top,l_rectClient.Width(),l_rectClient.Height(),SWP_HIDEWINDOW);
	}
	m_Dialog[nSel]->SetWindowPos(&wndTop,l_rectClient.left,l_rectClient.top,l_rectClient.Width(),l_rectClient.Height(),SWP_SHOWWINDOW);
	
	m_Dialog[nSel]->ShowWindow(SW_SHOW);
}

void CMyTabCtrl::InitTabDialog()
{
	int nSel = 0;
	if(m_Dialog[nSel]->m_hWnd)
		m_Dialog[nSel]->ShowWindow(SW_HIDE);
	
	CRect l_rectClient;
	CRect l_rectWnd;
	
	GetClientRect(l_rectClient);
	AdjustRect(FALSE,l_rectClient);
	GetWindowRect(l_rectWnd);
	GetParent()->ScreenToClient(l_rectWnd);
	l_rectClient.OffsetRect(l_rectWnd.left,l_rectWnd.top);

	for(int nCount=0; nCount < m_nPageCount; nCount++){
		m_Dialog[nCount]->SetWindowPos(&wndTop, l_rectClient.left,l_rectClient.top,l_rectClient.Width(),l_rectClient.Height(),SWP_HIDEWINDOW);
	}
	m_Dialog[nSel]->SetWindowPos(&wndTop,l_rectClient.left,l_rectClient.top,l_rectClient.Width(),l_rectClient.Height(),SWP_SHOWWINDOW);
	
	m_Dialog[nSel]->ShowWindow(SW_SHOW);
}
