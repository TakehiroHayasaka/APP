#pragma warning(disable : 4995)
/*---------------------------------------------------------------------------
 ExtCDaoDatabase.h
	MFC4.2 の DAO に関する機能を補強する関数群
---------------------------------------------------------------------------*/

#ifndef __JJG30211_EXTCDAODATABASE_H__
#define __JJG30211_EXTCDAODATABASE_H__

#include <afxdao.h> 
extern int UserASSERT(int f,LPCSTR lpszFileName, int nLine);
#ifdef ASSERT
#undef ASSERT
#endif
#define	ASSERT(f)	UserASSERT(f,__FILE__,__LINE__)

/*---------------------------------------------------------------------------
 レコードセットのレコード数を返す
	戻り値
		レコード数
	rs
		レコードセット
	解説
		CDaoRecordset.GetRecordCount() が正しいレコード数を返さない場合
		があるバグへの対応処置。
		一度最下段レコードまでカーソルを移動すれば正しいレコード数を返す。
	注意
		この関数を使用するとカーソルは最上段レコードに移動する。
---------------------------------------------------------------------------*/
inline int GetCount(CDaoRecordset &rs)
{
	int nCount = 0;
	if (rs.IsOpen())
	{
		if (!rs.IsEOF())
			rs.MoveLast();
		nCount = rs.GetRecordCount();
		if (!rs.IsBOF())
			rs.MoveFirst();
	}

	return nCount;
}

/*---------------------------------------------------------------------------
 カーソルレコードの値（整数）を返す
	戻り値
		カーソルレコードの値
	rs
		レコードセット
	strFieldName
		フィールド名
---------------------------------------------------------------------------*/
inline int GetFieldValueInteger(CDaoRecordset &rs, const CString strFieldName)
{
	COleVariant vValue;
	rs.GetFieldValue(strFieldName, vValue);

#ifdef _DEBUG
	CDaoFieldInfo info;
	rs.GetFieldInfo(strFieldName, info);
	ASSERT(info.m_nType == dbLong || info.m_nType == dbInteger);
#endif

	return vValue.intVal;
}

//マイナスの値がおかしくなる場合に使用
inline int GetFieldValueInteger2(CDaoRecordset &rs, const CString strFieldName)
{
	COleVariant vValue;
	rs.GetFieldValue(strFieldName, vValue);

#ifdef _DEBUG
	CDaoFieldInfo info;
	rs.GetFieldInfo(strFieldName, info);
	ASSERT(info.m_nType == dbLong || info.m_nType == dbInteger);
#endif
	unsigned short valueUnsignedShort = vValue.intVal;
	short valueShort = valueUnsignedShort;
	int valueInt = (int)valueShort;
	return valueInt;
}

/*---------------------------------------------------------------------------
 カーソルレコードの値（実数）を返す
	戻り値
		カーソルレコードの値
	rs
		レコードセット
	strFieldName
		フィールド名
---------------------------------------------------------------------------*/
inline double GetFieldValueDouble(CDaoRecordset &rs, const CString strFieldName)
{
	COleVariant vValue;
	rs.GetFieldValue(strFieldName, vValue);

#ifdef _DEBUG
	CDaoFieldInfo info;
	rs.GetFieldInfo(strFieldName, info);
	ASSERT(info.m_nType == dbDouble);
#endif

	return vValue.dblVal;
}

/*---------------------------------------------------------------------------
 カーソルレコードの値（文字列）を返す
	戻り値
		カーソルレコードの値
	rs
		レコードセット
	strFieldName
		フィールド名
---------------------------------------------------------------------------*/
inline CString GetFieldValueString(CDaoRecordset &rs, const CString strFieldName)
{
	COleVariant vValue;
	rs.GetFieldValue(strFieldName, vValue);

#ifdef _DEBUG
	CDaoFieldInfo info;
	rs.GetFieldInfo(strFieldName, info);
	ASSERT(info.m_nType == dbText);
#endif

	return vValue.pcVal;
}

/*---------------------------------------------------------------------------
 カーソルレコードの値（メモ型）を返す
	戻り値
		カーソルレコードの値
	rs
		レコードセット
	strFieldName
		フィールド名
---------------------------------------------------------------------------*/
inline CString GetFieldValueMemo(CDaoRecordset &rs, const CString strFieldName)
{
	COleVariant vValue;
	rs.GetFieldValue(strFieldName, vValue);

#ifdef _DEBUG
	CDaoFieldInfo info;
	rs.GetFieldInfo(strFieldName, info);
	ASSERT(info.m_nType == dbMemo);
#endif

	return vValue.pcVal;
}

#endif	// __JJG30211_EXTCDAODATABASE_H__
