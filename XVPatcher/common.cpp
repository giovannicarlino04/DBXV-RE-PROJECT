#include "common.h"

// Function to hook an imported function
bool HookImport(const char* moduleName, const char* functionName, void* newFunction) {
    HMODULE hModule = GetModuleHandleA(moduleName);
    if (hModule == NULL) {
        return false; // Module not found
    }

    // Get the address of the Import Address Table (IAT) for the module
    PIMAGE_DOS_HEADER dosHeader = (PIMAGE_DOS_HEADER)hModule;
    if (dosHeader->e_magic != IMAGE_DOS_SIGNATURE) {
        return false; // Invalid DOS header
    }

    PIMAGE_NT_HEADERS ntHeader = (PIMAGE_NT_HEADERS)((DWORD)dosHeader + dosHeader->e_lfanew);
    if (ntHeader->Signature != IMAGE_NT_SIGNATURE) {
        return false; // Invalid NT header
    }

    PIMAGE_IMPORT_DESCRIPTOR importDesc = (PIMAGE_IMPORT_DESCRIPTOR)((DWORD)dosHeader + ntHeader->OptionalHeader.DataDirectory[IMAGE_DIRECTORY_ENTRY_IMPORT].VirtualAddress);

    // Iterate through the imported functions
    while (importDesc->Name != NULL) {
        LPCSTR dllName = (LPCSTR)((DWORD)dosHeader + importDesc->Name);
        if (_stricmp(dllName, moduleName) == 0) {
            PIMAGE_THUNK_DATA thunk = (PIMAGE_THUNK_DATA)((DWORD)dosHeader + importDesc->FirstThunk);

            // Iterate through the imported functions from the current DLL
            while (thunk->u1.Function != NULL) {
                PIMAGE_IMPORT_BY_NAME import = (PIMAGE_IMPORT_BY_NAME)((DWORD)dosHeader + thunk->u1.AddressOfData);
                if (strcmp((const char*)import->Name, functionName) == 0) {
                    DWORD oldProtect;
                    if (!VirtualProtect(&thunk->u1.Function, sizeof(thunk->u1.Function), PAGE_EXECUTE_READWRITE, &oldProtect)) {
                        return false; // Failed to change protection
                    }

                    thunk->u1.Function = (DWORD)newFunction;

                    if (!VirtualProtect(&thunk->u1.Function, sizeof(thunk->u1.Function), oldProtect, &oldProtect)) {
                        return false; // Failed to restore protection
                    }

                    return true; // Hook successful
                }

                thunk++;
            }
        }

        importDesc++;
    }

    return false; // Function not found
}

uintptr_t GetModuleBaseAddress(const wchar_t* modName) {
    HMODULE hModule = GetModuleHandleW(modName);
    if (hModule != NULL) {
        return (uintptr_t)hModule; // Return the base address of the module
    } else {
        return 0; // Module not found
    }
}

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
