#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <windows.h>

#include "debug.h"
#include "patch.h"
#include "symbols.h"

#define DEBUG_BUFFER_SIZE	256

int __attribute__ ((format (printf, 1, 2))) DebugPrintf(const char* fmt, ...) 
{
	char *dbg;
	va_list ap;

	dbg = (char *)malloc(DEBUG_BUFFER_SIZE);
	if (!dbg)
		return 0;
		
	va_start(ap, fmt);
	size_t len = vsnprintf(dbg, DEBUG_BUFFER_SIZE, fmt, ap);
	va_end(ap);

	OutputDebugString(dbg);
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

	MessageBox(NULL, dbg, "XVPatcher", 0);
	free(dbg);
	return len;
}

int __attribute__ ((format (printf, 2, 3))) Dprintf1(int unk, const char* fmt, ...) 
{
	char *dbg;
	va_list ap;

	dbg = (char *)malloc(DEBUG_BUFFER_SIZE);
	if (!dbg)
		return 0;
		
	va_start(ap, fmt);
	size_t len = vsnprintf(dbg, DEBUG_BUFFER_SIZE, fmt, ap);
	va_end(ap);

	OutputDebugString(dbg);
	free(dbg);
	return len;
}

/*int __attribute__ ((format (printf, 2, 6))) DprintfC(int unk, const char* fmt, int, int, int, ...) 
{
	char *dbg;
	va_list ap;

	dbg = (char *)malloc(DEBUG_BUFFER_SIZE);
	if (!dbg)
		return 0;
		
	va_start(ap, fmt);
	size_t len = vsnprintf(dbg, DEBUG_BUFFER_SIZE, fmt, ap);
	va_end(ap);

	OutputDebugString(dbg);
	free(dbg);
	return len;
}*/

void debug_patches()
{
#ifdef DEBUG

#ifndef XENOVERSE
	HookFunction(DPRINTF1_SYMBOL, NULL, (void *)Dprintf1);
	HookFunction(DPRINTF2_SYMBOL, NULL, (void *)Dprintf1);
	//HookFunction(0x18AEBBB-MY_DUMP_BASE, NULL, (void *)DprintfC);
	
#endif

#endif
}

