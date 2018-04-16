#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//入力・横桁コネクション添接
class AtdDbInputCbeamConnSpl;
class AtdDbInputCbeamConnSplItem;

class AtdDbInputCbeamConnSpl
{
public:
	AtdDbInputCbeamConnSpl(void) {}
	~AtdDbInputCbeamConnSpl(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbInputCbeamConnSplItemList.size(); }
	void append(AtdDbInputCbeamConnSplItem &atdDbInputCbeamConnSplItem) { _atdDbInputCbeamConnSplItemList.push_back(atdDbInputCbeamConnSplItem); }
	JptErrorStatus getAt(int index, AtdDbInputCbeamConnSplItem &atdDbInputCbeamConnSplItem);
	int find(const int& nosec, const int& iposul);

private:
	vector<AtdDbInputCbeamConnSplItem> _atdDbInputCbeamConnSplItemList;

};

class AtdDbInputCbeamConnSplItem
{
public:
	AtdDbInputCbeamConnSplItem(void) {}
	~AtdDbInputCbeamConnSplItem(void) {}

	int &getNosec() {return _nosec;}
	int &getIposul() {return _iposul;}
	int &getJtype() {return _jtype;}
	double &getEdgec() {return _edgec;}
	double &getGagec() {return _gagec;}
	double &getGagecenc() {return _gagecenc;}

	const int &getNosec() const {return _nosec;}
	const int &getIposul() const {return _iposul;}
	const int &getJtype() const {return _jtype;}
	const double &getEdgec() const {return _edgec;}
	const double &getGagec() const {return _gagec;}
	const double &getGagecenc() const {return _gagecenc;}

	void setNosec(const int &val) {_nosec = val;}
	void setIposul(const int &val) {_iposul = val;}
	void setJtype(const int &val) {_jtype = val;}
	void setEdgec(const double &val) {_edgec = val;}
	void setGagec(const double &val) {_gagec = val;}
	void setGagecenc(const double &val) {_gagecenc = val;}

private:
	int _nosec;		//NOSEC
	int _iposul;	//IPOSUL
	int _jtype;		//JTYPE
	double _edgec;		//EDGEC ConnPL連結外側縁端
	double _gagec;		//GAGEC ConnPLゲージ
	double _gagecenc;	//GAGECENC ConnPL中心ゲージ

};

