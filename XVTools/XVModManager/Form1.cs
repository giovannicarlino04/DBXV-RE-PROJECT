using System.IO.Compression;
using System.Reflection;
using XVModManager.Properties;

namespace XVModManager
{

    public partial class XVModManager : Form
    {
        string datapath;
        string tempPath = "C:/XVModManagerTemp";

        private List<string> GetAllFilesInDirectory(string directoryPath)
        {
            List<string> fileList = new List<string>();

            // Enumerate files in the current directory and add them to the list
            fileList.AddRange(Directory.GetFiles(directoryPath));

            // Recursively traverse the subdirectories and add their files to the list
            foreach (string subdirectory in Directory.GetDirectories(directoryPath))
            {
                fileList.AddRange(GetAllFilesInDirectory(subdirectory));
            }

            return fileList;
        }
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

                    if (!File.Exists(XVPathDialog.SelectedPath + @"/steam_api_real.dll"))
                    {
                        var myAssembly = Assembly.GetExecutingAssembly();
                        var myStream = myAssembly.GetManifestResourceStream("XVModManager.XVPatcher.zip");
                        ZipArchive archive = new ZipArchive(myStream);
                        archive.ExtractToDirectory(XVPathDialog.SelectedPath, true);
                    }
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

                if (!Directory.Exists(datapath))
                {
                    MessageBox.Show("Data folder not found, please clear your installation", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (!File.Exists(Settings.Default.XVPath + @"/steam_api_real.dll"))
                {
                    MessageBox.Show("XVPatcher or one of it's components is missing, please clear your installation", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

            clbMods.Items.Clear();
            foreach (string item in Settings.Default.Modlist)
            {
                clbMods.Items.Add(item);
            }

            if (!string.IsNullOrEmpty(Settings.Default.CheckedItems))
            {
                Settings.Default.CheckedItems.Split(',')
                    .ToList()
                    .ForEach(item =>
                    {
                        var index = this.clbMods.Items.IndexOf(item);
                        this.clbMods.SetItemChecked(index, true);
                    });
            }
        }

        private void installmod(string mod)
        {
            Clean();

            // Extract the mod files to the temporary path
            ZipFile.ExtractToDirectory(mod, tempPath, true);

            string xmlFile = tempPath + @"/modinfo.xml";

            if (!File.Exists(xmlFile))
            {
                MessageBox.Show("Invalid mod file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Read the mod name and author from the modinfo.xml file
            var modName = File.ReadAllLines(xmlFile).First().ToString();
            var modAuthor = File.ReadAllLines(xmlFile).First().ToString();
            File.Delete(xmlFile);

            if (clbMods.Items.Contains(modName))
            {
                MessageBox.Show("Mod \"" + modName + "\" is already installed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<string> fileSystemEntries = GetAllFilesInDirectory(tempPath);
            string ModFilesFile = datapath + @"/installed/" + modName + ".xml";

            if (!Directory.Exists(datapath + @"/installed"))
                Directory.CreateDirectory(datapath + @"/installed");

            File.WriteAllLines(ModFilesFile, fileSystemEntries);


            // Read the file and replace the path
            string fileContentTemp = File.ReadAllText(ModFilesFile);
            string fileContentData = fileContentTemp.Replace(tempPath, datapath);
            var FileContentTempAarray = File.ReadAllLines(ModFilesFile);
            List<string> datafiles = GetAllFilesInDirectory(datapath);

            foreach (string file in FileContentTempAarray)
            {
                if (datafiles.Contains(file.Replace(tempPath, datapath)) || datafiles.Contains(file.Replace(tempPath, datapath) + @".disabled"))
                {
                    if (MessageBox.Show("File " + Path.GetFileName(file) + " already exists, do you want to replace it?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        if (!Directory.Exists(Path.GetDirectoryName(file.Replace(tempPath, datapath))))
                            Directory.CreateDirectory(Path.GetDirectoryName(file.Replace(tempPath, datapath)));
                        if (File.Exists(file.Replace(tempPath, datapath)))
                        {
                            File.Delete(file.Replace(tempPath, datapath));
                        }
                        if (File.Exists(file.Replace(tempPath, datapath) + @".disabled"))
                        {
                            File.Delete(file.Replace(tempPath, datapath) + @".disabled");
                        }
                        File.Move(file, file.Replace(tempPath, datapath), true);
                    }
                }
                else
                {
                    if (!Directory.Exists(Path.GetDirectoryName(file.Replace(tempPath, datapath))))
                        Directory.CreateDirectory(Path.GetDirectoryName(file.Replace(tempPath, datapath)));
                    File.Move(file, file.Replace(tempPath, datapath));
                }
            }

            // Write the modified content back to the file
            File.WriteAllText(ModFilesFile, fileContentData);

            Settings.Default.Modlist.Add(modName);
            Settings.Default.Save();
            clbMods.Items.Add(modName, true);

            MessageBox.Show("Mod Installed Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Clean();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void installModToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog modInstallDialog = new OpenFileDialog();
            modInstallDialog.Title = "Install mod";
            modInstallDialog.Filter = ".xvmod files | *.xvmod";
            modInstallDialog.Multiselect = true;

            if (modInstallDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string mod in modInstallDialog.FileNames)
                {
                    if (File.Exists(mod))
                    {
                        if (MessageBox.Show("Do you want to install " + Path.GetFileName(mod) + "?", "Install mod", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            installmod(mod);
                        }
                    }
                    else
                    {
                        MessageBox.Show(mod + " is an invalid mod file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
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

            var indices = this.clbMods.CheckedItems.Cast<string>().ToArray();

            Settings.Default.CheckedItems = string.Join(",", indices);
            Settings.Default.Save();
        }

        private void uninstallModToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clbMods.SelectedIndex > -1 && MessageBox.Show("Do you really want to uninstall this mod?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string modname = clbMods.SelectedItem.ToString();
                var files = File.ReadAllLines(datapath + @"/installed/" + modname + @".xml");
                foreach (var f in files)
                {
                    if (File.Exists(f))
                        File.Delete(f);

                    if (File.Exists(f + @".disabled"))
                        File.Delete(f + @".disabled");
                }

                File.Delete(datapath + @"/installed/" + modname + @".xml");

                Settings.Default.Modlist.Remove(modname);
                Settings.Default.Save();
                clbMods.Items.Remove(modname);

                DeleteEmptyDirectories(datapath);
                MessageBox.Show("Mod uninstalled successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Clean();
            }
        }
        private void clearInstallationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to clear installation?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                Clean();
                Directory.Delete(datapath, true);

                Settings.Default.Reset();
                Settings.Default.Save();
                MessageBox.Show("Installation cleared successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }
        private void clbMods_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string modname = clbMods.Items[e.Index].ToString();
            var files = File.ReadAllLines(datapath + @"/installed/" + modname + @".xml");

            // Here, we can perform the action when an item is about to be unchecked
            if (e.CurrentValue == CheckState.Checked && e.NewValue == CheckState.Unchecked)
            {
                foreach (var f in files)
                {
                    if (File.Exists(f))
                        File.Move(f, f + ".disabled");
                }
            }
            // Here, we can perform the action when an item is about to be checked
            else if (e.CurrentValue == CheckState.Unchecked && e.NewValue == CheckState.Checked)
            {
                foreach (var f in files)
                {
                    if (File.Exists(f + @".disabled"))
                        File.Move(f + @".disabled", f);
                }
            }

        }
    }
}