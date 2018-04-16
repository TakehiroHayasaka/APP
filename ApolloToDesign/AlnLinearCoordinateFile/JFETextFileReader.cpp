#pragma warning (disable:4786)

#include "stdafx.h"
#include <fstream>
#include "JFETextFileReader.h"


JptErrorStatus JFETextFileReader::load(std::ifstream &ifs, AlnLinearCoordinateFile *lcfPtr)
{
	if ( this->readFileSet(ifs, lcfPtr) != JPT_OK ) {
		errorPush("***ERROR*** 線形座標値ファイル["+ this->_fileName +"]の読み込みに失敗しました。\n");
		ifs.close();
		return JPT_ERROR;
	}

	ifs.close();
	return JPT_OK;
}



JptErrorStatus JFETextFileReader::readFileSet (std::ifstream& ifs, AlnLinearCoordinateFile *lcfPtr)
{
	int lineNumber = 0;
	while( !ifs.eof() ) {
		std::string line;
		getline(ifs, line);		// reads line name
		lineNumber++;

		AlnLinearCoordinateData lcd;
		lcd.setLineName( line.substr(0, line.find_first_of(" ")) );
		if ( this->setLinearCoordinateData(lineNumber, ifs, lcd ) != JPT_OK ) {

			std::stringstream ss;
			ss << "***ERROR*** " << lineNumber << " 行目で読み込みに失敗しました。\n";
			errorPush( ss.str() ); 

			return JPT_ERROR;
		}
		lcfPtr->append(lcd );
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

JptErrorStatus JFETextFileReader::setLinearCoordinateData(int& lineNumber, std::ifstream& ifs, AlnLinearCoordinateData& lcd ) 
{
	std::string line;
	getline (ifs, line);		//  reads header line
	lineNumber++;

	for (;;) {
		long location = ifs.tellg();
		getline(ifs, line);
		lineNumber++;

		// 2017/03/17 Nakagawa Add Start
		if ( line.empty() ) {
			ifs.seekg( location );
			break;
		}
		// 2017/03/17 Nakagawa Add End

		StringTokenizer pointDataStrkr ( line, "," );		
		if ( pointDataStrkr.sizeStr() == 0 ) {
			ifs.seekg( location );
			break;
		}
		if ( pointDataStrkr.sizeStr() == 10 && pointDataStrkr[1] == "" ) {
			ifs.seekg( location );
			break;
		}
		if ( pointDataStrkr.sizeStr() == 10  && pointDataStrkr[2] == "" ) {
			continue;
		}
		AlnPointData pointData;
		if ( setPointData(pointDataStrkr, pointData) != JPT_OK ) {
			return JPT_ERROR;
		}

		AlnPointData samePoint;
		if ( lcd.find( pointData.getPointName(), samePoint) == JPT_OK ) {
			if ( (samePoint.getX() == pointData.getX()) && ( samePoint.getY() == pointData.getY()) &&
				(samePoint.getHeight() == pointData.getHeight()) ) {
				continue;
			} else {
				return JPT_ERROR;
			}
		}
		lcd.append( pointData );
	}
	return JPT_OK;
}

JptErrorStatus JFETextFileReader::setPointData(StringTokenizer& pointDataStrkr, AlnPointData& pointData)
{
	if  ( pointDataStrkr.sizeStr() != 10 ) {
		return JPT_ERROR;
	}

	pointData.setPointName( pointDataStrkr[1] );
	pointData.setX( atof(pointDataStrkr[2].c_str() ) * 1000.0 );
	pointData.setY( atof(pointDataStrkr[3].c_str() ) * 1000.0 );
	pointData.setHeight( atof( pointDataStrkr[4].c_str() ) * 1000.0 );

	if ( this->setAlnLength( pointDataStrkr, pointData) != JPT_OK ) {
		return JPT_ERROR;
	}
	if ( pointData.setAngle( pointDataStrkr[7] ) != JPT_OK ) {
		return JPT_ERROR;
	}
	pointData.setStation( pointDataStrkr[8] );

	if ( pointDataStrkr[9].find("*") != string::npos ) {
		pointData.setCAngle(0.0);
	} else {
		if ( pointData.setCAngle( pointDataStrkr[9] ) != JPT_OK ) {
			return JPT_ERROR;
		}
	}

	return JPT_OK;
}

JptErrorStatus JFETextFileReader::setAlnLength( StringTokenizer& pointDataStkr, AlnPointData& pointData)
{
	if ( pointDataStkr.sizeStr() != 10 ) {
		return JPT_ERROR;
	}
	AlnMensuration mensurationLength;
	mensurationLength.setSingle( atof( pointDataStkr[5].c_str()) * 1000.0 );
	mensurationLength.setCumulative( atof(pointDataStkr[6].c_str()) * 1000.0 );

	pointData.setLength(mensurationLength);
	return JPT_OK;
}

