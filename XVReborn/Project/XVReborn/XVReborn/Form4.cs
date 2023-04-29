using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Windows.Forms;

namespace XVReborn
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void addSlotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (toolStripTextBox1.Text.Length == 3)
            {
                string id = toolStripTextBox1.Text;

                string s = richTextBox1.Text.Replace("[[\"JCO\",0,0,0,[110,111]]]", "[[\"JCO\",0,0,0,[110,111]]],[[\"" + id + "\",0,0,0,[-1,-1]]]");
                richTextBox1.Text = s;
            }
            else
            {
                MessageBox.Show("Invalid ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void removeSlotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (toolStripTextBox1.Text.Length == 3)
            {
                string id = toolStripTextBox1.Text;

                string s = richTextBox1.Text.Replace(",[[\"" + id + "\",0,0,0,[-1,-1]]]", "");
                richTextBox1.Text = s;
            }
            else
            {
                MessageBox.Show("Invalid ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            TextReader reader = new StreamReader(Properties.Settings.Default.datafolder + @"\XV1P_SLOTS.x1s");

            richTextBox1.Text = reader.ReadToEnd();

            reader.Close();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            File.WriteAllText(Properties.Settings.Default.datafolder + @"\XV1P_SLOTS.x1s", richTextBox1.Text);
            MessageBox.Show("Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
