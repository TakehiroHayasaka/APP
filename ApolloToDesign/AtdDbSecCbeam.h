#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//断面･横桁
class AtdDbSecCbeam;
class AtdDbSecCbeamItem;

class AtdDbSecCbeam
{
public:
	AtdDbSecCbeam(void) {}
	~AtdDbSecCbeam(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbSecCbeamItemList.size(); }
	void append(AtdDbSecCbeamItem &atdDbSecCbeamItem) { _atdDbSecCbeamItemList.push_back(atdDbSecCbeamItem); }
	JptErrorStatus getAt(int index, AtdDbSecCbeamItem &atdDbSecCbeamItem);
	int find(const int& nocrs);
	double findWebHeight(const int& nocrs);

private:
	vector<AtdDbSecCbeamItem> _atdDbSecCbeamItemList;

};

class AtdDbSecCbeamItem
{
public:
	AtdDbSecCbeamItem(void) {}
	~AtdDbSecCbeamItem(void) {}

	int &getNocrs() {return _nocrs;}
	double &getH3cl() {return _h3cl;}
	double &getH3cr() {return _h3cr;}
	double &getH1cl() {return _h1cl;}
	double &getH1cr() {return _h1cr;}
	double &getH2cl() {return _h2cl;}
	double &getH2cr() {return _h2cr;}
	double &getDspll() {return _dspll;}
	double &getDsplr() {return _dsplr;}
	int &getNocjul() {return _nocjul;}
	int &getNocjll() {return _nocjll;}
	int &getNocjwl() {return _nocjwl;}
	int &getNocjur() {return _nocjur;}
	int &getNocjlr() {return _nocjlr;}
	int &getNocjwr() {return _nocjwr;}

	const int &getNocrs() const {return _nocrs;}
	const double &getH3cl() const {return _h3cl;}
	const double &getH3cr() const {return _h3cr;}
	const double &getH1cl() const {return _h1cl;}
	const double &getH1cr() const {return _h1cr;}
	const double &getH2cl() const {return _h2cl;}
	const double &getH2cr() const {return _h2cr;}
	const double &getDspll() const {return _dspll;}
	const double &getDsplr() const {return _dsplr;}
	const int &getNocjul() const {return _nocjul;}
	const int &getNocjll() const {return _nocjll;}
	const int &getNocjwl() const {return _nocjwl;}
	const int &getNocjur() const {return _nocjur;}
	const int &getNocjlr() const {return _nocjlr;}
	const int &getNocjwr() const {return _nocjwr;}

	void setNocrs(const int &val) {_nocrs = val;}
	void setH3cl(const double &val) {_h3cl = val;}
	void setH3cr(const double &val) {_h3cr = val;}
	void setH1cl(const double &val) {_h1cl = val;}
	void setH1cr(const double &val) {_h1cr = val;}
	void setH2cl(const double &val) {_h2cl = val;}
	void setH2cr(const double &val) {_h2cr = val;}
	void setDspll(const double &val) {_dspll = val;}
	void setDsplr(const double &val) {_dsplr = val;}
	void setNocjul(const int &val) {_nocjul = val;}
	void setNocjll(const int &val) {_nocjll = val;}
	void setNocjwl(const int &val) {_nocjwl = val;}
	void setNocjur(const int &val) {_nocjur = val;}
	void setNocjlr(const int &val) {_nocjlr = val;}
	void setNocjwr(const int &val) {_nocjwr = val;}

private:
	int _nocrs;	//NOCRS
	double _h3cl;	//H3CL H鋼ウェブ高
	double _h3cr;	//H3CR H鋼ウェブ高
	double _h1cl;	//H1CL LU LC
	double _h1cr;	//H1CR LD RC
	double _h2cl;	//H2CL RU
	double _h2cr;	//H2CR RD
	double _dspll;	//DSPLL LJ
	double _dsplr;	//DSPLR RJ
	int _nocjul;	//NOCJUL:上フランジ左側の添接番号
	int _nocjll;	//NOCJLL:下フランジ左側の添接番号
	int _nocjwl;	//NOCJWL:ウェブ左側の添接番号
	int _nocjur;	//NOCJUR:上フランジ右側の添接番号
	int _nocjlr;	//NOCJLR:下フランジ右側の添接番号
	int _nocjwr;	//NOCJWR:ウェブ右側の添接番号

};

