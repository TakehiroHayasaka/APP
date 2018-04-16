#include "stdafx.h"
#include "AtdDbInputFillOption.h"

JptErrorStatus AtdDbInputFillOption::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [入力･フィラープレートオプション]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int ifilfu = GetFieldValueInteger(rs, _T("IFILFU"));	//
		int ifilfl = GetFieldValueInteger(rs, _T("IFILFL"));	//
		int ifilwb = GetFieldValueInteger(rs, _T("IFILWB"));	//
		AtdDbInputFillOptionItem atdDbInputFillOptionItem;
		atdDbInputFillOptionItem.setIfilfu(ifilfu);
		atdDbInputFillOptionItem.setIfilfl(ifilfl);
		atdDbInputFillOptionItem.setIfilwb(ifilwb);
		this->append(atdDbInputFillOptionItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbInputFillOption::getAt(int index, AtdDbInputFillOptionItem& atdDbInputFillOptionItem)
{
	if(_atdDbInputFillOptionItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbInputFillOptionItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbInputFillOptionItem = _atdDbInputFillOptionItemList[index];

	return JPT_OK;
}

