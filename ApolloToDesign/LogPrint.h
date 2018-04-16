/********************************************************************
	created:	2008/12/17
	author:		p.chen
	
	purpose:	
*********************************************************************/
#include <string>

#define LOG_FILENAME "jupiter.log" //Û¸ŞÌ§²Ù–¼
#define LOG_MAXSTR   (int)512      //•¶š—ñÅ‘å’·

#if defined(__cplusplus) && !defined(VS2010)
extern	"C"{
#endif

	void log_DateTimeEx(wchar_t *strdate,wchar_t *strtime);
	int log_MsgPrintf(const char *format, ...);

#if defined(__cplusplus) && !defined(VS2010)
}
#endif
