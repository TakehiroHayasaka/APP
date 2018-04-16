#ifndef __ALNMENSURATION_H__
#define __ALNMENSURATION_H__

#include <string>

const double ALN_TOLERANCE_LENGTH = 0.05;

class AlnMensuration {

public:

	AlnMensuration () : _single ( 0.0), _cumulative(0.0) {}
	AlnMensuration (double s, double c) : _single(s), _cumulative(c) {}
	~AlnMensuration () {}

	bool operator== ( const AlnMensuration& rhs) const;

	//get function
	double getSingle () const { return _single; }
	double getCumulative () const { return _cumulative; }

	//set function
	void setSingle ( double value ) { _single = value; }
	void setCumulative ( double value ) { _cumulative = value; }

	//toString 
	const std::string toString () const;


private:
	double _single;
	double _cumulative;
};

#endif  // __ALNMENSURATION_H__