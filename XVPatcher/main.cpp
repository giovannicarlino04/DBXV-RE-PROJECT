#include <windows.h>
#include <MinHook.h>
#include <vector>
#include <string>
#include <cstdint>
#include <filesystem>

#include "Mutex.h"
#include "xvpatcher.h"

static HMODULE patched_dll;
static Mutex mutex;
HMODULE myself;
std::string myself_path;

// Function to normalize a file path
std::string NormalizePath(const std::string& path) {
    std::filesystem::path normalizedPath = std::filesystem::absolute(path);
    return normalizedPath.string();
}

// Function to hook a function
void Hook(void* myFunc, void* detour, void* origFunc) {
    DWORD oldProtect;
    VirtualProtect(origFunc, 5, PAGE_EXECUTE_READWRITE, &oldProtect);
    DWORD offset = reinterpret_cast<DWORD>(detour) - reinterpret_cast<DWORD>(origFunc) - 5;
    *reinterpret_cast<BYTE*>(origFunc) = 0xE9;
    *reinterpret_cast<DWORD*>(reinterpret_cast<DWORD>(origFunc) + 1) = offset;
    VirtualProtect(origFunc, 5, oldProtect, &oldProtect);
}

// Function to check if a file exists
bool FileExists(const std::string& filePath) {
    DWORD fileAttributes = GetFileAttributesA(filePath.c_str());
    if (fileAttributes == INVALID_FILE_ATTRIBUTES) {
        if (GetLastError() == ERROR_FILE_NOT_FOUND || GetLastError() == ERROR_PATH_NOT_FOUND) {
            return false;
        }
    }
    return true;
}

extern "C"
{
	PUBLIC DWORD XInputGetState()
	{
		return ERROR_DEVICE_NOT_CONNECTED;
	}
	
	PUBLIC DWORD XInputSetState()
	{
		return ERROR_DEVICE_NOT_CONNECTED;
	}
	
	PUBLIC DWORD XInputGetBatteryInformation(DWORD,  BYTE, void *)
	{
		return ERROR_DEVICE_NOT_CONNECTED;
	}
	
	PUBLIC void XInputEnable(BOOL)
	{
	}
	
	PUBLIC DWORD XInputGetCapabilities(DWORD, DWORD, void *)
	{
		return ERROR_DEVICE_NOT_CONNECTED;
	}
	
	PUBLIC DWORD XInputGetDSoundAudioDeviceGuids(DWORD, void *, void *)
	{
		return ERROR_DEVICE_NOT_CONNECTED;
	}
	
	PUBLIC DWORD XInputGetKeystroke(DWORD, DWORD, void *)
	{
		return ERROR_DEVICE_NOT_CONNECTED;
	}
	
	PUBLIC DWORD XInputGetStateEx(DWORD, void *)
	{
		return ERROR_DEVICE_NOT_CONNECTED;
	}
	
	PUBLIC DWORD XInputWaitForGuideButton(DWORD, DWORD, void *)
	{
		return ERROR_DEVICE_NOT_CONNECTED;
	}
	
	PUBLIC DWORD XInputCancelGuideButtonWait()
	{
		return ERROR_DEVICE_NOT_CONNECTED;
	}
	
	PUBLIC DWORD XInputPowerOffController()
	{
		return ERROR_DEVICE_NOT_CONNECTED;
	}
}

static BOOL InGameProcess(VOID)
{
	char szPath[MAX_PATH];
	
	GetModuleFileNameW(NULL, reinterpret_cast<LPWSTR>(szPath), MAX_PATH);
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
	
	myself = GetModuleHandleW(L"xinput1_3.dll");

	GetModuleFileNameA(myself, mod_path, sizeof(mod_path));
	
	myself_path = NormalizePath(mod_path);
	myself_path = myself_path.substr(0, myself_path.rfind('/')+1);	
	
	if (FileExists(myself_path+"xinput_other.dll"))
	{
		strncpy(original_path, myself_path.c_str(), sizeof(original_path));
		strncat(original_path, "xinput_other.dll", sizeof(original_path));
	}
	else
	{	
		if (GetSystemDirectoryW(reinterpret_cast<LPWSTR>(original_path), sizeof(original_path)) == 0)
			return false;
		strncat(original_path, "\\xinput1_3.dll", sizeof(original_path));
	}
	
	patched_dll = LoadLibraryW(reinterpret_cast<LPWSTR>(original_path));		
	if (!patched_dll)
	{
		return false;
	}
	
		
	for (auto &export_name : exports)
	{
		uint64_t ordinal = (uint64_t)export_name;
		
		uint8_t *orig_func = (uint8_t *)GetProcAddress(patched_dll, export_name);
		
		if (!orig_func)
		{
			if (ordinal < 0x1000)			
			{
				continue;		
			}
			else
			{		
				return false;	
			}
		}
		uint8_t *my_func = (uint8_t *)GetProcAddress(myself, export_name);
		Hook(my_func, nullptr, orig_func);
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

VOID WINAPI GetStartupInfoW_Patched(LPSTARTUPINFOW lpStartupInfo)
{
	static bool started = false;
	
	// This function is only called once by the game but... just in case
	if (!started)
	{	
		if (!load_dll(false))
			exit(-1);
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
    switch (fdwReason)
    {
    case DLL_PROCESS_ATTACH:
    {

        if (InGameProcess())
        {
   
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

