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
        public void populateSkillData(string CUSFile)
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
                for (int i = 0; i < SuperCount; i++)
                {
                    CUS.BaseStream.Seek(SupAddress + (i * 48) + 8, SeekOrigin.Begin);
                    Supers[i].ID = CUS.ReadInt16();
                }

                Ultimates = new skill[UltimateCount];
                for (int i = 0; i < UltimateCount; i++)
                {
                    CUS.BaseStream.Seek(UltAddress + (i * 48) + 8, SeekOrigin.Begin);
                    Ultimates[i].ID = CUS.ReadInt16();
                }

                Evasives = new skill[EvasiveCount];
                for (int i = 0; i < EvasiveCount; i++)
                {
                    CUS.BaseStream.Seek(EvaAddress + (i * 48) + 8, SeekOrigin.Begin);
                    Evasives[i].ID = CUS.ReadInt16();
                }

            }

        }
        public void Save()
        {
            using (BinaryWriter CUS = new BinaryWriter(File.Open(Xenoverse.CUSFile, FileMode.Open)))
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

    }
}