#include "stdafx.h"
#include "AtdDbSecScaleSpl.h"

JptErrorStatus AtdDbSecScaleSpl::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [’f–Ê¥ƒXƒP[ƒ‹‹y‚Ñ•¶Žš‚‚³‚ÆÞŽ¿Žd—l¥ŽåŒ…“YÚŠÖŒW]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int ibuuti = GetFieldValueInteger(rs, _T("IBUUTI"));	//UFLG”ÂŒú“¦‚°•ûŒü
		int ibluti = GetFieldValueInteger(rs, _T("IBLUTI"));	//LFLG”ÂŒú“¦‚°•ûŒü
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

