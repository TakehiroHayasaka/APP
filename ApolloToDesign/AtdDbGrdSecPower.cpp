#include "stdafx.h"
#include "AtdDbGrdSecPower.h"

JptErrorStatus AtdDbGrdSecPower::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [主桁断面力データ]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int buzaiCode = GetFieldValueInteger(rs, _T("部材線コード"));	//部材線コード
		int secCode = GetFieldValueInteger(rs, _T("断面コード"));	//断面コード
		int shosaPosNo = GetFieldValueInteger(rs, _T("照査位置番号"));	//照査位置番号
		int secCaseNo = GetFieldValueInteger(rs, _T("断面力ケース番号"));	//断面力ケース番号
		double mageMx = GetFieldValueDouble(rs, _T("曲げモーメント（面内）Ｍｘ"));	//曲げモーメント（面内）Ｍｘ
		AtdDbGrdSecPowerItem atdDbGrdSecPowerItem;
		atdDbGrdSecPowerItem.setBuzaiCode(buzaiCode);
		atdDbGrdSecPowerItem.setSecCode(secCode);
		atdDbGrdSecPowerItem.ShosaPosNo(shosaPosNo);
		atdDbGrdSecPowerItem.SecCaseNo(secCaseNo);
		atdDbGrdSecPowerItem.setMageMx(mageMx);
		this->append(atdDbGrdSecPowerItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbGrdSecPower::getAt(int index, AtdDbGrdSecPowerItem& atdDbGrdSecPowerItem)
{
	if(_atdDbGrdSecPowerItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbGrdSecPowerItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbGrdSecPowerItem = _atdDbGrdSecPowerItemList[index];

	return JPT_OK;
}

double AtdDbGrdSecPower::findMageMx(const int& shosaPosNo, const int& secCaseNo)
{
	double mageMx = 0.0;
	for(int i=0;i<this->size();i++) {
		AtdDbGrdSecPowerItem gspItem;
		getAt(i, gspItem);
		int shosaPosNo2 = gspItem.getShosaPosNo();	//照査位置番号
		int secCaseNo2 = gspItem.getSecCaseNo();	//断面力ケース番号
		if(shosaPosNo2 == shosaPosNo && secCaseNo2 == secCaseNo) {
			mageMx = gspItem.getMageMx();	//曲げモーメント（面内）Ｍｘ
			break;
		}
	}

	return mageMx;
}

