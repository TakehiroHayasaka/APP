#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//線形･主桁(横桁/ブラケット)･キャンバー値
class AtdDbLineGrdCamber;
class AtdDbLineGrdCamberItem;

class AtdDbLineGrdCamber
{
public:
	AtdDbLineGrdCamber(void) {}
	~AtdDbLineGrdCamber(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbLineGrdCamberItemList.size(); }
	void append(AtdDbLineGrdCamberItem &atdDbLineGrdCamberItem) { _atdDbLineGrdCamberItemList.push_back(atdDbLineGrdCamberItem); }
	JptErrorStatus getAt(int index, AtdDbLineGrdCamberItem &atdDbLineGrdCamberItem);

private:
	vector<AtdDbLineGrdCamberItem> _atdDbLineGrdCamberItemList;

};

class AtdDbLineGrdCamberItem
{
public:
	AtdDbLineGrdCamberItem(void) {}
	~AtdDbLineGrdCamberItem(void) {}

	int &getNostr() {return _nostr;}
	int &getNopnl() {return _nopnl;}
	double &getZcamz() {return _zcamz;}

	const int &getNostr() const {return _nostr;}
	const int &getNopnl() const {return _nopnl;}
	const double &getZcamz() const {return _zcamz;}

	void setNostr(const int &val) {_nostr = val;}
	void setNopnl(const int &val) {_nopnl = val;}
	void setZcamz(const double &val) {_zcamz = val;}

private:
	int _nostr;		//NOSTR 桁名
	int _nopnl;		//NOPNL
	double _zcamz;	//ZCAMZ Zキャンバー

};

