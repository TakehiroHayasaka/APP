#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//ìYê⁄•ê¸å`ìYê⁄ã§í ÉfÅ[É^
class AtdDbSplCommon;
class AtdDbSplCommonItem;

class AtdDbSplCommon
{
public:
	AtdDbSplCommon(void) {}
	~AtdDbSplCommon(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbSplCommonItemList.size(); }
	void append(AtdDbSplCommonItem &atdDbSplCommonItem) { _atdDbSplCommonItemList.push_back(atdDbSplCommonItem); }
	JptErrorStatus getAt(int index, AtdDbSplCommonItem &atdDbSplCommonItem);
	double findHoleSize(const int& strcode);

private:
	vector<AtdDbSplCommonItem> _atdDbSplCommonItemList;

};

class AtdDbSplCommonItem
{
public:
	AtdDbSplCommonItem(void) {}
	~AtdDbSplCommonItem(void) {}

	int &getStrcode() {return _strcode;}
	double &getPhsg() {return _phsg;}

	const int &getStrcode() const {return _strcode;}
	const double &getPhsg() const {return _phsg;}

	void setStrcode(const int &val) {_strcode = val;}
	void setPhsg(const double &val) {_phsg = val;}

private:
	int _strcode;	//STRCODE
	double _phsg;	//PHSG çEåa

};

