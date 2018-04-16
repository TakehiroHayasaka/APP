#pragma once

#include "DiafManager.h"
#include "AtdGirderCommon.h"
#include "AtdCrossBeamCommon.h"
#include "AtdDbInputStfAki.h"
#include "AtdDbInputGrdSpl.h"
#include "AtdDbInputHeight.h"
#include "AtdDbInputGrdMen.h"
#include "AtdDbInputCbeam.h"
#include "AtdDbInputKetatanLeng.h"
#include "AtdDbInputUseMaterial.h"
#include "AtdDbInputFillOption.h"
#include "AtdDbInputHaraikomiHoko.h"
#include "AtdDbInputCbeamConnSpl.h"
#include "AtdDbLineGrdZahyo.h"
#include "AtdDbLineCbeam.h"
#include "AtdDbLineGrdCamber.h"
#include "AtdDbSecGrdKaku.h"
#include "AtdDbSecGrdLeng.h"
#include "AtdDbSecScaleSpl.h"
#include "AtdDbSecGrd.h"
#include "AtdDbSecSolePl.h"
#include "AtdDbSecManhole.h"
#include "AtdDbSecCutData.h"
#include "AtdDbSecGrdKakuVst.h"
#include "AtdDbSecVstf.h"
#include "AtdDbSecVstfHaichi.h"
#include "AtdDbSecHstf.h"
#include "AtdDbSecHstfHaichi.h"
#include "AtdDbSecStfDir.h"
#include "AtdDbSecScaleFill.h"
#include "AtdDbSecCbeamSec.h"
#include "AtdDbSecCbeam.h"
#include "AtdDbSecCbeamStf.h"
#include "AtdDbSecCbeamKind.h"
#include "AtdDbSecCbeamVstfNum.h"
#include "AtdDbSplCommon.h"
#include "AtdDbSplFlg.h"
#include "AtdDbSplWeb.h"
#include "AtdDbGrdHstfLap.h"
#include "AtdDbGrdVstfHaichi.h"
#include "AtdDbGrdVstfKyori.h"
#include "AtdDbSecGrdHeightConstant.h"
#include "AtdDbSecGrdHeightVariable.h"
#include "AtdDbLineGrdPanel.h"
#include "AtdDbStructAll.h"
#include "AtdDbGrdSecPower.h"
// 2018/02/26 take Add Start
#include "AtdDbBaseData.h"
#include "AtdDbStatusHstf.h"
#include "AtdDbRangeHstf.h"
#include "AtdDbGrdPower.h"
// 2018/02/26 take Add End

class AtdDbToCsv
{
public:
	AtdDbToCsv(void) {}
	~AtdDbToCsv(void) {}

	JptErrorStatus dbToCsvSklKakuten(string& sekkeiFilePath, AtdDbSecGrdKaku& sgk, AtdDbLineGrdZahyo& lgz, AtdDbInputGrdMen& igm, std::ofstream& ofOb);
	// 2018/02/27 take Edit Start
	//JptErrorStatus dbToCsvSklOudan(AtdDbSecGrdKaku& sgk, AtdDbLineGrdZahyo& lgz, std::ofstream& ofOb);
	JptErrorStatus dbToCsvSklOudan(AtdDbStructAll& sta, AtdDbSecGrdKaku& sgk, AtdDbLineGrdZahyo& lgz, std::ofstream& ofOb);	//「構成・全体」追加
	// 2018/02/27 take Edit End
	JptErrorStatus dbToCsvSklCamber(AtdDbSecGrdKaku& sgk, AtdDbLineGrdCamber& lgc, std::ofstream& ofOb);
	// 2018/02/28 take Edit Start
	//JptErrorStatus dbToCsvSklVstf(AtdDbGrdVstfHaichi& gvh, AtdDbGrdVstfKyori& gvk, std::ofstream& ofOb);
	JptErrorStatus dbToCsvSklVstf(AtdDbSecVstfHaichi& svh, AtdDbLineGrdPanel& lgp, std::ofstream& ofOb);
	// 2018/02/28 take Edit End
	JptErrorStatus dbToCsvSklJoint(AtdDbSecGrdLeng& sgl, std::ofstream& ofOb);
	JptErrorStatus dbToCsvSklWebHeight(AtdDbInputGrdMen& igm, AtdDbSecGrdHeightConstant& sghc, AtdDbSecGrdHeightVariable& sghv, AtdDbLineGrdPanel& lgp, AtdDbInputKetatanLeng& ikl, std::ofstream& ofOb);
	// 2018/02/28 take Edit Start
	//JptErrorStatus dbToCsvSklHstf(AtdDbGrdHstfLap& ghl, std::ofstream& ofOb);
	JptErrorStatus dbToCsvSklHstf(AtdDbStatusHstf& sh, AtdDbRangeHstf &rh, std::ofstream& ofOb);
	// 2018/02/28 take Edit End
	JptErrorStatus dbToCsvGirderCommon(AtdGirderCommon& agc, AtdDbSecScaleSpl& sss, AtdDbInputGrdSpl& igs, std::ofstream& ofOb);
	JptErrorStatus dbToCsvGirderSection(AtdDbInputUseMaterial& ium, AtdDbSecGrd& sgd, AtdDbSecGrdLeng& sgl, std::ofstream& ofOb);
	JptErrorStatus dbToCsvGirderLflgWidening(AtdDbSecSolePl& ssp, std::ofstream& ofOb);
	JptErrorStatus dbToCsvGirderWebManhole(AtdDbSecManhole& smh, AtdDbSecCutData& scd, std::ofstream& ofOb);
	//2018/03/01 take Edit Start
	//JptErrorStatus dbToCsvGirderVstfId(AtdDbInputUseMaterial& ium, AtdDbSecVstf& svs, AtdDbSecGrdKakuVst& sgv, AtdDbSecVstfHaichi& svh, AtdDbInputStfAki& isa, AtdDbStructAll& sta, AtdDbGrdSecPower& gsp, std::ofstream& ofOb);
	JptErrorStatus dbToCsvGirderVstfId(AtdDbInputUseMaterial& ium, AtdDbSecVstf& svs, AtdDbSecGrdKakuVst& sgv, AtdDbSecVstfHaichi& svh, AtdDbInputStfAki& isa, AtdDbStructAll& sta, AtdDbGrdPower& gp, std::ofstream& ofOb);
	//2018/03/01 take Edit End
	JptErrorStatus dbToCsvGirderVstfSetSc(AtdDbSecGrdKakuVst& sgv, std::ofstream& ofOb);
	JptErrorStatus dbToCsvGirderVstfSetV(AtdDbSecVstfHaichi& svh, AtdDbSecStfDir& ssd, std::ofstream& ofOb);
	JptErrorStatus dbToCsvGirderHstfId(AtdDbInputUseMaterial& ium, AtdDbSecHstf& shf, std::ofstream& ofOb);
	JptErrorStatus dbToCsvGirderHstfSet(AtdDbSecHstfHaichi& shh, AtdDbSecStfDir& ssd, AtdDbSecVstfHaichi& svh, std::ofstream& ofOb);
	JptErrorStatus dbToCsvGirderFlgSplId(AtdDbInputUseMaterial& ium, AtdDbSplCommon& scm, AtdDbSplFlg& sfg, AtdDbSecScaleFill& ssf, AtdDbInputFillOption& ifo, std::ofstream& ofOb);
	JptErrorStatus dbToCsvGirderWebSplId(AtdDbInputUseMaterial& ium, AtdDbSplCommon& scm, AtdDbSplWeb& swb, AtdDbSecScaleFill& ssf, AtdDbInputFillOption& ifo, std::ofstream& ofOb);
	JptErrorStatus dbToCsvGirderSplSet(std::ofstream& ofOb);
	JptErrorStatus dbToCsvCrossbeamCommon(AtdCrossBeamCommon& acc, AtdDbInputCbeam& icb, std::ofstream& ofOb);
	JptErrorStatus dbToCsvCrossbeamSectionId(AtdDbInputUseMaterial& ium, AtdDbSecCbeam& scb, AtdDbSecCbeamSec& scs, AtdDbSplWeb& swb, AtdDbLineCbeam& lcb, std::ofstream& ofOb);
	JptErrorStatus dbToCsvCrossbeamHbeamId(AtdDbInputUseMaterial& ium, AtdDbSecCbeam& scb, AtdDbSecCbeamSec& scs, AtdDbSplWeb& swb, AtdDbLineCbeam& lcb, std::ofstream& ofOb);
	JptErrorStatus dbToCsvCrossbeamFlgSplId(AtdDbInputUseMaterial& ium, AtdDbSplCommon& scm, AtdDbSplFlg& sfg, std::ofstream& ofOb);
	JptErrorStatus dbToCsvCrossbeamConnFsplId(AtdDbInputUseMaterial& ium, AtdDbSplCommon& scm, AtdDbSplFlg& sfg, AtdDbSecScaleFill& ssf, AtdDbInputFillOption& ifo, AtdDbInputCbeamConnSpl& icc, std::ofstream& ofOb);
	JptErrorStatus dbToCsvCrossbeamWsplId(AtdDbInputUseMaterial& ium, AtdDbSplCommon& scm, AtdDbSplWeb& swb, AtdDbSecScaleFill& ssf, AtdDbInputFillOption& ifo, std::ofstream& ofOb);
	JptErrorStatus dbToCsvCrossbeamVstfId(AtdDbInputUseMaterial& ium, AtdDbSecCbeamStf& sct, AtdDbSecCbeamVstfNum& scv, std::ofstream& ofOb);
	JptErrorStatus dbToCsvCrossbeamSet(AtdDbSecCbeam& scb, AtdDbLineCbeam& lcb, AtdDbLineGrdPanel& lgp, AtdDbInputHaraikomiHoko& ihh, std::ofstream& ofOb);

private:
	int _ngMax;		//主桁数
	int _npMax;		//支点、格点、桁端数
	int _jointMax;	//ジョイント数
	// 2018/02/27 take Add Start
	int _nSpan;		//径間数
	// 2018/02/27 take Add End

	vector<string> _ketaNameameList;	//主桁名

	vector<string> _oudanNameWithKetatanList;	//端支点、中間支点、格点、桁端点	_npMax
	vector< vector<double> > _kakutenLengAdd;	//支点格点追加距離	npMax-1;
	// 2018/02/27 take Add Start
	vector< vector<double> > _midVstfLengAdd;	//主桁中間VSTF配置追加距離
	vector< vector<int> > _nKetaHstfUpperPrg;	//桁の水平補剛材上段数
	vector< vector<int> > _nKetaHstfLowerPrg;	//桁の水平補剛材下段数
	vector< vector<int> > _hstfPanelNumber;		//主桁HSTF配置パネル位置
	vector< vector<int> > _hstfPanelUpperPrg;	//主桁HSTF配置上段数
	vector< vector<int> > _hstfPanelLowerPrg;	//主桁HSTF配置下段数
	vector< vector<double> > _hstfLengAdd;		//主桁HSTF配置追加距離
	// 2018/02/27 take Add End

	vector<int> _oudanType;		//0:端支点、1:中間支点、2:格点、(桁端点除く)	_npMax-2
	vector<string> _oudanNameList;	//端支点、中間支点、格点、(桁端点除く)	_npMax-2

	vector<int> _shitenKakutenNo;	//支点格点番号(格点、桁端点除く)
	vector<string> _shitenNameList;	//端支点、中間支点(格点、桁端点除く)

	//主桁断面
	vector< vector<double> > _danmenLengAdd;	//断面追加距離		_jointMax+1
	vector< vector<double> > _danmenAtsuUflg;	//上フランジ板厚	_jointMax+1
	vector< vector<double> > _danmenHabaUflg;	//上フランジ板幅	_jointMax+1
	vector< vector<double> > _danmenAtsuWeb;	//ウェブ板厚		_jointMax+1
	vector< vector<double> > _danmenAtsuLflg;	//下フランジ板厚	_jointMax+1
	//主桁フランジ添接
	vector<int> _uflgJointNo;	//上フランジジョイント番号
	vector<string> _uflgIdName;	//上フランジ識別名
	vector<int> _lflgJointNo;	//下フランジジョイント番号
	vector<string> _lflgIdName;	//下フランジ識別名
	//主桁ウェブ添接
	vector<int> _webJointNo;	//ウエブジョイント番号
	vector<string> _webIdName;	//ウエブ識別名

	//主桁垂直補剛材ID登録
	vector<int> _grdVstfNovssc;		//主桁垂直補剛材識別ID
	vector<double> _grdVstfItaatsu;	//主桁垂直補剛材板厚
	//支点・格点垂直補剛材配置
	vector<int> _grdKakuVstfKetaNo;			//支点・格点主桁垂直補剛材桁番号
	vector<int> _grdKakuVstfKakutenNo;		//支点・格点主桁垂直補剛材格点番号
	vector<string> _grdKakuVstfKetaName;	//支点・格点主桁垂直補剛材桁名
	vector<string> _grdKakuVstfKakutenName;	//支点・格点主桁垂直補剛材格点名
	vector<EnFace> _grdKakuVstfFace;		//支点・格点主桁垂直補剛材配置面
	vector<double> _grdKakuVstfitaastu;		//支点・格点主桁垂直補剛材板厚

	//横桁本体
	vector<int> _crsHontaiType;		//横桁本体タイプ(0:ビルド材 1:Ｈ鋼)
	vector<int> _crsShiguchiType;	//横桁仕口タイプ(0:仕口 1:CONN)
	vector<int> _crsbeamNocrs;		//横桁本体仕口の番号
	vector<string> _crsbeamIdname;	//横桁本体仕口識別名
	vector<double> _habaUflg;	//UFLG_幅
	vector<double> _atsuUflg;	//UFLG_板厚
	vector<int> _zaiUflg;		//UFLG_材質
	vector<double> _atsuWeb;	//WEB_板厚
	vector<double> _atsuLflg;	//LFLG_板厚
	vector<double> _zaiLflg;	//LFLG_材質
	vector<double> _atsuTsukiVatf;	//突き合わせVSTF板厚
	vector<int> _crsNocjul;	//NOCJUL:上フランジ左側の添接番号
	vector<int> _crsNocjll;	//NOCJLL:下フランジ左側の添接番号
	vector<int> _crsNocjwl;	//NOCJWL:ウェブ左側の添接番号
	vector<int> _crsNocjur;	//NOCJUR:上フランジ右側の添接番号
	vector<int> _crsNocjlr;	//NOCJLR:下フランジ右側の添接番号
	vector<int> _crsNocjwr;	//NOCJWR:ウェブ右側の添接番号
	//支点上横桁フランジ添接
	vector<int> _crsFsplNoj;		//noj
	vector<int> _crsFsplPos;		//0:端支点、1:中間支点、2:格点
	vector<int> _crsFsplPupdw;		//0:上 1:下
	vector<string> _crsFsplIdname;	//横桁フランジ添接形状識別名
	//横桁コネクション・フランジ添接
	vector<int> _crsConnNoj;		//noj
	vector<int> _crsConnPos;		//0:端支点、1:中間支点、2:格点
	vector<int> _crsConnPupdw;		//0:上 1:下
	vector<string> _crsConnIdname;	//横桁コネクション・フランジ添接形状識別名
	//横桁ウェブ添接
	vector<int> _crsWsplNowj;		//nowj
	vector<int> _crsWsplPos;		//0:端支点、1:中間支点、2:格点
	vector<string> _crsWsplIdname;	//横桁ウェブ添接板識別名
	//横桁垂直補剛材
	vector<int> _crsVstfNvstc;		//横桁垂直補剛材の本数
	vector<int> _crsVstfNocrs;		//横桁垂直補剛材の番号
	vector<string> _crsVstfIdname;	//横桁垂直補剛材識別名

	// 2018/02/28 take Add Start
	void devideItmb( const int& itmb, vector<int>& itmbMemberList );							//ITMBから構成項目を取得
	bool existConfigurationItem( const int& targetItem, const vector<int>& itmbMemberList );	//構成項目が存在するか（true:あり, false:なし）
	// 2018/02/28 take Add End
	int findType(int jointNo, vector<int>& jointNoList, vector<int>& idNameList);
	double findDouble(int jointNo, vector<int>& jointNoList, vector<double>& idNameList);
	string findIdName(int jointNo, vector<int>& jointNoList, vector<string>& idNameList);
	double findGrdKakutenVstfItaatsu(const string& ketaName, const string& kakutenName, const EnFace& face);
	string makePitchGage(const int& npg, const double& pg);
	double getFillItaatsu(const double& atsuSa);

};

