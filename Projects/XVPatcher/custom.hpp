#ifndef CUSTOM_HPP
#define CUSTOM_HPP

#include <cstdarg>
#include <iostream>
#include <fstream>
#include <ostream>
#include <map>
#include "debug.h"
#include "symbols.h"
// Custom implementation of DPRINTF to redirect output to console
void CustomDPRINTF(const char* format, ...) {
    // Format the variable arguments using vsprintf and print to std::cout
    va_list args;
    va_start(args, format);

    // Format the output
    char buffer[512]; // Adjust buffer size as needed
    vsnprintf(buffer, sizeof(buffer), format, args);

    // Output the formatted string to console
    std::cout << "Debug: " << buffer << std::endl;

    va_end(args);
}

std::map<std::string, std::string> readIniFile(const std::string& filename) {
    
    std::map<std::string, std::string> iniValues;
    std::ifstream file(filename);

    if (!file.is_open()) {
        UPRINTF("Error opening ini file, please install the patcher correctly.");
        return;
    }

    std::string line;
    std::string currentSection;

    while (std::getline(file, line)) {
        // Remove leading and trailing whitespaces
        line.erase(0, line.find_first_not_of(" \t\r\n"));
        line.erase(line.find_last_not_of(" \t\r\n") + 1);

        if (line.empty() || line[0] == ';') {
            // Skip empty lines or comments starting with ';'
            continue;
        } else if (line[0] == '[' && line[line.length() - 1] == ']') {
            // New section
            currentSection = line.substr(1, line.length() - 2);
        } else {
            // Key-value pair
            size_t separatorPos = line.find('=');

            if (separatorPos != std::string::npos) {
                std::string key = line.substr(0, separatorPos);
                std::string value = line.substr(separatorPos + 1);

                // Remove leading and trailing whitespaces from key and value
                key.erase(0, key.find_first_not_of(" \t\r\n"));
                key.erase(key.find_last_not_of(" \t\r\n") + 1);
                value.erase(0, value.find_first_not_of(" \t\r\n"));
                value.erase(value.find_last_not_of(" \t\r\n") + 1);

                iniValues[currentSection + "." + key] = value;
            } else {
                std::cerr << "Invalid line in INI file: " << line << std::endl;
            }
        }
    }

    file.close();

    return iniValues;
}
#endif