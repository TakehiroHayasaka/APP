#ifndef __GWD_SECTION_TEXTFILEREADER_H__
#define __GWD_SECTION_TEXTFILEREADER_H__

#include <fstream>

#include "AlnLinearCoordinateFile.h"
#include "StringTokenizer.h"
#include "ErrorStack.h"


class GWDSectionTextFileReader 
{
public:
	GWDSectionTextFileReader () {}
	GWDSectionTextFileReader ( const std::string &fileName ) : _fileName(fileName) {}
	~GWDSectionTextFileReader () {}

	JptErrorStatus load( std::ifstream &ifs, AlnLinearCoordinateFile *lcfPtr);
private:
	std::string _fileName;

	JptErrorStatus GWDSectionTextFileReader::readFileSet (std::ifstream& ifs, AlnLinearCoordinateFile *lcfPtr);
	JptErrorStatus GWDSectionTextFileReader::setAlnLinearCoordinateData(StringTokenizer& sectionStr, 
																	AlnLinearCoordinateFile *lcfPtr);
	JptErrorStatus GWDSectionTextFileReader::setAlnPointData(StringTokenizer& sectionStr, 
														 AlnLinearCoordinateData& linearCoordinateData);
};

#endif	// __GWD_SECTION_TEXTFILEREADER_H__
		//	川田テクノ断面順.csv ファイルReader
