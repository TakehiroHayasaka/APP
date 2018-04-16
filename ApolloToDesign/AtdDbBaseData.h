// 2018/02/26 take Add Start
#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector>

//橋梁設計基準
class AtdDbBaseData;
class AtdDbBaseDataItem;

class AtdDbBaseData
{
public:
	AtdDbBaseData() {}
	~AtdDbBaseData() {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbBaseDataItemList.size(); }
	void append(AtdDbBaseDataItem &atdDbBaseDataItem) { _atdDbBaseDataItemList.push_back(atdDbBaseDataItem); }
	JptErrorStatus getAt(int index, AtdDbBaseDataItem &atdDbBaseDataItem);
	JptErrorStatus judgeBrdgType(bool &brdgTypeFlg);

private:
	std::vector<AtdDbBaseDataItem> _atdDbBaseDataItemList;

};

class AtdDbBaseDataItem
{
public:
	AtdDbBaseDataItem() {}
	~AtdDbBaseDataItem() {}

	int &getBrdgType() {return _brdgType;}
	int &getCrossType() {return _crossType;}

	const int &getBrdgType() const {return _brdgType;}
	const int &getCrossType() const {return _crossType;}

	void setBrdgType(const int &brdgType) {_brdgType = brdgType;}
	void setCrossType(const int &crossType) {_crossType = crossType;}

private:
	int _brdgType;		//橋梁形式（1：RC鈑桁,2：RC箱桁,3：鋼床版鈑桁,4：鋼床版箱桁）
	int _crossType;		//横組構造形式（0：標準形式,1：少主桁形式）
};
// 2018/02/26 take Add End