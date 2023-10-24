using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace Xenoverse
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

    public class CharSkill
    {
        string FileName;
        public skill[] Supers;
        public skill[] Ultimates;
        public skill[] Evasives;
        int CharCount = 0;
        int CharAddress = 0;
        public Char_Data[] Chars;
        msg mText = new msg();
        public int SupAddress;
        public int UltAddress;
        public int EvaAddress;
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

                SupAddress = CUS.ReadInt32();
                UltAddress = CUS.ReadInt32();
                EvaAddress = CUS.ReadInt32();

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
        public void AddCharacter(Char_Data characterData)
        {
            Array.Resize(ref Chars, CharCount + 1); // Espandi l'array Char_Data
            Chars[CharCount] = characterData;
            CharCount++;
            Save(Xenoverse.xenoverse_path + @"/msg");
        }
        public void Save(string msgFolder)
        {
            using (BinaryWriter CUS = new BinaryWriter(File.Open(FileName, FileMode.Open)))
            {
                CUS.BaseStream.Seek(8, SeekOrigin.Begin);
                CUS.Write(CharCount);
                CUS.Write(CharAddress);
                CUS.Write(Supers.Length);
                CUS.Write(Ultimates.Length);
                CUS.Write(Evasives.Length);
                CUS.BaseStream.Seek(8, SeekOrigin.Current);

                CUS.Write(SupAddress);
                CUS.Write(UltAddress);
                CUS.Write(EvaAddress);

                for (int i = 0; i < CharCount; i++)
                {
                    CUS.BaseStream.Seek(CharAddress + (i * 32), SeekOrigin.Begin);
                    CUS.Write(Chars[i].charID);
                    CUS.Write(Chars[i].CostumeID);
                    CUS.Write(Chars[i].SuperIDs[0]);
                    CUS.Write(Chars[i].SuperIDs[1]);
                    CUS.Write(Chars[i].SuperIDs[2]);
                    CUS.Write(Chars[i].SuperIDs[3]);
                    CUS.Write(Chars[i].UltimateIDs[0]);
                    CUS.Write(Chars[i].UltimateIDs[1]);
                    CUS.Write(Chars[i].EvasiveID);
                }

                for (int i = 0; i < Supers.Length; i++)
                {
                    CUS.BaseStream.Seek(SupAddress + (i * 48) + 8, SeekOrigin.Begin);
                    CUS.Write(Supers[i].ID);
                }

                for (int i = 0; i < Ultimates.Length; i++)
                {
                    CUS.BaseStream.Seek(UltAddress + (i * 48) + 8, SeekOrigin.Begin);
                    CUS.Write(Ultimates[i].ID);
                }

                for (int i = 0; i < Evasives.Length; i++)
                {
                    CUS.BaseStream.Seek(EvaAddress + (i * 48) + 8, SeekOrigin.Begin);
                    CUS.Write(Evasives[i].ID);
                }
            }
        }

        public short FindSuperByName(string name)
        {
            for (int i = 0; i < Supers.Length; i++)
            {
                if (Supers[i].Name == name)
                {
                    return Supers[i].ID;
                }
            }
            return -1; // Restituisci un valore speciale (ad esempio -1) se la skill non viene trovata
        }

        public short FindUltimateByName(string name)
        {
            for (int i = 0; i < Ultimates.Length; i++)
            {
                if (Ultimates[i].Name == name)
                {
                    return Ultimates[i].ID;
                }
            }
            return -1; // Restituisci un valore speciale (ad esempio -1) se la skill non viene trovata
        }

        public short FindEvasiveByName(string name)
        {
            for (int i = 0; i < Evasives.Length; i++)
            {
                if (Evasives[i].Name == name)
                {
                    return Evasives[i].ID;
                }
            }
            return -1; // Restituisci un valore speciale (ad esempio -1) se la skill non viene trovata
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

    }
}