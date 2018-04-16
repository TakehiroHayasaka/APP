#pragma warning (disable:4786)

#include "stdafx.h"
#include <algorithm>
#include <sstream>
using namespace std;

#include "StringTokenizer.h"
#include "AlnLinearCoordinateData.h"

namespace {
	
	bool sortStation(const AlnPointData& point1, const AlnPointData& point2) 
	{	
        std::string::size_type pos1 = point1.getStation().find_first_of("+-");

        if (pos1 == std::string::npos) {
            return atof(point1.getStation().c_str()) < atof(point2.getStation().c_str());
        }

        std::string::size_type pos2 = point2.getStation().find_first_of("+-");
        if (point1.getStation().substr(0, pos1) == point2.getStation().substr(0, pos2)) {
            return atof(point1.getStation().substr(pos1).c_str())
                   < atof(point2.getStation().substr(pos2).c_str());
        } else {
            return atoi(point1.getStation().substr(0, pos1).c_str())
                   < atoi(point2.getStation().substr(0, pos2).c_str());
        }
	}

    bool sortCumulativeLength(const AlnPointData& point1, const AlnPointData& point2)
    {
        return point1.getLength().getCumulative() < point2.getLength().getCumulative();
    }

	std::string intToString(const int lineNumber)
	{
		char b[10];
		_ltoa(lineNumber, b, 10 );
		std::string stringLineNumber (b); 

		return stringLineNumber;
	}

}

/**
 *	appends AlnPointData to the vector
 */
void AlnLinearCoordinateData::append(const AlnPointData& point)
{
	this->_points.push_back( point );
}

/**
 *	gets AlnPointData at index location of the vector
 */
JptErrorStatus AlnLinearCoordinateData::getAt(int index, AlnPointData& point) const
{
	if ( ( index < 0) || ( index >= this->size() ) ) {
		return JPT_ERROR;
	}
	point = this->_points[index];
	return JPT_OK;
}

/**
 *	sets point into index location of the vector
 */
JptErrorStatus AlnLinearCoordinateData::setAt(int index, const AlnPointData& point)
{
	if ( ( index < 0 ) || ( index >= this->size() ) ) {
		return JPT_ERROR;
	}
	this->_points[index] = point;
	return JPT_OK;
}

/**
 *	to String
 */
const std::string AlnLinearCoordinateData::toString() const
{
	ostringstream str;
	str << "\"" << "AlnLinearCoordinateData" << "\"" << endl
		<< "\"" << _lineName << "\"" << endl
		<< this->toStringPoints();
	return str.str();
}

/**
 *	sort _points vector by station in ascending order
 */
JptErrorStatus AlnLinearCoordinateData::sortByStation()
{
	if ( this->size() == 0 ) {
		return JPT_ERROR;
	}
	std::sort( this->_points.begin(), this->_points.end(), sortStation );
	return JPT_OK;
}

/**
 * sort _points list by cumulative length in ascending order
 */
JptErrorStatus AlnLinearCoordinateData::sortByCumulativeLength()
{
    if (this->size() == 0) {
        return JPT_ERROR;
    }

    std::sort(_points.begin(), _points.end(), sortCumulativeLength);

    return JPT_OK;
}

/**
 *	
 */
bool AlnLinearCoordinateData::isAlreadyPoint(const std::string& pointName) const
{
	std::vector<AlnPointData>::const_iterator citer = this->_points.begin();
	for ( ; citer != this->_points.end(); ++citer ) {
		if ( pointName == citer->getPointName() ) {
			return true;
		}
	}
	return false;
}

/**
 *
 */
JptErrorStatus AlnLinearCoordinateData::find(const std::string& name, AlnPointData& point) const 
{
	std::vector<AlnPointData>::const_iterator citer = this->_points.begin();
	for ( ; citer != this->_points.end(); ++citer ) {
		if ( name == citer->getPointName() ) {
			point = *citer;
			return JPT_OK;
		}
	}
	return JPT_ERROR;
}

/**
 *	add lcdNew's AlnPointData vector to already existing AlnLinearCoordinateData
 *
 *	@param	rhs			-	contais the points to be added
 *	@return		JptErrorStatus	JPT_OK	normal termination
 *								JPT_ERROR	error, points of the same name having different coordinates
 */
JptErrorStatus AlnLinearCoordinateData::addPoints(const AlnLinearCoordinateData& rhs)
{
	for ( int i= 0; i < rhs.size(); i++) {
		//get AlnPointData to be added 
		AlnPointData point;
		if ( rhs.getAt(i, point) != JPT_OK ) {
			return JPT_ERROR;
		}
		//check if AlnPointData exist
		AlnPointData samePoint;
		if ( this->find(point.getPointName(), samePoint) == JPT_OK ) {
			if ( !(samePoint == point ) ) {
				return JPT_ERROR;
			}
		} else { 
			//append new point
			this->append( point );
		}
	}
	this->sortByStation();
	return JPT_OK;
}

/**
 *	returns index of name
 *	@param	name	- point name
 *	@return		int		index location	: pointName found
 *						-1				: pointName not found
 */
const int AlnLinearCoordinateData::index(const std::string& name) const
{
	for ( int i=0; i < this->size(); i++) {
		if ( this->_points[i].getPointName() == name) {
			return i;
		}
	}
	return -1;
}

/**
 *	assuming that the vector is already sorted according to station, 
 *	this function searches AlnPointData in between pointA and pointB
 *
 *	@param	pointA					:	first point
 *	@param	pointB					:	second point
 *	@param	AlnLinearCoordinateData	:	[out] contains AlnPointData in between the two points
 *	@return		JptErrorStatus	JPT_OK		-	normal termination
 *								JPT_ERROR	-	error
 */
JptErrorStatus AlnLinearCoordinateData::pickPoints(const std::string& pointA, const std::string& pointB, 
												AlnLinearCoordinateData& AlnLinearCoordinateData) const
{
	int startPoint = this->index(pointA);
	int endPoint = this->index(pointB);
	if ( ( startPoint == -1) || ( endPoint == -1) ) {
		return JPT_ERROR;
	}
	// from start point to end point
	if ( startPoint < endPoint) {
		for ( int i = startPoint; i <= endPoint; i++) {
			AlnLinearCoordinateData.append( this->_points[i]);
		}
	//from end point to start point
	} else if ( startPoint >= endPoint ) {
		for ( int i = startPoint; i >= endPoint; i--) {
			AlnLinearCoordinateData.append( this->_points[i]);
		}
	}
	return JPT_OK;
}

void AlnLinearCoordinateData::reverseY()
{
	std::vector<AlnPointData>::iterator iter = this->_points.begin();
	for ( ; iter != this->_points.end(); ++iter ) {
		iter->reverseY();
	}
}
/************************************************************************************
	private functions
*************************************************************************************/
/**
 *	prints _points vector
 */
const std::string AlnLinearCoordinateData::toStringPoints () const
{
	ostringstream str;
	str << this->size() << endl;
	std::vector<AlnPointData>::const_iterator citer = this->_points.begin();
	for ( ; citer != this->_points.end(); ++citer ) {
		str << citer->toString();
	}
	return str.str();
}

