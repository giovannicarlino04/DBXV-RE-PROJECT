//#define DLL_EXPORT WINAPI __declspec(dllexport)
#define DLL_EXPORT

typedef DLL_EXPORT void (* DummyFunction)(void);

extern "C"
{
	DLL_EXPORT void SteamAPI_SetMiniDumpComment(void);
	
	DLL_EXPORT void SteamAPI_WriteMiniDump(void);
	DLL_EXPORT void SteamUserStats(void);
	DLL_EXPORT void SteamAPI_UnregisterCallResult(void);
	DLL_EXPORT void SteamAPI_RegisterCallResult(void);
	DLL_EXPORT void SteamAPI_RunCallbacks(void);
	DLL_EXPORT void SteamAPI_RegisterCallback(void);
	DLL_EXPORT void SteamAPI_UnregisterCallback(void);
	DLL_EXPORT void SteamMatchmaking(void);
	DLL_EXPORT void SteamUtils(void);
	DLL_EXPORT void SteamUser(void);
	DLL_EXPORT void SteamFriends(void);
	DLL_EXPORT void SteamRemoteStorage(void);
	DLL_EXPORT void SteamClient(void);
	DLL_EXPORT void SteamAPI_Init(void);
	DLL_EXPORT void SteamAPI_Shutdown(void);
	DLL_EXPORT void SteamNetworking(void);
	DLL_EXPORT void SteamApps(void);
	
}


