#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//断面･主桁腹板(左右腹板高一定)
class AtdDbSecGrdHeightConstant;
class AtdDbSecGrdHeightConstantItem;

class AtdDbSecGrdHeightConstant
{
public:
	AtdDbSecGrdHeightConstant(void) {}
	~AtdDbSecGrdHeightConstant(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbSecGrdHeightConstantItemList.size(); }
	void append(AtdDbSecGrdHeightConstantItem &atdDbSecGrdHeightConstantItem) { _atdDbSecGrdHeightConstantItemList.push_back(atdDbSecGrdHeightConstantItem); }
	JptErrorStatus getAt(int index, AtdDbSecGrdHeightConstantItem &atdDbSecGrdHeightConstantItem);

private:
	vector<AtdDbSecGrdHeightConstantItem> _atdDbSecGrdHeightConstantItemList;

};

class AtdDbSecGrdHeightConstantItem
{
public:
	AtdDbSecGrdHeightConstantItem(void) {}
	~AtdDbSecGrdHeightConstantItem(void) {}

	int &getNogrd() {return _nogrd;}
	double &getHweb() {return _hweb;}

	const int &getNogrd() const {return _nogrd;}
	const double &getHweb() const {return _hweb;}

	void setNogrd(const int &val) {_nogrd = val;}
	void setHweb(const double &val) {_hweb = val;}

private:
	int _nogrd;		//NOGRD
	double _hweb;	//HWEB ウェブ高

};

