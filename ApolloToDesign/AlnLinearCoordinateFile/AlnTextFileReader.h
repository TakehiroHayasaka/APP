#ifndef __ALN_TEXT_FILE_READER_H__
#define __ALN_TEXT_FILE_READER_H__

#include "AlnLinearCoordinateFile.h"
#include "StringTokenizer.h"

class AlnTextFileReader {
public:

	AlnTextFileReader () {}
	AlnTextFileReader ( const std::string &fileName ) : _fileName(fileName) {}
	~AlnTextFileReader () {}

	//read data from text file
	JptErrorStatus load ( std::ifstream& ifs, AlnLinearCoordinateFile *lcfPtr);

private:
	std::string _fileName;

	JptErrorStatus readFileSet ( std::ifstream& ifs, AlnLinearCoordinateFile *lcfPtr);
	JptErrorStatus readSetLinearCoordinateFile(int &lineNumber, std::ifstream& ifs, AlnLinearCoordinateFile *lcfPtr);
	JptErrorStatus setLinearCoordinateData ( StringTokenizer& str, AlnLinearCoordinateData& linearCoordinateData);
	JptErrorStatus setPointData ( StringTokenizer& str, AlnPointData& point);
	JptErrorStatus setLength ( StringTokenizer& str, AlnPointData& point);
	JptErrorStatus setDistance ( StringTokenizer& str, AlnPointData& point);
	JptErrorStatus setCrossAngle ( StringTokenizer& str, AlnPointData& point);



	JptErrorStatus jipReadSetLinearCoordinateFile(std::ifstream& ifs, AlnLinearCoordinateFile *lcfPtr);
	JptErrorStatus jipReadSetLinearCoordinateData(const std::string& oudan, StringTokenizer& strTok, 
													  AlnLinearCoordinateFile  *lcfPtr);
	JptErrorStatus jipReadOudanName(std::ifstream& ifs, std::string& oudan);
	JptErrorStatus jipReadSetPointData(const std::string& oudan, StringTokenizer& strTok, 
										   AlnLinearCoordinateData& linearCoordinateData);
	void jipReadSetMensurationDistance(StringTokenizer& strTok, AlnPointData& pointData);
	void jipReadSetMensurationWidth(StringTokenizer& strTok, AlnPointData& pointData);
	JptErrorStatus jipReadSetBearingAngle(std::string& angle, AlnPointData& pointData);
	JptErrorStatus jipReadSetStation(std::string& station, AlnPointData& pointData);


	void sortPointByStation (AlnLinearCoordinateFile *lcfPtr);

};

#endif // __ALN_TEXT_FILE_READER_H__

	