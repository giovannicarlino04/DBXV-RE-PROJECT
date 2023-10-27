using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace XVRebornReplacerEdition
{
    public struct skill
    {
        public string Name;
        public short ID;
    }

    public struct Char_Data
    {
        public int charID;
        public int CostumeID;
        public short[] SuperIDs;
        public short[] UltimateIDs;
        public short EvasiveID;
    }

    class CharSkill
    {
        string FileName;
        public skill[] Supers;
        public skill[] Ultimates;
        public skill[] Evasives;
        int CharCount = 0;
        int CharAddress = 0;
        public Char_Data[] Chars;
        msg mText = new msg();

        public void populateSkillData(string msgFolder, string CUSFile, string lang)
        {
            FileName = CUSFile;
            using (BinaryReader CUS = new BinaryReader(File.Open(CUSFile, FileMode.Open)))
            {
                CUS.BaseStream.Seek(8, SeekOrigin.Begin);
                CharCount = CUS.ReadInt32();
                CharAddress = CUS.ReadInt32();

                int SuperCount = CUS.ReadInt32();
                int UltimateCount = CUS.ReadInt32();
                int EvasiveCount = CUS.ReadInt32();

                CUS.BaseStream.Seek(8, SeekOrigin.Current);

                int SupAddress = CUS.ReadInt32();
                int UltAddress = CUS.ReadInt32();
                int EvaAddress = CUS.ReadInt32();


                Chars = new Char_Data[CharCount];
                for (int i = 0; i < CharCount; i++)
                {
                    CUS.BaseStream.Seek(CharAddress + (i * 32), SeekOrigin.Begin);
                    Chars[i].charID = CUS.ReadInt32();
                    Chars[i].CostumeID = CUS.ReadInt32();

                    Chars[i].SuperIDs = new short[4];
                    Chars[i].UltimateIDs = new short[2];

                    Chars[i].SuperIDs[0] = CUS.ReadInt16();
                    Chars[i].SuperIDs[1] = CUS.ReadInt16();
                    Chars[i].SuperIDs[2] = CUS.ReadInt16();
                    Chars[i].SuperIDs[3] = CUS.ReadInt16();
                    Chars[i].UltimateIDs[0] = CUS.ReadInt16();
                    Chars[i].UltimateIDs[1] = CUS.ReadInt16();
                    Chars[i].EvasiveID = CUS.ReadInt16();
                }

                Supers = new skill[SuperCount];
                mText = msgStream.Load(msgFolder + "/proper_noun_skill_spa_name_" + lang + ".msg");
                for (int i = 0; i < SuperCount; i++)
                {
                    CUS.BaseStream.Seek(SupAddress + (i * 48) + 8, SeekOrigin.Begin);
                    Supers[i].ID = CUS.ReadInt16();
                    Supers[i].Name = findName("spe_skill_" + CUS.ReadInt16().ToString("000"));
                }

                Ultimates = new skill[UltimateCount];
                mText = msgStream.Load(msgFolder + "/proper_noun_skill_ult_name_" + lang + ".msg");
                for (int i = 0; i < UltimateCount; i++)
                {
                    CUS.BaseStream.Seek(UltAddress + (i * 48) + 8, SeekOrigin.Begin);
                    Ultimates[i].ID = CUS.ReadInt16();
                    Ultimates[i].Name = findName("ult_" + CUS.ReadInt16().ToString("000"));
                }

                Evasives = new skill[EvasiveCount];
                mText = msgStream.Load(msgFolder + "/proper_noun_skill_esc_name_" + lang + ".msg");
                for (int i = 0; i < EvasiveCount; i++)
                {
                    CUS.BaseStream.Seek(EvaAddress + (i * 48) + 8, SeekOrigin.Begin);
                    Evasives[i].ID = CUS.ReadInt16();
                    Evasives[i].Name = findName("avoid_skill_" + CUS.ReadInt16().ToString("000"));
                }

            }
        }

        public void Save()
        {
            using (BinaryWriter CUS = new BinaryWriter(File.Open(FileName, FileMode.Open)))
            {
                CUS.BaseStream.Seek(CharAddress, SeekOrigin.Begin);
                for (int i = 0; i < CharCount; i++)
                {
                    CUS.BaseStream.Seek(CharAddress + (i * 32) + 8, SeekOrigin.Begin);
                    CUS.Write(Chars[i].SuperIDs[0]);
                    CUS.Write(Chars[i].SuperIDs[1]);
                    CUS.Write(Chars[i].SuperIDs[2]);
                    CUS.Write(Chars[i].SuperIDs[3]);
                    CUS.Write(Chars[i].UltimateIDs[0]);
                    CUS.Write(Chars[i].UltimateIDs[1]);
                    CUS.Write(Chars[i].EvasiveID);
                }
            }
        }

        private string findName(string text_ID)
        {
            for (int i = 0; i < mText.data.Length; i++)
            {
                if (mText.data[i].NameID == text_ID)
                    return mText.data[i].Lines[0];
            }


            return "Unknown Skill";
        }

        public int FindSuper(short id)
        {
            for (int i = 0; i < Supers.Length; i++)
            {
                if (Supers[i].ID == id)
                    return i;
            }
            return -1;
        }

        public int FindUltimate(short id)
        {
            for (int i = 0; i < Ultimates.Length; i++)
            {
                if (Ultimates[i].ID == id)
                    return i;
            }
            return -1;
        }

        public int FindEvasive(short id)
        {
            for (int i = 0; i < Evasives.Length; i++)
            {
                if (Evasives[i].ID == id)
                    return i;
            }
            return -1;
        }

        public int DataExist(int id, int c)
        {
            for (int i = 0; i < Chars.Length; i++)
            {
                if (Chars[i].charID == id && Chars[i].CostumeID == c)
                    return i;
            }

            return -1;
        }
    }

}
