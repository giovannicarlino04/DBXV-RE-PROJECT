#include "AcbFile.h"
#include "Utils.h"
#include "debug.h"

AcbFile::AcbFile() : UtfFile()
{
	StreamAwbTocWorkOld = nullptr;
	AwbFile = nullptr;
	CueLimitWorkTable = nullptr;
	StreamAwbTocWork_Old = nullptr;
	PaddingArea = nullptr;
	StreamAwbTocWork = nullptr;
	StreamAwbAfs2Header = nullptr;
	
	Reset();
}

AcbFile::~AcbFile()
{
    Reset();
}

void AcbFile::Reset()
{
    UtfFile::Reset();

	FileIdentifier = 0;
	Size = 0;
	Version = 0;
	Type = 0;
	Target = 0;
	
	memset(AcfMd5Hash, 0, sizeof(AcfMd5Hash));
	
	CategoryExtension = 0;
	
	CueTable.Reset();
	CueNameTable.Reset();
	WaveformTable.Reset();
	AisacTable.Reset();
	GraphTable.Reset();
	GlobalAisacReferenceTable.Reset();
	AisacNameTable.Reset();
	SynthTable.Reset();
	CommandTable.Reset();
	TrackTable.Reset();
	SequenceTable.Reset();
	AisacControlNameTable.Reset();
	AutoModulationTable.Reset();
	
	if (StreamAwbTocWorkOld)
	{
		delete[] StreamAwbTocWorkOld;
		StreamAwbTocWorkOld = nullptr;
	}
	
	StreamAwbTocWorkOld_Size = 0;
	
	if (AwbFile)
	{
		delete[] AwbFile;
		AwbFile = nullptr;
	}
	
	AwbFile_Size = 0;
	
	VersionString.clear();
	
	if (CueLimitWorkTable)
	{
		delete[] CueLimitWorkTable;
		CueLimitWorkTable = nullptr;
	}
	
	CueLimitWorkTable_Size = 0;
	
	NumCueLimitListWorks = 0;
	NumCueLimitNodeWorks = 0;
	
	memset(AcbGuid, 0, sizeof(AcbGuid));
	memset(StreamAwbHash, 0, sizeof(StreamAwbHash));
	
	if (StreamAwbTocWork_Old)
	{
		delete[] StreamAwbTocWork_Old;
		StreamAwbTocWork_Old = nullptr;
	}
	
	StreamAwbTocWork_Old_Size = 0;
	
	AcbVolume = 0.0f;
	
	StringValueTable.Reset();
	OutsideLinkTable.Reset();
	BlockSequenceTable.Reset();
	BlockTable.Reset();
	
	Name.clear();
	
	CharacterEncodingType = 0;
	
	EventTable.Reset();
	ActionTrackTable.Reset();
	AcfReferenceTable.Reset();
	WaveformExtensionDataTable.Reset();
	
	if (PaddingArea)
	{
		delete[] PaddingArea;
		PaddingArea = nullptr;
	}
	
	PaddingArea_Size = 0;
	
	if (StreamAwbTocWork)
	{
		delete[] StreamAwbTocWork;
		StreamAwbTocWork = nullptr;
	}
	
	StreamAwbTocWork_Size = 0;
	
	if (StreamAwbAfs2Header)
	{
		delete[] StreamAwbAfs2Header;
		StreamAwbAfs2Header = nullptr;
	}
	
	StreamAwbAfs2Header_Size = 0;
	
	AwbFile_Modified = false;
	StreamAwbHash_Modified = false;
	StreamAwbAfs2Header_Modified = false;

    // New, for Xenoverse 2 modified format
    awb_header_in_table = false;
    awb_hash_in_table = false;
}

bool AcbFile::LoadTable(const std::string &name, UtfFile &table)
{
	uint8_t *utf_buf;
	unsigned int utf_buf_size;
	
	utf_buf = GetBlob(name, &utf_buf_size);
	if (!utf_buf)
	{
		//DPRINTF("--- %s not present.\n", name.c_str());
		return false;
	}
	
	if (!table.Load(utf_buf, utf_buf_size))
	{
        //DPRINTF("%s: Failed to load table \"%s\"\n", FUNCNAME, name.c_str());
		return false;
	}
	
	//DPRINTF("+++ %s present.\n", name.c_str());	
	return true;
}

bool AcbFile::Load(const uint8_t *buf, size_t size)
{
	if (!UtfFile::Load(buf, size))
		return false;
	
	if (GetDword("FileIdentifier", &FileIdentifier))
	{
		//DPRINTF("+++ FileIdentifier: %x\n", FileIdentifier);
	}
	else
	{
		//DPRINTF("--- FileIdentifier not found.\n");
	}
	
	if (GetDword("Size", &Size))
	{
		//DPRINTF("+++ Size: %x\n", Size);
	}
	else
	{
		//DPRINTF("--- Size not found.\n");
	}
	
	if (GetDword("Version", &Version))
	{
		//DPRINTF("+++ Version: %08x\n", Version);
	}
	else
	{
		//DPRINTF("--- Version not found.\n");
	}
	
	if (GetByte("Type", &Type))
	{
		//DPRINTF("+++ Type: %x\n", Type);
	}
	else
	{
		//DPRINTF("--- Type not found.\n");
	}
	
	if (GetByte("Target", &Target))
	{
		//DPRINTF("+++ Target: %x\n", Target);
	}
	else
	{
		//DPRINTF("--- Target not found.\n");
	}
	
	if (GetFixedBlob("AcfMd5Hash", AcfMd5Hash, sizeof(AcfMd5Hash)))
	{
		//DPRINTF("+++ AcfMd5Hash is present.\n");
	}
	else
	{
		//DPRINTF("--- AcfMd5Hash is not present or has incorrect size.\n");
	}
	
	if (GetByte("CategoryExtension", &CategoryExtension))
	{
		//DPRINTF("+++ CategoryExtension: %x\n", CategoryExtension);
	}
	else
	{
		//DPRINTF("--- CategoryExtension not found.\n");
	}
	
	LoadTable("CueTable", CueTable);
	LoadTable("CueNameTable", CueNameTable);
	LoadTable("WaveformTable", WaveformTable);
	LoadTable("AisacTable", AisacTable);
	LoadTable("GraphTable", GraphTable);
	LoadTable("GlobalAisacReferenceTable", GlobalAisacReferenceTable);
	LoadTable("AisacNameTable", AisacNameTable);
	LoadTable("SynthTable", SynthTable);
	LoadTable("CommandTable", CommandTable);
	LoadTable("TrackTable", TrackTable);
	LoadTable("SequenceTable", SequenceTable);
	LoadTable("AisacControlNameTable", AisacControlNameTable);
	LoadTable("AutoModulationTable", AutoModulationTable);
	
	StreamAwbTocWorkOld = GetBlob("StreamAwbTocWorkOld", &StreamAwbTocWorkOld_Size, true);
	if (StreamAwbTocWorkOld)
	{
		//DPRINTF("+++ StreamAwbTocWorkOld present.\n");
	}
	else
	{
		//DPRINTF("--- StreamAwbTocWorkOld not present.\n");
	}
	
	AwbFile = GetBlob("AwbFile", &AwbFile_Size, true);
	if (AwbFile)
	{
		//DPRINTF("+++ AwbFile present (this file is a standalone .acb)\n");
	}
	else
	{
		//DPRINTF("--- AwbFile not present (this file uses a external .awb)\n");
	}
	
	if (GetString("VersionString", &VersionString))
	{
		//DPRINTF("+++ VersionString: \"%s\"\n", VersionString.c_str());
	}
	else
	{
		//DPRINTF("--- VersionString not found.\n");
	}
	
	CueLimitWorkTable = GetBlob("CueLimitWorkTable", &CueLimitWorkTable_Size, true);
	if (CueLimitWorkTable)
	{
		//DPRINTF("+++ CueLimitWorkTable present.\n");
	}
	else
	{
		//DPRINTF("--- CueLimitWorkTable not present.\n");
	}
	
	if (GetWord("NumCueLimitListWorks", &NumCueLimitListWorks))
	{
		//DPRINTF("+++ NumCueLimitListWorks: %x\n", NumCueLimitListWorks);
	}
	else
	{
		//DPRINTF("--- NumCueLimitListWorks not found.\n");
	}
	
	if (GetWord("NumCueLimitNodeWorks", &NumCueLimitNodeWorks))
	{
		//DPRINTF("+++ NumCueLimitNodeWorks: %x\n", NumCueLimitNodeWorks);
	}
	else
	{
		//DPRINTF("--- NumCueLimitNodeWorks not found.\n");
	}
	
	if (GetFixedBlob("AcbGuid", AcbGuid, sizeof(AcbGuid)))
	{
		//DPRINTF("+++ AcbGuid present.\n");
	}
	else
	{
		//DPRINTF("--- AcbGuid not present.\n");
	}
	
	if (GetFixedBlob("StreamAwbHash", StreamAwbHash, sizeof(StreamAwbHash)))
	{
        //DPRINTF("+++ StreamAwbHash present.\n");       
	}
	else
	{
        // Xenoverse 2 new format

        UtfFile hash;

        if (LoadTable("StreamAwbHash", hash))
        {
            if (hash.GetFixedBlob("Hash", StreamAwbHash, sizeof(StreamAwbHash)))
            {
                awb_hash_in_table = true;
            }
        }
        //DPRINTF("--- StreamAwbHash not present.\n");
	}
	
	StreamAwbTocWork_Old = GetBlob("StreamAwbTocWork_Old", &StreamAwbTocWork_Old_Size, true);
	if (StreamAwbTocWork_Old)
	{
		//DPRINTF("+++ StreamAwbTocWork_Old present.\n");
	}
	else
	{
		//DPRINTF("--- StreamAwbTocWork_Old not present.\n");
	}
	
	if (GetFloat("AcbVolume", &AcbVolume))
	{
		//DPRINTF("+++ AcbVolume: %f\n", AcbVolume);
	}
	else
	{
		//DPRINTF("--- AcbVolume not present.\n");
	}

	LoadTable("StringValueTable", StringValueTable);
	LoadTable("OutsideLinkTable", OutsideLinkTable);
	LoadTable("BlockSequenceTable", BlockSequenceTable);
	LoadTable("BlockTable", BlockTable);
	
	if (GetString("Name", &Name))
	{
		//DPRINTF("+++ Name: %s\n", Name.c_str());
	}
	else
	{
		//DPRINTF("--- Name not found.\n");
	}
	
	if (GetByte("CharacterEncodingType", &CharacterEncodingType))
	{
		//DPRINTF("+++ CharacterEncodingType: %x\n", CharacterEncodingType);
	}
	else
	{
		//DPRINTF("--- CharacterEncodingType not found.\n");
	}
	
	LoadTable("EventTable", EventTable);
	LoadTable("ActionTrackTable", ActionTrackTable);
	LoadTable("AcfReferenceTable", AcfReferenceTable);
	LoadTable("WaveformExtensionDataTable", WaveformExtensionDataTable);
	
	PaddingArea = GetBlob("PaddingArea", &PaddingArea_Size, true);
	if (PaddingArea)
	{
		//DPRINTF("+++ PaddingArea present.\n");
	}
	else
	{
		//DPRINTF("--- PaddingArea not present.\n");
	}
	
	StreamAwbTocWork = GetBlob("StreamAwbTocWork", &StreamAwbTocWork_Size, true);
	if (StreamAwbTocWork)
	{
		//DPRINTF("+++ StreamAwbTocWork present.\n");
	}
	else
	{
		//DPRINTF("--- StreamAwbTocWork not present.\n");
	}
	
	StreamAwbAfs2Header = GetBlob("StreamAwbAfs2Header", &StreamAwbAfs2Header_Size, true);
	if (StreamAwbAfs2Header)
	{
        //DPRINTF("+++ StreamAwbAfs2Header present. (typical of .acb that needs external .awb)\n");
        if (StreamAwbAfs2Header_Size >= sizeof(uint32_t) && *(uint32_t *)StreamAwbAfs2Header == UTF_SIGNATURE)
        {
            // Xenoverse 2 new format;

            awb_header_in_table = true;
            delete[] StreamAwbAfs2Header;
            StreamAwbAfs2Header = nullptr;
            StreamAwbAfs2Header_Size = 0;

            UtfFile header;

            if (!LoadTable("StreamAwbAfs2Header", header))
                return false;

            StreamAwbAfs2Header = header.GetBlob("Header", &StreamAwbAfs2Header_Size, true);
        }
	}
	else
	{
		//DPRINTF("--- StreamAwbAfs2Header not present. (typical of standalone .acb)\n");
	}
		
	return true;
}

uint8_t *AcbFile::Save(size_t *psize)
{
	if (AwbFile_Modified)
	{
		if (!SetBlob("AwbFile", AwbFile, AwbFile_Size))
		{
			DPRINTF("%s: SetBlob failed.\n", FUNCNAME);
			return nullptr;
		}
		
		AwbFile_Modified = false;
	}
	
    if (StreamAwbHash_Modified && !awb_hash_in_table)
	{
		if (!SetBlob("StreamAwbHash", StreamAwbHash, sizeof(StreamAwbHash)))
		{
			DPRINTF("%s: SetBlob failed.\n", FUNCNAME);
			return nullptr;
		}
		
		StreamAwbHash_Modified = false;
	}
	
    if (StreamAwbAfs2Header_Modified && !awb_header_in_table)
	{
		if (!SetBlob("StreamAwbAfs2Header", StreamAwbAfs2Header, StreamAwbAfs2Header_Size))
		{
			DPRINTF("%s: SetBlob failed.\n", FUNCNAME);
			return nullptr;
		}
		
		StreamAwbAfs2Header_Modified = false;
	}
	
    return UtfFile::Save(psize);
}

bool AcbFile::LoadFromFile(const std::string &path, bool show_error)
{
    return BaseFile::LoadFromFile(path, show_error);
}

bool AcbFile::SaveToFile(const std::string &path, bool show_error, bool build_path)
{
    return BaseFile::SaveToFile(path, show_error, build_path);
}

uint8_t *AcbFile::GetAwb(uint32_t *awb_size) const
{
	if (!HasAwb())
		return nullptr;
	
	*awb_size = AwbFile_Size;
	return AwbFile;
}

uint8_t *AcbFile::GetAwbHeader(uint32_t *header_size) const
{
	if (!HasAwbHeader())
		return nullptr;

    *header_size = StreamAwbAfs2Header_Size;
     return StreamAwbAfs2Header;

}

bool AcbFile::SetAwb(void *awb, uint32_t awb_size, bool take_ownership)
{
	if (AwbFile)
		delete[] AwbFile;
		
	AwbFile = nullptr;
	AwbFile_Size = 0;
	
	if (awb)
	{
        if (take_ownership)
        {
            AwbFile = (uint8_t *)awb;
        }
        else
        {
            AwbFile = new uint8_t[awb_size];
            memcpy(AwbFile, awb, awb_size);
        }

		AwbFile_Size = awb_size;
	}	
	
	AwbFile_Modified = true;
    return true;
}

bool AcbFile::SetExternalAwbHash(uint8_t *hash)
{
	memcpy(StreamAwbHash, hash, sizeof(StreamAwbHash));

    if (awb_hash_in_table)
    {
        UtfFile hash;

        if (!LoadTable("StreamAwbHash", hash))
            return false;

        if (!hash.SetBlob("Hash", StreamAwbHash, sizeof(StreamAwbHash)))
            return false;

        size_t size;
        uint8_t *buf = hash.Save(&size);

        if (!buf)
            return false;

        return SetBlob("StreamAwbHash", buf, (unsigned int)size, 0, true);
    }
    else
    {
        StreamAwbHash_Modified = true;
    }

	return true;
}

bool AcbFile::SetAwbHeader(void *header_buf, uint32_t header_size, bool take_ownership)
{
	if (StreamAwbAfs2Header)
		delete[] StreamAwbAfs2Header;
		
	StreamAwbAfs2Header = nullptr;
	StreamAwbAfs2Header_Size = 0;
	
    if (header_buf)
	{
        if (take_ownership)
        {
            StreamAwbAfs2Header = (uint8_t *)header_buf;
        }
        else
        {
            StreamAwbAfs2Header = new uint8_t[header_size];
            memcpy(StreamAwbAfs2Header, header_buf, header_size);
        }

		StreamAwbAfs2Header_Size = header_size;

        if (awb_header_in_table)
        {
            UtfFile header;

            if (!LoadTable("StreamAwbAfs2Header", header) || !header.SetBlob("Header", StreamAwbAfs2Header, StreamAwbAfs2Header_Size))
            {
                if (take_ownership)
                    delete[] header_buf;

                StreamAwbAfs2Header = nullptr;
                StreamAwbAfs2Header_Size = 0;
                return false;
            }

            size_t size;
            uint8_t *buf = header.Save(&size);

            if (!buf)
                return false;

            return SetBlob("StreamAwbAfs2Header", buf, (unsigned int)size, 0, true);
        }
	}	
	
	StreamAwbAfs2Header_Modified = true;
	return true;
}
