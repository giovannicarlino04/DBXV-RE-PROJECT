#ifndef CMS_HPP
#define CMS_HPP

#include <iostream>
#include <fstream>
#include <vector>
#include <string>
#include <cereal/external/rapidxml/rapidxml.hpp>

struct CharacterData {
    int ID;
    std::string ShortName;
    std::vector<char> Unknown;
    std::vector<std::string> Paths;
};

class CMS {
private:
    std::vector<CharacterData> Data;
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
            Data[i].ID;
            br.seekg(Offset + (80 * i));
            br.read(reinterpret_cast<char*>(&Data[i].ID), sizeof(Data[i].ID));
            char ShortNameBuf[3];
            br.read(ShortNameBuf, sizeof(ShortNameBuf));
            Data[i].ShortName.assign(ShortNameBuf, 3);
            br.seekg(9, std::ios::cur);
            Data[i].Unknown.resize(8);
            br.read(reinterpret_cast<char*>(&Data[i].Unknown[0]), sizeof(char) * 8);
            br.seekg(8, std::ios::cur);
            Data[i].Paths.resize(7);
            for (int j = 0; j < 7; j++) {
                int address;
                br.read(reinterpret_cast<char*>(&address), sizeof(address));
                Data[i].Paths[j] = TextAtAddress(address);
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
        int wordstartposition = 16 + (Data.size() * 80);
        bw.open(FileName, std::ios::binary);
        bw.write("#CMS\xFE\xFF\0\0", 8);
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
            bw.seekp(16 + (80 * i));
            bw.write(reinterpret_cast<const char*>(&Data[i].ID), sizeof(Data[i].ID));
            bw.write(Data[i].ShortName.c_str(), 3);
            bw.seekp(9, std::ios::cur);
            bw.write(reinterpret_cast<const char*>(&Data[i].Unknown[0]), sizeof(char) * 8);
            bw.write("\xFF\xFF", 2);
            bw.seekp(6, std::ios::cur);
            for (int j = 0; j < 7; j++) {
                bw.write(reinterpret_cast<const char*>(&wordAddress[std::distance(CmnText.begin(), std::find(CmnText.begin(), CmnText.end(), Data[i].Paths[j]))]), sizeof(int));
            }
        }
        bw.close();
    }

    void AddCharacter(CharacterData character) {
        if (Data.empty()) {
            std::cout << "CMS data is not loaded." << std::endl;
            return;
        }

        Data.push_back(character);
        Save();
    }

        void SerializeToXML(const std::string& xmlFileName) {
        rapidxml::xml_document<> doc;
        rapidxml::xml_node<>* rootNode = doc.allocate_node(rapidxml::node_element, "CMS");

        for (const auto& character : Data) {
            rapidxml::xml_node<>* charNode = doc.allocate_node(rapidxml::node_element, "Character");

            rapidxml::xml_node<>* idNode = doc.allocate_node(rapidxml::node_element, "ID", std::to_string(character.ID).c_str());
            charNode->append_node(idNode);

            rapidxml::xml_node<>* shortNameNode = doc.allocate_node(rapidxml::node_element, "ShortName", character.ShortName.c_str());
            charNode->append_node(shortNameNode);

            // Add other data nodes...

            for (const auto& path : character.Paths) {
                rapidxml::xml_node<>* pathNode = doc.allocate_node(rapidxml::node_element, "Path", path.c_str());
                charNode->append_node(pathNode);
            }

            rootNode->append_node(charNode);
        }

        doc.append_node(rootNode);

        std::ofstream file(xmlFileName);
        file << doc;
        file.close();
    }

    void DeserializeFromXML(const std::string& xmlFileName) {
        rapidxml::xml_document<> doc;
        std::ifstream file(xmlFileName);
        std::vector<char> buffer((std::istreambuf_iterator<char>(file)), std::istreambuf_iterator<char>());
        buffer.push_back('\0');
        doc.parse<0>(&buffer[0]);

        rapidxml::xml_node<>* rootNode = doc.first_node("CMS");
        if (rootNode) {
            for (rapidxml::xml_node<>* charNode = rootNode->first_node("Character"); charNode; charNode = charNode->next_sibling()) {
                CharacterData character;
                character.ID = std::stoi(charNode->first_node("ID")->value());
                character.ShortName = charNode->first_node("ShortName")->value();

                // Parse other data nodes...

                for (rapidxml::xml_node<>* pathNode = charNode->first_node("Path"); pathNode; pathNode = pathNode->next_sibling()) {
                    character.Paths.push_back(pathNode->value());
                }

                Data.push_back(character);
            }
        }
    }
};
#endif