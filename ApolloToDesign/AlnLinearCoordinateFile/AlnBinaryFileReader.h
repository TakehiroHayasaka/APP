#ifndef __ALNBINARYFILEREADER_H__
#define __ALNBINARYFILEREADER_H__

#include "AlnLinearCoordinateFile.h"
#include "ErrorStack.h"
#include <fstream>

class AlnBinaryFileReader {
public:
	AlnBinaryFileReader () {}
	~AlnBinaryFileReader () {}

	JptErrorStatus load(const std::string& name, AlnLinearCoordinateFile *lcfPtr);
private:
	JptErrorStatus readFileOpen ( const std::string& name, std::ifstream& ifs);
	JptErrorStatus readFileSet (  std::ifstream& ifs, AlnLinearCoordinateFile *lcfPtr);

	JptErrorStatus setLinearCoordinateData(std::ifstream &ifs, const AlnPointData& point, 
		AlnLinearCoordinateFile *lcfPtr);

	JptErrorStatus setPointData ( std::ifstream& ifs, AlnPointData& point);
	JptErrorStatus setLength (  std::ifstream& ifs, AlnPointData& point);
	JptErrorStatus setDistance (  std::ifstream& ifs, AlnPointData& point);
	void readDummy (  std::ifstream& ifs);
	const std::string convertDoubleToString(const double number);

};

#endif // __ALNBINARY_FILE_READER_H__


