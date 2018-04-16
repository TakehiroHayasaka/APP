#pragma warning (disable:4786)

#include "stdafx.h"
#include "AlnBinaryFileReader.h"

#include <sstream>
#include <string>
#include <iomanip>
using namespace std;

JptErrorStatus AlnBinaryFileReader::load(const std::string& name, AlnLinearCoordinateFile *lcfPtr)
{
	if ( name.empty()) {
		return JPT_ERROR;
	}
	ifstream ifs;
	if ( readFileOpen(name, ifs) != JPT_OK ) {
		errorPush("***ERROR*** 線形座標値ファイル["+ name + "]を開くことができませんでした。\n");
		return JPT_ERROR;
	}
	if ( readFileSet(ifs, lcfPtr) != JPT_OK ) {
		errorPush("***ERROR*** 線形座標値ファイル[" + name + "]の読み込みに失敗しました。\n");
		return JPT_ERROR;
	}
	return JPT_OK;
}

JptErrorStatus AlnBinaryFileReader::readFileOpen(const std::string& name, std::ifstream& ifs)
{
	// 2017/04/06 Nakagawa Add Start  VS2005だと日本語のファイル名が読み込めない。
	locale default_locale;
	locale::global(locale("japanese"));
	// 2017/04/06 Nakagawa Add End
	ifs.open(name.c_str(), ios::in | ios::binary);
	// 2017/04/06 Nakagawa Add Start
	locale::global(locale(default_locale));
	// 2017/04/06 Nakagawa Add End
	if ( !ifs ) {
		return JPT_ERROR;
	}
	return JPT_OK;
}

JptErrorStatus AlnBinaryFileReader::readFileSet( std::ifstream& ifs, AlnLinearCoordinateFile *lcfPtr)
{
	while( !ifs.eof() ) {
		AlnPointData point;
		if ( this->setPointData(ifs, point) != JPT_OK ) {
            errorPush("***ERROR*** 点データの読み込みに失敗しました。\n");
			return JPT_ERROR;
		}
		if ( setLinearCoordinateData(ifs,  point, lcfPtr) != JPT_OK ) {
            errorPush("***ERROR*** 線データに点データを追加するのに失敗しました。\n");
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
        return JPT_ERROR;
    }

    ifs.close();
	return JPT_OK;	
}

JptErrorStatus AlnBinaryFileReader::setLinearCoordinateData(std::ifstream &ifs, const AlnPointData& point, 
														 AlnLinearCoordinateFile *lcfPtr)
{
    if (ifs.eof()) {
        return JPT_OK;
    }

	char array [12];
	ifs.read( array, sizeof(char)*12);
	std::string lineName (array);

	lineName = lineName.substr( lineName.find_first_not_of(" "), 
		lineName.find_last_not_of(" ") - lineName.find_first_not_of(" ")+1) ;

	if ( lcfPtr->isLineInFile( lineName ) ) {
		AlnLinearCoordinateData linearCoordinateData;
		if ( lcfPtr->find(lineName, linearCoordinateData ) != JPT_OK) {
			return JPT_ERROR;
		}
		linearCoordinateData.append(point);
		int index = lcfPtr->index(lineName);
		if ( lcfPtr->setAt(index, linearCoordinateData) != JPT_OK ) {
			return JPT_ERROR;
		}
	} else {
		AlnLinearCoordinateData linearCoordinateData;
		linearCoordinateData.setLineName( lineName );
		linearCoordinateData.append( point );
		lcfPtr->append(linearCoordinateData);
	}
	return JPT_OK;	
}

JptErrorStatus AlnBinaryFileReader::setPointData( std::ifstream& ifs, AlnPointData& point)
{
	double x;

	ifs.read( (char*)(&x), sizeof (double) );
    if (ifs.eof()) {
        return JPT_OK;
    }
	point.setX(x * 1000.0);

	ifs.read( (char*)(&x), sizeof (double) );
	point.setY(x * 1000.0);

	ifs.read( (char*)(&x), sizeof (double) );
	point.setHeight(x * 1000.0);

	ifs.read( (char*)(&x), sizeof (double) );
	point.setGrade(x);

	if ( setLength(ifs, point) != JPT_OK) {
		return JPT_ERROR;
	}
	if ( setDistance(ifs, point) != JPT_OK ) {
		return JPT_ERROR;
	}
	ifs.read( (char*)(&x), sizeof (double) );
	point.setAngle(x);
	ifs.read( (char*)(&x), sizeof (double) );
	point.setStation(this->convertDoubleToString(x));
	ifs.read( (char*)(&x), sizeof (double) );
	point.setCAngle(x);
	this->readDummy(ifs);
	
 	char array [12];
	ifs.read( array, sizeof(char)*12 );
	std::string pointName (array);
	
	point.setPointName( pointName.substr( pointName.find_first_not_of(" "), 
		pointName.find_last_not_of(" ") - pointName.find_first_not_of(" ")+1) ) ;

	return JPT_OK;
}

JptErrorStatus AlnBinaryFileReader::setLength(std::ifstream& ifs, AlnPointData& point)
{
	double x;
	AlnMensuration length;

	ifs.read( (char*)(&x), sizeof (double) );
	length.setSingle(x * 1000.0);

	ifs.read( (char*)(&x), sizeof(double) );
	length.setCumulative(x * 1000.0);

	point.setLength(length);

	return JPT_OK;
}

JptErrorStatus AlnBinaryFileReader::setDistance( std::ifstream& ifs, AlnPointData& point)
{

	double x;
	AlnMensuration distance;

	ifs.read( (char*)(&x), sizeof(double) );
	distance.setSingle(x * 1000.0);

	ifs.read( (char*)(&x), sizeof(double) );
	distance.setCumulative(x * 1000.0);

	point.setDistance(distance);

	return JPT_OK;
}

void AlnBinaryFileReader::readDummy(std::ifstream& ifs)
{
	double x;
	ifs.read( (char*)(&x), sizeof(double) );
	ifs.read( (char*)(&x), sizeof(double) );
}

const std::string AlnBinaryFileReader::convertDoubleToString(const double number)
{
	ostringstream str;
	str << std::fixed << setprecision(6) << number;
	return str.str();
}

