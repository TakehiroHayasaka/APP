/* SDS\ENTPOINT.C
 * Copyright (C) 1997-1998 Visio Corporation. All rights reserved.
 * 
 * Abstract
 * 
 * Sample SDS application entrypoint definition.
 * 
 *	$Revision: 1 $ $Date: 10/12/17 11:30 $
 * 
 */ 
#include "StdAfx.h"
extern "C"{
#include "sds.h"
}

#define SDS_CADAPI 1
#include <windows.h>
#define malloc sds_malloc
#define free sds_free
#define realloc sds_realloc
#define calloc sds_calloc

char      adsw_AppName[512];
char      *sds_argVec = adsw_AppName;
char      sds_appname[512];
HWND      adsw_hwndAcad;
HINSTANCE adsw_hInstance;

HWND      adsw_hWnd;
int       adsw_wait;

sds_matrix sds_identmat;

#if 1 // ADD By Guojin Li 2009/3/25 STA
extern HINSTANCE hTheApp;
#endif // ADD By Guojin Li 2009/3/25 END


// Protos
extern "C"{
	int SDS_GetGlobals(char *appname,HWND *hwnd,HINSTANCE *hInstance);
}
void __declspec(dllexport) SDS_EntryPoint(HWND hWnd);


void __declspec(dllexport) SDS_EntryPoint(HWND hWnd) {
#if 1 // ADD By Guojin Li 2009/3/25 STA
	AfxSetResourceHandle(hTheApp);
	AfxGetModuleState()->m_hCurrentInstanceHandle = hTheApp; 
	//AFX_MANAGE_STATE(AfxGetStaticModuleState());
#endif // ADD By Guojin Li 2009/3/25 END
	int i,j;
    for(i=0; i<=3; i++) for(j=0; j<=3; j++) sds_identmat[i][j]=0.0;
    for(i=0; i<=3; i++) sds_identmat[i][i]=1.0;

	SDS_GetGlobals(adsw_AppName,&adsw_hwndAcad,&adsw_hInstance);
    strncpy(sds_appname,adsw_AppName,sizeof(sds_appname)-1);
    SDS_main(1,&sds_argVec);
    return;
}

#if defined(SDS_OVERRIDEMEMORYFUNCS)
	#undef malloc
	#undef free
	#undef realloc
	#undef calloc

	void *malloc(size_t sizeBytes) {
		return(sds_malloc(sizeBytes));
	}

	void free(void *pMemLoc) {
		sds_free(pMemLoc);
	}

	void *realloc(void *pOldMemLoc, size_t sizeBytes) {
		return(sds_realloc(pOldMemLoc,sizeBytes));
	}

	void *calloc(size_t sizeHowMany, size_t sizeBytesEach) {
		return(sds_calloc(sizeHowMany,sizeBytesEach));
	}
#endif
