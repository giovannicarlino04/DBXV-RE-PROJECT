using System;
using System.ComponentModel.Design.Serialization;

namespace XVCMSTool
{
    public partial class Form1 : Form
    {
        CMS cms = new CMS();
        string CMSFile;

        public Form1()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open \"char_model_spec.cms\" file";
            ofd.Filter = "Xenoverse CMS Files | *.cms";
            ofd.Multiselect = false;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                CMSFile = ofd.FileName;
                cms.Load(CMSFile);
                Reload();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cms.Data.Length > 0)
            {
                cms.Save();
                Reload();
            }
            else
            {
                MessageBox.Show("You must open the CMS file first...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            textBox1.Text = cms.Data[comboBox1.SelectedIndex].Paths[0];
            textBox2.Text = cms.Data[comboBox1.SelectedIndex].Paths[1];
            textBox3.Text = cms.Data[comboBox1.SelectedIndex].Paths[2];
            textBox4.Text = cms.Data[comboBox1.SelectedIndex].Paths[3];
            textBox5.Text = cms.Data[comboBox1.SelectedIndex].Paths[4];
            textBox6.Text = cms.Data[comboBox1.SelectedIndex].Paths[5];
            textBox7.Text = cms.Data[comboBox1.SelectedIndex].Paths[6];
            numericUpDown1.Value = cms.Data[comboBox1.SelectedIndex].ID;
            textBox8.Text = cms.Data[comboBox1.SelectedIndex].ShortName;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (cms.Data.Length > 0)
            {
                cms.Data[comboBox1.SelectedIndex].Paths[0] = textBox1.Text;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (cms.Data.Length > 0)
            {
                cms.Data[comboBox1.SelectedIndex].Paths[1] = textBox2.Text;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (cms.Data.Length > 0)
            {
                cms.Data[comboBox1.SelectedIndex].Paths[2] = textBox3.Text;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (cms.Data.Length > 0)
            {
                cms.Data[comboBox1.SelectedIndex].Paths[3] = textBox4.Text;
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (cms.Data.Length > 0)
            {
                cms.Data[comboBox1.SelectedIndex].Paths[4] = textBox5.Text;
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (cms.Data.Length > 0)
            {
                cms.Data[comboBox1.SelectedIndex].Paths[5] = textBox6.Text;
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (cms.Data.Length > 0)
            {
                cms.Data[comboBox1.SelectedIndex].Paths[6] = textBox7.Text;
            }
        }

        private void Reload()
        {
            comboBox1.Items.Clear();
            for (int i = 0; i < cms.Data.Length; i++)
            {
                comboBox1.Items.Add(cms.Data[i].ShortName);
            }
            comboBox1.SelectedIndex = 0;
        }

        private void addEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModelData newCharacter = new ModelData
            {
                ID = cms.Data[comboBox1.SelectedIndex].ID + 1, // ID del personaggio
                ShortName = "NEW", // Nome abbreviato del personaggio
                Unknown = new byte[8], // Array di byte sconosciuto
                Paths = new string[7] // Array di percorsi
            };
            cms.AddCharacter(newCharacter);
            Reload();
        }

        private void removeEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cms.RemoveCharacter(cms.Data[comboBox1.SelectedIndex]);
            Reload();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (cms.Data.Length > 0)
            {
                cms.Data[comboBox1.SelectedIndex].ID = (int)numericUpDown1.Value;
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (cms.Data.Length > 0)
            {
                cms.Data[comboBox1.SelectedIndex].ShortName = textBox8.Text;
            }
        }
    }
}
