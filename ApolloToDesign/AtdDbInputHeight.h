#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//入力･腹板高･変化点
class AtdDbInputHeight;
class AtdDbInputHeightItem;

class AtdDbInputHeight
{
public:
	AtdDbInputHeight(void) {}
	~AtdDbInputHeight(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbInputHeightItemList.size(); }
	void append(AtdDbInputHeightItem &atdDbInputHeightItem) { _atdDbInputHeightItemList.push_back(atdDbInputHeightItem); }
	JptErrorStatus getAt(int index, AtdDbInputHeightItem &atdDbInputHeightItem);

private:
	vector<AtdDbInputHeightItem> _atdDbInputHeightItemList;

};

class AtdDbInputHeightItem
{
public:
	AtdDbInputHeightItem(void) {}
	~AtdDbInputHeightItem(void) {}

	int &getInogrd() {return _inogrd;}
	int &getInopt() {return _inopt;}
	double &getHweb() {return _hweb;}
	int &getItplc() {return _itplc;}

	const int &getInogrd() const {return _inogrd;}
	const int &getInopt() const {return _inopt;}
	const double &getHweb() const {return _hweb;}
	const int &getItplc() const {return _itplc;}

	void setInogrd(const int &val) {_inogrd = val;}
	void setInopt(const int &val) {_inopt = val;}
	void setHweb(const double &val) {_hweb = val;}
	void setItplc(const int &val) {_itplc = val;}

private:
	int _inogrd;	//NOGRD
	int _inopt;		//NOPT
	double _hweb;	//HWEB ウェブ高
	int _itplc;		//ITPLC ウェブ高補間方法

};

