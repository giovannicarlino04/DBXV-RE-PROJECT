using System;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace XVTool
{
    public partial class Form1 : Form
    {
        static string XVFolderInfo = Application.StartupPath + @"\XVPath.info";
        static string XVPath;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (string item in Properties.Settings.Default.modlist)
            {
                if (item != "Item.null")
                {
                    listBox1.Items.Add(item);
                }
            }

            if (!File.Exists(XVFolderInfo))
            {
                File.Create(XVFolderInfo).Close();
            }
            XVPath = File.ReadAllText(XVFolderInfo);
            if (!Directory.Exists(XVPath))
            {
                SelectXenoverseFolder();
            }
        }

        private void SelectXenoverseFolder()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select Xenoverse Folder";

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                XVPath = fbd.SelectedPath;
                File.WriteAllText(XVFolderInfo, XVPath);
            }
            else
            {
                MessageBox.Show("Xenoverse folder not selected. Exiting application.");
                Application.Exit();
            }
        }

        private void InstallMod(string modFilePath)
        {
            // Ottenere il nome della mod dal percorso del file
            string modFileName = Path.GetFileNameWithoutExtension(modFilePath);

            // Creare la cartella temporanea della mod
            string modTempFolder = Path.Combine(Path.GetTempPath(), modFileName);
            Directory.CreateDirectory(modTempFolder);

            // Estrarre i file della mod nella cartella temporanea
            using (ZipArchive modFile = ZipFile.OpenRead(modFilePath))
            {
                foreach (ZipArchiveEntry entry in modFile.Entries)
                {
                    string entryDestinationPath = Path.Combine(modTempFolder, entry.FullName);
                    if (entry.FullName.EndsWith("/") || entry.FullName.EndsWith("\\"))
                    {
                        Directory.CreateDirectory(entryDestinationPath);
                    }
                    else
                    {
                        entry.ExtractToFile(entryDestinationPath, true);
                    }
                }
            }

            // Ottenere i file della mod dalla cartella temporanea
            string[] fileList = Directory.GetFiles(modTempFolder, "*", SearchOption.AllDirectories);

            // Verificare se i file sono già presenti nella cartella data
            List<string> existingFiles = new List<string>();
            foreach (string filePath in fileList)
            {
                string relativePath = filePath.Replace(modTempFolder, "").TrimStart('\\');
                string dataFilePath = Path.Combine(XVPath, "data", relativePath);
                if (File.Exists(dataFilePath))
                {
                    existingFiles.Add(filePath);
                }
            }

            if (existingFiles.Count > 0)
            {
                string message = $"The following files already exist in the data folder:\n\n{string.Join("\n", existingFiles)}\n\nDo you want to continue and overwrite these files?";
                DialogResult result = MessageBox.Show(message, "Overwrite Files?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    return;
                }
            }

            // Spostare i file nella cartella della mod nella cartella data
            foreach (string filePath in fileList)
            {
                string relativePath = filePath.Replace(modTempFolder, "").TrimStart('\\');
                string dataFilePath = Path.Combine(XVPath, "data", relativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(dataFilePath));
                File.Copy(filePath, dataFilePath, true);
            }

            // Creare il file di elenco della mod
            string modFileListPath = Path.Combine(XVPath, "data", modFileName + ".txt");

            File.WriteAllLines(modFileListPath, fileList.Select(filePath => Path.GetFileName(filePath)));


            // Aggiungere la mod alla lista delle mod installate nel programma
            listBox1.Items.Add(modFileName);
            Properties.Settings.Default.modlist.Add(modFileName);
            Properties.Settings.Default.Save();

            // Eliminare la cartella temporanea della mod
            Directory.Delete(modTempFolder, true);
        }

        private void btnUninstallMod_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string modName = listBox1.SelectedItem.ToString();

                string modFileListPath = Path.Combine(XVPath, "data", modName + ".txt");

                if (File.Exists(modFileListPath))
                {
                    string[] fileList = File.ReadAllLines(modFileListPath);
                    foreach (string fileName in fileList)
                    {
                        string[] filePaths = Directory.GetFiles(XVPath, fileName, SearchOption.AllDirectories);
                        foreach (string filePath in filePaths)
                        {
                            File.Delete(filePath);
                        }
                    }

                    listBox1.Items.Remove(modName);
                    Properties.Settings.Default.modlist.Remove(modName);
                    Properties.Settings.Default.Save();

                    File.Delete(modFileListPath);
                }
            }
            // Rimuovere le sottocartelle vuote nella cartella data
            RemoveEmptyDirectories(Path.Combine(XVPath, "data"));
        }

        private void RemoveEmptyDirectories(string path)
        {
            // Ottenere i percorsi di tutte le sottocartelle della cartella specificata
            string[] directories = Directory.GetDirectories(path);

            foreach (string directory in directories)
            {
                // Rimuovere le sottocartelle vuote all'interno di questa sottocartella
                RemoveEmptyDirectories(directory);

                // Se la sottocartella è vuota, eliminarla
                if (Directory.GetFiles(directory).Length == 0 && Directory.GetDirectories(directory).Length == 0)
                {
                    Directory.Delete(directory, false);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void clearInstallationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear installation?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                Properties.Settings.Default.Reset();
                listBox1.Items.Clear();

                string Datapath = Path.Combine(XVPath, "data");
                Directory.Delete(Datapath, true);
                this.Close();
            }
        }

        private void installModToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Xenoverse 1 Mod Files | *.x1m";
            ofd.Title = "Select XV1 Mod To Install";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                InstallMod(ofd.FileName);
            }
        }
    }
}