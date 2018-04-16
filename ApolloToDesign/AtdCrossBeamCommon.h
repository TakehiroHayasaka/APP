#pragma once
#include "DiafManager.h"

class AtdCrossBeamCommon
{
public:
	AtdCrossBeamCommon() {}
	~AtdCrossBeamCommon() {}

	double &getShitenUflgJc() {return _shitenUflgJc;}
	double &getShitenWebJc() {return _shitenWebJc;}
	double &getShitenLflgJc() {return _shitenLflgJc;}
	double &getKakutenHflgJc() {return _kakutenHflgJc;}
	double &getKakutenHwebJc() {return _kakutenHwebJc;}
	string &getShitenConnCut() {return _shitenConnCut;}
	double &getShitenConnFillet() {return _shitenConnFillet;}
	double &getShitenConnTachiageryo() {return _shitenConnTachiageryo;}
	string &getKakutenConnCut() {return _kakutenConnCut;}
	double &getKakutenConnFillet() {return _kakutenConnFillet;}
	double &getKakutenConnTachiageryo() {return _kakutenConnTachiageryo;}
	string &getCvsCutWu() {return _cvsCutWu;}
	string &getCvsCutWd() {return _cvsCutWd;}
	double &getWebHoleSlopeLowerLimitCrs() {return _webHoleSlopeLowerLimitCrs;}
	EnTypeNo &getFlgSectionType() {return _flgSectionType;}
	double &getShitenUflgSplKyojikuZaitan() {return _shitenUflgSplKyojikuZaitan;}
	double &getShitenUflgSplKyotyokuZaitan() {return _shitenUflgSplKyotyokuZaitan;}
	double &getShitenWebSplKyotyokuZaitan() {return _shitenWebSplKyotyokuZaitan;}
	double &getShitenWebSplHeightZaitan() {return _shitenWebSplHeightZaitan;}
	double &getShitenLflgSplKyojikuZaitan() {return _shitenLflgSplKyojikuZaitan;}
	double &getShitenLflgSplKyotyokuZaitan() {return _shitenLflgSplKyotyokuZaitan;}
	double &getShitenConnKyojikuZaitan() {return _shitenConnKyojikuZaitan;}
	double &getShitenConnKyoutyokuZaitan() {return _shitenConnKyoutyokuZaitan;}
	double &getKakutenUflgSplKyojikuZaitan() {return _kakutenUflgSplKyojikuZaitan;}
	double &getKakutenUflgSplKyotyokuZaitan() {return _kakutenUflgSplKyotyokuZaitan;}
	double &getKakutenWebSplKyotyokuZaitan() {return _kakutenWebSplKyotyokuZaitan;}
	double &getKakutenWebSplHeightZaitan() {return _kakutenWebSplHeightZaitan;}
	double &getKakutenLflgSplKyojikuZaitan() {return _kakutenLflgSplKyojikuZaitan;}
	double &getKakutenLflgSplKyotyokuZaitan() {return _kakutenLflgSplKyotyokuZaitan;}
	double &getKakutenConnKyojikuZaitan() {return _kakutenConnKyojikuZaitan;}
	double &getKakutenConnKyoutyokuZaitan() {return _kakutenConnKyoutyokuZaitan;}

	const double &getShitenUflgJc() const {return _shitenUflgJc;}
	const double &getShitenWebJc() const {return _shitenWebJc;}
	const double &getShitenLflgJc() const {return _shitenLflgJc;}
	const double &getKakutenHflgJc() const {return _kakutenHflgJc;}
	const double &getKakutenHwebJc() const {return _kakutenHwebJc;}
	const string &getShitenConnCut() const {return _shitenConnCut;}
	const double &getShitenConnFillet() const {return _shitenConnFillet;}
	const double &getShitenConnTachiageryo() const {return _shitenConnTachiageryo;}
	const string &getKakutenConnCut() const {return _kakutenConnCut;}
	const double &getKakutenConnFillet() const {return _kakutenConnFillet;}
	const double &getKakutenConnTachiageryo() const {return _kakutenConnTachiageryo;}
	const string &getCvsCutWu() const {return _cvsCutWu;}
	const string &getCvsCutWd() const {return _cvsCutWd;}
	const double &getWebHoleSlopeLowerLimitCrs() const {return _webHoleSlopeLowerLimitCrs;}
	const EnTypeNo &getFlgSectionType() const {return _flgSectionType;}
	const double &getShitenUflgSplKyojikuZaitan() const {return _shitenUflgSplKyojikuZaitan;}
	const double &getShitenUflgSplKyotyokuZaitan() const {return _shitenUflgSplKyotyokuZaitan;}
	const double &getShitenWebSplKyotyokuZaitan() const {return _shitenWebSplKyotyokuZaitan;}
	const double &getShitenWebSplHeightZaitan() const {return _shitenWebSplHeightZaitan;}
	const double &getShitenLflgSplKyojikuZaitan() const {return _shitenLflgSplKyojikuZaitan;}
	const double &getShitenLflgSplKyotyokuZaitan() const {return _shitenLflgSplKyotyokuZaitan;}
	const double &getShitenConnKyojikuZaitan() const {return _shitenConnKyojikuZaitan;}
	const double &getShitenConnKyoutyokuZaitan() const {return _shitenConnKyoutyokuZaitan;}
	const double &getKakutenUflgSplKyojikuZaitan() const {return _kakutenUflgSplKyojikuZaitan;}
	const double &getKakutenUflgSplKyotyokuZaitan() const {return _kakutenUflgSplKyotyokuZaitan;}
	const double &getKakutenWebSplKyotyokuZaitan() const {return _kakutenWebSplKyotyokuZaitan;}
	const double &getKakutenWebSplHeightZaitan() const {return _kakutenWebSplHeightZaitan;}
	const double &getKakutenLflgSplKyojikuZaitan() const {return _kakutenLflgSplKyojikuZaitan;}
	const double &getKakutenLflgSplKyotyokuZaitan() const {return _kakutenLflgSplKyotyokuZaitan;}
	const double &getKakutenConnKyojikuZaitan() const {return _kakutenConnKyojikuZaitan;}
	const double &getKakutenConnKyoutyokuZaitan() const {return _kakutenConnKyoutyokuZaitan;}

	void setShitenUflgJc(const double &val) {_shitenUflgJc = val;}
	void setShitenWebJc(const double &val) {_shitenWebJc = val;}
	void setShitenLflgJc(const double &val) {_shitenLflgJc = val;}
	void setKakutenHflgJc(const double &val) {_kakutenHflgJc = val;}
	void setKakutenHwebJc(const double &val) {_kakutenHwebJc = val;}
	void setShitenConnCut(const string &val) {_shitenConnCut = val;}
	void setShitenConnFillet(const double &val) {_shitenConnFillet = val;}
	void setShitenConnTachiageryo(const double &val) {_shitenConnTachiageryo = val;}
	void setKakutenConnCut(const string &val) {_kakutenConnCut = val;}
	void setKakutenConnFillet(const double &val) {_kakutenConnFillet = val;}
	void setKakutenConnTachiageryo(const double &val) {_kakutenConnTachiageryo = val;}
	void setCvsCutWu(const string &val) {_cvsCutWu = val;}
	void setCvsCutWd(const string &val) {_cvsCutWd = val;}
	void setWebHoleSlopeLowerLimitCrs(const double &val) {_webHoleSlopeLowerLimitCrs = val;}
	void setFlgSectionType(const EnTypeNo &val) {_flgSectionType = val;}
	void setShitenUflgSplKyojikuZaitan(const double &val) {_shitenUflgSplKyojikuZaitan = val;}
	void setShitenUflgSplKyotyokuZaitan(const double &val) {_shitenUflgSplKyotyokuZaitan = val;}
	void setShitenWebSplKyotyokuZaitan(const double &val) {_shitenWebSplKyotyokuZaitan = val;}
	void setShitenWebSplHeightZaitan(const double &val) {_shitenWebSplHeightZaitan = val;}
	void setShitenLflgSplKyojikuZaitan(const double &val) {_shitenLflgSplKyojikuZaitan = val;}
	void setShitenLflgSplKyotyokuZaitan(const double &val) {_shitenLflgSplKyotyokuZaitan = val;}
	void setShitenConnKyojikuZaitan(const double &val) {_shitenConnKyojikuZaitan = val;}
	void setShitenConnKyoutyokuZaitan(const double &val) {_shitenConnKyoutyokuZaitan = val;}
	void setKakutenUflgSplKyojikuZaitan(const double &val) {_kakutenUflgSplKyojikuZaitan = val;}
	void setKakutenUflgSplKyotyokuZaitan(const double &val) {_kakutenUflgSplKyotyokuZaitan = val;}
	void setKakutenWebSplKyotyokuZaitan(const double &val) {_kakutenWebSplKyotyokuZaitan = val;}
	void setKakutenWebSplHeightZaitan(const double &val) {_kakutenWebSplHeightZaitan = val;}
	void setKakutenLflgSplKyojikuZaitan(const double &val) {_kakutenLflgSplKyojikuZaitan = val;}
	void setKakutenLflgSplKyotyokuZaitan(const double &val) {_kakutenLflgSplKyotyokuZaitan = val;}
	void setKakutenConnKyojikuZaitan(const double &val) {_kakutenConnKyojikuZaitan = val;}
	void setKakutenConnKyoutyokuZaitan(const double &val) {_kakutenConnKyoutyokuZaitan = val;}

private:
	//ジョイントクリアランス
	double _shitenUflgJc;	//支点上横桁（BH）上フランジのジョイントクリアランス
	double _shitenWebJc;	//支点上横桁（BH）ウェブのジョイントクリアランス
	double _shitenLflgJc;	//支点上横桁（BH）下フランジのジョイントクリアランス
	double _kakutenHflgJc;	//格点上横桁（H鋼）フランジのジョイントクリアランス
	double _kakutenHwebJc;	//格点上横桁（H鋼）ウェブのジョイントクリアランス
	//コネクションプレート
	string _shitenConnCut;				//主桁ウェブ付きコネクション（支点上）の溶接辺側切欠
	double _shitenConnFillet;			//主桁ウェブ付きコネクション（支点上）のフィレットのRサイズ
	double _shitenConnTachiageryo;		//主桁ウェブ付きコネクション（支点上）の溶接辺立上げ量
	string _kakutenConnCut;				//主桁ウェブ付きコネクション（格点上）の溶接辺側切欠を指定
	double _kakutenConnFillet;			//主桁ウェブ付きコネクション（格点上）のフィレットのRサイズ
	double _kakutenConnTachiageryo;		//主桁ウェブ付きコネクション（格点上）の溶接辺立上げ量
	//垂直補剛材
	string _cvsCutWu;					//横桁付垂直補剛材の溶接辺側上側切欠
	string _cvsCutWd;					//横桁付垂直補剛材の溶接辺側下側切欠
	//その他
	double _webHoleSlopeLowerLimitCrs;	//ウェブ孔の孔勾配の下限値
	EnTypeNo _flgSectionType;			//フランジ切口の方向
	//材端形状
	double _shitenUflgSplKyojikuZaitan;		//支点上横桁上フランジ添接板の橋軸方向材端
	double _shitenUflgSplKyotyokuZaitan;	//支点上横桁上フランジ添接板の橋直方向材端
	double _shitenWebSplKyotyokuZaitan;		//支点上横桁ウェブ添接板の橋直方向材端
	double _shitenWebSplHeightZaitan;		//支点上横桁ウェブ添接板の高さ方向材端
	double _shitenLflgSplKyojikuZaitan;		//支点上横桁下フランジ添接板の橋軸方向材端
	double _shitenLflgSplKyotyokuZaitan;	//支点上横桁下フランジ添接板の橋直方向材端
	double _shitenConnKyojikuZaitan;		//支点上コネクションの橋軸方向材端
	double _shitenConnKyoutyokuZaitan;		//支点上コネクションの橋直方向材端
	double _kakutenUflgSplKyojikuZaitan;	//格点上横桁上フランジ添接板の橋軸方向材端
	double _kakutenUflgSplKyotyokuZaitan;	//格点上横桁上フランジ添接板の橋直方向材端
	double _kakutenWebSplKyotyokuZaitan;	//格点上横桁ウェブ添接板の橋直方向材端
	double _kakutenWebSplHeightZaitan;		//格点上横桁ウェブ添接板の高さ方向材端
	double _kakutenLflgSplKyojikuZaitan;	//格点上横桁下フランジ添接板の橋軸方向材端
	double _kakutenLflgSplKyotyokuZaitan;	//格点上横桁下フランジ添接板の橋直方向材端
	double _kakutenConnKyojikuZaitan;		//格点上コネクションの橋軸方向材端
	double _kakutenConnKyoutyokuZaitan;		//格点上コネクションの橋直方向材端

};

