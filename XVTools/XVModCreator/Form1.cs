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
            sfd.Filter = ".xvmod file | *.xvmod";
            sfd.Title = "Save mod";
            sfd.FileName = textBox3.Text;

            if (textBox1.Text.Length > 0 && textBox2.Text.Length > 0 && textBox3.Text.Length > 0 && Directory.Exists(textBox1.Text))
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.AppendAllText(textBox1.Text + @"/modinfo.xml", textBox3.Text + "\n" + textBox2.Text);
                    ZipFile.CreateFromDirectory(textBox1.Text, sfd.FileName);
                    MessageBox.Show("Mod created successfully!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.UseDescriptionForTitle = true;
            fbd.Description = "Select mod folder";
            if (fbd.ShowDialog() == DialogResult.OK && Directory.Exists(fbd.SelectedPath))
            {
                textBox1.Text = fbd.SelectedPath;
            }
        }
    }
}