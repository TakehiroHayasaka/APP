#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//断面･ソールプレート
class AtdDbSecSolePl;
class AtdDbSecSolePlItem;

class AtdDbSecSolePl
{
public:
	AtdDbSecSolePl(void) {}
	~AtdDbSecSolePl(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbSecSolePlItemList.size(); }
	void append(AtdDbSecSolePlItem &atdDbSecSolePlItem) { _atdDbSecSolePlItemList.push_back(atdDbSecSolePlItem); }
	JptErrorStatus getAt(int index, AtdDbSecSolePlItem &atdDbSecSolePlItem);

private:
	vector<AtdDbSecSolePlItem> _atdDbSecSolePlItemList;

};

class AtdDbSecSolePlItem
{
public:
	AtdDbSecSolePlItem(void) {}
	~AtdDbSecSolePlItem(void) {}

	int &getNogrd() {return _nogrd;}
	int &getNos() {return _nos;}
	double &getBs1() {return _bs1;}
	double &getRls1() {return _rls1;}

	const int &getNogrd() const {return _nogrd;}
	const int &getNos() const {return _nos;}
	const double &getBs1() const {return _bs1;}
	const double &getRls1() const {return _rls1;}

	void setNogrd(const int &val) {_nogrd = val;}
	void setNos(const int &val) {_nos = val;}
	void setBs1(const double &val) {_bs1 = val;}
	void setRls1(const double &val) {_rls1 = val;}

private:
	int _nogrd;		//NOGRD 桁名
	int _nos;		//NOS
	double _bs1;	//BS1 SOLE-PL幅
	double _rls1;	//RLS1 SOLE-PL長さ

};

