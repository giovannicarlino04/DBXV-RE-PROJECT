#ifndef CUS_HPP
#define CUS_HPP
#include <iostream>
#include <fstream>
#include <vector>
#include <string>

struct skill {
    std::string Name;
    short ID;
};

struct Char_Data {
    int charID;
    int CostumeID;
    short SuperIDs[4];
    short UltimateIDs[2];
    short EvasiveID;
};

class CharSkill {
private:
    std::string FileName;
    std::vector<skill> Supers;
    std::vector<skill> Ultimates;
    std::vector<skill> Evasives;
    int CharCount = 0;
    int CharAddress = 0;
    std::vector<Char_Data> Chars;
    std::vector<std::string> mText;

    std::string findName(const std::string& text_ID) {
        for (const auto& data : mText) {
            if (data.find(text_ID) != std::string::npos)
                return data.substr(data.find("=") + 1);
        }

        return "Unknown Skill";
    }

public:
    void populateSkillData(const std::string& msgFolder, const std::string& CUSFile, const std::string& lang) {
        FileName = CUSFile;
        std::ifstream CUS(CUSFile, std::ios::binary);
        CUS.seekg(8);
        CUS.read(reinterpret_cast<char*>(&CharCount), sizeof(CharCount));
        CUS.read(reinterpret_cast<char*>(&CharAddress), sizeof(CharAddress));

        int SuperCount, UltimateCount, EvasiveCount;
        CUS.read(reinterpret_cast<char*>(&SuperCount), sizeof(SuperCount));
        CUS.read(reinterpret_cast<char*>(&UltimateCount), sizeof(UltimateCount));
        CUS.read(reinterpret_cast<char*>(&EvasiveCount), sizeof(EvasiveCount));
        CUS.seekg(8, std::ios::cur);

        int SupAddress, UltAddress, EvaAddress;
        CUS.read(reinterpret_cast<char*>(&SupAddress), sizeof(SupAddress));
        CUS.read(reinterpret_cast<char*>(&UltAddress), sizeof(UltAddress));
        CUS.read(reinterpret_cast<char*>(&EvaAddress), sizeof(EvaAddress));

        Chars.resize(CharCount);
        for (int i = 0; i < CharCount; i++) {
            CUS.seekg(CharAddress + (i * 32), std::ios::beg);
            CUS.read(reinterpret_cast<char*>(&Chars[i].charID), sizeof(Chars[i].charID));
            CUS.read(reinterpret_cast<char*>(&Chars[i].CostumeID), sizeof(Chars[i].CostumeID));

            CUS.read(reinterpret_cast<char*>(&Chars[i].SuperIDs), sizeof(Chars[i].SuperIDs));
            CUS.read(reinterpret_cast<char*>(&Chars[i].UltimateIDs), sizeof(Chars[i].UltimateIDs));
            CUS.read(reinterpret_cast<char*>(&Chars[i].EvasiveID), sizeof(Chars[i].EvasiveID));
        }

        Supers.resize(SuperCount);
        mText = loadMsgFile(msgFolder + "/proper_noun_skill_spa_name_" + lang + ".msg");
        for (int i = 0; i < SuperCount; i++) {
            CUS.seekg(SupAddress + (i * 48) + 8, std::ios::beg);
            CUS.read(reinterpret_cast<char*>(&Supers[i].ID), sizeof(Supers[i].ID));
            Supers[i].Name = findName("spe_skill_" + std::to_string(Supers[i].ID));
        }

        Ultimates.resize(UltimateCount);
        mText = loadMsgFile(msgFolder + "/proper_noun_skill_ult_name_" + lang + ".msg");
        for (int i = 0; i < UltimateCount; i++) {
            CUS.seekg(UltAddress + (i * 48) + 8, std::ios::beg);
            CUS.read(reinterpret_cast<char*>(&Ultimates[i].ID), sizeof(Ultimates[i].ID));
            Ultimates[i].Name = findName("ult_" + std::to_string(Ultimates[i].ID));
        }

        Evasives.resize(EvasiveCount);
        mText = loadMsgFile(msgFolder + "/proper_noun_skill_esc_name_" + lang + ".msg");
        for (int i = 0; i < EvasiveCount; i++) {
            CUS.seekg(EvaAddress + (i * 48) + 8, std::ios::beg);
            CUS.read(reinterpret_cast<char*>(&Evasives[i].ID), sizeof(Evasives[i].ID));
            Evasives[i].Name = findName("avoid_skill_" + std::to_string(Evasives[i].ID));
        }

        CUS.close();
    }

    void Save() {
        std::ofstream CUS(FileName, std::ios::binary);
        CUS.seekp(CharAddress, std::ios::beg);
        for (int i = 0; i < CharCount; i++) {
            CUS.seekp(CharAddress + (i * 32) + 8, std::ios::beg);
            CUS.write(reinterpret_cast<const char*>(&Chars[i].SuperIDs), sizeof(Chars[i].SuperIDs));
            CUS.write(reinterpret_cast<const char*>(&Chars[i].UltimateIDs), sizeof(Chars[i].UltimateIDs));
            CUS.write(reinterpret_cast<const char*>(&Chars[i].EvasiveID), sizeof(Chars[i].EvasiveID));
        }
        CUS.close();
    }

    int FindSuper(short id) {
        for (int i = 0; i < Supers.size(); i++) {
            if (Supers[i].ID == id)
                return i;
        }
        return -1;
    }

    int FindUltimate(short id) {
        for (int i = 0; i < Ultimates.size(); i++) {
            if (Ultimates[i].ID == id)
                return i;
        }
        return -1;
    }

    int FindEvasive(short id) {
        for (int i = 0; i < Evasives.size(); i++) {
            if (Evasives[i].ID == id)
                return i;
        }
        return -1;
    }

    int DataExist(int id, int c) {
        for (int i = 0; i < Chars.size(); i++) {
            if (Chars[i].charID == id && Chars[i].CostumeID == c)
                return i;
        }

        return -1;
    }

    std::vector<std::string> loadMsgFile(const std::string& msgFile) {
        std::vector<std::string> msgData;
        std::ifstream msgStream(msgFile);
        std::string line;
        while (std::getline(msgStream, line)) {
            msgData.push_back(line);
        }
        return msgData;
    }
};

#endif