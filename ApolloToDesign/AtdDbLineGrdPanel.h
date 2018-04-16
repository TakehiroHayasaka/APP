#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//線形･主桁(横桁/ブラケット)･パネル長
class AtdDbLineGrdPanel;
class AtdDbLineGrdPanelItem;

class AtdDbLineGrdPanel
{
public:
	AtdDbLineGrdPanel(void) {}
	~AtdDbLineGrdPanel(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbLineGrdPanelItemList.size(); }
	void append(AtdDbLineGrdPanelItem &atdDbLineGrdPanelItem) { _atdDbLineGrdPanelItemList.push_back(atdDbLineGrdPanelItem); }
	JptErrorStatus getAt(int index, AtdDbLineGrdPanelItem &atdDbLineGrdPanelItem);
	double findRlp(const int& strcode, const int& nostr, const int& nopnl);

private:
	vector<AtdDbLineGrdPanelItem> _atdDbLineGrdPanelItemList;

};

class AtdDbLineGrdPanelItem
{
public:
	AtdDbLineGrdPanelItem(void) {}
	~AtdDbLineGrdPanelItem(void) {}

	int &getStrcode() {return _strcode;}
	int &getNostr() {return _nostr;}
	int &getNopnl() {return _nopnl;}
	double &getRlp() {return _rlp;}

	const int &getStrcode() const {return _strcode;}
	const int &getNostr() const {return _nostr;}
	const int &getNopnl() const {return _nopnl;}
	const double &getRlp() const {return _rlp;}

	void setStrcode(const int &val) {_strcode = val;}
	void setNostr(const int &val) {_nostr = val;}
	void setNopnl(const int &val) {_nopnl = val;}
	void setRlp(const double &val) {_rlp = val;}

private:
	int _strcode;	//STRCODE
	int _nostr;		//NOSTR
	int _nopnl;		//NOPNL
	double _rlp;	//RLP

};

