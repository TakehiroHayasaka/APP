#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//断面･詳細･補剛材の向き
class AtdDbSecStfDir;
class AtdDbSecStfDirItem;

class AtdDbSecStfDir
{
public:
	AtdDbSecStfDir(void) {}
	~AtdDbSecStfDir(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbSecStfDirItemList.size(); }
	void append(AtdDbSecStfDirItem &atdDbSecStfDirItem) { _atdDbSecStfDirItemList.push_back(atdDbSecStfDirItem); }
	JptErrorStatus getAt(int index, AtdDbSecStfDirItem &atdDbSecStfDirItem);
	int find(const int& nostr);
	int findIdrv(const int& nostr);

private:
	vector<AtdDbSecStfDirItem> _atdDbSecStfDirItemList;

};

class AtdDbSecStfDirItem
{
public:
	AtdDbSecStfDirItem(void) {}
	~AtdDbSecStfDirItem(void) {}

	int &getNostr() {return _nostr;}
	int &getIdrv() {return _idrv;}
	int &getIdrh() {return _idrh;}

	const int &getNostr() const {return _nostr;}
	const int &getIdrv() const {return _idrv;}
	const int &getIdrh() const {return _idrh;}

	void setNostr(const int &val) {_nostr = val;}
	void setIdrv(const int &val) {_idrv = val;}
	void setIdrh(const int &val) {_idrh = val;}

private:
	int _nostr;		//NOSTR
	int _idrv;		//IDRV 配置面
	int _idrh;		//IDRH 配置面
};

