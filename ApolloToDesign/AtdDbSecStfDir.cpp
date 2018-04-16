#include "stdafx.h"
#include "AtdDbSecStfDir.h"

JptErrorStatus AtdDbSecStfDir::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [断面･詳細･補剛材の向き]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int strcode = GetFieldValueInteger(rs, _T("STRCODE"));	//
		if(strcode == 3) {
			int nostr = GetFieldValueInteger(rs, _T("NOSTR"));	//
			int idrv = GetFieldValueInteger(rs, _T("IDRV"));	//配置面
			int idrh = GetFieldValueInteger(rs, _T("IDRH"));	//配置面
			AtdDbSecStfDirItem atdDbSecStfDirItem;
			atdDbSecStfDirItem.setNostr(nostr);
			atdDbSecStfDirItem.setIdrv(idrv);
			atdDbSecStfDirItem.setIdrh(idrh);
			this->append(atdDbSecStfDirItem);
		}
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbSecStfDir::getAt(int index, AtdDbSecStfDirItem& atdDbSecStfDirItem)
{
	if(_atdDbSecStfDirItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbSecStfDirItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbSecStfDirItem = _atdDbSecStfDirItemList[index];

	return JPT_OK;
}

int AtdDbSecStfDir::find(const int& nostr)
{
	for(int i=0;i<this->size();i++) {
		AtdDbSecStfDirItem ssdItem;
		getAt(i, ssdItem);
		int nostr2 = ssdItem.getNostr();		//NOSTR
		if(nostr2 == nostr) {
			return i;
		}
	}

	return -1;
}

int AtdDbSecStfDir::findIdrv(const int& nostr)
{
	for(int i=0;i<this->size();i++) {
		AtdDbSecStfDirItem ssdItem;
		getAt(i, ssdItem);
		int nostr2 = ssdItem.getNostr();		//NOSTR
		if(nostr2 == nostr) {
			int idrv = ssdItem.getIdrv();	//IDRV 配置面
			return idrv;
		}
	}

	return -1;
}

