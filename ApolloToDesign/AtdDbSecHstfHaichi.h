#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//断面･HSTF配置
class AtdDbSecHstfHaichi;
class AtdDbSecHstfHaichiItem;

class AtdDbSecHstfHaichi
{
public:
	AtdDbSecHstfHaichi(void) {}
	~AtdDbSecHstfHaichi(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbSecHstfHaichiItemList.size(); }
	void append(AtdDbSecHstfHaichiItem &atdDbSecHstfHaichiItem) { _atdDbSecHstfHaichiItemList.push_back(atdDbSecHstfHaichiItem); }
	JptErrorStatus getAt(int index, AtdDbSecHstfHaichiItem& atdDbSecHstfHaichiItem);
	int find(const int& ig, const int& ip);

private:
	vector<AtdDbSecHstfHaichiItem> _atdDbSecHstfHaichiItemList;

};

class AtdDbSecHstfHaichiItem
{
public:
	AtdDbSecHstfHaichiItem(void) {}
	~AtdDbSecHstfHaichiItem(void) {}

	int &getNogrd() {return _nogrd;}
	int &getNopnl() {return _nopnl;}
	int &getNohsl1() {return _nohsl1;}
	int &getNohsl2() {return _nohsl2;}

	const int &getNogrd() const {return _nogrd;}
	const int &getNopnl() const {return _nopnl;}
	const int &getNohsl1() const {return _nohsl1;}
	const int &getNohsl2() const {return _nohsl2;}

	void setNogrd(const int &val) {_nogrd = val;}
	void setNopnl(const int &val) {_nopnl = val;}
	void setNohsl1(const int &val) {_nohsl1 = val;}
	void setNohsl2(const int &val) {_nohsl2 = val;}

private:
	int _nogrd;		//NOGRD 桁名
	int _nopnl;		//NOPNL 始側格点名 終側格点名 格点間番号
	int _nohsl1;	//NOHSL1 配置段番号
	int _nohsl2;	//NOHSL2 配置段番号

};

