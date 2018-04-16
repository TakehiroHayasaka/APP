#include "stdafx.h"
#include "AtdDbInputCbeamConnSpl.h"

JptErrorStatus AtdDbInputCbeamConnSpl::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [入力・横桁コネクション添接]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int nosec = GetFieldValueInteger(rs, _T("NOSEC"));		//
		int iposul = GetFieldValueInteger(rs, _T("IPOSUL"));	//
		int jtype = GetFieldValueInteger(rs, _T("JTYPE"));		//
		if(jtype == 2) {
			double edgec = GetFieldValueDouble(rs, _T("EDGEC"));		//ConnPL連結外側縁端
			double gagec = GetFieldValueDouble(rs, _T("GAGEC"));		//ConnPLゲージ
			double gagecenc = GetFieldValueDouble(rs, _T("GAGECENC"));	//ConnPL中心ゲージ
			AtdDbInputCbeamConnSplItem atdDbInputCbeamConnSplItem;
			atdDbInputCbeamConnSplItem.setNosec(nosec);
			atdDbInputCbeamConnSplItem.setIposul(iposul);
			atdDbInputCbeamConnSplItem.setJtype(jtype);
			atdDbInputCbeamConnSplItem.setEdgec(edgec);
			atdDbInputCbeamConnSplItem.setGagec(gagec);
			atdDbInputCbeamConnSplItem.setGagecenc(gagecenc);
			this->append(atdDbInputCbeamConnSplItem);
		}
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbInputCbeamConnSpl::getAt(int index, AtdDbInputCbeamConnSplItem& atdDbInputCbeamConnSplItem)
{
	if(_atdDbInputCbeamConnSplItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbInputCbeamConnSplItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbInputCbeamConnSplItem = _atdDbInputCbeamConnSplItemList[index];

	return JPT_OK;
}

int AtdDbInputCbeamConnSpl::find(const int& nosec, const int& iposul)
{
	for(int i=0;i<this->size();i++) {
		AtdDbInputCbeamConnSplItem iccItem;
		getAt(i, iccItem);

		int nosec2 = iccItem.getNosec();		//NOSEC
		int iposul2 = iccItem.getIposul();		//IPOSUL
		if(nosec2 == nosec && iposul2 == iposul) {
			return i;
		}
	}

	return -1;
}

