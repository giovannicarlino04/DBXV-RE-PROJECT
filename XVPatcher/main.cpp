#define _WIN32_WINNT	0x600

#include <windows.h>
#include <winbase.h>
#include <stdio.h>
#include <io.h>
#include <stdint.h>

#include "steam_api.h"
#include "patch.h"
#include "debug.h"
#include "symbols.h"
#include "CpkFile.h"

#define EXPORT WINAPI __declspec(dllexport)

#define NUM_EXPORTED_FUNCTIONS	18

#define PROCESS_NAME 	"dbxv.exe"
#define DATA_CPK	"data.cpk"
#define DATA2_CPK	"data2.cpk"
#define DATAP1_CPK	"data2.cpk"
#define DATAP2_CPK	"data2.cpk"
#define DATAP3_CPK	"data2.cpk"




const char *gSteamExportsNames[NUM_EXPORTED_FUNCTIONS] =
{
	"SteamAPI_SetMiniDumpComment",
	"SteamAPI_WriteMiniDump",
	"SteamUserStats",
	"SteamAPI_UnregisterCallResult",
	"SteamAPI_RegisterCallResult",
	"SteamAPI_RunCallbacks",
	"SteamAPI_RegisterCallback",
	"SteamAPI_UnregisterCallback",
	"SteamMatchmaking",
	"SteamUtils",
	"SteamUser",
	"SteamFriends",
	"SteamRemoteStorage",
	"SteamClient",
	"SteamAPI_Init",
	"SteamAPI_Shutdown",
	"SteamNetworking",
	"SteamApps"
};

DummyFunction gMyExports[NUM_EXPORTED_FUNCTIONS] =
{
	SteamAPI_SetMiniDumpComment,
	SteamAPI_WriteMiniDump,
	SteamUserStats,
	SteamAPI_UnregisterCallResult,
	SteamAPI_RegisterCallResult,
	SteamAPI_RunCallbacks,
	SteamAPI_RegisterCallback,
	SteamAPI_UnregisterCallback,
	SteamMatchmaking,
	SteamUtils,
	SteamUser,
	SteamFriends,
	SteamRemoteStorage,
	SteamClient,
	SteamAPI_Init,
	SteamAPI_Shutdown,
	SteamNetworking,
	SteamApps
};

uint8_t (* __thiscall cpk_file_exists)(void *, char *);

static HANDLE gOrigSteam;

static uint64_t localize_toc_offset, localize_toc_size;
static uint64_t resource_toc_offset, resource_toc_size;
static uint8_t *localize_toc, *resource_toc;
static uint8_t *localize_hdr, *resource_hdr;
static void **readfile_import;
static void *original_readfile;

static BOOL InGameProcess(VOID)
{
	char szPath[MAX_PATH];
	
	GetModuleFileName(NULL, szPath, MAX_PATH);
	_strlwr(szPath);
	
	// A very poor aproach, I know
	return (strstr(szPath, PROCESS_NAME) != NULL);
}

static BOOL LoadDllAndResolveExports(VOID)
{
	char szDll[32];
	
	strcpy(szDll, "steam_api_real.dll");
	
	gOrigSteam = LoadLibrary(szDll);		
	if (!gOrigSteam)
	{
		UPRINTF("I told you to rename original steam_api.dll as steam_api_real.dll!");
		return FALSE;
	}
		
	for (int i = 0; i < NUM_EXPORTED_FUNCTIONS; i++)
	{
		PVOID orig_func = (PVOID)GetProcAddress((HMODULE)gOrigSteam, gSteamExportsNames[i]);
		if (!orig_func)
		{
			DPRINTF("Failed to get original function: %s\n", gSteamExportsNames[i]);
			return FALSE;
		}
		
		uint8_t *my_func = (uint8_t *)gMyExports[i];	
		if (*my_func != 0xB8) // Not what we expected
		{
			DPRINTF("Eeer hmmm, seems like this is not the real address of the function.\n");
		}
		
		WriteMemory32(my_func+1, (DWORD)orig_func);	// my func+1 = the operand part in mov eax, 0x12345678 
	}
	
	return TRUE;
}

static VOID UnloadDll(VOID)
{
	if (gOrigSteam)
	{
		FreeLibrary((HMODULE)gOrigSteam);
		gOrigSteam = NULL;
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
		DPRINTF("read_file_from failed (2)\n");
		delete[] *toc_buf;
		goto clean;
	}
	
	if (rsize != *toc_size)
	{
		DPRINTF("Warning: read size doesn't match requested size.\n");
	}
	
	if (!cpk->ParseTocData(*toc_buf))
	{
		goto clean;
	}	
	
	DPRINTF("This .cpk has %d files.\n", cpk->GetNumFiles());
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

static bool get_cpk_tocs(CpkFile **loc, CpkFile **res)
{
	*loc = get_cpk_toc(DATA_CPK, &localize_toc_offset, &localize_toc_size, &localize_hdr, &localize_toc);
	if (!(*loc))
		return false;	
	
	*res = get_cpk_toc(DATA2_CPK, &resource_toc_offset, &resource_toc_size, &resource_hdr, &resource_toc);
	if (!(*res))
	{
		delete[] localize_toc;
		delete *loc;
		return false;
	}
	
	DPRINTF("localize.cpk.toc = %I64x, size = %I64x\n", localize_toc_offset, localize_toc_size);
	DPRINTF("resource.cpk.toc = %I64x, size = %I64x\n", resource_toc_offset, resource_toc_size);
	
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
	
	DPRINTF("%d files deleted in RAM.\n", count);
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
	static bool localize_patched = false;
	static bool resource_patched = false;
	
	if (localize_patched && resource_patched)
	{
		DPRINTF("Main patch is finished. Unhooking function.\n");
		WriteMemory32((void *)readfile_import, (uint32_t)original_readfile);
		
		delete[] localize_toc;
		delete[] resource_toc;
		delete[] localize_hdr;
		delete[] resource_hdr;
	}
	else
	{		
		if (IsThisFile(hFile, DATA_CPK))
		{
			if (nNumberOfBytesToRead == 0x800)
			{
				if (GetFilePointer(hFile) == 0)
				{
					memcpy(lpBuffer, localize_hdr, nNumberOfBytesToRead);
					SetFilePointer(hFile, nNumberOfBytesToRead, NULL, FILE_CURRENT);
					
					if (lpNumberOfBytesRead)
					{
						*lpNumberOfBytesRead = nNumberOfBytesToRead;
					}
				
					DPRINTF("localize.cpk HDR patched.\n");						
					return TRUE;
				}
			}
			
			if (nNumberOfBytesToRead == localize_toc_size)
			{
				if (GetFilePointer(hFile) == localize_toc_offset)
				{
					memcpy(lpBuffer, localize_toc, nNumberOfBytesToRead);
					SetFilePointer(hFile, nNumberOfBytesToRead, NULL, FILE_CURRENT);
				
					if (lpNumberOfBytesRead)
					{
						*lpNumberOfBytesRead = nNumberOfBytesToRead;
					}
				
					DPRINTF("localize.cpk TOC patched.\n");	
					localize_patched = true;
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
					memcpy(lpBuffer, resource_hdr, nNumberOfBytesToRead);
					SetFilePointer(hFile, nNumberOfBytesToRead, NULL, FILE_CURRENT);
					
					if (lpNumberOfBytesRead)
					{
						*lpNumberOfBytesRead = nNumberOfBytesToRead;
					}
				
					DPRINTF("resource.cpk HDR patched.\n");						
					return TRUE;
				}
			}			
			
			if (nNumberOfBytesToRead == resource_toc_size)
			{
				if (GetFilePointer(hFile) == resource_toc_offset)
				{
					memcpy(lpBuffer, resource_toc, nNumberOfBytesToRead);
					SetFilePointer(hFile, nNumberOfBytesToRead, NULL, FILE_CURRENT);
				
					if (lpNumberOfBytesRead)
					{
						*lpNumberOfBytesRead = nNumberOfBytesToRead;
					}
				
					DPRINTF("resource.cpk TOC patched.\n");
					resource_patched = true;
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
		//DPRINTF("File exists originally returned 0 (%s)", file);
		return local_file_exists(file);
	}
	
	return ret;
}

/*void *(* open_cpk_file)(int, const char *, int );

void *my_open_cpk_file(int a1, const char *s, int a3)
{
	void *ret = open_cpk_file(a1, s, a3);
	
	DPRINTF("open_cpk_file = %s; a1=%x, a3=%x; ret = %p\n", s, a1, a3, ret);
	
	return ret;
}*/

void patches()
{
	readfile_import = (void **)GetModuleImport(GetModuleHandle(NULL), "KERNEL32.dll", "ReadFile");
	if (!readfile_import)
	{
		DPRINTF("Cannot find ReadFile import!.\n");
		return;
	}
	
	original_readfile = *readfile_import;	
	DPRINTF("Patch at %p\n", readfile_import);
	WriteMemory32((void *)readfile_import, (uint32_t)ReadFile_patched);		

	HookFunction(CPK_FILE_EXISTS_SYMBOL, (void **)&cpk_file_exists, (void *)cpk_file_exists_patched);	
	//HookFunction(CPK_OPEN_FILE_SYMBOL, (void **)&open_cpk_file, (void *)my_open_cpk_file);
}

extern "C" BOOL EXPORT DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved)
{
	switch (fdwReason)
	{
		case DLL_PROCESS_ATTACH:
		
			DPRINTF("Hello world.\n");
		
			if (InGameProcess())
			{
				if (!LoadDllAndResolveExports())
					return FALSE;
				
				CpkFile *loc, *res;
				
				if (get_cpk_tocs(&loc, &res))
				{
					patch_toc(loc);
					patch_toc(res);
					
					loc->RevertEncryption(false);
					res->RevertEncryption(false);
										
					patches();
#ifdef DEBUG
					debug_patches();
#endif
					
					delete loc;
					delete res;
				}				
			}		
			
		break;
		
		case DLL_PROCESS_DETACH:		
			
			if (!lpvReserved)
				UnloadDll();
			
		break;
	}
	
	return TRUE;
}
