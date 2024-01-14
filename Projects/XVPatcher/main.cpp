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
#include "XVPatcher.h"

uint8_t (* __thiscall cpk_file_exists)(void *, char *);

static uint64_t data_toc_offset, data_toc_size;
static uint64_t data2_toc_offset, data2_toc_size;
static uint64_t datap1_toc_offset, datap1_toc_size;
static uint64_t datap2_toc_offset, datap2_toc_size;
static uint64_t datap3_toc_offset, datap3_toc_size;

static uint8_t *data_toc, *data2_toc, *datap1_toc, *datap2_toc, *datap3_toc;
static uint8_t *data_hdr, *data2_hdr, *datap1_hdr, *datap2_hdr, *datap3_hdr;
static void **readfile_import;
static void *original_readfile;
static HMODULE patched_dll;
static Mutex mutex;
HMODULE myself;
std::string myself_path;

// Custom implementation of DPRINTF to redirect output to console
void CustomDPRINTF(const char* format, ...) {
    // Format the variable arguments using vsprintf and print to std::cout
    va_list args;
    va_start(args, format);

    // Format the output
    char buffer[512]; // Adjust buffer size as needed
    vsnprintf(buffer, sizeof(buffer), format, args);

    // Output the formatted string to console
    std::cout << "Debug: " << buffer << std::endl;

    va_end(args);
}

std::map<std::string, std::string> readIniFile(const std::string& filename) {
    std::map<std::string, std::string> iniValues;

    std::ifstream file(filename);

    if (!file.is_open()) {
        UPRINTF("Error opening ini file, please install the patcher correctly.");
        return;
    }

    std::string line;
    std::string currentSection;

    while (std::getline(file, line)) {
        // Remove leading and trailing whitespaces
        line.erase(0, line.find_first_not_of(" \t\r\n"));
        line.erase(line.find_last_not_of(" \t\r\n") + 1);

        if (line.empty() || line[0] == ';') {
            // Skip empty lines or comments starting with ';'
            continue;
        } else if (line[0] == '[' && line[line.length() - 1] == ']') {
            // New section
            currentSection = line.substr(1, line.length() - 2);
        } else {
            // Key-value pair
            size_t separatorPos = line.find('=');

            if (separatorPos != std::string::npos) {
                std::string key = line.substr(0, separatorPos);
                std::string value = line.substr(separatorPos + 1);

                // Remove leading and trailing whitespaces from key and value
                key.erase(0, key.find_first_not_of(" \t\r\n"));
                key.erase(key.find_last_not_of(" \t\r\n") + 1);
                value.erase(0, value.find_first_not_of(" \t\r\n"));
                value.erase(value.find_last_not_of(" \t\r\n") + 1);

                iniValues[currentSection + "." + key] = value;
            } else {
                std::cerr << "Invalid line in INI file: " << line << std::endl;
            }
        }
    }

    file.close();

    return iniValues;
}

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
static uint8_t *read_file_from(const char *file,  uint64_t offset, unsigned int *psize)
{
	HANDLE hFile;
	LONG high;
	uint8_t *buf;
	
	// Microsoft and their 5000 parameter functions...
	hFile = CreateFile(	file, 
						GENERIC_READ, 
						FILE_SHARE_READ|FILE_SHARE_WRITE|FILE_SHARE_DELETE, 
						NULL,
						OPEN_EXISTING,
						0,
						NULL);
						
	if (hFile == INVALID_HANDLE_VALUE)
		return NULL;
	
	high = (offset>>32);
	if (SetFilePointer(hFile, offset&0xFFFFFFFF, &high, FILE_BEGIN) == INVALID_SET_FILE_POINTER)
	{
		CloseHandle(hFile);
		return NULL;		
	}
		
	buf = new uint8_t[*psize];
	if (!buf)
	{
		CloseHandle(hFile);
		return NULL;
	}
	
	if (!ReadFile(hFile, buf, *psize, (LPDWORD)psize, NULL))
	{
		delete[] buf;
		CloseHandle(hFile);
		return NULL;
	}
	
	CloseHandle(hFile);						
	return buf;
}

static CpkFile *get_cpk_toc(const char *file, uint64_t *toc_offset, uint64_t *toc_size, uint8_t **hdr_buf, uint8_t **toc_buf)
{
	unsigned rsize;
	bool success = false;
	
	*hdr_buf = NULL;
	*toc_buf = NULL;
	
	rsize = 0x800;
	*hdr_buf = read_file_from(file, 0, &rsize);
	if (!(*hdr_buf))
		return NULL;
	
	CpkFile *cpk = new CpkFile();
	
	if (!cpk->ParseHeaderData(*hdr_buf))
	{
		goto clean;
	}
	
	*toc_offset = cpk->GetTocOffset();
	*toc_size = cpk->GetTocSize();
	
	if (*toc_offset == (uint64_t)-1 || *toc_size == 0)
	{
		goto clean;
	}
	
	rsize = *toc_size;
	*toc_buf = read_file_from(file, *toc_offset, &rsize);
	
	if (!(*toc_buf))
	{
		CustomDPRINTF("read_file_from failed (2)\n");
		delete[] *toc_buf;
		goto clean;
	}
	
	if (rsize != *toc_size)
	{
		CustomDPRINTF("Warning: read size doesn't match requested size.\n");
	}
	
	if (!cpk->ParseTocData(*toc_buf))
	{
		goto clean;
	}	
	
	CustomDPRINTF("This .cpk has %d files.\n", cpk->GetNumFiles());
	success = true;
	
clean:

	if (!success)
	{
		if (*hdr_buf)
			delete[] *hdr_buf;
		
		if (*toc_buf)
			delete[] *toc_buf;
		
		delete cpk;
		cpk = NULL;
	}
	
	return cpk;
}

static bool get_cpk_tocs(CpkFile **data, CpkFile **data2, CpkFile **datap1, CpkFile **datap2, CpkFile **datap3)
{
	*data = get_cpk_toc(DATA_CPK, &data_toc_offset, &data_toc_size, &data_hdr, &data_toc);
	if (!(*data))
		return false;	
	
	*data2 = get_cpk_toc(DATA2_CPK, &data2_toc_offset, &data2_toc_size, &data2_hdr, &data2_toc);
	if (!(*data2))
	{
		delete[] data_toc;
		delete *data;
		return false;
	}
	*datap1 = get_cpk_toc(DATAP1_CPK, &datap1_toc_offset, &datap1_toc_size, &datap1_hdr, &datap1_toc);
	if (!(*datap1))
	{
		delete[] data2_toc;
		delete *data2;
		return false;
	}
	*datap2 = get_cpk_toc(DATAP2_CPK, &datap2_toc_offset, &datap2_toc_size, &datap2_hdr, &datap2_toc);
	if (!(*datap2))
	{
		delete[] datap1_toc;
		delete *datap1;
		return false;
	}
	*datap3 = get_cpk_toc(DATAP3_CPK, &datap3_toc_offset, &datap3_toc_size, &datap3_hdr, &datap3_toc);
	if (!(*datap3))
	{
		delete[] datap2_toc;
		delete *datap2;
		return false;
	}

	CustomDPRINTF("data.cpk.toc = %I64x, size = %I64x\n", data_toc_offset, data_toc_size);
	CustomDPRINTF("data2.cpk.toc = %I64x, size = %I64x\n", data2_toc_offset, data2_toc_size);
	CustomDPRINTF("datap1.cpk.toc = %I64x, size = %I64x\n", datap1_toc_offset, datap1_toc_size);
	CustomDPRINTF("datap2.cpk.toc = %I64x, size = %I64x\n", datap2_toc_offset, datap2_toc_size);
	CustomDPRINTF("datap3.cpk.toc = %I64x, size = %I64x\n", datap3_toc_offset, datap3_toc_size);

	
	return true;
}

static bool local_file_exists( char *path)
{
	HANDLE hFind;
	WIN32_FIND_DATA wfd;
	
	hFind = FindFirstFile(path, &wfd);
	if (hFind == INVALID_HANDLE_VALUE)
		return false;
	
	FindClose(hFind);	
	return true;
}

static bool local_file_exists(FileEntry *entry)
{
	char *path;
		
	path = new char[strlen(entry->dir_name) + strlen(entry->file_name) + 2];
	if (!path)
		return false;
	
	sprintf(path, "%s/%s", entry->dir_name, entry->file_name); // Should be safe... should...
	
	bool ret = local_file_exists(path);
	delete[] path;
	
	return ret;
}

static void patch_toc(CpkFile *cpk)
{
	int count = 0;
	size_t num_files = cpk->GetNumFiles();
	
	for (size_t i = 0; i < num_files; i++)
	{
		FileEntry *file = cpk->GetFileAt(i);
		
		if (local_file_exists(file))
		{
			cpk->UnlinkFileFromDirectory(i);
			count++;
		}
	}
	
	CustomDPRINTF("%d files deleted in RAM.\n", count);
}

static bool IsThisFile(HANDLE hFile, const char *name)
{
	static char path[MAX_PATH+1];
	
	memset(path, 0, sizeof(path));
	
	if (GetFinalPathNameByHandle(hFile, path, sizeof(path)-1, FILE_NAME_NORMALIZED) != 0)
	{
		_strlwr(path);
		return (strstr(path, name) != NULL);
	}
	
	return false;
}

static uint64_t GetFilePointer(HANDLE hFile)
{
	LONG high = 0;	
	DWORD ret = SetFilePointer(hFile, 0, &high, FILE_CURRENT);
	
	if (ret == INVALID_SET_FILE_POINTER)
		return (uint64_t)-1;
	
	return (((uint64_t)high << 32) | (uint64_t)ret);
}

static BOOL WINAPI ReadFile_patched(HANDLE hFile, LPVOID lpBuffer, DWORD nNumberOfBytesToRead, LPDWORD lpNumberOfBytesRead, LPOVERLAPPED lpOverlapped)
{
	static bool data_patched = false;
	static bool data2_patched = false;
	static bool datap1_patched = false;
	static bool datap2_patched = false;
	static bool datap3_patched = false;

	if (data_patched && data2_patched && datap1_patched && datap2_patched && datap3_patched)
	{
		CustomDPRINTF("Main patch is finished. Unhooking function.\n");
		WriteMemory32((void *)readfile_import, (uint32_t)original_readfile);
		
		delete[] data_toc;
		delete[] data2_toc;
		delete[] datap1_toc;
		delete[] datap2_toc;
		delete[] datap3_toc;

		delete[] data_hdr;
		delete[] data2_hdr;
		delete[] datap1_hdr;
		delete[] datap2_hdr;
		delete[] datap3_hdr;
	}
	else
	{		
		if (IsThisFile(hFile, DATA_CPK))
		{
			if (nNumberOfBytesToRead == 0x800)
			{
				if (GetFilePointer(hFile) == 0)
				{
					memcpy(lpBuffer, data_hdr, nNumberOfBytesToRead);
					SetFilePointer(hFile, nNumberOfBytesToRead, NULL, FILE_CURRENT);
					
					if (lpNumberOfBytesRead)
					{
						*lpNumberOfBytesRead = nNumberOfBytesToRead;
					}
				
					CustomDPRINTF("data.cpk HDR patched.\n");						
					return TRUE;
				}
			}
			
			if (nNumberOfBytesToRead == data_toc_size)
			{
				if (GetFilePointer(hFile) == data_toc_offset)
				{
					memcpy(lpBuffer, data_toc, nNumberOfBytesToRead);
					SetFilePointer(hFile, nNumberOfBytesToRead, NULL, FILE_CURRENT);
				
					if (lpNumberOfBytesRead)
					{
						*lpNumberOfBytesRead = nNumberOfBytesToRead;
					}
				
					CustomDPRINTF("data.cpk TOC patched.\n");	
					data_patched = true;
					return TRUE;
				}
			}
		}
		
		else if (IsThisFile(hFile, DATA2_CPK))
		{
			if (nNumberOfBytesToRead == 0x800)
			{
				if (GetFilePointer(hFile) == 0)
				{
					memcpy(lpBuffer, data2_hdr, nNumberOfBytesToRead);
					SetFilePointer(hFile, nNumberOfBytesToRead, NULL, FILE_CURRENT);
					
					if (lpNumberOfBytesRead)
					{
						*lpNumberOfBytesRead = nNumberOfBytesToRead;
					}
				
					CustomDPRINTF("data2.cpk HDR patched.\n");						
					return TRUE;
				}
			}			
			
			if (nNumberOfBytesToRead == data2_toc_size)
			{
				if (GetFilePointer(hFile) == data2_toc_offset)
				{
					memcpy(lpBuffer, data2_toc, nNumberOfBytesToRead);
					SetFilePointer(hFile, nNumberOfBytesToRead, NULL, FILE_CURRENT);
				
					if (lpNumberOfBytesRead)
					{
						*lpNumberOfBytesRead = nNumberOfBytesToRead;
					}
				
					CustomDPRINTF("data2.cpk TOC patched.\n");
					data2_patched = true;
					return TRUE;
				}
			}
		}
		else if (IsThisFile(hFile, DATAP1_CPK))
		{
			if (nNumberOfBytesToRead == 0x800)
			{
				if (GetFilePointer(hFile) == 0)
				{
					memcpy(lpBuffer, datap1_hdr, nNumberOfBytesToRead);
					SetFilePointer(hFile, nNumberOfBytesToRead, NULL, FILE_CURRENT);
					
					if (lpNumberOfBytesRead)
					{
						*lpNumberOfBytesRead = nNumberOfBytesToRead;
					}
				
					CustomDPRINTF("datap1.cpk HDR patched.\n");						
					return TRUE;
				}
			}			
			
			if (nNumberOfBytesToRead == datap1_toc_size)
			{
				if (GetFilePointer(hFile) == datap1_toc_offset)
				{
					memcpy(lpBuffer, datap1_toc, nNumberOfBytesToRead);
					SetFilePointer(hFile, nNumberOfBytesToRead, NULL, FILE_CURRENT);
				
					if (lpNumberOfBytesRead)
					{
						*lpNumberOfBytesRead = nNumberOfBytesToRead;
					}
				
					CustomDPRINTF("datap1.cpk TOC patched.\n");
					data2_patched = true;
					return TRUE;
				}
			}
		}
		else if (IsThisFile(hFile, DATAP2_CPK))
		{
			if (nNumberOfBytesToRead == 0x800)
			{
				if (GetFilePointer(hFile) == 0)
				{
					memcpy(lpBuffer, datap2_hdr, nNumberOfBytesToRead);
					SetFilePointer(hFile, nNumberOfBytesToRead, NULL, FILE_CURRENT);
					
					if (lpNumberOfBytesRead)
					{
						*lpNumberOfBytesRead = nNumberOfBytesToRead;
					}
				
					CustomDPRINTF("datap2.cpk HDR patched.\n");						
					return TRUE;
				}
			}			
			
			if (nNumberOfBytesToRead == datap2_toc_size)
			{
				if (GetFilePointer(hFile) == datap2_toc_offset)
				{
					memcpy(lpBuffer, datap2_toc, nNumberOfBytesToRead);
					SetFilePointer(hFile, nNumberOfBytesToRead, NULL, FILE_CURRENT);
				
					if (lpNumberOfBytesRead)
					{
						*lpNumberOfBytesRead = nNumberOfBytesToRead;
					}
				
					CustomDPRINTF("datap2.cpk TOC patched.\n");
					datap2_patched = true;
					return TRUE;
				}
			}
		}
		else if (IsThisFile(hFile, DATAP3_CPK))
		{
			if (nNumberOfBytesToRead == 0x800)
			{
				if (GetFilePointer(hFile) == 0)
				{
					memcpy(lpBuffer, datap3_hdr, nNumberOfBytesToRead);
					SetFilePointer(hFile, nNumberOfBytesToRead, NULL, FILE_CURRENT);
					
					if (lpNumberOfBytesRead)
					{
						*lpNumberOfBytesRead = nNumberOfBytesToRead;
					}
				
					CustomDPRINTF("datap3.cpk HDR patched.\n");						
					return TRUE;
				}
			}			
			
			if (nNumberOfBytesToRead == datap3_toc_size)
			{
				if (GetFilePointer(hFile) == datap3_toc_offset)
				{
					memcpy(lpBuffer, datap3_toc, nNumberOfBytesToRead);
					SetFilePointer(hFile, nNumberOfBytesToRead, NULL, FILE_CURRENT);
				
					if (lpNumberOfBytesRead)
					{
						*lpNumberOfBytesRead = nNumberOfBytesToRead;
					}
				
					CustomDPRINTF("datap3.cpk TOC patched.\n");
					datap3_patched = true;
					return TRUE;
				}
			}
		}
	}
	
	return ReadFile(hFile, lpBuffer, nNumberOfBytesToRead, lpNumberOfBytesRead, lpOverlapped);
}

static uint8_t __thiscall cpk_file_exists_patched(void *object, char *file)
{
	uint8_t ret = cpk_file_exists(object, file);
	
	if (ret == 0)
	{
		return local_file_exists(file);
	}
	
	return ret;
}

void patches()
{
	readfile_import = (void **)GetModuleImport(GetModuleHandle(NULL), "KERNEL32.dll", "ReadFile");
	if (!readfile_import)
	{
		CustomDPRINTF("Cannot find ReadFile import!.\n");
		return;
	}
	
	original_readfile = *readfile_import;	
	CustomDPRINTF("Patch at %p\n", readfile_import);
	WriteMemory32((void *)readfile_import, (uint32_t)ReadFile_patched);		

	HookFunction(CPK_FILE_EXISTS_SYMBOL, (void **)&cpk_file_exists, (void *)cpk_file_exists_patched);	
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
	
	
	//This patch MUST go here, it's not optional!!!
	std::string keyToCheck = "Debug.Debug_Console";
	if(iniValues.find(keyToCheck) != iniValues.end() && iniValues[keyToCheck] == "True")
	{
		// Allocate a console for the application
		AllocConsole();

		// Get handle to the console output
		consoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);
		if (consoleHandle == INVALID_HANDLE_VALUE) {
			// Handle error, if needed
			return 1;
		}

		// Redirect standard output to the console
		freopen("CONOUT$", "w", stdout);
		freopen("CONOUT$", "w", stderr);
	}
	else{
		CustomDPRINTF("Console Debugging is not enabled or the key is not present.\n");
	}
	//////////////////////////////////////////////


	switch (fdwReason)
	{
		case DLL_PROCESS_ATTACH: {


			if (InGameProcess())
			{
				HANDLE hProcess = GetCurrentProcess();
				uintptr_t moduleBaseAddress = GetModuleBaseAddress(L"DBXV.exe");

				if (!load_dll(false))
					return FALSE;	

				//PATCHES THAT DON'T REQUIRE INI FILE GO HERE
				CheckVersion();
				/////////////////////////////////////////////
				
				
				//PATCHES THAT REQUIRE INI FILE GO HERE
				std::string keyToCheck = "Patches.CMS_Patches";
				if (iniValues.find(keyToCheck) != iniValues.end() && iniValues[keyToCheck] == "True") {
					CustomDPRINTF("CMS Patches are enabled.\n");
					CMSPatches(hProcess, moduleBaseAddress);
				} 
				else 
				{
					CustomDPRINTF("CMS Patches are not enabled or the key is not present.\n");
				}



				keyToCheck = "Patches.Version_String_Patch";
				if (iniValues.find(keyToCheck) != iniValues.end() && iniValues[keyToCheck] == "True") {
					CustomDPRINTF("Version String Patch is enabled.\n");
					VersionStringPatch(hProcess, moduleBaseAddress);
				} 
				else 
				{
					CustomDPRINTF("Version String Patch is not enabled or the key is not present.\n");
				}
				
				
				
				keyToCheck = "Patches.CPK_Patch";
				if (iniValues.find(keyToCheck) != iniValues.end() && iniValues[keyToCheck] == "True") {
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
            if (!lpvReserved) {
                FreeConsole();
                unload_dll();
            }
    }            
	break;
}
	
	return TRUE;
}