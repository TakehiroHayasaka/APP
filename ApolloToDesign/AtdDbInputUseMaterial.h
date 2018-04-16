#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//入力・使用材料データ
class AtdDbInputUseMaterial;
class AtdDbInputUseMaterialItem;

class AtdDbInputUseMaterial
{
public:
	AtdDbInputUseMaterial(void) {}
	~AtdDbInputUseMaterial(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbInputUseMaterialItemList.size(); }
	void append(AtdDbInputUseMaterialItem &atdDbInputUseMaterialItem) { _atdDbInputUseMaterialItemList.push_back(atdDbInputUseMaterialItem); }
	JptErrorStatus getAt(int index, AtdDbInputUseMaterialItem &atdDbInputUseMaterialItem);
	string findMaterial(const double& itaatsu, const int& mno);

private:
	vector<AtdDbInputUseMaterialItem> _atdDbInputUseMaterialItemList;

};

class AtdDbInputUseMaterialItem
{
public:
	AtdDbInputUseMaterialItem(void) {}
	~AtdDbInputUseMaterialItem(void) {}

	int &getMt() {return _mt;}
	double &getT1() {return _t1;}
	double &getT2() {return _t2;}
	CString &getAmt() {return _amt;}

	const int &getMt() const {return _mt;}
	const double &getT1() const {return _t1;}
	const double &getT2() const {return _t2;}
	const CString &getAmt() const {return _amt;}

	void setMt(const int &val) {_mt = val;}
	void setT1(const double &val) {_t1 = val;}
	void setT2(const double &val) {_t2 = val;}
	void setAmt(const CString &val) {_amt = val;}

private:
	int _mt;		//MT
	double _t1;		//T1
	double _t2;		//T2
	CString _amt;	//AMT

};

