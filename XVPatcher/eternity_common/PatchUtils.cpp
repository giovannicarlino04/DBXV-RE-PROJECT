#include <windows.h>
#include <MinHook.h>
#include "PatchUtils.h"
#include "debug.h"

void *PatchUtils::GetPtr(size_t rel_address, const char *mod)
{
    uint8_t *mod_top = (uint8_t *)GetModuleHandleA(mod);
	if (!mod_top)
		return nullptr;
	
	return (void *)(mod_top+rel_address);
}

ptrdiff_t PatchUtils::RelAddress(void *ptr, const char *mod)
{
    uint8_t *mod_top = (uint8_t *)GetModuleHandleA(mod);
	if (!mod_top)
		return -1;
	
	return (uint8_t *)ptr - mod_top;
}

uint8_t PatchUtils::Read8(size_t rel_address, const char *mod)
{
    uint8_t *mod_top = (uint8_t *)GetModuleHandleA(mod);
	if (!mod_top)
		return 0;
	
	return *(mod_top+rel_address);
}

uint16_t PatchUtils::Read16(size_t rel_address, const char *mod)
{
    uint8_t *mod_top = (uint8_t *)GetModuleHandleA(mod);
	if (!mod_top)
		return 0;
	
	return *(uint16_t *)(mod_top+rel_address);
}

uint32_t PatchUtils::Read32(size_t rel_address, const char *mod)
{
    uint8_t *mod_top = (uint8_t *)GetModuleHandleA(mod);
	if (!mod_top)
		return 0;
	
	return *(uint32_t *)(mod_top+rel_address);
}

uint64_t PatchUtils::Read64(size_t rel_address, const char *mod)
{
    uint8_t *mod_top = (uint8_t *)GetModuleHandleA(mod);
	if (!mod_top)
		return 0;
	
	return *(uint64_t *)(mod_top+rel_address);
}

void PatchUtils::Write8(void *address, uint8_t data)
{
	DWORD lOldProtect;
	
	VirtualProtect(address, sizeof(uint8_t), PAGE_EXECUTE_READWRITE, &lOldProtect);
	*(uint8_t *)address = data;
	VirtualProtect(address, sizeof(uint8_t), lOldProtect, &lOldProtect);
}

bool PatchUtils::Write8(size_t rel_address, uint8_t data, const char *mod)
{
    uint8_t *mod_top = (uint8_t *)GetModuleHandleA(mod);
	if (!mod_top)
		return false;
	
	uint8_t *address = mod_top + rel_address;
	Write8(address, data);
	return true;
}

void PatchUtils::Write16(void *address, uint16_t data)
{
	DWORD lOldProtect;
	
	VirtualProtect(address, sizeof(uint16_t), PAGE_EXECUTE_READWRITE, &lOldProtect);
	*(uint16_t *)address = data;
	VirtualProtect(address, sizeof(uint16_t), lOldProtect, &lOldProtect);
}

bool PatchUtils::Write16(size_t rel_address, uint16_t data, const char *mod)
{
    uint8_t *mod_top = (uint8_t *)GetModuleHandleA(mod);
	if (!mod_top)
		return false;
	
	uint8_t *address = mod_top + rel_address;
	Write16(address, data);
	return true;
}

void PatchUtils::Write32(void *address, uint32_t data)
{
	DWORD lOldProtect;
	
	VirtualProtect(address, sizeof(uint32_t), PAGE_EXECUTE_READWRITE, &lOldProtect);
	*(uint32_t *)address = data;
	VirtualProtect(address, sizeof(uint32_t), lOldProtect, &lOldProtect);
}

bool PatchUtils::Write32(size_t rel_address, uint32_t data, const char *mod)
{
    uint8_t *mod_top = (uint8_t *)GetModuleHandleA(mod);
	if (!mod_top)
		return false;
	
	uint8_t *address = mod_top + rel_address;
	Write32(address, data);
	return true;
}

void PatchUtils::Write64(void *address, uint64_t data)
{
	DWORD lOldProtect;
	
	VirtualProtect(address, sizeof(uint64_t), PAGE_EXECUTE_READWRITE, &lOldProtect);
	*(uint64_t *)address = data;
	VirtualProtect(address, sizeof(uint64_t), lOldProtect, &lOldProtect);
}

bool PatchUtils::Write64(size_t rel_address, uint64_t data, const char *mod)
{
    uint8_t *mod_top = (uint8_t *)GetModuleHandleA(mod);
	if (!mod_top)
		return false;
	
	uint8_t *address = mod_top + rel_address;
	Write64(address, data);
	return true;
}

void PatchUtils::Copy(void *dst, void *src, size_t size)
{
	DWORD lOldProtect;
	
	VirtualProtect(dst, size, PAGE_EXECUTE_READWRITE, &lOldProtect);
	memcpy(dst, src, size);
	VirtualProtect(dst, size, lOldProtect, &lOldProtect);
}

bool PatchUtils::Copy(size_t dst, void *src, size_t size, const char *mod)
{
    uint8_t *mod_top = (uint8_t *)GetModuleHandleA(mod);
	if (!mod_top)
		return false;
	
	uint8_t *address = mod_top + dst;
	Copy(address, src, size);
	return true;
}

bool PatchUtils::Copy(void *dst, size_t src, size_t size, const char *mod)
{
    uint8_t *mod_top = (uint8_t *)GetModuleHandleA(mod);
	if (!mod_top)
		return false;
	
	uint8_t *address = mod_top + src;
	Copy(dst, address, size);
	return true;
}

void PatchUtils::Fill(void *dst, uint8_t value, size_t size)
{
	DWORD lOldProtect;
	
	VirtualProtect(dst, size, PAGE_EXECUTE_READWRITE, &lOldProtect);
	memset(dst, value, size);
	VirtualProtect(dst, size, lOldProtect, &lOldProtect);
}

bool PatchUtils::Fill(size_t dst, uint8_t value, size_t size, const char *mod)
{
    uint8_t *mod_top = (uint8_t *)GetModuleHandleA(mod);
	if (!mod_top)
		return false;
	
	uint8_t *address = mod_top + dst;
	Fill(address, value, size);
	return true;
}

bool PatchUtils::Hook(void *address, void **orig, void *new_func)
{	
    static bool mh_inited = false;

    if (!mh_inited)
    {
        if (MH_Initialize() != MH_OK)
        {
            //DPRINTF("Init failed.\n");
            return false;
        }

        mh_inited = true;
    }

    //DPRINTF("Hook %p -> %p\n", address, new_func);

    int err;
    if ((err = MH_CreateHook(address, new_func, orig)) != MH_OK)
    {
        //DPRINTF("Failed createhook. %d\n", err);
        return false;
    }

    return (MH_EnableHook(address) == MH_OK);
}

bool PatchUtils::Hook(size_t rel_address, void **orig, void *new_func, const char *mod)
{
    uint8_t *mod_top = (uint8_t *)GetModuleHandleA(mod);
	if (!mod_top)
		return false;
	
	uint8_t *address = mod_top + rel_address;
    return Hook(address, orig, new_func);
}

bool PatchUtils::Hook(const char *mod, const char *func, void **orig, void *new_func)
{
   HMODULE hMod = GetModuleHandleA(mod);
   if (!hMod)
       return false;

   void *address = (void *)GetProcAddress(hMod, func);
   if (!address)
       return false;

   return Hook(address, orig, new_func);
}

void *PatchUtils::FindImport(const char *import_mod, const char *import_func, const char *mod)
{
    uint8_t *mod_top = (uint8_t *)GetModuleHandleA(mod);
    if (!mod_top)
        return nullptr;

    IMAGE_DOS_HEADER *dos_hdr = (IMAGE_DOS_HEADER *)mod_top;
    IMAGE_NT_HEADERS *nt_hdr = (IMAGE_NT_HEADERS *)(mod_top + dos_hdr->e_lfanew);
    IMAGE_IMPORT_DESCRIPTOR	*import_dir = (IMAGE_IMPORT_DESCRIPTOR *)(mod_top + nt_hdr->OptionalHeader.DataDirectory[IMAGE_DIRECTORY_ENTRY_IMPORT].VirtualAddress);

    while (import_dir->Characteristics)
    {
        const char *name = (const char *)(mod_top + import_dir->Name);

        if (strcasecmp(name, import_mod) == 0)
        {
            IMAGE_THUNK_DATA *thunk_data = (IMAGE_THUNK_DATA *)(mod_top + import_dir->OriginalFirstThunk);
            size_t *iat = (uintptr_t *)(mod_top + import_dir->FirstThunk);

            while (thunk_data->u1.Ordinal)
            {
                if (!IMAGE_SNAP_BY_ORDINAL(thunk_data->u1.Ordinal))
                {
                    IMAGE_IMPORT_BY_NAME *import_info = (IMAGE_IMPORT_BY_NAME *)(mod_top + thunk_data->u1.AddressOfData);

                    if (strcasecmp((char *)import_info->Name, import_func) == 0)
                        return iat;
                }

                thunk_data++;
                iat++;
            }

            return nullptr;
        }

        import_dir++;
    }

    return nullptr;
}

bool PatchUtils::HookImport(const char *import_mod, const char *import_func, void *new_func, const char *mod)
{
    void *addr = FindImport(import_mod, import_func, mod);
    if (!addr)
        return false;

#ifdef CPU_X86_64
    Write64(addr, (uint64_t)new_func);
#else
    Write32(addr, (uint32_t)new_func);
#endif

    return true;
}

bool PatchUtils::Unhook(void *address)
{
    return (MH_DisableHook(address) == MH_OK);
}

bool PatchUtils::Unhook(size_t rel_address, const char *mod)
{
    uint8_t *mod_top = (uint8_t *)GetModuleHandleA(mod);
    if (!mod_top)
        return false;

    uint8_t *address = mod_top + rel_address;
    return Unhook(address);
}

bool PatchUtils::Unhook(const char *mod, const char *func)
{
    HMODULE hMod = GetModuleHandleA(mod);
    if (!hMod)
        return false;

    void *address = (void *)GetProcAddress(hMod, func);
    if (!address)
        return false;

    return Unhook(address);
}

#ifdef CPU_X86

bool PatchUtils::HookCall(void *call_addr, void **orig, void *new_addr)
{
    uint8_t *ptr = (uint8_t *)call_addr;
    if (*ptr != 0xE8)
        return false;

    int32_t rel = *(int32_t *)(ptr+1);
    uint32_t target = (uint32_t)new_addr - (uint32_t)call_addr - 5;

    *orig = (void *) ((uint32_t)call_addr + rel + 5);
    Write32(ptr+1, target);

    return true;
}

#else

bool PatchUtils::HookCall(void *call_addr, void **orig, void *new_addr)
{
    uint8_t *ptr = (uint8_t *)call_addr;
    if (*ptr != 0xE8)
        return false;

    size_t addr = Utils::Align2((size_t)call_addr, 0x100000);
    size_t top = addr + 0x7FF00000;
    size_t down = addr - 0x7FF00000;
    uint8_t *tramp = nullptr;

    for (addr = down; addr < top; addr += 0x100000)
    {
        tramp = (uint8_t *)VirtualAlloc((void *)addr, 0x1000, MEM_COMMIT|MEM_RESERVE, PAGE_EXECUTE_READWRITE);
        if (tramp)
            break;
    }

    if (!tramp)
        return false;

    tramp[0] = 0xFF;
    tramp[1] = 0x25;
    *(uint32_t *)&tramp[2] = 0;
    *(uint64_t *)&tramp[6] = (uint64_t)new_addr;

    int32_t rel = *(int32_t *)(ptr+1);
    uint32_t target = (uint32_t) ((uint64_t)tramp - (uint64_t)call_addr - 5);

    *orig = (void *) ((uint64_t)call_addr + (int64_t)rel + 5);
    Write32(ptr+1, target);

    return true;
}

#endif

