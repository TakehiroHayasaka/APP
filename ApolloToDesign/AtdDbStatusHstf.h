// 2018/02/28 take Add Start
#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector>

//断面･HSTFの位置関係(段数変化長さ･段数)
class AtdDbStatusHstf;
class AtdDbStatusHstfItem;

class AtdDbStatusHstf
{
public:
	AtdDbStatusHstf() {}
	~AtdDbStatusHstf() {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() {return (int)_atdDbStatusHstfItemList.size();}
	void append(AtdDbStatusHstfItem &atdDbStatusHstfItem) {_atdDbStatusHstfItemList.push_back(atdDbStatusHstfItem);}
	JptErrorStatus getAt(int index, AtdDbStatusHstfItem &atdDbStatusHstfItem);

private:
	std::vector<AtdDbStatusHstfItem> _atdDbStatusHstfItemList;
};

class AtdDbStatusHstfItem
{
public:
	AtdDbStatusHstfItem() {}
	~AtdDbStatusHstfItem() {}

	int &getNoGrd() {return _noGrd;}
	int &getRlsfh() {return _rlsfh;}
	int &getNsfh() {return _nsfh;}

	const int &getNoGrd() const {return _noGrd;}
	const int &getRlsfh() const {return _rlsfh;}
	const int &getNsfh() const {return _nsfh;}

	void setNoGrd(const int &noGrd) {_noGrd = noGrd;}
	void setRlsfh(const int &rlsfh) {_rlsfh = rlsfh;}
	void setNsfh(const int &nsfh) {_nsfh = nsfh;}

private:
	int _noGrd;		//主桁番号
	int _rlsfh;		//段数変化長
	int _nsfh;		//段数
};
// 2018/02/28 take Add End