#ifndef __GWD_LINE_TEXT_FILE_READER_H__
#define __GWD_LINE_TEXT_FILE_READER_H__

#include <fstream>

#include "AlnLinearCoordinateFile.h"
#include "StringTokenizer.h"
#include "ErrorStack.h"

class GWDLineTextFileReader {
public:
	GWDLineTextFileReader () {}
	GWDLineTextFileReader (const std::string &fileName) : _fileName(fileName) {}
	~GWDLineTextFileReader () {}

	JptErrorStatus load( std::ifstream &ifs, AlnLinearCoordinateFile *lcfPtr);
private:
	std::string _fileName;
	JptErrorStatus GWDLineTextFileReader::readFileOpen(const std::string& name, std::ifstream& ifs);
	JptErrorStatus GWDLineTextFileReader::readFileSet(std::ifstream& ifs, AlnLinearCoordinateFile *lcfPtr);
	JptErrorStatus GWDLineTextFileReader::setAlnLinearCoordinateData(int& lineNumber, std::ifstream& ifs, 
																 AlnLinearCoordinateData& lcd);
	JptErrorStatus GWDLineTextFileReader::setAlnPointData( StringTokenizer& lineStr, AlnLinearCoordinateData& lcd );




};

#endif __GWD_LINE_TEXT_FILE_READER_H__