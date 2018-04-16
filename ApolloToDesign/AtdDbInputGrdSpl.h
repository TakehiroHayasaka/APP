#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//ì¸óÕ•éÂåÖìYê⁄
class AtdDbInputGrdSpl;
class AtdDbInputGrdSplItem;

class AtdDbInputGrdSpl
{
public:
	AtdDbInputGrdSpl(void) {}
	~AtdDbInputGrdSpl(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbInputGrdSplItemList.size(); }
	void append(AtdDbInputGrdSplItem &atdDbInputGrdSplItem) { _atdDbInputGrdSplItemList.push_back(atdDbInputGrdSplItem); }
	JptErrorStatus getAt(int index, AtdDbInputGrdSplItem &atdDbInputGrdSplItem);

private:
	vector<AtdDbInputGrdSplItem> _atdDbInputGrdSplItemList;

};

class AtdDbInputGrdSplItem
{
public:
	AtdDbInputGrdSplItem(void) {}
	~AtdDbInputGrdSplItem(void) {}

	int &getIclegu() {return _iclegu;}
	int &getIclegw() {return _iclegw;}
	int &getIclegl() {return _iclegl;}

	const int &getIclegu() const {return _iclegu;}
	const int &getIclegw() const {return _iclegw;}
	const int &getIclegl() const {return _iclegl;}

	void setIclegu(const int &val) {_iclegu = val;}
	void setIclegw(const int &val) {_iclegw = val;}
	void setIclegl(const int &val) {_iclegl = val;}

private:
	int _iclegu;	//ICLEGU UFLG_JC
	int _iclegw;	//ICLEGW WEB_JC
	int _iclegl;	//ICLEGL LFLG_JC

};

