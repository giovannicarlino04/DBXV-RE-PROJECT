using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Slot_Editor
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string> characterIds = new Dictionary<string, string>();
        private int numEntries;
        private int startOffset = 0x19C9A;
        private int endOffset = 0x19D74;
        private int entrySize = 4;

        public Form1()
        {
            InitializeComponent();
            LoadCharacterIds();
            PopulateComboBox();
        }

        private void LoadCharacterIds()
        {
            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open("CHARASELE.iggy", FileMode.Open)))
                {
                    // Set reader position to start of character IDs
                    reader.BaseStream.Seek(0x19C9A, SeekOrigin.Begin);

                    // Read character IDs until end of slot section
                    while (reader.BaseStream.Position < 0x19D74)
                    {
                        string charId = Encoding.ASCII.GetString(reader.ReadBytes(3)).TrimEnd('\0');

                        // Remove control characters
                        charId = new string(charId.Where(c => !char.IsControl(c)).ToArray());

                        // Add character ID to dictionary
                        characterIds[charId] = "";

                        // Move to next entry
                        reader.BaseStream.Position += entrySize - 3;
                    }

                    // Populate combobox with character names
                    PopulateComboBox();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading character IDs: {ex.Message}");
            }
        }


        private void PopulateComboBox()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(characterIds.Keys.ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Add new character ID
            string newCharId = textBox1.Text.Trim().ToUpper();

            if (newCharId.Length != 3)
            {
                MessageBox.Show("Character ID must be 3 letters long.");
                return;
            }

            if (characterIds.ContainsKey(newCharId))
            {
                MessageBox.Show("Character ID already exists.");
                return;
            }

            // find the first empty entry
            KeyValuePair<string, string> emptyEntry = characterIds.FirstOrDefault(x => string.IsNullOrEmpty(x.Value));

            if (emptyEntry.Value != null)
            {
                characterIds[newCharId] = "";
                PopulateComboBox();
            }
            else
            {
                MessageBox.Show("No empty slots available.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Save changes
            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open("CHARASELE.iggy", FileMode.Open)))
                {
                    writer.BaseStream.Seek(startOffset, SeekOrigin.Begin);
                    writer.Write(numEntries);

                    foreach (var entry in characterIds)
                    {
                        writer.Write(Encoding.ASCII.GetBytes(entry.Key));
                        writer.Write(new byte[entrySize - 3]);
                    }
                }

                MessageBox.Show("Changes saved.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving changes: {ex.Message}");
            }
        }
    }
}
