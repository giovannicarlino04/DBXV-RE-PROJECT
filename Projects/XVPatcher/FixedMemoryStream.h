#ifndef __FIXEDMEMORYSTREAM_H__
#define __FIXEDMEMORYSTREAM_H__

#include "MemoryStream.h"

class FixedMemoryStream : public MemoryStream
{
public:

    FixedMemoryStream(uint8_t *buf, size_t size);
    virtual ~FixedMemoryStream();

    virtual bool Resize(uint64_t size) override
    {
        return false;
    }

    virtual bool Load(const uint8_t *buf, size_t size)
    {
        return false;
    }

    virtual bool LoadFromFile(const std::string &path, bool show_error=true)
    {
        return false;
    }

    virtual bool Copy(Stream *other, size_t size) override;
};

#endif // FIXEDMEMORYSTREAM_H
