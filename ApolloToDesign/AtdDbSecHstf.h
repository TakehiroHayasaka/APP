#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//ífñ •HSTFífñ 
class AtdDbSecHstf;
class AtdDbSecHstfItem;

class AtdDbSecHstf
{
public:
	AtdDbSecHstf(void) {}
	~AtdDbSecHstf(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbSecHstfItemList.size(); }
	void append(AtdDbSecHstfItem &atdDbSecHstfItem) { _atdDbSecHstfItemList.push_back(atdDbSecHstfItem); }
	JptErrorStatus getAt(int index, AtdDbSecHstfItem &atdDbSecHstfItem);

private:
	vector<AtdDbSecHstfItem> _atdDbSecHstfItemList;

};

class AtdDbSecHstfItem
{
public:
	AtdDbSecHstfItem(void) {}
	~AtdDbSecHstfItem(void) {}

	int &getNohssc() {return _nohssc;}
	double &getHssc3() {return _hssc3;}
	double &getHssc5() {return _hssc5;}
	int &getHssc8() {return _hssc8;}

	const int &getNohssc() const {return _nohssc;}
	const double &getHssc3() const {return _hssc3;}
	const double &getHssc5() const {return _hssc5;}
	const int &getHssc8() const {return _hssc8;}

	void setNohssc(const int &val) {_nohssc = val;}
	void setHssc3(const double &val) {_hssc3 = val;}
	void setHssc5(const double &val) {_hssc5 = val;}
	void setHssc8(const int &val) {_hssc8 = val;}

private:
	int _nohssc;	//NOHSSC éØï ID
	double _hssc3;	//HSSC3 ïù
	double _hssc5;	//HSSC5 î¬å˙
	int _hssc8;		//HSSC8 çﬁéø

};

