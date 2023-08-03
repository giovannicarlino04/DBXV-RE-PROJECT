#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <windows.h>

#include "xv2patcher.h"
#include "debug.h"

#define DEBUG_BUFFER_SIZE	384
#define DEBUG_FILE	"../XV2PATCHER/xv2_log.txt"

static int debug_level;
static FILE *file;

int set_debug_level(int level)
{
	int ret = debug_level;
	debug_level = level;
	return ret;
}

int mod_debug_level(int mod_by)
{
	return set_debug_level(debug_level+mod_by);
}

int __attribute__ ((format (printf, 1, 2))) DebugPrintf(const char* fmt, ...) 
{
	if (debug_level == 0)
		return 0;
	
	char *dbg;
	va_list ap;

	dbg = (char *)malloc(DEBUG_BUFFER_SIZE);
	if (!dbg)
		return 0;
		
	va_start(ap, fmt);
	size_t len = vsnprintf(dbg, DEBUG_BUFFER_SIZE, fmt, ap);
	va_end(ap);
	
	if (debug_level == 1)
	{
		OutputDebugStringA(dbg);
	}
	else if (debug_level == 2)
	{
		if (!file)
		{
			std::string file_path = myself_path + DEBUG_FILE;		
			file = fopen(file_path.c_str(), "w");
			if (!file)
				return 0;
		}

		fputs(dbg, file);
		fflush(file);
	}
	else
	{
		MessageBoxA(NULL, dbg, "XV2 Patcher", 0);
	}
	
	free(dbg);
	return len;
}

int __attribute__ ((format (printf, 1, 2))) UserPrintf(const char* fmt, ...) 
{
	char *dbg;
	va_list ap;

	dbg = (char *)malloc(DEBUG_BUFFER_SIZE);
	if (!dbg)
		return 0;
		
	va_start(ap, fmt);
	size_t len = vsnprintf(dbg, DEBUG_BUFFER_SIZE, fmt, ap);
	va_end(ap);

	MessageBoxA(NULL, dbg, "XV2 Patcher", 0);
	free(dbg);
	return len;
}

int __attribute__ ((format (printf, 1, 2))) FilePrintf(const char* fmt, ...) 
{
	char *dbg;
	va_list ap;

	dbg = (char *)malloc(DEBUG_BUFFER_SIZE);
	if (!dbg)
		return 0;
		
	va_start(ap, fmt);
	size_t len = vsnprintf(dbg, DEBUG_BUFFER_SIZE, fmt, ap);
	va_end(ap);
	
	if (!file)
	{
		std::string file_path = myself_path + DEBUG_FILE;		
		file = fopen(file_path.c_str(), "w");
		if (!file)
			return 0;
	}

	fputs(dbg, file);
	fflush(file);
	
	free(dbg);
	return len;
}

extern "C"
{

PUBLIC int __attribute__ ((format (printf, 2, 3))) DprintfPatched(uint32_t unk, const char* fmt, ...) 
{
	if (debug_level == 0)
		return 0;
	
	char *dbg;
	va_list ap;

	dbg = (char *)malloc(DEBUG_BUFFER_SIZE);
	if (!dbg)
		return 0;
		
	va_start(ap, fmt);
	size_t len = vsnprintf(dbg, DEBUG_BUFFER_SIZE, fmt, ap);
	va_end(ap);
	
	if (debug_level == 1)
	{
		OutputDebugStringA(dbg);
	}
	else if (debug_level == 2)
	{
		if (!file)
		{
			std::string file_path = myself_path + DEBUG_FILE;		
			file = fopen(file_path.c_str(), "w");
			if (!file)
				return 0;
		}

		fputs(dbg, file);
		fflush(file);
	}
	else
	{
		MessageBoxA(NULL, dbg, "XV2 Patcher", 0);
	}
	
	free(dbg);
	return len;
}

} // extern "C"