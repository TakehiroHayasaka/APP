#ifndef __ALNCROSSANGLE_H__
#define __ALNCROSSANGLE_H__

#include <string>

const double ALN_TOLERANCE_ANGLE = 1.0e-7;

class AlnCrossAngle {
public:
	AlnCrossAngle () : _crossAngle1(0.0), _crossAngle2(0.0), _crossAngle3(0.0), _crossAngle4(0.0) {}
	AlnCrossAngle ( double a, double b, double c, double d) : _crossAngle1(a), _crossAngle2(b),
		_crossAngle3(c), _crossAngle4(d) {}
	~AlnCrossAngle () {}

	bool operator== ( const AlnCrossAngle& rhs) const;

	//get function
	double getCrossAngle1() const { return _crossAngle1; }
	double getCrossAngle2() const { return _crossAngle2; }
	double getCrossAngle3() const { return _crossAngle3; }
	double getCrossAngle4() const { return _crossAngle4; }

	//set function
	void setCrossAngle1 (double value) { _crossAngle1 = value; }
	void setCrossAngle2 (double value) { _crossAngle2 = value; }
	void setCrossAngle3 (double value) { _crossAngle3 = value; }
	void setCrossAngle4 (double value) { _crossAngle4 = value; }

	//toString
	const std::string toString () const;


private:
	double _crossAngle1;
	double _crossAngle2;
	double _crossAngle3;
	double _crossAngle4;
};

#endif // __ALNCROSSANGLE_H__