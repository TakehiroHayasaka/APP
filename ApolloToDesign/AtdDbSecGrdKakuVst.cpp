#include "stdafx.h"
#include "AtdDbSecGrdKakuVst.h"

JptErrorStatus AtdDbSecGrdKakuVst::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [断面･主桁格点上VSTF]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int nogrd = GetFieldValueInteger(rs, _T("NOGRD"));		//桁名
		int nocrs = GetFieldValueInteger(rs, _T("NOCRS"));		//
		int novstp = GetFieldValueInteger(rs, _T("NOVSTP"));	//配置面 識別ID
		AtdDbSecGrdKakuVstItem atdDbSecGrdKakuVstItem;
		atdDbSecGrdKakuVstItem.setNogrd(nogrd);
		atdDbSecGrdKakuVstItem.setNocrs(nocrs);
		atdDbSecGrdKakuVstItem.setNovstp(novstp);
		this->append(atdDbSecGrdKakuVstItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbSecGrdKakuVst::getAt(int index, AtdDbSecGrdKakuVstItem& atdDbSecGrdKakuVstItem)
{
	if(_atdDbSecGrdKakuVstItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbSecGrdKakuVstItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbSecGrdKakuVstItem = _atdDbSecGrdKakuVstItemList[index];

	return JPT_OK;
}

int AtdDbSecGrdKakuVst::findId(const int& ig, const int& ip)
{
	for(int i=0;i<this->size();i++) {
		AtdDbSecGrdKakuVstItem sgvItem;
		getAt(i, sgvItem);
		int nogrd = sgvItem.getNogrd();		//NOGRD 桁名
		int nocrs = sgvItem.getNocrs();		//NOCRS
		if(nogrd == ig && nocrs == ip) {
			int novstp = sgvItem.getNovstp();	//NOVSTP 配置面 識別ID
			return novstp;
		}
	}

	return 0;
}
