#include "stdafx.h"
#include "AtdDbLineCbeam.h"

JptErrorStatus AtdDbLineCbeam::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [線形･横桁(対傾構/ブラケット)]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int strcode = GetFieldValueInteger(rs, _T("STRCODE"));	//
		if(strcode == 4) {
			int nostr = GetFieldValueInteger(rs, _T("NOSTR"));	//
			int nosec = GetFieldValueInteger(rs, _T("NOSEC"));	//
			int nosvt = GetFieldValueInteger(rs, _T("NOSVT"));	//左側桁名
			int noevt = GetFieldValueInteger(rs, _T("NOEVT"));	//右側桁名
			int nocr = GetFieldValueInteger(rs, _T("NOCR"));	//格点名
			AtdDbLineCbeamItem atdDbLineCbeamItem;
			atdDbLineCbeamItem.setNostr(nostr);
			atdDbLineCbeamItem.setStrcode(strcode);
			atdDbLineCbeamItem.setNosec(nosec);
			atdDbLineCbeamItem.setNosvt(nosvt);
			atdDbLineCbeamItem.setNoevt(noevt);
			atdDbLineCbeamItem.setNocr(nocr);
			this->append(atdDbLineCbeamItem);
		}
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbLineCbeam::getAt(int index, AtdDbLineCbeamItem& atdDbLineCbeamItem)
{
	if(_atdDbLineCbeamItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbLineCbeamItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbLineCbeamItem = _atdDbLineCbeamItemList[index];

	return JPT_OK;
}

int AtdDbLineCbeam::findNostr(const int& ig1, const int& ig2, const int& npt)
{
	for(int i=0;i<this->size();i++) {
		AtdDbLineCbeamItem lcbItem;
		getAt(i, lcbItem);
		int nosvt = lcbItem.getNosvt();	//NOSVT 左側桁名
		int noevt = lcbItem.getNoevt();	//NOEVT 右側桁名
		int nocr = lcbItem.getNocr();	//NOCR 格点名
		if(nosvt == ig1*100 && noevt == ig2*100 && nocr == npt) {
			int nostr = lcbItem.getNostr();	//NOSTR
			return nostr;
		}
	}

	return -1;
}

int AtdDbLineCbeam::findNosec(const int& ig1, const int& ig2, const int& npt)
{
	for(int i=0;i<this->size();i++) {
		AtdDbLineCbeamItem lcbItem;
		getAt(i, lcbItem);
		int nosvt = lcbItem.getNosvt();	//NOSVT 左側桁名
		int noevt = lcbItem.getNoevt();	//NOEVT 右側桁名
		int nocr = lcbItem.getNocr();	//NOCR 格点名
		if(nosvt == ig1*100 && noevt == ig2*100 && nocr == npt) {
			int nosec = lcbItem.getNosec();	//NOSEC
			return nosec;
		}
	}

	return -1;
}

