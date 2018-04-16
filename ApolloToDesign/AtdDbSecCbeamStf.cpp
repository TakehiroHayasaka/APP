#include "stdafx.h"
#include "AtdDbSecCbeamStf.h"

JptErrorStatus AtdDbSecCbeamStf::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [’f–Ê¥‰¡Œ…¥•â„Þ’f–Ê”]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int nocrs = GetFieldValueInteger(rs, _T("NOCRS"));		//
		int posstf = GetFieldValueInteger(rs, _T("POSSTF"));	//
		double stcw = GetFieldValueDouble(rs, _T("STCW"));	//•
		double stct = GetFieldValueDouble(rs, _T("STCT"));	//”ÂŒú
		int stcm = GetFieldValueInteger(rs, _T("STCM"));	//ÞŽ¿
		AtdDbSecCbeamStfItem atdDbSecCbeamStfItem;
		atdDbSecCbeamStfItem.setNocrs(nocrs);
		atdDbSecCbeamStfItem.setPosstf(posstf);
		atdDbSecCbeamStfItem.setStcw(stcw);
		atdDbSecCbeamStfItem.setStct(stct);
		atdDbSecCbeamStfItem.setStcm(stcm);
		this->append(atdDbSecCbeamStfItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbSecCbeamStf::getAt(int index, AtdDbSecCbeamStfItem& atdDbSecCbeamStfItem)
{
	if(_atdDbSecCbeamStfItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbSecCbeamStfItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbSecCbeamStfItem = _atdDbSecCbeamStfItemList[index];

	return JPT_OK;
}

