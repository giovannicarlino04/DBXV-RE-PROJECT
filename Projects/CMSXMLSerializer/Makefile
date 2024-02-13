CXX = i686-mingw32-w64-g++
CXXFLAGS = -std=c++11 -Wall -Wextra -O2
LIBS = -lrapidxml

SRCS = main.cpp CMS.hpp
OBJS = $(SRCS:.cpp=.o)

TARGET = CMSXMLSerializer

all: $(TARGET)

$(TARGET): $(OBJS)
	$(CXX) $(CXXFLAGS) -o $@ $^ $(LIBS)

%.o: %.cpp
	$(CXX) $(CXXFLAGS) -c -o $@ $<

clean:
	rm -f $(OBJS) $(TARGET)
