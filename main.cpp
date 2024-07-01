#include <iostream>
#include <string>

// Declaration of functions exported from the shared library.
extern "C" int MakeHttpGetRequest(const char* url, void** resultPtr, int* resultSize, int* httpStatusCode);
extern "C" void FreeMemory(void* ptr);

int main() 
{
    const char* url = "https://jsonplaceholder.typicode.com/comments";

    void* resultPtr = nullptr;
    int resultSize = 0;
    int httpStatusCode = 0;

    int status = MakeHttpGetRequest(url, &resultPtr, &resultSize, &httpStatusCode);

    if (status != 0)
    {
        std::cerr << "Failed to make HTTP request" << std::endl;

        return -1;
    }
    
    std::string result(static_cast<char*>(resultPtr), resultSize);

    std::cout << "HTTP Response: " << result << std::endl;
    std::cout << "HTTP Result size: " << resultSize << std::endl;
    std::cout << "HTTP Status code: " << httpStatusCode << std::endl;

    FreeMemory(resultPtr);

    return 0;
}