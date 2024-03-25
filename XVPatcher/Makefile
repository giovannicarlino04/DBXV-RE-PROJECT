CC := i686-w64-mingw32-gcc
CXX := i686-w64-mingw32-g++

OUTPUT := xinput1_3.dll

OBJS := main.o
CFLAGS := -I. -I./
CPPFLAGS := -Wall -I./
LDFLAGS := -Wl,-static --enable-stdcall-fixup -shared
LIBS := -lxinput1_3 -lstdc++ -lversion -lpthread -lMinHook -static-libgcc

.PHONY: all clean

all: $(OUTPUT)

clean:
	rm -f $(OUTPUT) *.o

$(OUTPUT): $(OBJS)
	$(CXX) $(CFLAGS) $(CPPFLAGS) $(LDFLAGS) -o $@ $^ $(LIBS)
