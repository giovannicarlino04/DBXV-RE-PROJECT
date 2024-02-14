using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace XVReborn
{
    public class CharacterData
    {
        public int ID { get; set; }
        public string ShortName { get; set; }
        public byte[] Unknown { get; set; }
        public string[] Paths { get; set; }
    }
    public class CMS
    {
        public CharacterData[] Data; // Modifica il tipo di Data in CharacterData[]
        BinaryReader br;
        BinaryWriter bw;
        string FileName;
        public void Load(string path)
        {

            using (br = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                FileName = path;
                br.BaseStream.Seek(8, SeekOrigin.Begin);
                int Count = br.ReadInt32();
                Data = new CharacterData[Count];
                int Offset = br.ReadInt32();

                for (int i = 0; i < Count; i++)
                {
                    Data[i] = new CharacterData(); // Inizializza il nuovo oggetto CharacterData
                    br.BaseStream.Seek(Offset + (80 * i), SeekOrigin.Begin);
                    Data[i].ID = br.ReadInt32();
                    Data[i].ShortName = System.Text.Encoding.ASCII.GetString(br.ReadBytes(3));
                    br.BaseStream.Seek(9, SeekOrigin.Current);
                    Data[i].Unknown = br.ReadBytes(8);
                    br.BaseStream.Seek(8, SeekOrigin.Current);
                    Data[i].Paths = new string[7];
                    Data[i].Paths[0] = TextAtAddress(br.ReadInt32());
                    Data[i].Paths[1] = TextAtAddress(br.ReadInt32());
                    br.BaseStream.Seek(4, SeekOrigin.Current);
                    Data[i].Paths[2] = TextAtAddress(br.ReadInt32());
                    br.BaseStream.Seek(8, SeekOrigin.Current);
                    Data[i].Paths[3] = TextAtAddress(br.ReadInt32());
                    Data[i].Paths[4] = TextAtAddress(br.ReadInt32());
                    Data[i].Paths[5] = TextAtAddress(br.ReadInt32());
                    Data[i].Paths[6] = TextAtAddress(br.ReadInt32());

                    //94 Lunghezza 50 Byte per personaggio
                }

            }

        }

        public string TextAtAddress(int Address)
        {
            long position = br.BaseStream.Position;
            string rText = "";
            byte[] c;
            if (Address != 0)
            {
                br.BaseStream.Seek(Address, SeekOrigin.Begin);
                do
                {
                    c = br.ReadBytes(1);
                    if (c[0] != 0x00)
                        rText += System.Text.Encoding.ASCII.GetString(c);
                    else
                        break;
                }
                while (true);

                br.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            return rText;
        }
        public void Save()
        {
            List<string> CmnText = new List<string>();
            for (int i = 0; i < Data.Length; i++)
            {
                for (int j = 0; j < Data[i].Paths.Length; j++)
                {
                    if (!CmnText.Contains(Data[i].Paths[j]))
                        CmnText.Add(Data[i].Paths[j]);
                }
            }

            int[] wordAddress = new int[CmnText.Count];
            int wordstartposition = 16 + (Data.Length * 80);

            // Usa FileMode.Create per sovrascrivere un file esistente o crearne uno nuovo
            using (bw = new BinaryWriter(File.Open(FileName, FileMode.Create)))
            {
                bw.Write(new byte[] { 0x23, 0x43, 0x4D, 0x53, 0xFE, 0xFF, 0x00, 0x00 });
                bw.Write(Data.Length);
                bw.Write((int)16);
                bw.Seek(wordstartposition, SeekOrigin.Begin);
                for (int i = 0; i < CmnText.Count; i++)
                {
                    wordAddress[i] = (int)bw.BaseStream.Position;
                    bw.Write(System.Text.Encoding.ASCII.GetBytes(CmnText[i]));
                    bw.Write((byte)0);
                }

                for (int i = 0; i < Data.Length; i++)
                {
                    bw.BaseStream.Seek(16 + (80 * i), SeekOrigin.Begin);
                    bw.Write(Data[i].ID);
                    bw.Write(System.Text.Encoding.ASCII.GetBytes(Data[i].ShortName));
                    bw.BaseStream.Seek(9, SeekOrigin.Current);
                    bw.Write(Data[i].Unknown);
                    bw.Write(new byte[] { 0xFF, 0xFF });
                    bw.BaseStream.Seek(6, SeekOrigin.Current);
                    bw.Write(wordAddress[CmnText.IndexOf(Data[i].Paths[0])]);
                    bw.Write(wordAddress[CmnText.IndexOf(Data[i].Paths[1])]);
                    bw.BaseStream.Seek(4, SeekOrigin.Current);
                    bw.Write(wordAddress[CmnText.IndexOf(Data[i].Paths[2])]);
                    bw.BaseStream.Seek(8, SeekOrigin.Current);
                    bw.Write(wordAddress[CmnText.IndexOf(Data[i].Paths[3])]);
                    bw.Write(wordAddress[CmnText.IndexOf(Data[i].Paths[4])]);
                    bw.Write(wordAddress[CmnText.IndexOf(Data[i].Paths[5])]);
                    bw.Write(wordAddress[CmnText.IndexOf(Data[i].Paths[6])]);
                }
            }
        }


        public void AddCharacter(CharacterData character)
        {
            if (character == null)
            {
                Console.WriteLine("Character data is null.");
                return;
            }

            if (Data == null)
            {
                Console.WriteLine("CMS data is not loaded.");
                return;
            }

            // Aggiungi il personaggio alla fine dei dati CMS
            List<CharacterData> newData = Data.ToList();
            newData.Add(character);

            // Assegna i nuovi dati CMS
            Data = newData.ToArray();

            // Salva i dati CMS aggiornati nel file
            Save();
        }

    }
}

