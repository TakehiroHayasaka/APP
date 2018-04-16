#ifndef __ALN_POINT_DATA_H__
#define __ALN_POINT_DATA_H__

#include "StringTokenizer.h"
#include "JptDef.h"
#include "AlnCrossAngle.h"
#include "AlnMensuration.h"
#include "ErrorStack.h"

class AlnPointData {
public:
	AlnPointData () : _pointName(""), _x(0.0), _y(0.0), _height(0.0), _grade(0.0), _angle(0.0),
        _station(""), _cAngle(0.0), _radius(0.0), _z1(0.0), _z2(0.0), _flag(0) {}
	
	AlnPointData ( const std::string& name, double x=0.0, double y=0.0, double height=0.0,
        double grade=0.0, double angle=0.0, const std::string& station ="", double cAngle=0.0,
        double radius=0.0, double z1=0.0, double z2=0.0, int flag=0) : _pointName(name), _x(x),
        _y(y), _height(height), _grade(grade), _angle(angle), _station(station), _cAngle(cAngle),
        _radius(radius), _z1(z1), _z2(z2), _flag(flag) {}

	~AlnPointData () {}

	bool operator== (const AlnPointData& ) const;

	//get function
	const std::string& getPointName () const { return _pointName; }
	double getX () const { return _x; }
	double getY () const { return _y; }
	double getHeight () const { return _height; }
	double getGrade () const { return _grade; }
	const AlnMensuration& getLength() const { return _length; }
	const AlnMensuration& getDistance() const { return _distance; }
	double getAngle () const { return _angle; }
	const std::string& getStation () const { return _station; }
	double getCAngle() const { return _cAngle;}
	const AlnCrossAngle& getCrossAngle() { return _crossAngle; }
	double getRadius () const { return _radius; }
	double getZ1() const { return _z1; }
	double getZ2() const { return _z2; }
	int getFlag() const { return _flag; }

	//set function
	void setPointName ( const std::string& p ) { _pointName = p; }
	void setX ( double x ) { _x = x; }
	void setY ( double y ) { _y = y; }
	void setHeight ( double h ) { _height = h; }
	void setGrade ( double g ) { _grade = g; }
	void setLength ( const AlnMensuration& l ) { _length = l; }
	void setDistance ( const AlnMensuration& d ) { _distance = d; }
	void setAngle ( double a ) { _angle = a; }
	void setStation ( const std::string& s ) { _station = s; }
	void setCAngle ( double c ) { _cAngle = c; }
	void setCrossAngle ( const AlnCrossAngle& c ) { _crossAngle = c; }
	void setRadius ( double r ) { _radius = r; }
	void setZ1 ( double z ) { _z1 = z; }
	void setZ2 ( double z ) { _z2 = z; }
	void setFlag ( int f ) { _flag = f; }

	//toString function
	const std::string toString () const;

	//reverse Y;
	void reverseY ();

	//set string format intersection angle to _angle
	JptErrorStatus setAngle(const std::string& angleStr);

	//set string format cross angle to _cAngle;
	JptErrorStatus setCAngle(const std::string& cAngleStr);

private:
	std::string _pointName;		
	double _x;
	double _y;
	double _height;
	double _grade;
	AlnMensuration _length;
	AlnMensuration _distance;
	double _angle;				
	std::string _station;
	double _cAngle;					
	AlnCrossAngle _crossAngle;
	double _radius;
	double _z1;
	double _z2;
	int _flag;

	double convertStrToRadians(StringTokenizer& strSTkr) ;
};
#endif // __ALN_POINT_DATA_H__
