#include "stdafx.h"
#include "AtdDbInputKetatanLeng.h"

JptErrorStatus AtdDbInputKetatanLeng::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [“ü—Í¥Œ…’[’·]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int strcode = GetFieldValueInteger(rs, _T("STRCODE"));	//
		if(strcode == 3) {
			int strno = GetFieldValueInteger(rs, _T("STRNO"));	//
			double rls = GetFieldValueDouble(rs, _T("RLS"));	//
			double rle = GetFieldValueDouble(rs, _T("RLE"));	//
			AtdDbInputKetatanLengItem atdDbInputKetatanLengItem;
			atdDbInputKetatanLengItem.setStrno(strno);
			atdDbInputKetatanLengItem.setRls(rls);
			atdDbInputKetatanLengItem.setRle(rle);
			this->append(atdDbInputKetatanLengItem);
		}
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbInputKetatanLeng::getAt(int index, AtdDbInputKetatanLengItem& atdDbInputKetatanLengItem)
{
	if(_atdDbInputKetatanLengItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbInputKetatanLengItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbInputKetatanLengItem = _atdDbInputKetatanLengItemList[index];

	return JPT_OK;
}

int AtdDbInputKetatanLeng::find(const int& ig)
{
	for(int i=0;i<this->size();i++) {
		AtdDbInputKetatanLengItem iklItem;
		getAt(i, iklItem);
		int strno = iklItem.getStrno();		//STRNO
		if(strno == ig) {
			return i;
		}
	}

	return -1;
}

