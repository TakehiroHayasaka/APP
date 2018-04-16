#include "stdafx.h"
#include "AtdDbSecVstf.h"

JptErrorStatus AtdDbSecVstf::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [’f–Ê¥VSTF’f–Ê]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int novssc = GetFieldValueInteger(rs, _T("NOVSSC"));	//Ž¯•ÊID
		double vssc3 = GetFieldValueDouble(rs, _T("VSSC3"));	//•
		double vssc5 = GetFieldValueDouble(rs, _T("VSSC5"));	//”ÂŒú
		int vssc8 = GetFieldValueInteger(rs, _T("VSSC8"));		//ÞŽ¿
		if(novssc > 0) {
			AtdDbSecVstfItem atdDbSecVstfItem;
			atdDbSecVstfItem.setNovssc(novssc);
			atdDbSecVstfItem.setVssc3(vssc3);
			atdDbSecVstfItem.setVssc5(vssc5);
			atdDbSecVstfItem.setVssc8(vssc8);
			this->append(atdDbSecVstfItem);
		}
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbSecVstf::getAt(int index, AtdDbSecVstfItem& atdDbSecVstfItem)
{
	if(_atdDbSecVstfItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbSecVstfItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbSecVstfItem = _atdDbSecVstfItemList[index];

	return JPT_OK;
}

