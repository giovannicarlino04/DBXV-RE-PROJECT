#include <iostream>
#include <Windows.h>
#include <Psapi.h>
#include <fstream>
#include <sstream>

std::ofstream logFile;

void LogDebug(const std::string& message)
{
    logFile << message << std::endl;
    std::cout << message << std::endl;
}

int main()
{
    try {
        logFile.open("XVPatcher//XVPatcher.log", std::ofstream::out | std::ofstream::trunc);

        // Find the process ID based on the window name
        HWND windowHandle = FindWindowW(nullptr, L"DRAGON BALL XENOVERSE");
        DWORD processId = 0;

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
