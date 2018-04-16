#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//ì¸óÕ•éÂåÖë§ñ å`èÛ
class AtdDbInputGrdMen;
class AtdDbInputGrdMenItem;

class AtdDbInputGrdMen
{
public:
	AtdDbInputGrdMen(void) {}
	~AtdDbInputGrdMen(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbInputGrdMenItemList.size(); }
	void append(AtdDbInputGrdMenItem &atdDbInputGrdMenItem) { _atdDbInputGrdMenItemList.push_back(atdDbInputGrdMenItem); }
	JptErrorStatus getAt(int index, AtdDbInputGrdMenItem &atdDbInputGrdMenItem);

private:
	vector<AtdDbInputGrdMenItem> _atdDbInputGrdMenItemList;

};

class AtdDbInputGrdMenItem
{
public:
	AtdDbInputGrdMenItem(void) {}
	~AtdDbInputGrdMenItem(void) {}

	int &getStrno() {return _strno;}
	int &getItpuw() {return _itpuw;}
	int &getItpww() {return _itpww;}

	const int &getStrno() const {return _strno;}
	const int &getItpuw() const {return _itpuw;}
	const int &getItpww() const {return _itpww;}

	void setStrno(const int &val) {_strno = val;}
	void setItpuw(const int &val) {_itpuw = val;}
	void setItpww(const int &val) {_itpww = val;}

private:
	int _strno;		//STRNO åÖñº
	int _itpuw;		//ITPUW ë§ñ ï‚ä‘ï˚ñ@
	int _itpww;		//ITPWW

};

