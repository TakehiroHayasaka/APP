#include "stdafx.h"
#include "AtdDbLineGrdPanel.h"

JptErrorStatus AtdDbLineGrdPanel::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [線形･主桁(横桁/ブラケット)･パネル長]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int strcode = GetFieldValueInteger(rs, _T("STRCODE"));	//
		if(strcode == 3 || strcode == 4) {
			int nostr = GetFieldValueInteger(rs, _T("NOSTR"));	//
			int nopnl = GetFieldValueInteger(rs, _T("NOPNL"));	//
			double rlp = GetFieldValueDouble(rs, _T("RLP"));	//
			AtdDbLineGrdPanelItem atdDbLineGrdPanelItem;
			atdDbLineGrdPanelItem.setStrcode(strcode);
			atdDbLineGrdPanelItem.setNostr(nostr);
			atdDbLineGrdPanelItem.setNopnl(nopnl);
			atdDbLineGrdPanelItem.setRlp(rlp);
			this->append(atdDbLineGrdPanelItem);
		}
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbLineGrdPanel::getAt(int index, AtdDbLineGrdPanelItem& atdDbLineGrdPanelItem)
{
	if(_atdDbLineGrdPanelItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbLineGrdPanelItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbLineGrdPanelItem = _atdDbLineGrdPanelItemList[index];

	return JPT_OK;
}

double AtdDbLineGrdPanel::findRlp(const int& strcode, const int& nostr, const int& nopnl)
{
	double rlp = 0.0;
	for(int i=0;i<this->size();i++) {
		AtdDbLineGrdPanelItem lgpItem;
		getAt(i, lgpItem);
		int strcode2 = lgpItem.getStrcode();		//
		int nostr2 = lgpItem.getNostr();		//
		int nopnl2 = lgpItem.getNopnl();		//
		if(strcode2 == strcode && nostr2 == nostr && nopnl2 == nopnl) {
			rlp = lgpItem.getRlp();	//
			break;
		}
	}

	return rlp;
}

