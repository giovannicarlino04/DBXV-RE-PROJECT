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
                File.WriteAllText(Application.StartupPath + @"\lang.txt", "ca");
                this.Close();
            }
            else if (checkBox2.Checked)
            {
                File.WriteAllText(Application.StartupPath + @"\lang.txt", "de");
                this.Close();
            }
            else if (checkBox3.Checked)
            {
                File.WriteAllText(Application.StartupPath + @"\lang.txt", "en");
                this.Close();
            }
            else if (checkBox4.Checked)
            {
                File.WriteAllText(Application.StartupPath + @"\lang.txt", "es");
                this.Close();
            }
            else if (checkBox5.Checked)
            {
                File.WriteAllText(Application.StartupPath + @"\lang.txt", "fr");
                this.Close();
            }
            else if (checkBox6.Checked)
            {
                File.WriteAllText(Application.StartupPath + @"\lang.txt", "it");
                this.Close();
            }
            else if (checkBox7.Checked)
            {
                File.WriteAllText(Application.StartupPath + @"\lang.txt", "pl");
                this.Close();
            }
            else if (checkBox8.Checked)
            {
                File.WriteAllText(Application.StartupPath + @"\lang.txt", "pt");
                this.Close();
            }
            else if (checkBox9.Checked)
            {
                File.WriteAllText(Application.StartupPath + @"\lang.txt", "ru");
                this.Close();
            }
        }
    }
}
