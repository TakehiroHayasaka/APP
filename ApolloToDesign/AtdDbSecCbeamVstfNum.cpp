#include "stdafx.h"
#include "AtdDbSecCbeamVstfNum.h"

JptErrorStatus AtdDbSecCbeamVstfNum::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [’f–Ê¥‰¡Œ…¥ƒpƒlƒ‹–ˆ‚ÌVSTF–{”]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int nocrs = GetFieldValueInteger(rs, _T("NOCRS"));	//
		int nvstc = GetFieldValueInteger(rs, _T("NVSTC"));	//
		AtdDbSecCbeamVstfNumItem atdDbSecCbeamVstfNumItem;
		atdDbSecCbeamVstfNumItem.setNocrs(nocrs);
		atdDbSecCbeamVstfNumItem.setNvstc(nvstc);
		this->append(atdDbSecCbeamVstfNumItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbSecCbeamVstfNum::getAt(int index, AtdDbSecCbeamVstfNumItem& atdDbSecCbeamVstfNumItem)
{
	if(_atdDbSecCbeamVstfNumItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbSecCbeamVstfNumItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbSecCbeamVstfNumItem = _atdDbSecCbeamVstfNumItemList[index];

	return JPT_OK;
}

int AtdDbSecCbeamVstfNum::findNvstc(const int& nocrs)
{
	int nvstc = -1;
	for(int i=0;i<this->size();i++) {
		AtdDbSecCbeamVstfNumItem lgpItem;
		getAt(i, lgpItem);
		int nocrs2 = lgpItem.getNocrs();		//


		if(nocrs2 == nocrs) {
			nvstc = lgpItem.getNvstc();	//
			break;
		}
	}

	return nvstc;
}

