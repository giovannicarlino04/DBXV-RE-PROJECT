#ifndef __ACBFILE_H__
#define __ACBFILE_H__

#include "CriwareAudioContainer.h"
#include "UtfFile.h"

class AcbFile : public CriwareAudioContainer, protected UtfFile
{
private:

	uint32_t FileIdentifier; // 0
	uint32_t Size; // 0
	uint32_t Version; // 1220400
	uint8_t Type; // 0
	uint8_t Target; // 0
	
	uint8_t AcfMd5Hash[0x10]; // We have no idea of which thing this hashes...
	
	uint8_t CategoryExtension;
	
	UtfFile CueTable;
	UtfFile CueNameTable;
	UtfFile WaveformTable;
	UtfFile AisacTable;
	UtfFile GraphTable;
	UtfFile GlobalAisacReferenceTable;
	UtfFile AisacNameTable;	
	UtfFile SynthTable;
	UtfFile CommandTable;
	UtfFile TrackTable;
	UtfFile SequenceTable;
	UtfFile AisacControlNameTable;
	UtfFile AutoModulationTable;
	
	uint8_t *StreamAwbTocWorkOld;
	uint32_t StreamAwbTocWorkOld_Size;
	
	uint8_t *AwbFile;
	uint32_t AwbFile_Size;
	
	std::string VersionString;
	
	uint8_t *CueLimitWorkTable;
	uint32_t CueLimitWorkTable_Size;
	
	uint16_t NumCueLimitListWorks;
	uint16_t NumCueLimitNodeWorks;
	
	uint8_t AcbGuid[0x10];
	uint8_t StreamAwbHash[0x10]; // Hash of full awb file, only if it uses external awb file, otherwise hash is zeroed.
	
	uint8_t *StreamAwbTocWork_Old;
	uint32_t StreamAwbTocWork_Old_Size;
	
	float AcbVolume;
	
	UtfFile StringValueTable;
	UtfFile OutsideLinkTable;
	UtfFile BlockSequenceTable;
	UtfFile BlockTable;
	
	std::string Name;
	
	uint8_t CharacterEncodingType;
	
	UtfFile EventTable;
	UtfFile ActionTrackTable;
	UtfFile AcfReferenceTable;
	UtfFile WaveformExtensionDataTable;
	
	uint8_t *PaddingArea;
	uint32_t PaddingArea_Size;
	
	uint8_t *StreamAwbTocWork;
	uint32_t StreamAwbTocWork_Size;
	
	uint8_t *StreamAwbAfs2Header;
	uint32_t StreamAwbAfs2Header_Size;

	bool AwbFile_Modified;
	bool StreamAwbHash_Modified;
	bool StreamAwbAfs2Header_Modified;

    bool awb_header_in_table = false; // true in new format added in Xenoverse 2
    bool awb_hash_in_table = false; // true in new format added in Xenoverse 2
	
	bool LoadTable(const std::string &name, UtfFile &table);

protected:

    inline bool IsAwbModified() { return AwbFile_Modified; }
    inline bool SetAwbModified(bool value) { AwbFile_Modified = value; }

public:

	AcbFile();
	virtual ~AcbFile();
	
	virtual void Reset() override;
	
    virtual bool Load(const uint8_t *buf, size_t size) override;
    virtual uint8_t *Save(size_t *psize) override;

    virtual bool LoadFromFile(const std::string &path, bool show_error=true) override;
    virtual bool SaveToFile(const std::string &path, bool show_error=true, bool build_path=false) override;
	
    virtual bool HasAwb() const override { return (AwbFile != nullptr); }
    virtual uint8_t *GetAwb(uint32_t *awb_size) const override;
    virtual bool SetAwb(void *awb, uint32_t awb_size, bool take_ownership=false) override;
	
    virtual const uint8_t *GetExternalAwbHash() const override { return StreamAwbHash; }
    virtual bool SetExternalAwbHash(uint8_t *hash) override;
	
    virtual bool HasAwbHeader() const override { return (StreamAwbAfs2Header != nullptr); }
    virtual uint8_t *GetAwbHeader(uint32_t *header_size) const override;
    virtual bool SetAwbHeader(void *header, uint32_t header_size, bool take_ownership) override;
};

#endif // __ACBFILE_H__
