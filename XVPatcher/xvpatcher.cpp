#include <iostream>
#include <string>
#include <Windows.h>
#include <fstream>
#include <thread>

constexpr wchar_t* INI_FILE_NAME = L"./XVPatcher/XVPatcher.ini";

bool debug = true;
std::ofstream logFile;

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

DWORD GetProcessIdByWindowTitle(const wchar_t* windowTitle)
{
    HWND hwnd = FindWindowW(nullptr, windowTitle);
    if (hwnd)
    {
        DWORD processId;
        GetWindowThreadProcessId(hwnd, &processId);
        return processId;
    }
    return 0;
}

void LaunchGame(const wchar_t* windowTitle)
{
    STARTUPINFOW startupInfo = { sizeof(STARTUPINFOW) };
    PROCESS_INFORMATION processInfo;

    std::wstring processName = L"DBXV.exe";
    std::wstring commandLineArgs = L"";
    std::wstring workingDirectory = L"./";

    // Combine the process name and command line arguments
    std::wstring commandLine = processName + L" " + commandLineArgs;

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

        // Wait for the game process to start and initialize
        Sleep(5000);

        // Get the game process ID
        DWORD processId = GetProcessIdByWindowTitle(windowTitle);
        if (processId != 0)
        {
            std::wcout << L"Process ID for " << windowTitle << L" is " << processId << std::endl;

            // You can proceed to interact with the process or modify it as needed.
            // Replace this comment with your code to work with the process.

            if (debug)
                Log("Debug: Patcher attached correctly to window title: " + ConvertWideStringToString(windowTitle));
        }
        else
        {
            std::wcout << L"Window title not found: " << windowTitle << std::endl;
        }

        CloseHandle(processInfo.hProcess);
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
    logFile.open("./XVPatcher/XVPatcher.log");
    if (!logFile.is_open())
    {
        std::cout << "Failed to open the log file." << std::endl;
        return 1;
    }

    const wchar_t* windowTitle = L"DRAGON BALL XENOVERSE";
    LaunchGame(windowTitle);

    logFile.close();

    return 0;
}
