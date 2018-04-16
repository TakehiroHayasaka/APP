#include "stdafx.h"
#include "AtdDbGrdVstfKyori.h"

JptErrorStatus AtdDbGrdVstfKyori::load(CDaoDatabase& dbFile)
{
	CDaoTableDefInfo tabledef;
	int nTableCount = dbFile.GetTableDefCount();
	bool bResult = false;
	for(int nCnt = 0; nCnt < nTableCount; nCnt++) {
		dbFile.GetTableDefInfo(nCnt, tabledef);
		if(tabledef.m_strName == "主桁中間垂直補剛材間隔データ") {
			bResult = true;
			break;
		}
	}
	if(bResult != true) {	//テーブルが存在しない場合
		return JPT_OK;
	}
	//
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [主桁中間垂直補剛材間隔データ]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int nostr = GetFieldValueInteger(rs, _T("部材線コード"));			//部材線コード
		int nopanel = GetFieldValueInteger(rs, _T("パネルコード"));			//パネルコード
		int vstfNum = GetFieldValueInteger(rs, _T("垂直補剛材間隔番号"));	//垂直補剛材間隔番号
		double vstfkyori = GetFieldValueDouble(rs, _T("垂直補剛材間隔"));	//垂直補剛材間隔
		AtdDbGrdVstfKyoriItem atdDbGrdVstfKyoriItem;
		atdDbGrdVstfKyoriItem.setNostr(nostr);
		atdDbGrdVstfKyoriItem.setNopanel(nopanel);
		atdDbGrdVstfKyoriItem.setVstfNum(vstfNum);
		atdDbGrdVstfKyoriItem.setVstfkyori(vstfkyori);
		this->append(atdDbGrdVstfKyoriItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbGrdVstfKyori::getAt(int index, AtdDbGrdVstfKyoriItem& atdDbGrdVstfKyoriItem)
{
	if(_atdDbGrdVstfKyoriItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbGrdVstfKyoriItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbGrdVstfKyoriItem = _atdDbGrdVstfKyoriItemList[index];

	return JPT_OK;
}

