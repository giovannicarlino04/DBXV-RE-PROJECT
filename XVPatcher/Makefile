CC := i686-w64-mingw32-gcc
CXX := i686-w64-mingw32-g++

OUTPUT := xinput1_3.dll

OBJS := main.o common.o patches.o
CFLAGS := -Wall -I. -I./ -std=gnu99 -mms-bitfields -s -O2 -masm=intel -shared -Wl,--subsystem,windows,--kill-at,--enable-stdcall-fixup -DNO_ZLIB -fpermissive
CPPFLAGS := -Wall -O2 -mno-ms-bitfields -std=c++17 -DTIXML_USE_STL -DNO_ZLIB
CXXFLAGS := -Wall -Wno-strict-aliasing -I./ -O2 -std=c++17 -mms-bitfields -DTIXML_USE_STL -fpermissive
LDFLAGS := -static xinput.def
LIBS := -lxinput1_3 -lstdc++ -lversion -lpthread -lMinHook

.PHONY: all clean

all: $(OUTPUT)

clean:
	rm -f $(OUTPUT) *.o

$(OUTPUT): $(OBJS)
	$(CXX) $(CFLAGS) $(CPPFLAGS) $(LDFLAGS) -o $@ $^ $(LIBS)
