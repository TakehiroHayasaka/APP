#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//ífñ •éÂåÖäiì_ñº
class AtdDbSecGrdKaku;
class AtdDbSecGrdKakuItem;

class AtdDbSecGrdKaku
{
public:
	AtdDbSecGrdKaku(void) {}
	~AtdDbSecGrdKaku(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbSecGrdKakuItemList.size(); }
	void append(AtdDbSecGrdKakuItem &atdDbSecGrdKakuItem) { _atdDbSecGrdKakuItemList.push_back(atdDbSecGrdKakuItem); }
	JptErrorStatus getAt(int index, AtdDbSecGrdKakuItem &atdDbSecGrdKakuItem);

private:
	vector<AtdDbSecGrdKakuItem> _atdDbSecGrdKakuItemList;

};

class AtdDbSecGrdKakuItem
{
public:
	AtdDbSecGrdKakuItem(void) {}
	~AtdDbSecGrdKakuItem(void) {}

	int &getNostr() {return _nostr;}
	int &getNocrs() {return _nocrs;}
	CString &getOdname() {return _odname;}

	const int &getNostr() const {return _nostr;}
	const int &getNocrs() const {return _nocrs;}
	const CString &getOdname() const {return _odname;}

	void setNostr(const int &val) {_nostr = val;}
	void setNocrs(const int &val) {_nocrs = val;}
	void setOdname(const CString &val) {_odname = val;}

private:
	int _nostr;		//NOSTR
	int _nocrs;		//NOCRS
	CString _odname;	//ODNAME â°ífê¸ñº

};

