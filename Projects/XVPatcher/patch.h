#include <windows.h>
#include <stdint.h>

void WriteMemory32(void *address, uint32_t data);
void WriteMemoryRel8(uint32_t rel_address, uint8_t data, const char *mod=NULL);

PVOID GetModuleImport(HMODULE hModule, LPCSTR lpModName, LPCSTR lpProcName);

long HookFunction(uint32_t rel_addr, void **orig, void *newfunc, const char *mod=NULL);