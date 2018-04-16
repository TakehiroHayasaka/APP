#pragma warning (disable:4786)

#include "stdafx.h"
#include <sstream>
#include <iostream>
using namespace std;

#include "AlnLinearCoordinateFile.h"
#include "AlnBinaryFileReader.h"
#include "AlnTextFileReader.h"
#include "JIPTextReader.h"
#include "GWDLineTextFileReader.h"
#include "GWDSectionTextFileReader.h"
#include "CrcCosumoTextReader.h"
#include "JFETextFileReader.h"

namespace {
	std::string intToString(const int lineNumber)
	{
		char b[10];
		_ltoa(lineNumber, b, 10 );
		std::string stringLineNumber (b); 

		return stringLineNumber;
	}
}

/**
 *	appends AlnLinearCoordinateData to vector
 *	@param	linearCoordinateData	
 */
void AlnLinearCoordinateFile::append(const AlnLinearCoordinateData& linearCoordinateData)
{
	this->_linearCoordinateList.push_back(linearCoordinateData);
}

/**
 *	gets AlnLinearCoordinateData from vector at location "index"
 *
 *	@param	index					:	location at vector
 *	@param	linearCoordinateData	:	[out]	AlnLinearCoordinateData
 *	@return		JptErrorStatus	-	JPT_ERROR	index error
 *								-	JPT_OK		normal termination
 */
JptErrorStatus AlnLinearCoordinateFile::getAt(int index,
        AlnLinearCoordinateData& linearCoordinateData) const
{
	if ( ( index < 0 ) || ( index >= this->size() ) ) {
		return JPT_ERROR;
	}
	linearCoordinateData = this->_linearCoordinateList[index];
	return JPT_OK;
}

/**
 *	sets AlnLinearCoordinateData to vector at location "index"
 *
 *	@param	index					:	location at vector
 *	@param	linearCoordinateData	:	AlnLinearCoordinateDAta
 *	@return		JptErrorStatus	-	JPT_ERROR	index error
 *								-	JPT_OK		normal termination
 */
JptErrorStatus AlnLinearCoordinateFile::setAt(int index,
        const AlnLinearCoordinateData& linearCoordinateData)
{
	if ( ( index < 0 ) || ( index >= this->size() ) ) {
		return JPT_ERROR;
	}
	this->_linearCoordinateList[index] = linearCoordinateData;
	return JPT_OK;
}

/**
 *	searches for AlnLinearCoordinateData to vector using string "name"
 *
 *	@param	name					:	lineName
 *	@param	linearCoordinateData	:	AlnLinearCoordinateData
 *	@return		JptErrorStatus	-	JPT_ERROR	string name not found
 *								-	JPT_OK		normal termination
 */
JptErrorStatus AlnLinearCoordinateFile::find(const std::string& name,
        AlnLinearCoordinateData& linearCoordinateData) const
{
	if ( name.empty() ) {
		return JPT_ERROR;
	}
	
    std::vector<AlnLinearCoordinateData>::const_iterator citer = this->_linearCoordinateList.begin();
	for ( ; citer != this->_linearCoordinateList.end(); ++citer) {
		if ( name == citer->getLineName() ) {
			linearCoordinateData = *citer;
			return JPT_OK;
		}
	}

    return JPT_ERROR;
}

/**
 *	searches for "lineName" in vector
 *
 *	@param	lineName	:	string "lineName"
 *	@return		bool	-	"true"		string lineName found 
 *						-	"false"		string lineName not found	
 */
bool AlnLinearCoordinateFile::isLineInFile(const std::string& lineName) const
{
	std::vector<AlnLinearCoordinateData>::const_iterator citer = this->_linearCoordinateList.begin();
	for ( ; citer != this->_linearCoordinateList.end(); ++citer) {
		if ( lineName == citer->getLineName() ) {
			return true;
		}
	}
	return false;
}

/**
 *	lineName のlocation を見つけます
 *
 *	@param	lineName	-	string "lineName"
 *	@return		int		vector location	:	string name found 
 *						-1				:	not found		
 */
const int AlnLinearCoordinateFile::index(const std::string& lineName ) const
{
	if ( lineName.empty() ) {
		return -1;
	}
	for ( int i=0; i < this->size(); i++) {
		if ( lineName == this->_linearCoordinateList[i].getLineName() ) {
			return  i;
		}
	}
	return -1;
}

/**
 *	toString
 */
const std::string AlnLinearCoordinateFile::toString() const
{
	ostringstream str;
	str << "\"" << "AlnLinearCoordinateFile" << "\"\n"
        << ((_sortingWay == ALN_SORT_STATION) ? "station" : "cumulative length") << "\n"
		<< this->size() << endl;
	std::vector<AlnLinearCoordinateData>::const_iterator citer = this->_linearCoordinateList.begin();
	for ( ; citer != this->_linearCoordinateList.end(); ++citer ) {
		str << citer->toString();
	}
	return str.str();
}

/**
 *	sort AlnLinearCoordinateData by station
 */
JptErrorStatus AlnLinearCoordinateFile::sortPointDataByStation()
{
	std::vector<AlnLinearCoordinateData>::iterator iter = this->_linearCoordinateList.begin();
	for ( ; iter != this->_linearCoordinateList.end(); ++iter) {
		if ( iter->sortByStation() != JPT_OK ) {
			return JPT_ERROR;
		}
	}
	return JPT_OK;
}

/**
 * sorts AlnLinearCoordinateData by cumulative length
 */
JptErrorStatus AlnLinearCoordinateFile::sortPointDataByCumulativeLength()
{
	std::vector<AlnLinearCoordinateData>::iterator iter = this->_linearCoordinateList.begin();
	for ( ; iter != this->_linearCoordinateList.end(); ++iter) {
		if ( iter->sortByCumulativeLength() != JPT_OK ) {
			return JPT_ERROR;
		}
	}
	return JPT_OK;
}

/**
 *	merges AlnLinearCoordinateFile "lcf" with already existing vector
 *	
 *	@param	lcf	:	AlnLinearCoordinateFile to be added
 *	@return		JptErrorStatus	-	JPT_ERROR	error
 *								-	JPT_OK		normal termination
 */
JptErrorStatus AlnLinearCoordinateFile::add(const AlnLinearCoordinateFile& lcf)
{
	for ( int i = 0; i < lcf.size(); i++) {
		//gets linearCoordinateData to be added
		AlnLinearCoordinateData lcdNew;
		if ( lcf.getAt(i, lcdNew) != JPT_OK ) {
			return JPT_ERROR;
		}
		//gets existing linearCoordinateData
		AlnLinearCoordinateData linearCoordinateData;
		if ( this->find( lcdNew.getLineName(), linearCoordinateData) == JPT_OK ) {
			//add PointData to linearCoordinateData
			if (  linearCoordinateData.addPoints (lcdNew) != JPT_OK ) {
				return JPT_ERROR;
			}
			//set back to vector
			if ( this->setAt( this->index(lcdNew.getLineName() ), linearCoordinateData ) != JPT_OK ) {
				return JPT_ERROR;
			}
		} else {
			this->append(lcdNew);
		}
	}
	return JPT_OK;
}


/**
 *	collects PointData from pointA to pointB in AlnLinearCoordinateData of "lineName"
 *
 *	@param	lineName	:	string line name
 *	@param	pointA		:	string name of start point
 *	@param	pointB		:	string name of end point
 *	@param	lcd			:	[output] AlnLinearCoordinateData 
 *	@return		JptErrorStatus	JPT_ERROR	error, lineName or pointName not found
 *								JPT_OK		normal termination
 */
JptErrorStatus AlnLinearCoordinateFile::pickLinePointData(const std::string& lineName,
        const std::string& pointA, const std::string& pointB, AlnLinearCoordinateData& lcd) const
{
	//sets line name
	lcd.setLineName(lineName);

	AlnLinearCoordinateData linearCoordinateData;
	//gets linearCoordinateData
	if ( this->find(lineName, linearCoordinateData) != JPT_OK ) {
		return JPT_ERROR;
	}
	//searches PointData in between pointA and pointB
	if ( linearCoordinateData.pickPoints(pointA, pointB, lcd) != JPT_OK ) {
		return JPT_ERROR;
	}
	return JPT_OK;
}

void AlnLinearCoordinateFile::reverseY()
{
	std::vector<AlnLinearCoordinateData>::iterator iter = this->_linearCoordinateList.begin();
	for ( ; iter != this->_linearCoordinateList.end(); ++iter ) {
		iter->reverseY();
	}
}

JptErrorStatus AlnLinearCoordinateFile::load(const std::string& name)
{
	if (name.empty() ) {
		return JPT_ERROR;
	}
	if ( getFilenameExtension(name) == "CSV" ) {
		if ( loadTextFile(name) != JPT_OK ) {
			return JPT_ERROR;
		}
	} else {
		AlnBinaryFileReader alnBinary;
		if ( alnBinary.load( name, this) != JPT_OK ) {
			return JPT_ERROR;
		}
	}
	return JPT_OK;
}

const std::string AlnLinearCoordinateFile::getFilenameExtension(
        const string& filename
) const
{
    string::size_type pos = filename.find_last_of(".");
    if (pos == string::npos) {
        return "";
    }
    string ext = filename.substr(pos + 1);
    for (int i = 0; i < ext.size(); ++i) {
        ext[i] = toupper(ext[i]);
    }
    return ext;
}

JptErrorStatus AlnLinearCoordinateFile::loadTextFile(const std::string& name)
{
	std::ifstream ifs;
	if ( readFileOpen(name, ifs) != JPT_OK ) {
		errorPush("***ERROR*** 線形座標値ファイル["+ name + "]を開くことができませんでした。\n");
		return JPT_ERROR;
	}

	std::string line;
	getline (ifs, line);
	if ( line.find("断面名称,曲線名称") != string::npos) {
		ifs.seekg( 0 );
		GWDSectionTextFileReader gwdSectionReader ( name );
		if ( gwdSectionReader.load(ifs, this) != JPT_OK ) {
			return JPT_ERROR;
		}
	} else if ( line.find("曲線名称,断面名称") != string::npos){
		ifs.seekg( 0 );
		GWDLineTextFileReader gwdLineReader ( name );
		if ( gwdLineReader.load(ifs, this) != JPT_OK ) {
			return JPT_ERROR;
		}	
	} else { 

		getline(ifs, line);		// reads second line
		getline(ifs, line);		// reads third line
		getline(ifs, line);		// reads fourth line
		StringTokenizer lineTok(line, ",");
		if ( lineTok.sizeStr() == 12 ) {
			JIPTextReader jipText ( name );
			ifs.seekg( 0 );
			if ( jipText.load(ifs, this) != JPT_OK ) {
				return JPT_ERROR;
			}
		} else if ( lineTok.sizeStr () == 11 ) {
			CrcCosumoTextReader crcCosumoReader ( name );
			ifs.seekg( 0 );
			if ( crcCosumoReader.load(ifs, this) != JPT_OK ) {
				return JPT_ERROR;
			}
		} else if ( lineTok.sizeStr() == 10 ) {
			JFETextFileReader jfeTextFileReader ( name );
			ifs.seekg( 0 );
			if ( jfeTextFileReader.load(ifs, this) != JPT_OK ) {
				return JPT_ERROR;
			}
		// 2014/03/08 Nakagawa Edit Start Z3対応
//		} else if ( lineTok.sizeStr() == 21 ) {
		} else if ( lineTok.sizeStr() >= 21 ) {
		// 2014/03/08 Nakagawa Edit End
			AlnTextFileReader alnText ( name );
			ifs.seekg( 0 );
			if ( alnText.load(ifs, this) != JPT_OK ) {
				return JPT_ERROR;
			}
		}
	}
	return JPT_OK;	
}

JptErrorStatus AlnLinearCoordinateFile::readFileOpen(const std::string& name, std::ifstream& ifs)
{
	// 2017/04/06 Nakagawa Add Start  VS2005だと日本語のファイル名が読み込めない。
	locale default_locale;
	locale::global(locale("japanese"));
	// 2017/04/06 Nakagawa Add End
	ifs.open(name.c_str(), ios::in);
	// 2017/04/06 Nakagawa Add Start
	locale::global(locale(default_locale));
	// 2017/04/06 Nakagawa Add End
	if ( !ifs ) {
		return JPT_ERROR;
	}
	return JPT_OK;
}

void AlnLinearCoordinateFile::clear()
{
	if ( !this->_linearCoordinateList.empty() ) {
		this->_linearCoordinateList.clear();
	}
}
