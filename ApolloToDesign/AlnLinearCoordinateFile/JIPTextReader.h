#ifndef __JIP_TEXT_READER_H__
#define __JIP_TEXT_READER_H__

#include <string>
#include <fstream>

#include "AlnLinearCoordinateFile.h"
#include "AlnLinearCoordinateData.h"
#include "AlnPointData.h"
#include "StringTokenizer.h"
#include "ErrorStack.h"


class JIPTextReader 
{
public:
	JIPTextReader () {}
	JIPTextReader ( const std::string &fileName ) : _fileName(fileName) {}
	~JIPTextReader () {}

	JptErrorStatus load(std::ifstream &ifs, AlnLinearCoordinateFile *lcfPtr);
private:
	std::string _fileName;

	JptErrorStatus JIPTextReader::jipReadFileSet(std::ifstream& ifs, AlnLinearCoordinateFile *lcfPtr);
	JptErrorStatus JIPTextReader::jipReadSetLinearCoordinateData(const std::string& oudan, StringTokenizer& strTok, 
													  AlnLinearCoordinateFile *lcfPtr);
	JptErrorStatus JIPTextReader::jipReadSetPointData(const std::string& oudan, StringTokenizer& strTok, 
										   AlnLinearCoordinateData& linearCoordinateData);
	void JIPTextReader::jipReadSetMensurationDistance(StringTokenizer& strTok, AlnPointData& pointData);
	void JIPTextReader::jipReadSetMensurationWidth(StringTokenizer& strTok, AlnPointData& pointData);
};



#endif // __JIP_TEXT_READER_H__



