#ifndef __CRIWAREAUDIOCONTAINER_H__
#define __CRIWAREAUDIOCONTAINER_H__

#include "BaseFile.h"

class CriwareAudioContainer : public BaseFile
{
public:

	virtual ~CriwareAudioContainer() {}
	
	virtual bool HasAwb() const = 0;
	virtual uint8_t *GetAwb(uint32_t *awb_size) const = 0;
    virtual bool SetAwb(void *awb, uint32_t awb_size, bool take_ownership=false) = 0;
	
    virtual const uint8_t *GetExternalAwbHash() const = 0;
	virtual bool SetExternalAwbHash(uint8_t *hash) = 0;
	
	virtual bool HasAwbHeader() const = 0;
	virtual uint8_t *GetAwbHeader(uint32_t *header_size) const = 0;		
    virtual bool SetAwbHeader(void *header, uint32_t header_size, bool take_ownership) = 0;
};

#endif // __CRIWAREAUDIOCONTAINER_H__
