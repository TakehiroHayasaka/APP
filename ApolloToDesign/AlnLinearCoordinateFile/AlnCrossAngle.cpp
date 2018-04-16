#pragma warning (disable:4786)

#include "stdafx.h"
#include <iomanip>
#include <sstream>
#include <cmath>
using namespace std;

#include "AlnCrossAngle.h"

const std::string AlnCrossAngle::toString() const
{
	ostringstream str;
	
	str << fixed << setprecision(6);
	str << _crossAngle1 << " " << _crossAngle2 << " " << _crossAngle3 << " " << _crossAngle4;

	return str.str();
}

bool AlnCrossAngle::operator ==(const AlnCrossAngle& rhs) const
{
	if ( !(fabs(this->_crossAngle1 - rhs._crossAngle1 ) <= ALN_TOLERANCE_ANGLE) || 
		!(fabs( this->_crossAngle2 - rhs._crossAngle2 ) <= ALN_TOLERANCE_ANGLE) || 
		!(fabs( this->_crossAngle3 - rhs._crossAngle3 ) <= ALN_TOLERANCE_ANGLE) || 
		!(fabs( this->_crossAngle4 - rhs._crossAngle4 ) <= ALN_TOLERANCE_ANGLE) ) {
		return false;
	}
	return true;
}