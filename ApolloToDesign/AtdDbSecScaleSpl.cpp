#include "stdafx.h"
#include "AtdDbSecScaleSpl.h"

JptErrorStatus AtdDbSecScaleSpl::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [fÊ¥XP[yÑ¶³ÆÞ¿dl¥åYÚÖW]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int ibuuti = GetFieldValueInteger(rs, _T("IBUUTI"));	//UFLGÂú¦°ûü
		int ibluti = GetFieldValueInteger(rs, _T("IBLUTI"));	//LFLGÂú¦°ûü
		AtdDbSecScaleSplItem atdDbSecScaleSplItem;
		atdDbSecScaleSplItem.setIbuuti(ibuuti);
		atdDbSecScaleSplItem.setIbluti(ibluti);
		this->append(atdDbSecScaleSplItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbSecScaleSpl::getAt(int index, AtdDbSecScaleSplItem& atdDbSecScaleSplItem)
{
	if(_atdDbSecScaleSplItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbSecScaleSplItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbSecScaleSplItem = _atdDbSecScaleSplItemList[index];

	return JPT_OK;
}

