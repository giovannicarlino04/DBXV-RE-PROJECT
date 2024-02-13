#ifndef CSO_HPP
#define CSO_HPP

#include <iostream>
#include <fstream>
#include <vector>
#include <string>

struct CSO_Data {
    int Char_ID;
    int Costume_ID;
    std::vector<std::string> Paths;
};

class CSO {
private:
    std::vector<CSO_Data> Data;
    std::ifstream br;
    std::ofstream bw;
    std::string FileName;

    std::string TextAtAddress(int Address) {
        long position = br.tellg();
        std::string rText;
        char c;
        if (Address != 0) {
            br.seekg(Address);
            while (br.get(c) && c != '\0') {
                rText += c;
            }
            br.seekg(position);
        }
        return rText;
    }

public:
    void Load(std::string path) {
        br.open(path, std::ios::binary);
        FileName = path;
        br.seekg(8);
        int Count;
        br.read(reinterpret_cast<char*>(&Count), sizeof(Count));
        Data.resize(Count);
        int Offset;
        br.read(reinterpret_cast<char*>(&Offset), sizeof(Offset));

        for (int i = 0; i < Count; i++) {
            br.seekg(Offset + (32 * i));
            br.read(reinterpret_cast<char*>(&Data[i].Char_ID), sizeof(Data[i].Char_ID));
            br.read(reinterpret_cast<char*>(&Data[i].Costume_ID), sizeof(Data[i].Costume_ID));
            Data[i].Paths.resize(4);
            for (int j = 0; j < 4; j++) {
                Data[i].Paths[j] = TextAtAddress(br.readInt32());
            }
        }
        br.close();
    }

    void Save() {
        std::vector<std::string> CmnText;
        for (auto& character : Data) {
            for (const auto& path : character.Paths) {
                if (std::find(CmnText.begin(), CmnText.end(), path) == CmnText.end()) {
                    CmnText.push_back(path);
                }
            }
        }

        int wordAddress[CmnText.size()];
        int wordstartposition = 16 + (Data.size() * 32);
        bw.open(FileName, std::ios::binary);
        bw.write("#CSO\xFE\xFF\0\0", 8);
        int Count = Data.size();
        bw.write(reinterpret_cast<const char*>(&Count), sizeof(Count));
        bw.write(reinterpret_cast<const char*>(&wordstartposition), sizeof(wordstartposition));

        bw.seekp(wordstartposition);
        for (size_t i = 0; i < CmnText.size(); i++) {
            wordAddress[i] = bw.tellp();
            bw.write(CmnText[i].c_str(), CmnText[i].size());
            bw.write("\0", 1);
        }

        for (size_t i = 0; i < Data.size(); i++) {
            bw.seekp(16 + (32 * i));
            bw.write(reinterpret_cast<const char*>(&Data[i].Char_ID), sizeof(Data[i].Char_ID));
            bw.write(reinterpret_cast<const char*>(&Data[i].Costume_ID), sizeof(Data[i].Costume_ID));
            for (int j = 0; j < 4; j++) {
                bw.write(reinterpret_cast<const char*>(&wordAddress[std::distance(CmnText.begin(), std::find(CmnText.begin(), CmnText.end(), Data[i].Paths[j]))]), sizeof(int));
            }
        }
        bw.close();
    }

    int DataExist(int id, int c) {
        for (int i = 0; i < Data.size(); i++) {
            if (Data[i].Char_ID == id && Data[i].Costume_ID == c)
                return i;
        }

        for (int j = 0; j < Data.size(); j++) {
            if (Data[j].Char_ID == id)
                return j;
        }

        return -1;
    }

    void AddCharacter(CSO_Data character) {
        int existingIndex = DataExist(character.Char_ID, character.Costume_ID);

        if (existingIndex >= 0) {
            Data[existingIndex] = character;
        }
        else {
            Data.push_back(character);
        }

        Save();
    }
};
#endif