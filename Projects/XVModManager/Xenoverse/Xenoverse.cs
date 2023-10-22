using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenoverse
{
    public class Xenoverse
    {
        // XVMODMANAGER PATHS
        public static string temp_path = "C:/XVModManagerTemp";

        // GAME PATHS
        public static string executable_name = "DBXV.exe";
        public static string xenoverse_path = @"./";
        public static string data_path = xenoverse_path + @"/data";
        public static string xvpatcher_dll = xenoverse_path + @"/xinput1_3.dll";

        // CPK FILES
        public static string data_cpk = xenoverse_path + "/data.cpk";
        public static string data2_cpk = xenoverse_path + "/data2.cpk";
        public static string datap1_cpk = xenoverse_path + "/datap1.cpk";
        public static string datap2_cpk = xenoverse_path + "/datap2.cpk";
        public static string datap3_cpk = xenoverse_path + "/datap3.cpk";

        // IGGY FILES
        public static string CHARASELE_IGGY = data_path + "/ui/iggy/CHARASELE.iggy";
        public static string STAGESELE_IGGY = data_path + "/ui/iggy/STAGESELE.iggy";

        // SYSTEM FILES
        public static string AURFile = data_path + "/system/aura_setting.aur";
        public static string CMSFile = data_path + "/system/char_model_spec.cms";
        public static string CSOFile = data_path + "/system/chara_sound.cso";
        public static string CUSFile = data_path + "/system/custom_skill.cus";
        public static string PSCFile = data_path + "/system/parameter_spec_char.psc";

        // MSG FILES
        public static string proper_noun_character_name = data_path + @"/msg/proper_noun_character_name_en.msg";
        public static string proper_noun_costume_name = data_path + @"/msg/proper_noun_costume_name_en.msg";

        // GAME CHARACTER IDS - UPDATED TO 1.08.00
        public static StringCollection characterIds = new StringCollection
        {
            "GOK", "BDK", "GK4", "GOD", "GKG", "GTG", "GHS", "GHM", "GHL",
            "PIC", "KLL", "YMC", "TSH", "RAD", "SBM", "NAP", "VGT", "VG4",
            "GRD", "BAT", "RCM", "JES", "GNY", "FRZ", "FR4", "FR5", "TRX",
            "TRS", "G17", "S17", "G18", "CL1", "CL3", "CL4", "CLJ", "VDL",
            "BUL", "BUM", "BUS", "GTX", "VTO", "BRL", "BLS", "PAN", "GIL",
            "OSV", "OSB", "OBB", "OSN", "SD3", "SD4", "SD1", "GGT", "STN",
            "DMG", "DM2", "SIN", "TOK", "TKT", "BUZ", "APL", "RSB", "GHP",
            "MIR", "TOW", "WIS", "HUM", "HUF", "MAM", "MAF", "FRI", "NMC"
        };

        // GAME LANGUAGES - UPDATED TO 1.08.00
        public static StringCollection languages = new StringCollection
        {
            "ca",
            "de",
            "en",
            "es",
            "fr",
            "it",
            "pl",
            "pt",
            "ru"
        };
    }
}
