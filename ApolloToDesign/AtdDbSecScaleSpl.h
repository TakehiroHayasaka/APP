#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//断面･スケール及び文字高さと材質仕様･主桁添接関係
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
	int _ibuuti;	//IBUUTI UFLG板厚逃げ方向
	int _ibluti;	//IBLUTI LFLG板厚逃げ方向

};

