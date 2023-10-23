using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Xenoverse
{
    public struct CharSet
    {
        public int id;
        public Parameters[] p;
    }

    public struct Parameters
    {
        public byte[] Data;
    }


    public class PSC
    {
        string FileName;
        public int statposition;
        public string[] ValNames =
        {
            "?????", "?????", "?????", "?????",
            "Health", "?????", "Ki", "Ki Recharging Damage Received",
            "?????", "?????", "?????", "Stamina",
            "Stamina Recover", "?????", "?????", "?????",
            "Basic Attack", "Normal Ki Blasts", "Strike Supers", "Ki Blast Supers",

            "Physical Damage Received", "Ki Damage Received", "?????", "?????",
            "Ground Speed", "Air Speed", "Boost Speed", "Dash Speed",
            "?????", "Reinforcement Skill Duration", "?????", "Revival HP Amount",
            "Ally Revival Speed", "?????", "?????", "?????",
            "?????", "?????", "?????", "?????",

            "?????", "?????", "Z-Soul", "?????",
            "?????", "?????"
        };

        public int[] type =
        {
            0,0,0,0,
            1,1,1,1,
            0,0,1,1,
            1,1,1,1,
            1,1,1,1,

            1,1,1,1,
            1,1,1,1,
            1,1,1,1,
            1,1,1,1,
            1,1,1,1,

            1,1,0,0,
            0,1
        };
        public CharSet[] CharParam;
        public void load(string path)
        {

            using (BinaryReader br = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                FileName = path;
                br.BaseStream.Seek(8, SeekOrigin.Begin);
                int Count = br.ReadInt32();
                CharParam = new CharSet[Count];
                //br.BaseStream.Seek(16, SeekOrigin.Begin);
                for (int i = 0; i < Count; i++)
                {
                    br.BaseStream.Seek(16 + (i * 12), SeekOrigin.Begin);
                    CharParam[i].id = br.ReadInt32();
                    CharParam[i].p = new Parameters[br.ReadInt32()];
                }

                br.BaseStream.Seek(4, SeekOrigin.Current);
                statposition = (int)br.BaseStream.Position;
                //MessageBox.Show(br.BaseStream.Position.ToString());
                for (int i = 0; i < Count; i++)
                {
                    for (int j = 0; j < CharParam[i].p.Length; j++)
                        CharParam[i].p[j].Data = br.ReadBytes(184);
                }

            }

        }

        public void Save()
        {
            using (BinaryWriter p = new BinaryWriter(File.Open(FileName, FileMode.Open)))
            {
                //952
                p.BaseStream.Seek(statposition, SeekOrigin.Begin);
                for (int i = 0; i < CharParam.Length; i++)
                {
                    for (int j = 0; j < CharParam[i].p.Length; j++)
                        p.Write(CharParam[i].p[j].Data);

                }
            }

        }

        public float readAsFloat(int charIndex, int pIndex, int pos)
        {
            return BitConverter.ToSingle(CharParam[charIndex].p[pIndex].Data, pos * 4);
        }

        public int readAsInt(int charIndex, int pIndex, int pos)
        {
            return BitConverter.ToInt32(CharParam[charIndex].p[pIndex].Data, pos * 4);
        }

        public string getPosText(int pos)
        {
            return ValNames[pos];
        }

        public int FindType(int pos)
        {
            return type[pos];
        }
        public string getVal(int charIndex, int pIndex, int pos)
        {
            string val = "0";
            switch (type[pos])
            {
                case 0:
                    val = BitConverter.ToInt32(CharParam[charIndex].p[pIndex].Data, pos * 4).ToString();
                    break;
                case 1:
                    val = BitConverter.ToSingle(CharParam[charIndex].p[pIndex].Data, pos * 4).ToString();
                    break;
            }
            return val;
        }

        public void SaveVal(int charIndex, int pIndex, int pos, string val)
        {
            switch (type[pos])
            {
                case 0:
                    int n;
                    if (int.TryParse(val, out n))
                    {
                        byte[] c = BitConverter.GetBytes(n);
                        for (int i = 0; i < 4; i++)
                            CharParam[charIndex].p[pIndex].Data[(pos * 4) + i] = c[i];
                    }
                    break;
                case 1:
                    float nf;
                    if (float.TryParse(val, out nf))
                    {
                        byte[] c = BitConverter.GetBytes(nf);
                        for (int i = 0; i < 4; i++)
                            CharParam[charIndex].p[pIndex].Data[(pos * 4) + i] = c[i];
                    }
                    break;
            }
        }

        public int FindCharacterIndex(int id)
        {
            for (int i = 0; i < CharParam.Length; i++)
            {
                if (id == CharParam[i].id)
                    return i;
            }

            return -1;
        }
    }
}
