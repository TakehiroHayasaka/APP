#pragma warning (disable:4786)

#include "stdafx.h"
#include <fstream>
using namespace std;

#include "AlnTextFileReader.h"
#include "JIPTextReader.h"


JptErrorStatus AlnTextFileReader::load(std::ifstream &ifs, AlnLinearCoordinateFile *lcfPtr)
{
	if  ( readFileSet ( ifs, lcfPtr) != JPT_OK ) {
		ifs.close();
		errorPush("***ERROR*** 線形座標値ファイル["+ this->_fileName + "]の読み込みに失敗しました。\n");
		return JPT_ERROR;
	}
	return JPT_OK;
}

/*******************************************************************************************
	private function
********************************************************************************************/

/**
 *	sets data 
 *
 *	@param	ifs		(input)
 *	@param	*lcfPtr	(input/output)
 *	@return		JptErrorStatus	JPT_OK		normal termination
 *								JPT_ERROR	error
 */
JptErrorStatus AlnTextFileReader::readFileSet(std::ifstream& ifs, AlnLinearCoordinateFile *lcfPtr)
{

	std::string stringLine;
	getline ( ifs, stringLine );


	StringTokenizer firstLine ( stringLine, ",");
	int lineNumber = 0;
	if ( this->readSetLinearCoordinateFile(lineNumber, ifs, lcfPtr) != JPT_OK ) {
		std::stringstream ss;
		ss << "***ERROR*** " << lineNumber<< " 行目で読み込みに失敗しました。\n";
		errorPush( ss.str() );
		return JPT_ERROR;
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

JptErrorStatus AlnTextFileReader::readSetLinearCoordinateFile( int &lineNumber,
															  std::ifstream& ifs, 
															  AlnLinearCoordinateFile *lcfPtr)
{
	std::string stringLine;
	for ( ;; ) {
		getline ( ifs, stringLine);
		lineNumber++;
		if ( stringLine.empty() ) {
			break;
		}
		StringTokenizer str ( stringLine, "," );
		if ( str.sizeStr() < 21 ) {
			continue;
		}
		// 2014/03/08 Nakagawa Edit Start Z3対応
//		if ( str.sizeStr() == 21 && str[2] == "" ) {
		if ( str.sizeStr() >= 21 && str[2] == "" ) {
		// 2014/03/08 Nakagawa Edit End
			continue;
		}
		if ( lcfPtr->isLineInFile(str[0]) ) {
			//get LinearCoordinateData
			AlnLinearCoordinateData linearCoordinateData;
			if ( lcfPtr->find(str[0], linearCoordinateData ) != JPT_OK ) {
				return JPT_ERROR;
			}
			//adds point data
			if ( this->setLinearCoordinateData (str, linearCoordinateData) != JPT_OK ) {
				return JPT_ERROR;
			}
			//sets back linearCoordinateData
			int index = lcfPtr->index(str[0]);
			if ( lcfPtr->setAt( index , linearCoordinateData) != JPT_OK  ) {
				return JPT_ERROR;
			}
		} else {
			AlnLinearCoordinateData linearCoordinateData;
			linearCoordinateData.setLineName(str[0]);
			if ( this->setLinearCoordinateData ( str, linearCoordinateData ) != JPT_OK ) {
				return JPT_ERROR;
			}
			lcfPtr->append( linearCoordinateData);
		}
	}
	return JPT_OK;
}

/**
 *	sets LinearCoordinateData from String Tokenizer
 *
 *	@param	str		(input)
 *	@param	linearCoordinateData	(output)
 *	@return		JptErrorStatus	JPT_OK		normal termination
 *								JPT_ERROR	error
 */
JptErrorStatus AlnTextFileReader::setLinearCoordinateData( StringTokenizer& str, 
        AlnLinearCoordinateData& linearCoordinateData)
{
	AlnPointData point;
	if ( this->setPointData (str, point) != JPT_OK ) {
		return JPT_ERROR;
	}
	linearCoordinateData.append(point);
	return JPT_OK;
}

/**
 *	sets PointData	from String Tokenizer
 *
 *	@param	str		(input)
 *	@param	point	(output)
 *	@return		JptErrorStatus	JPT_OK		normal termination
 *								JPT_ERROR	error
 */
JptErrorStatus AlnTextFileReader::setPointData( StringTokenizer& str, AlnPointData& point) 
{
	point.setPointName( str[1] );
	point.setX( atof(str[2].c_str() ) * 1000.0 );
	point.setY( atof(str[3].c_str() ) * 1000.0 );
	point.setHeight( atof( str[4].c_str() ) * 1000.0 );
	point.setGrade( atof( str[5].c_str() ) );

	if ( setDistance (str, point) != JPT_OK ) {
		return JPT_ERROR;
	}
	if ( setLength (str, point) != JPT_OK ) {
		return JPT_ERROR;
	}
	point.setAngle( atof( str[10].c_str()) );
	point.setStation( str[11].c_str() );
	point.setCAngle( atof( str[12].c_str() ) );

	if ( setCrossAngle ( str, point) != JPT_OK ) {
		return JPT_ERROR;
	}
	point.setRadius( atof(str[17].c_str() ) * 1000.0 );
	point.setZ1( atof(str[18].c_str() ) * 1000.0 );
	point.setZ2( atof(str[19].c_str() ) * 1000.0 );
	point.setFlag( atoi(str[20].c_str() ) );
	
	return JPT_OK;
}

/**
 *	sets distance from String Tokenizer
 *
 *	@param	str		(input)
 *	@param	point	(output)
 *	@return		JptErrorStatus	JPT_OK		normal termination
 *								JPT_ERROR	error
 */
JptErrorStatus AlnTextFileReader::setDistance( StringTokenizer& str, AlnPointData& point)
{
	// 2014/03/10 Nakagawa Edit Start
//	if ( str.sizeStr() != 21 ) {
	if ( str.sizeStr() < 21 ) {
	// 2014/03/10 Nakagawa Edit End
		return JPT_ERROR;
	}
	AlnMensuration distance;
	distance.setSingle( atof(str[8].c_str()) * 1000.0 );
	distance.setCumulative( atof(str[9].c_str()) * 1000.0 );
	point.setDistance( distance );
	return JPT_OK;
}

/**
 *	sets length from String Tokenizer
 *
 *	@param	str		(input)
 *	@param	point	(output)
 *	@return		JptErrorStatus	JPT_OK		normal termination
 *								JPT_ERROR	error
 */
JptErrorStatus AlnTextFileReader::setLength( StringTokenizer& str, AlnPointData& point)
{
	// 2014/03/10 Nakagawa Edit Start
//	if ( str.sizeStr() != 21 ) {
	if ( str.sizeStr() < 21 ) {
	// 2014/03/10 Nakagawa Edit End
		return JPT_ERROR;
	}
	AlnMensuration length;
	length.setSingle( atof(str[6].c_str() ) * 1000.0 );
	length.setCumulative( atof(str[7].c_str()) * 1000.0);
	point.setLength( length );
	return JPT_OK;
}

/**
 *	sets crossAngle	from String Tokenizer
 *
 *	@param	str		(input)
 *	@param	point	(output)
 *	@return		JptErrorStatus	JPT_OK		normal termination
 *								JPT_ERROR	error
 */
JptErrorStatus AlnTextFileReader::setCrossAngle(  StringTokenizer& str, AlnPointData& point)
{
	// 2014/03/10 Nakagawa Edit Start
//	if ( str.sizeStr() != 21 ) {
	if ( str.sizeStr() < 21 ) {
	// 2014/03/10 Nakagawa Edit End
		return  JPT_ERROR;
	}
	AlnCrossAngle crossAngle;
	crossAngle.setCrossAngle1( atof( str[13].c_str() ) );
	crossAngle.setCrossAngle2( atof( str[14].c_str() ) );
	crossAngle.setCrossAngle3( atof( str[15].c_str() ) );
	crossAngle.setCrossAngle4( atof( str[16].c_str() ) );
	point.setCrossAngle( crossAngle );
	return JPT_OK;
}

