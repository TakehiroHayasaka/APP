#pragma warning (disable:4786)

#include "stdafx.h"
#include "JIPTextReader.h"

JptErrorStatus JIPTextReader::load(std::ifstream &ifs, AlnLinearCoordinateFile *lcfPtr)
{
	if ( jipReadFileSet(ifs, lcfPtr) != JPT_OK ) {
		errorPush("***ERROR*** 線形座標値ファイル["+ this->_fileName +"]の読み込みに失敗しました。\n");
		ifs.close();
		return JPT_ERROR;
	}
	ifs.close();
	return JPT_OK;
}


JptErrorStatus JIPTextReader::jipReadFileSet(std::ifstream& ifs, AlnLinearCoordinateFile *lcfPtr)
{
	int lineNumber = 0;
	std::string str;
	getline (ifs, str);	// reads 径間名　
	lineNumber++;

	while ( !str.empty() ) {

		getline (ifs, str);					// reads 縦断面
		lineNumber++;

		StringTokenizer ptLineTok(str, ",");
		std::string oudan = ptLineTok[2];		

		getline (ifs, str);					// reads ヘディング　コメント
		lineNumber++;

		for (;;) {
			getline(ifs, str);
			lineNumber++;

			if ( str.empty() ) { 
				break;	
			}
			StringTokenizer strTok( str, "," );
			if ( strTok[0] != "" ) {
				break;
			}
			if ( jipReadSetLinearCoordinateData( oudan, strTok, lcfPtr) != JPT_OK ) {
				std::stringstream ss;
				ss << "***ERROR*** " << lineNumber << " 行目で読み込みに失敗しました。\n";
				errorPush( ss.str() ); 

				return JPT_ERROR;
			}	
		}
	}

    if (lcfPtr->getSortingWay() == ALN_SORT_STATION) {
        if (lcfPtr->sortPointDataByStation() != JPT_OK) {
            errorPush("***ERROR*** 測点による点データの並び替えに失敗しました。\n");
            return JPT_ERROR;
        }
    } else if (lcfPtr->getSortingWay() == ALN_SORT_CUMULATIVE_LENGTH) {
        if (lcfPtr->sortPointDataByCumulativeLength() != JPT_OK) {
            errorPush("***ERROR*** 追加距離による点データの並び替えに失敗しました。\n");
            return JPT_ERROR;
        }
    } else {
        errorPush("***ERROR*** 点データの並び替えのタイプ指定が誤りです。\n");
        return JPT_ERROR;
    }

	return JPT_OK;
}


JptErrorStatus JIPTextReader::jipReadSetLinearCoordinateData(const std::string& oudan, StringTokenizer& strTok, 
													  AlnLinearCoordinateFile  *lcfPtr)
{
	std::string juudan = strTok[1];
	AlnLinearCoordinateData linearCoordinateData;
	//checks if juudansen already exist
	JptErrorStatus findStatus = lcfPtr->find(juudan, linearCoordinateData);
	if ( findStatus != JPT_OK ) {
		linearCoordinateData.setLineName(juudan);
	}
	//checks if point already exist in vector
	if ( linearCoordinateData.isAlreadyPoint(oudan) ) {
		return JPT_ERROR;
	}
	//reads and sets point data
	if (jipReadSetPointData (oudan, strTok, linearCoordinateData) != JPT_OK ) {
		return JPT_ERROR;
	}
	//store pointData in linearCoordinateData
	if ( findStatus == JPT_OK ) {
		if ( lcfPtr->setAt( lcfPtr->index(juudan), linearCoordinateData)  != JPT_OK ) {
			return JPT_ERROR;
		}		
	} else {
		lcfPtr->append(linearCoordinateData);
	}
	return JPT_OK;
}



JptErrorStatus JIPTextReader::jipReadSetPointData(const std::string& oudan, StringTokenizer& strTok, 
										   AlnLinearCoordinateData& linearCoordinateData)
{
	if ( oudan.empty() ) {
		return JPT_ERROR;
	}
	if ( strTok[2] == "" ) {
		return JPT_OK;
	}
	AlnPointData pointData;
	pointData.setPointName(oudan);

	pointData.setX(atof(strTok[2].c_str()) * 1000.0 );
	pointData.setY(atof(strTok[3].c_str()) * 1000.0 );
	pointData.setHeight(atof(strTok[4].c_str()) * 1000.0 );
	pointData.setGrade( atof(strTok[5].c_str()) );

	jipReadSetMensurationDistance(strTok, pointData);
	jipReadSetMensurationWidth(strTok, pointData);
	pointData.setStation(strTok[11]);

	if ( pointData.setAngle(strTok[10]) != JPT_OK ) {
		return JPT_ERROR;
	}
	linearCoordinateData.append( pointData );
	return JPT_OK;
}

void JIPTextReader::jipReadSetMensurationDistance(StringTokenizer& strTok, AlnPointData& pointData)
{
	AlnMensuration distance;
	if ( strTok[8].find("*") != string::npos ) {
		distance.setSingle(0.0);
	} else {
		distance.setSingle( atof(strTok[8].c_str()) * 1000.0 );
	}
	if ( strTok[9].find("*") != string::npos ) {
		distance.setCumulative(0.0);
	} else {
		distance.setCumulative( atof( strTok[9].c_str()) * 1000.0 );
	}
	pointData.setDistance( distance );
}

void JIPTextReader::jipReadSetMensurationWidth(StringTokenizer& strTok, AlnPointData& pointData)
{
	AlnMensuration width;
	if ( strTok[6].find("*") != string::npos) {
		width.setSingle(0.0);
	} else {
		width.setSingle( atof(strTok[6].c_str()) * 1000.0 );
	}
	if ( strTok[7].find("*") != string::npos) {
		width.setCumulative(0.0);
	} else {
		width.setCumulative( atof(strTok[7].c_str()) * 1000.0 );
	}
	pointData.setLength(width);
}
