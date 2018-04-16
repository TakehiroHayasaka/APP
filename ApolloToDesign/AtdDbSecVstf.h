#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//ífñ •VSTFífñ 
class AtdDbSecVstf;
class AtdDbSecVstfItem;

class AtdDbSecVstf
{
public:
	AtdDbSecVstf(void) {}
	~AtdDbSecVstf(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbSecVstfItemList.size(); }
	void append(AtdDbSecVstfItem &atdDbSecVstfItem) { _atdDbSecVstfItemList.push_back(atdDbSecVstfItem); }
	JptErrorStatus getAt(int index, AtdDbSecVstfItem &atdDbSecVstfItem);

private:
	vector<AtdDbSecVstfItem> _atdDbSecVstfItemList;

};

class AtdDbSecVstfItem
{
public:
	AtdDbSecVstfItem(void) {}
	~AtdDbSecVstfItem(void) {}

	int &getNovssc() {return _novssc;}
	double &getVssc3() {return _vssc3;}
	double &getVssc5() {return _vssc5;}
	int &getVssc8() {return _vssc8;}

	const int &getNovssc() const {return _novssc;}
	const double &getVssc3() const {return _vssc3;}
	const double &getVssc5() const {return _vssc5;}
	const int &getVssc8() const {return _vssc8;}

	void setNovssc(const int &val) {_novssc = val;}
	void setVssc3(const double &val) {_vssc3 = val;}
	void setVssc5(const double &val) {_vssc5 = val;}
	void setVssc8(const int &val) {_vssc8 = val;}

private:
	int _novssc;	//NOVSSC éØï ID
	double _vssc3;	//VSSC3 ïù
	double _vssc5;	//VSSC5 î¬å˙
	int _vssc8;	//VSSC8 çﬁéø

};

