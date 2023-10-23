using System.Diagnostics;
using System.IO.Compression;
using System.Net.Security;
using System.Reflection;
using System.Text;
using System.Xml;
using Xenoverse;
using XVModManager.Properties;
using static Xenoverse.CMS;

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
                extractfilefromCPK("data/msg/proper_noun_skill_spa_name_en.msg", br);
                extractfilefromCPK("data/msg/proper_noun_skill_ult_name_en.msg", br);
                extractfilefromCPK("data/msg/proper_noun_skill_esc_name_en.msg", br);

                extractfilefromCPK("data/ui/texture/CHARA01.emb", br);

                var myAssembly = Assembly.GetExecutingAssembly();
                var myStream = myAssembly.GetManifestResourceStream("XVModManager.ZipFile_Blobs.scripts.zip");
                ZipArchive archive = new ZipArchive(myStream);
                archive.ExtractToDirectory(Xenoverse.Xenoverse.data_path);

                var myAssembly2 = Assembly.GetExecutingAssembly();
                var myStream2 = myAssembly2.GetManifestResourceStream("XVModManager.ZipFile_Blobs.iggy_as3_test.zip");
                ZipArchive archive2 = new ZipArchive(myStream2);
                archive2.ExtractToDirectory(Path.Combine(Xenoverse.Xenoverse.data_path + @"\ui\iggy"));

                var myAssembly3 = Assembly.GetExecutingAssembly();
                var myStream3 = myAssembly3.GetManifestResourceStream("XVModManager.ZipFile_Blobs.embpack.zip");
                ZipArchive archive3 = new ZipArchive(myStream3);
                archive3.ExtractToDirectory(Path.Combine(Xenoverse.Xenoverse.data_path + @"\ui\texture"));

                Process p = new Process();
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = "cmd.exe";
                info.CreateNoWindow = true;
                info.WindowStyle = ProcessWindowStyle.Hidden;
                info.RedirectStandardInput = true;
                info.UseShellExecute = false;

                p.StartInfo = info;
                p.Start();

                using (StreamWriter sw = p.StandardInput)
                {
                    if (sw.BaseStream.CanWrite)
                    {
                        sw.WriteLine("cd " + Xenoverse.Xenoverse.data_path + @"\ui\texture");
                        sw.WriteLine(@"embpack.exe CHARA01.emb");
                    }
                }

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
            int AUR_ID = 0;
            int AUR_GLARE = 0;
            string CMS_BCS = "";
            string CMS_EAN = "";
            string CMS_FCE_EAN = "";
            string CMS_CAM_EAN = "";
            string CMS_BAC = "";
            string CMS_BCM = "";
            string CMS_BAI = "";
            string CSO_1 = "";
            string CSO_2 = "";
            string CSO_3 = "";
            string CSO_4 = "";
            string CUS_SUPER_1 = "";
            string CUS_SUPER_2 = "";
            string CUS_SUPER_3 = "";
            string CUS_SUPER_4 = "";
            string CUS_ULTIMATE_1 = "";
            string CUS_ULTIMATE_2 = "";
            string CUS_EVASIVE = "";
            string MSG_CHARACTER_NAME = "";
            string MSG_COSTUME_NAME = "";

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

                                if (reader.Name == "AUR_ID")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        bool parseSuccess = Int32.TryParse(reader.GetAttribute("value").Trim(), out AUR_ID);
                                        if (!parseSuccess)
                                        {
                                            // Gestisci il caso in cui la conversione non riesce, ad esempio, fornisci un valore predefinito o mostra un messaggio di errore.
                                            MessageBox.Show("AUR_GLARE value not recognized", "Error", MessageBoxButtons.OK);
                                            return;
                                        }
                                    }
                                }
                                if (reader.Name == "AUR_GLARE")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        bool parseSuccess = Int32.TryParse(reader.GetAttribute("value").Trim(), out AUR_GLARE);
                                        if (!parseSuccess)
                                        {
                                            // Gestisci il caso in cui la conversione non riesce, ad esempio, fornisci un valore predefinito o mostra un messaggio di errore.
                                            MessageBox.Show("AUR_GLARE value not recognized", "Error", MessageBoxButtons.OK);
                                            return;
                                        }
                                    }
                                }
                                if (reader.Name == "CMS_BCS")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CMS_BCS = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CMS_EAN")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CMS_EAN = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CMS_FCE_EAN")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CMS_FCE_EAN = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CMS_CAM_EAN")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CMS_CAM_EAN = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CMS_BAC")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CMS_BAC = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CMS_BCM")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CMS_BCM = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CMS_BAI")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CMS_BAI = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CSO_1")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CSO_1 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CSO_2")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CSO_2 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CSO_3")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CSO_3 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CSO_4")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CSO_4 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CUS_SUPER_1")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CUS_SUPER_1 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CUS_SUPER_2")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CUS_SUPER_2 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CUS_SUPER_3")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CUS_SUPER_3 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CUS_SUPER_4")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CUS_SUPER_4 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CUS_ULTIMATE_1")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CUS_ULTIMATE_1 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CUS_ULTIMATE_2")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CUS_ULTIMATE_2 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CUS_EVASIVE")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CUS_EVASIVE = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "MSG_CHARACTER_NAME")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        MSG_CHARACTER_NAME = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "MSG_COSTUME_NAME")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        MSG_COSTUME_NAME = reader.GetAttribute("value").Trim();
                                    }
                                }

                            }
                        }

                    }
                    if (modtype == "REPLACER")
                    {

                        MergeDirectoriesWithConfirmation(Xenoverse.Xenoverse.temp_path, Xenoverse.Xenoverse.data_path);

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
                    
                    else if (modtype == "ADDED_CHARACTER")
                    {
                        int CharID = 108 + Properties.Settings.Default.modlist.Count;
                        MergeDirectoriesWithConfirmation(Xenoverse.Xenoverse.temp_path, Xenoverse.Xenoverse.data_path);

                        Clean();
                        listBox1.Items.Add(modname);
                        foreach (string item in listBox1.Items)
                        {
                            Properties.Settings.Default.modlist.Add(item);
                            Properties.Settings.Default.Save();
                        }

                        // Carica i dati CMS dal file binario
                        CMS cms = new CMS();
                        cms.Load(Xenoverse.Xenoverse.CMSFile);

                        // Crea un nuovo personaggio
                        CharacterData newCharacter = new CharacterData
                        {
                            ID = CharID, // ID del personaggio
                            ShortName = CMS_BCS, // Nome abbreviato del personaggio
                            Unknown = new byte[8], // Array di byte sconosciuto
                            Paths = new string[7] // Array di percorsi
                        };

                        // Inizializza gli elementi dell'array dei percorsi
                        newCharacter.Paths[0] = CMS_BCS;
                        newCharacter.Paths[1] = CMS_EAN;
                        newCharacter.Paths[2] = CMS_FCE_EAN;
                        newCharacter.Paths[3] = CMS_CAM_EAN;
                        newCharacter.Paths[4] = CMS_BAC;
                        newCharacter.Paths[5] = CMS_BCM;
                        newCharacter.Paths[6] = CMS_BAI;

                        // Aggiungi il nuovo personaggio ai dati CMS
                        cms.AddCharacter(newCharacter);

                        // Crea un'istanza della classe CSO
                        CSO cso = new CSO();
                        cso.Load(Xenoverse.Xenoverse.CSOFile);

                        // Supponiamo di avere un oggetto characterData con i dati da aggiungere
                        CSO_Data characterData = new CSO_Data
                        {
                            Char_ID = CharID,           // Sostituisci con l'ID del personaggio desiderato
                            Costume_ID = 0,      // Sostituisci con l'ID del costume desiderato
                            Paths = new string[4]  // Aggiungi i percorsi desiderati
                            {
                                    CSO_1,
                                    CSO_2,
                                    CSO_3,
                                    CSO_4
                            }
                        };

                        // Chiama la funzione AddCharacter per aggiungere i dati del personaggio
                        cso.AddCharacter(characterData);

                        CharSkill charSkill = new CharSkill();
                        charSkill.populateSkillData(Xenoverse.Xenoverse.data_path + @"/msg", Xenoverse.Xenoverse.CUSFile, "en"); //Leave it to "en" rn, we'll change it later

                        // Crea un nuovo personaggio
                        Char_Data newCharacterCUS = new Char_Data
                        {
                            charID = CharID, // ID del personaggio
                            CostumeID = 0, // ID del costume
                            SuperIDs = new short[]
                            {
                                    charSkill.FindSuperByName(CUS_SUPER_1),
                                    charSkill.FindSuperByName(CUS_SUPER_2),
                                    charSkill.FindSuperByName(CUS_SUPER_3),
                                    charSkill.FindSuperByName(CUS_SUPER_4)
                            }, // Array di ID delle Super mosse
                            UltimateIDs = new short[]
                            {
                                    charSkill.FindUltimateByName(CUS_ULTIMATE_1),
                                    charSkill.FindUltimateByName(CUS_ULTIMATE_2)
                            }, // Array di ID delle mosse Ultimate

                            EvasiveID = charSkill.FindEvasiveByName(CUS_EVASIVE)
                        };

                        // Aggiungi il nuovo personaggio ai dati di CharSkill
                        charSkill.AddCharacter(newCharacterCUS);

                        AUR aur = new AUR(); // Crea un'istanza della classe AUR
                        aur.Load(Xenoverse.Xenoverse.AURFile);

                        string Charalist = Xenoverse.Xenoverse.data_path + @"\scripts\action_script\Charalist.as";

                        var text10 = new StringBuilder();

                        foreach (string s in File.ReadAllLines(Charalist))
                        {
                            text10.AppendLine(s.Replace("[[\"JCO\",0,0,0,[110,111]]]", "[[\"JCO\",0,0,0,[110,111]]],[[\"" + CMS_BCS + "\",0,0,0,[-1,-1]]]"));
                        }

                        using (var file1 = new StreamWriter(File.Create(Charalist)))
                        {
                            file1.Write(text10.ToString());
                        }

                        CompileScripts();

                        msg MSGfile;
                        MSGfile = msgStream.Load(Xenoverse.Xenoverse.proper_noun_character_name);
                        msgData[] expand = new msgData[MSGfile.data.Length + 1];
                        Array.Copy(MSGfile.data, expand, MSGfile.data.Length);
                        string nameid = MSGfile.data[MSGfile.data.Length - 1].NameID;
                        int endid = int.Parse(nameid.Substring(nameid.Length - 3, 3));
                        expand[expand.Length - 1].ID = MSGfile.data.Length;
                        expand[expand.Length - 1].Lines = new string[] { MSG_CHARACTER_NAME };
                        expand[expand.Length - 1].NameID = "chara_" + CMS_BCS + "_" + (endid).ToString("000");

                        MSGfile.data = expand;

                        msgStream.Save(MSGfile, Xenoverse.Xenoverse.proper_noun_character_name);

                        Process p = new Process();
                        ProcessStartInfo info = new ProcessStartInfo();
                        info.FileName = "cmd.exe";
                        info.CreateNoWindow = true;
                        info.WindowStyle = ProcessWindowStyle.Hidden;
                        info.RedirectStandardInput = true;
                        info.UseShellExecute = false;

                        p.StartInfo = info;
                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                sw.WriteLine("cd " + Xenoverse.Xenoverse.data_path + @"\ui\texture");
                                sw.WriteLine(@"embpack.exe CHARA01");
                            }
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
            }
            else
            {
                MessageBox.Show("Mod type not implemented", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
     
        
        private void CompileScripts()
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            Process process = new Process();
            string sourcepath = "\"" + Xenoverse.Xenoverse.data_path + "\\scripts\"";
            string maintimelinepath = "\"" + Xenoverse.Xenoverse.data_path + "\\scripts\\dlc3_CHARASELE_fla\\MainTimeline.as\"";

            processStartInfo.FileName = "cmd.exe";
            processStartInfo.CreateNoWindow = true;
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.UseShellExecute = false;
            process.StartInfo = processStartInfo;
            process.Start();
            using (StreamWriter standardInput = process.StandardInput)
            {
                if (standardInput.BaseStream.CanWrite)
                {
                    standardInput.WriteLine("cd " + Xenoverse.Xenoverse.flex_sdk_path + "\\bin");
                    standardInput.WriteLine("mxmlc -compiler.source-path=" + sourcepath + " " + maintimelinepath);
                }
            }
            process.WaitForExit();
            Directory.CreateDirectory(Xenoverse.Xenoverse.data_path + "\\ui\\iggy\\");

            if (File.Exists(Xenoverse.Xenoverse.data_path + "\\ui\\iggy\\CHARASELE.swf"))
                File.Delete(Xenoverse.Xenoverse.data_path + "\\ui\\iggy\\CHARASELE.swf");


            File.Move(Xenoverse.Xenoverse.data_path + "\\scripts\\dlc3_CHARASELE_fla\\MainTimeline.swf", Xenoverse.Xenoverse.data_path + "\\ui\\iggy\\CHARASELE.swf");

            Thread.Sleep(1000);
            process.Start();
            using (StreamWriter standardInput = process.StandardInput)
            {
                if (standardInput.BaseStream.CanWrite)
                {
                    standardInput.WriteLine("cd " + Xenoverse.Xenoverse.data_path + @"\ui\iggy");
                    standardInput.WriteLine("iggy_as3_test.exe CHARASELE.swf");
                }
            }
            process.WaitForExit();

            Thread.Sleep(1000);

            if (File.Exists(Xenoverse.Xenoverse.data_path + "\\ui\\iggy\\CHARASELE.swf"))
                File.Delete(Xenoverse.Xenoverse.data_path + "\\ui\\iggy\\CHARASELE.swf");
        }

        public static void MergeDirectoriesWithConfirmation(string sourceDir, string destDir)
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

                if (File.Exists(destFile))
                {
                    // Ask for confirmation to replace the existing file.
                    var result = MessageBox.Show($"A file with the name '{fileName}' already exists. Do you want to replace it?", "File Replace Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        File.Copy(sourceFile, destFile, true); // Replace the existing file.
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        return; // Cancel the entire operation.
                    }
                    // If 'No' is chosen, the existing file will not be replaced.
                }
                else
                {
                    File.Copy(sourceFile, destFile);
                }
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
                MergeDirectoriesWithConfirmation(sourceSubDir, destSubDir);
            }
        }

        private void Clean()
        {
            if (Directory.Exists(Xenoverse.Xenoverse.temp_path))
                Directory.Delete(Xenoverse.Xenoverse.temp_path, true); 
            if (Directory.Exists(Xenoverse.Xenoverse.data_path + @"/xvmod.xml"))
                Directory.Delete(Xenoverse.Xenoverse.data_path + @"/xvmod.xml", true);
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

        private void compileScriptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CompileScripts();
        }
    }
}