#ifndef __ALN_LINEAR_COORDINATE_DATA_H__
#define __ALN_LINEAR_COORDINATE_DATA_H__

#include <vector>
#include "AlnPointData.h"
#include "ErrorStack.h"
#include "JptDef.h"

class AlnLinearCoordinateData {
public:
	AlnLinearCoordinateData () : _lineName("") {}
	AlnLinearCoordinateData ( const std::string& lineName ) : _lineName(lineName) {}

	~AlnLinearCoordinateData () {}

	//accessing vector _points
	void append ( const AlnPointData& point );
	JptErrorStatus getAt ( int index, AlnPointData& point) const;
	JptErrorStatus setAt ( int index, const AlnPointData& point);
	const int size() const { return _points.size(); }
	const int index (const std::string& name ) const;

	void setPoints ( const std::vector<AlnPointData>& points) { _points = points; }
	const std::vector<AlnPointData>& getPoints() const { return _points; }

	//get function
	const std::string& getLineName() const { return _lineName; }

	//set function
	void setLineName ( const std::string& lineName ) { _lineName = lineName; }

	//toString
	const std::string toString () const;

	//sorting
	JptErrorStatus sortByStation ();
    JptErrorStatus sortByCumulativeLength();

	//searching
	bool isAlreadyPoint ( const std::string& pointName ) const;
	JptErrorStatus find ( const std::string& pointName, AlnPointData& point)const;

	//add data
	JptErrorStatus addPoints ( const AlnLinearCoordinateData& rhs );

	//pickPoints
	JptErrorStatus pickPoints ( const std::string& pointA, const std::string& pointB, 
		AlnLinearCoordinateData& AlnlinearCoordinateData) const;

	//reverseY
	void reverseY();

private:
	std::string _lineName;
	std::vector<AlnPointData> _points;

	const std::string toStringPoints () const;

};

#endif // __ALN_LINEAR_COORDINATE_DATA_H__
