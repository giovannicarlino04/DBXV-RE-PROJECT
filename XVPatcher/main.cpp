#include <Windows.h>
#include <Psapi.h>
#include <fstream>
#include <sstream>
#include <iostream>
#include <string>
#include <stdexcept>
#include <algorithm>
#include <thread>
#include <mutex>
#include <tchar.h>
#include <TlHelp32.h>
#include <locale>
#include <codecvt>
#include <vector>

std::mutex iggyMessagesMutex;
std::string latestIggyTrace;
std::string latestIggyWarning;
std::string latestIggyError;
std::ofstream logFile;
std::string debug;

#define XVPATCHER_VERSION "1.00"
#define INI_FILE "XVPatcher/XVPatcher.ini"
#define LOG_FILE "XVPatcher/XVPatcher.log"

typedef void (*ExternalAS3CallbackType)(void* custom_arg, void* iggy_obj, const char** pfunc_name);
typedef void* (*IggyPlayerCallbackResultPathType)(void* unk0);
typedef void (*IggyValueSetStringUTF8RSType)(void* arg1, void* unk2, void* unk3, const char* str, size_t length);
typedef void (*IggyValueSetS32RSType)(void* arg1, uint32_t unk2, uint32_t unk3, uint32_t value);
typedef void (*_Battle_Mob_Destructor)(void*);

static ExternalAS3CallbackType ExternalAS3Callback;
static IggyPlayerCallbackResultPathType IggyPlayerCallbackResultPath;
static IggyValueSetStringUTF8RSType IggyValueSetStringUTF8RS;
static IggyValueSetS32RSType IggyValueSetS32RS;
static _Battle_Mob_Destructor Battle_Mob_Destructor;

// Dichiarazione delle funzioni
bool GetIggyModule(HANDLE processHandle, HMODULE& iggyModule);
void PatchIggyCallbacks(HMODULE iggyModule);
void PatchExternalAS3Callback(FARPROC externalAS3CallbackFunc, uintptr_t iggyBaseAddress);

void LogDebug(const std::string& message)
{
    if (debug == "true") {
        logFile << message << std::endl;
        std::cout << message << std::endl;
        logFile.flush();
    }
}

std::string GetIniValue(const std::string& filePath, const std::string& section, const std::string& key)
{
    const int bufferSize = 255;
    char buffer[bufferSize];

    DWORD bytesRead = GetPrivateProfileStringA(section.c_str(), key.c_str(), "", buffer, bufferSize, filePath.c_str());

    LogDebug("Got ini value at " + section + " " + key);
    return std::string(buffer, bytesRead);
}

void SetupExternalAS3Callback(ExternalAS3CallbackType orig);

typedef void (*SendToAS3Type)(void *, int32_t, const void *);
SendToAS3Type SendToAS3;

void HandleIggyTrace(const std::string& TraceMessage)
{
    std::lock_guard<std::mutex> lock(iggyMessagesMutex);
    // Perform actions based on the Iggy trace message
    // For example, log it, display a notification, or handle it in a specific way
    LogDebug("Iggy Trace: " + TraceMessage);
}

void HandleIggyWarning(const std::string& warningMessage)
{
    std::lock_guard<std::mutex> lock(iggyMessagesMutex);
    // Perform actions based on the Iggy warning message
    // For example, log it, display a notification, or handle it in a specific way
    LogDebug("Iggy Warning: " + warningMessage);
}

void HandleIggyError(const std::string& errorMessage)
{
    std::lock_guard<std::mutex> lock(iggyMessagesMutex);
    // Perform actions based on the Iggy error message
    // For example, log it, display an error message, or handle it in a specific way
    LogDebug("Iggy Error: " + errorMessage);
}

void iggy_trace_callback(void* param, void* unk, size_t len, const char* str)
{
    std::string traceMessage(str, len);
    {
        std::lock_guard<std::mutex> lock(iggyMessagesMutex);
        latestIggyTrace = traceMessage;
    }
    HandleIggyTrace(traceMessage);
}

void iggy_warning_callback(void* param, void* unk, size_t len, const char* str)
{
    std::string warningMessage(str, len);
    {
        std::lock_guard<std::mutex> lock(iggyMessagesMutex);
        latestIggyWarning = warningMessage;
    }
    HandleIggyWarning(warningMessage);
}

void iggy_error_callback(void* param, void* unk, size_t len, const char* str)
{
    std::string errorMessage(str, len);
    {
        std::lock_guard<std::mutex> lock(iggyMessagesMutex);
        latestIggyError = errorMessage;
    }
    HandleIggyError(errorMessage);
}

static void IggySetTraceCallbackUTF8Patched(void*, void* param)
{
    
    HMODULE iggy = GetModuleHandleA("iggy_w32.dll");
    if (!iggy) {
        LogDebug("Failed to get the module handle for iggy_w32.dll");
        return;
    }

    FARPROC func = GetProcAddress(iggy, "_IggySetTraceCallbackUTF8@8");
    if (!func) {
        LogDebug("Failed to get the function address for IggySetTraceCallbackUTF8@8");
        return;
    }

    typedef void (*IGGYSetTraceCallbackType)(void* callback, void* param);
    IGGYSetTraceCallbackType iggySetTraceCallback = reinterpret_cast<IGGYSetTraceCallbackType>(func);
    if (!iggySetTraceCallback) {
        LogDebug("Failed to cast the function address for IggySetTraceCallbackUTF8@8");
        return;
    }

    try {
        LogDebug("Patching IggySetTraceCallbackUTF8@8");
        iggySetTraceCallback(param, reinterpret_cast<void*>(&iggy_trace_callback));
    }
    catch (const std::exception& ex) {
        LogDebug("Exception occurred while patching IggySetTraceCallbackUTF8@8: " + std::string(ex.what()));
    }
    catch (...) {
        LogDebug("Unknown exception occurred while patching IggySetTraceCallbackUTF8@8");
    }
}

static void IggySetWarningCallbackPatched(void*, void* param)
{
    HMODULE iggy = GetModuleHandleA("iggy_w32.dll");
    if (!iggy) {
        LogDebug("Failed to get the module handle for iggy_w32.dll");
        return;
    }

    FARPROC func = GetProcAddress(iggy, "_IggySetWarningCallback@8");
    if (!func) {
        LogDebug("Failed to get the function address for IggySetWarningCallback@8");
        return;
    }

    typedef void (*IGGYSetWarningCallbackType)(void* callback, void* param);
    IGGYSetWarningCallbackType iggySetWarningCallback = reinterpret_cast<IGGYSetWarningCallbackType>(func);
    if (!iggySetWarningCallback) {
        LogDebug("Failed to cast the function address for IggySetWarningCallback@8");
        return;
    }

    try {
        LogDebug("Patching IggySetWarningCallback@8");
        iggySetWarningCallback(param, reinterpret_cast<void*>(&iggy_warning_callback));
    }
    catch (const std::exception& ex) {
        LogDebug("Exception occurred while patching IggySetWarningCallback@8: " + std::string(ex.what()));
    }
    catch (...) {
        LogDebug("Unknown exception occurred while patching IggySetWarningCallback@8");
    }
}

std::string ToString(const std::u16string& str)
{
    std::string result;
    for (const char16_t& ch : str) {
        result += static_cast<char>(ch);
    }
    return result;
}

void PatchGameVersionString(void* obj, int32_t code)
{
    try {
        static std::u16string version = u"ver.1.08.00"; // THIS VAR MUST BE STATIC TO AVOID OBJECT DESTRUCTION AT THE END OF THE METHOD

        LogDebug("PatchGameVersionString called");

        LogDebug("Running game version " + ToString(version));
        if (version.length() >= 4) {
            version = u"<font size=\"9\">v" + version.substr(1) + u" (patcher " XVPATCHER_VERSION ")</font>";
        }
        else {
            LogDebug("Invalid version string length");
            // Gestire l'errore o restituire un valore di default
            return;
        }

        SendToAS3(obj, code, version.c_str());
        LogDebug("Patched version string: " + std::string(version.begin(), version.end()));
    }
    catch (std::exception& ex) {
        LogDebug(ex.what());
    }
}

void PatchGameVersionStringPatched(void* obj, int32_t code)
{
    PatchGameVersionString(obj, code);
}

void SetupExternalAS3Callback(ExternalAS3CallbackType orig)
{
    ExternalAS3Callback = orig;
}

std::string GetLatestIggyWarning()
{
    std::lock_guard<std::mutex> lock(iggyMessagesMutex);
    return latestIggyWarning;
}

std::string GetLatestIggyError()
{
    std::lock_guard<std::mutex> lock(iggyMessagesMutex);
    return latestIggyError;
}

int ExternalAS3CallbackPatched(void* custom_arg, void* iggy_obj, const char** pfunc_name)
{
    if (pfunc_name && *pfunc_name)
    {
        const char* func_name = *pfunc_name;

        if (strcmp(func_name, "HelloWorld") == 0)
        {
            HMODULE iggy = GetModuleHandleA("iggy_w32.dll");
            if (!GetModuleHandleExW(GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS | GET_MODULE_HANDLE_EX_FLAG_UNCHANGED_REFCOUNT,
                reinterpret_cast<LPCWSTR>(&ExternalAS3CallbackPatched), &iggy))
            {
                LogDebug("Failed to get the module handle for iggy_w32.dll");
                return 0;
            }

            if (!IggyPlayerCallbackResultPath)
            {
                FARPROC func = GetProcAddress(iggy, "_IggyPlayerCallbackResultPath@4");
                IggyPlayerCallbackResultPath = reinterpret_cast<IggyPlayerCallbackResultPathType>(func);
                func = GetProcAddress(iggy, "_IggyValueSetStringUTF8RS@20");
                IggyValueSetStringUTF8RS = reinterpret_cast<IggyValueSetStringUTF8RSType>(func);
                func = GetProcAddress(iggy, "_IggyValueSetS32RS@16");
                IggyValueSetS32RS = reinterpret_cast<IggyValueSetS32RSType>(func);
            }

            void* ret = IggyPlayerCallbackResultPath(iggy_obj);
            if (!ret)
            {
                LogDebug("IggyPlayerCallbackResultPath returned NULL");
                return 0;
            }

            static const char* hello_world = "Hello world from the native side";
            IggyValueSetStringUTF8RS(ret, nullptr, nullptr, hello_world, strlen(hello_world));
            return 1;
        }
    }

    return ExternalAS3CallbackPatched(custom_arg, iggy_obj, pfunc_name);
}

void CheckIggyMessages()
{
    while (true) {
        // Controlla periodicamente per nuovi errori o warning di Iggy
        {
            std::lock_guard<std::mutex> lock(iggyMessagesMutex);
            if (!latestIggyWarning.empty()) {
                // Esegui azioni in base al nuovo warning di Iggy
                // Ad esempio, puoi loggarlo o visualizzare una notifica
                LogDebug("New Iggy Warning: " + latestIggyWarning);
                latestIggyWarning.clear(); // Resetta il warning
            }
            if (!latestIggyTrace.empty()) {
                // Esegui azioni in base al nuovo trace di Iggy
                // Ad esempio, puoi loggarlo o visualizzare una notifica
                LogDebug("New Iggy Trace: " + latestIggyTrace);
                latestIggyTrace.clear(); // Resetta il trace
            }
            if (!latestIggyError.empty()) {
                // Esegui azioni in base al nuovo errore di Iggy
                // Ad esempio, puoi loggarlo o visualizzare un messaggio di errore
                LogDebug("New Iggy Error: " + latestIggyError);
                latestIggyError.clear(); // Resetta l'errore
            }
        }

        // Aggiungi una breve pausa tra i controlli
        std::this_thread::sleep_for(std::chrono::milliseconds(100));
    }
}

void PatchGameVersionStringPatch()
{
    // Trova il punto in cui inserire la patch e sostituisci le istruzioni
    uintptr_t address = 0x4F7D65; // Indirizzo da patchare

    // Le istruzioni della patch
    const unsigned char patchInstructions[] = {
        0x4C, 0x8D, 0x05, 0x00, 0x00, 0x00, 0x00, // lea r8, '%ls'; comment=version_string
        0xE8, 0x00, 0x00, 0x00, 0x00, // call <send_method>
        0x4C, 0x8B, 0xC3, // mov r8, rbx
        0xBA, 0x04, 0x00, 0x00, 0x00, // mov edx, 4
        0x48, 0x8B, 0xCF, // mov rcx, rdi
        0xE8, 0x00, 0x00, 0x00, 0x00 // call <send_method>
    };

    // Calcola gli offset per le chiamate a <send_method>
    uintptr_t sendMethodOffset1 = 0x10; // L'offset della prima chiamata a <send_method>
    uintptr_t sendMethodOffset2 = 0x1B; // L'offset della seconda chiamata a <send_method>

    // Calcola l'indirizzo della funzione SendToAS3
    SendToAS3Type sendToAS3 = reinterpret_cast<SendToAS3Type>(GetProcAddress(GetModuleHandleA("iggy_w32.dll"), "SendToAS3"));
    if (!sendToAS3) {
        LogDebug("Failed to get the address of SendToAS3 function");
        return;
    }

    // Calcola gli indirizzi delle funzioni di callback
    uintptr_t iggyTraceCallback = reinterpret_cast<uintptr_t>(&iggy_trace_callback);
    uintptr_t iggyWarningCallback = reinterpret_cast<uintptr_t>(&iggy_warning_callback);

    // Calcola gli indirizzi delle istruzioni di chiamata a <send_method>
    uintptr_t sendMethodAddress1 = reinterpret_cast<uintptr_t>(sendToAS3) + sendMethodOffset1;
    uintptr_t sendMethodAddress2 = reinterpret_cast<uintptr_t>(sendToAS3) + sendMethodOffset2;

    // Calcola l'indirizzo della stringa di versione
    std::u16string versionString = u"ver.1.08.00";
    uintptr_t versionStringAddress = reinterpret_cast<uintptr_t>(&versionString[0]);

    // Calcola gli indirizzi delle istruzioni da patchare
    uintptr_t patchAddress1 = address + 0x3;
    uintptr_t patchAddress2 = address + 0xA;

    // Applica la patch sostituendo le istruzioni
    DWORD oldProtection;
    VirtualProtect(reinterpret_cast<void*>(address), sizeof(patchInstructions), PAGE_EXECUTE_READWRITE, &oldProtection);
    memcpy(reinterpret_cast<void*>(address), patchInstructions, sizeof(patchInstructions));
    VirtualProtect(reinterpret_cast<void*>(address), sizeof(patchInstructions), oldProtection, nullptr);

    // Scrivi gli indirizzi delle funzioni di callback e degli indirizzi delle istruzioni di chiamata a <send_method>
    *reinterpret_cast<uintptr_t*>(patchAddress1) = iggyTraceCallback;
    *reinterpret_cast<uintptr_t*>(patchAddress2) = iggyWarningCallback;
    *reinterpret_cast<uintptr_t*>(patchAddress1 + 0x12) = sendMethodAddress1;
    *reinterpret_cast<uintptr_t*>(patchAddress2 + 0x12) = sendMethodAddress2;

    // Aggiorna la versione nella funzione PatchGameVersionStringPatched
    std::string patchedVersion = "<font size=\"9\">v" + ToString(versionString.substr(1)) + " (patcher " XVPATCHER_VERSION ")</font>";
    *reinterpret_cast<uintptr_t*>(patchAddress1 + 0x7) = versionStringAddress;
    *reinterpret_cast<uintptr_t*>(patchAddress2 + 0x7) = versionStringAddress;

    LogDebug("PatchGameVersionString patch applied");
}

std::wstring ConvertTCharToWString(const TCHAR* tcharString)
{
#ifdef UNICODE
    return std::wstring(tcharString);
#else
    int length = MultiByteToWideChar(CP_ACP, 0, tcharString, -1, nullptr, 0);
    if (length == 0)
    {
        throw std::runtime_error("Failed to convert TCHAR to std::wstring.");
    }

    std::vector<wchar_t> buffer(length);
    MultiByteToWideChar(CP_ACP, 0, tcharString, -1, buffer.data(), length);

    return std::wstring(buffer.begin(), buffer.end());
#endif
}


bool GetIggyModule(HANDLE processHandle, HMODULE& iggyModule)
{
    HMODULE hMods[1024];
    DWORD cbNeeded;
    if (EnumProcessModules(processHandle, hMods, sizeof(hMods), &cbNeeded))
    {
        DWORD numModules = cbNeeded / sizeof(HMODULE);
        for (DWORD i = 0; i < numModules; i++)
        {
            TCHAR szModName[MAX_PATH];
            if (GetModuleFileNameEx(processHandle, hMods[i], szModName, sizeof(szModName) / sizeof(TCHAR)))
            {
                std::wstring wideModuleName = ConvertTCharToWString(szModName);
                std::string moduleName(wideModuleName.begin(), wideModuleName.end());
                std::transform(moduleName.begin(), moduleName.end(), moduleName.begin(), ::tolower);
                if (moduleName.find("iggy_w32.dll") != std::string::npos)
                {
                    iggyModule = hMods[i];
                    return true;
                }
            }
        }
    }
    return false;
}

// Implementazione della funzione PatchIggyCallbacks
void PatchIggyCallbacks(HMODULE iggyModule)
{
    FARPROC func = GetProcAddress(iggyModule, "_IggySetTraceCallbackUTF8@8");
    if (!func)
    {
        LogDebug("Failed to get the function address for IggySetTraceCallbackUTF8@8");
        return;
    }
    typedef void (*IggySetTraceCallbackType)(void* callback, void* param);
    IggySetTraceCallbackType iggySetTraceCallback = reinterpret_cast<IggySetTraceCallbackType>(func);
    if (!iggySetTraceCallback)
    {
        LogDebug("Failed to cast the function address for IggySetTraceCallbackUTF8@8");
        return;
    }

    func = GetProcAddress(iggyModule, "_IggySetWarningCallback@8");
    if (!func)
    {
        LogDebug("Failed to get the function address for IggySetWarningCallback@8");
        return;
    }
    typedef void (*IggySetWarningCallbackType)(void* callback, void* param);
    IggySetWarningCallbackType iggySetWarningCallback = reinterpret_cast<IggySetWarningCallbackType>(func);
    if (!iggySetWarningCallback)
    {
        LogDebug("Failed to cast the function address for IggySetWarningCallback@8");
        return;
    }

    iggySetTraceCallback(nullptr, reinterpret_cast<void*>(&iggy_trace_callback));
    iggySetWarningCallback(nullptr, reinterpret_cast<void*>(&iggy_warning_callback));
}

// Implementazione della funzione PatchExternalAS3Callback
void PatchExternalAS3Callback(FARPROC externalAS3CallbackFunc, uintptr_t iggyBaseAddress)
{
    ExternalAS3Callback = reinterpret_cast<ExternalAS3CallbackType>(externalAS3CallbackFunc);

    DWORD oldProtection;
    VirtualProtect(reinterpret_cast<void*>(externalAS3CallbackFunc), sizeof(void*), PAGE_EXECUTE_READWRITE, &oldProtection);
    *reinterpret_cast<uintptr_t*>(externalAS3CallbackFunc) = reinterpret_cast<uintptr_t>(&ExternalAS3CallbackPatched);
    VirtualProtect(reinterpret_cast<void*>(externalAS3CallbackFunc), sizeof(void*), oldProtection, nullptr);

    FARPROC func = GetProcAddress(reinterpret_cast<HMODULE>(iggyBaseAddress), "_IggyPlayerCallbackResultPath@4");
    IggyPlayerCallbackResultPath = reinterpret_cast<IggyPlayerCallbackResultPathType>(func);

    func = GetProcAddress(reinterpret_cast<HMODULE>(iggyBaseAddress), "_IggyValueSetStringUTF8RS@20");
    IggyValueSetStringUTF8RS = reinterpret_cast<IggyValueSetStringUTF8RSType>(func);

    func = GetProcAddress(reinterpret_cast<HMODULE>(iggyBaseAddress), "_IggyValueSetS32RS@16");
    IggyValueSetS32RS = reinterpret_cast<IggyValueSetS32RSType>(func);
}

int main()
{
    try {
        debug = GetIniValue(INI_FILE, "Patches", "debug_patches");

        logFile.open(LOG_FILE, std::ofstream::out | std::ofstream::trunc);
        if (!logFile.is_open()) {
            std::cout << "Failed to open log file: " << LOG_FILE << std::endl;
            return -1;
        }

        LogDebug(std::string("Welcome to XVPatcher! - version ") + XVPATCHER_VERSION);

        // Find the process ID based on the window name
        HWND windowHandle = FindWindowW(nullptr, L"DRAGON BALL XENOVERSE");
        DWORD processId = 0;

        if (!windowHandle) {
            // Launch the DBXV.exe executable
            ShellExecuteW(nullptr, L"open", L"DBXV.exe", nullptr, nullptr, SW_SHOWNORMAL);

            // Wait for 5 seconds
            Sleep(5000);

            // Find the process ID again
            windowHandle = FindWindowW(nullptr, L"DRAGON BALL XENOVERSE");
        }

        if (windowHandle) {
            DWORD processId2 = GetWindowThreadProcessId(windowHandle, &processId);

            // Load the iggy_w32.dll module from the DBXV.exe process
            HANDLE processHandle = OpenProcess(PROCESS_ALL_ACCESS, FALSE, processId);
            if (!processHandle) {
                LogDebug("Failed to open process");
                return -1;
            }

            HMODULE iggyModule = GetModuleHandleA("iggy_w32.dll");
            if (!GetIggyModule(processHandle, iggyModule)) {
                LogDebug("Failed to find iggy_w32.dll module");
                CloseHandle(processHandle);
                return -1;
            }

            PatchIggyCallbacks(iggyModule);
            LogDebug("Patched Iggy callbacks");

            PatchGameVersionStringPatch();
            LogDebug("Patched GameVersionString");

            uintptr_t iggyBaseAddress = reinterpret_cast<uintptr_t>(iggyModule);
            LogDebug("iggy_w32.dll base address: " + std::to_string(iggyBaseAddress));

            FARPROC externalAS3CallbackFunc = GetProcAddress(iggyModule, "ExternalAS3Callback");
            if (!externalAS3CallbackFunc) {
                LogDebug("Failed to get the address of ExternalAS3Callback");
                CloseHandle(processHandle);
                return -1;
            }

            PatchExternalAS3Callback(externalAS3CallbackFunc, iggyBaseAddress);
            LogDebug("Patched ExternalAS3Callback");

            std::thread iggyMessageThread(CheckIggyMessages);
            LogDebug("Started Iggy message thread");

            WaitForSingleObject(processHandle, INFINITE);
            LogDebug("DBXV.exe process exited");

            CloseHandle(processHandle);
            iggyMessageThread.join();
            LogDebug("Cleanup complete");
        }
        else {
            LogDebug("Failed to find the game window");
            logFile.close();
            return -1;
        }

        logFile.close();
    }
    catch (std::exception& ex) {
        std::cout << "Exception occurred: " << ex.what() << std::endl;
        LogDebug("Exception occurred: " + std::string(ex.what()));
        logFile.close();
        return -1;
    }

    return 0;
}
