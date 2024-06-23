# native-aot-httplib

Demonstration about how to use dotnet AOT technology to make HTTP requests in .NET and exporting functions to consume the request´s response using dynamic library from C++ client.

## How to work.

### 1. Generate dynamic library from C# project based in dotnet AOT.

```
dotnet publish -c Release -r linux-x64 --self-contained
```
### 2. Generated C++ client and linked with dynamic library.

```
g++ -o main main.cpp /home/javi/Repositories/native-aot-httplib/MyHttpLib/bin/Release/net8.0/linux-x64/publish/MyHttpLib.so -Wl,-rpath,/home/javi/Repositories/native-aot-httplib/MyHttpLib/bin/Release/net8.0/linux-x64/publish/
```