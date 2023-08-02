CXX = g++
CXXFLAGS = -std=c++11 -Wall -Wextra -fpermissive
LDFLAGS = -lpsapi -lkernel32

SRCS = main.cpp
OBJS = $(SRCS:.cpp=.o)
EXECUTABLE = XVPatcher.exe
INSTALL_DIR = C:\Program Files (x86)\Steam\steamapps\common\DB Xenoverse

.PHONY: all clean install

all: $(EXECUTABLE)

$(EXECUTABLE): $(OBJS)
	$(CXX) $(LDFLAGS) $(OBJS) -o $(EXECUTABLE)

%.o: %.cpp
	$(CXX) $(CXXFLAGS) -c $< -o $@

install: $(EXECUTABLE)
	cp $(EXECUTABLE) "$(INSTALL_DIR)"

clean:
	rm -f $(OBJS) $(EXECUTABLE)
