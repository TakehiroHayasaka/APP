#include "stdafx.h"
#include "AtdDbInputHaraikomiHoko.h"

JptErrorStatus AtdDbInputHaraikomiHoko::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [“ü—Í¥•¥‚¢ž‚Ý•ûŒü]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int strcode = GetFieldValueInteger(rs, _T("STRCODE"));	//
		if(strcode == 4) {
			int nostr = GetFieldValueInteger(rs, _T("NOSTR"));	//
			int itphr = GetFieldValueInteger(rs, _T("ITPHR"));	//
			AtdDbInputHaraikomiHokoItem atdDbInputHaraikomiHokoItem;
			atdDbInputHaraikomiHokoItem.setNostr(nostr);
			atdDbInputHaraikomiHokoItem.setItphr(itphr);
			this->append(atdDbInputHaraikomiHokoItem);
		}
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbInputHaraikomiHoko::getAt(int index, AtdDbInputHaraikomiHokoItem& atdDbInputHaraikomiHokoItem)
{
	if(_atdDbInputHaraikomiHokoItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbInputHaraikomiHokoItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbInputHaraikomiHokoItem = _atdDbInputHaraikomiHokoItemList[index];

	return JPT_OK;
}

int AtdDbInputHaraikomiHoko::findItphr(const int& nostr)
{
	int itphr = -1;
	for(int i=0;i<this->size();i++) {
		AtdDbInputHaraikomiHokoItem lgpItem;
		getAt(i, lgpItem);
		int nostr2 = lgpItem.getNostr();		//
		if(nostr2 == nostr) {
			itphr = lgpItem.getItphr();	//
			break;
		}
	}

	return itphr;
}

