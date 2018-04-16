#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//’f–Ê¥‰¡Œ…¥’f–Ê
class AtdDbSecCbeamSec;
class AtdDbSecCbeamSecItem;

class AtdDbSecCbeamSec
{
public:
	AtdDbSecCbeamSec(void) {}
	~AtdDbSecCbeamSec(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbSecCbeamSecItemList.size(); }
	void append(AtdDbSecCbeamSecItem &atdDbSecCbeamSecItem) { _atdDbSecCbeamSecItemList.push_back(atdDbSecCbeamSecItem); }
	JptErrorStatus getAt(int index, AtdDbSecCbeamSecItem &atdDbSecCbeamSecItem);

private:
	vector<AtdDbSecCbeamSecItem> _atdDbSecCbeamSecItemList;

};

class AtdDbSecCbeamSecItem
{
public:
	AtdDbSecCbeamSecItem(void) {}
	~AtdDbSecCbeamSecItem(void) {}

	int &getNocrs() {return _nocrs;}
	double &getBfcu() {return _bfcu;}
	double &getTfcu() {return _tfcu;}
	int &getMfcu() {return _mfcu;}
	double &getTfcw() {return _tfcw;}
	int &getMfcw() {return _mfcw;}
	double &getBfcl() {return _bfcl;}
	double &getTfcl() {return _tfcl;}
	int &getMfcl() {return _mfcl;}

	const int &getNocrs() const {return _nocrs;}
	const double &getBfcu() const {return _bfcu;}
	const double &getTfcu() const {return _tfcu;}
	const int &getMfcu() const {return _mfcu;}
	const double &getTfcw() const {return _tfcw;}
	const int &getMfcw() const {return _mfcw;}
	const double &getBfcl() const {return _bfcl;}
	const double &getTfcl() const {return _tfcl;}
	const int &getMfcl() const {return _mfcl;}

	void setNocrs(const int &val) {_nocrs = val;}
	void setBfcu(const double &val) {_bfcu = val;}
	void setTfcu(const double &val) {_tfcu = val;}
	void setMfcu(const int &val) {_mfcu = val;}
	void setTfcw(const double &val) {_tfcw = val;}
	void setMfcw(const int &val) {_mfcw = val;}
	void setBfcl(const double &val) {_bfcl = val;}
	void setTfcl(const double &val) {_tfcl = val;}
	void setMfcl(const int &val) {_mfcl = val;}

private:
	int _nocrs;		//NOCRS
	double _bfcu;	//BFCU UFLG_• H|ƒtƒ‰ƒ“ƒW•
	double _tfcu;	//TFCU UFLG_”ÂŒú H|ƒtƒ‰ƒ“ƒW”ÂŒú
	int _mfcu;		//MFCU UFLG_Ş¿ H|Ş¿
	double _tfcw;	//TFCW WEB_”ÂŒú H|ƒEƒFƒu”ÂŒú
	int _mfcw;		//MFCW WEB_Ş¿
	double _bfcl;	//BFCL LFLG_• H|ƒtƒ‰ƒ“ƒW•
	double _tfcl;	//TFCL LFLG_”ÂŒú H|ƒtƒ‰ƒ“ƒW”ÂŒú
	int _mfcl;		//MFCL LFLG_Ş¿ H|Ş¿

};

