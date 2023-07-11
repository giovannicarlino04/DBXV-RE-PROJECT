CC = gcc
CXX = g++

OUTPUT := steam_api.dll
OBJS=main.o debug.o CpkFile.o BaseFile.o patch.o asm.o
CFLAGS=-Wall -I. -std=gnu99 -mno-ms-bitfields -D_LARGEFILE64_SOURCE -D_FILE_OFFSET_BITS=64 -O2 -masm=intel -shared -s -Wl,--subsystem,windows,--kill-at,--enable-stdcall-fixup
CFLAGS += -static -static-libgcc -static-libstdc++
CFLAGS += -I/C:/mingw32/include
CPPFLAGS=-Wall -O2 -mno-ms-bitfields 
CPPFLAGS += -static-libgcc -static-libstdc++ -static -Wl,--subsystem,windows,--kill-at -fpermissive
LDFLAGS=-L./C:/mingw32/lib -static
LIBS = -lstdc++ -lMinHook -lpsapi

all: $(OUTPUT)

clean:
	del /F /Q $(OUTPUT) *.o

$(OUTPUT): $(OBJS)
	$(LINK.c) $(LDFLAGS) -o $@ $^ $(LIBS)
	copy $(OUTPUT) "C:\Program Files (x86)\Steam\steamapps\common\DB Xenoverse"