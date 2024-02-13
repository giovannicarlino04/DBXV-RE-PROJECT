#ifndef PSC_HPP
#define PSC_HPP

#include <vector>
#include <string>
#include <cstddef>

struct Parameters {
    std::vector<byte> Data;
};

struct CharSet {
    int id;
    std::vector<Parameters> p;
};

class PSC {
private:
    std::string FileName;
    int statposition;
    const std::vector<std::string> ValNames = {
        "?????", "?????", "?????", "?????",
        "Health", "?????", "Ki", "Ki Recharging Damage Received",
        "?????", "?????", "?????", "Stamina",
        "Stamina Recover", "?????", "?????", "?????",
        "Basic Attack", "Normal Ki Blasts", "Strike Supers", "Ki Blast Supers",

        "Physical Damage Received", "Ki Damage Received", "?????", "?????",
        "Ground Speed", "Air Speed", "Boost Speed", "Dash Speed",
        "?????", "Reinforcement Skill Duration", "?????", "Revival HP Amount",
        "Ally Revival Speed", "?????", "?????", "?????",
        "?????", "?????", "?????", "?????",

        "?????", "?????", "Z-Soul", "?????",
        "?????", "?????"
    };

    const std::vector<int> type = {
        0, 0, 0, 0,
        1, 1, 1, 1,
        0, 0, 1, 1,
        1, 1, 1, 1,
        1, 1, 1, 1,

        1, 1, 1, 1,
        1, 1, 1, 1,
        1, 1, 1, 1,
        1, 1, 1, 1,
        1, 1, 1, 1,

        1, 1, 0, 0,
        0, 1
    };

    std::vector<CharSet> CharParam;

public:
    void load(const std::string& path) {
        std::ifstream br(path, std::ios::binary);
        FileName = path;
        br.seekg(8, std::ios::beg);
        int Count;
        br.read(reinterpret_cast<char*>(&Count), sizeof(Count));
        CharParam.resize(Count);

        for (int i = 0; i < Count; i++) {
            br.seekg(16 + (i * 12), std::ios::beg);
            br.read(reinterpret_cast<char*>(&CharParam[i].id), sizeof(CharParam[i].id));
            int paramCount;
            br.read(reinterpret_cast<char*>(&paramCount), sizeof(paramCount));
            CharParam[i].p.resize(paramCount);
        }

        br.seekg(4, std::ios::cur);
        statposition = static_cast<int>(br.tellg());

        for (int i = 0; i < Count; i++) {
            for (int j = 0; j < CharParam[i].p.size(); j++)
                br.read(reinterpret_cast<char*>(CharParam[i].p[j].Data.data()), CharParam[i].p[j].Data.size());
        }
    }

    void Save() {
        std::ofstream p(FileName, std::ios::binary);

        p.seekp(statposition, std::ios::beg);
        for (int i = 0; i < CharParam.size(); i++) {
            for (int j = 0; j < CharParam[i].p.size(); j++)
                p.write(reinterpret_cast<char*>(CharParam[i].p[j].Data.data()), CharParam[i].p[j].Data.size());
        }
    }

    float readAsFloat(int charIndex, int pIndex, int pos) {
        return *reinterpret_cast<float*>(&CharParam[charIndex].p[pIndex].Data[pos * 4]);
    }

    int readAsInt(int charIndex, int pIndex, int pos) {
        return *reinterpret_cast<int*>(&CharParam[charIndex].p[pIndex].Data[pos * 4]);
    }

    std::string getPosText(int pos) {
        return ValNames[pos];
    }

    int FindType(int pos) {
        return type[pos];
    }

    std::string getVal(int charIndex, int pIndex, int pos) {
        std::string val = "0";
        switch (type[pos]) {
            case 0:
                val = std::to_string(*reinterpret_cast<int*>(&CharParam[charIndex].p[pIndex].Data[pos * 4]));
                break;
            case 1:
                val = std::to_string(*reinterpret_cast<float*>(&CharParam[charIndex].p[pIndex].Data[pos * 4]));
                break;
        }
        return val;
    }

    void SaveVal(int charIndex, int pIndex, int pos, const std::string& val) {
        switch (type[pos]) {
            case 0:
                int n;
                if (std::istringstream(val) >> n) {
                    *reinterpret_cast<int*>(&CharParam[charIndex].p[pIndex].Data[pos * 4]) = n;
                }
                break;
            case 1:
                float nf;
                if (std::istringstream(val) >> nf) {
                    *reinterpret_cast<float*>(&CharParam[charIndex].p[pIndex].Data[pos * 4]) = nf;
                }
                break;
        }
    }

    int FindCharacterIndex(int id) {
        for (int i = 0; i < CharParam.size(); i++) {
            if (id == CharParam[i].id)
                return i;
        }

        return -1;
    }
};
#endif