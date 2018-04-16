#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//断面･カットデータ
class AtdDbSecCutData;
class AtdDbSecCutDataItem;

class AtdDbSecCutData
{
public:
	AtdDbSecCutData(void) {}
	~AtdDbSecCutData(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbSecCutDataItemList.size(); }
	void append(AtdDbSecCutDataItem &atdDbSecCutDataItem) { _atdDbSecCutDataItemList.push_back(atdDbSecCutDataItem); }
	JptErrorStatus getAt(int index, AtdDbSecCutDataItem &atdDbSecCutDataItem);

private:
	vector<AtdDbSecCutDataItem> _atdDbSecCutDataItemList;

};

class AtdDbSecCutDataItem
{
public:
	AtdDbSecCutDataItem(void) {}
	~AtdDbSecCutDataItem(void) {}

	int &getDtcut1() {return _dtcut1;}
	double &getDtcut2() {return _dtcut2;}
	double &getDtcut4() {return _dtcut4;}
	double &getDtcut3() {return _dtcut3;}
	double &getDtcut5() {return _dtcut5;}

	const int &getDtcut1() const {return _dtcut1;}
	const double &getDtcut2() const {return _dtcut2;}
	const double &getDtcut4() const {return _dtcut4;}
	const double &getDtcut3() const {return _dtcut3;}
	const double &getDtcut5() const {return _dtcut5;}

	void setDtcut1(const int &val) {_dtcut1 = val;}
	void setDtcut2(const double &val) {_dtcut2 = val;}
	void setDtcut4(const double &val) {_dtcut4 = val;}
	void setDtcut3(const double &val) {_dtcut3 = val;}
	void setDtcut5(const double &val) {_dtcut5 = val;}

private:
	int _dtcut1;	//DTCUT1
	double _dtcut2;	//DTCUT2 MH_位置
	double _dtcut4;	//DTCUT4 MH_高さ
	double _dtcut3;	//DTCUT3 MH_幅
	double _dtcut5;	//DTCUT5 MH_Rサイズ

};

