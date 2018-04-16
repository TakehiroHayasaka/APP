#if !defined(AFX_MYTABCTRL_H__3F917FEC_B5C2_43B6_9091_A6FD9D88F5D2__INCLUDED_)
#define AFX_MYTABCTRL_H__3F917FEC_B5C2_43B6_9091_A6FD9D88F5D2__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// MyTabCtrl.h : header file
//

#include <vector>

/////////////////////////////////////////////////////////////////////////////
// CMyTabCtrl window

class CMyTabCtrl : public CTabCtrl
{
// Construction
public:
	CMyTabCtrl();

// Attributes
public:

// Operations

	int m_nPageCount;
	//Array to hold the list of dialog boxes/tab pages for CTabCtrl
	std::vector<int> m_DialogID;
	
	//CDialog Array Variable to hold the dialogs 
	std::vector<CDialog*> m_Dialog;
	
	//Function to Create the dialog boxes during startup
	void InitDialogs();
	
	//Function to activate the tab dialog boxes
	void ActivateTabDialogs();
	
	void InitTabDialog();

public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CMyTabCtrl)
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CMyTabCtrl();

	// Generated message map functions
protected:
	//{{AFX_MSG(CMyTabCtrl)
	afx_msg void OnSelchange(NMHDR* pNMHDR, LRESULT* pResult);
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_MYTABCTRL_H__3F917FEC_B5C2_43B6_9091_A6FD9D88F5D2__INCLUDED_)
