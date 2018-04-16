#include "stdafx.h"
#include "AtdDbSecGrdLeng.h"

JptErrorStatus AtdDbSecGrdLeng::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [断面･主桁の断面長]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int nogrd = GetFieldValueInteger(rs, _T("NOGRD"));		//桁名
		int nosec = GetFieldValueInteger(rs, _T("NOSEC"));		//ジョイント名
		double rlsec = GetFieldValueDouble(rs, _T("RLSEC"));	//ブロック長
		AtdDbSecGrdLengItem atdDbSecGrdLengItem;
		atdDbSecGrdLengItem.setNogrd(nogrd);
		atdDbSecGrdLengItem.setNosec(nosec);
		atdDbSecGrdLengItem.setRlsec(rlsec);
		this->append(atdDbSecGrdLengItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbSecGrdLeng::getAt(int index, AtdDbSecGrdLengItem& atdDbSecGrdLengItem)
{
	if(_atdDbSecGrdLengItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbSecGrdLengItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbSecGrdLengItem = _atdDbSecGrdLengItemList[index];

	return JPT_OK;
}

