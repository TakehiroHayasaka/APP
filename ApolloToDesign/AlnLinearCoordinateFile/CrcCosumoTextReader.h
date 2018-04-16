#ifndef __CRC_COSUMO_TEXT_READER_H__
#define __CRC_COSUMO_TEXT_READER_H__

#include <fstream>
#include "AlnLinearCoordinateFile.h"
#include "ErrorStack.h"

class CrcCosumoTextReader {
public:
	CrcCosumoTextReader () {}
	CrcCosumoTextReader ( const std::string &fileName )  : _fileName(fileName) {}

	~CrcCosumoTextReader () {}

	JptErrorStatus load ( std::ifstream &ifs, AlnLinearCoordinateFile *lcfPtr);
private:
	std::string _fileName;

	JptErrorStatus CrcCosumoTextReader::readFileSet(std::ifstream& ifs, AlnLinearCoordinateFile *lcfPtr);
	JptErrorStatus CrcCosumoTextReader::extractPointName ( const std::string& line, std::string& pointName);
	JptErrorStatus CrcCosumoTextReader::setAlnLinearCoordinateData(int &lineNumber, const std::string& pointName,
															   std::ifstream& ifs,
															   AlnLinearCoordinateFile *lcfPtr);
	JptErrorStatus CrcCosumoTextReader::setAlnPointData(const std::string& pointName, StringTokenizer& lineStrTok,
													AlnLinearCoordinateData& lcd);
	JptErrorStatus CrcCosumoTextReader::setAlnLengthMensuration(StringTokenizer& lineStrTok, AlnPointData& point);
	JptErrorStatus CrcCosumoTextReader::setAlnDistanceMensuration(StringTokenizer& lineStrTok, AlnPointData& point);






};

#endif // __CRC_COSUMO_TEXT_READER_H__