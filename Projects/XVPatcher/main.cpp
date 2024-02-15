#define _WIN32_WINNT	0x600

#include <windows.h>
#include <winbase.h>
#include <stdio.h>
#include <io.h>
#include <stdint.h>
#include <string>
#include <iostream>
#include <fstream>
#include <map>
#include <cstdio>
#include <algorithm>
#include <memory>
#include <thread>

#include "patch.h"
#include "debug.h"
#include "symbols.h"
#include "CpkFile.h"
#include "Mutex.h"
#include "Utils.h"
#include "PatchUtils.h"
#include "IggyFile.h"
#include "CPK.hpp"
#include "XVPatcher.h"
#include "custom.hpp"

static HMODULE patched_dll;
static Mutex mutex;
HMODULE myself;
std::string myself_path;

void iggy_trace_callback(void *, void *, const char *str, size_t)
{
	if (str && strcmp(str, "\n") == 0)
		return;
	
	CustomDPRINTF("IGGY: %s\n", str);
}
static void IggySetTraceCallbackUTF8Patched(void *, void *param)
{
	HMODULE iggy = GetModuleHandle("iggy_w32.dll");
	if (!iggy)
		return;
	
	IGGYSetTraceCallbackType func = (IGGYSetTraceCallbackType)GetProcAddress(iggy, "_IggySetTraceCallbackUTF8@8");
	
	if (func)
		func((void *)iggy_trace_callback, param);
}

extern "C"
{
	PUBLIC DWORD XInputGetState()
	{
		CustomDPRINTF("%s: ****************I have been called but I shouldn't!!!\n", FUNCNAME);
		return ERROR_DEVICE_NOT_CONNECTED;
	}
	
	PUBLIC DWORD XInputSetState()
	{
		CustomDPRINTF("%s ****************I have been called but I shouldn't!!!\n", FUNCNAME);
		return ERROR_DEVICE_NOT_CONNECTED;
	}
	
	PUBLIC DWORD XInputGetBatteryInformation(DWORD,  BYTE, void *)
	{
		CustomDPRINTF("%s: ****************I have been called but I shouldn't!!!\n", FUNCNAME);
		return ERROR_DEVICE_NOT_CONNECTED;
	}
	
	PUBLIC void XInputEnable(BOOL)
	{
		CustomDPRINTF("%s: ****************I have been called but I shouldn't!!!\n", FUNCNAME);
	}
	
	PUBLIC DWORD XInputGetCapabilities(DWORD, DWORD, void *)
	{
		CustomDPRINTF("%s: ****************I have been called but I shouldn't!!!\n", FUNCNAME);
		return ERROR_DEVICE_NOT_CONNECTED;
	}
	
	PUBLIC DWORD XInputGetDSoundAudioDeviceGuids(DWORD, void *, void *)
	{
		CustomDPRINTF("%s: ****************I have been called but I shouldn't!!!\n", FUNCNAME);
		return ERROR_DEVICE_NOT_CONNECTED;
	}
	
	PUBLIC DWORD XInputGetKeystroke(DWORD, DWORD, void *)
	{
		CustomDPRINTF("%s: ****************I have been called but I shouldn't!!!\n", FUNCNAME);
		return ERROR_DEVICE_NOT_CONNECTED;
	}
	
	PUBLIC DWORD XInputGetStateEx(DWORD, void *)
	{
		CustomDPRINTF("%s: ****************I have been called but I shouldn't!!!\n", FUNCNAME);
		return ERROR_DEVICE_NOT_CONNECTED;
	}
	
	PUBLIC DWORD XInputWaitForGuideButton(DWORD, DWORD, void *)
	{
		CustomDPRINTF("%s: ****************I have been called but I shouldn't!!!\n", FUNCNAME);
		return ERROR_DEVICE_NOT_CONNECTED;
	}
	
	PUBLIC DWORD XInputCancelGuideButtonWait()
	{
		CustomDPRINTF("%s: ****************I have been called but I shouldn't!!!\n", FUNCNAME);
		return ERROR_DEVICE_NOT_CONNECTED;
	}
	
	PUBLIC DWORD XInputPowerOffController()
	{
		CustomDPRINTF("%s: ****************I have been called but I shouldn't!!!\n", FUNCNAME);
		return ERROR_DEVICE_NOT_CONNECTED;
	}
}

static BOOL InGameProcess(VOID)
{
	char szPath[MAX_PATH];
	
	GetModuleFileName(NULL, szPath, MAX_PATH);
	_strlwr(szPath);
	
	// A very poor aproach, I know
	return (strstr(szPath, PROCESS_NAME) != NULL);
}

static bool load_dll(bool critical)
{
	static const std::vector<const char *> exports =
	{
		"XInputGetState",
		"XInputSetState",
		"XInputGetBatteryInformation",
		"XInputEnable",
		"XInputGetCapabilities",
		"XInputGetDSoundAudioDeviceGuids",
		"XInputGetKeystroke",
		(char *)100,
		(char *)101,
		(char *)102,
		(char *)103
	};	
	
	static char mod_path[2048];
	static char original_path[256];	
	static bool loaded = false;
	
	MutexLocker lock(&mutex);
	
	if (loaded)
		return true;
	
	myself = GetModuleHandle("xinput1_3.dll");

	GetModuleFileNameA(myself, mod_path, sizeof(mod_path));
	
	myself_path = Utils::NormalizePath(mod_path);
	myself_path = myself_path.substr(0, myself_path.rfind('/')+1);	
	CustomDPRINTF("Myself path = %s\n", myself_path.c_str());
	
	if (Utils::FileExists(myself_path+"xinput_other.dll"))
	{
		strncpy(original_path, myself_path.c_str(), sizeof(original_path));
		strncat(original_path, "xinput_other.dll", sizeof(original_path));
	}
	else
	{	
		if (GetSystemDirectory(original_path, sizeof(original_path)) == 0)
			return false;
		strncat(original_path, "\\xinput1_3.dll", sizeof(original_path));
	}
	
	patched_dll = LoadLibrary(original_path);		
	if (!patched_dll)
	{
		if (critical)
			UPRINTF("Cannot load original DLL (%s).\nLoadLibrary failed with error %u\n", original_path, (unsigned int)GetLastError());
				
		return false;
	}
	
	CustomDPRINTF("original DLL path: %s   base=%p\n", original_path, patched_dll);
		
	for (auto &export_name : exports)
	{
		uint64_t ordinal = (uint64_t)export_name;
		
		uint8_t *orig_func = (uint8_t *)GetProcAddress(patched_dll, export_name);
		
		if (!orig_func)
		{
			if (ordinal < 0x1000)			
			{
				CustomDPRINTF("Warning: ordinal function %I64d doesn't exist in this system.\n", ordinal);
				continue;		
			}
			else
			{
				if (Utils::IsWine())
				{
					CustomDPRINTF("Failed to get original function: %s --- ignoring error because running under wine.\n", export_name);
					continue;
				}
				else
				{
					UPRINTF("Failed to get original function: %s\n", export_name);			
					return false;
				}
			}
		}
		uint8_t *my_func = (uint8_t *)GetProcAddress(myself, export_name);		
		
		if (!my_func)
		{
			if (ordinal < 0x1000)			
				UPRINTF("Failed to get my function: %I64d\n", ordinal);
			else
				UPRINTF("Failed to get my function: %s\n", export_name);
			
			return false;
		}
		
		if (ordinal < 0x1000)
			CustomDPRINTF("%I64d: address of microsoft: %p, address of mine: %p\n", ordinal, orig_func, my_func);
		else
			CustomDPRINTF("%s: address of microsoft: %p, address of mine: %p\n", export_name, orig_func, my_func);
		
		CustomDPRINTF("Content of microsoft func: %02X%02X%02X%02X%02X\n", orig_func[0], orig_func[1], orig_func[2], orig_func[3], orig_func[4]);
		CustomDPRINTF("Content of my func: %02X%02X%02X%02X%02X\n", my_func[0], my_func[1], my_func[2], my_func[3], my_func[4]);
		
		PatchUtils::Hook(my_func, nullptr, orig_func);
		CustomDPRINTF("Content of my func after patch: %02X%02X%02X%02X%02X\n", my_func[0], my_func[1], my_func[2], my_func[3], my_func[4]);
	}
	loaded = true;
	return true;
}

static void unload_dll()
{
	if (patched_dll)
	{
		FreeLibrary((HMODULE)patched_dll);
		patched_dll = nullptr;
	}
}
// Function to get the last error message
std::string GetLastErrorAsString() {
    DWORD errorMessageID = GetLastError();
    if (errorMessageID == 0) {
        return std::string(); // No error message
    }

    LPVOID messageBuffer = nullptr;
    size_t size = FormatMessageA(
        FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
        NULL, errorMessageID, 0, (LPSTR)&messageBuffer, 0, NULL);

    std::string message(static_cast<char*>(messageBuffer), size);

    LocalFree(messageBuffer);

    return message;
}

uintptr_t GetModuleBaseAddress(const wchar_t* modName) {
    HMODULE hModule = GetModuleHandleW(modName);
    if (hModule != NULL) {
        return (uintptr_t)hModule; // Return the base address of the module
    } else {
        return 0; // Module not found
    }
}

void CheckVersion(){
	LPCWSTR my_dll_name = L"xinput1_3.dll";
	CustomDPRINTF("XVPATCHER VERSION " XVPATCHER_VERSION ". Exe base = %p. My Dll base = %p. My dll name: %s\n", GetModuleHandle(NULL), GetModuleHandleW(my_dll_name), my_dll_name);	
	
	float version = Utils::GetExeVersion(myself_path+EXE_PATH);
	CustomDPRINTF("Running on game version %.3f\n", version);
	
	if (version != 0.0 && version < (MINIMUM_GAME_VERSION - 0.00001))
	{
		UPRINTF("This game version (%.3f) is not compatible with this version of the patcher.\nMin version required is: %.3f\n", version, MINIMUM_GAME_VERSION);
		exit(-1);
	}
}

bool CMSPatches(HANDLE hProcess, uintptr_t moduleBaseAddress) {
   	const char* newBytes1 = "\x7F\x7C\x09\xB8\x00";  // CMS Patch 1  //7F 7C 09 B8 00
    const char* newBytes2 = "\x70\x7D\x6E\xC7\x45";  // CMS Patch 2  //70 7D 6E C7 45

	LPVOID address1 = nullptr;
    LPVOID address2 = nullptr;
    SIZE_T numberOfBytesWritten;
    DWORD oldProtect;

    if (moduleBaseAddress != 0) {
        address1 = (LPVOID)(moduleBaseAddress + 0x15EE39);
        address2 = (LPVOID)(moduleBaseAddress + 0x19363A);
    }
	else{
		UPRINTF("ModuleBaseAddress = 0");
	}

    /////////////// PATCHES GO HERE ///////////////

    // CMS Patch 1
    if (address1 == nullptr) {
        UPRINTF("Failed to calculate the address.\n");
    }

    if (WriteProcessMemory(hProcess, address1, newBytes1, strlen(newBytes1), &numberOfBytesWritten)) {
        CustomDPRINTF("Successfully applied CMS Patch n.1\n");
    }
    else {
        UPRINTF("Failed to replace the bytes.\n");
    }

    // CMS Patch 2
    if (address2 == nullptr) {
        UPRINTF("Failed to calculate the address.\n");
    }

    if (WriteProcessMemory(hProcess, address2, newBytes2, strlen(newBytes2), &numberOfBytesWritten)) {
        CustomDPRINTF("Successfully applied CMS Patch n.2\n");
    }
    else {
        UPRINTF("Failed to replace the bytes.\n");
    }
	return 0;
}

bool VersionStringPatch(HANDLE hProcess, uintptr_t moduleBaseAddress) {
	const BYTE patchsize = wcslen(L"\x76\x65\x72\x2e\x31\x2e\x30\x38\x2e\x30\x30") * sizeof(wchar_t);
    const wchar_t* newBytes1 = L"\x58\x56\x50\x61\x74\x63\x68\x65\x72";

    LPVOID address1 = nullptr;
    SIZE_T numberOfBytesWritten;
    DWORD oldProtect;
    DWORD newProtect = PAGE_EXECUTE_READWRITE;

    if (moduleBaseAddress != 0) {
        address1 = (LPVOID)(moduleBaseAddress + 0x11ACFB4);
    }

    ////////////////// PATCHES GO HERE ///////////////

    // Version String Patch 1
    if (address1 == nullptr) {
        UPRINTF("Failed to calculate the address.\n");
        return false;
    }

    if (!VirtualProtect(address1, patchsize, newProtect, &oldProtect)) {
        UPRINTF("Failed to change memory protection.\n");
        return false;
    }

    if (!WriteProcessMemory(hProcess, address1, newBytes1, patchsize, &numberOfBytesWritten)) {
        UPRINTF("Failed to replace the bytes.\n");
        return false;
    }

    if (!VirtualProtect(address1, patchsize, oldProtect, &newProtect)) {
        UPRINTF("Failed to restore memory protection.\n");
        return false;
    }

    CustomDPRINTF("Successfully applied Version String Patch\n");
    return true;
}

VOID WINAPI GetStartupInfoW_Patched(LPSTARTUPINFOW lpStartupInfo)
{
	static bool started = false;
	
	// This function is only called once by the game but... just in case
	if (!started)
	{	
		if (!load_dll(false))
			exit(-1);

		bool iggy_debug = true;
		if (iggy_debug)
		{
			if (!PatchUtils::HookImport("iggy_w32.dll", "_IggySetTraceCallbackUTF8@8", (void *)IggySetTraceCallbackUTF8Patched))
			{
				CustomDPRINTF("Failed to hook import of _IggySetTraceCallbackUTF8@8.\n");						
			}
		}
	}	
	GetStartupInfoW(lpStartupInfo);
}

DWORD WINAPI StartThread(LPVOID)
{
	load_dll(false);
	return 0;
}

extern "C" BOOL EXPORT DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved)
{
    HANDLE consoleHandle = nullptr;
    std::map<std::string, std::string> iniValues = readIniFile(INI_FILE);

    // This patch MUST go here, it's not optional!!!
    std::string keyToCheck = "Debug.Debug_Console";
    if (iniValues.find(keyToCheck) != iniValues.end() && iniValues[keyToCheck] == "True")
    {
        // Allocate a console for the application
        AllocConsole();

        // Get handle to the console output
        consoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);
        if (consoleHandle == INVALID_HANDLE_VALUE)
        {
            // Handle error, if needed
            return 1;
        }

        // Redirect standard output to the console
        freopen("CONOUT$", "w", stdout);
        freopen("CONOUT$", "w", stderr);
    }
    else
    {
        CustomDPRINTF("Console Debugging is not enabled or the key is not present.\n");
    }
    ////////////////////////////////////////////////

    switch (fdwReason)
    {
    case DLL_PROCESS_ATTACH:
    {

        if (InGameProcess())
        {
            HANDLE hProcess = GetCurrentProcess();
            uintptr_t moduleBaseAddress = GetModuleBaseAddress(L"DBXV.exe");

            if (!load_dll(false))
                return FALSE;

            // PATCHES THAT DON'T REQUIRE INI FILE GO HERE
            CheckVersion();
            /////////////////////////////////////////////

            // PATCHES THAT REQUIRE INI FILE GO HERE
            std::string keyToCheck = "Patches.CMS_Patches";
            if (iniValues.find(keyToCheck) != iniValues.end() && iniValues[keyToCheck] == "True")
            {
                CustomDPRINTF("CMS Patches are enabled.\n");
                CMSPatches(hProcess, moduleBaseAddress);
            }
            else
            {
                CustomDPRINTF("CMS Patches are not enabled or the key is not present.\n");
            }

            keyToCheck = "Patches.Version_String_Patch";
            if (iniValues.find(keyToCheck) != iniValues.end() && iniValues[keyToCheck] == "True")
            {
                CustomDPRINTF("Version String Patch is enabled.\n");
                VersionStringPatch(hProcess, moduleBaseAddress);
            }
            else
            {
                CustomDPRINTF("Version String Patch is not enabled or the key is not present.\n");
            }

            keyToCheck = "Patches.CPK_Patch";
            if (iniValues.find(keyToCheck) != iniValues.end() && iniValues[keyToCheck] == "True")
            {
                CustomDPRINTF("CPK Patch is enabled.\n");
                CpkFile *data, *data2, *datap1, *datap2, *datap3;

                if (get_cpk_tocs(&data, &data2, &datap1, &datap2, &datap3))
                {
                    patch_toc(data);
                    patch_toc(data2);
                    patch_toc(datap1);
                    patch_toc(datap2);
                    patch_toc(datap3);

                    data->RevertEncryption(false);
                    data2->RevertEncryption(false);
                    datap1->RevertEncryption(false);
                    datap2->RevertEncryption(false);
                    datap3->RevertEncryption(false);

                    patches();

                    delete data;
                    delete data2;
                    delete datap1;
                    delete datap2;
                    delete datap3;
                }
            }
            else
            {
                CustomDPRINTF("CPK Patch is not enabled or the key is not present.\n");
            }
            /////////////////////////////////////////////

            if (!PatchUtils::HookImport("KERNEL32.dll", "GetStartupInfoW", (void *)GetStartupInfoW_Patched))
            {
                UPRINTF("GetStartupInfoW hook failed.\n");
                return TRUE;
            }
        }
        break;
    }

    case DLL_PROCESS_DETACH:
    {
        if (!lpvReserved)
        {
            FreeConsole();
            unload_dll();
        }
    }
    break;
    }

    return TRUE;
}

