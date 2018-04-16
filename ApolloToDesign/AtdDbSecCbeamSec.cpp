#include "stdafx.h"
#include "AtdDbSecCbeamSec.h"

JptErrorStatus AtdDbSecCbeamSec::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [’f–Ê¥‰¡Œ…¥’f–Ê]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int nocrs = GetFieldValueInteger(rs, _T("NOCRS"));	//NOCRS
		double bfcu = GetFieldValueDouble(rs, _T("BFCU"));	//UFLG_• H|ƒtƒ‰ƒ“ƒW•
		double tfcu = GetFieldValueDouble(rs, _T("TFCU"));	//UFLG_”ÂŒú H|ƒtƒ‰ƒ“ƒW”ÂŒú
		int mfcu = GetFieldValueInteger(rs, _T("MFCU"));	//UFLG_ÞŽ¿ H|ÞŽ¿
		double tfcw = GetFieldValueDouble(rs, _T("TFCW"));	//WEB_”ÂŒú H|ƒEƒFƒu”ÂŒú
		int mfcw = GetFieldValueInteger(rs, _T("MFCW"));	//WEB_ÞŽ¿
		double bfcl = GetFieldValueDouble(rs, _T("BFCL"));	//LFLG_• H|ƒtƒ‰ƒ“ƒW•
		double tfcl = GetFieldValueDouble(rs, _T("TFCL"));	//LFLG_”ÂŒú H|ƒtƒ‰ƒ“ƒW”ÂŒú
		int mfcl = GetFieldValueInteger(rs, _T("MFCL"));	//LFLG_ÞŽ¿ H|ÞŽ¿
		AtdDbSecCbeamSecItem atdDbSecCbeamSecItem;
		atdDbSecCbeamSecItem.setNocrs(nocrs);
		atdDbSecCbeamSecItem.setBfcu(bfcu);
		atdDbSecCbeamSecItem.setTfcu(tfcu);
		atdDbSecCbeamSecItem.setMfcu(mfcu);
		atdDbSecCbeamSecItem.setTfcw(tfcw);
		atdDbSecCbeamSecItem.setMfcw(mfcw);
		atdDbSecCbeamSecItem.setBfcl(bfcl);
		atdDbSecCbeamSecItem.setTfcl(tfcl);
		atdDbSecCbeamSecItem.setMfcl(mfcl);
		this->append(atdDbSecCbeamSecItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbSecCbeamSec::getAt(int index, AtdDbSecCbeamSecItem& atdDbSecCbeamSecItem)
{
	if(_atdDbSecCbeamSecItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbSecCbeamSecItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbSecCbeamSecItem = _atdDbSecCbeamSecItemList[index];

	return JPT_OK;
}

