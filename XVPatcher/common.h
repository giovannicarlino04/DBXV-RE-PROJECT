
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#endif

#include <string>
#include <filesystem>

#ifndef COMMON_H
#define COMMON_H

bool HookImport(const char* moduleName, const char* functionName, void* newFunction);
uintptr_t GetModuleBaseAddress(const wchar_t* modName);
std::string NormalizePath(const std::string& path);
void Hook(void* myFunc, void* detour, void* origFunc);
bool FileExists(const std::string& filePath);

#endif //COMMON_H