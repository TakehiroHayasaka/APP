#pragma warning (disable:4786)

#include "stdafx.h"
#include <sstream>
#include <iostream>
#include <iomanip>
#include <cmath>
using namespace std;

#include "AlnPointData.h"

namespace {
	const double PI = acos(-1.0);
}

const std::string AlnPointData::toString() const 
{
	ostringstream str;
	str << "\""<< "AlnPointData" << "\"" << " "
		<<"\"" << _pointName << "\"" << " "
		<< fixed << setprecision(6)
		<< _x << " "
		<< _y << " " 
		<< _height << " "
		<< _grade << " "
		<< _length.toString() << " "
		<< _distance.toString() << " "
		<< _angle << " "
		<< _station << " "
		<< _cAngle << " "
		<< _crossAngle.toString() << " " 
		<< _radius << " "
		<< _z1 << " " 
		<< _z2 << " "
		<< _flag << endl;
	return str.str();
}

bool AlnPointData::operator ==(const AlnPointData& point) const
{
	if (  ( this->_flag != point._flag ) ||
		!(fabs( this->_grade - point._grade ) <= ALN_TOLERANCE_LENGTH) || 
		!(fabs( this->_height - point._height ) <= ALN_TOLERANCE_LENGTH) ) {
		return false;
	}
	if ( this->_pointName != point._pointName ) {
		return false;
	}
	if ( !(fabs( this->_x - point._x) <= ALN_TOLERANCE_LENGTH) || 
		!(fabs( this->_y - point._y) <= ALN_TOLERANCE_LENGTH) || 
		!(fabs( this->_z1 - point._z1 ) <= ALN_TOLERANCE_LENGTH) ||
		!(fabs(this->_z2 - point._z2 ) <= ALN_TOLERANCE_LENGTH) ){
		return false;
	}
	if ( !(fabs(this->_radius - point._radius ) <= ALN_TOLERANCE_ANGLE) ) {
		return false;
	}
	if( !(fabs( this->_angle - point._angle) <= ALN_TOLERANCE_ANGLE) ||
        !(fabs(this->_cAngle - point._cAngle) <= ALN_TOLERANCE_ANGLE) ) {
		return false;
	}
	bool equalDistance =( this->_distance == point._distance);
	bool equalLength = ( this->_length == point._length);
	bool equalCrossAngle = ( this->_crossAngle == point._crossAngle);
	if (( equalDistance == false ) || ( equalLength == false) || ( equalCrossAngle == false)) {
		return false;
	}
	if ( this->_station.find("+") != string::npos ) {
		if ( this->_station != point._station)  {
			return false;
		}
	} else {
		if ( atof( this->_station.c_str() ) != atof( point._station.c_str() ) ) {
			return false;
		}
	}
	return true;
}

void AlnPointData::reverseY()
{
	this->_y = this->_y * -1;
}

/**
 *  INTERSECTION ANGLE
 *	When intersection angle is of format "88-88-88.8", this set function should be used.
 *	This function converts the string angle to radians and sets it to private variable _angle
 * 
 *	@ param		:	stringAngle		-	"88-88-88.8" (string)
 *	@ return	:	JptErrorStatus	-	JPT_ERROR :	incorrect string format
 *									-	JPT_OK	  : normal termination
 */

JptErrorStatus AlnPointData::setAngle(const std::string& angleStr) 
{
	if (angleStr.empty() ) {
//		errorPush("***ERROR***  Empty angle string.\n");
		return JPT_ERROR;
	}
	StringTokenizer angleStrk(angleStr, "-");
	if ( angleStrk.sizeStr() != 3 ) {
//		errorPush("***ERROR***  Incorrect number of StringTokenizer elements.\n");
		return JPT_ERROR;
	}
	this->setAngle( this->convertStrToRadians(angleStrk)  );
	return JPT_OK;		
}

/**
 *  CROSS ANGLE
 *  When cross angle is of format "88-88-88.8", this set function should be used.
 *  This function converts the string anlge to radians and sets it to private variable _cAngle
 *
 *	@ param		:	stringAngle		-	"88-88-88.8" (string)
 *	@ return	:	JptErrorStatus	-	JPT_ERROR :	incorrect string format
 *									-	JPT_OK	  : normal termination
 */
JptErrorStatus AlnPointData::setCAngle( const std::string& cAngleStr )
{
	if ( cAngleStr.empty() ) {
//		errorPush("***ERROR***  Empty CAngle string.\n");
		return JPT_ERROR;
	}
	StringTokenizer cAngleStrk ( cAngleStr, "-");
	if ( cAngleStrk.sizeStr() != 3 ) {
//		errorPush("***ERROR***  Incorrect number of StringTokenizer elements.\n");
		return JPT_ERROR;
	}
	this->setCAngle( this->convertStrToRadians( cAngleStrk ) );
	return JPT_OK;
}

double AlnPointData::convertStrToRadians(StringTokenizer& strSTkr) 
{
	double deg = atof ( strSTkr[0].c_str() );
	double min = atof ( strSTkr[1].c_str() ) / 60;
	double sec = atof ( strSTkr[2].c_str() ) / 3600;
	return ((deg + min + sec)  * PI / 180);
}

