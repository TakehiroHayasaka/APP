#ifndef __ALN_LINEAR_COORDINATE_FILE_H__
#define __ALN_LINEAR_COORDINATE_FILE_H__

#include "AlnLinearCoordinateData.h"
#include "ErrorStack.h"

enum AlnSortingWay
{
    ALN_SORT_STATION,
    ALN_SORT_CUMULATIVE_LENGTH,
};

class AlnLinearCoordinateFile {
public:
	AlnLinearCoordinateFile () : _sortingWay(ALN_SORT_STATION) {} 
	~AlnLinearCoordinateFile () {}

	//accessing vector _linearCoordinateList;
	const int size() const { return _linearCoordinateList.size(); }
	void append ( const AlnLinearCoordinateData& linearCoordinateData );
	JptErrorStatus getAt(int index, AlnLinearCoordinateData& linearCoordinateData ) const;
	JptErrorStatus setAt ( int index, const AlnLinearCoordinateData& linearCoordinateData );

    void setSortingWay(enum AlnSortingWay way) { _sortingWay = way; }
    enum AlnSortingWay getSortingWay() const { return _sortingWay; }

	//searching
	JptErrorStatus find ( const std::string& lineName, AlnLinearCoordinateData& linearCoordinateData) const;
	bool isLineInFile ( const std::string& lineName ) const;
	const int index ( const std::string& lineName ) const;

	//toString
	const std::string toString () const;

	//load
	JptErrorStatus load(const std::string& name);


	//sorting
	JptErrorStatus sortPointDataByStation ();
    JptErrorStatus sortPointDataByCumulativeLength();

	//add AlnLinearCoordinateList
	JptErrorStatus add ( const AlnLinearCoordinateFile& lcf);

	JptErrorStatus pickLinePointData ( const std::string& lineName, const std::string& pointA,
		const std::string& pointB, AlnLinearCoordinateData& lcd) const;

	void reverseY();

	void clear ();

private:
	std::vector<AlnLinearCoordinateData> _linearCoordinateList;
    enum AlnSortingWay _sortingWay;

	const std::string getFilenameExtension(const std::string& filename) const;
	JptErrorStatus AlnLinearCoordinateFile::loadTextFile(const std::string& name);
	JptErrorStatus AlnLinearCoordinateFile::readFileOpen(const std::string& name, std::ifstream& ifs);

};

#endif // __ALN_LINEAR_COORDINATE_FILE_H__
