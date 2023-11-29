#ifndef __MEMORYSTREAM_H__
#define __MEMORYSTREAM_H__

#include "Stream.h"
#include <cstdint>
#include <string>

typedef __int64 off64_t; // For Windows

class MemoryStream : public Stream {
protected:
    uint8_t *mem;
    uint64_t file_size;
    uint64_t file_pos;
    uint64_t capacity;

public:
    MemoryStream();
    virtual ~MemoryStream();

    uint8_t *GetMemory(bool unlink);
    void SetMemory(void *buf, size_t size);
    bool FastRead(uint8_t **pbuf, size_t size);

    // Overrides from Stream
    virtual bool Resize(uint64_t size) override;

    virtual bool Read(void *buf, size_t size) override;
    virtual bool Write(const void *buf, size_t size) override;

    virtual bool Seek(off64_t offset, int whence);

    virtual bool Load(const uint8_t *buf, size_t size);
    virtual uint8_t *Save(size_t *psize);

    virtual bool LoadFromFile(const std::string &path, bool show_error = true);
    virtual bool SaveToFile(const std::string &path, bool show_error = true, bool build_path = false) ;

    virtual bool Align(unsigned int alignment) override;
};

#endif /* __MEMORYSTREAM_H__ */