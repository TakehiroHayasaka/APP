#pragma once
#include "DiafManager.h"

class AtdGirderCommon
{
public:
	AtdGirderCommon() {}
	~AtdGirderCommon() {}

	double &getUflgJc() {return _uflgJc;}
	double &getWebJc() {return _webJc;}
	double &getLflgJc() {return _lflgJc;}
	EnTypeNo &getUflgZaitanKeijo() {return _uflgZaitanKeijo;}
	EnTypeNo &getLflgZaitanKeijo() {return _lflgZaitanKeijo;}
	double &getUpdownFlgZaitanKeijoTachiageRyo() {return _updownFlgZaitanKeijoTachiageRyo;}
	double &getSolePlateKyotyokuFreeSpace() {return _solePlateKyotyokuFreeSpace;}
	double &getSolePlateKyojikuFreeSpace() {return _solePlateKyojikuFreeSpace;}
	double &getItatsugiZureRyo() {return _itatsugiZureRyo;}
	double &getLflgKakuhukubuTaper() {return _lflgKakuhukubuTaper;}
	double &getWebHoleSlopeLowerLimitGrd() {return _webHoleSlopeLowerLimitGrd;}
	string &getShitenVsCutWu() {return _shitenVsCutWu;}
	string &getShitenVsCutWd() {return _shitenVsCutWd;}
	string &getShitenVsCutFu() {return _shitenVsCutFu;}
	string &getKakutenVsCutWu() {return _kakutenVsCutWu;}
	string &getKakutenVsCutWd() {return _kakutenVsCutWd;}
	string &getKakutenVsCutFu() {return _kakutenVsCutFu;}
	string &getMiddleVsCutWu() {return _middleVsCutWu;}
	string &getMiddleVsCutWd() {return _middleVsCutWd;}
	double &getMiddleVsFreeSpace() {return _middleVsFreeSpace;}
	double &getHsFreeSpaceVs() {return _hsFreeSpaceVs;}
	double &getHsFreeSpaceSpl() {return _hsFreeSpaceSpl;}
	double &getHsFreeSpaceCbf() {return _hsFreeSpaceCbf;}
	double &getHsFreeSpaceCbfUlimit() {return _hsFreeSpaceCbfUlimit;}
	string &getHsSnipSizeVs() {return _hsSnipSizeVs;}
	string &getHsSnipSizeSpl() {return _hsSnipSizeSpl;}
	string &getHsSnipSizeCbf() {return _hsSnipSizeCbf;}
	double &getUflgSplKyojikuZaitan() {return _uflgSplKyojikuZaitan;}
	double &getUflgOutsideSplKyotyokuZaitan() {return _uflgOutsideSplKyotyokuZaitan;}
	double &getUflgInsideSplKyotyokuZaitan() {return _uflgInsideSplKyotyokuZaitan;}
	double &getWebSplKyojikuZaitan() {return _webSplKyojikuZaitan;}
	double &getWebSplHeightZaitan() {return _webSplHeightZaitan;}
	double &getLflgSplKyojikuZaitan() {return _lflgSplKyojikuZaitan;}
	double &getLflgSplKyotyokuZaitan() {return _lflgSplKyotyokuZaitan;}

	const double &getUflgJc() const {return _uflgJc;}
	const double &getWebJc() const {return _webJc;}
	const double &getLflgJc() const {return _lflgJc;}
	const EnTypeNo &getUflgZaitanKeijo() const {return _uflgZaitanKeijo;}
	const EnTypeNo &getLflgZaitanKeijo() const {return _lflgZaitanKeijo;}
	const double &getUpdownFlgZaitanKeijoTachiageRyo() const {return _updownFlgZaitanKeijoTachiageRyo;}
	const double &getSolePlateKyotyokuFreeSpace() const {return _solePlateKyotyokuFreeSpace;}
	const double &getSolePlateKyojikuFreeSpace() const {return _solePlateKyojikuFreeSpace;}
	const double &getItatsugiZureRyo() const {return _itatsugiZureRyo;}
	const double &getLflgKakuhukubuTaper() const {return _lflgKakuhukubuTaper;}
	const double &getWebHoleSlopeLowerLimitGrd() const {return _webHoleSlopeLowerLimitGrd;}
	const string &getShitenVsCutWu() const {return _shitenVsCutWu;}
	const string &getShitenVsCutWd() const {return _shitenVsCutWd;}
	const string &getShitenVsCutFu() const {return _shitenVsCutFu;}
	const string &getKakutenVsCutWu() const {return _kakutenVsCutWu;}
	const string &getKakutenVsCutWd() const {return _kakutenVsCutWd;}
	const string &getKakutenVsCutFu() const {return _kakutenVsCutFu;}
	const string &getMiddleVsCutWu() const {return _middleVsCutWu;}
	const string &getMiddleVsCutWd() const {return _middleVsCutWd;}
	const double &getMiddleVsFreeSpace() const {return _middleVsFreeSpace;}
	const double &getHsFreeSpaceVs() const {return _hsFreeSpaceVs;}
	const double &getHsFreeSpaceSpl() const {return _hsFreeSpaceSpl;}
	const double &getHsFreeSpaceCbf() const {return _hsFreeSpaceCbf;}
	const double &getHsFreeSpaceCbfUlimit() const {return _hsFreeSpaceCbfUlimit;}
	const string &getHsSnipSizeVs() const {return _hsSnipSizeVs;}
	const string &getHsSnipSizeSpl() const {return _hsSnipSizeSpl;}
	const string &getHsSnipSizeCbf() const {return _hsSnipSizeCbf;}
	const double &getUflgSplKyojikuZaitan() const {return _uflgSplKyojikuZaitan;}
	const double &getUflgOutsideSplKyotyokuZaitan() const {return _uflgOutsideSplKyotyokuZaitan;}
	const double &getUflgInsideSplKyotyokuZaitan() const {return _uflgInsideSplKyotyokuZaitan;}
	const double &getWebSplKyojikuZaitan() const {return _webSplKyojikuZaitan;}
	const double &getWebSplHeightZaitan() const {return _webSplHeightZaitan;}
	const double &getLflgSplKyojikuZaitan() const {return _lflgSplKyojikuZaitan;}
	const double &getLflgSplKyotyokuZaitan() const {return _lflgSplKyotyokuZaitan;}

	void setUflgJc(const double &val) {_uflgJc = val;}
	void setWebJc(const double &val) {_webJc = val;}
	void setLflgJc(const double &val) {_lflgJc = val;}
	void setUflgZaitanKeijo(const EnTypeNo &val) {_uflgZaitanKeijo = val;}
	void setLflgZaitanKeijo(const EnTypeNo &val) {_lflgZaitanKeijo = val;}
	void setUpdownFlgZaitanKeijoTachiageRyo(const double &val) {_updownFlgZaitanKeijoTachiageRyo = val;}
	void setSolePlateKyotyokuFreeSpace(const double &val) {_solePlateKyotyokuFreeSpace = val;}
	void setSolePlateKyojikuFreeSpace(const double &val) {_solePlateKyojikuFreeSpace = val;}
	void setItatsugiZureRyo(const double &val) {_itatsugiZureRyo = val;}
	void setLflgKakuhukubuTaper(const double &val) {_lflgKakuhukubuTaper = val;}
	void setWebHoleSlopeLowerLimitGrd(const double &val) {_webHoleSlopeLowerLimitGrd = val;}
	void setShitenVsCutWu(const string &val) {_shitenVsCutWu = val;}
	void setShitenVsCutWd(const string &val) {_shitenVsCutWd = val;}
	void setShitenVsCutFu(const string &val) {_shitenVsCutFu = val;}
	void setKakutenVsCutWu(const string &val) {_kakutenVsCutWu = val;}
	void setKakutenVsCutWd(const string &val) {_kakutenVsCutWd = val;}
	void setKakutenVsCutFu(const string &val) {_kakutenVsCutFu = val;}
	void setMiddleVsCutWu(const string &val) {_middleVsCutWu = val;}
	void setMiddleVsCutWd(const string &val) {_middleVsCutWd = val;}
	void setMiddleVsFreeSpace(const double &val) {_middleVsFreeSpace = val;}
	void setHsFreeSpaceVs(const double &val) {_hsFreeSpaceVs = val;}
	void setHsFreeSpaceSpl(const double &val) {_hsFreeSpaceSpl = val;}
	void setHsFreeSpaceCbf(const double &val) {_hsFreeSpaceCbf = val;}
	void setHsFreeSpaceCbfUlimit(const double &val) {_hsFreeSpaceCbfUlimit = val;}
	void setHsSnipSizeVs(const string &val) {_hsSnipSizeVs = val;}
	void setHsSnipSizeSpl(const string &val) {_hsSnipSizeSpl = val;}
	void setHsSnipSizeCbf(const string &val) {_hsSnipSizeCbf = val;}
	void setUflgSplKyojikuZaitan(const double &val) {_uflgSplKyojikuZaitan = val;}
	void setUflgOutsideSplKyotyokuZaitan(const double &val) {_uflgOutsideSplKyotyokuZaitan = val;}
	void setUflgInsideSplKyotyokuZaitan(const double &val) {_uflgInsideSplKyotyokuZaitan = val;}
	void setWebSplKyojikuZaitan(const double &val) {_webSplKyojikuZaitan = val;}
	void setWebSplHeightZaitan(const double &val) {_webSplHeightZaitan = val;}
	void setLflgSplKyojikuZaitan(const double &val) {_lflgSplKyojikuZaitan = val;}
	void setLflgSplKyotyokuZaitan(const double &val) {_lflgSplKyotyokuZaitan = val;}

private:
	//ジョイントクリアランス
	double _uflgJc;		//上フランジのジョイントクリアランス
	double _webJc;		//ウェブのジョイントクリアランス
	double _lflgJc;		//下フランジのジョイントクリアランス
	//材端形状
	EnTypeNo _uflgZaitanKeijo;					//上フランジ材端形状
	EnTypeNo _lflgZaitanKeijo;					//下フランジ材端形状
	double _updownFlgZaitanKeijoTachiageRyo;	//上下フランジ材端形状立上げ量
	//ソールプレート
	double _solePlateKyotyokuFreeSpace;		//ソールプレートの橋直方向空き量
	double _solePlateKyojikuFreeSpace;		//ソールプレートの橋軸方向空き量
	//その他
	double _itatsugiZureRyo;				//板継ズレ量
	double _lflgKakuhukubuTaper;			//下フランジ拡幅部のテーパー勾配
	double _webHoleSlopeLowerLimitGrd;		//ウェブ孔の孔勾配の下限値
	//垂直補剛材
	string _shitenVsCutWu;					//支点上垂直補剛材の溶接辺側上側切欠
	string _shitenVsCutWd;					//支点上垂直補剛材の溶接辺側下側切欠
	string _shitenVsCutFu;					//支点上垂直補剛材の上側切欠
	string _kakutenVsCutWu;					//格点上垂直補剛材の溶接辺側上側切欠
	string _kakutenVsCutWd;					//格点上垂直補剛材の溶接辺側下側切欠
	string _kakutenVsCutFu;					//格点上垂直補剛材の上側切欠
	string _middleVsCutWu;					//中間垂直補剛材の溶接辺側上側切欠
	string _middleVsCutWd;					//中間垂直補剛材（圧縮タイプ）の溶接辺側下側切欠
	double _middleVsFreeSpace;				//中間垂直補剛材（引張タイプ）の下側空き量
	//水平補剛材
	double _hsFreeSpaceVs;					//水平補剛材の垂直補剛材部、横桁ウェブ部空き量
	double _hsFreeSpaceSpl;					//水平補剛材の添接板部空き量
	double _hsFreeSpaceCbf;					//水平補剛材の横桁フランジ部空き量
	double _hsFreeSpaceCbfUlimit;			//水平補剛材の横桁フランジからの高さ寸法上限
	string _hsSnipSizeVs;					//水平補剛材の垂直補剛材部のスニップサイズ
	string _hsSnipSizeSpl;					//水平補剛材の添接部のスニップサイズ
	string _hsSnipSizeCbf;					//水平補剛材の横桁フランジ部のスニップサイズ
	//添接板
	double _uflgSplKyojikuZaitan;			//上フランジ添接板の橋軸方向材端
	double _uflgOutsideSplKyotyokuZaitan;	//上フランジ外側添接板の橋直方向材端
	double _uflgInsideSplKyotyokuZaitan;	//上フランジ内側添接板の橋直方向材端
	double _webSplKyojikuZaitan;			//ウェブ添接板の橋軸方向材端
	double _webSplHeightZaitan;				//ウェブ添接板の高さ方向材端
	double _lflgSplKyojikuZaitan;			//下フランジ添接板の橋軸方向材端
	double _lflgSplKyotyokuZaitan;			//下フランジ添接板の橋直方向材端

};
