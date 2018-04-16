// stdafx.h : 標準のシステム インクルード ファイル、
//            または参照回数が多く、かつあまり変更されない
//            プロジェクト専用のインクルード ファイルを記述します。
//

#if !defined(AFX_STDAFX_H__6A54AD52_E88B_49E6_8553_5AB07560FBF2__INCLUDED_)
#define AFX_STDAFX_H__6A54AD52_E88B_49E6_8553_5AB07560FBF2__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

// 2016/03/11 ohde Edit Start
#ifdef VS2010
#define WINVER 0x0500
#define _WIN32_WINNT 0x0500
#else
#define STRICT
#ifndef _WIN32_WINNT
#define _WIN32_WINNT 0x0500  //--09.10.5  0x0400
#endif
#endif
// 2016/03/11 ohde Edit End


#pragma warning(disable: 4786)


#define VC_EXTRALEAN		// Windows ヘッダーから殆ど使用されないスタッフを除外します。

// 'DEBUG workaround' below prevents the MFC or ATL #include-s 
// from pulling in "afx.h" that would force the debug CRT through
// #pragma-s.
//#ifdef _DEBUG
//#define _DEBUG_WAS_DEFINED
//#undef _DEBUG
//#define NDEBUG
//#endif

#include <afxwin.h>         // MFC のコアおよび標準コンポーネント
#include <afxext.h>         // MFC の拡張部分

#ifndef _AFX_NO_OLE_SUPPORT
#include <afxole.h>         // MFC OLE classes
#include <afxodlgs.h>       // MFC OLE dialog classes
#include <afxdisp.h>        // MFC Automation classes
#endif // _AFX_NO_OLE_SUPPORT

//#ifndef _AFX_NO_DB_SUPPORT
//#include <afxdb.h>          // MFC ODBC database classes
//#endif // _AFX_NO_DB_SUPPORT

//#ifndef _AFX_NO_DAO_SUPPORT
//#include <afxdao.h>         // MFC DAO database classes
//#endif // _AFX_NO_DAO_SUPPORT

#include <afxdtctl.h>		// MFC の Internet Explorer 4 コモン コントロール サポート
#ifndef _AFX_NO_AFXCMN_SUPPORT
#include <afxcmn.h>			// MFC の Windows コモン コントロール サポート
#endif // _AFX_NO_AFXCMN_SUPPORT


//#ifdef _DEBUG_WAS_DEFINED
//#undef NDEBUG
//#define _DEBUG
//#undef _DEBUG_WAS_DEFINED
//#endif

#include <vector>
extern HINSTANCE hTheApp;
//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ は前行の直前に追加の宣言を挿入します。

#endif // !defined(AFX_STDAFX_H__6A54AD52_E88B_49E6_8553_5AB07560FBF2__INCLUDED_)
