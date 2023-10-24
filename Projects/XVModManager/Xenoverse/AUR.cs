using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Xenoverse
{
    struct Aura
    {
        public int[] Color;
    }

    struct Charlisting
    {
        public int Name;
        public int Costume;
        public int ID;
        public bool inf;
    }

    struct CharName
    {
        public int ID;
        public string Name;

        public CharName(int i, string n)
        {
            ID = i;
            Name = n;
        }
    }

    public class AUR
    {
        Aura[] Auras;
        Charlisting[] Chars;
        byte[] backup = new byte[104];
        bool AuraLock = false;
        bool CharLock = false;

        byte[] AURfile = File.ReadAllBytes(Xenoverse.AURFile);

        public void ReadAUR()
        {
            //Aura Editor
            Auras = new Aura[BitConverter.ToInt32(AURfile, 8)];
            int AuraAddress = BitConverter.ToInt32(AURfile, 12);
            for (int A = 0; A < Auras.Length; A++)
            {
                int id = BitConverter.ToInt32(AURfile, AuraAddress + (16 * A));
                Auras[id].Color = new int[BitConverter.ToInt32(AURfile, AuraAddress + (16 * A) + 8)];
                int CAddress = BitConverter.ToInt32(AURfile, AuraAddress + (16 * A) + 12);
                for (int C = 0; C < Auras[id].Color.Length; C++)
                    Auras[id].Color[BitConverter.ToInt32(AURfile, CAddress + (C * 8))] = BitConverter.ToInt32(AURfile, CAddress + (C * 8) + 4);
            }

            //backup this data up
            int WAddress = BitConverter.ToInt32(AURfile, 20);
            Array.Copy(AURfile, WAddress, backup, 0, 104);

            //Character Aura Changer
            Chars = new Charlisting[BitConverter.ToInt32(AURfile, 24)];
            int ChAddress = BitConverter.ToInt32(AURfile, 28);
            for (int C = 0; C < Chars.Length; C++)
            {
                Chars[C].Name = BitConverter.ToInt32(AURfile, ChAddress + (C * 16));
                Chars[C].Costume = BitConverter.ToInt32(AURfile, ChAddress + (C * 16) + 4);
                Chars[C].ID = BitConverter.ToInt32(AURfile, ChAddress + (C * 16) + 8);
                Chars[C].inf = BitConverter.ToBoolean(AURfile, ChAddress + (C * 16) + 12);
            }
        }
        public void AURAddCharacter(int CharID, int auraID, bool Glare)
        {
            // First, let's create a new character entry
            Charlisting newCharacter = new Charlisting
            {
                Name = CharID, // Set the character's name (you may need to replace this with the actual name)
                Costume = 0,   // Set the initial costume to 0 (you can change this as needed)
                ID = auraID,    // Set the aura ID for the character
                inf = Glare     // Set the 'inf' value to false (you can change this as needed)
            };

            // Now, let's add this new character to the Chars array
            int charCount = Chars.Length;
            Array.Resize(ref Chars, charCount + 1);
            Chars[charCount] = newCharacter;

            // Update the character count in the AUR file
            using (BinaryWriter writer = new BinaryWriter(File.Open(Xenoverse.AURFile, FileMode.Open)))
            {
                int charCountOffset = 24; // Offset for character count
                writer.BaseStream.Seek(charCountOffset, SeekOrigin.Begin);
                writer.Write(Chars.Length); // Update the character count
            }
        }

        public void SaveAUR()
        {
            int auraCount = Auras.Length;
            int charCount = Chars.Length;

            using (BinaryWriter writer = new BinaryWriter(File.Open(Xenoverse.AURFile, FileMode.Open)))
            {
                // Write the aura count and aura address
                writer.BaseStream.Seek(8, SeekOrigin.Begin);
                writer.Write(auraCount);
                writer.Write(12); // Placeholder for aura address

                // Write the aura data
                int auraAddress = 40; // Start address for aura data
                for (int A = 0; A < auraCount; A++)
                {
                    writer.BaseStream.Seek(auraAddress + (16 * A), SeekOrigin.Begin);
                    writer.Write(A);
                    writer.Write(Auras[A].Color.Length);
                    writer.Write(auraAddress + auraCount * 16 + A * 8); // Aura color address

                    int colorAddress = auraAddress + auraCount * 16 + A * 8;
                    for (int C = 0; C < Auras[A].Color.Length; C++)
                    {
                        writer.BaseStream.Seek(colorAddress + C * 8, SeekOrigin.Begin);
                        writer.Write(C);
                        writer.Write(Auras[A].Color[C]);
                    }
                }

                // Write the backup data
                writer.BaseStream.Seek(20, SeekOrigin.Begin);
                writer.Write(auraAddress - 104);

                // Write the character count and character address
                writer.BaseStream.Seek(24, SeekOrigin.Begin);
                writer.Write(charCount);
                writer.Write(auraAddress + auraCount * 16); // Character address

                // Write the character data
                int charAddress = auraAddress + auraCount * 16 + charCount * 16;
                for (int C = 0; C < charCount; C++)
                {
                    writer.BaseStream.Seek(charAddress + (C * 16), SeekOrigin.Begin);
                    writer.Write(Chars[C].Name);
                    writer.Write(Chars[C].Costume);
                    writer.Write(Chars[C].ID);
                    writer.Write(Chars[C].inf);
                }
            }
        }
    }
}
