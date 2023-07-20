using System.IO.Compression;
using System.Text;
using System.Xml.Schema;
using XVModManager.Properties;
using static System.Reflection.Metadata.BlobBuilder;

namespace XVModManager
{

    public partial class XVModManager : Form
    {

        string datapath;
        string tempPath = "C:/XVModManagerTemp";
        string XVRebornFolder = Application.StartupPath + @"/XVModManager";
        int IggySlotStartOffset = 0x19C9A;
        int IggySlotEndOffset = 0x19D76;
        List<string> Iggyslots;

        public void DeleteEmptyDirectories(string rootFolderPath)
        {
            // Get all directories within the root folder
            string[] directories = Directory.GetDirectories(rootFolderPath, "*", SearchOption.AllDirectories);

            // Traverse the directories in reverse order to ensure deleting child directories before parent directories
            for (int i = directories.Length - 1; i >= 0; i--)
            {
                string directory = directories[i];

                // Check if the directory is empty
                if (IsDirectoryEmpty(directory))
                {
                    Directory.Delete(directory, false);
                }
            }
        }

        private bool IsDirectoryEmpty(string directoryPath)
        {
            return !Directory.EnumerateFileSystemEntries(directoryPath).Any();
        }

        public XVModManager()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clean(); //THIS IS IMPORTANT, OR FILES IN TEMP FOLDER WILL MESS SHIT UP

            FolderBrowserDialog XVPathDialog = new FolderBrowserDialog();
            XVPathDialog.UseDescriptionForTitle = true;
            XVPathDialog.Description = "Select \"DB Xenoverse\" Path";

            //Set game path
            if (Settings.Default.XVPath.Length == 0)
            {
                if (XVPathDialog.ShowDialog() == DialogResult.OK && Directory.Exists(XVPathDialog.SelectedPath))
                {
                    Settings.Default.XVPath = XVPathDialog.SelectedPath;
                    datapath = XVPathDialog.SelectedPath + @"/data";
                    Settings.Default.Save();
                }
                else
                {
                    MessageBox.Show("Invalid path", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            else
            {
                datapath = Settings.Default.XVPath + @"/data";
            }

            if (!Directory.Exists(datapath))
            {
                Directory.CreateDirectory(datapath);
                Directory.CreateDirectory(datapath + @"/installed");
            }

            if (Settings.Default.Modlist.Contains("Test1234567890"))
            {
                Settings.Default.Modlist.Clear();
            }

            lbMods.Items.Clear();
            foreach (string item in Settings.Default.Modlist)
            {
                lbMods.Items.Add(item);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void installModToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog modInstallDialog = new OpenFileDialog();
            modInstallDialog.Title = "Install .x1m mod";
            modInstallDialog.Filter = ".x1m mod files | *.x1m";
            modInstallDialog.Multiselect = false;


            if (modInstallDialog.ShowDialog() == DialogResult.OK && File.Exists(modInstallDialog.FileName))
            {
                string modFile = modInstallDialog.FileName;


                ZipFile.ExtractToDirectory(modFile, tempPath, true);

                string xmlFile = tempPath + @"/modinfo.xml";

                if (!File.Exists(xmlFile))
                    MessageBox.Show("Invalid mod file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                var modName = File.ReadAllLines(xmlFile).First().ToString();
                var modAuthor = File.ReadAllLines(xmlFile).First().ToString();

                File.Delete(xmlFile);

                IEnumerable<string> fileSystemEntries = Directory.EnumerateFileSystemEntries(tempPath, "*", SearchOption.AllDirectories);
                string ModFilesFile = datapath + @"/installed/" + modName + ".xml";

                File.WriteAllLines(ModFilesFile, fileSystemEntries);

                string folderPath = "C:/XVRebornTemp";
                string replacementPath = datapath;

                // Read the file and replace the path
                string fileContent = File.ReadAllText(ModFilesFile);
                fileContent = fileContent.Replace(folderPath, replacementPath);

                // Write the modified content back to the file
                File.WriteAllText(ModFilesFile, fileContent);

                //Copy the contents of XVRebornTemp to the data folder
                string[] directories = Directory.GetDirectories(tempPath);
                foreach (string directory in directories)
                {
                    string destinationPath = Path.Combine(datapath, Path.GetFileName(directory));
                    Directory.Move(directory, destinationPath);
                }

                // Move files
                string[] files = Directory.GetFiles(tempPath);
                foreach (string file in files)
                {
                    string destinationPath = Path.Combine(datapath, Path.GetFileName(file));
                    File.Move(file, destinationPath);
                }

                Settings.Default.Modlist.Add(modName);
                Settings.Default.Save();
                lbMods.Items.Clear();
                foreach (string item in Settings.Default.Modlist)
                {
                    lbMods.Items.Add(item);
                }

                MessageBox.Show("Mod Installed Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void Clean()
        {
            if (Directory.Exists(tempPath))
            {
                Directory.Delete(tempPath, true);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Clean();
        }

        private void clearInstallationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to clear installation?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                Directory.Delete(datapath, true);

                Settings.Default.Reset();
                Settings.Default.Save();
                MessageBox.Show("Installation cleared successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void uninstallModToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbMods.SelectedIndex > -1 && MessageBox.Show("Do you really want to uninstall this mod?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string modname = lbMods.SelectedItem.ToString();
                var files = File.ReadAllLines(datapath + @"/installed/" + modname + @".xml");
                foreach (var f in files)
                {
                    if (File.Exists(f))
                        File.Delete(f);
                }

                File.Delete(datapath + @"/installed/" + modname + @".xml");

                Settings.Default.Modlist.Remove(modname);
                Settings.Default.Save();
                lbMods.Items.Clear();
                foreach (string item in Settings.Default.Modlist)
                {
                    lbMods.Items.Add(item);
                }
                DeleteEmptyDirectories(datapath);
                MessageBox.Show("Mod uninstalled successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}