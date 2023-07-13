TARGET = xvpatcher.exe
CC = g++
CFLAGS = -std=c++11
LDFLAGS = -lwininet

.PHONY: all clean

all: $(TARGET)

$(TARGET): xvpatcher.cpp
	$(CC) $(CFLAGS) xvpatcher.cpp $(LDFLAGS) -o $(TARGET)
	xcopy /Y $(TARGET) "C:\Program Files (x86)\Steam\steamapps\common\DB Xenoverse"

clean:
	del -f $(TARGET)