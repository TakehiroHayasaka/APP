#pragma warning (disable:4786)

#include "stdafx.h"
#include "errorStack.h"

ErrorStack* ErrorStack::_instance = 0;

ErrorStack* ErrorStack::instance()
{
	if ( _instance == 0 ) {
		_instance = new ErrorStack();
	}

	return _instance;
}

const std::string ErrorStack::pop()
{
	std::string top;

	if ( !_errorMessages.empty() ) {

		top = _errorMessages.back();
		_errorMessages.pop_back();

	} else {

		top = "";
	}

	return top;
}

void ErrorStack::push(const std::string &value)
{
	_errorMessages.push_back(value);
}

bool ErrorStack::isEmpty() const
{
	return _errorMessages.empty(); 
}

void ErrorStack::cleanUp()
{
	if ( _instance != 0 ) {
		delete _instance;
        _instance = 0;
	}
}

const std::string ErrorStack::toString() const
{
    std::string messages = _progressMessage;
    
    std::vector<std::string>::const_reverse_iterator riter = _errorMessages.rbegin();
    for (; riter != _errorMessages.rend(); ++riter) {
        messages += *riter;
    }

    return messages;
}
