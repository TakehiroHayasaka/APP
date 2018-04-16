// ApolloToDesign.h : APOLLOTODESIGN アプリケーションのメイン ヘッダー ファイルです。
//

#if !defined(AFX_APOLLOTODESIGN_H__37A4472C_3958_44D3_A357_61E5950F9629__INCLUDED_)
#define AFX_APOLLOTODESIGN_H__37A4472C_3958_44D3_A357_61E5950F9629__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// メイン シンボル
#include "locale.h"

/////////////////////////////////////////////////////////////////////////////
// CApolloToDesignApp:
// このクラスの動作の定義に関しては ApolloToDesign.cpp ファイルを参照してください。
//

class CApolloToDesignApp : public CWinApp
{
public:
	CApolloToDesignApp();

// オーバーライド
	// ClassWizard は仮想関数のオーバーライドを生成します。
	//{{AFX_VIRTUAL(CApolloToDesignApp)
	public:
	virtual BOOL InitInstance();
	virtual int ExitInstance();
	//}}AFX_VIRTUAL

// インプリメンテーション

	//{{AFX_MSG(CApolloToDesignApp)
		// メモ - ClassWizard はこの位置にメンバ関数を追加または削除します。
		//        この位置に生成されるコードを編集しないでください。
	//}}AFX_MSG

private:
	BOOL	m_bInitOD;
	DECLARE_MESSAGE_MAP()
};

// Output to log file
void log_MessageBox(CString strMsg);
void log_Msg(const std::string& strMsg);

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ は前行の直前に追加の宣言を挿入します。

#endif // !defined(AFX_APOLLOTODESIGN_H__37A4472C_3958_44D3_A357_61E5950F9629__INCLUDED_)
