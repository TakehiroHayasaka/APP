#include "stdafx.h"
#include "AtdDbSecManhole.h"

JptErrorStatus AtdDbSecManhole::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [断面･桁端部のマンホールのカット]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int nogrd = GetFieldValueInteger(rs, _T("NOGRD"));	//桁名
		int sepos = GetFieldValueInteger(rs, _T("SEPOS"));	//配置側
		int iwcut = GetFieldValueInteger(rs, _T("IWCUT"));	//
		AtdDbSecManholeItem atdDbSecManholeItem;
		atdDbSecManholeItem.setNogrd(nogrd);
		atdDbSecManholeItem.setSepos(sepos);
		atdDbSecManholeItem.setIwcut(iwcut);
		this->append(atdDbSecManholeItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbSecManhole::getAt(int index, AtdDbSecManholeItem& atdDbSecManholeItem)
{
	if(_atdDbSecManholeItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbSecManholeItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbSecManholeItem = _atdDbSecManholeItemList[index];

	return JPT_OK;
}

