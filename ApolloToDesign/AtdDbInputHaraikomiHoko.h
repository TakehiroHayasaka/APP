#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//ì¸óÕ•ï•Ç¢çûÇ›ï˚å¸
class AtdDbInputHaraikomiHoko;
class AtdDbInputHaraikomiHokoItem;

class AtdDbInputHaraikomiHoko
{
public:
	AtdDbInputHaraikomiHoko(void) {}
	~AtdDbInputHaraikomiHoko(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbInputHaraikomiHokoItemList.size(); }
	void append(AtdDbInputHaraikomiHokoItem &atdDbInputHaraikomiHokoItem) { _atdDbInputHaraikomiHokoItemList.push_back(atdDbInputHaraikomiHokoItem); }
	JptErrorStatus getAt(int index, AtdDbInputHaraikomiHokoItem &atdDbInputHaraikomiHokoItem);
	int findItphr(const int& nostr);

private:
	vector<AtdDbInputHaraikomiHokoItem> _atdDbInputHaraikomiHokoItemList;

};

class AtdDbInputHaraikomiHokoItem
{
public:
	AtdDbInputHaraikomiHokoItem(void) {}
	~AtdDbInputHaraikomiHokoItem(void) {}

	int &getNostr() {return _nostr;}
	const int &getNostr() const {return _nostr;}
	void setNostr(const int &val) {_nostr = val;}

	int &getItphr() {return _itphr;}
	const int &getItphr() const {return _itphr;}
	void setItphr(const int &val) {_itphr = val;}

private:
	int _nostr;	//NOSTR
	int _itphr;	//ITPHR

};

