#ifndef PSEUDOCRIWAREAUDIOCONTAINER_H
#define PSEUDOCRIWAREAUDIOCONTAINER_H

#include "CriwareAudioContainer.h"

class PseudoCriwareAudioContainer : public CriwareAudioContainer
{
public:
    PseudoCriwareAudioContainer() { }
    virtual ~PseudoCriwareAudioContainer() { }

    virtual bool HasAwb() const override { return false; }
    virtual uint8_t *GetAwb(uint32_t *) const override { return nullptr; }
    virtual bool SetAwb(void *, uint32_t, bool) override { return false; }

    virtual const uint8_t *GetExternalAwbHash() const override { return nullptr; }
    virtual bool SetExternalAwbHash(uint8_t *) override { return false; }

    virtual bool HasAwbHeader() const override { return false; }
    virtual uint8_t *GetAwbHeader(uint32_t *) const override { return nullptr; }
    virtual bool SetAwbHeader(void *, uint32_t, bool) override { return nullptr; }
};

#endif // PSEUDOCRIWAREAUDIOCONTAINER_H
