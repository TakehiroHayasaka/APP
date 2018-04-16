#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

 //入力･フィラープレートオプション
class AtdDbInputFillOption;
class AtdDbInputFillOptionItem;

class AtdDbInputFillOption
{
public:
	AtdDbInputFillOption(void) {}
	~AtdDbInputFillOption(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbInputFillOptionItemList.size(); }
	void append(AtdDbInputFillOptionItem &atdDbInputFillOptionItem) { _atdDbInputFillOptionItemList.push_back(atdDbInputFillOptionItem); }
	JptErrorStatus getAt(int index, AtdDbInputFillOptionItem &atdDbInputFillOptionItem);

private:
	vector<AtdDbInputFillOptionItem> _atdDbInputFillOptionItemList;

};

class AtdDbInputFillOptionItem
{
public:
	AtdDbInputFillOptionItem(void) {}
	~AtdDbInputFillOptionItem(void) {}

	int &getIfilfu() {return _ifilfu;}
	const int &getIfilfu() const {return _ifilfu;}
	void setIfilfu(const int &val) {_ifilfu = val;}

	int &getIfilfl() {return _ifilfl;}
	const int &getIfilfl() const {return _ifilfl;}
	void setIfilfl(const int &val) {_ifilfl = val;}

	int &getIfilwb() {return _ifilwb;}
	const int &getIfilwb() const {return _ifilwb;}
	void setIfilwb(const int &val) {_ifilwb = val;}

private:
	int _ifilfu;	//IFILFU
	int _ifilfl;	//IFILFL
	int _ifilwb;	//IFILWB

};

