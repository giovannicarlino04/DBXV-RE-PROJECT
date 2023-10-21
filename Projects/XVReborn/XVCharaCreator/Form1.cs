using System;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace XVCharaCreator
{
    public partial class Form1 : Form
    {
        FolderBrowserDialog fbd = new FolderBrowserDialog();
        OpenFileDialog ofd = new OpenFileDialog();
        SaveFileDialog sfd = new SaveFileDialog();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtID.Text.Length == 3)
            {
                fbd.Description = "Select \"" + txtID.Text + "\" Folder";

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    DirectoryInfo lastFolderInfo = new DirectoryInfo(fbd.SelectedPath);
                    string lastFolderName = lastFolderInfo.Name;

                    if(lastFolderName == txtID.Text)
                    {
                        txtFolder.Text = fbd.SelectedPath;
                    }
                    else
                    {
                        MessageBox.Show("Invalid Folder, please select \"" + txtID.Text + "\" Folder");
                    }

                }
            }
            else
            {
                MessageBox.Show("Invalid ID Format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txtID.Text.Length == 3)
            {
                ofd.Title = "Select Mod Portrait";
                ofd.Filter = txtID.Text + "_000.dds | " + txtID.Text + "_000.dds";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtPortrait.Text = ofd.FileName;
                }
            }
            else
            {
                MessageBox.Show("Invalid ID Format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sfd.Filter = ".x1m files | *.x1m";
            sfd.Title = "Save Mod";

            if (Directory.Exists(txtFolder.Text) && File.Exists(txtPortrait.Text) && txtName.Text.Length > 0 && txtID.Text.Length == 3 && txtAuthor.Text.Length > 0)
            {
                string temp = @"C:\Modtemp";
                Directory.CreateDirectory(temp);
                Directory.CreateDirectory(temp + @"\chara");
                Directory.CreateDirectory(temp + @"\ui\texture\CHARA01");

                if (Directory.Exists(temp + @"\chara\" + txtID.Text))
                {
                    Directory.Delete(temp + @"\chara\" + txtID.Text);
                }

                if (File.Exists(temp + @"\ui\texture\CHARA01\" + txtPortrait.Text))
                {
                    File.Delete(temp + @"\ui\texture\CHARA01\" + txtPortrait.Text);
                }

                Directory.Move(txtFolder.Text, temp + @"\chara\" + txtID.Text);
                File.Move(txtPortrait.Text, temp + @"\ui\texture\CHARA01\" + txtID.Text + "_000.DDS");

                string xmlpath = temp + "\\modinfo.xml";
                File.WriteAllText(xmlpath, txtName.Text + "\n" + txtAuthor.Text + "\n" + txtID.Text);
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

        private void button3_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            char randomChar1 = (char)rnd.Next('A', 'Z');
            char randomChar2 = (char)rnd.Next('A', 'Z');
            char randomChar3 = (char)rnd.Next('A', 'Z');

            txtID.Text = randomChar1.ToString() + randomChar2.ToString() + randomChar3.ToString();
        }
    }
}
