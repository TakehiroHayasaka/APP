#pragma warning (disable:4786)

#include "stdafx.h"
#include "CrcCosumoTextReader.h"



JptErrorStatus CrcCosumoTextReader::load(std::ifstream &ifs, AlnLinearCoordinateFile *lcfPtr) 
{
	if ( readFileSet ( ifs, lcfPtr) != JPT_OK ) {
		errorPush("***ERROR*** 線形座標値ファイル["+this->_fileName+"]の読み込みに失敗しました。\n");
		ifs.close();
		return JPT_ERROR;
	}
	ifs.close();
	return JPT_OK;
}



JptErrorStatus CrcCosumoTextReader::readFileSet(std::ifstream& ifs, AlnLinearCoordinateFile *lcfPtr)
{
	int lineNumber = 0;
	std::string line;
	while ( !ifs.eof() ) {
		
		std::getline(ifs, line);		//	reads "SECTION....." line
		lineNumber++;

		if ( line.find("SECTION") == std::string::npos ) {
			continue;
		}
		std::string pointName;
		if ( extractPointName(line, pointName) != JPT_OK ) {

			std::stringstream ss;
			ss << "***ERROR*** " << lineNumber << " 行目で読み込みに失敗しました。\n";
			errorPush( ss.str() ); 

			return JPT_ERROR;
		}
		std::getline(ifs, line);	//	reads header "LINE	X	Y....." line 
		lineNumber++;
		if ( setAlnLinearCoordinateData(lineNumber, pointName, ifs, lcfPtr) != JPT_OK ) {

			std::stringstream ss;
			ss << "***ERROR*** " << lineNumber <<" 行目で読み込みに失敗しました。\n";
			errorPush( ss.str() ); 

			return JPT_ERROR;
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

JptErrorStatus CrcCosumoTextReader::extractPointName ( const std::string& line, std::string& pointName)
{
	if ( line.empty() ) {
		return JPT_ERROR;
	}
	int startPoint = line.find_first_not_of(" ", line.find_first_of(" ", 0));
	int endPoint = line.find_first_of(" ", startPoint);
	pointName = line.substr( startPoint, endPoint - startPoint );
	return JPT_OK;
}

JptErrorStatus CrcCosumoTextReader::setAlnLinearCoordinateData(int &lineNumber,
															   const std::string& pointName,
															   std::ifstream& ifs,
															   AlnLinearCoordinateFile *lcfPtr)
{
	 
	for ( ; ; ) {
		std::string line;
		long gLocation = ifs.tellg();
		std::getline(ifs, line);
		lineNumber++;
		if ( line.empty() ) {
			break;
		}
		StringTokenizer lineStrTok ( line, "," );
		if ( lineStrTok[0] == "" ) {
			continue;
		}		
		if ( lineStrTok[0].find("SECTION") != std::string::npos ) {
			ifs.seekg(gLocation);
			break;
		}
		int index = lcfPtr->index(lineStrTok[0]);
		AlnLinearCoordinateData lcd;
		if ( index != -1 ) {
			if ( lcfPtr->getAt(index, lcd) != JPT_OK ) {
				return JPT_ERROR;
			}
			if ( this->setAlnPointData(pointName, lineStrTok, lcd) != JPT_OK ) {
				return JPT_ERROR;
			}
			if ( lcfPtr->setAt(index, lcd) != JPT_OK ) {
				return JPT_ERROR;
			}
		} else {
			lcd.setLineName( lineStrTok[0] );
			if ( this->setAlnPointData(pointName, lineStrTok, lcd) != JPT_OK ) {
				return JPT_ERROR;
			}
			lcfPtr->append(lcd);
		}
	}
	return JPT_OK;
}

JptErrorStatus CrcCosumoTextReader::setAlnPointData(const std::string& pointName, StringTokenizer& lineStrTok,
													AlnLinearCoordinateData& lcd)
{
	if ( lineStrTok.sizeStr() == 11 && lineStrTok[1] == "" ) {
		return JPT_OK;
	}
	if ( lineStrTok.sizeStr() < 11 ) {
		return JPT_OK;
	}
	AlnPointData point;
	if ( this->setAlnLengthMensuration(lineStrTok, point) != JPT_OK ) {
		return JPT_ERROR;
	}
	if ( this->setAlnDistanceMensuration(lineStrTok, point) != JPT_OK ) {
		return JPT_ERROR;
	}
	if ( point.setAngle(lineStrTok[9]) != JPT_OK ) {
		return JPT_ERROR;
	}
	point.setPointName( pointName );
	point.setX( atof(lineStrTok[1].c_str()) * 1000.0);
	point.setY( atof(lineStrTok[2].c_str()) * 1000.0);
	point.setHeight( atof(lineStrTok[3].c_str()) * 1000.0);
	point.setGrade( atof(lineStrTok[4].c_str()) );
	point.setStation( lineStrTok[10] );

	AlnPointData samePoint;
	if ( lcd.find(pointName, samePoint) == JPT_OK ) {
		if ( point == samePoint ) {
			return JPT_OK;
		} else {
			return JPT_ERROR;
		}
	}
	lcd.append(point);
	return JPT_OK;
}

JptErrorStatus CrcCosumoTextReader::setAlnLengthMensuration(StringTokenizer& lineStrTok, AlnPointData& point)
{
	if ( lineStrTok.sizeStr() != 11 ) {
		return JPT_ERROR;
	}
	AlnMensuration length;
	length.setSingle( atof(lineStrTok[5].c_str()) * 1000.0 );
	length.setCumulative( atof( lineStrTok[6].c_str()) * 1000.0 );
	point.setLength( length );
	return JPT_OK;
}

JptErrorStatus CrcCosumoTextReader::setAlnDistanceMensuration(StringTokenizer& lineStrTok, AlnPointData& point)
{
	if ( lineStrTok.sizeStr() != 11 ) {
		return JPT_ERROR;
	}
	AlnMensuration distance;
	distance.setSingle( atof(lineStrTok[7].c_str()) * 1000.0 );
	distance.setCumulative( atof( lineStrTok[8].c_str()) * 1000.0 );
	point.setDistance( distance);
	return JPT_OK;
}
