using System;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace XVReplacerCreator
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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sfd.Filter = ".x1m files | *.x1m";
            sfd.Title = "Save Mod";

            if (Directory.Exists(txtFolder.Text) && txtName.Text.Length > 0 && txtAuthor.Text.Length > 0)
            {
                string xmlpath = txtFolder.Text + "\\modinfo.xml";

                File.WriteAllText(xmlpath, txtName.Text + "\n" + txtAuthor.Text);

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ZipFile.CreateFromDirectory(txtFolder.Text, sfd.FileName);
                    MessageBox.Show("Mod Created Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void button1_Click(object sender, EventArgs e)
        {
            fbd.Description = "Select Mod Folder";

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtFolder.Text = fbd.SelectedPath;
            }
        }
    }
}
