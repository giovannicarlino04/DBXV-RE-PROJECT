CC = gcc
CXX = g++

OUTPUT := xinput1_3.dll
OBJS=main.o debug.o CpkFile.o BaseFile.o patch.o PatchUtils.o Utils.o
OBJS += ./crypto/sha1.o ./crypto/md5.o ./crypto/rijndael.o
OBJS += ./tinyxml/tinystr.o ./tinyxml/tinyxml.o ./tinyxml/tinyxmlerror.o ./tinyxml/tinyxmlparser.o
CFLAGS +=-Wall -I. -I./ -std=gnu99 -mms-bitfields -s -O2 -masm=intel -shared -Wl,--subsystem,windows,--kill-at,--enable-stdcall-fixup
CFLAGS += -static -static-libgcc -static-libstdc++
CPPFLAGS=-Wall -O2 -mno-ms-bitfields 
CPPFLAGS += -static-libgcc -static-libstdc++ -static -Wl,--subsystem,windows,--kill-at
CXXFLAGS +=-Wall -Wno-strict-aliasing -I./ -O2 -std=c++11 -mms-bitfields -DTIXML_USE_STL
CXXFLAGS += -static-libgcc -static-libstdc++ -static -Wl,--subsystem,windows,--kill-at
CPPFLAGS += -DNO_ZLIB 
LDFLAGS=-L. -static xinput.def
LIBS = -lstdc++ -lversion -lMinHook -lxinput1_3 -lPsapi

all: $(OUTPUT)

clean:
	rm -f $(OUTPUT) *.o

$(OUTPUT): $(OBJS)
	$(LINK.c) $(LDFLAGS) -o $@ $^ $(LIBS)
