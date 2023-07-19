using System.IO.Compression;

namespace XVModCreator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void saveModToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = ".x1m mods| *.x1m";
            sfd.Title = "Save mod";

            if (textBox1.Text.Length > 0 && Directory.Exists(textBox1.Text))
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ZipFile.CreateFromDirectory(textBox1.Text, sfd.FileName);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog  fbd = new FolderBrowserDialog();
            fbd.UseDescriptionForTitle = true;
            fbd.Description = "Select mod folder";
            if (fbd.ShowDialog() == DialogResult.OK && Directory.Exists(fbd.SelectedPath))
            {
                textBox1.Text = fbd.SelectedPath;
            }
        }
    }
}