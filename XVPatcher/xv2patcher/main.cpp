#include <windows.h>
#include "IniFile.h"
#include "EPatchFile.h"
#include "PatchUtils.h"
#include "xv2patcher.h"
#include "debug.h"

static HMODULE patched_dll;
std::string myself_path;
IniFile ini;

static bool in_game_process()
{
	char szPath[MAX_PATH];
	
	GetModuleFileName(NULL, szPath, MAX_PATH);
	_strlwr(szPath);
	
	// A very poor aproach, I know
	return (strstr(szPath, PROCESS_NAME) != NULL);
}

static bool load_dll()
{
	static const std::vector<const char *> exports =
	{
		"XInputGetState",
		"XInputSetState",
		"XInputGetBatteryInformation",
		"XInputEnable",
		"XInputGetCapabilities",
		"XInputGetDSoundAudioDeviceGuids",
		"XInputGetKeystroke"
	};	
	
	static char mod_path[2048];
	static char original_path[256];	
	 	
	HMODULE myself = GetModuleHandle("xinput1_3.dll");
	GetModuleFileNameA(myself, mod_path, sizeof(mod_path));
	
	myself_path = Utils::NormalizePath(mod_path);
	myself_path = myself_path.substr(0, myself_path.rfind('/')+1);	
	DPRINTF("Myself path = %s\n", myself_path.c_str());
	
	if (GetSystemDirectory(original_path, sizeof(original_path)) == 0)
		return false;
	
	strncat(original_path, "\\xinput1_3.dll", sizeof(original_path));	
	patched_dll = LoadLibrary(original_path);		
	if (!patched_dll)
	{
		UPRINTF("Cannot load original DLL\n");
		return false;
	}	
		
	for (auto export_name : exports)
	{
		void *orig_func = (void *)GetProcAddress(patched_dll, export_name);
		if (!orig_func)
		{
			UPRINTF("Failed to get original function: %s\n", export_name);
			return false;
		}
		
		uint8_t *my_func = (uint8_t *)GetProcAddress(myself, export_name);
		if (*my_func != 0x48) // Not what we expected
		{
			DPRINTF("Eeer hmmm, seems like this is not the real address of my function (%s->%p   %02X%02X%02X%02X).\n", export_name, my_func, my_func[0], my_func[1], my_func[2], my_func[3]);
		}
		
		PatchUtils::Write64(my_func+2, (uint64_t)orig_func);	// my func+2 = the operand part in mov rax, 0x0123456789ABCDEF 
	}
	
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

static bool load_patch_file(const std::string &path, bool is_directory, void *custom_param)
{
	if (Utils::EndsWith(path, ".xml", false))
	{
		EPatchFile epf(myself_path+"xinput1_3.dll");
		
		int enabled;
		bool enabled_b;
		std::string setting;
		
		if (!epf.CompileFromFile(path))
		{
			UPRINTF("Failed to compile file \"%s\"\n", path.c_str());
			exit(-1);
		}
		
		//DPRINTF("File %s compiled.\n", path.c_str());		
		if ((enabled = epf.GetEnabled(setting)) < 0)
		{	
			ini.GetBooleanValue("Patches", setting, &enabled_b, false);
			enabled = enabled_b;
		}
		
		if (!enabled)
			return true;
		
		for (EPatch &patch : epf)
		{
			if ((enabled = patch.GetEnabled(setting)) < 0)
			{
				ini.GetBooleanValue("Patches", setting, &enabled_b, false);
				enabled = enabled_b;
			}
			
			if (!enabled)
				continue;
			
			if (!patch.Apply())
			{
				UPRINTF("Failed to apply patch \"%s:%s\"\n", epf.GetName().c_str(), patch.GetName().c_str());
				exit(-1);
			}
		}
	}	
	
	return true;
}

static void load_patches()
{
	Utils::VisitDirectory(myself_path+PATCHES_PATH, true, false, false, load_patch_file);
}

static void start()
{
	DPRINTF("Hello world. Exe base = %p. My Dll base = %p.\n", GetModuleHandle(NULL), GetModuleHandle("xinput1_3.dll"));	
	/*static char cwd_buf[2048];	
	
	GetCurrentDirectory(sizeof(cwd_buf), cwd_buf);		
	DPRINTF("Cwd = %s\n", cwd_buf);		*/	
	load_patches();				
}

VOID WINAPI GetStartupInfoW_Patched(LPSTARTUPINFOW lpStartupInfo)
{
	static bool started = false;
	
	// This function is only called once by the game but... just in case
	if (!started)
	{	
		start();
		started = true;
	}
	
	GetStartupInfoW(lpStartupInfo);
}

extern "C" BOOL WINAPI EXPORT DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved)
{
	switch (fdwReason)
	{
		case DLL_PROCESS_ATTACH:
		
			set_debug_level(1); 						
								
			if (in_game_process())
			{					
				int default_log_level;
								
				if (!load_dll())
					exit(-1);
				
				ini.LoadFromFile(myself_path+INI_PATH);
				ini.GetIntegerValue("General", "default_log_level", &default_log_level, 1);
				set_debug_level(default_log_level);	

				if (!PatchUtils::HookImport("KERNEL32.dll", "GetStartupInfoW", (void *)GetStartupInfoW_Patched))
				{
					DPRINTF("GetStartupInfoW hook failed.\n");
					return TRUE;
				}
			}			
			
		break;
		
		case DLL_PROCESS_DETACH:		
			
			if (!lpvReserved)
				unload_dll();
			
		break;
	}
	
	return TRUE;
}
