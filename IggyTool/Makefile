CXX=g++
CXXFLAGS=-std=c++11 -Wall -Wextra -pedantic

PROGRAM=extractor

SRCS=main.cpp

OBJS=$(SRCS:.cpp=.o)

.PHONY: all clean

all: $(PROGRAM)

$(PROGRAM): $(OBJS)
	$(CXX) $(CXXFLAGS) -o $(PROGRAM) $(OBJS)

main.o: main.cpp
	$(CXX) $(CXXFLAGS) -c main.cpp

clean:
	rm -f $(PROGRAM) $(OBJS)