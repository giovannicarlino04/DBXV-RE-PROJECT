#include "IggyFile.h"
#include "FixedMemoryStream.h"
#include "debug.h"
#include <vector>
#include <cassert>

// WIP File. We need assert always on
#undef NDEBUG

IggyFile::IggyFile()
{
    this->big_endian = false;
}

IggyFile::~IggyFile()
{

}

bool IggyFile::LoadFlashData32(FixedMemoryStream &stream)
{
    IGGYFlashHeader32 hdr;
    size_t size = (size_t)stream.GetSize();

    if (!stream.Read(&hdr, sizeof(hdr)))
        return false;

    if (hdr.main_offset < sizeof(IGGYFlashHeader32))
        return false;

    if (hdr.as3_section_offset >= size || hdr.unk_offset >= size || hdr.unk_offset2 >= size || hdr.unk_offset3 >= size)
    {
        DPRINTF("%s: Failure in some absolute offset.", FUNCNAME);
        return false;
    }

    stream.Seek(0, SEEK_SET);

    size_t prev_offset = 0;
    size_t next_offset = hdr.as3_section_offset;

    // as3_section_offset = main section size
    main_section.resize(next_offset - prev_offset);

    if (!stream.Read(main_section.data(), main_section.size()))
       return false;

    assert(stream.Tell() == next_offset);
    prev_offset = next_offset;

    if (hdr.as3_code_offset != 1)
    {
        next_offset = hdr.as3_code_offset + offsetof(IGGYFlashHeader32, as3_code_offset);
    }
    else
    {
        if (hdr.names_offset != 1)
        {
            next_offset = hdr.names_offset + offsetof(IGGYFlashHeader32, names_offset);
        }
        else
        {
            if (hdr.last_section_offset != 1)
            {
                next_offset = hdr.last_section_offset + offsetof(IGGYFlashHeader32, last_section_offset);
            }
            else
            {
                next_offset = size;
            }
        }
    }

    as3_names_section.resize(next_offset - prev_offset);

    if (!stream.Read(as3_names_section.data(), as3_names_section.size()))
        return false;

    assert(stream.Tell() == next_offset);
    prev_offset = next_offset;

    if (hdr.as3_code_offset != 1)
    {
        if (hdr.names_offset != 1)
        {
            next_offset = hdr.names_offset + offsetof(IGGYFlashHeader32, names_offset);
        }
        else
        {
            if (hdr.last_section_offset != 1)
            {
                next_offset = hdr.last_section_offset + offsetof(IGGYFlashHeader32, last_section_offset);
            }
            else
            {
                next_offset = size;
            }
        }

        as3_offset = (uint32_t)stream.Tell();
        as3_code_section.resize(next_offset - prev_offset);

        if (!stream.Read(as3_code_section.data(), as3_code_section.size()))
            return false;

        assert(stream.Tell() == next_offset);
        prev_offset = next_offset;
    }
    else
    {
        as3_code_section.clear();
    }

    if (hdr.names_offset != 1)
    {
        if (hdr.last_section_offset != 1)
        {
            next_offset = hdr.last_section_offset + offsetof(IGGYFlashHeader32, last_section_offset);
        }
        else
        {
            next_offset = size;
        }

        names_section.resize(next_offset - prev_offset);

        if (!stream.Read(names_section.data(), names_section.size()))
            return false;

        assert(stream.Tell() == next_offset);
        prev_offset = next_offset;
    }
    else
    {
        names_section.clear();
    }

    if (hdr.last_section_offset != 1)
    {
        next_offset = size;
        last_section.resize(next_offset - prev_offset);

        if (!stream.Read(last_section.data(), last_section.size()))
            return false;
    }
    else
    {
        last_section.clear();
    }

    assert(stream.Tell() == size);

    // Check as3 code section
    uint8_t *as3_ptr = as3_code_section.data();
    size_t max_as3_size = as3_code_section.size();

    if (max_as3_size != 0)
    {
        if (max_as3_size < 8 || *(uint32_t *)&as3_ptr[4] > max_as3_size-8)
        {
            DPRINTF("%s: Failed at AS3 size.\n", FUNCNAME);
            return false;
        }
    }

    return true;
}

bool IggyFile::LoadFlashData64(FixedMemoryStream &stream)
{
    IGGYFlashHeader64 hdr;
    size_t size = (size_t)stream.GetSize();

    if (!stream.Read(&hdr, sizeof(hdr)))
        return false;

    if (hdr.main_offset < sizeof(IGGYFlashHeader64))
        return false;

    if (hdr.as3_section_offset >= size || hdr.unk_offset >= size || hdr.unk_offset2 >= size || hdr.unk_offset3 >= size)
    {
        DPRINTF("%s: Failure in some absolute offset.", FUNCNAME);
        return false;
    }

    stream.Seek(0, SEEK_SET);

    size_t prev_offset = 0;
    size_t next_offset = hdr.as3_section_offset;

    // as3_section_offset = main section size
    main_section.resize(next_offset - prev_offset);

    if (!stream.Read(main_section.data(), main_section.size()))
       return false;

    assert(stream.Tell() == next_offset);
    prev_offset = next_offset;

    if (hdr.as3_code_offset != 1)
    {
        next_offset = hdr.as3_code_offset + offsetof(IGGYFlashHeader64, as3_code_offset);
    }
    else
    {
        if (hdr.names_offset != 1)
        {
            next_offset = hdr.names_offset + offsetof(IGGYFlashHeader64, names_offset);
        }
        else
        {
            if (hdr.last_section_offset != 1)
            {
                next_offset = hdr.last_section_offset + offsetof(IGGYFlashHeader64, last_section_offset);
            }
            else
            {
                next_offset = size;
            }
        }
    }

    as3_names_section.resize(next_offset - prev_offset);

    if (!stream.Read(as3_names_section.data(), as3_names_section.size()))
        return false;

    assert(stream.Tell() == next_offset);
    prev_offset = next_offset;

    if (hdr.as3_code_offset != 1)
    {
        if (hdr.names_offset != 1)
        {
            next_offset = hdr.names_offset + offsetof(IGGYFlashHeader64, names_offset);
        }
        else
        {
            if (hdr.last_section_offset != 1)
            {
                next_offset = hdr.last_section_offset + offsetof(IGGYFlashHeader64, last_section_offset);
            }
            else
            {
                next_offset = size;
            }
        }

        as3_offset = (uint32_t)stream.Tell();
        as3_code_section.resize(next_offset - prev_offset);

        if (!stream.Read(as3_code_section.data(), as3_code_section.size()))
            return false;

        assert(stream.Tell() == next_offset);
        prev_offset = next_offset;
    }
    else
    {
        as3_code_section.clear();
    }

    if (hdr.names_offset != 1)
    {
        if (hdr.last_section_offset != 1)
        {
            next_offset = hdr.last_section_offset + offsetof(IGGYFlashHeader64, last_section_offset);
        }
        else
        {
            next_offset = size;
        }

        names_section.resize(next_offset - prev_offset);

        if (!stream.Read(names_section.data(), names_section.size()))
            return false;

        assert(stream.Tell() == next_offset);
        prev_offset = next_offset;
    }
    else
    {
        names_section.clear();
    }

    if (hdr.last_section_offset != 1)
    {
        next_offset = size;
        last_section.resize(next_offset - prev_offset);

        if (!stream.Read(last_section.data(), last_section.size()))
            return false;
    }
    else
    {
        last_section.clear();
    }

    assert(stream.Tell() == size);

    // Check as3 code section
    uint8_t *as3_ptr = as3_code_section.data();
    size_t max_as3_size = as3_code_section.size();

    if (max_as3_size != 0)
    {
        if (max_as3_size < 0x10 || *(uint32_t *)&as3_ptr[8] > max_as3_size-0xC)
        {
            DPRINTF("%s: Failed at AS3 size.\n", FUNCNAME);
            return false;
        }
    }

    return true;
}

bool IggyFile::Load(const uint8_t *buf, size_t size)
{
    if (size < sizeof(IGGYHeader))
        return false;

    const IGGYHeader *hdr = (IGGYHeader *)buf;
    const uint8_t *top = buf + size;

    if (hdr->signature != IGGY_SIGNATURE)
    {
        DPRINTF("%s: Invalid iggy signature.\n", FUNCNAME);
        return false;
    }

    if (hdr->version != 0x900)
    {
        DPRINTF("%s: Unknown file version. Aborting load.\n", FUNCNAME);
        return false;
    }

    if (hdr->plattform[0] != 1 || hdr->plattform[2] != 1)
    {
        DPRINTF("%s: Unknown plattform data. Aborting load.\n", FUNCNAME);
        return false;
    }

    if (hdr->plattform[1] == 32)
    {
        is_64 = false;
    }
    else if (hdr->plattform[1] == 64)
    {
        is_64 = true;
    }
    else
    {
        DPRINTF("%s: file is for unknonwn pointer size %d\n", FUNCNAME, hdr->plattform[1]);
        return false;
    }

    unk_0C = hdr->unk_0C;

    if (hdr->num_subfiles != 2)
    {
        DPRINTF("%s: I can't process a number of subfiles different to 2.\n", FUNCNAME);
        return false;
    }

    IGGYSubFileEntry *entries = (IGGYSubFileEntry *)(hdr+1);

    if (entries[0].id != 1)
    {
        DPRINTF("%s: Cannot process first section id different to 1.\n", FUNCNAME);
        return false;
    }

    if (entries[1].id != 0)
    {
        DPRINTF("%s: Cannot process second section id different to 0.\n", FUNCNAME);
        return false;
    }
}

void IggyFile::SaveFlashData32(uint8_t *buf, size_t size)
{
    IGGYFlashHeader32 *hdr = (IGGYFlashHeader32 *)buf;

    memcpy(buf, main_section.data(), main_section.size());
    memcpy(buf+main_section.size(), as3_names_section.data(), as3_names_section.size());
    memcpy(buf+main_section.size()+as3_names_section.size(), as3_code_section.data(), as3_code_section.size());
    memcpy(buf+main_section.size()+as3_names_section.size()+as3_code_section.size(), names_section.data(), names_section.size());
    memcpy(buf+main_section.size()+as3_names_section.size()+as3_code_section.size()+names_section.size(), last_section.data(), last_section.size());

    hdr->as3_section_offset = (uint32_t)main_section.size();

    if (hdr->names_offset != 1)
        hdr->names_offset = (uint32_t) (main_section.size()+as3_names_section.size()+as3_code_section.size()-offsetof(IGGYFlashHeader32, names_offset));

    if (hdr->last_section_offset != 1)
        hdr->last_section_offset = (uint32_t) (size-last_section.size()-offsetof(IGGYFlashHeader32, last_section_offset));

    if (hdr->as3_code_offset != 1)
        hdr->as3_code_offset = (uint32_t) (main_section.size()+as3_names_section.size()-offsetof(IGGYFlashHeader32, as3_code_offset));

    if (hdr->as3_names_offset != 1)
        hdr->as3_names_offset = (uint32_t) (main_section.size()+8-offsetof(IGGYFlashHeader32, as3_names_offset));
}

void IggyFile::SaveFlashData64(uint8_t *buf, size_t size)
{
    IGGYFlashHeader64 *hdr = (IGGYFlashHeader64 *)buf;

    memcpy(buf, main_section.data(), main_section.size());
    memcpy(buf+main_section.size(), as3_names_section.data(), as3_names_section.size());
    memcpy(buf+main_section.size()+as3_names_section.size(), as3_code_section.data(), as3_code_section.size());
    memcpy(buf+main_section.size()+as3_names_section.size()+as3_code_section.size(), names_section.data(), names_section.size());
    memcpy(buf+main_section.size()+as3_names_section.size()+as3_code_section.size()+names_section.size(), last_section.data(), last_section.size());

    hdr->as3_section_offset = (uint64_t)main_section.size();

    if (hdr->names_offset != 1)
        hdr->names_offset = (uint64_t) (main_section.size()+as3_names_section.size()+as3_code_section.size()-offsetof(IGGYFlashHeader64, names_offset));

    if (hdr->last_section_offset != 1)
        hdr->last_section_offset = (uint64_t) (size-last_section.size()-offsetof(IGGYFlashHeader64, last_section_offset));

    if (hdr->as3_code_offset != 1)
        hdr->as3_code_offset = (uint64_t) (main_section.size()+as3_names_section.size()-offsetof(IGGYFlashHeader64, as3_code_offset));

    if (hdr->as3_names_offset != 1)
        hdr->as3_names_offset = (uint64_t) (main_section.size()+0xC-offsetof(IGGYFlashHeader64, as3_names_offset));
}

size_t IggyFile::GetFlashDataSize()
{
    return main_section.size() + as3_names_section.size() + as3_code_section.size() + names_section.size() + last_section.size();
}

uint8_t *IggyFile::Save(size_t *psize)
{
    size_t size = sizeof(IGGYHeader) + 2*sizeof(IGGYSubFileEntry);
    uint32_t offset = (uint32_t)size;

    size += GetFlashDataSize();
    size += index_data.size();

    uint8_t *buf = new uint8_t[size];
    //memset(buf, 0, size);

    IGGYHeader *hdr = (IGGYHeader *)buf;
    IGGYSubFileEntry *entries = (IGGYSubFileEntry *)(hdr+1);

    hdr->signature = IGGY_SIGNATURE;
    hdr->version = 0x900;
    hdr->plattform[0] = hdr->plattform[2] = 1;
    hdr->plattform[1] = (is_64) ? 64 : 32;
    hdr->unk_0C = unk_0C;
    hdr->num_subfiles = 2;

    entries[0].id = 1;
    entries[0].size = entries[0].size2 = (uint32_t)GetFlashDataSize();
    entries[0].offset = offset;

    if (is_64)
        SaveFlashData64(buf+offset, GetFlashDataSize());
    else
        SaveFlashData32(buf+offset, GetFlashDataSize());

    offset += entries[0].size;

    entries[1].id = 0;
    entries[1].size = entries[1].size2 = (uint32_t)index_data.size();
    entries[1].offset = offset;
    memcpy(buf+offset, index_data.data(), index_data.size());
    offset += entries[1].size;

    assert(offset == size);

    *psize = size;
    return buf;
}

uint8_t *IggyFile::GetAbcBlob(uint32_t *psize) const
{
    if (as3_code_section.size() == 0)
        return nullptr;

    const uint8_t *ptr = as3_code_section.data();
    *psize = (is_64) ? *(uint32_t *)&ptr[8] : *(uint32_t *)&ptr[4];

    uint8_t *buf = new uint8_t[*psize];
    memcpy(buf, (is_64) ? ptr+0xC : ptr+8, *psize);
    return buf;
}

bool IggyFile::SetAbcBlob(void *buf, uint32_t size)
{
}

bool IggyFile::RebuildIndexAfterAbc(int32_t diff)
{
}

void IggyFile::PrintIndex() const
{
}
