#include "stdafx.h"
#include "AtdDbSecCbeam.h"

JptErrorStatus AtdDbSecCbeam::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [断面･横桁]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int nocrs = GetFieldValueInteger(rs, _T("NOCRS"));		//NOCRS
		double h3cl = GetFieldValueDouble(rs, _T("H3CL"));		//H鋼ウェブ高
		double h3cr = GetFieldValueDouble(rs, _T("H3CR"));		//H鋼ウェブ高
		double h1cl = GetFieldValueDouble(rs, _T("H1CL"));		//LU LC
		double h1cr = GetFieldValueDouble(rs, _T("H1CR"));		//LD RC
		double h2cl = GetFieldValueDouble(rs, _T("H2CL"));		//RU
		double h2cr = GetFieldValueDouble(rs, _T("H2CR"));		//RD
		double dspll = GetFieldValueDouble(rs, _T("DSPLL"));	//LJ
		double dsplr = GetFieldValueDouble(rs, _T("DSPLR"));	//RJ
		int nocjul = GetFieldValueInteger(rs, _T("NOCJUL"));	//上フランジ左側の添接番号
		int nocjll = GetFieldValueInteger(rs, _T("NOCJLL"));	//下フランジ左側の添接番号
		int nocjwl = GetFieldValueInteger(rs, _T("NOCJWL"));	//ウェブ左側の添接番号
		int nocjur = GetFieldValueInteger(rs, _T("NOCJUR"));	//上フランジ右側の添接番号
		int nocjlr = GetFieldValueInteger(rs, _T("NOCJLR"));	//下フランジ右側の添接番号
		int nocjwr = GetFieldValueInteger(rs, _T("NOCJWR"));	//ウェブ右側の添接番号
		AtdDbSecCbeamItem atdDbSecCbeamItem;
		atdDbSecCbeamItem.setNocrs(nocrs);
		atdDbSecCbeamItem.setH3cl(h3cl);
		atdDbSecCbeamItem.setH3cr(h3cr);
		atdDbSecCbeamItem.setH1cl(h1cl);
		atdDbSecCbeamItem.setH1cr(h1cr);
		atdDbSecCbeamItem.setH2cl(h2cl);
		atdDbSecCbeamItem.setH2cr(h2cr);
		atdDbSecCbeamItem.setDspll(dspll);
		atdDbSecCbeamItem.setDsplr(dsplr);
		atdDbSecCbeamItem.setNocjul(nocjul);
		atdDbSecCbeamItem.setNocjll(nocjll);
		atdDbSecCbeamItem.setNocjwl(nocjwl);
		atdDbSecCbeamItem.setNocjur(nocjur);
		atdDbSecCbeamItem.setNocjlr(nocjlr);
		atdDbSecCbeamItem.setNocjwr(nocjwr);
		this->append(atdDbSecCbeamItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbSecCbeam::getAt(int index, AtdDbSecCbeamItem& atdDbSecCbeamItem)
{
	if(_atdDbSecCbeamItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbSecCbeamItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbSecCbeamItem = _atdDbSecCbeamItemList[index];

	return JPT_OK;
}

int AtdDbSecCbeam::find(const int& nocrs)
{
	for(int i=0;i<this->size();i++) {
		AtdDbSecCbeamItem scbItem;
		getAt(i, scbItem);
		int nocrs2 = scbItem.getNocrs();		//NOCRS
		if(nocrs2 == nocrs) {
			return i;
			break;
		}
	}

	return -1;
}

double AtdDbSecCbeam::findWebHeight(const int& nocrs)
{
	double height = 0.0;
	for(int i=0;i<this->size();i++) {
		AtdDbSecCbeamItem scbItem;
		getAt(i, scbItem);
		int nocrs2 = scbItem.getNocrs();		//NOCRS
		if(nocrs2 == nocrs) {
			double h3cl = scbItem.getH3cl();	//H3CL H鋼ウェブ高
			double h3cr = scbItem.getH3cr();	//H3CR H鋼ウェブ高
			height = (h3cl+h3cr) / 2;
			break;
		}
	}

	return height;
}

