#include <iostream>
#include <string>
#include <Windows.h>
#include <TlHelp32.h>
#include <fstream>
#include <vector>
#include <Psapi.h>
#include <sstream>
#include <cstring> // Add this line for the memcmp function
#include <memory>

#pragma comment(lib, "wininet.lib")

constexpr wchar_t* INI_FILE_NAME = L"XVPatcher/XVPatcher.ini";

bool debug = true;
std::ofstream logFile;

std::wstring ConvertTCHARToString(const TCHAR* str)
{
#ifdef _UNICODE
    return std::wstring(str);
#else
    int length = MultiByteToWideChar(CP_ACP, 0, str, -1, nullptr, 0);
    std::wstring wideString(length, L'\0');
    MultiByteToWideChar(CP_ACP, 0, str, -1, &wideString[0], length);
    return wideString;
#endif
}

std::wstring ConvertToWideString(const std::string& narrowString)
{
    int wideStringLength = MultiByteToWideChar(CP_UTF8, 0, narrowString.c_str(), -1, nullptr, 0);
    if (wideStringLength == 0)
    {
        return L"";
    }

    std::vector<wchar_t> buffer(wideStringLength);
    MultiByteToWideChar(CP_UTF8, 0, narrowString.c_str(), -1, buffer.data(), wideStringLength);

    return std::wstring(buffer.data());
}

std::string ConvertWideStringToString(const std::wstring& wideString)
{
    std::string narrowString;
    narrowString.resize(wideString.size());

    std::wcstombs(&narrowString[0], wideString.c_str(), narrowString.size());

    return narrowString;
}

void Log(const std::string& message)
{
    std::cout << message << std::endl;
    logFile << message << std::endl;
}

uintptr_t GetModuleBaseAddress(HANDLE processHandle, const std::wstring& moduleName)
{
    HMODULE modules[1024];
    DWORD needed;

    if (EnumProcessModules(processHandle, modules, sizeof(modules), &needed))
    {
        for (DWORD i = 0; i < (needed / sizeof(HMODULE)); i++)
        {
            wchar_t modulePath[MAX_PATH];

            if (GetModuleFileNameExW(processHandle, modules[i], modulePath, sizeof(modulePath) / sizeof(wchar_t)))
            {
                std::wstring moduleFileName = std::wstring(modulePath);
                size_t lastSlash = moduleFileName.find_last_of(L"\\");
                std::wstring currentModuleName = moduleFileName.substr(lastSlash + 1);

                if (_wcsicmp(currentModuleName.c_str(), moduleName.c_str()) == 0)
                {
                    if(debug){
                        Log("Debug: Module Scanned: " + ConvertWideStringToString(moduleFileName));
                    }
                    return reinterpret_cast<uintptr_t>(modules[i]);
                }
                else {
                    if(debug){
                        Log("Debug: Module NOT Scanned: " + ConvertWideStringToString(moduleFileName));
                    }
                }
            }
        }
    }

    return 0;
}

std::wstring GetIniValue(const std::wstring& section, const std::wstring& key)
{
    wchar_t buffer[256];
    GetPrivateProfileStringW(section.c_str(), key.c_str(), L"", buffer, sizeof(buffer), INI_FILE_NAME);

    if (debug)
        Log("Debug: Got Ini Value at: " + std::string(section.begin(), section.end()) + " " + std::string(key.begin(), key.end()));

    return buffer;
}

BOOL CheckIfGameRunning(const std::wstring& processName)
{
    DWORD processId = 0;
    HANDLE snapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
    if (snapshot != INVALID_HANDLE_VALUE)
    {
        PROCESSENTRY32 processEntry = { sizeof(PROCESSENTRY32) };
        if (Process32First(snapshot, &processEntry))
        {
            do
            {
                std::wstring currentProcessName = ConvertToWideString(processEntry.szExeFile);
                if (currentProcessName.find(processName) != std::wstring::npos)
                {
                    processId = processEntry.th32ProcessID;
                    break;
                }
            } while (Process32Next(snapshot, &processEntry));
        }

        CloseHandle(snapshot);
    }

    if (processId != 0)
    {
        return TRUE;
    }
    else
    {
        return FALSE;
    }
}

void LaunchGame()
{
    std::wstring processName = L"DBXV.exe";
    std::wstring commandLineArgs = L"";
    std::wstring workingDirectory = L"";

    // Combine the process name and command line arguments
    std::wstring commandLine = processName + L" " + commandLineArgs;

    STARTUPINFOW startupInfo = { sizeof(STARTUPINFOW) };
    PROCESS_INFORMATION processInfo;

    BOOL success = CreateProcessW(
        nullptr,
        const_cast<LPWSTR>(commandLine.c_str()),
        nullptr,
        nullptr,
        FALSE,
        CREATE_SUSPENDED,
        nullptr,
        workingDirectory.c_str(),
        &startupInfo,
        &processInfo
    );

    if (success)
    {
        // Resume the game process to start execution
        ResumeThread(processInfo.hThread);
        CloseHandle(processInfo.hThread);
        CloseHandle(processInfo.hProcess);

        // Wait for the game process to start and initialize
        Sleep(5000);

        // Get the game base address
        std::wstring moduleName = L"DBXV.exe";
        uintptr_t baseAddress = GetModuleBaseAddress(processInfo.hProcess, moduleName);
        if (baseAddress != 0)
        {
            HANDLE processHandle = OpenProcess(PROCESS_ALL_ACCESS, FALSE, processInfo.dwProcessId);
            if (processHandle == NULL)
            {
                if (debug)
                    Log("Debug: Failed to open the game process.");
                return;
            }
            CloseHandle(processHandle);
        }
    }
    else
    {
        DWORD errorCode = GetLastError();
        if (debug)
            Log("Debug: Failed to launch the game. Error code: " + std::to_string(errorCode));
    }
}

int main()
{
    logFile.open("XVPatcher/XVPatcher.log");
    if (!logFile.is_open())
    {
        std::cout << "Failed to open the log file." << std::endl;
        return 1;
    }

    LaunchGame();

    logFile.close();

    return 0;
}
