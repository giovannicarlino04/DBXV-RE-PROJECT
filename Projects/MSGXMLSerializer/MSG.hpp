#ifndef MSG_HPP
#define MSG_HPP

#include <iostream>
#include <fstream>
#include <vector>
#include <string>

struct msgData {
    std::string NameID;
    int ID;
    std::vector<std::string> Lines;
};

struct msg {
    int type;
    std::vector<msgData> data;

    std::string Find(const std::string& id) {
        for (const auto& m : data) {
            if (m.NameID == id)
                return m.Lines[0];
        }
        return "No Matching ID";
    }
};

class msgStream {
public:
    static msg Load2(const std::string& FileName) {
        msg file;
        std::ifstream br(FileName, std::ios::binary);
        br.seekg(4, std::ios::beg);
        br.read(reinterpret_cast<char*>(&file.type), sizeof(file.type));
        br.seekg(2, std::ios::cur);
        file.data.resize(brReadInt32(br));

        // read NameID
        int startaddress = brReadInt32(br);
        for (int i = 0; i < file.data.size(); i++) {
            br.seekg(startaddress + (i * 16), std::ios::beg);
            int textaddress = brReadInt32(br);
            br.seekg(4, std::ios::cur);
            int charCount = brReadInt32(br);
            br.seekg(textaddress, std::ios::beg);
            if (file.type == 256)
                file.data[i].NameID = brReadStringUnicode(br, charCount - 2);
            else
                file.data[i].NameID = brReadStringASCII(br, charCount - 1);
        }

        // read ID
        br.seekg(16, std::ios::beg);
        startaddress = brReadInt32(br);
        for (int i = 0; i < file.data.size(); i++)
            file.data[i].ID = brReadInt32(br);

        // read line/s
        br.seekg(20, std::ios::beg);
        startaddress = brReadInt32(br);
        int address;
        for (int i = 0; i < file.data.size(); i++) {
            br.seekg(startaddress + (i * 8), std::ios::beg);
            file.data[i].Lines.resize(brReadInt32(br));
            address = brReadInt32(br);
            int address2;
            for (int j = 0; j < file.data[i].Lines.size(); j++) {
                br.seekg(address + (j * 16), std::ios::beg);
                address2 = brReadInt32(br);
                br.seekg(4, std::ios::cur);
                int charCount = brReadInt32(br);
                br.seekg(address2, std::ios::beg);
                file.data[i].Lines[j] = brReadStringUnicode(br, charCount - 2);
            }
        }

        return file;
    }

    static void Save2(const msg& file, const std::string& FileName) {
        std::ofstream CUS(FileName, std::ios::binary);
        int byteCount = 0;
        int TopLength = 32;
        int Mid1Length = file.data.size() * 16;
        int Mid2Length = file.data.size() * 4;
        int Mid3Length = file.data.size() * 8;
        int lineCount = 0;
        for (const auto& data : file.data)
            lineCount += data.Lines.size();
        int Mid4Length = lineCount * 16;
        byteCount = TopLength + Mid1Length + Mid2Length + Mid3Length + Mid4Length;
        std::vector<char> fileData1(byteCount);
        std::vector<char> fileDataText;
        int TopStart = 0;
        int Mid1Start = 32;
        int Mid2Start = Mid1Start + Mid1Length;
        int Mid3Start = Mid2Start + Mid2Length;
        int Mid4Start = Mid3Start + Mid3Length;
        int LastStart = Mid4Start + Mid4Length;

        // generate top
        fileData1[0] = 0x23; fileData1[1] = 0x4D; fileData1[2] = 0x53; fileData1[3] = 0x47;
        if (file.type == 256) {
            fileData1[4] = 0x00; fileData1[5] = 0x01; fileData1[6] = 0x01; fileData1[7] = 0x00;
        }
        else {
            fileData1[4] = 0x00; fileData1[5] = 0x00; fileData1[6] = 0x01; fileData1[7] = 0x00;
        }

        Applybyte(fileData1, GenBytes(file.data.size()), 8, 4);
        Applybyte(fileData1, GenBytes(32), 12, 4);
        Applybyte(fileData1, GenBytes(Mid2Start), 16, 4);
        Applybyte(fileData1, GenBytes(Mid3Start), 20, 4);
        Applybyte(fileData1, GenBytes(file.data.size()), 24, 4);
        Applybyte(fileData1, GenBytes(Mid4Start), 28, 4);

        // generate Mid section 1
        for (int i = 0; i < file.data.size(); i++) {
            Applybyte(fileData1, GenWordsBytes(file.data[i].NameID, file.type == 256, fileDataText, LastStart), Mid1Start + (i * 16), 16);
        }

        // generate Mid section 2
        for (int i = 0; i < file.data.size(); i++) {
            Applybyte(fileData1, GenBytes(file.data[i].ID), Mid2Start + (i * 4), 4);
        }

        // generate Mid section 3 & 4
        int ListCount = 0;
        int address3;
        for (int i = 0; i < file.data.size(); i++) {
            address3 = Mid4Start + (ListCount * 16);
            for (int j = 0; j < file.data[i].Lines.size(); j++) {
                Applybyte(fileData1, GenWordsBytes(file.data[i].Lines[j], true, fileDataText, LastStart), Mid4Start + (ListCount * 16), 16);
                ListCount++;
            }
            Applybyte(fileData1, GenBytes(file.data[i].Lines.size()), Mid3Start + (i * 8), 4);
            Applybyte(fileData1, GenBytes(address3), Mid3Start + (i * 8) + 4, 4);
        }

        // finalize
        std::ofstream fs(FileName, std::ios::binary);
        fs.write(fileData1.data(), fileData1.size());
        fs.write(fileDataText.data(), fileDataText.size());
        fs.close();
    }

private:
    static int brReadInt32(std::ifstream& br) {
        int value;
        br.read(reinterpret_cast<char*>(&value), sizeof(value));
        return value;
    }

    static std::string brReadStringUnicode(std::ifstream& br, int charCount) {
        std::vector<char> buffer(charCount);
        br.read(buffer.data(), buffer.size());
        return std::wstring(buffer.data(), buffer.size() / sizeof(wchar_t));
    }

    static std::string brReadStringASCII(std::ifstream& br, int charCount) {
        std::vector<char> buffer(charCount);
        br.read(buffer.data(), buffer.size());
        return std::string(buffer.data(), buffer.size());
    }

    static std::vector<char> GenBytes(int value) {
        return std::vector<char>(reinterpret_cast<char*>(&value), reinterpret_cast<char*>(&value) + sizeof(value));
    }

    static void Applybyte(std::vector<char>& file, const std::vector<char>& data, int pos, int count) {
        for (int i = 0; i < count; i++)
            file[pos + i] = data[i];
    }

    static void Addbyte(std::vector<char>& file, const std::vector<char>& data, int pos, int count) {
        file.insert(file.begin() + pos, data.begin(), data.begin() + count);
    }

    static std::vector<char> GenWordsBytes(const std::string& Line, bool type256, std::vector<char>& text, int bCount) {
        std::vector<char> charArray;
        if (type256) {
            charArray = std::vector<char>(Line.begin(), Line.end());
            charArray.push_back('\0');
            charArray.push_back('\0');
        }
        else {
            charArray = std::vector<char>(Line.begin(), Line.end());
            charArray.push_back('\0');
        }

        std::vector<char> AddressInfo(16, 0);
        Applybyte(AddressInfo, GenBytes(bCount + text.size()), 0, 4); // address of text
        Applybyte(AddressInfo, GenBytes(Line.size()), 4, 4);
        Applybyte(AddressInfo, GenBytes(charArray.size()), 8, 4);

        text.insert(text.end(), charArray.begin(), charArray.end());

        return AddressInfo;
    }
};

int main() {
    // Example usage:
    msg testMsg = msgStream::Load2("your_file.msg");
    // Modify testMsg...
    msgStream::Save2(testMsg, "output_file.msg");

    return 0;
}
#endif