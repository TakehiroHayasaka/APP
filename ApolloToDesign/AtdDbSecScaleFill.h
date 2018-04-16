#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//断面･スケール及び文字高さと材質仕様･フィラープレート
class AtdDbSecScaleFill;
class AtdDbSecScaleFillItem;

class AtdDbSecScaleFill
{
public:
	AtdDbSecScaleFill(void) {}
	~AtdDbSecScaleFill(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbSecScaleFillItemList.size(); }
	void append(AtdDbSecScaleFillItem &atdDbSecScaleFillItem) { _atdDbSecScaleFillItemList.push_back(atdDbSecScaleFillItem); }
	JptErrorStatus getAt(int index, AtdDbSecScaleFillItem &atdDbSecScaleFillItem);

private:
	vector<AtdDbSecScaleFillItem> _atdDbSecScaleFillItemList;

};

class AtdDbSecScaleFillItem
{
public:
	AtdDbSecScaleFillItem(void) {}
	~AtdDbSecScaleFillItem(void) {}

	int &getMtfill() {return _mtfill;}
	const int &getMtfill() const {return _mtfill;}
	void setMtfill(const int &val) {_mtfill = val;}

	double &getRfilfu() {return _rfilfu;}
	const double &getRfilfu() const {return _rfilfu;}
	void setRfilfu(const double &val) {_rfilfu = val;}

	double &getRfilfl() {return _rfilfl;}
	const double &getRfilfl() const {return _rfilfl;}
	void setRfilfl(const double &val) {_rfilfl = val;}

	double &getRfilwb() {return _rfilwb;}
	const double &getRfilwb() const {return _rfilwb;}
	void setRfilwb(const double &val) {_rfilwb = val;}

private:
	int _mtfill;	//MTFILL Fill材質
	double _rfilfu;	//RFILFU
	double _rfilfl;	//RFILFL
	double _rfilwb;	//RFILWB

};

