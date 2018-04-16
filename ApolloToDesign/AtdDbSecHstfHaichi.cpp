#include "stdafx.h"
#include "AtdDbSecHstfHaichi.h"

JptErrorStatus AtdDbSecHstfHaichi::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [断面･HSTF配置]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int nogrd = GetFieldValueInteger(rs, _T("NOGRD"));		//桁名
		int nopnl = GetFieldValueInteger(rs, _T("NOPNL"));		//始側格点名 終側格点名 格点間番号
		int nohsl1 = GetFieldValueInteger(rs, _T("NOHSL1"));	//配置段番号
		int nohsl2 = GetFieldValueInteger(rs, _T("NOHSL2"));	//配置段番号
		AtdDbSecHstfHaichiItem atdDbSecHstfHaichiItem;
		atdDbSecHstfHaichiItem.setNogrd(nogrd);
		atdDbSecHstfHaichiItem.setNopnl(nopnl);
		atdDbSecHstfHaichiItem.setNohsl1(nohsl1);
		atdDbSecHstfHaichiItem.setNohsl2(nohsl2);
		this->append(atdDbSecHstfHaichiItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbSecHstfHaichi::getAt(int index, AtdDbSecHstfHaichiItem& atdDbSecHstfHaichiItem)
{
	if(_atdDbSecHstfHaichiItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbSecHstfHaichiItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbSecHstfHaichiItem = _atdDbSecHstfHaichiItemList[index];

	return JPT_OK;
}

int AtdDbSecHstfHaichi::find(const int& ig, const int& ip)
{
	for(int i=0;i<this->size();i++) {
		AtdDbSecHstfHaichiItem shhItem;
		getAt(i, shhItem);
		int nogrd = shhItem.getNogrd();		//NOGRD 桁名
		int nopnl = shhItem.getNopnl();		//NOPNL 始側格点名 終側格点名 格点間番号
		if(nogrd == ig && nopnl == ip) {
			return i;
		}
	}

	return -1;
}

