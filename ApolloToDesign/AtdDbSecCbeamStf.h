#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//ífñ •â°åÖ•ï‚çÑçﬁífñ êî
class AtdDbSecCbeamStf;
class AtdDbSecCbeamStfItem;

class AtdDbSecCbeamStf
{
public:
	AtdDbSecCbeamStf(void) {}
	~AtdDbSecCbeamStf(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbSecCbeamStfItemList.size(); }
	void append(AtdDbSecCbeamStfItem &atdDbSecCbeamStfItem) { _atdDbSecCbeamStfItemList.push_back(atdDbSecCbeamStfItem); }
	JptErrorStatus getAt(int index, AtdDbSecCbeamStfItem &atdDbSecCbeamStfItem);

private:
	vector<AtdDbSecCbeamStfItem> _atdDbSecCbeamStfItemList;

};

class AtdDbSecCbeamStfItem
{
public:
	AtdDbSecCbeamStfItem(void) {}
	~AtdDbSecCbeamStfItem(void) {}

	int &getNocrs() {return _nocrs;}
	int &getPosstf() {return _posstf;}
	double &getStcw() {return _stcw;}
	double &getStct() {return _stct;}
	int &getStcm() {return _stcm;}

	const int &getNocrs() const {return _nocrs;}
	const int &getPosstf() const {return _posstf;}
	const double &getStcw() const {return _stcw;}
	const double &getStct() const {return _stct;}
	const int &getStcm() const {return _stcm;}

	void setNocrs(const int &val) {_nocrs = val;}
	void setPosstf(const int &val) {_posstf = val;}
	void setStcw(const double &val) {_stcw = val;}
	void setStct(const double &val) {_stct = val;}
	void setStcm(const int &val) {_stcm = val;}

private:
	int _nocrs;		//NOCRS
	int _posstf;	//POSSTF
	double _stcw;	//STCW ïù
	double _stct;	//STCT î¬å˙
	int _stcm;		//STCM çﬁéø

};

