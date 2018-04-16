#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//“ü—Í¥‰¡Œ…
class AtdDbInputCbeam;
class AtdDbInputCbeamItem;

class AtdDbInputCbeam
{
public:
	AtdDbInputCbeam(void) {}
	~AtdDbInputCbeam(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbInputCbeamItemList.size(); }
	void append(AtdDbInputCbeamItem &atdDbInputCbeamItem) { _atdDbInputCbeamItemList.push_back(atdDbInputCbeamItem); }
	JptErrorStatus getAt(int index, AtdDbInputCbeamItem &atdDbInputCbeamItem);

private:
	vector<AtdDbInputCbeamItem> _atdDbInputCbeamItemList;

};

class AtdDbInputCbeamItem
{
public:
	AtdDbInputCbeamItem(void) {}
	~AtdDbInputCbeamItem(void) {}

	int &getIclcul() {return _iclcul;}
	int &getIclcwl() {return _iclcwl;}
	int &getIclcll() {return _iclcll;}

	const int &getIclcul() const {return _iclcul;}
	const int &getIclcwl() const {return _iclcwl;}
	const int &getIclcll() const {return _iclcll;}

	void setIclcul(const int &val) {_iclcul = val;}
	void setIclcwl(const int &val) {_iclcwl = val;}
	void seIclcllt(const int &val) {_iclcll = val;}

private:
	int _iclcul;	//ICLCUL Žx“_UFLG_JC Ši“_H_FLG_JC
	int _iclcwl;	//ICLCWL Žx“_WEB_JC Ši“_H_WEB_JC
	int _iclcll;	//ICLCLL Žx“_LFLG_JC

};

