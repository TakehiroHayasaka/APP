#include "stdafx.h"
#include "AtdDbSecGrdHeightVariable.h"

JptErrorStatus AtdDbSecGrdHeightVariable::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [’f–Ê¥ŽåŒ…• ”Â(Œ…‚’†S‰Â•Ï)]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int nogrd = GetFieldValueInteger(rs, _T("NOGRD"));		//
		double rcweb = GetFieldValueDouble(rs, _T("RCWEB"));	//
		double hcweb = GetFieldValueDouble(rs, _T("HCWEB"));	//
		int itplc = GetFieldValueInteger(rs, _T("ITPLC"));		//
		int itphc = GetFieldValueInteger(rs, _T("ITPHC"));		//
		AtdDbSecGrdHeightVariableItem atdDbSecGrdHeightVariableItem;
		atdDbSecGrdHeightVariableItem.setNogrd(nogrd);
		atdDbSecGrdHeightVariableItem.setRcweb(rcweb);
		atdDbSecGrdHeightVariableItem.setHcweb(hcweb);
		atdDbSecGrdHeightVariableItem.setItplc(itplc);
		atdDbSecGrdHeightVariableItem.setItphc(itphc);
		this->append(atdDbSecGrdHeightVariableItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbSecGrdHeightVariable::getAt(int index, AtdDbSecGrdHeightVariableItem& atdDbSecGrdHeightVariableItem)
{
	if(_atdDbSecGrdHeightVariableItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbSecGrdHeightVariableItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbSecGrdHeightVariableItem = _atdDbSecGrdHeightVariableItemList[index];

	return JPT_OK;
}

