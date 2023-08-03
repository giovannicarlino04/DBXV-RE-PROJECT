#ifndef EPATCHFILE_H
#define EPATCHFILE_H

#include <windef.h>
#include <stdexcept>
#include "BaseFile.h"
#include "Mutex.h"

struct EInstruction
{
    std::string comment;
    std::vector<uint16_t> search_pattern;

    bool Compile(const TiXmlElement *root);
};

enum EPatchType
{
    EPATCH_TYPE_WRITE,
    EPATCH_TYPE_NOP,
    EPATCH_TYPE_HOOK,
    EPATCH_TYPE_LOG,
    EPATCH_TYPE_NOTIFY,
    EPATCH_TYPE_MAX
};

enum HookType
{
    HOOK_TYPE_NORMAL,
    HOOK_TYPE_CALL,
    HOOK_TYPE_MAX
};

enum LogParamType
{
    LOG_PARAM_TYPE_U8,
    LOG_PARAM_TYPE_U16,
    LOG_PARAM_TYPE_U32,
    LOG_PARAM_TYPE_U64,
    LOG_PARAM_TYPE_S8,
    LOG_PARAM_TYPE_S16,
    LOG_PARAM_TYPE_S32,
    LOG_PARAM_TYPE_S64,
    LOG_PARAM_TYPE_FLOAT,
    LOG_PARAM_TYPE_DOUBLE,
    LOG_PARAM_TYPE_CSTR,
    LOG_PARAM_TYPE_WSTR,
    LOG_PARAM_TYPE_PTR,
    LOG_PARAM_TYPE_MAX
};

enum LogExtra
{
    LOG_EXTRA_HEXADECIMAL,
    LOG_EXTRA_DECIMAL,
    LOG_EXTRA_HEXADECIMAL_ALIGNED,
};

enum LogCallingConvention
{
    LOG_CC_CDECL,
    LOG_CC_STDCAL,
    LOG_CC_THISCALL,
    LOG_CC_FASTCALL,
    LOG_CC_MAX
};

class EPatch
{
private:

    // Global data
    static std::vector<EPatch> log_patches;
    static Mutex log_mutex;

    // Non serialized data
    std::string patcher_module;

    bool rebuild = true;
    std::vector<uint16_t> search_pattern; // instructions combined in a single item

    // Common
    std::string name;
    std::string comment;
    int type;
    std::string module;
    std::string enabled;

    size_t search_start;
    size_t search_down, search_up;

    std::vector<EInstruction> instructions;
    // Write
    std::vector<uint16_t> replace_pattern;

    // Hook
    std::string function; // For hook, a function name in the patcher_module; For log, this is just a friendly string to be displayed
    std::string function_module;
    std::string setup_function;
    int hook_type;

    // Log
    bool log_before;
    std::vector<std::string> log_params;
    std::vector<int> log_types;
    std::vector<bool> log_select;
    std::vector<int> log_extra;
    std::vector<int> log_ptr_sizes;
    std::vector<std::vector<int>> log_deep;
    int calling_convention;
    void *log_func;
    void *log_original_func;
    bool log_ra;

    // Common for some
    size_t inst_index;
    size_t inst_offset;
    size_t size;

    //
    bool IsPattern(size_t address, const std::vector<uint16_t> &pattern) const;
    uint8_t *Find();

    //
    void AddLogPatch();
    static EPatch *FindLogPatch(void *addr);

    void LogParam(size_t param, int index);
    static size_t Log0();
    static size_t Log1(size_t arg0);
    static size_t Log2(size_t arg0, size_t arg1);
    static size_t Log3(size_t arg0, size_t arg1, size_t arg2);
    static size_t Log4(size_t arg0, size_t arg1, size_t arg2, size_t arg3);
    static size_t Log5(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4);
    static size_t Log6(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5);
    static size_t Log7(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6);
    static size_t Log8(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6, size_t arg7);
    static size_t Log9(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6, size_t arg7, size_t arg8);
    static size_t Log10(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6, size_t arg7, size_t arg8, size_t arg9);

#ifdef CPU_X86

    static size_t __stdcall Log0_Stdcall();
    static size_t __stdcall Log1_Stdcall(size_t arg0);
    static size_t __stdcall Log2_Stdcall(size_t arg0, size_t arg1);
    static size_t __stdcall Log3_Stdcall(size_t arg0, size_t arg1, size_t arg2);
    static size_t __stdcall Log4_Stdcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3);
    static size_t __stdcall Log5_Stdcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4);
    static size_t __stdcall Log6_Stdcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5);
    static size_t __stdcall Log7_Stdcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6);
    static size_t __stdcall Log8_Stdcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6, size_t arg7);
    static size_t __stdcall Log9_Stdcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6, size_t arg7, size_t arg8);
    static size_t __stdcall Log10_Stdcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6, size_t arg7, size_t arg8, size_t arg9);

    static size_t __thiscall Log0_Thiscall();
    static size_t __thiscall Log1_Thiscall(size_t arg0);
    static size_t __thiscall Log2_Thiscall(size_t arg0, size_t arg1);
    static size_t __thiscall Log3_Thiscall(size_t arg0, size_t arg1, size_t arg2);
    static size_t __thiscall Log4_Thiscall(size_t arg0, size_t arg1, size_t arg2, size_t arg3);
    static size_t __thiscall Log5_Thiscall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4);
    static size_t __thiscall Log6_Thiscall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5);
    static size_t __thiscall Log7_Thiscall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6);
    static size_t __thiscall Log8_Thiscall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6, size_t arg7);
    static size_t __thiscall Log9_Thiscall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6, size_t arg7, size_t arg8);
    static size_t __thiscall Log10_Thiscall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6, size_t arg7, size_t arg8, size_t arg9);

    static size_t __fastcall Log0_Fastcall();
    static size_t __fastcall Log1_Fastcall(size_t arg0);
    static size_t __fastcall Log2_Fastcall(size_t arg0, size_t arg1);
    static size_t __fastcall Log3_Fastcall(size_t arg0, size_t arg1, size_t arg2);
    static size_t __fastcall Log4_Fastcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3);
    static size_t __fastcall Log5_Fastcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4);
    static size_t __fastcall Log6_Fastcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5);
    static size_t __fastcall Log7_Fastcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6);
    static size_t __fastcall Log8_Fastcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6, size_t arg7);
    static size_t __fastcall Log9_Fastcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6, size_t arg7, size_t arg8);
    static size_t __fastcall Log10_Fastcall(size_t arg0, size_t arg1, size_t arg2, size_t arg3, size_t arg4, size_t arg5, size_t arg6, size_t arg7, size_t arg8, size_t arg9);

#endif

public:

    EPatch() : type(EPATCH_TYPE_MAX) { }
    EPatch(const std::string &patcher) : patcher_module(patcher), type(EPATCH_TYPE_MAX) { }

    bool Compile(const TiXmlElement *root);
    bool Apply();

    const std::string &GetName() const
    {
        return name;
    }

    int GetEnabled(std::string &setting) const;
    void SetPatcher(const std::string &patcher) { patcher_module = patcher; }
};

class EPatchFile : public BaseFile
{
    std::string patcher_module;

    std::string name;
    std::string enabled;
    std::string comment;

    std::vector<EPatch> patches;

public:

    EPatchFile() { }
    EPatchFile(const std::string &patcher) : patcher_module(patcher) { }
    virtual ~EPatchFile() { }

    virtual bool Compile(TiXmlDocument *doc, bool big_endian) override;

    inline EPatch &operator[](size_t n) { return patches[n]; }
    inline const EPatch &operator[](size_t n) const { return patches[n]; }

    const std::string GetName() const { return name; }
    int GetEnabled(std::string &setting) const;

    inline EPatch &operator[](const std::string &patch_name)
    {
        for (EPatch &patch : patches)
        {
            if (Utils::ToLowerCase(patch_name) == Utils::ToLowerCase(patch.GetName()))
                return patch;
        }

        throw std::out_of_range("Patch " + patch_name + " doesn't exist.");
    }

    inline const EPatch &operator[](const std::string &patch_name) const
    {
        for (const EPatch &patch : patches)
        {
            if (Utils::ToLowerCase(patch_name) == Utils::ToLowerCase(patch.GetName()))
                return patch;
        }

        throw std::out_of_range("Patch " + patch_name + " doesn't exist.");
    }

    inline std::vector<EPatch>::iterator begin() { return patches.begin(); }
    inline std::vector<EPatch>::iterator end() { return patches.end(); }

    inline std::vector<EPatch>::const_iterator begin() const { return patches.begin(); }
    inline std::vector<EPatch>::const_iterator end() const { return patches.end(); }
};

#endif // EPATCHFILE_H
