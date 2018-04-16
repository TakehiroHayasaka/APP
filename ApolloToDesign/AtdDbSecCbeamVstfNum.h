#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//ífñ •â°åÖ•ÉpÉlÉãñàÇÃVSTFñ{êî
class AtdDbSecCbeamVstfNum;
class AtdDbSecCbeamVstfNumItem;

class AtdDbSecCbeamVstfNum
{
public:
	AtdDbSecCbeamVstfNum(void) {}
	~AtdDbSecCbeamVstfNum(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbSecCbeamVstfNumItemList.size(); }
	void append(AtdDbSecCbeamVstfNumItem &atdDbSecCbeamVstfNumItem) { _atdDbSecCbeamVstfNumItemList.push_back(atdDbSecCbeamVstfNumItem); }
	JptErrorStatus getAt(int index, AtdDbSecCbeamVstfNumItem &atdDbSecCbeamVstfNumItem);
	int findNvstc(const int& nocrs);

private:
	vector<AtdDbSecCbeamVstfNumItem> _atdDbSecCbeamVstfNumItemList;

};

class AtdDbSecCbeamVstfNumItem
{
public:
	AtdDbSecCbeamVstfNumItem(void) {}
	~AtdDbSecCbeamVstfNumItem(void) {}

	int &getNocrs() {return _nocrs;}
	const int &getNocrs() const {return _nocrs;}
	void setNocrs(const int &val) {_nocrs = val;}

	int &getNvstc() {return _nvstc;}
	const int &getNvstc() const {return _nvstc;}
	void setNvstc(const int &val) {_nvstc = val;}

private:
	int _nocrs;	//NOCRS
	int _nvstc;	//NVSTC

};

