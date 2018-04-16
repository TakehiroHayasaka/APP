#include "stdafx.h"
#include "AtdDbSplCommon.h"

JptErrorStatus AtdDbSplCommon::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [ìYê⁄•ê¸å`ìYê⁄ã§í ÉfÅ[É^]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int strcode = GetFieldValueInteger(rs, _T("STRCODE"));	//
		if(strcode == 3 || strcode == 4) {
			double phsg = GetFieldValueDouble(rs, _T("PHSG"));	//çEåa
			AtdDbSplCommonItem atdDbSplCommonItem;
			atdDbSplCommonItem.setStrcode(strcode);
			atdDbSplCommonItem.setPhsg(phsg);
			this->append(atdDbSplCommonItem);
		}
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbSplCommon::getAt(int index, AtdDbSplCommonItem& atdDbSplCommonItem)
{
	if(_atdDbSplCommonItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbSplCommonItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbSplCommonItem = _atdDbSplCommonItemList[index];

	return JPT_OK;
}

double AtdDbSplCommon::findHoleSize(const int& strcode)
{
	for(int i=0;i<this->size();i++) {
		AtdDbSplCommonItem scmItem;
		getAt(i, scmItem);
		int strcode2 = scmItem.getStrcode();	//STRCODE
		if(strcode2 == strcode) {
			double phsg = scmItem.getPhsg();	//PHSG çEåa
			return phsg;
		}
	}

	return 0.0;
}
