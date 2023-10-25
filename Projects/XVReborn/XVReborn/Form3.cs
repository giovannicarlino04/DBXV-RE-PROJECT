using System;
using System.IO;
using System.Windows.Forms;

namespace XVReborn
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0 && Directory.Exists(textBox1.Text) && textBox2.Text.Length > 0 && Directory.Exists(textBox2.Text))
            {
                Properties.Settings.Default.datafolder = textBox1.Text;
                Properties.Settings.Default.flexsdkfolder = textBox2.Text;
                Properties.Settings.Default.Save();
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog data = new FolderBrowserDialog();
            data.Description = "Locate data folder";

            if (data.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = data.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog flexsdk = new FolderBrowserDialog();
            flexsdk.Description = "Locate data folder";

            if (flexsdk.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = flexsdk.SelectedPath;
            }
        }
    }
}
