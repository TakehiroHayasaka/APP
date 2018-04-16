#pragma warning (disable:4786)

#include "stdafx.h"
#include "GWDSectionTextFileReader.h"


JptErrorStatus GWDSectionTextFileReader::load(std::ifstream &ifs, AlnLinearCoordinateFile *lcfPtr)
{
	if ( readFileSet (ifs, lcfPtr) != JPT_OK ) {
		errorPush("***ERROR*** 線形座標値ファイル["+this->_fileName+"]の読み込みに失敗しました。\n");
		ifs.close();
		return JPT_ERROR;
	}
	ifs.close();
	return JPT_OK;
}


JptErrorStatus GWDSectionTextFileReader::readFileSet (std::ifstream& ifs, AlnLinearCoordinateFile *lcfPtr)
{
	int lineNumber = 0;
	std::string line;
	std::getline(ifs, line);
	lineNumber++;

	for ( ; ; ) {
		std::getline(ifs, line);
		lineNumber++;
		if (line.empty() ) {
			break;
		}
		 StringTokenizer sectionStr(line, ",");
		 if (setAlnLinearCoordinateData(sectionStr, lcfPtr) != JPT_OK ) {
			std::stringstream ss;
			ss << "***ERROR*** " << lineNumber << " 行目で読み込みに失敗しました。\n";
			errorPush( ss.str() ); 
			 return JPT_ERROR;
		 }
	}
	return JPT_OK;
}

JptErrorStatus GWDSectionTextFileReader::setAlnLinearCoordinateData(StringTokenizer& sectionStr, 
																	AlnLinearCoordinateFile *lcfPtr)
{
	AlnLinearCoordinateData linearCoordinateData;
	int index = lcfPtr->index(sectionStr[1]);
	if ( index != -1 ) {
		if ( lcfPtr->getAt(index, linearCoordinateData ) != JPT_OK ) {
			linearCoordinateData.setLineName(sectionStr[1]);
		}
		if ( setAlnPointData(sectionStr, linearCoordinateData) != JPT_OK ) {
			return JPT_ERROR;
		}

		if ( lcfPtr->setAt(index, linearCoordinateData) != JPT_OK ) {
			return JPT_ERROR;
		}	
	} else {
		linearCoordinateData.setLineName( sectionStr[1] );

		if ( setAlnPointData(sectionStr, linearCoordinateData) != JPT_OK ) {
			return JPT_ERROR;
		}
		lcfPtr->append(linearCoordinateData);
	}

	return JPT_OK;
}

JptErrorStatus GWDSectionTextFileReader::setAlnPointData(StringTokenizer& sectionStr, 
														 AlnLinearCoordinateData& linearCoordinateData)
{
	if ( sectionStr.sizeStr() < 5 ) {
		return JPT_OK;
	}
	if ( sectionStr.sizeStr() == 5 && sectionStr[2] == "" ) {
		return JPT_OK;
	}
	AlnPointData pointData;
	pointData.setPointName(sectionStr[0]);
	pointData.setX(atof (sectionStr[2].c_str()) * 1000.0);
	pointData.setY(atof (sectionStr[3].c_str()) * 1000.0);
	pointData.setHeight(atof (sectionStr[4].c_str()) * 1000.0);

	if (linearCoordinateData.isAlreadyPoint(sectionStr[0])) {
		return JPT_ERROR;
	}

	linearCoordinateData.append(pointData);
	return JPT_OK;
}
