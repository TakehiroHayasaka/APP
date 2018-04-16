// 2018/03/01 take Add Start
#include "stdafx.h"
#include "AtdDbGrdPower.h"

JptErrorStatus AtdDbGrdPower::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [主桁応力度データ]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int buzaiCode = GetFieldValueInteger(rs, _T("部材線コード"));	//部材線コード
		int secCode = GetFieldValueInteger(rs, _T("断面コード"));		//断面コード
		int shosaPosNo = GetFieldValueInteger(rs, _T("照査位置番号"));	//照査位置番号
		int secCaseNo = GetFieldValueInteger(rs, _T("断面力ケース番号"));//断面力ケース番号
		int shosaPoiNo = GetFieldValueInteger(rs, _T("照査点番号(断面内)"));	//照査点番号
		double forceMx = GetFieldValueDouble(rs, _T("Ｍｘに対する応力度"));//Ｍｘに対する応力度

		AtdDbGrdPowerItem atdDbGrdPowerItem;
		atdDbGrdPowerItem.setBuzaiCode(buzaiCode);
		atdDbGrdPowerItem.setSecCode(secCode);
		atdDbGrdPowerItem.setShosaPosNo(shosaPosNo);
		atdDbGrdPowerItem.setSecCaseNo(secCaseNo);
		atdDbGrdPowerItem.setShosaPoiNo(shosaPoiNo);
		atdDbGrdPowerItem.setForceMx(forceMx);
		this->append(atdDbGrdPowerItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbGrdPower::getAt(int index, AtdDbGrdPowerItem& atdDbGrdPowerItem)
{
	if(_atdDbGrdPowerItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbGrdPowerItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbGrdPowerItem = _atdDbGrdPowerItemList[index];

	return JPT_OK;
}

double AtdDbGrdPower::findForceMx(const int& grdNo, const int& panelNo)
{
	double forceMx = 0.0;
	for(int i=0;i<this->size();i++) {
		AtdDbGrdPowerItem gpItem;
		getAt(i, gpItem);
		int buzaiCode = gpItem.getBuzaiCode();		//部材線コード
		int secNo = gpItem.getSecCode() % 1000;		//断面番号
		if( grdNo == buzaiCode && panelNo == secNo ){	//桁番号とパネルが一致するか
			int shosaPosNo = gpItem.getShosaPosNo();	//照査位置番号
			int secCaseNo = gpItem.getSecCaseNo();		//断面力ケース番号
			int shosaPoiNo = gpItem.getShosaPoiNo();	//照査点番号
			if( shosaPosNo == 3 && secCaseNo == 1 && shosaPoiNo == 4 ){	//照査位置番号(=横桁取付点)、断面力ケース番号(最大時)、照査点番号(=下フランジ面)
				forceMx = gpItem.getForceMx();			//Ｍｘに対する応力度
				break;
			}
		}
	}
	return forceMx;
}
// 2018/03/01 take Add End