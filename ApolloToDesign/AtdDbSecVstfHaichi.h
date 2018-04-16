#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//断面･VSTF配置
class AtdDbSecVstfHaichi;
class AtdDbSecVstfHaichiItem;

class AtdDbSecVstfHaichi
{
public:
	AtdDbSecVstfHaichi(void) {}
	~AtdDbSecVstfHaichi(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbSecVstfHaichiItemList.size(); }
	void append(AtdDbSecVstfHaichiItem &atdDbSecVstfHaichiItem) { _atdDbSecVstfHaichiItemList.push_back(atdDbSecVstfHaichiItem); }
	JptErrorStatus getAt(int index, AtdDbSecVstfHaichiItem &atdDbSecVstfHaichiItem);
	int findId(const int& ig, const int& ip);

private:
	vector<AtdDbSecVstfHaichiItem> _atdDbSecVstfHaichiItemList;

};

class AtdDbSecVstfHaichiItem
{
public:
	AtdDbSecVstfHaichiItem(void) {}
	~AtdDbSecVstfHaichiItem(void) {}

	int &getNogrd() {return _nogrd;}
	int &getNopnl() {return _nopnl;}
	int &getNvst() {return _nvst;}
	int &getNovst() {return _novst;}

	const int &getNogrd() const {return _nogrd;}
	const int &getNopnl() const {return _nopnl;}
	const int &getNvst() const {return _nvst;}
	const int &getNovst() const {return _novst;}

	void setNogrd(const int &val) {_nogrd = val;}
	void setNopnl(const int &val) {_nopnl = val;}
	void setNvst(const int &val) {_nvst = val;}
	void setNovst(const int &val) {_novst = val;}

private:
	int _nogrd;	//NOGRD 桁名
	int _nopnl;	//NOPNL 始側格点名 終側格点名
	int _nvst;	//NVST 中間点番号
	int _novst;	//NOVST 識別ID

};

