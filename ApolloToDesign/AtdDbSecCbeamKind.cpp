#include "stdafx.h"
#include "AtdDbSecCbeamKind.h"

JptErrorStatus AtdDbSecCbeamKind::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [’f–Ê¥‰¡Œ…Ží—Þ”]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int necrs = GetFieldValueInteger(rs, _T("NECRS"));	//
		int nmcrs = GetFieldValueInteger(rs, _T("NMCRS"));	//
		int nicrs = GetFieldValueInteger(rs, _T("NICRS"));	//
		this->setNecrs(necrs);
		this->setNmcrs(nmcrs);
		this->setNicrs(nicrs);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

