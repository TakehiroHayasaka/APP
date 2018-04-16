#include "stdafx.h"
#include "AtdDbGrdVstfHaichi.h"

JptErrorStatus AtdDbGrdVstfHaichi::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [主桁中間垂直補剛材配置データ]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int nostr = GetFieldValueInteger(rs, _T("部材線コード"));		//部材線コード
		int nopanel = GetFieldValueInteger(rs, _T("パネルコード"));		//パネルコード
		int numvstf = GetFieldValueInteger(rs, _T("垂直補剛材本数"));	//垂直補剛材本数
		AtdDbGrdVstfHaichiItem atdDbGrdVstfHaichiItem;
		atdDbGrdVstfHaichiItem.setNostr(nostr);
		atdDbGrdVstfHaichiItem.setNopanel(nopanel);
		atdDbGrdVstfHaichiItem.setNumvstf(numvstf);
		this->append(atdDbGrdVstfHaichiItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbGrdVstfHaichi::getAt(int index, AtdDbGrdVstfHaichiItem& atdDbGrdVstfHaichiItem)
{
	if(_atdDbGrdVstfHaichiItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbGrdVstfHaichiItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbGrdVstfHaichiItem = _atdDbGrdVstfHaichiItemList[index];

	return JPT_OK;
}

