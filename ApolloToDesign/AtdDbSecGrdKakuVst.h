#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//ífñ •éÂåÖäiì_è„VSTF
class AtdDbSecGrdKakuVst;
class AtdDbSecGrdKakuVstItem;

class AtdDbSecGrdKakuVst
{
public:
	AtdDbSecGrdKakuVst(void) {}
	~AtdDbSecGrdKakuVst(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbSecGrdKakuVstItemList.size(); }
	void append(AtdDbSecGrdKakuVstItem &atdDbSecGrdKakuVstItem) { _atdDbSecGrdKakuVstItemList.push_back(atdDbSecGrdKakuVstItem); }
	JptErrorStatus getAt(int index, AtdDbSecGrdKakuVstItem &atdDbSecGrdKakuVstItem);
	int findId(const int& ig, const int& ip);

private:
	vector<AtdDbSecGrdKakuVstItem> _atdDbSecGrdKakuVstItemList;

};

class AtdDbSecGrdKakuVstItem
{
public:
	AtdDbSecGrdKakuVstItem(void) {}
	~AtdDbSecGrdKakuVstItem(void) {}

	int &getNogrd() {return _nogrd;}
	int &getNocrs() {return _nocrs;}
	int &getNovstp() {return _novstp;}

	const int &getNogrd() const {return _nogrd;}
	const int &getNocrs() const {return _nocrs;}
	const int &getNovstp() const {return _novstp;}

	void setNogrd(const int &val) {_nogrd = val;}
	void setNocrs(const int &val) {_nocrs = val;}
	void setNovstp(const int &val) {_novstp = val;}

private:
	int _nogrd;		//NOGRD åÖñº
	int _nocrs;		//NOCRS
	int _novstp;	//NOVSTP îzíuñ  éØï ID

};

