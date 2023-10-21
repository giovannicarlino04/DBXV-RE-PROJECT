using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;

namespace XVSkillCreator
{
    public partial class Form1 : Form
    {

        FolderBrowserDialog fbd = new FolderBrowserDialog();
        SaveFileDialog sfd = new SaveFileDialog();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtSkillID.Text.Length == 11 | txtSkillID.Text.Length == 12)
            {
                fbd.Description = "Select \"" + txtSkillID.Text + "\" folder";


                if (fbd.ShowDialog() == DialogResult.OK && Directory.Exists(fbd.SelectedPath))
                {
                    DirectoryInfo dir = new DirectoryInfo(fbd.SelectedPath);

                    if (dir.Name == txtSkillID.Text)
                    {
                        txtSkillFolder.Text = fbd.SelectedPath;
                    }
                    else
                    {
                        MessageBox.Show("The folder you selected isn't " + "\"" + txtSkillID.Text + "\"", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Invalid ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sfd.Filter = ".x1m files | *.x1m";
            sfd.Title = "Save Mod";

            if (Directory.Exists(txtSkillFolder.Text) && txtSkillName.Text.Length > 0 && txtSkillAuthor.Text.Length > 0 && CBSkillType.SelectedIndex >= 0 && txtSkillID.Text.Length == 11 | txtSkillID.Text.Length == 12 )
            {
                string temp = @"C:\Modtemp";
                Directory.CreateDirectory(temp);

                if (Directory.Exists(temp + @"\skill\SPA\" + txtSkillID.Text))
                {
                    Directory.Delete(temp + @"\skill\SPA\" + txtSkillID.Text, true);
                }
                else if (Directory.Exists(temp + @"\skill\ULT\" + txtSkillID.Text))
                {
                    Directory.Delete(temp + @"\skill\ULT\" + txtSkillID.Text, true);
                }
                else if (Directory.Exists(temp + @"\skill\ESC\" + txtSkillID.Text))
                {
                    Directory.Delete(temp + @"\skill\ESC\" + txtSkillID.Text, true);
                }

                if (CBSkillType.SelectedIndex == 0)
                {
                    Directory.CreateDirectory(temp + @"\skill\SPA");
                    Directory.Move(txtSkillFolder.Text, temp + @"\skill\SPA\" + txtSkillID.Text);

                }
                else if (CBSkillType.SelectedIndex == 1)
                {
                    Directory.CreateDirectory(temp + @"\skill\ULT");
                    Directory.Move(txtSkillFolder.Text, temp + @"\skill\ULT\" + txtSkillID.Text);
                }
                else if (CBSkillType.SelectedIndex == 2)
                {
                    Directory.CreateDirectory(temp + @"\skill\ESC");
                    Directory.Move(txtSkillFolder.Text, temp + @"\skill\ESC\" + txtSkillID.Text);
                }

                string xmlpath = temp + "\\modinfo.xml";

                File.WriteAllText(xmlpath, txtSkillName.Text + "\n" + txtSkillAuthor.Text + "\n" + txtSkillID.Text + "\n" + CBSkillType.Text);

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ZipFile.CreateFromDirectory(temp, sfd.FileName);

                    MessageBox.Show("Mod Created Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (Directory.Exists(temp))
                    {
                        Directory.Delete(temp, true);
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("Invalid Format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

