TARGET = xvpatcher.exe
CC = g++
CFLAGS = -std=c++11 -fpermissive
LDFLAGS = -lwininet -lpsapi -lKernel32 -lUser32 -lWininet

.PHONY: all clean

all: $(TARGET)

$(TARGET): xvpatcher.cpp
	$(CC) $(CFLAGS) xvpatcher.cpp $(LDFLAGS) -o $(TARGET)
	xcopy /Y $(TARGET) "C:\Program Files (x86)\Steam\steamapps\common\DB Xenoverse"

clean:
	del -f $(TARGET)