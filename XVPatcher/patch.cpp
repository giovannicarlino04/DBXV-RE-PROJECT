#include <windows.h>
#include <stdint.h>
#include <MinHook.h>

#ifdef _MSC_VER 
//not #if defined(_WIN32) || defined(_WIN64) because we have strncasecmp in mingw
#define strncasecmp _strnicmp
#define strcasecmp _stricmp
#endif

#include "patch.h"

void WriteMemory32(void *address, uint32_t data)
{
	DWORD lOldProtect;
	
	VirtualProtect(address, 4, PAGE_EXECUTE_READWRITE, &lOldProtect);
	*(uint32_t *) address = data;
	VirtualProtect(address, 4, lOldProtect, &lOldProtect);
}

void WriteMemoryRel8(uint32_t rel_address, uint8_t data, const char *mod)
{
	DWORD lOldProtect;
	
	uint8_t *mod_top = (uint8_t *)GetModuleHandle(mod);	
	if (!mod_top)
		return;
	
	uint8_t *address = mod_top + rel_address;
	
	VirtualProtect(address, 4, PAGE_EXECUTE_READWRITE, &lOldProtect);
	*address = data;
	VirtualProtect(address, 4, lOldProtect, &lOldProtect);
}

PVOID GetModuleImport(HMODULE hModule, LPCSTR lpModName, LPCSTR lpProcName)
{
	PUINT8 pModTop;
	PIMAGE_DOS_HEADER pDosHdr;
	PIMAGE_NT_HEADERS pNtHdr;
	PIMAGE_IMPORT_DESCRIPTOR pImportDir;
		
	pModTop = (PUINT8) hModule;
	pDosHdr = (PIMAGE_DOS_HEADER) pModTop;
	pNtHdr = (PIMAGE_NT_HEADERS) (pModTop + pDosHdr->e_lfanew);
	pImportDir = (PIMAGE_IMPORT_DESCRIPTOR) (pModTop + pNtHdr->OptionalHeader.DataDirectory[IMAGE_DIRECTORY_ENTRY_IMPORT].VirtualAddress);
		
	while (pImportDir->OriginalFirstThunk != 0)
	{
		PCHAR pName = (PCHAR) (pModTop + pImportDir->Name);
		
		if (strcasecmp(pName, lpModName) == 0)
		{
			PIMAGE_THUNK_DATA pThunkData;
						
			pThunkData = (PIMAGE_THUNK_DATA) (pModTop + pImportDir->OriginalFirstThunk);
			
			while (pThunkData->u1.AddressOfData != 0)
			{
				PIMAGE_IMPORT_BY_NAME pImportByName = (PIMAGE_IMPORT_BY_NAME) (pModTop + pThunkData->u1.AddressOfData);
				
				if (strcmp((char *)pImportByName->Name, lpProcName) == 0)
				{
					return (PVOID) ((DWORD) (pThunkData) + pImportDir->FirstThunk - pImportDir->OriginalFirstThunk);
				}
				
				pThunkData++;
			}
			
			break;
		}
		
		pImportDir++;		
	}		
	
	return NULL;	
}

long HookFunction(uint32_t rel_addr, void **orig, void *newfunc, const char *mod)
{
	HMODULE hMod = GetModuleHandle(mod);
	if (!hMod)
		return -1;
	
	if (orig)
	{	
		*orig = (void *)( ((uint32_t)hMod) + rel_addr );
	
		MH_EnableHook(orig);	
		MH_CreateHook(orig, newfunc, NULL);
		return 1;
	}
	else
	{
		void *my_orig;

		my_orig = (void *)( ((uint32_t)hMod) + rel_addr );
		MH_EnableHook(&my_orig);	
		MH_CreateHook(&my_orig, newfunc, NULL);
		return 1;
	}
	
	return 0;
}
