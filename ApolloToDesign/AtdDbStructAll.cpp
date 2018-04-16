#include "stdafx.h"
#include "AtdDbStructAll.h"

JptErrorStatus AtdDbStructAll::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [\¬¥‘S‘Ì]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int nspan = GetFieldValueInteger(rs, _T("NSPAN"));	//
		AtdDbStructAllItem atdDbStructAllItem;
		atdDbStructAllItem.setNspan(nspan);
		this->append(atdDbStructAllItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbStructAll::getAt(int index, AtdDbStructAllItem& atdDbStructAllItem)
{
	if(_atdDbStructAllItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbStructAllItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbStructAllItem = _atdDbStructAllItemList[index];

	return JPT_OK;
}

