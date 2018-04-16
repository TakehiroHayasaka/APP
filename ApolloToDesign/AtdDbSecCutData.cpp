#include "stdafx.h"
#include "AtdDbSecCutData.h"

JptErrorStatus AtdDbSecCutData::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [断面･カットデータ]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		double dtcut1 = GetFieldValueDouble(rs, _T("DTCUT1"));	//
		double dtcut2 = GetFieldValueDouble(rs, _T("DTCUT2"));	//MH_位置
		double dtcut4 = GetFieldValueDouble(rs, _T("DTCUT4"));	//MH_高さ
		double dtcut3 = GetFieldValueDouble(rs, _T("DTCUT3"));	//MH_幅
		double dtcut5 = GetFieldValueDouble(rs, _T("DTCUT5"));	//MH_Rサイズ
		AtdDbSecCutDataItem atdDbSecCutDataItem;
		atdDbSecCutDataItem.setDtcut1((int)dtcut1);
		atdDbSecCutDataItem.setDtcut2(dtcut2);
		atdDbSecCutDataItem.setDtcut4(dtcut4);
		atdDbSecCutDataItem.setDtcut3(dtcut3);
		atdDbSecCutDataItem.setDtcut5(dtcut5);
		this->append(atdDbSecCutDataItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbSecCutData::getAt(int index, AtdDbSecCutDataItem& atdDbSecCutDataItem)
{
	if(_atdDbSecCutDataItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbSecCutDataItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbSecCutDataItem = _atdDbSecCutDataItemList[index];

	return JPT_OK;
}

