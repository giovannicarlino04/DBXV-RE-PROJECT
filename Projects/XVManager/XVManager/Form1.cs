using System.IO.Compression;
using XVManager.Properties;

namespace XVManager
{
    public partial class Form1 : Form
    {
        const string XVDefaultPath = @"C:/Program Files (x86)/Steam/steamapps/common/DB Xenoverse";
        string dataPath;

        public Form1()
        {
            InitializeComponent();
        }

        private void installMod(string mod)
        {
            ZipFile.ExtractToDirectory(mod, dataPath, true);
            Settings.Default.modList.Add(Path.GetFileName(mod));
            listBox1.Items.Clear();
            foreach(string item in Settings.Default.modList) { listBox1.Items.Add(item);}
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void installModToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Install Mod";
            ofd.Filter = ".x1m mods | (*.x1m)";
            ofd.RestoreDirectory = true;
            ofd.Multiselect = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string mod in ofd.FileNames)
                {
                    installMod(mod);
                }
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(XVDefaultPath) && Settings.Default.XVPath == null)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Select DB Xenoverse Folder";
                fbd.UseDescriptionForTitle = true;

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    Settings.Default.XVPath = fbd.SelectedPath;
                    Settings.Default.Save();
                    dataPath = Settings.Default.XVPath + @"/data";
                }
            }
            else if (Directory.Exists(XVDefaultPath))
            {
                Settings.Default.XVPath = XVDefaultPath;
                Settings.Default.Save();
                dataPath = Settings.Default.XVPath + @"/data";
            }
            else
            {
                dataPath = Settings.Default.XVPath + @"/data";
            }

            foreach (string mod in Settings.Default.modList)
            {
                listBox1.Items.Add(mod);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (string mod in listBox1.Items)
            {
                Settings.Default.modList.Add(mod);
                Settings.Default.Save();
            }
        }

        private void clearInstallationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Directory.Delete(dataPath, true);
            Settings.Default.Reset();
        }
    }
}
