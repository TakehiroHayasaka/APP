#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//“ü—Í¥Œ…’[’·
class AtdDbInputKetatanLeng;
class AtdDbInputKetatanLengItem;

class AtdDbInputKetatanLeng
{
public:
	AtdDbInputKetatanLeng(void) {}
	~AtdDbInputKetatanLeng(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbInputKetatanLengItemList.size(); }
	void append(AtdDbInputKetatanLengItem &atdDbInputKetatanLengItem) { _atdDbInputKetatanLengItemList.push_back(atdDbInputKetatanLengItem); }
	JptErrorStatus getAt(int index, AtdDbInputKetatanLengItem &atdDbInputKetatanLengItem);
	int find(const int& ig);

private:
	vector<AtdDbInputKetatanLengItem> _atdDbInputKetatanLengItemList;

};

class AtdDbInputKetatanLengItem
{
public:
	AtdDbInputKetatanLengItem(void) {}
	~AtdDbInputKetatanLengItem(void) {}

	int &getStrno() {return _strno;}
	double &getRls() {return _rls;}
	double &getRle() {return _rle;}

	const int &getStrno() const {return _strno;}
	const double &getRls() const {return _rls;}
	const double &getRle() const {return _rle;}

	void setStrno(const int &val) {_strno = val;}
	void setRls(const double &val) {_rls = val;}
	void setRle(const double &val) {_rle = val;}

private:
	int _strno;	//STRNO
	double _rls;	//RLS
	double _rle;	//RLE

};

