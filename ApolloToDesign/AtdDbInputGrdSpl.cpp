#include "stdafx.h"
#include "AtdDbInputGrdSpl.h"

JptErrorStatus AtdDbInputGrdSpl::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [“ü—Í¥ŽåŒ…“YÚ]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int iclegu = GetFieldValueInteger(rs, _T("ICLEGU"));	//UFLG_JC
		int iclegw = GetFieldValueInteger(rs, _T("ICLEGW"));	//WEB_JC
		int iclegl = GetFieldValueInteger(rs, _T("ICLEGL"));	//LFLG_JC
		AtdDbInputGrdSplItem atdDbInputGrdSplItem;
		atdDbInputGrdSplItem.setIclegu(iclegu);
		atdDbInputGrdSplItem.setIclegw(iclegw);
		atdDbInputGrdSplItem.setIclegl(iclegl);
		this->append(atdDbInputGrdSplItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbInputGrdSpl::getAt(int index, AtdDbInputGrdSplItem& atdDbInputGrdSplItem)
{
	if(_atdDbInputGrdSplItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbInputGrdSplItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbInputGrdSplItem = _atdDbInputGrdSplItemList[index];

	return JPT_OK;
}

