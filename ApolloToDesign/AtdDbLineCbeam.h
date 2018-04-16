#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//線形･横桁(対傾構/ブラケット)
class AtdDbLineCbeam;
class AtdDbLineCbeamItem;

class AtdDbLineCbeam
{
public:
	AtdDbLineCbeam(void) {}
	~AtdDbLineCbeam(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbLineCbeamItemList.size(); }
	void append(AtdDbLineCbeamItem &atdDbLineCbeamItem) { _atdDbLineCbeamItemList.push_back(atdDbLineCbeamItem); }
	JptErrorStatus getAt(int index, AtdDbLineCbeamItem &atdDbLineCbeamItem);
	int findNostr(const int& ig1, const int& ig2, const int& npt);
	int findNosec(const int& ig1, const int& ig2, const int& npt);

private:
	vector<AtdDbLineCbeamItem> _atdDbLineCbeamItemList;

};

class AtdDbLineCbeamItem
{
public:
	AtdDbLineCbeamItem(void) {}
	~AtdDbLineCbeamItem(void) {}

	int &getStrcode() {return _strcode;}
	int &getNostr() {return _nostr;}
	int &getNosec() {return _nosec;}
	int &getNosvt() {return _nosvt;}
	int &getNoevt() {return _noevt;}
	int &getNocr() {return _nocr;}

	const int &getStrcode() const {return _strcode;}
	const int &getNostr() const {return _nostr;}
	const int &getNosec() const {return _nosec;}
	const int &getNosvt() const {return _nosvt;}
	const int &getNoevt() const {return _noevt;}
	const int &getNocr() const {return _nocr;}

	void setStrcode(const int &val) {_strcode = val;}
	void setNostr(const int &val) {_nostr = val;}
	void setNosec(const int &val) {_nosec = val;}
	void setNosvt(const int &val) {_nosvt = val;}
	void setNoevt(const int &val) {_noevt = val;}
	void setNocr(const int &val) {_nocr = val;}

private:
	int _strcode;	//STRCODE
	int _nostr;	//NOSTR
	int _nosec;	//NOSEC
	int _nosvt;	//NOSVT 左側桁名
	int _noevt;	//NOEVT 右側桁名
	int _nocr;	//NOCR 格点名

};

