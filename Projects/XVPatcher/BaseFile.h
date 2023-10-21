#include <stdint.h>

#ifndef __BASEFILE_H__
#define __BASEFILE_H__

class BaseFile
{
protected:
	
	bool big_endian;
	
	uint64_t val64(uint64_t val);
	uint32_t val32(uint32_t val);
	uint16_t val16(uint16_t val);
	float val_float(float val);
	
	uint8_t *GetOffsetPtr(void *base, uint32_t offset, bool native=false);
	uint8_t *GetOffsetPtr(void *base, uint32_t *offsets_table, uint32_t idx, bool native=false);

	uint32_t DifPointer(void *ptr1, void *ptr2); // ptr1-ptr2
	
	void ModTable(uint32_t *offsets, unsigned int count, int mod_by);	
	
	unsigned int GetHighestValueIdx(uint32_t *values, unsigned int count, bool native=false);
};

#endif
