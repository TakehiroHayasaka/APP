/************************************************************************/
/*                                                                      */
/************************************************************************/

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "resource.h"		// メイン シンボル
#include "AtdGirderCommon.h"
#include "AtdCrossBeamCommon.h"

int ApolloToDesign_Main(string& sekkeiFilePath, string& seizuFilePath, string& csvFileName, AtdGirderCommon& agc, AtdCrossBeamCommon& acc);

