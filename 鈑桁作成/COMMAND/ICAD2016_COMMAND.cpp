
#include  <adslib.h>
#include  <rxregsvc.h>
#include "tchar.h"

#pragma warning(disable:4819)	//Warning C4819の抑制

//ヘッダ/ライブラリの定義
#include "arxHeaders.h"
#pragma comment(lib, "grxport.lib")
#pragma comment(lib, "TD_Root.lib")
#pragma comment(lib, "gced.lib")
#pragma comment(lib, "gcad.lib")
#pragma comment(lib, "gcap.lib")
#pragma comment(lib, "gcdb.lib")
#pragma comment(lib, "gcut.lib")
#pragma comment(lib, "TD_Db.lib")
#pragma comment(lib, "TD_DbRoot.lib")


#define ARRAYCOUNT(array) (sizeof(array)/sizeof((array)[0]))

static int invokefun (void);
static int funcload (void);

/////ここから
void drawGirder();

static int ICAD2016_GIRDER(struct ads_resbuf *rb = NULL)
{
	drawGirder();
	return 0;
}

struct functionDef{ wchar_t *function_name; int (*function) (struct ads_resbuf *); };
static struct functionDef funcTable[] =
{
    {L"C:COMMAND",	ICAD2016_GIRDER},
};
/////ここまで

extern "C" AcRx::AppRetCode
  acrxEntryPoint(AcRx::AppMsgCode msg, void* appId)
{
  switch(msg) {
  case AcRx::kInitAppMsg:
    acrxDynamicLinker->unlockApplication(appId);
    acrxDynamicLinker->registerAppMDIAware(appId);
    break;
  case AcRx::kInvkSubrMsg:
    invokefun();
    break;
  case AcRx::kLoadDwgMsg:
    funcload();
  }
  return AcRx::kRetOK;
}

static int funcload()
{
    int i;

    for (i = 0; i < ARRAYCOUNT(funcTable); i++) {
        if (!acedDefun(funcTable[i].function_name, i))
            return RTERROR;
    }

    return RTNORM;
}

static int invokefun()
{
    struct ads_resbuf *rb;
    int val;

    // Verify that function code is valid.
    if ((val = ads_getfuncode()) < 0 || val >= ARRAYCOUNT(funcTable))
	{
        ads_fail(L"Invalid function code.");
        return RTERROR;
    }

    // Get any arguments.
    rb = ads_getargs();

    // Call the function and return is return value
    val = (*funcTable[val].function)(rb);
    ads_relrb(rb);

    return val;
}
