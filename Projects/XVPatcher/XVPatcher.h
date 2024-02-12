#ifndef XVPATCHER_H
#define XVPATCHER_H

#define EXPORT WINAPI __declspec(dllexport)
#define PUBLIC EXPORT

#define NUM_EXPORTED_FUNCTIONS	18

#define EXE_PATH	"DBXV.exe"
#define PROCESS_NAME 	"dbxv.exe"
#define DATA_CPK		"data.cpk"
#define DATA2_CPK		"data2.cpk"
#define DATAP1_CPK		"datap1.cpk"
#define DATAP2_CPK		"datap2.cpk"
#define DATAP3_CPK		"datap3.cpk"

#define XVPATCHER_VERSION "1.04"

#define MINIMUM_GAME_VERSION	1.00f
#define INI_FILE "./XVPatcher/XVPatcher.ini"


typedef void (* IGGYSetTraceCallbackType)(void *callback, void *param);
extern void iggy_trace_callback(void *param, void *unk, const char *str, size_t len);
#endif