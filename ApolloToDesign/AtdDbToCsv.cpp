#include "stdafx.h"
#include "AtdDbToCsv.h"
#include "AlnLinearCoordinateFile\\AlnLinearCoordinateFile.h"
#include <set>

//２．基本形状

//(１)格点座標
JptErrorStatus AtdDbToCsv::dbToCsvSklKakuten(
	string& sekkeiFilePath,
	AtdDbSecGrdKaku& sgk,	//断面･主桁格点名
	AtdDbLineGrdZahyo& lgz,	//線形･主桁座標(縦桁/側縦桁)
	AtdDbInputGrdMen& igm,	//入力･主桁側面形状
	std::ofstream& ofOb)
{
	StringTokenizer strToken(sekkeiFilePath,"\\.");
	AlnLinearCoordinateFile alcf;
	if(strToken.size2() > 1) {
		string koji = strToken[strToken.size2()-2];
		string alignPath = "Align\\" + koji + ".sug";
		alcf.load(alignPath);
		if(alcf.size() < 1) {
			alignPath = "Align\\" + koji + "01.sug";
			alcf.load(alignPath);
		}
	}
	StringHandler sh;
	DiafSklKakuten dsk;
	_ngMax = 0;
	_npMax = 0;
	for(int i=0;i<lgz.size();i++) {
		AtdDbLineGrdZahyoItem lgzItem;
		lgz.getAt(i, lgzItem);
		int nostr = lgzItem.getNostr();		//NOSTR
		int nopt = lgzItem.getNopt();		//NOPT
		// 2018/02/15 take Edit Start
		// mm → m
		double xu = lgzItem.getXu() / 1000;
		double yu = lgzItem.getYu() / 1000;
		double zu = lgzItem.getZu() / 1000;
		//double xu = lgzItem.getXu();		//XU
		//double yu = lgzItem.getYu();		//YU
		//double zu = lgzItem.getZu();		//ZU
		// 2018/02/15 take Edit End
		EnHokan heimenHokan = S_TYOKUSEN;			//平面の補間方法
		EnHokan sokumenHokan = A_SANJIKYOKUSEN;		//側面の補間方法
		for(int j=0;j<igm.size();j++) {
			AtdDbInputGrdMenItem igmItem;
			igm.getAt(j, igmItem);
			int strno = igmItem.getStrno();		//STRNO 桁名
			if(nostr != strno) {
				continue;
			}
			int itpuw = igmItem.getItpuw();		//ITPUW 側面補間方法
			if(itpuw != 3) {
				sokumenHokan = S_TYOKUSEN;
			}
		}
		if(nostr > _ngMax) {
			_ngMax = nostr;
		}
		if(nopt > _npMax) {
			_npMax = nopt;
		}
		string odname = "";
		if(nopt == 1) {
			odname = "GE1";
		} else {
			for(int j=0;j<sgk.size();j++) {
				AtdDbSecGrdKakuItem atdDbSecGrdKakuItem;
				sgk.getAt(j, atdDbSecGrdKakuItem);
				int nostr2 = atdDbSecGrdKakuItem.getNostr();
				int nocrs2 = atdDbSecGrdKakuItem.getNocrs();
				if(nostr == nostr2 && nopt-1 == nocrs2) {
					odname = atdDbSecGrdKakuItem.getOdname();
					break;
				}
			}
			if(odname == "") {
				odname = "GE2";
			}
		}
		string ketaName = "G" + sh.toString(nostr);	//桁名
		for(int k=0;k<alcf.size();k++) {
			AlnLinearCoordinateData lcd;
			alcf.getAt(k, lcd);
			string lineName = lcd.getLineName();
			string lineName2 = lineName;
			if(lineName2.size() == ketaName.size()+1) {
				lineName2.erase(lineName2.begin() + 1);
			}
			if(lineName == ketaName || lineName2 == ketaName) {
				for(int l=0;l<lcd.size();l++) {
					AlnPointData point;
					lcd.getAt(l, point);
					string pointName = point.getPointName();
					if(odname == pointName) {
						double radius = point.getRadius();
						if(fabs(radius) < 0.00001) {
							heimenHokan = S_TYOKUSEN;		//直線
						} else {
							heimenHokan = A_SANJIKYOKUSEN;	//三次曲線(スプライン)
						}
						break;
					}
				}
			}
		}
		DiafSklKakutenItem dskItem;
		dskItem.setKetaName(ketaName);
		dskItem.setKakutenNo(nopt);
		dskItem.setXZahyou(xu);
		dskItem.setYZahyou(yu);
		dskItem.setWebzZahyou(zu);
		dskItem.setHeimenHokan(heimenHokan);
		dskItem.setSokumenHokan(sokumenHokan);
		dsk.append(dskItem);
	}
	dsk.sort();
	dsk.toCsv(ofOb);
	//桁名
	for(int i=0;i<_ngMax;i++) {
		string ketaName = "G" + sh.toString(i+1);
		_ketaNameameList.push_back(ketaName);
	}

	return JPT_OK;
}

//(２)横断線定義
// 2018/02/27 take Edit Start
//JptErrorStatus AtdDbToCsv::dbToCsvSklOudan(
//	AtdDbSecGrdKaku& sgk,	//断面･主桁格点名
//	AtdDbLineGrdZahyo& lgz,	//線形･主桁座標(縦桁/側縦桁)
//	std::ofstream& ofOb)
JptErrorStatus AtdDbToCsv::dbToCsvSklOudan(
	AtdDbStructAll& sta,	//構成・全体
	AtdDbSecGrdKaku& sgk,	//断面･主桁格点名
	AtdDbLineGrdZahyo& lgz,	//線形･主桁座標(縦桁/側縦桁)
	std::ofstream& ofOb)
// 2018/02/26 take Edit End
{
	StringHandler sh;
	DiafSklOudan dso;

	// 2018/02/27 take Add Start
	int nSpanCount = 0;
	AtdDbStructAllItem staItem;	//径間数
	sta.getAt(0, staItem);
	_nSpan = staItem.getNspan();
	// 2018/02/27 take Add End
	for(int i=0;i<_npMax;i++) {
		int itmbX = -1;
		vector<int> itmbMemberList;				//ITMB構成項目
		for(int j=0;j<lgz.size();j++) {
			AtdDbLineGrdZahyoItem lgzItem;
			lgz.getAt(j, lgzItem);
			int nopt = lgzItem.getNopt();		//NOPT
			int itmb = lgzItem.getItmb();		//ITMB
			// 2018/02/28 ITMBから構成情報を取得
			devideItmb( itmb, itmbMemberList );
			if(nopt == i+1) {
				itmbX = itmb;
				// 2018/02/28 take Add Start
				if(existConfigurationItem( 2, itmbMemberList ) == true){
					nSpanCount += 1;	//支点部をカウント
				}
				// 2018/02/28 take Add End
				break;
			}
			// 2018/02/28 take Add Start
			itmbMemberList.clear();
			// 2018/02/28 take Add End
		}
		if(itmbX == -1) {
//err
		}
		CString odanName = "";
		if(i == 0) {
			odanName = "GE1";
		} else if(i == _npMax-1) {
			odanName = "GE2";
		} else {
			for(int j=0;j<sgk.size();j++) {
				AtdDbSecGrdKakuItem sgkItem;
				sgk.getAt(j, sgkItem);
				int nocrs = sgkItem.getNocrs();		//NOCRS
				CString odname = sgkItem.getOdname();	//ODNAME 横断線名
				if(nocrs == i) {
					odanName = odname;
					break;
				}
			}
			if(odanName == "") {
				//err
			}
			// 2018/02/27 take Edit Start
			//if(itmbX == 6) {	//端支点
			//	_oudanType.push_back(0);
			//} else if(itmbX == 2) {	//中間支点
			//	_oudanType.push_back(1);
			//} else if(itmbX == 4) {	//格点
			//	_oudanType.push_back(2);
			//}
			if(existConfigurationItem( 2, itmbMemberList ) == true){
				if( nSpanCount == 1 || nSpanCount - 1 == _nSpan ){	//端支点
					_oudanType.push_back(0);
				}else{	//中間支点
					_oudanType.push_back(1);
				}
			// 2018/02/27 take Edit End
			} else if(existConfigurationItem( 4, itmbMemberList ) == true) {	//格点
				_oudanType.push_back(2);
			}
			_oudanNameList.push_back(odanName.GetBuffer());
		}
		_oudanNameWithKetatanList.push_back(odanName.GetBuffer());
		string oudanLineName = odanName.GetBuffer();	//横断線名
		EnOudanLineType oudanLineType;		//横断線種類
		// 2018/02/28 take Edit Start
		//if(itmbX == 128) {		//桁端
		//	oudanLineType = KETATAN;
		//} else if(itmbX == 6) {	//端支点
		//	oudanLineType = TANSHITEN;
		//	_shitenKakutenNo.push_back(i);
		//	_shitenNameList.push_back(odanName.GetBuffer());
		//} else if(itmbX == 2) {	//中間支点
		//	oudanLineType = TYUUKANSHITEN;
		//	_shitenKakutenNo.push_back(i);
		//	_shitenNameList.push_back(odanName.GetBuffer());
		//} else if(itmbX == 4) {	//格点
		//	oudanLineType = KAKUTEN;
		//} else {
		////err
		//}
		if(existConfigurationItem( 128, itmbMemberList ) == true) {		//桁端
			oudanLineType = KETATAN;
		}else if(existConfigurationItem( 2, itmbMemberList ) == true){
			if( nSpanCount == 1 || nSpanCount - 1 == _nSpan ){			//端支点
				oudanLineType = TANSHITEN;
			}else{														//中間支点
				oudanLineType = TYUUKANSHITEN;
			}
			_shitenKakutenNo.push_back(i);
			_shitenNameList.push_back(odanName.GetBuffer());
		
		} else if(existConfigurationItem( 4, itmbMemberList ) == true) {	//格点
			oudanLineType = KAKUTEN;
		} else {
//err
		}
		// 2018/02/28 take Edit End
		DiafSklOudanItem dsoItem;
		dsoItem.setOudanLineName(oudanLineName);
		dsoItem.setOudanLineType(oudanLineType);
		int kakutenNo = i+1;			//格点番号
		for(int n=0;n<_ngMax;n++) {
			string ketaName = _ketaNameameList[n];	//桁名
			DiafSklOudanData sklOudanData;
			sklOudanData.setKetaName(ketaName);
			sklOudanData.setKakutenNo(kakutenNo);
			dsoItem.append(sklOudanData);
		}
		dso.append(dsoItem);
	}
	dso.toCsv(ofOb);

	return JPT_OK;
}

//(３)キャンバー
JptErrorStatus AtdDbToCsv::dbToCsvSklCamber(
	AtdDbSecGrdKaku& sgk,		//断面･主桁格点名
	AtdDbLineGrdCamber& lgc,	//線形･主桁(横桁/ブラケット)･キャンバー値
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafSklCamber dsc;
	for(int i=0;i<sgk.size();i++) {
		AtdDbSecGrdKakuItem sgkItem;
		sgk.getAt(i, sgkItem);
		int nostr = sgkItem.getNostr();		//NOSTR
		int nocrs = sgkItem.getNocrs();		//NOCRS
		CString getOdname = sgkItem.getOdname();	//ODNAME 横断線名
		double zCamber = 0.0;			//格点のZキャンバー
		for(int j=0;j<lgc.size();j++) {
			AtdDbLineGrdCamberItem lgcItem;
			lgc.getAt(j, lgcItem);
			int nostr2 = lgcItem.getNostr();		//NOSTR 桁名
			int nopnl2 = lgcItem.getNopnl();		//NOPNL
			if(nostr == nostr2 && nocrs == nopnl2) {
				zCamber = lgcItem.getZcamz();	//ZCAMZ Zキャンバー
				break;
			}
		}
		DiafSklCamberItem dscItem;
		string ketaName = _ketaNameameList[nostr-1];	//桁名
		string kakutenName = getOdname;			//格点名
		int kakutenNo = nocrs;					//格点番号
		dscItem.setKetaName(ketaName);
		dscItem.setKakutenName(kakutenName);
		dscItem.setKakutenNo(kakutenNo);
		dscItem.setZCamber(zCamber);
		dsc.append(dscItem);
	}
	dsc.sort();
	dsc.toCsv(ofOb);

	return JPT_OK;
}

//(４)垂直補剛材位置
// 2018/02/28 take Edit Start
// 使用するテーブルを変更（等割のみ）
//JptErrorStatus AtdDbToCsv::dbToCsvSklVstf(
//	AtdDbGrdVstfHaichi& gvh,	//主桁中間垂直補剛材配置データ
//	AtdDbGrdVstfKyori& gvk,		//主桁中間垂直補剛材間隔データ
//	std::ofstream& ofOb)
//{
//	StringHandler sh;
//	DiafSklVstf dsv;
//	for(int i=0;i<_ngMax;i++) {
//		for(int j=0;j<_npMax-1;j++) {
//			int numvstf = 0;
//			for(int k=0;k<gvh.size();k++) {
//				AtdDbGrdVstfHaichiItem gvhItem;
//				gvh.getAt(k, gvhItem);
//				int nostr = gvhItem.getNostr();		//部材線コード
//				int nopanel = gvhItem.getNopanel();	//パネルコード
//				if(nostr != i+1) {
//					continue;
//				}
//				int pno = nopanel % 1000;	//パネル番号
//				if(pno != j+1) {
//					continue;
//				}
//				numvstf = gvhItem.getNumvstf();	//垂直補剛材本数
//				break;
//			}
//			if(numvstf < 1) {
//				continue;
//			}
//			// 2018/02/26 take Add Start
//			bool equalIntervalsFlg = true;		// 格点間にVSTFが等間隔配置されるか（true：等間隔,false：不等間隔）
//			// 2018/02/26 take Add End
//			double interval[10];
//			for(int l=0;l<10;l++) {
//				interval[l] = 0.0;	//距離1-10
//			}
//			for(int k=0;k<gvk.size();k++) {
//				AtdDbGrdVstfKyoriItem gvkItem;
//				gvk.getAt(k, gvkItem);
//				int nostr = gvkItem.getNostr();			//部材線コード
//				int nopanel = gvkItem.getNopanel();		//パネルコード
//				if(nostr != i+1) {
//					continue;
//				}
//				int pno = nopanel % 1000;	//パネル番号
//				if(pno != j+1) {
//					continue;
//				}
//				int vstfNum = gvkItem.getVstfNum();		//垂直補剛材間隔番号
//				double vstfkyori = gvkItem.getVstfkyori();	//垂直補剛材間隔
//				interval[vstfNum-1] = vstfkyori;
//				// 2018/02/26 take Add Start
//				if( equalIntervalsFlg == true && vstfNum > 1){
//					if( interval[vstfNum-2] != interval[vstfNum-1] ){
//						equalIntervalsFlg = false;
//					}
//				}
//				// 2018/02/26 take Add End
//			}
//			string ketaName = _ketaNameameList[i];				//桁名
//			string startSideKakutenName = _oudanNameList[j];	//始側格点名
//			string endSideKakutenName = _oudanNameList[j+1];	//終側格点名
//			int startSideKakutenNo = j+1;			//始側格点番号
//			int endSideKakutenNo = j+2;				//終側格点番号
//			int panelNo = j+1;						//パネル番号
//			int bunkatsuNum = numvstf + 1;		//分割数
//			// 2018/2/26 take Edit Start
//			/*for(int l=0;l<bunkatsuNum;l++) {
//				if(interval[l] > 1.0) {
//					bunkatsuNum = -1;
//					break;
//				}
//			}*/
//			if(equalIntervalsFlg == true){	// VSTFの配置が等間隔の場合
//				for(int l=0;l<10;l++) {
//					interval[l] = -1.0;		
//				}
//			}else{							// VSTFの配置が不等間隔の場合
//				bunkatsuNum = -1;
//			}
//			// 2018/2/26 take Edit End
//			DiafSklVstfItem dsvItem;
//			dsvItem.setKetaName(ketaName);
//			dsvItem.setStartSideKakutenName(startSideKakutenName);
//			dsvItem.setEndSideKakutenName(endSideKakutenName);
//			dsvItem.setStartSideKakutenNo(startSideKakutenNo);
//			dsvItem.setEndSideKakutenNo(endSideKakutenNo);
//			dsvItem.setPanelNo(panelNo);
//			dsvItem.setBunkatsuNum(bunkatsuNum);
//			dsvItem.setInterval1(interval[0]);
//			dsvItem.setInterval2(interval[1]);
//			dsvItem.setInterval3(interval[2]);
//			dsvItem.setInterval4(interval[3]);
//			dsvItem.setInterval5(interval[4]);
//			dsvItem.setInterval6(interval[5]);
//			dsvItem.setInterval7(interval[6]);
//			dsvItem.setInterval8(interval[7]);
//			dsvItem.setInterval9(interval[8]);
//			dsvItem.setInterval10(interval[9]);
//			dsv.append(dsvItem);
//		}
//	}
//	dsv.sort();
//	dsv.toCsv(ofOb);
//
//	return JPT_OK;
//}
JptErrorStatus AtdDbToCsv::dbToCsvSklVstf(
	AtdDbSecVstfHaichi& svh,	//断面･VSTF配置
	AtdDbLineGrdPanel& lgp,		//線形･主桁(横桁/ブラケット)･パネル長
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafSklVstf dsv;
	// 2018/02/28 take Add Start
	_midVstfLengAdd.resize( _ngMax );
	_hstfLengAdd.resize( _ngMax );
	_hstfPanelNumber.resize( _ngMax );
	// 2018/02/28 take Add End
	for(int i=0;i<_ngMax;i++) {
		// 2018/02/28 take Add Start
		double vstfLocation = 0.0;	//VSTFの位置
		// 2018/02/28 take Add End
		for(int j=0;j<_npMax-1;j++) {
			int numvstf = 0;
			for(int k=0;k<svh.size();k++) {
				AtdDbSecVstfHaichiItem svhItem;
				svh.getAt(k, svhItem);
				int nostr = svhItem.getNogrd();		//部材線コード
				int pno = svhItem.getNopnl();		//パネルコード
				if(nostr != i+1) {
					continue;
				}
				if(pno != j+1) {
					continue;
				}
				numvstf = svhItem.getNvst();	//垂直補剛材本数
				break;
			}
			if(numvstf < 1) {
				continue;
			}
			// 2018/02/28 take Add Start
			for(int k=0;k<lgp.size();k++) {		//VSTFの配置位置を取得（水平補剛材位置で使用）
				AtdDbLineGrdPanelItem lgpItem;
				lgp.getAt(k, lgpItem);
				int strcode = lgpItem.getStrcode();
				if( strcode != 3 ){
					continue;
				}
				int nostr = lgpItem.getNostr();		//主桁番号
				int pno = lgpItem.getNopnl();		//格点番号
				if(nostr != i+1){
					continue;
				}
				if(pno != j+1) {
					continue;
				}
				//
				double vstfInterval = lgpItem.getRlp() / (numvstf + 1);		//パネル間のVSTF配置間隔
				double halfVstfInterval = vstfInterval / 2;					//配置間隔の中間位置(垂直補剛材の中間位置)

				for(int l=0;l<numvstf;l++){
					vstfLocation += vstfInterval;
					_midVstfLengAdd[i].push_back(vstfLocation);
					_hstfLengAdd[i].push_back(vstfLocation - halfVstfInterval);
					_hstfPanelNumber[i].push_back(pno);
				}
				_hstfLengAdd[i].push_back(vstfLocation + halfVstfInterval);
				_hstfPanelNumber[i].push_back(pno);
				vstfLocation += vstfInterval;
			}
			// 2018/02/28 take Add End

			double interval[10];
			for(int l=0;l<10;l++) {
				interval[l] = -1.0;		//パネル間のVSTF本数のみ定義されているため、等間隔
			}
			
			string ketaName = _ketaNameameList[i];				//桁名
			string startSideKakutenName = _oudanNameList[j];	//始側格点名
			string endSideKakutenName = _oudanNameList[j+1];	//終側格点名
			int startSideKakutenNo = j+1;			//始側格点番号
			int endSideKakutenNo = j+2;				//終側格点番号
			int panelNo = j+1;						//パネル番号
			int bunkatsuNum = numvstf + 1;		//分割数
			
			DiafSklVstfItem dsvItem;
			dsvItem.setKetaName(ketaName);
			dsvItem.setStartSideKakutenName(startSideKakutenName);
			dsvItem.setEndSideKakutenName(endSideKakutenName);
			dsvItem.setStartSideKakutenNo(startSideKakutenNo);
			dsvItem.setEndSideKakutenNo(endSideKakutenNo);
			dsvItem.setPanelNo(panelNo);
			dsvItem.setBunkatsuNum(bunkatsuNum);
			dsvItem.setInterval1(interval[0]);
			dsvItem.setInterval2(interval[1]);
			dsvItem.setInterval3(interval[2]);
			dsvItem.setInterval4(interval[3]);
			dsvItem.setInterval5(interval[4]);
			dsvItem.setInterval6(interval[5]);
			dsvItem.setInterval7(interval[6]);
			dsvItem.setInterval8(interval[7]);
			dsvItem.setInterval9(interval[8]);
			dsvItem.setInterval10(interval[9]);
			dsv.append(dsvItem);
		}
	}
	dsv.sort();
	dsv.toCsv(ofOb);

	return JPT_OK;
}
// 2018/02/28take Edit End

//(５)ジョイント位置
JptErrorStatus AtdDbToCsv::dbToCsvSklJoint(
	AtdDbSecGrdLeng& sgl,	//断面･主桁の断面長
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafSklJoint dsj;
	_jointMax = 0;
	for(int i=0;i<sgl.size();i++) {
		AtdDbSecGrdLengItem sglItem;
		sgl.getAt(i, sglItem);
		int nosec = sglItem.getNosec();		//NOSEC ジョイント名
		if(nosec > _jointMax) {
			_jointMax = nosec;
		}
	}
	for(int i=0;i<sgl.size();i++) {
		AtdDbSecGrdLengItem sglItem;
		sgl.getAt(i, sglItem);
		int nogrd = sglItem.getNogrd();		//NOGRD 桁名
		int nosec = sglItem.getNosec();		//NOSEC ジョイント名
		if(_jointMax == nosec) {
			continue;
		}
		double rlsec = sglItem.getRlsec();	//RLSEC ブロック長
		string ketaName = _ketaNameameList[nogrd-1];		//桁名
		string jointName = "J" + sh.toString(nosec);		//ジョイント名
		DiafSklJointItem dsjItem;
		dsjItem.setKetaName(ketaName);
		dsjItem.setJointName(jointName);
		dsjItem.setJointNo(nosec);
		dsjItem.setBlockLength(rlsec);
		dsj.append(dsjItem);
	}
	_jointMax--;
	dsj.sort();
	dsj.toCsv(ofOb);

	return JPT_OK;
}

//(６)主桁ウェブ下端線
JptErrorStatus AtdDbToCsv::dbToCsvSklWebHeight(
	AtdDbInputGrdMen& igm,				//入力･主桁側面形状
	AtdDbSecGrdHeightConstant& sghc,	//断面･主桁腹板(左右腹板高一定)
	AtdDbSecGrdHeightVariable& sghv,	//断面･主桁腹板(桁高中心可変)
	AtdDbLineGrdPanel& lgp,				//線形･主桁(横桁/ブラケット)･パネル長
	AtdDbInputKetatanLeng& ikl,			//入力･桁端長
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafSklWebHeight dswh;
	_kakutenLengAdd.resize(_ngMax);
	for(int i=0;i<_ngMax;i++) {
		// 2018/02/27 take Edit Start
		//_kakutenLengAdd[i].resize(_npMax);
		_kakutenLengAdd[i].resize(_npMax - 1);
		// 2018/02/27 take Edit End
		int itpww = 3;
		for(int k=0;k<igm.size();k++) {
			AtdDbInputGrdMenItem igmItem;
			igm.getAt(k, igmItem);
			int strno = igmItem.getStrno();		//STRNO 桁名
			if(strno != i+1) {
				continue;
			}
			itpww = igmItem.getItpww();		//ITPWW 側面補間方法
		}
		string ketaName = _ketaNameameList[i];			//桁名
		for(int j=0;j<_npMax-1;j++) {
			double rlp = 0.0;
			if(j == 0 || j == _npMax-2) {
				int idx = ikl.find(i+1);
				if(idx > -1) {
					AtdDbInputKetatanLengItem atdDbInputKetatanLengItem;
					ikl.getAt(idx, atdDbInputKetatanLengItem);
					if(j == 0) {
						rlp = atdDbInputKetatanLengItem.getRls();
					} else {
						rlp = atdDbInputKetatanLengItem.getRle();
					}
				}
			} else {
				rlp = lgp.findRlp(3, i+1, j);
			}
			_kakutenLengAdd[i][j] = rlp;
		}
		_kakutenLengAdd[i][1] -= _kakutenLengAdd[i][0];
		_kakutenLengAdd[i][_npMax-3] -= _kakutenLengAdd[i][_npMax-2];
		for(int j=1;j<_npMax-1;j++) {
			_kakutenLengAdd[i][j] += _kakutenLengAdd[i][j-1];
		}

		// 2018/02/27 take Add Start
		double startLocation = 0.0;					//ウェブ高が変化する場合に使用（パネル間の位置関係を示す）
		double endLocation = _kakutenLengAdd[i][1];
		// 2018/02/27 take Add end
		for(int j=0;j<_npMax;j++) {
			string kakutenName = _oudanNameWithKetatanList[j];	//格点名
			int kakutenNo = j+1;						//格点番号
			double teigiPositionDim = 0.0;				//定義位置寸法
			double webHeight = 0.0;						//ウェブ高
			EnHokan webHeightHokanMethod = S_TYOKUSEN;	//ウェブ高補間方法
			if(itpww == 0 || itpww == 3) {			//桁高一定
				for(int k=0;k<sghc.size();k++) {
					AtdDbSecGrdHeightConstantItem sghcItem;
					sghc.getAt(k, sghcItem);
					int nogrd = sghcItem.getNogrd();	//NOGRD
					if(nogrd == i+1) {
						webHeight = sghcItem.getHweb();	//HWEB ウェブ高
						break;
					}
				}
				// 2018/02/26 take Add Start
				DiafSklWebHeightItem dswhItem;
				dswhItem.setKetaName(ketaName);
				dswhItem.setKakutenName(kakutenName);
				dswhItem.setKakutenNo(kakutenNo);
				dswhItem.setTeigiPositionDim(teigiPositionDim);
				dswhItem.setWebHeight(webHeight);
				dswhItem.setWebHeightHokanMethod(webHeightHokanMethod);
				dswh.append(dswhItem);
				// 2018/02/26 take Add End
			} else {	//桁高変化
				double rcweb = 0.0;
				int itplc = 0;
				for(int k=0;k<sghv.size();k++) {	//断面･主桁腹板(桁高中心可変)
					AtdDbSecGrdHeightVariableItem sghvItem;
					sghv.getAt(k, sghvItem);
					int nogrd = sghvItem.getNogrd();		//NOGRD
					if(nogrd != i+1) {
						continue;
					}
					rcweb = sghvItem.getRcweb();	//RCWEB
					webHeight = sghvItem.getHcweb();	//HCWEB
					itplc = sghvItem.getItplc();		//ITPLC
//					int itphc = sghvItem.getItphc();	//ITPHC

					// 2018/02/27 take Add Start
					if( j == 0 ){								// 始側桁端部（始側支点部の桁高と同じ値）
						sghv.getAt(0, sghvItem);
						teigiPositionDim = 0.0;
						webHeight = sghvItem.getHcweb();
					}else if( j == _npMax - 1 ){				// 終側桁端部（終側支点部の桁高と同じ値）
						sghv.getAt((sghv.size() - 1), sghvItem);
						teigiPositionDim = 0.0;
						webHeight = sghvItem.getHcweb();
					}else if( rcweb == startLocation ){								//格点部の桁高
						teigiPositionDim = 0.0;
					}else if( startLocation < rcweb && rcweb < endLocation ){		//格点間の桁高
						teigiPositionDim = rcweb - startLocation;
					}else if( rcweb > endLocation ){								//次の格点に移動
						startLocation = _kakutenLengAdd[i][j];
						endLocation = _kakutenLengAdd[i][j+1];
						break;
					}else{
						continue;
					}

					if(itplc == 3) {
						EnHokan webHeightHokanMethod = A_SANJIKYOKUSEN;		//ウェブ高補間方法
					}
					DiafSklWebHeightItem dswhItem;
					dswhItem.setKetaName(ketaName);
					dswhItem.setKakutenName(kakutenName);
					dswhItem.setKakutenNo(kakutenNo);
					dswhItem.setTeigiPositionDim(teigiPositionDim);
					dswhItem.setWebHeight(webHeight);
					dswhItem.setWebHeightHokanMethod(webHeightHokanMethod);
					dswh.append(dswhItem);
					if( j == 0 || j == _npMax - 1 ){						// 桁端部の場合
						break;
					}
					// 2018/02/27 take Add End
					// 2018/02/27 take Delete Start
					//break;
					// 2018/02/27 take Delete End
				}
				// 2018/02/27 take Delete Start
				//if(itplc == 3) {
				//	EnHokan webHeightHokanMethod = A_SANJIKYOKUSEN;		//ウェブ高補間方法
				//}
				////定義位置寸法
				//teigiPositionDim = _kakutenLengAdd[i][j] + rcweb;
				// 2018/02/27 take Delete End
			}
			// 2018/02/27 take Delete Start
			/*DiafSklWebHeightItem dswhItem;
			dswhItem.setKetaName(ketaName);
			dswhItem.setKakutenName(kakutenName);
			dswhItem.setKakutenNo(kakutenNo);
			dswhItem.setTeigiPositionDim(teigiPositionDim);
			dswhItem.setWebHeight(webHeight);
			dswhItem.setWebHeightHokanMethod(webHeightHokanMethod);
			dswh.append(dswhItem);*/
			// 2018/02/27 take Delete End
		}
	}
	dswh.sort();
	dswh.toCsv(ofOb);

	return JPT_OK;
}

// 2018/02/28 take Edit Start
//(７)水平補剛材高さ
//JptErrorStatus AtdDbToCsv::dbToCsvSklHstf(
//	AtdDbGrdHstfLap& ghl,	//主桁水平補剛材ラップ範囲
//	std::ofstream& ofOb)
//{
//	StringHandler sh;
//	DiafSklHstf dsh;
//	for(int i=0;i<_ngMax;i++) {
//		int hstfPos = 0;
//		int hstfNum = 0;
//		// 2018/02/16 take Add Start
//		//水平補剛材の取付位置と段数をブロック毎に取得
//		std::map<int,int> hstfTargetList;
//		// 2018/02/16 take Add End
//		for(int j=0;j<ghl.size();j++) {
//			AtdDbGrdHstfLapItem ghlItem;
//			ghl.getAt(j, ghlItem);
//			int buzaiCode = ghlItem.getBuzaiCode();	//部材線コード
//			if(buzaiCode == i+1) {
//				hstfPos = ghlItem.getHstfPos();	//水平補剛材取付位置
//				hstfNum = ghlItem.getHstfNum();	//水平補剛材段数
//				// 2018/02/16 take Add Start
//				hstfTargetList.insert(std::make_pair(hstfPos, hstfNum)); //[取付位置,段数]
//				// 2018/02/16 take Add End
//				// 2018/02/16 take Delete Start
//				//break;
//				// 2018/02/16 take Delete End
//			}
//		}
//		if(hstfNum < 1) {
//			continue;
//		}
//		string ketaName = _ketaNameameList[i];		//桁名
//		double hstfHeightRatio[6];	//水平補剛材高さ比率1-6
//		for(int k=0;k<6;k++) {
//			hstfHeightRatio[k] = 0.0;
//		}
//		// 2018/02/16 take Edit Start
//		std::set<double> hstfHeightRatioList;
//		for(std::map<int,int>::iterator hstfItr = hstfTargetList.begin(); hstfItr != hstfTargetList.end(); ++hstfItr) {
//			if(hstfItr->first == -1) {			//上
//				if(hstfItr->second == 1) {
//					hstfHeightRatioList.insert(0.2);
//				} else {
//					hstfHeightRatioList.insert(0.14);
//					hstfHeightRatioList.insert(0.36);
//				}
//			} else if(hstfItr->first == 1) {	//下
//				if(hstfItr->second == 1) {
//					hstfHeightRatioList.insert(-0.2);
//				} else {
//					hstfHeightRatioList.insert(-0.36);
//					hstfHeightRatioList.insert(-0.14);
//				}
//			} else if(hstfItr->first == 2) {	//上下
//				if(hstfItr->second == 1) {
//					hstfHeightRatioList.insert(0.2);
//					hstfHeightRatioList.insert(-0.2);
//				} else {
//					hstfHeightRatioList.insert(0.14);
//					hstfHeightRatioList.insert(0.36);
//					hstfHeightRatioList.insert(-0.36);
//					hstfHeightRatioList.insert(-0.14);
//				}
//			} else {
//				continue;
//			}
//		}
//		
//		//if(hstfPos == -1) {			//上
//		//	if(hstfNum == 1) {
//		//		hstfHeightRatio[0] = 0.2;
//		//	} else {
//		//		hstfHeightRatio[0] = 0.14;
//		//		hstfHeightRatio[1] = 0.36;
//		//	}
//		//} else if(hstfPos == 1) {	//下
//		//	if(hstfNum == 1) {
//		//		hstfHeightRatio[0] = -0.2;
//		//	} else {
//		//		hstfHeightRatio[0] = -0.36;
//		//		hstfHeightRatio[1] = -0.14;
//		//	}
//		//} else if(hstfPos == 2) {	//上下
//		//	if(hstfNum == 1) {
//		//		hstfHeightRatio[0] = 0.2;
//		//		hstfHeightRatio[1] = -0.2;
//		//	} else {
//		//		hstfHeightRatio[0] = 0.14;
//		//		hstfHeightRatio[1] = 0.36;
//		//		hstfHeightRatio[2] = -0.36;
//		//		hstfHeightRatio[3] = -0.14;
//		//	}
//		//} else {
//		//	continue;
//		//}
//		// 2018/02/16 take Edit End
//		DiafSklHstfItem dshItem;
//		dshItem.setKetaName(ketaName);
//		dshItem.setHstfHeightRatio1(hstfHeightRatio[0]);
//		dshItem.setHstfHeightRatio2(hstfHeightRatio[1]);
//		dshItem.setHstfHeightRatio3(hstfHeightRatio[2]);
//		dshItem.setHstfHeightRatio4(hstfHeightRatio[3]);
//		dshItem.setHstfHeightRatio5(hstfHeightRatio[4]);
//		dshItem.setHstfHeightRatio6(hstfHeightRatio[5]);
//		dsh.append(dshItem);
//	}
//	if(dsh.size() > 0) {
//		dsh.sort();
//		dsh.toCsv(ofOb);
//	}
//
//	return JPT_OK;
//}
JptErrorStatus AtdDbToCsv::dbToCsvSklHstf(
	AtdDbStatusHstf& sh,	//断面･HSTFの位置関係(段数変化長･段数)
	AtdDbRangeHstf &rh,		//断面･水平補剛材の入る範囲(追加距離)
	std::ofstream& ofOb)
{
	//StringHandler sh;
	DiafSklHstf dsh;

	// 2018/03/01 take Add Start
	_nKetaHstfUpperPrg.resize( _ngMax );
	_nKetaHstfLowerPrg.resize( _ngMax );
	_hstfPanelUpperPrg.resize( _ngMax );
	_hstfPanelLowerPrg.resize( _ngMax );
	// 2018/03/01 take Add End

	for(int i=0;i<_ngMax;i++) {
		// 2018/02/28 take Add Start
		set<int> nHstfUpperPrg;		//水平補剛材上側段数
		set<int> nHstfLowerPrg;		//水平補剛材下側段数
		set<double> hstfHeightRatio;//水平補剛材高さ比率
		vector<int>	nHstfList;		//水平補剛材段数
		vector<int>	rlHstfList;		//段数変化長
		double currLocation = 0.0;	//段数が変化する位置
		for(int j=0;j<sh.size();j++){
			AtdDbStatusHstfItem shItem;
			sh.getAt(j, shItem);
			int noGrd = shItem.getNoGrd();
			int nStf= shItem.getNsfh();
			double rlsfh = shItem.getRlsfh();
			if(noGrd != i + 1){
				continue;
			}
			currLocation += rlsfh;
			nHstfList.push_back(nStf);
			rlHstfList.push_back(currLocation);
		}
		
		for(int j=0;j<_hstfLengAdd[i].size();j++){
			// 2018/03/01 take Add Start
			bool existUpperPrgFlg = false;	//上段HSTFが存在フラグ
			bool existLowerPrgFlg = false;	//下段HSTFが存在フラグ
			// 2018/03/01 take Add End
			for(int k=0;k<rh.size();k++){
				AtdDbRangeHstfItem rhItem;
				rh.getAt(k, rhItem);
				int noGrd =	rhItem.getNoGrd();
				int noHstf = rhItem.getNoHstf();
				double tLen1 = rhItem.getTLen1();
				double tLen2 = rhItem.getTLen2();
				if(noGrd != i + 1){	
					continue;
				}
				if(noHstf == -1){	
					continue;
				}
				if(tLen1 > _hstfLengAdd[i][j] || _hstfLengAdd[i][j] > tLen2){
					continue;
				}
				int uplo = rhItem.getUplo();
				if(uplo == 0){			//上段
					existUpperPrgFlg = true;
					for(int l=0;l<rlHstfList.size();l++){
						if(_hstfLengAdd[i][j] < rlHstfList[l]){	//水平補剛材がどの断面変化長に属しているか
							if(nHstfList[l] == 1){				//1段の場合
								hstfHeightRatio.insert(0.2);	//0.2H
								_hstfPanelUpperPrg[i].push_back(1);
								nHstfUpperPrg.insert(1);
							}else if(nHstfList[l] == 2){		//2段の場合
								hstfHeightRatio.insert(0.14);	//0.14H
								hstfHeightRatio.insert(0.36);	//0.36H
								_hstfPanelUpperPrg[i].push_back(2);
								nHstfUpperPrg.insert(2);
							}
							break;
						}
					}
				}else if(uplo == 1){	//下段
					existLowerPrgFlg = true;
					for(int l=0;l<rlHstfList.size();l++){
						if(_hstfLengAdd[i][j] < rlHstfList[l]){	//水平補剛材がどの断面変化長に属しているか
							if(nHstfList[l] == 1){				//1段の場合
								hstfHeightRatio.insert(-0.2);	//-0.2H
								_hstfPanelLowerPrg[i].push_back(1);
								nHstfLowerPrg.insert(1);
							}else if(nHstfList[l] == 2){		//2段の場合
								hstfHeightRatio.insert(-0.14);	//-0.14H
								hstfHeightRatio.insert(-0.36);	//-0.36H
								_hstfPanelLowerPrg[i].push_back(2);
								nHstfLowerPrg.insert(2);
							}
							break;
						}
					}
				}
			}
			if( existUpperPrgFlg == false ){	//上段が存在しなかった場合
				_hstfPanelUpperPrg[i].push_back( 0 );
			}
			if( existLowerPrgFlg == false ){	//下段が存在しなかった場合
				_hstfPanelLowerPrg[i].push_back( 0 );
			}
		}
		//上段の数を取得
		int hstfPrg = 0;
		set<int>::iterator itr;
		for(itr=nHstfUpperPrg.begin(); itr != nHstfUpperPrg.end(); ++itr) {
			hstfPrg += *itr;
		}
		_nKetaHstfUpperPrg[i].push_back(hstfPrg);

		//下段の数を取得
		hstfPrg = 0;
		for(itr=nHstfLowerPrg.begin(); itr != nHstfLowerPrg.end(); ++itr) {
			hstfPrg += *itr;
		}
		_nKetaHstfLowerPrg[i].push_back(hstfPrg);

		string ketaName = _ketaNameameList[i];		//桁名
		double tempHstfHeightRatio[6];	//水平補剛材高さ比率1-6
		for(int j=0;j<6;j++){			//水平補剛材高さの数
			if(j < hstfHeightRatio.size()){
				set<double>::iterator temp = hstfHeightRatio.begin();
				for(int k=0;k<hstfHeightRatio.size()-(1+j);++k){	//要素の移動
					++temp;
				}
				tempHstfHeightRatio[j] = *temp;
			}else{
				tempHstfHeightRatio[j] = 0.0;
			}
		}
		// 2018/02/28 take Add End
		DiafSklHstfItem dshItem;
		dshItem.setKetaName(ketaName);
		dshItem.setHstfHeightRatio1(tempHstfHeightRatio[0]);
		dshItem.setHstfHeightRatio2(tempHstfHeightRatio[1]);
		dshItem.setHstfHeightRatio3(tempHstfHeightRatio[2]);
		dshItem.setHstfHeightRatio4(tempHstfHeightRatio[3]);
		dshItem.setHstfHeightRatio5(tempHstfHeightRatio[4]);
		dshItem.setHstfHeightRatio6(tempHstfHeightRatio[5]);
		dsh.append(dshItem);
	}
	if(dsh.size() > 0) {
		dsh.sort();
		dsh.toCsv(ofOb);
	}

	return JPT_OK;
}
// 2018/02/28 take Edit End

//３．主桁
//(１)共通詳細データ
JptErrorStatus AtdDbToCsv::dbToCsvGirderCommon(
	AtdGirderCommon& agc,
	AtdDbSecScaleSpl& sss,	//断面･スケール及び文字高さと材質仕様･主桁添接関係
	AtdDbInputGrdSpl& igs,	//入力･主桁添接	
	std::ofstream& ofOb)
{
	DiafGirderCommon dgc;
	int ibuuti = 0;
	int ibluti = 0;
	for(int i=0;i<sss.size();i++) {
		AtdDbSecScaleSplItem sssItem;
		sss.getAt(i, sssItem);
		ibuuti = sssItem.getIbuuti();	//IBUUTI UFLG板厚逃げ方向
		ibluti = sssItem.getIbluti();	//IBLUTI LFLG板厚逃げ方向
	}
	int iclegu = 0;
	int iclegw = 0;
	int iclegl = 0;
	for(int i=0;i<igs.size();i++) {
		AtdDbInputGrdSplItem igsItem;
		igs.getAt(i, igsItem);
		iclegu = igsItem.getIclegu();	//ICLEGU UFLG_JC
		iclegw = igsItem.getIclegw();	//ICLEGW WEB_JC
		iclegl = igsItem.getIclegl();	//ICLEGL LFLG_JC
		break;
	}
	EnPlateThicknessEscWay uflgPtEscWay;	//上フランジ板厚逃げ方向
	EnPlateThicknessEscWay lflgPtEscWay;	//下フランジ板厚逃げ方向
	if(ibuuti == 0) {	//外逃げ
		uflgPtEscWay = SOTONIGE;
	} else {			//内逃げ
		uflgPtEscWay = UCHINIGE;
	}
	if(ibluti == 0) {	//外逃げ
		lflgPtEscWay = SOTONIGE;
	} else {			//内逃げ
		lflgPtEscWay = UCHINIGE;
	}
//	double uflgJc = (double)(iclegu % 1000) / 100;			//上フランジのジョイントクリアランス
//	double webJc = (double)(iclegw % 1000) / 100;			//ウェブのジョイントクリアランス
//	double lflgJc = (double)(iclegl % 1000) / 100;			//下フランジのジョイントクリアランス
	double uflgJc = agc.getUflgJc();						//上フランジのジョイントクリアランス
	double webJc = agc.getWebJc();							//ウェブのジョイントクリアランス
	double lflgJc = agc.getLflgJc();						//下フランジのジョイントクリアランス
	double itatsugiZureRyo = agc.getItatsugiZureRyo();		//板継ズレ量
	EnTypeNo uflgZaitanKeijo = agc.getUflgZaitanKeijo();								//上フランジ材端形状
	EnTypeNo lflgZaitanKeijo = agc.getLflgZaitanKeijo();								//下フランジ材端形状
	double updownFlgZaitanKeijoTachiageRyo = agc.getUpdownFlgZaitanKeijoTachiageRyo();	//上下フランジ材端形状立上げ量
	double lflgKakuhukubuTaper = agc.getLflgKakuhukubuTaper();							//下フランジ拡幅部のテーパー勾配
	double solePlateKyotyokuFreeSpace = agc.getSolePlateKyotyokuFreeSpace();	//ソールプレートの橋直方向空き量
	double solePlateKyojikuFreeSpace = agc.getSolePlateKyojikuFreeSpace();		//ソールプレートの橋軸方向空き量
	double hsFreeSpaceVs = agc.getHsFreeSpaceVs();					//水平補剛材の垂直補剛材部、横桁ウェブ部空き量
	double hsFreeSpaceSpl = agc.getHsFreeSpaceSpl();				//水平補剛材の添接板部空き量
	double hsFreeSpaceCbf = agc.getHsFreeSpaceCbf();				//水平補剛材の横桁フランジ部空き量
	double hsFreeSpaceCbfUlimit = agc.getHsFreeSpaceCbfUlimit();	//水平補剛材の横桁フランジからの高さ寸法上限
	string hsSnipSizeVs = agc.getHsSnipSizeVs();					//水平補剛材の垂直補剛材部のスニップサイズ
	string hsSnipSizeSpl = agc.getHsSnipSizeSpl();					//水平補剛材の添接部のスニップサイズ
	string hsSnipSizeCbf = agc.getHsSnipSizeCbf();					//水平補剛材の横桁フランジ部のスニップサイズ
	string shitenVsCutWu = agc.getShitenVsCutWu();					//支点上垂直補剛材の溶接辺側上側切欠
	string shitenVsCutWd = agc.getShitenVsCutWd();					//支点上垂直補剛材の溶接辺側下側切欠
	string shitenVsCutFu = agc.getShitenVsCutFu();					//支点上垂直補剛材の上側切欠
	string kakutenVsCutWu = agc.getKakutenVsCutWu();				//格点上垂直補剛材の溶接辺側上側切欠
	string kakutenVsCutWd = agc.getKakutenVsCutWd();				//格点上垂直補剛材の溶接辺側下側切欠
	string kakutenVsCutFu = agc.getKakutenVsCutFu();				//格点上垂直補剛材の上側切欠
	string middleVsCutWu = agc.getMiddleVsCutWu();					//中間垂直補剛材の溶接辺側上側切欠
	string middleVsCutWd = agc.getMiddleVsCutWd();					//中間垂直補剛材（圧縮タイプ）の溶接辺側下側切欠
	double middleVsFreeSpace = agc.getMiddleVsFreeSpace();			//中間垂直補剛材（引張タイプ）の下側空き量
	double uflgSplKyojikuZaitan = agc.getUflgSplKyojikuZaitan();					//上フランジ添接板の橋軸方向材端
	double uflgOutsideSplKyotyokuZaitan = agc.getUflgOutsideSplKyotyokuZaitan();	//上フランジ外側添接板の橋直方向材端
	double uflgInsideSplKyotyokuZaitan = agc.getUflgInsideSplKyotyokuZaitan();		//上フランジ内側添接板の橋直方向材端
	double webSplKyojikuZaitan = agc.getWebSplKyojikuZaitan();						//ウェブ添接板の高さ方向材端
	double webSplHeightZaitan = agc.getWebSplHeightZaitan();						//ウェブ添接板の橋軸方向材端
	double lflgSplKyojikuZaitan = agc.getLflgSplKyojikuZaitan();					//下フランジ添接板の橋軸方向材端
	double lflgSplKyotyokuZaitan = agc.getLflgSplKyotyokuZaitan();					//下フランジ添接板の橋直方向材端
	double webHoleSlopeLowerLimit = agc.getWebHoleSlopeLowerLimitGrd();				//ウェブ孔の孔勾配の下限値
	dgc.setUflgPtEscWay(uflgPtEscWay);	//上フランジ板厚逃げ方向
	dgc.setLflgPtEscWay(lflgPtEscWay);	//下フランジ板厚逃げ方向
	dgc.setUflgJc(uflgJc);				//上フランジのジョイントクリアランス
	dgc.setWebJc(webJc);				//ウェブのジョイントクリアランス
	dgc.setLflgJc(lflgJc);				//下フランジのジョイントクリアランス
	dgc.setItatsugiZureRyo(itatsugiZureRyo);
	dgc.setUflgZaitanKeijo(uflgZaitanKeijo);
	dgc.setLflgZaitanKeijo(lflgZaitanKeijo);
	dgc.setUpdownFlgZaitanKeijoTachiageRyo(updownFlgZaitanKeijoTachiageRyo);
	dgc.setLflgKakuhukubuTaper(lflgKakuhukubuTaper);
	dgc.setSolePlateKyotyokuFreeSpace(solePlateKyotyokuFreeSpace);
	dgc.setSolePlateKyojikuFreeSpace(solePlateKyojikuFreeSpace);
	dgc.setHsFreeSpaceVs(hsFreeSpaceVs);
	dgc.setHsFreeSpaceSpl(hsFreeSpaceSpl);
	dgc.setHsFreeSpaceCbf(hsFreeSpaceCbf);
	dgc.setHsFreeSpaceCbfUlimit(hsFreeSpaceCbfUlimit);
	dgc.setHsSnipSizeVs(hsSnipSizeVs);
	dgc.setHsSnipSizeSpl(hsSnipSizeSpl);
	dgc.setHsSnipSizeCbf(hsSnipSizeCbf);
	dgc.setShitenVsCutWu(shitenVsCutWu);
	dgc.setShitenVsCutWd(shitenVsCutWd);
	dgc.setShitenVsCutFu(shitenVsCutFu);
	dgc.setKakutenVsCutWu(kakutenVsCutWu);
	dgc.setKakutenVsCutWd(kakutenVsCutWd);
	dgc.setKakutenVsCutFu(kakutenVsCutFu);
	dgc.setMiddleVsCutWu(middleVsCutWu);
	dgc.setMiddleVsCutWd(middleVsCutWd);
	dgc.setMiddleVsFreeSpace(middleVsFreeSpace);
	dgc.setUflgSplKyojikuZaitan(uflgSplKyojikuZaitan);
	dgc.setUflgOutsideSplKyotyokuZaitan(uflgOutsideSplKyotyokuZaitan);
	dgc.setUflgInsideSplKyotyokuZaitan(uflgInsideSplKyotyokuZaitan);
	dgc.setWebSplKyojikuZaitan(webSplKyojikuZaitan);
	dgc.setWebSplHeightZaitan(webSplHeightZaitan);
	dgc.setLflgSplKyojikuZaitan(lflgSplKyojikuZaitan);
	dgc.setLflgSplKyotyokuZaitan(lflgSplKyotyokuZaitan);
	dgc.setWebHoleSlopeLowerLimit(webHoleSlopeLowerLimit);
	dgc.toCsv(ofOb);

	return JPT_OK;
}

//(２)主桁断面
JptErrorStatus AtdDbToCsv::dbToCsvGirderSection(
AtdDbInputUseMaterial& ium,	//入力・使用材料データ
	AtdDbSecGrd& sgd,		//断面･主桁断面
	AtdDbSecGrdLeng& sgl,	//断面･主桁の断面長	
	std::ofstream& ofOb)
{
	DiafGirderSection dgs;

	_danmenLengAdd.resize(_ngMax);
	_danmenAtsuUflg.resize(_ngMax);
	_danmenHabaUflg.resize(_ngMax);
	_danmenAtsuWeb.resize(_ngMax);
	_danmenAtsuLflg.resize(_ngMax);
	for( int i=0; i<_ngMax; i++ ){
		_danmenLengAdd[i].resize(_jointMax+1);
		_danmenAtsuUflg[i].resize(_jointMax+1);
		_danmenHabaUflg[i].resize(_jointMax+1);
		_danmenAtsuWeb[i].resize(_jointMax+1);
		_danmenAtsuLflg[i].resize(_jointMax+1);
	}
	for(int i=0;i<sgd.size();i++) {//AtdDbSecGrd& sgd,		//断面･主桁断面
		AtdDbSecGrdItem sgdItem;
		sgd.getAt(i, sgdItem);
		int nogrd = sgdItem.getNogrd();				//NOGRD 桁名
		int nosec = sgdItem.getNosec();				//NOSEC 断面番号
		string ketaName = _ketaNameameList[nogrd-1];		//桁名
		int sectionNo = nosec;						//断面番号
		double sectionLength = 0.0;					//断面長
		double uflgWidth = sgdItem.getBuf();								//BUF UFLG_幅 上フランジ幅
		double uflgPlateThickness = sgdItem.getTuf();						//TUF UFLG_板厚 上フランジ板厚
		int muf = sgdItem.getMuf();											//MUF UFLG_材質
		string uflgMaterial = ium.findMaterial(uflgPlateThickness, muf);	//上フランジ材質
		double webPlateThickness = sgdItem.getBlw();						//BLW WEB_板厚 ウェブ板厚
		int mlw = sgdItem.getMlw();											//MLW WEB_材質
		string webMaterial = ium.findMaterial(webPlateThickness, mlw);		//ウェブ材質
		double lflgWidth = sgdItem.getBlf();								//BLF LFLG_幅 下フランジ幅
		double lflgPlateThickness = sgdItem.getTlf();						//TLF LFLG_板厚 下フランジ板厚
		int mlf = sgdItem.getMlf();											//MLF LFLG_材質
		string lflgMaterial = ium.findMaterial(lflgPlateThickness, mlf);	//下フランジ材質
		for(int j=0;j<sgl.size();j++) {//AtdDbSecGrdLeng& sgl,	//断面･主桁の断面長
			AtdDbSecGrdLengItem sglItem;
			sgl.getAt(j, sglItem);
			int nogrd2 = sglItem.getNogrd();		//NOGRD 桁名
			int nosec2 = sglItem.getNosec();		//NOSEC ジョイント名
			if(nogrd == nogrd2 && nosec == nosec2) {
				sectionLength = sglItem.getRlsec();	//RLSEC ブロック長
				break;
			}
		}
		if(sectionLength < 1.0) {
//err
		}
		_danmenLengAdd[nogrd-1][nosec-1] = sectionLength;		//長さ
		_danmenAtsuUflg[nogrd-1][nosec-1] = uflgPlateThickness;	//TUF UFLG_板厚 上フランジ板厚
		_danmenHabaUflg[nogrd-1][nosec-1] = uflgWidth;			//BUF UFLG_幅 上フランジ幅
		_danmenAtsuWeb[nogrd-1][nosec-1] = webPlateThickness;	//BLW WEB_板厚 ウェブ板厚
		_danmenAtsuLflg[nogrd-1][nosec-1] = lflgPlateThickness;	//TLF LFLG_板厚 下フランジ板厚
		DiafGirderSectionItem dgsItem;
		dgsItem.setKetaName(ketaName);
		dgsItem.setSectionNo(sectionNo);
		dgsItem.setSectionLength(sectionLength);
		dgsItem.setUflgWidth(uflgWidth);
		dgsItem.setUflgPlateThickness(uflgPlateThickness);
		dgsItem.setUflgMaterial(uflgMaterial);
		dgsItem.setWebPlateThickness(webPlateThickness);
		dgsItem.setWebMaterial(webMaterial);
		dgsItem.setLflgWidth(lflgWidth);
		dgsItem.setLflgPlateThickness(lflgPlateThickness);
		dgsItem.setLflgMaterial(lflgMaterial);
		dgs.append(dgsItem);
	}
	for( int i=0; i<_ngMax; i++ ){
		for( int j=1; j<_jointMax+1; j++ ){
			_danmenLengAdd[i][j] += _danmenLengAdd[i][j-1];
		}
	}
	dgs.sort();
	dgs.toCsv(ofOb);

	return JPT_OK;
}

//(３)下フランジ拡幅部形状
JptErrorStatus AtdDbToCsv::dbToCsvGirderLflgWidening(
//AtdDbSecGrdKaku& sgk,		//断面･主桁格点名
	AtdDbSecSolePl& ssp,	//断面･ソールプレート
	std::ofstream& ofOb)
{
	DiafGirderLflgWidening dglw;

	for(int i=0;i<ssp.size();i++) {
		AtdDbSecSolePlItem sspItem;
		ssp.getAt(i, sspItem);
		int nogrd = sspItem.getNogrd();				//NOGRD 桁名
		string ketaName = _ketaNameameList[nogrd-1];		//桁名
		int nos = sspItem.getNos();					//NOS
		int kakutenNo = _shitenKakutenNo[nos-1];	//格点番号
		string shitenName = _shitenNameList[nos-1];	//支点名
		double solePlateWidth = sspItem.getBs1();	//BS1 SOLE-PL幅 ソールプレートの幅
		double solePlateLength = sspItem.getRls1();	//RLS1 SOLE-PL長さ ソールプレートの長さ
		DiafGirderLflgWideningItem dglwItem;
		dglwItem.setKetaName(ketaName);
		dglwItem.setShitenName(shitenName);
		dglwItem.setKakutenNo(kakutenNo);
		dglwItem.setSolePlateWidth(solePlateWidth);
		dglwItem.setSolePlateLength(solePlateLength);
		dglw.append(dglwItem);
	}
	dglw.sort();
	dglw.toCsv(ofOb);

	return JPT_OK;
}

//(４)桁端マンホール形状
JptErrorStatus AtdDbToCsv::dbToCsvGirderWebManhole(
	AtdDbSecManhole& smh,	//断面･桁端部のマンホールのカット
	AtdDbSecCutData& scd,	//断面･カットデータ
	std::ofstream& ofOb)
{
	DiafGirderWebManhole dgwm;

	for(int i=0;i<smh.size();i++) {
		AtdDbSecManholeItem smhItem;
		smh.getAt(i, smhItem);
		int nogrd = smhItem.getNogrd();	//NOGRD 桁名
		int sepos = smhItem.getSepos();	//SEPOS 配置側
		int iwcut = smhItem.getIwcut();	//IWCUT
		string ketaName = _ketaNameameList[nogrd-1];	//桁名
		EnSide setSide;		//桁端マンホールの配置側
		if(sepos == 0) {	//始側
			setSide = STARTSIDE;
		} else {			//終側
			setSide = ENDSIDE;
		}
		double mhPosition = 0.0;	//桁端マンホールのウェブ下端からの寸法
		double mhHeight = 0.0;		//桁端マンホールの高さ
		double mhWidth = 0.0;		//桁端マンホールの幅
		double mhRsize = 0.0;		//桁端マンホールのRサイズ
		bool flag = false;
		for(int j=0;j<scd.size();j++) {
			AtdDbSecCutDataItem scdItem;
			scd.getAt(j, scdItem);
			int dtcut1 = scdItem.getDtcut1();	//DTCUT1
			if(iwcut == dtcut1) {
				mhPosition = scdItem.getDtcut2();	//DTCUT2 MH_位置
				mhHeight = scdItem.getDtcut4();		//DTCUT4 MH_高さ
				mhWidth = scdItem.getDtcut3();		//DTCUT3 MH_幅
				mhRsize = scdItem.getDtcut5();		//DTCUT5 MH_Rサイズ
				flag = true;
				break;
			}
		}
		if(flag != true) {
			continue;
		}
		DiafGirderWebManholeItem dgwmItem;
		dgwmItem.setKetaName(ketaName);
		dgwmItem.setSetSide(setSide);
		dgwmItem.setMhPosition(mhPosition);
		dgwmItem.setMhHeight(mhHeight);
		dgwmItem.setMhWidth(mhWidth);
		dgwmItem.setMhRsize(mhRsize);
		dgwm.append(dgwmItem);
	}
	if(dgwm.size() > 0) {
		dgwm.sort();
		dgwm.toCsv(ofOb);
	}

	return JPT_OK;
}

//(５)垂直補剛材ID登録
// 2018/03/01 take Edit Start
//JptErrorStatus AtdDbToCsv::dbToCsvGirderVstfId(
//	AtdDbInputUseMaterial& ium,	//入力・使用材料データ
//	AtdDbSecVstf& svs,			//断面･VSTF断面
//	AtdDbSecGrdKakuVst& sgv,	//断面･主桁格点上VSTF
//	AtdDbSecVstfHaichi& svh,	//断面･VSTF配置
//	AtdDbInputStfAki& isa,		//入力･ＳＴＦのあき
//	AtdDbStructAll& sta,		//構成･全体
//	AtdDbGrdSecPower& gsp,		//主桁断面力データ
//	std::ofstream& ofOb)
JptErrorStatus AtdDbToCsv::dbToCsvGirderVstfId(
	AtdDbInputUseMaterial& ium,	//入力・使用材料データ
	AtdDbSecVstf& svs,			//断面･VSTF断面
	AtdDbSecGrdKakuVst& sgv,	//断面･主桁格点上VSTF
	AtdDbSecVstfHaichi& svh,	//断面･VSTF配置
	AtdDbInputStfAki& isa,		//入力･ＳＴＦのあき
	AtdDbStructAll& sta,		//構成･全体
	AtdDbGrdPower& gp,			//主桁応力度データ
	std::ofstream& ofOb)
// 2018/03/01 take Edit End
{
	StringHandler sh;
	DiafGirderVstfId dgvi;

	for(int i=0;i<svs.size();i++) {
		AtdDbSecVstfItem svsItem;
		svs.getAt(i, svsItem);
		int novssc = svsItem.getNovssc();	//NOVSSC 識別ID
		double vssc3 = svsItem.getVssc3();	//VSSC3 幅
		double vssc5 = svsItem.getVssc5();	//VSSC5 板厚
		// 2018/02/27 take Edit Start
		//int vssc8 = svsItem.getVssc8();		//VSSC8 材質
		int vssc8 = svsItem.getVssc8() % 1000;
		// 2018/02/27 take Edit End
		int idgfvi = -2;
		for(int j=0;j<isa.size();j++) {
			AtdDbInputStfAkiItem isaItem;
			isa.getAt(j, isaItem);
			idgfvi = isaItem.getIdgfvi();	//IDGFVI 入力・STFのあき タイプ
		}
		int ig = -1;
		int ip = -1;
		// 2018/03/01 take Delete Start
		//double grdUflgHaba = 0.0;
		//double grdWebHaba = 0.0;
		// 2018/03/01 take Delete End
		// 2018/03/01 take Add Start
		bool OnVstfKakuten = false;		//VSTFが格点上フラグ（true：格点上,false：格点以外）
		// 2018/03/01 take Add End
		//主桁上フランジ板幅取得
		for(int j=0;j<sgv.size();j++) {	//断面･主桁格点上VSTF
			AtdDbSecGrdKakuVstItem sgvItem;
			sgv.getAt(j, sgvItem);
			int novstp = sgvItem.getNovstp();	//NOVSTP 配置面 識別ID
			int novst = abs(novstp) % 100;
			if(novst == novssc) {
				ig = sgvItem.getNogrd();		//NOGRD 桁名
				ip = sgvItem.getNocrs();		//NOCRS
				// 2018/03/01 take Add Start
				OnVstfKakuten = true;
				// 2018/03/01 take Add End
				break;
			}
		}
		if(ig < 0) {
			for(int j=0;j<svh.size();j++) {	//断面･VSTF配置
				AtdDbSecVstfHaichiItem svhItem;
				svh.getAt(j, svhItem);
				int novst = svhItem.getNovst();	//NOVST 識別ID
				if(novst == novssc) {
					ig = svhItem.getNogrd();	//NOGRD 桁名
					ip = svhItem.getNopnl();	//NOPNL 始側格点名 終側格点名
					break;
				}
			}
		}
		// 2018/03/01 take Delete Start
		/*if(ig > 0) {
			grdUflgHaba = _danmenHabaUflg[ig-1][0];
			grdWebHaba = _danmenAtsuWeb[ig-1][0];
		}*/
		// 2018/03/01 take Delete End

		EnTypeNo girderVstfType = TYPE1;	//垂直補剛材のタイプ
		// 2018/03/01 take Edit Start
		//if(idgfvi == -2) {	//『-2＝離さない』
		//	if((grdUflgHaba/2 - grdWebHaba/2) < vssc3) {
		//		girderVstfType = TYPE1;
		//	} else {
		//		girderVstfType = TYPE2;
		//	}
		//} else {	//『0＝引張部と相反部』、『1＝引張部のみ』
		//	int nspan = 1;
		//	if(sta.size() > 0) {
		//		AtdDbStructAllItem staItem;
		//		sta.getAt(0, staItem);
		//		nspan = staItem.getNspan();	//NSPAN
		//	}
		//	if(nspan == 1) {	//『構成・全体』テーブルの『NSPAN』の値が1のとき、すべての垂直補剛材がタイプ2
		//		girderVstfType = TYPE2;
		//	} else {
		//		double mageMx = gsp.findMageMx(1, 1);	//主桁断面力データ 曲げモーメント（面内）Ｍｘ
		//		if(mageMx > 0.0) {
		//			girderVstfType = TYPE2;
		//		} else {
		//			if((grdUflgHaba/2 - grdWebHaba/2) < vssc3) {
		//				girderVstfType = TYPE1;
		//			} else {
		//				girderVstfType = TYPE2;
		//			}
		//		}
		//	}
		//}
		if( OnVstfKakuten == true && _oudanType[ip-1] == 0 || OnVstfKakuten == true && _oudanType[ip-1] == 1 ){	//支点部の場合
			girderVstfType = TYPE1;
		}else if( OnVstfKakuten == true ){	//格点部の場合
			girderVstfType = TYPE2;
		}else if( idgfvi == 0 ){	//中間部の場合（0＝離さない）
			girderVstfType = TYPE2;
		}else{						//中間部の場合（-1＝引張部のみ,-2＝引張部と相反部）
			if(_nSpan == 1) {		//単純桁のとき、すべての垂直補剛材がタイプ3
				girderVstfType = TYPE3;
			} else {
				// 2018/03/01 take Edit Start
				//double mageMx = gsp.findMageMx(1, 1);	//主桁断面力データ 曲げモーメント（面内）Ｍｘ
				//if(mageMx > 0.0) {
				double forceMx = gp.findForceMx(ig, ip);//主桁応力度データ Ｍｘに対する応力度
				if(forceMx > 0.0) {
				// 2018/03/01 take Edit End
				
					girderVstfType = TYPE3;
				} else {
					girderVstfType = TYPE2;
				}
			}
		}
		// 2018/03/01 take Edit End
		string girderVstfId = "VS" + sh.toString(novssc);	//識別ID
		double width = vssc3;				//幅
		double plateThickness = vssc5;		//板厚
		string material = ium.findMaterial(plateThickness, vssc8);	//材質
		DiafGirderVstfIdItem dgviItem;
		dgviItem.setGirderVstfId(girderVstfId);
		dgviItem.setGirderVstfType(girderVstfType);
		dgviItem.setWidth(width);
		dgviItem.setPlateThickness(plateThickness);
		dgviItem.setMaterial(material);
		dgvi.append(dgviItem);
		_grdVstfNovssc.push_back(novssc);		//主桁垂直補剛材識別ID
		_grdVstfItaatsu.push_back(plateThickness);	//主桁垂直補剛材板厚
	}
	dgvi.sort();
	dgvi.toCsv(ofOb);

	return JPT_OK;
}

//(６)支点・格点垂直補剛材配置
JptErrorStatus AtdDbToCsv::dbToCsvGirderVstfSetSc(
	AtdDbSecGrdKakuVst& sgv,	//断面･主桁格点上VSTF
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafGirderVstfSetSc dgvss;

	for(int i=0;i<_ngMax;i++) {
		for(int j=0;j<(int)_oudanNameList.size();j++) {
			int novstp = sgv.findId(i+1, j+1);
			if(novstp == 0) {
				continue;
			}
			string ketaName = _ketaNameameList[i];			//桁名
			string kakutenName = _oudanNameList[j];		//格点名
			int kakutenNo = j+1;						//格点番号
			DiafGirderVstfSetScItem dgvssItem;
			dgvssItem.setKetaName(ketaName);
			dgvssItem.setKakutenName(kakutenName);
			dgvssItem.setKakutenNo(kakutenNo);
			string girderVstfId;
			if(novstp > 0) {		//両側
				double vstfAtsu =findDouble(novstp, _grdVstfNovssc, _grdVstfItaatsu);
				_grdKakuVstfKetaNo.push_back(i+1);	//支点・格点主桁垂直補剛材桁番号
				_grdKakuVstfKakutenNo.push_back(j+1);	//支点・格点主桁垂直補剛材格点番号
				_grdKakuVstfKetaName.push_back(ketaName);	//支点・格点主桁垂直補剛材桁名
				_grdKakuVstfKakutenName.push_back(kakutenName);	//支点・格点主桁垂直補剛材格点名
				_grdKakuVstfFace.push_back(NS);	//支点・格点主桁垂直補剛材配置面
				_grdKakuVstfitaastu.push_back(vstfAtsu);	//支点・格点主桁垂直補剛材板厚
				_grdKakuVstfKetaNo.push_back(i+1);	//支点・格点主桁垂直補剛材桁番号
				_grdKakuVstfKakutenNo.push_back(j+1);	//支点・格点主桁垂直補剛材格点番号
				_grdKakuVstfKetaName.push_back(ketaName);	//支点・格点主桁垂直補剛材桁名
				_grdKakuVstfKakutenName.push_back(kakutenName);	//支点・格点主桁垂直補剛材格点名
				_grdKakuVstfFace.push_back(FS);	//支点・格点主桁垂直補剛材配置面
				_grdKakuVstfitaastu.push_back(vstfAtsu);	//支点・格点主桁垂直補剛材板厚
				girderVstfId = "VS" + sh.toString(novstp);
				dgvssItem.setGirderVstfId(girderVstfId);
				dgvssItem.setSetFace(NS);
				dgvss.append(dgvssItem);
				dgvssItem.setSetFace(FS);
				dgvss.append(dgvssItem);
			} else {
				EnFace setFace;					//垂直補剛材の配置面
				novstp = abs(novstp);
				if(novstp > 100) {	//右側
					novstp = novstp % 100;
					setFace = NS;		//	表面
				} else {			//左側
					setFace = FS;		//	裏面
				}
				double vstfAtsu =findDouble(novstp, _grdVstfNovssc, _grdVstfItaatsu);
				_grdKakuVstfKetaNo.push_back(i+1);	//支点・格点主桁垂直補剛材桁番号
				_grdKakuVstfKakutenNo.push_back(j+1);	//支点・格点主桁垂直補剛材格点番号
				_grdKakuVstfKetaName.push_back(ketaName);	//支点・格点主桁垂直補剛材桁名
				_grdKakuVstfKakutenName.push_back(kakutenName);	//支点・格点主桁垂直補剛材格点名
				_grdKakuVstfFace.push_back(setFace);	//支点・格点主桁垂直補剛材配置面
				_grdKakuVstfitaastu.push_back(vstfAtsu);	//支点・格点主桁垂直補剛材板厚
				girderVstfId = "VS" + sh.toString(novstp);
				dgvssItem.setGirderVstfId(girderVstfId);
				dgvssItem.setSetFace(setFace);
				dgvss.append(dgvssItem);
			}
		}
	}
	dgvss.sort();
	dgvss.toCsv(ofOb);

	return JPT_OK;
}

//(７)中間垂直補剛材配置
JptErrorStatus AtdDbToCsv::dbToCsvGirderVstfSetV(
	AtdDbSecVstfHaichi& svh,	//断面･VSTF配置
	AtdDbSecStfDir& ssd,		//断面･詳細･補剛材の向き
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafGirderVstfSetV dgvsv;

	for(int i=0;i<_ngMax;i++) {
		int idrv = ssd.findIdrv(i+1);
		if(idrv < 0) {
//err
		}
		for(int j=0;j<(int)_oudanNameList.size()-1;j++) {
			int novst = svh.findId(i+1, j+1);
			if(novst < 1) {
				continue;
			}
			string ketaName = _ketaNameameList[i];					//桁名
			string startSideKakutenName = _oudanNameList[j];	//始側格点名
			string endSideKakutenName = _oudanNameList[j+1];	//終側格点名
			int startSideKakutenNo = j+1;						//始側格点番号
			int endSideKakutenNo = j+2;							//終側格点番号
			int middlePointNo = 0;								//格点間の中間点番号
			EnFace setFace;			//垂直補剛材の配置面
			if(idrv == 0) {
				setFace = FS;	//	裏面
			} else {			//左側
				setFace = NS;	//	表面
			}
			string girderVstfId = "VS" + sh.toString(novst);				//配置する垂直補剛材の識別ID
			DiafGirderVstfSetVItem dgvsvItem;
			dgvsvItem.setKetaName(ketaName);
			dgvsvItem.setStartSideKakutenName(startSideKakutenName);
			dgvsvItem.setEndSideKakutenName(endSideKakutenName);
			dgvsvItem.setStartSideKakutenNo(startSideKakutenNo);
			dgvsvItem.setEndSideKakutenNo(endSideKakutenNo);
			dgvsvItem.setMiddlePointNo(middlePointNo);
			dgvsvItem.setSetFace(setFace);
			dgvsvItem.setGirderVstfId(girderVstfId);
			dgvsv.append(dgvsvItem);
		}
	}
	dgvsv.sort();
	dgvsv.toCsv(ofOb);

	return JPT_OK;
}

//(８)水平補剛材ID登録
JptErrorStatus AtdDbToCsv::dbToCsvGirderHstfId(
	AtdDbInputUseMaterial& ium,	//入力・使用材料データ
	AtdDbSecHstf& shf,			//断面･HSTF断面	
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafGirderHstfId dghi;

	for(int i=0;i<shf.size();i++) {
		AtdDbSecHstfItem shfItem ;
		shf.getAt(i, shfItem);
		int nohssc = shfItem.getNohssc();	//NOHSSC 識別ID
		string girderHstfId = "HS" + sh.toString(nohssc);	//識別ID
		double width = shfItem.getHssc3();	//HSSC3 幅
		double plateThickness = shfItem.getHssc5();	//HSSC5 板厚
		int hssc8 = shfItem.getHssc8();		//HSSC8 材質
		string material = ium.findMaterial(plateThickness, hssc8);		//材質
		DiafGirderHstfIdItem dghiItem;
		dghiItem.setGirderHstfId(girderHstfId);
		dghiItem.setWidth(width);
		dghiItem.setPlateThickness(plateThickness);
		dghiItem.setMaterial(material);
		dghi.append(dghiItem);
	}
	dghi.sort();
	dghi.toCsv(ofOb);

	return JPT_OK;
}

//(９)水平補剛材配置
JptErrorStatus AtdDbToCsv::dbToCsvGirderHstfSet(
	AtdDbSecHstfHaichi& shh,	//断面･HSTF配置	
	AtdDbSecStfDir& ssd,		//断面･詳細･補剛材の向き
	AtdDbSecVstfHaichi& svh,	//断面･VSTF配置
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafGirderHstfSet dghs;

	for(int i=0;i<_ngMax;i++) {
		int idx = ssd.find(i+1);
		if(idx < 0) {
			continue;
		}
		AtdDbSecStfDirItem ssdItem;
		ssd.getAt(idx, ssdItem);
		int idrv = ssdItem.getIdrv();		//IDRV 配置面
		int idrh = ssdItem.getIdrh();		//IDRH 配置面
		if(idrh == 1) {	//1＝垂直補剛材と逆
			if(idrv == 0) {
				idrv = 1;
			} else {
				idrv = 0;
			}
		}
		//for(int j=0;j<(int)_oudanNameList.size()-1;j++) {
		//	int idx = shh.find(i+1, j+1);
		//	if(idx < 0) {
		//		continue;
		//	}
		//	AtdDbSecHstfHaichiItem shhItem;
		//	shh.getAt(idx, shhItem);
		//	int nohsl1 = shhItem.getNohsl1();	//NOHSL1 配置段番号
		//	int nohsl2 = shhItem.getNohsl2();	//NOHSL2 配置段番号
		//	string ketaName = _ketaNameameList[i];					//桁名
		//	string startSideKakutenName = _oudanNameList[j];	//始側格点名
		//	string endSideKakutenName = _oudanNameList[j+1];	//終側格点名
		//	int startSideKakutenNo = j+1;		//始側格点番号
		//	int endSideKakutenNo = j+2;			//終側格点番号
		//	int kakutenIntervalNo = 0;			//水平補剛材の格点間での配置位置
		//	EnFace setFace;						//水平補剛材の配置面
		//	if(idrv == 0) {
		//		setFace = FS;		//	裏面
		//	} else {			//左側
		//		setFace = NS;		//	表面
		//	}
		//	DiafGirderHstfSetItem dghsItem;
		//	dghsItem.setKetaName(ketaName);
		//	dghsItem.setStartSideKakutenName(startSideKakutenName);
		//	dghsItem.setEndSideKakutenName(endSideKakutenName);
		//	dghsItem.setStartSideKakutenNo(startSideKakutenNo);
		//	dghsItem.setEndSideKakutenNo(endSideKakutenNo);
		//	dghsItem.setKakutenIntervalNo(kakutenIntervalNo);
		//	dghsItem.setSetFace(setFace);
		//	int setStepNo = 1;							//水平補剛材を配置する段の番号
		//	string girderHstfId = "HS" + sh.toString(nohsl1);		//配置する水平補剛材の識別ID
		//	dghsItem.setSetStepNo(setStepNo);
		//	dghsItem.setGirderHstfId(girderHstfId);
		//	dghs.append(dghsItem);
		//	if(nohsl2 > 0) {
		//		int setStepNo = 2;							//水平補剛材を配置する段の番号
		//		string girderHstfId = "HS" + sh.toString(nohsl2);		//配置する水平補剛材の識別ID
		//		dghsItem.setSetStepNo(setStepNo);
		//		dghsItem.setGirderHstfId(girderHstfId);
		//		dghs.append(dghsItem);
		//	}
		//}

		for(int j=0;j<(int)_oudanNameList.size()-1;j++) {
			int idx = shh.find(i+1, j+1);
			if(idx < 0) {
				continue;
			}
			// 2018/03/01 take Add Start
			int kakutenIntervalNo = 0;		//水平補剛材の格点間での配置位置
			// 2018/03/01 take Add End
			AtdDbSecHstfHaichiItem shhItem;
			shh.getAt(idx, shhItem);
			for(int k = 0;k < _hstfPanelNumber.size();k++ ){
				int panel = shhItem.getNopnl();
				if( panel == _hstfPanelNumber[i][k] ){	//対象のHSTF配置パネルか
					kakutenIntervalNo += 1;
					int nohsl1 = shhItem.getNohsl1();	//NOHSL1 配置段番号
					int nohsl2 = shhItem.getNohsl2();	//NOHSL2 配置段番号
					string ketaName = _ketaNameameList[i];					//桁名
					string startSideKakutenName = _oudanNameList[j];	//始側格点名
					string endSideKakutenName = _oudanNameList[j+1];	//終側格点名
					int startSideKakutenNo = j+1;		//始側格点番号
					int endSideKakutenNo = j+2;			//終側格点番号
					EnFace setFace;						//水平補剛材の配置面
					if(idrv == 0) {
						setFace = FS;		//	裏面
					} else {			//左側
						setFace = NS;		//	表面
					}
					//int prgs = //現在の格点間番号に取付く水平補剛材の段数の合計
					for(int l=0;l<_hstfPanelUpperPrg[i][k];l++){ //上段
						
					}
					for(int l=0;l<_hstfPanelLowerPrg[i][k];l++){ //下段
						
					}
				}
				if( panel < _hstfPanelNumber[i][k] ){
					break;
				}
			}
			DiafGirderHstfSetItem dghsItem;
			//dghsItem.setKetaName(ketaName);
			//dghsItem.setStartSideKakutenName(startSideKakutenName);
			//dghsItem.setEndSideKakutenName(endSideKakutenName);
			//dghsItem.setStartSideKakutenNo(startSideKakutenNo);
			//dghsItem.setEndSideKakutenNo(endSideKakutenNo);
			//dghsItem.setKakutenIntervalNo(kakutenIntervalNo);
			//dghsItem.setSetFace(setFace);
			//int setStepNo = 1;							//水平補剛材を配置する段の番号
			//string girderHstfId = "HS" + sh.toString(nohsl1);		//配置する水平補剛材の識別ID
			//dghsItem.setSetStepNo(setStepNo);
			//dghsItem.setGirderHstfId(girderHstfId);
			//dghs.append(dghsItem);
			//if(nohsl2 > 0) {
			//	int setStepNo = 2;							//水平補剛材を配置する段の番号
			//	string girderHstfId = "HS" + sh.toString(nohsl2);		//配置する水平補剛材の識別ID
			//	dghsItem.setSetStepNo(setStepNo);
			//	dghsItem.setGirderHstfId(girderHstfId);
			//	dghs.append(dghsItem);
			//}
		}
	}
	dghs.sort();
	dghs.toCsv(ofOb);

	return JPT_OK;
}

//(１０)フランジ添接形状定義
JptErrorStatus AtdDbToCsv::dbToCsvGirderFlgSplId(
	AtdDbInputUseMaterial& ium,	//入力・使用材料データ
	AtdDbSplCommon& scm,		//添接･線形添接共通データ
	AtdDbSplFlg& sfg,			//添接･各添接･フランジ
	AtdDbSecScaleFill& ssf,		//断面･スケール及び文字高さと材質仕様･フィラープレート
	AtdDbInputFillOption& ifo,	//入力･フィラープレートオプション
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafGirderFlgSplId dgfsi;

	//孔径
	double holeSize = scm.findHoleSize(3);

	//断面･スケール及び文字高さと材質仕様･フィラープレート
	int mtfill = 0;
	double rfilfu = 0.0;	//RFILFU
	double rfilfl = 0.0;	//RFILFL
	if(ssf.size() > 0) {
		AtdDbSecScaleFillItem ssfItem;
		ssf.getAt(0, ssfItem);
		mtfill = ssfItem.getMtfill();	//MTFILL Fill材質
		rfilfu = ssfItem.getRfilfu();
		rfilfl = ssfItem.getRfilfl();
	}

	//入力･フィラープレートオプション
	int ifilfu = 0;	//IFILFU
	int ifilfl = 0;	//IFILFL
	if(ifo.size() > 0) {
		AtdDbInputFillOptionItem ifoItem;
		ifo.getAt(0, ifoItem);
		ifilfu = ifoItem.getIfilfu();
		ifilfl = ifoItem.getIfilfl();
	}

	//添接･各添接･フランジ
	int cnt = 0;
	for(int i=0;i<sfg.size();i++) {
		AtdDbSplFlgItem sfgItem;
		sfg.getAt(i, sfgItem);
		int strcode = sfgItem.getStrcode();	//STRCODE
		if(strcode != 3) {
			continue;
		}
		int iupdw = sfgItem.getIupdw();		//IUPDW 上下
		int noj = sfgItem.getNoj();			//NOJ ジョイント番号
		int ifmj = sfgItem.getIfmj();		//IFMJ 孔タイプ FLG孔_タイプ
		double poj = sfgItem.getPoj();		//POJ CP
		double pj = sfgItem.getPj();		//PJ P1 P FLG孔_P
		int npj = sfgItem.getNpj();			//NPJ P1の数
		double p00j = sfgItem.getP00j();	//P00J P2
		int npoj = sfgItem.getNpoj();		//NPOJ P2の数
		double goj = sfgItem.getGoj();		//GOJ CG FLG孔_CG
		double gj = sfgItem.getGj();		//GJ G FLG孔_G
		int ngj = sfgItem.getNgj();			//NGJ Gの数
		int b1j = sfgItem.getB1j();			//B1J SPLタイプ SPL_タイプ
		double t1j = sfgItem.getT1j();		//T1J NS板厚 SPL_板厚（外側） SPL_板厚（内側）
		double t2j = sfgItem.getT2j();		//T2J FS板厚 CONN_板厚
		int mtj = sfgItem.getMtj();			//MTJ NS材質 FS材質 SPL_材質（外側） SPL_材質（内側）
		cnt++;
		string girderFlgSplId = "GFS" + sh.toString(cnt);	//識別ID
		EnHoleType holeType;			//孔タイプ
		if(ifmj == 1) {			//格子
			holeType = KOUSHI;			//	格子
		} else if(ifmj == 2) {	//千鳥1
			holeType = CHIDORI1;		//	千鳥1
		} else {				//千鳥3
			holeType = CHIDORI3;		//	千鳥3
		}
		double centerPitch = poj;					//センターピッチ
		string pitch1 = makePitchGage(npj, pj);		//ピッチ1
		string pitch2 = makePitchGage(npoj, p00j);	//ピッチ2
		double centerGauge = goj;					//センターゲージ
		string gauge = makePitchGage(ngj, gj);		//ゲージ
		EnTypeNo splType;							//添接板タイプ
		if(b1j > 0) {	//タイプ1
			splType = TYPE1;
		} else {		//タイプ2
			splType = TYPE2;
		}
		double nsPlateThickness = t1j;		//オモテ側添接板の板厚
		double fsPlateThickness = t2j;		//ウラ側添接板の板厚
		string nsMaterial = ium.findMaterial(nsPlateThickness, mtj);		//オモテ側添接板の材質
		string fsMaterial = ium.findMaterial(fsPlateThickness, mtj);		//ウラ側添接板の材質
		// 2018/02/15 take Edit Start
		// フィラーを作成しない場合は、『-1.0』とする
		double fillerPlateThickness = -1.0;
		//double fillerPlateThickness = 0.0;	//フィラーの板厚
		// 2018/02/15 take Edit End
		string fillerMaterial = "";			//フィラーの材質
		if((iupdw == 0 && ifilfu == 1) ||(iupdw == 1 && ifilfl == 1)) {	//フィラー配置
			int ig = (noj-1) / _jointMax;
			int ij = (noj-1) % _jointMax;
			double atsu1, atsu2;
			if(iupdw == 0) {	//上フランジ
				atsu1 = fabs(_danmenAtsuUflg[ig][ij]);
				atsu2 = fabs(_danmenAtsuUflg[ig][ij+1]);
			} else {			//下フランジ
				atsu1 = fabs(_danmenAtsuLflg[ig][ij]);
				atsu2 = fabs(_danmenAtsuLflg[ig][ij+1]);
			}
			double atsuSa = fabs(atsu1-atsu2);
			if((iupdw == 0 && rfilfu <= atsuSa) ||(iupdw == 1 && rfilfl <= atsuSa)) {	//板厚差あり
				fillerPlateThickness = getFillItaatsu(atsuSa);	//フィラーの板厚
				fillerMaterial = ium.findMaterial(fillerPlateThickness, mtfill);	//フィラーの材質
			}
		}
		DiafGirderFlgSplIdItem dgfsiItem;
		dgfsiItem.setGirderFlgSplId(girderFlgSplId);
		dgfsiItem.setHoleType(holeType);
		dgfsiItem.setHoleSize(holeSize);
		dgfsiItem.setCenterPitch(centerPitch);
		dgfsiItem.setPitch1(pitch1);
		dgfsiItem.setPitch2(pitch2);
		dgfsiItem.setCenterGauge(centerGauge);
		dgfsiItem.setGauge(gauge);
		dgfsiItem.setSplType(splType);
		dgfsiItem.setNsPlateThickness(nsPlateThickness);
		dgfsiItem.setNsMaterial(nsMaterial);
		dgfsiItem.setFsPlateThickness(fsPlateThickness);
		dgfsiItem.setFsMaterial(fsMaterial);
		dgfsiItem.setFillerPlateThickness(fillerPlateThickness);
		dgfsiItem.setFillerMaterial(fillerMaterial);
		dgfsi.append(dgfsiItem);
		if(iupdw == 0) {	//上フランジ
			_uflgIdName.push_back(girderFlgSplId);	//上フランジ識別名
			_uflgJointNo.push_back(noj);	//上フランジジョイント番号
		} else {			//下フランジ
			_lflgIdName.push_back(girderFlgSplId);	//下フランジ識別名
			_lflgJointNo.push_back(noj);	//下フランジジョイント番号
		}
	}
	dgfsi.sort();
	dgfsi.toCsv(ofOb);

	return JPT_OK;
}

//(１１)ウェブ添接形状
JptErrorStatus AtdDbToCsv::dbToCsvGirderWebSplId(
AtdDbInputUseMaterial& ium,		//入力・使用材料データ
	AtdDbSplCommon& scm,		//添接･線形添接共通データ
	AtdDbSplWeb& swb,			//添接･各添接･ウェブ
	AtdDbSecScaleFill& ssf,		//断面･スケール及び文字高さと材質仕様･フィラープレート
	AtdDbInputFillOption& ifo,	//入力･フィラープレートオプション
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafGirderWebSplId dgwsi;

	//孔径
	double holeSize = scm.findHoleSize(3);

	//断面･スケール及び文字高さと材質仕様･フィラープレート
	int mtfill = 0;
	double rfilwb = 0.0;	//RFILWB
	if(ssf.size() > 0) {
		AtdDbSecScaleFillItem ssfItem;
		ssf.getAt(0, ssfItem);
		mtfill = ssfItem.getMtfill();	//MTFILL Fill材質
		rfilwb = ssfItem.getRfilwb();
	}

	//入力･フィラープレートオプション
	int ifilwb = 0;	//IFILWB
	if(ifo.size() > 0) {
		AtdDbInputFillOptionItem ifoItem;
		ifo.getAt(0, ifoItem);
		ifilwb = ifoItem.getIfilwb();
	}

	//添接･各添接･ウェブ
	int cnt = 0;
	for(int i=0;i<swb.size();i++) {
		AtdDbSplWebItem swbItem;
		swb.getAt(i, swbItem);
		int strcode = swbItem.getStrcode();	//STRCODE
		// 2018/02/15 take Add Start
		int itpwj = swbItem.getItpwj();		//ITPWJ
		// 2018/02/15 take Add End
		if(strcode != 3) {
			continue;
		}
		// 2018/02/15 take Add Start
		if(itpwj != 1) {
			continue;
		}
		// 2018/02/15 take Add End
		int nowj = swbItem.getNowj();		//NOWJ
		double gwj = swbItem.getGwj();		//GWJ P
		int ng2wj = swbItem.getNg2wj();		//NG2WJ Pの数
		double gowj = swbItem.getGowj();	//GOWJ CP
		double p2wj = swbItem.getP2wj();	//P2WJ 中央G G
		int nd2wj = swbItem.getNd2wj();		//ND2WJ 中央Gの数
		double p3wj = swbItem.getP3wj();	//P3WJ 上側G
		int nd3wj = swbItem.getNd3wj();		//ND3WJ 上側Gの数
		double p0wj = swbItem.getP0wj();	//P0WJ 上側空き量
		double p00j = swbItem.getP00j();	//P00J 下側空き量
		double t2wj = swbItem.getT2wj();	//T2WJ SPL板厚 SPL_板厚
		int mtwj = swbItem.getMtwj();		//MTWJ SPL材質 SPL_材質
		cnt++;
		string girderWebSplId = "GWS" + sh.toString(cnt);	//識別ID
		double centerPitch = gowj;			//センターピッチ
		string pitch = makePitchGage(ng2wj, gwj);				//ピッチ
		string upperGauge = makePitchGage(nd2wj, p2wj);		//上側ゲージ
		string centerGauge = makePitchGage(nd3wj, p3wj);		//中央ゲージ
		if(centerGauge == "") {
			centerGauge = upperGauge;
			upperGauge = "";
		}
		string lowerGauge = upperGauge;		//下側ゲージ
		double upperFreeSpace = p0wj;			//ウェブ上端から最上段孔までの空き量
		double lowerFreeSpace = p00j;			//ウェブ下端から最下段孔までの空き量
		double splPlateThickness = t2wj;		//添接板の板厚
		string splMaterial = ium.findMaterial(splPlateThickness, mtwj);		//添接板の材質
		// 2018/02/15 take Edit Start
		// フィラーを作成しない場合は、『-1.0』とする
		double fillerPlateThickness = -1.0;
		//double fillerPlateThickness = 0.0;	//フィラーの板厚
		// 2018/02/15 take Edit End
		string fillerMaterial = "";			//フィラーの材質
		if(ifilwb == 1) {	//フィラー配置
			int ig = (nowj-1) / _jointMax;
			int ij = (nowj-1) % _jointMax;
			double atsu1 = fabs(_danmenAtsuUflg[ig][ij]);
			double atsu2 = fabs(_danmenAtsuUflg[ig][ij+1]);
			double atsuSa = fabs(atsu1-atsu2);
			if(rfilwb <= atsuSa) {	//板厚差あり
				fillerPlateThickness = getFillItaatsu(atsuSa);	//フィラーの板厚
				fillerMaterial = ium.findMaterial(fillerPlateThickness, mtfill);	//フィラーの材質
			}
		}
		DiafGirderWebSplIdItem dgwsiItem;
		dgwsiItem.setGirderWebSplId(girderWebSplId);
		dgwsiItem.setHoleSize(holeSize);
		dgwsiItem.setCenterPitch(centerPitch);
		dgwsiItem.setPitch(pitch);
		dgwsiItem.setUpperGauge(upperGauge);
		dgwsiItem.setCenterGauge(centerGauge);
		dgwsiItem.setLowerGauge(lowerGauge);
		dgwsiItem.setUpperFreeSpace(upperFreeSpace);
		dgwsiItem.setLowerFreeSpace(lowerFreeSpace);
		dgwsiItem.setSplPlateThickness(splPlateThickness);
		dgwsiItem.setSplMaterial(splMaterial);
		dgwsiItem.setFillerPlateThickness(fillerPlateThickness);
		dgwsiItem.setFillerMaterial(fillerMaterial);
		dgwsi.append(dgwsiItem);
		_webIdName.push_back(girderWebSplId);	//ウエブ識別名
		_webJointNo.push_back(nowj);			//ウエブジョイント番号
	}
	dgwsi.sort();
	dgwsi.toCsv(ofOb);

	return JPT_OK;
}

//(１２)添接配置
JptErrorStatus AtdDbToCsv::dbToCsvGirderSplSet(
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafGirderSplSet dgss;

	for(int i=0;i<_ngMax;i++) {
		string ketaName = _ketaNameameList[i];		//桁名
		for(int j=0;j<_jointMax;j++) {
			string jointName = "J" + sh.toString(j+1);		//ジョイント名
			int jointNo = j+1;				//ジョイント番号
			int nogj = i*_jointMax + jointNo;
			string uflgSplId = findIdName(nogj, _uflgJointNo, _uflgIdName);;	//上フランジに配置する添接形状
			string webSplId = findIdName(nogj, _webJointNo, _webIdName);;		//ウェブに配置する添接形状
			string lflgSplId = findIdName(nogj, _lflgJointNo, _lflgIdName);;	//下フランジに配置する添接形状
			DiafGirderSplSetItem dgssItem;
			dgssItem.setKetaName(ketaName);
			dgssItem.setJointName(jointName);
			dgssItem.setJointNo(jointNo);
			dgssItem.setUflgSplId(uflgSplId);
			dgssItem.setWebSplId(webSplId);
			dgssItem.setLflgSplId(lflgSplId);
			dgss.append(dgssItem);
		}
	}
	dgss.sort();
	dgss.toCsv(ofOb);

	return JPT_OK;
}

//４．横桁

//(１)共通詳細データ
JptErrorStatus AtdDbToCsv::dbToCsvCrossbeamCommon(
	AtdCrossBeamCommon& acc,
	AtdDbInputCbeam& icb,	//入力･横桁
	std::ofstream& ofOb)
{
	DiafCrossbeamCommon dcc;

	double shitenUflgJc = acc.getShitenUflgJc();	//支点上横桁（BH）上フランジのジョイントクリアランス
	double shitenWebJc = acc.getShitenWebJc();		//支点上横桁（BH）ウェブのジョイントクリアランス
	double shitenLflgJc = acc.getShitenLflgJc();	//支点上横桁（BH）下フランジのジョイントクリアランス
	double kakutenHflgJc = acc.getKakutenHflgJc();	//格点上横桁（H鋼）フランジのジョイントクリアランス
	double kakutenHwebJc = acc.getKakutenHwebJc();	//格点上横桁（H鋼）ウェブのジョイントクリアランス
	string shitenConnCut = acc.getShitenConnCut();									//主桁ウェブ付きコネクション（支点上）の溶接辺側切欠
	double shitenConnFillet = acc.getShitenConnFillet();							//主桁ウェブ付きコネクション（支点上）のフィレットのRサイズ
	double shitenConnTachiageryo = acc.getShitenConnTachiageryo();					//主桁ウェブ付きコネクション（支点上）の溶接辺立上げ量
	string kakutenConnCut = acc.getKakutenConnCut();								//主桁ウェブ付きコネクション（格点上）の溶接辺側切欠を指定
	double kakutenConnFillet = acc.getKakutenConnFillet();							//主桁ウェブ付きコネクション（格点上）のフィレットのRサイズ
	double kakutenConnTachiageryo = acc.getKakutenConnTachiageryo();				//主桁ウェブ付きコネクション（格点上）の溶接辺立上げ量
	string cvsCutWu = acc.getCvsCutWu();											//横桁付垂直補剛材の溶接辺側上側切欠
	string cvsCutWd = acc.getCvsCutWd();											//横桁付垂直補剛材の溶接辺側下側切欠
	double shitenUflgSplKyojikuZaitan = acc.getShitenUflgSplKyojikuZaitan();		//支点上横桁上フランジ添接板の橋軸方向材端
	double shitenUflgSplKyotyokuZaitan = acc.getShitenUflgSplKyotyokuZaitan();		//支点上横桁上フランジ添接板の橋直方向材端
	double shitenWebSplKyotyokuZaitan = acc.getShitenWebSplKyotyokuZaitan();		//支点上横桁ウェブ添接板の橋直方向材端
	double shitenWebSplHeightZaitan = acc.getShitenWebSplHeightZaitan();			//支点上横桁ウェブ添接板の高さ方向材端
	double shitenLflgSplKyojikuZaitan = acc.getShitenLflgSplKyojikuZaitan();		//支点上横桁下フランジ添接板の橋軸方向材端
	double shitenLflgSplKyotyokuZaitan = acc.getShitenLflgSplKyotyokuZaitan();		//支点上横桁下フランジ添接板の橋直方向材端
	double shitenConnKyojikuZaitan = acc.getShitenConnKyojikuZaitan();				//支点上コネクションの橋軸方向材端
	double shitenConnKyoutyokuZaitan = acc.getShitenConnKyoutyokuZaitan();			//支点上コネクションの橋直方向材端
	double kakutenUflgSplKyojikuZaitan = acc.getKakutenUflgSplKyojikuZaitan();		//格点上横桁上フランジ添接板の橋軸方向材端
	double kakutenUflgSplKyotyokuZaitan = acc.getKakutenUflgSplKyotyokuZaitan();	//格点上横桁上フランジ添接板の橋直方向材端
	double kakutenWebSplKyotyokuZaitan = acc.getKakutenWebSplKyotyokuZaitan();		//格点上横桁ウェブ添接板の橋直方向材端
	double kakutenWebSplHeightZaitan = acc.getKakutenWebSplHeightZaitan();			//格点上横桁ウェブ添接板の高さ方向材端
	double kakutenLflgSplKyojikuZaitan = acc.getKakutenLflgSplKyojikuZaitan();		//格点上横桁下フランジ添接板の橋軸方向材端
	double kakutenLflgSplKyotyokuZaitan = acc.getKakutenLflgSplKyotyokuZaitan();	//格点上横桁下フランジ添接板の橋直方向材端
	double kakutenConnKyojikuZaitan = acc.getKakutenConnKyojikuZaitan();			//格点上コネクションの橋軸方向材端
	double kakutenConnKyoutyokuZaitan = acc.getKakutenConnKyoutyokuZaitan();		//格点上コネクションの橋直方向材端
	double webHoleSlopeLowerLimit = acc.getWebHoleSlopeLowerLimitCrs();				//ウェブ孔の孔勾配の下限値
	EnTypeNo flgSectionType = acc.getFlgSectionType();								//フランジ切口の方向
	dcc.setShitenUflgJc(shitenUflgJc);
	dcc.setShitenWebJc(shitenWebJc);
	dcc.setShitenLflgJc(shitenLflgJc);
	dcc.setKakutenHflgJc(kakutenHflgJc);
	dcc.setKakutenHwebJc(kakutenHwebJc);
	dcc.setShitenConnCut(shitenConnCut);
	dcc.setShitenConnFillet(shitenConnFillet);
	dcc.setShitenConnTachiageryo(shitenConnTachiageryo);
	dcc.setKakutenConnCut(kakutenConnCut);
	dcc.setKakutenConnFillet(kakutenConnFillet);
	dcc.setKakutenConnTachiageryo(kakutenConnTachiageryo);
	dcc.setCvsCutWu(cvsCutWu);
	dcc.setCvsCutWd(cvsCutWd);
	dcc.setShitenUflgSplKyojikuZaitan(shitenUflgSplKyojikuZaitan);
	dcc.setShitenUflgSplKyotyokuZaitan(shitenUflgSplKyotyokuZaitan);
	dcc.setShitenWebSplKyotyokuZaitan(shitenWebSplKyotyokuZaitan);
	dcc.setShitenWebSplHeightZaitan(shitenWebSplHeightZaitan);
	dcc.setShitenLflgSplKyojikuZaitan(shitenLflgSplKyojikuZaitan);
	dcc.setShitenLflgSplKyotyokuZaitan(shitenLflgSplKyotyokuZaitan);
	dcc.setShitenConnKyojikuZaitan(shitenConnKyojikuZaitan);
	dcc.setShitenConnKyoutyokuZaitan(shitenConnKyoutyokuZaitan);
	dcc.setKakutenUflgSplKyojikuZaitan(kakutenUflgSplKyojikuZaitan);
	dcc.setKakutenUflgSplKyotyokuZaitan(kakutenUflgSplKyotyokuZaitan);
	dcc.setKakutenWebSplKyotyokuZaitan(kakutenWebSplKyotyokuZaitan);
	dcc.setKakutenWebSplHeightZaitan(kakutenWebSplHeightZaitan);
	dcc.setKakutenLflgSplKyojikuZaitan(kakutenLflgSplKyojikuZaitan);
	dcc.setKakutenLflgSplKyotyokuZaitan(kakutenLflgSplKyotyokuZaitan);
	dcc.setKakutenConnKyojikuZaitan(kakutenConnKyojikuZaitan);
	dcc.setKakutenConnKyoutyokuZaitan(kakutenConnKyoutyokuZaitan);
	dcc.setWebHoleSlopeLowerLimit(webHoleSlopeLowerLimit);
	dcc.setFlgSectionType(flgSectionType);

	dcc.toCsv(ofOb);

	return JPT_OK;
}

//(２)支点上横桁断面ID登録
JptErrorStatus AtdDbToCsv::dbToCsvCrossbeamSectionId(
	AtdDbInputUseMaterial& ium,	//入力・使用材料データ
	AtdDbSecCbeam& scb,			//断面･横桁
	AtdDbSecCbeamSec& scs,		//断面･横桁･断面
	AtdDbSplWeb& swb,			//添接･各添接･ウェブ
	AtdDbLineCbeam& lcb,		//線形･横桁(対傾構/ブラケット)
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafCrossbeamSectionId dcsi;

	int cnt = 0;
	for(int i=0;i<scs.size();i++) {
		AtdDbSecCbeamSecItem scsItem;
		scs.getAt(i, scsItem);
		int nocrs = scsItem.getNocrs();	//NOCRS
		double bfcu = scsItem.getBfcu();	//BFCU UFLG_幅 H鋼フランジ幅
		double tfcu = scsItem.getTfcu();	//TFCU UFLG_板厚 H鋼フランジ板厚
		int mfcu = scsItem.getMfcu();		//MFCU UFLG_材質 H鋼材質
		double tfcw = scsItem.getTfcw();	//TFCW WEB_板厚 H鋼ウェブ板厚
		// 2018/02/15 take Edit Start
		// MFCWの一桁目がWEB材質
		int mfcw = scsItem.getMfcw();
		mfcw = mfcw % 10;
		//int mfcw = scsItem.getMfcw();		//MFCW WEB_材質
		// 2018/02/15 take Edit End
		double bfcl = scsItem.getBfcl();	//BFCL LFLG_幅 H鋼フランジ幅
		double tfcl = scsItem.getTfcl();	//TFCL LFLG_板厚 H鋼フランジ板厚
		int mfcl = scsItem.getMfcl();		//MFCL LFLG_材質 H鋼材質
		if(tfcu < 0.0 || tfcl < 0.0) {
			continue;
		}
		double idx = scb.find(nocrs);
		if(idx < 0) {
			continue;
		}

		AtdDbSecCbeamItem scbItem;
		scb.getAt(idx, scbItem);
		int nocjul = scbItem.getNocjul();	//NOCJUL:上フランジ左側の添接番号
		int nocjll = scbItem.getNocjll();	//NOCJLL:下フランジ左側の添接番号
		int nocjwl = scbItem.getNocjwl();	//NOCJWL:ウェブ左側の添接番号
		int nocjur = scbItem.getNocjur();	//NOCJUR:上フランジ右側の添接番号
		int nocjlr = scbItem.getNocjlr();	//NOCJLR:下フランジ右側の添接番号
		int nocjwr = scbItem.getNocjwr();	//NOCJWR:ウェブ右側の添接番号
		int itpwj1 = swb.findItpwj(nocjwl);
		int itpwj2 = swb.findItpwj(nocjwr);
		if(itpwj1 == -1 || itpwj1 == 3 || itpwj2 == -1 || itpwj2 == 3) {
			continue;
		}
		_habaUflg.push_back(bfcu);	//UFLG_幅
		_atsuUflg.push_back(tfcu);	//UFLG_板厚
		_zaiUflg.push_back(mfcu);	//UFLG_材質
		_zaiLflg.push_back(mfcl);	//LFLG_材質
		_atsuWeb.push_back(tfcw);	//WEB_板厚
		_atsuLflg.push_back(tfcl);	//LFLG_板厚
		_crsNocjul.push_back(nocjul);
		_crsNocjll.push_back(nocjll);
		_crsNocjwl.push_back(nocjwl);
		_crsNocjur.push_back(nocjur);
		_crsNocjlr.push_back(nocjlr);
		_crsNocjwr.push_back(nocjwr);
		//
		double atsuVstf = 0.0;
		for(int ii=0;ii<_ngMax-1;ii++) {
			string leftKetaName = _ketaNameameList[ii];		//左側桁名
			string rightKetaName = _ketaNameameList[ii+1];	//右側桁名
			for(int jj=0;jj<(int)_oudanNameList.size();jj++) {
				int nosec = lcb.findNosec(ii+1, ii+2, jj+1);
				if(nocrs == nosec) {
					for(int k=0;k<(int)_grdKakuVstfKakutenNo.size();k++) {
						if(_grdKakuVstfKakutenNo[k] == jj+1 && ((_grdKakuVstfKetaNo[k] == ii+1 && _grdKakuVstfFace[k] == NS) || (_grdKakuVstfKetaNo[k] == ii+2 && _grdKakuVstfFace[k] == FS))) {
							atsuVstf = _grdKakuVstfitaastu[k];
							break;
						}
					}
				}
			}
		}
		_atsuTsukiVatf.push_back(atsuVstf);
		//
		cnt++;
		string crossbeamSectionId = "BH" + sh.toString(cnt);				//識別ID
		double uflgWidth = bfcu;											//上フランジ幅
		double uflgPlateThickness = tfcu;									//上フランジ板厚
		string uflgMaterial = ium.findMaterial(uflgPlateThickness, mfcu);	//上フランジ材質
		double webPlateThickness = fabs(tfcw);								//ウェブ板厚
		string webMaterial = ium.findMaterial(webPlateThickness, mfcw);		//ウェブ材質
		double lflgWidth = bfcl;											//下フランジ幅
		double lflgPlateThickness = tfcl;									//下フランジ板厚
		string lflgMaterial = ium.findMaterial(lflgPlateThickness, mfcl);	//下フランジ材質
		DiafCrossbeamSectionIdItem dcsiItem;
		dcsiItem.setCrossbeamSectionId(crossbeamSectionId);
		dcsiItem.setUflgWidth(uflgWidth);
		dcsiItem.setUflgPlateThickness(uflgPlateThickness);
		dcsiItem.setUflgMaterial(uflgMaterial);
		dcsiItem.setWebPlateThickness(webPlateThickness);
		dcsiItem.setWebMaterial(webMaterial);
		dcsiItem.setLflgWidth(lflgWidth);
		dcsiItem.setLflgPlateThickness(lflgPlateThickness);
		dcsiItem.setLflgMaterial(lflgMaterial);
		dcsi.append(dcsiItem);
		//横桁ビルド材
		int shiguchiType;
		if(tfcw > 0.0) {	//仕口タイプ
			shiguchiType = 0;
		} else {			//CONNタイプ
			shiguchiType = 1;
		}
		_crsHontaiType.push_back(0);	//横桁本体タイプ(0:ビルド材 1:Ｈ鋼)
		_crsShiguchiType.push_back(shiguchiType);	//横桁仕口タイプ(0:仕口 1:CONN)
		_crsbeamNocrs.push_back(nocrs);	//横桁本体仕口の番号
		_crsbeamIdname.push_back(crossbeamSectionId);	//横桁本体仕口識別名
	}
	dcsi.sort();
	dcsi.toCsv(ofOb);

	return JPT_OK;
}

//(３)格点上横桁Ｈ鋼ID登録
JptErrorStatus AtdDbToCsv::dbToCsvCrossbeamHbeamId(
AtdDbInputUseMaterial& ium,	//入力・使用材料データ
	AtdDbSecCbeam& scb,		//断面･横桁
	AtdDbSecCbeamSec& scs,	//断面･横桁･断面
	AtdDbSplWeb& swb,		//添接･各添接･ウェブ
	AtdDbLineCbeam& lcb,	//線形･横桁(対傾構/ブラケット)
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafCrossbeamHbeamId dchi;

	int cnt = 0;
	for(int i=0;i<scs.size();i++) {
		AtdDbSecCbeamSecItem scsItem;
		scs.getAt(i, scsItem);
		int nocrs = scsItem.getNocrs();		//NOCRS
		double bfcu = scsItem.getBfcu();	//BFCU UFLG_幅 H鋼フランジ幅
		double tfcu = scsItem.getTfcu();	//TFCU UFLG_板厚 H鋼フランジ板厚
		int mfcu = scsItem.getMfcu();		//MFCU UFLG_材質 H鋼材質
		double tfcw = scsItem.getTfcw();	//TFCW WEB_板厚 H鋼ウェブ板厚
		if(tfcu > 0.0) {
			continue;
		}
		double idx = scb.find(nocrs);
		if(idx < 0) {
			continue;
		}
		AtdDbSecCbeamItem scbItem;
		scb.getAt(idx, scbItem);
		int nocjul = scbItem.getNocjul();	//NOCJUL:上フランジ左側の添接番号
		int nocjll = scbItem.getNocjll();	//NOCJLL:下フランジ左側の添接番号
		int nocjwl = scbItem.getNocjwl();	//NOCJWL:ウェブ左側の添接番号
		int nocjur = scbItem.getNocjur();	//NOCJUR:上フランジ右側の添接番号
		int nocjlr = scbItem.getNocjlr();	//NOCJLR:下フランジ右側の添接番号
		int nocjwr = scbItem.getNocjwr();	//NOCJWR:ウェブ右側の添接番号
		int itpwj1 = swb.findItpwj(nocjwl);
		int itpwj2 = swb.findItpwj(nocjwr);
		if(itpwj1 == -1 || itpwj1 == 3 || itpwj2 == -1 || itpwj2 == 3) {
			continue;
		}
		_habaUflg.push_back(bfcu);	//UFLG_幅
		_atsuUflg.push_back(tfcu);	//UFLG_板厚
		_zaiUflg.push_back(mfcu);	//UFLG_材質
		_zaiLflg.push_back(mfcu);	//LFLG_材質
		_atsuWeb.push_back(tfcw);	//WEB_板厚
		_atsuLflg.push_back(tfcu);	//LFLG_板厚
		_crsNocjul.push_back(nocjul);
		_crsNocjll.push_back(nocjll);
		_crsNocjwl.push_back(nocjwl);
		_crsNocjur.push_back(nocjur);
		_crsNocjlr.push_back(nocjlr);
		_crsNocjwr.push_back(nocjwr);
		//
		double atsuVstf = 0.0;
		for(int ii=0;ii<_ngMax-1;ii++) {
			string leftKetaName = _ketaNameameList[ii];		//左側桁名
			string rightKetaName = _ketaNameameList[ii+1];	//右側桁名
			for(int jj=0;jj<(int)_oudanNameList.size();jj++) {
				int nosec = lcb.findNosec(ii+1, ii+2, jj+1);
				if(nocrs == nosec) {
					for(int k=0;k<(int)_grdKakuVstfKakutenNo.size();k++) {
						if(_grdKakuVstfKakutenNo[k] == jj+1 && ((_grdKakuVstfKetaNo[k] == ii+1 && _grdKakuVstfFace[k] == NS) || (_grdKakuVstfKetaNo[k] == ii+2 && _grdKakuVstfFace[k] == FS))) {
							atsuVstf = _grdKakuVstfitaastu[k];
							break;
						}
					}
				}
			}
		}
		_atsuTsukiVatf.push_back(atsuVstf);
		//
		cnt++;
		string crossbeamHbeamId = "RH" + sh.toString(cnt);	//識別ID
		double hbeamWebHeight = scb.findWebHeight(nocrs);;	//H鋼ウェブ高
		double hbeamFlgWidth = bfcu;						//H鋼フランジ幅
		double hbeamWebPlateThickness = fabs(tfcw);			//H鋼ウェブ板厚
		double hbeamFlgPlateThickness = fabs(tfcu);			//H鋼フランジ板厚
		string hbeamMaterial = ium.findMaterial(hbeamWebPlateThickness, mfcu);			//H鋼材質
		DiafCrossbeamHbeamIdItem dchiItem;
		dchiItem.setCrossbeamHbeamId(crossbeamHbeamId);
		dchiItem.setHbeamWebHeight(hbeamWebHeight);
		dchiItem.setHbeamFlgWidth(hbeamFlgWidth);
		dchiItem.setHbeamWebPlateThickness(hbeamWebPlateThickness);
		dchiItem.setHbeamFlgPlateThickness(hbeamFlgPlateThickness);
		dchiItem.setHbeamMaterial(hbeamMaterial);
		dchi.append(dchiItem);
		//横桁Ｈ鋼
		int shiguchiType;
		if(tfcw > 0.0) {	//仕口タイプ(通常は有り得ない)
			shiguchiType = 0;
		} else {			//CONNタイプ
			shiguchiType = 1;
		}_crsHontaiType.push_back(1);				//横桁本体タイプ(0:ビルド材 1:Ｈ鋼)
		_crsShiguchiType.push_back(shiguchiType);	//横桁仕口タイプ(0:仕口 1:CONN)
		_crsbeamNocrs.push_back(nocrs);				//横桁本体仕口の番号
		_crsbeamIdname.push_back(crossbeamHbeamId);	//横桁本体仕口識別名
	}
	dchi.sort();
	dchi.toCsv(ofOb);

	return JPT_OK;
}

//(４)支点上横桁フランジ添接形状定義
JptErrorStatus AtdDbToCsv::dbToCsvCrossbeamFlgSplId(
AtdDbInputUseMaterial& ium,	//入力・使用材料データ
	AtdDbSplCommon& scm,	//添接･線形添接共通データ
	AtdDbSplFlg& sfg,		//添接･各添接･フランジ
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafCrossbeamFlgSplId dcfsi;

	//孔径
	double holeSize = scm.findHoleSize(4);

	//添接･各添接･フランジ
	int cnt = 0;
	for(int i=0;i<sfg.size();i++) {
		AtdDbSplFlgItem sfgItem;
		sfg.getAt(i, sfgItem);
		int strcode = sfgItem.getStrcode();	//STRCODE
		if(strcode != 4) {
			continue;
		}
		int pos = sfgItem.getPos();		//POS
		int iupdw = sfgItem.getIupdw();	//IUPDW 上下
		int noj = sfgItem.getNoj();		//NOJ ジョイント番号
		int ifmj = sfgItem.getIfmj();	//IFMJ 孔タイプ FLG孔_タイプ
		double poj = sfgItem.getPoj();	//POJ CP
		double pj = sfgItem.getPj();	//PJ P1 P FLG孔_P
		int npj = sfgItem.getNpj();		//NPJ P1の数
		double goj = sfgItem.getGoj();	//GOJ CG FLG孔_CG
		double gj = sfgItem.getGj();	//GJ G FLG孔_G
		int ngj = sfgItem.getNgj();		//NGJ Gの数
		int b1j = sfgItem.getB1j();		//B1J SPLタイプ SPL_タイプ
		double t1j = sfgItem.getT1j();	//T1J NS板厚 SPL_板厚（外側） SPL_板厚（内側）
		double t2j = sfgItem.getT2j();	//T2J FS板厚 CONN_板厚
		int mtj = sfgItem.getMtj();		//MTJ NS材質 FS材質 SPL_材質（外側） SPL_材質（内側）
		if(iupdw == 1) {	//配置は上下共通なので上側のみ登録
			continue;
		}
		bool flag = false;
		for(int k=0;k<_crsNocjul.size();k++) {
			if(_crsShiguchiType[k] == 1) {
				continue;
			}
			if(iupdw == 0) {	//上
				if(_crsNocjul[k] == noj || _crsNocjur[k] == noj) {
					flag = true;
					break;
				}
			} else {			//下
				if(_crsNocjll[k] == noj || _crsNocjlr[k] == noj) {
					flag = true;
					break;
				}
			}
		}
		if(flag != true) {
			continue;
		}
		cnt++;
		string crossbeamFlgSplId = "CFS" + sh.toString(cnt);	//識別ID
		EnHoleType holeType;			//孔タイプ
		if(ifmj == 1) {			//格子
			holeType = KOUSHI;			//	格子
		} else if(ifmj == 2) {	//千鳥1
			holeType = CHIDORI1;		//	千鳥1
		} else {				//千鳥3
			holeType = CHIDORI3;		//	千鳥3
		}
		double centerPitch = poj;				//センターピッチ
		string pitch = makePitchGage(npj, pj);					//ピッチ
		double centerGauge = goj;				//センターゲージ
		string gauge = makePitchGage(ngj, gj);					//ゲージ
		EnTypeNo splType;				//添接板タイプ
		if(b1j > 0) {	//タイプ1
			splType = TYPE1;
		} else {		//タイプ2
			splType = TYPE2;
		}
		double nsPlateThickness = t1j;			//オモテ側添接板の板厚
		double fsPlateThickness = t2j;			//ウラ側添接板の板厚
		string nsMaterial = ium.findMaterial(nsPlateThickness, mtj);			//オモテ側添接板の材質
		string fsMaterial = ium.findMaterial(fsPlateThickness, mtj);			//ウラ側添接板の材質
		double fillerPlateThickness = 0.0;	//フィラーの板厚(本体と仕口が同じなのでフィラーなし)
		string fillerMaterial = "";		//フィラーの材質(本体と仕口が同じなのでフィラーなし)
		DiafCrossbeamFlgSplIdItem dcfsiItem;
		dcfsiItem.setCrossbeamFlgSplId(crossbeamFlgSplId);
		dcfsiItem.setHoleType(holeType);
		dcfsiItem.setHoleSize(holeSize);
		dcfsiItem.setCenterPitch(centerPitch);
		dcfsiItem.setPitch(pitch);
		dcfsiItem.setCenterGauge(centerGauge);
		dcfsiItem.setGauge(gauge);
		dcfsiItem.setSplType(splType);
		dcfsiItem.setNsPlateThickness(nsPlateThickness);
		dcfsiItem.setNsMaterial(nsMaterial);
		dcfsiItem.setFsPlateThickness(fsPlateThickness);
		dcfsiItem.setFsMaterial(fsMaterial);
		dcfsiItem.setFillerPlateThickness(fillerPlateThickness);
		dcfsiItem.setFillerMaterial(fillerMaterial);
		dcfsi.append(dcfsiItem);
		_crsFsplNoj.push_back(noj);
		_crsFsplPos.push_back(pos);						//0:端支点、1:中間支点、2:格点
		_crsFsplPupdw.push_back(iupdw);					//0:上 1:下
		_crsFsplIdname.push_back(crossbeamFlgSplId);	//横桁フランジ添接形状識別名
	}
	dcfsi.sort();
	dcfsi.toCsv(ofOb);

	return JPT_OK;
}

//(５)コネクション・フランジ添接形状定義
JptErrorStatus AtdDbToCsv::dbToCsvCrossbeamConnFsplId(
AtdDbInputUseMaterial& ium,			//入力・使用材料データ
	AtdDbSplCommon& scm,			//添接･線形添接共通データ
	AtdDbSplFlg& sfg,				//添接･各添接･フランジ
	AtdDbSecScaleFill& ssf,			//断面･スケール及び文字高さと材質仕様･フィラープレート
	AtdDbInputFillOption& ifo,		//入力･フィラープレートオプション
	AtdDbInputCbeamConnSpl& icc,	//入力・横桁コネクション添接
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafCrossbeamConnFsplId dccfi;

	//孔径
	double holeSize = scm.findHoleSize(4);

	//断面･スケール及び文字高さと材質仕様･フィラープレート
	int mtfill = 0;
	double rfilfu = 0.0;	//RFILFU
	double rfilfl = 0.0;	//RFILFL
	if(ssf.size() > 0) {
		AtdDbSecScaleFillItem ssfItem;
		ssf.getAt(0, ssfItem);
		mtfill = ssfItem.getMtfill();	//MTFILL Fill材質
		rfilfu = ssfItem.getRfilfu();
		rfilfl = ssfItem.getRfilfl();
	}

	//入力･フィラープレートオプション
	int ifilfu = 0;	//IFILFU
	int ifilfl = 0;	//IFILFL
	if(ifo.size() > 0) {
		AtdDbInputFillOptionItem ifoItem;
		ifo.getAt(0, ifoItem);
		ifilfu = ifoItem.getIfilfu();
		ifilfl = ifoItem.getIfilfl();
	}

	//添接･各添接･フランジ
	int cnt = 0;
	for(int i=0;i<sfg.size();i++) {
		AtdDbSplFlgItem sfgItem;
		sfg.getAt(i, sfgItem);
		int strcode = sfgItem.getStrcode();	//STRCODE
		if(strcode != 4) {
			continue;
		}
		int pos = sfgItem.getPos();		//POS
		int iupdw = sfgItem.getIupdw();	//IUPDW 上下
		int noj = sfgItem.getNoj();		//NOJ ジョイント番号
		int ifmj = sfgItem.getIfmj();	//IFMJ 孔タイプ FLG孔_タイプ
		double pj = sfgItem.getPj();	//PJ P1 P FLG孔_P
		int npj = sfgItem.getNpj();		//NPJ P1の数
		double goj = sfgItem.getGoj();	//GOJ CG FLG孔_CG
		double gj = sfgItem.getGj();	//GJ G FLG孔_G
		int ngj = sfgItem.getNgj();		//NGJ Gの数
		int b1j = sfgItem.getB1j();		//B1J SPLタイプ SPL_タイプ
		double t1j = sfgItem.getT1j();	//T1J NS板厚 SPL_板厚（外側） SPL_板厚（内側）
		double t2j = sfgItem.getT2j();	//T2J FS板厚 CONN_板厚
		int mtj = sfgItem.getMtj();		//MTJ NS材質 FS材質 SPL_材質（外側） SPL_材質（内側）
		double gjc = sfgItem.getGjc();	//GJC CONN孔_寸法X
		int ngjc = sfgItem.getNgjc();	//NGJC CONN孔_寸法Xの数
		double pjc = sfgItem.getPjc();	//PJC CONN孔_寸法Y
		int npjc = sfgItem.getNpjc();	//NPJC CONN孔_寸法Yの数
		if(iupdw == 1) {	//配置は上下共通なので上側のみ登録
			continue;
		}
		int kk = 0;
		bool flag = false;
		for(int k=0;k<_crsNocjul.size();k++) {
			if(_crsShiguchiType[k] == 0) {
				continue;
			}
			if(iupdw == 0) {	//上
				if(_crsNocjul[k] == noj || _crsNocjur[k] == noj) {
					flag = true;
					kk = k;
					break;
				}
			} else {			//下
				if(_crsNocjll[k] == noj || _crsNocjlr[k] == noj) {
					flag = true;
					kk = k;
					break;
				}
			}
		}
		if(flag != true) {
			continue;
		}
		cnt++;
		string crossbeamConnFsplId = "CCS" + sh.toString(cnt);	//識別ID
		double connPlateThickness = t2j;						//コネクション板厚
		double edgec = 0.0;
		double gagec = 0.0;
		double gagecenc = 0.0;
		int idx = icc.find(noj, iupdw);	//入力・横桁コネクション添接
		if(idx > -1) {
			AtdDbInputCbeamConnSplItem iccItem;
			icc.getAt(idx, iccItem);
			edgec = iccItem.getEdgec();			//EDGEC ConnPL連結外側縁端
			gagec = iccItem.getGagec();			//GAGEC ConnPLゲージ
			gagecenc = iccItem.getGagecenc();	//GAGECENC ConnPL中心ゲージ
		} else {
//err
		}
		double atsuVstf = fabs(_atsuTsukiVatf[kk]);	//突き合わせVSTF板厚
		double connWidth = edgec + gagec + (gagecenc/2 + atsuVstf/2);				//コネクション幅
//		string connMaterial = ium.findMaterial(connPlateThickness, _zaiUflg[kk]);	//コネクション材質
		string connMaterial = ium.findMaterial(_atsuUflg[kk], _zaiUflg[kk]);		//コネクション材質
		EnHoleType flgHoleType;			//孔タイプ
		// 2018/02/15 take Edit Start
		// フランジの孔タイプは、格子か内千鳥
		if(ifmj == 1) {			//格子
			flgHoleType = KOUSHI;		//格子
		} else if(ifmj == 2) {	//内千鳥
			flgHoleType = UCHICHIDORI;	//内千鳥
		}
		//if(ifmj == 1) {			//格子
		//	flgHoleType = KOUSHI;		//格子
		//} else if(ifmj == 2) {	//千鳥1
		//	flgHoleType = CHIDORI1;		//千鳥1
		//} else {				//千鳥3
		//	flgHoleType = CHIDORI3;		//千鳥3
		//}
		// 2018/02/15 take Edit End
		string flgHolePitch = makePitchGage(npj, pj);		//フランジ孔のピッチ
		string flgHoleGauge = makePitchGage(ngj, gj);		//フランジ孔のゲージ
		double flgHoleCenterGauge = goj;					//フランジ孔のセンターゲージ
		string connHoleDimX = makePitchGage(ngjc, gjc);		//コネクション孔の寸法（X方向）
		string connHoleDimY = makePitchGage(npjc, pjc);		//コネクション孔の寸法（Y方向）
		EnTypeNo splType;				//添接板タイプ
		if(b1j > 0) {	//タイプ1
			splType = TYPE1;
		} else {		//タイプ2
			splType = TYPE2;
		}
		double splPlateThicknessOutside = t1j;		//添接板の板厚（外側）
		double splPlateThicknessInside = t1j;		//添接板の板厚（内側）
		string splMaterialOutside = ium.findMaterial(splPlateThicknessOutside, mtj);	//添接板の材質（外側）
		string splMaterialInside = ium.findMaterial(splPlateThicknessInside, mtj);		//添接板の材質（内側）
		// 2018/02/15 take Edit Start
		// フィラーを作成しない場合は、『-1.0』とする
		double fillerPlateThickness = -1.0;
		//double fillerPlateThickness = 0.0;	//フィラーの板厚
		// 2018/02/15 take Edit End
		string fillerMaterial = "";			//フィラーの材質
		if((iupdw == 0 && ifilfu == 1) ||(iupdw == 1 && ifilfl == 1)) {	//フィラー配置
			double atsu1 = fabs(_atsuUflg[kk]);			//UFLG_板厚
			double atsu2 = fabs(connPlateThickness); 	//コネクション板厚
			double atsuSa = fabs(atsu1-atsu2);
			if((iupdw == 0 && rfilfu <= atsuSa) ||(iupdw == 1 && rfilfl <= atsuSa)) {	//板厚差あり
				fillerPlateThickness = getFillItaatsu(atsuSa);						//フィラーの板厚
				fillerMaterial = ium.findMaterial(fillerPlateThickness, mtfill);	//フィラーの材質
			}
		}
		DiafCrossbeamConnFsplIdItem dccfiItem;
		dccfiItem.setCrossbeamConnFsplId(crossbeamConnFsplId);
		dccfiItem.setConnPlateThickness(connPlateThickness);
		dccfiItem.setConnWidth(connWidth);
		dccfiItem.setConnMaterial(connMaterial);
		dccfiItem.setHoleSize(holeSize);
		dccfiItem.setFlgHoleType(flgHoleType);
		dccfiItem.setFlgHolePitch(flgHolePitch);
		dccfiItem.setFlgHoleGauge(flgHoleGauge);
		dccfiItem.setFlgHoleCenterGauge(flgHoleCenterGauge);
		dccfiItem.setConnHoleDimX(connHoleDimX);
		dccfiItem.setConnHoleDimY(connHoleDimY);
		dccfiItem.setSplType(splType);
		dccfiItem.setSplPlateThicknessOutside(splPlateThicknessOutside);
		dccfiItem.setSplMaterialOutside(splMaterialOutside);
		dccfiItem.setSplPlateThicknessInside(splPlateThicknessInside);
		dccfiItem.setSplMaterialInside(splMaterialInside);
		dccfiItem.setFillerPlateThickness(fillerPlateThickness);
		dccfiItem.setFillerMaterial(fillerMaterial);
		dccfi.append(dccfiItem);
		_crsConnNoj.push_back(noj);
		_crsConnPos.push_back(pos);		//0:端支点、1:中間支点、2:格点
		_crsConnPupdw.push_back(iupdw);		//0:上 1:下
		_crsConnIdname.push_back(crossbeamConnFsplId);	//横桁コネクション・フランジ添接形状識別名
	}
	dccfi.sort();
	dccfi.toCsv(ofOb);

	return JPT_OK;
}

//(６)ウェブ添接形状定義
JptErrorStatus AtdDbToCsv::dbToCsvCrossbeamWsplId(
AtdDbInputUseMaterial& ium,		//入力・使用材料データ
	AtdDbSplCommon& scm,		//添接･線形添接共通データ
	AtdDbSplWeb& swb,			//添接･各添接･ウェブ
	AtdDbSecScaleFill& ssf,		//断面･スケール及び文字高さと材質仕様･フィラープレート
	AtdDbInputFillOption& ifo,	//入力･フィラープレートオプション
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafCrossbeamWsplId dcwi;

	//孔径
	double holeSize = scm.findHoleSize(4);

	//断面･スケール及び文字高さと材質仕様･フィラープレート
	int mtfill = 0;
	double rfilwb = 0.0;	//RFILWB
	if(ssf.size() > 0) {
		AtdDbSecScaleFillItem ssfItem;
		ssf.getAt(0, ssfItem);
		mtfill = ssfItem.getMtfill();	//MTFILL Fill材質
		rfilwb = ssfItem.getRfilwb();
	}

	//入力･フィラープレートオプション
	int ifilwb = 0;	//IFILWB
	if(ifo.size() > 0) {
		AtdDbInputFillOptionItem ifoItem;
		ifo.getAt(0, ifoItem);
		ifilwb = ifoItem.getIfilwb();
	}

	//添接･各添接･ウェブ
	int cnt = 0;
	for(int i=0;i<swb.size();i++) {
		AtdDbSplWebItem swbItem;
		swb.getAt(i, swbItem);
		int strcode = swbItem.getStrcode();	//STRCODE
		if(strcode != 4) {
			continue;
		}
		int pos = swbItem.getPos();	//POS
		int nowj = swbItem.getNowj();	//NOWJ
		double gwj = swbItem.getGwj();	//GWJ P
		int ng2wj = swbItem.getNg2wj();		//NG2WJ Pの数
		double gowj = swbItem.getGowj();	//GOWJ CP
		double p2wj = swbItem.getP2wj();	//P2WJ 上側G G
		int nd2wj = swbItem.getNd2wj();		//ND2WJ 上側Gの数
		double p0wj = swbItem.getP0wj();	//P0WJ 上側空き量
		double p00j = swbItem.getP00j();	//P00J 下側空き量
		double t2wj = swbItem.getT2wj();	//T2WJ SPL板厚 SPL_板厚
		int mtwj = swbItem.getMtwj();		//MTWJ SPL材質 SPL_材質
		int kk;
		bool flag = false;
		for(int k=0;k<_crsNocjwl.size();k++) {
			if(_crsNocjwl[k] == nowj || _crsNocjwr[k] == nowj) {
				flag = true;
				kk = k;
				break;
			}
		}
		if(flag != true) {
			continue;
		}
		//
		cnt++;
		string crossbeamWsplId = "CWS" + sh.toString(cnt);	//識別ID
		double centerPitch = gowj;			//ウェブ孔のセンターピッチ
		string pitch = makePitchGage(ng2wj, gwj);				//ウェブ孔のピッチ
		string gauge = makePitchGage(nd2wj, p2wj);				//ウェブ孔のゲージ
		double upperFreeSpace = p0wj;			//ウェブ上端から最上段孔までの空き量
		double lowerFreeSpace = p00j;			//ウェブ下端から最下段孔までの空き量
		double splPlateThickness = t2wj;		//添接板の板厚
		string splMaterial = ium.findMaterial(splPlateThickness, mtwj);		//添接板の材質
		// 2018/02/16 take Edit Start
		// フィラーを作成しない場合は、『-1.0』とする
		double fillerPlateThickness = -1.0;
		//double fillerPlateThickness = 0.0;	//フィラーの板厚
		// 2018/02/16 take Edit End
		string fillerMaterial = "";	//フィラーの材質
		if(ifilwb == 1) {	//フィラー配置
			double atsu1 = fabs(_atsuWeb[kk]);			//WEB_板厚
			double atsu2 = fabs(_atsuTsukiVatf[kk]);	//突き合わせVSTF板厚
			double atsuSa = fabs(atsu1-atsu2);
			if(rfilwb <= atsuSa) {	//板厚差あり
				fillerPlateThickness = getFillItaatsu(atsuSa);						//フィラーの板厚
				fillerMaterial = ium.findMaterial(fillerPlateThickness, mtfill);	//フィラーの材質
			}
		}
		DiafCrossbeamWsplIdItem dcwiItem;
		dcwiItem.setCrossbeamWsplId(crossbeamWsplId);
		dcwiItem.setHoleSize(holeSize);
		dcwiItem.setCenterPitch(centerPitch);
		dcwiItem.setPitch(pitch);
		dcwiItem.setGauge(gauge);
		dcwiItem.setUpperFreeSpace(upperFreeSpace);
		dcwiItem.setLowerFreeSpace(lowerFreeSpace);
		dcwiItem.setSplPlateThickness(splPlateThickness);
		dcwiItem.setSplMaterial(splMaterial);
		dcwiItem.setFillerPlateThickness(fillerPlateThickness);
		dcwiItem.setFillerMaterial(fillerMaterial);
		dcwi.append(dcwiItem);
		_crsWsplNowj.push_back(nowj);
		_crsWsplPos.push_back(pos);		//0:端支点、1:中間支点、2:格点
		_crsWsplIdname.push_back(crossbeamWsplId);	//横桁ウェブ添接板識別名
	}
	dcwi.sort();
	dcwi.toCsv(ofOb);

	return JPT_OK;
}

//(７)垂直補剛材ID登録
JptErrorStatus AtdDbToCsv::dbToCsvCrossbeamVstfId(
	AtdDbInputUseMaterial& ium,	//入力・使用材料データ
	AtdDbSecCbeamStf& sct	,	//断面･横桁･補剛材断面数
	AtdDbSecCbeamVstfNum& scv,	//断面･横桁･パネル毎のVSTF本数
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafCrossbeamVstfId dcvi;

	int cnt = 0;
	for(int i=0;i<sct.size();i++) {
		AtdDbSecCbeamStfItem sctItem;
		sct.getAt(i, sctItem);
		int nocrs = sctItem.getNocrs();		//NOCRS
		int posstf = sctItem.getPosstf();	//POSSTF
		double stcw = sctItem.getStcw();	//STCW 幅
		double stct = sctItem.getStct();	//STCT 板厚
		int stcm = sctItem.getStcm();		//STCM 材質
		if(posstf != 1 && posstf != 2) {
			continue;
		}
		if(stcw < 1.0 || stct < 1.0) {
			continue;
		}
		string cBramId = findIdName(nocrs, _crsbeamNocrs, _crsbeamIdname);
		if(cBramId == "") {
			continue;
		}
		int nvstc = scv.findNvstc(nocrs);	//断面･横桁･パネル毎のVSTF本数
		if(nvstc < 1) {
			continue;
		}
		cnt++;
		string crossbeamVstfId = "CVS" + sh.toString(cnt);	//識別ID
		EnTypeNo crossbeamVstfType = TYPE1;	//垂直補剛材のタイプ
		double uflgHaba = findDouble(nocrs, _crsbeamNocrs, _habaUflg);
		double webAtsu = findDouble(nocrs, _crsbeamNocrs, _atsuWeb);
		if((uflgHaba-webAtsu)/2 < stcw) {
			crossbeamVstfType = TYPE1;
		} else {
			crossbeamVstfType = TYPE2;
		}
		double width = stcw;			//幅
		double plateThickness = stct;	//板厚
		string material = ium.findMaterial(plateThickness, stcm);	//材質
		DiafCrossbeamVstfIdItem dcviItem;
		dcviItem.setCrossbeamVstfId(crossbeamVstfId);
		dcviItem.setCrossbeamVstfType(crossbeamVstfType);
		dcviItem.setWidth(width);
		dcviItem.setPlateThickness(plateThickness);
		dcviItem.setMaterial(material);
		dcvi.append(dcviItem);
		_crsVstfNvstc.push_back(nvstc);	//横桁垂直補剛材の本数
		_crsVstfNocrs.push_back(nocrs);	//横桁垂直補剛材の番号
		_crsVstfIdname.push_back(crossbeamVstfId);	//横桁垂直補剛材識別名
	}
	dcvi.sort();
	dcvi.toCsv(ofOb);

	return JPT_OK;
}

//(８)横桁配置
JptErrorStatus AtdDbToCsv::dbToCsvCrossbeamSet(
	AtdDbSecCbeam& scb,				//断面･横桁
	AtdDbLineCbeam& lcb			,	//線形･横桁(対傾構/ブラケット)
	AtdDbLineGrdPanel& lgp,			//線形･主桁(横桁/ブラケット)･パネル長
	AtdDbInputHaraikomiHoko& ihh,	//入力･払い込み方向
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafCrossbeamSet dcs;

	for(int i=0;i<_ngMax-1;i++) {
		string leftKetaName = _ketaNameameList[i];		//左側桁名
		string rightKetaName = _ketaNameameList[i+1];	//右側桁名
		for(int j=0;j<(int)_oudanNameList.size();j++) {
			int nostr = lcb.findNostr(i+1, i+2, j+1);
			int nosec = lcb.findNosec(i+1, i+2, j+1);
			if(nosec < 1) {
				continue;
			}
			int idx = scb.find(nosec);
			if(idx < 0) {
				continue;
			}
			string kakutenName = _oudanNameList[j];		//格点名
			int kakutenNo = j+1;						//格点番号
			AtdDbSecCbeamItem scbItem;
			scb.getAt(idx, scbItem);
			double luLength = scbItem.getH1cl();	//左側主桁上端から横桁ウェブ上端までの距離
			double ldLength = scbItem.getH1cr();	//左側主桁下端から横桁ウェブ下端までの距離
			double ruLength = scbItem.getH2cl();	//右側主桁上端から横桁ウェブ上端までの距離
			double rdLength = scbItem.getH2cr();	//右側主桁下端から横桁ウェブ下端までの距離
			double ljLength = scbItem.getDspll();	//左側主桁ウェブ芯から横桁ジョイント位置までの距離
			double rjLength = scbItem.getDsplr();	//右側主桁ウェブ芯から横桁ジョイント位置までの距離
			double lcLength = scbItem.getH1cl();	//左側主桁上端から横桁ウェブ芯までの距離
			double rcLength = scbItem.getH1cr();	//右側主桁上端から横桁ウェブ芯までの距離
			string crossbeamId = "";		//配置する横桁
			string leftShiguchiId = "";		//左側仕口
			string rightShiguchiId = "";	//右側仕口
			string leftFsplId = "";			//左側フランジ添接形状
			string rightFsplId = "";		//右側フランジ添接形状
			string leftConnId = "";			//左側コネクション・フランジ添接形状
			string rightConnId = "";		//右側コネクション・フランジ添接形状
			string leftWsplId = "";			//左側ウェブ添接板
			string rightWsplId = "";		//右側ウェブ添接板
			string vstfId = "";				//横桁に配置する垂直補剛材
			//横桁識別名検索
			int shiguchiType = findType(nosec, _crsbeamNocrs, _crsShiguchiType);
			string cBramId = findIdName(nosec, _crsbeamNocrs, _crsbeamIdname);
			if(cBramId == "") {
				continue;
			}
			int nocjul = findType(nosec, _crsbeamNocrs, _crsNocjul);	//NOCJUL:上フランジ左側の添接番号
			int nocjll = findType(nosec, _crsbeamNocrs, _crsNocjll);	//NOCJLL:下フランジ左側の添接番号
			int nocjwl = findType(nosec, _crsbeamNocrs, _crsNocjwl);	//NOCJWL:ウェブ左側の添接番号
			int nocjur = findType(nosec, _crsbeamNocrs, _crsNocjur);	//NOCJUR:上フランジ右側の添接番号
			int nocjlr = findType(nosec, _crsbeamNocrs, _crsNocjlr);	//NOCJLR:下フランジ右側の添接番号
			int nocjwr = findType(nosec, _crsbeamNocrs, _crsNocjwr);	//NOCJWR:ウェブ右側の添接番号
			crossbeamId = cBramId;		//配置する横桁
			if(shiguchiType == 0) {	//仕口タイプ
				// 2018/02/15 take Add Start
				// 仕口タイプで不必要な要素はブランクとする
				lcLength = -1.0;
				rcLength = -1.0;
				// 2018/02/15 take Add End
				leftShiguchiId = cBramId;	//左側仕口
				rightShiguchiId = cBramId;	//右側仕口
				leftFsplId = findIdName(nocjul, _crsFsplNoj, _crsFsplIdname);	//左側フランジ添接形状
				rightFsplId = findIdName(nocjur, _crsFsplNoj, _crsFsplIdname);	//右側フランジ添接形状
			} else {
				// 2018/02/15 take Add Start
				// 主桁取付VSTFタイプで不必要な要素はブランクとする
				luLength = -1.0;
				ldLength = -1.0;
				ruLength = -1.0;
				rdLength = -1.0;
				ljLength = -1.0;
				rjLength = -1.0;
				// 2018/02/15 take Add End
				leftConnId = findIdName(nocjul, _crsConnNoj, _crsConnIdname);	//左側コネクション・フランジ添接形状
				rightConnId = findIdName(nocjur, _crsConnNoj, _crsConnIdname);	//右側コネクション・フランジ添接形状
			}
			leftWsplId = findIdName(nocjwl, _crsWsplNowj, _crsWsplIdname);		//左側ウェブ添接板
			rightWsplId = findIdName(nocjwr, _crsWsplNowj, _crsWsplIdname);		//右側ウェブ添接板
			//VSTF
			// 2018/02/15 take Edit Start
			// 初期値変更
			double leftVstfDim = -1.0;		
			string centerVstfPitch = "-";	
			double rightVstfDim = -1.0;	
			EnFace vstfSetFace = NON;
			if( cBramId.find("BH") != std::string::npos ){
				leftVstfDim = 0.0;
				rightVstfDim = 0.0;
			}
			//double leftVstfDim = 0.0;		//左側VSTF位置の寸法
			//string centerVstfPitch = "";	//中央VSTF位置のピッチ
			//double rightVstfDim = 0.0;	//右側VSTF位置の寸法
			//EnFace vstfSetFace = NS;		//横桁垂直補剛材の配置面
			// 2018/02/15 take Edit End
			vstfId = findIdName(nosec, _crsVstfNocrs, _crsVstfIdname);
			if(vstfId != "") {
				int nvstf = findType(nosec, _crsVstfNocrs, _crsVstfNvstc);	//横桁垂直補剛材の本数
				if(nvstf > 0) {
					double rlp = lgp.findRlp(4, nostr, 1);
					double vsKyori = rlp / (nvstf+1);
					leftVstfDim = vsKyori;
					if(nvstf > 1) {
						rightVstfDim = vsKyori;
					}
					if(nvstf > 2) {
						if(nvstf == 3) {
							centerVstfPitch = vsKyori;
						} else {
							centerVstfPitch = sh.toString(nvstf-2) + "@" + sh.toString(vsKyori);
						}
					}
					int itphr = ihh.findItphr(nostr);	//入力･払い込み方向
					if(itphr % 10 == 0) {	//左側
						vstfSetFace = FS;
					} else {				//右側
						vstfSetFace = NS;
					}
				}
			}
			DiafCrossbeamSetItem dcsItem;
			dcsItem.setLeftKetaName(leftKetaName);
			dcsItem.setRightKetaName(rightKetaName);
			dcsItem.setKakutenName(kakutenName);
			dcsItem.setKakutenNo(kakutenNo);
			dcsItem.setLuLength(luLength);
			dcsItem.setLdLength(ldLength);
			dcsItem.setRuLength(ruLength);
			dcsItem.setRdLength(rdLength);
			dcsItem.setLjLength(ljLength);
			dcsItem.setRjLength(rjLength);
			dcsItem.setLcLength(lcLength);
			dcsItem.setRcLength(rcLength);
			dcsItem.setCrossbeamId(crossbeamId);
			dcsItem.setLeftShiguchiId(leftShiguchiId);
			dcsItem.setRightShiguchiId(rightShiguchiId);
			dcsItem.setLeftFsplId(leftFsplId);
			dcsItem.setRightFsplId(rightFsplId);
			dcsItem.setLeftConnId(leftConnId);
			dcsItem.setRightConnId(rightConnId);
			dcsItem.setLeftWsplId(leftWsplId);
			dcsItem.setRightWsplId(rightWsplId);
			dcsItem.setLeftVstfDim(leftVstfDim);
			dcsItem.setCenterVstfPitch(centerVstfPitch);
			dcsItem.setRightVstfDim(rightVstfDim);
			dcsItem.setVstfId(vstfId);
			dcsItem.setVstfSetFace(vstfSetFace);
			dcs.append(dcsItem);
		}
	}
	dcs.sort();
	dcs.toCsv(ofOb);

	return JPT_OK;
}

double AtdDbToCsv::findGrdKakutenVstfItaatsu(const string& ketaName, const string& kakutenName, const EnFace& face)
{
	for(int i=0;i<(int)_grdKakuVstfKetaName.size();i++) {
		if(_grdKakuVstfKetaName[i] == ketaName && _grdKakuVstfKakutenName[i] == kakutenName && _grdKakuVstfFace[i] == face) {
			return _grdKakuVstfitaastu[i];
		}
	}
	return 0.0;
}

// 2018/02/28 take Add Start
void AtdDbToCsv::devideItmb( const int& itmb, vector<int>& itmbMemberList )
{
	int currItmb = itmb;
	int targetIndex = 0;
	double min = 10000.0;
	double memberList[14] = { 2, 4, 8, 16, 32, 64, 128, 256, 512,		//構成項目
							  1024, 2048, 4096, 8192, 16384 };

	int nYouso = sizeof(memberList) / sizeof(memberList[0]);
	while(1){
		for( int i = 0; i < nYouso; i++ ){					//構成項目からitmbの値と近い項目番号を取得
			if( currItmb >= memberList[i] ){
				int subtract = currItmb - memberList[i];
				if( subtract < min ){
					min = subtract;
					targetIndex = i;					//インデックス番号を取得
				}
			}else{
				break;
			}
		}
		currItmb -= memberList[targetIndex];
		itmbMemberList.push_back( memberList[targetIndex] );
		if( currItmb == 0 ){			//ITMBの構成要素を全て取得した場合
			break;
		}
	}
}

bool AtdDbToCsv::existConfigurationItem( const int& targetItem, const vector<int>& itmbMemberList )
{
	for( int i = 0; i < itmbMemberList.size(); i++ ){
		if( itmbMemberList[i] == targetItem ){			//構成項目に目的の項目が存在した場合
			return true;
		}
	}
	return false;										//構成項目に目的の項目が存在しない場合
}
// 2018/02/28 take Add End

int AtdDbToCsv::findType(int jointNo, vector<int>& jointNoList, vector<int>& idNameList)
{
	int type = -1;
	for(int i=0;i<(int)jointNoList.size();i++) {
		if(jointNo == jointNoList[i]) {
			type = idNameList[i];
			break;
		}
	}

	return type;
}

double AtdDbToCsv::findDouble(int jointNo, vector<int>& jointNoList, vector<double>& idNameList)
{
	double type = 0.0;
	for(int i=0;i<(int)jointNoList.size();i++) {
		if(jointNo == jointNoList[i]) {
			type = idNameList[i];
			break;
		}
	}

	return type;
}

string AtdDbToCsv::findIdName(int jointNo, vector<int>& jointNoList, vector<string>& idNameList)
{
	string splId = "";
	for(int i=0;i<(int)jointNoList.size();i++) {
		if(jointNo == jointNoList[i]) {
			splId = idNameList[i];
			break;
		}
	}

	return splId;
}

string AtdDbToCsv::makePitchGage(const int& npg, const double& pg)
{
	StringHandler sh;
	string pitchGage = "";
	if(npg == 2) {
		pitchGage = sh.toString(pg);
	} else if(npg > 2) {
		pitchGage = sh.toString(npg-1) + "@" + sh.toString(pg);
	}

	return pitchGage;
}

double AtdDbToCsv::getFillItaatsu(const double& atsuSa)
{
	double itaatsu = atsuSa;

	if(atsuSa > 6.0) {
		for(int d=100;d>=7;d--) {
			if(atsuSa <= (double)(d)) {
				itaatsu = (double)(d);
			}
		}
		return itaatsu;
	}
	if(atsuSa <= 6.0) {
		itaatsu = 6.0;
	}
	if(atsuSa <= 4.5) {
		itaatsu = 4.5;
	}
	if(atsuSa <= 2.3) {
		itaatsu = 2.3;
	}
	if(atsuSa <= 1.6) {
		itaatsu = 1.6;
	}

	return itaatsu;
}

