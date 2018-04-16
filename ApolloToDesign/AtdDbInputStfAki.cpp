#include "stdafx.h"
#include "AtdDbInputStfAki.h"

JptErrorStatus AtdDbInputStfAki::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [“ü—Í¥‚r‚s‚e‚Ì‚ ‚«]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int idgfvi = GetFieldValueInteger(rs, _T("IDGFVI"));	//“ü—ÍESTF‚Ì‚ ‚« ƒ^ƒCƒv
		AtdDbInputStfAkiItem atdDbInputStfAkiItem;
		atdDbInputStfAkiItem.setIdgfvi(idgfvi);
		this->append(atdDbInputStfAkiItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbInputStfAki::getAt(int index, AtdDbInputStfAkiItem& atdDbInputStfAkiItem)
{
	if(_atdDbInputStfAkiItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbInputStfAkiItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbInputStfAkiItem = _atdDbInputStfAkiItemList[index];

	return JPT_OK;
}

