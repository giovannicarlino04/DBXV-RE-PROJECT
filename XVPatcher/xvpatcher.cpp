#include <Windows.h>
#include <Psapi.h>
#include <fstream>
#include <sstream>
#include <iostream>
#include <string>
#include <stdexcept>
#include <algorithm>

std::ofstream logFile;
std::string debug;

void LogDebug(const std::string& message)
{
    if(debug == "true"){
        logFile << message << std::endl;
        std::cout << message << std::endl;
    }
}

std::string GetIniValue(const std::string& filePath, const std::string& section, const std::string& key)
{
    const int bufferSize = 255;
    char buffer[bufferSize];

    DWORD bytesRead = GetPrivateProfileStringA(section.c_str(), key.c_str(), "", buffer, bufferSize, filePath.c_str());

    LogDebug("Got ini value at " + section + " " + key);
    return std::string(buffer, bytesRead);
}

int main()
{
    try {
        logFile.open("XVPatcher//XVPatcher.log", std::ofstream::out | std::ofstream::trunc);

        // Find the process ID based on the window name
        HWND windowHandle = FindWindowW(nullptr, L"DRAGON BALL XENOVERSE");
        DWORD processId = 0;

        // Load patch values from INI file
        std::string iniFilePath = "XVPatcher/XVPatcher.ini";
        debug = GetIniValue(iniFilePath, "Patches", "debug_patches");

        if (!windowHandle)
        {
            // Launch the DBXV.exe executable
            ShellExecuteW(nullptr, L"open", L"DBXV.exe", nullptr, nullptr, SW_SHOWNORMAL);

            // Wait for 3 seconds
            Sleep(3000);

            // Find the process ID again
            windowHandle = FindWindowW(nullptr, L"DRAGON BALL XENOVERSE");
        }

        if (windowHandle)
        {
            GetWindowThreadProcessId(windowHandle, &processId);
        }
        else
        {
            LogDebug("Debug: Failed to find the game window.");
            return 1;
        }

        // Open the process and obtain the process handle
        HANDLE processHandle = OpenProcess(PROCESS_ALL_ACCESS, FALSE, processId);
        if (processHandle)
        {
            // PATCHES GO HERE

            // Close the process handle
            CloseHandle(processHandle);
        }
        else
        {
            LogDebug("Debug: Failed to open the process.");
        }

        logFile.close();
    }
    catch (std::exception& ex) {
        LogDebug(ex.what());
    }

    return 0;
}
