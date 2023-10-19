TARGET = xvpatcher.exe
CC = g++
CFLAGS = -std=c++11 -m32 -fpermissive
LDFLAGS = -lwininet -lpsapi -lKernel32 -lUser32 -lWininet

.PHONY: all clean

all: $(TARGET)

$(TARGET): xvpatcher.cpp
	$(CC) $(CFLAGS) xvpatcher.cpp $(LDFLAGS) -o $(TARGET)

clean:
	del -f $(TARGET)