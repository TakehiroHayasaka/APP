#pragma warning(disable : 4995)
/************************************************************************/
/*                                                                      */
/************************************************************************/
#include "stdafx.h"
#include "ApolloToDesign_main.h"
#include "AtdDbToCsv.h"

/*:**********************************************************************
機　　　能：
戻　り　値：0	正常終了
1	異常終了
備　　　考：
************************************************************************/
int ApolloToDesign_Main(
	string& sekkeiFilePath,
	string& seizuFilePath,
	string& csvFileName,
	AtdGirderCommon& agc,
	AtdCrossBeamCommon& acc
)
/*:*/
{
	CDaoDatabase dbFile;
	CString seizuFileName = seizuFilePath.c_str();
	CString sekkeiFileName = sekkeiFilePath.c_str();

	//データベース読み込み(製図MDB)
	dbFile.Open(seizuFileName,FALSE,FALSE);

	//入力･ＳＴＦのあき	
	AtdDbInputStfAki isa;
	isa.load(dbFile);

	//入力･主桁添接
	AtdDbInputGrdSpl igs;
	igs.load(dbFile);

	//入力･腹板高･変化点
	AtdDbInputHeight iht;
	iht.load(dbFile);

	//入力･主桁側面形状
	AtdDbInputGrdMen igm;
	igm.load(dbFile);

	//入力･横桁
	AtdDbInputCbeam icb;
	icb.load(dbFile);

	//入力･桁端長
	AtdDbInputKetatanLeng ikl;
	ikl.load(dbFile);

	//入力・使用材料データ
	AtdDbInputUseMaterial ium;
	ium.load(dbFile);

	//入力･フィラープレートオプション
	AtdDbInputFillOption ifo;
	ifo.load(dbFile);

	//入力･払い込み方向
	AtdDbInputHaraikomiHoko ihh;
	ihh.load(dbFile);

	//入力・横桁コネクション添接
	AtdDbInputCbeamConnSpl icc;
	icc.load(dbFile);

	//線形･主桁座標(縦桁/側縦桁)
	AtdDbLineGrdZahyo lgz;
	lgz.load(dbFile);

	//線形･横桁(対傾構/ブラケット)
	AtdDbLineCbeam lcb;
	lcb.load(dbFile);

	//線形･主桁(横桁/ブラケット)･キャンバー値
	AtdDbLineGrdCamber lgc;
	lgc.load(dbFile);

	//線形･主桁(横桁/ブラケット)･パネル長
	AtdDbLineGrdPanel lgp;
	lgp.load(dbFile);

	//断面･主桁格点名
	AtdDbSecGrdKaku sgk;
	sgk.load(dbFile);

	//断面･主桁の断面長
	AtdDbSecGrdLeng sgl;
	sgl.load(dbFile);

	//断面･スケール及び文字高さと材質仕様･主桁添接関係
	AtdDbSecScaleSpl sss;
	sss.load(dbFile);

	//断面･主桁断面
	AtdDbSecGrd sgd;
	sgd.load(dbFile);

	//断面･ソールプレート
	AtdDbSecSolePl ssp;
	ssp.load(dbFile);

	//断面･桁端部のマンホールのカット
	AtdDbSecManhole smh;
	smh.load(dbFile);

	//断面･カットデータ
	AtdDbSecCutData scd;
	scd.load(dbFile);

	//断面･主桁格点上VSTF
	AtdDbSecGrdKakuVst sgv;
	sgv.load(dbFile);

	//断面･VSTF断面
	AtdDbSecVstf svs;
	svs.load(dbFile);

	//断面･VSTF配置
	AtdDbSecVstfHaichi svh;
	svh.load(dbFile);

	//断面･HSTF断面
	AtdDbSecHstf shf;
	shf.load(dbFile);

	//断面･HSTF配置
	AtdDbSecHstfHaichi shh;
	shh.load(dbFile);

	// 2018/02/28 take Add Start
	//断面･HSTFの位置関係(段数変化長さ･段数)
	AtdDbStatusHstf sh;
	sh.load(dbFile);
	//断面･水平補剛材の入る範囲(追加距離)
	AtdDbRangeHstf rh;
	rh.load(dbFile);
	// 2018/02/28 take Add End

	//断面･詳細･補剛材の向き
	AtdDbSecStfDir ssd;
	ssd.load(dbFile);

	//断面･スケール及び文字高さと材質仕様･フィラープレート
	AtdDbSecScaleFill ssf;
	ssf.load(dbFile);

	//断面･主桁腹板(左右腹板高一定)
	AtdDbSecGrdHeightConstant sghc;
	sghc.load(dbFile);

	//断面･主桁腹板(桁高中心可変)
	AtdDbSecGrdHeightVariable sghv;
	sghv.load(dbFile);

	//断面･横桁･断面
	AtdDbSecCbeamSec scs;
	scs.load(dbFile);

	//断面･横桁
	AtdDbSecCbeam scb;
	scb.load(dbFile);

	//断面･横桁･補剛材断面数
	AtdDbSecCbeamStf sct;
	sct.load(dbFile);

	//断面･横桁種類数
	AtdDbSecCbeamKind sck;
	sck.load(dbFile);

	//断面･横桁･パネル毎のVSTF本数
	AtdDbSecCbeamVstfNum scv;
	scv.load(dbFile);

	//添接･線形添接共通データ
	AtdDbSplCommon scm;
	scm.load(dbFile);

	//添接･各添接･フランジ
	AtdDbSplFlg sfg;
	sfg.load(dbFile);

	//添接･各添接･ウェブ
	AtdDbSplWeb swb;
	swb.load(dbFile);

	//構成･全体
	AtdDbStructAll sta;
	sta.load(dbFile);

	dbFile.Close();

	//データベース読み込み(設計MDB)
	dbFile.Open(sekkeiFileName,FALSE,FALSE);

	// 2018/02/26 take Add Start
	//橋梁設計基準
	AtdDbBaseData bd;
	bd.load(dbFile);
	// 2018/02/26 take Add End

	// 2018/02/28 take Delete Start
	//製図のテーブルから取得
	//主桁水平補剛材ラップ範囲
	//AtdDbGrdHstfLap ghl;
	//ghl.load(dbFile);
	// 2018/02/28 take Delete end

	// 2018/02/28 take Delete Start
	//製図のテーブルから取得
	//主桁中間垂直補剛材配置データ
	//AtdDbGrdVstfHaichi gvh;
	//gvh.load(dbFile);
	// 2018/02/28 take Delete End

	// 2018/02/28 take Delete Start
	//製図のテーブルから取得
	//主桁中間垂直補剛材間隔データ
	//AtdDbGrdVstfKyori gvk;
	//gvk.load(dbFile);
	// 2018/02/28 take Delete End

	// 2018/03/01 take Delete Start
	//主桁断面力データ
	//AtdDbGrdSecPower gsp;
	//gsp.load(dbFile);
	// 2018/03/01 take Delete End

	// 2018/03/01 take Add Start
	AtdDbGrdPower gp;
	gp.load(dbFile);
	// 2018/03/01 take Add End
	dbFile.Close();

	// 2018/02/26 take Add Start
	// 橋梁形式
	/*bool brdgTypeFlg = false;
	if( bd.judgeBrdgType( brdgTypeFlg ) != JPT_OK ){
		return(1);
	}*/
	// 2018/02/26 take Add End

	//データ出力
	std::ofstream ofOb;
	ofOb.open(csvFileName.c_str(), std::ios::out);

	//２．基本形状
	AtdDbToCsv atc;

	//(１)格点座標　2018/02/26 take 〇
	atc.dbToCsvSklKakuten(sekkeiFilePath, sgk, lgz, igm, ofOb);

	//(２)横断線定義 2018/02/28 take 〇（支点部の判定ロジック修正）
	// 2018/02/27 take Edit Start
	//atc.dbToCsvSklOudan(sgk, lgz, ofOb);
	atc.dbToCsvSklOudan(sta, sgk, lgz, ofOb);
	// 2018/02/28 take Edit End

	//(３)キャンバー 2018/02/26 take 〇（ただし、格点番号は未確認）
	atc.dbToCsvSklCamber(sgk, lgc, ofOb);

	//(４)垂直補剛材位置 2018/02/26 take 〇（等間隔のみに変更）
	// 2018/02/28 take Edit Start
	//atc.dbToCsvSklVstf(gvh, gvk, ofOb);
	atc.dbToCsvSklVstf(svh, lgp, ofOb);
	// 2018/02/28 take Edit End

	//(５)ジョイント位置 2018/02/26 take 〇（ただし、断面番号は未確認）
	atc.dbToCsvSklJoint(sgl, ofOb);

	//(６)主桁ウェブ下端線 2018/02/27 take 〇（ウェブ高変化ロジック修正）
	atc.dbToCsvSklWebHeight(igm, sghc, sghv, lgp, ikl, ofOb);

	//(７)水平補剛材高さ 2018/02/28 take 〇（ロジック変更）
	// 2018/02/28 take Edit Start
	//atc.dbToCsvSklHstf(ghl, ofOb);
	atc.dbToCsvSklHstf(sh, rh, ofOb);

	// 2018/02/28 take Edit End

	//３．主桁

	//(１)共通詳細データ 2018/02/27 take 〇
	atc.dbToCsvGirderCommon(agc, sss, igs, ofOb);

	//(２)主桁断面 2018/02/27 take 〇
	atc.dbToCsvGirderSection(ium, sgd, sgl, ofOb);

	//(３)下フランジ拡幅部形状 2018/02/27 take 〇
	atc.dbToCsvGirderLflgWidening(ssp, ofOb);

	//(４)桁端マンホール形状 2018/02/27 take 〇
	atc.dbToCsvGirderWebManhole(smh, scd, ofOb);

	//(５)垂直補剛材ID登録 2018/03/01 take 〇？
	// 2018/03/01 take Edit Start
	//atc.dbToCsvGirderVstfId(ium, svs, sgv, svh, isa, sta, gsp, ofOb);
	atc.dbToCsvGirderVstfId(ium, svs, sgv, svh, isa, sta, gp, ofOb);
	// 2018/03/01 take Edit End

	//(６)支点・格点垂直補剛材配置 2018/03/01 take 〇
	atc.dbToCsvGirderVstfSetSc(sgv, ofOb);

	//(７)中間垂直補剛材配置 2018/03/01 take 〇
	atc.dbToCsvGirderVstfSetV(svh, ssd, ofOb);

	//(８)水平補剛材ID登録 2018/03/01 take 〇
	atc.dbToCsvGirderHstfId(ium, shf, ofOb);

	//(９)水平補剛材配置
	atc.dbToCsvGirderHstfSet(shh, ssd, svh, ofOb);

	//(１０)フランジ添接形状定義
	atc.dbToCsvGirderFlgSplId(ium, scm, sfg, ssf, ifo, ofOb);

	//(１１)ウェブ添接形状
	atc.dbToCsvGirderWebSplId(ium, scm, swb, ssf, ifo, ofOb);

	//(１２)添接配置
	atc.dbToCsvGirderSplSet(ofOb);

	//４．横桁

	//(１)共通詳細データ
	atc.dbToCsvCrossbeamCommon(acc, icb, ofOb);

	//(２)支点上横桁断面ID登録
	atc.dbToCsvCrossbeamSectionId(ium, scb, scs, swb, lcb, ofOb);

	//(３)格点上横桁Ｈ鋼ID登録
	atc.dbToCsvCrossbeamHbeamId(ium, scb, scs, swb, lcb, ofOb);

	//(４)支点上横桁フランジ添接形状定義
	atc.dbToCsvCrossbeamFlgSplId(ium, scm, sfg, ofOb);

	//(５)コネクション・フランジ添接形状定義
	atc.dbToCsvCrossbeamConnFsplId(ium, scm, sfg, ssf, ifo, icc, ofOb);

	//(６)ウェブ添接形状定義
	atc.dbToCsvCrossbeamWsplId(ium, scm, swb, ssf, ifo, ofOb);

	//(７)垂直補剛材ID登録
	atc.dbToCsvCrossbeamVstfId(ium, sct, scv, ofOb);

	//(８)横桁配置
	atc.dbToCsvCrossbeamSet(scb, lcb, lgp, ihh, ofOb);

	ofOb.close();

	return(0);
}

