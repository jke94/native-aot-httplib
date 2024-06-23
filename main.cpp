#include <iostream>
#include <string>
#include <cstring>

// Declaración de las funciones exportadas de la librería compartida
extern "C" int MakeHttpGetRequest(const char* url, void** resultPtr, int* resultSize);
extern "C" void FreeMemory(void* ptr);

int main() 
{
    const char* url = "https://jsonplaceholder.typicode.com/users";
    void* resultPtr = nullptr;
    int resultSize = 0;

    int status = MakeHttpGetRequest(url, &resultPtr, &resultSize);

    if (status == 0) 
    {
        std::string result(static_cast<char*>(resultPtr), resultSize);

        std::cout << "HTTP Response: " << result << std::endl;
        std::cout << "HTTP Result size: " << resultSize << std::endl;

        FreeMemory(resultPtr);
    } 
    else 
    {
        std::cerr << "Failed to make HTTP request\n";
    }

    return 0;
}



/*
g++ -o main main.cpp /home/javi/Repositories/native-aot-httplib/MyHttpLib/bin/Release/net8.0/linux-x64/publish/MyHttpLib.so -Wl,-rpath,/home/javi/Repositories/native-aot-httplib/MyHttpLib/bin/Release/net8.0/linux-x64/publish/
*/