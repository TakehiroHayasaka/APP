#include "stdafx.h"
#include "AtdDbSecSolePl.h"

JptErrorStatus AtdDbSecSolePl::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [断面･ソールプレート]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int nogrd = GetFieldValueInteger(rs, _T("NOGRD"));	//桁名
		int nos = GetFieldValueInteger(rs, _T("NOS"));	//
		double bs1 = GetFieldValueDouble(rs, _T("BS1"));	//SOLE-PL幅
		double rls1 = GetFieldValueDouble(rs, _T("RLS1"));	//SOLE-PL長さ
		AtdDbSecSolePlItem atdDbSecSolePlItem;
		atdDbSecSolePlItem.setNogrd(nogrd);
		atdDbSecSolePlItem.setNos(nos);
		atdDbSecSolePlItem.setBs1(bs1);
		atdDbSecSolePlItem.setRls1(rls1);
		this->append(atdDbSecSolePlItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbSecSolePl::getAt(int index, AtdDbSecSolePlItem& atdDbSecSolePlItem)
{
	if(_atdDbSecSolePlItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbSecSolePlItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbSecSolePlItem = _atdDbSecSolePlItemList[index];

	return JPT_OK;
}

