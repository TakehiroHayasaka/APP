#include "stdafx.h"
#include "AtdDbInputGrdMen.h"

JptErrorStatus AtdDbInputGrdMen::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [“ü—Í¥ŽåŒ…‘¤–ÊŒ`ó]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int strcode = GetFieldValueInteger(rs, _T("STRCODE"));	//
		if(strcode == 3) {
			int strno = GetFieldValueInteger(rs, _T("STRNO"));	//Œ…–¼
			int itpuw = GetFieldValueInteger(rs, _T("ITPUW"));	//‘¤–Ê•âŠÔ•û–@
			int itpww = GetFieldValueInteger(rs, _T("ITPWW"));	//
			AtdDbInputGrdMenItem atdDbInputGrdMenItem;
			atdDbInputGrdMenItem.setStrno(strno);
			atdDbInputGrdMenItem.setItpuw(itpuw);
			atdDbInputGrdMenItem.setItpww(itpww);
			this->append(atdDbInputGrdMenItem);
		}
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbInputGrdMen::getAt(int index, AtdDbInputGrdMenItem& atdDbInputGrdMenItem)
{
	if(_atdDbInputGrdMenItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbInputGrdMenItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbInputGrdMenItem = _atdDbInputGrdMenItemList[index];

	return JPT_OK;
}

