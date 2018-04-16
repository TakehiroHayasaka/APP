#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//ç\ê¨•ëSëÃ
class AtdDbStructAll;
class AtdDbStructAllItem;

class AtdDbStructAll
{
public:
	AtdDbStructAll(void) {}
	~AtdDbStructAll(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbStructAllItemList.size(); }
	void append(AtdDbStructAllItem &atdDbStructAllItem) { _atdDbStructAllItemList.push_back(atdDbStructAllItem); }
	JptErrorStatus getAt(int index, AtdDbStructAllItem &atdDbStructAllItem);

private:
	vector<AtdDbStructAllItem> _atdDbStructAllItemList;

};

class AtdDbStructAllItem
{
public:
	AtdDbStructAllItem(void) {}
	~AtdDbStructAllItem(void) {}

	int &getNspan() {return _nspan;}
	const int &getNspan() const {return _nspan;}
	void setNspan(const int &val) {_nspan = val;}

private:
	int _nspan;	//NSPAN

};

