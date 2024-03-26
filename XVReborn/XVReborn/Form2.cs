using System;
using System.IO;
using System.Windows.Forms;

namespace XVReborn
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Properties.Settings.Default.language = "ca";
                this.Close();
            }
            else if (checkBox2.Checked)
            {
                Properties.Settings.Default.language = "de";
                this.Close();
            }
            else if (checkBox3.Checked)
            {
                Properties.Settings.Default.language = "en";
                this.Close();
            }
            else if (checkBox4.Checked)
            {
                Properties.Settings.Default.language = "es";
                this.Close();
            }
            else if (checkBox5.Checked)
            {
                Properties.Settings.Default.language = "fr";
                this.Close();
            }
            else if (checkBox6.Checked)
            {
                Properties.Settings.Default.language = "it";
                this.Close();
            }
            else if (checkBox7.Checked)
            {
                Properties.Settings.Default.language = "pl";
                this.Close();
            }
            else if (checkBox8.Checked)
            {
                Properties.Settings.Default.language = "pt";
                this.Close();
            }
            else if (checkBox9.Checked)
            {
                Properties.Settings.Default.language = "ru";
                this.Close();
            }

            Properties.Settings.Default.Save();
        }
    }
}
