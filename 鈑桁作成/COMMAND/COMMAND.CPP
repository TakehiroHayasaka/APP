#include "jptmgr.h"
#include <string>
#include <stdio.h>
#ifdef _ICAD
#include "sds.h"
#else
#include	<adslib.h>
#endif

double getValue( char *msg );    //入力値を取得
int    selectDrawingMenthod();   //配置点の設定
int    selectColor();            //色の設定
void   setPoint( double Uflg_width, double Lflg_width, double Web_height, double Gir_length ); //描画
void   makeSolid();             //ソリッドを作成
int    letsRegion( char *arg );  //レージョンを作成
int    letsExtrude();            //押し出し
int    makeHole();               //穴をあける
int    makeHole2( ads_point point, ads_name  ss1 );

ads_point putPoint; //配置点
int       color;    //色番号

/*:****************************************************************************
名前	COMMAND
機能	鈑桁を作成します	
******************************************************************************/
void drawGirder()
{
	double  Uflg_width = 50;	//上フランジ幅
	double  Lflg_width = 50;	//下フランジ幅
	double  Web_height = 200;	//ウェブ高さ
	double  Gir_length = 600;	//桁長
	char	*msg1 = "\n上フランジ幅を入力:";
	char	*msg2 = "下フランジ幅を入力:";
	char	*msg3 = "ウェブ高さを入力:";
	char	*msg4 = "桁長を入力:";
	int     res1 = 0;
	int     res2 = 0;
	
	/*
	Uflg_width = getValue( msg1 ); //上フランジ幅取得
	
	if(Uflg_width != 0)
	Lflg_width = getValue( msg2 ); //下フランジ幅取得
	
	if(Lflg_width != 0)
	Web_height = getValue( msg3 ); //ウェブ高さ取得
	
	if(Web_height != 0)
	Gir_length = getValue( msg4 ); //桁長取得
	*/
	//if(Gir_length != 0)
	//res1 = selectDrawingMenthod(); //配置方法を選択
	
	//if(res1 != 0)
	//res2 = selectColor();          //色を指定
	
	//if(res1 != 0) 
	setPoint( Uflg_width, Lflg_width, Web_height, Gir_length );//描画
	makeSolid(); //ソリッドを作成
	makeHole();  //穴をあける

}

/*:****************************************************************************
名前	getValue
機能	入力値を取得する。
引数　　表示するメッセージ内容
戻り値　入力値:入力値、0:キャンセル終了
備考	
******************************************************************************/
double getValue( char *msg )
{
	double  value = 0;
	int     result = 0;

	while(value <= 0){
		result = _tads_getreal( msg, &value );
		if( result == RTCAN){
			_tads_printf("キャンセルします。\n");
			return 0;
		}else if(value <= 0 ){
			_tads_printf("0より大きい値を入力して下さい。\n");
		}
	}
	return value;
}

/*:****************************************************************************
名前	selectDrawingMenthod
機能	配置点を選択する。
引数　　
戻り値　0:キャンセル終了 1:その他
備考	
******************************************************************************/
int selectDrawingMenthod()
{
	//ads_getstringで使用
	int  conly = 0;
	char *prompt = "配置する方法を指定[原点配置(o)/座標指定(s)]:";
	char result1[10];
	int  result2 = 0;
	std::string str1("o");
	std::string str2("s");
	
	//ads_getpointで使用
	ads_point pt;
	ads_point result;
	char      *msg = "配置する原点となる座標を指定：";
	ads_point origin[] = {0,0,0};

	while(1){
		result2 = _tads_getstring( conly, prompt, result1 );
		if(result1 == str1){
			_tads_printf("原点配置します。\n");
			putPoint[0] = *origin[X];
			putPoint[1] = *origin[Y];
			putPoint[2] = *origin[Z];
			return 1;
			break;
		}else if(result1 == str2){
			_tads_getpoint( pt, msg, result );
			_tads_printf("\nx:%lf, y:%lf, z:%lf\n",result[X],result[Y],result[Z]);
			putPoint[0] = result[X];
			putPoint[1] = result[Y];
			putPoint[2] = result[Z];
			return 1;
			break;
		}else if( result2 == RTCAN ){
			_tads_printf("キャンセルします。\n");
			return 0;
			break;
		}else{
			_tads_printf("オプションのキーワードを入力して下さい。\n");
		}
	}

}

/*:****************************************************************************
名前	selectColor
機能	線分の色を設定する。
引数　　
戻り値　0:キャンセル終了 1:その他
備考	
******************************************************************************/
int selectColor()
{
	int     value = 0;
	int     *result;
	char    *msg = "色番号を指定:";

	while(1){
		value = _tads_getint( msg, result );
		if((*result >= 0) && (*result <= 249)){
			color = *result;
			return 1;
			break;
		}else if( value == RTCAN){
			_tads_printf("キャンセルします。\n");
			return 0;
			break;
		}else if((*result <= 0) || (*result >= 249) ){
			_tads_printf("値は、0～249の範囲で入力して下さい。\n");
		}
	}
}

/*:****************************************************************************
名前	setPoint
機能	線を引く
引数　　表示するメッセージ内容
戻り値　入力値:入力値、0:キャンセル終了
備考	
******************************************************************************/
void setPoint( double Uflg_width, double Lflg_width, double Web_height, double Gir_length )
{
	double x = putPoint[0];    //配置点x
	double y = putPoint[1];    //配置点y
	double z = putPoint[2];    //配置点z
	char   *appname = "Layer"; //アプリケーション名

	_tads_regapp(appname); // アプリケーション名を登録

	ads_point p1  = {              x,                  y - 5, z + Web_height };
	ads_point p2  = {              x, y - ( Uflg_width / 2 ), z + Web_height };
	ads_point p3  = { x + Gir_length, y - ( Uflg_width / 2 ), z + Web_height };
	ads_point p4  = { x + Gir_length,                  y - 5, z + Web_height };
	ads_point p5  = { x + Gir_length, y + ( Uflg_width / 2 ), z + Web_height };
	ads_point p6  = {              x, y + ( Uflg_width / 2 ), z + Web_height };
	ads_point p7  = {              x,                  y - 5,              z };
	ads_point p8  = {              x, y - ( Uflg_width / 2 ),         z - 10 };
	ads_point p9  = { x + Gir_length, y - ( Uflg_width / 2 ),         z - 10 };
	ads_point p10 = { x + Gir_length,                  y - 5,              z };
	ads_point p11 = { x + Gir_length, y + ( Uflg_width / 2 ),         z - 10 };
	ads_point p12 = {              x, y + ( Uflg_width / 2 ),         z - 10 };

	/*コマンドでラインを引く場合*************************************************
	_tads_command(RTSTR, "LINE", RT3DPOINT, p2, RT3DPOINT, p3, RT3DPOINT,  p5, RT3DPOINT, p6,  RT3DPOINT, p2, RTSTR, "" );
	_tads_command(RTSTR, "LINE", RT3DPOINT, p8, RT3DPOINT, p9, RT3DPOINT, p11, RT3DPOINT, p12, RT3DPOINT, p8, RTSTR, "" );
	_tads_command(RTSTR, "LINE", RT3DPOINT, p1, RT3DPOINT, p4, RT3DPOINT, p10, RT3DPOINT, p7,  RT3DPOINT, p1, RTSTR, "" );
	*/

	struct resbuf *LINE1  = NULL;
	struct resbuf *LINE2  = NULL;
	struct resbuf *LINE3  = NULL;
	struct resbuf *LINE4  = NULL;
	struct resbuf *LINE5  = NULL;
	struct resbuf *LINE6  = NULL;
	struct resbuf *LINE7  = NULL;
	struct resbuf *LINE8  = NULL;
	struct resbuf *LINE9  = NULL;
	struct resbuf *LINE10 = NULL;
	struct resbuf *LINE11 = NULL;
	struct resbuf *LINE12 = NULL;

	LINE1  = _tads_buildlist( RTDXF0, "LINE", 8, "上フランジ", 62, color, 10,  p2, 11,  p3, -3, 1001, appname, 1000, "上フランジ", 0 );
	LINE2  = _tads_buildlist( RTDXF0, "LINE", 8, "上フランジ", 62, color, 10,  p3, 11,  p5, -3, 1001, appname, 1000, "上フランジ", 0 );
	LINE3  = _tads_buildlist( RTDXF0, "LINE", 8, "上フランジ", 62, color, 10,  p5, 11,  p6, -3, 1001, appname, 1000, "上フランジ", 0 );
	LINE4  = _tads_buildlist( RTDXF0, "LINE", 8, "上フランジ", 62, color, 10,  p6, 11,  p2, -3, 1001, appname, 1000, "上フランジ", 0 );
	LINE5  = _tads_buildlist( RTDXF0, "LINE", 8, "下フランジ", 62, color, 10,  p8, 11,  p9, -3, 1001, appname, 1000, "下フランジ", 0 );
	LINE6  = _tads_buildlist( RTDXF0, "LINE", 8, "下フランジ", 62, color, 10,  p9, 11, p11, -3, 1001, appname, 1000, "下フランジ", 0 );
	LINE7  = _tads_buildlist( RTDXF0, "LINE", 8, "下フランジ", 62, color, 10, p11, 11, p12, -3, 1001, appname, 1000, "下フランジ", 0 );
	LINE8  = _tads_buildlist( RTDXF0, "LINE", 8, "下フランジ", 62, color, 10, p12, 11,  p8, -3, 1001, appname, 1000, "下フランジ", 0 );
	LINE9  = _tads_buildlist( RTDXF0, "LINE", 8,     "ウェブ", 62, color, 10,  p1, 11,  p4, -3, 1001, appname, 1000,     "ウェブ", 0 );
	LINE10 = _tads_buildlist( RTDXF0, "LINE", 8,     "ウェブ", 62, color, 10,  p4, 11, p10, -3, 1001, appname, 1000,     "ウェブ", 0 );
	LINE11 = _tads_buildlist( RTDXF0, "LINE", 8,     "ウェブ", 62, color, 10, p10, 11,  p7, -3, 1001, appname, 1000,     "ウェブ", 0 );
	LINE12 = _tads_buildlist( RTDXF0, "LINE", 8,     "ウェブ", 62, color, 10,  p7, 11,  p1, -3, 1001, appname, 1000,     "ウェブ", 0 );

	ads_entmake (LINE1); //作図
	ads_entmake (LINE2); //作図
	ads_entmake (LINE3); //作図
	ads_entmake (LINE4); //作図
	ads_entmake (LINE5); //作図
	ads_entmake (LINE6); //作図
	ads_entmake (LINE7); //作図
	ads_entmake (LINE8); //作図
	ads_entmake (LINE9); //作図
	ads_entmake (LINE10); //作図
	ads_entmake (LINE11); //作図
	ads_entmake (LINE12); //作図

	ads_relrb(LINE1);   //メモリ解放
	ads_relrb(LINE2);   //メモリ解放
	ads_relrb(LINE3);   //メモリ解放
	ads_relrb(LINE4);   //メモリ解放
	ads_relrb(LINE5);   //メモリ解放
	ads_relrb(LINE6);   //メモリ解放
	ads_relrb(LINE7);   //メモリ解放
	ads_relrb(LINE8);   //メモリ解放
	ads_relrb(LINE9);   //メモリ解放
	ads_relrb(LINE10);   //メモリ解放
	ads_relrb(LINE11);   //メモリ解放
	ads_relrb(LINE12);   //メモリ解放
	
}

/*:****************************************************************************
名前	makeSolid
機能	ソリッドを作成する。
引数　　なし
戻り値　
備考	
******************************************************************************/
void makeSolid()
{
	char *msg1 = "上フランジ";
	char *msg2 = "下フランジ";
	char *msg3 = "ウェブ";

	letsRegion( msg1 );
	letsExtrude();
	letsRegion( msg2 );
	letsExtrude();
	letsRegion( msg3 );
	letsExtrude();
}

/*:****************************************************************************
名前	letsRegion
機能	リージョンを作成する。
引数　　レイヤ名
戻り値　
備考	
******************************************************************************/
int letsRegion( char *arg)
{
	ads_name ss;
	char *str = "X";
	struct resbuf *filter = NULL;
	char *msg = arg;

	filter = _tads_buildlist( 8, msg, 0 );

	_tads_ssget( str, NULL, NULL, filter, ss);
	_tads_command(RTSTR, "REGION", RTPICKS, ss, RTSTR, "", 0 );
	ads_relrb(filter);
	ads_ssfree(ss);
	return 0;

}

/*:****************************************************************************
名前	letsExtrude
機能	厚みを追加する。
引数　　なし
戻り値　0
備考	
******************************************************************************/
int letsExtrude()
{
	ads_name ss;
	ads_real value = 10;
	char *str = "L";

	_tads_ssget( str, NULL, NULL, NULL, ss);
	_tads_command(RTSTR, "EXTRUDE", RTPICKS, ss, RTSTR, "", RTREAL, value, RTSTR, "Y", 0 );
	ads_ssfree(ss);
	return 0;

}

/*:****************************************************************************
名前	makeHole
機能	穴をあける。
引数　　なし
戻り値　0
備考	
******************************************************************************/
int makeHole(){
	ads_point p1 = { 0, 0, 0 };
	ads_point p2 = { 600, 0, 0 };
	ads_point p3 = { 0, 0, 200 };
	ads_point p =  { 0, 20, 0 };
	ads_name  ss1;	//ウェブのエンティティ情報
	char      *str = "L";   //最後に作成されたエンティティ

	ads_point p4  = { 10, 60, -10 };
	ads_point p5  = { 10, 70, -10 };
	ads_point p6  = { 10, 80, -10 };
	ads_point p7  = { 10, 90, -10 };
	ads_point p8  = { 20, 60, -10 };
	ads_point p9  = { 20, 70, -10 };
	ads_point p10 = { 20, 80, -10 };
	ads_point p11 = { 20, 90, -10 };
	ads_point p12 = { 10, 110, -10 };
	ads_point p13 = { 10, 120, -10 };
	ads_point p14 = { 10, 130, -10 };
	ads_point p15 = { 10, 140, -10 };
	ads_point p16 = { 20, 110, -10 };
	ads_point p17 = { 20, 120, -10 };
	ads_point p18 = { 20, 130, -10 };
	ads_point p19 = { 20, 140, -10 };
	ads_point p20 = { 10, 100, -10 };
	ads_point p21 = { 20, 100, -10 };

	ads_point p2_4  = { 590, 60, -10 };
	ads_point p2_5  = { 590, 70, -10 };
	ads_point p2_6  = { 590, 80, -10 };
	ads_point p2_7  = { 590, 90, -10 };
	ads_point p2_8  = { 580, 60, -10 };
	ads_point p2_9  = { 580, 70, -10 };
	ads_point p2_10 = { 580, 80, -10 };
	ads_point p2_11 = { 580, 90, -10 };
	ads_point p2_12 = { 590, 110, -10 };
	ads_point p2_13 = { 590, 120, -10 };
	ads_point p2_14 = { 590, 130, -10 };
	ads_point p2_15 = { 590, 140, -10 };
	ads_point p2_16 = { 580, 110, -10 };
	ads_point p2_17 = { 580, 120, -10 };
	ads_point p2_18 = { 580, 130, -10 };
	ads_point p2_19 = { 580, 140, -10 };
	ads_point p2_20 = { 590, 100, -10 };
	ads_point p2_21 = { 580, 100, -10 };

	_tads_command(RTSTR, "", RTSTR, "UCS", RT3DPOINT, p1, RT3DPOINT, p2, RT3DPOINT, p3, NULL );//座標系変換
	_tads_ssget( str, NULL, NULL, NULL, ss1);  //ウェブのエンティティを記録

	makeHole2( p4, ss1 );
	makeHole2( p5, ss1 );
	makeHole2( p6, ss1 );
	makeHole2( p7, ss1 );
	makeHole2( p8, ss1 );
	makeHole2( p9, ss1 );
	makeHole2( p10, ss1 );
	makeHole2( p11, ss1 );
	makeHole2( p12, ss1 );
	makeHole2( p13, ss1 );
	makeHole2( p14, ss1 );
	makeHole2( p15, ss1 );
	makeHole2( p16, ss1 );
	makeHole2( p17, ss1 );
	makeHole2( p18, ss1 );
	makeHole2( p19, ss1 );
	makeHole2( p20, ss1 );
	makeHole2( p21, ss1 );

	makeHole2( p2_4, ss1 );
	makeHole2( p2_5, ss1 );
	makeHole2( p2_6, ss1 );
	makeHole2( p2_7, ss1 );
	makeHole2( p2_8, ss1 );
	makeHole2( p2_9, ss1 );
	makeHole2( p2_10, ss1 );
	makeHole2( p2_11, ss1 );
	makeHole2( p2_12, ss1 );
	makeHole2( p2_13, ss1 );
	makeHole2( p2_14, ss1 );
	makeHole2( p2_15, ss1 );
	makeHole2( p2_16, ss1 );
	makeHole2( p2_17, ss1 );
	makeHole2( p2_18, ss1 );
	makeHole2( p2_19, ss1 );
	makeHole2( p2_20, ss1 );
	makeHole2( p2_21, ss1 );

	_tads_command(RTSTR, "", RTSTR, "UCS", RT3DPOINT, p1, RT3DPOINT, p2, RT3DPOINT, p, NULL );//座標系変換
	_tads_command(RTSTR, "", RTSTR, "SHADE", NULL );//SHADE
	ads_ssfree(ss1);
	return 0;
}

int makeHole2( ads_point point, ads_name  ss1 ){
	ads_real  value = 2.5;	//円の半径
	ads_real  value2 = -20; //EXTRUDEの幅
	ads_name  ss2;	//描画した円のエンティティ情報
	char      *str = "L";   //最後に作成されたエンティティ

	_tads_command(RTSTR, "CIRCLE", RT3DPOINT, point, RTREAL, value, NULL ); //円を描画

	_tads_ssget( str, NULL, NULL, NULL, ss2);  //描画した円のエンティティを記録

	_tads_command(RTSTR, "REGION", RTPICKS, ss2, RTSTR, "", NULL );
	_tads_ssget( str, NULL, NULL, NULL, ss2);
	_tads_command(RTSTR, "EXTRUDE", RTPICKS, ss2, RTSTR, "", RTREAL, value2, RTSTR, "Y", NULL );
	_tads_ssget( str, NULL, NULL, NULL, ss2);
	_tads_command(RTSTR, "SUBTRACT", RTPICKS, ss1, RTSTR, "", RTPICKS, ss2, RTSTR, "", NULL );
	ads_ssfree(ss2);
	return 0;

}