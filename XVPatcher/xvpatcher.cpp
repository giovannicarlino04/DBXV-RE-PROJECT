#include <iostream>
#include <string>
#include <Windows.h>
#include <TlHelp32.h>

std::wstring ConvertToWideString(const CHAR* str)
{
    size_t length = strlen(str) + 1;
    std::wstring wideString(length, L' ');
    mbstowcs(&wideString[0], str, length);
    return wideString;
}

void PatchGameProcess(const std::wstring& processName)
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

    if (processId == 0)
    {
        MessageBoxW(NULL, L"Process not found.", L"Error", MB_OK | MB_ICONERROR);
        return;
    }

    HANDLE processHandle = OpenProcess(PROCESS_ALL_ACCESS, FALSE, processId);
    if (processHandle == NULL)
    {
        MessageBoxW(NULL, L"Failed to open process.", L"Error", MB_OK | MB_ICONERROR);
        return;
    }

    SIZE_T bytesRead = 0;
    MEMORY_BASIC_INFORMATION memoryInfo = { 0 };
    LPVOID address = NULL;

    while (true)
    {
        while (VirtualQueryEx(processHandle, address, &memoryInfo, sizeof(memoryInfo)) != 0)
        {
            if (memoryInfo.State == MEM_COMMIT && (memoryInfo.Type == MEM_MAPPED || memoryInfo.Type == MEM_PRIVATE))
            {
                const SIZE_T bufferSize = memoryInfo.RegionSize;
                std::wstring buffer(bufferSize / sizeof(wchar_t), L'\0');

                if (ReadProcessMemory(processHandle, memoryInfo.BaseAddress, &buffer[0], bufferSize, &bytesRead) && bytesRead > 0)
                {
                    
                }
            }

            address = reinterpret_cast<LPVOID>(reinterpret_cast<BYTE*>(address) + memoryInfo.RegionSize);
        }

        // Sleep for a certain duration before checking again (e.g., 1 second)
        Sleep(1000);
    }

    CloseHandle(processHandle);
}

void LaunchGame()
{
    // Path to the game executable
    const wchar_t* gamePath = L"DBXV.exe";

    // Command line arguments for the game (if any)
    const wchar_t* commandLineArgs = nullptr;

    // Create the process
    STARTUPINFOW startupInfo = { sizeof(STARTUPINFOW) };
    PROCESS_INFORMATION processInfo;
    BOOL success = CreateProcessW(gamePath, const_cast<LPWSTR>(commandLineArgs), nullptr, nullptr, FALSE, 0, nullptr, nullptr, &startupInfo, &processInfo);

    if (success)
    {
        // Close the process and thread handles (we don't need them)
        CloseHandle(processInfo.hProcess);
        CloseHandle(processInfo.hThread);
    }
    else
    {
        // Failed to launch the game, show an error message
        MessageBoxW(nullptr, L"Failed to launch the game.", L"Error", MB_OK | MB_ICONERROR);
    }
}

int main()
{
    LaunchGame();
    std::wstring processName = L"DBXV.exe";
    PatchGameProcess(processName);

    return 0;
}