#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//ê¸å`•éÂåÖç¿ïW(ècåÖ/ë§ècåÖ)
class AtdDbLineGrdZahyo;
class AtdDbLineGrdZahyoItem;

class AtdDbLineGrdZahyo
{
public:
	AtdDbLineGrdZahyo(void) {}
	~AtdDbLineGrdZahyo(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbLineGrdZahyoItemList.size(); }
	void append(AtdDbLineGrdZahyoItem &atdDbLineGrdZahyoItem) { _atdDbLineGrdZahyoItemList.push_back(atdDbLineGrdZahyoItem); }
	JptErrorStatus getAt(int index, AtdDbLineGrdZahyoItem &atdDbLineGrdZahyoItem);

private:
	vector<AtdDbLineGrdZahyoItem> _atdDbLineGrdZahyoItemList;

};

class AtdDbLineGrdZahyoItem
{
public:
	AtdDbLineGrdZahyoItem(void) {}
	~AtdDbLineGrdZahyoItem(void) {}

	int &getNostr() {return _nostr;}
	int &getNopt() {return _nopt;}
	int &getItmb() {return _itmb;}
	double &getXu() {return _xu;}
	double &getYu() {return _yu;}
	double &getZu() {return _zu;}

	const int &getNostr() const {return _nostr;}
	const int &getNopt() const {return _nopt;}
	const int &getItmb() const {return _itmb;}
	const double &getXu() const {return _xu;}
	const double &getYu() const {return _yu;}
	const double &getZu() const {return _zu;}

	void setNostr(const int &val) {_nostr = val;}
	void setNopt(const int &val) {_nopt = val;}
	void setItmb(const int &val) {_itmb = val;}
	void setXu(const double &val) {_xu = val;}
	void setYu(const double &val) {_yu = val;}
	void setZu(const double &val) {_zu = val;}

private:
	int _nostr;		//NOSTR
	int _nopt;		//NOPT
	int _itmb;		//ITMB
	double _xu;		//XU
	double _yu;		//YU
	double _zu;		//ZU

};

