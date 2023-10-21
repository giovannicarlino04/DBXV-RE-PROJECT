#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include "BaseFile.h"
#include "common.h"

uint64_t BaseFile::val64(uint64_t val)
{
#ifdef __BIG_ENDIAN__
	return (big_endian) ? val : LE64(val)
#else
	return (big_endian) ? BE64(val) : val;
#endif
}

uint32_t BaseFile::val32(uint32_t val)
{
#ifdef __BIG_ENDIAN__
	return (big_endian) ? val : LE32(val)
#else
	return (big_endian) ? BE32(val) : val;
#endif
}

uint16_t BaseFile::val16(uint16_t val)
{
#ifdef __BIG_ENDIAN__
	return (big_endian) ? val : LE16(val)
#else
	return (big_endian) ? BE16(val) : val;
#endif
}

float BaseFile::val_float(float val)
{
	uint32_t *p = (uint32_t *)&val;
	
	*p = val32(*p);
	
	return val;
}

uint8_t *BaseFile::GetOffsetPtr(void *base, uint32_t offset, bool native)
{
	if (native)
		return ((uint8_t *)base+offset);

	return ((uint8_t *)base+val32(offset));
}

uint8_t *BaseFile::GetOffsetPtr(void *base, uint32_t *offsets_table, uint32_t idx, bool native)
{
	if (native)
		return ((uint8_t *)base+offsets_table[idx]);
	
	return ((uint8_t *)base+val32(offsets_table[idx]));
}

uint32_t BaseFile::DifPointer(void *ptr1, void *ptr2)
{
	return (uint32_t) ((uint64_t)ptr1 - (uint64_t)ptr2);
}

void BaseFile::ModTable(uint32_t *offsets, unsigned int count, int mod_by)
{
	for (unsigned int i = 0; i < count; i++)
	{
		offsets[i] = val32(val32(offsets[i]) + mod_by);
	}
}

unsigned int BaseFile::GetHighestValueIdx(uint32_t *values, unsigned int count, bool native)
{
	unsigned int max_idx = 0xFFFFFFFF;
	uint32_t max = 0;
	
	for (unsigned int i = 0; i < count; i++)
	{
		uint32_t val = (native) ? values[i] : val32(values[i]);
		
		if (val > max)
		{
			max = val;
			max_idx = i;
		}
	}
	
	printf("Higuest value = %x.\n", max);
	return max_idx;	
}

