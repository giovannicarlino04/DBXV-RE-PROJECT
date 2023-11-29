CC := gcc
CXX := g++
OUTPUT := xinput1_3.dll
OUTPUT_COPY := "C:/Program Files (x86)/Steam/steamapps/common/DB Xenoverse/$(OUTPUT)"
OBJS := main.o debug.o CpkFile.o BaseFile.o patch.o PatchUtils.o Utils.o MemoryStream.o FixedMemoryStream.o Stream.o IggyFile.o
OBJS += ./crypto/sha1.o ./crypto/md5.o ./crypto/rijndael.o
OBJS += ./tinyxml/tinystr.o ./tinyxml/tinyxml.o ./tinyxml/tinyxmlerror.o ./tinyxml/tinyxmlparser.o
CFLAGS := -Wall -I. -I./ -std=gnu99 -mms-bitfields -s -O2 -masm=intel -shared -Wl,--subsystem,windows,--kill-at,--enable-stdcall-fixup -DNO_ZLIB
CPPFLAGS := -Wall -O2 -mno-ms-bitfields -std=c++11 -DTIXML_USE_STL -DNO_ZLIB
CXXFLAGS := -Wall -Wno-strict-aliasing -I./ -O2 -std=c++11 -mms-bitfields -DTIXML_USE_STL -fpermissive
LDFLAGS := -L. -static xinput.def
LIBS := -lstdc++ -lversion -lMinHook -lxinput1_3 -lPsapi

.PHONY: all clean

all: $(OUTPUT)

clean:
	rm -f $(OUTPUT) *.o

$(OUTPUT): $(OBJS)
	$(CXX) $(CFLAGS) $(LDFLAGS) -o $@ $^ $(LIBS)
	cp $(OUTPUT) $(OUTPUT_COPY)