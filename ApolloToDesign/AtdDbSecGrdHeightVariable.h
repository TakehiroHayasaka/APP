#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//ífñ •éÂåÖï†î¬(åÖçÇíÜêSâ¬ïœ)
class AtdDbSecGrdHeightVariable;
class AtdDbSecGrdHeightVariableItem;

class AtdDbSecGrdHeightVariable
{
public:
	AtdDbSecGrdHeightVariable(void) {}
	~AtdDbSecGrdHeightVariable(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbSecGrdHeightVariableItemList.size(); }
	void append(AtdDbSecGrdHeightVariableItem &atdDbSecGrdHeightVariableItem) { _atdDbSecGrdHeightVariableItemList.push_back(atdDbSecGrdHeightVariableItem); }
	JptErrorStatus getAt(int index, AtdDbSecGrdHeightVariableItem &atdDbSecGrdHeightVariableItem);

private:
	vector<AtdDbSecGrdHeightVariableItem> _atdDbSecGrdHeightVariableItemList;

};

class AtdDbSecGrdHeightVariableItem
{
public:
	AtdDbSecGrdHeightVariableItem(void) {}
	~AtdDbSecGrdHeightVariableItem(void) {}

	int &getNogrd() {return _nogrd;}
	double &getRcweb() {return _rcweb;}
	double &getHcweb() {return _hcweb;}
	int &getItplc() {return _itplc;}
	int &getItphc() {return _itphc;}

	const int &getNogrd() const {return _nogrd;}
	const double &getRcweb() const {return _rcweb;}
	const double &getHcweb() const {return _hcweb;}
	const int &getItplc() const {return _itplc;}
	const int &getItphc() const {return _itphc;}

	void setNogrd(const int &val) {_nogrd = val;}
	void setRcweb(const double &val) {_rcweb = val;}
	void setHcweb(const double &val) {_hcweb = val;}
	void setItplc(const int &val) {_itplc = val;}
	void setItphc(const int &val) {_itphc = val;}

private:
	int _nogrd;		//NOGRD
	double _rcweb;	//RCWEB
	double _hcweb;	//HCWEB
	int _itplc;		//ITPLC
	int _itphc;		//ITPHC

};

