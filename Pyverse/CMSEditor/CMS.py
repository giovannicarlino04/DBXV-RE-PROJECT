import struct

class CMS_Data:
    def __init__(self):
        self.ID = 0
        self.ShortName = ""
        self.Unknown = b""
        self.Paths = []

class CMS:
    def __init__(self):
        self.Data = []
        self.br = None
        self.bw = None
        self.FileName = ""

    def Load(self, path):
        with open(path, "rb") as self.br:
            self.FileName = path
            self.br.seek(8, 0)
            Count = struct.unpack("i", self.br.read(4))[0]
            self.Data = [CMS_Data() for _ in range(Count)]
            Offset = struct.unpack("i", self.br.read(4))[0]

            for i in range(Count):
                self.br.seek(Offset + (80 * i), 0)
                self.Data[i].ID = struct.unpack("i", self.br.read(4))[0]
                self.Data[i].ShortName = self.br.read(3).decode("ascii")
                self.br.seek(9, 1)
                self.Data[i].Unknown = self.br.read(8)
                self.br.seek(8, 1)
                self.Data[i].Paths = [self.TextAtAddress(struct.unpack("i", self.br.read(4))[0]) for _ in range(7)]

    def TextAtAddress(self, Address):
        position = self.br.tell()
        rText = ""
        if Address != 0:
            self.br.seek(Address, 0)
            while True:
                c = self.br.read(1)
                if c[0] != 0x00:
                    rText += c.decode("ascii")
                else:
                    break
            self.br.seek(position, 0)
        return rText

    def Save(self):
        CmnText = []
        for i in range(len(self.Data)):
            for j in range(len(self.Data[i].Paths)):
                if self.Data[i].Paths[j] not in CmnText:
                    CmnText.append(self.Data[i].Paths[j])

        wordAddress = [0] * len(CmnText)
        wordstartposition = 16 + (len(self.Data) * 80)
        with open(self.FileName, "wb") as self.bw:
            self.bw.write(b"\x23\x43\x4D\x53\xFE\xFF\x00\x00")
            self.bw.write(struct.pack("i", len(self.Data)))
            self.bw.write(struct.pack("i", 16))
            self.bw.seek(wordstartposition, 0)
            for i in range(len(CmnText)):
                wordAddress[i] = self.bw.tell()
                self.bw.write(CmnText[i].encode("ascii"))
                self.bw.write(b"\x00")

            for i in range(len(self.Data)):
                self.bw.seek(16 + (80 * i), 0)
                self.bw.write(struct.pack("i", self.Data[i].ID))
                self.bw.write(self.Data[i].ShortName.encode("ascii"))
                self.bw.seek(9, 1)
                self.bw.write(self.Data[i].Unknown)
                self.bw.write(b"\xFF\xFF")
                self.bw.seek(6, 1)
                self.bw.write(struct.pack("i", wordAddress[CmnText.index(self.Data[i].Paths[0])]))
                self.bw.write(struct.pack("i", wordAddress[CmnText.index(self.Data[i].Paths[1])]))
                self.bw.seek(4, 1)
                self.bw.write(struct.pack("i", wordAddress[CmnText.index(self.Data[i].Paths[2])]))
                self.bw.seek(8, 1)
                self.bw.write(struct.pack("i", wordAddress[CmnText.index(self.Data[i].Paths[3])]))
                self.bw.write(struct.pack("i", wordAddress[CmnText.index(self.Data[i].Paths[4])]))
                self.bw.write(struct.pack("i", wordAddress[CmnText.index(self.Data[i].Paths[5])]))
                self.bw.write(struct.pack("i", wordAddress[CmnText.index(self.Data[i].Paths[6])]))

# Usage example:
# cms = CMS()
# cms.Load("path/to/your/cms_file.bin")
# # Modify the data if needed
# cms.Save()
