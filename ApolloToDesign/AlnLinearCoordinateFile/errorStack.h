#ifndef __ERRORSTACK_H__
#define __ERRORSTACK_H__

#include <string>
#include <vector>

class ErrorStack {
public:
	static ErrorStack* instance();
	static void cleanUp();

	const std::string pop();
	void push(const std::string& value);

    void setProgressMessage(const std::string& message) { _progressMessage = message; }
    const std::string& getProgressMessage() const { return _progressMessage; }

	bool isEmpty() const;
	int size() const { return _errorMessages.size(); }
    void clear() { _errorMessages.clear(); }

    const std::string toString() const;

protected:

	//empty constructor
	ErrorStack() {}

private:
    std::vector<std::string> _errorMessages;
    std::string _progressMessage;

	static ErrorStack* _instance;
};

inline void errorPush(const std::string& text)
{
    ErrorStack::instance()->push(text);
}

inline const std::string errorPop()
{
    return ErrorStack::instance()->pop();
}

#endif // __ERRORSTACK_H__

