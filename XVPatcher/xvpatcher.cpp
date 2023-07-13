#include <iostream>
#include <string>
#include <Windows.h>
#include <TlHelp32.h>
#include <WinINet.h>
#include <fstream>
#include <streambuf>
#include <cstring>  // Added for strlen and mbstowcs

#pragma comment(lib, "wininet.lib")

#define OLD_CHARASELE_LIMIT 64;
#define INI_FILE_NAME L"XVPatcher/XVPatcher.ini"
#define LOG_FILE_NAME "XVPatcher/XVPatcher.log"

bool debug;
std::ofstream logFile;

std::wstring GetIniValue(const std::wstring& section, const std::wstring& key)
{
    wchar_t buffer[256] = { 0 };
    GetPrivateProfileStringW(section.c_str(), key.c_str(), nullptr, buffer, sizeof(buffer), INI_FILE_NAME);
    return std::wstring(buffer);
}

void WriteIniValue(const std::wstring& section, const std::wstring& key, const std::wstring& value)
{
    WritePrivateProfileStringW(section.c_str(), key.c_str(), value.c_str(), INI_FILE_NAME);
}

std::wstring ConvertToWideString(const CHAR* str)
{
    size_t length = strlen(str) + 1;
    std::wstring wideString(length, L' ');
    mbstowcs(&wideString[0], str, length);
    return wideString;
}

void CharacterMaxPatch(HANDLE processHandle, LPVOID address, BYTE newValue)
{
    SIZE_T bytesWritten = 0;
    BOOL success = WriteProcessMemory(processHandle, address, &newValue, sizeof(BYTE), &bytesWritten);
    if (success && bytesWritten == sizeof(BYTE))
    {
        if (debug)
        {
            std::cout << "Debug: CharacterMax patched successfully." << std::endl;
            logFile << "Debug: CharacterMax patched successfully" << std::endl;
        }
    }
    else
    {
        if (debug)
        {
            std::cout << "Debug: Failed to patch CharacterMax value." << std::endl;
            logFile << "Debug: Failed to patch CharacterMax value." << std::endl;
        }
    }
}

void Patches(const std::wstring& processName)
{
    while (true)
    {
        if (GetIniValue(L"Debug", L"debug_patches") == L"True")
        {
            debug = true;
        }

        int newCharaseleLimit = std::stoi(GetIniValue(L"Patches", L"new_charasele_limit"));

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
            if (debug)
            {
                std::cout << "Debug: Process not found, retrying..." << std::endl;
                logFile << "Debug: Process not found, retrying..." << std::endl;
            }
            continue;
        }

        HANDLE processHandle = OpenProcess(PROCESS_ALL_ACCESS, FALSE, processId);
        if (processHandle == NULL)
        {
            if (debug)
            {
                std::cout << "Debug: Failed to open process... Retrying" << std::endl;
                logFile << "Debug: Failed to open process... Retrying" << std::endl;
            }
            continue;
        }

        SIZE_T bytesRead = 0;
        MEMORY_BASIC_INFORMATION memoryInfo = { 0 };
        LPVOID address = NULL;

        while (VirtualQueryEx(processHandle, address, &memoryInfo, sizeof(memoryInfo)) != 0)
        {
            if (memoryInfo.State == MEM_COMMIT && (memoryInfo.Type == MEM_MAPPED || memoryInfo.Type == MEM_PRIVATE))
            {
                const SIZE_T bufferSize = memoryInfo.RegionSize;
                std::wstring buffer(bufferSize / sizeof(wchar_t), L'\0');

                if (ReadProcessMemory(processHandle, memoryInfo.BaseAddress, &buffer[0], bufferSize, &bytesRead) && bytesRead > 0)
                {
                    if (buffer.find(L"CharacterMax") != std::wstring::npos)
                    {
                        // Found the character limit, patch the value
                        BYTE newValue = static_cast<BYTE>(newCharaseleLimit);
                        CharacterMaxPatch(processHandle, memoryInfo.BaseAddress, newValue);
                        break;
                    }
                }
            }
            address = reinterpret_cast<LPVOID>(reinterpret_cast<BYTE*>(address) + memoryInfo.RegionSize);
        }
    }
}

void CheckIfGameRunning(const std::wstring& processName)
{
    // Get the process ID of the game
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

    // If the process ID is not 0, then the game is running
    if (processId == 0)
    {
        std::cout << "Game is not running: " << std::endl;
        logFile << "Game is not running: " << std::endl;
    }
}

void LaunchGame()
{
    std::wstring processName = GetIniValue(L"Game", L"process_name");

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

    // Debugging statements
    if (debug)
    {
        std::cout << "Debug: processId: " << processId << std::endl;
        logFile << "Debug: processId: " << processId << std::endl;
    }

    while (true)
    {
        if (processId == 0)
        {
            // Game process is not running, launch it
            STARTUPINFOW startupInfo = { sizeof(STARTUPINFOW) };
            PROCESS_INFORMATION processInfo;
            BOOL success = CreateProcessW(processName.c_str(), nullptr, nullptr, nullptr, FALSE, 0, nullptr, nullptr, &startupInfo, &processInfo);

            // Debugging statements
            if (debug)
            {
                std::cout << "Debug: CreateProcessW returned: " << success << std::endl;
                logFile << "Debug: CreateProcessW returned: " << success << std::endl;
            }

            if (success)
            {
                CloseHandle(processInfo.hThread);
                WaitForSingleObject(processInfo.hProcess, INFINITE);
                CloseHandle(processInfo.hProcess);

                // Game process has exited and relaunched, reattach the patcher
                Patches(processName);
            }
            else
            {
                if (debug)
                {
                    std::cout << L"Failed to launch the game." << "error" << std::endl;
                    logFile << L"Failed to launch the game." << "error" << std::endl;
                }
            }
        }
        else
        {
            Patches(processName);
            break;
        }
    }
}

int main()
{
    std::wstring processName = GetIniValue(L"Game", L"process_name");
    debug = false;
    logFile.open(LOG_FILE_NAME);

    bool gameRunning = false;

    while (true)
    {
        if (!gameRunning)
        {
            LaunchGame();
            gameRunning = true;
        }

        CheckIfGameRunning(processName);
    }

    logFile.close();
    return 0;
}
