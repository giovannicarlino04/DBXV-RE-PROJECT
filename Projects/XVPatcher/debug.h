#ifndef __DEFINE_DEBUG_H__
#define __DEFINE_DEBUG_H__

//#define DEBUG

#ifdef DEBUG
#define DPRINTF	DebugPrintf
#else
#define DPRINTF(...)
#endif

#define UPRINTF	UserPrintf

int __attribute__ ((format (printf, 1, 2))) DebugPrintf(const char* fmt, ...);
int __attribute__ ((format (printf, 1, 2))) UserPrintf(const char* fmt, ...);

#ifdef DEBUG

void debug_patches();
//int __attribute__ ((format (printf, 2, 3))) Dprintf1(int unk, const char* fmt, ...);

#define BRA	__builtin_return_address

static void __forceinline DumpStackTrace10(void)
{
	void *addrs[10];
	
	addrs[0] = BRA(0);
	addrs[1] = BRA(1);
	addrs[2] = BRA(2);
	addrs[3] = BRA(3);
	addrs[4] = BRA(4);
	addrs[5] = BRA(5);
	addrs[6] = BRA(6);
	addrs[7] = BRA(7);
	addrs[8] = BRA(8);
	addrs[9] = BRA(9);
	
	for (int i = 0; i < 10; i++)
	{
		DPRINTF("Called from %p\n", addrs[i]);
	}
}

static void __forceinline DumpStackTrace8(void)
{
	void *addrs[10];
	
	addrs[0] = BRA(0);
	addrs[1] = BRA(1);
	addrs[2] = BRA(2);
	addrs[3] = BRA(3);
	addrs[4] = BRA(4);
	addrs[5] = BRA(5);
	addrs[6] = BRA(6);
	addrs[7] = BRA(7);
	//addrs[8] = BRA(8);
	//addrs[9] = BRA(9);
	
	for (int i = 0; i < 8; i++)
	{
		DPRINTF("Called from %p\n", addrs[i]);
	}
}

static void __forceinline DumpStackTrace7(void)
{
	void *addrs[10];
	
	addrs[0] = BRA(0);
	addrs[1] = BRA(1);
	addrs[2] = BRA(2);
	addrs[3] = BRA(3);
	addrs[4] = BRA(4);
	addrs[5] = BRA(5);
	addrs[6] = BRA(6);
	//addrs[7] = BRA(7);
	//addrs[8] = BRA(8);
	//addrs[9] = BRA(9);
	
	for (int i = 0; i < 7; i++)
	{
		DPRINTF("Called from %p\n", addrs[i]);
	}
}

static void __forceinline DumpStackTrace6(void)
{
	void *addrs[10];
	
	addrs[0] = BRA(0);
	addrs[1] = BRA(1);
	addrs[2] = BRA(2);
	addrs[3] = BRA(3);
	addrs[4] = BRA(4);
	addrs[5] = BRA(5);
	//addrs[6] = BRA(6);
	//addrs[7] = BRA(7);
	//addrs[8] = BRA(8);
	//addrs[9] = BRA(9);
	
	for (int i = 0; i < 6; i++)
	{
		DPRINTF("Called from %p\n", addrs[i]);
	}
}

static void __forceinline DumpStackTrace5(void)
{
	void *addrs[10];
	
	addrs[0] = BRA(0);
	addrs[1] = BRA(1);
	addrs[2] = BRA(2);
	addrs[3] = BRA(3);
	addrs[4] = BRA(4);
	//addrs[5] = BRA(5);
	//addrs[6] = BRA(6);
	//addrs[7] = BRA(7);
	//addrs[8] = BRA(8);
	//addrs[9] = BRA(9);
	
	for (int i = 0; i < 5; i++)
	{
		DPRINTF("Called from %p\n", addrs[i]);
	}
}

static void __forceinline DumpStackTrace4(void)
{
	void *addrs[10];
	
	addrs[0] = BRA(0);
	addrs[1] = BRA(1);
	addrs[2] = BRA(2);
	addrs[3] = BRA(3);
	//addrs[4] = BRA(4);
	//addrs[5] = BRA(5);
	//addrs[6] = BRA(6);
	//addrs[7] = BRA(7);
	//addrs[8] = BRA(8);
	//addrs[9] = BRA(9);
	
	for (int i = 0; i < 4; i++)
	{
		DPRINTF("Called from %p\n", addrs[i]);
	}
}

static void __forceinline DumpStackTrace3(void)
{
	void *addrs[10];
	
	addrs[0] = BRA(0);
	addrs[1] = BRA(1);
	addrs[2] = BRA(2);
	//addrs[3] = BRA(3);
	//addrs[4] = BRA(4);
	//addrs[5] = BRA(5);
	//addrs[6] = BRA(6);
	//addrs[7] = BRA(7);
	//addrs[8] = BRA(8);
	//addrs[9] = BRA(9);
	
	for (int i = 0; i < 3; i++)
	{
		DPRINTF("Called from %p\n", addrs[i]);
	}
}

static void __forceinline DumpStackTrace2(void)
{
	void *addrs[10];
	
	addrs[0] = BRA(0);
	addrs[1] = BRA(1);
	//addrs[2] = BRA(2);
	//addrs[3] = BRA(3);
	//addrs[4] = BRA(4);
	//addrs[5] = BRA(5);
	//addrs[6] = BRA(6);
	//addrs[7] = BRA(7);
	//addrs[8] = BRA(8);
	//addrs[9] = BRA(9);
	
	for (int i = 0; i < 2; i++)
	{
		DPRINTF("Called from %p\n", addrs[i]);
	}
}

static void __forceinline DumpStackTrace1(void)
{
	void *addrs[10];
	
	addrs[0] = BRA(0);
	//addrs[1] = BRA(1);
	//addrs[2] = BRA(2);
	//addrs[3] = BRA(3);
	//addrs[4] = BRA(4);
	//addrs[5] = BRA(5);
	//addrs[6] = BRA(6);
	//addrs[7] = BRA(7);
	//addrs[8] = BRA(8);
	//addrs[9] = BRA(9);
	
	for (int i = 0; i < 1; i++)
	{
		DPRINTF("Called from %p\n", addrs[i]);
	}
}

#endif

#endif
