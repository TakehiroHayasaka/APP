#include "stdafx.h"
#include "AtdDbSecHstf.h"

JptErrorStatus AtdDbSecHstf::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [’f–Ê¥HSTF’f–Ê]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int nohssc = GetFieldValueInteger(rs, _T("NOHSSC"));	//Ž¯•ÊID
		double hssc3 = GetFieldValueDouble(rs, _T("HSSC3"));	//•
		double hssc5 = GetFieldValueDouble(rs, _T("HSSC5"));	//”ÂŒú
		int hssc8 = GetFieldValueInteger(rs, _T("HSSC8"));		//ÞŽ¿
		if(nohssc > 0) {
			AtdDbSecHstfItem atdDbSecHstfItem;
			atdDbSecHstfItem.setNohssc(nohssc);
			atdDbSecHstfItem.setHssc3(hssc3);
			atdDbSecHstfItem.setHssc5(hssc5);
			atdDbSecHstfItem.setHssc8(hssc8);
			this->append(atdDbSecHstfItem);
		}
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbSecHstf::getAt(int index, AtdDbSecHstfItem& atdDbSecHstfItem)
{
	if(_atdDbSecHstfItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbSecHstfItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbSecHstfItem = _atdDbSecHstfItemList[index];

	return JPT_OK;
}

