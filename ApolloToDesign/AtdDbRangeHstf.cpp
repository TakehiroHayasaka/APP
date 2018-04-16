// 2018/02/28 take Add Start
#include "stdafx.h"
#include "AtdDbRangeHstf.h"

JptErrorStatus AtdDbRangeHstf::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [’f–Ê¥…•½•â„Þ‚Ì“ü‚é”ÍˆÍ(’Ç‰Á‹——£)]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int noGrd = GetFieldValueInteger(rs, _T("NOGRD"));
		int uplo = GetFieldValueInteger(rs, _T("UPLO"));
		int noHstf = GetFieldValueInteger(rs, _T("NOHSTF"));
		int nChang = GetFieldValueInteger(rs, _T("NCHANG"));
		double tLen1 = GetFieldValueDouble(rs, _T("TLEN1"));
		double tLen2 = GetFieldValueDouble(rs, _T("TLEN2"));
		AtdDbRangeHstfItem atdDbRangeHstfItem;
		atdDbRangeHstfItem.setNogrd(noGrd);
		atdDbRangeHstfItem.setUplo(uplo);
		atdDbRangeHstfItem.setNoHstf(noHstf);
		atdDbRangeHstfItem.setNChang(nChang);
		atdDbRangeHstfItem.setTLen1(tLen1);
		atdDbRangeHstfItem.setTLen2(tLen2);
		this->append(atdDbRangeHstfItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbRangeHstf::getAt(int index, AtdDbRangeHstfItem& atdDbRangeHstfItem)
{
	if(_atdDbRangeHstfItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbRangeHstfItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbRangeHstfItem = _atdDbRangeHstfItemList[index];

	return JPT_OK;
}
// 2018/02/28 take Add End