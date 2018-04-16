#include "stdafx.h"
#include "AtdDbInputUseMaterial.h"

JptErrorStatus AtdDbInputUseMaterial::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [入力・使用材料データ]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int mt = GetFieldValueInteger(rs, _T("MT"));	//
		double t1 = GetFieldValueDouble(rs, _T("T1"));	//
		double t2 = GetFieldValueDouble(rs, _T("T2"));	//
		CString amt = GetFieldValueString(rs, _T("AMT"));	//
		AtdDbInputUseMaterialItem atdDbInputUseMaterialItem;
		atdDbInputUseMaterialItem.setMt(mt);
		atdDbInputUseMaterialItem.setT1(t1);
		atdDbInputUseMaterialItem.setT2(t2);
		atdDbInputUseMaterialItem.setAmt(amt);
		this->append(atdDbInputUseMaterialItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbInputUseMaterial::getAt(int index, AtdDbInputUseMaterialItem& atdDbInputUseMaterialItem)
{
	if(_atdDbInputUseMaterialItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbInputUseMaterialItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbInputUseMaterialItem = _atdDbInputUseMaterialItemList[index];

	return JPT_OK;
}

string AtdDbInputUseMaterial::findMaterial(const double& itaatsu, const int& mno)
{
	string material = "";
	if(mno == 0) {
		material = "SS400";
	} else if(mno == 1) {
		material = "SM400";
	} else if(mno == 2) {
		material = "SM490";
	} else if(mno == 3) {
		material = "SM490Y";
	} else if(mno == 4) {
		material = "SM570";
	} else if(mno == 5) {
		material = "SBHS400";
	} else if(mno == 6) {
		material = "SBHS500";
	}
	for(int i=0;i<this->size();i++) {
		AtdDbInputUseMaterialItem iumItem;
		getAt(i, iumItem);
		int mt = iumItem.getMt();		//MT
		if(mt != mno) {
			continue;
		}
		double t1 = iumItem.getT1();	//T1
		double t2 = iumItem.getT2();	//T2
		if(t1 <= fabs(itaatsu) && fabs(itaatsu) <= t2) {
			material = iumItem.getAmt();	//AMT
			break;
		}
	}

	return material;
}

