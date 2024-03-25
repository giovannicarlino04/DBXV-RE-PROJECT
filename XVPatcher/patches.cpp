#include "patches.h"

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
	}

    /////////////// PATCHES GO HERE ///////////////

    // CMS Patch 1
    if (address1 == nullptr) {
    }

    if (WriteProcessMemory(hProcess, address1, newBytes1, strlen(newBytes1), &numberOfBytesWritten)) {
    }
    else {
    }

    // CMS Patch 2
    if (address2 == nullptr) {
    }

    if (WriteProcessMemory(hProcess, address2, newBytes2, strlen(newBytes2), &numberOfBytesWritten)) {
    }
    else {
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
        return false;
    }

    if (!VirtualProtect(address1, patchsize, newProtect, &oldProtect)) {
        return false;
    }

    if (!WriteProcessMemory(hProcess, address1, newBytes1, patchsize, &numberOfBytesWritten)) {
        return false;
    }

    if (!VirtualProtect(address1, patchsize, oldProtect, &newProtect)) {
        return false;
    }

    return true;
}
