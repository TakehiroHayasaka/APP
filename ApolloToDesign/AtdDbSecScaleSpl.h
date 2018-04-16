#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//fÊ¥XP[yÑ¶³ÆÞ¿dl¥åYÚÖW
class AtdDbSecScaleSpl;
class AtdDbSecScaleSplItem;

class AtdDbSecScaleSpl
{
public:
	AtdDbSecScaleSpl(void) {}
	~AtdDbSecScaleSpl(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbSecScaleSplItemList.size(); }
	void append(AtdDbSecScaleSplItem &atdDbSecScaleSplItem) { _atdDbSecScaleSplItemList.push_back(atdDbSecScaleSplItem); }
	JptErrorStatus getAt(int index, AtdDbSecScaleSplItem &atdDbSecScaleSplItem);

private:
	vector<AtdDbSecScaleSplItem> _atdDbSecScaleSplItemList;

};

class AtdDbSecScaleSplItem
{
public:
	AtdDbSecScaleSplItem(void) {}
	~AtdDbSecScaleSplItem(void) {}

	int &getIbuuti() {return _ibuuti;}
	int &getIbluti() {return _ibluti;}

	const int &getIbuuti() const {return _ibuuti;}
	const int &getIbluti() const {return _ibluti;}

	void setIbuuti(const int &val) {_ibuuti = val;}
	void setIbluti(const int &val) {_ibluti = val;}

private:
	int _ibuuti;	//IBUUTI UFLGÂú¦°ûü
	int _ibluti;	//IBLUTI LFLGÂú¦°ûü

};

