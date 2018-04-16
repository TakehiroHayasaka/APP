#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//断面･主桁の断面長
class AtdDbSecGrdLeng;
class AtdDbSecGrdLengItem;

class AtdDbSecGrdLeng
{
public:
	AtdDbSecGrdLeng(void) {}
	~AtdDbSecGrdLeng(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbSecGrdLengItemList.size(); }
	void append(AtdDbSecGrdLengItem &atdDbSecGrdLengItem) { _atdDbSecGrdLengItemList.push_back(atdDbSecGrdLengItem); }
	JptErrorStatus getAt(int index, AtdDbSecGrdLengItem &atdDbSecGrdLengItem);

private:
	vector<AtdDbSecGrdLengItem> _atdDbSecGrdLengItemList;

};

class AtdDbSecGrdLengItem
{
public:
	AtdDbSecGrdLengItem(void) {}
	~AtdDbSecGrdLengItem(void) {}

	int &getNogrd() {return _nogrd;}
	int &getNosec() {return _nosec;}
	double &getRlsec() {return _rlsec;}

	const int &getNogrd() const {return _nogrd;}
	const int &getNosec() const {return _nosec;}
	const double &getRlsec() const {return _rlsec;}

	void setNogrd(const int &val) {_nogrd = val;}
	void setNosec(const int &val) {_nosec = val;}
	void setRlsec(const double &val) {_rlsec = val;}

private:
	int _nogrd;		//NOGRD 桁名
	int _nosec;		//NOSEC ジョイント名
	double _rlsec;	//RLSEC ブロック長

};

