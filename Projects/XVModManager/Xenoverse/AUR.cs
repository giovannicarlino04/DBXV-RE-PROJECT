using System;
using System.IO;

namespace Xenoverse
{
    public class AUR
    {
        public struct Aura
        {
            public int AUR_ID;
            public int AUR_Glare;
            public int[] Color;
        }

        public Aura[] Auras; // Dichiarato come campo per renderlo accessibile da altre parti della classe

        public void Load(string AURFileName)
        {
            byte[] AURfile = File.ReadAllBytes(AURFileName);

            int auraCount = BitConverter.ToInt32(AURfile, 8);
            Auras = new Aura[auraCount];

            int auraAddress = BitConverter.ToInt32(AURfile, 12);

            for (int i = 0; i < auraCount; i++)
            {
                int id = BitConverter.ToInt32(AURfile, auraAddress + (16 * i));
                int colorCount = BitConverter.ToInt32(AURfile, auraAddress + (16 * i) + 8);
                Auras[id].Color = new int[colorCount];

                int colorAddress = BitConverter.ToInt32(AURfile, auraAddress + (16 * i) + 12);

                for (int j = 0; j < colorCount; j++)
                {
                    int colorID = BitConverter.ToInt32(AURfile, colorAddress + (j * 8));
                    int colorValue = BitConverter.ToInt32(AURfile, colorAddress + (j * 8) + 4);

                    Auras[id].Color[colorID] = colorValue;
                }
            }
        }

        public void Save()
        {
            string AURFileName = Xenoverse.AURFile;
            byte[] AURfile = File.ReadAllBytes(AURFileName);
            int auraAddress = BitConverter.ToInt32(AURfile, 12);

            // Ora Auras contiene i dati dei personaggi, puoi scrivere AURfile con i nuovi dati
            using (BinaryWriter writer = new BinaryWriter(File.Open(AURFileName, FileMode.Open)))
            {
                writer.BaseStream.Seek(auraAddress, SeekOrigin.Begin);

                for (int A = 0; A < Auras.Length; A++)
                {
                    int id = A;
                    int colorCount = Auras[id].Color.Length;

                    writer.Write(id); // Scrivi l'ID del personaggio
                    writer.Write(colorCount); // Scrivi il numero di colori di aura
                    writer.Write(0); // Riserva spazio per l'indirizzo dei colori

                    // Scrivi i colori di aura
                    int colorAddress = (int)writer.BaseStream.Position;
                    for (int C = 0; C < colorCount; C++)
                    {
                        writer.Write(colorAddress + (C * 8)); // Scrivi l'indirizzo del colore
                        writer.Write(Auras[id].Color[C]); // Scrivi il colore
                    }
                }
            }
        }

        public void AddCharacter(int characterID, int costumeID, int AUR_ID, int AUR_Glare, int[] auraColors)
        {
            if (characterID < 0 || characterID >= Auras.Length)
            {
                // Espandi l'array Auras se characterID è fuori dai limiti
                Array.Resize(ref Auras, characterID + 1);
            }

            // Aggiungi o sovrascrivi il personaggio
            Auras[characterID] = new Aura
            {
                AUR_ID = AUR_ID,
                AUR_Glare = AUR_Glare,
                Color = auraColors
            };

            // Salva i dati AUR aggiornati nel file
            Save();
        }
    }
}
