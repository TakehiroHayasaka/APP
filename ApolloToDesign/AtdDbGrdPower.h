// 2018/03/01 take Add Start
#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//断面･VSTF配置
class AtdDbGrdPower;
class AtdDbGrdPowerItem;

class AtdDbGrdPower
{
public:
	AtdDbGrdPower(void) {}
	~AtdDbGrdPower(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbGrdPowerItemList.size(); }
	void append(AtdDbGrdPowerItem &atdDbGrdPowerItem) { _atdDbGrdPowerItemList.push_back(atdDbGrdPowerItem); }
	JptErrorStatus getAt(int index, AtdDbGrdPowerItem &atdDbGrdPowerItem);
	double findForceMx(const int& grdNo, const int& panelNo);

private:
	vector<AtdDbGrdPowerItem> _atdDbGrdPowerItemList;

};

class AtdDbGrdPowerItem
{
public:
	AtdDbGrdPowerItem(void) {}
	~AtdDbGrdPowerItem(void) {}

	int &getBuzaiCode() {return _buzaiCode;}
	int &getSecCode() {return _secCode;}
	int &getShosaPosNo() {return _shosaPosNo;}
	int &getSecCaseNo() {return _secCaseNo;}
	int &getShosaPoiNo() {return _shosaPoiNo;}
	double &getForceMx() {return _forceMx;}

	const int &getBuzaiCode() const {return _buzaiCode;}
	const int &getSecCode() const {return _secCode;}
	const int &getShosaPosNo() const {return _shosaPosNo;}
	const int &getSecCaseNo() const {return _secCaseNo;}
	const int &getShosaPoiNo() const {return _shosaPoiNo;}
	const double &getForceMx() const {return _forceMx;}

	void setBuzaiCode(const int &val) {_buzaiCode = val;}
	void setSecCode(const int &val) {_secCode = val;}
	void setShosaPosNo(const int &val) {_shosaPosNo = val;}
	void setSecCaseNo(const int &val) {_secCaseNo = val;}
	void setShosaPoiNo(const int &val) {_shosaPoiNo = val;}
	void setForceMx(const double &val) {_forceMx = val;}

private:
	int _buzaiCode;		//部材線コード
	int _secCode;		//断面コード
	int _shosaPosNo;	//照査位置番号
	int _secCaseNo;		//断面力ケース番号
	int _shosaPoiNo;	//照査点番号
	double _forceMx;	//Ｍｘに対する応力度
};
// 2018/03/01 take Add End