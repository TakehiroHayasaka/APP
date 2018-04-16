#include "stdafx.h"
#include "AtdDbLineGrdZahyo.h"

JptErrorStatus AtdDbLineGrdZahyo::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [üŒ`¥ŽåŒ…À•W(cŒ…/‘¤cŒ…)]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int strcode = GetFieldValueInteger(rs, _T("STRCODE"));	//
		if(strcode == 3) {
			int nostr = GetFieldValueInteger(rs, _T("NOSTR"));	//
			int nopt = GetFieldValueInteger(rs, _T("NOPT"));	//
			int itmb = GetFieldValueInteger(rs, _T("ITMB"));	//
			double xu = GetFieldValueDouble(rs, _T("XU"));	//
			double yu = GetFieldValueDouble(rs, _T("YU"));	//
			double zu = GetFieldValueDouble(rs, _T("ZU"));	//
			AtdDbLineGrdZahyoItem atdDbLineGrdZahyoItem;
			atdDbLineGrdZahyoItem.setNostr(nostr);
			atdDbLineGrdZahyoItem.setNopt(nopt);
			atdDbLineGrdZahyoItem.setItmb(itmb);
			atdDbLineGrdZahyoItem.setXu(xu);
			atdDbLineGrdZahyoItem.setYu(yu);
			atdDbLineGrdZahyoItem.setZu(zu);
			this->append(atdDbLineGrdZahyoItem);
		}
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbLineGrdZahyo::getAt(int index, AtdDbLineGrdZahyoItem& atdDbLineGrdZahyoItem)
{
	if(_atdDbLineGrdZahyoItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbLineGrdZahyoItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbLineGrdZahyoItem = _atdDbLineGrdZahyoItemList[index];

	return JPT_OK;
}

