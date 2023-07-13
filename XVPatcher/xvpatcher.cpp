#include <iostream>
#include <string>
#include <Windows.h>
#include <TlHelp32.h>
#include <WinINet.h>

#pragma comment(lib, "wininet.lib")

#define PROCESS_NAME L"DBXV.exe"
#define OLD_CHARASELE_LIMIT 64;
#define INI_FILE_NAME L"XVPatcher/XVPatcher.ini"

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
    while(true){
        SIZE_T bytesWritten = 0;
        BOOL success = WriteProcessMemory(processHandle, address, &newValue, sizeof(BYTE), &bytesWritten);
        if (success && bytesWritten == sizeof(BYTE))
        {
            std::cout << "CharacterMax patched successfully." << std::endl;
            break;
        }
        else
        {
            std::cout << "Failed to patch CharacterMax value." << std::endl;
        }
    }
}

void Patches(const std::wstring& processName)
{
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
                    LPVOID CharacterMaxaddress = reinterpret_cast<LPVOID>(0x3BF8E756);
                    // Define the new value to set
                    std::wstring newCharaseleLimitStr = GetIniValue(L"Patches", L"new_charasele_limit");
                    BYTE newValue = static_cast<BYTE>(std::stoi(newCharaseleLimitStr));
                    CharacterMaxPatch(processHandle, CharacterMaxaddress, newValue);
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
    std::wstring processName = GetIniValue(L"Game", L"process_name");

    STARTUPINFOW startupInfo = { sizeof(STARTUPINFOW) };
    PROCESS_INFORMATION processInfo;
    BOOL success = CreateProcessW(processName.c_str(), nullptr, nullptr, nullptr, FALSE, 0, nullptr, nullptr, &startupInfo, &processInfo);

    if (success)
    {
        CloseHandle(processInfo.hThread);
        WaitForSingleObject(processInfo.hProcess, INFINITE);
        CloseHandle(processInfo.hProcess);

        //THIS IS ESSENTIAL, OR THE PATCHER WON'T WORK
        Sleep(5000);

        // Game process has exited and relaunched, reattach the patcher
        Patches(processName);
    }
    else
    {
        MessageBoxW(nullptr, L"Failed to launch the game.", L"Error", MB_OK | MB_ICONERROR);
    }
}

int main()
{
    // Write default values to the INI file if it doesn't exist
    if (GetFileAttributesW(INI_FILE_NAME) == INVALID_FILE_ATTRIBUTES)
    {
        WriteIniValue(L"Game", L"process_name", L"DBXV.exe");
        WriteIniValue(L"Patches", L"new_charasele_limit", L"99");
    }

    LaunchGame();

    return 0;
}