#ifndef __CSBFILE_H__
#define __CSBFILE_H__

#include "AcbFile.h"

class CsbFile : public AcbFile
{
private:

    int se_row;
    bool data_modified;

    struct TrackData
    {
        uint8_t nch;
        uint32_t sfreq;
        uint32_t nsmpl;
    };

    std::vector<TrackData> tracks_data;

protected:

    void ResetCSB();

public:

    CsbFile();
    virtual ~CsbFile();

    virtual bool Load(const uint8_t *buf, size_t size) override;
    virtual uint8_t *Save(size_t *psize) override;

    bool SetTrackData(size_t track, uint8_t num_channels, uint32_t sample_rate, uint32_t num_samples);
};

#endif // __CSBFILE_H__
