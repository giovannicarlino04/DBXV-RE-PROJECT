using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace XVCSSEditor
{
    public partial class Form1 : Form
    {
        private byte[] fileBytes; // Store the file bytes globally
        private int charaListOffset = 0x19D52; // Update with the correct offset
        private int charaListLength = 129; // Update with the correct length
        private int entryLength = 4; // Update with the correct entry length

        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CHARASELE.iggy File|CHARASELE.iggy";
            ofd.Title = "Open XV1 CHARASELE.iggy file";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string filePath = ofd.FileName;

                try
                {
                    fileBytes = File.ReadAllBytes(filePath);

                    // Load characters into ComboBox
                    LoadCharacters(fileBytes, charaListOffset, charaListLength, entryLength);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadCharacters(byte[] fileBytes, int charaListOffset, int charaListLength, int entryLength)
        {
            comboBox1.Items.Clear();

            for (int i = 0; i < charaListLength; i += entryLength)
            {
                StringBuilder entry = new StringBuilder();

                for (int j = 0; j < entryLength; j++)
                {
                    int index = charaListOffset + i + j;
                    if (fileBytes[index] == 0x03 || fileBytes[index] == 0x06)
                    {
                        // Skip characters '03' and '06'
                        continue;
                    }

                    char character = (char)fileBytes[index];
                    entry.Append(character);
                }

                comboBox1.Items.Add(entry.ToString());
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileBytes == null)
            {
                MessageBox.Show("No file is currently open.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Check if all characters in ComboBox have a length of 3
            if (comboBox1.Items.Cast<string>().Any(character => character.Length != 3))
            {
                MessageBox.Show("Invalid characters in ComboBox. All characters must have a length of 3.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CHARASELE.iggy File|CHARASELE.iggy";
            sfd.Title = "Save XV1 CHARASELE.iggy file";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string destinationPath = sfd.FileName;

                try
                {
                    // Save modified bytes back to file
                    File.WriteAllBytes(destinationPath, fileBytes);
                    MessageBox.Show("File saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while saving the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = comboBox1.SelectedItem.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Update the data in both the ComboBox and the iggy
            UpdateData(textBox1.Text);
        }

        private void UpdateData(string newCharacter)
        {
            if (fileBytes == null)
            {
                MessageBox.Show("No file is currently open.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("No character selected in ComboBox.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            // Update the ComboBox
            comboBox1.Items[comboBox1.SelectedIndex] = newCharacter;

            // Update the iggy file
            int selectedIndex = comboBox1.SelectedIndex * entryLength;
            byte[] newCharacterBytes = Encoding.ASCII.GetBytes(newCharacter);

            for (int i = 0; i < entryLength; i++)
            {
                fileBytes[charaListOffset + selectedIndex + i] = (byte)(i < newCharacterBytes.Length ? newCharacterBytes[i] : 0x06);
            }
        }
    }
}
