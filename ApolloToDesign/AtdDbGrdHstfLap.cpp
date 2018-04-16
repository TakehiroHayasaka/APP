#include "stdafx.h"
#include "AtdDbGrdHstfLap.h"

JptErrorStatus AtdDbGrdHstfLap::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [主桁水平補剛材ラップ範囲]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int buzaiCode = GetFieldValueInteger(rs, _T("部材線コード"));		//部材線コード
		int hstfPos = GetFieldValueInteger2(rs, _T("水平補剛材取付位置"));		//水平補剛材取付位置
		int hstfCode = GetFieldValueInteger(rs, _T("水平補剛材断面コード"));	//水平補剛材断面コード
		int hstfNum = GetFieldValueInteger(rs, _T("水平補剛材段数"));			//水平補剛材段数
		AtdDbGrdHstfLapItem atdDbGrdHstfLapItem;
		atdDbGrdHstfLapItem.setBuzaiCode(buzaiCode);
		atdDbGrdHstfLapItem.setHstfPos(hstfPos);
		atdDbGrdHstfLapItem.setHstfCode(hstfCode);
		atdDbGrdHstfLapItem.setHstfNum(hstfNum);
		this->append(atdDbGrdHstfLapItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbGrdHstfLap::getAt(int index, AtdDbGrdHstfLapItem& atdDbGrdHstfLapItem)
{
	if(_atdDbGrdHstfLapItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbGrdHstfLapItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbGrdHstfLapItem = _atdDbGrdHstfLapItemList[index];

	return JPT_OK;
}

