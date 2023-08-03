#ifndef XV2PATCHER
#define XV2PATCHER

#include "IniFile.h"

#define EXPORT 	__declspec(dllexport)
#define PUBLIC	EXPORT

#define PROCESS_NAME "dbxv.exe"

#define INI_PATH		"./XVPATCHER/xvpatcher.ini"
#define PATCHES_PATH	"./XVPATCHER/EPatches"

#define CONTENT_ROOT	"./"

extern std::string myself_path;
extern IniFile ini;

#endif // XVPATCHER
