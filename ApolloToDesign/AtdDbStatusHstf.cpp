// 2018/02/28 take Add Start
#include "stdafx.h"
#include "AtdDbStatusHstf.h"

JptErrorStatus AtdDbStatusHstf::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [’f–Ê¥HSTF‚ÌˆÊ’uŠÖŒW(’i”•Ï‰»’·¥’i”)]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int nogrd = GetFieldValueInteger(rs, _T("NOGRD"));
		double rlsfh = GetFieldValueDouble(rs, _T("RLSFH"));
		int nsfh = GetFieldValueInteger(rs, _T("NSFH"));
		AtdDbStatusHstfItem atdDbStatusHstfItem;
		atdDbStatusHstfItem.setNoGrd(nogrd);
		atdDbStatusHstfItem.setRlsfh(rlsfh);
		atdDbStatusHstfItem.setNsfh(nsfh);
		this->append(atdDbStatusHstfItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbStatusHstf::getAt(int index, AtdDbStatusHstfItem& atdDbStatusHstfItem)
{
	if(_atdDbStatusHstfItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbStatusHstfItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbStatusHstfItem = _atdDbStatusHstfItemList[index];

	return JPT_OK;
}
// 2018/02/28 take Add End