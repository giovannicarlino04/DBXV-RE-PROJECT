using System.IO.Compression;
using System.Text;
using System.Xml;
using Xenoverse;

namespace XVModManager
{
    public partial class Form1 : Form
    {
        CPK cpk = new CPK();

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
            if (!File.Exists(Xenoverse.Xenoverse.xvpatcher_dll))
            {
                MessageBox.Show("XVPatcher not detected, XVModManager cannot work without it.",
                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            foreach (string mod in Properties.Settings.Default.modlist)
                listBox1.Items.Add(mod);

            if (!Directory.Exists(Xenoverse.Xenoverse.data_path))
            {
                Directory.CreateDirectory(Xenoverse.Xenoverse.data_path);

                BinaryReader br = new BinaryReader(File.OpenRead(Xenoverse.Xenoverse.data2_cpk));
                cpk.ReadCPK(Xenoverse.Xenoverse.data2_cpk);

                extractfilefromCPK("data/ui/iggy/CHARASELE.iggy", br);

                extractfilefromCPK("data/system/aura_setting.aur", br);
                extractfilefromCPK("data/system/chara_sound.cso", br);
                extractfilefromCPK("data/system/char_model_spec.cms", br);
                extractfilefromCPK("data/system/custom_skill.cus", br);
                extractfilefromCPK("data/system/parameter_spec_char.psc", br);

                extractfilefromCPK("data/msg/proper_noun_character_name_en.msg", br);
                extractfilefromCPK("data/msg/proper_noun_costume_name_en.msg", br);


            }



            Clean();

        }

        private void extractfilefromCPK(string extractMe, BinaryReader oldFile)
        {
            List<FileEntry> entries = null;

            entries = (extractMe.ToUpper() == "ALL") ? cpk.FileTable.Where(x => x.FileType == "FILE").ToList() : cpk.FileTable.Where(x => ((x.DirName != null) ? x.DirName + "/" : "") + x.FileName.ToString().ToLower() == extractMe.ToLower()).ToList();

            if (entries.Count == 0)
            {
                MessageBox.Show("Cannot find " + extractMe + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

            for (int i = 0; i < entries.Count; i++)
            {
                if (!String.IsNullOrEmpty((string)entries[i].DirName))
                {
                    Directory.CreateDirectory(entries[i].DirName.ToString());
                }

                oldFile.BaseStream.Seek((long)entries[i].FileOffset, SeekOrigin.Begin);
                string isComp = Encoding.ASCII.GetString(oldFile.ReadBytes(8));
                oldFile.BaseStream.Seek((long)entries[i].FileOffset, SeekOrigin.Begin);

                byte[] chunk = oldFile.ReadBytes(Int32.Parse(entries[i].FileSize.ToString()));
                if (isComp == "CRILAYLA")
                {
                    int size = Int32.Parse((entries[i].ExtractSize ?? entries[i].FileSize).ToString());
                    chunk = cpk.DecompressCRILAYLA(chunk, size);
                }

                
                File.WriteAllBytes(((entries[i].DirName != null) ? entries[i].DirName + "/" : "") + entries[i].FileName.ToString(), chunk);

                //MessageBox.Show("Debug: File extracted!");
            }
        }

        private void installModToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Install Mod";
            ofd.Filter = "Xenoverse Mod Files (*.xvmod)|*.xvmod";
            ofd.Multiselect = true;
            ofd.CheckFileExists = true;

            string modtype = "";
            string modname = "";
            string modauthor = "";
            string modversion = "";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in ofd.FileNames)
                {

                    ZipFile.ExtractToDirectory(file, Xenoverse.Xenoverse.temp_path);

                    string xmlfile = Xenoverse.Xenoverse.temp_path + @"/xvmod.xml";

                    if (!File.Exists(xmlfile))
                        MessageBox.Show("xvmod.xml file not found.",
                        "Invalid mod file", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    string xmlData = File.ReadAllText(xmlfile);

                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlData)))
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                if (reader.Name == "XVMOD")
                                {
                                    if (reader.GetAttribute("type").Length == 0)
                                    {
                                        MessageBox.Show("Invalid xmlreader attribute.",
                                        "Invalid mod file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }

                                    modtype = reader.GetAttribute("type");

                                }

                                if (reader.Name == "XVMOD")
                                {
                                    if (reader.GetAttribute("type").Length == 0)
                                    {
                                        MessageBox.Show("Invalid xmlreader attribute.", "Invalid mod file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    modtype = reader.GetAttribute("type");
                                }
                                if (reader.Name == "MOD_NAME")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        modname = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "MOD_AUTHOR")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        modauthor = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "MOD_VERSION")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        modversion = reader.GetAttribute("value").Trim();
                                    }
                                }

                            }
                        }

                    }
                    if (modtype == "REPLACER")
                    {
                        if (!DoesAnyFileExistInDestination(Xenoverse.Xenoverse.temp_path, Xenoverse.Xenoverse.data_path))
                        {
                            MergeDirectories(Xenoverse.Xenoverse.temp_path, Xenoverse.Xenoverse.data_path);

                            Clean();
                            listBox1.Items.Add(modname);
                            foreach (string item in listBox1.Items)
                            {
                                Properties.Settings.Default.modlist.Add(item);
                                Properties.Settings.Default.Save();
                            }
                            MessageBox.Show("Mod installed successfully", "Success", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("File already exists in data folder, cannot proceed with installation.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (modtype == "ADDED_CHARACTER")
                    {
                        if (!DoesAnyFileExistInDestination(Xenoverse.Xenoverse.temp_path, Xenoverse.Xenoverse.data_path))
                        {
                            MergeDirectories(Xenoverse.Xenoverse.temp_path, Xenoverse.Xenoverse.data_path);

                            Clean();
                            listBox1.Items.Add(modname);
                            foreach (string item in listBox1.Items)
                            {
                                Properties.Settings.Default.modlist.Add(item);
                                Properties.Settings.Default.Save();
                            }
                            MessageBox.Show("Mod installed successfully", "Success", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("File already exists in data folder, cannot proceed with installation.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Mod type not implemented", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public static void MergeDirectories(string sourceDir, string destDir)
        {
            if (!Directory.Exists(sourceDir) || !Directory.Exists(destDir))
            {
                throw new DirectoryNotFoundException("Source or destination directory does not exist.");
            }

            // Get the subdirectories in the source directory.
            string[] sourceSubDirs = Directory.GetDirectories(sourceDir);

            // Copy the files from the source to the destination.
            foreach (string sourceFile in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(sourceFile);
                string destFile = Path.Combine(destDir, fileName);

                // Handle file conflicts here, e.g., by renaming the file.
                if (File.Exists(destFile))
                {
                    string uniqueName = GetUniqueFileName(destDir, fileName);
                    destFile = Path.Combine(destDir, uniqueName);
                }

                File.Copy(sourceFile, destFile);
            }

            // Recursively merge the subdirectories.
            foreach (string sourceSubDir in sourceSubDirs)
            {
                string dirName = Path.GetFileName(sourceSubDir);
                string destSubDir = Path.Combine(destDir, dirName);

                // If the destination subdirectory doesn't exist, create it.
                if (!Directory.Exists(destSubDir))
                {
                    Directory.CreateDirectory(destSubDir);
                }

                // Recursively merge this subdirectory.
                MergeDirectories(sourceSubDir, destSubDir);
            }
        }

        // Helper function to generate a unique file name to handle conflicts.
        private static string GetUniqueFileName(string directory, string fileName)
        {
            string baseName = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            int counter = 1;

            while (File.Exists(Path.Combine(directory, fileName)))
            {
                fileName = $"{baseName}_{counter}{extension}";
                counter++;
            }

            return fileName;
        }

        public static bool DoesAnyFileExistInDestination(string sourceDir, string destDir)
        {
            if (!Directory.Exists(sourceDir) || !Directory.Exists(destDir))
            {
                throw new DirectoryNotFoundException("Source or destination directory does not exist.");
            }

            foreach (string sourceFile in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                string relativePath = sourceFile.Substring(sourceDir.Length);
                string destFile = Path.Combine(destDir, relativePath);

                if (File.Exists(destFile))
                {
                    return true; // At least one file exists in the destination
                }
            }

            // Check for subdirectories in the destination that are not in the source
            foreach (string destSubDir in Directory.GetDirectories(destDir, "*", SearchOption.AllDirectories))
            {
                string relativePath = destSubDir.Substring(destDir.Length);
                string sourceSubDir = Path.Combine(sourceDir, relativePath);

                if (!Directory.Exists(sourceSubDir))
                {
                    return true; // At least one subdirectory exists in the destination but not in the source
                }
            }

            return false; // No files or subdirectories from the source directory exist in the destination
        }
        private void Clean()
        {
            if (Directory.Exists(Xenoverse.Xenoverse.temp_path))
                Directory.Delete(Xenoverse.Xenoverse.temp_path, true);
        }


        private void clearInstallationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear your mod installation?", "Warning",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (Directory.Exists(Xenoverse.Xenoverse.data_path))
                    Directory.Delete(Xenoverse.Xenoverse.data_path, true);
                Properties.Settings.Default.Reset();
                Clean();
                this.Close();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Clean();
        }
    }
}