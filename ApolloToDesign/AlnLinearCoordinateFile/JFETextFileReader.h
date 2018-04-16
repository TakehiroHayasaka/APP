#ifndef __JFE_TEXTFILEREADER_H__
#define __JFE_TEXTFILEREADER_H__

#include "AlnLinearCoordinateFile.h"
#include "ErrorStack.h"

class JFETextFileReader 
{
public:
	JFETextFileReader () {}
	JFETextFileReader ( const std::string &fileName ) : _fileName(fileName) {}
	~JFETextFileReader () {}

	JptErrorStatus load ( std::ifstream &ifs, AlnLinearCoordinateFile* lcfPtr);
private:
	std::string _fileName;
	JptErrorStatus readFileSet (std::ifstream& ifs, AlnLinearCoordinateFile *lcfPtr);
	JptErrorStatus setLinearCoordinateData(int& lineNumber, std::ifstream& ifs, AlnLinearCoordinateData& lcd );
	JptErrorStatus setPointData(StringTokenizer& pointDataStrkr, AlnPointData& pointData);
	JptErrorStatus setAlnLength( StringTokenizer& pointDataStkr, AlnPointData& pointData);


};

#endif // __JFE_TEXTFILEREADER_H__

