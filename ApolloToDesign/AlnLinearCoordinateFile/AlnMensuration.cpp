#pragma warning (disable:4786)

#include "stdafx.h"
#include <sstream>
#include <string>
#include <iomanip>
#include <cmath>
using namespace std;

#include "AlnMensuration.h"

const std::string AlnMensuration::toString() const
{
	ostringstream str;

	str << fixed << setprecision(6);
	str << _single << " " << _cumulative;

	return str.str();
}

bool AlnMensuration::operator ==(const AlnMensuration& rhs) const 
{
	if ( !(fabs ( this->_single - rhs._single) <= ALN_TOLERANCE_LENGTH) ||
         !(fabs( this->_cumulative - rhs._cumulative) <= ALN_TOLERANCE_LENGTH)) {
		return false;
	}

	return true;
}
