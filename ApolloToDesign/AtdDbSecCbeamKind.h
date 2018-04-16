#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//’f–Ê¥‰¡Œ…í—Ş”
class AtdDbSecCbeamKind
{
public:
	AtdDbSecCbeamKind(void) {}
	~AtdDbSecCbeamKind(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);

	int &getNecrs() {return _necrs;}
	int &getNmcrs() {return _nmcrs;}
	int &getNicrs() {return _nicrs;}

	const int &getNecrs() const {return _necrs;}
	const int &getNmcrs() const {return _nmcrs;}
	const int &getNicrs() const {return _nicrs;}

	void setNecrs(const int &val) {_necrs = val;}
	void setNmcrs(const int &val) {_nmcrs = val;}
	void setNicrs(const int &val) {_nicrs = val;}

private:
	int _necrs;	//NECRS
	int _nmcrs;	//NMCRS
	int _nicrs;	//NICRS

};

