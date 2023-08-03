#include <windows.h>

#include "EpatchFile.h"
#include "PatchUtils.h"
#include "debug.h"

#define WILDCARD    0x777
#define DEFAULT_SEARCH_DOWN 0x100000
#define DEFAULT_SEARCH_UP   0x100000

#define LOG_PARAMS_LIMIT    10

using namespace PatchUtils;

#define CSTR(s) ((s.length() == 0) ? nullptr : s.c_str())
typedef void (* SETUP_FUNCTION)(void *);

static bool build_pattern(const std::string &c_code, std::vector<uint16_t> &pattern)
{
    std::string code = c_code;

    for (size_t i = 0; i < code.length(); i++)
    {
        if (code[i] == ' ')
        {
            code.erase(code.begin()+i);
            i--;
        }
    }

    size_t len = code.length();
    if (len == 0 || (len&1))
        return false;

    pattern.resize(len / 2);

    for (size_t i = 0; i < pattern.size(); i++)
    {
        char high, low;

        high = code[i*2];
        low = code[(i*2)+1];

        if (high == 'X' || high == 'x')
        {
            if (low != 'X' && low != 'x')
                return false;

            pattern[i] = WILDCARD;
        }
        else
        {
            uint8_t byte;

            if (high >= '0' && high <= '9')
                byte = (high-'0');
            else if (high >= 'A' && high <= 'F')
                byte = high-'A'+0xA;
            else if (high >= 'a' && high <= 'f')
                byte = high-'a'+0xA;
            else
                return false;

            byte <<= 4;

            if (low >= '0' && low <= '9')
                byte |= (low-'0');
            else if (low >= 'A' && low <= 'F')
                byte |= low-'A'+0xA;
            else if (low >= 'a' && low <= 'f')
                byte |= low-'a'+0xA;
            else
                return false;

            pattern[i] = byte;
        }
    }

    return true;
}

bool EInstruction::Compile(const TiXmlElement *root)
{
    std::string code;

    if (!Utils::ReadAttrString(root, "code", code))
        return false;

    if (!build_pattern(code, search_pattern))
        return false;

    if (!Utils::ReadAttrString(root, "comment", comment))
        comment.clear();

    return true;
}

std::vector<EPatch> EPatch::log_patches;
Mutex EPatch::log_mutex;

// Keep lower case
static const std::vector<std::string> epatch_types =
{
    "write",
    "nop",
    "hook",
    "log",
    "notify",
};

static const std::vector<std::string> hook_types =
{
    "normal",
    "call"
};

static const std::vector<std::string> log_param_types =
{
    "u8",
    "u16",
    "u32",
    "u64",
    "s8",
    "s16",
    "s32",
    "s64",
    "float",
    "double",
    "cstr",
    "wstr",
    "ptr",
};

static const std::vector<std::string> calling_conventions =
{
    "cdecl",
    "stdcall",
    "thiscall",
    "fastcall"
};

bool EPatch::IsPattern(size_t address, const std::vector<uint16_t> &pattern) const
{
    size_t length = pattern.size();
    uint8_t *module_top = (uint8_t *)GetModuleHandleA(CSTR(module));

    for (size_t i = 0; i < length; i++)
    {
        if (pattern[i] >= 0x100)
            continue;

        if (module_top[address+i] != pattern[i])
            return false;
    }

    return true;
}

uint8_t *EPatch::Find()
{
    if (type >= EPATCH_TYPE_MAX)
        return nullptr;

    if (rebuild)
    {
        if (instructions.size() == 0)
            return nullptr;

        size_t total_size = 0;
        for (const EInstruction &ins : instructions)
        {
            total_size += ins.search_pattern.size();
        }

        search_pattern.resize(total_size);
        uint16_t *data = search_pattern.data();

        //DPRINTF("total_size = %I64x\n", total_size);

        for (const EInstruction &ins : instructions)
        {
            memcpy(data, ins.search_pattern.data(), ins.search_pattern.size()*sizeof(uint16_t));
            data += ins.search_pattern.size();
        }

        rebuild = false;
    }

    size_t address_lowest, address_highest;
    size_t address_down, address_up;

    address_lowest = (search_start < search_down) ? 0 : search_start-search_down;
    address_highest = search_start+search_up;

    address_down=search_start;
    address_up=search_start+1;

    bool search_down_end = (address_down==address_lowest);
    bool search_up_end = (address_up==address_highest);

    while (!search_down_end || !search_up_end)
    {
        if (!search_down_end)
        {
            if (IsPattern(address_down, search_pattern))
            {
                return (uint8_t *)GetPtr(address_down, CSTR(module));
            }

            address_down--;
            if (address_down == address_lowest)
                search_down_end = true;
        }

        if (!search_up_end)
        {
            if (IsPattern(address_up, search_pattern))
            {
                return (uint8_t *)GetPtr(address_up, (CSTR(module)));
            }

            address_up++;
            if (address_up == address_highest)
                search_up_end = true;
        }
    }

    return nullptr;
}

void EPatch::AddLogPatch()
{
    log_patches.push_back(*this);
}

EPatch *EPatch::FindLogPatch(void *addr)
{
    for (EPatch &patch : log_patches)
    {
        if (patch.log_func == addr)
            return &patch;
    }

    return nullptr;
}

void EPatch::LogParam(size_t param, int index)
{
    if ((size_t)index >= log_select.size())
        return;

    if (!log_select[index])
        return;

    int extra = log_extra[index];
    const char *param_name = log_params[index].c_str();
    const std::vector<int> &deep = log_deep[index];

    for (size_t i = 0; i < deep.size(); i++)
    {
        if (param == 0)
        {
            DPRINTF("%s: <NULL>\n", param_name);
            return;
        }

        param = (size_t) ( ((uint8_t *)param) + deep[i] );
        param = *(size_t *)param;
    }

    switch (log_types[index])
    {
        case LOG_PARAM_TYPE_U8:
        {
            if (extra == LOG_EXTRA_DECIMAL)
            {
                DPRINTF("%s: %u\n", param_name, (uint8_t)param);
            }
            else if (extra == LOG_EXTRA_HEXADECIMAL_ALIGNED)
            {
                DPRINTF("%s: 0x%02x\n", param_name, (uint8_t)param);
            }
            else
            {
                DPRINTF("%s: 0x%x\n", param_name, (uint8_t)param);
            }
        }
        break;

        case LOG_PARAM_TYPE_U16:
        {
            if (extra == LOG_EXTRA_DECIMAL)
            {
                DPRINTF("%s: %u\n", param_name, (uint16_t)param);
            }
            else if (extra == LOG_EXTRA_HEXADECIMAL_ALIGNED)
            {
                DPRINTF("%s: 0x%04x\n", param_name, (uint16_t)param);
            }
            else
            {
                DPRINTF("%s: 0x%x\n", param_name, (uint16_t)param);
            }
        }
        break;

        case LOG_PARAM_TYPE_U32:
        {
            if (extra == LOG_EXTRA_DECIMAL)
            {
                DPRINTF("%s: %u\n", param_name, (uint32_t)param);
            }
            else if (extra == LOG_EXTRA_HEXADECIMAL_ALIGNED)
            {
                DPRINTF("%s: 0x%08x\n", param_name, (uint32_t)param);
            }
            else
            {
                DPRINTF("%s: 0x%x\n", param_name, (uint32_t)param);
            }
        }
        break;

#ifndef CPU_X86
        case LOG_PARAM_TYPE_U64:
        {
            if (extra == LOG_EXTRA_DECIMAL)
            {
                DPRINTF("%s: %I64u\n", param_name, param);
            }
            else if (extra == LOG_EXTRA_HEXADECIMAL_ALIGNED)
            {
                DPRINTF("%s: 0x%016I64x\n", param_name, param);
            }
            else
            {
                DPRINTF("%s: 0x%I64x\n", param_name, param);
            }
        }
        break;
#endif

        case LOG_PARAM_TYPE_S8:
        {
            if (extra == LOG_EXTRA_DECIMAL)
            {
                DPRINTF("%s: %d\n", param_name, (int8_t)param);
            }
            else if (extra == LOG_EXTRA_HEXADECIMAL_ALIGNED)
            {
                DPRINTF("%s: 0x%02x\n", param_name, (int8_t)param);
            }
            else
            {
                DPRINTF("%s: 0x%x\n", param_name, (int8_t)param);
            }
        }
        break;

        case LOG_PARAM_TYPE_S16:
        {
            if (extra == LOG_EXTRA_DECIMAL)
            {
                DPRINTF("%s: %d\n", param_name, (int16_t)param);
            }
            else if (extra == LOG_EXTRA_HEXADECIMAL_ALIGNED)
            {
                DPRINTF("%s: 0x%04x\n", param_name, (int16_t)param);
            }
            else
            {
                DPRINTF("%s: 0x%x\n", param_name, (int16_t)param);
            }
        }
        break;

        case LOG_PARAM_TYPE_S32:
        {
            if (extra == LOG_EXTRA_DECIMAL)
            {
                DPRINTF("%s: %d\n", param_name, (int32_t)param);
            }
            else if (extra == LOG_EXTRA_HEXADECIMAL_ALIGNED)
            {
                DPRINTF("%s: 0x%08x\n", param_name, (int32_t)param);
            }
            else
            {
                DPRINTF("%s: 0x%x\n", param_name, (int32_t)param);
            }
        }
        break;
		
#ifndef CPU_X86
        case LOG_PARAM_TYPE_S64:
        {
            if (extra == LOG_EXTRA_DECIMAL)
            {
                DPRINTF("%s: %I64d\n", param_name, (int64_t)param);
            }
            else if (extra == LOG_EXTRA_HEXADECIMAL_ALIGNED)
            {
                DPRINTF("%s: 0x%016I64x\n", param_name, (int64_t)param);
            }
            else
            {
                DPRINTF("%s: 0x%I64x\n", param_name, (int64_t)param);
            }
        }
        break;		
#endif

        case LOG_PARAM_TYPE_CSTR:
            DPRINTF("%s: \"%s\"\n", param_name, (const char *)param);
        break;

        case LOG_PARAM_TYPE_WSTR:
            DPRINTF("%s: \"%S\"\n", param_name, (const wchar_t *)param);
        break;

        case LOG_PARAM_TYPE_PTR:
        {
            if (param == 0)
            {
                DPRINTF("%s: <NULL>\n", param_name);
                break;
            }

            int size = log_ptr_sizes[index];
            int num_lines = size / extra;

            if ((size % extra) != 0)
                num_lines++;

            if (num_lines == 1)
                DPRINTF("%s: ", param_name);
            else
                DPRINTF("%s:\n", param_name);

            uint8_t *data = (uint8_t *)param;
            for (int i = 0; i < size; i++)
            {
                DPRINTF("%02X ", data[i]);

                if (i == (size-1) || (i != 0 && ((i+1) % extra) == 0))
                {
                    DPRINTF("\n");
                }
            }
        }
        break;
    }
}

typedef size_t (* FUNC_0PARAMS)(void);
typedef size_t (* FUNC_1PARAMS)(size_t);
typedef size_t (* FUNC_2PARAMS)(size_t, size_t);
typedef size_t (* FUNC_3PARAMS)(size_t, size_t, size_t);
typedef size_t (* FUNC_4PARAMS)(size_t, size_t, size_t, size_t);
typedef size_t (* FUNC_5PARAMS)(size_t, size_t, size_t, size_t, size_t);
typedef size_t (* FUNC_6PARAMS)(size_t, size_t, size_t, size_t, size_t, size_t);
typedef size_t (* FUNC_7PARAMS)(size_t, size_t, size_t, size_t, size_t, size_t, size_t);
typedef size_t (* FUNC_8PARAMS)(size_t, size_t, size_t, size_t, size_t, size_t, size_t, size_t);
typedef size_t (* FUNC_9PARAMS)(size_t, size_t, size_t, size_t, size_t, size_t, size_t, size_t, size_t);
typedef size_t (* FUNC_10PARAMS)(size_t, size_t, size_t, size_t, size_t, size_t, size_t, size_t, size_t, size_t);

size_t EPatch::Log0()
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log0);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }
    }

    FUNC_0PARAMS func = (FUNC_0PARAMS)patch->log_original_func;
    size_t ret = func();

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log1(size_t arg0)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log1);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
    }

    FUNC_1PARAMS func = (FUNC_1PARAMS)patch->log_original_func;
    size_t ret = func(arg0);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log2(size_t arg0, size_t arg1)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log2);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
    }

    FUNC_2PARAMS func = (FUNC_2PARAMS)patch->log_original_func;
    size_t ret = func(arg0, arg1);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log3(size_t arg0, size_t arg1, size_t arg2)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log3);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
    }

    FUNC_3PARAMS func = (FUNC_3PARAMS)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log4(size_t arg0, size_t arg1, size_t arg2, size_t arg3)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log4);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }    

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
    }

    FUNC_4PARAMS func = (FUNC_4PARAMS)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log5(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log5);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }    

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
    }

    FUNC_5PARAMS func = (FUNC_5PARAMS)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3, arg4);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log6(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log6);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }    

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
    }

    FUNC_6PARAMS func = (FUNC_6PARAMS)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3, arg4, arg5);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log7(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log7);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }    

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
    }

    FUNC_7PARAMS func = (FUNC_7PARAMS)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3, arg4, arg5, arg6);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log8(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6, size_t arg7)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log8);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }    

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
        patch->LogParam(arg7, 7);
    }

    FUNC_8PARAMS func = (FUNC_8PARAMS)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
        patch->LogParam(arg7, 7);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log9(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6, size_t arg7, size_t arg8)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log9);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }    

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
        patch->LogParam(arg7, 7);
        patch->LogParam(arg8, 8);
    }

    FUNC_9PARAMS func = (FUNC_9PARAMS)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
        patch->LogParam(arg7, 7);
        patch->LogParam(arg8, 8);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log10(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6, size_t arg7, size_t arg8, size_t arg9)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log10);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }    

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
        patch->LogParam(arg7, 7);
        patch->LogParam(arg8, 8);
        patch->LogParam(arg9, 9);
    }

    FUNC_10PARAMS func = (FUNC_10PARAMS)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
        patch->LogParam(arg7, 7);
        patch->LogParam(arg8, 8);
        patch->LogParam(arg9, 9);
    }

    DPRINTF("\n");

    return ret;
}

#ifdef CPU_X86

typedef size_t (* __stdcall FUNC_0PARAMS_STDCALL)(void);
typedef size_t (* __stdcall FUNC_1PARAMS_STDCALL)(size_t);
typedef size_t (* __stdcall FUNC_2PARAMS_STDCALL)(size_t, size_t);
typedef size_t (* __stdcall FUNC_3PARAMS_STDCALL)(size_t, size_t, size_t);
typedef size_t (* __stdcall FUNC_4PARAMS_STDCALL)(size_t, size_t, size_t, size_t);
typedef size_t (* __stdcall FUNC_5PARAMS_STDCALL)(size_t, size_t, size_t, size_t, size_t);
typedef size_t (* __stdcall FUNC_6PARAMS_STDCALL)(size_t, size_t, size_t, size_t, size_t, size_t);
typedef size_t (* __stdcall FUNC_7PARAMS_STDCALL)(size_t, size_t, size_t, size_t, size_t, size_t, size_t);
typedef size_t (* __stdcall FUNC_8PARAMS_STDCALL)(size_t, size_t, size_t, size_t, size_t, size_t, size_t, size_t);
typedef size_t (* __stdcall FUNC_9PARAMS_STDCALL)(size_t, size_t, size_t, size_t, size_t, size_t, size_t, size_t, size_t);
typedef size_t (* __stdcall FUNC_10PARAMS_STDCALL)(size_t, size_t, size_t, size_t, size_t, size_t, size_t, size_t, size_t, size_t);

size_t EPatch::Log0_Stdcall()
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log0);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }
    }

    FUNC_0PARAMS_STDCALL func = (FUNC_0PARAMS_STDCALL)patch->log_original_func;
    size_t ret = func();

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log1_Stdcall(size_t arg0)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log1);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
    }

    FUNC_1PARAMS_STDCALL func = (FUNC_1PARAMS_STDCALL)patch->log_original_func;
    size_t ret = func(arg0);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log2_Stdcall(size_t arg0, size_t arg1)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log2);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
    }

    FUNC_2PARAMS_STDCALL func = (FUNC_2PARAMS_STDCALL)patch->log_original_func;
    size_t ret = func(arg0, arg1);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log3_Stdcall(size_t arg0, size_t arg1, size_t arg2)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log3);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
    }

    FUNC_3PARAMS_STDCALL func = (FUNC_3PARAMS_STDCALL)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log4_Stdcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log4);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
    }

    FUNC_4PARAMS_STDCALL func = (FUNC_4PARAMS_STDCALL)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log5_Stdcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log5);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
    }

    FUNC_5PARAMS_STDCALL func = (FUNC_5PARAMS_STDCALL)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3, arg4);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log6_Stdcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log6);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
    }

    FUNC_6PARAMS_STDCALL func = (FUNC_6PARAMS_STDCALL)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3, arg4, arg5);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log7_Stdcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log7);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
    }

    FUNC_7PARAMS_STDCALL func = (FUNC_7PARAMS_STDCALL)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3, arg4, arg5, arg6);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log8_Stdcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6, size_t arg7)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log8);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
        patch->LogParam(arg7, 7);
    }

    FUNC_8PARAMS_STDCALL func = (FUNC_8PARAMS_STDCALL)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
        patch->LogParam(arg7, 7);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log9_Stdcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6, size_t arg7, size_t arg8)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log9);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
        patch->LogParam(arg7, 7);
        patch->LogParam(arg8, 8);
    }

    FUNC_9PARAMS_STDCALL func = (FUNC_9PARAMS_STDCALL)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
        patch->LogParam(arg7, 7);
        patch->LogParam(arg8, 8);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log10_Stdcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6, size_t arg7, size_t arg8, size_t arg9)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log10);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
        patch->LogParam(arg7, 7);
        patch->LogParam(arg8, 8);
        patch->LogParam(arg9, 9);
    }

    FUNC_10PARAMS_STDCALL func = (FUNC_10PARAMS_STDCALL)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
        patch->LogParam(arg7, 7);
        patch->LogParam(arg8, 8);
        patch->LogParam(arg9, 9);
    }

    DPRINTF("\n");

    return ret;
}

typedef size_t (* __thiscall FUNC_0PARAMS_THISCALL)(void);
typedef size_t (* __thiscall FUNC_1PARAMS_THISCALL)(size_t);
typedef size_t (* __thiscall FUNC_2PARAMS_THISCALL)(size_t, size_t);
typedef size_t (* __thiscall FUNC_3PARAMS_THISCALL)(size_t, size_t, size_t);
typedef size_t (* __thiscall FUNC_4PARAMS_THISCALL)(size_t, size_t, size_t, size_t);
typedef size_t (* __thiscall FUNC_5PARAMS_THISCALL)(size_t, size_t, size_t, size_t, size_t);
typedef size_t (* __thiscall FUNC_6PARAMS_THISCALL)(size_t, size_t, size_t, size_t, size_t, size_t);
typedef size_t (* __thiscall FUNC_7PARAMS_THISCALL)(size_t, size_t, size_t, size_t, size_t, size_t, size_t);
typedef size_t (* __thiscall FUNC_8PARAMS_THISCALL)(size_t, size_t, size_t, size_t, size_t, size_t, size_t, size_t);
typedef size_t (* __thiscall FUNC_9PARAMS_THISCALL)(size_t, size_t, size_t, size_t, size_t, size_t, size_t, size_t, size_t);
typedef size_t (* __thiscall FUNC_10PARAMS_THISCALL)(size_t, size_t, size_t, size_t, size_t, size_t, size_t, size_t, size_t, size_t);

size_t EPatch::Log0_Thiscall()
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log0);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }
    }

    FUNC_0PARAMS_THISCALL func = (FUNC_0PARAMS_THISCALL)patch->log_original_func;
    size_t ret = func();

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log1_Thiscall(size_t arg0)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log1);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
    }

    FUNC_1PARAMS_THISCALL func = (FUNC_1PARAMS_THISCALL)patch->log_original_func;
    size_t ret = func(arg0);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log2_Thiscall(size_t arg0, size_t arg1)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log2);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
    }

    FUNC_2PARAMS_THISCALL func = (FUNC_2PARAMS_THISCALL)patch->log_original_func;
    size_t ret = func(arg0, arg1);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log3_Thiscall(size_t arg0, size_t arg1, size_t arg2)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log3);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
    }

    FUNC_3PARAMS_THISCALL func = (FUNC_3PARAMS_THISCALL)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log4_Thiscall(size_t arg0, size_t arg1, size_t arg2, size_t arg3)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log4);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
    }

    FUNC_4PARAMS_THISCALL func = (FUNC_4PARAMS_THISCALL)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log5_Thiscall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log5);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
    }

    FUNC_5PARAMS_THISCALL func = (FUNC_5PARAMS_THISCALL)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3, arg4);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log6_Thiscall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log6);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
    }

    FUNC_6PARAMS_THISCALL func = (FUNC_6PARAMS_THISCALL)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3, arg4, arg5);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log7_Thiscall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log7);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
    }

    FUNC_7PARAMS_THISCALL func = (FUNC_7PARAMS_THISCALL)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3, arg4, arg5, arg6);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log8_Thiscall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6, size_t arg7)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log8);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
        patch->LogParam(arg7, 7);
    }

    FUNC_8PARAMS_THISCALL func = (FUNC_8PARAMS_THISCALL)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
        patch->LogParam(arg7, 7);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log9_Thiscall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6, size_t arg7, size_t arg8)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log9);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
        patch->LogParam(arg7, 7);
        patch->LogParam(arg8, 8);
    }

    FUNC_9PARAMS_THISCALL func = (FUNC_9PARAMS_THISCALL)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
        patch->LogParam(arg7, 7);
        patch->LogParam(arg8, 8);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log10_Thiscall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6, size_t arg7, size_t arg8, size_t arg9)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log10);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
        patch->LogParam(arg7, 7);
        patch->LogParam(arg8, 8);
        patch->LogParam(arg9, 9);
    }

    FUNC_10PARAMS_THISCALL func = (FUNC_10PARAMS_THISCALL)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
        patch->LogParam(arg7, 7);
        patch->LogParam(arg8, 8);
        patch->LogParam(arg9, 9);
    }

    DPRINTF("\n");

    return ret;
}

typedef size_t (* __fastcall FUNC_0PARAMS_FASTCALL)(void);
typedef size_t (* __fastcall FUNC_1PARAMS_FASTCALL)(size_t);
typedef size_t (* __fastcall FUNC_2PARAMS_FASTCALL)(size_t, size_t);
typedef size_t (* __fastcall FUNC_3PARAMS_FASTCALL)(size_t, size_t, size_t);
typedef size_t (* __fastcall FUNC_4PARAMS_FASTCALL)(size_t, size_t, size_t, size_t);
typedef size_t (* __fastcall FUNC_5PARAMS_FASTCALL)(size_t, size_t, size_t, size_t, size_t);
typedef size_t (* __fastcall FUNC_6PARAMS_FASTCALL)(size_t, size_t, size_t, size_t, size_t, size_t);
typedef size_t (* __fastcall FUNC_7PARAMS_FASTCALL)(size_t, size_t, size_t, size_t, size_t, size_t, size_t);
typedef size_t (* __fastcall FUNC_8PARAMS_FASTCALL)(size_t, size_t, size_t, size_t, size_t, size_t, size_t, size_t);
typedef size_t (* __fastcall FUNC_9PARAMS_FASTCALL)(size_t, size_t, size_t, size_t, size_t, size_t, size_t, size_t, size_t);
typedef size_t (* __fastcall FUNC_10PARAMS_FASTCALL)(size_t, size_t, size_t, size_t, size_t, size_t, size_t, size_t, size_t, size_t);

size_t EPatch::Log0_Fastcall()
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log0);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }
    }

    FUNC_0PARAMS_FASTCALL func = (FUNC_0PARAMS_FASTCALL)patch->log_original_func;
    size_t ret = func();

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log1_Fastcall(size_t arg0)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log1);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
    }

    FUNC_1PARAMS_FASTCALL func = (FUNC_1PARAMS_FASTCALL)patch->log_original_func;
    size_t ret = func(arg0);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log2_Fastcall(size_t arg0, size_t arg1)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log2);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
    }

    FUNC_2PARAMS_FASTCALL func = (FUNC_2PARAMS_FASTCALL)patch->log_original_func;
    size_t ret = func(arg0, arg1);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log3_Fastcall(size_t arg0, size_t arg1, size_t arg2)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log3);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
    }

    FUNC_3PARAMS_FASTCALL func = (FUNC_3PARAMS_FASTCALL)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log4_Fastcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log4);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
    }

    FUNC_4PARAMS_FASTCALL func = (FUNC_4PARAMS_FASTCALL)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log5_Fastcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log5);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
    }

    FUNC_5PARAMS_FASTCALL func = (FUNC_5PARAMS_FASTCALL)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3, arg4);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log6_Fastcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log6);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
    }

    FUNC_6PARAMS_FASTCALL func = (FUNC_6PARAMS_FASTCALL)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3, arg4, arg5);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log7_Fastcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log7);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
    }

    FUNC_7PARAMS_FASTCALL func = (FUNC_7PARAMS_FASTCALL)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3, arg4, arg5, arg6);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log8_Fastcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6, size_t arg7)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log8);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
        patch->LogParam(arg7, 7);
    }

    FUNC_8PARAMS_FASTCALL func = (FUNC_8PARAMS_FASTCALL)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
        patch->LogParam(arg7, 7);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log9_Fastcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6, size_t arg7, size_t arg8)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log9);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
        patch->LogParam(arg7, 7);
        patch->LogParam(arg8, 8);
    }

    FUNC_9PARAMS_FASTCALL func = (FUNC_9PARAMS_FASTCALL)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
        patch->LogParam(arg7, 7);
        patch->LogParam(arg8, 8);
    }

    DPRINTF("\n");

    return ret;
}

size_t EPatch::Log10_Fastcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6, size_t arg7, size_t arg8, size_t arg9)
{
    MutexLocker lock(&log_mutex);

    EPatch *patch = FindLogPatch((void *)Log10);
    if (!patch)
    {
        DPRINTF("%s: FATAL cannot find patch.\n", FUNCNAME);
        exit(-1);
    }

    if (patch->log_before)
    {
        DPRINTF("\n%s called.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
        patch->LogParam(arg7, 7);
        patch->LogParam(arg8, 8);
        patch->LogParam(arg9, 9);
    }

    FUNC_10PARAMS_FASTCALL func = (FUNC_10PARAMS_FASTCALL)patch->log_original_func;
    size_t ret = func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);

    if (!patch->log_before)
    {
        DPRINTF("\n%s was called and has returned.\n", patch->function.c_str());

        if (patch->log_ra)
        {
            DPRINTF("Return address is %p. Relative: 0x%x\n", BRA(0), (uint32_t)(size_t)BRA(0) - (uint32_t)(size_t)GetModuleHandleA(CSTR(patch->module)));
        }

        patch->LogParam(arg0, 0);
        patch->LogParam(arg1, 1);
        patch->LogParam(arg2, 2);
        patch->LogParam(arg3, 3);
        patch->LogParam(arg4, 4);
        patch->LogParam(arg5, 5);
        patch->LogParam(arg6, 6);
        patch->LogParam(arg7, 7);
        patch->LogParam(arg8, 8);
        patch->LogParam(arg9, 9);
    }

    DPRINTF("\n");

    return ret;
}

#endif // CPU_X86

bool EPatch::Compile(const TiXmlElement *root)
{
    std::string type_str;

    if (!Utils::ReadAttrString(root, "type", type_str))
        return false;

    type = EPATCH_TYPE_MAX;
    for (size_t i = 0 ; i < epatch_types.size(); i++)
    {
        if (Utils::ToLowerCase(type_str) == epatch_types[i])
        {
            type = (int)i;
            break;
        }
    }

    if (type >= EPATCH_TYPE_MAX)
        return false;

    if (!Utils::ReadAttrString(root, "name", name))
        return false;

    if (!Utils::ReadAttrString(root, "comment", comment))
        comment.clear();

    if (!Utils::ReadAttrString(root, "module", module))
        module.clear();

    if (!Utils::ReadAttrString(root, "enabled", enabled) || enabled.length() == 0)
        enabled = "true";

    if (!Utils::ReadAttrUnsigned(root, "search_start", &search_start))
        return false;

    if (!Utils::ReadAttrUnsigned(root, "search_down", &search_down))
        search_down = DEFAULT_SEARCH_DOWN;

    if (!Utils::ReadAttrUnsigned(root, "search_up", &search_up))
        search_up = DEFAULT_SEARCH_UP;

    size_t inst_count = Utils::GetElemCount(root, "Instruction");
    if (inst_count == 0)
        return false;

    instructions.resize(inst_count);

    size_t idx = 0;
    for (const TiXmlElement *elem = root->FirstChildElement(); elem; elem = elem->NextSiblingElement())
    {
        if (elem->ValueStr() == "Instruction")
        {
            EInstruction &instruction = instructions[idx++];

            if (!instruction.Compile(elem))
                return false;
        }
    }

    const TiXmlElement *common = nullptr;

    if (type == EPATCH_TYPE_WRITE)
    {
        std::string data;
        const TiXmlElement *write;

        if (!Utils::GetParamString(root, "Write", data, &write))
            return false;

        if (!build_pattern(data, replace_pattern))
            return false;

        common = write;
    }
    else if (type == EPATCH_TYPE_NOP)
    {
        const TiXmlElement *nop;

        if (Utils::GetElemCount(root, "Nop", &nop) == 0)
            return false;

        if (!Utils::ReadAttrUnsigned(nop, "size", &size))
            size = 0;

        common = nop;
    }
    else if (type == EPATCH_TYPE_HOOK)
    {
        const TiXmlElement *hook;
        std::string hook_type_str;

        if (!Utils::GetParamString(root, "Hook", function, &hook) || function.length() == 0)
            return false;

        if (!Utils::ReadAttrString(hook, "module", function_module))
            function_module = patcher_module;

        if (!Utils::ReadAttrString(hook, "setup", setup_function))
            setup_function.clear();

        if (Utils::ReadAttrString(hook, "type", hook_type_str))
        {
            hook_type = HOOK_TYPE_MAX;

            for (size_t i = 0; i < hook_types.size(); i++)
            {
                if (Utils::ToLowerCase(hook_type_str) == hook_types[i])
                {
                    hook_type = (int)i;
                    break;
                }
            }

            if (hook_type >= HOOK_TYPE_MAX)
                return false;
        }
        else
        {
            hook_type = HOOK_TYPE_NORMAL;
        }

        common = hook;
    }
    else if (type == EPATCH_TYPE_LOG)
    {
        const TiXmlElement *log;
        std::string when;
        std::string cc;
        std::string ra;

        if (!Utils::GetParamMultipleStrings(root, "Log", log_params, &log))
            return false;

        if (log_params.size() > LOG_PARAMS_LIMIT)
        {
            DPRINTF("%s: No more than %d parameters currently supported.\n", FUNCNAME, LOG_PARAMS_LIMIT);
            return false;
        }

        if (Utils::ReadAttrString(log, "ra", ra) && Utils::ToLowerCase(ra) == "true")
        {
            log_ra = true;
        }
        else
        {
            log_ra = false;
        }

        if (!Utils::ReadAttrString(log, "function", function))
            return false;

        if (Utils::ReadAttrString(log, "calling_convention", cc) && cc.length() != 0)
        {
            calling_convention = LOG_CC_MAX;
            for (size_t i = 0 ; i < calling_conventions.size(); i++)
            {
                if (Utils::ToLowerCase(cc) == calling_conventions[i])
                {
                    calling_convention = (int)i;
                    break;
                }
            }

            if (calling_convention >= LOG_CC_MAX)
                return false;

#ifndef CPU_X86
            calling_convention = LOG_CC_CDECL;
#endif
        }
        else
        {
            calling_convention = LOG_CC_CDECL;
        }

        if (Utils::ReadAttrString(log, "when", when) && Utils::ToLowerCase(when) == "after")
        {
            log_before = false;
        }
        else
        {
            log_before = true;
        }

        if (log_params.size() == 0)
        {
            log_types.clear();
            log_select.clear();
            log_extra.clear();
            log_ptr_sizes.clear();
            log_deep.clear();
        }
        else
        {
            std::vector<std::string> types;
            std::string select;
            std::vector<std::string> extra;

            if (!Utils::ReadAttrMultipleStrings(log, "types", types) || types.size() != log_params.size())
                return false;

            log_types.resize(log_params.size());
            log_deep.resize(log_params.size());

            for (size_t i = 0; i < log_types.size(); i++)
            {
                std::vector<std::string> deep;
                Utils::GetMultipleStrings(types[i], deep, ':');

                if (deep.size() == 0)
                    return false;

                int ptype = LOG_PARAM_TYPE_MAX;

                std::vector<int> &this_deep = log_deep[i];
                this_deep.resize(deep.size()-1);

                std::string this_type = Utils::ToLowerCase(deep[0]);
                Utils::TrimString(this_type);

                for (size_t j = 0 ; j < log_param_types.size(); j++)
                {
                    if (this_type == log_param_types[j])
                    {
                        ptype = (int)j;
                        break;
                    }
                }

                if (ptype >= LOG_PARAM_TYPE_MAX)
                    return false;

#ifdef CPU_X86

                if (ptype == LOG_PARAM_TYPE_S64 || ptype == LOG_PARAM_TYPE_U64)
                    return false;
#endif

                if (ptype == LOG_PARAM_TYPE_FLOAT || ptype == LOG_PARAM_TYPE_DOUBLE)
                {
                    DPRINTF("%s: float and double params currently not supported.\n", FUNCNAME);
                    return false;
                }

                log_types[i] = ptype;

                for (size_t j = 1; j < deep.size(); j++)
                {
                    this_deep[j-1] = Utils::GetUnsigned(deep[j]);
                }
            }

            if (Utils::ReadAttrString(log, "select", select))
            {
                Utils::TrimString(select);

                if (select.length() != log_params.size())
                    return false;

                log_select.resize(log_params.size());

                for (size_t i = 0; i < log_select.size(); i++)
                {
                    char bool_value = select[i];

                    if (bool_value == '1')
                        log_select[i] = true;
                    else if (bool_value == '0')
                        log_select[i] = false;
                    else
                        return false;
                }
            }
            else
            {
                log_select.clear();
                log_select.resize(log_params.size(), true);
            }

            if (Utils::ReadAttrMultipleStrings(log, "extra", extra))
            {
                if (extra.size() != log_params.size())
                    return false;

                log_extra.resize(log_params.size());
                log_ptr_sizes.resize(log_params.size());

                for (size_t i = 0; i < log_extra.size(); i++)
                {
                    int type = log_types[i];
                    std::string modifier = Utils::ToLowerCase(extra[i]);
                    Utils::TrimString(modifier);

                    if (type < LOG_PARAM_TYPE_PTR)
                    {
                        if (modifier == "d")
                            log_extra[i] = LOG_EXTRA_DECIMAL;
                        else if (modifier == "ha")
                            log_extra[i] = LOG_EXTRA_HEXADECIMAL_ALIGNED;
                        else
                            log_extra[i] = LOG_EXTRA_HEXADECIMAL;
                    }
                    else
                    {
                        size_t split = modifier.find(':');

                        if (split == std::string::npos)
                        {
                            log_ptr_sizes[i] = Utils::GetUnsigned(modifier);
                            log_extra[i] = 16;
                        }
                        else
                        {
                            log_ptr_sizes[i] =  Utils::GetUnsigned(modifier.substr(0, split));
                            log_extra[i] = Utils::GetUnsigned(modifier.substr(split+1));
                        }

                        if (log_extra[i] == 0)
                            log_extra[i] = 16;

                        if (log_ptr_sizes[i] == 0)
                            log_ptr_sizes[i] = 16;
                    }
                }
            }
            else
            {
                log_extra.resize(log_params.size());

                for (size_t i = 0; i < log_extra.size(); i++)
                {
                    int type = log_types[i];

                    if (type < LOG_PARAM_TYPE_PTR)
                        log_extra[i] = 0;
                    else
                        log_extra[i] = 16;
                }

                log_ptr_sizes.clear();
                log_ptr_sizes.resize(log_params.size(), 16);
            }
        }

        common = log;
    }
    else if (type == EPATCH_TYPE_NOTIFY)
    {
        const TiXmlElement *notify;

        if (!Utils::GetParamString(root, "Notify", function, &notify) || function.length() == 0)
            return false;

        if (!Utils::ReadAttrString(notify, "module", function_module))
            function_module = patcher_module;

        common = notify;
    }

    if (!Utils::ReadAttrUnsigned(common, "inst_index", &inst_index))
        inst_index = 0;

    if (!Utils::ReadAttrUnsigned(common, "inst_offset", &inst_offset))
        inst_offset = 0;

    if (inst_index >= instructions.size())
        return false;

    if (inst_offset >= instructions[inst_index].search_pattern.size())
        return false;

    if (type == EPATCH_TYPE_NOP && size == 0)
    {
        size = instructions[inst_index].search_pattern.size() - inst_offset;

        for (size_t i = inst_offset+1; i < instructions.size(); i++)
        {
            size += instructions[i].search_pattern.size();
        }
    }

    rebuild = true;
    return true;
}

bool EPatch::Apply()
{
    uint8_t *ptr = Find();
    if (!ptr)
        return false;

    for (size_t i = 0; i < inst_index; i++)
        ptr += instructions[i].search_pattern.size();

    ptr += inst_offset;

    DPRINTF("Patch \"%s\" located at address %p\n", name.c_str(), ptr);

    if (type == EPATCH_TYPE_WRITE)
    {
        for (uint16_t byte : replace_pattern)
        {
            if (byte < 0x100)
                Write8(ptr, (uint8_t)byte);

            ptr++;
        }
    }
    else if (type == EPATCH_TYPE_NOP)
    {
        Nop(ptr, size);
    }
    else if (type == EPATCH_TYPE_HOOK)
    {
        HMODULE mod = GetModuleHandleA(CSTR(function_module));
        if (!mod)
            return false;

        uint8_t *func = (uint8_t *)GetProcAddress(mod, function.c_str());
        if (!func)
            return false;

        void *orig;        

        if (hook_type == HOOK_TYPE_NORMAL)
        {
            if (!Hook(ptr, &orig, func))
                return false;
        }
        else
        {
            if (!HookCall(ptr, &orig, func))
                return false;
        }

        if (setup_function.length() > 0)
        {
            SETUP_FUNCTION setup = (SETUP_FUNCTION)GetProcAddress(mod, setup_function.c_str());
            if (!setup)
            {
                DPRINTF("%s: Warning, setup function \"%s\" not found.\n", FUNCNAME, setup_function.c_str());
            }

            if (setup)
            {
                setup(orig);
            }
        }        
    }
    else if (type == EPATCH_TYPE_LOG)
    {
        size_t num_params = log_params.size();

        if (calling_convention == LOG_CC_CDECL)
        {
            if (num_params == 0)
            {
                log_func = (void *)Log0;
            }
            else if (num_params == 1)
            {
                log_func = (void *)Log1;
            }
            else if (num_params == 2)
            {
                log_func = (void *)Log2;
            }
            else if (num_params == 3)
            {
                log_func = (void *)Log3;
            }
            else if (num_params == 4)
            {
                log_func = (void *)Log4;
            }
            else if (num_params == 5)
            {
                log_func = (void *)Log5;
            }
            else if (num_params == 6)
            {
                log_func = (void *)Log6;
            }
            else if (num_params == 7)
            {
                log_func = (void *)Log7;
            }
            else if (num_params == 8)
            {
                log_func = (void *)Log8;
            }
            else if (num_params == 9)
            {
                log_func = (void *)Log9;
            }
            else if (num_params == 10)
            {
                log_func = (void *)Log10;
            }
        }
#ifdef CPU_X86
        else if (calling_convention == LOG_CC_STDCAL)
        {
            if (num_params == 0)
            {
                log_func = (void *)Log0_Stdcall;
            }
            else if (num_params == 1)
            {
                log_func = (void *)Log1_Stdcall;
            }
            else if (num_params == 2)
            {
                log_func = (void *)Log2_Stdcall;
            }
            else if (num_params == 3)
            {
                log_func = (void *)Log3_Stdcall;
            }
            else if (num_params == 4)
            {
                log_func = (void *)Log4_Stdcall;
            }
            else if (num_params == 5)
            {
                log_func = (void *)Log5_Stdcall;
            }
            else if (num_params == 6)
            {
                log_func = (void *)Log6_Stdcall;
            }
            else if (num_params == 7)
            {
                log_func = (void *)Log7_Stdcall;
            }
            else if (num_params == 8)
            {
                log_func = (void *)Log8_Stdcall;
            }
            else if (num_params == 9)
            {
                log_func = (void *)Log9_Stdcall;
            }
            else if (num_params == 10)
            {
                log_func = (void *)Log10_Stdcall;
            }
        }
        else if (calling_convention == LOG_CC_THISCALL)
        {
            if (num_params == 0)
            {
                log_func = (void *)Log0_Thiscall;
            }
            else if (num_params == 1)
            {
                log_func = (void *)Log1_Thiscall;
            }
            else if (num_params == 2)
            {
                log_func = (void *)Log2_Thiscall;
            }
            else if (num_params == 3)
            {
                log_func = (void *)Log3_Thiscall;
            }
            else if (num_params == 4)
            {
                log_func = (void *)Log4_Thiscall;
            }
            else if (num_params == 5)
            {
                log_func = (void *)Log5_Thiscall;
            }
            else if (num_params == 6)
            {
                log_func = (void *)Log6_Thiscall;
            }
            else if (num_params == 7)
            {
                log_func = (void *)Log7_Thiscall;
            }
            else if (num_params == 8)
            {
                log_func = (void *)Log8_Thiscall;
            }
            else if (num_params == 9)
            {
                log_func = (void *)Log9_Thiscall;
            }
            else if (num_params == 10)
            {
                log_func = (void *)Log10_Thiscall;
            }
        }
        else if (calling_convention == LOG_CC_FASTCALL)
        {
            if (num_params == 0)
            {
                log_func = (void *)Log0_Fastcall;
            }
            else if (num_params == 1)
            {
                log_func = (void *)Log1_Fastcall;
            }
            else if (num_params == 2)
            {
                log_func = (void *)Log2_Fastcall;
            }
            else if (num_params == 3)
            {
                log_func = (void *)Log3_Fastcall;
            }
            else if (num_params == 4)
            {
                log_func = (void *)Log4_Fastcall;
            }
            else if (num_params == 5)
            {
                log_func = (void *)Log5_Fastcall;
            }
            else if (num_params == 6)
            {
                log_func = (void *)Log6_Fastcall;
            }
            else if (num_params == 7)
            {
                log_func = (void *)Log7_Fastcall;
            }
            else if (num_params == 8)
            {
                log_func = (void *)Log8_Fastcall;
            }
            else if (num_params == 9)
            {
                log_func = (void *)Log9_Fastcall;
            }
            else if (num_params == 10)
            {
                log_func = (void *)Log10_Fastcall;
            }
        }
#endif // CPU_X86
        else
        {
            // This code should be unreachable unless bugs
            assert(0);
            return false;
        }

        MutexLocker lock(&log_mutex);

        if (!Hook(ptr, &log_original_func, log_func))
            return false;

        AddLogPatch();
    }
    else if (type == EPATCH_TYPE_NOTIFY)
    {
        HMODULE mod = GetModuleHandleA(CSTR(function_module));
        if (!mod)
            return false;

        SETUP_FUNCTION setup = (SETUP_FUNCTION)GetProcAddress(mod, function.c_str());
        if (!setup)
            return false;

        setup(ptr);
    }

    return true;
}

int EPatch::GetEnabled(std::string &setting) const
{
    if (enabled == "true" || enabled == "1")
        return true;

    if (enabled == "false" || enabled == "0")
        return false;

    setting = enabled;
    return -1;
}

bool EPatchFile::Compile(TiXmlDocument *doc, bool big_endian)
{
    UNUSED(big_endian);

    TiXmlHandle handle(doc);
    const TiXmlElement *root = Utils::FindRoot(&handle, "EternityPatchFile");

    if (!root)
    {
        DPRINTF("%s: Cannot locate \"EternityPatchFile\" in xml.\n", FUNCNAME);
        return false;
    }

    if (!Utils::ReadAttrString(root, "name", name) || name.length() == 0)
    {
        DPRINTF("%s: name field is mandatory (and cannot be 0 length).\n", FUNCNAME);
        return false;
    }

    if (!Utils::ReadAttrString(root, "enabled", enabled) || enabled.length() == 0)
        enabled = "true";

    if (!Utils::ReadAttrString(root, "comment", comment))
        comment.clear();

    size_t patch_count = Utils::GetElemCount(root, "Patch");
    if (patch_count == 0)
    {
        DPRINTF("%s: No patches in file \"%s\"\n", FUNCNAME, name.c_str());
        return false;
    }

    patches.resize(patch_count);

    size_t idx = 0;
    for (const TiXmlElement *elem = root->FirstChildElement(); elem; elem = elem->NextSiblingElement())
    {
        if (elem->ValueStr() == "Patch")
        {
            EPatch &patch = patches[idx++];
            patch.SetPatcher(patcher_module);

            if (!patch.Compile(elem))
            {
                DPRINTF("%s: Failed to compile Patch at index 0x%x\n", FUNCNAME, (unsigned int)idx-1);
                return false;
            }
        }
    }

    return true;
}

int EPatchFile::GetEnabled(std::string &setting) const
{
    if (enabled == "true" || enabled == "1")
        return true;

    if (enabled == "false" || enabled == "0")
        return false;

    setting = enabled;
    return -1;
}


