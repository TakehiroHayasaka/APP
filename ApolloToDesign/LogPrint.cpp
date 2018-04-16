/********************************************************************
	created:	2008/12/17
	author:		p.chen
	
	purpose:	
*********************************************************************/

#include "StdAfx.h"
#include <SHARE.h>
#include "LogPrint.h"
#ifndef _EXE
#ifndef IJCAD2015
#include "sds.h"
#else
#include "adslib.h"
#endif
#else
#endif

/*:***********************************************************************
名  前：log_DateTime
機  能：現在の日付と時刻の文字列を返す
戻り値：なし
備  考：日付(yyyy/mm/dd),時刻(hh:mm:ss)
*************************************************************************/
#ifdef _UNICODE
void log_DateTimeEx(
				  wchar_t *strdate,		/*(I ) 日付 */
				  wchar_t *strtime		/*(I ) 時刻 */
				  )
#else
void log_DateTime(
				  char	*strdate,		/*(I ) 日付 */
				  char	*strtime		/*(I ) 時刻 */
				  )
/*:*/
#endif // _UNICODE
{
	time_t     ltime;
	struct tm  *ltm;
	
	time(&ltime);
	ltm = localtime(&ltime);
	_stprintf(strdate,_T("%u/%02u/%02u"),ltm->tm_year+1900,++ltm->tm_mon,ltm->tm_mday);
	_stprintf(strtime,_T("%02u:%02u:%02u"),ltm->tm_hour,ltm->tm_min,ltm->tm_sec);
}


/*:***********************************************************************
名  前：log_Fopen
機  能：ﾛｸﾞﾌｧｲﾙを開く
戻り値：正常終了：ﾌｧｲﾙﾎﾟｲﾝﾀ
異常終了：NULLﾎﾟｲﾝﾀ
備  考：ﾌｧｲﾙに対する書き込みｱｸｾｽを拒否します。
*************************************************************************/
FILE *log_FileOpen(void)
{
	return (_fsopen(LOG_FILENAME,"at",_SH_DENYWR));
}

/*:***********************************************************************
名  前：log_Fclose
機  能：ﾛｸﾞﾌｧｲﾙを閉じる
戻り値：正常終了：0 を返します。
異常終了：EOF を返します。
備  考：
*************************************************************************/
int log_FileClose(
			   FILE	*stream		/*(I ) ﾌｧｲﾙﾎﾟｲﾝﾀ */
			   )/*:*/
{
	return (fclose(stream));
}

/*:***********************************************************************
名  前：log_Printf
機  能：ﾛｸﾞﾌｧｲﾙに書式付で出力
戻り値：正常終了：出力した文字数を返します。
異常終了：負の値を返します。
備  考：
*************************************************************************/
int log_MsgPrintf(
				  const char	*format, ...	/*(I ) 書式制御文字列,ｵﾌﾟｼｮﾝの引数 */
				  )/*:*/
{
	int			stat;
	va_list		args;
	FILE		*stream;

	/* ﾛｸﾞﾌｧｲﾙｵｰﾌﾟﾝ */
	if((stream = log_FileOpen()) == NULL) return -1;

	/* 引数ﾘｽﾄから引数取得 */
	va_start(args,format);

	/* ﾛｸﾞﾌｧｲﾙに出力 */
#ifdef _PMFCAD
	stat = vfprintf(stream,format,args);
#else
#ifdef _UNICODE
	stat = _vftprintf(stream,format,args);
#else
	stat = vfprintf(stream,format,args);
#endif
#endif

	/* 引数ﾘｽﾄのﾎﾟｲﾝﾀﾘｾｯﾄ */
	va_end(args);

	/* ﾛｸﾞﾌｧｲﾙｸﾛｰｽﾞ */
	log_FileClose(stream);

	return stat;
}

