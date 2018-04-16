#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//ì¸óÕ•ÇrÇsÇeÇÃÇ†Ç´
class AtdDbInputStfAki;
class AtdDbInputStfAkiItem;

class AtdDbInputStfAki
{
public:
	AtdDbInputStfAki(void) {}
	~AtdDbInputStfAki(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbInputStfAkiItemList.size(); }
	void append(AtdDbInputStfAkiItem &atdDbInputStfAkiItem) { _atdDbInputStfAkiItemList.push_back(atdDbInputStfAkiItem); }
	JptErrorStatus getAt(int index, AtdDbInputStfAkiItem &atdDbInputStfAkiItem);

private:
	vector<AtdDbInputStfAkiItem> _atdDbInputStfAkiItemList;

};

class AtdDbInputStfAkiItem
{
public:
	AtdDbInputStfAkiItem(void) {}
	~AtdDbInputStfAkiItem(void) {}

	int &getIdgfvi() {return _idgfvi;}
	const int &getIdgfvi() const {return _idgfvi;}
	void setIdgfvi(const int &val) {_idgfvi = val;}

private:
	int _idgfvi;	//IDGFVI ì¸óÕÅESTFÇÃÇ†Ç´ É^ÉCÉv

};

