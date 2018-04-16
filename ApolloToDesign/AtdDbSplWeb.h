#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//添接･各添接･ウェブ
class AtdDbSplWeb;
class AtdDbSplWebItem;

class AtdDbSplWeb
{
public:
	AtdDbSplWeb(void) {}
	~AtdDbSplWeb(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbSplWebItemList.size(); }
	void append(AtdDbSplWebItem &atdDbSplWebItem) { _atdDbSplWebItemList.push_back(atdDbSplWebItem); }
	JptErrorStatus getAt(int index, AtdDbSplWebItem &atdDbSplWebItem);
	int findItpwj(const int& nocj);

private:
	vector<AtdDbSplWebItem> _atdDbSplWebItemList;

};

class AtdDbSplWebItem
{
public:
	AtdDbSplWebItem(void) {}
	~AtdDbSplWebItem(void) {}

	int &getStrcode() {return _strcode;}
	int &getPos() {return _pos;}
	int &getNowj() {return _nowj;}
	int &getItpwj() {return _itpwj;}
	double &getGwj() {return _gwj;}
	int &getNg2wj() {return _ng2wj;}
	double &getGowj() {return _gowj;}
	double &getP2wj() {return _p2wj;}
	int &getNd2wj() {return _nd2wj;}
	double &getP3wj() {return _p3wj;}
	int &getNd3wj() {return _nd3wj;}
	double &getP0wj() {return _p0wj;}
	double &getP00j() {return _p00j;}
	double &getT2wj() {return _t2wj;}
	int &getMtwj() {return _mtwj;}

	const int &getStrcode() const {return _strcode;}
	const int &getPos() const {return _pos;}
	const int &getNowj() const {return _nowj;}
	const int &getItpwj() const {return _itpwj;}
	const double &getGwj() const {return _gwj;}
	const int &getNg2wj() const {return _ng2wj;}
	const double &getGowj() const {return _gowj;}
	const double &getP2wj() const {return _p2wj;}
	const int &getNd2wj() const {return _nd2wj;}
	const double &getP3wj() const {return _p3wj;}
	const int &getNd3wj() const {return _nd3wj;}
	const double &getP0wj() const {return _p0wj;}
	const double &getP00j() const {return _p00j;}
	const double &getT2wj() const {return _t2wj;}
	const int &getMtwj() const {return _mtwj;}

	void setStrcode(const int &val) {_strcode = val;}
	void setPos(const int &val) {_pos = val;}
	void setNowj(const int &val) {_nowj = val;}
	void setItpwj(const int &val) {_itpwj = val;}
	void setGwj(const double &val) {_gwj = val;}
	void setNg2wj(const int &val) {_ng2wj = val;}
	void setGowj(const double &val) {_gowj = val;}
	void setP2wj(const double &val) {_p2wj = val;}
	void setNd2wj(const int &val) {_nd2wj = val;}
	void setP3wj(const double &val) {_p3wj = val;}
	void setNd3wj(const int &val) {_nd3wj = val;}
	void setP0wj(const double &val) {_p0wj = val;}
	void setP00j(const double &val) {_p00j = val;}
	void setT2wj(const double &val) {_t2wj = val;}
	void setMtwj(const int &val) {_mtwj = val;}

private:
	int _strcode;	//STRCODE
	int _pos;		//POS
	int _nowj;		//NOWJ ジョイント番号
	int _itpwj;		//ITPWJ
	double _gwj;	//GWJ P
	int _ng2wj;		//NG2WJ Pの数
	double _gowj;	//GOWJ CP
	double _p2wj;	//P2WJ 上側G G
	int _nd2wj;		//ND2WJ 上側Gの数
	double _p3wj;	//P3WJ 中央G
	int _nd3wj;		//ND3WJ 中央Gの数
	double _p0wj;	//P0WJ 上側空き量
	double _p00j;	//P00J 下側空き量
	double _t2wj;	//T2WJ SPL板厚 SPL_板厚
	int _mtwj;		//MTWJ SPL材質 SPL_材質

};

